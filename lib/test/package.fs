\ package.fs - tests of PACKAGE PUBLIC PROTECTED PRIVATE END-PACKAGE (lib/package.fs)
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

\ ---- TEMP-HEADS on: PRIVATE keeps no headers
\ The headers of the PRIVATE words lie in the temp store while the
\ package loads; END-PACKAGE gives it back. The code stays.
TEMP-HEADS ON!
PACKAGE PK-TEMP
: PK-T1 ( -- n )  5 ;
: PK-T2 ( -- n )  PK-T1 1+ ;                          \ a PRIVATE word calls another one
: PK-LIT ( -- )  7 POSTPONE LITERAL ;  IMMEDIATE     \ IMMEDIATE on a temp header
: PK-SEVEN ( -- n )  PK-LIT ;
: PK-FACT ( n -- n! )  DUP 2 < IF DROP 1 EXIT THEN  DUP 1- RECURSE * ;
: PK-CONST ( n "name" -- )  CREATE ,  DOES> @ ;
3 PK-CONST PK-THREE                                   \ DOES> on a temp header
: PK-P ( -- )  POSTPONE PK-T1 ;  IMMEDIATE
: PK-PT ( -- n )  PK-P ;
T{ PK-T2 PK-SEVEN 5 PK-FACT PK-THREE PK-PT -> 6 7 120 3 5 }T
T{ S" PK-T1" FIND-NAME TEMP?  ' PK-T1 TEMP? -> <TRUE> <FALSE> }T   \ the header in the temp store, the code not
T{ DICT-END 1- TEMP?  DICT-END TEMP?  -1 TEMP?  0 TEMP? -> <TRUE> <FALSE> <FALSE> <FALSE> }T
\   the temp store ends at DICT-END: above it lie the chip addresses of XDATA (ESP32)
VARIABLE PK-HERE  VARIABLE PK-TS
ALIGN  HERE PK-HERE !  TEMP-START @ PK-TS !
S" PK-HALF" HEADER  0 ,  FORGET-UNFINISHED            \ a definition that stopped before ';'
T{ HERE PK-HERE @ =  TEMP-START @ PK-TS @ = -> <TRUE> <TRUE> }T   \ code and header given back
T{ ' MARKER CATCH  ' FREEZE CATCH  ' PACKAGE CATCH -> -2 -2 -2 }T  \ not in the PRIVATE part
PUBLIC
' PK-T1 CONSTANT PK-XT
: PK-USE3 ( -- n )  PK-T2 ;
END-PACKAGE
T{ PK-USE3  PK-XT EXECUTE -> 6 5 }T                   \ the code stays
T{ S" PK-T1" FIND-NAME  PK-XT >NAME  S" PK-TEMP.I" FIND-NAME -> 0 0 0 }T   \ no names, no PK-TEMP.I
T{ TEMP-START @ DICT-END =  LIMIT DICT-END = -> <TRUE> <TRUE> }T   \ the temp store is empty again
T{ #ORDER @ PK-ORDER @ 1+ =  CURRENT @ PK-CURRENT @ = -> <TRUE> <TRUE> }T
PREVIOUS

\ PROTECTED makes PK-PROT.I; in the order it lies below the PRIVATE words
PACKAGE PK-PROT
: PK-P1 ( -- n )  1 ;
PROTECTED
: PK-P2 ( -- n )  2 ;
: PK-SAME ( -- n )  10 ;
PRIVATE
: PK-SAME ( -- n )  20 ;
: PK-P4 ( -- n )  4 ;
T{ PK-SAME -> 20 }T
PUBLIC
: PK-P3 ( -- n )  PK-P1 PK-P2 + PK-P4 + ;
END-PACKAGE
T{ PK-P3  S" PK-P1" FIND-NAME  S" PK-P4" FIND-NAME -> 7 0 0 }T   \ PRIVATE: gone
T{ ALSO PK-PROT.I  PK-P2 PK-SAME  PREVIOUS -> 2 10 }T             \ PROTECTED: in PK-PROT.I
PREVIOUS

\ A package in the PUBLIC part of another one, both with temp headers
PACKAGE PK-OUT2
: PK-O1 ( -- n )  1 ;
PUBLIC
PACKAGE PK-IN2
: PK-I1 ( -- n )  2 ;
PUBLIC  : PK-I2 ( -- n )  PK-I1 ;
END-PACKAGE
: PK-O2 ( -- n )  PK-O1 PK-I2 + ;                      \ PK-O1 is still there
END-PACKAGE
T{ PK-O2  TEMP-START @ DICT-END = -> 3 <TRUE> }T
PREVIOUS
TEMP-HEADS OFF!

S" package" .RESULTS
