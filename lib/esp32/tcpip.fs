\ tcpip.fs - our TCP/IP on an ESP32 board, for ucf term: all its packages with one #require
\
\ The board version of lib/net/tcpip.fs (that one is for a host, with
\ FILES). It loads TEXT, INET, ARP, IPV4, UDP, TCP, DHCP, LINK-NET, NET, DNS
\ and NET-TIMER (NET-START, NET-STOP) over the LINK that is there: NO-LINK
\ (lib/net/nolink.fs), or the WLAN (lib/esp32/esp32/wlstack.fs). IDLE is in
\ the image of the board. In ucf term:
\   #require NET-TIMER lib/esp32/tcpip.fs

ONLY FORTH
#require TEXT ../text.fs
ONLY FORTH
#require INET ../net/inet.fs
ONLY FORTH
#require ARP ../net/arp.fs
ONLY FORTH
#require IPV4 ../net/ipv4.fs
ONLY FORTH
#require UDP ../net/udp.fs
ONLY FORTH
#require TCP ../net/tcp.fs
ONLY FORTH
#require DHCP ../net/dhcp.fs
ONLY FORTH
#require LINK-NET ../net/linknet.fs
ONLY FORTH
#require NET ../net/net.fs
ONLY FORTH
#require DNS ../net/dns.fs
ONLY FORTH
#require NET-TIMER nettimer.fs
ONLY FORTH
