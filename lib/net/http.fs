\ http.fs - package HTTP: a small server of HTTP/1.0 over NET, one connection at a time
\
\ For the web page of a board (apps/irbridge/web.fs) and other small
\ pages. The server takes one connection at a time: the buffer of its
\ connections serves one of them (lib/net/README.md). It reads a request
\ up to its end (the empty line, for POST also the bytes of
\ Content-Length), finds its route, and the word of the route answers;
\ then the connection closes (Connection: close). A step in the chain of
\ IDLE does the work; it waits only while SOCK-WRITE gives an answer to
\ TCP.
\
\   a u port HTTP-START ( -- )   the server at port; a u: the buffer of its
\                     connections, at least 2 KB (half in, half out)
\   HTTP-STOP ( -- )   the server and its connection off
\   xt a u HTTP-GET ( -- )    xt ( -- ) answers GET of the path a u
\   xt a u HTTP-POST ( -- )   xt ( ba bu -- ) answers POST of the path a u;
\                     ba bu: the body of the request
\   ba bu ta tu HTTP-ANSWER ( -- )   in the word of a route: the answer 200,
\                     the body ba bu of the type ta tu ("text/html" ...)
\   a u HTTP-SEE-OTHER ( -- )   in the word of a route: the answer 303, the
\                     browser goes to the path a u
\   HTTP-BAD ( -- )   in the word of a route: the answer 400 (a field does not fit)
\   ba bu ka ku HTTP-FIELD ( -- va vu true | false )   the value of the field
\                     ka ku in the body of a form ("ip=1.2.3.4&port=1883"),
\                     as it is: no %XX
\   va vu HTTP-DECODE ( -- va vu' )   the value as it was typed, in place:
\                     + a space, %XX the byte XX; a % without two hex
\                     digits stays
\   .HTTP ( -- )   the requests, the answers 404 and 400, the code of the
\                     last error
\
\ A request of more than 1 KB, or one that does not end within 5 s, gets
\ the answer 400; a path without a route 404. At most 8 routes; a path
\ has at most 31 characters, and a query (?...) does not count. Load it
\ after NET, TEXT and IDLE. Test on the host: lib/net/test/http.fs.

PACKAGE HTTP  ALSO NET  ALSO TEXT  ALSO IDLE   \ lib/package.fs

1024 CONSTANT /REQUEST
/REQUEST BUFFER: REQUEST   VARIABLE REQ-N           \ the request so far
VARIABLE SERVER   VARIABLE PEER   VARIABLE SINCE      \ the connection at work; MILLIS at its start
VARIABLE #REQUESTS   VARIABLE #NOT-FOUND   VARIABLE #BAD   VARIABLE LAST-ERROR
1 CONSTANT GET   2 CONSTANT POST   VARIABLE WANT       \ the methods; that of the request
8 CONSTANT #ROUTES   2 CELLS 32 + CONSTANT /ROUTE     \ a route: the method, the xt, the path (counted)
#ROUTES /ROUTE * BUFFER: ROUTES   ROUTES #ROUTES /ROUTE * 0 FILL
CREATE CRLF  13 C, 10 C, 13 C, 10 C,                  \ CRLF 2: the end of a line; CRLF 4: of the head
256 BUFFER: HEAD                                      \ the head of an answer, counted
VARIABLE KA   VARIABLE KU                             \ HTTP-FIELD: the name
VARIABLE DST                                          \ HTTP-DECODE: the place of the next byte

\ ---- the answer
: STATUS-LINE ( a u -- )   S" HTTP/1.0 " HEAD PLACE  HEAD +PLACE  CRLF 2 HEAD +PLACE ;
: +HEADER ( va vu na nu -- )   HEAD +PLACE  S" : " HEAD +PLACE  HEAD +PLACE  CRLF 2 HEAD +PLACE ;
: SEND ( a u -- )   ?DUP IF  PEER @ SOCK-WRITE THROW  ELSE  DROP  THEN ;
: SEND-HEAD ( -- )   S" close" S" Connection" +HEADER  CRLF 2 HEAD +PLACE  HEAD COUNT SEND ;
: ANSWER ( ba bu ta tu sa su -- )   \ the status sa su, then the body ba bu of the type ta tu
  STATUS-LINE  S" Content-Type" +HEADER  DUP N>S S" Content-Length" +HEADER  SEND-HEAD  SEND ;
: NOT-FOUND ( -- )   1 #NOT-FOUND +!  S" not found" S" text/plain" S" 404 Not Found" ANSWER ;
: BAD ( -- )   1 #BAD +!  S" bad request" S" text/plain" S" 400 Bad Request" ANSWER ;

\ ---- the request
: HEAD-END ( -- u | 0 )   \ the length of the head with its empty line; 0: not there yet
  REQ-N @ 3 - 0 MAX 0 ?DO  REQUEST I + 4 CRLF 4 STR= IF  I 4 + UNLOOP EXIT  THEN  LOOP  0 ;
: #DIGITS ( a u -- n )   \ the digits at the start of a u
  DUP 0 ?DO  OVER I + C@ '0' - 10 U< 0= IF  2DROP I UNLOOP EXIT  THEN  LOOP  NIP ;
: NUMBER-OF ( a u -- n )   \ the whole number at the start of a u; 0: none, or too big
  OVER SWAP #DIGITS  10 DIGITS 1 = AND ;
: LENGTH ( u -- n )   \ Content-Length in the head of u bytes; 0 without it
  REQUEST SWAP  BEGIN  DUP 15 > WHILE
    OVER 15 S" Content-Length:" S= IF  15 /STRING -LEADING NUMBER-OF EXIT  THEN
    1/STRING
  REPEAT  2DROP 0 ;
: METHOD ( -- m )   \ GET, POST, or 0 for another one
  REQUEST 4 S" GET " STR= IF GET EXIT THEN  REQUEST 5 S" POST " STR= IF POST EXIT THEN  0 ;
: PATH ( -- a u )   \ the path of the request line, without a query
  REQUEST REQ-N @ 32 SCAN 1/STRING  2DUP 32 SCAN NIP -  2DUP '?' SCAN NIP - ;
: BODY ( -- a u )   HEAD-END DUP REQUEST + SWAP LENGTH ;
: COMPLETE? ( -- f )   \ the head is there, and the body of Content-Length
  HEAD-END ?DUP 0= IF FALSE EXIT THEN  DUP LENGTH +  REQ-N @ > 0= ;
: ROUTE ( i -- a )   /ROUTE * ROUTES + ;
: FIND-ROUTE ( a u -- xt | 0 )   \ the route of the method WANT and the path a u
  #ROUTES 0 DO
    I ROUTE @ WANT @ = IF  2DUP I ROUTE 2 CELLS + COUNT STR= IF  2DROP I ROUTE CELL+ @ UNLOOP EXIT  THEN  THEN
  LOOP  2DROP 0 ;
: DISPATCH ( -- )   \ the word of the route answers
  1 #REQUESTS +!
  METHOD DUP WANT !  0= IF BAD EXIT THEN
  PATH FIND-ROUTE ?DUP 0= IF NOT-FOUND EXIT THEN
  WANT @ POST = IF  BODY ROT  THEN  EXECUTE ;

\ ---- the step in the chain of IDLE
: CLOSE ( -- )   PEER @ ?DUP IF  SOCK-CLOSE DROP  THEN  0 PEER ! ;
: TAKE ( -- f )   \ the bytes that came, as many as fit; false: no room any more
  /REQUEST REQ-N @ -  DUP 0= IF  DROP FALSE EXIT  THEN
  REQUEST REQ-N @ + SWAP PEER @ SOCK-READ THROW  REQ-N +!  TRUE ;
: STEP ( -- )
  PEER @ 0= IF  SERVER @ TCP-ACCEPT IF  PEER !  0 REQ-N !  MILLIS SINCE !  THEN  EXIT  THEN
  PEER @ SOCK-AVAILABLE IF  TAKE 0= IF  BAD CLOSE EXIT  THEN  THEN
  COMPLETE? IF  DISPATCH CLOSE EXIT  THEN
  MILLIS SINCE @ - 5000 > IF  BAD CLOSE  THEN ;
: HTTP-STEP ( -- )   ['] STEP CATCH ?DUP IF  DUP -28 = IF THROW THEN  LAST-ERROR !  CLOSE  THEN ;
: FREE-ROUTE ( -- a )
  #ROUTES 0 DO  I ROUTE @ 0= IF  I ROUTE UNLOOP EXIT  THEN  LOOP  TRUE ABORT" HTTP: 8 routes already" ;
: ROUTE! ( xt a u m -- )
  >R  DUP 31 > ABORT" HTTP: a path has at most 31 characters"
  FREE-ROUTE  R> OVER !  >R  R@ 2 CELLS + PLACE  R> CELL+ ! ;
: NEXT-PART ( a u -- ra ru pa pu )   \ the part up to the first &, and the rest after it
  2DUP '&' SCAN  DUP >R  2SWAP R> -  2SWAP DUP IF 1/STRING THEN  2SWAP ;
: UNHEX ( a -- c f )   \ the two hex digits at a as a byte; f false: not two hex digits
  DUP C@ 16 DIGIT? >R  4 LSHIFT  SWAP 1+ C@ 16 DIGIT? >R  OR  R> R> AND ;

PUBLIC
: HTTP-START ( a u port -- )
  SERVER @ ABORT" HTTP: the server runs already"
  TCP-LISTEN THROW SERVER !  0 PEER !  0 #REQUESTS !  0 #NOT-FOUND !  0 #BAD !  0 LAST-ERROR !
  ['] HTTP-STEP IDLE+ ;
: HTTP-STOP ( -- )   ['] HTTP-STEP IDLE-  CLOSE  SERVER @ ?DUP IF  SOCK-CLOSE DROP  THEN  0 SERVER ! ;
: HTTP-GET ( xt a u -- )    GET ROUTE! ;
: HTTP-POST ( xt a u -- )   POST ROUTE! ;
: HTTP-ANSWER ( ba bu ta tu -- )   S" 200 OK" ANSWER ;
: HTTP-BAD ( -- )   BAD ;
: HTTP-SEE-OTHER ( a u -- )
  S" 303 See Other" STATUS-LINE  S" Location" +HEADER  S" 0" S" Content-Length" +HEADER  SEND-HEAD ;
: HTTP-FIELD ( ba bu ka ku -- va vu true | false )
  KU ! KA !
  BEGIN  DUP WHILE
    NEXT-PART                                        ( ra ru pa pu )
    DUP KU @ > IF
      OVER KU @ KA @ KU @ STR=  >R  OVER KU @ + C@ '=' =  R> AND IF
        KU @ 1+ /STRING  2SWAP 2DROP TRUE EXIT
      THEN
    THEN  2DROP
  REPEAT  2DROP FALSE ;
: HTTP-DECODE ( va vu -- va vu' )
  OVER DST !  OVER >R
  BEGIN  DUP WHILE
    OVER C@ '%' =  OVER 2 > AND  IF  OVER 1+ UNHEX  ELSE  0 FALSE  THEN     ( a u c f )
    IF  DST @ C!  3  ELSE  DROP  OVER C@  DUP '+' = IF DROP 32 THEN  DST @ C!  1  THEN
    /STRING  1 DST +!
  REPEAT  2DROP  R> DST @ OVER - ;
: .HTTP ( -- )
  ." HTTP requests/404/400/code: " #REQUESTS @ . #NOT-FOUND @ . #BAD @ . LAST-ERROR @ . CR ;

END-PACKAGE
