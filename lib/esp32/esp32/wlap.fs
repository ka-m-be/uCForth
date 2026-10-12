\ wlap.fs - package SETUP-AP: a temporary open WLAN for one device, to set up the board
\
\ SETUP-START makes the board an access point with the given name, open
\ (no password, no encryption). The first device that logs on is the only
\ one until SETUP-STOP. It gets its address by DHCP: the board is
\ 192.168.77.1, the device 192.168.77.2 (netmask 255.255.255.0); there is
\ no router and no DNS server, so the device reaches only the board. The
\ words of NET (TCP, UDP) work as with the station. The station
\ (WLAN-BACK) and the setup AP never run at the same time: RADIO-LINK
\ selects the LINK of the one that runs. Nothing starts the AP by itself.
\ Another device could log on first, and all data is readable: use it
\ only for a short time, and send no secret over it.
\
\   a n ch SETUP-START ( -- )   the AP with the name a n (1 to 32 bytes) on
\                               channel ch (1 to 13)
\   SETUP-STOP ( -- )           the AP, DHCP and the network off; the device
\                               gets a deauthentication
\   .SETUP ( -- )               the numbers of AP-LINK and DHCP-SERVER
\
\ Load it in ucf term after WLAN-BACK and DHCP-SERVER (apps/irbridge/app.fs
\ does it). Example:
\   ALSO WLAN-BACK WBACK-STOP  ALSO SETUP-AP  S" ucf-setup" 6 SETUP-START
\ Then a device logs on to ucf-setup; SETUP-STOP ends it. The test on the
\ host: lib/esp32/test/setup.fs.

PACKAGE SETUP-AP ALSO RADIO ALSO AP-LINK ALSO RADIO-LINK ALSO WLAN-BACK ALSO NETIF ALSO NET-TIMER ALSO DHCP-SERVER ALSO IDLE ALSO INET
192 168 77 1 IP CONSTANT BOARD-IP
192 168 77 2 IP CONSTANT DEVICE-IP
$FFFFFF00 CONSTANT SETUP-MASK
VARIABLE ACTIVE
: SERVICE ( -- )   \ in IDLE: DHCP starts when the device has logged on
  WIFI-AP-READY? IF
    DHCPS-RUNNING? 0= IF  WIFI-AP-PEER DEVICE-IP DHCPS-START  THEN
    DHCPS-TICK
  THEN ;
: START ( a n ch -- )
  LINK-ACCESS  WIFI-AP
  BOARD-IP SETUP-MASK 0 0 NET-STATIC
  ['] SERVICE IDLE+  NET-START  TRUE ACTIVE ! ;
PUBLIC
: SETUP-START ( a n ch -- )
  ACTIVE @ ABORT" SETUP-AP: already active"
  WBACK-RUNNING? ABORT" SETUP-AP: stop station recovery first"
  RADIO-CHANNEL@ ABORT" SETUP-AP: stop the radio first"
  ['] START CATCH ?DUP IF
    >R DHCPS-STOP WIFI-AP-STOP NET-DOWN LINK-STATION R> THROW
  THEN ;
: SETUP-STOP ( -- )
  ACTIVE @ 0= IF EXIT THEN
  NET-STOP ['] SERVICE IDLE- DHCPS-STOP WIFI-AP-STOP NET-DOWN
  LINK-STATION FALSE ACTIVE ! ;
: .SETUP ( -- ) .AP .DHCPS ;
END-PACKAGE
