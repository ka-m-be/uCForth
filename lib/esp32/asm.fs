\ asm.fs - the package ASM (CODE words) for the chip of the board
\
\ The ESP32-S3 runs a CODE word in the dictionary
\ (lib/esp32/esp32s3/asm.fs), the ESP32 in SRAM0 (lib/esp32/esp32/asm.fs);
\ both have the same words. This file is for ucf term: #when sends the
\ file for the chip of the board (host/README.md).
\
\ On the board, in ucf term (the assembler first):
\   #require FAIL lib/text.fs
\   #require ADD, asm/xtensa.fs
\   #include lib/esp32/asm.fs

#when ESP32S3 esp32s3/asm.fs
#when ESP32 esp32/asm.fs
