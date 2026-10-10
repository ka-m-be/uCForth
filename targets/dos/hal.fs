\ hal.fs - the HAL of uCForth for MS-DOS: start, console, files, images
\
\ UCF.COM runs on each PC with MS-DOS 2.0 or newer, FreeDOS, DR-DOS,
\ and in the DOS box of 32-bit Windows. It needs 128 KB of free memory:
\ its own segment of 64 KB (the code, buffers) and a second segment of
\ 64 KB, the memory of the VM. Loaded by build.fs after kernels/8086.fs.
\
\ UCF.COM is a bootstrap VM without Genesis and a target at once:
\ - with an image in the file (build.fs puts it after the code), it
\   starts this image;
\ - without it, it loads an image file: UCF -image SEED.HEX BASE.FS ...
\ The files of the command line load first, line by line through ?RX,
\ then the keyboard or a file with "<" (doc/vm.md, section 9). With the
\ seed (its start word is QUIT), the first file must be forth/base.fs:
\ after it the kernel completes the base (doc/genesis.md, section 6),
\ and "-dump FILE" writes the base image. FREEZE writes UCF.HEX.
\
\ The calls of DOS: INT 21h with the function in AH: 3Ch create a file,
\ 3Dh open, 3Eh close, 3Fh read, 40h write, 42h move the file pointer,
\ 44h IOCTL (is a handle a device?), 0Bh is a key there, 25h and 35h
\ set and get an interrupt vector, 4Ch end the program. The clock is
\ the tick counter of the BIOS at 0040:006C (18.2 ticks per second).
\
\ Registers: as in kernels/8086.fs. The subroutines here (CALL, RET)
\ keep SI BP DS ES, unless the comment says other. Variables of the
\ HAL are in the code segment: "CS: AX IN-POS [] MOV," reads one.

\ ==== Memory of the code segment ========================================
\ DOS gives a .COM program its segment of 64 KB. The code starts at
\ $100 (before it: the PSP of DOS, with the command line at $80). The
\ image follows the code (IMAGE-AT); buffers lie at the end.

$F000 CONSTANT LINE-BUF      \ the input line for ?RX: the text and LF
$F200 CONSTANT OUT-BUF       \ the output line of TX!, 256 bytes
$F300 CONSTANT NAME-BUF      \ a file name with a byte 0 at its end
$F400 CONSTANT REC-BUF       \ a line of an image file while it is written
$F480 CONSTANT NUM-BUF       \ the digits of a number
$F490 CONSTANT ARG-LIST      \ the files of the command line: 16 addresses
$F4B0 CONSTANT FID-TAB       \ fid 10 to 29: the handle of DOS + 1, or 0 (free)
20 CONSTANT #FIDS
$F500 CONSTANT OPTAB0        \ a copy of the table of the primitives, for BREAK
16 CONSTANT #ARGS
$FFF0 CONSTANT REGS          \ SYSCALL: AX BX CX DX SI DI flags ES, in the VM
DP CONSTANT SYS-START        \ the system variables start with DP at $0100
WL-FORTH CONSTANT DATA-START \ the RAM data start with the wordlist FORTH at $0200

: DOS, ( ah -- )   AH SWAP MOVI,  $21 INT, ;     \ a call of DOS
: C$, ( a u -- )   DUP TC, TS, ;                 \ a counted text in the code

\ ==== Variables =========================================================

LABEL VMSEG        0 T,         \ the segment of the VM
LABEL OLD1B        0 T, 0 T,    \ the vector of INT 1Bh before the start: offset, segment
LABEL START-TICKS  0 T,         \ the clock at the start
LABEL TTY          0 T,         \ not 0: the input is a device (the keyboard)
LABEL WAITING      0 T,         \ not 0: DOS reads a line of the keyboard now
LABEL BRK-FLAG     0 T,         \ not 0: Ctrl-Break came; BREAK runs next
LABEL IN-POS       0 T,         \ the next character of LINE-BUF for ?RX
LABEL IN-LEN       0 T,         \ the characters in LINE-BUF, with the LF
LABEL OUT-LEN      0 T,         \ the characters in OUT-BUF
LABEL ERRSEEN      0 T,         \ not 0: an output line had "error:"
LABEL NARGS        0 T,         \ the files of the command line
LABEL ARGI         0 T,         \ the next of them to load
LABEL CUR-FID      0 T,         \ the file that loads now, or 0
LABEL CUR-NAME     0 T,         \ its name
LABEL CUR-LN       0 T,         \ its line number
LABEL SEEDED       0 T,         \ not 0: the image is a seed, the first file is base.fs
LABEL IMG-NAME     0 T,         \ -image FILE, or 0
LABEL DUMP-NAME    0 T,         \ -dump FILE, or 0
LABEL IMG-FID      0 T,         \ the image file that is read or written
LABEL IMG-BOOT     0 T,         \ WRITE-IMAGE: the start word
LABEL XT-REFILL    0 T,         \ the body of REFILL: QUIT waits for a line there
LABEL REFILL-END   0 T,
LABEL OUT-A        0 T,         \ the address of OUT, the column of the output
LABEL FAM          0 T,         \ OPEN-FILE: the access
LABEL RL-BUF       0 T,         \ RDLINE: the buffer, its size, the bytes read
LABEL RL-MAX       0 T,
LABEL RL-GOT       0 T,
LABEL RL-PEEK      0 T,         \ RDLINE: the two bytes after a full buffer
LABEL RL-LF        0 T,         \ RDLINE: not 0 if the line ended with LF
LABEL CUR-LF       0 T,         \ the file so far ended with LF: one more line comes
LABEL SC-AX        0 T,         \ SYSCALL: AX, the flags and ES after the call
LABEL SC-FL        0 T,
LABEL SC-ES        0 T,
LABEL IMAGE-LEN    0 T,         \ build.fs: the bytes of the image after the code; 0: none
LABEL PRIMS$       32 TDP +!    \ the primitives of the image, as text (counted)

\ ==== Texts =============================================================

LABEL CRLF$        13 TC,  10 TC,
LABEL ERROR$       S" error:" TS,
LABEL SPACES$      S"   " TS,
LABEL IMAGE-OPT$   S" -image" C$,
LABEL DUMP-OPT$    S" -dump" C$,
LABEL UCF-HEX$     S" UCF.HEX" TS,  0 TC,
LABEL REFILL$      S" REFILL" C$,
LABEL OUT$         S" OUT" C$,
LABEL BOOT$        S" BOOT" C$,
LABEL BITS$        S" bits=" C$,
LABEL BOOT=$       S" boot=" C$,
LABEL QUIT=$       S" quit=" C$,
LABEL THROW=$      S" throw=" C$,
LABEL PRIMS=$      S" prims=" C$,
LABEL HEADER$      S" ; ucf image bits=16 boot=" C$,
LABEL QUIT-H$      S"  quit=" C$,
LABEL THROW-H$     S"  throw=" C$,
LABEL PRIMS-H$     S"  prims=" C$,
LABEL END-REC$     S" :00000001FF" TS,  10 TC,
LABEL UCF$         S" ucf: " C$,
LABEL CANNOT$      S" cannot read " C$,
LABEL CANNOT-W$    S" cannot write " C$,
LABEL STOPPED$     S" : loading stopped" C$,
LABEL SAVED$       S" saved: " C$,
LABEL MEMORY$      S" not enough memory: UCF needs 128 KB" C$,
LABEL USE$         S" use: UCF [-image FILE] [-dump FILE] [FILE ...]" C$,
LABEL NOT-IMAGE$   S"  is not an image of uCForth for 16-bit cells" C$,
LABEL NEEDS$       S"  needs primitive " C$,
LABEL CHECKSUM$    S" : wrong record" C$,
LABEL BANNER$      S" uCForth - DOS bootstrap VM - 16 bit - " C$,
LABEL WORDS$       S"  words in FORTH" C$,
LABEL IMAGE$       S"  - image " C$,
LABEL BUILT$       S"  - built in " C$,
LABEL MS$          S"  ms" C$,

\ ==== Output ============================================================

LABEL WRITE-CS   \ write CX bytes at CS:DX to the output (handle 1); uses AX BX
  DS PUSH,  CS PUSH,  DS POP,  BX 1 MOVI,  $40 DOS,  DS POP,  RET,
LABEL NEWLINE    \ CR LF to the output; uses AX BX CX DX
  DX CRLF$ MOVI,  CX 2 MOVI,  WRITE-CS JMP,
LABEL PUT$       \ write the counted text at CS:DX; uses AX BX CX DX
  BX DX MOV,  CS: CL 0 [BX] MOV,  CH CH XOR,  DX INC,  WRITE-CS JMP,
LABEL PUTZ       \ write the text at CS:DX up to its byte 0; uses AX BX CX DX
  BX DX MOV,  CX CX XOR,
  BEGIN,  CS: 0 [BX] BYTE 0 CMPI,  NZ WHILE,  BX INC,  CX INC,  REPEAT,
  WRITE-CS JMP,
LABEL >DEC       \ AX as decimal digits to ES:DI, DI goes after them; uses AX BX CX DX
  CX CX XOR,  BX 10 MOVI,
  BEGIN,  DX DX XOR,  BX DIV,  DX PUSH,  CX INC,  AX AX OR,  Z UNTIL,
  BEGIN,  AX POP,  AL '0' ADDI,  STOSB,  CX DEC,  Z UNTIL,
  RET,
LABEL PUTDEC     \ write AX as a decimal number; uses AX BX CX DX
  DI PUSH,  ES PUSH,  CS PUSH,  ES POP,  DI NUM-BUF MOVI,  >DEC CALL,
  CX DI MOV,  CX NUM-BUF SUBI,  DX NUM-BUF MOVI,  ES POP,  DI POP,  WRITE-CS JMP,

LABEL FIND-ERROR   \ ERRSEEN = 1 if OUT-BUF has "error:"; uses AX BX CX DX
  CS: DX OUT-LEN [] MOV,  DX 6 SUBI,  L1 JB,  DX INC,       \ DX: the places to look at
  SI PUSH,  DI PUSH,  DS PUSH,  ES PUSH,  CS PUSH,  DS POP,  CS PUSH,  ES POP,
  BX OUT-BUF MOVI,
LABEL L2
  SI BX MOV,  DI ERROR$ MOVI,  CX 6 MOVI,  REPE, CMPSB,  L3 JZ,
  BX INC,  DX DEC,  L2 JNZ,
  L4 JMP,
LABEL L3
  ERRSEEN [] 1 MOVI,                                      \ DS = CS here
LABEL L4
  ES POP,  DS POP,  DI POP,  SI POP,
LABEL L1
  RET,
LABEL OUT-LINE   \ show OUT-BUF and a new line, OUT-LEN = 0; uses AX BX CX DX
  FIND-ERROR CALL,
  CS: CX OUT-LEN [] MOV,  DX OUT-BUF MOVI,  WRITE-CS CALL,
  CS: OUT-LEN [] 0 MOVI,  NEWLINE JMP,
LABEL MSG-START  \ a message of the kernel: on its own line, after two spaces
  CS: OUT-LEN [] 0 CMPI,  NZ IF,  OUT-LINE CALL,  THEN,
  DX SPACES$ MOVI,  CX 2 MOVI,  WRITE-CS JMP,

\ ==== The end of the program ============================================

LABEL EXIT-DOS   \ AL: the exit status. The output line first, the old INT 1Bh back
  AX PUSH,
  CS: OUT-LEN [] 0 CMPI,  NZ IF,  OUT-LINE CALL,  THEN,
  CS: OLD1B 2 + [] 0 CMPI,  NZ IF,                        \ only when START set it
    CS: DX OLD1B [] LDS,  AX $251B MOVI,  $21 INT,
  THEN,
  AX POP,  $4C DOS,
LABEL FATAL      \ "ucf: " and the text CS:DX (counted), then the end with status 1
  DX PUSH,  DX UCF$ MOVI,  PUT$ CALL,  DX POP,  PUT$ CALL,  NEWLINE CALL,
  AL 1 MOVI,  EXIT-DOS JMP,
LABEL FATAL-NAME \ "ucf: " the name CUR-NAME, then the text CS:DX, the end with status 1
  DX PUSH,  DX UCF$ MOVI,  PUT$ CALL,  CS: DX CUR-NAME [] MOV,  PUTZ CALL,
  DX POP,  PUT$ CALL,  NEWLINE CALL,  AL 1 MOVI,  EXIT-DOS JMP,

\ ==== The clock =========================================================
\ The BIOS counts 1193180 / 65536 = 18.2 ticks per second at 0040:006C.
\ A tick is 54.925 ms = 56244 / 1024 ms, so ms = ticks * 56244 / 1024.

LABEL TICKS      \ AX: the tick counter of the BIOS, the low 16 bits
  ES PUSH,  AX $40 MOVI,  ES AX MOV,  ES: AX $6C [] MOV,  ES POP,  RET,
LABEL NOW        \ AX: milliseconds since the start, modulo 65536; uses CX DX
  TICKS CALL,  CS: AX START-TICKS [] SUB,
  CX 56244 MOVI,  CX MUL,                                 \ DX:AX = ticks * 56244
  CL 10 MOVI,  AX CL SHR,  CL 6 MOVI,  DX CL SHL,  AX DX OR,   \ / 1024
  RET,

\ ==== Words in the dictionary ===========================================

LABEL WL-OLDEST  \ BX: a wordlist, CS:DX a counted name -> AX: the oldest nt with
                 \ exactly this name, or 0; uses BX CX DI
  AX AX XOR,  BX 0 [BX] MOV,
  BEGIN,  BX BX OR,  NZ WHILE,
    CL 2 [BX] MOV,  CL 31 ANDI,  DI DX MOV,  CS: CL 0 [DI] CMP,
    Z IF,
      SI PUSH,  ES PUSH,  CS PUSH,  ES POP,  SI 3 [BX] LEA,  DI INC,  CH CH XOR,
      REPE, CMPSB,  ES POP,  SI POP,
      Z IF,  AX BX MOV,  THEN,
    THEN,
    BX 0 [BX] MOV,
  REPEAT,  RET,
LABEL SYSXT      \ CS:DX a counted name -> AX: the xt of the oldest word with this
                 \ name in FORTH, else in INTERNALS, or 0; uses BX CX DI
  BX WL-FORTH MOVI,  WL-OLDEST CALL,
  AX AX OR,  Z IF,  BX WL-INTERNALS MOVI,  WL-OLDEST CALL,  THEN,
  AX AX OR,  NZ IF,                                       \ xt = nt + 3 + length, even
    BX AX MOV,  CL 2 [BX] MOV,  CX 31 ANDI,  AX CX ADD,  AX 4 ADDI,  AX -2 ANDI,
  THEN,  RET,
LABEL BODY-END   \ AX: an xt -> AX: the next header in memory after it; uses BX DX DI
  DX -1 MOVI,  BX VOC-LINK [] MOV,
  BEGIN,  BX BX OR,  NZ WHILE,
    DI 0 [BX] MOV,
    BEGIN,  DI DI OR,  NZ WHILE,
      DI AX CMP,  A IF,  DI DX CMP,  B IF,  DX DI MOV,  THEN,  THEN,
      DI 0 [DI] MOV,
    REPEAT,
    BX 2 [BX] MOV,
  REPEAT,
  AX DX MOV,  RET,
LABEL #WORDS     \ AX: the words in FORTH; uses BX
  AX AX XOR,  BX WL-FORTH [] MOV,
  BEGIN,  BX BX OR,  NZ WHILE,  AX INC,  BX 0 [BX] MOV,  REPEAT,  RET,
LABEL REFILLING  \ CF 1: a return address on the return stack points into REFILL; uses AX BX CX DX
  CS: AX XT-REFILL [] MOV,  CS: DX REFILL-END [] MOV,  BX BP MOV,  CX 8 MOVI,
  BEGIN,
    BX R0 CMPI,  AE IF,  CLC,  RET,  THEN,
    0 [BX] AX CMP,  A IF,  0 [BX] DX CMP,  B IF,  STC,  RET,  THEN,  THEN,
    BX 2 ADDI,  CX DEC,
  Z UNTIL,
  CLC,  RET,
LABEL BANNER     \ "uCForth - DOS bootstrap VM - 16 bit - n words in FORTH"; uses AX BX CX DX
  DX BANNER$ MOVI,  PUT$ CALL,  #WORDS CALL,  PUTDEC CALL,  DX WORDS$ MOVI,  PUT$ JMP,

\ ==== Files =============================================================

LABEL SEEK-BACK  \ the file BX: its pointer goes back by CX bytes; uses AX CX DX
  CX CX OR,  Z IF,  RET,  THEN,
  DX CX MOV,  DX NEG,  CX -1 MOVI,  AX $4201 MOVI,  $21 INT,  RET,

\ RDLINE reads one line as READ-LINE does it in the C bootstrap VM: a
\ block of bytes, then the file pointer goes back to the byte after the
\ LF. A line longer than the buffer: the rest comes next; its end (LF
\ or CR LF) right after the buffer belongs to this line.
LABEL RDLINE     \ BX: the file, DS:DX the buffer, CX its size -> AX: the length
                 \ without CR LF, CF 0; the end of the file: CF 1, AX 0;
                 \ an error of DOS: CF 1, AX its code. Uses AX BX CX DX
  CS: RL-BUF [] DX MOV,  CS: RL-MAX [] CX MOV,  CS: RL-LF [] 1 MOVI,
  $3F DOS,  C IF,  RET,  THEN,
  AX AX OR,  Z IF,  STC,  RET,  THEN,
  CS: RL-GOT [] AX MOV,
  DI PUSH,  ES PUSH,  DS PUSH,  ES POP,
  CX AX MOV,  DI DX MOV,  AL 10 MOVI,  REPNE, SCASB,  ES POP,
  Z IF,                                                   \ the LF: DI is after it
    AX DI MOV,  DI POP,  CS: AX RL-BUF [] SUB,            \ AX: the bytes with the LF
    CS: CX RL-GOT [] MOV,  CX AX SUB,  AX DEC,
    AX PUSH,  SEEK-BACK CALL,  AX POP,  L5 JMP,
  THEN,
  DI POP,  CS: RL-LF [] 0 MOVI,
  CS: AX RL-GOT [] MOV,  CS: AX RL-MAX [] CMP,  L5 JB,    \ the last line, without LF
  DS PUSH,  CS PUSH,  DS POP,  DX RL-PEEK MOVI,  CX 2 MOVI,  $3F DOS,  DS POP,
  L6 JC,
  CX AX MOV,  CX CX OR,  L6 JZ,
  CS: RL-PEEK [] BYTE 10 CMPI,  Z IF,                     \ LF: it ends this line
    CX DEC,  SEEK-BACK CALL,  CS: AX RL-MAX [] MOV,  CS: RL-LF [] 1 MOVI,  L5 JMP,
  THEN,
  CX 2 CMPI,  Z IF,  CS: RL-PEEK [] $0A0D CMPI,  Z IF,    \ CR LF: they end this line
    CS: AX RL-MAX [] MOV,  CS: RL-LF [] 1 MOVI,  L7 JMP,
  THEN,  THEN,
  SEEK-BACK CALL,                                         \ else: they start the next line
LABEL L6
  CS: AX RL-MAX [] MOV,  L7 JMP,
LABEL L5                                                  \ a CR at the end goes away
  AX AX OR,  NZ IF,
    BX AX MOV,  CS: BX RL-BUF [] ADD,  -1 [BX] BYTE 13 CMPI,  Z IF,  AX DEC,  THEN,
  THEN,
LABEL L7
  CLC,  RET,

LABEL NAME>BUF   \ the name at the VM address SI, CX bytes, to NAME-BUF with a byte 0;
                 \ "/" becomes "\". Uses AX CX SI DI
  ES PUSH,  CS PUSH,  ES POP,  DI NAME-BUF MOVI,  CX 250 CMPI,  A IF,  CX 250 MOVI,  THEN,
  CX CX OR,  NZ IF,
    BEGIN,  LODSB,  AL '/' CMPI,  Z IF,  AL 92 MOVI,  THEN,  STOSB,  CX DEC,  Z UNTIL,
  THEN,
  AL AL XOR,  STOSB,  ES POP,  RET,
LABEL OPEN-CALL  \ DOS with the name in NAME-BUF: AX and CX as the function needs them
  DS PUSH,  CS PUSH,  DS POP,  DX NAME-BUF MOVI,  $21 INT,  DS POP,  RET,
LABEL IOR        \ AX: an error of DOS -> AX: -38 (the file is not there) or -37
  AX 2 CMPI,  Z IF,  AX -38 MOVI,  RET,  THEN,
  AX 3 CMPI,  Z IF,  AX -38 MOVI,  RET,  THEN,
  AX -37 MOVI,  RET,

\ ==== Writing an image: Intel HEX (doc/vm.md, section 10) ===============

LABEL HEX-DIGIT  \ AL 0-15 as a hex digit to ES:DI
  AL '0' ADDI,  AL '9' CMPI,  A IF,  AL 7 ADDI,  THEN,  STOSB,  RET,
LABEL HEX-BYTE   \ AL as two hex digits to ES:DI; BL = BL + AL (the checksum); uses AX
  BL AL ADD,  AH AL MOV,  AL 1 SHR,  AL 1 SHR,  AL 1 SHR,  AL 1 SHR,  HEX-DIGIT CALL,
  AL AH MOV,  AL $0F ANDI,  HEX-DIGIT JMP,
LABEL COPY$      \ the counted text at CS:SI to ES:DI; uses AX CX SI
  CS: LODSB,  CL AL MOV,  CH CH XOR,
  CX CX OR,  NZ IF,  BEGIN,  CS: LODSB,  STOSB,  CX DEC,  Z UNTIL,  THEN,  RET,
LABEL WRITE-REC  \ write REC-BUF up to DI to the image file; uses AX BX CX DX
  CX DI MOV,  CX REC-BUF SUBI,  DX REC-BUF MOVI,  CS: BX IMG-FID [] MOV,
  DS PUSH,  CS PUSH,  DS POP,  $40 DOS,  DS POP,  RET,
LABEL RECORD     \ a data record for CX bytes (1-16) at the VM address SI; SI goes after them.
                 \ Uses AX BX CX DX DI
  ES PUSH,  CS PUSH,  ES POP,  DI REC-BUF MOVI,
  AL ':' MOVI,  STOSB,  BL BL XOR,
  AL CL MOV,  HEX-BYTE CALL,                              \ the count
  DX SI MOV,  AL DH MOV,  HEX-BYTE CALL,  AL DL MOV,  HEX-BYTE CALL,   \ the address
  AL AL XOR,  HEX-BYTE CALL,                              \ type 00: data
  BEGIN,  LODSB,  HEX-BYTE CALL,  CX DEC,  Z UNTIL,
  AL BL MOV,  AL NEG,  HEX-BYTE CALL,                     \ the checksum
  AL 10 MOVI,  STOSB,  ES POP,
  WRITE-REC JMP,
LABEL AREA       \ the records for the VM addresses SI up to DX; uses AX BX CX DX DI
  BEGIN,  SI DX CMP,  B WHILE,
    CX DX MOV,  CX SI SUB,  CX 16 CMPI,  A IF,  CX 16 MOVI,  THEN,
    DX PUSH,  RECORD CALL,  DX POP,
  REPEAT,  RET,
LABEL WRITE-IMAGE  \ the image to the file CS:DX (a name with a byte 0), AX: its start
                   \ word. CF 1 if it cannot be written. Uses AX BX CX DX DI
  SI PUSH,  CS: IMG-BOOT [] AX MOV,
  DS PUSH,  CS PUSH,  DS POP,  CX CX XOR,  $3C DOS,  DS POP,
  C IF,  SI POP,  RET,  THEN,
  CS: IMG-FID [] AX MOV,
  ES PUSH,  CS PUSH,  ES POP,  DI REC-BUF MOVI,           \ the first line
  SI HEADER$ MOVI,  COPY$ CALL,  CS: AX IMG-BOOT [] MOV,  >DEC CALL,
  SI QUIT-H$ MOVI,  COPY$ CALL,  CS: AX XT-QUIT [] MOV,  >DEC CALL,
  SI THROW-H$ MOVI,  COPY$ CALL,  CS: AX XT-THROW [] MOV,  >DEC CALL,
  SI PRIMS-H$ MOVI,  COPY$ CALL,  SI PRIMS$ MOVI,  COPY$ CALL,
  AL 10 MOVI,  STOSB,  ES POP,  WRITE-REC CALL,
  SI SYS-START MOVI,  DX DATA-START MOVI,  AREA CALL,            \ the system variables
  SI DATA-START MOVI,  DX VP [] MOV,  AREA CALL,           \ the RAM data up to VP
  SI S0 MOVI,  DX DP [] MOV,  AREA CALL,                   \ the dictionary up to DP
  ES PUSH,  CS PUSH,  ES POP,  DI REC-BUF MOVI,  SI END-REC$ MOVI,
  CX 12 MOVI,  BEGIN,  CS: LODSB,  STOSB,  CX DEC,  Z UNTIL,  ES POP,  WRITE-REC CALL,
  CS: BX IMG-FID [] MOV,  $3E DOS,
  SI POP,  CLC,  RET,

\ ==== The end of base.fs (doc/genesis.md, section 6) ====================

LABEL BUILD-END  \ base.fs is complete: the base; with -dump, the base image
  CS: SEEDED [] 0 MOVI,
  AX DP [] MOV,  BASE-END [] AX MOV,  FROZEN-END [] AX MOV,
  #ORDER [] 1 MOVI,  CONTEXT [] WL-FORTH MOVI,  CURRENT [] WL-FORTH MOVI,  STATE [] 0 MOVI,
  DX BOOT$ MOVI,  SYSXT CALL,  AX AX OR,  NZ IF,  CS: XT-BOOT [] AX MOV,  THEN,
  CS: DX DUMP-NAME [] MOV,  DX DX OR,  NZ IF,
    CS: AX XT-BOOT [] MOV,  WRITE-IMAGE CALL,
    C IF,  MSG-START CALL,  DX CANNOT-W$ MOVI,  PUT$ CALL,  CS: DX DUMP-NAME [] MOV,  PUTZ CALL,  NEWLINE CALL,  THEN,
  THEN,
  CS: OUT-LEN [] 0 CMPI,  NZ IF,  OUT-LINE CALL,  THEN,
  BANNER CALL,  DX BUILT$ MOVI,  PUT$ CALL,  NOW CALL,  PUTDEC CALL,  DX MS$ MOVI,  PUT$ CALL,
  NEWLINE JMP,

\ ==== The input: the files of the command line, then the keyboard ======

LABEL STOP-FILES \ an error in a file: "file:line: loading stopped", no more files
  MSG-START CALL,  CS: DX CUR-NAME [] MOV,  PUTZ CALL,
  AL ':' MOVI,  CS: NUM-BUF [] AL MOV,  DX NUM-BUF MOVI,  CX 1 MOVI,  WRITE-CS CALL,
  CS: AX CUR-LN [] MOV,  PUTDEC CALL,  DX STOPPED$ MOVI,  PUT$ CALL,  NEWLINE CALL,
  CS: BX CUR-FID [] MOV,  $3E DOS,  CS: CUR-FID [] 0 MOVI,
  CS: SEEDED [] 0 CMPI,  NZ IF,  BUILD-END CALL,  THEN,    \ the base still gets its end
  CS: AX NARGS [] MOV,  CS: ARGI [] AX MOV,  RET,

LABEL NEXT-FILE  \ open the next file of the command line; a message if it cannot be read
  CS: AX ARGI [] MOV,  CS: ARGI [] INC,  BX AX MOV,  BX 1 SHL,
  CS: DX ARG-LIST [BX] MOV,  CS: CUR-NAME [] DX MOV,
  DS PUSH,  CS PUSH,  DS POP,  AX $3D00 MOVI,  $21 INT,  DS POP,
  NC IF,  CS: CUR-FID [] AX MOV,  CS: CUR-LN [] 0 MOVI,  CS: CUR-LF [] 1 MOVI,  RET,  THEN,
  MSG-START CALL,  DX CANNOT$ MOVI,  PUT$ CALL,  CS: DX CUR-NAME [] MOV,  PUTZ CALL,  NEWLINE CALL,
  CS: AX NARGS [] MOV,  CS: ARGI [] AX MOV,  RET,

LABEL READ-KEYS  \ a line of the keyboard (a device): DOS shows it while it is typed
  CS: CX OUT-LEN [] MOV,  DX OUT-BUF MOVI,  WRITE-CS CALL,  \ the prompt; it stays for Ctrl-C
  CS: WAITING [] 1 MOVI,
  DS PUSH,  CS PUSH,  DS POP,  BX BX XOR,  DX LINE-BUF MOVI,  CX 250 MOVI,  $3F DOS,  DS POP,
  CS: WAITING [] 0 MOVI,  CS: OUT-LEN [] 0 MOVI,
  C IF,  AX AX XOR,  THEN,
  AX AX OR,  Z IF,  NEWLINE CALL,  AL AL XOR,  EXIT-DOS JMP,  THEN,   \ the end of the input
  BX AX MOV,                                              \ CR and LF at the end go away
LABEL L4
  BX BX OR,  L5 JZ,
  CS: AL LINE-BUF -1 + [BX] MOV,
  AL 13 CMPI,  Z IF,  BX DEC,  L4 JMP,  THEN,
  AL 10 CMPI,  Z IF,  BX DEC,  L4 JMP,  THEN,
LABEL L5
  AX BX MOV,  RET,
LABEL READ-PIPE  \ a line of a file or pipe (the input with "<"): it shows after the prompt
  DS PUSH,  CS PUSH,  DS POP,  BX BX XOR,  DX LINE-BUF MOVI,  CX 256 MOVI,  RDLINE CALL,  DS POP,
  C IF,  AL AL XOR,  EXIT-DOS JMP,  THEN,                  \ the end of the input
  AX PUSH,
  CS: CX OUT-LEN [] MOV,  DX OUT-BUF MOVI,  WRITE-CS CALL,  CS: OUT-LEN [] 0 MOVI,
  AX POP,  AX PUSH,  CX AX MOV,  DX LINE-BUF MOVI,  WRITE-CS CALL,  NEWLINE CALL,
  AX POP,  RET,

LABEL NEXT-LINE  \ the next line for ?RX into LINE-BUF, with LF; IN-POS = 0
LABEL L1
  CS: CUR-FID [] 0 CMPI,  Z IF,                           \ no file loads now
    CS: AX ARGI [] MOV,  CS: AX NARGS [] CMP,  B IF,  NEXT-FILE CALL,  L1 JMP,  THEN,
    CS: TTY [] 0 CMPI,  NZ IF,  READ-KEYS CALL,  ELSE,  READ-PIPE CALL,  THEN,
    L2 JMP,
  THEN,
  CS: ERRSEEN [] 0 CMPI,  NZ IF,  CS: ERRSEEN [] 0 MOVI,  STOP-FILES CALL,  L1 JMP,  THEN,
  DS PUSH,  CS PUSH,  DS POP,
  CS: BX CUR-FID [] MOV,  DX LINE-BUF MOVI,  CX 256 MOVI,  RDLINE CALL,  DS POP,
  C IF,                                                   \ the end of the file
    CS: CUR-LF [] 0 CMPI,  NZ IF,                         \ the text after the last LF
      CS: CUR-LF [] 0 MOVI,  AX AX XOR,  L3 JMP,          \ is one more line (genesis.md 2)
    THEN,
    CS: BX CUR-FID [] MOV,  $3E DOS,  CS: CUR-FID [] 0 MOVI,
    CS: SEEDED [] 0 CMPI,  NZ IF,  BUILD-END CALL,  THEN,  \ base.fs is complete
    L1 JMP,
  THEN,
  CS: BX RL-LF [] MOV,  CS: CUR-LF [] BX MOV,
LABEL L3
  CS: CUR-LN [] INC,  CS: OUT-LEN [] 0 MOVI,              \ the prompt does not show
LABEL L2                                                  \ AX: the length of the line
  BX AX MOV,  CS: LINE-BUF [BX] BYTE 10 MOVI,  BX INC,
  CS: IN-LEN [] BX MOV,  CS: IN-POS [] 0 MOVI,  CS: ERRSEEN [] 0 MOVI,  RET,

\ ==== Break: Ctrl-Break and Ctrl-C (doc/vm.md, section 9) ===============
\ The BIOS calls INT 1Bh for Ctrl-Break at once, also in a loop of Forth;
\ DOS calls INT 23h when it sees Ctrl-C at the console. Both handlers
\ only change the table: each primitive is BREAK now. So the next
\ primitive runs BREAK, where the VM is complete, and NEXT costs nothing
\ more. BREAK puts the table back. While DOS reads a line of the
\ keyboard (WAITING), Ctrl-C only gives the prompt again; DOS then reads
\ the line again.

FORWARD BREAK
LABEL BREAK-ON   \ each primitive is BREAK now; keeps all registers
  AX PUSH,  CX PUSH,  DI PUSH,  ES PUSH,
  CS PUSH,  ES POP,  DI OPTAB MOVI,  AX BREAK MOVI,  CX 128 MOVI,  CLD,  REP, STOSW,
  CS: BRK-FLAG [] 1 MOVI,
  ES POP,  DI POP,  CX POP,  AX POP,  RET,
LABEL ON-1B      \ INT 1Bh: Ctrl-Break
  CS: WAITING [] 0 CMPI,  Z IF,  BREAK-ON CALL,  THEN,  IRET,
LABEL ON-23      \ INT 23h: Ctrl-C, seen by DOS; an IRET makes DOS go on
  CS: WAITING [] 0 CMPI,  Z IF,  BREAK-ON CALL,  IRET,  THEN,
  AX PUSH,  BX PUSH,  CX PUSH,  DX PUSH,
  CS: CX OUT-LEN [] MOV,  DX OUT-BUF MOVI,  WRITE-CS CALL,   \ the prompt again
  DX POP,  CX POP,  BX POP,  AX POP,  IRET,
LABEL BREAK      \ in the place of a primitive: stop the running word
  DS PUSH,  ES PUSH,  CS PUSH,  DS POP,  CS PUSH,  ES POP,
  SI OPTAB0 MOVI,  DI OPTAB MOVI,  CX 128 MOVI,  REP, MOVSW,   \ the table back
  ES POP,  DS POP,
  CS: BRK-FLAG [] 0 MOVI,
  CS: AX IN-LEN [] MOV,  CS: IN-POS [] AX MOV,           \ the rest of the input line goes
  OUT-LINE CALL,                                          \ a new line
  CS: BX OUT-A [] MOV,  BX BX OR,  NZ IF,  0 [BX] 0 MOVI,  THEN,
  HANDLER [] 0 CMPI,  NZ IF,  AX -28 MOVI,  THROW-CODE JMP,  THEN,   \ a CATCH gets "user interrupt"
  SP S0 MOVI,  BP R0 MOVI,  STATE [] 0 MOVI,               \ else: empty stacks, then QUIT
  CS: BX XT-QUIT [] MOV,  DISPATCH JMP,

\ ==== The console: primitives 29-31 =====================================
\ ?RX gives the characters of LINE-BUF. A new line comes when QUIT waits
\ in REFILL; a word that reads keys gets "no key" while files load, or
\ when the input is a file. At the keyboard, KEY? does not wait: DOS
\ tells if a key is there (0Bh); then DOS reads the whole line.

29 CODE ?RX
  CS: AX IN-POS [] MOV,  CS: AX IN-LEN [] CMP,  L1 JB,     \ a character of the line is there
  REFILLING CALL,  L2 JC,                                  \ QUIT waits for a line
  CS: CUR-FID [] 0 CMPI,  L3 JNZ,                          \ a file loads: no key
  CS: AX ARGI [] MOV,  CS: AX NARGS [] CMP,  L3 JB,        \ files wait: no key
  CS: TTY [] 0 CMPI,  L3 JZ,                               \ input from a file: no key
  $0B DOS,  AL AL OR,  L3 JZ,                              \ no key yet
LABEL L2
  NEXT-LINE CALL,
LABEL L1
  CS: BX IN-POS [] MOV,  CS: IN-POS [] INC,
  AX AX XOR,  CS: AL LINE-BUF [BX] MOV,  AX PUSH,  AX -1 MOVI,  AX PUSH,  NEXT,
LABEL L3
  AX AX XOR,  AX PUSH,  NEXT,

30 CODE TX!       \ the characters of a line wait in OUT-BUF; the line shows at LF
  AX POP,  AL AL OR,  L1 JZ,                               \ the byte 0 is not sent
  AL 10 CMPI,  Z IF,  OUT-LINE CALL,  NEXT,  THEN,
  CS: BX OUT-LEN [] MOV,  BX 256 CMPI,  AE IF,             \ a full buffer shows without LF
    AX PUSH,  CX BX MOV,  DX OUT-BUF MOVI,  WRITE-CS CALL,  AX POP,  BX BX XOR,
  THEN,
  CS: OUT-BUF [BX] AL MOV,  BX INC,  CS: OUT-LEN [] BX MOV,
LABEL L1
  NEXT,

31 CODE !IO       NEXT,

\ ==== HAL 80-88 =========================================================

80 CODE MILLIS    NOW CALL,  AX PUSH,  NEXT,
81 CODE MS
LABEL WAIT-MS     \ ( u -- ): wait u ms, in steps of a tick (55 ms); Ctrl-Break stops it
  DI POP,  NOW CALL,  BX AX MOV,
  BEGIN,
    CS: BRK-FLAG [] 0 CMPI,  NZ IF,  NEXT,  THEN,
    NOW CALL,  AX BX SUB,  AX DI CMP,
  AE UNTIL,
  NEXT,
82 CODE US        \ ( u -- ): (u + 999) / 1000 ms
  AX POP,  DX DX XOR,  AX 999 ADDI,  DX 0 ADCI,  CX 1000 MOVI,  CX DIV,  AX PUSH,  WAIT-MS JMP,
83 CODE BIS!      BX POP,  AX POP,  0 [BX] AL OR,  NEXT,
84 CODE BIC!      BX POP,  AX POP,  AL NOT,  0 [BX] AL AND,  NEXT,
85 CODE SAVE-IMAGE   \ ( -- ior ): the image to UCF.HEX
  DX UCF-HEX$ MOVI,  CS: AX XT-BOOT [] MOV,  WRITE-IMAGE CALL,
  C IF,  AX -37 MOVI,  AX PUSH,  NEXT,  THEN,
  MSG-START CALL,  DX SAVED$ MOVI,  PUT$ CALL,  DX UCF-HEX$ MOVI,  PUTZ CALL,  NEWLINE CALL,
  AX AX XOR,  AX PUSH,  NEXT,
86 CODE ERASE-IMAGE   NEXT,
87 CODE REBOOT    \ ( f -- ): empty stacks, then BOOT (f true) or QUIT
  AX POP,  SP S0 MOVI,  BP R0 MOVI,  STATE [] 0 MOVI,  HANDLER [] 0 MOVI,
  AX AX OR,  Z IF,  CS: BX XT-QUIT [] MOV,  ELSE,  CS: BX XT-BOOT [] MOV,  THEN,
  DISPATCH JMP,
88 CODE BYE       AL AL XOR,  EXIT-DOS JMP,
99 CODE (BYE)     AX POP,  EXIT-DOS JMP,

\ ==== Files: HAL 89-93, 98 (doc/vm.md, section 8) =======================
\ A fid is a number from 10 to 29, as in the C bootstrap VM: the lowest
\ free one. FID-TAB holds the handle of DOS for it. So the numbers in an
\ image do not depend on the VM, and Forth cannot close the handles 0-4
\ of DOS (the console) by mistake. OPEN-FILE with W/O writes at the end
\ of the file, and makes it if it is not there (as the C VM does).

LABEL NEW-FID     \ AX: a handle of DOS -> AX: a new fid; CF 1: no fid free (the file
                  \ is closed again, AX = 4: too many files). Uses BX CX
  BX FID-TAB MOVI,  CX #FIDS MOVI,
  BEGIN,
    CS: 0 [BX] 0 CMPI,  Z IF,
      AX INC,  CS: 0 [BX] AX MOV,  AX BX MOV,  AX FID-TAB SUBI,  AX 1 SHR,  AX 10 ADDI,
      CLC,  RET,
    THEN,
    BX 2 ADDI,  CX DEC,
  Z UNTIL,
  BX AX MOV,  $3E DOS,  AX 4 MOVI,  STC,  RET,
LABEL FID>HANDLE  \ BX: a fid -> BX: its handle of DOS, DI: its place in FID-TAB;
                  \ CF 1: it is no open fid
  BX 10 SUBI,  C IF,  RET,  THEN,
  BX #FIDS CMPI,  CMC,  C IF,  RET,  THEN,
  BX 1 SHL,  BX FID-TAB ADDI,  DI BX MOV,  CS: BX 0 [BX] MOV,
  BX BX OR,  Z IF,  STC,  RET,  THEN,
  BX DEC,  CLC,  RET,
LABEL FID-ERR     AX -37 MOVI,  AX PUSH,  NEXT,           \ ( -- -37 )

LABEL FILE-ERR    IOR CALL,  DX DX XOR,  DX PUSH,  AX PUSH,  NEXT,      \ ( -- 0 ior )
LABEL FILE-OK     NEW-FID CALL,  FILE-ERR JC,                          \ ( -- fid 0 )
                  AX PUSH,  AX AX XOR,  AX PUSH,  NEXT,
: FILE-END, ( -- )   C IF,  FILE-ERR JMP,  THEN,  FILE-OK JMP, ;   \ after a call of DOS

89 CODE OPEN-FILE ( a u fam -- fid ior )
  CS: FAM [] POP,  CX POP,  DX POP,  SI PUSH,  SI DX MOV,  NAME>BUF CALL,  SI POP,
  CS: AX FAM [] MOV,  AL 3 ANDI,  AH $3D MOVI,  OPEN-CALL CALL,
  NC IF,
    CS: FAM [] 1 CMPI,  Z IF,                              \ W/O: write at the end
      AX PUSH,  BX AX MOV,  CX CX XOR,  DX DX XOR,  AX $4202 MOVI,  $21 INT,  AX POP,
    THEN,
    FILE-OK JMP,
  THEN,
  CS: FAM [] 1 CMPI,  NZ IF,  FILE-ERR JMP,  THEN,         \ only W/O makes the file
  AX 2 CMPI,  NZ IF,  FILE-ERR JMP,  THEN,                  \ when it is not there
  CX CX XOR,  AH $3C MOVI,  OPEN-CALL CALL,  FILE-END,
90 CODE CREATE-FILE ( a u fam -- fid ior )
  AX POP,  CX POP,  DX POP,  SI PUSH,  SI DX MOV,  NAME>BUF CALL,  SI POP,
  CX CX XOR,  AH $3C MOVI,  OPEN-CALL CALL,  FILE-END,
91 CODE CLOSE-FILE ( fid -- ior )
  BX POP,  FID>HANDLE CALL,  C IF,  FID-ERR JMP,  THEN,
  CS: 0 [DI] 0 MOVI,  $3E DOS,  AX 0 MOVI,  C IF,  AX -37 MOVI,  THEN,  AX PUSH,  NEXT,   \ MOV keeps the carry
92 CODE READ-FILE ( a u fid -- u2 ior )
  BX POP,  CX POP,  DX POP,  FID>HANDLE CALL,  C IF,  AX 6 MOVI,  FILE-ERR JMP,  THEN,
  $3F DOS,  C IF,  FILE-ERR JMP,  THEN,
  AX PUSH,  AX AX XOR,  AX PUSH,  NEXT,
93 CODE WRITE-FILE ( a u fid -- ior )  \ 0 bytes: no call, DOS would cut the file there
  BX POP,  CX POP,  DX POP,  FID>HANDLE CALL,  C IF,  FID-ERR JMP,  THEN,  AX AX XOR,
  CX CX OR,  NZ IF,
    CX PUSH,  $40 DOS,  CX POP,
    C IF,  AX -37 MOVI,  ELSE,  AX CX CMP,  AX 0 MOVI,  B IF,  AX -37 MOVI,  THEN,  THEN,
  THEN,
  AX PUSH,  NEXT,
98 CODE READ-LINE ( a u fid -- u2 f ior )
  BX POP,  CX POP,  DX POP,  FID>HANDLE CALL,
  C IF,  DX DX XOR,  DX PUSH,  DX PUSH,  FID-ERR JMP,  THEN,
  RDLINE CALL,
  NC IF,  AX PUSH,  AX -1 MOVI,  AX PUSH,  AX AX XOR,  AX PUSH,  NEXT,  THEN,
  AX AX OR,  Z IF,  AX PUSH,  AX PUSH,  AX PUSH,  NEXT,  THEN,     \ the end: 0 false 0
  DX DX XOR,  DX PUSH,  DX PUSH,  AX -37 MOVI,  AX PUSH,  NEXT,

\ ==== HAL of DOS: 100, 110-113 ============================================
\ SYSCALL ( x1 ... xn n id -- x ): x1 to xn go to AX BX CX DX SI DI (n at
\ most 6), then INT id; x is AX after it. DS and ES are the segment of
\ the VM, so an address in a register is a VM address. After the call
\ the registers are at REGS ($FFF0): AX BX CX DX SI DI, the flags (bit 0
\ is the carry), ES.

LABEL SC-INT       $CD TC, $21 TC,  RET,     \ INT id: SYSCALL writes id into the second byte
100 CODE SYSCALL
  AX POP,  CS: SC-INT 1+ [] AL MOV,
  CX POP,  CX 6 CMPI,  A IF,  AX -24 MOVI,  THROW-CODE JMP,  THEN,
  DI REGS MOVI,  AX AX XOR,  BX CX MOV,  CX 8 MOVI,  REP, STOSW,
  BX 1 SHL,  BEGIN,  BX BX OR,  NZ WHILE,  BX 2 SUBI,  REGS [BX] POP,  REPEAT,
  SI PUSH,  BP PUSH,
  AX REGS [] MOV,  BX REGS 2 + [] MOV,  CX REGS 4 + [] MOV,  DX REGS 6 + [] MOV,
  SI REGS 8 + [] MOV,  DI REGS 10 + [] MOV,
  SC-INT CALL,
  CS: SC-AX [] AX MOV,  PUSHF,  AX POP,  CS: SC-FL [] AX MOV,
  AX ES MOV,  CS: SC-ES [] AX MOV,
  CS: AX VMSEG [] MOV,  DS AX MOV,  ES AX MOV,
  CS: AX SC-AX [] MOV,  REGS [] AX MOV,  REGS 2 + [] BX MOV,  REGS 4 + [] CX MOV,
  REGS 6 + [] DX MOV,  REGS 8 + [] SI MOV,  REGS 10 + [] DI MOV,
  CS: AX SC-FL [] MOV,  REGS 12 + [] AX MOV,  CS: AX SC-ES [] MOV,  REGS 14 + [] AX MOV,
  BP POP,  SI POP,  CS: AX SC-AX [] MOV,  AX PUSH,  NEXT,

110 CODE PC@      DX POP,  AL DX IN,  AH AH XOR,  AX PUSH,  NEXT,   \ ( port -- c )
111 CODE PC!      DX POP,  AX POP,  DX AL OUT,  NEXT,               \ ( c port -- )
112 CODE PW@      DX POP,  AX DX IN,  AX PUSH,  NEXT,               \ ( port -- x )
113 CODE PW!      DX POP,  AX POP,  DX AX OUT,  NEXT,               \ ( x port -- )


\ ==== Loading an image file (doc/vm.md, section 10) =====================
\ This runs at the start, before the VM: DS = ES = CS, SI and BP are free.

LABEL HEX-NIBBLE \ AL: a character -> AL: its value 0-15; CF 1: it is no hex digit
  AL '0' SUBI,  C IF,  RET,  THEN,                  \ below "0"
  AL 10 CMPI,  C IF,  CLC,  RET,  THEN,             \ 0-9
  AL $DF ANDI,  AL 17 SUBI,  C IF,  RET,  THEN,     \ a-f as A-F; below "A"
  AL 6 CMPI,  CMC,  C IF,  RET,  THEN,              \ above "F"
  AL 10 ADDI,  RET,
LABEL HEX2       \ two hex digits at SI -> AL; BL = BL + AL; SI + 2; CF 1: no hex digits
  LODSB,  HEX-NIBBLE CALL,  C IF,  RET,  THEN,  AH AL MOV,
  LODSB,  HEX-NIBBLE CALL,  C IF,  RET,  THEN,
  AH 1 SHL,  AH 1 SHL,  AH 1 SHL,  AH 1 SHL,  AL AH OR,  BL AL ADD,  CLC,  RET,
LABEL NUMBER-AT  \ the decimal number at SI -> AX, SI after it; uses CX DX
  AX AX XOR,
  BEGIN,  CL 0 [SI] MOV,  CL '0' SUBI,  CL 10 CMPI,  B WHILE,
    DX 10 MOVI,  DX MUL,  CH CH XOR,  AX CX ADD,  SI INC,
  REPEAT,  RET,
LABEL FIND-KEY   \ the counted text at DI in LINE-BUF (AX characters) -> SI after
                 \ it; CF 1: it is not there. Uses BX CX DX
  BX LINE-BUF MOVI,  DX AX MOV,  DX BX ADD,         \ DX: the end of the line
  BEGIN,  BX DX CMP,  B WHILE,
    SI BX MOV,  CL 0 [DI] MOV,  CH CH XOR,  DI PUSH,  DI INC,  REPE, CMPSB,  DI POP,
    Z IF,  CLC,  RET,  THEN,
    BX INC,
  REPEAT,
  STC,  RET,
LABEL HAVE?      \ AX: a primitive number -> CF 1: this kernel has it; uses BX
  AX 127 CMPI,  A IF,  CLC,  RET,  THEN,
  BX AX MOV,  BX 1 SHL,  CS: OPTAB [BX] UNKNOWN-OP CMPI,  NZ IF,  STC,  RET,  THEN,
  CLC,  RET,
LABEL NEEDS      \ AX: a primitive that this kernel does not have: the message, the end
  AX PUSH,  DX UCF$ MOVI,  PUT$ CALL,  CS: DX CUR-NAME [] MOV,  PUTZ CALL,
  DX NEEDS$ MOVI,  PUT$ CALL,  AX POP,  PUTDEC CALL,  NEWLINE CALL,  AL 1 MOVI,  EXIT-DOS JMP,
LABEL PRIMS?     \ SI: the text after "prims=", ranges as "0-31,80-88": copy it to
                 \ PRIMS$ for SAVE-IMAGE; each primitive must be in this kernel
  SI PUSH,  DI PRIMS$ 1+ MOVI,  CX CX XOR,
  BEGIN,  AL 0 [SI] MOV,  AL 32 CMPI,  A WHILE,     \ up to a blank, CR, LF or 0
    CX 30 CMPI,  B IF,  STOSB,  CX INC,  THEN,  SI INC,
  REPEAT,
  CS: PRIMS$ [] CL MOV,  SI POP,
LABEL L1                                            \ the next number or range
  AL 0 [SI] MOV,  AL '0' SUBI,  AL 10 CMPI,  AE IF,  RET,  THEN,
  NUMBER-AT CALL,  BP AX MOV,                       \ BP: the first number
  0 [SI] BYTE '-' CMPI,  Z IF,  SI INC,  NUMBER-AT CALL,  ELSE,  AX BP MOV,  THEN,
  DI AX MOV,                                        \ DI: the last number
  BEGIN,  BP DI CMP,  BE WHILE,
    AX BP MOV,  HAVE? CALL,  NC IF,  AX BP MOV,  NEEDS JMP,  THEN,  BP INC,
  REPEAT,
  0 [SI] BYTE ',' CMPI,  NZ IF,  RET,  THEN,  SI INC,  L1 JMP,

LABEL NOT-IMAGE  \ the end: the file is not an image for this kernel
  DX NOT-IMAGE$ MOVI,  FATAL-NAME JMP,
LABEL HEADER-LINE  \ AX: the length of a comment line in LINE-BUF. The first line
                   \ of an image: "; ucf image bits=16 boot=n quit=n throw=n prims=..."
  AX PUSH,  DI BOOT=$ MOVI,  FIND-KEY CALL,  C IF,  AX POP,  RET,  THEN,   \ another comment
  NUMBER-AT CALL,  CS: XT-BOOT [] AX MOV,
  AX POP,  AX PUSH,  DI BITS$ MOVI,  FIND-KEY CALL,  NOT-IMAGE JC,
  NUMBER-AT CALL,  AX 16 CMPI,  NOT-IMAGE JNZ,
  AX POP,  AX PUSH,  DI QUIT=$ MOVI,  FIND-KEY CALL,  NOT-IMAGE JC,
  NUMBER-AT CALL,  CS: XT-QUIT [] AX MOV,
  AX POP,  AX PUSH,  DI THROW=$ MOVI,  FIND-KEY CALL,  NOT-IMAGE JC,
  NUMBER-AT CALL,  CS: XT-THROW [] AX MOV,
  AX POP,  DI PRIMS=$ MOVI,  FIND-KEY CALL,  NC IF,  PRIMS? CALL,  THEN,
  RET,

LABEL BAD-RECORD \ the end: "ucf: file:line: wrong record"
  DX UCF$ MOVI,  PUT$ CALL,  CS: DX CUR-NAME [] MOV,  PUTZ CALL,
  AL ':' MOVI,  CS: NUM-BUF [] AL MOV,  DX NUM-BUF MOVI,  CX 1 MOVI,  WRITE-CS CALL,
  CS: AX CUR-LN [] MOV,  PUTDEC CALL,  DX CHECKSUM$ MOVI,  PUT$ CALL,  NEWLINE CALL,
  AL 1 MOVI,  EXIT-DOS JMP,
LABEL LOAD-HEX   \ load the image file CS:DX (a name with a byte 0) into the VM
  CS: CUR-NAME [] DX MOV,  AX $3D00 MOVI,  $21 INT,
  C IF,  DX UCF$ MOVI,  PUT$ CALL,  DX CANNOT$ MOVI,  PUT$ CALL,
         CS: DX CUR-NAME [] MOV,  PUTZ CALL,  NEWLINE CALL,  AL 1 MOVI,  EXIT-DOS JMP,  THEN,
  CS: IMG-FID [] AX MOV,  CS: CUR-LN [] 0 MOVI,
LABEL L1                                            \ the next line
  CS: BX IMG-FID [] MOV,  DX LINE-BUF MOVI,  CX 255 MOVI,  RDLINE CALL,
  C IF,  L8 JMP,  THEN,                             \ the end of the file
  CS: CUR-LN [] INC,  BP AX MOV,                    \ BP: the length of the line
  CS: LINE-BUF [] BYTE ';' CMPI,  Z IF,  HEADER-LINE CALL,  L1 JMP,  THEN,
  CS: LINE-BUF [] BYTE ':' CMPI,  L1 JNZ,
  SI LINE-BUF 1+ MOVI,  BL BL XOR,
  HEX2 CALL,  L7 JC,  CL AL MOV,  CH CH XOR,        \ CX: the number of data bytes
  HEX2 CALL,  L7 JC,  DH AL MOV,  HEX2 CALL,  L7 JC,  DL AL MOV,   \ DX: the address
  HEX2 CALL,  L7 JC,  BH AL MOV,                    \ BH: the type
  CX 100 CMPI,  L7 JA,
  AX CX MOV,  AX 1 SHL,  AX 11 ADDI,  AX BP CMP,  L7 JA,   \ the line holds all digits
  DI REC-BUF MOVI,  CX PUSH,
  CX CX OR,  NZ IF,  BEGIN,  HEX2 CALL,  L6 JC,  STOSB,  CX DEC,  Z UNTIL,  THEN,
  HEX2 CALL,  L6 JC,  CX POP,  BL BL OR,  L7 JNZ,   \ the sum of all bytes is 0
  BH 1 CMPI,  L8 JZ,                                \ type 1: the end of the image
  BH BH OR,  NZ IF,  L1 JMP,  THEN,                 \ another type: not for 16 bits
  ES PUSH,  CS: AX VMSEG [] MOV,  ES AX MOV,  SI REC-BUF MOVI,  DI DX MOV,  REP, MOVSB,  ES POP,
  L1 JMP,
LABEL L6
  CX POP,
LABEL L7
  BAD-RECORD JMP,
LABEL L8                                            \ the end of the file
  CS: BX IMG-FID [] MOV,  $3E DOS,
  CS: XT-QUIT [] 0 CMPI,  Z IF,  NOT-IMAGE JMP,  THEN,   \ no first line: no image
  RET,

FORWARD IMAGE-AT
LABEL COPY-IMAGE \ the image after the code into the VM: SYS, the RAM data, the dictionary
  SI IMAGE-AT MOVI,  ES PUSH,  CS: AX VMSEG [] MOV,  ES AX MOV,
  DI SYS-START MOVI,  CX 256 MOVI,  REP, MOVSB,
  ES: CX VP [] MOV,  CX DATA-START SUBI,  DI DATA-START MOVI,  REP, MOVSB,
  ES: CX DP [] MOV,  CX S0 SUBI,  DI S0 MOVI,  REP, MOVSB,
  ES POP,  RET,

\ ==== The command line ==================================================
\ DOS gives it at $80 of the PSP: its length, then the text. Each word
\ gets a byte 0 at its end; "/" in a name becomes "\".

LABEL NEXT-WORD  \ SI: in the command line, DX: its end -> SI: the next word; Z: none
  BEGIN,  SI DX CMP,  AE IF,  AL AL XOR,  RET,  THEN,
          0 [SI] BYTE 0 CMPI,  Z WHILE,  SI INC,  REPEAT,
  AL 1 MOVI,  AL AL OR,  RET,
LABEL SKIP-WORD  \ SI: at a word -> SI: after its byte 0
  BEGIN,  LODSB,  AL AL OR,  Z UNTIL,  RET,
LABEL OPTION?    \ SI: a word, DI: an option (counted, small letters) -> Z: the same
  BX SI MOV,  CL 0 [DI] MOV,  CH CH XOR,  DI INC,
  BEGIN,  AL 0 [BX] MOV,  AL $20 ORI,  AL 0 [DI] CMP,  NZ IF,  RET,  THEN,
    BX INC,  DI INC,  CX DEC,  Z UNTIL,
  0 [BX] BYTE 0 CMPI,  RET,
LABEL PARSE-ARGS \ options and files of the command line; DS = ES = CS
  CL $80 [] MOV,  CH CH XOR,  SI $81 MOVI,
  CX CX OR,  NZ IF,
    BEGIN,  AL 0 [SI] MOV,
      AL 32 CMPI,  BE IF,  0 [SI] BYTE 0 MOVI,  THEN,
      AL '/' CMPI,  Z IF,  0 [SI] BYTE 92 MOVI,  THEN,
      SI INC,  CX DEC,  Z UNTIL,
  THEN,
  0 [SI] BYTE 0 MOVI,  DX SI MOV,  SI $81 MOVI,
LABEL L1
  NEXT-WORD CALL,  Z IF,  RET,  THEN,
  DI IMAGE-OPT$ MOVI,  OPTION? CALL,  Z IF,
    SKIP-WORD CALL,  NEXT-WORD CALL,  L9 JZ,  CS: IMG-NAME [] SI MOV,  SKIP-WORD CALL,  L1 JMP,
  THEN,
  DI DUMP-OPT$ MOVI,  OPTION? CALL,  Z IF,
    SKIP-WORD CALL,  NEXT-WORD CALL,  L9 JZ,  CS: DUMP-NAME [] SI MOV,  SKIP-WORD CALL,  L1 JMP,
  THEN,
  0 [SI] BYTE '-' CMPI,  L9 JZ,                     \ an unknown option
  CS: BX NARGS [] MOV,  BX #ARGS CMPI,  L9 JAE,
  BX 1 SHL,  CS: ARG-LIST [BX] SI MOV,  CS: NARGS [] INC,
  SKIP-WORD CALL,  L1 JMP,
LABEL L9
  DX USE$ MOVI,  FATAL JMP,

\ ==== The start =========================================================

LABEL START      \ DOS starts the program here (via the jump at $100): CS = DS = ES = SS
  CLD,
  AX CS MOV,  AX $1000 ADDI,  CS: VMSEG [] AX MOV,  \ the VM: the 64 KB after this segment
  DX 2 [] MOV,  AX $1000 ADDI,  DX AX CMP,          \ the PSP: the end of the memory of UCF
  B IF,  DX MEMORY$ MOVI,  FATAL JMP,  THEN,
  PARSE-ARGS CALL,
  ES PUSH,  CS: AX VMSEG [] MOV,  ES AX MOV,  DI DI XOR,  CX $8000 MOVI,  AX AX XOR,
  REP, STOSW,  ES POP,                              \ the memory of the VM: all 0
  SI OPTAB MOVI,  DI OPTAB0 MOVI,  CX 128 MOVI,  REP, MOVSW,   \ the copy for BREAK
  DI FID-TAB MOVI,  CX #FIDS MOVI,  AX AX XOR,  REP, STOSW,      \ no fid is open
  AX $351B MOVI,  $21 INT,  CS: OLD1B [] BX MOV,  CS: OLD1B 2 + [] ES MOV,
  CS PUSH,  ES POP,
  DX ON-1B MOVI,  AX $251B MOVI,  $21 INT,          \ Ctrl-Break
  DX ON-23 MOVI,  AX $2523 MOVI,  $21 INT,          \ Ctrl-C
  AX $4400 MOVI,  BX BX XOR,  $21 INT,              \ is the input a device?
  NC IF,  DX $80 ANDI,  CS: TTY [] DX MOV,  THEN,
  TICKS CALL,  CS: START-TICKS [] AX MOV,
  CS: IMAGE-LEN [] 0 CMPI,  NZ IF,  COPY-IMAGE CALL,  ELSE,
    CS: DX IMG-NAME [] MOV,  DX DX OR,  Z IF,  DX USE$ MOVI,  FATAL JMP,  THEN,
    LOAD-HEX CALL,
    CS: AX XT-BOOT [] MOV,  CS: AX XT-QUIT [] CMP,  Z IF,  CS: SEEDED [] 1 MOVI,  THEN,
  THEN,
  CS: AX VMSEG [] MOV,  DS AX MOV,  ES AX MOV,      \ from now on: the segment of the VM
  CLI,  SS AX MOV,  SP S0 MOVI,  STI,
  DX REFILL$ MOVI,  SYSXT CALL,  CS: XT-REFILL [] AX MOV,  BODY-END CALL,  CS: REFILL-END [] AX MOV,
  DX OUT$ MOVI,  SYSXT CALL,  AX AX OR,  NZ IF,  BX AX MOV,  AX 2 [BX] MOV,  THEN,
  CS: OUT-A [] AX MOV,
  CS: IMAGE-LEN [] 0 CMPI,  Z IF,                   \ the bootstrap VM says what it runs
    BANNER CALL,  DX IMAGE$ MOVI,  PUT$ CALL,  CS: DX IMG-NAME [] MOV,  PUTZ CALL,  NEWLINE CALL,
  THEN,
  BP R0 MOVI,  STATE [] 0 MOVI,  HANDLER [] 0 MOVI,
  CS: BX XT-BOOT [] MOV,  DISPATCH JMP,

LABEL IMAGE-AT   \ build.fs puts the image here, after the code
