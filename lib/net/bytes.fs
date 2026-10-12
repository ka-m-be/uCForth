\ bytes.fs - package BYTES: exact byte comparison and 16-bit little-endian fields for packets
PACKAGE BYTES
PUBLIC
: BYTES= ( a b n -- f )
  0 ?DO
    OVER I + C@  OVER I + C@ <> IF  2DROP FALSE UNLOOP EXIT  THEN
  LOOP  2DROP TRUE ;
: BYTES< ( a b n -- f )
  0 ?DO
    OVER I + C@ OVER I + C@ 2DUP <> IF < >R 2DROP R> UNLOOP EXIT THEN 2DROP
  LOOP 2DROP FALSE ;
: LE16@ ( a -- u )   DUP C@ SWAP 1+ C@ 8 LSHIFT OR ;   \ 16 bits, the low byte first, as in 802.11
: LE16! ( u a -- )   2DUP C! SWAP 8 RSHIFT SWAP 1+ C! ;
END-PACKAGE
