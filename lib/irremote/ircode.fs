\ ircode.fs - package IRCODE: frames of IR remote controls in pulse distance code
\
\ Most IR remote controls send bytes in pulse distance code. Each bit is
\ a short time with light (the mark), then a time without light (the
\ space): a long space is a 1, a short space is a 0. A frame starts with
\ a long mark and a long space (the header). Its bytes can come in
\ blocks, with a long space (the gap) between two blocks. The lowest bit
\ of a byte goes first. Here each time is a cell, in microseconds (us):
\ mark, space, mark, space ... The last time is the mark after the last
\ bit. Runs on every target (times up to 32767 us also with 16-bit cells).
\
\   hm hs bm one zero gap blocks SHAPE name ( -- )
\                   a shape of frames: the header (mark hm, space hs; hm 0:
\                   no header), a bit (mark bm, then the space of a 1 or
\                   of a 0), the gap, and blocks: a counted list of the
\                   bytes in each block (see the example)
\   name ( -- )     this shape is the shape of >TIMINGS and TIMINGS>
\   a n t >TIMINGS ( -- m )     n bytes at a as m times at t
\   t m a u TIMINGS> ( -- bits )   m times at t back into bytes at a, at
\                               most u bytes; bits: the number of bits that
\                               came (8 for each byte), also those after u bytes
\
\ TIMINGS> reads what a receiver measured, so it is not strict: a space
\ longer than the middle of the two bit spaces is a 1, and a space of 2/3
\ of the gap or more ends a block. It skips the header when the first
\ mark and space are near (1/3) to the header of the shape.
\
\ Load: ALSO FILES S" lib/irremote/ircode.fs" INCLUDED (on the host), or in ucf
\ term: #include lib/irremote/ircode.fs (on a board). Example, the NEC code (4
\ bytes in one block; no gap):
\
\   ALSO IRCODE
\   CREATE NEC-BLOCKS  1 C, 4 C,          \ one block of 4 bytes
\   9000 4500 560 1690 560 0 NEC-BLOCKS SHAPE NEC
\   CREATE BYTES  $04 C, $FB C, $08 C, $F7 C,
\   CREATE TIMES  80 CELLS ALLOT   CREATE BACK 4 ALLOT
\   NEC  BYTES 4 TIMES >TIMINGS .         \ 67 times: 2 + 32 * 2 + 1
\   TIMES 67 BACK 4 TIMINGS> .            \ 32 bits; BACK holds the 4 bytes again

PACKAGE IRCODE   \ lib/package.fs

VARIABLE SHAPE-NOW                  \ the shape of >TIMINGS and TIMINGS>
: FIELD@ ( i -- x )   CELLS SHAPE-NOW @ + @ ;
: HDR-MARK ( -- us )   0 FIELD@ ;
: HDR-SPACE ( -- us )  1 FIELD@ ;
: BIT-MARK ( -- us )   2 FIELD@ ;
: ONE ( -- us )        3 FIELD@ ;
: ZERO ( -- us )       4 FIELD@ ;
: GAP ( -- us )        5 FIELD@ ;
: BLOCKS ( -- a )      6 FIELD@ ;
: ?SHAPE ( -- )   SHAPE-NOW @ 0= ABORT" IRCODE: no shape (SHAPE name, then name)" ;

VARIABLE T-AT                       \ >TIMINGS: the place of the next time
VARIABLE LEFT                       \ the bytes that are still to go
: T, ( us -- )   T-AT @ !  CELL T-AT +! ;
: BYTE, ( c -- )   \ 8 bits, the lowest first
  8 0 DO  BIT-MARK T,  DUP 1 AND IF ONE ELSE ZERO THEN T,  1 RSHIFT  LOOP  DROP ;
: BLOCK, ( a k -- a' )   \ k bytes from a, at most LEFT
  LEFT @ MIN  DUP NEGATE LEFT +!  0 ?DO  DUP C@ BYTE,  1+  LOOP ;

VARIABLE DEST                       \ TIMINGS>: the address of the bytes
VARIABLE ROOM                       \ so many bytes fit at DEST
VARIABLE #BITS                      \ the bits so far
: BIT! ( f -- )   \ the next bit into the bytes at DEST, if there is room
  #BITS @ 3 RSHIFT  DUP ROOM @ < IF
    DEST @ +                                         ( f a )
    #BITS @ 7 AND 0= IF  0 OVER C!  THEN             \ a new byte starts at 0
    SWAP IF  1 #BITS @ 7 AND LSHIFT  OVER C@ OR  SWAP C!  ELSE DROP THEN
  ELSE 2DROP THEN
  1 #BITS +! ;
: NEAR? ( us ref -- f )   \ us is near ref: less than 1/3 of ref away
  DUP 3 /  2DUP - >R  + 1+  R> SWAP WITHIN ;
: PAIR? ( end t -- end t f )   \ a mark at t and its space are there
  2DUP CELL+ > ;
: HEADER? ( end t -- end t f )   \ a header of the shape is at t
  HDR-MARK 0= IF FALSE EXIT THEN
  PAIR? IF  DUP @ HDR-MARK NEAR?  OVER CELL+ @ HDR-SPACE NEAR? AND  ELSE FALSE THEN ;

PUBLIC
: SHAPE ( hm hs bm one zero gap blocks "name" -- )
  CREATE  HERE 7 CELLS ALLOT
  7 0 DO  TUCK 6 I - CELLS + !  LOOP  DROP
  DOES> ( a -- )  SHAPE-NOW ! ;

: >TIMINGS ( a n t -- m )
  ?SHAPE  DUP T-AT !  >R  LEFT !                     ( a )  ( R: t )
  HDR-MARK ?DUP IF  T,  HDR-SPACE T,  THEN
  BLOCKS COUNT OVER + SWAP ?DO
    LEFT @ 0= IF LEAVE THEN
    I C@ BLOCK,  BIT-MARK T,  GAP T,
  LOOP  DROP
  CELL NEGATE T-AT +!                                \ no gap after the last block
  T-AT @ R> - CELL / ;

: TIMINGS> ( t m a u -- bits )
  ?SHAPE  ROOM !  DEST !  0 #BITS !
  CELLS OVER + SWAP                                  ( end t )
  HEADER? IF 2 CELLS + THEN
  BEGIN PAIR? WHILE
    DUP CELL+ @                                      ( end t space )
    DUP GAP 2 3 */ <  GAP 0= OR  IF  ONE ZERO + 2/ > BIT!  ELSE DROP THEN
    2 CELLS +
  REPEAT  2DROP  #BITS @ ;

END-PACKAGE
