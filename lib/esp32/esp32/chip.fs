\ chip.fs - package ESP32: the layer of the chip ESP32
\
\ The addresses and numbers of the ESP32 (for example in the module
\ ESP-WROOM-32) that the common packages in lib/esp32/ need (gpio.fs,
\ intr.fs, timers.fs, flash.fs). They write ALSO CHIP and take the
\ values from here; the ESP32-S3 has the same words with its own values
\ (lib/esp32/esp32s3/chip.fs). The values are from the Technical
\ Reference Manual of the ESP32 (TRM), the headers of ESP-IDF and the ROM
\ of the chip (esp32.rom.ld). The words: lib/esp32/esp32s3/chip.fs; here
\ also:
\
\   APB-MHZ ( -- n )       the APB clock in MHz: 40 at the start, 80 with
\                          the PLL (package CLOCK sets it with TO); the
\                          timer groups count it
\   MICROS ( -- u )        the time in us, from the clock of the HAL (timer 0
\                          of timer group 0); only the ESP32 has it so far
\
\ The GPIO numbers are 0 to 39, without 20, 24 and 28 to 31; 34 to 39
\ are inputs only, without pull resistors. The IO MUX has its registers
\ not in the order of the pins: >MUX takes them from a table and stops at
\ a number without a pin. The pins with an RTC function (0, 2, 4, 12 to
\ 15, 25 to 27, 32, 33) have their pull resistors in the RTC IO, not in
\ the IO MUX: >PULL gives the register and the bits (TRM chapter 4.11;
\ ESP-IDF: rtc_io_desc).
\
\ At the end, CHIP ( -- ) gives the package of the chip of this board:
\ a common package writes ALSO CHIP. The name ESP32 also tells ucf term
\ which chip the board has (#when, host/README.md).

PACKAGE ESP32   \ lib/package.fs

PUBLIC
: CHIP-NAME ( -- a u )   S" ESP32" ;
40 VALUE APB-MHZ

\ ---- GPIO and IO MUX (TRM chapter 4)
$3FF44000 CONSTANT GPIO-BASE
$3FF49000 CONSTANT IO-MUX
40 CONSTANT #PINS                     \ GPIO 0 to 39
PRIVATE
CREATE MUX-OFFSETS                    \ the register of GPIO n in the IO MUX; 0: no pin
  $44 C, $88 C, $40 C, $84 C, $48 C, $6C C, $60 C, $64 C, $68 C, $54 C,    \ 0-9
  $58 C, $5C C, $34 C, $38 C, $30 C, $3C C, $4C C, $50 C, $70 C, $74 C,    \ 10-19
  0 C,   $7C C, $80 C, $8C C, 0 C,   $24 C, $28 C, $2C C, 0 C,   0 C,      \ 20-29
  0 C,   0 C,   $1C C, $20 C, $14 C, $18 C, $04 C, $08 C, $0C C, $10 C,    \ 30-39
PUBLIC
: >MUX ( pin -- a )   \ IO_MUX_GPIOn
  DUP #PINS U< 0= ABORT" ESP32: no such GPIO"
  MUX-OFFSETS + C@  DUP 0= ABORT" ESP32: no such GPIO"  IO-MUX + ;
$2A00 CONSTANT MUX-GPIO               \ function 2 (GPIO), drive 2, input on
PRIVATE
CREATE TOUCH-PINS   4 C, 0 C, 2 C, 15 C, 13 C, 12 C, 14 C, 27 C,   \ the touch pads 0 to 7
: TOUCH-REG ( pin -- a | 0 )   \ RTC_IO_TOUCH_PADn_REG of the pin, or 0
  8 0 DO  DUP TOUCH-PINS I + C@ = IF  DROP $3FF48494 I CELLS + UNLOOP EXIT  THEN  LOOP
  DROP 0 ;
$8000000 CONSTANT RUE   $10000000 CONSTANT RDE   \ bits 27 and 28 of a pad of the RTC IO
PUBLIC
: >PULL ( pin -- a up down )   \ the register and the bits of the pull resistors of the pin
  DUP 34 40 WITHIN IF  >MUX 0 0 EXIT  THEN            \ inputs only: none
  DUP 32 = IF DROP $3FF4848C $400000 $800000 EXIT THEN  \ RTC_IO_XTAL_32K_PAD: X32P
  DUP 33 = IF DROP $3FF4848C RUE RDE EXIT THEN          \ X32N
  DUP 25 = IF DROP $3FF48484 RUE RDE EXIT THEN          \ RTC_IO_PAD_DAC1
  DUP 26 = IF DROP $3FF48488 RUE RDE EXIT THEN          \ RTC_IO_PAD_DAC2
  DUP TOUCH-REG ?DUP IF  NIP RUE RDE EXIT  THEN
  >MUX $100 $80 ;                                       \ FUN_WPU, FUN_WPD in the IO MUX
$88 CONSTANT PIN-REG                  \ GPIO_PIN0_REG
$130 CONSTANT IN-SEL                  \ GPIO_FUNC0_IN_SEL_CFG_REG
$530 CONSTANT OUT-SEL                 \ GPIO_FUNC0_OUT_SEL_CFG_REG
$68 CONSTANT PCPU-INT                 \ GPIO_PCPU_INT_REG: pins 0 to 31
$7C CONSTANT PCPU-INT1                \ GPIO_PCPU_INT1_REG: pins 32 to 39
$8000 CONSTANT PIN-INT-CPU            \ GPIO_PINn: INT_ENA, the PRO CPU (bit 15)
22 CONSTANT GPIO-SOURCE

\ ---- the interrupt matrix of the PRO CPU (TRM chapter 2)
$3FF00104 CONSTANT INTR-MAP           \ a register for each source: its CPU interrupt
$3FF000EC CONSTANT INTR-STATUS        \ 3 registers: the sources that ask, 32 in each
69 CONSTANT #SOURCES

\ ---- timer group 0: its timer 0 is the clock of the HAL (MS, US, MILLIS), 1 MHz
$3FF5F000 CONSTANT TG0                \ timer 0 at 0
$0C CONSTANT TG-UPDATE                \ a write takes the counter into TG-LO and TG-HI
$04 CONSTANT TG-LO                    \ bits 0 to 31 of the counter
: MICROS ( -- u )   0 TG0 TG-UPDATE + !  TG0 TG-LO + @ ;   \ the time in us (32 bits; it wraps after 71 minutes)

\ ---- timer group 1 (TRM chapter 18)
$3FF60000 CONSTANT TG1                \ timer 0 at 0
18 CONSTANT TG1-SOURCE                \ its timer 0, by level
$98 CONSTANT TG-INT-ENA
$A4 CONSTANT TG-INT-CLR
: TG-1MHZ ( -- x )   \ on, up, reload, alarm, interrupt by level; the APB clock / APB-MHZ
  $E0000C00  APB-MHZ 13 LSHIFT OR ;

\ ---- the SPI flash over the ROM
$40063238 CONSTANT 'FLASH-PARAMS      \ esp_rom_spiflash_config_param ( id size block sector page mask -- r )
0 CONSTANT 'FLASH-UNLOCK              \ ESP-IDF has its own; FLASH does not unlock on the ESP32
$40062CCC CONSTANT 'FLASH-ERASE       \ esp_rom_spiflash_erase_sector ( sector -- r )
$40062C4C CONSTANT 'FLASH-ERASE-BLOCK \ esp_rom_spiflash_erase_block ( block -- r ): 64 KB
$40062D50 CONSTANT 'FLASH-WRITE       \ esp_rom_spiflash_write ( faddr a u -- r )
$40062ED8 CONSTANT 'FLASH-READ        \ esp_rom_spiflash_read ( faddr a u -- r )
PARSE-NAME FLASH-SYSCALL HEADER  105 ,  REVEAL   \ ( x1 .. xn n a -- x ) SYSCALL with the cache of the flash off (targets/esp32/hal.fs)
$400000 CONSTANT FLASH-SIZE           \ 4 MB: the flash of the module ESP32-WROOM-32 N4
0 CONSTANT EXT-RAM                    \ no PSRAM
0 CONSTANT EXT-RAM-END

END-PACKAGE
: CHIP ( -- )   ESP32 ;   \ ALSO CHIP: the package of the chip of this board
