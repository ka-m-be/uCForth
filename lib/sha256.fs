\ sha256.fs - package SHA256: the hash function SHA-256 (FIPS 180-4)
\
\ SHA-256 makes a digest of 32 bytes from a text of any length. macOS
\ needs it for the signature of a program (lib/macos/macho.fs). The package
\ needs cells of 32 bits.
\
\   SHA-START               start a new digest
\   a u SHA-ADD             add u bytes; call it as often as necessary
\   SHA-END ( -- a )        the digest: 32 bytes at a
\   a u SHA-DIGEST ( -- a' ) the digest of one text
\
\ The steps: the text goes in blocks of 64 bytes into BLOCK. Each block
\ changes the 8 state words in STATE (64 rounds with the constants K).
\ At the end come the byte $80, zeros and the length in bits.

PACKAGE SHA256   \ lib/package.fs

PRIVATE
: NEED-32 ( -- )   CELL 4 <> ABORT" SHA256 needs cells of 32 bits" ;
NEED-32

CREATE K
  $428A2F98 , $71374491 , $B5C0FBCF , $E9B5DBA5 , $3956C25B , $59F111F1 , $923F82A4 , $AB1C5ED5 ,
  $D807AA98 , $12835B01 , $243185BE , $550C7DC3 , $72BE5D74 , $80DEB1FE , $9BDC06A7 , $C19BF174 ,
  $E49B69C1 , $EFBE4786 , $0FC19DC6 , $240CA1CC , $2DE92C6F , $4A7484AA , $5CB0A9DC , $76F988DA ,
  $983E5152 , $A831C66D , $B00327C8 , $BF597FC7 , $C6E00BF3 , $D5A79147 , $06CA6351 , $14292967 ,
  $27B70A85 , $2E1B2138 , $4D2C6DFC , $53380D13 , $650A7354 , $766A0ABB , $81C2C92E , $92722C85 ,
  $A2BFE8A1 , $A81A664B , $C24B8B70 , $C76C51A3 , $D192E819 , $D6990624 , $F40E3585 , $106AA070 ,
  $19A4C116 , $1E376C08 , $2748774C , $34B0BCB5 , $391C0CB3 , $4ED8AA4A , $5B9CCA4F , $682E6FF3 ,
  $748F82EE , $78A5636F , $84C87814 , $8CC70208 , $90BEFFFA , $A4506CEB , $BEF9A3F7 , $C67178F2 ,
CREATE START-STATE
  $6A09E667 , $BB67AE85 , $3C6EF372 , $A54FF53A , $510E527F , $9B05688C , $1F83D9AB , $5BE0CD19 ,

8 CELLS BUFFER: STATE              \ H0 to H7
8 CELLS BUFFER: WV                 \ the working words a to h of the rounds
64 CELLS BUFFER: W                 \ the message schedule
64 BUFFER: BLOCK   VARIABLE BLEN   \ the block that fills now, its bytes
VARIABLE TOTAL                     \ all bytes so far
32 BUFFER: DIGEST

: BE@ ( a -- x )   \ 4 bytes, the most significant first
  DUP C@ 24 LSHIFT  OVER 1+ C@ 16 LSHIFT OR  OVER 2 + C@ 8 LSHIFT OR  SWAP 3 + C@ OR ;
: BE! ( x a -- )
  OVER 24 RSHIFT OVER C!  OVER 16 RSHIFT OVER 1+ C!  OVER 8 RSHIFT OVER 2 + C!  3 + C! ;
: ROTR ( x n -- x' )   2DUP RSHIFT >R  32 SWAP - LSHIFT R> OR ;
: SIG0  ( x -- x' )   DUP 7 ROTR  OVER 18 ROTR XOR  SWAP 3 RSHIFT XOR ;
: SIG1  ( x -- x' )   DUP 17 ROTR  OVER 19 ROTR XOR  SWAP 10 RSHIFT XOR ;
: BSIG0 ( x -- x' )   DUP 2 ROTR  OVER 13 ROTR XOR  SWAP 22 ROTR XOR ;
: BSIG1 ( x -- x' )   DUP 6 ROTR  OVER 11 ROTR XOR  SWAP 25 ROTR XOR ;
: W@ ( i -- x )   CELLS W + @ ;
: V ( i -- a )   CELLS WV + ;        \ 0 is a, 7 is h

: SCHEDULE ( -- )
  16 0 DO  BLOCK I 4 * + BE@  I CELLS W + !  LOOP
  64 16 DO
    I 2 - W@ SIG1  I 7 - W@ +  I 15 - W@ SIG0 +  I 16 - W@ +  I CELLS W + !
  LOOP ;

: CH ( -- x )   4 V @ 5 V @ AND  4 V @ INVERT 6 V @ AND XOR ;
: MAJ ( -- x )
  0 V @ 1 V @ AND  0 V @ 2 V @ AND XOR  1 V @ 2 V @ AND XOR ;
: ROUND ( i -- )
  DUP CELLS K + @  SWAP W@ +  7 V @ +  4 V @ BSIG1 +  CH +   ( t1 )
  0 V @ BSIG0 MAJ +                                          ( t1 t2 )
  0 V  1 V  7 CELLS MOVE                                     \ h=g ... b=a
  OVER 4 V +!                                                \ e = d + t1
  + 0 V ! ;                                                  \ a = t1 + t2

: COMPRESS ( -- )   \ the block in BLOCK changes STATE
  SCHEDULE  STATE WV 8 CELLS MOVE
  64 0 DO  I ROUND  LOOP
  8 0 DO  I V @  I CELLS STATE + +!  LOOP  0 BLEN ! ;

: BYTE ( c -- )   \ one byte into the block
  BLOCK BLEN @ + C!  1 BLEN +!  BLEN @ 64 = IF COMPRESS THEN ;

PUBLIC
: SHA-START ( -- )   START-STATE STATE 8 CELLS MOVE  0 BLEN !  0 TOTAL ! ;
: SHA-ADD ( a u -- )
  DUP TOTAL +!  0 ?DO  DUP C@ BYTE 1+  LOOP  DROP ;
: SHA-END ( -- a )
  TOTAL @  $80 BYTE
  BEGIN BLEN @ 56 <> WHILE 0 BYTE REPEAT
  DUP 29 RSHIFT  BLOCK 56 + BE!  3 LSHIFT BLOCK 60 + BE!  64 BLEN !  COMPRESS
  8 0 DO  I CELLS STATE + @  I 4 * DIGEST + BE!  LOOP  DIGEST ;
: SHA-DIGEST ( a u -- a' )   SHA-START SHA-ADD SHA-END ;

END-PACKAGE
