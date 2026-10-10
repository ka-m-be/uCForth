\ disa64.fs - tests of the package DISA64 (asm/monitor/disa64.fs)
\
\ Load (in the folder ucf), after the package FILES and the tester:
\   build/ucf-c -32 host/files.fs forth/test/tester.fs asm/monitor/test/disa64.fs
\ The tests need cells of 32 bits.
\
\ Each line A: is one instruction: its address, the instruction as a
\ number of 32 bits, "|", and the text that DECODE must give. The texts
\ come from llvm-objdump 21 (Apple LLVM, the triple aarch64-none-elf; it
\ knows all extensions), once, and are kept here as data. The
\ instructions: each line of the table of DISA64 with 64 to 300 values
\ of its fields and with each of its fixed bits changed; all
\ combinations of the fields of the bit fields, logical immediates,
\ shifts, extensions, conditions and loads; all numbers of the system
\ registers for MRS, MSR, SYS and SYSL; 1.4 million other numbers; the
\ code of uCForth for Linux (arm64) and for the Raspberry Pi:
\ 2 151 621 instructions, checked once against llvm-objdump without an
\ open difference. These lines are a choice: each line of the table,
\ each hint, barriers, the fields of PSTATE, system registers and
\ operations, immediates, shifts, extensions, and each kind of
\ instruction that DISA64 does not know. Where this disassembler writes
\ otherwise (asm/monitor/README.md, "AArch64"), the text here is changed:
\
\   - .inst and the number for an instruction that DISA64 does not know
\     (llvm-objdump: its text, or <unknown> where there is none):
\     floating point and SIMD (but not their loads and stores), SVE,
\     SME (but SMSTART and SMSTOP), MOPS, and the extensions after
\     ARMv8.7 (CSSC, RPRFM, THE, D128, RCPC3, LSE128, GCS, BRBE, CPA,
\     PAuth_LR, LSUI, CMPBR, LSFE); some of them show as MSR, MRS or SYS
\     with the numbers of the register;
\   - a system register without a name in the table of DISA64: its
\     numbers, S3_0_C15_C2_0 (llvm-objdump: its name);
\   - a target without a symbol, MOV without the comment "// =...".

ONLY FORTH DEFINITIONS  ALSO FILES
S" lib/text.fs" INCLUDED
ONLY FORTH DEFINITIONS  ALSO FILES
S" asm/monitor/monitor.fs" INCLUDED
S" asm/monitor/disa64.fs" INCLUDED
ONLY FORTH DEFINITIONS  ALSO INTERNALS  ALSO TEXT  ALSO MONITOR  ALSO DISA64
DECIMAL  A64

VARIABLE CODE-AT   VARIABLE CODE-W
: CODE-C@ ( a -- c )   CODE-AT @ -  3 AND 8 *  CODE-W @ SWAP RSHIFT 255 AND ;
' CODE-C@ IS DIS-C@
: HEXDIGIT ( c -- n )   DUP '9' > IF  32 OR 'a' - 10 +  ELSE  '0' -  THEN ;
: HEX# ( a u -- n )   0 SWAP 0 ?DO  4 LSHIFT  OVER I + C@ HEXDIGIT OR  LOOP  NIP ;
: A: ( "addr" "word" "|" "text" -- )   \ DECODE gives the text, the next instruction is 4 bytes on
  PARSE-NAME HEX# CODE-AT !  PARSE-NAME HEX# CODE-W !  PARSE-NAME 2DROP
  SOURCE >IN @ /STRING -LEADING -TRAILING              ( a u )
  CODE-AT @ DECODE CODE-AT @ -  >R
  1 #TESTS +!
  2DUP TXT COUNT S= 0=  R> 4 <> OR IF
    1 #ERRORS +!  ." WRONG: " SOURCE TYPE CR ."   got: " TXT COUNT TYPE CR
  THEN  2DROP  SOURCE NIP >IN ! ;

A: 440 d50247df | msr S0_2_C4_C7_6, xzr
A: 710 7c2ffa6b | str h11, [x19, x15, sxtx #1]
A: 1410 f83b2028 | ldeor x27, x8, [x1]
A: 1c54 d51ca31a | msr AMAIR_EL2, x26
A: 29ec 53095100 | ubfx w0, w8, #9, #12
A: 2acc d5283da8 | sysl x8, #0x0, c3, c13, #0x5
A: 2ccc 3815bc00 | strb w0, [x0, #-0xa5]!
A: 2f38 aae33fdd | orn x29, x30, x3, ror #15
A: 3120 d5033a9f | dsb ishst
A: 4014 78c691e1 | ldursh w1, [x15, #0x69]
A: 5360 d5024c3f | msr S0_2_C4_C12_1, xzr
A: 53f8 eb2d23bf | cmp x29, w13, uxth
A: 5bb8 9ac02127 | lsl x7, x9, x0
A: 5e6c d5074cdf | msr S0_7_C4_C12_6, xzr
A: 6940 28ab4a4f | stp w15, w18, [x18], #-0xa8
A: 6a24 d50c87df | tlbi vmalls12e1
A: 6f78 d50324bf | hint #0x25
A: 747c d506493f | msr S0_6_C4_C9_1, xzr
A: 7604 d51be93c | msr PMEVCNTR9_EL0, x28
A: 799c 7c7fcabf | ldr h31, [x21, wzr, sxtw]
A: 7eb4 b8647b69 | ldr w9, [x27, x4, lsl #2]
A: 80a4 d507425f | msr S0_7_C4_C2_2, xzr
A: 81e4 d50146bf | msr S0_1_C4_C6_5, xzr
A: 8908 9c86bdd0 | ldr q16, 0xfffffffffff160c0
A: 8dc4 d500481f | cfinv
A: 989c d5034f5f | msr DIT, #0xf
A: a478 d501443f | msr S0_1_C4_C4_1, xzr
A: a4b8 d502455f | msr S0_2_C4_C5_2, xzr
A: ad50 530e4ea5 | ubfx w5, w21, #14, #6
A: ad6c d51d1046 | msr CPACR_EL12, x6
A: ada4 3c4e5d8d | ldr b13, [x12, #0xe5]!
A: b058 f83d0042 | ldadd x29, x2, [x2]
A: b594 d53ed0ec | mrs x12, SCXTNUM_EL3
A: c514 130c7f44 | asr w4, w26, #12
A: caf0 6de3bebe | ldp d30, d15, [x21, #-0x1c8]!
A: ccbc d5300492 | mrs x18, DBGBVR4_EL1
A: db68 d50546df | msr S0_5_C4_C6_6, xzr
A: ea54 d5054a3f | msr S0_5_C4_C10_1, xzr
A: ecd4 936da869 | sbfiz x9, x3, #19, #43
A: 105b4 0839be0d | .inst 0x0839be0d
A: 106fc d50740ff | msr S0_7_C4_C0_7, xzr
A: 11184 ca6360a6 | eon x6, x5, x3, lsr #24
A: 1147c d5004ddf | msr S0_0_C4_C13_6, xzr
A: 11d20 d5034f9f | msr TCO, #0xf
A: 11d5c d508817f | tlbi vaae1os, xzr
A: 123f0 93401df6 | sxtb x22, w15
A: 124b8 3c13b7a8 | str b8, [x29], #-0xc5
A: 1299c e8b475ba | .inst 0xe8b475ba
A: 129f4 38de5bfa | ldtrsb w26, [sp, #-0x1b]
A: 12e6c d5044fbf | msr S0_4_C4_C15_5, xzr
A: 13530 fc5172da | ldur d26, [x22, #-0xe9]
A: 136c4 7919e62e | strh w14, [x17, #0xcf2]
A: 139d8 f80b199b | sttr x27, [x12, #0xb1]
A: 13b60 f24ecbff | tst xzr, #0xfffc001fffffffff
A: 14034 d5034b3f | msr SSBS, #0xb
A: 141a0 9ac52a02 | asr x2, x16, x5
A: 1466c acb5c62f | stp q15, q17, [x17], #-0x150
A: 14f6c d96a201a | ldg x26, [x0, #0xa20]
A: 151dc d50242ff | msr S0_2_C4_C2_7, xzr
A: 16320 d53bd0fe | mrs x30, SCXTNUM_EL0
A: 16638 d5044e3f | msr S0_4_C4_C14_1, xzr
A: 1723c d50542ff | msr S0_5_C4_C2_7, xzr
A: 178e8 a8347411 | stnp x17, x29, [x0, #-0xc0]
A: 18840 d5074c5f | msr S0_7_C4_C12_2, xzr
A: 18cb0 d508853f | tlbi rvae1os, xzr
A: 19104 38d190a4 | ldursb w4, [x5, #-0xe7]
A: 19c00 f83fa364 | st64bv0 xzr, x4, [x27]
A: 19d50 bc0852fb | stur s27, [x23, #0x85]
A: 1a194 781af53e | strh w30, [x9], #-0x51
A: 1a9bc d5033f3f | msr S0_3_C3_C15_1, xzr
A: 1ab88 dac147e8 | xpacd x8
A: 1ad14 f80921c5 | stur x5, [x14, #0x92]
A: 1b0ac d5044d3f | msr S0_4_C4_C13_1, xzr
A: 1b504 d505441f | msr S0_5_C4_C4_0, xzr
A: 1ba6c d500411f | cfinv
A: 1badc 7a1303e0 | ngcs w0, w19
A: 1bebc 79143f90 | strh w16, [x28, #0xa1e]
A: 1c1ac d50246df | msr S0_2_C4_C6_6, xzr
A: 1c2b4 59c320bd | ldapursh w29, [x5, #0x32]
A: 1c380 fd7f36d3 | ldr d19, [x22, #0x7e68]
A: 1c490 a1242b37 | .inst 0xa1242b37
A: 1c78c d501489f | msr S0_1_C4_C8_4, xzr
A: 1c7d8 1ad32d59 | ror w25, w10, w19
A: 1caa0 88c751d6 | ldlar w22, [x14]
A: 1cad4 d50140bf | msr S0_1_C4_C0_5, xzr
A: 1d388 ba4423e7 | ccmn xzr, x4, #0x7, hs
A: 1da7c d65f0bff | retaa
A: 1dc60 2f10e4a5 | .inst 0x2f10e4a5
A: 1deb8 d5004f3f | xaflag
A: 1e0e8 d5054ebf | msr S0_5_C4_C14_5, xzr
A: 1e290 d507481f | msr S0_7_C4_C8_0, xzr
A: 1ec88 d507487f | msr S0_7_C4_C8_3, xzr
A: 20a0c d503341f | msr S0_3_C3_C4_0, xzr
A: 20fc8 d504457f | msr S0_4_C4_C5_3, xzr
A: 21008 6ce38cdf | ldp d31, d3, [x6], #-0x1c8
A: 213ac 13001ea3 | sxtb w3, w21
A: 219f4 d4002022 | hvc #0x101
A: 220b4 8b89537a | add x26, x27, x9, asr #20
A: 22528 d50745df | msr S0_7_C4_C5_6, xzr
A: 22ccc 93407c20 | sxtw x0, w1
A: 23074 b7f26b39 | tbnz x25, #0x3e, 0x27dd8
A: 236a0 3891e02e | ldursb x14, [x1, #-0xe2]
A: 23e60 d50440ff | msr S0_4_C4_C0_7, xzr
A: 240d0 530f7db6 | lsr w22, w13, #15
A: 25358 d50742bf | msr S0_7_C4_C2_5, xzr
A: 2535c d3651a2a | ubfiz x10, x17, #27, #7
A: 25740 b836617f | stumax w22, [x11]
A: 25b9c b8460bc7 | ldtr w7, [x30, #0x60]
A: 25dac d507427f | msr S0_7_C4_C2_3, xzr
A: 2614c 69614e59 | ldpsw x25, x19, [x18, #-0xf8]
A: 266c8 d50741ff | msr S0_7_C4_C1_7, xzr
A: 277f0 7c11c316 | stur h22, [x24, #-0xe4]
A: 27b98 53144ee2 | lsl w2, w23, #12
A: 27f04 d52cd93f | sysl xzr, #0x4, c13, c9, #0x1
A: 2818c d63f00c0 | blr x6
A: 2836c d503427f | smstop sm
A: 28a70 8b03d0a6 | add x6, x5, x3, lsl #52
A: 291ac dac10304 | pacia x4, x24
A: 29364 d51009a6 | msr DBGBCR9_EL1, x6
A: 29548 19947198 | ldapursb x24, [x12, #-0xb9]
A: 2991c d5024a5f | msr S0_2_C4_C10_2, xzr
A: 29c3c f9b3611f | prfm #0x1f, [x8, #0x66c0]
A: 2a5bc 7d53a9e2 | ldr h2, [x15, #0x9d4]
A: 2a798 d501419f | msr S0_1_C4_C1_4, xzr
A: 2a90c 59df20bc | ldapursh w28, [x5, #-0xe]
A: 2af64 d36f1b00 | ubfiz x0, x24, #17, #7
A: 2b074 f822de44 | ldraa x4, [x18, #0x168]!
A: 2b210 d51c4350 | msr SPSR_und, x16
A: 2b4dc d507467f | msr S0_7_C4_C6_3, xzr
A: 2bda4 d61f0020 | br x1
A: 2c320 2a242009 | orn w9, w0, w4, lsl #8
A: 2c85c fc7f5a36 | ldr d22, [x17, wzr, uxtw #3]
A: 2cc50 d5112810 | msr S2_1_C2_C8_0, x16
A: 2dc18 d50543bf | msr S0_5_C4_C3_5, xzr
A: 2e38c dac143fc | xpaci x28
A: 2ed18 9ad24e87 | crc32x w7, w20, x18
A: 2f234 3c85b577 | str q23, [x11], #0x5b
A: 2f3f8 2cbca0ec | stp s12, s8, [x7], #-0x1c
A: 2f858 d501441f | msr S0_1_C4_C4_0, xzr
A: 305b0 2d44837c | ldp s28, s0, [x27, #0x24]
A: 30f68 d52e1748 | sysl x8, #0x6, c1, c7, #0x2
A: 31220 d5054aff | msr S0_5_C4_C10_7, xzr
A: 312dc 9264b3f2 | and x18, xzr, #0xfffffffff00001ff
A: 31c68 aadff6c7 | orr x7, x22, xzr, ror #61
A: 31fe4 acf04fad | ldp q13, q19, [x29], #-0x200
A: 32030 d65f03c0 | ret
A: 32334 d502477f | msr S0_2_C4_C7_3, xzr
A: 328b0 d50240ff | msr S0_2_C4_C0_7, xzr
A: 32a24 d53bed01 | mrs x1, PMEVTYPER8_EL0
A: 32f6c d51c1077 | msr SCTLR2_EL2, x23
A: 33144 d5024eff | msr S0_2_C4_C14_7, xzr
A: 33198 d505479f | msr S0_5_C4_C7_4, xzr
A: 332bc d5014d3f | msr S0_1_C4_C13_1, xzr
A: 3337c d50741df | msr S0_7_C4_C1_6, xzr
A: 33ea8 7a1403e5 | ngcs w5, w20
A: 341cc b4b156bb | cbz x27, 0xfffffffffff96ca0
A: 34bf0 b823227f | steor w3, [x19]
A: 3546c d5074a3f | msr S0_7_C4_C10_1, xzr
A: 357b8 e986c72a | .inst 0xe986c72a
A: 36818 d5033a1f | msr S0_3_C3_C10_0, xzr
A: 36b4c d503203f | yield
A: 3729c d27353f2 | eor x18, xzr, #0x3ffffe000
A: 373ac 195b340d | .inst 0x195b340d
A: 37830 d51c110d | msr HCR_EL2, x13
A: 37e08 d50747ff | msr S0_7_C4_C7_7, xzr
A: 380b4 522e78eb | eor w11, w7, #0xfffdffff
A: 385b4 7c094185 | stur h5, [x12, #0x94]
A: 38be8 d63f0040 | blr x2
A: 38ea8 54303ea1 | b.ne 0x9967c
A: 39174 d51f8072 | msr S3_7_C8_C0_3, x18
A: 39800 d503315f | clrex #0x1
A: 39924 d5034a7f | msr S0_3_C4_C10_3, xzr
A: 39968 d51e9b60 | msr S3_6_C9_C11_3, x0
A: 39e1c d50144df | msr S0_1_C4_C4_6, xzr
A: 3a0d0 d500429f | msr PAN, #0x2
A: 3ad4c d507463f | msr S0_7_C4_C6_1, xzr
A: 3ae68 d5100e82 | msr DBGBVR14_EL1, x2
A: 3b8f8 d50144bf | msr S0_1_C4_C4_5, xzr
A: 3c434 d50241ff | msr S0_2_C4_C1_7, xzr
A: 3d82c d503375f | clrex #0x7
A: 3e134 d5033a7f | msr S0_3_C3_C10_3, xzr
A: 3e730 b3564595 | bfi x21, x12, #42, #18
A: 3f390 d50647ff | msr S0_6_C4_C7_7, xzr
A: 3f574 d50339ff | sb
A: 3f95c d505453f | msr S0_5_C4_C5_1, xzr
A: 401ec b212c6bf | orr sp, x21, #0xc0c0c0c0c0c0c0c0
A: 40938 d506481f | msr S0_6_C4_C8_0, xzr
A: 40b58 d92001bd | stzgm x29, [x13]
A: 40b6c b92c4d5a | str w26, [x10, #0x2c4c]
A: 4127c b80fec2f | str w15, [x1, #0xfe]!
A: 41d74 38894f1f | ldrsb xzr, [x24, #0x94]!
A: 41e98 79aad19f | ldrsh xzr, [x12, #0x1568]
A: 420b8 d50348bf | msr S0_3_C4_C8_5, xzr
A: 421f0 1ad74261 | crc32b w1, w19, w23
A: 42844 53103da4 | lsl w4, w13, #16
A: 43094 d504471f | msr S0_4_C4_C7_0, xzr
A: 434fc 79dc4a8a | ldrsh w10, [x20, #0xe24]
A: 438e8 d5004f7f | msr UAO, #0xf
A: 4404c d503401f | msr S0_3_C4_C0_0, xzr
A: 444d4 8a797f55 | bic x21, x26, x25, lsr #31
A: 44a0c b857a8c0 | ldtr w0, [x6, #-0x86]
A: 44aac 08347fe6 | casp w20, w21, w6, w7, [sp]
A: 44d84 d538001d | mrs x29, MIDR_EL1
A: 450c8 d50645ff | msr S0_6_C4_C5_7, xzr
A: 451f0 d50746ff | msr S0_7_C4_C6_7, xzr
A: 45864 bc486359 | ldur s25, [x26, #0x86]
A: 45898 d502449f | msr S0_2_C4_C4_4, xzr
A: 46194 d50649ff | msr S0_6_C4_C9_7, xzr
A: 4633c d5064c1f | msr S0_6_C4_C12_0, xzr
A: 463bc dac13bf1 | autdza x17
A: 46574 d502419f | msr S0_2_C4_C1_4, xzr
A: 469f8 d505419f | msr S0_5_C4_C1_4, xzr
A: 4747c d5034ddf | msr DAIFSet, #0xd
A: 47c80 ea8390bf | tst x5, x3, asr #36
A: 47f44 b206affa | mov x26, #-0x3c003c003c003c1
A: 480c0 598fe28d | ldapursh x13, [x20, #0xfe]
A: 48480 6cf4ba7d | ldp d29, d14, [x19], #-0xb8
A: 49104 2dc50cf6 | ldp s22, s3, [x7, #0x28]!
A: 493c4 2d361798 | stp s24, s5, [x28, #-0x50]
A: 4a9e8 d5044edf | msr S0_4_C4_C14_6, xzr
A: 4af14 d501471f | msr S0_1_C4_C7_0, xzr
A: 4b504 d501439f | msr S0_1_C4_C3_4, xzr
A: 4b60c 54ca66db | bc.lt 0xfffffffffffe02e4
A: 4b6f8 d503321f | msr S0_3_C3_C2_0, xzr
A: 4bfe8 b8ad6ac2 | ldrsw x2, [x22, x13]
A: 4c05c d50247bf | msr S0_2_C4_C7_5, xzr
A: 4c14c d63f0e1f | blrabz x16
A: 4cd44 fc4b45d2 | ldr d18, [x14], #0xb4
A: 4cdbc d50049bf | msr SPSel, #0x9
A: 4cf48 d53aac6f | mrs x15, S3_2_C10_C12_3
A: 4d3c0 fd4afcdb | ldr d27, [x6, #0x15f8]
A: 4d3fc d5004b3f | xaflag
A: 4e954 2929fd0d | stp w13, wzr, [x8, #-0xb4]
A: 4f668 d5054edf | msr S0_5_C4_C14_6, xzr
A: 4f7a0 d505463f | msr S0_5_C4_C6_1, xzr
A: 4fadc d507473f | msr S0_7_C4_C7_1, xzr
A: 50408 d51bd2a6 | msr S3_3_C13_C2_5, x6
A: 50640 9acb2a39 | asr x25, x17, x11
A: 50b60 434999b2 | .inst 0x434999b2
A: 50d54 1adf5833 | crc32cw w19, w1, wzr
A: 515ec 69fa44f9 | ldpsw x25, x17, [x7, #-0x30]!
A: 51a50 b35377ff | bfxil xzr, xzr, #19, #11
A: 524bc d5034a1f | msr S0_3_C4_C10_0, xzr
A: 5269c da842082 | cinv x2, x4, lo
A: 526e0 d5313e0b | mrs x11, S2_1_C3_C14_0
A: 52800 5a0403ff | ngc wzr, w4
A: 5285c d502441f | msr S0_2_C4_C4_0, xzr
A: 52d64 d53802e2 | mrs x2, ID_ISAR6_EL1
A: 5324c d37beadf | lsl xzr, x22, #5
A: 5338c 68af9786 | stgp x6, x5, [x28], #-0x210
A: 53688 d5033eff | sb
A: 53754 d5014c5f | msr S0_1_C4_C12_2, xzr
A: 53d28 568de1cc | .inst 0x568de1cc
A: 541e8 3cd62c7e | ldr q30, [x3, #-0x9e]!
A: 54fa4 690a5a92 | stgp x18, x22, [x20, #0x140]
A: 55978 d50241bf | msr S0_2_C4_C1_5, xzr
A: 56518 3220a557 | orr w23, w10, #0x3ff03ff
A: 56c2c d502415f | msr S0_2_C4_C1_2, xzr
A: 57498 3890b551 | ldrsb x17, [x10], #-0xf5
A: 57ab8 d5064e1f | msr S0_6_C4_C14_0, xzr
A: 57f30 d504435f | msr S0_4_C4_C3_2, xzr
A: 587f4 2d0b61c0 | stp s0, s24, [x14, #0x58]
A: 58850 d50e923f | tlbi rvae3isnxs, xzr
A: 58868 b22c567f | orr sp, x19, #0xfff003fffff003ff
A: 5887c 39049b8d | strb w13, [x28, #0x126]
A: 59248 1100009f | mov wsp, w4
A: 597e8 d5074f3f | msr S0_7_C4_C15_1, xzr
A: 59a78 4b22a7c8 | sub w8, w30, w2, sxth #1
A: 5a450 d5034eff | msr DAIFClr, #0xe
A: 5a5e4 d5024f7f | msr S0_2_C4_C15_3, xzr
A: 5a7e8 f82fb2d4 | st64bv x15, x20, [x22]
A: 5add4 eb386a80 | subs x0, x20, x24, uxtx #2
A: 5b060 d504465f | msr S0_4_C4_C6_2, xzr
A: 5b55c d507419f | msr S0_7_C4_C1_4, xzr
A: 5bfc0 d53bd050 | mrs x16, TPIDR_EL0
A: 5c39c cb57778f | sub x15, x28, x23, lsr #29
A: 5c78c d18e10a2 | subg x2, x5, #0xe0, #0x4
A: 5d4ac d503307f | tcommit
A: 5d7cc d5074f7f | msr S0_7_C4_C15_3, xzr
A: 5d870 d8c104b8 | prfm #0x18, 0xfffffffffffdf904
A: 5e068 d504475f | msr S0_4_C4_C7_2, xzr
A: 5e32c c1255ce8 | .inst 0xc1255ce8
A: 5ed48 5a83f64d | csneg w13, w18, w3, nv
A: 5ffe4 aa0f03f2 | mov x18, x15
A: 60844 a94b80b7 | ldp x23, x0, [x5, #0xb8]
A: 60e04 d5014bbf | msr S0_1_C4_C11_5, xzr
A: 60e18 93787094 | sbfiz x20, x4, #8, #29
A: 61174 b367525b | bfi x27, x18, #25, #21
A: 6156c b222d728 | orr x8, x25, #0xcfcfcfcfcfcfcfcf
A: 61b98 d50741bf | msr S0_7_C4_C1_5, xzr
A: 61e8c 52a00006 | movz w6, #0x0, lsl #16
A: 621f8 fc0e3dd4 | str d20, [x14, #0xe3]!
A: 63c24 eb242c3f | cmp x1, w4, uxth #3
A: 63cbc 1ac30c9a | sdiv w26, w4, w3
A: 63e38 d5054cdf | msr S0_5_C4_C12_6, xzr
A: 654e8 508dd450 | adr x16, 0xfffffffffff80f72
A: 65c54 d71f09ed | braa x15, x13
A: 65d50 d504455f | msr S0_4_C4_C5_2, xzr
A: 66ed4 d50540bf | msr S0_5_C4_C0_5, xzr
A: 67120 d506483f | msr S0_6_C4_C8_1, xzr
A: 6776c a8881cb1 | stp x17, x7, [x5], #0x80
A: 67a64 d50644df | msr S0_6_C4_C4_6, xzr
A: 67b9c dac112ec | autia x12, x23
A: 67fec 3c8cc1ea | stur q10, [x15, #0xcc]
A: 68218 cb43c3e6 | neg x6, x3, lsr #48
A: 682d4 3746b13c | tbnz w28, #0x8, 0x658f8
A: 68520 d53ec010 | mrs x16, VBAR_EL3
A: 68b88 d504485f | msr S0_4_C4_C8_2, xzr
A: 68de8 d506495f | msr S0_6_C4_C9_2, xzr
A: 691f8 d50642ff | msr S0_6_C4_C2_7, xzr
A: 69f88 b95a498a | ldr w10, [x12, #0x1a48]
A: 6a1f0 b83ff9df | str wzr, [x14, xzr, sxtx #2]
A: 6a7a8 f8333253 | ldset x19, x19, [x18]
A: 6a864 d5054fdf | msr S0_5_C4_C15_6, xzr
A: 6ace4 d50542df | msr S0_5_C4_C2_6, xzr
A: 6ae9c d507415f | msr S0_7_C4_C1_2, xzr
A: 6af28 d506451f | msr S0_6_C4_C5_0, xzr
A: 6af2c d505465f | msr S0_5_C4_C6_2, xzr
A: 6b45c d503471f | msr S0_3_C4_C7_0, xzr
A: 6b65c f99f62aa | prfm plil2keep, [x21, #0x3ec0]
A: 6b9d8 d5014ddf | msr S0_1_C4_C13_6, xzr
A: 6ba08 dac1092e | pacda x14, x9
A: 6bdcc 58e0ce0f | ldr x15, 0x2d78c
A: 6c87c d50640df | msr S0_6_C4_C0_6, xzr
A: 6cc40 3de566ff | ldr q31, [x23, #0x9590]
A: 6d370 52544fb7 | .inst 0x52544fb7
A: 6d548 d61f0cbf | brabz x5
A: 6d7f8 8a3fa4a6 | bic x6, x5, xzr, lsl #41
A: 6d81c d35ee57d | ubfx x29, x11, #30, #28
A: 6d848 d5014f5f | msr S0_1_C4_C15_2, xzr
A: 6d8f0 d5054f5f | msr S0_5_C4_C15_2, xzr
A: 6dbc0 b5efd175 | cbnz x21, 0x4d5ec
A: 6dda4 d50540ff | msr S0_5_C4_C0_7, xzr
A: 6e06c 9bda3593 | umulh x19, x12, x26
A: 6e0a4 3df86483 | ldr q3, [x4, #0xe190]
A: 6e834 fc0dad3e | str d30, [x9, #0xda]!
A: 6e8c8 9262d7f1 | and x17, xzr, #0xffffffffc00fffff
A: 6ea10 d51dc017 | msr VBAR_EL12, x23
A: 6f70c b8032967 | sttr w7, [x11, #0x32]
A: 6f760 d53cc037 | mrs x23, RVBAR_EL2
A: 6fec4 dac13be9 | autdza x9
A: 702b4 d5033cbf | dmb #12
A: 70824 d92e5d19 | stg x25, [x8, #0xe50]!
A: 70fec d504423f | msr S0_4_C4_C2_1, xzr
A: 711c0 dac133ee | autiza x14
A: 71304 d5064c7f | msr S0_6_C4_C12_3, xzr
A: 72008 99888245 | ldapursw x5, [x18, #0x88]
A: 72518 53030b16 | lsl w22, w24, #29
A: 725cc d5064a5f | msr S0_6_C4_C10_2, xzr
A: 7265c d5031025 | wfit x5
A: 72ecc d342fdae | lsr x14, x13, #2
A: 736f4 3880ed47 | ldrsb x7, [x10, #0xe]!
A: 73b98 3c895517 | str q23, [x8], #0x95
A: 73fb4 24da878a | .inst 0x24da878a
A: 74284 d5074a7f | msr S0_7_C4_C10_3, xzr
A: 742a0 3890b056 | ldursb x22, [x2, #-0xf5]
A: 74ac0 f1661643 | subs x3, x18, #0x985, lsl #12
A: 74be4 d5034b1f | msr S0_3_C4_C11_0, xzr
A: 74fa8 d52c5e05 | sysl x5, #0x4, c5, c14, #0x0
A: 75438 d505499f | msr S0_5_C4_C9_4, xzr
A: 75d80 d5044d9f | msr S0_4_C4_C13_4, xzr
A: 75d94 b6882fb6 | tbz x22, #0x31, 0x76388
A: 75d9c d505469f | msr S0_5_C4_C6_4, xzr
A: 76090 1b03fe9c | mneg w28, w20, w3
A: 76434 d5300ef8 | mrs x24, DBGWCR14_EL1
A: 76520 f82353df | stsmin x3, [x30]
A: 76734 d502401f | msr S0_2_C4_C0_0, xzr
A: 76874 f82dbcdb | ldraa x27, [x6, #0x6d8]!
A: 76910 dac0055d | rev16 x29, x10
A: 77578 d51cc018 | msr VBAR_EL2, x24
A: 77e04 93d79fff | extr xzr, xzr, x23, #0x27
A: 783c8 72b3b0bf | movk wzr, #0x9d85, lsl #16
A: 784b4 d50546ff | msr S0_5_C4_C6_7, xzr
A: 78e88 3c0aa089 | stur b9, [x4, #0xaa]
A: 79164 9a0400a0 | adc x0, x5, x4
A: 79328 d5062f5d | msr S0_6_C2_C15_2, x29
A: 79604 b9736856 | ldr w22, [x2, #0x3368]
A: 79750 d518ccd4 | msr ICC_IGRPEN0_EL1, x20
A: 7a634 d5014d1f | msr S0_1_C4_C13_0, xzr
A: 7aa08 ea63b4a6 | bics x6, x5, x3, lsr #45
A: 7aa94 d504463f | msr S0_4_C4_C6_1, xzr
A: 7b05c d51bc31d | msr S3_3_C12_C3_0, x29
A: 7b6f8 3c62d99f | ldr b31, [x12, w2, sxtw #0]
A: 7b910 d5064d7f | msr S0_6_C4_C13_3, xzr
A: 7ba20 d5033d5f | clrex #0xd
A: 7c9c4 d50249ff | msr S0_2_C4_C9_7, xzr
A: 7dc70 d5064e5f | msr S0_6_C4_C14_2, xzr
A: 7e430 d5064b5f | msr S0_6_C4_C11_2, xzr
A: 7ed2c ab2a6b5f | cmn x26, x10, uxtx #2
A: 7ef1c 4c007c26 | .inst 0x4c007c26
A: 7ff1c 6996f38c | stgp x12, x28, [x28, #0x2d0]!
A: 804ec 530c6c93 | ubfx w19, w4, #12, #16
A: 80554 d50321bf | hint #0xd
A: 8237c d50041df | msr S0_0_C4_C1_6, xzr
A: 83098 d41b0682 | hvc #0xd834
A: 835c4 d5024c9f | msr S0_2_C4_C12_4, xzr
A: 8367c 92a00007 | movn x7, #0x0, lsl #16
A: 83b68 d505493f | msr S0_5_C4_C9_1, xzr
A: 83bb0 d5024dbf | msr S0_2_C4_C13_5, xzr
A: 83d60 69edfec8 | ldpsw x8, xzr, [x22, #-0x94]!
A: 83f40 d501413f | msr S0_1_C4_C1_1, xzr
A: 84468 dac13fe1 | autdzb x1
A: 8446c d5054a9f | msr S0_5_C4_C10_4, xzr
A: 84798 a4beadde | .inst 0xa4beadde
A: 84850 d506439f | msr S0_6_C4_C3_4, xzr
A: 848f8 d51ccd65 | msr ICH_LR11_EL2, x5
A: 84b44 d5044f7f | msr S0_4_C4_C15_3, xzr
A: 84c5c b83d305f | stset w29, [x2]
A: 84e78 d504449f | msr S0_4_C4_C4_4, xzr
A: 85074 d50744ff | msr S0_7_C4_C4_7, xzr
A: 85154 d51009fd | msr DBGWCR9_EL1, x29
A: 8531c f21d27ff | tst xzr, #0x1ff800001ff8
A: 859c4 2aa357e6 | mvn w6, w3, asr #21
A: 85e24 d50740df | msr S0_7_C4_C0_6, xzr
A: 868b4 321ac7e7 | mov w7, #-0x3f3f3f40
A: 869b0 d9e00227 | ldgm x7, [x17]
A: 86f64 eb9f77e6 | negs x6, xzr, asr #29
A: 86ff4 f856b7d9 | ldr x25, [x30], #-0x95
A: 871c0 9b029c66 | msub x6, x3, x2, x7
A: 87360 d50449df | msr S0_4_C4_C9_6, xzr
A: 87bb0 d506413f | msr S0_6_C4_C1_1, xzr
A: 88440 d501487f | msr S0_1_C4_C8_3, xzr
A: 88750 d5054e3f | msr S0_5_C4_C14_1, xzr
A: 89170 d507449f | msr S0_7_C4_C4_4, xzr
A: 895d4 f8b36bf5 | prfm pstl3strm, [sp, x19]
A: 89d78 33033728 | bfxil w8, w25, #3, #11
A: 8a67c aad72f38 | orr x24, x25, x23, ror #11
A: 8a6d8 38ab4af0 | ldrsb x16, [x23, w11, uxtw]
A: 8b264 d5033bbf | dmb ish
A: 8b644 d505417f | msr S0_5_C4_C1_3, xzr
A: 8bbfc 9b36ffea | smnegl x10, wzr, w22
A: 8c198 d501469f | msr S0_1_C4_C6_4, xzr
A: 8c23c d51cc83c | msr ICH_AP0R1_EL2, x28
A: 8c7b4 9ac51755 | gmi x21, x26, x5
A: 8cf94 d5014dbf | msr S0_1_C4_C13_5, xzr
A: 8d054 8b5f7fe6 | add x6, xzr, xzr, lsr #31
A: 8d50c dac147ec | xpacd x12
A: 8e678 321264f6 | orr w22, w7, #0xffffc0ff
A: 8f870 d5014a7f | msr S0_1_C4_C10_3, xzr
A: 8fca0 d50243df | msr S0_2_C4_C3_6, xzr
A: 9078c d5064a7f | msr S0_6_C4_C10_3, xzr
A: 907a8 78ccb118 | ldursh w24, [x8, #0xcb]
A: 908ac d501495f | msr S0_1_C4_C9_2, xzr
A: 90c9c 13096b35 | sbfx w21, w25, #9, #18
A: 911d0 d518a28c | msr S3_0_C10_C2_4, x12
A: 91468 91455a2a | add x10, x17, #0x156, lsl #12
A: 914cc d502453f | msr S0_2_C4_C5_1, xzr
A: 915e4 d5064f3f | msr S0_6_C4_C15_1, xzr
A: 91678 d504499f | msr S0_4_C4_C9_4, xzr
A: 919ac d506491f | msr S0_6_C4_C9_0, xzr
A: 91cc8 5238e8e7 | eor w7, w7, #0x77777777
A: 91cd0 68fc8741 | ldpsw x1, x1, [x26], #-0x1c
A: 91df4 d5064abf | msr S0_6_C4_C10_5, xzr
A: 920e4 d61f0060 | br x3
A: 92c0c d50e813f | tlbi vae3os, xzr
A: 93494 d501425f | msr S0_1_C4_C2_2, xzr
A: 93710 383fea5f | strb wzr, [x18, xzr, sxtx]
A: 93ab0 d503399f | dsb ishld
A: 940cc d527c31f | mrs xzr, S0_7_C12_C3_0
A: 94ea0 d51cc054 | msr RMR_EL2, x20
A: 95670 1adb21ea | lsl w10, w15, w27
A: 96b04 d507423f | msr S0_7_C4_C2_1, xzr
A: 96c48 995de134 | ldapur w20, [x9, #-0x22]
A: 974b8 d5064c3f | msr S0_6_C4_C12_1, xzr
A: 9768c d5074fbf | msr S0_7_C4_C15_5, xzr
A: 978a8 80750dd0 | .inst 0x80750dd0
A: 97a34 280e8cb5 | stnp w21, w3, [x5, #0x74]
A: 98444 a976423d | ldp x29, x16, [x17, #-0xa0]
A: 98824 d5186120 | msr S3_0_C6_C1_1, x0
A: 99c20 b8063506 | str w6, [x8], #0x63
A: 9a278 d522f55f | mrs xzr, S0_2_C15_C5_2
A: 9ab58 d506415f | msr S0_6_C4_C1_2, xzr
A: 9ae80 b82a401f | stsmax w10, [x0]
A: 9aff4 699a6d2c | stgp x12, x27, [x9, #0x340]!
A: 9b3d8 d50441df | msr S0_4_C4_C1_6, xzr
A: 9b5d0 d5074e9f | msr S0_7_C4_C14_4, xzr
A: 9bc8c d50648bf | msr S0_6_C4_C8_5, xzr
A: 9be1c 32140810 | orr w16, w0, #0x7000
A: 9d140 d502407f | msr S0_2_C4_C0_3, xzr
A: 9d1b4 d501499f | msr S0_1_C4_C9_4, xzr
A: 9d554 d5044a1f | msr S0_4_C4_C10_0, xzr
A: 9d718 d5088713 | tlbi vmalle1
A: 9e540 dac00af7 | rev32 x23, x23
A: 9e6ac 3a0048ed | setf16 w7
A: 9ec0c 397a82dc | ldrb w28, [x22, #0xea0]
A: 9fdf4 f83f22e4 | ldeor xzr, x4, [x23]
A: a0194 d50442df | msr S0_4_C4_C2_6, xzr
A: a142c d50141bf | msr S0_1_C4_C1_5, xzr
A: a17fc e02c50e8 | .inst 0xe02c50e8
A: a1824 d51e513a | msr AFSR1_EL3, x26
A: a1b60 283dcca5 | stnp w5, w19, [x5, #-0x14]
A: a216c f83f53b8 | ldsmin xzr, x24, [x29]
A: a2368 d507465f | msr S0_7_C4_C6_2, xzr
A: a2404 d5054abf | msr S0_5_C4_C10_5, xzr
A: a267c d50043df | msr S0_0_C4_C3_6, xzr
A: a28e8 3a5f33ef | ccmn wzr, wzr, #0xf, lo
A: a2b58 d71f0cae | brab x5, x14
A: a2ea0 3c7ff844 | ldr b4, [x2, xzr, sxtx #0]
A: a3140 f84b2551 | ldr x17, [x10], #0xb2
A: a345c a823a64c | stnp x12, x9, [x18, #-0x1c8]
A: a3560 d5054dff | msr S0_5_C4_C13_7, xzr
A: a3634 d5034c1f | msr S0_3_C4_C12_0, xzr
A: a3d30 d502457f | msr S0_2_C4_C5_3, xzr
A: a3d98 dac147f0 | xpacd x16
A: a3f28 f8115db7 | str x23, [x13, #-0xeb]!
A: a4168 5ac015ac | cls w12, w13
A: a42b8 9376527b | sbfiz x27, x19, #10, #21
A: a4854 2c51d499 | ldnp s25, s21, [x4, #0x8c]
A: a4e3c d50648ff | msr S0_6_C4_C8_7, xzr
A: a4e44 d501437f | msr S0_1_C4_C3_3, xzr
A: a5bec d50447df | msr S0_4_C4_C7_6, xzr
A: a6b94 9acd0bb0 | udiv x16, x29, x13
A: a6ff8 93403f38 | sxth x24, w25
A: a767c b989a9e4 | ldrsw x4, [x15, #0x9a8]
A: a7e70 d5054d5f | msr S0_5_C4_C13_2, xzr
A: a82a0 d2c4e45a | mov x26, #0x272200000000
A: a8698 d5034bbf | msr S0_3_C4_C11_5, xzr
A: a8f3c d92127fc | stg x28, [sp], #0x120
A: a9278 d50746df | msr S0_7_C4_C6_6, xzr
A: a95cc f83fd3d4 | ld64b x20, [x30]
A: a9d74 d5064d3f | msr S0_6_C4_C13_1, xzr
A: a9efc d501483f | msr S0_1_C4_C8_1, xzr
A: aa068 d506455f | msr S0_6_C4_C5_2, xzr
A: aa664 d5064e9f | msr S0_6_C4_C14_4, xzr
A: aa7d8 d505485f | msr S0_5_C4_C8_2, xzr
A: aa908 d5089341 | tlbi aside1isnxs, x1
A: ab7ec 19db1678 | .inst 0x19db1678
A: ab820 d502479f | msr S0_2_C4_C7_4, xzr
A: ab8fc d50542bf | msr S0_5_C4_C2_5, xzr
A: abe8c d5033bdf | isb #11
A: ac0c8 d5074e1f | msr S0_7_C4_C14_0, xzr
A: ac224 d51bec4f | msr PMEVTYPER2_EL0, x15
A: ac440 f14e1d1f | cmp x8, #0x387, lsl #12
A: acc78 acae56e5 | stp q5, q21, [x23], #-0x240
A: ad07c d53daa42 | mrs x2, S3_5_C10_C10_2
A: ad230 d5024f1f | msr S0_2_C4_C15_0, xzr
A: adc08 d50049df | msr S0_0_C4_C9_6, xzr
A: ae0cc d50544ff | msr S0_5_C4_C4_7, xzr
A: ae668 d9b68a8d | st2g x13, [x20, #-0x980]
A: aea6c 6d1fa31c | stp d28, d8, [x24, #0x1f8]
A: af204 d502405f | msr S0_2_C4_C0_2, xzr
A: af6a4 131e7dd2 | asr w18, w14, #30
A: af6dc d505411f | msr S0_5_C4_C1_0, xzr
A: afbe8 36fe4688 | tbz w8, #0x1f, 0xac4b8
A: afe60 d503347f | msr S0_3_C3_C4_3, xzr
A: affb4 6d872283 | stp d3, d8, [x20, #0x70]!
A: b02c4 f82243a6 | ldsmax x2, x6, [x29]
A: b0330 1b104540 | madd w0, w10, w16, w17
A: b05ec 285be9f0 | ldnp w16, w26, [x15, #0xdc]
A: b0848 d508791f | at s1e1rp, xzr
A: b11f8 d50641df | msr S0_6_C4_C1_6, xzr
A: b2478 d50540df | msr S0_5_C4_C0_6, xzr
A: b358c d5074bff | msr S0_7_C4_C11_7, xzr
A: b37c4 923ff3c8 | and x8, x30, #0xaaaaaaaaaaaaaaaa
A: b3970 5a9fa3f5 | csetm w21, lt
A: b3bb4 d2e00003 | movz x3, #0x0, lsl #48
A: b3e90 d53fe24e | mrs x14, CNTPS_CVAL_EL1
A: b3ed8 d5024b9f | msr S0_2_C4_C11_4, xzr
A: b40dc 7853a1a6 | ldurh w6, [x13, #-0xc6]
A: b4240 d504405f | msr S0_4_C4_C0_2, xzr
A: b4acc d5257a5f | mrs xzr, S0_5_C7_C10_2
A: b4ba0 dac11f9d | autdb x29, x28
A: b5348 d5024f3f | msr S0_2_C4_C15_1, xzr
A: b5794 d9a000fd | stgm x29, [x7]
A: b5a08 d5074f1f | msr S0_7_C4_C15_0, xzr
A: b5e04 d507459f | msr S0_7_C4_C5_4, xzr
A: b60bc d53b452e | mrs x14, DLR_EL0
A: b61d4 d50646bf | msr S0_6_C4_C6_5, xzr
A: b69e8 041189b5 | .inst 0x041189b5
A: b7cdc d63f0f5f | blrabz x26
A: b8544 d506407f | msr S0_6_C4_C0_3, xzr
A: b8f88 d5064e3f | msr S0_6_C4_C14_1, xzr
A: b9104 d5014aff | msr S0_1_C4_C10_7, xzr
A: b9258 d507431f | msr S0_7_C4_C3_0, xzr
A: b933c d218349b | eor x27, x4, #0x3fff00003fff00
A: b9400 9b343580 | smaddl x0, w12, w20, x13
A: b9428 78ed69c9 | ldrsh w9, [x14, x13]
A: b9ad4 d50447bf | msr S0_4_C4_C7_5, xzr
A: ba5f4 d507421f | msr S0_7_C4_C2_0, xzr
A: babb8 d5074b9f | msr S0_7_C4_C11_4, xzr
A: bb424 d9e1273b | stz2g x27, [x25], #0x120
A: bba6c d34aa631 | ubfx x17, x17, #10, #32
A: bbbd4 a965d9e2 | ldp x2, x22, [x15, #-0x1a8]
A: bbbe0 7878599a | ldrh w26, [x12, w24, uxtw #1]
A: bbdb8 d501473f | msr S0_1_C4_C7_1, xzr
A: bc9cc 331512d3 | bfi w19, w22, #11, #5
A: bcd44 d5064c5f | msr S0_6_C4_C12_2, xzr
A: bcf78 d5004adf | msr S0_0_C4_C10_6, xzr
A: bd04c f8bf367f | ldrab xzr, [x19, #0xf98]
A: bd28c 910dea48 | add x8, x18, #0x37a
A: bd5d0 b82f317d | ldset w15, w29, [x11]
A: bd980 d5088627 | tlbi rvae1, x7
A: bdb84 d5032b7f | hint #0x5b
A: be218 d97a3aaf | stzg x15, [x21, #-0x5d0]
A: be55c d501449f | msr S0_1_C4_C4_4, xzr
A: bed08 138426d8 | extr w24, w22, w4, #0x9
A: bef70 d40dc0c2 | hvc #0x6e06
A: bfb68 d50648df | msr S0_6_C4_C8_6, xzr
A: bfffc d504491f | msr S0_4_C4_C9_0, xzr
A: c0324 9adf1096 | irg x22, x4
A: c095c c8737ece | ldxp x14, xzr, [x22]
A: c0c88 d505445f | msr S0_5_C4_C4_2, xzr
A: c0e0c 38971a12 | ldtrsb x18, [x16, #-0x8f]
A: c0e0c d50040ff | msr S0_0_C4_C0_7, xzr
A: c0e34 fc1667f0 | str d16, [sp], #-0x9a
A: c0f08 d507437f | msr S0_7_C4_C3_3, xzr
A: c1018 d5004d3f | xaflag
A: c1654 d5074d5f | msr S0_7_C4_C13_2, xzr
A: c1818 12a00004 | movn w4, #0x0, lsl #16
A: c1e68 d5074eff | msr S0_7_C4_C14_7, xzr
A: c1fdc d2a00019 | movz x25, #0x0, lsl #16
A: c2058 d5024ebf | msr S0_2_C4_C14_5, xzr
A: c236c acec4f58 | ldp q24, q19, [x26], #-0x280
A: c2594 fc4ed2b9 | ldur d25, [x21, #0xed]
A: c2774 d5044f3f | msr S0_4_C4_C15_1, xzr
A: c2d90 d5024a1f | msr S0_2_C4_C10_0, xzr
A: c2e9c 13001d45 | sxtb w5, w10
A: c35e8 9ac714ea | gmi x10, x7, x7
A: c36c8 936324f2 | sbfiz x18, x7, #29, #10
A: c3964 d5044e5f | msr S0_4_C4_C14_2, xzr
A: c39d4 32096ff2 | orr w18, wzr, #0xff87ffff
A: c3c80 d73f09af | blraa x13, x15
A: c3ed0 f823419f | stsmax x3, [x12]
A: c4770 b831217f | steor w17, [x11]
A: c4a0c d5014adf | msr S0_1_C4_C10_6, xzr
A: c4a10 d505487f | msr S0_5_C4_C8_3, xzr
A: c57c0 331f009f | bfi wzr, w4, #1, #1
A: c5878 d504417f | msr S0_4_C4_C1_3, xzr
A: c6854 d5034abf | msr S0_3_C4_C10_5, xzr
A: c6a84 3889298a | ldtrsb x10, [x12, #0x92]
A: c6fec dac11857 | autda x23, x2
A: c7110 d9f77bee | stz2g x14, [sp, #-0x890]
A: c7a90 d503393f | msr S0_3_C3_C9_1, xzr
A: c7b10 9bb6ff8e | umnegl x14, w28, w22
A: c8330 d501453f | msr S0_1_C4_C5_1, xzr
A: c8778 aa1803e4 | mov x4, x24
A: c8aa0 d5074a1f | msr S0_7_C4_C10_0, xzr
A: c9ec0 d501405f | msr S0_1_C4_C0_2, xzr
A: ca05c f914e946 | str x6, [x10, #0x29d0]
A: ca56c d942f0c0 | ldapur x0, [x6, #0x2f]
A: cac68 ab162c1e | adds x30, x0, x22, lsl #11
A: cb1b8 d502437f | msr S0_2_C4_C3_3, xzr
A: cb7d4 d5044d5f | msr S0_4_C4_C13_2, xzr
A: cbf88 9adc0332 | subp x18, x25, x28
A: cc448 d69f0fff | eretab
A: cd924 b88a80c4 | ldursw x4, [x6, #0xa8]
A: ce604 75da38a9 | .inst 0x75da38a9
A: ce7b8 884062c8 | ldxr w8, [x22]
A: ceb6c d503102c | wfit x12
A: cf360 9b5f4e4b | smulh x11, x18, xzr
A: cf520 3861fa1b | ldrb w27, [x16, x1, sxtx #0]
A: cf9c4 3c22da44 | str b4, [x18, w2, sxtw #0]
A: cffa0 9adf1157 | irg x23, x10
A: d01d8 d35f19ee | ubfiz x14, x15, #33, #7
A: d09a0 d5074e3f | msr S0_7_C4_C14_1, xzr
A: d0c6c d63f095f | blraaz x10
A: d1478 d503215f | pacib1716
A: d19ec 194343fe | ldapurb w30, [sp, #0x34]
A: d1b54 e49ddf3a | .inst 0xe49ddf3a
A: d1cd4 d50545ff | msr S0_5_C4_C5_7, xzr
A: d2248 6c80278c | stp d12, d9, [x28], #0x0
A: d24a0 7c50f3fc | ldur h28, [sp, #-0xf1]
A: d2734 fc5f7c64 | ldr d4, [x3, #-0x9]!
A: d2798 d502433f | msr S0_2_C4_C3_1, xzr
A: d3844 d5014f7f | msr S0_1_C4_C15_3, xzr
A: d38ac d5054b5f | msr S0_5_C4_C11_2, xzr
A: d3bcc dac127e8 | pacizb x8
A: d3fb8 3c1e1c5e | str b30, [x2, #-0x1f]!
A: d47c0 d53be35b | mrs x27, CNTV_CVAL_EL0
A: d486c d50442ff | msr S0_4_C4_C2_7, xzr
A: d554c c8dfecb2 | ldar x18, [x5]
A: d5b70 72113c8b | ands w11, w4, #0x7fff8000
A: d5e20 28c06228 | ldp w8, w24, [x17], #0x0
A: d651c 7941b326 | ldrh w6, [x25, #0xd8]
A: d662c 9377fcd0 | asr x16, x6, #55
A: d6708 9b1621db | madd x27, x14, x22, x8
A: d6c60 d51ccd8d | msr ICH_LR12_EL2, x13
A: d6c84 f85a5eca | ldr x10, [x22, #-0x5b]!
A: d6fb4 d5087e9f | dc cigsw, xzr
A: d7968 fc33f859 | str d25, [x2, x19, sxtx #3]
A: d79b8 eb3f30bf | cmp x5, wzr, uxth #4
A: d80b0 d50e9471 | tlbi rpaosnxs, x17
A: d810c 9914b0ae | stlur w14, [x5, #-0xb5]
A: d83e8 93699bea | sbfiz x10, xzr, #23, #39
A: d87d8 d508dcb3 | sys #0x0, c13, c12, #0x5, x19
A: d89f8 d50443bf | msr S0_4_C4_C3_5, xzr
A: d8b38 d50743df | msr S0_7_C4_C3_6, xzr
A: d8c68 ac7c9da1 | ldnp q1, q7, [x13, #-0x70]
A: d9134 38ffe9e4 | ldrsb w4, [x15, xzr, sxtx]
A: d98f4 a93dc112 | stp x18, x16, [x8, #-0x28]
A: d9ae0 f27e38ba | ands x26, x5, #0x1fffc
A: da2c8 93d25874 | extr x20, x3, x18, #0x16
A: da400 d504469f | msr S0_4_C4_C6_4, xzr
A: da56c bc30faaf | str s15, [x21, x16, sxtx #2]
A: da75c d507495f | msr S0_7_C4_C9_2, xzr
A: dad20 540e897a | bc.ge 0xf7e4c
A: db630 130f13d5 | sbfiz w21, w30, #17, #5
A: dbad4 d5074c7f | msr S0_7_C4_C12_3, xzr
A: dbdb8 d5033f9f | dsb sy
A: dc154 6cd53db4 | ldp d20, d15, [x13], #0x150
A: dc1ec d503385f | clrex #0x8
A: dc6ac d63f0e9f | blrabz x20
A: dc7ac d5044c3f | msr S0_4_C4_C12_1, xzr
A: dcc4c d51de32c | msr CNTV_CTL_EL02, x12
A: dcd38 9adc0f6b | sdiv x11, x27, x28
A: dce9c d503100b | wfet x11
A: dd6fc f20f60df | tst x6, #0xfffe03fffffe03ff
A: ddafc 78e6e873 | ldrsh w19, [x3, x6, sxtx]
A: dea34 d502447f | msr S0_2_C4_C4_3, xzr
A: dec44 d65f0fff | retab
A: df048 d505405f | msr S0_5_C4_C0_2, xzr
A: df66c 1aca5339 | crc32cb w25, w25, w10
A: df858 a8e3ecf1 | ldp x17, x27, [x7], #-0x1c8
A: e0778 d506463f | msr S0_6_C4_C6_1, xzr
A: e0d04 d503201f | nop
A: e0d3c 934fff12 | asr x18, x24, #15
A: e1590 d504443f | msr S0_4_C4_C4_1, xzr
A: e18f0 9370bbe4 | sbfiz x4, xzr, #16, #47
A: e19c0 d5074fdf | msr S0_7_C4_C15_6, xzr
A: e1fc0 b80c2508 | str w8, [x8], #0xc2
A: e22a4 d5004edf | msr S0_0_C4_C14_6, xzr
A: e2908 0502da6f | .inst 0x0502da6f
A: e2ce8 d5064c9f | msr S0_6_C4_C12_4, xzr
A: e2e74 54827f4e | b.al 0xfffffffffffe7e5c
A: e35fc c8427ee5 | ldxr x5, [x23]
A: e3620 1ad12187 | lsl w7, w12, w17
A: e3724 d5014c3f | msr S0_1_C4_C12_1, xzr
A: e38d4 7891e306 | ldursh x6, [x24, #-0xe2]
A: e3c64 d5044f9f | msr S0_4_C4_C15_4, xzr
A: e424c d50148df | msr S0_1_C4_C8_6, xzr
A: e45e8 1acf289a | asr w26, w4, w15
A: e4620 d5044a9f | msr S0_4_C4_C10_4, xzr
A: e520c b376b5e3 | bfi x3, x15, #10, #46
A: e5e10 d5014b1f | msr S0_1_C4_C11_0, xzr
A: e6298 d50c938a | tlbi alle1isnxs
A: e6998 dac11d0b | autdb x11, x8
A: e6ac0 d5074b1f | msr S0_7_C4_C11_0, xzr
A: e71d8 d501435f | msr S0_1_C4_C3_2, xzr
A: e779c eb834fe6 | negs x6, x3, asr #19
A: e7a84 d507455f | msr S0_7_C4_C5_2, xzr
A: e8428 d5074d1f | msr S0_7_C4_C13_0, xzr
A: e8438 f80dd4f2 | str x18, [x7], #0xdd
A: e90cc d505461f | msr S0_5_C4_C6_0, xzr
A: e948c d50c81bb | tlbi vale2os, x27
A: e98ac d5044e9f | msr S0_4_C4_C14_4, xzr
A: e9a3c 7212e01f | tst w0, #0x44444444
A: e9af4 722747fe | ands w30, wzr, #0xfe0007ff
A: e9d2c 78942eaa | ldrsh x10, [x21, #-0xbe]!
A: e9e84 c89813dd | stllr x29, [x30]
A: e9e98 d36a8d00 | ubfiz x0, x8, #22, #36
A: ea230 d5014b3f | msr S0_1_C4_C11_1, xzr
A: ea2d4 d5014d9f | msr S0_1_C4_C13_4, xzr
A: ea4f4 d4b0afe2 | dcps2 #0x857f
A: eb274 d50449ff | msr S0_4_C4_C9_7, xzr
A: eb2e0 d50342bf | msr S0_3_C4_C2_5, xzr
A: eb4f8 d5054b1f | msr S0_5_C4_C11_0, xzr
A: eb530 d1beaf0f | subg x15, x24, #0x3e0, #0xb
A: ec1c0 1ade5ba5 | crc32cw w5, w29, w30
A: ec270 d5185132 | msr AFSR1_EL1, x18
A: ec780 3da05fe6 | str q6, [sp, #0x8170]
A: ecf4c 7c1bd77e | str h30, [x27], #-0x43
A: ed3f0 fc52517d | ldur d29, [x11, #-0xdb]
A: edd64 d502403f | msr S0_2_C4_C0_1, xzr
A: ee020 d50143ff | msr S0_1_C4_C3_7, xzr
A: ee3f8 69326921 | stgp x1, x26, [x9, #-0x1c0]
A: ef474 dac11b22 | autda x2, x25
A: efcc0 d504425f | msr S0_4_C4_C2_2, xzr
A: f0c98 d50742df | msr S0_7_C4_C2_6, xzr
A: f0db8 d5300283 | mrs x3, DBGBVR2_EL1
A: f0f40 b8944d25 | ldrsw x5, [x9, #-0xbc]!
A: f1248 3309593f | bfxil wzr, w9, #9, #14
A: f2070 1ac95863 | crc32cw w3, w3, w9
A: f384c d503305f | clrex #0x0
A: f3b90 d50335bf | dmb nshld
A: f5060 7d28c875 | str h21, [x3, #0x1464]
A: f52e8 d71f095c | braa x10, x28
A: f5464 d50322bf | hint #0x15
A: f55e0 f8854005 | prfum pldl3strm, [x0, #0x54]
A: f57f8 d506485f | msr S0_6_C4_C8_2, xzr
A: f57fc d505421f | msr S0_5_C4_C2_0, xzr
A: f5db8 723ae7ff | tst wzr, #0xcccccccc
A: f6aa4 f862f904 | ldr x4, [x8, x2, sxtx #3]
A: f6e4c d5034fbf | msr S0_3_C4_C15_5, xzr
A: f70e8 badf0309 | subps x9, x24, sp
A: f7358 d506487f | msr S0_6_C4_C8_3, xzr
A: f75f4 d508781f | at s1e1r, xzr
A: f76e0 d73f0d26 | blrab x9, x6
A: f7b00 d504459f | msr S0_4_C4_C5_4, xzr
A: f7d7c d505491f | msr S0_5_C4_C9_0, xzr
A: f801c 9ac0164d | gmi x13, x18, x0
A: f8d24 93c496cf | extr x15, x22, x4, #0x25
A: f8dc8 887641be | ldxp w30, w16, [x13]
A: f9318 7836f983 | strh w3, [x12, x22, sxtx #1]
A: f93f8 9360fd7b | asr x27, x11, #32
A: f964c d504411f | msr S0_4_C4_C1_0, xzr
A: f97b0 d5024a7f | msr S0_2_C4_C10_3, xzr
A: f984c d5032c1f | hint #0x60
A: f9864 ea037cbf | tst x5, x3, lsl #31
A: f9d50 b87f6804 | ldr w4, [x0, xzr]
A: f9e20 d5014c7f | msr S0_1_C4_C12_3, xzr
A: fa194 d507443f | msr S0_7_C4_C4_1, xzr
A: fa1e0 d502475f | msr S0_2_C4_C7_2, xzr
A: fa438 d50644bf | msr S0_6_C4_C4_5, xzr
A: fb1f4 f80ed39f | stur xzr, [x28, #0xed]
A: fb4b0 918d416e | addg x14, x11, #0xd0, #0x0
A: fb5d8 d5024e1f | msr S0_2_C4_C14_0, xzr
A: fbe84 dac123fa | paciza x26
A: fc23c 8ad5097f | and xzr, x11, x21, ror #2
A: fc2d4 d506465f | msr S0_6_C4_C6_2, xzr
A: fd0f4 d50146df | msr S0_1_C4_C6_6, xzr
A: fd79c d5054bff | msr S0_5_C4_C11_7, xzr
A: fd894 79d9f2a0 | ldrsh w0, [x21, #0xcf8]
A: fd9fc d50047df | msr S0_0_C4_C7_6, xzr
A: fdba8 d50645bf | msr S0_6_C4_C5_5, xzr
A: fde24 d50445df | msr S0_4_C4_C5_6, xzr
A: fe36c 2dbffb93 | stp s19, s30, [x28, #-0x4]!
A: fedf0 d5054eff | msr S0_5_C4_C14_7, xzr
A: feee4 d5014bff | msr S0_1_C4_C11_7, xzr
A: ff040 d5034b7f | msr S0_3_C4_C11_3, xzr
A: ff55c dac127e4 | pacizb x4
A: ff87c 380c02ee | sturb w14, [x23, #0xc0]
A: ffa58 d503229f | csdb
A: ffdf0 d5074e5f | msr S0_7_C4_C14_2, xzr
A: 1001f8 d50740bf | msr S0_7_C4_C0_5, xzr
A: 100790 d5024b5f | msr S0_2_C4_C11_2, xzr
A: 100850 d52c8bb4 | sysl x20, #0x4, c8, c11, #0x5
A: 100a90 d5034f1f | msr S0_3_C4_C15_0, xzr
A: 100bcc d5074dbf | msr S0_7_C4_C13_5, xzr
A: 101914 9cc6b066 | ldr q6, 0x8ef20
A: 102088 d5019487 | msr S0_1_C9_C4_4, x7
A: 102244 3c8cde80 | str q0, [x20, #0xcd]!
A: 102618 d501493f | msr S0_1_C4_C9_1, xzr
A: 102664 1ac00ff8 | sdiv w24, wzr, w0
A: 102ab4 29e4342e | ldp w14, w13, [x1, #-0xe0]!
A: 103ed4 9b377ede | smull x30, w22, w23
A: 1045c4 d5054cbf | msr S0_5_C4_C12_5, xzr
A: 1052dc 9983d161 | ldapursw x1, [x11, #0x3d]
A: 10532c 3c901c51 | str q17, [x2, #-0xff]!
A: 105418 a9b6b369 | stp x9, x12, [x27, #-0x98]!
A: 105bb8 d4a40003 | dcps3 #0x2000
A: 1063a0 b104f47d | adds x29, x3, #0x13d
A: 106c9c d501407f | msr S0_1_C4_C0_3, xzr
A: 106fb4 d505473f | msr S0_5_C4_C7_1, xzr
A: 107864 8abc4327 | bic x7, x25, x28, asr #16
A: 107910 38a27ae4 | ldrsb x4, [x23, x2, lsl #0]
A: 107a44 d50c971d | tlbi alle2nxs
A: 107aa4 1a84309f | csel wzr, w4, w4, lo
A: 107c14 b3669fe6 | bfxil x6, xzr, #38, #2
A: 107c1c d4a00001 | dcps1
A: 107f10 3cca7716 | ldr q22, [x24], #0xa7
A: 107f2c d50e963f | tlbi rvae3nxs, xzr
A: 108378 3a004a2d | setf16 w17
A: 108abc d50242bf | msr S0_2_C4_C2_5, xzr
A: 108da0 d50649bf | msr S0_6_C4_C9_5, xzr
A: 1093fc d501423f | msr S0_1_C4_C2_1, xzr
A: 109938 d5004bdf | msr S0_0_C4_C11_6, xzr
A: 10a158 c8832dbc | stllr x28, [x13]
A: 10a644 786f7b00 | ldrh w0, [x24, x15, lsl #1]
A: 10abe0 5288f38d | mov w13, #0x479c
A: 10b10c acd0ef14 | ldp q20, q27, [x24], #0x210
A: 10b13c d502491f | msr S0_2_C4_C9_0, xzr
A: 10b960 d9a6547e | st2g x30, [x3], #0x650
A: 10c624 d503101b | wfet x27
A: 10c830 d503207f | wfi
A: 10c864 0d80005e | .inst 0x0d80005e
A: 10cae8 385524ac | ldrb w12, [x5], #-0xae
A: 10cd20 d5189e5e | msr PMINTENCLR_EL1, x30
A: 10ceb4 72238fff | tst wzr, #0xe001e001
A: 10da4c d5054c3f | msr S0_5_C4_C12_1, xzr
A: 10df60 d61f0140 | br x10
A: 10e5c8 1a936741 | csinc w1, w26, w19, vs
A: 10f480 53001cf5 | uxtb w21, w7
A: 10f6fc d5014a9f | msr S0_1_C4_C10_4, xzr
A: 110118 d50645df | msr S0_6_C4_C5_6, xzr
A: 110370 d53ea21b | mrs x27, MAIR_EL3
A: 110390 5a85d0a0 | cinv w0, w5, gt
A: 1109a0 d5044a5f | msr S0_4_C4_C10_2, xzr
A: 111018 78dbb88b | ldtrsh w11, [x4, #-0x45]
A: 1112d8 d5034d1f | msr S0_3_C4_C13_0, xzr
A: 1115e0 d5044dff | msr S0_4_C4_C13_7, xzr
A: 111754 d5064edf | msr S0_6_C4_C14_6, xzr
A: 1119bc d5044b7f | msr S0_4_C4_C11_3, xzr
A: 111ac4 513c4939 | sub w25, w9, #0xf12
A: 112d3c b25ecbeb | mov x11, #-0x3ffe00001
A: 112ea4 784d3169 | ldurh w9, [x11, #0xd3]
A: 1132c0 d5044cbf | msr S0_4_C4_C12_5, xzr
A: 11367c d50147bf | msr S0_1_C4_C7_5, xzr
A: 114280 d5054e9f | msr S0_5_C4_C14_4, xzr
A: 1145b8 d5064a3f | msr S0_6_C4_C10_1, xzr
A: 114884 d505457f | msr S0_5_C4_C5_3, xzr
A: 115028 d5044adf | msr S0_4_C4_C10_6, xzr
A: 1150ac d5024fbf | msr S0_2_C4_C15_5, xzr
A: 115b3c bd41eac1 | ldr s1, [x22, #0x1e8]
A: 1163fc ac7ac359 | ldnp q25, q16, [x26, #-0xb0]
A: 118d8c 110003e9 | mov w9, wsp
A: 1193e4 fc14a037 | stur d23, [x1, #-0xb6]
A: 11948c ededf510 | .inst 0xededf510
A: 119c74 38415003 | ldurb w3, [x0, #0x15]
A: 11a224 78267a40 | strh w0, [x18, x6, lsl #1]
A: 11a604 d5054c5f | msr S0_5_C4_C12_2, xzr
A: 11a9fc d501427f | msr S0_1_C4_C2_3, xzr
A: 11ae8c d50249df | msr S0_2_C4_C9_6, xzr
A: 11b0d0 53194738 | ubfiz w24, w25, #7, #18
A: 11b914 d503461f | msr S0_3_C4_C6_0, xzr
A: 11bc48 d505409f | msr S0_5_C4_C0_4, xzr
A: 11bd28 78dacdcc | ldrsh w12, [x14, #-0x54]!
A: 11bdd8 d4457b60 | hlt #0x2bdb
A: 11c2e8 d50749bf | msr S0_7_C4_C9_5, xzr
A: 11c5b8 d65f0120 | ret x9
A: 11ce40 d51ccd33 | msr ICH_LR9_EL2, x19
A: 11d6e4 ed63f74a | .inst 0xed63f74a
A: 11dd40 d5024d3f | msr S0_2_C4_C13_1, xzr
A: 11e390 d50441bf | msr S0_4_C4_C1_5, xzr
A: 11f114 d50643df | msr S0_6_C4_C3_6, xzr
A: 11f924 d5024dff | msr S0_2_C4_C13_7, xzr
A: 1204fc d5233074 | tstart x20
A: 1208bc b24123f5 | mov x21, #-0x7fffffffffffff01
A: 121668 d501479f | msr S0_1_C4_C7_4, xzr
A: 1217c0 2915b93c | stp w28, w14, [x9, #0xac]
A: 121a20 3d606483 | ldr b3, [x4, #0x819]
A: 121c80 d50543ff | msr S0_5_C4_C3_7, xzr
A: 1225d0 f2110c36 | ands x22, x1, #0x7800000078000
A: 122a14 d53be767 | mrs x7, S3_3_C14_C7_3
A: 123878 dac115a1 | autib x1, x13
A: 1238d8 d506429f | msr S0_6_C4_C2_4, xzr
A: 1240b4 6df43407 | ldp d7, d13, [x0, #-0xc0]!
A: 1248ec b805be54 | str w20, [x18, #0x5b]!
A: 124ea4 d501415f | msr S0_1_C4_C1_2, xzr
A: 124f9c d507409f | msr S0_7_C4_C0_4, xzr
A: 125170 9bb8f372 | umsubl x18, w27, w24, x28
A: 125170 d5044d7f | msr S0_4_C4_C13_3, xzr
A: 125a48 d5064bff | msr S0_6_C4_C11_7, xzr
A: 125a80 d506401f | msr S0_6_C4_C0_0, xzr
A: 126110 0de8c259 | .inst 0x0de8c259
A: 1264b0 a20f9fb5 | .inst 0xa20f9fb5
A: 126868 2a8f6a6d | orr w13, w19, w15, asr #26
A: 126ab4 3a00080d | setf8 w0
A: 12720c d50243ff | msr S0_2_C4_C3_7, xzr
A: 127868 d50decab | sys #0x5, c14, c12, #0x5, x11
A: 127e24 d50047ff | msr S0_0_C4_C7_7, xzr
A: 127f10 3c5e90aa | ldur b10, [x5, #-0x17]
A: 1280bc d505471f | msr S0_5_C4_C7_0, xzr
A: 128264 13003c3a | sxth w26, w1
A: 1288c4 322927f9 | mov w25, #-0x7fffff
A: 1288d8 b80d07b9 | str w25, [x29], #0xd0
A: 1294c4 2b280d8e | adds w14, w12, w8, uxtb #3
A: 12962c 7d5e6f99 | ldr h25, [x28, #0xf36]
A: 129d88 d5074c3f | msr S0_7_C4_C12_1, xzr
A: 129f48 d50b7eb5 | dc cigdvac, x21
A: 12a7dc 595200ed | ldapurh w13, [x7, #-0xe0]
A: 12a89c 295cba81 | ldp w1, w14, [x20, #0xe4]
A: 12ad24 ac0bb5ac | stnp q12, q13, [x13, #0x170]
A: 12ae38 d503373f | msr S0_3_C3_C7_1, xzr
A: 12b128 d5033c1f | msr S0_3_C3_C12_0, xzr
A: 12bed0 d50b7ebf | dc cigdvac, xzr
A: 12d04c d52bea33 | sysl x19, #0x3, c14, c10, #0x1
A: 12d9bc d50341bf | msr S0_3_C4_C1_5, xzr
A: 12de00 cae3acbf | eon xzr, x5, x3, ror #43
A: 12df40 38c05748 | ldrsb w8, [x26], #0x5
A: 12eb10 38ce5f5c | ldrsb w28, [x26, #0xe5]!
A: 12eb58 d5033dff | sb
A: 12f6d0 39bbe860 | ldrsb x0, [x3, #0xefa]
A: 130a84 139f6923 | extr w3, w9, wzr, #0x1a
A: 131644 d50148bf | msr S0_1_C4_C8_5, xzr
A: 1325f4 d503441f | msr S0_3_C4_C4_0, xzr
A: 1327cc d50443ff | msr S0_4_C4_C3_7, xzr
A: 133328 3882b1fb | ldursb x27, [x15, #0x2b]
A: 1339ac d5034cbf | msr S0_3_C4_C12_5, xzr
A: 134670 79ab31fe | ldrsh x30, [x15, #0x1598]
A: 134c0c d5033f5f | clrex
A: 135090 d5074ddf | msr S0_7_C4_C13_6, xzr
A: 1352e8 f2b670cd | movk x13, #0xb386, lsl #16
A: 1366cc d5044abf | msr S0_4_C4_C10_5, xzr
A: 1368bc dac123eb | paciza x11
A: 136ab8 f8207046 | ldumin x0, x6, [x2]
A: 136be8 d262f803 | eor x3, x0, #0xffffffffdfffffff
A: 13723c d504427f | msr S0_4_C4_C2_3, xzr
A: 137c3c a8cfffc5 | ldp x5, xzr, [x30], #0xf8
A: 138808 d504477f | msr S0_4_C4_C7_3, xzr
A: 138f9c 9341f897 | sbfx x23, x4, #1, #62
A: 1399a4 d5024d7f | msr S0_2_C4_C13_3, xzr
A: 139dac 7801292d | sttrh w13, [x9, #0x12]
A: 139fb8 b8224924 | str w4, [x9, w2, uxtw]
A: 13af20 d52e8708 | sysl x8, #0x6, c8, c7, #0x0
A: 13b614 9b367e88 | smull x8, w20, w22
A: 13c030 d5074adf | msr S0_7_C4_C10_6, xzr
A: 13c1d8 d5182021 | msr TTBR1_EL1, x1
A: 13c7f0 d504413f | msr S0_4_C4_C1_1, xzr
A: 13d584 5ac00afd | rev w29, w23
A: 13d7b4 38d23c18 | ldrsb w24, [x0, #-0xdd]!
A: 13e334 f27207ff | tst xzr, #0xc000
A: 13ed10 bc023ea9 | str s9, [x21, #0x23]!
A: 13f2cc cb3fe0bf | sub sp, x5, xzr, sxtx
A: 13f41c d5074b7f | msr S0_7_C4_C11_3, xzr
A: 13f650 d501475f | msr S0_1_C4_C7_2, xzr
A: 13f7d0 d503349f | pssbb
A: 13f820 c83fff84 | stlxp wzr, x4, xzr, [x28]
A: 13fac0 dac133e4 | autiza x4
A: 13fbb0 d525b6bf | mrs xzr, S0_5_C11_C6_5
A: 1405c4 2cef23b1 | ldp s17, s8, [x29], #-0x88
A: 14084c d5034c7f | msr S0_3_C4_C12_3, xzr
A: 140f10 d50338bf | dmb #8
A: 141038 d502495f | msr S0_2_C4_C9_2, xzr
A: 14178c b888203f | ldursw xzr, [x1, #0x82]
A: 141d14 d345ffee | lsr x14, xzr, #5
A: 1420e0 dac0150f | cls x15, x8
A: 142538 d503213f | hint #0x9
A: 142800 d50444ff | msr S0_4_C4_C4_7, xzr
A: 142bb4 d506457f | msr S0_6_C4_C5_3, xzr
A: 142fe4 382c7876 | strb w22, [x3, x12, lsl #0]
A: 1433f4 d506411f | msr S0_6_C4_C1_0, xzr
A: 1439c8 d5064aff | msr S0_6_C4_C10_7, xzr
A: 143aa4 d50143bf | msr S0_1_C4_C3_5, xzr
A: 143e6c b822637f | stumax w2, [x27]
A: 14456c 04a53e23 | .inst 0x04a53e23
A: 144630 f87495b9 | ldraa x25, [x13, #-0x5b8]
A: 144858 934769bf | sbfx xzr, x13, #7, #20
A: 144e60 b84551af | ldur w15, [x13, #0x55]
A: 14560c d50649df | msr S0_6_C4_C9_6, xzr
A: 145620 d518e11d | msr CNTKCTL_EL1, x29
A: 1459c0 28efa8fd | ldp w29, w10, [x7], #-0x84
A: 146b3c d5014e5f | msr S0_1_C4_C14_2, xzr
A: 147138 d50643bf | msr S0_6_C4_C3_5, xzr
A: 147278 ba40abef | ccmn xzr, #0x0, #0xf, ge
A: 147768 d5014d7f | msr S0_1_C4_C13_3, xzr
A: 1478fc d504451f | msr S0_4_C4_C5_0, xzr
A: 147d40 d507475f | msr S0_7_C4_C7_2, xzr
A: 147e90 d50544bf | msr S0_5_C4_C4_5, xzr
A: 1488b8 68efb562 | ldpsw x2, x13, [x11], #-0x84
A: 148be8 d5054adf | msr S0_5_C4_C10_6, xzr
A: 14960c d520ca5f | mrs xzr, S0_0_C12_C10_2
A: 14a2a0 ea435fff | tst xzr, x3, lsr #23
A: 14b72c d503217f | hint #0xb
A: 14bd4c d71f08f2 | braa x7, x18
A: 14bd78 d5300da9 | mrs x9, DBGBCR13_EL1
A: 14bf1c d522679f | mrs xzr, S0_2_C6_C7_4
A: 14c004 f8bfc301 | ldapr x1, [x24]
A: 14c13c 3c4465aa | ldr b10, [x13], #0x46
A: 14c5d8 5a9bc3ff | csinv wzr, wzr, w27, gt
A: 14c71c d5004eff | msr S0_0_C4_C14_7, xzr
A: 14c738 d504409f | msr S0_4_C4_C0_4, xzr
A: 14c924 d5064f7f | msr S0_6_C4_C15_3, xzr
A: 14cb44 d50748bf | msr S0_7_C4_C8_5, xzr
A: 14cbd8 d50336bf | dmb nshst
A: 14cee4 d5385513 | mrs x19, S3_0_C5_C5_0
A: 14cf30 da1102ec | sbc x12, x23, x17
A: 14d798 d50640ff | msr S0_6_C4_C0_7, xzr
A: 14d96c 120940c3 | and w3, w6, #0xff8000ff
A: 14de08 d5064ddf | msr S0_6_C4_C13_6, xzr
A: 14e100 9bad7c42 | umull x2, w2, w13
A: 14e5d8 7226f037 | ands w23, w1, #0x55555555
A: 14e764 0de5c98e | .inst 0x0de5c98e
A: 14f790 d5044aff | msr S0_4_C4_C10_7, xzr
A: 14f8a4 d505451f | msr S0_5_C4_C5_0, xzr
A: 14fb84 7c50bde2 | ldr h2, [x15, #-0xf5]!
A: 14fef4 9365fe40 | asr x0, x18, #37
A: 150090 e82e798d | .inst 0xe82e798d
A: 1501b0 38e27a5f | ldrsb wzr, [x18, x2, lsl #0]
A: 150504 d5074dff | msr S0_7_C4_C13_7, xzr
A: 150728 d50891ab | tlbi vale1osnxs, x11
A: 150884 d5024bbf | msr S0_2_C4_C11_5, xzr
A: 150a80 d50642df | msr S0_6_C4_C2_6, xzr
A: 150c08 b850a58a | ldr w10, [x12], #-0xf6
A: 151bac 9b473526 | smulh x6, x9, x7
A: 152668 b80f2f2c | str w12, [x25, #0xf2]!
A: 152b0c d00704cd | adrp x13, 0xe1ec000
A: 152bf4 d34313fc | ubfx x28, xzr, #3, #2
A: 1533a4 4adf1fe6 | eor w6, wzr, wzr, ror #7
A: 153e8c d4a1c841 | dcps1 #0xe42
A: 15460c 38081236 | sturb w22, [x17, #0x81]
A: 1546fc 53037fa4 | lsr w4, w29, #3
A: 154720 d506475f | msr S0_6_C4_C7_2, xzr
A: 1552a8 d500e39f | msr S0_0_C14_C3_4, xzr
A: 1558dc d501477f | msr S0_1_C4_C7_3, xzr
A: 155b70 d502417f | msr S0_2_C4_C1_3, xzr
A: 155d84 d503467f | smstop
A: 155fec d504419f | msr S0_4_C4_C1_4, xzr
A: 156134 d344fc49 | lsr x9, x2, #4
A: 156928 d5024ddf | msr S0_2_C4_C13_6, xzr
A: 156c0c d502469f | msr S0_2_C4_C6_4, xzr
A: 1574f4 f8244f8a | ldraa x10, [x28, #0x220]!
A: 157d50 3900c252 | strb w18, [x18, #0x30]
A: 157da4 d5034dbf | msr S0_3_C4_C13_5, xzr
A: 158c98 8a5d4511 | and x17, x8, x29, lsr #17
A: 1593c8 1aca4256 | crc32b w22, w18, w10
A: 159488 a11a94d3 | .inst 0xa11a94d3
A: 159d18 d501445f | msr S0_1_C4_C4_2, xzr
A: 159fdc d5064a1f | msr S0_6_C4_C10_0, xzr
A: 15a0f0 a9306d36 | stp x22, x27, [x9, #-0x100]
A: 15a1a4 cb3fb3e6 | sub x6, sp, wzr, sxth #4
A: 15a76c dac12bfe | pacdza x30
A: 15ab34 d5044bdf | msr S0_4_C4_C11_6, xzr
A: 15ab94 d50748df | msr S0_7_C4_C8_6, xzr
A: 15b2a4 f83f91f4 | st64b x20, [x15]
A: 15b2f4 1901f1a7 | stlurb w7, [x13, #0x1f]
A: 15b7c8 d505433f | msr S0_5_C4_C3_1, xzr
A: 15beec d52b651f | sysl xzr, #0x3, c6, c5, #0x0
A: 15bf54 3cbfca24 | str q4, [x17, wzr, sxtw]
A: 15c714 93407fdf | sxtw xzr, w30
A: 15ce98 2de65c4f | ldp s15, s23, [x2, #-0xd0]!
A: 15cefc d5014e3f | msr S0_1_C4_C14_1, xzr
A: 15d008 a8a4a043 | stp x3, x8, [x2], #-0x1b8
A: 15d03c 93500155 | sbfiz x21, x10, #48, #1
A: 15d1f4 d50147ff | msr S0_1_C4_C7_7, xzr
A: 15d68c d505437f | msr S0_5_C4_C3_3, xzr
A: 15d950 d518c92d | msr ICC_AP1R1_EL1, x13
A: 15df0c dac143f4 | xpaci x20
A: 15eae0 7c548f1a | ldr h26, [x24, #-0xb8]!
A: 15ece0 d5064b9f | msr S0_6_C4_C11_4, xzr
A: 15f5a4 b3453a7f | bfxil xzr, x19, #5, #10
A: 15f5ec e8d2ef60 | .inst 0xe8d2ef60
A: 160184 d51ccd40 | msr ICH_LR10_EL2, x0
A: 16048c fd5654ed | ldr d13, [x7, #0x2ca8]
A: 1609d0 d5054a7f | msr S0_5_C4_C10_3, xzr
A: 160a50 590c317e | stlurh w30, [x11, #0xc3]
A: 160af8 3859781b | ldtrb w27, [x0, #-0x69]
A: 160b68 591b51b5 | stlurh w21, [x13, #-0x4b]
A: 161068 f82b816a | swp x11, x10, [x11]
A: 161660 d503221f | esb
A: 162208 d9a00158 | stgm x24, [x10]
A: 162cc4 d5074abf | msr S0_7_C4_C10_5, xzr
A: 163a60 39efaf54 | ldrsb w20, [x26, #0xbeb]
A: 165030 d504429f | msr S0_4_C4_C2_4, xzr
A: 165510 d530024b | mrs x11, MDSCR_EL1
A: 165f64 d504437f | msr S0_4_C4_C3_3, xzr
A: 1665ac d5004bff | msr S0_0_C4_C11_7, xzr
A: 166b40 93c26c80 | extr x0, x4, x2, #0x1b
A: 166b78 d50145df | msr S0_1_C4_C5_6, xzr
A: 166c5c d507445f | msr S0_7_C4_C4_2, xzr
A: 167b40 d50246ff | msr S0_2_C4_C6_7, xzr
A: 167eac 1ac32dca | ror w10, w14, w3
A: 168e0c d51079d0 | msr DBGCLAIMCLR_EL1, x16
A: 169118 d5033d3f | msr S0_3_C3_C13_1, xzr
A: 169490 d5054c7f | msr S0_5_C4_C12_3, xzr
A: 169628 d50245bf | msr S0_2_C4_C5_5, xzr
A: 16a164 281da60c | stnp w12, w9, [x16, #0xec]
A: 16a2c4 d506459f | msr S0_6_C4_C5_4, xzr
A: 16b5d4 92e0001e | movn x30, #0x0, lsl #48
A: 16b7e0 d5044e7f | msr S0_4_C4_C14_3, xzr
A: 16bedc d503481f | msr S0_3_C4_C8_0, xzr
A: 16bfd4 d506471f | msr S0_6_C4_C7_0, xzr
A: 16c400 9ad75d1f | crc32cx wzr, w8, x23
A: 16ca5c f8345091 | ldsmin x20, x17, [x4]
A: 16cb8c 2b37243f | cmn w1, w23, uxth #1
A: 16d134 7c1b76f9 | str h25, [x23], #-0x49
A: 16d240 d505489f | msr S0_5_C4_C8_4, xzr
A: 16f378 4b5f1bff | neg wzr, wzr, lsr #6
A: 16f75c d500433f | xaflag
A: 16ff10 da89c522 | cneg x2, x9, le
A: 1702d0 d5054a5f | msr S0_5_C4_C10_2, xzr
A: 170334 fd1e12db | str d27, [x22, #0x3c20]
A: 17045c d34ee7ee | ubfx x14, xzr, #14, #44
A: 170768 d73f0a75 | blraa x19, x21
A: 170a8c d506425f | msr S0_6_C4_C2_2, xzr
A: 170b68 f83f928c | st64b x12, [x20]
A: 170e64 38d3f03e | ldursb w30, [x1, #-0xc1]
A: 170fbc d505443f | msr S0_5_C4_C4_1, xzr
A: 171288 d9f7f5a0 | stz2g x0, [x13], #-0x810
A: 171310 59d9e059 | ldapursh w25, [x2, #-0x62]
A: 171730 d2cb0832 | mov x18, #0x584100000000
A: 171d68 d5014cdf | msr S0_1_C4_C12_6, xzr
A: 172bec d50545df | msr S0_5_C4_C5_6, xzr
A: 1733f4 186b6125 | ldr w5, 0x24a018
A: 17372c d501467f | msr S0_1_C4_C6_3, xzr
A: 173da0 d50e92bf | tlbi rvale3isnxs, xzr
A: 17411c ad9467d5 | stp q21, q25, [x30, #0x280]!
A: 1744a4 d9ef4f30 | stz2g x16, [x25, #0xf40]!
A: 1744b8 d508977f | tlbi vaae1nxs, xzr
A: 174abc d5033c9f | dfb
A: 174f6c d9f3cd07 | stz2g x7, [x8, #-0xc40]!
A: 175660 c820c519 | stlxp w0, x25, x17, [x8]
A: 175c50 d5014a1f | msr S0_1_C4_C10_0, xzr
A: 175f4c d9ecef3f | stz2g sp, [x25, #0xce0]!
A: 176758 d53ccdc8 | mrs x8, ICH_LR14_EL2
A: 176794 d506419f | msr S0_6_C4_C1_4, xzr
A: 176a14 d502459f | msr S0_2_C4_C5_4, xzr
A: 176d70 d5004cff | msr S0_0_C4_C12_7, xzr
A: 17704c a9fcee7f | ldp xzr, x27, [x19, #-0x38]!
A: 177d5c d505477f | msr S0_5_C4_C7_3, xzr
A: 177db0 3100001f | cmn w0, #0x0
A: 17878c d502411f | msr S0_2_C4_C1_0, xzr
A: 1787c4 ab187c9f | cmn x4, x24, lsl #31
A: 1787d0 d50745bf | msr S0_7_C4_C5_5, xzr
A: 1799d4 9bd3472e | umulh x14, x25, x19
A: 179b88 d502429f | msr S0_2_C4_C2_4, xzr
A: 179d7c 93c784ea | ror x10, x7, #0x21
A: 17a148 b8bfc1a2 | ldapr w2, [x13]
A: 17a674 d50248ff | msr S0_2_C4_C8_7, xzr
A: 17ad48 d50441ff | msr S0_4_C4_C1_7, xzr
A: 17b30c d502465f | msr S0_2_C4_C6_2, xzr
A: 17b418 d53beac7 | mrs x7, PMEVCNTR22_EL0
A: 17b46c d5074b3f | msr S0_7_C4_C11_1, xzr
A: 17b9fc d5044bbf | msr S0_4_C4_C11_5, xzr
A: 17d060 d5054bbf | msr S0_5_C4_C11_5, xzr
A: 17d184 190622cb | stlurb w11, [x22, #0x62]
A: 17d7a0 d5054f9f | msr S0_5_C4_C15_4, xzr
A: 17d970 d5054f1f | msr S0_5_C4_C15_0, xzr
A: 17e29c 6b23439d | subs w29, w28, w3, uxtw
A: 17e44c d50246bf | msr S0_2_C4_C6_5, xzr
A: 17e588 da0b03c7 | sbc x7, x30, x11
A: 17e9f8 d50449bf | msr S0_4_C4_C9_5, xzr
A: 17eaa4 7898d410 | ldrsh x16, [x0], #-0x73
A: 17eefc d5014abf | msr S0_1_C4_C10_5, xzr
A: 17ef88 69a1724f | stgp x15, x28, [x18, #-0x3e0]!
A: 17f408 7807671a | strh w26, [x24], #0x76
A: 17f8a8 d506477f | msr S0_6_C4_C7_3, xzr
A: 17fb4c d53cc979 | mrs x25, ICH_AP1R3_EL2
A: 1804a4 d53f398c | mrs x12, S3_7_C3_C9_4
A: 181770 d5054a1f | msr S0_5_C4_C10_0, xzr
A: 182c14 d50346bf | msr S0_3_C4_C6_5, xzr
A: 182f7c d523cd5f | mrs xzr, S0_3_C12_C13_2
A: 1838c4 d5024f9f | msr S0_2_C4_C15_4, xzr
A: 183b10 d5034e7f | msr S0_3_C4_C14_3, xzr
A: 183f34 d5044b1f | msr S0_4_C4_C11_0, xzr
A: 1841ac dac103ca | pacia x10, x30
A: 18494c d50747df | msr S0_7_C4_C7_6, xzr
A: 18559c d37323ef | ubfiz x15, xzr, #13, #9
A: 185760 d50240df | msr S0_2_C4_C0_6, xzr
A: 18582c d45f00e0 | hlt #0xf807
A: 186398 d50347bf | msr S0_3_C4_C7_5, xzr
A: 186a80 78d61443 | ldrsh w3, [x2], #-0x9f
A: 186c78 d50040df | msr S0_0_C4_C0_6, xzr
A: 187094 5a84a09f | cinv wzr, w4, lt
A: 18717c d503103c | wfit x28
A: 18717c d518af51 | msr S3_0_C10_C15_2, x17
A: 1871ac d50749ff | msr S0_7_C4_C9_7, xzr
A: 187b0c d503447f | smstop za
A: 187b48 fc336b7a | str d26, [x27, x19]
A: 188e9c d5074cbf | msr S0_7_C4_C12_5, xzr
A: 1895f8 d50e979f | tlbi paallnxs
A: 189a18 d505467f | msr S0_5_C4_C6_3, xzr
A: 189afc d65f0020 | ret x1
A: 189e10 eb16f7ec | negs x12, x22, lsl #61
A: 18a8f0 d5034d9f | msr TCO, #0xd
A: 18b7a0 dac00f76 | rev x22, x27
A: 18bae4 3857e37a | ldurb w26, [x27, #-0x82]
A: 18bcac d507401f | msr S0_7_C4_C0_0, xzr
A: 18be48 13001e66 | sxtb w6, w19
A: 18c058 d503223f | psb csync
A: 18c55c 92c0001b | movn x27, #0x0, lsl #32
A: 18c690 138d5496 | extr w22, w4, w13, #0x15
A: 18cda8 d504495f | msr S0_4_C4_C9_2, xzr
A: 18d2ec d507451f | msr S0_7_C4_C5_0, xzr
A: 18de08 d50320bf | sevl
A: 18de1c 331a2402 | bfi w2, w0, #6, #10
A: 18e270 722fcd5f | tst w10, #0x1e1e1e1e
A: 18e2b4 d507429f | msr S0_7_C4_C2_4, xzr
A: 18e3c4 d503227f | gcsb dsync
A: 18e4dc d5300c85 | mrs x5, DBGBVR12_EL1
A: 18ee6c d50044ff | msr S0_0_C4_C4_7, xzr
A: 18fc88 d4b0a662 | dcps2 #0x8533
A: 1901a0 d5074f9f | msr S0_7_C4_C15_4, xzr
A: 1901a8 ea00e8df | tst x6, x0, lsl #58
A: 19023c f800741e | str x30, [x0], #0x7
A: 1904fc fa150136 | sbcs x22, x9, x21
A: 190af8 d5054fff | msr S0_5_C4_C15_7, xzr
A: 190de8 d43f0040 | brk #0xf802
A: 190fcc d5014e7f | msr S0_1_C4_C14_3, xzr
A: 1915f4 d347cc99 | ubfx x25, x4, #7, #45
A: 19161c fc4bc5a5 | ldr d5, [x13], #0xbc
A: 191c48 1b167f7c | mul w28, w27, w22
A: 19214c d505425f | msr S0_5_C4_C2_2, xzr
A: 19242c d507447f | msr S0_7_C4_C4_3, xzr
A: 192de4 d504445f | msr S0_4_C4_C4_2, xzr
A: 19303c d50646df | msr S0_6_C4_C6_6, xzr
A: 1932f0 d50549ff | msr S0_5_C4_C9_7, xzr
A: 1937cc d5113220 | msr S2_1_C3_C2_1, x0
A: 1938ec 9ac72d0e | ror x14, x8, x7
A: 193a18 b17b5bfc | adds x28, sp, #0xed6, lsl #12
A: 193a98 138674db | ror w27, w6, #0x1d
A: 194070 ac1bffff | stnp q31, q31, [sp, #0x370]
A: 19443c 531924f0 | ubfiz w16, w7, #7, #10
A: 194470 dac12bfc | pacdza x28
A: 194ecc 78c82600 | ldrsh w0, [x16], #0x82
A: 1951ec b8387943 | str w3, [x10, x24, lsl #2]
A: 19528c d50328bf | hint #0x45
A: 195898 fc6b4a13 | ldr d19, [x16, w11, uxtw]
A: 195cfc 9bb97c70 | umull x16, w3, w25
A: 1962b0 d507403f | msr S0_7_C4_C0_1, xzr
A: 1973e0 d5100ddd | msr DBGWVR13_EL1, x29
A: 197560 d500441f | cfinv
A: 197730 00006acd | udf #0x6acd
A: 198600 d5064d1f | msr S0_6_C4_C13_0, xzr
A: 198668 d5034e1f | msr S0_3_C4_C14_0, xzr
A: 198760 d4130ea3 | smc #0x9875
A: 198c38 dac133fa | autiza x26
A: 1991d0 1acb2035 | lsl w21, w1, w11
A: 199248 937201c1 | sbfiz x1, x14, #14, #1
A: 199364 d50046df | msr S0_0_C4_C6_6, xzr
A: 19a24c 38009b3b | sttrb w27, [x25, #0x9]
A: 19ac9c d51be345 | msr CNTV_CVAL_EL0, x5
A: 19b474 d5074d3f | msr S0_7_C4_C13_1, xzr
A: 19bb80 d50c8447 | tlbi ripas2e1, x7
A: 19bc50 d50548ff | msr S0_5_C4_C8_7, xzr
A: 19c734 d1b16cef | subg x15, x7, #0x310, #0xb
A: 19cf3c 925ceb9c | and x28, x28, #0xfffffff07fffffff
A: 19d6f4 f8447531 | ldr x17, [x9], #0x47
A: 19ded8 fc3febbf | str d31, [x29, xzr, sxtx]
A: 19e1cc 5ac003e9 | rbit w9, wzr
A: 19e818 eb89c425 | subs x5, x1, x9, asr #49
A: 19e95c d506467f | msr S0_6_C4_C6_3, xzr
A: 19eb98 5c14ca4a | ldr d10, 0x1c84e0
A: 19ed48 f822817f | swp x2, xzr, [x11]
A: 19ee98 f80a83ef | stur x15, [sp, #0xa8]
A: 19f7d4 243ff3e1 | .inst 0x243ff3e1
A: 19f994 d506473f | msr S0_6_C4_C7_1, xzr
A: 19fab4 d5014e1f | msr S0_1_C4_C14_0, xzr
A: 19fef0 d502423f | msr S0_2_C4_C2_1, xzr
A: 1a06b8 788c0e89 | ldrsh x9, [x20, #0xc0]!
A: 1a07b0 adfded5d | ldp q29, q27, [x10, #-0x50]!
A: 1a0a50 ba008686 | rmif x20, #0x1, #0x6
A: 1a0ab0 d5074cff | msr S0_7_C4_C12_7, xzr
A: 1a0b28 d50876bf | dc igdvac, xzr
A: 1a0ee8 d50141ff | msr S0_1_C4_C1_7, xzr
A: 1a139c d50140df | msr S0_1_C4_C0_6, xzr
A: 1a1c04 d50330bf | dmb #0
A: 1a1cb4 9376dd47 | sbfx x7, x10, #54, #2
A: 1a1da4 d507441f | msr S0_7_C4_C4_0, xzr
A: 1a2004 d5064d9f | msr S0_6_C4_C13_4, xzr
A: 1a2594 d50332df | isb #2
A: 1a2920 d5074edf | msr S0_7_C4_C14_6, xzr
A: 1a2e70 93401cc9 | sxtb x9, w6
A: 1a2efc d505481f | msr S0_5_C4_C8_0, xzr
A: 1a3070 29bcd720 | stp w0, w21, [x25, #-0x1c]!
A: 1a35b4 dac13bfa | autdza x26
A: 1a438c 784f277e | ldrh w30, [x27], #0xf2
A: 1a4770 d538a44d | mrs x13, LORN_EL1
A: 1a5820 a01578f2 | .inst 0xa01578f2
A: 1a5cac d29ad85d | mov x29, #0xd6c2
A: 1a5cd0 f8a6fa5a | prfm #0x1a, [x18, x6, sxtx #3]
A: 1a5e48 d50240bf | msr S0_2_C4_C0_5, xzr
A: 1a5e68 d50644ff | msr S0_6_C4_C4_7, xzr
A: 1a60c4 68b2f98d | stgp x13, x30, [x12], #-0x1b0
A: 1a6318 d96c0092 | ldg x18, [x4, #0xc00]
A: 1a6d3c d5074bbf | msr S0_7_C4_C11_5, xzr
A: 1a6f68 9b387c07 | smull x7, w0, w24
A: 1a7780 d50c82be | tlbi rvale2is, x30
A: 1a7904 a8747f92 | ldnp x18, xzr, [x28, #-0xc0]
A: 1a7fd8 d50444bf | msr S0_4_C4_C4_5, xzr
A: 1a83c0 9100017f | mov sp, x11
A: 1a8e4c 9340ff13 | asr x19, x24, #0
A: 1a8f58 d50643ff | msr S0_6_C4_C3_7, xzr
A: 1a8f98 bc0566b1 | str s17, [x21], #0x56
A: 1a93f0 d4a0e103 | dcps3 #0x708
A: 1a9488 d506435f | msr S0_6_C4_C3_2, xzr
A: 1a9548 d50244df | msr S0_2_C4_C4_6, xzr
A: 1a9910 7889ea17 | ldtrsh x23, [x16, #0x9e]
A: 1a9b9c f91e0bd8 | str x24, [x30, #0x3c10]
A: 1a9e7c 9a84a082 | csel x2, x4, x4, ge
A: 1aa3f4 3ca9ebe5 | str q5, [sp, x9, sxtx]
A: 1aba08 d506489f | msr S0_6_C4_C8_4, xzr
A: 1abac0 d50334df | isb #4
A: 1ac12c d50244ff | msr S0_2_C4_C4_7, xzr
A: 1accdc d51ce111 | msr CNTHCTL_EL2, x17
A: 1acce8 78839c09 | ldrsh x9, [x0, #0x39]!
A: 1ad2e8 d51827f2 | msr TCR2ALIAS_EL1, x18
A: 1ad7b8 d5024aff | msr S0_2_C4_C10_7, xzr
A: 1ad8b0 38ffd97f | ldrsb wzr, [x11, wzr, sxtw #0]
A: 1ae1e8 d5014b9f | msr S0_1_C4_C11_4, xzr
A: 1ae240 d502487f | msr S0_2_C4_C8_3, xzr
A: 1ae8a4 d50147df | msr S0_1_C4_C7_6, xzr
A: 1ae8c8 d5074c9f | msr S0_7_C4_C12_4, xzr
A: 1aee90 d508933f | tlbi vae1isnxs, xzr
A: 1af31c fc02cf9f | str d31, [x28, #0x2c]!
A: 1af908 d50140ff | msr S0_1_C4_C0_7, xzr
A: 1afbc4 3cff7aa4 | ldr q4, [x21, xzr, lsl #4]
A: 1b055c b82210a4 | ldclr w2, w4, [x5]
A: 1b0b20 d63f0b9f | blraaz x28
A: 1b0d24 d503407f | msr S0_3_C4_C0_3, xzr
A: 1b10ac d50042df | msr S0_0_C4_C2_6, xzr
A: 1b1620 78519d06 | ldrh w6, [x8, #-0xe7]!
A: 1b1878 fc68c9e8 | ldr d8, [x15, w8, sxtw]
A: 1b2108 d5024e9f | msr S0_2_C4_C14_4, xzr
A: 1b2158 d5014dff | msr S0_1_C4_C13_7, xzr
A: 1b236c d503451f | msr S0_3_C4_C5_0, xzr
A: 1b23b8 d50743ff | msr S0_7_C4_C3_7, xzr
A: 1b2548 dac137f2 | autizb x18
A: 1b2740 93599d49 | sbfx x9, x10, #25, #15
A: 1b2e88 d5054d3f | msr S0_5_C4_C13_1, xzr
A: 1b2f18 b8aa5b2c | ldrsw x12, [x25, w10, uxtw #2]
A: 1b3678 530c574d | ubfx w13, w26, #12, #10
A: 1b39fc d5024abf | msr S0_2_C4_C10_5, xzr
A: 1b3cbc d976619e | ldg x30, [x12, #-0x9a0]
A: 1b40b0 38968e86 | ldrsb x6, [x20, #-0x98]!
A: 1b422c d506461f | msr S0_6_C4_C6_0, xzr
A: 1b4314 bd2ac1f0 | str s16, [x15, #0x2ac0]
A: 1b43c0 d5380608 | mrs x8, ID_AA64ISAR0_EL1
A: 1b49fc f84bcd4f | ldr x15, [x10, #0xbc]!
A: 1b4a0c d5054d1f | msr S0_5_C4_C13_0, xzr
A: 1b5c88 c8810b72 | stllr x18, [x27]
A: 1b5d54 f2d8a1e3 | movk x3, #0xc50f, lsl #32
A: 1b620c 9a0703b2 | adc x18, x29, x7
A: 1b6508 789eb2f2 | ldursh x18, [x23, #-0x15]
A: 1b66b8 d5024e3f | msr S0_2_C4_C14_1, xzr
A: 1b688c d504441f | msr S0_4_C4_C4_0, xzr
A: 1b6f70 39e27d4f | ldrsb w15, [x10, #0x89f]
A: 1b73ac 0f045ba2 | .inst 0x0f045ba2
A: 1b785c b83821b4 | ldeor w24, w20, [x13]
A: 1b81cc 9a1a0289 | adc x9, x20, x26
A: 1b83a0 5c9434f6 | ldr d22, 0xe0a3c
A: 1b8edc 78cc9496 | ldrsh w22, [x4], #0xc9
A: 1b94f4 f827023f | stadd x7, [x17]
A: 1ba21c d502497f | msr S0_2_C4_C9_3, xzr
A: 1ba324 2cf8782b | ldp s11, s30, [x1], #-0x40
A: 1baa10 d69f0bff | eretaa
A: 1bae38 d50045ff | msr S0_0_C4_C5_7, xzr
A: 1bbd1c f26b97b0 | ands x16, x29, #0x7ffffffffe00000
A: 1bc6e0 d504453f | msr S0_4_C4_C5_1, xzr
A: 1bc718 530522e4 | ubfx w4, w23, #5, #4
A: 1bc730 d50345bf | msr S0_3_C4_C5_5, xzr
A: 1bcb18 f862da44 | ldr x4, [x18, w2, sxtw #3]
A: 1bd3f4 53003d4d | uxth w13, w10
A: 1bd420 d5014d5f | msr S0_1_C4_C13_2, xzr
A: 1bd7dc d5064cdf | msr S0_6_C4_C12_6, xzr
A: 1bddfc d501457f | msr S0_1_C4_C5_3, xzr
A: 1bde9c b245a3ec | mov x12, #-0x7fffff000000001
A: 1be180 fc22d93a | str d26, [x9, w2, sxtw #3]
A: 1be2e4 d5034f7f | msr S0_3_C4_C15_3, xzr
A: 1be5a0 d50248bf | msr S0_2_C4_C8_5, xzr
A: 1be618 b88907e5 | ldrsw x5, [sp], #0x90
A: 1beaec d5014bdf | msr S0_1_C4_C11_6, xzr
A: 1bf524 28bfc6b9 | stp w25, w17, [x21], #-0x4
A: 1bf53c dac137ea | autizb x10
A: 1bfa58 7c0d4d57 | str h23, [x10, #0xd4]!
A: 1c0104 d50247ff | msr S0_2_C4_C7_7, xzr
A: 1c0180 d51bead8 | msr PMEVCNTR22_EL0, x24
A: 1c08e0 93c06f5f | extr xzr, x26, x0, #0x1b
A: 1c0c88 d50742ff | msr S0_7_C4_C2_7, xzr
A: 1c11ec 934b5343 | sbfx x3, x26, #11, #10
A: 1c15fc 1ac749f6 | crc32w w22, w15, w7
A: 1c18d8 d50049ff | msr S0_0_C4_C9_7, xzr
A: 1c23c0 d50547df | msr S0_5_C4_C7_6, xzr
A: 1c2a3c d502473f | msr S0_2_C4_C7_1, xzr
A: 1c3448 f8ff56b2 | ldrab x18, [x21, #-0x58]
A: 1c35dc bc6e4ad6 | ldr s22, [x22, w14, uxtw]
A: 1c36e0 dac12bf8 | pacdza x24
A: 1c4664 d50747bf | msr S0_7_C4_C7_5, xzr
A: 1c4688 39b558a5 | ldrsb x5, [x5, #0xd56]
A: 1c48bc 5892c44a | ldr x10, 0xea144
A: 1c5040 5ac0102a | clz w10, w1
A: 1c54e4 f862fd1f | ldraa xzr, [x8, #-0xe88]!
A: 1c5708 d502443f | msr S0_2_C4_C4_1, xzr
A: 1c5a48 d5054bdf | msr S0_5_C4_C11_6, xzr
A: 1c5fb4 d51c1143 | msr CPTR_EL2, x3
A: 1c60fc 1d963a01 | .inst 0x1d963a01
A: 1c6180 d505439f | msr S0_5_C4_C3_4, xzr
A: 1c6310 d5033cff | sb
A: 1c63ec 2c45c128 | ldnp s8, s16, [x9, #0x2c]
A: 1c66e4 b36b6bff | bfi xzr, xzr, #21, #27
A: 1c6b84 934f8ffa | sbfx x26, xzr, #15, #21
A: 1c6e64 d50149df | msr S0_1_C4_C9_6, xzr
A: 1c6ee4 d501455f | msr S0_1_C4_C5_2, xzr
A: 1c7744 d50be77a | sys #0x3, c14, c7, #0x3, x26
A: 1c7a98 d506441f | msr S0_6_C4_C4_0, xzr
A: 1c7db0 3c5fc4ac | ldr b12, [x5], #-0x4
A: 1c8690 d5233066 | tstart x6
A: 1c8894 f2d98047 | movk x7, #0xcc02, lsl #32
A: 1c88ec 4dfce17c | .inst 0x4dfce17c
A: 1c8c8c 9bc95cb3 | umulh x19, x5, x9
A: 1c96b8 d50146ff | msr S0_1_C4_C6_7, xzr
A: 1c9820 d5024adf | msr S0_2_C4_C10_6, xzr
A: 1c9a48 d503339f | dsb osh
A: 1c9ad4 cb2deee3 | sub x3, x23, x13, sxtx #3
A: 1c9b84 d502489f | msr S0_2_C4_C8_4, xzr
A: 1ca334 b8049098 | stur w24, [x4, #0x49]
A: 1cb83c 8aca7934 | and x20, x9, x10, ror #30
A: 1cc704 3da3977c | str q28, [x27, #0x8e50]
A: 1ccb10 d50337df | isb #7
A: 1cd4dc 3c1f519f | stur b31, [x12, #-0xb]
A: 1cd67c 0b3f2bff | add wsp, wsp, wzr, uxth #2
A: 1cd90c 2b07427f | cmn w19, w7, lsl #16
A: 1ce1d4 ba52e205 | ccmn x16, x18, #0x5, al
A: 1ce224 b8b97aef | ldrsw x15, [x23, x25, lsl #2]
A: 1ce870 d504487f | msr S0_4_C4_C8_3, xzr
A: 1d0178 f841e124 | ldur x4, [x9, #0x1e]
A: 1d03a0 d50343bf | msr S0_3_C4_C3_5, xzr
A: 1d0bbc d503487f | msr S0_3_C4_C8_3, xzr
A: 1d1120 d505455f | msr S0_5_C4_C5_2, xzr
A: 1d1810 7c734be4 | ldr h4, [sp, w19, uxtw]
A: 1d1a04 d5014ebf | msr S0_1_C4_C14_5, xzr
A: 1d1c2c d8fe1f08 | prfm plil1keep, 0x1ce00c
A: 1d220c d51c2009 | msr TTBR0_EL2, x9
A: 1d31c4 da9692e6 | csinv x6, x23, x22, ls
A: 1d33e4 3c5311bf | ldur b31, [x13, #-0xcf]
A: 1d3930 6d608371 | ldp d17, d0, [x27, #-0x1f8]
A: 1d3934 d503333f | msr S0_3_C3_C3_1, xzr
A: 1d3a80 da0003eb | ngc x11, x0
A: 1d3fbc 387b5be8 | ldrb w8, [sp, w27, uxtw #0]
A: 1d4bb8 d508913f | tlbi vae1osnxs, xzr
A: 1d4c04 38b4fbee | ldrsb x14, [sp, x20, sxtx #0]
A: 1d5610 7d1db54d | str h13, [x10, #0xeda]
A: 1d56e0 d5054e5f | msr S0_5_C4_C14_2, xzr
A: 1d5944 9ad82cb0 | ror x16, x5, x24
A: 1d6244 3898bba8 | ldtrsb x8, [x29, #-0x75]
A: 1d6cfc d506431f | msr S0_6_C4_C3_0, xzr
A: 1d7354 78c0bf08 | ldrsh w8, [x24, #0xb]!
A: 1d77c8 3c1fe188 | stur b8, [x12, #-0x2]
A: 1d8768 d5034ebf | msr S0_3_C4_C14_5, xzr
A: 1d8ba0 d5064adf | msr S0_6_C4_C10_6, xzr
A: 1d9328 d5024b1f | msr S0_2_C4_C11_0, xzr
A: 1d9dcc 1cbd2150 | ldr s16, 0x1541f4
A: 1da030 7c16101f | stur h31, [x0, #-0x9f]
A: 1da314 b3516ce8 | bfxil x8, x7, #17, #11
A: 1daa18 d50442bf | msr S0_4_C4_C2_5, xzr
A: 1db37c fc040635 | str d21, [x17], #0x40
A: 1db794 f8f36e60 | ldrab x0, [x19, #-0x650]!
A: 1dbb24 b89d88b7 | ldtrsw x23, [x5, #-0x28]
A: 1dbba0 d5034d7f | msr S0_3_C4_C13_3, xzr
A: 1dbe50 d50445bf | msr S0_4_C4_C5_5, xzr
A: 1dc320 d5044dbf | msr S0_4_C4_C13_5, xzr
A: 1dcc44 9a86b4df | cinc xzr, x6, ge
A: 1dd334 3c533ee7 | ldr b7, [x23, #-0xcd]!
A: 1dd3f0 1a84b482 | cinc w2, w4, ge
A: 1de17c 7f119247 | .inst 0x7f119247
A: 1def00 d505435f | msr S0_5_C4_C3_2, xzr
A: 1df130 d5064b1f | msr S0_6_C4_C11_0, xzr
A: 1df71c 5951f39a | ldapurh w26, [x28, #-0xe1]
A: 1e11f8 6d537389 | ldp d9, d28, [x28, #0x130]
A: 1e1838 d5387401 | mrs x1, PAR_EL1
A: 1e1ab0 bc227b9f | str s31, [x28, x2, lsl #2]
A: 1e1b3c d5074c1f | msr S0_7_C4_C12_0, xzr
A: 1e1d68 d5024fff | msr S0_2_C4_C15_7, xzr
A: 1e20fc d50548df | msr S0_5_C4_C8_6, xzr
A: 1e28fc d507489f | msr S0_7_C4_C8_4, xzr
A: 1e308c 1d4ab630 | .inst 0x1d4ab630
A: 1e30d4 d5074d7f | msr S0_7_C4_C13_3, xzr
A: 1e3130 d503389f | dsb #8
A: 1e396c f82f7890 | str x16, [x4, x15, lsl #3]
A: 1e3e90 d503417f | msr S0_3_C4_C1_3, xzr
A: 1e4c5c d5074a5f | msr S0_7_C4_C10_2, xzr
A: 1e5524 d50746bf | msr S0_7_C4_C6_5, xzr
A: 1e567c 3cff48df | ldr q31, [x6, wzr, uxtw]
A: 1e5ff0 d50339bf | dmb ishld
A: 1e61d0 3d32e7cb | str b11, [x30, #0xcb9]
A: 1e626c d507407f | msr S0_7_C4_C0_3, xzr
A: 1e72b4 b82f201f | steor w15, [x0]
A: 1e7e24 d41ffa63 | smc #0xffd3
A: 1e813c dac11ddb | autdb x27, x14
A: 1e8168 d502499f | msr S0_2_C4_C9_4, xzr
A: 1e8b80 7c46967a | ldr h26, [x19], #0x69
A: 1e92f0 eada19bc | ands x28, x13, x26, ror #6
A: 1e9d8c d5024f5f | msr S0_2_C4_C15_2, xzr
A: 1e9ea4 2b0360a6 | adds w6, w5, w3, lsl #24
A: 1ea010 d5044c9f | msr S0_4_C4_C12_4, xzr
A: 1ea18c 30e04896 | adr x22, 0x1aaa9d
A: 1ea5e0 f85ed811 | ldtr x17, [x0, #-0x13]
A: 1ea6e4 7a4403e9 | ccmp wzr, w4, #0x9, eq
A: 1eb010 d5054f7f | msr S0_5_C4_C15_3, xzr
A: 1eb458 ce9df372 | .inst 0xce9df372
A: 1eb808 b84c9ddd | ldr w29, [x14, #0xc9]!
A: 1ebb24 7842ddad | ldrh w13, [x13, #0x2d]!
A: 1ebd34 93c0fbf2 | extr x18, xzr, x0, #0x3e
A: 1ec4fc d518c001 | msr VBAR_EL1, x1
A: 1ec5cc 886a7815 | ldxp w21, w30, [x0]
A: 1ece58 d5014a3f | msr S0_1_C4_C10_1, xzr
A: 1ecf04 d501465f | msr S0_1_C4_C6_2, xzr
A: 1ed7e8 d5064f5f | msr S0_6_C4_C15_2, xzr
A: 1ee0cc d50447ff | msr S0_4_C4_C7_7, xzr
A: 1ee60c 3a4dcb40 | ccmn w26, #0xd, #0x0, gt
A: 1ee7f4 dac12ff9 | pacdzb x25
A: 1eeb30 f839f955 | str x21, [x10, x25, sxtx #3]
A: 1eec44 d5277fbf | mrs xzr, S0_7_C7_C15_5
A: 1eef04 3c697a75 | ldr b21, [x19, x9, lsl #0]
A: 1eef80 d50c91bf | tlbi vale2osnxs, xzr
A: 1efd18 d5100fb2 | msr DBGBCR15_EL1, x18
A: 1f0cc4 d50896ff | tlbi rvaale1nxs, xzr
A: 1f1034 d5074d9f | msr S0_7_C4_C13_4, xzr
A: 1f12a4 d5064e7f | msr S0_6_C4_C14_3, xzr
A: 1f1450 d50744bf | msr S0_7_C4_C4_5, xzr
A: 1f2810 d50149ff | msr S0_1_C4_C9_7, xzr
A: 1f2b58 d50149bf | msr S0_1_C4_C9_5, xzr
A: 1f2e04 d50248df | msr S0_2_C4_C8_6, xzr
A: 1f36a8 d5004aff | msr S0_0_C4_C10_7, xzr
A: 1f3784 38093e3c | strb w28, [x17, #0x93]!
A: 1f40e4 780c6120 | sturh w0, [x9, #0xc6]
A: 1f4a64 a9e24394 | ldp x20, x16, [x28, #-0x1e0]!
A: 1f5048 d50e923a | tlbi rvae3isnxs, x26
A: 1f50f4 d51007e2 | msr DBGWCR7_EL1, x2
A: 1f57d8 7c1b4f27 | str h7, [x25, #-0x4c]!
A: 1f5b40 d50448df | msr S0_4_C4_C8_6, xzr
A: 1f5fbc d5104a97 | msr S2_0_C4_C10_4, x23
A: 1f6144 78757851 | ldrh w17, [x2, x21, lsl #1]
A: 1f6168 b3421a91 | bfxil x17, x20, #2, #5
A: 1f6270 53017e7e | lsr w30, w19, #1
A: 1f65c8 d5024cbf | msr S0_2_C4_C12_5, xzr
A: 1f69e0 38cb0b89 | ldtrsb w9, [x28, #0xb0]
A: 1f6c88 3c62cb7f | ldr b31, [x27, w2, sxtw]
A: 1f6fc4 d5044f1f | msr S0_4_C4_C15_0, xzr
A: 1f7684 7a1b03fc | ngcs w28, w27
A: 1f8134 d506421f | msr S0_6_C4_C2_0, xzr
A: 1f8b24 d50b1791 | sys #0x3, c1, c7, #0x4, x17
A: 1f8cbc d53b420d | mrs x13, NZCV
A: 1f8d34 d505415f | msr S0_5_C4_C1_2, xzr
A: 1f8e50 d507439f | msr S0_7_C4_C3_4, xzr
A: 1f91b0 7841d2be | ldurh w30, [x21, #0x1d]
A: 1f9410 53147ed2 | lsr w18, w22, #20
A: 1f9f70 d5044a3f | msr S0_4_C4_C10_1, xzr
A: 1fa368 d0cab88f | adrp x15, 0xffffffff9590c000
A: 1fa3c4 2ca95fde | stp s30, s23, [x30], #-0xb8
A: 1fa3f0 bc09bd83 | str s3, [x12, #0x9b]!
A: 1faeec d50446ff | msr S0_4_C4_C6_7, xzr
A: 1fb5f4 fa5bd846 | ccmp x2, #0x1b, #0x6, le
A: 1fb888 d501485f | msr S0_1_C4_C8_2, xzr
A: 1fc29c d50046ff | msr S0_0_C4_C6_7, xzr
A: 1fcb48 584c4540 | ldr x0, 0x2953f0
A: 1fd360 d5024b3f | msr S0_2_C4_C11_1, xzr
A: 1fd75c b83861b2 | ldumax w24, w18, [x13]
A: 1fd840 d53be222 | mrs x2, CNTP_CTL_EL0
A: 1fe48c d5004dff | msr S0_0_C4_C13_7, xzr
A: 1fe520 d97455d8 | stzg x24, [x14], #-0xbb0
A: 1fea98 b9468697 | ldr w23, [x20, #0x684]
A: 1ff42c eb252240 | subs x0, x18, w5, uxth
A: 1ffd68 d507405f | msr S0_7_C4_C0_2, xzr
A: 1fffec f831625f | stumax x17, [x18]
A: 20044c 6dd63f0e | ldp d14, d15, [x24, #0x160]!
A: 200550 d505431f | msr S0_5_C4_C3_0, xzr
A: 200744 f29553ae | movk x14, #0xaa9d
A: 200bb4 aa5c5e0d | orr x13, x16, x28, lsr #23
A: 20116c c8638f20 | ldaxp x0, x3, [x25]
A: 201930 dac115bc | autib x28, x13
A: 201a20 d50242df | msr S0_2_C4_C2_6, xzr
A: 2025fc 19c46780 | .inst 0x19c46780
A: 2028a4 aa1003f9 | mov x25, x16
A: 202bf8 93418a9e | sbfx x30, x20, #1, #34
A: 202f8c d507499f | msr S0_7_C4_C9_4, xzr
A: 203300 7a020278 | sbcs w24, w19, w2
A: 203420 d501463f | msr S0_1_C4_C6_1, xzr
A: 203658 d5074e7f | msr S0_7_C4_C14_3, xzr
A: 2048ac d505401f | msr S0_5_C4_C0_0, xzr
A: 204be0 d5064fff | msr S0_6_C4_C15_7, xzr
A: 204c5c d53000aa | mrs x10, DBGBCR0_EL1
A: 205a58 d5014b7f | msr S0_1_C4_C11_3, xzr
A: 2064a4 edd832ab | .inst 0xedd832ab
A: 206698 381d9e50 | strb w16, [x18, #-0x27]!
A: 206bd4 d37ff8b7 | lsl x23, x5, #1
A: 206c18 d50541df | msr S0_5_C4_C1_6, xzr
A: 206c74 fd15398a | str d10, [x12, #0x2a70]
A: 206cc0 d50042ff | msr S0_0_C4_C2_7, xzr
A: 206ef4 f9485ec8 | ldr x8, [x22, #0x10b8]
A: 207490 d4af2621 | dcps1 #0x7931
A: 208d60 d538c842 | mrs x2, ICC_HPPIR0_EL1
A: 208e98 d501431f | msr PM, #0x1
A: 208fec bc226be4 | str s4, [sp, x2]
A: 209b34 d5054b3f | msr S0_5_C4_C11_1, xzr
A: 20a2f0 d5024e7f | msr S0_2_C4_C14_3, xzr
A: 20a320 9bb1de38 | umsubl x24, w17, w17, x23
A: 20a55c dac1097f | pacda xzr, x11
A: 20acc4 d506405f | msr S0_6_C4_C0_2, xzr
A: 20af40 d503225f | tsb csync
A: 20b32c d5032edf | hint #0x76
A: 20b38c c8073e1b | stxr w7, x27, [x16]
A: 20c744 b8244324 | ldsmax w4, w4, [x25]
A: 20cacc d505475f | msr S0_5_C4_C7_2, xzr
A: 20de04 d5044b9f | msr S0_4_C4_C11_4, xzr
A: 20df80 d5044eff | msr S0_4_C4_C14_7, xzr
A: 20e4e0 d505459f | msr S0_5_C4_C5_4, xzr
A: 20e634 d51cc9be | msr ICC_SRE_EL2, x30
A: 20e6c4 311dfa3d | adds w29, w17, #0x77e
A: 20ea24 d5004cdf | msr S0_0_C4_C12_6, xzr
A: 20f534 d5014fff | msr S0_1_C4_C15_7, xzr
A: 20faa4 d50c8388 | tlbi alle1is
A: 20ffd8 129ffff3 | movn w19, #0xffff
A: 210638 1830ab6e | ldr w14, 0x271ba4
A: 210950 d5052c9d | msr S0_5_C2_C12_4, x29
A: 210a9c 13991b23 | ror w3, w25, #0x6
A: 210b7c d506423f | msr S0_6_C4_C2_1, xzr
A: 210bac d506403f | msr S0_6_C4_C0_1, xzr
A: 21102c d507433f | msr S0_7_C4_C3_1, xzr
A: 211414 d505407f | msr S0_5_C4_C0_3, xzr
A: 211680 d5054ddf | msr S0_5_C4_C13_6, xzr
A: 21185c d5024bff | msr S0_2_C4_C11_7, xzr
A: 2121e8 d507417f | msr S0_7_C4_C1_3, xzr
A: 2124ac d50646ff | msr S0_6_C4_C6_7, xzr
A: 212a90 d5064cbf | msr S0_6_C4_C12_5, xzr
A: 212ce8 d538c11d | mrs x29, ISR_EL1
A: 2139fc d50320ff | xpaclri
A: 2145a4 d5074fff | msr S0_7_C4_C15_7, xzr
A: 214974 f83c63c2 | ldumax x28, x2, [x30]
A: 214a80 c80c39ea | stxr w12, x10, [x15]
A: 2154d4 bd111d23 | str s3, [x9, #0x111c]
A: 215538 ad3ade12 | stp q18, q23, [x16, #-0xb0]
A: 215550 ba060668 | rmif x19, #0xc, #0x8
A: 2155c4 d5024d5f | msr S0_2_C4_C13_2, xzr
A: 21601c d501451f | msr S0_1_C4_C5_0, xzr
A: 2164e0 d51ccc3c | msr ICH_LR1_EL2, x28
A: 216798 d506443f | msr S0_6_C4_C4_1, xzr
A: 216bb8 d5014b5f | msr S0_1_C4_C11_2, xzr
A: 217864 dac103d1 | pacia x17, x30
A: 217dd4 f83f10ca | ldclr xzr, x10, [x6]
A: 218154 19c7e122 | ldapursb w2, [x9, #0x7e]
A: 218968 d5024e5f | msr S0_2_C4_C14_2, xzr
A: 219078 d5074bdf | msr S0_7_C4_C11_6, xzr
A: 2193f8 d50446bf | msr S0_4_C4_C6_5, xzr
A: 219654 d93dc76c | stg x12, [x27], #-0x240
A: 2199f4 d50744df | msr S0_7_C4_C4_6, xzr
A: 21a14c dac10f5d | pacdb x29, x26
A: 21a354 fc037380 | stur d0, [x28, #0x37]
A: 21a5b4 d5044fff | msr S0_4_C4_C15_7, xzr
A: 21a8c0 d5054e7f | msr S0_5_C4_C14_3, xzr
A: 21b294 f83fd0ac | ld64b x12, [x5]
A: 21b6c8 d5024d9f | msr S0_2_C4_C13_4, xzr
A: 21c608 9aca5f30 | crc32cx w16, w25, x10
A: 21c974 f8e24f84 | ldrab x4, [x28, #-0xee0]!
A: 21deac d6bf03e0 | drps
A: 21e4f8 78c040d3 | ldursh w19, [x6, #0x4]
A: 21edd0 d5054d7f | msr S0_5_C4_C13_3, xzr
A: 21f140 d502445f | msr S0_2_C4_C4_2, xzr
A: 21f148 00008835 | udf #0x8835
A: 21f45c d50641bf | msr S0_6_C4_C1_5, xzr
A: 21f67c 6b236fff | cmp wsp, w3, uxtx #3
A: 21f7dc d500415f | axflag
A: 220060 5ac0100f | clz w15, w0
A: 220470 d50142bf | msr S0_1_C4_C2_5, xzr
A: 221324 d73f0e74 | blrab x19, x20
A: 221824 12bfffe9 | movn w9, #0xffff, lsl #16
A: 221a84 d5024c7f | msr S0_2_C4_C12_3, xzr
A: 222b74 d501429f | msr S0_1_C4_C2_4, xzr
A: 222e9c 7c0fd598 | str h24, [x12], #0xfd
A: 223b54 d51814f5 | msr SCTLR2ALIAS_EL1, x21
A: 223d70 d5064fdf | msr S0_6_C4_C15_6, xzr
A: 224540 d53c601f | mrs xzr, FAR_EL2
A: 22476c d3501bef | ubfiz x15, xzr, #48, #7
A: 224860 d502471f | msr S0_2_C4_C7_0, xzr
A: 224cb4 fc0046dc | str d28, [x22], #0x4
A: 22541c d53ce35c | mrs x28, CNTHV_CVAL_EL2
A: 2256c0 d5087f3f | dc civaps, xzr
A: 2259e8 f1356cbf | cmp x5, #0xd5b
A: 22648c d50340bf | msr S0_3_C4_C0_5, xzr
A: 2264f8 d50344bf | msr S0_3_C4_C4_5, xzr
A: 226524 385bde7c | ldrb w28, [x19, #-0x43]!
A: 226570 f8656972 | ldr x18, [x11, x5]
A: 2269e0 d5054b9f | msr S0_5_C4_C11_4, xzr
A: 226dd0 d513e459 | msr S2_3_C14_C4_2, x25
A: 2273d0 d50781d9 | msr S0_7_C8_C1_6, x25
A: 2274c0 d5300881 | mrs x1, DBGBVR8_EL1
A: 227b48 d53ccc77 | mrs x23, ICH_LR3_EL2
A: 227b54 d5064bdf | msr S0_6_C4_C11_6, xzr
A: 228604 d51c276d | msr TCR2MASK_EL2, x13
A: 2286b0 b8381269 | ldclr w24, w9, [x19]
A: 228970 d50c925f | tlbi vmallws2e1isnxs
A: 228c4c d5301006 | mrs x6, MDRAR_EL1
A: 229be8 d504473f | msr S0_4_C4_C7_1, xzr
A: 22a378 d507497f | msr S0_7_C4_C9_3, xzr
A: 22aaec d500435f | axflag
A: 22c268 d50c96af | tlbi rvale2nxs, x15
A: 22c72c c8c3d6c8 | ldar x8, [x22]
A: 22c840 6921298e | stgp x14, x10, [x12, #-0x3e0]
A: 22cf90 9260ad7d | and x29, x11, #0xffffffff00000fff
A: 22d9b0 6b0330bf | cmp w5, w3, lsl #12
A: 22df1c 2f544968 | .inst 0x2f544968
A: 22e67c d507413f | msr S0_7_C4_C1_1, xzr
A: 22ed84 d51bdc9e | msr S3_3_C13_C12_4, x30
A: 22ee34 d502481f | msr S0_2_C4_C8_0, xzr
A: 22f51c 53177fd5 | lsr w21, w30, #23
A: 22fd40 d53be82a | mrs x10, PMEVCNTR1_EL0
A: 2300ac d93968a5 | stg x5, [x5, #-0x6a0]
A: 2304d4 9a9f97f6 | cset x22, hi
A: 230680 d503209f | sev
A: 2307a8 d61f0a1f | braaz x16
A: 2308bc 1ac65720 | crc32ch w0, w25, w6
A: 2308e8 d53003e0 | mrs x0, DBGWCR3_EL1
A: 230994 d5044a7f | msr S0_4_C4_C10_3, xzr
A: 231478 d9702f0b | stzg x11, [x24, #-0xfe0]!
A: 231664 78db8d80 | ldrsh w0, [x12, #-0x48]!
A: 23187c 53003df0 | uxth w16, w15
A: 231f08 d27f1428 | eor x8, x1, #0x7e
A: 232b60 d50c96bf | tlbi rvale2nxs, xzr
A: 23312c d501491f | msr S0_1_C4_C9_0, xzr
A: 233300 d5064a9f | msr S0_6_C4_C10_4, xzr
A: 2338cc d505449f | msr S0_5_C4_C4_4, xzr
A: 233a08 d5024b7f | msr S0_2_C4_C11_3, xzr
A: 234620 2c826caf | stp s15, s27, [x5], #0x10
A: 23465c 9361f3c7 | sbfx x7, x30, #33, #28
A: 235250 3ca2da97 | str q23, [x20, w2, sxtw #4]
A: 235f28 395fc57c | ldrb w28, [x11, #0x7f1]
A: 236840 d505413f | msr S0_5_C4_C1_1, xzr
A: 236aa8 3c03c6dd | str b29, [x22], #0x3c
A: 236adc ba13070f | rmif x24, #0x26, #0xf
A: 237790 ea2378bf | bics xzr, x5, x3, lsl #30
A: 237e9c d50647df | msr S0_6_C4_C7_6, xzr
A: 238ca8 d503219f | autia1716
A: 238d70 d503205f | wfe
A: 239dfc 33040e0a | bfi w10, w16, #28, #4
A: 23aeb8 513198da | sub w26, w6, #0xc66
A: 23aed0 eb0373ff | cmp xzr, x3, lsl #28
A: 23b3e4 c81dada9 | stlxr w29, x9, [x13]
A: 23b980 d5014fbf | msr S0_1_C4_C15_5, xzr
A: 23bc74 1207bbca | and w10, w30, #0xfefffeff
A: 23bd38 d5024fdf | msr S0_2_C4_C15_6, xzr
A: 23beb0 d504447f | msr S0_4_C4_C4_3, xzr
A: 23bec8 d50743bf | msr S0_7_C4_C3_5, xzr
A: 23c05c d50749df | msr S0_7_C4_C9_6, xzr
A: 23c0a4 bc2cdae3 | str s3, [x23, w12, sxtw #2]
A: 23c4a0 595990ce | ldapurh w14, [x6, #-0x67]
A: 23c7d4 f8093bc0 | sttr x0, [x30, #0x93]
A: 23c86c 53034fee | ubfx w14, wzr, #3, #17
A: 23c8d0 da9fc3e9 | csetm x9, le
A: 23ca20 d5185600 | msr TFSR_EL1, x0
A: 23cd1c d2480bcd | eor x13, x30, #0x700000000000000
A: 23ce58 d50048ff | msr S0_0_C4_C8_7, xzr
A: 23d054 d53b993a | mrs x26, S3_3_C9_C9_1
A: 23d0f8 d506445f | msr S0_6_C4_C4_2, xzr
A: 23e184 d50145ff | msr S0_1_C4_C5_7, xzr
A: 23e3ec d505495f | msr S0_5_C4_C9_2, xzr
A: 23eb48 d501461f | msr S0_1_C4_C6_0, xzr
A: 23ed80 d5054b7f | msr S0_5_C4_C11_3, xzr
A: 23eda8 d523307d | tstart x29
A: 23f350 d50448bf | msr S0_4_C4_C8_5, xzr
A: 23f510 d5044d1f | msr S0_4_C4_C13_0, xzr
A: 23fb20 78833404 | ldrsh x4, [x0], #0x33
A: 2401e0 990dd1a8 | stlur w8, [x13, #0xdd]
A: 240b54 6add4c4a | ands w10, w2, w29, ror #19
A: 240ebc d5054c1f | msr S0_5_C4_C12_0, xzr
A: 2412a4 b83a4197 | ldsmax w26, w23, [x12]
A: 2418b4 79e4fbed | ldrsh w13, [sp, #0x127c]
A: 242c20 d5014c1f | msr S0_1_C4_C12_0, xzr
A: 2432b0 138756ff | extr wzr, w23, w7, #0x15
A: 243348 4b037d3b | sub w27, w9, w3, lsl #31
A: 243724 d50543df | msr S0_5_C4_C3_6, xzr
A: 2439fc d52d601d | sysl x29, #0x5, c6, c0, #0x0
A: 244178 7c62dbbf | ldr h31, [x29, w2, sxtw #1]
A: 2444c0 78031bf7 | sttrh w23, [sp, #0x31]
A: 244974 166da4fa | b 0xfffffffff9dadd5c
A: 2456f8 1ac145fe | crc32h w30, w15, w1
A: 245c38 d5054f3f | msr S0_5_C4_C15_1, xzr
A: 24601c d507453f | msr S0_7_C4_C5_1, xzr
A: 246314 ab19e0ee | adds x14, x7, x25, lsl #56
A: 246478 d505403f | msr S0_5_C4_C0_1, xzr
A: 246eb0 d5024c1f | msr S0_2_C4_C12_0, xzr
A: 2472f0 d506433f | msr S0_6_C4_C3_1, xzr
A: 24800c d504421f | msr S0_4_C4_C2_0, xzr
A: 2482a8 6c58747e | ldnp d30, d29, [x3, #0x180]
A: 2488a8 acae2c16 | stp q22, q11, [x0], #-0x240
A: 248b34 d50243bf | msr S0_2_C4_C3_5, xzr
A: 248f04 2da7e725 | stp s5, s25, [x25, #-0xc4]!
A: 249400 780fe2b0 | sturh w16, [x21, #0xfe]
A: 249840 7c25fa2b | str h11, [x17, x5, sxtx #1]
A: 24a790 d903b307 | stlur x7, [x24, #0x3b]
A: 24a7a0 d506499f | msr S0_6_C4_C9_4, xzr
A: 24b908 d61f0c9f | brabz x4
A: 24bb48 d50337ff | sb
A: 24c068 93d3d27f | ror xzr, x19, #0x34
A: 24c8a4 78836049 | ldursh x9, [x2, #0x36]
A: 24c9b8 a841d189 | ldnp x9, x20, [x12, #0x18]
A: 24dbe4 d50338df | isb #8
A: 24e034 78b8d862 | ldrsh x2, [x3, w24, sxtw #1]
A: 24eed8 d9b6bc43 | st2g x3, [x2, #-0x950]!
A: 24ef1c d503369f | dsb nshst
A: 24f520 d34bfd47 | lsr x7, x10, #11
A: 24ff58 531871e0 | ubfx w0, w15, #24, #5
A: 2501e4 d508751f | ic iallu
A: 250408 d50321ff | hint #0xf
A: 250908 d5024edf | msr S0_2_C4_C14_6, xzr
A: 250d28 d50549bf | msr S0_5_C4_C9_5, xzr
A: 250df4 3d2bfad0 | str b16, [x22, #0xafe]
A: 251c8c d504481f | msr S0_4_C4_C8_0, xzr
A: 2521f4 d50c8644 | tlbi vmallws2e1
A: 252318 d5044bff | msr S0_4_C4_C11_7, xzr
A: 252f70 d5064d5f | msr S0_6_C4_C13_2, xzr
A: 25301c d503421f | msr S0_3_C4_C2_0, xzr
A: 2536b0 2d7bf1a5 | ldp s5, s28, [x13, #-0x24]
A: 253c1c 6e86f6cf | .inst 0x6e86f6cf
A: 253e68 eadf3be6 | ands x6, xzr, xzr, ror #14
A: 254000 f8b15eab | ldrab x11, [x21, #0x8a8]!
A: 25487c 0a5a1241 | and w1, w18, w26, lsr #4
A: 255a5c d50640bf | msr S0_6_C4_C0_5, xzr
A: 256790 3cc7916b | ldur q11, [x11, #0x79]
A: 256b08 88cc7fe7 | ldlar w7, [sp]
A: 256d68 d5044f5f | msr S0_4_C4_C15_2, xzr
A: 25708c d5074a9f | msr S0_7_C4_C10_4, xzr
A: 257974 d503437f | smstart sm
A: 2583b4 1ac92def | ror w15, w15, w9
A: 25889c d506449f | msr S0_6_C4_C4_4, xzr
A: 259794 d50745ff | msr S0_7_C4_C5_7, xzr
A: 25991c 93d76afb | ror x27, x23, #0x1a
A: 259ca4 84564f54 | .inst 0x84564f54
A: 25c55c d50320df | dgh
A: 25c5b8 0cd07d58 | .inst 0x0cd07d58
A: 25d9d4 d502425f | msr S0_2_C4_C2_2, xzr
A: 25da24 d50445ff | msr S0_4_C4_C5_7, xzr
A: 25e29c d502431f | msr S0_2_C4_C3_0, xzr
A: 25f580 d50148ff | msr S0_1_C4_C8_7, xzr
A: 260004 d505497f | msr S0_5_C4_C9_3, xzr
A: 260664 d503411f | msr S0_3_C4_C1_0, xzr
A: 260bc4 d5014eff | msr S0_1_C4_C14_7, xzr
A: 260cdc f8545ba4 | ldtr x4, [x29, #-0xbb]
A: 261638 d506479f | msr S0_6_C4_C7_4, xzr
A: 261740 d5233160 | ttest x0
A: 261c6c d69f03e0 | eret
A: 26242c f822029f | stadd x2, [x20]
A: 26256c d503431f | msr S0_3_C4_C3_0, xzr
A: 262938 d503101d | wfet x29
A: 263544 93cd048e | extr x14, x4, x13, #0x1
A: 2636a0 29e5157c | ldp w28, w5, [x11, #-0xd8]!
A: 264f90 9adf30b6 | pacga x22, x5, sp
A: 2654a8 530c2f77 | lsl w23, w27, #20
A: 265b94 138a37f2 | extr w18, wzr, w10, #0xd
A: 2660bc d93eefd3 | stg x19, [x30, #-0x120]!
A: 266778 d5004fff | msr S0_0_C4_C15_7, xzr
A: 26684c 330961ff | bfxil wzr, w15, #9, #16
A: 266868 d501433f | msr S0_1_C4_C3_1, xzr
A: 266908 d5044e1f | msr S0_4_C4_C14_0, xzr
A: 26696c d50641ff | msr S0_6_C4_C1_7, xzr
A: 2671b0 d504431f | msr S0_4_C4_C3_0, xzr
A: 267368 6c9f6569 | stp d9, d25, [x11], #0x1f0
A: 2679f8 f83f5224 | ldsmin xzr, x4, [x17]
A: 267c84 798c9188 | ldrsh x8, [x12, #0x648]
A: 267cf0 6d2285a4 | stp d4, d1, [x13, #-0x1d8]
A: 267ee4 d3613451 | ubfiz x17, x2, #31, #14
A: 267fbc d51004db | msr DBGWVR4_EL1, x27
A: 268bb8 69595a3f | ldpsw xzr, x22, [x17, #0xc8]
A: 268f70 1efd450c | .inst 0x1efd450c
A: 2699bc 6ac35fff | tst wzr, w3, ror #23
A: 269e88 d50041ff | msr S0_0_C4_C1_7, xzr
A: 26a1d4 d504489f | msr S0_4_C4_C8_4, xzr
A: 26a528 d5044c7f | msr S0_4_C4_C12_3, xzr
A: 26b514 381104a6 | strb w6, [x5], #-0xf0
A: 26b768 d507411f | msr S0_7_C4_C1_0, xzr
A: 26ca40 6c438c6a | ldnp d10, d3, [x3, #0x38]
A: 26cb34 d5054fbf | msr S0_5_C4_C15_5, xzr
A: 26d7e0 f2757ffa | ands x26, xzr, #0x7fffffff800
A: 26e1e4 53107f02 | lsr w2, w24, #16
A: 26e7d0 d503211f | pacia1716
A: 26f124 d50b7491 | dc gzva, x17
A: 26f5f8 d502463f | msr S0_2_C4_C6_1, xzr
A: 26fa90 dac10c48 | pacdb x8, x2
A: 26fc50 d50748ff | msr S0_7_C4_C8_7, xzr
A: 270780 f826033f | stadd x6, [x25]
A: 270924 d5064bbf | msr S0_6_C4_C11_5, xzr
A: 270d70 d502409f | msr S0_2_C4_C0_4, xzr
A: 270ef0 f846b985 | ldtr x5, [x12, #0x6b]
A: 271568 d5100ad5 | msr DBGWVR10_EL1, x21
A: 271aac d502413f | msr S0_2_C4_C1_1, xzr
A: 272274 b37b3328 | bfi x8, x25, #5, #13
A: 2723e8 d63f0200 | blr x16
A: 272c7c d50547bf | msr S0_5_C4_C7_5, xzr
A: 27330c 888df3dc | stlr w28, [x30]
A: 273734 d50541ff | msr S0_5_C4_C1_7, xzr
A: 273788 884a3319 | ldxr w25, [x24]
A: 273f48 d507485f | msr S0_7_C4_C8_2, xzr
A: 27435c c81f1464 | stxr wzr, x4, [x3]
A: 27465c b83b73ed | ldumin w27, w13, [sp]
A: 2753cc d501459f | msr S0_1_C4_C5_4, xzr
A: 275494 6cd84133 | ldp d19, d16, [x9], #0x180
A: 27591c d5064fbf | msr S0_6_C4_C15_5, xzr
A: 276944 d50143df | msr S0_1_C4_C3_6, xzr
A: 276e34 5ac0081b | rev w27, w0
A: 277088 d504461f | msr S0_4_C4_C6_0, xzr
A: 2773d4 dac13ff8 | autdzb x24
A: 277e70 b81a98ea | sttr w10, [x7, #-0x57]
A: 278440 130c50c0 | sbfx w0, w6, #12, #9
A: 2788a0 8b3fe0a6 | add x6, x5, xzr, sxtx
A: 278b64 d50048df | msr S0_0_C4_C8_6, xzr
A: 278b68 eb437cbf | cmp x5, x3, lsr #31
A: 279048 d419adc3 | smc #0xcd6e
A: 2790e4 d503457f | smstart za
A: 27952c d5014f9f | msr S0_1_C4_C15_4, xzr
A: 279b50 78d2a9ce | ldtrsh w14, [x14, #-0xd6]
A: 279bd0 d51e4105 | msr SP_EL2, x5
A: 279cb8 bc58a74f | ldr s15, [x26], #-0x76
A: 279cf4 d5024c5f | msr S0_2_C4_C12_2, xzr
A: 27a400 8a6378bf | bic xzr, x5, x3, lsr #30
A: 27a478 d50145bf | msr S0_1_C4_C5_5, xzr
A: 27abac bc5eff99 | ldr s25, [x28, #-0x11]!
A: 27ac10 d504439f | msr S0_4_C4_C3_4, xzr
A: 27b8a8 d5074ebf | msr S0_7_C4_C14_5, xzr
A: 27c2b8 784d8f6d | ldrh w13, [x27, #0xd8]!
A: 27c5a4 d50440bf | msr S0_4_C4_C0_5, xzr
A: 27d9e4 3a000aad | setf8 w21
A: 27e578 598b13d7 | ldapursh x23, [x30, #0xb1]
A: 27e5fc 93403c57 | sxth x23, w2
A: 27f180 d503477f | smstart
A: 27f37c 9acd0018 | subp x24, x0, x13
A: 27f42c f253dbf3 | ands x19, xzr, #0xffffe00fffffffff
A: 27f580 d4000001 | svc #0
A: 27f654 d502435f | msr S0_2_C4_C3_2, xzr
A: 27ff50 4faa980f | .inst 0x4faa980f
A: 2804ac ad6322c3 | ldp q3, q8, [x22, #-0x3a0]
A: 2808cc d504483f | msr S0_4_C4_C8_1, xzr
A: 2809a4 841b352f | .inst 0x841b352f
A: 2809c0 d53009e4 | mrs x4, DBGWCR9_EL1
A: 280cd4 983608b1 | ldrsw x17, 0x2ecde8
A: 28282c 93ce09ce | ror x14, x14, #0x2
A: 282b38 addb8634 | ldp q20, q1, [x17, #0x370]!
A: 2830c4 b88e9f9c | ldrsw x28, [x28, #0xe9]!
A: 2833b8 885ad08a | ldaxr w10, [x4]
A: 284848 d505429f | msr S0_5_C4_C2_4, xzr
A: 284d14 d501411f | msr ALLINT, #0x1
A: 284e08 92469491 | and x17, x4, #0xfc000000ffffffff
A: 285644 5a8bb7f4 | csneg w20, wzr, w11, lt
A: 285810 d501481f | msr S0_1_C4_C8_0, xzr
A: 2867a0 381e8679 | strb w25, [x19], #-0x18
A: 287070 05290def | .inst 0x05290def
A: 287158 d52fbd2d | sysl x13, #0x7, c11, c13, #0x1
A: 2876b8 d506453f | msr S0_6_C4_C5_1, xzr
A: 287ab0 d5014f3f | msr S0_1_C4_C15_1, xzr
A: 28843c d5044b3f | msr S0_4_C4_C11_1, xzr
A: 2885e0 d4b5a382 | dcps2 #0xad1c
A: 288738 7c1a5f1f | str h31, [x24, #-0x5b]!
A: 288f90 d5033bff | sb
A: 288fdc d502427f | msr S0_2_C4_C2_3, xzr
A: 2892a0 d505483f | msr S0_5_C4_C8_1, xzr
A: 289760 0db8082f | .inst 0x0db8082f
A: 289908 6c0654e1 | stnp d1, d21, [x7, #0x60]
A: 289a1c b828527f | stsmin w8, [x19]
A: 28aa84 33140b37 | bfi w23, w25, #12, #3
A: 28aa88 d504401f | msr S0_4_C4_C0_0, xzr
A: 28ad0c 30b49ce2 | adr x2, 0x1f40a9
A: 28ad10 d51bef56 | msr PMEVTYPER26_EL0, x22
A: 28b758 d5182142 | msr APIBKeyLo_EL1, x2
A: 28b9f0 93dbb9b4 | extr x20, x13, x27, #0x2e
A: 28be58 d503363f | dsb nshnxs
A: 28c1c4 bc7f7aff | ldr s31, [x23, xzr, lsl #2]
A: 28c2fc 78843832 | ldtrsh x18, [x1, #0x43]
A: 28c8c4 aabfc7ff | mvn xzr, xzr, asr #49
A: 28d67c d504497f | msr S0_4_C4_C9_3, xzr
A: 28dd8c 6c088be1 | stnp d1, d2, [sp, #0x88]
A: 28e4cc d50241df | msr S0_2_C4_C1_6, xzr
A: 28ead8 9bad4367 | umaddl x7, w27, w13, x16
A: 28ed68 d50142ff | msr S0_1_C4_C2_7, xzr
A: 28ed7c d50349bf | msr S0_3_C4_C9_5, xzr
A: 28f294 d50446df | msr S0_4_C4_C6_6, xzr
A: 28f5c4 7825db33 | strh w19, [x25, w5, sxtw #1]
A: 28f688 88c8bda3 | ldar w3, [x13]
A: 28fae4 53001c52 | uxtb w18, w2
A: 290a00 d50440df | msr S0_4_C4_C0_6, xzr
A: 290ba8 d5064b3f | msr S0_6_C4_C11_1, xzr
A: 290d98 d50245df | msr S0_2_C4_C5_6, xzr
A: 2911b8 131c7c91 | asr w17, w4, #28
A: 2913e0 4a184029 | eor w9, w1, w24, lsl #16
A: 29165c d507461f | msr S0_7_C4_C6_0, xzr
A: 2918c4 93558d25 | sbfx x5, x9, #21, #15
A: 2921dc d5064b7f | msr S0_6_C4_C11_3, xzr
A: 2929d8 d5033dbf | dmb ld
A: 292a18 d95263e4 | ldapur x4, [sp, #-0xda]
A: 293418 d50548bf | msr S0_5_C4_C8_5, xzr
A: 2934a4 d501447f | msr S0_1_C4_C4_3, xzr
A: 2937d0 7d176b2b | str h11, [x25, #0xbb4]
A: 293abc d504407f | msr S0_4_C4_C0_3, xzr
A: 293bec 9a9fb7ed | cset x13, ge
A: 293ef0 d5014c9f | msr S0_1_C4_C12_4, xzr
A: 2943fc d5004fdf | msr S0_0_C4_C15_6, xzr
A: 2948c0 d5074aff | msr S0_7_C4_C10_7, xzr
A: 294a74 d5044c5f | msr S0_4_C4_C12_2, xzr
A: 295a64 d73f0e52 | blrab x18, x18
A: 296184 d504479f | msr S0_4_C4_C7_4, xzr
A: 2965b4 f83fd0f2 | ld64b x18, [x7]
A: 296824 d5054dbf | msr S0_5_C4_C13_5, xzr
A: 296a2c 38406911 | ldtrb w17, [x8, #0x6]
A: 297054 295b2891 | ldp w17, w10, [x4, #0xd8]
A: 29749c ab32879c | adds x28, x28, w18, sxtb #1
A: 297984 9b30fc02 | smnegl x2, w0, w16
A: 2984e0 380be48a | strb w10, [x4], #0xbe
A: 298708 d5233172 | ttest x18
A: 29926c 139f53f8 | ror w24, wzr, #0x14
A: 299b40 882d775f | stxp w13, wzr, w29, [x26]
A: 29a0c0 c845abb8 | ldaxr x24, [x29]
A: 29ab38 d50e96be | tlbi rvale3nxs, x30
A: 29b590 5ac00461 | rev16 w1, w3
A: 29ba54 d50c9495 | tlbi ipas2le1osnxs, x21
A: 29c270 d5054e1f | msr S0_5_C4_C14_0, xzr
A: 29cb04 d504217f | msr S0_4_C2_C1_3, xzr
A: 29e05c d5024bdf | msr S0_2_C4_C11_6, xzr
A: 29e3b0 f83f33df | stset xzr, [x30]
A: 29e6e4 d506469f | msr S0_6_C4_C6_4, xzr
A: 29e904 d507471f | msr S0_7_C4_C7_0, xzr
A: 29edec d5024cff | msr S0_2_C4_C12_7, xzr
A: 29f254 b801f0c9 | stur w9, [x6, #0x1f]
A: 29f83c d5064f1f | msr S0_6_C4_C15_0, xzr
A: 29f938 d501403f | msr S0_1_C4_C0_1, xzr
A: 29fb7c 1add278b | lsr w11, w28, w29
A: 29fc94 d50541bf | msr S0_5_C4_C1_5, xzr
A: 2a0170 d477a020 | tcancel #0xbd01
A: 2a07f0 3c627adf | ldr b31, [x22, x2, lsl #0]
A: 2a0c68 d504493f | msr S0_4_C4_C9_1, xzr
A: 2a0f94 d502451f | msr S0_2_C4_C5_0, xzr
A: 2a19f8 d5044fdf | msr S0_4_C4_C15_6, xzr
A: 2a1fe4 1adf478e | crc32h w14, w28, wzr
A: 2a22dc d97c28d9 | stzg x25, [x6, #-0x3e0]
A: 2a3004 d506447f | msr S0_6_C4_C4_3, xzr
A: 2a30ac 38cc9df4 | ldrsb w20, [x15, #0xc9]!
A: 2a330c d50c84ff | tlbi ripas2le1os, xzr
A: 2a3768 38409d51 | ldrb w17, [x10, #0x9]!
A: 2a3b50 dac104c1 | pacib x1, x6
A: 2a4904 98b1769a | ldrsw x26, 0x2077d4
A: 2a5210 d502467f | msr S0_2_C4_C6_3, xzr
A: 2a57a4 d50141df | msr S0_1_C4_C1_6, xzr
A: 2a5fc8 d504467f | msr S0_4_C4_C6_3, xzr
A: 2a6150 d50044df | msr S0_0_C4_C4_6, xzr
A: 2a62d4 dac10a8b | pacda x11, x20
A: 2a6a34 1add09aa | udiv w10, w13, w29
A: 2a7010 2d4bf388 | ldp s8, s28, [x28, #0x5c]
A: 2a70fc 131c1f7e | sbfiz w30, w27, #4, #8
A: 2a8198 d5044cff | msr S0_4_C4_C12_7, xzr
A: 2a8558 d9e00054 | ldgm x20, [x2]
A: 2a8fe8 d50448ff | msr S0_4_C4_C8_7, xzr
A: 2a9038 d5087e99 | dc cigsw, x25
A: 2a9480 6d0099f9 | stp d25, d6, [x15, #0x8]
A: 2a963c d5014cbf | msr S0_1_C4_C12_5, xzr
A: 2a97dc d50249bf | msr S0_2_C4_C9_5, xzr
A: 2aa8d8 d50144ff | msr S0_1_C4_C4_7, xzr
A: 2aafe4 d35140a4 | lsl x4, x5, #47
A: 2abff0 d50444df | msr S0_4_C4_C4_6, xzr
A: 2ac0ec f822127f | stclr x2, [x19]
A: 2ac65c d5014cff | msr S0_1_C4_C12_7, xzr
A: 2acea0 88ac7dbb | cas w12, w27, [x13]
A: 2ad7f0 d9233894 | stg x20, [x4, #0x330]
A: 2ad95c a9d8ec1e | ldp x30, x27, [x0, #0x188]!
A: 2ae3a8 ab3a883f | cmn x1, w26, sxtb #2
A: 2ae478 d501497f | msr S0_1_C4_C9_3, xzr
A: 2ae6b4 a02b4f80 | .inst 0xa02b4f80
A: 2af070 d5044ebf | msr S0_4_C4_C14_5, xzr
A: 2af3d4 d5024a9f | msr S0_2_C4_C10_4, xzr
A: 2afd98 d61f091f | braaz x8
A: 2affac d50142df | msr S0_1_C4_C2_6, xzr
A: 2b0498 d5064cff | msr S0_6_C4_C12_7, xzr
A: 2b094c 29ac5892 | stp w18, w22, [x4, #-0xa0]!
A: 2b0efc d5014fdf | msr S0_1_C4_C15_6, xzr
A: 2b1258 d506427f | msr S0_6_C4_C2_3, xzr
A: 2b1c80 d51b9cbb | msr PMSELR_EL0, x27
A: 2b2b74 dac00fe6 | rev x6, xzr
A: 2b333c d502493f | msr S0_2_C4_C9_1, xzr
A: 2b36e8 d50545bf | msr S0_5_C4_C5_5, xzr
A: 2b3bb4 d503497f | msr S0_3_C4_C9_3, xzr
A: 2b3d70 d53be9c9 | mrs x9, PMEVCNTR14_EL0
A: 2b4264 f87f16e4 | ldraa x4, [x23, #-0x78]
A: 2b4438 d5014f1f | msr S0_1_C4_C15_0, xzr
A: 2b47fc 9a84249f | cinc xzr, x4, lo
A: 2b55fc d50443df | msr S0_4_C4_C3_6, xzr
A: 2b5894 f9583510 | ldr x16, [x8, #0x3068]
A: 2b6674 6ea0ced9 | .inst 0x6ea0ced9
A: 2b6f24 f25c084d | ands x13, x2, #0x7000000000
A: 2b75c8 3112e29f | cmn w20, #0x4b8
A: 2b75d8 d1798fcb | sub x11, x30, #0xe63, lsl #12
A: 2b77ec 1adb4bed | crc32w w13, wzr, w27
A: 2b82a8 f8276928 | str x8, [x9, x7]
A: 2b8414 d5024cdf | msr S0_2_C4_C12_6, xzr
A: 2b8494 d503491f | msr S0_3_C4_C9_0, xzr
A: 2b883c d970269b | stzg x27, [x20], #-0xfe0
A: 2b8ff8 93401f15 | sxtb x21, w24
A: 2b95fc d507435f | msr S0_7_C4_C3_2, xzr
A: 2ba0d4 3823f9a9 | strb w9, [x13, x3, sxtx #0]
A: 2ba8dc b82230df | stset w2, [x6]
A: 2bab2c d53854c0 | mrs x0, S3_0_C5_C4_6
A: 2bb2bc d508787f | at s1e0w, xzr
A: 2bb3b4 ab252253 | adds x19, x18, w5, uxth
A: 2bb850 92c0bc68 | mov x8, #-0x5e300000001
A: 2bbcf0 d5044b5f | msr S0_4_C4_C11_2, xzr
A: 2bbec0 b2405461 | orr x1, x3, #0x3fffff
A: 2bc4c8 d506409f | msr S0_6_C4_C0_4, xzr
A: 2bc9d0 d502421f | msr S0_2_C4_C2_0, xzr
A: 2bcb0c d50549df | msr S0_5_C4_C9_6, xzr
A: 2bd080 d509791f | sys #0x1, c7, c9, #0x0
A: 2bd100 d9350a51 | stg x17, [x18, #-0xb00]
A: 2be00c f84f6217 | ldur x23, [x16, #0xf6]
A: 2be0cc 9b38456d | smaddl x13, w11, w24, x17
A: 2be128 f8417342 | ldur x2, [x26, #0x17]
A: 2bed50 eabf07ff | bics xzr, xzr, xzr, asr #1
A: 2bf558 d5132efd | msr S2_3_C2_C14_7, x29
A: 2bfb5c d9a3ad73 | st2g x19, [x11, #0x3a0]!
A: 2bfd7c d507493f | msr S0_7_C4_C9_1, xzr
A: 2c0460 d504415f | msr S0_4_C4_C1_2, xzr
A: 2c0fd0 d4b08ee3 | dcps3 #0x8477
A: 2c1080 d505447f | msr S0_5_C4_C4_3, xzr
A: 2c11c4 d506437f | msr S0_6_C4_C3_3, xzr
A: 2c1290 d50642bf | msr S0_6_C4_C2_5, xzr
A: 2c1f20 cb176bf1 | neg x17, x23, lsl #26
A: 2c1f30 3d96598f | str q15, [x12, #0x5960]
A: 2c2590 d500475f | axflag
A: 2c2930 78405863 | ldtrh w3, [x3, #0x5]
A: 2c2b8c d5014edf | msr S0_1_C4_C14_6, xzr
A: 2c2ebc d4a00002 | dcps2
A: 2c2f94 d5033b1f | msr S0_3_C3_C11_0, xzr
A: 2c3020 d5044ddf | msr S0_4_C4_C13_6, xzr
A: 2c3e10 d506417f | msr S0_6_C4_C1_3, xzr
A: 2c4390 d508956e | tlbi rvaae1osnxs, x14
A: 2c4734 d5390009 | mrs x9, CCSIDR_EL1
A: 2c4c94 1ace483e | crc32w w30, w1, w14
A: 2c51a4 d52e051f | sysl xzr, #0x6, c0, c5, #0x0
A: 2c5840 3846cd41 | ldrb w1, [x10, #0x6c]!
A: 2c5fe4 d50321df | autib1716
A: 2c61dc d5014a5f | msr S0_1_C4_C10_2, xzr
A: 2c655c 2cddc777 | ldp s23, s17, [x27], #0xec
A: 2c6a5c 1993f05d | ldapursb x29, [x2, #-0xc1]
A: 2c6ee8 d5054c9f | msr S0_5_C4_C12_4, xzr
A: 2c7a3c d5044c1f | msr S0_4_C4_C12_0, xzr
A: 2c8280 d503323f | dsb oshnxs
A: 2c8f30 d5054cff | msr S0_5_C4_C12_7, xzr
A: 2c92c4 96c1a686 | bl 0xfffffffffb332cdc
A: 2c94ec d5024d1f | msr S0_2_C4_C13_0, xzr
A: 2c9da8 f2696734 | ands x20, x25, #0x1ffffff800000
A: 2c9db4 1ac42ff2 | ror w18, wzr, w4
A: 2ca660 d50c9256 | tlbi vmallws2e1isnxs
A: 2ca81c d502439f | msr S0_2_C4_C3_4, xzr
A: 2cafa4 d40a0f81 | svc #0x507c
A: 2cafec d50544df | msr S0_5_C4_C4_6, xzr
A: 2cb530 d50045df | msr S0_0_C4_C5_6, xzr
A: 2cb804 d50244bf | msr S0_2_C4_C4_5, xzr
A: 2cb908 3c85be8c | str q12, [x20, #0x5b]!
A: 2cb944 d50c913f | tlbi vae2osnxs, xzr
A: 2cbae4 d53ce0d8 | mrs x24, S3_4_C14_C0_6
A: 2cbb78 9c2b8deb | ldr q11, 0x322d34
A: 2cbb9c d5054d9f | msr S0_5_C4_C13_4, xzr
A: 2cbc9c f963cbe8 | ldr x8, [sp, #0x4790]
A: 2cbda4 d5064eff | msr S0_6_C4_C14_7, xzr
A: 2cbfa0 78abf8f9 | ldrsh x25, [x7, x11, sxtx #1]
A: 2cc650 dac001dc | rbit x28, x14
A: 2cc6a8 da9f03e1 | csetm x1, ne
A: 2cdad0 9a9fd7fb | cset x27, gt
A: 2cdba4 d506497f | msr S0_6_C4_C9_3, xzr
A: 2cdf0c d51be219 | msr CNTP_TVAL_EL0, x25
A: 2cdfd4 1293f94d | mov w13, #-0x9fcb
A: 2ce5e0 d507469f | msr S0_7_C4_C6_4, xzr
A: 2ce82c d507479f | msr S0_7_C4_C7_4, xzr
A: 2cfbc4 7282cb92 | movk w18, #0x165c
A: 2cfcb4 d965dc27 | stzg x7, [x1, #0x5d0]!
A: 2d0568 dac143fa | xpaci x26
A: 2d17fc b89355b0 | ldrsw x16, [x13], #-0xcb
A: 2d1800 d5024a3f | msr S0_2_C4_C10_1, xzr
A: 2d1818 d501417f | msr S0_1_C4_C1_3, xzr
A: 2d25d0 d5064dbf | msr S0_6_C4_C13_5, xzr
A: 2d3070 d50043ff | msr S0_0_C4_C3_7, xzr
A: 2d32a0 d5074f5f | msr S0_7_C4_C15_2, xzr
A: 2d349c d5064ebf | msr S0_6_C4_C14_5, xzr
A: 2d3500 d2c00013 | movz x19, #0x0, lsl #32
A: 2d3508 d5033e1f | msr S0_3_C3_C14_0, xzr
A: 2d3aa0 1b14ff8d | mneg w13, w28, w20
A: 2d3b28 d508923f | tlbi rvae1isnxs, xzr
A: 2d3e5c d50647bf | msr S0_6_C4_C7_5, xzr
A: 2d4124 d502485f | msr S0_2_C4_C8_2, xzr
A: 2d4364 d50876b3 | dc igdvac, x19
A: 2d4524 d504433f | msr S0_4_C4_C3_1, xzr
A: 2d4568 ad46209e | ldp q30, q8, [x4, #0xc0]
A: 2d46bc 3cf35b81 | ldr q1, [x28, w19, uxtw #4]
A: 2d4adc d538a334 | mrs x20, AMAIR2_EL1
A: 2d4ee4 d502461f | msr S0_2_C4_C6_0, xzr
A: 2d5368 d51002e7 | msr DBGWCR2_EL1, x7
A: 2d5a08 d507457f | msr S0_7_C4_C5_3, xzr
A: 2d6064 d502483f | msr S0_2_C4_C8_1, xzr
A: 2d60b8 d505423f | msr S0_5_C4_C2_1, xzr
A: 2d6144 599ab0d2 | ldapursh x18, [x6, #-0x55]
A: 2d6168 dac137e8 | autizb x8
A: 2d64f8 6c698a7a | ldnp d26, d2, [x19, #-0x168]
A: 2d6a54 d508751c | ic iallu
A: 2d7438 d5064dff | msr S0_6_C4_C13_7, xzr
A: 2d8350 91b1881f | addg sp, x0, #0x310, #0x2
A: 2d84a4 d504403f | msr S0_4_C4_C0_1, xzr
A: 2d85f4 d36bb231 | ubfx x17, x17, #43, #2
A: 2d895c d5014e9f | msr S0_1_C4_C14_4, xzr
A: 2d8ccc d507477f | msr S0_7_C4_C7_3, xzr
A: 2d8f7c bc7f7924 | ldr s4, [x9, xzr, lsl #2]
A: 2d903c d50546bf | msr S0_5_C4_C6_5, xzr
A: 2d9310 d5074b5f | msr S0_7_C4_C11_2, xzr
A: 2d980c d507483f | msr S0_7_C4_C8_1, xzr
A: 2d9ca4 d505427f | msr S0_5_C4_C2_3, xzr
A: 2da55c d503271f | hint #0x38
A: 2dbc4c d5382776 | mrs x22, TCR2MASK_EL1
A: 2dced0 d507491f | msr S0_7_C4_C9_0, xzr
A: 2dd424 f8028c3e | str x30, [x1, #0x28]!
A: 2ddb98 3d58ff24 | ldr b4, [x25, #0x63f]
A: 2ddc40 b124545f | cmn x2, #0x915
A: 2de048 88ab7c82 | cas w11, w2, [x4]
A: 2de5f8 d9ff0bb5 | stz2g x21, [x29, #-0x100]
A: 2de7dc d9a3e4a1 | st2g x1, [x5], #0x3e0
A: 2df4c8 d5044cdf | msr S0_4_C4_C12_6, xzr
A: 2df610 d501409f | msr S0_1_C4_C0_4, xzr
A: 2df92c 5ac001a5 | rbit w5, w13
A: 2dff0c b821735f | stumin w1, [x26]
A: 2e0130 d63f08bf | blraaz x5
A: 2e0250 d50245ff | msr S0_2_C4_C5_7, xzr
A: 2e0704 8b156833 | add x19, x1, x21, lsl #26
A: 2e0788 1a8c8613 | csinc w19, w16, w12, hi
A: 2e08c0 d5064f9f | msr S0_6_C4_C15_4, xzr
A: 2e08e8 dac127e6 | pacizb x6
A: 2e128c 1b1ffd62 | mneg w2, w11, wzr
A: 2e12ac d50c919f | tlbi alle1osnxs
A: 2e143c d5032e3f | hint #0x71
A: 2e14c8 d50547ff | msr S0_5_C4_C7_7, xzr
A: 2e4ba0 b3788ab3 | bfi x19, x21, #8, #35
A: 2e6af0 f833b342 | st64bv x19, x2, [x26]
A: 2e6ce4 b6998587 | tbz x7, #0x33, 0x2e9d94
A: 2e94bc 19c7f111 | ldapursb w17, [x8, #0x7f]
A: 2eabe4 9183e0c7 | addg x7, x6, #0x30, #0x8
A: 2eb7dc 131534a1 | sbfiz w1, w5, #11, #14
A: 2ed0e4 bc7bfb07 | ldr s7, [x24, x27, sxtx #2]
A: 2edad0 bc4d9e6b | ldr s11, [x19, #0xd9]!
A: 2eed5c 11475c01 | add w1, w0, #0x1d7, lsl #12
A: 2f0664 a0a97312 | .inst 0xa0a97312
A: 2f1208 5a9586bd | cneg w29, w21, ls
A: 2f17a0 f898b26f | prfum plislcstrm, [x19, #-0x75]
A: 2f3de4 d91e43b8 | stlur x24, [x29, #-0x1c]
A: 2f3ebc 2a2303e0 | mvn w0, w3
A: 2f4b08 d9a003e9 | stgm x9, [sp]
A: 2f68ac 3cd96d0e | ldr q14, [x8, #-0x6a]!
A: 2f8e2c 3c169dca | str b10, [x14, #-0x97]!
A: 2f96dc d71f0cee | brab x7, x14
A: 2f9edc 544cc3df | bc.nv 0x393754
A: 2f9fe0 78035fff | strh wzr, [sp, #0x35]!
A: 2fa9c4 6e2efe19 | .inst 0x6e2efe19
A: 2fc4b8 d3420620 | lsl x0, x17, #62
A: 2fecf0 b3451f62 | bfxil x2, x27, #5, #3
A: 3003c8 780c8ecc | strh w12, [x22, #0xc8]!
A: 300814 b82f00e8 | ldadd w15, w8, [x7]
A: 301150 9a9f77d6 | csinc x22, x30, xzr, vc
A: 3048e0 dac008c2 | rev32 x2, x6
A: 306be0 9ec31eb8 | .inst 0x9ec31eb8
A: 3070d0 74378387 | .inst 0x74378387
A: 3076ec d9634def | stzg x15, [x15, #0x340]!
A: 308588 7c59bf81 | ldr h1, [x28, #-0x65]!
A: 309b70 d2c00015 | movz x21, #0x0, lsl #32
A: 30be04 dac123fb | paciza x27
A: 30c650 d42ff7c0 | brk #0x7fbe
A: 30c890 6a5c7e70 | ands w16, w19, w28, lsr #31
A: 30ed60 8a9d425e | and x30, x18, x29, asr #16
A: 30f7e0 ab52d897 | adds x23, x4, x18, lsr #54
A: 310914 b895c801 | ldtrsw x1, [x0, #-0xa4]
A: 312370 3cd5f0fc | ldur q28, [x7, #-0xa1]
A: 315c74 d61f0cff | brabz x7
A: 317200 4dcb68ae | .inst 0x4dcb68ae
A: 3175d0 19dc74c3 | .inst 0x19dc74c3
A: 318ee0 458d0d50 | .inst 0x458d0d50
A: 3192dc d37fff55 | lsr x21, x26, #63
A: 319d14 eb3ec2ce | subs x14, x22, w30, sxtw
A: 31a4a8 b89e7fc0 | ldrsw x0, [x30, #-0x19]!
A: 31a518 786edad7 | ldrh w23, [x22, w14, sxtw #1]
A: 31cdd0 f80bca98 | sttr x24, [x20, #0xbc]
A: 31ee70 f8a9fbda | prfm #0x1a, [x30, x9, sxtx #3]
A: 3202e4 38d49517 | ldrsb w23, [x8], #-0xb7
A: 322660 f8ad76c4 | ldrab x4, [x22, #0x6b8]
A: 325750 2aa946b1 | orn w17, w21, w9, asr #17
A: 3265f0 7859cdb0 | ldrh w16, [x13, #-0x64]!
A: 3266ec 1ad4245c | lsr w28, w2, w20
A: 326e3c 7c4871df | ldur h31, [x14, #0x87]
A: 328b98 1a859261 | csel w1, w19, w5, ls
A: 328ca4 190966bc | .inst 0x190966bc
A: 329e10 d500421f | cfinv
A: 32b39c b34c67de | bfxil x30, x30, #12, #14
A: 32b4a4 d63f0140 | blr x10
A: 32b618 9ade24c3 | lsr x3, x6, x30
A: 32ba2c 72a249de | movk w30, #0x124e, lsl #16
A: 32c584 92166345 | and x5, x26, #0xfffffc07fffffc07
A: 32f764 a9b81aca | stp x10, x6, [x22, #-0x80]!
A: 3318b8 1d15e497 | .inst 0x1d15e497
A: 3322e0 5a841485 | cneg w5, w4, eq
A: 3324f8 1b157d91 | mul w17, w12, w21
A: 332b28 93403e19 | sxth x25, w16
A: 332ef0 d417b241 | svc #0xbd92
A: 3334d4 d9f82412 | stz2g x18, [x0], #-0x7e0
A: 3362c4 d4a6d1c1 | dcps1 #0x368e
A: 336a64 994ba1a0 | ldapur w0, [x13, #0xba]
A: 3372b4 176767ad | b 0xfffffffffdd11168
A: 337c04 1b107eb5 | mul w21, w21, w16
A: 338fe0 195ef432 | .inst 0x195ef432
A: 339464 d171fd50 | sub x16, x10, #0xc7f, lsl #12
A: 33979c 9bbe7f57 | umull x23, w26, w30
A: 33c79c 6899fa3b | stgp x27, x30, [x17], #0x330
A: 33d068 d946c18d | ldapur x13, [x12, #0x6c]
A: 33ed44 a895d87a | stp x26, x22, [x3], #0x158
A: 33f9a4 880ec2f6 | stlxr w14, w22, [x23]
A: 340068 e42ffc05 | .inst 0xe42ffc05
A: 34174c 3846d0e7 | ldurb w7, [x7, #0x6d]
A: 343c70 1ad754a6 | crc32ch w6, w5, w23
A: 343fd8 b844df4d | ldr w13, [x26, #0x4d]!
A: 344bd0 c888b73d | stlr x29, [x25]
A: 34539c d65f01e0 | ret x15
A: 347bf0 aa187553 | orr x19, x10, x24, lsl #29
A: 348848 936dfbe4 | sbfx x4, xzr, #45, #18
A: 34a278 384c099a | ldtrb w26, [x12, #0xc0]
A: 34a58c dac107da | pacib x26, x30
A: 34aca0 dac11024 | autia x4, x1
A: 34afc0 928a018c | mov x12, #-0x500d
A: 34bfa4 330351f4 | bfxil w20, w15, #3, #18
A: 34c07c 2cf223d4 | ldp s20, s8, [x30], #-0x70
A: 34c6a0 88869b8b | stlr w11, [x28]
A: 34d4c4 9bb0bdef | umsubl x15, w15, w16, x15
A: 34e268 482a7d68 | casp x10, x11, x8, x9, [x11]
A: 34e538 590283b9 | stlurh w25, [x29, #0x28]
A: 34ed18 6d67460e | ldp d14, d17, [x16, #-0x190]
A: 34f2c0 293a54f2 | stp w18, w21, [x7, #-0x30]
A: 351850 d5033a3f | dsb ishnxs
A: 352b50 2de3a947 | ldp s7, s10, [x10, #-0xe4]!
A: 3532c4 1ad620e2 | lsl w2, w7, w22
A: 354104 d46ea460 | tcancel #0x7523
A: 354c74 3dd31853 | ldr q19, [x2, #0x4c60]
A: 3564c4 ea96b230 | ands x16, x17, x22, asr #44
A: 35729c 9ada136f | irg x15, x27, x26
A: 357c5c 3c2ccb86 | str b6, [x28, w12, sxtw]
A: 35b5d4 3c14dd42 | str b2, [x10, #-0xb3]!
A: 35c608 1b11d658 | msub w24, w18, w17, w21
A: 35d5d0 381aca46 | sttrb w6, [x18, #-0x54]
A: 35ddf0 1cf04bdb | ldr s27, 0x33e768
A: 35e894 131b7e72 | asr w18, w19, #27
A: 35f964 13003fd5 | sxth w21, w30
A: 35ffec 9baf0c2b | umaddl x11, w1, w15, x3
A: 3618c0 8b368a79 | add x25, x19, w22, sxtb #2
A: 361d64 9adc2616 | lsr x22, x16, x28
A: 3624c8 3d5dc51b | ldr b27, [x8, #0x771]
A: 3654d8 3818a752 | strb w18, [x26], #-0x76
A: 367198 3854f725 | ldrb w5, [x25], #-0xb1
A: 367454 9999115b | ldapursw x27, [x10, #-0x6f]
A: 367a28 29f31290 | ldp w16, w4, [x20, #-0x68]!
A: 368af8 d42fac80 | brk #0x7d64
A: 368b40 1d0bd67c | .inst 0x1d0bd67c
A: 36f890 6ca04a74 | stp d20, d18, [x19], #-0x200
A: 36fac8 53091dc0 | ubfiz w0, w14, #23, #8
A: 371458 f837b324 | st64bv x23, x4, [x25]
A: 371a20 b88089f2 | ldtrsw x18, [x15, #0x8]
A: 3723b4 d9243d06 | stg x6, [x8, #0x430]!
A: 372b30 98afa24b | ldrsw x11, 0x2d1f78
A: 373868 b802d8ea | sttr w10, [x7, #0x2d]
A: 376830 dac008c8 | rev32 x8, x6
A: 377dac 9b38ba1a | smsubl x26, w16, w24, x14
A: 377ea8 b849678b | ldr w11, [x28], #0x96
A: 37c774 1997813a | ldapursb x26, [x9, #-0x88]
A: 37dd38 d967868f | stzg x15, [x20], #0x780
A: 37deb8 2ad7176b | orr w11, w27, w23, ror #5
A: 37f494 ad2dd912 | stp q18, q22, [x8, #-0x250]
A: 380cc0 0db19065 | .inst 0x0db19065
A: 3817b8 b4d1ee0e | cbz x14, 0x325578
A: 381928 d503367f | msr S0_3_C3_C6_3, xzr
A: 382f80 5ccd2ed0 | ldr d16, 0x31d558
A: 38313c 38c87271 | ldursb w17, [x19, #0x87]
A: 383444 b1665b97 | adds x23, x28, #0x996, lsl #12
A: 384178 194ee0e2 | ldapurb w2, [x7, #0xee]
A: 38462c b83832b4 | ldset w24, w20, [x21]
A: 388c64 dac00fdf | rev xzr, x30
A: 388c9c 19d7006d | ldapursb w13, [x3, #-0x90]
A: 3897e4 289644fb | stp w27, w17, [x7], #0xb0
A: 38a29c c8c06a2d | ldlar x13, [x17]
A: 38bef0 a99aa36b | stp x11, x8, [x27, #0x1a8]!
A: 38c350 d9b478cd | st2g x13, [x6, #-0xb90]
A: 38d724 88a17cb2 | cas w1, w18, [x5]
A: 393610 396553ae | ldrb w14, [x29, #0x954]
A: 394c98 3cd2be8d | ldr q13, [x20, #-0xd5]!
A: 395638 bc04541b | str s27, [x0], #0x45
A: 397094 2c02e548 | stnp s8, s25, [x10, #0x14]
A: 399ab8 bc01c7bc | str s28, [x29], #0x1c
A: 39b974 9ad1106e | irg x14, x3, x17
A: 39bd18 bad00304 | subps x4, x24, x16
A: 39be04 b56c76ae | cbnz x14, 0x474cd8
A: 39febc b82e62a7 | ldumax w14, w7, [x21]
A: 3a1258 ad56de76 | ldp q22, q23, [x19, #0x2d0]
A: 3a3410 942677ec | bl 0xd413c0
A: 3a3604 b81eb34c | stur w12, [x26, #-0x15]
A: 3a3fd0 6970483d | ldpsw x29, x18, [x1, #-0x80]
A: 3a4fe0 ada317af | stp q15, q5, [x29, #-0x3a0]!
A: 3a92fc d92001af | stzgm x15, [x13]
A: 3abc7c b83f11bf | stclr wzr, [x13]
A: 3acca4 f82c52df | stsmin x12, [x22]
A: 3ad4f8 29ff81b1 | ldp w17, w0, [x13, #-0x4]!
A: 3ae378 d46f8180 | tcancel #0x7c0c
A: 3b16b8 1aca28f4 | asr w20, w7, w10
A: 3b3700 5a1b00b9 | sbc w25, w5, w27
A: 3b37a8 38dda7d3 | ldrsb w19, [x30], #-0x26
A: 3b4528 990fd3c0 | stlur w0, [x30, #0xfd]
A: 3b74c0 35d09cea | cbnz w10, 0x35885c
A: 3b828c 5a9f83f3 | csetm w19, ls
A: 3b9da8 dac11ad6 | autda x22, x22
A: 3bb318 bc53e73b | ldr s27, [x25], #-0xc2
A: 3bb3c4 d73f0ac2 | blraa x22, x2
A: 3bb754 781ea510 | strh w16, [x8], #-0x16
A: 3bb928 bd50f346 | ldr s6, [x26, #0x10f0]
A: 3bcec8 7a4dcbcd | ccmp w30, #0xd, #0xd, gt
A: 3bd72c b36c6c2e | bfi x14, x1, #20, #28
A: 3be540 8857f88f | ldaxr w15, [x4]
A: 3bea2c 4c9c2f54 | .inst 0x4c9c2f54
A: 3bff34 b4ff5b16 | cbz x22, 0x3bea94
A: 3c241c b8bfc219 | ldapr w25, [x16]
A: 3c2978 b8548304 | ldur w4, [x24, #-0xb8]
A: 3c43e4 39aed509 | ldrsb x9, [x8, #0xbb5]
A: 3c62d8 d61f0b5f | braaz x26
A: 3c785c 935aff9f | asr xzr, x28, #26
A: 3c822c 39d0d8a9 | ldrsb w9, [x5, #0x436]
A: 3c9c58 380cd98f | sttrb w15, [x12, #0xcd]
A: 3ca0a0 b88f96b4 | ldrsw x20, [x21], #0xf9
A: 3cd060 79587176 | ldrh w22, [x11, #0xc38]
A: 3cd57c ac1ee1b7 | stnp q23, q24, [x13, #0x3d0]
A: 3cd918 9ad1226b | lsl x11, x19, x17
A: 3cee88 b8463768 | ldr w8, [x27], #0x63
A: 3cf2a8 8868e77b | ldaxp w27, w25, [x27]
A: 3cf4d0 6ccaf322 | ldp d2, d28, [x25], #0xa8
A: 3d27dc 7c591786 | ldr h6, [x28], #-0x6f
A: 3d4878 9ade1321 | irg x1, x25, x30
A: 3d5fd4 7d43981d | ldr h29, [x0, #0x1cc]
A: 3d6238 3a5a19c6 | ccmn w14, #0x1a, #0x6, ne
A: 3d6a3c 2d1d0704 | stp s4, s1, [x24, #0xe8]
A: 3d73a4 bc1acc79 | str s25, [x3, #-0x54]!
A: 3d78ac 387bea64 | ldrb w4, [x19, x27, sxtx]
A: 3d89a4 b3537131 | bfxil x17, x9, #19, #10
A: 3da9dc eb8e509e | subs x30, x4, x14, asr #20
A: 3dd924 fa53d32f | ccmp x25, x19, #0xf, le
A: 3dde48 131c2906 | sbfiz w6, w8, #4, #11
A: 3dfc38 d3597646 | ubfx x6, x18, #25, #5
A: 3e1d10 1ac252be | crc32cb w30, w21, w2
A: 3e2028 a03804dd | .inst 0xa03804dd
A: 3e2d4c 9ba2ff0b | umnegl x11, w24, w2
A: 3e3170 9b070e9e | madd x30, x20, x7, x3
A: 3e3bb8 dac13ff9 | autdzb x25
A: 3e5a34 13003e11 | sxth w17, w16
A: 3e613c b8597bf5 | ldtr w21, [sp, #-0x69]
A: 3e6f58 d9bd9f92 | st2g x18, [x28, #-0x270]!
A: 3e77c0 bc52b28f | ldur s15, [x20, #-0xd5]
A: 3e7b44 5ac014a4 | cls w4, w5
A: 3ea338 fc490680 | ldr d0, [x20], #0x90
A: 3ec7a8 3cc447d3 | ldr q19, [x30], #0x44
A: 3ed828 13037c50 | asr w16, w2, #3
A: 3eee1c d92bd7c1 | stg x1, [x30], #0xbd0
A: 3f12ec dac10de9 | pacdb x9, x15
A: 3f2048 d50331df | isb #1
A: 3f4010 3ccb16e1 | ldr q1, [x23], #0xb1
A: 3f43b0 9ac02dcd | ror x13, x14, x0
A: 3f49bc 93407d0f | sxtw x15, w8
A: 3f4e38 d5033fff | sb
A: 3f5114 0000e980 | udf #0xe980
A: 3f5334 38ab7a3c | ldrsb x28, [x17, x11, lsl #0]
A: 3f622c b9acbc89 | ldrsw x9, [x4, #0x2cbc]
A: 3f6720 d2da2e4b | mov x11, #0xd17200000000
A: 3f70a0 c80cf49c | stlxr w12, x28, [x4]
A: 3f84c4 f82ea12c | st64bv0 x14, x12, [x9]
A: 3f9840 d9f19b8f | stz2g x15, [x28, #-0xe70]
A: 3fe73c 883e584a | stxp w30, w10, w22, [x2]
A: 3feca0 fc5c7d50 | ldr d16, [x10, #-0x39]!
A: 3ff684 33091180 | bfi w0, w12, #23, #5
A: 40222c 1aca2119 | lsl w25, w8, w10
A: 402664 78d6c64d | ldrsh w13, [x18], #-0x94
A: 402e28 a8081778 | stnp x24, x5, [x27, #0x80]
A: 4030cc 781f5a52 | sttrh w18, [x18, #-0xb]
A: 403ca0 b88ba12c | ldursw x12, [x9, #0xba]
A: 4060f0 f167314e | subs x14, x10, #0x9cc, lsl #12
A: 407134 b852efa6 | ldr w6, [x29, #-0xd2]!
A: 407518 2b0c28bf | cmn w5, w12, lsl #10
A: 4082f8 13855dbe | extr w30, w13, w5, #0x17
A: 408f18 d71f0c6a | brab x3, x10
A: 40ba44 3a004acd | setf16 w22
A: 40c388 fc41fd89 | ldr d9, [x12, #0x1f]!
A: 40c5ec 9bb652bb | umaddl x27, w21, w22, x20
A: 40c810 13103d6d | sbfiz w13, w11, #16, #16
A: 40d5c0 a981d5e3 | stp x3, x21, [x15, #0x18]!
A: 40dab8 78ca4867 | ldtrsh w7, [x3, #0xa4]
A: 40ee24 3c4d3e0a | ldr b10, [x16, #0xd3]!
A: 40fa8c fd28c124 | str d4, [x9, #0x5180]
A: 410090 4cc16f37 | .inst 0x4cc16f37
A: 4118b0 d44dbf80 | hlt #0x6dfc
A: 4119b4 788b3b2b | ldtrsh x11, [x25, #0xb3]
A: 411d98 b86bdb28 | ldr w8, [x25, w11, sxtw #2]
A: 4127c4 d28ce96d | mov x13, #0x674b
A: 412d28 f82111df | stclr x1, [x14]
A: 41547c 9ac301f7 | subp x23, x15, x3
A: 4158b0 78766825 | ldrh w5, [x1, x22]
A: 4159b0 7801c18d | sturh w13, [x12, #0x1c]
A: 416aa4 531b59af | ubfiz w15, w13, #5, #23
A: 4170c0 297b10bc | ldp w28, w4, [x5, #-0x28]
A: 4188a4 bc0371d2 | stur s18, [x14, #0x37]
A: 41a6c4 54fb8dcd | b.le 0x41187c
A: 41b028 53171ea7 | ubfiz w7, w21, #9, #8
A: 41b61c 3c22fbc7 | str b7, [x30, x2, sxtx #0]
A: 41d6e0 7c5a4715 | ldr h21, [x24], #-0x5c
A: 41df44 d9a27403 | st2g x3, [x0], #0x270
A: 421598 3cd541c6 | ldur q6, [x14, #-0xac]
A: 4228c0 38c51506 | ldrsb w6, [x8], #0x51
A: 426720 ce50b626 | .inst 0xce50b626
A: 427214 3a18007a | adcs w26, w3, w24
A: 427708 2862769e | ldnp w30, w29, [x20, #-0xf0]
A: 427b80 3a69dc56 | .inst 0x3a69dc56
A: 4286bc 7842e759 | ldrh w25, [x26], #0x2e
A: 42a210 79037e1c | strh w28, [x16, #0x1be]
A: 42a9e0 d503387f | msr S0_3_C3_C8_3, xzr
A: 42bfd4 9b507189 | smulh x9, x12, x16
A: 42d37c 53184bfd | ubfiz w29, wzr, #8, #19
A: 42ef1c 6d9c4775 | stp d21, d17, [x27, #0x1c0]!
A: 4301b4 7c4a5147 | ldur h7, [x10, #0xa5]
A: 43147c 994770ae | ldapur w14, [x5, #0x77]
A: 431aa0 72bc6fe2 | movk w2, #0xe37f, lsl #16
A: 432060 b82a409f | stsmax w10, [x4]
A: 4323a0 a982085d | stp x29, x2, [x2, #0x20]!
A: 432f10 b34924a8 | bfxil x8, x5, #9, #1
A: 434054 7c2a695c | str h28, [x10, x10]
A: 43dac0 948dbd5a | bl 0x27ad028
A: 43e8b8 4ad23f11 | eor w17, w24, w18, ror #15
A: 43fe98 d90f4215 | stlur x21, [x16, #0xf4]
A: 440c3c 68ca8fcb | ldpsw x11, x3, [x30], #0x54
A: 44170c 9ad9314f | pacga x15, x10, x25
A: 4429e8 b9173643 | str w3, [x18, #0x1734]
A: 4432ec cadf4599 | eor x25, x12, xzr, ror #17
A: 443558 cb9b9a66 | sub x6, x19, x27, asr #38
A: 44372c 5a9733f8 | csinv w24, wzr, w23, lo
A: 443894 d4a00003 | dcps3
A: 443e54 b83c4b58 | str w24, [x26, w28, uxtw]
A: 44509c da0903f3 | ngc x19, x9
A: 4452d0 191da087 | stlurb w7, [x4, #-0x26]
A: 445528 1c03b81b | ldr s27, 0x44cc28
A: 447258 d503309f | ssbb
A: 447c94 de13d29d | .inst 0xde13d29d
A: 44c6ec 4a5702f4 | eor w20, w23, w23, lsr #0
A: 44df38 bc0e427a | stur s26, [x19, #0xe4]
A: 44e768 728048cc | movk w12, #0x246
A: 44fc5c d61f02a0 | br x21
A: 45437c d89b3707 | prfm pldslcstrm, 0x38aa5c
A: 454a78 f849bd30 | ldr x16, [x9, #0x9b]!
A: 457768 a849542d | ldnp x13, x21, [x1, #0x90]
A: 458758 729732a0 | movk w0, #0xb995
A: 460ae4 6b2b8644 | subs w4, w18, w11, sxtb #1
A: 462c54 4aac56d3 | eon w19, w22, w12, asr #21
A: 464230 789494eb | ldrsh x11, [x7], #-0xb7
A: 4651b4 f1380f50 | subs x16, x26, #0xe03
A: 466998 ac785d2c | ldnp q12, q23, [x9, #-0x100]
A: 466b58 d5033c3f | msr S0_3_C3_C12_1, xzr
A: 46a9f4 71101543 | subs w3, w10, #0x405
A: 46caf0 78a1789d | ldrsh x29, [x4, x1, lsl #1]
A: 46d460 f835739f | stumin x21, [x28]
A: 46e568 e0fc7162 | .inst 0xe0fc7162
A: 46ed2c 1ac52099 | lsl w25, w4, w5
A: 46fb1c bc5807f0 | ldr s16, [sp], #-0x80
A: 472db0 a6aaac94 | .inst 0xa6aaac94
A: 476940 7972d92d | ldrh w13, [x9, #0x196c]
A: 476eb8 a91e54a5 | stp x5, x21, [x5, #0x1e0]
A: 477804 78e4da4a | ldrsh w10, [x18, w4, sxtw #1]
A: 477ad0 37bc50b6 | tbnz w22, #0x17, 0x4704e4
A: 47a4c0 0b3ca5f9 | add w25, w15, w28, sxth #1
A: 47a718 edadac94 | .inst 0xedadac94
A: 47ab20 e039f3a2 | .inst 0xe039f3a2
A: 47cdd0 9ac34f4a | crc32x w10, w26, x3
A: 47da78 9b3498fd | smsubl x29, w7, w20, x6
A: 481730 c87fc4c0 | ldaxp x0, x17, [x6]
A: 482fa8 ed3b5efc | .inst 0xed3b5efc
A: 486624 bc42f1a5 | ldur s5, [x13, #0x2f]
A: 487514 78487659 | ldrh w25, [x18], #0x87
A: 48b74c 3a10019e | adcs w30, w12, w16
A: 48d538 a7c98023 | .inst 0xa7c98023
A: 48d53c dac11066 | autia x6, x3
A: 48d93c 53081c0a | lsl w10, w0, #24
A: 49030c dac11554 | autib x20, x10
A: 491318 f82aa108 | st64bv0 x10, x8, [x8]
A: 491c88 8b3460a4 | add x4, x5, x20, uxtx
A: 4938ac 3c90311a | stur q26, [x8, #-0xfd]
A: 493fdc d975db04 | stzg x4, [x24, #-0xa30]
A: 495170 9adf1075 | irg x21, x3
A: 496aac 71784b5f | cmp w26, #0xe12, lsl #12
A: 496de8 b9169552 | str w18, [x10, #0x1694]
A: 4973fc 1ac32a57 | asr w23, w18, w3
A: 498e7c 4b2ae0b8 | sub w24, w5, w10, sxtx
A: 49946c 384497f2 | ldrb w18, [sp], #0x49
A: 49fbf0 d5033fdf | isb
A: 4a4b48 ad138988 | stp q8, q2, [x12, #0x270]
A: 4a4b9c 4cdba88d | .inst 0x4cdba88d
A: 4a8120 3c91d4f7 | str q23, [x7], #-0xe3
A: 4a9668 299ed51a | stp w26, w21, [x8, #0xf4]!
A: 4a9a34 f80ecc36 | str x22, [x1, #0xec]!
A: 4aba60 428d9a1c | .inst 0x428d9a1c
A: 4abb78 1b11d525 | msub w5, w9, w17, w21
A: 4ad13c 183a67da | ldr w26, 0x521e34
A: 4ad4cc 2c647b68 | ldnp s8, s30, [x27, #-0xe0]
A: 4adc1c bc555c4d | ldr s13, [x2, #-0xab]!
A: 4ae060 381c02b8 | sturb w24, [x21, #-0x40]
A: 4b0220 d9baf9ca | st2g x10, [x14, #-0x510]
A: 4b05c0 1acd0a91 | udiv w17, w20, w13
A: 4b0b30 b821822b | swp w1, w11, [x17]
A: 4b5a38 9ba8fee3 | umnegl x3, w23, w8
A: 4b6404 382c7823 | strb w3, [x1, x12, lsl #0]
A: 4b80ec 928a825b | mov x27, #-0x5413
A: 4b85d8 dac12ff8 | pacdzb x24
A: 4b8d08 2c2da10c | stnp s12, s8, [x8, #-0x94]
A: 4bb324 9acc32d5 | pacga x21, x22, x12
A: 4bb470 d9e00027 | ldgm x7, [x1]
A: 4bd680 191894d0 | .inst 0x191894d0
A: 4c1da4 3c9d0d47 | str q7, [x10, #-0x30]!
A: 4c237c 6df20a09 | ldp d9, d2, [x16, #-0xe0]!
A: 4c2b44 1add5747 | crc32ch w7, w26, w29
A: 4c2c58 3c0517c4 | str b4, [x30], #0x51
A: 4c756c a1647fca | .inst 0xa1647fca
A: 4c89a0 1adc4023 | crc32b w3, w1, w28
A: 4c8b24 53153cd5 | ubfiz w21, w6, #11, #16
A: 4c980c bc7f7990 | ldr s16, [x12, xzr, lsl #2]
A: 4c9d2c 9b2e24e6 | smaddl x6, w7, w14, x9
A: 4cdc30 38d33b17 | ldtrsb w23, [x24, #-0xcd]
A: 4cfb2c dac007de | rev16 x30, x30
A: 4d1ffc f82e01fb | ldadd x14, x27, [x15]
A: 4d2c18 adbe393d | stp q29, q14, [x9, #-0x40]!
A: 4d366c f2f7ed7f | movk xzr, #0xbf6b, lsl #48
A: 4d4304 f99063cb | prfm plil2strm, [x30, #0x20c0]
A: 4d4abc 195db059 | ldapurb w25, [x2, #-0x25]
A: 4d6c70 7a540309 | ccmp w24, w20, #0x9, eq
A: 4d7600 9035685b | adrp x27, 0x6b1df000
A: 4d8e00 c82d7603 | stxp w13, x3, x29, [x16]
A: 4d97bc 3c5df0eb | ldur b11, [x7, #-0x21]
A: 4d9db0 13085f58 | sbfx w24, w26, #8, #16
A: 4da4b8 3a0008ad | setf8 w5
A: 4dca78 9ac32676 | lsr x22, x19, x3
A: 4df238 5ac009ae | rev w14, w13
A: 4e3c74 784ba81e | ldtrh w30, [x0, #0xba]
A: 4e3f24 b9b45d04 | ldrsw x4, [x8, #0x345c]
A: 4e41d4 a8a377d9 | stp x25, x29, [x30], #-0x1d0
A: 4e42f0 19463763 | .inst 0x19463763
A: 4e8600 eb03a7c2 | subs x2, x30, x3, lsl #41
A: 4e8d84 69f88a64 | ldpsw x4, x2, [x19, #-0x3c]!
A: 4e9828 53003e4e | uxth w14, w18
A: 4e9a38 6345b000 | .inst 0x6345b000
A: 4ead40 bd522497 | ldr s23, [x4, #0x1224]
A: 4ec7e8 d523317d | ttest x29
A: 4f0dc4 139b377e | ror w30, w27, #0xd
A: 4f1004 2a6a08e3 | orn w3, w7, w10, lsr #2
A: 4f3d48 a8b8ee8f | stp x15, x27, [x20], #-0x78
A: 4f4e84 28787e44 | ldnp w4, wzr, [x18, #-0x40]
A: 4f5a80 28fc16f0 | ldp w16, w5, [x23], #-0x20
A: 4f5c6c 2db5b1d4 | stp s20, s12, [x14, #-0x54]!
A: 4f67cc d920019e | stzgm x30, [x12]
A: 4f6dc0 f807d5b5 | str x21, [x13], #0x7d
A: 4f6e58 33184d84 | bfi w4, w12, #8, #20
A: 4f8c88 b824731f | stumin w4, [x24]
A: 4fd9f0 d5033f1f | msr S0_3_C3_C15_0, xzr
A: 4ff71c dac12ffe | pacdzb x30
A: 5002cc 3a18011a | adcs w26, w8, w24
A: 50216c 9b32fe7c | smnegl x28, w19, w18
A: 502bc0 9ac54d3b | crc32x w27, w9, x5
A: 504d1c dac10724 | pacib x4, x25
A: 50549c 3892658c | ldrsb x12, [x12], #-0xda
A: 50643c d50333ff | sb
A: 5068b4 ea96e1df | tst x14, x22, asr #56
A: 5068c4 b3545aff | bfxil xzr, x23, #20, #3
A: 508250 1399072d | ror w13, w25, #0x1
A: 508990 a8c36bfe | ldp x30, x26, [sp], #0x30
A: 5094a4 17179222 | b 0xfffffffffcaedd2c
A: 509cdc 29a0626b | stp w11, w24, [x19, #-0x100]!
A: 509e68 53001c5b | uxtb w27, w2
A: 509fc0 3894159c | ldrsb x28, [x12], #-0xbf
A: 50dc4c 1adb514e | crc32cb w14, w10, w27
A: 50dcf0 780e6fda | strh w26, [x30, #0xe6]!
A: 50e1c4 bad203f9 | subps x25, sp, x18
A: 50e754 fa58fb07 | ccmp x24, #0x18, #0x7, nv
A: 510360 9acb5cf9 | crc32cx w25, w7, x11
A: 511c1c bc70f965 | ldr s5, [x11, x16, sxtx #2]
A: 5136dc da9a75d0 | csneg x16, x14, x26, vc
A: 513780 c83be99e | stlxp w27, x30, x26, [x12]
A: 518408 7c16b5de | str h30, [x14], #-0x95
A: 518bec 9b21bcaf | smsubl x15, w5, w1, x15
A: 51978c f83ac62b | ldraa x11, [x17, #0xd60]
A: 51a264 adf7adbf | ldp q31, q11, [x13, #-0x110]!
A: 51bb30 f88cf3bd | prfum #0x1d, [x29, #0xcf]
A: 51bf5c fc1fd26e | stur d14, [x19, #-0x3]
A: 51efe8 d6e342bf | .inst 0xd6e342bf
A: 52012c aab01011 | orn x17, x0, x16, asr #4
A: 520238 ca57f7d1 | eor x17, x30, x23, lsr #61
A: 520870 7a1f0075 | sbcs w21, w3, wzr
A: 524f48 3c5dc5e2 | ldr b2, [x15], #-0x24
A: 526250 bd386ae7 | str s7, [x23, #0x3868]
A: 526ba0 4a8f4525 | eor w5, w9, w15, asr #17
A: 528418 936aa536 | sbfiz x22, x9, #22, #42
A: 529094 48367e28 | casp x22, x23, x8, x9, [x17]
A: 52a114 3d318d4f | str b15, [x10, #0xc63]
A: 52a480 f83f9006 | st64b x6, [x0]
A: 52ada4 f90106b9 | str x25, [x21, #0x208]
A: 52b198 93407f7f | sxtw xzr, w27
A: 52cebc 6d84e77e | stp d30, d25, [x27, #0x48]!
A: 52eee0 53127db1 | lsr w17, w13, #18
A: 52f414 1904a5fc | .inst 0x1904a5fc
A: 530ac4 3c8f11f2 | stur q18, [x15, #0xf1]
A: 53169c 2c1586f1 | stnp s17, s1, [x23, #0xac]
A: 5325bc 393f777f | strb wzr, [x27, #0xfdd]
A: 533a58 784078e0 | ldtrh w0, [x7, #0x7]
A: 534314 1ac04470 | crc32h w16, w3, w0
A: 534e40 f852f524 | ldr x4, [x9], #-0xd1
A: 536b2c 138a0d5c | ror w28, w10, #0x3
A: 536da8 b843c17d | ldur w29, [x11, #0x3c]
A: 53775c 6c3d9270 | stnp d16, d4, [x19, #-0x28]
A: 537bbc 52b0f9ec | mov w12, #-0x78310000
A: 538208 6dec72cf | ldp d15, d28, [x22, #-0x140]!
A: 538528 d408bde1 | svc #0x45ef
A: 53907c f836712e | ldumin x22, x14, [x9]
A: 53a034 9346e1de | sbfx x30, x14, #6, #51
A: 53bfd0 0a5c3631 | and w17, w17, w28, lsr #13
A: 53c6cc 69caf1f9 | ldpsw x25, x28, [x15, #0x54]!
A: 54086c 13177dc9 | asr w9, w14, #23
A: 540ecc 386b4bbd | ldrb w29, [x29, w11, uxtw]

S" disa64" .RESULTS
