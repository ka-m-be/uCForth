\ 8086.fs - package I8086: an assembler for the Intel 8086 (16-bit real mode)
\
\ The operands come first, in the order of the Intel manual (the
\ destination first), then the instruction with a comma. The words
\ are the Intel names; DEBUG shows the same instructions:
\
\   AX BX MOV,              \ MOV AX,BX
\   AX $1234 MOVI,          \ MOV AX,1234      I: an immediate number
\   AX 4 [BP] MOV,          \ MOV AX,[BP+4]    n [BP]: memory at BP + n
\   $0106 [] AX MOV,        \ MOV [0106],AX    n []: memory at address n
\   0 [DI] BYTE 7 MOVI,     \ MOV BYTE PTR [DI],07
\   AX 1 SHL,  AX CL SHL,   \ SHL AX,1   SHL AX,CL
\   CS: AX 0 [BX] MOV,      \ MOV AX,CS:[BX]   a prefix comes first
\   REP, MOVSB,             \ REP MOVSB
\   L1 JNZ,                 \ JNZ to the label L1
\
\ asm/README.md explains the assembler step by step, with a table from
\ the Intel form to this form.
\
\ Only instructions of the 8086 are here: DEBUG knows no other, and the
\ code runs on each PC from the IBM PC (8088) on. So conditional jumps
\ reach only 127 bytes, and a shift goes by 1 or by CL.
\
\ The code goes to TARGET, a memory in the host. ORG ( ta -- ) starts
\ new code: ta is the address of its first byte in the target segment,
\ for example $0100 for a .COM program of DOS. THERE is the address of
\ the next byte; TDP holds the number of bytes so far. On a PC with
\ uCForth the code goes into the dictionary instead, where it runs
\ (IN-PLACE; the words CODE and END-CODE of lib/pc/asm.fs).
\
\ Labels: LABEL name marks THERE. A jump to a label before its place
\ gets its offset when LABEL gives the place. L1 to L9 are local labels;
\ a new named label starts a new scope for them, and NEW-SCOPE too (a
\ kernel starts one for each primitive). FORWARD name declares a
\ named label before its place. CHECK-LABELS stops with an error if a
\ jump waits for a label without place. A named label is a word in the
\ dictionary: IN-PLACE has only L1 to L9. Short jumps inside a word:
\ IF, ELSE, THEN, BEGIN, UNTIL, AGAIN, WHILE, REPEAT, with a condition
\ (Z NZ C NC ...).
\
\ The package runs with cells of 16 or 32 bits: a DOS computer can
\ assemble its own kernel. It needs the package TEXT. The encodings are
\ from the Intel manual (iAPX 86/88 User's Manual, 1981); the tests in
\ asm/test/8086.fs compare them with clang (".code16").

PACKAGE I8086  ALSO TEXT   \ lib/package.fs

\ ==== Target memory =====================================================

PUBLIC
$2000 CONSTANT TARGET-SIZE
0 VALUE TARGET                      \ the code, byte for byte: ORG makes it
VARIABLE TORG                       \ the address of the first byte in the target
PRIVATE
VARIABLE TDP0                       \ the number of bytes so far (ORG)
VARIABLE 'TDP                       \ the counter: TDP0, or the dictionary pointer (IN-PLACE)
TDP0 'TDP !
VARIABLE TBASE                      \ the place of the byte at TORG in this system
VARIABLE TLIMIT                     \ the bytes from TBASE on
VARIABLE PLACED                     \ true: IN-PLACE
PUBLIC
: TDP ( -- a )  'TDP @ ;            \ the counter: TDP @ is the number of bytes so far
: THERE ( -- ta )  TORG @ TDP @ +  $FFFF AND ;
PRIVATE
: >HOST ( ta -- a )   \ the place of the target address in this system
  TARGET PLACED @ OR 0= ABORT" no target memory: ORG first"   \ neither ORG nor IN-PLACE
  TORG @ -  PLACED @ 0= IF $FFFF AND THEN
  DUP TLIMIT @ U< 0= ABORT" address outside the target memory"  TBASE @ + ;
PUBLIC
: TC@ ( ta -- c )  >HOST C@ ;
: TC! ( c ta -- )  >HOST C! ;
: T@ ( ta -- x )  DUP TC@  SWAP 1+ TC@ 8 LSHIFT OR ;     \ 16 bits, the low byte first
: T! ( x ta -- )  2DUP TC!  SWAP 8 RSHIFT SWAP 1+ TC! ;
: TC, ( c -- )  THERE TC!  1 TDP +! ;
: T,  ( x -- )  DUP TC,  8 RSHIFT TC, ;
: TS, ( a u -- )  0 ?DO  DUP I + C@ TC,  LOOP  DROP ;   \ the bytes of a text
: TALIGN ( -- )  THERE 1 AND IF 0 TC, THEN ;              \ an even address

\ ==== Operands ==========================================================
\ An operand is a number with a mark: $4001 + kind * $100 + 2 * n. The
\ numbers are odd, so they never look like a label or an address in the
\ host. A plain number in the place of an operand is an error.
\ Kinds: 1 word register, 2 byte register, 3 segment register, 4 memory,
\ 5 memory with BYTE. n is the number of the register, or the field
\ r/m of the byte ModR/M (8: an address without register).

PRIVATE
: OPERAND? ( x -- f )  $F001 AND $4001 = ;
: KIND ( op -- k )  DUP OPERAND? 0= ABORT" an operand is missing"  8 RSHIFT 15 AND ;
: NUM ( op -- n )  2/ 15 AND ;
: R16? ( op -- f )  KIND 1 = ;
: R8? ( op -- f )  KIND 2 = ;
: REG? ( op -- f )  KIND 3 < ;           \ a register for data, 8 or 16 bits
: SREG? ( op -- f )  KIND 3 = ;
: MEM? ( op -- f )  KIND 3 > ;
: REGS ( kind "name" x8 -- )  8 0 DO  DUP I 2* + CONSTANT  LOOP DROP ;
PUBLIC
$4101 REGS AX CX DX BX SP BP SI DI
$4201 REGS AL CL DL BL AH CH DH BH
$4301 CONSTANT ES   $4303 CONSTANT CS   $4305 CONSTANT SS   $4307 CONSTANT DS

PRIVATE
VARIABLE DISP                             \ the displacement of the memory operand
: MEM: ( rm "name" -- )  CREATE ,  DOES> @  SWAP DISP !  2* $4401 + ;
PUBLIC
0 MEM: [BX+SI]   1 MEM: [BX+DI]   2 MEM: [BP+SI]   3 MEM: [BP+DI]
4 MEM: [SI]      5 MEM: [DI]      6 MEM: [BP]      7 MEM: [BX]
8 MEM: []                                 \ n []: the memory at the address n
: BYTE ( mem -- mem' )   \ the memory operand holds a byte, not a word
  DUP KIND 4 <> ABORT" BYTE needs a memory operand before it"  $100 + ;

PRIVATE
: W-BIT ( op -- 1 | 0 | -1 )   \ 1 a word, 0 a byte, -1 memory without size
  KIND  DUP 1 = OVER 3 = OR IF DROP 1 EXIT THEN
  DUP 2 = SWAP 5 = OR IF 0 EXIT THEN  -1 ;
: W1 ( op -- w )   \ one operand: memory without BYTE is a word
  W-BIT  DUP 0< IF DROP 1 THEN ;
: W2 ( op1 op2 -- w )   \ two operands: a register gives the size, both must agree
  W-BIT SWAP W-BIT                                    ( w2 w1 )
  2DUP AND 0< ABORT" one operand must be a register"
  2DUP 0< 0= SWAP 0< 0= AND IF  2DUP <> ABORT" the operands have different sizes"  THEN
  MAX ;

: FITS8? ( n -- f )   \ -128 to 127, as a 16-bit number
  $FFFF AND  DUP $80 U<  SWAP $FF7F U> OR ;

: MODRM, ( r op -- )   \ the byte ModR/M with the field reg = r, then the displacement
  SWAP 7 AND 3 LSHIFT SWAP                           ( r*8 op )
  DUP MEM? 0= IF  NUM 7 AND OR $C0 OR TC, EXIT  THEN
  NUM  DUP 8 = IF  DROP 6 OR TC,  DISP @ T, EXIT  THEN   \ [n]: mod 00, r/m 110, 16 bits
  DISP @ $FFFF AND 0=  OVER 6 <> AND IF  OR TC, EXIT  THEN   \ [BP] always has a displacement
  DISP @ FITS8? IF  OR $40 OR TC,  DISP @ TC, EXIT  THEN
  OR $80 OR TC,  DISP @ T, ;
: RM, ( op1 op2 opcode -- )   \ opcode, then ModR/M: reg = the number of op1, r/m = op2
  TC,  SWAP NUM SWAP MODRM, ;

\ ==== MOV ===============================================================

PUBLIC
: MOV, ( dst src -- )
  OVER SREG? IF                                      \ MOV sreg,r/m16
    OVER CS = ABORT" MOV to CS is not possible"
    DUP SREG? ABORT" MOV from a segment register to a segment register is not possible"
    DUP R8? ABORT" MOV to a segment register takes 16 bits"
    $8E RM, EXIT  THEN
  DUP SREG? IF                                       \ MOV r/m16,sreg
    OVER R8? ABORT" a segment register has 16 bits"
    SWAP $8C RM, EXIT  THEN
  2DUP W2 >R
  DUP MEM? IF                                        \ MOV reg,mem
    OVER NUM 0=  OVER NUM 8 = AND IF  NIP DROP  $A0 R> OR TC,  DISP @ T, EXIT  THEN
    $8A R> OR RM, EXIT  THEN
  OVER MEM? IF                                       \ MOV mem,reg
    OVER NUM 8 =  OVER NUM 0= AND IF  2DROP  $A2 R> OR TC,  DISP @ T, EXIT  THEN
  THEN
  SWAP $88 R> OR RM, ;                               \ MOV r/m,reg

: MOVI, ( dst n -- )   \ MOV dst,n
  OVER REG? IF
    SWAP DUP W1 >R  NUM $B0 OR  R@ 3 LSHIFT OR TC,
    R> IF T, ELSE TC, THEN  EXIT  THEN
  OVER MEM? 0= ABORT" MOVI, needs a register or memory"
  OVER W1 >R  SWAP  $C6 R@ OR TC,  0 SWAP MODRM,
  R> IF T, ELSE TC, THEN ;

\ ==== Arithmetic and logic ==============================================
\ n: 0 ADD, 1 OR, 2 ADC, 3 SBB, 4 AND, 5 SUB, 6 XOR, 7 CMP. The form
\ with I takes a number: AX 5 ADDI, is ADD AX,5.

PRIVATE
: ALU ( dst src n -- )
  >R  2DUP W2                                        ( dst src w )
  OVER MEM? IF  R> 8 * 2 + OR RM, EXIT  THEN         \ reg,mem
  R> 8 * OR  >R SWAP R> RM, ;                        \ r/m,reg
: ALUI ( dst n i -- )
  >R SWAP  DUP W1                                    ( n dst w )
  IF
    OVER FITS8? IF  $83 TC,  R> SWAP MODRM,  TC, EXIT  THEN
    DUP AX = IF  DROP  R> 8 * 5 + TC,  T, EXIT  THEN
    $81 TC,  R> SWAP MODRM,  T, EXIT
  THEN
  DUP AL = IF  DROP  R> 8 * 4 + TC,  TC, EXIT  THEN
  $80 TC,  R> SWAP MODRM,  TC, ;
: ALU: ( n "name" -- )  CREATE ,  DOES> @ ALU ;
: ALUI: ( n "name" -- )  CREATE ,  DOES> @ ALUI ;
PUBLIC
0 ALU: ADD,   1 ALU: OR,   2 ALU: ADC,   3 ALU: SBB,
4 ALU: AND,   5 ALU: SUB,  6 ALU: XOR,   7 ALU: CMP,
0 ALUI: ADDI,  1 ALUI: ORI,  2 ALUI: ADCI,  3 ALUI: SBBI,
4 ALUI: ANDI,  5 ALUI: SUBI, 6 ALUI: XORI,  7 ALUI: CMPI,

: TEST, ( op1 op2 -- )   \ the flags of op1 AND op2
  2DUP W2 >R  DUP MEM? 0= IF SWAP THEN              ( reg r/m )
  $84 R> OR RM, ;
: TESTI, ( op n -- )
  SWAP DUP W1 IF
    DUP AX = IF  DROP $A9 TC,  T, EXIT  THEN
    $F7 TC,  0 SWAP MODRM,  T, EXIT
  THEN
  DUP AL = IF  DROP $A8 TC,  TC, EXIT  THEN
  $F6 TC,  0 SWAP MODRM,  TC, ;

PRIVATE
: F6: ( n "name" -- )  CREATE ,  DOES> @  SWAP  DUP W1 $F6 OR TC,  MODRM, ;
PUBLIC
2 F6: NOT,   3 F6: NEG,   4 F6: MUL,   5 F6: IMUL,   6 F6: DIV,   7 F6: IDIV,

: INC, ( op -- )  DUP R16? IF  NUM $40 OR TC, EXIT  THEN  DUP W1 $FE OR TC,  0 SWAP MODRM, ;
: DEC, ( op -- )  DUP R16? IF  NUM $48 OR TC, EXIT  THEN  DUP W1 $FE OR TC,  1 SWAP MODRM, ;

\ Shifts and rotations: op 1 SHL, or op CL SHL, (the 8086 has no other count).
PRIVATE
: SHIFT: ( n "name" -- )  CREATE ,
  DOES> @ >R                                         ( op count )
  DUP CL = IF  DROP $D2  ELSE  1 <> ABORT" the 8086 shifts by 1 or by CL"  $D0  THEN
  OVER W1 OR TC,  R> SWAP MODRM, ;
PUBLIC
0 SHIFT: ROL,   1 SHIFT: ROR,   2 SHIFT: RCL,   3 SHIFT: RCR,
4 SHIFT: SHL,   5 SHIFT: SHR,   7 SHIFT: SAR,

\ ==== Other instructions ================================================

: PUSH, ( op -- )
  DUP R16? IF  NUM $50 OR TC, EXIT  THEN
  DUP SREG? IF  NUM 3 LSHIFT 6 OR TC, EXIT  THEN
  DUP MEM? OVER W1 AND 0= ABORT" PUSH, takes 16 bits"
  $FF TC,  6 SWAP MODRM, ;
: POP, ( op -- )
  DUP R16? IF  NUM $58 OR TC, EXIT  THEN
  DUP CS = ABORT" POP CS is not possible"
  DUP SREG? IF  NUM 3 LSHIFT 7 OR TC, EXIT  THEN
  DUP MEM? OVER W1 AND 0= ABORT" POP, takes 16 bits"
  $8F TC,  0 SWAP MODRM, ;

: XCHG, ( op1 op2 -- )
  2DUP W2 >R
  R@ IF
    OVER AX =  OVER R16? AND IF  NIP NUM $90 OR TC,  R> DROP EXIT  THEN
    DUP AX = >R  OVER R16? R> AND IF  DROP NUM $90 OR TC,  R> DROP EXIT  THEN
  THEN
  OVER MEM? IF SWAP THEN  $86 R> OR RM, ;           \ reg = the register

PRIVATE
: LOAD: ( opcode "name" -- )  CREATE ,
  DOES> @ >R  OVER R16? OVER MEM? AND 0= ABORT" needs a word register and memory"
  R> RM, ;
PUBLIC
$8D LOAD: LEA,   $C5 LOAD: LDS,   $C4 LOAD: LES,

: IN, ( AL|AX port|DX -- )   \ IN AL,60 is AL $60 IN, ; IN AX,DX is AX DX IN,
  SWAP  DUP AL = IF DROP 0 ELSE  AX <> ABORT" IN, takes AL or AX"  1 THEN
  OVER DX = IF  NIP $EC OR TC, EXIT  THEN
  $E4 OR TC,  TC, ;
: OUT, ( port|DX AL|AX -- )   \ OUT 43,AL is $43 AL OUT,
  DUP AL = IF DROP 0 ELSE  AX <> ABORT" OUT, takes AL or AX"  1 THEN
  OVER DX = IF  NIP $EE OR TC, EXIT  THEN
  $E6 OR TC,  TC, ;

: INT, ( n -- )  DUP 3 = IF  DROP $CC TC, EXIT  THEN  $CD TC,  TC, ;

PRIVATE
: OP: ( c "name" -- )  CREATE ,  DOES> @ TC, ;
PUBLIC
$A4 OP: MOVSB,   $A5 OP: MOVSW,   $A6 OP: CMPSB,   $A7 OP: CMPSW,   $AA OP: STOSB,
$AB OP: STOSW,   $AC OP: LODSB,   $AD OP: LODSW,   $AE OP: SCASB,   $AF OP: SCASW,
$F3 OP: REP,     $F3 OP: REPE,    $F3 OP: REPZ,    $F2 OP: REPNE,   $F2 OP: REPNZ,
$F8 OP: CLC,     $F9 OP: STC,     $F5 OP: CMC,     $FC OP: CLD,     $FD OP: STD,
$FA OP: CLI,     $FB OP: STI,     $98 OP: CBW,     $99 OP: CWD,     $90 OP: NOP,
$F4 OP: HLT,     $9F OP: LAHF,    $9E OP: SAHF,    $9C OP: PUSHF,   $9D OP: POPF,
$D7 OP: XLAT,    $C3 OP: RET,     $CB OP: RETF,    $CF OP: IRET,
$26 OP: ES:      $2E OP: CS:      $36 OP: SS:      $3E OP: DS:      \ segment prefixes

\ ==== Labels ============================================================
\ The body of a label word is its record: its address (UNKNOWN before
\ its place), its name token, and a flag, true for L1 to L9. A jump to a
\ label before its place goes to the list JUMPS; the place fixes it.
\ So a label word gives the address, or UNKNOWN and a note in JUMPS.

-1 CONSTANT UNKNOWN
PRIVATE
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
PUBLIC
: NEW-SCOPE ( -- )   \ L1 to L9 are free again; no jump may wait for them
  #JUMPS 0 DO  I JUMP @  DUP IF DUP LOCAL? IF UNPLACED THEN THEN  DROP  LOOP
  #LOCALS 0 DO  UNKNOWN  LOCALS I CELLS + @ !  LOOP ;
: CHECK-LABELS ( -- )   \ every label that a jump uses has its place
  #JUMPS 0 DO  I JUMP @ ?DUP IF UNPLACED THEN  LOOP ;
: ORG ( ta -- )   \ new code in TARGET; its first byte goes to the target address ta
  TARGET 0= IF  ALIGN HERE TARGET-SIZE ALLOT TO TARGET  THEN
  TORG !  TDP0 'TDP !  0 TDP0 !  TARGET TBASE !  TARGET-SIZE TLIMIT !  FALSE PLACED !
  TARGET TARGET-SIZE 0 FILL  JUMPS #JUMPS 2* CELLS 0 FILL  NEW-SCOPE ;
: IN-PLACE ( ta a limit -- )   \ new code in this system, where it runs
  \ The next byte goes to the address in the counter at a (the dictionary
  \ pointer), below limit; the CPU sees the byte at address x at ta + x.
  TLIMIT !  'TDP !  TORG !  0 TBASE !  TRUE PLACED !
  JUMPS #JUMPS 2* CELLS 0 FILL  NEW-SCOPE ;

\ ==== Jumps =============================================================
\ A jump is short (2 bytes, -128 to 127 from its end) or near (3 bytes,
\ 16 bits). The conditional jumps, LOOP and JCXZ of the 8086 are short
\ only. JMP, to a label before its place is near.

PRIVATE
: REL8 ( ta end -- c )   \ the offset from end, the address after the jump
  OVER UNKNOWN = IF 2DROP 0 EXIT THEN
  -  DUP FITS8? 0= ABORT" the jump is too far for 8 bits (127 bytes)"  255 AND ;
: REL16 ( ta end -- x )  OVER UNKNOWN = IF 2DROP 0 EXIT THEN  -  $FFFF AND ;
: SHORT, ( ta opcode -- )  SWAP  THERE 2 + REL8  SWAP TC, TC, ;
: J: ( opcode "name" -- )  CREATE ,  DOES> @ SHORT, ;
PUBLIC
$70 J: JO,    $71 J: JNO,   $72 J: JB,    $72 J: JC,    $73 J: JAE,   $73 J: JNC,
$74 J: JE,    $74 J: JZ,    $75 J: JNE,   $75 J: JNZ,   $76 J: JBE,   $77 J: JA,
$78 J: JS,    $79 J: JNS,   $7A J: JPE,   $7B J: JPO,   $7C J: JL,    $7D J: JGE,
$7E J: JLE,   $7F J: JG,
$E0 J: LOOPNZ,  $E0 J: LOOPNE,  $E1 J: LOOPZ,  $E1 J: LOOPE,  $E2 J: LOOP,  $E3 J: JCXZ,

: JMP, ( ta -- )   \ short when the label has its place and is near, else near
  DUP UNKNOWN <> IF  DUP THERE 2 + - FITS8? IF  $EB SHORT, EXIT  THEN  THEN
  $E9 TC,  THERE 2 + REL16 T, ;
: CALL, ( ta -- )  $E8 TC,  THERE 2 + REL16 T, ;
: JMP-IND, ( op -- )   \ JMP to the address in a register or in memory: JMP [DI+100]
  DUP R16? OVER MEM? OR 0= ABORT" JMP-IND, takes a word register or memory"  $FF TC,  4 SWAP MODRM, ;
: CALL-IND, ( op -- )
  DUP R16? OVER MEM? OR 0= ABORT" CALL-IND, takes a word register or memory"  $FF TC,  2 SWAP MODRM, ;

\ Far jumps and calls go to another segment: CS changes too. The far
\ address is two words, the offset first; in memory as in LDS and LES.
: JMP-FAR, ( seg off -- )   $EA TC,  T,  T, ;      \ JMP seg:off
: CALL-FAR, ( seg off -- )  $9A TC,  T,  T, ;      \ CALL seg:off; the code ends with RETF
: JMP-FAR-IND, ( mem -- )   \ JMP to the far address in memory: JMP FAR [BP-4]
  DUP MEM? 0= ABORT" JMP-FAR-IND, takes memory"  $FF TC,  5 SWAP MODRM, ;
: CALL-FAR-IND, ( mem -- )
  DUP MEM? 0= ABORT" CALL-FAR-IND, takes memory"  $FF TC,  3 SWAP MODRM, ;

PRIVATE
: PATCH ( dest ta -- )   \ the jump (or MOVI, reg) at ta goes to dest now
  DUP TC@                                              ( dest ta op )
  DUP $F0 AND $70 =  OVER $FC AND $E0 = OR  OVER $EB = OR IF
    DROP  TUCK 2 + REL8  SWAP 1+ TC! EXIT  THEN      \ short jumps
  DUP $E8 =  OVER $E9 = OR IF
    DROP  TUCK 3 + REL16  SWAP 1+ T! EXIT  THEN      \ CALL, near JMP,
  $F8 AND $B8 = IF  1+ T! EXIT  THEN                 \ MOVI, to a word register
  TRUE ABORT" a label before its place is only for a jump, CALL, or MOVI, to a word register" ;
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

\ ==== Structures ========================================================
\ IF, jumps over the code when the condition is false: Z IF, ... THEN,
\ runs the code when the flag Z is set. The jumps are short.

 0 CONSTANT O    1 CONSTANT NO    2 CONSTANT B    2 CONSTANT C
 3 CONSTANT AE   3 CONSTANT NC    4 CONSTANT E    4 CONSTANT Z
 5 CONSTANT NE   5 CONSTANT NZ    6 CONSTANT BE   7 CONSTANT A
 8 CONSTANT S    9 CONSTANT NS   10 CONSTANT PE  11 CONSTANT PO
12 CONSTANT L   13 CONSTANT GE   14 CONSTANT LE  15 CONSTANT G

: IF, ( cond -- orig )   THERE SWAP  1 XOR $70 + TC,  0 TC, ;
: THEN, ( orig -- )   THERE SWAP PATCH ;
: ELSE, ( orig1 -- orig2 )   THERE $EB TC, 0 TC,  SWAP THEN, ;
: BEGIN, ( -- dest )   THERE ;
: UNTIL, ( dest cond -- )   1 XOR $70 + SHORT, ;    \ back to dest while the condition is false
: AGAIN, ( dest -- )   JMP, ;
: WHILE, ( dest cond -- orig dest )   IF, SWAP ;
: REPEAT, ( orig dest -- )   AGAIN, THEN, ;

END-PACKAGE
