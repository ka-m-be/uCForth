\ gpio.fs - package GPIO: the base of the drivers for the ESP32 chips
\
\ The words that the drivers share: bits in registers, RAM inside the
\ chip, GPIO, and the signals of the GPIO matrix. The same words on each
\ chip; the addresses and numbers come from the layer of the chip
\ (ALSO CHIP: lib/esp32/esp32s3/chip.fs, lib/esp32/esp32/chip.fs).
\
\   x mask a BITS! ( -- )       the bits of mask in the register at a get the bits of x
\   u SRAM-ALLOT ( -- a )       u bytes in the RAM inside the chip, also when the
\                               RAM data grow into the PSRAM (package PSRAM)
\   u SRAM-BUFFER: name         a buffer of u bytes in the RAM inside the chip,
\                               for example for DMA; name ( -- a )
\
\ GPIO: pin numbers are the GPIO numbers of the chip (ESP32-S3: 0 to 48;
\ ESP32: 0 to 39). A pin gets the function GPIO in the IO MUX and the
\ output register of the GPIO unit in the GPIO matrix (Technical
\ Reference Manual of the chip, the chapter "IO MUX and GPIO Matrix"):
\
\   pin OUTPUT  INPUT  PULLUP  PULLDOWN  OPEN-DRAIN     the kind of the pin
\   pin HIGH  LOW  TOGGLE   f pin PIN!   pin PIN@ ( -- f )
\   n pin DRIVE           the strength of the output, 0 to 3 (about 5, 10,
\                         20, 40 mA); OUTPUT gives 2
\   signal pin SIGNAL>PIN   the output signal of a peripheral drives the pin
\   pin signal PIN>SIGNAL   the pin goes to the input signal of a peripheral
\                           (the numbers of the signals: in the Technical
\                           Reference Manual of the chip)
\   xt pin mode PIN-INT!    xt ( -- ) runs when mode happens at the pin:
\                           RISING FALLING ANY-EDGE LOW-LEVEL HIGH-LEVEL;
\                           mode 0: not any more
\
\ A word of PIN-INT! is a Forth handler of interrupts (package INTR,
\ the source of the GPIO unit): +INT switches the handlers on. A pin
\ keeps its level for PIN@ also as an output.
\ Careful: some pins belong to the board (targets/esp32s3/README.md and
\ targets/esp32/README.md, section "The board").
\ On the board, in ucf term:  #include lib/esp32/gpio.fs  (needs CHIP and INTR)

PACKAGE GPIO  ALSO CHIP  ALSO INTR   \ lib/package.fs
: BIT ( pin -- mask high? )   \ pins 32 and up are in the second set of registers
  DUP 32 < IF 1 SWAP LSHIFT FALSE EXIT THEN  32 - 1 SWAP LSHIFT TRUE ;
: SET-REG ( pin offset offset1 -- mask a )   \ the register for the pin: offset or offset1
  ROT BIT IF ROT DROP ELSE NIP THEN  SWAP GPIO-BASE + ;
: >PIN ( pin -- a )  4 * GPIO-BASE PIN-REG + + ;   \ GPIO_PINn_REG
: NO-PULL ( pin -- )   >PULL OR SWAP BIC! ;
: ROUTE ( x pin -- )   \ IO MUX: GPIO, drive 2, input on, no pull; the matrix: x
  4 OVER >PIN BIC!                                \ not open drain
  DUP NO-PULL
  MUX-GPIO OVER >MUX !  4 * GPIO-BASE OUT-SEL + + ! ;   \ GPIO_FUNCn_OUT_SEL_CFG
: AS-GPIO ( pin -- )   256 SWAP ROUTE ;       \ signal 256: the register GPIO_OUT

PUBLIC
: BITS! ( x mask a -- )   \ the bits of mask at a get the bits of x
  DUP >R @  OVER INVERT AND  >R AND R> OR  R> ! ;
: SRAM-ALLOT ( u -- a )   DATA-ALLOT ;                \ in the RAM data region (core.fs)
: SRAM-BUFFER: ( u "name" -- )   SRAM-ALLOT CONSTANT ;

: HIGH   ( pin -- )    $08 $14 SET-REG ! ;          \ GPIO_OUT_W1TS, GPIO_OUT1_W1TS
: LOW    ( pin -- )    $0C $18 SET-REG ! ;          \ GPIO_OUT_W1TC, GPIO_OUT1_W1TC
: PIN@   ( pin -- f )  $3C $40 SET-REG @ AND 0<> ;  \ GPIO_IN, GPIO_IN1: the level at the pin
: TOGGLE ( pin -- )    DUP $04 $10 SET-REG @ AND IF LOW ELSE HIGH THEN ;   \ GPIO_OUT, GPIO_OUT1
: PIN!   ( f pin -- )  SWAP IF HIGH ELSE LOW THEN ;
: OUTPUT ( pin -- )    DUP AS-GPIO  $24 $30 SET-REG ! ;         \ GPIO_ENABLE_W1TS
: INPUT  ( pin -- )    DUP AS-GPIO  $28 $34 SET-REG ! ;         \ GPIO_ENABLE_W1TC; no pull
: PULLUP ( pin -- )    \ the pull resistor up (the layer of the chip knows its place)
  DUP INPUT  >PULL DROP  DUP 0= ABORT" GPIO: no pull resistor at this pin"  SWAP BIS! ;
: PULLDOWN ( pin -- )
  DUP INPUT  >PULL NIP  DUP 0= ABORT" GPIO: no pull resistor at this pin"  SWAP BIS! ;
: OPEN-DRAIN ( pin -- )   \ HIGH lets the pin go, LOW pulls it to 0
  DUP OUTPUT  4 SWAP >PIN BIS! ;                                \ PAD_DRIVER
: DRIVE ( n pin -- )   >R  3 AND 10 LSHIFT  $C00  R> >MUX BITS! ;   \ FUN_DRV
: SIGNAL>PIN ( signal pin -- )   \ the output enable comes from GPIO_ENABLE
  TUCK SWAP $400 OR SWAP ROUTE  $24 $30 SET-REG ! ;
: PIN>SIGNAL ( pin signal -- )   \ GPIO_FUNCn_IN_SEL_CFG: through the matrix
  OVER $80 OR  SWAP 4 * GPIO-BASE IN-SEL + + !  $200 SWAP >MUX BIS! ;

PRIVATE
#PINS CELLS BUFFER: PIN-XTS          \ the words of PIN-INT!
: CLEAR-PIN ( pin -- )   $4C $58 SET-REG ! ;          \ GPIO_STATUS_W1TC, GPIO_STATUS1_W1TC
: LEVEL? ( pin -- f )   >PIN @ 7 RSHIFT 7 AND 3 > ;   \ INT_TYPE 4 or 5
: RUN-PIN ( pin -- )   \ its word; the status of an edge goes before it, of a level after it
  DUP LEVEL? 0= IF DUP CLEAR-PIN THEN
  DUP CELLS PIN-XTS + @ ?DUP IF EXECUTE THEN
  DUP LEVEL? IF CLEAR-PIN ELSE DROP THEN ;
: RUN-PINS ( bits pin -- )   \ the pins with a bit in bits, from pin on
  SWAP  BEGIN ?DUP WHILE
    DUP 1 AND IF  OVER RUN-PIN  THEN
    1 RSHIFT  SWAP 1+ SWAP
  REPEAT  DROP ;
: GPIO-ISR ( -- )   \ the handler of the GPIO unit: the pins that ask
  GPIO-BASE PCPU-INT + @  0 RUN-PINS
  GPIO-BASE PCPU-INT1 + @  32 RUN-PINS ;

PUBLIC
1 CONSTANT RISING   2 CONSTANT FALLING   3 CONSTANT ANY-EDGE
4 CONSTANT LOW-LEVEL   5 CONSTANT HIGH-LEVEL
: PIN-INT! ( xt pin mode -- )
  >R  TUCK CELLS PIN-XTS + !                               ( pin )  ( R: mode )
  DUP CLEAR-PIN                                            \ an old event of the pin goes
  R@ 7 AND 7 LSHIFT  R> IF PIN-INT-CPU OR THEN             \ INT_TYPE; INT_ENA: the CPU
  $3E380 ROT >PIN BITS!
  GPIO-SOURCE INT@ ['] GPIO-ISR <> IF  ['] GPIO-ISR GPIO-SOURCE INT!  GPIO-SOURCE INT-ON  THEN ;

END-PACKAGE
