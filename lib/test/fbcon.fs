\ fbcon.fs - tests of the packages FONT8 and FBCON (lib/font8.fs, lib/fbcon.fs)
\
\ Load (in the folder ucf), after the package FILES and the tester:
\   ... host/files.fs forth/test/tester.fs lib/test/fbcon.fs
\ The framebuffer is a small area in the dictionary: 32 x 16 pixels,
\ so 4 characters in a line and 2 lines.

ONLY FORTH DEFINITIONS  ALSO FILES
S" lib/font8.fs" INCLUDED  S" lib/fbcon.fs" INCLUDED
ONLY FORTH DEFINITIONS  ALSO INTERNALS  ALSO FONT8  ALSO FBCON
DECIMAL

32 CELLS CONSTANT TP                  \ the pitch: 32 pixels
CREATE TFB  TP 16 * ALLOT
: PX ( x y -- x' )   TP * SWAP CELLS + TFB + @ ;   \ the pixel at x y

\ FONT8: "A" starts with $0C (two pixels in the middle); outside: "?"
T{ 'A' >GLYPH C@ -> $0C }T
T{ 0 >GLYPH  '?' >GLYPH = -> <TRUE> }T
T{ 127 >GLYPH  '?' >GLYPH = -> <TRUE> }T

\ FB-START: 4 characters, 2 lines; the cursor is the inverted bottom line
T{ TFB TP 32 16 FB-START  COLS @ ROWS @ -> 4 2 }T
T{ 0 7 PX  7 7 PX  8 7 PX  0 0 PX -> -1 -1 0 0 }T

\ A character: row 0 of "A" is $0C, bit 0 is the left pixel
T{ 'A' FB-EMIT  1 0 PX  2 0 PX  3 0 PX  4 0 PX -> 0 -1 -1 0 }T
T{ 0 7 PX  8 7 PX  15 7 PX  16 7 PX -> 0 -1 -1 0 }T        \ the cursor moved on

\ CR: the cursor to the start of the line; the "A" stays
T{ 13 FB-EMIT  2 0 PX  0 7 PX  8 7 PX -> -1 -1 0 }T

\ Backspace at the start of the line does nothing
T{ 8 FB-EMIT  0 7 PX -> -1 }T

\ LF: the next line (cleared); the line below it is cleared too (here: line 0)
T{ 10 FB-EMIT  0 15 PX  0 7 PX  2 0 PX -> -1 0 0 }T

\ At the end of a line the text goes on in the next line
T{ 'B' FB-EMIT 'B' FB-EMIT 'B' FB-EMIT 'B' FB-EMIT  0 7 PX  0 15 PX -> -1 0 }T

\ Codes below 32 do nothing; FB-PAGE clears all
T{ 7 FB-EMIT  0 7 PX -> -1 }T
T{ 'A' FB-EMIT  FB-PAGE  2 0 PX  0 7 PX -> 0 -1 }T

0 FB !                                \ FB-EMIT does nothing without a framebuffer
T{ 'A' FB-EMIT -> }T

S" fbcon" .RESULTS
