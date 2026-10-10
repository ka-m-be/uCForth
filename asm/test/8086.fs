\ 8086.fs - tests of the package I8086 (asm/8086.fs)
\
\ Load (in the folder ucf), after the package FILES and the tester:
\   build/ucf-c host/files.fs forth/test/tester.fs asm/test/8086.fs
\ The tests run with cells of 16 and of 32 bits.
\
\ Part 1: single instructions. The expected bytes come from clang 21
\ (Apple LLVM, "clang -target i386-pc-none-elf" with ".code16" and
\ ".intel_syntax noprefix"), once, and are kept here as data. The
\ comment of each test is the instruction for clang. GOT gives the
\ bytes since the last GOT, the first byte deepest. (The name G is the
\ condition "greater" of the assembler.)
\ Part 2: labels, jumps and structures in one piece of code at $0100.
\ The expected bytes come from this assembler; objdump (LLVM 21, 16-bit
\ mode) showed them as the instructions in the comments, each jump to
\ the right address.
\ Part 3: the errors, each with its message.
\ Part 4: code in place (IN-PLACE), as CODE words on a PC put it.

ONLY FORTH DEFINITIONS  ALSO FILES
S" lib/text.fs" INCLUDED
S" asm/8086.fs" INCLUDED
ONLY FORTH DEFINITIONS  ALSO INTERNALS  ALSO TEXT  ALSO I8086
DECIMAL

VARIABLE MARK
: GOT ( -- c1 ... cn )   \ the bytes since the last GOT
  MARK @  BEGIN DUP THERE U< WHILE  DUP TC@ SWAP 1+  REPEAT  DROP  THERE MARK ! ;
: AT ( ta n -- c1 ... cn )   \ n bytes at ta
  0 ?DO  DUP TC@ SWAP 1+  LOOP  DROP ;

\ ==== Part 1: single instructions ====
0 ORG  THERE MARK !

T{ AX BX MOV, GOT -> $89 $D8 }T                                \ mov ax, bx
T{ BX AX MOV, GOT -> $89 $C3 }T                                \ mov bx, ax
T{ CX DX MOV, GOT -> $89 $D1 }T                                \ mov cx, dx
T{ SP BP MOV, GOT -> $89 $EC }T                                \ mov sp, bp
T{ SI DI MOV, GOT -> $89 $FE }T                                \ mov si, di
T{ AL BH MOV, GOT -> $88 $F8 }T                                \ mov al, bh
T{ AX 0 [BX] MOV, GOT -> $8B $07 }T                            \ mov ax, word ptr [bx]
T{ AX 4 [BP] MOV, GOT -> $8B $46 $04 }T                        \ mov ax, word ptr [bp+4]
T{ DX 0 [BP] MOV, GOT -> $8B $56 $00 }T                        \ mov dx, word ptr [bp]
T{ CX -2 [SI] MOV, GOT -> $8B $4C $FE }T                       \ mov cx, word ptr [si-2]
T{ BX $100 [DI] MOV, GOT -> $8B $9D $00 $01 }T                 \ mov bx, word ptr [di+0x100]
T{ SI 2 [BX+SI] MOV, GOT -> $8B $70 $02 }T                     \ mov si, word ptr [bx+si+2]
T{ DI 0 [BX+DI] MOV, GOT -> $8B $39 }T                         \ mov di, word ptr [bx+di]
T{ AX 0 [BP+SI] MOV, GOT -> $8B $02 }T                         \ mov ax, word ptr [bp+si]
T{ AX $1234 [BP+DI] MOV, GOT -> $8B $83 $34 $12 }T             \ mov ax, word ptr [bp+di+0x1234]
T{ AX 127 [SI] MOV, GOT -> $8B $44 $7F }T                      \ mov ax, word ptr [si+127]
T{ AX 128 [SI] MOV, GOT -> $8B $84 $80 $00 }T                  \ mov ax, word ptr [si+128]
T{ AX -128 [SI] MOV, GOT -> $8B $44 $80 }T                     \ mov ax, word ptr [si-128]
T{ AX $1234 [] MOV, GOT -> $A1 $34 $12 }T                      \ mov ax, word ptr [0x1234]
T{ AL $12 [] MOV, GOT -> $A0 $12 $00 }T                        \ mov al, byte ptr [0x12]
T{ BX $1234 [] MOV, GOT -> $8B $1E $34 $12 }T                  \ mov bx, word ptr [0x1234]
T{ AL 0 [SI] MOV, GOT -> $8A $04 }T                            \ mov al, byte ptr [si]
T{ 0 [BX] AX MOV, GOT -> $89 $07 }T                            \ mov word ptr [bx], ax
T{ 4 [BP] SI MOV, GOT -> $89 $76 $04 }T                        \ mov word ptr [bp+4], si
T{ $1234 [] AX MOV, GOT -> $A3 $34 $12 }T                      \ mov word ptr [0x1234], ax
T{ $12 [] AL MOV, GOT -> $A2 $12 $00 }T                        \ mov byte ptr [0x12], al
T{ $1234 [] DX MOV, GOT -> $89 $16 $34 $12 }T                  \ mov word ptr [0x1234], dx
T{ 0 [DI] CL MOV, GOT -> $88 $0D }T                            \ mov byte ptr [di], cl
T{ ES AX MOV, GOT -> $8E $C0 }T                                \ mov es, ax
T{ DS BX MOV, GOT -> $8E $DB }T                                \ mov ds, bx
T{ SS CX MOV, GOT -> $8E $D1 }T                                \ mov ss, cx
T{ ES 4 [BX] MOV, GOT -> $8E $47 $04 }T                        \ mov es, word ptr [bx+4]
T{ AX DS MOV, GOT -> $8C $D8 }T                                \ mov ax, ds
T{ DX CS MOV, GOT -> $8C $CA }T                                \ mov dx, cs
T{ 0 [BX] ES MOV, GOT -> $8C $07 }T                            \ mov word ptr [bx], es
T{ AX $1234 MOVI, GOT -> $B8 $34 $12 }T                        \ mov ax, 0x1234
T{ BX 0 MOVI, GOT -> $BB $00 $00 }T                            \ mov bx, 0
T{ SI -1 MOVI, GOT -> $BE $FF $FF }T                           \ mov si, 0xffff
T{ DI 2 MOVI, GOT -> $BF $02 $00 }T                            \ mov di, 2
T{ AL 5 MOVI, GOT -> $B0 $05 }T                                \ mov al, 5
T{ BH $80 MOVI, GOT -> $B7 $80 }T                              \ mov bh, 0x80
T{ 0 [BX] 7 MOVI, GOT -> $C7 $07 $07 $00 }T                    \ mov word ptr [bx], 7
T{ 0 [DI] BYTE 7 MOVI, GOT -> $C6 $05 $07 }T                   \ mov byte ptr [di], 7
T{ $1234 [BX+DI] $5678 MOVI, GOT -> $C7 $81 $34 $12 $78 $56 }T \ mov word ptr [bx+di+0x1234], 0x5678
T{ -3 [BP+DI] BYTE 7 MOVI, GOT -> $C6 $43 $FD $07 }T           \ mov byte ptr [bp+di-3], 7
T{ $106 [] 0 MOVI, GOT -> $C7 $06 $06 $01 $00 $00 }T           \ mov word ptr [0x106], 0
T{ AX BX ADD, GOT -> $01 $D8 }T                                \ add ax, bx
T{ AX BX OR, GOT -> $09 $D8 }T                                 \ or ax, bx
T{ AX BX ADC, GOT -> $11 $D8 }T                                \ adc ax, bx
T{ AX BX SBB, GOT -> $19 $D8 }T                                \ sbb ax, bx
T{ AX BX AND, GOT -> $21 $D8 }T                                \ and ax, bx
T{ AX BX SUB, GOT -> $29 $D8 }T                                \ sub ax, bx
T{ AX BX XOR, GOT -> $31 $D8 }T                                \ xor ax, bx
T{ AX BX CMP, GOT -> $39 $D8 }T                                \ cmp ax, bx
T{ AL CL ADD, GOT -> $00 $C8 }T                                \ add al, cl
T{ DH BL XOR, GOT -> $30 $DE }T                                \ xor dh, bl
T{ CX 0 [SI] ADD, GOT -> $03 $0C }T                            \ add cx, word ptr [si]
T{ 0 [SI] CX ADD, GOT -> $01 $0C }T                            \ add word ptr [si], cx
T{ AL 0 [BX] CMP, GOT -> $3A $07 }T                            \ cmp al, byte ptr [bx]
T{ 0 [BX] AL CMP, GOT -> $38 $07 }T                            \ cmp byte ptr [bx], al
T{ DX 2 [BP] SUB, GOT -> $2B $56 $02 }T                        \ sub dx, word ptr [bp+2]
T{ 0 [BX] AX ADD, GOT -> $01 $07 }T                            \ add word ptr [bx], ax
T{ AX $106 [] SUB, GOT -> $2B $06 $06 $01 }T                   \ sub ax, word ptr [0x106]
T{ AX 1 ADDI, GOT -> $83 $C0 $01 }T                            \ add ax, 1
T{ AX 1000 ADDI, GOT -> $05 $E8 $03 }T                         \ add ax, 1000
T{ BX 1000 ADDI, GOT -> $81 $C3 $E8 $03 }T                     \ add bx, 1000
T{ BX -2 ADDI, GOT -> $83 $C3 $FE }T                           \ add bx, -2
T{ AX $FF ANDI, GOT -> $25 $FF $00 }T                          \ and ax, 0xff
T{ AL 5 CMPI, GOT -> $3C $05 }T                                \ cmp al, 5
T{ BL 5 CMPI, GOT -> $80 $FB $05 }T                            \ cmp bl, 5
T{ DI 127 CMPI, GOT -> $83 $FF $7F }T                          \ cmp di, 127
T{ DI 128 CMPI, GOT -> $81 $FF $80 $00 }T                      \ cmp di, 128
T{ SP 4 ADDI, GOT -> $83 $C4 $04 }T                            \ add sp, 4
T{ BP 6 SUBI, GOT -> $83 $ED $06 }T                            \ sub bp, 6
T{ 0 [BX] 1 ADDI, GOT -> $83 $07 $01 }T                        \ add word ptr [bx], 1
T{ 0 [BX] BYTE 1 ADDI, GOT -> $80 $07 $01 }T                   \ add byte ptr [bx], 1
T{ $106 [] 0 CMPI, GOT -> $83 $3E $06 $01 $00 }T               \ cmp word ptr [0x106], 0
T{ AX $8000 XORI, GOT -> $35 $00 $80 }T                        \ xor ax, 0x8000
T{ CL 3 ANDI, GOT -> $80 $E1 $03 }T                            \ and cl, 3
T{ DX 1 ADCI, GOT -> $83 $D2 $01 }T                            \ adc dx, 1
T{ DX 0 SBBI, GOT -> $83 $DA $00 }T                            \ sbb dx, 0
T{ AH $80 ORI, GOT -> $80 $CC $80 }T                           \ or ah, 0x80
T{ CX 16 CMPI, GOT -> $83 $F9 $10 }T                           \ cmp cx, 16
T{ AL $7F ANDI, GOT -> $24 $7F }T                              \ and al, 0x7f
T{ DX -1 XORI, GOT -> $83 $F2 $FF }T                           \ xor dx, -1
T{ AX BX TEST, GOT -> $85 $D8 }T                               \ test ax, bx
T{ AL AL TEST, GOT -> $84 $C0 }T                               \ test al, al
T{ 0 [BX] AX TEST, GOT -> $85 $07 }T                           \ test word ptr [bx], ax
T{ AX 0 [BX] TEST, GOT -> $85 $07 }T                           \ test ax, word ptr [bx]
T{ AX $8000 TESTI, GOT -> $A9 $00 $80 }T                       \ test ax, 0x8000
T{ AL 1 TESTI, GOT -> $A8 $01 }T                               \ test al, 1
T{ BL 1 TESTI, GOT -> $F6 $C3 $01 }T                           \ test bl, 1
T{ CX 3 TESTI, GOT -> $F7 $C1 $03 $00 }T                       \ test cx, 3
T{ 0 [BX] BYTE 1 TESTI, GOT -> $F6 $07 $01 }T                  \ test byte ptr [bx], 1
T{ AX NOT, GOT -> $F7 $D0 }T                                   \ not ax
T{ AX NEG, GOT -> $F7 $D8 }T                                   \ neg ax
T{ CX MUL, GOT -> $F7 $E1 }T                                   \ mul cx
T{ DX MUL, GOT -> $F7 $E2 }T                                   \ mul dx
T{ BX IMUL, GOT -> $F7 $EB }T                                  \ imul bx
T{ CX DIV, GOT -> $F7 $F1 }T                                   \ div cx
T{ BX IDIV, GOT -> $F7 $FB }T                                  \ idiv bx
T{ AL NEG, GOT -> $F6 $D8 }T                                   \ neg al
T{ BL MUL, GOT -> $F6 $E3 }T                                   \ mul bl
T{ 0 [BX] NOT, GOT -> $F7 $17 }T                               \ not word ptr [bx]
T{ 0 [BX] BYTE NEG, GOT -> $F6 $1F }T                          \ neg byte ptr [bx]
T{ AX INC, GOT -> $40 }T                                       \ inc ax
T{ DI DEC, GOT -> $4F }T                                       \ dec di
T{ SP INC, GOT -> $44 }T                                       \ inc sp
T{ AL INC, GOT -> $FE $C0 }T                                   \ inc al
T{ CH DEC, GOT -> $FE $CD }T                                   \ dec ch
T{ 0 [BX] INC, GOT -> $FF $07 }T                               \ inc word ptr [bx]
T{ 0 [BX] BYTE INC, GOT -> $FE $07 }T                          \ inc byte ptr [bx]
T{ 2 [BP] DEC, GOT -> $FF $4E $02 }T                           \ dec word ptr [bp+2]
T{ AX 1 SHL, GOT -> $D1 $E0 }T                                 \ shl ax, 1
T{ AX CL SHL, GOT -> $D3 $E0 }T                                \ shl ax, cl
T{ DX 1 SHR, GOT -> $D1 $EA }T                                 \ shr dx, 1
T{ AX 1 SAR, GOT -> $D1 $F8 }T                                 \ sar ax, 1
T{ BX 1 ROL, GOT -> $D1 $C3 }T                                 \ rol bx, 1
T{ CX 1 ROR, GOT -> $D1 $C9 }T                                 \ ror cx, 1
T{ DX 1 RCL, GOT -> $D1 $D2 }T                                 \ rcl dx, 1
T{ AX 1 RCR, GOT -> $D1 $D8 }T                                 \ rcr ax, 1
T{ AL CL SHR, GOT -> $D2 $E8 }T                                \ shr al, cl
T{ DI CL SAR, GOT -> $D3 $FF }T                                \ sar di, cl
T{ 0 [BX] 1 SAR, GOT -> $D1 $3F }T                             \ sar word ptr [bx], 1
T{ 0 [BX] BYTE CL SHL, GOT -> $D2 $27 }T                       \ shl byte ptr [bx], cl
T{ AX PUSH, GOT -> $50 }T                                      \ push ax
T{ DI PUSH, GOT -> $57 }T                                      \ push di
T{ ES PUSH, GOT -> $06 }T                                      \ push es
T{ CS PUSH, GOT -> $0E }T                                      \ push cs
T{ SS PUSH, GOT -> $16 }T                                      \ push ss
T{ DS PUSH, GOT -> $1E }T                                      \ push ds
T{ 2 [BX] PUSH, GOT -> $FF $77 $02 }T                          \ push word ptr [bx+2]
T{ 0 [BP] PUSH, GOT -> $FF $76 $00 }T                          \ push word ptr [bp]
T{ $106 [] PUSH, GOT -> $FF $36 $06 $01 }T                     \ push word ptr [0x106]
T{ AX POP, GOT -> $58 }T                                       \ pop ax
T{ SI POP, GOT -> $5E }T                                       \ pop si
T{ ES POP, GOT -> $07 }T                                       \ pop es
T{ DS POP, GOT -> $1F }T                                       \ pop ds
T{ SS POP, GOT -> $17 }T                                       \ pop ss
T{ 0 [BX] POP, GOT -> $8F $07 }T                               \ pop word ptr [bx]
T{ AX BX XCHG, GOT -> $93 }T                                   \ xchg ax, bx
T{ BX AX XCHG, GOT -> $93 }T                                   \ xchg bx, ax
T{ BX CX XCHG, GOT -> $87 $D9 }T                               \ xchg bx, cx
T{ AL BL XCHG, GOT -> $86 $C3 }T                               \ xchg al, bl
T{ AX 0 [BX] XCHG, GOT -> $87 $07 }T                           \ xchg ax, word ptr [bx]
T{ 0 [BX] AX XCHG, GOT -> $87 $07 }T                           \ xchg word ptr [bx], ax
T{ SI 2 [BX] LEA, GOT -> $8D $77 $02 }T                        \ lea si, [bx+2]
T{ AX 4 [BP] LEA, GOT -> $8D $46 $04 }T                        \ lea ax, [bp+4]
T{ DI 0 [BX] LES, GOT -> $C4 $3F }T                            \ les di, [bx]
T{ SI 0 [BX] LDS, GOT -> $C5 $37 }T                            \ lds si, [bx]
T{ AL $60 IN, GOT -> $E4 $60 }T                                \ in al, 0x60
T{ AX $40 IN, GOT -> $E5 $40 }T                                \ in ax, 0x40
T{ AL DX IN, GOT -> $EC }T                                     \ in al, dx
T{ AX DX IN, GOT -> $ED }T                                     \ in ax, dx
T{ $43 AL OUT, GOT -> $E6 $43 }T                               \ out 0x43, al
T{ $42 AX OUT, GOT -> $E7 $42 }T                               \ out 0x42, ax
T{ DX AL OUT, GOT -> $EE }T                                    \ out dx, al
T{ DX AX OUT, GOT -> $EF }T                                    \ out dx, ax
T{ $21 INT, GOT -> $CD $21 }T                                  \ int 0x21
T{ 3 INT, GOT -> $CC }T                                        \ int 3
T{ $1A INT, GOT -> $CD $1A }T                                  \ int 0x1a
T{ MOVSB, GOT -> $A4 }T                                        \ movsb
T{ MOVSW, GOT -> $A5 }T                                        \ movsw
T{ CMPSB, GOT -> $A6 }T                                        \ cmpsb
T{ CMPSW, GOT -> $A7 }T                                        \ cmpsw
T{ STOSB, GOT -> $AA }T                                        \ stosb
T{ STOSW, GOT -> $AB }T                                        \ stosw
T{ LODSB, GOT -> $AC }T                                        \ lodsb
T{ LODSW, GOT -> $AD }T                                        \ lodsw
T{ SCASB, GOT -> $AE }T                                        \ scasb
T{ SCASW, GOT -> $AF }T                                        \ scasw
T{ CLC, GOT -> $F8 }T                                          \ clc
T{ STC, GOT -> $F9 }T                                          \ stc
T{ CMC, GOT -> $F5 }T                                          \ cmc
T{ CLD, GOT -> $FC }T                                          \ cld
T{ STD, GOT -> $FD }T                                          \ std
T{ CLI, GOT -> $FA }T                                          \ cli
T{ STI, GOT -> $FB }T                                          \ sti
T{ CBW, GOT -> $98 }T                                          \ cbw
T{ CWD, GOT -> $99 }T                                          \ cwd
T{ NOP, GOT -> $90 }T                                          \ nop
T{ HLT, GOT -> $F4 }T                                          \ hlt
T{ LAHF, GOT -> $9F }T                                         \ lahf
T{ SAHF, GOT -> $9E }T                                         \ sahf
T{ PUSHF, GOT -> $9C }T                                        \ pushf
T{ POPF, GOT -> $9D }T                                         \ popf
T{ XLAT, GOT -> $D7 }T                                         \ xlatb
T{ RET, GOT -> $C3 }T                                        \ retw (AT&T: clang takes "ret" as 32 bits)
T{ RETF, GOT -> $CB }T                                       \ lretw (AT&T)
T{ IRET, GOT -> $CF }T                                         \ iret
T{ REP, MOVSB, GOT -> $F3 $A4 }T                               \ rep movsb
T{ REP, STOSW, GOT -> $F3 $AB }T                               \ rep stosw
T{ REPNE, SCASB, GOT -> $F2 $AE }T                             \ repne scasb
T{ REPE, CMPSB, GOT -> $F3 $A6 }T                              \ repe cmpsb
T{ CS: AX 0 [BX] MOV, GOT -> $2E $8B $07 }T                    \ mov ax, word ptr cs:[bx]
T{ ES: 0 [DI] AL MOV, GOT -> $26 $88 $05 }T                    \ mov byte ptr es:[di], al
T{ SS: AX 0 [SI] MOV, GOT -> $36 $8B $04 }T                    \ mov ax, word ptr ss:[si]
T{ DS: AX 0 [BP] MOV, GOT -> $3E $8B $46 $00 }T                \ mov ax, word ptr ds:[bp]
T{ CS: AX $120 [] MOV, GOT -> $2E $A1 $20 $01 }T               \ mov ax, word ptr cs:[0x120]
T{ 0 [DI] JMP-IND, GOT -> $FF $25 }T                           \ jmp word ptr [di]
T{ CS: $100 [DI] JMP-IND, GOT -> $2E $FF $A5 $00 $01 }T        \ jmp word ptr cs:[di+0x100]
T{ BX JMP-IND, GOT -> $FF $E3 }T                               \ jmp bx
T{ 0 [BX] CALL-IND, GOT -> $FF $17 }T                          \ call word ptr [bx]
T{ AX CALL-IND, GOT -> $FF $D0 }T                              \ call ax
T{ $F000 $E05B JMP-FAR, GOT -> $EA $5B $E0 $00 $F0 }T            \ ljmp 0xf000, 0xe05b
T{ $1234 $5678 CALL-FAR, GOT -> $9A $78 $56 $34 $12 }T          \ lcall 0x1234, 0x5678
T{ 0 [BX] JMP-FAR-IND, GOT -> $FF $2F }T                        \ ljmp word ptr [bx]
T{ 2 [BX+SI] CALL-FAR-IND, GOT -> $FF $58 $02 }T                \ lcall word ptr [bx+si+2]
T{ -4 [BP] JMP-FAR-IND, GOT -> $FF $6E $FC }T                   \ ljmp word ptr [bp-4]
T{ $1234 [] CALL-FAR-IND, GOT -> $FF $1E $34 $12 }T             \ lcall word ptr [0x1234]
T{ $100 [DI] JMP-FAR-IND, GOT -> $FF $AD $00 $01 }T             \ ljmp word ptr [di+0x100]

\ ==== Part 2: labels, jumps and structures ====
: NOPS ( n -- )  0 ?DO NOP, LOOP ;

$100 ORG
LABEL START
  L1 JMP,
  L1 JZ,
  L2 CALL,
  DX L3 MOVI,
LABEL L1
  NOP,
  L1 JNZ,
  L1 LOOP,
  L1 JCXZ,
  L1 JMP,
LABEL L2
  RET,
LABEL L3
  $41 TC,  TALIGN
LABEL STRUCT
  AX AX OR,  Z IF,  AX INC,  THEN,
  CX CX TEST,  NZ IF,  AX INC,  ELSE,  AX DEC,  THEN,
  BEGIN,  AX DEC,  Z UNTIL,
  BEGIN,  CX CX OR,  NZ WHILE,  CX DEC,  REPEAT,
  BEGIN,  NOP,  AGAIN,
FORWARD FAR
LABEL BACK
  FAR JMP,
  200 NOPS
  BACK JMP,
LABEL FAR
  START CALL,
CHECK-LABELS

T{ START -> $100 }T
T{ STRUCT -> $116 }T
T{ BACK FAR -> $130 $1FE }T
T{ $100 3 AT -> $E9 $08 $00 }T   \ jmp 0x10b (near: L1 had no place yet)
T{ $103 2 AT -> $74 $06 }T       \ je 0x10b
T{ $105 3 AT -> $E8 $0C $00 }T   \ call 0x114
T{ $108 3 AT -> $BA $15 $01 }T   \ mov dx, 0x115
T{ $10B 1 AT -> $90 }T           \ nop
T{ $10C 2 AT -> $75 $FD }T       \ jne 0x10b
T{ $10E 2 AT -> $E2 $FB }T       \ loop 0x10b
T{ $110 2 AT -> $E3 $F9 }T       \ jcxz 0x10b
T{ $112 2 AT -> $EB $F7 }T       \ jmp 0x10b (short: L1 has its place)
T{ $114 2 AT -> $C3 $41 }T       \ ret; the data byte
T{ $116 5 AT -> $09 $C0 $75 $01 $40 }T              \ or ax, ax; jne 0x11b; inc ax
T{ $11B 8 AT -> $85 $C9 $74 $03 $40 $EB $01 $48 }T  \ test cx, cx; je 0x122; inc ax; jmp 0x123; dec ax
T{ $123 3 AT -> $48 $75 $FD }T                      \ dec ax; jne 0x123
T{ $126 7 AT -> $09 $C9 $74 $03 $49 $EB $F9 }T      \ or cx, cx; je 0x12d; dec cx; jmp 0x126
T{ $12D 3 AT -> $90 $EB $FD }T                      \ nop; jmp 0x12d
T{ $130 3 AT -> $E9 $CB $00 }T   \ jmp 0x1fe
T{ $1FB 3 AT -> $E9 $32 $FF }T   \ jmp 0x130
T{ $1FE 3 AT -> $E8 $FF $FE }T   \ call 0x100
T{ THERE -> $201 }T

\ ==== Part 3: errors ====
\ Each case starts new code (NEW). An error is THROW -2 with a message;
\ MSG" compares the message. Words that parse (LABEL) run in a word.

: NEW ( -- )  $100 ORG ;
: MSG" ( "text<quote>" -- f )  34 PARSE  ERRMSG @ ERRMSG CELL+ @ S= ;

: E1  NEW AX BL MOV, ;          T{ ' E1  CATCH MSG" the operands have different sizes" -> -2 TRUE }T
: E2  NEW 0 [BX] 0 [SI] MOV, ;  T{ ' E2  CATCH MSG" one operand must be a register" -> -2 TRUE }T
: E3  NEW CS AX MOV, ;          T{ ' E3  CATCH MSG" MOV to CS is not possible" -> -2 TRUE }T
: E4  NEW AX 2 SHL, ;           T{ ' E4  CATCH MSG" the 8086 shifts by 1 or by CL" -> -2 TRUE }T
: E5  NEW 5 AX MOV, ;           T{ ' E5  CATCH MSG" an operand is missing" -> -2 TRUE }T
: E6  NEW AX BYTE ;             T{ ' E6  CATCH MSG" BYTE needs a memory operand before it" -> -2 TRUE }T
: E7  NEW AL PUSH, ;            T{ ' E7  CATCH MSG" PUSH, takes 16 bits" -> -2 TRUE }T
: E8  NEW CS POP, ;             T{ ' E8  CATCH MSG" POP CS is not possible" -> -2 TRUE }T
: E9  NEW 0 [BX] BYTE PUSH, ;   T{ ' E9  CATCH MSG" PUSH, takes 16 bits" -> -2 TRUE }T
: E10 NEW AX 0 [BX] BYTE MOV, ; T{ ' E10 CATCH MSG" the operands have different sizes" -> -2 TRUE }T
: E11 NEW ES AL MOV, ;          T{ ' E11 CATCH MSG" MOV to a segment register takes 16 bits" -> -2 TRUE }T
: E12 NEW BX 5 IN, ;            T{ ' E12 CATCH MSG" IN, takes AL or AX" -> -2 TRUE }T
: E13 NEW AX 0 [BX] LEA, ;      \ correct: no error
T{ ' E13 CATCH -> 0 }T
: E14 NEW 0 [BX] AX LEA, ;      T{ ' E14 CATCH MSG" needs a word register and memory" -> -2 TRUE }T
: E15 NEW BEGIN, 130 NOPS Z UNTIL, ;
T{ ' E15 CATCH MSG" the jump is too far for 8 bits (127 bytes)" -> -2 TRUE }T
: E16 NEW Z IF, 130 NOPS THEN, ;
T{ ' E16 CATCH MSG" the jump is too far for 8 bits (127 bytes)" -> -2 TRUE }T
: E17 NEW $2100 TC@ ;             \ ORG $100: the code reaches up to $20FF
T{ ' E17 CATCH MSG" address outside the target memory" -> -2 TRUE }T

\ Labels: a jump too far, without place, placed twice, not a jump.
\ LABEL parses its name, so it runs in the line itself.
NEW L1 JZ, 130 NOPS
T{ ' LABEL CATCH L1 MSG" the jump is too far for 8 bits (127 bytes)" -> -2 TRUE }T
NEW FORWARD X5  L1 JMP,          \ a named label starts a new scope: L1 waits
T{ ' LABEL CATCH X5 MSG" label without place: L1" -> -2 TRUE }T
NEW LABEL L2
T{ ' LABEL CATCH L2 MSG" label placed twice: L2" -> -2 TRUE }T
NEW L3 [] AX MOV,
T{ ' LABEL CATCH L3 MSG" a label before its place is only for a jump, CALL, or MOVI, to a word register" -> -2 TRUE }T
NEW FORWARD X6  X6 JMP,
T{ ' CHECK-LABELS CATCH MSG" label without place: X6" -> -2 TRUE }T
TARGET CONSTANT SAVED-TARGET
: E18 0 TO TARGET  $100 TC@ ;     \ no target memory yet: as before the first ORG
T{ ' E18 CATCH MSG" no target memory: ORG first" -> -2 TRUE }T
SAVED-TARGET TO TARGET
NEW
: E19 NEW AX JMP-FAR-IND, ;     T{ ' E19 CATCH MSG" JMP-FAR-IND, takes memory" -> -2 TRUE }T
: E20 NEW BX CALL-FAR-IND, ;    T{ ' E20 CATCH MSG" CALL-FAR-IND, takes memory" -> -2 TRUE }T

\ ==== Part 4: code in place ====
\ IN-PLACE puts the code into a buffer of this system, as CODE does into
\ the dictionary of a PC (lib/pc/asm.fs). The counter is PLACE-P; the CPU
\ sees the first byte at $0100 (so the jumps get the same bytes as in
\ Part 1 and 2).

CREATE PLACE-BUF 64 ALLOT   VARIABLE PLACE-P
: IN-BUF ( -- )   PLACE-BUF PLACE-P !  $100 PLACE-BUF -  PLACE-P  PLACE-BUF 64 +  IN-PLACE ;
: BUF@ ( n -- c1 ... cn )   0 ?DO  PLACE-BUF I + C@  LOOP ;
T{ IN-BUF  THERE -> $100 }T
T{ IN-BUF  AX 3 ADDI,  RETF,  PLACE-P @ PLACE-BUF -  3 BUF@ -> 4 $83 $C0 $03 }T   \ add ax, 3 / retf
T{ IN-BUF  L1 JMP,  NOP,  LABEL L1  RET,  5 BUF@ -> $E9 $01 $00 $90 $C3 }T        \ jmp 0x104 / nop / ret
T{ IN-BUF  BEGIN,  AX DEC,  Z UNTIL,  3 BUF@ -> $48 $75 $FD }T                  \ dec ax / jnz 0x100
T{ IN-BUF  -4 [BP] JMP-FAR-IND,  3 BUF@ -> $FF $6E $FC }T                       \ ljmp word ptr [bp-4]
: E21 IN-BUF  64 0 DO NOP, LOOP  NOP, ;
T{ ' E21 CATCH MSG" address outside the target memory" -> -2 TRUE }T
\ In place a named label would be a new word in the middle of the code
IN-BUF
T{ ' LABEL CATCH X7 MSG" here only the labels L1 to L9" -> -2 TRUE }T
T{ ' FORWARD CATCH X8 MSG" here only the labels L1 to L9" -> -2 TRUE }T
NEW

S" 8086" .RESULTS
