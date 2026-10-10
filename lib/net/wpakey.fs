\ wpakey.fs - WPA2-Personal/CCMP key derivation, no radio or credentials
\ Load SHA1 and KDF1 first. MACs are 6 bytes, nonces and PMK 32 bytes.
PACKAGE WPAKEY  ALSO KDF1 ALSO BYTES
76 BUFFER: INPUT
VARIABLE SORT-OUT  VARIABLE SORT-N
: PAIR ( a b n out -- )
  SORT-OUT ! SORT-N !
  2DUP SORT-N @ BYTES< 0= IF SWAP THEN
  OVER SORT-OUT @ SORT-N @ MOVE
  SORT-OUT @ SORT-N @ + SORT-N @ MOVE DROP ;

PUBLIC
: WPA-PMK-START ( pass pn ssid sn out -- )
  >R DUP 1 33 WITHIN 0= ABORT" SSID must have 1 to 32 bytes"
  >R >R DUP 8 64 WITHIN 0= ABORT" passphrase must have 8 to 63 bytes"
  R> R> 4096 R> 32 PBKDF2-START ;
: WPA-PMK-STEP ( budget -- done )  PBKDF2-STEP ;
: WPA-PMK-CANCEL ( -- )  PBKDF2-CANCEL ;
: WPA-PTK ( pmk mac1 mac2 nonce1 nonce2 out -- )
  >R 32 INPUT 12 + PAIR 6 INPUT PAIR
  32 S" Pairwise key expansion" INPUT 76 R> 64 SHA1-PRF INPUT 76 0 FILL ;
END-PACKAGE
