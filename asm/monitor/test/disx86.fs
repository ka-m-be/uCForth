\ disx86.fs - tests of the package DISX86 (asm/monitor/disx86.fs)
\
\ Load (in the folder ucf), after the package FILES and the tester:
\   build/ucf-c host/files.fs forth/test/tester.fs asm/monitor/test/disx86.fs
\ The tests run with cells of 16 and of 32 bits.
\
\ Each line X: is one instruction: its address, its bytes, "|", and the
\ text that DECODE must give; the instruction must also end after its
\ bytes. The texts come from llvm-objdump 21 (Apple LLVM, the triple
\ i386-pc-none-code16, "-M intel"), once, and are kept here as data.
\ The bytes: those of the tests of the assembler I8086 (asm/test/8086.fs),
\ then single cases, then each opcode with several bytes ModR/M and with
\ the prefixes, two cases of each form. Where this disassembler writes
\ otherwise (asm/monitor/README.md, "x86"), the text here is changed:
\
\   - a byte without sign extension has no sign: "mov al, 0xff", not
\     "mov al, -0x1" (also ENTER and AAM);
\   - an address without register has no sign: "[0xffff]", not "[-0x1]";
\   - a shift by 1 shows the 1: "shl al, 1";
\   - CBW and CWD with 16-bit operands are cbw and cwd (LLVM: cwde, cdq);
\   - PUSHA, POPA, PUSHAD, POPAD (LLVM: pushaw, popaw, pushal, popal);
\   - XCHG of two registers: the field r/m first, as for memory;
\   - the segment of a far address has no sign;
\   - FF /3 and FF /5 are lcall and ljmp also with the prefix 66h (LLVM:
\     call, jmp);
\   - a jump with 16-bit operands goes to its target modulo 64 KB, as the
\     CPU does (LLVM counts on over 64 KB);
\   - LOCK is a prefix in the same line;
\   - no "addr32", no "eiz" (a byte SIB without index);
\   - F1h is int1 (LLVM shows no text).

ONLY FORTH DEFINITIONS  ALSO FILES
S" lib/text.fs" INCLUDED
ONLY FORTH DEFINITIONS  ALSO FILES
S" asm/monitor/monitor.fs" INCLUDED
S" asm/monitor/disx86.fs" INCLUDED
ONLY FORTH DEFINITIONS  ALSO INTERNALS  ALSO TEXT  ALSO MONITOR
DECIMAL

CREATE CODE-BUF 16 ALLOT   VARIABLE CODE-AT   VARIABLE CODE-U
: CODE-C@ ( a -- c )   CODE-AT @ -  DUP 16 U< IF CODE-BUF + C@ ELSE DROP $90 THEN ;
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

X: 0000 89 d8 | mov ax, bx
X: 0010 89 c3 | mov bx, ax
X: 0020 89 d1 | mov cx, dx
X: 0030 89 ec | mov sp, bp
X: 0040 89 fe | mov si, di
X: 0050 88 f8 | mov al, bh
X: 0060 8b 07 | mov ax, word ptr [bx]
X: 0070 8b 46 04 | mov ax, word ptr [bp + 0x4]
X: 0080 8b 56 00 | mov dx, word ptr [bp]
X: 0090 8b 4c fe | mov cx, word ptr [si - 0x2]
X: 00a0 8b 9d 00 01 | mov bx, word ptr [di + 0x100]
X: 00b0 8b 70 02 | mov si, word ptr [bx + si + 0x2]
X: 00c0 8b 39 | mov di, word ptr [bx + di]
X: 00d0 8b 02 | mov ax, word ptr [bp + si]
X: 00e0 8b 83 34 12 | mov ax, word ptr [bp + di + 0x1234]
X: 00f0 8b 44 7f | mov ax, word ptr [si + 0x7f]
X: 0100 8b 84 80 00 | mov ax, word ptr [si + 0x80]
X: 0110 8b 44 80 | mov ax, word ptr [si - 0x80]
X: 0120 a1 34 12 | mov ax, word ptr [0x1234]
X: 0130 a0 12 00 | mov al, byte ptr [0x12]
X: 0140 8b 1e 34 12 | mov bx, word ptr [0x1234]
X: 0150 8a 04 | mov al, byte ptr [si]
X: 0160 89 07 | mov word ptr [bx], ax
X: 0170 89 76 04 | mov word ptr [bp + 0x4], si
X: 0180 a3 34 12 | mov word ptr [0x1234], ax
X: 0190 a2 12 00 | mov byte ptr [0x12], al
X: 01a0 89 16 34 12 | mov word ptr [0x1234], dx
X: 01b0 88 0d | mov byte ptr [di], cl
X: 01c0 8e c0 | mov es, ax
X: 01d0 8e db | mov ds, bx
X: 01e0 8e d1 | mov ss, cx
X: 01f0 8e 47 04 | mov es, word ptr [bx + 0x4]
X: 0200 8c d8 | mov ax, ds
X: 0210 8c ca | mov dx, cs
X: 0220 8c 07 | mov word ptr [bx], es
X: 0230 b8 34 12 | mov ax, 0x1234
X: 0240 bb 00 00 | mov bx, 0x0
X: 0250 be ff ff | mov si, 0xffff
X: 0260 bf 02 00 | mov di, 0x2
X: 0270 b0 05 | mov al, 0x5
X: 0280 b7 80 | mov bh, 0x80
X: 0290 c7 07 07 00 | mov word ptr [bx], 0x7
X: 02a0 c6 05 07 | mov byte ptr [di], 0x7
X: 02b0 c7 81 34 12 78 56 | mov word ptr [bx + di + 0x1234], 0x5678
X: 02c0 c6 43 fd 07 | mov byte ptr [bp + di - 0x3], 0x7
X: 02d0 c7 06 06 01 00 00 | mov word ptr [0x106], 0x0
X: 02e0 01 d8 | add ax, bx
X: 02f0 09 d8 | or ax, bx
X: 0300 11 d8 | adc ax, bx
X: 0310 19 d8 | sbb ax, bx
X: 0320 21 d8 | and ax, bx
X: 0330 29 d8 | sub ax, bx
X: 0340 31 d8 | xor ax, bx
X: 0350 39 d8 | cmp ax, bx
X: 0360 00 c8 | add al, cl
X: 0370 30 de | xor dh, bl
X: 0380 03 0c | add cx, word ptr [si]
X: 0390 01 0c | add word ptr [si], cx
X: 03a0 3a 07 | cmp al, byte ptr [bx]
X: 03b0 38 07 | cmp byte ptr [bx], al
X: 03c0 2b 56 02 | sub dx, word ptr [bp + 0x2]
X: 03d0 01 07 | add word ptr [bx], ax
X: 03e0 2b 06 06 01 | sub ax, word ptr [0x106]
X: 03f0 83 c0 01 | add ax, 0x1
X: 0400 05 e8 03 | add ax, 0x3e8
X: 0410 81 c3 e8 03 | add bx, 0x3e8
X: 0420 83 c3 fe | add bx, -0x2
X: 0430 25 ff 00 | and ax, 0xff
X: 0440 3c 05 | cmp al, 0x5
X: 0450 80 fb 05 | cmp bl, 0x5
X: 0460 83 ff 7f | cmp di, 0x7f
X: 0470 81 ff 80 00 | cmp di, 0x80
X: 0480 83 c4 04 | add sp, 0x4
X: 0490 83 ed 06 | sub bp, 0x6
X: 04a0 83 07 01 | add word ptr [bx], 0x1
X: 04b0 80 07 01 | add byte ptr [bx], 0x1
X: 04c0 83 3e 06 01 00 | cmp word ptr [0x106], 0x0
X: 04d0 35 00 80 | xor ax, 0x8000
X: 04e0 80 e1 03 | and cl, 0x3
X: 04f0 83 d2 01 | adc dx, 0x1
X: 0500 83 da 00 | sbb dx, 0x0
X: 0510 80 cc 80 | or ah, 0x80
X: 0520 83 f9 10 | cmp cx, 0x10
X: 0530 24 7f | and al, 0x7f
X: 0540 83 f2 ff | xor dx, -0x1
X: 0550 85 d8 | test ax, bx
X: 0560 84 c0 | test al, al
X: 0570 85 07 | test word ptr [bx], ax
X: 0590 a9 00 80 | test ax, 0x8000
X: 05a0 a8 01 | test al, 0x1
X: 05b0 f6 c3 01 | test bl, 0x1
X: 05c0 f7 c1 03 00 | test cx, 0x3
X: 05d0 f6 07 01 | test byte ptr [bx], 0x1
X: 05e0 f7 d0 | not ax
X: 05f0 f7 d8 | neg ax
X: 0600 f7 e1 | mul cx
X: 0610 f7 e2 | mul dx
X: 0620 f7 eb | imul bx
X: 0630 f7 f1 | div cx
X: 0640 f7 fb | idiv bx
X: 0650 f6 d8 | neg al
X: 0660 f6 e3 | mul bl
X: 0670 f7 17 | not word ptr [bx]
X: 0680 f6 1f | neg byte ptr [bx]
X: 0690 40 | inc ax
X: 06a0 4f | dec di
X: 06b0 44 | inc sp
X: 06c0 fe c0 | inc al
X: 06d0 fe cd | dec ch
X: 06e0 ff 07 | inc word ptr [bx]
X: 06f0 fe 07 | inc byte ptr [bx]
X: 0700 ff 4e 02 | dec word ptr [bp + 0x2]
X: 0710 d1 e0 | shl ax, 1
X: 0720 d3 e0 | shl ax, cl
X: 0730 d1 ea | shr dx, 1
X: 0740 d1 f8 | sar ax, 1
X: 0750 d1 c3 | rol bx, 1
X: 0760 d1 c9 | ror cx, 1
X: 0770 d1 d2 | rcl dx, 1
X: 0780 d1 d8 | rcr ax, 1
X: 0790 d2 e8 | shr al, cl
X: 07a0 d3 ff | sar di, cl
X: 07b0 d1 3f | sar word ptr [bx], 1
X: 07c0 d2 27 | shl byte ptr [bx], cl
X: 07d0 50 | push ax
X: 07e0 57 | push di
X: 07f0 06 | push es
X: 0800 0e | push cs
X: 0810 16 | push ss
X: 0820 1e | push ds
X: 0830 ff 77 02 | push word ptr [bx + 0x2]
X: 0840 ff 76 00 | push word ptr [bp]
X: 0850 ff 36 06 01 | push word ptr [0x106]
X: 0860 58 | pop ax
X: 0870 5e | pop si
X: 0880 07 | pop es
X: 0890 1f | pop ds
X: 08a0 17 | pop ss
X: 08b0 8f 07 | pop word ptr [bx]
X: 08c0 93 | xchg ax, bx
X: 08e0 87 d9 | xchg cx, bx
X: 08f0 86 c3 | xchg bl, al
X: 0900 87 07 | xchg word ptr [bx], ax
X: 0920 8d 77 02 | lea si, [bx + 0x2]
X: 0930 8d 46 04 | lea ax, [bp + 0x4]
X: 0940 c4 3f | les di, [bx]
X: 0950 c5 37 | lds si, [bx]
X: 0960 e4 60 | in al, 0x60
X: 0970 e5 40 | in ax, 0x40
X: 0980 ec | in al, dx
X: 0990 ed | in ax, dx
X: 09a0 e6 43 | out 0x43, al
X: 09b0 e7 42 | out 0x42, ax
X: 09c0 ee | out dx, al
X: 09d0 ef | out dx, ax
X: 09e0 cd 21 | int 0x21
X: 09f0 cc | int3
X: 0a00 cd 1a | int 0x1a
X: 0a10 a4 | movsb byte ptr es:[di], byte ptr [si]
X: 0a20 a5 | movsw word ptr es:[di], word ptr [si]
X: 0a30 a6 | cmpsb byte ptr [si], byte ptr es:[di]
X: 0a40 a7 | cmpsw word ptr [si], word ptr es:[di]
X: 0a50 aa | stosb byte ptr es:[di], al
X: 0a60 ab | stosw word ptr es:[di], ax
X: 0a70 ac | lodsb al, byte ptr [si]
X: 0a80 ad | lodsw ax, word ptr [si]
X: 0a90 ae | scasb al, byte ptr es:[di]
X: 0aa0 af | scasw ax, word ptr es:[di]
X: 0ab0 f8 | clc
X: 0ac0 f9 | stc
X: 0ad0 f5 | cmc
X: 0ae0 fc | cld
X: 0af0 fd | std
X: 0b00 fa | cli
X: 0b10 fb | sti
X: 0b20 98 | cbw
X: 0b30 99 | cwd
X: 0b40 90 | nop
X: 0b50 f4 | hlt
X: 0b60 9f | lahf
X: 0b70 9e | sahf
X: 0b80 9c | pushf
X: 0b90 9d | popf
X: 0ba0 d7 | xlatb
X: 0bb0 c3 | ret
X: 0bc0 cb | retf
X: 0bd0 cf | iret
X: 0be0 f3 a4 | rep movsb byte ptr es:[di], byte ptr [si]
X: 0bf0 f3 ab | rep stosw word ptr es:[di], ax
X: 0c00 f2 ae | repne scasb al, byte ptr es:[di]
X: 0c10 f3 a6 | rep cmpsb byte ptr [si], byte ptr es:[di]
X: 0c20 2e 8b 07 | mov ax, word ptr cs:[bx]
X: 0c30 26 88 05 | mov byte ptr es:[di], al
X: 0c40 36 8b 04 | mov ax, word ptr ss:[si]
X: 0c50 3e 8b 46 00 | mov ax, word ptr ds:[bp]
X: 0c60 2e a1 20 01 | mov ax, word ptr cs:[0x120]
X: 0c70 ff 25 | jmp word ptr [di]
X: 0c80 2e ff a5 00 01 | jmp word ptr cs:[di + 0x100]
X: 0c90 ff e3 | jmp bx
X: 0ca0 ff 17 | call word ptr [bx]
X: 0cb0 ff d0 | call ax
X: 0cc0 ea 5b e0 00 f0 | ljmp 0xf000, 0xe05b
X: 0cd0 9a 78 56 34 12 | lcall 0x1234, 0x5678
X: 0ce0 ff 2f | ljmp [bx]
X: 0cf0 ff 58 02 | lcall [bx + si + 0x2]
X: 0d00 ff 6e fc | ljmp [bp - 0x4]
X: 0d10 ff 1e 34 12 | lcall [0x1234]
X: 0d20 ff ad 00 01 | ljmp [di + 0x100]
X: 0d30 83 c0 ff | add ax, -0x1
X: 0d40 05 ff ff | add ax, 0xffff
X: 0d50 b0 ff | mov al, 0xff
X: 0d60 6a ff | push -0x1
X: 0d70 68 34 12 | push 0x1234
X: 0d80 8b 46 fe | mov ax, word ptr [bp - 0x2]
X: 0d90 8b 86 fe ff | mov ax, word ptr [bp - 0x2]
X: 0da0 8b 06 34 12 | mov ax, word ptr [0x1234]
X: 0dc0 26 8b 07 | mov ax, word ptr es:[bx]
X: 0e40 91 | xchg ax, cx
X: 0e80 ce | into
X: 0eb0 ca 04 00 | retf 0x4
X: 0ec0 c2 04 00 | ret 0x4
X: 0f00 d0 e0 | shl al, 1
X: 0f20 c1 e0 05 | shl ax, 0x5
X: 0f30 f6 f3 | div bl
X: 0f40 f7 f3 | div bx
X: 0f50 f7 c3 34 12 | test bx, 0x1234
X: 0f70 ff c0 | inc ax
X: 0f90 ff 16 34 12 | call word ptr [0x1234]
X: 0fb0 8e d8 | mov ds, ax
X: 0fc0 c4 1e 34 12 | les bx, [0x1234]
X: 0ff0 66 b8 78 56 34 12 | mov eax, 0x12345678
X: 1000 67 8b 04 24 | mov ax, word ptr [esp]
X: 1010 0f b6 c0 | movzx ax, al
X: 1020 0f a2 | cpuid
X: 1030 0f 31 | rdtsc
X: 1040 0f 20 c0 | mov eax, cr0
X: 1050 0f 01 16 34 12 | lgdtw [0x1234]
X: 1060 2e ff 2f | ljmp cs:[bx]
X: 1070 0f 0b | ud2
X: 1080 f0 87 07 | lock xchg word ptr [bx], ax
X: 1090 6c | insb byte ptr es:[di], dx
X: 10a0 6d | insw word ptr es:[di], dx
X: 10b0 60 | pusha
X: 10c0 61 | popa
X: 10d0 c8 08 00 00 | enter 0x8, 0x0
X: 10e0 c9 | leave
X: 10f0 62 07 | bound ax, word ptr [bx]
X: 1100 69 c0 34 12 | imul ax, ax, 0x1234
X: 1110 6b c0 05 | imul ax, ax, 0x5
X: 1120 8f 06 34 12 | pop word ptr [0x1234]
X: 1130 a0 34 12 | mov al, byte ptr [0x1234]
X: 1150 27 | daa
X: 1160 d4 0a | aam
X: 1170 d4 08 | aam 0x8
X: 11a0 66 98 | cwde
X: 11b0 66 99 | cdq
X: 11c0 9b | wait
X: 11e0 64 8b 07 | mov ax, word ptr fs:[bx]
X: 11f0 63 07 | arpl word ptr [bx], ax
X: 1210 82 c0 05 | add al, 0x5
X: 1220 eb fe | jmp 0x1220
X: 1230 74 00 | je 0x1232
X: 1240 e8 00 00 | call 0x1243
X: 1250 e9 fd ff | jmp 0x1250
X: 1260 e2 fe | loop 0x1260
X: 1270 e3 00 | jcxz 0x1272
X: 1280 0f 84 00 00 | je 0x1284
X: 12b0 66 50 | push eax
X: 12c0 66 0f b6 c0 | movzx eax, al
X: 12e0 66 a5 | movsd dword ptr es:[di], dword ptr [si]
X: 1330 6e | outsb dx, byte ptr [si]
X: 1340 6f | outsw dx, word ptr [si]
X: 1350 c5 1e 34 12 | lds bx, [0x1234]
X: 1360 8c 06 34 12 | mov word ptr [0x1234], es
X: 1380 d6 | salc
X: 1390 0f 00 d8 | ltr ax
X: 13a0 0f 01 e0 | smsw ax
X: 13b0 0f ba e0 05 | bt ax, 0x5
X: 13c0 0f a3 c3 | bt bx, ax
X: 13d0 0f af c3 | imul ax, bx
X: 13e0 0f be c0 | movsx ax, al
X: 13f0 0f bf c3 | movsx ax, bx
X: 1400 0f 94 c0 | sete al
X: 1410 0f a0 | push fs
X: 1420 0f a9 | pop gs
X: 1430 0f b2 1e 34 12 | lss bx, [0x1234]
X: 1440 0f c8 | bswap ax
X: 1450 66 0f c8 | bswap eax
X: 1460 0f 40 c3 | cmovo ax, bx
X: 1470 0f c1 c3 | xadd bx, ax
X: 1480 0f b1 c3 | cmpxchg bx, ax
X: 1490 0f 06 | clts
X: 14a0 0f 09 | wbinvd
X: 14b0 0f 30 | wrmsr
X: 14c0 0f 32 | rdmsr
X: 14d0 0f a4 c3 05 | shld bx, ax, 0x5
X: 14e0 0f ad c3 | shrd bx, ax, cl
X: 14f0 0f bc c3 | bsf ax, bx
X: 1500 0f 02 c3 | lar ax, bx
X: 1510 0f 22 c0 | mov cr0, eax
X: 1520 0f 21 c0 | mov eax, dr0
X: 1530 8b 40 fe | mov ax, word ptr [bx + si - 0x2]
X: 1540 8b 80 34 12 | mov ax, word ptr [bx + si + 0x1234]
X: 1550 8b 00 | mov ax, word ptr [bx + si]
X: 1560 8b 01 | mov ax, word ptr [bx + di]
X: 1580 8b 03 | mov ax, word ptr [bp + di]
X: 1590 8b 04 | mov ax, word ptr [si]
X: 15a0 8b 05 | mov ax, word ptr [di]
X: 15c0 88 e0 | mov al, ah
X: 15d0 86 e0 | xchg al, ah
X: 15e0 87 d8 | xchg ax, bx
X: 1600 85 c0 | test ax, ax
X: 1610 a8 05 | test al, 0x5
X: 1620 a9 34 12 | test ax, 0x1234
X: 1640 3d 34 12 | cmp ax, 0x1234
X: 1650 04 05 | add al, 0x5
X: 1660 14 05 | adc al, 0x5
X: 1670 37 | aaa
X: 1680 3f | aas
X: 1690 2f | das
X: 16a0 d5 0a | aad
X: 16e0 cd ff | int 0xff
X: 16f0 e4 ff | in al, 0xff
X: 1700 e6 ff | out 0xff, al
X: 1710 c8 ff ff ff | enter 0xffff, 0xff
X: 1720 c2 ff ff | ret 0xffff
X: 1730 68 ff ff | push 0xffff
X: 1740 6a 80 | push -0x80
X: 1750 3c 80 | cmp al, 0x80
X: 1760 a8 ff | test al, 0xff
X: 1770 c6 05 ff | mov byte ptr [di], 0xff
X: 1780 80 07 ff | add byte ptr [bx], 0xff
X: 1790 c0 c0 ff | rol al, 0xff
X: 17a0 d4 ff | aam 0xff
X: 17b0 66 b8 ff ff ff ff | mov eax, 0xffffffff
X: 17c0 66 83 c0 ff | add eax, -0x1
X: 17d0 66 05 ff ff ff ff | add eax, 0xffffffff
X: 17e0 b8 ff ff | mov ax, 0xffff
X: 17f0 04 ff | add al, 0xff
X: 1800 80 c0 80 | add al, 0x80
X: 1810 83 c0 80 | add ax, -0x80
X: 1820 8b 46 80 | mov ax, word ptr [bp - 0x80]
X: 1830 8b 46 7f | mov ax, word ptr [bp + 0x7f]
X: 1840 8b 86 00 80 | mov ax, word ptr [bp - 0x8000]
X: 1850 8b 06 ff ff | mov ax, word ptr [0xffff]
X: 1860 c7 06 34 12 ff ff | mov word ptr [0x1234], 0xffff
X: 1870 6b c0 ff | imul ax, ax, -0x1
X: 1880 69 c0 ff ff | imul ax, ax, 0xffff
X: 1890 ff 26 34 12 | jmp word ptr [0x1234]
X: 18a0 ff 2e 34 12 | ljmp [0x1234]
X: 18c0 ff 36 34 12 | push word ptr [0x1234]
X: 18d0 fe 0f | dec byte ptr [bx]
X: 18f0 2e 90 | nop
X: 1900 f2 a4 | repne movsb byte ptr es:[di], byte ptr [si]
X: 1930 8b 46 00 | mov ax, word ptr [bp]
X: 1940 8b 40 00 | mov ax, word ptr [bx + si]
X: 1950 67 8b 00 | mov ax, word ptr [eax]
X: 1960 67 8b 04 8b | mov ax, word ptr [ebx + 4*ecx]
X: 1970 67 8b 44 8b 10 | mov ax, word ptr [ebx + 4*ecx + 0x10]
X: 1980 67 8b 04 25 34 12 00 00 | mov ax, word ptr [0x1234]
X: 1990 67 8b 05 34 12 00 00 | mov ax, word ptr [0x1234]
X: 19a0 67 8b 45 00 | mov ax, word ptr [ebp]
X: 19c0 67 8b 84 88 78 56 34 12 | mov ax, word ptr [eax + 4*ecx + 0x12345678]
X: 19d0 67 8b 04 4d 10 00 00 00 | mov ax, word ptr [2*ecx + 0x10]
X: 19e0 67 8b 0c 20 | mov cx, word ptr [eax]
X: 19f0 2e a4 | movsb byte ptr es:[di], byte ptr cs:[si]
X: 1a00 67 a4 | movsb byte ptr es:[edi], byte ptr [esi]
X: 1a10 26 ac | lodsb al, byte ptr es:[si]
X: 1a20 2e aa | stosb byte ptr es:[di], al
X: 1a50 66 60 | pushad
X: 1a60 66 61 | popad
X: 1a70 66 9c | pushfd
X: 1a80 66 cf | iretd
X: 1a90 66 6d | insd dword ptr es:[di], dx
X: 1aa0 66 a7 | cmpsd dword ptr [si], dword ptr es:[di]
X: 1ac0 f3 ae | rep scasb al, byte ptr es:[di]
X: 1ad0 f2 a6 | repne cmpsb byte ptr [si], byte ptr es:[di]
X: 1ae0 67 e3 00 | jecxz 0x1ae3
X: 1af0 3e 90 | nop
X: 1b00 26 f3 a4 | rep movsb byte ptr es:[di], byte ptr es:[si]
X: 1b10 f3 26 a4 | rep movsb byte ptr es:[di], byte ptr es:[si]
X: 1b20 66 0f 01 16 34 12 | lgdtd [0x1234]
X: 1b30 0f 01 3f | invlpg byte ptr [bx]
X: 1b40 0f 00 07 | sldt word ptr [bx]
X: 1b50 0f 01 26 34 12 | smsw word ptr [0x1234]
X: 1b60 0f 01 36 34 12 | lmsw word ptr [0x1234]
X: 1b70 8c 2e 34 12 | mov word ptr [0x1234], gs
X: 1b80 8e 26 34 12 | mov fs, word ptr [0x1234]
X: 1b90 f6 c0 05 | test al, 0x5
X: 1bc0 f7 18 | neg word ptr [bx + si]
X: 1bd0 fe 08 | dec byte ptr [bx + si]
X: 1be0 ff 18 | lcall [bx + si]
X: 1c10 c6 c0 05 | mov al, 0x5
X: 1c20 c7 c0 34 12 | mov ax, 0x1234
X: 1c30 8f c0 | pop ax
X: 1c40 0f ba 20 05 | bt word ptr [bx + si], 0x5
X: 1c50 0f ba f0 05 | btr ax, 0x5
X: 1c60 66 0f a4 c3 05 | shld ebx, eax, 0x5
X: 1c70 66 69 c0 ff ff ff ff | imul eax, eax, 0xffffffff
X: 1c80 66 6a ff | push -0x1
X: 1c90 66 68 78 56 34 12 | push 0x12345678
X: 1ca0 66 ea 78 56 34 12 00 f0 | ljmp 0xf000, 0x12345678
X: 1cb0 66 e8 00 00 00 00 | call 0x1cb6
X: 1cc0 66 e9 fb ff ff ff | jmp 0x1cc1
X: 1cd0 66 0f 84 00 00 00 00 | je 0x1cd7
X: 1ce0 66 c2 04 00 | ret 0x4
X: 1cf0 66 cb | retf
X: 1d60 00 11 | add byte ptr [bx + di], dl
X: 1d70 00 00 | add byte ptr [bx + si], al
X: 1de0 00 c0 | add al, al
X: 1fe0 01 c0 | add ax, ax
X: 2160 02 11 | add dl, byte ptr [bx + di]
X: 2170 02 00 | add al, byte ptr [bx + si]
X: 2360 03 11 | add dx, word ptr [bx + di]
X: 2d60 08 11 | or byte ptr [bx + di], dl
X: 2d70 08 00 | or byte ptr [bx + si], al
X: 2de0 08 c0 | or al, al
X: 2df0 08 c1 | or cl, al
X: 2f60 09 11 | or word ptr [bx + di], dx
X: 2f70 09 00 | or word ptr [bx + si], ax
X: 2fe0 09 c0 | or ax, ax
X: 3160 0a 11 | or dl, byte ptr [bx + di]
X: 3170 0a 00 | or al, byte ptr [bx + si]
X: 3360 0b 11 | or dx, word ptr [bx + di]
X: 3370 0b 00 | or ax, word ptr [bx + si]
X: 3560 0c 11 | or al, 0x11
X: 3760 0d 11 22 | or ax, 0x2211
X: 3770 0d 00 11 | or ax, 0x1100
X: 3b60 10 11 | adc byte ptr [bx + di], dl
X: 3b70 10 00 | adc byte ptr [bx + si], al
X: 3be0 10 c0 | adc al, al
X: 3bf0 10 c1 | adc cl, al
X: 3d60 11 11 | adc word ptr [bx + di], dx
X: 3d70 11 00 | adc word ptr [bx + si], ax
X: 3de0 11 c0 | adc ax, ax
X: 3f60 12 11 | adc dl, byte ptr [bx + di]
X: 3f70 12 00 | adc al, byte ptr [bx + si]
X: 4160 13 11 | adc dx, word ptr [bx + di]
X: 4170 13 00 | adc ax, word ptr [bx + si]
X: 4360 14 11 | adc al, 0x11
X: 4560 15 11 22 | adc ax, 0x2211
X: 4b60 18 11 | sbb byte ptr [bx + di], dl
X: 4b70 18 00 | sbb byte ptr [bx + si], al
X: 4be0 18 c0 | sbb al, al
X: 4bf0 18 c1 | sbb cl, al
X: 4d60 19 11 | sbb word ptr [bx + di], dx
X: 4d70 19 00 | sbb word ptr [bx + si], ax
X: 4de0 19 c0 | sbb ax, ax
X: 4f60 1a 11 | sbb dl, byte ptr [bx + di]
X: 4f70 1a 00 | sbb al, byte ptr [bx + si]
X: 5160 1b 11 | sbb dx, word ptr [bx + di]
X: 5170 1b 00 | sbb ax, word ptr [bx + si]
X: 5360 1c 11 | sbb al, 0x11
X: 5370 1c 00 | sbb al, 0x0
X: 5560 1d 11 22 | sbb ax, 0x2211
X: 5b60 20 11 | and byte ptr [bx + di], dl
X: 5b70 20 00 | and byte ptr [bx + si], al
X: 5be0 20 c0 | and al, al
X: 5bf0 20 c1 | and cl, al
X: 5d60 21 11 | and word ptr [bx + di], dx
X: 5d70 21 00 | and word ptr [bx + si], ax
X: 5de0 21 c0 | and ax, ax
X: 5f60 22 11 | and dl, byte ptr [bx + di]
X: 5f70 22 00 | and al, byte ptr [bx + si]
X: 6160 23 11 | and dx, word ptr [bx + di]
X: 6170 23 00 | and ax, word ptr [bx + si]
X: 6560 25 11 22 | and ax, 0x2211
X: 6960 28 11 | sub byte ptr [bx + di], dl
X: 6970 28 00 | sub byte ptr [bx + si], al
X: 69e0 28 c0 | sub al, al
X: 69f0 28 c1 | sub cl, al
X: 6b60 29 11 | sub word ptr [bx + di], dx
X: 6b70 29 00 | sub word ptr [bx + si], ax
X: 6be0 29 c0 | sub ax, ax
X: 6d60 2a 11 | sub dl, byte ptr [bx + di]
X: 6d70 2a 00 | sub al, byte ptr [bx + si]
X: 7160 2c 11 | sub al, 0x11
X: 7170 2c 00 | sub al, 0x0
X: 7360 2d 11 22 | sub ax, 0x2211
X: 7760 30 11 | xor byte ptr [bx + di], dl
X: 7770 30 00 | xor byte ptr [bx + si], al
X: 77e0 30 c0 | xor al, al
X: 7960 31 11 | xor word ptr [bx + di], dx
X: 7970 31 00 | xor word ptr [bx + si], ax
X: 79e0 31 c0 | xor ax, ax
X: 7b60 32 11 | xor dl, byte ptr [bx + di]
X: 7b70 32 00 | xor al, byte ptr [bx + si]
X: 7d60 33 11 | xor dx, word ptr [bx + di]
X: 7d70 33 00 | xor ax, word ptr [bx + si]
X: 7f60 34 11 | xor al, 0x11
X: 7f70 34 00 | xor al, 0x0
X: 8160 35 11 22 | xor ax, 0x2211
X: 8560 38 11 | cmp byte ptr [bx + di], dl
X: 85e0 38 c0 | cmp al, al
X: 85f0 38 c1 | cmp cl, al
X: 8760 39 11 | cmp word ptr [bx + di], dx
X: 8770 39 00 | cmp word ptr [bx + si], ax
X: 87e0 39 c0 | cmp ax, ax
X: 8960 3a 11 | cmp dl, byte ptr [bx + di]
X: 8b60 3b 11 | cmp dx, word ptr [bx + di]
X: 8b70 3b 00 | cmp ax, word ptr [bx + si]
X: a360 48 | dec ax
X: d760 62 11 | bound dx, word ptr [bx + di]
X: d960 63 11 | arpl word ptr [bx + di], dx
X: d9e0 63 c0 | arpl ax, ax
X: d9f0 63 c1 | arpl cx, ax
X: 02a0 69 11 22 33 | imul dx, word ptr [bx + di], 0x3322
X: 02b0 69 00 11 22 | imul ax, word ptr [bx + si], 0x2211
X: 07a0 6b fe ff | imul di, si, -0x1
X: 07b0 6b 00 fe | imul ax, word ptr [bx + si], -0x2
X: 07c0 6b 06 fe ff 80 | imul ax, word ptr [0xfffe], -0x80
X: 10a0 70 11 | jo 0x10b3
X: 10b0 70 00 | jo 0x10b2
X: 12a0 71 11 | jno 0x12b3
X: 12b0 71 00 | jno 0x12b2
X: 14a0 72 11 | jb 0x14b3
X: 14b0 72 00 | jb 0x14b2
X: 16a0 73 11 | jae 0x16b3
X: 16b0 73 00 | jae 0x16b2
X: 1aa0 75 11 | jne 0x1ab3
X: 1ab0 75 00 | jne 0x1ab2
X: 1ca0 76 11 | jbe 0x1cb3
X: 1cb0 76 00 | jbe 0x1cb2
X: 1ea0 77 11 | ja 0x1eb3
X: 1eb0 77 00 | ja 0x1eb2
X: 20a0 78 11 | js 0x20b3
X: 20b0 78 00 | js 0x20b2
X: 22a0 79 11 | jns 0x22b3
X: 22b0 79 00 | jns 0x22b2
X: 24a0 7a 11 | jp 0x24b3
X: 24b0 7a 00 | jp 0x24b2
X: 26a0 7b 11 | jnp 0x26b3
X: 26b0 7b 00 | jnp 0x26b2
X: 28a0 7c 11 | jl 0x28b3
X: 28b0 7c 00 | jl 0x28b2
X: 2aa0 7d 11 | jge 0x2ab3
X: 2ab0 7d 00 | jge 0x2ab2
X: 2ca0 7e 11 | jle 0x2cb3
X: 2cb0 7e 00 | jle 0x2cb2
X: 2ea0 7f 11 | jg 0x2eb3
X: 2eb0 7f 00 | jg 0x2eb2
X: 30a0 80 11 22 | adc byte ptr [bx + di], 0x22
X: 3110 80 5e 11 22 | sbb byte ptr [bp + 0x11], 0x22
X: 3170 80 3f 11 | cmp byte ptr [bx], 0x11
X: 3180 80 2e 11 22 33 | sub byte ptr [0x2211], 0x33
X: 3190 80 a6 11 22 33 | and byte ptr [bp + 0x2211], 0x33
X: 3210 80 5e fe ff | sbb byte ptr [bp - 0x2], 0xff
X: 3270 80 3f fe | cmp byte ptr [bx], 0xfe
X: 3280 80 2e fe ff 80 | sub byte ptr [0xfffe], 0x80
X: 3290 80 a6 fe ff 80 | and byte ptr [bp - 0x2], 0x80
X: 32a0 81 11 22 33 | adc word ptr [bx + di], 0x3322
X: 32b0 81 00 11 22 | add word ptr [bx + si], 0x2211
X: 3310 81 5e 11 22 33 | sbb word ptr [bp + 0x11], 0x3322
X: 3370 81 3f 11 22 | cmp word ptr [bx], 0x2211
X: 3380 81 2e 11 22 33 44 | sub word ptr [0x2211], 0x4433
X: 3390 81 a6 11 22 33 44 | and word ptr [bp + 0x2211], 0x4433
X: 3410 81 5e fe ff 80 | sbb word ptr [bp - 0x2], 0x80ff
X: 3480 81 2e fe ff 80 7f | sub word ptr [0xfffe], 0x7f80
X: 3490 81 a6 fe ff 80 7f | and word ptr [bp - 0x2], 0x7f80
X: 34a0 82 11 22 | adc byte ptr [bx + di], 0x22
X: 36a0 83 11 22 | adc word ptr [bx + di], 0x22
X: 37a0 83 fe ff | cmp si, -0x1
X: 37b0 83 00 fe | add word ptr [bx + si], -0x2
X: 37c0 83 06 fe ff 80 | add word ptr [0xfffe], -0x80
X: 3810 83 5e fe ff | sbb word ptr [bp - 0x2], -0x1
X: 3840 83 d8 fe | sbb ax, -0x2
X: 3850 83 e3 fe | and bx, -0x2
X: 3860 83 f9 fe | cmp cx, -0x2
X: 3870 83 3f fe | cmp word ptr [bx], -0x2
X: 3880 83 2e fe ff 80 | sub word ptr [0xfffe], -0x80
X: 3890 83 a6 fe ff 80 | and word ptr [bp - 0x2], -0x80
X: 38a0 84 11 | test byte ptr [bx + di], dl
X: 38b0 84 00 | test byte ptr [bx + si], al
X: 3930 84 c1 | test cl, al
X: 3aa0 85 11 | test word ptr [bx + di], dx
X: 3ca0 86 11 | xchg byte ptr [bx + di], dl
X: 3cb0 86 00 | xchg byte ptr [bx + si], al
X: 3ea0 87 11 | xchg word ptr [bx + di], dx
X: 48a0 8c 11 | mov word ptr [bx + di], ss
X: 4910 8c 5e 11 | mov word ptr [bp + 0x11], ds
X: 4920 8c c0 | mov ax, es
X: 4930 8c c1 | mov cx, es
X: 4950 8c e3 | mov bx, fs
X: 4980 8c 2e 11 22 | mov word ptr [0x2211], gs
X: 4990 8c a6 11 22 | mov word ptr [bp + 0x2211], fs
X: 4a10 8c 5e fe | mov word ptr [bp - 0x2], ds
X: 4a90 8c a6 fe ff | mov word ptr [bp - 0x2], fs
X: 4ca0 8e 11 | mov ss, word ptr [bx + di]
X: 4cb0 8e 00 | mov es, word ptr [bx + si]
X: 4d10 8e 5e 11 | mov ds, word ptr [bp + 0x11]
X: 4d30 8e c1 | mov es, cx
X: 4d50 8e e3 | mov fs, bx
X: 4d80 8e 2e 11 22 | mov gs, word ptr [0x2211]
X: 4d90 8e a6 11 22 | mov fs, word ptr [bp + 0x2211]
X: 4e10 8e 5e fe | mov ds, word ptr [bp - 0x2]
X: 4e80 8e 2e fe ff | mov gs, word ptr [0xfffe]
X: 64a0 9a 11 22 33 44 | lcall 0x4433, 0x2211
X: b0a0 c0 11 22 | rcl byte ptr [bx + di], 0x22
X: b0b0 c0 00 11 | rol byte ptr [bx + si], 0x11
X: b0c0 c0 06 11 22 33 | rol byte ptr [0x2211], 0x33
X: b110 c0 5e 11 22 | rcr byte ptr [bp + 0x11], 0x22
X: b120 c0 c0 11 | rol al, 0x11
X: b140 c0 d8 11 | rcr al, 0x11
X: b150 c0 e3 11 | shl bl, 0x11
X: b160 c0 f9 11 | sar cl, 0x11
X: b170 c0 3f 11 | sar byte ptr [bx], 0x11
X: b180 c0 2e 11 22 33 | shr byte ptr [0x2211], 0x33
X: b190 c0 a6 11 22 33 | shl byte ptr [bp + 0x2211], 0x33
X: b1a0 c0 fe ff | sar dh, 0xff
X: b210 c0 5e fe ff | rcr byte ptr [bp - 0x2], 0xff
X: b240 c0 d8 fe | rcr al, 0xfe
X: b250 c0 e3 fe | shl bl, 0xfe
X: b270 c0 3f fe | sar byte ptr [bx], 0xfe
X: b280 c0 2e fe ff 80 | shr byte ptr [0xfffe], 0x80
X: b290 c0 a6 fe ff 80 | shl byte ptr [bp - 0x2], 0x80
X: b2a0 c1 11 22 | rcl word ptr [bx + di], 0x22
X: b2b0 c1 00 11 | rol word ptr [bx + si], 0x11
X: b2c0 c1 06 11 22 33 | rol word ptr [0x2211], 0x33
X: b310 c1 5e 11 22 | rcr word ptr [bp + 0x11], 0x22
X: b320 c1 c0 11 | rol ax, 0x11
X: b330 c1 c1 11 | rol cx, 0x11
X: b340 c1 d8 11 | rcr ax, 0x11
X: b350 c1 e3 11 | shl bx, 0x11
X: b360 c1 f9 11 | sar cx, 0x11
X: b370 c1 3f 11 | sar word ptr [bx], 0x11
X: b380 c1 2e 11 22 33 | shr word ptr [0x2211], 0x33
X: b390 c1 a6 11 22 33 | shl word ptr [bp + 0x2211], 0x33
X: b3a0 c1 fe ff | sar si, 0xff
X: b410 c1 5e fe ff | rcr word ptr [bp - 0x2], 0xff
X: b440 c1 d8 fe | rcr ax, 0xfe
X: b470 c1 3f fe | sar word ptr [bx], 0xfe
X: b480 c1 2e fe ff 80 | shr word ptr [0xfffe], 0x80
X: b490 c1 a6 fe ff 80 | shl word ptr [bp - 0x2], 0x80
X: c4a0 ca 11 22 | retf 0x2211
X: d0a0 d0 11 | rcl byte ptr [bx + di], 1
X: d0b0 d0 00 | rol byte ptr [bx + si], 1
X: d0c0 d0 06 11 22 | rol byte ptr [0x2211], 1
X: d110 d0 5e 11 | rcr byte ptr [bp + 0x11], 1
X: d120 d0 c0 | rol al, 1
X: d130 d0 c1 | rol cl, 1
X: d140 d0 d8 | rcr al, 1
X: d150 d0 e3 | shl bl, 1
X: d160 d0 f9 | sar cl, 1
X: d170 d0 3f | sar byte ptr [bx], 1
X: d180 d0 2e 11 22 | shr byte ptr [0x2211], 1
X: d190 d0 a6 11 22 | shl byte ptr [bp + 0x2211], 1
X: d1a0 d0 fe | sar dh, 1
X: d210 d0 5e fe | rcr byte ptr [bp - 0x2], 1
X: d280 d0 2e fe ff | shr byte ptr [0xfffe], 1
X: d290 d0 a6 fe ff | shl byte ptr [bp - 0x2], 1
X: d2a0 d1 11 | rcl word ptr [bx + di], 1
X: d2b0 d1 00 | rol word ptr [bx + si], 1
X: d2c0 d1 06 11 22 | rol word ptr [0x2211], 1
X: d310 d1 5e 11 | rcr word ptr [bp + 0x11], 1
X: d320 d1 c0 | rol ax, 1
X: d350 d1 e3 | shl bx, 1
X: d360 d1 f9 | sar cx, 1
X: d380 d1 2e 11 22 | shr word ptr [0x2211], 1
X: d390 d1 a6 11 22 | shl word ptr [bp + 0x2211], 1
X: d410 d1 5e fe | rcr word ptr [bp - 0x2], 1
X: d480 d1 2e fe ff | shr word ptr [0xfffe], 1
X: d490 d1 a6 fe ff | shl word ptr [bp - 0x2], 1
X: d4a0 d2 11 | rcl byte ptr [bx + di], cl
X: d4b0 d2 00 | rol byte ptr [bx + si], cl
X: d4c0 d2 06 11 22 | rol byte ptr [0x2211], cl
X: d510 d2 5e 11 | rcr byte ptr [bp + 0x11], cl
X: d520 d2 c0 | rol al, cl
X: d530 d2 c1 | rol cl, cl
X: d540 d2 d8 | rcr al, cl
X: d550 d2 e3 | shl bl, cl
X: d560 d2 f9 | sar cl, cl
X: d570 d2 3f | sar byte ptr [bx], cl
X: d580 d2 2e 11 22 | shr byte ptr [0x2211], cl
X: d590 d2 a6 11 22 | shl byte ptr [bp + 0x2211], cl
X: d5a0 d2 fe | sar dh, cl
X: d610 d2 5e fe | rcr byte ptr [bp - 0x2], cl
X: d680 d2 2e fe ff | shr byte ptr [0xfffe], cl
X: d6a0 d3 11 | rcl word ptr [bx + di], cl
X: d6b0 d3 00 | rol word ptr [bx + si], cl
X: d6c0 d3 06 11 22 | rol word ptr [0x2211], cl
X: d710 d3 5e 11 | rcr word ptr [bp + 0x11], cl
X: d720 d3 c0 | rol ax, cl
X: d730 d3 c1 | rol cx, cl
X: d740 d3 d8 | rcr ax, cl
X: d750 d3 e3 | shl bx, cl
X: d760 d3 f9 | sar cx, cl
X: d770 d3 3f | sar word ptr [bx], cl
X: d780 d3 2e 11 22 | shr word ptr [0x2211], cl
X: d790 d3 a6 11 22 | shl word ptr [bp + 0x2211], cl
X: d810 d3 5e fe | rcr word ptr [bp - 0x2], cl
X: d880 d3 2e fe ff | shr word ptr [0xfffe], cl
X: d890 d3 a6 fe ff | shl word ptr [bp - 0x2], cl
X: daa0 d5 11 | aad 0x11
X: dab0 d5 00 | aad 0x0
X: 05e0 e0 11 | loopne 0x5f3
X: 05f0 e0 00 | loopne 0x5f2
X: 07e0 e1 11 | loope 0x7f3
X: 07f0 e1 00 | loope 0x7f2
X: 09e0 e2 11 | loop 0x9f3
X: 0be0 e3 11 | jcxz 0xbf3
X: 0fe0 e5 11 | in ax, 0x11
X: 13e0 e7 11 | out 0x11, ax
X: 2be0 f6 11 | not byte ptr [bx + di]
X: 2bf0 f6 00 11 | test byte ptr [bx + si], 0x11
X: 2c50 f6 5e 11 | neg byte ptr [bp + 0x11]
X: 2ca0 f6 f9 | idiv cl
X: 2cb0 f6 3f | idiv byte ptr [bx]
X: 2cc0 f6 2e 11 22 | imul byte ptr [0x2211]
X: 2cd0 f6 a6 11 22 | mul byte ptr [bp + 0x2211]
X: 2ce0 f6 fe | idiv dh
X: 2dc0 f6 2e fe ff | imul byte ptr [0xfffe]
X: 2dd0 f6 a6 fe ff | mul byte ptr [bp - 0x2]
X: 2de0 f7 11 | not word ptr [bx + di]
X: 2df0 f7 00 11 22 | test word ptr [bx + si], 0x2211
X: 2e00 f7 06 11 22 33 44 | test word ptr [0x2211], 0x4433
X: 2e50 f7 5e 11 | neg word ptr [bp + 0x11]
X: 2ea0 f7 f9 | idiv cx
X: 2eb0 f7 3f | idiv word ptr [bx]
X: 2ec0 f7 2e 11 22 | imul word ptr [0x2211]
X: 2ed0 f7 a6 11 22 | mul word ptr [bp + 0x2211]
X: 2fc0 f7 2e fe ff | imul word ptr [0xfffe]
X: 2fd0 f7 a6 fe ff | mul word ptr [bp - 0x2]
X: 3bf0 fe 00 | inc byte ptr [bx + si]
X: 3c70 fe c1 | inc cl
X: 3df0 ff 00 | inc word ptr [bx + si]
X: 3fe0 0f 00 00 | sldt word ptr [bx + si]
X: 4000 0f 00 16 05 34 | lldt word ptr [0x3405]
X: 4010 0f 00 26 05 34 | verr word ptr [0x3405]
X: 4050 0f 00 c0 | sldt ax
X: 4060 0f 00 e1 | verr cx
X: 4070 0f 00 d3 | lldt bx
X: 40a0 0f 01 00 | sgdtw [bx + si]
X: 40b0 0f 01 06 05 34 | sgdtw [0x3405]
X: 40c0 0f 01 16 05 34 | lgdtw [0x3405]
X: 40d0 0f 01 26 05 34 | smsw word ptr [0x3405]
X: 40e0 0f 01 36 05 34 | lmsw word ptr [0x3405]
X: 40f0 0f 01 3e 05 34 | invlpg byte ptr [0x3405]
X: 4120 0f 01 e1 | smsw cx
X: 4160 0f 02 00 | lar ax, word ptr [bx + si]
X: 4170 0f 02 06 05 34 | lar ax, word ptr [0x3405]
X: 41d0 0f 02 c0 | lar ax, ax
X: 4220 0f 03 00 | lsl ax, word ptr [bx + si]
X: 4230 0f 03 06 05 34 | lsl ax, word ptr [0x3405]
X: 4290 0f 03 c0 | lsl ax, ax
X: 42a0 0f 03 e1 | lsl sp, cx
X: 43a0 0f 08 | invd
X: 4660 0f 20 e1 | mov ecx, cr4
X: 4670 0f 20 d3 | mov ebx, cr2
X: 4680 0f 20 f8 | mov eax, cr7
X: 4720 0f 21 e1 | mov ecx, dr4
X: 4730 0f 21 d3 | mov ebx, dr2
X: 4740 0f 21 f8 | mov eax, dr7
X: 47e0 0f 22 e1 | mov cr4, ecx
X: 47f0 0f 22 d3 | mov cr2, ebx
X: 4800 0f 22 f8 | mov cr7, eax
X: 4890 0f 23 c0 | mov dr0, eax
X: 48a0 0f 23 e1 | mov dr4, ecx
X: 48b0 0f 23 d3 | mov dr2, ebx
X: 48c0 0f 23 f8 | mov dr7, eax
X: 4b20 0f 33 | rdpmc
X: 4be0 0f 40 00 | cmovo ax, word ptr [bx + si]
X: 4bf0 0f 40 06 05 34 | cmovo ax, word ptr [0x3405]
X: 4c50 0f 40 c0 | cmovo ax, ax
X: 4ca0 0f 41 00 | cmovno ax, word ptr [bx + si]
X: 4cb0 0f 41 06 05 34 | cmovno ax, word ptr [0x3405]
X: 4d10 0f 41 c0 | cmovno ax, ax
X: 4d20 0f 41 e1 | cmovno sp, cx
X: 4d60 0f 42 00 | cmovb ax, word ptr [bx + si]
X: 4d70 0f 42 06 05 34 | cmovb ax, word ptr [0x3405]
X: 4dd0 0f 42 c0 | cmovb ax, ax
X: 4de0 0f 42 e1 | cmovb sp, cx
X: 4e20 0f 43 00 | cmovae ax, word ptr [bx + si]
X: 4e30 0f 43 06 05 34 | cmovae ax, word ptr [0x3405]
X: 4e90 0f 43 c0 | cmovae ax, ax
X: 4ea0 0f 43 e1 | cmovae sp, cx
X: 4ee0 0f 44 00 | cmove ax, word ptr [bx + si]
X: 4ef0 0f 44 06 05 34 | cmove ax, word ptr [0x3405]
X: 4f50 0f 44 c0 | cmove ax, ax
X: 4f60 0f 44 e1 | cmove sp, cx
X: 4fa0 0f 45 00 | cmovne ax, word ptr [bx + si]
X: 4fb0 0f 45 06 05 34 | cmovne ax, word ptr [0x3405]
X: 5010 0f 45 c0 | cmovne ax, ax
X: 5020 0f 45 e1 | cmovne sp, cx
X: 5060 0f 46 00 | cmovbe ax, word ptr [bx + si]
X: 5070 0f 46 06 05 34 | cmovbe ax, word ptr [0x3405]
X: 50d0 0f 46 c0 | cmovbe ax, ax
X: 50e0 0f 46 e1 | cmovbe sp, cx
X: 5120 0f 47 00 | cmova ax, word ptr [bx + si]
X: 5130 0f 47 06 05 34 | cmova ax, word ptr [0x3405]
X: 5190 0f 47 c0 | cmova ax, ax
X: 51a0 0f 47 e1 | cmova sp, cx
X: 51e0 0f 48 00 | cmovs ax, word ptr [bx + si]
X: 51f0 0f 48 06 05 34 | cmovs ax, word ptr [0x3405]
X: 5250 0f 48 c0 | cmovs ax, ax
X: 5260 0f 48 e1 | cmovs sp, cx
X: 52a0 0f 49 00 | cmovns ax, word ptr [bx + si]
X: 52b0 0f 49 06 05 34 | cmovns ax, word ptr [0x3405]
X: 5310 0f 49 c0 | cmovns ax, ax
X: 5320 0f 49 e1 | cmovns sp, cx
X: 5360 0f 4a 00 | cmovp ax, word ptr [bx + si]
X: 5370 0f 4a 06 05 34 | cmovp ax, word ptr [0x3405]
X: 53d0 0f 4a c0 | cmovp ax, ax
X: 53e0 0f 4a e1 | cmovp sp, cx
X: 5420 0f 4b 00 | cmovnp ax, word ptr [bx + si]
X: 5430 0f 4b 06 05 34 | cmovnp ax, word ptr [0x3405]
X: 5490 0f 4b c0 | cmovnp ax, ax
X: 54a0 0f 4b e1 | cmovnp sp, cx
X: 54e0 0f 4c 00 | cmovl ax, word ptr [bx + si]
X: 54f0 0f 4c 06 05 34 | cmovl ax, word ptr [0x3405]
X: 5550 0f 4c c0 | cmovl ax, ax
X: 5560 0f 4c e1 | cmovl sp, cx
X: 55a0 0f 4d 00 | cmovge ax, word ptr [bx + si]
X: 55b0 0f 4d 06 05 34 | cmovge ax, word ptr [0x3405]
X: 5610 0f 4d c0 | cmovge ax, ax
X: 5620 0f 4d e1 | cmovge sp, cx
X: 5660 0f 4e 00 | cmovle ax, word ptr [bx + si]
X: 5670 0f 4e 06 05 34 | cmovle ax, word ptr [0x3405]
X: 56d0 0f 4e c0 | cmovle ax, ax
X: 56e0 0f 4e e1 | cmovle sp, cx
X: 5720 0f 4f 00 | cmovg ax, word ptr [bx + si]
X: 5730 0f 4f 06 05 34 | cmovg ax, word ptr [0x3405]
X: 5790 0f 4f c0 | cmovg ax, ax
X: 57a0 0f 4f e1 | cmovg sp, cx
X: 63e0 0f 90 00 | seto byte ptr [bx + si]
X: 63f0 0f 90 06 05 34 | seto byte ptr [0x3405]
X: 6450 0f 90 c0 | seto al
X: 6460 0f 90 e1 | seto cl
X: 64a0 0f 91 00 | setno byte ptr [bx + si]
X: 64b0 0f 91 06 05 34 | setno byte ptr [0x3405]
X: 6510 0f 91 c0 | setno al
X: 6520 0f 91 e1 | setno cl
X: 6560 0f 92 00 | setb byte ptr [bx + si]
X: 6570 0f 92 06 05 34 | setb byte ptr [0x3405]
X: 65d0 0f 92 c0 | setb al
X: 65e0 0f 92 e1 | setb cl
X: 6620 0f 93 00 | setae byte ptr [bx + si]
X: 6630 0f 93 06 05 34 | setae byte ptr [0x3405]
X: 6690 0f 93 c0 | setae al
X: 66a0 0f 93 e1 | setae cl
X: 66e0 0f 94 00 | sete byte ptr [bx + si]
X: 66f0 0f 94 06 05 34 | sete byte ptr [0x3405]
X: 6760 0f 94 e1 | sete cl
X: 67a0 0f 95 00 | setne byte ptr [bx + si]
X: 67b0 0f 95 06 05 34 | setne byte ptr [0x3405]
X: 6810 0f 95 c0 | setne al
X: 6820 0f 95 e1 | setne cl
X: 6860 0f 96 00 | setbe byte ptr [bx + si]
X: 6870 0f 96 06 05 34 | setbe byte ptr [0x3405]
X: 68d0 0f 96 c0 | setbe al
X: 68e0 0f 96 e1 | setbe cl
X: 6920 0f 97 00 | seta byte ptr [bx + si]
X: 6930 0f 97 06 05 34 | seta byte ptr [0x3405]
X: 6990 0f 97 c0 | seta al
X: 69a0 0f 97 e1 | seta cl
X: 69e0 0f 98 00 | sets byte ptr [bx + si]
X: 69f0 0f 98 06 05 34 | sets byte ptr [0x3405]
X: 6a50 0f 98 c0 | sets al
X: 6a60 0f 98 e1 | sets cl
X: 6aa0 0f 99 00 | setns byte ptr [bx + si]
X: 6ab0 0f 99 06 05 34 | setns byte ptr [0x3405]
X: 6b10 0f 99 c0 | setns al
X: 6b20 0f 99 e1 | setns cl
X: 6b60 0f 9a 00 | setp byte ptr [bx + si]
X: 6b70 0f 9a 06 05 34 | setp byte ptr [0x3405]
X: 6bd0 0f 9a c0 | setp al
X: 6be0 0f 9a e1 | setp cl
X: 6c20 0f 9b 00 | setnp byte ptr [bx + si]
X: 6c30 0f 9b 06 05 34 | setnp byte ptr [0x3405]
X: 6c90 0f 9b c0 | setnp al
X: 6ca0 0f 9b e1 | setnp cl
X: 6ce0 0f 9c 00 | setl byte ptr [bx + si]
X: 6cf0 0f 9c 06 05 34 | setl byte ptr [0x3405]
X: 6d50 0f 9c c0 | setl al
X: 6d60 0f 9c e1 | setl cl
X: 6da0 0f 9d 00 | setge byte ptr [bx + si]
X: 6db0 0f 9d 06 05 34 | setge byte ptr [0x3405]
X: 6e10 0f 9d c0 | setge al
X: 6e20 0f 9d e1 | setge cl
X: 6e60 0f 9e 00 | setle byte ptr [bx + si]
X: 6e70 0f 9e 06 05 34 | setle byte ptr [0x3405]
X: 6ed0 0f 9e c0 | setle al
X: 6ee0 0f 9e e1 | setle cl
X: 6f20 0f 9f 00 | setg byte ptr [bx + si]
X: 6f30 0f 9f 06 05 34 | setg byte ptr [0x3405]
X: 6f90 0f 9f c0 | setg al
X: 6fa0 0f 9f e1 | setg cl
X: 70a0 0f a1 | pop fs
X: 7220 0f a3 00 | bt word ptr [bx + si], ax
X: 7230 0f a3 06 05 34 | bt word ptr [0x3405], ax
X: 7290 0f a3 c0 | bt ax, ax
X: 72e0 0f a4 00 05 | shld word ptr [bx + si], ax, 0x5
X: 72f0 0f a4 06 05 34 12 | shld word ptr [0x3405], ax, 0x12
X: 73a0 0f a5 00 | shld word ptr [bx + si], ax, cl
X: 73b0 0f a5 06 05 34 | shld word ptr [0x3405], ax, cl
X: 7410 0f a5 c0 | shld ax, ax, cl
X: 7420 0f a5 e1 | shld cx, sp, cl
X: 7460 0f a8 | push gs
X: 75e0 0f ab 00 | bts word ptr [bx + si], ax
X: 75f0 0f ab 06 05 34 | bts word ptr [0x3405], ax
X: 7650 0f ab c0 | bts ax, ax
X: 7660 0f ab e1 | bts cx, sp
X: 76a0 0f ac 00 05 | shrd word ptr [bx + si], ax, 0x5
X: 76b0 0f ac 06 05 34 12 | shrd word ptr [0x3405], ax, 0x12
X: 7710 0f ac c0 05 | shrd ax, ax, 0x5
X: 7720 0f ac e1 05 | shrd cx, sp, 0x5
X: 7760 0f ad 00 | shrd word ptr [bx + si], ax, cl
X: 7770 0f ad 06 05 34 | shrd word ptr [0x3405], ax, cl
X: 77d0 0f ad c0 | shrd ax, ax, cl
X: 7820 0f af 00 | imul ax, word ptr [bx + si]
X: 7830 0f af 06 05 34 | imul ax, word ptr [0x3405]
X: 7890 0f af c0 | imul ax, ax
X: 78e0 0f b0 00 | cmpxchg byte ptr [bx + si], al
X: 78f0 0f b0 06 05 34 | cmpxchg byte ptr [0x3405], al
X: 7950 0f b0 c0 | cmpxchg al, al
X: 7960 0f b0 e1 | cmpxchg cl, ah
X: 79a0 0f b1 00 | cmpxchg word ptr [bx + si], ax
X: 79b0 0f b1 06 05 34 | cmpxchg word ptr [0x3405], ax
X: 7a10 0f b1 c0 | cmpxchg ax, ax
X: 7a60 0f b2 00 | lss ax, [bx + si]
X: 7b20 0f b3 00 | btr word ptr [bx + si], ax
X: 7b30 0f b3 06 05 34 | btr word ptr [0x3405], ax
X: 7b90 0f b3 c0 | btr ax, ax
X: 7ba0 0f b3 e1 | btr cx, sp
X: 7be0 0f b4 00 | lfs ax, [bx + si]
X: 7bf0 0f b4 06 05 34 | lfs ax, [0x3405]
X: 7ca0 0f b5 00 | lgs ax, [bx + si]
X: 7cb0 0f b5 06 05 34 | lgs ax, [0x3405]
X: 7d60 0f b6 00 | movzx ax, byte ptr [bx + si]
X: 7d70 0f b6 06 05 34 | movzx ax, byte ptr [0x3405]
X: 7e20 0f b7 00 | movzx ax, word ptr [bx + si]
X: 7e30 0f b7 06 05 34 | movzx ax, word ptr [0x3405]
X: 7e90 0f b7 c0 | movzx ax, ax
X: 7ea0 0f b7 e1 | movzx sp, cx
X: 7f10 0f ba 26 05 34 12 | bt word ptr [0x3405], 0x12
X: 7f20 0f ba 36 05 34 12 | btr word ptr [0x3405], 0x12
X: 7f30 0f ba 3e 05 34 12 | btc word ptr [0x3405], 0x12
X: 7f60 0f ba e1 05 | bt cx, 0x5
X: 7f80 0f ba f8 05 | btc ax, 0x5
X: 7f90 0f ba 7f 05 34 | btc word ptr [bx + 0x5], 0x34
X: 7fa0 0f bb 00 | btc word ptr [bx + si], ax
X: 7fb0 0f bb 06 05 34 | btc word ptr [0x3405], ax
X: 8010 0f bb c0 | btc ax, ax
X: 8020 0f bb e1 | btc cx, sp
X: 8060 0f bc 00 | bsf ax, word ptr [bx + si]
X: 8070 0f bc 06 05 34 | bsf ax, word ptr [0x3405]
X: 80d0 0f bc c0 | bsf ax, ax
X: 8120 0f bd 00 | bsr ax, word ptr [bx + si]
X: 8130 0f bd 06 05 34 | bsr ax, word ptr [0x3405]
X: 8190 0f bd c0 | bsr ax, ax
X: 81a0 0f bd e1 | bsr sp, cx
X: 81e0 0f be 00 | movsx ax, byte ptr [bx + si]
X: 81f0 0f be 06 05 34 | movsx ax, byte ptr [0x3405]
X: 8260 0f be e1 | movsx sp, cl
X: 82a0 0f bf 00 | movsx ax, word ptr [bx + si]
X: 82b0 0f bf 06 05 34 | movsx ax, word ptr [0x3405]
X: 8310 0f bf c0 | movsx ax, ax
X: 8360 0f c0 00 | xadd byte ptr [bx + si], al
X: 8370 0f c0 06 05 34 | xadd byte ptr [0x3405], al
X: 83d0 0f c0 c0 | xadd al, al
X: 83e0 0f c0 e1 | xadd cl, ah
X: 8420 0f c1 00 | xadd word ptr [bx + si], ax
X: 8430 0f c1 06 05 34 | xadd word ptr [0x3405], ax
X: 8490 0f c1 c0 | xadd ax, ax
X: 8ae0 66 01 00 | add dword ptr [bx + si], eax
X: 8af0 66 01 04 | add dword ptr [si], eax
X: 8b90 66 03 00 | add eax, dword ptr [bx + si]
X: 8ba0 66 03 04 | add eax, dword ptr [si]
X: 91c0 66 81 00 88 11 22 33 | add dword ptr [bx + si], 0x33221188
X: 91d0 66 81 04 88 11 22 33 | add dword ptr [si], 0x33221188
X: 9250 66 81 0c 88 11 22 33 | or dword ptr [si], 0x33221188
X: 9260 66 81 1c 88 11 22 33 | sbb dword ptr [si], 0x33221188
X: 9270 66 83 00 88 | add dword ptr [bx + si], -0x78
X: 9280 66 83 04 88 | add dword ptr [si], -0x78
X: 9300 66 83 0c 88 | or dword ptr [si], -0x78
X: 9310 66 83 1c 88 | sbb dword ptr [si], -0x78
X: 9320 66 87 00 | xchg dword ptr [bx + si], eax
X: 9330 66 87 04 | xchg dword ptr [si], eax
X: 93d0 66 89 00 | mov dword ptr [bx + si], eax
X: 93e0 66 89 04 | mov dword ptr [si], eax
X: 9480 66 8b 00 | mov eax, dword ptr [bx + si]
X: 9490 66 8b 04 | mov eax, dword ptr [si]
X: 95e0 66 8f 00 | pop dword ptr [bx + si]
X: 95f0 66 8f 04 | pop dword ptr [si]
X: 9c10 66 ab | stosd dword ptr es:[di], eax
X: 9cc0 66 ad | lodsd eax, dword ptr [si]
X: 9d70 66 af | scasd eax, dword ptr es:[di]
X: 9ed0 66 c1 00 88 | rol dword ptr [bx + si], 0x88
X: 9ee0 66 c1 04 88 | rol dword ptr [si], 0x88
X: 9f60 66 c1 0c 88 | ror dword ptr [si], 0x88
X: 9f70 66 c1 1c 88 | rcr dword ptr [si], 0x88
X: 9f80 66 c7 00 88 11 22 33 | mov dword ptr [bx + si], 0x33221188
X: 9f90 66 c7 04 88 11 22 33 | mov dword ptr [si], 0x33221188
X: a0e0 66 d1 00 | rol dword ptr [bx + si], 1
X: a0f0 66 d1 04 | rol dword ptr [si], 1
X: a170 66 d1 0c | ror dword ptr [si], 1
X: a180 66 d1 1c | rcr dword ptr [si], 1
X: a190 66 d3 00 | rol dword ptr [bx + si], cl
X: a1a0 66 d3 04 | rol dword ptr [si], cl
X: a220 66 d3 0c | ror dword ptr [si], cl
X: a230 66 d3 1c | rcr dword ptr [si], cl
X: a5b0 66 f7 00 88 11 22 33 | test dword ptr [bx + si], 0x33221188
X: a5c0 66 f7 04 88 11 22 33 | test dword ptr [si], 0x33221188
X: a650 66 f7 1c | neg dword ptr [si]
X: a660 66 ff 00 | inc dword ptr [bx + si]
X: a670 66 ff 04 | inc dword ptr [si]
X: a6f0 66 ff 0c | dec dword ptr [si]
X: a7c0 66 a6 | cmpsb byte ptr [si], byte ptr es:[di]
X: a870 66 ac | lodsb al, byte ptr [si]
X: a920 66 ae | scasb al, byte ptr es:[di]
X: aa80 66 6c | insb byte ptr es:[di], dx
X: ab30 66 6e | outsb dx, byte ptr [si]
X: b000 67 60 | pusha
X: b210 67 6d | insw word ptr es:[edi], dx
X: b350 67 81 0c 88 11 22 | or word ptr [eax + 4*ecx], 0x2211
X: b400 67 83 0c 88 11 | or word ptr [eax + 4*ecx], 0x11
X: b790 67 98 | cbw
X: b840 67 99 | cwd
X: b9a0 67 9c | pushf
X: bbb0 67 a5 | movsw word ptr es:[edi], word ptr [esi]
X: bc60 67 a7 | cmpsw word ptr [esi], word ptr es:[edi]
X: bd10 67 ab | stosw word ptr es:[edi], ax
X: bdc0 67 ad | lodsw ax, word ptr [esi]
X: be70 67 af | scasw ax, word ptr es:[edi]
X: c060 67 c1 0c 88 11 | ror word ptr [eax + 4*ecx], 0x11
X: c130 67 cf | iret
X: c270 67 d1 0c 88 | ror word ptr [eax + 4*ecx], 1
X: c320 67 d3 0c 88 | ror word ptr [eax + 4*ecx], cl
X: c350 67 e3 04 | jecxz 0xc357
X: c7f0 67 ff 0c 88 | dec word ptr [eax + 4*ecx]
X: cce0 26 01 00 | add word ptr es:[bx + si], ax
X: ccf0 26 01 04 | add word ptr es:[si], ax
X: cd90 26 03 00 | add ax, word ptr es:[bx + si]
X: cda0 26 03 04 | add ax, word ptr es:[si]
X: cef0 26 0f b6 00 | movzx ax, byte ptr es:[bx + si]
X: cf00 26 0f b6 04 | movzx ax, byte ptr es:[si]
X: d3c0 26 81 00 88 11 | add word ptr es:[bx + si], 0x1188
X: d3d0 26 81 04 88 11 | add word ptr es:[si], 0x1188
X: d450 26 81 0c 88 11 | or word ptr es:[si], 0x1188
X: d460 26 81 1c 88 11 | sbb word ptr es:[si], 0x1188
X: d470 26 83 00 88 | add word ptr es:[bx + si], -0x78
X: d480 26 83 04 88 | add word ptr es:[si], -0x78
X: d500 26 83 0c 88 | or word ptr es:[si], -0x78
X: d510 26 83 1c 88 | sbb word ptr es:[si], -0x78
X: d520 26 87 00 | xchg word ptr es:[bx + si], ax
X: d530 26 87 04 | xchg word ptr es:[si], ax
X: d5d0 26 89 00 | mov word ptr es:[bx + si], ax
X: d5e0 26 89 04 | mov word ptr es:[si], ax
X: d730 26 8d 00 | lea ax, es:[bx + si]
X: d740 26 8d 04 | lea ax, es:[si]
X: d7e0 26 8f 00 | pop word ptr es:[bx + si]
X: d7f0 26 8f 04 | pop word ptr es:[si]
X: 01f0 26 a5 | movsw word ptr es:[di], word ptr es:[si]
X: 02a0 26 a7 | cmpsw word ptr es:[si], word ptr es:[di]
X: 0400 26 ad | lodsw ax, word ptr es:[si]
X: 0610 26 c1 00 88 | rol word ptr es:[bx + si], 0x88
X: 0620 26 c1 04 88 | rol word ptr es:[si], 0x88
X: 06a0 26 c1 0c 88 | ror word ptr es:[si], 0x88
X: 06b0 26 c1 1c 88 | rcr word ptr es:[si], 0x88
X: 06c0 26 c7 00 88 11 | mov word ptr es:[bx + si], 0x1188
X: 06d0 26 c7 04 88 11 | mov word ptr es:[si], 0x1188
X: 0820 26 d1 00 | rol word ptr es:[bx + si], 1
X: 0830 26 d1 04 | rol word ptr es:[si], 1
X: 08b0 26 d1 0c | ror word ptr es:[si], 1
X: 08c0 26 d1 1c | rcr word ptr es:[si], 1
X: 08d0 26 d3 00 | rol word ptr es:[bx + si], cl
X: 08e0 26 d3 04 | rol word ptr es:[si], cl
X: 0960 26 d3 0c | ror word ptr es:[si], cl
X: 0970 26 d3 1c | rcr word ptr es:[si], cl
X: 0cf0 26 f7 00 88 11 | test word ptr es:[bx + si], 0x1188
X: 0d00 26 f7 04 88 11 | test word ptr es:[si], 0x1188
X: 0d90 26 f7 1c | neg word ptr es:[si]
X: 0da0 26 ff 00 | inc word ptr es:[bx + si]
X: 0db0 26 ff 04 | inc word ptr es:[si]
X: 0e30 26 ff 0c | dec word ptr es:[si]
X: 0e40 26 ff 1c | lcall es:[si]
X: 0e50 26 a4 | movsb byte ptr es:[di], byte ptr es:[si]
X: 0f00 26 a6 | cmpsb byte ptr es:[si], byte ptr es:[di]
X: 1270 26 6e | outsb dx, byte ptr es:[si]
X: 1a90 2e 81 0c 88 11 | or word ptr cs:[si], 0x1188
X: 1aa0 2e 81 1c 88 11 | sbb word ptr cs:[si], 0x1188
X: 1b40 2e 83 0c 88 | or word ptr cs:[si], -0x78
X: 1b50 2e 83 1c 88 | sbb word ptr cs:[si], -0x78
X: 22f0 2e a5 | movsw word ptr es:[di], word ptr cs:[si]
X: 23a0 2e a7 | cmpsw word ptr cs:[si], word ptr es:[di]
X: 2500 2e ad | lodsw ax, word ptr cs:[si]
X: 27a0 2e c1 0c 88 | ror word ptr cs:[si], 0x88
X: 27b0 2e c1 1c 88 | rcr word ptr cs:[si], 0x88
X: 29b0 2e d1 0c | ror word ptr cs:[si], 1
X: 29c0 2e d1 1c | rcr word ptr cs:[si], 1
X: 2a60 2e d3 0c | ror word ptr cs:[si], cl
X: 2a70 2e d3 1c | rcr word ptr cs:[si], cl
X: 2e90 2e f7 1c | neg word ptr cs:[si]
X: 2f30 2e ff 0c | dec word ptr cs:[si]
X: 2f40 2e ff 1c | lcall cs:[si]
X: 3000 2e a6 | cmpsb byte ptr cs:[si], byte ptr es:[di]
X: 30b0 2e ac | lodsb al, byte ptr cs:[si]
X: 3370 2e 6e | outsb dx, byte ptr cs:[si]
X: b820 f0 01 00 | lock add word ptr [bx + si], ax
X: b830 f0 01 04 | lock add word ptr [si], ax
X: b8a0 f0 01 c0 | lock add ax, ax
X: b8d0 f0 03 00 | lock add ax, word ptr [bx + si]
X: b8e0 f0 03 04 | lock add ax, word ptr [si]
X: b950 f0 03 c0 | lock add ax, ax
X: b980 f0 05 00 88 | lock add ax, 0x8800
X: b990 f0 05 04 88 | lock add ax, 0x8804
X: ba30 f0 0f b6 00 | lock movzx ax, byte ptr [bx + si]
X: ba40 f0 0f b6 04 | lock movzx ax, byte ptr [si]
X: bab0 f0 0f b6 c0 | lock movzx ax, al
X: bae0 f0 40 | lock inc ax
X: bb90 f0 50 | lock push ax
X: bc40 f0 60 | lock pusha
X: bcf0 f0 68 00 88 | lock push 0x8800
X: bd00 f0 68 04 88 | lock push 0x8804
X: be00 f0 6a 84 | lock push -0x7c
X: be10 f0 6a 86 | lock push -0x7a
X: be50 f0 6d | lock insw word ptr es:[di], dx
X: bf00 f0 81 00 88 11 | lock add word ptr [bx + si], 0x1188
X: bf10 f0 81 04 88 11 | lock add word ptr [si], 0x1188
X: bf90 f0 81 0c 88 11 | lock or word ptr [si], 0x1188
X: bfa0 f0 81 1c 88 11 | lock sbb word ptr [si], 0x1188
X: bfb0 f0 83 00 88 | lock add word ptr [bx + si], -0x78
X: bfc0 f0 83 04 88 | lock add word ptr [si], -0x78
X: c030 f0 83 c0 88 | lock add ax, -0x78
X: c040 f0 83 0c 88 | lock or word ptr [si], -0x78
X: c050 f0 83 1c 88 | lock sbb word ptr [si], -0x78
X: c060 f0 87 00 | lock xchg word ptr [bx + si], ax
X: c0e0 f0 87 c0 | lock xchg ax, ax
X: c110 f0 89 00 | lock mov word ptr [bx + si], ax
X: c120 f0 89 04 | lock mov word ptr [si], ax
X: c190 f0 89 c0 | lock mov ax, ax
X: c1c0 f0 8b 00 | lock mov ax, word ptr [bx + si]
X: c1d0 f0 8b 04 | lock mov ax, word ptr [si]
X: c240 f0 8b c0 | lock mov ax, ax
X: c270 f0 8d 00 | lock lea ax, [bx + si]
X: c280 f0 8d 04 | lock lea ax, [si]
X: c320 f0 8f 00 | lock pop word ptr [bx + si]
X: c330 f0 8f 04 | lock pop word ptr [si]
X: c3a0 f0 8f c0 | lock pop ax
X: c530 f0 9a 00 88 11 22 | lock lcall 0x2211, 0x8800
X: c540 f0 9a 04 88 11 22 | lock lcall 0x2211, 0x8804
X: c5e0 f0 9c | lock pushf
X: c7f0 f0 a5 | lock movsw word ptr es:[di], word ptr [si]
X: c8a0 f0 a7 | lock cmpsw word ptr [si], word ptr es:[di]
X: c950 f0 ab | lock stosw word ptr es:[di], ax
X: ca00 f0 ad | lock lodsw ax, word ptr [si]
X: cab0 f0 af | lock scasw ax, word ptr es:[di]
X: cb60 f0 b8 00 88 | lock mov ax, 0x8800
X: cb70 f0 b8 04 88 | lock mov ax, 0x8804
X: cc10 f0 c1 00 88 | lock rol word ptr [bx + si], 0x88
X: cc20 f0 c1 04 88 | lock rol word ptr [si], 0x88
X: cc90 f0 c1 c0 88 | lock rol ax, 0x88
X: cca0 f0 c1 0c 88 | lock ror word ptr [si], 0x88
X: ccb0 f0 c1 1c 88 | lock rcr word ptr [si], 0x88
X: ccc0 f0 c7 00 88 11 | lock mov word ptr [bx + si], 0x1188
X: ccd0 f0 c7 04 88 11 | lock mov word ptr [si], 0x1188
X: cd70 f0 cf | lock iret
X: ce20 f0 d1 00 | lock rol word ptr [bx + si], 1
X: ce30 f0 d1 04 | lock rol word ptr [si], 1
X: cea0 f0 d1 c0 | lock rol ax, 1
X: ceb0 f0 d1 0c | lock ror word ptr [si], 1
X: cec0 f0 d1 1c | lock rcr word ptr [si], 1
X: ced0 f0 d3 00 | lock rol word ptr [bx + si], cl
X: cee0 f0 d3 04 | lock rol word ptr [si], cl
X: cf50 f0 d3 c0 | lock rol ax, cl
X: cf60 f0 d3 0c | lock ror word ptr [si], cl
X: cf70 f0 d3 1c | lock rcr word ptr [si], cl
X: cf80 f0 e3 00 | lock jcxz 0xcf83
X: cf90 f0 e3 04 | lock jcxz 0xcf97
X: d030 f0 e5 00 | lock in ax, 0x0
X: d040 f0 e5 04 | lock in ax, 0x4
X: d0e0 f0 e8 00 88 | lock call 0x58e4
X: d0f0 f0 e8 04 88 | lock call 0x58f8
X: d190 f0 e9 00 88 | lock jmp 0x5994
X: d1a0 f0 e9 04 88 | lock jmp 0x59a8
X: d240 f0 ea 00 88 11 22 | lock ljmp 0x2211, 0x8800
X: d250 f0 ea 04 88 11 22 | lock ljmp 0x2211, 0x8804
X: d2f0 f0 f7 00 88 11 | lock test word ptr [bx + si], 0x1188
X: d300 f0 f7 04 88 11 | lock test word ptr [si], 0x1188
X: d370 f0 f7 c0 88 11 | lock test ax, 0x1188
X: d390 f0 f7 1c | lock neg word ptr [si]
X: d3a0 f0 ff 00 | lock inc word ptr [bx + si]
X: d3b0 f0 ff 04 | lock inc word ptr [si]
X: d420 f0 ff c0 | lock inc ax
X: d430 f0 ff 0c | lock dec word ptr [si]
X: d440 f0 ff 1c | lock lcall [si]
X: d450 f0 a4 | lock movsb byte ptr es:[di], byte ptr [si]
X: d500 f0 a6 | lock cmpsb byte ptr [si], byte ptr es:[di]
X: d5b0 f0 ac | lock lodsb al, byte ptr [si]
X: d660 f0 ae | lock scasb al, byte ptr es:[di]
X: d710 f0 aa | lock stosb byte ptr es:[di], al
X: d7c0 f0 6c | lock insb byte ptr es:[di], dx
X: d870 f0 6e | lock outsb dx, byte ptr [si]
X: 0490 f3 6d | rep insw word ptr es:[di], dx
X: 0e30 f3 a5 | rep movsw word ptr es:[di], word ptr [si]
X: 0ee0 f3 a7 | rep cmpsw word ptr [si], word ptr es:[di]
X: 1040 f3 ad | rep lodsw ax, word ptr [si]
X: 10f0 f3 af | rep scasw ax, word ptr es:[di]
X: 1bf0 f3 ac | rep lodsb al, byte ptr [si]
X: 1d50 f3 aa | rep stosb byte ptr es:[di], al
X: 1e00 f3 6c | rep insb byte ptr es:[di], dx
X: 1eb0 f3 6e | rep outsb dx, byte ptr [si]
X: 2590 f2 6d | repne insw word ptr es:[di], dx
X: 2f30 f2 a5 | repne movsw word ptr es:[di], word ptr [si]
X: 2fe0 f2 a7 | repne cmpsw word ptr [si], word ptr es:[di]
X: 3090 f2 ab | repne stosw word ptr es:[di], ax
X: 3140 f2 ad | repne lodsw ax, word ptr [si]
X: 31f0 f2 af | repne scasw ax, word ptr es:[di]
X: 3cf0 f2 ac | repne lodsb al, byte ptr [si]
X: 3e50 f2 aa | repne stosb byte ptr es:[di], al
X: 3f00 f2 6c | repne insb byte ptr es:[di], dx
X: 3fb0 f2 6e | repne outsb dx, byte ptr [si]
X: 4480 66 67 60 | pushad
X: 4690 66 67 6d | insd dword ptr es:[edi], dx
X: 47d0 66 67 81 0c 88 11 22 33 44 | or dword ptr [eax + 4*ecx], 0x44332211
X: 47e0 66 67 81 1c 88 11 22 33 44 | sbb dword ptr [eax + 4*ecx], 0x44332211
X: 4c10 66 67 98 | cwde
X: 4cc0 66 67 99 | cdq
X: 4e20 66 67 9c | pushfd
X: 5030 66 67 a5 | movsd dword ptr es:[edi], dword ptr [esi]
X: 50e0 66 67 a7 | cmpsd dword ptr [esi], dword ptr es:[edi]
X: 5190 66 67 ab | stosd dword ptr es:[edi], eax
X: 5240 66 67 ad | lodsd eax, dword ptr [esi]
X: 52f0 66 67 af | scasd eax, dword ptr es:[edi]
X: 54e0 66 67 c1 0c 88 11 | ror dword ptr [eax + 4*ecx], 0x11
X: 54f0 66 67 c1 1c 88 11 | rcr dword ptr [eax + 4*ecx], 0x11
X: 55b0 66 67 cf | iretd
X: 56f0 66 67 d1 0c 88 | ror dword ptr [eax + 4*ecx], 1
X: 5700 66 67 d1 1c 88 | rcr dword ptr [eax + 4*ecx], 1
X: 57a0 66 67 d3 0c 88 | ror dword ptr [eax + 4*ecx], cl
X: 57b0 66 67 d3 1c 88 | rcr dword ptr [eax + 4*ecx], cl
X: 5bd0 66 67 f7 1c 88 | neg dword ptr [eax + 4*ecx]
X: 5c70 66 67 ff 0c 88 | dec dword ptr [eax + 4*ecx]
X: fff0 f1 | int1

S" disx86" .RESULTS
