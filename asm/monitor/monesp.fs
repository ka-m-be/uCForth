\ monesp.fs - package MON-ESP32: the monitor on the ESP32 and the ESP32-S3
\
\ The part of the monitor for the boards with an ESP32 or an ESP32-S3
\ (asm/monitor/README.md): the registers of the Xtensa, calls of the
\ functions of the ROM or of your own code with the registers of the
\ image, the registers of the last exception of the CPU, the special
\ registers, memory that the CPU reads only in cells. Its words are
\ CODE words (lib/esp32/asm.fs).
\
\   a CALL-AT ( -- )     a call of the function at a, as the ROM and C
\                        code expect it (CALLX8, the register window):
\                        its arguments a2 to a7 come from the image;
\                        afterwards the image has a2 to a7 as the function
\                        left them (its result in a2), SAR, and CYCLES:
\                        the cycles of the CPU for the call
\   LAST-FAULT ( -- )    the image gets the registers of the last
\                        exception of the CPU (FAULT-REGS of INTR)
\   n SR@ ( -- x )       the special register n; its name from XTENSA:
\                        PS SR@, CCOUNT SR@
\   x n SR! ( -- )       writes the special register n
\   a u DUMPC ( -- )     u bytes from a, as DUMP shows them, but read
\                        in cells: the RAM of the code and the ROM give
\                        an exception for a single byte
\   a CELL-C@ ( -- c )   a byte, read from its cell; DISASM reads the
\                        code with it (DIS-FETCH, after the load)
\
\ Breakpoints and single steps, as on a PC (monpc.fs):
\
\   a DEBUG-AT ( -- )    the function at a waits for STEP or GO: a call
\                        as CALL-AT makes it, with a stack of its own
\   STEP ( -- )          the function runs one instruction (ICOUNT)
\   GO ( -- )            the function runs up to a breakpoint, a BREAK
\                        instruction, or its end
\   a BREAK-AT ( -- )    a breakpoint at a (IBREAKA0, IBREAKA1): 2 at most
\   CLEAR-BREAKS ( -- )  no breakpoints;  .BREAKS ( -- ) shows them
\   STOPPED ( -- n )     why it stopped: 1 after a step, 3 at a breakpoint
\                        or BREAK, 0 at its end
\
\ The image has PC PS SAR EXCCAUSE EXCVADDR, a0 to a15, LBEG LEND LCOUNT
\ WINDOWBASE WINDOWSTART DEBUGCAUSE, and CYCLES (.REGS, SET-REG and
\ GET-REG of the package MONITOR). After an exception, .REGS also shows
\ the cause. For example, the ROM function esp_rom_spiflash_read
\ ( faddr a u -- r ) reads the start of the image from the flash (at
\ $1000 on the ESP32, at 0 on the ESP32-S3):
\
\   ok> CREATE BUF 16 ALLOT
\   ok> $1000 SET-REG A2  BUF >CHIP SET-REG A3  16 SET-REG A4
\   ok> 'FLASH-READ CALL-AT  GET-REG A2 .  BUF 16 DUMP
\
\ A call runs on the stack of the ROM functions (as SYSCALL), with a0 to
\ a7 of its caller kept. An exception in it ends as in SYSCALL: THROW,
\ and LAST-FAULT shows its registers. After each stop of DEBUG-AT, the
\ image holds the registers of the function (its register window a0 to
\ a15, PC, PS ...); SET-REG changes them for the next STEP or GO. At its
\ end, a2 to a7 hold what the function left there.
\
\ Needs TEXT, XTENSA, ASM, MONITOR, DISXT and the image of a board
\ (ROM, INTR). On the board, in ucf term (after a start the search order
\ has room for it: at most 16 vocabularies):
\   #require FAIL lib/text.fs
\   #require ADD, asm/xtensa.fs
\   #include lib/esp32/asm.fs
\   #include asm/monitor/monitor.fs
\   #include asm/monitor/disxt.fs
\   #include asm/monitor/monesp.fs

PACKAGE MON-ESP32  ALSO ASM  ALSO ROM  ALSO INTR  ALSO MONITOR   \ lib/package.fs

\ ==== Memory in cells ====================================================

PUBLIC
: CELL-C@ ( a -- c )   DUP -4 AND @  SWAP 3 AND 8 * RSHIFT  255 AND ;
: DUMPC ( a u -- )   \ as DUMP, from cells
  OVER + SWAP
  BEGIN 2DUP U> WHILE
    CR DUP .HEX ." : "
    16 0 DO  DUP I + CELL-C@ .H2 SPACE  LOOP
    16 0 DO  DUP I + CELL-C@  DUP 32 127 WITHIN 0= IF DROP 46 THEN  EMIT  LOOP
    16 +
  REPEAT 2DROP ;

\ ==== The image ===========================================================
\ The order of FAULT-REGS (lib/esp32/intr.fs), and CYCLES after it.

PRIVATE
112 CONSTANT /IMG
/IMG BUFFER: IMG   IMG /IMG 0 FILL
IMG IMAGE!
4 0 REGISTER: PC   4 4 REGISTER: PS   4 8 REGISTER: SAR   1 12 REGISTER: EXCCAUSE
4 16 REGISTER: EXCVADDR
/LINE 4 20 REGISTER: A0   4 24 REGISTER: A1   4 28 REGISTER: A2   4 32 REGISTER: A3
4 36 REGISTER: A4   4 40 REGISTER: A5
/LINE 4 44 REGISTER: A6   4 48 REGISTER: A7   4 52 REGISTER: A8   4 56 REGISTER: A9
4 60 REGISTER: A10   4 64 REGISTER: A11
/LINE 4 68 REGISTER: A12   4 72 REGISTER: A13   4 76 REGISTER: A14   4 80 REGISTER: A15
/LINE 4 84 REGISTER: LBEG   4 88 REGISTER: LEND   4 92 REGISTER: LCOUNT
1 96 REGISTER: WINDOWBASE   2 100 REGISTER: WINDOWSTART
/LINE 1 104 REGISTER: DEBUGCAUSE   4 108 REGISTER: CYCLES
VARIABLE FROM-FAULT                 \ true: the image holds the registers of an exception

\ The causes of an exception (Xtensa manual, table 4-64): n CAUSE: name;
\ 64 is the debug exception of the HAL (BREAK).
: CAUSE: ( n "name" -- )   C,  PARSE-NAME  DUP C,  0 ?DO  DUP I + C@ C,  LOOP  DROP ;
CREATE CAUSES
0 CAUSE: IllegalInstruction   1 CAUSE: Syscall   2 CAUSE: InstructionFetchError
3 CAUSE: LoadStoreError   4 CAUSE: Level1Interrupt   5 CAUSE: Alloca
6 CAUSE: IntegerDivideByZero   8 CAUSE: Privileged   9 CAUSE: LoadStoreAlignment
12 CAUSE: InstrPIFDataError   13 CAUSE: LoadStorePIFDataError
14 CAUSE: InstrPIFAddrError   15 CAUSE: LoadStorePIFAddrError   16 CAUSE: InstTLBMiss
17 CAUSE: InstTLBMultiHit   18 CAUSE: InstFetchPrivilege   20 CAUSE: InstFetchProhibited
24 CAUSE: LoadStoreTLBMiss   25 CAUSE: LoadStoreTLBMultiHit   26 CAUSE: LoadStorePrivilege
28 CAUSE: LoadProhibited   29 CAUSE: StoreProhibited   32 CAUSE: Coprocessor0Disabled
33 CAUSE: Coprocessor1Disabled   34 CAUSE: Coprocessor2Disabled   35 CAUSE: Coprocessor3Disabled
36 CAUSE: Coprocessor4Disabled   37 CAUSE: Coprocessor5Disabled   38 CAUSE: Coprocessor6Disabled
39 CAUSE: Coprocessor7Disabled   64 CAUSE: Debug
255 C, 0 C,  ALIGN                   \ the end
: .CAUSE ( n -- )   \ its name, or nothing
  CAUSES  BEGIN  DUP C@ 255 <> WHILE
    2DUP C@ = IF  NIP 1+ COUNT TYPE  EXIT  THEN
    1+ COUNT +
  REPEAT  2DROP ;
: .FIELD ( x shift bits a u -- x )   \ a u, "=", the field, in decimal
  TYPE ." ="  >R  OVER SWAP RSHIFT  1 R> LSHIFT 1- AND  0 .R SPACE ;
: .FLAGS-XT ( -- )   \ the fields of PS; after an exception its cause
  CR  BASE @ >R  DECIMAL  IMG 4 + @
  0 4 S" INTLEVEL" .FIELD   4 1 S" EXCM" .FIELD   5 1 S" UM" .FIELD   6 2 S" RING" .FIELD
  8 4 S" OWB" .FIELD   16 2 S" CALLINC" .FIELD   18 1 S" WOE" .FIELD  DROP
  FROM-FAULT @ IF  ."  cause: "  IMG 12 + C@ .CAUSE  THEN  R> BASE ! ;
' .FLAGS-XT IS .FLAGS

\ ==== Calls ===============================================================
\ SYSCALL calls (CALL) with CALLX8, as a function of the ROM: a2 is the
\ function, a3 the image (its chip address). (CALL) loads a10 to a15 (a2
\ to a7 of the function) from the image, calls it with CALLX8, and puts
\ them back; its own a2 to a7 stay.

CODE (CALL) ( -- )   \ not a word for the VM: a function for SYSCALL
  A1 32 ENTRY,
  A10 A3 28 L32I,  A11 A3 32 L32I,  A12 A3 36 L32I,
  A13 A3 40 L32I,  A14 A3 44 L32I,  A15 A3 48 L32I,
  A4 CCOUNT RSR,
  A2 CALLX8,
  A5 CCOUNT RSR,
  A10 A3 28 S32I,  A11 A3 32 S32I,  A12 A3 36 S32I,
  A13 A3 40 S32I,  A14 A3 44 S32I,  A15 A3 48 S32I,
  A5 A5 A4 SUB,  A5 A3 108 S32I,
  A6 SAR RSR,  A6 A3 8 S32I,
  RETW,
END-CODE

\ The special registers: a table of RSR a2, n and RET.N for each n (5
\ bytes), the same for WSR.
ALSO XTENSA
: RSR-TABLE, ( -- )   256 0 DO  A2 I RSR,  RET.N,  LOOP ;
: WSR-TABLE, ( -- )   256 0 DO  A2 I WSR,  RET.N,  LOOP ;
PREVIOUS
CODE (RSR) ( -- )   RSR-TABLE,  END-CODE
CODE (WSR) ( -- )   WSR-TABLE,  END-CODE

PUBLIC
: CALL-AT ( a -- )
  DUP IMG !  IMG >CHIP  2  [ ' (CALL) >CODE ] LITERAL  SYSCALL DROP  FROM-FAULT OFF! ;
: LAST-FAULT ( -- )   FAULT-REGS IMG 108 MOVE  FROM-FAULT ON! ;
CODE SR@ ( n -- x )
  A2 TOS@,  A2 A2 0 8 EXTUI,
  A3 A2 2 SLLI,  A3 A3 A2 ADD.N,                     \ 5 n
  A4 ' (RSR) >CODE CONST,  A3 A3 A4 ADD.N,
  A3 CALLX0,
  A2 TOS!,  NEXT,
END-CODE
CODE SR! ( x n -- )
  A2 POP,  A2 A2 0 8 EXTUI,
  A3 A2 2 SLLI,  A3 A3 A2 ADD.N,
  A4 ' (WSR) >CODE CONST,  A3 A3 A4 ADD.N,
  A2 POP,  A3 CALLX0,  ISYNC,
  NEXT,
END-CODE

\ ==== Breakpoints and single steps =======================================
\ DEBUG-AT prepares a call as CALL-AT makes it, but the function waits:
\ STEP lets it run one instruction (ICOUNT), GO up to a breakpoint
\ (IBREAKA0, IBREAKA1), a BREAK instruction or its end. It runs on a stack
\ of its own (DSTACK). While it waits, DSAVE holds its whole register file
\ (64 registers, view by view from its WINDOWBASE on) and its special
\ registers; while it runs, HSAVE holds those of the VM. SYSCALL calls
\ (DCALL), which changes the register files and goes on in the function
\ with RFI 6. A debug exception goes to (HOOK) (DEBUG-HOOK of INTR): it
\ changes the register files back and goes on in (CONT), the end of
\ (DCALL). At its end the function returns to (TRAMP-END), which does
\ the same. DSAVE and HSAVE: the 64 registers, then WINDOWBASE (cell
\ 64), WINDOWSTART, PS, PC, SAR, LBEG, LEND, LCOUNT, DEBUGCAUSE (72).

73 CONSTANT /SAVE
/SAVE CELLS BUFFER: DSAVE            \ the function that waits
/SAVE CELLS BUFFER: HSAVE            \ the VM, while the function runs
4096 CONSTANT /DSTACK
/DSTACK BUFFER: DSTACK               \ the stack of the function
VARIABLE ARM-COUNT                    \ ICOUNT for the next run; 0: no count
VARIABLE ARM-BREAKS                   \ IBREAKENABLE for the next run
VARIABLE ENDING                       \ true: the function returned (TRAMP-END)
2 CELLS BUFFER: BREAKS   VARIABLE #SET
ALSO XTENSA
: SAVE-FILE, ( -- )   \ the 64 registers to a2, view by view; a2 of the first view is in EXCSAVE6
  A0 A2 0 S32I,  A1 A2 4 S32I,  A3 A2 12 S32I,
  A3 EXCSAVE6 RSR,  A3 A2 8 S32I,
  15 0 DO                             \ a4 to a7 of a view are a0 to a3 of the next one
    A4 A2 16 S32I,  A5 A2 20 S32I,  A6 A2 24 S32I,  A7 A2 28 S32I,
    A6 A2 16 ADDI,  1 ROTW,           \ a2 of the next view: the place of its registers
  LOOP  1 ROTW, ;                     \ the first view again: a2 holds the address still
: RESTORE-FILE, ( -- )   \ the 64 registers from the address in EXCSAVE6, from the view of WINDOWBASE on
  16 0 DO
    A3 EXCSAVE6 RSR,  A2 A3 16 ADDI,  A2 EXCSAVE6 WSR,
    A0 A3 0 L32I,  A1 A3 4 L32I,  A2 A3 8 L32I,  A3 A3 12 L32I,
    1 ROTW,
  LOOP ;
: SPECIALS>, ( -- )   \ SAR LBEG LEND LCOUNT WINDOWSTART to the save at a2 (a3 is free)
  A3 SAR RSR,  A3 A2 272 S32I,   A3 LBEG RSR,  A3 A2 276 S32I,
  A3 LEND RSR,  A3 A2 280 S32I,  A3 LCOUNT RSR,  A3 A2 284 S32I,
  A3 WINDOWSTART RSR,  A3 A2 260 S32I, ;
: SPECIALS<, ( -- )   \ the same back from the save at a2; EPS6 and EPC6 from PS and PC
  A3 A2 272 L32I,  A3 SAR WSR,   A3 A2 276 L32I,  A3 LBEG WSR,
  A3 A2 280 L32I,  A3 LEND WSR,  A3 A2 284 L32I,  A3 LCOUNT WSR,
  A3 A2 260 L32I,  A3 WINDOWSTART WSR,
  A3 A2 264 L32I,  A3 EPS6 WSR,  A3 A2 268 L32I,  A3 EPC6 WSR, ;
: CHANGE, ( -- )   \ the register file of the save at a2: its WINDOWBASE, its 64 registers, RFI 6
  A2 EXCSAVE6 WSR,  A3 A2 256 L32I,  A3 WINDOWBASE WSR,  RSYNC,
  RESTORE-FILE,  6 RFI, ;
PREVIOUS

CODE (CONT) ( -- )   \ not a word for the VM: the end of (DCALL), after the function stopped
  RETW,
END-CODE
CODE (HOOK) ( -- )   \ not a word for the VM: DEBUG-HOOK; a2 is in EXCSAVE6
  A2 DSAVE >CHIP CONST,
  SAVE-FILE,                           \ the function
  A3 WINDOWBASE RSR,  A3 A2 256 S32I,
  A3 EPS6 RSR,  A3 A2 264 S32I,  A3 EPC6 RSR,  A3 A2 268 S32I,
  A3 DEBUGCAUSE RSR,  A3 A2 288 S32I,
  SPECIALS>,
  A3 0 MOVI.N,  A3 ICOUNTLEVEL WSR,  A3 IBREAKENABLE WSR,  ISYNC,   \ nothing stops it more
  A2 HSAVE >CHIP CONST,                \ the VM again
  A3 ' (CONT) >CODE CONST,  A3 A2 268 S32I,
  SPECIALS<,
  CHANGE,
END-CODE
CODE (TRAMP-END) ( -- )   \ not a word for the VM: the function returns here (CALLX8)
  A9 6 RSIL,                           \ a10 to a15: its a2 to a7
  A2 EXCSAVE6 WSR,
  A8 ENDING >CHIP CONST,  A9 1 MOVI.N,  A9 A8 0 S32I,  MEMW,
  A8 ' (HOOK) >CODE CONST,  A8 JX,
END-CODE
CODE (DCALL) ( -- )   \ not a word for the VM: a function for SYSCALL; the function of DSAVE goes on
  A1 32 ENTRY,
  A3 6 RSIL,                           \ no interrupt while the register files change
  A2 EXCSAVE6 WSR,
  A2 HSAVE >CHIP CONST,
  A3 A2 264 S32I,                      \ PS: before RSIL
  SPECIALS>,
  SAVE-FILE,                           \ the VM and this function
  A3 WINDOWBASE RSR,  A3 A2 256 S32I,
  A2 DSAVE >CHIP CONST,
  SPECIALS<,
  A3 ARM-COUNT >CHIP CONST,  A3 A3 0 L32I,  A3 ICOUNT WSR,    \ a step: ICOUNT from below 0
  A4 0 MOVI.N,  A5 1 MOVI.N,  A4 A5 A3 MOVNEZ,  A4 ICOUNTLEVEL WSR,   \ count at level 0 only
  A3 BREAKS >CHIP CONST,  A4 A3 0 L32I,  A4 IBREAKA0 WSR,  A4 A3 4 L32I,  A4 IBREAKA1 WSR,
  A3 ARM-BREAKS >CHIP CONST,  A3 A3 0 L32I,  A3 IBREAKENABLE WSR,
  ISYNC,
  CHANGE,
END-CODE

-2 CONSTANT STEP-COUNT                \ ICOUNT for one instruction
VARIABLE REASON   VARIABLE STARTED   VARIABLE SKIP
: DSAVE@ ( i -- x )   CELLS DSAVE + @ ;
: DSAVE! ( x i -- )   CELLS DSAVE + ! ;
: DSAVE>IMG ( -- )   \ the registers of the function into the image
  16 0 DO  I DSAVE@  IMG 20 + I CELLS + !  LOOP
  67 DSAVE@ IMG !  66 DSAVE@ IMG 4 + !  68 DSAVE@ IMG 8 + !  64 IMG 12 + !
  69 DSAVE@ IMG 84 + !  70 DSAVE@ IMG 88 + !  71 DSAVE@ IMG 92 + !
  64 DSAVE@ IMG 96 + !  65 DSAVE@ IMG 100 + !  72 DSAVE@ IMG 104 + !
  FROM-FAULT OFF! ;
: IMG>DSAVE ( -- )   \ a0 to a15 and PC from the image: SET-REG works while it waits
  16 0 DO  IMG 20 + I CELLS + @  I DSAVE!  LOOP  IMG @ 67 DSAVE! ;
: ?STARTED ( -- )   STARTED @ 0= ABORT" MON-ESP32: no function waits (DEBUG-AT)" ;
ALSO XTENSA
: DISARM ( -- )   0 DEBUG-HOOK !  0 ICOUNTLEVEL SR!  0 IBREAKENABLE SR! ;
PREVIOUS
: (RUN) ( count breaks -- )
  ARM-BREAKS !  ARM-COUNT !  ENDING OFF!
  [ ' (HOOK) >CODE ] LITERAL DEBUG-HOOK !
  0 [ ' (DCALL) >CODE ] LITERAL SYSCALL DROP
  0 DEBUG-HOOK ! ;
: RUN ( count breaks -- )   \ the function goes on: one step, or up to a stop
  REASON @ IF  SKIP @ IMG +!  0 SKIP !  IMG>DSAVE  THEN   \ after a BREAK: the instruction after it
  ['] (RUN) CATCH ?DUP IF  DISARM  STARTED OFF!  THROW  THEN
  ENDING @ IF
    6 0 DO  10 I + DSAVE@  IMG 28 + I CELLS + !  LOOP   \ a2 to a7: as the function left them
    0 REASON !  STARTED OFF!  EXIT
  THEN
  DSAVE>IMG
  72 DSAVE@  DUP 1 AND IF 1 ELSE 3 THEN REASON !      \ ICOUNT: a step
  DUP 8 AND IF 3 ELSE  DUP 16 AND IF 2 ELSE 0 THEN THEN SKIP !  DROP   \ BREAK, BREAK.N
  IMG @ [ ' (TRAMP-END) >CODE ] LITERAL = IF  0 0 RECURSE  THEN ;   \ a step of its RETW: the end
: AT-BREAK? ( -- f )   FALSE  #SET @ 0 ?DO  IMG @ BREAKS I CELLS + @ = OR  LOOP ;

PUBLIC
: DEBUG-AT ( a -- )
  DSAVE /SAVE CELLS 0 FILL  67 DSAVE!
  $00060020 66 DSAVE!                  \ PS: WOE, CALLINC 2 (CALLX8), UM
  1 65 DSAVE!                          \ WINDOWBASE 0, WINDOWSTART 1: the frame of the call
  DSTACK /DSTACK + 64 -                \ a1 of the frame; at a1 - 12 a1 + 32, as START does
  DUP 32 + >CHIP OVER 12 - !  >CHIP 1 DSAVE!
  [ ' (TRAMP-END) >CODE $3FFFFFFF AND $80000000 OR ] LITERAL 8 DSAVE!   \ a8: the return, CALLX8
  6 0 DO  IMG 28 + I CELLS + @  10 I + DSAVE!  LOOP   \ a10 to a15: a2 to a7 of the image
  0 REASON !  0 SKIP !  STARTED ON! ;
: STEP ( -- )   ?STARTED  STEP-COUNT 0 RUN ;
: GO ( -- )
  ?STARTED
  AT-BREAK? IF  STEP-COUNT 0 RUN  REASON @ 1 <> IF EXIT THEN  THEN   \ first the instruction there
  0  1 #SET @ LSHIFT 1-  RUN ;
: BREAK-AT ( a -- )
  #SET @ 2 = ABORT" MON-ESP32: 2 breakpoints at most"
  BREAKS #SET @ CELLS + !  1 #SET +! ;
: CLEAR-BREAKS ( -- )   0 #SET ! ;
: .BREAKS ( -- )   #SET @ 0 ?DO  CR BREAKS I CELLS + @ 8 .HEXN  LOOP ;
: STOPPED ( -- n )   REASON @ ;

PRIVATE
ALSO DISXT
: S3? ( -- f )   S" ESP32S3" FIND-NAME 0<> ;   \ the layer of the chip ESP32-S3 is in the image
: SET-UP ( -- )   \ DISASM reads cells; the code of the chip of this board
  ['] CELL-C@ DIS-FETCH  S3? IF XT-ESP32S3 ELSE XT-ESP32 THEN ;
PREVIOUS
SET-UP

END-PACKAGE
