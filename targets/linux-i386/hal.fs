\ hal.fs - the HAL of uCForth for Linux on a PC with 32 bits (i386)
\
\ The start code, the console (primitives 29-31) and the HAL (80-100)
\ as calls of the Linux kernel. No C library: a call puts its number
\ into EAX and its arguments into EBX, ECX and EDX, then "int 0x80"; the
\ result comes back in EAX, a negative number is an error. Linux keeps
\ all other registers. EBX is W of the VM: a primitive may change it,
\ NEXT loads it again. ESI, EDI and EBP are the VM; only the start and
\ SYSCALL (100: each call of the system for Forth) give more arguments,
\ in ESI, EDI and EBP.
\
\ Loaded by build.fs after kernels/x86.fs. build.fs gives MEM, SCR (the
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
\ dispatch runs BREAK (kernels/x86.fs). A call that waits (poll,
\ nanosleep) ends at once. Ctrl-\ (SIGQUIT) still ends the program. An
\ access to an address outside the program (for example $7FFFFFF0 @)
\ is THROW -9, not the end of the program: see ON-SEGV.

3 CONSTANT SYS-READ        4 CONSTANT SYS-WRITE       5 CONSTANT SYS-OPEN
6 CONSTANT SYS-CLOSE       54 CONSTANT SYS-IOCTL      55 CONSTANT SYS-FCNTL
162 CONSTANT SYS-NANOSLEEP 168 CONSTANT SYS-POLL      173 CONSTANT SYS-SIGRETURN
174 CONSTANT SYS-SIGACTION 252 CONSTANT SYS-EXIT      265 CONSTANT SYS-CLOCK
: SYS, ( n -- )   EAX SWAP MOVI,  $80 INT, ;   \ the arguments are in EBX ECX EDX

\ The scratch area SCR, after the memory of the VM (KCELLS: SCR 0-15):
SCR 16 +    CONSTANT S-CHAR       \ one byte for TX! and READ-LINE
SCR 20 +    CONSTANT START-MS     \ the clock at the start
SCR 24 +    CONSTANT S-ARGP       \ ESP at the start: argc, then the addresses of the arguments
SCR 28 +    CONSTANT S-POLL       \ struct pollfd: fd, events, revents (8 bytes)
SCR 36 +    CONSTANT S-TIME       \ struct timespec: seconds, nanoseconds (8 bytes)
SCR 44 +    CONSTANT F-FID        \ files and serial lines: the fd,
SCR 48 +    CONSTANT F-POS        \ the bytes so far,
SCR 52 +    CONSTANT F-ERR        \ an error code, or: a byte came (READ-LINE)
SCR 56 +    CONSTANT F-FLAGS      \ the flags of open
SCR 60 +    CONSTANT F-BAUD       \ the speed of a serial line,
SCR 64 +    CONSTANT F-CODE       \ and its code for c_cflag
SCR 68 +    CONSTANT S-MBITS      \ the bits for TIOCMBIS and TIOCMBIC
SCR 72 +    CONSTANT S-ACT        \ struct sigaction: handler, flags, restorer, mask (20 bytes)
SCR 96 +    CONSTANT S-DFL        \ the same, all 0: the default action
SCR 128 +   CONSTANT S-TIO        \ struct termios (36 bytes)
SCR 256 +   CONSTANT S-PATH       \ a file name with a byte 0 at the end (255 + 1)
SCR 512 +   CONSTANT S-REGS       \ SYSCALL: the 6 arguments, the id, ESI EDI EBP, the new stack
SCR $1000 + CONSTANT S-ARGS       \ ARG copies argument n here, 256 bytes each, n < 32

\ ==== Break and faults ==================================================

LABEL XT-REFILL    0 T,           \ build.fs: the xt of REFILL and the end of its body
LABEL REFILL-END   0 T,
LABEL OUT-ADDR     0 T,           \ build.fs: the VM address of OUT
LABEL CODE-END     0 T,           \ build.fs: the end of the code

\ ON-SIGINT: void handler (int, siginfo_t *, ucontext_t *). Linux calls
\ it between two instructions of the program; it returns through
\ SIGRETURN. Both handlers have SA_SIGINFO: then Linux makes the frame
\ that rt_sigreturn expects.
LABEL ON-SIGINT
  BREAK-OFF [] 128 MOVI,  RET,
LABEL SIGRETURN
  SYS-SIGRETURN SYS,

\ BREAK: each primitive after a break. It writes a new line and sets
\ OUT to 0. With a CATCH (HANDLER not 0) it runs -28 THROW ("user
\ interrupt"); without one (QUIT waits in REFILL, or BOOT waits for a
\ key) it starts QUIT again.
LABEL BREAK
  BREAK-OFF [] 0 MOVI,
  S-CHAR [] BYTE 10 MOVI,  EBX 1 MOVI,  ECX S-CHAR MOVI,  EDX 1 MOVI,  SYS-WRITE SYS,
  EAX OUT-ADDR [] MOV,  MEM [EAX] 0 MOVI,
  EAX MEM HANDLER + [] MOV,  EAX EAX TEST,
  Z IF,
    EDI S0 MOVI,  EBP R0 MOVI,  MEM STATE + [] 0 MOVI,
    EBX XT-QUIT [] MOV,  DISPATCH JMP,
  THEN,
  EAX -28 MOVI,  THROW-CODE JMP,

\ FAULT: a primitive read or wrote outside the program. THROW -9 (the
\ stacks are as before the primitive), or QUIT when a stack pointer
\ itself is outside the memory of the VM.
LABEL FAULT
  EDI VM-END CMPI,  AE IF,  L1 JMP,  THEN,
  EBP VM-END CMPI,  AE IF,  L1 JMP,  THEN,
  EAX -9 MOVI,  THROW-CODE JMP,
LABEL L1
  EDI S0 MOVI,  EBP R0 MOVI,  MEM STATE + [] 0 MOVI,  MEM HANDLER + [] 0 MOVI,
  EBX XT-QUIT [] MOV,  DISPATCH JMP,

\ ON-SEGV: void handler (int, siginfo_t *, ucontext_t *), for SIGSEGV.
\ EIP is at offset 76 of the ucontext (uc_mcontext.gregs[14]). A fault in
\ the code of the program goes on at FAULT. Another one: the default
\ action again, and the same fault ends the program.
LABEL ON-SEGV
  EAX 12 [ESP] MOV,  ECX 76 [EAX] MOV,
  ECX CODE-START CMPI,  L2 JB,  ECX CODE-END [] CMP,  L2 JAE,
  76 [EAX] FAULT MOVI,  RET,                         \ go on, at FAULT
LABEL L2
  EBX 11 MOVI,  ECX S-DFL MOVI,  EDX EDX XOR,  ESI 8 MOVI,  SYS-SIGACTION SYS,  RET,

\ ==== The console =======================================================

LABEL SLEEP   \ (JMP) wait ECX seconds and EDX nanoseconds, then NEXT; a break ends the wait
  S-TIME [] ECX MOV,  S-TIME 4 + [] EDX MOV,
  EBX S-TIME MOVI,  ECX ECX XOR,  SYS-NANOSLEEP SYS,  NEXT,

\ At the end of the input (a pipe, a file, Ctrl-D), ?RX ends the program
\ only when QUIT waits for a line: a return address on the return stack
\ points into REFILL, as in a bootstrap VM (doc/vm.md, section 9). Else
\ it answers "no key" and waits 1 ms.
LABEL IN-END
  EAX EBP MOV,  ECX 8 MOVI,
  BEGIN,
    EAX R0 CMPI,  B IF,
      EDX MEM [EAX] MOV,
      EDX XT-REFILL [] CMP,  A IF,  EDX REFILL-END [] CMP,  B IF,  EBX EBX XOR,  SYS-EXIT SYS,  THEN,  THEN,
    THEN,
    EAX 4 ADDI,  ECX DEC,
  Z UNTIL,
  EAX EAX XOR,  EAX DPUSH,
  ECX ECX XOR,  EDX 1000000 MOVI,  SLEEP JMP,

29 PRIM ( ?RX )   \ poll: wait at most 1 ms, then read 1 byte
  S-POLL [] 0 MOVI,  S-POLL 4 + [] 1 MOVI,                \ fd 0, POLLIN
  EBX S-POLL MOVI,  ECX 1 MOVI,  EDX 1 MOVI,  SYS-POLL SYS,   \ 1 fd, 1 ms
  EAX EAX TEST,  G IF,
    EBX EBX XOR,  ECX S-CHAR MOVI,  EDX 1 MOVI,  SYS-READ SYS,
    EAX EAX TEST,  IN-END JLE,                              \ the end of the input
    EAX S-CHAR [] BYTE MOVZX,  EAX DPUSH,  EAX -1 MOVI,  EAX DPUSH,  NEXT,
  THEN,
  EAX EAX XOR,  EAX DPUSH,  NEXT,

30 PRIM ( TX! )
  EAX DPOP,  S-CHAR [] AL MOV,
  EBX 1 MOVI,  ECX S-CHAR MOVI,  EDX 1 MOVI,  SYS-WRITE SYS,  NEXT,

31 PRIM ( !IO )   NEXT,

\ ==== Time ==============================================================

LABEL NOW-MS   \ (CALL) EAX = the milliseconds since some point, modulo 2^32
  EBX 1 MOVI,  ECX S-TIME MOVI,  SYS-CLOCK SYS,          \ CLOCK_MONOTONIC
  EAX S-TIME [] MOV,  ECX 1000 MOVI,  ECX MUL,  ECX EAX MOV,
  EAX S-TIME 4 + [] MOV,  EDX EDX XOR,  EBX 1000000 MOVI,  EBX DIV,
  EAX ECX ADD,  RET,

80 PRIM ( MILLIS )   NOW-MS CALL,  EAX START-MS [] SUB,  EAX DPUSH,  NEXT,
81 PRIM ( MS )   \ u / 1000 seconds, (u mod 1000) * 1000000 nanoseconds
  EAX DPOP,  EDX EDX XOR,  ECX 1000 MOVI,  ECX DIV,
  ECX EAX MOV,  EAX EDX MOV,  EDX 1000000 MOVI,  EDX MUL,  EDX EAX MOV,  SLEEP JMP,
82 PRIM ( US )   \ u / 1000000 seconds, (u mod 1000000) * 1000 nanoseconds
  EAX DPOP,  EDX EDX XOR,  ECX 1000000 MOVI,  ECX DIV,
  ECX EAX MOV,  EAX EDX MOV,  EDX 1000 MOVI,  EDX MUL,  EDX EAX MOV,  SLEEP JMP,

\ ==== HAL 83-88, 97, 99 ==================================================

83 PRIM ( BIS! )   EAX DPOP,  ECX DPOP,  MEM [EAX] CL OR,  NEXT,
84 PRIM ( BIC! )   EAX DPOP,  ECX DPOP,  CL NOT,  MEM [EAX] CL AND,  NEXT,
85 PRIM ( SAVE-IMAGE )   EAX -21 MOVI,  EAX DPUSH,  NEXT,   \ not here: build.fs makes programs
86 PRIM ( ERASE-IMAGE )  NEXT,
87 PRIM ( REBOOT )   \ ( f -- ): empty stacks, then BOOT (f true) or QUIT
  EAX DPOP,  EDI S0 MOVI,  EBP R0 MOVI,  MEM STATE + [] 0 MOVI,  MEM HANDLER + [] 0 MOVI,
  EAX EAX TEST,  Z IF,  EBX XT-QUIT [] MOV,  ELSE,  EBX XT-BOOT [] MOV,  THEN,
  DISPATCH JMP,
88 PRIM ( BYE )     EBX EBX XOR,  SYS-EXIT SYS,
99 PRIM ( BYE-N )   EBX DPOP,  SYS-EXIT SYS,

\ ==== SYSCALL: HAL 100 ===================================================
\ ( x1 .. xn n id -- x ): the call of Linux with the number id AND $FFFF;
\ x1 .. xn go to EBX ECX EDX ESI EDI EBP (n at most 6). Bit 16 + k of
\ id: x(k+1) is a VM address, the call gets MEM + x; 0 stays 0 (the null
\ pointer of C) (lib/linux/i386.fs). x: EAX; an error is - the error
\ number. ESI, EDI and EBP of the VM wait in S-REGS during the call.

100 PRIM ( SYSCALL )
  EDX DPOP,  ECX DPOP,                               \ EDX id, ECX n
  ECX 6 CMPI,  A IF,  EAX -24 MOVI,  THROW-CODE JMP,  THEN,
  EAX 0 [EDI] [ECX*4] LEA,                           \ EAX: the stack without x1 .. xn
  EAX S0 CMPI,  A IF,  EAX -4 MOVI,  THROW-CODE JMP,  THEN,
  S-REGS 24 + [] EDX MOV,  S-REGS 40 + [] EAX MOV,   \ the id, the new stack
  S-REGS 28 + [] ESI MOV,  S-REGS 32 + [] EDI MOV,  S-REGS 36 + [] EBP MOV,
  EBX S-REGS MOVI,  EDX 16 SHR,                      \ EBX: the place of x1; EDX the mask
  ECX ECX TEST,  NZ IF,
    BEGIN,
      EAX 4 SUBI,  ESI MEM [EAX] MOV,                \ x1, then x2 ...
      EDX 1 TESTI,  NZ IF,  ESI ESI TEST,  NZ IF,  ESI MEM ADDI,  THEN,  THEN,
      0 [EBX] ESI MOV,  EBX 4 ADDI,  EDX 1 SHR,  ECX DEC,
    Z UNTIL,
  THEN,
  EAX S-REGS 24 + [] MOV,  EAX $FFFF ANDI,
  EBX S-REGS [] MOV,       ECX S-REGS 4 + [] MOV,    EDX S-REGS 8 + [] MOV,
  ESI S-REGS 12 + [] MOV,  EDI S-REGS 16 + [] MOV,   EBP S-REGS 20 + [] MOV,
  $80 INT,
  ESI S-REGS 28 + [] MOV,  EBP S-REGS 36 + [] MOV,
  EDI S-REGS 40 + [] MOV,  EDI 4 SUBI,               \ the result goes to the place of x1
  TOS EAX MOV,  NEXT,

\ ARG ( n -- a u ): the argument n of the command line (0: the program).
\ Linux puts argc and the addresses of the arguments on the stack of the
\ start (S-ARGP). The copy goes to S-ARGS + 256 n, at most 255 bytes;
\ the VM reaches it (VM address S-ARGS - MEM + 256 n).
97 PRIM ( ARG )
  EAX TOS MOV,  EAX 31 CMPI,  L8 JA,
  EDX S-ARGP [] MOV,  EAX 0 [EDX] CMP,  L8 JAE,      \ n >= argc: none
  EDX 4 [EDX] [EAX*4] MOV,                           \ the address of argument n
  EBX EAX MOV,  EBX 8 SHL,  EBX S-ARGS ADDI,  ECX ECX XOR,
LABEL L1                                             \ copy up to the byte 0
  AL 0 [EDX] [ECX*1] MOV,  AL AL TEST,  L6 JZ,
  0 [EBX] [ECX*1] AL MOV,  ECX INC,  ECX 255 CMPI,  L1 JB,
LABEL L6
  EBX MEM SUBI,  TOS EBX MOV,  ECX DPUSH,  NEXT,
LABEL L8
  TOS 0 MOVI,  EAX EAX XOR,  EAX DPUSH,  NEXT,

\ ==== Files: HAL 89-93, 98 (doc/vm.md, section 8) ======================
\ A fid is a file descriptor of Linux. F-FID, F-POS and F-ERR hold the
\ fd, the bytes so far and the error while a call changes the registers.

LABEL PATH   \ (CALL) EAX a, ECX u: the name to S-PATH, then a byte 0
  ECX 255 CMPI,  A IF,  ECX 255 MOVI,  THEN,
  EDX S-PATH MOVI,  EAX MEM ADDI,
  BEGIN,  ECX ECX TEST,  NZ WHILE,
    BL 0 [EAX] MOV,  0 [EDX] BL MOV,  EAX INC,  EDX INC,  ECX DEC,
  REPEAT,
  0 [EDX] BYTE 0 MOVI,  RET,

LABEL OPEN-ERROR   \ EAX: - the error number; ( x x -- 0 ior )
  ECX -37 MOVI,  EAX -2 CMPI,  Z IF,  ECX -38 MOVI,  THEN,   \ ENOENT: the file is not there
  NOS 0 MOVI,  TOS ECX MOV,  NEXT,

LABEL OPEN-COMMON   \ ( a u fam -- fid ior ); EDX: more flags of open
  EAX DPOP,  EAX 3 ANDI,  EAX EDX OR,  F-FLAGS [] EAX MOV,   \ fam 0 1 2 = O_RDONLY O_WRONLY O_RDWR
  EAX NOS MOV,  ECX TOS MOV,  PATH CALL,
  EBX S-PATH MOVI,  ECX F-FLAGS [] MOV,  EDX $1A4 MOVI,  SYS-OPEN SYS,   \ mode 644
  EAX EAX TEST,  OPEN-ERROR JS,
  NOS EAX MOV,  TOS 0 MOVI,  NEXT,

89 PRIM ( OPEN-FILE )     EDX EDX XOR,  OPEN-COMMON JMP,
90 PRIM ( CREATE-FILE )   EDX $240 MOVI,  OPEN-COMMON JMP,   \ O_CREAT O_TRUNC

91 PRIM ( CLOSE-FILE )
  EBX TOS MOV,  SYS-CLOSE SYS,
  EAX EAX TEST,  EAX 0 MOVI,  S IF,  EAX -37 MOVI,  THEN,  TOS EAX MOV,  NEXT,

92 PRIM ( READ-FILE )   \ ( a u fid -- u2 ior )
  EAX DPOP,  F-FID [] EAX MOV,  F-POS [] 0 MOVI,  F-ERR [] 0 MOVI,
LABEL L1
  EAX F-POS [] MOV,  EAX TOS CMP,  L5 JAE,          \ u bytes: done
  EDX TOS MOV,  EDX EAX SUB,  ECX NOS MOV,  ECX EAX ADD,  ECX MEM ADDI,
  EBX F-FID [] MOV,  SYS-READ SYS,
  EAX EAX TEST,  S IF,  F-ERR [] -37 MOVI,  L5 JMP,  THEN,
  EAX EAX TEST,  L5 JZ,                             \ the end of the file
  F-POS [] EAX ADD,  L1 JMP,
LABEL L5
  EAX F-POS [] MOV,  NOS EAX MOV,  EAX F-ERR [] MOV,  TOS EAX MOV,  NEXT,

93 PRIM ( WRITE-FILE )   \ ( a u fid -- ior )
  EAX DPOP,  F-FID [] EAX MOV,  F-POS [] 0 MOVI,
LABEL L1
  EAX F-POS [] MOV,  EAX TOS CMP,  L5 JAE,
  EDX TOS MOV,  EDX EAX SUB,  ECX NOS MOV,  ECX EAX ADD,  ECX MEM ADDI,
  EBX F-FID [] MOV,  SYS-WRITE SYS,
  EAX EAX TEST,  L4 JLE,                            \ an error, or nothing written
  F-POS [] EAX ADD,  L1 JMP,
LABEL L4
  EDI 4 ADDI,  TOS -37 MOVI,  NEXT,
LABEL L5
  EDI 4 ADDI,  TOS 0 MOVI,  NEXT,

98 PRIM ( READ-LINE )   \ ( a u fid -- u2 f ior ): byte for byte up to LF
  EAX DPOP,  F-FID [] EAX MOV,  F-POS [] 0 MOVI,  F-ERR [] 0 MOVI,   \ F-ERR: a byte came
LABEL L1
  EAX F-POS [] MOV,  EAX TOS CMP,  L6 JAE,         \ the buffer is full
  EBX F-FID [] MOV,  ECX S-CHAR MOVI,  EDX 1 MOVI,  SYS-READ SYS,
  EAX EAX TEST,  L7 JS,                            \ an error
  L4 JZ,                                           \ no byte: the end of the file
  F-ERR [] 1 MOVI,
  AL S-CHAR [] MOV,  AL 10 CMPI,  L5 JZ,
  ECX NOS MOV,  EDX F-POS [] MOV,  MEM [ECX] [EDX*1] AL MOV,  F-POS [] INC,  L1 JMP,
LABEL L4
  F-ERR [] 0 CMPI,  L6 JNZ,                        \ a last line without LF
  NOS 0 MOVI,  TOS 0 MOVI,  EAX EAX XOR,  EAX DPUSH,  NEXT,
LABEL L5                                           \ LF: a CR before it goes away
  EAX F-POS [] MOV,  EAX EAX TEST,  L6 JZ,
  ECX NOS MOV,  MEM 1- [ECX] [EAX*1] BYTE 13 CMPI,  Z IF,  F-POS [] DEC,  THEN,
LABEL L6                                           \ a line: ( -- u2 true 0 )
  EAX F-POS [] MOV,  NOS EAX MOV,  TOS -1 MOVI,  EAX EAX XOR,  EAX DPUSH,  NEXT,
LABEL L7
  EAX F-POS [] MOV,  NOS EAX MOV,  TOS 0 MOVI,  EAX -37 MOVI,  EAX DPUSH,  NEXT,

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
  EDX BAUDS MOVI,
LABEL L1
  EAX 0 [EDX] MOV,  EAX EAX TEST,  Z IF,  EAX -22 MOVI,  RET,  THEN,   \ not in the table
  EAX F-BAUD [] CMP,  L2 JZ,
  EDX 8 ADDI,  L1 JMP,
LABEL L2
  EAX 4 [EDX] MOV,  F-CODE [] EAX MOV,
  EBX F-FID [] MOV,  ECX $5401 MOVI,  EDX S-TIO MOVI,  SYS-IOCTL SYS,   \ TCGETS
  EAX EAX TEST,  S IF,  RET,  THEN,
  S-TIO [] 0 MOVI,  S-TIO 4 + [] 0 MOVI,              \ iflag, oflag: raw
  EAX F-CODE [] MOV,  EAX $8B0 ORI,  S-TIO 8 + [] EAX MOV,   \ cflag: the speed, CS8 CREAD CLOCAL
  S-TIO 12 + [] 0 MOVI,                               \ lflag: no echo, no lines
  S-TIO 22 + [] BYTE 0 MOVI,  S-TIO 23 + [] BYTE 0 MOVI,   \ VTIME 0, VMIN 0
  EBX F-FID [] MOV,  ECX $5402 MOVI,  EDX S-TIO MOVI,  SYS-IOCTL SYS,  RET,   \ TCSETS

94 PRIM ( SERIAL-OPEN )   \ ( a u baud -- fid ior )
  EAX DPOP,  F-BAUD [] EAX MOV,
  EAX NOS MOV,  ECX TOS MOV,  PATH CALL,
  EBX S-PATH MOVI,  ECX $902 MOVI,  EDX EDX XOR,  SYS-OPEN SYS,   \ O_RDWR O_NOCTTY O_NONBLOCK
  EAX EAX TEST,  OPEN-ERROR JS,
  F-FID [] EAX MOV,
  EBX EAX MOV,  ECX 4 MOVI,  EDX EDX XOR,  SYS-FCNTL SYS,         \ F_SETFL 0: reads follow VMIN, VTIME
  SET-TIO CALL,
  EAX EAX TEST,  S IF,                             \ not a serial line, or a speed it does not know
    EBX F-FID [] MOV,  SYS-CLOSE SYS,  NOS 0 MOVI,  TOS -37 MOVI,  NEXT,
  THEN,
  EAX F-FID [] MOV,  NOS EAX MOV,  TOS 0 MOVI,  NEXT,

95 PRIM ( SERIAL-LINES )   \ ( dtr rts fid -- ior ): true = the line is active
  EAX DPOP,  F-FID [] EAX MOV,
  EAX DPOP,  ECX ECX XOR,  EAX EAX TEST,  NZ IF,  ECX 4 MOVI,  THEN,   \ TIOCM_RTS
  EAX TOS MOV,  EAX EAX TEST,  NZ IF,  ECX 2 ORI,  THEN,               \ TIOCM_DTR
  S-MBITS [] ECX MOV,
  EBX F-FID [] MOV,  ECX $5416 MOVI,  EDX S-MBITS MOVI,  SYS-IOCTL SYS,   \ TIOCMBIS: these on
  EAX EAX TEST,  S IF,  TOS -37 MOVI,  NEXT,  THEN,
  EAX 6 MOVI,  EAX S-MBITS [] XOR,  S-MBITS [] EAX MOV,
  EBX F-FID [] MOV,  ECX $5417 MOVI,  EDX S-MBITS MOVI,  SYS-IOCTL SYS,   \ TIOCMBIC: the others off
  EAX EAX TEST,  EAX 0 MOVI,  S IF,  EAX -37 MOVI,  THEN,  TOS EAX MOV,  NEXT,

96 PRIM ( SERIAL-BAUD )   \ ( baud fid -- ior )
  EAX DPOP,  F-FID [] EAX MOV,  EAX TOS MOV,  F-BAUD [] EAX MOV,
  SET-TIO CALL,  EAX EAX TEST,  EAX 0 MOVI,  S IF,  EAX -37 MOVI,  THEN,  TOS EAX MOV,  NEXT,

\ ==== The start =========================================================
\ Linux starts the program at START. The memory of the VM (the image)
\ is in place already: Linux mapped it from the file.

LABEL START
  S-ARGP [] ESP MOV,
  S-ACT [] ON-SIGINT MOVI,  S-ACT 4 + [] $04000004 MOVI,  S-ACT 8 + [] SIGRETURN MOVI,   \ SA_RESTORER SA_SIGINFO
  EBX 2 MOVI,  ECX S-ACT MOVI,  EDX EDX XOR,  ESI 8 MOVI,  SYS-SIGACTION SYS,   \ SIGINT
  S-ACT [] ON-SEGV MOVI,
  EBX 11 MOVI,  ECX S-ACT MOVI,  EDX EDX XOR,  ESI 8 MOVI,  SYS-SIGACTION SYS,  \ SIGSEGV
  NOW-MS CALL,  START-MS [] EAX MOV,
  EDI S0 MOVI,  EBP R0 MOVI,                             \ doc/vm.md, section 10
  MEM STATE + [] 0 MOVI,  MEM HANDLER + [] 0 MOVI,
  EBX XT-BOOT [] MOV,  DISPATCH JMP,
