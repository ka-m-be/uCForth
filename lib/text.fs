\ text.fs - package TEXT: strings and messages
\
\ Small words for strings that the base vocabulary leaves out, and FAIL
\ for error messages with a name in them. Runs on every target.
\
\   2SWAP /STRING 1/STRING BOUNDS               strings and ranges
\   S= STR= SCAN -LEADING -TRAILING SPLIT       compare, find, cut
\   a u ca PLACE   a u ca +PLACE                counted strings (at most 255)
\   u N>S ( -- a u )                            a number as text (HOLD space)
\   a1 u1 a2 u2 FAIL                            THROW -2 with the text a1 u1 a2 u2

PACKAGE TEXT   \ lib/package.fs

PUBLIC
: 2SWAP   ( x1 x2 x3 x4 -- x3 x4 x1 x2 )  ROT >R ROT R> ;
: /STRING ( a u n -- a+n u-n )  ROT OVER + ROT ROT - ;
: 1/STRING ( a u -- a+1 u-1 )  1- SWAP 1+ SWAP ;
: BOUNDS  ( a u -- a+u a )   OVER + SWAP ;   \ for a loop over the bytes: a u BOUNDS ?DO  I C@ ...  LOOP
: S=      ( a1 u1 a2 u2 -- f )   \ the same text; the case counts as for names
  ROT OVER = IF SAME? ELSE 2DROP DROP FALSE THEN ;
: STR=    ( a1 u1 a2 u2 -- f )   \ the same bytes; the case counts always
  ROT OVER <> IF  DROP 2DROP FALSE EXIT  THEN
  BOUNDS ?DO  DUP C@ I C@ <> IF  DROP UNLOOP FALSE EXIT  THEN  1+  LOOP  DROP TRUE ;
: SCAN    ( a u c -- a' u' )   \ the rest of a u from the first c on; u' 0: no c
  >R  BEGIN  DUP IF OVER C@ R@ <> ELSE 0 THEN  WHILE  1- SWAP 1+ SWAP  REPEAT  R> DROP ;
: -LEADING  ( a u -- a' u' )  BEGIN DUP IF OVER C@ 33 < ELSE 0 THEN WHILE 1- SWAP 1+ SWAP REPEAT ;
: -TRAILING ( a u -- a u' )   BEGIN DUP IF 2DUP + 1- C@ 33 < ELSE 0 THEN WHILE 1- REPEAT ;
: SPLIT ( a u -- a2 u2 a1 u1 )   \ a1 u1: the first word; a2 u2: the rest
  -LEADING  2DUP  BEGIN DUP IF OVER C@ 32 > ELSE 0 THEN WHILE 1- SWAP 1+ SWAP REPEAT
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
