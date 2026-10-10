\ syscall.fs - tests of SYSCALL over the package of the system (lib/net/test/net.fs)
\
\ lib/net/test/net.fs loads this file on macOS and Linux, after
\ lib/net/test/sockets.fs (from there: BUF).

ALSO SYSTEM
T{ SYS-GETPID 0 > -> TRUE }T
T{ -1 SYS-CLOSE -> -9 }T                        \ EBADF: the same number on each system
T{ 1 BUF 0 SYS-WRITE -> 0 }T                    \ an address argument
T{ -1 BUF 1 SYS-WRITE -> -9 }T
PREVIOUS
