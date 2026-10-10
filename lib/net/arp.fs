\ arp.fs - package ARP: the head of the Ethernet frames, and ARP
\
\ The lowest part of our TCP/IP (lib/net/README.md), over the contract
\ LINK. A frame of Ethernet: the MAC address of the receiver (6 bytes),
\ that of the sender (6), the type (2: $0800 IPv4, $0806 ARP), then the
\ payload. ARP (RFC 826) finds the MAC address of an IPv4 address in the
\ own network: a question to all ("who has 10.0.77.1?"), and the owner
\ answers. A table keeps 8 answers for 5 minutes. A frame to an address
\ without an answer waits (one frame; a second one takes its place).
\ ARP asks again after 1 s, at most 3 times. ARP answers each question
\ for the own address, ARP-IP.
\
\   ARP-IP ( -- a )            a variable: the own IPv4 address; 0: none yet
\   ETH-TX ( -- a )            the frame to send; its packet starts at ETH-TX 14 +
\   u mac ETH-SEND ( -- ior )  send ETH-TX with u bytes of IPv4 to mac
\   u ip ARP-SEND ( -- ior )   the same to the computer ip in the own
\                              network: its MAC from the table, or after ARP
\   a u ARP-INPUT ( -- )       a frame of ARP that came
\   ARP-TICK ( -- )            ask again, forget old answers: each step
\   ARP-ANNOUNCE ( -- )        tell all the own address (after DHCP)
\   BROADCAST-MAC ( -- a )     ff:ff:ff:ff:ff:ff
\   ip MULTICAST-MAC ( -- a )  the MAC of a group: 01:00:5e and 23 bits of ip
\   a1 a2 MAC= ( -- f )        the same 6 bytes: two MAC addresses
\   .ARP ( -- )                show the table
\   ARP-FORGET ( -- )          empty the table (tests)
\
\ The fields of a packet go over bytes (C@, BE16@, BE32@), never over @:
\ a frame has no alignment for cells.

PACKAGE ARP  ALSO LINK  ALSO INET   \ lib/package.fs

14 CONSTANT HEAD
$0800 CONSTANT TYPE-IP   $0806 CONSTANT TYPE-ARP
LINK-MTU HEAD + CONSTANT FRAME-SIZE
8 CONSTANT #ENTRIES
16 CONSTANT ENTRY             \ the IP (a cell), the MAC (6 bytes, 2 free), the time (a cell)
300000 CONSTANT MAX-AGE       \ 5 minutes
1000 CONSTANT RETRY           \ 1 s
3 CONSTANT #TRIES

PUBLIC
VARIABLE ARP-IP
FRAME-SIZE BUFFER: ETH-TX

PRIVATE
#ENTRIES ENTRY * BUFFER: TABLE   TABLE #ENTRIES ENTRY * 0 FILL   \ BUFFER: does not clear
FRAME-SIZE BUFFER: PENDING    \ a frame that waits for an answer of ARP
VARIABLE PENDING-N            \ its bytes after the head; 0: none
VARIABLE PENDING-IP   VARIABLE PENDING-TIME   VARIABLE PENDING-TRIES
60 BUFFER: ARP-FRAME          \ a question or an answer of ARP
CREATE ALL-MAC   $FF C, $FF C, $FF C, $FF C, $FF C, $FF C,
CREATE NO-MAC    0 C, 0 C, 0 C, 0 C, 0 C, 0 C,
6 BUFFER: GROUP-MAC                \ MULTICAST-MAC: 01:00:5e, then 23 bits of the group
$01 GROUP-MAC C!  $00 GROUP-MAC 1+ C!  $5E GROUP-MAC 2 + C!

: HEAD! ( mac type a -- )   \ the head of the frame at a: to mac, from us, type
  TUCK 12 + BE16!  TUCK 6 MOVE  LINK-MAC SWAP 6 + 6 MOVE ;
: OUT ( a n -- ior )   \ send the frame a of n bytes; at least 60 (Ethernet)
  DUP 60 < IF  2DUP + 60 ROT - 0 FILL  60  THEN  LINK-SEND ;

\ ---- the table
: E-MAC ( e -- a )   CELL+ ;
: E-TIME ( e -- a )   12 + ;
: ENTRY@ ( i -- e )   ENTRY * TABLE + ;
: FIND ( ip -- e | 0 )   \ the entry of ip
  DUP 0= IF EXIT THEN
  #ENTRIES 0 DO  I ENTRY@ 2DUP @ = IF NIP UNLOOP EXIT THEN DROP  LOOP  DROP 0 ;
: OLDEST ( -- e )   \ a free entry, or else the oldest
  0 ENTRY@
  #ENTRIES 0 DO
    I ENTRY@ @ 0= IF  DROP I ENTRY@ UNLOOP EXIT  THEN
    I ENTRY@ E-TIME @  OVER E-TIME @ -  0< IF  DROP I ENTRY@  THEN
  LOOP ;
: LEARN ( mac ip -- )   \ the MAC of ip into the table, new or again
  DUP FIND ?DUP 0= IF  OLDEST  2DUP !  THEN  NIP     ( mac e )
  MILLIS OVER E-TIME !  E-MAC 6 MOVE ;

\ ---- packets of ARP: after the head, 28 bytes
\ hardware 0, protocol 2, their lengths 4 and 5, op 6, the sender: MAC 8,
\ IP 14; the target: MAC 18, IP 24
: ARP! ( op tha tpa -- )   \ the packet in ARP-FRAME: op, to the MAC tha and the IP tpa
  ARP-FRAME HEAD + >R
  1 R@ BE16!  TYPE-IP R@ 2 + BE16!  6 R@ 4 + C!  4 R@ 5 + C!
  R@ 24 + BE32!  R@ 18 + 6 MOVE  R@ 6 + BE16!
  LINK-MAC R@ 8 + 6 MOVE  ARP-IP @ R> 14 + BE32! ;
: ASK ( ip -- ior )   \ to all: who has ip?
  ALL-MAC TYPE-ARP ARP-FRAME HEAD!
  1 NO-MAC ROT ARP!  ARP-FRAME HEAD 28 + OUT ;
: ANSWER ( mac ip -- ior )   \ to the asker: ARP-IP is at our MAC
  OVER TYPE-ARP ARP-FRAME HEAD!
  2 ROT ROT ARP!  ARP-FRAME HEAD 28 + OUT ;
: SEND-PENDING ( mac -- )   \ the frame that waits goes to mac
  TYPE-IP PENDING HEAD!  PENDING PENDING-N @ HEAD + OUT DROP  0 PENDING-N ! ;
: SENDER ( p -- mac ip )   DUP 8 +  SWAP 14 + BE32@ ;

PUBLIC
: MAC= ( a1 a2 -- f )   OVER BE32@ OVER BE32@ = >R  4 + BE16@  SWAP 4 + BE16@ =  R> AND ;
: BROADCAST-MAC ( -- a )   ALL-MAC ;
: MULTICAST-MAC ( ip -- a )   \ 01:00:5e, then the low 23 bits of the group
  DUP 16 RSHIFT $7F AND GROUP-MAC 3 + C!
  DUP 8 RSHIFT $FF AND GROUP-MAC 4 + C!
  $FF AND GROUP-MAC 5 + C!  GROUP-MAC ;
: ETH-SEND ( u mac -- ior )   TYPE-IP ETH-TX HEAD!  ETH-TX SWAP HEAD + OUT ;
: ARP-SEND ( u ip -- ior )
  DUP FIND ?DUP IF  NIP E-MAC ETH-SEND EXIT  THEN          ( u ip )
  PENDING-IP !  DUP PENDING-N !
  ETH-TX PENDING ROT HEAD + MOVE
  MILLIS PENDING-TIME !  1 PENDING-TRIES !
  PENDING-IP @ ASK ;
: ARP-INPUT ( a u -- )
  HEAD 28 + < IF DROP EXIT THEN  HEAD +                              ( p )
  DUP BE16@ 1 =  OVER 2 + BE16@ TYPE-IP = AND  OVER 4 + BE16@ $0604 = AND
  ARP-IP @ 0<> AND  0= IF DROP EXIT THEN
  DUP 14 + BE32@ FIND IF  DUP SENDER LEARN  THEN                    \ a known sender: its MAC again
  DUP 24 + BE32@ ARP-IP @ <> IF DROP EXIT THEN                      \ not for us
  DUP SENDER LEARN
  PENDING-N @ IF  DUP 14 + BE32@ PENDING-IP @ = IF  DUP 8 + SEND-PENDING  THEN  THEN
  DUP 6 + BE16@ 1 = IF  DUP SENDER ANSWER DROP  THEN
  DROP ;
: ARP-TICK ( -- )
  PENDING-N @ IF  MILLIS PENDING-TIME @ - RETRY < 0= IF
    PENDING-TRIES @ #TRIES < IF
      1 PENDING-TRIES +!  MILLIS PENDING-TIME !  PENDING-IP @ ASK DROP
    ELSE  0 PENDING-N !  THEN                                     \ no answer: the frame is lost
  THEN  THEN
  #ENTRIES 0 DO
    I ENTRY@ @ IF  MILLIS I ENTRY@ E-TIME @ - MAX-AGE > IF  0 I ENTRY@ !  THEN  THEN
  LOOP ;
: ARP-ANNOUNCE ( -- )   ARP-IP @ ?DUP IF ASK DROP THEN ;   \ a question for the own address
: ARP-FORGET ( -- )   TABLE #ENTRIES ENTRY * 0 FILL  0 PENDING-N ! ;
: .ARP ( -- )
  BASE @ >R
  #ENTRIES 0 DO  I ENTRY@ @ ?DUP IF
    .IP  HEX  6 0 DO  J ENTRY@ E-MAC I + C@ 0 <# # # #> TYPE  LOOP  DECIMAL
    ."  age " MILLIS I ENTRY@ E-TIME @ - 1000 / . ." s" CR
  THEN  LOOP
  R> BASE ! ;

END-PACKAGE
