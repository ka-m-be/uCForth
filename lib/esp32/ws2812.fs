\ ws2812.fs - package WS2812: an RGB LED with one data line, over the RMT
\
\ The LED takes 24 bits: green, red, blue, each with the high bit first.
\ A bit is a pulse of 1.2 us: 0.8 us high for a 1, 0.4 us for a 0. Then
\ 50 us low: the LED takes the color. Channel 0 of the RMT sends the
\ bits (package RMT); one tick is 0.1 us. Its signal goes to the pin
\ through the GPIO matrix.
\
\   pin RGB-PIN ( -- )    the LED hangs at pin (the diymore ESP32-S3 N16R8: 48, LED)
\   r g b RGB! ( -- )     one color, each part 0 to 255
\
\ On the board, in ucf term:
\   #require RMT lib/esp32/rmt.fs
\   #include lib/esp32/ws2812.fs
\   ALSO WS2812  48 RGB-PIN  0 32 0 RGB!

PACKAGE WS2812  ALSO RMT   \ lib/package.fs

0 CONSTANT CH                       \ the channel of the RMT
$00048008 CONSTANT ONE              \ a pulse word: 1: 0.8 us high, 0.4 us low
$00088004 CONSTANT ZERO             \ 0: 0.4 us high, 0.8 us low
500 CONSTANT LATCH                  \ 50 us low: the LED takes the color
25 CELLS BUFFER: BITS               \ 24 bits and the latch

PUBLIC
: RGB-PIN ( pin -- )   \ channel 0 of the RMT drives the data line at this pin
  RMT-ON  CH RMT-HZ 10000000 / RMT-DIV  CH 1 RMT-BLOCKS  CH 0 RMT-IDLE  CH 0 0 RMT-CARRIER
  CH SWAP RMT-OUT ;
PRIVATE
: BYTE, ( c i -- )   \ c as pulse words i to i+7, the high bit first
  8 0 DO  OVER 128 AND IF ONE ELSE ZERO THEN  OVER I + CELLS BITS + !  SWAP 2* SWAP  LOOP
  2DROP ;
PUBLIC
: RGB! ( r g b -- )   \ the LED wants green, red, blue
  ROT ROT  0 BYTE,  8 BYTE,  16 BYTE,
  LATCH 24 CELLS BITS + !  BITS 25 CH RMT-SEND ;

END-PACKAGE
