\ espimg.fs - package ESPIMG: the image format of the ESP32 ROM bootloader
\
\ An image holds segments (an address on the chip and the bytes for it)
\ and the address where the program starts. The ROM bootloader starts
\ such an image from the flash; "ucf run" loads its segments into the
\ RAM. The format: targets/esp32s3/README.md. Needs FILES and TEXT.
\
\ Write:  entry NEW-IMAGE   a u ta SEGMENT ...   a u WRITE-IMAGE
\ Read:   a u OPEN-IMAGE  (#SEGMENTS, ENTRY and IMAGE-CHIP hold its data), then for
\         each segment SEGMENT-HEAD ( -- ta u ) and a u READ-DATA,
\         at the end CLOSE-IMAGE (checks the checksum)

PACKAGE ESPIMG  ALSO FILES  ALSO TEXT   \ lib/package.fs

PUBLIC
VARIABLE #SEGMENTS
VARIABLE ENTRY                      \ the program starts here
VARIABLE IMAGE-CHIP   9 IMAGE-CHIP !    \ the chip of the image: 0 the ESP32, 9 the ESP32-S3

PRIVATE
$E9 CONSTANT MAGIC
24 CONSTANT HEAD-SIZE               \ the header and the extended header
4 CONSTANT MAX-SEGMENTS

HEAD-SIZE BUFFER: HEAD
MAX-SEGMENTS 3 * CELLS BUFFER: SEGMENTS   \ host address, length, address on the chip
VARIABLE SUM                        \ the checksum: XOR of all segment bytes, from $EF
VARIABLE POS                        \ bytes in the file so far
VARIABLE IMG-FID
16 BUFFER: SCRATCH

: LE! ( x a -- )  4 0 DO  2DUP C!  1+  SWAP 8 RSHIFT SWAP  LOOP  2DROP ;
: LE@ ( a -- x )  0  4 0 DO  8 LSHIFT  OVER 3 I - + C@ OR  LOOP  NIP ;
: SUM+ ( a u -- )  OVER + SWAP ?DO  I C@ SUM @ XOR SUM !  LOOP ;
: 4ALIGNED ( u -- u' )  3 + -4 AND ;
: PAD-SIZE ( -- u )  15 POS @ 15 AND - ;   \ zeros before the checksum byte

\ ---- write ------------------------------------------------------------

PUBLIC
: NEW-IMAGE ( entry -- )  ENTRY !  0 #SEGMENTS ! ;
: SEGMENT ( a u ta -- )   \ the u bytes at a go to the address ta on the chip
  #SEGMENTS @ MAX-SEGMENTS = ABORT" too many segments"
  #SEGMENTS @ 3 * CELLS SEGMENTS +  TUCK 2 CELLS + !  TUCK CELL+ !  !  1 #SEGMENTS +! ;
PRIVATE
: SEG ( i -- a u ta )  3 * CELLS SEGMENTS +  DUP @  OVER CELL+ @  ROT 2 CELLS + @ ;
: >FILE ( a u -- )  DUP POS +!  IMG-FID @ WRITE-FILE ABORT" cannot write the image" ;
: ZEROS ( u -- )  SCRATCH 16 0 FILL  BEGIN DUP WHILE  DUP 16 MIN SCRATCH OVER >FILE  - REPEAT DROP ;
: IMG-HEADER ( -- )   \ the 24 bytes at the start
  HEAD HEAD-SIZE 0 FILL
  MAGIC HEAD C!  #SEGMENTS @ HEAD 1+ C!
  2 HEAD 2 + C!                     \ SPI mode DIO
  IMAGE-CHIP @ IF $4F ELSE $20 THEN HEAD 3 + C!   \ 16 MB, 80 MHz (ESP32-S3); 4 MB, 40 MHz (ESP32)
  ENTRY @ HEAD 4 + LE!
  $EE HEAD 8 + C!                   \ no WP pin
  IMAGE-CHIP @ HEAD 12 + C!
  $FF HEAD 17 + C!  $FF HEAD 18 + C!    \ every revision of the chip
  HEAD HEAD-SIZE >FILE ;
: WRITE-SEGMENT ( a u ta -- )
  SCRATCH LE!  DUP 4ALIGNED SCRATCH 4 + LE!  SCRATCH 8 >FILE
  2DUP SUM+  TUCK >FILE  DUP 4ALIGNED SWAP - ZEROS ;
PUBLIC
: WRITE-IMAGE ( a u -- )   \ write the image with the segments so far to the file a u
  2DUP W/O BIN CREATE-FILE IF  DROP S" cannot create " 2SWAP FAIL  THEN  IMG-FID !  2DROP
  0 POS !  $EF SUM !  IMG-HEADER
  #SEGMENTS @ 0 ?DO  I SEG WRITE-SEGMENT  LOOP
  PAD-SIZE ZEROS  SUM @ SCRATCH C!  SCRATCH 1 >FILE
  IMG-FID @ CLOSE-FILE DROP ;

\ ---- read -------------------------------------------------------------

PRIVATE
: <FILE ( a u -- )   \ read exactly u bytes to a
  DUP POS +!  TUCK IMG-FID @ READ-FILE ABORT" cannot read the image"
  <> ABORT" the image is too short" ;
PUBLIC
: OPEN-IMAGE ( a u -- )   \ open the image file a u; then #SEGMENTS, ENTRY, IMAGE-CHIP hold its data
  2DUP R/O BIN OPEN-FILE IF  DROP S" cannot open " 2SWAP FAIL  THEN  IMG-FID !  2DROP
  0 POS !  $EF SUM !  HEAD HEAD-SIZE <FILE
  HEAD C@ MAGIC <> ABORT" not an image for the ROM bootloader"
  HEAD 12 + C@  DUP IMAGE-CHIP !
  DUP 0<> SWAP 9 <> AND ABORT" the image is neither for the ESP32 nor for the ESP32-S3"
  HEAD 1+ C@ #SEGMENTS !  HEAD 4 + LE@ ENTRY ! ;
: SEGMENT-HEAD ( -- ta u )  SCRATCH 8 <FILE  SCRATCH LE@  SCRATCH 4 + LE@ ;
: READ-DATA ( a u -- )  2DUP <FILE SUM+ ;
: CLOSE-IMAGE ( -- )   \ check the checksum at the end
  PAD-SIZE >R  SCRATCH R@ 1+ <FILE  SCRATCH R> + C@  IMG-FID @ CLOSE-FILE DROP
  SUM @ <> ABORT" the checksum of the image is wrong" ;

END-PACKAGE
