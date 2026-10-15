\ wlink.fs - package WIFI-LINK: the contract LINK over the WLAN station
\
\ Our TCP/IP (LINK-NET) sends and receives Ethernet frames. The station of
\ WLAN (wpasta.fs) has joined an access point and holds the keys. WIFI-LINK
\ turns an Ethernet frame into a protected 802.11 data frame (CCMP) and back.
\ The radio does the CCMP itself when it has the keys (ESP32-RADIO: hardware
\ CCMP, RADIO-CCMP?); for a radio without it, lib/net/wlsoft.fs does it in
\ Forth, in steps. WIFI-LINK checks the addresses and the packet number of
\ each received frame (no frame twice), and that its key is still the same.
\ LINK-TICK does a small part of the work: one frame in, or one frame out, or
\ one step of the CCMP in software; LINK-NET runs it in each step of the
\ network (NET-STEP, a timer). LINK-CONTROL gives the EAPOL frames of a new
\ group key to WLAN; that work waits for the radio, so it runs only in the
\ foreground (NET-CONTROL, in the chain of IDLE). The answer of WLAN goes
\ back protected, as a frame of the queue: with the key of the pair in use,
\ the AP takes no EAPOL frame without CCMP.
\
\   a WLINK-POOL! ( -- )       the packet RAM: WLINK-POOL-SIZE bytes at a
\   WLINK-START ( -- )         start, after the join (WIFI-STATUS 6)
\   WLINK-STOP ( -- )          stop; a frame on the air ends first; the RAM is cleared
\   LINK-MAC LINK-MTU LINK-UP? LINK-SEND LINK-RECEIVE LINK-JOIN   the contract LINK
\   LINK-TICK ( -- )           the work in the step of the network
\   LINK-CONTROL ( -- )        the work in the foreground
\   WLINK-BEACON-AGE ( -- ms ) the time since the last beacon of the AP
\   .WLINK ( -- )              up, frames received, sent, dropped, old, the last code
\
\ "old" counts the frames with a packet number or a key that came before:
\ the AP sends a frame again when it did not hear our ACK. That is normal
\ on the air and no error. No key and no frame is ever shown.
\
\ Load it in ucf term after WLAN and CCMP (apps/irbridge/app.fs does it);
\ RADIO-LINK (linksel.fs) gives it its RAM. The tests on the host:
\ lib/net/test/wlink.fs (software CCMP), hwlink.fs (hardware CCMP).

PACKAGE WIFI-LINK ALSO RADIO ALSO WLAN ALSO WLAN.I ALSO BYTES ALSO CCMP ALSO INET
1600 CONSTANT RAW-SIZE                \ the pool: 2 frames from the radio,
2 RAW-SIZE * CONSTANT AT-RADIO        \ the frame for the radio,
AT-RADIO 1600 + CONSTANT AT-TX        \ 2 Ethernet frames to send,
1520 CONSTANT ETH-SIZE
AT-TX 2 ETH-SIZE * + CONSTANT AT-ETH  \ 2 Ethernet frames received,
AT-ETH 2 ETH-SIZE * + CONSTANT AT-CONTROL   \ an EAPOL frame for WLAN
0 CONSTANT J-IDLE                     \ the states of JOB: nothing to do
1 CONSTANT J-DECRYPT                  \ software CCMP: a received frame
2 CONSTANT J-ENCRYPT                  \ software CCMP: a frame to send
3 CONSTANT J-SEND                     \ the radio sends the frame
4 CONSTANT J-READY                    \ the frame waits for the radio
5 CONSTANT J-RECEIVED                 \ the radio has decrypted a received frame
VARIABLE POOL
VARIABLE RUNNING
VARIABLE RAW-W VARIABLE RAW-R         \ the queues: written and read, mod 2
VARIABLE TX-W VARIABLE TX-R
VARIABLE ETH-W VARIABLE ETH-R
2 CELLS BUFFER: RAW-LENS
2 CELLS BUFFER: RAW-DECRYPTED
2 CELLS BUFFER: TX-LENS
2 CELLS BUFFER: ETH-LENS
VARIABLE CONTROL-N                    \ the bytes of the EAPOL frame; 0: none
VARIABLE JOB
VARIABLE JOB-N VARIABLE JOB-H VARIABLE JOB-TID VARIABLE JOB-SLOT VARIABLE JOB-GEN
VARIABLE JOB-PLEN VARIABLE JOB-PN-A
VARIABLE TX-SEQ
VARIABLE ERROR
VARIABLE RX-GOOD VARIABLE TX-GOOD VARIABLE DROPPED
VARIABLE OLD                          \ frames with an old packet number or key: repeats of the AP
VARIABLE SEEN                         \ MILLIS of the last beacon of the AP
6 BUFFER: PN
5 17 * 6 * BUFFER: RX-PNS             \ the last packet number: 5 keys, 17 traffic classes
5 CELLS BUFFER: GENERATIONS
: RAW-BUF ( i -- a ) 1 AND RAW-SIZE * POOL @ + ;
: TX-BUF ( i -- a ) 1 AND ETH-SIZE * AT-TX + POOL @ + ;
: ETH-BUF ( i -- a ) 1 AND ETH-SIZE * AT-ETH + POOL @ + ;
: RADIO-BUF ( -- a ) POOL @ AT-RADIO + ;
: CONTROL ( -- a ) POOL @ AT-CONTROL + ;
: AT-LEN ( i table -- a ) SWAP 1 AND CELLS + ;
?: RADIO-CCMP? ( -- f ) FALSE ;       \ optional words of the contract RADIO
?: RADIO-DECRYPTED? ( -- f ) FALSE ;
: HARDWARE? ( -- f ) RADIO-CCMP? ;
: DECRYPTED? ( -- f ) RADIO-DECRYPTED? ;
: NO-SOFT ( -- )   TRUE ABORT" WLINK: CCMP needs the keys in the radio, or lib/net/wlsoft.fs" ;
: NO-RX ( key -- )   DROP -411 THROW ;
: NO-TX ( -- )   -411 THROW ;
DEFER SOFT-START   ' NO-SOFT IS SOFT-START   \ the CCMP in software (lib/net/wlsoft.fs sets them)
DEFER SOFT-RX      ' NO-RX IS SOFT-RX        \ ( key -- ) begin J-DECRYPT
DEFER SOFT-TX      ' NO-TX IS SOFT-TX        \ ( -- ) begin J-ENCRYPT
DEFER SOFT-STEP    ' NOOP IS SOFT-STEP       \ ( -- ) a step of J-DECRYPT or J-ENCRYPT
DEFER SOFT-STOP    ' NOOP IS SOFT-STOP       \ ( -- ) end and clear
: UP? ( -- f ) RUNNING @ WIFI-STATUS 6 = AND ;
: NOTE ( ior -- ) ERROR ! 1 DROPPED +! ;
: DROP-RAW ( -- ) 1 RAW-R +! ;
: DROP-TX ( -- ) 1 TX-R +! ;
: KEY-NOW ( -- generation )
  JOB-SLOT @ DUP 0= IF DROP 0 FALSE ELSE 1- TRUE THEN WIFI-RX-KEY
  NIP NIP ;
: NONCE-COUNTER ( slot tid -- a ) SWAP 17 * + 6 * RX-PNS + ;
: REPLAY-STATE ( key rsc generation -- key )   \ a new key starts its packet numbers at rsc
  JOB-GEN ! JOB-SLOT @ CELLS GENERATIONS + DUP @ JOB-GEN @ <> IF
    JOB-GEN @ OVER ! DROP
    17 0 DO DUP JOB-SLOT @ I NONCE-COUNTER 6 MOVE LOOP
  ELSE DROP THEN
  DROP JOB-SLOT @ JOB-TID @ NONCE-COUNTER JOB-PN-A ! ;
: CAPTURE ( -- )   \ a frame from the radio into the queue
  RADIO-RECEIVE ?DUP IF
    DUP 24 1573 WITHIN 0= IF 2DROP -439 NOTE EXIT THEN
    RAW-W @ RAW-R @ - 2 < IF
      DECRYPTED? RAW-W @ RAW-DECRYPTED AT-LEN !
      DUP RAW-W @ RAW-LENS AT-LEN ! RAW-W @ RAW-BUF SWAP MOVE 1 RAW-W +!
    ELSE 2DROP -441 NOTE THEN
  THEN ;
: SAVE-CONTROL ( a n -- )   \ an EAPOL frame for LINK-CONTROL
  CONTROL-N @ IF 2DROP -441 NOTE EXIT THEN
  DUP 512 > IF 2DROP -439 NOTE EXIT THEN
  >R CONTROL R@ MOVE R> CONTROL-N ! ;
: BEGIN-RX ( -- )   \ a protected frame: from the AP, to us, a new packet number
  RAW-R @ RAW-BUF RAW-R @ RAW-LENS AT-LEN @
  DUP JOB-N ! 2DUP CCMP-HEAD SWAP JOB-H ! JOB-TID !
  JOB-H @ 24 = IF 16 JOB-TID ! THEN
  OVER 10 + STA-PEER 6 BYTES= 0= IF 2DROP -439 THROW THEN
  OVER 4 + C@ 1 AND 0= IF OVER 4 + STA-OWN 6 BYTES= 0= IF 2DROP -439 THROW THEN THEN
  OVER JOB-H @ + PN CCMP-PN@
  >R OVER 4 + C@ 1 AND IF R> DUP 1+ JOB-SLOT ! TRUE
  ELSE R> DUP 0<> IF DROP 2DROP -439 THROW THEN 0 JOB-SLOT ! FALSE THEN
  WIFI-RX-KEY REPLAY-STATE
  JOB-PN-A @ PN PN< 0= IF DROP 2DROP -440 THROW THEN
  NIP NIP                                                ( key )
  HARDWARE? IF
    DROP  RAW-R @ RAW-DECRYPTED AT-LEN @ 0= IF -411 THROW THEN
    JOB-N @ JOB-H @ - 8 - JOB-PLEN !  J-RECEIVED JOB !  EXIT
  THEN
  SOFT-RX ;
: BEGIN-TX ( -- )   \ an Ethernet frame into a data frame to the AP, with the CCMP header
  RADIO-BUF 1600 0 FILL $08 RADIO-BUF C! $41 RADIO-BUF 1+ C!
  60 RADIO-BUF 2 + C! 0 RADIO-BUF 3 + C!
  STA-PEER RADIO-BUF 4 + 6 MOVE STA-OWN RADIO-BUF 10 + 6 MOVE
  TX-R @ TX-BUF RADIO-BUF 16 + 6 MOVE
  TX-SEQ @ $FFF AND 4 LSHIFT DUP RADIO-BUF 22 + C! 8 RSHIFT RADIO-BUF 23 + C! 1 TX-SEQ +!
  STA-TXPN PN 6 MOVE STA-TXPN PN-NEXT PN RADIO-BUF 24 + 0 CCMP-PN!
  24 JOB-H ! 16 JOB-TID ! 0 JOB-SLOT ! STA-PGEN @ JOB-GEN !
  $AA RADIO-BUF 32 + C! $AA RADIO-BUF 33 + C! 3 RADIO-BUF 34 + C!
  TX-R @ TX-BUF 12 + RADIO-BUF 38 + 2 MOVE
  TX-R @ TX-LENS AT-LEN @ 14 - DUP JOB-PLEN !
  TX-R @ TX-BUF 14 + RADIO-BUF 40 + ROT MOVE
  HARDWARE? IF J-READY JOB ! EXIT THEN
  SOFT-TX ;
: RX-DONE ( -- )   \ the plain text of a received frame: EAPOL to WLAN, else an Ethernet frame
  JOB-PLEN @ 8 < IF -439 THROW THEN
  PN JOB-PN-A @ 6 MOVE
  RAW-R @ RAW-BUF JOB-H @ 8 + +
  DUP C@ $AA = OVER 1+ C@ $AA = AND OVER 2 + C@ 3 = AND 0= IF DROP -439 THROW THEN
  DUP 3 + C@ OVER 4 + C@ OR OVER 5 + C@ OR IF DROP -439 THROW THEN
  DUP 6 + BE16@ $888E = IF
    RAW-R @ RAW-BUF DUP 1+ DUP C@ $40 INVERT AND SWAP C!
    2DUP JOB-H @ + JOB-PLEN @ MOVE
    JOB-H @ JOB-PLEN @ + SAVE-CONTROL DROP
  ELSE
    ETH-W @ ETH-R @ - 2 < IF
      ETH-W @ ETH-BUF >R
      RAW-R @ RAW-BUF 4 + R@ 6 MOVE RAW-R @ RAW-BUF 16 + R@ 6 + 6 MOVE
      DUP 6 + R@ 12 + 2 MOVE 8 + R@ 14 + JOB-PLEN @ 8 - MOVE
      JOB-PLEN @ 6 + ETH-W @ ETH-LENS AT-LEN ! 1 ETH-W +! R> DROP 1 RX-GOOD +!
    ELSE DROP -441 NOTE THEN
  THEN DROP-RAW J-IDLE JOB ! ;
: NEW-JOB ( -- )   \ the next frame from the radio, else the next to send
  RAW-W @ RAW-R @ <> IF
    RAW-R @ RAW-BUF DUP C@ $80 = IF  DROP DROP-RAW MILLIS SEEN ! EXIT  THEN   \ a beacon of the AP
    DUP C@ 12 AND 8 = IF
      DUP C@ DUP 8 = SWAP $88 = OR 0= IF  DROP DROP-RAW EXIT  THEN           \ data without data
    THEN
    DUP 1+ C@ $40 AND 0= IF                                     \ not protected: EAPOL, a management frame
      RAW-R @ RAW-LENS AT-LEN @ SAVE-CONTROL DROP-RAW EXIT                     \ the frame a and its length
    THEN DROP
    J-DECRYPT JOB ! BEGIN-RX
  ELSE
    TX-W @ TX-R @ <> IF J-ENCRYPT JOB ! BEGIN-TX THEN
  THEN ;
: JOB-ERROR ( n -- )   \ the frame of the job goes; a lost radio stops the link
  DUP -433 = OVER -438 = OR IF RUNNING OFF! THEN
  DUP -440 = IF DROP 1 OLD +! ELSE NOTE THEN
  SOFT-STOP  JOB @ DUP J-DECRYPT = SWAP J-RECEIVED = OR IF DROP-RAW ELSE JOB @ IF DROP-TX THEN THEN
  J-IDLE JOB ! ;
: TICK-WORK ( -- )
  CAPTURE
  JOB @ CASE
    J-IDLE OF NEW-JOB ENDOF
    J-DECRYPT OF KEY-NOW JOB-GEN @ <> IF -440 THROW THEN SOFT-STEP ENDOF
    J-ENCRYPT OF STA-PGEN @ JOB-GEN @ <> IF -440 THROW THEN SOFT-STEP ENDOF
    J-SEND OF
      RADIO-TX-POLL DUP 1 <> IF
        ?DUP IF JOB-ERROR ELSE 1 TX-GOOD +! DROP-TX J-IDLE JOB ! THEN
      ELSE DROP THEN
    ENDOF
    J-READY OF
      RADIO-TX-BUSY? 0= CONTROL-N @ 0= AND IF
        RADIO-BUF JOB-PLEN @ 48 + TRUE RADIO-TX-START J-SEND JOB !
      THEN
    ENDOF
    J-RECEIVED OF KEY-NOW JOB-GEN @ <> IF -440 THROW THEN RX-DONE ENDOF
  ENDCASE ;
: SEND-EAPOL ( a n -- f )   \ an EAPOL frame of WLAN (802.11, LLC) to the queue; false: the link is down
  UP? 0= IF 2DROP FALSE EXIT THEN
  TX-W @ TX-R @ - 2 < 0= IF -1 TX-W +! -441 NOTE THEN     \ full: it takes the place of the newest frame
  TX-W @ TX-BUF >R
  OVER 16 + R@ 6 MOVE  OVER 10 + R@ 6 + 6 MOVE  OVER 30 + R@ 12 + 2 MOVE   \ to, from, type
  32 - DUP 14 + TX-W @ TX-LENS AT-LEN !
  SWAP 32 + R> 14 + ROT MOVE  1 TX-W +! TRUE ;
' SEND-EAPOL IS STA-LINK-SEND

PUBLIC
AT-CONTROL 512 + CONSTANT WLINK-POOL-SIZE
: WLINK-POOL! ( a -- )
  RUNNING @ ABORT" WLINK: stop before changing packet pool" POOL ! ;
: LINK-MAC ( -- a ) STA-OWN ;
: LINK-MTU ( -- n ) 1500 ;
: LINK-UP? ( -- f ) UP? ;
: WLINK-BEACON-AGE ( -- ms ) MILLIS SEEN @ - ;
: LINK-SEND ( a n -- ior )
  UP? 0= IF 2DROP NET-UNREACHABLE EXIT THEN
  DUP 14 1515 WITHIN 0= IF 2DROP NET-FAILED EXIT THEN
  OVER 6 + STA-OWN 6 BYTES= 0= IF 2DROP NET-FAILED EXIT THEN
  TX-W @ TX-R @ - 2 < 0= IF 2DROP NET-FAILED EXIT THEN
  DUP TX-W @ TX-LENS AT-LEN ! TX-W @ TX-BUF SWAP MOVE 1 TX-W +! 0 ;
: LINK-RECEIVE ( a -- n )
  UP? ETH-W @ ETH-R @ <> AND IF
    ETH-R @ ETH-LENS AT-LEN @ >R ETH-R @ ETH-BUF SWAP R@ MOVE 1 ETH-R +! R>
  ELSE DROP 0 THEN ;
: LINK-JOIN ( ip -- ior )
  24 RSHIFT 224 240 WITHIN IF 0 ELSE NET-FAILED THEN ;
: LINK-TICK ( -- )   \ the errors of a frame count in .WLINK; others go on
  UP? IF ['] TICK-WORK CATCH ?DUP IF
    DUP -441 -432 WITHIN OVER -411 = OR IF
      JOB-ERROR ELSE DUP JOB-ERROR THROW THEN
  THEN THEN ;
: LINK-CONTROL ( -- )
  RADIO-TX-BUSY? IF EXIT THEN
  CONTROL-N @ ?DUP IF CONTROL SWAP WIFI-INPUT 0 CONTROL-N ! THEN RADIO-TRACK ;
: WLINK-START ( -- )
  POOL @ 0= ABORT" WLINK: packet pool not configured"
  WIFI-STATUS 6 <> ABORT" WLINK: join first"
  HARDWARE? 0= IF SOFT-START THEN
  POOL @ WLINK-POOL-SIZE 0 FILL
  RX-PNS 5 17 * 6 * 0 FILL GENERATIONS 5 CELLS 0 FILL
  0 RAW-W ! 0 RAW-R ! 0 TX-W ! 0 TX-R ! 0 ETH-W ! 0 ETH-R !
  0 CONTROL-N ! J-IDLE JOB ! 0 ERROR ! MILLIS SEEN !
  RUNNING ON! ;
: WLINK-STOP ( -- )
  RUNNING OFF! RADIO-TX-END SOFT-STOP J-IDLE JOB ! 0 CONTROL-N !
  POOL @ ?DUP IF WLINK-POOL-SIZE 0 FILL THEN ;
: .WLINK ( -- )
  ." WLINK up/rx/tx/drop/old/code: " UP? . RX-GOOD @ . TX-GOOD @ . DROPPED @ . OLD @ . ERROR @ . CR ;
END-PACKAGE
: LINK ( -- ) WIFI-LINK ;
