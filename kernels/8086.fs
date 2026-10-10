\ 8086.fs - the virtual machine of uCForth in 8086 machine code
\
\ The kernel for each target with an 8086 in real mode (a PC with DOS,
\ a PC without an operating system): NEXT and the primitives 0-28 and
\ 32-66 of doc/vm.md, with 16-bit cells (66: machine code, the CODE
\ words of lib/pc/asm.fs). The target adds its HAL: the console
\ (29-31), 80-99, the start (targets/dos, targets/pc-bios).
\
\ Load it after asm/8086.fs, with ALSO I8086, in the vocabulary of the
\ build, after ORG. The build needs a host with 16-bit cells: then the
\ constants of the memory map (S0, R0, STATE ...) are the same for host
\ and target.
\
\ Memory: the 64 KB of the VM are one segment of the PC. VM address a is
\ the offset a in this segment, so a VM address is also the address for
\ the processor. The code and the table of the primitives are in the
\ code segment (CS); they are not in the memory of the VM, so Forth
\ cannot change them by mistake.
\
\ Registers:
\   SI  IP: the next cell of the running word
\   BX  W: the xt of the word that runs now
\   SP  the data stack: SP points to the top cell (the stack of the CPU)
\   BP  RP: the return stack pointer
\   DS = ES = SS = the segment of the VM; CS = the code
\   AX CX DX DI are free in each primitive. The direction flag is 0
\   (CLD), so LODSW and REP MOVSB count up.
\ With SP as the data stack, PUSH and POP are the stack of Forth, and a
\ primitive is short: + is "POP AX, POP DX, ADD AX,DX, PUSH AX". An
\ interrupt puts 6 bytes below the top of the data stack; that part of
\ the stack is free.

: NEED-16 ( -- )   CELL 2 <> ABORT" the kernel needs a host with 16-bit cells" ;
NEED-16

\ ==== The table and the cells for the start ============================
\ The code starts with a jump to START, the start of the HAL: DOS starts
\ a .COM program at its first byte, a PC its boot sector too.
\ OPTAB holds the address of the code of each primitive, 128 words.
\ CODE n notes THERE as the code of primitive n; FILL-TABLE writes the
\ table at the end. The HAL puts the xts of the image into XT-BOOT,
\ XT-QUIT and XT-THROW before it starts.

FORWARD START   START JMP,       \ the first byte of the code: a jump to the start in the HAL

CREATE PRIMS 128 CELLS ALLOT   PRIMS 128 CELLS 0 FILL   \ the code of each primitive, in the host
: CODE ( n "name" -- )   \ the code of primitive n starts here; the name is a comment
  NEW-SCOPE  THERE SWAP CELLS PRIMS + !  PARSE-NAME 2DROP ;

LABEL OPTAB     256 TDP +!        \ filled at the end (FILL-TABLE)
LABEL XT-BOOT   0 T,
LABEL XT-QUIT   0 T,
LABEL XT-THROW  0 T,
LABEL FN-A      0 T,              \ FIND-NAME: the address of the name it looks for
LABEL FN-U      0 T,              \ and its length

\ ==== Macros ============================================================

: NEXT, ( -- )   \ the inner interpreter (doc/vm.md, section 5)
  LODSW,                          \ AX = the cell at IP; IP = IP + 2
  BX AX XCHG,                     \ W = AX
  DI 0 [BX] MOV,                  \ DI = the code field of W: a primitive number
  DI 1 SHL,
  CS: OPTAB [DI] JMP-IND, ;       \ jump to the code of the primitive
: RPUSH, ( reg -- )   BP 2 SUBI,  0 [BP] SWAP MOV, ;
: RPOP, ( reg -- )   0 [BP] MOV,  BP 2 ADDI, ;
: UPPER, ( reg8 -- )   \ a to z become A to Z
  >R  R@ 'a' CMPI,  AE IF,  R@ 'z' CMPI,  BE IF,  R@ $20 SUBI,  THEN,  THEN,  R> DROP ;

\ An unknown primitive is THROW -21. THROW-CODE runs THROW of the image
\ with the code in AX. DISPATCH runs the word in W (BX) and checks its
\ code field; EXECUTE and the start use it. Each part goes on into the
\ next one.
LABEL UNKNOWN-OP   AX -21 MOVI,
LABEL THROW-CODE   AX PUSH,  CS: BX XT-THROW [] MOV,
LABEL DISPATCH     DI 0 [BX] MOV,  DI 127 CMPI,  UNKNOWN-OP JA,
                   DI 1 SHL,  CS: OPTAB [DI] JMP-IND,

: FILL-TABLE ( -- )   \ each primitive without code runs UNKNOWN-OP
  128 0 DO
    PRIMS I CELLS + @  ?DUP 0= IF UNKNOWN-OP THEN  OPTAB I 2* + T!
  LOOP ;

\ ==== Primitives 0-28 ===================================================

0 CODE DOCOL      SI RPUSH,  SI 2 [BX] LEA,  NEXT,
1 CODE DOVAR      AX 4 [BX] LEA,  AX PUSH,  NEXT,
2 CODE DOCON      2 [BX] PUSH,  NEXT,
3 CODE DODOES     AX 4 [BX] LEA,  AX PUSH,  SI RPUSH,  SI 2 [BX] MOV,  NEXT,
4 CODE EXIT       SI RPOP,  NEXT,
5 CODE LIT        LODSW,  AX PUSH,  NEXT,
6 CODE BRANCH     SI 0 [SI] MOV,  NEXT,
7 CODE ?BRANCH    AX POP,  AX AX OR,  Z IF,  SI 0 [SI] MOV,  NEXT,  THEN,
                  LODSW,  NEXT,                     \ LODSW: IP goes over the cell
8 CODE EXECUTE    BX POP,  DISPATCH JMP,
9 CODE @          BX POP,  0 [BX] PUSH,  NEXT,
10 CODE !         BX POP,  0 [BX] POP,  NEXT,
11 CODE C@        BX POP,  AX AX XOR,  AL 0 [BX] MOV,  AX PUSH,  NEXT,
12 CODE C!        BX POP,  AX POP,  0 [BX] AL MOV,  NEXT,
13 CODE DUP       AX POP,  AX PUSH,  AX PUSH,  NEXT,
14 CODE DROP      AX POP,  NEXT,
15 CODE SWAP      AX POP,  DX POP,  AX PUSH,  DX PUSH,  NEXT,
16 CODE OVER      DX POP,  AX POP,  AX PUSH,  DX PUSH,  AX PUSH,  NEXT,
17 CODE >R        AX POP,  AX RPUSH,  NEXT,
18 CODE R>        AX RPOP,  AX PUSH,  NEXT,
19 CODE R@        0 [BP] PUSH,  NEXT,
20 CODE SP@       AX SP MOV,  AX PUSH,  NEXT,      \ SP before the push
21 CODE SP!       AX POP,  SP AX MOV,  NEXT,
22 CODE RP@       BP PUSH,  NEXT,
23 CODE RP!       BP POP,  NEXT,
24 CODE AND       AX POP,  DX POP,  AX DX AND,  AX PUSH,  NEXT,
25 CODE OR        AX POP,  DX POP,  AX DX OR,  AX PUSH,  NEXT,
26 CODE XOR       AX POP,  DX POP,  AX DX XOR,  AX PUSH,  NEXT,
27 CODE 0<        AX POP,  CWD,  DX PUSH,  NEXT,   \ CWD: DX = -1 if AX is negative, else 0
28 CODE UM+       AX POP,  DX POP,  AX DX ADD,  AX PUSH,
                  AX AX SBB,  AX NEG,  AX PUSH,  NEXT,   \ the carry as 0 or 1

\ ==== Primitives 32-65 ==================================================
\ A flag comes from the carry: "SBB AX,AX" gives -1 when the carry is set,
\ else 0. "CMP AX,1" sets the carry only when AX is 0.

32 CODE +         AX POP,  DX POP,  AX DX ADD,  AX PUSH,  NEXT,
33 CODE -         AX POP,  DX POP,  DX AX SUB,  DX PUSH,  NEXT,
34 CODE 1+        AX POP,  AX INC,  AX PUSH,  NEXT,
35 CODE 1-        AX POP,  AX DEC,  AX PUSH,  NEXT,
36 CODE NEGATE    AX POP,  AX NEG,  AX PUSH,  NEXT,
37 CODE INVERT    AX POP,  AX NOT,  AX PUSH,  NEXT,
38 CODE 2*        AX POP,  AX 1 SHL,  AX PUSH,  NEXT,
39 CODE 2/        AX POP,  AX 1 SAR,  AX PUSH,  NEXT,
40 CODE LSHIFT    CX POP,  AX POP,  CX 16 CMPI,
                  AE IF,  AX AX XOR,  ELSE,  AX CL SHL,  THEN,  AX PUSH,  NEXT,
41 CODE RSHIFT    CX POP,  AX POP,  CX 16 CMPI,
                  AE IF,  AX AX XOR,  ELSE,  AX CL SHR,  THEN,  AX PUSH,  NEXT,
42 CODE =         AX POP,  DX POP,  AX DX SUB,  AX 1 CMPI,  AX AX SBB,  AX PUSH,  NEXT,
43 CODE 0=        AX POP,  AX 1 CMPI,  AX AX SBB,  AX PUSH,  NEXT,
44 CODE <         DX POP,  AX POP,  AX DX CMP,  AX 0 MOVI,
                  L IF,  AX DEC,  THEN,  AX PUSH,  NEXT,   \ MOV does not change the flags
45 CODE U<        DX POP,  AX POP,  AX DX CMP,  AX AX SBB,  AX PUSH,  NEXT,
46 CODE *         AX POP,  DX POP,  DX MUL,  AX PUSH,  NEXT,
47 CODE UM*       AX POP,  DX POP,  DX MUL,  AX PUSH,  DX PUSH,  NEXT,
48 CODE UM/MOD    \ ( lo hi u -- rem quot ): DIV must not overflow, so check first
  CX POP,  DX POP,  AX POP,
  CX CX OR,  Z IF,  AX -10 MOVI,  THROW-CODE JMP,  THEN,
  DX CX CMP,  AE IF,  AX -11 MOVI,  THROW-CODE JMP,  THEN,
  CX DIV,  DX PUSH,  AX PUSH,  NEXT,
49 CODE ROT       CX POP,  DX POP,  AX POP,  DX PUSH,  CX PUSH,  AX PUSH,  NEXT,
50 CODE NIP       AX POP,  DX POP,  AX PUSH,  NEXT,
51 CODE ?DUP      AX POP,  AX PUSH,  AX AX OR,  NZ IF,  AX PUSH,  THEN,  NEXT,
52 CODE 2DUP      DX POP,  AX POP,  AX PUSH,  DX PUSH,  AX PUSH,  DX PUSH,  NEXT,
53 CODE 2DROP     AX POP,  AX POP,  NEXT,
54 CODE +!        BX POP,  AX POP,  0 [BX] AX ADD,  NEXT,
55 CODE MOVE      \ ( a1 a2 u -- ), also for areas that overlap
  CX POP,  DI POP,  AX POP,  DX SI MOV,  SI AX MOV,   \ SI from, DI to; DX keeps IP
  DI SI CMP,  A IF,                                   \ a2 > a1: from the end
    SI CX ADD,  SI DEC,  DI CX ADD,  DI DEC,  STD,  REP, MOVSB,  CLD,
  ELSE,  REP, MOVSB,  THEN,
  SI DX MOV,  NEXT,
56 CODE FILL      \ ( a u c -- )
  AX POP,  CX POP,  DI POP,  REP, STOSB,  NEXT,
57 CODE COUNT     BX POP,  AX AX XOR,  AL 0 [BX] MOV,  BX INC,  BX PUSH,  AX PUSH,  NEXT,

\ FIND-NAME ( a u -- nt | 0 ): the search order from the top, each
\ wordlist from the newest word (doc/vm.md, section 6). IP and RP wait
\ on the stack; BP holds CASE-SENSE. DX counts the wordlists, BX is the
\ nt, SI and DI point into the two names, CX counts the characters.
58 CODE FIND-NAME
  CS: FN-U [] POP,  CS: FN-A [] POP,  SI PUSH,  BP PUSH,
  BP CASE-SENSE [] MOV,  DX #ORDER [] MOV,
  CS: CX FN-U [] MOV,  CX CX OR,  L7 JZ,  CX 31 CMPI,  L7 JA,
LABEL L1                                        \ the next wordlist
  DX DX OR,  L7 JZ,  DX DEC,
  BX DX MOV,  BX 1 SHL,  BX CONTEXT [BX] MOV,  BX 0 [BX] MOV,
LABEL L2                                        \ the next word
  BX BX OR,  L1 JZ,
  AL 2 [BX] MOV,  AL 31 ANDI,  CS: AL FN-U [] CMP,  L6 JNZ,
  SI 3 [BX] LEA,  CS: DI FN-A [] MOV,  CS: CX FN-U [] MOV,
LABEL L3                                        \ the next character
  LODSB,  AH 0 [DI] MOV,  DI INC,
  BP BP OR,  Z IF,  AL UPPER,  AH UPPER,  THEN,
  AL AH CMP,  L6 JNZ,
  L3 LOOP,
  AX BX MOV,  L8 JMP,                           \ found: the nt
LABEL L6
  BX 0 [BX] MOV,  L2 JMP,                       \ the next word of this wordlist
LABEL L7
  AX AX XOR,                                    \ not found
LABEL L8
  BP POP,  SI POP,  AX PUSH,  NEXT,

\ The loops: three cells on the return stack, the exit address (deepest),
\ the limit and the index (top).
59 CODE (DO)      CX POP,  DX POP,  LODSW,
                  BP 6 SUBI,  4 [BP] AX MOV,  2 [BP] DX MOV,  0 [BP] CX MOV,  NEXT,
60 CODE (?DO)     CX POP,  DX POP,  CX DX CMP,  E IF,  SI 0 [SI] MOV,  NEXT,  THEN,
                  LODSW,  BP 6 SUBI,  4 [BP] AX MOV,  2 [BP] DX MOV,  0 [BP] CX MOV,  NEXT,
61 CODE (LOOP)    0 [BP] INC,  AX 0 [BP] MOV,  AX 2 [BP] CMP,
                  E IF,  BP 6 ADDI,  LODSW,  NEXT,  THEN,
                  SI 0 [SI] MOV,  NEXT,
62 CODE (+LOOP)   \ the end when index - limit changes its sign, and n has the other sign
  AX POP,  DX 0 [BP] MOV,  DX 2 [BP] SUB,        \ AX n, DX index - limit
  0 [BP] AX ADD,                                 \ the new index
  CX DX MOV,  CX AX ADD,  CX DX XOR,  DX AX XOR,  CX DX AND,
  S IF,  BP 6 ADDI,  LODSW,  NEXT,  THEN,
  SI 0 [SI] MOV,  NEXT,
63 CODE I         0 [BP] PUSH,  NEXT,
64 CODE J         6 [BP] PUSH,  NEXT,
65 CODE UNLOOP    BP 6 ADDI,  NEXT,

\ ==== Primitive 66: machine code in the dictionary =======================
\ A CODE word (lib/pc/asm.fs) has the code field 66, and its machine
\ code follows, in the segment of the VM. DOCODE puts the way back on
\ the return stack, the far address of CODE-NEXT (the offset in the
\ lower cell, the segment of the kernel in the upper one), and jumps to
\ the code with RETF: CS is then the segment of the VM. The code ends
\ with NEXT, of the package ASM: it takes the way back from the return
\ stack and jumps to CODE-NEXT, the NEXT of the kernel.

LABEL CODE-NEXT   NEXT,
66 CODE DOCODE
  BP 4 SUBI,  0 [BP] CODE-NEXT MOVI,  2 [BP] CS MOV,    \ the way back
  DS PUSH,  AX 2 [BX] LEA,  AX PUSH,  RETF,             \ to the code after the code field
