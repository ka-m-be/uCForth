\ test.fs - the tests of uCForth on the Raspberry Pi, over the serial line
\
\ QEMU runs kernel8.img; its serial line is the pair of FIFOs
\ build/ser.in and build/ser.out. uCForth for Linux sends the test files
\ with the package SEND (host/send.fs), line by line after each prompt.
\ The exit status is 0 when no line with "error:" came back.
\ See README.md, section "Test".

ALSO FILES  S" host/send.fs" INCLUDED

: MAIN ( -- )
  S" build/ser.in" S" build/ser.out" CONNECT
  S" forth/test/tester.fs" SEND-FILE
  S" forth/test/t0.fs"     SEND-FILE
  S" forth/test/hayes.fs"  SEND-FILE
  S" forth/test/all.fs"    SEND-FILE
  CR  ERRORS @ 0<> 1 AND (BYE) ;
MAIN
