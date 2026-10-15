\ aircon.fs - package AIRCON: the words of the application aircon
\
\ The start script apps/aircon/app.fs loads this file and the
\ packages that it needs (RMT, IR, AC-BASE, IRCODE, IRCODE-READ, RTY3,
\ RTY3-READ); its README has the words and the wiring. The pins: IR-OUT
\ and IR-IN below.

ONLY FORTH DEFINITIONS
PACKAGE AIRCON  ALSO AC-BASE  ALSO RTY3  ALSO RTY3-READ  ALSO IR   \ lib/package.fs

4 CONSTANT IR-OUT                   \ GPIO 4: IRout of the transceiver (green wire)
14 CONSTANT IR-IN                   \ GPIO 14: IRin (white wire)

: TAKE ( a n -- )   \ a frame of the remote control becomes the state
  AC-DECODE 184 = IF  DUP AC-OK? IF AC-TAKE EXIT THEN  THEN  DROP ;
64 BUFFER: LAST                     \ the last frame that LISTEN showed
VARIABLE LAST-U                     \ its length in bytes
: .CHANGES ( a u -- )   \ the bytes that differ from the last frame of the same length
  DUP 0=  OVER LAST-U @ <>  OR IF 2DROP EXIT THEN          \ no byte: no loop over u-1
  ." changes:"  1- 0 ?DO                           \ not the checksum
    DUP I + C@  LAST I + C@  2DUP <> IF  ."  byte " I 0 .R ." : " .H2 ." >" .H2  ELSE 2DROP THEN
  LOOP  DROP CR ;
: REMEMBER ( a u -- )   DUP LAST-U !  LAST SWAP MOVE ;
: SHOW ( a n -- )   \ a frame for LISTEN: bits, bytes, check, changes, the state
  AC-DECODE                                        ( a bits )
  ." frame: " DUP 0 .R ."  bits"
  8 / 64 MIN                                       ( a u )
  2DUP AC-CHECK? IF ." , checksum right" ELSE ." , checksum wrong" THEN CR
  2DUP .BYTES  2DUP .CHANGES  2DUP REMEMBER
  DROP DUP AC-OK? IF AC-TAKE .AC ELSE DROP THEN ;
: WAIT-KEY ( -- )   BEGIN KEY? UNTIL  KEY DROP ;

PUBLIC
HEAT CONSTANT HEAT   AUTO CONSTANT AUTO   COOL CONSTANT COOL
DRY CONSTANT DRY     FAN CONSTANT FAN

: SEND ( -- )         AC-TIMINGS IR-SEND ;
: AIRCON-ON ( -- )    TRUE AC-POWER!  SEND ;
: AIRCON-OFF ( -- )   FALSE AC-POWER!  SEND ;
: MODE ( mode -- )    AC-MODE!  SEND ;
: TEMP ( c -- )       AC-TEMP!  SEND ;
: SPEED ( n -- )      AC-FAN!  SEND ;
: FLAP ( n -- )       AC-FLAP!  SEND ;
: CLOCK! ( h m -- )   AC-CLOCK! ;
: .AC ( -- )          .AC  AC-FRAME .FRAME ;
: AIRCON-START ( -- )   \ the pins, and the board takes each frame of the remote control
  IR-OUT IR-IN IR-PINS  ['] TAKE IR-LISTEN ;
: LISTEN ( -- )   \ show each frame of the remote control; the return key ends (or Ctrl-C)
  ." listening; the return key ends" CR
  0 LAST-U !  ['] SHOW IR-LISTEN  ['] WAIT-KEY CATCH  ['] TAKE IR-LISTEN  THROW ;
: AIRCON-HELP ( -- )
  ." AIRCON-ON  AIRCON-OFF        on, off" CR
  ." HEAT MODE  (COOL DRY FAN)     the mode" CR
  ." 23 TEMP                      the temperature, 16 to 30 C" CR
  ." 3 SPEED                      the fan: 0 auto, 1 to 5" CR
  ." 2 FLAP                       the flap: 0 to 4" CR
  ." 21 45 CLOCK!                 the clock of the frame" CR
  ." .AC  SEND  LISTEN            the state; send it again; frames of the remote" CR
  ." AIRCON-START                 set up again (after Ctrl-C twice or a restart)" CR ;

END-PACKAGE
