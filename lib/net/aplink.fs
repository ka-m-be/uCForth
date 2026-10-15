\ aplink.fs - package AP-LINK: the contract LINK over an open access point for one device
\
\ WIFI-AP makes the radio an access point (802.11) with a name, open, without
\ encryption: it sends beacons (each 102 ms), answers probes, lets the first
\ device log on (authentication, association) and then only that device,
\ until WIFI-AP-STOP. Our TCP/IP (LINK-NET) sends and receives Ethernet
\ frames over it, as over any LINK; there is no routing to another network.
\ The radio keeps the other devices out: in its AP mode (RADIO-AP) it gives
\ only authentications and probes to the program, and after RADIO-AP-PEER!
\ only the frames of that device. AP-LINK does the rest: the replies, the
\ state of the device, the data frames. It supports the legacy OFDM rates
\ (6 to 54 Mbit/s) and frames without fragments and without A-MSDU.
\ SETUP-AP (lib/esp32/esp32/wlap.fs) uses it for the setup network.
\
\   a AP-POOL! ( -- )          the packet RAM: AP-POOL-SIZE bytes at a; it may be the
\                              RAM of WIFI-LINK (only one of them runs)
\   a n ch WIFI-AP ( -- )      the AP with the name a n (1 to 32 bytes) on channel ch
\   WIFI-AP-STOP ( -- )        a deauthentication to the device (a device in power
\                              save can miss it); the radio goes off
\   WIFI-AP-READY? ( -- f )    the device has logged on (associated)
\   WIFI-AP-PEER ( -- a )      its MAC address
\   LINK-MAC LINK-MTU LINK-UP? LINK-SEND LINK-RECEIVE LINK-JOIN   the contract LINK
\   LINK-TICK ( -- )           a frame in, a frame out, a beacon (LINK-NET runs it)
\   LINK-CONTROL ( -- )        the foreground part (PHY-TRACK)
\   .AP ( -- )                 the state of the device (0 none, 1 authenticated,
\                              2 associated), frames in, out, dropped, the last code
\
\ A frame to all (broadcast) goes to the one device as a frame to it.
\ Load it in ucf term after WIFI-LINK (apps/irbridge/app.fs does it);
\ RADIO-LINK (linksel.fs) gives it its RAM. The test on the host:
\ lib/esp32/test/aphost.fs.

PACKAGE AP-LINK ALSO RADIO ALSO BYTES ALSO INET
1514 CONSTANT FRAME-SIZE              \ an Ethernet frame
1600 CONSTANT AT-REQUEST              \ the pool: the frame to send (1600 bytes), a request,
AT-REQUEST 512 + CONSTANT AT-RX       \ 2 frames received,
AT-RX 2 FRAME-SIZE * + CONSTANT AT-ETH   \ 2 frames to send
6 BUFFER: OWN
6 BUFFER: PEER                        \ the device
32 BUFFER: NAME
VARIABLE NAME-N
VARIABLE POOL
VARIABLE RUNNING
VARIABLE AP-STATE                     \ 0 none, 1 authenticated, 2 associated
VARIABLE BOUND                        \ the device is fixed (PEER)
VARIABLE SEQUENCE
VARIABLE BEACON-AT
VARIABLE TX-N VARIABLE TX-ACK VARIABLE TX-PENDING   \ the frame to send: its bytes, ACK, it waits
VARIABLE RX-W VARIABLE RX-R
VARIABLE ETH-W VARIABLE ETH-R
2 CELLS BUFFER: ETH-LENS
2 CELLS BUFFER: RX-LENS
VARIABLE ERROR
VARIABLE RX-COUNT VARIABLE TX-COUNT VARIABLE DROPPED
VARIABLE ACTION                       \ the AP-STATE when the device has the reply (ACK)
VARIABLE LENGTH
VARIABLE REQUEST-N                    \ a management frame waits; its bytes
VARIABLE LAST-SEQUENCE VARIABLE HAVE-SEQUENCE   \ a repeated data frame comes only once
: TX ( -- a ) POOL @ ;
: REQUEST ( -- a ) POOL @ AT-REQUEST + ;
: RX-BUF ( i -- a ) 1 AND FRAME-SIZE * AT-RX + POOL @ + ;
: RX-LEN ( i -- a ) 1 AND CELLS RX-LENS + ;
: ETH-BUF ( i -- a ) 1 AND FRAME-SIZE * AT-ETH + POOL @ + ;
: ETH-LEN ( i -- a ) 1 AND CELLS ETH-LENS + ;
: NOTE ( ior -- ) ERROR ! 1 DROPPED +! ;
: TX-HEADER ( type flags receiver -- )   \ the header of a frame from the AP
  TX 1600 0 FILL TX 4 + 6 MOVE TX 1+ C! TX C!
  OWN TX 10 + 6 MOVE OWN TX 16 + 6 MOVE
  SEQUENCE @ $FFF AND 4 LSHIFT TX 22 + LE16! 1 SEQUENCE +! ;
: RATES ( a -- a' )   \ the element of the rates: 6 to 54 Mbit/s; 6, 12 and 24 basic
  1 OVER C! 8 OVER 1+ C!
  $8C OVER 2 + C! $12 OVER 3 + C! $98 OVER 4 + C! $24 OVER 5 + C!
  $B0 OVER 6 + C! $48 OVER 7 + C! $60 OVER 8 + C! $6C OVER 9 + C! 10 + ;
: ADVERTISE ( type receiver -- )   \ a beacon or a probe response: time, interval, name, rates, channel, TIM
  >R 0 R> TX-HEADER
  MILLIS 1000 UM* TX 28 + ! TX 24 + !
  100 TX 32 + LE16! $401 TX 34 + LE16!               \ 100 TU; ESS, short slot time
  0 TX 36 + C! NAME-N @ TX 37 + C! NAME TX 38 + NAME-N @ MOVE
  TX 38 + NAME-N @ + RATES
  3 OVER C! 1 OVER 1+ C! RADIO-CHANNEL@ OVER 2 + C! 3 +
  5 OVER C! 4 OVER 1+ C! 1 OVER 3 + C! 6 +           \ TIM: DTIM count 0, period 1
  TX - TX-N ! TX-ACK OFF! TX-PENDING ON! ;
6 BUFFER: ALL
ALL 6 $FF FILL
: BEACON ( -- ) $80 ALL ADVERTISE MILLIS 102 + BEACON-AT ! ;
: RESPONSE ( type n action -- )   \ a reply to the device; action: its state after the ACK
  ACTION ! >R 0 PEER TX-HEADER R> TX-N ! 60 TX 2 + LE16!
  TX-ACK ON! TX-PENDING ON! ;
: AUTH ( a n -- )   \ open authentication, step 1; the first one fixes the device
  DUP 30 < IF 2DROP -439 THROW THEN
  OVER 24 + LE16@ IF 2DROP -439 THROW THEN
  OVER 26 + LE16@ 1 <> IF 2DROP -439 THROW THEN
  BOUND @ 0= IF  OVER 10 + DUP RADIO-AP-PEER! PEER 6 MOVE BOUND ON!  THEN
  2DROP
  $B0 30 1 RESPONSE 2 TX 26 + LE16! ;
: NAME-MATCH? ( a n -- f )   \ the elements a n hold our name (element 0)
  BEGIN DUP 2 < 0= WHILE
    OVER 1+ C@ 2 + OVER > IF 2DROP FALSE EXIT THEN
    OVER C@ 0= IF
      DROP DUP 1+ C@ NAME-N @ <> IF DROP FALSE EXIT THEN
      2 + NAME NAME-N @ BYTES= EXIT
    THEN
    OVER 1+ C@ 2 + >R R@ - SWAP R> + SWAP
  REPEAT 2DROP FALSE ;
: ASSOC ( a n -- )   \ (re)association after the authentication: AID 1
  OVER C@ $20 = IF 34 ELSE 28 THEN LENGTH !
  DUP LENGTH @ < IF 2DROP -439 THROW THEN
  AP-STATE @ 1 < IF 2DROP -439 THROW THEN
  LENGTH @ - SWAP LENGTH @ + SWAP
  NAME-MATCH? 0= IF -439 THROW THEN
  LENGTH @ 34 = IF $30 ELSE $10 THEN
  40 2 RESPONSE $401 TX 24 + LE16! $C001 TX 28 + LE16!
  TX 30 + RATES DROP ;
: DATA-IN ( a n -- )   \ a data frame of the device: an Ethernet frame into the queue
  AP-STATE @ 2 <> IF 2DROP EXIT THEN
  OVER 1+ C@ $47 AND 1 <> IF 2DROP -439 THROW THEN    \ to the AP, no fragments, not protected
  OVER C@ DUP 8 = SWAP $88 = OR 0= IF 2DROP EXIT THEN
  OVER 22 + LE16@ $F AND IF 2DROP -439 THROW THEN
  OVER 1+ C@ 8 AND HAVE-SEQUENCE @ AND IF
    OVER 22 + LE16@ LAST-SEQUENCE @ = IF 2DROP EXIT THEN
  THEN
  OVER C@ $80 AND IF 26 ELSE 24 THEN LENGTH !
  OVER 1+ C@ $80 AND IF 4 LENGTH +! THEN
  DUP LENGTH @ 8 + < IF 2DROP -439 THROW THEN
  DUP LENGTH @ - 8 - 1501 U< 0= IF 2DROP -439 THROW THEN
  LENGTH @ 24 > IF OVER 24 + C@ $80 AND IF 2DROP -439 THROW THEN THEN   \ no A-MSDU
  OVER LENGTH @ + DUP C@ $AA = OVER 1+ C@ $AA = AND
  OVER 2 + C@ 3 = AND OVER 3 + C@ 0= AND OVER 4 + C@ 0= AND SWAP 5 + C@ 0= AND
  0= IF 2DROP -439 THROW THEN
  RX-W @ RX-R @ - 2 = IF 2DROP -441 THROW THEN
  OVER 22 + LE16@ LAST-SEQUENCE ! HAVE-SEQUENCE ON!
  DUP LENGTH @ - 6 + RX-W @ RX-LEN !
  RX-W @ RX-BUF >R
  OVER 16 + R@ 6 MOVE PEER R@ 6 + 6 MOVE
  OVER LENGTH @ + 6 + R@ 12 + 2 MOVE
  LENGTH @ 8 + - SWAP LENGTH @ 8 + + R@ 14 + ROT MOVE
  R> DROP 1 RX-W +! 1 RX-COUNT +! ;
: INPUT ( a n -- )   \ a management frame (the radio let only ours in)
  DUP 24 < IF 2DROP -439 THROW THEN
  OVER C@ CASE
    $B0 OF AUTH ENDOF                                \ authentication
    $00 OF ASSOC ENDOF                               \ association
    $20 OF ASSOC ENDOF                               \ reassociation
    $40 OF DROP 10 + $50 SWAP ADVERTISE ENDOF        \ probe: probe response
    $A0 OF 2DROP 0 AP-STATE ! ENDOF                  \ disassociation
    $C0 OF 2DROP 0 AP-STATE ! ENDOF                  \ deauthentication
    $08 OF DATA-IN ENDOF
    $88 OF DATA-IN ENDOF
    >R 2DROP R>
  ENDCASE ;
: SAY-BYE ( -- )   \ tell the device that the network ends (deauthentication, reason 3)
  BOUND @ 0= RADIO-CHANNEL@ 0= OR IF EXIT THEN
  $C0 0 PEER TX-HEADER  3 TX 24 + LE16!
  8 0 DO  RADIO-RECEIVE ?DUP IF 2DROP THEN  LOOP
  TX 26 TRUE ['] RADIO-SEND CATCH IF 2DROP DROP THEN ;   \ the device may be gone already
: SEND-DATA ( a n -- )   \ an Ethernet frame into a data frame to the device
  $08 2 PEER TX-HEADER
  60 TX 2 + LE16!
  OVER 12 + TX 30 + 2 MOVE
  $AA TX 24 + C! $AA TX 25 + C! 3 TX 26 + C!
  14 - DUP 32 + TX-N ! SWAP 14 + TX 32 + ROT MOVE
  TX-ACK ON! TX-PENDING ON! 0 ACTION ! ;
: TICK ( -- )   \ the end of a send; a frame in; then a reply, a data frame or a beacon out
  RADIO-TX-BUSY? IF
    RADIO-TX-POLL DUP 1 <> IF
      DUP IF DUP -433 = IF RUNNING OFF! THEN NOTE 0 ACTION ! ELSE DROP
        ACTION @ ?DUP IF AP-STATE ! 0 ACTION ! THEN 1 TX-COUNT +!
      THEN
    ELSE DROP THEN
  THEN
  RUNNING @ 0= IF EXIT THEN
  RADIO-RECEIVE ?DUP IF
    OVER C@ 12 AND 8 = IF DATA-IN ELSE
      DUP 512 > REQUEST-N @ 0<> OR IF 2DROP -441 NOTE ELSE
        DUP REQUEST-N ! REQUEST SWAP MOVE
      THEN
    THEN
  THEN
  RADIO-TX-BUSY? IF EXIT THEN
  TX-PENDING @ 0= REQUEST-N @ 0<> AND IF
    REQUEST REQUEST-N @ 0 REQUEST-N ! INPUT
  THEN
  TX-PENDING @ 0= ETH-W @ ETH-R @ <> AND IF
    ETH-R @ ETH-BUF ETH-R @ ETH-LEN @ SEND-DATA 1 ETH-R +!
  THEN
  TX-PENDING @ 0= MILLIS BEACON-AT @ - 0< 0= AND IF BEACON THEN
  TX-PENDING @ IF
    TX TX-N @ TX-ACK @ RADIO-TX-START TX-PENDING OFF!
  THEN ;
PUBLIC
AT-ETH 2 FRAME-SIZE * + CONSTANT AP-POOL-SIZE
: AP-POOL! ( a -- )   RUNNING @ ABORT" AP: stop before changing packet pool" POOL ! ;
: WIFI-AP-PEER ( -- a ) PEER ;
: WIFI-AP ( a n ch -- )
  RUNNING @ ABORT" AP: already running"
  POOL @ 0= ABORT" AP: packet pool not configured"
  RADIO-CHANNEL@ ABORT" AP: stop station and radio first"
  >R DUP 1 33 WITHIN 0= ABORT" AP: name length must be 1 to 32"
  DUP NAME-N ! NAME SWAP MOVE
  RADIO-START RADIO-FACTORY R> RADIO-CHANNEL!
  RADIO-MAC@ OWN 6 MOVE RADIO-AP PEER 6 0 FILL BOUND OFF!
  POOL @ AP-POOL-SIZE 0 FILL
  0 AP-STATE ! 0 RX-W ! 0 RX-R ! 0 ETH-W ! 0 ETH-R !
  0 ACTION ! TX-PENDING OFF! 0 REQUEST-N ! HAVE-SEQUENCE OFF!
  MILLIS BEACON-AT !
  0 ERROR ! 0 RX-COUNT ! 0 TX-COUNT ! 0 DROPPED ! RUNNING ON! ;
: WIFI-AP-STOP ( -- )
  RUNNING @ IF  RUNNING OFF!  RADIO-TX-END  SAY-BYE  THEN  RADIO-STOP
  BOUND OFF! PEER 6 0 FILL
  0 AP-STATE ! TX-PENDING OFF! 0 RX-W ! 0 RX-R ! 0 ETH-W ! 0 ETH-R ! 0 REQUEST-N !
  POOL @ ?DUP IF AP-POOL-SIZE 0 FILL THEN ;
: WIFI-AP-READY? ( -- f ) RUNNING @ AP-STATE @ 2 = AND ;
: LINK-MAC ( -- a ) OWN ;
: LINK-MTU ( -- n ) 1500 ;
: LINK-UP? ( -- f ) WIFI-AP-READY? ;
: LINK-SEND ( a n -- ior )
  LINK-UP? 0= IF 2DROP NET-UNREACHABLE EXIT THEN
  ETH-W @ ETH-R @ - 2 = IF 2DROP NET-FAILED EXIT THEN
  DUP 14 1515 WITHIN 0= IF 2DROP NET-FAILED EXIT THEN
  OVER 6 + OWN 6 BYTES= 0= IF 2DROP NET-FAILED EXIT THEN
  OVER C@ 1 AND 0= IF OVER PEER 6 BYTES= 0= IF 2DROP NET-UNREACHABLE EXIT THEN THEN
  DUP ETH-W @ ETH-LEN ! ETH-W @ ETH-BUF SWAP MOVE 1 ETH-W +! 0 ;
: LINK-RECEIVE ( a -- n )
  RX-W @ RX-R @ <> IF
    RX-R @ RX-LEN @ >R RX-R @ RX-BUF SWAP R@ MOVE 1 RX-R +! R>
  ELSE DROP 0 THEN ;
: LINK-JOIN ( ip -- ior ) DROP 0 ;
: LINK-TICK ( -- )   \ the errors of a frame count in .AP; others go on
  RUNNING @ IF ['] TICK CATCH ?DUP IF
    DUP -439 = OVER -441 = OR IF ERROR ! 1 DROPPED +! ELSE DUP NOTE THROW THEN
  THEN THEN ;
: LINK-CONTROL ( -- ) RUNNING @ IF RADIO-TRACK THEN ;
: .AP ( -- )
  ." AP state/rx/tx/drop/code: " AP-STATE @ . RX-COUNT @ . TX-COUNT @ . DROPPED @ . ERROR @ . CR ;
END-PACKAGE
: LINK ( -- ) AP-LINK ;
