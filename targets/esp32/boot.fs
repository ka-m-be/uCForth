\ boot.fs - the start of uCForth on the ESP32: 240 MHz, then BOOT
\
\ build.fs starts the image with ON-BOOT when the image has this word.
\ Put this file before targets/freeze.fs, after lib/esp32/esp32/clock.fs
\ (targets/esp32/README.md, "Build"). ON-BOOT sets the CPU to 240 MHz,
\ then runs BOOT.

ALSO INTERNALS  ALSO CLOCK
: ON-BOOT ( -- )   240 MHZ  BOOT ;
PREVIOUS PREVIOUS
