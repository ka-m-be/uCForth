\ aes128.fs - AES-128 blocks (FIPS 197) and key unwrap (RFC 3394)
PACKAGE AES128
CREATE SBOX
  $63 C, $7C C, $77 C, $7B C, $F2 C, $6B C, $6F C, $C5 C, $30 C, $01 C, $67 C, $2B C, $FE C, $D7 C, $AB C, $76 C,
  $CA C, $82 C, $C9 C, $7D C, $FA C, $59 C, $47 C, $F0 C, $AD C, $D4 C, $A2 C, $AF C, $9C C, $A4 C, $72 C, $C0 C,
  $B7 C, $FD C, $93 C, $26 C, $36 C, $3F C, $F7 C, $CC C, $34 C, $A5 C, $E5 C, $F1 C, $71 C, $D8 C, $31 C, $15 C,
  $04 C, $C7 C, $23 C, $C3 C, $18 C, $96 C, $05 C, $9A C, $07 C, $12 C, $80 C, $E2 C, $EB C, $27 C, $B2 C, $75 C,
  $09 C, $83 C, $2C C, $1A C, $1B C, $6E C, $5A C, $A0 C, $52 C, $3B C, $D6 C, $B3 C, $29 C, $E3 C, $2F C, $84 C,
  $53 C, $D1 C, $00 C, $ED C, $20 C, $FC C, $B1 C, $5B C, $6A C, $CB C, $BE C, $39 C, $4A C, $4C C, $58 C, $CF C,
  $D0 C, $EF C, $AA C, $FB C, $43 C, $4D C, $33 C, $85 C, $45 C, $F9 C, $02 C, $7F C, $50 C, $3C C, $9F C, $A8 C,
  $51 C, $A3 C, $40 C, $8F C, $92 C, $9D C, $38 C, $F5 C, $BC C, $B6 C, $DA C, $21 C, $10 C, $FF C, $F3 C, $D2 C,
  $CD C, $0C C, $13 C, $EC C, $5F C, $97 C, $44 C, $17 C, $C4 C, $A7 C, $7E C, $3D C, $64 C, $5D C, $19 C, $73 C,
  $60 C, $81 C, $4F C, $DC C, $22 C, $2A C, $90 C, $88 C, $46 C, $EE C, $B8 C, $14 C, $DE C, $5E C, $0B C, $DB C,
  $E0 C, $32 C, $3A C, $0A C, $49 C, $06 C, $24 C, $5C C, $C2 C, $D3 C, $AC C, $62 C, $91 C, $95 C, $E4 C, $79 C,
  $E7 C, $C8 C, $37 C, $6D C, $8D C, $D5 C, $4E C, $A9 C, $6C C, $56 C, $F4 C, $EA C, $65 C, $7A C, $AE C, $08 C,
  $BA C, $78 C, $25 C, $2E C, $1C C, $A6 C, $B4 C, $C6 C, $E8 C, $DD C, $74 C, $1F C, $4B C, $BD C, $8B C, $8A C,
  $70 C, $3E C, $B5 C, $66 C, $48 C, $03 C, $F6 C, $0E C, $61 C, $35 C, $57 C, $B9 C, $86 C, $C1 C, $1D C, $9E C,
  $E1 C, $F8 C, $98 C, $11 C, $69 C, $D9 C, $8E C, $94 C, $9B C, $1E C, $87 C, $E9 C, $CE C, $55 C, $28 C, $DF C,
  $8C C, $A1 C, $89 C, $0D C, $BF C, $E6 C, $42 C, $68 C, $41 C, $99 C, $2D C, $0F C, $B0 C, $54 C, $BB C, $16 C,
256 BUFFER: INV
176 BUFFER: KEYS
16 BUFFER: ST
16 BUFFER: TMP
VARIABLE READY  VARIABLE RC
: MAKE-INV ( -- )  256 0 DO I SBOX I + C@ INV + C! LOOP ; MAKE-INV
: XT ( x -- x' )  DUP 2* SWAP $80 AND IF $1B XOR THEN $FF AND ;
: GMUL ( x y -- z )
  0 >R BEGIN DUP WHILE
    DUP 1 AND IF OVER R> XOR >R THEN 1 RSHIFT SWAP XT SWAP
  REPEAT 2DROP R> ;
: SUB ( table -- )
  16 0 DO DUP ST I + C@ + C@ ST I + C! LOOP DROP ;
: SHIFT ( direction -- )
  ST TMP 16 MOVE
  16 0 DO
    DUP I 3 AND * I 2 RSHIFT + 3 AND 4 * I 3 AND + TMP + C@ ST I + C!
  LOOP DROP ;
: ADDKEY ( round -- )
  16 * KEYS + 16 0 DO DUP I + C@ ST I + DUP C@ ROT XOR SWAP C! LOOP DROP ;
: MIX ( -- )
  4 0 DO
    ST I 4 * + DUP TMP 4 MOVE
    TMP C@ TMP 1+ C@ XOR TMP 2 + C@ XOR TMP 3 + C@ XOR
    4 0 DO
      TMP I + C@ OVER XOR TMP I + C@ TMP I 1+ 3 AND + C@ XOR XT XOR
      >R OVER R> SWAP I + C!
    LOOP 2DROP
  LOOP ;
: IMIX ( -- )
  4 0 DO
    ST I 4 * + DUP TMP 4 MOVE
    4 0 DO
      TMP I + C@ 14 GMUL TMP I 1+ 3 AND + C@ 11 GMUL XOR
      TMP I 2 + 3 AND + C@ 13 GMUL XOR TMP I 3 + 3 AND + C@ 9 GMUL XOR
      OVER I + C!
    LOOP DROP
  LOOP ;
: EXPAND ( -- )
  1 RC !
  44 4 DO
    KEYS I 1- 4 * + TMP 4 MOVE
    I 3 AND 0= IF
      TMP C@ >R
      TMP 1+ C@ SBOX + C@ RC @ XOR TMP C!
      TMP 2 + C@ SBOX + C@ TMP 1+ C!
      TMP 3 + C@ SBOX + C@ TMP 2 + C!
      R> SBOX + C@ TMP 3 + C! RC @ XT RC !
    THEN
    4 0 DO
      KEYS J 4 - 4 * + I + C@ TMP I + C@ XOR KEYS J 4 * + I + C!
    LOOP
  LOOP ;
: ?KEY ( -- )  READY @ 0= ABORT" AES128 key not set" ;
CREATE TE 256 CELLS ALLOT
16 BUFFER: FV
16 BUFFER: FN
VARIABLE FSTEP
: ROL32 ( x n -- x' ) 2DUP LSHIFT >R 32 SWAP - RSHIFT R> OR ;
: MAKE-TE ( -- )
  CELL 4 = IF
    256 0 DO
      SBOX I + C@ DUP XT OVER XOR 24 LSHIFT OVER 16 LSHIFT OR
      OVER 8 LSHIFT OR SWAP XT OR TE I CELLS + !
    LOOP
  THEN ;
MAKE-TE
: F@ ( column -- x ) 3 AND CELLS FV + @ ;
: T@ ( x -- x' ) $FF AND CELLS TE + @ ;
: FKEY ( column round -- x ) 4 * + CELLS KEYS + @ ;
: FROUND ( round -- )
  4 0 DO
    I F@ T@ I 1+ F@ 8 RSHIFT T@ 8 ROL32 XOR
    I 2 + F@ 16 RSHIFT T@ 16 ROL32 XOR I 3 + F@ 24 RSHIFT T@ 24 ROL32 XOR
    OVER I SWAP FKEY XOR FN I CELLS + !
  LOOP DROP FN FV 16 MOVE ;
: FBEGIN ( in -- )
  ?KEY FV 16 MOVE
  4 0 DO FV I CELLS + DUP @ I 0 FKEY XOR SWAP ! LOOP
  1 FSTEP ! ;
: FLAST ( out -- )
  >R
  4 0 DO
    I F@ $FF AND SBOX + C@
    I 1+ F@ 8 RSHIFT $FF AND SBOX + C@ 8 LSHIFT OR
    I 2 + F@ 16 RSHIFT $FF AND SBOX + C@ 16 LSHIFT OR
    I 3 + F@ 24 RSHIFT SBOX + C@ 24 LSHIFT OR
    I 10 FKEY XOR FN I CELLS + !
  LOOP FN R> 16 MOVE ;
: FAST-ENCRYPT ( in out -- )
  >R FBEGIN 10 1 DO I FROUND LOOP R> FLAST 11 FSTEP ! ;
: BYTE-ENCRYPT ( in out -- )
  ?KEY >R ST 16 MOVE 0 ADDKEY
  10 1 DO SBOX SUB 1 SHIFT MIX I ADDKEY LOOP
  SBOX SUB 1 SHIFT 10 ADDKEY ST R> 16 MOVE ;

PUBLIC
: AES128-KEY ( a -- )  KEYS 16 MOVE EXPAND TRUE READY ! ;
: AES128-ENCRYPT ( in out -- )
  CELL 4 = IF FAST-ENCRYPT ELSE BYTE-ENCRYPT THEN ;
: AES128-DECRYPT ( in out -- )
  ?KEY >R ST 16 MOVE 10 ADDKEY
  10 1 DO -1 SHIFT INV SUB 10 I - ADDKEY IMIX LOOP
  -1 SHIFT INV SUB 0 ADDKEY ST R> 16 MOVE ;
: AES128-CLEAR ( -- )
  KEYS 176 0 FILL ST 16 0 FILL TMP 16 0 FILL FV 16 0 FILL FN 16 0 FILL
  0 READY ! 0 RC ! 0 FSTEP ! ;
252 CONSTANT AES128-CONTEXT
: AES128-SAVE ( a -- )
  KEYS OVER 176 MOVE ST OVER 176 + 16 MOVE TMP OVER 192 + 16 MOVE
  RC @ OVER 208 + ! READY @ OVER 212 + !
  FV OVER 216 + 16 MOVE FN OVER 232 + 16 MOVE FSTEP @ SWAP 248 + ! ;
: AES128-RESTORE ( a -- )
  DUP KEYS 176 MOVE DUP 176 + ST 16 MOVE DUP 192 + TMP 16 MOVE
  DUP 208 + @ RC ! DUP 212 + @ READY !
  DUP 216 + FV 16 MOVE DUP 232 + FN 16 MOVE 248 + @ FSTEP ! ;
: AES128-BEGIN ( in -- )
  CELL 4 <> ABORT" incremental AES needs 32-bit cells" FBEGIN ;
: AES128-STEP ( out rounds -- done )
  DUP 1 < ABORT" AES round budget must be positive"
  FSTEP @ 0= ABORT" AES encryption not started"
  4 MIN 0 DO
    FSTEP @ 10 < IF FSTEP @ FROUND 1 FSTEP +!
    ELSE FSTEP @ 10 = IF DUP FLAST 11 FSTEP ! THEN THEN
  LOOP DROP FSTEP @ 11 = ;

PRIVATE
16 BUFFER: WRAP-B
8 BUFFER: WRAP-A
VARIABLE WRAP-OUT  VARIABLE WRAP-N
: XOR-T ( t -- )
  8 0 DO DUP $FF AND WRAP-B 7 I - + DUP C@ ROT XOR SWAP C! 8 RSHIFT LOOP DROP ;

PUBLIC
: AES128-UNWRAP ( in n out -- n' )
  ?KEY >R DUP 24 2057 WITHIN 0= OVER 7 AND 0<> OR ABORT" bad AES unwrap length"
  8 - DUP 8 / WRAP-N ! R> WRAP-OUT !
  OVER WRAP-A 8 MOVE >R 8 + WRAP-OUT @ R@ MOVE R> DROP
  6 0 DO
    WRAP-N @ 0 DO
      WRAP-A WRAP-B 8 MOVE
      WRAP-N @ I - DUP 1- 8 * WRAP-OUT @ + WRAP-B 8 + 8 MOVE
      WRAP-N @ 5 J - * + XOR-T WRAP-B WRAP-B AES128-DECRYPT
      WRAP-B WRAP-A 8 MOVE
      WRAP-B 8 + WRAP-OUT @ WRAP-N @ I - 1- 8 * + 8 MOVE
    LOOP
  LOOP
  0 8 0 DO WRAP-A I + C@ $A6 XOR OR LOOP
  WRAP-A 8 0 FILL WRAP-B 16 0 FILL
  IF WRAP-OUT @ WRAP-N @ 8 * 0 FILL -410 THROW THEN WRAP-N @ 8 * ;
END-PACKAGE
