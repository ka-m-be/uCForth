\ test.fs - the tests of uCForth on a PC with a BIOS, over COM1
\
\ QEMU runs build/ucf-pc.img; its serial line COM1 is the pair of FIFOs
\ build/ser.in and build/ser.out. uCForth on the host sends the test
\ files with the package SEND (host/send.fs), line by line after each
\ prompt. The exit status is 0 when no line with "error:" came back.
\ See README.md, section "Test".

ALSO FILES  S" host/send.fs" INCLUDED

: MAIN ( -- )
  S" build/ser.in" S" build/ser.out" CONNECT
  S" forth/test/tester.fs" SEND-FILE
  S" forth/test/t0.fs"     SEND-FILE
  S" forth/test/hayes.fs"  SEND-FILE
  S" lib/pc/test/bios.fs"  SEND-FILE
  S" lib/text.fs"          SEND-FILE    \ CODE words: TEXT, I8086, ASM, their test
  S" asm/8086.fs"          SEND-FILE
  S" lib/pc/asm.fs"        SEND-FILE
  S" lib/pc/test/asm.fs"   SEND-FILE
  S" asm/monitor/monitor.fs"   SEND-FILE    \ the monitor: MONITOR, DISX86, MON-PC, its test
  S" asm/monitor/disx86.fs"    SEND-FILE
  S" asm/monitor/monpc.fs"     SEND-FILE
  S" asm/monitor/test/monpc.fs" SEND-FILE
  S" forth/test/all.fs"    SEND-FILE
  CR  ERRORS @ 0<> 1 AND (BYE) ;
MAIN
