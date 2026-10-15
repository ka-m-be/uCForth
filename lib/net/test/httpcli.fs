\ httpcli.fs - a client of HTTP for the tests, over our TCP/IP and the loop (NO-LINK)
\
\ For lib/net/test/http.fs and apps/irbridge/test/web.fs, after HTTP (and
\ with HTTP.I in the search order: the steps of the server). In the texts
\ of the requests | stands for CR LF. The server listens at SERVER-PORT.

VARIABLE SERVER-PORT   80 SERVER-PORT !
1024 BUFFER: C-ROOM   VARIABLE CLIENT
1024 BUFFER: TEXT-OUT   VARIABLE OUT-N         \ a request, | as CR LF
2048 BUFFER: GOT   VARIABLE GOT-N              \ the answer
: CRLF| ( a u -- a2 u2 )   \ the text a u, each | as CR LF
  0 OUT-N !  BOUNDS ?DO
    I C@ '|' = IF  13 TEXT-OUT OUT-N @ + C!  1 OUT-N +!  10  ELSE  I C@  THEN
    TEXT-OUT OUT-N @ + C!  1 OUT-N +!
  LOOP  TEXT-OUT OUT-N @ ;
: STEPS ( n -- )   0 ?DO  NET-POLL  HTTP-STEP  LOOP ;
: OPEN ( -- )   C-ROOM 1024 S" localhost" SERVER-PORT @ TCP-OPEN THROW CLIENT ! ;
: GET-ANSWER ( -- a u )   \ all bytes of the answer, until the server closes
  0 GOT-N !  20 0 DO
    NET-POLL HTTP-STEP
    CLIENT @ SOCK-AVAILABLE IF  GOT GOT-N @ + 2048 GOT-N @ - CLIENT @ SOCK-READ THROW GOT-N +!  THEN
  LOOP  CLIENT @ SOCK-CLOSE DROP  GOT GOT-N @ ;
: ASK ( a u -- a2 u2 )   \ the request a u (| as CR LF), and its answer
  CRLF|  OPEN  2 STEPS  CLIENT @ SOCK-WRITE THROW  GET-ANSWER ;
VARIABLE WANT-A   VARIABLE WANT-U
: STARTS? ( a u a2 u2 -- f )   \ a u begins with a2 u2
  WANT-U ! WANT-A !  WANT-U @ < IF DROP FALSE EXIT THEN  WANT-U @ WANT-A @ WANT-U @ STR= ;
: ENDS? ( a u a2 u2 -- f )   \ a u ends with a2 u2
  WANT-U ! WANT-A !  DUP WANT-U @ < IF 2DROP FALSE EXIT THEN
  WANT-U @ - +  WANT-U @  WANT-A @ WANT-U @ STR= ;
: CONTAINS? ( a u a2 u2 -- f )   \ a u holds a2 u2 somewhere
  WANT-U ! WANT-A !
  BEGIN  DUP WANT-U @ < 0= WHILE
    OVER WANT-U @ WANT-A @ WANT-U @ STR= IF  2DROP TRUE EXIT  THEN  1/STRING
  REPEAT  2DROP FALSE ;
