\ hal.fs - the HAL of uCForth for Windows (32 bit)
\
\ The start code, the console (primitives 29-31) and the HAL (80-101) as
\ calls of KERNEL32.dll, the part of Windows that each program has. A
\ call pushes its arguments, the last one first, then "call [cell]":
\ Windows put the address of the function into the cell of the IAT
\ (lib/windows/pe.fs). The function takes the arguments from the stack
\ and gives its result in EAX; it changes ECX and EDX, and keeps EBX ESI
\ EDI EBP, the registers of the VM.
\
\ Loaded by build.fs after kernels/x86.fs. build.fs gives MEM, SCR (the
\ scratch area after the memory of the VM), KCELLS and CODE-START (the
\ address of the first byte of the code).
\
\ The console: a second thread (READER) reads the standard input with
\ ReadFile, a line at a time, and waits until ?RX has taken it. ?RX
\ waits at most 1 ms for a line, so KEY? does not stop the program. The
\ console of Windows shows the line while you type and sends it with the
\ return key; Ctrl-Z, then the return key, ends the input. The same code
\ reads a pipe or a file.
\
\ Ctrl-C stops the running word (doc/vm.md, section 9: break). Windows
\ calls ON-CTRL in a thread of its own; it sets BREAK-OFF to 128, so the
\ next dispatch runs BREAK (kernels/x86.fs). An access to an address
\ outside the program (for example $7FFFFFF0 @) is THROW -9, not the end
\ of the program: see ON-FAULT.

DLL KERNEL32.dll
IMPORT ExitProcess                IMPORT GetStdHandle             IMPORT GetConsoleMode
IMPORT ReadFile                   IMPORT WriteFile                IMPORT CreateFileA
IMPORT CloseHandle                IMPORT GetLastError             IMPORT Sleep
IMPORT CreateEventA               IMPORT SetEvent                 IMPORT ResetEvent
IMPORT WaitForSingleObject        IMPORT CreateThread             IMPORT SetConsoleCtrlHandler
IMPORT SetUnhandledExceptionFilter   IMPORT QueryPerformanceCounter
IMPORT QueryPerformanceFrequency  IMPORT GetCommandLineA          IMPORT GetCommState
IMPORT SetCommState               IMPORT SetCommTimeouts          IMPORT EscapeCommFunction
IMPORT GetCurrentThreadId         IMPORT LoadLibraryA             IMPORT GetProcAddress

: API, ( cell -- )   [] CALL-IND, ;   \ call the function whose address is in the cell

\ The scratch area SCR, after the memory of the VM (KCELLS: SCR 0-15):
SCR 16 +    CONSTANT S-CHAR       \ one byte for TX! and READ-LINE
SCR 20 +    CONSTANT S-DONE       \ the bytes that ReadFile or WriteFile moved
SCR 24 +    CONSTANT H-IN         \ the handles: standard input, standard output,
SCR 28 +    CONSTANT H-OUT
SCR 32 +    CONSTANT H-BREAK      \ the events: Ctrl-C came (it ends MS early),
SCR 36 +    CONSTANT H-DATA       \ READER has a line,
SCR 40 +    CONSTANT H-FREE       \ ?RX has taken the line
SCR 44 +    CONSTANT IN-COUNT     \ the bytes of the line in IN-BUF; 0: no line
SCR 48 +    CONSTANT IN-POS       \ the next byte for ?RX
SCR 52 +    CONSTANT IN-EOF       \ the input has ended
SCR 56 +    CONSTANT IN-CONSOLE   \ the input is a console (not a pipe, a file or NUL)
SCR 60 +    CONSTANT R-DONE       \ the bytes that READER got
SCR 64 +    CONSTANT TPM          \ the ticks of the performance counter in 1 ms
SCR 68 +    CONSTANT START-MS     \ the clock at the start
SCR 72 +    CONSTANT QPC          \ 8 bytes: the counter, or its frequency
SCR 80 +    CONSTANT F-FID        \ files and serial lines: the handle,
SCR 84 +    CONSTANT F-POS        \ the bytes so far,
SCR 88 +    CONSTANT F-ERR        \ an error code, or: a byte came (READ-LINE)
SCR 92 +    CONSTANT F-BAUD       \ the speed, or: RTS (SERIAL-LINES)
SCR 96 +    CONSTANT S-DCB        \ 28 bytes: the settings of a serial line (DCB)
SCR 128 +   CONSTANT S-TIMEOUTS   \ 20 bytes: its time limits (COMMTIMEOUTS)
SCR 148 +   CONSTANT L-DTR        \ the state of DTR and RTS: 1 on, 0 off
SCR 152 +   CONSTANT L-RTS
SCR 160 +   CONSTANT W-END        \ 8 bytes: MS waits until the counter is here
SCR 168 +   CONSTANT SC-ESP       \ SYSCALL: ESP before the call (0: no call), and the
SCR 172 +   CONSTANT SC-EDI       \ registers of the VM for a fault in the function
SCR 176 +   CONSTANT SC-ESI
SCR 180 +   CONSTANT SC-EBX
SCR 184 +   CONSTANT SC-EBP
SCR 188 +   CONSTANT MAIN-THREAD  \ the id of the thread that runs the VM
SCR 256 +   CONSTANT S-PATH       \ a file name with a byte 0 at the end (4 + 255 + 1)
SCR 1024 +  CONSTANT IN-BUF       \ a line of the input,
2048        CONSTANT IN-SIZE      \ at most 2048 bytes at a time
SCR $1000 + CONSTANT S-ARGS       \ ARG copies argument n here, 256 bytes each, n < 32

\ ==== Break and faults ==================================================

LABEL XT-REFILL    0 T,           \ build.fs: the xt of REFILL and the end of its body
LABEL REFILL-END   0 T,
LABEL OUT-ADDR     0 T,           \ build.fs: the VM address of OUT
LABEL CODE-END     0 T,           \ build.fs: the end of the code
LABEL CRLF         13 TC,  10 TC,  4 TALIGN

\ ON-CTRL: BOOL WINAPI (DWORD type). Ctrl-C (0) and Ctrl-Break (1)
\ become a break; else (the window closes) Windows ends the program.
LABEL ON-CTRL
  EAX 4 [ESP] MOV,  EAX 1 CMPI,  A IF,  EAX EAX XOR,  4 RETN,  THEN,
  BREAK-OFF [] 128 MOVI,
  H-BREAK [] PUSH,  SetEvent API,
  EAX 1 MOVI,  4 RETN,

\ BREAK: each primitive after a break. It writes a new line and sets
\ OUT to 0. With a CATCH (HANDLER not 0) it runs -28 THROW ("user
\ interrupt"); without one (QUIT waits in REFILL, or BOOT waits for a
\ key) it starts QUIT again.
LABEL BREAK
  BREAK-OFF [] 0 MOVI,  H-BREAK [] PUSH,  ResetEvent API,
  0 PUSHI,  S-DONE PUSHI,  2 PUSHI,  CRLF PUSHI,  H-OUT [] PUSH,  WriteFile API,
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

\ ON-FAULT: LONG WINAPI (EXCEPTION_POINTERS *). An exception of the
\ thread of the VM goes on at FAULT: in the code of the program (an
\ access violation, $C0000005), or in a function that SYSCALL called.
\ For the second case, the registers of the VM come back from SC-ESP to
\ SC-EBP. The offsets in the CONTEXT: EDI $9C, ESI $A0, EBX $A4, EBP $B4,
\ EIP $B8, ESP $C4. Else Windows handles the exception.
LABEL ON-FAULT
  GetCurrentThreadId API,  EAX MAIN-THREAD [] CMP,  L2 JNZ,   \ another thread
  EAX 4 [ESP] MOV,  EAX 4 [EAX] MOV,               \ EAX: the CONTEXT
  SC-ESP [] 0 CMPI,  NZ IF,                        \ in SYSCALL
    ECX SC-ESP [] MOV,  $C4 [EAX] ECX MOV,  ECX SC-EDI [] MOV,  $9C [EAX] ECX MOV,
    ECX SC-ESI [] MOV,  $A0 [EAX] ECX MOV,  ECX SC-EBX [] MOV,  $A4 [EAX] ECX MOV,
    ECX SC-EBP [] MOV,  $B4 [EAX] ECX MOV,  SC-ESP [] 0 MOVI,  L3 JMP,
  THEN,
  ECX $B8 [EAX] MOV,
  ECX CODE-START CMPI,  L2 JB,  ECX CODE-END [] CMP,  L2 JAE,
LABEL L3
  $B8 [EAX] FAULT MOVI,
  EAX -1 MOVI,  4 RETN,                            \ go on, at FAULT
LABEL L2
  EAX EAX XOR,  4 RETN,                            \ not ours

\ ==== The console =======================================================

\ READER: DWORD WINAPI (LPVOID), the thread that reads the standard
\ input. No bytes: the end of a pipe or a file; a console only stops
\ reading for a moment (Ctrl-C). Ctrl-Z at the start of a line of the
\ console ends the input too.
LABEL READER
LABEL L1
  0 PUSHI,  R-DONE PUSHI,  IN-SIZE PUSHI,  IN-BUF PUSHI,  H-IN [] PUSH,  ReadFile API,
  EAX EAX TEST,  L3 JZ,
  EAX R-DONE [] MOV,  EAX EAX TEST,  L3 JZ,
  IN-CONSOLE [] 0 CMPI,  NZ IF,  IN-BUF [] BYTE 26 CMPI,  L4 JZ,  THEN,
  IN-POS [] 0 MOVI,  IN-COUNT [] EAX MOV,          \ the line for ?RX
  H-DATA [] PUSH,  SetEvent API,
  -1 PUSHI,  H-FREE [] PUSH,  WaitForSingleObject API,   \ until ?RX has taken it
  L1 JMP,
LABEL L3                                           \ no bytes
  IN-CONSOLE [] 0 CMPI,  L4 JZ,
  10 PUSHI,  Sleep API,  L1 JMP,
LABEL L4                                           \ the end of the input
  IN-EOF [] 1 MOVI,  H-DATA [] PUSH,  SetEvent API,
  EAX EAX XOR,  4 RETN,

\ At the end of the input (a pipe, a file, Ctrl-Z), ?RX ends the program
\ only when QUIT waits for a line: a return address on the return stack
\ points into REFILL, as in a bootstrap VM (doc/vm.md, section 9). Else
\ it answers "no key" and waits 1 ms.
LABEL IN-END
  EAX EBP MOV,  EBX 8 MOVI,
  BEGIN,
    EAX R0 CMPI,  B IF,
      EDX MEM [EAX] MOV,
      EDX XT-REFILL [] CMP,  A IF,  EDX REFILL-END [] CMP,  B IF,  0 PUSHI,  ExitProcess API,  THEN,  THEN,
    THEN,
    EAX 4 ADDI,  EBX DEC,
  Z UNTIL,
  EAX EAX XOR,  EAX DPUSH,
  1 PUSHI,  H-BREAK [] PUSH,  WaitForSingleObject API,  NEXT,

29 PRIM ( ?RX )
LABEL L1
  EAX IN-COUNT [] MOV,  EAX EAX TEST,  L2 JZ,       \ no line
  ECX IN-POS [] MOV,  ECX EAX CMP,  L3 JAE,         \ the line is taken
  EAX IN-BUF [] [ECX*1] BYTE MOVZX,  ECX INC,  IN-POS [] ECX MOV,
  EAX DPUSH,  EAX -1 MOVI,  EAX DPUSH,  NEXT,
LABEL L3
  IN-COUNT [] 0 MOVI,  H-FREE [] PUSH,  SetEvent API,   \ READER may read the next line
LABEL L2
  IN-EOF [] 0 CMPI,  IN-END JNZ,
  1 PUSHI,  H-DATA [] PUSH,  WaitForSingleObject API,   \ at most 1 ms
  EAX EAX TEST,  L1 JZ,                             \ a line came
  EAX EAX XOR,  EAX DPUSH,  NEXT,

30 PRIM ( TX! )
  EAX DPOP,  S-CHAR [] AL MOV,
  0 PUSHI,  S-DONE PUSHI,  1 PUSHI,  S-CHAR PUSHI,  H-OUT [] PUSH,  WriteFile API,  NEXT,

31 PRIM ( !IO )   NEXT,

\ ==== Time ==============================================================

LABEL NOW-MS   \ (CALL) EAX = the milliseconds since some point, modulo 2^32
  QPC PUSHI,  QueryPerformanceCounter API,
  ECX TPM [] MOV,
  EAX QPC 4 + [] MOV,  EDX EDX XOR,  ECX DIV,       \ EDX: the upper half modulo TPM
  EAX QPC [] MOV,  ECX DIV,  RET,                   \ so this DIV never overflows

\ WAIT-MS: (JMP) wait EBX ms, then NEXT. A wait of Windows can end up to
\ one tick of its timer (often 15.6 ms) too early. So WAIT-MS waits again
\ until the performance counter, the clock of MILLIS, says that the time
\ is over. A break (Ctrl-C) ends the wait.
LABEL WAIT-MS
  QPC PUSHI,  QueryPerformanceCounter API,
  EAX EBX MOV,  TPM [] MUL,                        \ EDX:EAX: the ticks of EBX ms
  EAX QPC [] ADD,  EDX QPC 4 + [] ADC,
  W-END [] EAX MOV,  W-END 4 + [] EDX MOV,
LABEL L1
  QPC PUSHI,  QueryPerformanceCounter API,
  EAX W-END [] MOV,  EDX W-END 4 + [] MOV,
  EAX QPC [] SUB,  EDX QPC 4 + [] SBB,  L2 JB,     \ the ticks still to wait
  ECX EAX MOV,  ECX EDX OR,  L2 JZ,
  TPM [] DIV,  EDX EDX TEST,  NZ IF,  EAX INC,  THEN,   \ in ms, rounded up (at most EBX)
  EAX PUSH,  H-BREAK [] PUSH,  WaitForSingleObject API,
  EAX EAX TEST,  L1 JNZ,                           \ 0: the break came
LABEL L2
  NEXT,

80 PRIM ( MILLIS )   NOW-MS CALL,  EAX START-MS [] SUB,  EAX DPUSH,  NEXT,
81 PRIM ( MS )   EBX DPOP,  WAIT-MS JMP,
82 PRIM ( US )   \ in whole milliseconds: (u + 999) / 1000
  EAX DPOP,  EDX EDX XOR,  EAX 999 ADDI,  EDX 0 ADCI,  ECX 1000 MOVI,  ECX DIV,
  EBX EAX MOV,  WAIT-MS JMP,

\ ==== HAL 83-88, 97, 99 ==================================================

83 PRIM ( BIS! )   EAX DPOP,  ECX DPOP,  MEM [EAX] CL OR,  NEXT,
84 PRIM ( BIC! )   EAX DPOP,  ECX DPOP,  CL NOT,  MEM [EAX] CL AND,  NEXT,
85 PRIM ( SAVE-IMAGE )   EAX -21 MOVI,  EAX DPUSH,  NEXT,   \ not here: build.fs makes programs
86 PRIM ( ERASE-IMAGE )  NEXT,
87 PRIM ( REBOOT )   \ ( f -- ): empty stacks, then BOOT (f true) or QUIT
  EAX DPOP,  EDI S0 MOVI,  EBP R0 MOVI,  MEM STATE + [] 0 MOVI,  MEM HANDLER + [] 0 MOVI,
  EAX EAX TEST,  Z IF,  EBX XT-QUIT [] MOV,  ELSE,  EBX XT-BOOT [] MOV,  THEN,
  DISPATCH JMP,
88 PRIM ( BYE )     0 PUSHI,  ExitProcess API,
99 PRIM ( BYE-N )   EAX DPOP,  EAX PUSH,  ExitProcess API,

\ ARG ( n -- a u ): the argument n of the command line (0: the program).
\ Spaces and tabs part the arguments; a text in double quotes is one
\ argument, without the quotes. The copy goes to S-ARGS + 256 n, at most
\ 255 bytes; the VM reaches it (VM address S-ARGS - MEM + 256 n).
97 PRIM ( ARG )
  EAX TOS MOV,  EAX 31 CMPI,  L8 JA,
  GetCommandLineA API,  EDX EAX MOV,
  ECX TOS MOV,  EBX ECX MOV,  EBX 8 SHL,  EBX S-ARGS ADDI,
LABEL L1                                           \ the next argument: blanks first
  AL 0 [EDX] MOV,
  AL 32 CMPI,  Z IF,  EDX INC,  L1 JMP,  THEN,
  AL 9 CMPI,  Z IF,  EDX INC,  L1 JMP,  THEN,
  AL AL TEST,  L8 JZ,                              \ the end of the line: none
  AH AH XOR,  ECX ECX TEST,  L4 JZ,                \ AH: inside quotes; ECX 0: argument n
LABEL L2                                           \ skip this argument
  AL 0 [EDX] MOV,  AL AL TEST,  L1 JZ,
  AL 34 CMPI,  Z IF,  AH 1 XORI,  EDX INC,  L2 JMP,  THEN,
  AH AH TEST,  Z IF,  AL 32 CMPI,  L7 JZ,  AL 9 CMPI,  L7 JZ,  THEN,
  EDX INC,  L2 JMP,
LABEL L7
  ECX DEC,  L1 JMP,
LABEL L4                                           \ copy argument n; ECX counts its bytes
  AL 0 [EDX] MOV,  AL AL TEST,  L6 JZ,
  AL 34 CMPI,  Z IF,  AH 1 XORI,  EDX INC,  L4 JMP,  THEN,
  AH AH TEST,  Z IF,  AL 32 CMPI,  L6 JZ,  AL 9 CMPI,  L6 JZ,  THEN,
  ECX 255 CMPI,  B IF,  0 [EBX] [ECX*1] AL MOV,  ECX INC,  THEN,
  EDX INC,  L4 JMP,
LABEL L6
  EBX MEM SUBI,  TOS EBX MOV,  ECX DPUSH,  NEXT,
LABEL L8
  TOS 0 MOVI,  EAX EAX XOR,  EAX DPUSH,  NEXT,

\ ==== Files: HAL 89-93, 98 (doc/vm.md, section 8) ======================
\ A fid is a handle of Windows. F-FID, F-POS and F-ERR hold the handle,
\ the bytes so far and the error while a call of Windows changes EAX,
\ ECX and EDX.

LABEL PATH   \ (CALL) EAX a, ECX u, EDX offset in S-PATH: the name there, then a byte 0
  ECX 255 CMPI,  A IF,  ECX 255 MOVI,  THEN,
  EDX S-PATH ADDI,  EAX MEM ADDI,
  BEGIN,  ECX ECX TEST,  NZ WHILE,
    BL 0 [EAX] MOV,  0 [EDX] BL MOV,  EAX INC,  EDX INC,  ECX DEC,
  REPEAT,
  0 [EDX] BYTE 0 MOVI,  RET,

LABEL ACCESS   \ fam 0 1 2: read, write, both (GENERIC_READ, GENERIC_WRITE)
  $80000000 T,  $40000000 T,  $C0000000 T,  $C0000000 T,

LABEL OPEN-ERROR   \ the error of CreateFileA: ( x x -- 0 ior )
  GetLastError API,  ECX -37 MOVI,
  EAX 2 CMPI,  Z IF,  ECX -38 MOVI,  THEN,         \ the file is not there
  EAX 3 CMPI,  Z IF,  ECX -38 MOVI,  THEN,         \ the folder is not there
  NOS 0 MOVI,  TOS ECX MOV,  NEXT,

LABEL OPEN-COMMON   \ ( a u fam -- fid ior ); EDX: 3 an old file, 2 a new file
  EDX PUSH,
  EAX DPOP,  EAX 3 ANDI,  ACCESS [] [EAX*4] PUSH,
  EAX NOS MOV,  ECX TOS MOV,  EDX EDX XOR,  PATH CALL,
  ECX POP,  EDX POP,
  0 PUSHI,  $80 PUSHI,  EDX PUSH,  0 PUSHI,  3 PUSHI,  ECX PUSH,  S-PATH PUSHI,
  CreateFileA API,                                 \ shared for read and write, normal attributes
  EAX -1 CMPI,  OPEN-ERROR JZ,
  NOS EAX MOV,  TOS 0 MOVI,  NEXT,

89 PRIM ( OPEN-FILE )     EDX 3 MOVI,  OPEN-COMMON JMP,   \ OPEN_EXISTING
90 PRIM ( CREATE-FILE )   EDX 2 MOVI,  OPEN-COMMON JMP,   \ CREATE_ALWAYS: new, or the old one empty

91 PRIM ( CLOSE-FILE )
  EAX TOS MOV,  EAX PUSH,  CloseHandle API,
  EAX EAX TEST,  EAX 0 MOVI,  Z IF,  EAX -37 MOVI,  THEN,  TOS EAX MOV,  NEXT,

92 PRIM ( READ-FILE )   \ ( a u fid -- u2 ior )
  EAX DPOP,  F-FID [] EAX MOV,  F-POS [] 0 MOVI,  F-ERR [] 0 MOVI,
LABEL L1
  EAX F-POS [] MOV,  EAX TOS CMP,  L5 JAE,         \ u bytes: done
  ECX TOS MOV,  ECX EAX SUB,  EDX NOS MOV,  EDX EAX ADD,  EDX MEM ADDI,
  0 PUSHI,  S-DONE PUSHI,  ECX PUSH,  EDX PUSH,  F-FID [] PUSH,  ReadFile API,
  EAX EAX TEST,  Z IF,  F-ERR [] -37 MOVI,  L5 JMP,  THEN,
  EAX S-DONE [] MOV,  EAX EAX TEST,  L5 JZ,       \ the end of the file (or no byte of a line)
  F-POS [] EAX ADD,  L1 JMP,
LABEL L5
  EAX F-POS [] MOV,  NOS EAX MOV,  EAX F-ERR [] MOV,  TOS EAX MOV,  NEXT,

93 PRIM ( WRITE-FILE )   \ ( a u fid -- ior )
  EAX DPOP,  F-FID [] EAX MOV,  F-POS [] 0 MOVI,
LABEL L1
  EAX F-POS [] MOV,  EAX TOS CMP,  L5 JAE,
  ECX TOS MOV,  ECX EAX SUB,  EDX NOS MOV,  EDX EAX ADD,  EDX MEM ADDI,
  0 PUSHI,  S-DONE PUSHI,  ECX PUSH,  EDX PUSH,  F-FID [] PUSH,  WriteFile API,
  EAX EAX TEST,  L4 JZ,
  EAX S-DONE [] MOV,  EAX EAX TEST,  L4 JZ,        \ nothing written: an error
  F-POS [] EAX ADD,  L1 JMP,
LABEL L4
  EDI 4 ADDI,  TOS -37 MOVI,  NEXT,
LABEL L5
  EDI 4 ADDI,  TOS 0 MOVI,  NEXT,

98 PRIM ( READ-LINE )   \ ( a u fid -- u2 f ior ): byte for byte up to LF
  EAX DPOP,  F-FID [] EAX MOV,  F-POS [] 0 MOVI,  F-ERR [] 0 MOVI,   \ F-ERR: a byte came
LABEL L1
  EAX F-POS [] MOV,  EAX TOS CMP,  L6 JAE,         \ the buffer is full
  0 PUSHI,  S-DONE PUSHI,  1 PUSHI,  S-CHAR PUSHI,  F-FID [] PUSH,  ReadFile API,
  EAX EAX TEST,  L7 JZ,                            \ an error
  S-DONE [] 0 CMPI,  L4 JZ,                        \ no byte: the end of the file
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
\ bits, no parity, no flow control. The name is that of the port (COM3);
\ SERIAL-OPEN puts \\.\ before it, so COM10 and above work too. A read
\ gives the bytes that are there, or none (COMMTIMEOUTS: MAXDWORD, 0, 0).
\ DTR and RTS are on after the open; SERIAL-LINES changes them, and a
\ new speed keeps them as they are.

LABEL SET-DCB   \ (CALL) the settings of the line F-FID with the speed F-BAUD; EAX 0: an error
  S-DCB [] 28 MOVI,  S-DCB PUSHI,  F-FID [] PUSH,  GetCommState API,
  EAX EAX TEST,  Z IF,  RET,  THEN,
  EAX F-BAUD [] MOV,  S-DCB 4 + [] EAX MOV,
  EAX 1 MOVI,                                      \ binary, no flow control, DTR and RTS:
  L-DTR [] 0 CMPI,  NZ IF,  EAX $10 ORI,  THEN,    \ on (DTR_CONTROL_ENABLE) or off
  L-RTS [] 0 CMPI,  NZ IF,  EAX $1000 ORI,  THEN,
  S-DCB 8 + [] EAX MOV,
  S-DCB 18 + [] BYTE 8 MOVI,  S-DCB 19 + [] BYTE 0 MOVI,  S-DCB 20 + [] BYTE 0 MOVI,   \ 8N1
  S-DCB PUSHI,  F-FID [] PUSH,  SetCommState API,  RET,

94 PRIM ( SERIAL-OPEN )   \ ( a u baud -- fid ior )
  EAX DPOP,  F-BAUD [] EAX MOV,
  S-PATH [] $5C2E5C5C MOVI,                        \ \\.\
  EAX NOS MOV,  ECX TOS MOV,  EDX 4 MOVI,  PATH CALL,
  0 PUSHI,  0 PUSHI,  3 PUSHI,  0 PUSHI,  0 PUSHI,  $C0000000 PUSHI,  S-PATH PUSHI,
  CreateFileA API,                                 \ read and write, not shared, OPEN_EXISTING
  EAX -1 CMPI,  OPEN-ERROR JZ,
  F-FID [] EAX MOV,  L-DTR [] 1 MOVI,  L-RTS [] 1 MOVI,  SET-DCB CALL,
  EAX EAX TEST,  NZ IF,
    S-TIMEOUTS [] -1 MOVI,  S-TIMEOUTS 4 + [] 0 MOVI,  S-TIMEOUTS 8 + [] 0 MOVI,
    S-TIMEOUTS 12 + [] 0 MOVI,  S-TIMEOUTS 16 + [] 0 MOVI,
    S-TIMEOUTS PUSHI,  F-FID [] PUSH,  SetCommTimeouts API,
  THEN,
  EAX EAX TEST,  Z IF,                             \ not a serial line: close it again
    F-FID [] PUSH,  CloseHandle API,  NOS 0 MOVI,  TOS -37 MOVI,  NEXT,
  THEN,
  EAX F-FID [] MOV,  NOS EAX MOV,  TOS 0 MOVI,  NEXT,

95 PRIM ( SERIAL-LINES )   \ ( dtr rts fid -- ior ): true = the line is active
  EAX DPOP,  F-FID [] EAX MOV,
  EAX DPOP,  L-RTS [] 0 MOVI,  EAX EAX TEST,  NZ IF,  L-RTS [] 1 MOVI,  THEN,
  EAX TOS MOV,  L-DTR [] 0 MOVI,  EAX EAX TEST,  NZ IF,  L-DTR [] 1 MOVI,  THEN,
  EAX 6 MOVI,  EAX L-DTR [] SUB,                   \ SETDTR 5, CLRDTR 6
  EAX PUSH,  F-FID [] PUSH,  EscapeCommFunction API,
  EAX EAX TEST,  Z IF,  TOS -37 MOVI,  NEXT,  THEN,
  EAX 4 MOVI,  EAX L-RTS [] SUB,                   \ SETRTS 3, CLRRTS 4
  EAX PUSH,  F-FID [] PUSH,  EscapeCommFunction API,
  EAX EAX TEST,  EAX 0 MOVI,  Z IF,  EAX -37 MOVI,  THEN,  TOS EAX MOV,  NEXT,

96 PRIM ( SERIAL-BAUD )   \ ( baud fid -- ior )
  EAX DPOP,  F-FID [] EAX MOV,  EAX TOS MOV,  F-BAUD [] EAX MOV,
  SET-DCB CALL,  EAX EAX TEST,  EAX 0 MOVI,  Z IF,  EAX -37 MOVI,  THEN,  TOS EAX MOV,  NEXT,

\ ==== Functions of Windows: HAL 100 and 101 ============================
\ The package WINDOWS (lib/windows/windows.fs) gives these words their
\ names. START puts the addresses of LoadLibraryA and GetProcAddress
\ into the free cells at the top of the memory of the VM (doc/vm.md,
\ section 3): with them, the package finds each function of each DLL.

100 PRIM ( SYSCALL )   \ ( x1 .. xn n a -- x ): call the function at a with n arguments
  SC-EDI [] EDI MOV,  SC-ESI [] ESI MOV,  SC-EBX [] EBX MOV,  SC-EBP [] EBP MOV,
  EAX DPOP,  ECX DPOP,
  EDX 0 [EDI] [ECX*4] LEA,  EDX S0 CMPI,  A IF,    \ fewer than n cells: underflow
    EDI SC-EDI [] MOV,  EAX -4 MOVI,  THROW-CODE JMP,
  THEN,
  SC-ESP [] ESP MOV,
  ECX ECX TEST,  NZ IF,                            \ xn first, x1 last: x1 is the first argument
    BEGIN,  MEM [EDI] PUSH,  EDI 4 ADDI,  ECX DEC,  Z UNTIL,
  THEN,
  EAX CALL-IND,
  ESP SC-ESP [] MOV,  SC-ESP [] 0 MOVI,            \ so the caller may remove the arguments too (cdecl)
  EAX DPUSH,  NEXT,

101 PRIM ( >CHIP )   \ ( a -- a' ): the address for Windows of the VM address a; any other address stays
  TOS MEM-SIZE CMPI,  B IF,  TOS MEM ADDI,  THEN,  NEXT,

\ ==== The start =========================================================
\ Windows calls START as the first instruction of the program. The
\ memory of the VM (the image) is in place already: the loader of
\ Windows copied it from the file.

LABEL START
  -10 PUSHI,  GetStdHandle API,  H-IN [] EAX MOV,       \ STD_INPUT_HANDLE
  -11 PUSHI,  GetStdHandle API,  H-OUT [] EAX MOV,      \ STD_OUTPUT_HANDLE
  S-DONE PUSHI,  H-IN [] PUSH,  GetConsoleMode API,          \ only a console has a mode:
  EAX EAX TEST,  NZ IF,  IN-CONSOLE [] 1 MOVI,  THEN,       \ NUL is a device, but no console
  0 PUSHI,  0 PUSHI,  1 PUSHI,  0 PUSHI,  CreateEventA API,  H-BREAK [] EAX MOV,   \ manual reset
  0 PUSHI,  0 PUSHI,  0 PUSHI,  0 PUSHI,  CreateEventA API,  H-DATA [] EAX MOV,
  0 PUSHI,  0 PUSHI,  0 PUSHI,  0 PUSHI,  CreateEventA API,  H-FREE [] EAX MOV,
  0 PUSHI,  0 PUSHI,  0 PUSHI,  READER PUSHI,  0 PUSHI,  0 PUSHI,  CreateThread API,
  EAX PUSH,  CloseHandle API,                            \ the thread runs on without its handle
  1 PUSHI,  ON-CTRL PUSHI,  SetConsoleCtrlHandler API,
  ON-FAULT PUSHI,  SetUnhandledExceptionFilter API,
  QPC PUSHI,  QueryPerformanceFrequency API,             \ ticks per second
  EAX QPC [] MOV,  EDX QPC 4 + [] MOV,  ECX 1000 MOVI,  ECX DIV,
  EAX EAX TEST,  Z IF,  EAX INC,  THEN,  TPM [] EAX MOV,
  NOW-MS CALL,  START-MS [] EAX MOV,
  GetCurrentThreadId API,  MAIN-THREAD [] EAX MOV,
  EAX LoadLibraryA [] MOV,  MEM DICT-END + [] EAX MOV,       \ for the package WINDOWS
  EAX GetProcAddress [] MOV,  MEM DICT-END + 4 + [] EAX MOV,
  EDI S0 MOVI,  EBP R0 MOVI,                             \ doc/vm.md, section 10
  MEM STATE + [] 0 MOVI,  MEM HANDLER + [] 0 MOVI,
  EBX XT-BOOT [] MOV,  DISPATCH JMP,
