\ slip.fs - package SLIP: byte frames after RFC 1055
\
\ A frame starts and ends with the byte END ($C0). Inside a frame END
\ becomes ESC ESC-END ($DB $DC) and ESC becomes ESC ESC-ESC ($DB $DD).
\ The ROM bootloader of the ESP32 uses these frames (lib/esp32/esprom.fs).
\ Runs on every target.
\
\   a u dest ENCODE ( -- u' )   the frame for a u, to dest (2u+2 bytes)
\   NEW-FRAME                   start to collect a frame
\   c DECODE ( -- f )           add a received byte; true: FRAME holds a
\                               complete frame of FRAME-N bytes

PACKAGE SLIP   \ lib/package.fs
$C0 CONSTANT END   $DB CONSTANT ESC   $DC CONSTANT ESC-END   $DD CONSTANT ESC-ESC
: C+! ( c a -- a+1 )  TUCK C! 1+ ;
1100 CONSTANT FRAME-SIZE
VARIABLE ESCAPED

PUBLIC
FRAME-SIZE BUFFER: FRAME            \ the frame that DECODE collects
VARIABLE FRAME-N                    \ its length

: ENCODE ( a u dest -- u' )
  DUP >R  END SWAP C+!  ROT ROT  OVER + SWAP
  ?DO  I C@
    DUP END = IF DROP  ESC SWAP C+!  ESC-END SWAP C+!  ELSE
    DUP ESC = IF DROP  ESC SWAP C+!  ESC-ESC SWAP C+!  ELSE
    SWAP C+!  THEN THEN
  LOOP  END SWAP C+!  R> - ;

: NEW-FRAME ( -- )  0 FRAME-N !  0 ESCAPED ! ;

PRIVATE
: FRAME+ ( c -- )
  FRAME-N @ FRAME-SIZE < IF  FRAME FRAME-N @ + C!  1 FRAME-N +!  ELSE DROP THEN ;

PUBLIC
: DECODE ( c -- f )
  ESCAPED @ IF  0 ESCAPED !
    DUP ESC-END = IF DROP END ELSE  DUP ESC-ESC = IF DROP ESC THEN  THEN
    FRAME+ FALSE EXIT
  THEN
  DUP ESC = IF DROP TRUE ESCAPED ! FALSE EXIT THEN
  DUP END = IF DROP FRAME-N @ 0<> EXIT THEN
  FRAME+ FALSE ;

END-PACKAGE
