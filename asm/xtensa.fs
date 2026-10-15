\ xtensa.fs - package XTENSA: an assembler for the Xtensa LX7 (ESP32-S3) and LX6 (ESP32)
\
\ The operands come first, in the order of the GNU assembler, then the
\ instruction with a comma:
\
\   A2 A3 0 L32I,           \ l32i  a2, a3, 0
\   A2 A3 A4 ADD,           \ add   a2, a3, a4
\   A2 L1 BEQZ,             \ beqz  a2, L1
\
\ The code goes to TARGET, a memory in the host. ORG ( ta -- ) starts new
\ code: ta is the address of its first byte on the chip. THERE is the
\ address of the next byte; TDP holds the number of bytes so far. On the
\ board the code goes into the dictionary instead, where it runs
\ (IN-PLACE; the words CODE and END-CODE of lib/esp32/asm.fs).
\
\ Labels: LABEL name marks THERE. A jump to a label before its place
\ gets its offset when LABEL gives the place. For this, L1 to L9 are
\ local labels; a new named label starts a new scope for them. FORWARD
\ name declares a named label before its place. CHECK-LABELS stops with
\ an error if a jump waits for a label without place. A named label is a
\ word in the dictionary: IN-PLACE has only L1 to L9.
\
\ The package also has the registers of the virtual machine of uCForth
\ (kernels/xtensa.fs) and its macros NEXT, PUSH, POP ..., for the kernel
\ and for CODE words, and CONST, for a constant of 32 bits.
\
\ Most instructions are 3 bytes; the forms with .N (code density
\ option) are 2 bytes. The encodings are from the Xtensa Instruction Set
\ Architecture Reference Manual; the tests in asm/test/xtensa.fs compare
\ them with the GNU assembler. The package needs TEXT and cells of 32 bits.

PACKAGE XTENSA  ALSO TEXT   \ lib/package.fs

\ ==== Target memory =====================================================

PUBLIC
$8000 CONSTANT TARGET-SIZE
0 VALUE TARGET                      \ the code, byte for byte: ORG makes it
VARIABLE TORG                       \ the address of the first byte on the chip
PRIVATE
VARIABLE TDP0                       \ the number of bytes so far (ORG)
VARIABLE 'TDP                       \ the counter: TDP0, or the dictionary pointer (IN-PLACE)
TDP0 'TDP !
VARIABLE TBASE                      \ the place of the byte at TORG in this system
VARIABLE TLIMIT                     \ the bytes from TBASE on
VARIABLE PLACED                     \ true: IN-PLACE
PUBLIC
: TDP ( -- a )  'TDP @ ;            \ the counter: TDP @ is the number of bytes so far
: THERE ( -- ta )  TORG @ TDP @ + ;
PRIVATE
: >HOST ( ta -- a )   \ the place of the target address in this system
  TARGET PLACED @ OR 0= ABORT" no target memory: ORG first"   \ neither ORG nor IN-PLACE
  TORG @ -  DUP TLIMIT @ U< 0= ABORT" address outside the target memory"  TBASE @ + ;
: BYTES! ( x ta n -- )   \ the n low bytes of x, the lowest first
  0 ?DO  2DUP >HOST C!  SWAP 8 RSHIFT SWAP 1+  LOOP  2DROP ;
: BYTES, ( x n -- )  THERE SWAP  DUP TDP +!  BYTES! ;
PUBLIC
: TC@ ( ta -- c )  >HOST C@ ;
: TC! ( c ta -- )  >HOST C! ;
: T@ ( ta -- x )  0 SWAP 3 +  4 0 DO  SWAP 8 LSHIFT  OVER TC@ OR  SWAP 1-  LOOP  DROP ;
: T! ( x ta -- )  4 BYTES! ;
: TC, ( c -- )  1 BYTES, ;
: T,  ( x -- )  4 BYTES, ;
: TALIGN ( -- )  BEGIN THERE 3 AND WHILE 0 TC, REPEAT ;
: I16, ( x -- )  2 BYTES, ;
: I24, ( x -- )  3 BYTES, ;
: I24@ ( ta -- x )  DUP TC@  OVER 1+ TC@ 8 LSHIFT OR  SWAP 2 + TC@ 16 LSHIFT OR ;
: I24! ( x ta -- )  3 BYTES! ;

\ ==== Registers and fields ==============================================
\ A register is $1000 plus its number, so a number in its place is an error.

PRIVATE
: REGISTERS ( "name" x16 -- )  16 0 DO  $1000 I + CONSTANT  LOOP ;
PUBLIC
REGISTERS A0 A1 A2 A3 A4 A5 A6 A7 A8 A9 A10 A11 A12 A13 A14 A15

PRIVATE
: REG ( x -- n )  DUP -16 AND $1000 <> ABORT" a register is missing"  15 AND ;
: T| ( x reg -- x' )  REG 4 LSHIFT OR ;    \ field t: bits 4-7
: S| ( x reg -- x' )  REG 8 LSHIFT OR ;    \ field s: bits 8-11
: R| ( x reg -- x' )  REG 12 LSHIFT OR ;   \ field r: bits 12-15
: RRR ( r s t x -- x' )  SWAP T|  SWAP S|  SWAP R| ;

: RANGE ( n lo hi -- n )   \ lo <= n < hi
  >R >R DUP R> R> WITHIN 0= ABORT" number out of range" ;
: /EXACT ( n u -- n/u )  2DUP MOD ABORT" number is not a multiple of the step"  / ;

\ ==== Labels ============================================================
\ The body of a label word is its record: its address (UNKNOWN before
\ its place), its name token, and a flag, true for L1 to L9. A jump to a
\ label before its place goes to the list JUMPS; the place fixes the jump.

-1 CONSTANT UNKNOWN
64 CONSTANT #JUMPS
CREATE JUMPS  #JUMPS 2* CELLS ALLOT   \ pairs: label record, address of the jump
9 CONSTANT #LOCALS
CREATE LOCALS  #LOCALS CELLS ALLOT    \ the records of L1 to L9

: NAME$ ( lab -- a u )  CELL+ @ CELL+ COUNT 31 AND ;
: LOCAL? ( lab -- f )  2 CELLS + @ ;
: JUMP ( i -- a )  2* CELLS JUMPS + ;
: JUMP+ ( lab ta -- )
  #JUMPS 0 DO  I JUMP DUP @ 0= IF  TUCK CELL+ !  !  UNLOOP EXIT THEN  DROP  LOOP
  TRUE ABORT" too many jumps to labels before their place" ;
: UNPLACED ( lab -- )  S" label without place: " ROT NAME$ FAIL ;

: (LABEL) ( ta flag "name" -- lab )   \ a new label word
  CREATE  HERE >R  SWAP , LATEST @ , ,  R>
  DOES>  DUP @ DUP UNKNOWN <> IF NIP EXIT THEN  DROP  THERE JUMP+  UNKNOWN ;

: LOCAL-LABELS ( "name" x9 -- )
  #LOCALS 0 DO  UNKNOWN TRUE (LABEL)  LOCALS I CELLS + !  LOOP ;
PUBLIC
LOCAL-LABELS L1 L2 L3 L4 L5 L6 L7 L8 L9
PRIVATE
' L1 CELL+ @ CONSTANT LABEL-CODE    \ the DOES> code of all label words

: FIND-LABEL ( a u -- lab | 0 )
  FIND-NAME DUP IF  NAME>XT  DUP CELL+ @ LABEL-CODE = IF >BODY EXIT THEN  DROP 0  THEN ;
: NEW-SCOPE ( -- )   \ L1 to L9 are free again; no jump may wait for them
  #JUMPS 0 DO  I JUMP @  DUP IF DUP LOCAL? IF UNPLACED THEN THEN  DROP  LOOP
  #LOCALS 0 DO  UNKNOWN  LOCALS I CELLS + @ !  LOOP ;
PUBLIC
: CHECK-LABELS ( -- )   \ every label that a jump uses has its place
  #JUMPS 0 DO  I JUMP @ ?DUP IF UNPLACED THEN  LOOP ;
: ORG ( ta -- )   \ new code in TARGET; its first byte goes to the target address ta
  TARGET 0= IF  ALIGN HERE TARGET-SIZE ALLOT TO TARGET  THEN
  TORG !  TDP0 'TDP !  0 TDP0 !  TARGET TBASE !  TARGET-SIZE TLIMIT !  PLACED OFF!
  TARGET TARGET-SIZE 0 FILL  JUMPS #JUMPS 2* CELLS 0 FILL  NEW-SCOPE ;
: IN-PLACE ( ta a limit -- )   \ new code in this system, where it runs
  \ The next byte goes to the address in the counter at a (the dictionary
  \ pointer), below limit; the CPU sees the byte at address x at ta + x.
  TLIMIT !  'TDP !  TORG !  0 TBASE !  PLACED ON!
  JUMPS #JUMPS 2* CELLS 0 FILL  NEW-SCOPE ;

\ ==== Offsets of jumps ==================================================
\ A jump goes to ta; pc is the address of the jump. A label before its
\ place gives UNKNOWN; the jump gets offset 0 until the place comes.

PRIVATE
: >OFF ( ta pc -- n )  OVER UNKNOWN = IF 2DROP 0 EXIT THEN  4 + - ;
: OFF6  ( ta pc -- u )  >OFF 0 64 RANGE ;
: OFFU8 ( ta pc -- u )  >OFF 0 256 RANGE ;
: OFF8  ( ta pc -- u )  >OFF -128 128 RANGE 255 AND ;
: OFF12 ( ta pc -- u )  >OFF -2048 2048 RANGE 4095 AND ;
: OFF18 ( ta pc -- u )  >OFF -131072 131072 RANGE $3FFFF AND ;
: COFF18 ( ta pc -- u )   \ for CALLn: in words, from the aligned pc
  OVER UNKNOWN = IF 2DROP 0 EXIT THEN
  OVER 3 AND ABORT" the target of a call must be aligned"
  -4 AND 4 + -  4 /EXACT  -131072 131072 RANGE $3FFFF AND ;

: FIELD! ( ta u shift keep -- )   \ put u, shifted, into the instruction at ta
  >R LSHIFT  OVER I24@ R> AND OR  SWAP I24! ;
: PATCH ( dest ta -- )   \ the jump at ta goes to dest now
  DUP TC@ 15 AND  OVER TC@ 4 RSHIFT                    ( dest ta op0 mn )
  OVER 7 = IF 2DROP  TUCK OFF8 16 $FFFF FIELD! EXIT THEN
  OVER 5 = IF 2DROP  TUCK COFF18 6 $3F FIELD! EXIT THEN
  OVER 12 = OVER 8 AND AND IF 2DROP                    \ BEQZ.N BNEZ.N
    TUCK OFF6  DUP 15 AND 8 LSHIFT  SWAP 4 RSHIFT OR  4 $FF0FCF FIELD! EXIT THEN
  SWAP 6 <> ABORT" a label before its place can only be the target of a jump"
  DUP 3 AND 0= IF DROP  TUCK OFF18 6 $3F FIELD! EXIT THEN      \ J
  DUP 3 AND 1 = IF DROP  TUCK OFF12 12 $FFF FIELD! EXIT THEN   \ BEQZ BNEZ BLTZ BGEZ
  DUP 3 = ABORT" ENTRY is not a jump"
  7 = IF  DUP 1+ TC@ $80 AND IF  TUCK OFFU8 16 $FFFF FIELD! EXIT THEN  THEN   \ LOOP ... LOOPGTZ
  TUCK OFF8 16 $FFFF FIELD! ;                          \ BF BT, BEQI ... BGEUI

: RESOLVE ( lab -- )   \ the label gets its place THERE; fix the jumps to it
  THERE OVER !
  #JUMPS 0 DO  I JUMP  2DUP @ = IF  THERE OVER CELL+ @ PATCH  0 SWAP !  ELSE DROP THEN  LOOP
  DROP ;
PUBLIC
: LABEL ( "name" -- )   \ the label name for the address THERE
  >IN @  PARSE-NAME FIND-LABEL                         ( in lab|0 )
  DUP IF
    DUP LOCAL? IF  NIP  DUP @ UNKNOWN <> IF S" label placed twice: " ROT NAME$ FAIL THEN
      RESOLVE EXIT THEN
    DUP @ UNKNOWN = IF  NIP  NEW-SCOPE RESOLVE EXIT THEN   \ from FORWARD
  THEN
  PLACED @ ABORT" here only the labels L1 to L9"
  DROP >IN !  NEW-SCOPE  THERE FALSE (LABEL) DROP ;
: FORWARD ( "name" -- )
  PLACED @ IF  PARSE-NAME 2DROP  TRUE ABORT" here only the labels L1 to L9"  THEN
  UNKNOWN FALSE (LABEL) DROP ;

\ ==== Instructions ======================================================
\ The value x of each instruction holds its fixed bits.

PRIVATE
: RRR: ( x "name" -- )  CREATE ,  DOES> @ RRR I24, ;          \ ( ar as at -- )
PUBLIC
$100000 RRR: AND,    $200000 RRR: OR,     $300000 RRR: XOR,
$800000 RRR: ADD,    $900000 RRR: ADDX2,  $A00000 RRR: ADDX4,  $B00000 RRR: ADDX8,
$C00000 RRR: SUB,    $D00000 RRR: SUBX2,  $E00000 RRR: SUBX4,  $F00000 RRR: SUBX8,
$810000 RRR: SRC,
$820000 RRR: MULL,   $A20000 RRR: MULUH,  $B20000 RRR: MULSH,
$C20000 RRR: QUOU,   $D20000 RRR: QUOS,   $E20000 RRR: REMU,   $F20000 RRR: REMS,
$430000 RRR: MIN,    $530000 RRR: MAX,    $630000 RRR: MINU,   $730000 RRR: MAXU,
$830000 RRR: MOVEQZ, $930000 RRR: MOVNEZ, $A30000 RRR: MOVLTZ, $B30000 RRR: MOVGEZ,
: MOV, ( ar as -- )  DUP OR, ;

PRIVATE
: RT: ( x "name" -- )  CREATE ,  DOES> @  SWAP T|  SWAP R| I24, ;   \ ( ar at -- )
PUBLIC
$600000 RT: NEG,     $600100 RT: ABS,     $910000 RT: SRL,     $B10000 RT: SRA,
: SLL,  ( ar as -- )  $A10000 SWAP S|  SWAP R| I24, ;
: NSA,  ( at as -- )  $40E000 SWAP S|  SWAP T| I24, ;
: NSAU, ( at as -- )  $40F000 SWAP S|  SWAP T| I24, ;

PRIVATE
: S: ( x "name" -- )  CREATE ,  DOES> @  SWAP S| I24, ;           \ ( as -- )
PUBLIC
$400000 S: SSR,      $401000 S: SSL,      $402000 S: SSA8L,    $403000 S: SSA8B,
$0000A0 S: JX,       $0000C0 S: CALLX0,   $0000D0 S: CALLX4,   $0000E0 S: CALLX8,
$0000F0 S: CALLX12,

PRIVATE
: OP: ( x "name" -- )  CREATE ,  DOES> @ I24, ;                  \ ( -- )
PUBLIC
$000080 OP: RET,     $002000 OP: ISYNC,   $002010 OP: RSYNC,   $002020 OP: ESYNC,
$002030 OP: DSYNC,   $0020C0 OP: MEMW,    $0020D0 OP: EXTW,    $0020F0 OP: NOP,
$000090 OP: RETW,

\ Shifts by a constant. SLLI stores 32 - sa; SRAI and SLLI need 5 bits.
: SLLI, ( ar as sa -- )
  1 32 RANGE  32 SWAP -  DUP 15 AND 4 LSHIFT  SWAP 16 AND 16 LSHIFT OR  $010000 OR
  SWAP S|  SWAP R| I24, ;
: SRAI, ( ar at sa -- )
  0 32 RANGE  DUP 15 AND 8 LSHIFT  SWAP 16 AND 16 LSHIFT OR  $210000 OR
  SWAP T|  SWAP R| I24, ;
: SRLI, ( ar at sa -- )  0 16 RANGE 8 LSHIFT $410000 OR  SWAP T|  SWAP R| I24, ;
: SSAI, ( sa -- )  0 32 RANGE  DUP 15 AND 8 LSHIFT  SWAP 4 RSHIFT 4 LSHIFT OR  $404000 OR I24, ;
: EXTUI, ( ar at shift bits -- )   \ shift + bits <= 32
  2DUP + 33 < 0= ABORT" EXTUI reaches beyond bit 31"
  1 17 RANGE 1- 20 LSHIFT  SWAP 0 32 RANGE  DUP 15 AND 8 LSHIFT  SWAP 16 AND 12 LSHIFT OR
  OR $040000 OR  SWAP T|  SWAP R| I24, ;
: SEXT, ( ar as bit -- )  7 23 RANGE 7 - 4 LSHIFT $230000 OR  SWAP S|  SWAP R| I24, ;

\ Loads, stores, constants. The offset counts in units of the access.
PRIVATE
: RRI8 ( t s u x -- )  SWAP 255 AND 16 LSHIFT OR  SWAP S|  SWAP T| I24, ;
: MEM: ( x unit "name" -- )  CREATE , ,                          \ ( at as offset -- )
  DOES>  DUP @ ROT SWAP /EXACT 0 256 RANGE  SWAP CELL+ @ RRI8 ;
PUBLIC
$000002 1 MEM: L8UI,   $001002 2 MEM: L16UI,  $009002 2 MEM: L16SI,  $002002 4 MEM: L32I,
$004002 1 MEM: S8I,    $005002 2 MEM: S16I,   $006002 4 MEM: S32I,
: ADDI,  ( at as n -- )  -128 128 RANGE  $00C002 RRI8 ;
: ADDMI, ( at as n -- )  256 /EXACT -128 128 RANGE  $00D002 RRI8 ;
: MOVI,  ( at n -- )
  -2048 2048 RANGE  DUP 255 AND 16 LSHIFT  SWAP 8 RSHIFT 15 AND 8 LSHIFT OR  $00A002 OR
  SWAP T| I24, ;
: L32R, ( at ta -- )   \ load the constant at ta; it must lie before the code
  DUP UNKNOWN = ABORT" L32R needs its constant before the code"
  DUP 3 AND ABORT" the constant of L32R must be aligned"
  THERE 3 + -4 AND -  4 /EXACT  -65536 0 RANGE  $FFFF AND 8 LSHIFT 1 OR  SWAP T| I24, ;

\ Jumps. The target is an address, mostly a label.
PRIVATE
: B-RR: ( x "name" -- )  CREATE ,                                \ ( as at ta -- )
  DOES> @  SWAP THERE OFF8 16 LSHIFT OR  SWAP T|  SWAP S| I24, ;
PUBLIC
$001007 B-RR: BEQ,   $009007 B-RR: BNE,   $002007 B-RR: BLT,   $00A007 B-RR: BGE,
$003007 B-RR: BLTU,  $00B007 B-RR: BGEU,  $000007 B-RR: BNONE, $008007 B-RR: BANY,
$004007 B-RR: BALL,  $00C007 B-RR: BNALL, $005007 B-RR: BBC,   $00D007 B-RR: BBS,
PRIVATE
: B-BIT: ( x "name" -- )  CREATE ,                               \ ( as bit ta -- )
  DOES> @  SWAP THERE OFF8 16 LSHIFT OR
  SWAP 0 32 RANGE  DUP 15 AND 4 LSHIFT  SWAP 16 AND 8 LSHIFT OR OR  SWAP S| I24, ;
PUBLIC
$006007 B-BIT: BBCI,  $00E007 B-BIT: BBSI,
PRIVATE
: B-Z: ( x "name" -- )  CREATE ,                                 \ ( as ta -- )
  DOES> @  SWAP THERE OFF12 12 LSHIFT OR  SWAP S| I24, ;
PUBLIC
$000016 B-Z: BEQZ,   $000056 B-Z: BNEZ,   $000096 B-Z: BLTZ,   $0000D6 B-Z: BGEZ,

\ BEQI and its kin compare with one of 16 constants; r holds its index.
PRIVATE
CREATE B4CONST   -1 , 1 , 2 , 3 , 4 , 5 , 6 , 7 , 8 , 10 , 12 , 16 , 32 , 64 , 128 , 256 ,
CREATE B4CONSTU  32768 , 65536 , 2 , 3 , 4 , 5 , 6 , 7 , 8 , 10 , 12 , 16 , 32 , 64 , 128 , 256 ,
: CONST4 ( n table -- u )
  16 0 DO  2DUP I CELLS + @ = IF 2DROP I UNLOOP EXIT THEN  LOOP
  TRUE ABORT" no branch compares with this number" ;
: B-I: ( x table "name" -- )  CREATE , ,                         \ ( as n ta -- )
  DOES> >R  THERE OFF8 16 LSHIFT  SWAP R@ @ CONST4 12 LSHIFT OR  R> CELL+ @ OR  SWAP S| I24, ;
PUBLIC
$000026 B4CONST B-I: BEQI,   $000066 B4CONST B-I: BNEI,
$0000A6 B4CONST B-I: BLTI,   $0000E6 B4CONST B-I: BGEI,
$0000B6 B4CONSTU B-I: BLTUI, $0000F6 B4CONSTU B-I: BGEUI,

: J, ( ta -- )  THERE OFF18 6 LSHIFT 6 OR I24, ;
PRIVATE
: CALL: ( x "name" -- )  CREATE ,  DOES> @  SWAP THERE COFF18 6 LSHIFT OR I24, ;   \ ( ta -- )
PUBLIC
$000005 CALL: CALL0,  $000015 CALL: CALL4,  $000025 CALL: CALL8,  $000035 CALL: CALL12,

\ Special registers
PRIVATE
: SR: ( x "name" -- )  CREATE ,                                  \ ( at sr -- )
  DOES> @  SWAP 0 256 RANGE 8 LSHIFT OR  SWAP T| I24, ;
PUBLIC
$030000 SR: RSR,     $130000 SR: WSR,     $610000 SR: XSR,
: RSIL, ( at level -- )  0 16 RANGE 8 LSHIFT $006000 OR  SWAP T| I24, ;
3 CONSTANT SAR        72 CONSTANT WINDOWBASE  73 CONSTANT WINDOWSTART
177 CONSTANT EPC1     228 CONSTANT INTENABLE  230 CONSTANT PS
231 CONSTANT VECBASE  232 CONSTANT EXCCAUSE   234 CONSTANT CCOUNT
238 CONSTANT EXCVADDR
0 CONSTANT LBEG       1 CONSTANT LEND         2 CONSTANT LCOUNT
192 CONSTANT DEPC     226 CONSTANT INTERRUPT  226 CONSTANT INTSET
227 CONSTANT INTCLEAR 233 CONSTANT DEBUGCAUSE
240 CONSTANT CCOMPARE0  241 CONSTANT CCOMPARE1  242 CONSTANT CCOMPARE2
178 CONSTANT EPC2   179 CONSTANT EPC3   180 CONSTANT EPC4
181 CONSTANT EPC5   182 CONSTANT EPC6   183 CONSTANT EPC7
194 CONSTANT EPS2   195 CONSTANT EPS3   196 CONSTANT EPS4
197 CONSTANT EPS5   198 CONSTANT EPS6   199 CONSTANT EPS7
209 CONSTANT EXCSAVE1  210 CONSTANT EXCSAVE2  211 CONSTANT EXCSAVE3
212 CONSTANT EXCSAVE4  213 CONSTANT EXCSAVE5  214 CONSTANT EXCSAVE6
215 CONSTANT EXCSAVE7
12 CONSTANT SCOMPARE1   96 CONSTANT IBREAKENABLE   97 CONSTANT MEMCTL
99 CONSTANT ATOMCTL     128 CONSTANT IBREAKA0      129 CONSTANT IBREAKA1
144 CONSTANT DBREAKA0   145 CONSTANT DBREAKA1      160 CONSTANT DBREAKC0
161 CONSTANT DBREAKC1   224 CONSTANT CPENABLE      235 CONSTANT PRID
236 CONSTANT ICOUNT     237 CONSTANT ICOUNTLEVEL   244 CONSTANT MISC0
245 CONSTANT MISC1      246 CONSTANT MISC2         247 CONSTANT MISC3

\ Exceptions, interrupts and the register window: the return from an
\ exception or an interrupt of a level, the handlers of the window
\ (L32E S32E: offset -64 to -4), the wait for an interrupt, the turn of
\ the window by n times 4 registers (ROTW).
$003000 OP: RFE,     $003200 OP: RFDE,    $003400 OP: RFWO,    $003500 OP: RFWU,
: RFI, ( level -- )  2 16 RANGE 8 LSHIFT $003010 OR I24, ;
: WAITI, ( level -- )  0 16 RANGE 8 LSHIFT $007000 OR I24, ;
: ROTW, ( n -- )   \ the register window by n times 4 registers: -8 to 7
  -8 8 RANGE 15 AND 4 LSHIFT $408000 OR I24, ;
PRIVATE
: RRI4 ( at as offset x -- )
  >R  -64 0 RANGE 4 /EXACT 16 + 12 LSHIFT  R> OR  SWAP S|  SWAP T| I24, ;
PUBLIC
: L32E, ( at as offset -- )  $090000 RRI4 ;
: S32E, ( at as offset -- )  $490000 RRI4 ;

\ The register window: ENTRY at the start of a function that CALL4,
\ CALL8, CALL12 or CALLXn calls (as, n: the stack pointer and the bytes
\ of its frame, a multiple of 8), RETW at its end. The debug option:
\ BREAK s t and BREAK.N s stop with a debug exception.
: ENTRY, ( as n -- )  8 /EXACT 0 4096 RANGE 12 LSHIFT $000036 OR  SWAP S| I24, ;
: BREAK, ( s t -- )  0 16 RANGE 4 LSHIFT  SWAP 0 16 RANGE 8 LSHIFT OR  $004000 OR I24, ;

\ 16-bit instructions (code density option)
: L32I.N, ( at as offset -- )  4 /EXACT 0 16 RANGE 12 LSHIFT 8 OR  SWAP S|  SWAP T| I16, ;
: S32I.N, ( at as offset -- )  4 /EXACT 0 16 RANGE 12 LSHIFT 9 OR  SWAP S|  SWAP T| I16, ;
: ADD.N,  ( ar as at -- )  10 RRR I16, ;
: ADDI.N, ( ar as n -- )   \ n: -1 or 1 to 15
  DUP -1 = IF DROP 0 ELSE 1 16 RANGE THEN  4 LSHIFT 11 OR  SWAP S|  SWAP R| I16, ;
: MOV.N,  ( at as -- )  13 SWAP S|  SWAP T| I16, ;
: MOVI.N, ( as n -- )
  -32 96 RANGE  DUP 15 AND 12 LSHIFT  SWAP 4 RSHIFT 7 AND 4 LSHIFT OR  12 OR  SWAP S| I16, ;
PRIVATE
: B-Z.N ( as ta x -- )   \ a jump forward by 0 to 63 bytes
  >R  THERE OFF6  DUP 15 AND 12 LSHIFT  SWAP 4 RSHIFT 4 LSHIFT OR  R> OR  SWAP S| I16, ;
PUBLIC
: BEQZ.N, ( as ta -- )  $8C B-Z.N ;
: BNEZ.N, ( as ta -- )  $CC B-Z.N ;
: RET.N, ( -- )  $F00D I16, ;
: RETW.N, ( -- )  $F01D I16, ;
: NOP.N, ( -- )  $F03D I16, ;
: BREAK.N, ( s -- )  0 16 RANGE 8 LSHIFT $F02D OR I16, ;

\ The loop option: LOOP as ta runs the code up to ta (the end, 0 to 255
\ bytes after the next instruction) as times; LOOPNEZ not if as is 0,
\ LOOPGTZ only if as > 0. MUL16U multiplies the low 16 bits. MOVSP sets
\ the stack pointer of a function with a frame of variable size (it can
\ give the exception Alloca). A part of the floating point coprocessor
\ (FPU): its registers F0 to F15 and the booleans B0 to B15, which its
\ comparisons set and BF and BT test. The FPU must be on (bit 0 of
\ CPENABLE). The code of C uses these instructions (lib/esp32/clink.fs).
PRIVATE
: FREG ( x -- n )  DUP -16 AND $2000 <> ABORT" a floating point register is missing"  15 AND ;
: BREG ( x -- n )  DUP -16 AND $3000 <> ABORT" a boolean register is missing"  15 AND ;
: FIELDS ( r s t x -- x' )  SWAP 4 LSHIFT OR  SWAP 8 LSHIFT OR  SWAP 12 LSHIFT OR ;
: F3: ( x "name" -- )  CREATE ,                                  \ ( fr fs ft -- )
  DOES> @ >R  FREG ROT FREG ROT FREG ROT  R> FIELDS I24, ;
: B3: ( x "name" -- )  CREATE ,                                  \ ( br fs ft -- )
  DOES> @ >R  FREG ROT BREG ROT FREG ROT  R> FIELDS I24, ;
: LOOP: ( x "name" -- )  CREATE ,                                \ ( as ta -- )
  DOES> @  SWAP THERE OFFU8 16 LSHIFT OR  SWAP S| I24, ;
: BOOL: ( x "name" -- )  CREATE ,                                \ ( bs ta -- )
  DOES> @  SWAP THERE OFF8 16 LSHIFT OR  SWAP BREG 8 LSHIFT OR I24, ;
: LSCI ( ft as offset r -- )   \ LSI SSI: the offset counts in cells
  SWAP 4 /EXACT 0 256 RANGE 16 LSHIFT 3 OR                     ( ft as r x )
  >R >R  REG SWAP FREG  R> ROT ROT  R> FIELDS I24, ;
PUBLIC
$2000 CONSTANT F0   $2001 CONSTANT F1   $2002 CONSTANT F2   $2003 CONSTANT F3
$2004 CONSTANT F4   $2005 CONSTANT F5   $2006 CONSTANT F6   $2007 CONSTANT F7
$2008 CONSTANT F8   $2009 CONSTANT F9   $200A CONSTANT F10  $200B CONSTANT F11
$200C CONSTANT F12  $200D CONSTANT F13  $200E CONSTANT F14  $200F CONSTANT F15
$3000 CONSTANT B0   $3001 CONSTANT B1   $3002 CONSTANT B2   $3003 CONSTANT B3
$3004 CONSTANT B4   $3005 CONSTANT B5   $3006 CONSTANT B6   $3007 CONSTANT B7
$3008 CONSTANT B8   $3009 CONSTANT B9   $300A CONSTANT B10  $300B CONSTANT B11
$300C CONSTANT B12  $300D CONSTANT B13  $300E CONSTANT B14  $300F CONSTANT B15
$008076 LOOP: LOOP,  $009076 LOOP: LOOPNEZ,  $00A076 LOOP: LOOPGTZ,
$000076 BOOL: BF,    $001076 BOOL: BT,
$C10000 RRR: MUL16U,
: MOVSP, ( at as -- )  $001000 SWAP S|  SWAP T| I24, ;
$0A0000 F3: ADD.S,   $1A0000 F3: SUB.S,   $2A0000 F3: MUL.S,   $4A0000 F3: MADD.S,
$2B0000 B3: OEQ.S,   $4B0000 B3: OLT.S,   $6B0000 B3: OLE.S,
: FLOAT.S,  ( fr as n -- )  0 16 RANGE  ROT FREG ROT REG ROT  $CA0000 FIELDS I24, ;   \ fr = as / 2^n
: UFLOAT.S, ( fr as n -- )  0 16 RANGE  ROT FREG ROT REG ROT  $DA0000 FIELDS I24, ;   \ as without sign
: UTRUNC.S, ( ar fs n -- )  0 16 RANGE  ROT REG ROT FREG ROT  $EA0000 FIELDS I24, ;   \ ar = fs * 2^n
: MOV.S, ( fr fs -- )  FREG SWAP FREG SWAP 0  $FA0000 FIELDS I24, ;
: RFR,   ( ar fs -- )  FREG SWAP REG SWAP 4  $FA0000 FIELDS I24, ;
: WFR,   ( fr as -- )  REG SWAP FREG SWAP 5  $FA0000 FIELDS I24, ;
: LSI, ( ft as offset -- )  0 LSCI ;   \ offset: 0 to 1020, a multiple of 4
: SSI, ( ft as offset -- )  4 LSCI ;

\ ==== The virtual machine of uCForth =====================================
\ The registers of the VM (kernels/xtensa.fs, doc/vm.md section 5):
\   A10 MEM   the chip address of VM address 0
\   A11 TBL   the table of the primitives (its address on the data bus)
\   A12 IP    A13 W    A14 SP (the top item)    A15 RP
\ IP W SP RP hold chip addresses. A2 to A9 are free in a primitive;
\ >CHIP, uses A7 to A9 (A5, A8 and A9 when it converts A7). A0 holds the
\ return address of a subroutine (CALL0).
\
\ DATA-OFF is 0 when all VM addresses lie at MEM + address (ESP32-S3).
\ On the ESP32 the build (targets/esp32/build.fs) and ASM on the board
\ (lib/esp32/esp32/asm.fs) set it: VM addresses 0 to $FFFF (SYS and the
\ RAM data) lie at MEM + DATA-OFF + address there, in another part of
\ the RAM. >CHIP, then uses only A8 and A9.
\
\ FLASH-DICT? is false when the whole dictionary lies in the RAM
\ (ESP32-S3). On the ESP32 the same two places set it true: the frozen
\ part of the dictionary can lie in the flash (targets/esp32/README.md,
\ "Memory").
\ Slot 120 of the table of the primitives (FE-SLOT) then holds FE, the
\ end of the part in the flash (a VM address; S0: none), slot 121
\ (FM-SLOT) the chip address of VM address 0 in the flash, and slot 122
\ (S0-SLOT) S0: a VM address from S0 to FE lies at FM + address, the
\ others at MEM + address. The break table has the same slots, so the
\ slots are always at TBL; no primitive has the numbers 120 to 122.
\ +BASE, gives a VM address of the dictionary its chip address (in NEXT,
\ for each xt), -BASE, the reverse (DOCOL); >CHIP, also reads from S0
\ to FE in the flash. >CHIP!, is >CHIP, before a store: from S0 to FE
\ the code jumps to ro instead (the kernel: -9).
\
\ PER-CORE? is true on the ESP32 (the same two places set it, with
\ FLASH-DICT?): each core of the chip runs the VM with its own SYS and
\ RAM data of the base image, and its own TIB, PAD and stacks. Three
\ special registers of each core hold the bases (the HAL sets them at
\ the start of the core: targets/esp32/hal.fs, "Cores"):
\   MISC1   the chip address of VM address 0 for SYS and the RAM data of
\           the base image (VM addresses below PE)
\   MISC2   the chip address of VM address 0 for TIB, PAD and the stacks
\           (VM addresses $10000 up to S0)
\   MISC3   PE, the end of the RAM data of the base image; $10000 on core
\           0, which has its own part and the common part at one place
\ The other RAM data (from PE up to $FFFF) and the dictionary are the
\ same for all cores: on another core a cell there costs two jumps more.
\ MISC0 holds the variables of the HAL of the core.
\ SYS+, gives a cell of SYS the chip address of this core, STACKS, the
\ base of its stacks (SP@, SP!, RP@, RP!).

A10 CONSTANT MEM   A11 CONSTANT TBL   A12 CONSTANT IP   A13 CONSTANT W
A14 CONSTANT SP    A15 CONSTANT RP
VARIABLE DATA-OFF                   \ 0, or a multiple of 256 (see above)
VARIABLE FLASH-DICT?                \ true: the frozen part can lie in the flash (see above)
VARIABLE PER-CORE?                  \ true: each core has its own SYS, base RAM data and stacks (see above)
120 CELLS CONSTANT FE-SLOT   121 CELLS CONSTANT FM-SLOT   122 CELLS CONSTANT S0-SLOT
PRIVATE
VARIABLE B-D   VARIABLE B-A   VARIABLE B-S   VARIABLE B-T
PUBLIC
: +BASE, ( d a s t -- )   \ d = the chip address of the VM address a in the dictionary; s t: free
  FLASH-DICT? @ 0= IF  2DROP  MEM ADD.N,  EXIT THEN
  B-T !  B-S !  B-A !  B-D !
  B-S @ TBL FE-SLOT L32I,   B-T @ TBL FM-SLOT L32I,      \ s = FE, t = FM
  B-S @ B-A @ B-S @ SUB,    B-T @ MEM B-S @ MOVGEZ,     \ from FE on: t = MEM
  B-D @ B-A @ B-T @ ADD.N, ;
: -BASE, ( r a s t -- )   \ r = the VM address of the chip address a in the dictionary; s t: free
  FLASH-DICT? @ 0= IF  2DROP  MEM SUB,  EXIT THEN
  B-T !  B-S !  B-A !  B-D !
  B-T @ TBL FM-SLOT L32I,   B-S @ B-A @ 23 1 EXTUI,     \ bit 23 is 0 in the flash ($3F40_0000 ...)
  B-T @ MEM B-S @ MOVNEZ,   B-D @ B-A @ B-T @ SUB, ;    \ and 1 in the RAM ($3FFA_0000 ...)
: DISPATCH, ( -- )   \ run the xt in W (a chip address)
  A2 W 0 L32I.N,  A2 A2 TBL ADDX4,  A2 A2 0 L32I.N,  A2 JX, ;
: NEXT, ( -- )  W IP 0 L32I.N,  IP IP 4 ADDI.N,  W W A2 A3 +BASE,  DISPATCH, ;
: POP,  ( reg -- )  SP 0 L32I.N,  SP SP 4 ADDI.N, ;
: PUSH, ( reg -- )  SP SP -4 ADDI,  SP 0 S32I.N, ;
: TOS@, ( reg -- )  SP 0 L32I.N, ;
: TOS!, ( reg -- )  SP 0 S32I.N, ;
PRIVATE
VARIABLE C-REG   VARIABLE C-RO
: FROZEN>CHIP, ( reg ro -- )   \ SPLIT>CHIP, with FLASH-DICT? and PER-CORE?; 57 bytes, the jumps inside
  PER-CORE? @ 0= ABORT" FLASH-DICT? needs PER-CORE?"
  C-RO !  C-REG !
  A9 C-REG @ 16 16 EXTUI,             \ a9: the block of 64 KB
  A9 4 THERE 54 + BGEUI,              \ from $40000: a chip address, as it is
  A9 THERE 40 + BEQZ,                 \ below $10000: SYS and the RAM data
  A8 MEM MOV.N,                       \ the RAM: + MEM
  A9 TBL FE-SLOT L32I,
  C-REG @ A9 THERE 41 + BGEU,         \ from FE on
  A9 TBL S0-SLOT L32I,
  C-REG @ A9 THERE 9 + BGEU,          \ S0 to FE: the flash
  A8 MISC2 RSR,                       \ below S0: TIB, PAD, the stacks of this core
  THERE 29 + J,
  C-RO @ ?DUP IF  J,  ELSE  A8 TBL FM-SLOT L32I,  THEN   \ S0 to FE: + FM, the flash; a store: to ro
  THERE 23 + J,
  A8 DATA-OFF @ 8 RSHIFT MOVI,  A8 A8 8 SLLI,  A8 A8 MEM ADD.N,   \ from PE on: + MEM + DATA-OFF
  THERE 12 + J,
  A8 MISC3 RSR,                       \ below $10000: PE of this core ($10000 on core 0)
  C-REG @ A8 THERE -14 + BGEU,        \ from PE on: the RAM data of all cores
  A8 MISC1 RSR,                       \ below PE: SYS and the RAM data of the base image of this core
  C-REG @ DUP A8 ADD.N, ;
: SPLIT>CHIP, ( reg -- )   \ >CHIP, with DATA-OFF; 21 bytes, the jumps inside
  DATA-OFF @ 255 AND ABORT" DATA-OFF must be a multiple of 256"
  >R  A9 R@ 16 16 EXTUI,              \ a9: the block of 64 KB
  A9 4 THERE 18 + BGEUI,              \ from $40000: a chip address, as it is
  A8 MEM MOV.N,
  A9 THERE 11 + BNEZ,                 \ $10000 to $3FFFF: + MEM
  A8 DATA-OFF @ 8 RSHIFT MOVI,  A8 A8 8 SLLI,  A8 A8 MEM ADD.N,   \ below $10000: + MEM + DATA-OFF
  R> DUP A8 ADD.N, ;
: (>CHIP), ( reg ro -- )
  FLASH-DICT? @ IF  DATA-OFF @ 0= ABORT" FLASH-DICT? needs DATA-OFF"  FROZEN>CHIP,  EXIT THEN
  DROP  DATA-OFF @ IF  SPLIT>CHIP,  EXIT THEN
  >R  A9 R@ 18 14 EXTUI,  A8 MEM MOV.N,
  R@ A7 = IF A5 ELSE A7 THEN  DUP 0 MOVI.N,  A8 SWAP A9 MOVNEZ,
  R> DUP A8 ADD.N, ;
PUBLIC
: >CHIP, ( reg -- )   0 (>CHIP), ;   \ a VM address (below $40000) + its base; any other stays
: >CHIP!, ( reg ro -- )   \ >CHIP, before a store; in the frozen part in the flash: a jump to ro
  DUP 0= ABORT" >CHIP!, needs the address of the code for the error"  (>CHIP), ;
: CONST, ( reg x -- )   \ reg = x: L32R, after a jump over the constant
  THERE 6 + -4 AND 4 + J,  TALIGN  THERE >R  T,  R> L32R, ;
: SYS+, ( reg tmp -- )   \ reg: a VM address of SYS or of the RAM data of the base image; then its chip address
  PER-CORE? @ IF  DUP MISC1 RSR,  >R DUP R> ADD.N,  EXIT THEN   \ this core (tmp: free)
  DROP  DUP MEM ADD.N, ;                                     \ the literal holds + DATA-OFF (SYSVAR, of the kernel)
: STACKS, ( reg -- reg' )   \ reg' holds the chip address of VM address 0 for TIB, PAD and the stacks
  PER-CORE? @ IF  DUP MISC2 RSR,  EXIT THEN  DROP MEM ;      \ this core: reg; else MEM

END-PACKAGE
