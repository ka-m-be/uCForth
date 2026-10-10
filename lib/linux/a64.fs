\ a64.fs - package LINUX-A64: the calls of the Linux kernel on aarch64
\
\ uCForth for Linux on aarch64 (targets/linux-a64) can make each call of
\ the kernel with the HAL word SYSCALL (100). This package gives it its
\ header, words for the calls that the network needs, and the numbers of
\ Linux for them (the calls: the table of the kernel for aarch64; the
\ other numbers: the same on each processor of Linux here). macOS has
\ the same words with its own numbers (lib/macos/system.fs). At the end,
\ SYSTEM gives this package: a common package writes ALSO SYSTEM
\ (lib/net/posix.fs).
\ aarch64 has no call poll: SYS-POLL uses ppoll.
\
\   x1 .. xn n id SYSCALL ( -- x )   the call number id AND $FFFF with
\                    the arguments x1 .. xn (n at most 6); bit 16 + k of
\                    id: x(k+1) is a VM address. x: the result (0 or
\                    more), or - the error number of the system
\   number SYSCALL: name args        a word name ( x1 .. xn -- x ) for the
\                    call number; args has a letter for each argument,
\                    from the first: n a number, a a VM address; - none
\
\ A VM address is not the real address. So SYSCALL adds the place of the
\ memory of the VM to each address argument. An address argument 0 stays
\ 0: the null pointer of C, "no buffer" (the VM does not use VM address 0).
\
\ Example: the number of this process, and a text to the terminal (fd 1):
\   ALSO LINUX-A64  SYS-GETPID .
\   1 S" hello" SYS-WRITE .

PACKAGE LINUX-A64   \ lib/package.fs

PUBLIC
PARSE-NAME SYSCALL HEADER  100 ,  REVEAL

PRIVATE
VARIABLE #ARGS   VARIABLE ADDRESSES
: SIGNATURE ( a u -- n mask )   \ the letters of args: their number, and a bit for each a
  0 #ARGS !  0 ADDRESSES !
  0 ?DO
    DUP I + C@  DUP 'a' = IF  1 #ARGS @ LSHIFT ADDRESSES +!  THEN
    DUP 'a' =  SWAP 'n' =  OR IF  1 #ARGS +!  THEN
  LOOP  DROP  #ARGS @ ADDRESSES @ ;

PUBLIC
: SYSCALL: ( number "name" "args" -- )   \ name ( x1 .. xn -- x )
  CREATE  PARSE-NAME SIGNATURE  16 LSHIFT ROT OR  SWAP , ,   \ n, then id
  DOES> DUP @  SWAP CELL+ @  SYSCALL ;

\ ---- the calls; each gives - the error number on an error
 63 SYSCALL: SYS-READ        nan      \ ( fd a u -- n )
 64 SYSCALL: SYS-WRITE       nan      \ ( fd a u -- n )
 57 SYSCALL: SYS-CLOSE       n        \ ( fd -- r )
172 SYSCALL: SYS-GETPID      -        \ ( -- pid )
207 SYSCALL: SYS-RECVFROM    nannaa   \ ( fd a u flags sa alen -- n )
202 SYSCALL: SYS-ACCEPT      naa      \ ( fd sa alen -- fd2 )
205 SYSCALL: SYS-GETPEERNAME naa      \ ( fd sa alen -- r )
204 SYSCALL: SYS-GETSOCKNAME naa      \ ( fd sa alen -- r )
 29 SYSCALL: SYS-IOCTL       nna      \ ( fd request a -- r )
 25 SYSCALL: SYS-FCNTL       nnn      \ ( fd cmd x -- r )
198 SYSCALL: SYS-SOCKET      nnn      \ ( domain type protocol -- fd )
203 SYSCALL: SYS-CONNECT     nan      \ ( fd sa len -- r )
200 SYSCALL: SYS-BIND        nan      \ ( fd sa len -- r )
208 SYSCALL: SYS-SETSOCKOPT  nnnan    \ ( fd level name a len -- r )
201 SYSCALL: SYS-LISTEN      nn       \ ( fd backlog -- r )
209 SYSCALL: SYS-GETSOCKOPT  nnnaa    \ ( fd level name a alen -- r )
206 SYSCALL: SYS-SENDTO      nannan   \ ( fd a u flags sa len -- n ); sa 0: to the peer
 73 SYSCALL: SYS-PPOLL       anann    \ ( fds n ts sigmask size -- n ): aarch64 has no poll

\ ---- the numbers
2 CONSTANT AF-INET       1 CONSTANT SOCK-STREAM    2 CONSTANT SOCK-DGRAM
1 CONSTANT SOL-SOCKET    2 CONSTANT SO-REUSEADDR   6 CONSTANT SO-BROADCAST
4 CONSTANT SO-ERROR      0 CONSTANT IPPROTO-IP     35 CONSTANT IP-ADD-MEMBERSHIP
2 CONSTANT MSG-PEEK      $4000 CONSTANT MSG-NOSIGNAL   \ no signal SIGPIPE: an error instead
3 CONSTANT F-GETFL       4 CONSTANT F-SETFL        $800 CONSTANT O-NONBLOCK
$541B CONSTANT FIONREAD
1 CONSTANT POLLIN        4 CONSTANT POLLOUT        8 CONSTANT POLLERR     $10 CONSTANT POLLHUP
9 CONSTANT EBADF         32 CONSTANT EPIPE         11 CONSTANT EAGAIN     115 CONSTANT EINPROGRESS
98 CONSTANT EADDRINUSE   101 CONSTANT ENETUNREACH  103 CONSTANT ECONNABORTED
104 CONSTANT ECONNRESET  107 CONSTANT ENOTCONN     110 CONSTANT ETIMEDOUT
111 CONSTANT ECONNREFUSED  113 CONSTANT EHOSTUNREACH

: SA-FAMILY! ( a -- )   \ the start of a struct sockaddr_in: AF_INET in 16 bits
  AF-INET OVER C!  0 SWAP 1+ C! ;

PRIVATE
16 BUFFER: TS            \ struct timespec: seconds, nanoseconds (64 bits each)
PUBLIC
: SYS-POLL ( fds n ms -- n )   \ poll, with ppoll
  TS 16 0 FILL  1000 /MOD TS !  1000000 * TS 8 + !  TS 0 8 SYS-PPOLL ;

END-PACKAGE
: SYSTEM ( -- )   LINUX-A64 ;   \ ALSO SYSTEM: the package of the system that runs this program
