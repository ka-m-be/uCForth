\ mqttnet.fs - the tests of MQTT against a real broker, over the sockets of macOS and Linux
\
\ It needs an MQTT broker at 127.0.0.1, port 1883, without a password (on
\ Kai's Mac: Mosquitto, orga/Uebergabe.md). It uses the topics
\ ucf/mqttnet/... only and leaves no retained message behind. The tests:
\ connect, subscribe, a message to itself (also 4000 bytes), a retained
\ message after a new connection, the last will of a second connection
\ that ends without DISCONNECT, keep alive with pings, a message too big
\ for the client (5000 bytes: dropped and counted), the stop without the
\ will. A second connection (RAW) speaks MQTT with the packets of the
\ package, as another client would.
\ Use (in the folder ucf):
\   echo "ALSO FILES  INCLUDE lib/net/test/mqttnet.fs" | build/ucf-macos-a64
\ At the end: "mqttnet: 15 tests, 0 errors" (about 5 s). The tests without
\ a broker: lib/net/test/mqtt.fs.

ONLY FORTH DEFINITIONS  ALSO FILES
[DEFINED] T{ S" forth/test/tester.fs" ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] NET    S" lib/net/host.fs"    ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] IDLE   S" lib/idle.fs"        ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] BYTES  S" lib/net/bytes.fs"   ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] MQTT   S" lib/net/mqtt.fs"    ?INCLUDED
ONLY FORTH DEFINITIONS  ALSO INTERNALS  ALSO TEXT  ALSO INET  ALSO NET  ALSO BYTES
ALSO MQTT  ALSO MQTT.I  ALSO FILES
DECIMAL

'KEY? @ CONSTANT KEY0              \ MQTT-START sets it (IDLE+); the end of the file puts it back
12312 BUFFER: MQ-ROOM   MQ-ROOM 12312 MQTT-ROOM!   \ packets of at most 4096 bytes

: LOOPBACK ( -- a u )   S" 127.0.0.1" ;
: NET-ID ( -- a u )   S" ucfmqttnet" ;
: ALL ( -- a u )   S" ucf/mqttnet/#" ;
: TOPIC-A ( -- a u )   S" ucf/mqttnet/a" ;
: TOPIC-R ( -- a u )   S" ucf/mqttnet/retained" ;
: TOPIC-W ( -- a u )   S" ucf/mqttnet/will" ;
: TOPIC-B ( -- a u )   S" ucf/mqttnet/big" ;
: HELLO ( -- a u )   S" hello from uCForth" ;
: R1 ( -- a u )   S" r1" ;
: GONE ( -- a u )   S" gone" ;
CREATE BIG 5000 ALLOT
: PATTERN ( -- )   5000 0 DO  I 253 MOD BIG I + C!  LOOP ;
PATTERN

4096 BUFFER: GOT-TEXT   VARIABLE GOT-MN   128 BUFFER: GOT-TOPIC   VARIABLE GOT-TN   VARIABLE #GOT
: RECORD ( pa pu ta tu -- )   \ keep the last message
  DUP GOT-TN !  GOT-TOPIC SWAP MOVE  DUP GOT-MN !  GOT-TEXT SWAP MOVE  1 #GOT +! ;
: GOT? ( pa pu ta tu -- f )   GOT-TOPIC GOT-TN @ STR=  >R  GOT-TEXT GOT-MN @ STR=  R> AND ;
' RECORD MQTT-ON-MESSAGE
: SETTLE ( ms -- )   0 ?DO  MQTT-STEP NET-POLL  LOOP ;   \ NET-POLL waits 1 ms
: READY-IN ( ms -- f )   0 ?DO  MQTT-STEP MQTT-READY? IF UNLOOP TRUE EXIT THEN  NET-POLL  LOOP  FALSE ;
: GOT-IN ( n ms -- f )   \ n messages in all, within ms
  0 ?DO  MQTT-STEP  DUP #GOT @ > 0= IF DROP UNLOOP TRUE EXIT THEN  NET-POLL  LOOP  DROP FALSE ;

\ ---- a second connection, as another client
VARIABLE RAW   CREATE ACK 4 ALLOT   4096 BUFFER: RAW-ROOM
: RAW-ID ( -- a u )   S" ucfmqttraw" ;
: RAW-SEND ( a u -- )   RAW @ SOCK-WRITE THROW ;
: RAW-OPEN ( -- f )   \ connect with the settings of the package, CONNACK 0
  RAW-ROOM 4096 LOOPBACK 1883 TCP-OPEN THROW RAW !  CONNECT-PACKET RAW-SEND
  ACK 4 RAW @ SOCK-READ THROW  4 =  ACK C@ $20 = AND  ACK 3 + C@ 0= AND ;
: RAW-WILL ( -- f )   \ a client with a will; its connection ends without DISCONNECT
  RAW-ID MQTT-ID  GONE TOPIC-W MQTT-WILL  RAW-OPEN  RAW @ SOCK-CLOSE DROP
  NET-ID MQTT-ID  0 0 0 0 MQTT-WILL ;
CREATE HEAD 8 ALLOT
: RAW-BIG ( -- f )   \ another client sends 5000 bytes to ucf/mqttnet/big
  RAW-ID MQTT-ID  RAW-OPEN
  $30 HEAD C!  5000 2 + TOPIC-B NIP +  HEAD 1+ +LEN HEAD TUCK - RAW-SEND
  TOPIC-B NIP HEAD BE16!  HEAD 2 RAW-SEND  TOPIC-B RAW-SEND  BIG 5000 RAW-SEND
  DISCONNECT-PACKET RAW-SEND  RAW @ SOCK-CLOSE DROP  NET-ID MQTT-ID ;

\ ---- the tests
LOOPBACK 1883 MQTT-BROKER  NET-ID MQTT-ID  0 0 0 0 MQTT-WILL  60 MQTT-KEEPALIVE !
T{ MQTT-START  2000 READY-IN -> TRUE }T
T{ ALL MQTT-SUBSCRIBE  100 SETTLE  LAST-CODE @ -> 0 0 }T
T{ HELLO TOPIC-A MQTT-PUBLISH  1 2000 GOT-IN  HELLO TOPIC-A GOT? -> 0 TRUE TRUE }T
T{ BIG 4000 TOPIC-A MQTT-PUBLISH  2 2000 GOT-IN  BIG 4000 TOPIC-A GOT? -> 0 TRUE TRUE }T
\ retained: the broker gives it to the next subscription
T{ R1 TOPIC-R MQTT-RETAIN  3 2000 GOT-IN -> 0 TRUE }T
T{ MQTT-STOP  MQTT-START  2000 READY-IN  4 2000 GOT-IN  R1 TOPIC-R GOT? -> TRUE TRUE TRUE }T
T{ 0 0 TOPIC-R MQTT-RETAIN  5 2000 GOT-IN  GOT-MN @ -> 0 TRUE 0 }T     \ an empty text removes it
\ the will of another client, retained; then removed
T{ RAW-WILL  6 2000 GOT-IN  GONE TOPIC-W GOT? -> TRUE TRUE TRUE }T
T{ 0 0 TOPIC-W MQTT-RETAIN  7 2000 GOT-IN -> 0 TRUE }T
\ too big for the client: dropped and counted; the next one comes
T{ RAW-BIG  500 SETTLE  DROPPED @  #GOT @  MQTT-READY? -> TRUE 1 7 TRUE }T
T{ HELLO TOPIC-A MQTT-PUBLISH  8 2000 GOT-IN -> 0 TRUE }T
\ keep alive of 2 s: a PINGREQ after 1 s without a packet, and its PINGRESP
2 MQTT-KEEPALIVE !
T{ MQTT-STOP  MQTT-START  2000 READY-IN -> TRUE }T
T{ 3500 SETTLE  PINGS @ 2 < 0=  PINGING @  MQTT-READY? -> TRUE FALSE TRUE }T
60 MQTT-KEEPALIVE !
\ the stop with DISCONNECT: the broker drops the will
T{ MQTT-STOP  MQTT-READY?  FAULTS @  ATTEMPTS @ -> FALSE 0 1 }T
T{ .MQTT -> }T
KEY0 'KEY? !
S" mqttnet" .RESULTS
