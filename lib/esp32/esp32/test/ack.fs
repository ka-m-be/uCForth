\ ack.fs - at most four unicast probes, only to the AP selected by the owner
\
\ WIFI's last scan remains in RAM. AP-INDEX is supplied by the driver,
\ after the owner chose the entry locally. No SSID or BSSID is printed.
\ TEST-ACKCFG supplies bounded beacon-only RX housekeeping before TX.
\ The negative test temporarily assigns a different local receive MAC
\ to this board, so it cannot recognize the AP's ACK to the factory MAC.

ONLY FORTH  ALSO TEST-PROBE  ALSO TEST-ACKCFG  ALSO ESP32-RADIO  ALSO ESP32-RADIO.I  ALSO WIFI.I  DECIMAL
6 BUFFER: ACK-OWN
6 BUFFER: ACK-PEER
VARIABLE ACK-CHANNEL
VARIABLE ACK-TRIES
VARIABLE NEG-PMD
: SELECT-PEER ( -- )
  AP-INDEX 0 #NETS @ WITHIN 0= ABORT" ack: invalid selected AP index"
  AP-INDEX ENTRY DUP >CHAN C@ ACK-CHANNEL !  >BSSID ACK-PEER 6 MOVE
  ACK-PEER C@ 1 AND ABORT" ack: selected AP address is not unicast" ;
: OWN-ADDRESS ( -- )   $3FF5A004 @ $3FF5A008 @ ACK-OWN MAC-DECODE ;
: UNICAST ( -- a n )   ACK-OWN ACK-PEER PROBE-TO ;
: ONE-ACK ( -- )
  ACK-TRIES @ 4 < 0= ABORT" ack: test send limit"
  DRAIN-TEST-RX DROP  1 ACK-TRIES +!  UNICAST TRUE RADIO-SEND ;
: NEGATIVE ( -- )
  RA-ENABLE RA-MASK-HIGH BIC!
  RA-LOW @ 2 OR $10000 XOR RA-LOW !  RA-ENABLE RA-MASK-HIGH BIS!
  ['] ONE-ACK CATCH
  TX-BLOCKED? 0= IF  ACK-OWN RADIO-MAC!  THEN
  -2 <> ABORT" ack: missing ACK was not an error"
  TX-LAST-PMD @ NEG-PMD !
  TX-RESULT @ TX-COMPLETE <> ABORT" ack: negative test has no slot completion"
  NEG-PMD @ ACK-TIMEOUT? 0= ABORT" ack: expected an ACK timeout" ;
: FOUR ( -- )
  ONE-ACK  100 MS  ONE-ACK  100 MS  NEGATIVE  100 MS  ONE-ACK ;

SELECT-PEER  OWN-ADDRESS
RADIO-START  ACK-CHANNEL @ RADIO-CHANNEL!  ACK-OWN RADIO-MAC!  RADIO-BEACONS
0 ACK-TRIES !  0 NEG-PMD !
T{ ' FOUR CATCH -> 0 }T
T{ ACK-TRIES @ -> 4 }T
T{ NEG-PMD @ ACK-TIMEOUT? -> TRUE }T
T{ TX-RESULT @ TX-LAST-PMD @ $F000 AND TX-ACTIVE? TX-BLOCKED? -> TX-COMPLETE 0 FALSE FALSE }T
." ack: attempts " ACK-TRIES @ . ." channel " ACK-CHANNEL @ .
." negative PMD " NEG-PMD @ . ." final PMD " TX-LAST-PMD @ . CR
RADIO-STOP
S" ack" .RESULTS
