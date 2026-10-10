\ disa64.fs - package DISA64: a disassembler for AArch64 (the 64-bit ARM)
\
\ It turns an instruction into its text, for DISASM of the package
\ MONITOR. It knows the instructions of AArch64 (the instruction set A64)
\ of ARMv8.0 to ARMv8.7 for whole numbers and for the system, as a
\ kernel, a firmware or a C compiler makes them: arithmetic and logic,
\ bit fields, moves, jumps, loads and stores (also those of the
\ registers of floating point and SIMD), exclusive and atomic access
\ (LSE), pointer authentication (PAC), memory tags (MTE), barriers,
\ hints, the system registers and the operations of SYS (DC, IC, AT,
\ TLBI). The other instructions of floating point and SIMD, those of SVE
\ and SME, and most later extensions show as .inst with the number of
\ the instruction.
\
\ The text is the form of llvm-objdump: lower case; the registers x0 to
\ x30, w0 to w30, sp, xzr; a number as # and hex digits (the shifts and
\ the fields of bit fields in decimal); a system register in upper case;
\ a jump with its target:
\
\   00401384  d280001c  mov x28, #0x0
\   00401388  f2a0083c  movk x28, #0x41, lsl #16
\   00401394  8b09039a  add x26, x28, x9
\
\ DISASM shows each instruction as a number of 32 bits, as the ARM manual
\ and llvm-objdump do. The README of the monitor lists where the text
\ differs from llvm-objdump.
\
\   A64 ( -- )   DECODE of MONITOR disassembles AArch64 code (so after
\                the load)
\
\ Needs MONITOR and cells of 32 bits.
\ Load after MONITOR:  ALSO FILES  INCLUDE asm/monitor/disa64.fs

PACKAGE DISA64  ALSO MONITOR   \ lib/package.fs
: NEED-32 ( -- )   CELL 4 <> ABORT" DISA64 needs cells of 32 bits" ;  NEED-32

\ ==== Reading the instruction =============================================
\ A64 code is little endian, 4 bytes for each instruction. The fields
\ are those of the Arm Architecture Reference Manual (chapter C4): bit 31
\ is the highest; Rd or Rt is in bits 0 to 4, Rn in 5 to 9, Rm in 16 to
\ 20, Ra or Rt2 in 10 to 14.

VARIABLE PC                          \ the address of the instruction
VARIABLE INSN                        \ the instruction
VARIABLE BAD                         \ true: a field has a value that the instruction does not allow
: INSN@ ( a -- x )   0  4 0 DO  OVER I + DIS-C@  I 8 * LSHIFT OR  LOOP  NIP ;
: FIELD ( lo n -- u )   \ n bits of INSN from bit lo on
  >R  INSN @ SWAP RSHIFT  1 R> LSHIFT 1- AND ;
: BIT? ( n -- f )   INSN @ SWAP RSHIFT 1 AND 0<> ;
: SIGNED ( u n -- x )   \ u, a number of n bits, with its sign
  1 SWAP 1- LSHIFT  2DUP AND IF  2* -  ELSE  DROP  THEN ;
: BAD! ( -- )   TRUE BAD ! ;
: RD# ( -- n )   0 5 FIELD ;
: RN# ( -- n )   5 5 FIELD ;
: RM# ( -- n )   16 5 FIELD ;
: RA# ( -- n )   10 5 FIELD ;
: SF? ( -- f )   31 BIT? ;           \ 64 bits
: REGSIZE ( -- n )   SF? IF 64 ELSE 32 THEN ;
: IMMR ( -- n )   16 6 FIELD ;
: IMMS ( -- n )   10 6 FIELD ;
: COND# ( -- n )   12 4 FIELD ;

\ ==== Names of instructions ===============================================
\ The names, as counted strings one after another; an entry of the table
\ holds the offset of its name.

2000 CONSTANT /NAMES
CREATE NAMES  /NAMES ALLOT   VARIABLE NAMES-END   NAMES NAMES-END !
VARIABLE NA   VARIABLE NU
: NAME# ( a u -- n )   \ the offset of the name a u; a new name goes to the end
  NU !  NA !  NAMES
  BEGIN  DUP NAMES-END @ U< WHILE
    DUP COUNT  DUP NU @ = IF  NA @ SWAP SAME? IF  NAMES - EXIT  THEN  ELSE  2DROP  THEN
    COUNT +
  REPEAT
  DUP 1+ NU @ +  NAMES /NAMES + U> ABORT" DISA64: NAMES is full"
  NU @ OVER C!  NA @ OVER 1+ NU @ MOVE  DUP 1+ NU @ + NAMES-END !  NAMES - ;

\ ==== Kinds of operands ===================================================
\ Each kind of operand has a number; its name is a word here, so the
\ table below can name it.
\
\   Rd Rn Rm Ra   a register: w for 32 bits, x for 64 (bit 31, sf);
\                 31 is wzr or xzr; Rd|SP Rn|SP: 31 is wsp or sp
\   Rt Rt.b5      Rt by sf; by bit 31 (TBZ)
\   Wd ... Xs     a register of 32 or 64 bits: d n m in their fields, t
\                 in bits 0 to 4, t2 and a in 10 to 14, s in 16 to 20
\   Xd|SP ...     the same, 31 is sp
\   Rt.sz Rs.sz   w or x by the field size (bits 30, 31): x for 3
\   Rt.30 ...     w or x by bit 30 (pairs); Rs+1.30 Rt+1.30: the next
\                 register (CASP)
\   Bt ... Qt2    registers of floating point and SIMD
\   Rm.shift      Rm and its shift (ADD); Rm.lshift: also ROR (AND)
\   Rm.ext        Rm and its extension (ADD); Rm.exts: for ADDS and SUBS
\   Xt.64b        the first of 8 registers (LD64B): x0, x2 ... x22
\   [Xn] ...      memory: the base Xn or sp, with an offset (+s9: 9 bits
\                 with the sign, +u12: 12 bits; *4: times 4), after the
\                 access ([Xn]+s9), before it (]!), or a register
\                 (+Rm*8: its shift is 3)
\   #aimm #bimm   the immediates of ADD and of AND; #bimm.s: with the
\                 sign (MOV)
\   #movz #movn   the number that MOVZ or MOVN make (MOV)
\   #i16.hw       imm16 and its shift (MOVK)
\   #immr #lsl #lsb.iz #width.iz #width.x #imms   the fields of the
\                 bit fields: immr; the shift of LSL; lsb and width of
\                 SBFIZ ..., of SBFX ...; imms
\   cond cond.inv the condition, the inverted one (CSET)
\   @26 @19 @14 @adr @adrp   the target of a jump or of ADR, ADRP
\   prfop barrier sysreg   the names of PRFM, of DMB, of MRS and MSR
\   .ar .l .cas .sz .cond   these add to the name: a, l, al (LDADD); l
\                 (STADD); a, l (CAS); b or h (the size); .eq (B.cond)
\   @hint @msri @sys   the whole text of HINT, MSR (immediate), SYS

: KINDS ( n0 n "name" ... -- )   0 ?DO  DUP CONSTANT 1+  LOOP  DROP ;
1 8 KINDS  Rd Rn Rm Ra Rd|SP Rn|SP Rt Rt.b5
9 6 KINDS  Wd Wn Wm Wt Wt2 Ws
15 7 KINDS Xd Xn Xm Xa Xt Xt2 Xs
22 4 KINDS Xd|SP Xn|SP Xm|SP Xt|SP
26 7 KINDS Rt.sz Rs.sz Rt.30 Rt2.30 Rs.30 Rs+1.30 Rt+1.30
33 8 KINDS Bt Ht St Dt Qt St2 Dt2 Qt2
41 5 KINDS Rm.shift Rm.lshift Rm.ext Rm.exts Xt.64b
50 4 KINDS [Xn] [Xn+s9] [Xn]+s9 [Xn+s9]!
54 5 KINDS [Xn+u12] [Xn+u12*2] [Xn+u12*4] [Xn+u12*8] [Xn+u12*16]
59 9 KINDS [Xn+s7*4] [Xn+s7*8] [Xn+s7*16] [Xn]+s7*4 [Xn]+s7*8 [Xn]+s7*16 [Xn+s7*4]! [Xn+s7*8]! [Xn+s7*16]!
68 5 KINDS [Xn+Rm] [Xn+Rm*2] [Xn+Rm*4] [Xn+Rm*8] [Xn+Rm*16]
73 5 KINDS [Xn+s10*8] [Xn+s10*8]! [Xn+s9*16] [Xn]+s9*16 [Xn+s9*16]!
80 12 KINDS #aimm #bimm #bimm.s #movz #movn #i16.hw #immr #lsl #lsb.iz #width.iz #width.x #imms
92 8 KINDS #imm5 #nzcv cond cond.inv #i16 #u6*16 #u4 #bit
100 5 KINDS @26 @19 @14 @adr @adrp
105 15 KINDS #rmif #mask prfop barrier barrier.nxs #crm #crm.d sysreg #udf #op1 Cn Cm #op2 sm za
120 5 KINDS .ar .l .cas .sz .cond
130 3 KINDS @hint @msri @sys

\ ==== Numbers of 64 bits ==================================================
\ The immediates of AND and MOV have up to 64 bits: a double, lo hi.

: D+ ( d1 d2 -- d )   ROT + >R  UM+  R> + ;
: ONES ( n -- u )   \ n low bits set, n from 0 to 32
  DUP 32 = IF  DROP -1 EXIT  THEN  1 SWAP LSHIFT 1- ;
: DROR ( d r -- d' )   \ the 64 bits rotated right by r (0 to 63)
  DUP 32 < 0= IF  32 -  >R SWAP R>  THEN
  ?DUP 0= IF EXIT THEN
  >R  OVER R@ RSHIFT  OVER 32 R@ - LSHIFT OR
  ROT 32 R@ - LSHIFT  ROT R> RSHIFT OR ;
VARIABLE ESIZE                       \ the size of an element of a bit mask: 2 to 64
: ROR-E ( w r -- w' )   \ w rotated right by r inside ESIZE bits (up to 32)
  ?DUP 0= IF EXIT THEN
  2DUP RSHIFT >R  ESIZE @ SWAP - LSHIFT  R> OR  ESIZE @ ONES AND ;
: REPLICATE ( w -- w' )   \ the element of ESIZE bits over all 32 bits
  ESIZE @  BEGIN  DUP 32 < WHILE  >R  DUP R@ LSHIFT OR  R> 2*  REPEAT  DROP ;
: NO-MASK ( x -- 0 0 )   DROP BAD!  0 0 ;
: BITMASK ( -- d )   \ the immediate of AND, ORR, EOR, ANDS from N, immr, imms (ARM: DecodeBitMasks)
  22 1 FIELD 6 LSHIFT  IMMS INVERT 63 AND OR          ( N and NOT imms )
  0 SWAP  BEGIN  DUP 1 > WHILE  1 RSHIFT  SWAP 1+ SWAP  REPEAT  DROP   ( len )
  DUP 1 < IF  NO-MASK EXIT  THEN
  1 OVER LSHIFT ESIZE !
  SF? 0= ESIZE @ 64 = AND IF  NO-MASK EXIT  THEN
  ONES  IMMS OVER AND  OVER = IF  NO-MASK EXIT  THEN    ( levels )
  DUP IMMS AND  SWAP IMMR AND                         ( s r )
  ESIZE @ 64 = IF
    >R  1+ DUP 32 > IF  32 - ONES -1 SWAP  ELSE  ONES 0  THEN  R> DROR  EXIT
  THEN
  >R  1+ ONES  R> ROR-E  REPLICATE  DUP ;
: HALVES ( u -- n )   \ the halves of u that are not 0
  DUP $FFFF AND 0<> 1 AND  SWAP 16 RSHIFT 0<> 1 AND + ;
: ONE-HALF? ( d -- f )   \ at most one halfword of d is not 0: one MOVZ makes it
  HALVES SWAP HALVES + 2 < ;
: MOVE-WIDE? ( -- f )   \ MOVZ or MOVN make the immediate of ORR too
  BITMASK  SF? 0= IF  DROP 0  THEN  2DUP ONE-HALF? IF  2DROP TRUE EXIT  THEN
  INVERT SWAP INVERT SWAP  SF? 0= IF  DROP 0  THEN  ONE-HALF? ;
: WIDE ( -- d )   \ imm16 shifted by 16 times hw (MOVZ, MOVN, MOVK)
  5 16 FIELD  21 2 FIELD
  SF? 0= OVER 1 > AND IF  BAD!  THEN
  CASE  0 OF 0 ENDOF  1 OF 16 LSHIFT 0 ENDOF  2 OF 0 SWAP ENDOF  3 OF 16 LSHIFT 0 SWAP ENDOF  ENDCASE ;

\ ==== Conditions of aliases ===============================================
\ An alias has the bits of its instruction and a condition (ARM: the
\ preferred disassembly), WHEN: in the table.

: SP? ( -- f )   RD# 31 =  RN# 31 = OR ;             \ MOV for ADD: to or from sp
: MOVI? ( -- f )   MOVE-WIDE? 0= ;                     \ MOV for ORR
: MOVZ? ( -- f )   5 16 FIELD 0=  21 2 FIELD 0<> AND 0= ;   \ MOV for MOVZ
: MOVN? ( -- f )   MOVZ?  SF? 5 16 FIELD $FFFF <> OR  AND ;   \ MOV for MOVN
: IZ? ( -- f )   IMMS IMMR < ;                         \ SBFIZ, BFC, BFI, UBFIZ
: LSL? ( -- f )   IMMS 1+ IMMR = ;                     \ LSL for UBFM
: ROR? ( -- f )   RN# RM# = ;                          \ ROR for EXTR
: CINC? ( -- f )   RN# RM# =  RN# 31 <> AND  COND# 14 < AND ;   \ CINC, CINV
: CSET? ( -- f )   COND# 14 < ;                        \ CSET, CSETM
: CNEG? ( -- f )   RN# RM# =  COND# 14 < AND ;         \ CNEG

\ ==== The table ===========================================================
\ An entry: the link to the next entry of its group, the mask (the fixed
\ bits), the value of the fixed bits, its condition (an xt, or 0), the
\ offset of its name (2 bytes), 6 bytes for the kinds of its operands (0
\ ends them). The group is bits 26 to 28. OP: makes an entry from a
\ line: the name, the bits from 31 down to 0 in groups of 4 (0 and 1 are
\ fixed, a letter is a field), the kinds of the operands separated by
\ commas, or "-" for none. WHEN: before OP: names the condition: the
\ entry counts only when it gives true. In a group the entries count in
\ the order of the lines: an alias comes before its instruction.

CREATE FIRSTS  8 CELLS ALLOT   FIRSTS 8 CELLS 0 FILL
CREATE LASTS   8 CELLS ALLOT   LASTS 8 CELLS 0 FILL
VARIABLE PMASK   VARIABLE PMATCH   VARIABLE PRED-XT
: PBIT ( c -- )   \ the next bit of a pattern
  PMASK @ 2* PMASK !  PMATCH @ 2* PMATCH !
  DUP '0' = IF  DROP  1 PMASK +!  EXIT  THEN
  '1' = IF  1 PMASK +!  1 PMATCH +!  THEN ;
: PATTERN ( "groups" -- )   \ 8 groups of 4 bits, the highest first
  0 PMASK !  0 PMATCH !
  8 0 DO  PARSE-NAME  DUP 4 <> ABORT" DISA64: a group has 4 bits"
    0 ?DO  DUP I + C@ PBIT  LOOP  DROP
  LOOP ;
: KIND# ( a u -- k )   FIND-NAME DUP 0= ABORT" DISA64: no such kind of operand"  NAME>XT EXECUTE ;
VARIABLE THE-CHAR
: TO-CHAR ( a u c -- n )   \ the characters before the first c (u if there is none)
  THE-CHAR !  DUP >R
  0 ?DO  DUP I + C@ THE-CHAR @ = IF  DROP I  UNLOOP  R> DROP  EXIT  THEN  LOOP  DROP R> ;
: /SKIP ( a u n -- a' u' )   ROT OVER +  ROT ROT - ;
VARIABLE DST
: KINDS! ( a "kinds" -- )   \ the kinds of the operands into the 6 bytes at a
  DST !  PARSE-NAME
  DUP 1 = IF  OVER C@ '-' = IF  2DROP EXIT  THEN  THEN
  BEGIN  DUP 0 > WHILE
    2DUP ',' TO-CHAR >R  OVER R@ KIND#  DST @ C!  1 DST +!
    R> 1+ /SKIP
  REPEAT  2DROP ;
: GROUP ( x -- n )   26 RSHIFT 7 AND ;
: APPEND ( e -- )   \ e at the end of the list of its group
  PMATCH @ GROUP CELLS >R
  R@ LASTS + @ ?DUP IF  OVER SWAP !  ELSE  DUP R@ FIRSTS + !  THEN
  R> LASTS + ! ;
: WHEN: ( "name" -- )   \ the condition of the next entry
  PARSE-NAME FIND-NAME DUP 0= ABORT" DISA64: no such condition"  NAME>XT PRED-XT ! ;
: OP: ( "name" "8 groups" "kinds" -- )
  PARSE-NAME NAME# >R  PATTERN
  PMASK @ $1C000000 AND $1C000000 <> ABORT" DISA64: bits 26 to 28 must be fixed"
  ALIGN HERE  0 ,  PMASK @ ,  PMATCH @ ,  PRED-XT @ ,  0 PRED-XT !
  R> DUP C, 8 RSHIFT C,
  HERE 6 ALLOT  DUP 6 0 FILL  KINDS!
  ALIGN  APPEND ;
: MASK ( e -- x )    CELL+ @ ;
: MATCH ( e -- x )   2 CELLS + @ ;
: PRED ( e -- xt | 0 )   3 CELLS + @ ;
: NAME ( e -- a u )   4 CELLS + DUP C@ SWAP 1+ C@ 8 LSHIFT OR  NAMES + COUNT ;
: KIND ( e i -- k )   SWAP 4 CELLS + 2 + + C@ ;

\ ---- Data processing, immediate (ARM C4.1.86) -------------------------
OP: adr      0ii1 0000 iiii iiii iiii iiii iiid dddd   Xd,@adr
OP: adrp     1ii1 0000 iiii iiii iiii iiii iiid dddd   Xd,@adrp
WHEN: SP?
OP: mov      s001 0001 0000 0000 0000 00nn nnnd dddd   Rd|SP,Rn|SP
OP: add      s001 0001 0hii iiii iiii iinn nnnd dddd   Rd|SP,Rn|SP,#aimm
OP: cmn      s011 0001 0hii iiii iiii iinn nnn1 1111   Rn|SP,#aimm
OP: adds     s011 0001 0hii iiii iiii iinn nnnd dddd   Rd,Rn|SP,#aimm
OP: sub      s101 0001 0hii iiii iiii iinn nnnd dddd   Rd|SP,Rn|SP,#aimm
OP: cmp      s111 0001 0hii iiii iiii iinn nnn1 1111   Rn|SP,#aimm
OP: subs     s111 0001 0hii iiii iiii iinn nnnd dddd   Rd,Rn|SP,#aimm
OP: addg     1001 0001 10ii iiii xxii iinn nnnd dddd   Xd|SP,Xn|SP,#u6*16,#u4
OP: subg     1101 0001 10ii iiii xxii iinn nnnd dddd   Xd|SP,Xn|SP,#u6*16,#u4
OP: and      s001 0010 0Nrr rrrr ssss ssnn nnnd dddd   Rd|SP,Rn,#bimm
WHEN: MOVI?
OP: mov      s011 0010 0Nrr rrrr ssss ss11 111d dddd   Rd|SP,#bimm.s
OP: orr      s011 0010 0Nrr rrrr ssss ssnn nnnd dddd   Rd|SP,Rn,#bimm
OP: eor      s101 0010 0Nrr rrrr ssss ssnn nnnd dddd   Rd|SP,Rn,#bimm
OP: tst      s111 0010 0Nrr rrrr ssss ssnn nnn1 1111   Rn,#bimm
OP: ands     s111 0010 0Nrr rrrr ssss ssnn nnnd dddd   Rd,Rn,#bimm
WHEN: MOVN?
OP: mov      s001 0010 1hhi iiii iiii iiii iiid dddd   Rd,#movn
OP: movn     s001 0010 1hhi iiii iiii iiii iiid dddd   Rd,#i16.hw
WHEN: MOVZ?
OP: mov      s101 0010 1hhi iiii iiii iiii iiid dddd   Rd,#movz
OP: movz     s101 0010 1hhi iiii iiii iiii iiid dddd   Rd,#i16.hw
OP: movk     s111 0010 1hhi iiii iiii iiii iiid dddd   Rd,#i16.hw
\ The bit fields: SBFM, BFM, UBFM only as their aliases. 32 bits: N,
\ immr<5> and imms<5> are 0; 64 bits: N is 1.
OP: asr      0001 0011 000r rrrr 0111 11nn nnnd dddd   Rd,Rn,#immr
OP: sxtb     0001 0011 0000 0000 0001 11nn nnnd dddd   Rd,Wn
OP: sxth     0001 0011 0000 0000 0011 11nn nnnd dddd   Rd,Wn
WHEN: IZ?
OP: sbfiz    0001 0011 000r rrrr 0sss ssnn nnnd dddd   Rd,Rn,#lsb.iz,#width.iz
OP: sbfx     0001 0011 000r rrrr 0sss ssnn nnnd dddd   Rd,Rn,#immr,#width.x
OP: asr      1001 0011 01rr rrrr 1111 11nn nnnd dddd   Rd,Rn,#immr
OP: sxtb     1001 0011 0100 0000 0001 11nn nnnd dddd   Rd,Wn
OP: sxth     1001 0011 0100 0000 0011 11nn nnnd dddd   Rd,Wn
OP: sxtw     1001 0011 0100 0000 0111 11nn nnnd dddd   Rd,Wn
WHEN: IZ?
OP: sbfiz    1001 0011 01rr rrrr ssss ssnn nnnd dddd   Rd,Rn,#lsb.iz,#width.iz
OP: sbfx     1001 0011 01rr rrrr ssss ssnn nnnd dddd   Rd,Rn,#immr,#width.x
WHEN: IZ?
OP: bfi      0011 0011 000r rrrr 0sss ssnn nnnd dddd   Rd,Rn,#lsb.iz,#width.iz
OP: bfxil    0011 0011 000r rrrr 0sss ssnn nnnd dddd   Rd,Rn,#immr,#width.x
WHEN: IZ?
OP: bfi      1011 0011 01rr rrrr ssss ssnn nnnd dddd   Rd,Rn,#lsb.iz,#width.iz
OP: bfxil    1011 0011 01rr rrrr ssss ssnn nnnd dddd   Rd,Rn,#immr,#width.x
OP: lsr      0101 0011 000r rrrr 0111 11nn nnnd dddd   Rd,Rn,#immr
OP: uxtb     0101 0011 0000 0000 0001 11nn nnnd dddd   Wd,Wn
OP: uxth     0101 0011 0000 0000 0011 11nn nnnd dddd   Wd,Wn
WHEN: LSL?
OP: lsl      0101 0011 000r rrrr 0sss ssnn nnnd dddd   Rd,Rn,#lsl
WHEN: IZ?
OP: ubfiz    0101 0011 000r rrrr 0sss ssnn nnnd dddd   Rd,Rn,#lsb.iz,#width.iz
OP: ubfx     0101 0011 000r rrrr 0sss ssnn nnnd dddd   Rd,Rn,#immr,#width.x
OP: lsr      1101 0011 01rr rrrr 1111 11nn nnnd dddd   Rd,Rn,#immr
WHEN: LSL?
OP: lsl      1101 0011 01rr rrrr ssss ssnn nnnd dddd   Rd,Rn,#lsl
WHEN: IZ?
OP: ubfiz    1101 0011 01rr rrrr ssss ssnn nnnd dddd   Rd,Rn,#lsb.iz,#width.iz
OP: ubfx     1101 0011 01rr rrrr ssss ssnn nnnd dddd   Rd,Rn,#immr,#width.x
WHEN: ROR?
OP: ror      0001 0011 100m mmmm 0sss ssnn nnnd dddd   Rd,Rn,#imms
OP: extr     0001 0011 100m mmmm 0sss ssnn nnnd dddd   Rd,Rn,Rm,#imms
WHEN: ROR?
OP: ror      1001 0011 110m mmmm ssss ssnn nnnd dddd   Rd,Rn,#imms
OP: extr     1001 0011 110m mmmm ssss ssnn nnnd dddd   Rd,Rn,Rm,#imms

\ ---- Branches, exceptions, the system (ARM C4.1.87) ---------------------
OP: b        0101 0100 iiii iiii iiii iiii iii0 cccc   .cond,@19
OP: bc       0101 0100 iiii iiii iiii iiii iii1 cccc   .cond,@19
OP: svc      1101 0100 000i iiii iiii iiii iii0 0001   #i16
OP: hvc      1101 0100 000i iiii iiii iiii iii0 0010   #i16
OP: smc      1101 0100 000i iiii iiii iiii iii0 0011   #i16
OP: brk      1101 0100 001i iiii iiii iiii iii0 0000   #i16
OP: hlt      1101 0100 010i iiii iiii iiii iii0 0000   #i16
OP: tcancel  1101 0100 011i iiii iiii iiii iii0 0000   #i16
OP: dcps1    1101 0100 1010 0000 0000 0000 0000 0001   -
OP: dcps2    1101 0100 1010 0000 0000 0000 0000 0010   -
OP: dcps3    1101 0100 1010 0000 0000 0000 0000 0011   -
OP: dcps1    1101 0100 101i iiii iiii iiii iii0 0001   #i16
OP: dcps2    1101 0100 101i iiii iiii iiii iii0 0010   #i16
OP: dcps3    1101 0100 101i iiii iiii iiii iii0 0011   #i16
OP: wfet     1101 0101 0000 0011 0001 0000 000t tttt   Xt
OP: wfit     1101 0101 0000 0011 0001 0000 001t tttt   Xt
OP: hint     1101 0101 0000 0011 0010 xxxx xxx1 1111   @hint
OP: clrex    1101 0101 0000 0011 0011 1111 0101 1111   -
OP: clrex    1101 0101 0000 0011 0011 mmmm 0101 1111   #crm
OP: dsb      1101 0101 0000 0011 0011 mm10 0011 1111   barrier.nxs
OP: tcommit  1101 0101 0000 0011 0011 0000 0111 1111   -
OP: ssbb     1101 0101 0000 0011 0011 0000 1001 1111   -
OP: pssbb    1101 0101 0000 0011 0011 0100 1001 1111   -
OP: dfb      1101 0101 0000 0011 0011 1100 1001 1111   -
OP: dsb      1101 0101 0000 0011 0011 mmmm 1001 1111   barrier
OP: dmb      1101 0101 0000 0011 0011 mmmm 1011 1111   barrier
OP: isb      1101 0101 0000 0011 0011 1111 1101 1111   -
OP: isb      1101 0101 0000 0011 0011 mmmm 1101 1111   #crm.d
OP: sb       1101 0101 0000 0011 0011 mmmm 1111 1111   -
OP: cfinv    1101 0101 0000 0000 0100 mmmm 0001 1111   -
OP: xaflag   1101 0101 0000 0000 0100 mmmm 0011 1111   -
OP: axflag   1101 0101 0000 0000 0100 mmmm 0101 1111   -
OP: smstop   1101 0101 0000 0011 0100 0010 0111 1111   sm
OP: smstart  1101 0101 0000 0011 0100 0011 0111 1111   sm
OP: smstop   1101 0101 0000 0011 0100 0100 0111 1111   za
OP: smstart  1101 0101 0000 0011 0100 0101 0111 1111   za
OP: smstop   1101 0101 0000 0011 0100 0110 0111 1111   -
OP: smstart  1101 0101 0000 0011 0100 0111 0111 1111   -
OP: msr      1101 0101 0000 0ooo 0100 mmmm ooo1 1111   @msri
OP: tstart   1101 0101 0010 0011 0011 0000 011t tttt   Xt
OP: ttest    1101 0101 0010 0011 0011 0001 011t tttt   Xt
OP: msr      1101 0101 0000 0ooo nnnn mmmm ooot tttt   sysreg,Xt
OP: mrs      1101 0101 0010 0ooo nnnn mmmm ooot tttt   Xt,sysreg
OP: sys      1101 0101 0000 1ooo nnnn mmmm ooot tttt   @sys
OP: sysl     1101 0101 0010 1ooo nnnn mmmm ooot tttt   Xt,#op1,Cn,Cm,#op2
OP: msr      1101 0101 0001 oooo nnnn mmmm ooot tttt   sysreg,Xt
OP: mrs      1101 0101 0011 oooo nnnn mmmm ooot tttt   Xt,sysreg
\ Jumps to a register; BRAA ... BLRAB have the modifier in bits 0 to 4
OP: br       1101 0110 0001 1111 0000 00nn nnn0 0000   Xn
OP: braaz    1101 0110 0001 1111 0000 10nn nnn1 1111   Xn
OP: brabz    1101 0110 0001 1111 0000 11nn nnn1 1111   Xn
OP: blr      1101 0110 0011 1111 0000 00nn nnn0 0000   Xn
OP: blraaz   1101 0110 0011 1111 0000 10nn nnn1 1111   Xn
OP: blrabz   1101 0110 0011 1111 0000 11nn nnn1 1111   Xn
OP: ret      1101 0110 0101 1111 0000 0011 1100 0000   -
OP: ret      1101 0110 0101 1111 0000 00nn nnn0 0000   Xn
OP: retaa    1101 0110 0101 1111 0000 1011 1111 1111   -
OP: retab    1101 0110 0101 1111 0000 1111 1111 1111   -
OP: eret     1101 0110 1001 1111 0000 0011 1110 0000   -
OP: eretaa   1101 0110 1001 1111 0000 1011 1111 1111   -
OP: eretab   1101 0110 1001 1111 0000 1111 1111 1111   -
OP: drps     1101 0110 1011 1111 0000 0011 1110 0000   -
OP: braa     1101 0111 0001 1111 0000 10nn nnnm mmmm   Xn,Xd|SP
OP: brab     1101 0111 0001 1111 0000 11nn nnnm mmmm   Xn,Xd|SP
OP: blraa    1101 0111 0011 1111 0000 10nn nnnm mmmm   Xn,Xd|SP
OP: blrab    1101 0111 0011 1111 0000 11nn nnnm mmmm   Xn,Xd|SP
OP: b        0001 01ii iiii iiii iiii iiii iiii iiii   @26
OP: bl       1001 01ii iiii iiii iiii iiii iiii iiii   @26
OP: cbz      s011 0100 iiii iiii iiii iiii iiit tttt   Rt,@19
OP: cbnz     s011 0101 iiii iiii iiii iiii iiit tttt   Rt,@19
OP: tbz      b011 0110 bbbb biii iiii iiii iiit tttt   Rt.b5,#bit,@14
OP: tbnz     b011 0111 bbbb biii iiii iiii iiit tttt   Rt.b5,#bit,@14

\ ---- Loads and stores (ARM C4.1.88) --------------------------------------
\ Exclusive and ordered (bits 24 to 29: 001000); z is the size
OP: stxr     zz00 1000 000s ssss 0aaa aann nnnt tttt   .sz,Ws,Rt.sz,[Xn]
OP: stlxr    zz00 1000 000s ssss 1aaa aann nnnt tttt   .sz,Ws,Rt.sz,[Xn]
OP: ldxr     zz00 1000 010s ssss 0aaa aann nnnt tttt   .sz,Rt.sz,[Xn]
OP: ldaxr    zz00 1000 010s ssss 1aaa aann nnnt tttt   .sz,Rt.sz,[Xn]
OP: stxp     1z00 1000 001s ssss 0aaa aann nnnt tttt   Ws,Rt.30,Rt2.30,[Xn]
OP: stlxp    1z00 1000 001s ssss 1aaa aann nnnt tttt   Ws,Rt.30,Rt2.30,[Xn]
OP: ldxp     1z00 1000 011s ssss 0aaa aann nnnt tttt   Rt.30,Rt2.30,[Xn]
OP: ldaxp    1z00 1000 011s ssss 1aaa aann nnnt tttt   Rt.30,Rt2.30,[Xn]
OP: casp     0z00 1000 0L1s ssss o111 11nn nnnt tttt   .cas,Rs.30,Rs+1.30,Rt.30,Rt+1.30,[Xn]
OP: stllr    zz00 1000 100s ssss 0aaa aann nnnt tttt   .sz,Rt.sz,[Xn]
OP: stlr     zz00 1000 100s ssss 1aaa aann nnnt tttt   .sz,Rt.sz,[Xn]
OP: ldlar    zz00 1000 110s ssss 0aaa aann nnnt tttt   .sz,Rt.sz,[Xn]
OP: ldar     zz00 1000 110s ssss 1aaa aann nnnt tttt   .sz,Rt.sz,[Xn]
OP: cas      zz00 1000 1L1s ssss o111 11nn nnnt tttt   .cas,.sz,Rs.sz,Rt.sz,[Xn]
\ Ordered, with an offset of 9 bits (bits 24 to 29: 011001)
OP: stlurb   0001 1001 000i iiii iiii 00nn nnnt tttt   Wt,[Xn+s9]
OP: ldapurb  0001 1001 010i iiii iiii 00nn nnnt tttt   Wt,[Xn+s9]
OP: ldapursb 0001 1001 100i iiii iiii 00nn nnnt tttt   Xt,[Xn+s9]
OP: ldapursb 0001 1001 110i iiii iiii 00nn nnnt tttt   Wt,[Xn+s9]
OP: stlurh   0101 1001 000i iiii iiii 00nn nnnt tttt   Wt,[Xn+s9]
OP: ldapurh  0101 1001 010i iiii iiii 00nn nnnt tttt   Wt,[Xn+s9]
OP: ldapursh 0101 1001 100i iiii iiii 00nn nnnt tttt   Xt,[Xn+s9]
OP: ldapursh 0101 1001 110i iiii iiii 00nn nnnt tttt   Wt,[Xn+s9]
OP: stlur    1001 1001 000i iiii iiii 00nn nnnt tttt   Wt,[Xn+s9]
OP: ldapur   1001 1001 010i iiii iiii 00nn nnnt tttt   Wt,[Xn+s9]
OP: ldapursw 1001 1001 100i iiii iiii 00nn nnnt tttt   Xt,[Xn+s9]
OP: stlur    1101 1001 000i iiii iiii 00nn nnnt tttt   Xt,[Xn+s9]
OP: ldapur   1101 1001 010i iiii iiii 00nn nnnt tttt   Xt,[Xn+s9]
\ Relative to PC (literal)
OP: ldr      0001 1000 iiii iiii iiii iiii iiit tttt   Wt,@19
OP: ldr      0101 1000 iiii iiii iiii iiii iiit tttt   Xt,@19
OP: ldrsw    1001 1000 iiii iiii iiii iiii iiit tttt   Xt,@19
OP: prfm     1101 1000 iiii iiii iiii iiii iiit tttt   prfop,@19
OP: ldr      0001 1100 iiii iiii iiii iiii iiit tttt   St,@19
OP: ldr      0101 1100 iiii iiii iiii iiii iiit tttt   Dt,@19
OP: ldr      1001 1100 iiii iiii iiii iiii iiit tttt   Qt,@19
\ Pairs: no-allocate, after the access, with an offset, before it
OP: stnp     0010 1000 00ii iiii iaaa aann nnnt tttt   Wt,Wt2,[Xn+s7*4]
OP: ldnp     0010 1000 01ii iiii iaaa aann nnnt tttt   Wt,Wt2,[Xn+s7*4]
OP: stnp     1010 1000 00ii iiii iaaa aann nnnt tttt   Xt,Xt2,[Xn+s7*8]
OP: ldnp     1010 1000 01ii iiii iaaa aann nnnt tttt   Xt,Xt2,[Xn+s7*8]
OP: stnp     0010 1100 00ii iiii iaaa aann nnnt tttt   St,St2,[Xn+s7*4]
OP: ldnp     0010 1100 01ii iiii iaaa aann nnnt tttt   St,St2,[Xn+s7*4]
OP: stnp     0110 1100 00ii iiii iaaa aann nnnt tttt   Dt,Dt2,[Xn+s7*8]
OP: ldnp     0110 1100 01ii iiii iaaa aann nnnt tttt   Dt,Dt2,[Xn+s7*8]
OP: stnp     1010 1100 00ii iiii iaaa aann nnnt tttt   Qt,Qt2,[Xn+s7*16]
OP: ldnp     1010 1100 01ii iiii iaaa aann nnnt tttt   Qt,Qt2,[Xn+s7*16]
OP: stp      0010 1000 10ii iiii iaaa aann nnnt tttt   Wt,Wt2,[Xn]+s7*4
OP: ldp      0010 1000 11ii iiii iaaa aann nnnt tttt   Wt,Wt2,[Xn]+s7*4
OP: stgp     0110 1000 10ii iiii iaaa aann nnnt tttt   Xt,Xt2,[Xn]+s7*16
OP: ldpsw    0110 1000 11ii iiii iaaa aann nnnt tttt   Xt,Xt2,[Xn]+s7*4
OP: stp      1010 1000 10ii iiii iaaa aann nnnt tttt   Xt,Xt2,[Xn]+s7*8
OP: ldp      1010 1000 11ii iiii iaaa aann nnnt tttt   Xt,Xt2,[Xn]+s7*8
OP: stp      0010 1100 10ii iiii iaaa aann nnnt tttt   St,St2,[Xn]+s7*4
OP: ldp      0010 1100 11ii iiii iaaa aann nnnt tttt   St,St2,[Xn]+s7*4
OP: stp      0110 1100 10ii iiii iaaa aann nnnt tttt   Dt,Dt2,[Xn]+s7*8
OP: ldp      0110 1100 11ii iiii iaaa aann nnnt tttt   Dt,Dt2,[Xn]+s7*8
OP: stp      1010 1100 10ii iiii iaaa aann nnnt tttt   Qt,Qt2,[Xn]+s7*16
OP: ldp      1010 1100 11ii iiii iaaa aann nnnt tttt   Qt,Qt2,[Xn]+s7*16
OP: stp      0010 1001 00ii iiii iaaa aann nnnt tttt   Wt,Wt2,[Xn+s7*4]
OP: ldp      0010 1001 01ii iiii iaaa aann nnnt tttt   Wt,Wt2,[Xn+s7*4]
OP: stgp     0110 1001 00ii iiii iaaa aann nnnt tttt   Xt,Xt2,[Xn+s7*16]
OP: ldpsw    0110 1001 01ii iiii iaaa aann nnnt tttt   Xt,Xt2,[Xn+s7*4]
OP: stp      1010 1001 00ii iiii iaaa aann nnnt tttt   Xt,Xt2,[Xn+s7*8]
OP: ldp      1010 1001 01ii iiii iaaa aann nnnt tttt   Xt,Xt2,[Xn+s7*8]
OP: stp      0010 1101 00ii iiii iaaa aann nnnt tttt   St,St2,[Xn+s7*4]
OP: ldp      0010 1101 01ii iiii iaaa aann nnnt tttt   St,St2,[Xn+s7*4]
OP: stp      0110 1101 00ii iiii iaaa aann nnnt tttt   Dt,Dt2,[Xn+s7*8]
OP: ldp      0110 1101 01ii iiii iaaa aann nnnt tttt   Dt,Dt2,[Xn+s7*8]
OP: stp      1010 1101 00ii iiii iaaa aann nnnt tttt   Qt,Qt2,[Xn+s7*16]
OP: ldp      1010 1101 01ii iiii iaaa aann nnnt tttt   Qt,Qt2,[Xn+s7*16]
OP: stp      0010 1001 10ii iiii iaaa aann nnnt tttt   Wt,Wt2,[Xn+s7*4]!
OP: ldp      0010 1001 11ii iiii iaaa aann nnnt tttt   Wt,Wt2,[Xn+s7*4]!
OP: stgp     0110 1001 10ii iiii iaaa aann nnnt tttt   Xt,Xt2,[Xn+s7*16]!
OP: ldpsw    0110 1001 11ii iiii iaaa aann nnnt tttt   Xt,Xt2,[Xn+s7*4]!
OP: stp      1010 1001 10ii iiii iaaa aann nnnt tttt   Xt,Xt2,[Xn+s7*8]!
OP: ldp      1010 1001 11ii iiii iaaa aann nnnt tttt   Xt,Xt2,[Xn+s7*8]!
OP: stp      0010 1101 10ii iiii iaaa aann nnnt tttt   St,St2,[Xn+s7*4]!
OP: ldp      0010 1101 11ii iiii iaaa aann nnnt tttt   St,St2,[Xn+s7*4]!
OP: stp      0110 1101 10ii iiii iaaa aann nnnt tttt   Dt,Dt2,[Xn+s7*8]!
OP: ldp      0110 1101 11ii iiii iaaa aann nnnt tttt   Dt,Dt2,[Xn+s7*8]!
OP: stp      1010 1101 10ii iiii iaaa aann nnnt tttt   Qt,Qt2,[Xn+s7*16]!
OP: ldp      1010 1101 11ii iiii iaaa aann nnnt tttt   Qt,Qt2,[Xn+s7*16]!
\ One register, offset of 9 bits: without writing back (LDUR), after the
\ access, unprivileged (LDTR), before the access
OP: sturb    0011 1000 000i iiii iiii 00nn nnnt tttt   Wt,[Xn+s9]
OP: ldurb    0011 1000 010i iiii iiii 00nn nnnt tttt   Wt,[Xn+s9]
OP: ldursb   0011 1000 100i iiii iiii 00nn nnnt tttt   Xt,[Xn+s9]
OP: ldursb   0011 1000 110i iiii iiii 00nn nnnt tttt   Wt,[Xn+s9]
OP: sturh    0111 1000 000i iiii iiii 00nn nnnt tttt   Wt,[Xn+s9]
OP: ldurh    0111 1000 010i iiii iiii 00nn nnnt tttt   Wt,[Xn+s9]
OP: ldursh   0111 1000 100i iiii iiii 00nn nnnt tttt   Xt,[Xn+s9]
OP: ldursh   0111 1000 110i iiii iiii 00nn nnnt tttt   Wt,[Xn+s9]
OP: stur     1011 1000 000i iiii iiii 00nn nnnt tttt   Wt,[Xn+s9]
OP: ldur     1011 1000 010i iiii iiii 00nn nnnt tttt   Wt,[Xn+s9]
OP: ldursw   1011 1000 100i iiii iiii 00nn nnnt tttt   Xt,[Xn+s9]
OP: stur     1111 1000 000i iiii iiii 00nn nnnt tttt   Xt,[Xn+s9]
OP: ldur     1111 1000 010i iiii iiii 00nn nnnt tttt   Xt,[Xn+s9]
OP: prfum    1111 1000 100i iiii iiii 00nn nnnt tttt   prfop,[Xn+s9]
OP: stur     0011 1100 000i iiii iiii 00nn nnnt tttt   Bt,[Xn+s9]
OP: ldur     0011 1100 010i iiii iiii 00nn nnnt tttt   Bt,[Xn+s9]
OP: stur     0011 1100 100i iiii iiii 00nn nnnt tttt   Qt,[Xn+s9]
OP: ldur     0011 1100 110i iiii iiii 00nn nnnt tttt   Qt,[Xn+s9]
OP: stur     0111 1100 000i iiii iiii 00nn nnnt tttt   Ht,[Xn+s9]
OP: ldur     0111 1100 010i iiii iiii 00nn nnnt tttt   Ht,[Xn+s9]
OP: stur     1011 1100 000i iiii iiii 00nn nnnt tttt   St,[Xn+s9]
OP: ldur     1011 1100 010i iiii iiii 00nn nnnt tttt   St,[Xn+s9]
OP: stur     1111 1100 000i iiii iiii 00nn nnnt tttt   Dt,[Xn+s9]
OP: ldur     1111 1100 010i iiii iiii 00nn nnnt tttt   Dt,[Xn+s9]
OP: strb     0011 1000 000i iiii iiii 01nn nnnt tttt   Wt,[Xn]+s9
OP: ldrb     0011 1000 010i iiii iiii 01nn nnnt tttt   Wt,[Xn]+s9
OP: ldrsb    0011 1000 100i iiii iiii 01nn nnnt tttt   Xt,[Xn]+s9
OP: ldrsb    0011 1000 110i iiii iiii 01nn nnnt tttt   Wt,[Xn]+s9
OP: strh     0111 1000 000i iiii iiii 01nn nnnt tttt   Wt,[Xn]+s9
OP: ldrh     0111 1000 010i iiii iiii 01nn nnnt tttt   Wt,[Xn]+s9
OP: ldrsh    0111 1000 100i iiii iiii 01nn nnnt tttt   Xt,[Xn]+s9
OP: ldrsh    0111 1000 110i iiii iiii 01nn nnnt tttt   Wt,[Xn]+s9
OP: str      1011 1000 000i iiii iiii 01nn nnnt tttt   Wt,[Xn]+s9
OP: ldr      1011 1000 010i iiii iiii 01nn nnnt tttt   Wt,[Xn]+s9
OP: ldrsw    1011 1000 100i iiii iiii 01nn nnnt tttt   Xt,[Xn]+s9
OP: str      1111 1000 000i iiii iiii 01nn nnnt tttt   Xt,[Xn]+s9
OP: ldr      1111 1000 010i iiii iiii 01nn nnnt tttt   Xt,[Xn]+s9
OP: str      0011 1100 000i iiii iiii 01nn nnnt tttt   Bt,[Xn]+s9
OP: ldr      0011 1100 010i iiii iiii 01nn nnnt tttt   Bt,[Xn]+s9
OP: str      0011 1100 100i iiii iiii 01nn nnnt tttt   Qt,[Xn]+s9
OP: ldr      0011 1100 110i iiii iiii 01nn nnnt tttt   Qt,[Xn]+s9
OP: str      0111 1100 000i iiii iiii 01nn nnnt tttt   Ht,[Xn]+s9
OP: ldr      0111 1100 010i iiii iiii 01nn nnnt tttt   Ht,[Xn]+s9
OP: str      1011 1100 000i iiii iiii 01nn nnnt tttt   St,[Xn]+s9
OP: ldr      1011 1100 010i iiii iiii 01nn nnnt tttt   St,[Xn]+s9
OP: str      1111 1100 000i iiii iiii 01nn nnnt tttt   Dt,[Xn]+s9
OP: ldr      1111 1100 010i iiii iiii 01nn nnnt tttt   Dt,[Xn]+s9
OP: sttrb    0011 1000 000i iiii iiii 10nn nnnt tttt   Wt,[Xn+s9]
OP: ldtrb    0011 1000 010i iiii iiii 10nn nnnt tttt   Wt,[Xn+s9]
OP: ldtrsb   0011 1000 100i iiii iiii 10nn nnnt tttt   Xt,[Xn+s9]
OP: ldtrsb   0011 1000 110i iiii iiii 10nn nnnt tttt   Wt,[Xn+s9]
OP: sttrh    0111 1000 000i iiii iiii 10nn nnnt tttt   Wt,[Xn+s9]
OP: ldtrh    0111 1000 010i iiii iiii 10nn nnnt tttt   Wt,[Xn+s9]
OP: ldtrsh   0111 1000 100i iiii iiii 10nn nnnt tttt   Xt,[Xn+s9]
OP: ldtrsh   0111 1000 110i iiii iiii 10nn nnnt tttt   Wt,[Xn+s9]
OP: sttr     1011 1000 000i iiii iiii 10nn nnnt tttt   Wt,[Xn+s9]
OP: ldtr     1011 1000 010i iiii iiii 10nn nnnt tttt   Wt,[Xn+s9]
OP: ldtrsw   1011 1000 100i iiii iiii 10nn nnnt tttt   Xt,[Xn+s9]
OP: sttr     1111 1000 000i iiii iiii 10nn nnnt tttt   Xt,[Xn+s9]
OP: ldtr     1111 1000 010i iiii iiii 10nn nnnt tttt   Xt,[Xn+s9]
OP: strb     0011 1000 000i iiii iiii 11nn nnnt tttt   Wt,[Xn+s9]!
OP: ldrb     0011 1000 010i iiii iiii 11nn nnnt tttt   Wt,[Xn+s9]!
OP: ldrsb    0011 1000 100i iiii iiii 11nn nnnt tttt   Xt,[Xn+s9]!
OP: ldrsb    0011 1000 110i iiii iiii 11nn nnnt tttt   Wt,[Xn+s9]!
OP: strh     0111 1000 000i iiii iiii 11nn nnnt tttt   Wt,[Xn+s9]!
OP: ldrh     0111 1000 010i iiii iiii 11nn nnnt tttt   Wt,[Xn+s9]!
OP: ldrsh    0111 1000 100i iiii iiii 11nn nnnt tttt   Xt,[Xn+s9]!
OP: ldrsh    0111 1000 110i iiii iiii 11nn nnnt tttt   Wt,[Xn+s9]!
OP: str      1011 1000 000i iiii iiii 11nn nnnt tttt   Wt,[Xn+s9]!
OP: ldr      1011 1000 010i iiii iiii 11nn nnnt tttt   Wt,[Xn+s9]!
OP: ldrsw    1011 1000 100i iiii iiii 11nn nnnt tttt   Xt,[Xn+s9]!
OP: str      1111 1000 000i iiii iiii 11nn nnnt tttt   Xt,[Xn+s9]!
OP: ldr      1111 1000 010i iiii iiii 11nn nnnt tttt   Xt,[Xn+s9]!
OP: str      0011 1100 000i iiii iiii 11nn nnnt tttt   Bt,[Xn+s9]!
OP: ldr      0011 1100 010i iiii iiii 11nn nnnt tttt   Bt,[Xn+s9]!
OP: str      0011 1100 100i iiii iiii 11nn nnnt tttt   Qt,[Xn+s9]!
OP: ldr      0011 1100 110i iiii iiii 11nn nnnt tttt   Qt,[Xn+s9]!
OP: str      0111 1100 000i iiii iiii 11nn nnnt tttt   Ht,[Xn+s9]!
OP: ldr      0111 1100 010i iiii iiii 11nn nnnt tttt   Ht,[Xn+s9]!
OP: str      1011 1100 000i iiii iiii 11nn nnnt tttt   St,[Xn+s9]!
OP: ldr      1011 1100 010i iiii iiii 11nn nnnt tttt   St,[Xn+s9]!
OP: str      1111 1100 000i iiii iiii 11nn nnnt tttt   Dt,[Xn+s9]!
OP: ldr      1111 1100 010i iiii iiii 11nn nnnt tttt   Dt,[Xn+s9]!
\ Atomic (LSE): A and R in bits 23 and 22; ST... when Rt is 31 and A is 0
OP: stadd    zz11 1000 0R1s ssss 0000 00nn nnn1 1111   .l,.sz,Rs.sz,[Xn]
OP: ldadd    zz11 1000 AR1s ssss 0000 00nn nnnt tttt   .ar,.sz,Rs.sz,Rt.sz,[Xn]
OP: stclr    zz11 1000 0R1s ssss 0001 00nn nnn1 1111   .l,.sz,Rs.sz,[Xn]
OP: ldclr    zz11 1000 AR1s ssss 0001 00nn nnnt tttt   .ar,.sz,Rs.sz,Rt.sz,[Xn]
OP: steor    zz11 1000 0R1s ssss 0010 00nn nnn1 1111   .l,.sz,Rs.sz,[Xn]
OP: ldeor    zz11 1000 AR1s ssss 0010 00nn nnnt tttt   .ar,.sz,Rs.sz,Rt.sz,[Xn]
OP: stset    zz11 1000 0R1s ssss 0011 00nn nnn1 1111   .l,.sz,Rs.sz,[Xn]
OP: ldset    zz11 1000 AR1s ssss 0011 00nn nnnt tttt   .ar,.sz,Rs.sz,Rt.sz,[Xn]
OP: stsmax   zz11 1000 0R1s ssss 0100 00nn nnn1 1111   .l,.sz,Rs.sz,[Xn]
OP: ldsmax   zz11 1000 AR1s ssss 0100 00nn nnnt tttt   .ar,.sz,Rs.sz,Rt.sz,[Xn]
OP: stsmin   zz11 1000 0R1s ssss 0101 00nn nnn1 1111   .l,.sz,Rs.sz,[Xn]
OP: ldsmin   zz11 1000 AR1s ssss 0101 00nn nnnt tttt   .ar,.sz,Rs.sz,Rt.sz,[Xn]
OP: stumax   zz11 1000 0R1s ssss 0110 00nn nnn1 1111   .l,.sz,Rs.sz,[Xn]
OP: ldumax   zz11 1000 AR1s ssss 0110 00nn nnnt tttt   .ar,.sz,Rs.sz,Rt.sz,[Xn]
OP: stumin   zz11 1000 0R1s ssss 0111 00nn nnn1 1111   .l,.sz,Rs.sz,[Xn]
OP: ldumin   zz11 1000 AR1s ssss 0111 00nn nnnt tttt   .ar,.sz,Rs.sz,Rt.sz,[Xn]
OP: swp      zz11 1000 AR1s ssss 1000 00nn nnnt tttt   .ar,.sz,Rs.sz,Rt.sz,[Xn]
OP: ldapr    zz11 1000 1011 1111 1100 00nn nnnt tttt   .sz,Rt.sz,[Xn]
OP: st64b    1111 1000 0011 1111 1001 00nn nnnt tttt   Xt.64b,[Xn]
OP: ld64b    1111 1000 0011 1111 1101 00nn nnnt tttt   Xt.64b,[Xn]
OP: st64bv   1111 1000 001s ssss 1011 00nn nnnt tttt   Xs,Xt.64b,[Xn]
OP: st64bv0  1111 1000 001s ssss 1010 00nn nnnt tttt   Xs,Xt.64b,[Xn]
\ One register, offset in a register
OP: strb     0011 1000 001m mmmm ooos 10nn nnnt tttt   Wt,[Xn+Rm]
OP: ldrb     0011 1000 011m mmmm ooos 10nn nnnt tttt   Wt,[Xn+Rm]
OP: ldrsb    0011 1000 101m mmmm ooos 10nn nnnt tttt   Xt,[Xn+Rm]
OP: ldrsb    0011 1000 111m mmmm ooos 10nn nnnt tttt   Wt,[Xn+Rm]
OP: strh     0111 1000 001m mmmm ooos 10nn nnnt tttt   Wt,[Xn+Rm*2]
OP: ldrh     0111 1000 011m mmmm ooos 10nn nnnt tttt   Wt,[Xn+Rm*2]
OP: ldrsh    0111 1000 101m mmmm ooos 10nn nnnt tttt   Xt,[Xn+Rm*2]
OP: ldrsh    0111 1000 111m mmmm ooos 10nn nnnt tttt   Wt,[Xn+Rm*2]
OP: str      1011 1000 001m mmmm ooos 10nn nnnt tttt   Wt,[Xn+Rm*4]
OP: ldr      1011 1000 011m mmmm ooos 10nn nnnt tttt   Wt,[Xn+Rm*4]
OP: ldrsw    1011 1000 101m mmmm ooos 10nn nnnt tttt   Xt,[Xn+Rm*4]
OP: str      1111 1000 001m mmmm ooos 10nn nnnt tttt   Xt,[Xn+Rm*8]
OP: ldr      1111 1000 011m mmmm ooos 10nn nnnt tttt   Xt,[Xn+Rm*8]
OP: prfm     1111 1000 101m mmmm ooos 10nn nnnt tttt   prfop,[Xn+Rm*8]
OP: str      0011 1100 001m mmmm ooos 10nn nnnt tttt   Bt,[Xn+Rm]
OP: ldr      0011 1100 011m mmmm ooos 10nn nnnt tttt   Bt,[Xn+Rm]
OP: str      0011 1100 101m mmmm ooos 10nn nnnt tttt   Qt,[Xn+Rm*16]
OP: ldr      0011 1100 111m mmmm ooos 10nn nnnt tttt   Qt,[Xn+Rm*16]
OP: str      0111 1100 001m mmmm ooos 10nn nnnt tttt   Ht,[Xn+Rm*2]
OP: ldr      0111 1100 011m mmmm ooos 10nn nnnt tttt   Ht,[Xn+Rm*2]
OP: str      1011 1100 001m mmmm ooos 10nn nnnt tttt   St,[Xn+Rm*4]
OP: ldr      1011 1100 011m mmmm ooos 10nn nnnt tttt   St,[Xn+Rm*4]
OP: str      1111 1100 001m mmmm ooos 10nn nnnt tttt   Dt,[Xn+Rm*8]
OP: ldr      1111 1100 011m mmmm ooos 10nn nnnt tttt   Dt,[Xn+Rm*8]
\ With pointer authentication (key A or B in bit 23)
OP: ldraa    1111 1000 0s1i iiii iiii 01nn nnnt tttt   Xt,[Xn+s10*8]
OP: ldraa    1111 1000 0s1i iiii iiii 11nn nnnt tttt   Xt,[Xn+s10*8]!
OP: ldrab    1111 1000 1s1i iiii iiii 01nn nnnt tttt   Xt,[Xn+s10*8]
OP: ldrab    1111 1000 1s1i iiii iiii 11nn nnnt tttt   Xt,[Xn+s10*8]!
\ One register, an offset of 12 bits without sign, times the size
OP: strb     0011 1001 00ii iiii iiii iinn nnnt tttt   Wt,[Xn+u12]
OP: ldrb     0011 1001 01ii iiii iiii iinn nnnt tttt   Wt,[Xn+u12]
OP: ldrsb    0011 1001 10ii iiii iiii iinn nnnt tttt   Xt,[Xn+u12]
OP: ldrsb    0011 1001 11ii iiii iiii iinn nnnt tttt   Wt,[Xn+u12]
OP: strh     0111 1001 00ii iiii iiii iinn nnnt tttt   Wt,[Xn+u12*2]
OP: ldrh     0111 1001 01ii iiii iiii iinn nnnt tttt   Wt,[Xn+u12*2]
OP: ldrsh    0111 1001 10ii iiii iiii iinn nnnt tttt   Xt,[Xn+u12*2]
OP: ldrsh    0111 1001 11ii iiii iiii iinn nnnt tttt   Wt,[Xn+u12*2]
OP: str      1011 1001 00ii iiii iiii iinn nnnt tttt   Wt,[Xn+u12*4]
OP: ldr      1011 1001 01ii iiii iiii iinn nnnt tttt   Wt,[Xn+u12*4]
OP: ldrsw    1011 1001 10ii iiii iiii iinn nnnt tttt   Xt,[Xn+u12*4]
OP: str      1111 1001 00ii iiii iiii iinn nnnt tttt   Xt,[Xn+u12*8]
OP: ldr      1111 1001 01ii iiii iiii iinn nnnt tttt   Xt,[Xn+u12*8]
OP: prfm     1111 1001 10ii iiii iiii iinn nnnt tttt   prfop,[Xn+u12*8]
OP: str      0011 1101 00ii iiii iiii iinn nnnt tttt   Bt,[Xn+u12]
OP: ldr      0011 1101 01ii iiii iiii iinn nnnt tttt   Bt,[Xn+u12]
OP: str      0011 1101 10ii iiii iiii iinn nnnt tttt   Qt,[Xn+u12*16]
OP: ldr      0011 1101 11ii iiii iiii iinn nnnt tttt   Qt,[Xn+u12*16]
OP: str      0111 1101 00ii iiii iiii iinn nnnt tttt   Ht,[Xn+u12*2]
OP: ldr      0111 1101 01ii iiii iiii iinn nnnt tttt   Ht,[Xn+u12*2]
OP: str      1011 1101 00ii iiii iiii iinn nnnt tttt   St,[Xn+u12*4]
OP: ldr      1011 1101 01ii iiii iiii iinn nnnt tttt   St,[Xn+u12*4]
OP: str      1111 1101 00ii iiii iiii iinn nnnt tttt   Dt,[Xn+u12*8]
OP: ldr      1111 1101 01ii iiii iiii iinn nnnt tttt   Dt,[Xn+u12*8]
\ Memory tags (MTE)
OP: stzgm    1101 1001 0010 0000 0000 00nn nnnt tttt   Xt,[Xn]
OP: stg      1101 1001 001i iiii iiii 01nn nnnt tttt   Xt|SP,[Xn]+s9*16
OP: stg      1101 1001 001i iiii iiii 10nn nnnt tttt   Xt|SP,[Xn+s9*16]
OP: stg      1101 1001 001i iiii iiii 11nn nnnt tttt   Xt|SP,[Xn+s9*16]!
OP: ldg      1101 1001 011i iiii iiii 00nn nnnt tttt   Xt,[Xn+s9*16]
OP: stzg     1101 1001 011i iiii iiii 01nn nnnt tttt   Xt|SP,[Xn]+s9*16
OP: stzg     1101 1001 011i iiii iiii 10nn nnnt tttt   Xt|SP,[Xn+s9*16]
OP: stzg     1101 1001 011i iiii iiii 11nn nnnt tttt   Xt|SP,[Xn+s9*16]!
OP: stgm     1101 1001 1010 0000 0000 00nn nnnt tttt   Xt,[Xn]
OP: st2g     1101 1001 101i iiii iiii 01nn nnnt tttt   Xt|SP,[Xn]+s9*16
OP: st2g     1101 1001 101i iiii iiii 10nn nnnt tttt   Xt|SP,[Xn+s9*16]
OP: st2g     1101 1001 101i iiii iiii 11nn nnnt tttt   Xt|SP,[Xn+s9*16]!
OP: ldgm     1101 1001 1110 0000 0000 00nn nnnt tttt   Xt,[Xn]
OP: stz2g    1101 1001 111i iiii iiii 01nn nnnt tttt   Xt|SP,[Xn]+s9*16
OP: stz2g    1101 1001 111i iiii iiii 10nn nnnt tttt   Xt|SP,[Xn+s9*16]
OP: stz2g    1101 1001 111i iiii iiii 11nn nnnt tttt   Xt|SP,[Xn+s9*16]!

\ ---- Data processing, registers (ARM C4.1.89) ---------------------------
OP: subp     1001 1010 110m mmmm 0000 00nn nnnd dddd   Xd,Xn|SP,Xm|SP
OP: udiv     s001 1010 110m mmmm 0000 10nn nnnd dddd   Rd,Rn,Rm
OP: sdiv     s001 1010 110m mmmm 0000 11nn nnnd dddd   Rd,Rn,Rm
OP: irg      1001 1010 1101 1111 0001 00nn nnnd dddd   Xd|SP,Xn|SP
OP: irg      1001 1010 110m mmmm 0001 00nn nnnd dddd   Xd|SP,Xn|SP,Xm
OP: gmi      1001 1010 110m mmmm 0001 01nn nnnd dddd   Xd,Xn|SP,Xm
OP: lsl      s001 1010 110m mmmm 0010 00nn nnnd dddd   Rd,Rn,Rm
OP: lsr      s001 1010 110m mmmm 0010 01nn nnnd dddd   Rd,Rn,Rm
OP: asr      s001 1010 110m mmmm 0010 10nn nnnd dddd   Rd,Rn,Rm
OP: ror      s001 1010 110m mmmm 0010 11nn nnnd dddd   Rd,Rn,Rm
OP: pacga    1001 1010 110m mmmm 0011 00nn nnnd dddd   Xd,Xn,Xm|SP
OP: crc32b   0001 1010 110m mmmm 0100 00nn nnnd dddd   Wd,Wn,Wm
OP: crc32h   0001 1010 110m mmmm 0100 01nn nnnd dddd   Wd,Wn,Wm
OP: crc32w   0001 1010 110m mmmm 0100 10nn nnnd dddd   Wd,Wn,Wm
OP: crc32x   1001 1010 110m mmmm 0100 11nn nnnd dddd   Wd,Wn,Xm
OP: crc32cb  0001 1010 110m mmmm 0101 00nn nnnd dddd   Wd,Wn,Wm
OP: crc32ch  0001 1010 110m mmmm 0101 01nn nnnd dddd   Wd,Wn,Wm
OP: crc32cw  0001 1010 110m mmmm 0101 10nn nnnd dddd   Wd,Wn,Wm
OP: crc32cx  1001 1010 110m mmmm 0101 11nn nnnd dddd   Wd,Wn,Xm
OP: subps    1011 1010 110m mmmm 0000 00nn nnnd dddd   Xd,Xn|SP,Xm|SP
OP: rbit     s101 1010 1100 0000 0000 00nn nnnd dddd   Rd,Rn
OP: rev16    s101 1010 1100 0000 0000 01nn nnnd dddd   Rd,Rn
OP: rev      0101 1010 1100 0000 0000 10nn nnnd dddd   Rd,Rn
OP: rev32    1101 1010 1100 0000 0000 10nn nnnd dddd   Rd,Rn
OP: rev      1101 1010 1100 0000 0000 11nn nnnd dddd   Rd,Rn
OP: clz      s101 1010 1100 0000 0001 00nn nnnd dddd   Rd,Rn
OP: cls      s101 1010 1100 0000 0001 01nn nnnd dddd   Rd,Rn
OP: pacia    1101 1010 1100 0001 0000 00nn nnnd dddd   Xd,Xn|SP
OP: pacib    1101 1010 1100 0001 0000 01nn nnnd dddd   Xd,Xn|SP
OP: pacda    1101 1010 1100 0001 0000 10nn nnnd dddd   Xd,Xn|SP
OP: pacdb    1101 1010 1100 0001 0000 11nn nnnd dddd   Xd,Xn|SP
OP: autia    1101 1010 1100 0001 0001 00nn nnnd dddd   Xd,Xn|SP
OP: autib    1101 1010 1100 0001 0001 01nn nnnd dddd   Xd,Xn|SP
OP: autda    1101 1010 1100 0001 0001 10nn nnnd dddd   Xd,Xn|SP
OP: autdb    1101 1010 1100 0001 0001 11nn nnnd dddd   Xd,Xn|SP
OP: paciza   1101 1010 1100 0001 0010 0011 111d dddd   Xd
OP: pacizb   1101 1010 1100 0001 0010 0111 111d dddd   Xd
OP: pacdza   1101 1010 1100 0001 0010 1011 111d dddd   Xd
OP: pacdzb   1101 1010 1100 0001 0010 1111 111d dddd   Xd
OP: autiza   1101 1010 1100 0001 0011 0011 111d dddd   Xd
OP: autizb   1101 1010 1100 0001 0011 0111 111d dddd   Xd
OP: autdza   1101 1010 1100 0001 0011 1011 111d dddd   Xd
OP: autdzb   1101 1010 1100 0001 0011 1111 111d dddd   Xd
OP: xpaci    1101 1010 1100 0001 0100 0011 111d dddd   Xd
OP: xpacd    1101 1010 1100 0001 0100 0111 111d dddd   Xd
OP: and      s000 1010 hh0m mmmm iiii iinn nnnd dddd   Rd,Rn,Rm.lshift
OP: bic      s000 1010 hh1m mmmm iiii iinn nnnd dddd   Rd,Rn,Rm.lshift
OP: mov      s010 1010 000m mmmm 0000 0011 111d dddd   Rd,Rm
OP: orr      s010 1010 hh0m mmmm iiii iinn nnnd dddd   Rd,Rn,Rm.lshift
OP: mvn      s010 1010 hh1m mmmm iiii ii11 111d dddd   Rd,Rm.lshift
OP: orn      s010 1010 hh1m mmmm iiii iinn nnnd dddd   Rd,Rn,Rm.lshift
OP: eor      s100 1010 hh0m mmmm iiii iinn nnnd dddd   Rd,Rn,Rm.lshift
OP: eon      s100 1010 hh1m mmmm iiii iinn nnnd dddd   Rd,Rn,Rm.lshift
OP: tst      s110 1010 hh0m mmmm iiii iinn nnn1 1111   Rn,Rm.lshift
OP: ands     s110 1010 hh0m mmmm iiii iinn nnnd dddd   Rd,Rn,Rm.lshift
OP: bics     s110 1010 hh1m mmmm iiii iinn nnnd dddd   Rd,Rn,Rm.lshift
OP: add      s000 1011 hh0m mmmm iiii iinn nnnd dddd   Rd,Rn,Rm.shift
OP: cmn      s010 1011 hh0m mmmm iiii iinn nnn1 1111   Rn,Rm.shift
OP: adds     s010 1011 hh0m mmmm iiii iinn nnnd dddd   Rd,Rn,Rm.shift
OP: neg      s100 1011 hh0m mmmm iiii ii11 111d dddd   Rd,Rm.shift
OP: sub      s100 1011 hh0m mmmm iiii iinn nnnd dddd   Rd,Rn,Rm.shift
OP: cmp      s110 1011 hh0m mmmm iiii iinn nnn1 1111   Rn,Rm.shift
OP: negs     s110 1011 hh0m mmmm iiii ii11 111d dddd   Rd,Rm.shift
OP: subs     s110 1011 hh0m mmmm iiii iinn nnnd dddd   Rd,Rn,Rm.shift
OP: add      s000 1011 001m mmmm oooi iinn nnnd dddd   Rd|SP,Rn|SP,Rm.ext
OP: cmn      s010 1011 001m mmmm oooi iinn nnn1 1111   Rn|SP,Rm.exts
OP: adds     s010 1011 001m mmmm oooi iinn nnnd dddd   Rd,Rn|SP,Rm.exts
OP: sub      s100 1011 001m mmmm oooi iinn nnnd dddd   Rd|SP,Rn|SP,Rm.ext
OP: cmp      s110 1011 001m mmmm oooi iinn nnn1 1111   Rn|SP,Rm.exts
OP: subs     s110 1011 001m mmmm oooi iinn nnnd dddd   Rd,Rn|SP,Rm.exts
OP: adc      s001 1010 000m mmmm 0000 00nn nnnd dddd   Rd,Rn,Rm
OP: adcs     s011 1010 000m mmmm 0000 00nn nnnd dddd   Rd,Rn,Rm
OP: ngc      s101 1010 000m mmmm 0000 0011 111d dddd   Rd,Rm
OP: sbc      s101 1010 000m mmmm 0000 00nn nnnd dddd   Rd,Rn,Rm
OP: ngcs     s111 1010 000m mmmm 0000 0011 111d dddd   Rd,Rm
OP: sbcs     s111 1010 000m mmmm 0000 00nn nnnd dddd   Rd,Rn,Rm
OP: rmif     1011 1010 000i iiii i000 01nn nnn0 mmmm   Xn,#rmif,#mask
OP: setf8    0011 1010 0000 0000 0000 10nn nnn0 1101   Wn
OP: setf16   0011 1010 0000 0000 0100 10nn nnn0 1101   Wn
OP: ccmn     s011 1010 010m mmmm cccc 00nn nnn0 ffff   Rn,Rm,#nzcv,cond
OP: ccmp     s111 1010 010m mmmm cccc 00nn nnn0 ffff   Rn,Rm,#nzcv,cond
OP: ccmn     s011 1010 010i iiii cccc 10nn nnn0 ffff   Rn,#imm5,#nzcv,cond
OP: ccmp     s111 1010 010i iiii cccc 10nn nnn0 ffff   Rn,#imm5,#nzcv,cond
WHEN: CSET?
OP: cset     s001 1010 1001 1111 cccc 0111 111d dddd   Rd,cond.inv
WHEN: CINC?
OP: cinc     s001 1010 100m mmmm cccc 01nn nnnd dddd   Rd,Rn,cond.inv
OP: csel     s001 1010 100m mmmm cccc 00nn nnnd dddd   Rd,Rn,Rm,cond
OP: csinc    s001 1010 100m mmmm cccc 01nn nnnd dddd   Rd,Rn,Rm,cond
WHEN: CSET?
OP: csetm    s101 1010 1001 1111 cccc 0011 111d dddd   Rd,cond.inv
WHEN: CINC?
OP: cinv     s101 1010 100m mmmm cccc 00nn nnnd dddd   Rd,Rn,cond.inv
OP: csinv    s101 1010 100m mmmm cccc 00nn nnnd dddd   Rd,Rn,Rm,cond
WHEN: CNEG?
OP: cneg     s101 1010 100m mmmm cccc 01nn nnnd dddd   Rd,Rn,cond.inv
OP: csneg    s101 1010 100m mmmm cccc 01nn nnnd dddd   Rd,Rn,Rm,cond
OP: mul      s001 1011 000m mmmm 0111 11nn nnnd dddd   Rd,Rn,Rm
OP: madd     s001 1011 000m mmmm 0aaa aann nnnd dddd   Rd,Rn,Rm,Ra
OP: mneg     s001 1011 000m mmmm 1111 11nn nnnd dddd   Rd,Rn,Rm
OP: msub     s001 1011 000m mmmm 1aaa aann nnnd dddd   Rd,Rn,Rm,Ra
OP: smull    1001 1011 001m mmmm 0111 11nn nnnd dddd   Xd,Wn,Wm
OP: smaddl   1001 1011 001m mmmm 0aaa aann nnnd dddd   Xd,Wn,Wm,Xa
OP: smnegl   1001 1011 001m mmmm 1111 11nn nnnd dddd   Xd,Wn,Wm
OP: smsubl   1001 1011 001m mmmm 1aaa aann nnnd dddd   Xd,Wn,Wm,Xa
OP: smulh    1001 1011 010m mmmm 0aaa aann nnnd dddd   Xd,Xn,Xm
OP: umull    1001 1011 101m mmmm 0111 11nn nnnd dddd   Xd,Wn,Wm
OP: umaddl   1001 1011 101m mmmm 0aaa aann nnnd dddd   Xd,Wn,Wm,Xa
OP: umnegl   1001 1011 101m mmmm 1111 11nn nnnd dddd   Xd,Wn,Wm
OP: umsubl   1001 1011 101m mmmm 1aaa aann nnnd dddd   Xd,Wn,Wm,Xa
OP: umulh    1001 1011 110m mmmm 0aaa aann nnnd dddd   Xd,Xn,Xm

\ ---- Reserved: UDF (ARM C4.1.84) -----------------------------------------
OP: udf      0000 0000 0000 0000 iiii iiii iiii iiii   #udf

\ ==== Names of the system ==================================================
\ The system registers of MRS and MSR: op0 op1 CRn CRm op2 SR: access
\ name, as the ARM manual writes them. access: r- for MRS, -w for MSR,
\ rw for both. The registers of the base architecture, of its debug and
\ performance monitors, timers, the GIC, PAC and MTE; not those of SVE,
\ SME, trace, statistical profiling, activity monitors, RAS, MPAM and
\ some newer ones. A register without a name shows as S3_0_C15_C2_0.

: STR, ( a u -- )   \ a counted string, without alignment
  DUP C,  0 ?DO  DUP I + C@ C,  LOOP  DROP ;
: SR: ( op0 op1 crn crm op2 "access" "name" -- )
  SWAP 3 LSHIFT OR  SWAP 7 LSHIFT OR  SWAP 11 LSHIFT OR  SWAP 14 LSHIFT OR
  DUP C,  8 RSHIFT C,
  PARSE-NAME DROP  DUP C@ 'r' = 1 AND  SWAP 1+ C@ 'w' = 2 AND OR  C,
  PARSE-NAME STR, ;
CREATE SYSREGS
2 0 0 0 2 SR: rw OSDTRRX_EL1   2 0 0 0 4 SR: rw DBGBVR0_EL1
2 0 0 0 5 SR: rw DBGBCR0_EL1   2 0 0 0 6 SR: rw DBGWVR0_EL1
2 0 0 0 7 SR: rw DBGWCR0_EL1   2 0 0 1 4 SR: rw DBGBVR1_EL1
2 0 0 1 5 SR: rw DBGBCR1_EL1   2 0 0 1 6 SR: rw DBGWVR1_EL1
2 0 0 1 7 SR: rw DBGWCR1_EL1   2 0 0 2 0 SR: rw MDCCINT_EL1
2 0 0 2 2 SR: rw MDSCR_EL1   2 0 0 2 4 SR: rw DBGBVR2_EL1
2 0 0 2 5 SR: rw DBGBCR2_EL1   2 0 0 2 6 SR: rw DBGWVR2_EL1
2 0 0 2 7 SR: rw DBGWCR2_EL1   2 0 0 3 2 SR: rw OSDTRTX_EL1
2 0 0 3 4 SR: rw DBGBVR3_EL1   2 0 0 3 5 SR: rw DBGBCR3_EL1
2 0 0 3 6 SR: rw DBGWVR3_EL1   2 0 0 3 7 SR: rw DBGWCR3_EL1
2 0 0 4 4 SR: rw DBGBVR4_EL1   2 0 0 4 5 SR: rw DBGBCR4_EL1
2 0 0 4 6 SR: rw DBGWVR4_EL1   2 0 0 4 7 SR: rw DBGWCR4_EL1
2 0 0 5 4 SR: rw DBGBVR5_EL1   2 0 0 5 5 SR: rw DBGBCR5_EL1
2 0 0 5 6 SR: rw DBGWVR5_EL1   2 0 0 5 7 SR: rw DBGWCR5_EL1
2 0 0 6 2 SR: rw OSECCR_EL1   2 0 0 6 4 SR: rw DBGBVR6_EL1
2 0 0 6 5 SR: rw DBGBCR6_EL1   2 0 0 6 6 SR: rw DBGWVR6_EL1
2 0 0 6 7 SR: rw DBGWCR6_EL1   2 0 0 7 4 SR: rw DBGBVR7_EL1
2 0 0 7 5 SR: rw DBGBCR7_EL1   2 0 0 7 6 SR: rw DBGWVR7_EL1
2 0 0 7 7 SR: rw DBGWCR7_EL1   2 0 0 8 4 SR: rw DBGBVR8_EL1
2 0 0 8 5 SR: rw DBGBCR8_EL1   2 0 0 8 6 SR: rw DBGWVR8_EL1
2 0 0 8 7 SR: rw DBGWCR8_EL1   2 0 0 9 4 SR: rw DBGBVR9_EL1
2 0 0 9 5 SR: rw DBGBCR9_EL1   2 0 0 9 6 SR: rw DBGWVR9_EL1
2 0 0 9 7 SR: rw DBGWCR9_EL1   2 0 0 10 4 SR: rw DBGBVR10_EL1
2 0 0 10 5 SR: rw DBGBCR10_EL1   2 0 0 10 6 SR: rw DBGWVR10_EL1
2 0 0 10 7 SR: rw DBGWCR10_EL1   2 0 0 11 4 SR: rw DBGBVR11_EL1
2 0 0 11 5 SR: rw DBGBCR11_EL1   2 0 0 11 6 SR: rw DBGWVR11_EL1
2 0 0 11 7 SR: rw DBGWCR11_EL1   2 0 0 12 4 SR: rw DBGBVR12_EL1
2 0 0 12 5 SR: rw DBGBCR12_EL1   2 0 0 12 6 SR: rw DBGWVR12_EL1
2 0 0 12 7 SR: rw DBGWCR12_EL1   2 0 0 13 4 SR: rw DBGBVR13_EL1
2 0 0 13 5 SR: rw DBGBCR13_EL1   2 0 0 13 6 SR: rw DBGWVR13_EL1
2 0 0 13 7 SR: rw DBGWCR13_EL1   2 0 0 14 4 SR: rw DBGBVR14_EL1
2 0 0 14 5 SR: rw DBGBCR14_EL1   2 0 0 14 6 SR: rw DBGWVR14_EL1
2 0 0 14 7 SR: rw DBGWCR14_EL1   2 0 0 15 4 SR: rw DBGBVR15_EL1
2 0 0 15 5 SR: rw DBGBCR15_EL1   2 0 0 15 6 SR: rw DBGWVR15_EL1
2 0 0 15 7 SR: rw DBGWCR15_EL1   2 0 1 0 0 SR: r- MDRAR_EL1
2 0 1 0 4 SR: -w OSLAR_EL1   2 0 1 1 4 SR: r- OSLSR_EL1
2 0 1 3 4 SR: rw OSDLR_EL1   2 0 1 4 4 SR: rw DBGPRCR_EL1
2 0 7 8 6 SR: rw DBGCLAIMSET_EL1   2 0 7 9 6 SR: rw DBGCLAIMCLR_EL1
2 0 7 14 6 SR: r- DBGAUTHSTATUS_EL1   2 3 0 1 0 SR: r- MDCCSR_EL0
2 3 0 4 0 SR: rw DBGDTR_EL0   2 3 0 5 0 SR: r- DBGDTRRX_EL0
2 3 0 5 0 SR: -w DBGDTRTX_EL0   2 4 0 7 0 SR: rw DBGVCR32_EL2
3 0 0 0 0 SR: r- MIDR_EL1   3 0 0 0 5 SR: r- MPIDR_EL1
3 0 0 0 6 SR: r- REVIDR_EL1   3 0 0 1 0 SR: r- ID_PFR0_EL1
3 0 0 1 1 SR: r- ID_PFR1_EL1   3 0 0 1 2 SR: r- ID_DFR0_EL1
3 0 0 1 3 SR: r- ID_AFR0_EL1   3 0 0 1 4 SR: r- ID_MMFR0_EL1
3 0 0 1 5 SR: r- ID_MMFR1_EL1   3 0 0 1 6 SR: r- ID_MMFR2_EL1
3 0 0 1 7 SR: r- ID_MMFR3_EL1   3 0 0 2 0 SR: r- ID_ISAR0_EL1
3 0 0 2 1 SR: r- ID_ISAR1_EL1   3 0 0 2 2 SR: r- ID_ISAR2_EL1
3 0 0 2 3 SR: r- ID_ISAR3_EL1   3 0 0 2 4 SR: r- ID_ISAR4_EL1
3 0 0 2 5 SR: r- ID_ISAR5_EL1   3 0 0 2 6 SR: r- ID_MMFR4_EL1
3 0 0 2 7 SR: r- ID_ISAR6_EL1   3 0 0 3 0 SR: r- MVFR0_EL1
3 0 0 3 1 SR: r- MVFR1_EL1   3 0 0 3 2 SR: r- MVFR2_EL1
3 0 0 3 4 SR: r- ID_PFR2_EL1   3 0 0 3 5 SR: r- ID_DFR1_EL1
3 0 0 3 6 SR: r- ID_MMFR5_EL1   3 0 0 4 0 SR: r- ID_AA64PFR0_EL1
3 0 0 4 1 SR: r- ID_AA64PFR1_EL1   3 0 0 4 2 SR: r- ID_AA64PFR2_EL1
3 0 0 4 4 SR: r- ID_AA64ZFR0_EL1   3 0 0 4 5 SR: r- ID_AA64SMFR0_EL1
3 0 0 4 7 SR: r- ID_AA64FPFR0_EL1   3 0 0 5 0 SR: r- ID_AA64DFR0_EL1
3 0 0 5 1 SR: r- ID_AA64DFR1_EL1   3 0 0 5 2 SR: r- ID_AA64DFR2_EL1
3 0 0 5 4 SR: r- ID_AA64AFR0_EL1   3 0 0 5 5 SR: r- ID_AA64AFR1_EL1
3 0 0 6 0 SR: r- ID_AA64ISAR0_EL1   3 0 0 6 1 SR: r- ID_AA64ISAR1_EL1
3 0 0 6 2 SR: r- ID_AA64ISAR2_EL1   3 0 0 6 3 SR: r- ID_AA64ISAR3_EL1
3 0 0 7 0 SR: r- ID_AA64MMFR0_EL1   3 0 0 7 1 SR: r- ID_AA64MMFR1_EL1
3 0 0 7 2 SR: r- ID_AA64MMFR2_EL1   3 0 0 7 3 SR: r- ID_AA64MMFR3_EL1
3 0 0 7 4 SR: r- ID_AA64MMFR4_EL1   3 0 1 0 0 SR: rw SCTLR_EL1
3 0 1 0 1 SR: rw ACTLR_EL1   3 0 1 0 2 SR: rw CPACR_EL1
3 0 1 0 3 SR: rw SCTLR2_EL1   3 0 1 0 5 SR: rw RGSR_EL1
3 0 1 0 6 SR: rw GCR_EL1   3 0 1 4 3 SR: rw SCTLR2MASK_EL1
3 0 1 4 7 SR: rw SCTLR2ALIAS_EL1   3 0 2 0 0 SR: rw TTBR0_EL1
3 0 2 0 1 SR: rw TTBR1_EL1   3 0 2 0 2 SR: rw TCR_EL1
3 0 2 0 3 SR: rw TCR2_EL1   3 0 2 1 0 SR: rw APIAKeyLo_EL1
3 0 2 1 1 SR: rw APIAKeyHi_EL1   3 0 2 1 2 SR: rw APIBKeyLo_EL1
3 0 2 1 3 SR: rw APIBKeyHi_EL1   3 0 2 2 0 SR: rw APDAKeyLo_EL1
3 0 2 2 1 SR: rw APDAKeyHi_EL1   3 0 2 2 2 SR: rw APDBKeyLo_EL1
3 0 2 2 3 SR: rw APDBKeyHi_EL1   3 0 2 3 0 SR: rw APGAKeyLo_EL1
3 0 2 3 1 SR: rw APGAKeyHi_EL1   3 0 2 7 3 SR: rw TCR2MASK_EL1
3 0 2 7 7 SR: rw TCR2ALIAS_EL1   3 0 4 0 0 SR: rw SPSR_EL1
3 0 4 0 1 SR: rw ELR_EL1   3 0 4 1 0 SR: rw SP_EL0   3 0 4 2 0 SR: rw SPSel
3 0 4 2 2 SR: r- CurrentEL   3 0 4 2 3 SR: rw PAN   3 0 4 2 4 SR: rw UAO
3 0 4 3 0 SR: rw ALLINT   3 0 4 3 1 SR: rw PM   3 0 4 6 0 SR: rw ICC_PMR_EL1
3 0 5 1 0 SR: rw AFSR0_EL1   3 0 5 1 1 SR: rw AFSR1_EL1
3 0 5 2 0 SR: rw ESR_EL1   3 0 5 6 0 SR: rw TFSR_EL1
3 0 5 6 1 SR: rw TFSRE0_EL1   3 0 6 0 0 SR: rw FAR_EL1
3 0 7 4 0 SR: rw PAR_EL1   3 0 9 14 1 SR: rw PMINTENSET_EL1
3 0 9 14 2 SR: rw PMINTENCLR_EL1   3 0 9 14 6 SR: r- PMMIR_EL1
3 0 10 2 0 SR: rw MAIR_EL1   3 0 10 2 1 SR: rw MAIR2_EL1
3 0 10 3 0 SR: rw AMAIR_EL1   3 0 10 3 1 SR: rw AMAIR2_EL1
3 0 10 4 0 SR: rw LORSA_EL1   3 0 10 4 1 SR: rw LOREA_EL1
3 0 10 4 2 SR: rw LORN_EL1   3 0 10 4 3 SR: rw LORC_EL1
3 0 10 4 7 SR: r- LORID_EL1   3 0 12 0 0 SR: rw VBAR_EL1
3 0 12 0 1 SR: r- RVBAR_EL1   3 0 12 0 2 SR: rw RMR_EL1
3 0 12 1 0 SR: r- ISR_EL1   3 0 12 8 0 SR: r- ICC_IAR0_EL1
3 0 12 8 1 SR: -w ICC_EOIR0_EL1   3 0 12 8 2 SR: r- ICC_HPPIR0_EL1
3 0 12 8 3 SR: rw ICC_BPR0_EL1   3 0 12 8 4 SR: rw ICC_AP0R0_EL1
3 0 12 8 5 SR: rw ICC_AP0R1_EL1   3 0 12 8 6 SR: rw ICC_AP0R2_EL1
3 0 12 8 7 SR: rw ICC_AP0R3_EL1   3 0 12 9 0 SR: rw ICC_AP1R0_EL1
3 0 12 9 1 SR: rw ICC_AP1R1_EL1   3 0 12 9 2 SR: rw ICC_AP1R2_EL1
3 0 12 9 3 SR: rw ICC_AP1R3_EL1   3 0 12 9 5 SR: r- ICC_NMIAR1_EL1
3 0 12 11 1 SR: -w ICC_DIR_EL1   3 0 12 11 3 SR: r- ICC_RPR_EL1
3 0 12 11 5 SR: -w ICC_SGI1R_EL1   3 0 12 11 6 SR: -w ICC_ASGI1R_EL1
3 0 12 11 7 SR: -w ICC_SGI0R_EL1   3 0 12 12 0 SR: r- ICC_IAR1_EL1
3 0 12 12 1 SR: -w ICC_EOIR1_EL1   3 0 12 12 2 SR: r- ICC_HPPIR1_EL1
3 0 12 12 3 SR: rw ICC_BPR1_EL1   3 0 12 12 4 SR: rw ICC_CTLR_EL1
3 0 12 12 5 SR: rw ICC_SRE_EL1   3 0 12 12 6 SR: rw ICC_IGRPEN0_EL1
3 0 12 12 7 SR: rw ICC_IGRPEN1_EL1   3 0 13 0 1 SR: rw CONTEXTIDR_EL1
3 0 13 0 4 SR: rw TPIDR_EL1   3 0 13 0 7 SR: rw SCXTNUM_EL1
3 0 14 1 0 SR: rw CNTKCTL_EL1   3 1 0 0 0 SR: r- CCSIDR_EL1
3 1 0 0 1 SR: r- CLIDR_EL1   3 1 0 0 2 SR: r- CCSIDR2_EL1
3 1 0 0 4 SR: r- GMID_EL1   3 1 0 0 7 SR: r- AIDR_EL1
3 2 0 0 0 SR: rw CSSELR_EL1   3 3 0 0 1 SR: r- CTR_EL0
3 3 0 0 7 SR: r- DCZID_EL0   3 3 2 4 0 SR: r- RNDR   3 3 2 4 1 SR: r- RNDRRS
3 3 4 2 0 SR: rw NZCV   3 3 4 2 1 SR: rw DAIF   3 3 4 2 5 SR: rw DIT
3 3 4 2 6 SR: rw SSBS   3 3 4 2 7 SR: rw TCO   3 3 4 4 0 SR: rw FPCR
3 3 4 4 1 SR: rw FPSR   3 3 4 5 0 SR: rw DSPSR_EL0   3 3 4 5 1 SR: rw DLR_EL0
3 3 9 12 0 SR: rw PMCR_EL0   3 3 9 12 1 SR: rw PMCNTENSET_EL0
3 3 9 12 2 SR: rw PMCNTENCLR_EL0   3 3 9 12 3 SR: rw PMOVSCLR_EL0
3 3 9 12 4 SR: -w PMSWINC_EL0   3 3 9 12 5 SR: rw PMSELR_EL0
3 3 9 12 6 SR: r- PMCEID0_EL0   3 3 9 12 7 SR: r- PMCEID1_EL0
3 3 9 13 0 SR: rw PMCCNTR_EL0   3 3 9 13 1 SR: rw PMXEVTYPER_EL0
3 3 9 13 2 SR: rw PMXEVCNTR_EL0   3 3 9 14 0 SR: rw PMUSERENR_EL0
3 3 9 14 3 SR: rw PMOVSSET_EL0   3 3 13 0 2 SR: rw TPIDR_EL0
3 3 13 0 3 SR: rw TPIDRRO_EL0   3 3 13 0 5 SR: rw TPIDR2_EL0
3 3 13 0 7 SR: rw SCXTNUM_EL0   3 3 14 0 0 SR: rw CNTFRQ_EL0
3 3 14 0 1 SR: r- CNTPCT_EL0   3 3 14 0 2 SR: r- CNTVCT_EL0
3 3 14 2 0 SR: rw CNTP_TVAL_EL0   3 3 14 2 1 SR: rw CNTP_CTL_EL0
3 3 14 2 2 SR: rw CNTP_CVAL_EL0   3 3 14 3 0 SR: rw CNTV_TVAL_EL0
3 3 14 3 1 SR: rw CNTV_CTL_EL0   3 3 14 3 2 SR: rw CNTV_CVAL_EL0
3 3 14 8 0 SR: rw PMEVCNTR0_EL0   3 3 14 8 1 SR: rw PMEVCNTR1_EL0
3 3 14 8 2 SR: rw PMEVCNTR2_EL0   3 3 14 8 3 SR: rw PMEVCNTR3_EL0
3 3 14 8 4 SR: rw PMEVCNTR4_EL0   3 3 14 8 5 SR: rw PMEVCNTR5_EL0
3 3 14 8 6 SR: rw PMEVCNTR6_EL0   3 3 14 8 7 SR: rw PMEVCNTR7_EL0
3 3 14 9 0 SR: rw PMEVCNTR8_EL0   3 3 14 9 1 SR: rw PMEVCNTR9_EL0
3 3 14 9 2 SR: rw PMEVCNTR10_EL0   3 3 14 9 3 SR: rw PMEVCNTR11_EL0
3 3 14 9 4 SR: rw PMEVCNTR12_EL0   3 3 14 9 5 SR: rw PMEVCNTR13_EL0
3 3 14 9 6 SR: rw PMEVCNTR14_EL0   3 3 14 9 7 SR: rw PMEVCNTR15_EL0
3 3 14 10 0 SR: rw PMEVCNTR16_EL0   3 3 14 10 1 SR: rw PMEVCNTR17_EL0
3 3 14 10 2 SR: rw PMEVCNTR18_EL0   3 3 14 10 3 SR: rw PMEVCNTR19_EL0
3 3 14 10 4 SR: rw PMEVCNTR20_EL0   3 3 14 10 5 SR: rw PMEVCNTR21_EL0
3 3 14 10 6 SR: rw PMEVCNTR22_EL0   3 3 14 10 7 SR: rw PMEVCNTR23_EL0
3 3 14 11 0 SR: rw PMEVCNTR24_EL0   3 3 14 11 1 SR: rw PMEVCNTR25_EL0
3 3 14 11 2 SR: rw PMEVCNTR26_EL0   3 3 14 11 3 SR: rw PMEVCNTR27_EL0
3 3 14 11 4 SR: rw PMEVCNTR28_EL0   3 3 14 11 5 SR: rw PMEVCNTR29_EL0
3 3 14 11 6 SR: rw PMEVCNTR30_EL0   3 3 14 12 0 SR: rw PMEVTYPER0_EL0
3 3 14 12 1 SR: rw PMEVTYPER1_EL0   3 3 14 12 2 SR: rw PMEVTYPER2_EL0
3 3 14 12 3 SR: rw PMEVTYPER3_EL0   3 3 14 12 4 SR: rw PMEVTYPER4_EL0
3 3 14 12 5 SR: rw PMEVTYPER5_EL0   3 3 14 12 6 SR: rw PMEVTYPER6_EL0
3 3 14 12 7 SR: rw PMEVTYPER7_EL0   3 3 14 13 0 SR: rw PMEVTYPER8_EL0
3 3 14 13 1 SR: rw PMEVTYPER9_EL0   3 3 14 13 2 SR: rw PMEVTYPER10_EL0
3 3 14 13 3 SR: rw PMEVTYPER11_EL0   3 3 14 13 4 SR: rw PMEVTYPER12_EL0
3 3 14 13 5 SR: rw PMEVTYPER13_EL0   3 3 14 13 6 SR: rw PMEVTYPER14_EL0
3 3 14 13 7 SR: rw PMEVTYPER15_EL0   3 3 14 14 0 SR: rw PMEVTYPER16_EL0
3 3 14 14 1 SR: rw PMEVTYPER17_EL0   3 3 14 14 2 SR: rw PMEVTYPER18_EL0
3 3 14 14 3 SR: rw PMEVTYPER19_EL0   3 3 14 14 4 SR: rw PMEVTYPER20_EL0
3 3 14 14 5 SR: rw PMEVTYPER21_EL0   3 3 14 14 6 SR: rw PMEVTYPER22_EL0
3 3 14 14 7 SR: rw PMEVTYPER23_EL0   3 3 14 15 0 SR: rw PMEVTYPER24_EL0
3 3 14 15 1 SR: rw PMEVTYPER25_EL0   3 3 14 15 2 SR: rw PMEVTYPER26_EL0
3 3 14 15 3 SR: rw PMEVTYPER27_EL0   3 3 14 15 4 SR: rw PMEVTYPER28_EL0
3 3 14 15 5 SR: rw PMEVTYPER29_EL0   3 3 14 15 6 SR: rw PMEVTYPER30_EL0
3 3 14 15 7 SR: rw PMCCFILTR_EL0   3 4 0 0 0 SR: rw VPIDR_EL2
3 4 0 0 5 SR: rw VMPIDR_EL2   3 4 1 0 0 SR: rw SCTLR_EL2
3 4 1 0 1 SR: rw ACTLR_EL2   3 4 1 0 3 SR: rw SCTLR2_EL2
3 4 1 1 0 SR: rw HCR_EL2   3 4 1 1 1 SR: rw MDCR_EL2
3 4 1 1 2 SR: rw CPTR_EL2   3 4 1 1 3 SR: rw HSTR_EL2
3 4 1 1 7 SR: rw HACR_EL2   3 4 1 3 1 SR: rw SDER32_EL2
3 4 1 4 3 SR: rw SCTLR2MASK_EL2   3 4 2 0 0 SR: rw TTBR0_EL2
3 4 2 0 1 SR: rw TTBR1_EL2   3 4 2 0 2 SR: rw TCR_EL2
3 4 2 0 3 SR: rw TCR2_EL2   3 4 2 1 0 SR: rw VTTBR_EL2
3 4 2 1 2 SR: rw VTCR_EL2   3 4 2 7 3 SR: rw TCR2MASK_EL2
3 4 3 0 0 SR: rw DACR32_EL2   3 4 4 0 0 SR: rw SPSR_EL2
3 4 4 0 1 SR: rw ELR_EL2   3 4 4 1 0 SR: rw SP_EL1   3 4 4 3 0 SR: rw SPSR_irq
3 4 4 3 1 SR: rw SPSR_abt   3 4 4 3 2 SR: rw SPSR_und
3 4 4 3 3 SR: rw SPSR_fiq   3 4 5 0 1 SR: rw IFSR32_EL2
3 4 5 1 0 SR: rw AFSR0_EL2   3 4 5 1 1 SR: rw AFSR1_EL2
3 4 5 2 0 SR: rw ESR_EL2   3 4 5 3 0 SR: rw FPEXC32_EL2
3 4 5 6 0 SR: rw TFSR_EL2   3 4 6 0 0 SR: rw FAR_EL2
3 4 6 0 4 SR: rw HPFAR_EL2   3 4 10 1 1 SR: rw MAIR2_EL2
3 4 10 2 0 SR: rw MAIR_EL2   3 4 10 3 0 SR: rw AMAIR_EL2
3 4 10 3 1 SR: rw AMAIR2_EL2   3 4 12 0 0 SR: rw VBAR_EL2
3 4 12 0 1 SR: r- RVBAR_EL2   3 4 12 0 2 SR: rw RMR_EL2
3 4 12 8 0 SR: rw ICH_AP0R0_EL2   3 4 12 8 1 SR: rw ICH_AP0R1_EL2
3 4 12 8 2 SR: rw ICH_AP0R2_EL2   3 4 12 8 3 SR: rw ICH_AP0R3_EL2
3 4 12 9 0 SR: rw ICH_AP1R0_EL2   3 4 12 9 1 SR: rw ICH_AP1R1_EL2
3 4 12 9 2 SR: rw ICH_AP1R2_EL2   3 4 12 9 3 SR: rw ICH_AP1R3_EL2
3 4 12 9 5 SR: rw ICC_SRE_EL2   3 4 12 11 0 SR: rw ICH_HCR_EL2
3 4 12 11 1 SR: r- ICH_VTR_EL2   3 4 12 11 2 SR: r- ICH_MISR_EL2
3 4 12 11 3 SR: r- ICH_EISR_EL2   3 4 12 11 5 SR: r- ICH_ELRSR_EL2
3 4 12 11 7 SR: rw ICH_VMCR_EL2   3 4 12 12 0 SR: rw ICH_LR0_EL2
3 4 12 12 1 SR: rw ICH_LR1_EL2   3 4 12 12 2 SR: rw ICH_LR2_EL2
3 4 12 12 3 SR: rw ICH_LR3_EL2   3 4 12 12 4 SR: rw ICH_LR4_EL2
3 4 12 12 5 SR: rw ICH_LR5_EL2   3 4 12 12 6 SR: rw ICH_LR6_EL2
3 4 12 12 7 SR: rw ICH_LR7_EL2   3 4 12 13 0 SR: rw ICH_LR8_EL2
3 4 12 13 1 SR: rw ICH_LR9_EL2   3 4 12 13 2 SR: rw ICH_LR10_EL2
3 4 12 13 3 SR: rw ICH_LR11_EL2   3 4 12 13 4 SR: rw ICH_LR12_EL2
3 4 12 13 5 SR: rw ICH_LR13_EL2   3 4 12 13 6 SR: rw ICH_LR14_EL2
3 4 12 13 7 SR: rw ICH_LR15_EL2   3 4 13 0 1 SR: rw CONTEXTIDR_EL2
3 4 13 0 2 SR: rw TPIDR_EL2   3 4 13 0 7 SR: rw SCXTNUM_EL2
3 4 14 0 3 SR: rw CNTVOFF_EL2   3 4 14 1 0 SR: rw CNTHCTL_EL2
3 4 14 2 0 SR: rw CNTHP_TVAL_EL2   3 4 14 2 1 SR: rw CNTHP_CTL_EL2
3 4 14 2 2 SR: rw CNTHP_CVAL_EL2   3 4 14 3 0 SR: rw CNTHV_TVAL_EL2
3 4 14 3 1 SR: rw CNTHV_CTL_EL2   3 4 14 3 2 SR: rw CNTHV_CVAL_EL2
3 5 1 0 0 SR: rw SCTLR_EL12   3 5 1 0 1 SR: rw ACTLR_EL12
3 5 1 0 2 SR: rw CPACR_EL12   3 5 1 0 3 SR: rw SCTLR2_EL12
3 5 1 4 3 SR: rw SCTLR2MASK_EL12   3 5 2 0 0 SR: rw TTBR0_EL12
3 5 2 0 1 SR: rw TTBR1_EL12   3 5 2 0 2 SR: rw TCR_EL12
3 5 2 0 3 SR: rw TCR2_EL12   3 5 2 7 3 SR: rw TCR2MASK_EL12
3 5 4 0 0 SR: rw SPSR_EL12   3 5 4 0 1 SR: rw ELR_EL12
3 5 5 1 0 SR: rw AFSR0_EL12   3 5 5 1 1 SR: rw AFSR1_EL12
3 5 5 2 0 SR: rw ESR_EL12   3 5 5 6 0 SR: rw TFSR_EL12
3 5 6 0 0 SR: rw FAR_EL12   3 5 10 2 0 SR: rw MAIR_EL12
3 5 10 2 1 SR: rw MAIR2_EL12   3 5 10 3 0 SR: rw AMAIR_EL12
3 5 10 3 1 SR: rw AMAIR2_EL12   3 5 12 0 0 SR: rw VBAR_EL12
3 5 13 0 1 SR: rw CONTEXTIDR_EL12   3 5 13 0 7 SR: rw SCXTNUM_EL12
3 5 14 1 0 SR: rw CNTKCTL_EL12   3 5 14 2 0 SR: rw CNTP_TVAL_EL02
3 5 14 2 1 SR: rw CNTP_CTL_EL02   3 5 14 2 2 SR: rw CNTP_CVAL_EL02
3 5 14 3 0 SR: rw CNTV_TVAL_EL02   3 5 14 3 1 SR: rw CNTV_CTL_EL02
3 5 14 3 2 SR: rw CNTV_CVAL_EL02   3 6 1 0 0 SR: rw SCTLR_EL3
3 6 1 0 1 SR: rw ACTLR_EL3   3 6 1 0 3 SR: rw SCTLR2_EL3
3 6 1 1 0 SR: rw SCR_EL3   3 6 1 1 1 SR: rw SDER32_EL3
3 6 1 1 2 SR: rw CPTR_EL3   3 6 1 3 1 SR: rw MDCR_EL3
3 6 2 0 0 SR: rw TTBR0_EL3   3 6 2 0 2 SR: rw TCR_EL3
3 6 4 0 0 SR: rw SPSR_EL3   3 6 4 0 1 SR: rw ELR_EL3   3 6 4 1 0 SR: rw SP_EL2
3 6 5 1 0 SR: rw AFSR0_EL3   3 6 5 1 1 SR: rw AFSR1_EL3
3 6 5 2 0 SR: rw ESR_EL3   3 6 5 6 0 SR: rw TFSR_EL3
3 6 6 0 0 SR: rw FAR_EL3   3 6 10 1 1 SR: rw MAIR2_EL3
3 6 10 2 0 SR: rw MAIR_EL3   3 6 10 3 0 SR: rw AMAIR_EL3
3 6 10 3 1 SR: rw AMAIR2_EL3   3 6 12 0 0 SR: rw VBAR_EL3
3 6 12 0 1 SR: r- RVBAR_EL3   3 6 12 0 2 SR: rw RMR_EL3
3 6 12 12 4 SR: rw ICC_CTLR_EL3   3 6 12 12 5 SR: rw ICC_SRE_EL3
3 6 12 12 7 SR: rw ICC_IGRPEN1_EL3   3 6 13 0 2 SR: rw TPIDR_EL3
3 6 13 0 7 SR: rw SCXTNUM_EL3   3 7 14 2 0 SR: rw CNTPS_TVAL_EL1
3 7 14 2 1 SR: rw CNTPS_CTL_EL1   3 7 14 2 2 SR: rw CNTPS_CVAL_EL1
0 C, 0 C, 0 C,                       \ the end: access 0
VARIABLE SR-KEY   VARIABLE SR-ACCESS
: SR-NAME ( key access -- a u true | false )   \ the name of a register for MRS (access 1) or MSR (2)
  SR-ACCESS !  SR-KEY !  SYSREGS
  BEGIN  DUP 2 + C@ WHILE
    DUP C@ OVER 1+ C@ 8 LSHIFT OR  SR-KEY @ =
    OVER 2 + C@ SR-ACCESS @ AND 0<>  AND IF  3 + COUNT TRUE EXIT  THEN
    3 + COUNT +
  REPEAT  DROP FALSE ;

\ The operations of SYS with a name: op1 CRn CRm op2 SYS: name operation
\ (with Xt), SYSR: name (with Xt, no operation), SYS0: name operation
\ (without a register). Others show as sys #op1, Cn, Cm, #op2, Xt.
VARIABLE SYS-FLAG
CREATE SYSBUF  40 ALLOT
: SYS-ENTRY, ( op1 crn crm op2 a u -- )
  >R >R  SWAP 3 LSHIFT OR  SWAP 7 LSHIFT OR  SWAP 11 LSHIFT OR
  DUP C,  8 RSHIFT C,  SYS-FLAG @ C,  R> R> STR, ;
: 2NAMES ( "name" "operation" -- a u )   \ "name operation" in SYSBUF
  PARSE-NAME  DUP >R  SYSBUF SWAP MOVE  32 SYSBUF R@ + C!
  PARSE-NAME  SYSBUF R@ 1+ +  SWAP  DUP >R  MOVE
  SYSBUF  R> R> + 1+ ;
: SYS: ( op1 crn crm op2 "name" "operation" -- )   1 SYS-FLAG !  2NAMES SYS-ENTRY, ;
: SYSR: ( op1 crn crm op2 "name" -- )   2 SYS-FLAG !  PARSE-NAME SYS-ENTRY, ;
: SYS0: ( op1 crn crm op2 "name" "operation" -- )   0 SYS-FLAG !  2NAMES SYS-ENTRY, ;
CREATE SYS-OPS
0 7 1 0 SYS0: ic ialluis   0 7 5 0 SYS0: ic iallu   0 7 6 1 SYS: dc ivac
0 7 6 2 SYS: dc isw   0 7 6 3 SYS: dc igvac   0 7 6 4 SYS: dc igsw
0 7 6 5 SYS: dc igdvac   0 7 6 6 SYS: dc igdsw   0 7 8 0 SYS: at s1e1r
0 7 8 1 SYS: at s1e1w   0 7 8 2 SYS: at s1e0r   0 7 8 3 SYS: at s1e0w
0 7 9 0 SYS: at s1e1rp   0 7 9 1 SYS: at s1e1wp   0 7 9 2 SYS: at s1e1a
0 7 10 2 SYS: dc csw   0 7 10 4 SYS: dc cgsw   0 7 10 6 SYS: dc cgdsw
0 7 14 2 SYS: dc cisw   0 7 14 4 SYS: dc cigsw   0 7 14 6 SYS: dc cigdsw
0 7 15 1 SYS: dc civaps   0 7 15 5 SYS: dc cigdvaps
0 8 1 0 SYS0: tlbi vmalle1os   0 8 1 1 SYS: tlbi vae1os
0 8 1 2 SYS: tlbi aside1os   0 8 1 3 SYS: tlbi vaae1os
0 8 1 5 SYS: tlbi vale1os   0 8 1 7 SYS: tlbi vaale1os
0 8 2 1 SYS: tlbi rvae1is   0 8 2 3 SYS: tlbi rvaae1is
0 8 2 5 SYS: tlbi rvale1is   0 8 2 7 SYS: tlbi rvaale1is
0 8 3 0 SYS0: tlbi vmalle1is   0 8 3 1 SYS: tlbi vae1is
0 8 3 2 SYS: tlbi aside1is   0 8 3 3 SYS: tlbi vaae1is
0 8 3 5 SYS: tlbi vale1is   0 8 3 7 SYS: tlbi vaale1is
0 8 5 1 SYS: tlbi rvae1os   0 8 5 3 SYS: tlbi rvaae1os
0 8 5 5 SYS: tlbi rvale1os   0 8 5 7 SYS: tlbi rvaale1os
0 8 6 1 SYS: tlbi rvae1   0 8 6 3 SYS: tlbi rvaae1   0 8 6 5 SYS: tlbi rvale1
0 8 6 7 SYS: tlbi rvaale1   0 8 7 0 SYS0: tlbi vmalle1
0 8 7 1 SYS: tlbi vae1   0 8 7 2 SYS: tlbi aside1   0 8 7 3 SYS: tlbi vaae1
0 8 7 5 SYS: tlbi vale1   0 8 7 7 SYS: tlbi vaale1
0 9 1 0 SYS0: tlbi vmalle1osnxs   0 9 1 1 SYS: tlbi vae1osnxs
0 9 1 2 SYS: tlbi aside1osnxs   0 9 1 3 SYS: tlbi vaae1osnxs
0 9 1 5 SYS: tlbi vale1osnxs   0 9 1 7 SYS: tlbi vaale1osnxs
0 9 2 1 SYS: tlbi rvae1isnxs   0 9 2 3 SYS: tlbi rvaae1isnxs
0 9 2 5 SYS: tlbi rvale1isnxs   0 9 2 7 SYS: tlbi rvaale1isnxs
0 9 3 0 SYS0: tlbi vmalle1isnxs   0 9 3 1 SYS: tlbi vae1isnxs
0 9 3 2 SYS: tlbi aside1isnxs   0 9 3 3 SYS: tlbi vaae1isnxs
0 9 3 5 SYS: tlbi vale1isnxs   0 9 3 7 SYS: tlbi vaale1isnxs
0 9 5 1 SYS: tlbi rvae1osnxs   0 9 5 3 SYS: tlbi rvaae1osnxs
0 9 5 5 SYS: tlbi rvale1osnxs   0 9 5 7 SYS: tlbi rvaale1osnxs
0 9 6 1 SYS: tlbi rvae1nxs   0 9 6 3 SYS: tlbi rvaae1nxs
0 9 6 5 SYS: tlbi rvale1nxs   0 9 6 7 SYS: tlbi rvaale1nxs
0 9 7 0 SYS0: tlbi vmalle1nxs   0 9 7 1 SYS: tlbi vae1nxs
0 9 7 2 SYS: tlbi aside1nxs   0 9 7 3 SYS: tlbi vaae1nxs
0 9 7 5 SYS: tlbi vale1nxs   0 9 7 7 SYS: tlbi vaale1nxs   3 7 2 7 SYSR: trcit
3 7 3 4 SYS: cfp rctx   3 7 3 5 SYS: dvp rctx   3 7 3 6 SYS: cosp rctx
3 7 3 7 SYS: cpp rctx   3 7 4 1 SYS: dc zva   3 7 4 3 SYS: dc gva
3 7 4 4 SYS: dc gzva   3 7 5 1 SYS: ic ivau   3 7 7 0 SYSR: gcspushm
3 7 7 2 SYSR: gcsss1   3 7 10 1 SYS: dc cvac   3 7 10 3 SYS: dc cgvac
3 7 10 5 SYS: dc cgdvac   3 7 11 0 SYS: dc cvaoc   3 7 11 1 SYS: dc cvau
3 7 11 7 SYS: dc cgdvaoc   3 7 12 1 SYS: dc cvap   3 7 12 3 SYS: dc cgvap
3 7 12 5 SYS: dc cgdvap   3 7 13 1 SYS: dc cvadp   3 7 13 3 SYS: dc cgvadp
3 7 13 5 SYS: dc cgdvadp   3 7 14 1 SYS: dc civac   3 7 14 3 SYS: dc cigvac
3 7 14 5 SYS: dc cigdvac   3 7 15 0 SYS: dc civaoc   3 7 15 7 SYS: dc cigdvaoc
4 7 8 0 SYS: at s1e2r   4 7 8 1 SYS: at s1e2w   4 7 8 4 SYS: at s12e1r
4 7 8 5 SYS: at s12e1w   4 7 8 6 SYS: at s12e0r   4 7 8 7 SYS: at s12e0w
4 7 9 2 SYS: at s1e2a   4 7 14 0 SYS: dc cipae   4 7 14 7 SYS: dc cigdpae
4 8 0 1 SYS: tlbi ipas2e1is   4 8 0 2 SYS: tlbi ripas2e1is
4 8 0 5 SYS: tlbi ipas2le1is   4 8 0 6 SYS: tlbi ripas2le1is
4 8 1 0 SYS0: tlbi alle2os   4 8 1 1 SYS: tlbi vae2os
4 8 1 4 SYS0: tlbi alle1os   4 8 1 5 SYS: tlbi vale2os
4 8 1 6 SYS0: tlbi vmalls12e1os   4 8 2 1 SYS: tlbi rvae2is
4 8 2 2 SYS0: tlbi vmallws2e1is   4 8 2 5 SYS: tlbi rvale2is
4 8 3 0 SYS0: tlbi alle2is   4 8 3 1 SYS: tlbi vae2is
4 8 3 4 SYS0: tlbi alle1is   4 8 3 5 SYS: tlbi vale2is
4 8 3 6 SYS0: tlbi vmalls12e1is   4 8 4 0 SYS: tlbi ipas2e1os
4 8 4 1 SYS: tlbi ipas2e1   4 8 4 2 SYS: tlbi ripas2e1
4 8 4 3 SYS: tlbi ripas2e1os   4 8 4 4 SYS: tlbi ipas2le1os
4 8 4 5 SYS: tlbi ipas2le1   4 8 4 6 SYS: tlbi ripas2le1
4 8 4 7 SYS: tlbi ripas2le1os   4 8 5 1 SYS: tlbi rvae2os
4 8 5 2 SYS0: tlbi vmallws2e1os   4 8 5 5 SYS: tlbi rvale2os
4 8 6 1 SYS: tlbi rvae2   4 8 6 2 SYS0: tlbi vmallws2e1
4 8 6 5 SYS: tlbi rvale2   4 8 7 0 SYS0: tlbi alle2   4 8 7 1 SYS: tlbi vae2
4 8 7 4 SYS0: tlbi alle1   4 8 7 5 SYS: tlbi vale2
4 8 7 6 SYS0: tlbi vmalls12e1   4 9 0 1 SYS: tlbi ipas2e1isnxs
4 9 0 2 SYS: tlbi ripas2e1isnxs   4 9 0 5 SYS: tlbi ipas2le1isnxs
4 9 0 6 SYS: tlbi ripas2le1isnxs   4 9 1 0 SYS0: tlbi alle2osnxs
4 9 1 1 SYS: tlbi vae2osnxs   4 9 1 4 SYS0: tlbi alle1osnxs
4 9 1 5 SYS: tlbi vale2osnxs   4 9 1 6 SYS0: tlbi vmalls12e1osnxs
4 9 2 1 SYS: tlbi rvae2isnxs   4 9 2 2 SYS0: tlbi vmallws2e1isnxs
4 9 2 5 SYS: tlbi rvale2isnxs   4 9 3 0 SYS0: tlbi alle2isnxs
4 9 3 1 SYS: tlbi vae2isnxs   4 9 3 4 SYS0: tlbi alle1isnxs
4 9 3 5 SYS: tlbi vale2isnxs   4 9 3 6 SYS0: tlbi vmalls12e1isnxs
4 9 4 0 SYS: tlbi ipas2e1osnxs   4 9 4 1 SYS: tlbi ipas2e1nxs
4 9 4 2 SYS: tlbi ripas2e1nxs   4 9 4 3 SYS: tlbi ripas2e1osnxs
4 9 4 4 SYS: tlbi ipas2le1osnxs   4 9 4 5 SYS: tlbi ipas2le1nxs
4 9 4 6 SYS: tlbi ripas2le1nxs   4 9 4 7 SYS: tlbi ripas2le1osnxs
4 9 5 1 SYS: tlbi rvae2osnxs   4 9 5 2 SYS0: tlbi vmallws2e1osnxs
4 9 5 5 SYS: tlbi rvale2osnxs   4 9 6 1 SYS: tlbi rvae2nxs
4 9 6 2 SYS0: tlbi vmallws2e1nxs   4 9 6 5 SYS: tlbi rvale2nxs
4 9 7 0 SYS0: tlbi alle2nxs   4 9 7 1 SYS: tlbi vae2nxs
4 9 7 4 SYS0: tlbi alle1nxs   4 9 7 5 SYS: tlbi vale2nxs
4 9 7 6 SYS0: tlbi vmalls12e1nxs   6 7 0 0 SYSR: apas   6 7 8 0 SYS: at s1e3r
6 7 8 1 SYS: at s1e3w   6 7 9 2 SYS: at s1e3a   6 7 14 1 SYS: dc cipapa
6 7 14 5 SYS: dc cigdpapa   6 8 1 0 SYS0: tlbi alle3os
6 8 1 1 SYS: tlbi vae3os   6 8 1 4 SYS0: tlbi paallos
6 8 1 5 SYS: tlbi vale3os   6 8 2 1 SYS: tlbi rvae3is
6 8 2 5 SYS: tlbi rvale3is   6 8 3 0 SYS0: tlbi alle3is
6 8 3 1 SYS: tlbi vae3is   6 8 3 5 SYS: tlbi vale3is   6 8 4 3 SYS: tlbi rpaos
6 8 4 7 SYS: tlbi rpalos   6 8 5 1 SYS: tlbi rvae3os
6 8 5 5 SYS: tlbi rvale3os   6 8 6 1 SYS: tlbi rvae3
6 8 6 5 SYS: tlbi rvale3   6 8 7 0 SYS0: tlbi alle3   6 8 7 1 SYS: tlbi vae3
6 8 7 4 SYS0: tlbi paall   6 8 7 5 SYS: tlbi vale3
6 9 1 0 SYS0: tlbi alle3osnxs   6 9 1 1 SYS: tlbi vae3osnxs
6 9 1 4 SYS0: tlbi paallosnxs   6 9 1 5 SYS: tlbi vale3osnxs
6 9 2 1 SYS: tlbi rvae3isnxs   6 9 2 5 SYS: tlbi rvale3isnxs
6 9 3 0 SYS0: tlbi alle3isnxs   6 9 3 1 SYS: tlbi vae3isnxs
6 9 3 5 SYS: tlbi vale3isnxs   6 9 4 3 SYS: tlbi rpaosnxs
6 9 4 7 SYS: tlbi rpalosnxs   6 9 5 1 SYS: tlbi rvae3osnxs
6 9 5 5 SYS: tlbi rvale3osnxs   6 9 6 1 SYS: tlbi rvae3nxs
6 9 6 5 SYS: tlbi rvale3nxs   6 9 7 0 SYS0: tlbi alle3nxs
6 9 7 1 SYS: tlbi vae3nxs   6 9 7 4 SYS0: tlbi paallnxs
6 9 7 5 SYS: tlbi vale3nxs
0 C, 0 C, 255 C,                     \ the end
VARIABLE SYS-KEY
: SYS-NAME ( key -- a u flag true | false )   \ flag: 0 no Xt, 1 ", Xt", 2 " Xt"
  SYS-KEY !  SYS-OPS
  BEGIN  DUP 2 + C@ 255 <> WHILE
    DUP C@ OVER 1+ C@ 8 LSHIFT OR  SYS-KEY @ = IF
      DUP 3 + COUNT  ROT 2 + C@  TRUE EXIT
    THEN  3 + COUNT +
  REPEAT  DROP FALSE ;

\ HINT 0 to 40 (CRm:op2); "": hint #n
CREATE HINTS
  S" nop" STR,  S" yield" STR,  S" wfe" STR,  S" wfi" STR,  S" sev" STR,  S" sevl" STR,
  S" dgh" STR,  S" xpaclri" STR,  S" pacia1716" STR,  S" " STR,  S" pacib1716" STR,  S" " STR,
  S" autia1716" STR,  S" " STR,  S" autib1716" STR,  S" " STR,  S" esb" STR,  S" psb csync" STR,
  S" tsb csync" STR,  S" gcsb dsync" STR,  S" csdb" STR,  S" " STR,  S" clrbhb" STR,  S" " STR,
  S" paciaz" STR,  S" paciasp" STR,  S" pacibz" STR,  S" pacibsp" STR,  S" autiaz" STR,
  S" autiasp" STR,  S" autibz" STR,  S" autibsp" STR,  S" bti" STR,  S" " STR,  S" bti c" STR,
  S" " STR,  S" bti j" STR,  S" " STR,  S" bti jc" STR,  S" pacm" STR,  S" chkfeat x16" STR,
\ MSR (immediate): op1:op2 and the field of PSTATE
CREATE PSTATES
  3 C, S" UAO" STR,  4 C, S" PAN" STR,  5 C, S" SPSel" STR,  25 C, S" SSBS" STR,
  26 C, S" DIT" STR,  28 C, S" TCO" STR,  30 C, S" DAIFSet" STR,  31 C, S" DAIFClr" STR,
  255 C,
CREATE CONDS
  S" eq" STR,  S" ne" STR,  S" hs" STR,  S" lo" STR,  S" mi" STR,  S" pl" STR,  S" vs" STR,  S" vc" STR,
  S" hi" STR,  S" ls" STR,  S" ge" STR,  S" lt" STR,  S" gt" STR,  S" le" STR,  S" al" STR,  S" nv" STR,
CREATE SHIFTS   S" lsl" STR,  S" lsr" STR,  S" asr" STR,  S" ror" STR,
CREATE EXTENDS
  S" uxtb" STR,  S" uxth" STR,  S" uxtw" STR,  S" uxtx" STR,  S" sxtb" STR,  S" sxth" STR,  S" sxtw" STR,  S" sxtx" STR,
CREATE BARRIERS   \ DMB, DSB by CRm; "": #n
  S" " STR,  S" oshld" STR,  S" oshst" STR,  S" osh" STR,  S" " STR,  S" nshld" STR,  S" nshst" STR,  S" nsh" STR,
  S" " STR,  S" ishld" STR,  S" ishst" STR,  S" ish" STR,  S" " STR,  S" ld" STR,  S" st" STR,  S" sy" STR,
CREATE NXS   S" osh" STR,  S" nsh" STR,  S" ish" STR,  S" sy" STR,
CREATE PRF-TYPES     S" pld" STR,  S" pli" STR,  S" pst" STR,
CREATE PRF-TARGETS   S" l1" STR,  S" l2" STR,  S" l3" STR,  S" slc" STR,
ALIGN

\ ==== The text of the operands ============================================

: NTH ( n a -- a' u )   \ the n-th counted string from a
  SWAP 0 ?DO  COUNT +  LOOP  COUNT ;
: +NTH ( n a -- )   NTH +T ;
: +DEC ( n -- )   \ in decimal, with "-"
  DUP 0< IF  S" -" +T  NEGATE  THEN
  BASE @ >R  DECIMAL  0 <# #S #> +T  R> BASE ! ;
: +# ( -- )   S" #" +T ;
: +IMM ( n -- )    +#  S>D +SHEX ;   \ # and hex digits, with "-"
: +UIMM ( u -- )   +#  0 +HEX ;
: +#DEC ( n -- )   +#  +DEC ;
: +R ( n size -- )   \ register n of 32 or 64 bits; 31 is the zero register
  OVER 31 = IF  NIP 64 = IF S" xzr" ELSE S" wzr" THEN +T  EXIT  THEN
  64 = IF 'x' ELSE 'w' THEN +C  +DEC ;
: +RSP ( n size -- )   \ the same; 31 is the stack pointer
  OVER 31 = IF  NIP 64 = IF S" sp" ELSE S" wsp" THEN +T  EXIT  THEN  +R ;
: +X ( n -- )     64 +R ;
: +XSP ( n -- )   64 +RSP ;
: +W ( n -- )     32 +R ;
: +V ( n c -- )   +C +DEC ;          \ b0, h0, s0, d0, q0
: +BIT30 ( n -- )   30 BIT? IF +X ELSE +W THEN ;
: +SIZED ( n -- )   30 2 FIELD 3 = IF +X ELSE +W THEN ;
: EVEN ( n -- n )   DUP 1 AND IF  BAD!  THEN ;
: +TARGET ( offset -- )   \ PC + offset, a number of 64 bits
  S>D  PC @ 0 D+  +HEX ;
: +PAGE ( -- )   \ ADRP: the page of PC plus imm pages of 4 KB
  5 19 FIELD 2 LSHIFT  29 2 FIELD OR  21 SIGNED
  DUP 12 LSHIFT  SWAP 0<   PC @ $FFFFF000 AND 0 D+  +HEX ;

: +SHIFTED ( ror? -- )   \ Rm and its shift; ", lsl #0" not
  RM# REGSIZE +R
  22 2 FIELD  DUP 3 =  ROT 0= AND IF  BAD!  THEN
  10 6 FIELD  DUP 31 >  SF? 0= AND IF  BAD!  THEN
  OVER 0=  OVER 0= AND IF  2DROP EXIT  THEN
  S" , " +T  SWAP SHIFTS +NTH  S"  #" +T  +DEC ;
: +EXTENDED ( sp? -- )   \ Rm and its extension; with sp, UXTW or UXTX as lsl
  RM#  SF? 13 2 FIELD 3 = AND IF +X ELSE +W THEN
  13 3 FIELD  10 3 FIELD  DUP 4 > IF  BAD!  THEN
  ROT IF  OVER  SF? IF 3 ELSE 2 THEN  = IF
    NIP ?DUP IF  S" , lsl #" +T  +DEC  THEN  EXIT
  THEN THEN
  S" , " +T  SWAP EXTENDS +NTH  ?DUP IF  S"  #" +T  +DEC  THEN ;

: +REGISTER ( k -- )
  CASE
    Rd      OF  RD# REGSIZE +R  ENDOF
    Rn      OF  RN# REGSIZE +R  ENDOF
    Rm      OF  RM# REGSIZE +R  ENDOF
    Ra      OF  RA# REGSIZE +R  ENDOF
    Rd|SP   OF  RD# REGSIZE +RSP  ENDOF
    Rn|SP   OF  RN# REGSIZE +RSP  ENDOF
    Rt      OF  RD# REGSIZE +R  ENDOF
    Rt.b5   OF  RD# 31 BIT? IF +X ELSE +W THEN  ENDOF
    Wd      OF  RD# +W  ENDOF
    Wn      OF  RN# +W  ENDOF
    Wm      OF  RM# +W  ENDOF
    Wt      OF  RD# +W  ENDOF
    Wt2     OF  RA# +W  ENDOF
    Ws      OF  RM# +W  ENDOF
    Xd      OF  RD# +X  ENDOF
    Xn      OF  RN# +X  ENDOF
    Xm      OF  RM# +X  ENDOF
    Xa      OF  RA# +X  ENDOF
    Xt      OF  RD# +X  ENDOF
    Xt2     OF  RA# +X  ENDOF
    Xs      OF  RM# +X  ENDOF
    Xd|SP   OF  RD# +XSP  ENDOF
    Xn|SP   OF  RN# +XSP  ENDOF
    Xm|SP   OF  RM# +XSP  ENDOF
    Xt|SP   OF  RD# +XSP  ENDOF
    Rt.sz   OF  RD# +SIZED  ENDOF
    Rs.sz   OF  RM# +SIZED  ENDOF
    Rt.30   OF  RD# +BIT30  ENDOF
    Rt2.30  OF  RA# +BIT30  ENDOF
    Rs.30   OF  RM# EVEN +BIT30  ENDOF
    Rs+1.30 OF  RM# 1+ +BIT30  ENDOF
    Rt+1.30 OF  RD# EVEN 1+ +BIT30  ENDOF
    Bt      OF  RD# 'b' +V  ENDOF
    Ht      OF  RD# 'h' +V  ENDOF
    St      OF  RD# 's' +V  ENDOF
    Dt      OF  RD# 'd' +V  ENDOF
    Qt      OF  RD# 'q' +V  ENDOF
    St2     OF  RA# 's' +V  ENDOF
    Dt2     OF  RA# 'd' +V  ENDOF
    Qt2     OF  RA# 'q' +V  ENDOF
    Rm.shift  OF  FALSE +SHIFTED  ENDOF
    Rm.lshift OF  TRUE +SHIFTED  ENDOF
    Rm.ext    OF  SP? +EXTENDED  ENDOF
    Rm.exts   OF  RN# 31 = +EXTENDED  ENDOF
    Xt.64b    OF  RD#  DUP 1 AND  OVER 23 > OR IF  BAD!  THEN  +X  ENDOF
  ENDCASE ;

: +[XN ( -- )   S" [" +T  RN# +XSP ;
: +[OFF] ( n -- )   +[XN  ?DUP IF  S" , " +T  +IMM  THEN  S" ]" +T ;   \ [x1, #0x8], [x1]
: +[OFF]! ( n -- )   +[XN  S" , " +T  +IMM  S" ]!" +T ;                 \ [x1, #0x8]!
: +[]OFF ( n -- )   +[XN  S" ], " +T  +IMM ;                            \ [x1], #0x8
: S9 ( -- n )   12 9 FIELD 9 SIGNED ;
: S7 ( -- n )   15 7 FIELD 7 SIGNED ;
: U12 ( -- u )   10 12 FIELD ;
: S10 ( -- n )   12 9 FIELD  22 BIT? IF 512 OR THEN  10 SIGNED ;
: +[XN,RM] ( shift -- )   \ [x1, x2, lsl #3]: the shift when S is 1
  +[XN  S" , " +T
  13 3 FIELD  DUP 2 AND 0= IF  BAD!  THEN
  RM# OVER 1 AND IF +X ELSE +W THEN
  DUP 3 =  12 BIT? 0= AND IF  2DROP  S" ]" +T  EXIT  THEN
  S" , " +T  DUP 3 = IF  DROP S" lsl" +T  ELSE  EXTENDS +NTH  THEN
  12 BIT? IF  S"  #" +T  +DEC  ELSE  DROP  THEN  S" ]" +T ;

: +MEMORY ( k -- )
  CASE
    [Xn]          OF  +[XN  S" ]" +T  ENDOF
    [Xn+s9]      OF  S9 +[OFF]  ENDOF
    [Xn]+s9      OF  S9 +[]OFF  ENDOF
    [Xn+s9]!     OF  S9 +[OFF]!  ENDOF
    [Xn+u12]     OF  U12 +[OFF]  ENDOF
    [Xn+u12*2]   OF  U12 2* +[OFF]  ENDOF
    [Xn+u12*4]   OF  U12 4 * +[OFF]  ENDOF
    [Xn+u12*8]   OF  U12 8 * +[OFF]  ENDOF
    [Xn+u12*16]  OF  U12 16 * +[OFF]  ENDOF
    [Xn+s7*4]    OF  S7 4 * +[OFF]  ENDOF
    [Xn+s7*8]    OF  S7 8 * +[OFF]  ENDOF
    [Xn+s7*16]   OF  S7 16 * +[OFF]  ENDOF
    [Xn]+s7*4    OF  S7 4 * +[]OFF  ENDOF
    [Xn]+s7*8    OF  S7 8 * +[]OFF  ENDOF
    [Xn]+s7*16   OF  S7 16 * +[]OFF  ENDOF
    [Xn+s7*4]!   OF  S7 4 * +[OFF]!  ENDOF
    [Xn+s7*8]!   OF  S7 8 * +[OFF]!  ENDOF
    [Xn+s7*16]!  OF  S7 16 * +[OFF]!  ENDOF
    [Xn+Rm]       OF  0 +[XN,RM]  ENDOF
    [Xn+Rm*2]     OF  1 +[XN,RM]  ENDOF
    [Xn+Rm*4]     OF  2 +[XN,RM]  ENDOF
    [Xn+Rm*8]     OF  3 +[XN,RM]  ENDOF
    [Xn+Rm*16]    OF  4 +[XN,RM]  ENDOF
    [Xn+s10*8]   OF  S10 8 * +[OFF]  ENDOF
    [Xn+s10*8]!  OF  S10 8 * +[OFF]!  ENDOF
    [Xn+s9*16]   OF  S9 16 * +[OFF]  ENDOF
    [Xn]+s9*16   OF  S9 16 * +[]OFF  ENDOF
    [Xn+s9*16]!  OF  S9 16 * +[OFF]!  ENDOF
  ENDCASE ;

: +PRFOP ( -- )   \ pldl1keep ...; a number for the others
  0 5 FIELD  DUP 24 < IF
    DUP 3 RSHIFT PRF-TYPES +NTH  DUP 1 RSHIFT 3 AND PRF-TARGETS +NTH
    1 AND IF S" strm" ELSE S" keep" THEN +T  EXIT
  THEN  +UIMM ;
: +BARRIER ( -- )   8 4 FIELD  DUP BARRIERS NTH  ?DUP IF  +T DROP  ELSE  DROP +#DEC  THEN ;
: +GENERIC ( -- )   \ a system register by its numbers: S3_0_C15_C2_0
  S" S" +T  19 2 FIELD +DEC  S" _" +T  16 3 FIELD +DEC  S" _C" +T  12 4 FIELD +DEC
  S" _C" +T  8 4 FIELD +DEC  S" _" +T  5 3 FIELD +DEC ;
: +SYSREG ( -- )
  5 16 FIELD  21 BIT? IF 1 ELSE 2 THEN  SR-NAME IF  +T EXIT  THEN  +GENERIC ;

: +OTHER ( k -- )
  CASE
    #aimm     OF  10 12 FIELD +UIMM  22 BIT? IF  S" , lsl #12" +T  THEN  ENDOF
    #bimm     OF  +#  BITMASK  SF? 0= IF  DROP 0  THEN  +HEX  ENDOF
    #bimm.s   OF  +#  BITMASK  SF? 0= IF  DROP S>D  THEN  +SHEX  ENDOF
    #movz     OF  +#  WIDE  SF? 0= IF  DROP S>D  THEN  +SHEX  ENDOF
    #movn     OF  +#  WIDE  INVERT SWAP INVERT SWAP  SF? 0= IF  DROP S>D  THEN  +SHEX  ENDOF
    #i16.hw   OF  WIDE 2DROP  5 16 FIELD +UIMM
                  21 2 FIELD ?DUP IF  S" , lsl #" +T  16 * +DEC  THEN  ENDOF
    #immr     OF  IMMR +#DEC  ENDOF
    #lsl      OF  REGSIZE 1- IMMS - +#DEC  ENDOF
    #lsb.iz   OF  REGSIZE IMMR -  REGSIZE 1- AND +#DEC  ENDOF
    #width.iz OF  IMMS 1+ +#DEC  ENDOF
    #width.x  OF  IMMS IMMR - 1+ +#DEC  ENDOF
    #imms     OF  IMMS +UIMM  ENDOF
    #imm5     OF  16 5 FIELD +UIMM  ENDOF
    #nzcv     OF  0 4 FIELD +UIMM  ENDOF
    cond      OF  COND# CONDS +NTH  ENDOF
    cond.inv  OF  COND# 1 XOR CONDS +NTH  ENDOF
    #i16      OF  5 16 FIELD  ?DUP IF +UIMM ELSE S" #0" +T THEN  ENDOF
    #u6*16    OF  16 6 FIELD 16 * +UIMM  ENDOF
    #u4       OF  10 4 FIELD +UIMM  ENDOF
    #bit      OF  19 5 FIELD  31 BIT? IF 32 + THEN  +UIMM  ENDOF
    @26       OF  0 26 FIELD 26 SIGNED 4 * +TARGET  ENDOF
    @19       OF  5 19 FIELD 19 SIGNED 4 * +TARGET  ENDOF
    @14       OF  5 14 FIELD 14 SIGNED 4 * +TARGET  ENDOF
    @adr      OF  5 19 FIELD 2 LSHIFT  29 2 FIELD OR  21 SIGNED +TARGET  ENDOF
    @adrp     OF  +PAGE  ENDOF
    #rmif     OF  15 6 FIELD +UIMM  ENDOF
    #mask     OF  0 4 FIELD +UIMM  ENDOF
    prfop     OF  +PRFOP  ENDOF
    barrier   OF  +BARRIER  ENDOF
    barrier.nxs OF  10 2 FIELD NXS +NTH  S" nxs" +T  ENDOF
    #crm      OF  8 4 FIELD +UIMM  ENDOF
    #crm.d    OF  8 4 FIELD +#DEC  ENDOF
    sysreg    OF  +SYSREG  ENDOF
    #udf      OF  0 16 FIELD +UIMM  ENDOF
    #op1      OF  16 3 FIELD +UIMM  ENDOF
    Cn        OF  S" c" +T  12 4 FIELD +DEC  ENDOF
    Cm        OF  S" c" +T  8 4 FIELD +DEC  ENDOF
    #op2      OF  5 3 FIELD +UIMM  ENDOF
    sm        OF  S" sm" +T  ENDOF
    za        OF  S" za" +T  ENDOF
  ENDCASE ;

\ ==== The instruction ======================================================

: +MODIFIER ( k -- )   \ a, l, b, h, .eq ... after the name
  CASE
    .ar   OF  23 BIT? IF S" a" +T THEN  22 BIT? IF S" l" +T THEN  ENDOF
    .l    OF  22 BIT? IF S" l" +T THEN  ENDOF
    .cas  OF  22 BIT? IF S" a" +T THEN  15 BIT? IF S" l" +T THEN  ENDOF
    .sz   OF  30 2 FIELD  DUP 0= IF S" b" +T THEN  1 = IF S" h" +T THEN  ENDOF
    .cond OF  S" ." +T  0 4 FIELD CONDS +NTH  ENDOF
  ENDCASE ;
: +HINT ( -- )
  5 7 FIELD  DUP 41 < IF  DUP HINTS NTH  ?DUP IF  +T DROP EXIT  THEN  DROP  THEN
  S" hint " +T  +UIMM ;
: +MSRI ( -- )   \ MSR (immediate): the field of PSTATE, or a system register
  S" msr " +T
  16 3 FIELD 3 LSHIFT  5 3 FIELD OR
  DUP 8 = IF  DROP  8 4 FIELD 2/ ?DUP IF
      1 = IF  S" PM" +T  S" , " +T  8 1 FIELD +UIMM  EXIT  THEN
    ELSE  S" ALLINT" +T  S" , " +T  8 1 FIELD +UIMM  EXIT  THEN
    +GENERIC  S" , " +T  RD# +X  EXIT
  THEN
  PSTATES  BEGIN  DUP C@ 255 <> WHILE
    2DUP C@ = IF  NIP 1+ COUNT +T  S" , " +T  8 4 FIELD +UIMM  EXIT  THEN
    1+ COUNT +
  REPEAT  2DROP  +GENERIC  S" , " +T  RD# +X ;
: +SYS ( -- )   \ SYS: the name of the operation, or sys #op1, Cn, Cm, #op2, Xt
  5 14 FIELD SYS-NAME IF
    >R +T  R> ?DUP IF  1 = IF S" , " ELSE S"  " THEN +T  RD# +X  THEN  EXIT
  THEN
  S" sys " +T  16 3 FIELD +UIMM  S" , c" +T  12 4 FIELD +DEC  S" , c" +T  8 4 FIELD +DEC
  S" , " +T  5 3 FIELD +UIMM  RD# 31 <> IF  S" , " +T  RD# +X  THEN ;
: +WHOLE ( k -- )
  CASE  @hint OF +HINT ENDOF  @msri OF +MSRI ENDOF  @sys OF +SYS ENDOF  ENDCASE ;
: +OPERAND ( k -- )
  DUP 50 < IF  +REGISTER EXIT  THEN
  DUP 80 < IF  +MEMORY EXIT  THEN
  +OTHER ;
VARIABLE FIRST
: +SEP ( -- )   FIRST @ IF  S"  " +T  FALSE FIRST !  ELSE  S" , " +T  THEN ;
: +INSN ( e -- )
  DUP 0 KIND  DUP 130 140 WITHIN IF  +WHOLE DROP EXIT  THEN  DROP
  DUP NAME +T
  6 0 DO  DUP I KIND  DUP 120 130 WITHIN IF +MODIFIER ELSE DROP THEN  LOOP
  TRUE FIRST !
  6 0 DO  DUP I KIND  ?DUP IF
    DUP 120 130 WITHIN IF DROP ELSE  +SEP +OPERAND  THEN
  THEN  LOOP  DROP ;
: FIND-INSN ( -- e | 0 )   \ the entry of the instruction in INSN
  INSN @ GROUP CELLS FIRSTS + @
  BEGIN  DUP WHILE
    INSN @ OVER MASK AND  OVER MATCH = IF
      DUP PRED ?DUP IF EXECUTE ELSE TRUE THEN IF EXIT THEN
    THEN  @
  REPEAT ;
: +INST ( -- )   \ .inst and the number of the instruction
  TXT-START  S" .inst 0x" +T
  8 0 DO  INSN @ 28 I 4 * - RSHIFT 15 AND  DUP 10 < IF '0' ELSE 'a' 10 - THEN + +C  LOOP ;
: A64-DECODE ( a -- a' )
  DUP PC !  INSN@ INSN !  TXT-START  FALSE BAD !
  FIND-INSN ?DUP IF +INSN ELSE BAD! THEN
  BAD @ IF +INST THEN
  PC @ 4 + ;
: .WORD ( a a' -- )   \ the instruction as a number of 32 bits
  DROP INSN@ 8 .HEXN  2 SPACES ;

PUBLIC
: A64 ( -- )   ['] A64-DECODE IS DECODE  ['] .WORD IS .CODE  8 ADDR-DIGITS ! ;

END-PACKAGE
A64
