\ text.fs - package TEXT: strings and messages
\
\ Small words for strings that the base vocabulary leaves out, and FAIL
\ for error messages with a name in them. Runs on every target.
\
\   2SWAP /STRING S= -LEADING -TRAILING SPLIT   strings
\   a u ca PLACE   a u ca +PLACE                counted strings (at most 255)
\   u N>S ( -- a u )                            a number as text (HOLD space)
\   a1 u1 a2 u2 FAIL                            THROW -2 with the text a1 u1 a2 u2

PACKAGE TEXT   \ lib/package.fs

PUBLIC
: 2SWAP   ( x1 x2 x3 x4 -- x3 x4 x1 x2 )  ROT >R ROT R> ;
: /STRING ( a u n -- a+n u-n )  ROT OVER + ROT ROT - ;
: S=      ( a1 u1 a2 u2 -- f )   \ the same text; the case counts as for names
  ROT OVER = IF SAME? ELSE 2DROP DROP FALSE THEN ;
: -LEADING  ( a u -- a' u' )  BEGIN DUP IF OVER C@ 33 < ELSE 0 THEN WHILE 1 /STRING REPEAT ;
: -TRAILING ( a u -- a u' )   BEGIN DUP IF 2DUP + 1- C@ 33 < ELSE 0 THEN WHILE 1- REPEAT ;
: SPLIT ( a u -- a2 u2 a1 u1 )   \ a1 u1: the first word; a2 u2: the rest
  -LEADING  2DUP  BEGIN DUP IF OVER C@ 32 > ELSE 0 THEN WHILE 1 /STRING REPEAT
  DUP >R 2SWAP R> -  2SWAP -LEADING 2SWAP ;

: PLACE  ( a u ca -- )   \ store a u as a counted string at ca (at most 255 characters)
  SWAP 255 MIN SWAP  2DUP C!  1+ SWAP MOVE ;
: +PLACE ( a u ca -- )   \ add a u to the counted string at ca
  >R  255 R@ C@ - MIN  R@ COUNT +  SWAP DUP R@ C@ + R> C!  MOVE ;
: N>S ( u -- a u )  0 <# #S #> ;   \ the number as text, in HOLD space

PRIVATE
256 BUFFER: MSG

PUBLIC
: FAIL ( a1 u1 a2 u2 -- )   \ THROW -2 with the message a1 u1 a2 u2, as ABORT" does
  2SWAP MSG PLACE  MSG +PLACE  TRUE MSG COUNT (ABORT") ;

END-PACKAGE
