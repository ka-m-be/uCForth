\ airmqtt.fs - package AIR-MQTT: MQTT over the WLAN of the board, against a broker on a host (a test)
\
\ The host runs an MQTT broker at port 1883 without a password (on Kai's
\ Mac: Mosquitto, orga/Uebergabe.md). The board has the image of
\ apps/irbridge/app.fs, the station is joined (WBACK-READY?), and MQTT is
\ in the RAM. In ucf term, in the folder ucf:
\   #require MQTT lib/net/mqtt.fs
\   #include lib/esp32/esp32/test/airmqtt.fs
\ Then, with the address of the host:
\   S" 192.168.1.20" AIR-MQTT-START   connect as ucfwroom, subscribe ucf/wroom/#
\   n AIR-MQTT-ROUND   n messages to ucf/wroom/echo and back over the broker
\   u AIR-MQTT-BIG     one message of u bytes (at most 4000) and back
\   n AIR-MQTT-SEND    the numbers 1 to n to ucf/wroom/out, one message each
\   .AIR-MQTT          the messages of the host to ucf/wroom/in: how many,
\                      the next number expected, gaps; then .MQTT
\ On the host, the messages in, and the messages out:
\   seq 1 100 | mosquitto_pub -h 127.0.0.1 -t ucf/wroom/in -l
\   mosquitto_sub -h 127.0.0.1 -t ucf/wroom/out -C 100
\ The words show numbers only.

PACKAGE AIR-MQTT ALSO MQTT ALSO NET ALSO INET ALSO TEXT ALSO BYTES

VARIABLE #IN     VARIABLE NEXT-IN   VARIABLE GAPS     \ the numbers of the host
VARIABLE #ECHO   VARIABLE ECHO-N    VARIABLE ECHO-OK  \ the messages back from the broker
VARIABLE OTHER   VARIABLE LOST
4000 BUFFER: PAYLOAD
12312 BUFFER: MQ-ROOM                 \ the room of MQTT: packets of at most 4096 bytes
: PATTERN ( -- )   4000 0 DO  I 253 MOD PAYLOAD I + C!  LOOP ;
: ID ( -- a u )   S" ucfwroom" ;
: ALL ( -- a u )   S" ucf/wroom/#" ;
: ECHO-T ( -- a u )   S" ucf/wroom/echo" ;
: IN-T ( -- a u )   S" ucf/wroom/in" ;
: OUT-T ( -- a u )   S" ucf/wroom/out" ;
: FROM-HOST ( pa pu -- )   \ a number 1, 2, 3 ... from the host
  10 DIGITS 1 = 0= IF  DROP 1 GAPS +!  EXIT  THEN
  DUP NEXT-IN @ <> IF 1 GAPS +! THEN  1+ NEXT-IN !  1 #IN +! ;
: ON-MSG ( pa pu ta tu -- )
  2DUP ECHO-T STR= IF  2DROP  DUP ECHO-N !  PAYLOAD SWAP BYTES= ECHO-OK !  1 #ECHO +!  EXIT  THEN
  2DUP IN-T STR= IF  2DROP FROM-HOST  EXIT  THEN
  2DROP 2DROP  1 OTHER +! ;   \ our own messages to ucf/wroom/out come back too
: STEPS ( ms -- )   MILLIS +  BEGIN  MQTT-STEP NET-POLL  DUP MILLIS - 0<  UNTIL  DROP ;
: ECHO? ( n -- f )   \ the echo number n is there, within 2 s
  MILLIS 2000 +  BEGIN
    OVER #ECHO @ > 0= IF  2DROP TRUE EXIT  THEN
    MQTT-STEP NET-POLL  DUP MILLIS - 0<
  UNTIL  2DROP FALSE ;

PUBLIC
: AIR-MQTT-START ( a u -- )   \ the broker at the host a u, port 1883
  1883 MQTT-BROKER  ID MQTT-ID  0 0 0 0 MQTT-WILL  ['] ON-MSG MQTT-ON-MESSAGE  MQ-ROOM 12312 MQTT-ROOM!
  0 #IN !  1 NEXT-IN !  0 GAPS !  0 #ECHO !  0 OTHER !  PATTERN
  MQTT-START  MILLIS 5000 +  BEGIN  MQTT-STEP NET-POLL  MQTT-READY?  OVER MILLIS - 0<  OR  UNTIL  DROP
  MQTT-READY? 0= ABORT" AIR-MQTT: no connection to the broker"
  ALL MQTT-SUBSCRIBE THROW  500 STEPS  ." MQTT ready" CR ;
: AIR-MQTT-ROUND ( n -- )   \ n messages to the broker and back, one at a time
  0 #ECHO !  0 LOST !  MILLIS SWAP  DUP 0 ?DO
    PAYLOAD I 1+ 40 MIN ECHO-T MQTT-PUBLISH THROW
    I 1+ ECHO? 0= IF  1 LOST +!  I 1+ #ECHO !  THEN
  LOOP
  ." MQTT rounds/lost/ms: " DUP . LOST @ . SWAP MILLIS SWAP - SWAP / . CR ;
: AIR-MQTT-BIG ( u -- )   \ one message of u bytes and back: ok, the length, ms
  4000 MIN  0 #ECHO !  ECHO-OK OFF!  MILLIS SWAP
  PAYLOAD SWAP ECHO-T MQTT-PUBLISH THROW
  1 ECHO? 0= IF  DROP ." MQTT big: no echo" CR EXIT  THEN
  ." MQTT big ok/bytes/ms: " ECHO-OK @ . ECHO-N @ . MILLIS SWAP - . CR ;
: AIR-MQTT-SEND ( n -- )   \ the numbers 1 to n to the host
  MILLIS SWAP  DUP 0 ?DO  I 1+ N>S OUT-T MQTT-PUBLISH THROW  LOOP
  500 STEPS  ." MQTT sent/ms: " . MILLIS SWAP - . CR ;
: .AIR-MQTT ( -- )
  ." MQTT host in/next/gaps/other: " #IN @ . NEXT-IN @ . GAPS @ . OTHER @ . CR  .MQTT ;

END-PACKAGE
