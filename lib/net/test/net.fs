\ net.fs - tests of the network over the sockets of macOS and Linux (lib/net/)
\
\ Use (in the folder ucf):
\   echo "ALSO FILES  INCLUDE lib/net/test/net.fs" | build/ucf-macos-a64
\ (on Linux: build/ucf-linux-a64, ucf-linux-x64 or ucf-linux-i386). It loads
\ lib/net/host.fs, if NET is not there; then the tests of NET
\ (lib/net/test/sockets.fs) and the four tests of SYSCALL. At the end:
\ "net: 89 tests, 0 errors".

ONLY FORTH DEFINITIONS  ALSO FILES
INCLUDE forth/test/tester.fs
ONLY FORTH ALSO FILES  [DEFINED] NET  S" lib/net/host.fs"  ?INCLUDED
ONLY FORTH DEFINITIONS  ALSO INTERNALS  ALSO TEXT  ALSO INET  ALSO NET  ALSO FILES
INCLUDE lib/net/test/sockets.fs
INCLUDE lib/net/test/syscall.fs
S" net" .RESULTS
