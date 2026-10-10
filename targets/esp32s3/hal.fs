\ hal.fs - the HAL of uCForth for the ESP32-S3
\
\ The start code, the console (primitives 29-31) over UART0, the time
\ (80-82) from the SYSTIMER, and BYE as a restart of the chip. Loaded by
\ build.fs after kernels/xtensa.fs. HAL words of this target: SYSCALL
\ (100) calls a function of the ROM, >CHIP (101) gives a chip address
\ (package ROM, lib/esp32/rom.fs); +INT (102) and -INT (103) switch the
\ handlers of interrupts on and off, CODE-INT! (104) makes a CODE word
\ the handler of a CPU interrupt (package INTR, lib/esp32/intr.fs).
\ SAVE-IMAGE (85) runs the word (SAVE-IMAGE) of the image, if the image
\ has one (lib/esp32/esp32s3/bootimg.fs).
\
\ The console: UART0 at the pins of the "USB to serial port" socket of
\ the board. The ROM has set it up: 115200 baud, 8 data bits, no parity.
\ The terminal (ucf term) shows what it sends: the board does not echo.
\ An interrupt puts each byte that comes into the buffer RX (256 bytes);
\ the byte 3 (Ctrl-C) stops the running word instead (see BREAK). Ctrl-C
\ waits while a Forth handler runs, and while the variable NO-BREAK of
\ the package IDLE is not 0 (lib/idle.fs: a word of the network, of the
\ chain of IDLE). Ctrl-C twice within 1 s also switches the handlers
\ off; Ctrl-C while another one waits stops at once (a word that hangs).
\
\ Exceptions and interrupts go to the vectors of this HAL (VECBASE, at
\ the end of the code). An exception of the CPU (an address without
\ memory, an unknown instruction) does not stop the board: the VM goes
\ on with THROW (see FAULT). The interrupts:
\   level 1: CPU interrupt 1, the sources for Forth handlers (LEVEL1)
\   level 2: CPU interrupts 19, 20, 21, a CODE word each (LEVEL2)
\   level 3: CPU interrupt 23, UART0, the console (LEVEL3)
\ A handler of level 1 to 3 runs with PS.EXCM set: no other interrupt
\ of these levels comes meanwhile, and after its end the highest level
\ that waits comes first. So Ctrl-C (level 3) reaches the board also
\ when a handler of level 2 comes again and again.
\
\ The registers are in the Technical Reference Manual of the ESP32-S3,
\ the exceptions and the register window in the Xtensa Instruction Set
\ Architecture Reference Manual (chapter 4.4 and 4.7).

\ ==== Constants for L32R ================================================

TALIGN
LABEL UART0      $60000000 T,       \ FIFO at 0, STATUS at $1C
LABEL SYSTIMER   $60023000 T,
LABEL RTC-CNTL   $60008000 T,
LABEL TIMG0      $6001F000 T,
LABEL TIMG1      $60020000 T,
LABEL UART0-MAP  $600C206C T,       \ the interrupt matrix: the CPU interrupt of UART0 (source 27)
LABEL CODE-INTS  $00380000 T,       \ CPU interrupts 19, 20, 21: level 2, the CODE handlers
LABEL WDT-KEY    $50D83AA1 T,
LABEL SWD-KEY    $8F1D312A T,
LABEL BIT31      $80000000 T,
LABEL BIT30      $40000000 T,
LABEL BIT23      $00800000 T,       \ also: CPU interrupt 23, level 3
LABEL WAIT-START 12000000 T,        \ CPU cycles: 0.3 s at 40 MHz
LABEL TICKS/MS   16000 T,           \ SYSTIMER: 16 ticks per us
LABEL XT-SAVE    0 T,               \ build.fs: the xt of (SAVE-IMAGE); 0: none
LABEL CODE-LEN   0 T,               \ build.fs: the length of the code (a multiple of 4)
LABEL ENTRY-A    0 T,               \ build.fs: the address of START
LABEL XT-ISR     0 T,               \ build.fs: the xt of (ISR) of the package INTR; 0: none
LABEL XT-FAULT   0 T,               \ build.fs: the xt of (FAULT) of the package INTR; 0: none
LABEL FREGS-A    0 T,               \ build.fs: the chip address of FAULT-REGS of the package INTR; 0: none
LABEL OUT-V      0 T,               \ build.fs: the address of OUT
LABEL HOLD-V     0 T,               \ build.fs: the address of NO-BREAK of the package IDLE; 0: none
LABEL HALT-V     WL-FORTH 16 - T,   \ HALT-XT: the code field 127, then a thread cell (doc/vm.md 3)
LABEL ROM-SP     $3FC8FFC0 T,       \ a1: the stack for the functions of the ROM (see START)
LABEL PS-VM      $00040020 T,       \ PS: register window, user mode, interrupts on
LABEL PS-EXC     $00040030 T,       \ the same, in an exception (EXCM)
LABEL VARS-A     0 T,               \ the variables of the HAL on the data bus (set at the end)
LABEL BRKTAB-A   0 T,               \ the break table on the data bus (set at the end)
LABEL VECTORS-A  0 T,               \ VECBASE (set at the end)
LABEL RECOVER-A  0 T,               \ (set at the end)
LABEL WAIT-XT    0 T,               \ the cell V-WAIT on the data bus: an xt for primitive 126 (set at the end)
LABEL OFF-TEXT-A 0 T,               \ the text after Ctrl-C twice, on the data bus (set at the end)

\ The variables of the HAL: VARS, at the end of the code. The bytes 0 to
\ 255 are the buffer RX; the offsets of the other cells:
256 CONSTANT V-HEAD      \ the next byte from UART0 goes to RX + V-HEAD
260 CONSTANT V-TAIL      \ the next byte for ?RX is at RX + V-TAIL
264 CONSTANT V-CTRLC     \ a Ctrl-C that waits: 1; 2 twice within 1 s; 3 while one waited
268 CONSTANT V-INTREQ    \ not 0: CPU interrupt 1 came, the Forth handlers wait
272 CONSTANT V-INTON     \ not 0: +INT, the Forth handlers are on
276 CONSTANT V-INROM     \ not 0: SYSCALL runs a function of the ROM
280 CONSTANT V-SP        \ SP and RP of the VM, for an exception in the ROM
284 CONSTANT V-RP
288 CONSTANT V-CAUSE     \ the last exception: its cause, PC and address
292 CONSTANT V-PC
296 CONSTANT V-ADDR
300 CONSTANT V-SAVE      \ 9 cells: the registers of the handlers of level 1 to 3, SAR
336 CONSTANT V-INTMASK   \ the CPU interrupts that +INT switches on: 1, and those of V-CODE
340 CONSTANT V-CODE      \ 3 cells: the machine code for CPU interrupts 19, 20, 21, or 0
352 CONSTANT V-WAIT      \ 126: a code field; MS and US wait on with it after a break
356 CONSTANT V-INISR     \ not 0: a Forth handler runs ((ISR), until HALT)
360 CONSTANT V-LAST      \ the time of the last Ctrl-C, in steps of 2^20 ticks of the SYSTIMER (65.5 ms)
364 CONSTANT #VARS

FORWARD BREAK   FORWARD DEADLINE   FORWARD WAIT-UNTIL

\ ==== Subroutines: CALL0, the return address in A0 ======================

TALIGN LABEL TX   \ send the character in a2; uses a3 to a5
  A3 UART0 L32R,
LABEL L1
  A4 A3 $1C L32I,                   \ UART_STATUS
  A4 A4 16 10 EXTUI,                \ bytes in the transmit FIFO
  A5 120 MOVI,
  A4 A5 L1 BGEU,                    \ nearly full: wait
  MEMW,
  A2 A3 0 S32I,                     \ into the FIFO
  RET,

TALIGN LABEL TICKS   \ a2: SYSTIMER bits 31-0, a3: bits 51-32; uses a4 a5
  A5 3 RSIL,                        \ no interrupt meanwhile: DRAIN takes a value too
  A4 SYSTIMER L32R,
  A2 BIT30 L32R,
  A2 A4 4 S32I,                     \ SYSTIMER_UNIT0_OP: take the value
  MEMW,
LABEL L1
  A2 A4 4 L32I,
  A2 29 L1 BBCI,                    \ wait for VALUE_VALID
  A3 A4 $40 L32I,                   \ SYSTIMER_UNIT0_VALUE_HI
  A2 A4 $44 L32I,                   \ SYSTIMER_UNIT0_VALUE_LO
  A5 PS WSR,  RSYNC,
  RET,

TALIGN LABEL DRAIN   \ the bytes that wait in UART0 into RX; a2: VARS; uses a4 to a8
  A4 UART0 L32R,
  A5 $101 MOVI,  MEMW,  A5 A4 $10 S32I,     \ UART_INT_CLR: RXFIFO_FULL, RXFIFO_TOUT
LABEL L1
  A5 A4 $1C L32I,
  A5 A5 0 10 EXTUI,                 \ bytes in the receive FIFO
  A5 L9 BEQZ,
  A5 A4 0 L32I,  A5 A5 0 8 EXTUI,
  A5 3 L2 BNEI,
  \ Ctrl-C: the break, not a byte in RX. V-CTRLC: 1; 2 if the last Ctrl-C
  \ came less than 1 s before; 3 if a Ctrl-C still waits
  A6 SYSTIMER L32R,  A7 BIT30 L32R,  A7 A6 4 S32I,  MEMW,
LABEL L3
  A7 A6 4 L32I,  A7 29 L3 BBCI,     \ the SYSTIMER as in TICKS
  A7 A6 $40 L32I,  A8 A6 $44 L32I,
  A7 A7 12 SLLI,  A8 A8 20 12 EXTUI,  A7 A7 A8 OR,   \ bits 51-20: steps of 65.5 ms
  A8 A2 V-LAST L32I,  A7 A2 V-LAST S32I,
  A7 A7 A8 SUB,                     \ the steps since the last Ctrl-C
  A5 1 MOVI.N,
  A8 16 MOVI,  A7 A8 L4 BGEU,       \ 16 steps: 1.05 s
  A5 2 MOVI.N,
LABEL L4
  A7 A2 V-CTRLC L32I,
  A7 L5 BEQZ,
  A5 3 MOVI.N,
LABEL L5
  A5 A2 V-CTRLC S32I,
  L1 J,
LABEL L2
  A6 A2 V-HEAD L32I,
  A7 A6 1 ADDI.N,  A7 A7 0 8 EXTUI,
  A8 A2 V-TAIL L32I,
  A7 A8 L1 BEQ,                     \ RX is full: the byte is lost
  A8 A2 A6 ADD.N,  A5 A8 0 S8I,
  A7 A2 V-HEAD S32I,
  L1 J,
LABEL L9
  RET,

TALIGN LABEL INTEN   \ INTENABLE: the bits a2 on, the bits a3 off; uses a4 a5
  A4 3 RSIL,                        \ no interrupt of level 1 to 3 meanwhile
  A5 INTENABLE RSR,
  A5 A5 A3 OR,  A5 A5 A3 XOR,  A5 A5 A2 OR,
  A5 INTENABLE WSR,
  A4 PS WSR,  RSYNC,
  RET,

TALIGN LABEL HANDLERS-OFF   \ as -INT; also no handler runs any more; a2: VARS; uses a2 to a5
  A3 0 MOVI.N,  A3 A2 V-INTON S32I,  A3 A2 V-INISR S32I,
  A3 A2 V-INTMASK L32I,  A2 0 MOVI.N,
  INTEN J,

TALIGN LABEL REQUEST   \ a break waits: TBL = BRKTAB, but not while the ROM runs; a2: VARS; uses a4 a5
  A4 A2 V-CTRLC L32I,  A5 A2 V-INTREQ L32I,  A4 A4 A5 OR,
  A4 L1 BEQZ,
  A4 A2 V-INROM L32I,
  A4 L1 BNEZ,
  TBL BRKTAB-A L32R,
LABEL L1
  RET,

\ ==== The console =======================================================

29 CODE ?RX   ( -- c -1 | 0 )       \ from RX; the bytes in the FIFO go to RX first
  A9 3 RSIL,                        \ no interrupt of UART0 meanwhile: the bytes stay in order
  A2 VARS-A L32R,
  DRAIN CALL0,
  A3 A2 V-CTRLC L32I,
  A3 L1 BEQZ,
  TBL BRKTAB-A L32R,                \ Ctrl-C came: the break at the next NEXT
LABEL L1
  A3 A2 V-TAIL L32I,  A4 A2 V-HEAD L32I,
  A9 PS WSR,  RSYNC,
  A3 A4 L2 BEQ,
  A4 A2 A3 ADD.N,  A4 A4 0 L8UI,
  A3 A3 1 ADDI.N,  A3 A3 0 8 EXTUI,  A3 A2 V-TAIL S32I,
  A4 PUSH,  A4 -1 MOVI.N,  A4 PUSH,  NEXT,
LABEL L2                            \ no byte
  A4 0 MOVI.N,  A4 PUSH,  NEXT,
30 CODE TX!
  A2 POP,  TX CALL0,  NEXT,
31 CODE !IO                         \ the ROM has set up UART0; START adds the interrupt
  NEXT,

\ ==== HAL 80-88 =========================================================

80 CODE MILLIS   \ the SYSTIMER / 16000; runs over after 2^32 ms
  TICKS CALL0,
  A4 TICKS/MS L32R,
  A3 A3 A4 REMU,                    \ the high part mod 16000 keeps the low 32 bits right
  UDIV CALL0,
  A2 PUSH,  NEXT,
81 CODE MS   ( n -- )               \ until the SYSTIMER is n ms further (DEADLINE)
  A9 POP,
  A9 1 L1 BLTI,
  A4 TICKS/MS L32R,  A6 A9 A4 MULL,  A7 A9 A4 MULUH,
  DEADLINE J,
LABEL L1
  NEXT,
82 CODE US   ( n -- )               \ until the SYSTIMER is n us further
  A9 POP,
  A9 1 L1 BLTI,
  A6 A9 4 SLLI,  A7 A9 28 4 EXTUI,  \ 16 ticks per us
  DEADLINE J,
LABEL L1
  NEXT,
83 CODE BIS!   ( mask a -- )        \ a cell: the registers of the chip have 32 bits
  A2 SP 0 L32I.N,  A3 SP 4 L32I.N,  SP SP 8 ADDI.N,  A2 >CHIP,
  A4 A2 0 L32I,  A4 A4 A3 OR,  MEMW,  A4 A2 0 S32I,  NEXT,
84 CODE BIC!   ( mask a -- )
  A2 SP 0 L32I.N,  A3 SP 4 L32I.N,  SP SP 8 ADDI.N,  A2 >CHIP,
  A5 -1 MOVI.N,  A3 A3 A5 XOR,
  A4 A2 0 L32I,  A4 A4 A3 AND,  MEMW,  A4 A2 0 S32I,  NEXT,
85 CODE SAVE-IMAGE   ( -- ior )     \ (SAVE-IMAGE) ( a u entry -- ior ) of the image, else -21
  W XT-SAVE L32R,
  W L1 BEQZ,
  A2 TABLE-A L32R,  A2 PUSH,        \ the code starts with the table: its address on the data bus
  A2 CODE-LEN L32R,  A2 PUSH,
  A2 ENTRY-A L32R,  A2 PUSH,
  W W MEM ADD.N,  DISPATCH,
LABEL L1
  A2 -21 MOVI,  A2 PUSH,  NEXT,
86 CODE ERASE-IMAGE
  NEXT,

\ MS and US wait until the SYSTIMER reaches a time (deadline). A break
\ may come meanwhile: the deadline stays on the data stack, W becomes the
\ code field V-WAIT (primitive 126), and after the break WAIT-UNTIL goes
\ on. So MS and US wait as long as asked, also when handlers run. A
\ Ctrl-C that waited for the end of a word with NO-BREAK comes here too.

TALIGN LABEL DEADLINE   \ a7:a6 SYSTIMER ticks from now: the deadline onto the stack
  TICKS CALL0,
  A6 A6 A2 ADD,  A7 A7 A3 ADD,
  A6 A2 L1 BGEU,  A7 A7 1 ADDI.N,   \ the carry
LABEL L1
  A6 PUSH,  A7 PUSH,
  WAIT-UNTIL J,

126 CODE (WAIT)   ( lo hi -- )      \ until the SYSTIMER reaches hi:lo
LABEL WAIT-UNTIL
  TICKS CALL0,                      \ a2 lo, a3 hi
  A4 SP 0 L32I.N,  A5 SP 4 L32I.N,
  A4 A3 L2 BLTU,                    \ the deadline is past
  A3 A4 L1 BNE,
  A2 A5 L2 BGEU,
LABEL L1
  A6 TABLE-A L32R,
  TBL A6 L3 BNE,                    \ a break waits
  A6 VARS-A L32R,  A6 A6 V-CTRLC L32I,
  A6 WAIT-UNTIL BEQZ,               \ no Ctrl-C waits: on
LABEL L3
  W WAIT-XT L32R,  BREAK J,         \ after the break: here again
LABEL L2
  SP SP 8 ADDI.N,  NEXT,

TALIGN LABEL RESTART   \ a2: true: BOOT, false: QUIT; empty stacks, no handler runs, NO-BREAK 0
  A3 S0-V L32R,  SP A3 MEM ADD.N,
  A3 R0-V L32R,  RP A3 MEM ADD.N,
  A4 0 MOVI.N,
  A3 STATE-V L32R,  A3 A3 MEM ADD.N,  A4 A3 0 S32I,
  A3 HANDLER-V L32R,  A3 A3 MEM ADD.N,  A4 A3 0 S32I,
  A3 VARS-A L32R,  A4 A3 V-INISR S32I,
  A3 HOLD-V L32R,
  A3 L1 BEQZ,
  A3 >CHIP,  A4 A3 0 S32I,
LABEL L1
  W XT-QUIT L32R,  A3 XT-BOOT L32R,  W A3 A2 MOVNEZ,
  W W MEM ADD.N,
  DISPATCH,

87 CODE REBOOT   ( f -- )
  A2 POP,  RESTART J,
88 CODE BYE                         \ a restart of the chip (RTC_CNTL_OPTIONS0: SW_SYS_RST)
  A3 RTC-CNTL L32R,  A4 BIT31 L32R,
  A5 A3 0 L32I,  A5 A5 A4 OR,  MEMW,  A5 A3 0 S32I,
LABEL L1
  L1 J,

\ ==== HAL words of this target ==========================================
\ SYSCALL calls a function of the ROM. The ROM uses the windowed calls of
\ the Xtensa (CALLX8): the arguments go into a10 to a15, the arguments
\ after the 6th onto the stack of the caller (a1 + 0, + 4 ...), and the
\ result comes back in a10. a10 to a15 are the registers of the VM: they
\ wait in a2 to a6 (a0 to a7 keep their values over the call). a1 is the
\ stack for the ROM (see START). During the call TBL is not the table:
\ V-INROM tells the level-1 handler; SYSCALL looks for a break afterwards.

100 CODE SYSCALL   ( x1 .. xn n a -- x )   \ the function at a with n arguments, n <= 13
  A7 SP 0 L32I.N,                   \ a
  A8 SP 4 L32I.N,                   \ n
  SP SP 8 ADDI.N,
  A9 VARS-A L32R,
  SP A9 V-SP S32I,  RP A9 V-RP S32I,   \ for an exception in the ROM
  A2 1 MOVI.N,  A2 A9 V-INROM S32I,
  A2 MEM MOV.N,  A3 TBL MOV.N,  A4 IP MOV.N,  A6 RP MOV.N,
  A5 A8 SP ADDX4,                   \ the data stack without the arguments
  A9 A5 -24 ADDI,                   \ arguments 1 to 6 at a9 + 20 ... a9 + 0
  A8 A8 -6 ADDI,                    \ the number of arguments after the 6th
  A8 1 L2 BLTI,
  A10 A9 MOV.N,  A11 A1 MOV.N,
LABEL L1                            \ argument 7 from a9 - 4 to a1 + 0, and so on
  A10 A10 -4 ADDI,
  A12 A10 0 L32I.N,  A12 A11 0 S32I.N,
  A11 A11 4 ADDI.N,
  A8 A8 -1 ADDI.N,
  A8 L1 BNEZ,
LABEL L2
  A10 A9 20 L32I,  A11 A9 16 L32I,  A12 A9 12 L32I,
  A13 A9 8 L32I,   A14 A9 4 L32I,   A15 A9 0 L32I,
  A7 CALLX8,
  A9 A10 MOV.N,                     \ the result
  MEM A2 MOV.N,  TBL A3 MOV.N,  IP A4 MOV.N,  SP A5 MOV.N,  RP A6 MOV.N,
  A2 VARS-A L32R,  A3 0 MOVI.N,  A3 A2 V-INROM S32I,
  A3 A2 V-CTRLC L32I,  A4 A2 V-INTREQ L32I,  A3 A3 A4 OR,
  A3 L3 BEQZ,
  TBL BRKTAB-A L32R,                \ a break came during the call
LABEL L3
  A9 PUSH,  NEXT,
101 CODE >CHIP   ( a -- a' )         \ the chip address of a VM address; any other stays
  A2 TOS@,  A2 >CHIP,  A2 TOS!,  NEXT,
102 CODE +INT   ( -- )              \ the handlers on; -21 without the package INTR
  A2 XT-ISR L32R,
  A2 L1 BEQZ,
  A2 VARS-A L32R,  A3 1 MOVI.N,  A3 A2 V-INTON S32I,
  A2 A2 V-INTMASK L32I,  A3 0 MOVI.N,  INTEN CALL0,
  NEXT,
LABEL L1
  A2 -21 MOVI,  KTHROW J,
103 CODE -INT   ( -- )              \ the handlers off
  A2 VARS-A L32R,  HANDLERS-OFF CALL0,
  NEXT,
104 CODE CODE-INT!   ( xt n -- )    \ the CODE word xt handles CPU interrupt n (19 to 21); xt 0: none
  A3 POP,  A2 POP,
  A4 A3 -19 ADDI,  A5 3 MOVI.N,  A4 A5 L9 BGEU,
  A5 1 MOVI.N,  A3 SSL,  A5 A5 SLL,         \ its bit in INTENABLE
  A6 VARS-A L32R,  A7 A4 A6 ADDX4,
  A8 A6 V-INTMASK L32I,  A3 0 MOVI.N,
  A2 L1 BEQZ,
  A2 A2 4 ADDI.N,  A2 A2 MEM ADD.N,  A9 IRAM-OFF L32R,  A2 A2 A9 ADD.N,   \ its machine code
  A2 A7 V-CODE S32I,
  A8 A8 A5 OR,  A8 A6 V-INTMASK S32I,
  A2 0 MOVI.N,
  A9 A6 V-INTON L32I,  A2 A5 A9 MOVNEZ,     \ on now, if the handlers are on
  INTEN CALL0,  NEXT,
LABEL L1
  A2 A7 V-CODE S32I,                        \ xt 0: none, CPU interrupt n off
  A8 A8 A5 OR,  A8 A8 A5 XOR,  A8 A6 V-INTMASK S32I,
  A3 A5 MOV.N,  INTEN CALL0,  NEXT,
LABEL L9
  A2 -24 MOVI,  KTHROW J,

\ ==== Break: Ctrl-C and the Forth handlers ===============================
\ The level-1 handler (LEVEL1) sets TBL to BRKTAB, where each primitive
\ is BREAK. So the next dispatch runs BREAK, at a place where the VM is
\ complete (doc/vm.md, section 9). NEXT costs nothing more. While a
\ function of the ROM runs, SYSCALL looks for a break afterwards; MS and
\ US look for it while they wait (WAIT-UNTIL).
\
\ BREAK puts the table back. After CPU interrupt 1 it runs (ISR) of the
\ package INTR as if the word stood before W: IP and W wait on the
\ return stack, and (ISR) returns to the thread cell of HALT-XT
\ (doc/vm.md, section 3). HALT (127) takes IP and W back, switches CPU
\ interrupt 1 on again (if the handlers are still on) and runs W.
\
\ Ctrl-C waits while a Forth handler runs (V-INISR, until HALT) and
\ while NO-BREAK is not 0; HALT, the interrupts, ?RX, SYSCALL and MS
\ look for it again. Then BREAK drops the input before Ctrl-C and writes
\ a new line; with a CATCH (HANDLER not 0) it runs -28 THROW ("user
\ interrupt"), else QUIT. Ctrl-C twice within 1 s (V-CTRLC 2) also
\ switches the Forth handlers off (as -INT) and writes "handlers off".
\ Ctrl-C while one waits (3) does not wait: it stops a handler or a word
\ with NO-BREAK that hangs.

TALIGN LABEL BREAK   \ W: the xt that runs next (a chip address)
  TBL TABLE-A L32R,
  A2 VARS-A L32R,
  A3 A2 V-CTRLC L32I,
  A3 3 L5 BEQI,                     \ Ctrl-C while one waited: at once
  A3 A2 V-INTREQ L32I,
  A3 L2 BEQZ,
  A3 0 MOVI.N,  A3 A2 V-INTREQ S32I,
  A3 A2 V-INTON L32I,
  A3 L2 BEQZ,
  A3 IP MEM SUB,  RP RP -4 ADDI,  A3 RP 0 S32I.N,   \ IP and W for HALT
  A3 W MEM SUB,   RP RP -4 ADDI,  A3 RP 0 S32I.N,
  A3 HALT-V L32R,  IP A3 MEM ADD.N,  IP IP 4 ADDI.N,
  A3 1 MOVI.N,  A3 A2 V-INISR S32I,                 \ a handler runs
  W XT-ISR L32R,  W W MEM ADD.N,
  DISPATCH,
LABEL L2   \ Ctrl-C, unless it waits
  A3 A2 V-CTRLC L32I,
  A3 L1 BEQZ,
  A3 A2 V-INISR L32I,
  A3 L1 BNEZ,                       \ a handler runs: Ctrl-C waits for HALT
  A3 HOLD-V L32R,
  A3 L5 BEQZ,
  A3 >CHIP,  A3 A3 0 L32I,
  A3 L5 BEQZ,                       \ NO-BREAK not 0: Ctrl-C waits
LABEL L1
  DISPATCH,
LABEL L5   \ Ctrl-C now: its kind in a6
  A6 A2 V-CTRLC L32I,
  A3 0 MOVI.N,  A3 A2 V-CTRLC S32I,  A3 A2 V-INISR S32I,
  A4 HOLD-V L32R,
  A4 L6 BEQZ,
  A4 >CHIP,  A3 A4 0 S32I,          \ NO-BREAK 0: the words with NO-BREAK end now
LABEL L6
  A3 A2 V-HEAD L32I,  A3 A2 V-TAIL S32I,    \ the input before Ctrl-C is gone
  A2 10 MOVI,  TX CALL0,                     \ a new line: column 0
  A6 2 L7 BLTI,
  A2 VARS-A L32R,  A3 0 MOVI.N,  A3 A2 V-INTREQ S32I,
  HANDLERS-OFF CALL0,                        \ twice: the handlers off
  A7 OFF-TEXT-A L32R,
LABEL L8                                     \ "  handlers off", a new line
  A2 A7 0 L8UI,
  A2 L7 BEQZ,
  TX CALL0,
  A7 A7 1 ADDI.N,
  L8 J,
LABEL L7
  A2 0 MOVI.N,  A3 OUT-V L32R,  A3 A3 MEM ADD.N,  A2 A3 0 S32I,
  A3 HANDLER-V L32R,  A3 A3 MEM ADD.N,  A3 A3 0 L32I,
  A3 L9 BEQZ,
  A2 -28 MOVI,  KTHROW J,
LABEL L9
  A2 0 MOVI.N,  RESTART J,                   \ no CATCH: QUIT

127 CODE HALT   \ the end of (ISR): IP and W from the return stack
  W RP 0 L32I.N,  IP RP 4 L32I.N,  RP RP 8 ADDI.N,
  W W MEM ADD.N,  IP IP MEM ADD.N,
  A2 VARS-A L32R,  A3 0 MOVI.N,  A3 A2 V-INISR S32I,
  A3 A2 V-CTRLC L32I,
  A3 L1 BEQZ,
  TBL BRKTAB-A L32R,                \ a Ctrl-C waited for the end of the handlers
LABEL L1
  A2 A2 V-INTON L32I,
  A2 L2 BEQZ,
  A2 2 MOVI.N,  A3 0 MOVI.N,  INTEN CALL0,   \ CPU interrupt 1 on again
LABEL L2
  DISPATCH,

\ ==== The start =========================================================
\ The ROM calls START with CALLX8, in the register window of its function
\ main. START sets its own state: one register window (WINDOWBASE 0,
\ WINDOWSTART 1), a stack for the functions of the ROM, the own vectors.
\ The stack: a1 = ROM-SP, at the top of the RAM of the code, below VM
\ address 0; it grows down to the end of the code (at least 16 KB, see
\ the end of this file). A call of the ROM with CALLX8 can spill the
\ window of the VM onto it (window overflow 8). Then the cell at a1 - 12
\ must hold the stack pointer p of a caller, and a4 to a7 go to the 16
\ bytes at p - 32. START sets p = a1 + 32, so they go to a1 to a1 + 15.
\ The same state after an exception (RECOVER).

TALIGN LABEL START
  A2 0 MOVI.N,
  A2 INTENABLE WSR,  RSYNC,         \ no interrupts
  A3 RTC-CNTL L32R,                 \ the RTC watchdog off
  A4 WDT-KEY L32R,
  A4 A3 $B0 S32I,  A2 A3 $98 S32I,  A2 A3 $B0 S32I,
  A5 SWD-KEY L32R,                  \ the super watchdog feeds itself
  A5 A3 $B8 S32I,
  A5 A3 $B4 L32I,  A6 BIT31 L32R,  A5 A5 A6 OR,  A5 A3 $B4 S32I,
  A2 A3 $B8 S32I,
  A3 TIMG0 L32R,                    \ the watchdogs of timer group 0 and 1 off
  A4 A3 $64 S32I,  A2 A3 $48 S32I,  A2 A3 $64 S32I,
  A3 TIMG1 L32R,
  A4 A3 $64 S32I,  A2 A3 $48 S32I,  A2 A3 $64 S32I,
  MEMW,
  A2 CCOUNT RSR,                    \ wait 0.3 s: ucf leaves the protocol of the ROM
  A3 WAIT-START L32R,
  A2 A2 A3 ADD.N,
LABEL L1
  A3 CCOUNT RSR,
  A3 A3 A2 SUB,
  A3 L1 BLTZ,
  A2 0 MOVI.N,  A2 WINDOWBASE WSR,  RSYNC,
  A2 1 MOVI.N,  A2 WINDOWSTART WSR,  RSYNC,
  A2 PS-VM L32R,  A2 PS WSR,  RSYNC,
  A2 VECTORS-A L32R,  A2 VECBASE WSR,  RSYNC,
  A1 ROM-SP L32R,
  A2 A1 32 ADDI,  A3 A1 -16 ADDI,  A2 A3 4 S32I,   \ a1 - 12: a1 + 32
  A2 VARS-A L32R,  A3 0 MOVI.N,
  A3 A2 V-HEAD S32I,  A3 A2 V-TAIL S32I,  A3 A2 V-CTRLC S32I,
  A3 A2 V-INTREQ S32I,  A3 A2 V-INTON S32I,  A3 A2 V-INROM S32I,
  A3 A2 V-CODE S32I,  A3 A2 V-CODE 4 + S32I,  A3 A2 V-CODE 8 + S32I,
  A3 A2 V-INISR S32I,
  A3 2 MOVI.N,  A3 A2 V-INTMASK S32I,     \ CPU interrupt 1
  A3 126 MOVI,  A3 A2 V-WAIT S32I,        \ the code field for WAIT-UNTIL
  A3 -1000 MOVI,  A3 A2 V-LAST S32I,      \ the last Ctrl-C: long ago
  MEM VM-BASE L32R,  TBL TABLE-A L32R,
  A2 HALT-V L32R,  A3 A2 MEM ADD.N,
  A4 127 MOVI,  A4 A3 0 S32I,  A2 A3 4 S32I,        \ the HALT cells
  A3 UART0 L32R,                    \ UART0: an interrupt for more than 1 byte, or after 10 bits of rest
  A4 A3 $24 L32I,  A5 BIT23 L32R,  A4 A4 A5 OR,  A4 A3 $24 S32I,   \ UART_CONF1: RX_TOUT_EN
  A4 $101 MOVI,  A4 A3 $10 S32I,  A4 A3 $0C S32I,  \ UART_INT_CLR, UART_INT_ENA: RXFIFO_FULL, RXFIFO_TOUT
  A3 UART0-MAP L32R,  A4 23 MOVI,  A4 A3 0 S32I,    \ UART0 to CPU interrupt 23 (level 3)
  MEMW,
  A2 BIT23 L32R,  A2 INTENABLE WSR,  RSYNC,         \ CPU interrupt 23 on
  A2 1 MOVI.N,  RESTART J,          \ BOOT

\ ==== Data: the break table, the variables ================================

TALIGN LABEL BRKTAB   128 CELLS TDP +!     \ BREAK for each number (set at the end)
LABEL VARS   #VARS TDP +!                  \ the variables of the HAL (V-HEAD ...)
: TEXT, ( a u -- )   0 ?DO  DUP I + C@ TC,  LOOP  DROP ;
LABEL OFF-TEXT   S"   handlers off" TEXT,  10 TC,  0 TC,   \ after Ctrl-C twice; 0 ends it

\ ==== The vectors (VECBASE) ==============================================
\ Aligned on 1 KB; each vector has its fixed place (Xtensa ISA reference
\ manual, 4.4.1; ESP-IDF: xtensa_vectors.S). The handlers of the register
\ window are those of the ROM, byte for byte: SYSCALL calls the ROM with
\ CALLX8, and the ROM calls on. Levels 4 and 5 and the NMI are not used.
\ An exception runs FAULT, DOUBLE or DEBUG; an interrupt of level 1 to 3
\ runs LEVEL1, LEVEL2 or LEVEL3.

FORWARD LEVEL1   FORWARD LEVEL2   FORWARD LEVEL3
FORWARD FAULT    FORWARD DOUBLE   FORWARD DEBUG
: ALIGN-1K ( -- )   BEGIN THERE $3FF AND WHILE 0 TC, REPEAT ;
ALIGN-1K
LABEL VECTORS
: VECTOR ( offset -- )   \ the next vector: at VECTORS + offset
  VECTORS +  DUP THERE U< ABORT" the vector before is too long"
  BEGIN THERE OVER U< WHILE 0 TC, REPEAT  DROP ;

$000 VECTOR   \ window overflow 4
  A0 A5 -16 S32E,  A1 A5 -12 S32E,  A2 A5 -8 S32E,  A3 A5 -4 S32E,  RFWO,
$040 VECTOR   \ window underflow 4
  A0 A5 -16 L32E,  A1 A5 -12 L32E,  A2 A5 -8 L32E,  A3 A5 -4 L32E,  RFWU,
$080 VECTOR   \ window overflow 8
  A0 A9 -16 S32E,  A0 A1 -12 L32E,  A1 A9 -12 S32E,  A2 A9 -8 S32E,  A3 A9 -4 S32E,
  A4 A0 -32 S32E,  A5 A0 -28 S32E,  A6 A0 -24 S32E,  A7 A0 -20 S32E,  RFWO,
$0C0 VECTOR   \ window underflow 8
  A0 A9 -16 L32E,  A1 A9 -12 L32E,  A2 A9 -8 L32E,  A7 A1 -12 L32E,  A3 A9 -4 L32E,
  A4 A7 -32 L32E,  A5 A7 -28 L32E,  A6 A7 -24 L32E,  A7 A7 -20 L32E,  RFWU,
$100 VECTOR   \ window overflow 12
  A0 A13 -16 S32E,  A0 A1 -12 L32E,  A1 A13 -12 S32E,  A2 A13 -8 S32E,  A3 A13 -4 S32E,
  A4 A0 -48 S32E,  A5 A0 -44 S32E,  A6 A0 -40 S32E,  A7 A0 -36 S32E,
  A8 A0 -32 S32E,  A9 A0 -28 S32E,  A10 A0 -24 S32E,  A11 A0 -20 S32E,  RFWO,
$140 VECTOR   \ window underflow 12
  A0 A13 -16 L32E,  A1 A13 -12 L32E,  A2 A13 -8 L32E,  A11 A1 -12 L32E,  A3 A13 -4 L32E,
  A4 A11 -48 L32E,  A5 A11 -44 L32E,  A6 A11 -40 L32E,  A7 A11 -36 L32E,
  A8 A11 -32 L32E,  A9 A11 -28 L32E,  A10 A11 -24 L32E,  A11 A11 -20 L32E,  RFWU,
$180 VECTOR   \ level 2: the CODE handlers
  A2 EXCSAVE2 WSR,  LEVEL2 J,
$1C0 VECTOR   \ level 3: UART0
  A2 EXCSAVE3 WSR,  LEVEL3 J,
$200 VECTOR  4 RFI,                 \ level 4
$240 VECTOR  5 RFI,                 \ level 5
$280 VECTOR   \ debug: a BREAK instruction
  A2 EXCSAVE6 WSR,  DEBUG J,
$2C0 VECTOR  7 RFI,                 \ NMI
$300 VECTOR   \ kernel: an exception with PS.UM 0 (the VM does not run so)
  A2 EXCSAVE1 WSR,  FAULT J,
$340 VECTOR   \ user: an exception, or an interrupt of level 1 (cause 4)
  A2 EXCSAVE1 WSR,
  A2 EXCCAUSE RSR,
  A2 4 L1 BNEI,
  LEVEL1 J,
LABEL L1
  FAULT J,
$3C0 VECTOR   \ double: an exception in an exception handler
  A2 EXCSAVE1 WSR,  DOUBLE J,
$400 VECTOR

\ ==== Interrupts ========================================================
\ A handler runs between two instructions of the VM, of a handler of a
\ lower level, or of the ROM, with a2 in EXCSAVEn. It saves the
\ registers that it uses in V-SAVE and changes only TBL. A break waits
\ (REQUEST): TBL = BRKTAB, but not while a function of the ROM runs (TBL
\ is in a3 of SYSCALL then, and a11 belongs to the ROM).

LABEL LEVEL1   \ CPU interrupt 1: a source for the Forth handlers; off until HALT
  A2 VARS-A L32R,
  A0 A2 V-SAVE S32I,  A3 A2 V-SAVE 4 + S32I,  A4 A2 V-SAVE 8 + S32I,  A5 A2 V-SAVE 12 + S32I,
  A3 INTERRUPT RSR,  A4 INTENABLE RSR,  A3 A3 A4 AND,
  A3 1 L1 BBCI,
  A5 -3 MOVI,  A4 A4 A5 AND,  A4 INTENABLE WSR,
  A4 1 MOVI.N,  A4 A2 V-INTREQ S32I,
LABEL L1
  REQUEST CALL0,
  A0 A2 V-SAVE L32I,  A3 A2 V-SAVE 4 + L32I,  A4 A2 V-SAVE 8 + L32I,  A5 A2 V-SAVE 12 + L32I,
  A2 EXCSAVE1 RSR,
  RFE,

LABEL LEVEL2   \ CPU interrupts 19 to 21: the CODE word of the highest one, with CALLX0
  \ The CODE word may use a0 and a2 to a9, and SAR; it ends with RET.
  A2 VARS-A L32R,
  A0 A2 V-SAVE S32I,       A3 A2 V-SAVE 4 + S32I,   A4 A2 V-SAVE 8 + S32I,
  A5 A2 V-SAVE 12 + S32I,  A6 A2 V-SAVE 16 + S32I,  A7 A2 V-SAVE 20 + S32I,
  A8 A2 V-SAVE 24 + S32I,  A9 A2 V-SAVE 28 + S32I,
  A3 SAR RSR,  A3 A2 V-SAVE 32 + S32I,
  A3 INTERRUPT RSR,  A4 INTENABLE RSR,  A3 A3 A4 AND,
  A4 CODE-INTS L32R,  A3 A3 A4 AND,
  A3 L9 BEQZ,
  A4 A3 NSAU,  A5 31 MOVI,  A4 A5 A4 SUB,   \ the number of the CPU interrupt
  A5 A4 -19 ADDI,  A5 A5 A2 ADDX4,  A5 A5 V-CODE L32I,
  A5 L8 BEQZ,
  A5 CALLX0,
  L9 J,
LABEL L8                            \ no CODE word: this CPU interrupt off
  A5 1 MOVI.N,  A4 SSL,  A5 A5 SLL,
  A6 INTENABLE RSR,  A6 A6 A5 OR,  A6 A6 A5 XOR,  A6 INTENABLE WSR,
LABEL L9
  A2 VARS-A L32R,
  A3 A2 V-SAVE 32 + L32I,  A3 SAR WSR,
  A0 A2 V-SAVE L32I,       A3 A2 V-SAVE 4 + L32I,   A4 A2 V-SAVE 8 + L32I,
  A5 A2 V-SAVE 12 + L32I,  A6 A2 V-SAVE 16 + L32I,  A7 A2 V-SAVE 20 + L32I,
  A8 A2 V-SAVE 24 + L32I,  A9 A2 V-SAVE 28 + L32I,
  A2 EXCSAVE2 RSR,
  2 RFI,

LABEL LEVEL3   \ CPU interrupt 23, UART0: the bytes into RX; Ctrl-C twice switches the handlers off at once
  A2 VARS-A L32R,
  A0 A2 V-SAVE S32I,       A3 A2 V-SAVE 4 + S32I,   A4 A2 V-SAVE 8 + S32I,
  A5 A2 V-SAVE 12 + S32I,  A6 A2 V-SAVE 16 + S32I,  A7 A2 V-SAVE 20 + S32I,
  A8 A2 V-SAVE 24 + S32I,
  DRAIN CALL0,
  A4 A2 V-CTRLC L32I,
  A4 2 L1 BLTI,
  A4 0 MOVI.N,  A4 A2 V-INTON S32I,          \ as -INT, here (INTEN would lower the level)
  A4 A2 V-INTMASK L32I,  A5 INTENABLE RSR,  A5 A5 A4 OR,  A5 A5 A4 XOR,  A5 INTENABLE WSR,
LABEL L1
  REQUEST CALL0,
  A0 A2 V-SAVE L32I,       A3 A2 V-SAVE 4 + L32I,   A4 A2 V-SAVE 8 + L32I,
  A5 A2 V-SAVE 12 + L32I,  A6 A2 V-SAVE 16 + L32I,  A7 A2 V-SAVE 20 + L32I,
  A8 A2 V-SAVE 24 + L32I,
  A2 EXCSAVE3 RSR,
  3 RFI,

\ ==== Exceptions ========================================================
\ An exception of the CPU in the VM or in a function of the ROM: FAULT
\ keeps its cause, PC and address, and SP and RP of the VM (V-SP and V-RP
\ hold them while the ROM runs). It sets the state of START again (one
\ register window) and leaves the exception to RECOVER. RECOVER runs
\ (FAULT) of the package INTR ( addr pc cause -- ); without it: -9 THROW,
\ or QUIT without a CATCH. A stack pointer outside its stack becomes
\ the start of the stack.
\
\ First the registers of the exception go to FAULT-REGS of the package
\ INTR, for the monitor (asm/monitor/monesp.fs): a cell each for PC, PS,
\ SAR, the cause (EXCCAUSE, or 64 for DEBUG), EXCVADDR, a0 to a15 (the
\ register window at the exception), LBEG, LEND, LCOUNT, WINDOWBASE,
\ WINDOWSTART, DEBUGCAUSE. PS is as the handler sees it (EXCM is 1).
\ A debug exception (BREAK, ICOUNT, IBREAK) goes to the machine code at
\ DEBUG-HOOK of INTR instead, when it is not 0: the monitor stops code
\ there and lets it go on later (asm/monitor/monesp.fs, M4).

: L8-HERE ( -- )   ['] L8 >BODY RESOLVE ;   \ LABEL L8, inside a word (LABEL parses)
: SAVE-REGS, ( pc save debug? -- )   \ a2 is in the register save: the registers into FAULT-REGS
  >R  SWAP >R >R                      \ R: debug? pc save
  A2 FREGS-A L32R,
  A2 L8 BEQZ,
  A0 A2 20 S32I,   A1 A2 24 S32I,   A3 A2 32 S32I,   A4 A2 36 S32I,
  A5 A2 40 S32I,   A6 A2 44 S32I,   A7 A2 48 S32I,   A8 A2 52 S32I,
  A9 A2 56 S32I,   A10 A2 60 S32I,  A11 A2 64 S32I,  A12 A2 68 S32I,
  A13 A2 72 S32I,  A14 A2 76 S32I,  A15 A2 80 S32I,
  A3 R> RSR,  A3 A2 28 S32I,                          \ a2
  A3 R> RSR,  A3 A2 0 S32I,                           \ the PC
  A3 PS RSR,  A3 A2 4 S32I,   A3 SAR RSR,  A3 A2 8 S32I,
  R> IF  A3 64 MOVI,  ELSE  A3 EXCCAUSE RSR,  THEN  A3 A2 12 S32I,
  A3 EXCVADDR RSR,  A3 A2 16 S32I,
  A3 LBEG RSR,  A3 A2 84 S32I,   A3 LEND RSR,  A3 A2 88 S32I,   A3 LCOUNT RSR,  A3 A2 92 S32I,
  A3 WINDOWBASE RSR,  A3 A2 96 S32I,   A3 WINDOWSTART RSR,  A3 A2 100 S32I,
  A3 DEBUGCAUSE RSR,  A3 A2 104 S32I,
  MEMW,
  L8-HERE ;

FORWARD FAULT-GO
LABEL FAULT   \ a2 is in EXCSAVE1
  EPC1 EXCSAVE1 FALSE SAVE-REGS,
  A2 VARS-A L32R,
  A3 EXCCAUSE RSR,  A3 A2 V-CAUSE S32I,
  A3 EPC1 RSR,
  FAULT-GO J,
LABEL DOUBLE   \ an error in a handler, or in FAULT: the handlers off; a2 is in EXCSAVE1
  DEPC EXCSAVE1 FALSE SAVE-REGS,
  A2 VARS-A L32R,
  A3 EXCCAUSE RSR,  A3 A2 V-CAUSE S32I,
  A3 0 MOVI.N,  A3 A2 V-INTON S32I,
  A3 A2 V-INTMASK L32I,  A4 INTENABLE RSR,  A4 A4 A3 OR,  A4 A4 A3 XOR,  A4 INTENABLE WSR,
  A3 DEPC RSR,
  FAULT-GO J,
LABEL DEBUG   \ the cause: 64 (EXCCAUSE has no number for it); a2 is in EXCSAVE6
  A2 FREGS-A L32R,                         \ DEBUG-HOOK of INTR (FAULT-REGS + 108): the monitor
  A2 L7 BEQZ,
  A2 A2 108 L32I,
  A2 L7 BEQZ,
  A2 JX,                                   \ to the hook: a2 in EXCSAVE6, all else as at the exception
LABEL L7
  EPC6 EXCSAVE6 TRUE SAVE-REGS,
  A2 VARS-A L32R,
  A3 64 MOVI,  A3 A2 V-CAUSE S32I,
  A3 EPC6 RSR,
LABEL FAULT-GO   \ a2: VARS, a3: the PC
  A3 A2 V-PC S32I,
  A3 EXCVADDR RSR,  A3 A2 V-ADDR S32I,
  A3 A2 V-INROM L32I,
  A3 L1 BNEZ,
  SP A2 V-SP S32I,  RP A2 V-RP S32I,       \ the VM ran: SP and RP are in this window
LABEL L1
  A3 0 MOVI.N,  A3 A2 V-INROM S32I,
  A3 LCOUNT WSR,                          \ no loop of the ROM goes on
  A3 WINDOWBASE WSR,  RSYNC,               \ one register window, as at the start
  A3 1 MOVI.N,  A3 WINDOWSTART WSR,  RSYNC,
  A3 PS-EXC L32R,  A3 PS WSR,  RSYNC,
  A3 RECOVER-A L32R,  A3 EPC1 WSR,
  RFE,

LABEL RECOVER   \ level 0 again, not in an exception
  A1 ROM-SP L32R,
  MEM VM-BASE L32R,
  A2 VARS-A L32R,
  SP A2 V-SP L32I,                  \ the data stack: R0 to S0 (doc/vm.md 3)
  A3 SP MEM SUB,  A4 R0-V L32R,  A3 A3 A4 SUB,
  A4 $401 MOVI,  A3 A4 L1 BLTU,
  A3 S0-V L32R,  SP A3 MEM ADD.N,
LABEL L1
  RP A2 V-RP L32I,                  \ the return stack: R0 - 1024 to R0
  A3 RP MEM SUB,  A4 R0-V L32R,  A3 A3 A4 SUB,  A3 A3 1024 ADDMI,
  A4 $401 MOVI,  A3 A4 L2 BLTU,
  A3 R0-V L32R,  RP A3 MEM ADD.N,
LABEL L2
  TBL TABLE-A L32R,
  A3 A2 V-CTRLC L32I,  A4 A2 V-INTREQ L32I,  A3 A3 A4 OR,
  A3 L3 BEQZ,
  TBL BRKTAB-A L32R,                \ a break came meanwhile
LABEL L3
  W XT-FAULT L32R,
  W L4 BEQZ,
  A3 A2 V-ADDR L32I,  A3 PUSH,  A3 A2 V-PC L32I,  A3 PUSH,  A3 A2 V-CAUSE L32I,  A3 PUSH,
  W W MEM ADD.N,  DISPATCH,
LABEL L4
  A3 HANDLER-V L32R,  A3 A3 MEM ADD.N,  A3 A3 0 L32I,
  A3 L5 BEQZ,
  A2 -9 MOVI,  KTHROW J,
LABEL L5
  A2 0 MOVI.N,  RESTART J,

\ ==== The addresses that the code above needs ===========================

: ROOM? ( -- )   THERE $4037C000 U< 0= ABORT" the code reaches the stack of the ROM functions" ;
ROOM?

VARS >DRAM - VARS-A T!
BRKTAB >DRAM - BRKTAB-A T!
VECTORS VECTORS-A T!
RECOVER RECOVER-A T!
VARS V-WAIT + >DRAM - WAIT-XT T!
OFF-TEXT >DRAM - OFF-TEXT-A T!
: FILL-BRKTAB ( -- )   128 0 DO  BREAK BRKTAB I CELLS + T!  LOOP ;
FILL-BRKTAB
