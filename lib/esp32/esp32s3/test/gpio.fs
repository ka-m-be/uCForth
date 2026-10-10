\ gpio.fs - tests of GPIO and the GPIO matrix, on the board (lib/esp32/gpio.fs)
\
\ On the board, in ucf term, after the tester:
\   #include forth/test/tester.fs
\   #include lib/esp32/esp32s3/test/gpio.fs
\ No wires: GPIO 2 is an output, and its level comes back into the chip
\ (PIN@, the interrupts of the pin, the pulse counter). GPIO 0 is the
\ button BOOT: the test pulls it low for a moment, as a press does (only
\ low: the button connects it to 0 V).

#require RMT ../../rmt.fs
#require WS2812 ../../ws2812.fs
ONLY FORTH DEFINITIONS  ALSO INTERNALS  ALSO GPIO  ALSO INTR  ALSO WS2812  DECIMAL
: MUX@ ( pin -- x )   4 * 4 + $60009000 + @ ;   \ IO_MUX_GPIOn

\ ---- the kind of a pin
T{ 2 PULLUP  2 PIN@ -> TRUE }T
T{ 2 PULLDOWN  2 PIN@ -> FALSE }T
T{ 2 OUTPUT  2 HIGH  2 PIN@ -> TRUE }T
T{ 2 LOW  2 PIN@ -> FALSE }T
T{ 2 TOGGLE  2 PIN@ -> TRUE }T
T{ FALSE 2 PIN!  2 PIN@ -> FALSE }T
T{ 2 OPEN-DRAIN  2 LOW  2 PIN@ -> FALSE }T
T{ $80 2 4 * 4 + $60009000 + BIS!  2 HIGH  2 PIN@ -> FALSE }T   \ HIGH lets go: the pull-down wins
T{ 3 2 DRIVE  2 MUX@ 10 RSHIFT 3 AND -> 3 }T
T{ 2 OUTPUT  2 MUX@ 10 RSHIFT 3 AND -> 2 }T

\ ---- interrupts of a pin
VARIABLE EDGES
: EDGE ( -- )   1 EDGES +! ;
2 OUTPUT  2 LOW  +INT
T{ ' EDGE 2 RISING PIN-INT!  0 EDGES !  2 HIGH 10 US  2 LOW 10 US  2 HIGH 10 US  EDGES @ -> 2 }T
T{ ' EDGE 2 FALLING PIN-INT!  0 EDGES !  2 LOW 10 US  2 HIGH 10 US  2 LOW 10 US  EDGES @ -> 2 }T
T{ ' EDGE 2 ANY-EDGE PIN-INT!  0 EDGES !  2 HIGH 10 US  2 LOW 10 US  2 HIGH 10 US  EDGES @ -> 3 }T
T{ 0 2 0 PIN-INT!  0 EDGES !  2 LOW 10 US  2 HIGH 10 US  EDGES @ -> 0 }T
: HIGH-SEEN ( -- )   1 EDGES +!  2 LOW ;   \ it ends the level, else it comes again at once
T{ 2 LOW  ' HIGH-SEEN 2 HIGH-LEVEL PIN-INT!  0 EDGES !  2 HIGH 10 US  EDGES @  2 PIN@ -> 1 FALSE }T
T{ 0 2 0 PIN-INT! -> }T

\ ---- the button BOOT (GPIO 0): a press, made by the chip
VARIABLE PRESSES
: PRESSED ( -- )   1 PRESSES +! ;
T{ ' PRESSED 0 FALLING PIN-INT!  0 PRESSES !  0 OUTPUT 0 LOW 10 US  0 PULLUP 10 US  PRESSES @ -> 1 }T
T{ 0 0 0 PIN-INT!  0 PIN@ -> TRUE }T

\ ---- signals through the GPIO matrix: GPIO 2 to the pulse counter (PIN>SIGNAL),
\ ---- then the RMT to GPIO 2 (SIGNAL>PIN in RGB-PIN)
$60017000 CONSTANT PCNT
: PCNT-ON ( -- )   \ unit 0 counts the rising edges of its signal 0
  $400 $600C0018 BIS!  $400 $600C0020 BIC!      \ the clock of the PCNT on, out of reset
  1 18 LSHIFT PCNT !                            \ PCNT_U0_CONF0: CH0_POS_MODE 1, count up
  $10001 PCNT $60 + !  $10000 PCNT $60 + ! ;    \ PCNT_CTRL: clock on; the counter to 0
: COUNT@ ( -- n )   PCNT $30 + @ $FFFF AND ;
2 33 PIN>SIGNAL  PCNT-ON                        \ signal 33: PCNT_SIG_CH0_IN0
T{ COUNT@  2 HIGH 2 LOW  2 HIGH 2 LOW  COUNT@ SWAP - -> 2 }T
T{ 2 RGB-PIN  COUNT@  0 0 0 RGB!  COUNT@ SWAP - -> 24 }T   \ 24 bits: a rising edge each
-INT  2 INPUT

S" gpio" .RESULTS
