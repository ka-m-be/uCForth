\ timers.fs - tests of TIMERS, on the board (lib/esp32/timers.fs)
\
\ On the board, in ucf term, after the tester:
\   #include forth/test/tester.fs
\   #include lib/esp32/test/timers.fs
\ The tests wait about 4 s. One word stops with an error on purpose: its
\ message "BAD-ONE stopped: error -77" shows at the next prompt.

ONLY FORTH DEFINITIONS  ALSO INTERNALS  ALSO TIMERS  ALSO TIMERS.I  ALSO INTR  DECIMAL

VARIABLE RUNS
: RUN1 ( -- )   1 RUNS +! ;
T{ 0 RUNS !  ' RUN1 10 EVERY  1000 MS  ' RUN1 0 EVERY  RUNS @ 98 102 WITHIN -> TRUE }T
T{ RUNS @  100 MS  RUNS @ = -> TRUE }T                    \ 0 EVERY: not any more
VARIABLE ONCE
: ONE ( -- )   1 ONCE +! ;
T{ 0 ONCE !  ' ONE 50 AFTER  30 MS ONCE @  40 MS ONCE @  100 MS ONCE @ -> 0 1 1 }T
T{ 0 RUNS !  0 ONCE !  ' RUN1 20 EVERY  ' ONE 30 EVERY  1000 MS
   ' RUN1 0 EVERY  ' ONE 0 EVERY  RUNS @ 48 52 WITHIN  ONCE @ 31 35 WITHIN -> TRUE TRUE }T
VARIABLE BLINKS
: BLINK ( -- )   1 BLINKS +! ;
T{ 0 BLINKS !  ' BLINK 100 EVERY  1000 MS  ' BLINK 0 EVERY  BLINKS @ 9 11 WITHIN -> TRUE }T
T{ ' BLINK 100 EVERY  ' BLINK 200 EVERY  0 BLINKS !  1000 MS  ' BLINK 0 EVERY  BLINKS @ 4 6 WITHIN -> TRUE }T   \ a word waits once
VARIABLE BADS
: BAD-ONE ( -- )   1 BADS +!  BADS @ 3 = IF -77 THROW THEN ;
T{ 0 BADS !  0 RUNS !  ' BAD-ONE 10 EVERY  ' RUN1 10 EVERY  200 MS  ' RUN1 0 EVERY
   BADS @  ' BAD-ONE SLOT-OF  RUNS @ 18 22 WITHIN -> 3 0 TRUE }T          \ an error stops only its word
-INT

S" timers" .RESULTS
