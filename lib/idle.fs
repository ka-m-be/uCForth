\ idle.fs - package IDLE: words that run while the console waits
\
\ The chain: at most 12 words ( -- ). KEY runs each of them once, again
\ and again, while it waits for a key: at the prompt, and between the
\ bytes of a line. So a word of the chain never runs in the middle of
\ another word. On a board the services of an application run here: the
\ logic of the IR bridge, MQTT, a web page (level 2 in lib/net/README.md;
\ the network itself runs in a timer, level 1). A word of the chain may
\ do all that a word at the console may do: show numbers, use the
\ network, send IR. But it must not wait long (as loop() of Arduino: the
\ other words of the chain and the console wait meanwhile), and it must
\ not read keys.
\
\   xt IDLE+ ( -- )        xt runs from now on while the console waits
\   xt IDLE- ( -- )        not any more
\   IDLE-RUN ( -- )        each word of the chain once (KEY does it)
\   .IDLE ( -- )           show the words of the chain
\   xt n .STOPPED ( -- )   the message for the word xt, which stopped with
\                          the error n (also for the handlers of a board)
\   -BREAK ( -- )          Ctrl-C waits from now on, until +BREAK
\   +BREAK ( -- )          Ctrl-C comes again; a Ctrl-C that waits comes now
\   NO-BREAK ( -- a )      the variable of -BREAK and +BREAK: they count
\
\ Each word of the chain runs with CATCH; BASE and 'EMIT are the same
\ after it as before. An error takes the word out of the chain, and its
\ message shows:
\     M-LOOP stopped: division by zero
\ The message does not start with "error:": a file that ucf term loads
\ meanwhile goes on.
\
\ Ctrl-C does not stop a word of the chain in its middle. On a board (the
\ HAL of the ESP32 chips looks at NO-BREAK) Ctrl-C waits while NO-BREAK
\ is not 0, then it stops the word that waits for the key (at the
\ prompt: a new prompt). IDLE-RUN runs the chain inside -BREAK ... +BREAK,
\ and so does our TCP/IP each of its words (lib/net/linknet.fs). Ctrl-C
\ while another Ctrl-C waits does not wait (a word that hangs):
\ targets/esp32s3/README.md, "Interrupts and errors of the CPU". A host
\ does not look at NO-BREAK.
\
\ The first IDLE+ sets the vector 'KEY? of KEY (forth/core.fs). Until
\ then KEY reads the console as before.
\
\ On a board IDLE is in the image. On a host:  ALSO FILES  INCLUDE lib/idle.fs

PACKAGE IDLE   \ lib/package.fs

12 CONSTANT #CHAIN
#CHAIN CELLS BUFFER: CHAIN        \ the xts of the chain; 0: a free place
CHAIN #CHAIN CELLS 0 FILL         \ BUFFER: does not clear
VARIABLE RUNNING                  \ RP@ of the IDLE-RUN that runs; 0: none
VARIABLE OLD-KEY?                 \ 'KEY? before the first IDLE+

: PLACE-OF ( xt -- a | 0 )   \ the place of xt in the chain; 0 PLACE-OF: a free place
  #CHAIN 0 DO  I CELLS CHAIN +  2DUP @ = IF NIP UNLOOP EXIT THEN  DROP  LOOP  DROP 0 ;

PUBLIC
VARIABLE NO-BREAK
: -BREAK ( -- )   1 NO-BREAK +! ;
: +BREAK ( -- )   NO-BREAK @ 1- 0 MAX NO-BREAK ! ;   \ never below 0 (the HAL sets 0 after a break)
: .STOPPED ( xt n -- )   \ "  name stopped: " and the text of the error n, as .ERROR shows it
  OUT @ IF CR THEN  ."   " SWAP .NAME ."  stopped: "
  DUP -2 = IF  DROP ERRMSG @ ERRMSG CELL+ @ TYPE  ELSE
    DUP MESSAGE ?DUP IF TYPE ELSE DROP ." error " DUP 0 .R THEN
    DUP -9 = OVER -13 = OR OVER -14 = OR SWAP -24 = OR  ERRMSG CELL+ @ AND IF
      ." : " ERRMSG @ ERRMSG CELL+ @ TYPE
    THEN
  THEN  CR  0 ERRMSG CELL+ ! ;

PRIVATE
: RUN ( a -- n )   \ the word at the place a; an error other than -28 takes it out of the chain
  BASE @ >R  'EMIT @ >R
  DUP @ CATCH
  R> 'EMIT !  R> BASE !
  DUP 0=  OVER -28 = OR IF  NIP EXIT  THEN          ( a n )
  OVER @ SWAP .STOPPED  0 SWAP !  0 ;

PUBLIC
: IDLE-RUN ( -- )   \ each word of the chain once; not inside itself
  RUNNING @ ?DUP IF  RP@ U> IF EXIT THEN  THEN      \ deeper on the return stack: inside itself
  RP@ RUNNING !  -BREAK
  0  #CHAIN 0 DO
    I CELLS CHAIN +  DUP @ IF  RUN ?DUP IF NIP LEAVE THEN  ELSE DROP THEN
  LOOP
  +BREAK  0 RUNNING !
  ?DUP IF  HANDLER @ IF THROW THEN  QUIT  THEN ;    \ -28: Ctrl-C did not wait

PRIVATE
: IDLE-KEY? ( -- c true | false )   \ 'KEY?: a key; if none, the chain once
  OLD-KEY? @ EXECUTE  DUP IF EXIT THEN  DROP  IDLE-RUN  FALSE ;

PUBLIC
: IDLE+ ( xt -- )
  'KEY? @ ['] IDLE-KEY? <> IF  'KEY? @ OLD-KEY? !  ['] IDLE-KEY? 'KEY? !  THEN
  DUP PLACE-OF IF DROP EXIT THEN
  0 PLACE-OF  DUP 0= ABORT" IDLE: 12 words in the chain already"  ! ;
: IDLE- ( xt -- )   ?DUP IF  PLACE-OF ?DUP IF 0 SWAP ! THEN  THEN ;
: .IDLE ( -- )   #CHAIN 0 DO  I CELLS CHAIN + @ ?DUP IF .NAME SPACE THEN  LOOP  CR ;

END-PACKAGE
