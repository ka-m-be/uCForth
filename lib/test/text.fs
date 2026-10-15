\ text.fs - tests of the package TEXT (lib/text.fs)
\
\ Load (in the folder ucf), after the package FILES and the tester:
\   ... host/files.fs forth/test/tester.fs lib/test/text.fs

ONLY FORTH DEFINITIONS  ALSO FILES
S" lib/text.fs" INCLUDED
ONLY FORTH DEFINITIONS  ALSO INTERNALS  ALSO TEXT
DECIMAL

T{ 1 2 3 4 2SWAP -> 3 4 1 2 }T
: T-STR ( -- a u )   S" hello world" ;
T{ T-STR 6 /STRING  S" world" S= -> <TRUE> }T
T{ T-STR S" hello" S= -> <FALSE> }T
: T-PAD ( -- a u )   S"   ab  " ;
T{ T-PAD -LEADING S" ab  " S= -> <TRUE> }T
T{ T-PAD -TRAILING S"   ab" S= -> <TRUE> }T
: T-SPLIT ( -- f1 f2 )   T-STR SPLIT S" hello" S= >R S" world" S= R> ;
T{ T-SPLIT -> <TRUE> <TRUE> }T
T{ T-STR 1/STRING S" ello world" S= -> <TRUE> }T
T{ T-STR BOUNDS - -> 11 }T
\ STR=: the same bytes, the case too; empty texts, also at address 0
T{ T-STR S" hello world" STR=  T-STR S" Hello world" STR=  T-STR S" hello" STR= -> <TRUE> <FALSE> <FALSE> }T
T{ 0 0 T-STR DROP 0 STR=  T-STR DROP 0 0 0 STR=  T-STR 1/STRING S" ello world" STR= -> <TRUE> <TRUE> <TRUE> }T
: T-SCAN ( c -- u )   T-STR ROT SCAN NIP ;
T{ 'w' T-SCAN  'h' T-SCAN  'x' T-SCAN  'd' T-SCAN -> 5 11 0 1 }T
T{ T-STR 'w' SCAN S" world" S=  0 0 'a' SCAN NIP -> <TRUE> 0 }T
: T-CTRL ( -- a u )   S"  x" OVER 9 SWAP C! ;   \ a tab and an x
T{ T-CTRL -LEADING NIP -> 1 }T

CREATE T-CS 40 ALLOT
T{ S" abc" T-CS PLACE  T-CS COUNT S" abc" S= -> <TRUE> }T
T{ S" de" T-CS +PLACE  T-CS COUNT S" abcde" S= -> <TRUE> }T
T{ 1234 N>S S" 1234" S= -> <TRUE> }T

: T-FAIL ( -- )   S" cannot open " S" x.fs" FAIL ;
T{ ' T-FAIL CATCH  ERRMSG @ ERRMSG CELL+ @ S" cannot open x.fs" S= -> -2 <TRUE> }T

S" text" .RESULTS
