\ bridge.fs - package IRBRIDGE: the logic of the IR bridge, IR frames over MQTT
\
\ The bridge hears IR frames and sends them as messages to a broker; it
\ gets messages with frames and sends them as IR (apps/irbridge/README.md).
\ Over IR-QUEUE (the frames in the background), MQTT and JSON. The topics
\ are under IRBRIDGE/<ID>/, with the 12 hex digits of the MAC as the ID;
\ the client identifier is IRBRIDGE<ID>:
\   online   true while connected; false as the will and at BRIDGE-STOP (retained)
\   status   {"version":"0.2","ip":"192.168.1.20","uptime":3600,
\            "ir":{"heard":12,"sent":3,"failed":0},"ac":{"power":true,
\            "mode":"heat","temp":23,"fan":0,"flap":0,"clock":"14:05"}},
\            "broker":"192.168.1.20:1883","mqtt":true}, after each
\            connection and every 60 s (retained); "ac" is the state of the
\            air conditioner for the schema rty3
\   events   each frame heard: {"t":[8991,4507,528,576,1726,816],"s":"0123..."},
\            a table of at most 16 durations and a hex digit for each time;
\            with more durations {"times":[9000,4500,560,...]}
\   send     a frame to send, by its "schema":
\              raw    {"id":7,"t":[...],"s":"..."} as an event, or
\                     {"id":7,"times":[...]}; also without "schema"
\              pd     {"id":7,"schema":"pd","shape":[9000,4500,560,1690,540,0,800],
\                     "blocks":[23],"hex":"6038..."}: the pulse distance code
\              rty3   {"id":7,"schema":"rty3","power":true,"mode":"heat",
\                     "temp":23,"fan":0,"flap":0,"clock":"14:05"}: the fields
\                     change the state, then the whole frame goes out
\   result   {"id":7,"code":0}: 0 sent (its echo came), else the code of
\            IR-QUEUE, JSON or the bridge; without id when the request had none
\ The times are in us, the first one is light (as in IR-QUEUE). A frame
\ heard while MQTT is not connected is lost (counted). The texts of the
\ frames, the schemas and their codes: package BRIDGE-FRAMES (frames.fs).
\
\   out in BRIDGE-START ( -- )   IR at the pins out (the LEDs) and in (the
\                                receiver), MQTT to the broker of MQTT-BROKER,
\                                the step in the chain of IDLE; without a
\                                broker MQTT starts with BRIDGE-RECONNECT
\   BRIDGE-STOP ( -- )           online false, then MQTT and IR off
\   BRIDGE-STEP ( -- )           one step; it does not wait; an error counts
\   .BRIDGE ( -- )               the ID, the counters, the code of the last error;
\                                the broker
\   BRIDGE-RECONNECT ( -- )      a new connection to the broker of MQTT-BROKER
\   a u BRIDGE-STATUS ( -- ta tu ior )   the state of status as JSON in a u
\   BRIDGE-LOG ( -- a u )        the last messages: events, send and result,
\                                a line each ("events {...}"), at most 4 KB
\   mac BRIDGE-ID ( -- a u )     the 12 hex digits of the 6 bytes at mac: the
\                                ID in the topics (BRIDGE-START takes LINK-MAC)
\
\ Load it after IR-QUEUE, MQTT, JSON and BRIDGE-FRAMES (apps/irbridge/app.fs
\ does it).
\ Test on the host: apps/irbridge/test/bridge.fs.

PACKAGE IRBRIDGE  ALSO MQTT  ALSO JSON  ALSO IR-QUEUE  ALSO NET  ALSO INET  ALSO TEXT  ALSO IDLE  ALSO LINK
ALSO BRIDGE-FRAMES

\ the buffers: the ring of IR-QUEUE, the room of MQTT, the texts
4 IR-Q-SLOT * CONSTANT RING-SIZE   RING-SIZE BUFFER: RING   \ 4 frames
9216 CONSTANT ROOM-SIZE   ROOM-SIZE BUFFER: ROOM            \ MQTT: 3 KB out, 3 KB in, 3 KB for TCP
2560 CONSTANT MESSAGE-SIZE   MESSAGE-SIZE BUFFER: MESSAGE   \ a message out: 400 times of 5 digits
16 BUFFER: ID   40 BUFFER: TOPIC                            \ counted: the 12 hex digits; a topic
VARIABLE HEARD   VARIABLE SENT-OK   VARIABLE FAILED-N   VARIABLE LOST   VARIABLE ERROR
VARIABLE WAS-READY   VARIABLE STATUS-AT
VARIABLE PENDING   VARIABLE P-ID   VARIABLE P-HAS           \ a send at work, and its id
VARIABLE R-ID   VARIABLE R-HAS                              \ the id of the request
VARIABLE RUNNING                                            \ BRIDGE-START came, BRIDGE-STOP not yet
4096 CONSTANT LOG-SIZE   LOG-SIZE BUFFER: LOG   VARIABLE LOG-N   \ the last messages, a line each
512 CONSTANT /LINE   /LINE BUFFER: LINE   VARIABLE LINE-N   \ a line of the log, at most /LINE bytes
72 BUFFER: BROKER-TEXT                                      \ "ip:port", counted

: MAKE-ID ( mac -- )   \ the 12 hex digits of the 6 bytes at mac
  0 ID C!  BASE @ >R  HEX
  6 0 DO  DUP I + C@ 0 <# # # #> ID +PLACE  LOOP  DROP  R> BASE ! ;
: TOPIC-OF ( a u -- ta tu )   \ IRBRIDGE/<ID>/ and the name a u
  S" IRBRIDGE/" TOPIC PLACE  ID COUNT TOPIC +PLACE  S" /" TOPIC +PLACE  TOPIC +PLACE  TOPIC COUNT ;

\ ---- the log: the last messages heard, sent and answered, for the web page
: LINE+ ( a u -- )   \ to the line, as much as fits (one byte stays for the LF)
  /LINE 1- LINE-N @ - MIN  >R  LINE LINE-N @ + R@ MOVE  R> LINE-N +! ;
: DROP-LINE ( -- )   \ the oldest line of the log goes
  LOG LOG-N @ 10 SCAN  DUP IF 1/STRING THEN  DUP >R  LOG SWAP MOVE  R> LOG-N ! ;
: LOG+ ( a u ka ku -- )   \ the line "ka ku a u" at the end of the log
  0 LINE-N !  LINE+  S"  " LINE+  LINE+  10 LINE LINE-N @ + C!  1 LINE-N +!
  BEGIN  LOG-N @ LINE-N @ + LOG-SIZE > WHILE  DROP-LINE  REPEAT
  LINE LOG LOG-N @ + LINE-N @ MOVE  LINE-N @ LOG-N +! ;

\ ---- messages out
: ANNOUNCE ( a u -- )   S" online" TOPIC-OF MQTT-RETAIN DROP ;   \ the text true or false, retained
: UPTIME ( -- s )   MILLIS 0 1000 UM/MOD NIP ;
: BROKER$ ( -- a u )   \ the broker as "ip:port"
  MQTT-BROKER@ >R  BROKER-TEXT PLACE  S" :" BROKER-TEXT +PLACE  R> N>S BROKER-TEXT +PLACE  BROKER-TEXT COUNT ;
: STATUS-INTO ( a u -- ta tu ior )   \ the state as JSON in the buffer a u
  JSON-INTO
  S" 0.2" S" version" JSON-TEXT  MY-IP IP>S S" ip" JSON-TEXT  UPTIME S" uptime" JSON-NUMBER
  S" ir" JSON-OBJECT  HEARD @ S" heard" JSON-NUMBER  SENT-OK @ S" sent" JSON-NUMBER
  FAILED-N @ S" failed" JSON-NUMBER  JSON-END  AC-FIELDS
  MQTT-BROKER@ DROP NIP IF  BROKER$ S" broker" JSON-TEXT  THEN   \ none: no field
  MQTT-READY? S" mqtt" JSON-FLAG  JSON-DONE ;
: STATUS ( -- )   \ the state, retained
  MESSAGE MESSAGE-SIZE STATUS-INTO IF 2DROP EXIT THEN
  S" status" TOPIC-OF MQTT-RETAIN DROP  MILLIS STATUS-AT ! ;
: EVENT ( t m -- )   \ a frame heard: to events
  MESSAGE MESSAGE-SIZE JSON-INTO  FRAME-FIELDS
  JSON-DONE IF  2DROP 1 LOST +! EXIT  THEN
  2DUP S" events" LOG+  S" events" TOPIC-OF MQTT-PUBLISH IF 1 LOST +! ELSE 1 HEARD +! THEN ;
: RESULT ( code id has -- )   \ {"id":n,"code":c} to result; no id when the request had none
  MESSAGE MESSAGE-SIZE JSON-INTO  IF S" id" JSON-NUMBER ELSE DROP THEN
  DUP S" code" JSON-NUMBER  IF 1 FAILED-N +! ELSE 1 SENT-OK +! THEN
  JSON-DONE IF 2DROP EXIT THEN  2DUP S" result" LOG+  S" result" TOPIC-OF MQTT-PUBLISH DROP ;

\ ---- a request on send
: ID-OF ( pa pu -- )   \ the id of the request: R-ID, R-HAS
  R-HAS OFF!  S" id" JSON-FIELD 0= IF EXIT THEN
  JSON>NUMBER IF DROP EXIT THEN  R-ID !  R-HAS ON! ;
: REQUEST ( pa pu -- )   \ the frame to IR-QUEUE; an error is the result at once
  2DUP S" send" LOG+  2DUP JSON-CHECK ?DUP IF  NIP NIP 0 FALSE RESULT EXIT  THEN
  2DUP ID-OF  REQUEST-FRAME ?DUP IF  NIP NIP R-ID @ R-HAS @ RESULT EXIT  THEN
  IR-Q-SEND ?DUP IF  R-ID @ R-HAS @ RESULT EXIT  THEN
  R-ID @ P-ID !  R-HAS @ P-HAS !  PENDING ON! ;
: ON-MESSAGE ( pa pu ta tu -- )   S" send" TOPIC-OF STR= IF REQUEST ELSE 2DROP THEN ;

\ ---- the steps
: CONNECTED ( -- )   \ after each new connection: online and status
  MQTT-READY? DUP WAS-READY @ = IF DROP EXIT THEN
  DUP WAS-READY !  IF  S" true" ANNOUNCE  STATUS  THEN ;
: FRAME ( -- )   \ a frame heard: to events; without a connection it is lost
  IR-Q-FRAME? 0= IF EXIT THEN  DROP
  MQTT-READY? IF EVENT ELSE 2DROP 1 LOST +! THEN ;
: DONE ( -- )   \ the send at work has ended: its result
  PENDING @ 0= IF EXIT THEN
  IR-Q-RESULT DUP 1 = IF DROP EXIT THEN
  PENDING OFF!  P-ID @ P-HAS @ RESULT ;
: STEP ( -- )   CONNECTED FRAME DONE
  MQTT-READY?  MILLIS STATUS-AT @ - 60000 < 0=  AND IF STATUS THEN ;

PUBLIC
: BRIDGE-STEP ( -- )   ['] STEP CATCH ?DUP IF  DUP -28 = IF THROW THEN  ERROR !  THEN ;
: BRIDGE-START ( out in -- )   \ the broker first: MQTT-BROKER; else MQTT starts with BRIDGE-RECONNECT
  >R >R  RING RING-SIZE R> R> IR-Q-START
  LINK-MAC MAKE-ID
  S" IRBRIDGE" TOPIC PLACE  ID COUNT TOPIC +PLACE  TOPIC COUNT MQTT-ID
  S" false" S" online" TOPIC-OF MQTT-WILL  ['] ON-MESSAGE MQTT-ON-MESSAGE  ROOM ROOM-SIZE MQTT-ROOM!
  S" send" TOPIC-OF MQTT-SUBSCRIBE THROW
  0 HEARD !  0 SENT-OK !  0 FAILED-N !  0 LOST !  0 ERROR !  WAS-READY OFF!  PENDING OFF!
  0 LOG-N !  RUNNING ON!  ['] BRIDGE-STEP IDLE+
  MQTT-BROKER@ DROP NIP 0= IF EXIT THEN                       \ no broker yet: the web page sets it
  ['] MQTT-START CATCH ?DUP IF  ['] BRIDGE-STEP IDLE-  IR-Q-STOP  RUNNING OFF!  THROW  THEN ;
: BRIDGE-STOP ( -- )   \ online false, then MQTT and IR off
  ['] BRIDGE-STEP IDLE-
  MQTT-READY? IF  S" false" ANNOUNCE  THEN
  MQTT-STOP  IR-Q-STOP  PENDING OFF!  WAS-READY OFF!  RUNNING OFF! ;
: BRIDGE-RECONNECT ( -- )   \ if it runs: online false, then a connection to the broker of MQTT-BROKER
  RUNNING @ 0= IF EXIT THEN
  MQTT-READY? IF  S" false" ANNOUNCE  THEN  MQTT-STOP  WAS-READY OFF!  MQTT-START ;
: BRIDGE-STATUS ( a u -- ta tu ior )   STATUS-INTO ;   \ the state as JSON in the buffer a u
: BRIDGE-LOG ( -- a u )   LOG LOG-N @ ;               \ the last messages, a line each
: BRIDGE-ID ( mac -- a u )   MAKE-ID  ID COUNT ;        \ the ID of the topics
: .BRIDGE ( -- )
  ." bridge " ID COUNT TYPE ."  heard/sent/failed/lost/code: "
  HEARD @ . SENT-OK @ . FAILED-N @ . LOST @ . ERROR @ . CR
  ." broker " MQTT-BROKER@ >R TYPE ." :" R> 0 .R CR ;

END-PACKAGE
