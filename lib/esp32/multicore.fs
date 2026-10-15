\ multicore.fs - package MULTICORE: the cores of the chip, each a Forth computer
\
\ Each core of the chip runs Forth with its own state: its stacks, TIB
\ and PAD, its SYS and the RAM data of the base image (BASE, HANDLER, the
\ number output, 'EMIT, its handlers of interrupts, its timers and its
\ chain of IDLE). The dictionary and all RAM data after the base image
\ are the same for all cores (targets/esp32/hal.fs, "Cores"; orga/016).
\ Core 0 has the console and gives the other cores, 1 to CORES - 1, words
\ to run. The number of a core is on top of the stack:
\
\   CORES ( -- n )          the cores of the chip; the ESP32 has 2
\   CORE# ( -- i )          the core that runs this word: 0 to CORES - 1
\   xt i CORE-RUN ( -- )    core i starts with a fresh state and runs xt.
\                           When xt ends or throws, core i waits for its
\                           reset, and .CORE shows the code
\   i CORE-STOP ( -- )      core i stops: -28 in its CATCH; after 100 ms
\                           at most it is in reset
\   i CORE? ( -- f )        core i runs its word
\   i .CORE ( -- )          its state, and the code of its word
\   xt i ON-CORE ( -- )     core i runs xt once, in its chain of IDLE; it
\                           waits at most 1 s until core i takes it
\   CORE-SERVE ( -- )       a word for CORE-RUN: the chain of IDLE again and
\                           again, with 1 ms of rest (so ON-CORE works)
\
\ Channels carry cells from one core to another: a ring with one writer
\ and one reader, which need no lock (the writer moves only its place,
\ the reader only its own):
\
\   n CHANNEL name ( -- )   a channel with room for n cells; name ( -- ch )
\   x ch CH-PUT ( -- f )    x into the channel; false: it is full
\   ch CH-TAKE ( -- x true | false )   the next x, or false: it is empty
\
\ Only core 0 starts and stops cores. The output of another core (EMIT,
\ TYPE, .) goes into its own channel; core 0 writes it at the console
\ while it waits for a key (in its chain of IDLE). While the channel is
\ full, the other core waits. Example:
\
\   board ok> ALSO MULTICORE  VARIABLE N
\   board ok> : COUNTER ( -- )   BEGIN  1 N +!  AGAIN ;
\   board ok> ' COUNTER 1 CORE-RUN  N @ .  N @ .
\   board ok> 1 CORE-STOP  1 .CORE
\   core 1 stops, code -28
\   board ok> ' CORE-SERVE 1 CORE-RUN
\   board ok> : HELLO ( -- )  ." hello from core " CORE# . CR ;
\   board ok> ' HELLO 1 ON-CORE
\   hello from core 1
\
\ A word on another core must not change the dictionary (:, CREATE,
\ VARIABLE, ALLOT), write the flash, wait for a key, or call the words of
\ a package whose state is on core 0 (the network, IR, the bridge). It may
\ compute, use CATCH and THROW, BASE and the number output, MS and MILLIS
\ (its own clock: MS sleeps), its own handlers (INTR), timers (TIMERS,
\ EVERY) and chain of IDLE, the registers of the chip, channels and the
\ RAM data. Source 25 (FROM_CPU 1) belongs to MULTICORE.
\ While another core is on, FLASH stops it around each call of the ROM
\ ('FLASH-HOLD, 'FLASH-FREE), and FREEZE refuses: CORE-STOP first.
\ Needs FLASH, IDLE, INTR, TIMERS and the part of the chip (CHIP-CORES,
\ lib/esp32/esp32/cores.fs).
\ On the board, in ucf term:  #require MULTICORE lib/esp32/cores.fs
\ Test on the host: lib/esp32/test/cores.fs; on the board:
\ lib/esp32/esp32/test/cores.fs.

PACKAGE MULTICORE  ALSO CHIP-CORES  ALSO FLASH  ALSO IDLE  ALSO INTR  ALSO TIMERS   \ lib/package.fs

\ ---- channels: the place of the writer, of the reader, the number of places, then the cells
: PLACES ( ch -- n )   2 CELLS + @ ;
: AT ( i ch -- a )   SWAP 3 + CELLS + ;              \ the cell at place i
: NEXT-PLACE ( i ch -- i' )   PLACES  SWAP 1+  TUCK = IF DROP 0 THEN ;
: CH-CLEAR ( ch -- )   0 OVER !  0 SWAP CELL+ ! ;    \ empty; only while nobody writes or reads
: CHANNEL-ALLOT ( n -- ch )   \ room for n cells: n + 1 places, one stays free
  1+  DUP 3 + CELLS VALLOT  TUCK 2 CELLS + !  DUP CH-CLEAR ;
PUBLIC
: CHANNEL ( n "name" -- )   CHANNEL-ALLOT CONSTANT ;
: CH-PUT ( x ch -- f )
  DUP @ OVER NEXT-PLACE  OVER CELL+ @ OVER = IF  DROP 2DROP FALSE EXIT  THEN   \ full
  >R  TUCK  DUP @ SWAP AT !  R> SWAP !  TRUE ;      \ the cell first, then the place
: CH-TAKE ( ch -- x true | false )
  DUP CELL+ @  OVER @ = IF  DROP FALSE EXIT  THEN    \ empty
  DUP CELL+ @ OVER AT @                              ( ch x )
  SWAP  DUP CELL+ @ OVER NEXT-PLACE  SWAP CELL+ !  TRUE ;
PRIVATE

\ ---- the state of each core
#CORES CELLS BUFFER: XTS            \ the word of each core
#CORES CELLS BUFFER: CODES          \ the result of its CATCH: 0, or the error
#CORES CELLS BUFFER: STATES         \ 0 stops, 1 starts, 2 runs its word
#CORES CELLS BUFFER: OUTS           \ the channel of its output
#CORES CELLS BUFFER: ASKS           \ the channel of the words of ON-CORE
256 CONSTANT #OUT   4 CONSTANT #ASKS
XTS #CORES CELLS 0 FILL  CODES #CORES CELLS 0 FILL  STATES #CORES CELLS 0 FILL
OUTS #CORES CELLS 0 FILL  ASKS #CORES CELLS 0 FILL
: CHANNELS ( -- )   \ for core 1 to CORES - 1
  #CORES 1 DO  #OUT CHANNEL-ALLOT  I CELLS OUTS + !  #ASKS CHANNEL-ALLOT  I CELLS ASKS + !  LOOP ;
CHANNELS
: STATE@ ( i -- x )   CELLS STATES + @ ;
: STATE! ( x i -- )   CELLS STATES + ! ;
: CODE! ( n i -- )   CELLS CODES + ! ;
: OUT-OF ( i -- ch )   CELLS OUTS + @ ;
: ASKS-OF ( i -- ch )   CELLS ASKS + @ ;
: ?CORE ( i -- i )   \ another core, and core 0 asks
  THIS-CORE ABORT" MULTICORE: only core 0 starts and stops cores"
  DUP 1 #CORES WITHIN 0= ABORT" MULTICORE: no such core" ;

\ ---- on the other core
: CORE-EMIT ( c -- )   \ its 'EMIT: into its channel; it waits while the channel is full
  BEGIN  DUP THIS-CORE OUT-OF CH-PUT 0=  WHILE  1 MS  REPEAT  DROP ;
: SERVE ( -- )   \ in its chain of IDLE: the words of ON-CORE, each once; an error shows its message
  BEGIN  THIS-CORE ASKS-OF CH-TAKE  WHILE  DUP CATCH ?DUP IF .STOPPED ELSE DROP THEN  REPEAT ;
: CORE-WORD ( -- )   \ a fresh state for this core, then its word
  IDLE-CLEAR  THIS-CORE INTR-FOR  THIS-CORE TIMERS-FOR
  ['] CORE-EMIT 'EMIT !  ['] SERVE IDLE+
  2 THIS-CORE STATE!  THIS-CORE CELLS XTS + @ EXECUTE ;
: (CORE-MAIN) ( -- )   \ on core i: its word, then the code; it waits for its reset
  ['] CORE-WORD CATCH  THIS-CORE CODE!  0 THIS-CORE STATE!  CORE-PARK ;

\ ---- on core 0
: CORES-TYPE ( -- )   \ in the chain of IDLE of core 0: the output of the other cores
  #CORES 1 DO  #OUT 0 DO  J OUT-OF CH-TAKE 0= IF LEAVE THEN  EMIT  LOOP  LOOP ;
: STATE? ( x i ms -- f )   \ the state of core i becomes x within ms
  MILLIS + >R
  BEGIN  2DUP STATE@ = IF  2DROP R> DROP TRUE EXIT THEN
         R@ MILLIS - 0<  UNTIL
  2DROP R> DROP FALSE ;
: TAKEN? ( ch ms -- f )   \ the reader of the channel takes all within ms; the output of the cores meanwhile
  MILLIS + >R
  BEGIN  DUP CELL+ @ OVER @ = IF  DROP R> DROP TRUE EXIT THEN
         CORES-TYPE  R@ MILLIS - 0<  UNTIL
  DROP R> DROP FALSE ;
: HOLDS ( -- )   ['] CORES-HOLD 'FLASH-HOLD !  ['] CORES-FREE 'FLASH-FREE ! ;
: FREES ( -- )   \ no core is on any more: FLASH and FREEZE as without MULTICORE
  #CORES 1 DO  I STATE@ IF UNLOOP EXIT THEN  LOOP
  0 'FLASH-HOLD !  0 'FLASH-FREE ! ;

PUBLIC
#CORES CONSTANT CORES
: CORE# ( -- i )   THIS-CORE ;
: CORE? ( i -- f )   ?CORE STATE@ 0<> ;
: CORE-SERVE ( -- )   BEGIN  IDLE-RUN  1 MS  AGAIN ;
: CORE-STOP ( i -- )
  ?CORE  DUP STATE@ IF
    DUP CORE-ASK  0 OVER 50 STATE? 0= IF                    \ a second request does not wait
      DUP CORE-ASK  0 OVER 50 STATE? 0= IF  -28 OVER CODE!  0 OVER STATE!  THEN   \ it hangs: the reset
    THEN
  THEN  CORE-RESET  FREES ;
: CORE-RUN ( xt i -- )
  DUP CORE-STOP  CORES-TYPE  DUP OUT-OF CH-CLEAR  DUP ASKS-OF CH-CLEAR
  TUCK CELLS XTS + !  0 OVER CODE!  1 OVER STATE!  HOLDS  ['] CORES-TYPE IDLE+
  DUP CORE-SETUP  ['] (CORE-MAIN) OVER CORE-ENTRY  OVER CORE-BOOT
  2 OVER 100 STATE? IF DROP EXIT THEN
  DUP CORE-RESET  0 SWAP STATE!  FREES  TRUE ABORT" MULTICORE: the core does not start" ;
: ON-CORE ( xt i -- )
  ?CORE  DUP STATE@ 2 <> ABORT" MULTICORE: the core does not run"
  ASKS-OF  TUCK CH-PUT 0= ABORT" MULTICORE: the core has 4 words to run already"
  1000 TAKEN? 0= ABORT" MULTICORE: the core takes no word (IDLE-RUN)" ;
: .CORE ( i -- )
  ?CORE  ." core " DUP .  DUP STATE@ IF ." runs" ELSE ." stops" THEN
  ." , code " CELLS CODES + @ . CR ;

END-PACKAGE
