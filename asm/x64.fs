\ x64.fs - package X64: an assembler for x86-64 (the 64-bit mode)
\
\ The same way as the assembler for the 386 (asm/x86.fs): the operands
\ first, in the order of the Intel manual (the destination first), then
\ the instruction with a comma. The words are the Intel names:
\
\   RAX RBX MOV,                \ mov rax, rbx          registers of 64 bits
\   EAX R9D MOV,                \ mov eax, r9d          32 bits: the upper half of RAX becomes 0
\   EAX 8 [R15] [R12*1] MOV,    \ mov eax, [r15+r12+8]  n [R15]: memory at R15 + n
\   RAX $2000 [RIP] LEA,        \ lea rax, [rip+...]    n [RIP]: memory at the address n
\   0 [RDI] QWORD 7 MOVI,       \ mov qword ptr [rdi], 7
\   0 [RDI] BYTE 7 MOVI,        \ mov byte ptr [rdi], 7
\   L1 JNZ,                     \ jnz to the label L1
\
\ asm/README.md explains the assembler step by step, with a table from
\ the Intel form to this form.
\
\ The registers: RAX to R15 have 64 bits; EAX to R15D are their lower 32
\ bits; AL to R15B their lower 8 bits, and AH CH DH BH the second byte of
\ RAX to RBX. An instruction that writes a register of 32 bits sets the
\ upper half of the register of 64 bits to 0. Addresses have 64 bits:
\ the base and the index of memory are registers of 64 bits.
\
\ Numbers: the host has cells of 32 bits, so a number in an instruction
\ has at most 32 bits. With an operand of 64 bits the processor extends
\ it with its sign: RAX -1 MOVI, sets all 64 bits; EAX -1 MOVI, sets the
\ lower 32 bits and clears the upper ones.
\
\ n [RIP]: the processor adds the address of the next instruction, so
\ the assembler puts n minus that address into the instruction. Code
\ that reaches its data only this way can lie at any address (macOS and
\ UEFI load a program at an address that changes). The label must have
\ its place before (no jump waits for it).
\
\ The instructions are those that a kernel needs, in 64-bit mode: no
\ registers of 16 bits, no floating point, no vector registers.
\
\ The code goes to TARGET, a memory in the host. ORG ( ta -- ) starts
\ new code: ta is the address of its first byte in the target. THERE is
\ the address of the next byte; TDP holds the number of bytes so far.
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
\ encodings are from the Intel manual (Intel 64 and IA-32 Architectures
\ Software Developer's Manual, volume 2); the tests in asm/test/x64.fs
\ compare them with clang.

PACKAGE X64  ALSO TEXT   \ lib/package.fs

: NEED-32 ( -- )   CELL 4 <> ABORT" the assembler X64 needs a host with 32-bit cells" ;
NEED-32

\ ==== Target memory =====================================================

PUBLIC
$10000 CONSTANT TARGET-SIZE
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
\ 1 register of 64 bits, 2 of 32 bits, 3 of 8 bits, 4 memory, 5 memory
\ with BYTE, 6 memory with QWORD. n is the number of the register, 0 to
\ 15 (AH CH DH BH: 20 to 23); for memory the base register, or 16: no
\ base register (an address alone), or 17: RIP.

PRIVATE
: OPERAND? ( x -- f )  $FF00F001 AND $55000001 = ;
: KIND ( op -- k )  DUP OPERAND? 0= ABORT" an operand is missing"  16 RSHIFT 15 AND ;
: NUM ( op -- n )  2/ 31 AND ;
: R64? ( op -- f )  KIND 1 = ;
: R32? ( op -- f )  KIND 2 = ;
: R8? ( op -- f )  KIND 3 = ;
: REG? ( op -- f )  KIND 4 < ;
: MEM? ( op -- f )  KIND 3 > ;
: REGS ( kind "name" x16 -- )  16 0 DO  DUP I 2* + CONSTANT  LOOP DROP ;
PUBLIC
$55010001 REGS RAX RCX RDX RBX RSP RBP RSI RDI R8 R9 R10 R11 R12 R13 R14 R15
$55020001 REGS EAX ECX EDX EBX ESP EBP ESI EDI R8D R9D R10D R11D R12D R13D R14D R15D
$55030001 REGS AL CL DL BL SPL BPL SIL DIL R8B R9B R10B R11B R12B R13B R14B R15B
$55030029 CONSTANT AH   $5503002B CONSTANT CH   $5503002D CONSTANT DH   $5503002F CONSTANT BH

PRIVATE
VARIABLE DISP                             \ the displacement of the memory operand
VARIABLE INDEX                            \ its index register, or -1
VARIABLE SCALE                            \ the factor of the index: 0 1 2 3 for 1 2 4 8
: MEM: ( base "name" -- )  CREATE ,  DOES> @  SWAP DISP !  -1 INDEX !  2* $55040001 + ;
: MEMS ( "name" x16 -- )  16 0 DO  I MEM:  LOOP ;
PUBLIC
MEMS [RAX] [RCX] [RDX] [RBX] [RSP] [RBP] [RSI] [RDI] [R8] [R9] [R10] [R11] [R12] [R13] [R14] [R15]
16 MEM: []                                \ n []: the memory at the address n
17 MEM: [RIP]                             \ n [RIP]: the same, relative to the next instruction
: BYTE ( mem -- mem' )   \ the memory operand holds a byte, not 32 bits
  DUP KIND 4 <> ABORT" BYTE needs a memory operand before it"  $10000 + ;
: QWORD ( mem -- mem' )   \ the memory operand holds 64 bits, not 32
  DUP KIND 4 <> ABORT" QWORD needs a memory operand before it"  $20000 + ;
PRIVATE
: INDEX: ( reg scale "name" -- )   \ [RAX*4]: the index for the memory operand before it
  CREATE , ,
  DOES>  OVER MEM? 0= ABORT" an index needs a memory operand before it"
    OVER NUM 17 = ABORT" [RIP] takes no index"
    INDEX @ 0< 0= ABORT" a memory operand has one index register"
    DUP @ SCALE !  CELL+ @ INDEX ! ;
: INDEXES ( reg "name" x4 -- )  4 0 DO  DUP I INDEX:  LOOP DROP ;
PUBLIC
 0 INDEXES [RAX*1] [RAX*2] [RAX*4] [RAX*8]
 1 INDEXES [RCX*1] [RCX*2] [RCX*4] [RCX*8]
 2 INDEXES [RDX*1] [RDX*2] [RDX*4] [RDX*8]
 3 INDEXES [RBX*1] [RBX*2] [RBX*4] [RBX*8]
 5 INDEXES [RBP*1] [RBP*2] [RBP*4] [RBP*8]
 6 INDEXES [RSI*1] [RSI*2] [RSI*4] [RSI*8]
 7 INDEXES [RDI*1] [RDI*2] [RDI*4] [RDI*8]
 8 INDEXES [R8*1]  [R8*2]  [R8*4]  [R8*8]
 9 INDEXES [R9*1]  [R9*2]  [R9*4]  [R9*8]
10 INDEXES [R10*1] [R10*2] [R10*4] [R10*8]
11 INDEXES [R11*1] [R11*2] [R11*4] [R11*8]
12 INDEXES [R12*1] [R12*2] [R12*4] [R12*8]
13 INDEXES [R13*1] [R13*2] [R13*4] [R13*8]
14 INDEXES [R14*1] [R14*2] [R14*4] [R14*8]
15 INDEXES [R15*1] [R15*2] [R15*4] [R15*8]

\ ==== Sizes and the REX prefix ==========================================
\ The size of an operand: 0 a byte, 1 32 bits, 2 64 bits, -1 memory
\ without BYTE or QWORD (the other operand gives the size; alone it has
\ 32 bits). An opcode for bytes becomes the one for 32 and 64 bits with
\ +1 (OW); 64 bits need the bit W of the REX prefix.
\
\ The REX prefix ($40 to $4F) comes right before the opcode: bit 8 W (64
\ bits), 4 R (the field reg is R8 to R15), 2 X (the index is R8 to R15),
\ 1 B (r/m, the base, or the register in the opcode is R8 to R15). SPL
\ BPL SIL DIL need a REX prefix, also $40 alone; AH CH DH BH cannot go
\ with one.

PRIVATE
: SIZE ( op -- s )
  KIND  DUP 1 = IF DROP 2 EXIT THEN  DUP 2 = IF DROP 1 EXIT THEN
  DUP 3 =  OVER 5 = OR IF DROP 0 EXIT THEN  6 = IF 2 EXIT THEN  -1 ;
: S1 ( op -- s )   \ one operand: memory without size has 32 bits
  SIZE  DUP 0< IF DROP 1 THEN ;
: S2 ( op1 op2 -- s )   \ two operands: a register gives the size, both must agree
  SIZE SWAP SIZE                                      ( s2 s1 )
  2DUP AND 0< ABORT" one operand must be a register"
  2DUP 0< 0= SWAP 0< 0= AND IF  2DUP <> ABORT" the operands have different sizes"  THEN
  MAX ;
: OW ( opc s -- opc' )   IF 1+ THEN ;
: ISIZE ( s -- n )   IF 4 ELSE 1 THEN ;   \ the bytes of a number: 4 also for 64 bits
: IMM, ( n s -- )   IF T, ELSE TC, THEN ;
: FITS8? ( n -- f )  128 + 256 U< ;      \ -128 to 127

VARIABLE RX      \ the bits of the REX prefix
VARIABLE RX!     \ an operand needs a REX prefix (SPL BPL SIL DIL)
VARIABLE NO-RX   \ an operand forbids it (AH CH DH BH)
VARIABLE FOLLOW  \ the bytes of the instruction after the displacement (for [RIP])
VARIABLE IMM     \ the number of the instruction

: RX+ ( bits -- )   RX @ OR RX ! ;
: BYTE-REG ( op -- )   \ note what SPL to DIL and AH to BH need
  DUP R8? 0= IF DROP EXIT THEN
  NUM  DUP 4 8 WITHIN IF RX! ON! THEN  19 > IF NO-RX ON! THEN ;
: EXT? ( n -- f )   8 AND 0<> ;          \ R8 to R15: the fourth bit goes to REX
: REG-RX ( op -- )   \ the register op is in the field reg
  DUP BYTE-REG  NUM EXT? IF 4 RX+ THEN ;
: RM-RX ( op -- )   \ op is in the field r/m: a register, or the base and index of memory
  DUP MEM? IF
    NUM  DUP 16 < IF  EXT? IF 1 RX+ THEN  ELSE DROP THEN
    INDEX @ 0< 0= IF  INDEX @ EXT? IF 2 RX+ THEN  THEN  EXIT
  THEN
  DUP BYTE-REG  NUM EXT? IF 1 RX+ THEN ;
: PREFIX, ( s -- )   \ the REX prefix, if the instruction needs one; s 2: 64 bits
  2 = IF 8 RX+ THEN
  RX @ 0<>  RX! @ OR IF
    NO-RX @ ABORT" AH CH DH BH do not go with a REX prefix (R8-R15, SPL-DIL, 64 bits)"
    RX @ $40 OR TC,
  THEN  0 RX !  RX! OFF!  NO-RX OFF! ;
: OPC, ( opc -- )   \ one byte, or two: $0F05 is $0F, then 5
  DUP $FF > IF  DUP 8 RSHIFT TC,  THEN  TC, ;

: SIB, ( base -- )   SCALE @ 6 LSHIFT  INDEX @ 7 AND 3 LSHIFT OR  OR TC, ;
: DISP, ( mod -- )   \ the displacement after ModR/M and SIB: none, 8 or 32 bits
  DUP $40 = IF  DISP @ TC,  THEN  $80 = IF  DISP @ T,  THEN ;
: MODRM, ( r op -- )   \ the byte ModR/M with the field reg = r (0-7), then SIB and displacement
  SWAP 7 AND 3 LSHIFT SWAP                           ( r*8 op )
  DUP MEM? 0= IF  NUM 7 AND OR $C0 OR TC, EXIT  THEN
  NUM  DUP 17 = IF                                   \ [RIP]: mod 00, r/m 101
    DROP 5 OR TC,
    DISP @ UNKNOWN = ABORT" [RIP] needs a label with its place"
    DISP @  THERE 4 + FOLLOW @ +  -  T, EXIT  THEN
  DUP 16 = IF                                        \ no base register: SIB, base 101
    DROP 4 OR TC,  INDEX @ 0< IF  4 INDEX !  0 SCALE !  THEN  5 SIB,  DISP @ T, EXIT  THEN
  7 AND  DUP >R  5 <>  DISP @ 0= AND IF  0           \ no displacement ([RBP] [R13] need one)
  ELSE  DISP @ FITS8? IF $40 ELSE $80 THEN  THEN     ( r*8 mod )  ( R: base )
  TUCK OR                                            ( mod modr )
  INDEX @ 0<  R@ 4 <> AND IF  R> OR TC,  DISP, EXIT  THEN   \ the base alone
  4 OR TC,  INDEX @ 0< IF  4 INDEX !  0 SCALE !  THEN  R> SIB,  DISP, ;   \ SIB: [RSP] [R12] or an index
: RM, ( r op s opc -- )   \ REX, opcode, ModR/M: r a register (the field reg) or 0-7, op in r/m
  >R >R  OVER OPERAND? IF  OVER REG-RX  THEN  DUP RM-RX
  R> PREFIX,  R> OPC,
  SWAP DUP OPERAND? IF NUM THEN  SWAP MODRM,  0 FOLLOW ! ;
: OPREG, ( reg s opc -- )   \ REX, then the opcode with the register in its low 3 bits
  >R >R  DUP RM-RX  R> PREFIX,  R> SWAP NUM 7 AND OR TC, ;

\ ==== MOV ===============================================================

PUBLIC
: MOV, ( dst src -- )
  2DUP S2 >R
  DUP MEM? IF  R@ $8A R> OW RM, EXIT  THEN          \ MOV reg,mem
  SWAP  R@ $88 R> OW RM, ;                           \ MOV r/m,reg

: MOVI, ( dst n -- )   \ MOV dst,n
  SWAP  DUP REG? IF                                  ( n reg )
    DUP R64? IF  0 SWAP 2 $C7 RM,  T, EXIT  THEN     \ 64 bits: the number with its sign
    DUP SIZE >R  R@ $B0 R@ IF 8 + THEN OPREG,  R> IMM, EXIT
  THEN
  DUP MEM? 0= ABORT" MOVI, needs a register or memory"
  DUP S1 >R  R@ ISIZE FOLLOW !                       ( n mem )  ( R: s )
  0 SWAP R@ $C6 R@ OW RM,  R> IMM, ;

PRIVATE
: EXTEND: ( opcode "name" -- )   \ MOVZX, MOVSX: a register of 32 or 64 bits from a byte
  CREATE ,
  DOES> @ >R  OVER DUP R32? SWAP R64? OR 0= ABORT" needs a register of 32 or 64 bits first"
    DUP SIZE ABORT" needs a byte register or BYTE memory second"
    OVER SIZE R> RM, ;
PUBLIC
$0FB6 EXTEND: MOVZX,   $0FBE EXTEND: MOVSX,
: MOVSXD, ( reg op -- )   \ a register of 64 bits from 32 bits, with the sign
  OVER R64? 0= ABORT" MOVSXD, needs a register of 64 bits first"
  DUP SIZE  DUP 1 = SWAP 0< OR 0= ABORT" MOVSXD, needs 32 bits second"
  2 $63 RM, ;

\ ==== Arithmetic and logic ==============================================
\ n: 0 ADD, 1 OR, 2 ADC, 3 SBB, 4 AND, 5 SUB, 6 XOR, 7 CMP. The form
\ with I takes a number: EAX 5 ADDI, is ADD EAX,5.

PRIVATE
: ALU ( dst src n -- )
  >R  2DUP S2                                        ( dst src s )  ( R: n )
  OVER MEM? IF  R> 8 * 2 + OVER OW RM, EXIT  THEN    \ reg,mem
  >R SWAP R>  R> 8 * OVER OW RM, ;                   \ r/m,reg
: ALUI ( dst n i -- )
  SWAP IMM !  SWAP DUP S1                            ( i dst s )
  DUP 0= IF                                          \ a byte
    OVER AL = IF  2DROP  8 * 4 + TC,  IMM @ TC, EXIT  THEN
    1 FOLLOW !  $80 RM,  IMM @ TC, EXIT
  THEN
  IMM @ FITS8? IF  1 FOLLOW !  $83 RM,  IMM @ TC, EXIT  THEN
  OVER DUP EAX = SWAP RAX = OR IF  NIP PREFIX,  8 * 5 + TC,  IMM @ T, EXIT  THEN
  4 FOLLOW !  $81 RM,  IMM @ T, ;
: ALU: ( n "name" -- )  CREATE ,  DOES> @ ALU ;
: ALUI: ( n "name" -- )  CREATE ,  DOES> @ ALUI ;
PUBLIC
0 ALU: ADD,   1 ALU: OR,   2 ALU: ADC,   3 ALU: SBB,
4 ALU: AND,   5 ALU: SUB,  6 ALU: XOR,   7 ALU: CMP,
0 ALUI: ADDI,  1 ALUI: ORI,  2 ALUI: ADCI,  3 ALUI: SBBI,
4 ALUI: ANDI,  5 ALUI: SUBI, 6 ALUI: XORI,  7 ALUI: CMPI,

: TEST, ( op1 op2 -- )   \ the flags of op1 AND op2
  2DUP S2 >R  DUP MEM? 0= IF SWAP THEN              ( reg r/m )
  R@ $84 R> OW RM, ;
: TESTI, ( op n -- )
  IMM !  DUP S1                                      ( op s )
  OVER DUP AL =  OVER EAX = OR  SWAP RAX = OR IF
    NIP DUP >R PREFIX,  $A8 R@ OW TC,  IMM @ R> IMM, EXIT
  THEN
  DUP >R  DUP ISIZE FOLLOW !  0 ROT ROT  $F6 R@ OW RM,  IMM @ R> IMM, ;

PRIVATE
: F6: ( n "name" -- )  CREATE ,  DOES> @  SWAP  DUP S1  $F6 OVER OW RM, ;
PUBLIC
2 F6: NOT,   3 F6: NEG,   4 F6: MUL,   5 F6: IMUL,   6 F6: DIV,   7 F6: IDIV,

: INC, ( op -- )  0 SWAP  DUP S1  $FE OVER OW RM, ;
: DEC, ( op -- )  1 SWAP  DUP S1  $FE OVER OW RM, ;

\ Shifts and rotations: op n SHL, (n a number) or op CL SHL,.
PRIVATE
: SHIFT: ( n "name" -- )  CREATE ,
  DOES> @ >R  OVER S1 >R                             ( op count )  ( R: n s )
  DUP CL = IF  DROP  R> R> ROT ROT  $D2 OVER OW RM, EXIT  THEN
  DUP 1 = IF  DROP  R> R> ROT ROT  $D0 OVER OW RM, EXIT  THEN
  DUP  R@ 2 = IF 64 ELSE 32 THEN  U< 0= ABORT" a shift takes CL or a number from 0 to 31 (64 bits: 63)"
  IMM !  R> R> ROT ROT  1 FOLLOW !  $C0 OVER OW RM,  IMM @ TC, ;
PUBLIC
0 SHIFT: ROL,   1 SHIFT: ROR,   2 SHIFT: RCL,   3 SHIFT: RCR,
4 SHIFT: SHL,   5 SHIFT: SHR,   7 SHIFT: SAR,

\ ==== Other instructions ================================================

: PUSH, ( op -- )
  DUP R64? IF  1 $50 OPREG, EXIT  THEN
  DUP MEM? 0= OVER KIND 5 = OR ABORT" PUSH, takes 64 bits"
  6 SWAP 1 $FF RM, ;
: PUSHI, ( n -- )   \ PUSH n: 8 bits with sign when it fits, else 32 bits (and for UNKNOWN)
  DUP UNKNOWN = IF  $68 TC,  T, EXIT  THEN
  DUP FITS8? IF  $6A TC,  TC, EXIT  THEN  $68 TC,  T, ;
: POP, ( op -- )
  DUP R64? IF  1 $58 OPREG, EXIT  THEN
  DUP MEM? 0= OVER KIND 5 = OR ABORT" POP, takes 64 bits"
  0 SWAP 1 $8F RM, ;

PRIVATE
: ACC? ( op -- f )   DUP EAX = SWAP RAX = OR ;
PUBLIC
: XCHG, ( op1 op2 -- )
  2DUP S2 >R
  OVER ACC? IF SWAP THEN                             \ the accumulator second
  DUP ACC?  >R OVER REG?  R> AND  >R OVER ACC? 0=  R> AND IF
    DROP  R> $90 OPREG, EXIT  THEN                   \ XCHG EAX,reg: one byte
  OVER MEM? IF SWAP THEN  R@ $86 R> OW RM, ;         \ reg = the register

: LEA, ( reg mem -- )
  OVER DUP R64? SWAP R32? OR  OVER MEM? AND 0= ABORT" LEA, needs a register of 32 or 64 bits and memory"
  OVER SIZE $8D RM, ;

: SETCC, ( op cond -- )   \ the byte op gets 1 when the condition holds, else 0
  >R  DUP SIZE ABORT" SETCC, needs a byte register or BYTE memory"
  0 SWAP 0 R> $0F90 OR RM, ;

: INT, ( n -- )  DUP 3 = IF  DROP $CC TC, EXIT  THEN  $CD TC,  TC, ;
: RETN, ( n -- )  $C2 TC,  TW, ;                    \ RET n: return and drop n bytes of arguments

PRIVATE
: OP: ( opcode "name" -- )  CREATE ,  DOES> @ OPC, ;   \ $48A5: REX.W, then $A5
PUBLIC
$A4 OP: MOVSB,   $A5 OP: MOVSD,   $48A5 OP: MOVSQ,  $A6 OP: CMPSB,
$AA OP: STOSB,   $AB OP: STOSD,   $48AB OP: STOSQ,
$AC OP: LODSB,   $AD OP: LODSD,   $48AD OP: LODSQ,  $AE OP: SCASB,
$F3 OP: REP,     $F3 OP: REPE,    $F3 OP: REPZ,     $F2 OP: REPNE,   $F2 OP: REPNZ,
$F8 OP: CLC,     $F9 OP: STC,     $F5 OP: CMC,      $FC OP: CLD,     $FD OP: STD,
$FA OP: CLI,     $FB OP: STI,     $99 OP: CDQ,      $4899 OP: CQO,   $90 OP: NOP,
$F4 OP: HLT,     $9C OP: PUSHFQ,  $9D OP: POPFQ,    $C3 OP: RET,     $F0 OP: LOCK,
$0F05 OP: SYSCALL,  $0F31 OP: RDTSC,  $0FA2 OP: CPUID,  $F390 OP: PAUSE,

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
  TORG !  0 TDP !  TARGET TARGET-SIZE 0 FILL  JUMPS #JUMPS 2* CELLS 0 FILL  NEW-SCOPE
  0 RX !  RX! OFF!  NO-RX OFF!  0 FOLLOW ! ;

\ ==== Jumps =============================================================
\ A jump is short (8 bits: -128 to 127 from its end) or near (32 bits).
\ A jump to a label with its place takes the short form when it fits;
\ a jump to a label before its place is near. LOOP, and JRCXZ, are short
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
$E0 S: LOOPNZ,  $E0 S: LOOPNE,  $E1 S: LOOPZ,  $E1 S: LOOPE,  $E2 S: LOOP,  $E3 S: JRCXZ,

: JMP, ( ta -- )   \ short when the label has its place and is near, else near
  DUP 2 SHORT? IF  $EB SHORT, EXIT  THEN
  $E9 TC,  THERE 4 + REL32 T, ;
: CALL, ( ta -- )  $E8 TC,  THERE 4 + REL32 T, ;
: JMP-IND, ( op -- )   \ jump to the address in a register or in memory: jmp [r14+rax*8]
  DUP R64? OVER MEM? OR  OVER KIND 5 <> AND 0= ABORT" JMP-IND, takes a register of 64 bits or memory"
  4 SWAP 1 $FF RM, ;
: CALL-IND, ( op -- )   \ call the address in a register or in memory: call [rax+8]
  DUP R64? OVER MEM? OR  OVER KIND 5 <> AND 0= ABORT" CALL-IND, takes a register of 64 bits or memory"
  2 SWAP 1 $FF RM, ;

PRIVATE
: PATCH ( dest ta -- )   \ the jump (or MOVI, reg, or PUSHI,) at ta goes to dest now
  DUP TC@                                              ( dest ta op )
  DUP $F0 AND $70 =  OVER $FC AND $E0 = OR  OVER $EB = OR IF
    DROP  TUCK 2 + REL8  SWAP 1+ TC! EXIT  THEN      \ short jumps
  DUP $0F = IF
    DROP  TUCK 6 + REL32  SWAP 2 + T! EXIT  THEN     \ near conditional jumps
  DUP $E8 =  OVER $E9 = OR IF
    DROP  TUCK 5 + REL32  SWAP 1+ T! EXIT  THEN      \ CALL, near JMP,
  DUP $F8 AND $B8 =  SWAP $68 = OR IF  1+ T! EXIT  THEN   \ MOVI, to EAX-EDI, PUSHI,
  TRUE ABORT" a label before its place is only for a jump, CALL, MOVI, to EAX-EDI or PUSHI," ;
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
