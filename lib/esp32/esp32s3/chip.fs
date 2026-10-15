\ chip.fs - package ESP32S3: the layer of the chip ESP32-S3
\
\ The addresses and numbers of the ESP32-S3 that the common packages in
\ lib/esp32/ need (gpio.fs, intr.fs, timers.fs, flash.fs). They write
\ ALSO CHIP and take the values from here; the ESP32 has the same words
\ with its own values (lib/esp32/esp32/chip.fs). The values are from the
\ Technical Reference Manual of the ESP32-S3 (TRM) and the ROM of the
\ chip (esp32s3.rom.ld of ESP-IDF).
\
\   CHIP-NAME ( -- a u )   the name of the chip: ESP32-S3
\   GPIO-BASE IO-MUX       the GPIO unit and the IO MUX (TRM chapter 6)
\   #PINS                  the GPIO numbers are 0 to #PINS - 1
\   pin >MUX ( -- a )      the register of the pin in the IO MUX
\   MUX-GPIO               a pin as GPIO in the IO MUX: function 1, drive 2, input on
\   pin >PULL ( -- a up down )   the register of the pull resistors of the pin
\                          and its bits for up and down (here the IO MUX)
\   PIN-REG IN-SEL OUT-SEL     offsets in the GPIO unit: GPIO_PIN0, the input
\                              of signal 0, the output of pin 0 (4 bytes each)
\   PCPU-INT PCPU-INT1     offsets: the pins 0-31 and 32-48 that ask the CPU
\   PIN-INT-CPU            the bit in GPIO_PINn: the interrupt goes to the CPU
\   GPIO-SOURCE            the interrupt source of the GPIO unit
\   INTR-MAP INTR-STATUS #SOURCES   the interrupt matrix (TRM chapter 9)
\   i CORE-INTR-MAP i CORE-INTR-STATUS ( -- a )   the same of core i
\   TG1 TG1-SOURCE         timer group 1 and the source of its timer 0
\   i CORE-TG1 i CORE-TG1-SOURCE   the timer of TIMERS on core i, its source
\   TG-INT-ENA TG-INT-CLR  offsets of the interrupt registers of a timer group
\   TG-1MHZ ( -- x )       the configuration of a timer: on, up, reload,
\                          alarm, 1 MHz
\   'FLASH-READ ...        functions of the ROM for the SPI flash
\   FLASH-SIZE             the size of the flash of the board
\   EXT-RAM EXT-RAM-END    the PSRAM: the functions of the flash take no
\                          buffer there
\
\ At the end, CHIP ( -- ) gives the package of the chip of this board:
\ a common package writes ALSO CHIP. The name ESP32S3 also tells ucf
\ term which chip the board has (#when, host/README.md).

PACKAGE ESP32S3   \ lib/package.fs

PUBLIC
: CHIP-NAME ( -- a u )   S" ESP32-S3" ;

\ ---- GPIO and IO MUX
$60004000 CONSTANT GPIO-BASE
$60009000 CONSTANT IO-MUX
49 CONSTANT #PINS                     \ GPIO 0 to 48
: >MUX ( pin -- a )   4 * 4 + IO-MUX + ;   \ IO_MUX_GPIOn: in the order of the pins
$1A00 CONSTANT MUX-GPIO               \ function 1 (GPIO), drive 2, input on
: >PULL ( pin -- a up down )   >MUX $100 $80 ;   \ FUN_WPU, FUN_WPD
$74 CONSTANT PIN-REG                  \ GPIO_PIN0_REG
$154 CONSTANT IN-SEL                  \ GPIO_FUNC0_IN_SEL_CFG_REG
$554 CONSTANT OUT-SEL                 \ GPIO_FUNC0_OUT_SEL_CFG_REG
$5C CONSTANT PCPU-INT                 \ GPIO_PCPU_INT_REG: pins 0 to 31
$68 CONSTANT PCPU-INT1                \ GPIO_PCPU_INT1_REG: pins 32 to 48
$2000 CONSTANT PIN-INT-CPU            \ GPIO_PINn: INT_ENA, bit 13
16 CONSTANT GPIO-SOURCE

\ ---- the interrupt matrix of core 0
$600C2000 CONSTANT INTR-MAP           \ a register for each source: its CPU interrupt
$600C218C CONSTANT INTR-STATUS        \ 4 registers: the sources that ask, 32 in each
99 CONSTANT #SOURCES
: CORE-INTR-MAP ( i -- a )   ABORT" ESP32S3: only core 0 so far"  INTR-MAP ;
: CORE-INTR-STATUS ( i -- a )   ABORT" ESP32S3: only core 0 so far"  INTR-STATUS ;

\ ---- timer group 1 (TRM chapter 12)
$60020000 CONSTANT TG1                \ timer 0 at 0
53 CONSTANT TG1-SOURCE                \ its timer 0
$70 CONSTANT TG-INT-ENA
$7C CONSTANT TG-INT-CLR
: CORE-TG1 ( i -- a )   ABORT" ESP32S3: only core 0 so far"  TG1 ;
: CORE-TG1-SOURCE ( i -- n )   ABORT" ESP32S3: only core 0 so far"  TG1-SOURCE ;
$E0000000 40 13 LSHIFT OR $600 OR CONSTANT TG-1MHZ   \ on, up, reload; the crystal / 40; alarm

\ ---- the SPI flash over the ROM
$40000A50 CONSTANT 'FLASH-PARAMS      \ esp_rom_spiflash_config_param ( id size block sector page mask -- r )
$40000A2C CONSTANT 'FLASH-UNLOCK      \ esp_rom_spiflash_unlock ( -- r )
$400009FC CONSTANT 'FLASH-ERASE       \ esp_rom_spiflash_erase_sector ( sector -- r )
$40000A08 CONSTANT 'FLASH-ERASE-BLOCK \ esp_rom_spiflash_erase_block ( block -- r ): 64 KB
$40000A14 CONSTANT 'FLASH-WRITE       \ esp_rom_spiflash_write ( faddr a u -- r )
$40000A20 CONSTANT 'FLASH-READ        \ esp_rom_spiflash_read ( faddr a u -- r )
PARSE-NAME FLASH-SYSCALL HEADER  100 ,  REVEAL   \ ( x1 .. xn n a -- x ) here the same as SYSCALL
$1000000 CONSTANT FLASH-SIZE          \ 16 MB: the flash of the board (esp32s3/diymore/n16r8.fs)
$3C000000 CONSTANT EXT-RAM            \ the PSRAM through the data cache
$3E000000 CONSTANT EXT-RAM-END

END-PACKAGE
: CHIP ( -- )   ESP32S3 ;   \ ALSO CHIP: the package of the chip of this board
