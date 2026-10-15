\ wifi.fs - the test of WIFI on the host: the list of the networks
\
\ WIFI (lib/net/wifi.fs) is over the contract RADIO. Here the package
\ MOCK-RADIO stands for the radio. The test builds the frames itself,
\ beacons of made-up access points (02:00:00:00:00:id) and other frames;
\ each one comes on a channel with a level (HEARD). After RADIO-CHANNEL!
\ RADIO-RECEIVE gives each frame of the channel once, then 0.
\ Each system with files and 32-bit cells runs it; in the folder ucf:
\   build/ucf-c -32 host/files.fs lib/net/test/wifi.fs </dev/null
\ At the end: "wifi: 25 tests, 0 errors".

ONLY FORTH DEFINITIONS  ALSO FILES
[DEFINED] T{ S" forth/test/tester.fs" ?INCLUDED
ONLY FORTH DEFINITIONS  ALSO FILES

PACKAGE MOCK-RADIO
2048 BUFFER: POOL                \ the frames
64 CONSTANT #AIR                 \ the frames that come, at most
#AIR 4 * CELLS BUFFER: AIR       \ each: the channel, the level, the address, the length
VARIABLE USED   VARIABLE START   VARIABLE #HEARD
VARIABLE ON   VARIABLE CH   VARIABLE POS   VARIABLE LEVEL
: REC ( i -- a )   4 CELLS * AIR + ;
: ?ON ( -- )   ON @ 0= ABORT" MOCK-RADIO: the radio is off" ;
PUBLIC
VARIABLE BEACONS-ON
: RADIO-START ( -- )   ON ON!  1 CH !  0 POS !  BEACONS-ON OFF! ;
: RADIO-STOP ( -- )   ON OFF!  BEACONS-ON OFF! ;
: RADIO-CHANNEL! ( ch -- )   ?ON  CH !  0 POS ! ;
: RADIO-CHANNEL@ ( -- ch )   ON @ IF CH @ ELSE 0 THEN ;
: RADIO-BEACONS ( -- )   ?ON  BEACONS-ON ON! ;
: RADIO-RECEIVE ( -- a n | 0 )   \ the next frame on the channel
  ?ON  BEGIN  POS @ #HEARD @ <  WHILE
    POS @ REC  1 POS +!
    DUP @ CH @ = IF  CELL+ DUP @ LEVEL !  CELL+ DUP @  SWAP CELL+ @  EXIT  THEN  DROP
  REPEAT  0 ;
: RADIO-LEVEL ( -- dbm )   LEVEL @ ;
\ ---- the frames of the test
: B, ( c -- )   POOL USED @ + C!  1 USED +! ;
: BYTES, ( a u -- )   0 ?DO  DUP I + C@ B,  LOOP DROP ;
: ZEROS, ( u -- )   0 ?DO  0 B,  LOOP ;
: ADDR, ( id -- )   2 B,  4 ZEROS,  B, ;
: HEADER ( fc0 fc1 id -- )   \ a new frame: control, duration, to all, from and BSSID id, sequence, time ...
  USED @ START !  >R  OVER B,  DUP B,  NIP  2 ZEROS,  6 0 DO $FF B, LOOP  R@ ADDR,  R> ADDR,  2 ZEROS,
  $80 AND IF  4 ZEROS,  THEN  8 ZEROS,  $64 B, 0 B,  $11 B, 4 B, ;
: IE, ( a u id -- )   B,  DUP B,  BYTES, ;
: RATES, ( -- )   1 B,  8 B,  8 0 DO $82 B, LOOP ;   \ element 1, as in a real beacon
: DS, ( ch -- )   3 B,  1 B,  B, ;
: BUILT ( -- a n )   POOL START @ +  USED @ START @ - ;   \ the frame built last
: CUT ( n -- )   START @ +  USED ! ;   \ the frame built last: only its first n bytes
: HEARD ( ch dbm -- )   \ the frame built last comes on channel ch with level dbm
  #HEARD @ REC >R  R@ CELL+ !  R@ !  BUILT R@ 3 CELLS + !  R> 2 CELLS + !  1 #HEARD +! ;
END-PACKAGE
: RADIO ( -- )   MOCK-RADIO ;

INCLUDE lib/net/bytes.fs
INCLUDE lib/net/wifi.fs
ALSO WIFI.I  ALSO MOCK-RADIO
2 TO DWELL

: NAME? ( a u ca cu -- f )   ROT OVER <> IF  2DROP DROP FALSE EXIT  THEN  BYTES= ;
: NET? ( i ca cu -- ch dbm f )   \ network i: its channel, its level; TRUE if its name is ca cu
  ROT WIFI-NET >R >R  NAME?  R> R> ROT ;

\ The parts of a beacon: its elements, its channel, its name.
T{ $80 0 1 HEADER  S" alpha" 0 IE,  RATES,  6 DS,  BUILT NIP -> 56 }T
T{ BUILT BEACON?  BUILT BEACON-CH  BUILT BEACON-SSID S" alpha" NAME? -> TRUE 6 TRUE }T
T{ BUILT ELEMENTS 1 ELEMENT ROT DROP  BUILT ELEMENTS 7 ELEMENT -> 8 TRUE FALSE }T

\ The air around: alpha also comes on the near channels, weaker; beta;
\ a hidden network (no name, and a name of zeros); gamma without its
\ channel (then the channel of the radio); a probe response (no beacon);
\ eps with 4 bytes more in the header (bit 7 of byte 1); alpha again from
\ another access point; an element longer than the frame; a frame too
\ short for a beacon.
5 -80 HEARD  6 -40 HEARD  7 -75 HEARD
$80 0 2 HEADER  S" beta" 0 IE,  RATES,  1 DS,  1 -60 HEARD
$80 0 3 HEADER  S" " 0 IE,  RATES,  11 DS,  11 -50 HEARD
$80 0 4 HEADER  0 B, 5 B, 5 ZEROS,  RATES,  11 DS,  11 -51 HEARD
$80 0 5 HEADER  S" gamma" 0 IE,  RATES,  11 -70 HEARD
$50 0 6 HEADER  S" delta" 0 IE,  RATES,  3 DS,  3 -30 HEARD
$80 $80 7 HEADER  S" eps" 0 IE,  RATES,  9 DS,  9 -65 HEARD
$80 0 8 HEADER  S" alpha" 0 IE,  RATES,  11 DS,  11 -85 HEARD
$80 0 9 HEADER  0 B, 40 B,  S" zeta" BYTES,  13 -20 HEARD
$80 0 10 HEADER  30 CUT  13 -20 HEARD

T{ BUILT BEACON?  $80 $80 7 HEADER S" eps" 0 IE, 9 DS,  BUILT BEACON-CH -> FALSE 9 }T
T{ BUILT BEACON-SSID S" eps" NAME?  $80 0 9 HEADER 0 B, 40 B,  BUILT BEACON-SSID NIP -> TRUE 0 }T

\ While the radio is off: an error.
T{ ' WIFI-SCAN CATCH -> -2 }T

\ The scan: each access point once, the strongest first, with the
\ channel from its beacon and its strongest level.
T{ RADIO-START  6 RADIO-CHANNEL!  WIFI-SCAN -> 5 }T
T{ 0 S" alpha" NET? -> 6 -40 TRUE }T
T{ 1 S" beta" NET? -> 1 -60 TRUE }T
T{ 2 S" eps" NET? -> 9 -65 TRUE }T
T{ 3 S" gamma" NET? -> 11 -70 TRUE }T
T{ 4 S" alpha" NET? -> 11 -85 TRUE }T
T{ 5 ' WIFI-NET CATCH NIP -> -2 }T
T{ RADIO-CHANNEL@  BEACONS-ON @ -> 6 TRUE }T   \ the channel as before; the filter stays

\ A network by its name: the strongest one with WPA2.
T{ S" alpha" WIFI-FIND -> FALSE }T                  \ none of them has WPA2
1 4 ENTRY >SEC C!
T{ S" alpha" WIFI-FIND  S" alph" WIFI-FIND  S" delta" WIFI-FIND -> 4 TRUE FALSE FALSE }T
1 0 ENTRY >SEC C!
T{ S" alpha" WIFI-FIND -> 0 TRUE }T
0 0 ENTRY >SEC C!  0 4 ENTRY >SEC C!

\ Room for 32 networks: 30 more on channel 12, the last channel with
\ beacons; 27 of them find room, the others are not in the list.
: MANY ( -- )   30 0 DO  $80 0 I 100 + HEADER  S" n" 0 IE,  12 DS,  12 -70 HEARD  LOOP ;
T{ MANY  WIFI-SCAN -> 32 }T
T{ 0 S" alpha" NET? -> 6 -40 TRUE }T
2 TO DWELL
: STEPPED ( -- ) BEGIN WIFI-SCAN-STEP UNTIL ;
T{ WIFI-SCAN-START SCANNING @ -> TRUE }T
T{ ' WIFI-SCAN-START CATCH -> -2 }T
T{ STEPPED #NETS @ RADIO-CHANNEL@ SCANNING @ -> 32 6 FALSE }T
T{ ' WIFI-SCAN-STEP CATCH -> -2 }T
T{ WIFI-SCAN-START WIFI-SCAN-CANCEL SCANNING @ -> FALSE }T
T{ WIFI-SCAN-START WIFI-SCAN-STEP DROP WIFI-SCAN-CANCEL RADIO-CHANNEL@ -> 6 }T   \ back on its channel
RADIO-STOP

S" wifi" .RESULTS
