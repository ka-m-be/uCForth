\ x86.fs - tests of the package X86 (asm/x86.fs)
\
\ Load (in the folder ucf), after the package FILES and the tester, with
\ a bootstrap VM with 32-bit cells:
\   build/ucf-c -32 host/files.fs forth/test/tester.fs asm/test/x86.fs
\
\ Part 1: single instructions. The expected bytes come from clang 21
\ (Apple LLVM, "clang -target i386-pc-none-elf" with
\ ".intel_syntax noprefix"), once, and are kept here as data. The
\ comment of each test is the instruction for clang. GOT gives the
\ bytes since the last GOT, the first byte deepest. (The name G is the
\ condition "greater" of the assembler.)
\ Part 2: labels, jumps and structures in one piece of code at $402000.
\ The expected bytes come from this assembler; objdump (LLVM 21) showed
\ them as the instructions in the comments, each jump to the right
\ address.
\ Part 3: the errors, each with its message.

ONLY FORTH DEFINITIONS  ALSO FILES
S" lib/text.fs" INCLUDED
S" asm/x86.fs" INCLUDED
ONLY FORTH DEFINITIONS  ALSO INTERNALS  ALSO TEXT  ALSO X86
DECIMAL

VARIABLE MARK
: GOT ( -- c1 ... cn )   \ the bytes since the last GOT
  MARK @  BEGIN DUP THERE U< WHILE  DUP TC@ SWAP 1+  REPEAT  DROP  THERE MARK ! ;
: AT ( ta n -- c1 ... cn )   \ n bytes at ta
  0 ?DO  DUP TC@ SWAP 1+  LOOP  DROP ;

\ ==== Part 1: single instructions ====
$402000 ORG  THERE MARK !

T{ EAX EBX MOV, GOT -> $89 $D8 }T                              \ mov eax, ebx
T{ EBX EAX MOV, GOT -> $89 $C3 }T                              \ mov ebx, eax
T{ ECX EDX MOV, GOT -> $89 $D1 }T                              \ mov ecx, edx
T{ ESP EBP MOV, GOT -> $89 $EC }T                              \ mov esp, ebp
T{ ESI EDI MOV, GOT -> $89 $FE }T                              \ mov esi, edi
T{ AL BH MOV, GOT -> $88 $F8 }T                                \ mov al, bh
T{ CL DL MOV, GOT -> $88 $D1 }T                                \ mov cl, dl
T{ EAX 0 [EBX] MOV, GOT -> $8B $03 }T                          \ mov eax, dword ptr [ebx]
T{ EAX 4 [EBP] MOV, GOT -> $8B $45 $04 }T                      \ mov eax, dword ptr [ebp+4]
T{ EDX 0 [EBP] MOV, GOT -> $8B $55 $00 }T                      \ mov edx, dword ptr [ebp]
T{ ECX -4 [ESI] MOV, GOT -> $8B $4E $FC }T                     \ mov ecx, dword ptr [esi-4]
T{ EBX $100 [EDI] MOV, GOT -> $8B $9F $00 $01 $00 $00 }T       \ mov ebx, dword ptr [edi+0x100]
T{ EAX 127 [ESI] MOV, GOT -> $8B $46 $7F }T                    \ mov eax, dword ptr [esi+127]
T{ EAX 128 [ESI] MOV, GOT -> $8B $86 $80 $00 $00 $00 }T        \ mov eax, dword ptr [esi+128]
T{ EAX -128 [ESI] MOV, GOT -> $8B $46 $80 }T                   \ mov eax, dword ptr [esi-128]
T{ EAX -129 [ESI] MOV, GOT -> $8B $86 $7F $FF $FF $FF }T       \ mov eax, dword ptr [esi-129]
T{ EBX $410000 [ESI] MOV, GOT -> $8B $9E $00 $00 $41 $00 }T    \ mov ebx, dword ptr [esi+0x410000]
T{ EAX $410000 [EBX] MOV, GOT -> $8B $83 $00 $00 $41 $00 }T    \ mov eax, dword ptr [ebx+0x410000]
T{ EAX 0 [ESP] MOV, GOT -> $8B $04 $24 }T                      \ mov eax, dword ptr [esp]
T{ EAX 4 [ESP] MOV, GOT -> $8B $44 $24 $04 }T                  \ mov eax, dword ptr [esp+4]
T{ EDX $200 [ESP] MOV, GOT -> $8B $94 $24 $00 $02 $00 $00 }T   \ mov edx, dword ptr [esp+0x200]
T{ EAX $402000 [] MOV, GOT -> $A1 $00 $20 $40 $00 }T           \ mov eax, dword ptr [0x402000]
T{ AL $402000 [] MOV, GOT -> $A0 $00 $20 $40 $00 }T            \ mov al, byte ptr [0x402000]
T{ EBX $402000 [] MOV, GOT -> $8B $1D $00 $20 $40 $00 }T       \ mov ebx, dword ptr [0x402000]
T{ $402000 [] EAX MOV, GOT -> $A3 $00 $20 $40 $00 }T           \ mov dword ptr [0x402000], eax
T{ $402000 [] AL MOV, GOT -> $A2 $00 $20 $40 $00 }T            \ mov byte ptr [0x402000], al
T{ $402000 [] EDX MOV, GOT -> $89 $15 $00 $20 $40 $00 }T       \ mov dword ptr [0x402000], edx
T{ 0 [EBX] EAX MOV, GOT -> $89 $03 }T                          \ mov dword ptr [ebx], eax
T{ 4 [EBP] ESI MOV, GOT -> $89 $75 $04 }T                      \ mov dword ptr [ebp+4], esi
T{ $410000 [EDI] EAX MOV, GOT -> $89 $87 $00 $00 $41 $00 }T    \ mov dword ptr [edi+0x410000], eax
T{ 0 [EDI] CL MOV, GOT -> $88 $0F }T                           \ mov byte ptr [edi], cl
T{ $410000 [EAX] CL MOV, GOT -> $88 $88 $00 $00 $41 $00 }T     \ mov byte ptr [eax+0x410000], cl
T{ EAX 0 [ECX] [EAX*4] MOV, GOT -> $8B $04 $81 }T              \ mov eax, dword ptr [ecx+eax*4]
T{ EAX 8 [ECX] [EAX*4] MOV, GOT -> $8B $44 $81 $08 }T          \ mov eax, dword ptr [ecx+eax*4+8]
T{ EAX $1000 [ECX] [EDX*2] MOV, GOT -> $8B $84 $51 $00 $10 $00 $00 }T \ mov eax, dword ptr [ecx+edx*2+0x1000]
T{ EBX $412000 [] [EDX*4] MOV, GOT -> $8B $1C $95 $00 $20 $41 $00 }T \ mov ebx, dword ptr [edx*4+0x412000]
T{ EAX 0 [EBP] [ESI*1] MOV, GOT -> $8B $44 $35 $00 }T          \ mov eax, dword ptr [ebp+esi]
T{ EAX 0 [EAX] [EBP*8] MOV, GOT -> $8B $04 $E8 }T              \ mov eax, dword ptr [eax+ebp*8]
T{ AL 5 [EBX] [ECX*1] MOV, GOT -> $8A $44 $0B $05 }T           \ mov al, byte ptr [ebx+ecx+5]
T{ 0 [ESP] [EAX*4] EDX MOV, GOT -> $89 $14 $84 }T              \ mov dword ptr [esp+eax*4], edx
T{ EAX $12345678 MOVI, GOT -> $B8 $78 $56 $34 $12 }T           \ mov eax, 0x12345678
T{ EBX 0 MOVI, GOT -> $BB $00 $00 $00 $00 }T                   \ mov ebx, 0
T{ ESI -1 MOVI, GOT -> $BE $FF $FF $FF $FF }T                  \ mov esi, 0xffffffff
T{ EDI 2 MOVI, GOT -> $BF $02 $00 $00 $00 }T                   \ mov edi, 2
T{ AL 5 MOVI, GOT -> $B0 $05 }T                                \ mov al, 5
T{ BH $FF MOVI, GOT -> $B7 $FF }T                              \ mov bh, 0xff
T{ 0 [EBX] 7 MOVI, GOT -> $C7 $03 $07 $00 $00 $00 }T           \ mov dword ptr [ebx], 7
T{ $402000 [] $12345678 MOVI, GOT -> $C7 $05 $00 $20 $40 $00 $78 $56 $34 $12 }T \ mov dword ptr [0x402000], 0x12345678
T{ 0 [EDI] BYTE 7 MOVI, GOT -> $C6 $07 $07 }T                  \ mov byte ptr [edi], 7
T{ $410000 [EBP] 0 MOVI, GOT -> $C7 $85 $00 $00 $41 $00 $00 $00 $00 $00 }T \ mov dword ptr [ebp+0x410000], 0
T{ 4 [ESP] -1 MOVI, GOT -> $C7 $44 $24 $04 $FF $FF $FF $FF }T  \ mov dword ptr [esp+4], 0xffffffff
T{ EAX BL MOVZX, GOT -> $0F $B6 $C3 }T                         \ movzx eax, bl
T{ EAX $410000 [EBX] BYTE MOVZX, GOT -> $0F $B6 $83 $00 $00 $41 $00 }T \ movzx eax, byte ptr [ebx+0x410000]
T{ ECX 0 [ESI] BYTE MOVZX, GOT -> $0F $B6 $0E }T               \ movzx ecx, byte ptr [esi]
T{ EDX AH MOVSX, GOT -> $0F $BE $D4 }T                         \ movsx edx, ah
T{ EAX 4 [EBX] LEA, GOT -> $8D $43 $04 }T                      \ lea eax, [ebx+4]
T{ ESI 4 [EBX] LEA, GOT -> $8D $73 $04 }T                      \ lea esi, [ebx+4]
T{ EDI $410000 [EDX] LEA, GOT -> $8D $BA $00 $00 $41 $00 }T    \ lea edi, [edx+0x410000]
T{ EAX 0 [EAX] [EAX*2] LEA, GOT -> $8D $04 $40 }T              \ lea eax, [eax+eax*2]
T{ ECX 1 [] [EDX*4] LEA, GOT -> $8D $0C $95 $01 $00 $00 $00 }T \ lea ecx, [edx*4+1]
T{ EAX EDX ADD, GOT -> $01 $D0 }T                              \ add eax, edx
T{ EDX EAX SUB, GOT -> $29 $C2 }T                              \ sub edx, eax
T{ ECX EBX AND, GOT -> $21 $D9 }T                              \ and ecx, ebx
T{ EAX EAX OR, GOT -> $09 $C0 }T                               \ or eax, eax
T{ EAX EAX XOR, GOT -> $31 $C0 }T                              \ xor eax, eax
T{ EAX EDX CMP, GOT -> $39 $D0 }T                              \ cmp eax, edx
T{ EAX EAX SBB, GOT -> $19 $C0 }T                              \ sbb eax, eax
T{ EDX ECX ADC, GOT -> $11 $CA }T                              \ adc edx, ecx
T{ AL AH CMP, GOT -> $38 $E0 }T                                \ cmp al, ah
T{ AL 0 [EDI] CMP, GOT -> $3A $07 }T                           \ cmp al, byte ptr [edi]
T{ EAX $410000 [EDI] ADD, GOT -> $03 $87 $00 $00 $41 $00 }T    \ add eax, dword ptr [edi+0x410000]
T{ $410000 [EDI] EAX ADD, GOT -> $01 $87 $00 $00 $41 $00 }T    \ add dword ptr [edi+0x410000], eax
T{ $410000 [EDI] EAX SUB, GOT -> $29 $87 $00 $00 $41 $00 }T    \ sub dword ptr [edi+0x410000], eax
T{ $410000 [EDI] EAX AND, GOT -> $21 $87 $00 $00 $41 $00 }T    \ and dword ptr [edi+0x410000], eax
T{ $410000 [EDI] EAX OR, GOT -> $09 $87 $00 $00 $41 $00 }T     \ or dword ptr [edi+0x410000], eax
T{ $410000 [EDI] EAX XOR, GOT -> $31 $87 $00 $00 $41 $00 }T    \ xor dword ptr [edi+0x410000], eax
T{ $410000 [EDI] EAX CMP, GOT -> $39 $87 $00 $00 $41 $00 }T    \ cmp dword ptr [edi+0x410000], eax
T{ EAX $402000 [] ADD, GOT -> $03 $05 $00 $20 $40 $00 }T       \ add eax, dword ptr [0x402000]
T{ ECX $402000 [] CMP, GOT -> $3B $0D $00 $20 $40 $00 }T       \ cmp ecx, dword ptr [0x402000]
T{ AL $402000 [] CMP, GOT -> $3A $05 $00 $20 $40 $00 }T        \ cmp al, byte ptr [0x402000]
T{ EAX 5 ADDI, GOT -> $83 $C0 $05 }T                           \ add eax, 5
T{ EAX $1234 ADDI, GOT -> $05 $34 $12 $00 $00 }T               \ add eax, 0x1234
T{ EBX $1234 ADDI, GOT -> $81 $C3 $34 $12 $00 $00 }T           \ add ebx, 0x1234
T{ EDI 4 ADDI, GOT -> $83 $C7 $04 }T                           \ add edi, 4
T{ EDI 4 SUBI, GOT -> $83 $EF $04 }T                           \ sub edi, 4
T{ EBP 12 ADDI, GOT -> $83 $C5 $0C }T                          \ add ebp, 12
T{ ESP 16 ADDI, GOT -> $83 $C4 $10 }T                          \ add esp, 16
T{ ECX -1 ANDI, GOT -> $83 $E1 $FF }T                          \ and ecx, 0xffffffff
T{ EAX 127 CMPI, GOT -> $83 $F8 $7F }T                         \ cmp eax, 127
T{ EAX 128 CMPI, GOT -> $3D $80 $00 $00 $00 }T                 \ cmp eax, 128
T{ EAX -128 CMPI, GOT -> $83 $F8 $80 }T                        \ cmp eax, -128
T{ ECX 32 CMPI, GOT -> $83 $F9 $20 }T                          \ cmp ecx, 32
T{ EDX $80000000 ORI, GOT -> $81 $CA $00 $00 $00 $80 }T        \ or edx, 0x80000000
T{ EAX $C0000005 CMPI, GOT -> $3D $05 $00 $00 $C0 }T           \ cmp eax, 0xc0000005
T{ AL 31 ANDI, GOT -> $24 $1F }T                               \ and al, 31
T{ AL 97 CMPI, GOT -> $3C $61 }T                               \ cmp al, 97
T{ AH $20 SUBI, GOT -> $80 $EC $20 }T                          \ sub ah, 0x20
T{ BL 1 XORI, GOT -> $80 $F3 $01 }T                            \ xor bl, 1
T{ 0 [EBX] 1 ADDI, GOT -> $83 $03 $01 }T                       \ add dword ptr [ebx], 1
T{ $402000 [] 0 CMPI, GOT -> $83 $3D $00 $20 $40 $00 $00 }T    \ cmp dword ptr [0x402000], 0
T{ $402000 [] 128 CMPI, GOT -> $81 $3D $00 $20 $40 $00 $80 $00 $00 $00 }T \ cmp dword ptr [0x402000], 128
T{ $402000 [] BYTE 0 CMPI, GOT -> $80 $3D $00 $20 $40 $00 $00 }T \ cmp byte ptr [0x402000], 0
T{ 4 [EBP] BYTE 3 ANDI, GOT -> $80 $65 $04 $03 }T              \ and byte ptr [ebp+4], 3
T{ EAX 0 ADCI, GOT -> $83 $D0 $00 }T                           \ adc eax, 0
T{ EAX EBX TEST, GOT -> $85 $D8 }T                             \ test eax, ebx
T{ EAX EAX TEST, GOT -> $85 $C0 }T                             \ test eax, eax
T{ 0 [EBX] EAX TEST, GOT -> $85 $03 }T                         \ test dword ptr [ebx], eax
T{ AL CL TEST, GOT -> $84 $C8 }T                               \ test al, cl
T{ EAX 1 TESTI, GOT -> $A9 $01 $00 $00 $00 }T                  \ test eax, 1
T{ EDX $100 TESTI, GOT -> $F7 $C2 $00 $01 $00 $00 }T           \ test edx, 0x100
T{ AL 1 TESTI, GOT -> $A8 $01 }T                               \ test al, 1
T{ 0 [EBX] BYTE $80 TESTI, GOT -> $F6 $03 $80 }T               \ test byte ptr [ebx], 0x80
T{ EAX NOT, GOT -> $F7 $D0 }T                                  \ not eax
T{ EAX NEG, GOT -> $F7 $D8 }T                                  \ neg eax
T{ ECX MUL, GOT -> $F7 $E1 }T                                  \ mul ecx
T{ EBX IMUL, GOT -> $F7 $EB }T                                 \ imul ebx
T{ ECX DIV, GOT -> $F7 $F1 }T                                  \ div ecx
T{ ECX IDIV, GOT -> $F7 $F9 }T                                 \ idiv ecx
T{ AL NEG, GOT -> $F6 $D8 }T                                   \ neg al
T{ $410000 [EDI] NOT, GOT -> $F7 $97 $00 $00 $41 $00 }T        \ not dword ptr [edi+0x410000]
T{ $410000 [EDI] NEG, GOT -> $F7 $9F $00 $00 $41 $00 }T        \ neg dword ptr [edi+0x410000]
T{ $402000 [] MUL, GOT -> $F7 $25 $00 $20 $40 $00 }T           \ mul dword ptr [0x402000]
T{ EAX INC, GOT -> $40 }T                                      \ inc eax
T{ EDI DEC, GOT -> $4F }T                                      \ dec edi
T{ AL INC, GOT -> $FE $C0 }T                                   \ inc al
T{ $410000 [EDI] INC, GOT -> $FF $87 $00 $00 $41 $00 }T        \ inc dword ptr [edi+0x410000]
T{ $410000 [EBP] DEC, GOT -> $FF $8D $00 $00 $41 $00 }T        \ dec dword ptr [ebp+0x410000]
T{ 0 [EBX] BYTE INC, GOT -> $FE $03 }T                         \ inc byte ptr [ebx]
T{ EAX 1 SHL, GOT -> $D1 $E0 }T                                \ shl eax, 1
T{ EAX CL SHL, GOT -> $D3 $E0 }T                               \ shl eax, cl
T{ EAX 3 SHL, GOT -> $C1 $E0 $03 }T                            \ shl eax, 3
T{ EDX 31 SAR, GOT -> $C1 $FA $1F }T                           \ sar edx, 31
T{ ECX 8 SHR, GOT -> $C1 $E9 $08 }T                            \ shr ecx, 8
T{ EAX CL SHR, GOT -> $D3 $E8 }T                               \ shr eax, cl
T{ EBX 4 ROL, GOT -> $C1 $C3 $04 }T                            \ rol ebx, 4
T{ EBX 1 ROR, GOT -> $D1 $CB }T                                \ ror ebx, 1
T{ EDX 1 RCL, GOT -> $D1 $D2 }T                                \ rcl edx, 1
T{ EDX CL RCR, GOT -> $D3 $DA }T                               \ rcr edx, cl
T{ AL 1 SHL, GOT -> $D0 $E0 }T                                 \ shl al, 1
T{ AH 4 SHR, GOT -> $C0 $EC $04 }T                             \ shr ah, 4
T{ $410000 [EDI] 1 SHL, GOT -> $D1 $A7 $00 $00 $41 $00 }T      \ shl dword ptr [edi+0x410000], 1
T{ $410000 [EDI] 1 SAR, GOT -> $D1 $BF $00 $00 $41 $00 }T      \ sar dword ptr [edi+0x410000], 1
T{ $410000 [EDI] CL SHL, GOT -> $D3 $A7 $00 $00 $41 $00 }T     \ shl dword ptr [edi+0x410000], cl
T{ $410000 [EDI] 31 SAR, GOT -> $C1 $BF $00 $00 $41 $00 $1F }T \ sar dword ptr [edi+0x410000], 31
T{ EAX PUSH, GOT -> $50 }T                                     \ push eax
T{ EDI PUSH, GOT -> $57 }T                                     \ push edi
T{ $402000 [] PUSH, GOT -> $FF $35 $00 $20 $40 $00 }T          \ push dword ptr [0x402000]
T{ 4 [ESP] PUSH, GOT -> $FF $74 $24 $04 }T                     \ push dword ptr [esp+4]
T{ EAX POP, GOT -> $58 }T                                      \ pop eax
T{ EBP POP, GOT -> $5D }T                                      \ pop ebp
T{ $402000 [] POP, GOT -> $8F $05 $00 $20 $40 $00 }T           \ pop dword ptr [0x402000]
T{ 0 PUSHI, GOT -> $6A $00 }T                                  \ push 0
T{ 1 PUSHI, GOT -> $6A $01 }T                                  \ push 1
T{ -10 PUSHI, GOT -> $6A $F6 }T                                \ push -10
T{ 127 PUSHI, GOT -> $6A $7F }T                                \ push 127
T{ 128 PUSHI, GOT -> $68 $80 $00 $00 $00 }T                    \ push 128
T{ $402000 PUSHI, GOT -> $68 $00 $20 $40 $00 }T                \ push 0x402000
T{ EAX EBX XCHG, GOT -> $93 }T                                 \ xchg eax, ebx
T{ EDX EAX XCHG, GOT -> $92 }T                                 \ xchg edx, eax
T{ ECX EDX XCHG, GOT -> $87 $CA }T                             \ xchg ecx, edx
T{ AL AH XCHG, GOT -> $86 $C4 }T                               \ xchg al, ah
T{ 0 [EBX] EAX XCHG, GOT -> $87 $03 }T                         \ xchg dword ptr [ebx], eax
T{ AL E SETCC, GOT -> $0F $94 $C0 }T                           \ sete al
T{ CL NE SETCC, GOT -> $0F $95 $C1 }T                          \ setne cl
T{ DL L SETCC, GOT -> $0F $9C $C2 }T                           \ setl dl
T{ BL B SETCC, GOT -> $0F $92 $C3 }T                           \ setb bl
T{ $402000 [] BYTE G SETCC, GOT -> $0F $9F $05 $00 $20 $40 $00 }T \ setg byte ptr [0x402000]
T{ EAX CALL-IND, GOT -> $FF $D0 }T                             \ call eax
T{ $402000 [] CALL-IND, GOT -> $FF $15 $00 $20 $40 $00 }T      \ call dword ptr [0x402000]
T{ 4 [EBX] CALL-IND, GOT -> $FF $53 $04 }T                     \ call dword ptr [ebx+4]
T{ ECX JMP-IND, GOT -> $FF $E1 }T                              \ jmp ecx
T{ $401000 [] [EAX*4] JMP-IND, GOT -> $FF $24 $85 $00 $10 $40 $00 }T \ jmp dword ptr [eax*4+0x401000]
T{ 0 [ECX] [EAX*4] JMP-IND, GOT -> $FF $24 $81 }T              \ jmp dword ptr [ecx+eax*4]
T{ 3 INT, GOT -> $CC }T                                        \ int3
T{ $80 INT, GOT -> $CD $80 }T                                  \ int 0x80
T{ 4 RETN, GOT -> $C2 $04 $00 }T                               \ ret 4
T{ RET, GOT -> $C3 }T                                          \ ret
T{ MOVSB, GOT -> $A4 }T                                        \ movsb byte ptr es:[edi], byte ptr [esi]
T{ MOVSD, GOT -> $A5 }T                                        \ movsd dword ptr es:[edi], dword ptr [esi]
T{ STOSB, GOT -> $AA }T                                        \ stosb byte ptr es:[edi], al
T{ STOSD, GOT -> $AB }T                                        \ stosd dword ptr es:[edi], eax
T{ LODSB, GOT -> $AC }T                                        \ lodsb al, byte ptr [esi]
T{ LODSD, GOT -> $AD }T                                        \ lodsd eax, dword ptr [esi]
T{ CMPSB, GOT -> $A6 }T                                        \ cmpsb byte ptr [esi], byte ptr es:[edi]
T{ SCASB, GOT -> $AE }T                                        \ scasb al, byte ptr es:[edi]
T{ REP, MOVSB, GOT -> $F3 $A4 }T                               \ rep movsb byte ptr es:[edi], byte ptr [esi]
T{ REP, STOSB, GOT -> $F3 $AA }T                               \ rep stosb byte ptr es:[edi], al
T{ CLD, GOT -> $FC }T                                          \ cld
T{ STD, GOT -> $FD }T                                          \ std
T{ CLC, GOT -> $F8 }T                                          \ clc
T{ STC, GOT -> $F9 }T                                          \ stc
T{ CMC, GOT -> $F5 }T                                          \ cmc
T{ CLI, GOT -> $FA }T                                          \ cli
T{ STI, GOT -> $FB }T                                          \ sti
T{ CDQ, GOT -> $99 }T                                          \ cdq
T{ NOP, GOT -> $90 }T                                          \ nop
T{ HLT, GOT -> $F4 }T                                          \ hlt
T{ PUSHFD, GOT -> $9C }T                                       \ pushfd
T{ POPFD, GOT -> $9D }T                                        \ popfd
T{ PUSHAD, GOT -> $60 }T                                       \ pushal
T{ POPAD, GOT -> $61 }T                                        \ popal
T{ LOCK, 0 [EBX] EAX XCHG, GOT -> $F0 $87 $03 }T               \ lock xchg dword ptr [ebx], eax

\ ==== Part 2: labels, jumps and structures ====
: NOPS ( n -- )  0 ?DO NOP, LOOP ;

$402000 ORG
LABEL START
  L1 JMP,
  L1 JZ,
  L2 CALL,
  EDX L3 MOVI,
  L3 PUSHI,
LABEL L1
  NOP,
  L1 JNZ,
  L1 LOOP,
  L1 JECXZ,
  L1 JMP,
  L1 JC,
LABEL L2
  RET,
LABEL L3
  $41 TC,  4 TALIGN
LABEL STRUCT
  EAX EAX OR,  Z IF,  EAX INC,  THEN,
  ECX ECX TEST,  NZ IF,  EAX INC,  ELSE,  EAX DEC,  THEN,
  BEGIN,  EAX DEC,  Z UNTIL,
  BEGIN,  ECX ECX OR,  NZ WHILE,  ECX DEC,  REPEAT,
  BEGIN,  NOP,  AGAIN,
FORWARD FAR
LABEL BACK
  FAR JMP,
  200 NOPS
  BACK JMP,
  BACK JZ,
LABEL FAR
  START CALL,
CHECK-LABELS

T{ START -> $402000 }T
T{ STRUCT -> $402028 }T
T{ BACK FAR -> $402042 $40211A }T
T{ $402000 5 AT -> $E9 $15 $00 $00 $00 }T        \ jmp 0x40201a (near: L1 had no place yet)
T{ $402005 6 AT -> $0F $84 $0F $00 $00 $00 }T    \ je 0x40201a (near)
T{ $40200B 5 AT -> $E8 $15 $00 $00 $00 }T        \ call 0x402025
T{ $402010 5 AT -> $BA $26 $20 $40 $00 }T        \ mov edx, 0x402026
T{ $402015 5 AT -> $68 $26 $20 $40 $00 }T        \ push 0x402026
T{ $40201A 1 AT -> $90 }T                        \ nop
T{ $40201B 2 AT -> $75 $FD }T                    \ jne 0x40201a
T{ $40201D 2 AT -> $E2 $FB }T                    \ loop 0x40201a
T{ $40201F 2 AT -> $E3 $F9 }T                    \ jecxz 0x40201a
T{ $402021 2 AT -> $EB $F7 }T                    \ jmp 0x40201a (short: L1 has its place)
T{ $402023 2 AT -> $72 $F5 }T                    \ jb 0x40201a
T{ $402025 3 AT -> $C3 $41 $00 }T                \ ret; the data byte; 4 TALIGN
T{ $402028 5 AT -> $09 $C0 $75 $01 $40 }T              \ or eax, eax; jne 0x40202d; inc eax
T{ $40202D 8 AT -> $85 $C9 $74 $03 $40 $EB $01 $48 }T  \ test ecx, ecx; je 0x402034; inc eax; jmp 0x402035; dec eax
T{ $402035 3 AT -> $48 $75 $FD }T                      \ dec eax; jne 0x402035
T{ $402038 7 AT -> $09 $C9 $74 $03 $49 $EB $F9 }T      \ or ecx, ecx; je 0x40203f; dec ecx; jmp 0x402038
T{ $40203F 3 AT -> $90 $EB $FD }T                      \ nop; jmp 0x40203f
T{ $402042 5 AT -> $E9 $D3 $00 $00 $00 }T        \ jmp 0x40211a
T{ $40210F 5 AT -> $E9 $2E $FF $FF $FF }T        \ jmp 0x402042 (near: too far for 8 bits)
T{ $402114 6 AT -> $0F $84 $28 $FF $FF $FF }T    \ je 0x402042 (near)
T{ $40211A 5 AT -> $E8 $E1 $FE $FF $FF }T        \ call 0x402000
T{ THERE -> $40211F }T

\ ==== Part 3: errors ====
\ Each case starts new code (NEW). An error is THROW -2 with a message;
\ MSG" compares the message. Words that parse (LABEL) run in a word.

: NEW ( -- )  $402000 ORG ;
: MSG" ( "text<quote>" -- f )  34 PARSE  ERRMSG @ ERRMSG CELL+ @ S= ;

: E1  NEW EAX BL MOV, ;            T{ ' E1  CATCH MSG" the operands have different sizes" -> -2 TRUE }T
: E2  NEW 0 [EBX] 0 [ESI] MOV, ;   T{ ' E2  CATCH MSG" one operand must be a register" -> -2 TRUE }T
: E3  NEW EAX 40 SHL, ;            T{ ' E3  CATCH MSG" a shift takes CL or a number from 0 to 31" -> -2 TRUE }T
: E4  NEW 5 EAX MOV, ;             T{ ' E4  CATCH MSG" an operand is missing" -> -2 TRUE }T
: E5  NEW EAX BYTE ;               T{ ' E5  CATCH MSG" BYTE needs a memory operand before it" -> -2 TRUE }T
: E6  NEW AL PUSH, ;               T{ ' E6  CATCH MSG" PUSH, takes 32 bits" -> -2 TRUE }T
: E7  NEW 0 [EBX] BYTE POP, ;      T{ ' E7  CATCH MSG" POP, takes 32 bits" -> -2 TRUE }T
: E8  NEW EAX 0 [EBX] BYTE MOV, ;  T{ ' E8  CATCH MSG" the operands have different sizes" -> -2 TRUE }T
: E9  NEW EAX [EAX*4] ;            T{ ' E9  CATCH MSG" an index needs a memory operand before it" -> -2 TRUE }T
: E10 NEW 0 [EBX] [EAX*4] [ECX*2] ;
T{ ' E10 CATCH MSG" a memory operand has one index register" -> -2 TRUE }T
: E11 NEW EAX 0 [EBX] LEA, ;       \ correct: no error
T{ ' E11 CATCH -> 0 }T
: E12 NEW AL 0 [EBX] LEA, ;        T{ ' E12 CATCH MSG" LEA, needs a register of 32 bits and memory" -> -2 TRUE }T
: E13 NEW AL BL MOVZX, ;           T{ ' E13 CATCH MSG" needs a register of 32 bits first" -> -2 TRUE }T
: E14 NEW EAX EBX MOVZX, ;         T{ ' E14 CATCH MSG" needs a byte register or BYTE memory second" -> -2 TRUE }T
: E15 NEW EAX E SETCC, ;           T{ ' E15 CATCH MSG" SETCC, needs a byte register or BYTE memory" -> -2 TRUE }T
: E16 NEW AL JMP-IND, ;            T{ ' E16 CATCH MSG" JMP-IND, takes a register of 32 bits or memory" -> -2 TRUE }T
: E17 NEW BEGIN, 130 NOPS Z UNTIL, ;
T{ ' E17 CATCH MSG" the jump is too far for 8 bits (127 bytes)" -> -2 TRUE }T
: E18 NEW Z IF, 130 NOPS THEN, ;
T{ ' E18 CATCH MSG" the jump is too far for 8 bits (127 bytes)" -> -2 TRUE }T
: E19 NEW $40A000 TC@ ;            \ ORG $402000: the code reaches up to $409FFF
T{ ' E19 CATCH MSG" address outside the target memory" -> -2 TRUE }T

\ Labels: a short jump too far, without place, placed twice, not a
\ jump. LABEL parses its name, so it runs in the line itself.
NEW L1 LOOP, 130 NOPS
T{ ' LABEL CATCH L1 MSG" the jump is too far for 8 bits (127 bytes)" -> -2 TRUE }T
NEW FORWARD X5  L1 JMP,            \ a named label starts a new scope: L1 waits
T{ ' LABEL CATCH X5 MSG" label without place: L1" -> -2 TRUE }T
NEW LABEL L2
T{ ' LABEL CATCH L2 MSG" label placed twice: L2" -> -2 TRUE }T
NEW L3 [] EAX MOV,
T{ ' LABEL CATCH L3 MSG" a label before its place is only for a jump, CALL, MOVI, to a register or PUSHI," -> -2 TRUE }T
NEW FORWARD X6  X6 JMP,
T{ ' CHECK-LABELS CATCH MSG" label without place: X6" -> -2 TRUE }T
TARGET CONSTANT SAVED-TARGET
: E20 0 TO TARGET  $402000 TC@ ;   \ no target memory yet: as before the first ORG
T{ ' E20 CATCH MSG" no target memory: ORG first" -> -2 TRUE }T
SAVED-TARGET TO TARGET
NEW

S" x86" .RESULTS
