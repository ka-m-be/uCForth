\ tap.fs - tests of our TCP/IP against the kernel of Linux, over a TAP device
\
\ Use (in the folder ucf), in Docker, as root (lib/net/README.md, "Test"):
\   docker run --rm --network none --cap-add NET_ADMIN --device /dev/net/tun \
\     -v "$PWD":/u -w /u debian:bookworm-slim \
\     sh -c 'echo "ALSO FILES  INCLUDE lib/net/test/tap.fs" | ./build/ucf-linux-a64'
\ Our TCP/IP is 10.0.77.2 at the TAP device ucf0 (package TAP); the kernel
\ is 10.0.77.1 at the other side. Our side uses the words of NET, the
\ side of the kernel the words of POSIX-NET; they do not wait, for the
\ kernel works by itself. So one thread is enough: a test waits with
\ NET-POLL, and meanwhile our TCP/IP works. With --network none no
\ packet reaches another network. At the end: "tap: n tests, 0 errors".

ONLY FORTH DEFINITIONS  ALSO FILES
INCLUDE forth/test/tester.fs
ONLY FORTH ALSO FILES  INCLUDE lib/net/hostsys.fs
ONLY FORTH ALSO FILES  [DEFINED] POSIX-NET  S" lib/net/posix.fs"  ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] TAP        S" lib/linux/tap.fs"  ?INCLUDED
ONLY FORTH ALSO FILES  INCLUDE lib/net/tcpip.fs
ONLY FORTH DEFINITIONS  ALSO INTERNALS  ALSO TEXT  ALSO INET  ALSO NET  ALSO SYSTEM
ALSO TAP  ALSO ARP  ALSO LINK-NET
DECIMAL

10 0 77 1 IP CONSTANT K-IP        \ the kernel
10 0 77 2 IP CONSTANT OUR-IP      \ our TCP/IP
255 255 255 0 IP CONSTANT MASK
CREATE BUF 2000 ALLOT

\ ---- the side of the kernel: POSIX-NET, and a socket for ping
ALSO POSIX-NET
: K-UDP ( port -- h )   (UDP-OPEN) THROW ;
: K-SEND ( a u ip port h -- ior )   (SENDTO) ;
: K-RECV ( a u h -- u2 ip port true | false )   (RECVFROM) ;
: K-CLOSE ( h -- )   (CLOSE) DROP ;
PREVIOUS
1 CONSTANT IPPROTO-ICMP   32 CONSTANT IP-MULTICAST-IF
: NOWAIT ( fd -- )   DUP F-GETFL 0 SYS-FCNTL  O-NONBLOCK OR  F-SETFL SWAP SYS-FCNTL DROP ;
VARIABLE ADDR
: VIA-TAP ( fd -- )   \ the multicast of this socket goes out at ucf0
  K-IP ADDR BE32!  IPPROTO-IP IP-MULTICAST-IF ADDR 4 SYS-SETSOCKOPT DROP ;

\ ---- the device, the addresses
T{ S" ucf0" TAP-OPEN -> 0 }T
T{ K-IP MASK TAP-HOST -> 0 }T
OUR-IP MASK K-IP K-IP NET-STATIC
T{ NET-UP  MY-IP NETMASK GATEWAY DNS-SERVER -> 0 OUR-IP MASK K-IP K-IP }T

\ ---- UDP from the kernel: it asks with ARP first, our TCP/IP answers
VARIABLE U-OUR   VARIABLE U-KERNEL
: POLLS ( n -- )   0 ?DO NET-POLL LOOP ;
: OURS ( -- u ip port true | false )   \ a datagram for U-OUR, in at most 3 s
  3000 0 DO  BUF 2000 U-OUR @ UDP-RECEIVE IF TRUE UNLOOP EXIT THEN  NET-POLL  LOOP  FALSE ;
: KERNELS ( -- u ip port true | false )   \ a datagram for the kernel, in at most 3 s
  3000 0 DO  BUF 2000 U-KERNEL @ K-RECV IF TRUE UNLOOP EXIT THEN  NET-POLL  LOOP  FALSE ;
T{ 47021 UDP-OPEN SWAP U-OUR ! -> 0 }T
T{ 47022 K-UDP U-KERNEL ! -> }T
T{ S" from linux" OUR-IP 47021 U-KERNEL @ K-SEND -> 0 }T
T{ OURS -> 10 K-IP 47022 TRUE }T
T{ BUF 10 S" from linux" S= -> TRUE }T

\ ---- UDP to the kernel; the MAC of the kernel came with its question
T{ S" from ucf" K-IP 47022 U-OUR @ UDP-SEND -> 0 }T
T{ KERNELS -> 8 OUR-IP 47021 TRUE }T
T{ BUF 8 S" from ucf" S= -> TRUE }T
\ our question of ARP: an empty table
T{ ARP-FORGET  S" ask first" K-IP 47022 U-OUR @ UDP-SEND -> 0 }T
T{ KERNELS -> 9 OUR-IP 47021 TRUE }T
\ the question is lost: ARP asks again after 1 s
T{ ARP-FORGET  1 TAP-LOSE  S" again" K-IP 47022 U-OUR @ UDP-SEND  0 TAP-LOSE -> 0 }T
: TIMED ( -- u ip port true ms | false ms )   MILLIS >R  KERNELS  MILLIS R> - ;
T{ TIMED 900 2000 WITHIN -> 5 OUR-IP 47021 TRUE TRUE }T
\ to all: 255.255.255.255 and 10.0.77.255
T{ S" all" -1 47022 U-OUR @ UDP-SEND -> 0 }T
T{ KERNELS -> 3 OUR-IP 47021 TRUE }T
T{ S" net" 10 0 77 255 IP 47022 U-OUR @ UDP-SEND -> 0 }T
T{ KERNELS -> 3 OUR-IP 47021 TRUE }T
\ too long for one frame
T{ BUF 1473 K-IP 47022 U-OUR @ UDP-SEND -> NET-FAILED }T
T{ BUF 1472 K-IP 47022 U-OUR @ UDP-SEND -> 0 }T
T{ KERNELS -> 1472 OUR-IP 47021 TRUE }T
\ to the own address: the loop
T{ S" self" OUR-IP 47021 U-OUR @ UDP-SEND -> 0 }T
T{ OURS -> 4 OUR-IP 47021 TRUE }T

\ ---- a group (multicast): only after UDP-JOIN
VARIABLE U-GROUP
T{ 47023 UDP-OPEN SWAP U-GROUP ! -> 0 }T
T{ U-KERNEL @ VIA-TAP  S" group" 224 0 0 251 IP 47023 U-KERNEL @ K-SEND -> 0 }T
T{ 100 POLLS  BUF 100 U-GROUP @ UDP-RECEIVE -> FALSE }T
T{ 224 0 0 251 IP U-GROUP @ UDP-JOIN -> 0 }T
T{ S" group" 224 0 0 251 IP 47023 U-KERNEL @ K-SEND -> 0 }T
: GROUPS ( -- u ip port true | false )
  3000 0 DO  BUF 100 U-GROUP @ UDP-RECEIVE IF TRUE UNLOOP EXIT THEN  NET-POLL  LOOP  FALSE ;
T{ GROUPS -> 5 K-IP 47022 TRUE }T
T{ U-GROUP @ SOCK-CLOSE -> 0 }T

\ ---- ping from the kernel (a socket for ICMP; the kernel sets the number and the checksum)
VARIABLE P-SOCK
CREATE MSG 1500 ALLOT
: ECHO-REQUEST ( u -- n )   \ an echo request with u bytes of data in MSG; n: its length
  MSG 8 0 FILL  8 MSG C!  1 MSG 6 + BE16!
  DUP 0 ?DO  I 255 AND  MSG 8 + I + C!  LOOP  8 + ;
: PING ( u ip -- ior )   >R ECHO-REQUEST  MSG SWAP  R> 0 P-SOCK @ K-SEND ;
: PONG ( -- u )   \ the length of the data of the echo reply, in at most 1 s; 0: none
  1000 0 DO  BUF 2000 P-SOCK @ K-RECV IF  2DROP 8 - UNLOOP EXIT  THEN  NET-POLL  LOOP  0 ;
: DATA? ( u -- f )   \ the reply in BUF has the data that went
  TRUE SWAP 0 ?DO  BUF 8 + I + C@  I 255 AND <> IF DROP FALSE THEN  LOOP ;
T{ AF-INET SOCK-DGRAM IPPROTO-ICMP SYS-SOCKET DUP P-SOCK ! 0 > -> TRUE }T
P-SOCK @ NOWAIT
T{ 56 OUR-IP PING -> 0 }T
T{ PONG  BUF C@  56 DATA? -> 56 0 TRUE }T
T{ 1472 OUR-IP PING -> 0 }T
T{ PONG  BUF C@  1472 DATA? -> 1472 0 TRUE }T
T{ 56 10 0 77 3 IP PING -> 0 }T                \ nobody: no answer
T{ PONG -> 0 }T

T{ U-OUR @ SOCK-CLOSE -> 0 }T
U-KERNEL @ K-CLOSE  P-SOCK @ K-CLOSE

\ ---- TCP: the side of the kernel
ALSO POSIX-NET
: K-LISTEN ( port -- h )   (TCP-LISTEN) THROW ;
: K-ACCEPT ( h -- h2 true | false )   (TCP-ACCEPT) ;
: K-CONNECT ( ip port -- h )   (TCP-CONNECT) THROW ;
: K-STATE ( h -- n )   (TCP-STATE) ;
: K-WRITE ( a u h -- u2 ior )   (SEND) ;
: K-READ ( a u h -- u2 ior )   (RECV) ;
PREVIOUS
13 CONSTANT SO-LINGER   8 CONSTANT SO-RCVBUF
CREATE NO-LINGER   1 , 0 ,                   \ struct linger: on, 0 s: close sends RST
: K-RESET ( h -- )   DUP SOL-SOCKET SO-LINGER NO-LINGER 8 SYS-SETSOCKOPT DROP  K-CLOSE ;
VARIABLE OPTION
: K-SMALL ( h -- )   \ a small buffer to receive: the window of the kernel closes soon
  1 OPTION !  SOL-SOCKET SO-RCVBUF OPTION 4 SYS-SETSOCKOPT DROP ;
: K-ACCEPTED ( h -- h2 true | false )   \ the kernel accepts, in at most 3 s
  3000 0 DO  DUP K-ACCEPT IF  NIP TRUE UNLOOP EXIT  THEN  NET-POLL  LOOP  DROP FALSE ;
: K-OPEN? ( h -- n )   \ the state of the kernel's connection, after 3 s at most
  3000 0 DO  DUP K-STATE ?DUP IF  NIP UNLOOP EXIT  THEN  NET-POLL  LOOP  DROP 0 ;

\ ---- many bytes, with a check: each byte is its place in the stream, modulo 251
CREATE CHUNK 1000 ALLOT
VARIABLE SENT   VARIABLE GOT   VARIABLE WRONG   VARIABLE TOTAL   VARIABLE DUE
VARIABLE OUR-S   VARIABLE K-S                \ the two ends of a test
: FILL-CHUNK ( n -- )   0 ?DO  SENT @ I + 251 MOD  CHUNK I + C!  LOOP ;
: CHECK ( a u -- )   \ bytes that came: each must be its place modulo 251
  0 ?DO  DUP I + C@  GOT @ I + 251 MOD <> IF 1 WRONG +! THEN  LOOP  DROP ;
: GOT+ ( a u -- )   TUCK CHECK  GOT +! ;
: START ( n -- )   TOTAL !  0 SENT !  0 GOT !  0 WRONG !  MILLIS 20000 + DUE ! ;
: IN-TIME? ( -- f )   DUE @ MILLIS - 0 > ;
: NEXT-PIECE ( -- n )   TOTAL @ SENT @ -  1000 MIN ;
: TO-KERNEL ( n sock kh -- f )   \ n bytes from our sock to the kernel's kh; true: all right
  K-S !  OUR-S !  START
  BEGIN  GOT @ TOTAL @ <  IN-TIME? AND  WHILE
    NEXT-PIECE ?DUP IF  DUP FILL-CHUNK  CHUNK OVER OUR-S @ SOCK-WRITE IF DROP ELSE SENT +! THEN  THEN
    BUF 2000 K-S @ K-READ DROP  BUF SWAP GOT+
    NET-POLL
  REPEAT
  GOT @ TOTAL @ =  WRONG @ 0= AND ;
: FROM-KERNEL ( n sock kh -- f )   \ n bytes from the kernel's kh to our sock
  K-S !  OUR-S !  START
  BEGIN  GOT @ TOTAL @ <  IN-TIME? AND  WHILE
    NEXT-PIECE ?DUP IF  DUP FILL-CHUNK  CHUNK SWAP K-S @ K-WRITE DROP SENT +!  THEN
    OUR-S @ SOCK-AVAILABLE IF  BUF 2000 OUR-S @ SOCK-READ DROP  BUF SWAP GOT+  THEN
    NET-POLL
  REPEAT
  GOT @ TOTAL @ =  WRONG @ 0= AND ;

\ ---- TCP: we open, the kernel listens
3000 NET-TIMEOUT !
VARIABLE K-SRV   VARIABLE K-C   VARIABLE OUR-C
T{ 47031 K-LISTEN K-SRV ! -> }T
T{ S" 10.0.77.1" 47031 TCP-OPEN SWAP OUR-C ! -> 0 }T
T{ K-SRV @ K-ACCEPTED SWAP K-C ! -> TRUE }T
T{ OUR-C @ SOCK-OPEN?  K-C @ K-OPEN? -> TRUE 1 }T
T{ S" hello" OUR-C @ SOCK-WRITE -> 0 }T
T{ 100 POLLS  BUF 100 K-C @ K-READ  BUF 5 S" hello" S= -> 5 0 TRUE }T
T{ S" back" K-C @ K-WRITE -> 4 0 }T
T{ BUF 100 OUR-C @ SOCK-READ  BUF 4 S" back" S= -> 4 0 TRUE }T
T{ 100000 OUR-C @ K-C @ TO-KERNEL -> TRUE }T
T{ 100000 OUR-C @ K-C @ FROM-KERNEL -> TRUE }T
\ we close: the kernel sees the end
T{ OUR-C @ SOCK-CLOSE -> 0 }T
T{ K-C @ K-OPEN? -> NET-CLOSED }T
K-C @ K-CLOSE

\ the kernel closes: we read the end
T{ S" 10.0.77.1" 47031 TCP-OPEN SWAP OUR-C ! -> 0 }T
T{ K-SRV @ K-ACCEPTED SWAP K-C ! -> TRUE }T
T{ S" bye" K-C @ K-WRITE -> 3 0 }T
K-C @ K-CLOSE
T{ BUF 100 OUR-C @ SOCK-READ -> 3 0 }T
T{ BUF 100 OUR-C @ SOCK-READ  OUR-C @ SOCK-OPEN? -> 0 0 FALSE }T
T{ OUR-C @ SOCK-CLOSE -> 0 }T

\ the kernel resets the connection: our next write fails
T{ S" 10.0.77.1" 47031 TCP-OPEN SWAP OUR-C ! -> 0 }T
T{ K-SRV @ K-ACCEPTED SWAP K-C ! -> TRUE }T
K-C @ K-RESET
T{ 100 POLLS  OUR-C @ SOCK-OPEN?  S" x" OUR-C @ SOCK-WRITE -> FALSE NET-CLOSED }T
T{ OUR-C @ SOCK-CLOSE -> 0 }T

\ no server at the port; nobody at the address
T{ S" 10.0.77.1" 47039 TCP-OPEN -> 0 NET-REFUSED }T
1000 NET-TIMEOUT !
: TIMED-OPEN ( -- sock ior ms )   MILLIS >R  S" 10.0.77.3" 47031 TCP-OPEN  MILLIS R> - ;
T{ TIMED-OPEN 900 1500 WITHIN -> 0 NET-TIMED-OUT TRUE }T
3000 NET-TIMEOUT !

\ ---- TCP: we listen, the kernel opens
VARIABLE SRV
T{ 47032 TCP-LISTEN SWAP SRV ! -> 0 }T
T{ OUR-IP 47032 K-CONNECT K-C ! -> }T
: ACCEPTED ( server -- sock true | false )   \ a connection, in at most 3 s
  3000 0 DO  DUP TCP-ACCEPT IF  NIP TRUE UNLOOP EXIT  THEN  NET-POLL  LOOP  DROP FALSE ;
T{ SRV @ ACCEPTED SWAP OUR-C ! -> TRUE }T
T{ K-C @ K-OPEN? -> 1 }T
T{ 20000 OUR-C @ K-C @ TO-KERNEL -> TRUE }T
T{ 20000 OUR-C @ K-C @ FROM-KERNEL -> TRUE }T
T{ OUR-C @ SOCK-CLOSE -> 0 }T
T{ K-C @ K-OPEN? -> NET-CLOSED }T
K-C @ K-CLOSE

\ ---- lost frames: each 7th in each direction; TCP brings all bytes
T{ OUR-IP 47032 K-CONNECT K-C ! -> }T
T{ SRV @ ACCEPTED SWAP OUR-C ! -> TRUE }T
7 TAP-LOSE
T{ 30000 OUR-C @ K-C @ TO-KERNEL -> TRUE }T
T{ 30000 OUR-C @ K-C @ FROM-KERNEL -> TRUE }T
0 TAP-LOSE
T{ OUR-C @ SOCK-CLOSE -> 0 }T
K-C @ K-CLOSE

\ ---- windows of 0: we do not read for a while; the kernel does not read for a while
T{ OUR-IP 47032 K-CONNECT K-C ! -> }T
T{ SRV @ ACCEPTED SWAP OUR-C ! -> TRUE }T
: STUFF ( n kh -- )   \ n bytes into the kernel's socket, as far as it takes them now
  K-S !  TOTAL !  0 SENT !
  BEGIN  NEXT-PIECE ?DUP WHILE
    DUP FILL-CHUNK  CHUNK SWAP K-S @ K-WRITE DROP  ?DUP 0= IF EXIT THEN  SENT +!
  REPEAT ;
T{ 10000 K-C @ STUFF  500 POLLS  OUR-C @ SOCK-AVAILABLE -> 2048 }T   \ our buffer is full: our window is 0
: READ-REST ( n sock -- f )   \ read n bytes with the check; the kernel sends the rest
  OUR-S !  TOTAL !  0 GOT !  0 WRONG !  MILLIS 20000 + DUE !
  BEGIN  GOT @ TOTAL @ <  IN-TIME? AND  WHILE
    NEXT-PIECE ?DUP IF  DUP FILL-CHUNK  CHUNK SWAP K-S @ K-WRITE DROP SENT +!  THEN
    OUR-S @ SOCK-AVAILABLE IF  BUF 2000 OUR-S @ SOCK-READ DROP  BUF SWAP GOT+  THEN
    NET-POLL
  REPEAT  GOT @ TOTAL @ =  WRONG @ 0= AND ;
T{ 10000 OUR-C @ READ-REST -> TRUE }T
T{ OUR-C @ SOCK-CLOSE -> 0 }T
K-C @ K-CLOSE

T{ SRV @ SOCK-CLOSE  K-SRV @ K-CLOSE -> 0 }T

\ ---- DHCP: a server in Forth at the side of the kernel (port 67, only at ucf0)
25 CONSTANT SO-BINDTODEVICE
CREATE DEV-NAME   'u' C, 'c' C, 'f' C, '0' C, 0 C,
1 CONSTANT D-DISCOVER   2 CONSTANT D-OFFER   3 CONSTANT D-REQUEST   5 CONSTANT D-ACK   6 CONSTANT D-NAK
VARIABLE DS                                  \ the socket of the server
1024 BUFFER: DQ   VARIABLE DQN   VARIABLE D-FROM   \ the question that came, its sender
1024 BUFFER: DA   VARIABLE DP                \ the answer of the server, and the place in it
: D-OPEN ( -- )   67 K-UDP DS !  DS @ SOL-SOCKET SO-BINDTODEVICE DEV-NAME 5 SYS-SETSOCKOPT DROP ;
: D-OPTION ( code -- a | 0 )   \ the value of an option in the question
  DQ 240 +
  BEGIN  DUP DQ DQN @ + U<  WHILE
    DUP C@ 255 = IF  2DROP 0 EXIT  THEN
    DUP C@ 0= IF  1+  ELSE
      2DUP C@ = IF  NIP 2 + EXIT  THEN
      DUP 1+ C@ 2 + +
    THEN
  REPEAT  2DROP 0 ;
: HEAR-D ( -- type true | false )   \ a message of our client, in at most 3 s
  3000 0 DO
    DQ 1024 DS @ K-RECV IF
      DROP D-FROM !  DQN !  53 D-OPTION ?DUP IF C@ ELSE 0 THEN  TRUE UNLOOP EXIT
    THEN  NET-POLL
  LOOP  FALSE ;
: DB, ( c -- )   DP @ C!  1 DP +! ;
: DL, ( x -- )   DP @ BE32!  4 DP +! ;
: D-ANSWER ( type lease -- )   \ OFFER, ACK or NAK, to all at port 68
  DA 1024 0 FILL  DQ DA 240 MOVE  2 DA C!               \ the head of the question; BOOTREPLY
  OVER D-NAK = IF 0 ELSE OUR-IP THEN  DA 16 + BE32!     \ yiaddr
  DA 240 + DP !
  53 DB,  1 DB,  OVER DB,   54 DB,  4 DB,  K-IP DL,
  OVER D-NAK <> IF
    51 DB, 4 DB, DUP DL,  1 DB, 4 DB, MASK DL,  3 DB, 4 DB, K-IP DL,  6 DB, 4 DB, K-IP DL,
  THEN  2DROP  255 DB,
  DA  DP @ DA - 300 MAX  -1 68 DS @ K-SEND DROP ;
: NAME? ( -- f )   12 D-OPTION ?DUP IF  DUP 1- C@ S" ucf-660001" S=  ELSE FALSE THEN ;

D-OPEN
T{ NET-DHCP  (NET-UP)  MY-IP -> 0 0 }T                \ DHCP asks; no address yet
T{ HEAR-D -> D-DISCOVER TRUE }T
T{ NAME?  DQ 28 + LINK-MAC MAC= -> TRUE TRUE }T
D-OFFER 30 D-ANSWER
T{ HEAR-D -> D-REQUEST TRUE }T
T{ 50 D-OPTION BE32@  54 D-OPTION BE32@ -> OUR-IP K-IP }T
D-ACK 4 D-ANSWER
T{ 10 POLLS  (NET-UP)  MY-IP NETMASK GATEWAY DNS-SERVER -> 1 OUR-IP MASK K-IP K-IP }T
\ after half of the lease (2 s) the client asks its server for more time, from its address
T{ HEAR-D  D-FROM @  DQ 12 + BE32@ -> D-REQUEST TRUE OUR-IP OUR-IP }T
D-ACK 4 D-ANSWER
T{ 10 POLLS  (NET-UP)  MY-IP -> 1 OUR-IP }T
\ a NAK: the address is gone, the client starts again
T{ HEAR-D -> D-REQUEST TRUE }T
D-NAK 0 D-ANSWER
T{ 10 POLLS  MY-IP -> 0 }T
T{ HEAR-D -> D-DISCOVER TRUE }T
\ no OFFER: the DISCOVER comes again
T{ HEAR-D -> D-DISCOVER TRUE }T
D-OFFER 2 D-ANSWER
T{ HEAR-D -> D-REQUEST TRUE }T
D-ACK 2 D-ANSWER
T{ 10 POLLS  MY-IP -> OUR-IP }T
\ nobody answers: at the end of the lease (2 s) the address is gone
T{ 2500 POLLS  MY-IP -> 0 }T
\ NET-UP without a server: no address in NET-TIMEOUT
500 NET-TIMEOUT !
T{ NET-UP -> NET-TIMED-OUT }T
3000 NET-TIMEOUT !
\ fixed addresses again
OUR-IP MASK K-IP K-IP NET-STATIC
T{ NET-UP  MY-IP -> 0 OUR-IP }T
DS @ K-CLOSE

S" tap" .RESULTS
