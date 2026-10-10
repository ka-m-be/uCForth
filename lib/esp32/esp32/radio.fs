\ radio.fs - package ESP32-RADIO: the radio of the ESP32 for Wi-Fi (contract RADIO)
\
\ ESP32-PHY (phy.fs) calibrates the radio and sets its channel. This
\ package makes of it a part that an application uses, with the words of
\ the contract RADIO. The radio of another chip gives the same words.
\
\   RADIO-START ( -- )          the radio on: calibration, MAC, reception, channel 1
\   ch RADIO-CHANNEL! ( ch -- ) the channel 1 to 13
\   RADIO-CHANNEL@ ( -- ch )    the channel; 0 while the radio is off
\   RADIO-BEACONS ( -- )        let the beacons in: for a scan of the networks
\   RADIO-RECEIVE ( -- a n | 0 )  the next frame that came: n bytes at a; or 0
\   RADIO-LEVEL ( -- dbm )      the level of that frame in dBm (-90 weak, -30 strong)
\   RADIO-MAC! ( a -- )        own 6-byte address, after RADIO-START
\   RADIO-MAC@ ( -- a )        the configured address; do not change its bytes
\   RADIO-FACTORY ( -- )       configure the ESP32 factory MAC, while on
\   RADIO-STATION ( bssid -- ) normal address filters, then strict software delivery
\   RADIO-RANDOM ( a n -- )    hardware random bytes while RF is on, spaced reads
\   RADIO-SEND ( a n ack? -- )  send one frame, optionally wait for its ACK
\   RADIO-STOP ( -- )           the radio off
\
\ RADIO-START does what the reference does at the start (esp-wifi-hal
\ 0.3.3, the function init in src/ll.rs): PHY-INIT (the power and the
\ clocks of Wi-Fi, a full calibration), a reset of the MAC (the part of
\ the chip that sends and receives the frames), the MAC ready, then
\ HAL-INIT sets up its registers. The addresses come from the register
\ map of the reference (the crate esp32 0.40.2): facts about the chip, no
\ code. RADIO-START takes about 60 ms; each one calibrates again and
\ resets the MAC.
\
\ The reception. The MAC writes each frame with DMA into a buffer in
\ SRAM1, outside the memory of the VM: 8 buffers of 1 600 bytes, each
\ with a descriptor of 3 cells (the size of the buffer, the length of the
\ data and the flags; the address of the buffer; the next descriptor).
\ The descriptors make a list, and the DMA fills them one after the
\ other. RADIO-RECEIVE takes the first one when it holds the end of a
\ frame; the next RADIO-RECEIVE gives it back at the end of the list. So
\ the n bytes at a stay until the next RADIO-RECEIVE. Before each frame
\ the MAC writes a header of 28 bytes about the reception: byte 0 the
\ level, bytes 24 and 25 (bits 0 to 11) the length of the frame with its
\ check sum (FCS, 4 bytes, not in the buffer). RADIO-RECEIVE gives the
\ frame from its first byte, without the header and the FCS; RADIO-LEVEL
\ gives the level from the header. A frame longer than a buffer fills
\ several descriptors, and only the last one has the end of the frame:
\ RADIO-RECEIVE gives them back and counts the frame in #LONG.
\
\ Without filters the MAC lets no frame through. RADIO-BEACONS sets the
\ filter of a scan: the MAC lets the beacons and the probe responses of
\ the access points through (the chip has no setting for the beacons
\ only). RADIO-RECEIVE then gives only the beacons; it gives each other
\ frame back unread (it reads only the first byte, the type) and counts
\ it in #OTHER. A Forth handler (package INTR, level 1) counts the
\ interrupts of the MAC and clears them. Call RADIO-RECEIVE from one
\ place only: from the program, or from one handler (as NET-STEP of a
\ timer).
\
\ After a reset, and after a start from an image (FREEZE), the radio is
\ off: RADIO-START first. So do FREEZE only while the radio is off.
\ ALSO RADIO gives this package (the word RADIO at the end).
\
\ Load it in ucf term, in the folder ucf, after phy.fs (which loads right
\ after a reset):
\   #include lib/esp32/esp32/phy.fs
\   #include lib/net/bytes.fs
\   #include lib/esp32/esp32/macinit.fs
\   #include lib/esp32/esp32/radio.fs
\ The test on the board: lib/esp32/esp32/test/radio.fs; on the host:
\ lib/esp32/test/radiohost.fs.
\
\ TX uses one descriptor and buffer after RX, and hardware slot 2.
\ RADIO-SEND waits at most 100 ms, with interrupts on; use it only in
\ the foreground, not in a Forth handler. An unknown end locks TX until
\ RADIO-STOP and RADIO-START reset the MAC. Tests: txhost.fs, txdone.fs
\ in lib/esp32/test. ACK waiting needs a configured own MAC that matches
\ the source address in the header. Only a completion with no PMD error
\ counts as a received ACK when it was requested.

PACKAGE ESP32-RADIO  ALSO ESP32-PHY  ALSO ESP32-MAC  ALSO INTR  ALSO BYTES   \ lib/package.fs

$3FF000D0 CONSTANT WIFI-RST-EN   \ DPORT: the resets of the parts of Wi-Fi
4 CONSTANT MAC-RST               \ bit 2: the MAC
$3FF73000 CONSTANT MAC           \ the registers of the MAC
MAC $84 + CONSTANT RX-CTRL       \ the reception of frames
$80000000 CONSTANT RX-ON         \ bit 31: on
1 CONSTANT RX-RELOAD             \ bit 0: the DMA reads the list again; 0 when it is done
MAC $88 + CONSTANT RX-BASE       \ the descriptor where the DMA begins
MAC $8C + CONSTANT RX-NEXT       \ the descriptor that the DMA fills next
$3FF00000 CONSTANT RX-END        \ RX-NEXT after the last descriptor (also after a reset)
MAC $C48 + CONSTANT INT-STATUS   \ the interrupts of the MAC that came
MAC $C4C + CONSTANT INT-CLEAR    \ write the bits of INT-STATUS here: they go
0 CONSTANT MAC-SOURCE            \ the interrupt source of the MAC (package INTR)
MAC $CB8 + CONSTANT MAC-CTRL     \ name and meaning not known; the start clears bits in it
$17FF CONSTANT MAC-INIT          \ these bits (the reference keeps the mask $FFFFE800)
MAC $D8 + CONSTANT FILTER        \ the filters of interface 0
$117 CONSTANT FILTER-BITS        \ bits 0 1 2: by address and BSSID; 4: beacons pass; 8: all pass
$17 CONSTANT SCAN-FILTER         \ their values for a scan: beacons and probe responses pass
MAC $F8 + CONSTANT CTRL-FILTER   \ the control frames that pass, interface 0
$FF800000 CONSTANT CTRL-TYPES    \ bits 23 to 31: a scan lets none of them pass (HAL-INIT sets two)
28 CONSTANT RX-HEAD              \ the header of the MAC before each frame
$80 CONSTANT BEACON              \ the first byte of a beacon: type 0 (management), subtype 8

$3FFE4400 CONSTANT DESCS         \ SRAM1: the descriptors, 3 cells each (targets/esp32/README.md, "Memory")
8 CONSTANT #RX                   \ the descriptors, each with a buffer
1600 CONSTANT BUF-SIZE           \ the bytes of a buffer (the reference: at least 1 600)
DESCS #RX 3 CELLS * + CONSTANT BUFS   \ the buffers, after the descriptors
$80000000 CONSTANT DMA-OWNS      \ bit 31 of a descriptor: the DMA may fill it
$40000000 CONSTANT FRAME-END     \ bit 30: the buffer holds the end of a frame

2 CONSTANT TX-SLOT              \ best effort; slot 0 is for beacons
MAC $CFC + TX-SLOT 8 * + CONSTANT TX-CONFIG
MAC $D00 + TX-SLOT 8 * + CONSTANT TX-CTRL
MAC $1168 + TX-SLOT 60 * + CONSTANT TX-PLCP1
MAC $116C + TX-SLOT 60 * + CONSTANT TX-PLCP2
MAC $1178 + TX-SLOT 60 * + CONSTANT TX-DURATION
MAC $1180 + TX-SLOT 60 * + CONSTANT TX-PMD
MAC $CBC + CONSTANT TX-ERROR-CLEAR
MAC $CC0 + CONSTANT TX-ERROR-STATUS
MAC $CC4 + CONSTANT TX-COMPLETE-CLEAR
MAC $CC8 + CONSTANT TX-COMPLETE-STATUS
1 4 TX-SLOT - LSHIFT CONSTANT TX-MASK   \ the status bits number the slots in reverse
TX-MASK 16 LSHIFT CONSTANT TX-TIMEOUT-MASK
TX-MASK TX-TIMEOUT-MASK OR CONSTANT TX-ERROR-MASK
$80180 CONSTANT TX-INTS          \ complete, channel access timeout, collision
$C0000000 CONSTANT TX-START-BITS
$01000000 CONSTANT TX-WAIT-ACK
$0F3FFFFF CONSTANT TX-ACCESS-BITS
BUFS #RX BUF-SIZE * + CONSTANT TX-DESC
TX-DESC 3 CELLS + CONSTANT TX-BUF
BUF-SIZE RX-HEAD - CONSTANT TX-MAX
$60035144 CONSTANT RNG-DATA      \ ESP-IDF 5.5.2, ESP32 WDEV_RND_REG; backoff only
100 CONSTANT TX-WAIT-MS
2 CONSTANT TX-COMPLETE
3 CONSTANT TX-ACCESS-TIMEOUT
4 CONSTANT TX-COLLISION
5 CONSTANT TX-CANCELLED
6 CONSTANT TX-NO-END
MAC $40 + CONSTANT RA-LOW
MAC $44 + CONSTANT RA-HIGH
MAC $60 + CONSTANT RA-MASK-LOW
MAC $64 + CONSTANT RA-MASK-HIGH
$10000 CONSTANT RA-ENABLE
6 BUFFER: MAC-BYTES
6 BUFFER: BSS-BYTES
0 VALUE STATION?
0 VALUE SOFT-CCMP?
VARIABLE RANDOM-WORD
0 VALUE MAC-SET?

0 VALUE ON?       \ TRUE between RADIO-START and RADIO-STOP
0 VALUE CHANNEL   \ the channel; 0 while the radio is off
0 VALUE HEAD      \ the first descriptor of the list: the DMA fills it, or it is full
0 VALUE TAIL      \ the last descriptor of the list
-1 VALUE HELD     \ the descriptor of the last RADIO-RECEIVE, or -1
0 VALUE BEACONS?  \ TRUE after RADIO-BEACONS: RADIO-RECEIVE gives only the beacons
0 VALUE LEVEL     \ the level of the frame of the last RADIO-RECEIVE
VARIABLE #INTS    \ the interrupts of the MAC since RADIO-START
VARIABLE INT-BITS \ the bits that they had
VARIABLE #LONG    \ the frames longer than a buffer, given back
VARIABLE #OTHER   \ the frames that were no beacon, given back unread
0 VALUE TX-ACTIVE?
0 VALUE TX-BLOCKED?             \ an unknown DMA end; only a MAC reset makes reuse safe
0 VALUE TX-SINCE
VARIABLE TX-RESULT              \ 0: no result yet; otherwise TX-COMPLETE or an error
VARIABLE TX-LAST-PMD
VARIABLE #TX                   \ slot completions, also when PMD reports a protocol error
VARIABLE TX-TRACK              \ completions since the last PHY-TRACK
DEFER MAC-SETUP
' MAC-HAL-INIT IS MAC-SETUP
: MAC-SELECT ( xt -- )
  ON? ABORT" ESP32-RADIO: select MAC initialization only while off"
  DUP 0= ABORT" ESP32-RADIO: MAC initialization has no word"
  IS MAC-SETUP ;

: MAC-RESET ( -- )   \ the registers of the MAC as after a reset of the chip; no reception
  MAC-RST WIFI-RST-EN BIS!  MAC-RST WIFI-RST-EN BIC!  RX-ON RX-CTRL BIC! ;
: MAC-READY ( -- )   MAC-INIT MAC-CTRL BIC! ;   \ as the reference does before hal_init
: ?ON ( -- )   ON? 0= ABORT" ESP32-RADIO: the radio is off" ;
: ?CONFIG ( -- )
  TX-ACTIVE? TX-BLOCKED? OR ABORT" ESP32-RADIO: TX needs to end or be reset" ;
: ?MAC ( a -- a )
  DUP C@ 1 AND ABORT" ESP32-RADIO: own MAC must be unicast"
  DUP 0 6 0 DO  OVER I + C@ OR  LOOP
  NIP 0= ABORT" ESP32-RADIO: own MAC must not be zero" ;
: MAC-LOW ( a -- x )
  DUP C@  OVER 1+ C@ 8 LSHIFT OR  OVER 2 + C@ 16 LSHIFT OR
  SWAP 3 + C@ 24 LSHIFT OR ;
: SET-MAC ( a -- )
  ?MAC  MAC-BYTES 6 MOVE
  RA-ENABLE RA-MASK-HIGH BIC!
  MAC-BYTES MAC-LOW RA-LOW !
  RA-HIGH @ $FFFF0000 AND
  MAC-BYTES 4 + DUP C@ SWAP 1+ C@ 8 LSHIFT OR  OR RA-HIGH !
  -1 RA-MASK-LOW !
  RA-MASK-HIGH @ $FFFF0000 AND $FFFF OR RA-MASK-HIGH !
  RA-ENABLE RA-MASK-HIGH BIS!  TRUE TO MAC-SET? ;

\ ---- prepare one transmission; no TX start
: TX-LENGTH ( n -- u )
  DUP 24 TX-MAX 1+ WITHIN 0= ABORT" ESP32-RADIO: TX length must be 24 to 1572"
  4 + ;   \ the hardware adds the FCS; the descriptor counts its four bytes
: TX-FLAGS ( n -- x )   TX-LENGTH DUP 12 LSHIFT OR  DMA-OWNS OR FRAME-END OR ;
: TX-RATE ( n -- x )   TX-LENGTH $1000B000 OR ;   \ OFDM 6 Mbit/s, interface 0
: TX-POINTER ( ack? -- x )
  IF TX-WAIT-ACK ELSE 0 THEN  TX-DESC $FFFFF AND OR ;
: TX-ACCESS ( old backoff -- x )
  DUP 0 16 WITHIN 0= ABORT" ESP32-RADIO: TX backoff must be 0 to 15"
  12 LSHIFT $0300000A OR  SWAP TX-ACCESS-BITS INVERT AND OR ;
: ?TX ( a n ack? -- )
  ?ON  TX-BLOCKED? ABORT" ESP32-RADIO: TX needs a MAC reset"
  TX-ACTIVE? TX-CTRL @ TX-START-BITS AND OR ABORT" ESP32-RADIO: the TX slot is busy"
  >R  TX-LENGTH DROP
  DUP 1+ C@ $40 AND IF
    SOFT-CCMP? 0= ABORT" ESP32-RADIO: enable software CCMP before protected TX"
    STATION? 0= ABORT" ESP32-RADIO: protected TX needs station filters"
  THEN
  R> IF
    DUP 4 + C@ 1 AND ABORT" ESP32-RADIO: a group address cannot be ACKed"
    MAC-SET? 0= ABORT" ESP32-RADIO: configure own MAC before requesting ACK"
    RX-NEXT @ RX-END = ABORT" ESP32-RADIO: receive queued frames before requesting ACK"
    10 + MAC-BYTES 6 BYTES= 0= ABORT" ESP32-RADIO: TX source differs from own MAC"
  ELSE DROP THEN ;
: TX-PREPARE ( a n ack? backoff -- )
  DUP 0 SWAP TX-ACCESS DROP  >R >R  2DUP R@ ?TX
  2DUP TX-BUF SWAP MOVE  TX-BUF OVER + 4 0 FILL
  DUP TX-FLAGS TX-DESC !  TX-BUF TX-DESC CELL+ !  0 TX-DESC 2 CELLS + !
  DUP TX-RATE TX-PLCP1 !  $20 TX-PLCP2 !
  OVER 2 + DUP C@ SWAP 1+ C@ 8 LSHIFT OR  DUP 16 LSHIFT OR TX-DURATION !
  R> TX-POINTER R>  TX-CONFIG @ SWAP TX-ACCESS TX-CONFIG !  TX-CTRL !
  2DROP ;

\ ---- one active sender; the handler records the result before clearing it
: TX-CLEAR ( -- )
  TX-MASK TX-COMPLETE-CLEAR !  TX-ERROR-MASK TX-ERROR-CLEAR !
  TX-INTS INT-CLEAR ! ;
: TX-RESET ( -- )
  FALSE TO TX-ACTIVE?  FALSE TO TX-BLOCKED?  0 TO TX-SINCE
  0 TX-RESULT !  0 TX-LAST-PMD !  0 #TX !  0 TX-TRACK !  FALSE TO MAC-SET?
  FALSE TO SOFT-CCMP? ;
: TX-RELEASE ( -- )   0 TX-CTRL !  FALSE TO TX-ACTIVE? ;
: TX-CANCEL ( -- )
  TX-RESULT @ DUP 0= SWAP TX-NO-END = OR IF  TX-PMD @ TX-LAST-PMD !  THEN
  TRUE TO TX-BLOCKED?  TX-RELEASE
  TX-RESULT @ 0= IF  TX-CANCELLED TX-RESULT !  THEN ;
: TX-INT ( -- )
  TX-COMPLETE-STATUS @ TX-MASK AND  TX-ERROR-STATUS @ TX-ERROR-MASK AND
  2DUP OR IF
    TX-ACTIVE? TX-RESULT @ 0= AND IF
      TX-PMD @ TX-LAST-PMD !
      DUP TX-TIMEOUT-MASK AND IF  TX-ACCESS-TIMEOUT  ELSE
        DUP TX-MASK AND IF  TX-COLLISION  ELSE  TX-COMPLETE  THEN
      THEN >R
      0 TX-CTRL !  TX-ERROR-CLEAR !  TX-COMPLETE-CLEAR !  R> TX-RESULT !
      EXIT
    THEN
  THEN
  TX-ERROR-CLEAR !  TX-COMPLETE-CLEAR ! ;
: TX-KICK ( -- )
  TX-CLEAR  0 TX-RESULT !  0 TX-LAST-PMD !
  MILLIS TO TX-SINCE  TRUE TO TX-ACTIVE?
  TX-START-BITS TX-CTRL BIS! ;
: TX-WAIT ( -- )
  BEGIN  TX-RESULT @ 0=  WHILE
    TX-ACTIVE? 0= ABORT" ESP32-RADIO: TX was stopped"
    MILLIS TX-SINCE - TX-WAIT-MS U< 0= IF
      TX-RESULT @ 0= IF
        TX-NO-END TX-RESULT !  TRUE ABORT" ESP32-RADIO: TX did not end in 100 ms"
      THEN
    THEN
  REPEAT
  TX-ACTIVE? 0= ABORT" ESP32-RADIO: TX was stopped" ;
: ACK-TIMEOUT? ( pmd -- f )
  DUP 12 RSHIFT 15 AND  DUP 5 = IF  2DROP TRUE EXIT  THEN
  4 = IF  255 AND 2 6 WITHIN  ELSE  DROP FALSE  THEN ;
: TX-CHECK ( -- )
  TX-RESULT @ TX-ACCESS-TIMEOUT = ABORT" ESP32-RADIO: TX channel access timeout"
  TX-RESULT @ TX-COLLISION = ABORT" ESP32-RADIO: TX collision"
  TX-RESULT @ TX-COMPLETE <> ABORT" ESP32-RADIO: TX has no completion"
  TX-LAST-PMD @ ACK-TIMEOUT? ABORT" ESP32-RADIO: ACK timeout"
  TX-LAST-PMD @ $F000 AND ABORT" ESP32-RADIO: TX protocol error (TX-LAST-PMD)" ;
: (TX-SEND) ( -- )
  TX-KICK  TX-WAIT  TX-RELEASE
  TX-RESULT @ TX-COMPLETE = IF
    1 #TX +!  1 TX-TRACK +!
    TX-TRACK @ 5 = IF  PHY-TRACK  0 TX-TRACK !  THEN
  THEN
  TX-CHECK ;
: TX-EXECUTE ( -- )
  ['] (TX-SEND) CATCH ?DUP IF  TX-ACTIVE? IF TX-CANCEL THEN  THROW  THEN ;

\ ---- the list of the descriptors
: DESC ( i -- a )   3 CELLS * DESCS + ;
: BUF ( i -- a )   BUF-SIZE * BUFS + ;
: >NEXT ( a -- a' )   2 CELLS + ;              \ the cell with the next descriptor
: >INDEX ( a -- i )   DESCS -  3 CELLS / ;     \ the number of a descriptor at a
: >LENGTH ( x -- n )   12 RSHIFT $FFF AND ;    \ bits 12 to 23: the bytes in the buffer
: RELOAD ( -- )   \ the DMA reads the list again
  RX-RELOAD RX-CTRL BIS!
  1000 0 DO  RX-CTRL @ RX-RELOAD AND 0= IF UNLOOP EXIT THEN  LOOP
  TRUE ABORT" ESP32-RADIO: the DMA does not reload" ;
: EMPTY ( i -- )   \ descriptor i: its buffer, the end of a list, for the DMA
  DUP BUF OVER DESC CELL+ !  0 OVER DESC >NEXT !  BUF-SIZE DMA-OWNS OR  SWAP DESC ! ;
: RX-SETUP ( -- )   \ all descriptors in one list; the DMA begins with the first
  #RX 0 DO  I EMPTY  LOOP
  #RX 1 DO  I DESC  I 1- DESC >NEXT !  LOOP
  0 TO HEAD  #RX 1- TO TAIL  -1 TO HELD
  0 DESC RX-BASE !  RELOAD ;
: GIVE ( i -- )   \ descriptor i back to the DMA, at the end of the list
  DUP EMPTY  DUP DESC  TAIL DESC >NEXT !  DUP TO TAIL
  DESC RX-BASE !  RELOAD ;   \ a DMA at the end of the list begins with it
: TAKE ( -- i )   \ the first descriptor out of the list
  HEAD  DUP DESC >NEXT @ >INDEX  TO HEAD ;
: END-DESC ( -- i | -1 )   \ the descriptor with the end of the first frame; -1: no frame yet
  HEAD BEGIN
    DUP DESC @ FRAME-END AND IF EXIT THEN
    DUP TAIL = IF  DROP -1 EXIT  THEN
    DESC >NEXT @ >INDEX
  AGAIN ;
: DROP-LONG ( i -- )   \ a frame from HEAD to descriptor i, longer than a buffer: give it back
  BEGIN  TAKE DUP GIVE  OVER =  UNTIL DROP  1 #LONG +! ;
: RX-FRAME ( i -- a n )   \ the frame in descriptor i: after the header, without the FCS
  DUP BUF  DUP 24 + @ $FFF AND 4 -               ( i b n )
  ROT DESC @ >LENGTH RX-HEAD - MIN 0 MAX  SWAP RX-HEAD + SWAP ;
: >LEVEL ( b -- dbm )   C@  DUP $80 AND IF 256 - THEN  96 - ;   \ byte 0 of the header
: WANTED? ( a n -- f )   \ the frame goes to the program: any frame; after RADIO-BEACONS a beacon
  STATION? IF
    24 < IF DROP FALSE EXIT THEN
    DUP C@ 12 AND DUP 0= IF
      DROP DUP 16 + BSS-BYTES 6 BYTES= OVER 10 + BSS-BYTES 6 BYTES= AND
    ELSE
      8 = IF DUP 1+ C@ 3 AND 2 = DUP IF
        DROP DUP 10 + BSS-BYTES 6 BYTES=
      THEN ELSE FALSE THEN
    THEN
    SWAP DUP 4 + C@ 1 AND IF DROP ELSE 4 + MAC-BYTES 6 BYTES= AND THEN
    EXIT
  THEN
  BEACONS? 0= IF  2DROP TRUE EXIT  THEN
  IF  C@ BEACON =  ELSE  DROP FALSE  THEN ;
: MAC-INT ( -- )   \ the Forth handler of the MAC: count the interrupts, then clear them
  INT-STATUS @  DUP INT-BITS @ OR INT-BITS !
  DUP TX-INTS AND IF TX-INT THEN  INT-CLEAR !  1 #INTS +! ;

PUBLIC
: RADIO-CHANNEL! ( ch -- )   \ the channel 1 to 13
  ?ON  DUP 1 14 WITHIN 0= ABORT" ESP32-RADIO: the channels are 1 to 13"
  ?CONFIG
  DUP PHY-CHANNEL!  TO CHANNEL ;
: RADIO-CHANNEL@ ( -- ch )   CHANNEL ;   \ 0 while the radio is off
: RADIO-START ( -- )   \ the radio on: calibration, MAC, reception, channel 1
  TX-ACTIVE? ABORT" ESP32-RADIO: cannot restart during TX"
  ON? TX-BLOCKED? AND ABORT" ESP32-RADIO: stop the radio before resetting TX"
  PHY-INIT 1 U> ABORT" ESP32-RADIO: the calibration failed"
  MAC-RESET  MAC-READY  MAC-SETUP  TX-RESET
  TRUE TO ON?  1 RADIO-CHANNEL!  FALSE TO BEACONS? FALSE TO STATION?  0 TO LEVEL
  RX-SETUP  0 #INTS !  0 INT-BITS !  0 #LONG !  0 #OTHER !
  ['] MAC-INT MAC-SOURCE INT!  MAC-SOURCE INT-ON  +INT
  RX-ON RX-CTRL BIS! ;
: RADIO-BEACONS ( -- )   \ let the beacons in, for a scan; RADIO-RECEIVE gives only them
  ?ON  ?CONFIG
  FILTER @  FILTER-BITS INVERT AND  SCAN-FILTER OR  FILTER !
  CTRL-TYPES CTRL-FILTER BIC!  TRUE TO BEACONS? FALSE TO STATION? FALSE TO SOFT-CCMP? ;
: RADIO-STATION ( bssid -- )
  ?ON ?CONFIG
  MAC-SET? 0= ABORT" ESP32-RADIO: configure own MAC first"
  ?MAC BSS-BYTES 6 MOVE
  RA-ENABLE MAC $24 + BIC!
  BSS-BYTES MAC-LOW MAC !
  MAC 4 + @ $FFFF0000 AND BSS-BYTES 4 + DUP C@ SWAP 1+ C@ 8 LSHIFT OR OR MAC 4 + !
  -1 MAC $20 + ! MAC $24 + @ $FFFF0000 AND $FFFF OR RA-ENABLE OR MAC $24 + !
  FILTER @ FILTER-BITS INVERT AND 7 OR FILTER !
  CTRL-TYPES CTRL-FILTER BIC! FALSE TO BEACONS? TRUE TO STATION? ;
: RADIO-FACTORY ( -- )
  ?ON ?CONFIG
  $3FF5A008 @ DUP 8 RSHIFT MAC-BYTES C! MAC-BYTES 1+ C!
  $3FF5A004 @ DUP 24 RSHIFT MAC-BYTES 2 + C! DUP 16 RSHIFT MAC-BYTES 3 + C!
  DUP 8 RSHIFT MAC-BYTES 4 + C! MAC-BYTES 5 + C! MAC-BYTES SET-MAC ;
: RADIO-RANDOM ( a n -- )
  ?ON DUP 0 1025 WITHIN 0= ABORT" ESP32-RADIO: random length must be 0 to 1024"
  0 ?DO
    I 3 AND 0= IF 1 MS RNG-DATA @ RANDOM-WORD ! THEN
    RANDOM-WORD @ I 3 AND 8 * RSHIFT OVER I + C!
  LOOP DROP 0 RANDOM-WORD ! ;
: RADIO-RECEIVE ( -- a n | 0 )   \ the next frame: n bytes at a, until the next RADIO-RECEIVE
  ?ON  HELD -1 <> IF  HELD GIVE  -1 TO HELD  THEN
  END-DESC  DUP 0< IF  DROP 0 EXIT  THEN
  DUP HEAD <> IF  DROP-LONG 0 EXIT  THEN
  DROP TAKE  DUP TO HELD  RX-FRAME  2DUP WANTED? IF  HELD BUF >LEVEL TO LEVEL  EXIT  THEN
  2DROP  HELD GIVE  -1 TO HELD  1 #OTHER +!  0 ;
: RADIO-LEVEL ( -- dbm )   LEVEL ;   \ the level of the frame of the last RADIO-RECEIVE
: RADIO-MAC! ( a -- )   ?ON  ?CONFIG  SET-MAC ;
: RADIO-MAC@ ( -- a )
  ?ON  MAC-SET? 0= ABORT" ESP32-RADIO: own MAC is not configured"  MAC-BYTES ;
: RADIO-SEND ( a n ack? -- )
  ?ON
  RNG-DATA @ 15 AND TX-PREPARE
  TX-EXECUTE ;
: RADIO-SOFT-CCMP ( -- )
  ?ON ?CONFIG STATION? MAC-SET? AND 0= ABORT" ESP32-RADIO: software CCMP needs station mode"
  TRUE TO SOFT-CCMP? ;
: RADIO-TX-START ( a n ack? -- )
  ?ON RNG-DATA @ 15 AND TX-PREPARE TX-KICK ;
: RADIO-TX-BUSY? ( -- f ) TX-ACTIVE? ;
: RADIO-TX-BLOCKED? ( -- f ) TX-BLOCKED? ;
: RADIO-TX-CANCEL ( -- ) TX-ACTIVE? IF TX-CANCEL THEN ;
: RADIO-TX-POLL ( -- status )
  ?ON TX-ACTIVE? 0= ABORT" ESP32-RADIO: no asynchronous TX"
  TX-RESULT @ 0= IF
    MILLIS TX-SINCE - TX-WAIT-MS U< IF 1 EXIT THEN
    TX-NO-END TX-RESULT ! TRUE TO TX-BLOCKED?
  THEN
  TX-RELEASE
  TX-RESULT @ CASE
    TX-NO-END OF -433 ENDOF
    TX-ACCESS-TIMEOUT OF -435 ENDOF
    TX-COLLISION OF -436 ENDOF
    TX-COMPLETE OF
      1 #TX +! 1 TX-TRACK +!
      TX-LAST-PMD @ ACK-TIMEOUT? IF -434 ELSE
        TX-LAST-PMD @ $F000 AND IF -437 ELSE 0 THEN
      THEN
    ENDOF
    >R -437 R>
  ENDCASE ;
: RADIO-TRACK ( -- )
  ?ON TX-ACTIVE? 0= TX-TRACK @ 5 < 0= AND IF PHY-TRACK 0 TX-TRACK ! THEN ;
: RADIO-STOP ( -- )   \ the radio off
  ON? IF
    TX-ACTIVE? IF TX-CANCEL ELSE 0 TX-CTRL ! THEN
    RX-ON RX-CTRL BIC!  MAC-SOURCE INT-OFF  PHY-OFF
    TX-BLOCKED? IF MAC-RESET THEN
  THEN
  FALSE TO ON?  0 TO CHANNEL  -1 TO HELD  FALSE TO BEACONS? FALSE TO STATION?
  FALSE TO SOFT-CCMP? ;
END-PACKAGE
: RADIO ( -- )   ESP32-RADIO ;   \ ALSO RADIO: the radio of this chip
