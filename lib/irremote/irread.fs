\ irread.fs - package IRCODE-READ: received times of pulse distance code back into bytes
\
\ The second part of IRCODE (ircode.fs): it reads a frame with the shape
\ of IRCODE back into bytes.
\
\   t m a u TIMINGS> ( -- bits )   m times at t back into bytes at a, at
\                               most u bytes; bits: the number of bits that
\                               came (8 for each byte), also those after u bytes
\
\ TIMINGS> reads what a receiver measured, so it is not strict: a space
\ longer than the middle of the two bit spaces is a 1, and a space of 2/3
\ of the gap or more ends a block. It skips the header when the first
\ mark and space are near (1/3) to the header of the shape.
\
\ Load after IRCODE: ALSO FILES S" lib/irremote/irread.fs" INCLUDED (on the
\ host), or in ucf term: #include lib/irremote/irread.fs (on a board).
\ Example, after the example of ircode.fs:
\
\   ALSO IRCODE-READ
\   CREATE BACK 4 ALLOT
\   TIMES 67 BACK 4 TIMINGS> .            \ 32 bits; BACK holds the 4 bytes again

PACKAGE IRCODE-READ  ALSO IRCODE.I   \ lib/package.fs

VARIABLE DEST                       \ the address of the bytes
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
