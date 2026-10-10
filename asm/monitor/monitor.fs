\ monitor.fs - package MONITOR: the common part of the monitor
\
\ The monitor looks into the machine that uCForth runs on: its
\ registers, its code, calls of its BIOS or ROM (asm/monitor/README.md).
\ This package is the part that is the same for each CPU and platform:
\
\   .REGS ( -- )            the registers of the image, then the flags
\   x SET-REG name ( -- )   the register name of the image gets x
\   GET-REG name ( -- x )   the register name of the image
\   a n DISASM ( -- a' )    n instructions from a; a' is the next one,
\                           so "16 DISASM" goes on
\   xt DIS-FETCH ( -- )     DISASM reads each byte of the code with
\                           xt ( a -- c ); "' C@ DIS-FETCH": the memory
\                           of the VM, as after the load
\
\ The image holds the registers for the next call, and after a call the
\ registers that it left. The part of a platform (monpc.fs ...) makes
\ the image, its registers and the calls; the part of a CPU (disx86.fs
\ ...) the disassembler. They use these words:
\
\   a IMAGE! ( -- )                 the image is at a
\   size offset REGISTER: name      a register of the image: size 1, 2 or
\                                   4 bytes (not more than a cell), at offset;
\                                   the name has up to 11 characters
\   /LINE ( -- )                    .REGS starts a new line before the
\                                   next register
\   .FLAGS ( -- )                   (vector) the flags, after the registers
\   DECODE ( a -- a' )              (vector) the instruction at a as text
\                                   in TXT; a' is the next instruction
\   DIS-C@ ( a -- c )               (vector) a byte of the code: C@, or a
\                                   word for other memory (monpc.fs)
\   .ADDR ( a -- )                  (vector) the address at the start of a line;
\                                   (.ADDR) shows it with ADDR-DIGITS hex digits
\   .CODE ( a a' -- )               (vector) the code of the instruction from a
\                                   up to a'; (.BYTES) shows its bytes
\   ADDR-DIGITS ( -- a )            the hex digits of an address (4 or 8)
\   u n .HEXN ( -- )                u with at least n hex digits, lower case
\   TXT ( -- a )                    the text: a counted string
\   TXT-START  a u +T  c +C         make the text
\   ud +HEX  d +SHEX                a number: 0x and hex digits, lowercase;
\                                   +SHEX with "-" if d is negative
\
\ Load (needs nothing else):  ALSO FILES  INCLUDE asm/monitor/monitor.fs

PACKAGE MONITOR   \ lib/package.fs

\ ==== The text of an instruction ========================================

PUBLIC
82 BUFFER: TXT                      \ a counted string: up to 80 characters
: TXT-START ( -- )   0 TXT C! ;
: +C ( c -- )   TXT C@ 80 < IF  TXT COUNT + C!  TXT C@ 1+ TXT C!  ELSE DROP THEN ;
: +T ( a u -- )   0 ?DO  DUP I + C@ +C  LOOP  DROP ;
PRIVATE
: LOWER ( c -- c' )   DUP 'A' 'Z' 1+ WITHIN IF 32 + THEN ;
: HEXN ( ud n -- a u )   \ ud as hex digits, at least n
  BASE @ >R  HEX  >R <#  R> 0 ?DO # LOOP  BEGIN 2DUP OR WHILE # REPEAT  #>  R> BASE ! ;
PUBLIC
: +HEX ( ud -- )   S" 0x" +T  1 HEXN  0 ?DO  DUP I + C@ LOWER +C  LOOP  DROP ;
: +SHEX ( d -- )   DUP 0< IF  S" -" +T  DNEGATE  THEN  +HEX ;

\ ==== The register image ================================================
\ A table of the registers: for each its name (a counted string, up to 11
\ characters), its offset in the image, its size in bytes, and true if
\ .REGS starts a new line before it.

PRIVATE
32 CONSTANT #REGS
15 CONSTANT /ENTRY                  \ 12 bytes for the name, the offset, the size, the new line
CREATE REG-TABLE  #REGS /ENTRY * ALLOT
VARIABLE REG-COUNT
VARIABLE THE-IMAGE
VARIABLE NEW-LINE                   \ true: the next register starts a new line
: ENTRY ( i -- a )   /ENTRY * REG-TABLE + ;
: R-SIZE ( i -- n )   ENTRY 13 + C@ ;
: R-ADDR ( i -- a )   ENTRY 12 + C@  THE-IMAGE @ + ;
: R-LINE? ( i -- f )   ENTRY 14 + C@ 0<> ;
: BYTES@ ( a n -- x )   \ n bytes, the low byte first
  0 SWAP  1- 0 SWAP DO  8 LSHIFT  OVER I + C@ OR  -1 +LOOP  NIP ;
: BYTES! ( x a n -- )   0 ?DO  2DUP C!  SWAP 8 RSHIFT SWAP 1+  LOOP  2DROP ;
: REG= ( a u i -- f )   ENTRY COUNT  ROT OVER = IF SAME? ELSE 2DROP DROP FALSE THEN ;
: FIND-REG ( "name" -- i )   \ the register with this name; upper and lower case are the same
  PARSE-NAME  REG-COUNT @ 0 ?DO
    2DUP I REG= IF  2DROP I UNLOOP EXIT  THEN
  LOOP  2DROP  TRUE ABORT" MONITOR: no such register" ;
: .REG ( i -- )   \ NAME=hex digits, as many as the register has
  DUP R-LINE? IF CR THEN
  DUP ENTRY COUNT TYPE  ." ="
  DUP R-ADDR OVER R-SIZE BYTES@  0  ROT R-SIZE 2* HEXN TYPE  SPACE ;

PUBLIC
: IMAGE! ( a -- )   THE-IMAGE ! ;
: /LINE ( -- )   TRUE NEW-LINE ! ;
: REGISTER: ( size offset "name" -- )
  REG-COUNT @ #REGS < 0= ABORT" MONITOR: too many registers"
  REG-COUNT @ ENTRY >R
  R@ 12 + C!  R@ 13 + C!  NEW-LINE @ R@ 14 + C!  FALSE NEW-LINE !
  PARSE-NAME 11 MIN  DUP R@ C!  R> 1+ SWAP MOVE
  1 REG-COUNT +! ;
DEFER .FLAGS ( -- )   ' CR IS .FLAGS
: .REGS ( -- )   REG-COUNT @ 0 ?DO  I .REG  LOOP  .FLAGS ;
: SET-REG ( x "name" -- )   FIND-REG  DUP R-ADDR SWAP R-SIZE BYTES! ;
: GET-REG ( "name" -- x )   FIND-REG  DUP R-ADDR SWAP R-SIZE BYTES@ ;

\ ==== DISASM =============================================================

DEFER DECODE ( a -- a' )   ' NOOP IS DECODE
DEFER DIS-C@ ( a -- c )    ' C@ IS DIS-C@
: DIS-FETCH ( xt -- )   IS DIS-C@ ;   \ DISASM reads each byte of the code with xt ( a -- c )
VARIABLE ADDR-DIGITS   CELL 2* ADDR-DIGITS !
: .HEXN ( u n -- )   \ u with at least n hex digits, lowercase
  0 SWAP HEXN  0 ?DO  DUP I + C@ LOWER EMIT  LOOP  DROP ;
: (.ADDR) ( a -- )   ADDR-DIGITS @ .HEXN ;
DEFER .ADDR ( a -- )   ' (.ADDR) IS .ADDR
PRIVATE
8 CONSTANT #SHOWN                   \ the bytes of an instruction in a line; a longer one shows its first 8
PUBLIC
: (.BYTES) ( a a' -- )   \ the bytes from a up to a', each with a space, in a column of #SHOWN bytes
  OVER - #SHOWN MIN  DUP >R
  0 ?DO  DUP I + DIS-C@ 2 .HEXN SPACE  LOOP  DROP
  #SHOWN R> - 3 * SPACES ;
DEFER .CODE ( a a' -- )   ' (.BYTES) IS .CODE
: DISASM ( a n -- a' )
  0 ?DO
    CR  DUP .ADDR  2 SPACES  DUP DECODE  2DUP .CODE  TXT COUNT TYPE  NIP
  LOOP ;

END-PACKAGE
