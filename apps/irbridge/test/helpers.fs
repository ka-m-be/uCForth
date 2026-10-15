\ helpers.fs - the helpers of the tests of the IR bridge, after mocks.fs and bridge.fs

\ ---- the helpers of the test
CREATE TEXTS 4096 ALLOT  VARIABLE TEXTS-N  0 TEXTS-N !  VARIABLE DEST
: Q ( a u -- a2 u )   \ the text a u, each ' as ", at a place of its own
  TEXTS-N @ 3000 > IF 0 TEXTS-N ! THEN  TEXTS TEXTS-N @ + DEST !
  DUP 0 ?DO  OVER I + C@  DUP 39 = IF DROP 34 THEN  DEST @ I + C!  LOOP
  NIP DEST @ SWAP  DUP TEXTS-N +! ;
VARIABLE WANT-A   VARIABLE WANT-U
: PREFIX? ( a u a2 u2 -- f )   \ a u starts with a2 u2
  WANT-U ! WANT-A !  WANT-U @ < IF DROP FALSE EXIT THEN  WANT-U @ WANT-A @ WANT-U @ STR= ;
: SUFFIX? ( a u a2 u2 -- f )   \ a u ends with a2 u2
  WANT-U ! WANT-A !  DUP WANT-U @ < IF 2DROP FALSE EXIT THEN
  WANT-U @ - +  WANT-U @  WANT-A @ WANT-U @ STR= ;
: CONTAINS? ( a u a2 u2 -- f )   \ a u holds a2 u2 somewhere
  WANT-U ! WANT-A !
  BEGIN  DUP WANT-U @ < 0= WHILE
    OVER WANT-U @ WANT-A @ WANT-U @ STR= IF  2DROP TRUE EXIT  THEN  1/STRING
  REPEAT  2DROP FALSE ;
: QL ( a u -- a2 u2 )   \ as Q, and each | as the end of a line (LF)
  Q  2DUP OVER + SWAP ?DO  I C@ '|' = IF  10 I C!  THEN  LOOP ;
: AT? ( n a u -- f )   \ the message n went to the topic IRBRIDGE/A0B1C2D3E4F5/ and a u
  ROT DUP >R TOPIC@ 22 /STRING STR=  R> TOPIC@ S" IRBRIDGE/A0B1C2D3E4F5/" PREFIX?  AND ;
: ONLINE ( -- a u )   S" online" ;   : STATUS-T ( -- a u )   S" status" ;
: EVENTS ( -- a u )   S" events" ;   : RESULT-T ( -- a u )   S" result" ;
: SEND-T ( -- a u )   S" IRBRIDGE/A0B1C2D3E4F5/send" ;
: ASK ( a u -- )   Q SEND-T DELIVER ;                   \ a request on send, with ' for "
: ANSWER ( -- a u )   LAST TEXT@ ;
