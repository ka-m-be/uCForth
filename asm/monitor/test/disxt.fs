\ disxt.fs - tests of the package DISXT (asm/monitor/disxt.fs)
\
\ Load (in the folder ucf), after the package FILES and the tester:
\   build/ucf-c -32 host/files.fs forth/test/tester.fs asm/monitor/test/disxt.fs
\ The tests need cells of 32 bits.
\
\ Each line X: is one instruction: its address, its bytes, "|", and the
\ text that DECODE must give; the instruction must also end after its
\ bytes. The texts come from GNU objdump 2.35.1 for the ESP32 (esp-2021r2,
\ PlatformIO), once, and are kept here as data. The bytes: every
\ encoding of op0 0 (QRST) and 4 (MAC16) and every instruction of 16
\ bits, the others with several values of each field; each text that
\ objdump gave was checked with GNU as (the same bytes at the same
\ address). 2 314 376 encodings were checked so; these lines are a
\ choice: each instruction with several values, each special register,
\ and each kind of encoding without an instruction. Where this
\ disassembler writes otherwise (asm/monitor/README.md, "Xtensa"), the
\ text here is changed:
\
\   - a target is 0x and hex digits (objdump: 8 hex digits and a symbol);
\   - an encoding without an instruction is one line .byte with all its
\     bytes: 3 for op0 0 to 7, 2 for op0 8 to 13, 1 for op0 14 and 15
\     (objdump: a line for each byte, and many such encodings as lsi);
\   - RET, RETW, RET.N, RETW.N and RFDD only with the fields that the
\     manual wants 0; ANY4, ALL4, ANY8, ALL8 only with bs a multiple of
\     4 or 8 (objdump ignores these fields);
\   - a special register without a name in the table: its number after
\     the other operands, "rsr a2, 99" (objdump: lsi);
\   - .byte writes 0 as 0x0 (objdump: 00).
\
\ EXTUI with shift + bits > 32 shows as objdump shows it (the manual
\ calls it undefined; GNU as does not take it).

ONLY FORTH DEFINITIONS  ALSO FILES
S" lib/text.fs" INCLUDED
ONLY FORTH DEFINITIONS  ALSO FILES
S" asm/monitor/monitor.fs" INCLUDED
S" asm/monitor/disxt.fs" INCLUDED
ONLY FORTH DEFINITIONS  ALSO INTERNALS  ALSO TEXT  ALSO MONITOR  ALSO DISXT
DECIMAL

CREATE CODE-BUF 16 ALLOT   VARIABLE CODE-AT   VARIABLE CODE-U
: CODE-C@ ( a -- c )   CODE-AT @ -  DUP 16 U< IF CODE-BUF + C@ ELSE DROP $FF THEN ;
' CODE-C@ IS DIS-C@
: HEXDIGIT ( c -- n )   DUP '9' > IF  32 OR 'a' - 10 +  ELSE  '0' -  THEN ;
: HEX# ( a u -- n )   0 SWAP 0 ?DO  4 LSHIFT  OVER I + C@ HEXDIGIT OR  LOOP  NIP ;
: BAR? ( a u -- f )   DUP 1 = IF  OVER C@ '|' =  ELSE  FALSE  THEN  NIP NIP ;
: X: ( "addr" "bytes" "|" "text" -- )   \ one case: DECODE gives the text and ends after the bytes
  PARSE-NAME HEX# CODE-AT !  0 CODE-U !
  BEGIN  PARSE-NAME 2DUP BAR? 0= WHILE  HEX# CODE-BUF CODE-U @ + C!  1 CODE-U +!  REPEAT  2DROP
  SOURCE >IN @ /STRING -LEADING -TRAILING              ( a u )
  CODE-AT @ DECODE CODE-AT @ -  >R
  1 #TESTS +!
  2DUP TXT COUNT S= 0=  R> CODE-U @ <> OR IF
    1 #ERRORS +!  ." WRONG: " SOURCE TYPE CR ."   got: " TXT COUNT TYPE CR
  THEN  2DROP  SOURCE NIP >IN ! ;

X: 40080000 00 00 00 | ill
X: 40080040 80 00 00 | ret
X: 40080048 90 00 00 | retw
X: 40080050 a0 00 00 | jx a0
X: 40080250 a0 04 00 | jx a4
X: 400807d0 a0 0f 00 | jx a15
X: 40080060 c0 00 00 | callx0 a0
X: 400805e0 c0 0b 00 | callx0 a11
X: 400807e0 c0 0f 00 | callx0 a15
X: 40080068 d0 00 00 | callx4 a0
X: 40080368 d0 06 00 | callx4 a6
X: 400807e8 d0 0f 00 | callx4 a15
X: 40080070 e0 00 00 | callx8 a0
X: 400806f0 e0 0d 00 | callx8 a13
X: 400807f0 e0 0f 00 | callx8 a15
X: 40080078 f0 00 00 | callx12 a0
X: 40080278 f0 04 00 | callx12 a4
X: 400807f8 f0 0f 00 | callx12 a15
X: 40080800 00 10 00 | movsp a0, a0
X: 400809d0 a0 13 00 | movsp a10, a3
X: 40080a58 b0 14 00 | movsp a11, a4
X: 40080ff8 f0 1f 00 | movsp a15, a15
X: 40081000 00 20 00 | isync
X: 40081008 10 20 00 | rsync
X: 40081010 20 20 00 | esync
X: 40081018 30 20 00 | dsync
X: 40081040 80 20 00 | excw
X: 40081060 c0 20 00 | memw
X: 40081068 d0 20 00 | extw
X: 40081078 f0 20 00 | nop
X: 40081800 00 30 00 | rfe
X: 40081808 10 30 00 | rfi 0
X: 40081c08 10 38 00 | rfi 8
X: 40081f88 10 3f 00 | rfi 15
X: 40081900 00 32 00 | rfde
X: 40081a00 00 34 00 | rfwo
X: 40081a80 00 35 00 | rfwu
X: 40082000 00 40 00 | break 0, 0
X: 40082230 60 44 00 | break 4, 6
X: 40082600 00 4c 00 | break 12, 0
X: 400827f8 f0 4f 00 | break 15, 15
X: 40082800 00 50 00 | syscall
X: 40082880 00 51 00 | simcall
X: 40083000 00 60 00 | rsil a0, 0
X: 40083100 00 62 00 | rsil a0, 2
X: 40083458 b0 68 00 | rsil a11, 8
X: 400837f8 f0 6f 00 | rsil a15, 15
X: 40083800 00 70 00 | waiti 0
X: 40083900 00 72 00 | waiti 2
X: 40083f80 00 7f 00 | waiti 15
X: 40083870 e0 70 00 | lddr32.p a0
X: 40083e70 e0 7c 00 | lddr32.p a12
X: 40083ff0 e0 7f 00 | lddr32.p a15
X: 40083878 f0 70 00 | sddr32.p a0
X: 40083af8 f0 75 00 | sddr32.p a5
X: 40083ff8 f0 7f 00 | sddr32.p a15
X: 40084000 00 80 00 | any4 b0, b0:b1:b2:b3
X: 40084228 50 84 00 | any4 b5, b4:b5:b6:b7
X: 40084678 f0 8c 00 | any4 b15, b12:b13:b14:b15
X: 40084800 00 90 00 | all4 b0, b0:b1:b2:b3
X: 40084a18 30 94 00 | all4 b3, b4:b5:b6:b7
X: 40084e78 f0 9c 00 | all4 b15, b12:b13:b14:b15
X: 40085000 00 a0 00 | any8 b0, b0:b1:b2:b3:b4:b5:b6:b7
X: 40085068 d0 a0 00 | any8 b13, b0:b1:b2:b3:b4:b5:b6:b7
X: 40085478 f0 a8 00 | any8 b15, b8:b9:b10:b11:b12:b13:b14:b15
X: 40085800 00 b0 00 | all8 b0, b0:b1:b2:b3:b4:b5:b6:b7
X: 40085c30 60 b8 00 | all8 b6, b8:b9:b10:b11:b12:b13:b14:b15
X: 40085c78 f0 b8 00 | all8 b15, b8:b9:b10:b11:b12:b13:b14:b15
X: 40088000 00 00 01 | slli a0, a0, 32
X: 4008aab8 70 55 01 | slli a5, a5, 25
X: 4008eeb0 60 dd 01 | slli a13, a13, 26
X: 4008ef38 70 de 01 | slli a13, a14, 25
X: 4010d030 60 a0 11 | slli a10, a0, 10
X: 4010fff8 f0 ff 11 | slli a15, a15, 1
X: 40090000 00 00 02 | andb b0, b0, b0
X: 40092598 30 4b 02 | andb b4, b11, b3
X: 40095998 30 b3 02 | andb b11, b3, b3
X: 40097ff8 f0 ff 02 | andb b15, b15, b15
X: 40098040 80 00 03 | rsr.lbeg a8
X: 400980c0 80 01 03 | rsr.lend a8
X: 40098140 80 02 03 | rsr.lcount a8
X: 400981c0 80 03 03 | rsr.sar a8
X: 40098240 80 04 03 | rsr.br a8
X: 400982c0 80 05 03 | rsr.litbase a8
X: 40098640 80 0c 03 | rsr.scompare1 a8
X: 40098840 80 10 03 | rsr.acclo a8
X: 400988c0 80 11 03 | rsr.acchi a8
X: 40099040 80 20 03 | rsr.m0 a8
X: 400990c0 80 21 03 | rsr.m1 a8
X: 40099140 80 22 03 | rsr.m2 a8
X: 400991c0 80 23 03 | rsr.m3 a8
X: 4009a440 80 48 03 | rsr.windowbase a8
X: 4009a4c0 80 49 03 | rsr.windowstart a8
X: 4009b040 80 60 03 | rsr.ibreakenable a8
X: 4009b0c0 80 61 03 | rsr.memctl a8
X: 4009b1c0 80 63 03 | rsr.atomctl a8
X: 4009b440 80 68 03 | rsr.ddr a8
X: 4009c040 80 80 03 | rsr.ibreaka0 a8
X: 4009c0c0 80 81 03 | rsr.ibreaka1 a8
X: 4009c840 80 90 03 | rsr.dbreaka0 a8
X: 4009c8c0 80 91 03 | rsr.dbreaka1 a8
X: 4009d040 80 a0 03 | rsr.dbreakc0 a8
X: 4009d0c0 80 a1 03 | rsr.dbreakc1 a8
X: 4009d840 80 b0 03 | rsr.configid0 a8
X: 4009d8c0 80 b1 03 | rsr.epc1 a8
X: 4009d940 80 b2 03 | rsr.epc2 a8
X: 4009d9c0 80 b3 03 | rsr.epc3 a8
X: 4009da40 80 b4 03 | rsr.epc4 a8
X: 4009dac0 80 b5 03 | rsr.epc5 a8
X: 4009db40 80 b6 03 | rsr.epc6 a8
X: 4009dbc0 80 b7 03 | rsr.epc7 a8
X: 4009e040 80 c0 03 | rsr.depc a8
X: 4009e140 80 c2 03 | rsr.eps2 a8
X: 4009e1c0 80 c3 03 | rsr.eps3 a8
X: 4009e240 80 c4 03 | rsr.eps4 a8
X: 4009e2c0 80 c5 03 | rsr.eps5 a8
X: 4009e340 80 c6 03 | rsr.eps6 a8
X: 4009e3c0 80 c7 03 | rsr.eps7 a8
X: 4009e840 80 d0 03 | rsr.configid1 a8
X: 4009e8c0 80 d1 03 | rsr.excsave1 a8
X: 4009e940 80 d2 03 | rsr.excsave2 a8
X: 4009e9c0 80 d3 03 | rsr.excsave3 a8
X: 4009ea40 80 d4 03 | rsr.excsave4 a8
X: 4009eac0 80 d5 03 | rsr.excsave5 a8
X: 4009eb40 80 d6 03 | rsr.excsave6 a8
X: 4009ebc0 80 d7 03 | rsr.excsave7 a8
X: 4009f040 80 e0 03 | rsr.cpenable a8
X: 4009f140 80 e2 03 | rsr.interrupt a8
X: 4009f240 80 e4 03 | rsr.intenable a8
X: 4009f340 80 e6 03 | rsr.ps a8
X: 4009f3c0 80 e7 03 | rsr.vecbase a8
X: 4009f440 80 e8 03 | rsr.exccause a8
X: 4009f4c0 80 e9 03 | rsr.debugcause a8
X: 4009f540 80 ea 03 | rsr.ccount a8
X: 4009f5c0 80 eb 03 | rsr.prid a8
X: 4009f640 80 ec 03 | rsr.icount a8
X: 4009f6c0 80 ed 03 | rsr.icountlevel a8
X: 4009f740 80 ee 03 | rsr.excvaddr a8
X: 4009f840 80 f0 03 | rsr.ccompare0 a8
X: 4009f8c0 80 f1 03 | rsr.ccompare1 a8
X: 4009f940 80 f2 03 | rsr.ccompare2 a8
X: 4009fa40 80 f4 03 | rsr.misc0 a8
X: 4009fac0 80 f5 03 | rsr.misc1 a8
X: 4009fb40 80 f6 03 | rsr.misc2 a8
X: 4009fbc0 80 f7 03 | rsr.misc3 a8
X: 4008e678 f0 f0 f5 | extui a15, a15, 16, 16
X: 400a0000 00 00 04 | extui a0, a0, 0, 1
X: 4012c4f0 e0 89 15 | extui a8, a14, 25, 2
X: 4012dcd0 a0 b9 15 | extui a11, a10, 25, 2
X: 401926e0 c0 c9 b4 | extui a12, a12, 9, 12
X: 40194fb8 70 47 84 | extui a4, a7, 7, 9
X: 400c0000 00 00 08 | lsx f0, a0, a0
X: 400c0880 00 11 08 | lsx f1, a1, a0
X: 400c6a18 30 d4 08 | lsx f13, a4, a3
X: 400c7ff8 f0 ff 08 | lsx f15, a15, a15
X: 400c8000 00 00 09 | l32e a0, a0, -64
X: 400ca4d0 a0 49 09 | l32e a10, a9, -48
X: 400ce8a0 40 d1 09 | l32e a4, a1, -12
X: 400cfff8 f0 ff 09 | l32e a15, a15, -4
X: 400d0000 00 00 0a | add.s f0, f0, f0
X: 400d1490 20 29 0a | add.s f2, f9, f2
X: 400d4c40 80 98 0a | add.s f9, f8, f8
X: 400d7ff8 f0 ff 0a | add.s f15, f15, f15
X: 400de700 00 ce 0b | rf64r a0, 0
X: 400de740 80 ce 0b | rf64r a8, 0
X: 400de7f8 f0 cf 0b | rf64r a15, 1
X: 400df000 00 e0 0b | wf64r a0, a0, 0
X: 400df9b8 70 f3 0b | wf64r a3, a7, 1
X: 400df9f8 f0 f3 0b | wf64r a3, a15, 1
X: 400dfff8 f0 ff 0b | wf64r a15, a15, 1
X: 400f0000 00 00 0e | read_impwire a0
X: 400f0008 10 00 0e | read_impwire a1
X: 400f0078 f0 00 0e | read_impwire a15
X: 400f0800 00 10 0e | setb_expstate 0
X: 400f0880 00 11 0e | setb_expstate 16
X: 400f08f8 f0 11 0e | setb_expstate 31
X: 400f0900 00 12 0e | clrb_expstate 0
X: 400f0960 c0 12 0e | clrb_expstate 12
X: 400f09f8 f0 13 0e | clrb_expstate 31
X: 400f1000 00 20 0e | wrmsk_expstate a0, a0
X: 400f1190 20 23 0e | wrmsk_expstate a2, a3
X: 400f14f8 f0 29 0e | wrmsk_expstate a15, a9
X: 400f17f8 f0 2f 0e | wrmsk_expstate a15, a15
X: 400f6800 00 d0 0e | f64sig a0, a0
X: 400f68d8 b0 d1 0e | f64sig a11, a1
X: 400f6f40 80 de 0e | f64sig a8, a14
X: 400f6ff8 f0 df 0e | f64sig a15, a15
X: 400dedf8 f0 ff ff | f64cmph a15, a15, a15, 15
X: 400e8e68 d0 98 9f | f64cmph a9, a8, a13, 9
X: 400f8000 00 00 0f | f64cmph a0, a0, a0, 0
X: 400fc4c0 80 89 0f | f64cmph a8, a9, a8, 0
X: 40165228 50 f4 df | f64cmph a15, a4, a5, 13
X: 401eb620 40 e8 bf | f64cmph a14, a8, a4, 11
X: 40100000 00 00 10 | and a0, a0, a0
X: 40102748 90 4e 10 | and a4, a14, a9
X: 40103a38 70 74 10 | and a7, a4, a7
X: 40107ff8 f0 ff 10 | and a15, a15, a15
X: 40110000 00 00 12 | andbc b0, b0, b0
X: 401104d8 b0 09 12 | andbc b0, b9, b11
X: 401175a8 50 eb 12 | andbc b14, b11, b5
X: 40117ff8 f0 ff 12 | andbc b15, b15, b15
X: 40118040 80 00 13 | wsr.lbeg a8
X: 401180c0 80 01 13 | wsr.lend a8
X: 40118140 80 02 13 | wsr.lcount a8
X: 401181c0 80 03 13 | wsr.sar a8
X: 40118240 80 04 13 | wsr.br a8
X: 401182c0 80 05 13 | wsr.litbase a8
X: 40118640 80 0c 13 | wsr.scompare1 a8
X: 40118840 80 10 13 | wsr.acclo a8
X: 401188c0 80 11 13 | wsr.acchi a8
X: 40119040 80 20 13 | wsr.m0 a8
X: 401190c0 80 21 13 | wsr.m1 a8
X: 40119140 80 22 13 | wsr.m2 a8
X: 401191c0 80 23 13 | wsr.m3 a8
X: 4011a440 80 48 13 | wsr.windowbase a8
X: 4011a4c0 80 49 13 | wsr.windowstart a8
X: 4011acc0 80 59 13 | wsr.mmid a8
X: 4011b040 80 60 13 | wsr.ibreakenable a8
X: 4011b0c0 80 61 13 | wsr.memctl a8
X: 4011b1c0 80 63 13 | wsr.atomctl a8
X: 4011b440 80 68 13 | wsr.ddr a8
X: 4011c040 80 80 13 | wsr.ibreaka0 a8
X: 4011c0c0 80 81 13 | wsr.ibreaka1 a8
X: 4011c840 80 90 13 | wsr.dbreaka0 a8
X: 4011c8c0 80 91 13 | wsr.dbreaka1 a8
X: 4011d040 80 a0 13 | wsr.dbreakc0 a8
X: 4011d0c0 80 a1 13 | wsr.dbreakc1 a8
X: 4011d840 80 b0 13 | wsr.configid0 a8
X: 4011d8c0 80 b1 13 | wsr.epc1 a8
X: 4011d940 80 b2 13 | wsr.epc2 a8
X: 4011d9c0 80 b3 13 | wsr.epc3 a8
X: 4011da40 80 b4 13 | wsr.epc4 a8
X: 4011dac0 80 b5 13 | wsr.epc5 a8
X: 4011db40 80 b6 13 | wsr.epc6 a8
X: 4011dbc0 80 b7 13 | wsr.epc7 a8
X: 4011e040 80 c0 13 | wsr.depc a8
X: 4011e140 80 c2 13 | wsr.eps2 a8
X: 4011e1c0 80 c3 13 | wsr.eps3 a8
X: 4011e240 80 c4 13 | wsr.eps4 a8
X: 4011e2c0 80 c5 13 | wsr.eps5 a8
X: 4011e340 80 c6 13 | wsr.eps6 a8
X: 4011e3c0 80 c7 13 | wsr.eps7 a8
X: 4011e8c0 80 d1 13 | wsr.excsave1 a8
X: 4011e940 80 d2 13 | wsr.excsave2 a8
X: 4011e9c0 80 d3 13 | wsr.excsave3 a8
X: 4011ea40 80 d4 13 | wsr.excsave4 a8
X: 4011eac0 80 d5 13 | wsr.excsave5 a8
X: 4011eb40 80 d6 13 | wsr.excsave6 a8
X: 4011ebc0 80 d7 13 | wsr.excsave7 a8
X: 4011f040 80 e0 13 | wsr.cpenable a8
X: 4011f140 80 e2 13 | wsr.intset a8
X: 4011f1c0 80 e3 13 | wsr.intclear a8
X: 4011f240 80 e4 13 | wsr.intenable a8
X: 4011f340 80 e6 13 | wsr.ps a8
X: 4011f3c0 80 e7 13 | wsr.vecbase a8
X: 4011f440 80 e8 13 | wsr.exccause a8
X: 4011f4c0 80 e9 13 | wsr.debugcause a8
X: 4011f540 80 ea 13 | wsr.ccount a8
X: 4011f640 80 ec 13 | wsr.icount a8
X: 4011f6c0 80 ed 13 | wsr.icountlevel a8
X: 4011f740 80 ee 13 | wsr.excvaddr a8
X: 4011f840 80 f0 13 | wsr.ccompare0 a8
X: 4011f8c0 80 f1 13 | wsr.ccompare1 a8
X: 4011f940 80 f2 13 | wsr.ccompare2 a8
X: 4011fa40 80 f4 13 | wsr.misc0 a8
X: 4011fac0 80 f5 13 | wsr.misc1 a8
X: 4011fb40 80 f6 13 | wsr.misc2 a8
X: 4011fbc0 80 f7 13 | wsr.misc3 a8
X: 40140000 00 00 18 | lsxp f0, a0, a0
X: 40145b98 30 b7 18 | lsxp f11, a7, a3
X: 40146bd8 b0 d7 18 | lsxp f13, a7, a11
X: 40147ff8 f0 ff 18 | lsxp f15, a15, a15
X: 40150000 00 00 1a | sub.s f0, f0, f0
X: 401552b0 60 a5 1a | sub.s f10, f5, f6
X: 401559e0 c0 b3 1a | sub.s f11, f3, f12
X: 40157ff8 f0 ff 1a | sub.s f15, f15, f15
X: 40158000 00 00 1b | un.s b0, f0, f0
X: 4015c3f8 f0 87 1b | un.s b8, f7, f15
X: 4015ddc0 80 bb 1b | un.s b11, f11, f8
X: 4015fff8 f0 ff 1b | un.s b15, f15, f15
X: 40170000 00 00 1e | f64sexp a0, a0, a0
X: 401713b0 60 27 1e | f64sexp a2, a7, a6
X: 401778c8 90 f1 1e | f64sexp a15, a1, a9
X: 40177ff8 f0 ff 1e | f64sexp a15, a15, a15
X: 40180000 00 00 20 | or a0, a0, a0
X: 40180c18 30 18 20 | or a1, a8, a3
X: 401859a0 40 b3 20 | or a11, a3, a4
X: 40187ff8 f0 ff 20 | or a15, a15, a15
X: 400835a0 40 3f 31 | srai a3, a4, 31
X: 40086330 60 9a 31 | srai a9, a6, 26
X: 400895f8 f0 ff 31 | srai a15, a15, 31
X: 40188000 00 00 21 | srai a0, a0, 0
X: 40188dd8 b0 1b 21 | srai a1, a11, 11
X: 4018ded0 a0 bd 21 | srai a11, a10, 13
X: 40190000 00 00 22 | orb b0, b0, b0
X: 40195988 10 b3 22 | orb b11, b3, b1
X: 40197ea8 50 fd 22 | orb b15, b13, b5
X: 40197ff8 f0 ff 22 | orb b15, b15, b15
X: 40198000 00 00 23 | sext a0, a0, 7
X: 40199208 10 24 23 | sext a2, a4, 8
X: 4019ba48 90 74 23 | sext a7, a4, 16
X: 4019fff8 f0 ff 23 | sext a15, a15, 22
X: 401d0000 00 00 2a | mul.s f0, f0, f0
X: 401d0f68 d0 1e 2a | mul.s f1, f14, f13
X: 401d39f8 f0 73 2a | mul.s f7, f3, f15
X: 401d7ff8 f0 ff 2a | mul.s f15, f15, f15
X: 401d8000 00 00 2b | oeq.s b0, f0, f0
X: 401d9018 30 20 2b | oeq.s b2, f0, f3
X: 401dd768 d0 ae 2b | oeq.s b10, f14, f13
X: 401dfff8 f0 ff 2b | oeq.s b15, f15, f15
X: 400ea148 90 16 3e | f64norm a1, a6, a9, 1
X: 400eabe8 d0 2b 3e | f64norm a2, a11, a13, 1
X: 400ec270 e0 58 3e | f64norm a5, a8, a14, 1
X: 400f1568 d0 fe 3e | f64norm a15, a14, a13, 1
X: 400f15f8 f0 ff 3e | f64norm a15, a15, a15, 1
X: 401f0000 00 00 2e | f64norm a0, a0, a0, 0
X: 40080db8 70 ef 30 | xor a14, a15, a7
X: 400815f8 f0 ff 30 | xor a15, a15, a15
X: 40200000 00 00 30 | xor a0, a0, a0
X: 40202db0 60 5b 30 | xor a5, a11, a6
X: 40089600 00 00 32 | orbc b0, b0, b0
X: 4008c948 90 66 32 | orbc b6, b6, b9
X: 4008d568 d0 7e 32 | orbc b7, b14, b13
X: 400915f8 f0 ff 32 | orbc b15, b15, b15
X: 40091600 00 00 33 | clamps a0, a0, 7
X: 40094e60 c0 70 33 | clamps a7, a0, 19
X: 40097668 d0 c0 33 | clamps a12, a0, 20
X: 400995f8 f0 ff 33 | clamps a15, a15, 22
X: 400d1600 00 00 3b | ueq.s b0, f0, f0
X: 400d3758 b0 42 3b | ueq.s b4, f2, f11
X: 400d4e90 20 71 3b | ueq.s b7, f1, f2
X: 400d95f8 f0 ff 3b | ueq.s b15, f15, f15
X: 400f9600 00 00 40 | ssr a0
X: 400f9800 00 04 40 | ssr a4
X: 400f9d80 00 0f 40 | ssr a15
X: 400f9e00 00 10 40 | ssl a0
X: 400fa380 00 1b 40 | ssl a11
X: 400fa580 00 1f 40 | ssl a15
X: 400fa600 00 20 40 | ssa8l a0
X: 400fa800 00 24 40 | ssa8l a4
X: 400fad80 00 2f 40 | ssa8l a15
X: 400fae00 00 30 40 | ssa8b a0
X: 400fb000 00 34 40 | ssa8b a4
X: 400fb580 00 3f 40 | ssa8b a15
X: 400fb600 00 40 40 | ssai 0
X: 400fbd00 00 4e 40 | ssai 14
X: 400fbd88 10 4f 40 | ssai 31
X: 400fc600 00 60 40 | rer a0, a0
X: 400fca20 40 68 40 | rer a4, a8
X: 400fcba0 40 6b 40 | rer a4, a11
X: 400fcdf8 f0 6f 40 | rer a15, a15
X: 400fce00 00 70 40 | wer a0, a0
X: 400fceb8 70 71 40 | wer a7, a1
X: 400fd158 b0 76 40 | wer a11, a6
X: 400fd5f8 f0 7f 40 | wer a15, a15
X: 400fd600 00 80 40 | rotw 0
X: 400fd658 b0 80 40 | rotw -5
X: 400fd678 f0 80 40 | rotw -1
X: 40100600 00 e0 40 | nsa a0, a0
X: 401006c8 90 e1 40 | nsa a9, a1
X: 40100c18 30 ec 40 | nsa a3, a12
X: 40100df8 f0 ef 40 | nsa a15, a15
X: 40100e00 00 f0 40 | nsau a0, a0
X: 40100e78 f0 f0 40 | nsau a15, a0
X: 401011b8 70 f7 40 | nsau a7, a7
X: 401015f8 f0 ff 40 | nsau a15, a15
X: 40101600 00 00 41 | srli a0, a0, 0
X: 401036e0 c0 41 41 | srli a4, a12, 1
X: 40105210 20 78 41 | srli a7, a2, 8
X: 401095f8 f0 ff 41 | srli a15, a15, 15
X: 40109600 00 00 42 | xorb b0, b0, b0
X: 4010c230 60 58 42 | xorb b5, b8, b6
X: 401115d0 a0 ff 42 | xorb b15, b15, b10
X: 401115f8 f0 ff 42 | xorb b15, b15, b15
X: 40111600 00 00 43 | min a0, a0, a0
X: 40112d90 20 2f 43 | min a2, a15, a2
X: 40114948 90 66 43 | min a6, a6, a9
X: 401195f8 f0 ff 43 | min a15, a15, a15
X: 40139600 00 00 48 | ssx f0, a0, a0
X: 4013d078 f0 74 48 | ssx f7, a4, a15
X: 401409e8 d0 e7 48 | ssx f14, a7, a13
X: 401415f8 f0 ff 48 | ssx f15, a15, a15
X: 40141600 00 00 49 | s32e a0, a0, -64
X: 40145690 20 81 49 | s32e a2, a1, -32
X: 401492e8 d0 f9 49 | s32e a13, a9, -4
X: 401495f8 f0 ff 49 | s32e a15, a15, -4
X: 40149600 00 00 4a | madd.s f0, f0, f0
X: 4014d6c0 80 81 4a | madd.s f8, f1, f8
X: 4014d6d0 a0 81 4a | madd.s f8, f1, f10
X: 401515f8 f0 ff 4a | madd.s f15, f15, f15
X: 40151600 00 00 4b | olt.s b0, f0, f0
X: 40158770 e0 e2 4b | olt.s b14, f2, f14
X: 40159108 10 f6 4b | olt.s b15, f6, f1
X: 401595f8 f0 ff 4b | olt.s b15, f15, f15
X: 401647d8 b0 37 7e | f64rnd a3, a7, a11, 3
X: 40167018 30 88 7e | f64rnd a8, a8, a3, 3
X: 40169600 00 00 4e | f64rnd a0, a0, a0, 0
X: 4016abf8 f0 ff 7e | f64rnd a15, a15, a15, 3
X: 4016bc58 b0 4c 4e | f64rnd a4, a12, a11, 0
X: 4016bcf8 f0 4d 4e | f64rnd a4, a13, a15, 0
X: 4017ae00 00 30 50 | ritlb0 a0, a0
X: 4017b478 f0 3c 50 | ritlb0 a15, a12
X: 4017b5e8 d0 3f 50 | ritlb0 a13, a15
X: 4017b5f8 f0 3f 50 | ritlb0 a15, a15
X: 4017b600 00 40 50 | iitlb a0
X: 4017b900 00 46 50 | iitlb a6
X: 4017bd80 00 4f 50 | iitlb a15
X: 4017be00 00 50 50 | pitlb a0, a0
X: 4017c5a0 40 5f 50 | pitlb a4, a15
X: 4017c5d0 a0 5f 50 | pitlb a10, a15
X: 4017c5f8 f0 5f 50 | pitlb a15, a15
X: 4017c600 00 60 50 | witlb a0, a0
X: 4017c8e8 d0 65 50 | witlb a13, a5
X: 4017cd58 b0 6e 50 | witlb a11, a14
X: 4017cdf8 f0 6f 50 | witlb a15, a15
X: 4017ce00 00 70 50 | ritlb1 a0, a0
X: 4017cf38 70 72 50 | ritlb1 a7, a2
X: 4017d1b8 70 77 50 | ritlb1 a7, a7
X: 4017d5f8 f0 7f 50 | ritlb1 a15, a15
X: 4017ee00 00 b0 50 | rdtlb0 a0, a0
X: 4017f1d8 b0 b7 50 | rdtlb0 a11, a7
X: 4017f4e8 d0 bd 50 | rdtlb0 a13, a13
X: 4017f5f8 f0 bf 50 | rdtlb0 a15, a15
X: 4017f600 00 c0 50 | idtlb a0
X: 4017fc00 00 cc 50 | idtlb a12
X: 4017fd80 00 cf 50 | idtlb a15
X: 4017fe00 00 d0 50 | pdtlb a0, a0
X: 401801b8 70 d7 50 | pdtlb a7, a7
X: 40180458 b0 dc 50 | pdtlb a11, a12
X: 401805f8 f0 df 50 | pdtlb a15, a15
X: 40180600 00 e0 50 | wdtlb a0, a0
X: 40180838 70 e4 50 | wdtlb a7, a4
X: 40180d60 c0 ee 50 | wdtlb a12, a14
X: 40180df8 f0 ef 50 | wdtlb a15, a15
X: 40180e00 00 f0 50 | rdtlb1 a0, a0
X: 401810e0 c0 f5 50 | rdtlb1 a12, a5
X: 40181210 20 f8 50 | rdtlb1 a2, a8
X: 401815f8 f0 ff 50 | rdtlb1 a15, a15
X: 40191600 00 00 53 | max a0, a0, a0
X: 40192908 10 26 53 | max a2, a6, a1
X: 40195320 40 7a 53 | max a7, a10, a4
X: 401995f8 f0 ff 53 | max a15, a15, a15
X: 401b9600 00 00 58 | ssxp f0, a0, a0
X: 401bc6f8 f0 61 58 | ssxp f6, a1, a15
X: 401bdf88 10 93 58 | ssxp f9, a3, a1
X: 401c15f8 f0 ff 58 | ssxp f15, a15, a15
X: 401c1600 00 00 59 | s32nb a0, a0, 0
X: 401c5d48 90 8e 59 | s32nb a9, a14, 32
X: 401c7e98 30 d1 59 | s32nb a3, a1, 52
X: 401c95f8 f0 ff 59 | s32nb a15, a15, 60
X: 401c9600 00 00 5a | msub.s f0, f0, f0
X: 401cc730 60 62 5a | msub.s f6, f2, f6
X: 401d0a18 30 e8 5a | msub.s f14, f8, f3
X: 401d15f8 f0 ff 5a | msub.s f15, f15, f15
X: 401d1600 00 00 5b | ult.s b0, f0, f0
X: 401d17c0 80 03 5b | ult.s b0, f3, f8
X: 401d76c8 90 c1 5b | ult.s b12, f1, f9
X: 401d95f8 f0 ff 5b | ult.s b15, f15, f15
X: 401f9600 00 00 60 | neg a0, a0
X: 401fa658 b0 20 60 | neg a2, a11
X: 401ff618 30 c0 60 | neg a12, a3
X: 40200e78 f0 f0 60 | neg a15, a15
X: 401f9680 00 01 60 | abs a0, a0
X: 401fc680 00 61 60 | abs a6, a0
X: 401fcec0 80 71 60 | abs a7, a8
X: 40200ef8 f0 f1 60 | abs a15, a15
X: 40201640 80 00 61 | xsr.lbeg a8
X: 402016c0 80 01 61 | xsr.lend a8
X: 40201740 80 02 61 | xsr.lcount a8
X: 402017c0 80 03 61 | xsr.sar a8
X: 40201840 80 04 61 | xsr.br a8
X: 402018c0 80 05 61 | xsr.litbase a8
X: 40201c40 80 0c 61 | xsr.scompare1 a8
X: 40201e40 80 10 61 | xsr.acclo a8
X: 40201ec0 80 11 61 | xsr.acchi a8
X: 40202640 80 20 61 | xsr.m0 a8
X: 402026c0 80 21 61 | xsr.m1 a8
X: 40202740 80 22 61 | xsr.m2 a8
X: 402027c0 80 23 61 | xsr.m3 a8
X: 40203a40 80 48 61 | xsr.windowbase a8
X: 40203ac0 80 49 61 | xsr.windowstart a8
X: 40204640 80 60 61 | xsr.ibreakenable a8
X: 402046c0 80 61 61 | xsr.memctl a8
X: 402047c0 80 63 61 | xsr.atomctl a8
X: 40204a40 80 68 61 | xsr.ddr a8
X: 40205640 80 80 61 | xsr.ibreaka0 a8
X: 402056c0 80 81 61 | xsr.ibreaka1 a8
X: 40205e40 80 90 61 | xsr.dbreaka0 a8
X: 40205ec0 80 91 61 | xsr.dbreaka1 a8
X: 40206640 80 a0 61 | xsr.dbreakc0 a8
X: 402066c0 80 a1 61 | xsr.dbreakc1 a8
X: 400804c0 80 b1 61 | xsr.epc1 a8
X: 40080540 80 b2 61 | xsr.epc2 a8
X: 400805c0 80 b3 61 | xsr.epc3 a8
X: 40080640 80 b4 61 | xsr.epc4 a8
X: 400806c0 80 b5 61 | xsr.epc5 a8
X: 40080740 80 b6 61 | xsr.epc6 a8
X: 400807c0 80 b7 61 | xsr.epc7 a8
X: 40080c40 80 c0 61 | xsr.depc a8
X: 40080d40 80 c2 61 | xsr.eps2 a8
X: 40080dc0 80 c3 61 | xsr.eps3 a8
X: 40080e40 80 c4 61 | xsr.eps4 a8
X: 40080ec0 80 c5 61 | xsr.eps5 a8
X: 40080f40 80 c6 61 | xsr.eps6 a8
X: 40080fc0 80 c7 61 | xsr.eps7 a8
X: 400814c0 80 d1 61 | xsr.excsave1 a8
X: 40081540 80 d2 61 | xsr.excsave2 a8
X: 400815c0 80 d3 61 | xsr.excsave3 a8
X: 40081640 80 d4 61 | xsr.excsave4 a8
X: 400816c0 80 d5 61 | xsr.excsave5 a8
X: 40081740 80 d6 61 | xsr.excsave6 a8
X: 400817c0 80 d7 61 | xsr.excsave7 a8
X: 40081c40 80 e0 61 | xsr.cpenable a8
X: 40081e40 80 e4 61 | xsr.intenable a8
X: 40081f40 80 e6 61 | xsr.ps a8
X: 40081fc0 80 e7 61 | xsr.vecbase a8
X: 40082040 80 e8 61 | xsr.exccause a8
X: 400820c0 80 e9 61 | xsr.debugcause a8
X: 40082140 80 ea 61 | xsr.ccount a8
X: 40082240 80 ec 61 | xsr.icount a8
X: 400822c0 80 ed 61 | xsr.icountlevel a8
X: 40082340 80 ee 61 | xsr.excvaddr a8
X: 40082440 80 f0 61 | xsr.ccompare0 a8
X: 400824c0 80 f1 61 | xsr.ccompare1 a8
X: 40082540 80 f2 61 | xsr.ccompare2 a8
X: 40082640 80 f4 61 | xsr.misc0 a8
X: 400826c0 80 f5 61 | xsr.misc1 a8
X: 40082740 80 f6 61 | xsr.misc2 a8
X: 400827c0 80 f7 61 | xsr.misc3 a8
X: 4008ac00 00 00 63 | minu a0, a0, a0
X: 400908a8 50 b9 63 | minu a11, a9, a5
X: 40091ce8 d0 e1 63 | minu a14, a1, a13
X: 40092bf8 f0 ff 63 | minu a15, a15, a15
X: 400c2c00 00 00 6a | maddn.s f0, f0, f0
X: 400c74c0 80 91 6a | maddn.s f9, f1, f8
X: 400c7bb0 60 9f 6a | maddn.s f9, f15, f6
X: 400cabf8 f0 ff 6a | maddn.s f15, f15, f15
X: 400cac00 00 00 6b | ole.s b0, f0, f0
X: 400cf3c8 90 8f 6b | ole.s b8, f15, f9
X: 400cffe8 d0 a7 6b | ole.s b10, f7, f13
X: 400d2bf8 f0 ff 6b | ole.s b15, f15, f15
X: 4010ac00 00 00 73 | maxu a0, a0, a0
X: 4010c288 10 2d 73 | maxu a2, a13, a1
X: 40110ad8 b0 bd 73 | maxu a11, a13, a11
X: 40112bf8 f0 ff 73 | maxu a15, a15, a15
X: 40142c00 00 00 7a | divn.s f0, f0, f0
X: 40147698 30 95 7a | divn.s f9, f5, f3
X: 40148bf8 f0 bf 7a | divn.s f11, f15, f15
X: 4014abf8 f0 ff 7a | divn.s f15, f15, f15
X: 4014ac00 00 00 7b | ule.s b0, f0, f0
X: 4014b550 a0 12 7b | ule.s b1, f2, f10
X: 401508d8 b0 b9 7b | ule.s b11, f9, f11
X: 40152bf8 f0 ff 7b | ule.s b15, f15, f15
X: 40172c00 00 00 80 | add a0, a0, a0
X: 401733b8 70 0f 80 | add a0, a15, a7
X: 40177e90 20 a5 80 | add a10, a5, a2
X: 4017abf8 f0 ff 80 | add a15, a15, a15
X: 4017ac00 00 00 81 | src a0, a0, a0
X: 4017c088 10 29 81 | src a2, a9, a1
X: 4017fb80 00 9f 81 | src a9, a15, a0
X: 40182bf8 f0 ff 81 | src a15, a15, a15
X: 40182c00 00 00 82 | mull a0, a0, a0
X: 40183d30 60 22 82 | mull a2, a2, a6
X: 4018a7e0 c0 f7 82 | mull a15, a7, a12
X: 4018abf8 f0 ff 82 | mull a15, a15, a15
X: 4018ac00 00 00 83 | moveqz a0, a0, a0
X: 4018acf8 f0 01 83 | moveqz a0, a1, a15
X: 40191ec8 90 e5 83 | moveqz a14, a5, a9
X: 40192bf8 f0 ff 83 | moveqz a15, a15, a15
X: 401c2c00 00 00 8a | round.s a0, f0, 0
X: 401c5ea0 40 65 8a | round.s a6, f5, 4
X: 401c6580 00 73 8a | round.s a7, f3, 0
X: 401cabf8 f0 ff 8a | round.s a15, f15, 15
X: 401cac00 00 00 8b | moveqz.s f0, f0, a0
X: 401cd4a8 50 51 8b | moveqz.s f5, f1, a5
X: 401ce000 00 68 8b | moveqz.s f6, f8, a0
X: 401d2bf8 f0 ff 8b | moveqz.s f15, f15, a15
X: 400d4058 b0 a4 fe | f64iter a10, a4, a11, 3, 1
X: 400d6df8 f0 ff fe | f64iter a15, a15, a15, 3, 1
X: 400e2150 a0 be 9e | f64iter a11, a14, a10, 1, 0
X: 401608f8 f0 8d ae | f64iter a8, a13, a15, 2, 0
X: 401e2c00 00 00 8e | f64iter a0, a0, a0, 0, 0
X: 401e5740 80 56 8e | f64iter a5, a6, a8, 0, 0
X: 401f2c00 00 00 90 | addx2 a0, a0, a0
X: 401f49d0 a0 3b 90 | addx2 a3, a11, a10
X: 401f96d0 a0 d5 90 | addx2 a13, a5, a10
X: 401fabf8 f0 ff 90 | addx2 a15, a15, a15
X: 401fac00 00 00 91 | srl a0, a0
X: 401fd440 80 50 91 | srl a5, a8
X: 40202458 b0 f0 91 | srl a15, a11
X: 40202478 f0 f0 91 | srl a15, a15
X: 40084200 00 00 93 | movnez a0, a0, a0
X: 40088430 60 84 93 | movnez a8, a4, a6
X: 40089600 00 a8 93 | movnez a10, a8, a0
X: 4008c1f8 f0 ff 93 | movnez a15, a15, a15
X: 400bc200 00 00 9a | trunc.s a0, f0, 0
X: 400c34e0 c0 e5 9a | trunc.s a14, f5, 12
X: 400c3d58 b0 f6 9a | trunc.s a15, f6, 11
X: 400c41f8 f0 ff 9a | trunc.s a15, f15, 15
X: 400c4200 00 00 9b | movnez.s f0, f0, a0
X: 400c4b08 10 12 9b | movnez.s f1, f2, a1
X: 400c88b8 70 8d 9b | movnez.s f8, f13, a7
X: 400cc1f8 f0 ff 9b | movnez.s f15, f15, a15
X: 400ec200 00 00 a0 | addx4 a0, a0, a0
X: 400ef930 60 6e a0 | addx4 a6, a14, a6
X: 400f0478 f0 84 a0 | addx4 a8, a4, a15
X: 400f41f8 f0 ff a0 | addx4 a15, a15, a15
X: 400f4200 00 00 a1 | sll a0, a0
X: 400f4700 00 0a a1 | sll a0, a10
X: 400f7780 00 6b a1 | sll a6, a11
X: 400fc180 00 ff a1 | sll a15, a15
X: 400fc200 00 00 a2 | muluh a0, a0, a0
X: 400ffd28 50 76 a2 | muluh a7, a6, a5
X: 40103bf0 e0 f3 a2 | muluh a15, a3, a14
X: 401041f8 f0 ff a2 | muluh a15, a15, a15
X: 40104200 00 00 a3 | movltz a0, a0, a0
X: 40107088 10 5d a3 | movltz a5, a13, a1
X: 4010a470 e0 c4 a3 | movltz a12, a4, a14
X: 4010c1f8 f0 ff a3 | movltz a15, a15, a15
X: 4013c200 00 00 aa | floor.s a0, f0, 0
X: 401407e0 c0 8b aa | floor.s a8, f11, 12
X: 40142b10 20 d2 aa | floor.s a13, f2, 2
X: 401441f8 f0 ff aa | floor.s a15, f15, 15
X: 40144200 00 00 ab | movltz.s f0, f0, a0
X: 401451d8 b0 1f ab | movltz.s f1, f15, a11
X: 40145508 10 26 ab | movltz.s f2, f6, a1
X: 4014c1f8 f0 ff ab | movltz.s f15, f15, a15
X: 4016c200 00 00 b0 | addx8 a0, a0, a0
X: 4016c7d0 a0 0b b0 | addx8 a0, a11, a10
X: 40170558 b0 86 b0 | addx8 a8, a6, a11
X: 401741f8 f0 ff b0 | addx8 a15, a15, a15
X: 40174200 00 00 b1 | sra a0, a0
X: 40179250 a0 a0 b1 | sra a10, a10
X: 4017a278 f0 c0 b1 | sra a12, a15
X: 4017ba78 f0 f0 b1 | sra a15, a15
X: 4017c200 00 00 b2 | mulsh a0, a0, a0
X: 4017ebb8 70 53 b2 | mulsh a5, a3, a7
X: 40181548 90 a6 b2 | mulsh a10, a6, a9
X: 401841f8 f0 ff b2 | mulsh a15, a15, a15
X: 40184200 00 00 b3 | movgez a0, a0, a0
X: 4018a978 f0 ce b3 | movgez a12, a14, a15
X: 4018be10 20 f8 b3 | movgez a15, a8, a2
X: 4018c1f8 f0 ff b3 | movgez a15, a15, a15
X: 401bc200 00 00 ba | ceil.s a0, f0, 0
X: 401bd330 60 22 ba | ceil.s a2, f2, 6
X: 401c3ef0 e0 f9 ba | ceil.s a15, f9, 14
X: 401c41f8 f0 ff ba | ceil.s a15, f15, 15
X: 401c4200 00 00 bb | movgez.s f0, f0, a0
X: 401c4ff0 e0 1b bb | movgez.s f1, f11, a14
X: 401c74a0 40 65 bb | movgez.s f6, f5, a4
X: 401cc1f8 f0 ff bb | movgez.s f15, f15, a15
X: 401ec200 00 00 c0 | sub a0, a0, a0
X: 401f12b8 70 a1 c0 | sub a10, a1, a7
X: 401f2d60 c0 d6 c0 | sub a13, a6, a12
X: 401f41f8 f0 ff c0 | sub a15, a15, a15
X: 401f4200 00 00 c1 | mul16u a0, a0, a0
X: 401faaf0 e0 d1 c1 | mul16u a13, a1, a14
X: 401fbf98 30 fb c1 | mul16u a15, a11, a3
X: 401fc1f8 f0 ff c1 | mul16u a15, a15, a15
X: 401fc200 00 00 c2 | quou a0, a0, a0
X: 401fe168 d0 3e c2 | quou a3, a14, a13
X: 40201a00 00 b0 c2 | quou a11, a0, a0
X: 402041f8 f0 ff c2 | quou a15, a15, a15
X: 400847b0 60 df c3 | movf a13, a15, b6
X: 400857f8 f0 ff c3 | movf a15, a15, b15
X: 40204200 00 00 c3 | movf a0, a0, b0
X: 402068b8 70 4d c3 | movf a4, a13, b7
X: 400b5800 00 00 ca | float.s f0, a0, 0
X: 400b98a0 40 81 ca | float.s f8, a1, 4
X: 400bcbf0 e0 e7 ca | float.s f14, a7, 14
X: 400bd7f8 f0 ff ca | float.s f15, a15, 15
X: 400bd800 00 00 cb | movf.s f0, f0, b0
X: 400bfcc8 90 49 cb | movf.s f4, f9, b9
X: 400c21b8 70 93 cb | movf.s f9, f3, b7
X: 400c57f8 f0 ff cb | movf.s f15, f15, b15
X: 400e5800 00 00 d0 | subx2 a0, a0, a0
X: 400ea8a8 50 a1 d0 | subx2 a10, a1, a5
X: 400eb320 40 b6 d0 | subx2 a11, a6, a4
X: 400ed7f8 f0 ff d0 | subx2 a15, a15, a15
X: 400ed800 00 00 d1 | mul16s a0, a0, a0
X: 400f1040 80 70 d1 | mul16s a7, a0, a8
X: 400f5358 b0 f6 d1 | mul16s a15, a6, a11
X: 400f57f8 f0 ff d1 | mul16s a15, a15, a15
X: 400f5800 00 00 d2 | quos a0, a0, a0
X: 400fafa0 40 af d2 | quos a10, a15, a4
X: 400fc030 60 d0 d2 | quos a13, a0, a6
X: 400fd7f8 f0 ff d2 | quos a15, a15, a15
X: 400fd800 00 00 d3 | movt a0, a0, b0
X: 400fe150 a0 12 d3 | movt a1, a2, b10
X: 401046e0 c0 dd d3 | movt a13, a13, b12
X: 401057f8 f0 ff d3 | movt a15, a15, b15
X: 40135800 00 00 da | ufloat.s f0, a0, 0
X: 40138498 30 59 da | ufloat.s f5, a9, 3
X: 4013c568 d0 da da | ufloat.s f13, a10, 13
X: 4013d7f8 f0 ff da | ufloat.s f15, a15, 15
X: 4013d800 00 00 db | movt.s f0, f0, b0
X: 40140ab0 60 65 db | movt.s f6, f5, b6
X: 40142b58 b0 a6 db | movt.s f10, f6, b11
X: 401457f8 f0 ff db | movt.s f15, f15, b15
X: 40165800 00 00 e0 | subx4 a0, a0, a0
X: 40166308 10 16 e0 | subx4 a1, a6, a1
X: 4016b320 40 b6 e0 | subx4 a11, a6, a4
X: 4016d7f8 f0 ff e0 | subx4 a15, a15, a15
X: 40175800 00 00 e2 | remu a0, a0, a0
X: 40177f58 b0 4e e2 | remu a4, a14, a11
X: 40178cf0 e0 69 e2 | remu a6, a9, a14
X: 4017d7f8 f0 ff e2 | remu a15, a15, a15
X: 40181f30 60 8e e3 | rur.expstate a8
X: 40181f38 70 8e e3 | rur.threadptr a8
X: 40181f40 80 8e e3 | rur.fcr a8
X: 40181f48 90 8e e3 | rur.fsr a8
X: 40181f50 a0 8e e3 | rur.f64r_lo a8
X: 40181f58 b0 8e e3 | rur.f64r_hi a8
X: 40181f60 c0 8e e3 | rur.f64s a8
X: 401b5800 00 00 ea | utrunc.s a0, f0, 0
X: 401b6b78 f0 26 ea | utrunc.s a2, f6, 15
X: 401b7e00 00 4c ea | utrunc.s a4, f12, 0
X: 401bd7f8 f0 ff ea | utrunc.s a15, f15, 15
X: 401bd800 00 00 eb | f64cmpl a0, a0, a0
X: 401c3df8 f0 cb eb | f64cmpl a12, a11, a15
X: 401c46d8 b0 dd eb | f64cmpl a13, a13, a11
X: 401c57f8 f0 ff eb | f64cmpl a15, a15, a15
X: 401e5800 00 00 f0 | subx8 a0, a0, a0
X: 401e6238 70 14 f0 | subx8 a1, a4, a7
X: 401e7218 30 34 f0 | subx8 a3, a4, a3
X: 401ed7f8 f0 ff f0 | subx8 a15, a15, a15
X: 401f4800 00 e0 f1 | rfdo 0
X: 401f4e80 00 ed f1 | rfdo 13
X: 401f4f80 00 ef f1 | rfdo 15
X: 401f4808 10 e0 f1 | rfdd
X: 401f5800 00 00 f2 | rems a0, a0, a0
X: 401f85c8 90 5b f2 | rems a5, a11, a9
X: 401f9e00 00 8c f2 | rems a8, a12, a0
X: 401fd7f8 f0 ff f2 | rems a15, a15, a15
X: 40204b40 80 e6 f3 | wur.expstate a8
X: 40204bc0 80 e7 f3 | wur.threadptr a8
X: 40204c40 80 e8 f3 | wur.fcr a8
X: 40204cc0 80 e9 f3 | wur.fsr a8
X: 40204d40 80 ea f3 | wur.f64r_lo a8
X: 40204dc0 80 eb f3 | wur.f64r_hi a8
X: 40204e40 80 ec f3 | wur.f64s a8
X: 400aee00 00 00 fa | mov.s f0, f0
X: 400b6000 00 e4 fa | mov.s f14, f4
X: 400b6380 00 eb fa | mov.s f14, f11
X: 400b6d80 00 ff fa | mov.s f15, f15
X: 400aee08 10 00 fa | abs.s f0, f0
X: 400afb08 10 1a fa | abs.s f1, f10
X: 400b4f88 10 c3 fa | abs.s f12, f3
X: 400b6d88 10 ff fa | abs.s f15, f15
X: 400aee18 30 00 fa | const.s f0, 0
X: 400b2098 30 65 fa | const.s f6, 5
X: 400b4018 30 a4 fa | const.s f10, 4
X: 400b6d98 30 ff fa | const.s f15, 15
X: 400aee20 40 00 fa | rfr a0, f0
X: 400b3d20 40 9e fa | rfr a9, f14
X: 400b6a20 40 f8 fa | rfr a15, f8
X: 400b6da0 40 ff fa | rfr a15, f15
X: 400aee28 50 00 fa | wfr f0, a0
X: 400af4a8 50 0d fa | wfr f0, a13
X: 400b5ea8 50 e1 fa | wfr f14, a1
X: 400b6da8 50 ff fa | wfr f15, a15
X: 400aee30 60 00 fa | neg.s f0, f0
X: 400b0f30 60 42 fa | neg.s f4, f2
X: 400b37b0 60 93 fa | neg.s f9, f3
X: 400b6db0 60 ff fa | neg.s f15, f15
X: 400aee38 70 00 fa | div0.s f0, f0
X: 400b1eb8 70 61 fa | div0.s f6, f1
X: 400b3038 70 84 fa | div0.s f8, f4
X: 400b6db8 70 ff fa | div0.s f15, f15
X: 400aee40 80 00 fa | recip0.s f0, f0
X: 400af8c0 80 15 fa | recip0.s f1, f5
X: 400afac0 80 19 fa | recip0.s f1, f9
X: 400b6dc0 80 ff fa | recip0.s f15, f15
X: 400aee48 90 00 fa | sqrt0.s f0, f0
X: 400af0c8 90 05 fa | sqrt0.s f0, f5
X: 400b65c8 90 ef fa | sqrt0.s f14, f15
X: 400b6dc8 90 ff fa | sqrt0.s f15, f15
X: 400aee50 a0 00 fa | rsqrt0.s f0, f0
X: 400b0650 a0 30 fa | rsqrt0.s f3, f0
X: 400b0750 a0 32 fa | rsqrt0.s f3, f2
X: 400b6dd0 a0 ff fa | rsqrt0.s f15, f15
X: 400aee58 b0 00 fa | nexp01.s f0, f0
X: 400b4ed8 b0 c1 fa | nexp01.s f12, f1
X: 400b6a58 b0 f8 fa | nexp01.s f15, f8
X: 400b6dd8 b0 ff fa | nexp01.s f15, f15
X: 400aee60 c0 00 fa | mksadj.s f0, f0
X: 400af960 c0 16 fa | mksadj.s f1, f6
X: 400b12e0 c0 49 fa | mksadj.s f4, f9
X: 400b6de0 c0 ff fa | mksadj.s f15, f15
X: 400aee68 d0 00 fa | mkdadj.s f0, f0
X: 400b3468 d0 8c fa | mkdadj.s f8, f12
X: 400b61e8 d0 e7 fa | mkdadj.s f14, f7
X: 400b6de8 d0 ff fa | mkdadj.s f15, f15
X: 400aee70 e0 00 fa | addexp.s f0, f0
X: 400b0270 e0 28 fa | addexp.s f2, f8
X: 400b6af0 e0 f9 fa | addexp.s f15, f9
X: 400b6df0 e0 ff fa | addexp.s f15, f15
X: 400aee78 f0 00 fa | addexpm.s f0, f0
X: 400b2278 f0 68 fa | addexpm.s f6, f8
X: 400b49f8 f0 b7 fa | addexpm.s f11, f7
X: 400b6df8 f0 ff fa | addexpm.s f15, f15
X: 400b6e00 00 00 fb | f64addc a0, a0, 0, 0
X: 400ba760 c0 72 fb | f64addc a12, a2, 3, 1
X: 400baac8 90 79 fb | f64addc a9, a9, 3, 1
X: 400badf8 f0 7f fb | f64addc a15, a15, 3, 1
X: 400bae00 00 80 fb | f64subc a0, a0, 0, 0
X: 400bc298 30 a9 fb | f64subc a3, a9, 1, 0
X: 400bdc80 00 dd fb | f64subc a0, a13, 2, 1
X: 400bedf8 f0 ff fb | f64subc a15, a15, 3, 1
X: 400dee00 01 00 00 | l32r a0, 0x4009ee00
X: 400dee60 11 ff ff | l32r a1, 0x400dee5c
X: 400deec0 31 00 80 | l32r a3, 0x400beec0
X: 400df178 f1 09 e4 | l32r a15, 0x400d819c
X: 400df180 02 00 00 | l8ui a0, a0, 0
X: 40170078 f2 0e 49 | l8ui a15, a14, 73
X: 4018f208 82 00 db | l8ui a8, a0, 219
X: 4019f6e0 62 05 0c | l8ui a6, a5, 12
X: 401af8a8 22 07 2d | l8ui a2, a7, 45
X: 401d0178 f2 0f 1f | l8ui a15, a15, 31
X: 400e0180 02 10 00 | l16ui a0, a0, 0
X: 40120918 92 17 f4 | l16ui a9, a7, 0x1e8
X: 40130980 02 18 05 | l16ui a0, a8, 10
X: 40140fd8 52 1e 66 | l16ui a5, a14, 204
X: 401b0b50 d2 19 0d | l16ui a13, a9, 26
X: 401d1178 f2 1f 4f | l16ui a15, a15, 158
X: 400e1180 02 20 00 | l32i a0, a0, 0
X: 40111270 f2 20 03 | l32i a15, a0, 12
X: 40111700 82 25 03 | l32i a8, a5, 12
X: 40181530 b2 23 0a | l32i a11, a3, 40
X: 401d2160 e2 2f 0f | l32i a14, a15, 60
X: 401d2178 f2 2f df | l32i a15, a15, 0x37c
X: 400e3180 02 40 00 | s8i a0, a0, 0
X: 400f33a0 22 42 01 | s8i a2, a2, 1
X: 401a3ea0 22 4d 0c | s8i a2, a13, 12
X: 401b3db0 32 4c 0d | s8i a3, a12, 13
X: 401c3850 d2 46 0e | s8i a13, a6, 14
X: 401d4178 f2 4f 4f | s8i a15, a15, 79
X: 400e4180 02 50 00 | s16i a0, a0, 0
X: 40104878 f2 56 32 | s16i a15, a6, 100
X: 40104c88 02 5b 72 | s16i a0, a11, 228
X: 40194830 b2 56 0b | s16i a11, a6, 22
X: 40194b28 a2 59 2b | s16i a10, a9, 86
X: 401d5178 f2 5f 6f | s16i a15, a15, 222
X: 400e5180 02 60 00 | s32i a0, a0, 0
X: 40115978 f2 67 53 | s32i a15, a7, 0x14c
X: 401760c8 42 6f b9 | s32i a4, a15, 0x2e4
X: 401857d8 52 66 da | s32i a5, a6, 0x368
X: 401d54e0 62 63 0f | s32i a6, a3, 60
X: 401d6178 f2 6f 8f | s32i a15, a15, 0x23c
X: 400e8180 02 90 00 | l16si a0, a0, 0
X: 400e90b8 32 9f 40 | l16si a3, a15, 128
X: 40178a08 82 98 59 | l16si a8, a8, 178
X: 40198b00 82 99 0b | l16si a8, a9, 22
X: 40199168 e2 9f 7b | l16si a14, a15, 246
X: 401d9178 f2 9f ff | l16si a15, a15, 0x1fe
X: 400e9180 02 a0 00 | movi a0, 0
X: 40149440 c2 a2 06 | movi a12, 0x206
X: 40149928 a2 a7 16 | movi a10, 0x716
X: 401595a8 22 a4 17 | movi a2, 0x417
X: 40199968 e2 a7 eb | movi a14, 0x7eb
X: 401da178 f2 af 7f | movi a15, -129
X: 400ea180 02 b0 00 | l32ai a0, a0, 0
X: 4016a620 a2 b4 08 | l32ai a10, a4, 32
X: 4019a370 f2 b1 0b | l32ai a15, a1, 44
X: 401aac28 a2 ba 4c | l32ai a10, a10, 0x130
X: 401babb8 32 ba 1d | l32ai a3, a10, 116
X: 401db178 f2 bf ff | l32ai a15, a15, 0x3fc
X: 400eb180 02 c0 00 | addi a0, a0, 0
X: 400eb750 d2 c5 00 | addi a13, a5, 0
X: 400fbf28 a2 cd 81 | addi a10, a13, -127
X: 4013b300 82 c1 05 | addi a8, a1, 5
X: 4013bee8 62 cd 75 | addi a6, a13, 117
X: 401dc178 f2 cf ef | addi a15, a15, -17
X: 400ec180 02 d0 00 | addmi a0, a0, 0
X: 4013d060 e2 de 05 | addmi a14, a14, 0x500
X: 4014d0b8 32 df 56 | addmi a3, a15, 0x5600
X: 4015c398 12 d2 c7 | addmi a1, a2, 0xffffc700
X: 4015d0b0 32 df 07 | addmi a3, a15, 0x700
X: 401dd178 f2 df df | addmi a15, a15, 0xffffdf00
X: 400ed180 02 e0 00 | s32c1i a0, a0, 0
X: 400fd9e8 62 e8 81 | s32c1i a6, a8, 0x204
X: 4015ddf0 72 ec 07 | s32c1i a7, a12, 28
X: 4019ddb0 32 ec 0b | s32c1i a3, a12, 44
X: 401bd370 f2 e1 0d | s32c1i a15, a1, 52
X: 401de178 f2 ef 9f | s32c1i a15, a15, 0x27c
X: 400ee180 02 f0 00 | s32ri a0, a0, 0
X: 4011e2e0 62 f1 03 | s32ri a6, a1, 12
X: 4014e998 12 f8 d6 | s32ri a1, a8, 0x358
X: 401ae230 b2 f0 0c | s32ri a11, a0, 48
X: 401bf118 92 ff 1d | s32ri a9, a15, 116
X: 401df178 f2 ff 2f | s32ri a15, a15, 188
X: 401df180 03 00 00 | lsi f0, a0, 0
X: 401e02c8 83 0b ff | lsi f8, a11, 0x3fc
X: 401e0960 e3 0f 3a | lsi f14, a15, 232
X: 401e0978 f3 0f 44 | lsi f15, a15, 0x110
X: 401e5180 03 40 00 | ssi f0, a0, 0
X: 401e56a8 73 43 00 | ssi f7, a3, 0
X: 401e6398 13 4c 00 | ssi f1, a12, 0
X: 401e6978 f3 4f 1e | ssi f15, a15, 120
X: 401eb180 03 80 00 | lsip f0, a0, 0
X: 401ebb38 73 86 32 | lsip f7, a6, 200
X: 401ec1b0 c3 8a fb | lsip f12, a10, 0x3ec
X: 401ec978 f3 8f 1a | lsip f15, a15, 104
X: 401f1180 03 c0 00 | ssip f0, a0, 0
X: 401f14e0 43 c2 00 | ssip f4, a2, 0
X: 401f2258 33 cb 89 | ssip f3, a11, 0x224
X: 401f2978 f3 cf 46 | ssip f15, a15, 0x118
X: 400b0780 04 00 08 | mula.dd.ll.ldinc m0, a0, m0, m2
X: 400b2200 04 35 08 | mula.dd.ll.ldinc m3, a5, m0, m2
X: 400b2920 44 43 08 | mula.dd.ll.ldinc m0, a3, m1, m3
X: 400b4720 44 7f 08 | mula.dd.ll.ldinc m3, a15, m1, m3
X: 400b8780 04 00 09 | mula.dd.hl.ldinc m0, a0, m0, m2
X: 400b9280 04 16 09 | mula.dd.hl.ldinc m1, a6, m0, m2
X: 400b9d20 44 2b 09 | mula.dd.hl.ldinc m2, a11, m0, m3
X: 400bc720 44 7f 09 | mula.dd.hl.ldinc m3, a15, m1, m3
X: 400c0780 04 00 0a | mula.dd.lh.ldinc m0, a0, m0, m2
X: 400c1320 44 17 0a | mula.dd.lh.ldinc m1, a7, m0, m3
X: 400c25a0 44 3c 0a | mula.dd.lh.ldinc m3, a12, m0, m3
X: 400c4720 44 7f 0a | mula.dd.lh.ldinc m3, a15, m1, m3
X: 400c8780 04 00 0b | mula.dd.hh.ldinc m0, a0, m0, m2
X: 400c9a80 04 26 0b | mula.dd.hh.ldinc m2, a6, m0, m2
X: 400cc1a0 44 74 0b | mula.dd.hh.ldinc m3, a4, m1, m3
X: 400cc720 44 7f 0b | mula.dd.hh.ldinc m3, a15, m1, m3
X: 40130780 04 00 18 | mula.dd.ll.lddec m0, a0, m0, m2
X: 40131180 04 14 18 | mula.dd.ll.lddec m1, a4, m0, m2
X: 40131c20 44 29 18 | mula.dd.ll.lddec m2, a9, m0, m3
X: 40134720 44 7f 18 | mula.dd.ll.lddec m3, a15, m1, m3
X: 40138780 04 00 19 | mula.dd.hl.lddec m0, a0, m0, m2
X: 4013b020 44 51 19 | mula.dd.hl.lddec m1, a1, m1, m3
X: 4013b100 04 53 19 | mula.dd.hl.lddec m1, a3, m1, m2
X: 4013c720 44 7f 19 | mula.dd.hl.lddec m3, a15, m1, m3
X: 40140780 04 00 1a | mula.dd.lh.lddec m0, a0, m0, m2
X: 40141b00 04 27 1a | mula.dd.lh.lddec m2, a7, m0, m2
X: 40141f80 04 30 1a | mula.dd.lh.lddec m3, a0, m0, m2
X: 40144720 44 7f 1a | mula.dd.lh.lddec m3, a15, m1, m3
X: 40148780 04 00 1b | mula.dd.hh.lddec m0, a0, m0, m2
X: 4014bd80 04 6c 1b | mula.dd.hh.lddec m2, a12, m1, m2
X: 4014c600 04 7d 1b | mula.dd.hh.lddec m3, a13, m1, m2
X: 4014c720 44 7f 1b | mula.dd.hh.lddec m3, a15, m1, m3
X: 40190780 04 00 24 | mul.dd.ll m0, m2
X: 401927a0 44 40 24 | mul.dd.ll m1, m3
X: 40198780 04 00 25 | mul.dd.hl m0, m2
X: 4019a7a0 44 40 25 | mul.dd.hl m1, m3
X: 401a0780 04 00 26 | mul.dd.lh m0, m2
X: 401a27a0 44 40 26 | mul.dd.lh m1, m3
X: 401a8780 04 00 27 | mul.dd.hh m0, m2
X: 401aa7a0 44 40 27 | mul.dd.hh m1, m3
X: 401b0780 04 00 28 | mula.dd.ll m0, m2
X: 401b27a0 44 40 28 | mula.dd.ll m1, m3
X: 401b8780 04 00 29 | mula.dd.hl m0, m2
X: 401ba7a0 44 40 29 | mula.dd.hl m1, m3
X: 401c0780 04 00 2a | mula.dd.lh m0, m2
X: 401c27a0 44 40 2a | mula.dd.lh m1, m3
X: 401c8780 04 00 2b | mula.dd.hh m0, m2
X: 401ca7a0 44 40 2b | mula.dd.hh m1, m3
X: 401d0780 04 00 2c | muls.dd.ll m0, m2
X: 401d27a0 44 40 2c | muls.dd.ll m1, m3
X: 401d8780 04 00 2d | muls.dd.hl m0, m2
X: 401da7a0 44 40 2d | muls.dd.hl m1, m3
X: 401e0780 04 00 2e | muls.dd.lh m0, m2
X: 401e27a0 44 40 2e | muls.dd.lh m1, m3
X: 401e8780 04 00 2f | muls.dd.hh m0, m2
X: 401ea7a0 44 40 2f | muls.dd.hh m1, m3
X: 40089d80 04 00 34 | mul.ad.ll a0, m2
X: 4008a120 44 07 34 | mul.ad.ll a7, m3
X: 4008a520 44 0f 34 | mul.ad.ll a15, m3
X: 40091d80 04 00 35 | mul.ad.hl a0, m2
X: 40092120 44 07 35 | mul.ad.hl a7, m3
X: 40092520 44 0f 35 | mul.ad.hl a15, m3
X: 40099d80 04 00 36 | mul.ad.lh a0, m2
X: 4009a1a0 44 08 36 | mul.ad.lh a8, m3
X: 4009a520 44 0f 36 | mul.ad.lh a15, m3
X: 400a1d80 04 00 37 | mul.ad.hh a0, m2
X: 400a2420 44 0d 37 | mul.ad.hh a13, m3
X: 400a2520 44 0f 37 | mul.ad.hh a15, m3
X: 400a9d80 04 00 38 | mula.ad.ll a0, m2
X: 400a9da0 44 00 38 | mula.ad.ll a0, m3
X: 400aa520 44 0f 38 | mula.ad.ll a15, m3
X: 400b1d80 04 00 39 | mula.ad.hl a0, m2
X: 400b24a0 44 0e 39 | mula.ad.hl a14, m3
X: 400b2520 44 0f 39 | mula.ad.hl a15, m3
X: 400b9d80 04 00 3a | mula.ad.lh a0, m2
X: 400ba1a0 44 08 3a | mula.ad.lh a8, m3
X: 400ba520 44 0f 3a | mula.ad.lh a15, m3
X: 400c1d80 04 00 3b | mula.ad.hh a0, m2
X: 400c1fa0 44 04 3b | mula.ad.hh a4, m3
X: 400c2520 44 0f 3b | mula.ad.hh a15, m3
X: 400c9d80 04 00 3c | muls.ad.ll a0, m2
X: 400ca480 04 0e 3c | muls.ad.ll a14, m2
X: 400ca520 44 0f 3c | muls.ad.ll a15, m3
X: 400d1d80 04 00 3d | muls.ad.hl a0, m2
X: 400d2080 04 06 3d | muls.ad.hl a6, m2
X: 400d2520 44 0f 3d | muls.ad.hl a15, m3
X: 400d9d80 04 00 3e | muls.ad.lh a0, m2
X: 400da000 04 05 3e | muls.ad.lh a5, m2
X: 400da520 44 0f 3e | muls.ad.lh a15, m3
X: 400e1d80 04 00 3f | muls.ad.hh a0, m2
X: 400e2100 04 07 3f | muls.ad.hh a7, m2
X: 400e2520 44 0f 3f | muls.ad.hh a15, m3
X: 40129d80 04 00 48 | mula.da.ll.ldinc m0, a0, m0, a0
X: 4012a668 d4 11 48 | mula.da.ll.ldinc m1, a1, m0, a13
X: 4012ceb0 64 62 48 | mula.da.ll.ldinc m2, a2, m1, a6
X: 4012dd78 f4 7f 48 | mula.da.ll.ldinc m3, a15, m1, a15
X: 40131d80 04 00 49 | mula.da.hl.ldinc m0, a0, m0, a0
X: 40131e18 34 01 49 | mula.da.hl.ldinc m0, a1, m0, a3
X: 40131f88 14 04 49 | mula.da.hl.ldinc m0, a4, m0, a1
X: 40135d78 f4 7f 49 | mula.da.hl.ldinc m3, a15, m1, a15
X: 40139d80 04 00 4a | mula.da.lh.ldinc m0, a0, m0, a0
X: 4013a718 34 13 4a | mula.da.lh.ldinc m1, a3, m0, a3
X: 4013c3f0 e4 4c 4a | mula.da.lh.ldinc m0, a12, m1, a14
X: 4013dd78 f4 7f 4a | mula.da.lh.ldinc m3, a15, m1, a15
X: 40141d80 04 00 4b | mula.da.hh.ldinc m0, a0, m0, a0
X: 401435b0 64 30 4b | mula.da.hh.ldinc m3, a0, m0, a6
X: 401453c8 94 6c 4b | mula.da.hh.ldinc m2, a12, m1, a9
X: 40145d78 f4 7f 4b | mula.da.hh.ldinc m3, a15, m1, a15
X: 401a9d80 04 00 58 | mula.da.ll.lddec m0, a0, m0, a0
X: 401a9ff0 e4 04 58 | mula.da.ll.lddec m0, a4, m0, a14
X: 401ace20 44 61 58 | mula.da.ll.lddec m2, a1, m1, a4
X: 401add78 f4 7f 58 | mula.da.ll.lddec m3, a15, m1, a15
X: 401b1d80 04 00 59 | mula.da.hl.lddec m0, a0, m0, a0
X: 401b3208 14 29 59 | mula.da.hl.lddec m2, a9, m0, a1
X: 401b4ce8 d4 5e 59 | mula.da.hl.lddec m1, a14, m1, a13
X: 401b5d78 f4 7f 59 | mula.da.hl.lddec m3, a15, m1, a15
X: 401b9d80 04 00 5a | mula.da.lh.lddec m0, a0, m0, a0
X: 401bac78 f4 1d 5a | mula.da.lh.lddec m1, a13, m0, a15
X: 401bb000 04 25 5a | mula.da.lh.lddec m2, a5, m0, a0
X: 401bdd78 f4 7f 5a | mula.da.lh.lddec m3, a15, m1, a15
X: 401c1d80 04 00 5b | mula.da.hh.lddec m0, a0, m0, a0
X: 401c4850 a4 55 5b | mula.da.hh.lddec m1, a5, m1, a10
X: 401c5570 e4 6f 5b | mula.da.hh.lddec m2, a15, m1, a14
X: 401c5d78 f4 7f 5b | mula.da.hh.lddec m3, a15, m1, a15
X: 40083380 04 00 64 | mul.da.ll m0, a0
X: 400853f0 e4 40 64 | mul.da.ll m1, a14
X: 400853f8 f4 40 64 | mul.da.ll m1, a15
X: 4008b380 04 00 65 | mul.da.hl m0, a0
X: 4008b3f0 e4 00 65 | mul.da.hl m0, a14
X: 4008d3f8 f4 40 65 | mul.da.hl m1, a15
X: 40093380 04 00 66 | mul.da.lh m0, a0
X: 400933f0 e4 00 66 | mul.da.lh m0, a14
X: 400953f8 f4 40 66 | mul.da.lh m1, a15
X: 4009b380 04 00 67 | mul.da.hh m0, a0
X: 4009d3a0 44 40 67 | mul.da.hh m1, a4
X: 4009d3f8 f4 40 67 | mul.da.hh m1, a15
X: 400a3380 04 00 68 | mula.da.ll m0, a0
X: 400a33b0 64 00 68 | mula.da.ll m0, a6
X: 400a53f8 f4 40 68 | mula.da.ll m1, a15
X: 400ab380 04 00 69 | mula.da.hl m0, a0
X: 400ad3f0 e4 40 69 | mula.da.hl m1, a14
X: 400ad3f8 f4 40 69 | mula.da.hl m1, a15
X: 400b3380 04 00 6a | mula.da.lh m0, a0
X: 400b3388 14 00 6a | mula.da.lh m0, a1
X: 400b53f8 f4 40 6a | mula.da.lh m1, a15
X: 400bb380 04 00 6b | mula.da.hh m0, a0
X: 400bd3d8 b4 40 6b | mula.da.hh m1, a11
X: 400bd3f8 f4 40 6b | mula.da.hh m1, a15
X: 400c3380 04 00 6c | muls.da.ll m0, a0
X: 400c53a0 44 40 6c | muls.da.ll m1, a4
X: 400c53f8 f4 40 6c | muls.da.ll m1, a15
X: 400cb380 04 00 6d | muls.da.hl m0, a0
X: 400cb3d8 b4 00 6d | muls.da.hl m0, a11
X: 400cd3f8 f4 40 6d | muls.da.hl m1, a15
X: 400d3380 04 00 6e | muls.da.lh m0, a0
X: 400d53b8 74 40 6e | muls.da.lh m1, a7
X: 400d53f8 f4 40 6e | muls.da.lh m1, a15
X: 400db380 04 00 6f | muls.da.hh m0, a0
X: 400dd380 04 40 6f | muls.da.hh m1, a0
X: 400dd3f8 f4 40 6f | muls.da.hh m1, a15
X: 400e3380 04 00 70 | umul.aa.ll a0, a0
X: 400e3398 34 00 70 | umul.aa.ll a0, a3
X: 400e3840 84 09 70 | umul.aa.ll a9, a8
X: 400e3b78 f4 0f 70 | umul.aa.ll a15, a15
X: 400eb380 04 00 71 | umul.aa.hl a0, a0
X: 400eb408 14 01 71 | umul.aa.hl a1, a1
X: 400eb5c8 94 04 71 | umul.aa.hl a4, a9
X: 400ebb78 f4 0f 71 | umul.aa.hl a15, a15
X: 400f3380 04 00 72 | umul.aa.lh a0, a0
X: 400f36e0 c4 06 72 | umul.aa.lh a6, a12
X: 400f39e0 c4 0c 72 | umul.aa.lh a12, a12
X: 400f3b78 f4 0f 72 | umul.aa.lh a15, a15
X: 400fb380 04 00 73 | umul.aa.hh a0, a0
X: 400fb538 74 03 73 | umul.aa.hh a3, a7
X: 400fb9d0 a4 0c 73 | umul.aa.hh a12, a10
X: 400fbb78 f4 0f 73 | umul.aa.hh a15, a15
X: 40103380 04 00 74 | mul.aa.ll a0, a0
X: 40103458 b4 01 74 | mul.aa.ll a1, a11
X: 401035b8 74 04 74 | mul.aa.ll a4, a7
X: 40103b78 f4 0f 74 | mul.aa.ll a15, a15
X: 4010b380 04 00 75 | mul.aa.hl a0, a0
X: 4010b580 04 04 75 | mul.aa.hl a4, a0
X: 4010b8c8 94 0a 75 | mul.aa.hl a10, a9
X: 4010bb78 f4 0f 75 | mul.aa.hl a15, a15
X: 40113380 04 00 76 | mul.aa.lh a0, a0
X: 40113508 14 03 76 | mul.aa.lh a3, a1
X: 40113510 24 03 76 | mul.aa.lh a3, a2
X: 40113b78 f4 0f 76 | mul.aa.lh a15, a15
X: 4011b380 04 00 77 | mul.aa.hh a0, a0
X: 4011b948 94 0b 77 | mul.aa.hh a11, a9
X: 4011ba68 d4 0d 77 | mul.aa.hh a13, a13
X: 4011bb78 f4 0f 77 | mul.aa.hh a15, a15
X: 40123380 04 00 78 | mula.aa.ll a0, a0
X: 40123758 b4 07 78 | mula.aa.ll a7, a11
X: 40123b20 44 0f 78 | mula.aa.ll a15, a4
X: 40123b78 f4 0f 78 | mula.aa.ll a15, a15
X: 4012b380 04 00 79 | mula.aa.hl a0, a0
X: 4012b418 34 01 79 | mula.aa.hl a1, a3
X: 4012b9f8 f4 0c 79 | mula.aa.hl a12, a15
X: 4012bb78 f4 0f 79 | mula.aa.hl a15, a15
X: 40133380 04 00 7a | mula.aa.lh a0, a0
X: 401339a8 54 0c 7a | mula.aa.lh a12, a5
X: 40133b50 a4 0f 7a | mula.aa.lh a15, a10
X: 40133b78 f4 0f 7a | mula.aa.lh a15, a15
X: 4013b380 04 00 7b | mula.aa.hh a0, a0
X: 4013b5c0 84 04 7b | mula.aa.hh a4, a8
X: 4013b668 d4 05 7b | mula.aa.hh a5, a13
X: 4013bb78 f4 0f 7b | mula.aa.hh a15, a15
X: 40143380 04 00 7c | muls.aa.ll a0, a0
X: 40143420 44 01 7c | muls.aa.ll a1, a4
X: 40143620 44 05 7c | muls.aa.ll a5, a4
X: 40143b78 f4 0f 7c | muls.aa.ll a15, a15
X: 4014b380 04 00 7d | muls.aa.hl a0, a0
X: 4014b768 d4 07 7d | muls.aa.hl a7, a13
X: 4014b980 04 0c 7d | muls.aa.hl a12, a0
X: 4014bb78 f4 0f 7d | muls.aa.hl a15, a15
X: 40153380 04 00 7e | muls.aa.lh a0, a0
X: 40153450 a4 01 7e | muls.aa.lh a1, a10
X: 40153830 64 09 7e | muls.aa.lh a9, a6
X: 40153b78 f4 0f 7e | muls.aa.lh a15, a15
X: 4015b380 04 00 7f | muls.aa.hh a0, a0
X: 4015b3b8 74 00 7f | muls.aa.hh a0, a7
X: 4015b9c8 94 0c 7f | muls.aa.hh a12, a9
X: 4015bb78 f4 0f 7f | muls.aa.hh a15, a15
X: 40163380 04 00 80 | ldinc m0, a0
X: 40164d80 04 34 80 | ldinc m3, a4
X: 40165300 04 3f 80 | ldinc m3, a15
X: 401e3380 04 00 90 | lddec m0, a0
X: 401e3b00 04 0f 90 | lddec m0, a15
X: 401e5300 04 3f 90 | lddec m3, a15
X: 400cf580 05 00 00 | call0 0x400cf584
X: 400cf598 05 00 80 | call0 0x4004f59c
X: 400cf5a8 45 9d ac | call0 0x4007bf80
X: 400cf5b0 15 00 00 | call4 0x400cf5b4
X: 400cf5b8 55 00 00 | call4 0x400cf5c0
X: 400cf5d8 d5 fc a0 | call4 0x400705a8
X: 400cf5e0 25 00 00 | call8 0x400cf5e4
X: 400cf5e8 65 00 00 | call8 0x400cf5f0
X: 400cf608 a5 ca 5a | call8 0x4012a2b4
X: 400cf610 35 00 00 | call12 0x400cf614
X: 400cf628 35 00 80 | call12 0x4004f62c
X: 400cf638 35 67 55 | call12 0x40124cac
X: 400cf640 06 00 00 | j 0x400cf644
X: 400dce90 86 56 80 | j 0x400bcfee
X: 400df7d8 06 67 ff | j 0x400df578
X: 400f25c0 c6 df 32 | j 0x400ff143
X: 400f4450 06 ec 80 | j 0x400d4804
X: 400f75c0 c6 ff 77 | j 0x401155c3
X: 400cf668 16 00 00 | beqz a0, 0x400cf66c
X: 400e9800 16 a7 ff | beqz a7, 0x400e97fe
X: 400f2168 16 de 00 | beqz a14, 0x400f2179
X: 400f7408 16 ff 93 | beqz a15, 0x400f6d4b
X: 400cf690 26 00 00 | beqi a0, -1, 0x400cf694
X: 400dc898 26 54 7f | beqi a4, 5, 0x400dc91b
X: 400e2ca0 26 7c 80 | beqi a12, 7, 0x400e2c24
X: 400f7430 26 ff 1a | beqi a15, 0x100, 0x400f744e
X: 400cf6b8 36 00 00 | entry a0, 0
X: 400e22d0 36 78 ff | entry a8, 0x7fb8
X: 400ee5d0 36 c6 ff | entry a6, 0x7fe0
X: 400f7458 36 ff db | entry a15, 0x6df8
X: 400cf708 56 00 00 | bnez a0, 0x400cf70c
X: 400dfd88 56 69 00 | bnez a9, 0x400dfd92
X: 400ed208 56 be 00 | bnez a14, 0x400ed217
X: 400f74a8 56 ff 78 | bnez a15, 0x400f7c3b
X: 400cf730 66 00 00 | bnei a0, -1, 0x400cf734
X: 400d3340 66 18 80 | bnei a8, 1, 0x400d32c4
X: 400e4138 66 84 7f | bnei a4, 8, 0x400e41bb
X: 400f74d0 66 ff 2d | bnei a15, 0x100, 0x400f7501
X: 400cf758 76 00 00 | bf b0, 0x400cf75c
X: 400d0df0 76 09 ff | bf b9, 0x400d0df3
X: 400d1578 76 0c e8 | bf b12, 0x400d1564
X: 400d1cf8 76 0f 53 | bf b15, 0x400d1d4f
X: 400cf7a8 96 00 00 | bltz a0, 0x400cf7ac
X: 400d65b0 96 2c 7f | bltz a12, 0x400d6da6
X: 400d8dc8 96 3c 78 | bltz a12, 0x400d954f
X: 400f7548 96 ff 17 | bltz a15, 0x400f76cb
X: 400cf7d0 a6 00 00 | blti a0, -1, 0x400cf7d4
X: 400d3650 a6 19 00 | blti a9, 1, 0x400d3654
X: 400d47e0 a6 20 80 | blti a0, 2, 0x400d4764
X: 400f7570 a6 ff cb | blti a15, 0x100, 0x400f753f
X: 400cf7f8 b6 00 00 | bltui a0, 0x8000, 0x400cf7fc
X: 400d5c00 b6 28 7f | bltui a8, 2, 0x400d5c83
X: 400f0288 b6 d1 80 | bltui a1, 64, 0x400f020c
X: 400f7598 b6 ff f1 | bltui a15, 0x100, 0x400f758d
X: 400cf848 d6 00 00 | bgez a0, 0x400cf84c
X: 400d5258 d6 24 80 | bgez a4, 0x400d4a5e
X: 400e5b58 d6 8e 80 | bgez a14, 0x400e5364
X: 400f75e8 d6 ff fd | bgez a15, 0x400f75cb
X: 400cf870 e6 00 00 | bgei a0, -1, 0x400cf874
X: 400d6178 e6 2a 7f | bgei a10, 2, 0x400d61fb
X: 400dd708 e6 59 ff | bgei a9, 5, 0x400dd70b
X: 400f7610 e6 ff 2b | bgei a15, 0x100, 0x400f763f
X: 400cf898 f6 00 00 | bgeui a0, 0x8000, 0x400cf89c
X: 400d7828 f6 33 80 | bgeui a3, 3, 0x400d77ac
X: 400ebfb8 f6 b6 e4 | bgeui a6, 16, 0x400ebfa0
X: 400f7638 f6 ff cc | bgeui a15, 0x100, 0x400f7608
X: 400d1f58 76 10 00 | bt b0, 0x400d1f5c
X: 400d26e0 76 13 7f | bt b3, 0x400d2763
X: 400d2958 76 14 00 | bt b4, 0x400d295c
X: 400d44f8 76 1f 37 | bt b15, 0x400d4533
X: 400e3758 76 80 00 | loop a0, 0x400e375c
X: 400e3c70 76 82 ff | loop a2, 0x400e3d73
X: 400e5060 76 8a 7f | loop a10, 0x400e50e3
X: 400e5cf8 76 8f ec | loop a15, 0x400e5de8
X: 400e5f58 76 90 00 | loopnez a0, 0x400e5f5c
X: 400e61e8 76 91 80 | loopnez a1, 0x400e626c
X: 400e7fd8 76 9d 00 | loopnez a13, 0x400e7fdc
X: 400e84f8 76 9f 44 | loopnez a15, 0x400e8540
X: 400e8758 76 a0 00 | loopgtz a0, 0x400e875c
X: 400e9170 76 a4 ff | loopgtz a4, 0x400e9273
X: 400e9df8 76 a9 e3 | loopgtz a9, 0x400e9edf
X: 400eacf8 76 af 08 | loopgtz a15, 0x400ead04
X: 400f7640 07 00 00 | bnone a0, a0, 0x400f7644
X: 400f85b8 37 06 00 | bnone a6, a3, 0x400f85bc
X: 400f9a88 87 0e 7f | bnone a14, a8, 0x400f9b0b
X: 400f9e38 f7 0f f4 | bnone a15, a15, 0x400f9e30
X: 400f9e40 07 10 00 | beq a0, a0, 0x400f9e44
X: 400fb3a0 87 18 10 | beq a8, a8, 0x400fb3b4
X: 400fbb60 a7 1b 80 | beq a11, a10, 0x400fbae4
X: 400fc638 f7 1f 30 | beq a15, a15, 0x400fc66c
X: 400fc640 07 20 00 | blt a0, a0, 0x400fc644
X: 400fcf60 a7 23 80 | blt a3, a10, 0x400fcee4
X: 400fcf78 b7 23 00 | blt a3, a11, 0x400fcf7c
X: 400fee38 f7 2f 01 | blt a15, a15, 0x400fee3d
X: 400fee40 07 30 00 | bltu a0, a0, 0x400fee44
X: 400ff898 27 34 7f | bltu a4, a2, 0x400ff91b
X: 40101300 b7 3e 7f | bltu a14, a11, 0x40101383
X: 40101638 f7 3f af | bltu a15, a15, 0x401015eb
X: 40101640 07 40 00 | ball a0, a0, 0x40101644
X: 40101d30 c7 42 80 | ball a2, a12, 0x40101cb4
X: 40103458 07 4c ff | ball a12, a0, 0x4010345b
X: 40103e38 f7 4f d3 | ball a15, a15, 0x40103e0f
X: 40103e40 07 50 00 | bbc a0, a0, 0x40103e44
X: 401058e0 a7 5a 80 | bbc a10, a10, 0x40105864
X: 40105e80 e7 5c 80 | bbc a12, a14, 0x40105e04
X: 40106638 f7 5f 6f | bbc a15, a15, 0x401066ab
X: 40106640 07 60 00 | bbci a0, 0, 0x40106644
X: 40108210 27 6b 00 | bbci a11, 2, 0x40108214
X: 401090d8 07 71 ff | bbci a1, 16, 0x401090db
X: 4010b638 f7 7f f5 | bbci a15, 31, 0x4010b631
X: 4010b640 07 80 00 | bany a0, a0, 0x4010b644
X: 4010c760 d7 86 ff | bany a6, a13, 0x4010c763
X: 4010cab0 27 88 27 | bany a8, a2, 0x4010cadb
X: 4010de38 f7 8f e6 | bany a15, a15, 0x4010de22
X: 4010de40 07 90 00 | bne a0, a0, 0x4010de44
X: 4010ef78 e7 96 7f | bne a6, a14, 0x4010effb
X: 40110530 97 9f 7f | bne a15, a9, 0x401105b3
X: 40110638 f7 9f cd | bne a15, a15, 0x40110609
X: 40110640 07 a0 00 | bge a0, a0, 0x40110644
X: 40110d40 c7 a2 94 | bge a2, a12, 0x40110cd8
X: 40112000 47 aa 72 | bge a10, a4, 0x40112076
X: 40112e38 f7 af fd | bge a15, a15, 0x40112e39
X: 40112e40 07 b0 00 | bgeu a0, a0, 0x40112e44
X: 40113260 a7 b1 80 | bgeu a1, a10, 0x401131e4
X: 40114bb8 c7 bb ff | bgeu a11, a12, 0x40114bbb
X: 40115638 f7 bf 5d | bgeu a15, a15, 0x40115699
X: 40115640 07 c0 00 | bnall a0, a0, 0x40115644
X: 40115ed8 77 c3 00 | bnall a3, a7, 0x40115edc
X: 401176b0 f7 cc ff | bnall a12, a15, 0x401176b3
X: 40117e38 f7 cf 0b | bnall a15, a15, 0x40117e47
X: 40117e40 07 d0 00 | bbs a0, a0, 0x40117e44
X: 40118208 87 d1 7f | bbs a1, a8, 0x4011828b
X: 40118970 77 d4 ff | bbs a4, a7, 0x40118973
X: 4011a638 f7 df 48 | bbs a15, a15, 0x4011a684
X: 4011a640 07 e0 00 | bbsi a0, 0, 0x4011a644
X: 4011c690 e7 ec e9 | bbsi a12, 14, 0x4011c67d
X: 4011e210 e7 f7 18 | bbsi a7, 30, 0x4011e22c
X: 4011f638 f7 ff 4e | bbsi a15, 31, 0x4011f68a
X: 4011f640 08 00 | l32i.n a0, a0, 0
X: 40120e88 98 30 | l32i.n a9, a0, 12
X: 40124110 a8 95 | l32i.n a10, a5, 36
X: 40127638 f8 ff | l32i.n a15, a15, 60
X: 40127640 09 00 | s32i.n a0, a0, 0
X: 40129798 b9 42 | s32i.n a11, a2, 16
X: 40129c70 69 4c | s32i.n a6, a12, 16
X: 4012f638 f9 ff | s32i.n a15, a15, 60
X: 4012f640 0a 00 | add.n a0, a0, a0
X: 40133080 8a 74 | add.n a7, a4, a8
X: 40133a50 2a 88 | add.n a8, a8, a2
X: 40137638 fa ff | add.n a15, a15, a15
X: 40137640 0b 00 | addi.n a0, a0, -1
X: 40138f60 4b 32 | addi.n a3, a2, 4
X: 4013f378 7b fa | addi.n a15, a10, 7
X: 4013f638 fb ff | addi.n a15, a15, 15
X: 4013f640 0c 00 | movi.n a0, 0
X: 40143ad0 2c 89 | movi.n a9, 40
X: 40146958 3c e6 | movi.n a6, 62
X: 401475f8 7c ff | movi.n a15, -1
X: 4013f680 8c 00 | beqz.n a0, 0x4013f684
X: 4013fc18 bc 0b | beqz.n a11, 0x4013fc4c
X: 40146480 8c dc | beqz.n a12, 0x40146491
X: 40147618 bc ff | beqz.n a15, 0x4014765b
X: 4013f6a0 cc 00 | bnez.n a0, 0x4013f6a4
X: 401440b0 ec 94 | bnez.n a4, 0x401440dd
X: 40144820 cc a3 | bnez.n a3, 0x4014482e
X: 40147638 fc ff | bnez.n a15, 0x4014767b
X: 40147640 0d 00 | mov.n a0, a0
X: 40147700 8d 01 | mov.n a8, a1
X: 40147b98 bd 0a | mov.n a11, a10
X: 40147e38 fd 0f | mov.n a15, a15
X: 4014ee40 0d f0 | ret.n
X: 4014ee48 1d f0 | retw.n
X: 4014ee50 2d f0 | break.n 0
X: 4014f250 2d f8 | break.n 8
X: 4014f5d0 2d ff | break.n 15
X: 4014ee58 3d f0 | nop.n
X: 4014ee70 6d f0 | ill.n
X: 400864b8 70 c9 00 | .byte 0x70, 0xc9, 0x0
X: 400f5cf0 e0 61 70 | .byte 0xe0, 0x61, 0x70
X: 40102408 10 f0 71 | .byte 0x10, 0xf0, 0x71
X: 401ee520 40 1a f1 | .byte 0x40, 0x1a, 0xf1
X: 400ca488 10 1d 3a | .byte 0x10, 0x1d, 0x3a
X: 400cc218 30 58 3a | .byte 0x30, 0x58, 0x3a
X: 400d8390 20 07 0b | .byte 0x20, 0x7, 0xb
X: 400dc340 80 86 0b | .byte 0x80, 0x86, 0xb
X: 40157d88 10 a3 7c | .byte 0x10, 0xa3, 0x7c
X: 4015c7b8 70 63 4c | .byte 0x70, 0x63, 0x4c
X: 400ce480 00 19 cd | .byte 0x0, 0x19, 0xcd
X: 401678e0 c0 c5 4d | .byte 0xc0, 0xc5, 0x4d
X: 400f5990 20 b3 0e | .byte 0x20, 0xb3, 0xe
X: 400f7170 e0 e2 0e | .byte 0xe0, 0xe2, 0xe
X: 40080c90 20 95 92 | .byte 0x20, 0x95, 0x92
X: 401915a0 40 ff 52 | .byte 0x40, 0xff, 0x52
X: 40118bc8 90 67 d6 | .byte 0x90, 0x67, 0xd6
X: 4011cce8 d0 15 a6 | .byte 0xd0, 0x15, 0xa6
X: 40133e18 30 50 47 | .byte 0x30, 0x50, 0x47
X: 401a62d8 b0 41 b7 | .byte 0xb0, 0x41, 0xb7
X: 40125be8 d0 07 d8 | .byte 0xd0, 0x7, 0xd8
X: 401b40a0 40 29 88 | .byte 0x40, 0x29, 0x88
X: 400b2e98 30 ad c9 | .byte 0x30, 0xad, 0xc9
X: 401b0bd8 b0 67 e9 | .byte 0xb0, 0x67, 0xe9
X: 4014a388 9d 5a | .byte 0x9d, 0x5a
X: 4014bc60 4d 8c | .byte 0x4d, 0x8c
X: 40153830 ee | .byte 0xee
X: 40156ec0 0e | .byte 0xe
X: 4015bde0 4f | .byte 0x4f
X: 4015eff8 7f | .byte 0x7f
X: 401830b8 32 3f ca | .byte 0x32, 0x3f, 0xca
X: 401d7578 f2 83 ff | .byte 0xf2, 0x83, 0xff
X: 401e28c0 d3 24 ff | .byte 0xd3, 0x24, 0xff
X: 401e74c8 83 57 ff | .byte 0x83, 0x57, 0xff
X: 401910f0 e4 e6 54 | .byte 0xe4, 0xe6, 0x54
X: 401cfc48 94 bd 5c | .byte 0x94, 0xbd, 0x5c
X: 400f1878 76 da 7a | .byte 0x76, 0xda, 0x7a
X: 400f6378 76 f8 3c | .byte 0x76, 0xf8, 0x3c
X: 4009a680 00 c9 95 | extui a12, a0, 25, 10
X: 401a33e0 c0 3b 55 | extui a3, a12, 27, 6
X: 400996e0 c0 2d 03 | rsr a12, 45
X: 4009af80 00 5f 03 | rsr a0, 95
X: 4017efa8 50 2f e3 | rur a2, 245
X: 4017f288 10 35 e3 | rur a3, 81
X: 4011c3b8 70 87 13 | wsr a7, 135
X: 4011cf90 20 9f 13 | wsr a2, 159
X: 401ffe10 20 4c f3 | wur a2, 76
X: 40205190 20 f3 f3 | wur a2, 243
X: 40082960 c0 fa 61 | xsr a12, 250
X: 402068c8 90 a5 61 | xsr a9, 165

\ The ESP32-S3: op0 4 is the PIE (MAC16 on the ESP32), and op0 14 and 15
\ start instructions of 4 bytes (PIE); the other instructions are those
\ of the ESP32.
XT-ESP32S3
X: 40080000 36 41 00 | entry a1, 32
X: 40080003 04 00 08 | .byte 0x4, 0x0, 0x8
X: 40080006 c4 cf 24 | .byte 0xc4, 0xcf, 0x24
X: 40080009 0e 12 34 56 | .byte 0xe, 0x12, 0x34, 0x56
X: 4008000d 1f d3 0a e1 | .byte 0x1f, 0xd3, 0xa, 0xe1
X: 40080011 0d f0 | ret.n
XT-ESP32
X: 40080003 04 00 08 | mula.dd.ll.ldinc m0, a0, m0, m2
X: 40080009 0e | .byte 0xe

S" disxt" .RESULTS
