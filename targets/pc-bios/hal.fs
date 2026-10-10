\ hal.fs - the HAL of uCForth for a PC with a BIOS, without an operating system
\
\ The PC starts uCForth from a disk: a floppy disk, a USB stick or a
\ hard disk (build/ucf-pc.img). The BIOS loads the boot sector
\ (bootsect.fs) to 0000:7C00; the boot sector loads the code (the kernel
\ and this HAL) to CODESEG:0000 and the memory of the VM to VMSEG:0000,
\ then jumps to the first byte of the code, the jump to START. Each PC
\ from the IBM PC (8088) on runs it, with 128 KB of RAM, and each newer
\ PC with a BIOS or with a CSM ("legacy boot"). Loaded by build.fs after
\ kernels/8086.fs; the constants VMSEG and VM-LBA come from build.fs.
\
\ The console has two sides at once:
\ - the keyboard and the screen, through the BIOS (INT 16h, INT 10h);
\ - the serial line COM1: 115200 baud, 8 data bits, no parity; the UART
\   (8250 or 16550) over its ports. An interrupt (IRQ 4) puts each byte
\   that comes into the buffer RX-BUF; the byte 3 (Ctrl-C) stops the
\   running word instead (see BREAK).
\ Each output goes to both. A key that comes from the keyboard shows on
\ both (ACCEPT echoes it: ?RX sets ECHO). A byte from COM1 shows only on
\ the screen: the terminal (ucf term) shows what it sends, as for a
\ board.
\
\ Time: the timer of the PC (8253/8254, channel 0) interrupts 1000 times
\ a second, and MILLIS counts these interrupts. Each 55 ms the BIOS gets
\ its old interrupt (18.2 times a second, as before): its clock and the
\ motor of the floppy disk drive go on as usual.
\
\ Break: Ctrl-C on the keyboard (the interrupt of the timer looks into
\ the buffer of the keys), Ctrl-Break (INT 1Bh) and the byte 3 from COM1
\ switch the table of the primitives to BREAK, as in targets/dos.
\
\ FREEZE: SAVE-IMAGE writes the 64 KB of the VM back to the disk, to the
\ sectors that the boot sector loads. So the PC starts the new image.
\
\ Calls of the BIOS run on a stack of their own: 0000:7C00 and down, the
\ stack of the boot sector. The data stack of the VM has only 512 bytes,
\ and a BIOS can need more. An interrupt of the hardware still uses the
\ stack that is there: the data stack of the VM, below its top cell.
\
\ Registers: as in kernels/8086.fs. CS is the segment of the code, and
\ the variables of the HAL are there: "CS: AX MS-COUNT [] MOV," reads
\ one. DS = ES = SS are the segment of the VM. The subroutines here
\ (CALL, RET) keep SI BP DS ES, unless the comment says other.

\ ==== Memory ============================================================
\ The code segment: the code from 0, at most 8 KB (16 sectors), then the
\ buffers. Its physical address is $8000; the VM starts at $10000.

$2000 CONSTANT OPTAB0        \ a copy of the table of the primitives, for BREAK
$2100 CONSTANT RX-BUF        \ the bytes from COM1: 256
$7C00 CONSTANT BIOS-SP       \ SP for the calls of the BIOS, in the segment 0
$FFF0 CONSTANT REGS          \ SYSCALL: AX BX CX DX SI DI flags ES, in the VM
1193 CONSTANT PIT-1MS        \ the timer counts 1193182 times a second: 1193 for 1 ms

\ The registers of the UART: the port of COM1 + n. With DLAB (bit 7 of
\ LCR), 0 and 1 hold the divisor of the speed.
0 CONSTANT RBR   0 CONSTANT THR   0 CONSTANT DLL   1 CONSTANT DLM
1 CONSTANT IER   2 CONSTANT IIR   2 CONSTANT FCR   3 CONSTANT LCR
4 CONSTANT MCR   5 CONSTANT LSR   6 CONSTANT MSR

\ ==== Variables =========================================================

LABEL DRIVE        0 T,         \ the disk of the start: 0 floppy disk A:, $80 the first hard disk
LABEL SPT          0 T,         \ its sectors per track
LABEL HEADS        0 T,         \ its heads
LABEL EXT          0 T,         \ 1: the BIOS writes it by the number of the sector (LBA)
LABEL ZERO         0 T,         \ the segment 0, for MOV SS
LABEL VM-SS        0 T,         \ SS and SP before a call of the BIOS
LABEL VM-SP        0 T,
LABEL OLD08        0 T, 0 T,    \ the vector of INT 08h of the BIOS: offset, segment
LABEL TIMER-ACC    0 T,         \ + 1193 each ms; a carry: a tick of the BIOS
LABEL MS-COUNT     0 T,         \ ms since the start: MILLIS
LABEL COM          0 T,         \ the port of COM1 (the BIOS says, usually $3F8), or 0
LABEL RX-HEAD      0 T,         \ the next byte from COM1 goes to RX-BUF + RX-HEAD
LABEL RX-TAIL      0 T,         \ ?RX takes the byte at RX-BUF + RX-TAIL
LABEL LAST-RX      0 T,         \ the byte from COM1 before, for the screen
LABEL BRK-FLAG     0 T,         \ not 0: a break came; BREAK runs next
LABEL ECHO-A       0 T,         \ build.fs: the address of ECHO
LABEL OUT-A        0 T,         \ build.fs: the address of OUT
LABEL D-OP         0 T,         \ DISK: 2 read, 3 write
LABEL D-LBA        0 T,         \ the first sector, from 0
LABEL D-LEFT       0 T,         \ the sectors to go
LABEL D-BUF        0 T,         \ the address in ES
LABEL D-N          0 T,         \ the sectors of this call
LABEL D-AX         0 T,         \ AX for this call: the function, the sectors
LABEL D-TRIES      0 T,
LABEL DAP          16 TDP +!    \ the block for the functions 42h and 43h: 16 bytes
LABEL SC-AX        0 T,         \ SYSCALL: AX, the flags and ES after the call
LABEL SC-FL        0 T,
LABEL SC-ES        0 T,

\ ==== Calls of the BIOS =================================================

: BIOS, ( n -- )   \ INT n on the stack of the BIOS calls; AX BX CX DX ES as the
                   \ call needs them. Keeps SI DI BP DS ES, and the flags of the call
  SI PUSH,  DI PUSH,  BP PUSH,  DS PUSH,  ES PUSH,
  CLI,  CS: VM-SS [] SS MOV,  CS: VM-SP [] SP MOV,  CS: SS ZERO [] MOV,  SP BIOS-SP MOVI,  STI,
  INT,
  CLI,  CS: SS VM-SS [] MOV,  CS: SP VM-SP [] MOV,  STI,
  ES POP,  DS POP,  BP POP,  DI POP,  SI POP, ;
LABEL INT10   $10 BIOS,  RET,   \ the screen
LABEL INT13   $13 BIOS,  RET,   \ the disks
LABEL INT16   $16 BIOS,  RET,   \ the keyboard

\ ==== Output ============================================================

: COM-PORT, ( reg -- )   \ DX = the port of the register reg of COM1
  CS: DX COM [] MOV,  ?DUP IF  DX SWAP ADDI,  THEN ;
: COM!, ( value reg -- )   \ the byte value to the register reg of COM1; uses AL DX
  COM-PORT,  AL SWAP MOVI,  DX AL OUT, ;
: COM@, ( reg -- )   \ the register reg of COM1 to AL; uses DX
  COM-PORT,  AL DX IN, ;

LABEL SCREEN     \ AL to the screen (INT 10h, 0Eh: as a typewriter); uses AX BX
  AH $0E MOVI,  BX 7 MOVI,  INT10 JMP,                    \ BH: page 0, BL: the color
LABEL SEND-COM   \ AL to COM1; uses AX CX DX
  CS: DX COM [] MOV,  DX DX OR,  Z IF,  RET,  THEN,
  AH AL MOV,  DX LSR ADDI,  CX CX XOR,
LABEL L1
  AL DX IN,  AL $20 TESTI,  L2 JNZ,  L1 LOOP,  RET,      \ the UART takes no byte: it is lost
LABEL L2
  DX LSR SUBI,  AL AH MOV,  DX AL OUT,  RET,
LABEL PUT-CHAR   \ AL to the screen and to COM1; uses AX BX CX DX
  AX PUSH,  SCREEN CALL,  AX POP,  SEND-COM JMP,
LABEL CRLF       \ a new line on both; uses AX BX CX DX
  AL 13 MOVI,  PUT-CHAR CALL,  AL 10 MOVI,  PUT-CHAR JMP,
LABEL MIRROR     \ AL, a byte from COM1, to the screen; CR or LF gives a new line,
                 \ an LF right after a CR nothing. Uses AX BX
  CS: BX LAST-RX [] MOV,  AH AH XOR,  CS: LAST-RX [] AX MOV,
  AL 10 CMPI,  Z IF,  BL 13 CMPI,  Z IF,  RET,  THEN,  AL 13 MOVI,  THEN,
  AL 13 CMPI,  NZ IF,  SCREEN JMP,  THEN,
  SCREEN CALL,  AL 10 MOVI,  SCREEN JMP,

\ ==== Break: Ctrl-C, Ctrl-Break (doc/vm.md, section 9) ==================
\ The handlers only change the table: each primitive is BREAK now. So
\ the next primitive runs BREAK, where the VM is complete, and NEXT
\ costs nothing more. BREAK puts the table back.

FORWARD BREAK
LABEL BREAK-ON   \ each primitive is BREAK now; keeps all registers
  AX PUSH,  CX PUSH,  DI PUSH,  ES PUSH,
  CS PUSH,  ES POP,  DI OPTAB MOVI,  AX BREAK MOVI,  CX 128 MOVI,  CLD,  REP, STOSW,
  CS: BRK-FLAG [] 1 MOVI,
  ES POP,  DI POP,  CX POP,  AX POP,  RET,
LABEL ON-1B      \ INT 1Bh: the BIOS saw Ctrl-Break
  BREAK-ON CALL,  IRET,
LABEL KEY-BREAK  \ Ctrl-C in the buffer of the keyboard (0040:001E, or as 0040:0080
                 \ says): empty the buffer, then the break. Keeps all registers
  AX PUSH,  BX PUSH,  CX PUSH,  DX PUSH,  ES PUSH,
  AX $40 MOVI,  ES AX MOV,
  ES: CX $80 [] MOV,  ES: DX $82 [] MOV,                  \ the start and the end of the buffer
  CX CX OR,  Z IF,  CX $1E MOVI,  DX $3E MOVI,  THEN,     \ an old BIOS: the first place
  ES: BX $1A [] MOV,                                      \ the first key that waits
  BEGIN,  ES: BX $1C [] CMP,  NZ WHILE,                   \ up to the place of the next key
    ES: 0 [BX] BYTE 3 CMPI,  Z IF,
      ES: AX $1C [] MOV,  ES: $1A [] AX MOV,  BREAK-ON CALL,  L1 JMP,
    THEN,
    BX 2 ADDI,  BX DX CMP,  AE IF,  BX CX MOV,  THEN,
  REPEAT,
LABEL L1
  ES POP,  DX POP,  CX POP,  BX POP,  AX POP,  RET,

LABEL BREAK      \ in the place of a primitive: stop the running word
  DS PUSH,  ES PUSH,  CS PUSH,  DS POP,  CS PUSH,  ES POP,
  SI OPTAB0 MOVI,  DI OPTAB MOVI,  CX 128 MOVI,  REP, MOVSW,   \ the table back
  ES POP,  DS POP,
  CS: BRK-FLAG [] 0 MOVI,
  CS: AX RX-HEAD [] MOV,  CS: RX-TAIL [] AX MOV,          \ the bytes from COM1 that wait go
  CS: BX OUT-A [] MOV,  0 [BX] 0 CMPI,  NZ IF,            \ a new line, if this one is not empty
    CRLF CALL,  CS: BX OUT-A [] MOV,  0 [BX] 0 MOVI,
  THEN,
  HANDLER [] 0 CMPI,  NZ IF,  AX -28 MOVI,  THROW-CODE JMP,  THEN,   \ a CATCH gets "user interrupt"
  SP S0 MOVI,  BP R0 MOVI,  STATE [] 0 MOVI,               \ else: empty stacks, then QUIT
  CS: BX XT-QUIT [] MOV,  DISPATCH JMP,

\ ==== Interrupts ========================================================

LABEL TIMER-ISR  \ INT 08h (IRQ 0), each ms: MILLIS; each 65536 counts of the timer
                 \ the old handler of the BIOS, and a look for Ctrl-C
  CS: MS-COUNT [] INC,
  CS: TIMER-ACC [] PIT-1MS ADDI,
  C IF,                                                   \ 55 ms: a tick for the BIOS
    KEY-BREAK CALL,
    CS: OLD08 2 + [] PUSH,  CS: OLD08 [] PUSH,  RETF,      \ on to its handler: it ends the interrupt
  THEN,
  AX PUSH,  AL $20 MOVI,  $20 AL OUT,  AX POP,  IRET,     \ the end of the interrupt for the PIC
LABEL COM-ISR    \ INT 0Ch (IRQ 4): the bytes that came at COM1 into RX-BUF; Ctrl-C: the break
  AX PUSH,  BX PUSH,  DX PUSH,
  BEGIN,  LSR COM@,  AL 1 TESTI,  NZ WHILE,
    RBR COM@,
    AL 3 CMPI,  Z IF,  BREAK-ON CALL,  ELSE,
      CS: BX RX-HEAD [] MOV,  CS: RX-BUF [BX] AL MOV,  BX INC,  BX $FF ANDI,
      CS: BX RX-TAIL [] CMP,  NZ IF,  CS: RX-HEAD [] BX MOV,  THEN,   \ a full buffer loses the byte
    THEN,
  REPEAT,
  AL $20 MOVI,  $20 AL OUT,
  DX POP,  BX POP,  AX POP,  IRET,

\ ==== The disk ==========================================================
\ Two ways, as in the boot sector. By the number of the sector (LBA),
\ when the BIOS has the extensions of INT 13h (EXT): functions 42h and
\ 43h with a block DAP, at most 64 sectors a call. Else by cylinder,
\ head and sector (CHS): the sector number n from 0 is the sector
\ n mod SPT + 1, the head (n / SPT) mod HEADS, the cylinder n / SPT /
\ HEADS; one call at most up to the end of a track (an old BIOS can do
\ no more).

LABEL DISK       \ D-OP (2 read, 3 write) for D-LEFT sectors from D-LBA, at ES:D-BUF.
                 \ CF 1: an error, after three tries. Uses AX BX CX DX
LABEL L1
  CS: D-LEFT [] 0 CMPI,  Z IF,  CLC,  RET,  THEN,
  CS: EXT [] 0 CMPI,  L4 JZ,
  CX 64 MOVI,  CS: CX D-LEFT [] CMP,  A IF,  CS: CX D-LEFT [] MOV,  THEN,   \ LBA
  CS: D-N [] CX MOV,
  CS: AH D-OP [] MOV,  AH $40 ORI,  AL AL XOR,  CS: D-AX [] AX MOV,   \ 42h or 43h; AL 0: no check
  L5 JMP,
LABEL L4                                                  \ CHS
  CS: AX D-LBA [] MOV,  DX DX XOR,  CS: SPT [] DIV,       \ AX: the track, DX: the sector in it
  CS: CX SPT [] MOV,  CX DX SUB,                          \ CX: the sectors up to the end of the track
  CS: CX D-LEFT [] CMP,  A IF,  CS: CX D-LEFT [] MOV,  THEN,
  CS: D-N [] CX MOV,  CS: CH D-OP [] MOV,  CS: D-AX [] CX MOV,   \ AH 2 or 3, AL the sectors
  DX INC,  CX DX MOV,                                     \ CL: the sector, from 1
  DX DX XOR,  CS: HEADS [] DIV,                           \ AX: the cylinder, DL: the head
  CH AL MOV,  AH 1 ROR,  AH 1 ROR,  CL AH OR,             \ bits 8 and 9 of the cylinder: bits 6 and 7 of CL
  DH DL MOV,
LABEL L5
  CS: DL DRIVE [] MOV,
  CS: D-TRIES [] 3 MOVI,
LABEL L2
  CS: DAP [] $10 MOVI,  CS: AX D-N [] MOV,  CS: DAP 2 + [] AX MOV,   \ the block: its size, the sectors,
  CS: AX D-BUF [] MOV,  CS: DAP 4 + [] AX MOV,  CS: DAP 6 + [] ES MOV,   \ the address,
  CS: AX D-LBA [] MOV,  CS: DAP 8 + [] AX MOV,           \ the sector; DAP + 10 to 15 stay 0
  CS: AX D-AX [] MOV,  CS: BX D-BUF [] MOV,
  SI PUSH,  DS PUSH,  CX PUSH,  DX PUSH,
  CS PUSH,  DS POP,  SI DAP MOVI,  INT13 CALL,            \ DS:SI: the block
  DX POP,  CX POP,  DS POP,  SI POP,
  L3 JNC,
  CS: D-TRIES [] DEC,  Z IF,  STC,  RET,  THEN,
  CX PUSH,  DX PUSH,  AX AX XOR,  INT13 CALL,  DX POP,  CX POP,   \ reset the disk, then again
  L2 JMP,
LABEL L3
  CS: AX D-N [] MOV,  CS: D-LBA [] AX ADD,  CS: D-LEFT [] AX SUB,
  CL 9 MOVI,  AX CL SHL,  CS: D-BUF [] AX ADD,            \ 512 bytes for each sector
  L1 JMP,

\ ==== The console: primitives 29-31 =====================================
\ ?RX takes a byte from COM1 first, then a key. A key gives ECHO = -1
\ (ACCEPT shows it on the screen and on COM1), a byte from COM1 gives
\ ECHO = 0 and shows on the screen here.

29 CODE ?RX
  CS: BX RX-TAIL [] MOV,  CS: BX RX-HEAD [] CMP,  NZ IF,  \ a byte from COM1
    AX AX XOR,  CS: AL RX-BUF [BX] MOV,  BX INC,  BX $FF ANDI,  CS: RX-TAIL [] BX MOV,
    CS: BX ECHO-A [] MOV,  0 [BX] 0 MOVI,                 \ the terminal shows it
    AX PUSH,  MIRROR CALL,  AX POP,                       \ and the screen
    L2 JMP,
  THEN,
  AH 1 MOVI,  INT16 CALL,  L1 JZ,                         \ no key
  AH AH XOR,  INT16 CALL,                                 \ the key: AL the character
  AL 3 CMPI,  Z IF,  BREAK-ON CALL,  L1 JMP,  THEN,       \ Ctrl-C: the break
  AL AL OR,  L1 JZ,                                       \ a key without a character (F1, arrows)
  CS: BX ECHO-A [] MOV,  0 [BX] -1 MOVI,                  \ ACCEPT shows it
  AH AH XOR,
LABEL L2
  AX PUSH,  AX -1 MOVI,  AX PUSH,  NEXT,
LABEL L1
  AX AX XOR,  AX PUSH,  NEXT,

30 CODE TX!       \ to the screen and to COM1; the byte 0 is not sent
  AX POP,  AL AL OR,  NZ IF,  PUT-CHAR CALL,  THEN,  NEXT,

31 CODE !IO       NEXT,

\ ==== HAL 80-88, 99 =====================================================

80 CODE MILLIS    CS: MS-COUNT [] PUSH,  NEXT,
81 CODE MS
LABEL WAIT-MS     \ ( u -- ): from the next tick of the timer u ticks more: at least u ms
  DI POP,  DI DI OR,  Z IF,  NEXT,  THEN,
  CS: BX MS-COUNT [] MOV,
  BEGIN,                                                   \ the next tick: the start
    CS: BRK-FLAG [] 0 CMPI,  NZ IF,  NEXT,  THEN,
    HLT,                                                   \ until an interrupt comes
    CS: BX MS-COUNT [] CMP,
  NZ UNTIL,
  CS: BX MS-COUNT [] MOV,
  BEGIN,
    CS: BRK-FLAG [] 0 CMPI,  NZ IF,  NEXT,  THEN,          \ a break ends the wait
    CS: AX MS-COUNT [] MOV,  AX BX SUB,  AX DI CMP,  AE IF,  NEXT,  THEN,
    HLT,
  AGAIN,
82 CODE US        \ ( u -- ): (u + 999) / 1000 ms
  AX POP,  DX DX XOR,  AX 999 ADDI,  DX 0 ADCI,  CX 1000 MOVI,  CX DIV,  AX PUSH,  WAIT-MS JMP,
83 CODE BIS!      BX POP,  AX POP,  0 [BX] AL OR,  NEXT,
84 CODE BIC!      BX POP,  AX POP,  AL NOT,  0 [BX] AL AND,  NEXT,
85 CODE SAVE-IMAGE   \ ( -- ior ): the 64 KB of the VM to the disk, from the sector VM-LBA
  CS: D-OP [] 3 MOVI,  CS: D-LBA [] VM-LBA MOVI,  CS: D-LEFT [] 128 MOVI,  CS: D-BUF [] 0 MOVI,
  DISK CALL,
  AX 0 MOVI,  C IF,  AX -37 MOVI,  THEN,  AX PUSH,  NEXT,   \ MOV keeps the carry
86 CODE ERASE-IMAGE   NEXT,
87 CODE REBOOT    \ ( f -- ): empty stacks, then BOOT (f true) or QUIT
  AX POP,  SP S0 MOVI,  BP R0 MOVI,  STATE [] 0 MOVI,  HANDLER [] 0 MOVI,
  AX AX OR,  Z IF,  CS: BX XT-QUIT [] MOV,  ELSE,  CS: BX XT-BOOT [] MOV,  THEN,
  DISPATCH JMP,
88 CODE BYE
LABEL RESTART     \ the PC starts again; COM1 sends its last bytes first
  CS: DX COM [] MOV,  DX DX OR,  L2 JZ,
  DX LSR ADDI,  CX CX XOR,
LABEL L1
  AL DX IN,  AL $40 TESTI,  L2 JNZ,  L1 LOOP,             \ LSR bit 6: all is sent
LABEL L2
  CLI,  AL $FE MOVI,  $64 AL OUT,                         \ an AT: the keyboard controller resets the CPU
  AX $40 MOVI,  DS AX MOV,  $72 [] $1234 MOVI,            \ else (an XT): the BIOS from its start, warm
  AX $FFFF MOVI,  AX PUSH,  AX AX XOR,  AX PUSH,  RETF,   \ JMP FAR FFFF:0000
99 CODE (BYE)     AX POP,  RESTART JMP,

\ ==== HAL of the PC: 100, 110-113 ========================================
\ SYSCALL ( x1 ... xn n id -- x ) as in targets/dos: x1 to xn go to AX
\ BX CX DX SI DI (n at most 6), then INT id on the stack of the BIOS
\ calls; x is AX after it. DS and ES are the segment of the VM, so an
\ address in a register is a VM address. After the call the registers
\ are at REGS ($FFF0): AX BX CX DX SI DI, the flags (bit 0 is the
\ carry), ES.

LABEL SC-INT       $CD TC, $10 TC,  RET,     \ INT id: SYSCALL writes id into the second byte
100 CODE SYSCALL
  AX POP,  CS: SC-INT 1+ [] AL MOV,
  CX POP,  CX 6 CMPI,  A IF,  AX -24 MOVI,  THROW-CODE JMP,  THEN,
  DI REGS MOVI,  AX AX XOR,  BX CX MOV,  CX 8 MOVI,  REP, STOSW,
  BX 1 SHL,  BEGIN,  BX BX OR,  NZ WHILE,  BX 2 SUBI,  REGS [BX] POP,  REPEAT,
  SI PUSH,  BP PUSH,
  CLI,  CS: VM-SS [] SS MOV,  CS: VM-SP [] SP MOV,  CS: SS ZERO [] MOV,  SP BIOS-SP MOVI,  STI,
  AX REGS [] MOV,  BX REGS 2 + [] MOV,  CX REGS 4 + [] MOV,  DX REGS 6 + [] MOV,
  SI REGS 8 + [] MOV,  DI REGS 10 + [] MOV,
  SC-INT CALL,
  CS: SC-AX [] AX MOV,  PUSHF,  AX POP,  CS: SC-FL [] AX MOV,
  AX ES MOV,  CS: SC-ES [] AX MOV,
  CLI,  CS: SS VM-SS [] MOV,  CS: SP VM-SP [] MOV,  STI,
  AX VMSEG MOVI,  DS AX MOV,  ES AX MOV,
  CS: AX SC-AX [] MOV,  REGS [] AX MOV,  REGS 2 + [] BX MOV,  REGS 4 + [] CX MOV,
  REGS 6 + [] DX MOV,  REGS 8 + [] SI MOV,  REGS 10 + [] DI MOV,
  CS: AX SC-FL [] MOV,  REGS 12 + [] AX MOV,  CS: AX SC-ES [] MOV,  REGS 14 + [] AX MOV,
  BP POP,  SI POP,  CS: AX SC-AX [] MOV,  AX PUSH,  NEXT,

110 CODE PC@      DX POP,  AL DX IN,  AH AH XOR,  AX PUSH,  NEXT,   \ ( port -- c )
111 CODE PC!      DX POP,  AX POP,  DX AL OUT,  NEXT,               \ ( c port -- )
112 CODE PW@      DX POP,  AX DX IN,  AX PUSH,  NEXT,               \ ( port -- x )
113 CODE PW!      DX POP,  AX POP,  DX AX OUT,  NEXT,               \ ( x port -- )

\ ==== The start =========================================================

LABEL COM-INIT   \ COM1: 115200 baud, 8 data bits, no parity, 1 stop bit; an
                 \ interrupt for each byte that comes. Uses AX DX ES
  0 IER COM!,                                             \ no interrupts while it changes
  $80 LCR COM!,  1 DLL COM!,  0 DLM COM!,                 \ the divisor: 115200 / 1 baud
  3 LCR COM!,                                             \ 8 data bits, no parity, 1 stop bit
  $C7 FCR COM!,                                           \ the FIFOs on and empty (16550; an 8250 has none)
  $0B MCR COM!,                                           \ DTR, RTS, OUT2: OUT2 lets the interrupt go to the PIC
  LSR COM@,  RBR COM@,  IIR COM@,  MSR COM@,              \ nothing waits
  AX AX XOR,  ES AX MOV,  ES: $30 [] COM-ISR MOVI,  ES: $32 [] CS MOV,   \ INT 0Ch: IRQ 4
  AL $21 IN,  AL $EF ANDI,  $21 AL OUT,                   \ the PIC lets IRQ 4 through
  1 IER COM!,                                             \ an interrupt when a byte came
  RET,

LABEL START      \ the boot sector jumps here: CS = the code; DL the disk, CX its
                 \ sectors per track, BX its heads, AX 1 for LBA. The stack is the one
                 \ of the boot sector
  CLI,  CLD,
  DH DH XOR,  CS: DRIVE [] DX MOV,  CS: SPT [] CX MOV,  CS: HEADS [] BX MOV,  CS: EXT [] AX MOV,
  AX CS MOV,  DS AX MOV,  ES AX MOV,
  SI OPTAB MOVI,  DI OPTAB0 MOVI,  CX 128 MOVI,  REP, MOVSW,   \ the copy for BREAK
  AX AX XOR,  ES AX MOV,                                  \ the vectors: segment 0
  ES: AX $20 [] MOV,  CS: OLD08 [] AX MOV,  ES: AX $22 [] MOV,  CS: OLD08 2 + [] AX MOV,
  ES: $20 [] TIMER-ISR MOVI,  ES: $22 [] CS MOV,          \ INT 08h: the timer
  ES: $6C [] ON-1B MOVI,  ES: $6E [] CS MOV,              \ INT 1Bh: Ctrl-Break
  AL $34 MOVI,  $43 AL OUT,                               \ the timer, channel 0: mode 2, 16 bits
  AL PIT-1MS 255 AND MOVI,  $40 AL OUT,  AL PIT-1MS 8 RSHIFT MOVI,  $40 AL OUT,   \ each ms
  AX $40 MOVI,  ES AX MOV,  ES: AX 0 [] MOV,  CS: COM [] AX MOV,   \ the port of COM1, from the BIOS
  AX AX OR,  NZ IF,  COM-INIT CALL,  THEN,
  STI,
  AH $0F MOVI,  $10 INT,  AL $7F ANDI,  AH AH XOR,  $10 INT,   \ the screen: its mode again, empty
  AX VMSEG MOVI,  DS AX MOV,  ES AX MOV,                  \ from now on: the VM
  CLI,  SS AX MOV,  SP S0 MOVI,  STI,
  BP R0 MOVI,  STATE [] 0 MOVI,  HANDLER [] 0 MOVI,
  CS: BX OUT-A [] MOV,  0 [BX] 0 MOVI,
  CS: BX XT-BOOT [] MOV,  DISPATCH JMP,
