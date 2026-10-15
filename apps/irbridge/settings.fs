\ settings.fs - package BRIDGE-SETTINGS: the broker of the IR bridge in the flash
\
\ The bridge keeps the address and the port of its broker in the flash,
\ so that it can start without a terminal. The record is a pair of
\ FLASH-PAIR (lib/esp32/flpair.fs) in the two sectors before the profile
\ of the WLAN: $3F_C000 and $3F_D000 (targets/esp32/README.md, "Memory").
\ A power loss while it writes keeps the broker of before.
\
\   BRIDGE-SAVE ( -- )     the broker of MQTT-BROKER into the flash
\   BRIDGE-LOAD ( -- f )   the broker from the flash to MQTT-BROKER;
\                          false: none saved
\   BRIDGE-FORGET ( -- )   no broker in the flash
\
\ The record: the port (2 bytes, the low one first), the length of the
\ address (1 byte), the address as text (at most 64 bytes, as MQTT keeps
\ it). Load it after FLASH-PAIR, BYTES and MQTT (apps/irbridge/app.fs does
\ it). Test on the host: apps/irbridge/test/settings.fs.

PACKAGE BRIDGE-SETTINGS  ALSO FLASH-PAIR  ALSO BYTES  ALSO MQTT   \ lib/package.fs

$3FC000 $3FD000 $31425249 PAIR BROKER-PAIR   \ the magic: IRB1
68 BUFFER: RECORD

PUBLIC
: BRIDGE-SAVE ( -- )
  MQTT-BROKER@  OVER 0= ABORT" BRIDGE: no broker (MQTT-BROKER)"   ( a u port )
  RECORD 68 0 FILL  RECORD LE16!  DUP RECORD 2 + C!  RECORD 3 + SWAP MOVE
  RECORD 68 BROKER-PAIR PAIR-WRITE  ." bridge: broker saved" CR ;
: BRIDGE-LOAD ( -- f )
  RECORD 68 BROKER-PAIR PAIR-READ 0= IF  FALSE EXIT  THEN
  RECORD 3 +  RECORD 2 + C@  RECORD LE16@  MQTT-BROKER  TRUE ;
: BRIDGE-FORGET ( -- )   BROKER-PAIR PAIR-ERASE ;

END-PACKAGE
