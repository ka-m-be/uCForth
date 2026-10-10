\ gpio.fs - tests of GPIO and the GPIO matrix on the ESP32, on the board (lib/esp32/gpio.fs)
\
\ On the board, in ucf term, after the tester:
\   #include forth/test/tester.fs
\   #include lib/esp32/esp32/test/gpio.fs
\ No wires: GPIO 25 is an output, and its level comes back into the chip
\ (PIN@, the interrupts of the pin, the pulse counter). GPIO 25 is free
\ on the boards with the module WROOM-32 (GPIO 2 has the LED, and it is
\ a strapping pin). GPIO 0 is the button BOOT: the test pulls it low for
\ a moment, as a press does (only low: the button connects it to 0 V).

ONLY FORTH DEFINITIONS  ALSO INTERNALS  ALSO CHIP  ALSO GPIO  ALSO INTR  DECIMAL
25 CONSTANT P
: MUX@ ( pin -- x )   >MUX @ ;            \ IO_MUX of the pin (the layer of the chip)

\ ---- the kind of a pin
T{ P PULLUP  P PIN@ -> TRUE }T
T{ P PULLDOWN  P PIN@ -> FALSE }T
T{ P OUTPUT  P HIGH  P PIN@ -> TRUE }T
T{ P LOW  P PIN@ -> FALSE }T
T{ P TOGGLE  P PIN@ -> TRUE }T
T{ FALSE P PIN!  P PIN@ -> FALSE }T
T{ P OPEN-DRAIN  P LOW  P PIN@ -> FALSE }T
T{ P >PULL NIP SWAP BIS!  P HIGH  P PIN@ -> FALSE }T   \ HIGH lets go: the pull-down (RTC IO) wins
T{ 3 P DRIVE  P MUX@ 10 RSHIFT 3 AND -> 3 }T
T{ P OUTPUT  P MUX@ 10 RSHIFT 3 AND -> 2 }T
T{ 20 ' >MUX CATCH NIP -> -2 }T                  \ GPIO 20 does not exist on the ESP32
T{ 36 ' PULLUP CATCH NIP -> -2 }T                \ GPIO 34 to 39 have no pull resistors

\ ---- interrupts of a pin
VARIABLE EDGES
: EDGE ( -- )   1 EDGES +! ;
P OUTPUT  P LOW  +INT
T{ ' EDGE P RISING PIN-INT!  0 EDGES !  P HIGH 10 US  P LOW 10 US  P HIGH 10 US  EDGES @ -> 2 }T
T{ ' EDGE P FALLING PIN-INT!  0 EDGES !  P LOW 10 US  P HIGH 10 US  P LOW 10 US  EDGES @ -> 2 }T
T{ ' EDGE P ANY-EDGE PIN-INT!  0 EDGES !  P HIGH 10 US  P LOW 10 US  P HIGH 10 US  EDGES @ -> 3 }T
T{ 0 P 0 PIN-INT!  0 EDGES !  P LOW 10 US  P HIGH 10 US  EDGES @ -> 0 }T
: HIGH-SEEN ( -- )   1 EDGES +!  P LOW ;   \ it ends the level, else it comes again at once
T{ P LOW  ' HIGH-SEEN P HIGH-LEVEL PIN-INT!  0 EDGES !  P HIGH 10 US  EDGES @  P PIN@ -> 1 FALSE }T
T{ 0 P 0 PIN-INT! -> }T

\ ---- the button BOOT (GPIO 0): a press, made by the chip
VARIABLE PRESSES
: PRESSED ( -- )   1 PRESSES +! ;
T{ ' PRESSED 0 FALLING PIN-INT!  0 PRESSES !  0 OUTPUT 0 LOW 10 US  0 PULLUP 10 US  PRESSES @ -> 1 }T
T{ 0 0 0 PIN-INT!  0 PIN@ -> TRUE }T

\ ---- a signal through the GPIO matrix: GPIO 25 to the pulse counter (PIN>SIGNAL)
$3FF57000 CONSTANT PCNT
: PCNT-ON ( -- )   \ unit 0 counts the rising edges of its signal 0
  $400 $3FF000C0 BIS!  $400 $3FF000C4 BIC!      \ DPORT: the clock of the PCNT on, out of reset
  1 18 LSHIFT PCNT !                            \ PCNT_U0_CONF0: CH0_POS_MODE 1, count up
  $10001 PCNT $B0 + !  $10000 PCNT $B0 + ! ;    \ PCNT_CTRL: clock on; the counter to 0
: COUNT@ ( -- n )   PCNT $60 + @ $FFFF AND ;
P 39 PIN>SIGNAL  PCNT-ON                        \ signal 39: PCNT_SIG_CH0_IN0
T{ COUNT@  P HIGH P LOW  P HIGH P LOW  COUNT@ SWAP - -> 2 }T
-INT  P INPUT

S" gpio" .RESULTS
