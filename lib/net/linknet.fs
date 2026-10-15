\ linknet.fs - package LINK-NET: the layer NETIF over our TCP/IP
\
\ The contract NETIF (lib/net/README.md) for a system without its own
\ TCP/IP: our packages ARP, IPV4, UDP, TCP and DHCP over the contract
\ LINK (a board; Linux with a TAP device in the tests). At the end,
\ NETIF gives this package: the package NET writes ALSO NETIF.
\
\ Each word of NETIF here runs the word with the same name of the
\ package below it (UDP, TCP) with LOCKED: meanwhile a step of the
\ network waits, and at its end the packets of the loop come in (IPV4).
\ So the loop works as in the kernel of a system: after TCP-OPEN to
\ 127.0.0.1 the server has the connection already.
\
\ A step of the network: one frame from the LINK, one from the loop, the
\ timers of ARP, TCP and DHCP. (NET-POLL) runs a step, then waits ms.
\ On a board a timer runs the steps (NET-START, lib/esp32/nettimer.fs);
\ a step that comes while a word of NETIF runs waits for the end of the
\ word. NET-STEP ( -- ) is the step: it does not wait, shows nothing,
\ and it keeps its errors.
\
\ The addresses come from DHCP: (NET-UP) starts it and gives 0 until
\ DHCP has an address, then 1 (NET-UP of NET waits for it). Or fixed:
\   ip mask gateway dns NET-STATIC ( -- )   the own addresses, no DHCP
\   NET-DHCP ( -- )            DHCP again (at the start): the next NET-UP asks
\   a u NET-HOSTNAME ( -- )    the name for the router (DHCP, option 12)
\
\ Errors in a step are lost (a bad packet must not stop the network).
\ Ctrl-C waits until a word of NETIF or a step ends (-BREAK of the
\ package IDLE): so it never stops TCP in the middle of a segment.

PACKAGE LINK-NET  ALSO LINK  ALSO ARP  ALSO IPV4  ALSO UDP  ALSO TCP  ALSO DHCP  ALSO INET  ALSO TEXT  ALSO IDLE   \ lib/package.fs

VARIABLE BUSY        \ a word of NETIF runs
VARIABLE WANTED      \ a step came meanwhile: the word does it at its end
VARIABLE STATIC      \ true: the addresses of NET-STATIC; false: DHCP
LINK-MTU 14 + BUFFER: RX   \ the frame that came
?: LINK-TICK ( -- ) ;
?: LINK-CONTROL ( -- ) ;

: FRAME-IN ( a u -- )   \ a frame from the LINK: ARP or IPv4, to our MAC or to a group
  DUP 14 < IF 2DROP EXIT THEN
  OVER C@ 1 AND 0= IF  OVER LINK-MAC MAC= 0= IF 2DROP EXIT THEN  THEN
  OVER 12 + BE16@ CASE
    $0806 OF ARP-INPUT ENDOF
    $0800 OF 14 /STRING IP-INPUT ENDOF
    >R 2DROP R>
  ENDCASE ;
: WORK ( -- )
  LINK-TICK
  RX LINK-RECEIVE ?DUP IF RX SWAP FRAME-IN THEN
  IP-STEP ARP-TICK TCP-TICK DHCP-TICK ;
: FINISH ( -- )   IP-FLUSH  WANTED @ IF  WANTED OFF!  WORK  THEN ;
: LOCKED ( i*x xt -- j*x )   \ run xt, a word of UDP or TCP, while a step and Ctrl-C wait
  -BREAK  1 BUSY +!  CATCH
  BUSY @ 1 = IF  ['] FINISH CATCH  -28 = IF DROP -28 THEN  THEN   \ only Ctrl-C at once counts
  -1 BUSY +!  +BREAK  THROW ;

PUBLIC
DEFER NET-SERVICE
' NOOP IS NET-SERVICE
: NET-CONTROL ( -- ) ['] LINK-CONTROL LOCKED NET-SERVICE ;
: NET-STEP ( -- )   \ a step of the network, but not while a word of NETIF runs
  BUSY @ IF  WANTED ON!  EXIT  THEN
  -BREAK  1 BUSY !  ['] WORK CATCH  0 BUSY !  WANTED OFF!  +BREAK
  DUP -28 = IF THROW THEN  DROP ;
: (TCP-CONNECT) ( a u ip port -- h ior )   ['] (TCP-CONNECT) LOCKED ;
: (TCP-STATE) ( h -- n )               ['] (TCP-STATE) LOCKED ;
: (TCP-LISTEN) ( a u port -- h ior )   ['] (TCP-LISTEN) LOCKED ;
: (TCP-ACCEPT) ( h -- h2 true | false )   ['] (TCP-ACCEPT) LOCKED ;
: (SEND) ( a u h -- u2 ior )           ['] (SEND) LOCKED ;
: (RECV) ( a u h -- u2 ior )           ['] (RECV) LOCKED ;
: (AVAILABLE) ( h -- n )               ['] (AVAILABLE) LOCKED ;
: (CLOSE) ( h -- ior )
  DUP UDP-HANDLE? IF ['] (UDP-CLOSE) ELSE ['] (TCP-CLOSE) THEN  LOCKED ;
: (UDP-OPEN) ( a u port -- h ior )     ['] (UDP-OPEN) LOCKED ;
: (SENDTO) ( a u ip port h -- ior )    ['] (SENDTO) LOCKED ;
: (RECVFROM) ( a u h -- u2 ip port true | false )   ['] (RECVFROM) LOCKED ;
: (JOIN) ( ip h -- ior )               ['] (JOIN) LOCKED ;

: (NET-UP) ( -- n )   \ 1: the network is up; 0: it comes (DHCP asks); else the ior
  LINK-UP? 0= IF NET-UNREACHABLE EXIT THEN
  STATIC @ DHCP-BOUND? OR IF 1 EXIT THEN
  ['] DHCP-START LOCKED  0 ;
: (MY-IP) ( -- ip )         ARP-IP @ ;
: (GATEWAY) ( -- ip )       IP-GATEWAY @ ;
: (NETMASK) ( -- ip )       IP-MASK @ ;
: (DNS-SERVER) ( -- ip )    IP-DNS @ ;
: (NET-POLL) ( ms -- )   NET-CONTROL NET-STEP MS ;
PRIVATE
: FIXED ( ip mask gateway dns -- )   DHCP-STOP  IP-CONFIG  STATIC ON! ;
: NO-ADDRESS ( -- )   DHCP-STOP  0 0 0 0 IP-CONFIG  STATIC OFF! ;
: DOWN ( -- ) NO-ADDRESS ARP-FORGET IP-FORGET UDP-FORGET TCP-DOWN ;
PUBLIC
: NET-STATIC ( ip mask gateway dns -- )   ['] FIXED LOCKED ;
: NET-DHCP ( -- )   ['] NO-ADDRESS LOCKED ;
: NET-DOWN ( -- ) ['] DOWN LOCKED ;
: NET-HOSTNAME ( a u -- )   ['] DHCP-NAME! LOCKED ;

END-PACKAGE
: NETIF ( -- )   LINK-NET ;   \ ALSO NETIF: the layer under the package NET
