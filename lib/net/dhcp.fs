\ dhcp.fs - package DHCP: the own address from the router (RFC 2131)
\
\ A DHCP server in the network (at home: the router) gives a computer
\ its address, the mask, the gateway and the DNS server, for a time (the
\ lease). The client asks all (DISCOVER), a server offers an address
\ (OFFER), the client asks for it (REQUEST), the server confirms (ACK)
\ or says no (NAK: the client starts again). After half of the lease
\ (T1) the client asks its server for more time; after 7/8 of it (T2)
\ each server; at the end the address is gone. The client tells its
\ name (option 12), so the router shows it by name. A question goes
\ again after 1, 2, 4 ... s, at most 32 s (RFC 2131 says 4 s first: too
\ long for NET-UP). A lease longer than 24 days counts as 24 days
\ (MILLIS wraps after 49 days).
\
\   DHCP-START ( -- )      ask for an address, if DHCP does not run yet;
\                          the steps go on in DHCP-TICK
\   DHCP-STOP ( -- )       no more DHCP; the address stays
\   DHCP-TICK ( -- )       answers and times: each step of the network
\   DHCP-BOUND? ( -- f )   DHCP gave an address, and it is valid
\   a u DHCP-NAME! ( -- )  the name for the router (at most 32 characters);
\                          at first ucf- and the last 3 bytes of the MAC
\   .DHCP ( -- )           show the state
\
\ The words run inside a step of the network (lib/net/linknet.fs).

PACKAGE DHCP  ALSO UDP  ALSO IPV4  ALSO ARP  ALSO LINK  ALSO INET   \ lib/package.fs

67 CONSTANT SERVER-PORT   68 CONSTANT CLIENT-PORT
$63825363 CONSTANT MAGIC          \ the start of the options
1024 CONSTANT MSG-SIZE
2073600 CONSTANT LEASE-MAX        \ 24 days in seconds
\ the states of the client
0 CONSTANT OFF   1 CONSTANT SELECTING   2 CONSTANT REQUESTING
3 CONSTANT BOUND   4 CONSTANT RENEWING   5 CONSTANT REBINDING
\ the types of the messages (option 53)
1 CONSTANT DISCOVER   2 CONSTANT OFFER   3 CONSTANT REQUEST   5 CONSTANT DHCPACK   6 CONSTANT NAK

VARIABLE D-STATE
VARIABLE SOCK                     \ the UDP socket at port 68
VARIABLE XID                      \ the number of the questions
VARIABLE NEXT-TIME   VARIABLE WAIT   VARIABLE TRIES
VARIABLE OFFERED   VARIABLE SERVER-ID   VARIABLE LEASE
VARIABLE T1-AT   VARIABLE T2-AT   VARIABLE END-AT
VARIABLE A-TYPE   VARIABLE A-SERVER   VARIABLE A-MASK   VARIABLE A-ROUTER   \ from an answer
VARIABLE A-DNS    VARIABLE A-LEASE    VARIABLE A-T1     VARIABLE A-T2
MSG-SIZE BUFFER: MSG   VARIABLE P   VARIABLE M-END
34 BUFFER: NAME   VARIABLE NAME-N

: PAST? ( t -- f )   MILLIS - 0< ;
: DUE? ( -- f )   NEXT-TIME @ PAST? ;

\ ---- the name
: DIGIT ( n -- c )   DUP 9 > IF 87 + ELSE 48 + THEN ;   \ 0-9 a-f
: DIGIT, ( n -- )   DIGIT  NAME NAME-N @ + C!  1 NAME-N +! ;
: DEFAULT-NAME ( -- )   \ ucf- and the last 3 bytes of the MAC, in hex
  S" ucf-" NAME SWAP MOVE  4 NAME-N !
  6 3 DO  LINK-MAC I + C@  DUP 4 RSHIFT DIGIT,  $F AND DIGIT,  LOOP ;

\ ---- a message of the client
: B, ( c -- )   P @ C!  1 P +! ;
: W, ( x -- )   P @ BE16!  2 P +! ;
: L, ( x -- )   P @ BE32!  4 P +! ;
: OPTION, ( a u code -- )   B,  DUP B,  P @ SWAP  DUP P +!  MOVE ;
: OWN-ADDRESS? ( -- f )   D-STATE @ DUP RENEWING =  SWAP REBINDING = OR ;   \ it asks for its address
: MESSAGE ( type -- a u )   \ DISCOVER or REQUEST, at least 300 bytes (BOOTP)
  NAME-N @ 0= IF DEFAULT-NAME THEN
  MSG MSG-SIZE 0 FILL  MSG P !
  1 B,  1 B,  6 B,  0 B,  XID @ L,  0 W,  0 W,        \ a request, Ethernet, a MAC of 6 bytes; no hops, no time, no flags
  OWN-ADDRESS? IF ARP-IP @ ELSE 0 THEN L,             \ ciaddr
  0 L,  0 L,  0 L,                                    \ yiaddr siaddr giaddr
  LINK-MAC P @ 6 MOVE  16 P +!  192 P +!              \ chaddr; sname and file stay empty
  MAGIC L,
  53 B,  1 B,  B,                                     \ the type
  61 B,  7 B,  1 B,  LINK-MAC P @ 6 MOVE  6 P +!      \ the client: Ethernet and the MAC
  NAME NAME-N @ 12 OPTION,
  D-STATE @ REQUESTING = IF  50 B, 4 B, OFFERED @ L,  54 B, 4 B, SERVER-ID @ L,  THEN
  55 B,  6 B,  1 B,  3 B,  6 B,  51 B,  58 B,  59 B,  \ mask, router, DNS server, lease, T1, T2
  255 B,
  MSG  P @ MSG -  300 MAX ;
: ASK ( type -- )   \ send a message; the next question waits twice as long
  MESSAGE  D-STATE @ RENEWING = IF SERVER-ID @ ELSE -1 THEN  SERVER-PORT  SOCK @ (SENDTO) DROP
  MILLIS WAIT @ + NEXT-TIME !  WAIT @ 2* 32000 MIN WAIT ! ;
: NEW-STATE ( state -- )   D-STATE !  1000 WAIT !  0 TRIES ! ;

\ ---- an answer of a server
: OURS? ( u -- f )   \ an answer to our question: BOOTREPLY, our number, our MAC
  240 < IF FALSE EXIT THEN
  MSG C@ 2 =  MSG 4 + BE32@ XID @ = AND  MSG 28 + LINK-MAC MAC= AND  MSG 236 + BE32@ MAGIC = AND ;
: OPTION ( p code -- )   \ one option at p: its code, its length, its value
  CASE
    53 OF  2 + C@ A-TYPE !  ENDOF
    54 OF  2 + BE32@ A-SERVER !  ENDOF
    1 OF  2 + BE32@ A-MASK !  ENDOF
    3 OF  2 + BE32@ A-ROUTER !  ENDOF
    6 OF  2 + BE32@ A-DNS !  ENDOF
    51 OF  2 + BE32@ A-LEASE !  ENDOF
    58 OF  2 + BE32@ A-T1 !  ENDOF
    59 OF  2 + BE32@ A-T2 !  ENDOF
    NIP
  ENDCASE ;
: OPTIONS ( u -- )   \ the options of the answer of u bytes in MSG
  0 A-TYPE !  0 A-SERVER !  0 A-MASK !  0 A-ROUTER !  0 A-DNS !  0 A-LEASE !  0 A-T1 !  0 A-T2 !
  MSG + M-END !  MSG 240 +
  BEGIN  DUP M-END @ U<  WHILE
    DUP C@ 255 = IF DROP EXIT THEN
    DUP C@ 0= IF  1+  ELSE
      DUP 1+ C@ 2 +  OVER +  M-END @ U> IF DROP EXIT THEN    \ cut short: not the rest
      DUP DUP C@ OPTION  DUP 1+ C@ 2 + +
    THEN
  REPEAT  DROP ;
: TIMES ( -- )   \ the lease from the ACK: T1, T2, the end
  A-LEASE @ ?DUP 0= IF 3600 THEN  LEASE-MAX MIN  LEASE !
  LEASE @ 1000 * MILLIS + END-AT !
  A-T1 @ ?DUP 0= IF LEASE @ 2/ THEN  LEASE-MAX MIN 1000 * MILLIS + T1-AT !
  A-T2 @ ?DUP 0= IF LEASE @ 7 * 8 / THEN  LEASE-MAX MIN 1000 * MILLIS + T2-AT ! ;
: BIND ( -- )   \ an ACK: the address and the times
  MSG 16 + BE32@  A-MASK @  A-ROUTER @  A-DNS @  IP-CONFIG
  A-SERVER @ ?DUP IF SERVER-ID ! THEN
  TIMES  BOUND NEW-STATE  ARP-ANNOUNCE ;
: RESTART ( -- )   \ from the start: no address, DISCOVER
  0 0 0 0 IP-CONFIG  SELECTING NEW-STATE  IP-RANDOM XID !  DISCOVER ASK ;
: ASKING? ( -- f )   D-STATE @ DUP REQUESTING =  OVER RENEWING = OR  SWAP REBINDING = OR ;
: REPLY ( -- )   \ the answer in MSG, with its options
  D-STATE @ SELECTING =  A-TYPE @ OFFER = AND IF
    MSG 16 + BE32@ OFFERED !  A-SERVER @ SERVER-ID !
    REQUESTING NEW-STATE  REQUEST ASK  EXIT
  THEN
  ASKING? 0= IF EXIT THEN
  A-TYPE @ DHCPACK = IF  BIND  EXIT  THEN
  A-TYPE @ NAK = IF  RESTART  THEN ;
: ANSWERS ( -- )   \ the datagrams at port 68
  BEGIN  MSG MSG-SIZE SOCK @ (RECVFROM)  WHILE      ( u ip port )
    2DROP  DUP OURS? IF  OPTIONS REPLY  ELSE  DROP  THEN
  REPEAT ;

PUBLIC
: DHCP-BOUND? ( -- f )   D-STATE @ DUP BOUND =  OVER RENEWING = OR  SWAP REBINDING = OR ;
: DHCP-START ( -- )
  D-STATE @ OFF <> IF EXIT THEN
  SOCK @ 0= IF  CLIENT-PORT (UDP-OPEN) IF DROP EXIT THEN  SOCK !  THEN    \ port 68 in use: no DHCP
  RESTART ;
: DHCP-STOP ( -- )   OFF D-STATE !  SOCK @ ?DUP IF  (UDP-CLOSE) DROP  0 SOCK !  THEN ;
: DHCP-TICK ( -- )
  D-STATE @ OFF = IF EXIT THEN
  ANSWERS
  D-STATE @ CASE
    SELECTING OF  DUE? IF DISCOVER ASK THEN  ENDOF
    REQUESTING OF  DUE? IF  1 TRIES +!  TRIES @ 4 > IF RESTART ELSE REQUEST ASK THEN  THEN  ENDOF
    BOUND OF  T1-AT @ PAST? IF  RENEWING NEW-STATE  REQUEST ASK  THEN  ENDOF
    RENEWING OF
      T2-AT @ PAST? IF  REBINDING NEW-STATE  REQUEST ASK  ELSE  DUE? IF REQUEST ASK THEN  THEN
    ENDOF
    REBINDING OF  END-AT @ PAST? IF  RESTART  ELSE  DUE? IF REQUEST ASK THEN  THEN  ENDOF
  ENDCASE ;
: DHCP-NAME! ( a u -- )   32 MIN  DUP NAME-N !  NAME SWAP MOVE ;
: .DHCP ( -- )
  ." DHCP: "
  D-STATE @ CASE
    OFF OF ." off" ENDOF            SELECTING OF ." asks" ENDOF
    REQUESTING OF ." requests" ENDOF  BOUND OF ." bound" ENDOF
    RENEWING OF ." renews" ENDOF    REBINDING OF ." rebinds" ENDOF
  ENDCASE
  DHCP-BOUND? IF
    ." , server " SERVER-ID @ .IP  ." lease " LEASE @ . ." s, its end in " END-AT @ MILLIS - 1000 / . ." s"
  THEN  CR ;

END-PACKAGE
