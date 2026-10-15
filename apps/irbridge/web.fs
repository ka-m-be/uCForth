\ web.fs - package BRIDGE-WEB: the web pages of the IR bridge
\
\ A page for the browser at http://<the address of the board>/: the
\ state (as status sends it to MQTT), the broker with a form to change
\ it, and the last messages heard and sent. A few lines of JavaScript ask
\ for the state and the messages every 2 s. No password (orga/Archiv/013, F5):
\ the form changes only the broker. While the setup network is open
\ (BRIDGE-SETUP, setup.fs), the same address gives the page of the setup:
\ the networks of the scan, the passphrase, the broker.
\
\   BRIDGE-WEB-START ( -- )   the server of the pages at port 80 (HTTP)
\   BRIDGE-WEB-STOP ( -- )    the server off
\
\ The routes: GET / the page (in the setup: the page of the setup); GET
\ /status the state as JSON; GET /events the last messages as text, a
\ line each (BRIDGE-LOG); POST /settings the form (ip, port): the broker
\ to MQTT-BROKER, into the flash (BRIDGE-SAVE) and a new connection
\ (BRIDGE-RECONNECT), then back to the page. Only in the setup: GET /nets
\ the networks with WPA2 ({"0":"name",...}), the broker and the code of
\ the last try as JSON; POST /setup the form of the setup (net, pass, ip,
\ port): the broker to MQTT-BROKER and the try (BRIDGE-SETUP-JOIN), then a
\ page that says what comes. The address has 1 to 64 letters, digits,
\ dots and hyphens, the port is 1 to 65535, the passphrase has 8 to 63
\ characters from space to ~ (WPA2); else the answer is 400 and nothing
\ changes. After the form of the setup its body and the bytes that the
\ connection received are 0: they held the passphrase. The pages are
\ fixed text in the dictionary (1.8 KB). Load it after HTTP, IRBRIDGE,
\ BRIDGE-SETTINGS, MQTT, JSON and BRIDGE-SETUP (apps/irbridge/app.fs does
\ it). Test on the host: apps/irbridge/test/web.fs.

PACKAGE BRIDGE-WEB  ALSO HTTP  ALSO IRBRIDGE  ALSO BRIDGE-SETTINGS  ALSO MQTT  ALSO TEXT
ALSO JSON  ALSO WIFI  ALSO BRIDGE-SETUP   \ lib/package.fs

4096 BUFFER: WEB-ROOM               \ the connections of the server: 2 KB in, 2 KB out
512 BUFFER: STATUS-TEXT             \ the state as JSON
2048 BUFFER: PAGE-TEXT              \ a page: the head and its part; or the networks as JSON

\ ---- the pages: the head, then the part of each page
: +PAGE ( a u -- )   HERE OVER ALLOT SWAP MOVE  10 C, ;   \ a line of a page
CREATE HEAD
S" <!DOCTYPE html><html><head><meta charset='utf-8'><title>IR bridge</title>" +PAGE
S" <meta name='viewport' content='width=device-width,initial-scale=1'><style>" +PAGE
S" body{font-family:sans-serif;margin:1em;max-width:60em}td{padding:0 1em 0 0}" +PAGE
S" pre{background:#eee;padding:.5em;white-space:pre-wrap;word-break:break-all}" +PAGE
S" </style></head><body><h1>IR bridge</h1>" +PAGE
HERE HEAD - CONSTANT #HEAD
CREATE PAGE                         \ the page of the bridge
S" <table id='s'></table>" +PAGE
S" <h2>Broker</h2><form method='post' action='/settings'>" +PAGE
S" IP <input name='ip' size='15'> Port <input name='port' size='5'>" +PAGE
S" <button>Save</button></form><h2>Heard and sent</h2><pre id='e'></pre><script>" +PAGE
S" let f=document.forms[0],n=0;function g(i){return document.getElementById(i)}" +PAGE
S" function u(){fetch('/status').then(r=>r.json()).then(d=>{let t='';" +PAGE
S" for(const k in d)t+='<tr><td>'+k+'<td>'+JSON.stringify(d[k]);g('s').innerHTML=t;" +PAGE
S" if(!n++&&d.broker){let b=d.broker.split(':');" +PAGE
S" f.ip.value=b[0];f.port.value=b[1]}});" +PAGE
S" fetch('/events').then(r=>r.text()).then(t=>{g('e').textContent=t})}" +PAGE
S" u();setInterval(u,2000)</script></body></html>" +PAGE
HERE PAGE - CONSTANT #PAGE
CREATE SETUP-FORM                   \ the page of the setup
S" <h2>Setup</h2><p id='c'></p><form method='post' action='/setup'>" +PAGE
S" <p>WLAN <select name='net'></select><p>Passphrase <input name='pass'" +PAGE
S" type='password' minlength='8' maxlength='63' required><p>Broker" +PAGE
S" <input name='ip' size='15' required> Port <input name='port' size='5'" +PAGE
S" required><p><button>Save</button></form><script>let f=document.forms[0];" +PAGE
S" fetch('/nets').then(r=>r.json()).then(d=>{for(const i in d.nets)" +PAGE
S" f.net.add(new Option(d.nets[i],i));f.ip.value=d.ip;f.port.value=d.port||1883;" +PAGE
S" if(d.code)document.getElementById('c').textContent=" +PAGE
S" 'The last try failed: code '+d.code})</script></body></html>" +PAGE
HERE SETUP-FORM - CONSTANT #SETUP-FORM
CREATE JOINING                      \ the answer to the form of the setup
S" <h2>Setup</h2><p>Saved. The bridge closes this network now and joins the" +PAGE
S" WLAN; that takes about a minute. If it works, the bridge runs there." +PAGE
S" <p>If this network comes back, the try failed: join it again and open" +PAGE
S" this page.</body></html>" +PAGE
HERE JOINING - CONSTANT #JOINING

\ ---- the answers
: ANSWER-PAGE ( a u -- )   \ the head, then the part a u
  >R  HEAD PAGE-TEXT #HEAD MOVE  PAGE-TEXT #HEAD + R@ MOVE
  PAGE-TEXT #HEAD R> +  S" text/html; charset=utf-8" HTTP-ANSWER ;
: ANSWER-JSON ( a u ior -- )   IF  2DROP HTTP-BAD EXIT  THEN  S" application/json" HTTP-ANSWER ;

\ ---- the checks of the fields
: ALL? ( a u xt -- f )   \ xt ( c -- f ) is true for each byte of a u
  ROT ROT BOUNDS ?DO  I C@ OVER EXECUTE 0= IF  DROP UNLOOP FALSE EXIT  THEN  LOOP  DROP TRUE ;
: HOST-CHAR? ( c -- f )   \ a letter, a digit, . or -
  DUP 36 DIGIT? NIP  OVER '.' = OR  SWAP '-' = OR ;
: HOST? ( a u -- f )   DUP 1 65 WITHIN IF  ['] HOST-CHAR? ALL? EXIT  THEN  2DROP FALSE ;   \ 1 to 64 of them
: PRINTABLE? ( c -- f )   32 127 WITHIN ;   \ space to ~
: PASS? ( a u -- f )   DUP 8 64 WITHIN IF  ['] PRINTABLE? ALL? EXIT  THEN  2DROP FALSE ;   \ WPA2
: NUMBER-IN? ( a u lo hi -- n true | false )   \ a decimal number from lo to hi-1
  2SWAP 10 DIGITS 1 <> IF  DROP 2DROP FALSE EXIT  THEN  DUP 2SWAP WITHIN IF TRUE EXIT THEN  DROP FALSE ;
: PORT? ( a u -- port true | false )   1 65536 NUMBER-IN? ;
: BROKER-FIELDS ( ba bu -- ia iu port true | false )   \ ip and port of a form
  2DUP S" ip" HTTP-FIELD 0= IF  2DROP FALSE EXIT  THEN
  2DUP HOST? 0= IF  2DROP 2DROP FALSE EXIT  THEN                    ( ba bu ia iu )
  2SWAP S" port" HTTP-FIELD 0= IF  2DROP FALSE EXIT  THEN
  PORT? 0= IF  2DROP FALSE EXIT  THEN  TRUE ;

\ ---- the routes of the page
: PAGE-GET ( -- )
  BRIDGE-SETUP? IF  SETUP-FORM #SETUP-FORM  ELSE  PAGE #PAGE  THEN  ANSWER-PAGE ;
: STATUS-GET ( -- )   STATUS-TEXT 512 BRIDGE-STATUS ANSWER-JSON ;
: EVENTS-GET ( -- )   BRIDGE-LOG S" text/plain; charset=utf-8" HTTP-ANSWER ;
: SETTINGS-POST ( ba bu -- )
  BROKER-FIELDS 0= IF  HTTP-BAD EXIT  THEN  MQTT-BROKER  BRIDGE-SAVE  BRIDGE-RECONNECT  S" /" HTTP-SEE-OTHER ;

\ ---- the routes of the setup
: NETS-GET ( -- )   \ the networks with WPA2, the broker, the code of the last try
  BRIDGE-SETUP? 0= IF  HTTP-BAD EXIT  THEN
  PAGE-TEXT 2048 JSON-INTO  S" nets" JSON-OBJECT
  BRIDGE-SETUP-NETS 0 ?DO  I WIFI-WPA2? IF  I WIFI-NET 2DROP  I N>S JSON-TEXT  THEN  LOOP  JSON-END
  MQTT-BROKER@ >R  S" ip" JSON-TEXT  R> S" port" JSON-NUMBER  BRIDGE-SETUP-CODE S" code" JSON-NUMBER
  JSON-DONE ANSWER-JSON ;
: NET-FIELD ( ba bu -- i )   \ the network of the form: one of the scan, with WPA2
  S" net" HTTP-FIELD 0= THROW  0 BRIDGE-SETUP-NETS NUMBER-IN? 0= THROW  DUP WIFI-WPA2? 0= THROW ;
: PASS-FIELD ( ba bu -- a n )   \ the passphrase of the form, decoded in place
  S" pass" HTTP-FIELD 0= THROW  HTTP-DECODE  2DUP PASS? 0= THROW ;
: TAKE-SETUP ( ba bu -- )   \ each check: 0= THROW
  BRIDGE-SETUP? 0= THROW
  2DUP NET-FIELD >R  2DUP PASS-FIELD 2SWAP  BROKER-FIELDS 0= THROW  MQTT-BROKER  R> ROT ROT BRIDGE-SETUP-JOIN ;
: SETUP-POST ( ba bu -- )   \ then the body and the bytes received are 0: they held the passphrase
  2DUP ['] TAKE-SETUP CATCH  DUP IF  >R 2DROP R>  THEN  >R  0 FILL  WEB-ROOM 2048 0 FILL  R>
  IF  HTTP-BAD EXIT  THEN  JOINING #JOINING ANSWER-PAGE ;
' PAGE-GET S" /" HTTP-GET
' STATUS-GET S" /status" HTTP-GET
' EVENTS-GET S" /events" HTTP-GET
' SETTINGS-POST S" /settings" HTTP-POST
' NETS-GET S" /nets" HTTP-GET
' SETUP-POST S" /setup" HTTP-POST

PUBLIC
: BRIDGE-WEB-START ( -- )   WEB-ROOM 4096 80 HTTP-START ;
: BRIDGE-WEB-STOP ( -- )    HTTP-STOP ;

END-PACKAGE
