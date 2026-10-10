\ monpc.fs - package MON-PC: the monitor on a PC (DOS, or without an operating system)
\
\ The part of the monitor for a PC in real mode (asm/monitor/README.md):
\ the registers of the 8086, calls of the BIOS and of code anywhere in
\ the memory, memory with segment and offset. Its words are CODE words
\ (lib/pc/asm.fs).
\
\   n INT-CALL ( -- )            INT n with the registers of the image;
\                                afterwards the image holds the registers
\                                and the flags that INT n left
\   seg off CALL-AT ( -- )       a far call of the code at seg:off with the
\                                registers of the image; the code ends with
\                                RETF; afterwards the image as for INT-CALL
\   seg off @L ( -- x )   x seg off !L ( -- )      a word in the memory of the PC
\   seg off C@L ( -- c )  c seg off C!L ( -- )     a byte
\   seg off u DUMPL ( -- )       u bytes from seg:off, as DUMP shows them
\   seg off n DISASML ( -- seg off' )   n instructions from seg:off
\   VM-SEG ( -- seg )            the segment of the VM: a VM address a is
\                                the address VM-SEG:a
\
\ Breakpoints and single steps, as DEBUG of DOS has them (G and T):
\
\   seg off DEBUG-AT ( -- )      the code at seg:off waits for STEP or GO,
\                                with the registers of the image and a
\                                stack of its own; it ends with RETF
\   STEP ( -- )                  the code runs one instruction (the trap
\                                flag)
\   GO ( -- )                    the code runs up to a breakpoint, or to
\                                its end
\   seg off BREAK-AT ( -- )      a breakpoint at seg:off (INT 3), at most 8
\   CLEAR-BREAKS ( -- )          no breakpoints
\   .BREAKS ( -- )               the breakpoints
\   STOPPED ( -- n )             why the code stopped: 1 after a step, 3 at
\                                a breakpoint, 0 at its end
\
\ The image has AX BX CX DX SI DI BP DS ES and the flags FL (.REGS,
\ SET-REG, GET-REG of the package MONITOR), and CS IP SS SP of the code
\ that DEBUG-AT prepared. DS and ES are VM-SEG at the start: then an
\ address in BX, DX, SI or DI is a VM address. For INT-CALL and CALL-AT,
\ FL is only a result: a call starts with the flags of uCForth; STEP and
\ GO load FL too (DEBUG-AT sets $0202: interrupts on).
\
\   ok> $0E41 SET-REG AX  $10 INT-CALL       \ "A" on the screen (BIOS)
\   ok> $12 INT-CALL  GET-REG AX .           \ the memory below 640 KB, in KB
\   ok> $F000 $FFF0 4 DISASML 2DROP          \ the reset vector of the BIOS
\   ok> VM-SEG MY-CODE DEBUG-AT  STEP  .REGS \ one instruction of the code at MY-CODE
\
\ A call runs on a stack of its own (512 bytes), not on the data stack.
\ It keeps IP, RP and the data stack of the VM, and sets DS and ES back
\ to VM-SEG. What else the code changes (the memory, the vectors, the
\ ports) stays changed: a wrong call can stop the PC.
\
\ Needs TEXT, I8086, ASM, MONITOR and DISX86. On DOS:
\   ALSO FILES  INCLUDE lib/text.fs  INCLUDE asm/8086.fs  INCLUDE lib/pc/asm.fs
\   INCLUDE asm/monitor/monitor.fs  INCLUDE asm/monitor/disx86.fs
\   INCLUDE asm/monitor/monpc.fs

PACKAGE MON-PC  ALSO ASM  ALSO I8086  ALSO MONITOR   \ lib/package.fs

\ ==== Memory with segment and offset ====================================
\ ES reaches the other segment for one instruction; then it holds the
\ segment of the VM again, as the kernel needs it.

PUBLIC
CODE @L ( seg off -- x )    BX POP,  ES POP,  ES: AX 0 [BX] MOV,  CX DS MOV,  ES CX MOV,  AX PUSH,  NEXT,  END-CODE
CODE !L ( x seg off -- )    BX POP,  ES POP,  AX POP,  ES: 0 [BX] AX MOV,  CX DS MOV,  ES CX MOV,  NEXT,  END-CODE
CODE C@L ( seg off -- c )   BX POP,  ES POP,  AX AX XOR,  ES: AL 0 [BX] MOV,  CX DS MOV,  ES CX MOV,  AX PUSH,  NEXT,  END-CODE
CODE C!L ( c seg off -- )   BX POP,  ES POP,  AX POP,  ES: 0 [BX] AL MOV,  CX DS MOV,  ES CX MOV,  NEXT,  END-CODE
CODE VM-SEG ( -- seg )      DS PUSH,  NEXT,  END-CODE

: DUMPL ( seg off u -- )
  DUP 4 RSHIFT  SWAP 15 AND 0<> -  0 ?DO          \ lines of 16 bytes
    CR  OVER 4 .HEXN  ':' EMIT  DUP 4 .HEXN  ." : "
    16 0 DO  2DUP I + C@L 2 .HEXN SPACE  LOOP
    16 0 DO  2DUP I + C@L  DUP 32 127 WITHIN 0= IF DROP 46 THEN  EMIT  LOOP
    16 +
  LOOP  2DROP ;

\ ==== The image ==========================================================

PRIVATE
CREATE IMG  28 ALLOT   IMG 28 0 FILL   \ AX BX CX DX SI DI BP DS ES FL IP CS SP SS, a word each
IMG IMAGE!
2 0 REGISTER: AX   2 2 REGISTER: BX   2 4 REGISTER: CX   2 6 REGISTER: DX
2 8 REGISTER: SI   2 10 REGISTER: DI  2 12 REGISTER: BP  2 14 REGISTER: DS
2 16 REGISTER: ES  2 18 REGISTER: FL
/LINE 2 22 REGISTER: CS   2 20 REGISTER: IP   2 26 REGISTER: SS   2 24 REGISTER: SP
VM-SEG IMG 14 + !  VM-SEG IMG 16 + !

: .FLAG ( fl bit a u -- fl )   \ a u: the text when the flag is set, then when it is clear
  DROP >R  OVER SWAP RSHIFT 1 AND 0= 2 AND  R> +  2 TYPE SPACE ;
: .FLAGS-PC ( -- )   \ as DEBUG shows them
  CR  IMG 18 + @
  11 S" OVNV" .FLAG  10 S" DNUP" .FLAG  9 S" EIDI" .FLAG  7 S" NGPL" .FLAG
  6 S" ZRNZ" .FLAG  4 S" ACNA" .FLAG  2 S" PEPO" .FLAG  0 S" CYNC" .FLAG  DROP ;
' .FLAGS-PC IS .FLAGS

\ ==== The calls ==========================================================
\ While a CODE word runs, CS is the segment of the VM: "CS:" reaches the
\ image and the saved registers also when DS holds another segment.

CREATE SAVED  8 ALLOT                \ SI BP SP of the VM during a call, its flags (STEP, GO)
CREATE CALL-STACK  512 ALLOT         \ the stack of a call
CREATE FAR-PTR  4 ALLOT              \ CALL-AT: the offset, then the segment
: SAVE-VM, ( -- )   \ IP, RP and the data stack of the VM to SAVED; SP to the stack of the call
  CS: SAVED [] SI MOV,  CS: SAVED 2 + [] BP MOV,  CS: SAVED 4 + [] SP MOV,
  SP CALL-STACK 512 + MOVI, ;
: LOAD-IMAGE, ( -- )   \ the registers from the image; DS last
  CS: AX IMG [] MOV,       CS: BX IMG 2 + [] MOV,   CS: CX IMG 4 + [] MOV,
  CS: DX IMG 6 + [] MOV,   CS: SI IMG 8 + [] MOV,   CS: DI IMG 10 + [] MOV,
  CS: BP IMG 12 + [] MOV,  CS: ES IMG 16 + [] MOV,  CS: DS IMG 14 + [] MOV, ;
: STORE-IMAGE, ( -- )   \ the flags and the registers after the call into the image
  PUSHF,  CS: IMG 18 + [] POP,
  CS: IMG [] AX MOV,       CS: IMG 2 + [] BX MOV,   CS: IMG 4 + [] CX MOV,
  CS: IMG 6 + [] DX MOV,   CS: IMG 8 + [] SI MOV,   CS: IMG 10 + [] DI MOV,
  CS: IMG 12 + [] BP MOV,  CS: IMG 14 + [] DS MOV,  CS: IMG 16 + [] ES MOV, ;
: RESTORE-VM, ( -- )   \ DS, ES, SS: the segment of the VM; IP, RP, the data stack; CLD
  AX CS MOV,  DS AX MOV,  ES AX MOV,
  SS AX MOV,  CS: SP SAVED 4 + [] MOV,              \ no interrupt between the two
  CS: BP SAVED 2 + [] MOV,  CS: SI SAVED [] MOV,  CLD, ;

PUBLIC
CODE INT-CALL ( n -- )
  AX POP,  DI L1 MOVI,  CS: 1 [DI] AL MOV,          \ n into the instruction INT below
  SAVE-VM,  LOAD-IMAGE,
  LABEL L1  $21 INT,
  STORE-IMAGE,  RESTORE-VM,  NEXT,  END-CODE
CODE CALL-AT ( seg off -- )
  CS: FAR-PTR [] POP,  CS: FAR-PTR 2 + [] POP,
  SAVE-VM,  LOAD-IMAGE,  CS: FAR-PTR [] CALL-FAR-IND,
  STORE-IMAGE,  RESTORE-VM,  NEXT,  END-CODE

\ ==== Breakpoints and single steps =======================================
\ DEBUG-AT prepares the code: CS:IP and SS:SP in the image, on its stack
\ the way back for its RETF at the end, FL $0202. (RESUME) lets it run:
\ SS:SP of the code, FL (with the trap flag for STEP), CS and IP on its
\ stack, the registers of the image, IRET. While it runs, the vectors 1
\ (single step) and 3 (INT 3, the byte $CC of a breakpoint) point to
\ (STEP) and (BREAK); its RETF at the end goes to (ENDED). Each of them
\ puts the registers of the code into the image, and goes back to the VM
\ as if (RESUME) ended: SAVED holds IP, RP, the data stack and the flags
\ of the VM (an interrupt turns the interrupts off).
\ The code waits for the next STEP or GO on its own stack.

PRIVATE
CREATE DEBUG-STACK  512 ALLOT         \ the stack of the code that DEBUG-AT prepared
VARIABLE TRAP                         \ $100: the trap flag for the next run
VARIABLE REASON                       \ the last stop: 0 the end, 1 a step, 3 a breakpoint
VARIABLE STARTED                      \ true: code waits for STEP or GO
: STOP, ( reason -- )   \ the code stopped: its registers into the image; back to the VM
  >R
  CS: IMG [] AX MOV,       CS: IMG 2 + [] BX MOV,   CS: IMG 4 + [] CX MOV,
  CS: IMG 6 + [] DX MOV,   CS: IMG 8 + [] SI MOV,   CS: IMG 10 + [] DI MOV,
  CS: IMG 12 + [] BP MOV,  CS: IMG 14 + [] DS MOV,  CS: IMG 16 + [] ES MOV,
  R@ IF                                             \ an interrupt: IP, CS, FL on the stack
    CS: IMG 20 + [] POP,  CS: IMG 22 + [] POP,  CS: IMG 18 + [] POP,
    CS: IMG 18 + [] $FEFF ANDI,                     \ the flags without the trap flag
  ELSE  PUSHF,  CS: IMG 18 + [] POP,  THEN          \ RETF at the end
  R@ 3 = IF  CS: IMG 20 + [] DEC,  THEN             \ INT 3: IP of the breakpoint
  CS: IMG 24 + [] SP MOV,  CS: IMG 26 + [] SS MOV,
  CS: REASON [] R> MOVI,
  RESTORE-VM,  CS: SAVED 6 + [] PUSH,  POPF,         \ the flags of the VM: interrupts on again
  NEXT, ;
CODE (STEP) ( -- )    1 STOP,  END-CODE       \ not words for the VM: the handler of INT 1,
CODE (BREAK) ( -- )   3 STOP,  END-CODE       \ of INT 3,
CODE (ENDED) ( -- )   0 STOP,  END-CODE       \ and the way back of the RETF at the end
CODE (RESUME) ( -- )   \ the code of the image goes on
  CS: SAVED [] SI MOV,  CS: SAVED 2 + [] BP MOV,  CS: SAVED 4 + [] SP MOV,
  PUSHF,  CS: SAVED 6 + [] POP,
  CLI,  CS: SS IMG 26 + [] MOV,  CS: SP IMG 24 + [] MOV,
  CS: AX IMG 18 + [] MOV,  CS: AX TRAP [] OR,  AX PUSH,
  CS: IMG 22 + [] PUSH,  CS: IMG 20 + [] PUSH,
  LOAD-IMAGE,  IRET,
END-CODE

CREATE OLD-VECTORS  8 ALLOT           \ the vectors 1 and 3 before the run
: VECTORS! ( -- )   \ the vectors 1 and 3 to (STEP) and (BREAK)
  0 4 @L OLD-VECTORS !  0 6 @L OLD-VECTORS 2 + !  0 12 @L OLD-VECTORS 4 + !  0 14 @L OLD-VECTORS 6 + !
  ['] (STEP) >CODE 0 4 !L  VM-SEG 0 6 !L  ['] (BREAK) >CODE 0 12 !L  VM-SEG 0 14 !L ;
: VECTORS-BACK ( -- )
  OLD-VECTORS @ 0 4 !L  OLD-VECTORS 2 + @ 0 6 !L  OLD-VECTORS 4 + @ 0 12 !L  OLD-VECTORS 6 + @ 0 14 !L ;
: ENDED? ( -- f )   \ a step stopped at (ENDED): the RETF at the end was the step
  IMG 20 + @ ['] (ENDED) >CODE =  IMG 22 + @ VM-SEG = AND ;
: RUN ( trap -- )   \ the code runs: one step ($100), or up to a stop (0)
  TRAP !  VECTORS!  (RESUME)  VECTORS-BACK
  REASON @ 1 =  ENDED? AND IF  0 REASON !  THEN
  REASON @ 0= IF  FALSE STARTED !  THEN ;
: ?STARTED ( -- )   STARTED @ 0= ABORT" MON-PC: no code waits (DEBUG-AT)" ;

8 CONSTANT #BREAKS
CREATE BREAKS  #BREAKS 6 * ALLOT       \ each: seg, off, the byte of the code under $CC
VARIABLE #SET
: BREAK# ( i -- a )   6 * BREAKS + ;
: BREAKS-IN ( -- )   \ $CC at each breakpoint; the byte of the code to BREAKS
  #SET @ 0 ?DO  I BREAK# >R  R@ @ R@ 2 + @ C@L  R@ 4 + C!  $CC R@ @ R> 2 + @ C!L  LOOP ;
: BREAKS-OUT ( -- )   \ the bytes of the code back, the last breakpoint first
  #SET @ 0 ?DO  #SET @ 1- I - BREAK# >R  R@ 4 + C@  R@ @ R> 2 + @ C!L  LOOP ;
: AT-BREAK? ( -- f )   \ CS:IP of the image is a breakpoint
  FALSE  #SET @ 0 ?DO
    I BREAK# DUP @ IMG 22 + @ =  SWAP 2 + @ IMG 20 + @ = AND OR
  LOOP ;

PUBLIC
: DEBUG-AT ( seg off -- )
  IMG 20 + !  IMG 22 + !
  DEBUG-STACK 512 + 4 -  DUP IMG 24 + !  VM-SEG IMG 26 + !
  ['] (ENDED) >CODE OVER !  VM-SEG SWAP 2 + !      \ the way back for RETF: IP, then CS
  $0202 IMG 18 + !  0 REASON !  TRUE STARTED ! ;
: STEP ( -- )   ?STARTED  $100 RUN ;
: GO ( -- )
  ?STARTED
  AT-BREAK? IF  $100 RUN  REASON @ 1 <> IF EXIT THEN  THEN   \ first the instruction under the breakpoint
  BREAKS-IN  0 RUN  BREAKS-OUT ;
: BREAK-AT ( seg off -- )
  #SET @ #BREAKS = ABORT" MON-PC: 8 breakpoints at most"
  #SET @ BREAK# >R  R@ 2 + !  R> !  1 #SET +! ;
: CLEAR-BREAKS ( -- )   0 #SET ! ;
: .BREAKS ( -- )   #SET @ 0 ?DO  CR I BREAK# DUP @ 4 .HEXN  ':' EMIT  2 + @ 4 .HEXN  LOOP ;
: STOPPED ( -- n )   REASON @ ;

\ ==== Disassembling with segment and offset ==============================

PRIVATE
VARIABLE CUR-SEG
: SEG-C@ ( off -- c )   CUR-SEG @ SWAP C@L ;
: .SEG-ADDR ( off -- )   CUR-SEG @ 4 .HEXN  ':' EMIT  4 .HEXN ;
PUBLIC
: DISASML ( seg off n -- seg off' )
  ROT CUR-SEG !  ['] SEG-C@ DIS-FETCH  ['] .SEG-ADDR IS .ADDR
  ['] DISASM CATCH  ['] C@ DIS-FETCH  ['] (.ADDR) IS .ADDR  THROW
  CUR-SEG @ SWAP ;

END-PACKAGE
