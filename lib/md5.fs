\ md5.fs - package MD5: the message digest MD5 after RFC 1321
\
\ The ROM bootloader of the ESP32 checks a flash image with MD5
\ (lib/esp32/esprom.fs). Needs cells of 32 bits.
\
\   MD5-INIT                start a digest
\   a u MD5-UPDATE          add u bytes; as often as necessary
\   MD5-FINAL ( -- a )      the 16 bytes of the digest
\   MD5-HEX ( -- a u )      after MD5-FINAL: 32 lower-case hex digits

PACKAGE MD5   \ lib/package.fs
: ?32BIT ( -- )  CELL 4 < ABORT" MD5 needs 32-bit cells" ;  ?32BIT

VARIABLE H0  VARIABLE H1  VARIABLE H2  VARIABLE H3    \ the state
VARIABLE RA  VARIABLE RB  VARIABLE RC  VARIABLE RD    \ the registers of one block
VARIABLE TOTAL                      \ bytes so far
VARIABLE FILLED                     \ bytes in CHUNK
64 BUFFER: CHUNK
16 BUFFER: DIGEST
8 BUFFER: LENGTH

CREATE K-TAB   \ K[i] = floor(abs(sin(i+1)) * 2^32)
  $D76AA478 , $E8C7B756 , $242070DB , $C1BDCEEE , $F57C0FAF , $4787C62A , $A8304613 , $FD469501 ,
  $698098D8 , $8B44F7AF , $FFFF5BB1 , $895CD7BE , $6B901122 , $FD987193 , $A679438E , $49B40821 ,
  $F61E2562 , $C040B340 , $265E5A51 , $E9B6C7AA , $D62F105D , $02441453 , $D8A1E681 , $E7D3FBC8 ,
  $21E1CDE6 , $C33707D6 , $F4D50D87 , $455A14ED , $A9E3E905 , $FCEFA3F8 , $676F02D9 , $8D2A4C8A ,
  $FFFA3942 , $8771F681 , $6D9D6122 , $FDE5380C , $A4BEEA44 , $4BDECFA9 , $F6BB4B60 , $BEBFBC70 ,
  $289B7EC6 , $EAA127FA , $D4EF3085 , $04881D05 , $D9D4D039 , $E6DB99E5 , $1FA27CF8 , $C4AC5665 ,
  $F4292244 , $432AFF97 , $AB9423A7 , $FC93A039 , $655B59C3 , $8F0CCC92 , $FFEFF47D , $85845DD1 ,
  $6FA87E4F , $FE2CE6E0 , $A3014314 , $4E0811A1 , $F7537E82 , $BD3AF235 , $2AD7D2BB , $EB86D391 ,
CREATE S-TAB   \ the rotation of each step: four values per round
  7 C, 12 C, 17 C, 22 C,   5 C,  9 C, 14 C, 20 C,   4 C, 11 C, 16 C, 23 C,   6 C, 10 C, 15 C, 21 C,
CREATE ONE-BIT  $80 C,
CREATE ZERO     0 C,

: ROL ( x n -- x' )  2DUP LSHIFT >R  32 SWAP - RSHIFT  R> OR ;
: FUN-F ( b c d -- x )  >R OVER AND SWAP INVERT R> AND OR ;     \ (b and c) or (not b and d)
: FUN-G ( b c d -- x )  TUCK INVERT AND >R AND R> OR ;          \ (b and d) or (c and not d)
: FUN-H ( b c d -- x )  XOR XOR ;                              \ b xor c xor d
: FUN-I ( b c d -- x )  INVERT ROT OR XOR ;                    \ c xor (b or not d)
: BCD@ ( -- b c d )   RB @ RC @ RD @ ;

: STEP ( f g i -- )   \ step i with the round value f and the message word g
  >R  CELLS CHUNK + @  +  R@ CELLS K-TAB + @ +  RA @ +
  R@ 4 RSHIFT 2 LSHIFT  R> 3 AND +  S-TAB + C@  ROL  RB @ +
  RD @ RA !  RC @ RD !  RB @ RC !  RB ! ;

: TRANSFORM ( -- )   \ add the 64 bytes in CHUNK to the state
  H0 @ RA !  H1 @ RB !  H2 @ RC !  H3 @ RD !
  16 0 DO  BCD@ FUN-F  I                    I      STEP  LOOP
  16 0 DO  BCD@ FUN-G  I 5 * 1+ 15 AND      I 16 + STEP  LOOP
  16 0 DO  BCD@ FUN-H  I 3 * 5 + 15 AND     I 32 + STEP  LOOP
  16 0 DO  BCD@ FUN-I  I 7 * 15 AND         I 48 + STEP  LOOP
  RA @ H0 +!  RB @ H1 +!  RC @ H2 +!  RD @ H3 +! ;

PUBLIC
: MD5-INIT ( -- )
  $67452301 H0 !  $EFCDAB89 H1 !  $98BADCFE H2 !  $10325476 H3 !  0 TOTAL !  0 FILLED ! ;
: MD5-UPDATE ( a u -- )
  DUP TOTAL +!
  BEGIN DUP WHILE
    64 FILLED @ -  OVER MIN
    >R  OVER CHUNK FILLED @ + R@ MOVE
    R@ FILLED +!  R@ - SWAP R> + SWAP
    FILLED @ 64 = IF TRANSFORM 0 FILLED ! THEN
  REPEAT 2DROP ;
: MD5-FINAL ( -- a )
  TOTAL @  ONE-BIT 1 MD5-UPDATE
  BEGIN FILLED @ 56 <> WHILE ZERO 1 MD5-UPDATE REPEAT
  DUP 3 LSHIFT LENGTH !  29 RSHIFT LENGTH CELL+ !  LENGTH 8 MD5-UPDATE
  H0 @ DIGEST !  H1 @ DIGEST 4 + !  H2 @ DIGEST 8 + !  H3 @ DIGEST 12 + !  DIGEST ;

PRIVATE
32 BUFFER: HEX-OUT
: HEX-DIGIT ( u -- c )  DUP 9 > IF 87 ELSE 48 THEN + ;

PUBLIC
: MD5-HEX ( -- a u )
  16 0 DO
    DIGEST I + C@  DUP 4 RSHIFT HEX-DIGIT  HEX-OUT I 2* + C!
    15 AND HEX-DIGIT  HEX-OUT I 2* 1+ + C!
  LOOP  HEX-OUT 32 ;

END-PACKAGE
