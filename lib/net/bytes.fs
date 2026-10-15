\ bytes.fs - package BYTES: exact byte comparison and 16-bit little-endian fields for packets
PACKAGE BYTES
PUBLIC
: BYTES= ( a b n -- f )   \ the n bytes at a are those at b
  OVER + SWAP ?DO  DUP C@ I C@ <> IF  DROP UNLOOP FALSE EXIT  THEN  1+  LOOP  DROP TRUE ;
: BYTES< ( a b n -- f )   \ the n bytes at a come before those at b: the first other byte decides
  OVER + SWAP ?DO  DUP C@ I C@ 2DUP <> IF  < NIP UNLOOP EXIT  THEN  2DROP 1+  LOOP  DROP FALSE ;
: LE16@ ( a -- u )   DUP C@ SWAP 1+ C@ 8 LSHIFT OR ;   \ 16 bits, the low byte first, as in 802.11
: LE16! ( u a -- )   2DUP C! SWAP 8 RSHIFT SWAP 1+ C! ;
END-PACKAGE
