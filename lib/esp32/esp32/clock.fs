\ clock.fs - package CLOCK: the clock of the CPU of the ESP32
\
\ After a reset the CPU runs at 40 MHz from the crystal (XTAL), and the
\ APB clock is 40 MHz too. The PLL gives 80 or 160 MHz (the PLL at 320
\ MHz) or 240 MHz (the PLL at 480 MHz); the APB clock is then 80 MHz. The
\ steps are those of ESP-IDF (components/esp_hw_support/port/esp32/
\ rtc_clk.c). Each change goes to the crystal first:
\
\ 1. The PLL on, at 320 or 480 MHz. Its analog registers are behind an
\    internal I2C bus; the ROM writes them (rom_i2c_writeReg, over SYSCALL).
\ 2. For 240 MHz a higher core voltage (1.25 V less the value in the
\    eFuse; 1.1 V else), before the faster clock and after a slower one.
\    Then the CPU clock comes from the PLL, and the ROM gets the new
\    clock (for its own waits).
\ 3. Unlike on the ESP32-S3, UART0 and the timer groups count the APB
\    clock: a new APB clock gets UART0 a new divider (115200 baud) and
\    the timer of the HAL (timer 0 of group 0) and timer 0 of group 1
\    (package TIMERS) new dividers for 1 MHz. APB-MHZ (package ESP32)
\    holds the APB clock. The SPI flash runs at the APB clock / 2:
\    40 MHz with the PLL.
\
\   n MHZ ( -- )       the CPU clock: 40, 80, 160 or 240
\   CPU-MHZ ( -- n )   the CPU clock now
\
\ On the board, in ucf term:  #include lib/esp32/esp32/clock.fs

PACKAGE CLOCK  ALSO ROM  ALSO CHIP  ALSO GPIO   \ lib/package.fs

$3FF48000 CONSTANT OPTIONS0         \ RTC_CNTL_OPTIONS0: bits 6, 8, 10, 18 power the PLL down
$3FF4807C CONSTANT RTC-REG          \ RTC_CNTL_REG: DIG_DBIAS_WAK, bits 13-11
$3FF48070 CONSTANT CLK-CONF         \ RTC_CNTL_CLK_CONF: SOC_CLK_SEL, bits 28-27 (0 crystal, 1 PLL)
$3FF0003C CONSTANT PER-CONF         \ DPORT_CPU_PER_CONF: CPUPERIOD_SEL (0 80 MHz, 1 160, 2 240)
$400041A4 CONSTANT 'ANA!            \ rom_i2c_writeReg ( block host reg data -- )
$40008550 CONSTANT 'TICKS/US        \ ets_update_cpu_frequency_rom ( mhz -- )
$3FF40000 CONSTANT UART0            \ CLKDIV at $14, STATUS at $1C
$3FF5F000 CONSTANT TG0              \ timer 0 of timer group 0: the clock of the HAL
4 CONSTANT 1V10                     \ DIG_DBIAS: 1.1 V

: BBPLL! ( reg data -- )   \ a register of the PLL (I2C block $66, host 4)
  >R >R  $66 4 R> R>  4 'ANA! SYSCALL DROP ;
: 1V25 ( -- n )   \ DIG_DBIAS for 240 MHz: 1.25 V less VOL_LEVEL_HP_INV of the eFuse
  7  $3FF5A014 @ 22 RSHIFT 3 AND - ;
: DBIAS! ( n -- )   11 LSHIFT  $3800 RTC-REG BITS! ;
: PLL-ON ( mhz -- )   \ the PLL at 480 or 320 MHz from the crystal of 40 MHz
  $40540 OPTIONS0 BIC!
  0 $18 BBPLL!  1 $20 BBPLL!  4 $9A BBPLL!  10 0 BBPLL!  12 0 BBPLL!   \ reset its configuration
  480 = IF    1V25 DBIAS!  3 US  11 $C3 BBPLL!  9 $74 BBPLL!  28   \ ENDIV5, BBADC_DSMP; divider 28
        ELSE  1V10 DBIAS!        11 $43 BBPLL!  9 $84 BBPLL!  32  THEN
  >R  2 0 BBPLL!  3 R> BBPLL!  5 $C6 BBPLL!                           \ OC_LREF, OC_DIV_7_0, OC_DCUR
  80 US ;
: PLL? ( -- f )   CLK-CONF @ 27 RSHIFT 3 AND 1 = ;
: WAIT-TX ( -- )   \ until UART0 has sent all its bytes: TXFIFO_CNT and the state 0
  BEGIN  UART0 $1C + @  $0FFF0000 AND 0=  UNTIL ;
: UART-DIV ( hz -- )   \ UART_CLKDIV for 115200 baud: hz x 16 / 115200; integer bits 19-0, fraction 23-20
  16 * 115200 /  DUP 4 RSHIFT  SWAP 15 AND 20 LSHIFT OR  UART0 $14 + ! ;
: REDIVIDE ( mhz a -- )   \ the timer at a counts 1 MHz again; its count stays
  DUP @ >R                                       ( mhz a )  ( R: its configuration )
  R@ $7FFFFFFF AND OVER !                        \ EN off: the divider may change
  SWAP 13 LSHIFT  R> $1FFFE000 INVERT AND OR  SWAP ! ;   \ the new divider; EN as before
: APB! ( mhz -- )   \ the APB clock is now mhz
  DUP TO APB-MHZ
  DUP 1000000 * UART-DIV
  DUP TG0 REDIVIDE
  TG1 REDIVIDE ;
: PERIOD ( mhz -- n )   DUP 80 = IF DROP 0 EXIT THEN  160 = IF 1 ELSE 2 THEN ;

: CRYSTAL ( -- )   \ the CPU from the crystal; then the lower voltage
  0 $18000000 CLK-CONF BITS!  1V10 DBIAS!  40 APB! ;

PUBLIC
: CPU-MHZ ( -- n )
  PLL? 0= IF 40 EXIT THEN
  PER-CONF @ 3 AND  DUP 0= IF DROP 80 EXIT THEN  1 = IF 160 ELSE 240 THEN ;
: MHZ ( n -- )   \ 40, 80, 160 or 240
  DUP 40 = OVER 80 = OR OVER 160 = OR OVER 240 = OR 0= ABORT" CLOCK: 40, 80, 160 or 240 MHz"
  WAIT-TX  CRYSTAL
  DUP 40 <> IF
    DUP 240 = IF 480 ELSE 320 THEN PLL-ON              \ the voltage for the clock comes with it
    DUP PERIOD PER-CONF !
    $08000000 $18000000 CLK-CONF BITS!                  \ the CPU from the PLL
    80 APB!
  THEN
  1 'TICKS/US SYSCALL DROP ;

END-PACKAGE
