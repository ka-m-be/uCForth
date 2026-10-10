\ blink.fs - the LED of the board blinks
\
\ On the board, in ucf term:  #include examples/blink.fs  then  BLINK
\ A key stops it. The words of the board come from its package (BOARD,
\ lib/esp32/board.fs): on the diymore ESP32-S3 N16R8 the RGB LED blinks
\ green (packages RMT and WS2812), on the diymore ESP-WROOM-32 N4 the LED
\ at GPIO 2.

#require BOARD ../lib/esp32/board.fs
ONLY FORTH DEFINITIONS  ALSO BOARD

: BLINK ( -- )
  BEGIN  LED-ON 250 MS  LED-OFF 250 MS  KEY? UNTIL ;
