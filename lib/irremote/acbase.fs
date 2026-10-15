\ acbase.fs - package AC-BASE: what the IR frames of air conditioners share
\
\ The packages RTY3 and KELON168 keep the state of an air conditioner as
\ its remote control sends it: in bit fields of a frame of bytes, with
\ the clock of the remote control and a checksum. This package has what
\ both need: the modes, the fields, the XOR of bytes, the clock and the
\ words that show a state. Runs on every target; the clock runs only
\ with 32-bit cells (MILLIS).
\
\   HEAT AUTO COOL DRY FAN ( -- mode )   the modes: 0 to 4
\   a FIELDS-IN             the next fields are in the frame at a
\   byte bit width FIELD name ( -- field )   a field: width bits of the
\                           byte, the lowest of them at bit
\   field FIELD@ ( -- x )   x field FIELD!   read, write (a flag TRUE gives 1)
\   field >INDEX ( -- i )   field >MASK ( -- m )   its byte, its bits there
\   a u XOR-BYTES ( -- c )  the XOR of the u bytes at a
\   min SET-CLOCK           the clock is min now (the minute of the day), and runs
\   NOW ( -- min )          the minute of the day; 0 if the clock is not set
\   CLOCK-SET ( -- a )      TRUE when the clock is set
\   h m ?TIME ( -- h m )    stops unless h is 0-23 and m 0-59
\   mode MODE-NAME ( -- a u )   heat, auto, cool, dry or fan (? for another number)
\   min HM>S ( -- a u )     the minute of the day as hh:mm (in the HOLD space)
\   f .ON   mode .MODE   min .HM   a u .BYTES
\                           show on or off, the mode, the time hh:mm, u bytes in hex
\
\ There is one clock for all frames: the time of day. Load this package
\ before RTY3 or KELON168:
\   on the host:   ALSO FILES  S" lib/irremote/acbase.fs" INCLUDED
\   in ucf term:   #require AC-BASE lib/irremote/acbase.fs
\ Example:
\
\   ALSO AC-BASE
\   CREATE F  2 ALLOT   F FIELDS-IN
\   1 4 3 FIELD F-MODE              \ byte 1, bits 4 to 6
\   F 2 0 FILL  COOL F-MODE FIELD!  F 2 .BYTES
\   00 20
\   F-MODE FIELD@ .MODE
\   cool

PACKAGE AC-BASE   \ lib/package.fs

VARIABLE IN-FRAME                   \ FIELD: the frame of the next fields
VARIABLE CLOCK-MIN                  \ the minute of the day when the clock was set
VARIABLE CLOCK-MS                   \ MILLIS then

PUBLIC
0 CONSTANT HEAT   1 CONSTANT AUTO   2 CONSTANT COOL   3 CONSTANT DRY   4 CONSTANT FAN

\ ---- fields: a field holds its width, its first bit, its byte and the frame
: FIELDS-IN ( a -- )   IN-FRAME ! ;
: FIELD ( byte bit width "name" -- )   CREATE , , ,  IN-FRAME @ , ;   \ name ( -- field )
: >INDEX ( field -- i )   2 CELLS + @ ;
PRIVATE
: >SHIFT ( field -- n )   CELL+ @ ;
: >BYTE ( field -- a )    DUP >INDEX  SWAP 3 CELLS + @ + ;
PUBLIC
: >MASK ( field -- m )    DUP @  1 SWAP LSHIFT 1-  SWAP >SHIFT LSHIFT ;
: FIELD@ ( field -- x )   DUP >BYTE C@  OVER >MASK AND  SWAP >SHIFT RSHIFT ;
: FIELD! ( x field -- )   \ a flag TRUE gives 1
  >R  R@ >SHIFT LSHIFT  R@ >MASK AND
  R@ >BYTE C@  R@ >MASK INVERT AND  OR  R> >BYTE C! ;

\ ---- the checksum of both frames
: XOR-BYTES ( a u -- c )   0 ROT ROT  OVER + SWAP ?DO  I C@ XOR  LOOP ;

\ ---- the clock
VARIABLE CLOCK-SET
: NOW ( -- min )
  CLOCK-SET @ 0= IF 0 EXIT THEN
  MILLIS CLOCK-MS @ -  0 60000 UM/MOD NIP  CLOCK-MIN @ +  1440 MOD ;
: SET-CLOCK ( min -- )   CLOCK-MIN !  MILLIS CLOCK-MS !  CLOCK-SET ON! ;
: ?TIME ( h m -- h m )
  OVER 0 24 WITHIN  OVER 0 60 WITHIN AND 0= ABORT" AC-BASE: the time is h 0-23, m 0-59" ;

\ ---- the state as text, for .AC and for messages
: MODE-NAME ( mode -- a u )
  CASE  HEAT OF S" heat" ENDOF  AUTO OF S" auto" ENDOF  COOL OF S" cool" ENDOF
        DRY OF S" dry" ENDOF   FAN OF S" fan" ENDOF  S" ?" ROT
  ENDCASE ;
: HM>S ( min -- a u )   60 /MOD SWAP 0 <# # # 2DROP ':' HOLD 0 # # #> ;
: .ON ( f -- )   IF ." on" ELSE ." off" THEN ;
: .HM ( min -- )   HM>S TYPE ;
: .MODE ( mode -- )   DUP 0 5 WITHIN IF  MODE-NAME TYPE  ELSE  ." mode " 0 .R  THEN ;
: .BYTES ( a u -- )   0 ?DO  I IF SPACE THEN  DUP I + C@ .H2  LOOP  DROP CR ;

END-PACKAGE
