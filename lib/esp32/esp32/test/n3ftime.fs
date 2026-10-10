\ n3ftime.fs - public zero-key arithmetic budget and TCP/IP memory; no RF or flash
#reset
ONLY FORTH
#require AES128 ../../../aes128.fs
ONLY FORTH
#require NO-LINK ../../../net/nolink.fs
ONLY FORTH
#require TEXT ../../../text.fs
ONLY FORTH
#require INET ../../../net/inet.fs
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
ONLY FORTH DEFINITIONS ALSO INTERNALS ALSO AES128
16 BUFFER: TEST-KEY
16 BUFFER: TEST-BLOCK
TEST-KEY 16 0 FILL TEST-BLOCK 16 0 FILL TEST-KEY AES128-KEY
: ENCS ( -- ) 100 0 DO TEST-BLOCK TEST-BLOCK AES128-ENCRYPT LOOP ;
: KEYS ( -- ) 100 0 DO TEST-KEY AES128-KEY LOOP ;
: DECS ( -- ) 20 0 DO TEST-BLOCK TEST-BLOCK AES128-DECRYPT LOOP ;
: TIME ( xt -- n ) MILLIS >R EXECUTE MILLIS R> - ;
." AES encrypt 100 blocks ms: " ' ENCS TIME . CR
." AES key 100 schedules ms: " ' KEYS TIME . CR
." AES decrypt 20 blocks ms: " ' DECS TIME . CR
." after TCP/IP and AES dictionary/data free: " UNUSED . DATA-END VP @ - . CR
#reset
#bye
