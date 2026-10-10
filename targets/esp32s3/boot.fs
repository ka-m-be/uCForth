\ boot.fs - the start of uCForth on the ESP32-S3: 240 MHz, the PSRAM, then BOOT
\
\ build.fs starts the image with ON-BOOT when the image has this word
\ (as targets/rpi4/screen.fs). Put this file before targets/freeze.fs,
\ after lib/esp32/esp32s3/clock.fs, psram.fs and bootimg.fs
\ (targets/esp32s3/README.md, "Build"). ON-BOOT sets the CPU to 240 MHz,
\ switches the PSRAM on, puts the RAM data that FREEZE saved back into
\ it, then runs BOOT. A chip without PSRAM starts too: then PSRAM-SIZE
\ is 0, and the RAM data stay inside the chip.

ALSO INTERNALS  ALSO CLOCK  ALSO PSRAM  ALSO BOOTIMG
: ON-BOOT ( -- )   240 MHZ  ['] PSRAM-ON CATCH DROP  ['] XDATA-LOAD CATCH DROP  BOOT ;
PREVIOUS PREVIOUS PREVIOUS PREVIOUS
