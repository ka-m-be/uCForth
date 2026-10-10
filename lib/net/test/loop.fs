\ loop.fs - the tests of NET over our TCP/IP, without a network
\
\ Our TCP/IP (lib/net/tcpip.fs) over NO-LINK: no frame goes out, and the
\ tests of NET (lib/net/test/sockets.fs) talk over the loop (127.0.0.1),
\ as they do over the sockets of the system. Each system with files and
\ 32-bit cells can run them; on macOS and Linux, in the folder ucf:
\   echo "ALSO FILES  INCLUDE lib/net/test/loop.fs" | build/ucf-macos-a64
\ At the end: "loop: 85 tests, 0 errors". A board runs them with
\ lib/net/test/board.fs.

ONLY FORTH DEFINITIONS  ALSO FILES
INCLUDE forth/test/tester.fs
ONLY FORTH ALSO FILES  [DEFINED] NO-LINK  S" lib/net/nolink.fs"  ?INCLUDED
ONLY FORTH ALSO FILES  INCLUDE lib/net/tcpip.fs
ONLY FORTH DEFINITIONS  ALSO INTERNALS  ALSO TEXT  ALSO INET  ALSO NET  ALSO FILES
INCLUDE lib/net/test/sockets.fs
S" loop" .RESULTS
