\ posix.fs - package POSIX-NET: the network over the sockets of macOS and Linux
\
\ The layer NETIF (lib/net/README.md) for a system with its own TCP/IP:
\ each word of the contract is a call of the system, over the package
\ of the system (ALSO SYSTEM: lib/macos/system.fs, lib/linux/a64.fs ...).
\ A handle h is the file descriptor of a socket. No word waits: each
\ socket is non-blocking. (NET-POLL) waits with MS.
\
\ The system makes the network (DHCP) and knows its addresses. (MY-IP)
\ asks it which address of this computer goes to the network: a UDP
\ socket with connect to 192.0.2.1 (an address for examples; no packet
\ goes out), then getsockname. (DNS-SERVER) is the first line
\ "nameserver a.b.c.d" in /etc/resolv.conf. (GATEWAY) and (NETMASK) give
\ 0: the system keeps them for itself.
\
\ At the end, NETIF gives this package: the package NET writes ALSO NETIF.

PACKAGE POSIX-NET  ALSO SYSTEM  ALSO INET  ALSO TEXT  ALSO FILES   \ lib/package.fs

16 BUFFER: SA            \ a struct sockaddr_in
VARIABLE SA-LEN          \ its length, for a call that gives an address back
8 BUFFER: PFD            \ a struct pollfd: fd (32 bits), events, revents (16 bits each)
VARIABLE OPTION          \ the value of a socket option
VARIABLE OPTION-LEN
VARIABLE N-READY         \ FIONREAD: the bytes that wait
VARIABLE PEEKED          \ the byte of PEEK
8 BUFFER: MREQ           \ a struct ip_mreq: the group, then the interface (0: any)

: SA! ( ip port -- )   SA 16 0 FILL  SA SA-FAMILY!  SA 2 + BE16!  SA 4 + BE32! ;
: SA@ ( -- ip port )   SA 4 + BE32@  SA 2 + BE16@ ;
: SA-ROOM ( -- sa alen )   16 SA-LEN !  SA SA-LEN ;   \ for a call that writes an address

: >IOR ( r -- ior )   \ the result of a call as an error of INET: 0 and more is none
  DUP 0< 0= IF DROP 0 EXIT THEN
  NEGATE CASE
    ECONNREFUSED OF NET-REFUSED ENDOF
    ETIMEDOUT    OF NET-TIMED-OUT ENDOF
    ENETUNREACH  OF NET-UNREACHABLE ENDOF
    EHOSTUNREACH OF NET-UNREACHABLE ENDOF
    ECONNRESET   OF NET-CLOSED ENDOF
    ECONNABORTED OF NET-CLOSED ENDOF
    EPIPE        OF NET-CLOSED ENDOF
    ENOTCONN     OF NET-CLOSED ENDOF
    EBADF        OF NET-CLOSED ENDOF
    EADDRINUSE   OF NET-IN-USE ENDOF
    NET-FAILED SWAP
  ENDCASE ;
: >COUNT ( r -- u ior )   \ the result of a send or a receive: "not now" is no error
  DUP 0< 0= IF 0 EXIT THEN
  DUP EAGAIN NEGATE = IF DROP 0 0 EXIT THEN
  >IOR 0 SWAP ;

: SET-OPTION ( fd name -- )   \ a socket option on: SO_REUSEADDR, SO_BROADCAST
  1 OPTION !  SOL-SOCKET SWAP OPTION 4 SYS-SETSOCKOPT DROP ;
: OPTION@ ( fd name -- x )   \ the value of a socket option: SO_ERROR
  0 OPTION !  4 OPTION-LEN !  SOL-SOCKET SWAP OPTION OPTION-LEN SYS-GETSOCKOPT DROP  OPTION @ ;
: NONBLOCK ( fd -- )   \ the calls on fd do not wait
  DUP F-GETFL 0 SYS-FCNTL  O-NONBLOCK OR  F-SETFL SWAP SYS-FCNTL DROP ;
: NEW-SOCKET ( type -- fd ior )   \ a socket for IPv4 that does not wait
  AF-INET SWAP 0 SYS-SOCKET
  DUP 0< IF  >IOR 0 SWAP  EXIT  THEN
  DUP NONBLOCK  0 ;
: BOUND ( port type -- fd ior )   \ a new socket at port, for each address of this computer
  NEW-SOCKET ?DUP IF  ROT DROP  EXIT  THEN          ( port fd )
  DUP SO-REUSEADDR SET-OPTION
  SWAP 0 SWAP SA!
  DUP SA 16 SYS-BIND  DUP 0< IF  >IOR SWAP SYS-CLOSE DROP  0 SWAP  EXIT  THEN
  DROP 0 ;

: POLL-NOW ( fd events -- revents )   \ which of the events are there now
  PFD 8 0 FILL  PFD 4 + C!  PFD !  PFD 1 0 SYS-POLL DROP  PFD 6 + C@ ;
: CONNECTED? ( fd -- f )   SA-ROOM SYS-GETPEERNAME 0= ;
: PEEK ( fd -- r )   \ one byte that stays: 1 bytes wait, 0 the end of the stream, < 0 an error
  PEEKED 1 MSG-PEEK 0 0 SYS-RECVFROM ;
: FAILED ( fd -- ior )   \ why the connection did not come
  SO-ERROR OPTION@  ?DUP IF NEGATE >IOR ELSE NET-CLOSED THEN ;
: OPEN-STATE ( fd revents -- n )   \ connected: 1, or NET-CLOSED after the end of the stream or an error
  DUP POLLIN AND IF
    DROP PEEK  DUP 0 >  SWAP EAGAIN NEGATE =  OR IF 1 ELSE NET-CLOSED THEN  EXIT
  THEN
  NIP  POLLERR POLLHUP OR AND IF NET-CLOSED ELSE 1 THEN ;

PUBLIC
: (TCP-CONNECT) ( ip port -- h ior )   \ the connection begins; (TCP-STATE) tells when it stands
  SOCK-STREAM NEW-SOCKET ?DUP IF  >R DROP 2DROP 0 R> EXIT  THEN    ( ip port fd )
  >R SA!  R@ SA 16 SYS-CONNECT                                     ( r )  ( R: fd )
  DUP 0< 0=  OVER EINPROGRESS NEGATE =  OR IF  DROP R> 0 EXIT  THEN
  >IOR  R> SYS-CLOSE DROP  0 SWAP ;
: (TCP-STATE) ( h -- n )   \ 1 the connection stands; 0 it comes; else the ior why it ended
  DUP POLLIN POLLOUT OR POLL-NOW                    ( h revents )
  OVER CONNECTED? IF  OPEN-STATE  EXIT  THEN
  IF  FAILED  ELSE  DROP 0  THEN ;
: (TCP-LISTEN) ( port -- h ior )   \ a server at port
  SOCK-STREAM BOUND ?DUP IF EXIT THEN
  DUP 8 SYS-LISTEN DROP  0 ;
: (TCP-ACCEPT) ( h -- h2 true | false )   \ a new connection to the server h
  SA-ROOM SYS-ACCEPT  DUP 0< IF DROP FALSE EXIT THEN
  DUP NONBLOCK  TRUE ;
: (SEND) ( a u h -- u2 ior )   \ as many bytes as the system takes now
  ROT ROT MSG-NOSIGNAL 0 0 SYS-SENDTO >COUNT ;
: (RECV) ( a u h -- u2 ior )   \ the bytes that are there now; 0 0 also at the end of the stream
  ROT ROT 0 0 0 SYS-RECVFROM >COUNT ;
: (AVAILABLE) ( h -- n )   FIONREAD N-READY SYS-IOCTL 0< IF 0 ELSE N-READY @ THEN ;
: (CLOSE) ( h -- ior )   SYS-CLOSE >IOR ;

: (UDP-OPEN) ( port -- h ior )   \ port 0: any port; it may send to 255.255.255.255
  SOCK-DGRAM BOUND ?DUP IF EXIT THEN
  DUP SO-BROADCAST SET-OPTION  0 ;
: (SENDTO) ( a u ip port h -- ior )   \ one datagram
  >R SA!  R> ROT ROT  MSG-NOSIGNAL SA 16 SYS-SENDTO >IOR ;
: (RECVFROM) ( a u h -- u2 ip port true | false )   \ one datagram, if one is there
  ROT ROT 0 SA-ROOM SYS-RECVFROM  DUP 0< IF DROP FALSE EXIT THEN  SA@ TRUE ;
: (JOIN) ( ip h -- ior )   \ the group ip for multicast
  SWAP MREQ BE32!  0 MREQ 4 + !  IPPROTO-IP IP-ADD-MEMBERSHIP MREQ 8 SYS-SETSOCKOPT >IOR ;

: (NET-UP) ( -- n )   1 ;   \ the system made the network
: (MY-IP) ( -- ip )   \ the address of this computer to the network; 0: none
  SOCK-DGRAM NEW-SOCKET IF DROP 0 EXIT THEN
  192 0 2 1 IP 9 SA!
  DUP SA 16 SYS-CONNECT 0< IF  SYS-CLOSE DROP 0 EXIT  THEN
  DUP SA-ROOM SYS-GETSOCKNAME DROP  SYS-CLOSE DROP  SA@ DROP ;
: (GATEWAY) ( -- ip )   0 ;
: (NETMASK) ( -- ip )   0 ;

PRIVATE
128 BUFFER: LINE
VARIABLE CONF            \ the file /etc/resolv.conf
: NAMESERVER ( a u -- ip )   \ the address in a line "nameserver a.b.c.d"; 0: another line
  SPLIT S" nameserver" S= 0= IF 2DROP 0 EXIT THEN
  SPLIT 2SWAP 2DROP  >IP 0= IF 0 THEN ;
: NEXT-SERVER ( -- ip f )   \ the address in the next line (0: none); f false at the end of the file
  LINE 128 CONF @ READ-LINE  IF 2DROP 0 FALSE EXIT THEN
  SWAP LINE SWAP NAMESERVER SWAP ;
PUBLIC
: (DNS-SERVER) ( -- ip )   \ the first server in /etc/resolv.conf; 0: none
  S" /etc/resolv.conf" R/O OPEN-FILE IF DROP 0 EXIT THEN  CONF !
  BEGIN  NEXT-SERVER  OVER 0= AND  WHILE  DROP  REPEAT
  CONF @ CLOSE-FILE DROP ;
: (NET-POLL) ( ms -- )   MS ;

END-PACKAGE
: NETIF ( -- )   POSIX-NET ;   \ ALSO NETIF: the layer under the package NET
