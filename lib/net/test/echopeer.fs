\ echopeer.fs - the peer on the host for the socket test over the air (lib/esp32/esp32/test/airsock.fs)
\
\ It sends back each byte of a TCP client on port 47021 and each UDP
\ datagram on port 37021, until a key. The host and the board are in the
\ same network. On macOS or Linux, in the top folder:
\   build/ucf-macos-a64
\   ok> ALSO FILES  INCLUDE lib/net/test/echopeer.fs
\   ok> ECHO-PEER
\ Then the board runs AIR-UDP-CLIENT and AIR-TCP-CLIENT (airsock.fs) with
\ the address of the host. A key ends ECHO-PEER.

ALSO FILES  INCLUDE lib/net/host.fs
ONLY FORTH DEFINITIONS  ALSO INET  ALSO NET

2048 BUFFER: PEER-BUF
VARIABLE TCP-SERVER   VARIABLE TCP-PEER   VARIABLE UDP-PEER
: TCP-STEP ( -- )   \ a client, then its bytes back; a new client after its end
  TCP-PEER @ 0= IF  TCP-SERVER @ TCP-ACCEPT IF TCP-PEER ! THEN  EXIT  THEN
  TCP-PEER @ SOCK-OPEN? 0= IF  TCP-PEER @ SOCK-CLOSE DROP  0 TCP-PEER !  EXIT  THEN
  TCP-PEER @ SOCK-AVAILABLE IF
    PEER-BUF 2048 TCP-PEER @ SOCK-READ THROW  PEER-BUF SWAP TCP-PEER @ SOCK-WRITE THROW
  THEN ;
: UDP-STEP ( -- )   \ a datagram back to its sender
  PEER-BUF 2048 UDP-PEER @ UDP-RECEIVE IF
    >R >R  PEER-BUF SWAP  R> R>  UDP-PEER @ UDP-SEND THROW
  THEN ;
: ECHO-PEER ( -- )
  47021 TCP-LISTEN THROW TCP-SERVER !  0 TCP-PEER !  37021 UDP-OPEN THROW UDP-PEER !
  BEGIN  KEY? 0=  WHILE  TCP-STEP UDP-STEP NET-POLL  REPEAT  KEY DROP
  TCP-PEER @ ?DUP IF SOCK-CLOSE DROP THEN
  TCP-SERVER @ SOCK-CLOSE DROP  UDP-PEER @ SOCK-CLOSE DROP ;
