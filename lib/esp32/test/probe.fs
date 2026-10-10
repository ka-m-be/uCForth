\ probe.fs - synthetic probe requests for the radio tests; no hardware access
PACKAGE TEST-PROBE
64 BUFFER: FRAME
VARIABLE SEQUENCE
: HEADER ( mac -- )
  FRAME 64 0 FILL  $40 FRAME C!
  FRAME 4 + 6 $FF FILL  FRAME 10 + 6 MOVE  FRAME 16 + 6 $FF FILL
  SEQUENCE @ $FFF AND 4 LSHIFT  DUP FRAME 22 + C!  8 RSHIFT FRAME 23 + C!
  1 SEQUENCE +! ;
: RATES ( a -- )
  1 OVER C!  3 OVER 1+ C!
  12 OVER 2 + C!  24 OVER 3 + C!  48 SWAP 4 + C! ;
PUBLIC
: MAC-DECODE ( low high a -- )   \ ESP32 factory MAC: bits 72..79 down to 32..39
  >R  DUP 8 RSHIFT R@ C!  R@ 1+ C!
  DUP 24 RSHIFT R@ 2 + C!  DUP 16 RSHIFT R@ 3 + C!
  DUP 8 RSHIFT R@ 4 + C!  R> 5 + C! ;
: PROBE-NAMED ( mac name n -- a u )
  DUP 0 33 WITHIN 0= ABORT" TEST-PROBE: name length must be 0 to 32"
  >R >R  HEADER  R> R>
  DUP FRAME 25 + C!  DUP >R  FRAME 26 + SWAP MOVE
  R> DUP FRAME 26 + + RATES  31 + FRAME SWAP ;
: PROBE-BUILD ( mac -- a n )   S" ucf-n3c" PROBE-NAMED ;
: PROBE-TARGET ( a n peer -- a n )
  >R
  OVER 4 + R@ SWAP 6 MOVE  OVER 16 + R> SWAP 6 MOVE
  OVER 2 + 60 SWAP C! ;   \ SIFS and the legacy OFDM ACK; no RTS
: PROBE-TO ( mac peer -- a n )   >R PROBE-BUILD R> PROBE-TARGET ;
END-PACKAGE
