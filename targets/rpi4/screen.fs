\ screen.fs - the console of the Raspberry Pi on the screen (HDMI) too
\
\ At the start, ON-BOOT gets a framebuffer of 640 x 480 pixels from the
\ GPU (80 x 60 characters; the GPU scales it to the HDMI mode) and sends
\ each character to the UART and to the screen. Then it runs BOOT.
\ build.fs starts the image with ON-BOOT when the word is there.
\
\ Load before targets/freeze.fs, after lib/font8.fs, lib/fbcon.fs and
\ lib/rpi/bcm2711.fs (see README.md).

ONLY FORTH DEFINITIONS  ALSO INTERNALS  ALSO BCM2711  ALSO FBCON

VARIABLE UART-EMIT   'EMIT @ UART-EMIT !     \ the xt of TX!
: BOTH-EMIT ( c -- )   DUP UART-EMIT @ EXECUTE  FB-EMIT ;

: SCREEN-ON ( -- )   \ without a framebuffer, only the UART
  640 480 FB-ALLOC  >R >R  OVER IF
    R> R> FB-START  ['] BOTH-EMIT 'EMIT !
  ELSE  R> R> 2DROP 2DROP  THEN ;

: ON-BOOT ( -- )   SCREEN-ON  BOOT ;

ONLY FORTH DEFINITIONS
