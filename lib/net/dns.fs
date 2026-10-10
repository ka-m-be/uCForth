\ dns.fs - package DNS: the address of a host from its name
\
\ A question for the address (record A, IPv4) goes over UDP to a DNS
\ server (RFC 1035); the server asks further for us (recursion). An
\ answer can name another name (CNAME): then the next question asks for
\ that one, at most 4 times. No answer in half of NET-TIMEOUT: the
\ question goes again, once. The same code on each system: the resolver
\ of macOS and Linux is in their C library, which uCForth does not use.
\
\ Loading this package gives DNS-QUERY to RESOLVE (package NET):
\ after it, TCP-OPEN takes names too.
\
\   a u DNS-QUERY ( -- ip ior )   the address of the name a u
\   DNS-IP ( -- a )     a variable: the server; 0 (at the start): the
\                       server of the network (DNS-SERVER)
\   DNS-PORT ( -- a )   a variable: the port of the server (53)
\
\ Errors: NET-NO-HOST (the server has no address for the name),
\ NET-TIMED-OUT (no answer), NET-FAILED (another answer of the server),
\ and those of UDP-OPEN and UDP-SEND.
\
\ Example:  ALSO INET ALSO NET  S" example.org" RESOLVE THROW .IP

PACKAGE DNS  ALSO NET  ALSO INET  ALSO TEXT   \ lib/package.fs

PUBLIC
VARIABLE DNS-IP
VARIABLE DNS-PORT   53 DNS-PORT !

PRIVATE
512 BUFFER: MSG        \ the question, then the answer
VARIABLE P             \ the place in MSG
VARIABLE END           \ the end of the answer in MSG
VARIABLE ID            \ the number of the last question
256 BUFFER: HOST       \ the name to ask for
VARIABLE HOST-N
VARIABLE CNAME         \ the place of the name of a CNAME record in MSG; 0: none
VARIABLE USOCK         \ the UDP socket of the question

\ ---- the question
: B, ( c -- )   P @ C!  1 P +! ;
: W, ( x -- )   P @ BE16!  2 P +! ;
: LABEL-LENGTH ( a u -- n )   \ the bytes up to the first dot, or all
  >R 0  BEGIN  DUP R@ < IF  2DUP + C@ '.' <>  ELSE 0 THEN  WHILE  1+  REPEAT  NIP  R> DROP ;
: QNAME, ( a u -- )   \ the name as labels: each with its length first; then 0
  BEGIN  DUP WHILE
    2DUP LABEL-LENGTH  DUP B,                    ( a u n )
    >R  OVER P @ R@ MOVE  R@ P +!
    R> 1+ OVER MIN /STRING                        \ past the label and its dot
  REPEAT  2DROP  0 B, ;
: QUESTION ( -- a u )   \ the question for the address of HOST, in MSG
  MSG P !
  ID @ W,  $0100 W,  1 W,  0 W,  0 W,  0 W,      \ its number, recursion, one question
  HOST HOST-N @ QNAME,  1 W,  1 W,               \ the name, type A, class IN
  MSG  P @ MSG - ;
: SERVER ( -- ip )   DNS-IP @ ?DUP 0= IF DNS-SERVER THEN ;
: ASK ( -- ior )   QUESTION SERVER DNS-PORT @ USOCK @ UDP-SEND ;

\ ---- the answer
: IN? ( p -- p f )   DUP END @ U< ;   \ p is inside the answer
: NAME-END ( p -- p' )   \ past the name at p: labels up to 0, or a pointer of 2 bytes
  BEGIN  IN? WHILE
    DUP C@  DUP 0= IF  DROP 1+ EXIT  THEN
    $C0 AND $C0 = IF  2 + EXIT  THEN
    DUP C@ 1+ +
  REPEAT ;
: ANSWERS ( p n -- ip ior )   \ the n records at p: an address, or the place of a CNAME
  0 ?DO
    NAME-END  IN? 0= IF  LEAVE  THEN               \ type, class, time, length, data
    DUP BE16@ 1 =  OVER 8 + BE16@ 4 = AND IF  10 + BE32@ 0 UNLOOP EXIT  THEN
    DUP BE16@ 5 = IF  DUP 10 + CNAME !  THEN
    DUP 8 + BE16@ + 10 +
  LOOP  DROP  0 NET-NO-HOST ;
: ANSWER ( -- ip ior )   \ the address in the answer in MSG; or NET-NO-HOST and maybe CNAME
  0 CNAME !
  MSG 3 + C@ $0F AND  ?DUP IF  3 = IF NET-NO-HOST ELSE NET-FAILED THEN  0 SWAP EXIT  THEN
  MSG 12 +  MSG 4 + BE16@ 0 ?DO  NAME-END 4 +  LOOP     \ past the questions
  MSG 6 + BE16@ ANSWERS ;
: RECEIVED? ( -- f )   \ the answer to the question came: in MSG
  MSG 512 USOCK @ UDP-RECEIVE 0= IF FALSE EXIT THEN
  2DROP  DUP MSG + END !
  12 < IF FALSE EXIT THEN
  MSG BE16@ ID @ =  MSG 2 + C@ $80 AND 0<> AND ;     \ its number, and it is an answer
: WAIT-ANSWER ( t -- f )   \ until the answer comes, or the time t is over
  BEGIN  RECEIVED? IF  DROP TRUE EXIT  THEN  DUP MILLIS - 0 > WHILE  NET-POLL  REPEAT
  DROP FALSE ;
: TRY ( -- ip ior )   \ the question, a second time without an answer; then the answer
  ID @ MILLIS + 1+ $FFFF AND ID !                 \ a new number for each name
  2 0 DO
    ASK ?DUP IF  0 SWAP UNLOOP EXIT  THEN
    MILLIS NET-TIMEOUT @ 2/ +  WAIT-ANSWER IF  ANSWER UNLOOP EXIT  THEN
  LOOP  0 NET-TIMED-OUT ;

: NAME>HOST ( p -- )   \ the name at p in the answer, as text in HOST
  0 HOST-N !
  64 0 DO                                         \ at most 64 labels and pointers
    IN? 0= IF  LEAVE  THEN
    DUP C@  DUP 0= IF  DROP LEAVE  THEN
    DUP $C0 AND $C0 = IF  DROP BE16@ $3FFF AND MSG +
    ELSE
      DUP HOST-N @ + 250 > IF  DROP LEAVE  THEN
      HOST-N @ IF  '.' HOST HOST-N @ + C!  1 HOST-N +!  THEN
      >R 1+  DUP HOST HOST-N @ + R@ MOVE  R@ HOST-N +!  R> +
    THEN
  LOOP  DROP ;
: LOOKUP ( -- ip ior )   \ the address of HOST; a CNAME leads to its name, at most 4 times
  4 0 DO
    TRY  DUP NET-NO-HOST =  CNAME @ 0<> AND  0= IF  UNLOOP EXIT  THEN
    2DROP  CNAME @ NAME>HOST
  LOOP  0 NET-NO-HOST ;

PUBLIC
: DNS-QUERY ( a u -- ip ior )   \ the address of the name a u, from the server DNS-IP
  250 MIN  DUP HOST-N !  HOST SWAP MOVE
  0 UDP-OPEN ?DUP IF EXIT THEN  USOCK !
  ['] LOOKUP CATCH  USOCK @ SOCK-CLOSE DROP  THROW ;

' DNS-QUERY IS (RESOLVE)

END-PACKAGE
