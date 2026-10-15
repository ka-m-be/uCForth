\ board.fs - the tests of NET over our TCP/IP on a board, over the loop
\
\ The same tests as lib/net/test/loop.fs, for a board with 32-bit cells
\ (the ESP32 chips): our TCP/IP (on a base image over NO-LINK), the
\ tests of NET (lib/net/test/sockets.fs) and of the buffers (rooms.fs)
\ over the loop, while a timer runs the steps of the network in the
\ background (NET-START, lib/esp32/nettimer.fs). When the steps run
\ already (WLAN-BACK started them), they go on after the end.
\ In ucf term:
\   board ok> #include lib/net/test/board.fs
\ At the end: "board: 128 tests, 0 errors". Each package loads with a
\ short search order (ONLY FORTH), as in lib/net/tcpip.fs.

ONLY FORTH
#require T{ ../../../forth/test/tester.fs
ONLY FORTH
#require TEXT ../../text.fs
ONLY FORTH
#require INET ../inet.fs
ONLY FORTH
#require IDLE ../../idle.fs
ONLY FORTH
#require LINK ../nolink.fs
ONLY FORTH
#require ARP ../arp.fs
ONLY FORTH
#require IPV4 ../ipv4.fs
ONLY FORTH
#require UDP ../udp.fs
ONLY FORTH
#require TCP ../tcp.fs
ONLY FORTH
#require DHCP ../dhcp.fs
ONLY FORTH
#require LINK-NET ../linknet.fs
ONLY FORTH
#require NET ../net.fs
ONLY FORTH
#require DNS ../dns.fs
ONLY FORTH
#require NET-TIMER ../../esp32/nettimer.fs
ONLY FORTH DEFINITIONS  ALSO INTERNALS  ALSO TEXT  ALSO INET  ALSO NET  ALSO NET-TIMER
VARIABLE OWN-STEPS   \ this file started the steps (with WLAN-BACK they run already)
: ?NET-START ( -- )   NET-RUNNING? IF EXIT THEN  NET-START  OWN-STEPS ON! ;
: ?NET-STOP ( -- )   OWN-STEPS @ IF NET-STOP THEN ;
?NET-START
#include sockets.fs
#include rooms.fs
?NET-STOP
S" board" .RESULTS
