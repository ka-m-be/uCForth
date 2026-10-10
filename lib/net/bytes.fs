\ bytes.fs - exact byte comparison for packet fields, independent of CASE-SENSE
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
END-PACKAGE
