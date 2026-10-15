\ slip.fs - tests of the package SLIP (lib/slip.fs)
\
\ Load (in the folder ucf), after the package FILES and the tester:
\   ... host/files.fs forth/test/tester.fs lib/test/slip.fs

ONLY FORTH DEFINITIONS  ALSO FILES
S" lib/slip.fs" INCLUDED
S" lib/text.fs" INCLUDED
ONLY FORTH DEFINITIONS  ALSO INTERNALS  ALSO TEXT  ALSO SLIP
DECIMAL

CREATE MSG  $01 C, $C0 C, $02 C, $DB C, $03 C,     \ END and ESC inside
CREATE WIRE 16 ALLOT
CREATE EXPECTED  $C0 C, $01 C, $DB C, $DC C, $02 C, $DB C, $DD C, $03 C, $C0 C,

T{ MSG 5 WIRE ENCODE -> 9 }T
T{ WIRE 9 EXPECTED 9 STR= -> <TRUE> }T

: FEED ( a u -- f )   \ give the bytes to DECODE; the flag after the last one
  NEW-FRAME  FALSE  ROT ROT  0 ?DO  NIP  DUP I + C@ DECODE SWAP  LOOP  DROP ;

T{ WIRE 9 FEED  FRAME-N @ -> <TRUE> 5 }T
T{ FRAME 5 MSG 5 STR= -> <TRUE> }T
T{ WIRE 8 FEED -> <FALSE> }T                 \ the END at the end is missing
T{ WIRE 1 FEED -> <FALSE> }T                 \ END of an empty frame: no frame

S" slip" .RESULTS
