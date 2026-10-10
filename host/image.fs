\ image.fs - package IMAGE: read the image ucf.hex, write a binary file
\
\ The build of a target (targets/.../build.fs) takes the image that
\ FREEZE saved in ucf.hex (doc/vm.md, section 10) and puts it into a
\ program file: an ELF file, a kernel8.img, a flash image, UCF.COM. The
\ package needs the packages FILES and IHEX (lib/ihex.fs). The image has
\ the cell size of this system (16 bits for DOS, else 32 bits).
\
\ Reading ucf.hex:
\   IMAGE-OPEN              open ucf.hex and read its first line; the
\                           start words go to BOOT-XT, QUIT-XT, THROW-XT
\   IMAGE-PRIMS ( -- a u )  the text after "prims=" in the first line
\   IMAGE-RECORD ( -- n addr true | false )
\                           the next data record: n bytes at the image
\                           address addr, in IHEX-BUF; false at the end
\   IMAGE-CLOSE
\   IMAGE-SIZE ( -- u )     the first address after the image
\
\ Writing the file:
\   a u WRITE-START         create the file with the name a u
\   a u PUT                 write u bytes
\   n PUT-ZEROS             write bytes 0 until the file has n bytes
\   offset PUT-IMAGE        write the image: address a goes to offset + a
\   WRITTEN ( -- n )        the bytes in the file so far
\   WRITE-END               close the file
\   ON-PUT ( a u -- )       a vector: PUT gives it each part it writes
\                           (lib/macos/macho.fs hashes the pages there)

PACKAGE IMAGE  ALSO FILES  ALSO IHEX   \ lib/package.fs

PUBLIC
VARIABLE BOOT-XT   VARIABLE QUIT-XT   VARIABLE THROW-XT

PRIVATE
CREATE LBUF 256 ALLOT   VARIABLE IFID   VARIABLE HIGH16
CREATE PRIMS-TEXT 32 ALLOT   0 PRIMS-TEXT C!

: LINE? ( -- a u f )   \ the next line of ucf.hex; f false at the end
  LBUF 256 IFID @ READ-LINE THROW  LBUF ROT ROT ;

VARIABLE KA   VARIABLE KU
: FIELD ( a u ka ku -- n )   \ the decimal number after the text ka ku in the line a u
  KU !  KA !
  BEGIN  DUP KU @ > WHILE
    OVER KA @ KU @ SAME? IF
      KU @ -  SWAP KU @ +  SWAP  0 >R
      BEGIN  DUP IF OVER C@ 48 58 WITHIN ELSE 0 THEN  WHILE
        OVER C@ 48 -  R> 10 * + >R  1- SWAP 1+ SWAP
      REPEAT  2DROP R> EXIT
    THEN  1- SWAP 1+ SWAP
  REPEAT  2DROP  -1 ABORT" the first line of ucf.hex is not right" ;

: PLACE-TEXT ( a u ca -- )   \ a u as a counted text at ca
  2DUP C!  1+ SWAP MOVE ;

PUBLIC
: IMAGE-OPEN ( -- )
  S" ucf.hex" R/O OPEN-FILE THROW IFID !  0 HIGH16 !
  LINE? 0= ABORT" ucf.hex is empty"
  2DUP S" bits=" FIELD CELL 8 * <> ABORT" ucf.hex has another cell size than this system"
  2DUP S" boot=" FIELD BOOT-XT !
  2DUP S" quit=" FIELD QUIT-XT !
  2DUP S" throw=" FIELD THROW-XT !
  0 PRIMS-TEXT C!
  BEGIN  DUP 6 > WHILE                               \ the text after "prims="
    OVER S" prims=" SAME? IF  6 - SWAP 6 + SWAP  31 MIN PRIMS-TEXT PLACE-TEXT EXIT  THEN
    1- SWAP 1+ SWAP
  REPEAT  2DROP ;

: IMAGE-PRIMS ( -- a u )   PRIMS-TEXT COUNT ;

: IMAGE-CLOSE ( -- )   IFID @ CLOSE-FILE THROW ;

: IMAGE-RECORD ( -- n addr true | false )
  BEGIN
    LINE? 0= IF 2DROP FALSE EXIT THEN
    DUP IF OVER C@ 58 = ELSE 0 THEN 0= WHILE 2DROP   \ not a record: the next line
  REPEAT
  IHEX>  DUP 4 = IF                                    \ the upper 16 bits of the address
    DROP 2DROP  IHEX-BUF C@ 8 LSHIFT IHEX-BUF 1+ C@ OR 16 LSHIFT HIGH16 !
    RECURSE EXIT
  THEN
  0= IF  HIGH16 @ +  TRUE EXIT  THEN
  2DROP FALSE ;                                        \ type 1: the end

: IMAGE-SIZE ( -- u )
  0  IMAGE-OPEN
  BEGIN IMAGE-RECORD WHILE  + UMAX  REPEAT
  IMAGE-CLOSE ;

PRIVATE
VARIABLE OFID   VARIABLE POS   VARIABLE BASE-OFFSET
CREATE ZEROS 256 ALLOT   ZEROS 256 0 FILL

PUBLIC
DEFER ON-PUT ( a u -- )   ' 2DROP IS ON-PUT
: WRITE-START ( a u -- )   W/O CREATE-FILE THROW OFID !  0 POS ! ;
: WRITE-END ( -- )   OFID @ CLOSE-FILE THROW ;
: WRITTEN ( -- n )   POS @ ;
: PUT ( a u -- )   2DUP ON-PUT  DUP POS +!  OFID @ WRITE-FILE THROW ;
: PUT-ZEROS ( n -- )
  BEGIN  POS @ OVER U< WHILE  DUP POS @ - 256 MIN  ZEROS SWAP PUT  REPEAT  DROP ;
: PUT-IMAGE ( offset -- )
  BASE-OFFSET !  IMAGE-OPEN
  BEGIN IMAGE-RECORD WHILE
    BASE-OFFSET @ + PUT-ZEROS  IHEX-BUF SWAP PUT
  REPEAT  IMAGE-CLOSE ;

END-PACKAGE
