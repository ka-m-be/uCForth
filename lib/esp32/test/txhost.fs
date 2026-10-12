\ txhost.fs - private TX preparation of ESP32-RADIO; no transmission
\
\ Reuse the MAC model and its RX tests, then check TX with synthetic
\ bytes. MAC registers above offset $1000 need an 8 KB model. The
\ counters check that no slot starts, and invalid inputs write nothing.
\ In the folder ucf, with 32-bit cells:
\   build/ucf-c -32 host/files.fs lib/esp32/test/txhost.fs </dev/null
\ At the end: "radiohost: 34 tests, 0 errors", then "txhost: 36 tests, 0 errors".

ONLY FORTH DEFINITIONS ALSO FILES
[DEFINED] MOCK-MAC S" lib/esp32/test/radiohost.fs" ?INCLUDED
ONLY FORTH DEFINITIONS ALSO FILES ALSO MOCK-MAC ALSO ESP32-PHY ALSO INTR ALSO BYTES ALSO ESP32-MAC ALSO ESP32-RADIO ALSO ESP32-RADIO.I   \ as radiohost.fs leaves it

1600 BUFFER: TX-TEST
VARIABLE TX-N   24 TX-N !
VARIABLE TX-ACK   FALSE TX-ACK !
VARIABLE TX-BACKOFF   7 TX-BACKOFF !
: SAMPLE ( n -- a n )
  TX-TEST 1600 0 FILL
  DUP 0 MAX 1600 MIN 0 ?DO  I 255 AND TX-TEST I + C!  LOOP
  $40 TX-TEST C!  0 TX-TEST 1+ C!
  $34 TX-TEST 2 + C!  $12 TX-TEST 3 + C!  2 TX-TEST 4 + C!
  TX-TEST SWAP ;
: PREPARE ( -- )   TX-N @ SAMPLE  TX-ACK @ TX-BACKOFF @ TX-PREPARE ;
: SAME-BYTES? ( a b n -- f )
  0 ?DO
    OVER I + C@  OVER I + C@ <> IF  2DROP FALSE UNLOOP EXIT  THEN
  LOOP  2DROP TRUE ;
: INVALID? ( -- n u u )
  0 #WRITES !  0 #TX-STARTS !  ['] PREPARE CATCH  #WRITES @ #TX-STARTS @ ;
: MOCK-STARTS ( -- u )
  0 #TX-STARTS !
  5 0 DO  $C0000000 MAC $D00 + I 8 * + !  LOOP  #TX-STARTS @ ;

\ Check the detector itself before proving that preparation starts no slot.
T{ MOCK-STARTS -> 5 }T
T{ TX-SLOT TX-MASK TX-MAX -> 2 4 1572 }T
T{ TX-CONFIG TX-CTRL TX-PLCP1 TX-PLCP2 TX-DURATION TX-PMD ->
   $3FF73D0C $3FF73D10 $3FF741E0 $3FF741E4 $3FF741F0 $3FF741F8 }T
T{ TX-DESC TX-BUF TX-BUF BUF-SIZE + -> $3FFE7660 $3FFE766C $3FFE7CAC }T
T{ 24 TX-LENGTH 24 TX-FLAGS 24 TX-RATE -> 28 $C001C01C $1000B01C }T
T{ 1572 TX-LENGTH 1572 TX-FLAGS 1572 TX-RATE -> 1576 $C0628628 $1000B628 }T
T{ FALSE TX-POINTER TRUE TX-POINTER -> $000E7660 $010E7660 }T
T{ $F0C00000 0 TX-ACCESS  $F0C00000 15 TX-ACCESS -> $F3C0000A $F3C0F00A }T

\ No preparation while off; not even the descriptor changes.
T{ INVALID? -> -2 0 0 }T
RADIO-START
TX-DESC 4 - >HOST 4 $A5 FILL
TX-BUF BUF-SIZE + >HOST 4 $5A FILL
$F0C00000 TX-CONFIG !
0 #TX-STARTS !
T{ 0 #WRITES !  PREPARE #WRITES @ 0<> -> TRUE }T
T{ PREPARE  TX-DESC @ TX-DESC CELL+ @ TX-DESC 2 CELLS + @ ->
   $C001C01C TX-BUF 0 }T
T{ TX-CTRL @ TX-CONFIG @ TX-PLCP1 @ TX-PLCP2 @ TX-DURATION @ ->
   $000E7660 $F3C0700A $1000B01C $20 $12341234 }T
T{ TX-TEST TX-BUF 24 SAME-BYTES?  TX-BUF 24 + @ -> TRUE 0 }T
T{ TX-CTRL @ TX-START-BITS AND  #TX-STARTS @ -> 0 0 }T
T{ TX-BUF BUF-SIZE + @ -> $5A5A5A5A }T

\ ACK bit and the largest frame; the FCS reserve is really in the buffer.
24 SAMPLE DROP 10 + RADIO-MAC!
TRUE TX-ACK !
T{ PREPARE  TX-CTRL @ -> $010E7660 }T
1572 TX-N !
T{ PREPARE  TX-DESC @ TX-PLCP1 @ -> $C0628628 $1000B628 }T
T{ TX-TEST TX-BUF 1572 SAME-BYTES?  TX-BUF 1572 + @ -> TRUE 0 }T
T{ TX-DESC 2 CELLS + @  TX-DESC 4 - @  TX-BUF BUF-SIZE + @  #TX-STARTS @ ->
   0 $A5A5A5A5 $5A5A5A5A 0 }T

\ The frame is copied, not borrowed; the RX list is unchanged.
T{ 0 TX-TEST C!  TX-BUF C@ -> $40 }T
T{ 0 DESC @  7 DESC >NEXT @  RX-NEXT @ -> $80000640 0 0 DESC }T

\ A length not divisible by four: only the payload and four FCS bytes change.
TX-BUF BUF-SIZE $CC FILL
25 TX-N !  FALSE TX-ACK !
T{ PREPARE  TX-DESC @ TX-PLCP1 @ -> $C001D01D $1000B01D }T
T{ TX-BUF 24 + C@  TX-BUF 25 + C@  TX-BUF 26 + C@
   TX-BUF 27 + C@  TX-BUF 28 + C@  TX-BUF 29 + C@ -> 24 0 0 0 0 $CC }T
T{ TX-TEST TX-BUF 25 SAME-BYTES?  #TX-STARTS @ -> TRUE 0 }T

23 TX-N !
T{ INVALID? -> -2 0 0 }T
0 TX-N !
T{ INVALID? -> -2 0 0 }T
-1 TX-N !
T{ INVALID? -> -2 0 0 }T
1573 TX-N !
T{ INVALID? -> -2 0 0 }T
24 TX-N !  -1 TX-BACKOFF !
T{ INVALID? -> -2 0 0 }T
16 TX-BACKOFF !
T{ INVALID? -> -2 0 0 }T
7 TX-BACKOFF !
$80000000 TX-CTRL >HOST !
T{ INVALID? -> -2 0 0 }T
$40000000 TX-CTRL >HOST !
T{ INVALID? -> -2 0 0 }T
0 TX-CTRL !

: PROTECTED ( -- )   24 SAMPLE  $40 TX-TEST 1+ C!  FALSE 7 TX-PREPARE ;
: GROUP-ACK ( -- )   24 SAMPLE  $FF TX-TEST 4 + C!  TRUE 7 TX-PREPARE ;
: SHORT ( -- )   $DEADBEEF 23 FALSE 7 TX-PREPARE ;
T{ 0 #WRITES !  ' PROTECTED CATCH #WRITES @ -> -2 0 }T
T{ 0 #WRITES !  ' GROUP-ACK CATCH #WRITES @ -> -2 0 }T
T{ 0 #WRITES !  ' SHORT CATCH #WRITES @ -> -2 0 }T
T{ #TX-STARTS @ -> 0 }T

RADIO-STOP
S" txhost" .RESULTS
