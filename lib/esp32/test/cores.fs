\ cores.fs - the test of MULTICORE on the host, with a mock of the part of the chip
\
\ CORES-MOCK stands for ESP32-CORES, FLASH, IDLE, INTR and TIMERS: it
\ counts the calls, and its "core 1" does what a test puts into BOOT-XT
\ (after CORE-BOOT) and ASK-XT (after CORE-ASK). MILLIS of the mock goes
\ 1 ms further at each call, MS adds its time, so the waits of MULTICORE
\ end at once. The test on the board: lib/esp32/esp32/test/cores.fs.
\ Each system with files and 32-bit cells runs it; in the folder ucf:
\   build/ucf-c -32 host/files.fs lib/esp32/test/cores.fs </dev/null
\ At the end: "cores: 24 tests, 0 errors".

ONLY FORTH DEFINITIONS ALSO FILES
[DEFINED] T{ S" forth/test/tester.fs" ?INCLUDED
PACKAGE CORES-MOCK
PUBLIC
VARIABLE NOW   VARIABLE THE-CORE   VARIABLE ENTRY-XT   VARIABLE BOOT-XT   VARIABLE ASK-XT
VARIABLE SETUPS   VARIABLE BOOTS   VARIABLE RESETS   VARIABLE ASKED   VARIABLE PARKS   VARIABLE HELD
VARIABLE 'FLASH-HOLD   VARIABLE 'FLASH-FREE
VARIABLE CLEARS   VARIABLE INTRS   VARIABLE TIMERSS   VARIABLE CHAINED
2 CONSTANT #CORES
: MILLIS ( -- n )   1 NOW +!  NOW @ ;
: MS ( n -- )   NOW +! ;
: IDLE-CLEAR ( -- )   1 CLEARS +! ;
: IDLE+ ( xt -- )   CHAINED ! ;
: IDLE-RUN ( -- ) ;
: .STOPPED ( xt n -- )   2DROP ;
: INTR-FOR ( i -- )   INTRS ! ;
: TIMERS-FOR ( i -- )   TIMERSS ! ;
: THIS-CORE ( -- i )   THE-CORE @ ;
: CORE-ENTRY ( xt i -- a )   DROP ENTRY-XT !  $40081234 ;
: CORE-PARK ( -- )   1 PARKS +! ;
: CORE-SETUP ( i -- )   DROP  1 SETUPS +! ;
: CORE-BOOT ( a i -- )   2DROP  1 BOOTS +!  BOOT-XT @ ?DUP IF EXECUTE THEN ;
: CORE-RESET ( i -- )   DROP  1 RESETS +! ;
: CORE-ASK ( i -- )   DROP  1 ASKED +!  ASK-XT @ ?DUP IF EXECUTE THEN ;
: CORES-HOLD ( -- )   1 HELD +! ;
: CORES-FREE ( -- )   -1 HELD +! ;
: COUNTS-OFF ( -- )   0 SETUPS !  0 BOOTS !  0 RESETS !  0 ASKED !  0 PARKS ! ;
END-PACKAGE
: CHIP-CORES CORES-MOCK ;  : FLASH CORES-MOCK ;  : IDLE CORES-MOCK ;  : INTR CORES-MOCK ;  : TIMERS CORES-MOCK ;
ONLY FORTH DEFINITIONS ALSO FILES
INCLUDE lib/esp32/multicore.fs
ONLY FORTH DEFINITIONS ALSO INTERNALS
: 'OUT ( -- a )   'EMIT ;                             \ the vector of EMIT (INTERNALS)
ONLY FORTH DEFINITIONS ALSO CORES-MOCK ALSO MULTICORE ALSO MULTICORE.I

: STARTS ( -- )   2 1 STATE! ;                        \ core 1 runs its word
: POLITE ( -- )   -28 1 CODE!  0 1 STATE! ;           \ it stops at the first request
VARIABLE N-ASK
: SECOND ( -- )   1 N-ASK +!  N-ASK @ 2 = IF POLITE THEN ;   \ only at the second
VARIABLE X
: SEVEN ( -- )   7 X ! ;
: THROWS ( -- )   -5 THROW ;
: MAIN ( xt -- )   \ (CORE-MAIN) as core 1 runs it, with xt as its word; 'EMIT stays as it was
  'OUT @ >R  1 CELLS XTS + !  1 THE-CORE !  (CORE-MAIN)  0 THE-CORE !  R> 'OUT ! ;
CREATE SEEN 16 ALLOT   VARIABLE #SEEN
: SEE-IT ( c -- )   SEEN #SEEN @ + C!  1 #SEEN +! ;
: TYPED ( -- a u )   \ what CORES-TYPE writes
  0 #SEEN !  'OUT @ >R  ['] SEE-IT 'OUT !  CORES-TYPE  R> 'OUT !  SEEN #SEEN @ ;
3 CHANNEL CH   7 CHANNEL C7
: ROUNDS ( -- n )   \ 300 times 5 cells through C7: the wrong ones
  0  300 0 DO
    5 0 DO  I C7 CH-PUT 0= IF 1+ THEN  LOOP
    5 0 DO  C7 CH-TAKE 0= IF 1+ ELSE I <> IF 1+ THEN THEN  LOOP
  LOOP ;

\ Channels: empty, full, around the end
T{ CH CH-TAKE -> FALSE }T
T{ 1 CH CH-PUT  2 CH CH-PUT  3 CH CH-PUT  4 CH CH-PUT -> TRUE TRUE TRUE FALSE }T
T{ CH CH-TAKE  CH CH-TAKE -> 1 TRUE 2 TRUE }T
T{ 5 CH CH-PUT  6 CH CH-PUT  7 CH CH-PUT -> TRUE TRUE FALSE }T
T{ CH CH-TAKE  CH CH-TAKE  CH CH-TAKE  CH CH-TAKE -> 3 TRUE 5 TRUE 6 TRUE FALSE }T
T{ ROUNDS -> 0 }T
\ The output of core 1 goes into its channel; core 0 writes it
1 THE-CORE !  'A' CORE-EMIT  'B' CORE-EMIT  0 THE-CORE !
T{ TYPED  OVER C@  ROT 1+ C@  TYPED NIP -> 2 65 66 0 }T
\ The numbers, and the checks of the argument and of the core that asks
T{ CORES CORE# -> 2 0 }T
T{ 0 ' CORE? CATCH NIP  2 ' CORE? CATCH NIP -> -2 -2 }T
1 THE-CORE !
T{ 1 ' CORE-STOP CATCH NIP  CORE# -> -2 1 }T
0 THE-CORE !
\ A start: core 1 comes to its word; FLASH holds it from now on
' STARTS BOOT-XT !  COUNTS-OFF
T{ ' SEVEN 1 CORE-RUN  1 CORE?  SETUPS @ BOOTS @ RESETS @ -> TRUE 1 1 1 }T
T{ ENTRY-XT @  1 CELLS XTS + @  'FLASH-HOLD @ 0<>  'FLASH-FREE @ 0<> -> ' (CORE-MAIN) ' SEVEN TRUE TRUE }T
\ A stop at the first request: its reset, FLASH as before
' POLITE ASK-XT !  COUNTS-OFF
T{ 1 CORE-STOP  1 CORE?  ASKED @ RESETS @  'FLASH-HOLD @ 'FLASH-FREE @ -> FALSE 1 1 0 0 }T
T{ 1 CELLS CODES + @ -> -28 }T
\ A stop at the second request, and a core that hangs: two requests, then the reset
' SEVEN 1 CORE-RUN  ' SECOND ASK-XT !  0 N-ASK !  COUNTS-OFF
T{ 1 CORE-STOP  1 CORE?  ASKED @ RESETS @ -> FALSE 2 1 }T
' SEVEN 1 CORE-RUN  0 ASK-XT !  0 1 CODE!  COUNTS-OFF
T{ NOW @  1 CORE-STOP  NOW @ SWAP -  100 <  1 CORE?  ASKED @ RESETS @  1 CELLS CODES + @ -> FALSE FALSE 2 1 -28 }T
\ A core that does not start: an error, its reset, FLASH as before
0 BOOT-XT !  COUNTS-OFF
T{ ' SEVEN 1 ' CORE-RUN CATCH NIP NIP  1 CORE?  RESETS @  'FLASH-HOLD @ -> -2 FALSE 2 0 }T
\ The word on core 1: a fresh state, its word; it ends, or it throws; then the core waits for its reset
COUNTS-OFF  0 X !  -1 1 CODE!  0 CLEARS !  -1 INTRS !  -1 TIMERSS !
T{ ' SEVEN MAIN  X @  1 STATE@  1 CELLS CODES + @  PARKS @ -> 7 0 0 1 }T
T{ CLEARS @  INTRS @  TIMERSS @  CHAINED @ -> 1 1 1 ' SERVE }T
T{ ' THROWS MAIN  1 STATE@  1 CELLS CODES + @  PARKS @ -> 0 -5 2 }T
\ CORE-RUN after a word that ended by itself: a fresh start
' STARTS BOOT-XT !  COUNTS-OFF
T{ ' SEVEN 1 CORE-RUN  1 CORE?  RESETS @  1 CELLS CODES + @  CHAINED @ -> TRUE 1 0 ' CORES-TYPE }T
\ ON-CORE: the word waits in the channel of core 1 until it runs its chain (SERVE)
0 X !
T{ ' SEVEN 1 ' ON-CORE CATCH NIP NIP  X @ -> -2 0 }T
1 THE-CORE !  SERVE  0 THE-CORE !
T{ X @  1 ASKS-OF CH-TAKE -> 7 FALSE }T
T{ 'FLASH-HOLD @ EXECUTE  HELD @  'FLASH-FREE @ EXECUTE  HELD @ -> 1 0 }T   \ FLASH around a call of the ROM
' POLITE ASK-XT !  1 CORE-STOP

S" cores" .RESULTS
