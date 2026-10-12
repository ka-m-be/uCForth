# lib/net: the network

TCP and UDP over IPv4, and names over DNS, with the same words on each
system. An application uses the package NET; under it lies a layer for
the system. On macOS and Linux it uses the TCP/IP of the system. A
system without one (the ESP32 boards) gets our own TCP/IP in Forth:
ARP, IPv4 with ping, UDP, TCP and DHCP over Ethernet frames. Its tests
run on each system over the loop (127.0.0.1), and on Linux against the
kernel over a TAP device. The ESP32 boards get WLAN later; Windows
comes later too.

## Start

On macOS or Linux, in the folder `ucf`:

    $ build/ucf-macos-a64
    ok> ALSO FILES  INCLUDE lib/net/host.fs
    ok> MY-IP .IP  DNS-SERVER .IP
    192.168.0.50 192.168.0.1
    ok> S" example.org" RESOLVE . .IP
    0 172.66.157.237

`host.fs` loads all packages for the system that runs the program. After
it, the search order is `FORTH FILES INET NET`.

A server and a client in one program, over the computer itself:

    ok> 7007 TCP-LISTEN . CONSTANT SERVER
    0
    ok> S" localhost" 7007 TCP-OPEN . CONSTANT CLIENT
    0
    ok> SERVER TCP-ACCEPT . CONSTANT PEER
    -1
    ok> S" hello" CLIENT SOCK-WRITE .
    0
    ok> CREATE BUF 100 ALLOT
    ok> BUF 100 PEER SOCK-READ . BUF SWAP TYPE
    0 hello
    ok> S" 127.0.0.1" 7008 TCP-OPEN . .
    -301 0

-301 is `NET-REFUSED`: no server at port 7008.

Our TCP/IP instead of that of the system, without a network (only the
loop works); the same words:

    ok> ALSO FILES  INCLUDE lib/net/nolink.fs  INCLUDE lib/net/tcpip.fs
    ok> 7007 TCP-LISTEN . CONSTANT SERVER
    0

On Linux, as root, our TCP/IP gets a TAP device, and the kernel is the
other computer of the network (section "Test").

Two examples:

| File | What it does |
| --- | --- |
| `examples/webget.fs` | `S" example.org" WEBGET` shows the answer of a web server: DNS, TCP, `>SOCKET` |
| `examples/echo.fs` | `7007 ECHO-SERVER`: a server that sends back each byte; try it with `nc 127.0.0.1 7007` |

## The words of an application

Package NET, with the addresses from the package INET:
`ALSO INET  ALSO NET`. A connection (`sock`), a server and a UDP
socket (`usock`) are numbers. An error is an `ior` (next section); 0 is
no error.

| Word | Effect |
| --- | --- |
| `NET-UP ( -- ior )` | the network ready: with our TCP/IP the address from DHCP (waits at most `NET-TIMEOUT`); on macOS and Linux nothing (the system did it) |
| `MY-IP`, `GATEWAY`, `NETMASK`, `DNS-SERVER` ( -- ip ) | the addresses of the network; 0: not known |
| `NET-TIMEOUT ( -- a )` | a variable: a word waits at most so many ms (5000) |
| `NET-POLL ( -- )` | the network works one step of at most 1 ms |
| `a u RESOLVE ( -- ip ior )` | the address of a host: `192.168.1.10`, `localhost`, or a name (with the package DNS) |
| `a u port TCP-OPEN ( -- sock ior )` | a connection to the host a u (as for `RESOLVE`) |
| `port TCP-LISTEN ( -- server ior )` | a server at port |
| `server TCP-ACCEPT ( -- sock true \| false )` | a new connection; does not wait |
| `a u sock SOCK-WRITE ( -- ior )` | all bytes |
| `a u sock SOCK-READ ( -- u2 ior )` | the bytes that are there; waits for the first. u2 0: none in time, or the end of the connection |
| `sock SOCK-AVAILABLE ( -- n )` | so many bytes wait |
| `sock SOCK-OPEN? ( -- f )` | the connection stands, or bytes wait |
| `sock SOCK-CLOSE ( -- ior )` | also for a server and a UDP socket |
| `sock >SOCKET`, `>CONSOLE` | the output (`TYPE`, `.`, `EMIT`) goes to the connection, then back to the console |
| `xt sock SOCK-PRINT ( -- ior )` | xt ( -- ) runs with its output to the connection; then the output goes back to the console in each case |
| `port UDP-OPEN ( -- usock ior )` | port 0: any port |
| `a u ip port usock UDP-SEND ( -- ior )` | a datagram; ip `255.255.255.255`: to each computer of the network |
| `a u usock UDP-RECEIVE ( -- u2 ip port true \| false )` | a datagram, with its sender; does not wait |
| `ip usock UDP-JOIN ( -- ior )` | a group for multicast (mDNS) |

Package INET:

| Word | Effect |
| --- | --- |
| `a b c d IP ( -- ip )` | the address a.b.c.d as one cell: `192 168 1 10 IP` is `$C0A8010A` |
| `a u >IP ( -- ip true \| false )` | the address in a text: `S" 192.168.1.10" >IP` |
| `ip .IP ( -- )` | show it: `192.168.1.10` |
| `BE16@ BE16! BE32@ BE32!` | 16 or 32 bits in the order of the network (the high byte first) |

Package DNS: `a u DNS-QUERY ( -- ip ior )` asks the DNS server; the
variables `DNS-IP` (0: `DNS-SERVER`) and `DNS-PORT` (53) select it.
The package gives `DNS-QUERY` to `RESOLVE`, so `TCP-OPEN` takes names.
An answer can name another name (CNAME): `DNS-QUERY` follows it, at
most 4 times. Without an answer, the question goes again after half
of `NET-TIMEOUT`.

## Errors

The same numbers on each system:

| ior | Name | Meaning |
| --- | --- | --- |
| -300 | `NET-FAILED` | another error of the system |
| -301 | `NET-REFUSED` | no server at this port |
| -302 | `NET-TIMED-OUT` | no answer in time (`NET-TIMEOUT`) |
| -303 | `NET-UNREACHABLE` | no way to the host: no network, no route |
| -304 | `NET-CLOSED` | the connection is closed or broken; also a socket that is not open |
| -305 | `NET-NO-HOST` | no address for the name (DNS) |
| -306 | `NET-IN-USE` | the port is in use (`TCP-LISTEN`, `UDP-OPEN`) |

`ior THROW` shows the number: `error: error -301`. Writing into a connection
that the other side closed gives -304; the program goes on (no signal
SIGPIPE).

## Waiting

- A word that waits (`TCP-OPEN`, `SOCK-READ`, `SOCK-WRITE` while the
  system takes no byte, `DNS-QUERY`) does so for at most `NET-TIMEOUT`
  ms, in steps of `NET-POLL`.
- Ctrl-C stops the wait, as it stops `MS`. With our TCP/IP on a board
  it waits until a word of NETIF ends: it never stops TCP in the middle
  of a segment.
- `TCP-ACCEPT`, `UDP-RECEIVE` and `SOCK-AVAILABLE` do not wait. A
  loop that serves more than one thing asks them, then calls `NET-POLL`
  (`examples/echo.fs`). `NET-POLL` waits 1 ms; with our TCP/IP it
  first runs a step of it (section "Our TCP/IP").

## The output to a connection

`sock >SOCKET` sends the output to the connection: `TYPE`, `.`, `EMIT`
and each word that uses them. `>CONSOLE` sends the rest and gives the
output back to the console. The bytes go in parts of 256. The two do not
nest: a second `>SOCKET` ends the first one.

    : HELLO ( sock -- )   >SOCKET  ." hello " 42 .  >CONSOLE ;

If the connection breaks during the output, the output goes back to the
console, then `THROW` gives the ior. An error of the application between
`>SOCKET` and `>CONSOLE` leaves the output at the connection, and the
next prompt goes there too. `SOCK-PRINT` avoids this: it runs a word
with `CATCH`, then gives the output back to the console in each case.
Its ior is the error of the word, or of the connection:

    : TEMPERATURE ( -- )   ." temperature " 22 . ;
    : REPORT ( sock -- ior )   ['] TEMPERATURE SWAP SOCK-PRINT ;

## The layers

As for the ESP32 chips (`lib/esp32/README.md`): each layer has a fixed
set of words, its contract. The code above it knows only this contract.
At the end of a layer file, an alias like `CHIP` names it: the common
code writes `ALSO NETIF` or `ALSO SYSTEM`.

```
  application (apps/, examples/)
  protocols:  DNS (later SNTP, mDNS, HTTP, MQTT)              over NET
  NET:        the words of an application
  ---------------------------------------------------------- contract NETIF
  a system with its own TCP/IP:    | a system without TCP/IP:
  POSIX-NET over SYSTEM            | LINK-NET: our TCP/IP in Forth
  (macOS, Linux); later WINSOCK    |   DHCP  TCP  UDP  IPV4 (ping)  ARP
  (Windows)                        | ---------------------- contract LINK
                                   | the Ethernet frames: NO-LINK (none),
                                   | TAP (Linux), later the WLAN of a board
  next to it, a board with WLAN: WIFI (the networks around, the
  contract WIFI) over the contract RADIO of the chip
```

| File | Package | Contents |
| --- | --- | --- |
| `inet.fs` | INET | addresses, byte order, the errors; no system |
| `posix.fs` | POSIX-NET | NETIF over the sockets of macOS and Linux, over `ALSO SYSTEM`; at the end `NETIF` |
| `net.fs` | NET | the words of an application, over `ALSO NETIF` |
| `dns.fs` | DNS | `DNS-QUERY`, for `RESOLVE` |
| `host.fs` | | loads all for macOS and Linux: `hostsys.fs`, then POSIX-NET, NET, DNS |
| `hostsys.fs` | HOST-NET | loads TEXT, INET and the package of the system (`SYSTEM-LAYER`: which file) |
| `../macos/system.fs` | MACOS | SYSTEM of macOS: `SYSCALL`, the calls and numbers; at the end `SYSTEM` |
| `../linux/a64.fs`, `x64.fs`, `i386.fs` | LINUX-A64, LINUX-X64, LINUX-I386 | SYSTEM of Linux on each processor |
| `arp.fs` | ARP | our TCP/IP: the head of the Ethernet frames, ARP |
| `ipv4.fs` | IPV4 | packets of IPv4, the checksum, the way (own network or gateway), ping, the loop |
| `udp.fs` | UDP | 4 sockets for UDP |
| `tcp.fs` | TCP | 4 connections and 4 servers |
| `dhcp.fs` | DHCP | the own address from the router |
| `linknet.fs` | LINK-NET | NETIF over our TCP/IP; at the end `NETIF` |
| `tcpip.fs` | | loads our TCP/IP over the LINK that is there, then NET and DNS |
| `nolink.fs` | NO-LINK | a LINK without a network: only the loop works; at the end `LINK` |
| `../linux/tap.fs` | TAP | a LINK on Linux: a TAP device; at the end `LINK` |
| `bytes.fs` | BYTES | exact `BYTES= ( a b n -- f )` for packet fields, independent of name/text case rules; `LE16@`, `LE16!`; load before WIFI and ESP32-RADIO |
| `wifi.fs` | WIFI | the Wi-Fi networks around: `WIFI-SCAN` (also in steps: `WIFI-SCAN-START`, `WIFI-SCAN-STEP`), `WIFI-NET`, `.WIFI`; over `ALSO RADIO` and BYTES (section "The contract WIFI") |
| `wpakey.fs` | WPAKEY | the keys of WPA2: PMK, PTK (section "WPA2 key derivation") |
| `wpasta.fs` | WLAN | the WPA2 station: join, the 4-way handshake, group keys, leave (section "WPA2 station") |
| `wlsetup.fs` | WLAN-SETUP | the passphrase typed at the board, the profile saved (section "Local input and saved profiles") |
| `ccmp.fs` | CCMP | the CCMP header, AAD, nonce and the 48-bit packet numbers |
| `wlink.fs` | WIFI-LINK | the contract LINK over the station; the CCMP in the radio, or with `wlsoft.fs` |
| `wlsoft.fs` | WLINK-SOFT | the CCMP of WIFI-LINK in Forth, for a radio without CCMP in its hardware (needs CCM) |
| `aplink.fs` | AP-LINK | the contract LINK over an open access point for one device (section "The setup network") |
| `dhcps.fs` | DHCP-SERVER | the address for that device |
| `linksel.fs` | RADIO-LINK | one LINK of the two for LINK-NET: the station or the AP; at the end `LINK` |
| `test/` | | the tests (section "Test") |

`hostsys.fs` finds the system so: on Linux, `/proc/self/exe` is the
program itself, and its ELF header names the processor; on macOS, the
file `/System/Library/CoreServices/SystemVersion.plist` exists. Each
Mac takes `lib/macos/system.fs`: the HAL of `macos-x64` adds the class
of the call itself.

The packages need 32-bit cells: an address is one cell. On macOS INET,
POSIX-NET, NET, DNS and SYSTEM take 11.8 KB of the dictionary and 1.5 KB
of the RAM data; the package TEXT 0.8 KB and 0.3 KB more. Our TCP/IP:
section "Our TCP/IP".

### The contract NETIF

A handle `h` is the number of a socket. No word waits.

| Word | Effect |
| --- | --- |
| `(TCP-CONNECT) ( ip port -- h ior )` | the connection begins |
| `(TCP-STATE) ( h -- n )` | 1: it stands, or bytes wait; 0: it comes; else the ior why it ended (`NET-CLOSED`, `NET-REFUSED` ...) |
| `(TCP-LISTEN) ( port -- h ior )`, `(TCP-ACCEPT) ( h -- h2 true \| false )` | a server |
| `(SEND) ( a u h -- u2 ior )` | as many bytes as the system takes now |
| `(RECV) ( a u h -- u2 ior )` | the bytes that are there now; 0 0 also at the end of the connection |
| `(AVAILABLE) ( h -- n )`, `(CLOSE) ( h -- ior )` | |
| `(UDP-OPEN) ( port -- h ior )`, `(SENDTO) ( a u ip port h -- ior )`, `(RECVFROM) ( a u h -- u2 ip port true \| false )`, `(JOIN) ( ip h -- ior )` | UDP |
| `(NET-UP) ( -- n )` | 1: the network is up; 0: it comes (DHCP asks); else the ior why not |
| `(MY-IP)`, `(GATEWAY)`, `(NETMASK)`, `(DNS-SERVER) ( -- ip )` | the addresses of the network |
| `(NET-POLL) ( ms -- )` | wait at most ms; on a board, run the TCP/IP meanwhile |

POSIX-NET makes each socket non-blocking. `(MY-IP)` asks the system
which of its addresses goes to the network: a UDP socket with `connect`
to 192.0.2.1 (an address for examples; no packet goes out), then
`getsockname`. `(DNS-SERVER)` is the first line `nameserver a.b.c.d` of
`/etc/resolv.conf`. `(GATEWAY)` and `(NETMASK)` give 0 on macOS and
Linux: the system keeps them for itself. POSIX-NET gives 1 for
`(NET-UP)`: the system made the network.

### The contract SYSTEM

What POSIX-NET needs from the system: `SYSCALL`, the calls `SYS-READ
SYS-WRITE SYS-CLOSE SYS-GETPID SYS-SOCKET SYS-CONNECT SYS-BIND
SYS-LISTEN SYS-ACCEPT SYS-SENDTO SYS-RECVFROM SYS-SETSOCKOPT
SYS-GETSOCKOPT SYS-GETSOCKNAME SYS-GETPEERNAME SYS-FCNTL SYS-IOCTL
SYS-POLL`, their constants (`AF-INET` ... `POLLHUP`), the error numbers
of the system (`EAGAIN` ... `EHOSTUNREACH`), and `SA-FAMILY!` (the
start of a `struct sockaddr_in`: macOS has its length in the first
byte). Each call gives a result of 0 or more, or minus the error
number. Linux on aarch64 has no `poll` (`SYS-POLL` uses `ppoll`); Linux
on i386 has no `accept` (`SYS-ACCEPT` uses `accept4`).

### The contract LINK

What our TCP/IP needs from below: Ethernet frames (the MAC address of
the receiver, of the sender, the type, the payload; no checksum). No
word waits. At the end of a LINK file, `: LINK ( -- ) package ;`.

| Word | Effect |
| --- | --- |
| `LINK-MAC ( -- a )` | the 6 bytes of the own MAC address |
| `LINK-MTU ( -- n )` | the largest payload of a frame: 1500 |
| `LINK-UP? ( -- f )` | the link is there (on a board: the WLAN is joined) |
| `a u LINK-SEND ( -- ior )` | send a frame |
| `a LINK-RECEIVE ( -- u )` | the next frame to a, with room for `LINK-MTU` + 14 bytes; 0: none |
| `ip LINK-JOIN ( -- ior )` | let the frames of the group ip in (multicast) |

Two optional words are empty on an immediate LINK: `LINK-TICK` does
one bounded step of the data path; `LINK-CONTROL` does the work that
may wait, in the foreground. WIFI-LINK, AP-LINK and RADIO-LINK have
both. LINK-NET calls the first inside its lock (in `NET-STEP`, also
from the timer), the second from `NET-CONTROL` (in IDLE, never from a
timer).

A new system or board needs only its lowest layer: a system with
sockets a layer like POSIX-NET (or only a SYSTEM layer, if it has the
calls of POSIX); a board a LINK. A board with WLAN also has the
contract WIFI (next section).

### The contract WIFI

The package WIFI (`wifi.fs`) lists the Wi-Fi networks around. It is
the same on each chip: it uses only the words of the contract RADIO,
the radio of the chip (on the ESP32 the package ESP32-RADIO,
`lib/esp32/README.md`, "The radio of the ESP32: ESP32-RADIO"). The
radio must be on (`RADIO-START`).

| Word | Effect |
| --- | --- |
| `WIFI-SCAN ( -- u )` | listen on the channels 1 to 13, 120 ms each: u networks; the strongest first |
| `i WIFI-NET ( -- a n ch dbm )` | network i (0 to u-1) of the last `WIFI-SCAN`: its name (n bytes at a), its channel and its level in dBm |
| `.WIFI ( -- )` | the list of the last `WIFI-SCAN`: channel, level, name |
| `WIFI-WPA2? ( i -- f )` | true for PSK/CCMP with no required PMF, from the cached beacon |
| `WIFI-PEER ( i -- a )` | BSSID, 6 read-only bytes |
| `WIFI-RSN ( i -- a n cap )` | cached RSN body (up to 128 bytes) and capabilities; error for an unsupported AP |

For background recovery, `WIFI-SCAN-START` begins the same passive
scan and `WIFI-SCAN-STEP ( -- done? )` takes the frames that wait (at
most 8) and, after 120 ms on a channel, goes to the next. Call it from
the foreground until done. `WIFI-SCAN-CANCEL` ends it before its end:
the radio goes back to its channel. `WIFI-SCAN` is the same scan in one
word. It never prints
network identities. Do not run a synchronous scan at the same time.

On the ESP32, in `ucf term`, in the folder `ucf`, right after a reset
(the loads take about 67 s; the names here are examples):

    board ok> #include lib/esp32/esp32/phy.fs
    board ok> #include lib/net/bytes.fs
    board ok> #include lib/esp32/esp32/macinit.fs
    board ok> #include lib/esp32/esp32/radio.fs
    board ok> #include lib/net/wifi.fs
    board ok> ALSO RADIO  ALSO WIFI
    board ok> RADIO-START  WIFI-SCAN .
    5
    board ok> .WIFI
      6  -37  home
      6  -37  home-guest
      6  -80  home
      1  -87  neighbour
     11  -89  printer-direct

An access point sends a beacon to all, about ten times each second
(every 102.4 ms): its address (the BSSID), the name of its network
(the SSID) and its channel. `WIFI-SCAN` sets the filter of a scan
(`RADIO-BEACONS`) and notes each access point once, by its BSSID, with
its strongest level; so a network with two access points shows twice.
The channel comes from the beacon (its element "DS parameter set"): a
beacon also comes on the channels next to it, weaker. Not in the list:
networks without a name (hidden), more than 32 networks. A beacon
at -87 dBm or weaker is lost now and then: such a network can miss in
one scan and show in the next. At the end the radio is on its channel
again, and the filter of the scan stays. A control character in a
name shows as `?`.

The contract RADIO, under WIFI (on the ESP32 in
`lib/esp32/esp32/radio.fs`):

| Word | Effect |
| --- | --- |
| `RADIO-START ( -- )` | the radio on, channel 1; no frames come yet |
| `ch RADIO-CHANNEL! ( -- )`, `RADIO-CHANNEL@ ( -- ch )` | the channel 1 to 13; 0 while the radio is off |
| `RADIO-BEACONS ( -- )` | the filter of a scan: then only beacons come |
| `RADIO-RECEIVE ( -- a n \| 0 )` | the next frame (IEEE 802.11, from its first byte, without its check sum): n bytes at a, until the next `RADIO-RECEIVE`; 0: none |
| `RADIO-LEVEL ( -- dbm )` | the level of that frame |
| `RADIO-MAC! ( a -- )`, `RADIO-MAC@ ( -- a )` | own 6-byte address after each start, copied into the radio; getter bytes are read-only |
| `RADIO-SEND ( a n ack? -- )` | foreground only; ACK needs matching own source MAC and free RX descriptors; broadcast and ACK are tested on the WROOM |
| `a n ack? RADIO-TX-START ( -- )`, `RADIO-TX-POLL ( -- n )` | send without waiting (WIFI-LINK, AP-LINK); the result later: 1 at work, 0 done, else the code |
| `RADIO-TX-BUSY? RADIO-TX-BLOCKED? ( -- f )`, `RADIO-TX-CANCEL ( -- )`, `RADIO-TX-END ( -- )` | the state of such a send; cancel it; let it end first (a stop does that) |
| `RADIO-TRACK ( -- )` | the PHY follows the PLL after some sends; from the foreground |
| `RADIO-FACTORY ( -- )`, `a n RADIO-RANDOM ( -- )` | the own address of the chip; random bytes of the hardware (WLAN) |
| `bssid RADIO-STATION ( -- )`, `RADIO-SOFT-CCMP ( -- )` | the filters of a station; protected frames in software (`wlsoft.fs`) |
| `RADIO-AP ( -- )`, `peer RADIO-AP-PEER! ( -- )` | the filters of an AP (AP-LINK): authentications and probes, then only the frames of the device peer |
| `RADIO-STOP ( -- )` | the radio off |

A radio with CCMP in its hardware also has `key peer id group? RADIO-CCMP-KEY!`,
`RADIO-KEYS-CLEAR`, `RADIO-CCMP? ( -- f )` and `RADIO-DECRYPTED? ( -- f )`
(ESP32-RADIO: `lib/esp32/README.md`, "The WLAN of the WROOM"). They are
optional: WLAN and WIFI-LINK define empty ones with `?:` when the radio
has none, and then WIFI-LINK needs the CCMP in software (`wlsoft.fs`).
The chip gives the packet RAM of the LINKs (ESP32-RADIO: `RADIO-POOL`).

WLAN (`wpasta.fs`, below) provides WIFI-JOIN, WIFI-LEAVE and
WIFI-STATUS. AP-LINK provides the explicitly started open WIFI-AP
(section "The setup network"). WIFI-RSSI is not provided yet.

### WPA2 key derivation (N3d)

`wpakey.fs` is package WPAKEY, above BYTES, SHA1 and KDF1 (`lib/README.md`,
"Protocol arithmetic"). It does no association or radio operation.
Its arithmetic is tested on hosts and the WROOM; WLAN uses it for
the station exchange below.

| Word | Effect |
| --- | --- |
| `WPA-PMK-START ( pass pn ssid sn out -- )` | PBKDF2-HMAC-SHA1, 4096 rounds, 32 output bytes; pn 8..63, sn 1..32 |
| `WPA-PMK-STEP ( budget -- done )` | the bounded PBKDF2 step, at most 8 rounds |
| `WPA-PMK-CANCEL ( -- )` | cancel and erase incomplete output and private derivation state |
| `WPA-PTK ( pmk mac1 mac2 nonce1 nonce2 out -- )` | 64 bytes from the pairwise key expansion PRF |

PMK and nonces have 32 bytes, MACs 6. PTK sorts the two MACs and the
two nonces by their exact unsigned bytes, not by case-insensitive text.
The output starts with KCK (16 bytes), KEK (16), and TK (16).
AES-CCMP uses these first 48 bytes; the last 16 of the traditional
64-byte PRF output are unused. A raw 32-byte PMK may go directly to PTK;
text decoding of a 64-digit key is not part of this package.

For EAPOL descriptor version 2, the MIC is the first 16 HMAC-SHA1
bytes. AES128-UNWRAP checks wrapped key data; CCM provides the CCMP
arithmetic with 13 nonce and 8 tag bytes. Packet parsing, AAD/nonce
construction, replay counters, MIC coverage, key installation and
negotiation belong to N3e/N3f, not to these words.

The intended mode is WPA2-Personal with AES-CCMP, not WPA/TKIP,
WPA3/SAE, enterprise authentication or protected management frames.
A later association must explicitly reject modes it does not support.
Use only owner-entered credentials on the board; the tests use public
vectors and generated byte sequences. Clear caller keys after use.

### WPA2 station (N3e)

`wpasta.fs` is package WLAN, over RADIO, WIFI, BYTES, SHA1, KDF1,
AES128 and WPAKEY. Load with a short search order (`ONLY FORTH` on
a board, `ONLY FORTH ALSO FILES` on a host) before each package.
The search order holds 16 wordlists.

| Word | Effect |
| --- | --- |
| `WIFI-JOIN ( i pmk -- )` | start at entry i of the current scan, with a 32-byte PMK |
| `WIFI-DERIVE ( i pass n -- )` | start bounded PMK derivation; n 8..63; do not put real passphrases in source lines |
| `WIFI-POLL ( -- )` | one foreground step: at most 8 derivation rounds, or one received frame and a bounded retry |
| `WIFI-WAIT ( -- ior )` | blocking foreground form; it may take about 50 s with derivation on the WROOM; not for a term include line |
| `WIFI-STATUS ( -- n )` | state below |
| `WIFI-ERROR ( -- ior )` | 0, or the last failure |
| `WIFI-TK ( -- a )` | 16-byte pairwise key, only in state 6; caller must not change or print it |
| `WIFI-GROUP ( -- a id rsc )` | GTK, key id and 6-byte receive counter, only in state 6; read-only |
| `WIFI-LEAVE ( -- )` | after an association: a deauthentication to the AP (reason 3: the station goes), so a new join works at once; then erase RAM keys and identity, return to scan filters |

States: **0** idle, **1** deriving, **2** open authentication,
**3** association, **4** waiting for EAPOL message 1,
**5** waiting for message 3, **6** authenticated with keys installed,
**7** failed. Leave before starting another join. Failure prints only
the stage and error code, never packet bytes or credentials.

The RSN group update accepts Key Length 0 (as sent by hostap), or 16;
pairwise messages still require 16. Ethernet padding after the declared
EAPOL length is excluded from MIC checks and duplicate comparisons.
The complete declared packet must be present. Group updates retain the
pairwise packet number and do not reinstall an identical group key.

The profile is WPA2-Personal, CCMP for both pairwise and group traffic,
EAPOL descriptor version 2, no required PMF. Open, TKIP, enterprise
and SAE-only networks are rejected before password input. Association
uses legacy b/g rates, not HT, aggregation or power saving. RSN bodies
over 128 bytes are unsupported. The 4-way exchange requires matching
AP RSN data and a 16-byte GTK in message 3.

MIC coverage is the complete EAPOL packet, with its MIC field zeroed,
not the 802.11/LLC headers. Nonces and replay counters are checked.
A repeated message 1 reuses the same station nonce and message 2.
A repeated message 3 must be identical to the accepted EAPOL packet:
it sends message 4 again, without reinstalling keys or resetting packet
numbers. A join has at most 24 transmitted frames and bounded deadlines.
Unexpected Forth exceptions are cleaned up and rethrown, not hidden.

| Code | Meaning |
| --- | --- |
| -2 | explicit RADIO failure (for example ACK timeout); a restart may be needed |
| -410 | key unwrap integrity failure |
| -421 | EAPOL MIC mismatch |
| -422 | authentication, association or key exchange timeout |
| -423 | AP refused authentication/association |
| -425 | stale or changed replayed message 3 |
| -426 | malformed/unsupported exchange or changed nonce/RSN/key data |
| -427 | station nonce source did not produce differing words |
| -428 | deauthentication/disassociation from the selected AP |
| -429 | transmission bound reached |

WLAN stores keys for the software CCMP layer. **This is not an IP
connection yet.** Protected data transport, Ethernet/LINK adaptation,
DHCP/TCP/IP, group rekey, automatic reconnect and the IR combination
belong to N3f. RADIO-SEND still rejects protected frames. Unhandled
key exchanges fail explicitly, rather than pretending the link is ready.
WIFI-POLL may send and run crypto: never call it from a timer, interrupt
or the sub-millisecond NETIF handler.

### Local input and saved profiles

`wlsetup.fs` is package WLAN-SETUP (needs IDLE, WIFI and WLAN).
`WLAN-PASSWORD ( i -- )` reads a passphrase directly into a private
buffer, with board echo off, starts derivation and erases that buffer
on both success and failure. Echo and the Ctrl-C deferral count are
restored. The first Ctrl-C waits until the input line ends; finish it
with Enter. Do not use an input file, command line, chat or a terminal
log for real credentials. The raw 64-hex-digit key input form is not
provided by this prompt; WIFI-JOIN can take a binary PMK.

On the 4-MB WROOM only, `lib/esp32/esp32/wlsave.fs` is package WLSAVE:

| Word | Effect |
| --- | --- |
| `WLAN-SAVE ( -- )` | only after state 6; save SSID and PMK, never the plaintext passphrase |
| `WLAN-SAVED? ( -- f )` | a valid saved profile exists; temporary read buffers are erased |
| `WLAN-SAVED-NET ( -- i )` | strongest supported entry of the latest scan whose SSID matches the saved profile; no names or credentials are printed |
| `WLAN-JOIN ( i -- )` | selected current entry must match the saved SSID exactly; use the PMK privately on the board |
| `WLAN-FORGET ( -- )` | erase both reserved sectors and leave the station |

Call `WIFI-SCAN` just before `WLAN-SAVED-NET WLAN-JOIN`. The index is
not kept across scans or resets. No matching supported network raises
an explicit error; no saved profile throws -431. Temporary profile
buffers are wiped on both paths. Matching an SSID selects a candidate,
not an authenticated peer; the WPA2 exchange still checks the PMK.

Reserve **0x3FE000 and 0x3FF000**, 4096 bytes each. They must not hold
an image, filesystem or another application. Get the owner's approval
before overwriting pre-existing data. The boot image's erase range is
separate. This reservation is not yet a contract for the S3.

Each record has magic/version, a 32-bit sequence, SSID length and
32-byte slot, a 32-byte PMK, a SHA1 checksum, and a 4-byte commit at
offset 96. Save erases only the inactive sector, writes 96 bytes,
commits last and compares all 100 written bytes after reading them
back. Power loss leaves the previous committed record. Damage is
reported; a surviving valid older record is used with a warning.
Both invalid records throw **-430**, no saved record for JOIN throws
**-431**, and a write/readback mismatch throws **-432**. Flash errors
are propagated. All temporary profile buffers are wiped on every exit.

PMK is a credential too: the sectors are **not encrypted** or protected
against physical flash reads. The checksum is for integrity, not
authentication against a malicious flash writer. Keep image builds
separate from provisioning: freeze only while radio is off and RAM keys
are erased, before entering credentials. Do not export an image of a
live provisioned session.

### N3e tests

In `ucf`, without a real network:

    build/ucf-c -32 host/files.fs lib/net/test/wpasta.fs </dev/null
    build/ucf-c -32 host/files.fs lib/esp32/test/wlsave.fs </dev/null
    build/ucf-c -32 host/files.fs lib/net/test/wlsetup.fs </dev/null

Expected: **wpasta 29/0**, **wlsave 20/0**, **wlsetup 2/0**.
The latter loaders repeat the station vectors too. The synthetic AP,
beacon and key packets are generated test data, with independently
computed Node crypto MIC/key-wrap fixtures. No captured frames.
`tools/test.fs` runs them with the other tests.

The station vectors (25, with the deauthentication at the leave) pass
on C and awk; the first 24 also on JavaScript, native macOS and the
WROOM. The WROOM synthetic exchange took **426 ms**, with a timer
running, no PHY/radio/flash access. This is not a data-plane IR test.
Real WROOM: owner-selected AP, local password input, first join and
join after reset with saved PMK both **state 6, error 0, one installation,
four transmissions**. No credential, SSID, BSSID or key byte was printed.

Facts: [EAPOL fields](https://github.com/vanhoefm/hostap-wpa3/blob/master/src/common/wpa_common.h),
[RSN reply rules](https://github.com/vanhoefm/hostap-wpa3/blob/master/src/rsn_supp/wpa.c);
these are protocol references, not copied implementations.

## Our TCP/IP

The packages ARP, IPV4, UDP, TCP and DHCP over the contract LINK, and
LINK-NET as the layer NETIF over them. `tcpip.fs` loads them over the
LINK that is there; a board loads them in `ucf term` (as
`test/board.fs`). What they do:

- **ARP** (RFC 826): answers questions for the own address; asks for the
  MAC of a computer in the own network (or of the gateway). A table
  keeps 8 answers for 5 minutes. A frame waits for the answer (one
  frame); ARP asks again after 1 s, at most 3 times.
- **IPV4** (RFC 791, 1122): checks each packet (version, lengths,
  checksum) and takes those to the own address, to all, and to a group
  of `UDP-JOIN`. No fragments: a packet in pieces is lost, and only what
  fits into one frame goes out (UDP: 1472 bytes; TCP: segments of 1460).
  A ping to the own address gets its answer (ICMP).
- **The loop:** a packet to 127.x.x.x or to the own address does not go
  to the LINK. Each word of NETIF lets these packets come in before it
  ends, as the kernel of a system does: after `TCP-OPEN` to 127.0.0.1
  the server has the connection already.
- **UDP** (RFC 768): 4 sockets with 1.5 KB each for the datagrams that
  wait; a datagram that finds no room is lost.
- **TCP** (RFC 9293), small: 4 connections with 2 KB to receive and 2 KB
  to send each, and 4 servers. In order only (a segment after a gap is
  lost; the acknowledgement tells the other side where the gap
  starts). An acknowledgement for each segment with data, at once;
  data goes at once (no Nagle). The only option is MSS. The time to send
  again (RFC 6298): measured, at least 200 ms, at first 1 s, doubled up
  to 60 s; after 8 times the connection ends with -302. A window of 0:
  a test with one byte, again and again while the other side answers.
  `SOCK-CLOSE` with bytes not read, and data for a connection that the
  program closed, give RST, as in Linux. TIME-WAIT lasts 60 s; a new
  connection may take its place earlier.
- **DHCP** (RFC 2131): DISCOVER, OFFER, REQUEST, ACK; NAK starts again.
  It asks for the mask, the router, the DNS server, the lease, T1 and
  T2, and it tells the name of the computer (option 12): at first `ucf-`
  and the last 3 bytes of the MAC. After half of the lease it asks its
  server for more time, after 7/8 each server; at the end the address
  is gone. A question goes again after 1, 2, 4 ... s, at most 32 s.

The words of LINK-NET besides NETIF:

| Word | Effect |
| --- | --- |
| `ip mask gateway dns NET-STATIC ( -- )` | fixed addresses, no DHCP |
| `NET-DHCP ( -- )` | the addresses from DHCP again (at the start): the next `NET-UP` asks |
| `NET-DOWN ( -- )` | drop addresses, ARP, queued IP/UDP data; active TCP handles report NET-UNREACHABLE and must be closed; listeners and UDP ports remain |
| `a u NET-HOSTNAME ( -- )` | the name for the router (at most 32 characters) |
| `NET-STEP ( -- )` | a step of the network (below); a timer runs it |
| `NET-CONTROL ( -- )` | foreground-only LINK control under the network lock, then NET-SERVICE |

**A step of the network:** one bounded LINK step, at most one frame
from the LINK and one from the loop,
and the timers of ARP, TCP and DHCP. `NET-POLL` runs a step, and each
word of NET that waits runs it with `NET-POLL`. A step does not run
while a word of NETIF runs (a lock): it waits for the end of that word,
which runs it then. Synchronous socket words still flush the loop
before returning. `NET-STEP` does not wait, shows nothing and keeps
its errors, so it may run as a handler of an interrupt.

**In the background of a board:** `lib/esp32/nettimer.fs` (package
NET-TIMER) has `NET-START`: a timer runs `NET-STEP` every 1 ms, also
while the console computes or loads a file (level 1;
`targets/esp32s3/README.md`, "Two levels"). `NET-START` also puts
`NET-CONTROL` in IDLE; `NET-STOP` removes both. Foreground NET-POLL
runs the same control. The
services of an application (MQTT, a web page) run in the chain of IDLE
(level 2) and use the words of NET as everywhere. Ctrl-C waits while a
word of NETIF or a step runs (`-BREAK` of the package IDLE). Measured on
the ESP32-S3: the 85 tests of `test/board.fs` with `NET-START`, 0
errors.

**Over the WLAN of a board:** RADIO-LINK (`linksel.fs`) gives LINK-NET
one LINK of two: WIFI-LINK (the station, `wlink.fs`) or AP-LINK (an
open access point for one device, `aplink.fs`). Only one of them runs;
the board never routes between them, and the mode changes only while
the radio is off. Both use the same packet RAM (`LINK-POOL!`). On the
WROOM: `lib/esp32/README.md`, "The WLAN of the WROOM".

### The setup network

`aplink.fs` (AP-LINK) is the contract LINK over an open access point:
beacons, probe responses, open authentication and (re)association, the
data frames of one device. The first device that logs on is the only
one until `WIFI-AP-STOP`, which sends it a deauthentication. A device
in power save can miss it; it then sees the end some seconds later,
when no beacons come. The AP does not hold frames back for a device in
power save. `dhcps.fs`
(DHCP-SERVER) gives that device its address; the answer names the
netmask, but no router and no DNS server. On the WROOM,
`lib/esp32/esp32/wlap.fs` (SETUP-AP) puts both together: `SETUP-START`,
`SETUP-STOP`, `.SETUP`. The network is open and its data readable: use
it only for a short time, and send no secret over it.

| Word | Effect |
| --- | --- |
| `a AP-POOL! ( -- )`, `AP-POOL-SIZE ( -- n )` | the packet RAM of AP-LINK |
| `a n ch WIFI-AP ( -- )`, `WIFI-AP-STOP ( -- )` | the AP on, off |
| `WIFI-AP-READY? ( -- f )`, `WIFI-AP-PEER ( -- a )` | the device has logged on; its MAC address |
| `mac ip DHCPS-START ( -- )`, `DHCPS-TICK ( -- )`, `DHCPS-STOP ( -- )` | serve the device mac with the address ip; one message per tick |
| `a n LINK-POOL! ( -- )`, `LINK-STATION ( -- )`, `LINK-ACCESS ( -- )`, `LINK-AP? ( -- f )` | RADIO-LINK: the RAM of both LINKs; select one |

**Memory and speed**, measured on the ESP32-S3 at 240 MHz: TEXT, INET,
NO-LINK, our TCP/IP, NET and DNS take 35.7 KB of the dictionary and
35.5 KB of the RAM data (there in the PSRAM). Over the loop, TCP brings
200 KB (100 times 1000 bytes in each way) in 365 ms. The checksum in
Forth costs 0.74 ms for a segment of 1460 bytes.

The fields of a packet go over bytes (`C@`, `BE16@`, `BE32@`), never
over `@`: the ESP32-S3 reads an `@` at an address that 4 does not
divide without an error, but wrong.

## SYSCALL on macOS and Linux

The HAL word 100 calls the kernel:

    x1 .. xn n id SYSCALL ( -- x )

- `id AND $FFFF` is the number of the call (on a Mac with Intel, the
  HAL adds the class `$2000000`). n is at most 6.
- Bit 16 + k of id: argument k + 1 is a VM address. The HAL adds the
  place of the memory of the VM. The reason: macOS loads a program
  above 4 GB, and a cell has 32 bits. An address argument 0 stays 0:
  the null pointer of C (the VM does not use VM address 0).
- Each other argument goes to 64 bits with its sign.
- x is the result, or minus the error number. A second result (X1 or
  RDX on macOS: `pipe`, `fork`) is lost.

`SYSCALL:` in the package of the system writes the bits: one letter
for each argument, `n` a number, `a` an address, `-` none.

    ALSO SYSTEM
    4 SYSCALL: MY-WRITE nan      \ write ( fd a u -- n ): macOS
    1 S" hi" MY-WRITE .

A structure with an address in it (`struct iovec`, `struct msghdr`)
needs the real address, which a cell cannot hold: such calls do not
work with `SYSCALL`.

## Test

The tests of NET (`test/sockets.fs`) run over each layer NETIF. They
need no other computer, and no packet goes to the network: a server and
a client in the program talk over 127.0.0.1 (ports 47011 to 47015):
connect, accept, both ways, 64 KB in each way, the output with
`>SOCKET` and `SOCK-PRINT`, the end from each side, a closed
connection, a peer that sends nothing, a port without a server, UDP
both ways. A small DNS server in Forth answers on 127.0.0.1: an
address, a CNAME with its address, only a CNAME, no such name, an
answer with a wrong number, no answer. The program has one thread, so
these tests run the steps of `DNS-QUERY` one by one, and the server
answers between them.

| File | Over | Where | At the end |
| --- | --- | --- | --- |
| `test/net.fs` | the sockets of the system; with the 4 tests of `SYSCALL` | macOS, Linux | `net: 89 tests, 0 errors` (1.3 s) |
| `test/loop.fs` | our TCP/IP over NO-LINK | each system with files | `loop: 85 tests, 0 errors` (1.1 s on a Mac) |
| `test/board.fs` | our TCP/IP over NO-LINK, the steps in a timer (`NET-START`) | an ESP32 board, in `ucf term` | `board: 85 tests, 0 errors` |
| `test/tap.fs` | our TCP/IP over a TAP device, against the kernel | Linux, as root | `tap: 99 tests, 0 errors` (8 to 12 s) |
| `test/wifi.fs` | WIFI over MOCK-RADIO, with beacons that the test makes; also the scan in steps | each system with files and 32-bit cells | `wifi: 22 tests, 0 errors` (0.1 s) |
| `test/bytes.fs` | exact packet-byte comparison, including unequal case bytes and empty ranges; `LE16@`, `LE16!` | each system with files, 16- or 32-bit cells | `bytes: 9 tests, 0 errors` |
| `test/hwrekey.fs` | with `wpasta.fs`, `wlink.fs`, `hwlink.fs`, `rekey.fs`: the station, WIFI-LINK with CCMP in software and in the radio, a new group key | each system with files and 32-bit cells | `hwrekey: 4 tests, 0 errors` after the others |
| `test/linksel.fs` | RADIO-LINK: the two LINKs, the mode, the shared RAM | the same | `linksel: 6 tests, 0 errors` |
| `test/aphttp.fs` | with `loop.fs` and `dhcps.fs`: DHCP-SERVER, the test reply of the setup network | the same | `aphttp: 4 tests, 0 errors` after the others |
| `test/netdown.fs` | with `loop.fs` and `ping.fs`: `NET-DOWN`, the echo check of `AIR-PING` | the same | `netdown: 10 tests, 0 errors` after the others |
| `test/echopeer.fs` | not a test: the peer on a host for `lib/esp32/esp32/test/airsock.fs` (TCP and UDP echo) | macOS, Linux | |

`test/wifi.fs` checks the parts of a beacon (its elements, channel and
name; 4 bytes more in its header when bit 7 of byte 1 is set), the
list (each access point once, its strongest level, the channel from
the beacon, the strongest first, room for 32), and what is not in it:
hidden networks, other frames, an element longer than the frame, a
frame too short for a beacon. In the folder `ucf`:

    build/ucf-c -32 host/files.fs lib/net/test/wifi.fs </dev/null

On the ESP32 the test of the radio (`lib/esp32/esp32/test/radio.fs`)
receives real beacons.

On macOS or Linux, in the folder `ucf`:

    echo "ALSO FILES  INCLUDE lib/net/test/net.fs" | build/ucf-macos-a64

On Linux, the same with `build/ucf-linux-a64`, `ucf-linux-x64` or
`ucf-linux-i386`; in Docker on a Mac, for example:

    docker run --rm --platform linux/amd64 -v "$PWD":/u -w /u debian:trixie-slim \
      sh -c 'echo "ALSO FILES  INCLUDE lib/net/test/net.fs" | ./build/ucf-linux-x64'

`test/net.fs` needs a network for `UDP-JOIN`: with `--network none` the
kernel has no way for a group, and that one test fails.

**Against the kernel of Linux:** `test/tap.fs` makes the TAP device
`ucf0`. Our TCP/IP is 10.0.77.2, the kernel is 10.0.77.1 at the other
side. Our side uses the words of NET, the side of the kernel those of
POSIX-NET; they do not wait, for the kernel works by itself, so one
thread is enough. The container needs the right to make the device; with
`--network none` no packet reaches another network:

    docker run --rm --network none --cap-add NET_ADMIN --device /dev/net/tun \
      -v "$PWD":/u -w /u debian:bookworm-slim \
      sh -c 'echo "ALSO FILES  INCLUDE lib/net/test/tap.fs" | ./build/ucf-linux-a64'

The tests: ARP in both ways, also with a lost question; UDP in both
ways, to all, 1472 bytes, a group; ping from the kernel (56 and 1472
bytes); TCP in both ways with 100 KB in each way, the end from each
side, RST, a port without a server, an address without a computer; a
window of 0 on our side; each 7th frame lost (`TAP-LOSE`) with 30 KB in
each way; DHCP against a server in Forth at port 67 of the kernel
(DISCOVER with the name, OFFER, REQUEST, ACK, more time after half of
the lease, NAK, a DISCOVER again, the end of the lease, `NET-UP`
without a server).

**Tested (2026):** `test/net.fs` on macOS (Apple silicon; Intel with
Rosetta) and on Linux in Docker (aarch64, x86-64, i386); Ctrl-C during
a wait (macOS, Linux). `test/loop.fs` on macOS and on the three Linux
systems; `test/tap.fs` on the three Linux systems in Docker;
`test/board.fs` on the ESP32-S3. As a cross-check with tools from
outside the project: `nc` as a client of `examples/echo.fs` (lines,
4000 bytes); `examples/webget.fs` with example.org; `RESOLVE` of names
with a CNAME over the DNS server of the network. For our TCP/IP over the
TAP device: `dnsmasq` as the DHCP server (the address, the mask, the
gateway, the DNS server, the name), `ping` from the kernel, `nc` to an
echo server, `curl` to a page of a small HTTP server in Forth.

## Limits

- IPv4 only. No TLS (https, MQTT over TLS): the network is the home
  network.
- 32-bit cells: not on DOS, the PC with a BIOS, the C64.
- One thread: a word that waits stops the rest. A program that serves
  more than one thing asks the words that do not wait, in a loop.
- `GATEWAY` and `NETMASK` give 0 on macOS and Linux.
- A UDP datagram larger than the buffer of `UDP-RECEIVE` loses its rest.
- Our TCP/IP: no fragments; TCP in order only, without window scale; 4
  connections with 2 KB in each way; no IGMP (a group of `UDP-JOIN`
  comes in, but the router does not hear of it: enough for 224.0.0.x,
  for example mDNS); no test of a new address with ARP before DHCP uses
  it (RFC 5227); no protection against an attacker in the own network.
- Windows (Winsock), the WLAN of the ESP32 boards and UEFI do not have
  the network yet.
