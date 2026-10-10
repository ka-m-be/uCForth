\ aarch64.fs - the virtual machine of uCForth in A64 machine code
\
\ The kernel for each target with a 64-bit ARM processor: NEXT and the
\ primitives 0-28 and 32-65 of doc/vm.md, with 32-bit cells. The target
\ adds its HAL: the console (29-31) and 80-99, and the start code.
\
\ Load it after asm/aarch64.fs, with ALSO AARCH64, in the vocabulary of
\ the build (targets/.../build.fs), after START-CODE. The labels go into
\ that vocabulary. The build needs a host with 32-bit cells: then the
\ constants of the memory map (S0, R0, STATE ...) are the same for host
\ and target.
\
\ Registers:
\   X28 MEM   the address of VM address 0; VM address a is at MEM + a
\   W19 IP    W20 DSP (data stack)   W21 RSP (return stack)   W22 WP (W)
\   X27 TAB   the table of the primitives
\   X26 SCR   a scratch area for the HAL (the target sets it)
\ IP, DSP, RSP and WP hold VM addresses, 32 bits. A load is
\ "ldr w0, [x28, w1, uxtw]": the processor adds the VM address to MEM.
\ W0-W13 are free in each primitive. A call of the system keeps all
\ registers but X0.

: NEED-32 ( -- )   CELL 4 <> ABORT" the kernel needs a host with 32-bit cells" ;
NEED-32

X28 CONSTANT MEM   W19 CONSTANT IP   W20 CONSTANT DSP   W21 CONSTANT RSP
W22 CONSTANT WP    X27 CONSTANT TAB   X26 CONSTANT SCR

\ ==== Macros ============================================================

: PUSH, ( wt -- )   DSP DSP 4 SUBI,  MEM DSP STR-UXTW, ;
: POP,  ( wt -- )   MEM DSP LDR-UXTW,  DSP DSP 4 ADDI, ;
: TOS@, ( wt -- )   MEM DSP LDR-UXTW, ;
: TOS!, ( wt -- )   MEM DSP STR-UXTW, ;
: NOS@, ( wt -- )   W9 DSP 4 ADDI,  MEM W9 LDR-UXTW, ;   \ the second cell
: NOS!, ( wt -- )   W9 DSP 4 ADDI,  MEM W9 STR-UXTW, ;
: RPUSH, ( wt -- )  RSP RSP 4 SUBI,  MEM RSP STR-UXTW, ;
: RPOP,  ( wt -- )  MEM RSP LDR-UXTW,  RSP RSP 4 ADDI, ;
: UPPER, ( wt -- )   \ a-z to A-Z; uses W10 W11
  >R  W10 R@ 97 SUBI,  W10 26 CMPI,  W11 R@ 32 SUBI,  R@ W11 R> LO CSEL, ;

FORWARD UNKNOWN-OP   \ its place comes below, before the first jump to it
: DISPATCH, ( -- )   \ run the code field of WP
  W0 MEM WP LDR-UXTW,  W0 127 CMPI,  UNKNOWN-OP HI B.COND,
  W1 TAB X0 LDR-LSL2,  X1 TAB X1 ADD,  X1 BR, ;   \ the table holds offsets from TAB
: NEXT, ( -- )   \ the inner interpreter (doc/vm.md, section 5)
  WP MEM IP LDR-UXTW,  IP IP 4 ADDI,  DISPATCH, ;

\ ==== The table and the start values ====================================
\ OPTAB holds the place of the code of each primitive as an offset from
\ OPTAB, 32 bits each. So the code can lie at any address (macOS loads a
\ program at an address above 4 GB that changes with each start).
\ PRIM n notes THERE as the code of primitive n; FILL-TABLE writes the
\ table at the end. The build puts the xts of the image into XT-BOOT,
\ XT-QUIT and XT-THROW.

CREATE OPS 128 CELLS ALLOT   OPS 128 CELLS 0 FILL
: PRIM ( n -- )   CELLS OPS +  THERE SWAP ! ;
: RESERVE ( n -- )   0 ?DO 0 A, LOOP ;

LABEL OPTAB     128 RESERVE
LABEL XT-BOOT   0 A,
LABEL XT-QUIT   0 A,
LABEL XT-THROW  0 A,

\ An unknown primitive is THROW -21. THROW-CODE runs THROW of the image
\ with the code in W0. DISPATCH runs the code field of WP. Each part goes
\ on into the next one.
LABEL UNKNOWN-OP   W0 -21 MOVI,
LABEL THROW-CODE   W0 PUSH,  WP XT-THROW LDR-LIT,
LABEL DISPATCH     DISPATCH,

: FILL-TABLE ( -- )   \ each primitive without code runs UNKNOWN-OP
  128 0 DO
    OPS I CELLS + @ ?DUP 0= IF UNKNOWN-OP @ THEN  OPTAB @ -  OPTAB @ I CELLS + A!
  LOOP ;


\ ==== Primitives 0-28 ===================================================

0 PRIM ( DOCOL )    IP RPUSH,  IP WP 4 ADDI,  NEXT,
1 PRIM ( DOVAR )    W0 WP 8 ADDI,  W0 PUSH,  NEXT,
2 PRIM ( DOCON )    W1 WP 4 ADDI,  W0 MEM W1 LDR-UXTW,  W0 PUSH,  NEXT,
3 PRIM ( DODOES )   W0 WP 8 ADDI,  W0 PUSH,  IP RPUSH,
                    W1 WP 4 ADDI,  IP MEM W1 LDR-UXTW,  NEXT,
4 PRIM ( EXIT )     IP RPOP,  NEXT,
5 PRIM ( LIT )      W0 MEM IP LDR-UXTW,  IP IP 4 ADDI,  W0 PUSH,  NEXT,
6 PRIM ( BRANCH )   IP MEM IP LDR-UXTW,  NEXT,
7 PRIM ( ?BRANCH )  W0 POP,  W0 0=IF,  IP MEM IP LDR-UXTW,  ELSE,  IP IP 4 ADDI,  THEN,  NEXT,
8 PRIM ( EXECUTE )  WP POP,  DISPATCH B,
9 PRIM ( @ )        W1 TOS@,  W0 MEM W1 LDR-UXTW,  W0 TOS!,  NEXT,
10 PRIM ( ! )       W1 POP,  W0 POP,  W0 MEM W1 STR-UXTW,  NEXT,
11 PRIM ( C@ )      W1 TOS@,  W0 MEM W1 LDRB-UXTW,  W0 TOS!,  NEXT,
12 PRIM ( C! )      W1 POP,  W0 POP,  W0 MEM W1 STRB-UXTW,  NEXT,
13 PRIM ( DUP )     W0 TOS@,  W0 PUSH,  NEXT,
14 PRIM ( DROP )    DSP DSP 4 ADDI,  NEXT,
15 PRIM ( SWAP )    W0 TOS@,  W1 NOS@,  W1 TOS!,  W0 NOS!,  NEXT,
16 PRIM ( OVER )    W1 NOS@,  W1 PUSH,  NEXT,
17 PRIM ( >R )      W0 POP,  W0 RPUSH,  NEXT,
18 PRIM ( R> )      W0 RPOP,  W0 PUSH,  NEXT,
19 PRIM ( R@ )      W0 MEM RSP LDR-UXTW,  W0 PUSH,  NEXT,
20 PRIM ( SP@ )     W0 DSP MOV,  W0 PUSH,  NEXT,
21 PRIM ( SP! )     DSP MEM DSP LDR-UXTW,  NEXT,
22 PRIM ( RP@ )     W0 RSP MOV,  W0 PUSH,  NEXT,
23 PRIM ( RP! )     RSP POP,  NEXT,
24 PRIM ( AND )     W0 POP,  W1 TOS@,  W1 W1 W0 AND,  W1 TOS!,  NEXT,
25 PRIM ( OR )      W0 POP,  W1 TOS@,  W1 W1 W0 ORR,  W1 TOS!,  NEXT,
26 PRIM ( XOR )     W0 POP,  W1 TOS@,  W1 W1 W0 EOR,  W1 TOS!,  NEXT,
27 PRIM ( 0< )      W0 TOS@,  W0 W0 31 ASRI,  W0 TOS!,  NEXT,
28 PRIM ( UM+ )     W0 TOS@,  W1 NOS@,  W1 W1 W0 ADDS,  W0 HS CSET,
                    W1 NOS!,  W0 TOS!,  NEXT,

\ ==== Primitives 32-65 ==================================================

32 PRIM ( + )       W0 POP,  W1 TOS@,  W1 W1 W0 ADD,  W1 TOS!,  NEXT,
33 PRIM ( - )       W0 POP,  W1 TOS@,  W1 W1 W0 SUB,  W1 TOS!,  NEXT,
34 PRIM ( 1+ )      W0 TOS@,  W0 W0 1 ADDI,  W0 TOS!,  NEXT,
35 PRIM ( 1- )      W0 TOS@,  W0 W0 1 SUBI,  W0 TOS!,  NEXT,
36 PRIM ( NEGATE )  W0 TOS@,  W0 W0 NEG,  W0 TOS!,  NEXT,
37 PRIM ( INVERT )  W0 TOS@,  W0 W0 MVN,  W0 TOS!,  NEXT,
38 PRIM ( 2* )      W0 TOS@,  W0 W0 1 LSLI,  W0 TOS!,  NEXT,
39 PRIM ( 2/ )      W0 TOS@,  W0 W0 1 ASRI,  W0 TOS!,  NEXT,
40 PRIM ( LSHIFT )  W1 POP,  W0 TOS@,  W1 32 CMPI,
                    HS IF,  W0 WZR MOV,  ELSE,  W0 W0 W1 LSLV,  THEN,  W0 TOS!,  NEXT,
41 PRIM ( RSHIFT )  W1 POP,  W0 TOS@,  W1 32 CMPI,
                    HS IF,  W0 WZR MOV,  ELSE,  W0 W0 W1 LSRV,  THEN,  W0 TOS!,  NEXT,
42 PRIM ( = )       W0 POP,  W1 TOS@,  W1 W0 CMP,  W1 EQ CSETM,  W1 TOS!,  NEXT,
43 PRIM ( 0= )      W0 TOS@,  W0 0 CMPI,  W0 EQ CSETM,  W0 TOS!,  NEXT,
44 PRIM ( < )       W0 POP,  W1 TOS@,  W1 W0 CMP,  W1 LT CSETM,  W1 TOS!,  NEXT,
45 PRIM ( U< )      W0 POP,  W1 TOS@,  W1 W0 CMP,  W1 LO CSETM,  W1 TOS!,  NEXT,
46 PRIM ( * )       W0 POP,  W1 TOS@,  W1 W1 W0 MUL,  W1 TOS!,  NEXT,
47 PRIM ( UM* )     W0 TOS@,  W1 NOS@,  X2 W1 W0 UMULL,  X3 X2 32 LSRI,
                    W2 NOS!,  W3 TOS!,  NEXT,
48 PRIM ( UM/MOD )  \ ( lo hi u -- rem quot ): 64 bits divided by 32 bits
  W2 POP,  W1 TOS@,  W0 NOS@,
  W2 0=IF,  W0 -10 MOVI,  THROW-CODE B,  THEN,
  W1 W2 CMP,  HS IF,  W0 -11 MOVI,  THROW-CODE B,  THEN,
  X1 X1 32 LSLI,  X0 X0 X1 ORR,  X3 X0 X2 UDIV,  X4 X3 X2 X0 MSUB,
  W4 NOS!,  W3 TOS!,  NEXT,
49 PRIM ( ROT )     W0 TOS@,  W1 NOS@,  W10 DSP 8 ADDI,  W2 MEM W10 LDR-UXTW,
                    W2 TOS!,  W0 NOS!,  W1 MEM W10 STR-UXTW,  NEXT,
50 PRIM ( NIP )     W0 POP,  W0 TOS!,  NEXT,
51 PRIM ( ?DUP )    W0 TOS@,  W0 0<>IF,  W0 PUSH,  THEN,  NEXT,
52 PRIM ( 2DUP )    W0 TOS@,  W1 NOS@,  W1 PUSH,  W0 PUSH,  NEXT,
53 PRIM ( 2DROP )   DSP DSP 8 ADDI,  NEXT,
54 PRIM ( +! )      W1 POP,  W0 POP,  W2 MEM W1 LDR-UXTW,  W2 W2 W0 ADD,
                    W2 MEM W1 STR-UXTW,  NEXT,
55 PRIM ( MOVE )    \ ( a1 a2 u -- ), also for areas that overlap
  W2 POP,  W1 POP,  W0 POP,
  W2 0<>IF,
    W1 W0 CMP,  HI IF,                   \ a2 > a1: from the end
      BEGIN,  W2 W2 1 SUBI,
        W10 W0 W2 ADD,  W3 MEM W10 LDRB-UXTW,  W10 W1 W2 ADD,  W3 MEM W10 STRB-UXTW,
      W2 0 CMPI,  EQ UNTIL,
    ELSE,                                \ else from the start
      W4 WZR MOV,
      BEGIN,
        W10 W0 W4 ADD,  W3 MEM W10 LDRB-UXTW,  W10 W1 W4 ADD,  W3 MEM W10 STRB-UXTW,
      W4 W4 1 ADDI,  W4 W2 CMP,  EQ UNTIL,
    THEN,
  THEN,  NEXT,
56 PRIM ( FILL )    \ ( a u c -- )
  W3 POP,  W2 POP,  W0 POP,
  W2 0<>IF,
    W4 WZR MOV,
    BEGIN,  W10 W0 W4 ADD,  W3 MEM W10 STRB-UXTW,  W4 W4 1 ADDI,  W4 W2 CMP,  EQ UNTIL,
  THEN,  NEXT,
57 PRIM ( COUNT )   W0 TOS@,  W1 MEM W0 LDRB-UXTW,  W0 W0 1 ADDI,  W0 TOS!,  W1 PUSH,  NEXT,

\ FIND-NAME ( a u -- nt | 0 ): the search order from the top, each
\ wordlist from the newest word (doc/vm.md, section 6).
\ W1 u, W2 a, W3 wordlists left, W4 nt, W6 j, W12 the result, W13 CASE-SENSE
FORWARD FN-WL   FORWARD FN-NT   FORWARD FN-CHAR   FORWARD FN-NEXT   FORWARD FN-DONE
58 PRIM ( FIND-NAME )
  W1 POP,  W2 TOS@,  W12 WZR MOV,
  W1 FN-DONE CBZ,  W1 32 CMPI,  FN-DONE HS B.COND,
  W10 CASE-SENSE MOVI,  W13 MEM W10 LDR-UXTW,
  W10 #ORDER MOVI,  W3 MEM W10 LDR-UXTW,
LABEL FN-WL
  W3 FN-DONE CBZ,  W3 W3 1 SUBI,
  W10 W3 2 LSLI,  W10 W10 CONTEXT ADDI,  W4 MEM W10 LDR-UXTW,  W4 MEM W4 LDR-UXTW,
LABEL FN-NT
  W4 FN-WL CBZ,
  W10 W4 4 ADDI,  W5 MEM W10 LDRB-UXTW,  W10 31 MOVI,  W5 W5 W10 AND,
  W5 W1 CMP,  FN-NEXT NE B.COND,
  W6 WZR MOV,
LABEL FN-CHAR
  W10 W4 5 ADDI,  W10 W10 W6 ADD,  W7 MEM W10 LDRB-UXTW,
  W10 W2 W6 ADD,  W8 MEM W10 LDRB-UXTW,
  W13 0=IF,  W7 UPPER,  W8 UPPER,  THEN,
  W7 W8 CMP,  FN-NEXT NE B.COND,
  W6 W6 1 ADDI,  W6 W1 CMP,  FN-CHAR NE B.COND,
  W12 W4 MOV,  FN-DONE B,
LABEL FN-NEXT
  W4 MEM W4 LDR-UXTW,  FN-NT B,
LABEL FN-DONE
  W12 TOS!,  NEXT,

\ The loops: three cells on the return stack, the exit address (deepest),
\ the limit and the index (top).
59 PRIM ( DO )      W0 POP,  W1 POP,  W2 MEM IP LDR-UXTW,  IP IP 4 ADDI,
                    W2 RPUSH,  W1 RPUSH,  W0 RPUSH,  NEXT,
60 PRIM ( ?DO )     W0 POP,  W1 POP,  W0 W1 CMP,
  EQ IF,  IP MEM IP LDR-UXTW,
  ELSE,  W2 MEM IP LDR-UXTW,  IP IP 4 ADDI,  W2 RPUSH,  W1 RPUSH,  W0 RPUSH,  THEN,  NEXT,
61 PRIM ( LOOP )    W0 MEM RSP LDR-UXTW,  W0 W0 1 ADDI,  W10 RSP 4 ADDI,  W1 MEM W10 LDR-UXTW,
  W0 W1 CMP,
  EQ IF,  RSP RSP 12 ADDI,  IP IP 4 ADDI,
  ELSE,  W0 MEM RSP STR-UXTW,  IP MEM IP LDR-UXTW,  THEN,  NEXT,
62 PRIM ( +LOOP )   \ the end when index - limit changes its sign, and n has the other sign
  W3 POP,  W0 MEM RSP LDR-UXTW,  W10 RSP 4 ADDI,  W1 MEM W10 LDR-UXTW,
  W4 W0 W1 SUB,  W5 W4 W3 ADD,  W0 W0 W3 ADD,  W0 MEM RSP STR-UXTW,
  W6 W4 W5 EOR,  W7 W4 W3 EOR,  W6 W6 W7 AND,  W6 0 CMPI,
  MI IF,  RSP RSP 12 ADDI,  IP IP 4 ADDI,
  ELSE,  IP MEM IP LDR-UXTW,  THEN,  NEXT,
63 PRIM ( I )       W0 MEM RSP LDR-UXTW,  W0 PUSH,  NEXT,
64 PRIM ( J )       W10 RSP 12 ADDI,  W0 MEM W10 LDR-UXTW,  W0 PUSH,  NEXT,
65 PRIM ( UNLOOP )  RSP RSP 12 ADDI,  NEXT,
