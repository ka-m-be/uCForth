\ ircode.fs - tests of the packages IRCODE and IRCODE-READ (lib/irremote/ircode.fs, irread.fs)
\
\ Load (in the folder ucf), after the package FILES and the tester:
\   ... host/files.fs forth/test/tester.fs lib/test/ircode.fs
\ The times of the Kelon frame were checked with Kai's Python codec
\ (Klima_IR, clima/protocol/kelon168.py, to_timings), used as an oracle.

ONLY FORTH DEFINITIONS  ALSO FILES
S" lib/irremote/ircode.fs" INCLUDED
S" lib/irremote/irread.fs" INCLUDED
S" lib/text.fs" INCLUDED
ONLY FORTH DEFINITIONS  ALSO INTERNALS  ALSO TEXT  ALSO IRCODE  ALSO IRCODE.I  ALSO IRCODE-READ
DECIMAL

CREATE TIMES  400 CELLS ALLOT
CREATE BACK  32 ALLOT
: T@ ( i -- us )   CELLS TIMES + @ ;
: SUM-TIMES ( n -- sum )   \ the low 16 bits of the sum: the same with 16-bit cells
  0 SWAP 0 ?DO  I T@ +  LOOP  $FFFF AND ;
: BLUR ( n -- )   \ as a receiver: marks 20 % longer, spaces 20 % shorter
  0 ?DO  I T@  I 1 AND IF 4 5 */ ELSE 6 5 */ THEN  I CELLS TIMES + !  LOOP ;

\ ---- the NEC code: one block of 4 bytes, no gap
CREATE NEC-BLOCKS  1 C, 4 C,
9000 4500 560 1690 560 0 NEC-BLOCKS SHAPE NEC
CREATE BYTES  $04 C, $FB C, $08 C, $F7 C,
T{ NEC  BYTES 4 TIMES >TIMINGS -> 67 }T
T{ 0 T@  1 T@  2 T@  3 T@  7 T@  66 T@ -> 9000 4500 560 560 1690 560 }T
T{ TIMES 67 BACK 4 TIMINGS> -> 32 }T
T{ BACK 4 BYTES 4 STR= -> TRUE }T

\ ---- the Kelon code: 21 bytes in blocks of 6, 8 and 7 (lib/irremote/kelon168.fs)
CREATE KELON-BLOCKS  3 C, 6 C, 8 C, 7 C,
9000 4600 560 1680 600 8000 KELON-BLOCKS SHAPE KELON
CREATE FRAME1   \ a frame of Kai's notes: cool, 22 C, fan 3, swing, 13:23, key TEMP
  $83 C, $06 C, $82 C, $62 C, $00 C, $00 C, $8D C, $17 C, $40 C, $00 C, $00 C,
  $00 C, $00 C, $3A C, $00 C, $02 C, $00 C, $00 C, $38 C, $00 C, $3A C,
T{ KELON  FRAME1 21 TIMES >TIMINGS -> 343 }T
T{ 98 T@  99 T@  228 T@  229 T@  342 T@ -> 560 8000 560 8000 560 }T
T{ 343 SUM-TIMES -> 63032 }T                        \ as the oracle: 259640 in all
T{ TIMES 343 BACK 32 TIMINGS> -> 168 }T
T{ BACK 21 FRAME1 21 STR= -> TRUE }T
T{ 343 BLUR  TIMES 343 BACK 32 TIMINGS> -> 168 }T    \ a receiver changes the times
T{ BACK 21 FRAME1 21 STR= -> TRUE }T
T{ FRAME1 21 TIMES >TIMINGS  2 -  TIMES 2 CELLS + SWAP  BACK 32 TIMINGS> -> 168 }T   \ no header
T{ BACK 21 FRAME1 21 STR= -> TRUE }T
T{ BACK 32 $55 FILL  FRAME1 21 TIMES >TIMINGS  TIMES SWAP BACK 2 TIMINGS> -> 168 }T   \ room for 2 bytes
T{ BACK 2 FRAME1 2 STR=  BACK 2 + C@ -> TRUE $55 }T
T{ FRAME1 3 TIMES >TIMINGS -> 51 }T                 \ 3 bytes: the frame ends in block 1
T{ TIMES 51 BACK 32 TIMINGS>  BACK 3 FRAME1 3 STR= -> 24 TRUE }T
T{ 0 SHAPE-NOW !  BYTES 4 TIMES ' >TIMINGS CATCH  NIP NIP NIP -> -2 }T

S" ircode" .RESULTS
