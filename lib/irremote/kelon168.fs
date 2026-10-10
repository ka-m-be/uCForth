\ kelon168.fs - package KELON168: the IR frame of Hisense air conditioners
\
\ Some remote controls of Hisense air conditioners (DG11R2-01, RCH-R0Y3)
\ send the whole state in each frame: 21 bytes = 168 bits, in blocks of
\ 6, 8 and 7 bytes, in pulse distance code (package IRCODE) on a carrier
\ of 38 kHz. IRremoteESP8266 calls the code KELON168 (Kelon belongs to
\ Hisense). Not all do: the remote control RCH-RTY3-0 sends 23 bytes
\ that begin with 60 38 13 05 (archive/plan/002_Plan_Fassung73.md, 23.12). A frame sets all at once: on or off, the mode, the
\ temperature, the fan and so on. So the air conditioner does what the
\ last frame says, and this package keeps such a frame: the state. Each
\ AC- word (AC: air conditioner) changes the state and names its key in
\ the frame, as the remote control does. Runs on every target; the clock
\ runs only with 32-bit cells.
\
\   HEAT AUTO COOL DRY FAN ( -- mode )   the modes
\   AC-RESET            the state after the load: off, COOL, 24 C, fan auto,
\                       swing, sleep and super off, display on, no clock
\   f AC-POWER!         on (true) or off: an absolute bit, not a toggle
\   mode AC-MODE!       the mode; in DRY and FAN the frame has 23 C
\   c AC-TEMP!          the set temperature, 16 to 30 C
\   n AC-FAN!           the fan: 0 auto, 1 the lowest to 5 the highest speed
\   f AC-SWING!   f AC-SLEEP!   f AC-SUPER!   f AC-DISPLAY!   on or off
\   h m AC-CLOCK!       the clock of the frame is h:m now, and runs; without
\                       it the frame says 0:00
\   h m AC-ON-TIMER!    switch on at h:m; needs the clock
\   h m AC-OFF-TIMER!   switch off at h:m; needs the clock
\   AC-NO-TIMERS        no timers
\   AC-FRAME ( -- a )   the 21 bytes of the state, with clock and checksums
\   AC-TIMINGS ( -- t m )   the frame as m times in us at t, for a sender
\   t m AC-DECODE ( -- a bits )   received times as bytes at a; bits: 168
\                       for a frame of this code
\   a AC-OK? ( -- f )   the 21 bytes at a have the preamble and checksums
\   a AC-TAKE           the frame at a is the state now (from the remote
\                       control: its clock too)
\   .AC                 the state in words
\   a .FRAME            21 bytes at a in hex
\   a .UNKNOWN          the bits at a that the field map does not explain
\   byte bit FAN2-AT    the third bit of the fan: 17 6 (as measured, the
\                       default) or 16 1 (as in IRremoteESP8266)
\
\ A timer goes out in each frame until its time has come (as on the
\ remote control); then it goes away.
\
\ The field map comes from Kai's notes (Klima_IR/Doc/protokoll-hisense-
\ kelon168.md), checked with 361 frames of the remote control DG11R2-01.
\ Open there: the place of the third bit of the fan (FAN2-AT), and what
\ some bits mean (.UNKNOWN shows them).
\
\ Load (it needs the package IRCODE):
\   on the host:   ALSO FILES  S" lib/irremote/ircode.fs" INCLUDED
\                  S" lib/irremote/kelon168.fs" INCLUDED
\   in ucf term:   #require IRCODE lib/irremote/ircode.fs
\                  #include lib/irremote/kelon168.fs
\ Example:
\
\   ALSO KELON168
\   TRUE AC-POWER!  HEAT AC-MODE!  23 AC-TEMP!  .AC
\   on, heat, 23 C, fan auto
\   swing off, sleep off, super off, display on, clock not set
\   AC-FRAME .FRAME
\   83 06 00 70 00 00 80 00 00 00 00 00 00 F0 00 02 00 00 38 00 3A

PACKAGE KELON168  ALSO IRCODE   \ lib/package.fs

21 CONSTANT #BYTES
#BYTES BUFFER: FRAME                \ the frame of the state

\ ---- fields: byte, first bit, width (Kai's field map, section 4)
: FIELD ( byte bit width "name" -- )   CREATE , , , ;   \ name ( -- field )
: >INDEX ( field -- i )   2 CELLS + @ ;
: >BYTE ( field -- a )    >INDEX FRAME + ;
: >SHIFT ( field -- n )   CELL+ @ ;
: >MASK ( field -- m )    DUP @  1 SWAP LSHIFT 1-  SWAP >SHIFT LSHIFT ;
: FIELD@ ( field -- x )   DUP >BYTE C@  OVER >MASK AND  SWAP >SHIFT RSHIFT ;
: FIELD! ( x field -- )   \ a flag TRUE gives 1
  >R  R@ >SHIFT LSHIFT  R@ >MASK AND
  R@ >BYTE C@  R@ >MASK INVERT AND  OR  R> >BYTE C! ;

 2 0 2 FIELD FAN-LOW                \ the two low bits of the fan
 2 2 1 FIELD POWER-KEY              \ 1: the key of the frame is on/off
 2 3 1 FIELD SLEEP-BIT
 2 4 3 FIELD DRY-GRADE              \ DRY: sign (bit 2) and amount (bits 0-1)
 2 7 1 FIELD SWING1
 3 0 3 FIELD MODE-BITS
 3 4 4 FIELD TEMP-BITS              \ the temperature - 16
 5 4 1 FIELD SUPER1
 5 7 1 FIELD SUPER2                 \ always as SUPER1
 6 0 5 FIELD CLOCK-H
 6 5 1 FIELD DARK                   \ 1: the display is off
 6 7 1 FIELD BIT67                  \ 1 in each frame; its meaning is not known
 7 0 6 FIELD CLOCK-M
 7 7 1 FIELD OFF-SET                \ the off timer is on
 8 0 5 FIELD OFF-H
 8 6 1 FIELD SWING2                 \ always as SWING1
 9 0 6 FIELD OFF-M
 9 7 1 FIELD ON-SET                 \ the on timer is on
10 0 5 FIELD ON-H
11 0 6 FIELD ON-M
15 0 8 FIELD KEY-CODE               \ the key of the frame
16 2 3 FIELD DRY-EXTRA
17 6 1 FIELD FAN-HIGH               \ the third bit of the fan (FAN2-AT)
18 4 1 FIELD POWER-BIT              \ the state: 1 on

\ ---- the keys of the remote control, for byte 15
$00 CONSTANT K-DISPLAY   $01 CONSTANT K-POWER     $02 CONSTANT K-TEMP
$03 CONSTANT K-SLEEP     $04 CONSTANT K-SUPER     $05 CONSTANT K-ON-TIMER
$06 CONSTANT K-MODE      $07 CONSTANT K-SWING     $11 CONSTANT K-FAN
$1D CONSTANT K-OFF-TIMER
: KEY! ( k -- )   DUP KEY-CODE FIELD!  K-POWER = POWER-KEY FIELD! ;

\ ---- the fan: 0 auto, 1 the lowest ... 5 the highest speed; the code
\ ---- of each: bits 0-1 go to FAN-LOW, bit 2 to FAN-HIGH
CREATE FAN-CODES  0 C, 3 C, 7 C, 2 C, 5 C, 1 C,
: FAN! ( n -- )   FAN-CODES + C@  DUP 3 AND FAN-LOW FIELD!  2 RSHIFT FAN-HIGH FIELD! ;
: FAN@ ( -- n )   \ 0 (auto) for a code that is not in the list
  FAN-LOW FIELD@  FAN-HIGH FIELD@ 2 LSHIFT OR  0
  6 0 DO  OVER I FAN-CODES + C@ = IF DROP I THEN  LOOP  NIP ;

\ ---- the temperature: DRY and FAN carry 23 C; the set value stays here
VARIABLE SET-TEMP
: FIXED-TEMP? ( -- f )   MODE-BITS FIELD@ 3 5 WITHIN ;
: TEMP>FRAME ( -- )   FIXED-TEMP? IF 23 ELSE SET-TEMP @ THEN  16 - TEMP-BITS FIELD! ;

\ ---- the clock: the minute of the day when it was set, and MILLIS then
VARIABLE CLOCK-SET   VARIABLE CLOCK-MIN   VARIABLE CLOCK-MS
VARIABLE ON-DUE   VARIABLE OFF-DUE      \ MILLIS when a timer has come
: NOW ( -- min )   \ the minute of the day; 0 if the clock is not set
  CLOCK-SET @ 0= IF 0 EXIT THEN
  MILLIS CLOCK-MS @ -  0 60000 UM/MOD NIP  CLOCK-MIN @ +  1440 MOD ;
: SET-CLOCK ( min -- )   CLOCK-MIN !  MILLIS CLOCK-MS !  TRUE CLOCK-SET ! ;
: ?CLOCK ( -- )   CLOCK-SET @ 0= ABORT" KELON168: set the clock first (h m AC-CLOCK!)" ;
: ?TIME ( h m -- h m )
  OVER 0 24 WITHIN  OVER 0 60 WITHIN AND 0= ABORT" KELON168: the time is h 0-23, m 0-59" ;
: DUE ( h m -- ms )   \ MILLIS at the next h:m
  SWAP 60 * +  NOW -  1440 +  1440 MOD  60000 *  MILLIS + ;
: PASSED? ( ms -- f )   MILLIS SWAP - 0< 0= ;
: EXPIRE ( -- )   \ a timer whose time has come goes away
  CLOCK-SET @ 0= IF EXIT THEN
  ON-SET FIELD@ IF  ON-DUE @ PASSED? IF  0 ON-SET FIELD!  0 ON-H FIELD!  0 ON-M FIELD!  THEN  THEN
  OFF-SET FIELD@ IF  OFF-DUE @ PASSED? IF  0 OFF-SET FIELD!  0 OFF-H FIELD!  0 OFF-M FIELD!  THEN  THEN ;

\ ---- checksums: byte 13 is the XOR of bytes 2 to 12, byte 20 of bytes 14 to 19
: XOR-BYTES ( a u -- c )   0 ROT ROT  OVER + SWAP ?DO  I C@ XOR  LOOP ;
: SUM1 ( a -- c )   2 + 11 XOR-BYTES ;
: SUM2 ( a -- c )   14 + 6 XOR-BYTES ;

\ ---- the times of the code (package IRCODE)
CREATE KELON-BLOCKS  3 C, 6 C, 8 C, 7 C,
9000 4600 560 1680 600 8000 KELON-BLOCKS SHAPE KELON
350 CELLS BUFFER: TIMES             \ a frame has 343 times
64 CONSTANT #IN
#IN BUFFER: FRAME-IN                \ the bytes of a received frame

\ ---- words for .AC and .UNKNOWN
: .ON ( f -- )   IF ." on" ELSE ." off" THEN ;
: .2 ( n -- )   0 <# # # #> TYPE ;
: .HM ( min -- )   60 /MOD .2 ." :" .2 ;
: .MODE ( mode -- )
  CASE  0 OF ." heat" ENDOF  1 OF ." auto" ENDOF  2 OF ." cool" ENDOF
        3 OF ." dry" ENDOF   4 OF ." fan" ENDOF  ." mode " DUP 0 .R
  ENDCASE ;
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
0 CONSTANT HEAT   1 CONSTANT AUTO   2 CONSTANT COOL   3 CONSTANT DRY   4 CONSTANT FAN

: AC-POWER! ( f -- )   POWER-BIT FIELD!  K-POWER KEY! ;
: AC-MODE! ( mode -- )
  DUP 0 5 WITHIN 0= ABORT" KELON168: the modes are 0 to 4 (HEAT AUTO COOL DRY FAN)"
  DUP MODE-BITS FIELD!  TEMP>FRAME
  DRY <> IF  0 DRY-GRADE FIELD!  0 DRY-EXTRA FIELD!  THEN
  K-MODE KEY! ;
: AC-TEMP! ( c -- )
  DUP 16 31 WITHIN 0= ABORT" KELON168: the temperature is 16 to 30 C"
  SET-TEMP !  TEMP>FRAME  K-TEMP KEY! ;
: AC-FAN! ( n -- )
  DUP 0 6 WITHIN 0= ABORT" KELON168: the fan is 0 (auto) to 5"
  FAN!  K-FAN KEY! ;
: AC-SWING! ( f -- )     DUP SWING1 FIELD!  SWING2 FIELD!  K-SWING KEY! ;
: AC-SLEEP! ( f -- )     SLEEP-BIT FIELD!  K-SLEEP KEY! ;
: AC-SUPER! ( f -- )     DUP SUPER1 FIELD!  SUPER2 FIELD!  K-SUPER KEY! ;
: AC-DISPLAY! ( f -- )   0= DARK FIELD!  K-DISPLAY KEY! ;

: AC-CLOCK! ( h m -- )   ?TIME  SWAP 60 * +  SET-CLOCK ;
: AC-ON-TIMER! ( h m -- )
  ?CLOCK ?TIME  2DUP DUE ON-DUE !  ON-M FIELD!  ON-H FIELD!  1 ON-SET FIELD!  K-ON-TIMER KEY! ;
: AC-OFF-TIMER! ( h m -- )
  ?CLOCK ?TIME  2DUP DUE OFF-DUE !  OFF-M FIELD!  OFF-H FIELD!  1 OFF-SET FIELD!  K-OFF-TIMER KEY! ;
: AC-NO-TIMERS ( -- )
  0 ON-SET FIELD!  0 ON-H FIELD!  0 ON-M FIELD!
  0 OFF-SET FIELD!  0 OFF-H FIELD!  0 OFF-M FIELD!  K-OFF-TIMER KEY! ;

: AC-RESET ( -- )
  FRAME #BYTES 0 FILL  $83 FRAME C!  $06 FRAME 1+ C!
  1 BIT67 FIELD!  $28 FRAME 18 + C!                  \ the model: bits 0-3 and 5-7 of byte 18
  24 SET-TEMP !  COOL MODE-BITS FIELD!  TEMP>FRAME
  0 CLOCK-SET !  K-POWER KEY! ;

: AC-FRAME ( -- a )
  NOW 60 /MOD CLOCK-H FIELD!  CLOCK-M FIELD!  EXPIRE
  FRAME SUM1 FRAME 13 + C!  FRAME SUM2 FRAME 20 + C!  FRAME ;
: AC-TIMINGS ( -- t m )   KELON  AC-FRAME #BYTES TIMES >TIMINGS  TIMES SWAP ;
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
: .FRAME ( a -- )   #BYTES 0 DO  I IF SPACE THEN  DUP I + C@ .H2  LOOP  DROP CR ;
: .UNKNOWN ( a -- )
  0  #BYTES 0 DO                                    ( a n )
    OVER I UNKNOWN@  ?DUP IF
      OVER IF ." , " THEN  ." byte " I 0 .R ."  $" .H2  1+
    THEN
  LOOP  NIP  0= IF ." none" THEN  CR ;
: FAN2-AT ( byte bit -- )
  OVER 0 21 WITHIN  OVER 0 8 WITHIN AND 0= ABORT" KELON168: byte 0-20, bit 0-7"
  FAN@ >R  0 FAN-HIGH FIELD!
  FAN-HIGH CELL+ !  FAN-HIGH 2 CELLS + !  R> FAN! ;

AC-RESET
END-PACKAGE
