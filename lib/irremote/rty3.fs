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
\ changes only the package. The modes HEAT AUTO COOL DRY FAN are in
\ AC-BASE.
\
\   AC-RESET            off, cool, 24 C, fan 0, flap 0, no clock
\   f AC-POWER!         on (true) or off
\   mode AC-MODE!       the mode; in DRY and FAN the frame has 23 C
\   c AC-TEMP!          the temperature, 16 to 30 C
\   n AC-FAN!           the fan: 0 to 7, the code as the remote control sends it
\   n AC-FLAP!          the flap: 0 to 7, the code as the remote control sends it
\   h m AC-CLOCK!       the clock of the frame is h:m now, and runs (32-bit
\                       cells); without it the frame has 0:00
\   AC-POWER@ ( -- f )   AC-MODE@ ( -- mode )   AC-TEMP@ ( -- c )
\   AC-FAN@ ( -- n )   AC-FLAP@ ( -- n )   the state as the words above set it
\   c ?AC-TEMP ( -- c )   n ?AC-FAN ( -- n )   n ?AC-FLAP ( -- n )   the checks
\                       of AC-TEMP! AC-FAN! AC-FLAP!: they stop if the value
\                       does not fit (for a program that checks all first)
\   AC-FRAME ( -- a )   the 23 bytes of the state, with clock and checksum
\   AC-TIMINGS ( -- t m )   the frame as m times in us at t, for a sender
\   a u AC>TIMES ( -- t m )   the same for the u bytes at a (at most 23)
\
\ This package only makes frames. Frames back into bytes, their check,
\ a frame as the state and the state in words: package RTY3-READ
\ (rty3read.fs).
\
\ Load (it needs the packages AC-BASE and IRCODE):
\   on the host:   ALSO FILES  S" lib/irremote/acbase.fs" INCLUDED
\                  S" lib/irremote/ircode.fs" INCLUDED
\                  S" lib/irremote/rty3.fs" INCLUDED
\   in ucf term:   #require AC-BASE lib/irremote/acbase.fs
\                  #require IRCODE lib/irremote/ircode.fs
\                  #include lib/irremote/rty3.fs
\ Example:
\
\   ALSO AC-BASE  ALSO RTY3
\   TRUE AC-POWER!  HEAT AC-MODE!  23 AC-TEMP!
\   AC-FRAME 23 .BYTES
\   60 38 13 01 FF 00 46 00 00 00 00 00 00 00 00 00 00 00 00 02 00 00 BA

PACKAGE RTY3  ALSO AC-BASE  ALSO IRCODE   \ lib/package.fs

PROTECTED                           \ the frame and its fields, for RTY3-READ (rty3read.fs)
23 CONSTANT #BYTES
#BYTES BUFFER: FRAME                \ the frame of the state
FRAME FIELDS-IN                     \ the fields: byte, first bit, width
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

PRIVATE
\ ---- the keys of the remote control, for byte 19
$25 CONSTANT K-POWER   $06 CONSTANT K-MODE   $01 CONSTANT K-UP   $02 CONSTANT K-DOWN
$07 CONSTANT K-FLAP    $08 CONSTANT K-FAN
: KEY! ( k -- )   KEY-CODE FIELD! ;

\ ---- the temperature: DRY and FAN carry 23 C; the set value stays here
PROTECTED
VARIABLE SET-TEMP
: FIXED-TEMP? ( -- f )   MODE-BITS FIELD@ 3 5 WITHIN ;
PRIVATE
: TEMP>FRAME ( -- )   FIXED-TEMP? IF 23 ELSE SET-TEMP @ THEN  16 - 10 * TEMP-CODE FIELD! ;

\ ---- the times of the code (package IRCODE): one block of 23 bytes
CREATE RTY3-BLOCKS  1 C, 23 C,
PROTECTED
9000 4500 560 1690 540 0 RTY3-BLOCKS SHAPE RTY3-CODE
PRIVATE
800 CONSTANT STOP-MARK              \ the last light of a frame
380 CELLS BUFFER: TIMES             \ a frame has 371 times

PUBLIC
: AC-POWER! ( f -- )   POWER-BIT FIELD!  K-POWER KEY! ;
: AC-MODE! ( mode -- )
  DUP 0 5 WITHIN 0= ABORT" RTY3: the modes are 0 to 4 (HEAT AUTO COOL DRY FAN)"
  DUP MODE-BITS FIELD!  TEMP>FRAME
  DRY <> IF  0 DRY-GRADE FIELD!  THEN
  K-MODE KEY! ;
: ?AC-TEMP ( c -- c )   DUP 16 31 WITHIN 0= ABORT" RTY3: the temperature is 16 to 30 C" ;
: ?AC-FAN ( n -- n )   DUP 0 8 WITHIN 0= ABORT" RTY3: the fan is 0 to 7" ;
: ?AC-FLAP ( n -- n )   DUP 0 8 WITHIN 0= ABORT" RTY3: the flap is 0 to 7" ;
: AC-TEMP! ( c -- )
  ?AC-TEMP  DUP SET-TEMP @ < IF K-DOWN ELSE K-UP THEN  SWAP SET-TEMP !  TEMP>FRAME  KEY! ;
: AC-FAN! ( n -- )    ?AC-FAN  FAN-CODE FIELD!  K-FAN KEY! ;
: AC-FLAP! ( n -- )   ?AC-FLAP  FLAP-CODE FIELD!  K-FLAP KEY! ;
: AC-CLOCK! ( h m -- )   ?TIME  SWAP 60 * +  SET-CLOCK ;
: AC-POWER@ ( -- f )   POWER-BIT FIELD@ 0<> ;
: AC-MODE@ ( -- mode )   MODE-BITS FIELD@ ;
: AC-TEMP@ ( -- c )   SET-TEMP @ ;
: AC-FAN@ ( -- n )   FAN-CODE FIELD@ ;
: AC-FLAP@ ( -- n )   FLAP-CODE FIELD@ ;

: AC-RESET ( -- )
  FRAME #BYTES 0 FILL  $60 FRAME C!  $38 FRAME 1+ C!  $13 FRAME 2 + C!
  $FF FRAME 4 + C!                                   \ as the remote control sends it
  24 SET-TEMP !  COOL MODE-BITS FIELD!  TEMP>FRAME
  CLOCK-SET OFF!  K-POWER KEY! ;

: AC-FRAME ( -- a )
  CLOCK-SET @ IF  NOW 60 /MOD CLOCK-H FIELD!  CLOCK-M FIELD!  THEN
  FRAME 3 + 19 XOR-BYTES  FRAME 22 + C!  FRAME ;
: AC>TIMES ( a u -- t m )   \ the frame of the u bytes at a; the last light is STOP-MARK
  RTY3-CODE  TIMES >TIMINGS                                 ( m )
  STOP-MARK OVER 1- CELLS TIMES + !  TIMES SWAP ;
: AC-TIMINGS ( -- t m )   AC-FRAME #BYTES AC>TIMES ;

AC-RESET
END-PACKAGE
