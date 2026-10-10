\ hal.fs - the HAL of uCForth for the Raspberry Pi 4 and 400, bare metal
\
\ The start code, the console (primitives 29-31) over the UART PL011 on
\ GPIO 14 (TXD) and 15 (RXD), the time (80-82) from the system timer,
\ and BYE as a restart through the watchdog.
\
\ Loaded by build.fs after kernels/aarch64.fs.
\
\ Memory: the firmware loads kernel8.img at address 0 (config.txt:
\ kernel_old=1). VM addresses are the addresses of the chip: MEM (X28)
\ is 0. The image lies at $100 to $3FFFF, as the memory map says, and
\ the code follows at CODE-ADDR. So a program reaches the registers of
\ the chip with @ and ! (the peripherals start at $FE000000).
\
\ All four cores start at address 0. Cores 1 to 3 wait there (WFE).
\
\ An exception (for example @ at an address that is not aligned: the
\ MMU is off) runs FAULT: -9 THROW, "invalid memory address".

$FE000000 CONSTANT PERI
PERI $003000 + CONSTANT SYSTIMER   \ 1 MHz: CLO at 4, CHI at 8
PERI $100000 + CONSTANT PM         \ power management: the watchdog
PERI $200000 + CONSTANT GPIO
PERI $201000 + CONSTANT UART0      \ PL011

$00 CONSTANT U-DR    $18 CONSTANT U-FR    $24 CONSTANT U-IBRD   $28 CONSTANT U-FBRD
$2C CONSTANT U-LCRH  $30 CONSTANT U-CR    $44 CONSTANT U-ICR
$10 CONSTANT FR-RXFE   $20 CONSTANT FR-TXFF   \ receive FIFO empty, send FIFO full

\ ==== Start =============================================================

LABEL ECHO-ADDR  0 A,   \ build.fs: the address of ECHO

LABEL UART-INIT   \ 115200 baud, 8 bits, no parity, 1 stop bit; clock 48 MHz
  X9 UART0 MOVI,  WZR X9 U-CR STR-I,                    \ off while it changes
  X10 GPIO MOVI,
  W11 X10 4 LDR-I,                                      \ GPFSEL1: GPIO 14 and 15 ALT0
  W12 $3F000 MOVI,  W11 W11 W12 BIC,  W12 $24000 MOVI,  W11 W11 W12 ORR,
  W11 X10 4 STR-I,
  W11 X10 12 LDR-I,                                     \ GPFSEL3: GPIO 32 and 33 input
  W12 $FC0 MOVI,  W11 W11 W12 BIC,  W11 X10 12 STR-I,   \ (they go to Bluetooth)
  W11 X10 $E4 LDR-I,                                    \ no pull-up, no pull-down on 14 and 15
  W12 $F0000000 MOVI,  W11 W11 W12 BIC,  W11 X10 $E4 STR-I,
  W11 $7FF MOVI,  W11 X9 U-ICR STR-I,
  W11 26 MOVI,  W11 X9 U-IBRD STR-I,                    \ 48000000 / (16 * 115200) = 26.04
  W11 3 MOVI,  W11 X9 U-FBRD STR-I,                     \ 0.04 * 64 = 3
  W11 $70 MOVI,  W11 X9 U-LCRH STR-I,                   \ 8 bits, FIFOs on
  W11 $301 MOVI,  W11 X9 U-CR STR-I,                    \ on: UART, send, receive
  RET,

\ ==== Exceptions ========================================================
\ The table of the exception vectors: 16 entries of 128 bytes, aligned
\ on 2 KB. Each one jumps to FAULT. FAULT does not return: it stays in
\ the exception level of the start and runs the VM on (the interrupts
\ stay masked).

FORWARD FAULT
: ALIGN2K ( -- )   BEGIN THERE $7FF AND WHILE 0 A, REPEAT ;
: VECTOR ( -- )   FAULT B,  31 0 DO 0 A, LOOP ;
: VECTORS, ( -- )   16 0 DO VECTOR LOOP ;
ALIGN2K  LABEL VECTORS  VECTORS,

LABEL FAULT   \ with a CATCH: -9 THROW; else QUIT
  W10 HANDLER MOVI,  W1 MEM W10 LDR-UXTW,
  W1 0=IF,
    DSP S0 MOVI,  RSP R0 MOVI,  W10 STATE MOVI,  WZR MEM W10 STR-UXTW,
    WP XT-QUIT LDR-LIT,  DISPATCH B,
  THEN,
  W0 -9 MOVI,  THROW-CODE B,

LABEL SET-VECTORS   \ VBAR of the exception level of the start
  X0 CURRENTEL MRS,  X0 X0 2 LSRI,  X1 VECTORS ADR,
  X0 3 CMPI,  EQ IF,  VBAR-EL3 X1 MSR,  THEN,
  X0 2 CMPI,  EQ IF,  VBAR-EL2 X1 MSR,  THEN,
  X0 1 CMPI,  EQ IF,  VBAR-EL1 X1 MSR,  THEN,
  ISB,  RET,

LABEL START   \ the entry of the program, from address 0
  X0 MPIDR-EL1 MRS,  X1 3 MOVI,  X0 X0 X1 AND,
  X0 0<>IF,  BEGIN,  WFE,  AGAIN,  THEN,               \ cores 1-3 wait
  MEM XZR MOV,  TAB OPTAB ADR,
  DSP S0 MOVI,  RSP R0 MOVI,                           \ doc/vm.md, section 10
  W10 STATE MOVI,  WZR MEM W10 STR-UXTW,  W10 HANDLER MOVI,  WZR MEM W10 STR-UXTW,
  W10 ECHO-ADDR LDR-LIT,  W11 -1 MOVI,  W11 MEM W10 STR-UXTW,   \ the terminal does not echo
  SET-VECTORS BL,  UART-INIT BL,
  WP XT-BOOT LDR-LIT,  DISPATCH B,

\ ==== The console =======================================================
\ The terminal on the other side sends each key at once and does not
\ show it: ACCEPT echoes (ECHO is on). No break with Ctrl-C yet: the
\ running word sees the byte 3 only when it reads keys.

29 PRIM ( ?RX )
  X9 UART0 MOVI,  W0 X9 U-FR LDR-I,  W1 FR-RXFE MOVI,  W0 W0 W1 AND,
  W0 0=IF,
    W0 X9 U-DR LDR-I,  W1 255 MOVI,  W0 W0 W1 AND,  W0 PUSH,  W0 -1 MOVI,  W0 PUSH,  NEXT,
  THEN,
  WZR PUSH,  NEXT,

30 PRIM ( TX! )
  W0 POP,  X9 UART0 MOVI,
  BEGIN,  W1 X9 U-FR LDR-I,  W2 FR-TXFF MOVI,  W1 W1 W2 AND,  W1 0 CMPI,  EQ UNTIL,
  W0 X9 U-DR STR-I,  NEXT,

31 PRIM ( !IO )   NEXT,

\ ==== Time: HAL 80-82 ===================================================

LABEL WAIT-US   \ wait W0 microseconds
  X9 SYSTIMER MOVI,  W2 X9 4 LDR-I,
  BEGIN,  W3 X9 4 LDR-I,  W3 W3 W2 SUB,  W3 W0 CMP,  HS UNTIL,
  NEXT,

80 PRIM ( MILLIS )   \ the system timer (64 bits) / 1000
  X9 SYSTIMER MOVI,  W1 X9 8 LDR-I,  W0 X9 4 LDR-I,
  X1 X1 32 LSLI,  X0 X0 X1 ORR,  X2 1000 MOVI,  X0 X0 X2 UDIV,
  W0 PUSH,  NEXT,
81 PRIM ( MS )   W0 POP,  W1 1000 MOVI,  W0 W0 W1 MUL,  WAIT-US B,
82 PRIM ( US )   W0 POP,  WAIT-US B,

\ ==== HAL 83-88 =========================================================

83 PRIM ( BIS! )   W1 POP,  W0 POP,  W2 MEM W1 LDRB-UXTW,  W2 W2 W0 ORR,  W2 MEM W1 STRB-UXTW,  NEXT,
84 PRIM ( BIC! )   W1 POP,  W0 POP,  W2 MEM W1 LDRB-UXTW,  W2 W2 W0 BIC,  W2 MEM W1 STRB-UXTW,  NEXT,
85 PRIM ( SAVE-IMAGE )   W0 -21 MOVI,  W0 PUSH,  NEXT,   \ not yet
86 PRIM ( ERASE-IMAGE )  NEXT,
87 PRIM ( REBOOT )   \ ( f -- ): empty stacks, then BOOT (f true) or QUIT
  W0 POP,  DSP S0 MOVI,  RSP R0 MOVI,
  W10 STATE MOVI,  WZR MEM W10 STR-UXTW,  W10 HANDLER MOVI,  WZR MEM W10 STR-UXTW,
  W0 0=IF,  WP XT-QUIT LDR-LIT,  ELSE,  WP XT-BOOT LDR-LIT,  THEN,  DISPATCH B,

LABEL RESTART   \ the watchdog restarts the Pi after 10 ticks (about 150 us)
  X9 PM MOVI,  W10 $5A000000 MOVI,                      \ the password
  W11 10 MOVI,  W11 W11 W10 ORR,  W11 X9 $24 STR-I,      \ PM_WDOG
  W11 X9 $1C LDR-I,  W12 $30 MOVI,  W11 W11 W12 BIC,     \ PM_RSTC: full reset
  W12 $20 MOVI,  W11 W11 W12 ORR,  W11 W11 W10 ORR,  W11 X9 $1C STR-I,
  BEGIN,  WFE,  AGAIN,

88 PRIM ( BYE )     RESTART B,
99 PRIM ( BYE-N )   W0 POP,  RESTART B,   \ a board has no exit status
