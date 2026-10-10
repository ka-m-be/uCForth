\ diag.fs - four controlled probes, only to the AP selected by the owner
\
\ Case 0: fictional name, existing TX values.
\ Case 1: selected AP's name, existing TX values.
\ Case 2: case 1, only CONFIG bits 0..11 changed from 10 to 30.
\ Case 3: case 1, only PLCP1 bit 28 cleared.
\ The name and BSSID stay in RAM and are not printed.
\ All cases use backoff 7; normal frame sequence numbers still advance.
\ PHY, BYTES, RADIO, WIFI, tester, TEST-PROBE and TEST-ACKCFG are loaded.

ONLY FORTH  ALSO TEST-PROBE  ALSO TEST-ACKCFG  ALSO ESP32-RADIO  ALSO ESP32-RADIO.I  ALSO WIFI.I  DECIMAL
6 BUFFER: DIAG-OWN
6 BUFFER: DIAG-PEER
32 BUFFER: DIAG-NAME
VARIABLE DIAG-NLEN
VARIABLE DIAG-CHANNEL
VARIABLE DIAG-CASE
4 5 * CELLS BUFFER: DIAG-ROWS   \ ior, result, PMD, CONFIG, PLCP1
: ROW ( -- a )   DIAG-CASE @ 5 * CELLS DIAG-ROWS + ;
: SELECT-DIAG ( -- )
  AP-INDEX 0 #NETS @ WITHIN 0= ABORT" diag: invalid selected AP index"
  AP-INDEX ENTRY
  DUP >CHAN C@ DIAG-CHANNEL !  DUP >BSSID DIAG-PEER 6 MOVE
  DUP >NLEN C@ DUP 1 33 WITHIN 0= ABORT" diag: invalid selected name length"
  DUP DIAG-NLEN !  SWAP >SSID DIAG-NAME ROT MOVE
  DIAG-PEER C@ 1 AND ABORT" diag: selected address is not unicast" ;
: SETUP-DIAG ( -- )
  RADIO-START  DIAG-CHANNEL @ RADIO-CHANNEL!
  DIAG-OWN RADIO-MAC!  RADIO-BEACONS ;
: DIAG-PACKET ( -- a n )
  DIAG-CASE @ 0= IF  DIAG-OWN DIAG-PEER PROBE-TO  ELSE
    DIAG-OWN DIAG-NAME DIAG-NLEN @ PROBE-NAMED  DIAG-PEER PROBE-TARGET
  THEN ;
: ATTEMPT ( -- )
  DRAIN-TEST-RX DROP
  DIAG-PACKET TRUE 7 TX-PREPARE
  DIAG-CASE @ DIAG-CONFIG
  TX-CONFIG @ ROW 3 CELLS + !  TX-PLCP1 @ ROW 4 CELLS + !
  TX-EXECUTE ;
: ONE-CASE ( i -- )
  DIAG-CASE !
  ['] ATTEMPT CATCH ROW !
  TX-RESULT @ ROW CELL+ !  TX-LAST-PMD @ ROW 2 CELLS + !
  TX-BLOCKED? IF  RADIO-STOP SETUP-DIAG  THEN ;
: SHOW-CASE ( i -- )
  DIAG-CASE !  ." diag: case " DIAG-CASE @ . ." ior " ROW @ .
  ." result " ROW CELL+ @ .  BASE @ >R HEX
  ." PMD $" ROW 2 CELLS + @ . ." CONFIG $" ROW 3 CELLS + @ .
  ." PLCP1 $" ROW 4 CELLS + @ . R> BASE ! CR ;
: DIAGNOSE ( -- )   4 0 DO  I ONE-CASE  100 MS  LOOP ;
: SHOW-DIAG ( -- )   4 0 DO  I SHOW-CASE  LOOP ;

SELECT-DIAG
$3FF5A004 @ $3FF5A008 @ DIAG-OWN MAC-DECODE
DIAG-ROWS 4 5 * CELLS 0 FILL
SETUP-DIAG
' DIAGNOSE CATCH ." diag: runner ior " . CR
RADIO-STOP
SHOW-DIAG
