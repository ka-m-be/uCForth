\ irsym.fs - tests of the package IR-SYMBOLS (lib/irremote/irsym.fs)
\
\ Load (in the folder ucf), after the package FILES and the tester:
\   ... host/files.fs forth/test/tester.fs lib/test/irsym.fs
\ RAW is a real frame of the remote control RCH-RTY3-0 (lib/test/rty3raw.fs);
\ the table of its 6 durations is that of orga/Archiv/013 §3 (a script in Python).

ONLY FORTH DEFINITIONS  ALSO FILES
S" lib/irremote/irsym.fs" INCLUDED
S" lib/test/rty3raw.fs" INCLUDED
S" lib/text.fs" INCLUDED
ONLY FORTH DEFINITIONS  ALSO TEXT  ALSO IR-SYMBOLS  ALSO IR-SYMBOLS.I
DECIMAL

400 BUFFER: SYMS
400 CELLS BUFFER: BACK
: D@ ( i -- us )   CELLS DURATIONS + @ ;
: BACK@ ( i -- us )   CELLS BACK + @ ;
: RAW@ ( i -- us )   CELLS RAW + @ ;
: WORST ( m -- n )   \ the largest difference of BACK to RAW, in 0.1 % of RAW
  0 SWAP 0 ?DO  I RAW@ I BACK@ - ABS  1000 I RAW@ */  MAX  LOOP ;

\ ---- the real frame: 6 durations, the digits, and back
T{ RAW 371 SYMS >SYMBOLS -> 6 TRUE }T
T{ 0 D@ 1 D@ 2 D@ 3 D@ 4 D@ 5 D@ -> 8991 4507 528 576 1726 816 }T
T{ SYMS 6 S" 012323" STR=  SYMS 370 + 1 S" 5" STR= -> TRUE TRUE }T
T{ DURATIONS 6 SYMS 371 BACK SYMBOLS> -> 371 TRUE }T
T{ 0 BACK@  13 BACK@  370 BACK@  371 WORST 63 < -> 8991 1726 816 TRUE }T

\ ---- alike: at most 20 %, at least 80 us
T{ 1200 1000 ALIKE?  1201 1000 ALIKE?  800 1000 ALIKE?  799 1000 ALIKE? -> TRUE FALSE TRUE FALSE }T
T{ 180 100 ALIKE?  181 100 ALIKE?  20 100 ALIKE?  19 100 ALIKE? -> TRUE FALSE TRUE FALSE }T

\ ---- marks and spaces have their own durations; 16 at most
CREATE MANY
  200 , 300 , 400 , 600 , 800 , 1200 , 1600 , 2400 , 3200 , 4800 , 6400 , 9600 ,
  12800 , 19200 , 25600 , 150 , 100 ,
T{ MANY 16 SYMS >SYMBOLS  SYMS 16 S" 0123456789ABCDEF" STR= -> 16 TRUE TRUE }T
T{ MANY 17 SYMS >SYMBOLS -> FALSE }T                  \ the 17th duration
CREATE SAME  560 , 560 , 560 ,
T{ SAME 3 SYMS >SYMBOLS  SYMS 3 S" 010" STR= -> 2 TRUE TRUE }T
T{ SAME 0 SYMS >SYMBOLS -> 0 TRUE }T

\ ---- the sums do not overflow with 16-bit cells
CREATE LONG  30000 , 30001 , 30002 , 30003 , 30004 ,
T{ LONG 5 SYMS >SYMBOLS  0 D@  1 D@ -> 2 TRUE 30002 30002 }T

\ ---- digits back into times: capitals and small letters; a digit outside the table
CREATE T12  1 , 2 , 3 , 4 , 5 , 6 , 7 , 8 , 9 , 10 , 11 , 12 ,
T{ T12 12 S" 0aB" BACK SYMBOLS>  0 BACK@ 1 BACK@ 2 BACK@ -> 3 TRUE 1 11 12 }T
T{ T12 12 S" 0C" BACK SYMBOLS> -> FALSE }T
T{ T12 12 S" 0G" BACK SYMBOLS> -> FALSE }T
T{ T12 12 S" " BACK SYMBOLS> -> 0 TRUE }T

S" irsym" .RESULTS
