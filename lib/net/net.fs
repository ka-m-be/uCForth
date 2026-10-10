\ net.fs - package NET: the network for an application
\
\ TCP and UDP over IPv4, with the same words on each system (lib/net/
\ README.md). Under NET lies the layer NETIF: the sockets of the system
\ (POSIX-NET: macOS and Linux), or on a board our own TCP/IP. The
\ addresses and the errors come from the package INET.
\
\ A connection (sock), a server and a UDP socket (usock) are numbers. An
\ error is an ior of INET (NET-REFUSED ...); 0 is no error. A word that
\ waits does it for at most NET-TIMEOUT ms, in steps of NET-POLL; Ctrl-C
\ stops it.
\
\   NET-UP ( -- ior )          the network ready: on a board WLAN and DHCP
\                              (waits at most NET-TIMEOUT); on a system with
\                              its own network nothing
\   MY-IP GATEWAY NETMASK DNS-SERVER ( -- ip )   the addresses; 0: not known
\   NET-TIMEOUT ( -- a )       a variable: a word waits at most so many ms (5000)
\   NET-POLL ( -- )            the network works one step of at most 1 ms
\   a u RESOLVE ( -- ip ior )  the address of a host: 192.168.1.10,
\                              localhost, or a name (with the package DNS)
\   a u port TCP-OPEN ( -- sock ior )   a connection to the host a u
\   port TCP-LISTEN ( -- server ior )   a server at port
\   server TCP-ACCEPT ( -- sock true | false )   a new connection; does not wait
\   a u sock SOCK-WRITE ( -- ior )      all bytes
\   a u sock SOCK-READ ( -- u2 ior )    the bytes that are there; waits for
\                              the first (u2 0: none in time, or the end)
\   sock SOCK-AVAILABLE ( -- n )        so many bytes wait
\   sock SOCK-OPEN? ( -- f )   the connection stands, or bytes wait
\   sock SOCK-CLOSE ( -- ior ) also for a server and a UDP socket
\   sock >SOCKET   >CONSOLE    the output (TYPE . EMIT) goes to the
\                              connection, then back to the console
\   xt sock SOCK-PRINT ( -- ior )   xt ( -- ) runs with its output to the
\                              connection; then the console in each case
\   port UDP-OPEN ( -- usock ior )      port 0: any port
\   a u ip port usock UDP-SEND ( -- ior )   a datagram; ip 255.255.255.255: to all
\   a u usock UDP-RECEIVE ( -- u2 ip port true | false )   does not wait
\   ip usock UDP-JOIN ( -- ior )        a group for multicast
\
\ >CONSOLE sends the rest of the output. An error of the connection during
\ the output (in EMIT or >CONSOLE) puts the output back to the console,
\ then it THROWs the ior. An error of the application between >SOCKET
\ and >CONSOLE leaves the output at the connection: SOCK-PRINT catches it.
\
\ Example (lib/net/README.md has more):
\   ALSO INET  ALSO NET
\   S" 127.0.0.1" 7 TCP-OPEN THROW  CONSTANT S
\   S" hello" S SOCK-WRITE THROW  S SOCK-CLOSE THROW

PACKAGE NET  ALSO NETIF  ALSO INET  ALSO TEXT   \ lib/package.fs

PUBLIC
VARIABLE NET-TIMEOUT   5000 NET-TIMEOUT !

: NET-POLL ( -- )          1 (NET-POLL) ;
: MY-IP ( -- ip )          (MY-IP) ;
: GATEWAY ( -- ip )        (GATEWAY) ;
: NETMASK ( -- ip )        (NETMASK) ;
: DNS-SERVER ( -- ip )     (DNS-SERVER) ;

PRIVATE
: DEADLINE ( -- t )   MILLIS NET-TIMEOUT @ + ;
: PAST? ( t -- f )    MILLIS - 0< ;
: NO-DNS ( a u -- ip ior )   2DROP 0 NET-NO-HOST ;

PUBLIC
: NET-UP ( -- ior )   \ waits at most NET-TIMEOUT for the network (on a board: DHCP)
  DEADLINE >R
  BEGIN  (NET-UP)  DUP 0=  R@ PAST? 0= AND  WHILE  DROP NET-POLL  REPEAT  R> DROP
  DUP 1 = IF  DROP 0 EXIT  THEN
  ?DUP 0= IF NET-TIMED-OUT THEN ;
DEFER (RESOLVE)   ' NO-DNS IS (RESOLVE)   \ a u ( -- ip ior ): a name; the package DNS sets it
: RESOLVE ( a u -- ip ior )   \ 192.168.1.10, localhost, or a name
  2DUP >IP IF  NIP NIP 0 EXIT  THEN
  2DUP S" localhost" S= IF  2DROP 127 0 0 1 IP 0 EXIT  THEN
  (RESOLVE) ;

\ ---- TCP
: TCP-OPEN ( a u port -- sock ior )   \ waits at most NET-TIMEOUT for the connection
  >R RESOLVE ?DUP IF  R> DROP EXIT  THEN
  R> (TCP-CONNECT) ?DUP IF EXIT THEN                         ( h )
  DEADLINE >R                                                \ the state once: the system tells an error once
  BEGIN  DUP (TCP-STATE)  DUP 0=  R@ PAST? 0= AND  WHILE  DROP NET-POLL  REPEAT  R> DROP
  DUP 1 = IF  DROP 0 EXIT  THEN                              ( h n )
  ?DUP 0= IF NET-TIMED-OUT THEN
  SWAP (CLOSE) DROP  0 SWAP ;
: TCP-LISTEN ( port -- server ior )             (TCP-LISTEN) ;
: TCP-ACCEPT ( server -- sock true | false )    (TCP-ACCEPT) ;
: SOCK-AVAILABLE ( sock -- n )                  (AVAILABLE) ;
: SOCK-OPEN? ( sock -- f )                      (TCP-STATE) 1 = ;
: SOCK-CLOSE ( sock -- ior )                    (CLOSE) ;

PRIVATE
VARIABLE SOCK   VARIABLE DUE
: WAIT-BYTES ( sock -- sock )   \ until bytes wait, the connection ends, or NET-TIMEOUT is over
  DEADLINE DUE !
  BEGIN  DUP (AVAILABLE) 0=  OVER (TCP-STATE) 1 = AND  DUE @ PAST? 0= AND  WHILE  NET-POLL  REPEAT ;
PUBLIC
: SOCK-READ ( a u sock -- u2 ior )   WAIT-BYTES (RECV) ;
: SOCK-WRITE ( a u sock -- ior )   \ waits at most NET-TIMEOUT while the system takes no byte
  SOCK !  DEADLINE DUE !
  BEGIN  DUP WHILE
    2DUP SOCK @ (SEND)  ?DUP IF  NIP NIP NIP EXIT  THEN       ( a u u2 )
    ?DUP IF  /STRING  DEADLINE DUE !
    ELSE  DUE @ PAST? IF  2DROP NET-TIMED-OUT EXIT  THEN  NET-POLL  THEN
  REPEAT  2DROP 0 ;

\ ---- the output to a connection
PRIVATE
VARIABLE OUT-SOCK        \ the connection of >SOCKET
VARIABLE CONSOLE-EMIT    \ 'EMIT before >SOCKET; 0: the output goes to the console
256 BUFFER: OUT-BUF   VARIABLE OUT-N
: SEND-OUT ( -- ior )   OUT-BUF OUT-N @ OUT-SOCK @ SOCK-WRITE  0 OUT-N ! ;
: CONSOLE ( -- )   CONSOLE-EMIT @ 'EMIT !  0 CONSOLE-EMIT ! ;
: SOCK-EMIT ( c -- )   \ 'EMIT for >SOCKET: 256 bytes at a time
  OUT-BUF OUT-N @ + C!  1 OUT-N +!
  OUT-N @ 256 < IF EXIT THEN
  SEND-OUT ?DUP IF  CONSOLE THROW  THEN ;
PUBLIC
: >CONSOLE ( -- )   \ the output back to the console, after the rest went to the connection
  CONSOLE-EMIT @ 0= IF EXIT THEN
  CONSOLE  SEND-OUT THROW ;
: >SOCKET ( sock -- )   \ the output goes to the connection sock
  >CONSOLE  OUT-SOCK !  0 OUT-N !  'EMIT @ CONSOLE-EMIT !  ['] SOCK-EMIT 'EMIT ! ;
: SOCK-PRINT ( xt sock -- ior )   \ xt ( -- ) runs, its output to sock; then the console in each case
  >SOCKET  CATCH  ['] >CONSOLE CATCH  OVER IF DROP ELSE NIP THEN ;   \ the first error counts

\ ---- UDP
: UDP-OPEN ( port -- usock ior )                         (UDP-OPEN) ;
: UDP-SEND ( a u ip port usock -- ior )                  (SENDTO) ;
: UDP-RECEIVE ( a u usock -- u2 ip port true | false )   (RECVFROM) ;
: UDP-JOIN ( ip usock -- ior )                           (JOIN) ;

END-PACKAGE
