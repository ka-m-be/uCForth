\ package.fs - tests of PACKAGE PUBLIC PRIVATE END-PACKAGE (lib/package.fs)
\
\ Load (in the folder ucf), after the package FILES and the tester:
\   ... host/files.fs forth/test/tester.fs lib/test/package.fs

ONLY FORTH DEFINITIONS  ALSO FILES  ALSO INTERNALS
DECIMAL

VARIABLE PK-ORDER   VARIABLE PK-CURRENT
#ORDER @ PK-ORDER !  CURRENT @ PK-CURRENT !

PACKAGE PK-TEST
: PK-HELPER ( -- n )  7 ;
PUBLIC
: PK-USE ( -- n )  PK-HELPER 1+ ;
PRIVATE
: PK-HELPER2 ( -- n )  9 ;
PUBLIC
: PK-USE2 ( -- n )  PK-HELPER2 ;
END-PACKAGE

T{ PK-USE PK-USE2 -> 8 9 }T                 \ the exported words work
T{ S" PK-HELPER" FIND-NAME  S" PK-HELPER2" FIND-NAME -> 0 0 }T   \ the helpers are hidden
T{ #ORDER @ PK-ORDER @ 1+ =  CURRENT @ PK-CURRENT @ = -> <TRUE> <TRUE> }T   \ one more: PK-TEST
T{ ALSO PK-TEST.I  PK-HELPER  PREVIOUS -> 7 }T   \ a test reaches the helpers
PREVIOUS

\ Packages inside each other; a package that uses another one
PACKAGE PK-OUTER
PUBLIC : PK-O ( -- n )  1 ;
PACKAGE PK-INNER
PUBLIC : PK-I ( -- n )  2 ;
END-PACKAGE
: PK-OI ( -- n n )  PK-O PK-I ;             \ PK-INNER is in the order again
END-PACKAGE
T{ PK-OI -> 1 2 }T
T{ #ORDER @ PK-ORDER @ 1+ = -> <TRUE> }T    \ only PK-OUTER stays
PREVIOUS

T{ ' PUBLIC CATCH -> -2 }T                  \ not in a package
T{ ' END-PACKAGE CATCH -> -2 }T

S" package" .RESULTS
