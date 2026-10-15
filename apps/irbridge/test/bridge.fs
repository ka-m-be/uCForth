\ bridge.fs - the test of IRBRIDGE and BRIDGE-FRAMES on the host: topics and texts (apps/irbridge/)
\
\ MQTT, IR-QUEUE, NET and LINK are mocks here (test/mocks.fs, with the
\ helpers in test/helpers.fs); AC-BASE, IRCODE, RTY3 and IR-SYMBOLS are
\ the real packages. Each system with files and 32-bit cells; in the
\ folder ucf:
\   build/ucf-c -32 host/files.fs apps/irbridge/test/bridge.fs </dev/null
\ tools/test.fs runs it too. At the end: "bridge: n tests, 0 errors".

ONLY FORTH DEFINITIONS  ALSO FILES
[DEFINED] T{ S" forth/test/tester.fs" ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] TEXT        S" lib/text.fs"              ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] INET        S" lib/net/inet.fs"          ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] IDLE        S" lib/idle.fs"              ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] JSON        S" lib/json.fs"              ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] AC-BASE     S" lib/irremote/acbase.fs"   ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] IRCODE      S" lib/irremote/ircode.fs"   ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] RTY3        S" lib/irremote/rty3.fs"     ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] IR-SYMBOLS  S" lib/irremote/irsym.fs"    ?INCLUDED
ONLY FORTH DEFINITIONS  ALSO TEXT  ALSO FILES
DECIMAL
INCLUDE apps/irbridge/test/mocknet.fs
INCLUDE apps/irbridge/test/mocks.fs
ONLY FORTH DEFINITIONS  ALSO FILES
INCLUDE apps/irbridge/frames.fs
ONLY FORTH DEFINITIONS  ALSO FILES
INCLUDE apps/irbridge/bridge.fs
ONLY FORTH DEFINITIONS  ALSO TEXT  ALSO MQTT  ALSO IR-QUEUE  ALSO IRBRIDGE  ALSO IRBRIDGE.I
ALSO AC-BASE  ALSO RTY3  ALSO FILES
INCLUDE apps/irbridge/test/helpers.fs
CREATE FRAME3  100 , 200 , 300 ,
CREATE MANY   \ 17 durations: marks and spaces, each far from the others
  200 , 300 , 400 , 600 , 800 , 1200 , 1600 , 2400 , 3200 , 4800 , 6400 , 9600 ,
  12800 , 19200 , 25600 , 150 , 100 ,
400 CELLS BUFFER: BIG-FRAME
: FILL-BIG ( -- )   400 0 DO  32767 BIG-FRAME I CELLS + !  LOOP ;
FILL-BIG
: SENT@ ( i -- us )   CELLS SENT-TIMES + @ ;
: SENT= ( t m -- f )   \ the frame sent is the frame of m times at t
  DUP SENT-N @ <> IF 2DROP FALSE EXIT THEN  CELLS SENT-TIMES OVER STR= ;
: A1-HEX ( -- a u )   S" 60381305FF005020DC0015030000000003030006000046" ;
CREATE A1  $60 C, $38 C, $13 C, $05 C, $FF C, $00 C, $50 C, $20 C, $DC C, $00 C, $15 C, $03 C,
  $00 C, $00 C, $00 C, $00 C, $03 C, $03 C, $00 C, $06 C, $00 C, $00 C, $46 C,

\ ---- the start: IR, the ID, the will, the subscription, the room of MQTT
S" 10.0.0.1" 1883 MQTT-BROKER
T{ 4 14 BRIDGE-START  Q-ON @ Q-RING @ Q-OUT @ Q-IN @ -> TRUE 6432 4 14 }T
T{ M-ID COUNT S" IRBRIDGEA0B1C2D3E4F5" STR= -> TRUE }T
T{ MAC BRIDGE-ID S" A0B1C2D3E4F5" STR= -> TRUE }T
T{ W-TOPIC COUNT S" IRBRIDGE/A0B1C2D3E4F5/online" STR= -> TRUE }T
T{ W-TEXT COUNT S" false" STR=  SUB COUNT SEND-T STR= -> TRUE TRUE }T
T{ STARTED @ ROOM-U @ -> TRUE 9216 }T
T{ BRIDGE-STEP #PUB @ -> 0 }T                             \ not connected yet

\ ---- connected: online and status, retained; the state of the air conditioner
T{ READY ON!  BRIDGE-STEP #PUB @ -> 2 }T
T{ 0 ONLINE AT?  0 TEXT@ S" true" STR=  0 RETAINED? -> TRUE TRUE TRUE }T
T{ 1 STATUS-T AT?  1 RETAINED? -> TRUE TRUE }T
T{ 1 TEXT@ S" {'version':'0.2','ip':'192.168.1.20','uptime':" Q PREFIX? -> TRUE }T
: STATUS0 ( -- a u )   S" ,'ir':{'heard':0,'sent':0,'failed':0},'ac':{'power':false,'mode':'cool','temp':24,'fan':0,'flap':0},'broker':'10.0.0.1:1883','mqtt':true}" ;
T{ 1 TEXT@ STATUS0 Q SUFFIX? -> TRUE }T
T{ BRIDGE-STEP #PUB @ -> 2 }T                             \ once for each connection

\ ---- frames heard: events, as a table and a digit for each time
T{ FRAME3 3 HEAR  BRIDGE-STEP  2 EVENTS AT?  2 RETAINED? -> TRUE FALSE }T
T{ ANSWER S" {'t':[100,200,300],'s':'012'}" Q STR= -> TRUE }T
T{ MANY 17 HEAR  BRIDGE-STEP  ANSWER S" {'times':[200,300,400," Q PREFIX? -> TRUE }T   \ 17 durations
T{ MANY 16 HEAR  BRIDGE-STEP  ANSWER S" ,'s':'0123456789ABCDEF'}" Q SUFFIX? -> TRUE }T
T{ BIG-FRAME 400 HEAR  BRIDGE-STEP  ANSWER NIP  HEARD @ -> 426 4 }T   \ "t":[32767,32767] and 400 digits

\ ---- requests on send: raw, the frame to IR-QUEUE, the result after the send
T{ S" {'id':7,'times':[2000,1000,600]}" ASK  #SENDS @ SENT-N @ 0 SENT@ #PUB @ -> 1 3 2000 6 }T
T{ BRIDGE-STEP #PUB @ -> 6 }T                             \ IR-QUEUE is at work
T{ 0 RESULT-CODE !  BRIDGE-STEP  LAST RESULT-T AT?  ANSWER S" {'id':7,'code':0}" Q STR= -> TRUE TRUE }T
T{ S" {'id':8,'t':[2000,1000,600],'s':'012'}" ASK  SENT-N @ 0 SENT@ 1 SENT@ 2 SENT@ -> 3 2000 1000 600 }T
T{ -453 RESULT-CODE !  BRIDGE-STEP  ANSWER S" {'id':8,'code':-453}" Q STR= -> TRUE }T
T{ S" {'id':9,'schema':'raw','t':[9000,4500],'s':'010'}" ASK  SENT-N @ 2 SENT@ -> 3 9000 }T
T{ 0 RESULT-CODE !  BRIDGE-STEP  ANSWER S" {'id':9,'code':0}" Q STR= -> TRUE }T
T{ S" {'id':'x','times':[1,2]}" ASK  0 RESULT-CODE !  BRIDGE-STEP  ANSWER S" {'code':0}" Q STR= -> TRUE }T

\ ---- pd: the pulse distance code; with the last light, with blocks
T{ S" {'id':10,'schema':'pd','shape':[9000,4500,560,1690,560,0],'hex':'04FB08F7'}" ASK
   SENT-N @ 0 SENT@ 3 SENT@ 7 SENT@ 66 SENT@ -> 67 9000 560 1690 560 }T
T{ 0 RESULT-CODE !  BRIDGE-STEP  ANSWER S" {'id':10,'code':0}" Q STR= -> TRUE }T
: PD-A1 ( -- a u )   S" {'id':11,'schema':'pd','shape':[9000,4500,560,1690,540,0,800],'blocks':[23],'hex':'60381305FF005020DC0015030000000003030006000046'}" ;
T{ PD-A1 ASK  A1 23 AC>TIMES SENT= -> TRUE }T            \ as RTY3 makes it, with its last light 800
T{ 0 RESULT-CODE !  BRIDGE-STEP  ANSWER S" {'id':11,'code':0}" Q STR= -> TRUE }T
: PD-F21 ( -- a u )   S" {'schema':'pd','shape':[9000,4600,560,1680,600,8000],'blocks':[6,8,7],'hex':'8306826200008D1740000000003A0002000038003A'}" ;
T{ PD-F21 ASK  SENT-N @ 98 SENT@ 99 SENT@ 342 SENT@ -> 343 560 8000 560 }T
T{ 0 RESULT-CODE !  BRIDGE-STEP  ANSWER S" {'code':0}" Q STR= -> TRUE }T

\ ---- rty3: the fields change the state, then the whole frame goes out
T{ S" {'id':12,'schema':'rty3','power':true,'mode':'heat','temp':23}" ASK
   AC-POWER@ AC-MODE@ AC-TEMP@  AC-TIMINGS SENT= -> TRUE HEAT 23 TRUE }T
T{ 0 RESULT-CODE !  BRIDGE-STEP  ANSWER S" {'id':12,'code':0}" Q STR= -> TRUE }T
T{ S" {'id':13,'schema':'rty3','fan':2,'flap':5,'clock':'14:05'}" ASK  AC-FAN@ AC-FLAP@ NOW -> 2 5 845 }T
T{ 0 RESULT-CODE !  BRIDGE-STEP  ANSWER S" {'id':13,'code':0}" Q STR= -> TRUE }T
T{ S" {'id':14,'schema':'rty3','mode':'cool','temp':31}" ASK  AC-MODE@ AC-TEMP@ -> HEAT 23 }T   \ nothing changes
T{ ANSWER S" {'id':14,'code':-482}" Q STR= -> TRUE }T
T{ S" {'id':15,'schema':'rty3','mode':'warm'}" ASK  ANSWER S" {'id':15,'code':-482}" Q STR= -> TRUE }T
T{ S" {'id':16,'schema':'rty3','clock':'24:00'}" ASK  ANSWER S" {'id':16,'code':-482}" Q STR= -> TRUE }T
T{ S" {'id':17,'schema':'rty3','clock':'1405'}" ASK  ANSWER S" {'id':17,'code':-482}" Q STR= -> TRUE }T
T{ S" {'id':18,'schema':'rty3','temp':'x'}" ASK  ANSWER S" {'id':18,'code':-472}" Q STR= -> TRUE }T

\ ---- requests that fail: the result at once
T{ S" {'id':19," ASK  ANSWER S" {'code':-471}" Q STR= -> TRUE }T
T{ S" {'id':20}" ASK  ANSWER S" {'id':20,'code':-480}" Q STR= -> TRUE }T
T{ S" {'id':21,'schema':'nec'}" ASK  ANSWER S" {'id':21,'code':-481}" Q STR= -> TRUE }T
T{ S" {'id':22,'t':[1,2],'s':'013'}" ASK  ANSWER S" {'id':22,'code':-482}" Q STR= -> TRUE }T
T{ S" {'id':23,'s':'010'}" ASK  ANSWER S" {'id':23,'code':-480}" Q STR= -> TRUE }T
T{ S" {'id':24,'times':'x'}" ASK  ANSWER S" {'id':24,'code':-472}" Q STR= -> TRUE }T
: PD-5 ( -- a u )   S" {'id':25,'schema':'pd','shape':[9000,4500,560,1690,560],'hex':'04'}" ;
: PD-1 ( -- a u )   S" {'id':26,'schema':'pd','shape':[9000,4500,560,1690,560,0],'blocks':[1],'hex':'0405'}" ;
: PD-0 ( -- a u )   S" {'id':27,'schema':'pd','shape':[9000,4500,560,1690,560,0],'blocks':[0,2],'hex':'0405'}" ;
: PD-NO-HEX ( -- a u )   S" {'id':28,'schema':'pd','shape':[9000,4500,560,1690,560,0]}" ;
: PD-25 ( -- a u )   S" {'id':30,'schema':'pd','shape':[9000,4500,560,1690,560,0],'hex':'00112233445566778899AABBCCDDEEFF001122334455667788'}" ;
T{ PD-5 ASK  ANSWER S" {'id':25,'code':-482}" Q STR= -> TRUE }T      \ a shape of 5 times
T{ PD-1 ASK  ANSWER S" {'id':26,'code':-482}" Q STR= -> TRUE }T      \ blocks with fewer bytes
T{ PD-0 ASK  ANSWER S" {'id':27,'code':-482}" Q STR= -> TRUE }T      \ a block of 0 bytes
T{ PD-NO-HEX ASK  ANSWER S" {'id':28,'code':-480}" Q STR= -> TRUE }T
T{ S" {'id':29,'schema':'pd','hex':'04'}" ASK  ANSWER S" {'id':29,'code':-480}" Q STR= -> TRUE }T
T{ PD-25 ASK  ANSWER S" {'id':30,'code':-473}" Q STR= -> TRUE }T     \ 25 bytes
T{ -451 SEND-CODE !  S" {'id':31,'times':[1]}" ASK  0 SEND-CODE !  ANSWER S" {'id':31,'code':-451}" Q STR= -> TRUE }T
T{ #PUB @  S" {'id':32}" Q S" other/topic" DELIVER  #PUB @ - -> 0 }T    \ another topic: nothing
T{ HEARD @ SENT-OK @ FAILED-N @ LOST @ -> 4 8 19 0 }T

\ ---- without a connection a frame is lost; a new connection: online and status again
T{ READY OFF!  FRAME3 3 HEAR  BRIDGE-STEP  LOST @ -> 1 }T
T{ #PUB @  READY ON!  BRIDGE-STEP  #PUB @ SWAP -  LAST STATUS-T AT? -> 2 TRUE }T
T{ LAST 1- ONLINE AT? -> TRUE }T
: STATUS1 ( -- a u )   S" ,'ir':{'heard':4,'sent':8,'failed':19},'ac':{'power':true,'mode':'heat','temp':23,'fan':2,'flap':5,'clock':'14:05'},'broker':'10.0.0.1:1883','mqtt':true}" ;
T{ LAST TEXT@ STATUS1 Q SUFFIX? -> TRUE }T
T{ #PUB @  MILLIS 60000 - STATUS-AT !  BRIDGE-STEP  #PUB @ SWAP -  LAST STATUS-T AT? -> 1 TRUE }T

\ ---- the log of the last messages, and the state for the web page
: LOG-31 ( -- a u )   S" send {'id':31,'times':[1]}|result {'id':31,'code':-451}|" QL ;
T{ BRIDGE-LOG LOG-31 SUFFIX? -> TRUE }T
: LOG-12 ( -- a u )   S" |send {'id':12,'schema':'rty3','power':true,'mode':'heat','temp':23}|result {'id':12,'code':0}|" QL ;
T{ BRIDGE-LOG LOG-12 CONTAINS? -> TRUE }T
T{ BRIDGE-LOG NIP 4097 < -> TRUE }T
512 BUFFER: WEB-TEXT
T{ S" 10.0.0.2" 1883 MQTT-BROKER  WEB-TEXT 512 BRIDGE-STATUS  ROT ROT S" ,'broker':'10.0.0.2:1883','mqtt':true}" Q SUFFIX? -> 0 TRUE }T
T{ #PUB @  BRIDGE-RECONNECT  #PUB @ SWAP -  LAST ONLINE AT?  ANSWER S" false" STR=  STARTED @ -> 1 TRUE TRUE TRUE }T
T{ READY ON!  BRIDGE-STEP  LAST STATUS-T AT?  LAST 1- ONLINE AT? -> TRUE TRUE }T   \ the new connection: online, status

\ ---- an error in a step is counted, the bridge goes on
T{ -9 FRAME-ERROR !  BRIDGE-STEP  ERROR @ -> -9 }T
T{ FRAME3 3 HEAR  BRIDGE-STEP  HEARD @ -> 5 }T

\ ---- the end: online false, then MQTT and IR off
T{ BRIDGE-STOP  LAST ONLINE AT?  ANSWER S" false" STR=  LAST RETAINED?  STARTED @ Q-ON @ -> TRUE TRUE TRUE FALSE FALSE }T
T{ BRIDGE-STEP #PUB @  BRIDGE-STOP #PUB @ = -> TRUE }T      \ not connected: no message

\ ---- without a broker: IR and the step, MQTT only after BRIDGE-RECONNECT (the web page sets the broker)
T{ 0 M-HOST C!  4 14 BRIDGE-START  Q-ON @  STARTED @ -> TRUE FALSE }T
T{ S" 10.0.0.3" 1883 MQTT-BROKER  BRIDGE-RECONNECT  STARTED @ -> TRUE }T
BRIDGE-STOP

S" bridge" .RESULTS
