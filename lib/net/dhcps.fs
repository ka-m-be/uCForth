\ dhcps.fs - package DHCP-SERVER: the address for the one client of the setup network
\
\ The setup AP (package SETUP-AP, lib/esp32/esp32/wlap.fs) lets one device
\ in: the first that logs on. This server gives that device its IPv4
\ address by DHCP (RFC 2131) and answers no other device: DHCPS-START fixes
\ the client by its MAC address. It answers DISCOVER with OFFER, REQUEST
\ for the given address with ACK (another address: NAK); RELEASE needs no
\ answer. The lease is 600 s. An answer names the server and the netmask,
\ but no router and no DNS server: the network leads nowhere else. The
\ server runs over NET (UDP port 67) on any LINK; NET has its address
\ first (NET-STATIC).
\
\   mac ip DHCPS-START ( -- )   serve the client with the MAC at mac: it gets
\                               ip, an address in the network of MY-IP
\   DHCPS-TICK ( -- )           take and answer one message; it does not wait
\   DHCPS-STOP ( -- )           no more answers; the buffers are cleared
\   DHCPS-RUNNING? ( -- f )
\   .DHCPS ( -- )               the answers sent, the code of the last error
\
\ Load it in ucf term after NET (apps/irbridge/app.fs does it). SETUP-AP
\ starts it after the association of the client, in short:
\   WIFI-AP-PEER  192 168 77 2 IP  DHCPS-START
\ The test on the host: lib/net/test/dhcps.fs.

PACKAGE DHCP-SERVER ALSO NET ALSO NETIF ALSO INET ALSO BYTES
576 BUFFER: MSG                  \ the message that came (DHCP: at most 576 bytes)
576 BUFFER: REPLY
584 BUFFER: ROOM                 \ the buffer of port 67: one message, 8 bytes before it
6 BUFFER: CLIENT                 \ the MAC of the client
VARIABLE CLIENT-IP
VARIABLE SOCK                    \ UDP port 67
VARIABLE RUNNING
VARIABLE MSG-N                   \ the bytes in MSG
VARIABLE MSG-TYPE                \ option 53 of MSG
VARIABLE REQUESTED               \ option 50 of MSG: the address the client asks for; 0: none
VARIABLE SERVER                  \ option 54 of MSG: the server the client took; 0: none
VARIABLE P                       \ the next option in MSG or REPLY
VARIABLE END                     \ the end of MSG
VARIABLE REPLY-TYPE
VARIABLE REPLIES                 \ the answers sent
VARIABLE ERROR                   \ the code of the last error
1 CONSTANT DISCOVER   2 CONSTANT OFFER   3 CONSTANT REQUEST
5 CONSTANT ACK        6 CONSTANT NAK
$63825363 CONSTANT COOKIE        \ before the options
600 CONSTANT LEASE               \ s; renew (T1) after the half, rebind (T2) after 7/8
: OPTION! ( value n code -- )   \ an option with n = 1 or 4 bytes into REPLY at P
  >R DUP P @ 1+ C! R> P @ C!
  1 = IF P @ 2 + C! 3 P +! ELSE P @ 2 + BE32! 6 P +! THEN ;
: OPTIONS ( -- )   \ the options of MSG that the server needs: 53, 50, 54
  0 MSG-TYPE ! 0 REQUESTED ! 0 SERVER ! MSG MSG-N @ + END ! MSG 240 + P !
  BEGIN P @ END @ U< WHILE
    P @ C@ DUP 255 = IF DROP EXIT THEN               \ the end
    DUP 0= IF DROP 1 P +! ELSE                       \ a pad byte
      P @ 2 + END @ U> IF DROP -439 THROW THEN
      P @ 1+ C@ DUP P @ 2 + + END @ U> IF 2DROP -439 THROW THEN
      OVER 53 = IF DUP 1 <> IF 2DROP -439 THROW THEN P @ 2 + C@ MSG-TYPE ! THEN
      OVER 50 = IF DUP 4 <> IF 2DROP -439 THROW THEN P @ 2 + BE32@ REQUESTED ! THEN
      OVER 54 = IF DUP 4 <> IF 2DROP -439 THROW THEN P @ 2 + BE32@ SERVER ! THEN
      2 + P +! DROP
    THEN
  REPEAT ;
: RESPONSE ( type -- )   \ the answer to MSG, to all: the client has no address yet
  REPLY-TYPE ! REPLY 576 0 FILL
  2 REPLY C! 1 REPLY 1+ C! 6 REPLY 2 + C!                  \ a reply; Ethernet, 6 bytes of MAC
  MSG 4 + REPLY 4 + 8 MOVE                                  \ the number, seconds and flags of the client
  REPLY-TYPE @ NAK <> IF CLIENT-IP @ REPLY 16 + BE32! THEN  \ its address
  MY-IP REPLY 20 + BE32!  CLIENT REPLY 28 + 6 MOVE
  COOKIE REPLY 236 + BE32!  REPLY 240 + P !
  REPLY-TYPE @ 1 53 OPTION!  MY-IP 4 54 OPTION!
  REPLY-TYPE @ NAK <> IF
    NETMASK 4 1 OPTION!  LEASE 4 51 OPTION!  LEASE 2/ 4 58 OPTION!  LEASE 7 8 */ 4 59 OPTION!
  THEN
  255 P @ C!
  REPLY 300 -1 68 SOCK @ UDP-SEND DUP ERROR ! THROW  1 REPLIES +! ;
: INPUT ( n -- )   \ a message of n bytes in MSG
  DUP 240 577 WITHIN 0= IF DROP -439 THROW THEN MSG-N !
  MSG C@ 1 <> MSG 1+ C@ 1 <> OR MSG 2 + C@ 6 <> OR IF -439 THROW THEN   \ a request over Ethernet
  MSG 28 + CLIENT 6 BYTES= 0= IF EXIT THEN                 \ another device: no answer
  MSG 24 + BE32@ IF EXIT THEN                              \ over a relay: no answer
  MSG 236 + BE32@ COOKIE <> IF -439 THROW THEN  OPTIONS
  MSG-TYPE @ CASE
    DISCOVER OF  OFFER RESPONSE  ENDOF
    REQUEST OF
      SERVER @ DUP 0<> SWAP MY-IP <> AND IF EXIT THEN      \ it took another server
      REQUESTED @ ?DUP 0= IF MSG 12 + BE32@ THEN           \ else the address it has
      CLIENT-IP @ = IF ACK ELSE NAK THEN RESPONSE
    ENDOF
  ENDCASE ;
: TICK ( -- )   \ a message from port 68: answer it; a wrong one only sets ERROR
  MSG 576 SOCK @ UDP-RECEIVE IF
    68 <> IF 2DROP EXIT THEN  DROP
    ['] INPUT CATCH ?DUP IF  NIP  DUP ERROR !  DUP -439 <> IF THROW THEN  DROP  THEN
  THEN ;
PUBLIC
: DHCPS-START ( peer client-ip -- )
  RUNNING @ ABORT" DHCPS: already running"
  MY-IP 0= ABORT" DHCPS: configure the server address first"
  DUP MY-IP XOR NETMASK AND ABORT" DHCPS: client must be in server subnet"
  DUP NETMASK INVERT AND DUP 0= SWAP NETMASK INVERT = OR ABORT" DHCPS: client host part is 0 or all ones"
  DUP MY-IP = ABORT" DHCPS: client and server have the same address"
  CLIENT-IP !
  DUP C@ 1 AND ABORT" DHCPS: client address must be unicast"
  CLIENT 6 MOVE ROOM 584 67 UDP-OPEN THROW SOCK !
  0 ERROR ! 0 REPLIES ! RUNNING ON! ;
: DHCPS-STOP ( -- )
  RUNNING @ IF SOCK @ SOCK-CLOSE THROW THEN RUNNING OFF!
  CLIENT 6 0 FILL MSG 576 0 FILL REPLY 576 0 FILL ROOM 584 0 FILL ;
: DHCPS-RUNNING? ( -- f ) RUNNING @ ;
: DHCPS-TICK ( -- ) RUNNING @ IF TICK THEN ;
: .DHCPS ( -- ) ." DHCP server replies/code: " REPLIES @ . ERROR @ . CR ;
END-PACKAGE
