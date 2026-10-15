\ disx86.fs - package DISX86: a disassembler for the x86 (16, 32 and 64 bits)
\
\ It turns the bytes of an instruction into its text, for DISASM of the
\ package MONITOR. It knows the instructions of the 8086 and those that
\ the 80186 to the x86-64 added for whole numbers and for the system
\ (PUSHA, ENTER, MOVZX, BT, LGDT, MOV CR0, SYSCALL, CMPXCHG16B ...), with
\ the prefixes 66h (another size of operands), 67h (another size of
\ addresses) and REX (64-bit mode), in each of the three modes of the
\ CPU: real mode (and 16-bit code), 32-bit and 64-bit code. The
\ coprocessor (8087: D8h to DFh) shows as ESC with its number, as the
\ manual of the 8086 writes it; a byte that starts no instruction (also
\ those of SSE and AVX) shows as db.
\
\ The text is the Intel form, as clang and llvm-objdump write it: lower
\ case, numbers as 0x and hex digits, "word ptr" for the size of memory,
\ the destination first:
\
\   0100  b8 34 12                  mov ax, 0x1234
\   0103  8b 46 fe                  mov ax, word ptr [bp - 0x2]
\   0106  26 c6 07 ff               mov byte ptr es:[bx], 0xff
\   010a  ea 5b e0 00 f0            ljmp 0xf000, 0xe05b
\
\ A number is without sign in the size of its field; only a number that
\ the CPU extends with its sign (a byte of 83h, 6Ah, 6Bh; 32 bits for an
\ operand of 64 bits) and a displacement have a sign. A jump shows its
\ target; an address relative to RIP shows it after "#". The README of
\ the monitor lists where this differs from llvm-objdump.
\
\   X86-16 ( -- )   DECODE of MONITOR disassembles code of the 8086, or
\                   16-bit code of a later x86 (so after the load)
\   X86-32 ( -- )   32-bit code (the 386 in protected mode, Windows,
\                   Linux i386)
\   X86-64 ( -- )   64-bit code (x86-64); needs cells of 32 bits
\
\ Load after MONITOR:  ALSO FILES  INCLUDE asm/monitor/disx86.fs

PACKAGE DISX86  ALSO MONITOR   \ lib/package.fs

\ ==== Reading the bytes =================================================

VARIABLE DA                          \ the address of the next byte
: BYTE+ ( -- c )   DA @ DIS-C@  1 DA +! ;
: WORD+ ( -- u )   BYTE+  BYTE+ 8 LSHIFT OR ;
: >UD ( lo hi -- ud )   CELL 2 = IF EXIT THEN  16 LSHIFT OR 0 ;     \ 32 bits as a double
: >D ( lo hi -- d )     CELL 2 = IF EXIT THEN  16 LSHIFT OR S>D ;   \ the same, with the sign
: DWORD+ ( -- ud )    WORD+ WORD+ >UD ;
: SDWORD+ ( -- d )    WORD+ WORD+ >D ;
: QWORD+ ( -- ud )    DWORD+ DROP  DWORD+ DROP ;    \ 64 bits: with cells of 32 bits
: SX8 ( c -- n )    DUP $80 AND IF  $FF INVERT OR  THEN ;
: SX16 ( u -- n )   DUP $8000 AND IF  $FFFF INVERT OR  THEN ;
: D+ ( d1 d2 -- d )   ROT + >R  UM+  R> + ;

\ ==== The state of one instruction ======================================

VARIABLE MODE   16 MODE !            \ 16, 32 or 64: the code of the CPU
VARIABLE OSIZE   VARIABLE ASIZE      \ 16, 32 or 64: the size of operands, of addresses
VARIABLE O16   VARIABLE A16          \ the prefix 66h, 67h came
VARIABLE REX                         \ the prefix REX (40h to 4Fh, 64-bit code), else 0
VARIABLE SEG                         \ a segment prefix: 0 none, else 1 + its register
VARIABLE REPX                        \ $F2 or $F3, else 0
VARIABLE LOCKX                       \ true: the prefix LOCK
VARIABLE OPC                         \ the opcode
VARIABLE MRM   VARIABLE HAVE-MRM     \ the byte ModR/M, and whether it came
VARIABLE RIP?                        \ true: an address relative to RIP came
VARIABLE RIP-LO   VARIABLE RIP-HI    \ its displacement, a double
: MODF ( -- n )   MRM @ 6 RSHIFT ;
: REGF ( -- n )   MRM @ 3 RSHIFT 7 AND ;
: RMF ( -- n )    MRM @ 7 AND ;
: MODRM ( -- )   HAVE-MRM @ IF EXIT THEN  BYTE+ MRM !  HAVE-MRM ON! ;
: REX? ( bit -- f )   REX @ AND 0<> ;
: +R ( n -- n' )   4 REX? IF 8 + THEN ;     \ REX.R extends the field reg
: +X ( n -- n' )   2 REX? IF 8 + THEN ;     \ REX.X the index of SIB
: +B ( n -- n' )   1 REX? IF 8 + THEN ;     \ REX.B r/m, the base, the register in the opcode
: X64? ( -- f )   MODE @ 64 = ;

\ ==== Names of instructions =============================================
\ The names, as counted strings one after another; an entry of a table
\ holds the number of its name (1, 2 ...). A name with "/" has a form for
\ each size of operands: 16, 32, 64 bits (cbw/cwde/cdqe; the last form
\ for the sizes it lacks), a name with ":" one for each size of
\ addresses (jcxz:jecxz:jrcxz).

1900 CONSTANT /MNEMS
CREATE MNEMS  /MNEMS ALLOT   VARIABLE MN-END   MNEMS MN-END !
VARIABLE NA   VARIABLE NU
: MN# ( a u -- n )   \ the number of the name a u; a new name goes to the end
  NU !  NA !  1 MNEMS
  BEGIN  DUP MN-END @ U< WHILE
    DUP COUNT  DUP NU @ = IF  NA @ SWAP SAME? IF  DROP EXIT  THEN  ELSE  2DROP  THEN
    COUNT +  SWAP 1+ SWAP
  REPEAT
  NU @ OVER C!  NA @ OVER 1+ NU @ MOVE  NU @ + 1+ MN-END !
  DUP 250 > ABORT" DISX86: too many names"
  MN-END @ MNEMS /MNEMS + U> ABORT" DISX86: MNEMS is full" ;
: MN$ ( n -- a u )   MNEMS SWAP 1- 0 ?DO  COUNT +  LOOP  COUNT ;

\ ==== Kinds of operands =================================================
\ Each kind of operand has a number; its name is a word here, so the
\ tables below can name it. E: a register or memory, from ModR/M; G:
\ the register in the field reg; S: a segment register in the field
\ reg; I: an immediate number (Ibs: a byte that the CPU extends with its
\ sign; IbA: the number of AAM and AAD, not shown when it is 10); J: a
\ jump; A: a far address; O: an address in the instruction; M: only
\ memory, without a size (Mb Md Mq Mo: with a size); Z: the register in
\ the low 3 bits of the opcode; X and Y: the operands of the string
\ instructions; C, D, R: a control register, a debug register, a
\ register of the CPU (MOV CR0); Ewv: memory of 16 bits, or a register
\ of the size of operands (MOV AX,ES); Rv: only a register.
\ b byte, w word, d dword, v word, dword or qword (the size of operands).

: KINDS ( n "name" ... -- )   0 ?DO  DUP CONSTANT 1+  LOOP  DROP ;
1 7 KINDS  Eb Ev Ew Gb Gv Gw Sw
8 9 KINDS  Ib Ibs Iw Iv IbA Jb Jv Ap ONE
17 2 KINDS M Mp
19 6 KINDS Ob Ov AL CL DX eAX
25 6 KINDS Zb Zv Xb Xv Yb Yv
31 3 KINDS Cd Dd Rd
34 6 KINDS ES CS SS DS FS GS
40 8 KINDS Ewv Ed Mb Md Mq Mo Mv Rv

\ ==== The tables ========================================================
\ An entry is 4 bytes: the number of the name, then three kinds of
\ operands (0: none). Name 0: no instruction. Name 254: a group; the
\ second byte is its number, and the field reg of ModR/M chooses one of
\ its 8 entries. Name 253: the coprocessor.

254 CONSTANT GROUP   253 CONSTANT ESC-87   252 CONSTANT SYS-0F01
CREATE OP1  1024 ALLOT   OP1 1024 0 FILL      \ the opcodes of one byte
CREATE OP2  1024 ALLOT   OP2 1024 0 FILL      \ the opcodes after 0Fh
27 CONSTANT #GROUPS
CREATE GROUPS  #GROUPS 32 * ALLOT   GROUPS #GROUPS 32 * 0 FILL
VARIABLE TABLE   OP1 TABLE !
: OP1? ( -- f )   TABLE @ OP1 = ;     \ the opcode has one byte (not after 0Fh)
: ENTRY ( op -- e )   4 * TABLE @ + ;
: GENTRY ( g reg -- e )   SWAP 8 * + 4 * GROUPS + ;

\ The words that fill the tables when this file loads. The operands
\ come as one word, separated by commas (Ev,Gv), or "-" for none.
: KIND# ( a u -- k )   FIND-NAME DUP 0= ABORT" DISX86: no such kind of operand"  NAME>XT EXECUTE ;
VARIABLE THE-CHAR
: TO-CHAR ( a u c -- n )   \ the characters before the first c (u if there is none)
  THE-CHAR !  DUP >R
  0 ?DO  DUP I + C@ THE-CHAR @ = IF  DROP I  UNLOOP  R> DROP  EXIT  THEN  LOOP  DROP R> ;
: TO-COMMA ( a u -- n )   ',' TO-CHAR ;
: /SKIP ( a u n -- a' u' )   ROT OVER +  ROT ROT - ;
VARIABLE DST
: OPS! ( e a u -- )   \ the kinds of the operand text a u into the entry e
  ROT 1+ DST !
  DUP 1 = IF  OVER C@ '-' = IF  2DROP EXIT  THEN  THEN
  BEGIN  DUP 0 > WHILE
    2DUP TO-COMMA >R  OVER R@ KIND#  DST @ C!  1 DST +!
    R> 1+ /SKIP
  REPEAT  2DROP ;
: NAME# ( a u -- n )   \ the number of a name; "-" is no instruction: 0
  DUP 1 = IF  OVER C@ '-' = IF  2DROP 0 EXIT  THEN  THEN  MN# ;
VARIABLE OT-A   VARIABLE OT-U   VARIABLE MN-V
: OP: ( op "name" "ops" -- )   ENTRY  PARSE-NAME NAME# OVER C!  PARSE-NAME OPS! ;
: SAME: ( op n "name" "ops" -- )   \ n opcodes from op, with the same name and operands
  PARSE-NAME NAME# MN-V !  PARSE-NAME OT-U ! OT-A !
  0 ?DO  DUP I + ENTRY  MN-V @ OVER C!  OT-A @ OT-U @ OPS!  LOOP  DROP ;
: RUN: ( op n "ops" "name" ... -- )   \ n opcodes from op, with n names and the same operands
  PARSE-NAME OT-U ! OT-A !
  0 ?DO  DUP I + ENTRY  PARSE-NAME NAME# OVER C!  OT-A @ OT-U @ OPS!  LOOP  DROP ;
: NAMED ( op -- e )   ENTRY  MN-V @ OVER C! ;
: ALU: ( op "name" -- )   \ the 6 forms of ADD, OR ... CMP from op on
  PARSE-NAME NAME# MN-V !
  DUP NAMED S" Eb,Gb" OPS!   DUP 1+ NAMED S" Ev,Gv" OPS!   DUP 2 + NAMED S" Gb,Eb" OPS!
  DUP 3 + NAMED S" Gv,Ev" OPS!   DUP 4 + NAMED S" AL,Ib" OPS!   5 + NAMED S" eAX,Iv" OPS! ;
: GROUP-AT ( op g -- )   SWAP ENTRY  GROUP OVER C!  1+ C! ;
: GROUP: ( g "ops" "name0" ... "name7" -- )   \ the 8 entries of group g, the same operands
  PARSE-NAME OT-U ! OT-A !
  8 0 DO  DUP I GENTRY  PARSE-NAME NAME# OVER C!  OT-A @ OT-U @ OPS!  LOOP  DROP ;
: GR: ( g reg "name" "ops" -- )   GENTRY  PARSE-NAME NAME# OVER C!  PARSE-NAME OPS! ;
: ALONE: ( "name" "ops" -- )   \ an entry outside the tables: CREATE name, then the entry
  CREATE  HERE 4 ALLOT  DUP 4 0 FILL  PARSE-NAME NAME# OVER C!  PARSE-NAME OPS! ;

\ ---- The opcodes of one byte (Intel SDM, volume 2, table A-2) ---------

$00 ALU: add   $08 ALU: or    $10 ALU: adc   $18 ALU: sbb
$20 ALU: and   $28 ALU: sub   $30 ALU: xor   $38 ALU: cmp
$06 OP: push ES   $07 OP: pop ES   $0E OP: push CS
$16 OP: push SS   $17 OP: pop SS   $1E OP: push DS   $1F OP: pop DS
$27 OP: daa -   $2F OP: das -   $37 OP: aaa -   $3F OP: aas -
$40 8 SAME: inc Zv   $48 8 SAME: dec Zv   $50 8 SAME: push Zv   $58 8 SAME: pop Zv
$60 OP: pusha/pushad -   $61 OP: popa/popad -   $62 OP: bound Gv,Mv   $63 OP: arpl Ew,Gw
$68 OP: push Iv   $69 OP: imul Gv,Ev,Iv   $6A OP: push Ibs   $6B OP: imul Gv,Ev,Ibs
$6C OP: insb Yb,DX   $6D OP: insw/insd Yv,DX   $6E OP: outsb DX,Xb   $6F OP: outsw/outsd DX,Xv
$70 16 RUN: Jb  jo jno jb jae je jne jbe ja js jns jp jnp jl jge jle jg
$84 OP: test Eb,Gb   $85 OP: test Ev,Gv   $86 OP: xchg Eb,Gb   $87 OP: xchg Ev,Gv
$88 OP: mov Eb,Gb    $89 OP: mov Ev,Gv    $8A OP: mov Gb,Eb    $8B OP: mov Gv,Ev
$8C OP: mov Ewv,Sw   $8D OP: lea Gv,M     $8E OP: mov Sw,Ewv
$90 OP: nop -   $91 7 SAME: xchg eAX,Zv
$98 OP: cbw/cwde/cdqe -   $99 OP: cwd/cdq/cqo -   $9A OP: lcall Ap   $9B OP: wait -
$9C OP: pushf/pushfd/pushfq -   $9D OP: popf/popfd/popfq -   $9E OP: sahf -   $9F OP: lahf -
$A0 OP: mov AL,Ob   $A1 OP: mov eAX,Ov   $A2 OP: mov Ob,AL   $A3 OP: mov Ov,eAX
$A4 OP: movsb Yb,Xb   $A5 OP: movsw/movsd/movsq Yv,Xv   $A6 OP: cmpsb Xb,Yb   $A7 OP: cmpsw/cmpsd/cmpsq Xv,Yv
$A8 OP: test AL,Ib    $A9 OP: test eAX,Iv     $AA OP: stosb Yb,AL   $AB OP: stosw/stosd/stosq Yv,eAX
$AC OP: lodsb AL,Xb   $AD OP: lodsw/lodsd/lodsq eAX,Xv   $AE OP: scasb AL,Yb   $AF OP: scasw/scasd/scasq eAX,Yv
$B0 8 SAME: mov Zb,Ib   $B8 8 SAME: mov Zv,Iv
$C2 OP: ret Iw   $C3 OP: ret -   $C4 OP: les Gv,Mp   $C5 OP: lds Gv,Mp
$C8 OP: enter Iw,Ib   $C9 OP: leave -   $CA OP: retf/retf/retfq Iw   $CB OP: retf/retf/retfq -
$CC OP: int3 -   $CD OP: int Ib   $CE OP: into -   $CF OP: iret/iretd/iretq -
$D4 OP: aam IbA   $D5 OP: aad IbA   $D6 OP: salc -   $D7 OP: xlatb -
$E0 OP: loopne Jb   $E1 OP: loope Jb   $E2 OP: loop Jb   $E3 OP: jcxz:jecxz:jrcxz Jb
$E4 OP: in AL,Ib    $E5 OP: in eAX,Ib  $E6 OP: out Ib,AL  $E7 OP: out Ib,eAX
$E8 OP: call Jv     $E9 OP: jmp Jv     $EA OP: ljmp Ap    $EB OP: jmp Jb
$EC OP: in AL,DX    $ED OP: in eAX,DX  $EE OP: out DX,AL  $EF OP: out DX,eAX
$F1 OP: int1 -   $F4 OP: hlt -   $F5 OP: cmc -
$F8 OP: clc -   $F9 OP: stc -   $FA OP: cli -   $FB OP: sti -   $FC OP: cld -   $FD OP: std -
: ESCAPES ( -- )   8 0 DO  ESC-87  $D8 I + ENTRY C!  LOOP ;   ESCAPES

\ The groups: the field reg of ModR/M chooses the instruction
$80 1 GROUP-AT   $82 1 GROUP-AT   $81 2 GROUP-AT   $83 3 GROUP-AT   $8F 4 GROUP-AT
$C0 5 GROUP-AT   $C1 6 GROUP-AT   $D0 7 GROUP-AT   $D1 8 GROUP-AT   $D2 9 GROUP-AT
$D3 10 GROUP-AT  $C6 11 GROUP-AT  $C7 12 GROUP-AT  $F6 13 GROUP-AT  $F7 14 GROUP-AT
$FE 15 GROUP-AT  $FF 16 GROUP-AT
1 GROUP: Eb,Ib   add or adc sbb and sub xor cmp
2 GROUP: Ev,Iv   add or adc sbb and sub xor cmp
3 GROUP: Ev,Ibs  add or adc sbb and sub xor cmp
4 GROUP: Ev      pop - - - - - - -
5 GROUP: Eb,Ib   rol ror rcl rcr shl shr - sar
6 GROUP: Ev,Ib   rol ror rcl rcr shl shr - sar
7 GROUP: Eb,ONE  rol ror rcl rcr shl shr - sar
8 GROUP: Ev,ONE  rol ror rcl rcr shl shr - sar
9 GROUP: Eb,CL   rol ror rcl rcr shl shr - sar
10 GROUP: Ev,CL  rol ror rcl rcr shl shr - sar
11 GROUP: Eb,Ib  mov - - - - - - -
12 GROUP: Ev,Iv  mov - - - - - - -
13 GROUP: Eb     - - not neg mul imul div idiv    13 0 GR: test Eb,Ib
14 GROUP: Ev     - - not neg mul imul div idiv    14 0 GR: test Ev,Iv
15 GROUP: Eb     inc dec - - - - - -
16 GROUP: Ev     inc dec call - jmp - push -      16 3 GR: lcall Mp   16 5 GR: ljmp Mp

\ ---- The opcodes after 0Fh (table A-3) --------------------------------

OP2 TABLE !
$00 17 GROUP-AT   $01 18 GROUP-AT   $BA 19 GROUP-AT   $0D 20 GROUP-AT   $18 21 GROUP-AT
$AE 22 GROUP-AT   $C7 23 GROUP-AT   $1C 26 GROUP-AT
$02 OP: lar Gv,Ew   $03 OP: lsl Gv,Ew   $05 OP: syscall -   $06 OP: clts -
$07 OP: sysret/sysret/sysretq -   $08 OP: invd -   $09 OP: wbinvd -   $0B OP: ud2 -
$1F OP: nop Ev   $37 OP: getsec -
$20 OP: mov Rd,Cd   $21 OP: mov Rd,Dd   $22 OP: mov Cd,Rd   $23 OP: mov Dd,Rd
$30 OP: wrmsr -   $31 OP: rdtsc -   $32 OP: rdmsr -   $33 OP: rdpmc -
$34 OP: sysenter -   $35 OP: sysexit/sysexit/sysexitq -
$40 16 RUN: Gv,Ev  cmovo cmovno cmovb cmovae cmove cmovne cmovbe cmova cmovs cmovns cmovp cmovnp cmovl cmovge cmovle cmovg
$80 16 RUN: Jv  jo jno jb jae je jne jbe ja js jns jp jnp jl jge jle jg
$90 16 RUN: Eb  seto setno setb setae sete setne setbe seta sets setns setp setnp setl setge setle setg
$A0 OP: push FS   $A1 OP: pop FS   $A2 OP: cpuid -   $A3 OP: bt Ev,Gv
$A4 OP: shld Ev,Gv,Ib   $A5 OP: shld Ev,Gv,CL   $A8 OP: push GS   $A9 OP: pop GS
$AA OP: rsm -   $AB OP: bts Ev,Gv   $AC OP: shrd Ev,Gv,Ib   $AD OP: shrd Ev,Gv,CL   $AF OP: imul Gv,Ev
$B0 OP: cmpxchg Eb,Gb   $B1 OP: cmpxchg Ev,Gv   $B2 OP: lss Gv,Mp   $B3 OP: btr Ev,Gv
$B4 OP: lfs Gv,Mp   $B5 OP: lgs Gv,Mp   $B6 OP: movzx Gv,Eb   $B7 OP: movzx Gv,Ew   $B9 OP: ud1 Gv,Ev
$BB OP: btc Ev,Gv   $BC OP: bsf Gv,Ev   $BD OP: bsr Gv,Ev   $BE OP: movsx Gv,Eb   $BF OP: movsx Gv,Ew
$C0 OP: xadd Eb,Gb   $C1 OP: xadd Ev,Gv   $C8 8 SAME: bswap Zv
17 GROUP: Ew     sldt str lldt ltr verr verw - -    17 0 GR: sldt Ewv   17 1 GR: str Ewv
18 GROUP: M      sgdtw/sgdtd/sgdt sidtw/sidtd/sidt lgdtw/lgdtd/lgdt lidtw/lidtd/lidt - - - -
18 4 GR: smsw Ewv   18 6 GR: lmsw Ew   18 7 GR: invlpg Mb
19 GROUP: Ev,Ib  - - - - bt bts btr btc
20 GROUP: Mb     prefetch prefetchw prefetchwt1 - - - - -
21 GROUP: Mb     prefetchnta prefetcht0 prefetcht1 prefetcht2 prefetchrst2 - - -
22 GROUP: M      fxsave/fxsave/fxsave64 fxrstor/fxrstor/fxrstor64 - - xsave/xsave/xsave64 xrstor/xrstor/xrstor64 xsaveopt/xsaveopt/xsaveopt64 -
22 2 GR: ldmxcsr Md   22 3 GR: stmxcsr Md   22 7 GR: clflush Mb
23 GROUP: M      - - - xrstors/xrstors/xrstors64 xsavec/xsavec/xsavec64 xsaves/xsaves/xsaves64 - -
23 1 GR: cmpxchg8b Mq   23 6 GR: vmptrld Mq   23 7 GR: vmptrst Mq
24 GROUP: Rv     - - - - - - rdrand rdseed      \ 0Fh C7h with a register
25 GROUP: -      - - - - - lfence mfence sfence \ 0Fh AEh with a register
26 GROUP: Mb     cldemote - - - - - - -
OP1 TABLE !

\ Entries for the forms that depend on a prefix, the mode or the byte ModR/M
ALONE: E-PAUSE pause -   ALONE: E-POPCNT popcnt Gv,Ev   ALONE: E-TZCNT tzcnt Gv,Ev
ALONE: E-LZCNT lzcnt Gv,Ev   ALONE: E-ENDBR64 endbr64 -   ALONE: E-ENDBR32 endbr32 -
ALONE: E-MOVSXD movsxd Gv,Ed   ALONE: E-XCHG8 xchg eAX,Zv   ALONE: E-CX16 cmpxchg16b Mo
ALONE: E-WBNOINVD wbnoinvd -   ALONE: E-XABORT xabort Ib   ALONE: E-XBEGIN xbegin Jv
ALONE: E-CLWB clwb Mb   ALONE: E-VMCLEAR vmclear Mq
ALONE: E-VMXON vmxon Mq   ALONE: E-RDPID rdpid Rv
ALONE: E-PREFETCHIT0 prefetchit0 Mb   ALONE: E-PREFETCHIT1 prefetchit1 Mb
ALONE: E-RDFSBASE rdfsbase Rv   ALONE: E-RDGSBASE rdgsbase Rv
ALONE: E-WRFSBASE wrfsbase Rv   ALONE: E-WRGSBASE wrgsbase Rv
CREATE E-0F01  SYS-0F01 C, 0 C, 0 C, 0 C,   \ 0Fh 01h with a register: its name from 0F01-NAMES
ALONE: E-MOVABS movabs -   \ only its name: B8h to BFh, A0h to A3h with 64 bits

\ 0Fh 01h with a register (ModR/M C0h to FFh, but SMSW and LMSW): the
\ whole byte ModR/M chooses the instruction; the names as llvm-objdump
\ knows them (VMX, SVM, SGX and others of the system)
12 CONSTANT /0F01                    \ a counted string of up to 11 characters for each byte
CREATE 0F01-NAMES  64 /0F01 * ALLOT   0F01-NAMES 64 /0F01 * 0 FILL
: 0F01-NAME ( modrm -- a )   $C0 - /0F01 * 0F01-NAMES + ;
: 0F01: ( "modrm" "name" ... -- )   \ pairs up to the end of the line
  BEGIN  PARSE-NAME DUP WHILE
    NUMBER? 0= ABORT" DISX86: not a number"  0F01-NAME  PARSE-NAME 11 MIN  ROT 2DUP C!  1+ SWAP MOVE
  REPEAT  2DROP ;
HEX
0F01: C0 enclv C1 vmcall C2 vmlaunch C3 vmresume C4 vmxoff C5 pconfig C6 wrmsrns
0F01: C8 monitor C9 mwait CA clac CB stac CF encls D0 xgetbv D1 xsetbv D4 vmfunc
0F01: D5 xend D6 xtest D7 enclu D8 vmrun D9 vmmcall DA vmload DB vmsave DC stgi
0F01: DD clgi DE skinit DF invlpga E8 serialize EE rdpkru EF wrpkru F8 swapgs
0F01: F9 rdtscp FA monitorx FB mwaitx FC clzero FD rdpru FE invlpgb FF tlbsync
DECIMAL

\ The opcodes of one byte that 64-bit code does not have, and those that
\ take 64-bit operands there unless 66h comes (a bit each)
CREATE INV64  32 ALLOT   INV64 32 0 FILL
CREATE D64    64 ALLOT   D64 64 0 FILL         \ 256 bits for one byte, 256 after 0Fh
: BIT! ( n a -- )   OVER 3 RSHIFT +  1 ROT 7 AND LSHIFT  OVER C@ OR  SWAP C! ;
: BIT@ ( n a -- f )   OVER 3 RSHIFT + C@  SWAP 7 AND RSHIFT 1 AND 0<> ;
: BITS: ( a "op" ... -- )   \ the opcodes up to the end of the line
  BEGIN  PARSE-NAME DUP WHILE  NUMBER? 0= ABORT" DISX86: not a number"  OVER BIT!  REPEAT  2DROP DROP ;
HEX
INV64 BITS: 06 07 0E 16 17 1E 1F 27 2F 37 3F 60 61 62 82 9A C4 C5 CE D4 D5 D6 EA
D64 BITS: 50 51 52 53 54 55 56 57 58 59 5A 5B 5C 5D 5E 5F 68 6A 8F 9C 9D C2 C3 C9 E8 E9 EB
D64 BITS: 70 71 72 73 74 75 76 77 78 79 7A 7B 7C 7D 7E 7F E0 E1 E2 E3
D64 BITS: 180 181 182 183 184 185 186 187 188 189 18A 18B 18C 18D 18E 18F 1A0 1A1 1A8 1A9
DECIMAL

\ ==== The text of the operands ===========================================

: STR, ( a u -- )   \ a counted string, without alignment
  DUP C,  0 ?DO  DUP I + C@ C,  LOOP  DROP ;
: +NTH ( n a -- )   \ the n-th counted string from a
  SWAP 0 ?DO  COUNT +  LOOP  COUNT +T ;
CREATE R8-NAMES   \ the registers of 8 bits: without REX, with REX
  S" al" STR, S" cl" STR, S" dl" STR, S" bl" STR, S" ah" STR, S" ch" STR, S" dh" STR, S" bh" STR,
  S" spl" STR, S" bpl" STR, S" sil" STR, S" dil" STR,  ALIGN
: +DEC ( n -- )   BASE @ >R  DECIMAL  0 <# #S #> +T  R> BASE ! ;
: +REG ( n size -- )   \ the name of register n (0 to 15) of size 8, 16, 32 or 64
  OVER 7 > IF  S" r" +T  SWAP +DEC                   \ r8 ... r15
    DUP 8 = IF DROP S" b" +T EXIT THEN  DUP 16 = IF DROP S" w" +T EXIT THEN
    32 = IF S" d" +T THEN  EXIT
  THEN
  DUP 8 = IF  DROP  DUP 4 8 WITHIN  REX @ 0<> AND IF 4 + THEN  R8-NAMES +NTH  EXIT  THEN
  DUP 32 = IF  S" e" +T  THEN  64 = IF  S" r" +T  THEN
  2* S" axcxdxbxspbpsidi" DROP + 2 +T ;
: +SREG ( n -- )   2* S" escsssdsfsgs????" DROP + 2 +T ;
: +PTR ( size -- )   \ the size of memory: byte ptr ... xmmword ptr; 0: none
  CASE
    8 OF  S" byte ptr " +T  ENDOF      16 OF  S" word ptr " +T  ENDOF
    32 OF  S" dword ptr " +T  ENDOF    64 OF  S" qword ptr " +T  ENDOF
    128 OF  S" xmmword ptr " +T  ENDOF
  ENDCASE ;
: +SEG ( -- )   SEG @ ?DUP IF  1- +SREG  S" :" +T  THEN ;
: +DISP ( d -- )   \ a displacement: " + 0x4", " - 0x2"; nothing for 0
  2DUP OR 0= IF  2DROP EXIT  THEN
  DUP 0< IF  S"  - " +T  DNEGATE  ELSE  S"  + " +T  THEN  +HEX ;
CREATE BASES16   \ the registers of ModR/M with 16-bit addresses, for r/m 0 to 7
  S" bx + si" STR,  S" bx + di" STR,  S" bp + si" STR,  S" bp + di" STR,
  S" si" STR,  S" di" STR,  S" bp" STR,  S" bx" STR,  ALIGN
: +ADDR16 ( -- )
  MODF 0= RMF 6 = AND IF  WORD+ 0 +HEX EXIT  THEN
  RMF BASES16 +NTH
  MODF 1 = IF  BYTE+ SX8 S>D +DISP  THEN
  MODF 2 = IF  WORD+ SX16 S>D +DISP  THEN ;
: +INDEX ( sib -- )   \ scale*register; nothing for index 4 without REX.X (no index)
  DUP 3 RSHIFT 7 AND +X  DUP 4 = IF  2DROP EXIT  THEN
  SWAP 6 RSHIFT  1 SWAP LSHIFT '0' + +C  S" *" +T  ASIZE @ +REG ;
: +INDEX1 ( sib -- )   \ the same, without "1*"
  DUP 6 RSHIFT IF  +INDEX EXIT  THEN
  3 RSHIFT 7 AND +X  DUP 4 = IF  DROP EXIT  THEN  ASIZE @ +REG ;
: INDEX? ( sib -- f )   3 RSHIFT 7 AND +X 4 <> ;
: +SIB ( -- )   \ the address after the byte SIB
  BYTE+  DUP 7 AND 5 =  MODF 0= AND IF               \ no base: index and 32 bits
    DUP INDEX? 0= IF  DROP DWORD+ +HEX EXIT  THEN
    +INDEX  SDWORD+ +DISP EXIT
  THEN
  DUP 7 AND +B ASIZE @ +REG
  DUP INDEX? IF  S"  + " +T  THEN  +INDEX1
  MODF 1 = IF  BYTE+ SX8 S>D +DISP  THEN
  MODF 2 = IF  SDWORD+ +DISP  THEN ;
: +ADDR32 ( -- )   \ 32-bit and 64-bit addresses
  RMF 4 = IF  +SIB EXIT  THEN
  MODF 0= RMF 5 = AND IF
    X64? IF  ASIZE @ 64 = IF S" rip" ELSE S" eip" THEN +T
      SDWORD+  2DUP RIP-HI ! RIP-LO !  RIP? ON!  +DISP EXIT  THEN
    DWORD+ +HEX EXIT
  THEN
  RMF +B ASIZE @ +REG
  MODF 1 = IF  BYTE+ SX8 S>D +DISP  THEN
  MODF 2 = IF  SDWORD+ +DISP  THEN ;
: +MEM ( size -- )   +PTR +SEG  S" [" +T  ASIZE @ 16 = IF +ADDR16 ELSE +ADDR32 THEN  S" ]" +T ;
: +E ( size -- )   MODF 3 = IF  RMF +B SWAP +REG  ELSE  +MEM  THEN ;
: +MOFFS ( size -- )   \ the address in the instruction
  +PTR +SEG  S" [" +T
  ASIZE @ 16 = IF WORD+ 0 ELSE  ASIZE @ 32 = IF DWORD+ ELSE QWORD+ THEN  THEN
  +HEX  S" ]" +T ;
: +SI ( -- )   ASIZE @ 16 = IF S" si" EXIT THEN  ASIZE @ 32 = IF S" esi" EXIT THEN  S" rsi" ;
: +DI ( -- )   ASIZE @ 16 = IF S" di" EXIT THEN  ASIZE @ 32 = IF S" edi" EXIT THEN  S" rdi" ;
: +XSTR ( size -- )   +PTR +SEG  S" [" +T  +SI +T  S" ]" +T ;
: +YSTR ( size -- )   +PTR  S" es:[" +T  +DI +T  S" ]" +T ;
: +TARGET ( d -- )   \ the target of a jump: the address after it + d
  DA @ 0 D+  X64? IF  +HEX EXIT  THEN
  OSIZE @ 16 = IF  DROP $FFFF AND 0  ELSE  CELL 4 = IF DROP 0 THEN  THEN  +HEX ;
: +IMM ( size -- )   16 = IF WORD+ 0 ELSE DWORD+ THEN  +HEX ;
: +IV ( -- )   \ an immediate of the size of operands; 64 bits: 32 with the sign, or 64 (MOV r64)
  OSIZE @ 64 = IF
    OPC @ $B8 $C0 WITHIN  TABLE @ OP1 = AND IF  QWORD+ +HEX  ELSE  SDWORD+ +SHEX  THEN  EXIT
  THEN  OSIZE @ +IMM ;
: REL16? ( -- f )   \ a jump with 16 bits: 16-bit operands; in 64-bit code 66h (as llvm-objdump)
  X64? 0= IF  OSIZE @ 16 =  EXIT  THEN
  O16 @ 0= IF  FALSE EXIT  THEN
  OP1? 0= IF  TRUE EXIT  THEN                       \ Jcc: also with REX.W
  OPC @ $E9 =  OPC @ $C7 = OR  8 REX? 0= AND ;                       \ JMP, XBEGIN: not with REX.W; CALL never
: +CR ( c -- )   +T  REGF +R +DEC ;

: +OPERAND ( k -- )
  CASE
    Eb  OF  8 +E  ENDOF
    Ev  OF  OSIZE @ +E  ENDOF
    Ew  OF  16 +E  ENDOF
    Gb  OF  REGF +R 8 +REG  ENDOF
    Gv  OF  REGF +R OSIZE @ +REG  ENDOF
    Gw  OF  REGF +R 16 +REG  ENDOF
    Sw  OF  REGF +SREG  ENDOF
    Ib  OF  BYTE+ 0 +HEX  ENDOF
    Ibs OF  BYTE+ SX8 S>D +SHEX  ENDOF
    Iw  OF  16 +IMM  ENDOF
    Iv  OF  +IV  ENDOF
    IbA OF  BYTE+ DUP 10 = IF DROP ELSE 0 +HEX THEN  ENDOF
    Jb  OF  BYTE+ SX8 S>D +TARGET  ENDOF
    Jv  OF  REL16? IF  WORD+ SX16 S>D  ELSE  SDWORD+  THEN  +TARGET  ENDOF
    Ap  OF  OSIZE @ 16 = IF  WORD+ 0  ELSE  DWORD+  THEN  WORD+ 0 +HEX  S" , " +T  +HEX  ENDOF
    ONE OF  S" 1" +T  ENDOF
    M   OF  0 +MEM  ENDOF
    Mp  OF  0 +MEM  ENDOF
    Ob  OF  8 +MOFFS  ENDOF
    Ov  OF  OSIZE @ +MOFFS  ENDOF
    AL  OF  S" al" +T  ENDOF
    CL  OF  S" cl" +T  ENDOF
    DX  OF  S" dx" +T  ENDOF
    eAX OF  0 OSIZE @ +REG  ENDOF
    Zb  OF  OPC @ 7 AND +B 8 +REG  ENDOF
    Zv  OF  OPC @ 7 AND +B OSIZE @ +REG  ENDOF
    Xb  OF  8 +XSTR  ENDOF
    Xv  OF  OSIZE @ +XSTR  ENDOF
    Yb  OF  8 +YSTR  ENDOF
    Yv  OF  OSIZE @ +YSTR  ENDOF
    Cd  OF  S" cr" +CR  ENDOF
    Dd  OF  S" dr" +CR  ENDOF
    Rd  OF  RMF +B  X64? IF 64 ELSE 32 THEN  +REG  ENDOF
    Ewv OF  MODF 3 = IF  OSIZE @ +E  ELSE  16 +E  THEN  ENDOF
    Ed  OF  32 +E  ENDOF
    Mb  OF  8 +MEM  ENDOF
    Md  OF  32 +MEM  ENDOF
    Mq  OF  64 +MEM  ENDOF
    Mo  OF  128 +MEM  ENDOF
    Mv  OF  OSIZE @ +MEM  ENDOF
    Rv  OF  OSIZE @ +E  ENDOF
    DUP ES - 0 6 WITHIN IF  DUP ES - +SREG  THEN     \ ES CS SS DS FS GS
  ENDCASE ;

\ ==== The instruction =====================================================

: PREFIX ( c -- f )   \ true: c is a prefix; it is noted
  X64? IF  DUP $F0 AND $40 = IF  REX !  TRUE EXIT  THEN  THEN
  CASE
    $26 OF  1 SEG !  TRUE  ENDOF     $2E OF  2 SEG !  TRUE  ENDOF
    $36 OF  3 SEG !  TRUE  ENDOF     $3E OF  4 SEG !  TRUE  ENDOF
    $64 OF  5 SEG !  TRUE  ENDOF     $65 OF  6 SEG !  TRUE  ENDOF
    $66 OF  O16 ON!  TRUE  ENDOF
    $67 OF  A16 ON!  TRUE  ENDOF
    $F0 OF  LOCKX ON!  TRUE  ENDOF
    $F2 OF  $F2 REPX !  TRUE  ENDOF  $F3 OF  $F3 REPX !  TRUE  ENDOF
    FALSE SWAP
  ENDCASE
  DUP IF  0 REX !  THEN ;             \ REX counts only right before the opcode
: PREFIXES ( -- op )   BEGIN  BYTE+ DUP PREFIX WHILE  DROP  REPEAT ;
: SIZES ( -- )   \ the sizes of operands and addresses from the mode and the prefixes
  X64? IF
    8 REX? IF 64 ELSE  O16 @ IF 16 ELSE 32 THEN  THEN  OSIZE !
    A16 @ IF 32 ELSE 64 THEN  ASIZE !  EXIT
  THEN
  MODE @  O16 @ IF  48 SWAP -  THEN  OSIZE !
  MODE @  A16 @ IF  48 SWAP -  THEN  ASIZE ! ;
: DEFAULT64 ( -- )   \ 64-bit code: an instruction that takes 64 bits unless 66h came
  8 REX? 0=  O16 @ AND IF 16 ELSE 64 THEN  OSIZE ! ;
: MRM? ( e -- f )   \ an operand of the entry e comes from ModR/M
  FALSE  4 1 DO  OVER I + C@  DUP Eb Sw 1+ WITHIN  OVER M Mp 1+ WITHIN OR
    OVER Cd Rd 1+ WITHIN OR  SWAP Ewv Mv 1+ WITHIN OR  OR  LOOP  NIP ;
: MEM-ONLY? ( e -- f )   \ an operand of the entry e must be memory
  FALSE  4 1 DO  OVER I + C@  DUP M Mp 1+ WITHIN  SWAP Mb Mv 1+ WITHIN OR  OR  LOOP  NIP ;
: SREG? ( e -- f )   \ an operand of the entry e is a segment register in the field reg
  FALSE  4 1 DO  OVER I + C@ Sw =  OR  LOOP  NIP ;
: REG-ONLY? ( e -- f )   \ an operand of the entry e must be a register
  FALSE  4 1 DO  OVER I + C@  DUP Rv =  SWAP Rd = OR  OR  LOOP  NIP ;
: FORM ( a u c size -- a' u' )   \ "x/y/z": x for 16, y for 32, z for 64; with two forms y for 32 and 64
  >R  THE-CHAR !  R>                                 ( a u size )
  >R  2DUP THE-CHAR @ TO-CHAR  2DUP = IF  DROP R> DROP EXIT  THEN   \ one form: as it is
  R@ 16 = IF  NIP R> DROP EXIT  THEN                 ( a u n )
  1+ /SKIP                                           \ after the first c
  2DUP THE-CHAR @ TO-CHAR  2DUP = IF  DROP R> DROP EXIT  THEN       \ two forms: the second
  R> 32 = IF  NIP EXIT  THEN  1+ /SKIP ;             \ three forms: the second or the third
: +MNEM ( n -- )
  DUP SYS-0F01 = IF  DROP MRM @ 0F01-NAME COUNT +T EXIT  THEN
  MN$  '/' OSIZE @ FORM  ':' ASIZE @ FORM  +T ;
: +OPERANDS ( e -- )
  4 1 DO  DUP I + C@ ?DUP IF  I 1 = IF S"  " ELSE S" , " THEN +T  +OPERAND  THEN  LOOP  DROP ;
VARIABLE START-A
: INVALID ( -- )   \ no instruction: its first byte as db
  TXT-START  S" db " +T  START-A @ DIS-C@ 0 +HEX  START-A @ 1+ DA ! ;
: +PREFIXES ( -- )   \ LOCK, and REP or REPNE before an opcode of one byte
  LOCKX @ IF  S" lock " +T  THEN
  OP1? IF  REPX @ $F3 = IF  S" rep " +T  THEN   REPX @ $F2 = IF  S" repne " +T  THEN  THEN ;
: +ESC ( -- )   \ the coprocessor: ESC with its number of 6 bits and the operand
  MODRM  +PREFIXES  S" esc " +T  OPC @ 7 AND 3 LSHIFT REGF OR 0 +HEX  S" , " +T  16 +E ;
: TRIM ( -- )   \ no spaces at the end of the text
  BEGIN  TXT C@ IF  TXT COUNT + 1- C@ 32 =  ELSE  FALSE  THEN  WHILE  TXT C@ 1- TXT C!  REPEAT ;

\ The forms that depend on a prefix, the mode or the byte ModR/M: the
\ entry for them, the entry e of the table as it is, or 0 (no instruction)
: 0F01-REG ( -- e | 0 )   \ 0Fh 01h with a register: the whole byte ModR/M chooses
  MRM @ 0F01-NAME C@  IF E-0F01 ELSE 0 THEN ;
: ENDBR ( e -- e' )   \ F3h 0Fh 1Eh FAh or FBh: ENDBR64, ENDBR32
  DA @ DIS-C@  DUP $FA = IF  2DROP  1 DA +!  0 REPX !  E-ENDBR64 EXIT  THEN
  $FB = IF  DROP  1 DA +!  0 REPX !  E-ENDBR32  THEN ;
: SPECIAL ( e -- e' | 0 )
  OP1? IF
    OPC @ $90 = IF
      REPX @ $F3 = IF  DROP  0 REPX !  REX @ IF E-XCHG8 ELSE E-PAUSE THEN  EXIT  THEN
      1 REX? IF  DROP E-XCHG8  THEN  EXIT
    THEN
    OPC @ $63 =  X64? AND IF  DROP E-MOVSXD  EXIT  THEN
    OPC @ $C6 $C8 WITHIN IF  DA @ DIS-C@ $F8 = IF  1 DA +!  DROP
      OPC @ $C6 = IF E-XABORT ELSE E-XBEGIN THEN  THEN  THEN  EXIT
  THEN
  REPX @ $F3 = IF
    OPC @ $B8 = IF  DROP  0 REPX !  E-POPCNT  EXIT  THEN
    OPC @ $BC = IF  DROP  0 REPX !  E-TZCNT   EXIT  THEN
    OPC @ $BD = IF  DROP  0 REPX !  E-LZCNT   EXIT  THEN
    OPC @ $1E = IF  ENDBR EXIT  THEN
    OPC @ $09 = IF  DROP  0 REPX !  E-WBNOINVD  EXIT  THEN
  THEN
  OPC @ $01 = IF  MODRM  MODF 3 = IF  REGF 4 <>  REGF 6 <> AND IF
    DROP  O16 @ REPX @ OR IF 0 ELSE 0F01-REG THEN  THEN  THEN  THEN ;
\ 0Fh AEh and 0Fh C7h with 66h, F2h or F3h: the forms below, and RDRAND
\ and RDSEED with 16 bits; any other is no instruction
: PREFIXED ( g -- e | 0 )   \ g: 22 or 25 (0Fh AEh), 23 or 24 (0Fh C7h)
  REPX @ $F3 = IF
    DUP 25 = REGF 4 < AND X64? AND IF  DROP REGF
      CASE  0 OF E-RDFSBASE ENDOF  1 OF E-RDGSBASE ENDOF  2 OF E-WRFSBASE ENDOF  E-WRGSBASE SWAP  ENDCASE  EXIT
    THEN
    DUP 23 = REGF 6 = AND IF  DROP E-VMXON EXIT  THEN
    DUP 24 = REGF 7 = AND IF  DROP  X64? IF 64 ELSE 32 THEN OSIZE !  E-RDPID EXIT  THEN
    DROP 0 EXIT
  THEN
  REPX @ IF  DROP 0 EXIT  THEN                       \ F2h
  DUP 22 = REGF 6 = AND IF  DROP E-CLWB EXIT  THEN    \ 66h
  DUP 23 = REGF 6 = AND IF  DROP E-VMCLEAR EXIT  THEN
  DUP 24 = IF  REGF GENTRY EXIT  THEN
  DROP 0 ;
: GROUP-ENTRY ( e -- e' | 0 )   \ the entry of the group that e names, by the field reg
  MODRM  1+ C@                                       ( g )
  DUP 22 = MODF 3 = AND IF  DROP 25  THEN            \ 0Fh AEh with a register: LFENCE ...
  DUP 23 = MODF 3 = AND IF  DROP 24  THEN            \ 0Fh C7h with a register: RDRAND ...
  DUP 22 26 WITHIN  O16 @ REPX @ OR AND IF  PREFIXED  EXIT  THEN
  DUP 23 =  REGF 1 = AND  OSIZE @ 64 = AND IF  DROP E-CX16 EXIT  THEN
  DUP 16 =  X64? AND IF  REGF 2 = REGF 4 = OR IF  64 OSIZE !  THEN  REGF 6 = IF  DEFAULT64  THEN  THEN
  DUP 18 =  X64? AND IF  REGF 4 < IF  64 OSIZE !  THEN  THEN     \ SGDT ... LIDT
  DUP 21 =  X64? AND  REGF 6 = AND IF  DROP E-PREFETCHIT1 EXIT  THEN
  DUP 21 =  X64? AND  REGF 7 = AND IF  DROP E-PREFETCHIT0 EXIT  THEN
  REGF GENTRY ;
: D64? ( -- f )   \ the opcode takes 64 bits in 64-bit code
  OPC @  OP1? 0= IF 256 + THEN  D64 BIT@ ;
: MOVABS? ( -- f )   \ MOV with 64 bits of immediate, or with an address in it: movabs
  X64? OP1? AND 0= IF FALSE EXIT THEN
  OPC @ $B8 $C0 WITHIN  OSIZE @ 64 = AND
  OPC @ $A0 $A4 WITHIN  ASIZE @ 64 = AND  OR ;
: +RIP ( -- )   \ an address relative to RIP (not EIP, without a segment): " # ", the address
  RIP? @ 0=  ASIZE @ 32 = OR  SEG @ OR IF EXIT THEN
  S"  # " +T  DA @ 0  RIP-LO @ RIP-HI @ D+  +HEX ;
: X86-DECODE ( a -- a' )
  DUP START-A !  DA !  TXT-START
  O16 OFF!  A16 OFF!  0 REX !  0 SEG !  0 REPX !  LOCKX OFF!
  HAVE-MRM OFF!  RIP? OFF!
  PREFIXES DUP OPC !  SIZES
  $0F = IF  OP2 TABLE !  BYTE+ OPC !  ELSE  OP1 TABLE !  THEN
  X64? OP1? AND IF  OPC @ INV64 BIT@ IF  INVALID  DA @ EXIT  THEN  THEN
  OPC @ ENTRY  SPECIAL                                 ( e | 0 )
  DUP 0= IF  DROP INVALID  DA @ EXIT  THEN
  DUP C@ GROUP = IF  GROUP-ENTRY  DUP 0= IF  DROP INVALID  DA @ EXIT  THEN  THEN
  DUP C@ ESC-87 = IF  DROP +ESC  DA @ EXIT  THEN
  DUP C@ 0= IF  DROP INVALID  DA @ EXIT  THEN
  DUP MRM? IF  MODRM  THEN
  DUP MEM-ONLY? IF  MODF 3 = IF  DROP INVALID  DA @ EXIT  THEN  THEN
  DUP REG-ONLY? IF  MODF 3 <> IF  DROP INVALID  DA @ EXIT  THEN  THEN
  DUP SREG? IF  REGF 5 > IF  DROP INVALID  DA @ EXIT  THEN  THEN
  X64? IF  D64? IF  DEFAULT64  THEN  THEN
  OSIZE @ 64 =  OP1? AND IF  OPC @ DUP $6D = OVER $6F = OR  SWAP $E4 $F0 WITHIN OR IF  32 OSIZE !  THEN  THEN
  +PREFIXES
  MOVABS? IF  E-MOVABS C@  ELSE  DUP C@  THEN  +MNEM
  +OPERANDS  +RIP  TRIM  DA @ ;

PUBLIC
: X86-16 ( -- )   16 MODE !  ['] X86-DECODE IS DECODE  ['] (.BYTES) IS .CODE  4 ADDR-DIGITS ! ;
: X86-32 ( -- )   32 MODE !  ['] X86-DECODE IS DECODE  ['] (.BYTES) IS .CODE  8 ADDR-DIGITS ! ;
: X86-64 ( -- )
  CELL 4 < ABORT" X86-64 needs cells of 32 bits"
  64 MODE !  ['] X86-DECODE IS DECODE  ['] (.BYTES) IS .CODE  8 ADDR-DIGITS ! ;

END-PACKAGE
X86-16
