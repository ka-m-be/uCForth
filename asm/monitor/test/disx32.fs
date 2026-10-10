\ disx32.fs - tests of the package DISX86 (asm/monitor/disx86.fs) with 32-bit code
\
\ Load (in the folder ucf), after the package FILES and the tester:
\   build/ucf-c -32 host/files.fs forth/test/tester.fs asm/monitor/test/disx32.fs
\ The tests need cells of 32 bits.
\
\ Each line X: is one instruction: its address, its bytes, "|", and the
\ text that DECODE must give; the instruction must also end after its
\ bytes. A line DB: has bytes that start no instruction: DECODE gives
\ "db" and the first byte, and goes on after it. The texts come from
\ llvm-objdump 21 (Apple LLVM, the triple i386-pc-none-elf, "-M intel"), once,
\ and are kept here as data. The bytes: each opcode of one byte and
\ after 0Fh with 29 kinds of ModR/M, SIB and displacement, each without
\ a prefix and with the prefixes 66h, 67h, F2h, F3h, F0h, 2Eh and 64h,
\ each byte ModR/M with some opcodes, each SIB with mod 0, 1 and 2:
\ 70 820 instructions, checked once against llvm-objdump without an open
\ difference. These lines are a choice: each opcode with each of its
\ names, a sample with prefixes, bytes that are no instruction, and some
\ of SSE, AVX and the coprocessor. Where this disassembler writes
\ otherwise (asm/monitor/README.md, "x86"), the text here is changed, by
\ the rules of the 16-bit test (asm/monitor/test/disx86.fs) and these:
\
\   - no "addr16", "addr32", "data16": the prefix shows in the operands;
\   - a SIB byte without index and base has none: "[0x1000]", not
\     "[8*eiz + 0x1000]";
\   - LOCK, REP and REPNE also before the coprocessor (esc); XACQUIRE and
\     XRELEASE (F2h, F3h for a lock) as REPNE and REP;
\   - the coprocessor (D8h to DFh) is esc, also where llvm-objdump knows
\     no instruction; int1 also with prefixes;
\   - MMX, SSE, AVX and the prefix REX2 (D5h) are db (not in DISX86);
\     0Fh AEh and 0Fh C7h with 66h, F2h or F3h only as CLWB, VMCLEAR,
\     VMXON, RDPID, RDFSBASE ... WRGSBASE, and RDRAND, RDSEED with 66h,
\     else db; CET, PTWRITE, WAITPKG and LKGS are db;
\   - an instruction that ignores a prefix shows without it (llvm-objdump:
\     no instruction), for example 66h 0Fh 09h: wbinvd;
\   - with 67h, A0h to A3h are mov and E3h jecxz (llvm-objdump in 64-bit
\     code: "addr32 movabs", "addr32 jrcxz"; and "addr32 tpause" for 67h
\     0Fh AEh F6h, which is mfence).

ONLY FORTH DEFINITIONS  ALSO FILES
S" lib/text.fs" INCLUDED
ONLY FORTH DEFINITIONS  ALSO FILES
S" asm/monitor/monitor.fs" INCLUDED
S" asm/monitor/disx86.fs" INCLUDED
ONLY FORTH DEFINITIONS  ALSO INTERNALS  ALSO TEXT  ALSO MONITOR  ALSO DISX86
DECIMAL  X86-32

CREATE CODE-BUF 16 ALLOT   VARIABLE CODE-AT   VARIABLE CODE-U
: CODE-C@ ( a -- c )   CODE-AT @ -  DUP CODE-U @ U< IF CODE-BUF + C@ ELSE DROP $90 THEN ;
' CODE-C@ IS DIS-C@
: HEXDIGIT ( c -- n )   DUP '9' > IF  32 OR 'a' - 10 +  ELSE  '0' -  THEN ;
: HEX# ( a u -- n )   0 SWAP 0 ?DO  4 LSHIFT  OVER I + C@ HEXDIGIT OR  LOOP  NIP ;
: BAR? ( a u -- f )   DUP 1 = IF  OVER C@ '|' =  ELSE  FALSE  THEN  NIP NIP ;
VARIABLE LEN                         \ the length that DECODE must give; 0: all bytes
: CASE ( "addr" "bytes" "|" "text" -- )   \ DECODE gives the text and the length
  PARSE-NAME HEX# CODE-AT !  0 CODE-U !
  BEGIN  PARSE-NAME 2DUP BAR? 0= WHILE  HEX# CODE-BUF CODE-U @ + C!  1 CODE-U +!  REPEAT  2DROP
  LEN @ 0= IF  CODE-U @ LEN !  THEN
  SOURCE >IN @ /STRING -LEADING -TRAILING              ( a u )
  CODE-AT @ DECODE CODE-AT @ -  >R
  1 #TESTS +!
  2DUP TXT COUNT S= 0=  R> LEN @ <> OR IF
    1 #ERRORS +!  ." WRONG: " SOURCE TYPE CR ."   got: " TXT COUNT TYPE CR
  THEN  2DROP  SOURCE NIP >IN ! ;
: X: ( "addr" "bytes" "|" "text" -- )   0 LEN !  CASE ;    \ an instruction: it ends after its bytes
: DB: ( "addr" "bytes" "|" "text" -- )   1 LEN !  CASE ;   \ no instruction: the first byte as db

X: 000e 00 05 11 22 33 44 | add byte ptr [0x44332211], al
X: 009c 00 0c 8d 10 00 00 00 | add byte ptr [4*ecx + 0x10], cl
X: 0136 00 d3 | add bl, dl
X: 0152 00 e0 | add al, ah
X: 05c8 f2 00 40 7f | repne add byte ptr [eax + 0x7f], al
X: 07ee 01 40 7f | add dword ptr [eax + 0x7f], eax
X: 0831 01 0c 8d 10 00 00 00 | add dword ptr [4*ecx + 0x10], ecx
X: 08a1 01 c1 | add ecx, eax
X: 0911 01 f0 | add eax, esi
X: 0d9a f2 01 04 88 | repne add dword ptr [eax + 4*ecx], eax
X: 0f51 02 04 88 | add al, byte ptr [eax + 4*ecx]
X: 0ff7 02 45 fc | add al, byte ptr [ebp - 0x4]
X: 1079 02 d8 | add bl, al
X: 1087 02 e0 | add ah, al
X: 12d8 67 02 04 | add al, byte ptr [si]
X: 16b1 66 67 02 c8 | add cl, al
X: 170c 03 04 24 | add eax, dword ptr [esp]
X: 1781 03 1c e5 00 10 00 00 | add ebx, dword ptr [0x1000]
X: 17d0 03 c0 | add eax, eax
X: 1840 03 e8 | add ebp, eax
X: 1a1b 66 03 f0 | add si, ax
X: 1fb0 04 d8 | add al, 0xd8
X: 1fbe 04 e0 | add al, 0xe0
X: 2411 f3 04 80 | rep add al, 0x80
X: 25eb 66 67 04 44 | add al, 0x44
X: 26ac 05 84 88 00 01 | add eax, 0x1008884
X: 27ad 05 f6 11 22 33 | add eax, 0x332211f6
X: 2b79 f3 05 0c 8d 10 00 | rep add eax, 0x108d0c
X: 2f29 06 | push es
X: 30d7 66 06 | push es
X: 34f8 66 67 06 | push es
X: 35e7 07 | pop es
X: 3898 66 07 | pop es
X: 3c65 64 07 | pop es
X: 3d9c 08 0c 8d 10 00 00 00 | or byte ptr [4*ecx + 0x10], cl
X: 3e28 08 d0 | or al, dl
X: 42d7 f2 08 f0 | repne or al, dh
X: 4575 09 75 00 | or dword ptr [ebp], esi
X: 461f 09 f6 | or esi, esi
X: 4c94 0a 41 80 | or al, byte ptr [ecx - 0x80]
X: 4d7e 0a e0 | or ah, al
X: 5219 f2 0a d3 | repne or dl, bl
X: 548f 0b 45 fc | or eax, dword ptr [ebp - 0x4]
X: 54f5 0b d0 | or edx, eax
X: 55cf 66 0b 44 24 08 | or ax, word ptr [esp + 0x8]
X: 59e7 f0 0b c0 | lock or eax, eax
X: 5c04 0c 0c | or al, 0xc
X: 6028 67 0c d0 | or al, 0xd0
X: 6313 0d 05 ff fe ff | or eax, 0xfffeff05
X: 6c65 0e | push cs
X: 6f70 67 0e | push cs
X: 7161 2e 0e | push cs
X: 72f5 10 0c 8d 10 00 00 00 | adc byte ptr [4*ecx + 0x10], cl
X: 7365 10 c1 | adc cl, al
X: 742b 66 10 04 88 | adc byte ptr [eax + 4*ecx], al
X: 7a67 11 80 10 20 30 40 | adc dword ptr [eax + 0x40302010], eax
X: 7b5e 11 e8 | adc eax, ebp
X: 8029 f0 11 f8 | lock adc eax, edi
X: 8094 2e 11 e8 | adc eax, ebp
X: 81e1 12 44 24 08 | adc al, byte ptr [esp + 0x8]
X: 82c0 12 d0 | adc dl, al
X: 86ee f3 12 e8 | rep adc ch, al
X: 87ef f0 12 04 25 ff fe ff ff | lock adc al, byte ptr [0xfffffeff]
X: 89f0 13 45 fc | adc eax, dword ptr [ebp - 0x4]
X: 8a56 13 d0 | adc edx, eax
X: 8bb0 66 13 45 fc | adc ax, word ptr [ebp - 0x4]
X: 8ff1 2e 13 0c 8d 10 00 00 00 | adc ecx, dword ptr cs:[4*ecx + 0x10]
X: 9207 14 d3 | adc al, 0xd3
X: 9647 f3 14 e0 | rep adc al, 0xe0
X: 9a18 15 ff ff fe ff | adc eax, 0xfffeffff
X: a113 16 | push ss
X: a90b 17 | pop ss
X: af95 18 04 24 | sbb byte ptr [esp], al
X: b0e5 18 f6 | sbb dh, dh
X: b32e 67 18 44 24 | sbb byte ptr [si + 0x24], al
X: b51f f2 18 f6 | repne sbb dh, dh
X: b7da 19 75 00 | sbb dword ptr [ebp], esi
X: b806 19 c1 | sbb ecx, eax
X: becc 1a 44 24 08 | sbb al, byte ptr [esp + 0x8]
X: bfe3 1a e5 | sbb ah, ch
X: c1f9 66 1a ff | sbb bh, bh
X: c316 67 1a c0 | sbb al, al
X: c4e7 f0 1a 80 10 20 30 40 | lock sbb al, byte ptr [eax + 0x40302010]
X: c538 2e 1a e0 | sbb ah, al
X: c5ae 64 1a 45 fc | sbb al, byte ptr fs:[ebp - 0x4]
X: c678 1b 40 7f | sbb eax, dword ptr [eax + 0x7f]
X: c72b 1b c1 | sbb eax, ecx
X: cc23 f2 1b c0 | repne sbb eax, eax
X: cf2a 1c e5 | sbb al, 0xe5
X: d024 66 1c 0c | sbb al, 0xc
X: d709 1d f8 ff fe ff | sbb eax, 0xfffefff8
X: dc5c 64 1d c8 ff fe ff | sbb eax, 0xfffeffc8
X: dcbd 66 67 1d d8 ff | sbb ax, 0xffd8
X: de7d 1e | push ds
X: ded2 66 1e | push ds
X: e374 f0 1e | lock push ds
X: e3a2 2e 1e | push ds
X: e553 1f | pop ds
X: eab2 f2 1f | repne pop ds
X: eb51 2e 1f | pop ds
X: ec45 20 00 | and byte ptr [eax], al
X: edc1 20 f0 | and al, dh
X: f313 2e 20 d8 | and al, bl
X: f340 64 20 75 00 | and byte ptr fs:[ebp], dh
X: f407 21 04 88 | and dword ptr [eax + 4*ecx], eax
X: f52f 21 d8 | and eax, ebx
X: f97d f3 21 e8 | rep and eax, ebp
X: fc65 22 75 00 | and dh, byte ptr [ebp]
X: fd1d 22 f8 | and bh, al
X: fe65 66 22 c8 | and cl, al
X: 100db f3 22 f6 | rep and dh, dh
X: 103aa 23 84 88 00 01 00 00 | and eax, dword ptr [eax + 4*ecx + 0x100]
X: 1043b 23 c8 | and ecx, eax
X: 105b3 66 23 5d 08 | and bx, word ptr [ebp + 0x8]
X: 10c24 24 e5 | and al, 0xe5
X: 1106b f3 24 04 | rep and al, 0x4
X: 11167 2e 24 75 | and al, 0x75
X: 112c6 25 41 80 11 22 | and eax, 0x22118041
X: 11829 f2 25 d0 11 22 33 | repne and eax, 0x332211d0
X: 11b5b 27 | daa
X: 12045 f0 27 | lock daa
X: 1226a 28 75 00 | sub byte ptr [ebp], dh
X: 122c0 28 d3 | sub bl, dl
X: 12a13 29 34 24 | sub dword ptr [esp], esi
X: 12a68 29 d8 | sub eax, ebx
X: 12df3 67 29 d3 | sub ebx, edx
X: 130aa 66 67 29 e0 | sub ax, sp
X: 13113 2a 40 7f | sub al, byte ptr [eax + 0x7f]
X: 13236 2a f0 | sub dh, al
X: 136b5 f2 2a c0 | repne sub al, al
X: 1393f 2b 34 24 | sub esi, dword ptr [esp]
X: 139f6 2b ff | sub edi, edi
X: 13b12 66 2b c0 | sub ax, ax
X: 14189 2c f8 | sub al, 0xf8
X: 14599 f3 2c 04 | rep sub al, 0x4
X: 147a3 2d 05 11 22 33 | sub eax, 0x33221105
X: 14db2 f0 2d e0 11 22 33 | lock sub eax, 0x332211e0
X: 14f59 2f | das
X: 1576d 30 1c e5 00 10 00 00 | xor byte ptr [0x1000], bl
X: 15810 30 e0 | xor al, ah
X: 15b9c 67 30 d8 | xor al, bl
X: 15e6f 31 04 88 | xor dword ptr [eax + 4*ecx], eax
X: 15fc1 31 e8 | xor eax, ebp
X: 163b8 f3 31 e5 | rep xor ebp, esp
X: 1642d f2 31 c0 | repne xor eax, eax
X: 1652c 64 31 c1 | xor ecx, eax
X: 165f5 32 00 | xor al, byte ptr [eax]
X: 1678d 32 f8 | xor bh, al
X: 16c3a f0 32 f6 | lock xor dh, dh
X: 16dae 33 04 88 | xor eax, dword ptr [eax + 4*ecx]
X: 16f38 33 ff | xor edi, edi
X: 17659 34 d0 | xor al, 0xd0
X: 17df2 35 c8 11 22 33 | xor eax, 0x332211c8
X: 183e1 64 35 d0 11 22 33 | xor eax, 0x332211d0
X: 184ec 37 | aaa
X: 18655 66 37 | aaa
X: 18ae5 2e 37 | aaa
X: 18bf3 66 67 37 | aaa
X: 18c4c 38 44 24 08 | cmp byte ptr [esp + 0x8], al
X: 18d8d 38 f6 | cmp dh, dh
X: 191ae f3 38 5d 08 | rep cmp byte ptr [ebp + 0x8], bl
X: 19311 64 38 45 fc | cmp byte ptr fs:[ebp - 0x4], al
X: 19362 66 67 38 04 | cmp byte ptr [si], al
X: 19475 39 5d 08 | cmp dword ptr [ebp + 0x8], ebx
X: 1952e 39 f6 | cmp esi, esi
X: 19937 f3 39 44 24 08 | rep cmp dword ptr [esp + 0x8], eax
X: 19ae1 66 67 39 e8 | cmp ax, bp
X: 19c2c 3a 75 00 | cmp dh, byte ptr [ebp]
X: 19c90 3a d8 | cmp bl, al
X: 1a18c f0 3a 40 7f | lock cmp al, byte ptr [eax + 0x7f]
X: 1a28a 66 67 3a c1 | cmp al, cl
X: 1a328 3b 04 24 | cmp eax, dword ptr [esp]
X: 1a440 3b e0 | cmp esp, eax
X: 1a5bf 66 3b c1 | cmp ax, cx
X: 1aa7a 66 67 3b 1c | cmp bx, word ptr [si]
X: 1abef 3c e5 | cmp al, 0xe5
X: 1b10b 2e 3c 05 | cmp al, 0x5
X: 1b1b9 64 3c e0 | cmp al, 0xe0
X: 1b2ec 3d 45 fc 11 22 | cmp eax, 0x2211fc45
X: 1ba2b 3f | aas
X: 1bbe6 66 3f | aas
X: 1bfd8 f0 3f | lock aas
X: 1c080 2e 3f | aas
X: 1c12b 66 67 3f | aas
X: 1c25a 40 | inc eax
X: 1c5cf 67 40 | inc eax
X: 1c81b 2e 40 | inc eax
X: 1caae 41 | inc ecx
X: 1d107 42 | inc edx
X: 1d77f 64 42 | inc edx
X: 1d9b6 43 | inc ebx
X: 1de6e f0 43 | lock inc ebx
X: 1e0e3 44 | inc esp
X: 1e3bb 67 44 | inc esp
X: 1e7db 45 | inc ebp
X: 1eeba 66 67 45 | inc bp
X: 1ef2c 46 | inc esi
X: 1f60d 64 46 | inc esi
X: 1f6ad 47 | inc edi
X: 1ff7e 48 | dec eax
X: 206bd 49 | dec ecx
X: 20b7f f2 49 | repne dec ecx
X: 20c36 f0 49 | lock dec ecx
X: 20f03 4a | dec edx
X: 215c1 4b | dec ebx
X: 21bde 64 4b | dec ebx
X: 21c9a 4c | dec esp
X: 2240b 66 67 4c | dec sp
X: 22457 4d | dec ebp
X: 229bf f3 4d | rep dec ebp
X: 22b24 64 4d | dec ebp
X: 22c3f 4e | dec esi
X: 23303 66 67 4e | dec si
X: 23418 4f | dec edi
X: 237a8 67 4f | dec edi
X: 23a84 64 4f | dec edi
X: 23b6b 50 | push eax
X: 24096 f3 50 | rep push eax
X: 24193 2e 50 | push eax
X: 241fd 64 50 | push eax
X: 2436a 51 | push ecx
X: 2492e 2e 51 | push ecx
X: 24bb3 52 | push edx
X: 24cb3 66 52 | push dx
X: 24fd9 f3 52 | rep push edx
X: 2508a f0 52 | lock push edx
X: 250d8 2e 52 | push edx
X: 2519d 66 67 52 | push dx
X: 252ef 53 | push ebx
X: 25835 f0 53 | lock push ebx
X: 258fe 64 53 | push ebx
X: 25994 54 | push esp
X: 261af 55 | push ebp
X: 2644d 66 55 | push bp
X: 2687b 66 67 55 | push bp
X: 26949 56 | push esi
X: 2705b 57 | push edi
X: 27423 67 57 | push edi
X: 27595 f3 57 | rep push edi
X: 27705 2e 57 | push edi
X: 278c0 58 | pop eax
X: 27ee7 64 58 | pop eax
X: 27f46 66 67 58 | pop ax
X: 280c2 59 | pop ecx
X: 286e4 66 67 59 | pop cx
X: 28834 5a | pop edx
X: 28b45 67 5a | pop edx
X: 28d7f f0 5a | lock pop edx
X: 28fb2 5b | pop ebx
X: 29779 5c | pop esp
X: 29d54 64 5c | pop esp
X: 29e42 5d | pop ebp
X: 2a2d7 67 5d | pop ebp
X: 2a6c1 5e | pop esi
X: 2a74e 66 5e | pop si
X: 2ab96 f2 5e | repne pop esi
X: 2abc4 f0 5e | lock pop esi
X: 2aca4 64 5e | pop esi
X: 2aec5 5f | pop edi
X: 2b30e f2 5f | repne pop edi
X: 2b37c f0 5f | lock pop edi
X: 2b463 66 67 5f | pop di
X: 2b518 60 | pushad
X: 2bcb3 61 | popad
X: 2c139 67 61 | popad
X: 2c45a 62 44 24 08 | bound eax, dword ptr [esp + 0x8]
X: 2cbf5 63 44 24 08 | arpl word ptr [esp + 0x8], ax
X: 2ccd4 63 d0 | arpl ax, dx
X: 2d27a 64 63 d0 | arpl ax, dx
X: 2d44e 68 c1 ff fe ff | push 0xfffeffc1
X: 2d4b0 68 e8 11 22 33 | push 0x332211e8
X: 2dafc 69 05 ff fe ff ff 80 00 00 00 | imul eax, dword ptr [0xfffffeff], 0x80
X: 2dc40 69 e0 11 22 33 44 | imul esp, eax, 0x44332211
X: 2e315 6a 0c | push 0xc
X: 2e359 6a 75 | push 0x75
X: 2ea14 6b 00 ff | imul eax, dword ptr [eax], -0x1
X: 2eb58 6b d8 11 | imul ebx, eax, 0x11
X: 2efb9 f3 6b c1 11 | rep imul eax, ecx, 0x11
X: 2f257 6c | insb byte ptr es:[edi], dx
X: 2f695 67 6c | insb byte ptr es:[di], dx
X: 2f9e6 6d | insd dword ptr es:[edi], dx
X: 30188 6e | outsb dx, byte ptr [esi]
X: 302a0 66 6e | outsb dx, byte ptr [esi]
X: 30642 f3 6e | rep outsb dx, byte ptr [esi]
X: 309b9 6f | outsd dx, dword ptr [esi]
X: 31043 70 04 | jo 0x31049
X: 3138e 66 70 f8 | jo 0x1389
X: 3157d f3 70 45 | rep jo 0x315c5
X: 31906 71 d8 | jno 0x318e0
X: 31a93 66 71 c1 | jno 0x1a57
X: 31ba6 67 71 40 | jno 0x31be9
X: 31e73 2e 71 e0 | jno 0x31e56
X: 31e96 64 71 f6 | jno 0x31e8f
X: 3204c 72 34 | jb 0x32082
X: 32361 67 72 80 | jb 0x322e4
X: 3251f f2 72 84 | repne jb 0x324a6
X: 3276a 73 80 | jae 0x326ec
X: 32cea f2 73 1c | repne jae 0x32d09
X: 32fe0 74 e0 | je 0x32fc2
X: 33204 66 74 ff | je 0x3206
X: 334e0 f0 74 34 | lock je 0x33517
X: 33593 64 74 44 | je 0x335da
X: 33741 75 c8 | jne 0x3370b
X: 33ba5 f3 75 c0 | rep jne 0x33b68
X: 33c31 f2 75 c0 | repne jne 0x33bf4
X: 33d17 64 75 41 | jne 0x33d5b
X: 33e9e 76 75 | jbe 0x33f15
X: 3459d 77 04 | ja 0x345a3
X: 34776 66 77 40 | ja 0x47b9
X: 34ae6 f3 77 75 | rep ja 0x34b5e
X: 34c2c 2e 77 40 | ja 0x34c6f
X: 34e43 78 d8 | js 0x34e1d
X: 3535c f0 78 c0 | lock js 0x3531f
X: 355f1 79 e0 | jns 0x355d3
X: 35a22 f3 79 f6 | rep jns 0x35a1b
X: 35a92 f2 79 e0 | repne jns 0x35a75
X: 35b07 f0 79 45 | lock jns 0x35b4f
X: 35d68 7a d0 | jp 0x35d3a
X: 36222 f2 7a 04 | repne jp 0x36229
X: 36538 7b e5 | jnp 0x3651f
X: 366b8 66 7b c8 | jnp 0x6683
X: 368b6 67 7b d8 | jnp 0x36891
X: 36add 64 7b e8 | jnp 0x36ac8
X: 36cc0 7c d8 | jl 0x36c9a
X: 37137 f2 7c 40 | repne jl 0x3717a
X: 371d6 f0 7c f0 | lock jl 0x371c9
X: 37241 2e 7c e8 | jl 0x3722c
X: 373f0 7d 75 | jge 0x37467
X: 37686 66 7d ff | jge 0x7688
X: 378a4 f3 7d 80 | rep jge 0x37827
X: 37a91 66 67 7d 84 | jge 0x7a19
X: 37ad3 7e 04 | jle 0x37ad9
X: 383ca 7f f6 | jg 0x383c2
X: 388a8 f0 7f d3 | lock jg 0x3887e
X: 38985 66 67 7f c1 | jg 0x894a
X: 38ae2 80 c1 ff | add cl, 0xff
X: 39068 2e 80 1c e5 00 10 00 00 ff | sbb byte ptr cs:[0x1000], 0xff
X: 3917d 81 05 11 22 33 44 55 66 77 88 | add dword ptr [0x44332211], 0x88776655
X: 391e6 81 80 10 20 30 40 11 22 33 44 | add dword ptr [eax + 0x40302010], 0x44332211
X: 3920b 81 0c 8d 10 00 00 00 11 22 33 44 | or dword ptr [4*ecx + 0x10], 0x44332211
X: 3921e 81 1c e5 00 10 00 00 ff fe ff ff | sbb dword ptr [0x1000], 0xfffffeff
X: 3925e 81 34 24 11 22 33 44 | xor dword ptr [esp], 0x44332211
X: 3926d 81 c0 ff fe ff ff | add eax, 0xfffffeff
X: 3927b 81 c1 ff fe ff ff | add ecx, 0xfffffeff
X: 39289 81 c8 ff fe ff ff | or eax, 0xfffffeff
X: 39297 81 d0 11 22 33 44 | adc eax, 0x44332211
X: 392b3 81 d8 ff fe ff ff | sbb eax, 0xfffffeff
X: 392c1 81 e0 11 22 33 44 | and eax, 0x44332211
X: 392dd 81 e8 11 22 33 44 | sub eax, 0x44332211
X: 392eb 81 f0 ff fe ff ff | xor eax, 0xfffffeff
X: 39307 81 f8 ff fe ff ff | cmp eax, 0xfffffeff
X: 39939 82 04 25 ff fe ff ff 80 | add byte ptr [0xfffffeff], 0x80
X: 39957 82 44 24 08 11 | add byte ptr [esp + 0x8], 0x11
X: 399aa 82 0c 8d 10 00 00 00 11 | or byte ptr [4*ecx + 0x10], 0x11
X: 399bd 82 1c e5 00 10 00 00 ff | sbb byte ptr [0x1000], 0xff
X: 399fd 82 34 24 ff | xor byte ptr [esp], 0xff
X: 39a0c 82 c0 11 | add al, 0x11
X: 39a1a 82 c1 11 | add cl, 0x11
X: 39a28 82 c8 ff | or al, 0xff
X: 39a36 82 d0 11 | adc al, 0x11
X: 39a52 82 d8 11 | sbb al, 0x11
X: 39a60 82 e0 11 | and al, 0x11
X: 39a7c 82 e8 ff | sub al, 0xff
X: 39a8a 82 f0 11 | xor al, 0x11
X: 39ab4 82 ff 11 | cmp bh, 0x11
X: 3a109 83 41 80 ff | add dword ptr [ecx - 0x80], -0x1
X: 3a13d 83 0c 8d 10 00 00 00 11 | or dword ptr [4*ecx + 0x10], 0x11
X: 3a150 83 1c e5 00 10 00 00 11 | sbb dword ptr [0x1000], 0x11
X: 3a163 83 45 fc 11 | add dword ptr [ebp - 0x4], 0x11
X: 3a190 83 34 24 11 | xor dword ptr [esp], 0x11
X: 3a19f 83 c0 11 | add eax, 0x11
X: 3a1ad 83 c1 ff | add ecx, -0x1
X: 3a1bb 83 c8 11 | or eax, 0x11
X: 3a1d7 83 d3 11 | adc ebx, 0x11
X: 3a1e5 83 d8 11 | sbb eax, 0x11
X: 3a201 83 e5 11 | and ebp, 0x11
X: 3a20f 83 e8 11 | sub eax, 0x11
X: 3a22b 83 f6 11 | xor esi, 0x11
X: 3a239 83 f8 11 | cmp eax, 0x11
X: 3a767 2e 83 d8 11 | sbb eax, 0x11
X: 3a906 84 5d 08 | test byte ptr [ebp + 0x8], bl
X: 3a995 84 e5 | test ch, ah
X: 3ad9a f3 84 d3 | rep test bl, dl
X: 3afbd 66 67 84 75 00 | test byte ptr [di], dh
X: 3b056 85 80 10 20 30 40 | test dword ptr [eax + 0x40302010], eax
X: 3b14d 85 e8 | test eax, ebp
X: 3b63d f0 85 1c e5 00 10 00 00 | lock test dword ptr [0x1000], ebx
X: 3b6d2 64 85 e5 | test ebp, esp
X: 3b725 66 67 85 04 | test word ptr [si], ax
X: 3b7e5 86 40 7f | xchg byte ptr [eax + 0x7f], al
X: 3b8ec 86 e5 | xchg ch, ah
X: 3bfdf 87 1c e5 00 10 00 00 | xchg dword ptr [0x1000], ebx
X: 3c0ac 87 f0 | xchg eax, esi
X: 3c6dc 88 05 11 22 33 44 | mov byte ptr [0x44332211], al
X: 3c727 88 40 7f | mov byte ptr [eax + 0x7f], al
X: 3c812 88 d8 | mov al, bl
X: 3c84a 88 f0 | mov al, dh
X: 3cdca 64 88 e5 | mov ch, ah
X: 3cf90 89 d3 | mov ebx, edx
X: 3d634 8a 04 25 11 22 33 44 | mov al, byte ptr [0x44332211]
X: 3d680 8a 80 10 20 30 40 | mov al, byte ptr [eax + 0x40302010]
X: 3d769 8a e5 | mov ah, ch
X: 3d785 8a f0 | mov dh, al
X: 3e375 f2 8b e5 | repne mov esp, ebp
X: 3e412 2e 8b e8 | mov ebp, eax
X: 3e4dd 66 67 8b c0 | mov ax, ax
X: 3e53d 8c 05 11 22 33 44 | mov word ptr [0x44332211], es
X: 3e5de 8c 1c e5 00 10 00 00 | mov word ptr [0x1000], ds
X: 3e62d 8c c0 | mov eax, es
X: 3e665 8c d3 | mov ebx, ss
X: 3e902 67 8c 44 24 | mov word ptr [si + 0x24], es
X: 3eaf9 f2 8c 40 7f | repne mov word ptr [eax + 0x7f], es
X: 3ec04 2e 8c 84 88 00 01 00 00 | mov word ptr cs:[eax + 4*ecx + 0x100], es
X: 3f262 f3 8d 45 fc | rep lea eax, [ebp - 0x4]
X: 3f4ad 8e 04 25 11 22 33 44 | mov es, word ptr [0x44332211]
X: 3f544 8e 45 fc | mov es, word ptr [ebp - 0x4]
X: 3f580 8e c0 | mov es, eax
X: 3f5aa 8e d0 | mov ss, eax
X: 3f674 66 8e 04 24 | mov es, word ptr [esp]
X: 3f951 67 8e d3 | mov ss, ebx
X: 3fa78 f2 8e 05 ff fe ff ff | repne mov es, word ptr [0xfffffeff]
X: 3fc40 8f 04 25 11 22 33 44 | pop dword ptr [0x44332211]
X: 3fd13 8f c0 | pop eax
DB: 3fd67 8f e0 11 22 33 44 55 66 | db 0x8f
X: 4016b f3 8f 04 25 ff fe ff ff | rep pop dword ptr [0xfffffeff]
DB: 40248 f0 8f d3 ff fe ff ff 80 | db 0xf0
X: 40257 f0 8f c0 | lock pop eax
X: 403a7 90 | nop
X: 4094a f3 90 | pause
X: 40ccf 91 | xchg eax, ecx
X: 41014 67 91 | xchg eax, ecx
X: 41214 2e 91 | xchg eax, ecx
X: 413ac 92 | xchg eax, edx
X: 41b3f 93 | xchg eax, ebx
X: 41e40 67 93 | xchg eax, ebx
X: 42381 94 | xchg eax, esp
X: 428d6 2e 94 | xchg eax, esp
X: 42b34 95 | xchg eax, ebp
X: 43226 96 | xchg eax, esi
X: 438ef 97 | xchg eax, edi
X: 43e68 f3 97 | rep xchg eax, edi
X: 44109 98 | cwde
X: 448eb 99 | cdq
X: 44dda f2 99 | repne cdq
X: 44fea 9a 04 24 11 22 33 44 | lcall 0x4433, 0x22112404
X: 45585 f2 9a 1c e5 00 10 00 00 | repne lcall 0x0, 0x1000e51c
X: 4588c 9b | wait
X: 459b6 66 9b | wait
X: 45fd8 9c | pushfd
X: 4638b 67 9c | pushfd
X: 464c8 f2 9c | repne pushfd
X: 466c2 9d | popfd
X: 4686b 66 9d | popf
X: 46bde f3 9d | rep popfd
X: 46fb5 9e | sahf
X: 476f6 9f | lahf
X: 47d59 a0 05 11 22 33 | mov al, byte ptr [0x33221105]
X: 47ed5 a0 f6 11 22 33 | mov al, byte ptr [0x332211f6]
X: 484a1 66 67 a0 c1 11 | mov al, byte ptr [0x11c1]
X: 485e0 a1 c0 11 22 33 | mov eax, dword ptr [0x332211c0]
X: 485ee a1 c1 11 22 33 | mov eax, dword ptr [0x332211c1]
X: 48afd f0 a1 05 11 22 33 | lock mov eax, dword ptr [0x33221105]
X: 48b5c 2e a1 f8 ff fe ff | mov eax, dword ptr cs:[0xfffefff8]
X: 48d73 a2 34 24 11 22 | mov byte ptr [0x22112434], al
X: 48e0e a2 f6 ff fe ff | mov byte ptr [0xfffefff6], al
X: 48f55 66 a2 c1 11 22 33 | mov byte ptr [0x332211c1], al
X: 49459 a3 04 24 11 22 | mov dword ptr [0x22112404], eax
X: 494ce a3 1c e5 00 10 | mov dword ptr [0x1000e51c], eax
X: 49b3e 64 a3 75 00 ff fe | mov dword ptr fs:[0xfeff0075], eax
X: 49cb4 a4 | movsb byte ptr es:[edi], byte ptr [esi]
X: 4a33b 66 67 a4 | movsb byte ptr es:[di], byte ptr [si]
X: 4a440 a5 | movsd dword ptr es:[edi], dword ptr [esi]
X: 4a8c5 f3 a5 | rep movsd dword ptr es:[edi], dword ptr [esi]
X: 4abed a6 | cmpsb byte ptr [esi], byte ptr es:[edi]
X: 4ad44 66 a6 | cmpsb byte ptr [esi], byte ptr es:[edi]
X: 4afdc 67 a6 | cmpsb byte ptr [si], byte ptr es:[di]
X: 4b082 f3 a6 | rep cmpsb byte ptr [esi], byte ptr es:[edi]
X: 4b15e f0 a6 | lock cmpsb byte ptr [esi], byte ptr es:[edi]
X: 4b1d1 64 a6 | cmpsb byte ptr fs:[esi], byte ptr es:[edi]
X: 4b424 a7 | cmpsd dword ptr [esi], dword ptr es:[edi]
X: 4b4bd 66 a7 | cmpsw word ptr [esi], word ptr es:[edi]
X: 4b9b3 64 a7 | cmpsd dword ptr fs:[esi], dword ptr es:[edi]
X: 4ba65 a8 04 | test al, 0x4
X: 4bfe0 f3 a8 d0 | rep test al, 0xd0
X: 4c04c f0 a8 f8 | lock test al, 0xf8
X: 4c0ee 2e a8 84 | test al, 0x84
X: 4c2cf a9 34 24 11 22 | test eax, 0x22112434
X: 4c746 f3 a9 e5 11 22 33 | rep test eax, 0x332211e5
X: 4cabd aa | stosb byte ptr es:[edi], al
X: 4cf3f f2 aa | repne stosb byte ptr es:[edi], al
X: 4d293 ab | stosd dword ptr es:[edi], eax
X: 4d878 66 67 ab | stosw word ptr es:[di], ax
X: 4da29 ac | lodsb al, byte ptr [esi]
X: 4dc8e 67 ac | lodsb al, byte ptr [si]
X: 4e03c 66 67 ac | lodsb al, byte ptr [si]
X: 4e087 ad | lodsd eax, dword ptr [esi]
X: 4e6e4 2e ad | lodsd eax, dword ptr cs:[esi]
X: 4e93f ae | scasb al, byte ptr es:[edi]
X: 4ef80 66 67 ae | scasb al, byte ptr es:[di]
X: 4f08e af | scasd eax, dword ptr es:[edi]
X: 4f790 b0 40 | mov al, 0x40
X: 4f851 b0 c8 | mov al, 0xc8
X: 4fbe8 67 b0 c8 | mov al, 0xc8
X: 4fdec 2e b0 00 | mov al, 0x0
X: 4ff1d b1 04 | mov cl, 0x4
X: 50027 b1 d8 | mov cl, 0xd8
X: 5047c f3 b1 04 | rep mov cl, 0x4
X: 50759 b2 5d | mov dl, 0x5d
X: 50786 b2 c0 | mov dl, 0xc0
X: 50e13 66 67 b2 d8 | mov dl, 0xd8
X: 50e3f b3 04 | mov bl, 0x4
X: 50f03 b3 75 | mov bl, 0x75
X: 516b0 b4 34 | mov ah, 0x34
X: 51713 b4 e0 | mov ah, 0xe0
X: 51c8b 2e b4 5d | mov ah, 0x5d
X: 51cca 64 b4 40 | mov ah, 0x40
X: 51d5d b5 00 | mov ch, 0x0
X: 51eaf b5 e0 | mov ch, 0xe0
X: 5236f f0 b5 40 | lock mov ch, 0x40
X: 524c0 66 67 b5 e0 | mov ch, 0xe0
X: 52550 b6 40 | mov dh, 0x40
X: 5255f b6 41 | mov dh, 0x41
X: 52d22 b7 84 | mov bh, 0x84
X: 52e3f b7 ff | mov bh, 0xff
X: 53467 b8 04 24 11 22 | mov eax, 0x22112404
X: 53563 b8 d3 11 22 33 | mov eax, 0x332211d3
X: 536ef 66 b8 c0 ff | mov ax, 0xffc0
X: 53c6b b9 0c 8d 10 00 | mov ecx, 0x108d0c
X: 53cf7 b9 d0 11 22 33 | mov ecx, 0x332211d0
X: 541c3 f2 b9 75 00 ff fe | repne mov ecx, 0xfeff0075
X: 54240 2e b9 04 88 11 22 | mov ecx, 0x22118804
X: 543cc ba 40 7f 11 22 | mov edx, 0x22117f40
X: 543db ba 41 80 ff fe | mov edx, 0xfeff8041
X: 548d8 f3 ba e8 11 22 33 | rep mov edx, 0x332211e8
X: 54bf4 bb 34 24 11 22 | mov ebx, 0x22112434
X: 54c73 bb e8 11 22 33 | mov ebx, 0x332211e8
X: 54f44 67 bb 1c e5 00 10 | mov ebx, 0x1000e51c
X: 551b7 2e bb 40 7f 11 22 | mov ebx, 0x22117f40
X: 553c5 bc d0 ff fe ff | mov esp, 0xfffeffd0
X: 553e1 bc d8 11 22 33 | mov esp, 0x332211d8
X: 557b7 67 bc f0 11 22 33 | mov esp, 0x332211f0
X: 55b6b bd d0 11 22 33 | mov ebp, 0x332211d0
X: 55bcd bd f6 11 22 33 | mov ebp, 0x332211f6
X: 56141 64 bd 5d 08 11 22 | mov ebp, 0x2211085d
X: 56199 66 67 bd 34 24 | mov bp, 0x2434
X: 562a2 be 45 fc 11 22 | mov esi, 0x2211fc45
X: 56332 be e0 11 22 33 | mov esi, 0x332211e0
X: 5677b f3 be d3 11 22 33 | rep mov esi, 0x332211d3
X: 568e2 64 be c8 11 22 33 | mov esi, 0x332211c8
X: 569ba bf 04 25 11 22 | mov edi, 0x22112504
X: 56a2b bf 0c 8d 10 00 | mov edi, 0x108d0c
X: 56f32 f3 bf f6 ff fe ff | rep mov edi, 0xfffefff6
X: 56f8d f2 bf d8 11 22 33 | repne mov edi, 0x332211d8
X: 57058 64 bf 05 ff fe ff | mov edi, 0xfffeff05
X: 57186 c0 41 80 ff | rol byte ptr [ecx - 0x80], 0xff
X: 571ba c0 0c 8d 10 00 00 00 ff | ror byte ptr [4*ecx + 0x10], 0xff
X: 571ef c0 5d 08 11 | rcr byte ptr [ebp + 0x8], 0x11
X: 5721c c0 c0 ff | rol al, 0xff
X: 57238 c0 c8 11 | ror al, 0x11
X: 57254 c0 d3 ff | rcl bl, 0xff
X: 57262 c0 d8 ff | rcr al, 0xff
X: 5727e c0 e5 11 | shl ch, 0x11
X: 5728c c0 e8 ff | shr al, 0xff
X: 572c4 c0 ff 11 | sar bh, 0x11
X: 57674 f3 c0 d8 11 | rep rcr al, 0x11
X: 57765 f0 c0 04 25 ff fe ff ff 80 | lock rol byte ptr [0xfffffeff], 0x80
DB: 57793 2e c0 f0 11 22 33 44 55 | db 0x2e
X: 57909 c1 44 24 08 ff | rol dword ptr [esp + 0x8], 0xff
X: 5795c c1 0c 8d 10 00 00 00 ff | ror dword ptr [4*ecx + 0x10], 0xff
X: 57991 c1 5d 08 11 | rcr dword ptr [ebp + 0x8], 0x11
X: 579be c1 c0 11 | rol eax, 0x11
X: 579da c1 c8 11 | ror eax, 0x11
X: 579f6 c1 d3 11 | rcl ebx, 0x11
X: 57a04 c1 d8 11 | rcr eax, 0x11
X: 57a20 c1 e5 11 | shl ebp, 0x11
X: 57a2e c1 e8 11 | shr eax, 0x11
X: 57a58 c1 f8 ff | sar eax, 0xff
X: 57e35 f3 c1 c8 ff | rep ror eax, 0xff
X: 57f32 2e c1 e5 11 | shl ebp, 0x11
X: 57f8f 64 c1 e8 ff | shr eax, 0xff
X: 58160 c2 c1 11 | ret 0x11c1
X: 58638 f2 c2 e5 11 | repne ret 0x11e5
X: 586dc 2e c2 84 88 | ret 0x8884
X: 58945 c3 | ret
X: 58dcd f2 c3 | repne ret
X: 58f04 64 c3 | ret
X: 59035 c4 0c 8d 10 00 00 00 | les ecx, [4*ecx + 0x10]
X: 59791 c5 40 7f | lds eax, [eax + 0x7f]
X: 59c9e f3 c5 45 fc | rep lds eax, [ebp - 0x4]
X: 59ef9 c6 04 25 11 22 33 44 55 | mov byte ptr [0x44332211], 0x55
X: 59f27 c6 40 7f ff | mov byte ptr [eax + 0x7f], 0xff
X: 59fcc c6 c0 ff | mov al, 0xff
X: 59fda c6 c1 11 | mov cl, 0x11
X: 5a066 c6 f8 11 | xabort 0x11
DB: 5a415 67 c6 ff 11 22 33 44 55 | db 0x67
X: 5a76d c7 c1 ff fe ff ff | mov ecx, 0xfffffeff
X: 5a7f9 c7 f8 11 22 33 44 | xbegin 0x4438ca10
X: 5acc7 f0 c7 f8 11 22 33 44 | lock xbegin 0x4438cedf
X: 5ad38 64 c7 45 fc 11 22 33 44 | mov dword ptr fs:[ebp - 0x4], 0x44332211
X: 5af1b c8 c8 11 22 | enter 0x11c8, 0x22
X: 5b5b9 c9 | leave
X: 5bbb6 f0 c9 | lock leave
X: 5be64 ca d0 11 | retf 0x11d0
X: 5bf1e 66 ca 04 25 | retf 0x2504
X: 5c2f7 f2 ca f6 11 | repne retf 0x11f6
X: 5c3b6 2e ca 5d 08 | retf 0x85d
X: 5c57c cb | retf
X: 5c883 67 cb | retf
X: 5ca84 f3 cb | rep retf
X: 5cad3 f2 cb | repne retf
X: 5cce8 cc | int3
X: 5d034 67 cc | int3
X: 5d1e6 f3 cc | rep int3
X: 5d59c cd f8 | int 0xf8
X: 5dca4 ce | into
X: 5ddd8 66 ce | into
X: 5dfb9 67 ce | into
X: 5e225 f0 ce | lock into
X: 5e254 2e ce | into
X: 5e291 64 ce | into
X: 5e4a6 cf | iretd
X: 5e920 f2 cf | repne iretd
X: 5e9eb 2e cf | iretd
X: 5eb39 d0 44 24 08 | rol byte ptr [esp + 0x8], 1
X: 5eb8c d0 0c 8d 10 00 00 00 | ror byte ptr [4*ecx + 0x10], 1
X: 5ebc1 d0 5d 08 | rcr byte ptr [ebp + 0x8], 1
X: 5ebfc d0 c1 | rol cl, 1
X: 5ec0a d0 c8 | ror al, 1
X: 5ec18 d0 d0 | rcl al, 1
X: 5ec34 d0 d8 | rcr al, 1
X: 5ec42 d0 e0 | shl al, 1
X: 5ec5e d0 e8 | shr al, 1
X: 5ec88 d0 f8 | sar al, 1
X: 5f064 f3 d0 04 25 ff fe ff ff | rep rol byte ptr [0xfffffeff], 1
X: 5f2ed d1 41 80 | rol dword ptr [ecx - 0x80], 1
X: 5f321 d1 0c 8d 10 00 00 00 | ror dword ptr [4*ecx + 0x10], 1
X: 5f356 d1 5d 08 | rcr dword ptr [ebp + 0x8], 1
X: 5f391 d1 c1 | rol ecx, 1
X: 5f39f d1 c8 | ror eax, 1
X: 5f3bb d1 d3 | rcl ebx, 1
X: 5f3c9 d1 d8 | rcr eax, 1
X: 5f3d7 d1 e0 | shl eax, 1
X: 5f3f3 d1 e8 | shr eax, 1
X: 5f41d d1 f8 | sar eax, 1
X: 5f84b f2 d1 44 24 08 | repne rol dword ptr [esp + 0x8], 1
X: 5fa86 d2 41 80 | rol byte ptr [ecx - 0x80], cl
X: 5faba d2 0c 8d 10 00 00 00 | ror byte ptr [4*ecx + 0x10], cl
X: 5facd d2 1c e5 00 10 00 00 | rcr byte ptr [0x1000], cl
DB: 5fb0d d2 34 24 11 22 33 44 55 | db 0xd2
X: 5fb1c d2 c0 | rol al, cl
X: 5fb38 d2 c8 | ror al, cl
X: 5fb46 d2 d0 | rcl al, cl
X: 5fb62 d2 d8 | rcr al, cl
X: 5fb70 d2 e0 | shl al, cl
X: 5fb8c d2 e8 | shr al, cl
DB: 5fba8 d2 f6 11 22 33 44 55 66 | db 0xd2
X: 5fbb6 d2 f8 | sar al, cl
X: 60134 64 d2 04 88 | rol byte ptr fs:[eax + 4*ecx], cl
X: 601d3 d3 04 88 | rol dword ptr [eax + 4*ecx], cl
X: 60253 d3 0c 8d 10 00 00 00 | ror dword ptr [4*ecx + 0x10], cl
X: 60266 d3 1c e5 00 10 00 00 | rcr dword ptr [0x1000], cl
X: 602c3 d3 c1 | rol ecx, cl
X: 602d1 d3 c8 | ror eax, cl
X: 602ed d3 d3 | rcl ebx, cl
X: 602fb d3 d8 | rcr eax, cl
X: 60317 d3 e5 | shl ebp, cl
X: 60325 d3 e8 | shr eax, cl
X: 6034f d3 f8 | sar eax, cl
X: 6071d f3 d3 f8 | rep sar eax, cl
X: 60897 64 d3 04 24 | rol dword ptr fs:[esp], cl
X: 60ac9 d4 e8 | aam 0xe8
X: 61155 d5 40 | aad 0x40
X: 61445 66 d5 f0 | aad 0xf0
X: 616e6 f2 d5 f0 | repne aad 0xf0
X: 6180a 64 d5 d8 | aad 0xd8
X: 6192a d6 | salc
X: 621a8 d7 | xlatb
X: 622cc 66 d7 | xlatb
X: 6273d 64 d7 | xlatb
X: 628df d8 34 24 | esc 0x6, word ptr [esp]
X: 62d95 f3 d8 f0 | rep esc 0x6, ax
X: 63137 66 d9 00 | esc 0x8, word ptr [eax]
X: 63805 da 75 00 | esc 0x16, word ptr [ebp]
X: 63e18 64 da 34 24 | esc 0x16, word ptr fs:[esp]
X: 646e9 dc 0c 8d 10 00 00 00 | esc 0x21, word ptr [4*ecx + 0x10]
X: 64783 dc d3 | esc 0x22, bx
X: 64a8c 67 dc 1c | esc 0x23, word ptr [si]
X: 64b0d 67 dc d0 | esc 0x22, ax
X: 64bc1 f3 dc 45 fc | rep esc 0x20, word ptr [ebp - 0x4]
X: 64ce2 2e dc 84 88 00 01 00 00 | esc 0x20, word ptr cs:[eax + 4*ecx + 0x100]
X: 65043 66 dd 0c 8d 10 00 00 00 | esc 0x29, word ptr [4*ecx + 0x10]
X: 654e0 64 dd 04 24 | esc 0x28, word ptr fs:[esp]
X: 6550f 64 dd f0 | esc 0x2e, ax
X: 6560e de 84 88 00 01 00 00 | esc 0x30, word ptr [eax + 4*ecx + 0x100]
X: 658a1 66 de e0 | esc 0x34, ax
X: 66069 66 df f0 | esc 0x3e, ax
X: 66306 f2 df f0 | repne esc 0x3e, ax
X: 66528 e0 41 | loopne 0x6656b
X: 66c60 e1 05 | loope 0x66c67
X: 66f50 66 e1 d3 | loope 0x6f26
X: 66fe6 67 e1 05 | loope 0x66fee
X: 672d5 2e e1 5d | loope 0x67335
X: 67373 64 e1 f6 | loope 0x6736c
X: 67382 66 67 e1 0c | loope 0x7392
X: 67489 e2 0c | loop 0x67497
X: 67b55 66 67 e2 00 | loop 0x7b59
X: 67bb2 e3 04 | jecxz 0x67bb8
X: 68074 67 e3 d8 | jcxz 0x6804f
X: 68225 2e e3 f6 | jecxz 0x6821e
X: 6847f e4 d8 | in al, 0xd8
X: 6855e 66 e4 41 | in al, 0x41
X: 68a5b 64 e4 c8 | in al, 0xc8
X: 68aec e5 05 | in eax, 0x5
X: 6940c e6 f6 | out 0xf6, al
X: 69544 66 e6 c0 | out 0xc0, al
X: 698f6 f0 e6 d8 | lock out 0xd8, al
X: 69ba1 e7 f0 | out 0xf0, eax
X: 69d32 66 e7 d8 | out 0xd8, ax
X: 6a234 e8 80 10 20 30 | call 0x3026b2b9
X: 6a246 e8 84 88 00 01 | call 0x1072acf
X: 6a889 2e e8 f6 11 22 33 | call 0x3328ba85
X: 6a90f 66 67 e8 75 00 | call 0xa989
X: 6aa2a e9 45 fc 11 22 | jmp 0x2218a674
X: 6ab00 e9 f8 11 22 33 | jmp 0x3328bcfd
X: 6aecd f3 e9 80 10 20 30 | rep jmp 0x3026bf53
X: 6afea 2e e9 84 88 00 01 | jmp 0x1073874
X: 6b0c9 66 67 e9 41 80 | jmp 0x310f
X: 6b208 ea c0 11 22 33 44 55 | ljmp 0x5544, 0x332211c0
X: 6b6e1 f2 ea 05 11 22 33 44 55 | repne ljmp 0x5544, 0x33221105
X: 6b99c eb c0 | jmp 0x6b95e
X: 6ba36 eb f8 | jmp 0x6ba30
X: 6bda9 67 eb e8 | jmp 0x6bd94
X: 6c072 ec | in al, dx
X: 6c799 66 67 ec | in al, dx
X: 6c8b9 ed | in eax, dx
X: 6cf57 66 67 ed | in ax, dx
X: 6d0ff ee | out dx, al
X: 6d606 2e ee | out dx, al
X: 6d73b ef | out dx, eax
X: 6dae5 67 ef | out dx, eax
X: 6dc95 f3 ef | rep out dx, eax
X: 6df44 f1 | int1
X: 6e3eb f3 f1 | rep int1
X: 6e789 f4 | hlt
X: 6ec4e f0 f4 | lock hlt
X: 6ee44 f5 | cmc
X: 6f52e 66 67 f5 | cmc
X: 6f5ef f6 80 10 20 30 40 ff | test byte ptr [eax + 0x40302010], 0xff
X: 6f627 f6 1c e5 00 10 00 00 | neg byte ptr [0x1000]
X: 6f658 f6 75 00 | div byte ptr [ebp]
X: 6f684 f6 c1 11 | test cl, 0x11
X: 6f6a0 f6 d0 | not al
X: 6f6bc f6 d8 | neg al
X: 6f6d8 f6 e5 | mul ch
X: 6f6e6 f6 e8 | imul al
X: 6f6f4 f6 f0 | div al
X: 6f71e f6 ff | idiv bh
X: 6f77a 66 f6 44 24 08 ff | test byte ptr [esp + 0x8], 0xff
X: 6fce4 66 67 f6 5d 08 | neg byte ptr [di + 0x8]
X: 6fe03 f7 c0 ff fe ff ff | test eax, 0xfffffeff
X: 70012 66 f7 d8 | neg ax
X: 70144 67 f7 1c | neg dword ptr [si]
X: 7026a f3 f7 00 11 22 33 44 | rep test dword ptr [eax], 0x44332211
X: 703e1 64 f7 05 11 22 33 44 55 66 77 88 | test dword ptr fs:[0x44332211], 0x88776655
X: 704e9 f8 | clc
X: 70a48 f3 f8 | rep clc
X: 70a84 f2 f8 | repne clc
X: 70d60 f9 | stc
X: 714a8 fa | cli
X: 71a19 f0 fa | lock cli
X: 71c80 fb | sti
X: 724c0 fc | cld
X: 728e6 f2 fc | repne cld
X: 7296f f0 fc | lock cld
X: 72c17 fd | std
X: 72cfd 66 fd | std
X: 730f0 f0 fd | lock std
X: 7327f fe 04 25 11 22 33 44 | inc byte ptr [0x44332211]
X: 732f0 fe 0c 8d 10 00 00 00 | dec byte ptr [4*ecx + 0x10]
DB: 73334 fe 75 00 11 22 33 44 55 | db 0xfe
X: 73360 fe c1 | inc cl
X: 7336e fe c8 | dec al
X: 73a92 ff 0c 8d 10 00 00 00 | dec dword ptr [4*ecx + 0x10]
X: 73b2c ff d3 | call ebx
DB: 74474 66 0f 00 34 24 11 22 33 | db 0x66
X: 74640 67 0f 00 5d 08 | ltr word ptr [di + 0x8]
X: 749f3 2e 0f 00 41 80 | sldt word ptr cs:[ecx - 0x80]
X: 74af6 0f 01 05 11 22 33 44 | sgdtd [0x44332211]
X: 74bc5 0f 01 5d 08 | lidtd [ebp + 0x8]
X: 74be5 0f 01 34 24 | lmsw word ptr [esp]
X: 74bf5 0f 01 c0 | enclv
X: 74c04 0f 01 c1 | vmcall
X: 74c13 0f 01 c8 | monitor
X: 74c22 0f 01 d0 | xgetbv
X: 74c40 0f 01 d8 | vmrun
X: 74c6d 0f 01 e8 | serialize
X: 74ca9 0f 01 ff | tlbsync
X: 75445 0f 02 04 88 | lar eax, word ptr [eax + 4*ecx]
X: 755da 0f 02 f8 | lar edi, ax
X: 75ddb 0f 03 41 80 | lsl eax, word ptr [ecx - 0x80]
X: 75e89 0f 03 c1 | lsl eax, cx
X: 76641 64 0f 03 d3 | lsl edx, bx
X: 771c8 0f 05 | syscall
X: 778be 64 0f 05 | syscall
X: 77a70 0f 06 | clts
X: 780ef f2 0f 06 | clts
X: 78134 f0 0f 06 | lock clts
X: 7837e 0f 07 | sysret
X: 78c9c 0f 08 | invd
X: 79458 2e 0f 08 | invd
X: 794a3 64 0f 08 | invd
X: 796f3 0f 09 | wbinvd
X: 79e20 66 67 0f 09 | wbinvd
DB: 7a427 f3 0f 0a 00 11 22 33 44 | db 0xf3
X: 7a816 0f 0b | ud2
DB: 7b405 66 0f 0c c1 11 22 33 44 | db 0x66
DB: 7b4f6 67 0f 0c 04 25 11 22 33 | db 0x67
DB: 7b929 f0 0f 0c f6 11 22 33 44 | db 0xf0
DB: 7bbeb 0f 0d f6 11 22 33 44 55 | db 0xf
X: 7be26 67 0f 0d 04 | prefetch byte ptr [si]
X: 7c2bc 2e 0f 0d 04 25 11 22 33 44 | prefetch byte ptr cs:[0x44332211]
DB: 7c2ee 64 0f 0d 5d 08 11 22 33 | db 0x64
X: 7c37b 66 67 0f 0d 00 | prefetch byte ptr [bx + si]
DB: 7dd71 f3 0f 10 f0 ff fe ff ff | db 0xf3
DB: 807f4 66 0f 15 e0 ff fe ff ff | db 0x66
DB: 81c5d 67 0f 17 d8 11 22 33 44 | db 0x67
X: 82249 66 0f 18 00 | prefetchnta byte ptr [eax]
X: 828e2 2e 0f 18 1c e5 00 10 00 00 | prefetcht2 byte ptr cs:[0x1000]
DB: 82d2c 66 0f 19 e5 ff fe ff ff | db 0x66
DB: 83883 67 0f 1a f0 ff fe ff ff | db 0x67
X: 845ff 0f 1c 44 24 08 | cldemote byte ptr [esp + 0x8]
X: 84809 66 0f 1c 80 10 20 30 40 | cldemote byte ptr [eax + 0x40302010]
X: 84e0e 2e 0f 1c 05 11 22 33 44 | cldemote byte ptr cs:[0x44332211]
X: 84e74 64 0f 1c 04 25 11 22 33 44 | cldemote byte ptr fs:[0x44332211]
DB: 84eb0 66 67 0f 1c f6 ff fe ff | db 0x66
X: 861d9 0f 1f 40 7f | nop dword ptr [eax + 0x7f]
X: 8632d 0f 1f f8 | nop eax
X: 8697e f0 0f 1f 5d 08 | lock nop dword ptr [ebp + 0x8]
X: 86c20 0f 20 d8 | mov eax, cr3
X: 86c2f 0f 20 e0 | mov eax, cr4
X: 86e76 66 0f 20 ff | mov edi, cr7
X: 87242 f3 0f 20 f8 | mov eax, cr7
X: 87530 0f 21 c1 | mov ecx, dr0
X: 8757b 0f 21 e0 | mov eax, dr4
X: 87900 67 0f 21 c1 | mov ecx, dr0
DB: 87abc f3 0f 21 75 00 ff fe ff | db 0xf3
DB: 87d65 0f 22 00 11 22 33 44 55 | db 0xf
X: 87eaf 0f 22 d3 | mov cr2, ebx
X: 87f27 0f 22 ff | mov cr7, edi
DB: 87f9b 66 0f 22 40 7f 11 22 33 | db 0x66
X: 880f4 66 0f 22 f6 | mov cr6, esi
X: 88292 67 0f 22 d8 | mov cr3, eax
DB: 88322 f3 0f 22 05 11 22 33 44 | db 0xf3
X: 88510 f2 0f 22 e5 | mov cr4, ebp
DB: 8874d 0f 23 0c 8d 10 00 00 00 | db 0xf
X: 8881e 0f 23 e5 | mov dr4, ebp
X: 8885a 0f 23 f8 | mov dr7, eax
X: 88bc4 67 0f 23 d3 | mov dr2, ebx
X: 88e83 f2 0f 23 f6 | mov dr6, esi
X: 88e93 f0 0f 23 f6 | lock mov dr6, esi
DB: 8956f 67 0f 24 f8 ff fe ff ff | db 0x67
DB: 8a8f7 f3 0f 26 1c e5 00 10 00 | db 0xf3
DB: 8c694 f0 0f 29 d8 11 22 33 44 | db 0xf0
X: 9010d 0f 30 | wrmsr
X: 9082f 64 0f 30 | wrmsr
X: 908a8 0f 31 | rdtsc
X: 910a9 f0 0f 31 | lock rdtsc
X: 9112a 2e 0f 31 | rdtsc
X: 9115b 64 0f 31 | rdtsc
X: 9136d 0f 32 | rdmsr
X: 91a0c f0 0f 32 | lock rdmsr
X: 91c7d 0f 33 | rdpmc
X: 9237a f0 0f 33 | lock rdpmc
X: 923be 2e 0f 33 | rdpmc
X: 92400 64 0f 33 | rdpmc
X: 925b8 0f 34 | sysenter
X: 92f6b 0f 35 | sysexit
X: 93057 66 0f 35 | sysexit
X: 9346e f3 0f 35 | sysexit
X: 936b2 64 0f 35 | sysexit
X: 936d9 66 67 0f 35 | sysexit
DB: 937c3 0f 36 0c 8d 10 00 00 00 | db 0xf
DB: 938a3 0f 36 e8 11 22 33 44 55 | db 0xf
X: 940a6 0f 37 | getsec
X: 94407 66 0f 37 | getsec
X: 94829 f2 0f 37 | getsec
X: 948f0 2e 0f 37 | getsec
X: 94921 64 0f 37 | getsec
DB: 94cdd 66 0f 38 d3 11 22 33 44 | db 0x66
DB: 96b7d f3 0f 3b 04 24 11 22 33 | db 0xf3
DB: 970ec 66 0f 3c 04 24 11 22 33 | db 0x66
DB: 98786 f3 0f 3e 44 24 08 11 22 | db 0xf3
DB: 98985 f0 0f 3e 75 00 ff fe ff | db 0xf0
X: 99468 0f 40 40 7f | cmovo eax, dword ptr [eax + 0x7f]
X: 99562 0f 40 d8 | cmovo ebx, eax
X: 99b14 f3 0f 40 d3 | cmovo edx, ebx
X: 99dc9 0f 41 41 80 | cmovno eax, dword ptr [ecx - 0x80]
X: 99eef 0f 41 f0 | cmovno esi, eax
X: 99f3b 66 0f 41 05 ff fe ff ff | cmovno ax, word ptr [0xfffffeff]
X: 9a455 f3 0f 41 d0 | cmovno edx, eax
X: 9a724 0f 42 80 10 20 30 40 | cmovb eax, dword ptr [eax + 0x40302010]
X: 9a7d1 0f 42 c8 | cmovb ecx, eax
X: 9aeb4 f0 0f 42 05 11 22 33 44 | lock cmovb eax, dword ptr [0x44332211]
X: 9b054 0f 43 40 7f | cmovae eax, dword ptr [eax + 0x7f]
X: 9b18a 0f 43 f0 | cmovae esi, eax
X: 9b3e5 67 0f 43 04 | cmovae eax, dword ptr [si]
X: 9b880 2e 0f 43 40 7f | cmovae eax, dword ptr cs:[eax + 0x7f]
X: 9b8e1 66 67 0f 43 75 00 | cmovae si, word ptr [di]
X: 9b93d 0f 44 00 | cmove eax, dword ptr [eax]
X: 9bae1 0f 44 f6 | cmove esi, esi
X: 9c1b1 2e 0f 44 04 25 ff fe ff ff | cmove eax, dword ptr cs:[0xfffffeff]
X: 9c29e 0f 45 05 11 22 33 44 | cmovne eax, dword ptr [0x44332211]
X: 9c406 0f 45 e5 | cmovne esp, ebp
X: 9c7cc 67 0f 45 e0 | cmovne esp, eax
X: 9ca9c f0 0f 45 c1 | lock cmovne eax, ecx
X: 9cacc 2e 0f 45 40 7f | cmovne eax, dword ptr cs:[eax + 0x7f]
X: 9cbb0 66 67 0f 45 d0 | cmovne dx, ax
X: 9cc41 0f 46 41 80 | cmovbe eax, dword ptr [ecx - 0x80]
X: 9cce0 0f 46 c0 | cmovbe eax, eax
X: 9d41a 2e 0f 46 45 fc | cmovbe eax, dword ptr cs:[ebp - 0x4]
X: 9d583 0f 47 41 80 | cmova eax, dword ptr [ecx - 0x80]
X: 9d64f 0f 47 d0 | cmova edx, eax
X: 9dcaf f2 0f 47 41 80 | cmova eax, dword ptr [ecx - 0x80]
X: 9ded0 0f 48 41 80 | cmovs eax, dword ptr [ecx - 0x80]
X: 9e023 0f 48 ff | cmovs edi, edi
X: 9e6b1 2e 0f 48 84 88 00 01 00 00 | cmovs eax, dword ptr cs:[eax + 4*ecx + 0x100]
X: 9e760 66 67 0f 48 34 | cmovs si, word ptr [si]
X: 9e7a5 0f 49 00 | cmovns eax, dword ptr [eax]
X: 9e8c2 0f 49 c1 | cmovns eax, ecx
X: 9ef40 f2 0f 49 00 | cmovns eax, dword ptr [eax]
X: 9efec 2e 0f 49 44 24 08 | cmovns eax, dword ptr cs:[esp + 0x8]
X: 9f193 0f 4a 0c 8d 10 00 00 00 | cmovp ecx, dword ptr [4*ecx + 0x10]
X: 9f1fb 0f 4a c0 | cmovp eax, eax
X: 9f96d 2e 0f 4a 75 00 | cmovp esi, dword ptr cs:[ebp]
X: 9f98f 64 0f 4a c0 | cmovp eax, eax
X: 9fb2d 0f 4b 34 24 | cmovnp esi, dword ptr [esp]
X: 9fb88 0f 4b d8 | cmovnp ebx, eax
X: a011a f3 0f 4b c8 | cmovnp ecx, eax
X: a02ca 64 0f 4b f8 | cmovnp edi, eax
X: a0322 66 67 0f 4b f8 | cmovnp di, ax
X: a03f9 0f 4c 80 10 20 30 40 | cmovl eax, dword ptr [eax + 0x40302010]
X: a053c 0f 4c ff | cmovl edi, edi
X: a0bf4 2e 0f 4c 45 fc | cmovl eax, dword ptr cs:[ebp - 0x4]
X: a0d7a 0f 4d 1c e5 00 10 00 00 | cmovge ebx, dword ptr [0x1000]
X: a0e19 0f 4d d8 | cmovge ebx, eax
X: a1358 f3 0f 4d 5d 08 | cmovge ebx, dword ptr [ebp + 0x8]
X: a145b f2 0f 4d c0 | cmovge eax, eax
X: a1591 64 0f 4d f0 | cmovge esi, eax
X: a15e9 66 67 0f 4d f8 | cmovge di, ax
X: a16c6 0f 4e 1c e5 00 10 00 00 | cmovle ebx, dword ptr [0x1000]
X: a1765 0f 4e d8 | cmovle ebx, eax
X: a191b 66 0f 4e c8 | cmovle cx, ax
X: a1e5c 2e 0f 4e 41 80 | cmovle eax, dword ptr cs:[ecx - 0x80]
X: a1f68 0f 4f 05 ff fe ff ff | cmovg eax, dword ptr [0xfffffeff]
X: a2094 0f 4f d0 | cmovg edx, eax
X: a2755 f0 0f 4f 44 24 08 | lock cmovg eax, dword ptr [esp + 0x8]
DB: a494d 67 0f 53 c0 11 22 33 44 | db 0x67
DB: ab1a8 f3 0f 5e d8 11 22 33 44 | db 0xf3
DB: aeecd 66 0f 65 d3 11 22 33 44 | db 0x66
DB: b2b22 f2 0f 6b 41 80 11 22 33 | db 0xf2
DB: b32b0 f3 0f 6c 40 7f 11 22 33 | db 0xf3
DB: b4e30 f3 0f 6f 05 11 22 33 44 | db 0xf3
DB: b70b0 66 0f 73 c8 ff fe ff ff | db 0x66
DB: ba7c5 66 0f 79 0c 8d 10 00 00 | db 0x66
DB: bbd1f 67 0f 7b e0 ff fe ff ff | db 0x67
X: be80c 0f 80 ff 11 22 33 | jo 0x332dfa11
X: bed15 f3 0f 80 c0 11 22 33 | jo 0x332dfedc
X: bf062 0f 81 45 fc ff fe | jno 0xff0becad
X: bf777 f2 0f 81 f0 ff fe ff | jno 0xaf76e
X: bfa5b 0f 82 e5 11 22 33 | jb 0x332e0c46
X: c01ec 66 67 0f 82 d0 ff | jb 0x1c2
X: c02f0 0f 83 1c e5 00 10 | jae 0x100ce812
X: c0bc4 0f 84 04 24 ff fe | je 0xff0b2fce
X: c13c6 2e 0f 84 1c e5 00 10 | je 0x100cf8e9
X: c1496 66 67 0f 84 c0 11 | je 0x265c
X: c1571 0f 85 0c 8d 10 00 | jne 0x1ca283
X: c1c9b f2 0f 85 41 80 11 22 | jne 0x221d9ce3
X: c1d3a 2e 0f 85 84 88 00 01 | jne 0x10ca5c5
X: c1dda 66 67 0f 85 d0 11 | jne 0x2fb0
X: c1f4a 0f 86 c8 11 22 33 | jbe 0x332e3118
X: c25ce f2 0f 86 e5 ff fe ff | jbe 0xb25ba
X: c275c 66 67 0f 86 e8 ff | jbe 0x274a
X: c28b7 0f 87 d3 11 22 33 | ja 0x332e3a90
X: c307f 66 67 0f 87 c1 11 | ja 0x4246
X: c31de 0f 88 c8 11 22 33 | js 0x332e43ac
X: c34b3 67 0f 88 04 24 11 22 | js 0x221d58be
X: c39b1 66 67 0f 88 ff ff | js 0x39b6
X: c3ab0 0f 89 1c e5 00 10 | jns 0x100d1fd2
X: c3fe5 f3 0f 89 04 24 ff fe | jns 0xff0b63f0
X: c42fd 66 67 0f 89 c0 ff | jns 0x42c3
X: c44b9 0f 8a e5 11 22 33 | jp 0x332e56a4
X: c4b87 2e 0f 8a f6 11 22 33 | jp 0x332e5d84
X: c4bd8 64 0f 8a 44 24 08 ff | jp 0xff147023
X: c4cbf 0f 8b 04 25 11 22 | jnp 0x221d71c9
X: c543c f2 0f 8b ff 11 22 33 | jnp 0x332e6642
X: c573d 0f 8c e0 ff fe ff | jl 0xb5723
X: c60a6 0f 8d e8 11 22 33 | jge 0x332e7294
X: c6543 f3 0f 8d 41 80 11 22 | jge 0x221de58b
X: c68d1 0f 8e 41 80 ff fe | jle 0xff0be918
X: c6dcf 67 0f 8e f0 11 22 33 | jle 0x332e7fc6
X: c704d f0 0f 8e c0 11 22 33 | lock jle 0x332e8214
X: c70d6 2e 0f 8e 04 24 11 22 | jle 0x221d94e1
X: c71c9 0f 8f 04 88 11 22 | jg 0x221df9d3
X: c7b2c 0f 90 04 24 | seto byte ptr [esp]
X: c7c29 0f 90 d0 | seto al
X: c843d 0f 91 05 ff fe ff ff | setno byte ptr [0xfffffeff]
X: c85d2 0f 91 f6 | setno dh
X: c8c2e f0 0f 91 84 88 00 01 00 00 | lock setno byte ptr [eax + 4*ecx + 0x100]
X: c8c78 2e 0f 91 05 11 22 33 44 | setno byte ptr cs:[0x44332211]
X: c8dcc 0f 92 44 24 08 | setb byte ptr [esp + 0x8]
X: c8ef5 0f 92 e5 | setb ch
X: c95e0 2e 0f 92 f8 | setb al
X: c9741 0f 93 80 10 20 30 40 | setae byte ptr [eax + 0x40302010]
X: c9848 0f 93 e8 | setae al
X: ca008 0f 94 00 | sete byte ptr [eax]
X: ca18e 0f 94 e8 | sete al
X: ca889 2e 0f 94 75 00 | sete byte ptr cs:[ebp]
X: ca95b 0f 95 00 | setne byte ptr [eax]
X: caa78 0f 95 c1 | setne cl
X: cb086 f3 0f 95 e0 | setne al
X: cb251 66 67 0f 95 e8 | setne al
X: cb2b7 0f 96 05 ff fe ff ff | setbe byte ptr [0xfffffeff]
X: cb3c5 0f 96 c1 | setbe cl
X: cbaf3 2e 0f 96 00 | setbe byte ptr cs:[eax]
X: cbcab 0f 97 1c e5 00 10 00 00 | seta byte ptr [0x1000]
X: cbd1d 0f 97 c8 | seta al
X: cc17e 67 0f 97 f8 | seta al
X: cc49d 64 0f 97 41 80 | seta byte ptr fs:[ecx - 0x80]
X: cc5c8 0f 98 84 88 00 01 00 00 | sets byte ptr [eax + 4*ecx + 0x100]
X: cc6bc 0f 98 e8 | sets al
X: ccc81 f3 0f 98 e8 | sets al
X: ccdbe 2e 0f 98 c8 | sets al
X: cced7 0f 99 44 24 08 | setns byte ptr [esp + 0x8]
X: ccfb5 0f 99 c8 | setns al
X: cd690 f0 0f 99 84 88 00 01 00 00 | lock setns byte ptr [eax + 4*ecx + 0x100]
X: cd847 0f 9a 41 80 | setp byte ptr [ecx - 0x80]
X: cd99a 0f 9a ff | setp bh
X: cdcc5 67 0f 9a c1 | setp cl
X: ce19d 0f 9b 80 10 20 30 40 | setnp byte ptr [eax + 0x40302010]
X: ce286 0f 9b e0 | setnp al
X: ce9ed 64 0f 9b d0 | setnp al
X: ceac3 0f 9c 40 7f | setl byte ptr [eax + 0x7f]
X: cebae 0f 9c d3 | setl bl
X: cf235 f2 0f 9c e5 | setl ch
X: cf37b 66 67 0f 9c c0 | setl al
X: cf445 0f 9d 84 88 00 01 00 00 | setge byte ptr [eax + 4*ecx + 0x100]
X: cf557 0f 9d f6 | setge dh
X: cfcf5 0f 9e 00 | setle byte ptr [eax]
X: cfe4e 0f 9e d8 | setle al
X: d06e1 0f 9f 0c 8d 10 00 00 00 | setg byte ptr [4*ecx + 0x10]
X: d07fd 0f 9f ff | setg bh
X: d0fbd 0f a0 | push fs
X: d12ce 66 0f a0 | push fs
X: d133e 67 0f a0 | push fs
X: d178e f0 0f a0 | lock push fs
X: d1a80 0f a1 | pop fs
X: d1dba 67 0f a1 | pop fs
X: d2143 2e 0f a1 | pop fs
X: d2201 66 67 0f a1 | pop fs
X: d23a8 0f a2 | cpuid
X: d2bd7 0f a3 80 10 20 30 40 | bt dword ptr [eax + 0x40302010], eax
X: d2ced 0f a3 f0 | bt eax, esi
X: d2d49 66 0f a3 04 88 | bt word ptr [eax + 4*ecx], ax
X: d3454 66 67 0f a3 e5 | bt bp, sp
X: d34f0 0f a4 44 24 08 11 | shld dword ptr [esp + 0x8], eax, 0x11
X: d35ec 0f a4 d3 ff | shld ebx, edx, 0xff
X: d3cba f0 0f a4 84 88 00 01 00 00 ff | lock shld dword ptr [eax + 4*ecx + 0x100], eax, 0xff
X: d3d2b 2e 0f a4 f8 11 | shld eax, edi, 0x11
X: d3ee8 0f a5 75 00 | shld dword ptr [ebp], esi, cl
X: d3f44 0f a5 d3 | shld ebx, edx, cl
X: d5a44 0f a8 | push gs
X: d5d05 66 0f a8 | push gs
X: d5fa3 f3 0f a8 | push gs
X: d6191 f2 0f a8 | push gs
X: d6373 0f a9 | pop gs
X: d6d75 0f aa | rsm
X: d6ff6 66 0f aa | rsm
X: d75eb 0f ab 04 88 | bts dword ptr [eax + 4*ecx], eax
X: d76db 0f ab c0 | bts eax, eax
X: d7fbb 0f ac 0c 8d 10 00 00 00 ff | shrd dword ptr [4*ecx + 0x10], ecx, 0xff
X: d809b 0f ac e8 11 | shrd eax, ebp, 0x11
X: d85f0 f3 0f ac c1 11 | shrd ecx, eax, 0x11
X: d876a 2e 0f ac 5d 08 ff | shrd dword ptr cs:[ebp + 0x8], ebx, 0xff
X: d8939 0f ad 5d 08 | shrd dword ptr [ebp + 0x8], ebx, cl
X: d89ff 0f ad f6 | shrd esi, esi, cl
X: d90fd 64 0f ad e0 | shrd eax, esp, cl
X: d917c 66 67 0f ad f6 | shrd si, si, cl
X: d9334 0f ae f0 | mfence
DB: d996f f2 0f ae 41 80 11 22 33 | db 0xf2
X: d99d2 f0 0f ae 41 80 | lock fxsave [ecx - 0x80]
X: d9b8e 0f af 0c 8d 10 00 00 00 | imul ecx, dword ptr [4*ecx + 0x10]
X: d9c50 0f af e0 | imul esp, eax
X: d9efa 67 0f af 44 24 | imul eax, dword ptr [si + 0x24]
X: da4a4 0f b0 80 10 20 30 40 | cmpxchg byte ptr [eax + 0x40302010], al
X: da5d8 0f b0 f8 | cmpxchg al, bh
X: dac47 f0 0f b0 05 ff fe ff ff | lock cmpxchg byte ptr [0xfffffeff], al
X: dacee 64 0f b0 1c e5 00 10 00 00 | cmpxchg byte ptr fs:[0x1000], bl
X: dad48 66 67 0f b0 f0 | cmpxchg al, dh
X: dae4d 0f b1 5d 08 | cmpxchg dword ptr [ebp + 0x8], ebx
X: daef5 0f b1 e8 | cmpxchg eax, ebp
X: db0de 66 0f b1 e8 | cmpxchg ax, bp
X: db25c 67 0f b1 c1 | cmpxchg ecx, eax
X: db70b 0f b2 40 7f | lss eax, [eax + 0x7f]
DB: dba2b 66 0f b2 f0 11 22 33 44 | db 0x66
X: dc0c2 0f b3 45 fc | btr dword ptr [ebp - 0x4], eax
X: dc14d 0f b3 d8 | btr eax, ebx
X: dc628 f3 0f b3 80 10 20 30 40 | btr dword ptr [eax + 0x40302010], eax
X: dc87b 2e 0f b3 c0 | btr eax, eax
X: dc88b 64 0f b3 f6 | btr esi, esi
X: dc923 66 67 0f b3 1c | btr word ptr [si], bx
X: dca37 0f b4 34 24 | lfs esi, [esp]
X: dcd4b 67 0f b4 44 24 | lfs eax, [si + 0x24]
X: dd20a 64 0f b4 40 7f | lfs eax, fs:[eax + 0x7f]
X: dd2e3 0f b5 40 7f | lgs eax, [eax + 0x7f]
X: dd841 f3 0f b5 05 11 22 33 44 | lgs eax, [0x44332211]
X: ddb3a 64 0f b5 75 00 | lgs esi, fs:[ebp]
X: ddbf7 0f b6 04 25 11 22 33 44 | movzx eax, byte ptr [0x44332211]
X: ddcf5 0f b6 c8 | movzx ecx, al
X: de423 2e 0f b6 45 fc | movzx eax, byte ptr cs:[ebp - 0x4]
X: de5f0 0f b7 5d 08 | movzx ebx, word ptr [ebp + 0x8]
X: de6c5 0f b7 f8 | movzx edi, ax
X: de841 66 0f b7 d3 | movzx dx, bx
X: dea1f 67 0f b7 d0 | movzx edx, ax
X: decbe f2 0f b7 1c e5 00 10 00 00 | movzx ebx, word ptr [0x1000]
X: ded3f f0 0f b7 e5 | lock movzx esp, bp
X: ded6f 2e 0f b7 41 80 | movzx eax, word ptr cs:[ecx - 0x80]
X: df8a4 0f b9 34 24 | ud1 esi, dword ptr [esp]
X: df8e1 0f b9 d0 | ud1 edx, eax
X: e0060 64 0f b9 f0 | ud1 esi, eax
X: e00d9 66 67 0f b9 80 10 20 | ud1 ax, word ptr [bx + si + 0x2010]
X: e0b3b 0f bb 34 24 | btc dword ptr [esp], esi
X: e0bf0 0f bb f8 | btc eax, edi
X: e0ef8 67 0f bb 75 00 | btc dword ptr [di], esi
X: e13f7 0f bc 41 80 | bsf eax, dword ptr [ecx - 0x80]
X: e153b 0f bc f8 | bsf edi, eax
X: e1c01 2e 0f bc 05 11 22 33 44 | bsf eax, dword ptr cs:[0x44332211]
X: e1cd2 0f bd 00 | bsr eax, dword ptr [eax]
X: e1e3a 0f bd e0 | bsr esp, eax
X: e2643 0f be 04 88 | movsx eax, byte ptr [eax + 4*ecx]
X: e27e7 0f be ff | movsx edi, bh
X: e2cdf f3 0f be 34 24 | movsx esi, byte ptr [esp]
X: e2ed1 64 0f be 1c e5 00 10 00 00 | movsx ebx, byte ptr fs:[0x1000]
X: e2f29 66 67 0f be e8 | movsx bp, al
X: e306c 0f bf 34 24 | movsx esi, word ptr [esp]
X: e30e5 0f bf e5 | movsx esp, bp
X: e31b5 66 0f bf 41 80 | movsx ax, word ptr [ecx - 0x80]
X: e38e4 0f c0 04 25 11 22 33 44 | xadd byte ptr [0x44332211], al
X: e39d3 0f c0 c1 | xadd cl, al
X: e42b9 0f c1 1c e5 00 10 00 00 | xadd dword ptr [0x1000], ebx
X: e4394 0f c1 f0 | xadd eax, esi
X: e457e 66 0f c1 f0 | xadd ax, si
X: e479c 67 0f c1 ff | xadd edi, edi
X: e49db f2 0f c1 04 25 11 22 33 44 | xadd dword ptr [0x44332211], eax
DB: e54c3 0f c3 04 24 11 22 33 44 | db 0xf
DB: e779c f3 0f c6 e5 11 22 33 44 | db 0xf3
DB: e78dd 2e 0f c6 e5 ff fe ff ff | db 0x2e
X: e8421 0f c8 | bswap eax
X: e8823 67 0f c8 | bswap eax
X: e8dc2 0f c9 | bswap ecx
X: e916a 67 0f c9 | bswap ecx
X: e94c4 64 0f c9 | bswap ecx
X: e9527 66 67 0f c9 | bswap cx
X: e963a 0f ca | bswap edx
X: e9d5a f0 0f ca | lock bswap edx
X: e9dd2 2e 0f ca | bswap edx
X: e9f47 0f cb | bswap ebx
X: ea7c9 66 67 0f cb | bswap bx
X: ea8a4 0f cc | bswap esp
X: eb0ca 64 0f cc | bswap esp
X: eb212 0f cd | bswap ebp
X: eb931 f0 0f cd | lock bswap ebp
X: ebb2f 0f ce | bswap esi
X: ec29a f0 0f ce | lock bswap esi
X: ec4d5 0f cf | bswap edi
X: ecca4 64 0f cf | bswap edi
DB: edf3b 64 0f d1 44 24 08 11 22 | db 0x64
DB: eeca9 66 0f d3 ff ff fe ff ff | db 0x66
DB: f73e9 64 0f e1 ff 11 22 33 44 | db 0x64
DB: fa8d2 f3 0f e7 04 25 ff fe ff | db 0xf3
DB: ff8dd 66 0f f0 80 10 20 30 40 | db 0x66
DB: 102979 67 0f f5 0c 8d 10 00 00 | db 0x67
DB: 106859 66 0f fc 40 7f ff fe ff | db 0x66
DB: 1073e0 67 0f fd 84 88 00 01 00 | db 0x67
DB: 108586 66 0f ff e8 11 22 33 44 | db 0x66
X: 108be0 0f 0d 01 | prefetch byte ptr [ecx]
X: 108ebf 0f c7 09 | cmpxchg8b qword ptr [ecx]
X: 109081 0f 18 0d 11 22 33 44 | prefetcht0 byte ptr [0x44332211]
X: 1092bb 0f 01 13 | lgdtd [ebx]
X: 1092ca 0f ae 13 | ldmxcsr dword ptr [ebx]
X: 10958b 0f 00 1a | ltr word ptr [edx]
X: 109711 0f 18 1d 11 22 33 44 | prefetcht2 byte ptr [0x44332211]
X: 109900 0f c7 22 | xsavec [edx]
X: 109a86 0f 01 26 | smsw word ptr [esi]
X: 109bd0 0f ae 29 | xrstor [ecx]
X: 109d92 0f ba 2d 11 22 33 44 55 | bts dword ptr [0x44332211], 0x55
X: 109f9f 0f ba 32 11 | btr dword ptr [edx], 0x11
X: 10a2d8 0f c7 3a | vmptrst qword ptr [edx]
X: 10a4c7 0f 01 3f | invlpg byte ptr [edi]
X: 10aa0d 0f 00 4c 11 22 | str word ptr [ecx + edx + 0x22]
X: 10aa2b 0f ae 4c 11 22 | fxrstor [ecx + edx + 0x22]
X: 10ab39 0f 0d 4e 11 | prefetchw byte ptr [esi + 0x11]
X: 10ac1a 0f 00 51 11 | lldt word ptr [ecx + 0x11]
X: 10ae72 0f 18 56 11 | prefetcht1 byte ptr [esi + 0x11]
X: 10b061 0f c7 5b 11 | xrstors [ebx + 0x11]
X: 10b2c8 0f ae 61 11 | xsave [ecx + 0x11]
X: 10b3b8 0f ba 63 11 22 | bt dword ptr [ebx + 0x11], 0x22
X: 10b688 0f c7 6a 11 | xsaves [edx + 0x11]
DB: 10b8b3 0f 18 6f 11 22 33 44 55 | db 0xf
X: 10bafc 0f ae 75 11 | xsaveopt [ebp + 0x11]
X: 10bb74 0f c7 76 11 | vmptrld qword ptr [esi + 0x11]
DB: 10be26 0f 00 7d 11 22 33 44 55 | db 0xf
X: 10becb 0f ba 7e 11 22 | btc dword ptr [esi + 0x11], 0x22
X: 10c051 0f ae 82 11 22 33 44 | fxsave [edx + 0x44332211]
X: 10c0e7 0f 18 83 11 22 33 44 | prefetchnta byte ptr [ebx + 0x44332211]
X: 10c105 0f 00 84 11 22 33 44 55 | sldt word ptr [ecx + edx + 0x55443322]
X: 10c597 0f 01 8f 11 22 33 44 | sidtd [edi + 0x44332211]
X: 10c92a 0f 0d 97 11 22 33 44 | prefetchwt1 byte ptr [edi + 0x44332211]
X: 10c9c0 0f ae 99 11 22 33 44 | stmxcsr dword ptr [ecx + 0x44332211]
X: 10cd35 0f 18 a1 11 22 33 44 | prefetchrst2 byte ptr [ecx + 0x44332211]
X: 10ce25 0f 00 a4 11 22 33 44 55 | verr word ptr [ecx + edx + 0x55443322]
X: 10d16d 0f 00 ac 11 22 33 44 55 | verw word ptr [ecx + edx + 0x55443322]
X: 10d8ed 0f ae be 11 22 33 44 | clflush byte ptr [esi + 0x44332211]
X: 10d9ec 0f 01 c2 | vmlaunch
X: 10da55 0f 01 c3 | vmresume
X: 10dabe 0f 01 c4 | vmxoff
X: 10db27 0f 01 c5 | pconfig
X: 10db90 0f 01 c6 | wrmsrns
X: 10dbea 0f 00 c7 | sldt edi
X: 10dc80 0f 01 c9 | mwait
X: 10dce9 0f 01 ca | clac
X: 10dd52 0f 01 cb | stac
X: 10ddac 0f 00 cc | str esp
X: 10def6 0f 01 cf | encls
X: 10df8c 0f 01 d1 | xsetbv
X: 10e09a 0f 01 d4 | vmfunc
X: 10e0f4 0f 00 d5 | lldt bp
X: 10e103 0f 01 d5 | xend
X: 10e16c 0f 01 d6 | xtest
X: 10e1d5 0f 01 d7 | enclu
X: 10e25c 0f 01 d9 | vmmcall
X: 10e2c5 0f 01 da | vmload
X: 10e32e 0f 01 db | vmsave
X: 10e397 0f 01 dc | stgi
X: 10e400 0f 01 dd | clgi
X: 10e469 0f 01 de | skinit
X: 10e4c3 0f 00 df | ltr di
X: 10e4d2 0f 01 df | invlpga
X: 10e53b 0f 00 e1 | verr cx
X: 10e61c 0f 01 e3 | smsw ebx
X: 10e7b1 0f ba e7 11 | bt edi, 0x11
X: 10e8bf 0f ba ea 11 | bts edx, 0x11
X: 10e955 0f 00 ec | verw sp
X: 10e973 0f ae ec | lfence
X: 10ea36 0f 01 ee | rdpkru
X: 10ea9f 0f 01 ef | wrpkru
X: 10ec34 0f ba f3 11 | btr ebx, 0x11
X: 10ed42 0f 01 f7 | lmsw di
X: 10ed60 0f c7 f7 | rdrand edi
X: 10edab 0f 01 f8 | swapgs
X: 10edd8 0f 01 f9 | rdtscp
X: 10ede7 0f ae f9 | sfence
X: 10ee41 0f 01 fa | monitorx
X: 10ee5f 0f c7 fa | rdseed edx
X: 10ee6e 0f ba fa 11 | btc edx, 0x11
X: 10eeaa 0f 01 fb | mwaitx
X: 10ef13 0f 01 fc | clzero
X: 10ef7c 0f 01 fd | rdpru
DB: 10efc7 0f 0d fd 11 22 33 44 55 | db 0xf
X: 10efe5 0f 01 fe | invlpgb
X: 10f13e 80 02 11 | add byte ptr [edx], 0x11
X: 10f14c c7 02 11 22 33 44 | mov dword ptr [edx], 0x44332211
X: 10f21e ff 04 11 | inc dword ptr [ecx + edx]
X: 10f248 c7 04 11 22 33 44 55 | mov dword ptr [ecx + edx], 0x55443322
X: 10f8ae 80 12 11 | adc byte ptr [edx], 0x11
X: 10f910 ff 13 | call dword ptr [ebx]
X: 10fafa f7 17 | not dword ptr [edi]
X: 10fb78 f7 18 | neg dword ptr [eax]
X: 10fbbe d8 18 | esc 0x3, word ptr [eax]
X: 10ff06 8d 1f | lea ebx, [edi]
X: 11016e ff 24 11 | jmp dword ptr [ecx + edx]
X: 110622 dd 2d 11 22 33 44 | esc 0x2d, word ptr [0x44332211]
X: 110916 dd 33 | esc 0x2e, word ptr [ebx]
X: 110a90 dd 36 | esc 0x2e, word ptr [esi]
X: 110b62 80 38 11 | cmp byte ptr [eax], 0x11
X: 110eaa f7 3f | idiv dword ptr [edi]
X: 111024 f7 42 11 22 33 44 55 | test dword ptr [edx + 0x11], 0x55443322
X: 111a88 ff 57 11 | call dword ptr [edi + 0x11]
X: 111ce2 89 5c 11 22 | mov dword ptr [ecx + edx + 0x22], ebx
X: 112054 89 63 11 | mov dword ptr [ebx + 0x11], esp
X: 1121ea ff 66 11 | jmp dword ptr [esi + 0x11]
X: 1123e2 ff 6a 11 | ljmp [edx + 0x11]
X: 112452 f7 6b 11 | imul dword ptr [ebx + 0x11]
X: 112498 d8 6b 11 | esc 0x5, word ptr [ebx + 0x11]
X: 11279a dd 71 11 | esc 0x2e, word ptr [ecx + 0x11]
X: 11294c ff 75 11 | push dword ptr [ebp + 0x11]
X: 1131c6 80 86 11 22 33 44 55 | add byte ptr [esi + 0x44332211], 0x55
DB: 113316 f7 89 11 22 33 44 55 66 | db 0xf7
X: 11343c 80 8b 11 22 33 44 55 | or byte ptr [ebx + 0x44332211], 0x55
X: 113848 d8 93 11 22 33 44 | esc 0x2, word ptr [ebx + 0x44332211]
X: 113a16 8d 97 11 22 33 44 | lea edx, [edi + 0x44332211]
X: 113b82 ff 9a 11 22 33 44 | lcall [edx + 0x44332211]
X: 113c9a 80 9c 11 22 33 44 55 66 | sbb byte ptr [ecx + edx + 0x55443322], 0x66
DB: 113e22 c7 9f 11 22 33 44 55 66 | db 0xc7
X: 11415c f7 a6 11 22 33 44 | mul dword ptr [esi + 0x44332211]
X: 114186 80 a6 11 22 33 44 55 | and byte ptr [esi + 0x44332211], 0x55
X: 11437e 80 aa 11 22 33 44 55 | sub byte ptr [edx + 0x44332211], 0x55
X: 1146d4 ff b1 11 22 33 44 | push dword ptr [ecx + 0x44332211]
X: 114966 80 b6 11 22 33 44 55 | xor byte ptr [esi + 0x44332211], 0x55
X: 1149ba f7 b7 11 22 33 44 | div dword ptr [edi + 0x44332211]
DB: 114e98 8d c2 11 22 33 44 55 66 | db 0x8d
X: 114fb0 c7 c4 11 22 33 44 | mov esp, 0x44332211
X: 115004 ff c5 | inc ebp
X: 11509e 80 c6 11 | add dh, 0x11
X: 1152dc 8b cc | mov ecx, esp
X: 115306 ff cc | dec esp
X: 1153a0 80 cd 11 | or ch, 0x11
X: 1155d0 80 d2 11 | adc dl, 0x11
X: 1156be f7 d5 | not ebp
X: 1156cc ff d5 | call ebp
X: 11596c f7 db | neg ebx
X: 115b10 80 de 11 | sbb dh, 0x11
X: 115c8a ff e2 | jmp edx
X: 115cfa f7 e3 | mul ebx
X: 115e9e ff e7 | jmp edi
X: 115eba 80 e7 11 | and bh, 0x11
X: 115f8c 89 ea | mov edx, ebp
X: 11613e 80 ed 11 | sub ch, 0x11
X: 116210 f7 ef | imul edi
X: 1162b8 f7 f1 | div ecx
X: 1162c6 ff f1 | push ecx
X: 1162e2 80 f1 11 | xor cl, 0x11
DB: 11646a c7 f4 11 22 33 44 55 66 | db 0xc7
X: 1164be ff f5 | push ebp
X: 1165ba f7 f8 | idiv eax
X: 1166a8 80 fa 11 | cmp dl, 0x11
X: 1167dc 8b fd | mov edi, ebp
X: 11894b 8b 44 b8 11 | mov eax, dword ptr [eax + 4*edi + 0x11]
X: 119305 8b 84 ef 11 22 33 44 | mov eax, dword ptr [edi + 8*ebp + 0x44332211]

X86-16
S" disx32" .RESULTS
