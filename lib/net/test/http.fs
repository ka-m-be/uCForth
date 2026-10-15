\ http.fs - the test of HTTP (lib/net/http.fs) over our TCP/IP and the loop (NO-LINK)
\
\ The server and a client in uCForth, both over the loop of NO-LINK. In
\ the texts of the requests | stands for CR LF. 32-bit cells; in the
\ folder ucf:
\   build/ucf-c -32 host/files.fs lib/net/test/http.fs </dev/null
\ tools/test.fs runs it too. At the end: "http: n tests, 0 errors".

ONLY FORTH DEFINITIONS ALSO FILES
[DEFINED] T{ S" forth/test/tester.fs" ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] NO-LINK  S" lib/net/nolink.fs"  ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] LINK-NET  S" lib/net/tcpip.fs"  ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] TEXT  S" lib/text.fs"  ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] IDLE  S" lib/idle.fs"  ?INCLUDED
ONLY FORTH DEFINITIONS ALSO FILES  INCLUDE lib/net/http.fs
ONLY FORTH DEFINITIONS  ALSO TEXT  ALSO NET  ALSO IDLE  ALSO HTTP  ALSO HTTP.I  ALSO FILES
DECIMAL

4096 BUFFER: S-ROOM
INCLUDE lib/net/test/httpcli.fs
8081 SERVER-PORT !

\ ---- the routes of the test
: HELLO ( -- )   S" hello" S" text/plain" HTTP-ANSWER ;
' HELLO S" /a" HTTP-GET
64 BUFFER: FIELD-IP   64 BUFFER: FIELD-PORT          \ counted
: FORM ( ba bu -- )
  2DUP S" ip" HTTP-FIELD IF  FIELD-IP PLACE  THEN
  S" port" HTTP-FIELD IF  FIELD-PORT PLACE  THEN
  S" /next" HTTP-SEE-OTHER ;
' FORM S" /form" HTTP-POST

S-ROOM 4096 8081 HTTP-START
: HELLO-ANSWER ( -- a u )   S" HTTP/1.0 200 OK|Content-Type: text/plain|Content-Length: 5|Connection: close||hello" CRLF| ;
T{ S" GET /a HTTP/1.0||" ASK  HELLO-ANSWER STR= -> TRUE }T
T{ S" GET /a?x=1 HTTP/1.1|Host: board||" ASK  S" ||hello" CRLF| ENDS? -> TRUE }T     \ a query: no matter
T{ S" GET /b HTTP/1.0||" ASK  S" HTTP/1.0 404 Not Found|" CRLF| STARTS? -> TRUE }T
T{ S" PUT /a HTTP/1.0||" ASK  S" HTTP/1.0 400 Bad Request|" CRLF| STARTS? -> TRUE }T
T{ S" POST /a HTTP/1.0|Content-Length: 0||" ASK  S" HTTP/1.0 404" STARTS? -> TRUE }T   \ a route for GET only
T{ S" POST /form HTTP/1.0|Content-Length: 21||ip=10.0.0.1&port=1883" ASK
   S" HTTP/1.0 303 See Other|Location: /next|" CRLF| STARTS? -> TRUE }T
T{ FIELD-IP COUNT S" 10.0.0.1" STR=  FIELD-PORT COUNT S" 1883" STR= -> TRUE TRUE }T
T{ #REQUESTS @ #NOT-FOUND @ #BAD @ -> 6 2 1 }T

\ ---- a request that does not end: 400 after 5 s; one of more than 1 KB: 400 at once
T{ OPEN 2 STEPS  S" GET /a" CLIENT @ SOCK-WRITE  4 STEPS  MILLIS 6000 - SINCE !  GET-ANSWER
   S" HTTP/1.0 400" STARTS? -> 0 TRUE }T
: LONG ( -- )   \ 1 120 bytes without the end of the head
  OPEN 2 STEPS  8 0 DO  S" GET /aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" CLIENT @ SOCK-WRITE DROP  2 STEPS  LOOP ;
T{ LONG GET-ANSWER S" HTTP/1.0 400" STARTS? -> TRUE }T

\ ---- the fields of a form
: F ( -- a u )   S" a=1&bb=22&c=" ;
T{ F S" a" HTTP-FIELD  F S" bb" HTTP-FIELD -> F DROP 2 + 1 TRUE  F DROP 7 + 2 TRUE }T
T{ F S" c" HTTP-FIELD ROT DROP  F S" d" HTTP-FIELD  F S" b" HTTP-FIELD -> 0 TRUE FALSE FALSE }T

\ ---- a value as it was typed: + a space, %XX a byte, in place
16 BUFFER: TYPED
: DECODED ( a u -- a2 u2 )   >R  TYPED R@ MOVE  TYPED R> HTTP-DECODE ;
T{ S" a+b%21c" DECODED S" a b!c" STR= -> TRUE }T
T{ S" %41%4a%zz%z4%4" DECODED S" AJ%zz%z4%4" STR= -> TRUE }T   \ small hex digits too; the rest stays
T{ S" x%4142" TYPED SWAP MOVE  TYPED 3 HTTP-DECODE  S" x%4" STR= -> TRUE }T   \ only the bytes of the value
T{ S" " DECODED NIP  S" %" DECODED S" %" STR= -> 0 TRUE }T

HTTP-STOP
T{ SERVER @ PEER @ -> 0 0 }T
S" http" .RESULTS
