\ hal.fs - the HAL of uCForth for Linux on aarch64
\
\ The start code, the console (primitives 29-31) and the HAL (80-100)
\ as calls of the Linux kernel. No C library: a call puts its
\ number into X8 and its arguments into X0-X5, then "svc #0"; the result
\ comes back in X0, a negative number is an error. SYSCALL (100) gives
\ each call of the system to Forth.
\
\ Loaded by build.fs after kernels/aarch64.fs. build.fs gives the memory
\ layout: MEM-VADDR (the address of VM address 0) and MEM-SIZE.
\
\ The console: ?RX waits at most 1 ms for a byte of the standard input,
\ so KEY? does not stop the program. At the end of the input (a pipe or
\ a file) see IN-END below. TX! writes each byte at once.
\ The terminal stays in its normal mode: it shows the line while you
\ type and sends it with the return key.
\
\ Ctrl-C stops the running word (doc/vm.md, section 9: break): the
\ signal SIGINT does not end the program, see BREAK below. Ctrl-\
\ (SIGQUIT) still ends it at once.

63 CONSTANT SYS-READ      64 CONSTANT SYS-WRITE     56 CONSTANT SYS-OPENAT
57 CONSTANT SYS-CLOSE     94 CONSTANT SYS-EXIT      73 CONSTANT SYS-PPOLL
101 CONSTANT SYS-NANOSLEEP   113 CONSTANT SYS-CLOCK
134 CONSTANT SYS-SIGACTION   139 CONSTANT SYS-SIGRETURN
29 CONSTANT SYS-IOCTL     25 CONSTANT SYS-FCNTL
: SYSCALL, ( n -- )   X8 SWAP MOVI,  SVC, ;

\ The scratch area SCR lies after the memory of the VM:
 0 CONSTANT S-CHAR    \ one byte for the console
16 CONSTANT S-TIME    \ struct timespec: seconds and nanoseconds, 64 bits each
32 CONSTANT S-POLL    \ struct pollfd: fd (32 bits), events, revents (16 bits each)
48 CONSTANT S-START   \ the time of the start in milliseconds
64 CONSTANT S-PATH    \ a file name with a byte 0 at the end, at most 255 bytes
320 CONSTANT S-ACT    \ struct sigaction: handler, flags, restorer, mask (64 bits each)
400 CONSTANT S-TIO    \ struct termios (36 bytes)
480 CONSTANT S-MBITS  \ the bits for TIOCMBIS and TIOCMBIC (32 bits each)
496 CONSTANT S-ARGC   \ argc and the address of argv, from the stack of the start (64 bits each)
504 CONSTANT S-ARGV
512 CONSTANT S-REGS   \ SYSCALL: X0-X5 for the call (64 bits each)
$1000 CONSTANT S-ARGS \ ARG copies argument n here, 256 bytes each, n < 32;
                      \ VM address MEM-SIZE + S-ARGS + 256 n (the VM reaches SCR)

\ ==== Start and time ====================================================

LABEL NOW-MS   \ W2 = the time since some point, in milliseconds (32 bits)
  X0 1 MOVI,  X1 SCR S-TIME ADDI,  SYS-CLOCK SYSCALL,        \ CLOCK_MONOTONIC
  X2 SCR S-TIME LDRX-I,  X3 SCR S-TIME 8 + LDRX-I,
  X4 1000 MOVI,  X2 X2 X4 MUL,  X4 1000000 MOVI,  X3 X3 X4 UDIV,  X2 X2 X3 ADD,
  RET,

\ ==== Break: Ctrl-C ======================================================
\ Linux calls ON-SIGINT between two instructions of the program. The
\ handler changes only X27 (TAB) in the saved registers: it points to
\ BRKTAB, where each primitive is BREAK. So the next dispatch runs
\ BREAK, at a place where the VM is complete. NEXT costs nothing more.
\ A call of the system that waits (ppoll, nanosleep) stops with EINTR.
\ BREAK puts OPTAB back and writes a new line. With a CATCH (HANDLER
\ not 0) it runs -28 THROW ("user interrupt"). Without one (QUIT waits
\ in REFILL, or BOOT waits for a key) it starts QUIT again.

LABEL BRKTAB   128 RESERVE
LABEL OUT-ADDR  0 A,       \ build.fs: the address of OUT

LABEL ON-SIGINT   \ ( x0 signal, x1 info, x2 ucontext ): regs[27] is at 400
  X9 BRKTAB ADR,  X9 X2 400 STRX-I,  RET,
LABEL SIGRETURN   \ Linux returns here from the handler
  SYS-SIGRETURN SYSCALL,

LABEL BREAK
  TAB OPTAB ADR,
  W0 10 MOVI,  W0 SCR S-CHAR STRB-I,
  X0 1 MOVI,  X1 SCR S-CHAR ADDI,  X2 1 MOVI,  SYS-WRITE SYSCALL,
  W10 OUT-ADDR LDR-LIT,  WZR MEM W10 STR-UXTW,
  W10 HANDLER MOVI,  W0 MEM W10 LDR-UXTW,
  W0 0=IF,
    DSP S0 MOVI,  RSP R0 MOVI,  W10 STATE MOVI,  WZR MEM W10 STR-UXTW,
    WP XT-QUIT LDR-LIT,  DISPATCH B,
  THEN,
  W0 -28 MOVI,  THROW-CODE B,

: FILL-BRKTAB ( -- )   128 0 DO  BREAK @ BRKTAB @ -  BRKTAB @ I CELLS +  A!  LOOP ;
FILL-BRKTAB

LABEL START   \ the entry of the program: SP points to argc, then argv
  MEM MEM-VADDR MOVI,  X9 MEM-SIZE MOVI,  SCR MEM X9 ADD,
  X10 XSP 0 LDRX-I,  X10 SCR S-ARGC STRX-I,  X10 XSP 8 ADDI,  X10 SCR S-ARGV STRX-I,
  TAB OPTAB ADR,
  X9 SCR S-ACT ADDI,                                    \ SIGINT: ON-SIGINT
  X10 ON-SIGINT ADR,  X10 X9 0 STRX-I,
  X10 $04000004 MOVI,  X10 X9 8 STRX-I,                \ SA_RESTORER SA_SIGINFO
  X10 SIGRETURN ADR,  X10 X9 16 STRX-I,  XZR X9 24 STRX-I,
  X0 2 MOVI,  X1 X9 MOV,  X2 XZR MOV,  X3 8 MOVI,  SYS-SIGACTION SYSCALL,
  DSP S0 MOVI,  RSP R0 MOVI,                           \ doc/vm.md, section 10
  W10 STATE MOVI,  WZR MEM W10 STR-UXTW,  W10 HANDLER MOVI,  WZR MEM W10 STR-UXTW,
  NOW-MS BL,  W2 SCR S-START STR-I,
  WP XT-BOOT LDR-LIT,  DISPATCH B,

LABEL SLEEP   \ wait W2 seconds and W3 nanoseconds
  W2 SCR S-TIME STR-I,  WZR SCR S-TIME 4 + STR-I,
  W3 SCR S-TIME 8 + STR-I,  WZR SCR S-TIME 12 + STR-I,
  X0 SCR S-TIME ADDI,  X1 XZR MOV,  SYS-NANOSLEEP SYSCALL,  NEXT,

\ ==== The console =======================================================

\ At the end of the input (a pipe, a file, Ctrl-D), ?RX ends the program
\ only when QUIT waits for a line: a return address on the return stack
\ points into REFILL, as in a bootstrap VM (doc/vm.md, section 9). Else
\ it answers "no key" and waits 1 ms. build.fs puts the xt of REFILL and
\ the end of its body into these cells.
LABEL XT-REFILL    0 A,
LABEL REFILL-END   0 A,

LABEL IN-END
  W10 XT-REFILL LDR-LIT,  W11 REFILL-END LDR-LIT,  W14 R0 MOVI,
  W12 RSP MOV,  W13 8 MOVI,
  BEGIN,
    W12 W14 CMP,  LO IF,
      W0 MEM W12 LDR-UXTW,
      W0 W10 CMP,  HI IF,  W0 W11 CMP,  LO IF,  X0 XZR MOV,  SYS-EXIT SYSCALL,  THEN,  THEN,
    THEN,
    W12 W12 4 ADDI,  W13 W13 1 SUBI,  W13 0 CMPI,
  EQ UNTIL,
  WZR PUSH,  W2 WZR MOV,  W3 1000000 MOVI,  SLEEP B,

29 PRIM ( ?RX )   \ ppoll: wait at most 1 ms, then read 1 byte
  WZR SCR S-POLL STR-I,  W10 1 MOVI,  W10 SCR S-POLL 4 + STR-I,   \ fd 0, POLLIN
  WZR SCR S-TIME STR-I,  WZR SCR S-TIME 4 + STR-I,
  W10 1000000 MOVI,  W10 SCR S-TIME 8 + STR-I,  WZR SCR S-TIME 12 + STR-I,
  X0 SCR S-POLL ADDI,  X1 1 MOVI,  X2 SCR S-TIME ADDI,  X3 XZR MOV,  X4 XZR MOV,
  SYS-PPOLL SYSCALL,
  W0 0 CMPI,  GT IF,
    X0 XZR MOV,  X1 SCR S-CHAR ADDI,  X2 1 MOVI,  SYS-READ SYSCALL,
    W0 0 CMPI,  IN-END LE B.COND,                            \ the end of the input
    W0 SCR S-CHAR LDRB-I,  W0 PUSH,  W0 -1 MOVI,  W0 PUSH,  NEXT,
  THEN,
  WZR PUSH,  NEXT,

30 PRIM ( TX! )
  W0 POP,  W0 SCR S-CHAR STRB-I,
  X0 1 MOVI,  X1 SCR S-CHAR ADDI,  X2 1 MOVI,  SYS-WRITE SYSCALL,  NEXT,

31 PRIM ( !IO )   NEXT,

\ ==== HAL 80-88 =========================================================

80 PRIM ( MILLIS )   NOW-MS BL,  W3 SCR S-START LDR-I,  W2 W2 W3 SUB,  W2 PUSH,  NEXT,
81 PRIM ( MS )   \ u / 1000 seconds, (u mod 1000) * 1000000 nanoseconds
  W0 POP,  W1 1000 MOVI,  W2 W0 W1 UDIV,  W3 W2 W1 W0 MSUB,
  W4 1000000 MOVI,  W3 W3 W4 MUL,  SLEEP B,
82 PRIM ( US )
  W0 POP,  W1 1000000 MOVI,  W2 W0 W1 UDIV,  W3 W2 W1 W0 MSUB,
  W4 1000 MOVI,  W3 W3 W4 MUL,  SLEEP B,
83 PRIM ( BIS! )   W1 POP,  W0 POP,  W2 MEM W1 LDRB-UXTW,  W2 W2 W0 ORR,  W2 MEM W1 STRB-UXTW,  NEXT,
84 PRIM ( BIC! )   W1 POP,  W0 POP,  W2 MEM W1 LDRB-UXTW,  W2 W2 W0 BIC,  W2 MEM W1 STRB-UXTW,  NEXT,
85 PRIM ( SAVE-IMAGE )   W0 -21 MOVI,  W0 PUSH,  NEXT,   \ not yet: build.fs makes programs
86 PRIM ( ERASE-IMAGE )  NEXT,
87 PRIM ( REBOOT )   \ ( f -- ): empty stacks, then BOOT (f true) or QUIT
  W0 POP,  DSP S0 MOVI,  RSP R0 MOVI,
  W10 STATE MOVI,  WZR MEM W10 STR-UXTW,  W10 HANDLER MOVI,  WZR MEM W10 STR-UXTW,
  W0 0=IF,  WP XT-QUIT LDR-LIT,  ELSE,  WP XT-BOOT LDR-LIT,  THEN,  DISPATCH B,
88 PRIM ( BYE )     X0 XZR MOV,  SYS-EXIT SYSCALL,
99 PRIM ( BYE-N )  W0 POP,  SYS-EXIT SYSCALL,

\ ==== SYSCALL: HAL 100 ===================================================
\ ( x1 .. xn n id -- x ): the call of Linux with the number id AND $FFFF;
\ x1 .. xn go to X0 .. X5 (n at most 6). Bit 16 + k of id: x(k+1) is a
\ VM address, the call gets MEM + x; 0 stays 0 (the null pointer of C)
\ (lib/linux/a64.fs). Each other argument goes to 64 bits with its sign.
\ x: the low 32 bits of X0; an error is - the error number.

100 PRIM ( SYSCALL )
  W12 POP,  W13 POP,                                    \ W12 id, W13 n
  W13 6 CMPI,  HI IF,  W0 -24 MOVI,  THROW-CODE B,  THEN,
  W14 W13 2 LSLI,  W14 DSP W14 ADD,                     \ W14: the stack without x1 .. xn
  W10 S0 MOVI,  W14 W10 CMP,  HI IF,  W0 -4 MOVI,  THROW-CODE B,  THEN,
  W11 W12 16 LSRI,  W15 W14 MOV,  X10 SCR S-REGS ADDI,  \ W11 the mask; X10 the place of X0
  W13 0<>IF,
    W8 W13 MOV,
    BEGIN,
      W15 W15 4 SUBI,  W9 MEM W15 LDR-UXTW,            \ x1, then x2 ...
      W7 1 MOVI,  W7 W11 W7 AND,
      W7 0=IF,  X9 W9 SXTW,  ELSE,  W9 0<>IF,  X9 MEM X9 ADD,  THEN,  THEN,
      X9 X10 0 STRX-I,  X10 X10 8 ADDI,
      W11 W11 1 LSRI,  W8 W8 1 SUBI,  W8 0 CMPI,
    EQ UNTIL,
  THEN,
  X0 SCR S-REGS LDRX-I,       X1 SCR S-REGS 8 + LDRX-I,    X2 SCR S-REGS 16 + LDRX-I,
  X3 SCR S-REGS 24 + LDRX-I,  X4 SCR S-REGS 32 + LDRX-I,   X5 SCR S-REGS 40 + LDRX-I,
  W8 W12 16 LSLI,  W8 W8 16 LSRI,  SVC,
  DSP W14 4 SUBI,  W0 TOS!,  NEXT,

FORWARD ARG-END   FORWARD ARG-NONE
97 PRIM ( ARG )   \ ( n -- a u ): the argument n of the command line; u = 0: none
  W0 TOS@,  X1 SCR S-ARGC LDRX-I,  X0 X1 CMP,  ARG-NONE HS B.COND,
  W0 31 CMPI,  ARG-NONE HI B.COND,
  X2 SCR S-ARGV LDRX-I,  X3 X0 3 LSLI,  X2 X2 X3 ADD,  X2 X2 0 LDRX-I,   \ argv[n]
  W4 W0 8 LSLI,  W5 MEM-SIZE S-ARGS + MOVI,  W4 W4 W5 ADD,  X6 MEM X4 ADD,
  W5 WZR MOV,
LABEL ARG-LOOP   \ copy the text up to its byte 0, at most 255 bytes
  W7 X2 W5 LDRB-UXTW,  W7 ARG-END CBZ,
  W7 X6 W5 STRB-UXTW,  W5 W5 1 ADDI,  W5 255 CMPI,  ARG-LOOP LO B.COND,
LABEL ARG-END
  W4 TOS!,  W5 PUSH,  NEXT,
LABEL ARG-NONE
  WZR TOS!,  WZR PUSH,  NEXT,

\ ==== Files: HAL 89-93, 98 (doc/vm.md, section 8) ======================
\ A fid is a file descriptor of Linux. W12 a, W13 u, W14 fid, W15 count.

LABEL OPEN-PATH   \ (BL) W1 a, W2 u: the name; W3 the flags of openat; X0: fd or - error
  W2 255 CMPI,  HI IF,  W2 255 MOVI,  THEN,
  X9 SCR S-PATH ADDI,  W5 WZR MOV,                    \ the name, then a byte 0
  W2 0<>IF,
    BEGIN,  W10 W1 W5 ADD,  W6 MEM W10 LDRB-UXTW,  W6 X9 W5 STRB-UXTW,
    W5 W5 1 ADDI,  W5 W2 CMP,  EQ UNTIL,
  THEN,
  WZR X9 W2 STRB-UXTW,
  X0 -100 MOVI,  X1 X9 MOV,  W2 W3 MOV,  X3 $1A4 MOVI,  SYS-OPENAT SYSCALL,   \ AT_FDCWD, mode 644
  RET,

LABEL OPEN-ERROR   \ X0 = - error number: ( x -- 0 ior )
  W1 W0 2 ADDI,  W2 -37 MOVI,  W1 0=IF,  W2 -38 MOVI,  THEN,   \ -2 ENOENT: -38
  WZR TOS!,  W2 PUSH,  NEXT,

LABEL OPEN-COMMON   \ ( a u fam -- fid ior ); W4 holds more flags of openat
  W3 POP,  W2 POP,  W1 TOS@,
  W10 3 MOVI,  W3 W3 W10 AND,  W3 W3 W4 ORR,           \ fam 0 1 2 = O_RDONLY O_WRONLY O_RDWR
  OPEN-PATH BL,
  X0 0 CMPI,  OPEN-ERROR LT B.COND,
  W0 TOS!,  WZR PUSH,  NEXT,

89 PRIM ( OPEN-FILE )     W4 WZR MOV,  OPEN-COMMON B,
90 PRIM ( CREATE-FILE )   W4 $240 MOVI,  OPEN-COMMON B,        \ O_CREAT O_TRUNC

91 PRIM ( CLOSE-FILE )
  W0 TOS@,  SYS-CLOSE SYSCALL,  W1 WZR MOV,  X0 0 CMPI,  LT IF,  W1 -37 MOVI,  THEN,
  W1 TOS!,  NEXT,

FORWARD RF-DONE
92 PRIM ( READ-FILE )   \ ( a u fid -- u2 ior )
  W14 POP,  W13 POP,  W12 TOS@,  W15 WZR MOV,  W7 WZR MOV,
LABEL RF-LOOP
  W15 W13 CMP,  RF-DONE HS B.COND,
  W0 W14 MOV,  W10 W12 W15 ADD,  X1 MEM X10 ADD,  W2 W13 W15 SUB,  SYS-READ SYSCALL,
  X0 0 CMPI,  LT IF,  W7 -37 MOVI,  RF-DONE B,  THEN,
  W0 RF-DONE CBZ,
  W15 W15 W0 ADD,  RF-LOOP B,
LABEL RF-DONE
  W15 TOS!,  W7 PUSH,  NEXT,

FORWARD WF-DONE
93 PRIM ( WRITE-FILE )   \ ( a u fid -- ior )
  W14 POP,  W13 POP,  W12 TOS@,  W15 WZR MOV,  W7 WZR MOV,
LABEL WF-LOOP
  W15 W13 CMP,  WF-DONE HS B.COND,
  W0 W14 MOV,  W10 W12 W15 ADD,  X1 MEM X10 ADD,  W2 W13 W15 SUB,  SYS-WRITE SYSCALL,
  X0 0 CMPI,  LE IF,  W7 -37 MOVI,  WF-DONE B,  THEN,
  W15 W15 W0 ADD,  WF-LOOP B,
LABEL WF-DONE
  W7 TOS!,  NEXT,

FORWARD RL-LF   FORWARD RL-LINE
98 PRIM ( READ-LINE )   \ ( a u fid -- u2 f ior ): byte for byte up to LF
  W14 POP,  W13 POP,  W12 TOS@,  W15 WZR MOV,  W6 WZR MOV,   \ W6: a byte came
LABEL RL-LOOP
  W15 W13 CMP,  RL-LINE HS B.COND,                            \ the buffer is full
  W0 W14 MOV,  X1 SCR S-CHAR ADDI,  X2 1 MOVI,  SYS-READ SYSCALL,
  X0 0 CMPI,  LT IF,  W15 TOS!,  WZR PUSH,  W0 -37 MOVI,  W0 PUSH,  NEXT,  THEN,
  W0 0=IF,                                                   \ the end of the file
    W6 RL-LINE CBNZ,  WZR TOS!,  WZR PUSH,  WZR PUSH,  NEXT,
  THEN,
  W6 1 MOVI,
  W0 SCR S-CHAR LDRB-I,  W0 10 CMPI,  RL-LF EQ B.COND,
  W10 W12 W15 ADD,  W0 MEM W10 STRB-UXTW,  W15 W15 1 ADDI,  RL-LOOP B,
LABEL RL-LF   \ a CR before the LF goes away
  W15 RL-LINE CBZ,
  W10 W12 W15 ADD,  W10 W10 1 SUBI,  W0 MEM W10 LDRB-UXTW,  W0 13 CMPI,
  EQ IF,  W15 W15 1 SUBI,  THEN,
LABEL RL-LINE
  W15 TOS!,  W0 -1 MOVI,  W0 PUSH,  WZR PUSH,  NEXT,

\ ==== Serial lines: HAL 94-96 (doc/vm.md, section 8) =====================
\ The tool ucf talks to a board over a serial line: raw bytes, 8 data
\ bits, no parity, no flow control; a read gives the bytes that are
\ there, or none (VMIN = VTIME = 0). The speed is one of the table
\ BAUDS: Linux gives c_cflag a code for each of them (termbits.h).

LABEL BAUDS   \ pairs: the speed, its code; 0 at the end
      50 A,     1 A,       75 A,     2 A,      110 A,     3 A,      134 A,     4 A,
     150 A,     5 A,      200 A,     6 A,      300 A,     7 A,      600 A,     8 A,
    1200 A,     9 A,     1800 A,    10 A,     2400 A,    11 A,     4800 A,    12 A,
    9600 A,    13 A,    19200 A,    14 A,    38400 A,    15 A,    57600 A, $1001 A,
  115200 A, $1002 A,   230400 A, $1003 A,   460800 A, $1004 A,   500000 A, $1005 A,
  576000 A, $1006 A,   921600 A, $1007 A,  1000000 A, $1008 A,  1152000 A, $1009 A,
 1500000 A, $100A A,  2000000 A, $100B A,  2500000 A, $100C A,  3000000 A, $100D A,
 3500000 A, $100E A,  4000000 A, $100F A,        0 A,     0 A,

FORWARD SS-NONE   FORWARD SS-FOUND
LABEL SERIAL-SET   \ (BL) W14 fd, W13 baud: raw 8N1 at this speed; X0 < 0: an error
  X9 BAUDS ADR,
LABEL SS-LOOP
  W10 X9 0 LDR-I,  W10 SS-NONE CBZ,                     \ not in the table
  W10 W13 CMP,  SS-FOUND EQ B.COND,
  X9 X9 8 ADDI,  SS-LOOP B,
LABEL SS-NONE
  X0 -22 MOVI,  RET,
LABEL SS-FOUND
  W15 X9 4 LDR-I,                                       \ the code of the speed
  W0 W14 MOV,  X1 $5401 MOVI,  X2 SCR S-TIO ADDI,  SYS-IOCTL SYSCALL,   \ TCGETS
  X0 0 CMPI,  LT IF,  RET,  THEN,
  WZR SCR S-TIO STR-I,  WZR SCR S-TIO 4 + STR-I,        \ iflag, oflag: raw
  W10 $8B0 MOVI,  W10 W10 W15 ORR,  W10 SCR S-TIO 8 + STR-I,   \ cflag: the speed, CS8 CREAD CLOCAL
  WZR SCR S-TIO 12 + STR-I,                             \ lflag: no echo, no lines
  WZR SCR S-TIO 22 + STRB-I,  WZR SCR S-TIO 23 + STRB-I,   \ VTIME 0, VMIN 0
  W0 W14 MOV,  X1 $5402 MOVI,  X2 SCR S-TIO ADDI,  SYS-IOCTL SYSCALL,  RET,   \ TCSETS

94 PRIM ( SERIAL-OPEN )   \ ( a u baud -- fid ior )
  W13 POP,  W2 POP,  W1 TOS@,
  W3 $902 MOVI,  OPEN-PATH BL,                      \ O_RDWR O_NOCTTY O_NONBLOCK
  X0 0 CMPI,  OPEN-ERROR LT B.COND,
  W14 W0 MOV,
  X1 4 MOVI,  X2 XZR MOV,  SYS-FCNTL SYSCALL,       \ F_SETFL 0: reads follow VMIN, VTIME
  SERIAL-SET BL,
  X0 0 CMPI,  LT IF,                                \ not a serial line, or a speed it does not know
    W0 W14 MOV,  SYS-CLOSE SYSCALL,  WZR TOS!,  W0 -37 MOVI,  W0 PUSH,  NEXT,
  THEN,
  W14 TOS!,  WZR PUSH,  NEXT,

95 PRIM ( SERIAL-LINES )   \ ( dtr rts fid -- ior ): true = the line is active
  W14 POP,  W1 POP,  W0 TOS@,
  W3 2 MOVI,  W0 0 CMPI,  W4 W3 WZR NE CSEL,       \ TIOCM_DTR
  W3 4 MOVI,  W1 0 CMPI,  W5 W3 WZR NE CSEL,       \ TIOCM_RTS
  W4 W4 W5 ORR,  W5 6 MOVI,  W5 W5 W4 BIC,         \ W4 on, W5 off
  W4 SCR S-MBITS STR-I,  W5 SCR S-MBITS 4 + STR-I,
  W0 W14 MOV,  X1 $5416 MOVI,  X2 SCR S-MBITS ADDI,  SYS-IOCTL SYSCALL,   \ TIOCMBIS
  X0 0 CMPI,  LT IF,  W0 -37 MOVI,  W0 TOS!,  NEXT,  THEN,
  W0 W14 MOV,  X1 $5417 MOVI,  X2 SCR S-MBITS 4 + ADDI,  SYS-IOCTL SYSCALL, \ TIOCMBIC
  X0 0 CMPI,  LT IF,  W0 -37 MOVI,  W0 TOS!,  NEXT,  THEN,
  WZR TOS!,  NEXT,

96 PRIM ( SERIAL-BAUD )   \ ( baud fid -- ior )
  W14 POP,  W13 TOS@,  SERIAL-SET BL,
  W1 WZR MOV,  X0 0 CMPI,  LT IF,  W1 -37 MOVI,  THEN,  W1 TOS!,  NEXT,
