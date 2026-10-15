\ timers.fs - package TIMERS: Forth words that run later, or again and again
\
\   xt ms EVERY ( -- )   xt ( -- ) runs every ms milliseconds; 0 EVERY: not any more
\   xt ms AFTER ( -- )   xt runs once, after ms milliseconds
\   .TIMERS ( -- )       the words that wait, their period, and when they come
\
\ A word waits at most once: EVERY or AFTER with a word that waits
\ already gives it the new time. At most 8 words wait at the same time.
\ They run as Forth handlers of interrupts (package INTR), at the next
\ NEXT: timer 0 of timer group 1 (ESP32-S3: source 53) comes each
\ millisecond while a word waits. EVERY and AFTER switch the handlers on
\ (+INT). Ctrl-C twice switches them off, and so the timers too, until
\ +INT. A word of a timer must not wait, show output, or change the
\ dictionary: the rules are in targets/esp32s3/README.md, "Words that
\ run later". An error stops only this word (as 0 EVERY); its message
\ comes when the console waits:
\     M-TICK stopped: division by zero
\
\ Each core has its own timers: core i uses timer i of timer group 1
\ (CORE-TG1 of the layer of the chip; core 1 of the ESP32: timer 1, source
\ 19). A core that starts takes its timer, with no word waiting yet:
\   i TIMERS-FOR ( -- )   this core is core i (lib/esp32/multicore.fs)
\ On the board, in ucf term:  #include lib/esp32/timers.fs  (needs CHIP and INTR)

PACKAGE TIMERS  ALSO CHIP  ALSO INTR   \ lib/package.fs

8 CONSTANT #TIMERS
#TIMERS 3 * CELLS BUFFER: SLOTS     \ each: xt (0: free), period (0: once), MILLIS when it comes
TG1 VALUE TMR                       \ the registers of the timer of this core
TG1-SOURCE VALUE TMR-SOURCE         \ its source
1 VALUE TMR-BIT                     \ its bit in TG-INT-ENA and TG-INT-CLR of the group

: TIMER ( i -- a )   3 * CELLS SLOTS + ;
: SLOT-OF ( xt -- a | 0 )   \ the place of xt, or 0
  #TIMERS 0 DO  I TIMER @ OVER = IF DROP I TIMER UNLOOP EXIT THEN  LOOP  DROP 0 ;
: WAITING? ( -- f )   #TIMERS 0 DO  I TIMER @ IF TRUE UNLOOP EXIT THEN  LOOP  FALSE ;
: TICK-ON ( -- )   \ an alarm each 1000 us; the counter runs at 1 MHz
  0 TMR !  1000 TMR $10 + !  0 TMR $14 + !
  0 TMR $18 + !  0 TMR $1C + !  1 TMR $20 + !       \ the counter from 0
  TMR-BIT TG1 TG-INT-CLR + !  TMR-BIT TG1 TG-INT-ENA + BIS!   \ its interrupt
  TG-1MHZ TMR ! ;                                    \ on, up, reload, alarm, 1 MHz
: TICK-OFF ( -- )   0 TMR !  TMR-BIT TG1 TG-INT-ENA + BIC!  TMR-BIT TG1 TG-INT-CLR + ! ;
: STALE ( -- )   \ the timer runs while a word waits: else the places are old (a restart)
  TMR @ 0< 0= IF  SLOTS #TIMERS 3 * CELLS 0 FILL  THEN ;
: DUE? ( a -- xt true | false )   \ the word at a, if its time has come
  DUP @ 0= IF DROP FALSE EXIT THEN
  MILLIS OVER 2 CELLS + @ - 0< IF DROP FALSE EXIT THEN
  DUP CELL+ @ ?DUP IF                                ( a period )
    OVER 2 CELLS + +!                                \ the next time
    MILLIS OVER 2 CELLS + @ - 0 > IF  MILLIS OVER CELL+ @ +  OVER 2 CELLS + !  THEN   \ too late: from now on
    @ TRUE
  ELSE  DUP @ SWAP 0 SWAP !  TRUE  THEN ;            \ once: the place is free again
: RUN-WORD ( xt -- )   \ an error other than -28 frees the place of the word
  DUP CATCH  ?DUP 0= IF DROP EXIT THEN               ( xt n )
  DUP -28 = IF THROW THEN                            \ Ctrl-C did not wait: INTR knows
  OVER SLOT-OF ?DUP IF  3 CELLS 0 FILL  THEN  INT-STOPPED ;
: TIMER-TICK ( -- )   \ the handler of the timer
  TMR-BIT TG1 TG-INT-CLR + !  $400 TMR BIS!          \ clear the interrupt; the next alarm
  #TIMERS 0 DO  I TIMER DUE? IF RUN-WORD THEN  LOOP
  WAITING? 0= IF  TICK-OFF  TMR-SOURCE INT-OFF  THEN ;
: SCHEDULE ( xt ms period -- )
  STALE  ROT DUP SLOT-OF ?DUP 0= IF  0 SLOT-OF  THEN
  DUP 0= ABORT" TIMERS: 8 words wait already"        ( ms period xt a )
  >R  ROT MILLIS + R@ 2 CELLS + !  SWAP R@ CELL+ !  R> !   \ the xt last: the timer sees a whole place
  TMR-SOURCE INT@ ['] TIMER-TICK <> IF  ['] TIMER-TICK TMR-SOURCE INT!  THEN
  TMR-SOURCE INT-ON  TICK-ON  +INT ;

PUBLIC
: EVERY ( xt ms -- )
  ?DUP IF  DUP SCHEDULE EXIT  THEN
  SLOT-OF ?DUP IF  3 CELLS 0 FILL  THEN ;
: AFTER ( xt ms -- )   0 SCHEDULE ;
: .TIMERS ( -- )
  STALE  #TIMERS 0 DO  I TIMER @ ?DUP IF
    .NAME SPACE  I TIMER CELL+ @ ?DUP IF ." every " . ." ms, " ELSE ." once, " THEN
    ." in " I TIMER 2 CELLS + @ MILLIS - . ." ms" CR
  THEN  LOOP ;
: TIMERS-FOR ( i -- )   \ this core is core i: its timer, no word waits yet
  DUP CORE-TG1 TO TMR  DUP CORE-TG1-SOURCE TO TMR-SOURCE  1 SWAP LSHIFT TO TMR-BIT
  SLOTS #TIMERS 3 * CELLS 0 FILL ;

END-PACKAGE
