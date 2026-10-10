\ wifi.fs - package WIFI: the Wi-Fi networks around (contract WIFI, first part)
\
\ Over the contract RADIO: the radio of the chip (on the ESP32 the package
\ ESP32-RADIO, lib/esp32/esp32/radio.fs). The words:
\
\   WIFI-SCAN ( -- u )            listen on the channels 1 to 13: u networks
\   WIFI-NET ( i -- a n ch dbm )  network i (0 to u-1, the strongest first):
\                                 its name (n bytes at a), channel and level
\   .WIFI ( -- )                  the list: channel, level in dBm, name
\   WIFI-WPA2? ( i -- f )         PSK/CCMP, without required PMF
\   WIFI-PEER ( i -- a )          the selected BSSID (read-only)
\   WIFI-RSN ( i -- a n cap )     RSN body and capabilities, for a supported AP
\
\ An access point sends a beacon to all, about ten times each second
\ (every 102.4 ms): its address (the BSSID), the name of its network (the
\ SSID) and its channel. WIFI-SCAN sets the filter of a scan
\ (RADIO-BEACONS), stays 120 ms on each channel and notes each access
\ point once, by its BSSID, with its strongest level. The channel comes
\ from the beacon (its element "DS parameter set"), because a beacon also
\ comes on a near channel, weaker. A network without a name (hidden) is
\ not in the list. The list has room for 32 networks. At the end the
\ radio is on its channel again, and the filter of the scan stays.
\
\ The radio must be on. On the ESP32, in ucf term, in the folder ucf,
\ right after a reset:
\   #include lib/esp32/esp32/phy.fs
\   #include lib/net/bytes.fs
\   #include lib/esp32/esp32/radio.fs
\   #include lib/net/wifi.fs
\   RADIO-START  WIFI-SCAN .  .WIFI
\ The test on the host: lib/net/test/wifi.fs.
\
\ A beacon as RADIO-RECEIVE gives it (IEEE 802.11), from byte 0:
\   0   2 bytes frame control: $80 $00 (byte 1 bit 7: 4 bytes more at 24)
\   2   2 bytes duration      4  6 bytes to: all      10  6 bytes from
\   16  6 bytes the BSSID     22 2 bytes sequence
\   24  8 bytes time, 2 bytes interval, 2 bytes capabilities
\   36  the elements: id (1 byte), length (1 byte), the data; id 0 is
\       the SSID (0 to 32 bytes), id 3 the channel (1 byte)

PACKAGE WIFI  ALSO RADIO  ALSO BYTES

176 CONSTANT /NET                \ identity, capabilities and up to 128 RSN bytes
32 CONSTANT #MAX                 \ the networks in the list, at most
#MAX /NET * BUFFER: ENTRIES      \ the list
VARIABLE #NETS                   \ the networks in the list
/NET BUFFER: SPARE               \ room for one network while the list is sorted
120 VALUE DWELL                  \ the time on each channel, in ms

\ ---- a network in the list: its level (a cell), channel, the length of
\ ---- its name, its BSSID, its name
: ENTRY ( i -- a )   /NET * ENTRIES + ;
: >CHAN ( a -- a' )   4 + ;
: >NLEN ( a -- a' )   5 + ;
: >BSSID ( a -- a' )   6 + ;
: >SSID ( a -- a' )   12 + ;
: >CAP ( a -- a' )   44 + ;
: >SEC ( a -- a' )   46 + ;
: >RLEN ( a -- a' )  47 + ;
: >RSN ( a -- a' )   48 + ;
: LE16@ ( a -- u )  DUP C@ SWAP 1+ C@ 8 LSHIFT OR ;
CREATE CCMP-SUITE 0 C, $0F C, $AC C, 4 C,
CREATE PSK-SUITE 0 C, $0F C, $AC C, 2 C,
VARIABLE RSN-A VARIABLE RSN-N VARIABLE RSN-WANT
: RSN-TAKE ( n -- a )
  RSN-A @ SWAP DUP NEGATE RSN-N +! RSN-A +! ;
: RSN-LIST ( suite -- f )
  RSN-WANT ! RSN-N @ 2 < IF FALSE EXIT THEN
  2 RSN-TAKE LE16@ DUP 1 < OVER 32 > OR IF DROP FALSE EXIT THEN
  DUP 4 * RSN-N @ > IF DROP FALSE EXIT THEN
  FALSE SWAP 0 DO 4 RSN-TAKE RSN-WANT @ 4 BYTES= OR LOOP ;
: RSN-WPA2? ( a n -- f )
  DUP 18 129 WITHIN 0= IF 2DROP FALSE EXIT THEN RSN-N ! RSN-A !
  2 RSN-TAKE LE16@ 1 <> IF FALSE EXIT THEN
  4 RSN-TAKE CCMP-SUITE 4 BYTES= 0= IF FALSE EXIT THEN
  CCMP-SUITE RSN-LIST 0= IF FALSE EXIT THEN
  PSK-SUITE RSN-LIST 0= IF FALSE EXIT THEN
  RSN-N @ 0= IF TRUE EXIT THEN
  RSN-N @ 2 < IF FALSE EXIT THEN
  2 RSN-TAKE LE16@ $40 AND IF FALSE EXIT THEN
  RSN-N @ 0= IF TRUE EXIT THEN
  RSN-N @ 2 < IF FALSE EXIT THEN
  2 RSN-TAKE LE16@ 16 * DUP RSN-N @ > IF DROP FALSE EXIT THEN
  RSN-TAKE DROP RSN-N @ DUP 0= SWAP 4 = OR ;

\ ---- a beacon
: SKIP-BYTES ( a n k -- a' n' )   TUCK - >R + R> ;
: ELEMENTS ( a n -- a' n' )   \ the elements of a beacon
  OVER 1+ C@ $80 AND IF 40 ELSE 36 THEN  SKIP-BYTES ;
: FITS? ( a n -- f )   \ an element begins at a, inside the n bytes
  DUP 2 < IF  2DROP FALSE EXIT  THEN  SWAP 1+ C@ 2 + < 0= ;
: ELEMENT ( a n id -- a' u TRUE | FALSE )   \ the data of element id among the elements a n
  >R  BEGIN  2DUP FITS?  WHILE
    OVER C@ R@ = IF  DROP  DUP 2 +  SWAP 1+ C@  R> DROP TRUE EXIT  THEN
    OVER 1+ C@ 2 +  SKIP-BYTES
  REPEAT  2DROP  R> DROP FALSE ;
: BEACON? ( a n -- f )
  DUP 36 < IF 2DROP FALSE EXIT THEN
  OVER 1+ C@ $80 AND IF 40 ELSE 36 THEN < IF DROP FALSE EXIT THEN C@ $80 = ;
: BEACON-CH ( a n -- ch )   \ the channel of a beacon: its element 3, else the channel of the radio
  ELEMENTS 3 ELEMENT IF  IF C@ ELSE DROP 0 THEN  ELSE 0 THEN
  DUP 1 15 WITHIN 0= IF  DROP RADIO-CHANNEL@  THEN ;
: BEACON-SSID ( a n -- a' u )   \ the name in a beacon; u = 0: none (hidden)
  ELEMENTS 0 ELEMENT 0= IF  0 0 EXIT  THEN
  DUP 1 33 WITHIN 0= IF  DROP 0 EXIT  THEN
  OVER C@ 0= IF  DROP 0  THEN ;

\ ---- the list
: SAME? ( a1 a2 u -- f )   \ the u bytes at a1 and at a2 are the same
  BYTES= ;
: FIND-NET ( a -- i | -1 )   \ the network with the BSSID at a
  #NETS @ 0 ?DO  DUP I ENTRY >BSSID 6 SAME? IF  DROP I UNLOOP EXIT  THEN  LOOP  DROP -1 ;
: NEW-NET ( a n -- )   \ the beacon of a new access point: into the list, if there is room
  #NETS @ #MAX = IF  2DROP EXIT  THEN
  #NETS @ ENTRY DUP /NET 0 FILL >R  1 #NETS +!
  OVER DUP 1+ C@ $80 AND IF 38 ELSE 34 THEN + LE16@ R@ >CAP !
  2DUP ELEMENTS 48 ELEMENT IF
    2 R@ >SEC C!
    2DUP RSN-WPA2? IF
      DUP R@ >RLEN C! R@ >RSN SWAP MOVE 1 R@ >SEC C!
    ELSE 2DROP THEN
  THEN
  OVER 16 + R@ >BSSID 6 MOVE  2DUP BEACON-CH R@ >CHAN C!
  BEACON-SSID  DUP R@ >NLEN C!  R@ >SSID SWAP MOVE  RADIO-LEVEL R> ! ;
: NOTE ( a n -- )   \ a frame from the radio: a beacon with a name goes into the list
  2DUP BEACON? 0= IF  2DROP EXIT  THEN
  2DUP BEACON-SSID NIP 0= IF  2DROP EXIT  THEN
  OVER 16 + FIND-NET  DUP 0< IF  DROP NEW-NET EXIT  THEN
  NIP NIP ENTRY  RADIO-LEVEL OVER @ MAX  SWAP ! ;   \ known: keep the strongest level
: EXCHANGE ( a1 a2 -- )   \ two networks change their places
  OVER SPARE /NET MOVE  TUCK SWAP /NET MOVE  SPARE SWAP /NET MOVE ;
: SORT ( -- )   \ the strongest first
  #NETS @ 0 ?DO  #NETS @ 1- 0 ?DO
    I ENTRY @  I 1+ ENTRY @ < IF  I ENTRY  I 1+ ENTRY EXCHANGE  THEN
  LOOP  LOOP ;
: LISTEN ( ms -- )   \ the beacons that come in ms into the list
  MILLIS +  BEGIN  RADIO-RECEIVE ?DUP IF NOTE THEN  DUP MILLIS - 0 <  UNTIL DROP ;
: .NAME ( a n -- )   \ a control character shows as ?
  0 ?DO  DUP I + C@  DUP 32 < IF  DROP 63  THEN  EMIT  LOOP DROP ;

PUBLIC
: WIFI-PEER ( i -- a )
  DUP #NETS @ U< 0= ABORT" WIFI: no such network" ENTRY >BSSID ;
: WIFI-WPA2? ( i -- f )
  DUP #NETS @ U< 0= ABORT" WIFI: no such network" ENTRY >SEC C@ 1 = ;
: WIFI-RSN ( i -- a n cap )
  DUP WIFI-WPA2? 0= ABORT" WIFI: WPA2-PSK/CCMP without required PMF needed"
  ENTRY DUP >RSN OVER >RLEN C@ ROT >CAP LE16@ ;
: WIFI-SCAN ( -- u )   \ listen on the channels 1 to 13: u networks, the strongest first
  RADIO-CHANNEL@ >R  RADIO-BEACONS  0 #NETS !
  14 1 DO  I RADIO-CHANNEL!  DWELL LISTEN  LOOP
  R> RADIO-CHANNEL!  SORT  #NETS @ ;
: WIFI-NET ( i -- a n ch dbm )   \ network i of the last WIFI-SCAN: name, channel, level
  DUP #NETS @ U< 0= ABORT" WIFI: no such network"
  ENTRY  DUP >SSID OVER >NLEN C@  ROT DUP >CHAN C@  SWAP @ ;
: .WIFI ( -- )   \ the networks of the last WIFI-SCAN: channel, level in dBm, name
  #NETS @ 0 ?DO  I WIFI-NET  SWAP 3 .R  5 .R  2 SPACES  .NAME CR  LOOP ;
END-PACKAGE
