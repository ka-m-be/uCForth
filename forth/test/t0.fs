\ t0.fs - kernel conformance test of uCForth
\
\ Each virtual machine must pass this test: every bootstrap VM in
\ bootstrap/ and every kernel in kernels/ (doc/vm.md). On a board the
\ file goes over the serial console, after forth/test/tester.fs. The
\ output must be the same everywhere: an empty line from the TX! test,
\ then the line "t0: n tests, 0 errors".
\
\ The file tests each primitive 0-65 (doc/vm.md) with its edge
\ cases, the four code field types and the HAL words MILLIS MS US BIS!
\ BIC!. It does not touch I/O registers. The tests work with 16-bit and
\ 32-bit cells. They expect the low byte first in memory (little endian),
\ as on all targets so far.
\
\ Load:  bash bootstrap/bash/ucf.sh forth/test/tester.fs forth/test/t0.fs

ONLY FORTH DEFINITIONS  ALSO INTERNALS
DECIMAL

8 BUFFER: T0-BUF                 \ RAM for memory tests: two cells or more
VARIABLE T0-STEP                 \ step for the +LOOP tests

\ ==== Primitive numbers in the code field ====
\ A primitive has its number in the code field. An optional primitive
\ (32-65) can also be a Forth word from base.fs: code field 0 (DOCOL).

: CF  ( xt -- n )     @ ;
: CF? ( xt n -- n )   SWAP @ ?DUP IF NIP THEN ;

T{ ' EXIT CF  ' LIT CF  ' BRANCH CF  ' ?BRANCH CF  ' EXECUTE CF -> 4 5 6 7 8 }T
T{ ' @ CF  ' ! CF  ' C@ CF  ' C! CF -> 9 10 11 12 }T
T{ ' DUP CF  ' DROP CF  ' SWAP CF  ' OVER CF -> 13 14 15 16 }T
T{ ' >R CF  ' R> CF  ' R@ CF -> 17 18 19 }T
T{ ' SP@ CF  ' SP! CF  ' RP@ CF  ' RP! CF -> 20 21 22 23 }T
T{ ' AND CF  ' OR CF  ' XOR CF  ' 0< CF -> 24 25 26 27 }T
T{ ' UM+ CF  ' ?RX CF  ' TX! CF  ' !IO CF -> 28 29 30 31 }T
T{ ' + 32 CF?  ' - 33 CF?  ' 1+ 34 CF?  ' 1- 35 CF?  ' NEGATE 36 CF? -> 32 33 34 35 36 }T
T{ ' INVERT 37 CF?  ' 2* 38 CF?  ' 2/ 39 CF?  ' LSHIFT 40 CF?  ' RSHIFT 41 CF? -> 37 38 39 40 41 }T
T{ ' = 42 CF?  ' 0= 43 CF?  ' < 44 CF?  ' U< 45 CF? -> 42 43 44 45 }T
T{ ' * 46 CF?  ' UM* 47 CF?  ' UM/MOD 48 CF? -> 46 47 48 }T
T{ ' ROT 49 CF?  ' NIP 50 CF?  ' ?DUP 51 CF?  ' 2DUP 52 CF?  ' 2DROP 53 CF? -> 49 50 51 52 53 }T
T{ ' +! 54 CF?  ' MOVE 55 CF?  ' FILL 56 CF?  ' COUNT 57 CF? -> 54 55 56 57 }T
T{ ' FIND-NAME 58 CF? -> 58 }T
T{ ' (DO) 59 CF?  ' (?DO) 60 CF?  ' (LOOP) 61 CF?  ' (+LOOP) 62 CF? -> 59 60 61 62 }T
T{ ' I 63 CF?  ' J 64 CF?  ' UNLOOP 65 CF? -> 63 64 65 }T

\ ==== 0-3  Code field types: DOCOL DOVAR DOCON DODOES ====

: T0-COLON ( -- x )  42 ;
CREATE T0-CREATED  7 ,
99 CONSTANT T0-CONST
VARIABLE T0-VAR
: T0-DOES ( x "name" -- )  CREATE ,  DOES> @ 1+ ;
10 T0-DOES T0-TEN

T{ ' T0-COLON CF  ' T0-CREATED CF  ' T0-CONST CF  ' T0-VAR CF  ' T0-TEN CF -> 0 1 2 2 3 }T
T{ T0-COLON -> 42 }T
T{ T0-CREATED ' T0-CREATED >BODY = -> <TRUE> }T      \ DOVAR gives the data field
T{ T0-CREATED @ -> 7 }T
T{ T0-CONST -> 99 }T                                  \ DOCON gives the value
T{ T0-VAR @ -> 0 }T
T{ 5 T0-VAR !  T0-VAR @ -> 5 }T
T{ T0-TEN -> 11 }T                                    \ DODOES: data field, then the DOES> code
T{ ' T0-TEN >BODY @ -> 10 }T

\ ==== 4-8  EXIT LIT BRANCH ?BRANCH EXECUTE ====

: T0-EXIT ( -- x )  1 EXIT 2 ;
: T0-EXIT2 ( f -- x )  IF 1 EXIT THEN 2 ;
T{ T0-EXIT -> 1 }T
T{ 0 T0-EXIT2  -1 T0-EXIT2 -> 2 1 }T

: T0-LITS ( -- x*5 )  0 -1 [ MSB ] LITERAL [ MAX-INT ] LITERAL [ MAX-UINT ] LITERAL ;
T{ T0-LITS -> 0 -1 MSB MAX-INT MAX-UINT }T
: T0-LIT ( -- x )  LIT [ 1234 , ] ;                   \ LIT with the number in the next cell
T{ T0-LIT -> 1234 }T

\ BRANCH and ?BRANCH take the absolute target address from the next cell.
: T0-BRANCH ( -- x )  BRANCH [ HERE 0 , ] 1 [ HERE SWAP ! ] 2 ;
T{ T0-BRANCH -> 2 }T
: T0-?BRANCH ( f -- x )  ?BRANCH [ HERE 0 , ] 1 EXIT [ HERE SWAP ! ] 2 ;
T{ 0 T0-?BRANCH  1 T0-?BRANCH  -1 T0-?BRANCH  MSB T0-?BRANCH -> 2 1 1 1 }T
: T0-BACK ( n -- 0 )  [ HERE ] 1- DUP 0= ?BRANCH [ , ] ;   \ a jump back
T{ 5 T0-BACK -> 0 }T

T{ 3 ' DUP EXECUTE -> 3 3 }T                          \ a primitive
T{ ' T0-COLON EXECUTE -> 42 }T                        \ a colon word
T{ ' T0-CONST EXECUTE  ' T0-TEN EXECUTE -> 99 11 }T
T{ ' T0-CREATED EXECUTE -> T0-CREATED }T
T{ 4 ' DUP ' EXECUTE EXECUTE -> 4 4 }T                \ EXECUTE of EXECUTE
: T0-XT ( xt -- )  EXECUTE ;
T{ 6 ' DUP T0-XT -> 6 6 }T
: T0-EXIT-XT ( -- )  ['] EXIT EXECUTE 1 ;             \ EXECUTE of EXIT leaves the word
T{ T0-EXIT-XT -> }T

\ ==== 9-12  @ ! C@ C! ====

T{ -1 T0-BUF !  T0-BUF @ -> -1 }T
T{ MSB T0-BUF !  T0-BUF @ -> MSB }T
T{ 1 T0-BUF !  2 T0-BUF CELL+ !  T0-BUF @  T0-BUF CELL+ @ -> 1 2 }T
T{ $1234 T0-BUF !  T0-BUF C@  T0-BUF 1+ C@ -> $34 $12 }T      \ the low byte first
T{ 0 T0-BUF !  $5A T0-BUF 1+ C!  T0-BUF @ -> $5A00 }T
T{ -1 T0-BUF !  0 T0-BUF C!  T0-BUF @ -> -256 }T               \ C! changes one byte
T{ $1234 T0-BUF C!  T0-BUF C@ -> $34 }T                        \ C! stores the low byte
T{ -1 T0-BUF C!  T0-BUF C@ -> 255 }T                           \ C@ gives no sign
T{ 0 T0-BUF 1+ C!  $80 T0-BUF 1+ C!  T0-BUF 1+ C@ -> $80 }T    \ also at an odd address

\ ==== 13-16  DUP DROP SWAP OVER ====

T{ 1 DUP -> 1 1 }T
T{ MSB DUP -> MSB MSB }T
T{ 1 2 DROP -> 1 }T
T{ 1 2 SWAP -> 2 1 }T
T{ MSB 0 SWAP -> 0 MSB }T
T{ 1 2 OVER -> 1 2 1 }T
T{ -1 MSB OVER -> -1 MSB -1 }T

\ ==== 17-19  >R R> R@ ====

: T0-R1 ( x -- x x )  >R R@ R> ;
: T0-R2 ( a b -- a b )  >R >R R> R> ;
: T0-R3 ( -- x )  1 >R 2 >R R@ R> R> + + ;
T{ MSB T0-R1 -> MSB MSB }T
T{ 1 2 T0-R2 -> 1 2 }T
T{ T0-R3 -> 5 }T

\ ==== 20-23  SP@ SP! RP@ RP! ====

T{ SP@ SP@ - -> 1 CELLS }T                          \ SP@ gives the address of the top
T{ 5 SP@ @ -> 5 5 }T
T{ S0 SP@ - -> DEPTH 1+ CELLS }T
T{ 1 2 SP@ 3 4 ROT SP! -> 1 2 }T                    \ SP! drops 3 and 4 again
T{ DEPTH 7 SWAP DEPTH SWAP - -> 7 2 }T

: T0-RP ( -- n )  RP@  1 >R  RP@  R> DROP  - ;
T{ T0-RP -> 1 CELLS }T
: T0-RP! ( -- x )  7 >R  RP@  1 >R 2 >R  RP!  R> ;
T{ T0-RP! -> 7 }T                                   \ RP! drops 1 and 2 again
: T0-R0 ( -- f )  RP@ R0 U< ;
T{ T0-R0 -> <TRUE> }T

\ CATCH and THROW use SP@ SP! RP@ RP!.
: T0-THROW ( -- )  1 2 3 >R 5 THROW ;
T{ 9 ' T0-THROW CATCH -> 9 5 }T
T{ ' T0-COLON CATCH -> 42 0 }T

\ ==== 24-27  AND OR XOR 0< ====

T{ $F0F0 $FF00 AND -> $F000 }T
T{ $F0F0 $FF00 OR -> $FFF0 }T
T{ $F0F0 $FF00 XOR -> $0FF0 }T
T{ 1S MSB AND  0 MSB OR  1S MSB XOR  1S 1S XOR -> MSB MSB MAX-INT 0 }T
T{ 0 0<  1 0<  MAX-INT 0< -> <FALSE> <FALSE> <FALSE> }T
T{ -1 0<  MSB 0<  MSB 1+ 0< -> <TRUE> <TRUE> <TRUE> }T   \ only the top bit counts

\ ==== 28-31  UM+ ?RX TX! !IO ====

T{ 1 2 UM+ -> 3 0 }T
T{ 0 0 UM+ -> 0 0 }T
T{ MAX-UINT 0 UM+ -> MAX-UINT 0 }T
T{ MAX-UINT 1 UM+ -> 0 1 }T                         \ the carry
T{ MAX-UINT MAX-UINT UM+ -> MAX-UINT 1- 1 }T
T{ MSB MSB UM+ -> 0 1 }T
T{ MAX-INT 1 UM+ -> MSB 0 }T                        \ no carry at the sign bit

\ ?RX gives c -1 or 0. While the file loads, usually no key is there.
: T0-RX ( -- f )  ?RX ?DUP IF TRUE = SWAP 256 U< AND ELSE TRUE THEN ;
T{ T0-RX -> <TRUE> }T
T{ 13 TX! 10 TX! -> }T                              \ TX! sends an empty line
T{ !IO -> }T

\ ==== 32-41  + - 1+ 1- NEGATE INVERT 2* 2/ LSHIFT RSHIFT ====

T{ 1 2 +  MAX-INT 1 +  MAX-UINT 1 +  MIN-INT -1 + -> 3 MIN-INT 0 MAX-INT }T
T{ 5 7 -  MIN-INT 1 -  0 1 - -> -2 MAX-INT MAX-UINT }T
T{ 0 1+  MAX-INT 1+  MAX-UINT 1+ -> 1 MIN-INT 0 }T
T{ 0 1-  MIN-INT 1-  1 1- -> -1 MAX-INT 0 }T
T{ 5 NEGATE  -5 NEGATE  0 NEGATE -> -5 5 0 }T
T{ MIN-INT NEGATE  MAX-INT NEGATE -> MIN-INT MIN-INT 1+ }T
T{ 0 INVERT  MSB INVERT  1S INVERT -> 1S MAX-INT 0 }T
T{ 1 2*  MAX-INT 2*  MSB 2*  1S 2* -> 2 -2 0 -2 }T
T{ 5 2/  -5 2/  -7 2/  -1 2/  1 2/ -> 2 -3 -4 -1 0 }T  \ 2/ rounds down
T{ MSB 2/ -> MSB MSB 1 RSHIFT OR }T                    \ 2/ keeps the sign bit
T{ MAX-INT 2/ -> MAX-INT 1 RSHIFT }T
T{ 1 0 LSHIFT  1 1 LSHIFT  1 #BITS 1- LSHIFT  1S #BITS 1- LSHIFT -> 1 2 MSB MSB }T
T{ 1 #BITS LSHIFT  1S #BITS LSHIFT -> 0 0 }T          \ from #BITS on the result is 0
T{ MSB 0 RSHIFT  MSB 1 RSHIFT -> MSB 1 #BITS 2 - LSHIFT }T   \ RSHIFT fills with 0
T{ MSB #BITS 1- RSHIFT  1S #BITS 1- RSHIFT  -2 1 RSHIFT -> 1 1 MAX-INT }T
T{ 1S #BITS RSHIFT  MSB #BITS RSHIFT -> 0 0 }T
T{ $123 4 LSHIFT  $1230 4 RSHIFT -> $1230 $123 }T

\ ==== 42-45  = 0= < U< ====

T{ 0 0 =  1 1 =  MSB MSB = -> <TRUE> <TRUE> <TRUE> }T
T{ 1 2 =  MSB 0 =  -1 1 = -> <FALSE> <FALSE> <FALSE> }T
T{ 0 0=  1 0=  MSB 0=  1S 0= -> <TRUE> <FALSE> <FALSE> <FALSE> }T
T{ 1 2 <  -1 0 <  MIN-INT MAX-INT <  MIN-INT 0 < -> <TRUE> <TRUE> <TRUE> <TRUE> }T
T{ 2 1 <  0 -1 <  1 1 <  MAX-INT MIN-INT < -> <FALSE> <FALSE> <FALSE> <FALSE> }T
T{ MIN-INT 1+ MIN-INT <  MAX-INT 1- MAX-INT < -> <FALSE> <TRUE> }T
T{ 1 2 U<  0 -1 U<  MAX-INT MIN-INT U<  MAX-UINT 1- MAX-UINT U< -> <TRUE> <TRUE> <TRUE> <TRUE> }T
T{ 2 1 U<  -1 0 U<  1 1 U<  MIN-INT MAX-INT U< -> <FALSE> <FALSE> <FALSE> <FALSE> }T

\ ==== 46-48  * UM* UM/MOD ====

T{ 3 4 *  -3 4 *  -3 -4 *  0 -1 * -> 12 -12 12 0 }T
T{ MAX-INT 2 *  MIN-INT -1 *  MIN-INT 2 *  -1 -1 * -> -2 MIN-INT 0 1 }T
T{ 3 4 UM* -> 12 0 }T
T{ 0 MAX-UINT UM* -> 0 0 }T
T{ MAX-UINT 2 UM* -> MAX-UINT 1- 1 }T
T{ MSB 2 UM* -> 0 1 }T
T{ MSB MSB UM* -> 0 MSB 1 RSHIFT }T
T{ MAX-UINT MAX-UINT UM* -> 1 MAX-UINT 1- }T           \ the biggest product
T{ 13 0 5 UM/MOD -> 3 2 }T
T{ 5 0 7 UM/MOD -> 5 0 }T
T{ 0 1 2 UM/MOD -> 0 MSB }T                            \ exact
T{ MAX-UINT 0 MAX-UINT UM/MOD -> 0 1 }T
T{ 0 MAX-INT MSB UM/MOD -> 0 MAX-UINT 1- }T
T{ MAX-UINT MAX-UINT UM* MAX-UINT UM/MOD -> 0 MAX-UINT }T
T{ MAX-UINT MAX-UINT 1- MAX-UINT UM/MOD -> MAX-UINT 1- MAX-UINT }T   \ the biggest quotient

\ ==== 49-57  ROT NIP ?DUP 2DUP 2DROP +! MOVE FILL COUNT ====

T{ 1 2 3 ROT -> 2 3 1 }T
T{ 1 2 3 ROT ROT -> 3 1 2 }T
T{ 1 2 NIP -> 2 }T
T{ 0 ?DUP  5 ?DUP  MSB ?DUP -> 0 5 5 MSB MSB }T
T{ 1 2 2DUP -> 1 2 1 2 }T
T{ 1 2 3 2DROP -> 1 }T
T{ 5 T0-BUF !  3 T0-BUF +!  T0-BUF @ -> 8 }T
T{ -9 T0-BUF +!  T0-BUF @ -> -1 }T
T{ MAX-INT T0-BUF !  1 T0-BUF +!  T0-BUF @ -> MIN-INT }T

: T0-SEQ ( -- )   8 0 DO I 1+ T0-BUF I + C! LOOP ;    \ the bytes 1 2 3 4 5 6 7 8
: T0-B@ ( i -- c )  T0-BUF + C@ ;
T{ T0-SEQ  T0-BUF T0-BUF 4 + 0 MOVE  4 T0-B@ -> 5 }T  \ length 0 moves nothing
T{ T0-SEQ  T0-BUF 0 0 FILL  0 T0-B@ -> 1 }T           \ length 0 fills nothing
T{ T0-SEQ  T0-BUF 1+ 3 $AA FILL  0 T0-B@ 1 T0-B@ 3 T0-B@ 4 T0-B@ -> 1 $AA $AA 5 }T
\ MOVE with overlap: upward it must copy from the end, downward from the start.
T{ T0-SEQ  T0-BUF T0-BUF 1+ 4 MOVE  0 T0-B@ 1 T0-B@ 4 T0-B@ 5 T0-B@ -> 1 1 4 6 }T
T{ T0-SEQ  T0-BUF 1+ T0-BUF 4 MOVE  0 T0-B@ 3 T0-B@ 4 T0-B@ -> 2 5 5 }T
T{ T0-SEQ  T0-BUF T0-BUF 3 MOVE  2 T0-B@ -> 3 }T
T{ T0-SEQ  T0-BUF 5 + T0-BUF 3 MOVE  0 T0-B@ 2 T0-B@ 3 T0-B@ -> 6 8 4 }T
T{ T0-SEQ  3 T0-BUF C!  T0-BUF COUNT -> T0-BUF 1+ 3 }T
T{ 255 T0-BUF C!  T0-BUF COUNT NIP -> 255 }T          \ the count has no sign

\ ==== 58  FIND-NAME ====

T{ S" DUP" FIND-NAME NAME>XT -> ' DUP }T
T{ S" dup" FIND-NAME NAME>XT -> ' DUP }T              \ the case of letters does not count
T{ S" LIT" FIND-NAME NAME>XT -> ' LIT }T              \ also in INTERNALS
T{ S" T0-COLON" FIND-NAME NAME>XT -> ' T0-COLON }T
T{ S" T0-NOT-THERE" FIND-NAME -> 0 }T
T{ S" DUPX" DROP 2 FIND-NAME -> 0 }T                  \ the length counts
T{ S" DUP" DROP 0 FIND-NAME -> 0 }T                   \ an empty name
T{ S" ABCDEFGHIJKLMNOPQRSTUVWXYZ012345" FIND-NAME -> 0 }T   \ 32 characters

\ ==== 59-65  (DO) (?DO) (LOOP) (+LOOP) I J UNLOOP ====

: T0-DO ( limit start -- i*x )  DO I LOOP ;
T{ 3 0 T0-DO -> 0 1 2 }T
T{ 1 0 T0-DO -> 0 }T
T{ -1 -3 T0-DO -> -3 -2 }T
T{ MIN-INT MAX-INT T0-DO -> MAX-INT }T                \ the index goes over MAX-INT
T{ 0 MAX-UINT T0-DO -> MAX-UINT }T

: T0-?DO ( limit start -- i*x )  ?DO I LOOP ;
T{ 3 0 T0-?DO -> 0 1 2 }T
T{ 5 5 T0-?DO -> }T                                   \ limit = start: no pass
T{ MIN-INT MIN-INT T0-?DO -> }T

: T0-+LOOP ( limit start n -- i*x )  T0-STEP !  DO I T0-STEP @ +LOOP ;
T{ 10 0 3 T0-+LOOP -> 0 3 6 9 }T
T{ 10 0 5 T0-+LOOP -> 0 5 }T
T{ 0 10 -3 T0-+LOOP -> 10 7 4 1 }T
T{ 0 10 -5 T0-+LOOP -> 10 5 0 }T                      \ down: the limit itself is a pass
T{ -1 2 -1 T0-+LOOP -> 2 1 0 -1 }T
T{ 1 -2 1 T0-+LOOP -> -2 -1 0 }T
T{ MIN-INT MAX-INT 1 T0-+LOOP -> MAX-INT }T
T{ MAX-INT MIN-INT -1 T0-+LOOP -> MIN-INT MAX-INT }T

\ The boundary between limit-1 and limit also counts after a wrap
\ around of the index: 256 passes over the whole number range.
: T0-PASSES ( limit start n -- count )  T0-STEP !  0 ROT ROT  DO 1+ T0-STEP @ +LOOP ;
MAX-UINT 8 RSHIFT 1+ CONSTANT T0-USTEP               \ 256 with 16-bit cells
MAX-INT 7 RSHIFT 1+ CONSTANT T0-SSTEP
T{ MAX-UINT 0 T0-USTEP T0-PASSES -> 256 }T
T{ 0 MAX-UINT T0-USTEP NEGATE T0-PASSES -> 256 }T
T{ MAX-INT MIN-INT T0-SSTEP T0-PASSES -> 256 }T
T{ MIN-INT MAX-INT T0-SSTEP NEGATE T0-PASSES -> 256 }T

: T0-IJ ( -- i*x )  3 1 DO  2 0 DO  J 10 * I +  LOOP  LOOP ;
T{ T0-IJ -> 10 11 20 21 }T
: T0-J3 ( -- i*x )  2 0 DO  2 0 DO  2 0 DO  J  LOOP  LOOP  LOOP ;
T{ T0-J3 -> 0 0 1 1 0 0 1 1 }T                        \ J is the next outer loop
: T0-UNLOOP ( -- x )  10 0 DO  I 3 = IF I UNLOOP EXIT THEN  LOOP  99 ;
T{ T0-UNLOOP -> 3 }T
: T0-UNLOOP2 ( -- i j )
  10 0 DO  10 0 DO  I J + 5 = IF I J UNLOOP UNLOOP EXIT THEN  LOOP  LOOP  99 ;
T{ T0-UNLOOP2 -> 5 0 }T
: T0-LEAVE ( -- i*x )  10 0 DO  I  I 3 = IF LEAVE THEN  LOOP ;
T{ T0-LEAVE -> 0 1 2 3 }T
: T0-RLOOP ( -- x )  5 >R  3 0 DO LOOP  R> ;          \ the loop keeps the return stack
T{ T0-RLOOP -> 5 }T

\ ==== 80-84  HAL: MILLIS MS US BIS! BIC! ====

\ A clock that counts only seconds (bash 3.2, awk) steps by 1000: the
\ step can come between two MILLIS. So the limits are 2000.
T{ MILLIS MILLIS SWAP - 2000 U< -> <TRUE> }T          \ MILLIS does not go back
T{ MILLIS 10 MS MILLIS SWAP - 10 2000 WITHIN -> <TRUE> }T   \ MS 10 adds about 10
T{ 0 MS  10 US -> }T
\ BIS! and BIC! change a byte on the AVR and a cell on the ESP32-S3,
\ whose registers have 32 bits. These tests hold for both.
T{ $0F T0-BUF C!  $30 T0-BUF BIS!  T0-BUF C@ -> $3F }T
T{ $05 T0-BUF BIC!  T0-BUF C@ -> $3A }T
T{ -1 T0-BUF !  $FF T0-BUF BIC!  T0-BUF @ -> -256 }T
T{ 0 T0-BUF !  $81 T0-BUF BIS!  T0-BUF @ -> $81 }T

S" t0" .RESULTS
