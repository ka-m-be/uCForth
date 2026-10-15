\ flpair.fs - package FLASH-PAIR: a record in two sectors of the flash, written in turn
\
\ A record of at most 68 bytes stays in the flash through a power loss:
\ each write goes to the other of two sectors, with a number one higher,
\ a SHA-1 digest and, last, a word that commits it; a read takes the
\ newest whole record. WLSAVE keeps the WLAN profile so
\ (lib/esp32/esp32/wlsave.fs), the IR bridge its broker
\ (apps/irbridge/settings.fs). After each word its buffers are clear
\ again: a record may hold a key.
\
\   a1 a2 magic PAIR name ( -- )   two sectors (4 KB each) at the flash
\                     addresses a1 and a2, for records with the cell magic;
\                     name ( -- pair )
\   a u pair PAIR-WRITE ( -- )   the u bytes at a (at most 68) as the newest
\                     record; it erases only the sector of the older one,
\                     and both sectors when neither holds a whole record
\   a u pair PAIR-READ ( -- f )   the newest record into the u bytes at a;
\                     false: none. A record that is not whole shows a line
\   pair PAIR-ERASE ( -- )   both sectors empty
\
\ The codes: -430 both records are damaged, or they have the same number;
\ -432 the record does not read back as it was written; the codes of
\ FLASH. A record, at the start of its sector: the magic (4 bytes), the
\ number (4), the 68 bytes, the SHA-1 of the 76 bytes before it (20), the
\ commit word $54494D43 (4). That is the layout of WLSAVE since N3e.
\ Load it after FLASH, SHA1 and BYTES (lib/esp32/esp32/wlstack.fs does it).
\ Test on the host: lib/esp32/test/flpair.fs.

PACKAGE FLASH-PAIR  ALSO FLASH  ALSO SHA1  ALSO BYTES   \ lib/package.fs

$54494D43 CONSTANT COMMIT
68 CONSTANT /DATA                    \ the bytes of a record for the program
128 BUFFER: REC-A   128 BUFFER: REC-B   128 BUFFER: WORK
VARIABLE PAIR-AT                     \ the pair of the word at work
VARIABLE VALID-A   VARIABLE VALID-B   VARIABLE ACTIVE
VARIABLE DESTINATION   VARIABLE NEXT-SEQ
: SECTOR ( i -- faddr )   CELLS PAIR-AT @ + @ ;   \ 0 the first, 1 the second
: MAGIC ( -- x )   PAIR-AT @ 2 CELLS + @ ;
: WIPE ( -- )   REC-A 128 0 FILL  REC-B 128 0 FILL  WORK 128 0 FILL  SHA1-CLEAR ;
: .RECORD ( a -- )   \ the flash address of the record read into a
  REC-B = 1 AND SECTOR  ." record at $" .HEX ;
: VALID ( a -- status )   \ 0: empty or foreign, 1: whole, 2: damaged
  DUP @ MAGIC <> IF
    DUP @ -1 <> IF  ." FLASH-PAIR: a foreign " DUP .RECORD ." , not loaded" CR  THEN  DROP 0 EXIT
  THEN
  DUP 96 + @ COMMIT <> IF  ." FLASH-PAIR: an incomplete " DUP .RECORD ." , ignored" CR  DROP 0 EXIT  THEN
  DUP 76 SHA1-DIGEST OVER 76 + 20 BYTES=  SHA1-CLEAR IF  DROP 1 EXIT  THEN
  ." FLASH-PAIR: a damaged " .RECORD CR  2 ;
: READ-SLOTS ( -- )   \ ACTIVE: 0 or 1, the sector of the newer whole record; -1 none
  -1 ACTIVE !
  0 SECTOR REC-A 128 FLASH-READ THROW  1 SECTOR REC-B 128 FLASH-READ THROW
  REC-A VALID VALID-A !  REC-B VALID VALID-B !
  VALID-A @ 1 = VALID-B @ 1 = AND IF
    REC-A 4 + @ REC-B 4 + @ - DUP 0= IF DROP -430 THROW THEN
    0< IF 1 ELSE 0 THEN ACTIVE ! EXIT
  THEN
  VALID-A @ 1 = IF 0 ACTIVE ! EXIT THEN
  VALID-B @ 1 = IF 1 ACTIVE ! EXIT THEN
  VALID-A @ 2 = VALID-B @ 2 = OR IF -430 THROW THEN ;
: ACTIVE-REC ( -- a )   ACTIVE @ IF REC-B ELSE REC-A THEN ;
: (ERASE) ( -- )   0 SECTOR 4096 FLASH-ERASE THROW  1 SECTOR 4096 FLASH-ERASE THROW ;
: (WRITE) ( a u -- )
  DUP /DATA > ABORT" FLASH-PAIR: a record has at most 68 bytes"
  READ-SLOTS
  ACTIVE @ 0< IF  (ERASE)  0 DESTINATION !  1 NEXT-SEQ !         \ the first: old bytes go
  ELSE  1 ACTIVE @ - DESTINATION !  ACTIVE-REC 4 + @ 1+ NEXT-SEQ !  THEN
  WORK 128 0 FILL  MAGIC WORK !  NEXT-SEQ @ WORK 4 + !  WORK 8 + SWAP MOVE
  WORK 76 SHA1-DIGEST WORK 76 + 20 MOVE  SHA1-CLEAR
  DESTINATION @ SECTOR 4096 FLASH-ERASE THROW
  WORK 96 DESTINATION @ SECTOR FLASH-WRITE THROW
  COMMIT WORK 96 + !  WORK 96 + 4 DESTINATION @ SECTOR 96 + FLASH-WRITE THROW   \ commit last
  WORK REC-A 100 MOVE  DESTINATION @ SECTOR WORK 128 FLASH-READ THROW
  WORK REC-A 100 BYTES= 0= IF -432 THROW THEN ;
: (READ) ( a u -- f )
  READ-SLOTS  ACTIVE @ 0< IF  2DROP FALSE EXIT  THEN
  /DATA MIN  ACTIVE-REC 8 + ROT ROT MOVE  TRUE ;
: CLEAN ( i*x xt pair -- j*x )   PAIR-AT !  CATCH  WIPE  THROW ;

PUBLIC
: PAIR ( a1 a2 magic "name" -- )   CREATE  ROT , SWAP , , ;   \ name ( -- pair )
: PAIR-WRITE ( a u pair -- )   ['] (WRITE) SWAP CLEAN ;
: PAIR-READ ( a u pair -- f )   ['] (READ) SWAP CLEAN ;
: PAIR-ERASE ( pair -- )   ['] (ERASE) SWAP CLEAN ;

END-PACKAGE
