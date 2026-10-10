\ xtensa.fs - the virtual machine of uCForth in Xtensa machine code
\
\ The kernel for the ESP32 chips (Xtensa LX7 in the ESP32-S3, LX6 in the
\ ESP32): NEXT and the primitives 0-28 and 32-66 of doc/vm.md, with
\ 32-bit cells. The target adds its HAL: the console (29-31), 80-88 and
\ the start (targets/esp32s3/hal.fs, targets/esp32/hal.fs).
\
\ Load it after asm/xtensa.fs, with ALSO XTENSA ALSO XTENSA.I, in
\ the vocabulary of the build, after ORG. The build needs a host with
\ 32-bit cells: the constants of the memory map (S0, STATE ...) are then
\ the same for host and target. The build defines two constants first:
\   RAM-ADDR   the chip address of VM address 0 (VM-BASE)
\   >DRAM      code address - >DRAM = the same byte on the data bus; the
\              kernel reads its table there (ESP32-S3: $6F0000, the RAM
\              is on both buses; 0: the table is read where it is)
\ It can also set the variable DATA-OFF of the package XTENSA (see below).
\
\ Memory: the image uses the memory map of doc/vm.md (VM addresses 0 to
\ $3FFFF). They lie in the RAM of the chip from VM-BASE on. With DATA-OFF
\ not 0, VM addresses 0 to $FFFF (SYS and the RAM data) lie at VM-BASE +
\ DATA-OFF instead, in another part of the RAM (the ESP32:
\ targets/esp32/README.md, "Memory"); IP and W never point there. The
\ registers IP W SP RP hold chip addresses; the cells in memory
\ (threads, stacks, variables) hold VM addresses. A value becomes a chip
\ address with "+ MEM" where a VM address goes into a register (NEXT,
\ EXIT, BRANCH, EXECUTE, SP!, RP!), and a VM address again with "- MEM"
\ (DOCOL, SP@). @ ! C@ C! and the other memory words take an address
\ below $40000 as a VM address (>CHIP,) and any other as a chip address:
\ so Forth reaches the registers of the chip ($6000_0000 on) with @ and !.
\
\ Registers (asm/xtensa.fs, "The virtual machine of uCForth", has their
\ names and the macros NEXT, DISPATCH, PUSH, POP, TOS@, TOS!, >CHIP,):
\   A10 MEM   the chip address of VM address 0
\   A11 TBL   the table of the primitives (its DRAM address)
\   A12 IP    A13 W    A14 SP (the top item)    A15 RP
\   A2-A9 are free in each primitive; >CHIP, uses A7-A9.
\   A0 holds the return address of a subroutine (CALL0; there is no
\   C stack).

: NEED-32 ( -- )   CELL 4 <> ABORT" the kernel needs a host with 32-bit cells" ;
NEED-32

\ ==== The table and the constants before the code =======================
\ L32R reads a constant that lies before the code. The build puts the
\ xts of the image into XT-BOOT, XT-QUIT and XT-THROW. The code adds MEM
\ to the address of a system variable; SYSVAR, adds DATA-OFF to it first.

CREATE PRIMS 128 CELLS ALLOT   PRIMS 128 CELLS 0 FILL   \ the code of each primitive
: CODE ( n "name" -- )   \ the code of primitive n starts here
  TALIGN NEW-SCOPE  THERE SWAP CELLS PRIMS + !  PARSE-NAME 2DROP ;
: SYSVAR, ( a -- )   DATA-OFF @ + T, ;   \ a VM address below $10000, for "+ MEM"

LABEL TABLE   128 CELLS TDP +!       \ filled at the end
LABEL TABLE-A      TABLE >DRAM - T,
LABEL VM-BASE      RAM-ADDR T,       \ the RAM of the chip for VM addresses 0 to $3FFFF
LABEL XT-BOOT      0 T,
LABEL XT-QUIT      0 T,
LABEL XT-THROW     0 T,
LABEL S0-V         S0 T,
LABEL R0-V         R0 T,
LABEL STATE-V      STATE SYSVAR,
LABEL HANDLER-V    HANDLER SYSVAR,
LABEL CASE-SENSE-V CASE-SENSE SYSVAR,
LABEL #ORDER-V     #ORDER SYSVAR,
LABEL CONTEXT-V    CONTEXT SYSVAR,
LABEL IRAM-OFF     >DRAM T,

\ ==== Subroutines: CALL0, the return address in A0 ======================

TALIGN LABEL UDIV   \ a3:a2 / a4 -> quotient a2, remainder a3; needs a3 < a4; uses a5 to a7
  A3 L2 BNEZ,
  A5 A2 A4 REMU,                    \ 32 bits only: the chip divides
  A2 A2 A4 QUOU,
  A3 A5 MOV.N,
  RET,
LABEL L2
  A5 32 MOVI,                       \ else one bit per step
LABEL L1
  A6 A3 31 1 EXTUI,                 \ the bit that leaves a3
  A3 A3 1 SLLI,
  A7 A2 31 1 EXTUI,
  A3 A3 A7 OR,
  A2 A2 1 SLLI,
  A6 L3 BNEZ,
  A3 A4 L4 BLTU,
LABEL L3
  A3 A3 A4 SUB,
  A2 A2 1 ADDI.N,
LABEL L4
  A5 A5 -1 ADDI.N,
  A5 L1 BNEZ,
  RET,

\ ==== Errors in primitives ==============================================
\ KTHROW: a THROW code in a2; THROW of the image runs. UNKNOWN: a number
\ without a primitive in this kernel gives -21. RO-ERROR: a store into
\ the frozen part in the flash gives -9 (>CHIP!, of asm/xtensa.fs; only
\ with FLASH-DICT?, else it has no code).

TALIGN LABEL KTHROW
  A2 PUSH,
  W XT-THROW L32R,  W W A3 A4 +BASE,
  DISPATCH,
LABEL UNKNOWN
  A2 -21 MOVI,
  KTHROW J,
: RO-ERROR, ( -- )   FLASH-DICT? @ IF  A2 -9 MOVI,  KTHROW J,  THEN ;
LABEL RO-ERROR
  RO-ERROR,

\ ==== Primitives 0-28 ===================================================

0 CODE DOCOL                        \ its body follows the code field
  A2 IP A3 A4 -BASE,  RP RP -4 ADDI,  A2 RP 0 S32I.N,
  IP W 4 ADDI.N,
  NEXT,
1 CODE DOVAR                        \ CREATE: the body after the cell for DOES>
  A2 W 8 ADDI.N,  A2 A2 A3 A4 -BASE,  A2 PUSH,  NEXT,
2 CODE DOCON
  A2 W 4 L32I.N,  A2 PUSH,  NEXT,
3 CODE DODOES                       \ the body, then the code after DOES>
  A2 W 8 ADDI.N,  A2 A2 A3 A4 -BASE,  A2 PUSH,
  A2 IP A3 A4 -BASE,  RP RP -4 ADDI,  A2 RP 0 S32I.N,
  IP W 4 L32I.N,  IP IP A2 A3 +BASE,
  NEXT,
4 CODE EXIT
  IP RP 0 L32I.N,  RP RP 4 ADDI.N,  IP IP A2 A3 +BASE,  NEXT,
5 CODE LIT
  A2 IP 0 L32I.N,  IP IP 4 ADDI.N,  A2 PUSH,  NEXT,
6 CODE BRANCH
  IP IP 0 L32I.N,  IP IP A2 A3 +BASE,  NEXT,
7 CODE ?BRANCH                      \ jump if the flag is 0
  A2 POP,
  A3 IP 0 L32I.N,  A3 A3 A4 A5 +BASE,  IP IP 4 ADDI.N,
  IP A3 A2 MOVEQZ,
  NEXT,
8 CODE EXECUTE
  W POP,  W W A2 A3 +BASE,  DISPATCH,
9 CODE @
  A2 TOS@,  A2 >CHIP,  A2 A2 0 L32I,  A2 TOS!,  NEXT,
10 CODE !
  A2 SP 0 L32I.N,  A3 SP 4 L32I.N,  SP SP 8 ADDI.N,
  A2 RO-ERROR >CHIP!,  MEMW,  A3 A2 0 S32I,  NEXT,
11 CODE C@
  A2 TOS@,  A2 >CHIP,  A2 A2 0 L8UI,  A2 TOS!,  NEXT,
12 CODE C!
  A2 SP 0 L32I.N,  A3 SP 4 L32I.N,  SP SP 8 ADDI.N,
  A2 RO-ERROR >CHIP!,  MEMW,  A3 A2 0 S8I,  NEXT,
13 CODE DUP
  A2 TOS@,  A2 PUSH,  NEXT,
14 CODE DROP
  SP SP 4 ADDI.N,  NEXT,
15 CODE SWAP
  A2 SP 0 L32I.N,  A3 SP 4 L32I.N,  A3 SP 0 S32I.N,  A2 SP 4 S32I.N,  NEXT,
16 CODE OVER
  A2 SP 4 L32I.N,  A2 PUSH,  NEXT,
17 CODE >R
  A2 POP,  RP RP -4 ADDI,  A2 RP 0 S32I.N,  NEXT,
18 CODE R>
  A2 RP 0 L32I.N,  RP RP 4 ADDI.N,  A2 PUSH,  NEXT,
19 CODE R@
  A2 RP 0 L32I.N,  A2 PUSH,  NEXT,
20 CODE SP@                         \ the address of the top item before SP@
  A2 SP MEM SUB,  A2 PUSH,  NEXT,
21 CODE SP!
  SP SP 0 L32I.N,  SP SP MEM ADD.N,  NEXT,
22 CODE RP@
  A2 RP MEM SUB,  A2 PUSH,  NEXT,
23 CODE RP!
  RP POP,  RP RP MEM ADD.N,  NEXT,
24 CODE AND
  A2 POP,  A3 TOS@,  A3 A3 A2 AND,  A3 TOS!,  NEXT,
25 CODE OR
  A2 POP,  A3 TOS@,  A3 A3 A2 OR,  A3 TOS!,  NEXT,
26 CODE XOR
  A2 POP,  A3 TOS@,  A3 A3 A2 XOR,  A3 TOS!,  NEXT,
27 CODE 0<
  A2 TOS@,  A2 A2 31 SRAI,  A2 TOS!,  NEXT,
28 CODE UM+   ( u1 u2 -- sum carry )
  A2 SP 0 L32I.N,  A3 SP 4 L32I.N,
  A4 A3 A2 ADD.N,  A4 SP 4 S32I.N,
  A5 0 MOVI.N,  A4 A3 L1 BGEU,  A5 1 MOVI.N,     \ carry: the sum is smaller than u1
LABEL L1
  A5 TOS!,  NEXT,

\ ==== Primitives 32-65 ==================================================

32 CODE +
  A2 POP,  A3 TOS@,  A3 A3 A2 ADD.N,  A3 TOS!,  NEXT,
33 CODE -
  A2 POP,  A3 TOS@,  A3 A3 A2 SUB,  A3 TOS!,  NEXT,
34 CODE 1+
  A2 TOS@,  A2 A2 1 ADDI.N,  A2 TOS!,  NEXT,
35 CODE 1-
  A2 TOS@,  A2 A2 -1 ADDI.N,  A2 TOS!,  NEXT,
36 CODE NEGATE
  A2 TOS@,  A2 A2 NEG,  A2 TOS!,  NEXT,
37 CODE INVERT
  A2 TOS@,  A3 -1 MOVI.N,  A2 A2 A3 XOR,  A2 TOS!,  NEXT,
38 CODE 2*
  A2 TOS@,  A2 A2 A2 ADD.N,  A2 TOS!,  NEXT,
39 CODE 2/
  A2 TOS@,  A2 A2 1 SRAI,  A2 TOS!,  NEXT,
40 CODE LSHIFT   ( x u -- x' )     \ from u = 32 on the result is 0
  A2 POP,  A3 TOS@,
  A4 32 MOVI,  A2 A4 L1 BGEU,
  A2 SSL,  A3 A3 SLL,  A3 TOS!,  NEXT,
LABEL L1
  A3 0 MOVI.N,  A3 TOS!,  NEXT,
41 CODE RSHIFT
  A2 POP,  A3 TOS@,
  A4 32 MOVI,  A2 A4 L1 BGEU,
  A2 SSR,  A3 A3 SRL,  A3 TOS!,  NEXT,
LABEL L1
  A3 0 MOVI.N,  A3 TOS!,  NEXT,
42 CODE =
  A2 POP,  A3 TOS@,  A3 A3 A2 SUB,
  A4 0 MOVI.N,  A5 -1 MOVI.N,  A4 A5 A3 MOVEQZ,  A4 TOS!,  NEXT,
43 CODE 0=
  A2 TOS@,  A4 0 MOVI.N,  A5 -1 MOVI.N,  A4 A5 A2 MOVEQZ,  A4 TOS!,  NEXT,
44 CODE <   ( n1 n2 -- f )
  A2 POP,  A3 TOS@,
  A4 0 MOVI.N,  A3 A2 L1 BGE,  A4 -1 MOVI.N,
LABEL L1
  A4 TOS!,  NEXT,
45 CODE U<
  A2 POP,  A3 TOS@,
  A4 0 MOVI.N,  A3 A2 L1 BGEU,  A4 -1 MOVI.N,
LABEL L1
  A4 TOS!,  NEXT,
46 CODE *
  A2 POP,  A3 TOS@,  A3 A3 A2 MULL,  A3 TOS!,  NEXT,
47 CODE UM*   ( u1 u2 -- lo hi )
  A2 SP 0 L32I.N,  A3 SP 4 L32I.N,
  A4 A3 A2 MULL,  A5 A3 A2 MULUH,
  A4 SP 4 S32I.N,  A5 SP 0 S32I.N,  NEXT,
48 CODE UM/MOD   ( lo hi u -- rem quot )
  A4 POP,  A3 SP 0 L32I.N,  A2 SP 4 L32I.N,
  A4 L1 BEQZ,                       \ division by zero
  A3 A4 L2 BGEU,                    \ the quotient needs more than 32 bits
  UDIV CALL0,
  A3 SP 4 S32I.N,  A2 SP 0 S32I.N,  NEXT,
LABEL L1
  A2 -10 MOVI,  KTHROW J,
LABEL L2
  A2 -11 MOVI,  KTHROW J,
49 CODE ROT   ( x1 x2 x3 -- x2 x3 x1 )
  A2 SP 0 L32I.N,  A3 SP 4 L32I.N,  A4 SP 8 L32I.N,
  A3 SP 8 S32I.N,  A2 SP 4 S32I.N,  A4 SP 0 S32I.N,  NEXT,
50 CODE NIP
  A2 POP,  A2 TOS!,  NEXT,
51 CODE ?DUP
  A2 TOS@,  A2 L1 BEQZ,  A2 PUSH,
LABEL L1
  NEXT,
52 CODE 2DUP
  A2 SP 0 L32I.N,  A3 SP 4 L32I.N,  SP SP -8 ADDI,
  A3 SP 4 S32I.N,  A2 SP 0 S32I.N,  NEXT,
53 CODE 2DROP
  SP SP 8 ADDI.N,  NEXT,
54 CODE +!   ( n a -- )
  A2 SP 0 L32I.N,  A3 SP 4 L32I.N,  SP SP 8 ADDI.N,  A2 RO-ERROR >CHIP!,
  A4 A2 0 L32I,  A4 A4 A3 ADD.N,  MEMW,  A4 A2 0 S32I,  NEXT,
55 CODE MOVE   ( a1 a2 u -- )       \ the areas may overlap
  A4 SP 0 L32I.N,  A3 SP 4 L32I.N,  A2 SP 8 L32I.N,  SP SP 12 ADDI.N,
  A2 >CHIP,  A3 RO-ERROR >CHIP!,
  A4 L9 BEQZ,
  A3 A2 L2 BGEU,                    \ a2 >= a1: maybe from the end
LABEL L1                            \ from the start
  A5 A2 0 L8UI,  A5 A3 0 S8I,
  A2 A2 1 ADDI.N,  A3 A3 1 ADDI.N,  A4 A4 -1 ADDI.N,
  A4 L1 BNEZ,
  L9 J,
LABEL L2
  A5 A2 A4 ADD.N,  A3 A5 L1 BGEU,   \ no overlap: from the start
  A2 A2 A4 ADD.N,  A3 A3 A4 ADD.N,
LABEL L3                            \ from the end
  A2 A2 -1 ADDI.N,  A3 A3 -1 ADDI.N,
  A5 A2 0 L8UI,  A5 A3 0 S8I,
  A4 A4 -1 ADDI.N,
  A4 L3 BNEZ,
LABEL L9
  NEXT,
56 CODE FILL   ( a u c -- )
  A2 SP 0 L32I.N,  A3 SP 4 L32I.N,  A4 SP 8 L32I.N,  SP SP 12 ADDI.N,
  A4 RO-ERROR >CHIP!,
  A3 L2 BEQZ,
LABEL L1
  A2 A4 0 S8I,  A4 A4 1 ADDI.N,  A3 A3 -1 ADDI.N,
  A3 L1 BNEZ,
LABEL L2
  NEXT,
57 CODE COUNT   ( a -- a+1 c )
  A2 TOS@,  A3 A2 MOV.N,  A3 >CHIP,
  A4 A3 0 L8UI,  A2 A2 1 ADDI.N,  A2 TOS!,  A4 PUSH,  NEXT,

\ FIND-NAME ( a u -- nt | 0 ): the search order from the top down, each
\ wordlist from the newest word. With CASE-SENSE 0, two bytes are the
\ same when they differ only in bit 5 and are a letter. A name of 32
\ characters or more matches no word: a header holds the length in 5
\ bits. IP is free here (on the return stack) and holds CASE-SENSE; W
\ holds a byte.
\ A2 the name (chip), A3 u, A4 the result, A6 wordlists left, A7 the nt,
\ A8 the nt (chip), A9 j, A0 a byte, A5 free.
58 CODE FIND-NAME
  A3 POP,  A2 TOS@,  A2 >CHIP,
  RP RP -4 ADDI,  IP RP 0 S32I.N,
  A4 0 MOVI.N,
  A3 L9 BEQZ,
  A5 CASE-SENSE-V L32R,  A5 A5 MEM ADD.N,  IP A5 0 L32I,
  A5 #ORDER-V L32R,  A5 A5 MEM ADD.N,  A6 A5 0 L32I,
LABEL L1   \ the next wordlist
  A6 L9 BEQZ,  A6 A6 -1 ADDI.N,
  A5 CONTEXT-V L32R,  A5 A5 MEM ADD.N,  A8 A6 2 SLLI,  A5 A5 A8 ADD.N,
  A7 A5 0 L32I,                     \ the wordlist: a VM address, or a chip address (PSRAM)
  A7 >CHIP,                         \ uses A5 A8 A9
  A7 A7 0 L32I,
LABEL L2   \ the next nt
  A7 L1 BEQZ,
  A8 A7 A5 A9 +BASE,
  A5 A8 4 L8UI,  A5 A5 0 5 EXTUI,  A5 A3 L3 BNE,     \ the length
  A9 0 MOVI.N,
LABEL L4   \ the next byte
  A5 A8 A9 ADD.N,  A0 A5 5 L8UI,
  A5 A2 A9 ADD.N,  W A5 0 L8UI,
  A5 A0 W XOR,  A5 L6 BEQZ,         \ the same byte
  IP L3 BNEZ,                       \ the case counts: not the same
  A5 32 L3 BNEI,                    \ more than the case bit
  A5 32 MOVI,  A5 A0 A5 OR,  A5 A5 -97 ADDI,  A5 L3 BLTZ,
  A5 A5 -26 ADDI,  A5 L3 BGEZ,      \ not a letter
LABEL L6
  A9 A9 1 ADDI.N,  A9 A3 L4 BNE,
  A4 A7 MOV.N,  L9 J,               \ found
LABEL L3   \ not this word
  A7 A8 0 L32I,  L2 J,
LABEL L9
  IP RP 0 L32I.N,  RP RP 4 ADDI.N,
  A4 TOS!,  NEXT,

\ The loops: three cells on the return stack, the exit address (deepest),
\ the limit and the index (top). The exit address is a VM address.
59 CODE (DO)   ( limit start -- )
  A2 SP 0 L32I.N,  A3 SP 4 L32I.N,  SP SP 8 ADDI.N,
  A4 IP 0 L32I.N,  IP IP 4 ADDI.N,
  RP RP -12 ADDI,
  A2 RP 0 S32I.N,  A3 RP 4 S32I.N,  A4 RP 8 S32I.N,  NEXT,
60 CODE (?DO)                       \ as (DO); if limit = start: to the exit
  A2 SP 0 L32I.N,  A3 SP 4 L32I.N,  SP SP 8 ADDI.N,
  A4 IP 0 L32I.N,  IP IP 4 ADDI.N,
  A2 A3 L1 BNE,
  IP A4 A5 A6 +BASE,  NEXT,
LABEL L1
  RP RP -12 ADDI,
  A2 RP 0 S32I.N,  A3 RP 4 S32I.N,  A4 RP 8 S32I.N,  NEXT,
61 CODE (LOOP)
  A2 IP 0 L32I.N,  IP IP 4 ADDI.N,
  A3 RP 0 L32I.N,  A3 A3 1 ADDI.N,  A4 RP 4 L32I.N,
  A3 A4 L1 BEQ,
  A3 RP 0 S32I.N,  IP A2 A4 A5 +BASE,  NEXT,
LABEL L1
  RP RP 12 ADDI.N,  NEXT,
62 CODE (+LOOP)   ( n -- )          \ ends when index - limit crosses the sign
  A5 POP,
  A2 IP 0 L32I.N,  IP IP 4 ADDI.N,
  A3 RP 0 L32I.N,  A4 RP 4 L32I.N,
  A6 A3 A4 SUB,                     \ x = index - limit
  A7 A6 A5 ADD.N,                   \ x + n
  A8 A6 A7 XOR,
  A9 A6 A5 XOR,
  A8 A8 A9 AND,
  A8 L1 BLTZ,
  A3 A3 A5 ADD.N,  A3 RP 0 S32I.N,  IP A2 A4 A6 +BASE,  NEXT,
LABEL L1
  RP RP 12 ADDI.N,  NEXT,
63 CODE I
  A2 RP 0 L32I.N,  A2 PUSH,  NEXT,
64 CODE J
  A2 RP 12 L32I.N,  A2 PUSH,  NEXT,
65 CODE UNLOOP
  RP RP 12 ADDI.N,  NEXT,

\ ==== Primitive 66: machine code in the dictionary =======================
\ A CODE word (lib/esp32/asm.fs) has the code field 66, and its machine
\ code follows. The CPU runs it on the instruction bus (the address +
\ >DRAM). It ends with NEXT, as each primitive does.

66 CODE DOCODE
  A2 W 4 ADDI.N,  A3 IRAM-OFF L32R,  A2 A2 A3 ADD.N,  A2 JX,

: FILL-TABLE ( -- )   \ an entry for each number; UNKNOWN for the numbers without code
  128 0 DO  I CELLS PRIMS + @  ?DUP 0= IF UNKNOWN THEN  TABLE I CELLS + T!  LOOP ;
