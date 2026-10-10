\ rmt.fs - the package RMT for the chip of the board
\
\ The RMT of the ESP32-S3 and the RMT of the ESP32 are built in different
\ ways. So each chip has a package RMT of its own, with the same words:
\ lib/esp32/esp32s3/rmt.fs and lib/esp32/esp32/rmt.fs. This file is for
\ ucf term: #when sends the file for the chip of the board
\ (host/README.md). A build on the host names the file of the chip.
\
\ On the board, in ucf term:  #require RMT lib/esp32/rmt.fs

#when ESP32S3 esp32s3/rmt.fs
#when ESP32 esp32/rmt.fs
