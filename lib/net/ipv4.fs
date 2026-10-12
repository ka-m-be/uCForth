\ ipv4.fs - package IPV4: packets of IPv4, ping, and the loop to the own computer
\
\ The part of our TCP/IP above ARP (lib/net/README.md). A packet of IPv4
\ (RFC 791): a head of 20 bytes (the version, lengths, a number, the
\ time to live, the protocol, a checksum, the addresses of sender and
\ receiver), then the part of UDP, TCP or ICMP. IPV4 sends a packet to
\ the computer in the own network, or else to the gateway; to all
\ (255.255.255.255, or the broadcast of the own network); to a group
\ (multicast). A packet that comes is for us if it goes to the own
\ address, to all, or to a group of IP-JOIN. Without an own address
\ (DHCP did not answer yet) each packet is for us.
\
\ No fragments: a packet in pieces is lost, and IPV4 sends only what
\ fits into one frame (UDP: 1472 bytes). ICMP: an echo request (ping)
\ to the own address gets its answer; the other messages are lost.
\
\ The loop: a packet to 127.x.x.x or to the own address does not go to
\ the LINK. It waits in a queue, and IP-FLUSH lets it come in. The layer
\ NETIF (lib/net/linknet.fs) runs IP-FLUSH at the end of each of its
\ words, so the loop works as in the kernel of a system.
\
\   IP-CONFIG ( ip mask gateway dns -- )   the own address, the mask, ...
\   IP-MASK IP-GATEWAY IP-DNS ( -- a )     variables (the own address: ARP-IP)
\   IP-DATA ( -- a )           where UDP, TCP and ICMP build their part
\   proto u dst IP-SEND ( -- ior )   u bytes at IP-DATA as a packet to dst
\   dst IP-SOURCE ( -- ip )    the own address for a packet to dst
\   a u IP-INPUT ( -- )        a packet that came (a: its head)
\   IP-SRC IP-DST ( -- a )     variables: the addresses of the packet that came
\   IP>UDP IP>TCP              deferred ( a u -- ): the part of UDP, TCP
\   x a u IP-SUM ( -- x' )     add the bytes as 16-bit words to the sum
\   x IP-FOLD ( -- c )         the checksum of the sum (0: the sum was right)
\   src dst proto u IP-PSEUDO ( -- x )   the sum of the head that UDP
\                              and TCP check with their bytes
\   ip IP-JOIN ( -- ior )      packets to the group ip come in (4 groups)
\   IP-FLUSH ( -- )            the packets in the loop come in
\   IP-RANDOM ( -- u )         a number of 32 bits (xorshift)

PACKAGE IPV4  ALSO ARP  ALSO LINK  ALSO INET   \ lib/package.fs

14 CONSTANT ETH-HEAD
20 CONSTANT HEAD
1 CONSTANT PROTO-ICMP   6 CONSTANT PROTO-TCP   17 CONSTANT PROTO-UDP
4 CONSTANT #GROUPS
4096 CONSTANT LOOP-SIZE

PUBLIC
VARIABLE IP-MASK   VARIABLE IP-GATEWAY   VARIABLE IP-DNS
VARIABLE IP-SRC    VARIABLE IP-DST
DEFER IP>UDP   DEFER IP>TCP
: IP-DATA ( -- a )   ETH-TX ETH-HEAD + HEAD + ;
: IP-CONFIG ( ip mask gateway dns -- )   IP-DNS !  IP-GATEWAY !  IP-MASK !  ARP-IP ! ;

\ ---- the checksum (RFC 1071)
: IP-SUM ( x a u -- x' )
  DUP 1 AND IF  2DUP + 1- C@ 8 LSHIFT >R  1-  ELSE  0 >R  THEN   \ an odd last byte: the high half of a word
  OVER + SWAP ?DO  I BE16@ +  2 +LOOP  R> + ;
: IP-FOLD ( x -- c )
  BEGIN  DUP 16 RSHIFT  WHILE  DUP $FFFF AND  SWAP 16 RSHIFT +  REPEAT
  INVERT $FFFF AND ;
: IP-PSEUDO ( src dst proto u -- x )
  +  OVER 16 RSHIFT +  SWAP $FFFF AND +  OVER 16 RSHIFT +  SWAP $FFFF AND + ;

PRIVATE
VARIABLE SEED
#GROUPS CELLS BUFFER: GROUPS   GROUPS #GROUPS CELLS 0 FILL   \ BUFFER: does not clear
LOOP-SIZE BUFFER: LOOP-Q   VARIABLE LOOP-N   \ packets that wait: 2 bytes length, then the packet
LINK-MTU BUFFER: LOOP-IN                     \ the packet of the loop that comes in now
VARIABLE FROM-LOOP
VARIABLE NEXT-ID
VARIABLE HL   VARIABLE TL                    \ the length of the head and of the packet that came
: NO-INPUT ( a u -- )   2DROP ;
' NO-INPUT IS IP>UDP   ' NO-INPUT IS IP>TCP

PUBLIC
: IP-RANDOM ( -- u )
  SEED @ ?DUP 0= IF  MILLIS  LINK-MAC 2 + BE32@ XOR  1 OR  THEN
  DUP 13 LSHIFT XOR  DUP 17 RSHIFT XOR  DUP 5 LSHIFT XOR  DUP SEED ! ;
: IP-SOURCE ( dst -- ip )   24 RSHIFT 127 = IF  127 0 0 1 IP  ELSE  ARP-IP @  THEN ;

PRIVATE
: JOINED? ( ip -- f )   FALSE  #GROUPS 0 DO  OVER I CELLS GROUPS + @ = OR  LOOP  NIP ;
: LOCAL? ( ip -- f )   ARP-IP @ XOR  IP-MASK @ AND  0= ;   \ in the own network
: BROADCAST? ( ip -- f )   \ 255.255.255.255, or the broadcast of the own network
  DUP -1 = IF DROP TRUE EXIT THEN
  IP-MASK @ 0= IF DROP FALSE EXIT THEN
  DUP LOCAL?  SWAP IP-MASK @ INVERT TUCK AND =  AND ;
: GROUP? ( ip -- f )   24 RSHIFT 224 240 WITHIN ;
: SELF? ( ip -- f )   \ to this computer: 127.x.x.x, or the own address
  DUP 24 RSHIFT 127 =  SWAP ARP-IP @ =  ARP-IP @ 0<> AND  OR ;
: HEADER ( proto u dst -- )   \ the head in ETH-TX for u bytes at IP-DATA
  ETH-TX ETH-HEAD + >R
  DUP R@ 16 + BE32!  IP-SOURCE R@ 12 + BE32!
  HEAD + R@ 2 + BE16!  R@ 9 + C!
  $4500 R@ BE16!  NEXT-ID @ R@ 4 + BE16!  1 NEXT-ID +!
  $4000 R@ 6 + BE16!  64 R@ 8 + C!                  \ no fragments; time to live 64
  0 R@ 10 + BE16!  0 R@ HEAD IP-SUM IP-FOLD  R> 10 + BE16! ;
: ENQUEUE ( a u -- )   \ a packet into the loop; no room: it is lost
  DUP 2 + LOOP-N @ + LOOP-SIZE > IF 2DROP EXIT THEN
  DUP LOOP-Q LOOP-N @ + BE16!
  LOOP-Q LOOP-N @ + 2 +  SWAP  DUP 2 + LOOP-N +!  MOVE ;
: DEQUEUE ( -- u )   \ the first packet of the loop to LOOP-IN; 0: none
  LOOP-N @ 0= IF 0 EXIT THEN
  LOOP-Q BE16@ >R
  LOOP-Q 2 + LOOP-IN R@ MOVE
  LOOP-Q R@ 2 + +  LOOP-Q  LOOP-N @ R@ 2 + -  MOVE        \ the rest to the front
  R@ 2 + NEGATE LOOP-N +!  R> ;

PUBLIC
: IP-SEND ( proto u dst -- ior )
  2DUP >R >R  HEADER  R> HEAD + R>                        ( n dst )
  DUP SELF? IF  DROP ETH-TX ETH-HEAD + SWAP ENQUEUE 0 EXIT  THEN
  DUP BROADCAST? IF  DROP BROADCAST-MAC ETH-SEND EXIT  THEN
  DUP GROUP? IF  MULTICAST-MAC ETH-SEND EXIT  THEN
  ARP-IP @ 0= IF  2DROP NET-UNREACHABLE EXIT  THEN        \ no own address yet
  DUP LOCAL? 0= IF  DROP IP-GATEWAY @  DUP 0= IF 2DROP NET-UNREACHABLE EXIT THEN  THEN
  ARP-SEND ;
: IP-JOIN ( ip -- ior )
  DUP JOINED? IF DROP 0 EXIT THEN
  #GROUPS 0 DO
    I CELLS GROUPS + @ 0= IF  DUP I CELLS GROUPS + !  LINK-JOIN UNLOOP EXIT  THEN
  LOOP  DROP NET-FAILED ;

PRIVATE
: CHECKED? ( a u -- a u f )   2DUP 0 ROT ROT IP-SUM IP-FOLD 0= ;
: ICMP-INPUT ( a u -- )   \ ping: an echo request to this computer gets its answer
  DUP 8 < IF 2DROP EXIT THEN
  OVER C@ 8 <>  IP-DST @ SELF? 0= OR IF 2DROP EXIT THEN
  CHECKED? 0= IF 2DROP EXIT THEN
  LINK-MTU HEAD - MIN  TUCK IP-DATA SWAP MOVE              ( u )
  0 IP-DATA C!  0 IP-DATA 2 + BE16!                        \ an echo reply
  DUP 0 IP-DATA ROT IP-SUM IP-FOLD  IP-DATA 2 + BE16!
  PROTO-ICMP SWAP IP-SRC @ IP-SEND DROP ;
: BAD-HEAD? ( a u -- f )   \ not a whole packet of IPv4 with a right head
  OVER C@ $F0 AND $40 <> IF 2DROP TRUE EXIT THEN
  OVER C@ $F AND 4 * HL !  OVER 2 + BE16@ TL !
  TL @ <  TL @ HL @ < OR  HL @ HEAD < OR  IF DROP TRUE EXIT THEN
  0 SWAP HL @ IP-SUM IP-FOLD 0<> ;
: FRAGMENT? ( a -- f )   6 + BE16@ $3FFF AND 0<> ;
: FOR-US? ( dst -- f )
  FROM-LOOP @ IF DROP TRUE EXIT THEN
  ARP-IP @ 0= IF DROP TRUE EXIT THEN                    \ no own address yet: each packet (DHCP)
  DUP ARP-IP @ =  OVER BROADCAST? OR  SWAP JOINED? OR ;

PUBLIC
: IP-INPUT ( a u -- )
  DUP HEAD < IF 2DROP EXIT THEN
  2DUP BAD-HEAD? IF 2DROP EXIT THEN
  DROP  DUP FRAGMENT? IF DROP EXIT THEN
  DUP 12 + BE32@ IP-SRC !  DUP 16 + BE32@ IP-DST !
  IP-DST @ FOR-US? 0= IF DROP EXIT THEN
  DUP 9 + C@  SWAP HL @ +  TL @ HL @ -  ROT                ( a' u' proto )
  CASE
    PROTO-ICMP OF ICMP-INPUT ENDOF
    PROTO-UDP OF IP>UDP ENDOF
    PROTO-TCP OF IP>TCP ENDOF
    >R 2DROP R>
  ENDCASE ;
: IP-STEP ( -- )   \ one queued loop packet; synchronous words use IP-FLUSH
  TRUE FROM-LOOP !
  DEQUEUE ?DUP IF LOOP-IN SWAP IP-INPUT THEN
  FALSE FROM-LOOP ! ;
: IP-FORGET ( -- ) 0 LOOP-N ! ;
: IP-FLUSH ( -- )   \ at most 64 packets, so a loop that answers itself ends
  TRUE FROM-LOOP !
  64 0 DO  DEQUEUE ?DUP 0= IF LEAVE THEN  LOOP-IN SWAP IP-INPUT  LOOP
  FALSE FROM-LOOP ! ;

END-PACKAGE
