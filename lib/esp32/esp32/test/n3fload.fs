\ n3fload.fs - WROOM resources and passive scan; no join, no credentials printed
#reset
ONLY FORTH
#require ESP32-PHY ../phy.fs
ONLY FORTH
#require BYTES ../../../net/bytes.fs
ONLY FORTH
#require ESP32-MAC ../macinit.fs
ONLY FORTH
#require ESP32-RADIO ../radio.fs
ONLY FORTH
#require WIFI ../../../net/wifi.fs
ONLY FORTH
#require SHA1 ../../../sha1.fs
ONLY FORTH
#require KDF1 ../../../kdf1.fs
ONLY FORTH
#require AES128 ../../../aes128.fs
ONLY FORTH
#require CCM ../../../ccm.fs
ONLY FORTH
#require WPAKEY ../../../net/wpakey.fs
ONLY FORTH
#require WLAN ../../../net/wpasta.fs
ONLY FORTH
#require WLSAVE ../wlsave.fs
ONLY FORTH
#require TEXT ../../../text.fs
ONLY FORTH
#require INET ../../../net/inet.fs
ONLY FORTH
#require CCMP ../../../net/ccmp.fs
ONLY FORTH
#require WIFI-LINK ../../../net/wlink.fs
ONLY FORTH ALSO WIFI-LINK
$3FFE0500 WLINK-POOL!   \ SRAM1: the stack of the ROM at the start, free later
ONLY FORTH
#require ARP ../../../net/arp.fs
ONLY FORTH
#require IPV4 ../../../net/ipv4.fs
ONLY FORTH
#require UDP ../../../net/udp.fs
ONLY FORTH
#require TCP ../../../net/tcp.fs
ONLY FORTH
#require DHCP ../../../net/dhcp.fs
ONLY FORTH
#require LINK-NET ../../../net/linknet.fs
ONLY FORTH
#require NET ../../../net/net.fs
ONLY FORTH
#require DNS ../../../net/dns.fs
ONLY FORTH
#require NET-TIMER ../../nettimer.fs
ONLY FORTH DEFINITIONS ALSO INTERNALS
." N3f dictionary/data free: " UNUSED . DATA-END VP @ - . CR
ONLY FORTH ALSO RADIO ALSO WIFI ALSO WLSAVE
RADIO-START RADIO-FACTORY
." passive scan count: " WIFI-SCAN . CR
." saved profile valid: " WLAN-SAVED? . CR
#bye
