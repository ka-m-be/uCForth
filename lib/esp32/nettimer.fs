\ nettimer.fs - package NET-TIMER: our TCP/IP in the background of a board
\
\ Our TCP/IP works in steps: frames from the LINK, the loop, the timers
\ of TCP, ARP and DHCP (NET-STEP, lib/net/linknet.fs). Without this
\ package the steps run only while a word of NET waits (NET-POLL). With
\ NET-START a timer of the package TIMERS runs a step every millisecond,
\ as a Forth handler (level 1 in lib/net/README.md): the network answers
\ ping, takes connections, sends segments again and renews DHCP also
\ while the console computes, loads a file or waits at the prompt. A
\ step that comes while a word of NETIF runs waits for the end of the
\ word (the lock of LINK-NET).
\
\   NET-START ( -- )   a step every millisecond from now on; after a
\                      restart the program calls it again (TIMERS);
\                      foreground control in IDLE
\   NET-STOP ( -- )    no more steps in the background
\
\ On the board, in ucf term, after our TCP/IP (lib/net/tcpip.fs lists
\ the packages):  #include lib/esp32/nettimer.fs

PACKAGE NET-TIMER  ALSO TIMERS  ALSO NETIF ALSO IDLE   \ lib/package.fs

PUBLIC
: NET-START ( -- )   ['] NET-CONTROL IDLE+ ['] NET-STEP 1 EVERY ;
: NET-STOP ( -- )    ['] NET-STEP 0 EVERY ['] NET-CONTROL IDLE- ;

END-PACKAGE
