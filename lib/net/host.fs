\ host.fs - the network on macOS and Linux: all packages with one INCLUDE
\
\ Loads the packages of the network for the system that runs this
\ program: TEXT, INET and the layer of the system (SYSTEM) with
\ lib/net/hostsys.fs, then POSIX-NET (the layer NETIF), NET and DNS. A
\ package that is there already does not load again.
\
\ Each package loads with a short search order (ONLY FORTH ALSO FILES):
\ a package takes up to 6 more places while it loads, and the search
\ order has 16. At the end the search order is FORTH FILES INET NET.
\
\ Use (in the folder ucf):  ALSO FILES  INCLUDE lib/net/host.fs

ONLY FORTH ALSO FILES  [DEFINED] HOST-NET  S" lib/net/hostsys.fs" ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] NETIF   S" lib/net/posix.fs" ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] NET     S" lib/net/net.fs"   ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] DNS     S" lib/net/dns.fs"   ?INCLUDED
ONLY FORTH ALSO FILES  ALSO INET  ALSO NET
