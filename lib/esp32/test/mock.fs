\ mock.fs - the packages ROM and ASM of the ESP32 for the test on the host
\
\ The same words as lib/esp32/rom.fs and lib/esp32/esp32/asm.fs of ucf,
\ without the board. CODE assembles into a buffer of 2 KB, as on the
\ ESP32; the cell after the code field holds the chip address of the code
\ in SRAM0, from $40082000 on. END-CODE does not copy the code: it shows
\ it (C chip-address bytes), for a check against the objects of a library
\ (blob/esp32/README.md). HOST! is CODE! of CLINK (lib/esp32/clink.fs) for
\ the test: a cell of data in this system gets x, a cell of code is shown
\ (P chip-address x), as a later change of the code. With QUIET on, both
\ show nothing (a test with the tester). >CHIP gives the chip addresses
\ of the ESP32: VM addresses 0 to $FFFF at $3FFE8000 + address (SRAM1),
\ the others at $3FFA0000 + address (SRAM2, from $3FFB0000 on).

PACKAGE ROM   \ lib/package.fs
PUBLIC
: >CHIP ( a -- a' )   DUP $40000 U< IF  DUP $10000 U< IF $3FFE8000 ELSE $3FFA0000 THEN +  THEN ;
: SYSCALL ( x1 .. xn n a -- x )   TRUE ABORT" SYSCALL: not on the host" ;
END-PACKAGE

PACKAGE ASM  ALSO XTENSA  ALSO ROM
$3FFE8000 $3FFA0000 - DATA-OFF !    \ as lib/esp32/esp32/asm.fs on the board
TRUE FLASH-DICT? !
PUBLIC
VARIABLE QUIET                      \ true: END-CODE and HOST! show nothing
: H8 ( u -- )   BASE @ >R HEX 0 <# # # # # # # # # #> TYPE R> BASE ! ;
: H2 ( c -- )   BASE @ >R HEX 0 <# # # #> TYPE R> BASE ! ;
PRIVATE
2048 CONSTANT #BUF                  \ as on the ESP32: at most 2 KB for one CODE word
#BUF BUFFER: BUF
VARIABLE BP                         \ the next byte in BUF
$40082000 CONSTANT CODE-START
VARIABLE IHERE  CODE-START IHERE !  \ the chip address of the next code
: ASSEMBLE ( -- )   BUF BP !  IHERE @ BUF -  BP  BUF #BUF +  IN-PLACE ;
PUBLIC
: CODE ( "name" -- )   PARSE-NAME HEADER  66 ,  IHERE @ ,  ASSEMBLE  ALSO XTENSA ;
: END-CODE ( -- )
  CHECK-LABELS  QUIET @ 0= IF  ." C " IHERE @ H8 SPACE  BP @ BUF ?DO I C@ H2 LOOP CR  THEN
  BP @ BUF - 3 + -4 AND IHERE +!  PREVIOUS  REVEAL ;
: >CODE ( xt -- a )   CELL+ @ ;
: CODE-SIZE ( -- u )   IHERE @ CODE-START - ;
: HOST! ( x a -- )   \ CODE! of the test
  DUP $40000000 U< IF  DUP $3FFE8000 U< IF $3FFA0000 ELSE $3FFE8000 THEN -  !  EXIT THEN
  QUIET @ IF  2DROP EXIT  THEN  ." P " H8 SPACE H8 CR ;
END-PACKAGE
