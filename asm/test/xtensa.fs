\ xtensa.fs - tests of the package XTENSA (asm/xtensa.fs)
\
\ Load (in the folder ucf, with 32-bit cells), after the package FILES
\ and the tester:
\   ... -32 host/files.fs forth/test/tester.fs asm/test/xtensa.fs
\ The expected encodings come from the GNU assembler 2.35.1 for the
\ ESP32 (esp-2021r2, PlatformIO), once, with "as --no-transform" and
\ "ld --no-relax -Ttext=0x40378000", and are kept here as data. The
\ comment of each test is the instruction for GNU. Xtensa code is little
\ endian: LAST and AT read 3 bytes (2 for the .N forms) as one number,
\ as objdump shows it.
\
\ Part 1 is one piece of code at $40378000, as in the GNU source: first
\ single instructions, then jumps to labels, then the test of each jump
\ at its offset. Part 2: the errors, each with its message. Part 3: the
\ loop option, MUL16U, MOVSP and a part of the FPU, from a GNU source of
\ its own, with their errors.

ONLY FORTH DEFINITIONS  ALSO FILES
S" lib/text.fs" INCLUDED
S" asm/xtensa.fs" INCLUDED
ONLY FORTH DEFINITIONS  ALSO INTERNALS  ALSO TEXT  ALSO XTENSA
DECIMAL

: I16@ ( ta -- x )  DUP TC@  SWAP 1+ TC@ 8 LSHIFT OR ;
: LAST ( -- x )  THERE 3 - I24@ ;           \ the instruction that came last
: LAST.N ( -- x )  THERE 2 - I16@ ;         \ the same for a .N form
: AT ( offset -- x )  TORG @ + I24@ ;       \ the instruction at ORG + offset
: AT.N ( offset -- x )  TORG @ + I16@ ;
: SKIP ( n -- )  TDP +! ;                   \ .skip n: n bytes 0

\ ==== Part 1: the code of the GNU source ====
$40378000 ORG

\ ---- constants for L32R, before the code
LABEL LIT0
$12345678 T,
LABEL LIT1
-1 T,
LABEL LIT2
0 T,
LABEL START

\ ---- three registers
T{ A0 A1 A2 AND, LAST -> $100120 }T   \ and a0, a1, a2
T{ A15 A14 A13 OR, LAST -> $20FED0 }T   \ or a15, a14, a13
T{ A3 A4 A5 XOR, LAST -> $303450 }T   \ xor a3, a4, a5
T{ A2 A3 A4 ADD, LAST -> $802340 }T   \ add a2, a3, a4
T{ A15 A15 A15 ADD, LAST -> $80FFF0 }T   \ add a15, a15, a15
T{ A1 A2 A3 ADDX2, LAST -> $901230 }T   \ addx2 a1, a2, a3
T{ A2 A2 A11 ADDX4, LAST -> $A022B0 }T   \ addx4 a2, a2, a11
T{ A4 A5 A6 ADDX8, LAST -> $B04560 }T   \ addx8 a4, a5, a6
T{ A7 A8 A9 SUB, LAST -> $C07890 }T   \ sub a7, a8, a9
T{ A10 A11 A12 SUBX2, LAST -> $D0ABC0 }T   \ subx2 a10, a11, a12
T{ A13 A14 A15 SUBX4, LAST -> $E0DEF0 }T   \ subx4 a13, a14, a15
T{ A0 A0 A0 SUBX8, LAST -> $F00000 }T   \ subx8 a0, a0, a0
T{ A2 A3 A4 SRC, LAST -> $812340 }T   \ src a2, a3, a4
T{ A2 A3 A4 MULL, LAST -> $822340 }T   \ mull a2, a3, a4
T{ A5 A6 A7 MULUH, LAST -> $A25670 }T   \ muluh a5, a6, a7
T{ A8 A9 A10 MULSH, LAST -> $B289A0 }T   \ mulsh a8, a9, a10
T{ A2 A3 A4 QUOU, LAST -> $C22340 }T   \ quou a2, a3, a4
T{ A11 A12 A13 QUOS, LAST -> $D2BCD0 }T   \ quos a11, a12, a13
T{ A14 A15 A0 REMU, LAST -> $E2EF00 }T   \ remu a14, a15, a0
T{ A1 A2 A3 REMS, LAST -> $F21230 }T   \ rems a1, a2, a3
T{ A2 A3 A4 MIN, LAST -> $432340 }T   \ min a2, a3, a4
T{ A5 A6 A7 MAX, LAST -> $535670 }T   \ max a5, a6, a7
T{ A8 A9 A10 MINU, LAST -> $6389A0 }T   \ minu a8, a9, a10
T{ A11 A12 A13 MAXU, LAST -> $73BCD0 }T   \ maxu a11, a12, a13
T{ A2 A3 A4 MOVEQZ, LAST -> $832340 }T   \ moveqz a2, a3, a4
T{ A5 A6 A7 MOVNEZ, LAST -> $935670 }T   \ movnez a5, a6, a7
T{ A8 A9 A10 MOVLTZ, LAST -> $A389A0 }T   \ movltz a8, a9, a10
T{ A11 A12 A13 MOVGEZ, LAST -> $B3BCD0 }T   \ movgez a11, a12, a13
T{ A2 A3 MOV, LAST -> $202330 }T   \ mov a2, a3
T{ A15 A0 MOV, LAST -> $20F000 }T   \ mov a15, a0

\ ---- two registers, one register, none
T{ A2 A3 NEG, LAST -> $602030 }T   \ neg a2, a3
T{ A15 A0 NEG, LAST -> $60F000 }T   \ neg a15, a0
T{ A4 A5 ABS, LAST -> $604150 }T   \ abs a4, a5
T{ A2 A3 SRL, LAST -> $912030 }T   \ srl a2, a3
T{ A4 A5 SRA, LAST -> $B14050 }T   \ sra a4, a5
T{ A6 A7 SLL, LAST -> $A16700 }T   \ sll a6, a7
T{ A2 A3 NSA, LAST -> $40E320 }T   \ nsa a2, a3
T{ A4 A5 NSAU, LAST -> $40F540 }T   \ nsau a4, a5
T{ A2 SSR, LAST -> $400200 }T   \ ssr a2
T{ A15 SSL, LAST -> $401F00 }T   \ ssl a15
T{ A3 SSA8L, LAST -> $402300 }T   \ ssa8l a3
T{ A4 SSA8B, LAST -> $403400 }T   \ ssa8b a4
T{ A2 JX, LAST -> $2A0 }T   \ jx a2
T{ A15 JX, LAST -> $FA0 }T   \ jx a15
T{ A0 CALLX0, LAST -> $C0 }T   \ callx0 a0
T{ A5 CALLX4, LAST -> $5D0 }T   \ callx4 a5
T{ A10 CALLX8, LAST -> $AE0 }T   \ callx8 a10
T{ A3 CALLX12, LAST -> $3F0 }T   \ callx12 a3
T{ RET, LAST -> $80 }T   \ ret
T{ ISYNC, LAST -> $2000 }T   \ isync
T{ RSYNC, LAST -> $2010 }T   \ rsync
T{ ESYNC, LAST -> $2020 }T   \ esync
T{ DSYNC, LAST -> $2030 }T   \ dsync
T{ MEMW, LAST -> $20C0 }T   \ memw
T{ EXTW, LAST -> $20D0 }T   \ extw
T{ NOP, LAST -> $20F0 }T   \ nop

\ ---- shifts and bit fields by constants
T{ A2 A3 1 SLLI, LAST -> $1123F0 }T   \ slli a2, a3, 1
T{ A2 A3 15 SLLI, LAST -> $112310 }T   \ slli a2, a3, 15
T{ A2 A3 16 SLLI, LAST -> $112300 }T   \ slli a2, a3, 16
T{ A2 A3 17 SLLI, LAST -> $123F0 }T   \ slli a2, a3, 17
T{ A15 A0 31 SLLI, LAST -> $1F010 }T   \ slli a15, a0, 31
T{ A2 A3 0 SRAI, LAST -> $212030 }T   \ srai a2, a3, 0
T{ A2 A3 15 SRAI, LAST -> $212F30 }T   \ srai a2, a3, 15
T{ A2 A3 16 SRAI, LAST -> $312030 }T   \ srai a2, a3, 16
T{ A15 A0 31 SRAI, LAST -> $31FF00 }T   \ srai a15, a0, 31
T{ A2 A3 0 SRLI, LAST -> $412030 }T   \ srli a2, a3, 0
T{ A15 A0 15 SRLI, LAST -> $41FF00 }T   \ srli a15, a0, 15
T{ 0 SSAI, LAST -> $404000 }T   \ ssai 0
T{ 15 SSAI, LAST -> $404F00 }T   \ ssai 15
T{ 16 SSAI, LAST -> $404010 }T   \ ssai 16
T{ 31 SSAI, LAST -> $404F10 }T   \ ssai 31
T{ A2 A3 0 1 EXTUI, LAST -> $42030 }T   \ extui a2, a3, 0, 1
T{ A2 A3 0 16 EXTUI, LAST -> $F42030 }T   \ extui a2, a3, 0, 16
T{ A2 A3 15 16 EXTUI, LAST -> $F42F30 }T   \ extui a2, a3, 15, 16
T{ A2 A3 16 16 EXTUI, LAST -> $F52030 }T   \ extui a2, a3, 16, 16
T{ A15 A0 31 1 EXTUI, LAST -> $5FF00 }T   \ extui a15, a0, 31, 1
T{ A4 A5 4 8 EXTUI, LAST -> $744450 }T   \ extui a4, a5, 4, 8
T{ A2 A3 7 SEXT, LAST -> $232300 }T   \ sext a2, a3, 7
T{ A4 A5 15 SEXT, LAST -> $234580 }T   \ sext a4, a5, 15
T{ A6 A7 22 SEXT, LAST -> $2367F0 }T   \ sext a6, a7, 22

\ ---- loads, stores, constants
T{ A2 A3 0 L8UI, LAST -> $322 }T   \ l8ui a2, a3, 0
T{ A15 A0 255 L8UI, LAST -> $FF00F2 }T   \ l8ui a15, a0, 255
T{ A2 A3 0 L16UI, LAST -> $1322 }T   \ l16ui a2, a3, 0
T{ A4 A5 510 L16UI, LAST -> $FF1542 }T   \ l16ui a4, a5, 510
T{ A6 A7 2 L16SI, LAST -> $19762 }T   \ l16si a6, a7, 2
T{ A8 A9 510 L16SI, LAST -> $FF9982 }T   \ l16si a8, a9, 510
T{ A2 A3 0 L32I, LAST -> $2322 }T   \ l32i a2, a3, 0
T{ A4 A5 4 L32I, LAST -> $12542 }T   \ l32i a4, a5, 4
T{ A15 A0 1020 L32I, LAST -> $FF20F2 }T   \ l32i a15, a0, 1020
T{ A2 A3 0 S8I, LAST -> $4322 }T   \ s8i a2, a3, 0
T{ A4 A5 255 S8I, LAST -> $FF4542 }T   \ s8i a4, a5, 255
T{ A6 A7 510 S16I, LAST -> $FF5762 }T   \ s16i a6, a7, 510
T{ A8 A9 0 S32I, LAST -> $6982 }T   \ s32i a8, a9, 0
T{ A10 A11 1020 S32I, LAST -> $FF6BA2 }T   \ s32i a10, a11, 1020
T{ A2 A3 0 ADDI, LAST -> $C322 }T   \ addi a2, a3, 0
T{ A2 A3 127 ADDI, LAST -> $7FC322 }T   \ addi a2, a3, 127
T{ A2 A3 -128 ADDI, LAST -> $80C322 }T   \ addi a2, a3, -128
T{ A15 A15 -1 ADDI, LAST -> $FFCFF2 }T   \ addi a15, a15, -1
T{ A2 A3 0 ADDMI, LAST -> $D322 }T   \ addmi a2, a3, 0
T{ A2 A3 256 ADDMI, LAST -> $1D322 }T   \ addmi a2, a3, 256
T{ A2 A3 32512 ADDMI, LAST -> $7FD322 }T   \ addmi a2, a3, 32512
T{ A2 A3 -32768 ADDMI, LAST -> $80D322 }T   \ addmi a2, a3, -32768
T{ A4 A5 -256 ADDMI, LAST -> $FFD542 }T   \ addmi a4, a5, -256
T{ A2 0 MOVI, LAST -> $A022 }T   \ movi a2, 0
T{ A2 1 MOVI, LAST -> $1A022 }T   \ movi a2, 1
T{ A2 255 MOVI, LAST -> $FFA022 }T   \ movi a2, 255
T{ A2 256 MOVI, LAST -> $A122 }T   \ movi a2, 256
T{ A2 2047 MOVI, LAST -> $FFA722 }T   \ movi a2, 2047
T{ A2 -1 MOVI, LAST -> $FFAF22 }T   \ movi a2, -1
T{ A2 -2048 MOVI, LAST -> $A822 }T   \ movi a2, -2048
T{ A15 $5A5 MOVI, LAST -> $A5A5F2 }T   \ movi a15, 0x5a5
T{ A2 LIT0 L32R, LAST -> $FFA921 }T   \ l32r a2, lit0
T{ A3 LIT1 L32R, LAST -> $FFAA31 }T   \ l32r a3, lit1
T{ A15 LIT2 L32R, LAST -> $FFAAF1 }T   \ l32r a15, lit2

\ ---- special registers
T{ A2 PS RSR, LAST -> $3E620 }T   \ rsr a2, ps
T{ A3 INTENABLE WSR, LAST -> $13E430 }T   \ wsr a3, intenable
T{ A4 SAR XSR, LAST -> $610340 }T   \ xsr a4, sar
T{ A5 CCOUNT RSR, LAST -> $3EA50 }T   \ rsr a5, ccount
T{ A6 VECBASE WSR, LAST -> $13E760 }T   \ wsr a6, vecbase
T{ A7 EXCCAUSE RSR, LAST -> $3E870 }T   \ rsr a7, exccause
T{ A8 EPC1 RSR, LAST -> $3B180 }T   \ rsr a8, epc1
T{ A9 EXCVADDR RSR, LAST -> $3EE90 }T   \ rsr a9, excvaddr
T{ A10 WINDOWBASE RSR, LAST -> $348A0 }T   \ rsr a10, windowbase
T{ A11 WINDOWSTART RSR, LAST -> $349B0 }T   \ rsr a11, windowstart
T{ A12 3 RSR, LAST -> $303C0 }T   \ rsr a12, 3
T{ A2 0 RSIL, LAST -> $6020 }T   \ rsil a2, 0
T{ A15 15 RSIL, LAST -> $6FF0 }T   \ rsil a15, 15

\ ---- 16-bit instructions
T{ A2 A3 0 L32I.N, LAST.N -> $328 }T   \ l32i.n a2, a3, 0
T{ A15 A0 60 L32I.N, LAST.N -> $F0F8 }T   \ l32i.n a15, a0, 60
T{ A13 A12 0 L32I.N, LAST.N -> $CD8 }T   \ l32i.n a13, a12, 0
T{ A2 A3 4 S32I.N, LAST.N -> $1329 }T   \ s32i.n a2, a3, 4
T{ A14 A15 60 S32I.N, LAST.N -> $FFE9 }T   \ s32i.n a14, a15, 60
T{ A2 A3 A4 ADD.N, LAST.N -> $234A }T   \ add.n a2, a3, a4
T{ A15 A0 A1 ADD.N, LAST.N -> $F01A }T   \ add.n a15, a0, a1
T{ A2 A3 -1 ADDI.N, LAST.N -> $230B }T   \ addi.n a2, a3, -1
T{ A2 A3 1 ADDI.N, LAST.N -> $231B }T   \ addi.n a2, a3, 1
T{ A12 A12 4 ADDI.N, LAST.N -> $CC4B }T   \ addi.n a12, a12, 4
T{ A15 A0 15 ADDI.N, LAST.N -> $F0FB }T   \ addi.n a15, a0, 15
T{ A2 A3 MOV.N, LAST.N -> $32D }T   \ mov.n a2, a3
T{ A15 A0 MOV.N, LAST.N -> $FD }T   \ mov.n a15, a0
T{ A2 0 MOVI.N, LAST.N -> $20C }T   \ movi.n a2, 0
T{ A2 -32 MOVI.N, LAST.N -> $26C }T   \ movi.n a2, -32
T{ A2 -1 MOVI.N, LAST.N -> $F27C }T   \ movi.n a2, -1
T{ A2 95 MOVI.N, LAST.N -> $F25C }T   \ movi.n a2, 95
T{ A15 17 MOVI.N, LAST.N -> $1F1C }T   \ movi.n a15, 17
T{ RET.N, LAST.N -> $F00D }T   \ ret.n
T{ NOP.N, LAST.N -> $F03D }T   \ nop.n

\ ---- jumps forward and back, in reach
LABEL BRANCHES
A2 A3 L1 BEQ,                 \ beq a2, a3, 1f
A4 A5 L1 BNE,                 \ bne a4, a5, 1f
A6 A7 L1 BLT,                 \ blt a6, a7, 1f
A8 A9 L1 BGE,                 \ bge a8, a9, 1f
A10 A11 L1 BLTU,              \ bltu a10, a11, 1f
A12 A13 L1 BGEU,              \ bgeu a12, a13, 1f
A14 A15 L1 BNONE,             \ bnone a14, a15, 1f
A0 A1 L1 BANY,                \ bany a0, a1, 1f
A2 A3 L1 BALL,                \ ball a2, a3, 1f
A4 A5 L1 BNALL,               \ bnall a4, a5, 1f
A6 A7 L1 BBC,                 \ bbc a6, a7, 1f
A8 A9 L1 BBS,                 \ bbs a8, a9, 1f
A2 0 L1 BBCI,                 \ bbci a2, 0, 1f
A3 15 L1 BBCI,                \ bbci a3, 15, 1f
A4 16 L1 BBCI,                \ bbci a4, 16, 1f
A5 31 L1 BBSI,                \ bbsi a5, 31, 1f
A2 L1 BEQZ,                   \ beqz a2, 1f
A3 L1 BNEZ,                   \ bnez a3, 1f
A4 L1 BLTZ,                   \ bltz a4, 1f
A5 L1 BGEZ,                   \ bgez a5, 1f
A2 -1 L1 BEQI,                \ beqi a2, -1, 1f
A2 1 L1 BEQI,                 \ beqi a2, 1, 1f
A3 10 L1 BNEI,                \ bnei a3, 10, 1f
A4 256 L1 BLTI,               \ blti a4, 256, 1f
A5 12 L1 BGEI,                \ bgei a5, 12, 1f
A6 32768 L1 BLTUI,            \ bltui a6, 32768, 1f
A6 65536 L1 BLTUI,            \ bltui a6, 65536, 1f
A7 2 L1 BGEUI,                \ bgeui a7, 2, 1f
A7 256 L1 BGEUI,              \ bgeui a7, 256, 1f
A2 L1 BEQZ.N,                 \ beqz.n a2, 1f
A3 L1 BNEZ.N,                 \ bnez.n a3, 1f
L1 J,                         \ j 1f
LABEL L1
A2 A3 L1 BEQ,                 \ beq a2, a3, 1b
A4 L1 BNEZ,                   \ bnez a4, 1b
A5 7 L1 BBSI,                 \ bbsi a5, 7, 1b
A6 8 L1 BGEUI,                \ bgeui a6, 8, 1b
L1 J,                         \ j 1b
A2 A3 BRANCHES BEQ,           \ beq a2, a3, branches
BRANCHES J,                   \ j branches

\ ---- the limits of each jump
LABEL LIMITS
A2 A3 L1 BEQ,                 \ beq a2, a3, 1f
128 SKIP
LABEL L1
124 SKIP
A2 A3 L1 BNE,                 \ bne a2, a3, 1b
A2 L2 BEQZ,                   \ beqz a2, 2f
2048 SKIP
LABEL L2
2044 SKIP
A2 L2 BNEZ,                   \ bnez a2, 2b
A2 4 L3 BEQI,                 \ beqi a2, 4, 3f
128 SKIP
LABEL L3
124 SKIP
A2 4 L3 BGEI,                 \ bgei a2, 4, 3b
A2 L4 BEQZ.N,                 \ beqz.n a2, 4f
65 SKIP
LABEL L4
A2 L5 BNEZ.N,                 \ bnez.n a2, 5f
2 SKIP
LABEL L5
L6 J,                         \ j 6f
10000 SKIP
LABEL L6
10000 SKIP
L6 J,                         \ j 6b
A2 LIT1 L32R,                 \ l32r a2, lit1

\ ---- a label before its place with FORWARD; calls to aligned targets
LABEL CALLS
FORWARD SUB1
SUB1 CALL0,                   \ call0 sub1
SUB1 CALL4,                   \ call4 sub1
SUB1 CALL8,                   \ call8 sub1
SUB1 CALL12,                  \ call12 sub1
FORWARD LATER
LATER J,                      \ j later
TALIGN
LABEL SUB1
RET,                          \ ret
SUB1 CALL0,                   \ call0 sub1
5 SKIP
SUB1 CALL0,                   \ call0 sub1
TALIGN
LABEL LATER
LATER CALL0,                  \ call0 later
LATER CALL8,                  \ call8 later
RET.N,                        \ ret.n
T{ CHECK-LABELS -> }T

\ ---- the jumps above, each at its offset from ORG
T{ $1B1 AT -> $5A1237 }T   \ beq a2, a3, 1f
T{ $1B4 AT -> $579457 }T   \ bne a4, a5, 1f
T{ $1B7 AT -> $542677 }T   \ blt a6, a7, 1f
T{ $1BA AT -> $51A897 }T   \ bge a8, a9, 1f
T{ $1BD AT -> $4E3AB7 }T   \ bltu a10, a11, 1f
T{ $1C0 AT -> $4BBCD7 }T   \ bgeu a12, a13, 1f
T{ $1C3 AT -> $480EF7 }T   \ bnone a14, a15, 1f
T{ $1C6 AT -> $458017 }T   \ bany a0, a1, 1f
T{ $1C9 AT -> $424237 }T   \ ball a2, a3, 1f
T{ $1CC AT -> $3FC457 }T   \ bnall a4, a5, 1f
T{ $1CF AT -> $3C5677 }T   \ bbc a6, a7, 1f
T{ $1D2 AT -> $39D897 }T   \ bbs a8, a9, 1f
T{ $1D5 AT -> $366207 }T   \ bbci a2, 0, 1f
T{ $1D8 AT -> $3363F7 }T   \ bbci a3, 15, 1f
T{ $1DB AT -> $307407 }T   \ bbci a4, 16, 1f
T{ $1DE AT -> $2DF5F7 }T   \ bbsi a5, 31, 1f
T{ $1E1 AT -> $2A216 }T   \ beqz a2, 1f
T{ $1E4 AT -> $27356 }T   \ bnez a3, 1f
T{ $1E7 AT -> $24496 }T   \ bltz a4, 1f
T{ $1EA AT -> $215D6 }T   \ bgez a5, 1f
T{ $1ED AT -> $1E0226 }T   \ beqi a2, -1, 1f
T{ $1F0 AT -> $1B1226 }T   \ beqi a2, 1, 1f
T{ $1F3 AT -> $189366 }T   \ bnei a3, 10, 1f
T{ $1F6 AT -> $15F4A6 }T   \ blti a4, 256, 1f
T{ $1F9 AT -> $12A5E6 }T   \ bgei a5, 12, 1f
T{ $1FC AT -> $F06B6 }T   \ bltui a6, 32768, 1f
T{ $1FF AT -> $C16B6 }T   \ bltui a6, 65536, 1f
T{ $202 AT -> $927F6 }T   \ bgeui a7, 2, 1f
T{ $205 AT -> $6F7F6 }T   \ bgeui a7, 256, 1f
T{ $208 AT.N -> $328C }T   \ beqz.n a2, 1f
T{ $20A AT.N -> $13CC }T   \ bnez.n a3, 1f
T{ $20C AT -> $FFFFC6 }T   \ j 1f
T{ $20F AT -> $FC1237 }T   \ beq a2, a3, 1b
T{ $212 AT -> $FF9456 }T   \ bnez a4, 1b
T{ $215 AT -> $F6E577 }T   \ bbsi a5, 7, 1b
T{ $218 AT -> $F386F6 }T   \ bgeui a6, 8, 1b
T{ $21B AT -> $FFFC06 }T   \ j 1b
T{ $21E AT -> $8F1237 }T   \ beq a2, a3, branches
T{ $221 AT -> $FFE306 }T   \ j branches
T{ $224 AT -> $7F1237 }T   \ beq a2, a3, 1f
T{ $323 AT -> $809237 }T   \ bne a2, a3, 1b
T{ $326 AT -> $7FF216 }T   \ beqz a2, 2f
T{ $1325 AT -> $800256 }T   \ bnez a2, 2b
T{ $1328 AT -> $7F4226 }T   \ beqi a2, 4, 3f
T{ $1427 AT -> $8042E6 }T   \ bgei a2, 4, 3b
T{ $142A AT.N -> $F2BC }T   \ beqz.n a2, 4f
T{ $146D AT.N -> $2CC }T   \ bnez.n a2, 5f
T{ $1471 AT -> $9C3C6 }T   \ j 6f
T{ $6294 AT -> $F63B06 }T   \ j 6b
T{ $6297 AT -> $E75B21 }T   \ l32r a2, lit1
T{ $629A AT -> $105 }T   \ call0 sub1
T{ $629D AT -> $D5 }T   \ call4 sub1
T{ $62A0 AT -> $A5 }T   \ call8 sub1
T{ $62A3 AT -> $B5 }T   \ call12 sub1
T{ $62A6 AT -> $486 }T   \ j later
T{ $62AC AT -> $80 }T   \ ret
T{ $62AF AT -> $FFFFC5 }T   \ call0 sub1
T{ $62B7 AT -> $FFFF45 }T   \ call0 sub1
T{ $62BC AT -> $FFFFC5 }T   \ call0 later
T{ $62BF AT -> $FFFFE5 }T   \ call8 later
T{ $62C2 AT.N -> $F00D }T   \ ret.n
T{ TDP @ -> 25284 }T

\ ---- exceptions, interrupts, the register window (after the part above)
T{ RFE, LAST -> $3000 }T   \ rfe
T{ RFDE, LAST -> $3200 }T   \ rfde
T{ RFWO, LAST -> $3400 }T   \ rfwo
T{ RFWU, LAST -> $3500 }T   \ rfwu
T{ 2 RFI, LAST -> $3210 }T   \ rfi 2
T{ 5 RFI, LAST -> $3510 }T   \ rfi 5
T{ 7 RFI, LAST -> $3710 }T   \ rfi 7
T{ 1 ROTW, LAST -> $408010 }T   \ rotw 1
T{ -1 ROTW, LAST -> $4080F0 }T   \ rotw -1
T{ 7 ROTW, LAST -> $408070 }T   \ rotw 7
T{ -8 ROTW, LAST -> $408080 }T   \ rotw -8
T{ 0 WAITI, LAST -> $7000 }T   \ waiti 0
T{ 15 WAITI, LAST -> $7F00 }T   \ waiti 15
T{ A0 A5 -16 L32E, LAST -> $9C500 }T   \ l32e a0, a5, -16
T{ A7 A1 -12 L32E, LAST -> $9D170 }T   \ l32e a7, a1, -12
T{ A11 A11 -20 L32E, LAST -> $9BBB0 }T   \ l32e a11, a11, -20
T{ A2 A3 -64 L32E, LAST -> $90320 }T   \ l32e a2, a3, -64
T{ A15 A14 -4 L32E, LAST -> $9FEF0 }T   \ l32e a15, a14, -4
T{ A0 A9 -16 S32E, LAST -> $49C900 }T   \ s32e a0, a9, -16
T{ A4 A0 -32 S32E, LAST -> $498040 }T   \ s32e a4, a0, -32
T{ A11 A0 -20 S32E, LAST -> $49B0B0 }T   \ s32e a11, a0, -20
T{ A1 A2 -64 S32E, LAST -> $490210 }T   \ s32e a1, a2, -64
T{ A15 A14 -4 S32E, LAST -> $49FEF0 }T   \ s32e a15, a14, -4
T{ A2 EXCSAVE1 WSR, LAST -> $13D120 }T   \ wsr a2, excsave1
T{ A3 EXCSAVE1 RSR, LAST -> $3D130 }T   \ rsr a3, excsave1
T{ A2 EXCSAVE2 XSR, LAST -> $61D220 }T   \ xsr a2, excsave2
T{ A4 INTERRUPT RSR, LAST -> $3E240 }T   \ rsr a4, interrupt
T{ A5 INTCLEAR WSR, LAST -> $13E350 }T   \ wsr a5, intclear
T{ A6 INTSET WSR, LAST -> $13E260 }T   \ wsr a6, intset
T{ A7 DEPC RSR, LAST -> $3C070 }T   \ rsr a7, depc
T{ A8 LCOUNT WSR, LAST -> $130280 }T   \ wsr a8, lcount
T{ A9 LBEG RSR, LAST -> $30090 }T   \ rsr a9, lbeg
T{ A10 LEND RSR, LAST -> $301A0 }T   \ rsr a10, lend
T{ A11 EPC2 RSR, LAST -> $3B2B0 }T   \ rsr a11, epc2
T{ A12 EPC6 RSR, LAST -> $3B6C0 }T   \ rsr a12, epc6
T{ A13 EPS2 WSR, LAST -> $13C2D0 }T   \ wsr a13, eps2
T{ A14 EPS6 RSR, LAST -> $3C6E0 }T   \ rsr a14, eps6
T{ A15 EXCSAVE5 XSR, LAST -> $61D5F0 }T   \ xsr a15, excsave5
T{ A2 DEBUGCAUSE RSR, LAST -> $3E920 }T   \ rsr a2, debugcause
T{ A3 CCOMPARE0 WSR, LAST -> $13F030 }T   \ wsr a3, ccompare0
T{ A4 CCOMPARE2 WSR, LAST -> $13F240 }T   \ wsr a4, ccompare2
T{ A1 32 ENTRY, LAST -> $4136 }T   \ entry a1, 32
T{ A1 0 ENTRY, LAST -> $136 }T   \ entry a1, 0
T{ A15 32760 ENTRY, LAST -> $FFFF36 }T   \ entry a15, 32760
T{ A2 8 ENTRY, LAST -> $1236 }T   \ entry a2, 8
T{ RETW, LAST -> $90 }T   \ retw
T{ RETW.N, LAST.N -> $F01D }T   \ retw.n
T{ 0 0 BREAK, LAST -> $4000 }T   \ break 0, 0
T{ 1 2 BREAK, LAST -> $4120 }T   \ break 1, 2
T{ 15 15 BREAK, LAST -> $4FF0 }T   \ break 15, 15
T{ 0 BREAK.N, LAST.N -> $F02D }T   \ break.n 0
T{ 3 BREAK.N, LAST.N -> $F32D }T   \ break.n 3
T{ 15 BREAK.N, LAST.N -> $FF2D }T   \ break.n 15
T{ A2 IBREAKENABLE RSR, LAST -> $36020 }T   \ rsr a2, ibreakenable
T{ A3 IBREAKA0 WSR, LAST -> $138030 }T   \ wsr a3, ibreaka0
T{ A4 IBREAKA1 WSR, LAST -> $138140 }T   \ wsr a4, ibreaka1
T{ A5 DBREAKA0 WSR, LAST -> $139050 }T   \ wsr a5, dbreaka0
T{ A6 DBREAKA1 WSR, LAST -> $139160 }T   \ wsr a6, dbreaka1
T{ A7 DBREAKC0 WSR, LAST -> $13A070 }T   \ wsr a7, dbreakc0
T{ A8 DBREAKC1 WSR, LAST -> $13A180 }T   \ wsr a8, dbreakc1
T{ A9 ICOUNT WSR, LAST -> $13EC90 }T   \ wsr a9, icount
T{ A10 ICOUNTLEVEL WSR, LAST -> $13EDA0 }T   \ wsr a10, icountlevel
T{ A11 PRID RSR, LAST -> $3EBB0 }T   \ rsr a11, prid
T{ A12 CPENABLE RSR, LAST -> $3E0C0 }T   \ rsr a12, cpenable
T{ A13 MISC0 WSR, LAST -> $13F4D0 }T   \ wsr a13, misc0
T{ A14 MISC3 RSR, LAST -> $3F7E0 }T   \ rsr a14, misc3
T{ A15 ATOMCTL RSR, LAST -> $363F0 }T   \ rsr a15, atomctl
T{ A2 MEMCTL RSR, LAST -> $36120 }T   \ rsr a2, memctl
T{ A3 SCOMPARE1 RSR, LAST -> $30C30 }T   \ rsr a3, scompare1

\ ---- CONST,: a jump over the constant, then L32R (j 1f; .align; .word; 1: l32r)
TALIGN
T{ A2 $12345678 CONST,  LAST -> $FFFF21 }T         \ l32r a2, the word before
T{ THERE 7 - T@  THERE 11 - I24@ -> $12345678 $106 }T   \ the word; j to the l32r

\ ---- IN-PLACE: the code goes into this system, at the address in the counter
CREATE PLACE-BUF 16 ALLOT   VARIABLE PLACE-P
: IN-BUF ( -- )   PLACE-BUF PLACE-P !  $40000000 PLACE-P $7FFFFFFF IN-PLACE ;
T{ IN-BUF  A2 A3 A4 ADD,  PLACE-P @ PLACE-BUF - -> 3 }T
T{ PLACE-BUF C@  PLACE-BUF 1+ C@  PLACE-BUF 2 + C@ -> $40 $23 $80 }T   \ add a2, a3, a4
T{ THERE  PLACE-BUF 3 + $40000000 + = -> TRUE }T
T{ IN-BUF  L1 J,  LABEL L1  CHECK-LABELS  PLACE-BUF $40000000 + I24@ -> $FFFFC6 }T   \ j 1f

\ ==== Part 2: errors ====
\ Each case starts new code (NEW). An error is THROW -2 with a message;
\ MSG" compares the message. Words that parse (LABEL) run under CATCH in
\ the line itself.

: NEW ( -- )  $40378000 ORG ;
: MSG" ( "text<quote>" -- f )  34 PARSE  ERRMSG @ ERRMSG CELL+ @ S= ;

: E1  NEW A2 3 0 L32I, ;         T{ ' E1  CATCH MSG" a register is missing" -> -2 TRUE }T
: E2  NEW 2 A3 0 L32I, ;         T{ ' E2  CATCH MSG" a register is missing" -> -2 TRUE }T
: E3  NEW A2 A3 A4 L32I, ;       T{ ' E3  CATCH MSG" number out of range" -> -2 TRUE }T
: E4  NEW A2 A3 1024 L32I, ;     T{ ' E4  CATCH MSG" number out of range" -> -2 TRUE }T
: E5  NEW A2 A3 -4 L32I, ;       T{ ' E5  CATCH MSG" number out of range" -> -2 TRUE }T
: E6  NEW A2 A3 2 L32I, ;
T{ ' E6  CATCH MSG" number is not a multiple of the step" -> -2 TRUE }T
: E7  NEW A2 A3 511 L16UI, ;
T{ ' E7  CATCH MSG" number is not a multiple of the step" -> -2 TRUE }T
: E8  NEW A2 A3 128 ADDI, ;      T{ ' E8  CATCH MSG" number out of range" -> -2 TRUE }T
: E9  NEW A2 A3 -129 ADDI, ;     T{ ' E9  CATCH MSG" number out of range" -> -2 TRUE }T
: E10 NEW A2 A3 100 ADDMI, ;
T{ ' E10 CATCH MSG" number is not a multiple of the step" -> -2 TRUE }T
: E11 NEW A2 A3 32768 ADDMI, ;   T{ ' E11 CATCH MSG" number out of range" -> -2 TRUE }T
: E12 NEW A2 2048 MOVI, ;        T{ ' E12 CATCH MSG" number out of range" -> -2 TRUE }T
: E13 NEW A2 -2049 MOVI, ;       T{ ' E13 CATCH MSG" number out of range" -> -2 TRUE }T
: E14 NEW A2 A3 0 SLLI, ;        T{ ' E14 CATCH MSG" number out of range" -> -2 TRUE }T
: E15 NEW A2 A3 32 SLLI, ;       T{ ' E15 CATCH MSG" number out of range" -> -2 TRUE }T
: E16 NEW A2 A3 32 SRAI, ;       T{ ' E16 CATCH MSG" number out of range" -> -2 TRUE }T
: E17 NEW A2 A3 16 SRLI, ;       T{ ' E17 CATCH MSG" number out of range" -> -2 TRUE }T
: E18 NEW 32 SSAI, ;             T{ ' E18 CATCH MSG" number out of range" -> -2 TRUE }T
: E19 NEW A2 A3 0 17 EXTUI, ;    T{ ' E19 CATCH MSG" number out of range" -> -2 TRUE }T
: E20 NEW A2 A3 17 16 EXTUI, ;   T{ ' E20 CATCH MSG" EXTUI reaches beyond bit 31" -> -2 TRUE }T
: E21 NEW A2 A3 6 SEXT, ;        T{ ' E21 CATCH MSG" number out of range" -> -2 TRUE }T
: E22 NEW A2 A3 0 ADDI.N, ;      T{ ' E22 CATCH MSG" number out of range" -> -2 TRUE }T
: E23 NEW A2 A3 16 ADDI.N, ;     T{ ' E23 CATCH MSG" number out of range" -> -2 TRUE }T
: E24 NEW A2 96 MOVI.N, ;        T{ ' E24 CATCH MSG" number out of range" -> -2 TRUE }T
: E25 NEW A2 -33 MOVI.N, ;       T{ ' E25 CATCH MSG" number out of range" -> -2 TRUE }T
: E26 NEW A2 A3 64 L32I.N, ;     T{ ' E26 CATCH MSG" number out of range" -> -2 TRUE }T
: E27 NEW A2 256 RSR, ;          T{ ' E27 CATCH MSG" number out of range" -> -2 TRUE }T
: E28 NEW A2 9 L1 BEQI, ;
T{ ' E28 CATCH MSG" no branch compares with this number" -> -2 TRUE }T
: E29 NEW A2 1 L1 BLTUI, ;
T{ ' E29 CATCH MSG" no branch compares with this number" -> -2 TRUE }T

\ Jumps out of reach, back and forward
NEW LABEL X1 125 SKIP
: E30 A2 A3 X1 BEQ, ;            T{ ' E30 CATCH MSG" number out of range" -> -2 TRUE }T
NEW A2 A3 L1 BEQ, 129 SKIP
T{ ' LABEL CATCH L1 MSG" number out of range" -> -2 TRUE }T
NEW LABEL X2
: E32 A2 X2 BEQZ.N, ;            T{ ' E32 CATCH MSG" number out of range" -> -2 TRUE }T
NEW A2 L1 BEQZ.N, 66 SKIP
T{ ' LABEL CATCH L1 MSG" number out of range" -> -2 TRUE }T

\ Constants of L32R, targets of calls
: E34 NEW A2 L1 L32R, ;
T{ ' E34 CATCH MSG" L32R needs its constant before the code" -> -2 TRUE }T
NEW NOP.N, LABEL X3 0 T,
: E35 A2 X3 L32R, ;
T{ ' E35 CATCH MSG" the constant of L32R must be aligned" -> -2 TRUE }T
NEW NOP.N, LABEL X4
: E36 X4 CALL0, ;
T{ ' E36 CATCH MSG" the target of a call must be aligned" -> -2 TRUE }T

\ Labels without place, placed twice, not a jump
\ LABEL X5 for a new name sets >IN back before its error; so X5 comes from
\ FORWARD here (the same check: a named label starts a new scope).
NEW FORWARD X5 A2 L1 BEQZ,
T{ ' LABEL CATCH X5 MSG" label without place: L1" -> -2 TRUE }T
NEW LABEL L2
T{ ' LABEL CATCH L2 MSG" label placed twice: L2" -> -2 TRUE }T
NEW L3 T,
T{ ' LABEL CATCH L3 MSG" a label before its place can only be the target of a jump" -> -2 TRUE }T
NEW L4 DROP $4136 I24,           \ entry a1, 32
T{ ' LABEL CATCH L4 MSG" ENTRY is not a jump" -> -2 TRUE }T
NEW FORWARD X6 X6 J,
T{ ' CHECK-LABELS CATCH MSG" label without place: X6" -> -2 TRUE }T
: E42 NEW TARGET-SIZE SKIP NOP, ;
T{ ' E42 CATCH MSG" address outside the target memory" -> -2 TRUE }T

\ The handlers of the window, the return from a level
: E43 NEW A2 A3 0 L32E, ;        T{ ' E43 CATCH MSG" number out of range" -> -2 TRUE }T
: E44 NEW A2 A3 -68 S32E, ;      T{ ' E44 CATCH MSG" number out of range" -> -2 TRUE }T
: E45 NEW A2 A3 -6 L32E, ;
T{ ' E45 CATCH MSG" number is not a multiple of the step" -> -2 TRUE }T
: E46 NEW 1 RFI, ;               T{ ' E46 CATCH MSG" number out of range" -> -2 TRUE }T
: E47 NEW 16 WAITI, ;            T{ ' E47 CATCH MSG" number out of range" -> -2 TRUE }T
: E49 NEW A1 36 ENTRY, ;
T{ ' E49 CATCH MSG" number is not a multiple of the step" -> -2 TRUE }T
: E50 NEW A1 32768 ENTRY, ;      T{ ' E50 CATCH MSG" number out of range" -> -2 TRUE }T
: E51 NEW 16 0 BREAK, ;          T{ ' E51 CATCH MSG" number out of range" -> -2 TRUE }T
: E52 NEW 16 BREAK.N, ;          T{ ' E52 CATCH MSG" number out of range" -> -2 TRUE }T
NEW

\ In place a named label would be a new word in the middle of the code
IN-BUF
T{ ' LABEL CATCH X7 MSG" here only the labels L1 to L9" -> -2 TRUE }T
T{ ' FORWARD CATCH X8 MSG" here only the labels L1 to L9" -> -2 TRUE }T
NEW

\ No target memory yet: as before the first ORG (and without IN-PLACE)
TARGET CONSTANT SAVED-TARGET
: E48 0 TO TARGET  $40378000 TC@ ;
T{ ' E48 CATCH MSG" no target memory: ORG first" -> -2 TRUE }T
SAVED-TARGET TO TARGET
NEW

\ ==== Part 3: the loop option, MUL16U, MOVSP, a part of the FPU ====
\ One more piece of code at $40378000, from a GNU source of its own (the
\ same assembler as part 1), then the errors of these instructions.

$40378000 ORG
T{ A2 A3 A4 MUL16U, LAST -> $C12340 }T   \ mul16u a2, a3, a4
T{ A15 A0 A1 MUL16U, LAST -> $C1F010 }T   \ mul16u a15, a0, a1
T{ A1 A2 MOVSP, LAST -> $1210 }T   \ movsp a1, a2
T{ A15 A14 MOVSP, LAST -> $1EF0 }T   \ movsp a15, a14
T{ F0 F1 F2 ADD.S, LAST -> $A0120 }T   \ add.s f0, f1, f2
T{ F15 F14 F13 ADD.S, LAST -> $AFED0 }T   \ add.s f15, f14, f13
T{ F3 F4 F5 SUB.S, LAST -> $1A3450 }T   \ sub.s f3, f4, f5
T{ F6 F7 F8 MUL.S, LAST -> $2A6780 }T   \ mul.s f6, f7, f8
T{ F9 F10 F11 MADD.S, LAST -> $4A9AB0 }T   \ madd.s f9, f10, f11
T{ B0 F1 F2 OEQ.S, LAST -> $2B0120 }T   \ oeq.s b0, f1, f2
T{ B15 F14 F13 OLT.S, LAST -> $4BFED0 }T   \ olt.s b15, f14, f13
T{ B7 F8 F9 OLE.S, LAST -> $6B7890 }T   \ ole.s b7, f8, f9
T{ F0 A2 0 FLOAT.S, LAST -> $CA0200 }T   \ float.s f0, a2, 0
T{ F15 A15 15 FLOAT.S, LAST -> $CAFFF0 }T   \ float.s f15, a15, 15
T{ F1 A3 4 UFLOAT.S, LAST -> $DA1340 }T   \ ufloat.s f1, a3, 4
T{ A2 F3 0 UTRUNC.S, LAST -> $EA2300 }T   \ utrunc.s a2, f3, 0
T{ A15 F15 15 UTRUNC.S, LAST -> $EAFFF0 }T   \ utrunc.s a15, f15, 15
T{ F2 F3 MOV.S, LAST -> $FA2300 }T   \ mov.s f2, f3
T{ F15 F0 MOV.S, LAST -> $FAF000 }T   \ mov.s f15, f0
T{ A2 F3 RFR, LAST -> $FA2340 }T   \ rfr a2, f3
T{ A15 F0 RFR, LAST -> $FAF040 }T   \ rfr a15, f0
T{ F4 A5 WFR, LAST -> $FA4550 }T   \ wfr f4, a5
T{ F0 A15 WFR, LAST -> $FA0F50 }T   \ wfr f0, a15
T{ F0 A1 0 LSI, LAST -> $103 }T   \ lsi f0, a1, 0
T{ F15 A15 1020 LSI, LAST -> $FF0FF3 }T   \ lsi f15, a15, 1020
T{ F1 A2 4 SSI, LAST -> $14213 }T   \ ssi f1, a2, 4
T{ F14 A13 1020 SSI, LAST -> $FF4DE3 }T   \ ssi f14, a13, 1020

\ ---- loops and the jumps on a boolean, with their limits
LABEL LOOPS
A2 L1 LOOP,                   \ loop a2, 1f
NOP,                          \ nop
LABEL L1
A3 L2 LOOPNEZ,                \ loopnez a3, 2f
NOP.N,                        \ nop.n
LABEL L2
A15 L3 LOOPGTZ,               \ loopgtz a15, 3f
NOP.N,  NOP.N,                \ nop.n; nop.n
LABEL L3
A4 L4 LOOP,                   \ loop a4, 4f
256 SKIP
LABEL L4
A5 L5 LOOP,                   \ loop a5, 5f
1 SKIP
LABEL L5
B0 L6 BF,                     \ bf b0, 6f
B15 L6 BT,                    \ bt b15, 6f
125 SKIP
LABEL L6
B1 L6 BF,                     \ bf b1, 6b
LABEL L7
124 SKIP
B2 L7 BT,                     \ bt b2, 7b
NOP,                          \ nop
T{ CHECK-LABELS -> }T
T{ $51 AT -> $28276 }T   \ loop a2, 1f
T{ $57 AT -> $19376 }T   \ loopnez a3, 2f
T{ $5C AT -> $3AF76 }T   \ loopgtz a15, 3f
T{ $63 AT -> $FF8476 }T   \ loop a4, 4f
T{ $166 AT -> $8576 }T   \ loop a5, 5f
T{ $16A AT -> $7F0076 }T   \ bf b0, 6f
T{ $16D AT -> $7C1F76 }T   \ bt b15, 6f
T{ $1ED AT -> $FC0176 }T   \ bf b1, 6b
T{ $26C AT -> $801276 }T   \ bt b2, 7b
T{ TDP @ -> $272 }T

\ ---- the errors
: E53 NEW A2 A3 A4 ADD.S, ;
T{ ' E53 CATCH MSG" a floating point register is missing" -> -2 TRUE }T
: E54 NEW F0 F1 F2 OEQ.S, ;
T{ ' E54 CATCH MSG" a boolean register is missing" -> -2 TRUE }T
: E55 NEW A2 F0 F1 MUL16U, ;     T{ ' E55 CATCH MSG" a register is missing" -> -2 TRUE }T
: E56 NEW F0 A1 2 LSI, ;
T{ ' E56 CATCH MSG" number is not a multiple of the step" -> -2 TRUE }T
: E57 NEW F0 A1 1024 SSI, ;      T{ ' E57 CATCH MSG" number out of range" -> -2 TRUE }T
: E58 NEW F0 A2 16 FLOAT.S, ;    T{ ' E58 CATCH MSG" number out of range" -> -2 TRUE }T
NEW LABEL X9
: E59 A2 X9 LOOP, ;              T{ ' E59 CATCH MSG" number out of range" -> -2 TRUE }T
NEW A2 L1 LOOP, 257 SKIP
T{ ' LABEL CATCH L1 MSG" number out of range" -> -2 TRUE }T
NEW B0 L1 BF, 129 SKIP
T{ ' LABEL CATCH L1 MSG" number out of range" -> -2 TRUE }T
NEW LABEL X10 126 SKIP
: E60 B0 X10 BT, ;               T{ ' E60 CATCH MSG" number out of range" -> -2 TRUE }T
NEW

S" xtensa" .RESULTS
