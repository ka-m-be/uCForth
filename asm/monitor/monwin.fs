\ monwin.fs - package MON-WIN32: the monitor on Windows (32 bits)
\
\ The part of the monitor for uCForth on Windows (targets/win32,
\ asm/monitor/README.md): the registers of the 386, calls of code at any
\ address with them, code of your own in memory that Windows lets run,
\ and the memory of the whole program, not only that of the VM.
\
\   x1 .. xn n a CALL-AT ( -- )   a call of the code at the address a
\                        (for Windows) with the registers of the image
\                        and n arguments on the stack, as SYSCALL takes
\                        them (x1 is the first); afterwards the image
\                        holds the registers and the flags that the code
\                        left
\   u EXEC-ALLOT ( -- xa )   u bytes of memory that Windows lets run, read
\                        and write (VirtualAlloc); xa is an address for
\                        Windows
\   PLACE-CODE ( -- )    the code that the assembler X86 made since ORG
\                        goes to its place: ORG takes the address from
\                        EXEC-ALLOT
\   a X-C@ ( -- c )      a byte at a VM address or at an address for
\                        Windows; DISASM reads the code with it (so after
\                        the load)
\   a u DUMPX ( -- )     u bytes from a, as DUMP shows them, at a VM
\                        address or an address for Windows
\
\ The image has EAX EBX ECX EDX ESI EDI EBP and the flags EFL (.REGS,
\ SET-REG and GET-REG of the package MONITOR). ESP and the segments stay
\ as Windows has them; EFL is only a result. Your own code ends with RET:
\
\   ok> 64 EXEC-ALLOT CONSTANT MY-CODE
\   ok> MY-CODE ORG  EAX EBX MOV,  EAX 4 [ESP] ADD,  RET,  PLACE-CODE
\   ok> 3 SET-REG EBX  7 1 MY-CODE CALL-AT  GET-REG EAX .
\   10
\
\ An exception in a call (Windows reads at a wrong address) gives the
\ error -9, as in SYSCALL; the program goes on.
\
\ Needs TEXT, X86, WINDOWS, MONITOR and DISX86, on Windows:
\   ALSO FILES  INCLUDE lib/text.fs  INCLUDE asm/x86.fs  INCLUDE lib/windows/windows.fs
\   INCLUDE asm/monitor/monitor.fs  INCLUDE asm/monitor/disx86.fs
\   INCLUDE asm/monitor/monwin.fs

PACKAGE MON-WIN32  ALSO X86  ALSO WINDOWS  ALSO MONITOR   \ lib/package.fs

PRIVATE
LIBRARY KERNEL32.dll
4 WINAPI: VirtualAlloc
3 WINAPI: RtlMoveMemory

\ ==== Memory ==============================================================

PUBLIC
: EXEC-ALLOT ( u -- xa )   \ MEM_COMMIT MEM_RESERVE, PAGE_EXECUTE_READWRITE
  0 SWAP $3000 $40 VirtualAlloc  DUP 0= ABORT" EXEC-ALLOT: no memory" ;
: PLACE-CODE ( -- )   \ the bytes of TARGET to the address of ORG
  CHECK-LABELS  TORG @  TARGET >CHIP  TDP @  RtlMoveMemory DROP ;
PRIVATE
VARIABLE ONE-BYTE
PUBLIC
: X-C@ ( a -- c )   ONE-BYTE >CHIP  SWAP >CHIP  1 RtlMoveMemory DROP  ONE-BYTE C@ ;
: DUMPX ( a u -- )   \ as DUMP, at any address
  OVER + SWAP
  BEGIN  2DUP U> WHILE
    CR DUP 8 .HEXN ." : "
    16 0 DO  DUP I + X-C@ 2 .HEXN SPACE  LOOP
    16 0 DO  DUP I + X-C@  DUP 32 127 WITHIN 0= IF DROP 46 THEN  EMIT  LOOP
    16 +
  REPEAT  2DROP ;

\ ==== The image ===========================================================

PRIVATE
CREATE IMG  32 ALLOT   IMG 32 0 FILL   \ EAX EBX ECX EDX ESI EDI EBP EFL, a cell each
IMG IMAGE!
4 0 REGISTER: EAX   4 4 REGISTER: EBX   4 8 REGISTER: ECX   4 12 REGISTER: EDX
4 16 REGISTER: ESI  4 20 REGISTER: EDI  4 24 REGISTER: EBP  4 28 REGISTER: EFL
: .FLAG ( fl bit a u -- fl )   \ a u: the text when the flag is set, then when it is clear
  DROP >R  OVER SWAP RSHIFT 1 AND 0= 2 AND  R> +  2 TYPE SPACE ;
: .FLAGS-386 ( -- )   \ as DEBUG shows them
  CR  IMG 28 + @
  11 S" OVNV" .FLAG  10 S" DNUP" .FLAG  9 S" EIDI" .FLAG  7 S" NGPL" .FLAG
  6 S" ZRNZ" .FLAG  4 S" ACNA" .FLAG  2 S" PEPO" .FLAG  0 S" CYNC" .FLAG  DROP ;
' .FLAGS-386 IS .FLAGS

\ ==== Calls ===============================================================
\ SYSCALL calls (CALL) with the n arguments. (CALL) keeps the registers
\ that Windows wants kept (EBX ESI EDI EBP) and ESP, pushes the arguments
\ again, loads the registers of the image, calls the code, and puts the
\ registers and the flags back into the image. Its code lies in memory
\ from EXEC-ALLOT; it reaches the image and the variables at their
\ addresses for Windows.

VARIABLE 'TARGET   VARIABLE 'N   VARIABLE 'ESP
: IMG@ ( n -- xa )   IMG + >CHIP ;
64 EXEC-ALLOT CONSTANT (CALL)
(CALL) ORG
  EBP PUSH,  EBP ESP MOV,  EBX PUSH,  ESI PUSH,  EDI PUSH,
  'ESP >CHIP [] ESP MOV,
  ECX 'N >CHIP [] MOV,  ECX ECX TEST,  NZ IF,      \ xn first, x1 last
    BEGIN,  4 [EBP] [ECX*4] PUSH,  ECX DEC,  Z UNTIL,
  THEN,
  EAX 0 IMG@ [] MOV,   EBX 4 IMG@ [] MOV,   ECX 8 IMG@ [] MOV,   EDX 12 IMG@ [] MOV,
  ESI 16 IMG@ [] MOV,  EDI 20 IMG@ [] MOV,  EBP 24 IMG@ [] MOV,
  'TARGET >CHIP [] CALL-IND,
  PUSHFD,  28 IMG@ [] POP,
  0 IMG@ [] EAX MOV,   4 IMG@ [] EBX MOV,   8 IMG@ [] ECX MOV,   12 IMG@ [] EDX MOV,
  16 IMG@ [] ESI MOV,  20 IMG@ [] EDI MOV,  24 IMG@ [] EBP MOV,
  ESP 'ESP >CHIP [] MOV,  EDI POP,  ESI POP,  EBX POP,  EBP POP,  RET,
PLACE-CODE

PUBLIC
: CALL-AT ( x1 .. xn n a -- )   'TARGET !  DUP 'N !  (CALL) SYSCALL DROP ;

PRIVATE
ALSO DISX86
: SET-UP ( -- )   ['] X-C@ DIS-FETCH  X86-32 ;   \ DISASM reads any address, 32-bit code
PREVIOUS
SET-UP

END-PACKAGE
