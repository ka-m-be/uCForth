\ inet.fs - package INET: addresses, byte order and errors of the network
\
\ The base of the packages for the network (lib/net/README.md). It needs
\ no system: the layer under NET (POSIX-NET, later our own TCP/IP), NET
\ itself and each protocol use it. An application needs it for the
\ addresses: ALSO INET  ALSO NET.
\
\ An IPv4 address is one cell: 192.168.1.10 is $C0A8010A. So the network
\ needs cells of 32 bits.
\
\   a b c d IP ( -- ip )             the address a.b.c.d
\   a u >IP ( -- ip true | false )   the address in the text a u: 192.168.1.10
\   ip .IP ( -- )                    show it: 192.168.1.10
\   ip IP>S ( -- a u )               the same as text, in the space of HOLD
\   a BE16@ ( -- x )   x a BE16! ( -- )   16 or 32 bits in the order of the
\   a BE32@ ( -- x )   x a BE32! ( -- )   network: the high byte first
\
\ The errors (ior) of the network, the same on each system; 0 is no error:
\   NET-FAILED       -300   another error of the system
\   NET-REFUSED      -301   no server at this port
\   NET-TIMED-OUT    -302   no answer in time (NET-TIMEOUT)
\   NET-UNREACHABLE  -303   no way to the host: no network, no route
\   NET-CLOSED       -304   the connection is closed or broken
\   NET-NO-HOST      -305   no address for the name (DNS)
\   NET-IN-USE       -306   the port is in use (TCP-LISTEN, UDP-OPEN)
\   NET-NO-BUFFER    -307   no buffer, or one too small (TCP-OPEN, TCP-LISTEN, UDP-OPEN)
\
\ Example:  ALSO INET  192 168 1 10 IP .IP   S" 10.0.0.1" >IP . .IP

PACKAGE INET  ALSO TEXT   \ lib/package.fs, lib/text.fs
: NEED-32 ( -- )   CELL 4 <> ABORT" INET: the network needs cells of 32 bits" ;
NEED-32

PUBLIC
-300 CONSTANT NET-FAILED
-301 CONSTANT NET-REFUSED
-302 CONSTANT NET-TIMED-OUT
-303 CONSTANT NET-UNREACHABLE
-304 CONSTANT NET-CLOSED
-305 CONSTANT NET-NO-HOST
-306 CONSTANT NET-IN-USE
-307 CONSTANT NET-NO-BUFFER

: BE16@ ( a -- x )   DUP C@ 8 LSHIFT  SWAP 1+ C@ OR ;
: BE16! ( x a -- )   OVER 8 RSHIFT OVER C!  1+ C! ;
: BE32@ ( a -- x )   DUP BE16@ 16 LSHIFT  SWAP 2 + BE16@ OR ;
: BE32! ( x a -- )   OVER 16 RSHIFT OVER BE16!  2 + BE16! ;

: IP ( a b c d -- ip )   SWAP 8 LSHIFT OR  SWAP 16 LSHIFT OR  SWAP 24 LSHIFT OR ;
: IP>S ( ip -- a u )   \ 192.168.1.10: the lowest byte first, from the right
  <#  3 0 DO  DUP 255 AND 0 #S 2DROP  '.' HOLD  8 RSHIFT  LOOP  0 #S #> ;
: .IP ( ip -- )   IP>S TYPE SPACE ;

PRIVATE
VARIABLE PART   VARIABLE #DIGITS   VARIABLE BUILT
: PART? ( a u -- a' u' f )   \ a number 0 to 255 at the start of a u, into PART: 1 to 3 digits
  0 PART !  0 #DIGITS !
  BEGIN  DUP IF  OVER C@ '0' - 10 U<  ELSE 0 THEN  WHILE
    OVER C@ '0' -  PART @ 10 * +  PART !  1 #DIGITS +!  1/STRING
  REPEAT
  #DIGITS @ 1 4 WITHIN  PART @ 256 < AND ;
: DOT? ( a u -- a' u' f )   \ a dot at the start of a u; then the text after it
  DUP IF  OVER C@ '.' = IF  1/STRING TRUE EXIT  THEN  THEN  FALSE ;

PUBLIC
: >IP ( a u -- ip true | false )   \ four numbers 0 to 255 with dots between them
  0 BUILT !
  4 0 DO
    I IF  DOT? 0= IF  2DROP UNLOOP FALSE EXIT  THEN  THEN
    PART? 0= IF  2DROP UNLOOP FALSE EXIT  THEN
    BUILT @ 8 LSHIFT  PART @ OR  BUILT !
  LOOP
  NIP IF FALSE EXIT THEN  BUILT @ TRUE ;

END-PACKAGE
