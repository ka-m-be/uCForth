\ cores.fs - the package MULTICORE for the chip of the board
\
\ MULTICORE (lib/esp32/multicore.fs) is the same for each chip; it uses
\ the part of the chip: ESP32-CORES (lib/esp32/esp32/cores.fs). This file
\ is for ucf term: #when sends the part for the chip of the board
\ (host/README.md), then the package follows.
\ On the board, in ucf term:  #require MULTICORE lib/esp32/cores.fs

#when ESP32 esp32/cores.fs
#include multicore.fs
