\ irqueue.fs - package IR-QUEUE: IR frames for the bridge, received and sent in the background
\
\ For the WROOM with IR-CAPTURE (lib/esp32/esp32/ircap.fs). Three parts work
\ together, so that no frame gets lost while the console and the network work:
\   - IR-CAPTURE, a CODE handler at interrupt level 2, copies each frame of
\     the receiver from the RAM of the RMT into one of 4 raw slots;
\   - IR-Q-TICK, a timer word every 1 ms, decodes at most 16 pulse words per
\     step into times in us and puts each frame into a ring of 16 frames;
\   - IR-Q-CONTROL, a word in the chain of IDLE, sends a frame after at least
\     15 ms without light. The receiver stays on, so the own frame comes back
\     (the echo); IR-Q-TICK compares it, at most 32 times per step. A wrong or
\     missing echo means: send again, at most two times more.
\ A frame has at most 400 times. A longer frame, or a frame longer than the
\ RAM of the RMT (256 pulse words), is counted and dropped, never cut. A frame
\ that comes while the queue waits for its echo, and that is not the echo,
\ goes into the ring like any other.
\
\   out in IR-Q-START ( -- )   the pins (as IR-PINS), then the services on
\   IR-Q-STOP ( -- )           all off; the buffers are cleared
\   IR-Q-FRAME? ( -- a n end-us true | false )   the next frame: n times at a,
\                              valid until the next IR-Q-FRAME? or IR-Q-STOP
\   a n IR-Q-SEND ( -- ior )   copy a frame of n times to send; 0: accepted.
\                              n is odd: the frame ends with light
\   IR-Q-RESULT ( -- ior | 1 ) 1: at work; 0: sent, the echo matched
\   IR-Q-IDLE-HIGH ( -- a )    TRUE (the default): the receiver gives a high
\                              level without light, as the transceiver of Adafruit
\   .IR-Q ( -- )               the counters, the longest step, those of IR-CAPTURE
\
\ The codes: -450 a time or a count out of range, or a frame longer than 400
\ times; -451 a send waits already, or the ring was full; -452 no 15 ms
\ without light within 2 s; -453 no matching echo after 3 attempts; -454 the
\ queue does not run. An error in a step of IR-Q-TICK or IR-Q-CONTROL shows
\ as the code in .IR-Q; the service goes on. While the queue runs, it alone
\ uses the RMT channels 1 (TX) and 4 (RX): do not use IR-LISTEN, IR-FRAME?
\ or IR-SEND then. If each send ends with -452 and .IR-Q shows many short
\ frames, the input pin floats: check the wire of the receiver.
\
\ Load it in ucf term after IR-CAPTURE (apps/irbridge/app.fs does it).
\ Example, with the transceiver at GPIO 4 (out) and GPIO 14 (in):
\   ALSO IR-QUEUE  4 14 IR-Q-START
\   CREATE F 2000 , 1000 , 600 , 600 , 600 ,
\   F 5 IR-Q-SEND .        \ 0
\   IR-Q-RESULT .          \ 0 (a moment later)
\ Tests: lib/esp32/test/irqueue.fs (host), lib/esp32/esp32/test/irqueue.fs (board).

PACKAGE IR-QUEUE ALSO IR ALSO IR.I ALSO IR-CAPTURE ALSO GPIO ALSO TIMERS ALSO IDLE ALSO CHIP

16 CONSTANT Q-SLOTS                       \ the ring of received frames
400 CONSTANT Q-TIMES                      \ the times of a frame, at most
Q-TIMES CELLS 2 CELLS + CONSTANT Q-SIZE   \ a slot: the count, the end in us, the times
Q-SLOTS Q-SIZE * BUFFER: FRAMES
Q-TIMES CELLS BUFFER: SEND-TIMES          \ the copy of the frame to send
16 CONSTANT WORDS/STEP                    \ pulse words that a step decodes
32 CONSTANT TIMES/STEP                    \ times of an echo that a step compares
15000 CONSTANT QUIET-US                   \ so long without light before a send
40 CONSTANT ECHO-US                       \ an echo time differs at most so much
0 CONSTANT NO-SEND                        \ the states of SEND-STATE
1 CONSTANT WAIT-QUIET
2 CONSTANT WAIT-ECHO
VARIABLE RUNNING
VARIABLE WRITE-N VARIABLE READ-N VARIABLE HELD     \ the ring; HELD: the frame of IR-Q-FRAME?
VARIABLE DECODING                         \ a raw frame is taken from IR-CAPTURE
VARIABLE RAW-A VARIABLE RAW-N VARIABLE FRAME-US    \ its next pulse word, the words left, its end
VARIABLE COMPARE-I                        \ the next time of the echo to compare; -1: none
VARIABLE RECEIVED VARIABLE FULL-N VARIABLE LONG-N VARIABLE ERROR
VARIABLE INPUT-PIN
VARIABLE POSITION VARIABLE CAPTURE-US VARIABLE QUIET-AT   \ the quiet: since QUIET-AT
VARIABLE SEND-N VARIABLE SEND-STATE VARIABLE SEND-RESULT
VARIABLE SEND-AT VARIABLE SEND-DEADLINE   \ the end of the echo wait; of the quiet wait
VARIABLE ATTEMPTS VARIABLE SENT-N VARIABLE ECHO-BAD
VARIABLE START-US VARIABLE MIN-QUIET      \ the start of the last send; the shortest quiet
VARIABLE EXPECTED-N                       \ the times of the echo
VARIABLE BUSY                             \ IR-Q-CONTROL runs
VARIABLE STEP-MAX                         \ the longest step of IR-Q-TICK in us
PUBLIC
VARIABLE IR-Q-IDLE-HIGH
TRUE IR-Q-IDLE-HIGH !
PRIVATE
: SLOT ( i -- a ) Q-SLOTS 1- AND Q-SIZE * FRAMES + ;
: QUIET-STEP ( -- )   \ light, a new pulse word or a new frame: the quiet starts again
  IR-CAP-POS DUP POSITION @ <> IF  DUP POSITION !  MICROS QUIET-AT !  THEN DROP
  IR-CAP-LAST DUP CAPTURE-US @ <> IF  DUP CAPTURE-US !  MICROS QUIET-AT !  THEN DROP
  INPUT-PIN @ PIN@ IR-Q-IDLE-HIGH @ <> IF  MICROS QUIET-AT !  THEN ;
: QUEUE ( -- )   \ the decoded frame into the ring; with the ring full it is lost (FULL-N)
  #FRAME @ 0= IF EXIT THEN
  WRITE-N @ READ-N @ - Q-SLOTS < IF
    WRITE-N @ SLOT  #FRAME @ OVER !  FRAME-US @ OVER CELL+ !
    2 CELLS + TIMES SWAP #FRAME @ CELLS MOVE
    1 WRITE-N +!  1 RECEIVED +!
  ELSE  1 FULL-N +!  -451 ERROR !  THEN ;
: FINISH ( -- )   \ the raw frame is done: IR-CAPTURE may fill its slot again
  0 DECODING !  -1 COMPARE-I !  IR-CAP-RELEASE ;
: NOT-ECHO ( -- )   \ not the own frame: the attempt failed, the frame is a reception
  -453 SEND-RESULT !  1 ECHO-BAD +!  QUEUE  FINISH ;
: COMPARE ( -- )   \ at most TIMES/STEP times of the echo; at the end the send is done
  COMPARE-I @  DUP TIMES/STEP + #FRAME @ MIN  DUP COMPARE-I !  SWAP ?DO
    TIMES I CELLS + @  SEND-TIMES I CELLS + @ -  ABS ECHO-US < 0= IF  NOT-ECHO UNLOOP EXIT  THEN
  LOOP
  COMPARE-I @ #FRAME @ = IF  0 SEND-RESULT !  NO-SEND SEND-STATE !  FINISH  THEN ;
: ECHO? ( -- f )   \ the frame ended after the start of the own send, which waits for its echo
  SEND-STATE @ WAIT-ECHO =  FRAME-US @ START-US @ - 0< 0=  AND ;
: FRAME-DONE ( -- )   \ a frame is decoded: into the ring, or compare it as the echo
  #FRAME @ 0= IF  FINISH EXIT  THEN                  \ an end without light
  ECHO? 0= IF  QUEUE FINISH EXIT  THEN
  #FRAME @ EXPECTED-N @ <> IF  NOT-ECHO EXIT  THEN
  0 COMPARE-I !  COMPARE ;
: RAW-WORD ( x -- continue? )   \ the two halves of a pulse word as times
  DUP $7FFF AND OVER 15 RSHIFT 1 AND HALF+ 0= IF DROP FALSE EXIT THEN
  DUP 16 RSHIFT $7FFF AND SWAP 31 RSHIFT HALF+ ;
: DECODE ( -- )   \ a step: the next part of the echo, or at most WORDS/STEP pulse words
  COMPARE-I @ 0< 0= IF  COMPARE EXIT  THEN
  DECODING @ 0= IF
    IR-CAP-FRAME? 0= IF EXIT THEN
    FRAME-US ! RAW-N ! RAW-A !  0 #FRAME !  TRUE DECODING !
  THEN
  WORDS/STEP 0 DO
    RAW-A @ @ RAW-WORD 0= IF  FRAME-DONE UNLOOP EXIT  THEN
    CELL RAW-A +!  -1 RAW-N +!
    RAW-N @ 0= IF  FRAME-DONE UNLOOP EXIT  THEN
  LOOP ;
: TICK ( -- )   \ an error drops the frame and counts; only Ctrl-C goes on
  QUIET-STEP  ['] DECODE CATCH ?DUP 0= IF EXIT THEN
  DUP -28 = IF THROW THEN
  DUP ERROR !  -450 = IF 1 LONG-N +! THEN
  DECODING @ IF FINISH THEN ;
: REQUEST! ( a n -- )   \ check the frame and copy it; the sending waits for quiet
  DUP 1 Q-TIMES 1+ WITHIN 0= IF 2DROP -450 THROW THEN
  DUP 0 ?DO  OVER I CELLS + @ 1 12000 WITHIN 0= IF 2DROP UNLOOP -450 THROW THEN  LOOP
  DUP 1 AND 0= IF 1- THEN                            \ a last dark time is not sent
  DUP 0= IF 2DROP -450 THROW THEN
  DUP EXPECTED-N ! DUP SEND-N ! CELLS SEND-TIMES SWAP MOVE
  0 SEND-RESULT !  0 ATTEMPTS !  WAIT-QUIET SEND-STATE !
  MICROS 2000000 + SEND-DEADLINE ! ;
: SEND-ONCE ( -- )   \ after QUIET-US without light: send, with the receiver on
  QUIET-STEP
  MICROS QUIET-AT @ - DUP QUIET-US < IF DROP EXIT THEN
  IR-CAP-PENDING? DECODING @ OR IF DROP EXIT THEN    \ a frame still waits for the timer
  MIN-QUIET @ ?DUP IF MIN THEN MIN-QUIET !
  MICROS START-US !  1 ATTEMPTS +!  WAIT-ECHO SEND-STATE !  0 SEND-RESULT !
  IR-ECHO @ >R  TRUE IR-ECHO !
  SEND-TIMES SEND-N @ ['] IR-SEND CATCH
  R> IR-ECHO !  ?DUP IF  >R 2DROP R>  DUP ERROR !  SEND-RESULT !  NO-SEND SEND-STATE !  EXIT  THEN
  1 SENT-N +!  MICROS 100000 + SEND-AT ! ;
: CONTROL ( -- )   \ wait for quiet and send; a wrong or missing echo: again, at most 3 attempts
  SEND-STATE @ WAIT-QUIET = IF
    MICROS SEND-DEADLINE @ - 0< 0= IF  -452 DUP ERROR ! SEND-RESULT !  NO-SEND SEND-STATE !  EXIT  THEN
    SEND-ONCE EXIT
  THEN
  SEND-STATE @ WAIT-ECHO = IF
    SEND-RESULT @ -453 =  MICROS SEND-AT @ - 0< 0=  OR IF
      SEND-RESULT @ 0= IF  -453 SEND-RESULT !  1 ECHO-BAD +!  THEN
      ATTEMPTS @ 3 < IF
        WAIT-QUIET SEND-STATE !  MICROS QUIET-AT !  MICROS 2000000 + SEND-DEADLINE !
      ELSE  SEND-RESULT @ ERROR !  NO-SEND SEND-STATE !  THEN
    THEN
  THEN ;
: START ( out in -- )
  DUP INPUT-PIN ! IR-PINS
  0 WRITE-N ! 0 READ-N ! FALSE HELD ! 0 DECODING ! -1 COMPARE-I !
  NO-SEND SEND-STATE ! 0 SEND-RESULT !
  0 RECEIVED ! 0 FULL-N ! 0 LONG-N ! 0 ERROR ! 0 SENT-N ! 0 ECHO-BAD ! 0 MIN-QUIET !
  0 STEP-MAX ! FALSE BUSY !
  IR-CAP-ON
  IR-CAP-POS POSITION !  IR-CAP-LAST CAPTURE-US !  MICROS QUIET-AT !
  TRUE RUNNING ! ;
PUBLIC
: IR-Q-TICK ( -- )   \ the timer word: decode or compare a part; it measures its time
  RUNNING @ IF  MICROS >R TICK MICROS R> - STEP-MAX @ MAX STEP-MAX !  THEN ;
: IR-Q-CONTROL ( -- )   \ the word in IDLE: the sending; an error ends the send with its code
  RUNNING @ BUSY @ 0= AND IF
    TRUE BUSY !  ['] CONTROL CATCH  FALSE BUSY !
    ?DUP IF  DUP -28 = IF THROW THEN  DUP ERROR !  SEND-RESULT !  NO-SEND SEND-STATE !  THEN
  THEN ;
: IR-Q-STOP ( -- )
  FALSE RUNNING ! ['] IR-Q-TICK 0 EVERY ['] IR-Q-CONTROL IDLE- IR-CAP-OFF
  NO-SEND SEND-STATE ! 0 DECODING ! -1 COMPARE-I ! 0 WRITE-N ! 0 READ-N ! FALSE HELD !
  FRAMES Q-SLOTS Q-SIZE * 0 FILL SEND-TIMES Q-TIMES CELLS 0 FILL ;
PRIVATE
: START-SERVICES ( out in -- )
  START ['] IR-Q-TICK 1 EVERY ['] IR-Q-CONTROL IDLE+ ;
PUBLIC
: IR-Q-START ( out in -- )
  RUNNING @ ABORT" IR-QUEUE: already running"
  LISTENER @ ABORT" IR-QUEUE: stop IR-LISTEN first"
  ['] START-SERVICES CATCH DUP IF >R 2DROP IR-Q-STOP R> THROW THEN DROP ;
: IR-Q-FRAME? ( -- a n end-us true | false )
  HELD @ IF 1 READ-N +! FALSE HELD ! THEN
  READ-N @ WRITE-N @ <> IF
    READ-N @ SLOT DUP 2 CELLS + SWAP DUP @ SWAP CELL+ @ TRUE TRUE HELD !
  ELSE FALSE THEN ;
: IR-Q-SEND ( a n -- ior )
  RUNNING @ 0= IF 2DROP -454 EXIT THEN
  SEND-STATE @ IF 2DROP -451 EXIT THEN
  ['] REQUEST! CATCH DUP IF >R 2DROP R> THEN ;
: IR-Q-RESULT ( -- ior | 1 ) SEND-STATE @ IF 1 ELSE SEND-RESULT @ THEN ;
: .IR-Q ( -- )
  ." IR received/full/long/sent/echo-bad/code/min quiet us: "
  RECEIVED @ . FULL-N @ . LONG-N @ . SENT-N @ . ECHO-BAD @ . ERROR @ . MIN-QUIET @ . CR
  ." IR step max us: " STEP-MAX @ . CR
  .IR-CAP ;
END-PACKAGE
