\ wroom-n4.fs - package DIYMORE-WROOM-N4: the board ESP-WROOM-32 N4 of diymore
\
\ The module ESP32-WROOM-32 with 4 MB flash ("N4") and a USB serial bridge
\ CH340C (targets/esp32/README.md, "The board"). The LED hangs at GPIO 2,
\ the button BOOT at GPIO 0: 0 when pressed.
\
\   LED ( -- pin )      the pin of the LED
\   BUTTON ( -- pin )   the button BOOT; first BUTTON PULLUP (package GPIO)
\   LED-ON  LED-OFF     the LED on or off
\
\ At the end, BOARD ( -- ) gives this package: ALSO BOARD gives the words
\ of the board, as ALSO CHIP the values of the chip (lib/esp32/board.fs).
\ On the board, in ucf term:  #include lib/esp32/esp32/diymore/wroom-n4.fs

PACKAGE DIYMORE-WROOM-N4  ALSO GPIO   \ lib/package.fs
PUBLIC
2 CONSTANT LED
0 CONSTANT BUTTON
: LED-ON ( -- )    LED OUTPUT  LED HIGH ;
: LED-OFF ( -- )   LED OUTPUT  LED LOW ;
END-PACKAGE
: BOARD ( -- )   DIYMORE-WROOM-N4 ;   \ ALSO BOARD: the package of this board
