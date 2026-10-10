\ x64.fs - tests of the package X64 (asm/x64.fs)
\
\ Load (in the folder ucf), after the package FILES and the tester, with
\ a bootstrap VM with 32-bit cells:
\   build/ucf-c -32 host/files.fs forth/test/tester.fs asm/test/x64.fs
\
\ Part 1: single instructions. The expected bytes come from clang 21
\ (Apple LLVM, "clang -target x86_64-pc-none-elf" with
\ ".intel_syntax noprefix"), once, and are kept here as data. The
\ comment of each test is the instruction for clang. GOT gives the
\ bytes since the last GOT, the first byte deepest. A test with [RIP]
\ names its target as THERE + n, the address of the instruction + n;
\ clang got the displacement: n minus the length of the instruction.
\ (The name G is the condition "greater" of the assembler.)
\ Part 2: labels, jumps, structures and [RIP] in one piece of code at
\ $402000. The expected bytes come from this assembler; objdump (LLVM
\ 21) showed them as the instructions in the comments, each jump to the
\ right address.
\ Part 3: the errors, each with its message.

ONLY FORTH DEFINITIONS  ALSO FILES
S" lib/text.fs" INCLUDED
S" asm/x64.fs" INCLUDED
ONLY FORTH DEFINITIONS  ALSO INTERNALS  ALSO TEXT  ALSO X64
DECIMAL

VARIABLE MARK
: GOT ( -- c1 ... cn )   \ the bytes since the last GOT
  MARK @  BEGIN DUP THERE U< WHILE  DUP TC@ SWAP 1+  REPEAT  DROP  THERE MARK ! ;
: AT ( ta n -- c1 ... cn )   \ n bytes at ta
  0 ?DO  DUP TC@ SWAP 1+  LOOP  DROP ;

\ ==== Part 1: single instructions ====
$402000 ORG  THERE MARK !

\ MOV between registers
T{ RAX RBX MOV, GOT -> $48 $89 $D8 }T                           \ mov rax, rbx
T{ RBX RAX MOV, GOT -> $48 $89 $C3 }T                           \ mov rbx, rax
T{ EAX EBX MOV, GOT -> $89 $D8 }T                               \ mov eax, ebx
T{ R8 RAX MOV, GOT -> $49 $89 $C0 }T                            \ mov r8, rax
T{ RAX R8 MOV, GOT -> $4C $89 $C0 }T                            \ mov rax, r8
T{ R15 R12 MOV, GOT -> $4D $89 $E7 }T                           \ mov r15, r12
T{ EAX R9D MOV, GOT -> $44 $89 $C8 }T                           \ mov eax, r9d
T{ R13D ESI MOV, GOT -> $41 $89 $F5 }T                          \ mov r13d, esi
T{ RSP RBP MOV, GOT -> $48 $89 $EC }T                           \ mov rsp, rbp
T{ AL BH MOV, GOT -> $88 $F8 }T                                 \ mov al, bh
T{ CL DL MOV, GOT -> $88 $D1 }T                                 \ mov cl, dl
T{ AL R8B MOV, GOT -> $44 $88 $C0 }T                            \ mov al, r8b
T{ R15B CL MOV, GOT -> $41 $88 $CF }T                           \ mov r15b, cl
T{ AL SIL MOV, GOT -> $40 $88 $F0 }T                            \ mov al, sil
T{ DIL BL MOV, GOT -> $40 $88 $DF }T                            \ mov dil, bl
T{ SPL AL MOV, GOT -> $40 $88 $C4 }T                            \ mov spl, al
T{ BPL R9B MOV, GOT -> $44 $88 $CD }T                           \ mov bpl, r9b

\ MOV with memory: base, displacement, index
T{ EAX 0 [RBX] MOV, GOT -> $8B $03 }T                           \ mov eax, dword ptr [rbx]
T{ RAX 0 [RBX] MOV, GOT -> $48 $8B $03 }T                       \ mov rax, qword ptr [rbx]
T{ EAX 4 [RBP] MOV, GOT -> $8B $45 $04 }T                       \ mov eax, dword ptr [rbp+4]
T{ EDX 0 [RBP] MOV, GOT -> $8B $55 $00 }T                       \ mov edx, dword ptr [rbp]
T{ ECX -4 [RSI] MOV, GOT -> $8B $4E $FC }T                      \ mov ecx, dword ptr [rsi-4]
T{ EBX $100 [RDI] MOV, GOT -> $8B $9F $00 $01 $00 $00 }T        \ mov ebx, dword ptr [rdi+0x100]
T{ EAX 127 [RSI] MOV, GOT -> $8B $46 $7F }T                     \ mov eax, dword ptr [rsi+127]
T{ EAX 128 [RSI] MOV, GOT -> $8B $86 $80 $00 $00 $00 }T         \ mov eax, dword ptr [rsi+128]
T{ EAX -128 [RSI] MOV, GOT -> $8B $46 $80 }T                    \ mov eax, dword ptr [rsi-128]
T{ EAX -129 [RSI] MOV, GOT -> $8B $86 $7F $FF $FF $FF }T        \ mov eax, dword ptr [rsi-129]
T{ EAX 0 [RSP] MOV, GOT -> $8B $04 $24 }T                       \ mov eax, dword ptr [rsp]
T{ EAX 4 [RSP] MOV, GOT -> $8B $44 $24 $04 }T                   \ mov eax, dword ptr [rsp+4]
T{ RDX $200 [RSP] MOV, GOT -> $48 $8B $94 $24 $00 $02 $00 $00 }T \ mov rdx, qword ptr [rsp+0x200]
T{ EAX 0 [R8] MOV, GOT -> $41 $8B $00 }T                        \ mov eax, dword ptr [r8]
T{ R9D 8 [R10] MOV, GOT -> $45 $8B $4A $08 }T                   \ mov r9d, dword ptr [r10+8]
T{ EAX 0 [R12] MOV, GOT -> $41 $8B $04 $24 }T                   \ mov eax, dword ptr [r12]
T{ EAX 8 [R12] MOV, GOT -> $41 $8B $44 $24 $08 }T               \ mov eax, dword ptr [r12+8]
T{ EAX 0 [R13] MOV, GOT -> $41 $8B $45 $00 }T                   \ mov eax, dword ptr [r13]
T{ EAX 0 [R15] MOV, GOT -> $41 $8B $07 }T                       \ mov eax, dword ptr [r15]
T{ RAX $40000 [R15] MOV, GOT -> $49 $8B $87 $00 $00 $04 $00 }T  \ mov rax, qword ptr [r15+0x40000]
T{ EBX 0 [R15] [R12*1] MOV, GOT -> $43 $8B $1C $27 }T           \ mov ebx, dword ptr [r15+r12]
T{ EAX 0 [R15] [RBX*1] MOV, GOT -> $41 $8B $04 $1F }T           \ mov eax, dword ptr [r15+rbx]
T{ EAX 4 [R15] [R13*1] MOV, GOT -> $43 $8B $44 $2F $04 }T       \ mov eax, dword ptr [r15+r13+4]
T{ EAX 0 [R14] [RAX*4] MOV, GOT -> $41 $8B $04 $86 }T           \ mov eax, dword ptr [r14+rax*4]
T{ RAX 0 [RCX] [RDX*8] MOV, GOT -> $48 $8B $04 $D1 }T           \ mov rax, qword ptr [rcx+rdx*8]
T{ EAX 0 [R13] [R8*2] MOV, GOT -> $43 $8B $44 $45 $00 }T        \ mov eax, dword ptr [r13+r8*2]
T{ EAX 0 [RBP] [RBP*1] MOV, GOT -> $8B $44 $2D $00 }T           \ mov eax, dword ptr [rbp+rbp]
T{ EAX -1 [R15] [R9*1] MOV, GOT -> $43 $8B $44 $0F $FF }T       \ mov eax, dword ptr [r15+r9-1]
T{ EAX 0 [RSP] [R12*1] MOV, GOT -> $42 $8B $04 $24 }T           \ mov eax, dword ptr [rsp+r12]
T{ EAX $1000 [] MOV, GOT -> $8B $04 $25 $00 $10 $00 $00 }T      \ mov eax, dword ptr [0x1000]
T{ EAX $1000 [] [RCX*4] MOV, GOT -> $8B $04 $8D $00 $10 $00 $00 }T \ mov eax, dword ptr [rcx*4+0x1000]
T{ 0 [RBX] EAX MOV, GOT -> $89 $03 }T                           \ mov dword ptr [rbx], eax
T{ 0 [RBX] RAX MOV, GOT -> $48 $89 $03 }T                       \ mov qword ptr [rbx], rax
T{ 8 [R15] [R13*1] EAX MOV, GOT -> $43 $89 $44 $2F $08 }T       \ mov dword ptr [r15+r13+8], eax
T{ 0 [R15] [RBP*1] R11D MOV, GOT -> $45 $89 $1C $2F }T          \ mov dword ptr [r15+rbp], r11d
T{ AL 0 [RSI] MOV, GOT -> $8A $06 }T                            \ mov al, byte ptr [rsi]
T{ 0 [RDI] CL MOV, GOT -> $88 $0F }T                            \ mov byte ptr [rdi], cl
T{ 0 [R15] [RAX*1] R8B MOV, GOT -> $45 $88 $04 $07 }T           \ mov byte ptr [r15+rax], r8b
T{ 0 [RDI] SIL MOV, GOT -> $40 $88 $37 }T                       \ mov byte ptr [rdi], sil
T{ AH 1 [RBX] MOV, GOT -> $8A $63 $01 }T                        \ mov ah, byte ptr [rbx+1]
T{ EAX 0 [RBX] BYTE MOVZX, GOT -> $0F $B6 $03 }T                \ movzx eax, byte ptr [rbx]

\ MOV with RIP: the target is the address of the instruction + n
T{ EAX THERE $100 + [RIP] MOV, GOT -> $8B $05 $FA $00 $00 $00 }T \ mov eax, dword ptr [rip+250]
T{ RBX THERE -$40 + [RIP] MOV, GOT -> $48 $8B $1D $B9 $FF $FF $FF }T \ mov rbx, qword ptr [rip-71]
T{ THERE $2000 + [RIP] ECX MOV, GOT -> $89 $0D $FA $1F $00 $00 }T \ mov dword ptr [rip+8186], ecx
T{ R12D THERE 6 + [RIP] MOV, GOT -> $44 $8B $25 $FF $FF $FF $FF }T \ mov r12d, dword ptr [rip-1]

\ MOVI
T{ EAX $12345678 MOVI, GOT -> $B8 $78 $56 $34 $12 }T            \ mov eax, 0x12345678
T{ EBX 0 MOVI, GOT -> $BB $00 $00 $00 $00 }T                    \ mov ebx, 0
T{ R8D 5 MOVI, GOT -> $41 $B8 $05 $00 $00 $00 }T                \ mov r8d, 5
T{ R15D -1 MOVI, GOT -> $41 $BF $FF $FF $FF $FF }T              \ mov r15d, 0xffffffff
T{ RAX 5 MOVI, GOT -> $48 $C7 $C0 $05 $00 $00 $00 }T            \ mov rax, 5
T{ RAX -1 MOVI, GOT -> $48 $C7 $C0 $FF $FF $FF $FF }T           \ mov rax, -1
T{ R14 $7FFFFFFF MOVI, GOT -> $49 $C7 $C6 $FF $FF $FF $7F }T    \ mov r14, 0x7fffffff
T{ AL 7 MOVI, GOT -> $B0 $07 }T                                 \ mov al, 7
T{ BH $FF MOVI, GOT -> $B7 $FF }T                               \ mov bh, 0xff
T{ R9B 1 MOVI, GOT -> $41 $B1 $01 }T                            \ mov r9b, 1
T{ SIL 2 MOVI, GOT -> $40 $B6 $02 }T                            \ mov sil, 2
T{ 0 [RBX] 5 MOVI, GOT -> $C7 $03 $05 $00 $00 $00 }T            \ mov dword ptr [rbx], 5
T{ 0 [RBX] BYTE 5 MOVI, GOT -> $C6 $03 $05 }T                   \ mov byte ptr [rbx], 5
T{ 0 [RBX] QWORD -2 MOVI, GOT -> $48 $C7 $03 $FE $FF $FF $FF }T \ mov qword ptr [rbx], -2
T{ $40000 [R15] 0 MOVI, GOT -> $41 $C7 $87 $00 $00 $04 $00 $00 $00 $00 $00 }T \ mov dword ptr [r15+0x40000], 0
T{ 0 [R15] [R13*1] 128 MOVI, GOT -> $43 $C7 $04 $2F $80 $00 $00 $00 }T \ mov dword ptr [r15+r13], 128
T{ THERE $80 + [RIP] 9 MOVI, GOT -> $C7 $05 $76 $00 $00 $00 $09 $00 $00 $00 }T \ mov dword ptr [rip+118], 9
T{ THERE $80 + [RIP] BYTE 9 MOVI, GOT -> $C6 $05 $79 $00 $00 $00 $09 }T \ mov byte ptr [rip+121], 9

\ MOVZX, MOVSX, MOVSXD
T{ EAX BL MOVZX, GOT -> $0F $B6 $C3 }T                          \ movzx eax, bl
T{ RAX BL MOVZX, GOT -> $48 $0F $B6 $C3 }T                      \ movzx rax, bl
T{ R8D 0 [R15] [RAX*1] BYTE MOVZX, GOT -> $45 $0F $B6 $04 $07 }T \ movzx r8d, byte ptr [r15+rax]
T{ EAX SIL MOVZX, GOT -> $40 $0F $B6 $C6 }T                     \ movzx eax, sil
T{ ECX AH MOVZX, GOT -> $0F $B6 $CC }T                          \ movzx ecx, ah
T{ EDX AL MOVSX, GOT -> $0F $BE $D0 }T                          \ movsx edx, al
T{ RDX 0 [RSI] BYTE MOVSX, GOT -> $48 $0F $BE $16 }T            \ movsx rdx, byte ptr [rsi]
T{ RAX EBX MOVSXD, GOT -> $48 $63 $C3 }T                        \ movsxd rax, ebx
T{ RCX 0 [R14] [RAX*4] MOVSXD, GOT -> $49 $63 $0C $86 }T        \ movsxd rcx, dword ptr [r14+rax*4]

\ Arithmetic and logic
T{ EAX EBX ADD, GOT -> $01 $D8 }T                               \ add eax, ebx
T{ RAX RBX ADD, GOT -> $48 $01 $D8 }T                           \ add rax, rbx
T{ R12D 4 ADDI, GOT -> $41 $83 $C4 $04 }T                       \ add r12d, 4
T{ R12 4 ADDI, GOT -> $49 $83 $C4 $04 }T                        \ add r12, 4
T{ EAX 0 [RBX] ADD, GOT -> $03 $03 }T                           \ add eax, dword ptr [rbx]
T{ 0 [RBX] EAX ADD, GOT -> $01 $03 }T                           \ add dword ptr [rbx], eax
T{ 0 [R15] [RBP*1] ECX ADD, GOT -> $41 $01 $0C $2F }T           \ add dword ptr [r15+rbp], ecx
T{ AL CL ADD, GOT -> $00 $C8 }T                                 \ add al, cl
T{ EAX EBX OR, GOT -> $09 $D8 }T                                \ or eax, ebx
T{ RAX R9 ADC, GOT -> $4C $11 $C8 }T                            \ adc rax, r9
T{ EAX EAX SBB, GOT -> $19 $C0 }T                               \ sbb eax, eax
T{ ECX EDX AND, GOT -> $21 $D1 }T                               \ and ecx, edx
T{ R10 R11 SUB, GOT -> $4D $29 $DA }T                           \ sub r10, r11
T{ EAX EAX XOR, GOT -> $31 $C0 }T                               \ xor eax, eax
T{ R8D R8D XOR, GOT -> $45 $31 $C0 }T                           \ xor r8d, r8d
T{ EAX EBX CMP, GOT -> $39 $D8 }T                               \ cmp eax, ebx
T{ RAX $40000 [R15] CMP, GOT -> $49 $3B $87 $00 $00 $04 $00 }T  \ cmp rax, qword ptr [r15+0x40000]
T{ EAX 5 ADDI, GOT -> $83 $C0 $05 }T                            \ add eax, 5
T{ EBX 5 ADDI, GOT -> $83 $C3 $05 }T                            \ add ebx, 5
T{ EAX 1000 ADDI, GOT -> $05 $E8 $03 $00 $00 }T                 \ add eax, 1000
T{ RAX 1000 ADDI, GOT -> $48 $05 $E8 $03 $00 $00 }T             \ add rax, 1000
T{ EBX 1000 ADDI, GOT -> $81 $C3 $E8 $03 $00 $00 }T             \ add ebx, 1000
T{ RBX 1000 SUBI, GOT -> $48 $81 $EB $E8 $03 $00 $00 }T         \ sub rbx, 1000
T{ R13D 8 SUBI, GOT -> $41 $83 $ED $08 }T                       \ sub r13d, 8
T{ RSP 40 SUBI, GOT -> $48 $83 $EC $28 }T                       \ sub rsp, 40
T{ RSP -16 ANDI, GOT -> $48 $83 $E4 $F0 }T                      \ and rsp, -16
T{ EAX 127 CMPI, GOT -> $83 $F8 $7F }T                          \ cmp eax, 127
T{ EAX 128 CMPI, GOT -> $3D $80 $00 $00 $00 }T                  \ cmp eax, 128
T{ EAX -128 CMPI, GOT -> $83 $F8 $80 }T                         \ cmp eax, -128
T{ ECX $FF ANDI, GOT -> $81 $E1 $FF $00 $00 $00 }T              \ and ecx, 0xff
T{ AL 'a' CMPI, GOT -> $3C $61 }T                               \ cmp al, 97
T{ BL $20 SUBI, GOT -> $80 $EB $20 }T                           \ sub bl, 0x20
T{ R9B 1 ORI, GOT -> $41 $80 $C9 $01 }T                         \ or r9b, 1
T{ 0 [RBX] 5 ADDI, GOT -> $83 $03 $05 }T                        \ add dword ptr [rbx], 5
T{ 0 [RBX] QWORD 5 ADDI, GOT -> $48 $83 $03 $05 }T              \ add qword ptr [rbx], 5
T{ 0 [RBX] BYTE 5 ADDI, GOT -> $80 $03 $05 }T                   \ add byte ptr [rbx], 5
T{ 0 [RBX] $1000 CMPI, GOT -> $81 $3B $00 $10 $00 $00 }T        \ cmp dword ptr [rbx], 0x1000
T{ $40000 [R15] 0 CMPI, GOT -> $41 $83 $BF $00 $00 $04 $00 $00 }T \ cmp dword ptr [r15+0x40000], 0
T{ THERE $10 + [RIP] 1 CMPI, GOT -> $83 $3D $09 $00 $00 $00 $01 }T \ cmp dword ptr [rip+9], 1
T{ THERE $10 + [RIP] $1000 CMPI, GOT -> $81 $3D $06 $00 $00 $00 $00 $10 $00 $00 }T \ cmp dword ptr [rip+6], 0x1000
T{ EAX $12345678 XORI, GOT -> $35 $78 $56 $34 $12 }T            \ xor eax, 0x12345678
T{ RAX -2 XORI, GOT -> $48 $83 $F0 $FE }T                       \ xor rax, -2
T{ ECX 3 ADCI, GOT -> $83 $D1 $03 }T                            \ adc ecx, 3
T{ EDX 0 SBBI, GOT -> $83 $DA $00 }T                            \ sbb edx, 0
T{ EAX EBX TEST, GOT -> $85 $D8 }T                              \ test eax, ebx
T{ RAX RAX TEST, GOT -> $48 $85 $C0 }T                          \ test rax, rax
T{ R12D R12D TEST, GOT -> $45 $85 $E4 }T                        \ test r12d, r12d
T{ AL AL TEST, GOT -> $84 $C0 }T                                \ test al, al
T{ 0 [RBX] EAX TEST, GOT -> $85 $03 }T                          \ test dword ptr [rbx], eax
T{ EAX 0 [RBX] TEST, GOT -> $85 $03 }T                          \ test dword ptr [rbx], eax
T{ EAX 1 TESTI, GOT -> $A9 $01 $00 $00 $00 }T                   \ test eax, 1
T{ RAX 1 TESTI, GOT -> $48 $A9 $01 $00 $00 $00 }T               \ test rax, 1
T{ AL 1 TESTI, GOT -> $A8 $01 }T                                \ test al, 1
T{ EBX 1 TESTI, GOT -> $F7 $C3 $01 $00 $00 $00 }T               \ test ebx, 1
T{ CL 1 TESTI, GOT -> $F6 $C1 $01 }T                            \ test cl, 1
T{ 0 [RBX] BYTE 1 TESTI, GOT -> $F6 $03 $01 }T                  \ test byte ptr [rbx], 1
T{ R8 $100 TESTI, GOT -> $49 $F7 $C0 $00 $01 $00 $00 }T         \ test r8, 0x100

\ NOT NEG MUL IMUL DIV IDIV
T{ EAX NOT, GOT -> $F7 $D0 }T                                   \ not eax
T{ RAX NOT, GOT -> $48 $F7 $D0 }T                               \ not rax
T{ 0 [R15] [R13*1] NOT, GOT -> $43 $F7 $14 $2F }T               \ not dword ptr [r15+r13]
T{ CL NOT, GOT -> $F6 $D1 }T                                    \ not cl
T{ EAX NEG, GOT -> $F7 $D8 }T                                   \ neg eax
T{ R8 NEG, GOT -> $49 $F7 $D8 }T                                \ neg r8
T{ ECX MUL, GOT -> $F7 $E1 }T                                   \ mul ecx
T{ RCX MUL, GOT -> $48 $F7 $E1 }T                               \ mul rcx
T{ EBX IMUL, GOT -> $F7 $EB }T                                  \ imul ebx
T{ ECX DIV, GOT -> $F7 $F1 }T                                   \ div ecx
T{ R10 DIV, GOT -> $49 $F7 $F2 }T                               \ div r10
T{ RCX IDIV, GOT -> $48 $F7 $F9 }T                              \ idiv rcx

\ INC DEC
T{ EAX INC, GOT -> $FF $C0 }T                                   \ inc eax
T{ RAX INC, GOT -> $48 $FF $C0 }T                               \ inc rax
T{ R12D INC, GOT -> $41 $FF $C4 }T                              \ inc r12d
T{ ECX DEC, GOT -> $FF $C9 }T                                   \ dec ecx
T{ R13 DEC, GOT -> $49 $FF $CD }T                               \ dec r13
T{ AL INC, GOT -> $FE $C0 }T                                    \ inc al
T{ 0 [RBX] INC, GOT -> $FF $03 }T                               \ inc dword ptr [rbx]
T{ 0 [RBX] BYTE DEC, GOT -> $FE $0B }T                          \ dec byte ptr [rbx]
T{ 0 [RBX] QWORD INC, GOT -> $48 $FF $03 }T                     \ inc qword ptr [rbx]
T{ $40000 [R15] INC, GOT -> $41 $FF $87 $00 $00 $04 $00 }T      \ inc dword ptr [r15+0x40000]

\ Shifts
T{ EAX 1 SHL, GOT -> $D1 $E0 }T                                 \ shl eax, 1
T{ EAX 3 SHL, GOT -> $C1 $E0 $03 }T                             \ shl eax, 3
T{ RAX 32 SHL, GOT -> $48 $C1 $E0 $20 }T                        \ shl rax, 32
T{ RAX 63 SHR, GOT -> $48 $C1 $E8 $3F }T                        \ shr rax, 63
T{ EAX CL SHL, GOT -> $D3 $E0 }T                                \ shl eax, cl
T{ RDX CL SHR, GOT -> $48 $D3 $EA }T                            \ shr rdx, cl
T{ 0 [R15] [R13*1] 31 SAR, GOT -> $43 $C1 $3C $2F $1F }T        \ sar dword ptr [r15+r13], 31
T{ 0 [R15] [R13*1] 1 SAR, GOT -> $43 $D1 $3C $2F }T             \ sar dword ptr [r15+r13], 1
T{ R9D 4 ROL, GOT -> $41 $C1 $C1 $04 }T                         \ rol r9d, 4
T{ AL 1 ROR, GOT -> $D0 $C8 }T                                  \ ror al, 1
T{ BL CL RCL, GOT -> $D2 $D3 }T                                 \ rcl bl, cl
T{ EAX 2 RCR, GOT -> $C1 $D8 $02 }T                             \ rcr eax, 2

\ PUSH POP
T{ RAX PUSH, GOT -> $50 }T                                      \ push rax
T{ RBP PUSH, GOT -> $55 }T                                      \ push rbp
T{ R8 PUSH, GOT -> $41 $50 }T                                   \ push r8
T{ R15 PUSH, GOT -> $41 $57 }T                                  \ push r15
T{ 0 [RBX] PUSH, GOT -> $FF $33 }T                              \ push qword ptr [rbx]
T{ 8 [R12] QWORD PUSH, GOT -> $41 $FF $74 $24 $08 }T            \ push qword ptr [r12+8]
T{ RAX POP, GOT -> $58 }T                                       \ pop rax
T{ R12 POP, GOT -> $41 $5C }T                                   \ pop r12
T{ 0 [RSP] POP, GOT -> $8F $04 $24 }T                           \ pop qword ptr [rsp]
T{ 5 PUSHI, GOT -> $6A $05 }T                                   \ push 5
T{ -2 PUSHI, GOT -> $6A $FE }T                                  \ push -2
T{ 1000 PUSHI, GOT -> $68 $E8 $03 $00 $00 }T                    \ push 1000

\ XCHG LEA SETCC
T{ EAX EBX XCHG, GOT -> $93 }T                                  \ xchg eax, ebx
T{ EBX EAX XCHG, GOT -> $93 }T                                  \ xchg ebx, eax
T{ RAX R8 XCHG, GOT -> $49 $90 }T                               \ xchg rax, r8
T{ ECX EDX XCHG, GOT -> $87 $CA }T                              \ xchg ecx, edx
T{ 0 [RBX] EAX XCHG, GOT -> $87 $03 }T                          \ xchg dword ptr [rbx], eax
T{ ECX 0 [RBX] XCHG, GOT -> $87 $0B }T                          \ xchg ecx, dword ptr [rbx]
T{ AL CL XCHG, GOT -> $86 $C1 }T                                \ xchg al, cl
T{ RAX 8 [RBX] LEA, GOT -> $48 $8D $43 $08 }T                   \ lea rax, [rbx+8]
T{ EAX 8 [RBX] LEA, GOT -> $8D $43 $08 }T                       \ lea eax, [rbx+8]
T{ R12D 4 [R12] LEA, GOT -> $45 $8D $64 $24 $04 }T              \ lea r12d, [r12+4]
T{ RSI 0 [R15] [RAX*1] LEA, GOT -> $49 $8D $34 $07 }T           \ lea rsi, [r15+rax]
T{ RDI $40100 [R15] LEA, GOT -> $49 $8D $BF $00 $01 $04 $00 }T  \ lea rdi, [r15+0x40100]
T{ R15 THERE $2000 + [RIP] LEA, GOT -> $4C $8D $3D $F9 $1F $00 $00 }T \ lea r15, [rip+8185]
T{ RCX THERE -$100 + [RIP] LEA, GOT -> $48 $8D $0D $F9 $FE $FF $FF }T \ lea rcx, [rip-263]
T{ R14 0 [] [RAX*8] LEA, GOT -> $4C $8D $34 $C5 $00 $00 $00 $00 }T \ lea r14, [rax*8]
T{ AL E SETCC, GOT -> $0F $94 $C0 }T                            \ sete al
T{ CL B SETCC, GOT -> $0F $92 $C1 }T                            \ setb cl
T{ R9B NZ SETCC, GOT -> $41 $0F $95 $C1 }T                      \ setne r9b
T{ SIL L SETCC, GOT -> $40 $0F $9C $C6 }T                       \ setl sil
T{ 0 [RBX] BYTE G SETCC, GOT -> $0F $9F $03 }T                  \ setg byte ptr [rbx]

\ Other instructions
T{ 3 INT, GOT -> $CC }T                                         \ int3
T{ $80 INT, GOT -> $CD $80 }T                                   \ int 0x80
T{ RET, GOT -> $C3 }T                                           \ ret
T{ 8 RETN, GOT -> $C2 $08 $00 }T                                \ ret 8
T{ SYSCALL, GOT -> $0F $05 }T                                   \ syscall
T{ RDTSC, GOT -> $0F $31 }T                                     \ rdtsc
T{ CPUID, GOT -> $0F $A2 }T                                     \ cpuid
T{ PAUSE, GOT -> $F3 $90 }T                                     \ pause
T{ NOP, GOT -> $90 }T                                           \ nop
T{ HLT, GOT -> $F4 }T                                           \ hlt
T{ CLD, GOT -> $FC }T                                           \ cld
T{ STD, GOT -> $FD }T                                           \ std
T{ CLI, GOT -> $FA }T                                           \ cli
T{ STI, GOT -> $FB }T                                           \ sti
T{ CLC, GOT -> $F8 }T                                           \ clc
T{ STC, GOT -> $F9 }T                                           \ stc
T{ CMC, GOT -> $F5 }T                                           \ cmc
T{ CDQ, GOT -> $99 }T                                           \ cdq
T{ CQO, GOT -> $48 $99 }T                                       \ cqo
T{ PUSHFQ, GOT -> $9C }T                                        \ pushfq
T{ POPFQ, GOT -> $9D }T                                         \ popfq
T{ MOVSB, GOT -> $A4 }T                                         \ movsb
T{ MOVSD, GOT -> $A5 }T                                         \ movsd
T{ MOVSQ, GOT -> $48 $A5 }T                                     \ movsq
T{ STOSB, GOT -> $AA }T                                         \ stosb
T{ STOSD, GOT -> $AB }T                                         \ stosd
T{ STOSQ, GOT -> $48 $AB }T                                     \ stosq
T{ LODSB, GOT -> $AC }T                                         \ lodsb
T{ LODSQ, GOT -> $48 $AD }T                                     \ lodsq
T{ CMPSB, GOT -> $A6 }T                                         \ cmpsb
T{ SCASB, GOT -> $AE }T                                         \ scasb
T{ REP, MOVSB, GOT -> $F3 $A4 }T                                \ rep movsb
T{ REP, STOSQ, GOT -> $F3 $48 $AB }T                            \ rep stosq
T{ REPNE, SCASB, GOT -> $F2 $AE }T                              \ repne scasb
T{ LOCK, 0 [RBX] INC, GOT -> $F0 $FF $03 }T                     \ lock inc dword ptr [rbx]

\ Indirect jumps and calls
T{ RAX JMP-IND, GOT -> $FF $E0 }T                               \ jmp rax
T{ R11 JMP-IND, GOT -> $41 $FF $E3 }T                           \ jmp r11
T{ 0 [R14] [RAX*8] JMP-IND, GOT -> $41 $FF $24 $C6 }T           \ jmp qword ptr [r14+rax*8]
T{ RAX CALL-IND, GOT -> $FF $D0 }T                              \ call rax
T{ R10 CALL-IND, GOT -> $41 $FF $D2 }T                          \ call r10
T{ 8 [RAX] CALL-IND, GOT -> $FF $50 $08 }T                      \ call qword ptr [rax+8]
T{ $58 [R12] CALL-IND, GOT -> $41 $FF $54 $24 $58 }T            \ call qword ptr [r12+0x58]
T{ THERE $100 + [RIP] CALL-IND, GOT -> $FF $15 $FA $00 $00 $00 }T \ call qword ptr [rip+250]

\ ==== Part 2: labels, jumps, structures and [RIP] ====
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
  L1 JRCXZ,
  L1 JMP,
  L1 JC,
LABEL L2
  RET,
LABEL L3
  $41 TC,  4 TALIGN
LABEL STRUCT
  EAX EAX OR,  Z IF,  EAX INC,  THEN,
  RCX RCX TEST,  NZ IF,  EAX INC,  ELSE,  EAX DEC,  THEN,
  BEGIN,  EAX DEC,  Z UNTIL,
  BEGIN,  ECX ECX OR,  NZ WHILE,  ECX DEC,  REPEAT,
  BEGIN,  NOP,  AGAIN,
LABEL DATA
  $12345678 T,
LABEL RIPS
  EAX DATA [RIP] MOV,
  DATA [RIP] 5 ADDI,
  RAX START [RIP] LEA,
  R15 THERE [RIP] LEA,
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
T{ DATA RIPS -> $402048 $40204C }T
T{ BACK FAR -> $402067 $40213F }T
T{ $402000 5 AT -> $E9 $15 $00 $00 $00 }T        \ jmp 0x40201a (near: L1 had no place yet)
T{ $402005 6 AT -> $0F $84 $0F $00 $00 $00 }T    \ je 0x40201a (near)
T{ $40200B 5 AT -> $E8 $15 $00 $00 $00 }T        \ call 0x402025
T{ $402010 5 AT -> $BA $26 $20 $40 $00 }T        \ mov edx, 0x402026
T{ $402015 5 AT -> $68 $26 $20 $40 $00 }T        \ push 0x402026
T{ $40201A 1 AT -> $90 }T                        \ nop
T{ $40201B 2 AT -> $75 $FD }T                    \ jne 0x40201a
T{ $40201D 2 AT -> $E2 $FB }T                    \ loop 0x40201a
T{ $40201F 2 AT -> $E3 $F9 }T                    \ jrcxz 0x40201a
T{ $402021 2 AT -> $EB $F7 }T                    \ jmp 0x40201a (short: L1 has its place)
T{ $402023 2 AT -> $72 $F5 }T                    \ jb 0x40201a
T{ $402025 3 AT -> $C3 $41 $00 }T                \ ret; the data byte; 4 TALIGN
T{ $402028 6 AT -> $09 $C0 $75 $02 $FF $C0 }T                  \ or eax, eax; jne 0x40202e; inc eax
T{ $40202E 11 AT -> $48 $85 $C9 $74 $04 $FF $C0 $EB $02 $FF $C8 }T
                       \ test rcx, rcx; je 0x402037; inc eax; jmp 0x402039; dec eax
T{ $402039 4 AT -> $FF $C8 $75 $FC }T                          \ dec eax; jne 0x402039
T{ $40203D 8 AT -> $09 $C9 $74 $04 $FF $C9 $EB $F8 }T          \ or ecx, ecx; je 0x402045; dec ecx; jmp 0x40203d
T{ $402045 3 AT -> $90 $EB $FD }T                              \ nop; jmp 0x402045
T{ DATA T@ -> $12345678 }T
T{ $40204C 6 AT -> $8B $05 $F6 $FF $FF $FF }T                  \ mov eax, dword ptr [rip-0xa]: 0x402048
T{ $402052 7 AT -> $83 $05 $EF $FF $FF $FF $05 }T              \ add dword ptr [rip-0x11], 5: 0x402048
T{ $402059 7 AT -> $48 $8D $05 $A0 $FF $FF $FF }T              \ lea rax, [rip-0x60]: 0x402000
T{ $402060 7 AT -> $4C $8D $3D $F9 $FF $FF $FF }T              \ lea r15, [rip-7]: 0x402060
T{ $402067 5 AT -> $E9 $D3 $00 $00 $00 }T        \ jmp 0x40213f
T{ $402134 5 AT -> $E9 $2E $FF $FF $FF }T        \ jmp 0x402067 (near: too far for 8 bits)
T{ $402139 6 AT -> $0F $84 $28 $FF $FF $FF }T    \ je 0x402067 (near)
T{ $40213F 5 AT -> $E8 $BC $FE $FF $FF }T        \ call 0x402000
T{ THERE -> $402144 }T

\ ==== Part 3: errors ====
\ Each case starts new code (NEW). An error is THROW -2 with a message;
\ MSG" compares the message. Words that parse (LABEL) run in a word.

: NEW ( -- )  $402000 ORG ;
: MSG" ( "text<quote>" -- f )  34 PARSE  ERRMSG @ ERRMSG CELL+ @ S= ;

: E1  NEW EAX BL MOV, ;            T{ ' E1  CATCH MSG" the operands have different sizes" -> -2 TRUE }T
: E2  NEW RAX EBX MOV, ;           T{ ' E2  CATCH MSG" the operands have different sizes" -> -2 TRUE }T
: E3  NEW 0 [RBX] 0 [RSI] MOV, ;   T{ ' E3  CATCH MSG" one operand must be a register" -> -2 TRUE }T
: E4  NEW EAX 32 SHL, ;            T{ ' E4  CATCH MSG" a shift takes CL or a number from 0 to 31 (64 bits: 63)" -> -2 TRUE }T
: E5  NEW RAX 64 SHL, ;            T{ ' E5  CATCH MSG" a shift takes CL or a number from 0 to 31 (64 bits: 63)" -> -2 TRUE }T
: E6  NEW 5 EAX MOV, ;             T{ ' E6  CATCH MSG" an operand is missing" -> -2 TRUE }T
: E7  NEW EAX BYTE ;               T{ ' E7  CATCH MSG" BYTE needs a memory operand before it" -> -2 TRUE }T
: E8  NEW 0 [RBX] BYTE QWORD ;     T{ ' E8  CATCH MSG" QWORD needs a memory operand before it" -> -2 TRUE }T
: E9  NEW EAX PUSH, ;              T{ ' E9  CATCH MSG" PUSH, takes 64 bits" -> -2 TRUE }T
: E10 NEW 0 [RBX] BYTE POP, ;      T{ ' E10 CATCH MSG" POP, takes 64 bits" -> -2 TRUE }T
: E11 NEW EAX 0 [RBX] BYTE MOV, ;  T{ ' E11 CATCH MSG" the operands have different sizes" -> -2 TRUE }T
: E12 NEW EAX [RAX*4] ;            T{ ' E12 CATCH MSG" an index needs a memory operand before it" -> -2 TRUE }T
: E13 NEW 0 [RBX] [RAX*4] [RCX*2] ;
T{ ' E13 CATCH MSG" a memory operand has one index register" -> -2 TRUE }T
: E14 NEW 0 [RIP] [RAX*4] ;        T{ ' E14 CATCH MSG" [RIP] takes no index" -> -2 TRUE }T
: E15 NEW RAX 0 [RBX] LEA, ;       \ correct: no error
T{ ' E15 CATCH -> 0 }T
: E16 NEW AL 0 [RBX] LEA, ;        T{ ' E16 CATCH MSG" LEA, needs a register of 32 or 64 bits and memory" -> -2 TRUE }T
: E17 NEW AL BL MOVZX, ;           T{ ' E17 CATCH MSG" needs a register of 32 or 64 bits first" -> -2 TRUE }T
: E18 NEW EAX EBX MOVZX, ;         T{ ' E18 CATCH MSG" needs a byte register or BYTE memory second" -> -2 TRUE }T
: E19 NEW EAX EBX MOVSXD, ;        T{ ' E19 CATCH MSG" MOVSXD, needs a register of 64 bits first" -> -2 TRUE }T
: E20 NEW RAX RBX MOVSXD, ;        T{ ' E20 CATCH MSG" MOVSXD, needs 32 bits second" -> -2 TRUE }T
: E21 NEW EAX E SETCC, ;           T{ ' E21 CATCH MSG" SETCC, needs a byte register or BYTE memory" -> -2 TRUE }T
: E22 NEW EAX JMP-IND, ;           T{ ' E22 CATCH MSG" JMP-IND, takes a register of 64 bits or memory" -> -2 TRUE }T
: E23 NEW AH R8B MOV, ;
T{ ' E23 CATCH MSG" AH CH DH BH do not go with a REX prefix (R8-R15, SPL-DIL, 64 bits)" -> -2 TRUE }T
: E24 NEW AH SIL MOV, ;
T{ ' E24 CATCH MSG" AH CH DH BH do not go with a REX prefix (R8-R15, SPL-DIL, 64 bits)" -> -2 TRUE }T
: E25 NEW RAX AH MOVZX, ;
T{ ' E25 CATCH MSG" AH CH DH BH do not go with a REX prefix (R8-R15, SPL-DIL, 64 bits)" -> -2 TRUE }T
: E26 NEW AH BL MOV, ;             \ correct: no REX prefix
T{ ' E26 CATCH -> 0 }T
: E27 NEW BEGIN, 130 NOPS Z UNTIL, ;
T{ ' E27 CATCH MSG" the jump is too far for 8 bits (127 bytes)" -> -2 TRUE }T
: E28 NEW Z IF, 130 NOPS THEN, ;
T{ ' E28 CATCH MSG" the jump is too far for 8 bits (127 bytes)" -> -2 TRUE }T
: E29 NEW $412000 TC@ ;            \ ORG $402000: the code reaches up to $411FFF
T{ ' E29 CATCH MSG" address outside the target memory" -> -2 TRUE }T

\ Labels: a short jump too far, without place, placed twice, not a
\ jump, [RIP] before the place. LABEL parses its name, so it runs in
\ the line itself.
NEW L1 LOOP, 130 NOPS
T{ ' LABEL CATCH L1 MSG" the jump is too far for 8 bits (127 bytes)" -> -2 TRUE }T
NEW FORWARD X5  L1 JMP,            \ a named label starts a new scope: L1 waits
T{ ' LABEL CATCH X5 MSG" label without place: L1" -> -2 TRUE }T
NEW LABEL L2
T{ ' LABEL CATCH L2 MSG" label placed twice: L2" -> -2 TRUE }T
NEW L3 [] EAX MOV,
T{ ' LABEL CATCH L3 MSG" a label before its place is only for a jump, CALL, MOVI, to EAX-EDI or PUSHI," -> -2 TRUE }T
NEW FORWARD X6  X6 JMP,
T{ ' CHECK-LABELS CATCH MSG" label without place: X6" -> -2 TRUE }T
: E30 NEW EAX L4 [RIP] MOV, ;      T{ ' E30 CATCH MSG" [RIP] needs a label with its place" -> -2 TRUE }T
TARGET CONSTANT SAVED-TARGET
: E31 0 TO TARGET  $402000 TC@ ;   \ no target memory yet: as before the first ORG
T{ ' E31 CATCH MSG" no target memory: ORG first" -> -2 TRUE }T
SAVED-TARGET TO TARGET
NEW

S" x64" .RESULTS
