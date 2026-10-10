\ wpakey.fs - public and synthetic vectors; load KDF1, WPAKEY, BYTES, tester
ONLY FORTH DEFINITIONS ALSO KDF1 ALSO WPAKEY ALSO BYTES
32 BUFFER: PMK
76 BUFFER: SEQ
64 BUFFER: PTK
CREATE EPMK $F4 C, $2C C, $6F C, $C5 C, $2D C, $F0 C, $EB C, $EF C,
  $9E C, $BB C, $4B C, $90 C, $B3 C, $8A C, $5F C, $90 C,
  $2E C, $83 C, $FE C, $1B C, $13 C, $5A C, $70 C, $E2 C,
  $3A C, $ED C, $76 C, $2E C, $97 C, $10 C, $A1 C, $2E C,
CREATE EPTK $E4 C, $49 C, $8E C, $73 C, $75 C, $80 C, $46 C, $49 C,
  $BE C, $E6 C, $3F C, $3E C, $89 C, $63 C, $95 C, $68 C,
  $E2 C, $E4 C, $3C C, $EA C, $12 C, $28 C, $29 C, $E4 C,
  $CA C, $F9 C, $A0 C, $D9 C, $03 C, $A7 C, $C1 C, $ED C,
  $4A C, $7A C, $2B C, $E5 C, $86 C, $46 C, $BD C, $0A C,
  $75 C, $30 C, $F5 C, $B3 C, $CC C, $CE C, $0E C, $66 C,
  $52 C, $E8 C, $8B C, $87 C, $47 C, $47 C, $8C C, $F5 C,
  $78 C, $52 C, $60 C, $C8 C, $C3 C, $E7 C, $DD C, $2A C,
: FILL-SEQ ( -- )  76 0 DO I SEQ I + C! LOOP SEQ PMK 32 MOVE ;
: PTK-FORWARD ( -- )  PMK SEQ SEQ 6 + SEQ 12 + SEQ 44 + PTK WPA-PTK ;
: PTK-REVERSE ( -- )  PMK SEQ 6 + SEQ SEQ 44 + SEQ 12 + PTK WPA-PTK ;
FILL-SEQ
T{ PTK-FORWARD PTK EPTK 64 BYTES= -> TRUE }T
T{ PTK-REVERSE PTK EPTK 64 BYTES= -> TRUE }T
: PMK-START ( -- )  S" password" S" IEEE" PMK WPA-PMK-START ;
: PMK-TEST ( -- )
  PMK-START BEGIN 16 WPA-PMK-STEP UNTIL T{ PMK EPMK 32 BYTES= -> TRUE }T ;
: BAD-PASS ( -- )  S" short" S" IEEE" PMK WPA-PMK-START ;
: BAD-SSID ( -- )  S" password" S" " PMK WPA-PMK-START ;
T{ ' BAD-PASS CATCH 0<> -> TRUE }T
T{ ' BAD-SSID CATCH 0<> -> TRUE }T
: AUTO-PMK ( -- )  [ [DEFINED] KDF-SHORT ] LITERAL 0= IF PMK-TEST THEN ;
AUTO-PMK
S" wpakey" .RESULTS
