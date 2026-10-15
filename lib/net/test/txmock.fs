\ txmock.fs - the sending of the radio for WIFI-LINK, as a mock in TEST-STA (lib/net/test/stamock.fs)
\ The frames go to AIR-TX; nothing goes to the air (lib/net/test/wlink.fs, lib/net/test/heads.fs).
ONLY FORTH DEFINITIONS ALSO BYTES ALSO TEST-STA
TEST-STA DEFINITIONS
1600 BUFFER: AIR-TX
VARIABLE AIR-N VARIABLE AIR-BUSY VARIABLE AIR-RESULT
: RADIO-SOFT-CCMP ( -- ) ;
: RADIO-TX-START ( a n ack? -- )
  0= ABORT" test: uplink must request ACK" DUP AIR-N ! AIR-TX SWAP MOVE AIR-BUSY ON! ;
: RADIO-TX-POLL ( -- n ) AIR-BUSY OFF! AIR-RESULT @ ;
: RADIO-TX-BUSY? ( -- f ) AIR-BUSY @ ;
: RADIO-TX-BLOCKED? ( -- f ) FALSE ;
: RADIO-TX-CANCEL ( -- ) AIR-BUSY OFF! ;
: RADIO-TX-END ( -- ) AIR-BUSY OFF! ;
: RADIO-TRACK ( -- ) ;
ONLY FORTH DEFINITIONS ALSO FILES
