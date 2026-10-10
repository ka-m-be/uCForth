\ i386.fs - package LINUX-I386: the calls of the Linux kernel on i386
\
\ uCForth for Linux on i386 (targets/linux-i386) can make each call of
\ the kernel with the HAL word SYSCALL (100). This package gives it its
\ header, words for the calls that the network needs, and the numbers of
\ Linux for them (the calls: the table of the kernel for i386; the
\ other numbers: the same on each processor of Linux here). macOS has
\ the same words with its own numbers (lib/macos/system.fs). At the end,
\ SYSTEM gives this package: a common package writes ALSO SYSTEM
\ (lib/net/posix.fs).
\ i386 has no call accept: SYS-ACCEPT uses accept4.
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
\   ALSO LINUX-I386  SYS-GETPID .
\   1 S" hello" SYS-WRITE .

PACKAGE LINUX-I386   \ lib/package.fs

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
  3 SYSCALL: SYS-READ        nan      \ ( fd a u -- n )
  4 SYSCALL: SYS-WRITE       nan      \ ( fd a u -- n )
  6 SYSCALL: SYS-CLOSE       n        \ ( fd -- r )
 20 SYSCALL: SYS-GETPID      -        \ ( -- pid )
371 SYSCALL: SYS-RECVFROM    nannaa   \ ( fd a u flags sa alen -- n )
364 SYSCALL: SYS-ACCEPT4     naan     \ ( fd sa alen flags -- fd2 ): i386 has no accept
368 SYSCALL: SYS-GETPEERNAME naa      \ ( fd sa alen -- r )
367 SYSCALL: SYS-GETSOCKNAME naa      \ ( fd sa alen -- r )
 54 SYSCALL: SYS-IOCTL       nna      \ ( fd request a -- r )
 55 SYSCALL: SYS-FCNTL       nnn      \ ( fd cmd x -- r )
359 SYSCALL: SYS-SOCKET      nnn      \ ( domain type protocol -- fd )
362 SYSCALL: SYS-CONNECT     nan      \ ( fd sa len -- r )
361 SYSCALL: SYS-BIND        nan      \ ( fd sa len -- r )
366 SYSCALL: SYS-SETSOCKOPT  nnnan    \ ( fd level name a len -- r )
363 SYSCALL: SYS-LISTEN      nn       \ ( fd backlog -- r )
365 SYSCALL: SYS-GETSOCKOPT  nnnaa    \ ( fd level name a alen -- r )
369 SYSCALL: SYS-SENDTO      nannan   \ ( fd a u flags sa len -- n ); sa 0: to the peer
168 SYSCALL: SYS-POLL        ann      \ ( fds n ms -- n )

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

: SYS-ACCEPT ( fd sa alen -- fd2 )   0 SYS-ACCEPT4 ;

END-PACKAGE
: SYSTEM ( -- )   LINUX-I386 ;   \ ALSO SYSTEM: the package of the system that runs this program
