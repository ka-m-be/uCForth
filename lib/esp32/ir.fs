\ ir.fs - package IR: frames of IR remote controls over the RMT of the ESP32 chips
\
\ An IR remote control sends pulses of light on a carrier (mostly
\ 38 kHz). This package sends and receives them as times in
\ microseconds (us), one cell each: mark (light), space (no light), mark
\ ... The first time is a mark. The RMT (package RMT) makes the carrier
\ while it sends, and it measures the times at a receiver.
\
\ Made for the IR transceiver of Adafruit (product 5990): its pin IRout
\ switches the IR LEDs on while it is high (the board has no carrier), and
\ its receiver gives the demodulated signal at IRin: low while light with
\ 38 kHz comes. Other 38-kHz receivers (TSOP382, VS1838) work the same.
\ Connect VIN to 3V3, not to 5 V: the receiver gives VIN - 0.4 V at IRin.
\
\   out in IR-PINS        set up: the LEDs at pin out (channel 1 of the RMT,
\                         38 kHz, 33 % light in each period), the receiver
\                         at pin in (channel 4 with 4 blocks of RAM: up to
\                         384 times in a frame on the ESP32-S3; on the
\                         ESP32 400, the size of the buffer TIMES)
\   hz % IR-CARRIER       another carrier: hz, and % light in each period
\   a n IR-SEND           send n times from a (the first is a mark)
\   IR-FRAME? ( -- a n true | false )   a frame that came: n times at a
\   xt IR-LISTEN          xt ( a n -- ) runs for each frame that comes (a
\                         Forth handler, package INTR); 0 IR-LISTEN: no more
\   a n .TIMES            show times
\   IR-ECHO ( -- a )      a variable; false (the start): the receiver does
\                         not listen while the board sends; true: it hears
\                         the own LEDs (a test)
\
\ A frame ends after 12 ms without light. Times up to 32767 us go out in
\ one piece, longer ones in several pieces. A decoded frame above 400
\ times throws -450, rather than returning a truncated frame. Ctrl-C twice switches
\ IR-LISTEN off (it switches all Forth handlers off): +INT switches it
\ on again. An error in the word of IR-LISTEN switches it off: IR-LISTEN
\ again.
\
\ On the board, in ucf term:
\   #require RMT lib/esp32/rmt.fs
\   #include lib/esp32/ir.fs
\ Example: IRout at GPIO 4, IRin at GPIO 14; show the frames of a remote control:
\   ALSO IR  4 14 IR-PINS
\   : SHOW ( a n -- )   DUP . ." times:" CR .TIMES ;
\   ' SHOW IR-LISTEN          \ now press keys on the remote control
\   0 IR-LISTEN

PACKAGE IR  ALSO RMT  ALSO GPIO  ALSO INTR   \ lib/package.fs

PROTECTED                           \ for IR-QUEUE (lib/esp32/esp32/irqueue.fs) and the board tests
1 CONSTANT TX                       \ the sending channel of the RMT
4 CONSTANT RX                       \ the receiving channel, with 4 blocks
400 CONSTANT #TIMES
#TIMES CELLS BUFFER: TIMES          \ the times of the last frame
VARIABLE #FRAME                     \ their number
VARIABLE LEVEL                      \ the level of the last time
256 CONSTANT #PULSES
#PULSES CELLS BUFFER: PULSES          \ the pulse words to send
VARIABLE N-PULSES   VARIABLE HALF   VARIABLE PENDING
VARIABLE LISTENER                   \ the word of IR-LISTEN, or 0
PRIVATE

\ ---- times into pulse words
: PULSE, ( x -- )
  N-PULSES @ #PULSES < 0= ABORT" IR: too many times"
  N-PULSES @ CELLS PULSES + !  1 N-PULSES +! ;
: HALF, ( t level -- )   \ a time and its level: one half of a pulse word
  IF $8000 OR THEN
  HALF @ IF  16 LSHIFT PENDING @ OR PULSE,  HALF OFF!  ELSE  PENDING !  HALF ON!  THEN ;
: TIME, ( t level -- )   \ a time of any length, in pieces of at most 32767 us
  OVER 0= IF 2DROP EXIT THEN
  >R  BEGIN DUP 32767 > WHILE  32767 R@ HALF,  32767 -  REPEAT  R> HALF, ;
PROTECTED
: >PULSES ( a n -- )   \ n times at a as pulse words
  0 N-PULSES !  HALF OFF!
  1 SWAP 0 ?DO  OVER I CELLS + @ OVER TIME,  1 XOR  LOOP  2DROP
  HALF @ IF PENDING @ PULSE, THEN ;                  \ the other half is 0: the end
PRIVATE

\ ---- pulse words into times
: TIME+ ( t level -- )   \ the next time; with the level of the last one it adds to it
  #FRAME @ 0= OVER 0= AND IF 2DROP EXIT THEN        \ a frame starts with a mark
  #FRAME @ IF  DUP LEVEL @ = IF  DROP  #FRAME @ 1- CELLS TIMES + +!  EXIT  THEN  THEN
  LEVEL !  #FRAME @ #TIMES < IF  #FRAME @ CELLS TIMES + !  1 #FRAME +!
  ELSE DROP -450 THROW THEN ;
PROTECTED
: HALF+ ( t level -- f )   \ a half of a pulse word; false: the end of the frame
  OVER 0= IF  2DROP #FRAME @ 0= EXIT  THEN          \ an end before the first time is old
  TIME+ TRUE ;
PRIVATE
: >TIMES ( n -- )   \ the n pulse words in the RAM of the receiving channel as times
  0 #FRAME !  RX RMT-RAM SWAP 0 ?DO                  ( a )
    DUP @  DUP $7FFF AND  OVER 15 RSHIFT 1 AND  HALF+ 0= IF DROP LEAVE THEN
    DUP 16 RSHIFT $7FFF AND  SWAP 31 RSHIFT  HALF+ 0= IF LEAVE THEN
    CELL+
  LOOP  DROP ;

: US-DIV ( -- n )   RMT-HZ 1000000 / ;            \ a tick of 1 us
: FILTER ( -- n )   RMT-HZ 200000 / 255 MIN ;     \ shorter than 5 us: no pulse (at most 255 clocks)

PUBLIC
VARIABLE IR-ECHO
: IR-PINS ( out in -- )
  RMT-ON
  TX US-DIV RMT-DIV  TX 1 RMT-BLOCKS  TX 0 RMT-IDLE  TX 38000 33 RMT-CARRIER
  SWAP TX SWAP RMT-OUT
  DUP INPUT  RX RMT-IN-INV
  RX US-DIV RMT-DIV  RX 4 RMT-BLOCKS  RX FILTER RMT-FILTER  RX 12000 RMT-END  RX 0 RMT-DEMOD
  RX RMT-RX-ON ;
: IR-CARRIER ( hz % -- )   TX ROT ROT RMT-CARRIER ;
: IR-SEND ( a n -- )
  >PULSES
  IR-ECHO @ 0= IF RX RMT-RX-OFF THEN
  PULSES N-PULSES @ TX RMT-SEND
  IR-ECHO @ 0= IF  50 MS  RX RMT-RX-ON  THEN ;
: IR-FRAME? ( -- a n true | false )
  RX RMT-RECEIVED  DUP 0= IF EXIT THEN
  DUP 0 > IF >TIMES ELSE DROP 0 #FRAME ! THEN          \ -1: longer than the RAM
  RX RMT-RX-ON
  #FRAME @ IF  TIMES #FRAME @ TRUE  ELSE  FALSE  THEN ;
PRIVATE
: HEARD ( -- )   \ the handler of the RMT
  IR-FRAME? IF  LISTENER @ ?DUP IF EXECUTE ELSE 2DROP THEN  THEN ;
PUBLIC
: IR-LISTEN ( xt -- )
  DUP LISTENER !
  IF    ['] HEARD RMT-SOURCE INT!  RX TRUE RMT-RX-INT  RMT-SOURCE INT-ON  +INT
  ELSE  RX FALSE RMT-RX-INT  RMT-SOURCE INT-OFF  THEN ;
: .TIMES ( a n -- )
  0 ?DO  DUP I CELLS + @ .  I 1+ 12 MOD 0= IF CR THEN  LOOP  DROP CR ;

END-PACKAGE
