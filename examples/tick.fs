\ tick.fs - an application: RUN runs 'SETUP once, then 'LOOP again and again
\
\ Build it into a program, for example for Linux on aarch64:
\   LC_ALL=C awk -f bootstrap/awk/ucf.awk -- -32 host/files.fs examples/tick.fs targets/linux-a64/build.fs
\ The program starts RUN after 1 s. ESC or Ctrl-C in this second gives the
\ prompt instead.

VARIABLE TICKS
: SETUP ( -- )   0 TICKS !  ." start" CR ;
: STEP  ( -- )   1 TICKS +!  ." tick " TICKS @ . CR  1000 MS ;
' SETUP 'SETUP !   ' STEP 'LOOP !   TURNKEY
