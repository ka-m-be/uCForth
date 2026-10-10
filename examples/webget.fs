\ webget.fs - a page from a web server, over HTTP/1.0
\
\ On macOS or Linux, in the folder ucf, with a network:
\   build/ucf-macos-a64
\   ok> ALSO FILES  INCLUDE examples/webget.fs
\   ok> S" example.org" WEBGET
\ The answer of the server comes as text: the lines of its header, an
\ empty line, then the page. The name goes to the DNS server of the
\ network (package DNS). Only http: no https (no TLS).

ALSO FILES  INCLUDE lib/net/host.fs
ONLY FORTH DEFINITIONS  ALSO INET  ALSO NET

CREATE PAGE-BUF 512 ALLOT
: REQUEST ( a u -- )   \ the request for the page / of the host a u; CR gives CR LF
  ." GET / HTTP/1.0" CR  ." Host: " TYPE CR  CR ;
: WEBGET ( a u -- )   \ the page / of the web server a u
  2DUP 80 TCP-OPEN THROW >R
  R@ >SOCKET  REQUEST  >CONSOLE
  BEGIN  PAGE-BUF 512 R@ SOCK-READ THROW  ?DUP WHILE  PAGE-BUF SWAP TYPE  REPEAT
  R> SOCK-CLOSE THROW ;
