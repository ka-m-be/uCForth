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
\ runs only with 32-bit cells. The modes HEAT AUTO COOL DRY FAN are in
\ AC-BASE.
\
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
\   AC-POWER@ ( -- f )   AC-MODE@ ( -- mode )   AC-TEMP@ ( -- c )
\   AC-FAN@ ( -- n )    the state as the words above set it
\   AC-FRAME ( -- a )   the 21 bytes of the state, with clock and checksums
\   AC-TIMINGS ( -- t m )   the frame as m times in us at t, for a sender
\   a u AC>TIMES ( -- t m )   the same for the u bytes at a (at most 21)
\   byte bit FAN2-AT    the third bit of the fan: 17 6 (as measured, the
\                       default) or 16 1 (as in IRremoteESP8266)
\
\ A timer goes out in each frame until its time has come (as on the
\ remote control); then it goes away. This package only makes frames.
\ Frames back into bytes, their check, a frame as the state and the
\ state in words: package KELON168-READ (kelonrd.fs).
\
\ The field map comes from Kai's notes (Klima_IR/Doc/protokoll-hisense-
\ kelon168.md), checked with 361 frames of the remote control DG11R2-01.
\ Open there: the place of the third bit of the fan (FAN2-AT), and what
\ some bits mean (.UNKNOWN of KELON168-READ shows them).
\
\ Load (it needs the packages AC-BASE and IRCODE):
\   on the host:   ALSO FILES  S" lib/irremote/acbase.fs" INCLUDED
\                  S" lib/irremote/ircode.fs" INCLUDED
\                  S" lib/irremote/kelon168.fs" INCLUDED
\   in ucf term:   #require AC-BASE lib/irremote/acbase.fs
\                  #require IRCODE lib/irremote/ircode.fs
\                  #include lib/irremote/kelon168.fs
\ Example:
\
\   ALSO AC-BASE  ALSO KELON168
\   TRUE AC-POWER!  HEAT AC-MODE!  23 AC-TEMP!
\   AC-FRAME 21 .BYTES
\   83 06 00 70 00 00 80 00 00 00 00 00 00 F0 00 02 00 00 38 00 3A

PACKAGE KELON168  ALSO AC-BASE  ALSO IRCODE   \ lib/package.fs

PROTECTED                           \ the frame and its fields, for KELON168-READ (kelonrd.fs)
21 CONSTANT #BYTES
#BYTES BUFFER: FRAME                \ the frame of the state
FRAME FIELDS-IN                     \ the fields: byte, first bit, width (Kai's field map, section 4)
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

PRIVATE
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
PROTECTED
: FAN@ ( -- n )   \ 0 (auto) for a code that is not in the list
  FAN-LOW FIELD@  FAN-HIGH FIELD@ 2 LSHIFT OR  0
  6 0 DO  OVER I FAN-CODES + C@ = IF DROP I THEN  LOOP  NIP ;

\ ---- the temperature: DRY and FAN carry 23 C; the set value stays here
VARIABLE SET-TEMP
: FIXED-TEMP? ( -- f )   MODE-BITS FIELD@ 3 5 WITHIN ;
PRIVATE
: TEMP>FRAME ( -- )   FIXED-TEMP? IF 23 ELSE SET-TEMP @ THEN  16 - TEMP-BITS FIELD! ;

\ ---- the timers (the clock is in AC-BASE)
PROTECTED
VARIABLE ON-DUE   VARIABLE OFF-DUE      \ MILLIS when a timer has come
: DUE ( h m -- ms )   \ MILLIS at the next h:m
  SWAP 60 * +  NOW -  1440 +  1440 MOD  60000 *  MILLIS + ;
PRIVATE
: ?CLOCK ( -- )   CLOCK-SET @ 0= ABORT" KELON168: set the clock first (h m AC-CLOCK!)" ;
: PASSED? ( ms -- f )   MILLIS SWAP - 0< 0= ;
: EXPIRE ( -- )   \ a timer whose time has come goes away
  CLOCK-SET @ 0= IF EXIT THEN
  ON-SET FIELD@ IF  ON-DUE @ PASSED? IF  0 ON-SET FIELD!  0 ON-H FIELD!  0 ON-M FIELD!  THEN  THEN
  OFF-SET FIELD@ IF  OFF-DUE @ PASSED? IF  0 OFF-SET FIELD!  0 OFF-H FIELD!  0 OFF-M FIELD!  THEN  THEN ;

\ ---- checksums: byte 13 is the XOR of bytes 2 to 12, byte 20 of bytes 14 to 19
PROTECTED
: SUM1 ( a -- c )   2 + 11 XOR-BYTES ;
: SUM2 ( a -- c )   14 + 6 XOR-BYTES ;

\ ---- the times of the code (package IRCODE)
PRIVATE
CREATE KELON-BLOCKS  3 C, 6 C, 8 C, 7 C,
PROTECTED
9000 4600 560 1680 600 8000 KELON-BLOCKS SHAPE KELON
PRIVATE
350 CELLS BUFFER: TIMES             \ a frame has 343 times

PUBLIC
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
: AC-POWER@ ( -- f )   POWER-BIT FIELD@ 0<> ;
: AC-MODE@ ( -- mode )   MODE-BITS FIELD@ ;
: AC-TEMP@ ( -- c )   SET-TEMP @ ;
: AC-FAN@ ( -- n )   FAN@ ;

: AC-RESET ( -- )
  FRAME #BYTES 0 FILL  $83 FRAME C!  $06 FRAME 1+ C!
  1 BIT67 FIELD!  $28 FRAME 18 + C!                  \ the model: bits 0-3 and 5-7 of byte 18
  24 SET-TEMP !  COOL MODE-BITS FIELD!  TEMP>FRAME
  CLOCK-SET OFF!  K-POWER KEY! ;

: AC-FRAME ( -- a )
  NOW 60 /MOD CLOCK-H FIELD!  CLOCK-M FIELD!  EXPIRE
  FRAME SUM1 FRAME 13 + C!  FRAME SUM2 FRAME 20 + C!  FRAME ;
: AC>TIMES ( a u -- t m )   KELON  TIMES >TIMINGS  TIMES SWAP ;   \ the frame of the u bytes at a
: AC-TIMINGS ( -- t m )   AC-FRAME #BYTES AC>TIMES ;
: FAN2-AT ( byte bit -- )
  OVER 0 21 WITHIN  OVER 0 8 WITHIN AND 0= ABORT" KELON168: byte 0-20, bit 0-7"
  FAN@ >R  0 FAN-HIGH FIELD!
  FAN-HIGH CELL+ !  FAN-HIGH 2 CELLS + !  R> FAN! ;

AC-RESET
END-PACKAGE
