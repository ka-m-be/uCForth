\ kdf1.fs - RFC 2202/6070; load SHA1, KDF1, BYTES and tester first
ONLY FORTH DEFINITIONS ALSO SHA1 ALSO KDF1 ALSO BYTES
CREATE H1 $B6 C, $17 C, $31 C, $86 C, $55 C, $05 C, $72 C, $64 C, $E2 C, $8B C,
  $C0 C, $B6 C, $FB C, $37 C, $8C C, $8E C, $F1 C, $46 C, $BE C, $00 C,
CREATE H2 $EF C, $FC C, $DF C, $6A C, $E5 C, $EB C, $2F C, $A2 C, $D2 C, $74 C,
  $16 C, $D5 C, $F1 C, $84 C, $DF C, $9C C, $25 C, $9A C, $7C C, $79 C,
CREATE H6 $AA C, $4A C, $E5 C, $E1 C, $52 C, $72 C, $D0 C, $0E C, $95 C, $70 C,
  $56 C, $37 C, $CE C, $8A C, $3B C, $55 C, $ED C, $40 C, $21 C, $12 C,
80 BUFFER: KTEST
64 BUFFER: KOUT
CREATE P1 $0C C, $60 C, $C8 C, $0F C, $96 C, $1F C, $0E C, $71 C, $F3 C, $A9 C,
  $B5 C, $24 C, $AF C, $60 C, $12 C, $06 C, $2F C, $E0 C, $37 C, $A6 C,
CREATE P2 $EA C, $6C C, $01 C, $4D C, $C7 C, $2D C, $6F C, $8C C, $CD C, $1E C,
  $D9 C, $2A C, $CE C, $1D C, $41 C, $F0 C, $D8 C, $DE C, $89 C, $57 C,
CREATE P4096 $4B C, $00 C, $79 C, $01 C, $B7 C, $65 C, $48 C, $9A C, $BE C, $AD C,
  $49 C, $D9 C, $26 C, $F7 C, $21 C, $D0 C, $65 C, $A4 C, $29 C, $C1 C,
CREATE P25 $3D C, $2E C, $EC C, $4F C, $E4 C, $1C C, $84 C, $9B C, $80 C, $C8 C,
  $D8 C, $36 C, $62 C, $C0 C, $E4 C, $4A C, $8B C, $29 C, $1A C, $96 C,
  $4C C, $F2 C, $F0 C, $70 C, $38 C,
CREATE PNUL $56 C, $FA C, $6A C, $A7 C, $55 C, $48 C, $09 C, $9D C,
  $CC C, $37 C, $D7 C, $F0 C, $34 C, $25 C, $E0 C, $C3 C,
CREATE PASS0 112 C, 97 C, 115 C, 115 C, 0 C, 119 C, 111 C, 114 C, 100 C,
CREATE SALT0 115 C, 97 C, 0 C, 108 C, 116 C,
: RFC1 ( count -- )  >R S" password" S" salt" R> KOUT 20 PBKDF2 ;
: RFC-H2 ( -- a )  S" Jefe" S" what do ya want for nothing?" HMAC1-DIGEST ;
KTEST 20 $0B FILL
T{ KTEST 20 S" Hi There" HMAC1-DIGEST H1 20 BYTES= -> TRUE }T
T{ RFC-H2 H2 20 BYTES= -> TRUE }T
KTEST 80 $AA FILL
T{ KTEST 80 S" Test Using Larger Than Block-Size Key - Hash Key First" HMAC1-DIGEST H6 20 BYTES= -> TRUE }T
: HSTREAM ( -- f )
  S" Jefe" HMAC1-START S" what do ya " HMAC1-ADD
  S" want for nothing?" HMAC1-ADD HMAC1-END H2 20 BYTES= ;
T{ HSTREAM -> TRUE }T
T{ 1 RFC1 KOUT P1 20 BYTES= -> TRUE }T
T{ 2 RFC1 KOUT P2 20 BYTES= -> TRUE }T
\ Long cases are named words: the board driver advances them in short steps.
: RFC4096-START ( -- )  S" password" S" salt" 4096 KOUT 20 PBKDF2-START ;
: RFC25-START ( -- )
  S" passwordPASSWORDpassword" S" saltSALTsaltSALTsaltSALTsaltSALTsalt" 4096 KOUT 25 PBKDF2-START ;
: RFCNUL-START ( -- )  PASS0 9 SALT0 5 4096 KOUT 16 PBKDF2-START ;
: COMPLETE ( -- )  BEGIN 16 PBKDF2-STEP UNTIL ;
: LONG-TESTS ( -- )
  RFC4096-START COMPLETE T{ KOUT P4096 20 BYTES= -> TRUE }T
  RFC25-START COMPLETE T{ KOUT P25 25 BYTES= -> TRUE }T
  RFCNUL-START COMPLETE T{ KOUT PNUL 16 BYTES= -> TRUE }T ;
: INTERLEAVE ( -- f )
  S" password" S" salt" 2 KOUT 20 PBKDF2-START 1 PBKDF2-STEP DROP
  S" Jefe" S" what do ya want for nothing?" HMAC1-DIGEST DROP
  1 PBKDF2-STEP KOUT P2 20 BYTES= AND ;
T{ INTERLEAVE -> TRUE }T
: BAD-COUNT ( -- )  S" password" S" salt" 0 KOUT 20 PBKDF2-START ;
T{ ' BAD-COUNT CATCH 0<> -> TRUE }T
: BAD-BUDGET ( -- )  0 PBKDF2-STEP DROP ;
T{ ' BAD-BUDGET CATCH 0<> -> TRUE }T
: ZERO-OUT? ( -- f )  TRUE 20 0 DO KOUT I + C@ 0= AND LOOP ;
: CANCEL-START ( -- )  S" password" S" salt" 3 KOUT 20 PBKDF2-START ;
CANCEL-START
1 PBKDF2-STEP DROP PBKDF2-CANCEL
T{ ZERO-OUT? -> TRUE }T
: PARTIAL ( -- f )
  S" password" S" salt" 1 KOUT 25 PBKDF2-START
  1 PBKDF2-STEP 0= KOUT P1 20 BYTES= AND ;
T{ PARTIAL -> TRUE }T
PBKDF2-CANCEL
T{ ZERO-OUT? KOUT 24 + C@ -> TRUE 0 }T
: BOUND ( -- f )
  S" password" S" salt" 9 KOUT 20 PBKDF2-START
  1000 PBKDF2-STEP 0= 1 PBKDF2-STEP AND ;
T{ BOUND -> TRUE }T
: BAD-OUT ( -- )  S" password" S" salt" 1 KOUT 0 PBKDF2-START ;
T{ ' BAD-OUT CATCH 0<> -> TRUE }T
PBKDF2-CANCEL
\ A board driver defines KDF-SHORT and advances the long cases separately.
: AUTO-LONG ( -- )  [ [DEFINED] KDF-SHORT ] LITERAL 0= IF LONG-TESTS THEN ;
AUTO-LONG
HMAC1-CLEAR
S" kdf1" .RESULTS
