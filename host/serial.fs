\ serial.fs - package SERIAL: the serial port of the host
\
\ One port at a time: a USB serial line to a board, for example
\ /dev/cu.usbserial-1110 on a Mac. The port is raw: 8 data bits, no
\ parity, no flow control. The host supplies the HAL primitives 94-96
\ (doc/vm.md, section 8: targets/macos-a64, targets/macos-x64,
\ targets/win32, targets/linux-a64, targets/linux-x64,
\ targets/linux-i386). Needs the packages FILES and TEXT.
\
\   a u OPEN-PORT       open the port a u with BAUD (a VALUE, 115200)
\   CLOSE-PORT          close it
\   a u SEND            send u bytes
\   RECV ( -- c | -1 )  a byte, or -1 when none is there (no wait)
\   ms RECV-WAIT ( -- c | -1 )   wait up to ms for a byte
\   ms DRAIN            read and drop the input for ms
\   f DTR!  f RTS!      the control lines; LINES? false: the port has none
\   PULSE-RTS           restart a board whose EN pin hangs at RTS (ESP32)
\ DTR! and RTS! change one line and keep the other one.

PACKAGE SERIAL  ALSO FILES  ALSO TEXT   \ lib/package.fs

PRIVATE
: PRIM ( n "name" -- )   PARSE-NAME HEADER ,  REVEAL ;   \ a header for the primitive n
94 PRIM SERIAL-OPEN    ( a u baud -- fid ior )
95 PRIM SERIAL-LINES   ( dtr rts fid -- ior )
96 PRIM SERIAL-BAUD    ( baud fid -- ior )

256 BUFFER: PORT-NAME               \ counted string
256 BUFFER: RX   VARIABLE RX-N   VARIABLE RX-I     \ bytes read ahead
VARIABLE DTR   VARIABLE RTS         \ the state of the control lines

PUBLIC
VARIABLE PORT                       \ fid of the open port; 0: none
115200 VALUE BAUD
: PORT$ ( -- a u )  PORT-NAME COUNT ;

PRIVATE
: (OPEN) ( -- ior )  PORT$ BAUD SERIAL-OPEN  SWAP PORT ! ;
: REOPEN ( -- )   \ a USB port can vanish for a moment at a reset: try for 2 s
  PORT @ ?DUP IF CLOSE-FILE DROP THEN  0 PORT !  MILLIS 2000 +
  BEGIN (OPEN) WHILE
    DUP MILLIS - 0< IF DROP  S" the port is gone: " PORT$ FAIL THEN  100 MS
  REPEAT  DROP  0 RX-N !  0 RX-I ! ;
: LINES! ( -- ior )  DTR @ RTS @ PORT @ SERIAL-LINES ;

PUBLIC
: CLOSE-PORT ( -- )  PORT @ ?DUP IF CLOSE-FILE DROP  0 PORT ! THEN ;
: OPEN-PORT ( a u -- )   \ close the port that is open, open a u
  CLOSE-PORT  PORT-NAME PLACE  0 RX-N !  0 RX-I !
  DTR ON!  RTS ON!                  \ the host sets both lines when it opens the port
  (OPEN) IF  0 PORT !  S" cannot open " PORT$ FAIL  THEN ;

: SEND ( a u -- )
  PORT @ 0= ABORT" no port is open"
  2DUP PORT @ WRITE-FILE IF  REOPEN  PORT @ WRITE-FILE ABORT" cannot write to the port"
  ELSE 2DROP THEN ;
: RECV ( -- c | -1 )
  RX-I @ RX-N @ = IF
    RX 256 PORT @ READ-FILE IF DROP REOPEN -1 EXIT THEN
    DUP RX-N !  0 RX-I !  0= IF -1 EXIT THEN
  THEN  RX RX-I @ + C@  1 RX-I +! ;
: RECV-WAIT ( ms -- c | -1 )
  MILLIS +
  BEGIN RECV DUP 0< WHILE
    DROP  DUP MILLIS - 0< IF DROP -1 EXIT THEN  1 MS
  REPEAT  NIP ;
: DRAIN ( ms -- )
  MILLIS +  BEGIN RECV 0< IF 1 MS THEN  DUP MILLIS - 0< UNTIL  DROP ;

: LINES? ( -- f )  LINES! 0= ;
: DTR! ( f -- )  DTR !  LINES! DROP ;
: RTS! ( f -- )  RTS !  LINES! DROP ;
: PULSE-RTS ( -- )
  FALSE RTS!  FALSE DTR!            \ RTS first: DTR 0 with RTS 1 is already a reset
  TRUE RTS!  100 MS  FALSE RTS! ;
: SET-BAUD ( n -- )   \ a new speed for the open port
  DUP TO BAUD  PORT @ SERIAL-BAUD ABORT" cannot set the speed" ;

END-PACKAGE
