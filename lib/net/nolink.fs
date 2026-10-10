\ nolink.fs - package NO-LINK: the contract LINK without a network
\
\ A LINK gives our TCP/IP its Ethernet frames (lib/net/README.md). This
\ one has no network: it sends no frame and gets none. Over it, our
\ TCP/IP still talks to itself (loopback: 127.0.0.1, and its own
\ address). So the tests of the network (lib/net/test/net.fs) run over
\ our TCP/IP on each system with 32-bit cells, also on a board before it
\ has WLAN (lib/net/test/loop.fs).
\
\   LINK-MAC ( -- a )        the 6 bytes of the own MAC address
\   LINK-MTU ( -- n )        the largest payload of a frame: 1500
\   LINK-UP? ( -- f )        true: the link is there
\   a u LINK-SEND ( -- ior ) send a frame (here: it goes nowhere)
\   a LINK-RECEIVE ( -- u )  the next frame to a; 0: none (here: never one)
\   ip LINK-JOIN ( -- ior )  let the frames of the group ip in (multicast)
\
\ At the end, LINK gives this package: our TCP/IP writes ALSO LINK.

PACKAGE NO-LINK   \ lib/package.fs

CREATE MAC   $02 C, 0 C, 0 C, 0 C, 0 C, 1 C,   \ locally administered (bit 1 of the first byte)

PUBLIC
: LINK-MAC ( -- a )            MAC ;
: LINK-MTU ( -- n )            1500 ;
: LINK-UP? ( -- f )            TRUE ;
: LINK-SEND ( a u -- ior )     2DROP 0 ;
: LINK-RECEIVE ( a -- u )      DROP 0 ;
: LINK-JOIN ( ip -- ior )      DROP 0 ;

END-PACKAGE
: LINK ( -- )   NO-LINK ;   \ ALSO LINK: the frames under our TCP/IP
