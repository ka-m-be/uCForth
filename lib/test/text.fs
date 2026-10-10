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

CREATE T-CS 40 ALLOT
T{ S" abc" T-CS PLACE  T-CS COUNT S" abc" S= -> <TRUE> }T
T{ S" de" T-CS +PLACE  T-CS COUNT S" abcde" S= -> <TRUE> }T
T{ 1234 N>S S" 1234" S= -> <TRUE> }T

: T-FAIL ( -- )   S" cannot open " S" x.fs" FAIL ;
T{ ' T-FAIL CATCH  ERRMSG @ ERRMSG CELL+ @ S" cannot open x.fs" S= -> -2 <TRUE> }T

S" text" .RESULTS
