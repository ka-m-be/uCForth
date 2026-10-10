\ ihex.fs - package IHEX: write Intel HEX records
\
\ Intel HEX is text: each line (a record) is ":", the byte count, the
\ address, the type, the data and a checksum, all as hex digits. The
\ checksum makes the sum of all bytes of the record 0 (modulo 256).
\ Images of uCForth use this format (doc/vm.md, section 10).
\
\ This package makes the lines. It gives each line to the vector
\ IHEX-LINE ( a u -- ); the tool puts a word there that writes the line
\ to a file, shows it or compares it.
\
\   IHEX-START              before the first record
\   a u addr IHEX-DATA      records for the u bytes at a; addr is their
\                           address in the image, 16 bytes per record
\   IHEX-END                the last record, :00000001FF
\   a u IHEX>               read one record (see below)
\
\ A record with type 04 (the upper 16 bits of the address) comes only
\ for addresses above $FFFF.

PACKAGE IHEX   \ lib/package.fs

PUBLIC
DEFER IHEX-LINE ( a u -- )   ' TYPE IS IHEX-LINE
CREATE IHEX-BUF 256 ALLOT            \ the data bytes of the record IHEX> reads

PRIVATE
CREATE LINE 48 ALLOT                \ the record that is made now
VARIABLE LEN   VARIABLE SUM   VARIABLE ULBA

: +C  ( c -- )   LINE LEN @ + C!  1 LEN +! ;
: +H2 ( b -- )   \ a byte as two hex digits; it counts for the checksum
  255 AND  DUP SUM +!  DUP 4 RSHIFT DIGIT +C  15 AND DIGIT +C ;
: RECORD ( u addr type -- )   \ start a record
  0 LEN !  0 SUM !  58 +C  ROT +H2  SWAP DUP 8 RSHIFT +H2  +H2  +H2 ;
: END-RECORD ( -- )   SUM @ NEGATE +H2  LINE LEN @ IHEX-LINE ;
: ?ULBA ( addr -- )   \ a record 04 when the upper 16 bits change
  16 RSHIFT  DUP ULBA @ = IF DROP EXIT THEN
  DUP ULBA !  2 0 4 RECORD  DUP 8 RSHIFT +H2  +H2  END-RECORD ;

VARIABLE SRC   VARIABLE CNT   VARIABLE ADR

PUBLIC
: IHEX-START ( -- )   0 ULBA ! ;

: IHEX-DATA ( a u addr -- )
  ADR !  CNT !  SRC !
  BEGIN  CNT @ WHILE
    ADR @ ?ULBA
    CNT @ 16 MIN                             ( n )
    CELL 4 = IF  ADR @ $FFFF AND $FFFF SWAP - 1+ MIN  THEN   \ not over $FFFF
    DUP ADR @ 0 RECORD
    DUP 0 DO  SRC @ I + C@ +H2  LOOP
    END-RECORD
    DUP SRC +!  DUP ADR +!  NEGATE CNT +!
  REPEAT ;

: IHEX-END ( -- )   S" :00000001FF" IHEX-LINE ;

\ Reading: IHEX> ( a u -- n addr type ) takes the record in the text a u.
\ The n data bytes go to IHEX-BUF; addr is the 16-bit address. A wrong
\ checksum stops with an error.

PRIVATE
VARIABLE RA   VARIABLE RSUM   VARIABLE RN   VARIABLE RADR   VARIABLE RTYPE

: HEXDIGIT ( c -- n )   UPC  DUP 58 < IF 48 - ELSE 55 - THEN ;
: RB ( -- b )   \ the next byte of the record
  RA @ C@ HEXDIGIT 4 LSHIFT  RA @ 1+ C@ HEXDIGIT OR  DUP RSUM +!  2 RA +! ;

PUBLIC
: IHEX> ( a u -- n addr type )
  11 < ABORT" an Intel HEX record is too short"
  DUP C@ 58 <> ABORT" not an Intel HEX record"
  1+ RA !  0 RSUM !
  RB RN !  RB 8 LSHIFT RB OR RADR !  RB RTYPE !
  RN @ 0 ?DO  RB IHEX-BUF I + C!  LOOP  RB DROP
  RSUM @ 255 AND ABORT" wrong checksum in an Intel HEX record"
  RN @ RADR @ RTYPE @ ;

END-PACKAGE
