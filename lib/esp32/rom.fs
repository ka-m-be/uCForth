\ rom.fs - package ROM: call the functions in the ROM of the ESP32-S3
\
\ The ROM of the chip has many functions: for the flash, the caches, the
\ clock and more. Their addresses are in the linker script esp32s3.rom.ld
\ of ESP-IDF. The kernel has two HAL words for them (targets/esp32s3/hal.fs);
\ this package gives them their headers:
\
\   x1 .. xn n a SYSCALL ( -- x )   run the function at a with n arguments
\                                   (at most 13); x is its result
\   a >CHIP ( -- a' )               the chip address of the VM address a;
\                                   any other address stays
\
\ A function of the ROM wants chip addresses: give it "BUF >CHIP", not
\ "BUF" (targets/esp32s3/README.md, "Memory").
\
\ On the board, in ucf term:  #include lib/esp32/rom.fs
\ Example: the clock of the CPU in MHz (ets_get_cpu_frequency):
\   0 $40001A40 SYSCALL .

PACKAGE ROM   \ lib/package.fs
PUBLIC
PARSE-NAME SYSCALL HEADER  100 ,  REVEAL
PARSE-NAME >CHIP   HEADER  101 ,  REVEAL
END-PACKAGE
