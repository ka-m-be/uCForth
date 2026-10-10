\ disx64.fs - tests of the package DISX86 (asm/monitor/disx86.fs) with 64-bit code
\
\ Load (in the folder ucf), after the package FILES and the tester:
\   build/ucf-c -32 host/files.fs forth/test/tester.fs asm/monitor/test/disx64.fs
\ The tests need cells of 32 bits.
\
\ Each line X: is one instruction: its address, its bytes, "|", and the
\ text that DECODE must give; the instruction must also end after its
\ bytes. A line DB: has bytes that start no instruction: DECODE gives
\ "db" and the first byte, and goes on after it. The texts come from
\ llvm-objdump 21 (Apple LLVM, the triple x86_64-pc-none-elf, "-M intel"), once,
\ and are kept here as data. The bytes: each opcode of one byte and
\ after 0Fh with 29 kinds of ModR/M, SIB and displacement, each without
\ a prefix and with the prefixes 66h, 67h, F2h, F3h, F0h, 2Eh and 64h,
\ and with REX; each byte ModR/M with some opcodes, each SIB with mod 0,
\ 1 and 2:
\ 133 465 instructions, checked once against llvm-objdump without an open
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
DECIMAL  X86-64

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

X: 001c 00 04 88 | add byte ptr [rax + 4*rcx], al
X: 00d1 00 5d 08 | add byte ptr [rbp + 0x8], bl
X: 0144 00 d8 | add al, bl
X: 0198 00 f8 | add al, bh
X: 0746 66 67 00 5d 08 | add byte ptr [ebp + 0x8], bl
X: 0ff8 01 04 88 | add dword ptr [rax + 4*rcx], eax
X: 108b 01 1c e5 00 10 00 00 | add dword ptr [0x1000], ebx
X: 1112 01 d3 | add ebx, edx
X: 1158 01 f0 | add eax, esi
X: 15dd f2 01 c8 | repne add eax, ecx
X: 162c f0 01 f0 | lock add eax, esi
X: 19ef 44 01 41 80 | add dword ptr [rcx - 0x80], r8d
X: 1d51 42 01 00 | add dword ptr [rax], eax
X: 203a 02 84 88 00 01 00 00 | add al, byte ptr [rax + 4*rcx + 0x100]
X: 2060 02 1c e5 00 10 00 00 | add bl, byte ptr [0x1000]
X: 20d9 02 d0 | add dl, al
X: 211f 02 e8 | add ch, al
X: 2783 40 02 45 fc | add al, byte ptr [rbp - 0x4]
X: 2bb4 49 02 04 24 | add al, byte ptr [r12]
X: 2fdd 03 44 24 08 | add eax, dword ptr [rsp + 0x8]
X: 3083 03 34 24 | add esi, dword ptr [rsp]
X: 30a0 03 c1 | add eax, ecx
X: 312c 03 f8 | add edi, eax
X: 3753 40 03 44 24 08 | add eax, dword ptr [rsp + 0x8]
X: 3fcf 04 41 | add al, 0x41
X: 40ab 04 d8 | add al, 0xd8
X: 4fb6 05 41 80 11 22 | add eax, 0x22118041
X: 50d8 05 f6 11 22 33 | add eax, 0x332211f6
X: 5194 66 05 84 88 | add ax, 0x8884
X: 5556 f2 05 34 24 11 22 | repne add eax, 0x22112434
X: 55fc 2e 05 75 00 11 22 | add eax, 0x22110075
X: 5746 40 05 5d 08 ff fe | add eax, 0xfeff085d
DB: 60c4 06 e8 ff fe ff ff 80 00 | db 0x6
DB: 658c f0 06 d0 ff fe ff ff 80 | db 0xf0
DB: 6b2d 48 06 f8 ff fe ff ff 80 | db 0x48
DB: 749a f3 07 f6 11 22 33 44 55 | db 0xf3
DB: 7691 66 67 07 f0 11 22 33 44 | db 0x66
X: 7f72 08 04 24 | or byte ptr [rsp], al
X: 807c 08 d8 | or al, bl
X: 868e 66 67 08 0c 8d 10 00 00 00 | or byte ptr [4*ecx + 0x10], cl
X: 8741 40 08 d3 | or bl, dl
X: 8f77 09 40 7f | or dword ptr [rax + 0x7f], eax
X: 908c 09 e8 | or eax, ebp
X: 91d0 66 09 34 24 | or word ptr [rsp], si
X: 9725 41 09 05 11 22 33 44 | or dword ptr [rip + 0x44332211], eax # 0x4433b93d
X: 9d1f 66 48 09 c8 | or rax, rcx
X: 9f91 0a 84 88 00 01 00 00 | or al, byte ptr [rax + 4*rcx + 0x100]
X: a068 0a e5 | or ah, ch
X: ac1e 4d 0a 04 25 11 22 33 44 | or r8b, byte ptr [0x44332211]
X: afa8 0b 45 fc | or eax, dword ptr [rbp - 0x4]
X: b054 0b e8 | or ebp, eax
X: b41e 67 0b f8 | or edi, eax
X: b489 f3 0b 04 25 11 22 33 44 | rep or eax, dword ptr [0x44332211]
X: b5cc 64 0b 84 88 00 01 00 00 | or eax, dword ptr fs:[rax + 4*rcx + 0x100]
X: bbc5 4d 0b d0 | or r10, r8
X: c045 0c f0 | or al, 0xf0
X: c8d0 44 0c c8 | or al, 0xc8
X: cd03 66 41 0c c8 | or al, 0xc8
X: cd9c 67 48 0c d3 | or al, 0xd3
X: ce40 f0 48 0c 0c | lock or al, 0xc
X: d020 0d f6 ff fe ff | or eax, 0xfffefff6
X: d3ec f3 0d 80 10 20 30 | rep or eax, 0x30201080
X: d459 f2 0d 1c e5 00 10 | repne or eax, 0x1000e51c
X: ddc8 f3 48 0d d0 11 22 33 | rep or rax, 0x332211d0
DB: e152 66 0e c1 11 22 33 44 55 | db 0x66
X: eef4 10 1c e5 00 10 00 00 | adc byte ptr [0x1000], bl
X: efa5 10 e5 | adc ch, ah
X: f058 66 10 40 7f | adc byte ptr [rax + 0x7f], al
X: f284 67 10 1c e5 00 10 00 00 | adc byte ptr [0x1000], bl
X: f41b f2 10 e5 | repne adc ch, ah
X: fbae 4f 10 84 88 00 01 00 00 | adc byte ptr [r8 + 4*r9 + 0x100], r8b
X: fcd3 66 41 10 1c e5 00 10 00 00 | adc byte ptr [0x1000], bl
X: fd90 f3 48 10 f6 | rep adc sil, sil
X: fe65 11 04 24 | adc dword ptr [rsp], eax
X: ffd1 11 ff | adc edi, edi
X: ffdf 66 11 00 | adc word ptr [rax], ax
X: 103d3 f3 11 c0 | rep adc eax, eax
X: 10c58 66 48 11 41 80 | adc qword ptr [rcx - 0x80], rax
X: 10e58 12 40 7f | adc al, byte ptr [rax + 0x7f]
X: 10fa5 12 ff | adc bh, bh
X: 11e0d 13 04 24 | adc eax, dword ptr [rsp]
X: 11f25 13 e0 | adc esp, eax
X: 12eda 14 d3 | adc al, 0xd3
X: 134e6 66 67 14 f8 | adc al, 0xf8
X: 1389c 48 14 45 | adc al, 0x45
X: 13bc7 66 48 14 c8 | adc al, 0xc8
X: 13c62 67 48 14 05 | adc al, 0x5
X: 13e52 15 75 00 ff fe | adc eax, 0xfeff0075
X: 14352 f2 15 d0 11 22 33 | repne adc eax, 0x332211d0
X: 14471 64 15 f6 11 22 33 | adc eax, 0x332211f6
X: 1449f 66 67 15 ff ff | adc ax, 0xffff
X: 16d5b 18 41 80 | sbb byte ptr [rcx - 0x80], al
X: 16e37 18 d8 | sbb al, bl
X: 16fb5 66 18 c0 | sbb al, al
X: 17706 44 18 04 88 | sbb byte ptr [rax + 4*rcx], r8b
X: 17965 4c 18 80 10 20 30 40 | sbb byte ptr [rax + 0x40302010], r8b
X: 17b00 66 48 18 04 25 11 22 33 44 | sbb byte ptr [0x44332211], al
X: 17b87 66 41 18 f8 | sbb r8b, dil
X: 17d22 19 40 7f | sbb dword ptr [rax + 0x7f], eax
X: 17df1 19 d0 | sbb eax, edx
X: 180f4 67 19 0c 8d 10 00 00 00 | sbb dword ptr [4*ecx + 0x10], ecx
X: 18cc4 1a 04 25 11 22 33 44 | sbb al, byte ptr [0x44332211]
X: 18ddd 1a d8 | sbb bl, al
X: 18fb5 66 1a e0 | sbb ah, al
X: 19665 44 1a d8 | sbb r11b, al
X: 198de 49 1a 04 24 | sbb al, byte ptr [r12]
X: 19d21 1b 1c e5 00 10 00 00 | sbb ebx, dword ptr [0x1000]
X: 19d7e 1b c1 | sbb eax, ecx
X: 1a302 2e 1b d3 | sbb edx, ebx
X: 1ab71 f3 48 1b 44 24 08 | rep sbb rax, qword ptr [rsp + 0x8]
X: 1ace0 1c 1c | sbb al, 0x1c
X: 1afa7 66 1c ff | sbb al, 0xff
X: 1b269 f0 1c 5d | lock sbb al, 0x5d
X: 1b48b 41 1c 40 | sbb al, 0x40
X: 1bb24 67 48 1c 04 | sbb al, 0x4
X: 1bd77 1d f0 ff fe ff | sbb eax, 0xfffefff0
X: 1bf8f 67 1d 05 11 22 33 | sbb eax, 0x33221105
X: 1c34f 66 67 1d 44 24 | sbb ax, 0x2444
X: 1c9bd 42 1d 05 ff fe ff | sbb eax, 0xfffeff05
X: 1cb37 f3 48 1d f0 11 22 33 | rep sbb rax, 0x332211f0
DB: 1e7c7 49 1f 44 24 08 ff fe ff | db 0x49
X: 1ebea 20 41 80 | and byte ptr [rcx - 0x80], al
X: 1ed28 20 ff | and bh, bh
X: 1f255 64 20 34 24 | and byte ptr fs:[rsp], dh
X: 1fba2 21 04 25 ff fe ff ff | and dword ptr [0xfffffeff], eax
X: 1fcc9 21 e0 | and eax, esp
X: 1ff8e 67 21 84 88 00 01 00 00 | and dword ptr [eax + 4*ecx + 0x100], eax
X: 208db 4f 21 e8 | and r8, r13
X: 20909 42 21 80 10 20 30 40 | and dword ptr [rax + 0x40302010], eax
X: 20993 66 48 21 f6 | and rsi, rsi
X: 209c3 66 41 21 04 88 | and word ptr [r8 + 4*rcx], ax
X: 20c3e 22 75 00 | and dh, byte ptr [rbp]
X: 20c5c 22 c0 | and al, al
X: 21105 f3 22 45 fc | rep and al, byte ptr [rbp - 0x4]
X: 21344 40 22 75 00 | and sil, byte ptr [rbp]
X: 2176c 49 22 44 24 08 | and al, byte ptr [r12 + 0x8]
X: 21be4 23 0c 8d 10 00 00 00 | and ecx, dword ptr [4*rcx + 0x10]
X: 21cd2 23 f6 | and esi, esi
X: 22794 49 23 00 | and rax, qword ptr [r8]
X: 22af3 f0 48 23 f0 | lock and rsi, rax
X: 22bc0 24 0c | and al, 0xc
X: 23501 44 24 d0 | and al, 0xd0
X: 23a50 f3 48 24 1c | rep and al, 0x1c
X: 23c54 25 d8 ff fe ff | and eax, 0xfffeffd8
X: 23d56 66 25 84 88 | and ax, 0x8884
X: 24308 41 25 00 11 22 33 | and eax, 0x33221100
X: 2496c 66 41 25 34 24 | and ax, 0x2434
DB: 24b90 27 1c e5 00 10 00 00 11 | db 0x27
DB: 25077 f3 27 c0 11 22 33 44 55 | db 0xf3
X: 25b2a 28 40 7f | sub byte ptr [rax + 0x7f], al
X: 25c69 28 f8 | sub al, bh
X: 25d53 66 28 45 fc | sub byte ptr [rbp - 0x4], al
X: 26637 48 28 c8 | sub al, cl
X: 2693c 66 41 28 e8 | sub r8b, bpl
X: 26b5b 29 84 88 00 01 00 00 | sub dword ptr [rax + 4*rcx + 0x100], eax
X: 26c08 29 d3 | sub ebx, edx
X: 27ac6 2a 05 11 22 33 44 | sub al, byte ptr [rip + 0x44332211] # 0x44359cdd
X: 27c0a 2a e0 | sub ah, al
X: 28835 42 2a f6 | sub sil, sil
X: 28ac0 2b 04 25 ff fe ff ff | sub eax, dword ptr [0xfffffeff]
X: 28be7 2b e0 | sub esp, eax
X: 28cb8 66 2b 41 80 | sub ax, word ptr [rcx - 0x80]
X: 29b64 2c c0 | sub al, 0xc0
X: 29f53 67 2c e0 | sub al, 0xe0
X: 2ab65 2d c8 11 22 33 | sub eax, 0x332211c8
X: 2b054 f2 2d ff ff fe ff | repne sub eax, 0xfffeffff
X: 2b6dd 4c 2d c1 11 22 33 | sub rax, 0x332211c1
DB: 2c975 f3 48 2f c8 11 22 33 44 | db 0xf3
X: 2cac9 30 0c 8d 10 00 00 00 | xor byte ptr [4*rcx + 0x10], cl
X: 2cb7f 30 e0 | xor al, ah
X: 2cc87 66 30 0c 8d 10 00 00 00 | xor byte ptr [4*rcx + 0x10], cl
X: 2d132 64 30 0c 8d 10 00 00 00 | xor byte ptr fs:[4*rcx + 0x10], cl
X: 2d657 49 30 c8 | xor r8b, cl
X: 2d91c 67 48 30 d8 | xor al, bl
X: 2da86 31 41 80 | xor dword ptr [rcx - 0x80], eax
X: 2db8c 31 e8 | xor eax, ebp
X: 2dcc0 66 31 75 00 | xor word ptr [rbp], si
X: 2e639 49 31 d3 | xor r11, rdx
X: 2eaa0 32 1c e5 00 10 00 00 | xor bl, byte ptr [0x1000]
X: 2eb0b 32 c8 | xor cl, al
X: 2f0e4 64 32 05 ff fe ff ff | xor al, byte ptr fs:[rip - 0x101]
X: 2f3ad 41 32 ff | xor dil, r15b
X: 2f667 4c 32 44 24 08 | xor r8b, byte ptr [rsp + 0x8]
X: 2fa42 33 80 10 20 30 40 | xor eax, dword ptr [rax + 0x40302010]
X: 2fb01 33 d3 | xor edx, ebx
X: 30179 40 33 45 fc | xor eax, dword ptr [rbp - 0x4]
X: 303d5 44 33 1c e5 00 10 00 00 | xor r11d, dword ptr [0x1000]
X: 30733 4f 33 04 88 | xor r8, qword ptr [r8 + 4*r9]
X: 30ab8 34 c1 | xor al, 0xc1
X: 3111e 66 67 34 75 | xor al, 0x75
X: 3198b 35 00 11 22 33 | xor eax, 0x33221100
X: 322f4 41 35 e5 ff fe ff | xor eax, 0xfffeffe5
X: 3260f 4c 35 04 25 ff fe | xor rax, -0x100dafc
X: 339bb 38 41 80 | cmp byte ptr [rcx - 0x80], al
X: 33ac1 38 e8 | cmp al, ch
X: 33ee7 f3 38 0c 8d 10 00 00 00 | rep cmp byte ptr [4*rcx + 0x10], cl
X: 34976 39 44 24 08 | cmp dword ptr [rsp + 0x8], eax
X: 34a2b 39 c0 | cmp eax, eax
X: 34f70 f0 39 c8 | lock cmp eax, ecx
X: 359b4 3a 1c e5 00 10 00 00 | cmp bl, byte ptr [0x1000]
X: 35a81 3a f0 | cmp dh, al
X: 36173 41 3a 80 10 20 30 40 | cmp al, byte ptr [r8 + 0x40302010]
X: 363b8 48 3a 84 88 00 01 00 00 | cmp al, byte ptr [rax + 4*rcx + 0x100]
X: 368d4 3b 00 | cmp eax, dword ptr [rax]
X: 36a34 3b e5 | cmp esp, ebp
X: 36ae7 66 3b 40 7f | cmp ax, word ptr [rax + 0x7f]
X: 36f1a f0 3b f0 | lock cmp esi, eax
X: 376b6 66 48 3b ff | cmp rdi, rdi
X: 37a0f 3c e5 | cmp al, 0xe5
X: 385a1 4d 3c 40 | cmp al, 0x40
X: 38690 66 48 3c 45 | cmp al, 0x45
X: 38a2b 3d ff 11 22 33 | cmp eax, 0x332211ff
X: 39584 4d 3d d8 ff fe ff | cmp rax, -0x10028
X: 39859 f0 48 3d e5 11 22 33 | lock cmp rax, 0x332211e5
DB: 3a597 4f 3f 04 24 ff fe ff ff | db 0x4f
X: 3a991 50 | push rax
X: 3af19 2e 50 | push rax
X: 3b238 44 50 | push rax
X: 3b901 51 | push rcx
X: 3c4da 4c 51 | push rcx
X: 3c860 52 | push rdx
X: 3cb9b 67 52 | push rdx
X: 3d1f5 44 52 | push rdx
X: 3d319 48 52 | push rdx
X: 3d80b 53 | push rbx
X: 3dfc3 40 53 | push rbx
X: 3e638 66 41 53 | push r11w
X: 3e78c f0 48 53 | lock push rbx
X: 3e7f9 54 | push rsp
X: 3ee22 f0 54 | lock push rsp
X: 3f266 48 54 | push rsp
X: 3f508 4f 54 | push r12
X: 3f7fb 55 | push rbp
X: 3fd89 f2 55 | repne push rbp
X: 400e8 41 55 | push r13
X: 404af 4d 55 | push r13
X: 4050c 4f 55 | push r13
X: 407ae 56 | push rsi
X: 40b37 67 56 | push rsi
X: 41185 44 56 | push rsi
X: 4142f 4c 56 | push rsi
X: 41807 57 | push rdi
X: 41b97 67 57 | push rdi
X: 41ded 2e 57 | push rdi
X: 4278f 58 | pop rax
X: 42cf7 f2 58 | repne pop rax
X: 42d58 f0 58 | lock pop rax
X: 42e24 64 58 | pop rax
X: 431c3 48 58 | pop rax
X: 43367 49 58 | pop r8
X: 437ef 59 | pop rcx
X: 445a8 66 41 59 | pop r9w
X: 447b1 5a | pop rdx
X: 448a3 66 5a | pop dx
X: 44c54 f3 5a | rep pop rdx
X: 44d32 f0 5a | lock pop rdx
X: 453e3 4d 5a | pop r10
X: 4552c 66 41 5a | pop r10w
X: 4582d 5b | pop rbx
X: 45a53 67 5b | pop rbx
X: 45d89 2e 5b | pop rbx
X: 462f1 49 5b | pop r11
X: 46333 4c 5b | pop rbx
X: 466a1 5c | pop rsp
X: 47437 42 5c | pop rsp
X: 47595 67 48 5c | pop rsp
X: 47710 5d | pop rbp
X: 48058 44 5d | pop rbp
X: 486a2 5e | pop rsi
X: 48adc 67 5e | pop rsi
X: 48bbd f3 5e | rep pop rsi
X: 48f42 41 5e | pop r14
X: 49283 49 5e | pop r14
X: 49430 66 48 5e | pop rsi
X: 494ac 66 41 5e | pop r14w
X: 49790 5f | pop rdi
X: 49b8c f3 5f | rep pop rdi
X: 49c52 f0 5f | lock pop rdi
X: 4a57b f3 48 5f | rep pop rdi
DB: 4ab45 67 60 ff ff fe ff ff 80 | db 0x67
X: 4d666 63 45 fc | movsxd eax, dword ptr [rbp - 0x4]
X: 4d6a2 63 c0 | movsxd eax, eax
X: 4d9a8 67 63 80 10 20 30 40 | movsxd eax, dword ptr [eax + 0x40302010]
X: 4e2ad 4d 63 40 7f | movsxd r8, dword ptr [r8 + 0x7f]
X: 4e353 42 63 04 88 | movsxd eax, dword ptr [rax + 4*r9]
X: 4e658 68 75 00 11 22 | push 0x22110075
X: 4e6f4 68 f0 11 22 33 | push 0x332211f0
X: 4ea83 67 68 e8 ff fe ff | push -0x10018
X: 4ebc0 f0 68 04 24 11 22 | lock push 0x22112404
X: 4f4fc f0 48 68 ff ff fe ff | lock push -0x10001
X: 4f5fd 69 0c 8d 10 00 00 00 11 22 33 44 | imul ecx, dword ptr [4*rcx + 0x10], 0x44332211
X: 4f6dd 69 f0 ff fe ff ff | imul esi, eax, 0xfffffeff
X: 4fb6b f2 69 40 7f ff fe ff ff | repne imul eax, dword ptr [rax + 0x7f], 0xfffffeff
X: 4fd25 40 69 44 24 08 ff fe ff ff | imul eax, dword ptr [rsp + 0x8], 0xfffffeff
X: 501e2 4c 69 0c 8d 10 00 00 00 11 22 33 44 | imul r9, qword ptr [4*rcx + 0x10], 0x44332211
X: 5048f f3 48 69 75 00 11 22 33 44 | rep imul rsi, qword ptr [rbp], 0x44332211
X: 5066b 6a c1 | push -0x3f
X: 50705 6a ff | push -0x1
X: 50a4c 67 6a e0 | push -0x20
X: 5120a 4c 6a 75 | push 0x75
X: 51391 66 48 6a f0 | push -0x10
X: 513e3 66 41 6a 04 | push 0x4
X: 51571 6b 04 24 ff | imul eax, dword ptr [rsp], -0x1
X: 516b3 6b f0 11 | imul esi, eax, 0x11
X: 51f3d 44 6b f6 11 | imul r14d, esi, 0x11
X: 52465 f3 48 6b d3 ff | rep imul rdx, rbx, -0x1
X: 52552 6c | insb byte ptr es:[rdi], dx
X: 532b7 42 6c | insb byte ptr es:[rdi], dx
X: 53356 66 41 6c | insb byte ptr es:[rdi], dx
X: 53565 6d | insd dword ptr es:[rdi], dx
X: 5393c 67 6d | insd dword ptr es:[edi], dx
X: 541b2 4c 6d | insd dword ptr es:[rdi], dx
X: 5455e 6e | outsb dx, byte ptr [rsi]
X: 54c56 66 67 6e | outsb dx, byte ptr [esi]
X: 55131 4c 6e | outsb dx, byte ptr [rsi]
X: 555cb 6f | outsd dx, dword ptr [rsi]
X: 556b1 66 6f | outsw dx, word ptr [rsi]
X: 55bfe 66 67 6f | outsw dx, word ptr [esi]
X: 562d4 66 48 6f | outsd dx, dword ptr [rsi]
X: 5647e f0 48 6f | lock outsd dx, dword ptr [rsi]
X: 564ac 70 05 | jo 0x564b3
X: 56a9a f2 70 34 | repne jo 0x56ad1
X: 56aeb f0 70 75 | lock jo 0x56b63
X: 56bc0 64 70 d3 | jo 0x56b96
X: 572ab 66 48 70 00 | jo 0x572af
X: 574a2 71 05 | jno 0x574a9
X: 581ad 4d 71 45 | jno 0x581f5
X: 5842c f0 48 71 00 | lock jno 0x58430
X: 584db 72 40 | jb 0x5851d
X: 591eb 42 72 05 | jb 0x591f3
X: 5933e 67 48 72 45 | jb 0x59387
X: 595d8 73 f6 | jae 0x595d0
X: 59f31 48 73 1c | jae 0x59f50
X: 5a527 74 34 | je 0x5a55d
X: 5a8bb 67 74 34 | je 0x5a8f2
X: 5af0d 48 74 0c | je 0x5af1c
X: 5b5c2 75 ff | jne 0x5b5c3
X: 5b8dc 67 75 d0 | jne 0x5b8af
X: 5be08 44 75 00 | jne 0x5be0b
X: 5c14e 4f 75 75 | jne 0x5c1c6
X: 5c433 76 04 | jbe 0x5c439
X: 5d291 66 41 76 44 | jbe 0x5d2d9
X: 5d4e1 77 c0 | ja 0x5d4a3
X: 5d939 f3 77 d8 | rep ja 0x5d914
X: 5dc55 41 77 41 | ja 0x5dc99
X: 5dde5 44 77 44 | ja 0x5de2c
X: 5e3ac f0 48 77 80 | lock ja 0x5e330
X: 5e569 78 f8 | js 0x5e563
X: 5f03b 4c 78 40 | js 0x5f07e
X: 5f1f3 66 48 78 c0 | js 0x5f1b7
X: 5f4ff 79 e5 | jns 0x5f4e6
X: 6021e 66 41 79 1c | jns 0x6023e
X: 603f2 7a 41 | jp 0x60435
X: 60921 f3 7a 00 | rep jp 0x60924
X: 6099d f0 7a 80 | lock jp 0x60920
X: 60a1e 2e 7a e0 | jp 0x60a01
X: 60a8d 64 7a 04 | jp 0x60a94
X: 611c5 66 41 7a 40 | jp 0x61209
X: 61472 7b d0 | jnp 0x61444
X: 61966 f0 7b 84 | lock jnp 0x618ed
X: 6203a 4d 7b 44 | jnp 0x62081
X: 624d9 7c ff | jl 0x624da
X: 62a52 64 7c e5 | jl 0x62a3a
X: 62d54 44 7c 80 | jl 0x62cd7
X: 630d1 42 7c c0 | jl 0x63094
X: 63439 7d d0 | jge 0x6340b
X: 640fa 66 48 7d 04 | jge 0x64102
X: 6448a 7e ff | jle 0x6448b
X: 64ff2 4d 7e d0 | jle 0x64fc5
X: 652b2 f0 48 7e e0 | lock jle 0x65296
X: 65394 7f 45 | jg 0x653db
X: 657fb 67 7f f6 | jg 0x657f4
X: 65ec3 49 7f d0 | jg 0x65e96
X: 662be 80 04 25 ff fe ff ff 80 | add byte ptr [0xfffffeff], 0x80
X: 66391 80 c0 11 | add al, 0x11
X: 663d7 80 d8 11 | sbb al, 0x11
X: 66439 80 ff 11 | cmp bh, 0x11
X: 66627 67 80 05 11 22 33 44 55 | add byte ptr [eip + 0x44332211], 0x55
X: 66f12 4c 80 5d 08 11 | sbb byte ptr [rbp + 0x8], 0x11
X: 67273 81 00 11 22 33 44 | add dword ptr [rax], 0x44332211
X: 6730f 81 0c 8d 10 00 00 00 11 22 33 44 | or dword ptr [4*rcx + 0x10], 0x44332211
X: 67335 81 45 fc 11 22 33 44 | add dword ptr [rbp - 0x4], 0x44332211
X: 67344 81 5d 08 11 22 33 44 | sbb dword ptr [rbp + 0x8], 0x44332211
X: 67362 81 34 24 11 22 33 44 | xor dword ptr [rsp], 0x44332211
X: 67371 81 c0 11 22 33 44 | add eax, 0x44332211
X: 6737f 81 c1 11 22 33 44 | add ecx, 0x44332211
X: 6738d 81 c8 ff fe ff ff | or eax, 0xfffffeff
X: 673a9 81 d3 ff fe ff ff | adc ebx, 0xfffffeff
X: 673b7 81 d8 11 22 33 44 | sbb eax, 0x44332211
X: 673c5 81 e0 11 22 33 44 | and eax, 0x44332211
X: 673e1 81 e8 ff fe ff ff | sub eax, 0xfffffeff
X: 673ef 81 f0 11 22 33 44 | xor eax, 0x44332211
X: 67419 81 ff 11 22 33 44 | cmp edi, 0x44332211
X: 6776f 67 81 e5 11 22 33 44 | and ebp, 0x44332211
X: 677c9 f3 81 f0 11 22 33 44 | rep xor eax, 0x44332211
X: 67938 2e 81 ff 11 22 33 44 | cmp edi, 0x44332211
X: 68031 42 81 45 fc 11 22 33 44 | add dword ptr [rbp - 0x4], 0x44332211
X: 680ce 66 41 81 e0 ff fe | and r8w, 0xfeff
X: 681a9 f3 48 81 34 24 11 22 33 44 | rep xor qword ptr [rsp], 0x44332211
X: 692a1 83 41 80 11 | add dword ptr [rcx - 0x80], 0x11
X: 692d5 83 0c 8d 10 00 00 00 11 | or dword ptr [4*rcx + 0x10], 0x11
X: 692fb 83 45 fc 11 | add dword ptr [rbp - 0x4], 0x11
X: 6930a 83 5d 08 ff | sbb dword ptr [rbp + 0x8], -0x1
X: 69328 83 34 24 11 | xor dword ptr [rsp], 0x11
X: 69337 83 c0 11 | add eax, 0x11
X: 69345 83 c1 11 | add ecx, 0x11
X: 69353 83 c8 ff | or eax, -0x1
X: 6936f 83 d3 11 | adc ebx, 0x11
X: 6937d 83 d8 ff | sbb eax, -0x1
X: 6938b 83 e0 ff | and eax, -0x1
X: 693a7 83 e8 ff | sub eax, -0x1
X: 693c3 83 f6 11 | xor esi, 0x11
X: 693df 83 ff 11 | cmp edi, 0x11
X: 6989a f0 83 d3 11 | lock adc ebx, 0x11
X: 699bf 66 67 83 04 88 11 | add word ptr [eax + 4*ecx], 0x11
X: 6a1b2 f0 48 83 d0 11 | lock adc rax, 0x11
X: 6a2f8 84 34 24 | test byte ptr [rsp], dh
X: 6a3af 84 ff | test bh, bh
X: 6ada7 48 84 e0 | test al, spl
X: 6ae33 49 84 d0 | test r8b, dl
X: 6b26f 85 80 10 20 30 40 | test dword ptr [rax + 0x40302010], eax
X: 6b366 85 e8 | test eax, ebp
X: 6bbff 44 85 84 88 00 01 00 00 | test dword ptr [rax + 4*rcx + 0x100], r8d
X: 6be6f 4c 85 e0 | test rax, r12
X: 6c1b8 f0 48 85 f6 | lock test rsi, rsi
X: 6c2bd 86 75 00 | xchg byte ptr [rbp], dh
X: 6c2f7 86 c8 | xchg al, cl
X: 6c921 66 67 86 d8 | xchg al, bl
X: 6cd22 48 86 c1 | xchg cl, al
X: 6cde5 49 86 04 24 | xchg byte ptr [r12], al
X: 6d195 f0 48 86 04 25 ff fe ff ff | lock xchg byte ptr [0xfffffeff], al
X: 6d205 87 04 24 | xchg dword ptr [rsp], eax
X: 6d2e5 87 c8 | xchg eax, ecx
X: 6d95b 66 67 87 f0 | xchg ax, si
X: 6db5e 41 87 f0 | xchg r8d, esi
X: 6de60 4c 87 0c 8d 10 00 00 00 | xchg qword ptr [4*rcx + 0x10], r9
X: 6e1b0 88 00 | mov byte ptr [rax], al
X: 6e25f 88 1c e5 00 10 00 00 | mov byte ptr [0x1000], bl
X: 6e2d8 88 d0 | mov al, dl
X: 6e310 88 e5 | mov ch, ah
X: 6e87e 2e 88 c1 | mov cl, al
X: 6e900 66 67 88 05 ff fe ff ff | mov byte ptr [eip - 0x101], al
X: 6f1c8 89 04 25 ff fe ff ff | mov dword ptr [0xfffffeff], eax
X: 6f66c 67 89 d3 | mov ebx, edx
X: 701ec 8a 80 10 20 30 40 | mov al, byte ptr [rax + 0x40302010]
X: 70237 8a 45 fc | mov al, byte ptr [rbp - 0x4]
X: 70281 8a c1 | mov al, cl
X: 702ff 8a f6 | mov dh, dh
X: 70427 66 8a 34 24 | mov dh, byte ptr [rsp]
X: 704fa 67 8a 00 | mov al, byte ptr [eax]
X: 70798 f0 8a 04 88 | lock mov al, byte ptr [rax + 4*rcx]
X: 70e44 4d 8a c1 | mov r8b, r9b
X: 70f4c 66 48 8a 05 11 22 33 44 | mov al, byte ptr [rip + 0x44332211] # 0x443a3165
X: 71882 64 8b c0 | mov eax, eax
X: 7216f 8c 04 24 | mov word ptr [rsp], es
X: 721d1 8c 0c 8d 10 00 00 00 | mov word ptr [4*rcx + 0x10], cs
X: 7225d 8c d0 | mov eax, ss
X: 72279 8c d8 | mov eax, ds
X: 72574 67 8c 1c e5 00 10 00 00 | mov word ptr [0x1000], ds
X: 72a94 41 8c d8 | mov r8d, ds
X: 72d6b 49 8c 0c 8d 10 00 00 00 | mov word ptr [4*rcx + 0x10], cs
X: 7318d 8d 80 10 20 30 40 | lea eax, [rax + 0x40302010]
X: 73803 64 8d 34 24 | lea esi, fs:[rsp]
DB: 73ab9 41 8d f8 ff fe ff ff 80 | db 0x41
X: 73b16 44 8d 5d 08 | lea r11d, [rbp + 0x8]
X: 74184 8e 0c 8d 10 00 00 00 | mov cs, word ptr [4*rcx + 0x10]
X: 74197 8e 1c e5 00 10 00 00 | mov ds, word ptr [0x1000]
X: 741e6 8e c0 | mov es, eax
X: 7421e 8e d3 | mov ss, ebx
X: 74d02 49 8e 04 25 ff fe ff ff | mov es, word ptr [0xfffffeff]
X: 75024 f3 48 8e 1c e5 00 10 00 00 | rep mov ds, word ptr [0x1000]
X: 750ea 8f 04 25 11 22 33 44 | pop qword ptr [0x44332211]
X: 751cb 8f c1 | pop rcx
X: 7565d f3 8f 05 11 22 33 44 | rep pop qword ptr [rip + 0x44332211] # 0x443a7875
DB: 7572a f0 8f 34 24 11 22 33 44 | db 0xf0
X: 7574a 2e 8f 40 7f | pop qword ptr cs:[rax + 0x7f]
X: 75d2f 4c 8f 04 88 | pop qword ptr [rax + 4*rcx]
X: 75e4b 42 8f 04 88 | pop qword ptr [rax + 4*r9]
X: 75e9d 66 48 8f 04 25 11 22 33 44 | pop qword ptr [0x44332211]
X: 761a6 90 | nop
X: 76ec8 66 48 90 | nop
X: 76eec 66 41 90 | xchg ax, r8w
X: 76fc4 f3 48 90 | xchg rax, rax
X: 77026 f0 48 90 | lock nop
X: 7721f 91 | xchg eax, ecx
X: 77a60 44 91 | xchg eax, ecx
X: 77d2e 4c 91 | xchg rax, rcx
X: 781fd 92 | xchg eax, edx
X: 785d8 f3 92 | rep xchg eax, edx
X: 78736 2e 92 | xchg eax, edx
X: 78e62 66 48 92 | xchg rax, rdx
X: 78f66 67 48 92 | xchg rax, rdx
X: 79017 f0 48 92 | lock xchg rax, rdx
X: 791aa 93 | xchg eax, ebx
X: 7966b f0 93 | lock xchg eax, ebx
X: 79c9a 49 93 | xchg rax, r11
X: 79fbd f0 48 93 | lock xchg rax, rbx
X: 7a0ae 94 | xchg eax, esp
X: 7a217 66 94 | xchg ax, sp
X: 7ad9b 42 94 | xchg eax, esp
X: 7ae97 66 41 94 | xchg ax, r12w
X: 7b166 95 | xchg eax, ebp
X: 7bf67 f3 48 95 | rep xchg rax, rbp
X: 7c0c4 96 | xchg eax, esi
X: 7cf21 f3 48 96 | rep xchg rax, rsi
X: 7d0eb 97 | xchg eax, edi
X: 7d756 66 67 97 | xchg ax, di
X: 7d9fb 44 97 | xchg eax, edi
X: 7dcbd 4d 97 | xchg rax, r15
X: 7dd2f 4f 97 | xchg rax, r15
X: 7df48 f3 48 97 | rep xchg rax, rdi
X: 7e0d6 98 | cwde
X: 7edf2 66 48 98 | cdqe
X: 7efef 99 | cdq
X: 7f33a 67 99 | cdq
X: 7ff1b f3 48 99 | rep cqo
DB: 80314 67 9a 00 11 22 33 44 55 | db 0x67
X: 81083 9b | wait
X: 8161c 2e 9b | wait
X: 81be4 4c 9b | wait
X: 81d5b 66 48 9b | wait
X: 81f98 9c | pushfq
X: 82561 f0 9c | lock pushfq
X: 826d4 40 9c | pushfq
X: 82fd0 9d | popfq
X: 836e0 40 9d | popfq
X: 83721 41 9d | popfq
X: 83fa9 9e | sahf
X: 846a1 40 9e | sahf
X: 84b79 4c 9e | sahf
X: 84d67 66 41 9e | sahf
X: 84da7 67 48 9e | sahf
X: 84edf 9f | lahf
X: 855a4 64 9f | lahf
X: 85604 66 67 9f | lahf
X: 858c9 44 9f | lahf
X: 85d0a 66 48 9f | lahf
X: 85d1b 66 41 9f | lahf
X: 85e6f f0 48 9f | lock lahf
X: 85f92 a0 75 00 11 22 33 44 55 66 | movabs al, byte ptr [0x6655443322110075]
X: 8639f 67 a0 e0 11 22 33 | mov al, byte ptr [0x332211e0]
X: 865d2 64 a0 34 24 ff fe ff ff 80 00 | movabs al, byte ptr fs:[0x80fffffeff2434]
X: 86b4c 4c a0 d8 ff fe ff ff 80 00 00 | movabs al, byte ptr [0x80fffffeffd8]
X: 87021 a1 f8 11 22 33 44 55 66 77 | movabs eax, dword ptr [0x77665544332211f8]
X: 87544 2e a1 41 80 11 22 33 44 55 66 | movabs eax, dword ptr cs:[0x6655443322118041]
X: 878af 44 a1 5d 08 11 22 33 44 55 66 | movabs eax, dword ptr [0x665544332211085d]
X: 87bdb 4f a1 80 10 20 30 40 ff fe ff | movabs rax, qword ptr [0xfffeff4030201080]
X: 87cba 66 48 a1 75 00 11 22 33 44 55 66 | movabs rax, qword ptr [0x6655443322110075]
X: 87d8f f3 48 a1 f0 11 22 33 44 55 66 77 | rep movabs rax, qword ptr [0x77665544332211f0]
X: 87df2 f0 48 a1 d0 ff fe ff ff 80 00 00 | lock movabs rax, qword ptr [0x80fffffeffd0]
X: 87e98 a2 04 24 11 22 33 44 55 66 | movabs byte ptr [0x6655443322112404], al
X: 8862c 40 a2 c8 11 22 33 44 55 66 77 | movabs byte ptr [0x77665544332211c8], al
X: 8883a 44 a2 f8 11 22 33 44 55 66 77 | movabs byte ptr [0x77665544332211f8], al
X: 88c85 66 48 a2 ff ff fe ff ff 80 00 00 | movabs byte ptr [0x80fffffeffff], al
X: 88f3c a3 c1 ff fe ff ff 80 00 00 | movabs dword ptr [0x80fffffeffc1], eax
X: 892a3 67 a3 75 00 ff fe | mov dword ptr [0xfeff0075], eax
X: 89699 41 a3 41 80 11 22 33 44 55 66 | movabs dword ptr [0x6655443322118041], eax
X: 8984b 44 a3 41 80 11 22 33 44 55 66 | movabs dword ptr [0x6655443322118041], eax
X: 89a7d 49 a3 e8 ff fe ff ff 80 00 00 | movabs qword ptr [0x80fffffeffe8], rax
X: 89be9 42 a3 e0 11 22 33 44 55 66 77 | movabs dword ptr [0x77665544332211e0], eax
X: 89c1e 66 48 a3 f8 11 22 33 44 55 66 77 | movabs qword ptr [0x77665544332211f8], rax
X: 89ec2 a4 | movsb byte ptr es:[rdi], byte ptr [rsi]
X: 8a441 f0 a4 | lock movsb byte ptr es:[rdi], byte ptr [rsi]
X: 8aa8c 4c a4 | movsb byte ptr es:[rdi], byte ptr [rsi]
X: 8ab3b 4f a4 | movsb byte ptr es:[rdi], byte ptr [rsi]
X: 8aba0 42 a4 | movsb byte ptr es:[rdi], byte ptr [rsi]
X: 8acd0 67 48 a4 | movsb byte ptr es:[edi], byte ptr [esi]
X: 8ae18 a5 | movsd dword ptr es:[rdi], dword ptr [rsi]
X: 8b1e2 67 a5 | movsd dword ptr es:[edi], dword ptr [esi]
X: 8b3a1 f2 a5 | repne movsd dword ptr es:[rdi], dword ptr [rsi]
X: 8bc9b 67 48 a5 | movsq qword ptr es:[edi], qword ptr [esi]
X: 8bdac f0 48 a5 | lock movsq qword ptr es:[rdi], qword ptr [rsi]
X: 8bf46 a6 | cmpsb byte ptr [rsi], byte ptr es:[rdi]
X: 8c1a0 67 a6 | cmpsb byte ptr [esi], byte ptr es:[edi]
X: 8c4d0 64 a6 | cmpsb byte ptr fs:[rsi], byte ptr es:[rdi]
X: 8ccbe f3 48 a6 | rep cmpsb byte ptr [rsi], byte ptr es:[rdi]
X: 8cd2a f0 48 a6 | lock cmpsb byte ptr [rsi], byte ptr es:[rdi]
X: 8ce08 a7 | cmpsd dword ptr [rsi], dword ptr es:[rdi]
X: 8d6a6 41 a7 | cmpsd dword ptr [rsi], dword ptr es:[rdi]
X: 8dd72 a8 05 | test al, 0x5
X: 8ed86 a9 04 24 ff fe | test eax, 0xfeff2404
X: 8f52f 40 a9 ff 11 22 33 | test eax, 0x332211ff
X: 8f741 44 a9 d3 11 22 33 | test eax, 0x332211d3
X: 8f941 49 a9 5d 08 ff fe | test rax, -0x100f7a3
X: 8fb7f 66 48 a9 d0 11 22 33 | test rax, 0x332211d0
X: 8fdc9 aa | stosb byte ptr es:[rdi], al
X: 9044c 64 aa | stosb byte ptr es:[rdi], al
X: 90b48 66 48 aa | stosb byte ptr es:[rdi], al
X: 90d25 ab | stosd dword ptr es:[rdi], eax
X: 91070 66 ab | stosw word ptr es:[rdi], ax
X: 9115c 67 ab | stosd dword ptr es:[edi], eax
X: 91c96 f0 48 ab | lock stosq qword ptr es:[rdi], rax
X: 91d21 ac | lodsb al, byte ptr [rsi]
X: 921c5 67 ac | lodsb al, byte ptr [esi]
X: 923a6 2e ac | lodsb al, byte ptr cs:[rsi]
X: 9259c 41 ac | lodsb al, byte ptr [rsi]
X: 92915 49 ac | lodsb al, byte ptr [rsi]
X: 92b60 66 41 ac | lodsb al, byte ptr [rsi]
X: 92c10 f3 48 ac | rep lodsb al, byte ptr [rsi]
X: 92e75 ad | lodsd eax, dword ptr [rsi]
X: 93b77 66 41 ad | lodsw ax, word ptr [rsi]
X: 93c1f f3 48 ad | rep lodsq rax, qword ptr [rsi]
X: 93ccc ae | scasb al, byte ptr es:[rdi]
X: 9445c 40 ae | scasb al, byte ptr es:[rdi]
X: 94921 4c ae | scasb al, byte ptr es:[rdi]
X: 94a45 42 ae | scasb al, byte ptr es:[rdi]
X: 94df0 af | scasd eax, dword ptr es:[rdi]
X: 9521c f3 af | rep scasd eax, dword ptr es:[rdi]
X: 953f5 66 67 af | scasw ax, word ptr es:[edi]
X: 9545a 40 af | scasd eax, dword ptr es:[rdi]
X: 95962 4d af | scasq rax, qword ptr es:[rdi]
X: 95daf b0 d3 | mov al, 0xd3
X: 95e11 b0 f8 | mov al, 0xf8
X: 9645a 40 b0 c0 | mov al, 0xc0
X: 96c7a b1 04 | mov cl, 0x4
X: 96d5c b1 c0 | mov cl, 0xc0
X: 97100 67 b1 c1 | mov cl, 0xc1
X: 9744a 40 b1 e8 | mov cl, 0xe8
X: 9760c 41 b1 f8 | mov r9b, 0xf8
X: 978af 49 b1 04 | mov r9b, 0x4
X: 9795c 4d b1 75 | mov r9b, 0x75
X: 97c7a b2 04 | mov dl, 0x4
X: 97d85 b2 d3 | mov dl, 0xd3
X: 98262 f0 b2 f0 | lock mov dl, 0xf0
X: 983b3 66 67 b2 0c | mov dl, 0xc
X: 9876a 48 b2 75 | mov dl, 0x75
X: 9884d 49 b2 45 | mov r10b, 0x45
X: 98c6c b3 04 | mov bl, 0x4
X: 98d76 b3 d8 | mov bl, 0xd8
X: 98ee4 66 b3 34 | mov bl, 0x34
X: 992ac 2e b3 d0 | mov bl, 0xd0
X: 9992d 4d b3 f0 | mov r11b, 0xf0
X: 99c6d b4 44 | mov ah, 0x44
X: 99cd3 b4 1c | mov ah, 0x1c
X: 9a018 67 b4 41 | mov ah, 0x41
X: 9a189 f3 b4 c0 | rep mov ah, 0xc0
X: 9a405 40 b4 c0 | mov spl, 0xc0
X: 9ac85 b5 84 | mov ch, 0x84
X: 9acfa b5 c0 | mov ch, 0xc0
X: 9b2e5 64 b5 45 | mov ch, 0x45
X: 9ba17 66 48 b5 0c | mov bpl, 0xc
X: 9baa6 67 48 b5 40 | mov bpl, 0x40
X: 9bca3 b6 5d | mov dh, 0x5d
X: 9bcd0 b6 c0 | mov dh, 0xc0
X: 9c65d 48 b6 40 | mov sil, 0x40
X: 9cafc f3 48 b6 00 | rep mov sil, 0x0
X: 9cbcf b7 04 | mov bh, 0x4
X: 9cc66 b7 45 | mov bh, 0x45
X: 9d185 f2 b7 84 | repne mov bh, 0x84
X: 9d570 44 b7 f8 | mov dil, 0xf8
X: 9d9df 66 48 b7 f8 | mov dil, 0xf8
X: 9dc29 b8 0c 8d 10 00 | mov eax, 0x108d0c
X: 9dc5e b8 5d 08 11 22 | mov eax, 0x2211085d
X: 9ec70 b9 34 24 11 22 | mov ecx, 0x22112434
X: 9ed0b b9 f6 11 22 33 | mov ecx, 0x332211f6
X: 9fafa f0 48 b9 44 24 08 11 22 33 44 55 | lock movabs rcx, 0x5544332211082444
X: 9fbf9 ba 0c 8d 10 00 | mov edx, 0x108d0c
X: 9fccb ba e8 11 22 33 | mov edx, 0x332211e8
X: a0819 4d ba 40 7f 11 22 33 44 55 66 | movabs r10, 0x6655443322117f40
X: a08ed 42 ba e8 11 22 33 | mov edx, 0x332211e8
X: a0be9 bb 45 fc 11 22 | mov ebx, 0x2211fc45
X: a0c5d bb d3 11 22 33 | mov ebx, 0x332211d3
X: a12eb 40 bb 04 88 11 22 | mov ebx, 0x22118804
X: a15ad 48 bb 44 24 08 11 22 33 44 55 | movabs rbx, 0x5544332211082444
X: a1b6f bc 41 80 ff fe | mov esp, 0xfeff8041
X: a1bf6 bc 34 24 ff fe | mov esp, 0xfeff2434
X: a1d09 66 bc 44 24 | mov sp, 0x2444
X: a21b8 2e bc 34 24 11 22 | mov esp, 0x22112434
X: a2af1 bd 05 ff fe ff | mov ebp, 0xfffeff05
X: a2aff bd 04 88 11 22 | mov ebp, 0x22118804
X: a3748 4c bd 44 24 08 11 22 33 44 55 | movabs rbp, 0x5544332211082444
X: a38e3 66 48 bd f0 ff fe ff ff 80 00 00 | movabs rbp, 0x80fffffefff0
X: a3aac f0 48 bd f6 11 22 33 44 55 66 77 | lock movabs rbp, 0x77665544332211f6
X: a3bab be 34 24 11 22 | mov esi, 0x22112434
X: a3c1c be e5 ff fe ff | mov esi, 0xfffeffe5
X: a49b7 67 48 be 05 11 22 33 44 55 66 77 | movabs rsi, 0x7766554433221105
X: a4ade bf 04 25 11 22 | mov edi, 0x22112504
X: a4be9 bf d3 11 22 33 | mov edi, 0x332211d3
X: a5750 4c bf c8 ff fe ff ff 80 00 00 | movabs rdi, 0x80fffffeffc8
X: a586e 42 bf 80 10 20 30 | mov edi, 0x30201080
X: a59e9 f3 48 bf 05 11 22 33 44 55 66 77 | rep movabs rdi, 0x7766554433221105
X: a5a9e c0 00 11 | rol byte ptr [rax], 0x11
X: a5b3a c0 0c 8d 10 00 00 00 ff | ror byte ptr [4*rcx + 0x10], 0xff
X: a5b6f c0 5d 08 11 | rcr byte ptr [rbp + 0x8], 0x11
X: a5b9c c0 c0 ff | rol al, 0xff
X: a5bb8 c0 c8 11 | ror al, 0x11
X: a5bd4 c0 d3 ff | rcl bl, 0xff
X: a5be2 c0 d8 ff | rcr al, 0xff
X: a5bfe c0 e5 ff | shl ch, 0xff
X: a5c0c c0 e8 11 | shr al, 0x11
X: a5c44 c0 ff 11 | sar bh, 0x11
X: a6910 66 41 c0 e8 11 | shr r8b, 0x11
X: a6a08 f3 48 c0 40 7f ff | rep rol byte ptr [rax + 0x7f], 0xff
X: a6aad c1 04 88 ff | rol dword ptr [rax + 4*rcx], 0xff
X: a6b2d c1 0c 8d 10 00 00 00 11 | ror dword ptr [4*rcx + 0x10], 0x11
X: a6b62 c1 5d 08 ff | rcr dword ptr [rbp + 0x8], 0xff
X: a6b9d c1 c1 11 | rol ecx, 0x11
X: a6bab c1 c8 11 | ror eax, 0x11
X: a6bc7 c1 d3 ff | rcl ebx, 0xff
X: a6bd5 c1 d8 11 | rcr eax, 0x11
X: a6bf1 c1 e5 11 | shl ebp, 0x11
X: a6bff c1 e8 11 | shr eax, 0x11
X: a6c37 c1 ff 11 | sar edi, 0x11
X: a6fe7 f3 c1 45 fc 11 | rep rol dword ptr [rbp - 0x4], 0x11
X: a708c f2 c1 04 24 ff | repne rol dword ptr [rsp], 0xff
X: a7787 4d c1 04 88 ff | rol qword ptr [r8 + 4*rcx], 0xff
X: a7ab5 c2 44 24 | ret 0x2444
X: a81c5 66 67 c2 05 11 | ret 0x1105
X: a871e 4c c2 d0 11 | ret 0x11d0
X: a897d f3 48 c2 05 11 | rep ret 0x1105
X: a8b73 c3 | ret
X: a964e 49 c3 | ret
X: a96d0 4c c3 | ret
X: a976f 4f c3 | ret
X: a98fe 67 48 c3 | ret
X: aba03 c6 04 88 11 | mov byte ptr [rax + 4*rcx], 0x11
X: aba21 c6 04 24 11 | mov byte ptr [rsp], 0x11
X: abae5 c6 c0 11 | mov al, 0x11
X: abaf3 c6 c1 11 | mov cl, 0x11
X: abb7f c6 f8 ff | xabort 0xff
X: abf70 f3 c6 44 24 08 11 | rep mov byte ptr [rsp + 0x8], 0x11
X: ac030 f0 c6 c0 ff | lock mov al, 0xff
X: ac5f5 49 c6 04 24 11 | mov byte ptr [r12], 0x11
X: ac78b 42 c6 40 7f 11 | mov byte ptr [rax + 0x7f], 0x11
X: ac7ed 66 48 c6 c0 ff | mov al, 0xff
X: acae6 c7 c1 11 22 33 44 | mov ecx, 0x44332211
X: acb72 c7 f8 ff fe ff ff | xbegin 0xaca77
X: acbed 66 c7 40 7f ff fe | mov word ptr [rax + 0x7f], 0xfeff
X: ad858 66 41 c7 00 11 22 | mov word ptr [r8], 0x2211
X: ada68 c8 1c e5 00 | enter 0xe51c, 0x0
X: adfe6 f0 c8 75 00 11 | lock enter 0x75, 0x11
X: ae38a 44 c8 00 11 22 | enter 0x1100, 0x22
X: ae6cb 4d c8 f0 11 22 | enter 0x11f0, 0x22
X: ae970 f0 48 c8 f0 11 22 | lock enter 0x11f0, 0x22
X: aea2e c9 | leave
X: af6a2 4f c9 | leave
X: af95a f0 48 c9 | lock leave
X: afaa0 ca d3 11 | retf 0x11d3
X: b0038 2e ca 41 80 | retf 0x8041
X: b03ed 48 ca 40 7f | retfq 0x7f40
X: b057d 49 ca c0 ff | retfq 0xffc0
X: b09c4 cb | retf
X: b0ea3 f3 cb | rep retf
X: b1005 2e cb | retf
X: b15c1 4c cb | retfq
X: b16f1 42 cb | retf
X: b1882 f3 48 cb | rep retfq
X: b19e9 cc | int3
X: b1fcd 2e cc | int3
X: b2557 49 cc | int3
X: b2953 cd 04 | int 0x4
X: b2ccc 67 cd 04 | int 0x4
X: b3066 66 67 cd 05 | int 0x5
X: b33a4 48 cd 40 | int 0x40
X: b386d f3 48 cd 04 | rep int 0x4
DB: b39e0 ce 34 24 11 22 33 44 55 | db 0xce
X: b49ff cf | iretd
X: b5676 42 cf | iretd
X: b56dc 66 48 cf | iretq
X: b5837 f3 48 cf | rep iretq
X: b58de d0 05 11 22 33 44 | rol byte ptr [rip + 0x44332211], 1 # 0x443e7af5
X: b596c d0 0c 8d 10 00 00 00 | ror byte ptr [4*rcx + 0x10], 1
X: b597f d0 1c e5 00 10 00 00 | rcr byte ptr [0x1000], 1
X: b59ce d0 c0 | rol al, 1
X: b59ea d0 c8 | ror al, 1
X: b5a06 d0 d3 | rcl bl, 1
X: b5a14 d0 d8 | rcr al, 1
X: b5a30 d0 e5 | shl ch, 1
X: b5a3e d0 e8 | shr al, 1
X: b5a68 d0 f8 | sar al, 1
X: b5faa 64 d0 45 fc | rol byte ptr fs:[rbp - 0x4], 1
X: b67b0 67 48 d0 d0 | rcl al, 1
X: b6834 f3 48 d0 e5 | rep shl bpl, 1
X: b6932 d1 84 88 00 01 00 00 | rol dword ptr [rax + 4*rcx + 0x100], 1
X: b6945 d1 0c 8d 10 00 00 00 | ror dword ptr [4*rcx + 0x10], 1
X: b6958 d1 1c e5 00 10 00 00 | rcr dword ptr [0x1000], 1
X: b69b5 d1 c1 | rol ecx, 1
X: b69c3 d1 c8 | ror eax, 1
X: b69d1 d1 d0 | rcl eax, 1
X: b69ed d1 d8 | rcr eax, 1
X: b69fb d1 e0 | shl eax, 1
X: b6a17 d1 e8 | shr eax, 1
X: b6a4f d1 ff | sar edi, 1
X: b6ba7 66 d1 d3 | rcl bx, 1
DB: b6dc3 67 d1 f0 ff fe ff ff 80 | db 0x67
X: b706d 40 d1 c1 | rol ecx, 1
X: b779b 67 48 d1 e8 | shr rax, 1
X: b77f1 f3 48 d1 c8 | rep ror rax, 1
X: b7901 d2 40 7f | rol byte ptr [rax + 0x7f], cl
X: b7944 d2 0c 8d 10 00 00 00 | ror byte ptr [4*rcx + 0x10], cl
X: b7957 d2 1c e5 00 10 00 00 | rcr byte ptr [0x1000], cl
X: b79b4 d2 c1 | rol cl, cl
X: b79c2 d2 c8 | ror al, cl
X: b79de d2 d3 | rcl bl, cl
X: b79ec d2 d8 | rcr al, cl
X: b79fa d2 e0 | shl al, cl
X: b7a16 d2 e8 | shr al, cl
X: b7a40 d2 f8 | sar al, cl
X: b7dfe f3 d2 0c 8d 10 00 00 00 | rep ror byte ptr [4*rcx + 0x10], cl
X: b7f16 f0 d2 80 10 20 30 40 | lock rol byte ptr [rax + 0x40302010], cl
X: b88d0 d3 04 25 11 22 33 44 | rol dword ptr [0x44332211], cl
X: b8941 d3 0c 8d 10 00 00 00 | ror dword ptr [4*rcx + 0x10], cl
X: b8976 d3 5d 08 | rcr dword ptr [rbp + 0x8], cl
X: b89b1 d3 c1 | rol ecx, cl
X: b89bf d3 c8 | ror eax, cl
X: b89cd d3 d0 | rcl eax, cl
X: b89e9 d3 d8 | rcr eax, cl
X: b89f7 d3 e0 | shl eax, cl
X: b8a13 d3 e8 | shr eax, cl
X: b8a3d d3 f8 | sar eax, cl
X: b8b27 66 d3 45 fc | rol word ptr [rbp - 0x4], cl
X: b8c68 67 d3 04 24 | rol dword ptr [esp], cl
X: b9730 66 41 d3 0c 8d 10 00 00 00 | ror word ptr [4*rcx + 0x10], cl
X: b97ef f3 48 d3 c0 | rep rol rax, cl
X: bc829 d7 | xlatb
X: bd6ca 67 48 d7 | xlatb
X: bdddd f2 d8 1c e5 00 10 00 00 | repne esc 0x3, word ptr [0x1000]
X: be86d d9 84 88 00 01 00 00 | esc 0x8, word ptr [rax + 4*rcx + 0x100]
X: c0018 41 da 41 80 | esc 0x10, word ptr [r9 - 0x80]
X: c039d 48 da ff | esc 0x17, di
X: c0f54 40 db f8 | esc 0x1f, ax
X: c1192 44 db 84 88 00 01 00 00 | esc 0x18, word ptr [rax + 4*rcx + 0x100]
X: c178c dc 40 7f | esc 0x20, word ptr [rax + 0x7f]
X: c1f18 40 dc d0 | esc 0x22, ax
X: c2627 67 48 dc d8 | esc 0x23, ax
X: c36c9 f0 48 dd 44 24 08 | lock esc 0x28, word ptr [rsp + 0x8]
X: c3bc8 67 de e0 | esc 0x34, ax
X: c4068 41 de e8 | esc 0x35, r8w
X: c4587 66 41 de 04 88 | esc 0x30, word ptr [r8 + 4*rcx]
X: c489b 66 df 04 88 | esc 0x38, word ptr [rax + 4*rcx]
X: c490f 66 df 84 88 00 01 00 00 | esc 0x38, word ptr [rax + 4*rcx + 0x100]
X: c4abd 67 df 41 80 | esc 0x38, word ptr [ecx - 0x80]
X: c5771 e0 5d | loopne 0xc57d0
X: c6152 48 e0 80 | loopne 0xc60d5
X: c655b 67 48 e0 44 | loopne 0xc65a3
X: c6677 f0 48 e0 c1 | lock loopne 0xc663c
X: c67a8 e1 c1 | loope 0xc676b
X: c6cfd f0 e1 04 | lock loope 0xc6d04
X: c6f07 41 e1 41 | loope 0xc6f4b
X: c72fc 4c e1 75 | loope 0xc7374
X: c73ee 4f e1 04 | loope 0xc73f5
X: c746d 42 e1 45 | loope 0xc74b5
X: c75d4 f3 48 e1 ff | rep loope 0xc75d7
X: c7661 f0 48 e1 c1 | lock loope 0xc7626
X: c77c5 e2 d8 | loop 0xc779f
X: c7c42 f2 e2 41 | repne loop 0xc7c86
X: c7e06 66 67 e2 44 | loop 0xc7e4e
X: c83d1 4f e2 34 | loop 0xc8408
X: c8776 e3 d0 | jrcxz 0xc8748
X: c8c3b f2 e3 04 | repne jrcxz 0xc8c42
X: c8c7e f0 e3 e8 | lock jrcxz 0xc8c69
X: c8d79 64 e3 04 | jrcxz 0xc8d80
X: c8d99 66 67 e3 44 | jecxz 0xc8de1
X: c9252 49 e3 e5 | jrcxz 0xc923a
X: c9554 67 48 e3 04 | jecxz 0xc955c
X: c9664 e4 04 | in al, 0x4
X: c9be2 f2 e4 34 | repne in al, 0x34
X: c9f36 41 e4 c0 | in al, 0xc0
X: ca3c8 42 e4 e8 | in al, 0xe8
X: ca40a 66 48 e4 1c | in al, 0x1c
X: ca6fc e5 34 | in eax, 0x34
X: caf4e 41 e5 d3 | in eax, 0xd3
X: cb43e 66 48 e5 0c | in eax, 0xc
X: cb621 e6 04 | out 0x4, al
X: cbc3a f0 e6 04 | lock out 0x4, al
X: cc300 4d e6 d3 | out 0xd3, al
X: cc64c e7 80 | out 0x80, eax
X: cd4e2 f3 48 e7 05 | rep out 0x5, eax
X: cd6a9 e8 c0 11 22 33 | call 0x332ee86e
X: cd6ef e8 d8 11 22 33 | call 0x332ee8cc
X: cdb01 f3 e8 40 7f ff fe | rep call 0xffffffffff0c5a47
X: ce376 42 e8 0c 8d 10 00 | call 0x1d7088
X: ce5ff e9 40 7f 11 22 | jmp 0x221e6544
X: ce706 e9 e5 11 22 33 | jmp 0x332ef8f0
X: ceb99 f2 e9 45 fc 11 22 | repne jmp 0x221ee7e4
X: ced37 66 67 e9 d3 11 | jmp 0xcff0f
X: cf294 4d e9 04 25 ff fe | jmp 0xffffffffff0c179e
X: cf517 f3 48 e9 04 88 ff fe | rep jmp 0xffffffffff0c7d22
X: d064d eb 45 | jmp 0xd0694
X: d06c1 eb d3 | jmp 0xd0696
X: d0bb5 f0 eb 44 | lock jmp 0xd0bfc
X: d0d11 66 67 eb 04 | jmp 0xd0d19
X: d0d61 40 eb e5 | jmp 0xd0d49
X: d1032 48 eb 41 | jmp 0xd1076
X: d14cd f3 48 eb d3 | rep jmp 0xd14a4
X: d1609 ec | in al, dx
X: d1df3 41 ec | in al, dx
X: d2259 4d ec | in al, dx
X: d23e5 66 41 ec | in al, dx
X: d26d4 ed | in eax, dx
X: d2b99 f0 ed | lock in eax, dx
X: d3657 ee | out dx, al
X: d3b83 2e ee | out dx, al
X: d3cbc 40 ee | out dx, al
X: d4482 f0 48 ee | lock out dx, al
X: d4572 ef | out dx, eax
X: d46ac 66 ef | out dx, ax
X: d5122 49 ef | out dx, eax
X: d5651 f1 | int1
X: d5c3d 66 67 f1 | int1
X: d645e f0 48 f1 | lock int1
X: d65ab f4 | hlt
X: d70e8 49 f4 | hlt
X: d7316 66 41 f4 | hlt
X: d7409 f3 48 f4 | rep hlt
X: d757c f5 | cmc
X: d76c0 66 f5 | cmc
X: d7b7c 64 f5 | cmc
X: d803e 48 f5 | cmc
X: d8119 4c f5 | cmc
X: d81b8 4f f5 | cmc
X: d8423 f0 48 f5 | lock cmc
X: d848e f6 04 24 11 | test byte ptr [rsp], 0x11
X: d8525 f6 5d 08 | neg byte ptr [rbp + 0x8]
X: d8534 f6 75 00 | div byte ptr [rbp]
X: d8552 f6 c0 11 | test al, 0x11
X: d857c f6 d0 | not al
X: d8598 f6 d8 | neg al
X: d85b4 f6 e5 | mul ch
X: d85c2 f6 e8 | imul al
X: d85d0 f6 f0 | div al
X: d85fa f6 ff | idiv bh
X: d8617 66 f6 05 11 22 33 44 55 | test byte ptr [rip + 0x44332211], 0x55 # 0x4440a830
X: d8b57 64 f6 c0 11 | test al, 0x11
X: d8d41 41 f6 34 24 | div byte ptr [r12]
X: d958e f7 e5 | mul ebp
X: d9e38 44 f7 d8 | neg eax
X: da20c 66 48 f7 04 24 ff fe ff ff | test qword ptr [rsp], -0x101
X: da436 f8 | clc
X: da9b0 f3 f8 | rep clc
X: daafc 64 f8 | clc
X: db0d3 4d f8 | clc
X: db141 4f f8 | clc
X: db50a f9 | stc
X: db91a 67 f9 | stc
X: dbaee 64 f9 | stc
X: dc050 4c f9 | stc
X: dc111 4f f9 | stc
X: dc381 f0 48 f9 | lock stc
X: dc3ec fa | cli
X: dc9e9 f0 fa | lock cli
X: dcd3e 41 fa | cli
X: dcd7a 44 fa | cli
X: dd419 fb | sti
X: dd99e f2 fb | repne sti
X: ddde9 48 fb | sti
X: de48c fc | cld
X: dea0f 2e fc | cld
X: deaaf 66 67 fc | cld
X: ded0b 41 fc | cld
X: df138 42 fc | cld
X: df34e f0 48 fc | lock cld
X: df46f fd | std
X: df535 66 fd | std
X: df947 f2 fd | repne std
X: e00ea 42 fd | std
X: e0333 fe 00 | inc byte ptr [rax]
X: e03cf fe 0c 8d 10 00 00 00 | dec byte ptr [4*rcx + 0x10]
X: e0431 fe c0 | inc al
X: e044d fe c8 | dec al
X: e0b08 40 fe c8 | dec al
X: e0d16 44 fe c1 | inc cl
X: e0f94 4c fe 84 88 00 01 00 00 | inc byte ptr [rax + 4*rcx + 0x100]
X: e1433 ff d3 | call rbx
X: e144f ff e0 | jmp rax
X: e1487 ff f6 | push rsi
X: e2356 0f 00 84 88 00 01 00 00 | sldt word ptr [rax + 4*rcx + 0x100]
X: e242c 0f 00 e0 | verr ax
X: e25c3 66 0f 00 c1 | sldt cx
X: e295c f3 0f 00 5d 08 | ltr word ptr [rbp + 0x8]
X: e2aa5 f2 0f 00 41 80 | sldt word ptr [rcx - 0x80]
X: e2ae7 f0 0f 00 d0 | lock lldt ax
X: e2b60 64 0f 00 c8 | str eax
X: e2cfb 41 0f 00 84 88 00 01 00 00 | sldt word ptr [r8 + 4*rcx + 0x100]
X: e3201 42 0f 00 e0 | verr ax
X: e343e 0f 01 04 24 | sgdt [rsp]
X: e350e 0f 01 c0 | enclv
X: e351d 0f 01 c1 | vmcall
X: e352c 0f 01 c8 | monitor
X: e353b 0f 01 d0 | xgetbv
X: e3559 0f 01 d8 | vmrun
X: e3586 0f 01 e8 | serialize
X: e35b3 0f 01 f8 | swapgs
X: e35c2 0f 01 ff | tlbsync
X: e3d74 40 0f 01 ff | tlbsync
X: e43c0 66 48 0f 01 05 11 22 33 44 | sgdt [rip + 0x44332211] # 0x444165da
X: e45a5 0f 02 41 80 | lar eax, word ptr [rcx - 0x80]
X: e46bc 0f 02 e8 | lar ebp, ax
X: e5663 0f 03 05 ff fe ff ff | lsl eax, word ptr [rip - 0x101] # 0xe5569
X: e5771 0f 03 c1 | lsl eax, cx
X: e64b9 4c 0f 03 04 88 | lsl r8, word ptr [rax + 4*rcx]
X: e64db 4d 0f 03 1c e5 00 10 00 00 | lsl r11, word ptr [0x1000]
X: e65db 66 48 0f 03 f6 | lsl rsi, si
X: e661f 66 41 0f 03 e0 | lsl sp, r8w
DB: e6b38 66 0f 04 ff 11 22 33 44 | db 0x66
DB: e7418 48 0f 04 1c e5 00 10 00 | db 0x48
X: e79b5 0f 05 | syscall
X: e8a9a 0f 06 | clts
X: e8d67 66 0f 06 | clts
X: e91b8 f2 0f 06 | clts
X: e95d6 44 0f 06 | clts
X: e98f8 4f 0f 06 | clts
X: e9968 42 0f 06 | clts
X: e9ba5 0f 07 | sysret
X: ea1de f3 0f 07 | sysret
X: ea2f2 f2 0f 07 | sysret
X: ea396 2e 0f 07 | sysret
X: eaa4f 4f 0f 07 | sysretq
X: eaaa4 42 0f 07 | sysret
X: eada6 0f 08 | invd
X: ebb26 4d 0f 08 | invd
X: ebcdd 67 48 0f 08 | invd
X: ebeae 0f 09 | wbinvd
X: ec64f 2e 0f 09 | wbinvd
DB: eda71 41 0f 0a e5 ff fe ff ff | db 0x41
X: ee0fe 0f 0b | ud2
X: ee8b8 f0 0f 0b | lock ud2
X: eef57 4f 0f 0b | ud2
X: ef0a3 66 41 0f 0b | ud2
X: ef0d6 67 48 0f 0b | ud2
X: ef182 f0 48 0f 0b | lock ud2
X: f032c 0f 0d 04 25 11 22 33 44 | prefetch byte ptr [0x44332211]
X: f0d7a 41 0f 0d 45 fc | prefetch byte ptr [r13 - 0x4]
X: f10f2 49 0f 0d 04 88 | prefetch byte ptr [r8 + 4*rcx]
X: f11dd 4f 0f 0d 44 24 08 | prefetch byte ptr [r12 + r12 + 0x8]
DB: fcddb 48 0f 18 d0 11 22 33 44 | db 0x48
X: fce8b 49 0f 18 45 fc | prefetchnta byte ptr [r13 - 0x4]
X: fd06f 66 48 0f 18 40 7f | prefetchnta byte ptr [rax + 0x7f]
DB: fd7e2 f3 0f 19 44 24 08 ff fe | db 0xf3
DB: fd98d f2 0f 19 f6 11 22 33 44 | db 0xf2
DB: fda17 f0 0f 19 f8 ff fe ff ff | db 0xf0
DB: fe249 67 48 0f 19 f8 ff fe ff | db 0x67
X: 1005b4 0f 1c 04 88 | cldemote byte ptr [rax + 4*rcx]
X: 100e3e 64 0f 1c 84 88 00 01 00 00 | cldemote byte ptr fs:[rax + 4*rcx + 0x100]
X: 100f1c 40 0f 1c 04 24 | cldemote byte ptr [rsp]
X: 101534 66 48 0f 1c 80 10 20 30 40 | cldemote byte ptr [rax + 0x40302010]
X: 103a4b 0f 1f 34 24 | nop dword ptr [rsp]
X: 103ab5 0f 1f e0 | nop eax
X: 1041f0 64 0f 1f c8 | nop eax
X: 104881 42 0f 1f 5d 08 | nop dword ptr [rbp + 0x8]
X: 1049b2 67 48 0f 1f e8 | nop rax
X: 104ba6 0f 20 c1 | mov rcx, cr0
X: 104bf1 0f 20 e0 | mov rax, cr4
DB: 104f0d 67 0f 20 1c e5 00 10 00 | db 0x67
X: 104f76 67 0f 20 c1 | mov rcx, cr0
X: 1053f9 40 0f 20 d3 | mov rbx, cr2
X: 105870 49 0f 20 d8 | mov r8, cr3
X: 1059b4 42 0f 20 f6 | mov rsi, cr6
X: 105cd3 0f 21 c1 | mov rcx, dr0
X: 105d3c 0f 21 e8 | mov rax, dr5
X: 1063cb f0 0f 21 e8 | lock mov rax, dr5
X: 1069fc 4c 0f 21 ff | mov rdi, dr15
X: 106c6c f3 48 0f 21 c1 | mov rcx, dr0
X: 106df9 0f 22 c0 | mov cr0, rax
X: 106e8f 0f 22 f6 | mov cr6, rsi
X: 10765b 40 0f 22 e8 | mov cr5, rax
X: 107d82 f3 48 0f 22 e5 | mov cr4, rbp
X: 107f35 0f 23 c0 | mov dr0, rax
X: 107f80 0f 23 d8 | mov dr3, rax
X: 1089a3 41 0f 23 ff | mov dr7, r15
DB: 10a76a f3 0f 25 c1 11 22 33 44 | db 0xf3
DB: 10aa89 41 0f 25 40 7f ff fe ff | db 0x41
DB: 11217c 2e 0f 2c 44 24 08 11 22 | db 0x2e
DB: 114e89 0f 2f f8 ff fe ff ff 80 | db 0xf
X: 115e7e 0f 30 | wrmsr
X: 116dda 66 41 0f 30 | wrmsr
X: 116f3d 0f 31 | rdtsc
X: 1177a3 2e 0f 31 | rdtsc
X: 11784e 66 67 0f 31 | rdtsc
X: 117ea2 66 48 0f 31 | rdtsc
X: 118180 0f 32 | rdmsr
X: 1190fe f3 48 0f 32 | rdmsr
X: 119183 0f 33 | rdpmc
X: 11992e f2 0f 33 | rdpmc
X: 119b50 41 0f 33 | rdpmc
X: 119e7e 48 0f 33 | rdpmc
X: 11a41b 0f 34 | sysenter
X: 11a4c4 66 0f 34 | sysenter
X: 11aa5d f2 0f 34 | sysenter
X: 11aaaf f0 0f 34 | lock sysenter
X: 11af5b 48 0f 34 | sysenter
X: 11b51d 0f 35 | sysexit
DB: 11c7d3 66 0f 36 1c e5 00 10 00 | db 0x66
X: 11d791 0f 37 | getsec
X: 11e2dc 48 0f 37 | getsec
X: 11e4a9 4c 0f 37 | getsec
X: 11e521 4d 0f 37 | getsec
DB: 121972 42 0f 3a f8 11 22 33 44 | db 0x42
DB: 124853 41 0f 3d 0c 8d 10 00 00 | db 0x41
X: 1271af 0f 40 00 | cmovo eax, dword ptr [rax]
X: 127335 0f 40 e8 | cmovo ebp, eax
X: 127aa5 66 67 0f 40 41 80 | cmovo ax, word ptr [ecx - 0x80]
X: 1282eb 0f 41 00 | cmovno eax, dword ptr [rax]
X: 128426 0f 41 d0 | cmovno edx, eax
X: 128b8a 64 0f 41 44 24 08 | cmovno eax, dword ptr fs:[rsp + 0x8]
X: 1290c5 49 0f 41 34 24 | cmovno rsi, qword ptr [r12]
X: 129515 0f 42 5d 08 | cmovb ebx, dword ptr [rbp + 0x8]
X: 129581 0f 42 d3 | cmovb edx, ebx
X: 129c37 f0 0f 42 80 10 20 30 40 | lock cmovb eax, dword ptr [rax + 0x40302010]
X: 129d2e 66 67 0f 42 1c e5 00 10 00 00 | cmovb bx, word ptr [0x1000]
X: 129fc9 44 0f 42 f6 | cmovb r14d, esi
X: 12a2d4 4d 0f 42 80 10 20 30 40 | cmovb r8, qword ptr [r8 + 0x40302010]
X: 12a397 42 0f 42 45 fc | cmovb eax, dword ptr [rbp - 0x4]
X: 12a581 0f 43 00 | cmovae eax, dword ptr [rax]
X: 12a6ad 0f 43 c8 | cmovae ecx, eax
X: 12b47f 4f 0f 43 1c e5 00 10 00 00 | cmovae r11, qword ptr [8*r12 + 0x1000]
X: 12b510 66 48 0f 43 44 24 08 | cmovae rax, qword ptr [rsp + 0x8]
X: 12b59d 66 41 0f 43 d8 | cmovae bx, r8w
X: 12b62e f3 48 0f 43 44 24 08 | cmovae rax, qword ptr [rsp + 0x8]
X: 12b7ad 0f 44 75 00 | cmove esi, dword ptr [rbp]
X: 12b836 0f 44 e5 | cmove esp, ebp
X: 12c152 41 0f 44 75 00 | cmove esi, dword ptr [r13]
X: 12c26a 44 0f 44 e8 | cmove r13d, eax
X: 12c510 4c 0f 44 e0 | cmove r12, rax
X: 12c61b 42 0f 44 04 25 ff fe ff ff | cmove eax, dword ptr [1*r12 - 0x101]
X: 12c62c 66 48 0f 44 f6 | cmove rsi, rsi
X: 12c733 67 48 0f 44 e8 | cmove rbp, rax
X: 12c8ef 0f 45 34 24 | cmovne esi, dword ptr [rsp]
X: 12c9b3 0f 45 ff | cmovne edi, edi
X: 12cf1c f3 0f 45 e0 | cmovne esp, eax
X: 12d34e 41 0f 45 f6 | cmovne esi, r14d
X: 12d4e3 48 0f 45 34 24 | cmovne rsi, qword ptr [rsp]
X: 12d815 66 41 0f 45 44 24 08 | cmovne ax, word ptr [r12 + 0x8]
X: 12da03 0f 46 45 fc | cmovbe eax, dword ptr [rbp - 0x4]
X: 12daac 0f 46 e5 | cmovbe esp, ebp
X: 12e311 41 0f 46 04 24 | cmovbe eax, dword ptr [r12]
X: 12eae3 0f 47 41 80 | cmova eax, dword ptr [rcx - 0x80]
X: 12ebfa 0f 47 e8 | cmova ebp, eax
X: 12f22f f2 0f 47 e5 | cmova esp, ebp
X: 12f3cd 40 0f 47 d3 | cmova edx, ebx
X: 12f52c 41 0f 47 c0 | cmova eax, r8d
X: 12f860 49 0f 47 40 7f | cmova rax, qword ptr [r8 + 0x7f]
X: 12faf9 67 48 0f 47 f6 | cmova rsi, rsi
X: 12fb0a f3 48 0f 47 c8 | cmova rcx, rax
X: 12fc1e 0f 48 40 7f | cmovs eax, dword ptr [rax + 0x7f]
X: 12fd45 0f 48 e8 | cmovs ebp, eax
X: 13036f f2 0f 48 41 80 | cmovs eax, dword ptr [rcx - 0x80]
X: 130a77 4d 0f 48 40 7f | cmovs r8, qword ptr [r8 + 0x7f]
X: 130d33 0f 49 04 24 | cmovns eax, dword ptr [rsp]
X: 130e8a 0f 49 f0 | cmovns esi, eax
X: 130f4d 66 0f 49 80 10 20 30 40 | cmovns ax, word ptr [rax + 0x40302010]
X: 131ace 49 0f 49 d3 | cmovns rdx, r11
X: 131b61 4c 0f 49 f0 | cmovns r14, rax
X: 131ecc 0f 4a 80 10 20 30 40 | cmovp eax, dword ptr [rax + 0x40302010]
X: 131fa6 0f 4a d8 | cmovp ebx, eax
X: 1322a7 67 0f 4a 84 88 00 01 00 00 | cmovp eax, dword ptr [eax + 4*ecx + 0x100]
X: 1327c6 40 0f 4a 05 ff fe ff ff | cmovp eax, dword ptr [rip - 0x101] # 0x1326cd
X: 132a9c 48 0f 4a 41 80 | cmovp rax, qword ptr [rcx - 0x80]
X: 132c34 49 0f 4a 04 24 | cmovp rax, qword ptr [r12]
X: 132dd3 66 48 0f 4a 41 80 | cmovp rax, qword ptr [rcx - 0x80]
X: 132eaf 67 48 0f 4a c8 | cmovp rcx, rax
X: 133051 0f 4b 5d 08 | cmovnp ebx, dword ptr [rbp + 0x8]
X: 1330ae 0f 4b d0 | cmovnp edx, eax
X: 133da9 4c 0f 4b 0c 8d 10 00 00 00 | cmovnp r9, qword ptr [4*rcx + 0x10]
X: 133f2e 66 48 0f 4b 80 10 20 30 40 | cmovnp rax, qword ptr [rax + 0x40302010]
X: 1340d4 0f 4c 04 25 ff fe ff ff | cmovl eax, dword ptr [0xfffffeff]
X: 1341c3 0f 4c c1 | cmovl eax, ecx
X: 1347b1 f3 0f 4c d3 | cmovl edx, ebx
X: 134851 f2 0f 4c 1c e5 00 10 00 00 | cmovl ebx, dword ptr [0x1000]
X: 13524c 0f 4d 40 7f | cmovge eax, dword ptr [rax + 0x7f]
X: 135328 0f 4d d0 | cmovge edx, eax
X: 1355ac 67 0f 4d 00 | cmovge eax, dword ptr [eax]
X: 135a64 2e 0f 4d e0 | cmovge esp, eax
X: 135b57 40 0f 4d 44 24 08 | cmovge eax, dword ptr [rsp + 0x8]
X: 1360b3 4f 0f 4d 04 25 11 22 33 44 | cmovge r8, qword ptr [1*r12 + 0x44332211]
X: 136185 66 48 0f 4d e0 | cmovge rsp, rax
X: 1361da 66 41 0f 4d 5d 08 | cmovge bx, word ptr [r13 + 0x8]
X: 13629f f3 48 0f 4d d8 | cmovge rbx, rax
X: 13633f 0f 4e 04 88 | cmovle eax, dword ptr [rax + 4*rcx]
X: 136489 0f 4e e0 | cmovle esp, eax
X: 136c68 40 0f 4e e0 | cmovle esp, eax
X: 137424 f0 48 0f 4e e5 | lock cmovle rsp, rbp
X: 1374c7 0f 4f 41 80 | cmovg eax, dword ptr [rcx - 0x80]
X: 1375ed 0f 4f f0 | cmovg esi, eax
DB: 13c0f2 f2 0f 53 c0 11 22 33 44 | db 0xf2
DB: 13dfe6 67 0f 55 40 7f ff fe ff | db 0x67
DB: 13f542 f0 0f 56 e8 11 22 33 44 | db 0xf0
DB: 147c3f 67 0f 5e e0 11 22 33 44 | db 0x67
DB: 15f126 66 48 0f 73 f6 11 22 33 | db 0x66
X: 16c2ef 0f 80 e0 ff fe ff | jo 0x15c2d5
X: 16d1f0 67 48 0f 80 80 10 20 30 | jo 0x3036e278
X: 16d2cb 0f 81 00 11 22 33 | jno 0x3338e3d1
X: 16daf3 f0 0f 81 f0 ff fe ff | lock jno 0x15daea
X: 16db48 2e 0f 81 d3 ff fe ff | jno 0x15db22
X: 16de7d 44 0f 81 e0 11 22 33 | jno 0x3338f064
X: 16e24f 66 48 0f 81 d3 11 | jno 0x16f428
X: 16e57e 0f 82 e0 11 22 33 | jb 0x3338f764
X: 16eb11 f3 0f 82 d0 ff fe ff | jb 0x15eae8
X: 16f272 4c 0f 82 04 88 ff fe | jb 0xffffffffff167a7d
X: 16f3d7 66 48 0f 82 75 00 | jb 0x16f452
X: 16f519 f0 48 0f 82 0c 8d 10 00 | lock jb 0x27822d
X: 16f650 0f 83 75 00 11 22 | jae 0x2227f6cb
X: 16fb2f f3 0f 83 04 88 11 22 | jae 0x2228833a
X: 16fd1d f2 0f 83 34 24 11 22 | jae 0x22282158
X: 16fea0 40 0f 83 ff 11 22 33 | jae 0x333910a6
X: 1703d8 4d 0f 83 44 24 08 11 | jae 0x111f2823
X: 170729 0f 84 0c 8d 10 00 | je 0x27943b
X: 171471 49 0f 84 0c 8d 10 00 | je 0x27a184
X: 17160c 66 48 0f 84 45 fc | je 0x171257
X: 1716e7 67 48 0f 84 e0 11 22 33 | je 0x333928cf
X: 1718b8 0f 85 34 24 ff fe | jne 0xffffffffff163cf2
X: 171f25 f3 0f 85 f6 11 22 33 | jne 0x33393122
X: 172058 64 0f 85 34 24 11 22 | jne 0x22284493
X: 17252b 48 0f 85 e5 ff fe ff | jne 0x162517
X: 17259c 49 0f 85 d8 11 22 33 | jne 0x3339377b
X: 17268b 4f 0f 85 c8 11 22 33 | jne 0x3339385a
X: 172712 42 0f 85 45 fc 11 22 | jne 0x2229235e
X: 172a8c 0f 86 f6 11 22 33 | jbe 0x33393c88
X: 172fb3 f3 0f 86 c0 11 22 33 | jbe 0x3339417a
X: 173094 f2 0f 86 f8 11 22 33 | jbe 0x33394293
X: 1731ee 66 67 0f 86 5d 08 | jbe 0x173a51
X: 173a46 0f 87 04 24 11 22 | ja 0x22285e50
X: 173d77 66 0f 87 e8 11 | ja 0x174f64
X: 174b5f 0f 88 05 11 22 33 | js 0x33395c6a
X: 1756e0 44 0f 88 ff ff fe ff | js 0x1656e6
X: 175a63 4f 0f 88 e8 11 22 33 | js 0x33396c52
X: 175e57 0f 89 ff ff fe ff | jns 0x165e5c
X: 176024 66 0f 89 f6 ff | jns 0x17601f
X: 176430 f2 0f 89 34 24 ff fe | jns 0xffffffffff16886b
X: 176507 2e 0f 89 5d 08 11 22 | jns 0x22286d6b
X: 176a7e 49 0f 89 1c e5 00 10 | jns 0x10184fa1
X: 176b1c 4d 0f 89 40 7f ff fe | jns 0xffffffffff16ea63
X: 176be3 42 0f 89 e0 ff fe ff | jns 0x166bca
X: 176c15 66 48 0f 89 e8 ff | jns 0x176c03
X: 176d41 f3 48 0f 89 c8 11 22 33 | jns 0x33397f11
X: 176f80 0f 8a ff 11 22 33 | jp 0x33398185
X: 177708 40 0f 8a 40 7f 11 22 | jp 0x2228f64f
X: 177f46 0f 8b 44 24 08 ff | jnp 0xffffffffff1fa390
X: 178728 f0 0f 8b 34 24 11 22 | lock jnp 0x2228ab63
X: 17876a 2e 0f 8b ff ff fe ff | jnp 0x168770
X: 178d94 4d 0f 8b 04 88 ff fe | jnp 0xffffffffff17159f
X: 179089 0f 8c 44 24 08 11 | jl 0x111fb4d3
X: 179c0e 44 0f 8c f8 ff fe ff | jl 0x169c0d
X: 17a2e5 0f 8d e0 11 22 33 | jge 0x3339b4cb
X: 17aa55 64 0f 8d c8 11 22 33 | jge 0x3339bc24
X: 17ac31 41 0f 8d c0 11 22 33 | jge 0x3339bdf8
X: 17b10f 66 48 0f 8d 0c 8d | jge 0x173e21
X: 17b3de 0f 8e c1 ff fe ff | jle 0x16b3a5
X: 17bbc7 66 67 0f 8e f0 ff | jle 0x17bbbd
X: 17c17b 4d 0f 8e 04 88 11 22 | jle 0x22294986
X: 17c1c1 4f 0f 8e c0 ff fe ff | jle 0x16c188
X: 17c4b6 0f 8f 1c e5 00 10 | jg 0x1018a9d8
X: 17c9eb f3 0f 8f 04 24 ff fe | jg 0xffffffffff16edf6
X: 17ccb4 64 0f 8f f0 11 22 33 | jg 0x3339deab
X: 17d23b 4c 0f 8f d8 11 22 33 | jg 0x3339e41a
X: 17d57d 0f 90 44 24 08 | seto byte ptr [rsp + 0x8]
X: 17d697 0f 90 e0 | seto al
X: 17dcef f2 0f 90 e8 | seto al
X: 17dd64 f0 0f 90 f0 | lock seto al
X: 17de8b 40 0f 90 e8 | seto al
X: 17e0ea 44 0f 90 04 25 11 22 33 44 | seto byte ptr [0x44332211]
X: 17e470 42 0f 90 d3 | seto bl
X: 17e6b4 0f 91 04 24 | setno byte ptr [rsp]
X: 17e7b1 0f 91 d0 | setno al
X: 17eefe 2e 0f 91 80 10 20 30 40 | setno byte ptr cs:[rax + 0x40302010]
X: 17f5bd 42 0f 91 1c e5 00 10 00 00 | setno byte ptr [8*r12 + 0x1000]
X: 17f616 66 48 0f 91 41 80 | setno byte ptr [rcx - 0x80]
X: 17f83e 0f 92 80 10 20 30 40 | setb byte ptr [rax + 0x40302010]
X: 17f954 0f 92 f0 | setb al
X: 180124 40 0f 92 84 88 00 01 00 00 | setb byte ptr [rax + 4*rcx + 0x100]
X: 180383 44 0f 92 c1 | setb cl
X: 180671 4d 0f 92 45 fc | setb byte ptr [r13 - 0x4]
X: 1807a8 66 41 0f 92 04 88 | setb byte ptr [r8 + 4*rcx]
X: 1809fa 0f 93 5d 08 | setae byte ptr [rbp + 0x8]
X: 180a2a 0f 93 c0 | setae al
X: 181203 64 0f 93 04 88 | setae byte ptr fs:[rax + 4*rcx]
X: 1812b1 40 0f 93 e8 | setae al
X: 18141f 41 0f 93 d3 | setae r11b
X: 181720 49 0f 93 44 24 08 | setae byte ptr [r12 + 0x8]
X: 181b1b 0f 94 1c e5 00 10 00 00 | sete byte ptr [0x1000]
X: 181bf6 0f 94 f0 | sete al
X: 181d60 66 0f 94 c1 | sete cl
X: 18213c f3 0f 94 c1 | sete cl
X: 18221c f2 0f 94 e5 | sete ch
X: 182363 66 67 0f 94 ff | sete bh
X: 1829d0 66 48 0f 94 d0 | sete al
X: 182b5a f0 48 0f 94 84 88 00 01 00 00 | lock sete byte ptr [rax + 4*rcx + 0x100]
X: 182bb0 0f 95 04 88 | setne byte ptr [rax + 4*rcx]
X: 182cbe 0f 95 c8 | setne al
X: 1836b4 41 0f 95 e5 | setne r13b
X: 183a21 4d 0f 95 ff | setne r15b
X: 183cb8 0f 96 00 | setbe byte ptr [rax]
X: 183e11 0f 96 d8 | setbe al
X: 184453 f2 0f 96 ff | setbe bh
X: 184568 64 0f 96 84 88 00 01 00 00 | setbe byte ptr fs:[rax + 4*rcx + 0x100]
X: 184e94 0f 97 84 88 00 01 00 00 | seta byte ptr [rax + 4*rcx + 0x100]
X: 184f1f 0f 97 c1 | seta cl
X: 1855ae f2 0f 97 f6 | seta dh
X: 1856d9 64 0f 97 1c e5 00 10 00 00 | seta byte ptr fs:[0x1000]
X: 186021 0f 98 5d 08 | sets byte ptr [rbp + 0x8]
X: 18609c 0f 98 d8 | sets al
X: 186776 f0 0f 98 00 | lock sets byte ptr [rax]
X: 186b51 48 0f 98 04 25 11 22 33 44 | sets byte ptr [0x44332211]
X: 186ffb f3 48 0f 98 00 | sets byte ptr [rax]
X: 18710e 0f 99 84 88 00 01 00 00 | setns byte ptr [rax + 4*rcx + 0x100]
X: 18722f 0f 99 f8 | setns al
X: 187569 67 0f 99 c1 | setns cl
X: 187725 f3 0f 99 75 00 | setns byte ptr [rbp]
X: 187848 f2 0f 99 40 7f | setns byte ptr [rax + 0x7f]
X: 187f72 4f 0f 99 44 24 08 | setns byte ptr [r12 + r12 + 0x8]
X: 188142 f3 48 0f 99 05 11 22 33 44 | setns byte ptr [rip + 0x44332211] # 0x444ba35c
X: 18828a 0f 9a 5d 08 | setp byte ptr [rbp + 0x8]
X: 188332 0f 9a e8 | setp al
X: 1887be f3 0f 9a 40 7f | setp byte ptr [rax + 0x7f]
X: 189309 0f 9b 04 25 11 22 33 44 | setnp byte ptr [0x44332211]
X: 18949d 0f 9b ff | setnp bh
X: 189b80 64 0f 9b 75 00 | setnp byte ptr fs:[rbp]
X: 18a1ad 4f 0f 9b c8 | setnp r8b
X: 18a3c9 f0 48 0f 9b 00 | lock setnp byte ptr [rax]
X: 18a412 0f 9c 00 | setl byte ptr [rax]
X: 18a589 0f 9c e5 | setl ch
X: 18a824 67 0f 9c 44 24 08 | setl byte ptr [esp + 0x8]
X: 18aa24 f3 0f 9c 40 7f | setl byte ptr [rax + 0x7f]
X: 18abce f2 0f 9c 00 | setl byte ptr [rax]
X: 18b369 42 0f 9c e0 | setl al
X: 18b42f 66 41 0f 9c 40 7f | setl byte ptr [r8 + 0x7f]
X: 18b58a 0f 9d 04 24 | setge byte ptr [rsp]
X: 18b65a 0f 9d c0 | setge al
X: 18be89 40 0f 9d f6 | setge sil
X: 18c30b 49 0f 9d c8 | setge r8b
X: 18c56d 67 48 0f 9d 80 10 20 30 40 | setge byte ptr [eax + 0x40302010]
X: 18c66a 0f 9e 00 | setle byte ptr [rax]
X: 18c81d 0f 9e f8 | setle al
X: 18d88e 0f 9f 45 fc | setg byte ptr [rbp - 0x4]
X: 18d937 0f 9f e5 | setg ch
X: 18db4f 66 0f 9f f6 | setg dh
X: 18e91f 0f a0 | push fs
X: 18f0d2 f2 0f a0 | push fs
X: 18f1e4 64 0f a0 | push fs
X: 18f341 41 0f a0 | push fs
X: 18fb65 0f a1 | pop fs
X: 19033c 66 67 0f a1 | pop fs
X: 1903de 40 0f a1 | pop fs
X: 190622 44 0f a1 | pop fs
X: 1907f0 48 0f a1 | pop fs
X: 1908f7 4d 0f a1 | pop fs
X: 190a54 66 41 0f a1 | pop fs
X: 190d32 0f a2 | cpuid
X: 1919b6 4c 0f a2 | cpuid
X: 191c55 f0 48 0f a2 | lock cpuid
X: 191d0f 0f a3 40 7f | bt dword ptr [rax + 0x7f], eax
X: 191e54 0f a3 f6 | bt esi, esi
X: 192be8 42 0f a3 04 24 | bt dword ptr [rsp + r12], eax
X: 192df9 0f a4 05 ff fe ff ff 80 | shld dword ptr [rip - 0x101], eax, 0x80 # 0x192d00
X: 192f16 0f a4 c8 ff | shld eax, ecx, 0xff
X: 193080 66 0f a4 1c e5 00 10 00 00 ff | shld word ptr [0x1000], bx, 0xff
X: 1935d7 f0 0f a4 05 11 22 33 44 55 | lock shld dword ptr [rip + 0x44332211], eax, 0x55 # 0x444c57f1
X: 193c08 49 0f a4 f0 ff | shld r8, rsi, 0xff
X: 193c6e 4d 0f a4 04 25 ff fe ff ff 80 | shld qword ptr [0xfffffeff], r8, 0x80
X: 193e77 f3 48 0f a4 c8 11 | shld rax, rcx, 0x11
X: 193f3a 0f a5 05 ff fe ff ff | shld dword ptr [rip - 0x101], eax, cl # 0x193e40
X: 1940ed 0f a5 ff | shld edi, edi, cl
X: 194428 67 0f a5 c8 | shld eax, ecx, cl
X: 194833 66 67 0f a5 05 11 22 33 44 | shld word ptr [eip + 0x44332211], ax, cl
X: 194aed 44 0f a5 c1 | shld ecx, r8d, cl
X: 195042 f0 48 0f a5 45 fc | lock shld qword ptr [rbp - 0x4], rax, cl
X: 19749e 0f a8 | push gs
X: 197fab 48 0f a8 | push gs
X: 1983ab f3 48 0f a8 | push gs
X: 1984cb 0f a9 | pop gs
X: 198c53 f0 0f a9 | lock pop gs
X: 1992e1 4d 0f a9 | pop gs
X: 199456 66 41 0f a9 | pop gs
X: 1995dd 0f aa | rsm
X: 19995e 66 0f aa | rsm
X: 19a158 44 0f aa | rsm
X: 19a793 0f ab 84 88 00 01 00 00 | bts dword ptr [rax + 4*rcx + 0x100], eax
X: 19a8b4 0f ab f8 | bts eax, edi
X: 19aa50 66 0f ab e0 | bts ax, sp
X: 19af80 2e 0f ab 41 80 | bts dword ptr cs:[rcx - 0x80], eax
X: 19b1f1 41 0f ab c8 | bts r8d, ecx
X: 19b891 0f ac 04 24 11 | shrd dword ptr [rsp], eax, 0x11
X: 19b9d9 0f ac e8 ff | shrd eax, ebp, 0xff
X: 19bd40 67 0f ac c1 11 | shrd ecx, eax, 0x11
X: 19c1e2 40 0f ac 40 7f 11 | shrd dword ptr [rax + 0x7f], eax, 0x11
X: 19c724 4f 0f ac d0 11 | shrd r8, r10, 0x11
X: 19c89f 67 48 0f ac 1c e5 00 10 00 00 ff | shrd qword ptr [0x1000], rbx, 0xff
X: 19ca10 0f ad 41 80 | shrd dword ptr [rcx - 0x80], eax, cl
X: 19caaf 0f ad c0 | shrd eax, eax, cl
X: 19d84b 4d 0f ad 75 00 | shrd qword ptr [r13], r14, cl
X: 19d8bc 42 0f ad c0 | shrd eax, eax, cl
X: 19d949 66 48 0f ad e5 | shrd rbp, rsp, cl
X: 19dc78 0f ae f6 | mfence
X: 19e670 44 0f ae 0c 8d 10 00 00 00 | fxrstor [4*rcx + 0x10]
X: 19ec7e 0f af 80 10 20 30 40 | imul eax, dword ptr [rax + 0x40302010]
X: 19ed76 0f af e5 | imul esp, ebp
X: 19f417 f0 0f af ff | lock imul edi, edi
X: 19f560 40 0f af 04 24 | imul eax, dword ptr [rsp]
X: 19f655 41 0f af 0c 8d 10 00 00 00 | imul ecx, dword ptr [4*rcx + 0x10]
X: 19f7c8 44 0f af e5 | imul r12d, ebp
X: 19fa40 4c 0f af 34 24 | imul r14, qword ptr [rsp]
X: 19fa81 4d 0f af 84 88 00 01 00 00 | imul r8, qword ptr [r8 + 4*rcx + 0x100]
X: 19fbcc 66 48 0f af c0 | imul rax, rax
X: 19fe30 0f b0 75 00 | cmpxchg byte ptr [rbp], dh
X: 19ff04 0f b0 ff | cmpxchg bh, bh
X: 1a05e7 64 0f b0 04 88 | cmpxchg byte ptr fs:[rax + 4*rcx], al
X: 1a0b0f 49 0f b0 e0 | cmpxchg r8b, spl
X: 1a0f6b 0f b1 75 00 | cmpxchg dword ptr [rbp], esi
X: 1a1021 0f b1 f6 | cmpxchg esi, esi
X: 1a162d f2 0f b1 0c 8d 10 00 00 00 | cmpxchg dword ptr [4*rcx + 0x10], ecx
X: 1a16ed 2e 0f b1 c0 | cmpxchg eax, eax
X: 1a1879 41 0f b1 44 24 08 | cmpxchg dword ptr [r12 + 0x8], eax
X: 1a1b67 48 0f b1 75 00 | cmpxchg qword ptr [rbp], rsi
X: 1a1e64 66 41 0f b1 80 10 20 30 40 | cmpxchg word ptr [r8 + 0x40302010], ax
X: 1a1fe2 0f b2 05 11 22 33 44 | lss eax, [rip + 0x44332211] # 0x444d41fa
X: 1a288a 64 0f b2 44 24 08 | lss eax, fs:[rsp + 0x8]
X: 1a2c01 48 0f b2 04 24 | lss rax, [rsp]
X: 1a2e05 4c 0f b2 84 88 00 01 00 00 | lss r8, [rax + 4*rcx + 0x100]
X: 1a316c 0f b3 04 24 | btr dword ptr [rsp], eax
X: 1a325a 0f b3 c8 | btr eax, ecx
X: 1a37f9 f3 0f b3 c0 | btr eax, eax
X: 1a3b0a 41 0f b3 04 24 | btr dword ptr [r12], eax
X: 1a4297 0f b4 04 25 ff fe ff ff | lfs eax, [0xfffffeff]
X: 1a4b9e 66 67 0f b4 41 80 | lfs ax, [ecx - 0x80]
X: 1a510b 4d 0f b4 41 80 | lfs r8, [r9 - 0x80]
X: 1a5383 f0 48 0f b4 44 24 08 | lock lfs rax, [rsp + 0x8]
X: 1a53c5 0f b5 04 88 | lgs eax, [rax + 4*rcx]
X: 1a57ed 67 0f b5 80 10 20 30 40 | lgs eax, [eax + 0x40302010]
X: 1a62ca 42 0f b5 04 25 ff fe ff ff | lgs eax, [1*r12 - 0x101]
DB: 1a635b 66 48 0f b5 f8 11 22 33 | db 0x66
X: 1a6583 0f b6 84 88 00 01 00 00 | movzx eax, byte ptr [rax + 4*rcx + 0x100]
X: 1a6677 0f b6 e8 | movzx ebp, al
X: 1a6e09 66 67 0f b6 e8 | movzx bp, al
X: 1a6fd7 41 0f b6 c8 | movzx ecx, r8b
X: 1a747e 66 48 0f b6 41 80 | movzx rax, byte ptr [rcx - 0x80]
X: 1a765f 0f b7 04 25 ff fe ff ff | movzx eax, word ptr [0xfffffeff]
X: 1a77b7 0f b7 e8 | movzx ebp, ax
X: 1a7867 66 0f b7 40 7f | movzx ax, word ptr [rax + 0x7f]
X: 1a7fa7 40 0f b7 c1 | movzx eax, cx
X: 1a84d5 4d 0f b7 e0 | movzx r12, r8w
X: 1a861b 66 41 0f b7 d3 | movzx dx, r11w
X: 1a98b8 0f b9 00 | ud1 eax, dword ptr [rax]
X: 1a9a3e 0f b9 e8 | ud1 ebp, eax
X: 1aa2eb 41 0f b9 80 10 20 30 40 | ud1 eax, dword ptr [r8 + 0x40302010]
X: 1aa74a 4d 0f b9 ff | ud1 r15, r15
X: 1aa87f 66 48 0f b9 84 88 00 01 00 00 | ud1 rax, qword ptr [rax + 4*rcx + 0x100]
X: 1aa9e7 f0 48 0f b9 34 24 | lock ud1 rsi, qword ptr [rsp]
X: 1aab86 0f ba e5 ff | bt ebp, 0xff
X: 1aab95 0f ba e8 11 | bts eax, 0x11
DB: 1aac22 66 0f ba 04 24 11 22 33 | db 0x66
X: 1aafac 67 0f ba ff 11 | btc edi, 0x11
DB: 1ab2c6 64 0f ba 41 80 11 22 33 | db 0x64
X: 1ab30d 66 67 0f ba e5 ff | bt bp, 0xff
X: 1abb69 0f bb 05 11 22 33 44 | btc dword ptr [rip + 0x44332211], eax # 0x444ddd81
X: 1abc95 0f bb d0 | btc eax, edx
X: 1ac30a f2 0f bb c8 | btc eax, ecx
X: 1ac41a 64 0f bb 05 ff fe ff ff | btc dword ptr fs:[rip - 0x101], eax
X: 1ac5d7 41 0f bb 45 fc | btc dword ptr [r13 - 0x4], eax
X: 1ac9d8 4d 0f bb e5 | btc r13, r12
X: 1acd8b 0f bc 75 00 | bsf esi, dword ptr [rbp]
X: 1acdab 0f bc c0 | bsf eax, eax
X: 1ad23a 67 0f bc ff | bsf edi, edi
X: 1adcf6 67 48 0f bc d3 | bsf rdx, rbx
X: 1ade60 0f bd 80 10 20 30 40 | bsr eax, dword ptr [rax + 0x40302010]
X: 1adf67 0f bd e8 | bsr ebp, eax
X: 1ae5b1 f2 0f bd 00 | bsr eax, dword ptr [rax]
X: 1ae6f2 66 67 0f bd 00 | bsr ax, word ptr [eax]
X: 1aefc7 0f be 1c e5 00 10 00 00 | movsx ebx, byte ptr [0x1000]
X: 1af066 0f be d8 | movsx ebx, al
X: 1afbce 48 0f be 45 fc | movsx rax, byte ptr [rbp - 0x4]
X: 1b00b4 0f bf 40 7f | movsx eax, word ptr [rax + 0x7f]
X: 1b0190 0f bf d0 | movsx edx, ax
X: 1b10ac 67 48 0f bf 40 7f | movsx rax, word ptr [eax + 0x7f]
X: 1b11c1 0f c0 04 24 | xadd byte ptr [rsp], al
X: 1b1345 0f c0 ff | xadd bh, bh
X: 1b1354 66 0f c0 00 | xadd byte ptr [rax], al
X: 1b1a43 64 0f c0 c1 | xadd cl, al
X: 1b1a74 66 67 0f c0 d8 | xadd al, bl
X: 1b238d 0f c1 1c e5 00 10 00 00 | xadd dword ptr [0x1000], ebx
X: 1b240e 0f c1 d0 | xadd eax, edx
X: 1b2682 66 0f c1 ff | xadd di, di
X: 1b291b f3 0f c1 84 88 00 01 00 00 | xadd dword ptr [rax + 4*rcx + 0x100], eax
X: 1b2a83 f2 0f c1 05 11 22 33 44 | xadd dword ptr [rip + 0x44332211], eax # 0x444e4c9c
X: 1b2b56 2e 0f c1 f0 | xadd eax, esi
DB: 1b5e51 f2 0f c4 e0 ff fe ff ff | db 0xf2
X: 1b8c11 0f c7 f8 | rdseed eax
X: 1b933a 64 0f c7 ff | rdseed edi
X: 1b9862 49 0f c7 1c e5 00 10 00 00 | xrstors64 [0x1000]
DB: 1b98ef 4d 0f c7 40 7f 11 22 33 | db 0x4d
X: 1b99a2 42 0f c7 0c 8d 10 00 00 00 | cmpxchg8b qword ptr [4*r9 + 0x10]
X: 1b9a9f 67 48 0f c7 5d 08 | xrstors64 [ebp + 0x8]
X: 1b9baa 0f c8 | bswap eax
X: 1ba8e7 48 0f c8 | bswap rax
X: 1baca5 f0 48 0f c8 | lock bswap rax
X: 1bad3a 0f c9 | bswap ecx
X: 1bb4b0 f2 0f c9 | bswap ecx
X: 1bb506 f0 0f c9 | lock bswap ecx
X: 1bb5ee 66 67 0f c9 | bswap cx
X: 1bb633 40 0f c9 | bswap ecx
X: 1bbbc9 4f 0f c9 | bswap r9
X: 1bbfd1 0f ca | bswap edx
X: 1bc9ee 44 0f ca | bswap edx
X: 1bca0f 48 0f ca | bswap rdx
X: 1bd079 0f cb | bswap ebx
X: 1bddf7 4d 0f cb | bswap r11
X: 1bdfa8 67 48 0f cb | bswap rbx
X: 1be153 0f cc | bswap esp
X: 1be602 67 0f cc | bswap esp
X: 1beb27 41 0f cc | bswap r12d
X: 1befec 42 0f cc | bswap esp
X: 1bf0e6 67 48 0f cc | bswap rsp
X: 1bf243 0f cd | bswap ebp
X: 1bfa1c 2e 0f cd | bswap ebp
X: 1c009f 4f 0f cd | bswap r13
X: 1c0114 42 0f cd | bswap ebp
X: 1c02f6 f0 48 0f cd | lock bswap rbp
X: 1c039a 0f ce | bswap esi
X: 1c07d4 67 0f ce | bswap esi
X: 1c0aa3 f2 0f ce | bswap esi
X: 1c0afe f0 0f ce | lock bswap esi
X: 1c1552 0f cf | bswap edi
X: 1c1b5f f3 0f cf | bswap edi
X: 1c1c69 f0 0f cf | lock bswap edi
X: 1c1cdc 2e 0f cf | bswap edi
X: 1c1f10 41 0f cf | bswap r15d
X: 1c21c9 48 0f cf | bswap rdi
X: 1c2322 4d 0f cf | bswap r15
X: 1c23e9 66 48 0f cf | bswap rdi
DB: 1c6fef 67 0f d4 e8 11 22 33 44 | db 0x67
DB: 1cafe4 0f d8 40 7f ff fe ff ff | db 0xf
DB: 1cea18 f3 0f db c8 11 22 33 44 | db 0xf3
DB: 1cf93b 67 0f dc c0 11 22 33 44 | db 0x67
DB: 1cff73 41 0f dc d8 11 22 33 44 | db 0x41
DB: 1d3261 41 0f df 1c e5 00 10 00 | db 0x41
DB: 1d6b36 66 48 0f e2 f6 ff fe ff | db 0x66
DB: 1d87a1 40 0f e4 f6 ff fe ff ff | db 0x40
DB: 1df657 f3 48 0f ea f6 11 22 33 | db 0xf3
DB: 1e1985 0f ed 44 24 08 11 22 33 | db 0xf
DB: 1e295b 66 41 0f ed 84 88 00 01 | db 0x66
DB: 1e3fdf 67 0f ef 44 24 08 11 22 | db 0x67
DB: 1ea106 4f 0f f4 d0 11 22 33 44 | db 0x4f
DB: 1eb56f 0f f6 c0 11 22 33 44 55 | db 0xf
DB: 1ed017 41 0f f7 45 fc 11 22 33 | db 0x41
DB: 1f1b48 f0 48 0f fb e5 11 22 33 | db 0xf0
X: 1f6a57 0f 0d 0c 11 | prefetchw byte ptr [rcx + rdx]
X: 1f6c85 0f c7 0f | cmpxchg8b qword ptr [rdi]
X: 1f6e94 0f 0d 11 | prefetchwt1 byte ptr [rcx]
X: 1f7255 0f ae 16 | ldmxcsr dword ptr [rsi]
X: 1f74e0 0f ae 19 | stmxcsr dword ptr [rcx]
X: 1f7673 0f 01 1b | lidt [rbx]
X: 1f7b89 0f 01 21 | smsw word ptr [rcx]
X: 1f81b6 0f c7 28 | xsaves [rax]
X: 1f84fb 0f ae 2c 11 | xrstor [rcx + rdx]
X: 1f91b2 0f ae 3b | clflush byte ptr [rbx]
X: 1f92e8 0f 18 3c 11 | prefetchit0 byte ptr [rcx + rdx]
X: 1f947b 0f ba 3e 11 | btc dword ptr [rsi], 0x11
X: 1f95ef 0f ae 40 11 | fxsave [rax + 0x11]
X: 1f964c 0f 18 40 11 | prefetchnta byte ptr [rax + 0x11]
X: 1f9f23 0f 01 4b 11 | sidt [rbx + 0x11]
X: 1fa22a 0f 18 4e 11 | prefetcht0 byte ptr [rsi + 0x11]
X: 1fa3dc 0f 18 50 11 | prefetcht1 byte ptr [rax + 0x11]
X: 1fa930 0f 00 57 11 | lldt word ptr [rdi + 0x11]
DB: 1fae27 0f 0d 5c 11 22 33 44 55 | db 0xf
X: 1fb2c1 0f ae 62 11 | xsave [rdx + 0x11]
X: 1fb3d8 0f ba 63 11 22 | bt dword ptr [rbx + 0x11], 0x22
X: 1fb3f7 0f 18 63 11 | prefetchrst2 byte ptr [rbx + 0x11]
X: 1fb799 0f 00 68 11 | verw word ptr [rax + 0x11]
DB: 1fc013 0f 00 72 11 22 33 44 55 | db 0xf
X: 1fc3d4 0f c7 76 11 | vmptrld qword ptr [rsi + 0x11]
X: 1fc621 0f 01 79 11 | invlpg byte ptr [rcx + 0x11]
X: 1fd392 0f 00 89 11 22 33 44 | str word ptr [rcx + 0x44332211]
X: 1fd8e6 0f ae 8f 11 22 33 44 | fxrstor [rdi + 0x44332211]
X: 1fdd04 0f 01 94 11 22 33 44 55 | lgdt [rcx + rdx + 0x55443322]
X: 1fe0a6 0f c7 98 11 22 33 44 | xrstors [rax + 0x44332211]
X: 1fe122 0f 00 99 11 22 33 44 | ltr word ptr [rcx + 0x44332211]
X: 1fe448 0f 18 9c 11 22 33 44 55 | prefetcht2 byte ptr [rcx + rdx + 0x55443322]
X: 1fea75 0f 00 a4 11 22 33 44 55 | verr word ptr [rcx + rdx + 0x55443322]
X: 1fec84 0f c7 a6 11 22 33 44 | xsavec [rsi + 0x44332211]
X: 1ff0e0 0f ba ab 11 22 33 44 55 | bts dword ptr [rbx + 0x44332211], 0x55
X: 1ff51d 0f ba b0 11 22 33 44 55 | btr dword ptr [rax + 0x44332211], 0x55
DB: 1ff7f5 48 0f 0d b3 11 22 33 44 | db 0x48
X: 1ff824 0f 01 b4 11 22 33 44 55 | lmsw word ptr [rcx + rdx + 0x55443322]
X: 1ff91c 0f ae b5 11 22 33 44 | xsaveopt [rbp + 0x44332211]
X: 1ffa52 0f 18 b6 11 22 33 44 | prefetchit1 byte ptr [rsi + 0x44332211]
X: 1ffbc6 0f c7 b8 11 22 33 44 | vmptrst qword ptr [rax + 0x44332211]
DB: 1ffde4 48 0f 0d ba 11 22 33 44 | db 0x48
X: 2002db 0f 01 c2 | vmlaunch
X: 2003b4 0f 01 c3 | vmresume
X: 20046e 0f 00 c4 | sldt esp
X: 20048d 0f 01 c4 | vmxoff
X: 200566 0f 01 c5 | pconfig
X: 20063f 0f 01 c6 | wrmsrns
X: 20082f 0f 01 c9 | mwait
X: 2008e9 0f 00 ca | str edx
X: 200908 0f 01 ca | clac
X: 2009e1 0f 01 cb | stac
X: 200d45 0f 01 cf | encls
X: 200e69 0f 01 d1 | xsetbv
X: 201058 0f 00 d4 | lldt sp
X: 201077 0f 01 d4 | vmfunc
X: 201150 0f 01 d5 | xend
X: 201229 0f 01 d6 | xtest
X: 201302 0f 01 d7 | enclu
X: 2013cb 0f 00 d9 | ltr cx
X: 2013ea 0f 01 d9 | vmmcall
X: 2014c3 0f 01 da | vmload
X: 20159c 0f 01 db | vmsave
X: 201675 0f 01 dc | stgi
X: 20174e 0f 01 dd | clgi
X: 201827 0f 01 de | skinit
X: 201900 0f 01 df | invlpga
X: 201d6a 0f 01 e5 | smsw ebp
DB: 201e13 48 0f c7 e6 11 22 33 44 | db 0x48
DB: 201eec 48 0f c7 e7 11 22 33 44 | db 0x48
X: 201fa6 0f ae e9 | lfence
X: 2023c4 0f 01 ee | rdpkru
X: 20247e 0f 00 ef | verw di
X: 20249d 0f 01 ef | wrpkru
X: 202775 0f 01 f3 | lmsw bx
X: 2027d2 0f ba f3 11 | btr ebx, 0x11
X: 202a7a 0f c7 f7 | rdrand edi
X: 202b52 0f 01 f9 | rdtscp
X: 202b71 0f ae f9 | sfence
X: 202c2b 0f 01 fa | monitorx
X: 202d04 0f 01 fb | mwaitx
X: 202ddd 0f 01 fc | clzero
X: 202eb6 0f 01 fd | rdpru
X: 202f13 0f ba fd 11 | btc ebp, 0x11
X: 202f8f 0f 01 fe | invlpgb
X: 20323c 4d dd 01 | esc 0x28, word ptr [r9]
X: 203b5e ff 0b | dec dword ptr [rbx]
X: 20448b ff 14 11 | call qword ptr [rcx + rdx]
X: 20479a ff 17 | call qword ptr [rdi]
X: 204ced 80 1c 11 22 | sbb byte ptr [rcx + rdx], 0x22
X: 205d03 ff 2c 11 | ljmp [rcx + rdx]
X: 205d3d 80 2c 11 22 | sub byte ptr [rcx + rdx], 0x22
X: 205d94 dd 2c 11 | esc 0x2d, word ptr [rcx + rdx]
X: 205f8f 4d d8 2e | esc 0x5, word ptr [r14]
X: 206256 80 31 11 | xor byte ptr [rcx], 0x11
X: 2075d2 c7 44 11 22 33 44 55 66 | mov dword ptr [rcx + rdx + 0x22], 0x66554433
X: 2076d7 c7 45 11 22 33 44 55 | mov dword ptr [rbp + 0x11], 0x55443322
X: 207ace 80 49 11 22 | or byte ptr [rcx + 0x11], 0x22
X: 207b33 4d dd 49 11 | esc 0x29, word ptr [r9 + 0x11]
X: 2088bd f7 57 11 | not dword ptr [rdi + 0x11]
X: 208aaa 89 59 11 | mov dword ptr [rcx + 0x11], ebx
X: 2095fe f7 64 11 22 | mul dword ptr [rcx + rdx + 0x22]
X: 20961b ff 64 11 22 | jmp qword ptr [rcx + rdx + 0x22]
X: 209b17 f7 69 11 | imul dword ptr [rcx + 0x11]
X: 209cbb 4d d8 6a 11 | esc 0x5, word ptr [r10 + 0x11]
X: 20a95d f7 77 11 | div dword ptr [rdi + 0x11]
X: 20b111 d8 7e 11 | esc 0x7, word ptr [rsi + 0x11]
X: 20b185 f7 7f 11 | idiv dword ptr [rdi + 0x11]
X: 20b6f5 80 84 11 22 33 44 55 66 | add byte ptr [rcx + rdx + 0x55443322], 0x66
X: 20b7a3 f7 85 11 22 33 44 55 66 77 88 | test dword ptr [rbp + 0x44332211], 0x88776655
X: 20b9ca ff 87 11 22 33 44 | inc dword ptr [rdi + 0x44332211]
X: 20be8c 8b 8c 11 22 33 44 55 | mov ecx, dword ptr [rcx + rdx + 0x55443322]
X: 20c640 80 93 11 22 33 44 55 | adc byte ptr [rbx + 0x44332211], 0x55
X: 20ce2e ff 9b 11 22 33 44 | lcall [rbx + 0x44332211]
X: 20d120 f7 9e 11 22 33 44 | neg dword ptr [rsi + 0x44332211]
X: 20d381 80 a0 11 22 33 44 55 | and byte ptr [rax + 0x44332211], 0x55
X: 20d44c ff a1 11 22 33 44 | jmp qword ptr [rcx + 0x44332211]
X: 20db8c 8d a8 11 22 33 44 | lea ebp, [rax + 0x44332211]
X: 20e5a1 ff b2 11 22 33 44 | push qword ptr [rdx + 0x44332211]
X: 20e8b0 ff b5 11 22 33 44 | push qword ptr [rbp + 0x44332211]
X: 20e8db 4d 8d b5 11 22 33 44 | lea r14, [r13 + 0x44332211]
X: 20ea29 d8 b6 11 22 33 44 | esc 0x6, word ptr [rsi + 0x44332211]
X: 20f00d 80 bc 11 22 33 44 55 66 | cmp byte ptr [rcx + rdx + 0x55443322], 0x66
X: 20f4f3 ff c2 | inc edx
X: 20f754 c7 c4 11 22 33 44 | mov esp, 0x44332211
X: 20f7e5 f7 c5 11 22 33 44 | test ebp, 0x44332211
X: 20f941 80 c6 11 | add dh, 0x11
X: 20fba5 ff c9 | dec ecx
X: 20feee 80 cc 11 | or ah, 0x11
X: 210280 89 d0 | mov eax, edx
X: 2102ac ff d0 | call rax
X: 2104b8 80 d2 11 | adc dl, 0x11
X: 2108b2 dd d6 | esc 0x2a, si
X: 210909 f7 d7 | not edi
X: 210a58 8b d9 | mov ebx, ecx
X: 210b5d 8b da | mov ebx, edx
X: 210b97 f7 da | neg edx
X: 211388 ff e2 | jmp rdx
X: 211886 80 e7 11 | and bh, 0x11
X: 211dea f7 ed | imul ebp
X: 211f46 80 ee 11 | sub dh, 0x11
X: 211fab 4d dd ee | esc 0x2d, r14w
X: 21219a ff f1 | push rcx
X: 212265 89 f2 | mov edx, esi
X: 212387 f7 f3 | div ebx
X: 2125e8 80 f5 11 | xor ch, 0x11
X: 212cad f7 fd | idiv ebp
X: 2156d9 4b 8b 84 6d 11 22 33 44 | mov rax, qword ptr [r13 + 2*r13 + 0x44332211]
X: 2181a8 8b 84 e3 11 22 33 44 | mov eax, dword ptr [rbx + 0x44332211]
X: 218c03 f3 0f 1e fb | endbr32

X86-16
S" disx64" .RESULTS
