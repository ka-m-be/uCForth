\ system.fs - package MACOS: the calls of the macOS kernel
\
\ uCForth for macOS (targets/macos-a64, targets/macos-x64) can make each
\ call of the kernel with the HAL word SYSCALL (100). This package gives
\ it its header, words for the calls that the network needs, and the
\ numbers of macOS for them (from the headers of the macOS SDK: sys/
\ syscall.h, sys/socket.h, netinet/in.h, sys/fcntl.h, sys/filio.h, sys/
\ poll.h, sys/errno.h). Linux has the same words with its own numbers
\ (lib/linux/a64.fs, x64.fs, i386.fs). At the end, SYSTEM gives this
\ package: a common package writes ALSO SYSTEM (lib/net/posix.fs).
\
\   x1 .. xn n id SYSCALL ( -- x )   the call number id AND $FFFF with
\                    the arguments x1 .. xn (n at most 6); bit 16 + k of
\                    id: x(k+1) is a VM address. x: the result (0 or
\                    more), or - the error number of the system
\   number SYSCALL: name args        a word name ( x1 .. xn -- x ) for the
\                    call number; args has a letter for each argument,
\                    from the first: n a number, a a VM address; - none
\
\ A VM address is not the real address: macOS loads the program above
\ 4 GB, and a cell has 32 bits. So SYSCALL adds the place of the memory
\ of the VM to each address argument. An address argument 0 stays 0: the
\ null pointer of C, "no buffer" (the VM does not use VM address 0).
\
\ Example: the number of this process, and a text to the terminal (fd 1):
\   ALSO MACOS  SYS-GETPID .
\   1 S" hello" SYS-WRITE .

PACKAGE MACOS   \ lib/package.fs

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
 29 SYSCALL: SYS-RECVFROM    nannaa   \ ( fd a u flags sa alen -- n )
 30 SYSCALL: SYS-ACCEPT      naa      \ ( fd sa alen -- fd2 )
 31 SYSCALL: SYS-GETPEERNAME naa      \ ( fd sa alen -- r )
 32 SYSCALL: SYS-GETSOCKNAME naa      \ ( fd sa alen -- r )
 54 SYSCALL: SYS-IOCTL       nna      \ ( fd request a -- r )
 92 SYSCALL: SYS-FCNTL       nnn      \ ( fd cmd x -- r )
 97 SYSCALL: SYS-SOCKET      nnn      \ ( domain type protocol -- fd )
 98 SYSCALL: SYS-CONNECT     nan      \ ( fd sa len -- r )
104 SYSCALL: SYS-BIND        nan      \ ( fd sa len -- r )
105 SYSCALL: SYS-SETSOCKOPT  nnnan    \ ( fd level name a len -- r )
106 SYSCALL: SYS-LISTEN      nn       \ ( fd backlog -- r )
118 SYSCALL: SYS-GETSOCKOPT  nnnaa    \ ( fd level name a alen -- r )
133 SYSCALL: SYS-SENDTO      nannan   \ ( fd a u flags sa len -- n ); sa 0: to the peer
230 SYSCALL: SYS-POLL        ann      \ ( fds n ms -- n )

\ ---- the numbers
2 CONSTANT AF-INET       1 CONSTANT SOCK-STREAM    2 CONSTANT SOCK-DGRAM
$FFFF CONSTANT SOL-SOCKET   4 CONSTANT SO-REUSEADDR   $20 CONSTANT SO-BROADCAST
$1007 CONSTANT SO-ERROR     0 CONSTANT IPPROTO-IP     12 CONSTANT IP-ADD-MEMBERSHIP
2 CONSTANT MSG-PEEK      $80000 CONSTANT MSG-NOSIGNAL   \ no signal SIGPIPE: an error instead
3 CONSTANT F-GETFL       4 CONSTANT F-SETFL        4 CONSTANT O-NONBLOCK
$4004667F CONSTANT FIONREAD
1 CONSTANT POLLIN        4 CONSTANT POLLOUT        8 CONSTANT POLLERR     $10 CONSTANT POLLHUP
9 CONSTANT EBADF         32 CONSTANT EPIPE         35 CONSTANT EAGAIN     36 CONSTANT EINPROGRESS
48 CONSTANT EADDRINUSE   51 CONSTANT ENETUNREACH   53 CONSTANT ECONNABORTED
54 CONSTANT ECONNRESET   57 CONSTANT ENOTCONN      60 CONSTANT ETIMEDOUT
61 CONSTANT ECONNREFUSED 65 CONSTANT EHOSTUNREACH

: SA-FAMILY! ( a -- )   \ the start of a struct sockaddr_in: its length 16, then AF_INET
  16 OVER C!  AF-INET SWAP 1+ C! ;

END-PACKAGE
: SYSTEM ( -- )   MACOS ;   \ ALSO SYSTEM: the package of the system that runs this program
