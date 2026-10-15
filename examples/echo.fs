\ echo.fs - a server for TCP: a client gets back each byte it sends
\
\ On macOS or Linux, in the folder ucf:
\   build/ucf-macos-a64
\   ok> ALSO FILES  INCLUDE examples/echo.fs
\   ok> 7007 ECHO-SERVER
\ Then in a second terminal:  nc 127.0.0.1 7007
\ Each line comes back. One client at a time: the next one waits until
\ the first closes. A key in the first terminal ends the server.

ALSO FILES  INCLUDE lib/net/host.fs
ONLY FORTH DEFINITIONS  ALSO INET  ALSO NET

CREATE ECHO-BUF 256 ALLOT
CREATE ECHO-ROOM 4096 ALLOT   \ the buffer of the server: our TCP/IP uses it, a system has its own
: ECHO-ONCE ( sock -- )   \ the bytes that wait go back
  >R  ECHO-BUF 256 R@ SOCK-READ THROW  ECHO-BUF SWAP R> SOCK-WRITE THROW ;
: SERVE ( sock -- )   \ until the client closes, or a key
  BEGIN  DUP SOCK-OPEN?  KEY? 0= AND  WHILE
    DUP SOCK-AVAILABLE IF  DUP ECHO-ONCE  ELSE  NET-POLL  THEN
  REPEAT  SOCK-CLOSE DROP ;
: ECHO-SERVER ( port -- )   \ the clients one after the other, until a key
  ECHO-ROOM 4096 ROT TCP-LISTEN THROW
  BEGIN  KEY? 0=  WHILE
    DUP TCP-ACCEPT IF  ." a client" CR  SERVE  ." closed" CR  ELSE  NET-POLL  THEN
  REPEAT  SOCK-CLOSE DROP  KEY DROP ;
