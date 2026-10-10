\ disxt.fs - package DISXT: a disassembler for the Xtensa (ESP32, ESP32-S3)
\
\ It turns the bytes of an instruction into its text, for DISASM of the
\ package MONITOR. It knows the instructions of the ESP32 (Xtensa LX6)
\ as the GNU tools of Espressif know them: the core and the options of
\ the ESP32 (code density, windowed registers, loops, booleans, MAC16,
\ multiply and divide, floating point, exceptions, interrupts, debug,
\ the special registers) and the instructions that Espressif added
\ (f64..., expstate). The ESP32-S3 (Xtensa LX7) has the same ones
\ without MAC16; its own instructions (PIE, ee...) show as bytes.
\
\ The text is the form of the GNU tools: lower case; the registers a0
\ to a15, f0 to f15, b0 to b15, m0 to m3; a number from -255 to 255 in
\ decimal, any other as 0x and hex digits; a special register in the
\ name (rsr.ps a2); a jump with its target:
\
\   40080000  36 41 00                 entry a1, 32
\   40080003  21 ff ff                 l32r a2, 0x40080000
\   40080006  20 e6 03                 rsr.ps a2
\   40080009  0c 03                    movi.n a3, 0
\
\ An instruction that the table does not know shows as .byte with all
\ its bytes (3 for op0 0 to 7, 2 for 8 to 13). The README of the
\ monitor lists where this differs from GNU objdump.
\
\   XT-ESP32 ( -- )     DECODE of MONITOR disassembles code of the ESP32
\                       (so after the load)
\   XT-ESP32S3 ( -- )   the same for the ESP32-S3: op0 4 (MAC16 on the
\                       ESP32) is the PIE there, and op0 14 and 15 start
\                       instructions of 4 bytes
\
\ Needs MONITOR and cells of 32 bits.
\ Load after MONITOR:  ALSO FILES  INCLUDE asm/monitor/disxt.fs

PACKAGE DISXT  ALSO MONITOR   \ lib/package.fs
: NEED-32 ( -- )   CELL 4 <> ABORT" DISXT needs cells of 32 bits" ;  NEED-32

\ ==== Reading the instruction =============================================
\ Xtensa code is little endian. op0 (the low 4 bits of the first byte)
\ gives the length. INSN holds the instruction as one number, as the
\ manual shows it: op0 in bits 0 to 3, t 4 to 7, s 8 to 11, r 12 to 15,
\ op1 16 to 19, op2 20 to 23.

VARIABLE DA                          \ the address of the next byte
VARIABLE PC                          \ the address of the instruction
VARIABLE INSN   VARIABLE ILEN        \ the instruction, its length in bytes
VARIABLE S3?                         \ true: the code of an ESP32-S3
: BYTE+ ( -- c )   DA @ DIS-C@  1 DA +! ;
: LEN ( op0 -- n )
  DUP 8 < IF  DROP 3 EXIT  THEN
  14 < IF  2 EXIT  THEN
  S3? @ IF 4 ELSE 1 THEN ;
: READ ( a -- )   \ the instruction at a: INSN, ILEN
  DUP PC !  DA !
  BYTE+  DUP 15 AND LEN ILEN !
  ILEN @ 1 ?DO  BYTE+ I 8 * LSHIFT OR  LOOP  INSN ! ;
: FIELD ( lo n -- u )   \ n bits of INSN from bit lo on
  >R  INSN @ SWAP RSHIFT  1 R> LSHIFT 1- AND ;
: R# ( -- n )   12 4 FIELD ;
: S# ( -- n )   8 4 FIELD ;
: T# ( -- n )   4 4 FIELD ;
: SIGNED ( u n -- x )   \ u, a number of n bits, with its sign
  1 SWAP 1- LSHIFT  2DUP AND IF  2* -  ELSE  DROP  THEN ;

\ ==== Names of special registers ==========================================
\ n SR: access name, as GNU writes them for the ESP32; access: r for
\ RSR, w for WSR, x for XSR. n UR: the same for RUR and WUR.

: STR, ( a u -- )   \ a counted string, without alignment
  DUP C,  0 ?DO  DUP I + C@ C,  LOOP  DROP ;
: ACCESS ( "rwx" -- flags )   \ r 1, w 2, x 4
  0  PARSE-NAME 0 ?DO
    DUP I + C@  DUP 'r' = IF DROP SWAP 1 OR SWAP ELSE
    DUP 'w' = IF DROP SWAP 2 OR SWAP ELSE  'x' = IF SWAP 4 OR SWAP THEN THEN THEN
  LOOP  DROP ;
: REG-NAME, ( n "access" "name" -- )   C,  ACCESS C,  PARSE-NAME STR, ;
: SR: ( n "access" "name" -- )   REG-NAME, ;
: UR: ( n "access" "name" -- )   REG-NAME, ;
CREATE SRS
0 SR: rwx lbeg   1 SR: rwx lend   2 SR: rwx lcount   3 SR: rwx sar
4 SR: rwx br   5 SR: rwx litbase   12 SR: rwx scompare1
16 SR: rwx acclo   17 SR: rwx acchi   32 SR: rwx m0   33 SR: rwx m1
34 SR: rwx m2   35 SR: rwx m3   72 SR: rwx windowbase
73 SR: rwx windowstart   89 SR: -w- mmid   96 SR: rwx ibreakenable
97 SR: rwx memctl   99 SR: rwx atomctl   104 SR: rwx ddr
128 SR: rwx ibreaka0   129 SR: rwx ibreaka1   144 SR: rwx dbreaka0
145 SR: rwx dbreaka1   160 SR: rwx dbreakc0   161 SR: rwx dbreakc1
176 SR: rw- configid0   177 SR: rwx epc1   178 SR: rwx epc2
179 SR: rwx epc3   180 SR: rwx epc4   181 SR: rwx epc5   182 SR: rwx epc6
183 SR: rwx epc7   192 SR: rwx depc   194 SR: rwx eps2   195 SR: rwx eps3
196 SR: rwx eps4   197 SR: rwx eps5   198 SR: rwx eps6   199 SR: rwx eps7
208 SR: r-- configid1   209 SR: rwx excsave1   210 SR: rwx excsave2
211 SR: rwx excsave3   212 SR: rwx excsave4   213 SR: rwx excsave5
214 SR: rwx excsave6   215 SR: rwx excsave7   224 SR: rwx cpenable
226 SR: r-- interrupt   226 SR: -w- intset   227 SR: -w- intclear
228 SR: rwx intenable   230 SR: rwx ps   231 SR: rwx vecbase
232 SR: rwx exccause   233 SR: rwx debugcause   234 SR: rwx ccount
235 SR: r-- prid   236 SR: rwx icount   237 SR: rwx icountlevel
238 SR: rwx excvaddr   240 SR: rwx ccompare0   241 SR: rwx ccompare1
242 SR: rwx ccompare2   244 SR: rwx misc0   245 SR: rwx misc1
246 SR: rwx misc2   247 SR: rwx misc3
0 C, 0 C,                            \ the end: access 0
CREATE URS
230 UR: rw expstate   231 UR: rw threadptr   232 UR: rw fcr   233 UR: rw fsr
234 UR: rw f64r_lo   235 UR: rw f64r_hi   236 UR: rw f64s
0 C, 0 C,  ALIGN
: REG-NAME ( n access table -- a u true | false )   \ the name of register n with this access
  BEGIN  DUP 1+ C@ WHILE                             ( n access e )
    >R  OVER R@ C@ =  OVER R@ 1+ C@ AND 0<> AND IF
      2DROP  R> 2 + COUNT TRUE EXIT
    THEN  R> 2 + COUNT +
  REPEAT  DROP 2DROP FALSE ;

\ ==== Kinds of operands ==================================================
\ Each kind of operand has a number; its name is a word here, so the
\ table below can name it. The fields of the instruction (r, s, t, imm8
\ ...) are those of the Xtensa manual, chapter 7.
\
\   ar as at   the register a0 to a15 in field r, s or t
\   fr fs ft   the same for f0 to f15; br bs bt for b0 to b15
\   b4 b8      4 or 8 registers b from field s: b4:b5:b6:b7
\   mw mx my   m0 to m3 in bits 12-13, m0 or m1 (bit 14), m2 or m3 (bit 6)
\   i8 i8x2 i8x4   imm8 (bits 16-23), times 1, 2 or 4
\   s8 s8x256  imm8 with its sign, times 1 or 256 (ADDI, ADDMI)
\   s12        imm8 and s with its sign (MOVI)
\   r4x4 e4    r times 4; (r - 16) times 4 (L32E, S32E)
\   ai4 i7     t, 0 for -1 (ADDI.N); the 7 bits of MOVI.N, 96 to 127 for
\              -32 to -1
\   sa32 sas   the shift of SLLI (32 - sa) and of SRAI
\   s4 t4 t7   the field s, t, t + 7 (SEXT, CLAMPS)
\   st5 xsh xm the shift of SSAI and EXTUI, the bits of EXTUI
\   t4s e12    t with its sign (ROTW); imm12 times 8 (ENTRY)
\   c4 c4u     the constant of BEQI ... BGEI, of BLTUI and BGEUI (r)
\   bbi        the bit of BBCI and BBSI
\   j8 j12 j18 the target: PC + 4 + the offset of 8, 12 or 18 bits
\   c18 lr lp j6   the target of CALLn, L32R, LOOP, BEQZ.N and BNEZ.N
\   x4 x1 x2 x22 y2 y1 z1 t5   fields of the instructions of Espressif
\   .rsr .wsr .xsr .rur .wur   a special register in the name

: KINDS ( n0 n "name" ... -- )   0 ?DO  DUP CONSTANT 1+  LOOP  DROP ;
1 14 KINDS  ar as at fr fs ft br bs bt b4 b8 mw mx my
15 23 KINDS i8 i8x2 i8x4 s8 s8x256 s12 r4x4 e4 ai4 i7 sa32 sas s4 t4 t7 st5 xsh xm t4s e12 c4 c4u bbi
38 7 KINDS  j8 j12 j18 c18 lr lp j6
45 8 KINDS  x4 x1 x2 x22 y2 y1 z1 t5
53 5 KINDS  .rsr .wsr .xsr .rur .wur

\ ==== The table ===========================================================
\ An entry: the link to the next entry of its op0, the mask (the fixed
\ bits), the value of the fixed bits, 6 bytes for the kinds of the
\ operands (0 ends them), the name. OP: makes an entry from a line: the
\ name, the bits from 23 down to 0 in groups of 4 (0 and 1 are fixed, a
\ letter is a field), the kinds of the operands separated by commas, or
\ "-" for none. OPN: the same for an instruction of 16 bits.

CREATE LISTS  16 CELLS ALLOT   LISTS 16 CELLS 0 FILL   \ the entries for each op0
VARIABLE PMASK   VARIABLE PMATCH
: PBIT ( c -- )   \ the next bit of a pattern
  PMASK @ 2* PMASK !  PMATCH @ 2* PMATCH !
  DUP '0' = IF  DROP  1 PMASK +!  EXIT  THEN
  '1' = IF  1 PMASK +!  1 PMATCH +!  THEN ;
: PATTERN ( n "groups" -- )   \ n groups of 4 bits, the highest first
  0 PMASK !  0 PMATCH !
  0 ?DO  PARSE-NAME  DUP 4 <> ABORT" DISXT: a group has 4 bits"
    0 ?DO  DUP I + C@ PBIT  LOOP  DROP
  LOOP ;
: KIND# ( a u -- k )   FIND-NAME DUP 0= ABORT" DISXT: no such kind of operand"  NAME>XT EXECUTE ;
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
VARIABLE NA   VARIABLE NU
: ENTRY, ( n "name" "groups" "kinds" -- )
  PARSE-NAME NU ! NA !  PATTERN
  ALIGN HERE  PMATCH @ 15 AND CELLS LISTS +  DUP @ ,  !   \ the link: the newest first
  PMASK @ ,  PMATCH @ ,
  HERE 6 ALLOT  DUP 6 0 FILL  KINDS!
  NA @ NU @ STR,  ALIGN ;
: OP: ( "name" "6 groups" "kinds" -- )   6 ENTRY, ;
: OPN: ( "name" "4 groups" "kinds" -- )   4 ENTRY, ;
: MASK ( e -- x )    CELL+ @ ;
: MATCH ( e -- x )   2 CELLS + @ ;
: KIND ( e i -- k )   SWAP 3 CELLS + + C@ ;
: NAME ( e -- a u )   3 CELLS + 6 + COUNT ;

\ ---- op0 0, QRST: the core and its options (Xtensa manual, tables 7-193 on)
OP:   ill               0000 0000 0000 0000 0000 0000   -
OP:   ret               0000 0000 0000 0000 1000 0000   -
OP:   retw              0000 0000 0000 0000 1001 0000   -
OP:   jx                0000 0000 0000 ssss 1010 0000   as
OP:   callx0            0000 0000 0000 ssss 1100 0000   as
OP:   callx4            0000 0000 0000 ssss 1101 0000   as
OP:   callx8            0000 0000 0000 ssss 1110 0000   as
OP:   callx12           0000 0000 0000 ssss 1111 0000   as
OP:   movsp             0000 0000 0001 ssss tttt 0000   at,as
OP:   isync             0000 0000 0010 0000 0000 0000   -
OP:   rsync             0000 0000 0010 0000 0001 0000   -
OP:   esync             0000 0000 0010 0000 0010 0000   -
OP:   dsync             0000 0000 0010 0000 0011 0000   -
OP:   excw              0000 0000 0010 0000 1000 0000   -
OP:   memw              0000 0000 0010 0000 1100 0000   -
OP:   extw              0000 0000 0010 0000 1101 0000   -
OP:   nop               0000 0000 0010 0000 1111 0000   -
OP:   rfe               0000 0000 0011 0000 0000 0000   -
OP:   rfde              0000 0000 0011 0010 0000 0000   -
OP:   rfwo              0000 0000 0011 0100 0000 0000   -
OP:   rfwu              0000 0000 0011 0101 0000 0000   -
OP:   rfi               0000 0000 0011 ssss 0001 0000   s4
OP:   break             0000 0000 0100 ssss tttt 0000   s4,t4
OP:   syscall           0000 0000 0101 0000 0000 0000   -
OP:   simcall           0000 0000 0101 0001 0000 0000   -
OP:   rsil              0000 0000 0110 ssss tttt 0000   at,s4
OP:   waiti             0000 0000 0111 ssss 0000 0000   s4
OP:   lddr32.p          0000 0000 0111 ssss 1110 0000   as
OP:   sddr32.p          0000 0000 0111 ssss 1111 0000   as
OP:   any4              0000 0000 1000 ss00 tttt 0000   bt,b4
OP:   all4              0000 0000 1001 ss00 tttt 0000   bt,b4
OP:   any8              0000 0000 1010 s000 tttt 0000   bt,b8
OP:   all8              0000 0000 1011 s000 tttt 0000   bt,b8
OP:   and               0001 0000 rrrr ssss tttt 0000   ar,as,at
OP:   or                0010 0000 rrrr ssss tttt 0000   ar,as,at
OP:   xor               0011 0000 rrrr ssss tttt 0000   ar,as,at
OP:   ssr               0100 0000 0000 ssss 0000 0000   as
OP:   ssl               0100 0000 0001 ssss 0000 0000   as
OP:   ssa8l             0100 0000 0010 ssss 0000 0000   as
OP:   ssa8b             0100 0000 0011 ssss 0000 0000   as
OP:   ssai              0100 0000 0100 ssss 000t 0000   st5
OP:   rer               0100 0000 0110 ssss tttt 0000   at,as
OP:   wer               0100 0000 0111 ssss tttt 0000   at,as
OP:   rotw              0100 0000 1000 0000 tttt 0000   t4s
OP:   nsa               0100 0000 1110 ssss tttt 0000   at,as
OP:   nsau              0100 0000 1111 ssss tttt 0000   at,as
OP:   ritlb0            0101 0000 0011 ssss tttt 0000   at,as
OP:   iitlb             0101 0000 0100 ssss 0000 0000   as
OP:   pitlb             0101 0000 0101 ssss tttt 0000   at,as
OP:   witlb             0101 0000 0110 ssss tttt 0000   at,as
OP:   ritlb1            0101 0000 0111 ssss tttt 0000   at,as
OP:   rdtlb0            0101 0000 1011 ssss tttt 0000   at,as
OP:   idtlb             0101 0000 1100 ssss 0000 0000   as
OP:   pdtlb             0101 0000 1101 ssss tttt 0000   at,as
OP:   wdtlb             0101 0000 1110 ssss tttt 0000   at,as
OP:   rdtlb1            0101 0000 1111 ssss tttt 0000   at,as
OP:   neg               0110 0000 rrrr 0000 tttt 0000   ar,at
OP:   abs               0110 0000 rrrr 0001 tttt 0000   ar,at
OP:   add               1000 0000 rrrr ssss tttt 0000   ar,as,at
OP:   addx2             1001 0000 rrrr ssss tttt 0000   ar,as,at
OP:   addx4             1010 0000 rrrr ssss tttt 0000   ar,as,at
OP:   addx8             1011 0000 rrrr ssss tttt 0000   ar,as,at
OP:   sub               1100 0000 rrrr ssss tttt 0000   ar,as,at
OP:   subx2             1101 0000 rrrr ssss tttt 0000   ar,as,at
OP:   subx4             1110 0000 rrrr ssss tttt 0000   ar,as,at
OP:   subx8             1111 0000 rrrr ssss tttt 0000   ar,as,at
OP:   slli              000i 0001 rrrr ssss tttt 0000   ar,as,sa32
OP:   srai              001i 0001 rrrr ssss tttt 0000   ar,at,sas
OP:   srli              0100 0001 rrrr ssss tttt 0000   ar,at,s4
OP:   xsr               0110 0001 rrrr ssss tttt 0000   .xsr,at
OP:   src               1000 0001 rrrr ssss tttt 0000   ar,as,at
OP:   srl               1001 0001 rrrr 0000 tttt 0000   ar,at
OP:   sll               1010 0001 rrrr ssss 0000 0000   ar,as
OP:   sra               1011 0001 rrrr 0000 tttt 0000   ar,at
OP:   mul16u            1100 0001 rrrr ssss tttt 0000   ar,as,at
OP:   mul16s            1101 0001 rrrr ssss tttt 0000   ar,as,at
OP:   rfdo              1111 0001 1110 ssss 0000 0000   s4
OP:   rfdd              1111 0001 1110 0000 0001 0000   -
OP:   andb              0000 0010 rrrr ssss tttt 0000   br,bs,bt
OP:   andbc             0001 0010 rrrr ssss tttt 0000   br,bs,bt
OP:   orb               0010 0010 rrrr ssss tttt 0000   br,bs,bt
OP:   orbc              0011 0010 rrrr ssss tttt 0000   br,bs,bt
OP:   xorb              0100 0010 rrrr ssss tttt 0000   br,bs,bt
OP:   mull              1000 0010 rrrr ssss tttt 0000   ar,as,at
OP:   muluh             1010 0010 rrrr ssss tttt 0000   ar,as,at
OP:   mulsh             1011 0010 rrrr ssss tttt 0000   ar,as,at
OP:   quou              1100 0010 rrrr ssss tttt 0000   ar,as,at
OP:   quos              1101 0010 rrrr ssss tttt 0000   ar,as,at
OP:   remu              1110 0010 rrrr ssss tttt 0000   ar,as,at
OP:   rems              1111 0010 rrrr ssss tttt 0000   ar,as,at
OP:   rsr               0000 0011 rrrr ssss tttt 0000   .rsr,at
OP:   wsr               0001 0011 rrrr ssss tttt 0000   .wsr,at
OP:   sext              0010 0011 rrrr ssss tttt 0000   ar,as,t7
OP:   clamps            0011 0011 rrrr ssss tttt 0000   ar,as,t7
OP:   min               0100 0011 rrrr ssss tttt 0000   ar,as,at
OP:   max               0101 0011 rrrr ssss tttt 0000   ar,as,at
OP:   minu              0110 0011 rrrr ssss tttt 0000   ar,as,at
OP:   maxu              0111 0011 rrrr ssss tttt 0000   ar,as,at
OP:   moveqz            1000 0011 rrrr ssss tttt 0000   ar,as,at
OP:   movnez            1001 0011 rrrr ssss tttt 0000   ar,as,at
OP:   movltz            1010 0011 rrrr ssss tttt 0000   ar,as,at
OP:   movgez            1011 0011 rrrr ssss tttt 0000   ar,as,at
OP:   movf              1100 0011 rrrr ssss tttt 0000   ar,as,bt
OP:   movt              1101 0011 rrrr ssss tttt 0000   ar,as,bt
OP:   rur               1110 0011 rrrr ssss tttt 0000   .rur,ar
OP:   wur               1111 0011 rrrr ssss tttt 0000   .wur,at
OP:   extui             iiii 010i rrrr ssss tttt 0000   ar,at,xsh,xm
OP:   lsx               0000 1000 rrrr ssss tttt 0000   fr,as,at
OP:   lsxp              0001 1000 rrrr ssss tttt 0000   fr,as,at
OP:   ssx               0100 1000 rrrr ssss tttt 0000   fr,as,at
OP:   ssxp              0101 1000 rrrr ssss tttt 0000   fr,as,at
OP:   l32e              0000 1001 rrrr ssss tttt 0000   at,as,e4
OP:   s32e              0100 1001 rrrr ssss tttt 0000   at,as,e4
OP:   s32nb             0101 1001 rrrr ssss tttt 0000   at,as,r4x4
OP:   add.s             0000 1010 rrrr ssss tttt 0000   fr,fs,ft
OP:   sub.s             0001 1010 rrrr ssss tttt 0000   fr,fs,ft
OP:   mul.s             0010 1010 rrrr ssss tttt 0000   fr,fs,ft
OP:   madd.s            0100 1010 rrrr ssss tttt 0000   fr,fs,ft
OP:   msub.s            0101 1010 rrrr ssss tttt 0000   fr,fs,ft
OP:   maddn.s           0110 1010 rrrr ssss tttt 0000   fr,fs,ft
OP:   divn.s            0111 1010 rrrr ssss tttt 0000   fr,fs,ft
OP:   round.s           1000 1010 rrrr ssss tttt 0000   ar,fs,t4
OP:   trunc.s           1001 1010 rrrr ssss tttt 0000   ar,fs,t4
OP:   floor.s           1010 1010 rrrr ssss tttt 0000   ar,fs,t4
OP:   ceil.s            1011 1010 rrrr ssss tttt 0000   ar,fs,t4
OP:   float.s           1100 1010 rrrr ssss tttt 0000   fr,as,t4
OP:   ufloat.s          1101 1010 rrrr ssss tttt 0000   fr,as,t4
OP:   utrunc.s          1110 1010 rrrr ssss tttt 0000   ar,fs,t4
OP:   mov.s             1111 1010 rrrr ssss 0000 0000   fr,fs
OP:   abs.s             1111 1010 rrrr ssss 0001 0000   fr,fs
OP:   const.s           1111 1010 rrrr ssss 0011 0000   fr,s4
OP:   rfr               1111 1010 rrrr ssss 0100 0000   ar,fs
OP:   wfr               1111 1010 rrrr ssss 0101 0000   fr,as
OP:   neg.s             1111 1010 rrrr ssss 0110 0000   fr,fs
OP:   div0.s            1111 1010 rrrr ssss 0111 0000   fr,fs
OP:   recip0.s          1111 1010 rrrr ssss 1000 0000   fr,fs
OP:   sqrt0.s           1111 1010 rrrr ssss 1001 0000   fr,fs
OP:   rsqrt0.s          1111 1010 rrrr ssss 1010 0000   fr,fs
OP:   nexp01.s          1111 1010 rrrr ssss 1011 0000   fr,fs
OP:   mksadj.s          1111 1010 rrrr ssss 1100 0000   fr,fs
OP:   mkdadj.s          1111 1010 rrrr ssss 1101 0000   fr,fs
OP:   addexp.s          1111 1010 rrrr ssss 1110 0000   fr,fs
OP:   addexpm.s         1111 1010 rrrr ssss 1111 0000   fr,fs
OP:   un.s              0001 1011 rrrr ssss tttt 0000   br,fs,ft
OP:   oeq.s             0010 1011 rrrr ssss tttt 0000   br,fs,ft
OP:   ueq.s             0011 1011 rrrr ssss tttt 0000   br,fs,ft
OP:   olt.s             0100 1011 rrrr ssss tttt 0000   br,fs,ft
OP:   ult.s             0101 1011 rrrr ssss tttt 0000   br,fs,ft
OP:   ole.s             0110 1011 rrrr ssss tttt 0000   br,fs,ft
OP:   ule.s             0111 1011 rrrr ssss tttt 0000   br,fs,ft
OP:   moveqz.s          1000 1011 rrrr ssss tttt 0000   fr,fs,at
OP:   movnez.s          1001 1011 rrrr ssss tttt 0000   fr,fs,at
OP:   movltz.s          1010 1011 rrrr ssss tttt 0000   fr,fs,at
OP:   movgez.s          1011 1011 rrrr ssss tttt 0000   fr,fs,at
OP:   movf.s            1100 1011 rrrr ssss tttt 0000   fr,fs,bt
OP:   movt.s            1101 1011 rrrr ssss tttt 0000   fr,fs,bt
\ the instructions of Espressif (op1 11, 14, 15)
OP:   rf64r             0000 1011 1100 111s tttt 0000   at,z1
OP:   wf64r             0000 1011 111r ssss tttt 0000   as,at,y1
OP:   f64cmpl           1110 1011 rrrr ssss tttt 0000   ar,as,at
OP:   f64addc           1111 1011 0rrr ssss tttt 0000   at,as,y2,y1
OP:   f64subc           1111 1011 1rrr ssss tttt 0000   at,as,y2,y1
OP:   read_impwire      0000 1110 0000 0000 tttt 0000   at
OP:   setb_expstate     0000 1110 0001 000s tttt 0000   t5
OP:   clrb_expstate     0000 1110 0001 001s tttt 0000   t5
OP:   wrmsk_expstate    0000 1110 0010 ssss tttt 0000   at,as
OP:   f64sig            0000 1110 1101 ssss tttt 0000   at,as
OP:   f64sexp           0001 1110 rrrr ssss tttt 0000   ar,as,at
OP:   f64norm           001i 1110 rrrr ssss tttt 0000   ar,as,at,x1
OP:   f64rnd            01ii 1110 rrrr ssss tttt 0000   ar,as,at,x2
OP:   f64iter           1iii 1110 rrrr ssss tttt 0000   ar,as,at,x2,x22
OP:   f64cmph           iiii 1111 rrrr ssss tttt 0000   ar,as,at,x4

\ ---- op0 1 to 3: L32R, LSAI, LSCI
OP:   l32r              iiii iiii iiii iiii tttt 0001   at,lr
OP:   l8ui              iiii iiii 0000 ssss tttt 0010   at,as,i8
OP:   l16ui             iiii iiii 0001 ssss tttt 0010   at,as,i8x2
OP:   l32i              iiii iiii 0010 ssss tttt 0010   at,as,i8x4
OP:   s8i               iiii iiii 0100 ssss tttt 0010   at,as,i8
OP:   s16i              iiii iiii 0101 ssss tttt 0010   at,as,i8x2
OP:   s32i              iiii iiii 0110 ssss tttt 0010   at,as,i8x4
OP:   l16si             iiii iiii 1001 ssss tttt 0010   at,as,i8x2
OP:   movi              iiii iiii 1010 ssss tttt 0010   at,s12
OP:   l32ai             iiii iiii 1011 ssss tttt 0010   at,as,i8x4
OP:   addi              iiii iiii 1100 ssss tttt 0010   at,as,s8
OP:   addmi             iiii iiii 1101 ssss tttt 0010   at,as,s8x256
OP:   s32c1i            iiii iiii 1110 ssss tttt 0010   at,as,i8x4
OP:   s32ri             iiii iiii 1111 ssss tttt 0010   at,as,i8x4
OP:   lsi               iiii iiii 0000 ssss tttt 0011   ft,as,i8x4
OP:   ssi               iiii iiii 0100 ssss tttt 0011   ft,as,i8x4
OP:   lsip              iiii iiii 1000 ssss tttt 0011   ft,as,i8x4
OP:   ssip              iiii iiii 1100 ssss tttt 0011   ft,as,i8x4

\ ---- op0 4: MAC16 (ESP32 only; the ESP32-S3 has the PIE there)
OP:   mula.dd.ll.ldinc  0000 1000 0rrr ssss 0t00 0100   mw,as,mx,my
OP:   mula.dd.hl.ldinc  0000 1001 0rrr ssss 0t00 0100   mw,as,mx,my
OP:   mula.dd.lh.ldinc  0000 1010 0rrr ssss 0t00 0100   mw,as,mx,my
OP:   mula.dd.hh.ldinc  0000 1011 0rrr ssss 0t00 0100   mw,as,mx,my
OP:   mula.dd.ll.lddec  0001 1000 0rrr ssss 0t00 0100   mw,as,mx,my
OP:   mula.dd.hl.lddec  0001 1001 0rrr ssss 0t00 0100   mw,as,mx,my
OP:   mula.dd.lh.lddec  0001 1010 0rrr ssss 0t00 0100   mw,as,mx,my
OP:   mula.dd.hh.lddec  0001 1011 0rrr ssss 0t00 0100   mw,as,mx,my
OP:   mul.dd.ll         0010 0100 0r00 0000 0t00 0100   mx,my
OP:   mul.dd.hl         0010 0101 0r00 0000 0t00 0100   mx,my
OP:   mul.dd.lh         0010 0110 0r00 0000 0t00 0100   mx,my
OP:   mul.dd.hh         0010 0111 0r00 0000 0t00 0100   mx,my
OP:   mula.dd.ll        0010 1000 0r00 0000 0t00 0100   mx,my
OP:   mula.dd.hl        0010 1001 0r00 0000 0t00 0100   mx,my
OP:   mula.dd.lh        0010 1010 0r00 0000 0t00 0100   mx,my
OP:   mula.dd.hh        0010 1011 0r00 0000 0t00 0100   mx,my
OP:   muls.dd.ll        0010 1100 0r00 0000 0t00 0100   mx,my
OP:   muls.dd.hl        0010 1101 0r00 0000 0t00 0100   mx,my
OP:   muls.dd.lh        0010 1110 0r00 0000 0t00 0100   mx,my
OP:   muls.dd.hh        0010 1111 0r00 0000 0t00 0100   mx,my
OP:   mul.ad.ll         0011 0100 0000 ssss 0t00 0100   as,my
OP:   mul.ad.hl         0011 0101 0000 ssss 0t00 0100   as,my
OP:   mul.ad.lh         0011 0110 0000 ssss 0t00 0100   as,my
OP:   mul.ad.hh         0011 0111 0000 ssss 0t00 0100   as,my
OP:   mula.ad.ll        0011 1000 0000 ssss 0t00 0100   as,my
OP:   mula.ad.hl        0011 1001 0000 ssss 0t00 0100   as,my
OP:   mula.ad.lh        0011 1010 0000 ssss 0t00 0100   as,my
OP:   mula.ad.hh        0011 1011 0000 ssss 0t00 0100   as,my
OP:   muls.ad.ll        0011 1100 0000 ssss 0t00 0100   as,my
OP:   muls.ad.hl        0011 1101 0000 ssss 0t00 0100   as,my
OP:   muls.ad.lh        0011 1110 0000 ssss 0t00 0100   as,my
OP:   muls.ad.hh        0011 1111 0000 ssss 0t00 0100   as,my
OP:   mula.da.ll.ldinc  0100 1000 0rrr ssss tttt 0100   mw,as,mx,at
OP:   mula.da.hl.ldinc  0100 1001 0rrr ssss tttt 0100   mw,as,mx,at
OP:   mula.da.lh.ldinc  0100 1010 0rrr ssss tttt 0100   mw,as,mx,at
OP:   mula.da.hh.ldinc  0100 1011 0rrr ssss tttt 0100   mw,as,mx,at
OP:   mula.da.ll.lddec  0101 1000 0rrr ssss tttt 0100   mw,as,mx,at
OP:   mula.da.hl.lddec  0101 1001 0rrr ssss tttt 0100   mw,as,mx,at
OP:   mula.da.lh.lddec  0101 1010 0rrr ssss tttt 0100   mw,as,mx,at
OP:   mula.da.hh.lddec  0101 1011 0rrr ssss tttt 0100   mw,as,mx,at
OP:   mul.da.ll         0110 0100 0r00 0000 tttt 0100   mx,at
OP:   mul.da.hl         0110 0101 0r00 0000 tttt 0100   mx,at
OP:   mul.da.lh         0110 0110 0r00 0000 tttt 0100   mx,at
OP:   mul.da.hh         0110 0111 0r00 0000 tttt 0100   mx,at
OP:   mula.da.ll        0110 1000 0r00 0000 tttt 0100   mx,at
OP:   mula.da.hl        0110 1001 0r00 0000 tttt 0100   mx,at
OP:   mula.da.lh        0110 1010 0r00 0000 tttt 0100   mx,at
OP:   mula.da.hh        0110 1011 0r00 0000 tttt 0100   mx,at
OP:   muls.da.ll        0110 1100 0r00 0000 tttt 0100   mx,at
OP:   muls.da.hl        0110 1101 0r00 0000 tttt 0100   mx,at
OP:   muls.da.lh        0110 1110 0r00 0000 tttt 0100   mx,at
OP:   muls.da.hh        0110 1111 0r00 0000 tttt 0100   mx,at
OP:   umul.aa.ll        0111 0000 0000 ssss tttt 0100   as,at
OP:   umul.aa.hl        0111 0001 0000 ssss tttt 0100   as,at
OP:   umul.aa.lh        0111 0010 0000 ssss tttt 0100   as,at
OP:   umul.aa.hh        0111 0011 0000 ssss tttt 0100   as,at
OP:   mul.aa.ll         0111 0100 0000 ssss tttt 0100   as,at
OP:   mul.aa.hl         0111 0101 0000 ssss tttt 0100   as,at
OP:   mul.aa.lh         0111 0110 0000 ssss tttt 0100   as,at
OP:   mul.aa.hh         0111 0111 0000 ssss tttt 0100   as,at
OP:   mula.aa.ll        0111 1000 0000 ssss tttt 0100   as,at
OP:   mula.aa.hl        0111 1001 0000 ssss tttt 0100   as,at
OP:   mula.aa.lh        0111 1010 0000 ssss tttt 0100   as,at
OP:   mula.aa.hh        0111 1011 0000 ssss tttt 0100   as,at
OP:   muls.aa.ll        0111 1100 0000 ssss tttt 0100   as,at
OP:   muls.aa.hl        0111 1101 0000 ssss tttt 0100   as,at
OP:   muls.aa.lh        0111 1110 0000 ssss tttt 0100   as,at
OP:   muls.aa.hh        0111 1111 0000 ssss tttt 0100   as,at
OP:   ldinc             1000 0000 00rr ssss 0000 0100   mw,as
OP:   lddec             1001 0000 00rr ssss 0000 0100   mw,as

\ ---- op0 5 to 7: CALLN, SI, B
OP:   call0             iiii iiii iiii iiii ii00 0101   c18
OP:   call4             iiii iiii iiii iiii ii01 0101   c18
OP:   call8             iiii iiii iiii iiii ii10 0101   c18
OP:   call12            iiii iiii iiii iiii ii11 0101   c18
OP:   j                 iiii iiii iiii iiii ii00 0110   j18
OP:   beqz              iiii iiii iiii ssss 0001 0110   as,j12
OP:   bnez              iiii iiii iiii ssss 0101 0110   as,j12
OP:   bltz              iiii iiii iiii ssss 1001 0110   as,j12
OP:   bgez              iiii iiii iiii ssss 1101 0110   as,j12
OP:   beqi              iiii iiii rrrr ssss 0010 0110   as,c4,j8
OP:   bnei              iiii iiii rrrr ssss 0110 0110   as,c4,j8
OP:   blti              iiii iiii rrrr ssss 1010 0110   as,c4,j8
OP:   bgei              iiii iiii rrrr ssss 1110 0110   as,c4,j8
OP:   entry             iiii iiii iiii ssss 0011 0110   as,e12
OP:   bf                iiii iiii 0000 ssss 0111 0110   bs,j8
OP:   bt                iiii iiii 0001 ssss 0111 0110   bs,j8
OP:   loop              iiii iiii 1000 ssss 0111 0110   as,lp
OP:   loopnez           iiii iiii 1001 ssss 0111 0110   as,lp
OP:   loopgtz           iiii iiii 1010 ssss 0111 0110   as,lp
OP:   bltui             iiii iiii rrrr ssss 1011 0110   as,c4u,j8
OP:   bgeui             iiii iiii rrrr ssss 1111 0110   as,c4u,j8
OP:   bnone             iiii iiii 0000 ssss tttt 0111   as,at,j8
OP:   beq               iiii iiii 0001 ssss tttt 0111   as,at,j8
OP:   blt               iiii iiii 0010 ssss tttt 0111   as,at,j8
OP:   bltu              iiii iiii 0011 ssss tttt 0111   as,at,j8
OP:   ball              iiii iiii 0100 ssss tttt 0111   as,at,j8
OP:   bbc               iiii iiii 0101 ssss tttt 0111   as,at,j8
OP:   bbci              iiii iiii 011i ssss tttt 0111   as,bbi,j8
OP:   bany              iiii iiii 1000 ssss tttt 0111   as,at,j8
OP:   bne               iiii iiii 1001 ssss tttt 0111   as,at,j8
OP:   bge               iiii iiii 1010 ssss tttt 0111   as,at,j8
OP:   bgeu              iiii iiii 1011 ssss tttt 0111   as,at,j8
OP:   bnall             iiii iiii 1100 ssss tttt 0111   as,at,j8
OP:   bbs               iiii iiii 1101 ssss tttt 0111   as,at,j8
OP:   bbsi              iiii iiii 111i ssss tttt 0111   as,bbi,j8

\ ---- op0 8 to 13: the instructions of 16 bits (code density option)
OPN:  l32i.n            iiii ssss tttt 1000   at,as,r4x4
OPN:  s32i.n            iiii ssss tttt 1001   at,as,r4x4
OPN:  add.n             rrrr ssss tttt 1010   ar,as,at
OPN:  addi.n            rrrr ssss iiii 1011   ar,as,ai4
OPN:  movi.n            iiii ssss 0iii 1100   as,i7
OPN:  beqz.n            iiii ssss 10ii 1100   as,j6
OPN:  bnez.n            iiii ssss 11ii 1100   as,j6
OPN:  mov.n             0000 ssss tttt 1101   at,as
OPN:  ret.n             1111 0000 0000 1101   -
OPN:  retw.n            1111 0000 0001 1101   -
OPN:  break.n           1111 ssss 0010 1101   s4
OPN:  nop.n             1111 0000 0011 1101   -
OPN:  ill.n             1111 0000 0110 1101   -

\ ==== The text of the operands ============================================

: +DEC ( n -- )   \ in decimal, with "-"
  DUP 0< IF  S" -" +T  NEGATE  THEN
  BASE @ >R  DECIMAL  0 <# #S #> +T  R> BASE ! ;
: +NUM ( n -- )   \ -255 to 255 in decimal, any other as 0x and 8 hex digits at most
  DUP -255 256 WITHIN IF  +DEC  ELSE  0 +HEX  THEN ;
: +REG ( n c -- )   +C  +DEC ;
: +BREGS ( n count -- )   \ count registers b from n on: b4:b5:b6:b7
  0 ?DO  I IF  S" :" +T  THEN  DUP I + 'b' +REG  LOOP  DROP ;
: +TARGET ( offset -- )   PC @ 4 + +  0 +HEX ;
CREATE B4CONST   -1 , 1 , 2 , 3 , 4 , 5 , 6 , 7 , 8 , 10 , 12 , 16 , 32 , 64 , 128 , 256 ,
CREATE B4CONSTU  32768 , 65536 , 2 , 3 , 4 , 5 , 6 , 7 , 8 , 10 , 12 , 16 , 32 , 64 , 128 , 256 ,
: IMM8 ( -- u )   16 8 FIELD ;
: +OPERAND ( k -- )
  CASE
    ar   OF  R# 'a' +REG  ENDOF
    as   OF  S# 'a' +REG  ENDOF
    at   OF  T# 'a' +REG  ENDOF
    fr   OF  R# 'f' +REG  ENDOF
    fs   OF  S# 'f' +REG  ENDOF
    ft   OF  T# 'f' +REG  ENDOF
    br   OF  R# 'b' +REG  ENDOF
    bs   OF  S# 'b' +REG  ENDOF
    bt   OF  T# 'b' +REG  ENDOF
    b4   OF  S# 4 +BREGS  ENDOF
    b8   OF  S# 8 +BREGS  ENDOF
    mw   OF  12 2 FIELD 'm' +REG  ENDOF
    mx   OF  14 1 FIELD 'm' +REG  ENDOF
    my   OF  6 1 FIELD 2 + 'm' +REG  ENDOF
    i8   OF  IMM8 +NUM  ENDOF
    i8x2 OF  IMM8 2* +NUM  ENDOF
    i8x4 OF  IMM8 4 * +NUM  ENDOF
    s8   OF  IMM8 8 SIGNED +NUM  ENDOF
    s8x256 OF  IMM8 8 SIGNED 256 * +NUM  ENDOF
    s12  OF  IMM8  S# 8 LSHIFT OR  12 SIGNED +NUM  ENDOF
    r4x4 OF  R# 4 * +NUM  ENDOF
    e4   OF  R# 16 - 4 * +NUM  ENDOF
    ai4  OF  T# ?DUP 0= IF -1 THEN +NUM  ENDOF
    i7   OF  4 3 FIELD 4 LSHIFT R# OR  DUP 95 > IF 128 - THEN +NUM  ENDOF
    sa32 OF  32  T# 20 1 FIELD 4 LSHIFT OR  - +NUM  ENDOF
    sas  OF  S# 20 1 FIELD 4 LSHIFT OR +NUM  ENDOF
    s4   OF  S# +NUM  ENDOF
    t4   OF  T# +NUM  ENDOF
    t7   OF  T# 7 + +NUM  ENDOF
    st5  OF  S# 4 1 FIELD 4 LSHIFT OR +NUM  ENDOF
    xsh  OF  S# 16 1 FIELD 4 LSHIFT OR +NUM  ENDOF
    xm   OF  20 4 FIELD 1+ +NUM  ENDOF
    t4s  OF  T# 4 SIGNED +NUM  ENDOF
    e12  OF  12 12 FIELD 8 * +NUM  ENDOF
    c4   OF  R# CELLS B4CONST + @ +NUM  ENDOF
    c4u  OF  R# CELLS B4CONSTU + @ +NUM  ENDOF
    bbi  OF  T# 12 1 FIELD 4 LSHIFT OR +NUM  ENDOF
    j8   OF  IMM8 8 SIGNED +TARGET  ENDOF
    j12  OF  12 12 FIELD 12 SIGNED +TARGET  ENDOF
    j18  OF  6 18 FIELD 18 SIGNED +TARGET  ENDOF
    c18  OF  PC @ -4 AND 4 +  6 18 FIELD 18 SIGNED 4 * +  0 +HEX  ENDOF
    lr   OF  PC @ 3 + -4 AND  8 16 FIELD $FFFF0000 OR 4 * +  0 +HEX  ENDOF
    lp   OF  IMM8 +TARGET  ENDOF
    j6   OF  R# 4 2 FIELD 4 LSHIFT OR +TARGET  ENDOF
    x4   OF  20 4 FIELD +NUM  ENDOF
    x1   OF  20 1 FIELD +NUM  ENDOF
    x2   OF  20 2 FIELD +NUM  ENDOF
    x22  OF  22 1 FIELD +NUM  ENDOF
    y2   OF  13 2 FIELD +NUM  ENDOF
    y1   OF  12 1 FIELD +NUM  ENDOF
    z1   OF  8 1 FIELD +NUM  ENDOF
    t5   OF  4 5 FIELD +NUM  ENDOF
  ENDCASE ;

\ ==== The instruction ======================================================
\ A special register goes into the name (rsr.ps a2). A number without a
\ name in the table comes after the other operands (rsr a2, 99).

VARIABLE EXTRA                       \ the number of a register without a name, or -1
: +SPECIAL ( n access table -- )   \ ".name", or the number for later
  >R  OVER SWAP R> REG-NAME IF  S" ." +T  +T  DROP  ELSE  EXTRA !  THEN ;
: SUFFIX? ( k -- f )   .rsr .wur 1+ WITHIN ;
: +SUFFIX ( k -- )
  CASE
    .rsr OF  8 8 FIELD 1 SRS +SPECIAL  ENDOF
    .wsr OF  8 8 FIELD 2 SRS +SPECIAL  ENDOF
    .xsr OF  8 8 FIELD 4 SRS +SPECIAL  ENDOF
    .rur OF  4 8 FIELD 1 URS +SPECIAL  ENDOF
    .wur OF  8 8 FIELD 2 URS +SPECIAL  ENDOF
  ENDCASE ;
VARIABLE FIRST
: +SEP ( -- )   FIRST @ IF  S"  " +T  FALSE FIRST !  ELSE  S" , " +T  THEN ;
: +INSN ( e -- )
  DUP NAME +T  -1 EXTRA !
  6 0 DO  DUP I KIND  DUP SUFFIX? IF +SUFFIX ELSE DROP THEN  LOOP
  TRUE FIRST !
  6 0 DO  DUP I KIND  ?DUP IF  DUP SUFFIX? IF DROP ELSE  +SEP +OPERAND  THEN  THEN  LOOP  DROP
  EXTRA @ 0< 0= IF  +SEP  EXTRA @ +NUM  THEN ;
: UNKNOWN ( -- )   \ .byte and the bytes of the instruction
  S" .byte " +T
  ILEN @ 0 DO  I IF  S" , " +T  THEN  INSN @ I 8 * RSHIFT 255 AND 0 +HEX  LOOP ;
: FIND-INSN ( -- e | 0 )   \ the entry of the instruction in INSN
  INSN @ 15 AND  DUP 4 = S3? @ AND IF  DROP 0 EXIT  THEN   \ the ESP32-S3: no MAC16
  ILEN @ 4 = IF  DROP 0 EXIT  THEN
  CELLS LISTS + @
  BEGIN  DUP WHILE
    INSN @ OVER MASK AND  OVER MATCH = IF EXIT THEN
    @
  REPEAT ;
: XT-DECODE ( a -- a' )
  READ  TXT-START
  FIND-INSN ?DUP IF +INSN ELSE UNKNOWN THEN
  PC @ ILEN @ + ;

PUBLIC
: XT-ESP32 ( -- )     FALSE S3? !  ['] XT-DECODE IS DECODE  ['] (.BYTES) IS .CODE  8 ADDR-DIGITS ! ;
: XT-ESP32S3 ( -- )   TRUE S3? !   ['] XT-DECODE IS DECODE  ['] (.BYTES) IS .CODE  8 ADDR-DIGITS ! ;

END-PACKAGE
XT-ESP32
