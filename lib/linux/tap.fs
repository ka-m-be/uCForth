\ tap.fs - package TAP: the contract LINK on Linux, over a TAP device
\
\ A TAP device is a network card of the Linux kernel in software: each
\ Ethernet frame that the kernel sends to it comes to this program, and
\ each frame that this program writes reaches the kernel. So our TCP/IP
\ (lib/net/tcpip.fs) talks to the kernel of Linux as to another
\ computer in the network, and the kernel is a strict partner for the
\ tests (lib/net/test/tap.fs). Only Linux has TAP devices. Making one
\ needs the right CAP_NET_ADMIN; in Docker the container needs
\ --cap-add NET_ADMIN --device /dev/net/tun.
\
\   a u TAP-OPEN ( -- ior )   make the TAP device with the name a u (at
\                             most 15 characters, for example ucf0); its
\                             frames come to this program
\   ip mask TAP-HOST ( -- ior )   the side of Linux: its address, its
\                             mask, and the device on
\   n TAP-LOSE ( -- )         lose each n-th frame in each direction (a
\                             test); 0: none
\   LINK-MAC ... LINK-JOIN    the contract LINK (lib/net/nolink.fs)
\
\ The calls go over the package of the system (ALSO SYSTEM: lib/linux/
\ a64.fs, x64.fs, i386.fs). The numbers of the ioctl calls are the same
\ on each processor of Linux. At the end, LINK gives this package.
\
\ Example (as root, in the folder ucf):
\   ALSO TAP  S" ucf0" TAP-OPEN .  10 0 77 1 IP 255 255 255 0 IP TAP-HOST .

PACKAGE TAP  ALSO SYSTEM  ALSO INET  ALSO FILES   \ lib/package.fs

$400454CA CONSTANT TUNSETIFF        \ the device gets its name and its kind
$0002 CONSTANT IFF-TAP              \ frames of Ethernet, not packets of IP
$1000 CONSTANT IFF-NO-PI            \ no 4 bytes of information before each frame
$8913 CONSTANT SIOCGIFFLAGS   $8914 CONSTANT SIOCSIFFLAGS
$8916 CONSTANT SIOCSIFADDR    $891C CONSTANT SIOCSIFNETMASK
1 CONSTANT IFF-UP
1514 CONSTANT FRAME-MAX             \ 1500 bytes and the head of 14

40 BUFFER: IFR                      \ a struct ifreq: the name (16 bytes), then flags or an address
16 BUFFER: NAME   VARIABLE NAME-N
VARIABLE FD                         \ the device; 0: not open
VARIABLE SOCK                       \ a socket for the ioctl calls of TAP-HOST
VARIABLE LOSE   VARIABLE OUT-N   VARIABLE IN-N
CREATE MAC   $02 C, $75 C, $63 C, $66 C, 0 C, 1 C,   \ locally administered; 75 63 66: "ucf"

: IFR-NAME ( -- )   IFR 40 0 FILL  NAME IFR NAME-N @ MOVE ;
: LE16! ( x a -- )   2DUP C!  SWAP 8 RSHIFT SWAP 1+ C! ;   \ 16 bits, the low byte first
: LE16@ ( a -- x )   DUP C@  SWAP 1+ C@ 8 LSHIFT OR ;
: NONBLOCK ( fd -- )   DUP F-GETFL 0 SYS-FCNTL  O-NONBLOCK OR  F-SETFL SWAP SYS-FCNTL DROP ;
: LOST? ( a -- f )   \ a: the counter of a direction; true: this frame is lost
  LOSE @ 0= IF DROP FALSE EXIT THEN
  1 OVER +!  @ LOSE @ MOD 0= ;
: SET-ADDR ( ip request -- r )   \ an address of the device: SIOCSIFADDR, SIOCSIFNETMASK
  SWAP  IFR-NAME  IFR 16 + SA-FAMILY!  IFR 20 + BE32!
  SOCK @ SWAP IFR SYS-IOCTL ;
: DEVICE-UP ( -- r )   \ the flags of the device, with IFF_UP
  IFR-NAME  SOCK @ SIOCGIFFLAGS IFR SYS-IOCTL DROP
  IFR 16 + LE16@  IFF-UP OR  IFR 16 + LE16!
  SOCK @ SIOCSIFFLAGS IFR SYS-IOCTL ;

PUBLIC
: TAP-OPEN ( a u -- ior )   \ the device a u; its frames come to this program
  15 MIN DUP NAME-N !  NAME SWAP MOVE
  S" /dev/net/tun" R/W OPEN-FILE IF DROP NET-FAILED EXIT THEN     ( fd )
  IFR-NAME  IFF-TAP IFF-NO-PI OR IFR 16 + LE16!
  DUP TUNSETIFF IFR SYS-IOCTL 0< IF  SYS-CLOSE DROP NET-FAILED EXIT  THEN
  DUP NONBLOCK  FD !  0 ;
: TAP-HOST ( ip mask -- ior )   \ the side of Linux: its address and mask, the device on
  AF-INET SOCK-DGRAM 0 SYS-SOCKET  DUP 0< IF DROP 2DROP NET-FAILED EXIT THEN  SOCK !
  SWAP SIOCSIFADDR SET-ADDR  SWAP SIOCSIFNETMASK SET-ADDR OR  DEVICE-UP OR
  SOCK @ SYS-CLOSE DROP
  0< IF NET-FAILED ELSE 0 THEN ;
: TAP-LOSE ( n -- )   LOSE !  0 OUT-N !  0 IN-N ! ;

: LINK-MAC ( -- a )   MAC ;
: LINK-MTU ( -- n )   1500 ;
: LINK-UP? ( -- f )   FD @ 0<> ;
: LINK-SEND ( a u -- ior )
  FD @ 0= IF 2DROP NET-UNREACHABLE EXIT THEN
  OUT-N LOST? IF 2DROP 0 EXIT THEN
  FD @ ROT ROT SYS-WRITE 0< IF NET-FAILED ELSE 0 THEN ;
: LINK-RECEIVE ( a -- u )   \ the next frame; 0: none
  FD @ 0= IF DROP 0 EXIT THEN
  FD @ SWAP FRAME-MAX SYS-READ  DUP 1 < IF DROP 0 EXIT THEN    \ -EAGAIN: none
  IN-N LOST? IF DROP 0 THEN ;
: LINK-JOIN ( ip -- ior )   DROP 0 ;   \ the kernel sends the frames of each group

END-PACKAGE
: LINK ( -- )   TAP ;   \ ALSO LINK: the frames under our TCP/IP
