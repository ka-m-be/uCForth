\ aarch64.fs - tests of the package AARCH64 (asm/aarch64.fs)
\
\ Load (in the folder ucf, with 32-bit cells), after the package FILES
\ and the tester:
\   ... -32 host/files.fs forth/test/tester.fs asm/test/aarch64.fs
\ The expected encodings come from the GNU assembler 2.40 (Debian 12,
\ arm64; MRS, MSR, WFE, ISB: 2.44, Debian 13; SXTW: clang 21, macOS),
\ once, and are kept here as data. The comment of each test is
\ the instruction for GNU.

ONLY FORTH DEFINITIONS  ALSO FILES
S" lib/text.fs" INCLUDED
S" asm/aarch64.fs" INCLUDED
ONLY FORTH DEFINITIONS  ALSO INTERNALS  ALSO TEXT  ALSO AARCH64
DECIMAL

: LAST ( -- x )   THERE 4 - A@ ;   \ the instruction that came last
0 START-CODE

\ ==== Single instructions ====
T{ W0 W1 W2 ADD, LAST -> $B020020 }T   \ add w0, w1, w2
T{ X28 X27 X3 ADD, LAST -> $8B03037C }T   \ add x28, x27, x3
T{ W3 W4 W5 ADDS, LAST -> $2B050083 }T   \ adds w3, w4, w5
T{ W20 W20 W1 SUB, LAST -> $4B010294 }T   \ sub w20, w20, w1
T{ X1 X2 X3 SUBS, LAST -> $EB030041 }T   \ subs x1, x2, x3
T{ W0 W1 W2 AND, LAST -> $A020020 }T   \ and w0, w1, w2
T{ X5 X6 X7 ORR, LAST -> $AA0700C5 }T   \ orr x5, x6, x7
T{ W9 W10 W11 ORN, LAST -> $2A2B0149 }T   \ orn w9, w10, w11
T{ W0 W0 W1 EOR, LAST -> $4A010000 }T   \ eor w0, w0, w1
T{ W2 W2 W0 BIC, LAST -> $0A200042 }T   \ bic w2, w2, w0
T{ W0 W1 W2 LSLV, LAST -> $1AC22020 }T   \ lsl w0, w1, w2
T{ W3 W4 W5 LSRV, LAST -> $1AC52483 }T   \ lsr w3, w4, w5
T{ W6 W7 W8 ASRV, LAST -> $1AC828E6 }T   \ asr w6, w7, w8
T{ X3 X0 X2 UDIV, LAST -> $9AC20803 }T   \ udiv x3, x0, x2
T{ W0 W1 W2 MUL, LAST -> $1B027C20 }T   \ mul w0, w1, w2
T{ W0 W1 MOV, LAST -> $2A0103E0 }T   \ mov w0, w1
T{ X2 X30 MOV, LAST -> $AA1E03E2 }T   \ mov x2, x30
T{ W0 W1 MVN, LAST -> $2A2103E0 }T   \ mvn w0, w1
T{ W5 W6 NEG, LAST -> $4B0603E5 }T   \ neg w5, w6
T{ W1 W2 CMP, LAST -> $6B02003F }T   \ cmp w1, w2
T{ X1 X2 CMP, LAST -> $EB02003F }T   \ cmp x1, x2
T{ X4 X3 X2 X0 MSUB, LAST -> $9B028064 }T   \ msub x4, x3, x2, x0
T{ X0 W1 W2 UMULL, LAST -> $9BA27C20 }T   \ umull x0, w1, w2
T{ W19 W22 4 ADDI, LAST -> $110012D3 }T   \ add w19, w22, #4
T{ X28 X28 4095 ADDI, LAST -> $913FFF9C }T   \ add x28, x28, #4095
T{ X0 XSP 16 ADDI, LAST -> $910043E0 }T   \ add x0, sp, #16
T{ W20 W20 4 SUBI, LAST -> $51001294 }T   \ sub w20, w20, #4
T{ W0 W1 7 SUBSI, LAST -> $71001C20 }T   \ subs w0, w1, #7
T{ W0 127 CMPI, LAST -> $7101FC1F }T   \ cmp w0, #127
T{ X3 0 CMPI, LAST -> $F100007F }T   \ cmp x3, #0
T{ W0 W1 1 LSLI, LAST -> $531F7820 }T   \ lsl w0, w1, #1
T{ X0 X1 32 LSLI, LAST -> $D3607C20 }T   \ lsl x0, x1, #32
T{ X1 X0 32 LSRI, LAST -> $D360FC01 }T   \ lsr x1, x0, #32
T{ W1 W0 8 LSRI, LAST -> $53087C01 }T   \ lsr w1, w0, #8
T{ W0 W0 31 ASRI, LAST -> $131F7C00 }T   \ asr w0, w0, #31
T{ W0 W0 1 ASRI, LAST -> $13017C00 }T   \ asr w0, w0, #1
T{ X9 W9 SXTW, LAST -> $93407D29 }T   \ sxtw x9, w9 (clang 21)
T{ X0 W1 SXTW, LAST -> $93407C20 }T   \ sxtw x0, w1 (clang 21)
T{ W0 $1234 0 MOVZ, LAST -> $52824680 }T   \ movz w0, #0x1234
T{ X28 $41 1 MOVZ, LAST -> $D2A0083C }T   \ movz x28, #0x41, lsl #16
T{ W0 $ABCD 1 MOVK, LAST -> $72B579A0 }T   \ movk w0, #0xabcd, lsl #16
T{ W0 37 0 MOVN, LAST -> $128004A0 }T   \ movn w0, #37
T{ W0 W1 W2 LO CSEL, LAST -> $1A823020 }T   \ csel w0, w1, w2, lo
T{ W3 HS CSET, LAST -> $1A9F37E3 }T   \ cset w3, hs
T{ W0 EQ CSET, LAST -> $1A9F17E0 }T   \ cset w0, eq
T{ W0 LT CSETM, LAST -> $5A9FA3E0 }T   \ csetm w0, lt
T{ X1 NE CSETM, LAST -> $DA9F03E1 }T   \ csetm x1, ne
T{ W0 X28 W1 LDR-UXTW, LAST -> $B8614B80 }T   \ ldr w0, [x28, w1, uxtw]
T{ W19 X28 W21 STR-UXTW, LAST -> $B8354B93 }T   \ str w19, [x28, w21, uxtw]
T{ W2 X28 W3 LDRB-UXTW, LAST -> $38634B82 }T   \ ldrb w2, [x28, w3, uxtw]
T{ W2 X28 W3 STRB-UXTW, LAST -> $38234B82 }T   \ strb w2, [x28, w3, uxtw]
T{ W1 X27 X0 LDR-LSL2, LAST -> $B8607B61 }T   \ ldr w1, [x27, x0, lsl #2]
T{ W0 X1 8 LDR-I, LAST -> $B9400820 }T   \ ldr w0, [x1, #8]
T{ X2 X26 16 LDRX-I, LAST -> $F9400B42 }T   \ ldr x2, [x26, #16]
T{ X9 X2 400 STRX-I, LAST -> $F900C849 }T   \ str x9, [x2, #400]
T{ X0 MPIDR-EL1 MRS, LAST -> $D53800A0 }T   \ mrs x0, mpidr_el1
T{ X1 CURRENTEL MRS, LAST -> $D5384241 }T   \ mrs x1, currentel
T{ WFE, LAST -> $D503205F }T   \ wfe
T{ VBAR-EL1 X1 MSR, LAST -> $D518C001 }T   \ msr vbar_el1, x1
T{ VBAR-EL2 X1 MSR, LAST -> $D51CC001 }T   \ msr vbar_el2, x1
T{ VBAR-EL3 X1 MSR, LAST -> $D51EC001 }T   \ msr vbar_el3, x1
T{ ISB, LAST -> $D5033FDF }T   \ isb
T{ XZR X9 24 STRX-I, LAST -> $F9000D3F }T   \ str xzr, [x9, #24]
T{ W0 X28 256 STR-I, LAST -> $B9010380 }T   \ str w0, [x28, #256]
T{ W0 X1 3 LDRB-I, LAST -> $39400C20 }T   \ ldrb w0, [x1, #3]
T{ W0 X1 0 STRB-I, LAST -> $39000020 }T   \ strb w0, [x1]
T{ X1 BR, LAST -> $D61F0020 }T   \ br x1
T{ X2 BLR, LAST -> $D63F0040 }T   \ blr x2
T{ RET, LAST -> $D65F03C0 }T   \ ret
T{ SVC, LAST -> $D4000001 }T   \ svc #0
T{ NOP, LAST -> $D503201F }T   \ nop

\ ==== Jumps and labels ====
\ start: b fwd / bl start / b.hi fwd / cbz w1, start / cbnz x2, fwd /
\ ldr w0, lit / adr x27, start / fwd: nop / lit: .word 0x12345678
\ Then: cmp w0, w1 / b.ne 1f / add w0, w0, #1 / 1: cbnz w2, 2f / nop /
\ b 3f / 2: nop / 3: 4: subs w0, w0, #1 / b.ne 4b / b 4b / cbz w3, 5f /
\ nop / 5: nop
0 START-CODE
FORWARD J-FWD   FORWARD J-LIT
LABEL J-START
J-FWD B,  J-START BL,  J-FWD HI B.COND,  W1 J-START CBZ,  X2 J-FWD CBNZ,
W0 J-LIT LDR-LIT,  X27 J-START ADR,
LABEL J-FWD  NOP,
LABEL J-LIT  $12345678 A,
W0 W1 CMP,  EQ IF,  W0 W0 1 ADDI,  THEN,
W2 0=IF,  NOP,  ELSE,  NOP,  THEN,
BEGIN,  W0 W0 1 SUBSI,  DUP EQ UNTIL,  AGAIN,
W3 0<>IF,  NOP,  THEN,  NOP,
END-CODE

CREATE J-EXPECT   $14000007 , $97FFFFFF , $540000A8 , $34FFFFA1 , $B5000062 , $18000060 ,
  $10FFFF5B , $D503201F , $12345678 , $6B01001F , $54000041 , $11000400 ,
  $35000062 , $D503201F , $14000002 , $D503201F , $71000400 , $54FFFFE1 ,
  $17FFFFFE , $34000043 , $D503201F , $D503201F ,
: J-DIFFS ( -- n )   \ the number of words that are not the same
  0  22 0 DO  CODE I CELLS + @  J-EXPECT I CELLS + @ <> IF 1+ THEN  LOOP ;
T{ CODE# @ -> 88 }T
T{ J-DIFFS -> 0 }T

\ ==== An error ====
\ The error is THROW -2 with a message; MSG" compares the message.
: MSG" ( "text<quote>" -- f )  34 PARSE  ERRMSG @ ERRMSG CELL+ @ S= ;
ORG @ CONSTANT SAVED-ORG
: E1 -1 ORG !  0 A, ;             \ no target address yet: as before the first START-CODE
T{ ' E1 CATCH MSG" no target address: START-CODE first" -> -2 TRUE }T
SAVED-ORG ORG !

S" aarch64" .RESULTS
