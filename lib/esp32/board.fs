\ board.fs - the package of the board (BOARD): the tested board of each chip
\
\ A file that needs the words of the board (LED, BUTTON, LED-ON, LED-OFF)
\ loads this file in ucf term:  #require BOARD lib/esp32/board.fs
\ On an ESP32-S3 it loads the package of the board ESP32-S3 N16R8 of
\ diymore, on an ESP32 the package of the board ESP-WROOM-32 N4 of diymore.
\ Another board: load the file of that board first; it defines BOARD, and
\ #require does not load this file then (lib/esp32/README.md).

#when ESP32S3 esp32s3/diymore/n16r8.fs
#when ESP32 esp32/diymore/wroom-n4.fs
