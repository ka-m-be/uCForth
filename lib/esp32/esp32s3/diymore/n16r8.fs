\ n16r8.fs - package DIYMORE-N16R8: the board ESP32-S3 N16R8 of diymore
\
\ DevKitC-1 compatible: the chip ESP32-S3 with 8 MB PSRAM, 16 MB flash
\ (targets/esp32s3/README.md, "The board"). The RGB LED (WS2812) hangs at
\ GPIO 48 (package WS2812), the button BOOT at GPIO 0: 0 when pressed.
\
\   LED ( -- pin )      the data line of the RGB LED
\   BUTTON ( -- pin )   the button BOOT; first BUTTON PULLUP (package GPIO)
\   LED-ON  LED-OFF     the RGB LED green or dark
\
\ At the end, BOARD ( -- ) gives this package: ALSO BOARD gives the words
\ of the board, as ALSO CHIP the values of the chip (lib/esp32/board.fs).
\ On the board, in ucf term:  #include lib/esp32/esp32s3/diymore/n16r8.fs

#require RMT ../../rmt.fs
#require WS2812 ../../ws2812.fs
PACKAGE DIYMORE-N16R8  ALSO WS2812   \ lib/package.fs
PUBLIC
48 CONSTANT LED
0 CONSTANT BUTTON
: LED-ON ( -- )    LED RGB-PIN  0 32 0 RGB! ;
: LED-OFF ( -- )   LED RGB-PIN  0 0 0 RGB! ;
END-PACKAGE
: BOARD ( -- )   DIYMORE-N16R8 ;   \ ALSO BOARD: the package of this board
