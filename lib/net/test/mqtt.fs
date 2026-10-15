\ mqtt.fs - the tests of the package MQTT (lib/net/mqtt.fs), without a broker
\
\ Part 1, without a connection: the packets that the client builds, byte
\ for byte after MQTT 3.1.1 (the remaining length, CONNECT with and without
\ a will, PUBLISH, SUBSCRIBE, PINGREQ, DISCONNECT), and its parser with
\ packets from the broker: whole, in pieces of one byte, two in one piece,
\ too big, against the protocol.
\ Part 2, a session over the network layer that is there (on each system
\ our TCP/IP over NO-LINK; after lib/net/host.fs the sockets of the
\ system), at 127.0.0.1, port 47021, with a small broker in Forth (the
\ peer): connect with a will, subscribe, messages in both ways, ping, a
\ lost PINGRESP, the broker closes, refuses, does not answer, no broker at
\ the port; connect again, filters, stop. The program has one thread:
\ the tests run MQTT-STEP and a step of the peer in turn, and they set the
\ clocks of MQTT (white box) instead of waiting.
\ Each system with files and 32-bit cells; in the folder ucf:
\   build/ucf-c -32 host/files.fs lib/net/test/mqtt.fs </dev/null
\ At the end: "mqtt: 68 tests, 0 errors". With the broker Mosquitto:
\ lib/net/test/mqttnet.fs.

ONLY FORTH DEFINITIONS  ALSO FILES
[DEFINED] T{ S" forth/test/tester.fs" ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] NET    S" lib/net/nolink.fs"  ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] NET    S" lib/net/tcpip.fs"   ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] IDLE   S" lib/idle.fs"        ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] BYTES  S" lib/net/bytes.fs"   ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] MQTT   S" lib/net/mqtt.fs"    ?INCLUDED
ONLY FORTH DEFINITIONS  ALSO INTERNALS  ALSO TEXT  ALSO INET  ALSO NET  ALSO BYTES
ALSO MQTT  ALSO MQTT.I  ALSO FILES
DECIMAL

'KEY? @ CONSTANT KEY0              \ MQTT-START sets it (IDLE+); the end of the file puts it back
12312 BUFFER: MQ-ROOM   MQ-ROOM 12312 MQTT-ROOM!   \ packets of at most 4096 bytes

\ texts: in definitions, for the second S" of a line at the prompt overwrites the first one
: HI ( -- a u )   S" hi" ;
: A/B ( -- a u )   S" a/b" ;
: A/# ( -- a u )   S" a/#" ;
: UCF ( -- a u )   S" ucf" ;
: WILL$ ( -- a u )   S" false" ;
: T/O ( -- a u )   S" t/o" ;
: HELLO ( -- a u )   S" hello" ;
: X/Y ( -- a u )   S" x/y" ;
: LOOPBACK ( -- a u )   S" 127.0.0.1" ;
CREATE BIG 5000 ALLOT
: PATTERN ( -- )   5000 0 DO  I 251 MOD BIG I + C!  LOOP ;
PATTERN

\ ---- the room of the program: a third each for a packet out, a packet in, the TCP connection
: NO-ROOM ( -- )   0 1536 MQTT-ROOM! ;
: SMALL-ROOM ( -- )   MQ-ROOM 1535 MQTT-ROOM! ;
: FULL-ROOM ( -- )   MQ-ROOM 12312 MQTT-ROOM! ;
T{ MAX-PACKET  MQ-ROOM 1536 MQTT-ROOM!  MAX-PACKET  FULL-ROOM -> 4096 504 }T
T{ ' NO-ROOM CATCH  ' SMALL-ROOM CATCH  MAX-PACKET  RX TX - -> -2 -2 4096 4104 }T   \ the room stays

\ ---- part 1: the remaining length (MQTT 3.1.1, section 2.2.3)
CREATE LB 8 ALLOT
: LEN? ( n a u -- f )   \ the remaining length n gives the bytes a u
  >R >R  LB +LEN  LB TUCK -  R> R> STR= ;
CREATE L0      0 C,
CREATE L127    $7F C,
CREATE L128    $80 C, $01 C,
CREATE L321    $C1 C, $02 C,                       \ the example of the standard
CREATE L16383  $FF C, $7F C,
CREATE L16384  $80 C, $80 C, $01 C,
CREATE LMAX    $FF C, $FF C, $FF C, $7F C,          \ 268 435 455, the largest
T{ 0 L0 1 LEN?  127 L127 1 LEN?  128 L128 2 LEN? -> TRUE TRUE TRUE }T
T{ 321 L321 2 LEN?  16383 L16383 2 LEN?  16384 L16384 3 LEN? -> TRUE TRUE TRUE }T
T{ 268435455 LMAX 4 LEN? -> TRUE }T
T{ 127 #LEN  128 #LEN  16384 #LEN  268435455 #LEN -> 1 2 3 4 }T

\ ---- part 1: the packets out
CREATE CONNECT1   $10 C, 15 C,  0 C, 4 C, 'M' C, 'Q' C, 'T' C, 'T' C,  4 C,  2 C,  0 C, 60 C,
                  0 C, 3 C, 'u' C, 'c' C, 'f' C,
HERE CONNECT1 - CONSTANT #CONNECT1
CREATE CONNECT2   $10 C, 27 C,  0 C, 4 C, 'M' C, 'Q' C, 'T' C, 'T' C,  4 C,  $26 C,  0 C, 60 C,
                  0 C, 3 C, 'u' C, 'c' C, 'f' C,  0 C, 3 C, 't' C, '/' C, 'o' C,
                  0 C, 5 C, 'f' C, 'a' C, 'l' C, 's' C, 'e' C,
HERE CONNECT2 - CONSTANT #CONNECT2
UCF MQTT-ID  0 0 0 0 MQTT-WILL  60 MQTT-KEEPALIVE !
T{ CONNECT-PACKET CONNECT1 #CONNECT1 STR= -> TRUE }T          \ a clean session, no will
WILL$ T/O MQTT-WILL
T{ CONNECT-PACKET CONNECT2 #CONNECT2 STR= -> TRUE }T          \ the will, retained: flags $26
CREATE PUB1   $30 C, 7 C,  0 C, 3 C, 'a' C, '/' C, 'b' C,  'h' C, 'i' C,
HERE PUB1 - CONSTANT #PUB1
T{ HI A/B $30 PUBLISH-PACKET PUB1 #PUB1 STR= -> TRUE }T
T{ HI A/B $31 PUBLISH-PACKET  DROP C@ -> $31 }T                 \ with retain
CREATE PUB200   $30 C, $CD C, $01 C,                           \ 205 bytes after the header
T{ BIG 200 A/B $30 PUBLISH-PACKET  SWAP 3 PUB200 3 STR= -> 208 TRUE }T
CREATE SUB1   $82 C, 8 C,  0 C, 1 C,  0 C, 3 C, 'a' C, '/' C, '#' C,  0 C,
HERE SUB1 - CONSTANT #SUB1
0 PACKET-ID !
T{ A/# SUBSCRIBE-PACKET SUB1 #SUB1 STR= -> TRUE }T
T{ A/# SUBSCRIBE-PACKET  DROP 2 + BE16@ -> 2 }T                 \ the next number
T{ $FFFF PACKET-ID !  NEXT-ID -> 1 }T                           \ never 0
CREATE PINGREQ  $C0 C, 0 C,
CREATE DISC     $E0 C, 0 C,
T{ PING-PACKET PINGREQ 2 STR=  DISCONNECT-PACKET DISC 2 STR= -> TRUE TRUE }T
: WILL? ( ma mu ta tu -- n )   ['] MQTT-WILL CATCH  DUP IF  >R 2DROP 2DROP R>  THEN ;
T{ BIG 65 ' MQTT-ID CATCH NIP NIP -> MQTT-TOO-BIG }T             \ the settings have limits
T{ BIG 129 A/B WILL? -> MQTT-TOO-BIG }T

\ ---- part 1: the parser
128 BUFFER: GOT-TOPIC   VARIABLE GOT-TN   2000 BUFFER: GOT-TEXT   VARIABLE GOT-MN   VARIABLE #GOT
: RECORD ( pa pu ta tu -- )   \ the handler of the tests: it keeps the last message
  DUP GOT-TN !  GOT-TOPIC SWAP MOVE  DUP GOT-MN !  GOT-TEXT SWAP MOVE  1 #GOT +! ;
: GOT? ( pa pu ta tu -- f )   \ the last message had this text and topic
  GOT-TOPIC GOT-TN @ STR=  >R  GOT-TEXT GOT-MN @ STR=  R> AND ;
' RECORD MQTT-ON-MESSAGE
: AS-READY ( -- )   \ the parser as after CONNACK, without a connection
  S-READY MQ-STATE !  0 PHASE !  0 CONN !  0 #GOT !  0 LAST-CODE !  PINGING OFF!  0 DROPPED ! ;
: AS-CONNACK ( -- )   AS-READY  S-CONNACK MQ-STATE !  0 CONNECTS !  4000 DELAY-MS ! ;
: FEED? ( a u -- n )   ['] FEED CATCH  DUP IF NIP NIP THEN ;
: BYTEWISE ( a u -- )   0 ?DO  DUP I + 1 FEED  LOOP  DROP ;
AS-READY
T{ PUB1 #PUB1 FEED  #GOT @  HI A/B GOT? -> 1 TRUE }T
AS-READY
T{ PUB1 #PUB1 BYTEWISE  #GOT @  HI A/B GOT? -> 1 TRUE }T          \ one byte at a time
CREATE TWO   $30 C, 7 C,  0 C, 3 C, 'a' C, '/' C, 'b' C,  'h' C, 'i' C,  $D0 C, 0 C,
AS-READY  PINGING ON!
T{ TWO 11 FEED  #GOT @  PINGING @ -> 1 FALSE }T                  \ PUBLISH and PINGRESP in one piece
AS-READY
T{ BIG 200 A/B $30 PUBLISH-PACKET FEED  #GOT @  BIG 200 GOT-TEXT GOT-MN @ STR= -> 1 TRUE }T
CREATE HUGE   $30 C, $88 C, $27 C,                             \ the remaining length 5000
AS-READY
T{ HUGE 3 FEED  BIG 5000 FEED  #GOT @  DROPPED @  LAST-CODE @ -> 0 1 MQTT-TOO-BIG }T
T{ PUB1 #PUB1 FEED  #GOT @ -> 1 }T                              \ the next packet comes as before
CREATE BAD-LEN  $30 C, $FF C, $FF C, $FF C, $FF C, $01 C,         \ 5 bytes of length
CREATE PUBACK   $40 C, 2 C, 0 C, 1 C,
CREATE PUB-QOS1 $32 C, 9 C,  0 C, 3 C, 'a' C, '/' C, 'b' C,  0 C, 1 C,  'h' C, 'i' C,
CREATE PUB-BAD  $30 C, 3 C,  0 C, 5 C, 'a' C,                    \ the topic is longer than the packet
CREATE CONNACK0 $20 C, 2 C, 0 C, 0 C,
CREATE CONNACK5 $20 C, 2 C, 0 C, 5 C,
AS-READY  T{ BAD-LEN 6 FEED? -> MQTT-PROTOCOL }T
AS-READY  T{ PUBACK 4 FEED? -> MQTT-PROTOCOL }T
AS-READY  T{ PUB-QOS1 11 FEED? -> MQTT-PROTOCOL }T
AS-READY  T{ PUB-BAD 5 FEED? -> MQTT-PROTOCOL }T
AS-READY  T{ CONNACK0 4 FEED? -> MQTT-PROTOCOL }T                \ CONNACK only once, at the start
AS-CONNACK
T{ CONNACK0 4 FEED  MQ-STATE @  CONNECTS @  DELAY-MS @ -> S-READY 1 1000 }T
AS-CONNACK
T{ CONNACK5 4 FEED?  CONNACK-CODE @ -> MQTT-REFUSED 5 }T
CREATE SUBACK0  $90 C, 3 C, 0 C, 1 C, 0 C,
CREATE SUBACK80 $90 C, 3 C, 0 C, 1 C, $80 C,
AS-READY  T{ SUBACK0 5 FEED  LAST-CODE @ -> 0 }T
AS-READY  T{ SUBACK80 5 FEED  LAST-CODE @ -> MQTT-SUB-FAILED }T
: BOOM ( pa pu ta tu -- )   2DROP 2DROP -77 THROW ;
AS-READY  ' BOOM MQTT-ON-MESSAGE  0 FAULTS !
T{ PUB1 #PUB1 FEED  FAULTS @  LAST-CODE @  MQ-STATE @ -> 1 -77 S-READY }T
' RECORD MQTT-ON-MESSAGE
T{ HI 0 0 MQTT-PUBLISH -> MQTT-PROTOCOL }T                       \ an empty topic
T{ BIG 4092 A/B MQTT-PUBLISH -> MQTT-TOO-BIG }T                 \ 2 + 3 + 4092 > 4096
T{ 0 0 MQTT-SUBSCRIBE  BIG 129 MQTT-SUBSCRIBE -> MQTT-PROTOCOL MQTT-TOO-BIG }T
S-OFF MQ-STATE !  0 PHASE !  PINGING OFF!  0 LAST-CODE !
T{ HI A/B MQTT-PUBLISH -> MQTT-NOT-READY }T

\ ---- part 2: the peer, a small broker in Forth
47021 CONSTANT MQ-PORT
VARIABLE LISTENER   VARIABLE PEER              \ the server, and its end of the connection; 0: none
2048 BUFFER: PIN   VARIABLE PIN-N               \ bytes from the client that the peer has not used yet
VARIABLE ANSWER     \ the code in CONNACK; -1: the peer sends no CONNACK
VARIABLE PONG       \ true: the peer answers PINGREQ
VARIABLE SUB-CODE   \ the code in SUBACK
VARIABLE #CONNECT   VARIABLE #SUBSCRIBE   VARIABLE #PINGREQ   VARIABLE #PUBLISH   VARIABLE #DISCONNECT
256 BUFFER: SEEN-CONNECT   VARIABLE SEEN-CONNECT-N     \ the body of the last CONNECT
256 BUFFER: SEEN-SUB       VARIABLE SEEN-SUB-N          \ of the last SUBSCRIBE
2048 BUFFER: SEEN-PUB      VARIABLE SEEN-PUB-N   VARIABLE SEEN-PUB-TYPE   \ of the last PUBLISH, its first byte
VARIABLE P-LEN   VARIABLE P-HEAD   \ the packet at the start of PIN: its remaining length, its header bytes
CREATE P-OUT 8 ALLOT
: P-WHOLE? ( -- f )   \ a whole packet at the start of PIN
  0 P-LEN !  1 P-HEAD !
  BEGIN
    P-HEAD @ PIN-N @ < 0= IF FALSE EXIT THEN
    PIN P-HEAD @ + C@  DUP 127 AND  P-HEAD @ 1- 7 * LSHIFT P-LEN +!  1 P-HEAD +!
    128 AND 0=
  UNTIL
  P-HEAD @ P-LEN @ + PIN-N @ > 0= ;
: P-BODY ( -- a u )   PIN P-HEAD @ +  P-LEN @ ;
: P-NEXT ( -- )   \ the packet out of PIN
  P-HEAD @ P-LEN @ +  DUP >R  PIN +  PIN  PIN-N @ R@ -  MOVE  R> NEGATE PIN-N +! ;
: P-SEND ( a u -- )   PEER @ SOCK-WRITE DROP ;
: KEEP-BODY ( buf var -- )   >R  P-BODY >R  SWAP R@ MOVE  R> R> ! ;
: P-CONNACK ( -- )
  ANSWER @ 0< IF EXIT THEN
  $20 P-OUT C!  2 P-OUT 1+ C!  0 P-OUT 2 + C!  ANSWER @ P-OUT 3 + C!  P-OUT 4 P-SEND ;
: P-SUBACK ( -- )   \ with the packet identifier of the SUBSCRIBE
  $90 P-OUT C!  3 P-OUT 1+ C!  P-BODY DROP BE16@ P-OUT 2 + BE16!  SUB-CODE @ P-OUT 4 + C!
  P-OUT 5 P-SEND ;
: P-ANSWER ( -- )   \ the packet at the start of PIN
  PIN C@ CASE
    $10 OF  1 #CONNECT +!  SEEN-CONNECT SEEN-CONNECT-N KEEP-BODY  P-CONNACK  ENDOF
    $82 OF  1 #SUBSCRIBE +!  SEEN-SUB SEEN-SUB-N KEEP-BODY  P-SUBACK  ENDOF
    $C0 OF  1 #PINGREQ +!  PONG @ IF  $D0 P-OUT C!  0 P-OUT 1+ C!  P-OUT 2 P-SEND  THEN  ENDOF
    $E0 OF  1 #DISCONNECT +!  ENDOF
    DUP $F0 AND $30 = IF  1 #PUBLISH +!  DUP SEEN-PUB-TYPE !  SEEN-PUB SEEN-PUB-N KEEP-BODY  THEN
  ENDCASE ;
: PEER-DROP ( -- )   PEER @ ?DUP IF SOCK-CLOSE DROP THEN  0 PEER !  0 PIN-N ! ;
: PEER-STEP ( -- )   \ accept, read, answer
  PEER @ 0= IF  LISTENER @ TCP-ACCEPT IF PEER ! THEN  EXIT  THEN
  PEER @ SOCK-AVAILABLE ?DUP IF
    2048 PIN-N @ - MIN  PIN PIN-N @ + SWAP PEER @ SOCK-READ DROP  PIN-N +!
  ELSE  PEER @ SOCK-OPEN? 0= IF PEER-DROP EXIT THEN  THEN
  BEGIN P-WHOLE? WHILE  P-ANSWER P-NEXT  REPEAT ;
: STEPS ( n -- )   0 ?DO  MQTT-STEP PEER-STEP NET-POLL  LOOP ;
: DRIP ( a u -- )   0 ?DO  DUP I + 1 P-SEND  2 STEPS  LOOP  DROP ;   \ one byte at a time

\ ---- part 2: a session
4096 BUFFER: L-ROOM   \ the buffer of the broker
T{ L-ROOM 4096 MQ-PORT TCP-LISTEN SWAP LISTENER ! -> 0 }T
0 PEER !  0 PIN-N !  0 ANSWER !  PONG ON!  0 SUB-CODE !  0 PACKET-ID !
LOOPBACK MQ-PORT MQTT-BROKER  UCF MQTT-ID  WILL$ T/O MQTT-WILL  60 MQTT-KEEPALIVE !
T{ MQTT-START  20 STEPS  MQTT-READY? -> TRUE }T
T{ ' FULL-ROOM CATCH -> -2 }T                                    \ not while it runs
T{ #CONNECT @  ATTEMPTS @  CONNECTS @ -> 1 1 1 }T
T{ SEEN-CONNECT SEEN-CONNECT-N @  CONNECT2 2 + #CONNECT2 2 - STR= -> TRUE }T   \ as built in part 1
T{ A/# MQTT-SUBSCRIBE  5 STEPS  #SUBSCRIBE @  LAST-CODE @ -> 0 1 0 }T
T{ SEEN-SUB SEEN-SUB-N @  SUB1 2 + #SUB1 2 - STR= -> TRUE }T
0 #GOT !
T{ PUB1 #PUB1 P-SEND  5 STEPS  #GOT @  HI A/B GOT? -> 1 TRUE }T            \ from the broker
T{ PUB1 #PUB1 DRIP  #GOT @  HI A/B GOT? -> 2 TRUE }T                       \ one byte at a time
T{ BIG 1500 A/B $30 PUBLISH-PACKET P-SEND  12 STEPS  #GOT @  GOT-MN @ -> 3 1500 }T
T{ BIG 1500 GOT-TEXT 1500 STR= -> TRUE }T
CREATE PUB-HELLO  0 C, 3 C, 'x' C, '/' C, 'y' C,  'h' C, 'e' C, 'l' C, 'l' C, 'o' C,
T{ HELLO X/Y MQTT-PUBLISH  3 STEPS  #PUBLISH @  SEEN-PUB-TYPE @ -> 0 1 $30 }T   \ to the broker
T{ SEEN-PUB SEEN-PUB-N @ PUB-HELLO 10 STR= -> TRUE }T
T{ HELLO X/Y MQTT-RETAIN  3 STEPS  #PUBLISH @  SEEN-PUB-TYPE @ -> 0 2 $31 }T
T{ BIG 1800 X/Y MQTT-PUBLISH  5 STEPS  SEEN-PUB-N @  SENT @ -> 0 1805 3 }T
T{ SEEN-PUB 5 + 1800 BIG 1800 STR= -> TRUE }T

\ keep alive: the clocks set instead of waiting
T{ MILLIS 100000 - SENT-AT !  3 STEPS  #PINGREQ @  PINGS @  PINGING @ -> 1 1 FALSE }T
PONG OFF!
T{ MILLIS 100000 - SENT-AT !  2 STEPS  PINGING @  MQTT-READY? -> TRUE TRUE }T
T{ MILLIS 100000 - PING-AT !  2 STEPS  MQ-STATE @  LAST-CODE @ -> S-WAIT MQTT-NO-PING }T
T{ DELAY-MS @ -> 2000 }T                                                  \ the pause doubles
PONG ON!
T{ 0 LATER  20 STEPS  MQTT-READY?  #CONNECT @  #SUBSCRIBE @  CONNECTS @ -> TRUE 2 2 2 }T  \ again, with the filter
T{ DELAY-MS @  ATTEMPTS @ -> 1000 2 }T

\ the broker closes, refuses, does not answer; no broker
T{ PEER-DROP  5 STEPS  MQ-STATE @  LAST-CODE @ -> S-WAIT NET-CLOSED }T
5 ANSWER !
T{ 0 LATER  20 STEPS  MQ-STATE @  LAST-CODE @  CONNACK-CODE @ -> S-WAIT MQTT-REFUSED 5 }T
-1 ANSWER !
T{ 0 LATER  20 STEPS  MQ-STATE @ -> S-CONNACK }T
T{ MILLIS 1- DUE !  2 STEPS  MQ-STATE @  LAST-CODE @ -> S-WAIT MQTT-NO-ANSWER }T
0 ANSWER !
LOOPBACK 47022 MQTT-BROKER
T{ MQTT-BROKER@ 47022 =  ROT ROT LOOPBACK STR= -> TRUE TRUE }T
T{ 0 LATER  10 STEPS  MQ-STATE @  LAST-CODE @ -> S-WAIT NET-REFUSED }T
LOOPBACK MQ-PORT MQTT-BROKER
T{ 0 LATER  20 STEPS  MQTT-READY?  CONNECTS @ -> TRUE 3 }T

\ four filters at most; a filter that the broker refuses
: F2 ( -- a u )   S" b/+" ;
: F3 ( -- a u )   S" c" ;
: F4 ( -- a u )   S" d" ;
: F5 ( -- a u )   S" e" ;
T{ F2 MQTT-SUBSCRIBE  F3 MQTT-SUBSCRIBE  F4 MQTT-SUBSCRIBE  F5 MQTT-SUBSCRIBE -> 0 0 0 MQTT-TOO-BIG }T
T{ A/# MQTT-SUBSCRIBE  5 STEPS  LAST-CODE @ -> 0 0 }T                    \ kept already: no fifth one
$80 SUB-CODE !
T{ F3 MQTT-SUBSCRIBE  5 STEPS  LAST-CODE @  MQTT-READY? -> 0 MQTT-SUB-FAILED TRUE }T
0 SUB-CODE !

\ stop: DISCONNECT, then close
T{ 5 STEPS  MQTT-STOP  5 STEPS  #DISCONNECT @  MQ-STATE @  MQTT-READY? -> 1 S-OFF FALSE }T
T{ HELLO X/Y MQTT-PUBLISH -> MQTT-NOT-READY }T
T{ PEER @  LISTENER @ SOCK-CLOSE -> 0 0 }T                               \ the peer saw the end
KEY0 'KEY? !
S" mqtt" .RESULTS
