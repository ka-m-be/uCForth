\ x86.fs - package X86: an assembler for the Intel 386 and later (32 bit)
\
\ The same way as the assembler for the 8086 (asm/8086.fs): the operands
\ first, in the order of the Intel manual (the destination first), then
\ the instruction with a comma. The words are the Intel names:
\
\   EAX EBX MOV,              \ mov eax, ebx
\   EAX $12345678 MOVI,       \ mov eax, 12345678h     I: an immediate number
\   EAX 8 [EBX] MOV,          \ mov eax, [ebx+8]       n [EBX]: memory at EBX + n
\   $402000 [] EAX MOV,       \ mov [402000h], eax     n []: memory at address n
\   EAX 0 [ECX] [EDX*4] MOV,  \ mov eax, [ecx+edx*4]   an index register, times 1, 2, 4 or 8
\   0 [EDI] BYTE 7 MOVI,      \ mov byte ptr [edi], 7
\   EAX 3 SHL,  EAX CL SHL,   \ shl eax, 3   shl eax, cl
\   L1 JNZ,                   \ jnz to the label L1
\
\ asm/README.md explains the assembler step by step, with a table from
\ the Intel form to this form.
\
\ The instructions are those of the 386 that a kernel needs: registers
\ of 32 and 8 bits, no 16-bit registers, no floating point. The code
\ runs on each PC from the 386 on, and on each processor of today in
\ 32-bit mode (Windows from 95 to 11, Linux i386).
\
\ The code goes to TARGET, a memory in the host. ORG ( ta -- ) starts
\ new code: ta is the address of its first byte in the target, for
\ example $402000 in a program for Windows. THERE is the address of the
\ next byte; TDP holds the number of bytes so far.
\
\ Labels: LABEL name marks THERE. A jump to a label before its place
\ gets its offset when LABEL gives the place. L1 to L9 are local labels;
\ a new named label starts a new scope for them, and NEW-SCOPE too.
\ FORWARD name declares a named label before its place. CHECK-LABELS
\ stops with an error if a jump waits for a label without place. Short
\ jumps inside a routine: IF, ELSE, THEN, BEGIN, UNTIL, AGAIN, WHILE,
\ REPEAT, with a condition (Z NZ C NC ...).
\
\ The package needs cells of 32 bits, and the package TEXT. The
\ encodings are from the Intel manual (Intel 80386 Programmer's
\ Reference Manual, 1986); the tests in asm/test/x86.fs compare them
\ with clang.

PACKAGE X86  ALSO TEXT   \ lib/package.fs

: NEED-32 ( -- )   CELL 4 <> ABORT" the assembler X86 needs a host with 32-bit cells" ;
NEED-32

\ ==== Target memory =====================================================

PUBLIC
$8000 CONSTANT TARGET-SIZE
0 VALUE TARGET                      \ the code, byte for byte: ORG makes it
VARIABLE TORG                       \ the address of the first byte in the target
VARIABLE TDP                        \ the number of bytes so far
: THERE ( -- ta )  TORG @ TDP @ + ;
PRIVATE
: >HOST ( ta -- a )   \ the place of the target address in this system
  TARGET 0= ABORT" no target memory: ORG first"
  TORG @ -  DUP TARGET-SIZE U< 0= ABORT" address outside the target memory"  TARGET + ;
PUBLIC
: TC@ ( ta -- c )  >HOST C@ ;
: TC! ( c ta -- )  >HOST C! ;
: T@ ( ta -- x )   \ 32 bits, the low byte first
  0  4 0 DO  8 LSHIFT  OVER 3 I - + TC@ OR  LOOP  NIP ;
: T! ( x ta -- )   4 0 DO  2DUP TC!  1+ SWAP 8 RSHIFT SWAP  LOOP  2DROP ;
: TC, ( c -- )  THERE TC!  1 TDP +! ;
: TW, ( x -- )  DUP TC,  8 RSHIFT TC, ;                   \ 16 bits
: T,  ( x -- )  DUP TW,  16 RSHIFT TW, ;                  \ 32 bits
: TS, ( a u -- )  0 ?DO  DUP I + C@ TC,  LOOP  DROP ;   \ the bytes of a text
: TALIGN ( n -- )  BEGIN  THERE OVER 1- AND WHILE  0 TC,  REPEAT  DROP ;   \ n: 2, 4, 8 ...
-1 CONSTANT UNKNOWN                 \ the address of a label before its place

\ ==== Operands ==========================================================
\ An operand is a number with a mark: $55000001 + kind * $10000 + 2 * n.
\ So a plain number in the place of an operand is an error. Kinds:
\ 1 register of 32 bits, 2 register of 8 bits, 4 memory, 5 memory with
\ BYTE. n is the number of the register; for memory the base register,
\ or 8: no base register (an address alone).

PRIVATE
: OPERAND? ( x -- f )  $FF00F001 AND $55000001 = ;
: KIND ( op -- k )  DUP OPERAND? 0= ABORT" an operand is missing"  16 RSHIFT 15 AND ;
: NUM ( op -- n )  2/ 15 AND ;
: R32? ( op -- f )  KIND 1 = ;
: R8? ( op -- f )  KIND 2 = ;
: REG? ( op -- f )  KIND 3 < ;
: MEM? ( op -- f )  KIND 3 > ;
: REGS ( kind "name" x8 -- )  8 0 DO  DUP I 2* + CONSTANT  LOOP DROP ;
PUBLIC
$55010001 REGS EAX ECX EDX EBX ESP EBP ESI EDI
$55020001 REGS AL CL DL BL AH CH DH BH

PRIVATE
VARIABLE DISP                             \ the displacement of the memory operand
VARIABLE INDEX                            \ its index register, or -1
VARIABLE SCALE                            \ the factor of the index: 0 1 2 3 for 1 2 4 8
: MEM: ( base "name" -- )  CREATE ,  DOES> @  SWAP DISP !  -1 INDEX !  2* $55040001 + ;
PUBLIC
0 MEM: [EAX]   1 MEM: [ECX]   2 MEM: [EDX]   3 MEM: [EBX]
4 MEM: [ESP]   5 MEM: [EBP]   6 MEM: [ESI]   7 MEM: [EDI]
8 MEM: []                                 \ n []: the memory at the address n
: BYTE ( mem -- mem' )   \ the memory operand holds a byte, not 32 bits
  DUP KIND 4 <> ABORT" BYTE needs a memory operand before it"  $10000 + ;
PRIVATE
: INDEX: ( reg scale "name" -- )   \ [EAX*4]: the index for the memory operand before it
  CREATE , ,
  DOES>  OVER MEM? 0= ABORT" an index needs a memory operand before it"
    INDEX @ 0< 0= ABORT" a memory operand has one index register"
    DUP @ SCALE !  CELL+ @ INDEX ! ;
: INDEXES ( reg "name" x4 -- )  4 0 DO  DUP I INDEX:  LOOP DROP ;
PUBLIC
0 INDEXES [EAX*1] [EAX*2] [EAX*4] [EAX*8]
1 INDEXES [ECX*1] [ECX*2] [ECX*4] [ECX*8]
2 INDEXES [EDX*1] [EDX*2] [EDX*4] [EDX*8]
3 INDEXES [EBX*1] [EBX*2] [EBX*4] [EBX*8]
5 INDEXES [EBP*1] [EBP*2] [EBP*4] [EBP*8]
6 INDEXES [ESI*1] [ESI*2] [ESI*4] [ESI*8]
7 INDEXES [EDI*1] [EDI*2] [EDI*4] [EDI*8]

PRIVATE
: W-BIT ( op -- 1 | 0 | -1 )   \ 1 32 bits, 0 a byte, -1 memory without size
  KIND  DUP 1 = IF DROP 1 EXIT THEN
  DUP 2 = SWAP 5 = OR IF 0 EXIT THEN  -1 ;
: W1 ( op -- w )   \ one operand: memory without BYTE has 32 bits
  W-BIT  DUP 0< IF DROP 1 THEN ;
: W2 ( op1 op2 -- w )   \ two operands: a register gives the size, both must agree
  W-BIT SWAP W-BIT                                    ( w2 w1 )
  2DUP AND 0< ABORT" one operand must be a register"
  2DUP 0< 0= SWAP 0< 0= AND IF  2DUP <> ABORT" the operands have different sizes"  THEN
  MAX ;

: FITS8? ( n -- f )  128 + 256 U< ;      \ -128 to 127

: SIB, ( base -- )   SCALE @ 6 LSHIFT  INDEX @ 3 LSHIFT OR  OR TC, ;
: DISP, ( mod -- )   \ the displacement after ModR/M and SIB: none, 8 or 32 bits
  DUP $40 = IF  DISP @ TC,  THEN  $80 = IF  DISP @ T,  THEN ;
: MODRM, ( r op -- )   \ the byte ModR/M with the field reg = r, then SIB and displacement
  SWAP 7 AND 3 LSHIFT SWAP                           ( r*8 op )
  DUP MEM? 0= IF  NUM 7 AND OR $C0 OR TC, EXIT  THEN
  NUM  DUP 8 = IF                                    \ no base register
    DROP  INDEX @ 0< IF  5 OR TC,  DISP @ T, EXIT  THEN   \ [n]: mod 00, r/m 101
    4 OR TC,  5 SIB,  DISP @ T, EXIT  THEN              \ [n + index*s]: SIB, base 101
  DUP >R  5 <>  DISP @ 0= AND IF  0                  \ no displacement ([EBP] needs one)
  ELSE  DISP @ FITS8? IF $40 ELSE $80 THEN  THEN     ( r*8 mod )  ( R: base )
  TUCK OR                                            ( mod modr )
  INDEX @ 0<  R@ 4 <> AND IF  R> OR TC,  DISP, EXIT  THEN   \ the base alone
  4 OR TC,  INDEX @ 0< IF  4 INDEX !  0 SCALE !  THEN  R> SIB,  DISP, ;   \ SIB: [ESP] or an index
: RM, ( op1 op2 opcode -- )   \ opcode, then ModR/M: reg = the number of op1, r/m = op2
  TC,  SWAP NUM SWAP MODRM, ;
: IMM, ( n w -- )   IF T, ELSE TC, THEN ;

\ ==== MOV ===============================================================

PUBLIC
: MOV, ( dst src -- )
  2DUP W2 >R
  DUP MEM? IF                                        \ MOV reg,mem
    OVER NUM 0=  OVER NUM 8 = AND  INDEX @ 0< AND IF
      NIP DROP  $A0 R> OR TC,  DISP @ T, EXIT  THEN   \ MOV AL/EAX,[n]
    $8A R> OR RM, EXIT  THEN
  OVER MEM? IF                                       \ MOV mem,reg
    OVER NUM 8 =  OVER NUM 0= AND  INDEX @ 0< AND IF
      2DROP  $A2 R> OR TC,  DISP @ T, EXIT  THEN     \ MOV [n],AL/EAX
  THEN
  SWAP $88 R> OR RM, ;                               \ MOV r/m,reg

: MOVI, ( dst n -- )   \ MOV dst,n
  OVER REG? IF
    SWAP DUP W1 >R  NUM  R@ IF $B8 ELSE $B0 THEN OR TC,  R> IMM, EXIT  THEN
  OVER MEM? 0= ABORT" MOVI, needs a register or memory"
  OVER W1 >R  SWAP  $C6 R@ OR TC,  0 SWAP MODRM,  R> IMM, ;

PRIVATE
: EXTEND: ( opcode "name" -- )   \ MOVZX, MOVSX: a register of 32 bits from a byte
  CREATE ,
  DOES> @ >R  OVER R32? 0= ABORT" needs a register of 32 bits first"
    DUP W-BIT 0<> ABORT" needs a byte register or BYTE memory second"
    $0F TC,  R> RM, ;
PUBLIC
$B6 EXTEND: MOVZX,   $BE EXTEND: MOVSX,

\ ==== Arithmetic and logic ==============================================
\ n: 0 ADD, 1 OR, 2 ADC, 3 SBB, 4 AND, 5 SUB, 6 XOR, 7 CMP. The form
\ with I takes a number: EAX 5 ADDI, is ADD EAX,5.

PRIVATE
: ALU ( dst src n -- )
  >R  2DUP W2                                        ( dst src w )
  OVER MEM? IF  R> 8 * 2 + OR RM, EXIT  THEN         \ reg,mem
  R> 8 * OR  >R SWAP R> RM, ;                        \ r/m,reg
: ALUI ( dst n i -- )
  >R SWAP  DUP W1                                    ( n dst w )
  IF
    OVER FITS8? IF  $83 TC,  R> SWAP MODRM,  TC, EXIT  THEN
    DUP EAX = IF  DROP  R> 8 * 5 + TC,  T, EXIT  THEN
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
    DUP EAX = IF  DROP $A9 TC,  T, EXIT  THEN
    $F7 TC,  0 SWAP MODRM,  T, EXIT
  THEN
  DUP AL = IF  DROP $A8 TC,  TC, EXIT  THEN
  $F6 TC,  0 SWAP MODRM,  TC, ;

PRIVATE
: F6: ( n "name" -- )  CREATE ,  DOES> @  SWAP  DUP W1 $F6 OR TC,  MODRM, ;
PUBLIC
2 F6: NOT,   3 F6: NEG,   4 F6: MUL,   5 F6: IMUL,   6 F6: DIV,   7 F6: IDIV,

: INC, ( op -- )  DUP R32? IF  NUM $40 OR TC, EXIT  THEN  DUP W1 $FE OR TC,  0 SWAP MODRM, ;
: DEC, ( op -- )  DUP R32? IF  NUM $48 OR TC, EXIT  THEN  DUP W1 $FE OR TC,  1 SWAP MODRM, ;

\ Shifts and rotations: op n SHL, (n a number) or op CL SHL,.
PRIVATE
: SHIFT: ( n "name" -- )  CREATE ,
  DOES> @ >R                                         ( op count )
  DUP CL = IF  DROP  DUP W1 $D2 OR TC,  R> SWAP MODRM, EXIT  THEN
  DUP 1 = IF  DROP  DUP W1 $D0 OR TC,  R> SWAP MODRM, EXIT  THEN
  DUP 32 U< 0= ABORT" a shift takes CL or a number from 0 to 31"
  >R  DUP W1 $C0 OR TC,  R> R> ROT MODRM,  TC, ;
PUBLIC
0 SHIFT: ROL,   1 SHIFT: ROR,   2 SHIFT: RCL,   3 SHIFT: RCR,
4 SHIFT: SHL,   5 SHIFT: SHR,   7 SHIFT: SAR,

\ ==== Other instructions ================================================

: PUSH, ( op -- )
  DUP R32? IF  NUM $50 OR TC, EXIT  THEN
  DUP MEM? OVER W1 AND 0= ABORT" PUSH, takes 32 bits"
  $FF TC,  6 SWAP MODRM, ;
: PUSHI, ( n -- )   \ PUSH n: 8 bits with sign when it fits, else 32 bits (and for UNKNOWN)
  DUP UNKNOWN = IF  $68 TC,  T, EXIT  THEN
  DUP FITS8? IF  $6A TC,  TC, EXIT  THEN  $68 TC,  T, ;
: POP, ( op -- )
  DUP R32? IF  NUM $58 OR TC, EXIT  THEN
  DUP MEM? OVER W1 AND 0= ABORT" POP, takes 32 bits"
  $8F TC,  0 SWAP MODRM, ;

: XCHG, ( op1 op2 -- )
  2DUP W2 >R
  R@ IF
    OVER EAX =  OVER R32? AND IF  NIP NUM $90 OR TC,  R> DROP EXIT  THEN
    DUP EAX = >R  OVER R32? R> AND IF  DROP NUM $90 OR TC,  R> DROP EXIT  THEN
  THEN
  OVER MEM? IF SWAP THEN  $86 R> OR RM, ;           \ reg = the register

: LEA, ( reg mem -- )
  OVER R32? OVER MEM? AND 0= ABORT" LEA, needs a register of 32 bits and memory"
  $8D RM, ;

: SETCC, ( op cond -- )   \ the byte op gets 1 when the condition holds, else 0
  $0F TC,  $90 OR TC,  DUP W1 ABORT" SETCC, needs a byte register or BYTE memory"
  0 SWAP MODRM, ;

: INT, ( n -- )  DUP 3 = IF  DROP $CC TC, EXIT  THEN  $CD TC,  TC, ;
: RETN, ( n -- )  $C2 TC,  TW, ;                    \ RET n: return and drop n bytes of arguments

PRIVATE
: OP: ( c "name" -- )  CREATE ,  DOES> @ TC, ;
PUBLIC
$A4 OP: MOVSB,   $A5 OP: MOVSD,   $A6 OP: CMPSB,   $AA OP: STOSB,   $AB OP: STOSD,
$AC OP: LODSB,   $AD OP: LODSD,   $AE OP: SCASB,
$F3 OP: REP,     $F3 OP: REPE,    $F3 OP: REPZ,    $F2 OP: REPNE,   $F2 OP: REPNZ,
$F8 OP: CLC,     $F9 OP: STC,     $F5 OP: CMC,     $FC OP: CLD,     $FD OP: STD,
$FA OP: CLI,     $FB OP: STI,     $99 OP: CDQ,     $90 OP: NOP,     $F4 OP: HLT,
$9C OP: PUSHFD,  $9D OP: POPFD,   $60 OP: PUSHAD,  $61 OP: POPAD,   $C3 OP: RET,
$F0 OP: LOCK,

\ ==== Labels ============================================================
\ The body of a label word is its record: its address (UNKNOWN before
\ its place), its name token, and a flag, true for L1 to L9. A jump to a
\ label before its place goes to the list JUMPS; the place fixes it.
\ So a label word gives the address, or UNKNOWN and a note in JUMPS.

PRIVATE
128 CONSTANT #JUMPS
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
  TORG !  0 TDP !  TARGET TARGET-SIZE 0 FILL  JUMPS #JUMPS 2* CELLS 0 FILL  NEW-SCOPE ;

\ ==== Jumps =============================================================
\ A jump is short (8 bits: -128 to 127 from its end) or near (32 bits).
\ A jump to a label with its place takes the short form when it fits;
\ a jump to a label before its place is near. LOOP, and JECXZ, are short
\ only.

PRIVATE
: REL8 ( ta end -- c )   \ the offset from end, the address after the jump
  OVER UNKNOWN = IF 2DROP 0 EXIT THEN
  -  DUP FITS8? 0= ABORT" the jump is too far for 8 bits (127 bytes)"  255 AND ;
: REL32 ( ta end -- x )  OVER UNKNOWN = IF 2DROP 0 EXIT THEN  - ;
: SHORT? ( ta size -- f )   \ the label has its place, and a short jump reaches it
  OVER UNKNOWN = IF 2DROP FALSE EXIT THEN  THERE + - FITS8? ;
: SHORT, ( ta opcode -- )  SWAP  THERE 2 + REL8  SWAP TC, TC, ;
: JCC ( ta cond -- )
  OVER 2 SHORT? IF  $70 OR SHORT, EXIT  THEN
  $0F TC,  $80 OR TC,  THERE 4 + REL32 T, ;
: J: ( cond "name" -- )  CREATE ,  DOES> @ JCC ;
: S: ( opcode "name" -- )  CREATE ,  DOES> @ SHORT, ;
PUBLIC
 0 J: JO,    1 J: JNO,   2 J: JB,    2 J: JC,    3 J: JAE,   3 J: JNC,
 4 J: JE,    4 J: JZ,    5 J: JNE,   5 J: JNZ,   6 J: JBE,   7 J: JA,
 8 J: JS,    9 J: JNS,  10 J: JPE,  11 J: JPO,  12 J: JL,   13 J: JGE,
14 J: JLE,  15 J: JG,
$E0 S: LOOPNZ,  $E0 S: LOOPNE,  $E1 S: LOOPZ,  $E1 S: LOOPE,  $E2 S: LOOP,  $E3 S: JECXZ,

: JMP, ( ta -- )   \ short when the label has its place and is near, else near
  DUP 2 SHORT? IF  $EB SHORT, EXIT  THEN
  $E9 TC,  THERE 4 + REL32 T, ;
: CALL, ( ta -- )  $E8 TC,  THERE 4 + REL32 T, ;
: JMP-IND, ( op -- )   \ jump to the address in a register or in memory: jmp [ecx+eax*4]
  DUP R32? OVER MEM? OR 0= ABORT" JMP-IND, takes a register of 32 bits or memory"
  $FF TC,  4 SWAP MODRM, ;
: CALL-IND, ( op -- )   \ call the address in a register or in memory: call [402000h]
  DUP R32? OVER MEM? OR 0= ABORT" CALL-IND, takes a register of 32 bits or memory"
  $FF TC,  2 SWAP MODRM, ;

PRIVATE
: PATCH ( dest ta -- )   \ the jump (or MOVI, reg, or PUSHI,) at ta goes to dest now
  DUP TC@                                              ( dest ta op )
  DUP $F0 AND $70 =  OVER $FC AND $E0 = OR  OVER $EB = OR IF
    DROP  TUCK 2 + REL8  SWAP 1+ TC! EXIT  THEN      \ short jumps
  DUP $0F = IF
    DROP  TUCK 6 + REL32  SWAP 2 + T! EXIT  THEN     \ near conditional jumps
  DUP $E8 =  OVER $E9 = OR IF
    DROP  TUCK 5 + REL32  SWAP 1+ T! EXIT  THEN      \ CALL, near JMP,
  DUP $F8 AND $B8 =  SWAP $68 = OR IF  1+ T! EXIT  THEN   \ MOVI, to a register, PUSHI,
  TRUE ABORT" a label before its place is only for a jump, CALL, MOVI, to a register or PUSHI," ;
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
  DROP >IN !  NEW-SCOPE  THERE FALSE (LABEL) DROP ;
: FORWARD ( "name" -- )  UNKNOWN FALSE (LABEL) DROP ;

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
