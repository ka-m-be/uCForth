\ udp.fs - package UDP: datagrams over our TCP/IP
\
\ UDP (RFC 768): a head of 8 bytes (the ports of sender and receiver,
\ the length, a checksum), then the data. 4 sockets; each has a port and
\ a buffer of 1.5 KB for the datagrams that came and wait. A datagram
\ that finds no room is lost, as UDP allows. Port 0 at (UDP-OPEN): a
\ free port from 49152 on.
\
\ The words have the names of the contract NETIF (lib/net/README.md);
\ the layer NETIF (lib/net/linknet.fs) gives them to the package NET:
\
\   port (UDP-OPEN) ( -- h ior )
\   a u ip port h (SENDTO) ( -- ior )   at most 1472 bytes
\   a u h (RECVFROM) ( -- u2 ip port true | false )
\   ip h (JOIN) ( -- ior )
\   h (UDP-CLOSE) ( -- ior )
\   h UDP-HANDLE? ( -- f )   h is a handle of UDP (17 to 20)

PACKAGE UDP  ALSO IPV4  ALSO ARP  ALSO LINK  ALSO INET   \ lib/package.fs

4 CONSTANT #SOCKETS
1536 CONSTANT ROOM              \ the buffer of a socket: each datagram with 8 bytes before it
17 CONSTANT FIRST               \ the handle of socket 0
8 CONSTANT HEAD
17 CONSTANT PROTO-UDP

#SOCKETS CELLS BUFFER: PORTS    \ the port of each socket; 0: free
#SOCKETS CELLS BUFFER: FILLED   \ the bytes in its buffer
PORTS #SOCKETS CELLS 0 FILL   FILLED #SOCKETS CELLS 0 FILL   \ BUFFER: does not clear
#SOCKETS ROOM * BUFFER: ROOMS   \ each datagram: its length (2 bytes), the IP and the port of its sender, the data
VARIABLE NEXT-PORT
VARIABLE TO-IP
VARIABLE IN-I   VARIABLE AT

: PORT@ ( i -- a )   CELLS PORTS + ;
: FILL@ ( i -- a )   CELLS FILLED + ;
: ROOM@ ( i -- a )   ROOM * ROOMS + ;
: SOCKET? ( h -- i true | false )   \ the socket of the handle h, if it is open
  FIRST -  DUP #SOCKETS U< IF  DUP PORT@ @ IF TRUE EXIT THEN  THEN  DROP FALSE ;
: BOUND? ( port -- f )   FALSE  #SOCKETS 0 DO  OVER I PORT@ @ = OR  LOOP  NIP ;
: OWNER ( port -- i true | false )   \ the socket at port
  #SOCKETS 0 DO  DUP I PORT@ @ = IF DROP I TRUE UNLOOP EXIT THEN  LOOP  DROP FALSE ;
: FREE-PORT ( -- port )   \ a port from 49152 on that no socket has
  NEXT-PORT @ 0= IF  IP-RANDOM 16383 AND 49152 + NEXT-PORT !  THEN
  BEGIN
    NEXT-PORT @ 1+  DUP 65535 > IF DROP 49152 THEN  DUP NEXT-PORT !
    DUP BOUND?
  WHILE  DROP  REPEAT ;
: CHECKSUM ( len -- )   \ the checksum of the datagram at IP-DATA with len bytes, to TO-IP
  >R  TO-IP @ IP-SOURCE  TO-IP @  PROTO-UDP R@ IP-PSEUDO  IP-DATA R> IP-SUM IP-FOLD
  ?DUP 0= IF $FFFF THEN  IP-DATA 6 + BE16! ;               \ 0 means: no checksum
: TAKE ( i -- )   \ the first datagram out of the buffer of socket i
  DUP ROOM@ DUP BE16@ 8 + >R                               ( i room ) ( R: n )
  DUP R@ + SWAP                                            ( i room+n room )
  ROT FILL@ DUP @ R@ - DUP >R SWAP !                       ( room+n room ) ( R: n rest )
  R> MOVE  R> DROP ;
: BAD? ( a u -- f )   \ too short, or a wrong checksum
  DUP HEAD < IF 2DROP TRUE EXIT THEN
  OVER 4 + BE16@ TUCK <  OVER HEAD < OR IF 2DROP TRUE EXIT THEN     ( a len )
  OVER 6 + BE16@ 0= IF 2DROP FALSE EXIT THEN
  >R  IP-SRC @ IP-DST @ PROTO-UDP R@ IP-PSEUDO  SWAP R> IP-SUM IP-FOLD 0<> ;
: UDP-INPUT ( a u -- )   \ a datagram that came: into the buffer of its socket
  2DUP BAD? IF 2DROP EXIT THEN
  DROP  DUP 2 + BE16@ OWNER 0= IF DROP EXIT THEN  IN-I !       ( a )
  DUP 4 + BE16@ HEAD -                                         ( a n )
  IN-I @ FILL@ @  OVER + 8 +  ROOM > IF 2DROP EXIT THEN        \ no room: the datagram is lost
  IN-I @ ROOM@  IN-I @ FILL@ @ +  AT !
  DUP AT @ BE16!  IP-SRC @ AT @ 2 + BE32!  OVER BE16@ AT @ 6 + BE16!
  DUP 8 + IN-I @ FILL@ +!
  SWAP HEAD +  AT @ 8 +  ROT MOVE ;
' UDP-INPUT IS IP>UDP

PUBLIC
: UDP-HANDLE? ( h -- f )   FIRST -  #SOCKETS U< ;
: UDP-FORGET ( -- ) FILLED #SOCKETS CELLS 0 FILL ;
: (UDP-OPEN) ( port -- h ior )
  ?DUP 0= IF FREE-PORT THEN
  DUP BOUND? IF DROP 0 NET-IN-USE EXIT THEN
  #SOCKETS 0 DO
    I PORT@ @ 0= IF  I PORT@ !  0 I FILL@ !  I FIRST + 0 UNLOOP EXIT  THEN
  LOOP  DROP 0 NET-FAILED ;
: (UDP-CLOSE) ( h -- ior )   SOCKET? 0= IF NET-CLOSED EXIT THEN  0 SWAP PORT@ !  0 ;
: (SENDTO) ( a u ip port h -- ior )
  SOCKET? 0= IF 2DROP 2DROP NET-CLOSED EXIT THEN             ( a u ip port i )
  PORT@ @ IP-DATA BE16!  IP-DATA 2 + BE16!  TO-IP !          ( a u )
  DUP LINK-MTU 28 - > IF 2DROP NET-FAILED EXIT THEN          \ more than one frame
  TUCK IP-DATA HEAD + SWAP MOVE  HEAD +                      ( len )
  DUP IP-DATA 4 + BE16!  0 IP-DATA 6 + BE16!  DUP CHECKSUM
  PROTO-UDP SWAP TO-IP @ IP-SEND ;
: (RECVFROM) ( a u h -- u2 ip port true | false )
  SOCKET? 0= IF 2DROP FALSE EXIT THEN                        ( a u i )
  DUP FILL@ @ 0= IF DROP 2DROP FALSE EXIT THEN
  >R  R@ ROOM@ BE16@ MIN                                     ( a u2 )
  TUCK  R@ ROOM@ 8 +  ROT ROT  MOVE                          ( u2 )
  R@ ROOM@ 2 + BE32@  R@ ROOM@ 6 + BE16@  R> TAKE  TRUE ;
: (JOIN) ( ip h -- ior )   SOCKET? 0= IF DROP NET-CLOSED EXIT THEN  DROP IP-JOIN ;

END-PACKAGE
