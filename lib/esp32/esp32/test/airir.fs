\ airir.fs - package AIR-IR: MQTT and IR together on the board, for a long run (M4; a test)
\
\ The board has the image of apps/irbridge/app.fs, the station is joined
\ (WBACK-READY?), IR-QUEUE runs, MQTT is in the RAM, and a host runs an MQTT
\ broker at port 1883 without a password (on Kai's Mac: Mosquitto,
\ orga/Uebergabe.md). A step in the chain of IDLE does a round once a second:
\   - a message of 3 to 202 bytes to the broker and back; it must come back
\     byte for byte before the next round;
\   - an IR frame: in odd rounds a send with IR-Q-SEND (its echo counts), in
\     even rounds a frame straight from the RMT, which IR-QUEUE must hear
\     with the same number of times, each one within AIR-IR-SLACK us;
\   - the result of each IR frame to the host (topic ucf/wroom/ir: the round
\     and the code; 0 good, -1 other times, -2 not heard, else the ior).
\ The frames mean nothing to a device: a head of 3000 us light and 3000 us
\ dark, which no common remote control uses, then 5 to 197 times of 600 and
\ 1100 us. The host sends numbers to ucf/wroom/in; the step counts the gaps.
\ In ucf term, in the folder ucf, with the address of the host, the bridge
\ stopped (MQTT and IR-QUEUE are in the image of apps/irbridge/app.fs):
\   #include lib/esp32/esp32/test/airir.fs
\   ALSO AIR-IR  AIR-RING 4 IR-Q-SLOT * 4 14 IR-Q-START   the LEDs and the receiver
\   ALSO AIR-IR  S" 192.168.1.20" AIR-IR-START
\   .AIR-IR                               the counters, at any time
\   AIR-IR-STOP
\ On the host, the IR results, and a number each second:
\   mosquitto_sub -h 127.0.0.1 -t ucf/wroom/ir -v
\   (for i in $(seq 1 2000); do echo $i; sleep 1; done) | mosquitto_pub -h 127.0.0.1 -t ucf/wroom/in -l
\ The words show numbers only.

PACKAGE AIR-IR ALSO MQTT ALSO NET ALSO TEXT ALSO BYTES ALSO IR ALSO IR.I ALSO RMT ALSO IR-QUEUE ALSO IDLE

PUBLIC
VARIABLE AIR-IR-SLACK  200 AIR-IR-SLACK !   \ the largest difference of a heard time, us
PRIVATE
VARIABLE ROUND   VARIABLE DUE      VARIABLE WAITING   \ WAITING: 0 none, 1 a send, 2 a heard frame
VARIABLE FAULTS  VARIABLE FAULT-CODE
VARIABLE SENT-OK VARIABLE SENT-BAD VARIABLE SENT-CODE VARIABLE SKIPPED
VARIABLE HEARD-OK VARIABLE HEARD-BAD VARIABLE HEARD-LOST VARIABLE STRAY VARIABLE WORST
VARIABLE ECHO-K  VARIABLE ECHO-OK  VARIABLE ECHO-BAD  VARIABLE ECHO-LATE VARIABLE ECHO-LOST
VARIABLE PUB-FAIL
VARIABLE #IN     VARIABLE NEXT-IN  VARIABLE GAPS      VARIABLE OTHER
202 BUFFER: PAYLOAD  202 BUFFER: EXPECT
PUBLIC  4 IR-Q-SLOT * BUFFER: AIR-RING  PRIVATE   \ the ring of IR-QUEUE: 4 frames
12312 BUFFER: MQ-ROOM                 \ the room of MQTT: packets of at most 4096 bytes
400 CELLS BUFFER: TIMES-OUT  VARIABLE #OUT
40 BUFFER: LINE  VARIABLE #LINE
: ID ( -- a u )   S" ucfwroom" ;
: ALL-T ( -- a u )   S" ucf/wroom/#" ;
: ECHO-T ( -- a u )   S" ucf/wroom/echo" ;
: IN-T ( -- a u )   S" ucf/wroom/in" ;
: IR-T ( -- a u )   S" ucf/wroom/ir" ;

\ ---- the IR frame of round k
: LENGTH-OF ( k -- n )   2/ 3 AND 64 * 5 + ;   \ 5, 69, 133 or 197 times: the last one is light
: TIME-OF ( k i -- us )
  DUP 2 < IF  2DROP 3000 EXIT  THEN              \ the head
  DUP 1 AND 0= IF  2DROP 600 EXIT  THEN          \ light
  + 7 MOD 3 < IF 1100 ELSE 600 THEN ;            \ dark
: BUILD ( k -- )
  DUP LENGTH-OF DUP #OUT !  0 DO  DUP I TIME-OF  I CELLS TIMES-OUT + !  LOOP  DROP ;
: DIRECT ( -- )   \ the frame straight from the RMT, past IR-QUEUE: a frame of another sender
  TIMES-OUT #OUT @ >PULSES  PULSES N-PULSES @ TX RMT-SEND ;
: AS-SENT? ( a n -- f )   \ the heard frame has the times of TIMES-OUT, each within the slack
  DUP #OUT @ <> IF  2DROP FALSE EXIT  THEN
  TRUE SWAP 0 ?DO                                ( a f )
    OVER I CELLS + @  I CELLS TIMES-OUT + @ - ABS
    DUP WORST @ MAX WORST !  AIR-IR-SLACK @ > IF DROP FALSE THEN
  LOOP  NIP ;

\ ---- the messages
: +LINE ( a u -- )   >R  LINE #LINE @ +  R@ MOVE  R> #LINE +! ;
: IOR>S ( n -- a u )   DUP ABS 0 <# #S ROT SIGN #> ;   \ N>S, with the sign
: NOTE-HOST ( code -- )   \ the round and the code of an IR frame to the host
  0 #LINE !  ROUND @ N>S +LINE  S"  " +LINE  IOR>S +LINE
  LINE #LINE @ IR-T MQTT-PUBLISH IF 1 PUB-FAIL +! THEN ;
: PAYLOAD-OF ( k a -- n )   \ the message of round k at a: k in 2 bytes, then 1 to 200 bytes of a pattern
  OVER 8 RSHIFT OVER C!  2DUP 1+ C!
  OVER 37 * 200 MOD 1+                            ( k a m )
  DUP >R  0 DO  OVER I + 251 MOD  OVER 2 + I + C!  LOOP  2DROP  R> 2 + ;
: K-OF ( pa -- k )   DUP C@ 8 LSHIFT SWAP 1+ C@ OR ;
: CHECK-ECHO ( pa pu -- )   \ a message back from the broker: the bytes of its round, and in time
  DUP 3 < IF  2DROP 1 ECHO-BAD +!  EXIT  THEN
  OVER K-OF  DUP >R  EXPECT PAYLOAD-OF            ( pa pu n )  ( R: k )
  OVER = IF  EXPECT SWAP BYTES=  ELSE  2DROP FALSE  THEN
  0= IF  R> DROP  1 ECHO-BAD +!  EXIT  THEN
  R> ECHO-K @ = IF  1 ECHO-OK +!  0 ECHO-K !  ELSE  1 ECHO-LATE +!  THEN ;
: FROM-HOST ( pa pu -- )   \ a number 1, 2, 3 ... from the host
  10 DIGITS 1 = 0= IF  DROP 1 GAPS +!  EXIT  THEN
  DUP NEXT-IN @ <> IF 1 GAPS +! THEN  1+ NEXT-IN !  1 #IN +! ;
: ON-MSG ( pa pu ta tu -- )
  2DUP ECHO-T STR= IF  2DROP CHECK-ECHO  EXIT  THEN
  2DUP IN-T STR= IF  2DROP FROM-HOST  EXIT  THEN
  2DROP 2DROP  1 OTHER +! ;   \ the own messages to ucf/wroom/ir come back too

\ ---- a round, and the checks in between
: ECHO-OUT ( -- )   \ the message of this round; the one before is lost if it did not come
  ECHO-K @ IF 1 ECHO-LOST +! THEN
  ROUND @ DUP ECHO-K !  PAYLOAD PAYLOAD-OF
  PAYLOAD SWAP ECHO-T MQTT-PUBLISH IF 1 PUB-FAIL +! THEN ;
: IR-ROUND ( -- )   \ odd rounds a send, even rounds a frame straight from the RMT
  WAITING @ 1 = IF  1 SKIPPED +!  EXIT  THEN     \ the send before still runs
  WAITING @ 2 = IF  1 HEARD-LOST +!  -2 NOTE-HOST  0 WAITING !  THEN
  ROUND @ DUP BUILD  1 AND IF
    TIMES-OUT #OUT @ IR-Q-SEND ?DUP IF  DUP SENT-CODE !  1 SENT-BAD +!  NOTE-HOST  EXIT  THEN
    1 WAITING !
  ELSE  DIRECT  2 WAITING !  THEN ;
: IR-CHECK ( -- )   \ the result of the send, or a frame that came
  WAITING @ 1 = IF
    IR-Q-RESULT DUP 1 = IF  DROP EXIT  THEN
    DUP NOTE-HOST  ?DUP IF  SENT-CODE !  1 SENT-BAD +!  ELSE  1 SENT-OK +!  THEN
    0 WAITING !  EXIT
  THEN
  IR-Q-FRAME? IF
    DROP  WAITING @ 2 = IF
      AS-SENT? IF  1 HEARD-OK +!  0  ELSE  1 HEARD-BAD +!  -1  THEN  NOTE-HOST  0 WAITING !
    ELSE  2DROP  1 STRAY +!  THEN
  THEN ;
: FAULT ( ior -- )   \ an error of a part goes into the counters; Ctrl-C goes through
  DUP -28 = IF THROW THEN  FAULT-CODE !  1 FAULTS +! ;
: STEP ( -- )   \ the word in IDLE: the checks, and once a second a round
  ['] IR-CHECK CATCH ?DUP IF  FAULT  0 WAITING !  THEN
  MILLIS DUE @ - 0< IF EXIT THEN
  MILLIS 1000 + DUE !  1 ROUND +!
  ['] ECHO-OUT CATCH ?DUP IF FAULT THEN
  ['] IR-ROUND CATCH ?DUP IF  FAULT  0 WAITING !  THEN ;
: CLEAR ( -- )
  0 ROUND !  0 WAITING !  0 FAULTS !  0 FAULT-CODE !
  0 SENT-OK !  0 SENT-BAD !  0 SENT-CODE !  0 SKIPPED !
  0 HEARD-OK !  0 HEARD-BAD !  0 HEARD-LOST !  0 STRAY !  0 WORST !
  0 ECHO-K !  0 ECHO-OK !  0 ECHO-BAD !  0 ECHO-LATE !  0 ECHO-LOST !  0 PUB-FAIL !
  0 #IN !  1 NEXT-IN !  0 GAPS !  0 OTHER ! ;

PUBLIC
: AIR-IR-STEP ( -- )   STEP ;
: AIR-IR-START ( a u -- )   \ the broker at the host a u, port 1883; IR-Q-START first
  0 0 IR-Q-SEND -454 = ABORT" AIR-IR: IR-Q-START first"   \ no frame: only the check that it runs
  1883 MQTT-BROKER  ID MQTT-ID  0 0 0 0 MQTT-WILL  ['] ON-MSG MQTT-ON-MESSAGE  MQ-ROOM 12312 MQTT-ROOM!  CLEAR
  MQTT-START  MILLIS 5000 +  BEGIN  MQTT-STEP NET-POLL  MQTT-READY?  OVER MILLIS - 0<  OR  UNTIL  DROP
  MQTT-READY? 0= ABORT" AIR-IR: no connection to the broker"
  ALL-T MQTT-SUBSCRIBE THROW
  MILLIS 1000 + DUE !  ['] AIR-IR-STEP IDLE+  ." AIR-IR runs" CR ;
: AIR-IR-STOP ( -- )   ['] AIR-IR-STEP IDLE-  MQTT-STOP ;
: .AIR-IR ( -- )
  ." AIR-IR rounds/faults/code: " ROUND @ . FAULTS @ . FAULT-CODE @ . CR
  ." IR sent ok/bad/code/skipped: " SENT-OK @ . SENT-BAD @ . SENT-CODE @ . SKIPPED @ . CR
  ." IR heard ok/bad/lost/stray, worst us: " HEARD-OK @ . HEARD-BAD @ . HEARD-LOST @ . STRAY @ . WORST @ . CR
  ." MQTT echo ok/bad/late/lost/publish failed: " ECHO-OK @ . ECHO-BAD @ . ECHO-LATE @ . ECHO-LOST @ . PUB-FAIL @ . CR
  ." MQTT host in/next/gaps/other: " #IN @ . NEXT-IN @ . GAPS @ . OTHER @ . CR ;

END-PACKAGE
