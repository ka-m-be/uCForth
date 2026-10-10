\ hal.fs - the HAL of uCForth for Linux on a PC with 64 bits (x86-64)
\
\ The start code, the console (primitives 29-31) and the HAL (80-100)
\ as calls of the Linux kernel. No C library: a call puts its number
\ into EAX and its arguments into RDI, RSI, RDX, R10, R8 and R9, then
\ "syscall"; the result comes back in RAX, a negative number is an
\ error. SYSCALL (100) gives each call of the system to Forth. Linux changes
\ RAX, RCX and R11, and keeps all other registers: the VM in RBX RBP
\ R12-R15 stays (kernels/x64.fs).
\
\ Loaded by build.fs after kernels/x64.fs. build.fs gives MEM-AT (the
\ address of VM address 0 in the program), SCR (the VM address of the
\ scratch area after the memory of the VM), KCELLS and CODE-START.
\
\ The console: ?RX waits at most 1 ms for a byte of the standard input
\ (poll), so KEY? does not stop the program. At the end of the input (a
\ pipe or a file) see IN-END below. TX! writes each byte at once. The
\ terminal stays in its normal mode: it shows the line while you type
\ and sends it with the return key.
\
\ Ctrl-C stops the running word (doc/vm.md, section 9: break): the
\ handler of the signal SIGINT sets BREAK-OFF to 128, so the next
\ dispatch runs BREAK (kernels/x64.fs). A call that waits (poll,
\ nanosleep) ends at once. Ctrl-\ (SIGQUIT) still ends the program. An
\ access to an address outside the program (for example $7FFFFFF0 @)
\ is THROW -9, not the end of the program: see ON-SEGV.

0 CONSTANT SYS-READ        1 CONSTANT SYS-WRITE       2 CONSTANT SYS-OPEN
3 CONSTANT SYS-CLOSE       7 CONSTANT SYS-POLL        13 CONSTANT SYS-SIGACTION
15 CONSTANT SYS-SIGRETURN  16 CONSTANT SYS-IOCTL      35 CONSTANT SYS-NANOSLEEP
60 CONSTANT SYS-EXIT       72 CONSTANT SYS-FCNTL      228 CONSTANT SYS-CLOCK
: SYS, ( n -- )   EAX SWAP MOVI,  SYSCALL, ;   \ the arguments are in RDI RSI RDX R10

\ The scratch area SCR, after the memory of the VM (KCELLS: SCR 0-15).
\ These are VM addresses: n [R15] is the cell, RSI n [R15] LEA, its address.
SCR 16 +    CONSTANT S-CHAR       \ one byte for TX! and READ-LINE
SCR 20 +    CONSTANT START-MS     \ the clock at the start
SCR 24 +    CONSTANT S-ARGP       \ RSP at the start (64 bits): argc, then the addresses of the arguments
SCR 32 +    CONSTANT S-POLL       \ struct pollfd: fd, events, revents (8 bytes)
SCR 40 +    CONSTANT S-TIME       \ struct timespec: seconds, nanoseconds (64 bits each)
SCR 56 +    CONSTANT F-FID        \ files and serial lines: the fd,
SCR 60 +    CONSTANT F-POS        \ the bytes so far,
SCR 64 +    CONSTANT F-ERR        \ an error code, or: a byte came (READ-LINE)
SCR 68 +    CONSTANT F-FLAGS      \ the flags of open
SCR 72 +    CONSTANT F-BAUD       \ the speed of a serial line,
SCR 76 +    CONSTANT F-CODE       \ and its code for c_cflag
SCR 80 +    CONSTANT S-MBITS      \ the bits for TIOCMBIS and TIOCMBIC
SCR 96 +    CONSTANT S-ACT        \ struct sigaction: handler, flags, restorer, mask (64 bits each)
SCR 128 +   CONSTANT S-DFL        \ the same, all 0: the default action
SCR 160 +   CONSTANT S-TIO        \ struct termios (36 bytes)
SCR 256 +   CONSTANT S-PATH       \ a file name with a byte 0 at the end (255 + 1)
SCR 512 +   CONSTANT S-REGS       \ SYSCALL: the 6 arguments (64 bits each)
SCR $1000 + CONSTANT S-ARGS       \ ARG copies argument n here, 256 bytes each, n < 32

\ ==== Break and faults ==================================================

LABEL XT-REFILL    0 T,           \ build.fs: the xt of REFILL and the end of its body
LABEL REFILL-END   0 T,
LABEL OUT-ADDR     0 T,           \ build.fs: the VM address of OUT
LABEL CODE-BYTES   0 T,             \ build.fs: the bytes of the code from CODE-START

\ ON-SIGINT: void handler (int, siginfo_t *, ucontext_t *). Linux calls
\ it between two instructions of the program; it returns through
\ SIGRETURN. Both handlers have SA_SIGINFO: then Linux makes the frame
\ that rt_sigreturn expects. A handler finds the memory of the VM
\ relative to itself, not in R15.
LABEL ON-SIGINT
  RAX MEM-AT [RIP] LEA,  BREAK-OFF [RAX] 128 MOVI,  RET,
LABEL SIGRETURN
  SYS-SIGRETURN SYS,

\ BREAK: each primitive after a break. It writes a new line and sets
\ OUT to 0. With a CATCH (HANDLER not 0) it runs -28 THROW ("user
\ interrupt"); without one (QUIT waits in REFILL, or BOOT waits for a
\ key) it starts QUIT again.
LABEL BREAK
  BREAK-OFF [R15] 0 MOVI,
  S-CHAR [R15] BYTE 10 MOVI,  EDI 1 MOVI,  RSI S-CHAR [R15] LEA,  EDX 1 MOVI,  SYS-WRITE SYS,
  EAX OUT-ADDR [RIP] MOV,  0 [R15] [RAX*1] 0 MOVI,
  EAX HANDLER [R15] MOV,  EAX EAX TEST,
  Z IF,
    R13D S0 MOVI,  EBP R0 MOVI,  STATE [R15] 0 MOVI,
    EBX XT-QUIT [RIP] MOV,  DISPATCH JMP,
  THEN,
  EAX -28 MOVI,  THROW-CODE JMP,

\ FAULT: a primitive read or wrote outside the program. THROW -9 (the
\ stacks are as before the primitive), or QUIT when a stack pointer
\ itself is outside the memory of the VM.
LABEL FAULT
  R13D VM-END CMPI,  AE IF,  L1 JMP,  THEN,
  EBP VM-END CMPI,  AE IF,  L1 JMP,  THEN,
  EAX -9 MOVI,  THROW-CODE JMP,
LABEL L1
  R13D S0 MOVI,  EBP R0 MOVI,  STATE [R15] 0 MOVI,  HANDLER [R15] 0 MOVI,
  EBX XT-QUIT [RIP] MOV,  DISPATCH JMP,

\ ON-SEGV: void handler (int, siginfo_t *, ucontext_t *), for SIGSEGV.
\ RIP is at offset 168 of the ucontext (uc_mcontext.gregs[16]). A fault
\ in the code of the program goes on at FAULT. Another one: the default
\ action again, and the same fault ends the program.
LABEL ON-SEGV
  RAX 168 [RDX] MOV,  RCX CODE-START [RIP] LEA,  RAX RCX SUB,
  ECX CODE-BYTES [RIP] MOV,  RAX RCX CMP,  L2 JAE,
  RAX FAULT [RIP] LEA,  168 [RDX] RAX MOV,  RET,     \ go on, at FAULT
LABEL L2
  RAX MEM-AT [RIP] LEA,
  EDI 11 MOVI,  RSI S-DFL [RAX] LEA,  EDX EDX XOR,  R10D 8 MOVI,  SYS-SIGACTION SYS,  RET,

\ ==== The console =======================================================

LABEL SLEEP   \ (JMP) wait EAX seconds and EDX nanoseconds, then NEXT; a break ends the wait
  S-TIME [R15] RAX MOV,  S-TIME 8 + [R15] RDX MOV,
  RDI S-TIME [R15] LEA,  ESI ESI XOR,  SYS-NANOSLEEP SYS,  NEXT,

\ At the end of the input (a pipe, a file, Ctrl-D), ?RX ends the program
\ only when QUIT waits for a line: a return address on the return stack
\ points into REFILL, as in a bootstrap VM (doc/vm.md, section 9). Else
\ it answers "no key" and waits 1 ms.
LABEL IN-END
  EAX EBP MOV,  ECX 8 MOVI,
  BEGIN,
    EAX R0 CMPI,  B IF,
      EDX 0 [R15] [RAX*1] MOV,
      EDX XT-REFILL [RIP] CMP,  A IF,  EDX REFILL-END [RIP] CMP,  B IF,  EDI EDI XOR,  SYS-EXIT SYS,  THEN,  THEN,
    THEN,
    EAX 4 ADDI,  ECX DEC,
  Z UNTIL,
  EAX EAX XOR,  EAX DPUSH,
  EDX 1000000 MOVI,  SLEEP JMP,

29 PRIM ( ?RX )   \ poll: wait at most 1 ms, then read 1 byte
  S-POLL [R15] 0 MOVI,  S-POLL 4 + [R15] 1 MOVI,            \ fd 0, POLLIN
  RDI S-POLL [R15] LEA,  ESI 1 MOVI,  EDX 1 MOVI,  SYS-POLL SYS,   \ 1 fd, 1 ms
  EAX EAX TEST,  G IF,
    EDI EDI XOR,  RSI S-CHAR [R15] LEA,  EDX 1 MOVI,  SYS-READ SYS,
    EAX EAX TEST,  IN-END JLE,                              \ the end of the input
    EAX S-CHAR [R15] BYTE MOVZX,  EAX DPUSH,  EAX -1 MOVI,  EAX DPUSH,  NEXT,
  THEN,
  EAX EAX XOR,  EAX DPUSH,  NEXT,

30 PRIM ( TX! )
  EAX DPOP,  S-CHAR [R15] AL MOV,
  EDI 1 MOVI,  RSI S-CHAR [R15] LEA,  EDX 1 MOVI,  SYS-WRITE SYS,  NEXT,

31 PRIM ( !IO )   NEXT,

\ ==== Time ==============================================================

LABEL NOW-MS   \ (CALL) EAX = the milliseconds since some point, modulo 2^32
  EDI 1 MOVI,  RSI S-TIME [R15] LEA,  SYS-CLOCK SYS,      \ CLOCK_MONOTONIC
  RAX S-TIME [R15] MOV,  ECX 1000 MOVI,  RCX MUL,  RCX RAX MOV,
  RAX S-TIME 8 + [R15] MOV,  EDX EDX XOR,  ESI 1000000 MOVI,  RSI DIV,
  RAX RCX ADD,  RET,

80 PRIM ( MILLIS )   NOW-MS CALL,  EAX START-MS [R15] SUB,  EAX DPUSH,  NEXT,
81 PRIM ( MS )   \ u / 1000 seconds, (u mod 1000) * 1000000 nanoseconds
  EAX DPOP,  EDX EDX XOR,  ECX 1000 MOVI,  ECX DIV,
  ECX EAX MOV,  EAX EDX MOV,  EDX 1000000 MOVI,  EDX MUL,  EDX EAX MOV,  EAX ECX MOV,  SLEEP JMP,
82 PRIM ( US )   \ u / 1000000 seconds, (u mod 1000000) * 1000 nanoseconds
  EAX DPOP,  EDX EDX XOR,  ECX 1000000 MOVI,  ECX DIV,
  ECX EAX MOV,  EAX EDX MOV,  EDX 1000 MOVI,  EDX MUL,  EDX EAX MOV,  EAX ECX MOV,  SLEEP JMP,

\ ==== HAL 83-88, 97, 99 ==================================================

83 PRIM ( BIS! )   EAX DPOP,  ECX DPOP,  0 [R15] [RAX*1] CL OR,  NEXT,
84 PRIM ( BIC! )   EAX DPOP,  ECX DPOP,  CL NOT,  0 [R15] [RAX*1] CL AND,  NEXT,
85 PRIM ( SAVE-IMAGE )   EAX -21 MOVI,  EAX DPUSH,  NEXT,   \ not here: build.fs makes programs
86 PRIM ( ERASE-IMAGE )  NEXT,
87 PRIM ( REBOOT )   \ ( f -- ): empty stacks, then BOOT (f true) or QUIT
  EAX DPOP,  R13D S0 MOVI,  EBP R0 MOVI,  STATE [R15] 0 MOVI,  HANDLER [R15] 0 MOVI,
  EAX EAX TEST,  Z IF,  EBX XT-QUIT [RIP] MOV,  ELSE,  EBX XT-BOOT [RIP] MOV,  THEN,
  DISPATCH JMP,
88 PRIM ( BYE )     EDI EDI XOR,  SYS-EXIT SYS,
99 PRIM ( BYE-N )   EDI DPOP,  SYS-EXIT SYS,

\ ==== SYSCALL: HAL 100 ===================================================
\ ( x1 .. xn n id -- x ): the call of Linux with the number id AND $FFFF;
\ x1 .. xn go to RDI RSI RDX R10 R8 R9 (n at most 6). Bit 16 + k of id:
\ x(k+1) is a VM address, the call gets MEM + x; 0 stays 0 (the null
\ pointer of C) (lib/linux/x64.fs). Each other argument goes to 64 bits
\ with its sign. x: the low 32 bits of RAX; an error is - the error number.

100 PRIM ( SYSCALL )
  EDX DPOP,  ECX DPOP,                               \ EDX id, ECX n
  ECX 6 CMPI,  A IF,  EAX -24 MOVI,  THROW-CODE JMP,  THEN,
  EAX 0 [R13] [RCX*4] LEA,                           \ EAX: the stack without x1 .. xn
  EAX S0 CMPI,  A IF,  EAX -4 MOVI,  THROW-CODE JMP,  THEN,
  R11D EDX MOV,  EDX 16 SHR,                         \ R11D the id; EDX the mask
  ESI EAX MOV,  RDI S-REGS [R15] LEA,                \ ESI: after x1; RDI: the place of x1
  ECX ECX TEST,  NZ IF,
    BEGIN,
      ESI 4 SUBI,  R9D 0 [R15] [RSI*1] MOV,          \ x1, then x2 ...
      EDX 1 TESTI,  Z IF,  R9 R9D MOVSXD,
      ELSE,  R9D R9D TEST,  NZ IF,  R9 R15 ADD,  THEN,  THEN,
      0 [RDI] R9 MOV,  RDI 8 ADDI,  EDX 1 SHR,  ECX DEC,
    Z UNTIL,
  THEN,
  R13D EAX MOV,  R13D 4 SUBI,                        \ the result goes to the place of x1
  EAX R11D MOV,  EAX $FFFF ANDI,
  RDI S-REGS [R15] MOV,       RSI S-REGS 8 + [R15] MOV,    RDX S-REGS 16 + [R15] MOV,
  R10 S-REGS 24 + [R15] MOV,  R8 S-REGS 32 + [R15] MOV,    R9 S-REGS 40 + [R15] MOV,
  SYSCALL,  TOS EAX MOV,  NEXT,

\ ARG ( n -- a u ): the argument n of the command line (0: the program).
\ Linux puts argc and the addresses of the arguments on the stack of the
\ start (S-ARGP), 64 bits each. The copy goes to S-ARGS + 256 n, at most
\ 255 bytes; the VM reaches it.
97 PRIM ( ARG )
  EAX TOS MOV,  EAX 31 CMPI,  L8 JA,
  RDX S-ARGP [R15] MOV,  RAX 0 [RDX] CMP,  L8 JAE,    \ n >= argc: none
  RDX 8 [RDX] [RAX*8] MOV,                           \ the address of argument n
  ECX EAX MOV,  ECX 8 SHL,  ECX S-ARGS ADDI,         \ the VM address of the copy
  RDI 0 [R15] [RCX*1] LEA,  ESI ESI XOR,
LABEL L1                                             \ copy up to the byte 0
  AL 0 [RDX] [RSI*1] MOV,  AL AL TEST,  L6 JZ,
  0 [RDI] [RSI*1] AL MOV,  ESI INC,  ESI 255 CMPI,  L1 JB,
LABEL L6
  TOS ECX MOV,  ESI DPUSH,  NEXT,
LABEL L8
  TOS 0 MOVI,  EAX EAX XOR,  EAX DPUSH,  NEXT,

\ ==== Files: HAL 89-93, 98 (doc/vm.md, section 8) ======================
\ A fid is a file descriptor of Linux. F-FID, F-POS and F-ERR hold the
\ fd, the bytes so far and the error while a call changes the registers.

LABEL PATH   \ (CALL) EAX a, ECX u: the name to S-PATH, then a byte 0; RDI its address
  ECX 255 CMPI,  A IF,  ECX 255 MOVI,  THEN,
  RSI 0 [R15] [RAX*1] LEA,  RDI S-PATH [R15] LEA,  REP, MOVSB,
  0 [RDI] BYTE 0 MOVI,  RDI S-PATH [R15] LEA,  RET,

LABEL OPEN-ERROR   \ EAX: - the error number; ( x x -- 0 ior )
  ECX -37 MOVI,  EAX -2 CMPI,  Z IF,  ECX -38 MOVI,  THEN,   \ ENOENT: the file is not there
  NOS 0 MOVI,  TOS ECX MOV,  NEXT,

LABEL OPEN-COMMON   \ ( a u fam -- fid ior ); EDX: more flags of open
  EAX DPOP,  EAX 3 ANDI,  EAX EDX OR,  F-FLAGS [R15] EAX MOV,   \ fam 0 1 2 = O_RDONLY O_WRONLY O_RDWR
  EAX NOS MOV,  ECX TOS MOV,  PATH CALL,
  ESI F-FLAGS [R15] MOV,  EDX $1A4 MOVI,  SYS-OPEN SYS,   \ mode 644
  EAX EAX TEST,  OPEN-ERROR JS,
  NOS EAX MOV,  TOS 0 MOVI,  NEXT,

89 PRIM ( OPEN-FILE )     EDX EDX XOR,  OPEN-COMMON JMP,
90 PRIM ( CREATE-FILE )   EDX $240 MOVI,  OPEN-COMMON JMP,   \ O_CREAT O_TRUNC

91 PRIM ( CLOSE-FILE )
  EDI TOS MOV,  SYS-CLOSE SYS,
  EAX EAX TEST,  EAX 0 MOVI,  S IF,  EAX -37 MOVI,  THEN,  TOS EAX MOV,  NEXT,

92 PRIM ( READ-FILE )   \ ( a u fid -- u2 ior )
  EAX DPOP,  F-FID [R15] EAX MOV,  F-POS [R15] 0 MOVI,  F-ERR [R15] 0 MOVI,
LABEL L1
  EAX F-POS [R15] MOV,  EAX TOS CMP,  L5 JAE,       \ u bytes: done
  EDX TOS MOV,  EDX EAX SUB,  ESI NOS MOV,  ESI EAX ADD,  RSI R15 ADD,
  EDI F-FID [R15] MOV,  SYS-READ SYS,
  EAX EAX TEST,  S IF,  F-ERR [R15] -37 MOVI,  L5 JMP,  THEN,
  EAX EAX TEST,  L5 JZ,                             \ the end of the file
  F-POS [R15] EAX ADD,  L1 JMP,
LABEL L5
  EAX F-POS [R15] MOV,  NOS EAX MOV,  EAX F-ERR [R15] MOV,  TOS EAX MOV,  NEXT,

93 PRIM ( WRITE-FILE )   \ ( a u fid -- ior )
  EAX DPOP,  F-FID [R15] EAX MOV,  F-POS [R15] 0 MOVI,
LABEL L1
  EAX F-POS [R15] MOV,  EAX TOS CMP,  L5 JAE,
  EDX TOS MOV,  EDX EAX SUB,  ESI NOS MOV,  ESI EAX ADD,  RSI R15 ADD,
  EDI F-FID [R15] MOV,  SYS-WRITE SYS,
  EAX EAX TEST,  L4 JLE,                            \ an error, or nothing written
  F-POS [R15] EAX ADD,  L1 JMP,
LABEL L4
  R13D 4 ADDI,  TOS -37 MOVI,  NEXT,
LABEL L5
  R13D 4 ADDI,  TOS 0 MOVI,  NEXT,

98 PRIM ( READ-LINE )   \ ( a u fid -- u2 f ior ): byte for byte up to LF
  EAX DPOP,  F-FID [R15] EAX MOV,  F-POS [R15] 0 MOVI,  F-ERR [R15] 0 MOVI,   \ F-ERR: a byte came
LABEL L1
  EAX F-POS [R15] MOV,  EAX TOS CMP,  L6 JAE,       \ the buffer is full
  EDI F-FID [R15] MOV,  RSI S-CHAR [R15] LEA,  EDX 1 MOVI,  SYS-READ SYS,
  EAX EAX TEST,  L7 JS,                            \ an error
  L4 JZ,                                           \ no byte: the end of the file
  F-ERR [R15] 1 MOVI,
  AL S-CHAR [R15] MOV,  AL 10 CMPI,  L5 JZ,
  ECX NOS MOV,  ECX F-POS [R15] ADD,  0 [R15] [RCX*1] AL MOV,  F-POS [R15] INC,  L1 JMP,
LABEL L4
  F-ERR [R15] 0 CMPI,  L6 JNZ,                     \ a last line without LF
  NOS 0 MOVI,  TOS 0 MOVI,  EAX EAX XOR,  EAX DPUSH,  NEXT,
LABEL L5                                           \ LF: a CR before it goes away
  EAX F-POS [R15] MOV,  EAX EAX TEST,  L6 JZ,
  ECX NOS MOV,  ECX EAX ADD,  -1 [R15] [RCX*1] BYTE 13 CMPI,  Z IF,  F-POS [R15] DEC,  THEN,
LABEL L6                                           \ a line: ( -- u2 true 0 )
  EAX F-POS [R15] MOV,  NOS EAX MOV,  TOS -1 MOVI,  EAX EAX XOR,  EAX DPUSH,  NEXT,
LABEL L7
  EAX F-POS [R15] MOV,  NOS EAX MOV,  TOS 0 MOVI,  EAX -37 MOVI,  EAX DPUSH,  NEXT,

\ ==== Serial lines: HAL 94-96 (doc/vm.md, section 8) =====================
\ The tool ucf talks to a board over a serial line: raw bytes, 8 data
\ bits, no parity, no flow control; a read gives the bytes that are
\ there, or none (VMIN = VTIME = 0). The speed is one of the table
\ BAUDS: Linux gives c_cflag a code for each of them (termbits.h).

LABEL BAUDS   \ pairs: the speed, its code; 0 at the end
      50 T,     1 T,       75 T,     2 T,      110 T,     3 T,      134 T,     4 T,
     150 T,     5 T,      200 T,     6 T,      300 T,     7 T,      600 T,     8 T,
    1200 T,     9 T,     1800 T,    10 T,     2400 T,    11 T,     4800 T,    12 T,
    9600 T,    13 T,    19200 T,    14 T,    38400 T,    15 T,    57600 T, $1001 T,
  115200 T, $1002 T,   230400 T, $1003 T,   460800 T, $1004 T,   500000 T, $1005 T,
  576000 T, $1006 T,   921600 T, $1007 T,  1000000 T, $1008 T,  1152000 T, $1009 T,
 1500000 T, $100A T,  2000000 T, $100B T,  2500000 T, $100C T,  3000000 T, $100D T,
 3500000 T, $100E T,  4000000 T, $100F T,        0 T,     0 T,

LABEL SET-TIO   \ (CALL) the line F-FID raw at the speed F-BAUD; EAX < 0: an error
  RDX BAUDS [RIP] LEA,
LABEL L1
  EAX 0 [RDX] MOV,  EAX EAX TEST,  Z IF,  EAX -22 MOVI,  RET,  THEN,   \ not in the table
  EAX F-BAUD [R15] CMP,  L2 JZ,
  RDX 8 ADDI,  L1 JMP,
LABEL L2
  EAX 4 [RDX] MOV,  F-CODE [R15] EAX MOV,
  EDI F-FID [R15] MOV,  ESI $5401 MOVI,  RDX S-TIO [R15] LEA,  SYS-IOCTL SYS,   \ TCGETS
  EAX EAX TEST,  S IF,  RET,  THEN,
  S-TIO [R15] 0 MOVI,  S-TIO 4 + [R15] 0 MOVI,        \ iflag, oflag: raw
  EAX F-CODE [R15] MOV,  EAX $8B0 ORI,  S-TIO 8 + [R15] EAX MOV,   \ cflag: the speed, CS8 CREAD CLOCAL
  S-TIO 12 + [R15] 0 MOVI,                            \ lflag: no echo, no lines
  S-TIO 22 + [R15] BYTE 0 MOVI,  S-TIO 23 + [R15] BYTE 0 MOVI,   \ VTIME 0, VMIN 0
  EDI F-FID [R15] MOV,  ESI $5402 MOVI,  RDX S-TIO [R15] LEA,  SYS-IOCTL SYS,  RET,   \ TCSETS

94 PRIM ( SERIAL-OPEN )   \ ( a u baud -- fid ior )
  EAX DPOP,  F-BAUD [R15] EAX MOV,
  EAX NOS MOV,  ECX TOS MOV,  PATH CALL,
  ESI $902 MOVI,  EDX EDX XOR,  SYS-OPEN SYS,       \ O_RDWR O_NOCTTY O_NONBLOCK
  EAX EAX TEST,  OPEN-ERROR JS,
  F-FID [R15] EAX MOV,
  EDI EAX MOV,  ESI 4 MOVI,  EDX EDX XOR,  SYS-FCNTL SYS,   \ F_SETFL 0: reads follow VMIN, VTIME
  SET-TIO CALL,
  EAX EAX TEST,  S IF,                             \ not a serial line, or a speed it does not know
    EDI F-FID [R15] MOV,  SYS-CLOSE SYS,  NOS 0 MOVI,  TOS -37 MOVI,  NEXT,
  THEN,
  EAX F-FID [R15] MOV,  NOS EAX MOV,  TOS 0 MOVI,  NEXT,

95 PRIM ( SERIAL-LINES )   \ ( dtr rts fid -- ior ): true = the line is active
  EAX DPOP,  F-FID [R15] EAX MOV,
  EAX DPOP,  ECX ECX XOR,  EAX EAX TEST,  NZ IF,  ECX 4 MOVI,  THEN,   \ TIOCM_RTS
  EAX TOS MOV,  EAX EAX TEST,  NZ IF,  ECX 2 ORI,  THEN,               \ TIOCM_DTR
  S-MBITS [R15] ECX MOV,
  EDI F-FID [R15] MOV,  ESI $5416 MOVI,  RDX S-MBITS [R15] LEA,  SYS-IOCTL SYS,   \ TIOCMBIS: these on
  EAX EAX TEST,  S IF,  TOS -37 MOVI,  NEXT,  THEN,
  S-MBITS [R15] 6 XORI,
  EDI F-FID [R15] MOV,  ESI $5417 MOVI,  RDX S-MBITS [R15] LEA,  SYS-IOCTL SYS,   \ TIOCMBIC: the others off
  EAX EAX TEST,  EAX 0 MOVI,  S IF,  EAX -37 MOVI,  THEN,  TOS EAX MOV,  NEXT,

96 PRIM ( SERIAL-BAUD )   \ ( baud fid -- ior )
  EAX DPOP,  F-FID [R15] EAX MOV,  EAX TOS MOV,  F-BAUD [R15] EAX MOV,
  SET-TIO CALL,  EAX EAX TEST,  EAX 0 MOVI,  S IF,  EAX -37 MOVI,  THEN,  TOS EAX MOV,  NEXT,

\ ==== The start =========================================================
\ Linux starts the program at START. The memory of the VM (the image)
\ is in place already: Linux mapped it from the file.

LABEL START
  R15 MEM-AT [RIP] LEA,  R14 OPTAB [RIP] LEA,
  S-ARGP [R15] RSP MOV,
  RAX ON-SIGINT [RIP] LEA,  S-ACT [R15] RAX MOV,
  S-ACT 8 + [R15] QWORD $04000004 MOVI,                 \ SA_RESTORER SA_SIGINFO
  RAX SIGRETURN [RIP] LEA,  S-ACT 16 + [R15] RAX MOV,
  EDI 2 MOVI,  RSI S-ACT [R15] LEA,  EDX EDX XOR,  R10D 8 MOVI,  SYS-SIGACTION SYS,   \ SIGINT
  RAX ON-SEGV [RIP] LEA,  S-ACT [R15] RAX MOV,
  EDI 11 MOVI,  RSI S-ACT [R15] LEA,  EDX EDX XOR,  R10D 8 MOVI,  SYS-SIGACTION SYS,  \ SIGSEGV
  NOW-MS CALL,  START-MS [R15] EAX MOV,
  R13D S0 MOVI,  EBP R0 MOVI,                            \ doc/vm.md, section 10
  STATE [R15] 0 MOVI,  HANDLER [R15] 0 MOVI,
  EBX XT-BOOT [RIP] MOV,  DISPATCH JMP,
