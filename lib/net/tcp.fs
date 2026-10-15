\ tcp.fs - package TCP: connections over our TCP/IP
\
\ TCP (RFC 9293) in a small form, for the home network:
\ - 4 connections and 4 servers; each is a small block with the address
\   of the buffer of the program: its first half receives, its second
\   half sends. A buffer at the address 0 takes nothing.
\ - A server gives its buffer to one connection at a time: a new
\   connection takes a free place until TCP-ACCEPT gives it to the
\   program; while a connection has the buffer, a SYN for the server is
\   lost (it comes again).
\ - SOCK-CLOSE: the bytes that the other side has not acknowledged still
\   go, from the buffer; after their acknowledgement the buffer belongs
\   to the program again. A new connection with that buffer before then
\   ends the old one with RST.
\ - In order only: a segment after a gap is lost, and a second
\   acknowledgement tells the other side where the gap starts.
\ - Each segment with data gets its acknowledgement at once, and we send
\   at once (no Nagle). The only option is MSS: we tell 1460.
\ - The time to send again (RTO, RFC 6298): measured, at least 200 ms,
\   at first 1 s, doubled each time up to 60 s. After 8 times without an
\   answer the connection ends (NET-TIMED-OUT).
\ - On the way is at most the half to send; with 4 KB or less that is
\   less than the first window of RFC 5681. After a timeout: one
\   segment, then all again.
\ - A window of 0 from the other side: a test with one byte, again and
\   again, while the other side answers (RFC 9293).
\ - SOCK-CLOSE with bytes that the program did not read, and data for a
\   connection that the program closed, get RST, as in Linux.
\ - TIME-WAIT lasts 60 s; a new connection may take its place earlier.
\ No urgent data (the bytes come as normal bytes), no keep-alive.
\
\ The words have the names of the contract NETIF (lib/net/README.md);
\ the layer NETIF (lib/net/linknet.fs) gives them to the package NET:
\   a u ip port (TCP-CONNECT) ( -- h ior )   h (TCP-STATE) ( -- n )
\   a u port (TCP-LISTEN) ( -- h ior )   h (TCP-ACCEPT) ( -- h2 true | false )
\   a u h (SEND) ( -- u2 ior )           a u h (RECV) ( -- u2 ior )
\   h (AVAILABLE) ( -- n )               h (TCP-CLOSE) ( -- ior )
\   TCP-TICK ( -- )                      the timers: each step of the network
\ A connection is 1 to 4, a server 9 to 12.

PACKAGE TCP  ALSO IPV4  ALSO ARP  ALSO LINK  ALSO INET   \ lib/package.fs

4 CONSTANT #CONNS   4 CONSTANT #SERVERS
1 CONSTANT FIRST-CONN   9 CONSTANT FIRST-SERVER      \ the handles
6 CONSTANT PROTO-TCP
20 CONSTANT HEAD
LINK-MTU 40 - CONSTANT MY-MSS
536 CONSTANT MSS-START            \ the MSS of the other side, until its SYN tells it
1000 CONSTANT RTO-START   200 CONSTANT RTO-MIN   60000 CONSTANT RTO-MAX
8 CONSTANT #TRIES
60000 CONSTANT LINGER             \ TIME-WAIT; FIN-WAIT-2 after the program closed

\ the states of a connection; ENDED: an error ended it (T-ERROR), the program must close it
0 CONSTANT FREE        1 CONSTANT SYN-SENT     2 CONSTANT SYN-RCVD     3 CONSTANT ESTABLISHED
4 CONSTANT FIN-WAIT-1  5 CONSTANT FIN-WAIT-2   6 CONSTANT CLOSE-WAIT   7 CONSTANT CLOSING
8 CONSTANT LAST-ACK    9 CONSTANT TIME-WAIT   10 CONSTANT ENDED
\ the flags of a segment
1 CONSTANT FIN   2 CONSTANT SYN   4 CONSTANT RST   8 CONSTANT PSH   16 CONSTANT ACK

\ ---- the connections: T is the one that the words work on
VARIABLE T
: T-FIELD ( n "name" -- n+1 )   CREATE DUP CELLS ,  1+  DOES> @ T @ + ;
0 T-FIELD T-STATE    T-FIELD T-INDEX    T-FIELD T-LPORT    T-FIELD T-RIP      T-FIELD T-RPORT
  T-FIELD T-UNA      T-FIELD T-NXT      T-FIELD T-WND      T-FIELD T-MSS      T-FIELD T-RCV
  T-FIELD T-ADV      T-FIELD T-TXN      T-FIELD T-RXN      T-FIELD T-TIMER    T-FIELD T-RTO
  T-FIELD T-TRIES    T-FIELD T-SLOW     T-FIELD T-RTT-SEQ  T-FIELD T-RTT-AT   T-FIELD T-SRTT
  T-FIELD T-RTTVAR   T-FIELD T-FIN-SEQ  T-FIELD T-CLOSED   T-FIELD T-OWNED    T-FIELD T-SERVER
  T-FIELD T-ERROR    T-FIELD T-ACK-NOW  T-FIELD T-MAX      T-FIELD T-BUF      T-FIELD T-HALF
CELLS CONSTANT TCB-SIZE
\ T-UNA T-NXT: the first byte without acknowledgement, the next byte to send (RFC: SND.UNA,
\ SND.NXT); T-MAX: the highest T-NXT so far (after a timeout T-NXT goes back to T-UNA); T-WND T-MSS: the window and the MSS of the other side; T-RCV: the next byte to
\ receive (RCV.NXT); T-ADV: the right edge of the window that we told; T-TXN T-RXN: the bytes
\ in the buffers; T-TIMER: when the time to send again, to test the window or to free ends
\ (0: none); T-FIN-SEQ: the number of our FIN; T-CLOSED: the program closed it; T-OWNED: the
\ program has the handle; T-SERVER: the server of a connection that came (1 to 4), or 0;
\ T-BUF T-HALF: the buffer of the program (0: none) and the size of each half

#CONNS TCB-SIZE * BUFFER: TCBS
3 CELLS CONSTANT /SERVER          \ a server: its port (0: free), its buffer, the size of it
#SERVERS /SERVER * BUFFER: SERVERS
TCBS #CONNS TCB-SIZE * 0 FILL   SERVERS #SERVERS /SERVER * 0 FILL   \ BUFFER: does not clear
VARIABLE NEXT-PORT

: USE ( i -- )   TCB-SIZE * TCBS + T ! ;
: NUMBER-THEM ( -- )   #CONNS 0 DO  I USE  I T-INDEX !  LOOP ;
NUMBER-THEM
: RX-BUF ( -- a )   T-BUF @ ;
: TX-BUF ( -- a )   T-BUF @ T-HALF @ + ;
: HALF ( -- n )   T-HALF @ ;
: BUFFER! ( a u -- )   \ T gets the buffer a u; a 0: no room
  OVER 0= IF DROP 0 THEN  2/ T-HALF !  T-BUF ! ;
: HANDLE ( -- h )   T-INDEX @ FIRST-CONN + ;
: RELEASE ( -- )   T-INDEX @  T @ TCB-SIZE 0 FILL  T-INDEX ! ;   \ T is free again
: SEQ< ( a b -- f )   - 0< ;                                   \ numbers of 32 bits that wrap
: SEQ<= ( a b -- f )   - 1 < ;
: TIMER-AT ( ms -- )   MILLIS + 1 OR T-TIMER ! ;                \ never 0: 0 is "no timer"
: RESTART-TIMER ( -- )   T-RTO @ TIMER-AT ;
: START-TIMER ( -- )   T-TIMER @ 0= IF RESTART-TIMER THEN ;
: MAX-UP ( -- )   T-MAX @ T-NXT @ SEQ< IF  T-NXT @ T-MAX !  THEN ;   \ T-NXT went further than ever

\ ---- the segment that came
VARIABLE SEG   VARIABLE SEG-HL   VARIABLE SEG-LEN   VARIABLE SEG-DATA
VARIABLE SEG-SPORT   VARIABLE SEG-DPORT   VARIABLE SEG-SEQ   VARIABLE SEG-ACK
VARIABLE SEG-FLAGS   VARIABLE SEG-WND
: FLAG? ( flag -- f )   SEG-FLAGS @ AND 0<> ;
: PARSE ( a u -- f )   \ the head of the segment a u; false: too short or a wrong checksum
  DUP HEAD < IF 2DROP FALSE EXIT THEN
  OVER 12 + C@ 4 RSHIFT 4 * SEG-HL !
  SEG-HL @ HEAD <  OVER SEG-HL @ < OR IF 2DROP FALSE EXIT THEN
  DUP >R  IP-SRC @ IP-DST @ PROTO-TCP R> IP-PSEUDO
  >R 2DUP R> ROT ROT IP-SUM IP-FOLD IF 2DROP FALSE EXIT THEN
  OVER BE16@ SEG-SPORT !  OVER 2 + BE16@ SEG-DPORT !
  OVER 4 + BE32@ SEG-SEQ !  OVER 8 + BE32@ SEG-ACK !
  OVER 13 + C@ SEG-FLAGS !  OVER 14 + BE16@ SEG-WND !
  SEG-HL @ - SEG-LEN !  DUP SEG-HL @ + SEG-DATA !  SEG !  TRUE ;
: PEER-MSS ( -- n )   \ the option MSS of the SYN that came; else 536
  SEG @ SEG-HL @ +  SEG @ HEAD +                       ( end p )
  BEGIN  2DUP SWAP U<  WHILE
    DUP C@ 0= IF  2DROP MSS-START EXIT  THEN          \ the end of the options
    DUP C@ 1 = IF  1+  ELSE                           \ no operation
      DUP C@ 2 = IF  NIP 2 + BE16@ 64 MAX MY-MSS MIN EXIT  THEN
      DUP 1+ C@ 2 MAX +                               \ another option: past it
    THEN
  REPEAT  2DROP MSS-START ;

\ ---- segments that go
VARIABLE DEST
VARIABLE S-SEQ   VARIABLE S-N   VARIABLE S-FLAGS   VARIABLE PART
: SEND-TO ( len ip -- ior )   \ the segment at IP-DATA with len bytes to ip
  DEST !  >R  0 IP-DATA 16 + BE16!
  DEST @ IP-SOURCE  DEST @  PROTO-TCP R@ IP-PSEUDO  IP-DATA R@ IP-SUM IP-FOLD  IP-DATA 16 + BE16!
  PROTO-TCP R> DEST @ IP-SEND ;
: FREE-ROOM ( -- n )   HALF T-RXN @ - ;
: BIG-STEP ( -- n )   HALF 2/ MY-MSS MIN ;
: WINDOW ( -- n )   \ the window to tell; its right edge grows only in big steps (no "silly window")
  T-RCV @ FREE-ROOM +  T-ADV @ -  BIG-STEP < 0= IF  T-RCV @ FREE-ROOM + T-ADV !  THEN
  T-ADV @ T-RCV @ -  0 MAX ;
: HEAD-LENGTH ( flags -- n )   SYN AND IF 24 ELSE 20 THEN ;   \ a SYN has the option MSS
: TCP-HEAD! ( seq flags hl -- )   \ the head of a segment of T at IP-DATA
  T-LPORT @ IP-DATA BE16!  T-RPORT @ IP-DATA 2 + BE16!
  >R  SWAP IP-DATA 4 + BE32!  T-RCV @ IP-DATA 8 + BE32!
  R@ 2 RSHIFT 12 LSHIFT OR  IP-DATA 12 + BE16!          \ the length of the head in words, the flags
  WINDOW IP-DATA 14 + BE16!  0 IP-DATA 16 + BE32!       \ the checksum and the urgent pointer 0
  R> 24 = IF  $02040000 MY-MSS OR  IP-DATA 20 + BE32!  THEN ;
: SEGMENT ( seq n flags -- ior )   \ a segment of T: n bytes of the buffer to send, from seq on
  S-FLAGS !  S-N !  S-SEQ !
  S-FLAGS @ HEAD-LENGTH >R
  S-SEQ @ S-FLAGS @ R@ TCP-HEAD!
  S-N @ IF  TX-BUF S-SEQ @ T-UNA @ - +  IP-DATA R@ +  S-N @ MOVE  THEN
  R> S-N @ +  T-RIP @ SEND-TO ;
: REFUSE ( -- )   \ RST for the segment that came, without a connection (RFC 9293)
  RST FLAG? IF EXIT THEN
  SEG-DPORT @ IP-DATA BE16!  SEG-SPORT @ IP-DATA 2 + BE16!
  ACK FLAG? IF  SEG-ACK @  0  RST
  ELSE  0  SEG-SEQ @ SEG-LEN @ +  SYN FLAG? -  FIN FLAG? -  RST ACK OR  THEN   \ SYN and FIN count one each
  $5000 OR IP-DATA 12 + BE16!  IP-DATA 8 + BE32!  IP-DATA 4 + BE32!
  0 IP-DATA 14 + BE32!  0 IP-DATA 18 + BE16!
  HEAD IP-SRC @ SEND-TO DROP ;

\ ---- what T sends
: LIVE? ( -- f )   T-STATE @ DUP FREE <> SWAP ENDED <> AND ;
: IN-FLIGHT ( -- n )   T-NXT @ T-UNA @ - ;
: UNSENT ( -- n )   T-UNA @ T-TXN @ +  T-NXT @ -  0 MAX ;    \ data not yet sent
: SEND-ROOM ( -- n )   \ the bytes that the window lets go now; after a timeout one segment
  T-SLOW @ IF  IN-FLIGHT IF 0 EXIT THEN  THEN
  T-UNA @ T-WND @ +  T-NXT @ -  0 MAX ;
: CAN-SEND? ( -- f )   \ the handshake is done, and data or our FIN may still go
  T-STATE @ DUP ESTABLISHED =  OVER CLOSE-WAIT = OR  OVER FIN-WAIT-1 = OR
  OVER CLOSING = OR  SWAP LAST-ACK = OR ;
: DATA-FLAGS ( n -- flags )   \ ACK PSH, and FIN with the last bytes before it
  T-NXT @ +  T-FIN-SEQ @ =  T-CLOSED @ AND  IF  ACK PSH OR FIN OR  ELSE  ACK PSH OR  THEN ;
: TIMING ( -- )   \ measure the time to the acknowledgement of this segment (RFC 6298, Karn)
  T-RTT-AT @ 0=  T-SLOW @ 0= AND IF  T-NXT @ T-RTT-SEQ !  MILLIS 1 OR T-RTT-AT !  THEN ;
: SEND-DATA ( -- )   \ the data that the window lets go, in segments of at most MSS
  BEGIN  UNSENT SEND-ROOM MIN T-MSS @ MIN  DUP PART !  0 >  WHILE
    T-NXT @  PART @  PART @ DATA-FLAGS  SEGMENT DROP
    PART @ T-NXT +!
    T-CLOSED @  T-NXT @ T-FIN-SEQ @ = AND IF  1 T-NXT +!  THEN   \ the FIN went with them
    MAX-UP  TIMING  START-TIMER
  REPEAT ;
: SEND-SYN ( flags -- ior )   T-UNA @ 0 ROT SEGMENT  1 T-NXT +!  MAX-UP  START-TIMER ;
: OUTPUT ( -- )   \ what T may send now: SYN, data, FIN, or at least an acknowledgement
  LIVE? 0= IF EXIT THEN
  T-STATE @ SYN-SENT =  T-NXT @ T-UNA @ = AND IF  SYN SEND-SYN DROP  EXIT  THEN
  T-STATE @ SYN-RCVD =  T-NXT @ T-UNA @ = AND IF  SYN ACK OR SEND-SYN DROP  EXIT  THEN
  CAN-SEND? IF
    T-NXT @ >R  SEND-DATA
    T-CLOSED @  T-NXT @ T-FIN-SEQ @ = AND IF                 \ no data left: the FIN alone
      T-NXT @ 0 ACK FIN OR SEGMENT DROP  1 T-NXT +!  MAX-UP  START-TIMER
    THEN
    T-NXT @ R> <> IF  T-ACK-NOW OFF!  THEN                  \ a segment went, with the ACK
    T-TIMER @ 0=  UNSENT 0 > AND IF  RESTART-TIMER  THEN    \ the window is 0: test it later
  THEN
  T-ACK-NOW @ IF  T-NXT @ 0 ACK SEGMENT DROP  T-ACK-NOW OFF!  THEN ;
: ABORT-CONN ( -- )   T-NXT @ 0 RST ACK OR SEGMENT DROP  RELEASE ;
: END-CONN ( ior -- )   \ an error ends T: the program sees it, or T is free
  T-OWNED @  T-CLOSED @ 0= AND IF  T-ERROR !  ENDED T-STATE !  0 T-TIMER !
  ELSE  DROP RELEASE  THEN ;

\ ---- the segment that came, for T
: RTT-SAMPLE ( -- )   \ RFC 6298: SRTT and RTTVAR from one measured time, then RTO
  T-RTT-AT @ 0= IF EXIT THEN
  SEG-ACK @ T-RTT-SEQ @ SEQ< IF EXIT THEN
  MILLIS T-RTT-AT @ -  0 T-RTT-AT !                     ( r )
  T-SRTT @ 0= IF  DUP T-SRTT !  2/ T-RTTVAR !
  ELSE  DUP T-SRTT @ - ABS  T-RTTVAR @ 3 * +  4 / T-RTTVAR !
        T-SRTT @ 7 * +  8 / T-SRTT !  THEN
  T-SRTT @  T-RTTVAR @ 4 * 1 MAX +  RTO-MIN MAX RTO-MAX MIN  T-RTO ! ;
: ACKED ( n -- )   \ n bytes at the start of the buffer to send arrived
  ?DUP 0= IF EXIT THEN
  >R  TX-BUF R@ +  TX-BUF  T-TXN @ R@ -  MOVE  R> NEGATE T-TXN +! ;
: FIN-ACKED? ( -- f )   T-CLOSED @  T-UNA @ T-FIN-SEQ @ 1+ = AND ;
: ?GIVE-BACK ( -- )   \ closed, and all bytes arrived: the buffer belongs to the program again
  T-CLOSED @  T-TXN @ 0= AND IF  0 0 BUFFER!  THEN ;
: TAKE-ACK ( -- f )   \ the acknowledgement: data and FIN that arrived, the window; false: drop the segment
  SEG-ACK @ T-MAX @ - 0 > IF  T-ACK-NOW ON!  FALSE EXIT  THEN      \ for bytes that we never sent
  SEG-WND @ T-WND !  0 T-TRIES !                              \ the other side lives
  T-NXT @ SEG-ACK @ SEQ< IF  SEG-ACK @ T-NXT !  THEN          \ they came before we sent them again
  SEG-ACK @ T-UNA @ -  DUP 1 < IF DROP TRUE EXIT THEN        ( n )
  DUP T-TXN @ MIN ACKED  T-UNA +!  T-SLOW OFF!  RTT-SAMPLE  ?GIVE-BACK
  T-UNA @ T-NXT @ = IF  0 T-TIMER !  ELSE  RESTART-TIMER  THEN
  FIN-ACKED? IF
    T-STATE @ CASE
      FIN-WAIT-1 OF  FIN-WAIT-2 T-STATE !  LINGER TIMER-AT  ENDOF
      CLOSING OF  TIME-WAIT T-STATE !  LINGER TIMER-AT  ENDOF
      LAST-ACK OF  RELEASE  ENDOF
    ENDCASE
  THEN  TRUE ;
: RECEIVING? ( -- f )   T-STATE @ DUP ESTABLISHED =  OVER FIN-WAIT-1 = OR  SWAP FIN-WAIT-2 = OR ;
: TAKE-DATA ( -- )   \ the data of the segment into the buffer, as far as there is room
  SEG-LEN @ 0=  RECEIVING? 0= OR IF EXIT THEN
  T-CLOSED @ IF  ABORT-CONN EXIT  THEN                        \ the program closed it: RST, as Linux
  SEG-LEN @ FREE-ROOM MIN >R
  SEG-DATA @  RX-BUF T-RXN @ +  R@ MOVE
  R@ T-RXN +!  R> T-RCV +!  T-ACK-NOW ON! ;
: TAKE-FIN ( -- )   \ the FIN of the other side, when all its data came
  FIN FLAG? 0= IF EXIT THEN
  SEG-SEQ @ SEG-LEN @ +  T-RCV @ <> IF EXIT THEN
  1 T-RCV +!  T-ACK-NOW ON!
  T-STATE @ CASE
    ESTABLISHED OF  CLOSE-WAIT T-STATE !  ENDOF
    FIN-WAIT-1 OF  CLOSING T-STATE !  ENDOF
    FIN-WAIT-2 OF  TIME-WAIT T-STATE !  LINGER TIMER-AT  ENDOF
  ENDCASE ;
: TRIM ( -- )   \ cut the bytes before RCV.NXT: they came already
  T-RCV @ SEG-SEQ @ -  DUP 1 < IF DROP EXIT THEN             ( d )
  T-ACK-NOW ON!                                               \ it came again: our ACK was lost
  SYN FLAG? IF  1-  SEG-FLAGS @ SYN INVERT AND SEG-FLAGS !  1 SEG-SEQ +!  THEN
  SEG-LEN @ MIN  DUP SEG-DATA +!  DUP NEGATE SEG-LEN +!  SEG-SEQ +! ;
: IN-SYN-SENT ( -- )   \ our SYN went; the answer
  ACK FLAG? IF  SEG-ACK @ T-UNA @ 1+ <> IF  REFUSE EXIT  THEN  THEN
  RST FLAG? IF  ACK FLAG? IF  NET-REFUSED END-CONN  THEN  EXIT  THEN
  SYN FLAG? 0= IF EXIT THEN
  SEG-SEQ @ 1+ T-RCV !  T-RCV @ HALF + T-ADV !  SEG-WND @ T-WND !  PEER-MSS T-MSS !
  ACK FLAG? IF
    1 T-UNA +!  ESTABLISHED T-STATE !  0 T-TIMER !  0 T-TRIES !  T-ACK-NOW ON!
  ELSE  SYN-RCVD T-STATE !  T-UNA @ T-NXT !  THEN    \ both sides opened at once: our SYN again, with ACK
  ;
: IN-OTHER ( -- )   \ T has its SYN-ACK, or comes from a server
  RST FLAG? IF  SEG-SEQ @ T-RCV @ = IF  NET-CLOSED END-CONN  THEN  EXIT  THEN
  T-STATE @ SYN-RCVD =  SYN FLAG? AND  ACK FLAG? 0= AND IF  T-UNA @ T-NXT !  EXIT  THEN   \ its SYN again
  TRIM
  SEG-SEQ @ T-RCV @ <>  SYN FLAG? OR IF  T-ACK-NOW ON!  EXIT  THEN      \ a gap, old, or a SYN: only an ACK
  ACK FLAG? 0= IF EXIT THEN
  T-STATE @ SYN-RCVD = IF
    SEG-ACK @ T-UNA @ 1+ <> IF  REFUSE EXIT  THEN
    ESTABLISHED T-STATE !
  THEN
  TAKE-ACK 0= IF EXIT THEN
  T-STATE @ IF  TAKE-DATA  THEN  T-STATE @ IF  TAKE-FIN  THEN ;

\ ---- servers, ports
: SRV ( s -- a )   /SERVER * SERVERS + ;   \ the block of server s: the port, the buffer, its size
: SERVER-AT ( port -- i true | false )
  #SERVERS 0 DO  DUP I SRV @ = IF DROP I TRUE UNLOOP EXIT THEN  LOOP  DROP FALSE ;
: HAS? ( a -- f )   DUP 0<>  SWAP T-BUF @ = AND ;   \ T has the buffer a (a free T has none)
: HELD? ( a -- f )   \ a connection has the buffer a
  FALSE  #CONNS 0 DO  I USE  OVER HAS? OR  LOOP  NIP ;
: TAKE-BACK ( a -- )   \ a connection that still has the buffer a: RST, and its place is free
  #CONNS 0 DO  I USE  DUP HAS? IF ABORT-CONN THEN  LOOP  DROP ;
: FIND-CONN ( -- f )   \ the connection of the segment that came: T
  #CONNS 0 DO
    I USE  LIVE?  T-LPORT @ SEG-DPORT @ = AND  T-RPORT @ SEG-SPORT @ = AND  T-RIP @ IP-SRC @ = AND
    IF TRUE UNLOOP EXIT THEN
  LOOP  FALSE ;
: FREE-CONN ( -- f )   \ a free connection: T; or one in TIME-WAIT, freed; false: none
  #CONNS 0 DO  I USE  T-STATE @ FREE = IF TRUE UNLOOP EXIT THEN  LOOP
  #CONNS 0 DO  I USE  T-STATE @ TIME-WAIT = IF  RELEASE TRUE UNLOOP EXIT  THEN  LOOP
  FALSE ;
: PORT-USED? ( port -- f )   \ a server or a connection has the port here
  DUP SERVER-AT IF  2DROP TRUE EXIT  THEN
  #CONNS 0 DO  I USE  T-STATE @ IF  DUP T-LPORT @ = IF DROP TRUE UNLOOP EXIT THEN  THEN  LOOP
  DROP FALSE ;
: FREE-PORT ( -- port )   \ a port from 49152 on that nothing has here
  NEXT-PORT @ 0= IF  IP-RANDOM 16383 AND 49152 + NEXT-PORT !  THEN
  BEGIN
    NEXT-PORT @ 1+  DUP 65535 > IF DROP 49152 THEN  DUP NEXT-PORT !
    DUP PORT-USED?
  WHILE  DROP  REPEAT ;
: START-CONN ( -- )   \ the own numbers, the timer, empty buffers
  IP-RANDOM DUP T-UNA !  DUP T-NXT !  T-MAX !  0 T-TXN !  0 T-RXN !
  RTO-START T-RTO !  0 T-TRIES !  0 T-TIMER !  0 T-RTT-AT !  0 T-SRTT !  0 T-RTTVAR ! ;
: NEW-CONN ( -- )   \ a segment without a connection: a SYN for a server makes one; else RST
  SYN FLAG? 0=  ACK FLAG? OR  RST FLAG? OR IF  REFUSE EXIT  THEN
  SEG-DPORT @ SERVER-AT 0= IF  REFUSE EXIT  THEN              ( s )
  DUP SRV CELL+ @ HELD? IF  DROP EXIT  THEN                   \ its buffer is busy: the SYN is lost, it comes again
  FREE-CONN 0= IF  DROP EXIT  THEN                            \ no place: the same
  DUP SRV CELL+ DUP @ SWAP CELL+ @ BUFFER!
  1+ T-SERVER !  SYN-RCVD T-STATE !
  SEG-DPORT @ T-LPORT !  IP-SRC @ T-RIP !  SEG-SPORT @ T-RPORT !
  START-CONN
  SEG-SEQ @ 1+ T-RCV !  T-RCV @ HALF + T-ADV !  SEG-WND @ T-WND !  PEER-MSS T-MSS !
  OUTPUT ;
: TCP-INPUT ( a u -- )
  PARSE 0= IF EXIT THEN
  FIND-CONN 0= IF  NEW-CONN EXIT  THEN
  T-STATE @ SYN-SENT = IF  IN-SYN-SENT  ELSE  IN-OTHER  THEN
  OUTPUT ;
' TCP-INPUT IS IP>TCP

\ ---- the timers
: RESEND ( -- )   \ no acknowledgement in time: again from the first byte without one
  1 T-TRIES +!
  T-TRIES @ #TRIES > IF  T-NXT @ 0 RST SEGMENT DROP  NET-TIMED-OUT END-CONN  EXIT  THEN
  T-RTO @ 2* RTO-MAX MIN T-RTO !  T-SLOW ON!  0 T-RTT-AT !
  T-UNA @ T-NXT !  OUTPUT ;
: PROBE ( -- )   \ the window of the other side is 0: one byte as a question
  T-NXT @ 1 ACK SEGMENT DROP  1 T-NXT +!  MAX-UP  RESTART-TIMER ;
: DUE? ( -- f )   T-TIMER @ DUP IF  MILLIS SWAP - 0< 0=  THEN ;
: TICK-ONE ( -- )
  DUE? 0= IF EXIT THEN  0 T-TIMER !
  T-STATE @ DUP TIME-WAIT =  SWAP FIN-WAIT-2 = OR IF  RELEASE EXIT  THEN
  IN-FLIGHT IF  RESEND EXIT  THEN
  UNSENT IF  PROBE  THEN ;

PUBLIC
: TCP-TICK ( -- )   #CONNS 0 DO  I USE  T-STATE @ IF TICK-ONE THEN  LOOP ;
: TCP-DOWN ( -- )
  #CONNS 0 DO I USE T-STATE @ IF NET-UNREACHABLE END-CONN THEN LOOP ;

PRIVATE
: CONN? ( h -- f )   \ h is a connection of the program: T
  FIRST-CONN -  DUP #CONNS U< 0= IF DROP FALSE EXIT THEN
  USE  T-STATE @ 0<>  T-OWNED @ AND  T-CLOSED @ 0= AND ;
: SERVER? ( h -- s true | false )   \ h is a server that listens
  FIRST-SERVER -  DUP #SERVERS U< IF  DUP SRV @ IF TRUE EXIT THEN  THEN  DROP FALSE ;
: START-CLOSE ( -- )   \ the FIN goes after the data
  T-CLOSED ON!  T-UNA @ T-TXN @ + T-FIN-SEQ !
  T-STATE @ ESTABLISHED = IF FIN-WAIT-1 ELSE LAST-ACK THEN T-STATE !
  ?GIVE-BACK  OUTPUT ;
: CLOSE-SERVER ( s -- )   \ connections that came and wait for TCP-ACCEPT get RST
  DUP SRV /SERVER 0 FILL  1+
  #CONNS 0 DO  I USE  T-SERVER @ OVER =  T-OWNED @ 0= AND  T-STATE @ 0<> AND IF  ABORT-CONN  THEN  LOOP
  DROP ;
: WINDOW-UPDATE ( -- )   \ after reading: tell a much bigger window
  T-RCV @ FREE-ROOM +  T-ADV @ -  BIG-STEP < 0= IF  T-ACK-NOW ON!  OUTPUT  THEN ;

PUBLIC
: (TCP-CONNECT) ( a u ip port -- h ior )   \ a u: the buffer
  >R >R  OVER TAKE-BACK  R> R>
  FREE-PORT >R
  FREE-CONN 0= IF  R> DROP 2DROP 2DROP 0 NET-FAILED EXIT  THEN
  T-RPORT !  T-RIP !  R> T-LPORT !  BUFFER!
  START-CONN  SYN-SENT T-STATE !  T-OWNED ON!  MSS-START T-MSS !
  SYN SEND-SYN  ?DUP IF  RELEASE 0 SWAP EXIT  THEN          \ no way to the host
  HANDLE 0 ;
: (TCP-STATE) ( h -- n )   \ 1: it stands, or bytes wait; 0: it comes; else the ior why it ended
  CONN? 0= IF NET-CLOSED EXIT THEN
  T-RXN @ IF 1 EXIT THEN
  T-STATE @ CASE
    SYN-SENT OF 0 ENDOF
    SYN-RCVD OF 0 ENDOF
    ESTABLISHED OF 1 ENDOF
    ENDED OF T-ERROR @ ENDOF
    >R NET-CLOSED R>                                       \ CLOSE-WAIT: the other side ended it
  ENDCASE ;
: (TCP-LISTEN) ( a u port -- h ior )   \ a u: the buffer for its connections, one at a time
  DUP SERVER-AT IF  2DROP 2DROP 0 NET-IN-USE EXIT  THEN
  0 SERVER-AT 0= IF  DROP 2DROP 0 NET-FAILED EXIT  THEN    ( a u port s )
  >R  R@ SRV !  R@ SRV 2 CELLS + !  R@ SRV CELL+ !  R> FIRST-SERVER + 0 ;
: (TCP-ACCEPT) ( h -- h2 true | false )   \ a connection of the server that the program has not
  SERVER? 0= IF FALSE EXIT THEN  1+
  #CONNS 0 DO
    I USE  T-SERVER @ OVER =  T-OWNED @ 0= AND
    T-STATE @ DUP ESTABLISHED = SWAP CLOSE-WAIT = OR AND IF
      DROP  T-OWNED ON!  HANDLE TRUE UNLOOP EXIT
    THEN
  LOOP  DROP FALSE ;
: (SEND) ( a u h -- u2 ior )   \ as many bytes as the buffer takes now
  CONN? 0= IF  2DROP 0 NET-CLOSED EXIT  THEN
  T-STATE @ ENDED = IF  2DROP 0 T-ERROR @ EXIT  THEN
  T-STATE @ ESTABLISHED < IF  2DROP 0 0 EXIT  THEN          \ the connection comes
  HALF T-TXN @ - MIN                                        ( a n )
  TUCK  TX-BUF T-TXN @ +  SWAP MOVE                         ( n )
  DUP T-TXN +!  OUTPUT  0 ;
: (RECV) ( a u h -- u2 ior )   \ the bytes that are there; 0 0 also at the end
  CONN? 0= IF  2DROP 0 NET-CLOSED EXIT  THEN
  T-RXN @ MIN                                               ( a n )
  ?DUP 0= IF  DROP 0  T-STATE @ ENDED = IF T-ERROR @ ELSE 0 THEN  EXIT  THEN
  TUCK  RX-BUF ROT ROT MOVE                                 ( n )
  DUP >R  RX-BUF R@ +  RX-BUF  T-RXN @ R@ -  MOVE  R> NEGATE T-RXN +!
  WINDOW-UPDATE  0 ;
: (AVAILABLE) ( h -- n )   CONN? IF T-RXN @ ELSE 0 THEN ;
: (TCP-CLOSE) ( h -- ior )
  DUP SERVER? IF  NIP CLOSE-SERVER 0 EXIT  THEN
  CONN? 0= IF NET-CLOSED EXIT THEN
  T-STATE @ DUP SYN-SENT =  SWAP ENDED = OR IF  RELEASE 0 EXIT  THEN
  T-RXN @ IF  ABORT-CONN  ELSE  START-CLOSE  THEN  0 ;

END-PACKAGE
