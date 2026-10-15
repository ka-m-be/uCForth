\ rty3read.fs - package RTY3-READ: frames of RTY3 read back, and the state in words
\
\ The second part of RTY3 (rty3.fs): received times back into bytes, the
\ checks of a frame, a frame of the remote control as the state, and the
\ state in words.
\
\   t m AC-DECODE ( -- a bits )   received times as bytes at a
\   a AC-OK? ( -- f )   the 23 bytes at a are a frame of the state
\   a u AC-CHECK? ( -- f )   the u bytes at a are a frame of any type:
\                       $60, the length, the XOR (the short frames too)
\   a AC-TAKE           the frame at a is the state now, with its clock
\   .AC                 the state in words
\   a .FRAME            23 bytes at a in hex
\
\ Load after RTY3 and IRCODE-READ:
\   on the host:   ALSO FILES  S" lib/irremote/irread.fs" INCLUDED
\                  S" lib/irremote/rty3read.fs" INCLUDED
\   in ucf term:   #require IRCODE-READ lib/irremote/irread.fs
\                  #include lib/irremote/rty3read.fs
\ Example:
\
\   ALSO AC-BASE  ALSO RTY3  ALSO RTY3-READ
\   TRUE AC-POWER!  HEAT AC-MODE!  23 AC-TEMP!  .AC
\   on, heat, 23 C, fan 0, flap 0, clock not set
\   AC-FRAME .FRAME
\   60 38 13 01 FF 00 46 00 00 00 00 00 00 00 00 00 00 00 00 02 00 00 BA

PACKAGE RTY3-READ  ALSO AC-BASE  ALSO IRCODE-READ  ALSO RTY3  ALSO RTY3.I   \ lib/package.fs

64 CONSTANT #IN
#IN BUFFER: FRAME-IN                \ the bytes of a received frame

PUBLIC
: AC-DECODE ( t m -- a bits )   RTY3-CODE  FRAME-IN #IN TIMINGS>  FRAME-IN SWAP ;
: AC-CHECK? ( a u -- f )
  DUP 5 < IF 2DROP FALSE EXIT THEN
  OVER C@ $60 <> IF 2DROP FALSE EXIT THEN
  OVER 2 + C@ OVER 4 - <> IF 2DROP FALSE EXIT THEN   ( a u )
  OVER 3 + OVER 4 - XOR-BYTES  ROT ROT + 1- C@ = ;
: AC-OK? ( a -- f )
  DUP 1+ C@ $38 =  OVER 2 + C@ $13 = AND  SWAP #BYTES AC-CHECK? AND ;
: AC-TAKE ( a -- )
  FRAME #BYTES MOVE
  FIXED-TEMP? 0= IF  TEMP-CODE FIELD@ 10 / 16 +  SET-TEMP !  THEN
  CLOCK-H FIELD@ 60 *  CLOCK-M FIELD@ +  1440 MOD  SET-CLOCK ;

: .AC ( -- )
  POWER-BIT FIELD@ .ON  ." , "  MODE-BITS FIELD@ .MODE
  FIXED-TEMP? 0= IF  ." , " TEMP-CODE FIELD@ 10 /MOD 16 + 0 .R
    ?DUP IF ." ." 0 .R THEN  ."  C"  THEN
  MODE-BITS FIELD@ DRY = IF  ." , grade " DRY-GRADE FIELD@ 0 .R  THEN
  ." , fan " FAN-CODE FIELD@ 0 .R
  ." , flap " FLAP-CODE FIELD@ 0 .R
  ." , clock " CLOCK-SET @ IF NOW .HM ELSE ." not set" THEN
  SUPER-BIT FIELD@ IF ." , super" THEN
  IFEEL FIELD@ IF  ." , iFeel " IFEEL-TEMP FIELD@ 10 /MOD 0 .R ." ." 0 .R ."  C"  THEN  CR ;
: .FRAME ( a -- )   #BYTES .BYTES ;

END-PACKAGE
