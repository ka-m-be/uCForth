\ rty3.fs - package RTY3: the IR frame of the Hisense remote control RCH-RTY3-0
\
\ The remote control RCH-RTY3-0 (air conditioner Hisense Energy Pro X,
\ QH35XV4AG) sends the whole state in each frame, as KELON168 does, but
\ in a frame of its own:
\
\   $60, the type ($38: the state), n (19), n data bytes, their XOR
\
\ 23 bytes = 184 bits in one block, the lowest bit first, in pulse
\ distance code (package IRCODE) on 38 kHz: a header of 9000 us light
\ and 4500 us dark, each bit 560 us light, then 540 us (0) or 1690 us
\ (1) dark. No library knows this frame: the field map comes from 
\ measurements with the transceiver:
\
\   byte 3    bit 0: on; bits 1-3: the mode (0 heat, 2 cool, 3 dry, 4 fan;
\             1 auto is not measured)
\   byte 6    the temperature: (T - 16) x 10; dry and fan: 23 C
\   byte 7    dry: the grade (0 to 2 measured); bit 5: iFeel on
\   byte 8    iFeel: the temperature at the remote control, in 0.1 C
\   byte 10   the clock: the hour; byte 11, bits 0-5: the minute
\   byte 11   bit 6: super; bit 7: not known yet (key $24)
\   byte 16   the flap (key $07): 0, 3 to 7; each mode has its own
\   byte 17   the fan (key $08): 0 to 4 seen
\   byte 19   the key of the frame
\ The frame ends with 800 us light (the remote control: 816 us).
\
\ The other bytes stay as the last frame of the remote control had them
\ (AC-TAKE). The words have the names of KELON168, so an application
\ changes only the package:
\
\   HEAT AUTO COOL DRY FAN ( -- mode )   the modes
\   AC-RESET            off, cool, 24 C, fan 0, flap 0, no clock
\   f AC-POWER!         on (true) or off
\   mode AC-MODE!       the mode; in DRY and FAN the frame has 23 C
\   c AC-TEMP!          the temperature, 16 to 30 C
\   n AC-FAN!           the fan: 0 to 7, the code as the remote control sends it
\   n AC-FLAP!          the flap: 0 to 7, the code as the remote control sends it
\   h m AC-CLOCK!       the clock of the frame is h:m now, and runs (32-bit
\                       cells); without it the frame has 0:00
\   AC-FRAME ( -- a )   the 23 bytes of the state, with clock and checksum
\   AC-TIMINGS ( -- t m )   the frame as m times in us at t, for a sender
\   t m AC-DECODE ( -- a bits )   received times as bytes at a
\   a AC-OK? ( -- f )   the 23 bytes at a are a frame of the state
\   a u AC-CHECK? ( -- f )   the u bytes at a are a frame of any type:
\                       $60, the length, the XOR (the short frames too)
\   a AC-TAKE           the frame at a is the state now, with its clock
\   .AC                 the state in words
\   a .FRAME            23 bytes at a in hex
\
\ Load (it needs the package IRCODE):
\   on the host:   ALSO FILES  S" lib/irremote/ircode.fs" INCLUDED
\                  S" lib/irremote/rty3.fs" INCLUDED
\   in ucf term:   #require IRCODE lib/irremote/ircode.fs
\                  #include lib/irremote/rty3.fs
\ Example:
\
\   ALSO RTY3
\   TRUE AC-POWER!  HEAT AC-MODE!  23 AC-TEMP!  .AC
\   on, heat, 23 C, fan 0, flap 0, clock not set
\   AC-FRAME .FRAME
\   60 38 13 01 FF 00 46 00 00 00 00 00 00 00 00 00 00 00 00 02 00 00 BA

PACKAGE RTY3  ALSO IRCODE   \ lib/package.fs

23 CONSTANT #BYTES
#BYTES BUFFER: FRAME                \ the frame of the state

\ ---- fields: byte, first bit, width
: FIELD ( byte bit width "name" -- )   CREATE , , , ;   \ name ( -- field )
: >INDEX ( field -- i )   2 CELLS + @ ;
: >BYTE ( field -- a )    >INDEX FRAME + ;
: >SHIFT ( field -- n )   CELL+ @ ;
: >MASK ( field -- m )    DUP @  1 SWAP LSHIFT 1-  SWAP >SHIFT LSHIFT ;
: FIELD@ ( field -- x )   DUP >BYTE C@  OVER >MASK AND  SWAP >SHIFT RSHIFT ;
: FIELD! ( x field -- )   \ a flag TRUE gives 1
  >R  R@ >SHIFT LSHIFT  R@ >MASK AND
  R@ >BYTE C@  R@ >MASK INVERT AND  OR  R> >BYTE C! ;

 3 0 1 FIELD POWER-BIT
 3 1 3 FIELD MODE-BITS
 6 0 8 FIELD TEMP-CODE              \ (T - 16) x 10
 7 0 5 FIELD DRY-GRADE
 7 5 1 FIELD IFEEL
 8 0 8 FIELD IFEEL-TEMP             \ 0.1 C
10 0 8 FIELD CLOCK-H
11 0 6 FIELD CLOCK-M
11 6 1 FIELD SUPER-BIT
16 0 8 FIELD FLAP-CODE              \ 0, 3 to 7
17 0 8 FIELD FAN-CODE               \ 0 to 4 seen
19 0 8 FIELD KEY-CODE

\ ---- the keys of the remote control, for byte 19
$25 CONSTANT K-POWER   $06 CONSTANT K-MODE   $01 CONSTANT K-UP   $02 CONSTANT K-DOWN
$07 CONSTANT K-FLAP    $08 CONSTANT K-FAN
: KEY! ( k -- )   KEY-CODE FIELD! ;

\ ---- the temperature: DRY and FAN carry 23 C; the set value stays here
VARIABLE SET-TEMP
: FIXED-TEMP? ( -- f )   MODE-BITS FIELD@ 3 5 WITHIN ;
: TEMP>FRAME ( -- )   FIXED-TEMP? IF 23 ELSE SET-TEMP @ THEN  16 - 10 * TEMP-CODE FIELD! ;

\ ---- the clock: the minute of the day when it was set, and MILLIS then
VARIABLE CLOCK-SET   VARIABLE CLOCK-MIN   VARIABLE CLOCK-MS
: NOW ( -- min )   \ the minute of the day; 0 if the clock is not set
  CLOCK-SET @ 0= IF 0 EXIT THEN
  MILLIS CLOCK-MS @ -  0 60000 UM/MOD NIP  CLOCK-MIN @ +  1440 MOD ;
: SET-CLOCK ( min -- )   CLOCK-MIN !  MILLIS CLOCK-MS !  TRUE CLOCK-SET ! ;
: ?TIME ( h m -- h m )
  OVER 0 24 WITHIN  OVER 0 60 WITHIN AND 0= ABORT" RTY3: the time is h 0-23, m 0-59" ;

\ ---- the checksum: the XOR of the data bytes
: XOR-BYTES ( a u -- c )   0 ROT ROT  OVER + SWAP ?DO  I C@ XOR  LOOP ;

\ ---- the times of the code (package IRCODE): one block of 23 bytes
CREATE RTY3-BLOCKS  1 C, 23 C,
9000 4500 560 1690 540 0 RTY3-BLOCKS SHAPE RTY3-CODE
800 CONSTANT STOP-MARK              \ the last light of a frame
380 CELLS BUFFER: TIMES             \ a frame has 371 times
64 CONSTANT #IN
#IN BUFFER: FRAME-IN                \ the bytes of a received frame

\ ---- words for .AC
: .ON ( f -- )   IF ." on" ELSE ." off" THEN ;
: .2 ( n -- )   0 <# # # #> TYPE ;
: .HM ( min -- )   60 /MOD .2 ." :" .2 ;
: .MODE ( mode -- )
  CASE  0 OF ." heat" ENDOF  1 OF ." auto" ENDOF  2 OF ." cool" ENDOF
        3 OF ." dry" ENDOF   4 OF ." fan" ENDOF  ." mode " DUP 0 .R
  ENDCASE ;

PUBLIC
0 CONSTANT HEAT   1 CONSTANT AUTO   2 CONSTANT COOL   3 CONSTANT DRY   4 CONSTANT FAN

: AC-POWER! ( f -- )   POWER-BIT FIELD!  K-POWER KEY! ;
: AC-MODE! ( mode -- )
  DUP 0 5 WITHIN 0= ABORT" RTY3: the modes are 0 to 4 (HEAT AUTO COOL DRY FAN)"
  DUP MODE-BITS FIELD!  TEMP>FRAME
  DRY <> IF  0 DRY-GRADE FIELD!  THEN
  K-MODE KEY! ;
: AC-TEMP! ( c -- )
  DUP 16 31 WITHIN 0= ABORT" RTY3: the temperature is 16 to 30 C"
  DUP SET-TEMP @ < IF K-DOWN ELSE K-UP THEN  SWAP SET-TEMP !  TEMP>FRAME  KEY! ;
: AC-FAN! ( n -- )
  DUP 0 8 WITHIN 0= ABORT" RTY3: the fan is 0 to 7"
  FAN-CODE FIELD!  K-FAN KEY! ;
: AC-FLAP! ( n -- )
  DUP 0 8 WITHIN 0= ABORT" RTY3: the flap is 0 to 7"
  FLAP-CODE FIELD!  K-FLAP KEY! ;
: AC-CLOCK! ( h m -- )   ?TIME  SWAP 60 * +  SET-CLOCK ;

: AC-RESET ( -- )
  FRAME #BYTES 0 FILL  $60 FRAME C!  $38 FRAME 1+ C!  $13 FRAME 2 + C!
  $FF FRAME 4 + C!                                   \ as the remote control sends it
  24 SET-TEMP !  COOL MODE-BITS FIELD!  TEMP>FRAME
  0 CLOCK-SET !  K-POWER KEY! ;

: AC-FRAME ( -- a )
  CLOCK-SET @ IF  NOW 60 /MOD CLOCK-H FIELD!  CLOCK-M FIELD!  THEN
  FRAME 3 + 19 XOR-BYTES  FRAME 22 + C!  FRAME ;
: AC-TIMINGS ( -- t m )
  RTY3-CODE  AC-FRAME #BYTES TIMES >TIMINGS                 ( m )
  STOP-MARK OVER 1- CELLS TIMES + !  TIMES SWAP ;
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
: .FRAME ( a -- )   #BYTES 0 DO  I IF SPACE THEN  DUP I + C@ .H2  LOOP  DROP CR ;

AC-RESET
END-PACKAGE
