\ hayes.fs - tests of the core words of uCForth
\
\ Adapted for uCForth from the core tests of John Hayes:
\
\   From: John Hayes S1I
\   Subject: core.fr
\   Date: Mon, 27 Nov 95 13:10
\   (C) 1995 JOHNS HOPKINS UNIVERSITY / APPLIED PHYSICS LABORATORY
\   MAY BE DISTRIBUTED FREELY AS LONG AS THIS COPYRIGHT NOTICE REMAINS.
\   VERSION 1.2
\
\ Changes for uCForth:
\ - The syntax is T{ ... -> ... }T from forth/test/tester.fs. Numbers are
\   decimal, $ marks hex. The tests work with any cell size.
\ - No tests for words that uCForth does not have: 2OVER 2SWAP 2@ 2!
\   CHAR [CHAR] BL CHARS CHAR+ WORD FIND EVALUATE SOURCE >NUMBER FM/MOD
\   */MOD ENVIRONMENT?, and no tricks with >IN. FIND-NAME replaces FIND.
\ - The division is symmetric (the quotient rounds toward 0), so the
\   reference words T/MOD etc. use SM/REM.
\ - DO with limit = start runs 65536 times. Loops that can run 0 times
\   use ?DO.
\ - Output and input tests compare text through the vectors 'EMIT and
\   'KEY?, not by eye.
\ - New tests for words that Hayes does not cover: the lines after a
\   comment "uCForth:", the sections CASE, Data and Number input, and
\   all sections from Errors on.
\ - A test with the comment "KNOWN FAIL" shows a suspected fault of the
\   system. The test stays until the system is repaired.
\
\ Load:  bash bootstrap/bash/ucf.sh forth/test/tester.fs forth/test/hayes.fs

ONLY FORTH DEFINITIONS  ALSO INTERNALS
'EMIT @ CONSTANT EMIT0   ECHO @ CONSTANT ECHO0   \ the console at the start: the tests put it back
DECIMAL

\ ==== Helpers: compare strings, capture output, fake input ====

: S= ( a1 u1 a2 u2 -- f )        \ compare two strings (Hayes)
  >R SWAP R@ = IF
    R> ?DUP IF
      0 DO
        OVER C@ OVER C@ - IF 2DROP <FALSE> UNLOOP EXIT THEN
        SWAP 1+ SWAP 1+
      LOOP
    THEN
    2DROP <TRUE>
  ELSE
    R> DROP 2DROP <FALSE>
  THEN ;

128 BUFFER: OUT-BUF              \ OUT{ ... }OUT collects the output here
VARIABLE OUT-LEN
VARIABLE OLD-EMIT
VARIABLE OLD-OUT                 \ the column of the console stays as it was
: OUT-EMIT ( c -- )
  OUT-LEN @ 128 < IF  OUT-BUF OUT-LEN @ + C!  1 OUT-LEN +!  EXIT THEN  DROP ;
: OUT{ ( -- )     0 OUT-LEN !  OUT @ OLD-OUT !  'EMIT @ OLD-EMIT !  ['] OUT-EMIT 'EMIT ! ;
: }OUT ( -- a u ) OLD-EMIT @ 'EMIT !  OLD-OUT @ OUT !  OUT-BUF OUT-LEN @ ;
: AFTER-CR ( a u -- a+2 u-2 )    \ skip the CR at the start of an output
  2 - SWAP 2 + SWAP ;

VARIABLE IN-PTR                  \ IN{ ... }IN gives KEY the characters of a string
VARIABLE IN-END
VARIABLE OLD-KEY?
: FAKE-RX ( -- c -1 | 0 )
  IN-PTR @ IN-END @ = IF 0 EXIT THEN
  IN-PTR @ C@  1 IN-PTR +!  TRUE ;
: IN{ ( a u -- )  OVER + IN-END !  IN-PTR !  'KEY? @ OLD-KEY? !  ['] FAKE-RX 'KEY? ! ;
: }IN ( -- )      OLD-KEY? @ 'KEY? !  -1 KEYBUF ! ;

: RESET-ORDER ( -- )  ONLY FORTH DEFINITIONS  ALSO INTERNALS ;

\ ==== Basic assumptions ====

T{ -> }T                                  \ start with a clean slate
( test if any bits are set; answer in base 1 )
T{ : BITSSET? IF 0 0 ELSE 0 THEN ; -> }T
T{  0 BITSSET? -> 0 }T                    ( zero is all bits clear )
T{  1 BITSSET? -> 0 0 }T                  ( other number have at least one bit )
T{ -1 BITSSET? -> 0 0 }T

\ ==== Booleans: INVERT AND OR XOR ====

T{ 0 0 AND -> 0 }T
T{ 0 1 AND -> 0 }T
T{ 1 0 AND -> 0 }T
T{ 1 1 AND -> 1 }T

T{ 0 INVERT 1 AND -> 1 }T
T{ 1 INVERT 1 AND -> 0 }T

T{ 0S INVERT -> 1S }T
T{ 1S INVERT -> 0S }T

T{ 0S 0S AND -> 0S }T
T{ 0S 1S AND -> 0S }T
T{ 1S 0S AND -> 0S }T
T{ 1S 1S AND -> 1S }T

T{ 0S 0S OR -> 0S }T
T{ 0S 1S OR -> 1S }T
T{ 1S 0S OR -> 1S }T
T{ 1S 1S OR -> 1S }T

T{ 0S 0S XOR -> 0S }T
T{ 0S 1S XOR -> 1S }T
T{ 1S 0S XOR -> 1S }T
T{ 1S 1S XOR -> 0S }T

\ ==== Shifts: 2* 2/ LSHIFT RSHIFT ====

T{ MSB BITSSET? -> 0 0 }T

T{ 0S 2* -> 0S }T
T{ 1 2* -> 2 }T
T{ $4000 2* -> $8000 }T
T{ 1S 2* 1 XOR -> 1S }T
T{ MSB 2* -> 0S }T

T{ 0S 2/ -> 0S }T
T{ 1 2/ -> 0 }T
T{ $4000 2/ -> $2000 }T
T{ 1S 2/ -> 1S }T                         \ msb propagated
T{ 1S 1 XOR 2/ -> 1S }T
T{ MSB 2/ MSB AND -> MSB }T

T{ 1 0 LSHIFT -> 1 }T
T{ 1 1 LSHIFT -> 2 }T
T{ 1 2 LSHIFT -> 4 }T
T{ 1 15 LSHIFT -> $8000 }T                \ biggest guaranteed shift
T{ 1S 1 LSHIFT 1 XOR -> 1S }T
T{ MSB 1 LSHIFT -> 0 }T

T{ 1 0 RSHIFT -> 1 }T
T{ 1 1 RSHIFT -> 0 }T
T{ 2 1 RSHIFT -> 1 }T
T{ 4 2 RSHIFT -> 1 }T
T{ $8000 15 RSHIFT -> 1 }T                \ biggest
T{ MSB 1 RSHIFT MSB AND -> 0 }T           \ rshift zero fills msbs
T{ MSB 1 RSHIFT 2* -> MSB }T

\ uCForth: 2/ keeps the sign, RSHIFT fills with 0, from #BITS on the result is 0
T{ -7 2/ -> -4 }T
T{ -2 2/ -> -1 }T
T{ -1 2/ -> -1 }T
T{ MIN-INT 2/ -> MIN-INT MIN-INT 1 RSHIFT OR }T
T{ -1 1 RSHIFT -> MAX-INT }T
T{ -2 1 RSHIFT -> MAX-INT }T
T{ 1 #BITS 1- LSHIFT -> MSB }T
T{ 1S #BITS 1- LSHIFT -> MSB }T
T{ MSB #BITS 1- RSHIFT -> 1 }T
T{ 1S #BITS 1- RSHIFT -> 1 }T
T{ 1 #BITS LSHIFT -> 0 }T
T{ 1S #BITS RSHIFT -> 0 }T
T{ $123 4 LSHIFT -> $1230 }T
T{ $1230 4 RSHIFT -> $123 }T

\ ==== Comparisons: 0= = 0< < > U< MIN MAX ====

T{ 0 0= -> <TRUE> }T
T{ 1 0= -> <FALSE> }T
T{ 2 0= -> <FALSE> }T
T{ -1 0= -> <FALSE> }T
T{ MAX-UINT 0= -> <FALSE> }T
T{ MIN-INT 0= -> <FALSE> }T
T{ MAX-INT 0= -> <FALSE> }T

T{ 0 0 = -> <TRUE> }T
T{ 1 1 = -> <TRUE> }T
T{ -1 -1 = -> <TRUE> }T
T{ 1 0 = -> <FALSE> }T
T{ -1 0 = -> <FALSE> }T
T{ 0 1 = -> <FALSE> }T
T{ 0 -1 = -> <FALSE> }T

T{ 0 0< -> <FALSE> }T
T{ -1 0< -> <TRUE> }T
T{ MIN-INT 0< -> <TRUE> }T
T{ 1 0< -> <FALSE> }T
T{ MAX-INT 0< -> <FALSE> }T

T{ 0 1 < -> <TRUE> }T
T{ 1 2 < -> <TRUE> }T
T{ -1 0 < -> <TRUE> }T
T{ -1 1 < -> <TRUE> }T
T{ MIN-INT 0 < -> <TRUE> }T
T{ MIN-INT MAX-INT < -> <TRUE> }T
T{ 0 MAX-INT < -> <TRUE> }T
T{ 0 0 < -> <FALSE> }T
T{ 1 1 < -> <FALSE> }T
T{ 1 0 < -> <FALSE> }T
T{ 2 1 < -> <FALSE> }T
T{ 0 -1 < -> <FALSE> }T
T{ 1 -1 < -> <FALSE> }T
T{ 0 MIN-INT < -> <FALSE> }T
T{ MAX-INT MIN-INT < -> <FALSE> }T
T{ MAX-INT 0 < -> <FALSE> }T

T{ 0 1 > -> <FALSE> }T
T{ 1 2 > -> <FALSE> }T
T{ -1 0 > -> <FALSE> }T
T{ -1 1 > -> <FALSE> }T
T{ MIN-INT 0 > -> <FALSE> }T
T{ MIN-INT MAX-INT > -> <FALSE> }T
T{ 0 MAX-INT > -> <FALSE> }T
T{ 0 0 > -> <FALSE> }T
T{ 1 1 > -> <FALSE> }T
T{ 1 0 > -> <TRUE> }T
T{ 2 1 > -> <TRUE> }T
T{ 0 -1 > -> <TRUE> }T
T{ 1 -1 > -> <TRUE> }T
T{ 0 MIN-INT > -> <TRUE> }T
T{ MAX-INT MIN-INT > -> <TRUE> }T
T{ MAX-INT 0 > -> <TRUE> }T

T{ 0 1 U< -> <TRUE> }T
T{ 1 2 U< -> <TRUE> }T
T{ 0 MID-UINT U< -> <TRUE> }T
T{ 0 MAX-UINT U< -> <TRUE> }T
T{ MID-UINT MAX-UINT U< -> <TRUE> }T
T{ 0 0 U< -> <FALSE> }T
T{ 1 1 U< -> <FALSE> }T
T{ 1 0 U< -> <FALSE> }T
T{ 2 1 U< -> <FALSE> }T
T{ MID-UINT 0 U< -> <FALSE> }T
T{ MAX-UINT 0 U< -> <FALSE> }T
T{ MAX-UINT MID-UINT U< -> <FALSE> }T

T{ 0 1 MIN -> 0 }T
T{ 1 2 MIN -> 1 }T
T{ -1 0 MIN -> -1 }T
T{ -1 1 MIN -> -1 }T
T{ MIN-INT 0 MIN -> MIN-INT }T
T{ MIN-INT MAX-INT MIN -> MIN-INT }T
T{ 0 MAX-INT MIN -> 0 }T
T{ 0 0 MIN -> 0 }T
T{ 1 1 MIN -> 1 }T
T{ 1 0 MIN -> 0 }T
T{ 2 1 MIN -> 1 }T
T{ 0 -1 MIN -> -1 }T
T{ 1 -1 MIN -> -1 }T
T{ 0 MIN-INT MIN -> MIN-INT }T
T{ MAX-INT MIN-INT MIN -> MIN-INT }T
T{ MAX-INT 0 MIN -> 0 }T

T{ 0 1 MAX -> 1 }T
T{ 1 2 MAX -> 2 }T
T{ -1 0 MAX -> 0 }T
T{ -1 1 MAX -> 1 }T
T{ MIN-INT 0 MAX -> 0 }T
T{ MIN-INT MAX-INT MAX -> MAX-INT }T
T{ 0 MAX-INT MAX -> MAX-INT }T
T{ 0 0 MAX -> 0 }T
T{ 1 1 MAX -> 1 }T
T{ 1 0 MAX -> 1 }T
T{ 2 1 MAX -> 2 }T
T{ 0 -1 MAX -> 0 }T
T{ 1 -1 MAX -> 1 }T
T{ 0 MIN-INT MAX -> 0 }T
T{ MAX-INT MIN-INT MAX -> MAX-INT }T
T{ MAX-INT 0 MAX -> MAX-INT }T

\ uCForth: <> 0<> TRUE FALSE WITHIN U>, and UMAX from INTERNALS
T{ 0 0 <> -> <FALSE> }T
T{ 1 0 <> -> <TRUE> }T
T{ -1 1 <> -> <TRUE> }T
T{ MIN-INT MIN-INT <> -> <FALSE> }T
T{ 0 0<> -> <FALSE> }T
T{ 1 0<> -> <TRUE> }T
T{ 2 0<> -> <TRUE> }T
T{ -1 0<> -> <TRUE> }T
T{ MSB 0<> -> <TRUE> }T
T{ TRUE -> <TRUE> }T
T{ FALSE -> <FALSE> }T

T{ 5 1 10 WITHIN -> <TRUE> }T
T{ 1 1 10 WITHIN -> <TRUE> }T
T{ 10 1 10 WITHIN -> <FALSE> }T
T{ 0 1 10 WITHIN -> <FALSE> }T
T{ -5 -10 0 WITHIN -> <TRUE> }T
T{ -10 -10 0 WITHIN -> <TRUE> }T
T{ 0 -10 0 WITHIN -> <FALSE> }T
T{ 0 -1 1 WITHIN -> <TRUE> }T
T{ MAX-INT 0 MIN-INT WITHIN -> <TRUE> }T          \ also for unsigned numbers
T{ MIN-INT 0 MIN-INT WITHIN -> <FALSE> }T
T{ MAX-UINT 0 MAX-UINT WITHIN -> <FALSE> }T
T{ MAX-INT MAX-INT MIN-INT WITHIN -> <TRUE> }T    \ the range goes over MAX-INT
T{ 1 1 1 WITHIN -> <FALSE> }T                     \ an empty range

T{ 1 0 U> -> <TRUE> }T
T{ 0 1 U> -> <FALSE> }T
T{ 1 1 U> -> <FALSE> }T
T{ MAX-UINT 0 U> -> <TRUE> }T
T{ MSB MAX-INT U> -> <TRUE> }T
T{ 3 5 UMAX -> 5 }T
T{ 1 MAX-UINT UMAX -> MAX-UINT }T
T{ MSB MAX-INT UMAX -> MSB }T

\ ==== Stack: 2DROP 2DUP ?DUP DEPTH DROP DUP OVER ROT SWAP NIP TUCK ====

T{ 1 2 2DROP -> }T
T{ 1 2 2DUP -> 1 2 1 2 }T
T{ 0 ?DUP -> 0 }T
T{ 1 ?DUP -> 1 1 }T
T{ -1 ?DUP -> -1 -1 }T
T{ DEPTH -> 0 }T
T{ 0 DEPTH -> 0 1 }T
T{ 0 1 DEPTH -> 0 1 2 }T
T{ 0 DROP -> }T
T{ 1 2 DROP -> 1 }T
T{ 1 DUP -> 1 1 }T
T{ 1 2 OVER -> 1 2 1 }T
T{ 1 2 3 ROT -> 2 3 1 }T
T{ 1 2 SWAP -> 2 1 }T
T{ 1 2 NIP -> 2 }T
T{ 1 2 TUCK -> 2 1 2 }T
T{ MSB 1S NIP -> 1S }T

\ ==== Return stack: >R R> R@ ====

T{ : GR1 >R R> ; -> }T
T{ : GR2 >R R@ R> DROP ; -> }T
T{ 123 GR1 -> 123 }T
T{ 123 GR2 -> 123 }T
T{ 1S GR1 -> 1S }T                        ( return stack holds cells )

\ ==== Add and subtract: + - 1+ 1- ABS NEGATE ====

T{ 0 5 + -> 5 }T
T{ 5 0 + -> 5 }T
T{ 0 -5 + -> -5 }T
T{ -5 0 + -> -5 }T
T{ 1 2 + -> 3 }T
T{ 1 -2 + -> -1 }T
T{ -1 2 + -> 1 }T
T{ -1 -2 + -> -3 }T
T{ -1 1 + -> 0 }T
T{ MID-UINT 1 + -> MID-UINT+1 }T

T{ 0 5 - -> -5 }T
T{ 5 0 - -> 5 }T
T{ 0 -5 - -> 5 }T
T{ -5 0 - -> -5 }T
T{ 1 2 - -> -1 }T
T{ 1 -2 - -> 3 }T
T{ -1 2 - -> -3 }T
T{ -1 -2 - -> 1 }T
T{ 0 1 - -> -1 }T
T{ MID-UINT+1 1 - -> MID-UINT }T

T{ 0 1+ -> 1 }T
T{ -1 1+ -> 0 }T
T{ 1 1+ -> 2 }T
T{ MID-UINT 1+ -> MID-UINT+1 }T

T{ 2 1- -> 1 }T
T{ 1 1- -> 0 }T
T{ 0 1- -> -1 }T
T{ MID-UINT+1 1- -> MID-UINT }T

T{ 0 NEGATE -> 0 }T
T{ 1 NEGATE -> -1 }T
T{ -1 NEGATE -> 1 }T
T{ 2 NEGATE -> -2 }T
T{ -2 NEGATE -> 2 }T

T{ 0 ABS -> 0 }T
T{ 1 ABS -> 1 }T
T{ -1 ABS -> 1 }T
T{ MIN-INT ABS -> MID-UINT+1 }T

\ uCForth: no overflow check, all arithmetic wraps around
T{ MAX-INT 1+ -> MIN-INT }T
T{ MIN-INT 1- -> MAX-INT }T
T{ MIN-INT NEGATE -> MIN-INT }T
T{ MAX-UINT 1 + -> 0 }T

\ ==== Multiply: S>D * M* UM* ====

T{ 0 S>D -> 0 0 }T
T{ 1 S>D -> 1 0 }T
T{ 2 S>D -> 2 0 }T
T{ -1 S>D -> -1 -1 }T
T{ -2 S>D -> -2 -1 }T
T{ MIN-INT S>D -> MIN-INT -1 }T
T{ MAX-INT S>D -> MAX-INT 0 }T

T{ 0 0 M* -> 0 S>D }T
T{ 0 1 M* -> 0 S>D }T
T{ 1 0 M* -> 0 S>D }T
T{ 1 2 M* -> 2 S>D }T
T{ 2 1 M* -> 2 S>D }T
T{ 3 3 M* -> 9 S>D }T
T{ -3 3 M* -> -9 S>D }T
T{ 3 -3 M* -> -9 S>D }T
T{ -3 -3 M* -> 9 S>D }T
T{ 0 MIN-INT M* -> 0 S>D }T
T{ 1 MIN-INT M* -> MIN-INT S>D }T
T{ 2 MIN-INT M* -> 0 1S }T
T{ 0 MAX-INT M* -> 0 S>D }T
T{ 1 MAX-INT M* -> MAX-INT S>D }T
T{ 2 MAX-INT M* -> MAX-INT 1 LSHIFT 0 }T
T{ MIN-INT MIN-INT M* -> 0 MSB 1 RSHIFT }T
T{ MAX-INT MIN-INT M* -> MSB MSB 2/ }T
T{ MAX-INT MAX-INT M* -> 1 MSB 2/ INVERT }T

T{ 0 0 * -> 0 }T                          \ test identities
T{ 0 1 * -> 0 }T
T{ 1 0 * -> 0 }T
T{ 1 2 * -> 2 }T
T{ 2 1 * -> 2 }T
T{ 3 3 * -> 9 }T
T{ -3 3 * -> -9 }T
T{ 3 -3 * -> -9 }T
T{ -3 -3 * -> 9 }T

T{ MID-UINT+1 1 RSHIFT 2 * -> MID-UINT+1 }T
T{ MID-UINT+1 2 RSHIFT 4 * -> MID-UINT+1 }T
T{ MID-UINT+1 1 RSHIFT MID-UINT+1 OR 2 * -> MID-UINT+1 }T

T{ 0 0 UM* -> 0 0 }T
T{ 0 1 UM* -> 0 0 }T
T{ 1 0 UM* -> 0 0 }T
T{ 1 2 UM* -> 2 0 }T
T{ 2 1 UM* -> 2 0 }T
T{ 3 3 UM* -> 9 0 }T

T{ MID-UINT+1 1 RSHIFT 2 UM* -> MID-UINT+1 0 }T
T{ MID-UINT+1 2 UM* -> 0 1 }T
T{ MID-UINT+1 4 UM* -> 0 2 }T
T{ 1S 2 UM* -> 1S 1 LSHIFT 1 }T
T{ MAX-UINT MAX-UINT UM* -> 1 1 INVERT }T

\ ==== Divide: SM/REM UM/MOD */ / /MOD MOD ====

T{ 0 S>D 1 SM/REM -> 0 0 }T
T{ 1 S>D 1 SM/REM -> 0 1 }T
T{ 2 S>D 1 SM/REM -> 0 2 }T
T{ -1 S>D 1 SM/REM -> 0 -1 }T
T{ -2 S>D 1 SM/REM -> 0 -2 }T
T{ 0 S>D -1 SM/REM -> 0 0 }T
T{ 1 S>D -1 SM/REM -> 0 -1 }T
T{ 2 S>D -1 SM/REM -> 0 -2 }T
T{ -1 S>D -1 SM/REM -> 0 1 }T
T{ -2 S>D -1 SM/REM -> 0 2 }T
T{ 2 S>D 2 SM/REM -> 0 1 }T
T{ -1 S>D -1 SM/REM -> 0 1 }T
T{ -2 S>D -2 SM/REM -> 0 1 }T
T{  7 S>D  3 SM/REM -> 1 2 }T
T{  7 S>D -3 SM/REM -> 1 -2 }T
T{ -7 S>D  3 SM/REM -> -1 -2 }T
T{ -7 S>D -3 SM/REM -> -1 2 }T
T{ MAX-INT S>D 1 SM/REM -> 0 MAX-INT }T
T{ MIN-INT S>D 1 SM/REM -> 0 MIN-INT }T
T{ MAX-INT S>D MAX-INT SM/REM -> 0 1 }T
T{ MIN-INT S>D MIN-INT SM/REM -> 0 1 }T
T{ 1S 1 4 SM/REM -> 3 MAX-INT }T
T{ 2 MIN-INT M* 2 SM/REM -> 0 MIN-INT }T
T{ 2 MIN-INT M* MIN-INT SM/REM -> 0 2 }T
T{ 2 MAX-INT M* 2 SM/REM -> 0 MAX-INT }T
T{ 2 MAX-INT M* MAX-INT SM/REM -> 0 2 }T
T{ MIN-INT MIN-INT M* MIN-INT SM/REM -> 0 MIN-INT }T
T{ MIN-INT MAX-INT M* MIN-INT SM/REM -> 0 MAX-INT }T
T{ MIN-INT MAX-INT M* MAX-INT SM/REM -> 0 MIN-INT }T
T{ MAX-INT MAX-INT M* MAX-INT SM/REM -> 0 MAX-INT }T

T{ 0 0 1 UM/MOD -> 0 0 }T
T{ 1 0 1 UM/MOD -> 0 1 }T
T{ 1 0 2 UM/MOD -> 1 0 }T
T{ 3 0 2 UM/MOD -> 1 1 }T
T{ MAX-UINT 2 UM* 2 UM/MOD -> 0 MAX-UINT }T
T{ MAX-UINT 2 UM* MAX-UINT UM/MOD -> 0 2 }T
T{ MAX-UINT MAX-UINT UM* MAX-UINT UM/MOD -> 0 MAX-UINT }T

\ uCForth divides symmetric, so the reference words use SM/REM.
: T/MOD  >R S>D R> SM/REM ;
: T/     T/MOD SWAP DROP ;
: TMOD   T/MOD DROP ;
: T*/MOD >R M* R> SM/REM ;
: T*/    T*/MOD SWAP DROP ;

T{ 0 1 /MOD -> 0 1 T/MOD }T
T{ 1 1 /MOD -> 1 1 T/MOD }T
T{ 2 1 /MOD -> 2 1 T/MOD }T
T{ -1 1 /MOD -> -1 1 T/MOD }T
T{ -2 1 /MOD -> -2 1 T/MOD }T
T{ 0 -1 /MOD -> 0 -1 T/MOD }T
T{ 1 -1 /MOD -> 1 -1 T/MOD }T
T{ 2 -1 /MOD -> 2 -1 T/MOD }T
T{ -1 -1 /MOD -> -1 -1 T/MOD }T
T{ -2 -1 /MOD -> -2 -1 T/MOD }T
T{ 2 2 /MOD -> 2 2 T/MOD }T
T{ -1 -1 /MOD -> -1 -1 T/MOD }T
T{ -2 -2 /MOD -> -2 -2 T/MOD }T
T{ 7 3 /MOD -> 7 3 T/MOD }T
T{ 7 -3 /MOD -> 7 -3 T/MOD }T
T{ -7 3 /MOD -> -7 3 T/MOD }T
T{ -7 -3 /MOD -> -7 -3 T/MOD }T
T{ MAX-INT 1 /MOD -> MAX-INT 1 T/MOD }T
T{ MIN-INT 1 /MOD -> MIN-INT 1 T/MOD }T
T{ MAX-INT MAX-INT /MOD -> MAX-INT MAX-INT T/MOD }T
T{ MIN-INT MIN-INT /MOD -> MIN-INT MIN-INT T/MOD }T

T{ 0 1 / -> 0 1 T/ }T
T{ 1 1 / -> 1 1 T/ }T
T{ 2 1 / -> 2 1 T/ }T
T{ -1 1 / -> -1 1 T/ }T
T{ -2 1 / -> -2 1 T/ }T
T{ 0 -1 / -> 0 -1 T/ }T
T{ 1 -1 / -> 1 -1 T/ }T
T{ 2 -1 / -> 2 -1 T/ }T
T{ -1 -1 / -> -1 -1 T/ }T
T{ -2 -1 / -> -2 -1 T/ }T
T{ 2 2 / -> 2 2 T/ }T
T{ -1 -1 / -> -1 -1 T/ }T
T{ -2 -2 / -> -2 -2 T/ }T
T{ 7 3 / -> 7 3 T/ }T
T{ 7 -3 / -> 7 -3 T/ }T
T{ -7 3 / -> -7 3 T/ }T
T{ -7 -3 / -> -7 -3 T/ }T
T{ MAX-INT 1 / -> MAX-INT 1 T/ }T
T{ MIN-INT 1 / -> MIN-INT 1 T/ }T
T{ MAX-INT MAX-INT / -> MAX-INT MAX-INT T/ }T
T{ MIN-INT MIN-INT / -> MIN-INT MIN-INT T/ }T

T{ 0 1 MOD -> 0 1 TMOD }T
T{ 1 1 MOD -> 1 1 TMOD }T
T{ 2 1 MOD -> 2 1 TMOD }T
T{ -1 1 MOD -> -1 1 TMOD }T
T{ -2 1 MOD -> -2 1 TMOD }T
T{ 0 -1 MOD -> 0 -1 TMOD }T
T{ 1 -1 MOD -> 1 -1 TMOD }T
T{ 2 -1 MOD -> 2 -1 TMOD }T
T{ -1 -1 MOD -> -1 -1 TMOD }T
T{ -2 -1 MOD -> -2 -1 TMOD }T
T{ 2 2 MOD -> 2 2 TMOD }T
T{ -1 -1 MOD -> -1 -1 TMOD }T
T{ -2 -2 MOD -> -2 -2 TMOD }T
T{ 7 3 MOD -> 7 3 TMOD }T
T{ 7 -3 MOD -> 7 -3 TMOD }T
T{ -7 3 MOD -> -7 3 TMOD }T
T{ -7 -3 MOD -> -7 -3 TMOD }T
T{ MAX-INT 1 MOD -> MAX-INT 1 TMOD }T
T{ MIN-INT 1 MOD -> MIN-INT 1 TMOD }T
T{ MAX-INT MAX-INT MOD -> MAX-INT MAX-INT TMOD }T
T{ MIN-INT MIN-INT MOD -> MIN-INT MIN-INT TMOD }T

T{ 0 2 1 */ -> 0 2 1 T*/ }T
T{ 1 2 1 */ -> 1 2 1 T*/ }T
T{ 2 2 1 */ -> 2 2 1 T*/ }T
T{ -1 2 1 */ -> -1 2 1 T*/ }T
T{ -2 2 1 */ -> -2 2 1 T*/ }T
T{ 0 2 -1 */ -> 0 2 -1 T*/ }T
T{ 1 2 -1 */ -> 1 2 -1 T*/ }T
T{ 2 2 -1 */ -> 2 2 -1 T*/ }T
T{ -1 2 -1 */ -> -1 2 -1 T*/ }T
T{ -2 2 -1 */ -> -2 2 -1 T*/ }T
T{ 2 2 2 */ -> 2 2 2 T*/ }T
T{ -1 2 -1 */ -> -1 2 -1 T*/ }T
T{ -2 2 -2 */ -> -2 2 -2 T*/ }T
T{ 7 2 3 */ -> 7 2 3 T*/ }T
T{ 7 2 -3 */ -> 7 2 -3 T*/ }T
T{ -7 2 3 */ -> -7 2 3 T*/ }T
T{ -7 2 -3 */ -> -7 2 -3 T*/ }T
T{ MAX-INT 2 MAX-INT */ -> MAX-INT 2 MAX-INT T*/ }T
T{ MIN-INT 2 MIN-INT */ -> MIN-INT 2 MIN-INT T*/ }T

\ uCForth: the quotient rounds toward 0, the remainder has the sign of n1
T{ -7 2 / -> -3 }T
T{ -7 2 MOD -> -1 }T
T{ 7 -2 / -> -3 }T
T{ 7 -2 MOD -> 1 }T
T{ -7 -2 / -> 3 }T
T{ -7 -2 MOD -> -1 }T
T{ -3 2 / -> -1 }T
T{ 7 2 /MOD -> 1 3 }T
T{ -7 2 /MOD -> -1 -3 }T
T{ 512 5000 1023 */ -> 2502 }T                    \ the product has two cells
T{ MAX-INT 2 2 */ -> MAX-INT }T
T{ MAX-INT MAX-INT MAX-INT */ -> MAX-INT }T

\ ==== Memory: HERE , @ ! CELL+ CELLS C, C@ C! ALIGN ALIGNED +! ALLOT UNUSED ====

HERE 1 ALLOT
HERE
CONSTANT 2NDA
CONSTANT 1STA
T{ 1STA 2NDA U< -> <TRUE> }T              \ here must grow with allot
T{ 1STA 1+ -> 2NDA }T                     \ ... by one address unit
\ uCForth: a negative ALLOT gives the space back
T{ HERE 5 ALLOT -5 ALLOT HERE = -> <TRUE> }T
\ uCForth: UNUSED is the free space of the dictionary; ALLOT takes from it
T{ UNUSED 5 ALLOT UNUSED - -5 ALLOT -> 5 }T

HERE 1 ,
HERE 2 ,
CONSTANT 2ND
CONSTANT 1ST
T{ 1ST 2ND U< -> <TRUE> }T                \ here must grow with allot
T{ 1ST CELL+ -> 2ND }T                    \ ... by one cell
T{ 1ST 1 CELLS + -> 2ND }T
T{ 1ST @ 2ND @ -> 1 2 }T
T{ 5 1ST ! -> }T
T{ 1ST @ 2ND @ -> 5 2 }T
T{ 6 2ND ! -> }T
T{ 1ST @ 2ND @ -> 5 6 }T
T{ 1S 1ST !  1ST @ -> 1S }T               \ can store cell-wide value

HERE 1 C,
HERE 2 C,
CONSTANT 2NDC
CONSTANT 1STC
T{ 1STC 2NDC U< -> <TRUE> }T              \ here must grow with allot
T{ 1STC 1+ -> 2NDC }T                     \ ... by one char
T{ 1STC C@ 2NDC C@ -> 1 2 }T
T{ 3 1STC C! -> }T
T{ 1STC C@ 2NDC C@ -> 3 2 }T
T{ 4 2NDC C! -> }T
T{ 1STC C@ 2NDC C@ -> 3 4 }T

ALIGN 1 ALLOT HERE ALIGN HERE 3 CELLS ALLOT
CONSTANT A-ADDR  CONSTANT UA-ADDR
T{ UA-ADDR ALIGNED -> A-ADDR }T
T{    1 A-ADDR C!  A-ADDR C@ ->    1 }T
T{ 1234 A-ADDR  !  A-ADDR  @ -> 1234 }T
T{ 2 A-ADDR 1+ C!  A-ADDR 1+ C@ -> 2 }T
T{ 3 A-ADDR CELL+ C!  A-ADDR CELL+ C@ -> 3 }T
T{ 1234 A-ADDR CELL+ !  A-ADDR CELL+ @ -> 1234 }T

: BITS ( x -- u )
  0 SWAP BEGIN DUP WHILE DUP MSB AND IF >R 1+ R> THEN 2* REPEAT DROP ;
( cells >= 1 au, integral multiple of char size, >= 16 bits )
T{ 1 CELLS 1 < -> <FALSE> }T
T{ 1S BITS 16 < -> <FALSE> }T

T{ 0 1ST ! -> }T
T{ 1 1ST +! -> }T
T{ 1ST @ -> 1 }T
T{ -1 1ST +! 1ST @ -> 0 }T

\ uCForth: a character is one byte, the low byte comes first
T{ 1S BITS -> 1 CELLS 8 * }T
T{ 0 CELL+ -> 1 CELLS }T
T{ 3 CELLS -> 1 CELLS 3 * }T
T{ 0 ALIGNED -> 0 }T
T{ 1 ALIGNED ALIGNED -> 1 ALIGNED }T
T{ 1 ALIGNED 1 CELLS 1+ U< -> <TRUE> }T
T{ $1234 1ST !  1ST C@  1ST 1+ C@ -> $34 $12 }T
T{ MAX-INT 1ST !  1 1ST +!  1ST @ -> MIN-INT }T

\ ==== Characters and strings: 'c' S" COUNT ====

T{ 'X' -> 88 }T
T{ : GC1 'X' ; -> }T
T{ GC1 -> 88 }T
T{ : GC3 [ GC1 ] LITERAL ; -> }T
T{ GC3 -> 88 }T
T{ : GC4 S" XY" ; -> }T
T{ GC4 SWAP DROP -> 2 }T
T{ GC4 DROP DUP C@ SWAP 1+ C@ -> 'X' 'Y' }T

\ uCForth: S" at the prompt uses a buffer for at most 80 characters
T{ S" abc" NIP -> 3 }T
T{ S" abc" DROP C@ -> 'a' }T
T{ S" " NIP -> 0 }T
T{ S" 1234567890123456789012345678901234567890123456789012345678901234567890123456789012345" NIP -> 80 }T

CREATE GT1STRING 3 C, 'G' C, 'T' C, '1' C,
CREATE GT2STRING 3 C, 'G' C, 'T' C, '2' C,
T{ GT1STRING COUNT -> GT1STRING 1+ 3 }T
T{ GT1STRING COUNT S" GT1" S= -> <TRUE> }T

\ ==== Compiler: ' ['] EXECUTE IMMEDIATE LITERAL POSTPONE STATE FIND-NAME ====

T{ : GT1 123 ; -> }T
T{ ' GT1 EXECUTE -> 123 }T
T{ : GT2 ['] GT1 ; IMMEDIATE -> }T
T{ GT2 EXECUTE -> 123 }T
\ uCForth: FIND-NAME ( a u -- nt | 0 ) replaces FIND
T{ GT1STRING COUNT FIND-NAME NAME>XT -> ' GT1 }T
T{ GT1STRING COUNT FIND-NAME IMMEDIATE? -> <FALSE> }T
T{ GT2STRING COUNT FIND-NAME NAME>XT -> ' GT2 }T
T{ GT2STRING COUNT FIND-NAME IMMEDIATE? -> <TRUE> }T
T{ S" GT-NOT-THERE" FIND-NAME -> 0 }T
T{ : GT3 GT2 LITERAL ; -> }T
T{ GT3 -> ' GT1 }T

T{ : GT4 POSTPONE GT1 ; IMMEDIATE -> }T
T{ : GT5 GT4 ; -> }T
T{ GT5 -> 123 }T
T{ : GT6 345 ; IMMEDIATE -> }T
T{ : GT7 POSTPONE GT6 ; -> }T
T{ GT7 -> 345 }T

T{ : GT8 STATE @ ; IMMEDIATE -> }T
T{ GT8 -> 0 }T
T{ : GT9 GT8 LITERAL ; -> }T
T{ GT9 0= -> <FALSE> }T

\ uCForth: STATE is -1 while compiling, [ ] switch the state
T{ GT9 -> <TRUE> }T
T{ : GT10 [ 1 2 + ] LITERAL ; GT10 -> 3 }T

\ uCForth: comments. \ ends at the end of the line, ( ends there at the latest.
T{ 1 ( this comment has no end
2 -> 1 2 }T
T{ 3 \ -> 4 }T
-> 3 }T

\ ==== Control: IF ELSE THEN BEGIN WHILE REPEAT UNTIL AGAIN RECURSE ====

T{ : GI1 IF 123 THEN ; -> }T
T{ : GI2 IF 123 ELSE 234 THEN ; -> }T
T{ 0 GI1 -> }T
T{ 1 GI1 -> 123 }T
T{ -1 GI1 -> 123 }T
T{ 0 GI2 -> 234 }T
T{ 1 GI2 -> 123 }T
T{ -1 GI1 -> 123 }T

T{ : GI3 BEGIN DUP 5 < WHILE DUP 1+ REPEAT ; -> }T
T{ 0 GI3 -> 0 1 2 3 4 5 }T
T{ 4 GI3 -> 4 5 }T
T{ 5 GI3 -> 5 }T
T{ 6 GI3 -> 6 }T

T{ : GI4 BEGIN DUP 1+ DUP 5 > UNTIL ; -> }T
T{ 3 GI4 -> 3 4 5 6 }T
T{ 5 GI4 -> 5 6 }T
T{ 6 GI4 -> 6 7 }T

T{ : GI5 BEGIN DUP 2 > WHILE DUP 5 < WHILE DUP 1+ REPEAT 123 ELSE 345 THEN ; -> }T
T{ 1 GI5 -> 1 345 }T
T{ 2 GI5 -> 2 345 }T
T{ 3 GI5 -> 3 4 5 123 }T
T{ 4 GI5 -> 4 5 123 }T
T{ 5 GI5 -> 5 123 }T

T{ : GI6 ( N -- 0,1,..N ) DUP IF DUP >R 1- RECURSE R> THEN ; -> }T
T{ 0 GI6 -> 0 }T
T{ 1 GI6 -> 0 1 }T
T{ 2 GI6 -> 0 1 2 }T
T{ 3 GI6 -> 0 1 2 3 }T
T{ 4 GI6 -> 0 1 2 3 4 }T

\ uCForth: AGAIN ends only through EXIT
T{ : GI7 ( n -- n' ) BEGIN 1+ DUP 5 > IF EXIT THEN AGAIN ; -> }T
T{ 0 GI7 -> 6 }T
T{ 9 GI7 -> 10 }T

\ ==== Counted loops: DO ?DO LOOP +LOOP I J UNLOOP LEAVE EXIT ====

T{ : GD1 DO I LOOP ; -> }T
T{ 4 1 GD1 -> 1 2 3 }T
T{ 2 -1 GD1 -> -1 0 1 }T
T{ MID-UINT+1 MID-UINT GD1 -> MID-UINT }T

T{ : GD2 DO I -1 +LOOP ; -> }T
T{ 1 4 GD2 -> 4 3 2 1 }T
T{ -1 2 GD2 -> 2 1 0 -1 }T
T{ MID-UINT MID-UINT+1 GD2 -> MID-UINT+1 MID-UINT }T

T{ : GD3 DO 1 0 DO J LOOP LOOP ; -> }T
T{ 4 1 GD3 -> 1 2 3 }T
T{ 2 -1 GD3 -> -1 0 1 }T
T{ MID-UINT+1 MID-UINT GD3 -> MID-UINT }T

T{ : GD4 DO 1 0 DO J LOOP -1 +LOOP ; -> }T
T{ 1 4 GD4 -> 4 3 2 1 }T
T{ -1 2 GD4 -> 2 1 0 -1 }T
T{ MID-UINT MID-UINT+1 GD4 -> MID-UINT+1 MID-UINT }T

T{ : GD5 123 SWAP 0 DO I 4 > IF DROP 234 LEAVE THEN LOOP ; -> }T
T{ 1 GD5 -> 123 }T
T{ 5 GD5 -> 123 }T
T{ 6 GD5 -> 234 }T

T{ : GD6  ( PAT: {0 0},{0 0}{1 0}{1 1},{0 0}{1 0}{1 1}{2 0}{2 1}{2 2} )
   0 SWAP 0 DO
      I 1+ 0 DO I J + 3 = IF I UNLOOP I UNLOOP EXIT THEN 1+ LOOP
    LOOP ; -> }T
T{ 1 GD6 -> 1 }T
T{ 2 GD6 -> 3 }T
T{ 3 GD6 -> 4 1 2 }T

\ uCForth: ?DO skips the loop for limit = start
T{ : GD7 ?DO I LOOP ; -> }T
T{ 4 1 GD7 -> 1 2 3 }T
T{ 1 1 GD7 -> }T
T{ MIN-INT MIN-INT GD7 -> }T
T{ : GD8 ?DO I LEAVE LOOP ; -> }T
T{ 5 3 GD8 -> 3 }T
T{ 5 5 GD8 -> }T

\ uCForth: +LOOP ends when the index crosses the boundary between
\ limit-1 and limit, in both directions. GD9 stops after 6 passes and
\ gives the indexes and the number of passes.
VARIABLE STEP
VARIABLE PASSES
: GD9 ( limit start step -- i*x n )
  STEP !  0 PASSES !
  DO  1 PASSES +!  I  PASSES @ 6 = IF LEAVE THEN  STEP @ +LOOP
  PASSES @ ;
T{  10  0  3 GD9 -> 0 3 6 9 4 }T
T{  10  0  5 GD9 -> 0 5 2 }T
T{   0 10 -3 GD9 -> 10 7 4 1 4 }T
T{   0 10 -5 GD9 -> 10 5 0 3 }T
T{   4  4 -1 GD9 -> 4 1 }T
T{   1  4 -1 GD9 -> 4 3 2 1 4 }T
T{   4  1 -1 GD9 -> 1 0 -1 -2 -3 -4 6 }T
T{   4  1  0 GD9 -> 1 1 1 1 1 1 6 }T
T{   0  0  0 GD9 -> 0 0 0 0 0 0 6 }T
T{   1  4  0 GD9 -> 4 4 4 4 4 4 6 }T
T{   1  4  1 GD9 -> 4 5 6 7 8 9 6 }T
T{   4  1  1 GD9 -> 1 2 3 3 }T
T{   4  4  1 GD9 -> 4 5 6 7 8 9 6 }T
T{   2 -1 -1 GD9 -> -1 -2 -3 -4 -5 -6 6 }T
T{  -1  2 -1 GD9 -> 2 1 0 -1 4 }T
T{   2 -1  0 GD9 -> -1 -1 -1 -1 -1 -1 6 }T
T{  -1  2  0 GD9 -> 2 2 2 2 2 2 6 }T
T{  -1  2  1 GD9 -> 2 3 4 5 6 7 6 }T
T{   2 -1  1 GD9 -> -1 0 1 3 }T
T{ -20 30 -10 GD9 -> 30 20 10 0 -10 -20 6 }T
T{ -20 31 -10 GD9 -> 31 21 11 1 -9 -19 6 }T
T{ -20 29 -10 GD9 -> 29 19 9 -1 -11 5 }T

\ uCForth: +LOOP with big steps over the whole number range
MAX-UINT 8 RSHIFT 1+ CONSTANT USTEP      \ 256 with 16-bit cells
USTEP NEGATE CONSTANT -USTEP
MAX-INT 7 RSHIFT 1+ CONSTANT SSTEP
SSTEP NEGATE CONSTANT -SSTEP
: GD10 ( n limit start step -- n' )  STEP !  DO 1+ STEP @ +LOOP ;
T{ 0 MAX-UINT 0 USTEP GD10 -> 256 }T
T{ 0 0 MAX-UINT -USTEP GD10 -> 256 }T
T{ 0 MAX-INT MIN-INT SSTEP GD10 -> 256 }T
T{ 0 MIN-INT MAX-INT -SSTEP GD10 -> 256 }T

\ uCForth: LEAVE leaves the inner loop only
T{ : GD11 3 0 DO 3 0 DO I 1 = IF LEAVE THEN J I LOOP LOOP ; -> }T
T{ GD11 -> 0 0 1 0 2 0 }T

\ ==== CASE OF ENDOF ENDCASE ====

: CS1 ( n -- x )
  CASE
    1 OF 111 ENDOF
    2 OF 222 ENDOF
    MIN-INT OF 333 ENDOF
    DUP 1000 + SWAP              \ no case matches: the result under n
  ENDCASE ;
T{ 1 CS1 -> 111 }T
T{ 2 CS1 -> 222 }T
T{ MIN-INT CS1 -> 333 }T
T{ 3 CS1 -> 1003 }T
T{ -1 CS1 -> 999 }T
: CS2 ( n -- i*x )  CASE 1 OF 10 ENDOF ENDCASE ;    \ ENDCASE drops n
T{ 1 CS2 -> 10 }T
T{ 5 CS2 -> }T
: CS3 ( n -- )  CASE ENDCASE ;
T{ 7 CS3 -> }T
: CS4 ( b a -- x )                 \ a CASE in a CASE
  CASE
    1 OF  CASE 1 OF 11 ENDOF 2 OF 12 ENDOF >R 10 R> ENDCASE  ENDOF
    2 OF  20 +  ENDOF
    NIP 99 SWAP
  ENDCASE ;
T{ 1 1 CS4 -> 11 }T
T{ 2 1 CS4 -> 12 }T
T{ 3 1 CS4 -> 10 }T
T{ 5 2 CS4 -> 25 }T
T{ 5 3 CS4 -> 99 }T

\ ==== Defining words: : ; CONSTANT VARIABLE CREATE DOES> >BODY ====

T{ 123 CONSTANT X123 -> }T
T{ X123 -> 123 }T
T{ : EQU CONSTANT ; -> }T
T{ X123 EQU Y123 -> }T
T{ Y123 -> 123 }T

T{ VARIABLE V1 -> }T
T{ 123 V1 ! -> }T
T{ V1 @ -> 123 }T

T{ : NOP : POSTPONE ; ; -> }T
T{ NOP NOP1 NOP NOP2 -> }T
T{ NOP1 -> }T
T{ NOP2 -> }T

T{ : DOES1 DOES> @ 1 + ; -> }T
T{ : DOES2 DOES> @ 2 + ; -> }T
T{ CREATE CR1 -> }T
T{ CR1 -> HERE }T
T{ ' CR1 >BODY -> HERE }T
T{ 1 , -> }T
T{ CR1 @ -> 1 }T
T{ DOES1 -> }T
T{ CR1 -> 2 }T
T{ DOES2 -> }T
T{ CR1 -> 3 }T

T{ : WEIRD: CREATE DOES> 1 + DOES> 2 + ; -> }T
T{ WEIRD: W1 -> }T
T{ ' W1 >BODY -> HERE }T
T{ W1 -> HERE 1 + }T
T{ W1 -> HERE 2 + }T

\ uCForth: VARIABLE starts with 0 and lives in the RAM data region, or in
\ its extension when the target has one (core.fs: XVP, XDATA-END)
: DATA? ( a -- f )   DUP DATA-END U< IF VP @ U< ELSE XVP @ U< THEN ;
T{ VARIABLE V2 V2 @ -> 0 }T
T{ V2 DATA? -> <TRUE> }T

\ ==== Data: VALUE TO BUFFER: DEFER IS ON! OFF! ====

T{ 111 VALUE VAL1 -> }T
T{ VAL1 -> 111 }T
T{ 222 TO VAL1 VAL1 -> 222 }T
T{ : VD1 TO VAL1 ; -> }T                  \ TO in a definition
T{ 333 VD1 VAL1 -> 333 }T
T{ -1 VALUE VAL2 VAL2 -> -1 }T
T{ VAL1 VAL2 -> 333 -1 }T

T{ V2 ON!  V2 @ -> <TRUE> }T
T{ V2 OFF!  V2 @ -> <FALSE> }T

T{ 5 BUFFER: BUF1 -> }T
T{ 5 BUFFER: BUF2 -> }T
T{ BUF1 ALIGNED -> BUF1 }T
T{ BUF2 BUF1 5 + U< -> <FALSE> }T          \ the buffers do not overlap
T{ BUF1 5 'a' FILL  BUF2 5 'b' FILL  BUF1 4 + C@  BUF2 C@ -> 'a' 'b' }T
T{ BUF2 DATA? -> <TRUE> }T

T{ DEFER DF1 -> }T
T{ DF1 -> }T                               \ at first DF1 does nothing
T{ ' DF1 >BODY @ DATA? -> <TRUE> }T      \ the behavior lives in the RAM data
T{ ' DUP IS DF1 -> }T
T{ 5 DF1 -> 5 5 }T
T{ : DF-SET ['] SWAP IS DF1 ; -> }T         \ IS in a definition
T{ DF-SET 1 2 DF1 -> 2 1 }T
T{ : DF-USE 7 8 DF1 ; -> }T
T{ ' OVER IS DF1 DF-USE -> 7 8 7 }T         \ the behavior can change later

\ ==== Number output: <# # #S #> HOLD SIGN BASE HEX DECIMAL ====

: GP1  <# 'A' HOLD 'B' HOLD 0 0 #> S" BA" S= ;
T{ GP1 -> <TRUE> }T

: GP2  <# -1 SIGN 0 SIGN -1 SIGN 0 0 #> S" --" S= ;
T{ GP2 -> <TRUE> }T

: GP3  <# 1 0 # # #> S" 01" S= ;
T{ GP3 -> <TRUE> }T

: GP4  <# 1 0 #S #> S" 1" S= ;
T{ GP4 -> <TRUE> }T

36 CONSTANT MAX-BASE                       \ base 2 .. 36
#BITS 2* CONSTANT #BITS-UD                 \ number of bits in ud

: GP5
   BASE @ <TRUE>
   MAX-BASE 1+ 2 DO                        \ for each possible base
      I BASE !
      I 0 <# #S #> S" 10" S= AND
   LOOP
   SWAP BASE ! ;
T{ GP5 -> <TRUE> }T

: GP6
   BASE @ >R  2 BASE !
   MAX-UINT MAX-UINT <# #S #>              \ maximum ud to binary
   R> BASE !
   DUP #BITS-UD = SWAP
   0 ?DO
      OVER C@ '1' = AND                    \ all ones
      >R 1+ R>
   LOOP SWAP DROP ;
T{ GP6 -> <TRUE> }T

: GP7
   BASE @ >R    MAX-BASE BASE !
   <TRUE>
   10 0 DO
      I 0 <# #S #>
      1 = SWAP C@ I '0' + = AND AND
   LOOP
   MAX-BASE 10 DO
      I 0 <# #S #>
      1 = SWAP C@ 'A' I 10 - + = AND AND
   LOOP
   R> BASE ! ;
T{ GP7 -> <TRUE> }T

: GN2  BASE @ >R  HEX BASE @  DECIMAL BASE @  R> BASE ! ;
T{ GN2 -> 16 10 }T

\ uCForth: more formats
T{ 1234 0 <# # # # '.' HOLD #S #> S" 1.234" S= -> <TRUE> }T
T{ -56 DUP ABS 0 <# #S ROT SIGN #> S" -56" S= -> <TRUE> }T
T{ 0 0 <# #S #> S" 0" S= -> <TRUE> }T      \ #S gives at least one digit
T{ 123 0 <# #S -> 0 0 }T
T{ HEX 0 1 <# # HLD @ C@ DECIMAL -> MSB 3 RSHIFT 0 '0' }T   \ # divides the double number
T{ BASE @ -> 10 }T

\ ==== Number input: BASE and the prefixes $ # % 'c' ====

T{ $FF #99 %1010 'A' -> 255 99 10 65 }T
T{ $ff $0 %0 #0 -> 255 0 0 0 }T
T{ -$10 -#10 -%11 -7 -> -16 -10 -3 0 7 - }T
T{ HEX 10 #10 $10 %10 DECIMAL -> 16 10 16 2 }T
T{ HEX ff FF -ff DECIMAL -> 255 255 -255 }T
T{ 2 BASE ! 1010 -1 DECIMAL -> 10 -1 }T
T{ 36 BASE ! Z zz DECIMAL -> 35 1295 }T
T{ '0' 'a' '~' ''' -> 48 97 126 39 }T
T{ -1 -> MAX-UINT }T
T{ : GN3 $10 'b' -5 ; GN3 -> 16 98 -5 }T

\ ==== Fill and move: FILL MOVE ====

CREATE FBUF $00 C, $00 C, $00 C,
CREATE SBUF $12 C, $34 C, $56 C,
: SEEBUF FBUF C@  FBUF 1+ C@  FBUF 1+ 1+ C@ ;

T{ FBUF 0 $20 FILL -> }T
T{ SEEBUF -> $00 $00 $00 }T

T{ FBUF 1 $20 FILL -> }T
T{ SEEBUF -> $20 $00 $00 }T

T{ FBUF 3 $20 FILL -> }T
T{ SEEBUF -> $20 $20 $20 }T

T{ FBUF FBUF 3 MOVE -> }T                  \ bizarre special case
T{ SEEBUF -> $20 $20 $20 }T

T{ SBUF FBUF 0 MOVE -> }T
T{ SEEBUF -> $20 $20 $20 }T

T{ SBUF FBUF 1 MOVE -> }T
T{ SEEBUF -> $12 $20 $20 }T

T{ SBUF FBUF 3 MOVE -> }T
T{ SEEBUF -> $12 $34 $56 }T

T{ FBUF FBUF 1+ 2 MOVE -> }T
T{ SEEBUF -> $12 $12 $34 }T

T{ FBUF 1+ FBUF 2 MOVE -> }T
T{ SEEBUF -> $12 $34 $34 }T

\ uCForth: longer overlaps in both directions, FILL stores the low byte
16 BUFFER: MBUF
: MB-INIT ( -- )   16 0 DO I MBUF I + C! LOOP ;
: MB@ ( i -- c )   MBUF + C@ ;
T{ MB-INIT  MBUF MBUF 4 + 8 MOVE  0 MB@ 4 MB@ 11 MB@ 12 MB@ -> 0 0 7 12 }T
T{ MB-INIT  MBUF 4 + MBUF 8 MOVE  0 MB@ 7 MB@ 8 MB@ -> 4 11 8 }T
T{ MB-INIT  MBUF 1+ 2 $1FF FILL  0 MB@ 1 MB@ 2 MB@ 3 MB@ -> 0 $FF $FF 3 }T

\ ==== Errors: CATCH THROW ABORT ABORT" QUIT ====

: T-NOTHROW ( -- x )  42 ;
: T-THROW3 ( -- )  1 2 3 THROW ;
: T-EAT ( x1 x2 -- )  2DROP 7 8 9 3 THROW ;
T{ ' T-NOTHROW CATCH -> 42 0 }T
T{ ' T-THROW3 CATCH -> 3 }T
T{ 5 ' T-THROW3 CATCH -> 5 3 }T
\ After THROW the stack has the depth from before CATCH. The values can change.
T{ 1 2 ' T-EAT CATCH NIP NIP -> 3 }T
T{ 0 THROW -> }T
T{ ' NOOP CATCH -> 0 }T

: T-DEEP1 ( -- )  10 0 DO I 5 = IF I THROW THEN LOOP ;
: T-DEEP2 ( -- )  1 >R 2 >R T-DEEP1 R> R> 2DROP ;
T{ ' T-DEEP2 CATCH -> 5 }T                 \ THROW leaves loops and nested words
: T-INNER ( -- code )  ['] T-THROW3 CATCH ;
T{ ' T-INNER CATCH -> 3 0 }T
: T-RETHROW ( -- )  ['] T-THROW3 CATCH THROW ;
T{ ' T-RETHROW CATCH -> 3 }T
T{ HANDLER @  ' T-THROW3 CATCH DROP  HANDLER @ = -> <TRUE> }T

T{ ' ABORT CATCH -> -1 }T
\ No test for QUIT: it empties the return stack and reads the next line
\ from the console, so it never returns to CATCH. (The old test here
\ never ran to its end; it did not count.)
: T-ABQ ( f -- )  ABORT" oops" ;
T{ 0 ' T-ABQ CATCH -> 0 }T
T{ 1 ' T-ABQ CATCH NIP  ERRMSG CELL+ @  ERRMSG @ C@ -> -2 4 'o' }T
: T-POS ( -- )  1000 THROW ;
T{ ' T-POS CATCH -> 1000 }T
: T-TICK ( "name" -- xt )  ' ;
T{ ' T-TICK CATCH no-such-word -> -13 }T
: T-UNDEF ( -- )  S" nothing" UNDEFINED ;
T{ ' T-UNDEF CATCH -> -13 }T

\ ==== Output: EMIT TYPE CR SPACE SPACES . U. .R ." .( .S ====

T{ OUT{ 'H' EMIT 'i' EMIT }OUT S" Hi" S= -> <TRUE> }T
T{ OUT{ S" abc" TYPE }OUT S" abc" S= -> <TRUE> }T
T{ OUT{ S" abc" DROP 0 TYPE }OUT NIP -> 0 }T
T{ OUT{ CR }OUT DROP DUP C@ SWAP 1+ C@ -> 13 10 }T
T{ OUT{ CR }OUT NIP -> 2 }T
T{ OUT{ SPACE }OUT S"  " S= -> <TRUE> }T
T{ OUT{ 3 SPACES }OUT S"    " S= -> <TRUE> }T
T{ OUT{ 0 SPACES -3 SPACES }OUT NIP -> 0 }T
T{ OUT{ 5 . -5 . 0 . }OUT S" 5 -5 0 " S= -> <TRUE> }T
: DOT-MIN ( -- f1 f2 )   \ . of MIN-INT: the minus, the digits of MIN-INT as unsigned, a space
  OUT{ MIN-INT . }OUT  OVER C@ '-' = >R  2 - SWAP 1+ SWAP  MIN-INT 0 <# #S #> S=  R> ;
T{ DOT-MIN -> <TRUE> <TRUE> }T
T{ OUT{ 5 U. 0 U. }OUT S" 5 0 " S= -> <TRUE> }T
T{ OUT{ -1 U. }OUT 1-  MAX-UINT 0 <# #S #> S= -> <TRUE> }T
T{ OUT{ 123 6 .R }OUT S"    123" S= -> <TRUE> }T
T{ OUT{ -12 4 .R }OUT S"  -12" S= -> <TRUE> }T
T{ OUT{ 12345 2 .R }OUT S" 12345" S= -> <TRUE> }T   \ the field is too small: all digits
T{ OUT{ 255 HEX . DECIMAL }OUT S" FF " S= -> <TRUE> }T
T{ OUT{ HEX BASE @ . DECIMAL }OUT S" 10 " S= -> <TRUE> }T
: GO1 ( -- )  ." hello" ;
T{ OUT{ GO1 }OUT S" hello" S= -> <TRUE> }T
T{ OUT{ ." xy" }OUT S" xy" S= -> <TRUE> }T
T{ OUT{ .( at once) }OUT S" at once" S= -> <TRUE> }T
T{ OUT{ .S }OUT S" <0> " S= -> <TRUE> }T
T{ 1 -2 OUT{ .S }OUT S" <2> 1 -2 " S= -> 1 -2 <TRUE> }T

\ ==== Input: KEY KEY? ACCEPT, through the vector 'KEY? ====

CREATE IN-TEXT  'h' C, 'i' C, 13 C, 'x' C,          \ hi Enter x
CREATE IN-BS    'a' C, 'b' C, 8 C, 'c' C, 13 C,     \ ab Backspace c Enter
CREATE IN-DEL   8 C, 'a' C, 'b' C, 127 C, 13 C,     \ Backspace ab Delete Enter
CREATE IN-LONG  'a' C, 'b' C, 'c' C, 'd' C, 'e' C, 13 C,
CREATE IN-LF    'z' C, 10 C,
16 BUFFER: ABUF

T{ KEY? -> <FALSE> }T                      \ while a file loads, no key is there
T{ S" ab" IN{ KEY KEY }IN -> 'a' 'b' }T
T{ S" " IN{ KEY? }IN -> <FALSE> }T
T{ S" q" IN{ KEY? KEY? KEY }IN -> <TRUE> <TRUE> 'q' }T   \ KEY? keeps the character for KEY
T{ IN-TEXT 4 IN{ ABUF 16 ACCEPT KEY }IN -> 2 'x' }T
T{ ABUF 2 S" hi" S= -> <TRUE> }T
T{ IN-BS 5 IN{ ABUF 16 ACCEPT }IN ABUF SWAP S" ac" S= -> <TRUE> }T
T{ IN-DEL 5 IN{ ABUF 16 ACCEPT }IN ABUF SWAP S" a" S= -> <TRUE> }T
T{ ABUF 16 0 FILL  IN-LONG 6 IN{ ABUF 3 ACCEPT }IN  ABUF 2 + C@  ABUF 3 + C@ -> 3 'c' 0 }T
T{ IN-LF 2 IN{ ABUF 16 ACCEPT }IN -> 1 }T
\ ECHO: a b, Backspace as BS SPACE BS, c, and a space at the end of the line
T{ ECHO ON!  IN-BS 5 IN{ OUT{ ABUF 16 ACCEPT }OUT }IN  ECHO0 ECHO !  NIP -> 2 7 }T

\ ==== Search order: VOCABULARY ALSO ONLY PREVIOUS DEFINITIONS FORTH ORDER WORDS VOCS ====

VOCABULARY TVOC
ALSO TVOC DEFINITIONS
: TV1 ( -- x )  11 ;
: TV2 ( -- x )  22 ;
RESET-ORDER
: IN-TVOC? ( a u -- f )  ['] TVOC >BODY @ WL-FIND NIP NIP 0<> ;

T{ S" TV1" FIND-NAME -> 0 }T               \ TVOC is not in the search order
T{ ALSO TVOC TV1 TV2 PREVIOUS -> 11 22 }T
T{ S" TV1" WL-FORTH WL-FIND NIP NIP -> 0 }T
T{ S" TV1" IN-TVOC? -> <TRUE> }T
T{ #ORDER @  CURRENT @ -> 2 WL-FORTH }T
T{ CONTEXT @  CONTEXT CELL+ @  TOP @ -> WL-FORTH WL-INTERNALS WL-INTERNALS }T
T{ ALSO #ORDER @ PREVIOUS #ORDER @ -> 3 2 }T
T{ ALSO FORTH TOP @ PREVIOUS -> WL-FORTH }T
T{ ALSO INTERNALS TOP @ PREVIOUS -> WL-INTERNALS }T
T{ OUT{ ORDER }OUT S" INTERNALS FORTH   current: FORTH " S= -> <TRUE> }T
T{ ALSO TVOC OUT{ ORDER }OUT PREVIOUS S" TVOC INTERNALS FORTH   current: FORTH " S= -> <TRUE> }T
T{ ALSO TVOC OUT{ WORDS }OUT PREVIOUS AFTER-CR S" TV2 TV1 " S= -> <TRUE> }T
: T-ONLY ( -- n wl )  ONLY #ORDER @ CONTEXT @ RESET-ORDER ;
T{ T-ONLY -> 1 WL-FORTH }T
: T-PREVIOUS ( -- n )  ONLY PREVIOUS PREVIOUS #ORDER @ RESET-ORDER ;
T{ T-PREVIOUS -> 1 }T                       \ one wordlist stays
: T-ALSO16 ( -- )  16 0 DO ALSO LOOP ;
T{ ' T-ALSO16 CATCH #ORDER @ RESET-ORDER -> -49 16 }T  \ at most 16 wordlists
T{ WORDLIST DUP @ SWAP VOC-LINK @ = -> 0 <TRUE> }T
T{ OUT{ WL-FORTH .WL WORDLIST .WL }OUT S" FORTH ? " S= -> <TRUE> }T
T{ OUT{ 0 OUT ! VOCS }OUT DROP 9 S" ? ? TVOC " S= -> <TRUE> }T   \ the newest first

\ ==== Names and the dictionary ====

\ A new word is visible only after ;
T{ : GDX [ S" GDX" FIND-NAME ] LITERAL ; -> }T
T{ GDX -> 0 }T
T{ S" GDX" FIND-NAME 0<> -> <TRUE> }T

\ The search does not see the case of letters, the header keeps it.
T{ 1 dup -> 1 1 }T
T{ S" dUp" FIND-NAME  S" DUP" FIND-NAME = -> <TRUE> }T
T{ CASE-SENSE @ -> 0 }T
T{ CASE-SENSITIVE S" dup" FIND-NAME CASE-INSENSITIVE -> 0 }T
T{ CASE-SENSITIVE S" DUP" FIND-NAME 0<> CASE-INSENSITIVE -> <TRUE> }T
: MixedCase ( -- ) ;
T{ OUT{ S" MIXEDCASE" FIND-NAME .ID }OUT S" MixedCase" S= -> <TRUE> }T

\ MARKER forgets itself and all newer words, data and wordlists.
VARIABLE MK-HERE
VARIABLE MK-VP
VARIABLE MK-LINK
HERE MK-HERE !  VP @ MK-VP !  VOC-LINK @ MK-LINK !  MARKER MK-FORGET
: MK-WORD ( -- ) ;
7 BUFFER: MK-BUF
VOCABULARY MK-VOC
ALSO TVOC DEFINITIONS
: MK-TVOC ( -- ) ;                          \ a new word in an older wordlist
RESET-ORDER
T{ MK-FORGET -> }T
T{ RESET-ORDER -> }T                        \ MARKER leaves ONLY FORTH DEFINITIONS
T{ S" MK-WORD" FIND-NAME  S" MK-FORGET" FIND-NAME  S" MK-VOC" FIND-NAME -> 0 0 0 }T
T{ S" MK-TVOC" IN-TVOC?  S" TV1" IN-TVOC? -> <FALSE> <TRUE> }T
T{ HERE MK-HERE @ =  VP @ MK-VP @ =  VOC-LINK @ MK-LINK @ = -> <TRUE> <TRUE> <TRUE> }T

\ HIDE takes a word out of its wordlist; older words with the name come back.
: HD-A ( -- n ) 1 ;
: HD-B ( -- n ) 2 ;
: HD-A ( -- n ) 3 ;
: HD-C ( -- n ) HD-A ;
T{ HD-A HD-C -> 3 3 }T
T{ HIDE HD-A -> }T
T{ HD-A HD-B HD-C -> 1 2 3 }T               \ HD-C keeps its code
T{ HIDE HD-B  S" HD-B" FIND-NAME -> 0 }T
T{ HIDE HD-C  S" HD-C" FIND-NAME -> 0 }T      \ the newest word
T{ HD-A -> 1 }T

\ The RAM data can grow into an extension (core.fs: XVP, XDATA-END). Here
\ a part of the dictionary plays the extension.
CREATE XD-AREA 32 ALLOT
VARIABLE XD-VP   VARIABLE XD-X   VARIABLE XD-OLD   VARIABLE XD-OLD-END
: XD-ON  ( -- )   \ the extension of the target (if any) waits
  XVP @ XD-OLD !  XDATA-END @ XD-OLD-END !  XD-AREA XVP !  XD-AREA 32 + XDATA-END ! ;
: XD-OFF ( -- )  XD-OLD @ XVP !  XD-OLD-END @ XDATA-END ! ;
VP @ XD-VP !
T{ XD-ON  VARIABLE XD-V1  XD-V1 XD-AREA = -> <TRUE> }T      \ VARIABLE goes into the extension
T{ 8 BUFFER: XD-B1  XD-B1 XD-AREA CELL+ = -> <TRUE> }T
T{ 5 VALUE XD-VAL  XD-VAL  XVP @ XD-AREA - -> 5 CELL 8 + CELL + }T
T{ VP @ XD-VP @ = -> <TRUE> }T                               \ the RAM data region stays
T{ XVP @ XD-X !  MARKER XD-M  CELL VALLOT DROP  XD-M -> }T
T{ RESET-ORDER  XVP @ XD-X @ = -> <TRUE> }T                  \ MARKER sets XVP back
T{ 64 ' VALLOT CATCH NIP -> -8 }T                            \ the extension is full
T{ CELL DATA-ALLOT XD-VP @ = -> <TRUE> }T                    \ DATA-ALLOT: the RAM data region
T{ 0 XDATA-END !  CELL VALLOT  XD-VP @ CELL+ =  XD-OFF -> <TRUE> }T   \ without an extension: the RAM data region

\ ==== Tools: .S (above) DUMP SEE ====

: AFTER-COLON ( a u -- a' u' )   \ skip the address of a DUMP line and ": "
  BEGIN DUP WHILE OVER C@ ':' <> WHILE 1- SWAP 1+ SWAP REPEAT 2 - SWAP 2 + SWAP THEN ;
T{ OUT{ SBUF 3 DUMP }OUT DROP C@ -> 13 }T
T{ OUT{ SBUF 3 DUMP }OUT AFTER-COLON NIP -> 64 }T         \ always a whole line of 16 bytes
T{ OUT{ SBUF 3 DUMP }OUT AFTER-COLON DROP 8 S" 12 34 56" S= -> <TRUE> }T
T{ OUT{ SEE X123 }OUT AFTER-CR S" X123 constant 123 " S= -> <TRUE> }T
T{ OUT{ SEE DUP }OUT AFTER-CR S" DUP primitive 13 " S= -> <TRUE> }T

\ ==== INTERNALS: system variables ====

T{ DP @ -> HERE }T
T{ BASE-END @ FROZEN-END @ U> -> <FALSE> }T
T{ HERE FROZEN-END @ U< -> <FALSE> }T
T{ AUTORUN @ -> 0 }T
T{ VP @ DATA-END U< -> <TRUE> }T
T{ >IN @ >IN @ - -> -6 }T                   \ each ">IN @ " moves >IN by 6
T{ TIB C@ -> 'T' }T                          \ this line starts with T
T{ 'SETUP @ 'LOOP @ -> ' NOOP ' NOOP }T
T{ 'EMIT @ 'KEY? @ KEYBUF @ ECHO @ -> EMIT0 ' ?RX -1 ECHO0 }T
T{ S0 SP@ - -> DEPTH 1+ CELLS }T
: LAT1 ( -- ) ;
T{ LATEST @ NAME>XT -> ' LAT1 }T
: LAT2 ( -- nt )  [ LATEST @ ] LITERAL ;   \ LATEST is set before REVEAL
T{ LAT2 NAME>XT -> ' LAT2 }T

\ ==== INTERNALS: parser, headers, flags ====

: PN1 ( "name" -- a u )  PARSE-NAME ;
T{ PN1 hello NIP -> 5 }T
T{ PN1    spaced NIP -> 6 }T
: PN-END ( -- u )  >IN @ >R  #TIB @ >IN !  PARSE-NAME NIP  R> >IN ! ;
T{ PN-END -> 0 }T                          \ at the end of the line u is 0
T{ 41 PARSE abc) NIP -> 3 }T
T{ S" HDR1" HEADER 2 , 77 , REVEAL HDR1 -> 77 }T
T{ ' HDR1 @ -> 2 }T
T{ S" HDR2" HEADER  S" HDR2" FIND-NAME  2 , 5 , REVEAL  S" HDR2" FIND-NAME 0<> -> 0 <TRUE> }T
: IM1 ( -- ) ; IMMEDIATE
T{ S" IM1" FIND-NAME IMMEDIATE? -> <TRUE> }T
: CO1 ( -- ) ; COMPILE-ONLY
T{ S" CO1" FIND-NAME CELL+ C@ 64 AND -> 64 }T
: UC1 ( -- ) ; UNCHECKED
T{ S" UC1" FIND-NAME CELL+ C@ 32 AND -> 32 }T
: FL1 ( -- ) ; 128 FLAG!
T{ S" FL1" FIND-NAME IMMEDIATE? -> <TRUE> }T
T{ S" DUP" FIND-NAME NAME>XT -> ' DUP }T
T{ ' DUP >NAME  S" DUP" FIND-NAME = -> <TRUE> }T
T{ HERE >NAME -> 0 }T
T{ OUT{ ' DUP .NAME HERE .NAME }OUT DROP 4 S" DUP?" S= -> <TRUE> }T
T{ OUT{ HERE .NAME }OUT NIP -> CELL 2* 1+ }T   \ without a name: ? and the xt in hex
T{ S" DUP" WL-FORTH WL-FIND NIP NIP  S" DUP" FIND-NAME = -> <TRUE> }T
T{ S" DUP" WL-INTERNALS WL-FIND NIP NIP -> 0 }T
T{ S" LIT" WL-INTERNALS WL-FIND NIP NIP 0<> -> <TRUE> }T
T{ 'a' UPC 'z' UPC 'A' UPC '1' UPC '{' UPC -> 'A' 'Z' 'A' '1' '{' }T
: SAME1 S" aBc" ;
: SAME2 S" AbC" ;
: SAME3 S" abd" ;
T{ SAME1 DROP SAME2 SAME? -> <TRUE> }T
T{ SAME1 DROP SAME3 SAME? -> <FALSE> }T
T{ SAME1 DROP SAME3 DROP 2 SAME? -> <TRUE> }T
>INTERNALS
: PRIV1 ( -- x )  5 ;
>FORTH
T{ S" PRIV1" WL-INTERNALS WL-FIND NIP NIP 0<> -> <TRUE> }T
T{ S" PRIV1" WL-FORTH WL-FIND NIP NIP -> 0 }T
T{ PRIV1 CURRENT @ -> 5 WL-FORTH }T

\ ==== INTERNALS: numbers, memory, tools ====

T{ 1 0 DNEGATE -> -1 -1 }T
T{ 0 0 DNEGATE -> 0 0 }T
T{ 0 1 DNEGATE -> 0 -1 }T
T{ -1 -1 DNEGATE -> 1 0 }T
T{ -1 -1 DABS -> 1 0 }T
T{ 5 0 DABS -> 5 0 }T
T{ 0 -1 DABS -> 0 1 }T
T{ 0 DIGIT 9 DIGIT 10 DIGIT 35 DIGIT -> '0' '9' 'A' 'Z' }T
T{ HOLD-END PAD - -> 256 }T
T{ <# HLD @ -> HOLD-END }T
T{ HERE S" hi" S,  DUP C@  SWAP 1+ C@  HERE ALIGNED HERE = -> 2 'h' <TRUE> }T
T{ VP @ 3 DATA-ALLOT = -> <TRUE> }T
T{ VP @ 3 DATA-ALLOT DROP VP @ SWAP - -> 3 ALIGNED }T
T{ 1 NOOP -> 1 }T
T{ ' BRANCH BRANCHY?  ' ?BRANCH BRANCHY? -> <TRUE> <TRUE> }T
T{ ' (LOOP) BRANCHY?  ' (DO) BRANCHY? -> <TRUE> <TRUE> }T
T{ ' LIT BRANCHY?  ' DUP BRANCHY? -> <FALSE> <FALSE> }T
: HEX-WANT ( -- a u )  CELL 2 = IF S" 00FF1A" EXIT THEN  S" 000000FF1A" ;
T{ OUT{ 255 .HEX $1A .H2 }OUT HEX-WANT S= -> <TRUE> }T

\ A DATA-ALLOT (VALLOT without an extension) that fails must not use up
\ the RAM data region. After each test VP gets its old value back, so
\ that the next tests have room.
VARIABLE VP-SAVE
: T-VFULL ( -- a )  DATA-END DATA-ALLOT ;   \ more than the whole region
: T-VHUGE ( -- a )  MAX-UINT 1- DATA-ALLOT ;   \ VP plus this goes past the end of memory
VP @ VP-SAVE !
\ VALLOT checks before it changes VP: after THROW -8 VP is unchanged.
T{ ' T-VFULL CATCH  VP @ VP-SAVE @ = -> -8 <TRUE> }T
VP-SAVE @ VP !
\ VP must not wrap around past the end of memory.
T{ ' T-VHUGE CATCH -> -8 }T
VP-SAVE @ VP !

\ Anhang B: CATCH catches the THROW codes -16 and -19 of HEADER.
: T-NONAME ( -- )  S" " HEADER ;
T{ ' T-NONAME CATCH -> -16 }T

S" hayes" .RESULTS
