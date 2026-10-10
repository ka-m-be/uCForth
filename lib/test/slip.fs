\ slip.fs - tests of the package SLIP (lib/slip.fs)
\
\ Load (in the folder ucf), after the package FILES and the tester:
\   ... host/files.fs forth/test/tester.fs lib/test/slip.fs

ONLY FORTH DEFINITIONS  ALSO FILES
S" lib/slip.fs" INCLUDED
ONLY FORTH DEFINITIONS  ALSO INTERNALS  ALSO SLIP
DECIMAL

CREATE MSG  $01 C, $C0 C, $02 C, $DB C, $03 C,     \ END and ESC inside
CREATE WIRE 16 ALLOT
CREATE EXPECTED  $C0 C, $01 C, $DB C, $DC C, $02 C, $DB C, $DD C, $03 C, $C0 C,
: SAME-BYTES? ( a1 a2 u -- f )
  0 ?DO  OVER I + C@  OVER I + C@ <> IF 2DROP UNLOOP FALSE EXIT THEN  LOOP  2DROP TRUE ;

T{ MSG 5 WIRE ENCODE -> 9 }T
T{ WIRE EXPECTED 9 SAME-BYTES? -> <TRUE> }T

: FEED ( a u -- f )   \ give the bytes to DECODE; the flag after the last one
  NEW-FRAME  FALSE  ROT ROT  0 ?DO  NIP  DUP I + C@ DECODE SWAP  LOOP  DROP ;

T{ WIRE 9 FEED  FRAME-N @ -> <TRUE> 5 }T
T{ FRAME MSG 5 SAME-BYTES? -> <TRUE> }T
T{ WIRE 8 FEED -> <FALSE> }T                 \ the END at the end is missing
T{ WIRE 1 FEED -> <FALSE> }T                 \ END of an empty frame: no frame

S" slip" .RESULTS
