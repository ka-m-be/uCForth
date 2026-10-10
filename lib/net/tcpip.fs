\ tcpip.fs - our TCP/IP over the LINK that is there: all packages with one INCLUDE
\
\ Loads TEXT, INET and IDLE (if they are not there), then our TCP/IP: ARP,
\ IPV4, UDP, TCP, DHCP and LINK-NET (the layer NETIF), then NET and DNS over
\ it. A LINK must be there first: lib/net/nolink.fs (no network, only
\ the loop), lib/linux/tap.fs (a TAP device of Linux), or the LINK of a
\ board. Each package loads with a short search order, as in
\ lib/net/host.fs; at the end the search order is FORTH FILES INET NET.
\
\ Use (in the folder ucf):
\   ALSO FILES  INCLUDE lib/net/nolink.fs  INCLUDE lib/net/tcpip.fs

ONLY FORTH ALSO FILES  [DEFINED] TEXT      S" lib/text.fs"         ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] INET      S" lib/net/inet.fs"     ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] IDLE      S" lib/idle.fs"         ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] ARP       S" lib/net/arp.fs"      ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] IPV4      S" lib/net/ipv4.fs"     ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] UDP       S" lib/net/udp.fs"      ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] TCP       S" lib/net/tcp.fs"      ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] DHCP      S" lib/net/dhcp.fs"     ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] LINK-NET  S" lib/net/linknet.fs"  ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] NET       S" lib/net/net.fs"      ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] DNS       S" lib/net/dns.fs"      ?INCLUDED
ONLY FORTH ALSO FILES  ALSO INET  ALSO NET
