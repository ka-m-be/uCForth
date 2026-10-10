\ x64.fs - the virtual machine of uCForth in x86-64 machine code
\
\ The kernel for each target with an x86-64 processor in 64-bit mode
\ (Linux, macOS on Intel, UEFI): NEXT and the primitives 0-28 and 32-65
\ of doc/vm.md, with 32-bit cells. The target adds its HAL: the console
\ (29-31), 80-99, BREAK and the start (targets/linux-x64/hal.fs,
\ targets/macos-x64/hal.fs, targets/uefi-x64/hal.fs).
\
\ Load it after asm/x64.fs, with ALSO X64, in the vocabulary of the
\ build, after ORG. The build gives two constants first: VM-END, the
\ first VM address after the memory that the VM may use; KCELLS, the VM
\ address of 4 cells that the program can write (the break uses the
\ first). The build needs a host with 32-bit cells: then the constants
\ of the memory map (S0, R0, STATE ...) are the same for host and target.
\
\ The code does not depend on its address: macOS and UEFI load a program
\ at an address that changes. It reaches the memory of the VM through
\ R15, and its own cells (the table, XT-BOOT ...) relative to the
\ instruction (n [RIP]). The start code of the HAL sets R15 and R14.
\
\ Registers:
\   R15  MEM: the address of VM address 0
\   R14  TAB: the address of the table OPTAB
\   R12  IP: the next cell of the running word (a VM address)
\   RBX  W: the xt of the word that runs now
\   R13  DSP: the data stack pointer; it points to the top cell
\   RBP  RSP: the return stack pointer
\   RSP  the stack of the processor, for the calls of the system
\ IP, W, DSP and RSP hold VM addresses of 32 bits: an instruction on
\ R12D, EBX, R13D or EBP sets the upper half of the 64-bit register to 0.
\ A load is "mov eax, [r15+r12]": EAX 0 [R15] [R12*1] MOV,. RAX RCX
\ RDX RSI RDI R8-R11 are free in each primitive, and RBX (NEXT loads it
\ again). A call of the system keeps RBX RBP R12-R15, and so the VM:
\ Linux and macOS ("syscall") change only RAX RCX R11 (macOS also RDX);
\ UEFI (the calling convention of Microsoft) keeps them too.

: NEED-32 ( -- )   CELL 4 <> ABORT" the kernel needs a host with 32-bit cells" ;
NEED-32

KCELLS  CONSTANT BREAK-OFF     \ VM address of a cell: 0, or 128 after a break (DISPATCH)

\ ==== Macros ============================================================
\ n [DS] is the cell n bytes above the top of the data stack, n [RS] the
\ same on the return stack. [IP] is the cell at IP.

: [DS] ( n -- mem )  [R15] [R13*1] ;
: [RS] ( n -- mem )  [R15] [RBP*1] ;
: [IP] ( -- mem )    0 [R15] [R12*1] ;
: TOS ( -- mem )  0 [DS] ;                      \ the top cell of the data stack
: NOS ( -- mem )  4 [DS] ;                      \ the second cell
: DPUSH, ( reg32 -- )   R13D 4 SUBI,  TOS SWAP MOV, ;
: DPOP, ( reg32 -- )    TOS MOV,  R13D 4 ADDI, ;
: RPUSH, ( reg32 -- )   EBP 4 SUBI,  0 [RS] SWAP MOV, ;
: RPOP, ( reg32 -- )    0 [RS] MOV,  EBP 4 ADDI, ;
: UPPER, ( reg8 -- )   \ a to z become A to Z
  >R  R@ 'a' CMPI,  AE IF,  R@ 'z' CMPI,  BE IF,  R@ $20 SUBI,  THEN,  THEN,  R> DROP ;

\ ==== The table and the cells for the start ============================
\ OPTAB holds the place of the code of each primitive as an offset from
\ OPTAB, 32 bits each: 128 cells, then 128 more for the break, each the
\ offset of BREAK (the HAL). So the table is the same at each address.
\ PRIM n notes THERE as the code of primitive n; FILL-TABLE writes the
\ table at the end. The build puts the xts of the image into XT-BOOT,
\ XT-QUIT and XT-THROW. All of this lies in the code and is read only.

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
  EAX 0 [R15] [RBX*1] MOV,  EAX 127 CMPI,  UNKNOWN-OP JA,
  EAX BREAK-OFF [R15] ADD,
  EAX 0 [R14] [RAX*4] MOV,  RAX R14 ADD,  RAX JMP-IND, ;
: NEXT, ( -- )   \ the inner interpreter (doc/vm.md, section 5)
  EBX [IP] MOV,  R12D 4 ADDI,  DISPATCH, ;

\ An unknown primitive is THROW -21. THROW-CODE runs THROW of the image
\ with the code in EAX. Each part goes on into the next one.
LABEL UNKNOWN-OP   EAX -21 MOVI,
LABEL THROW-CODE   EAX DPUSH,  EBX XT-THROW [RIP] MOV,
LABEL DISPATCH     DISPATCH,

: FILL-TABLE ( -- )   \ each primitive without code runs UNKNOWN-OP
  BREAK UNKNOWN = ABORT" the HAL has no BREAK"
  128 0 DO
    OPS I CELLS + @  ?DUP 0= IF UNKNOWN-OP THEN  OPTAB -  OPTAB I CELLS + T!
    BREAK OPTAB -  OPTAB I 128 + CELLS + T!
  LOOP ;

\ ==== Primitives 0-28 ===================================================

0 PRIM ( DOCOL )    R12D RPUSH,  R12D 4 [RBX] LEA,  NEXT,
1 PRIM ( DOVAR )    EAX 8 [RBX] LEA,  EAX DPUSH,  NEXT,
2 PRIM ( DOCON )    EAX 4 [R15] [RBX*1] MOV,  EAX DPUSH,  NEXT,
3 PRIM ( DODOES )   EAX 8 [RBX] LEA,  EAX DPUSH,  R12D RPUSH,  R12D 4 [R15] [RBX*1] MOV,  NEXT,
4 PRIM ( EXIT )     R12D RPOP,  NEXT,
5 PRIM ( LIT )      EAX [IP] MOV,  R12D 4 ADDI,  EAX DPUSH,  NEXT,
6 PRIM ( BRANCH )   R12D [IP] MOV,  NEXT,
7 PRIM ( ?BRANCH )  EAX DPOP,  EAX EAX TEST,  Z IF,  R12D [IP] MOV,  NEXT,  THEN,
                    R12D 4 ADDI,  NEXT,
8 PRIM ( EXECUTE )  EBX DPOP,  DISPATCH JMP,
9 PRIM ( @ )        EAX TOS MOV,  EAX 0 [R15] [RAX*1] MOV,  TOS EAX MOV,  NEXT,
10 PRIM ( ! )       EAX DPOP,  ECX DPOP,  0 [R15] [RAX*1] ECX MOV,  NEXT,
11 PRIM ( C@ )      EAX TOS MOV,  EAX 0 [R15] [RAX*1] BYTE MOVZX,  TOS EAX MOV,  NEXT,
12 PRIM ( C! )      EAX DPOP,  ECX DPOP,  0 [R15] [RAX*1] CL MOV,  NEXT,
13 PRIM ( DUP )     EAX TOS MOV,  EAX DPUSH,  NEXT,
14 PRIM ( DROP )    R13D 4 ADDI,  NEXT,
15 PRIM ( SWAP )    EAX TOS MOV,  ECX NOS MOV,  TOS ECX MOV,  NOS EAX MOV,  NEXT,
16 PRIM ( OVER )    EAX NOS MOV,  EAX DPUSH,  NEXT,
17 PRIM ( >R )      EAX DPOP,  EAX RPUSH,  NEXT,
18 PRIM ( R> )      EAX RPOP,  EAX DPUSH,  NEXT,
19 PRIM ( R@ )      EAX 0 [RS] MOV,  EAX DPUSH,  NEXT,
20 PRIM ( SP@ )     EAX R13D MOV,  EAX DPUSH,  NEXT,         \ DSP before the push
21 PRIM ( SP! )     R13D TOS MOV,  NEXT,
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
49 PRIM ( ROT )     EAX 8 [DS] MOV,  ECX NOS MOV,  EDX TOS MOV,
                    8 [DS] ECX MOV,  NOS EDX MOV,  TOS EAX MOV,  NEXT,
50 PRIM ( NIP )     EAX DPOP,  TOS EAX MOV,  NEXT,
51 PRIM ( ?DUP )    EAX TOS MOV,  EAX EAX TEST,  NZ IF,  EAX DPUSH,  THEN,  NEXT,
52 PRIM ( 2DUP )    EAX TOS MOV,  ECX NOS MOV,  ECX DPUSH,  EAX DPUSH,  NEXT,
53 PRIM ( 2DROP )   R13D 8 ADDI,  NEXT,
54 PRIM ( +! )      EAX DPOP,  ECX DPOP,  0 [R15] [RAX*1] ECX ADD,  NEXT,

\ MOVE and FILL copy with REP MOVSB and REP STOSB, in RSI, RDI and RCX.
\ An area outside the memory of the VM (up to VM-END) is THROW -9 before
\ the copy, so the copy itself never faults.
LABEL BAD-ADDRESS   EAX -9 MOVI,  THROW-CODE JMP,
: IN-VM, ( areg -- )   \ the area a u, a in areg and u in ECX: THROW -9 unless inside
  EDX OVER MOV,  EDX ECX ADD,  BAD-ADDRESS JC,  EDX VM-END CMPI,  BAD-ADDRESS JA,  DROP ;
55 PRIM ( MOVE )    \ ( a1 a2 u -- ), also for areas that overlap
  ECX DPOP,  EDI DPOP,  ESI DPOP,  ESI IN-VM,  EDI IN-VM,
  RSI R15 ADD,  RDI R15 ADD,
  RDI RSI CMP,                                     \ a2 > a1: from the end
  A IF,  RSI RCX ADD,  RSI DEC,  RDI RCX ADD,  RDI DEC,  STD,  REP, MOVSB,  CLD,
  ELSE,  REP, MOVSB,  THEN,  NEXT,
56 PRIM ( FILL )    \ ( a u c -- )
  EAX DPOP,  ECX DPOP,  EDI DPOP,  EDI IN-VM,
  RDI R15 ADD,  REP, STOSB,  NEXT,
57 PRIM ( COUNT )   EAX TOS MOV,  ECX 0 [R15] [RAX*1] BYTE MOVZX,  EAX INC,  TOS EAX MOV,
                    ECX DPUSH,  NEXT,

\ FIND-NAME ( a u -- nt | 0 ): the search order from the top, each
\ wordlist from the newest word (doc/vm.md, section 6). R8D u, R9 a,
\ R10D the wordlists left, R11D CASE-SENSE, EBX the nt, RSI MEM + nt,
\ RDI MEM + a, EDX counts the characters, EAX the result.
58 PRIM ( FIND-NAME )
  R8D DPOP,  R9D TOS MOV,  EAX EAX XOR,
  R8D R8D TEST,  L8 JZ,  R8D 31 CMPI,  L8 JA,
  R10D #ORDER [R15] MOV,  R11D CASE-SENSE [R15] MOV,
  RDI 0 [R15] [R9*1] LEA,
LABEL L1                                        \ the next wordlist
  R10D R10D TEST,  L7 JZ,  R10D DEC,
  EBX CONTEXT [R15] [R10*4] MOV,  EBX 0 [R15] [RBX*1] MOV,
LABEL L2                                        \ the next word
  EBX EBX TEST,  L1 JZ,
  CL 4 [R15] [RBX*1] MOV,  CL 31 ANDI,  CL R8B CMP,  L6 JNZ,
  RSI 5 [R15] [RBX*1] LEA,  EDX EDX XOR,
LABEL L3                                        \ the next character
  AL 0 [RSI] [RDX*1] MOV,  CL 0 [RDI] [RDX*1] MOV,
  R11D R11D TEST,  Z IF,  AL UPPER,  CL UPPER,  THEN,
  AL CL CMP,  L6 JNZ,
  EDX INC,  EDX R8D CMP,  L3 JB,
  EAX EBX MOV,  L8 JMP,                         \ found: the nt
LABEL L6
  EBX 0 [R15] [RBX*1] MOV,  L2 JMP,             \ the next word of this wordlist
LABEL L7
  EAX EAX XOR,                                  \ not found
LABEL L8
  TOS EAX MOV,  NEXT,

\ The loops: three cells on the return stack, the exit address (deepest),
\ the limit and the index (top).
59 PRIM ( DO )      ECX DPOP,  EDX DPOP,  EAX [IP] MOV,  R12D 4 ADDI,
                    EBP 12 SUBI,  8 [RS] EAX MOV,  4 [RS] EDX MOV,  0 [RS] ECX MOV,  NEXT,
60 PRIM ( ?DO )     ECX DPOP,  EDX DPOP,  ECX EDX CMP,  E IF,  R12D [IP] MOV,  NEXT,  THEN,
                    EAX [IP] MOV,  R12D 4 ADDI,
                    EBP 12 SUBI,  8 [RS] EAX MOV,  4 [RS] EDX MOV,  0 [RS] ECX MOV,  NEXT,
61 PRIM ( LOOP )    0 [RS] INC,  EAX 0 [RS] MOV,  EAX 4 [RS] CMP,
                    E IF,  EBP 12 ADDI,  R12D 4 ADDI,  NEXT,  THEN,
                    R12D [IP] MOV,  NEXT,
62 PRIM ( +LOOP )   \ the end when index - limit changes its sign, and n has the other sign
  EAX DPOP,  EDX 0 [RS] MOV,  EDX 4 [RS] SUB,               \ EAX n, EDX index - limit
  0 [RS] EAX ADD,                                           \ the new index
  ECX EDX MOV,  ECX EAX ADD,  ECX EDX XOR,  EDX EAX XOR,  ECX EDX AND,
  S IF,  EBP 12 ADDI,  R12D 4 ADDI,  NEXT,  THEN,
  R12D [IP] MOV,  NEXT,
63 PRIM ( I )       EAX 0 [RS] MOV,  EAX DPUSH,  NEXT,
64 PRIM ( J )       EAX 12 [RS] MOV,  EAX DPUSH,  NEXT,
65 PRIM ( UNLOOP )  EBP 12 ADDI,  NEXT,
