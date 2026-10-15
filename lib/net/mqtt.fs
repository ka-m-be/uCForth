\ mqtt.fs - package MQTT: a client of MQTT 3.1.1 over the words of NET
\
\ MQTT carries short messages between programs through a broker (a server
\ such as Mosquitto). A client sends a message to a topic, a name such as
\ home/kitchen/temperature; the broker gives it to each client that
\ subscribed to the topic. This client speaks MQTT 3.1.1 (OASIS, 2014)
\ over one TCP connection: QoS 0 in both ways, a clean session, a last
\ will with retain, keep alive with PINGREQ. It never waits for the
\ broker: MQTT-STEP does one step (connect, read, hand on, ping), in the
\ chain of IDLE (MQTT-START puts it there) or in a loop of the program.
\ When the connection breaks, it connects again after 1 s, then 2, 4 ...
\ at most 32 s, until MQTT-STOP, and it subscribes again.
\
\   a u MQTT-ROOM! ( -- )  the room of the program, at least 1536 bytes: a
\                         third for a packet out, a third for a packet in,
\                         a third for the TCP connection (half each way)
\   a u port MQTT-BROKER ( -- )   the broker: its IP address as text (or a
\                         name, with the package DNS) and its port (1883)
\   MQTT-BROKER@ ( -- a u port )   the broker that MQTT-BROKER set; u 0: none
\   a u MQTT-ID ( -- )    the client identifier, at most 64 bytes; each broker
\                         takes 1 to 23 of 0-9 a-z A-Z. None: the broker gives one
\   ma mu ta tu MQTT-WILL ( -- )   the last will: when the connection breaks,
\                         the broker sends the text ma mu to the topic ta tu,
\                         retained. 0 0 0 0 MQTT-WILL: none
\   MQTT-KEEPALIVE ( -- a )   a variable: the keep alive in seconds (60); 0: none
\   xt MQTT-ON-MESSAGE ( -- )   xt ( pa pu ta tu -- ) gets each message: its
\                         text and its topic, valid only while xt runs
\   MQTT-START ( -- )     connect now, and stay connected
\   MQTT-STOP ( -- )      DISCONNECT (the broker drops the will), then close
\   MQTT-STEP ( -- )      one step; it does not wait
\   MQTT-READY? ( -- f )  connected: the broker accepted
\   pa pu ta tu MQTT-PUBLISH ( -- ior )   the text pa pu to the topic ta tu
\   pa pu ta tu MQTT-RETAIN ( -- ior )    the same; the broker keeps it for
\                         the clients that subscribe later
\   a u MQTT-SUBSCRIBE ( -- ior )   the messages of the filter a u (+ is one
\                         level, # the rest); also after each new connection
\   .MQTT ( -- )          the state, the counters, the code of the last failure
\
\ The topic comes last, on top of the stack: a program sets it just before
\ it sends, and computes the text before (Kai):
\   S" 22.5" S" home/kitchen/temperature" MQTT-PUBLISH THROW
\ (in a definition: at the prompt the second S" overwrites the first one).
\
\ The codes: -460 the broker refused (.MQTT shows its code: 2 the
\ identifier, 5 not authorized ...); -461 no CONNACK in time (NET-TIMEOUT);
\ -462 a packet, topic or text too big for its buffer, or a fifth filter;
\ -463 against the protocol: an empty topic, or a packet from the broker
\ that must not come; -464 not connected; -465 no PINGRESP in time; -466 the
\ broker refused a filter. The other codes come from NET (-301 no broker at
\ the port, -304 the connection ended ...). MQTT-PUBLISH and MQTT-SUBSCRIBE
\ give their code; MQTT-STEP keeps it for .MQTT and connects again later.
\ An error of the word of MQTT-ON-MESSAGE is counted (faults) and kept.
\
\ The states in .MQTT: 0 off, 1 wait for the next attempt, 2 the TCP
\ connection comes, 3 wait for CONNACK, 4 ready.
\
\ Limits: one connection; a packet at most a third of the room less 8
\ bytes in each way (with 12312 bytes: 4096; a bigger one from the broker is
\ read, dropped and counted); a filter or the will at
\ most 128 bytes, 4 filters; no TLS, no QoS 1 or 2, no session that stays.
\ A step reads at most 256 bytes. MQTT-PUBLISH waits while TCP takes no
\ more bytes, at most NET-TIMEOUT. As for NET: words of level 2 only (the
\ console, the chain of IDLE), not in a handler or a timer.
\
\ Load it after NET, BYTES and IDLE; on a host, in the folder ucf:
\   ALSO FILES  INCLUDE lib/net/host.fs  INCLUDE lib/net/bytes.fs
\   INCLUDE lib/idle.fs  INCLUDE lib/net/mqtt.fs
\ Example, with a broker on the same computer (lib/net/README.md, "MQTT"):
\   ALSO MQTT  : SHOW ( pa pu ta tu -- )  CR TYPE ." : " TYPE CR ;
\   6144 BUFFER: ROOM  ROOM 6144 MQTT-ROOM!      \ packets of at most 2040 bytes
\   ' SHOW MQTT-ON-MESSAGE  S" 127.0.0.1" 1883 MQTT-BROKER  MQTT-START
\   S" ucf/#" MQTT-SUBSCRIBE .                     \ 0
\   : HELLO ( -- ior )  S" hello" S" ucf/test" MQTT-PUBLISH ;
\   HELLO .                                        \ 0, then: ucf/test: hello
\ Tests: lib/net/test/mqtt.fs (with a broker in Forth), lib/net/test/mqttnet.fs
\ (with Mosquitto).

PACKAGE MQTT  ALSO NET  ALSO INET  ALSO TEXT  ALSO BYTES  ALSO IDLE   \ lib/package.fs

PUBLIC
-460 CONSTANT MQTT-REFUSED       \ the broker refused the connection
-461 CONSTANT MQTT-NO-ANSWER     \ no CONNACK in time
-462 CONSTANT MQTT-TOO-BIG       \ a packet, topic or text too big; a fifth filter
-463 CONSTANT MQTT-PROTOCOL      \ against the protocol
-464 CONSTANT MQTT-NOT-READY     \ not connected
-465 CONSTANT MQTT-NO-PING       \ no PINGRESP in time
-466 CONSTANT MQTT-SUB-FAILED    \ the broker refused a filter
VARIABLE MQTT-KEEPALIVE   60 MQTT-KEEPALIVE !

PRIVATE
128 CONSTANT MAX-TOPIC            \ a filter, and each part of the will, at most
64 CONSTANT MAX-NAME              \ the host and the client identifier, at most
4 CONSTANT #FILTERS
0 CONSTANT S-OFF                  \ the states of MQ-STATE
1 CONSTANT S-WAIT
2 CONSTANT S-TCP
3 CONSTANT S-CONNACK
4 CONSTANT S-READY

\ ---- the settings
MAX-NAME BUFFER: BROKER-HOST   VARIABLE BROKER-N   VARIABLE BROKER-PORT
MAX-NAME BUFFER: CLIENT-ID     VARIABLE CLIENT-N
MAX-TOPIC BUFFER: WILL-TOPIC   VARIABLE WILL-TN
MAX-TOPIC BUFFER: WILL-TEXT    VARIABLE WILL-MN
#FILTERS MAX-TOPIC * BUFFER: FILTERS
VARIABLE ROOM   VARIABLE PART     \ the room of the program (0: none), a third of it
: TX ( -- a )   ROOM @ ;                    \ a packet out: the body after 5 bytes of room
: RX ( -- a )   ROOM @ PART @ + ;           \ a packet in
: MAX-PACKET ( -- u )   PART @ 8 - ;        \ the remaining length of a packet, at most
#FILTERS CELLS BUFFER: FILTER-N   FILTER-N #FILTERS CELLS 0 FILL   \ the length of each; 0: free
VARIABLE ON-MESSAGE
: IGNORE ( pa pu ta tu -- )   2DROP 2DROP ;
' IGNORE ON-MESSAGE !

\ ---- the state and the counters
VARIABLE MQ-STATE
VARIABLE CONN                     \ the connection to the broker; 0: none
VARIABLE DUE                      \ MILLIS: the next attempt, or the end of a wait
VARIABLE DELAY-MS                 \ the pause before the next attempt
VARIABLE SENT-AT                  \ MILLIS: the last packet out
VARIABLE PINGING   VARIABLE PING-AT   \ a PINGREQ waits for its PINGRESP, since PING-AT
VARIABLE LAST-CODE   VARIABLE CONNACK-CODE
VARIABLE ATTEMPTS   VARIABLE CONNECTS   VARIABLE SENT   VARIABLE RECEIVED
VARIABLE DROPPED    VARIABLE PINGS      VARIABLE FAULTS
VARIABLE PACKET-ID

: DUE? ( -- f )   MILLIS DUE @ - 0< 0= ;
: LATER ( ms -- )   MILLIS + DUE ! ;
: KEEP ( a u buf max -- u )   \ copy the text a u to buf, at most max bytes
  >R OVER R> > IF MQTT-TOO-BIG THROW THEN  SWAP DUP >R MOVE R> ;

\ ---- packets out: the body after 5 bytes of room, then the fixed header before it
VARIABLE TX-END   \ the next free byte of the body
: BODY ( -- )   TX 5 + TX-END ! ;
: +BYTE ( c -- )   TX-END @ C!  1 TX-END +! ;
: +U16 ( u -- )   TX-END @ BE16!  2 TX-END +! ;
: +BYTES ( a u -- )   TX-END @ SWAP  DUP TX-END +!  MOVE ;
: +STR ( a u -- )   DUP +U16 +BYTES ;   \ a string of MQTT: its length in 2 bytes, then its bytes
: #LEN ( n -- k )   \ the number of bytes of the remaining length n
  1 BEGIN  OVER 127 >  WHILE  SWAP 7 RSHIFT SWAP 1+  REPEAT  NIP ;
: +LEN ( n p -- p' )   \ the remaining length n at p: 7 bits in each byte, the low bits first
  BEGIN
    >R  DUP 127 AND  OVER 127 > IF 128 OR THEN  R@ C!  R> 1+
    SWAP 7 RSHIFT SWAP  OVER 0=
  UNTIL  NIP ;
: PACKET ( type -- a u )   \ the whole packet: the fixed header before the body
  TX-END @ TX 5 + -  DUP #LEN  TX 5 + SWAP - 1-        ( type n a )
  >R  SWAP R@ C!  R@ 1+ +LEN DROP  R> TX-END @ OVER - ;
: CONNECT-PACKET ( -- a u )
  BODY  S" MQTT" +STR  4 +BYTE                        \ the protocol and its level: 3.1.1
  2  WILL-TN @ IF $24 OR THEN  +BYTE                  \ a clean session; a will, retained
  MQTT-KEEPALIVE @ +U16  CLIENT-ID CLIENT-N @ +STR
  WILL-TN @ IF  WILL-TOPIC WILL-TN @ +STR  WILL-TEXT WILL-MN @ +STR  THEN
  $10 PACKET ;
: NEXT-ID ( -- u )   \ the next packet identifier: 1 to 65535
  PACKET-ID @ 1+ $FFFF AND  DUP 0= IF 1+ THEN  DUP PACKET-ID ! ;
: SUBSCRIBE-PACKET ( a u -- a2 u2 )   BODY  NEXT-ID +U16  +STR  0 +BYTE  $82 PACKET ;   \ QoS 0
: PUBLISH-PACKET ( pa pu ta tu type -- a u )   >R  BODY +STR +BYTES  R> PACKET ;
: PING-PACKET ( -- a u )   BODY $C0 PACKET ;
: DISCONNECT-PACKET ( -- a u )   BODY $E0 PACKET ;
: SEND ( a u -- ior )   CONN @ SOCK-WRITE  DUP 0= IF MILLIS SENT-AT ! THEN ;

\ ---- the end of a connection
VARIABLE PHASE   \ the parser: 0 a type byte comes, 1 the remaining length, 2 the body, 3 a body to drop
: CLOSE-CONN ( -- )   CONN @ ?DUP IF SOCK-CLOSE DROP THEN  0 CONN ! ;
: FAILED ( ior -- )   \ the connection ends; the next attempt after the pause, which then doubles
  LAST-CODE !  CLOSE-CONN  0 PHASE !  PINGING OFF!
  S-WAIT MQ-STATE !  DELAY-MS @ LATER  DELAY-MS @ 2* 32000 MIN DELAY-MS ! ;

\ ---- the filters, kept for each new connection
: FILTER ( i -- a u )   DUP MAX-TOPIC * FILTERS +  SWAP CELLS FILTER-N + @ ;
: KNOWN? ( a u -- f )   \ the filter a u is kept already
  #FILTERS 0 DO  2DUP I FILTER STR= IF  2DROP TRUE UNLOOP EXIT  THEN  LOOP  2DROP FALSE ;
: REMEMBER ( a u -- ior )   \ keep the filter a u in a free place
  #FILTERS 0 DO
    I CELLS FILTER-N + @ 0= IF
      I MAX-TOPIC * FILTERS + MAX-TOPIC KEEP  I CELLS FILTER-N + !  0 UNLOOP EXIT
    THEN
  LOOP  2DROP MQTT-TOO-BIG ;
: RESUBSCRIBE ( -- )   \ each kept filter again, after CONNACK
  #FILTERS 0 DO  I FILTER ?DUP IF SUBSCRIBE-PACKET SEND THROW ELSE DROP THEN  LOOP ;

\ ---- packets in: the parser takes the bytes in pieces of any size
256 BUFFER: CHUNK
VARIABLE RX-TYPE   VARIABLE RX-LEN   VARIABLE RX-SHIFT   VARIABLE RX-N
: CONNACKED ( -- )   \ the answer to CONNECT
  MQ-STATE @ S-CONNACK <>  RX-LEN @ 2 <>  OR IF MQTT-PROTOCOL THROW THEN
  RX 1+ C@  DUP CONNACK-CODE !  IF MQTT-REFUSED THROW THEN
  S-READY MQ-STATE !  1000 DELAY-MS !  0 LAST-CODE !  1 CONNECTS +!  RESUBSCRIBE ;
: DELIVER ( -- )   \ a PUBLISH: its text and its topic to the program
  MQ-STATE @ S-READY <>  RX-TYPE @ 6 AND  OR IF MQTT-PROTOCOL THROW THEN   \ QoS 1 or 2: never asked for
  RX BE16@  DUP 2 + RX-LEN @ > IF MQTT-PROTOCOL THROW THEN
  >R  RX 2 + R@ +  RX-LEN @ 2 - R@ -  RX 2 + R>          ( pa pu ta tu )
  1 RECEIVED +!
  ON-MESSAGE @ CATCH ?DUP IF  >R 2DROP 2DROP R> LAST-CODE !  1 FAULTS +!  THEN ;
: SUBACKED ( -- )   \ a return code $80: the broker refused a filter
  RX-LEN @ 2 ?DO  RX I + C@ $80 = IF  MQTT-SUB-FAILED LAST-CODE !  THEN  LOOP ;
: DISPATCH ( -- )   \ a whole packet in RX
  RX-TYPE @ $F0 AND CASE
    $20 OF  CONNACKED  ENDOF
    $30 OF  DELIVER  ENDOF
    $90 OF  SUBACKED  ENDOF
    $D0 OF  PINGING OFF!  ENDOF
    MQTT-PROTOCOL THROW                                   \ any other packet
  ENDCASE ;
: COMPLETE? ( -- )   \ the body is there: hand the packet on, or count the one that was too big
  RX-N @ RX-LEN @ < IF EXIT THEN
  PHASE @  0 PHASE !  2 = IF DISPATCH EXIT THEN
  1 DROPPED +!  MQTT-TOO-BIG LAST-CODE ! ;
: LENGTH-BYTE ( c -- )   \ a byte of the remaining length; after the last one the body comes
  DUP 127 AND RX-SHIFT @ LSHIFT RX-LEN +!  7 RX-SHIFT +!
  128 AND IF  RX-SHIFT @ 28 < 0= IF MQTT-PROTOCOL THROW THEN  EXIT  THEN
  0 RX-N !  RX-LEN @ MAX-PACKET > IF 3 ELSE 2 THEN PHASE !  COMPLETE? ;
: TAKE ( a u -- a2 u2 )   \ bytes of the body into RX
  RX-LEN @ RX-N @ -  OVER MIN  >R  OVER RX RX-N @ + R@ MOVE  R@ RX-N +!  R> /STRING ;
: DROP-BYTES ( a u -- a2 u2 )   \ bytes of a body that is too big
  RX-LEN @ RX-N @ -  OVER MIN  DUP RX-N +!  /STRING ;
: LIVE? ( -- f )   MQ-STATE @ S-CONNACK < 0= ;
: FEED ( a u -- )   \ bytes from the broker
  BEGIN  DUP 0<>  LIVE? AND  WHILE
    PHASE @ CASE
      0 OF  OVER C@ RX-TYPE !  0 RX-LEN !  0 RX-SHIFT !  1 PHASE !  1/STRING  ENDOF
      1 OF  OVER C@ >R  1/STRING  R> LENGTH-BYTE  ENDOF
      2 OF  TAKE COMPLETE?  ENDOF
      3 OF  DROP-BYTES COMPLETE?  ENDOF
    ENDCASE
  REPEAT  2DROP ;
: RECEIVE ( -- )   \ at most 256 bytes that wait; the end of the connection is an error
  CONN @ SOCK-AVAILABLE ?DUP 0= IF  CONN @ SOCK-OPEN? IF EXIT THEN  NET-CLOSED THROW  THEN
  256 MIN  CHUNK SWAP CONN @ SOCK-READ THROW  CHUNK SWAP FEED ;

\ ---- keep alive
: HALF ( -- ms )   MQTT-KEEPALIVE @ 500 * ;   \ half of the keep alive
: SINCE ( t -- ms )   MILLIS SWAP - ;
: KEEP-ALIVE ( -- )   \ PINGREQ after half of the keep alive without a packet out;
  HALF 0= IF EXIT THEN                       \ no PINGRESP within another half: the end
  PINGING @ IF  PING-AT @ SINCE HALF < 0= IF MQTT-NO-PING THROW THEN  EXIT  THEN
  SENT-AT @ SINCE HALF < IF EXIT THEN
  PING-PACKET SEND THROW  MILLIS PING-AT !  PINGING ON!  1 PINGS +! ;

\ ---- the steps
: OPEN-TCP ( -- )   \ the next attempt: the TCP connection begins
  1 ATTEMPTS +!  RX PART @ +  PART @  BROKER-HOST BROKER-N @ BROKER-PORT @ TCP-CONNECT   ( sock ior )
  ?DUP IF NIP THROW THEN  CONN !  S-TCP MQ-STATE !  NET-TIMEOUT @ LATER ;
: CONNECTING ( -- )   \ when the TCP connection stands: CONNECT
  CONN @ SOCK-STATE  DUP 0< IF THROW THEN
  IF  CONNECT-PACKET SEND THROW  S-CONNACK MQ-STATE !  NET-TIMEOUT @ LATER  EXIT  THEN
  DUE? IF NET-TIMED-OUT THROW THEN ;
: (STEP) ( -- )
  MQ-STATE @ CASE
    S-WAIT    OF  DUE? IF OPEN-TCP THEN  ENDOF
    S-TCP     OF  CONNECTING  ENDOF
    S-CONNACK OF  RECEIVE  MQ-STATE @ S-CONNACK =  DUE? AND IF MQTT-NO-ANSWER THROW THEN  ENDOF
    S-READY   OF  RECEIVE  MQ-STATE @ S-READY = IF KEEP-ALIVE THEN  ENDOF
  ENDCASE ;

\ ---- publish
VARIABLE PUB-TYPE   \ $30, or $31 with retain
: REFUSE ( pa pu ta tu ior -- ior )   >R 2DROP 2DROP R> ;
: FITS? ( pa pu ta tu -- pa pu ta tu f )   >R OVER R@ + 2 + MAX-PACKET > 0= R> SWAP ;
: PUBLISH ( pa pu ta tu -- ior )
  MQ-STATE @ S-READY <> IF  MQTT-NOT-READY REFUSE EXIT  THEN
  DUP 0= IF  MQTT-PROTOCOL REFUSE EXIT  THEN
  FITS? 0= IF  MQTT-TOO-BIG REFUSE EXIT  THEN
  PUB-TYPE @ PUBLISH-PACKET SEND  DUP IF  DUP FAILED EXIT  THEN  1 SENT +! ;

PUBLIC
: MQTT-STEP ( -- )   \ one step: connect, read, hand on, ping; it does not wait
  MQ-STATE @ S-OFF = IF EXIT THEN
  ['] (STEP) CATCH ?DUP 0= IF EXIT THEN
  DUP -28 = IF THROW THEN                          \ Ctrl-C, as at the console
  MQ-STATE @ S-OFF = IF DROP EXIT THEN             \ MQTT-STOP came meanwhile
  FAILED ;
: MQTT-START ( -- )   \ connect now, and stay connected until MQTT-STOP
  BROKER-N @ 0= ABORT" MQTT: no broker (MQTT-BROKER)"
  ROOM @ 0= ABORT" MQTT: no room (MQTT-ROOM!)"
  MQ-STATE @ S-OFF <> IF EXIT THEN
  0 LAST-CODE !  0 CONNACK-CODE !  0 ATTEMPTS !  0 CONNECTS !  0 SENT !  0 RECEIVED !
  0 DROPPED !  0 PINGS !  0 FAULTS !
  1000 DELAY-MS !  0 LATER  S-WAIT MQ-STATE !  ['] MQTT-STEP IDLE+ ;
: MQTT-STOP ( -- )   \ DISCONNECT, so the broker drops the will; then close
  ['] MQTT-STEP IDLE-
  MQ-STATE @ S-READY = IF  DISCONNECT-PACKET SEND DROP  THEN
  CLOSE-CONN  0 PHASE !  PINGING OFF!  S-OFF MQ-STATE ! ;
: MQTT-READY? ( -- f )   MQ-STATE @ S-READY = ;
: MQTT-PUBLISH ( pa pu ta tu -- ior )   $30 PUB-TYPE !  PUBLISH ;
: MQTT-RETAIN ( pa pu ta tu -- ior )    $31 PUB-TYPE !  PUBLISH ;
: MQTT-SUBSCRIBE ( a u -- ior )   \ now, if connected, and after each new connection
  DUP 0= IF  2DROP MQTT-PROTOCOL EXIT  THEN
  DUP MAX-TOPIC > IF  2DROP MQTT-TOO-BIG EXIT  THEN
  2DUP KNOWN? 0= IF  2DUP REMEMBER ?DUP IF  >R 2DROP R> EXIT  THEN  THEN
  MQ-STATE @ S-READY <> IF  2DROP 0 EXIT  THEN
  SUBSCRIBE-PACKET SEND  DUP IF  DUP FAILED  THEN ;
: MQTT-BROKER ( a u port -- )   BROKER-PORT !  BROKER-HOST MAX-NAME KEEP BROKER-N ! ;
: MQTT-BROKER@ ( -- a u port )   BROKER-HOST BROKER-N @  BROKER-PORT @ ;
: MQTT-ROOM! ( a u -- )   \ the room for the packets and the connection; MQTT-STOP first
  MQ-STATE @ S-OFF <> ABORT" MQTT: MQTT-STOP before MQTT-ROOM!"
  OVER 0=  OVER 1536 < OR ABORT" MQTT: the room needs at least 1536 bytes"
  3 /  CELL NEGATE AND  PART !  ROOM ! ;
: MQTT-ID ( a u -- )   CLIENT-ID MAX-NAME KEEP CLIENT-N ! ;
: MQTT-WILL ( ma mu ta tu -- )
  WILL-TOPIC MAX-TOPIC KEEP WILL-TN !  WILL-TEXT MAX-TOPIC KEEP WILL-MN ! ;
: MQTT-ON-MESSAGE ( xt -- )   ON-MESSAGE ! ;
: .MQTT ( -- )
  ." MQTT state/attempts/connects/code/connack: "
  MQ-STATE @ . ATTEMPTS @ . CONNECTS @ . LAST-CODE @ . CONNACK-CODE @ . CR
  ." MQTT sent/received/dropped/pings/faults: "
  SENT @ . RECEIVED @ . DROPPED @ . PINGS @ . FAULTS @ . CR ;

END-PACKAGE
