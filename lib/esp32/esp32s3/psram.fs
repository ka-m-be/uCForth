\ psram.fs - package PSRAM: the octal PSRAM of the ESP32-S3R8, 8 MB
\
\ The chip ESP32-S3R8 has 8 MB PSRAM in its package, an octal DDR chip of
\ AP Memory at chip select 1 of the memory SPI (SPI0 for the cache, SPI1
\ for commands). PSRAM-ON sets it up as ESP-IDF 5.3 does
\ (components/esp_psram/esp32s3/esp_psram_impl_octal.c), at 40 MHz,
\ where it needs no timing tuning:
\
\ 1. The pins (GPIO 26 chip select, GPIO 33-37 data 4-7 and strobe), the
\    times of the chip select, the clocks slow (20 MHz).
\ 2. Commands to the chip over SPI1 (esp_rom_opiflash_exec_cmd of the
\    ROM): fixed read latency, then a test word; the vendor and the size
\    come from its mode registers.
\ 3. The clock to 40 MHz; SPI0 gets the commands, addresses and dummy
\    cycles of the chip, for the data cache.
\ 4. The data cache is set up again, as the ROM does it when it starts
\    from the flash (ROM_Boot_Cache_Init; after "ucf run" the ROM has
\    not done it). Its MMU maps the 8 MB as 128 pages of 64 KB to the
\    chip addresses $3C000000 to $3C7FFFFF.
\
\ Then @ ! C@ C! MOVE and all memory words reach the PSRAM with these
\ addresses (a chip address stays as it is, targets/esp32s3/README.md).
\ The data cache (64 KB, lines of 32 bytes) holds the recent lines. The
\ RAM data of VALLOT grow into the PSRAM (core.fs: XVP, XDATA-END):
\ VARIABLE, BUFFER:, VALUE and DEFER are there after PSRAM-ON.
\
\   PSRAM-ON ( -- )          set up the PSRAM (ON-BOOT does it)
\   PSRAM-ADDR ( -- a )      its first address: $3C000000
\   PSRAM-SIZE ( -- u )      its size in bytes, after PSRAM-ON
\   .PSRAM ( -- )            show the mode registers of the chip
\   size ways line DCACHE-MODE ( -- )   another mode of the data cache
\
\ Needs the CPU clock from the PLL (package CLOCK, 80 MHz or more).
\ On the board, in ucf term:  #include lib/esp32/esp32s3/psram.fs

PACKAGE PSRAM  ALSO ROM  ALSO CLOCK  ALSO GPIO   \ lib/package.fs

$40000894 CONSTANT 'PIN-CONFIG      \ esp_rom_opiflash_pin_config ( -- )
$400008B8 CONSTANT 'EXEC-CMD        \ esp_rom_opiflash_exec_cmd: 13 arguments (EXEC)
$4000093C CONSTANT 'DTR-SWAP        \ esp_rom_spi_set_dtr_swap_mode ( spi wr rd -- )
$40001668 CONSTANT 'CACHE-INIT      \ ROM_Boot_Cache_Init ( -- ): the data cache as after a start from the flash
$400018B4 CONSTANT 'SUSPEND         \ Cache_Suspend_DCache ( -- state )
$400018C0 CONSTANT 'RESUME          \ Cache_Resume_DCache ( state -- )
$400016E0 CONSTANT 'INVALIDATE      \ Cache_Invalidate_DCache_All ( -- )
$40001A28 CONSTANT 'CACHE-MODE      \ rom_config_data_cache_mode ( size ways line -- )
$400016F8 CONSTANT 'WRITEBACK       \ Cache_WriteBack_All ( -- ): changed lines into the PSRAM

$60003000 CONSTANT SPI0             \ the memory SPI for the cache
$60002000 CONSTANT SPI1             \ the memory SPI for commands
$6000906C CONSTANT MUX-GPIO26       \ IO MUX of GPIO 26: chip select 1
$600C4130 CONSTANT CACHE-STATE      \ bits 23-12: 1 when the data cache is suspended
$600C5000 CONSTANT MMU              \ 512 entries, one for each 64 KB from $3C000000
$600C4148 CONSTANT MMU-OWNER        \ 0 after "ucf run": the data cache is not set up
PUBLIC
$3C000000 CONSTANT PSRAM-ADDR       \ the first byte of the PSRAM for the CPU
PRIVATE


\ ---- commands over SPI1 --------------------------------------------------

8 BUFFER: DATA                      \ the bytes of one command
VARIABLE SIZE
: EXEC ( cmd addr dummy mosi bits miso bits -- )   \ OPI DTR, 16-bit command, 32-bit address, CS1
  >R >R >R >R >R >R                 ( cmd )     ( R: bits miso bits mosi dummy addr )
  1 7 ROT 16  R> 32  R> R> R> R> R>  2 0  13 'EXEC-CMD SYSCALL DROP ;
: REG@ ( n -- x )   \ mode register n and n+1 (16 bits)
  0 DATA !  $4040 SWAP 8 0 0 DATA >CHIP 16 EXEC  DATA @ $FFFF AND ;
: MR0! ( x -- )   DATA !  $C0C0 0 0 DATA >CHIP 16 0 0 EXEC ;
: WORD! ( x -- )   DATA !  $8080 0 8 DATA >CHIP 32 0 0 EXEC ;       \ at PSRAM address 0
: WORD@ ( -- x )   0 DATA !  $0000 0 18 0 0 DATA >CHIP 32 EXEC  DATA @ ;

\ ---- the steps -----------------------------------------------------------

: PINS ( -- )
  0 'PIN-CONFIG SYSCALL DROP                    \ GPIO 33-37: data 4-7, strobe
  $C00 $7C00 MUX-GPIO26 BITS!                  \ function 0 (SPICS1), drive 3
  3 3 SPI0 $3FC + BITS!                        \ the clock pin of the PSRAM: drive 3
  3 SPI0 $DC + BIS!                             \ chip select: setup and hold on
  3 7 LSHIFT  $F80 SPI0 $DC + BITS!            \ hold time 3
  3 2 LSHIFT  $7C  SPI0 $DC + BITS!            \ setup time 3
  2 25 LSHIFT $7E000000 SPI0 $DC + BITS! ;     \ hold delay 2
: SLOW ( -- )   \ the core of the memory SPI at 80 MHz, all clocks / 4
  0 SPI0 $EC + !  $30103 SPI0 $14 + !  $30103 SPI1 $14 + !  $30103 SPI0 $50 + ! ;
: FAST ( -- )   \ PSRAM and flash at 40 MHz
  $10001 SPI0 $50 + !  $10001 SPI0 $14 + !  $10001 SPI1 $14 + !
  2 SPI1 $3C + BIC! ;                           \ SPI1: 3-byte addresses again, for the flash
: CHIP-SETUP ( -- )
  2 SPI1 $E0 + BIS!                             \ variable dummy cycles
  1 0 0 3 'DTR-SWAP SYSCALL DROP
  0 REG@  $3F INVERT AND  $28 OR  MR0! ;        \ MR0: fixed latency, latency code 2, full drive
: PHASES ( -- )   \ how SPI0 reads and writes the PSRAM for the cache
  $F0008080 SPI0 $4C + !                        \ write: 16-bit command $8080
  $F0000000 SPI0 $48 + !                        \ read: 16-bit command $0000
  $00100039 SPI0 $40 + BIS!                     \ 4-byte address, dummy, commands on
  17 6 LSHIFT  $FC0 SPI0 $40 + BITS!           \ read dummy: 18 cycles
  31 14 LSHIFT $FC000 SPI0 $40 + BITS!         \ address: 32 bits
  7 22 LSHIFT  $FC00000 SPI0 $40 + BITS!       \ write dummy: 8 cycles
  3 SPI0 $E4 + BIS!  $C SPI0 $E4 + BIC!         \ DDR on, variable dummy, no byte swap
  $7C0000 SPI0 $44 + BIS!                       \ octal: dummy out, command, address, data
  $200000 SPI0 $40 + BIS! ;                     \ SRAM octal
: SUSPEND ( -- state )   0 'SUSPEND SYSCALL  BEGIN CACHE-STATE @ 12 RSHIFT $FFF AND 1 = UNTIL ;
: RESUME ( state -- )   0 'INVALIDATE SYSCALL DROP  1 'RESUME SYSCALL DROP ;
: MAP ( -- )   \ the PSRAM in pages of 64 KB from $3C000000
  0 'CACHE-INIT SYSCALL DROP                    \ after "ucf run" the cache has no memory yet
  SUSPEND
  $10000 8 32 3 'CACHE-MODE SYSCALL DROP        \ 64 KB, 8 ways, lines of 32 bytes (measured: README)
  SIZE @ 16 RSHIFT 0 DO  $8000 I OR  MMU I CELLS + !  LOOP
  RESUME ;
: EXTEND ( -- )   \ the RAM data grow into the PSRAM (core.fs: XVP, XDATA-END)
  PSRAM-ADDR SIZE @ + XDATA-END !
  XVP @ PSRAM-ADDR -  SIZE @ U< 0= IF  PSRAM-ADDR XVP !  THEN ;   \ XVP of a frozen image stays

PUBLIC
: PSRAM-SIZE ( -- u )   SIZE @ ;
: DCACHE-MODE ( size ways line -- )   \ the data cache: 32 or 64 KB, 4 or 8 ways, lines of 16, 32 or 64 bytes
  0 'WRITEBACK SYSCALL DROP  SUSPEND >R  3 'CACHE-MODE SYSCALL DROP  R> RESUME ;
: .PSRAM ( -- )
  BASE @ >R HEX
  ." MR0 " 0 REG@ $FF AND .  ." vendor " 0 REG@ 8 RSHIFT $1F AND .
  ." MR2 " 2 REG@ $FF AND .  ." MR4 " 4 REG@ $FF AND .
  R> BASE ! ;
: PSRAM-ON ( -- )
  MMU-OWNER @ IF  MMU @ $8000 = IF 0 'WRITEBACK SYSCALL DROP THEN  THEN   \ PSRAM on: changed lines back first
  0 SIZE !  0 XDATA-END !  CPU-MHZ 80 < ABORT" PSRAM-ON: first 240 MHZ"
  PINS SLOW CHIP-SETUP
  WORD@ >R  $5A6B7C8D WORD!  WORD@  R> WORD!     \ the test word; the old word stays
  $5A6B7C8D <> IF  FAST  TRUE ABORT" PSRAM-ON: no answer from the PSRAM"  THEN
  $400000  2 REG@ 7 AND 1- 2/  LSHIFT  SIZE !   \ density 1, 3, 5, 7: 4, 8, 16, 32 MB
  FAST PHASES MAP EXTEND ;

END-PACKAGE
