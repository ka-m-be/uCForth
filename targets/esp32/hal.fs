\ hal.fs - the HAL of uCForth for the ESP32 (for example in the module ESP-WROOM-32)
\
\ The start code, the console (primitives 29-31) over UART0, the time
\ (80-82) from timer 0 of timer group 0, and BYE as a restart of the
\ chip. It is the HAL of targets/esp32s3, with the addresses of the
\ ESP32 and these differences: the code is in the RAM of the instruction
\ bus (SRAM0), and the CPU reads it there only in cells of 32 bits; so
\ the variables of the HAL are in the RAM for data (VARS-ADDR). The ESP32
\ has no SYSTIMER: the time comes from timer 0 of timer group 0 at 1 MHz
\ (the clock of the timers is the APB clock: 40 MHz at the start;
\ package CLOCK sets the divider when it changes). Loaded by
\ build.fs after kernels/xtensa.fs. HAL words of this target: SYSCALL
\ (100) calls a function of the ROM, >CHIP (101) gives a chip address
\ (package ROM, lib/esp32/rom.fs); +INT (102) and -INT (103) switch the
\ handlers of interrupts on and off, CODE-INT! (104) makes a CODE word
\ the handler of a CPU interrupt (package INTR, lib/esp32/intr.fs);
\ FLASH-SYSCALL (105) is SYSCALL for the functions of the ROM for the
\ flash (package FLASH, through the layer of the chip).
\ THIS-CORE (106), CORE-ENTRY (107) and CORE-PARK (108) serve the other
\ core (packages ESP32-CORES and MULTICORE, lib/esp32/esp32/cores.fs): each
\ core runs the VM with its own state (see "Cores").
\ SAVE-IMAGE (85) runs the word (SAVE-IMAGE) of the image, if the image
\ has one (lib/esp32/esp32/bootimg.fs). The CPU runs no code in the RAM
\ for data: the machine code of a CODE word lies in SRAM0, and this HAL
\ has its own primitive 66 for it (see DOCODE).
\
\ The memory of the VM (targets/esp32/README.md, "Memory"): VM addresses
\ 0 to $FFFF (SYS and the RAM data) lie in SRAM1, the others in SRAM2
\ (build.fs: DATA-ADDR and RAM-ADDR; DATA-OFF of asm/xtensa.fs). So the
\ constants STATE-V, HANDLER-V and OUT-V hold the VM address + DATA-OFF:
\ the code adds MEM. The cells of HALT-XT lie in the 16 free bytes after
\ the dictionary, in SRAM2 like the dictionary: IP and W never point
\ into SRAM1.
\
\ The frozen part of the dictionary can lie in the flash (FLASH-DICT? of
\ asm/xtensa.fs): slots 120 to 122 of the table of the primitives hold
\ the end of this part, the chip address of VM address 0 in the flash
\ (DROM-ADDR) and S0. TBL points to copies of the table and of the break
\ table in SRAM1 (TABLES-ADDR; START makes them, see COPY-TABLE). An
\ image of build.fs has S0 as the end: all in the RAM. FREEZE (lib/esp32/esp32/bootimg.fs) writes
\ the dictionary into the flash and moves the end. START sets up the
\ cache of the flash for it.
\
\ The console: UART0 (GPIO 1 and 3) at the USB serial bridge of the
\ board. The ROM has set it up: 115200 baud, 8 data bits, no parity.
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
\ The registers are in the Technical Reference Manual of the ESP32,
\ the exceptions and the register window in the Xtensa Instruction Set
\ Architecture Reference Manual (chapter 4.4 and 4.7).

\ ==== Constants for L32R ================================================

TALIGN
$3FFF8000 CONSTANT VARS-ADDR        \ core 0: the variables of the HAL, in the RAM for data (SRAM1)
VARS-ADDR 512 + CONSTANT TABLES-ADDR   \ core 0: the copies of the two tables for TBL (SRAM1), 512 bytes each
$3FFFFFC0 CONSTANT ROM-SP0          \ core 0: a1, the stack for the functions of the ROM, at the top of SRAM1
$3FFE5400 CONSTANT VARS1-ADDR       \ core 1: the same in its part of SRAM1 (see "Cores")
VARS1-ADDR 512 + CONSTANT TABLES1-ADDR
$3FFE7FC0 CONSTANT ROM-SP1
$3FFAE700 CONSTANT DATA1-ADDR       \ core 1: VM address 0 of its SYS and RAM data of the base image (SRAM2)
$3FF9F600 CONSTANT STACKS1-ADDR     \ core 1: VM address 0 of its TIB, PAD and stacks: $10000 at $3FFA_F600
$0F00 CONSTANT PE-MAX               \ core 1: its copy of VM addresses $100 up to PE ends below $3FFA_F600
2 CONSTANT #CORES
LABEL UART0      $3FF40000 T,       \ FIFO at 0 (read), STATUS at $1C, CONF1 at $24, MEM_RX_STATUS at $60
LABEL UART0-FIFO $60000000 T,       \ the FIFO of UART0 on the AHB bus: the CPU writes it there (as ESP-IDF)
LABEL RTC-CNTL   $3FF48000 T,
LABEL TIMG0      $3FF5F000 T,
LABEL TIMG1      $3FF60000 T,
LABEL UART0-MAP  $3FF0018C T,       \ the interrupt matrix: the CPU interrupt of UART0 (source 34)
LABEL CODE-INTS  $00380000 T,       \ CPU interrupts 19, 20, 21: level 2, the CODE handlers
LABEL WDT-KEY    $50D83AA1 T,
LABEL BIT31      $80000000 T,
LABEL BIT23      $00800000 T,       \ also: CPU interrupt 23, level 3
LABEL CONF1-KEEP $00FFFF80 T,       \ UART_CONF1: the bits that stay
LABEL CONF1-RX   $82000001 T,       \ RX_TOUT_EN, RX_TOUT_THRHD 2, RXFIFO_FULL_THRHD 1
LABEL WAIT-START 12000000 T,        \ CPU cycles: 0.3 s at 40 MHz
LABEL NAP        24000 T,           \ CPU cycles: a sleep of another core in MS (0.1 ms at 240 MHz)
LABEL TICKS/MS   1000 T,            \ timer 0 of timer group 0: 1 MHz
LABEL TG0-1MHZ   $C0000000 40 13 LSHIFT OR T,   \ its T0CONFIG: on, up; the APB clock (40 MHz) / 40
LABEL XT-SAVE    0 T,               \ build.fs: the xt of (SAVE-IMAGE); 0: none
LABEL CODE-LEN   0 T,               \ build.fs: the length of the code (a multiple of 4)
LABEL ENTRY-A    0 T,               \ build.fs: the address of START
LABEL XT-ISR     0 T,               \ build.fs: the xt of (ISR) of the package INTR; 0: none
LABEL XT-FAULT   0 T,               \ build.fs: the xt of (FAULT) of the package INTR; 0: none
LABEL FREGS-A    0 T,               \ build.fs: the chip address of FAULT-REGS of the package INTR; 0: none
LABEL OUT-V      0 T,               \ build.fs: the VM address of OUT (SYS+, adds the base of the core)
LABEL HOLD-V     0 T,               \ build.fs: the address of NO-BREAK of the package IDLE; 0: none
LABEL HALT-V     DICT-END T,        \ HALT-XT: the code field 127, then a thread cell (doc/vm.md 3)
LABEL PS-VM      $00040020 T,       \ PS: register window, user mode, interrupts on
LABEL PS-EXC     $00040030 T,       \ the same, in an exception (EXCM)
LABEL PE-V       0 T,               \ build.fs: PE, the end of the RAM data of the base image (see "Cores")
LABEL CORE-TAB-A 0 T,               \ the table of the cores (set at the end)
LABEL START1-A   0 T,               \ START1, the start of the other cores (set at the end)
LABEL CACHE-CTRL $3FF00040 T,       \ DPORT_PRO_CACHE_CTRL_REG: bit 3 the cache on, bit 4 flush, bit 5 flush done
LABEL CACHE-MASK $3FF00044 T,       \ DPORT_PRO_CACHE_CTRL1_REG: bit 4 masks DROM0 ($3F40_0000 ...)
LABEL CACHE-DBUG $3FF003F0 T,       \ DPORT_PRO_DCACHE_DBUG0_REG: bits 18-7 are 1 when the cache rests
LABEL SPI0-CACHE $3FF43050 T,       \ SPI_CACHE_FCTRL_REG of SPI0: bit 0, SPI0 serves the cache (CACHE_REQ_EN)
LABEL MMU-INIT   $400095A4 T,       \ mmu_init ( cpu -- ): all entries of the MMU empty
LABEL MMU-SET    $400095E0 T,       \ cache_flash_mmu_set ( cpu pid vaddr paddr psize num -- r )
LABEL READ-MODE  $40062B64 T,       \ esp_rom_spiflash_config_readmode ( mode -- r ); 2: DIO
LABEL SPI-CLOCK  $40062BC8 T,       \ esp_rom_spiflash_config_clk ( div spi -- r ); 2 0: SPI0, 80 MHz / 2
LABEL DROM-PAGE  DROM-ADDR $10000 + T,   \ VM address $10000 in the flash, for the CPU
LABEL DICT-PAGE  DICT-FLASH $10000 + T,  \ VM address $10000 in the flash: the flash address
LABEL BRKTAB-A   0 T,               \ the break table, in the code (set at the end; read in cells)
LABEL VECTORS-A  0 T,               \ VECBASE (set at the end)
LABEL RECOVER-A  0 T,               \ (set at the end)
LABEL OFF-TEXT-A 0 T,               \ the text after Ctrl-C twice, a byte in each cell (set at the end)

\ The variables of the HAL: each core has its own, in the RAM for data
\ (core 0 at VARS-ADDR), and its register MISC0 holds their address. The
\ bytes 0 to 255 are the buffer RX (only core 0 has the console); the
\ offsets of the other cells:
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
360 CONSTANT V-LAST      \ the time of the last Ctrl-C, in steps of 2^16 us of the timer (65.5 ms)
364 CONSTANT V-TBL       \ TBL: the copy of the table of the primitives of this core
368 CONSTANT V-BRK       \ TBL during a break: the copy of the break table
372 CONSTANT V-ROMSP     \ a1: the stack of this core for the functions of the ROM
376 CONSTANT V-TIMER     \ its clock: timer 0 (core 0) or 1 (core 1) of timer group 0, 1 MHz
380 CONSTANT V-ASK       \ core 1: the register FROM_CPU, by which core 0 asks for a break; core 0: 0 (the console)
384 CONSTANT V-FREGS     \ the chip address of FAULT-REGS (package INTR) of this core, or 0
388 CONSTANT V-XT        \ core 1: the xt that it runs after its start (CORE-ENTRY)
392 CONSTANT #VARS

FORWARD BREAK   FORWARD DEADLINE   FORWARD WAIT-UNTIL   FORWARD ROM-CALL   FORWARD VM-STATE

\ ==== Subroutines: CALL0, the return address in A0 ======================

TALIGN LABEL TX   \ send the character in a2; uses a3 to a5
  A3 UART0 L32R,
LABEL L1
  A4 A3 $1C L32I,                   \ UART_STATUS
  A4 A4 16 8 EXTUI,                 \ bytes in the transmit FIFO (TXFIFO_CNT, 8 bits)
  A5 120 MOVI,
  A4 A5 L1 BGEU,                    \ nearly full: wait
  A3 UART0-FIFO L32R,
  MEMW,
  A2 A3 0 S32I,                     \ into the FIFO, on the AHB bus
  RET,

TALIGN LABEL TICKS   \ a2: the timer of this core (V-TIMER), bits 31-0, a3: bits 63-32 (1 MHz); uses a4 a5
  A5 3 RSIL,                        \ no interrupt meanwhile: DRAIN reads the timer of core 0 too
  A4 MISC0 RSR,  A4 A4 V-TIMER L32I,
  MEMW,  A4 A4 $0C S32I,            \ TIMG_TnUPDATE: any value latches the counter
  MEMW,
  A3 A4 8 L32I,                     \ TIMG_TnHI
  A2 A4 4 L32I,                     \ TIMG_TnLO
  A5 PS WSR,  RSYNC,
  RET,

TALIGN LABEL DRAIN   \ the bytes that wait in UART0 into RX; a2: VARS; uses a4 to a8
  A4 UART0 L32R,
  A5 $101 MOVI,  MEMW,  A5 A4 $10 S32I,     \ UART_INT_CLR: RXFIFO_FULL, RXFIFO_TOUT
LABEL L1                            \ a byte waits? The pointers of the FIFO (as ESP-IDF: RXFIFO_CNT
  A5 A4 $60 L32I,                   \ is not sure when the FIFO is read on the DPORT bus); UART_MEM_RX_STATUS
  A6 A5 13 11 EXTUI,  A5 A5 2 11 EXTUI,    \ the write pointer, the read pointer
  A5 A6 L3 BNE,
  A5 A4 $1C L32I,  A5 A5 0 8 EXTUI, \ the same: empty, or full (RXFIFO_CNT not 0)
  A5 L9 BEQZ,
LABEL L3
  A5 A4 0 L32I,  A5 A5 0 8 EXTUI,
  A5 3 L2 BNEI,
  \ Ctrl-C: the break, not a byte in RX. V-CTRLC: 1; 2 if the last Ctrl-C
  \ came less than 1 s before; 3 if a Ctrl-C still waits
  A6 TIMG0 L32R,  MEMW,  A6 A6 $0C S32I,  MEMW,    \ the timer as in TICKS
  A7 A6 8 L32I,  A8 A6 4 L32I,
  A7 A7 16 SLLI,  A8 A8 16 16 EXTUI,  A7 A7 A8 OR,   \ bits 47-16: steps of 65.5 ms
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

\ COPY-TABLE: TBL points to copies of the two tables in SRAM1 (TABLES-ADDR),
\ not to the tables in the code: the CPU reads SRAM0 on the data bus
\ about 8 cycles slower than SRAM1 (measured: 33 ns more for each @), and
\ NEXT reads 3 cells at TBL. START makes the copies. The table in the
\ code keeps slots 120 to 122 for the image (FREEZE changes both).

TALIGN LABEL COPY-TABLE   \ 128 cells from a2 to a3, a3 + 512 afterwards; uses a4 a5
  A4 128 MOVI,
LABEL L1
  A5 A2 0 L32I,  A5 A3 0 S32I,
  A2 A2 4 ADDI.N,  A3 A3 4 ADDI.N,  A4 A4 -1 ADDI.N,
  A4 L1 BNEZ,
  RET,

TALIGN LABEL CACHE-ON   \ the cache of the flash: flush it, then on (ESP32 TRM 1.3.4); uses a2 a7 a8
  A7 CACHE-CTRL L32R,  A8 A7 0 L32I,
  A2 -17 MOVI,  A8 A8 A2 AND,  MEMW,  A8 A7 0 S32I,     \ FLUSH_ENA 0,
  A2 16 MOVI,  A2 A8 A2 OR,  MEMW,  A2 A7 0 S32I,       \ then 1: the flush starts
LABEL L1
  A2 A7 0 L32I,  A2 5 L1 BBCI,                          \ until FLUSH_DONE
  A2 8 MOVI,  A8 A8 A2 OR,  MEMW,  A8 A7 0 S32I,        \ FLUSH_ENA 0, PRO_CACHE_ENABLE 1
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
  TBL A2 V-BRK L32I,
LABEL L1
  RET,

\ ==== The console =======================================================

29 CODE ?RX   ( -- c -1 | 0 )       \ from RX; the bytes in the FIFO go to RX first
  A9 3 RSIL,                        \ no interrupt of UART0 meanwhile: the bytes stay in order
  A2 MISC0 RSR,
  DRAIN CALL0,
  A3 A2 V-CTRLC L32I,
  A3 L1 BEQZ,
  TBL A2 V-BRK L32I,                \ Ctrl-C came: the break at the next NEXT
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

80 CODE MILLIS   \ the timer / 1000; runs over after 2^32 ms
  TICKS CALL0,
  A4 TICKS/MS L32R,
  A3 A3 A4 REMU,                    \ the high part mod 16000 keeps the low 32 bits right
  UDIV CALL0,
  A2 PUSH,  NEXT,
81 CODE MS   ( n -- )               \ until the timer is n ms further (DEADLINE)
  A9 POP,
  A9 1 L1 BLTI,
  A4 TICKS/MS L32R,  A6 A9 A4 MULL,  A7 A9 A4 MULUH,
  DEADLINE J,
LABEL L1
  NEXT,
82 CODE US   ( n -- )               \ until the timer is n us further
  A9 POP,
  A9 1 L1 BLTI,
  A6 A9 MOV.N,  A7 0 MOVI.N,        \ 1 tick per us
  DEADLINE J,
LABEL L1
  NEXT,
83 CODE BIS!   ( mask a -- )        \ a cell: the registers of the chip have 32 bits
  A2 SP 0 L32I.N,  A3 SP 4 L32I.N,  SP SP 8 ADDI.N,  A2 RO-ERROR >CHIP!,
  A4 A2 0 L32I,  A4 A4 A3 OR,  MEMW,  A4 A2 0 S32I,  NEXT,
84 CODE BIC!   ( mask a -- )
  A2 SP 0 L32I.N,  A3 SP 4 L32I.N,  SP SP 8 ADDI.N,  A2 RO-ERROR >CHIP!,
  A5 -1 MOVI.N,  A3 A3 A5 XOR,
  A4 A2 0 L32I,  A4 A4 A3 AND,  MEMW,  A4 A2 0 S32I,  NEXT,
85 CODE SAVE-IMAGE   ( -- ior )     \ (SAVE-IMAGE) ( a u entry -- ior ) of the image, else -21
  W XT-SAVE L32R,
  W L1 BEQZ,
  A2 TABLE-A L32R,  A2 PUSH,        \ the code starts with the table: its address on the data bus
  A2 CODE-LEN L32R,  A2 PUSH,
  A2 ENTRY-A L32R,  A2 PUSH,
  W W A2 A3 +BASE,  DISPATCH,
LABEL L1
  A2 -21 MOVI,  A2 PUSH,  NEXT,
86 CODE ERASE-IMAGE
  NEXT,

\ MS and US wait until the timer reaches a time (deadline). A break
\ may come meanwhile: the deadline stays on the data stack, W becomes the
\ code field V-WAIT (primitive 126), and after the break WAIT-UNTIL goes
\ on. So MS and US wait as long as asked, also when handlers run. A
\ Ctrl-C that waited for the end of a word with NO-BREAK comes here too.
\ Another core than core 0 sleeps meanwhile, while more than 200 us are
\ left (the two cores fetch their code from SRAM0: a core that waits in a
\ loop slows the other one down): WAITI until its CPU timer (CCOMPARE0,
\ CPU interrupt 6, NAP cycles later; LEVEL1 switches it off again) or
\ another interrupt comes.

TALIGN LABEL DEADLINE   \ a7:a6 ticks of the timer from now: the deadline onto the stack
  TICKS CALL0,
  A6 A6 A2 ADD,  A7 A7 A3 ADD,
  A6 A2 L1 BGEU,  A7 A7 1 ADDI.N,   \ the carry
LABEL L1
  A6 PUSH,  A7 PUSH,
  WAIT-UNTIL J,

126 CODE (WAIT)   ( lo hi -- )      \ until the timer reaches hi:lo
LABEL WAIT-UNTIL
  TICKS CALL0,                      \ a2 lo, a3 hi
  A4 SP 0 L32I.N,  A5 SP 4 L32I.N,
  A4 A3 L2 BLTU,                    \ the deadline is past
  A3 A4 L1 BNE,
  A2 A5 L2 BGEU,
LABEL L1
  A6 MISC0 RSR,  A7 A6 V-TBL L32I,
  TBL A7 L3 BNE,                    \ a break waits
  A7 A6 V-CTRLC L32I,
  A7 L3 BNEZ,                       \ a Ctrl-C waits
  A7 A6 V-ASK L32I,
  A7 WAIT-UNTIL BEQZ,               \ core 0: on
  A7 A5 A2 SUB,  A9 200 MOVI,  A7 A9 WAIT-UNTIL BLTU,   \ less than 200 us left: on, without a sleep
  A8 15 RSIL,                       \ no interrupt between the timer and WAITI
  A7 CCOUNT RSR,  A9 NAP L32R,  A7 A7 A9 ADD.N,  A7 CCOMPARE0 WSR,
  A7 INTENABLE RSR,  A9 64 MOVI,  A7 A7 A9 OR,  A7 INTENABLE WSR,  RSYNC,   \ CPU interrupt 6 on
  0 WAITI,                          \ PS.INTLEVEL 0 again; after the interrupt: on
  WAIT-UNTIL J,
LABEL L3
  W MISC0 RSR,  A6 V-WAIT MOVI,  W W A6 ADD.N,   \ the cell V-WAIT of this core: an xt for primitive 126
  BREAK J,                          \ after the break: here again
LABEL L2
  SP SP 8 ADDI.N,  NEXT,

TALIGN LABEL RESTART   \ a2: true: BOOT, false: QUIT; empty stacks, no handler runs, NO-BREAK 0
  W XT-QUIT L32R,  A3 XT-BOOT L32R,  W A3 A2 MOVNEZ,
LABEL RESTART-W   \ the same with the xt in W (a VM address); START1 comes here
  A3 S0-V L32R,  A4 MISC2 RSR,  SP A3 A4 ADD.N,     \ the stacks of this core
  A3 R0-V L32R,  RP A3 A4 ADD.N,
  A4 0 MOVI.N,
  A3 STATE-V L32R,  A3 A5 SYS+,  A4 A3 0 S32I,
  A3 HANDLER-V L32R,  A3 A5 SYS+,  A4 A3 0 S32I,
  A3 MISC0 RSR,  A4 A3 V-INISR S32I,
  A3 HOLD-V L32R,
  A3 L1 BEQZ,
  A3 >CHIP,  A4 A3 0 S32I,
LABEL L1
  W W A3 A4 +BASE,
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
\
\ FLASH-SYSCALL is SYSCALL for the functions of the ROM that read, write
\ or erase the flash (package FLASH). The flash and the cache share the
\ SPI of the memory, and the frozen part of the dictionary lies in the
\ flash: so the cache is off during the call, as in ESP-IDF
\ (spi_flash_disable_cache), and no interrupt of level 1 or 2 comes
\ (a CODE handler could read the flash). Level 3, the console, stays: its
\ handler reads only the RAM. After the call the cache is flushed (the
\ flash may have new bytes) and on again.

100 CODE SYSCALL   ( x1 .. xn n a -- x )   \ the function at a with n arguments, n <= 13
  A0 0 MOVI.N,  ROM-CALL J,
105 CODE FLASH-SYSCALL   ( x1 .. xn n a -- x )   \ as SYSCALL, with the cache of the flash off
  A0 2 RSIL,                        \ a0: PS before (not 0)
LABEL ROM-CALL
  A7 SP 0 L32I.N,                   \ a
  A8 SP 4 L32I.N,                   \ n
  SP SP 8 ADDI.N,
  A9 MISC0 RSR,
  SP A9 V-SP S32I,  RP A9 V-RP S32I,   \ for an exception in the ROM
  A2 1 MOVI.N,  A2 A9 V-INROM S32I,
  A2 MEM MOV.N,  A3 TBL MOV.N,  A4 IP MOV.N,  A6 RP MOV.N,
  A5 A8 SP ADDX4,                   \ the data stack without the arguments
  A9 A5 -24 ADDI,                   \ arguments 1 to 6 at a9 + 20 ... a9 + 0
  A0 L6 BEQZ,                       \ FLASH-SYSCALL: the cache off; first wait until it rests
  A10 CACHE-DBUG L32R,
LABEL L4
  A11 A10 0 L32I,  A11 A11 7 12 EXTUI,  A11 1 L4 BNEI,
  A10 CACHE-CTRL L32R,  A11 A10 0 L32I,  A12 -9 MOVI,  A11 A11 A12 AND,
  MEMW,  A11 A10 0 S32I,            \ PRO_CACHE_ENABLE 0
LABEL L6
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
  A0 L5 BEQZ,
  A3 A0 MOV.N,  CACHE-ON CALL0,  A3 PS WSR,  RSYNC,      \ FLASH-SYSCALL: the cache on, the interrupts as before
LABEL L5
  A2 MISC0 RSR,  A3 0 MOVI.N,  A3 A2 V-INROM S32I,
  A3 A2 V-CTRLC L32I,  A4 A2 V-INTREQ L32I,  A3 A3 A4 OR,
  A3 L3 BEQZ,
  TBL A2 V-BRK L32I,                \ a break came during the call
LABEL L3
  A9 PUSH,  NEXT,
101 CODE >CHIP   ( a -- a' )         \ the chip address of a VM address; any other stays
  A2 TOS@,  A2 >CHIP,  A2 TOS!,  NEXT,
102 CODE +INT   ( -- )              \ the handlers on; -21 without the package INTR
  A2 XT-ISR L32R,
  A2 L1 BEQZ,
  A2 MISC0 RSR,  A3 1 MOVI.N,  A3 A2 V-INTON S32I,
  A2 A2 V-INTMASK L32I,  A3 0 MOVI.N,  INTEN CALL0,
  NEXT,
LABEL L1
  A2 -21 MOVI,  KTHROW J,
103 CODE -INT   ( -- )              \ the handlers off
  A2 MISC0 RSR,  HANDLERS-OFF CALL0,
  NEXT,
104 CODE CODE-INT!   ( xt n -- )    \ the CODE word xt handles CPU interrupt n (19 to 21); xt 0: none
  A3 POP,  A2 POP,
  A4 A3 -19 ADDI,  A5 3 MOVI.N,  A4 A5 L2 BLTU,
  A2 -24 MOVI,  KTHROW J,
LABEL L2
  A5 1 MOVI.N,  A3 SSL,  A5 A5 SLL,         \ its bit in INTENABLE
  A6 MISC0 RSR,  A7 A4 A6 ADDX4,
  A8 A6 V-INTMASK L32I,  A3 0 MOVI.N,
  A2 L1 BEQZ,
  A2 A2 A9 A0 +BASE,  A2 A2 4 L32I,         \ its machine code: the address after the code field (DOCODE)
  A2 A7 V-CODE S32I,
  A8 A8 A5 OR,  A8 A6 V-INTMASK S32I,
  A2 0 MOVI.N,
  A9 A6 V-INTON L32I,  A2 A5 A9 MOVNEZ,     \ on now, if the handlers are on
  INTEN CALL0,  NEXT,
LABEL L1
  A2 A7 V-CODE S32I,                        \ xt 0: none, CPU interrupt n off
  A8 A8 A5 OR,  A8 A8 A5 XOR,  A8 A6 V-INTMASK S32I,
  A3 A5 MOV.N,  INTEN CALL0,  NEXT,
106 CODE THIS-CORE   ( -- i )       \ the number of this core: bit 13 of PRID (see "Cores")
  A2 PRID RSR,  A2 A2 13 1 EXTUI,  A2 PUSH,  NEXT,
107 CODE CORE-ENTRY   ( xt i -- a ) \ core i (1 to #CORES - 1) runs xt after its start; a: START1, for the chip
  A3 POP,
  A4 A3 -1 ADDI.N,  A5 #CORES 1- MOVI,  A4 A5 L1 BGEU,
  A2 TOS@,
  A4 CORE-TAB-A L32R,  A5 A3 5 SLLI,  A4 A4 A5 ADD.N,
  A4 A4 0 L32I,  A2 A4 V-XT S32I,          \ V-XT in the variables of core i
  A2 START1-A L32R,  A2 TOS!,  NEXT,
LABEL L1
  A2 -24 MOVI,  KTHROW J,
108 CODE CORE-PARK   ( -- )         \ this core waits for its reset; it runs no NEXT any more
LABEL L1
  15 WAITI,  L1 J,

\ ==== CODE words ==========================================================
\ The ESP32 runs no code in the RAM for data, where the dictionary is. The
\ machine code of a CODE word lies in SRAM0, after the code of this HAL
\ (lib/esp32/esp32/asm.fs), and the cell after its code field holds its
\ address there. So this primitive 66 replaces the one of the kernel,
\ which runs the code after the code field.

66 CODE DOCODE
  A2 W 4 L32I.N,  A2 JX,

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
  A2 MISC0 RSR,
  TBL A2 V-TBL L32I,
  A3 A2 V-CTRLC L32I,
  A3 3 L3 BNEI,
  L5 J,                             \ Ctrl-C while one waited: at once
LABEL L3
  A3 A2 V-INTREQ L32I,
  A3 L2 BEQZ,
  A3 0 MOVI.N,  A3 A2 V-INTREQ S32I,
  A3 A2 V-INTON L32I,
  A3 L2 BEQZ,
  A3 IP A4 A5 -BASE,  RP RP -4 ADDI,  A3 RP 0 S32I.N,   \ IP and W for HALT
  A3 W A4 A5 -BASE,   RP RP -4 ADDI,  A3 RP 0 S32I.N,
  A3 HALT-V L32R,  IP A3 MEM ADD.N,  IP IP 4 ADDI.N,
  A3 1 MOVI.N,  A3 A2 V-INISR S32I,                 \ a handler runs
  W XT-ISR L32R,  W W A3 A4 +BASE,
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
  A3 A2 V-ASK L32I,  A3 L7 BNEZ,             \ another core: nothing on the console
  A2 10 MOVI,  TX CALL0,                     \ a new line: column 0
  A6 2 L7 BLTI,
  A2 MISC0 RSR,  A3 0 MOVI.N,  A3 A2 V-INTREQ S32I,
  HANDLERS-OFF CALL0,                        \ twice: the handlers off
  A7 OFF-TEXT-A L32R,
LABEL L8                                     \ "  handlers off", a new line: a byte in each cell
  A2 A7 0 L32I,
  A2 L7 BEQZ,
  TX CALL0,
  A7 A7 4 ADDI.N,
  L8 J,
LABEL L7
  A2 0 MOVI.N,  A3 OUT-V L32R,  A3 A4 SYS+,  A2 A3 0 S32I,
  A3 HANDLER-V L32R,  A3 A4 SYS+,  A3 A3 0 L32I,
  A3 L9 BEQZ,
  A2 -28 MOVI,  KTHROW J,
LABEL L9
  A2 0 MOVI.N,  RESTART J,                   \ no CATCH: QUIT

127 CODE HALT   \ the end of (ISR): IP and W from the return stack
  W RP 0 L32I.N,  IP RP 4 L32I.N,  RP RP 8 ADDI.N,
  W W A2 A3 +BASE,  IP IP A2 A3 +BASE,
  A2 MISC0 RSR,  A3 0 MOVI.N,  A3 A2 V-INISR S32I,
  A3 A2 V-CTRLC L32I,
  A3 L1 BEQZ,
  TBL A2 V-BRK L32I,                \ a Ctrl-C waited for the end of the handlers
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
\ The stack: a1 = ROM-SP0, at the top of SRAM1; it grows down to the
\ buffers of ESP32-RADIO (16 KB below). A call of the ROM with CALLX8 can spill the
\ window of the VM onto it (window overflow 8). Then the cell at a1 - 12
\ must hold the stack pointer p of a caller, and a4 to a7 go to the 16
\ bytes at p - 32. START sets p = a1 + 32, so they go to a1 to a1 + 15.
\ The same state after an exception (RECOVER). The state of each core
\ comes from VM-STATE (see "Cores").

TALIGN LABEL START
  A2 0 MOVI.N,
  A2 INTENABLE WSR,  RSYNC,         \ no interrupts
  A3 RTC-CNTL L32R,                 \ the RTC watchdog off (RTC_CNTL_WDTWPROTECT, _WDTCONFIG0)
  A4 WDT-KEY L32R,
  A4 A3 $A4 S32I,  A2 A3 $8C S32I,  A2 A3 $A4 S32I,
  A3 TIMG0 L32R,                    \ the watchdogs of timer group 0 and 1 off
  A4 A3 $64 S32I,  A2 A3 $48 S32I,  A2 A3 $64 S32I,
  A5 TG0-1MHZ L32R,  A5 A3 0 S32I,  \ timer 0 of group 0 runs: the clock of the HAL
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
  A2 CORE-TAB-A L32R,  A1 A2 12 L32I,              \ a1: ROM-SP0, for the calls of the ROM below
  A2 A1 32 ADDI,  A3 A1 -16 ADDI,  A2 A3 4 S32I,   \ a1 - 12: a1 + 32
  \ The cache of the flash for the frozen part of the dictionary (the
  \ README, "Memory"), as the bootloader of ESP-IDF does it: the cache
  \ off; SPI0 as after a start from the flash (DIO and 40 MHz, as the
  \ header of the image says, and it serves the cache: after "ucf run"
  \ the download mode of the ROM leaves it otherwise); the MMU of
  \ PRO_CPU empty, DROM pages 1 to 3 (VM addresses $10000 to $3FFFF) on
  \ the flash from DICT-FLASH + $10000, DROM0 not masked, flushed, on.
  A2 CACHE-CTRL L32R,  A3 A2 0 L32I,  A4 -9 MOVI,  A3 A3 A4 AND,  MEMW,  A3 A2 0 S32I,
  A10 2 MOVI.N,  A8 READ-MODE L32R,  A8 CALLX8,
  A10 2 MOVI.N,  A11 0 MOVI.N,  A8 SPI-CLOCK L32R,  A8 CALLX8,
  A2 SPI0-CACHE L32R,  A3 A2 0 L32I,  A4 1 MOVI.N,  A3 A3 A4 OR,  MEMW,  A3 A2 0 S32I,
  A10 0 MOVI.N,  A8 MMU-INIT L32R,  A8 CALLX8,
  A10 0 MOVI.N,  A11 0 MOVI.N,  A12 DROM-PAGE L32R,  A13 DICT-PAGE L32R,
  A14 64 MOVI,  A15 3 MOVI.N,  A8 MMU-SET L32R,  A8 CALLX8,
  A2 CACHE-MASK L32R,  A3 A2 0 L32I,  A4 -17 MOVI,  A3 A3 A4 AND,  MEMW,  A3 A2 0 S32I,
  CACHE-ON CALL0,
  A2 CORE-TAB-A L32R,  VM-STATE CALL0,             \ core 0: row 0 of the table of the cores
  A2 HALT-V L32R,  A3 A2 MEM ADD.N,
  A4 127 MOVI,  A4 A3 0 S32I,  A2 A3 4 S32I,        \ the HALT cells
  A3 UART0 L32R,                    \ UART0: an interrupt for more than 1 byte, or after a rest
  A4 A3 $24 L32I,  A5 CONF1-KEEP L32R,  A4 A4 A5 AND,
  A5 CONF1-RX L32R,  A4 A4 A5 OR,  A4 A3 $24 S32I,  \ UART_CONF1
  A4 $101 MOVI,  A4 A3 $10 S32I,  A4 A3 $0C S32I,  \ UART_INT_CLR, UART_INT_ENA: RXFIFO_FULL, RXFIFO_TOUT
  A3 UART0-MAP L32R,  A4 23 MOVI,  A4 A3 0 S32I,    \ UART0 to CPU interrupt 23 (level 3)
  MEMW,
  A2 BIT23 L32R,  A2 INTENABLE WSR,  RSYNC,         \ CPU interrupt 23 on
  A2 1 MOVI.N,  RESTART J,          \ BOOT

\ ==== Cores ===============================================================
\ Each core of the chip runs the VM with its own state (016 §7): its
\ variables of the HAL (register MISC0), its SYS and RAM data of the
\ base image (MISC1: VM addresses below PE), its TIB, PAD and stacks
\ (MISC2), its two tables for TBL and its stack for the functions of
\ the ROM; MISC3 holds PE (asm/xtensa.fs, PER-CORE?). The dictionary
\ and the other RAM data are the same for all cores. CORE-TAB has a row
\ of 8 cells for each core: the address of its variables of the HAL,
\ the bases for MISC1 and MISC2, its a1, its tables, its clock (V-TIMER),
\ V-ASK and MISC3. Core 0 is the console; its MISC3 is $10000, as its
\ own part and the common part of the RAM data lie at one place (one
\ jump less for each cell there). Core 1 (targets/esp32/README.md,
\ "Memory"): its variables and tables at $3FFE_5400 (512 bytes each),
\ its stack from $3FFE_7FC0 down; its SYS and RAM data of the base image
\ in the gap of SRAM2 (VM address $100 at $3FFA_E800, up to PE), its
\ TIB, PAD and stacks at $3FFA_F600 to $3FFA_FFFF; its clock is timer
\ 1 of timer group 0. The ROM of core 1 writes $3FFE_3F20 to $3FFE_522F
\ and the bytes above $3FFE_7DC0 before START1 runs (Befund 107).
\
\ START1 is the start of core i, 1 to #CORES - 1. Core 0 prepares it
\ (package MULTICORE, lib/esp32/cores.fs): CORE-ENTRY (HAL 107) puts the
\ xt for core i into its V-XT and gives the address of START1; the chip
\ starts core i there (its ROM calls START1 with CALLX8). START1 sets the
\ state of the core (VM-STATE), copies SYS and the RAM data of the base
\ image of core 0 into its own (VM addresses $100 up to PE), switches on
\ CPU interrupt 23 for the requests of core 0 (LEVEL3), and runs the xt
\ on empty stacks (RESTART-W). THIS-CORE (HAL 106) gives the number of
\ the core, CORE-PARK (HAL 108) lets the core wait for its reset.

TALIGN LABEL CORE-TAB   \ for each core: VARS, MISC1, MISC2, a1, the tables, V-TIMER, V-ASK, MISC3
  VARS-ADDR T,   DATA-ADDR T,   RAM-ADDR T,      ROM-SP0 T,  TABLES-ADDR T,   $3FF5F000 T,  0 T,           $10000 T,
  VARS1-ADDR T,  DATA1-ADDR T,  STACKS1-ADDR T,  ROM-SP1 T,  TABLES1-ADDR T,  $3FF5F024 T,  $3FF000E0 T,   0 T,
: PE! ( pe -- )   \ build.fs: PE for START1 and MISC3 of core 1
  DUP PE-V T!  CORE-TAB 32 + 28 + T! ;

TALIGN LABEL VM-STATE   \ a2: the row of this core in CORE-TAB; its state, MEM and TBL; uses a0, a2 to a7 (CALL0)
  A7 A0 MOV.N,                      \ the return address: COPY-TABLE needs a0
  A3 A2 0 L32I,  A3 MISC0 WSR,      \ the variables of the HAL of this core
  A4 0 MOVI.N,
  A4 A3 V-HEAD S32I,  A4 A3 V-TAIL S32I,  A4 A3 V-CTRLC S32I,
  A4 A3 V-INTREQ S32I,  A4 A3 V-INTON S32I,  A4 A3 V-INROM S32I,
  A4 A3 V-CODE S32I,  A4 A3 V-CODE 4 + S32I,  A4 A3 V-CODE 8 + S32I,
  A4 A3 V-INISR S32I,
  A4 2 MOVI.N,  A4 A3 V-INTMASK S32I,     \ CPU interrupt 1
  A4 126 MOVI,  A4 A3 V-WAIT S32I,        \ the code field for WAIT-UNTIL
  A4 -1000 MOVI,  A4 A3 V-LAST S32I,      \ the last Ctrl-C: long ago
  A4 A2 12 L32I,  A4 A3 V-ROMSP S32I,
  A4 A2 20 L32I,  A4 A3 V-TIMER S32I,
  A4 A2 24 L32I,  A4 A3 V-ASK S32I,
  A4 FREGS-A L32R,                        \ FAULT-REGS: in the RAM data of the base image, so each core has its own
  A4 L1 BEQZ,
  A5 CORE-TAB-A L32R,  A5 A5 4 L32I,  A4 A4 A5 SUB,  A5 A2 4 L32I,  A4 A4 A5 ADD.N,
LABEL L1
  A4 A3 V-FREGS S32I,
  A4 A2 4 L32I,  A4 MISC1 WSR,            \ SYS and the RAM data of the base image
  A4 A2 8 L32I,  A4 MISC2 WSR,            \ TIB, PAD and the stacks
  A4 A2 28 L32I,  A4 MISC3 WSR,           \ PE, or $10000 (core 0)
  A1 A2 12 L32I,                          \ the stack for the functions of the ROM (see START)
  A4 A1 32 ADDI,  A5 A1 -16 ADDI,  A4 A5 4 S32I,   \ a1 - 12: a1 + 32
  A6 A2 16 L32I,  A6 A3 V-TBL S32I,       \ the two tables (see COPY-TABLE)
  A4 A6 512 ADDMI,  A4 A3 V-BRK S32I,
  A2 TABLE-A L32R,  A3 A6 MOV.N,  COPY-TABLE CALL0,
  A2 BRKTAB-A L32R,  COPY-TABLE CALL0,
  A3 A6 512 ADDMI,                        \ the break table: slots 120 to 122 of the table
  A4 A6 FE-SLOT L32I,  A4 A3 FE-SLOT S32I,
  A4 A6 FM-SLOT L32I,  A4 A3 FM-SLOT S32I,
  A4 A6 S0-SLOT L32I,  A4 A3 S0-SLOT S32I,
  MEM VM-BASE L32R,  TBL A6 MOV.N,
  A0 A7 MOV.N,  RET,

TALIGN LABEL START1   \ core i, 1 to #CORES - 1: its ROM calls it with CALLX8, in its window
  A2 0 MOVI.N,  A2 INTENABLE WSR,  RSYNC,         \ no interrupts
  A2 0 MOVI.N,  A2 WINDOWBASE WSR,  RSYNC,        \ as START: one register window
  A2 1 MOVI.N,  A2 WINDOWSTART WSR,  RSYNC,
  A2 PS-VM L32R,  A2 PS WSR,  RSYNC,
  A2 VECTORS-A L32R,  A2 VECBASE WSR,  RSYNC,     \ the vectors of this HAL
  A2 PRID RSR,  A2 A2 13 1 EXTUI,                 \ the row of this core
  A2 A2 5 SLLI,  A3 CORE-TAB-A L32R,  A2 A2 A3 ADD.N,
  VM-STATE CALL0,
  A3 CORE-TAB-A L32R,  A3 A3 4 L32I,              \ SYS and the RAM data of the base image of core 0
  A4 MISC1 RSR,  A5 PE-V L32R,  A6 256 MOVI,      \ to this core: VM addresses $100 up to PE
LABEL L1
  A7 A3 A6 ADD.N,  A7 A7 0 L32I,  A8 A4 A6 ADD.N,  A7 A8 0 S32I,
  A6 A6 4 ADDI.N,  A6 A5 L1 BLTU,
  MEMW,
  A2 BIT23 L32R,  A2 INTENABLE WSR,  RSYNC,       \ CPU interrupt 23: the requests of core 0 (LEVEL3)
  A2 MISC0 RSR,  W A2 V-XT L32I,
  RESTART-W J,

\ ==== Data: the break table, the variables ================================

TALIGN LABEL BRKTAB   128 CELLS TDP +!     \ BREAK for each number (set at the end)
: TEXT, ( a u -- )   0 ?DO  DUP I + C@ T,  LOOP  DROP ;   \ a byte in each cell: the CPU reads cells here
LABEL OFF-TEXT   S"   handlers off" TEXT,  10 T,  0 T,   \ after Ctrl-C twice; 0 ends it

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

LABEL LEVEL1   \ CPU interrupt 1: a source for the Forth handlers; off until HALT. CPU interrupt 6: see WAIT-UNTIL
  A2 MISC0 RSR,
  A0 A2 V-SAVE S32I,  A3 A2 V-SAVE 4 + S32I,  A4 A2 V-SAVE 8 + S32I,  A5 A2 V-SAVE 12 + S32I,
  A3 INTERRUPT RSR,  A4 INTENABLE RSR,  A3 A3 A4 AND,
  A3 6 L2 BBCI,                     \ the CPU timer of a sleep in MS: off; a write of CCOMPARE0 clears it
  A5 -65 MOVI,  A4 A4 A5 AND,  A4 INTENABLE WSR,
  A5 CCOMPARE0 RSR,  A5 CCOMPARE0 WSR,
LABEL L2
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
  A2 MISC0 RSR,
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
  A2 MISC0 RSR,
  A3 A2 V-SAVE 32 + L32I,  A3 SAR WSR,
  A0 A2 V-SAVE L32I,       A3 A2 V-SAVE 4 + L32I,   A4 A2 V-SAVE 8 + L32I,
  A5 A2 V-SAVE 12 + L32I,  A6 A2 V-SAVE 16 + L32I,  A7 A2 V-SAVE 20 + L32I,
  A8 A2 V-SAVE 24 + L32I,  A9 A2 V-SAVE 28 + L32I,
  A2 EXCSAVE2 RSR,
  2 RFI,

LABEL LEVEL3   \ CPU interrupt 23. Core 0: UART0, the bytes into RX; Ctrl-C twice switches the handlers off at once
  \ Another core: core 0 asks for a break (FROM_CPU, V-ASK): V-CTRLC 1, or 3 when one waits already
  A2 MISC0 RSR,
  A0 A2 V-SAVE S32I,       A3 A2 V-SAVE 4 + S32I,   A4 A2 V-SAVE 8 + S32I,
  A5 A2 V-SAVE 12 + S32I,  A6 A2 V-SAVE 16 + S32I,  A7 A2 V-SAVE 20 + S32I,
  A8 A2 V-SAVE 24 + S32I,
  A4 A2 V-ASK L32I,
  A4 L2 BEQZ,                                \ core 0: the console
  A5 0 MOVI.N,  MEMW,  A5 A4 0 S32I,         \ the request is taken
  A4 A2 V-CTRLC L32I,  A5 1 MOVI.N,  A6 3 MOVI.N,  A5 A6 A4 MOVNEZ,  A5 A2 V-CTRLC S32I,
  L1 J,
LABEL L2
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
  A2 MISC0 RSR,  A2 A2 V-FREGS L32I,  \ FAULT-REGS of this core
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
  A2 MISC0 RSR,
  A3 EXCCAUSE RSR,  A3 A2 V-CAUSE S32I,
  A3 EPC1 RSR,
  FAULT-GO J,
LABEL DOUBLE   \ an error in a handler, or in FAULT: the handlers off; a2 is in EXCSAVE1
  DEPC EXCSAVE1 FALSE SAVE-REGS,
  A2 MISC0 RSR,
  A3 EXCCAUSE RSR,  A3 A2 V-CAUSE S32I,
  A3 0 MOVI.N,  A3 A2 V-INTON S32I,
  A3 A2 V-INTMASK L32I,  A4 INTENABLE RSR,  A4 A4 A3 OR,  A4 A4 A3 XOR,  A4 INTENABLE WSR,
  A3 DEPC RSR,
  FAULT-GO J,
LABEL DEBUG   \ the cause: 64 (EXCCAUSE has no number for it); a2 is in EXCSAVE6
  A2 MISC0 RSR,  A2 A2 V-FREGS L32I,       \ DEBUG-HOOK of INTR (FAULT-REGS + 108): the monitor
  A2 L7 BEQZ,
  A2 A2 108 L32I,
  A2 L7 BEQZ,
  A2 JX,                                   \ to the hook: a2 in EXCSAVE6, all else as at the exception
LABEL L7
  EPC6 EXCSAVE6 TRUE SAVE-REGS,
  A2 MISC0 RSR,
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
  A2 MISC0 RSR,
  A1 A2 V-ROMSP L32I,
  MEM VM-BASE L32R,
  A5 MISC2 RSR,                     \ the base of the stacks of this core
  SP A2 V-SP L32I,                  \ the data stack: R0 to S0 (doc/vm.md 3)
  A3 SP A5 SUB,  A4 R0-V L32R,  A3 A3 A4 SUB,
  A4 $401 MOVI,  A3 A4 L1 BLTU,
  A3 S0-V L32R,  SP A3 A5 ADD.N,
LABEL L1
  RP A2 V-RP L32I,                  \ the return stack: R0 - 1024 to R0
  A3 RP A5 SUB,  A4 R0-V L32R,  A3 A3 A4 SUB,  A3 A3 1024 ADDMI,
  A4 $401 MOVI,  A3 A4 L2 BLTU,
  A3 R0-V L32R,  RP A3 A5 ADD.N,
LABEL L2
  TBL A2 V-TBL L32I,
  A3 A2 V-CTRLC L32I,  A4 A2 V-INTREQ L32I,  A3 A3 A4 OR,
  A3 L3 BEQZ,
  TBL A2 V-BRK L32I,                \ a break came meanwhile
LABEL L3
  W XT-FAULT L32R,
  W L4 BEQZ,
  A3 A2 V-ADDR L32I,  A3 PUSH,  A3 A2 V-PC L32I,  A3 PUSH,  A3 A2 V-CAUSE L32I,  A3 PUSH,
  W W A3 A4 +BASE,  DISPATCH,
LABEL L4
  A3 HANDLER-V L32R,  A3 A4 SYS+,  A3 A3 0 L32I,
  A3 L5 BEQZ,
  A2 -9 MOVI,  KTHROW J,
LABEL L5
  A2 0 MOVI.N,  RESTART J,

\ ==== The addresses that the code above needs ===========================

: ROOM? ( -- )   THERE $400A0000 U< 0= ABORT" the code does not fit into SRAM0" ;
ROOM?

BRKTAB >DRAM - BRKTAB-A T!
VECTORS VECTORS-A T!
RECOVER RECOVER-A T!
CORE-TAB CORE-TAB-A T!
START1 START1-A T!
OFF-TEXT OFF-TEXT-A T!
: FILL-BRKTAB ( -- )   \ BREAK for each number (START puts slots 120 to 122 of the table into the copy)
  128 0 DO  BREAK BRKTAB I CELLS + T!  LOOP ;
FILL-BRKTAB
