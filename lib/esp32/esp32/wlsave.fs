\ wlsave.fs - WROOM 4 MB: two reserved profile sectors, SSID and PMK only
\ No plaintext password. Commit last; keep the previous valid sector: the
\ two sectors are a pair of FLASH-PAIR (lib/esp32/flpair.fs), load it first.
PACKAGE WLSAVE ALSO WLAN ALSO WLAN.I ALSO WIFI ALSO WIFI.I ALSO CHIP ALSO BYTES ALSO FLASH-PAIR
: CHECK-MAP ( -- ) FLASH-SIZE $400000 <> ABORT" WLSAVE needs the 4-MB WROOM flash map" ;
CHECK-MAP
$3FE000 $3FF000 $314E4C57 PAIR PROFILE   \ lib/esp32/flpair.fs
68 BUFFER: DATA                     \ the record: SSID length (byte 0), SSID (4-35), PMK (36-67)
: WIPE ( -- ) DATA 68 0 FILL ;
: (SAVE) ( -- )
  WIFI-STATUS 6 <> ABORT" WLAN: save only after a successful join"
  DATA 68 0 FILL  STA-SLEN @ DATA C!  STA-SSID DATA 4 + STA-SLEN @ MOVE  STA-PMK DATA 36 + 32 MOVE
  DATA 68 PROFILE PAIR-WRITE  ." WLAN profile saved" CR ;
: (SAVED?) ( -- f ) DATA 68 PROFILE PAIR-READ ;
: MATCH? ( i -- f )
  WIFI-NET 2DROP DUP DATA C@ <> IF 2DROP FALSE EXIT THEN
  DATA 4 + SWAP BYTES= ;
: LOAD-SAVED ( -- )   \ the valid profile into DATA; -431: none; -430: a name of 0 or more than 32 bytes
  DATA 68 PROFILE PAIR-READ 0= IF -431 THROW THEN  DATA C@ 1 33 WITHIN 0= IF -430 THROW THEN ;
: (SAVED-NET) ( -- i )   \ the strongest WPA2 entry of the last scan with the saved name
  LOAD-SAVED  DATA 4 + DATA C@ WIFI-FIND 0= ABORT" WLAN: saved network not found in latest scan" ;
: (JOIN) ( i -- )
  WIFI-STATUS 0<> ABORT" WLAN: leave before loading a saved profile"
  LOAD-SAVED  DUP MATCH? 0= ABORT" WLAN: selected SSID differs from saved profile"
  DATA 36 + WIFI-JOIN ;
: (FORGET) ( -- )   PROFILE PAIR-ERASE  WIFI-LEAVE ;
: EXECUTE-CLEAN ( xt -- ) CATCH WIPE THROW ;
PUBLIC
: WLAN-SAVE ( -- ) ['] (SAVE) EXECUTE-CLEAN ;
: WLAN-SAVED? ( -- f ) ['] (SAVED?) EXECUTE-CLEAN ;
: WLAN-SAVED-NET ( -- i ) ['] (SAVED-NET) EXECUTE-CLEAN ;
: WLAN-JOIN ( i -- ) ['] (JOIN) EXECUTE-CLEAN ;
: WLAN-FORGET ( -- ) ['] (FORGET) EXECUTE-CLEAN ;
END-PACKAGE
