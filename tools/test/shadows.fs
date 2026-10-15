\ shadows.fs - the test of the tool tools/shadows.fs: a hidden name is reported, others are not
\
\ SHADOW-B uses SHADOW-A and has a private word PART of its own; A has a
\ public PART. The later words of B find the PART of A: the tool reports
\ it at the end of B. SHADOW-C uses A too, without such a name: no line.

ONLY FORTH DEFINITIONS  ALSO FILES
[DEFINED] T{ S" forth/test/tester.fs" ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] TEXT  S" lib/text.fs"  ?INCLUDED
ONLY FORTH DEFINITIONS  ALSO FILES
INCLUDE tools/shadows.fs
ONLY FORTH DEFINITIONS  ALSO INTERNALS  ALSO TEXT
CREATE OUTPUT 200 ALLOT   VARIABLE OUTPUT-N   VARIABLE OLD-EMIT   VARIABLE BEFORE
: CATCH-EMIT ( c -- )   OUTPUT-N @ 200 < IF  OUTPUT OUTPUT-N @ + C!  1 OUTPUT-N +!  ELSE DROP THEN ;
: CAPTURE ( -- )   #SHADOWS @ BEFORE !  0 OUTPUT-N !  'EMIT @ OLD-EMIT !  ['] CATCH-EMIT 'EMIT ! ;
: RELEASE ( -- )   OLD-EMIT @ 'EMIT ! ;
: STARTS? ( a u a2 u2 -- f )   ROT OVER < IF  2DROP DROP FALSE  ELSE  TUCK STR=  THEN ;
: LINE-B ( -- a u )   S"   SHADOW-B: PART is also in SHADOW-A" ;

PACKAGE SHADOW-A
PUBLIC  : PART ( -- n )   1 ;   : ONLY-A ( -- n )   2 ;
END-PACKAGE

PACKAGE SHADOW-B  ALSO SHADOW-A
: PART ( -- n )   10 ;
: USE ( -- n )   PART ;
PUBLIC  : B-USE ( -- n )   USE ;
CAPTURE END-PACKAGE RELEASE
T{ OUTPUT OUTPUT-N @ LINE-B STARTS?  #SHADOWS @ BEFORE @ - -> TRUE 1 }T
T{ B-USE -> 1 }T                          \ the PART of SHADOW-A, not the own one (10)

PACKAGE SHADOW-C  ALSO SHADOW-A
: OTHER ( -- n )   ONLY-A 1+ ;
PUBLIC  : C-USE ( -- n )   OTHER ;
CAPTURE END-PACKAGE RELEASE
T{ OUTPUT-N @  #SHADOWS @ BEFORE @ -  C-USE -> 0 0 3 }T

TEMP-HEADS ON!                            \ PROTECTED words are checked too
PACKAGE SHADOW-D  ALSO SHADOW-A
PROTECTED  : PART ( -- n )   30 ;
PUBLIC  : D-USE ( -- n )   PART ;
CAPTURE END-PACKAGE RELEASE
TEMP-HEADS OFF!
T{ OUTPUT OUTPUT-N @ S"   SHADOW-D: PART is also in SHADOW-A" STARTS?  #SHADOWS @ BEFORE @ -  D-USE -> TRUE 1 1 }T

S" shadows" .RESULTS
