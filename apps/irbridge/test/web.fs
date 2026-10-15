\ web.fs - the test of BRIDGE-WEB on the host: the page, the state, the messages, the form
\
\ HTTP and a client over our TCP/IP and the loop (NO-LINK). MQTT,
\ IRBRIDGE, BRIDGE-SETTINGS, WIFI and BRIDGE-SETUP are small mocks here:
\ the state and the messages are fixed texts, the broker is kept, the
\ saves and the new connections are counted, the scan has 3 networks,
\ the try of the setup notes its network and passphrase. The real
\ BRIDGE-STATUS and BRIDGE-LOG: test of the bridge (test/bridge.fs); the
\ real setup: test/setup.fs. 32-bit cells; in the folder ucf:
\   build/ucf-c -32 host/files.fs apps/irbridge/test/web.fs </dev/null
\ tools/test.fs runs it too. At the end: "web: n tests, 0 errors".

ONLY FORTH DEFINITIONS  ALSO FILES
[DEFINED] T{ S" forth/test/tester.fs" ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] NO-LINK   S" lib/net/nolink.fs"  ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] LINK-NET  S" lib/net/tcpip.fs"   ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] TEXT      S" lib/text.fs"        ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] IDLE      S" lib/idle.fs"        ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] JSON      S" lib/json.fs"        ?INCLUDED
ONLY FORTH DEFINITIONS  ALSO TEXT
PACKAGE MQTT  ALSO TEXT  PUBLIC
64 BUFFER: M-HOST   0 M-HOST C!   VARIABLE M-PORT
: MQTT-BROKER ( a u port -- )   M-PORT !  M-HOST PLACE ;
: MQTT-BROKER@ ( -- a u port )   M-HOST COUNT  M-PORT @ ;
END-PACKAGE
PACKAGE IRBRIDGE  PUBLIC  VARIABLE RECONNECTS
: BRIDGE-STATUS ( a u -- ta tu ior )   2DROP S" {'version':'0.2'}" 0 ;
: BRIDGE-LOG ( -- a u )   S" events {'t':[100],'s':'0'}" ;
: BRIDGE-RECONNECT ( -- )   1 RECONNECTS +! ;
END-PACKAGE
PACKAGE BRIDGE-SETTINGS  PUBLIC  VARIABLE SAVES
: BRIDGE-SAVE ( -- )   1 SAVES +! ;
END-PACKAGE
CREATE QUOTED  6 C, 'B' C, 'e' C, '"' C, 't' C, 'a' C, '\' C,    \ a name with " and \
PACKAGE WIFI  ALSO TEXT  PUBLIC                                    \ the last of the 3 without WPA2
: WIFI-NET ( i -- a n ch dbm )   CASE  0 OF S" Alpha" ENDOF  1 OF QUOTED COUNT ENDOF  S" Open" ROT  ENDCASE  6 -50 ;
: WIFI-WPA2? ( i -- f )   2 <> ;
END-PACKAGE
PACKAGE BRIDGE-SETUP  ALSO TEXT  PUBLIC  VARIABLE SETUP?  VARIABLE #TRIES  VARIABLE T-NET  64 BUFFER: T-PASS
: BRIDGE-SETUP? ( -- f )   SETUP? @ ;
: BRIDGE-SETUP-NETS ( -- u )   3 ;
: BRIDGE-SETUP-CODE ( -- ior )   -422 ;
: BRIDGE-SETUP-JOIN ( i a n -- )   T-PASS PLACE  T-NET !  1 #TRIES +! ;
END-PACKAGE
ONLY FORTH DEFINITIONS  ALSO FILES  INCLUDE lib/net/http.fs
ONLY FORTH DEFINITIONS  ALSO FILES  INCLUDE apps/irbridge/web.fs
ONLY FORTH DEFINITIONS  ALSO TEXT  ALSO NET  ALSO IDLE  ALSO HTTP  ALSO HTTP.I  ALSO MQTT
ALSO IRBRIDGE  ALSO BRIDGE-SETTINGS  ALSO BRIDGE-SETUP  ALSO BRIDGE-WEB  ALSO BRIDGE-WEB.I  ALSO FILES
INCLUDE lib/net/test/httpcli.fs
DECIMAL
: BROKER? ( a u port -- f )   MQTT-BROKER@ >R ROT R> =  >R STR= R> AND ;
: BODY? ( a u -- f )   GOT GOT-N @ 2SWAP ENDS? ;   \ the answer ends with a u

BRIDGE-WEB-START

\ ---- the page, the state, the messages
: PAGE-HEAD ( -- a u )   S" HTTP/1.0 200 OK|Content-Type: text/html; charset=utf-8|Content-Length: " CRLF| ;
T{ S" GET / HTTP/1.1|Host: bridge||" ASK  PAGE-HEAD STARTS?  PAGE #PAGE BODY? -> TRUE TRUE }T
T{ GOT GOT-N @ HEAD #HEAD CONTAINS?  #HEAD #PAGE + 900 1100 WITHIN  HEAD 15 S" <!DOCTYPE html>" STR= -> TRUE TRUE TRUE }T
: ASCII? ( a u -- f )   \ only the end of a line and the characters from space to ~
  BOUNDS ?DO  I C@ DUP 10 =  SWAP 32 127 WITHIN OR 0= IF  UNLOOP FALSE EXIT  THEN  LOOP  TRUE ;
T{ HEAD #HEAD ASCII?  PAGE #PAGE ASCII?  SETUP-FORM #SETUP-FORM ASCII?  JOINING #JOINING ASCII? -> TRUE TRUE TRUE TRUE }T
: JSON-HEAD ( -- a u )   S" HTTP/1.0 200 OK|Content-Type: application/json|Content-Length: 17|" CRLF| ;
T{ S" GET /status HTTP/1.0||" ASK  JSON-HEAD STARTS?  S" {'version':'0.2'}" BODY? -> TRUE TRUE }T
T{ S" GET /events HTTP/1.0||" ASK  S" ||events {'t':[100],'s':'0'}" CRLF| ENDS? -> TRUE }T
T{ S" GET /favicon.ico HTTP/1.0||" ASK  S" HTTP/1.0 404" STARTS? -> TRUE }T

\ ---- the form: the broker, into the flash, a new connection
: FORM-OK ( -- a u )   S" POST /settings HTTP/1.0|Content-Length: 21||ip=10.0.0.9&port=1884" ;
: FORM-PORT ( -- a u )   S" POST /settings HTTP/1.0|Content-Length: 22||ip=10.0.0.9&port=70000" ;
: FORM-IP ( -- a u )   S" POST /settings HTTP/1.0|Content-Length: 21||ip=10.0.0<9&port=1884" ;
: FORM-NO-PORT ( -- a u )   S" POST /settings HTTP/1.0|Content-Length: 11||ip=10.0.0.9" ;
T{ FORM-OK ASK  S" HTTP/1.0 303 See Other|Location: /|" CRLF| STARTS? -> TRUE }T
T{ S" 10.0.0.9" 1884 BROKER?  SAVES @  RECONNECTS @ -> TRUE 1 1 }T
T{ FORM-PORT ASK  S" HTTP/1.0 400" STARTS?  S" 10.0.0.9" 1884 BROKER?  SAVES @ -> TRUE TRUE 1 }T   \ nothing changes
T{ FORM-IP ASK  S" HTTP/1.0 400" STARTS?  FORM-NO-PORT ASK  S" HTTP/1.0 400" STARTS?  SAVES @ -> TRUE TRUE 1 }T
T{ #REQUESTS @  #NOT-FOUND @  #BAD @ -> 8 1 3 }T

\ ---- the checks of the form
T{ S" 192.168.1.20" HOST?  S" mqtt.local" HOST?  S" a-b" HOST? -> TRUE TRUE TRUE }T
T{ S" " HOST?  S" a b" HOST?  S" a%41" HOST? -> FALSE FALSE FALSE }T
T{ S" 1883" PORT?  S" 65535" PORT? -> 1883 TRUE 65535 TRUE }T
T{ S" 0" PORT?  S" 65536" PORT?  S" 18x3" PORT?  S" " PORT? -> FALSE FALSE FALSE FALSE }T
T{ S" 12345678" PASS?  S" a b~c!d e" PASS?  S" 1234567" PASS? -> TRUE TRUE FALSE }T
T{ S" 123456789012345678901234567890123456789012345678901234567890123" PASS? -> TRUE }T   \ 63
T{ S" 1234567890123456789012345678901234567890123456789012345678901234" PASS? -> FALSE }T

\ ---- the setup: the page, the networks, the form; not outside the setup
: Q ( a u -- a u )   2DUP BOUNDS ?DO  I C@ 39 = IF  34 I C!  THEN  LOOP ;   \ each ' as "
: FORM-SETUP ( -- a u )   S" POST /setup HTTP/1.0|Content-Length: 45||net=1&pass=ab%21cd+efgh&ip=10.0.0.7&port=1883" ;
: NO-WPA2 ( -- a u )   S" POST /setup HTTP/1.0|Content-Length: 45||net=2&pass=ab%21cd+efgh&ip=10.0.0.7&port=1883" ;
: NO-NET ( -- a u )    S" POST /setup HTTP/1.0|Content-Length: 45||net=3&pass=ab%21cd+efgh&ip=10.0.0.7&port=1883" ;
: SHORT ( -- a u )     S" POST /setup HTTP/1.0|Content-Length: 36||net=0&pass=abc&ip=10.0.0.7&port=1883" ;
: CONTROL ( -- a u )   S" POST /setup HTTP/1.0|Content-Length: 44||net=0&pass=abcdefgh%01&ip=10.0.0.7&port=1883" ;
: NO-IP ( -- a u )     S" POST /setup HTTP/1.0|Content-Length: 29||net=0&pass=abcdefgh&port=1883" ;
: BAD? ( a u -- f )   ASK S" HTTP/1.0 400" STARTS? ;
: NETS ( -- a u )   S" {'nets':{'0':'Alpha','1':'Be\'ta\\'},'ip':'10.0.0.9','port':1884,'code':-422}" Q ;
T{ S" GET /nets HTTP/1.0||" BAD?  FORM-SETUP BAD?  #TRIES @ -> TRUE TRUE 0 }T
SETUP? ON!
T{ S" GET / HTTP/1.0||" ASK  2DUP HEAD #HEAD CONTAINS?  ROT ROT SETUP-FORM #SETUP-FORM ENDS? -> TRUE TRUE }T
T{ S" GET /nets HTTP/1.0||" ASK  NETS ENDS? -> TRUE }T
T{ FORM-SETUP ASK  S" <h2>Setup</h2><p>Saved." CONTAINS?  #TRIES @  T-NET @  T-PASS COUNT S" ab!cd efgh" STR= -> TRUE 1 1 TRUE }T
T{ S" 10.0.0.7" 1883 BROKER?  REQUEST 1024 S" efgh" CONTAINS?  WEB-ROOM 2048 S" efgh" CONTAINS? -> TRUE FALSE FALSE }T
T{ NO-WPA2 BAD?  NO-NET BAD?  SHORT BAD?  CONTROL BAD?  NO-IP BAD?  #TRIES @ -> TRUE TRUE TRUE TRUE TRUE 1 }T
T{ S" 10.0.0.7" 1883 BROKER?  SETUP? OFF!  S" GET / HTTP/1.0||" ASK  PAGE #PAGE ENDS? -> TRUE TRUE }T

BRIDGE-WEB-STOP
S" web" .RESULTS
