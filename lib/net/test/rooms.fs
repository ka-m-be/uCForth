\ rooms.fs - the buffers of the program in our TCP/IP (lib/net/)
\
\ Only over our TCP/IP: the sockets of macOS and Linux have their own
\ buffers. The data wait in the buffer of the socket; what does not fit
\ is lost; after SOCK-CLOSE the buffer belongs to the program again; a
\ buffer at the address 0 takes nothing. lib/net/test/loop.fs and
\ board.fs load it after sockets.fs (BUF, the search order).

\ ---- UDP: each datagram with 8 bytes before it
30 BUFFER: SMALL   20 BUFFER: OTHER
VARIABLE U-IN   VARIABLE U-OUT   VARIABLE CELL0
T{ SMALL 30 47017 UDP-OPEN SWAP U-IN ! -> 0 }T
T{ OTHER 20 47018 UDP-OPEN SWAP U-OUT ! -> 0 }T
: SEND-IN ( a u -- ior )   127 0 0 1 IP 47017 U-OUT @ UDP-SEND ;
T{ S" 1234567890" SEND-IN  S" abcd" SEND-IN  S" x" SEND-IN -> 0 0 0 }T  \ 18 and 12 bytes: full
T{ BUF 100 U-IN @ UDP-RECEIVE NIP NIP -> 10 TRUE }T
T{ BUF 100 U-IN @ UDP-RECEIVE NIP NIP -> 4 TRUE }T
T{ BUF 100 U-IN @ UDP-RECEIVE -> FALSE }T                           \ "x" found no room
T{ S" again" SEND-IN  BUF 100 U-IN @ UDP-RECEIVE NIP NIP -> 0 5 TRUE }T
T{ U-IN @ SOCK-CLOSE -> 0 }T
: UNTOUCHED? ( -- f )   TRUE  SMALL 30 BOUNDS ?DO  I C@ 'z' <> IF DROP FALSE THEN  LOOP ;
T{ SMALL 30 'z' FILL  S" late" SEND-IN  UNTOUCHED? -> 0 TRUE }T     \ closed: the buffer stays as it is
ALSO NETIF
T{ 0 @ CELL0 !  0 100 47019 (UDP-OPEN) SWAP U-IN ! -> 0 }T          \ the layer under NET takes it
T{ S" zero" 127 0 0 1 IP 47019 U-OUT @ UDP-SEND  0 @ CELL0 @ = -> 0 TRUE }T
T{ BUF 100 U-IN @ UDP-RECEIVE -> FALSE }T
PREVIOUS
T{ U-IN @ SOCK-CLOSE  U-OUT @ SOCK-CLOSE -> 0 0 }T

\ ---- TCP: the first half of the buffer receives, the second sends
47041 CONSTANT PORT-A   47042 CONSTANT PORT-B
4096 BUFFER: ROOM-A   4096 BUFFER: ROOM-B   64 BUFFER: TINY
VARIABLE SRV-A   VARIABLE SRV-B   VARIABLE C1   VARIABLE C2   VARIABLE P1   VARIABLE P2
: POLLS ( n -- )   0 ?DO NET-POLL LOOP ;
: LOCAL ( a u port -- sock ior )   >R S" 127.0.0.1" R> TCP-OPEN ;
T{ ROOM-A 4096 PORT-A TCP-LISTEN SWAP SRV-A ! -> 0 }T
\ a client with 32 bytes in each way: 1000 bytes there and back
T{ TINY 64 PORT-A LOCAL SWAP C1 ! -> 0 }T
T{ SRV-A @ TCP-ACCEPT SWAP P1 ! -> TRUE }T
C1 @ CLIENT !  P1 @ PEER !
T{ 0 ROUND  77 ROUND -> TRUE TRUE }T
\ a server gives its buffer to one connection at a time: the SYN of a
\ second client is lost (it would come again after 1 s); after the end
\ of the first connection the next client gets the buffer at once
T{ ROOM-B 4096 S" 127.0.0.1" PORT-A TCP-CONNECT SWAP C2 ! -> 0 }T
T{ 20 POLLS  C2 @ SOCK-STATE  SRV-A @ TCP-ACCEPT -> 0 FALSE }T
T{ C2 @ SOCK-CLOSE  C1 @ SOCK-CLOSE  P1 @ SOCK-CLOSE -> 0 0 0 }T
T{ ROOM-B 4096 PORT-A LOCAL SWAP C2 !  SRV-A @ TCP-ACCEPT SWAP P2 ! -> 0 TRUE }T
T{ S" two" C2 @ SOCK-WRITE  BUF 100 P2 @ SOCK-READ  BUF 3 S" two" S= -> 0 3 0 TRUE }T
T{ C2 @ SOCK-CLOSE  P2 @ SOCK-CLOSE -> 0 0 }T
\ SOCK-CLOSE with bytes that the other side has not acknowledged: they
\ still go from the buffer (a server with 32 bytes to receive)
VARIABLE C3   VARIABLE P3
: FIRST-100? ( -- f )   TRUE  100 0 DO  BUF I + C@ I <> IF DROP FALSE THEN  LOOP ;
T{ TINY 64 PORT-B TCP-LISTEN SWAP SRV-B ! -> 0 }T
T{ ROOM-B 4096 PORT-B LOCAL SWAP C3 ! -> 0 }T
T{ SRV-B @ TCP-ACCEPT SWAP P3 ! -> TRUE }T
T{ 0 PATTERN  BUF 100 C3 @ SOCK-WRITE  C3 @ SOCK-CLOSE -> 0 0 }T
T{ P3 @ SOCK-AVAILABLE -> 32 }T
T{ BUF 100 0 FILL  100 P3 @ READ-ALL  FIRST-100? -> TRUE TRUE }T
T{ BUF 100 P3 @ SOCK-READ  P3 @ SOCK-CLOSE -> 0 0 0 }T      \ the end, after the last byte
\ a new connection with that buffer before the acknowledgement: RST for the old one
VARIABLE C4   VARIABLE P4
T{ ROOM-B 4096 PORT-B LOCAL SWAP C3 ! -> 0 }T
T{ SRV-B @ TCP-ACCEPT SWAP P3 ! -> TRUE }T
T{ 0 PATTERN  BUF 100 C3 @ SOCK-WRITE  C3 @ SOCK-CLOSE -> 0 0 }T
T{ ROOM-B 4096 PORT-A LOCAL SWAP C4 ! -> 0 }T
T{ SRV-A @ TCP-ACCEPT SWAP P4 ! -> TRUE }T
T{ BUF 100 P3 @ SOCK-READ  P3 @ SOCK-STATE -> 32 0 NET-CLOSED }T
T{ S" four" C4 @ SOCK-WRITE  BUF 100 P4 @ SOCK-READ  BUF 4 S" four" S= -> 0 4 0 TRUE }T
T{ C4 @ SOCK-CLOSE  P4 @ SOCK-CLOSE  P3 @ SOCK-CLOSE -> 0 0 0 }T
T{ SRV-A @ SOCK-CLOSE  SRV-B @ SOCK-CLOSE -> 0 0 }T
