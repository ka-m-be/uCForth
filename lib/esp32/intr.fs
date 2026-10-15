\ intr.fs - package INTR: interrupts and exceptions of the CPU (ESP32 chips)
\
\ Interrupts. The chip has #SOURCES sources (the layer of the chip,
\ ALSO CHIP; ESP32-S3: 99, for example GPIO 16, UART0 27, timer group 0
\ 50 to 52, FROM_CPU 79 to 82; ESP32: 69). A source that is on goes to
\ a CPU interrupt, and its handler runs. The handler must clear the
\ interrupt in the registers of the source, else it runs again at once.
\ Two kinds of handlers:
\
\ - A Forth word ( -- ): CPU interrupt 1 (level 1). It runs at the next
\   NEXT, between two primitives of the word that runs, on its stacks
\   (the HAL of the target, "Break"). It must leave the stacks as they
\   were. It must not wait, show output, or change the dictionary:
\   targets/esp32s3/README.md, "Words that run later".
\ - A CODE word (ESP32-S3: lib/esp32/esp32s3/asm.fs): CPU interrupt 19,
\   20 or 21 (level 2), so 3 sources at most. It runs at once, in the
\   interrupt, with CALLX0: it may use A0, A2 to A9 and SAR, and it ends
\   with RET, (not NEXT,). It must not change A1 and A10 to A15, and not
\   use the PSRAM (a word of FLASH may run). It can ask for a Forth
\   handler: with the register of FROM_CPU 1 (source 80), for example.
\
\   xt i INT! ( -- )    xt is the handler of source i
\   i INT@ ( -- xt )    the handler of source i, or 0
\   i INT-ON ( -- )     source i goes to its handler
\   i INT-OFF ( -- )    not any more
\   +INT ( -- )         the handlers on (HAL 102)
\   -INT ( -- )         the handlers off (HAL 103)
\
\ An error in a Forth handler switches its source off; the other
\ handlers and the word that ran go on. Its message comes when the
\ console waits for a key (in the chain of the package IDLE):
\     H-ERR stopped: division by zero
\ i INT-ON switches the source on again. The package TIMERS does the
\ same for each word of a timer, with INT-STOPPED:
\
\   xt n INT-STOPPED ( -- )   the handler xt stopped with the error n: its
\                       message comes later (also inside a handler)
\
\ Each core has its own handlers, on its own interrupt matrix (the state
\ of INTR is in the RAM data of the base image: each core has a copy,
\ targets/esp32/README.md, "Two cores"). A core that starts takes its
\ matrix, with no handler yet:
\
\   i INTR-FOR ( -- )   this core is core i (lib/esp32/multicore.fs)
\
\ Ctrl-C stops the word that runs, not the handlers; it waits until a
\ handler ends. Ctrl-C twice within 1 s also switches the handlers off
\ (as -INT), and so does Ctrl-C while another one waits: it stops a
\ handler that hangs (targets/esp32s3/README.md, "Interrupts and errors
\ of the CPU").
\
\ Exceptions. After an exception of the CPU (an address without memory,
\ an unknown instruction, a division by 0) the kernel runs (FAULT): -9
\ THROW with the address and the PC in the message, or -10 THROW:
\
\   ok> $10000000 @
\     error: invalid memory address: $10000000 at pc $4037xxxx
\
\ build.fs gives the kernel the words (ISR) and (FAULT): this package
\ must be in the image (targets/esp32s3/README.md, "Build"). Without it
\ +INT gives -21, and an exception gives -9 without the address.
\
\   FAULT-REGS ( -- a )   the registers of the last exception, a cell
\                       each: PC, PS, SAR, the cause (EXCCAUSE, 64 for a
\                       debug exception), EXCVADDR, a0 to a15, LBEG,
\                       LEND, LCOUNT, WINDOWBASE, WINDOWSTART, DEBUGCAUSE;
\                       the HAL writes them (the monitor shows them:
\                       asm/monitor/monesp.fs)
\   DEBUG-HOOK ( -- a )   a cell after them: the address of machine code
\                       for a debug exception (BREAK, ICOUNT, IBREAK);
\                       0: the HAL handles it as an error. The monitor
\                       sets it while code that it debugs runs

PACKAGE INTR  ALSO CHIP  ALSO IDLE   \ lib/package.fs

INTR-MAP VALUE MAP              \ the matrix of this core: a register for each source, its CPU interrupt
INTR-STATUS VALUE STATUS        \ registers: the sources that ask, 32 in each
1 CONSTANT CPU-INT              \ the CPU interrupt of the Forth handlers: level 1, by level
6 CONSTANT NOWHERE              \ a CPU interrupt that no source reaches (as ESP-IDF)
66 CONSTANT DOCODE               \ the code field of a CODE word (doc/vm.md, section 7)
#SOURCES CELLS BUFFER: HANDLERS
PROTECTED                       \ for the board test lib/esp32/esp32/test/irqueue.fs
#SOURCES BUFFER: CPU            \ a byte for each source: its CPU interrupt, 1 or 19 to 21
PRIVATE
CPU #SOURCES CPU-INT FILL
#SOURCES BUFFER: LIST           \ the sources for Forth handlers that are on, a byte each
VARIABLE #ON                    \ their number
PARSE-NAME CODE-INT! HEADER  104 ,  REVEAL   \ HAL 104 ( xt n -- ): xt handles CPU interrupt n

: ?SOURCE ( i -- i )   DUP #SOURCES U< 0= ABORT" INTR: no such source" ;
: PLACE? ( i -- u true | false )   \ the place of source i in LIST
  #ON @ 0 ?DO  DUP I LIST + C@ = IF  DROP I TRUE UNLOOP EXIT  THEN  LOOP  DROP FALSE ;
: ON? ( i -- f )   \ source i goes to its CPU interrupt
  DUP CELLS MAP + @  SWAP CPU + C@ = ;
: CODE? ( xt -- f )   DUP IF @ DOCODE = THEN ;
: USED? ( n -- f )   \ a source has CPU interrupt n
  FALSE  #SOURCES 0 DO  OVER I CPU + C@ = OR  LOOP  NIP ;
: SLOT ( -- n )   \ a free CPU interrupt for a CODE word
  22 19 DO  I USED? 0= IF I UNLOOP EXIT THEN  LOOP
  TRUE ABORT" INTR: CODE words for 3 sources at most" ;
: FREE ( i -- )   \ source i gives its CPU interrupt for a CODE word back
  CPU +  DUP C@ 1 > IF  0 OVER C@ CODE-INT!  1 SWAP C!  ELSE DROP THEN ;
: REMOVE ( u -- )   \ the place u out of LIST
  LIST + DUP 1+ TUCK  LIST #ON @ + SWAP -  MOVE  -1 #ON +! ;
: ASKS? ( i -- f )   \ source i asks now
  DUP 5 RSHIFT CELLS STATUS + @  SWAP 31 AND RSHIFT  1 AND ;

32 DATA-ALLOT CONSTANT REPORT     \ the message of (FAULT): inside the chip, not in the PSRAM
PUBLIC
28 CELLS DATA-ALLOT CONSTANT FAULT-REGS   \ the registers of the last exception (build.fs tells the HAL)
FAULT-REGS 27 CELLS + CONSTANT DEBUG-HOOK   \ code for a debug exception, or 0 (the HAL reads it)
PRIVATE
VARIABLE #REPORT
: +REPORT ( a u -- )   TUCK  REPORT #REPORT @ + SWAP MOVE  #REPORT +! ;
: +HEX ( x -- )   S" $" +REPORT  BASE @ >R HEX  0 <# # # # # # # # # #>  R> BASE !  +REPORT ;

\ ---- the handlers that stopped: a ring of 4, with the text of the error
4 CONSTANT #STOPS
40 CONSTANT TEXT-SIZE
#STOPS 2 CELLS * DATA-ALLOT CONSTANT STOPS        \ each: the xt, the error
#STOPS TEXT-SIZE * DATA-ALLOT CONSTANT TEXTS      \ each: ERRMSG, counted
VARIABLE STOPS-IN   VARIABLE STOPS-OUT            \ the handlers write, the chain of IDLE reads
VARIABLE STOPS-LOST                               \ the ring was full
: STOP@ ( u -- a )   #STOPS 1- AND  2 CELLS *  STOPS + ;
: TEXT@ ( u -- a )   #STOPS 1- AND  TEXT-SIZE *  TEXTS + ;
: .STOPS ( -- )   \ in the chain of IDLE: the messages of the handlers that stopped
  BEGIN  STOPS-OUT @ STOPS-IN @ <>  WHILE
    STOPS-OUT @ TEXT@ COUNT  ERRMSG CELL+ !  ERRMSG !
    STOPS-OUT @ STOP@  DUP @  SWAP CELL+ @  .STOPPED
    1 STOPS-OUT +!
  REPEAT
  STOPS-LOST @ ?DUP IF  ."   and " DUP . ." more" CR  NEGATE STOPS-LOST +!  THEN ;

PUBLIC
PARSE-NAME +INT HEADER  102 ,  REVEAL
PARSE-NAME -INT HEADER  103 ,  REVEAL

: INT@ ( i -- xt )   ?SOURCE CELLS HANDLERS + @ ;
: INT-ON ( i -- )   ?SOURCE
  DUP CPU + C@ CPU-INT = IF
    DUP PLACE? IF DROP ELSE  DUP LIST #ON @ + C!  1 #ON +!  THEN
    ['] .STOPS IDLE+                                \ the messages of the handlers that stop
  THEN
  DUP CPU + C@  SWAP CELLS MAP + ! ;
: INT-OFF ( i -- )   ?SOURCE  NOWHERE OVER CELLS MAP + !  PLACE? IF REMOVE THEN ;
: INT! ( xt i -- )   \ a source that is on stays on, with the new handler
  ?SOURCE  DUP ON? >R  DUP INT-OFF  DUP FREE
  2DUP CELLS HANDLERS + !                           ( xt i )
  SWAP CODE? IF  SLOT  2DUP SWAP CPU + C!           ( i n )
    OVER CELLS HANDLERS + @ SWAP CODE-INT!
  THEN
  R> IF INT-ON ELSE DROP THEN ;

: INT-STOPPED ( xt n -- )
  STOPS-IN @ STOPS-OUT @ -  #STOPS = IF  2DROP  1 STOPS-LOST +!  EXIT  THEN   \ the ring is full
  STOPS-IN @ STOP@  TUCK CELL+ !  !
  STOPS-IN @ TEXT@  ERRMSG CELL+ @ TEXT-SIZE 1- MIN  2DUP SWAP C!     ( t u )
  ERRMSG @  ROT 1+  ROT MOVE
  1 STOPS-IN +! ;

PRIVATE
VARIABLE BROKE                  \ a handler stopped with -28: Ctrl-C did not wait
: SERVE ( i -- )   \ the handler of source i; an error other than -28 switches source i off
  DUP CELLS HANDLERS + @  ?DUP 0= IF DROP EXIT THEN          ( i xt )
  DUP CATCH  ?DUP 0= IF 2DROP EXIT THEN                      ( i xt n )
  DUP -28 = IF  BROKE ON!  DROP 2DROP EXIT  THEN
  ROT  NOWHERE SWAP CELLS MAP + !                            \ one store: the list stays
  INT-STOPPED ;
PUBLIC
: (ISR) ( -- )   \ the kernel runs it after CPU interrupt 1: the handlers of the sources that ask
  ERRMSG @  ERRMSG CELL+ @  BROKE OFF!                       \ ERRMSG of the word that ran
  #ON @ 0 ?DO
    I LIST + C@  DUP ASKS?  OVER ON? AND IF SERVE ELSE DROP THEN
    BROKE @ IF LEAVE THEN
  LOOP
  ERRMSG CELL+ !  ERRMSG !
  BROKE @ IF  -INT  HANDLER @ IF -28 THROW THEN  QUIT  THEN ;
: (FAULT) ( addr pc cause -- )   \ the kernel runs it after an exception of the CPU
  0 #REPORT !
  6 = IF  2DROP -10  ELSE  SWAP +HEX  S"  at pc " +REPORT  +HEX  -9  THEN   \ 6: IntegerDivideByZero
  REPORT #REPORT @ ROT  HANDLER @ IF NAME-THROW THEN
  >R  ERRMSG CELL+ !  ERRMSG !  R> .ERROR  QUIT ;   \ no CATCH waits: the message, then QUIT
: INTR-FOR ( i -- )   \ this core is core i: its matrix, no handler, no message yet
  DUP CORE-INTR-MAP TO MAP  CORE-INTR-STATUS TO STATUS
  HANDLERS #SOURCES CELLS 0 FILL  CPU #SOURCES CPU-INT FILL  0 #ON !
  STOPS-IN @ STOPS-OUT !  0 STOPS-LOST !  BROKE OFF! ;

END-PACKAGE
