\ irqueue.fs - package IR-QUEUE: IR frames for the bridge, received and sent in the background
\
\ For the WROOM with IR-CAPTURE (lib/esp32/esp32/ircap.fs). Three parts work
\ together, so that no frame gets lost while the console and the network work:
\   - IR-CAPTURE, a CODE handler at interrupt level 2, copies each frame of
\     the receiver from the RAM of the RMT into one of 4 raw slots;
\   - IR-Q-TICK, a timer word every 1 ms, decodes at most 16 pulse words per
\     step into times in us and puts each frame into the ring that the
\     program gives (IR-Q-SLOT bytes for each frame);
\   - IR-Q-CONTROL, a word in the chain of IDLE, sends a frame after at least
\     15 ms without light. The receiver stays on, so the own frame comes back
\     (the echo): the first frame that ends after the start of the send. With
\     as many times as were sent, the send is done. The lengths of the times
\     do not count: the receiver gives light shorter and darkness longer, more
\     so on long frames. The echo shows that light went out (the LED, its wire,
\     its supply), not that a device got the frame. There is no second attempt.
\ A frame has at most 400 times. A longer frame, or a frame longer than the
\ RAM of the RMT (256 pulse words), is counted and dropped, never cut. The
\ echo does not go into the ring; a frame in its place with another number of
\ times ends the send with -453 and goes into the ring like any other.
\
\   a u out in IR-Q-START ( -- )   a u: the ring, u / IR-Q-SLOT frames (at
\                              least one); the pins (as IR-PINS), then the
\                              services on
\   IR-Q-STOP ( -- )           all off; the buffers and the ring are cleared,
\                              the ring belongs to the program again
\   IR-Q-SLOT ( -- u )         the bytes of a frame in the ring (1 608)
\   IR-Q-FRAME? ( -- a n end-us true | false )   the next frame: n times at a,
\                              valid until the next IR-Q-FRAME? or IR-Q-STOP
\   a n IR-Q-SEND ( -- ior )   copy a frame of n times to send; 0: accepted.
\                              n is odd: the frame ends with light
\   IR-Q-RESULT ( -- ior | 1 ) 1: at work; 0: sent, the echo came
\   IR-Q-IDLE-HIGH ( -- a )    TRUE (the default): the receiver gives a high
\                              level without light, as the transceiver of Adafruit
\   .IR-Q ( -- )               the counters, the longest step, those of IR-CAPTURE
\
\ The codes: -450 a time or a count out of range, or a frame longer than 400
\ times; -451 a send waits already, or the ring was full; -452 no 15 ms
\ without light within 2 s; -453 no echo within 100 ms after the send, or one
\ with another number of times; -454 the
\ queue does not run. An error in a step of IR-Q-TICK or IR-Q-CONTROL shows
\ as the code in .IR-Q; the service goes on. While the queue runs, it alone
\ uses the RMT channels 1 (TX) and 4 (RX): do not use IR-LISTEN, IR-FRAME?
\ or IR-SEND then. If each send ends with -452 and .IR-Q shows many short
\ frames, the input pin floats: check the wire of the receiver.
\
\ Load it in ucf term after IR-CAPTURE (apps/irbridge/app.fs does it).
\ Example, with the transceiver at GPIO 4 (out) and GPIO 14 (in), a ring of 4 frames:
\   ALSO IR-QUEUE  4 IR-Q-SLOT * BUFFER: RING
\   RING 4 IR-Q-SLOT * 4 14 IR-Q-START
\   CREATE F 2000 , 1000 , 600 , 600 , 600 ,
\   F 5 IR-Q-SEND .        \ 0
\   IR-Q-RESULT .          \ 0 (a moment later)
\ Tests: lib/esp32/test/irqueue.fs (host), lib/esp32/esp32/test/irqueue.fs (board).

PACKAGE IR-QUEUE ALSO IR ALSO IR.I ALSO IR-CAPTURE ALSO GPIO ALSO TIMERS ALSO IDLE ALSO CHIP

400 CONSTANT Q-TIMES                      \ the times of a frame, at most
Q-TIMES CELLS 2 CELLS + CONSTANT Q-SIZE   \ a slot: the count, the end in us, the times
Q-TIMES CELLS BUFFER: SEND-TIMES          \ the copy of the frame to send
VARIABLE RING   VARIABLE #SLOTS           \ the ring of the program (0: none), its frames
16 CONSTANT WORDS/STEP                    \ pulse words that a step decodes
15000 CONSTANT QUIET-US                   \ so long without light before a send
0 CONSTANT NO-SEND                        \ the states of SEND-STATE
1 CONSTANT WAIT-QUIET
2 CONSTANT WAIT-ECHO
VARIABLE RUNNING
\ PROTECTED: the words that the board tests use (lib/esp32/esp32/test/irqueue.fs, irqbusy.fs)
PROTECTED
VARIABLE WRITE-N VARIABLE READ-N VARIABLE HELD     \ the ring; HELD: the frame of IR-Q-FRAME?
VARIABLE DECODING                         \ a raw frame is taken from IR-CAPTURE
PRIVATE
VARIABLE RAW-A VARIABLE RAW-N VARIABLE FRAME-US    \ its next pulse word, the words left, its end
VARIABLE RECEIVED  PROTECTED  VARIABLE FULL-N VARIABLE LONG-N  PRIVATE  VARIABLE ERROR
VARIABLE INPUT-PIN
VARIABLE POSITION VARIABLE CAPTURE-US VARIABLE QUIET-AT   \ the quiet: since QUIET-AT
VARIABLE SEND-N VARIABLE SEND-STATE VARIABLE SEND-RESULT
VARIABLE SEND-AT VARIABLE SEND-DEADLINE   \ the end of the echo wait; of the quiet wait
PROTECTED  VARIABLE SENT-N VARIABLE ECHO-BAD  PRIVATE
VARIABLE START-US  PROTECTED  VARIABLE MIN-QUIET  PRIVATE   \ the start of the last send; the shortest quiet
VARIABLE EXPECTED-N                       \ the times of the echo
VARIABLE BUSY                             \ IR-Q-CONTROL runs
PROTECTED
VARIABLE STEP-MAX                         \ the longest step of IR-Q-TICK in us
PUBLIC
VARIABLE IR-Q-IDLE-HIGH
IR-Q-IDLE-HIGH ON!
Q-SIZE CONSTANT IR-Q-SLOT
PRIVATE
: SLOT ( i -- a )   0 #SLOTS @ UM/MOD DROP  Q-SIZE * RING @ + ;
: RING! ( a u -- )   \ the ring of the program: u / Q-SIZE frames
  OVER 0=  OVER Q-SIZE < OR ABORT" IR-QUEUE: the ring needs at least 1608 bytes"
  Q-SIZE / #SLOTS !  RING ! ;
: GIVE-BACK ( -- )   \ the ring cleared, and no longer ours
  RING @ ?DUP IF  #SLOTS @ Q-SIZE * 0 FILL  THEN  0 RING !  0 #SLOTS ! ;
: QUIET-STEP ( -- )   \ light, a new pulse word or a new frame: the quiet starts again
  IR-CAP-POS DUP POSITION @ <> IF  DUP POSITION !  MICROS QUIET-AT !  THEN DROP
  IR-CAP-LAST DUP CAPTURE-US @ <> IF  DUP CAPTURE-US !  MICROS QUIET-AT !  THEN DROP
  INPUT-PIN @ PIN@ IR-Q-IDLE-HIGH @ <> IF  MICROS QUIET-AT !  THEN ;
: QUEUE ( -- )   \ the decoded frame into the ring; with the ring full it is lost (FULL-N)
  #FRAME @ 0= IF EXIT THEN
  WRITE-N @ READ-N @ - #SLOTS @ < IF
    WRITE-N @ SLOT  #FRAME @ OVER !  FRAME-US @ OVER CELL+ !
    2 CELLS + TIMES SWAP #FRAME @ CELLS MOVE
    1 WRITE-N +!  1 RECEIVED +!
  ELSE  1 FULL-N +!  -451 ERROR !  THEN ;
: FINISH ( -- )   \ the raw frame is done: IR-CAPTURE may fill its slot again
  DECODING OFF!  IR-CAP-RELEASE ;
: SEND-FAILED ( ior -- )   \ the send ends with the code ior
  DUP ERROR !  SEND-RESULT !  NO-SEND SEND-STATE ! ;
: ECHO? ( -- f )   \ the frame ended after the start of the own send, which waits for its echo
  SEND-STATE @ WAIT-ECHO =  FRAME-US @ START-US @ - 0< 0=  AND ;
: FRAME-DONE ( -- )   \ a frame is decoded: the echo ends the send, any other goes into the ring
  #FRAME @ IF                                        \ 0: an end without light
    ECHO? IF
      #FRAME @ EXPECTED-N @ = IF  0 SEND-RESULT !  NO-SEND SEND-STATE !
      ELSE  -453 SEND-FAILED  1 ECHO-BAD +!  QUEUE  THEN
    ELSE  QUEUE  THEN
  THEN  FINISH ;
: RAW-WORD ( x -- continue? )   \ the two halves of a pulse word as times
  DUP $7FFF AND OVER 15 RSHIFT 1 AND HALF+ 0= IF DROP FALSE EXIT THEN
  DUP 16 RSHIFT $7FFF AND SWAP 31 RSHIFT HALF+ ;
: DECODE ( -- )   \ a step: at most WORDS/STEP pulse words
  DECODING @ 0= IF
    IR-CAP-FRAME? 0= IF EXIT THEN
    FRAME-US ! RAW-N ! RAW-A !  0 #FRAME !  DECODING ON!
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
  0 SEND-RESULT !  WAIT-QUIET SEND-STATE !
  MICROS 2000000 + SEND-DEADLINE ! ;
: SEND-ONCE ( -- )   \ after QUIET-US without light: send, with the receiver on
  QUIET-STEP
  MICROS QUIET-AT @ - DUP QUIET-US < IF DROP EXIT THEN
  IR-CAP-PENDING? DECODING @ OR IF DROP EXIT THEN    \ a frame still waits for the timer
  MIN-QUIET @ ?DUP IF MIN THEN MIN-QUIET !
  MICROS START-US !  WAIT-ECHO SEND-STATE !  0 SEND-RESULT !
  IR-ECHO @ >R  IR-ECHO ON!
  SEND-TIMES SEND-N @ ['] IR-SEND CATCH
  R> IR-ECHO !  ?DUP IF  >R 2DROP R>  SEND-FAILED  EXIT  THEN
  1 SENT-N +!  MICROS 100000 + SEND-AT ! ;
: CONTROL ( -- )   \ wait for quiet and send; then at most 100 ms for the echo
  SEND-STATE @ WAIT-QUIET = IF
    MICROS SEND-DEADLINE @ - 0< 0= IF  -452 SEND-FAILED  EXIT  THEN
    SEND-ONCE EXIT
  THEN
  SEND-STATE @ WAIT-ECHO =  MICROS SEND-AT @ - 0< 0=  AND IF
    -453 SEND-FAILED  1 ECHO-BAD +!
  THEN ;
: START ( a u out in -- )
  >R >R RING! R> R>  DUP INPUT-PIN ! IR-PINS
  0 WRITE-N ! 0 READ-N ! HELD OFF! DECODING OFF!
  NO-SEND SEND-STATE ! 0 SEND-RESULT !
  0 RECEIVED ! 0 FULL-N ! 0 LONG-N ! 0 ERROR ! 0 SENT-N ! 0 ECHO-BAD ! 0 MIN-QUIET !
  0 STEP-MAX ! BUSY OFF!
  IR-CAP-ON
  IR-CAP-POS POSITION !  IR-CAP-LAST CAPTURE-US !  MICROS QUIET-AT !
  RUNNING ON! ;
PUBLIC
: IR-Q-TICK ( -- )   \ the timer word: decode a part; it measures its time
  RUNNING @ IF  MICROS >R TICK MICROS R> - STEP-MAX @ MAX STEP-MAX !  THEN ;
: IR-Q-CONTROL ( -- )   \ the word in IDLE: the sending; an error ends the send with its code
  RUNNING @ BUSY @ 0= AND IF
    BUSY ON!  ['] CONTROL CATCH  BUSY OFF!
    ?DUP IF  DUP -28 = IF THROW THEN  SEND-FAILED  THEN
  THEN ;
: IR-Q-STOP ( -- )
  RUNNING OFF! ['] IR-Q-TICK 0 EVERY ['] IR-Q-CONTROL IDLE- IR-CAP-OFF
  NO-SEND SEND-STATE ! DECODING OFF! 0 WRITE-N ! 0 READ-N ! HELD OFF!
  GIVE-BACK  SEND-TIMES Q-TIMES CELLS 0 FILL ;
PRIVATE
: START-SERVICES ( a u out in -- )
  START ['] IR-Q-TICK 1 EVERY ['] IR-Q-CONTROL IDLE+ ;
PUBLIC
: IR-Q-START ( a u out in -- )   \ a u: the ring of the program
  RUNNING @ ABORT" IR-QUEUE: already running"
  LISTENER @ ABORT" IR-QUEUE: stop IR-LISTEN first"
  ['] START-SERVICES CATCH DUP IF >R 2DROP 2DROP IR-Q-STOP R> THROW THEN DROP ;
: IR-Q-FRAME? ( -- a n end-us true | false )
  HELD @ IF 1 READ-N +! HELD OFF! THEN
  READ-N @ WRITE-N @ <> IF
    READ-N @ SLOT DUP 2 CELLS + SWAP DUP @ SWAP CELL+ @ TRUE HELD ON!
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
