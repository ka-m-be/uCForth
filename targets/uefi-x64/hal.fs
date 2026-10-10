\ hal.fs - the HAL of uCForth for a PC with UEFI (x86-64), without an operating system
\
\ The start code, the console (primitives 29-31) and the HAL (80-99) as
\ calls of the firmware (UEFI boot services). UEFI calls the program as
\ a function with the calling convention of Microsoft: the arguments in
\ RCX, RDX, R8, R9, a fifth one on the stack; the stack aligned to 16
\ bytes with 32 free bytes for the function. The firmware keeps RBX RBP
\ RDI RSI R12-R15, so the VM (kernels/x64.fs) stays. EFI, makes each
\ call: the table of functions in RAX, a fifth argument in R10.
\
\ Loaded by build.fs after kernels/x64.fs. build.fs gives MEM-AT (the
\ RVA of VM address 0 in the program), SCR (the VM address of the
\ scratch area after the memory of the VM), KCELLS and MEM-SIZE. UEFI
\ loads the program at an address of its own choice: the code finds
\ everything relative to itself.
\
\ The console: ConIn and ConOut of the firmware, the keyboard and the
\ screen (and a serial line, if the firmware has it as its console). A
\ timer event (ON-TIMER) reads the keys every 10 ms into a ring buffer;
\ ?RX takes them from there. ACCEPT shows the keys (ECHO). TX! writes
\ LF as CR LF.
\
\ Ctrl-C stops the running word (doc/vm.md, section 9: break): ON-TIMER
\ sets BREAK-OFF to 128 when it reads Ctrl-C, so the next dispatch runs
\ BREAK (kernels/x64.fs). This also stops a word that reads no key.
\
\ Time: MILLIS, MS and US count the clock of the processor (RDTSC); the
\ start measures its ticks for each millisecond with the function Stall
\ of the firmware.
\
\ Files: the file system of the disk that UEFI loaded the program from
\ (a USB stick with FAT): its root is the folder of the files. FREEZE
\ (SAVE-IMAGE) writes the memory of the VM back into the program file,
\ so the next start has the new image.

\ The offsets of the functions in the tables of UEFI:
48 CONSTANT ST-CONIN     64 CONSTANT ST-CONOUT    96 CONSTANT ST-BS         \ the system table
8 CONSTANT IN-READKEY    8 CONSTANT OUT-STRING   48 CONSTANT OUT-CLEAR      \ ConIn, ConOut
80 CONSTANT BS-CREATEEVENT   88 CONSTANT BS-SETTIMER   112 CONSTANT BS-CLOSEEVENT
152 CONSTANT BS-HANDLEPROTOCOL   248 CONSTANT BS-STALL   256 CONSTANT BS-WATCHDOG
24 CONSTANT LI-DEVICE    32 CONSTANT LI-PATH                                \ the loaded image
8 CONSTANT FS-OPENVOLUME                                                    \ the file system
8 CONSTANT F-OPEN   16 CONSTANT F-CLOSE   24 CONSTANT F-DELETE              \ a file
32 CONSTANT F-READ   40 CONSTANT F-WRITE   56 CONSTANT F-SETPOS

: EFI, ( n -- )   \ call the function at offset n of the table in RAX; RSI keeps RSP
  >R  RSI RSP MOV,  RSP -16 ANDI,  RSP 48 SUBI,  32 [RSP] R10 MOV,
  R> [RAX] CALL-IND,  RSP RSI MOV, ;
: TSC, ( -- )   RDTSC,  RDX 32 SHL,  RAX RDX OR, ;   \ RAX: the clock of the processor

\ The scratch area SCR, after the memory of the VM (KCELLS: SCR 0-15).
\ These are VM addresses: n [R15] is the cell, RDX n [R15] LEA, its address.
SCR 16 +    CONSTANT S-RSP        \ RSP of the firmware, for the return (64 bits)
SCR 24 +    CONSTANT S-IMAGE      \ the handle of this program
SCR 32 +    CONSTANT S-SYSTAB     \ the system table
SCR 40 +    CONSTANT S-CONIN      \ its ConIn, ConOut, boot services
SCR 48 +    CONSTANT S-CONOUT
SCR 56 +    CONSTANT S-BS
SCR 64 +    CONSTANT S-EVENT      \ the timer event of ON-TIMER
SCR 72 +    CONSTANT S-LI         \ the loaded image of this program
SCR 80 +    CONSTANT S-FS         \ the file system of its disk,
SCR 88 +    CONSTANT S-ROOT       \ and its root folder (0: none)
SCR 96 +    CONSTANT S-HANDLE     \ a file that Open gives
SCR 104 +   CONSTANT S-SIZE       \ the bytes for Read and Write (64 bits)
SCR 112 +   CONSTANT S-MODE       \ the mode for Open (64 bits)
SCR 120 +   CONSTANT S-TSC0       \ the clock at the start
SCR 128 +   CONSTANT S-TPMS       \ ticks of the clock for each millisecond
SCR 136 +   CONSTANT S-STATUS     \ the status for the firmware at the end
SCR 144 +   CONSTANT S-KEY        \ a key: the scan code, the character (16 bits each)
SCR 148 +   CONSTANT S-HEAD       \ the ring buffer of the keys: the next place for ON-TIMER,
SCR 152 +   CONSTANT S-TAIL       \ the next key for ?RX
SCR 156 +   CONSTANT F-SLOT       \ files: the place in S-FILES,
SCR 160 +   CONSTANT F-POS        \ the bytes so far,
SCR 164 +   CONSTANT F-ERR        \ an error code, or: a byte came (READ-LINE)
SCR 168 +   CONSTANT F-NEW        \ not 0: CREATE-FILE removes the old file first
SCR 172 +   CONSTANT S-CHAR       \ one byte for READ-LINE
SCR 176 +   CONSTANT F-H          \ the file of READ-LINE (64 bits)
SCR 184 +   CONSTANT S-OUT        \ a text for OutputString: up to 3 characters of 16 bits
SCR 192 +   CONSTANT S-FILES      \ the open files: 16 cells of 64 bits; fid = 10 + the place
SCR 512 +   CONSTANT S-WPATH      \ a file name in 16-bit characters, 0 at the end (255 + 1)
SCR 1024 +  CONSTANT S-SELF       \ the name of the program file, the same way
SCR 2048 +  CONSTANT S-RING       \ the ring buffer of the keys, 1024 bytes
$3FF CONSTANT RING-MASK

\ ==== Cells that build.fs fills, and the GUIDs ===========================

LABEL XT-REFILL    0 T,           \ the xt of REFILL and the end of its body
LABEL REFILL-END   0 T,
LABEL OUT-ADDR     0 T,           \ the VM address of OUT
LABEL ECHO-ADDR    0 T,           \ the VM address of ECHO
LABEL DATA-POS     0 T,           \ the place of VM address 0 in the program file

LABEL LOADED-IMAGE-GUID   \ 5B1B31A1-9562-11D2-8E3F-00A0C969723B
  $5B1B31A1 T,  $9562 TW,  $11D2 TW,  $8E TC, $3F TC, $00 TC, $A0 TC, $C9 TC, $69 TC, $72 TC, $3B TC,
LABEL FILE-SYSTEM-GUID    \ 964E5B22-6459-11D2-8E39-00A0C969723B
  $964E5B22 T,  $6459 TW,  $11D2 TW,  $8E TC, $39 TC, $00 TC, $A0 TC, $C9 TC, $69 TC, $72 TC, $3B TC,

\ ==== The console =======================================================

\ ON-TIMER: void notify(EFI_EVENT, void *), called by the firmware
\ every 10 ms between two instructions of the program (or of the
\ firmware). It reads each key that is there into the ring buffer.
\ Ctrl-C is the break: the keys before it are gone. ESC is 27; a key
\ without a character (F1, the arrows) is none. A full buffer loses the
\ key. It finds the memory of the VM relative to itself (RBX), not in
\ R15, and keeps RBX and RSI for the firmware.
LABEL ON-TIMER
  RBX PUSH,  RSI PUSH,  RBX MEM-AT [RIP] LEA,
LABEL L1
  RAX S-CONIN [RBX] MOV,  RCX RAX MOV,  RDX S-KEY [RBX] LEA,  IN-READKEY EFI,
  RAX RAX TEST,  L9 JNZ,                                  \ no key
  EAX S-KEY [RBX] MOV,  ECX EAX MOV,  EAX 16 SHR,  ECX $FFFF ANDI,   \ EAX the character, ECX the scan code
  EAX 3 CMPI,  Z IF,
    BREAK-OFF [RBX] 128 MOVI,  EAX S-TAIL [RBX] MOV,  S-HEAD [RBX] EAX MOV,  L1 JMP,
  THEN,
  EAX EAX TEST,  Z IF,  ECX $17 CMPI,  L1 JNZ,  EAX 27 MOVI,  THEN,
  EAX 255 CMPI,  L1 JA,
  EDX S-HEAD [RBX] MOV,  ECX EDX MOV,  ECX INC,  ECX RING-MASK ANDI,
  ECX S-TAIL [RBX] CMP,  L1 JZ,                           \ the buffer is full
  S-RING [RBX] [RDX*1] AL MOV,  S-HEAD [RBX] ECX MOV,  L1 JMP,
LABEL L9
  RSI POP,  RBX POP,  RET,

LABEL PUTC   \ (CALL) AL: the byte to the console; LF as CR LF, the byte 0 not
  AL AL TEST,  Z IF,  RET,  THEN,
  AL 10 CMPI,  Z IF,  S-OUT [R15] $000A000D MOVI,  S-OUT 4 + [R15] 0 MOVI,
  ELSE,  EAX $FF ANDI,  S-OUT [R15] EAX MOV,  THEN,
  RAX S-CONOUT [R15] MOV,  RCX RAX MOV,  RDX S-OUT [R15] LEA,  OUT-STRING EFI,  RET,

\ BREAK: each primitive after a break. It writes a new line and sets
\ OUT to 0. With a CATCH (HANDLER not 0) it runs -28 THROW ("user
\ interrupt"); without one (QUIT waits in REFILL, or BOOT waits for a
\ key) it starts QUIT again.
LABEL BREAK
  BREAK-OFF [R15] 0 MOVI,
  EAX 10 MOVI,  PUTC CALL,
  EAX OUT-ADDR [RIP] MOV,  0 [R15] [RAX*1] 0 MOVI,
  EAX HANDLER [R15] MOV,  EAX EAX TEST,
  Z IF,
    R13D S0 MOVI,  EBP R0 MOVI,  STATE [R15] 0 MOVI,
    EBX XT-QUIT [RIP] MOV,  DISPATCH JMP,
  THEN,
  EAX -28 MOVI,  THROW-CODE JMP,

29 PRIM ( ?RX )   \ a key from the ring buffer, or none; no wait
  EAX S-TAIL [R15] MOV,  EAX S-HEAD [R15] CMP,  Z IF,  EAX EAX XOR,  EAX DPUSH,  NEXT,  THEN,
  ECX S-RING [R15] [RAX*1] BYTE MOVZX,  EAX INC,  EAX RING-MASK ANDI,  S-TAIL [R15] EAX MOV,
  EAX ECHO-ADDR [RIP] MOV,  0 [R15] [RAX*1] -1 MOVI,     \ ACCEPT shows the key
  ECX DPUSH,  EAX -1 MOVI,  EAX DPUSH,  NEXT,

30 PRIM ( TX! )   EAX DPOP,  PUTC CALL,  NEXT,
31 PRIM ( !IO )   NEXT,

\ ==== Time ==============================================================

80 PRIM ( MILLIS )
  TSC,  RAX S-TSC0 [R15] SUB,  EDX EDX XOR,  S-TPMS [R15] QWORD DIV,  EAX DPUSH,  NEXT,
81 PRIM ( MS )   \ until the clock counted u * the ticks for 1 ms; a break ends the wait
  EAX DPOP,  S-TPMS [R15] QWORD MUL,  RCX RAX MOV,  TSC,  RCX RAX ADD,
  BEGIN,
    BREAK-OFF [R15] 0 CMPI,  NZ IF,  NEXT,  THEN,
    PAUSE,  TSC,  RAX RCX CMP,
  AE UNTIL,
  NEXT,
82 PRIM ( US )
  EAX DPOP,  S-TPMS [R15] QWORD MUL,  ECX 1000 MOVI,  RCX DIV,  RCX RAX MOV,  TSC,  RCX RAX ADD,
  BEGIN,  PAUSE,  TSC,  RAX RCX CMP,  AE UNTIL,
  NEXT,

\ ==== HAL 83-88, 97, 99 ==================================================

83 PRIM ( BIS! )   EAX DPOP,  ECX DPOP,  0 [R15] [RAX*1] CL OR,  NEXT,
84 PRIM ( BIC! )   EAX DPOP,  ECX DPOP,  CL NOT,  0 [R15] [RAX*1] CL AND,  NEXT,
86 PRIM ( ERASE-IMAGE )  NEXT,
87 PRIM ( REBOOT )   \ ( f -- ): empty stacks, then BOOT (f true) or QUIT
  EAX DPOP,  R13D S0 MOVI,  EBP R0 MOVI,  STATE [R15] 0 MOVI,  HANDLER [R15] 0 MOVI,
  EAX EAX TEST,  Z IF,  EBX XT-QUIT [RIP] MOV,  ELSE,  EBX XT-BOOT [RIP] MOV,  THEN,
  DISPATCH JMP,

\ The end: back to the firmware (the boot menu, or the UEFI shell) with
\ the status in EAX. The timer stops first: the firmware may give the
\ memory of the program to another one.
LABEL EXIT
  S-STATUS [R15] RAX MOV,
  RAX S-BS [R15] MOV,  RCX S-EVENT [R15] MOV,  BS-CLOSEEVENT EFI,
  RAX S-STATUS [R15] MOV,  RSP S-RSP [R15] MOV,
  RDI POP,  RSI POP,  R15 POP,  R14 POP,  R13 POP,  R12 POP,  RBP POP,  RBX POP,  RET,
88 PRIM ( BYE )     EAX EAX XOR,  EXIT JMP,
99 PRIM ( BYE-N )   EAX DPOP,  EXIT JMP,
97 PRIM ( ARG )     TOS 0 MOVI,  EAX EAX XOR,  EAX DPUSH,  NEXT,   \ ( n -- a 0 ): no command line

\ ==== Files: HAL 89-93, 98 (doc/vm.md, section 8) ======================
\ A file of UEFI is a table of functions (EFI_FILE_PROTOCOL); its address
\ has 64 bits. So a fid is 10 + its place in S-FILES. A name a u is
\ relative to the root of the disk; / and \ both separate the folders.

LABEL WPATH   \ (CALL) EAX a, ECX u: the name to S-WPATH in 16-bit characters, then 0
  ECX 255 CMPI,  A IF,  ECX 255 MOVI,  THEN,
  RDX 0 [R15] [RAX*1] LEA,  RDI S-WPATH [R15] LEA,
  BEGIN,  ECX ECX TEST,  NZ WHILE,
    EAX 0 [RDX] BYTE MOVZX,  EAX '/' CMPI,  Z IF,  EAX '\' MOVI,  THEN,
    0 [RDI] AL MOV,  1 [RDI] BYTE 0 MOVI,  RDX INC,  RDI 2 ADDI,  ECX DEC,
  REPEAT,
  0 [RDI] BYTE 0 MOVI,  1 [RDI] BYTE 0 MOVI,  RET,

LABEL FID>H   \ (CALL) EAX fid: EAX its place, RCX the file (0: not an open file)
  EAX 10 SUBI,  EAX 16 CMPI,  AE IF,  ECX ECX XOR,  RET,  THEN,
  RCX S-FILES [R15] [RAX*8] MOV,  RET,

LABEL OPEN   \ (CALL) Open(root, S-HANDLE, R8 the name, R9 the mode, 0): RAX the status
  RAX S-ROOT [R15] MOV,  RCX RAX MOV,  RDX S-HANDLE [R15] LEA,  R10D R10D XOR,  F-OPEN EFI,  RET,

LABEL OPEN-COMMON   \ ( a u -- fid ior ); S-MODE: the mode; F-NEW: remove an old file first
  ECX ECX XOR,
  BEGIN,  S-FILES [R15] [RCX*8] QWORD 0 CMPI,  NZ WHILE,   \ a free place
    ECX INC,  ECX 16 CMPI,  L8 JAE,                       \ none: too many files
  REPEAT,
  F-SLOT [R15] ECX MOV,
  S-ROOT [R15] QWORD 0 CMPI,  L8 JZ,                      \ no file system
  EAX NOS MOV,  ECX TOS MOV,  WPATH CALL,
  F-NEW [R15] 0 CMPI,  NZ IF,                             \ CREATE-FILE: remove the old file
    R8 S-WPATH [R15] LEA,  R9D 3 MOVI,  OPEN CALL,  RAX RAX TEST,
    Z IF,  RAX S-HANDLE [R15] MOV,  RCX RAX MOV,  F-DELETE EFI,  THEN,
  THEN,
  R8 S-WPATH [R15] LEA,  R9 S-MODE [R15] MOV,  OPEN CALL,
  RAX RAX TEST,  NZ IF,
    ECX -37 MOVI,  EAX 14 CMPI,  Z IF,  ECX -38 MOVI,  THEN,   \ EFI_NOT_FOUND
    NOS 0 MOVI,  TOS ECX MOV,  NEXT,
  THEN,
  ECX F-SLOT [R15] MOV,  RAX S-HANDLE [R15] MOV,  S-FILES [R15] [RCX*8] RAX MOV,
  ECX 10 ADDI,  NOS ECX MOV,  TOS 0 MOVI,  NEXT,
LABEL L8
  NOS 0 MOVI,  TOS -37 MOVI,  NEXT,

89 PRIM ( OPEN-FILE )   \ ( a u fam -- fid ior ): read (fam 0), or read and write
  EAX DPOP,  EAX 3 ANDI,  ECX 1 MOVI,  EAX EAX TEST,  NZ IF,  ECX 3 MOVI,  THEN,
  S-MODE [R15] RCX MOV,  F-NEW [R15] 0 MOVI,  OPEN-COMMON JMP,
90 PRIM ( CREATE-FILE )   \ read, write, create
  R13D 4 ADDI,  ECX 1 MOVI,  RCX 63 SHL,  RCX 3 ORI,
  S-MODE [R15] RCX MOV,  F-NEW [R15] 1 MOVI,  OPEN-COMMON JMP,

91 PRIM ( CLOSE-FILE )
  EAX TOS MOV,  FID>H CALL,  RCX RCX TEST,  Z IF,  TOS -37 MOVI,  NEXT,  THEN,
  S-FILES [R15] [RAX*8] QWORD 0 MOVI,
  RAX RCX MOV,  F-CLOSE EFI,  TOS 0 MOVI,  NEXT,

\ READ-FILE and WRITE-FILE: Read and Write of the file, u bytes at a.
\ The firmware does not check the address: an area outside the memory of
\ the VM is THROW -9 first (IN-VM, of the kernel). RDI holds the file:
\ the firmware keeps it.
92 PRIM ( READ-FILE )   \ ( a u fid -- u2 ior )
  EAX DPOP,  FID>H CALL,  RCX RCX TEST,  Z IF,  NOS 0 MOVI,  TOS -37 MOVI,  NEXT,  THEN,
  RDI RCX MOV,  EAX NOS MOV,  ECX TOS MOV,  EAX IN-VM,
  S-SIZE [R15] RCX MOV,  R8 0 [R15] [RAX*1] LEA,
  RAX RDI MOV,  RCX RDI MOV,  RDX S-SIZE [R15] LEA,  F-READ EFI,
  RAX RAX TEST,  NZ IF,  NOS 0 MOVI,  TOS -37 MOVI,  NEXT,  THEN,
  EAX S-SIZE [R15] MOV,  NOS EAX MOV,  TOS 0 MOVI,  NEXT,

93 PRIM ( WRITE-FILE )   \ ( a u fid -- ior )
  EAX DPOP,  FID>H CALL,  RCX RCX TEST,  Z IF,  R13D 4 ADDI,  TOS -37 MOVI,  NEXT,  THEN,
  RDI RCX MOV,  EAX NOS MOV,  ECX TOS MOV,  EAX IN-VM,
  S-SIZE [R15] RCX MOV,  R8 0 [R15] [RAX*1] LEA,
  RAX RDI MOV,  RCX RDI MOV,  RDX S-SIZE [R15] LEA,  F-WRITE EFI,
  R13D 4 ADDI,  TOS 0 MOVI,  RAX RAX TEST,  NZ IF,  TOS -37 MOVI,  THEN,  NEXT,

98 PRIM ( READ-LINE )   \ ( a u fid -- u2 f ior ): byte for byte up to LF
  EAX DPOP,  F-POS [R15] 0 MOVI,  F-ERR [R15] 0 MOVI,       \ F-ERR: a byte came
  FID>H CALL,  RCX RCX TEST,  L7 JZ,
  F-H [R15] RCX MOV,  EAX NOS MOV,  ECX TOS MOV,  EAX IN-VM,
LABEL L1
  EAX F-POS [R15] MOV,  EAX TOS CMP,  L6 JAE,       \ the buffer is full
  S-SIZE [R15] QWORD 1 MOVI,
  RAX F-H [R15] MOV,  RCX RAX MOV,  RDX S-SIZE [R15] LEA,  R8 S-CHAR [R15] LEA,  F-READ EFI,
  RAX RAX TEST,  L7 JNZ,                           \ an error
  S-SIZE [R15] QWORD 0 CMPI,  L4 JZ,               \ no byte: the end of the file
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

\ SAVE-IMAGE (FREEZE): the memory of the VM, 256 KB, into the program
\ file at DATA-POS, where UEFI loads it from at the next start.
85 PRIM ( SAVE-IMAGE )   \ ( -- ior )
  S-ROOT [R15] QWORD 0 CMPI,  L8 JZ,                    \ no file system
  S-SELF [R15] 0 CMPI,  L8 JZ,                          \ the name of the file is not known
  R8 S-SELF [R15] LEA,  R9D 3 MOVI,  OPEN CALL,  RAX RAX TEST,  L8 JNZ,
  RDI S-HANDLE [R15] MOV,
  RAX RDI MOV,  RCX RDI MOV,  EDX DATA-POS [RIP] MOV,  F-SETPOS EFI,
  RAX RAX TEST,  L7 JNZ,
  S-SIZE [R15] QWORD MEM-SIZE MOVI,
  RAX RDI MOV,  RCX RDI MOV,  RDX S-SIZE [R15] LEA,  R8 R15 MOV,  F-WRITE EFI,
LABEL L7                                                \ close the file; RAX the status
  S-STATUS [R15] RAX MOV,
  RAX RDI MOV,  RCX RDI MOV,  F-CLOSE EFI,
  EAX EAX XOR,  S-STATUS [R15] QWORD 0 CMPI,  NZ IF,  EAX -37 MOVI,  THEN,  EAX DPUSH,  NEXT,
LABEL L8
  EAX -37 MOVI,  EAX DPUSH,  NEXT,

\ ==== The start =========================================================
\ The firmware calls START as efi_main(ImageHandle, SystemTable). The
\ memory of the VM (the image) is in place already: UEFI loaded it from
\ the file. EXIT goes back with the registers of the firmware.

LABEL START
  RBX PUSH,  RBP PUSH,  R12 PUSH,  R13 PUSH,  R14 PUSH,  R15 PUSH,  RSI PUSH,  RDI PUSH,
  R15 MEM-AT [RIP] LEA,  R14 OPTAB [RIP] LEA,
  S-RSP [R15] RSP MOV,  S-IMAGE [R15] RCX MOV,  S-SYSTAB [R15] RDX MOV,
  RAX ST-CONIN [RDX] MOV,  S-CONIN [R15] RAX MOV,
  RAX ST-CONOUT [RDX] MOV,  S-CONOUT [R15] RAX MOV,
  RAX ST-BS [RDX] MOV,  S-BS [R15] RAX MOV,
  ECX ECX XOR,  EDX EDX XOR,  R8D R8D XOR,  R9D R9D XOR,  BS-WATCHDOG EFI,   \ no watchdog
  RAX S-CONOUT [R15] MOV,  RCX RAX MOV,  OUT-CLEAR EFI,  \ an empty screen
  TSC,  S-TSC0 [R15] RAX MOV,                           \ the clock: its ticks in 10 ms
  RAX S-BS [R15] MOV,  ECX 10000 MOVI,  BS-STALL EFI,
  TSC,  RAX S-TSC0 [R15] SUB,  EDX EDX XOR,  ECX 10 MOVI,  RCX DIV,
  RAX RAX TEST,  Z IF,  EAX 1 MOVI,  THEN,  S-TPMS [R15] RAX MOV,
  TSC,  S-TSC0 [R15] RAX MOV,
  S-HEAD [R15] 0 MOVI,  S-TAIL [R15] 0 MOVI,  BREAK-OFF [R15] 0 MOVI,
  RAX S-BS [R15] MOV,  ECX $80000200 MOVI,  EDX 8 MOVI,  R8 ON-TIMER [RIP] LEA,   \ EVT_TIMER EVT_NOTIFY_SIGNAL,
  R9D R9D XOR,  R10 S-EVENT [R15] LEA,  BS-CREATEEVENT EFI,                        \ TPL_CALLBACK
  RAX S-BS [R15] MOV,  RCX S-EVENT [R15] MOV,  EDX 1 MOVI,  R8D 100000 MOVI,  BS-SETTIMER EFI,   \ every 10 ms
  S-ROOT [R15] QWORD 0 MOVI,  S-SELF [R15] 0 MOVI,      \ the files: the disk of this program
  RAX S-BS [R15] MOV,  RCX S-IMAGE [R15] MOV,  RDX LOADED-IMAGE-GUID [RIP] LEA,  R8 S-LI [R15] LEA,
  BS-HANDLEPROTOCOL EFI,  RAX RAX TEST,  L5 JNZ,
  RAX S-LI [R15] MOV,  RCX LI-DEVICE [RAX] MOV,  RDX FILE-SYSTEM-GUID [RIP] LEA,  R8 S-FS [R15] LEA,
  RAX S-BS [R15] MOV,  BS-HANDLEPROTOCOL EFI,  RAX RAX TEST,  L5 JNZ,
  RAX S-FS [R15] MOV,  RCX RAX MOV,  RDX S-ROOT [R15] LEA,  FS-OPENVOLUME EFI,
  RAX RAX TEST,  NZ IF,  S-ROOT [R15] QWORD 0 MOVI,  THEN,
  \ the name of the program file: the nodes "file path" (type 4, subtype 4) of its device path
  RAX S-LI [R15] MOV,  RDX LI-PATH [RAX] MOV,  RDI S-SELF [R15] LEA,  R9D 500 MOVI,   \ R9D: bytes left
LABEL L1
  RDX RDX TEST,  L4 JZ,
  EAX 0 [RDX] MOV,  AL $7F CMPI,  L4 JZ,               \ the end of the path
  R8D EAX MOV,  R8D 16 SHR,  R8D 6 CMPI,  L4 JB,       \ the length of the node
  EAX $FFFF ANDI,  EAX $0404 CMPI,  L2 JNZ,
  ECX R8D MOV,  ECX 4 SUBI,  ECX R9D CMP,  L4 JA,      \ too long
  R9D ECX SUB,  RSI 4 [RDX] LEA,  REP, MOVSB,
  EAX -2 [RDI] BYTE MOVZX,  ECX -1 [RDI] BYTE MOVZX,  EAX ECX OR,  Z IF,  RDI 2 SUBI,  THEN,   \ without its 0
LABEL L2
  RDX R8 ADD,  L1 JMP,
LABEL L4
  0 [RDI] BYTE 0 MOVI,  1 [RDI] BYTE 0 MOVI,
LABEL L5
  R13D S0 MOVI,  EBP R0 MOVI,                            \ doc/vm.md, section 10
  STATE [R15] 0 MOVI,  HANDLER [R15] 0 MOVI,
  EBX XT-BOOT [RIP] MOV,  DISPATCH JMP,
