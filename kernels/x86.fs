\ x86.fs - the virtual machine of uCForth in x86 machine code (32 bit)
\
\ The kernel for each target with a 386 or later in 32-bit mode
\ (Windows, Linux i386): NEXT and the primitives 0-28 and 32-65 of
\ doc/vm.md, with 32-bit cells. The target adds its HAL: the console
\ (29-31), 80-99 (Windows: also 100 and 101), BREAK and the start
\ (targets/win32/hal.fs, targets/linux-i386/hal.fs).
\
\ Load it after asm/x86.fs, with ALSO X86, in the vocabulary of the
\ build, after ORG. The build gives three constants first: MEM, the
\ address of VM address 0; VM-END, the first VM address after the
\ memory that the VM may use; KCELLS, the address of 4 cells that the
\ program can write (the break and FIND-NAME use them). The build needs
\ a host with 32-bit cells: then the constants of the memory map (S0,
\ R0, STATE ...) are the same for host and target.
\
\ Memory: VM address a is at MEM + a. The program lies at a fixed
\ address (a program for Windows without relocations, an ELF file for
\ Linux), so each access is one instruction: "mov eax, [ebx+MEM]" is
\ EAX MEM [EBX] MOV,.
\
\ Registers:
\   ESI  IP: the next cell of the running word (a VM address)
\   EBX  W: the xt of the word that runs now
\   EDI  DSP: the data stack pointer; it points to the top cell
\   EBP  RSP: the return stack pointer
\   ESP  the stack of the processor, for the calls of the system
\   EAX ECX EDX are free in each primitive, and EBX (NEXT loads it
\   again). A call of the system keeps ESI EDI EBP, and so the VM:
\   Windows (stdcall) keeps EBX too; Linux (int 0x80) keeps all
\   registers but EAX, and takes its arguments in EBX ECX EDX.

: NEED-32 ( -- )   CELL 4 <> ABORT" the kernel needs a host with 32-bit cells" ;
NEED-32

KCELLS      CONSTANT BREAK-OFF   \ 0, or 128 after a break (DISPATCH)
KCELLS 4 +  CONSTANT FN-A        \ FIND-NAME: the name it looks for,
KCELLS 8 +  CONSTANT FN-U        \ its length,
KCELLS 12 + CONSTANT FN-WL       \ the wordlists left

\ ==== Macros ============================================================

: TOS ( -- mem )  MEM [EDI] ;                    \ the top cell of the data stack
: NOS ( -- mem )  MEM 4 + [EDI] ;                \ the second cell
: DPUSH, ( reg -- )   EDI 4 SUBI,  TOS SWAP MOV, ;
: DPOP, ( reg -- )    TOS MOV,  EDI 4 ADDI, ;
: RPUSH, ( reg -- )   EBP 4 SUBI,  MEM [EBP] SWAP MOV, ;
: RPOP, ( reg -- )    MEM [EBP] MOV,  EBP 4 ADDI, ;
: UPPER, ( reg8 -- )   \ a to z become A to Z
  >R  R@ 'a' CMPI,  AE IF,  R@ 'z' CMPI,  BE IF,  R@ $20 SUBI,  THEN,  THEN,  R> DROP ;

\ ==== The table and the cells for the start ============================
\ OPTAB holds the address of the code of each primitive: 128 cells, then
\ 128 more for the break, each the address of BREAK (the HAL). PRIM n
\ notes THERE as the code of primitive n; FILL-TABLE writes the table at
\ the end. The build puts the xts of the image into XT-BOOT, XT-QUIT and
\ XT-THROW.

CREATE OPS 128 CELLS ALLOT   OPS 128 CELLS 0 FILL   \ the code of each primitive, in the host
: PRIM ( n -- )   NEW-SCOPE  CELLS OPS +  THERE SWAP ! ;
FORWARD BREAK                                     \ the HAL gives its place

4 TALIGN
LABEL OPTAB     256 CELLS TDP +!                  \ filled at the end (FILL-TABLE)
LABEL XT-BOOT   0 T,
LABEL XT-QUIT   0 T,
LABEL XT-THROW  0 T,

\ NEXT runs the word at IP. DISPATCH runs the word in W: its code field
\ holds the number of a primitive, 0 to 127; a greater number is THROW
\ -21. The cell BREAK-OFF is 0, or 128 after a break (Ctrl-C): then each
\ primitive is BREAK, at a place where the VM is complete (doc/vm.md,
\ section 9). So NEXT needs no test for the break.
FORWARD UNKNOWN-OP
: DISPATCH, ( -- )
  EAX MEM [EBX] MOV,  EAX 127 CMPI,  UNKNOWN-OP JA,
  EAX BREAK-OFF [] ADD,  OPTAB [] [EAX*4] JMP-IND, ;
: NEXT, ( -- )   \ the inner interpreter (doc/vm.md, section 5)
  EBX MEM [ESI] MOV,  ESI 4 ADDI,  DISPATCH, ;

\ An unknown primitive is THROW -21. THROW-CODE runs THROW of the image
\ with the code in EAX. Each part goes on into the next one.
LABEL UNKNOWN-OP   EAX -21 MOVI,
LABEL THROW-CODE   EAX DPUSH,  EBX XT-THROW [] MOV,
LABEL DISPATCH     DISPATCH,

: FILL-TABLE ( -- )   \ each primitive without code runs UNKNOWN-OP
  BREAK UNKNOWN = ABORT" the HAL has no BREAK"
  128 0 DO
    OPS I CELLS + @  ?DUP 0= IF UNKNOWN-OP THEN  OPTAB I CELLS + T!
    BREAK  OPTAB I 128 + CELLS + T!
  LOOP ;

\ ==== Primitives 0-28 ===================================================

0 PRIM ( DOCOL )    ESI RPUSH,  ESI 4 [EBX] LEA,  NEXT,
1 PRIM ( DOVAR )    EAX 8 [EBX] LEA,  EAX DPUSH,  NEXT,
2 PRIM ( DOCON )    EAX MEM 4 + [EBX] MOV,  EAX DPUSH,  NEXT,
3 PRIM ( DODOES )   EAX 8 [EBX] LEA,  EAX DPUSH,  ESI RPUSH,  ESI MEM 4 + [EBX] MOV,  NEXT,
4 PRIM ( EXIT )     ESI RPOP,  NEXT,
5 PRIM ( LIT )      EAX MEM [ESI] MOV,  ESI 4 ADDI,  EAX DPUSH,  NEXT,
6 PRIM ( BRANCH )   ESI MEM [ESI] MOV,  NEXT,
7 PRIM ( ?BRANCH )  EAX DPOP,  EAX EAX TEST,  Z IF,  ESI MEM [ESI] MOV,  NEXT,  THEN,
                    ESI 4 ADDI,  NEXT,
8 PRIM ( EXECUTE )  EBX DPOP,  DISPATCH JMP,
9 PRIM ( @ )        EAX TOS MOV,  EAX MEM [EAX] MOV,  TOS EAX MOV,  NEXT,
10 PRIM ( ! )       EAX DPOP,  ECX DPOP,  MEM [EAX] ECX MOV,  NEXT,
11 PRIM ( C@ )      EAX TOS MOV,  EAX MEM [EAX] BYTE MOVZX,  TOS EAX MOV,  NEXT,
12 PRIM ( C! )      EAX DPOP,  ECX DPOP,  MEM [EAX] CL MOV,  NEXT,
13 PRIM ( DUP )     EAX TOS MOV,  EAX DPUSH,  NEXT,
14 PRIM ( DROP )    EDI 4 ADDI,  NEXT,
15 PRIM ( SWAP )    EAX TOS MOV,  ECX NOS MOV,  TOS ECX MOV,  NOS EAX MOV,  NEXT,
16 PRIM ( OVER )    EAX NOS MOV,  EAX DPUSH,  NEXT,
17 PRIM ( >R )      EAX DPOP,  EAX RPUSH,  NEXT,
18 PRIM ( R> )      EAX RPOP,  EAX DPUSH,  NEXT,
19 PRIM ( R@ )      EAX MEM [EBP] MOV,  EAX DPUSH,  NEXT,
20 PRIM ( SP@ )     EAX EDI MOV,  EAX DPUSH,  NEXT,          \ DSP before the push
21 PRIM ( SP! )     EDI TOS MOV,  NEXT,
22 PRIM ( RP@ )     EBP DPUSH,  NEXT,
23 PRIM ( RP! )     EBP DPOP,  NEXT,
24 PRIM ( AND )     EAX DPOP,  TOS EAX AND,  NEXT,
25 PRIM ( OR )      EAX DPOP,  TOS EAX OR,  NEXT,
26 PRIM ( XOR )     EAX DPOP,  TOS EAX XOR,  NEXT,
27 PRIM ( 0< )      TOS 31 SAR,  NEXT,                        \ the sign in each bit
28 PRIM ( UM+ )     EAX DPOP,  TOS EAX ADD,                   \ the carry as 0 or 1
                    EAX 0 MOVI,  EAX 0 ADCI,  EAX DPUSH,  NEXT,

\ ==== Primitives 32-65 ==================================================
\ A flag comes from the carry: "SBB EAX,EAX" gives -1 when the carry is
\ set, else 0. "CMP EAX,1" sets the carry only when EAX is 0.

32 PRIM ( + )       EAX DPOP,  TOS EAX ADD,  NEXT,
33 PRIM ( - )       EAX DPOP,  TOS EAX SUB,  NEXT,
34 PRIM ( 1+ )      TOS INC,  NEXT,
35 PRIM ( 1- )      TOS DEC,  NEXT,
36 PRIM ( NEGATE )  TOS NEG,  NEXT,
37 PRIM ( INVERT )  TOS NOT,  NEXT,
38 PRIM ( 2* )      TOS 1 SHL,  NEXT,
39 PRIM ( 2/ )      TOS 1 SAR,  NEXT,
40 PRIM ( LSHIFT )  ECX DPOP,  ECX 32 CMPI,
                    AE IF,  TOS 0 MOVI,  ELSE,  TOS CL SHL,  THEN,  NEXT,
41 PRIM ( RSHIFT )  ECX DPOP,  ECX 32 CMPI,
                    AE IF,  TOS 0 MOVI,  ELSE,  TOS CL SHR,  THEN,  NEXT,
42 PRIM ( = )       EAX DPOP,  ECX TOS MOV,  ECX EAX SUB,  ECX 1 CMPI,  ECX ECX SBB,
                    TOS ECX MOV,  NEXT,
43 PRIM ( 0= )      EAX TOS MOV,  EAX 1 CMPI,  EAX EAX SBB,  TOS EAX MOV,  NEXT,
44 PRIM ( < )       EAX DPOP,  ECX ECX XOR,  TOS EAX CMP,  L IF,  ECX DEC,  THEN,
                    TOS ECX MOV,  NEXT,
45 PRIM ( U< )      EAX DPOP,  TOS EAX CMP,  EAX EAX SBB,  TOS EAX MOV,  NEXT,
46 PRIM ( * )       ECX DPOP,  EAX TOS MOV,  ECX MUL,  TOS EAX MOV,  NEXT,
47 PRIM ( UM* )     EAX TOS MOV,  ECX NOS MOV,  ECX MUL,  NOS EAX MOV,  TOS EDX MOV,  NEXT,
48 PRIM ( UM/MOD )  \ ( lo hi u -- rem quot ): DIV must not overflow, so check first
  ECX DPOP,  EDX TOS MOV,  EAX NOS MOV,
  ECX ECX TEST,  Z IF,  EAX -10 MOVI,  THROW-CODE JMP,  THEN,
  EDX ECX CMP,  AE IF,  EAX -11 MOVI,  THROW-CODE JMP,  THEN,
  ECX DIV,  NOS EDX MOV,  TOS EAX MOV,  NEXT,
49 PRIM ( ROT )     EAX MEM 8 + [EDI] MOV,  ECX NOS MOV,  EDX TOS MOV,
                    MEM 8 + [EDI] ECX MOV,  NOS EDX MOV,  TOS EAX MOV,  NEXT,
50 PRIM ( NIP )     EAX DPOP,  TOS EAX MOV,  NEXT,
51 PRIM ( ?DUP )    EAX TOS MOV,  EAX EAX TEST,  NZ IF,  EAX DPUSH,  THEN,  NEXT,
52 PRIM ( 2DUP )    EAX TOS MOV,  ECX NOS MOV,  ECX DPUSH,  EAX DPUSH,  NEXT,
53 PRIM ( 2DROP )   EDI 8 ADDI,  NEXT,
54 PRIM ( +! )      EAX DPOP,  ECX DPOP,  MEM [EAX] ECX ADD,  NEXT,

\ MOVE and FILL copy with REP MOVSB and REP STOSB. They need ESI and
\ EDI, so IP and DSP wait on the stack of the processor meanwhile. An
\ area outside the memory of the VM (up to VM-END) is THROW -9 before
\ the copy, so the copy itself never faults.
LABEL BAD-ADDRESS   EAX -9 MOVI,  THROW-CODE JMP,
: IN-VM, ( areg -- )   \ the area a u, a in areg and u in ECX: THROW -9 unless inside
  EDX OVER MOV,  EDX ECX ADD,  BAD-ADDRESS JC,  EDX VM-END CMPI,  BAD-ADDRESS JA,  DROP ;
55 PRIM ( MOVE )    \ ( a1 a2 u -- ), also for areas that overlap
  ECX DPOP,  EBX DPOP,  EAX DPOP,  EAX IN-VM,  EBX IN-VM,
  ESI PUSH,  EDI PUSH,
  ESI MEM [EAX] LEA,  EDI MEM [EBX] LEA,
  EBX EAX CMP,  A IF,                                \ a2 > a1: from the end
    ESI ECX ADD,  ESI DEC,  EDI ECX ADD,  EDI DEC,  STD,  REP, MOVSB,  CLD,
  ELSE,  REP, MOVSB,  THEN,
  EDI POP,  ESI POP,  NEXT,
56 PRIM ( FILL )    \ ( a u c -- )
  EAX DPOP,  ECX DPOP,  EBX DPOP,  EBX IN-VM,
  EDI PUSH,  EDI MEM [EBX] LEA,  REP, STOSB,  EDI POP,  NEXT,
57 PRIM ( COUNT )   EAX TOS MOV,  ECX MEM [EAX] BYTE MOVZX,  EAX INC,  TOS EAX MOV,
                    ECX DPUSH,  NEXT,

\ FIND-NAME ( a u -- nt | 0 ): the search order from the top, each
\ wordlist from the newest word (doc/vm.md, section 6). It keeps IP, DSP
\ and RSP in their registers: EBX is the nt, ECX counts the characters,
\ EDX points to the name that it looks for; FN-A, FN-U and FN-WL are
\ cells of KCELLS.
58 PRIM ( FIND-NAME )
  EAX TOS MOV,  FN-U [] EAX MOV,  EAX NOS MOV,  FN-A [] EAX MOV,  EDI 4 ADDI,
  EAX EAX XOR,
  ECX FN-U [] MOV,  ECX ECX TEST,  L8 JZ,  ECX 31 CMPI,  L8 JA,
  EDX MEM #ORDER + [] MOV,  FN-WL [] EDX MOV,
LABEL L1                                        \ the next wordlist
  EDX FN-WL [] MOV,  EDX EDX TEST,  L7 JZ,  EDX DEC,  FN-WL [] EDX MOV,
  EBX MEM CONTEXT + [] [EDX*4] MOV,  EBX MEM [EBX] MOV,
LABEL L2                                        \ the next word
  EBX EBX TEST,  L1 JZ,
  AL MEM 4 + [EBX] MOV,  AL 31 ANDI,  AL FN-U [] CMP,  L6 JNZ,
  ECX ECX XOR,  EDX FN-A [] MOV,
LABEL L3                                        \ the next character
  AL MEM 5 + [EBX] [ECX*1] MOV,  AH MEM [EDX] [ECX*1] MOV,
  MEM CASE-SENSE + [] 0 CMPI,  Z IF,  AL UPPER,  AH UPPER,  THEN,
  AL AH CMP,  L6 JNZ,
  ECX INC,  ECX FN-U [] CMP,  L3 JB,
  EAX EBX MOV,  L8 JMP,                         \ found: the nt
LABEL L6
  EBX MEM [EBX] MOV,  L2 JMP,                   \ the next word of this wordlist
LABEL L7
  EAX EAX XOR,                                  \ not found
LABEL L8
  TOS EAX MOV,  NEXT,

\ The loops: three cells on the return stack, the exit address (deepest),
\ the limit and the index (top).
59 PRIM ( DO )      ECX DPOP,  EDX DPOP,  EAX MEM [ESI] MOV,  ESI 4 ADDI,
                    EBP 12 SUBI,  MEM 8 + [EBP] EAX MOV,  MEM 4 + [EBP] EDX MOV,
                    MEM [EBP] ECX MOV,  NEXT,
60 PRIM ( ?DO )     ECX DPOP,  EDX DPOP,  ECX EDX CMP,  E IF,  ESI MEM [ESI] MOV,  NEXT,  THEN,
                    EAX MEM [ESI] MOV,  ESI 4 ADDI,
                    EBP 12 SUBI,  MEM 8 + [EBP] EAX MOV,  MEM 4 + [EBP] EDX MOV,
                    MEM [EBP] ECX MOV,  NEXT,
61 PRIM ( LOOP )    MEM [EBP] INC,  EAX MEM [EBP] MOV,  EAX MEM 4 + [EBP] CMP,
                    E IF,  EBP 12 ADDI,  ESI 4 ADDI,  NEXT,  THEN,
                    ESI MEM [ESI] MOV,  NEXT,
62 PRIM ( +LOOP )   \ the end when index - limit changes its sign, and n has the other sign
  EAX DPOP,  EDX MEM [EBP] MOV,  EDX MEM 4 + [EBP] SUB,     \ EAX n, EDX index - limit
  MEM [EBP] EAX ADD,                                        \ the new index
  ECX EDX MOV,  ECX EAX ADD,  ECX EDX XOR,  EDX EAX XOR,  ECX EDX AND,
  S IF,  EBP 12 ADDI,  ESI 4 ADDI,  NEXT,  THEN,
  ESI MEM [ESI] MOV,  NEXT,
63 PRIM ( I )       EAX MEM [EBP] MOV,  EAX DPUSH,  NEXT,
64 PRIM ( J )       EAX MEM 12 + [EBP] MOV,  EAX DPUSH,  NEXT,
65 PRIM ( UNLOOP )  EBP 12 ADDI,  NEXT,
