\ kelonrd.fs - package KELON168-READ: frames of KELON168 read back, and the state in words
\
\ The second part of KELON168 (kelon168.fs): received times back into
\ bytes, the check of a frame, a frame of the remote control as the
\ state, and the state in words.
\
\   t m AC-DECODE ( -- a bits )   received times as bytes at a; bits: 168
\                       for a frame of this code
\   a AC-OK? ( -- f )   the 21 bytes at a have the preamble and checksums
\   a AC-TAKE           the frame at a is the state now (from the remote
\                       control: its clock too)
\   .AC                 the state in words
\   a .FRAME            21 bytes at a in hex
\   a .UNKNOWN          the bits at a that the field map does not explain
\
\ Load after KELON168 and IRCODE-READ:
\   on the host:   ALSO FILES  S" lib/irremote/irread.fs" INCLUDED
\                  S" lib/irremote/kelonrd.fs" INCLUDED
\   in ucf term:   #require IRCODE-READ lib/irremote/irread.fs
\                  #include lib/irremote/kelonrd.fs
\ Example:
\
\   ALSO AC-BASE  ALSO KELON168  ALSO KELON168-READ
\   TRUE AC-POWER!  HEAT AC-MODE!  23 AC-TEMP!  .AC
\   on, heat, 23 C, fan auto
\   swing off, sleep off, super off, display on, clock not set
\   AC-FRAME .FRAME
\   83 06 00 70 00 00 80 00 00 00 00 00 00 F0 00 02 00 00 38 00 3A

PACKAGE KELON168-READ  ALSO AC-BASE  ALSO IRCODE-READ  ALSO KELON168  ALSO KELON168.I   \ lib/package.fs

64 CONSTANT #IN
#IN BUFFER: FRAME-IN                \ the bytes of a received frame

\ ---- words for .AC and .UNKNOWN
: GRADE ( -- n )   \ the grade of DRY, -2 to 2
  DRY-GRADE FIELD@  DUP 3 AND  SWAP 4 AND IF NEGATE THEN ;
CREATE KNOWN   \ for each byte: the bits of the field map
  $FF C, $FF C, $FF C, $F7 C, $00 C, $90 C, $BF C, $BF C, $5F C, $BF C, $1F C,
  $3F C, $00 C, $FF C, $00 C, $FF C, $1C C, $00 C, $FF C, $00 C, $FF C,
: KNOWN@ ( i -- m )   \ with the third bit of the fan, where it is now
  DUP KNOWN + C@  SWAP FAN-HIGH >INDEX = IF  FAN-HIGH >MASK OR  THEN ;
: UNKNOWN@ ( a i -- bits )   \ the bits of byte i at a that the map does not explain
  DUP >R + C@  R> KNOWN@ INVERT AND ;

PUBLIC
: AC-DECODE ( t m -- a bits )   KELON  FRAME-IN #IN TIMINGS>  FRAME-IN SWAP ;
: AC-OK? ( a -- f )
  DUP C@ $83 =  OVER 1+ C@ $06 = AND  >R
  DUP SUM1 OVER 13 + C@ =  SWAP DUP SUM2 SWAP 20 + C@ =  AND  R> AND ;
: AC-TAKE ( a -- )
  FRAME #BYTES MOVE
  FIXED-TEMP? 0= IF  TEMP-BITS FIELD@ 16 +  SET-TEMP !  THEN
  CLOCK-H FIELD@ 60 *  CLOCK-M FIELD@ +  1440 MOD  SET-CLOCK
  ON-SET FIELD@ IF  ON-H FIELD@ ON-M FIELD@ DUE ON-DUE !  THEN
  OFF-SET FIELD@ IF  OFF-H FIELD@ OFF-M FIELD@ DUE OFF-DUE !  THEN ;

: .AC ( -- )
  POWER-BIT FIELD@ .ON  ." , "  MODE-BITS FIELD@ .MODE
  FIXED-TEMP? 0= IF  ." , " TEMP-BITS FIELD@ 16 + 0 .R ."  C"  THEN
  MODE-BITS FIELD@ DRY = GRADE AND IF  ." , grade " GRADE 0 .R  THEN
  ." , fan " FAN@ ?DUP IF 0 .R ELSE ." auto" THEN  CR
  ." swing " SWING1 FIELD@ .ON  ." , sleep " SLEEP-BIT FIELD@ .ON
  ." , super " SUPER1 FIELD@ .ON  ." , display " DARK FIELD@ 0= .ON
  ." , clock " CLOCK-SET @ IF NOW .HM ELSE ." not set" THEN
  ON-SET FIELD@ IF  ." , on at " ON-H FIELD@ 60 * ON-M FIELD@ + .HM  THEN
  OFF-SET FIELD@ IF  ." , off at " OFF-H FIELD@ 60 * OFF-M FIELD@ + .HM  THEN  CR ;
: .FRAME ( a -- )   #BYTES .BYTES ;
: .UNKNOWN ( a -- )
  0  #BYTES 0 DO                                    ( a n )
    OVER I UNKNOWN@  ?DUP IF
      OVER IF ." , " THEN  ." byte " I 0 .R ."  $" .H2  1+
    THEN
  LOOP  NIP  0= IF ." none" THEN  CR ;

END-PACKAGE
