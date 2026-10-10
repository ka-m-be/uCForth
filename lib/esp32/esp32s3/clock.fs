\ clock.fs - package CLOCK: the clock of the CPU of the ESP32-S3
\
\ After a reset the CPU runs at 40 MHz from the crystal (XTAL). The PLL
\ gives 80, 160 or 240 MHz. The steps are those of ESP-IDF
\ (components/esp_hw_support/port/esp32s3/rtc_clk.c):
\
\ 1. The PLL on, at 480 MHz. Its analog registers are behind an internal
\    I2C bus; the ROM writes them (rom_i2c_writeReg, over SYSCALL).
\ 2. For 240 MHz a higher core voltage (28 instead of 27). Then the CPU
\    clock comes from the PLL, and the ROM gets the new clock (for its
\    own waits).
\ 3. The SPI flash stays at 40 MHz or slower: its clock comes from the
\    PLL (80 MHz), divided by 2 (SPI_MEM_CLOCK_REG of SPI1 and SPI0).
\    The divider changes before the switch to the PLL, and after the
\    switch back to the crystal: so the flash never gets 80 MHz.
\
\ UART0 and the SYSTIMER run from the crystal: the console and MS do
\ not change. The APB clock is 80 MHz with the PLL, else the CPU clock.
\
\   n MHZ ( -- )       the CPU clock: 40, 80, 160 or 240
\   CPU-MHZ ( -- n )   the CPU clock now
\
\ On the board, in ucf term:  #include lib/esp32/esp32s3/clock.fs

PACKAGE CLOCK  ALSO ROM  ALSO GPIO   \ lib/package.fs

$40005D60 CONSTANT 'ANA!            \ rom_i2c_writeReg ( block host reg data -- )
$40005D6C CONSTANT 'ANA-BITS!       \ rom_i2c_writeReg_Mask ( block host reg msb lsb data -- )
$4000069C CONSTANT 'TX-IDLE         \ uart_tx_wait_idle ( uart -- )
$40001A4C CONSTANT 'TICKS/US        \ ets_update_cpu_frequency ( mhz -- )

$600C0010 CONSTANT CPU-PER-CONF     \ bit 2: PLL at 480 MHz; bits 1-0: 80, 160, 240 MHz
$600C0060 CONSTANT SYSCLK-CONF      \ bits 11-10: 0 XTAL, 1 PLL; bits 9-0: divider - 1
$60008000 CONSTANT RTC-OPTIONS0     \ bits 6, 8, 10: the PLL and its I2C powered down
$600081FC CONSTANT RTC-DATE         \ bits 18-13: LDO slaves powered down
$6000E040 CONSTANT ANA-CONF0        \ bits 2, 3: stop the PLL calibration; bit 24: done
$6000E044 CONSTANT ANA-CONFIG       \ bits 17-8: the slaves on the analog I2C bus off
$66 CONSTANT BBPLL                  \ blocks on the analog I2C bus, both with host 1
$6D CONSTANT DIG-REG
$60002014 CONSTANT SPI1-CLOCK       \ SPI_MEM_CLOCK_REG of SPI1 (the flash)
$60003014 CONSTANT SPI0-CLOCK       \ and of SPI0 (the cache): bit 31 = not divided
VARIABLE OLD                        \ the clock before the change

: ANA! ( block reg data -- )   >R >R 1 R> R>  4 'ANA! SYSCALL DROP ;
: ANA-BITS! ( block reg msb lsb data -- )   >R >R >R >R 1 R> R> R> R>  6 'ANA-BITS! SYSCALL DROP ;
: ROM-MHZ ( mhz -- )   1 'TICKS/US SYSCALL DROP ;

: PLL-ON ( -- )   \ the PLL at 480 MHz, from the 40-MHz crystal
  $3FF00 ANA-CONFIG BIS!  $20000 ANA-CONFIG BIC!   \ all analog I2C slaves off, then the PLL on
  $540 RTC-OPTIONS0 BIC!                          \ power up the PLL and its I2C
  4 CPU-PER-CONF BIS!                             \ PLL_FREQ_SEL: 480 MHz
  4 ANA-CONF0 BIC!  8 ANA-CONF0 BIS!              \ start the calibration
  BBPLL 4 $6B ANA!                                \ MODE_HF
  BBPLL 2 $50 ANA!                                \ DCHGP 5, DIV_REF 0
  BBPLL 3 8 ANA!                                  \ DIV_7_0
  BBPLL 5 2 0 0 ANA-BITS!  BBPLL 5 6 4 0 ANA-BITS!   \ DR1, DR3
  BBPLL 6 $73 ANA!                                \ DLREF_SEL 1, DHREF_SEL 3, DCUR 3
  BBPLL 9 1 0 3 ANA-BITS!                         \ VCO_DBIAS
  BEGIN ANA-CONF0 @ $1000000 AND UNTIL           \ the calibration is done
  10 US  8 ANA-CONF0 BIC!  4 ANA-CONF0 BIS! ;    \ stop it
: VOLTAGE ( dbias -- )   \ the core voltage: 28 for 240 MHz, else 27
  >R  DIG-REG 4 4 0 R@ ANA-BITS!  DIG-REG 6 4 0 R> ANA-BITS!  40 US ;
: SLAVES ( mhz -- )   \ LDO slaves off: 7 for 40 MHz, 3 for 80, 1 for 160, 0 for 240
  80 /  7 SWAP RSHIFT  13 LSHIFT  $7E000 RTC-DATE BITS! ;
: FLASH-DIV ( x -- )   \ the clock of the SPI flash (SPI1) and the cache (SPI0)
  DUP SPI1-CLOCK !  SPI0-CLOCK ! ;
$80000000 CONSTANT /1   $00010001 CONSTANT /2

: >XTAL ( -- )
  40 ROM-MHZ  0 $FFF SYSCLK-CONF BITS!          \ the crystal, not divided
  OLD @ 240 = IF 27 VOLTAGE THEN  40 SLAVES  /1 FLASH-DIV ;
: >PLL ( mhz -- )   \ 80, 160 or 240; up: first the voltage, down: afterwards
  SYSCLK-CONF @ $C00 AND 0= IF PLL-ON THEN
  DUP OLD @ > IF  DUP 240 = IF 28 VOLTAGE THEN  DUP SLAVES  THEN
  /2 FLASH-DIV                                    \ the flash: 20 MHz now, 40 MHz with the PLL
  DUP 80 / 1-  3 CPU-PER-CONF BITS!
  $400 $FFF SYSCLK-CONF BITS!                    \ the PLL, not divided
  DUP ROM-MHZ
  DUP OLD @ < IF  OLD @ 240 = IF 27 VOLTAGE THEN  DUP SLAVES  THEN  DROP ;
: VALID? ( n -- f )   DUP 40 =  OVER 80 = OR  OVER 160 = OR  SWAP 240 = OR ;

PUBLIC
: CPU-MHZ ( -- n )
  SYSCLK-CONF @ 10 RSHIFT 3 AND IF  CPU-PER-CONF @ 3 AND 1+ 80 *
  ELSE  40 SYSCLK-CONF @ $3FF AND 1+ /  THEN ;
: MHZ ( n -- )   \ 40, 80, 160 or 240
  DUP VALID? 0= ABORT" MHZ: 40, 80, 160 or 240"
  CPU-MHZ OLD !  0 1 'TX-IDLE SYSCALL DROP        \ the console has sent all
  DUP 40 = IF DROP >XTAL EXIT THEN  >PLL ;

END-PACKAGE
