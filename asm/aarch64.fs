\ aarch64.fs - package AARCH64: an assembler for 64-bit ARM (A64)
\
\ The operands come first, in the order of the GNU assembler, then the
\ instruction with a comma:
\
\   W0 W1 W2 ADD,           \ add  w0, w1, w2
\   W0 W1 4 ADDI,           \ add  w0, w1, #4
\   W0 X28 W1 LDR-UXTW,     \ ldr  w0, [x28, w1, uxtw]
\   NEXT B,                 \ b    next
\
\ The code goes to CODE, a memory in the host; ORG is the address of its
\ first byte in the target. THERE is the address of the next byte.
\ START-CODE ( ta -- ) starts new code at the target address ta; before
\ it, each instruction stops with an error.
\
\ Labels: LABEL name marks THERE. A label that comes later needs
\ FORWARD name first; LABEL name then gives it its place and fixes all
\ jumps to it. END-CODE stops with an error if a label has no place.
\ Short jumps inside a word: IF, ELSE, THEN, BEGIN, UNTIL, AGAIN, with a
\ condition (EQ NE ...) or a register (0=IF, 0<>IF,).
\
\ The encodings are from the Arm Architecture Reference Manual (A64);
\ the tests in asm/test/aarch64.fs compare them with the GNU assembler.
\ The package needs cells of 32 bits.

PACKAGE AARCH64   \ lib/package.fs

PRIVATE

: PICK ( xu ... x0 u -- xu ... x0 xu )   1+ CELLS SP@ + @ ;   \ doc/vm.md 5: the stack

\ ==== Code memory =======================================================

$4000 CONSTANT CODE-SIZE
PUBLIC
ALIGN  HERE CODE-SIZE ALLOT  CONSTANT CODE
VARIABLE ORG   VARIABLE CODE#   \ the target address of CODE, the bytes so far
-1 ORG !                        \ no target address before START-CODE

: THERE ( -- ta )   ORG @ CODE# @ + ;
PRIVATE
: >CODE ( ta -- a )   \ the place of a target address in CODE
  ORG @ -1 = ABORT" no target address: START-CODE first"
  ORG @ -  DUP CODE-SIZE U< 0= ABORT" address outside the code"  CODE + ;
PUBLIC
: A@ ( ta -- x )   >CODE @ ;
: A! ( x ta -- )   >CODE ! ;
: A, ( x -- )   THERE A!  4 CODE# +! ;      \ one instruction or data word
: ALIGN8 ( -- )   BEGIN THERE 7 AND WHILE 0 A, REPEAT ;
: START-CODE ( ta -- )   ORG !  0 CODE# !  CODE CODE-SIZE 0 FILL ;

\ ==== Registers =========================================================
\ A register is $1000 + n (W, 32 bit) or $1100 + n (X, 64 bit). So a
\ number in the place of a register is an error.

PRIVATE
: WREGS ( "name" x32 -- )   32 0 DO  $1000 I + CONSTANT  LOOP ;
: XREGS ( "name" x32 -- )   32 0 DO  $1100 I + CONSTANT  LOOP ;
PUBLIC
WREGS W0 W1 W2 W3 W4 W5 W6 W7 W8 W9 W10 W11 W12 W13 W14 W15 W16 W17 W18 W19 W20 W21 W22 W23 W24 W25 W26 W27 W28 W29 W30 WZR
XREGS X0 X1 X2 X3 X4 X5 X6 X7 X8 X9 X10 X11 X12 X13 X14 X15 X16 X17 X18 X19 X20 X21 X22 X23 X24 X25 X26 X27 X28 X29 X30 XZR
XZR CONSTANT XSP   \ the stack pointer: register 31 in ADDI, and loads

PRIVATE
: REG ( r -- n )   DUP -512 AND $1000 <> ABORT" a register is missing"  31 AND ;
: SF ( r -- x )   $100 AND IF $80000000 ELSE 0 THEN ;   \ bit 31: 64 bit
: RD ( x r -- x' )   REG OR ;
: RN ( x r -- x' )   REG 5 LSHIFT OR ;
: RM ( x r -- x' )   REG 16 LSHIFT OR ;
: RA ( x r -- x' )   REG 10 LSHIFT OR ;

: RANGE ( n lo hi -- n )   \ lo <= n < hi
  >R >R DUP R> R> WITHIN 0= ABORT" number out of range" ;

\ ==== Data processing ===================================================

: REG3 ( rd rn rm op -- )   \ rd = rn op rm
  3 PICK SF OR  SWAP RM  SWAP RN  SWAP RD  A, ;
PUBLIC
: ADD,  ( rd rn rm -- )   $0B000000 REG3 ;
: ADDS, ( rd rn rm -- )   $2B000000 REG3 ;
: SUB,  ( rd rn rm -- )   $4B000000 REG3 ;
: SUBS, ( rd rn rm -- )   $6B000000 REG3 ;
: AND,  ( rd rn rm -- )   $0A000000 REG3 ;
: ORR,  ( rd rn rm -- )   $2A000000 REG3 ;
: ORN,  ( rd rn rm -- )   $2A200000 REG3 ;
: EOR,  ( rd rn rm -- )   $4A000000 REG3 ;
: BIC,  ( rd rn rm -- )   $0A200000 REG3 ;   \ rd = rn AND NOT rm
: LSLV, ( rd rn rm -- )   $1AC02000 REG3 ;
: LSRV, ( rd rn rm -- )   $1AC02400 REG3 ;
: ASRV, ( rd rn rm -- )   $1AC02800 REG3 ;
: UDIV, ( rd rn rm -- )   $1AC00800 REG3 ;
: MUL,  ( rd rn rm -- )   $1B007C00 REG3 ;   \ madd rd, rn, rm, zr

PRIVATE
: ZR ( r -- zr )   SF IF XZR ELSE WZR THEN ;
PUBLIC
: MOV, ( rd rm -- )   OVER ZR SWAP ORR, ;          \ orr rd, zr, rm
: MVN, ( rd rm -- )   OVER ZR SWAP ORN, ;          \ orn rd, zr, rm
: NEG, ( rd rm -- )   OVER ZR SWAP SUB, ;          \ sub rd, zr, rm
: CMP, ( rn rm -- )   OVER ZR ROT ROT SUBS, ;      \ subs zr, rn, rm

: MSUB, ( rd rn rm ra -- )   \ rd = ra - rn * rm
  $1B008000  4 PICK SF OR  SWAP RA  SWAP RM  SWAP RN  SWAP RD  A, ;
: UMULL, ( xd wn wm -- )   \ xd = wn * wm, 64 bit
  $9BA07C00  SWAP RM  SWAP RN  SWAP RD  A, ;

PRIVATE
: IMM12 ( rd rn imm op -- )   \ rd = rn op imm, 0 <= imm < 4096
  3 PICK SF OR  SWAP 0 4096 RANGE 10 LSHIFT OR  SWAP RN  SWAP RD  A, ;
PUBLIC
: ADDI,  ( rd rn imm -- )   $11000000 IMM12 ;
: SUBI,  ( rd rn imm -- )   $51000000 IMM12 ;
: SUBSI, ( rd rn imm -- )   $71000000 IMM12 ;
: CMPI,  ( rn imm -- )   OVER ZR ROT ROT SUBSI, ;

PRIVATE
: BFM ( rd rn immr imms op -- )   \ bitfield move
  4 PICK SF IF $00400000 OR THEN  4 PICK SF OR
  SWAP 10 LSHIFT OR  SWAP 16 LSHIFT OR  SWAP RN  SWAP RD  A, ;
: BITS ( r -- n )   SF IF 64 ELSE 32 THEN ;
PUBLIC
: LSLI, ( rd rn n -- )   \ lsl rd, rn, #n = ubfm rd, rn, #(-n mod bits), #(bits-1-n)
  OVER BITS >R  R@ OVER - R@ 1- AND  SWAP R> 1- SWAP -  $53000000 BFM ;
: LSRI, ( rd rn n -- )   OVER BITS 1-  $53000000 BFM ;
: ASRI, ( rd rn n -- )   OVER BITS 1-  $13000000 BFM ;
: SXTW, ( xd wn -- )   0 31 $13000000 BFM ;   \ sxtw xd, wn = sbfm xd, xn, #0, #31: wn with its sign

PRIVATE
: MOVW ( rd imm16 hw op -- )   \ a move of 16 bits into the part hw
  3 PICK SF OR  SWAP 21 LSHIFT OR  SWAP $FFFF AND 5 LSHIFT OR  SWAP RD  A, ;
PUBLIC
: MOVZ, ( rd imm16 hw -- )   $52800000 MOVW ;
: MOVN, ( rd imm16 hw -- )   $12800000 MOVW ;
: MOVK, ( rd imm16 hw -- )   $72800000 MOVW ;
: MOVI, ( rd x -- )   \ any number of 32 bits, with one or two instructions
  DUP $10000 U< IF 0 MOVZ, EXIT THEN
  DUP INVERT $10000 U< IF INVERT 0 MOVN, EXIT THEN
  2DUP 0 MOVZ,  16 RSHIFT 1 MOVK, ;

\ Conditions, and conditional selects
 0 CONSTANT EQ    1 CONSTANT NE    2 CONSTANT HS    3 CONSTANT LO
 4 CONSTANT MI    5 CONSTANT PL    8 CONSTANT HI    9 CONSTANT LS
10 CONSTANT GE   11 CONSTANT LT   12 CONSTANT GT   13 CONSTANT LE
PRIVATE
: NOT-COND ( cond -- cond' )   1 XOR ;
PUBLIC
: CSEL, ( rd rn rm cond -- )   \ rd = cond ? rn : rm
  12 LSHIFT $1A800000 OR  3 PICK SF OR  SWAP RM  SWAP RN  SWAP RD  A, ;
: CSET,  ( rd cond -- )   \ rd = cond ? 1 : 0   (csinc rd, zr, zr, !cond)
  NOT-COND 12 LSHIFT $1A9F07E0 OR  OVER SF OR  SWAP RD  A, ;
: CSETM, ( rd cond -- )   \ rd = cond ? -1 : 0  (csinv rd, zr, zr, !cond)
  NOT-COND 12 LSHIFT $5A9F03E0 OR  OVER SF OR  SWAP RD  A, ;

\ ==== Loads and stores ==================================================
\ [xn, wm, uxtw]: the address is xn + wm, wm without sign (32 bit).

PRIVATE
: LS-R ( rt xn rm op -- )   SWAP RM  SWAP RN  SWAP RD  A, ;
PUBLIC
: LDR-UXTW,  ( wt xn wm -- )   $B8604800 LS-R ;   \ ldr  wt, [xn, wm, uxtw]
: STR-UXTW,  ( wt xn wm -- )   $B8204800 LS-R ;   \ str  wt, [xn, wm, uxtw]
: LDRB-UXTW, ( wt xn wm -- )   $38604800 LS-R ;   \ ldrb wt, [xn, wm, uxtw]
: STRB-UXTW, ( wt xn wm -- )   $38204800 LS-R ;   \ strb wt, [xn, wm, uxtw]
: LDR-LSL2,  ( wt xn xm -- )   $B8607800 LS-R ;   \ ldr  wt, [xn, xm, lsl #2]

: LDR-I, ( wt xn imm -- )   \ ldr wt, [xn, #imm]
  2 RSHIFT 0 4096 RANGE 10 LSHIFT $B9400000 OR  SWAP RN  SWAP RD  A, ;
: STR-I, ( wt xn imm -- )   \ str wt, [xn, #imm]
  2 RSHIFT 0 4096 RANGE 10 LSHIFT $B9000000 OR  SWAP RN  SWAP RD  A, ;
: LDRX-I, ( xt xn imm -- )   \ ldr xt, [xn, #imm]: 64 bits
  3 RSHIFT 0 4096 RANGE 10 LSHIFT $F9400000 OR  SWAP RN  SWAP RD  A, ;
: STRX-I, ( xt xn imm -- )   \ str xt, [xn, #imm]: 64 bits
  3 RSHIFT 0 4096 RANGE 10 LSHIFT $F9000000 OR  SWAP RN  SWAP RD  A, ;
: LDRB-I, ( wt xn imm -- )   0 4096 RANGE 10 LSHIFT $39400000 OR  SWAP RN  SWAP RD  A, ;
: STRB-I, ( wt xn imm -- )   0 4096 RANGE 10 LSHIFT $39000000 OR  SWAP RN  SWAP RD  A, ;

\ ==== Labels and jumps ==================================================
\ A label is a CREATE word; its cell holds the target address, or -1
\ before its place. A jump to a label without place goes to the list
\ FIXES; LABEL fixes it.

PRIVATE
-1 CONSTANT UNKNOWN
128 CONSTANT #FIXES
CREATE FIXES #FIXES 3 * CELLS ALLOT   \ label, address of the jump, kind
VARIABLE NFIX

: OFFSET ( ta -- n )   THERE - 2/ 2/ ;   \ in instructions, from THERE
: FIX-FIELD ( n ta kind -- )   \ put the offset n into the jump at ta
  >R  DUP A@  R>                         ( n ta x kind )
  DUP 26 = IF DROP ROT $3FFFFFF AND OR SWAP A! EXIT THEN
  DUP 19 = IF DROP ROT $7FFFF AND 5 LSHIFT OR SWAP A! EXIT THEN
  14 = IF ROT $3FFF AND 5 LSHIFT OR SWAP A! EXIT THEN
  TRUE ABORT" unknown kind of jump" ;
: FIX+ ( lab kind -- )   \ remember a jump at THERE to a label without place
  NFIX @ #FIXES < 0= ABORT" too many jumps to labels without place"
  FIXES NFIX @ 3 * CELLS +  >R  SWAP R@ !  THERE R@ CELL+ !  R> 2 CELLS + !
  1 NFIX +! ;
: TARGET ( lab kind -- n )   \ the offset to the label, 0 before its place
  OVER @ UNKNOWN = IF FIX+ 0 ELSE DROP @ OFFSET THEN ;

VARIABLE FL
: FIX-ALL ( lab -- )   \ the label has its place now: fix the jumps to it
  FL !  NFIX @ 0 ?DO
    FIXES I 3 * CELLS +  DUP @ FL @ = IF
      >R  FL @ @ R@ CELL+ @ - 2/ 2/          ( n )
      R@ CELL+ @  R@ 2 CELLS + @  FIX-FIELD  0 R> !
    ELSE DROP THEN
  LOOP ;

PUBLIC
: FORWARD ( "name" -- )   CREATE UNKNOWN , ;
: LABEL ( "name" -- )   \ THERE is the place of name
  >IN @  PARSE-NAME CURRENT @ WL-FIND NIP NIP  ?DUP IF
    NAME>XT >BODY  DUP @ UNKNOWN = IF  NIP THERE OVER !  FIX-ALL EXIT  THEN  DROP
  THEN  >IN !  CREATE THERE , ;
: END-CODE ( -- )   \ each jump must have its target now
  NFIX @ 0 ?DO  FIXES I 3 * CELLS + @ IF TRUE ABORT" a label has no place" THEN  LOOP
  0 NFIX ! ;

: B,  ( lab -- )   26 TARGET $3FFFFFF AND $14000000 OR A, ;
: BL, ( lab -- )   26 TARGET $3FFFFFF AND $94000000 OR A, ;
: B.COND, ( lab cond -- )   SWAP 19 TARGET $7FFFF AND 5 LSHIFT OR $54000000 OR A, ;
: CBZ,  ( rt lab -- )   19 TARGET $7FFFF AND 5 LSHIFT OVER SF OR $34000000 OR SWAP RD A, ;
: CBNZ, ( rt lab -- )   19 TARGET $7FFFF AND 5 LSHIFT OVER SF OR $35000000 OR SWAP RD A, ;
: BR,  ( xn -- )   $D61F0000 SWAP RN A, ;
: BLR, ( xn -- )   $D63F0000 SWAP RN A, ;
: RET, ( -- )   $D65F03C0 A, ;
: SVC, ( -- )   $D4000001 A, ;        \ svc #0: a call of the system
: NOP, ( -- )   $D503201F A, ;
: WFE, ( -- )   $D503205F A, ;        \ wait for an event: a core waits with little power

\ System registers: op0 op1 CRn CRm op2 as one number
\ (op0 - 2) * $4000 + op1 * $800 + CRn * $80 + CRm * 8 + op2.
$4005 CONSTANT MPIDR-EL1     \ the number of this core, bits 0-7
$4212 CONSTANT CURRENTEL     \ the exception level, bits 2-3
$4600 CONSTANT VBAR-EL1      \ the table of the exception vectors, for EL1
$6600 CONSTANT VBAR-EL2
$7600 CONSTANT VBAR-EL3
: MRS, ( xt sysreg -- )   \ xt = the system register
  5 LSHIFT $D5300000 OR  SWAP RD A, ;
: MSR, ( sysreg xt -- )   \ the system register = xt
  SWAP 5 LSHIFT $D5100000 OR  SWAP RD A, ;
: ISB, ( -- )   $D5033FDF A, ;        \ the next instructions see the new system registers
: LDR-LIT, ( wt lab -- )   \ ldr wt, label: 32 bits from the code
  19 TARGET $7FFFF AND 5 LSHIFT $18000000 OR  SWAP RD A, ;
: ADR, ( xd lab -- )   \ xd = the address of the label (+-1 MB)
  DUP @ UNKNOWN = ABORT" ADR needs a label with a place"
  @ THERE -  DUP 3 AND 29 LSHIFT  SWAP 2/ 2/ $7FFFF AND 5 LSHIFT OR
  $10000000 OR  SWAP RD A, ;

\ ==== Structures ========================================================
\ IF, ... THEN, jumps over the code when the condition is false.

: THEN, ( orig -- )   \ the jump at orig goes to THERE; B has 26 bits, the others 19
  THERE OVER - 2/ 2/  OVER  ROT A@ $FC000000 AND $14000000 = IF 26 ELSE 19 THEN  FIX-FIELD ;
: IF, ( cond -- orig )   THERE SWAP NOT-COND $54000000 OR A, ;
: 0=IF, ( rt -- orig )   THERE SWAP  $35000000 OVER SF OR SWAP RD A, ;   \ cbnz
: 0<>IF, ( rt -- orig )  THERE SWAP  $34000000 OVER SF OR SWAP RD A, ;   \ cbz
: ELSE, ( orig1 -- orig2 )   THERE $14000000 A,  SWAP THEN, ;
: BEGIN, ( -- dest )   THERE ;
: UNTIL, ( dest cond -- )   \ back to dest while the condition is false
  NOT-COND  SWAP OFFSET $7FFFF AND 5 LSHIFT OR $54000000 OR A, ;
: AGAIN, ( dest -- )   OFFSET $3FFFFFF AND $14000000 OR A, ;

END-PACKAGE
