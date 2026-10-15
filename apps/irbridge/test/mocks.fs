\ mocks.fs - the mocks of MQTT and IR-QUEUE for the tests of the IR bridge (apps/irbridge/test/)
\
\ MQTT and IR-QUEUE as the bridge sees them: MQTT keeps the last four
\ messages that the bridge publishes and hands it messages (DELIVER),
\ IR-QUEUE gives it frames (HEAR) and keeps what it sends. S" cannot hold
\ a ": the texts of the tests have ' for each ", and Q (helpers.fs)
\ turns them back. Load it before apps/irbridge/bridge.fs, after TEXT,
\ INET, IDLE and JSON, and after NET and LINK: the real ones, or those of
\ mocknet.fs; then the helpers need the bridge.

PACKAGE MQTT  ALSO TEXT
PUBLIC
VARIABLE READY   VARIABLE STARTED   VARIABLE #PUB   VARIABLE ROOM-U   VARIABLE ON-MSG
64 BUFFER: M-ID   64 BUFFER: W-TOPIC   16 BUFFER: W-TEXT   64 BUFFER: SUB   \ counted
64 BUFFER: M-HOST   0 M-HOST C!   VARIABLE M-PORT                           \ the broker, counted
\ a message kept: retain, the topic (64 bytes, counted), the length and the text
2 CELLS 64 + 2600 + CONSTANT /KEPT   4 /KEPT * BUFFER: KEPT
: ENTRY ( n -- a )   3 AND /KEPT * KEPT + ;          \ the place of the message number n
: TOPIC@ ( n -- a u )   ENTRY CELL+ COUNT ;
: TEXT@ ( n -- a u )   ENTRY CELL+ 64 + DUP CELL+ SWAP @ ;
: RETAINED? ( n -- f )   ENTRY @ ;
: LAST ( -- n )   #PUB @ 1- ;
: MQTT-ID ( a u -- )   M-ID PLACE ;
: MQTT-BROKER ( a u port -- )   M-PORT !  M-HOST PLACE ;
: MQTT-BROKER@ ( -- a u port )   M-HOST COUNT  M-PORT @ ;
: MQTT-WILL ( ma mu ta tu -- )   W-TOPIC PLACE  W-TEXT PLACE ;
: MQTT-ON-MESSAGE ( xt -- )   ON-MSG ! ;
: MQTT-ROOM! ( a u -- )   ROOM-U ! DROP ;
: MQTT-SUBSCRIBE ( a u -- ior )   SUB PLACE 0 ;
: MQTT-START ( -- )   STARTED ON! ;
: MQTT-STOP ( -- )   STARTED OFF!  READY OFF! ;
: MQTT-READY? ( -- f )   READY @ ;
: KEEP ( pa pu ta tu retain -- ior )
  READY @ 0= IF  DROP 2DROP 2DROP -464 EXIT  THEN
  #PUB @ ENTRY >R  R@ !  R@ CELL+ PLACE
  DUP R@ CELL+ 64 + !  R> CELL+ 64 + CELL+ SWAP MOVE  1 #PUB +!  0 ;
: MQTT-PUBLISH ( pa pu ta tu -- ior )   FALSE KEEP ;
: MQTT-RETAIN ( pa pu ta tu -- ior )   TRUE KEEP ;
: DELIVER ( pa pu ta tu -- )   ON-MSG @ EXECUTE ;   \ a message from the broker
END-PACKAGE

PACKAGE IR-QUEUE
PUBLIC
1608 CONSTANT IR-Q-SLOT
VARIABLE Q-ON   VARIABLE Q-RING   VARIABLE Q-OUT   VARIABLE Q-IN
VARIABLE FRAME-A   VARIABLE FRAME-N   VARIABLE FRAME-ERROR   \ a frame that waits; 0: none
VARIABLE SEND-CODE   VARIABLE RESULT-CODE                      \ what IR-Q-SEND and IR-Q-RESULT give
400 CELLS BUFFER: SENT-TIMES   VARIABLE SENT-N   VARIABLE #SENDS
: IR-Q-START ( a u out in -- )   Q-IN !  Q-OUT !  Q-RING !  DROP  Q-ON ON! ;
: IR-Q-STOP ( -- )   Q-ON OFF! ;
: IR-Q-FRAME? ( -- a n end-us true | false )
  FRAME-ERROR @ ?DUP IF  0 FRAME-ERROR !  THROW  THEN
  FRAME-N @ 0= IF FALSE EXIT THEN  FRAME-A @ FRAME-N @ 0  0 FRAME-N !  TRUE ;
: IR-Q-SEND ( a n -- ior )
  SEND-CODE @ ?DUP IF  NIP NIP EXIT  THEN
  DUP SENT-N !  CELLS SENT-TIMES SWAP MOVE  1 #SENDS +!  1 RESULT-CODE !  0 ;
: IR-Q-RESULT ( -- ior | 1 )   RESULT-CODE @ ;
: HEAR ( a n -- )   FRAME-N !  FRAME-A ! ;      \ the frame of n times at a comes
END-PACKAGE

