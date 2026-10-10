\ fbcon.fs - package FBCON: text on a framebuffer
\
\ A framebuffer is memory with one cell (32 bits on the Pi) for each pixel, line
\ after line; a line has PITCH bytes. The package writes characters of
\ 8 x 8 pixels (package FONT8) into it: COLS characters in a line, ROWS
\ lines on the screen. It needs lib/font8.fs.
\
\   a pitch w h FB-START    use the framebuffer at a, clear it
\   c FB-EMIT               show the character c: CR, LF and backspace
\                           move the cursor, other codes below 32 do
\                           nothing
\   FB-PAGE                 clear the screen, the cursor to the top left
\   FG BG                   the colors of the text and of the background
\
\ At the bottom the text goes on at the top: each new line and the line
\ below it are cleared first. (Moving the whole screen up is slow while
\ the data cache is off.)

PACKAGE FBCON  ALSO FONT8   \ lib/package.fs

PUBLIC
VARIABLE FB   VARIABLE PITCH   VARIABLE COLS   VARIABLE ROWS
VARIABLE FG   VARIABLE BG                  \ BG: four equal bytes (FILL clears)
-1 FG !  0 BG !                              \ white on black

PRIVATE
VARIABLE COL   VARIABLE ROW   VARIABLE GA

: ROW8 ( bits a -- )   \ 8 pixels from a; bit 0 is the left one
  8 0 DO  OVER 1 AND IF FG @ ELSE BG @ THEN  OVER !  CELL+  SWAP 2/ SWAP  LOOP  2DROP ;
: CELL-ADDR ( -- a )   \ the top left pixel of the cursor cell
  ROW @ 8 * PITCH @ *  COL @ 8 CELLS * +  FB @ + ;
: GLYPH ( c -- )   \ the character c in the cursor cell
  >GLYPH GA !  CELL-ADDR
  8 0 DO  GA @ I + C@ OVER ROW8  PITCH @ +  LOOP  DROP ;
: CLEAR-ROW ( row -- )   8 * PITCH @ * FB @ +  PITCH @ 8 *  BG @ FILL ;
: CURSOR ( -- )   \ invert the bottom line of the cursor cell; again: off
  CELL-ADDR PITCH @ 7 * +  8 0 DO  DUP @ INVERT OVER !  CELL+  LOOP  DROP ;
: ROW+ ( row -- row' )   1+  DUP ROWS @ = IF DROP 0 THEN ;
: NEXT-ROW ( -- )   \ clear the new line and the one below it: a gap to the old text
  ROW @ ROW+  DUP ROW !  DUP CLEAR-ROW  ROW+ CLEAR-ROW ;

PUBLIC
: FB-PAGE ( -- )
  FB @ PITCH @ ROWS @ 8 * * BG @ FILL  0 COL !  0 ROW !  CURSOR ;

: FB-START ( a pitch w h -- )
  8 / ROWS !  8 / COLS !  PITCH !  FB !  FB-PAGE ;

: FB-EMIT ( c -- )
  FB @ 0= IF DROP EXIT THEN
  CURSOR                                     \ off
  DUP 13 = IF DROP 0 COL !  CURSOR EXIT THEN
  DUP 10 = IF DROP NEXT-ROW  CURSOR EXIT THEN
  DUP 8 = IF DROP COL @ IF -1 COL +! THEN  CURSOR EXIT THEN
  DUP 32 < IF DROP CURSOR EXIT THEN
  GLYPH  1 COL +!
  COL @ COLS @ = IF 0 COL !  NEXT-ROW THEN  CURSOR ;

END-PACKAGE
