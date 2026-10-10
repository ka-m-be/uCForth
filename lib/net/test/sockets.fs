\ sockets.fs - the tests of the words of NET, on each system (lib/net/)
\
\ No other computer and no packet to the network: a server and a client
\ in this program talk over 127.0.0.1 (ports 47011 to 47015), and a small
\ DNS server in Forth answers on 127.0.0.1. The program has one thread:
\ the tests of DNS run the steps of DNS-QUERY one by one, and between
\ them the server answers. A file before this one loads the tester,
\ the network and the search order INTERNALS TEXT INET NET:
\   lib/net/test/net.fs     over the sockets of macOS and Linux
\   lib/net/test/loop.fs    over our TCP/IP, without a network
\   lib/net/test/board.fs   over our TCP/IP on a board (ucf term)

DECIMAL

CREATE BUF 1000 ALLOT
CREATE OUTPUT 64 ALLOT   VARIABLE OUTPUT-N
: CATCH-EMIT ( c -- )   OUTPUT OUTPUT-N @ + C!  1 OUTPUT-N +! ;
: SHOWN ( xt -- a u )   \ the output of xt as text
  0 OUTPUT-N !  'EMIT @ >R  ['] CATCH-EMIT 'EMIT !  EXECUTE  R> 'EMIT !  OUTPUT OUTPUT-N @ ;


\ ---- INET
T{ 192 168 1 10 IP -> $C0A8010A }T
T{ S" 192.168.1.10" >IP -> $C0A8010A TRUE }T
T{ S" 0.0.0.0" >IP -> 0 TRUE }T
T{ S" 255.255.255.255" >IP -> -1 TRUE }T
T{ S" 1.2.3" >IP -> FALSE }T
T{ S" 1.2.3.4.5" >IP -> FALSE }T
T{ S" 1.2.3.256" >IP -> FALSE }T
T{ S" 1..2.3" >IP -> FALSE }T
T{ S" 1.2.3.4 " >IP -> FALSE }T
T{ S" 0001.2.3.4" >IP -> FALSE }T
T{ S" " >IP -> FALSE }T
: SHOW-IP ( -- )   $C0A8010A .IP  0 .IP ;
T{ ' SHOW-IP SHOWN S" 192.168.1.10 0.0.0.0 " S= -> TRUE }T
T{ $C0A8010A BUF BE32!  BUF C@  BUF 3 + C@  BUF BE32@ -> 192 10 $C0A8010A }T
T{ $1234 BUF BE16!  BUF C@  BUF 1+ C@  BUF BE16@ -> $12 $34 $1234 }T

\ ---- RESOLVE without DNS
T{ S" 10.1.2.3" RESOLVE -> 10 1 2 3 IP 0 }T
T{ S" localhost" RESOLVE -> 127 0 0 1 IP 0 }T

\ ---- TCP over 127.0.0.1
47011 CONSTANT PORT
VARIABLE SERVER   VARIABLE CLIENT   VARIABLE PEER   \ PEER: the end of the connection at the server
T{ PORT TCP-LISTEN SWAP SERVER ! -> 0 }T
T{ PORT TCP-LISTEN -> 0 NET-IN-USE }T
T{ SERVER @ TCP-ACCEPT -> FALSE }T              \ nobody yet
T{ S" 127.0.0.1" PORT TCP-OPEN SWAP CLIENT ! -> 0 }T
T{ SERVER @ TCP-ACCEPT SWAP PEER ! -> TRUE }T
T{ CLIENT @ SOCK-OPEN?  PEER @ SOCK-OPEN? -> TRUE TRUE }T
T{ S" hello" CLIENT @ SOCK-WRITE -> 0 }T
T{ 10 MS  PEER @ SOCK-AVAILABLE -> 5 }T
T{ BUF 100 PEER @ SOCK-READ  BUF 5 S" hello" S= -> 5 0 TRUE }T
T{ S" back" PEER @ SOCK-WRITE  BUF 100 CLIENT @ SOCK-READ  BUF 4 S" back" S= -> 0 4 0 TRUE }T
T{ PEER @ SOCK-AVAILABLE  CLIENT @ SOCK-AVAILABLE -> 0 0 }T

\ many bytes, both ways: 64 times 1000 bytes, each time with other values
VARIABLE GOT   VARIABLE WANTED   VARIABLE SOCKET
: READ-ALL ( u sock -- f )   \ u bytes into BUF, with SOCK-READ; false: an error, or too few in time
  SOCKET !  WANTED !  0 GOT !
  BEGIN  GOT @ WANTED @ <  WHILE
    BUF GOT @ +  WANTED @ GOT @ -  SOCKET @ SOCK-READ
    ?DUP IF  2DROP FALSE EXIT  THEN
    ?DUP 0= IF  FALSE EXIT  THEN  GOT +!
  REPEAT  TRUE ;
: PATTERN ( n -- )   1000 0 DO  DUP I + BUF I + C!  LOOP  DROP ;
: PATTERN? ( n -- f )
  TRUE  1000 0 DO  OVER I + 255 AND  BUF I + C@ <> IF DROP FALSE THEN  LOOP  NIP ;
: ROUND ( n -- f )   \ 1000 bytes from the client to the server, and back
  DUP PATTERN  BUF 1000 CLIENT @ SOCK-WRITE IF DROP FALSE EXIT THEN
  BUF 1000 0 FILL  1000 PEER @ READ-ALL 0= IF DROP FALSE EXIT THEN
  DUP PATTERN? 0= IF DROP FALSE EXIT THEN
  BUF 1000 PEER @ SOCK-WRITE IF DROP FALSE EXIT THEN
  BUF 1000 0 FILL  1000 CLIENT @ READ-ALL 0= IF DROP FALSE EXIT THEN
  PATTERN? ;
: ROUNDS ( -- n )   0  64 0 DO  I ROUND IF 1+ THEN  LOOP ;
T{ ROUNDS -> 64 }T

\ the output to a connection
: GREET ( -- )   CLIENT @ >SOCKET  ." hello " 42 .  >CONSOLE ;
T{ GREET  9 PEER @ READ-ALL  BUF 9 S" hello 42 " S= -> TRUE TRUE }T
: MANY ( -- )   CLIENT @ >SOCKET  300 0 DO  'x' EMIT  LOOP  >CONSOLE ;   \ more than the buffer of 256
: ALL-X? ( u -- f )   TRUE SWAP 0 DO  BUF I + C@ 'x' <> IF DROP FALSE THEN  LOOP ;
T{ MANY  300 PEER @ READ-ALL  300 ALL-X? -> TRUE TRUE }T
T{ ' SHOW-IP SHOWN NIP -> 21 }T                  \ the console has the output again
: GIVE-42 ( -- )   ." 42 " ;
: FAULT ( -- )   ." 7 " -77 THROW ;
T{ ' GIVE-42 CLIENT @ SOCK-PRINT  3 PEER @ READ-ALL  BUF 3 S" 42 " S= -> 0 TRUE TRUE }T
T{ ' FAULT CLIENT @ SOCK-PRINT  2 PEER @ READ-ALL  BUF 2 S" 7 " S= -> -77 TRUE TRUE }T
T{ ' SHOW-IP SHOWN NIP -> 21 }T                  \ the console has the output again

\ the client closes: the server reads the rest, then the end
T{ S" end" CLIENT @ SOCK-WRITE  CLIENT @ SOCK-CLOSE -> 0 0 }T
T{ 10 MS  PEER @ SOCK-OPEN? -> TRUE }T          \ bytes wait
T{ BUF 100 PEER @ SOCK-READ -> 3 0 }T
T{ PEER @ SOCK-OPEN? -> FALSE }T
T{ BUF 100 PEER @ SOCK-READ -> 0 0 }T          \ the end: no wait
T{ PEER @ SOCK-CLOSE -> 0 }T

\ the server closes; then the client writes into a closed connection
T{ S" 127.0.0.1" PORT TCP-OPEN SWAP CLIENT ! -> 0 }T
T{ SERVER @ TCP-ACCEPT SWAP PEER ! -> TRUE }T
T{ PEER @ SOCK-CLOSE -> 0 }T
T{ 10 MS  CLIENT @ SOCK-OPEN? -> FALSE }T
: WRITE-TWICE ( -- ior )   S" 1" CLIENT @ SOCK-WRITE DROP  10 MS  S" 2" CLIENT @ SOCK-WRITE ;
T{ WRITE-TWICE -> NET-CLOSED }T                \ an error, not the end of the program (SIGPIPE)
T{ CLIENT @ SOCK-CLOSE -> 0 }T

\ a peer that sends nothing: SOCK-READ waits NET-TIMEOUT
200 NET-TIMEOUT !
T{ S" 127.0.0.1" PORT TCP-OPEN SWAP CLIENT ! -> 0 }T
T{ SERVER @ TCP-ACCEPT SWAP PEER ! -> TRUE }T
: TIMED-READ ( -- u ior ms )   MILLIS >R  BUF 100 CLIENT @ SOCK-READ  MILLIS R> - ;
T{ TIMED-READ 190 300 WITHIN -> 0 0 TRUE }T
T{ CLIENT @ SOCK-OPEN? -> TRUE }T
T{ CLIENT @ SOCK-CLOSE  PEER @ SOCK-CLOSE -> 0 0 }T

\ errors
T{ S" 127.0.0.1" PORT 1+ TCP-OPEN -> 0 NET-REFUSED }T     \ no server at the port
T{ SERVER @ SOCK-CLOSE -> 0 }T
T{ SERVER @ SOCK-CLOSE -> NET-CLOSED }T                   \ not open

\ ---- UDP
VARIABLE U1   VARIABLE U2
T{ 47013 UDP-OPEN SWAP U1 ! -> 0 }T
T{ 47014 UDP-OPEN SWAP U2 ! -> 0 }T
T{ BUF 100 U2 @ UDP-RECEIVE -> FALSE }T
T{ S" ping" 127 0 0 1 IP 47014 U1 @ UDP-SEND -> 0 }T
T{ 10 MS  BUF 100 U2 @ UDP-RECEIVE -> 4 127 0 0 1 IP 47013 TRUE }T
T{ BUF 4 S" ping" S= -> TRUE }T
T{ S" pong!" 127 0 0 1 IP 47013 U2 @ UDP-SEND -> 0 }T
T{ 10 MS  BUF 100 U1 @ UDP-RECEIVE -> 5 127 0 0 1 IP 47014 TRUE }T
T{ BUF 5 S" pong!" S= -> TRUE }T
T{ 239 255 0 1 IP U1 @ UDP-JOIN -> 0 }T
T{ 0 UDP-OPEN SWAP SOCK-CLOSE -> 0 0 }T                     \ any port
T{ U1 @ SOCK-CLOSE  U2 @ SOCK-CLOSE -> 0 0 }T

\ ---- DNS: a server in Forth on 127.0.0.1
ALSO DNS  ALSO DNS.I
VARIABLE FAKE
T{ 47015 UDP-OPEN SWAP FAKE ! -> 0 }T
127 0 0 1 IP DNS-IP !   47015 DNS-PORT !

512 BUFFER: Q          \ the server: the question that came, then the answer
VARIABLE QN   VARIABLE FROM-IP   VARIABLE FROM-PORT
VARIABLE R             \ the place in Q for the answer
VARIABLE TARGET        \ the place of the name in a CNAME record
: HEAR ( -- f )   \ the server gets a question, in at most 100 ms
  100 0 DO
    Q 512 FAKE @ UDP-RECEIVE IF  FROM-PORT ! FROM-IP ! QN !  TRUE UNLOOP EXIT  THEN  1 MS
  LOOP  FALSE ;
: R, ( c -- )    R @ C!  1 R +! ;
: RW, ( x -- )   R @ BE16!  2 R +! ;
: REPLY ( flags n -- )   \ the question back as the answer, with n records after it
  Q 6 + BE16!  Q 2 + BE16!  Q QN @ + R ! ;
: HEAD ( name type length -- )   >R >R RW, R> RW, 1 RW, 0 RW, 60 RW, R> RW, ;   \ class IN, 60 s
: A-RECORD ( name ip -- )   SWAP 1 4 HEAD  R @ BE32!  4 R +! ;
: SEND-REPLY ( -- )   Q  R @ Q -  FROM-IP @ FROM-PORT @ FAKE @ UDP-SEND DROP ;

: ASK-FOR ( a u -- )   \ the client: the first steps of DNS-QUERY
  DUP HOST-N !  HOST SWAP MOVE  0 UDP-OPEN DROP USOCK !  1 ID +!  ASK DROP ;
: GET ( -- ip ior )   \ the client: the last steps, with 200 ms for the answer
  MILLIS 200 + WAIT-ANSWER IF ANSWER ELSE 0 NET-TIMED-OUT THEN  USOCK @ SOCK-CLOSE DROP ;

\ the question
T{ S" www.example.org" ASK-FOR  HEAR -> TRUE }T
CREATE QNAME   3 C, 'w' C, 'w' C, 'w' C, 7 C, 'e' C, 'x' C, 'a' C, 'm' C, 'p' C, 'l' C, 'e' C,
               3 C, 'o' C, 'r' C, 'g' C, 0 C,  0 C, 1 C, 0 C, 1 C,
T{ QN @  Q 2 + BE16@  Q 4 + BE16@  Q 12 + 21 QNAME 21 S= -> 33 $0100 1 TRUE }T
T{ Q BE16@ ID @ = -> TRUE }T
T{ GET -> 0 NET-TIMED-OUT }T                   \ no answer

\ an address
T{ S" a.test" ASK-FOR  HEAR -> TRUE }T
$8180 1 REPLY  $C00C 10 0 0 1 IP A-RECORD  SEND-REPLY
T{ GET -> 10 0 0 1 IP 0 }T

\ a CNAME, and the address of its name
T{ S" www.test" ASK-FOR  HEAR -> TRUE }T
$8180 2 REPLY
$C00C 5 8 HEAD  R @ Q - TARGET !  1 R, 'b' R, 4 R, 't' R, 'e' R, 's' R, 't' R, 0 R,
$C000 TARGET @ OR  10 0 0 2 IP A-RECORD  SEND-REPLY
T{ GET -> 10 0 0 2 IP 0 }T

\ only a CNAME: its name is the next question; here with a pointer to "test" in the question
T{ S" c.test" ASK-FOR  HEAR -> TRUE }T
$8180 1 REPLY  $C00C 5 4 HEAD  1 R, 'd' R, $C0 R, 14 R,  SEND-REPLY
T{ GET  CNAME @ 0<> -> 0 NET-NO-HOST TRUE }T
T{ CNAME @ NAME>HOST  HOST HOST-N @ S" d.test" S= -> TRUE }T

\ no such name; an answer with another number
T{ S" x.test" ASK-FOR  HEAR -> TRUE }T
$8183 0 REPLY  SEND-REPLY
T{ GET  CNAME @ -> 0 NET-NO-HOST 0 }T
T{ S" a.test" ASK-FOR  HEAR -> TRUE }T
Q BE16@ 1+ Q BE16!  $8180 1 REPLY  $C00C 10 0 0 1 IP A-RECORD  SEND-REPLY
T{ GET -> 0 NET-TIMED-OUT }T

\ DNS-QUERY and RESOLVE: the server is silent; two questions in NET-TIMEOUT
: TIMED-QUERY ( -- ip ior ms )   MILLIS >R  S" a.test" RESOLVE  MILLIS R> - ;
T{ TIMED-QUERY 190 300 WITHIN -> 0 NET-TIMED-OUT TRUE }T
T{ HEAR HEAR HEAR -> TRUE TRUE FALSE }T
T{ FAKE @ SOCK-CLOSE -> 0 }T
0 DNS-IP !  53 DNS-PORT !  5000 NET-TIMEOUT !
PREVIOUS PREVIOUS
