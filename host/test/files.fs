\ files.fs - tests of the package FILES (host/files.fs)
\
\ Load (in the folder ucf), after the package and the tester:
\   bash bootstrap/bash/ucf.sh host/files.fs forth/test/tester.fs host/test/files.fs
\ The tests write host/test/files1.tmp and host/test/files2.tmp.
\ The line "cannot open host/test/none.tmp" is part of the test.

ONLY FORTH DEFINITIONS  ALSO INTERNALS  ALSO FILES
DECIMAL

80 BUFFER: TF-BUF                \ for READ-LINE and READ-FILE
VARIABLE TF-FID
1 BUFFER: TF-LF   10 TF-LF C!    \ the line end

: TF-NAME1 ( -- a u )   S" host/test/files1.tmp" ;
: TF-NAME2 ( -- a u )   S" host/test/files2.tmp" ;
: TF-LINE ( a u -- ior )   \ write a u and a line end to TF-FID
  TF-FID @ WRITE-FILE  TF-LF 1 TF-FID @ WRITE-FILE  OR ;
: TF-SAME ( a1 u1 a2 u2 -- f )   \ true if both texts are the same
  ROT OVER <> IF 2DROP DROP FALSE EXIT THEN
  BEGIN DUP WHILE
    >R  OVER C@ OVER C@ <> IF R> DROP 2DROP FALSE EXIT THEN
    1+ SWAP 1+ SWAP  R> 1-
  REPEAT  DROP 2DROP TRUE ;

\ ==== Write a file ====
T{ TF-NAME1 W/O CREATE-FILE SWAP TF-FID ! -> 0 }T
T{ S" first line" TF-LINE -> 0 }T
T{ S" 3 4 +" TF-LINE -> 0 }T
T{ S" last" TF-FID @ WRITE-FILE -> 0 }T          \ the last line has no line end
T{ TF-FID @ CLOSE-FILE -> 0 }T

\ ==== READ-LINE ====
T{ TF-NAME1 R/O OPEN-FILE SWAP TF-FID ! -> 0 }T
T{ TF-BUF 80 TF-FID @ READ-LINE -> 10 TRUE 0 }T
T{ TF-BUF 10 S" first line" TF-SAME -> TRUE }T
T{ TF-BUF 3 TF-FID @ READ-LINE -> 3 TRUE 0 }T     \ a short buffer: the rest comes next
T{ TF-BUF 80 TF-FID @ READ-LINE -> 2 TRUE 0 }T
T{ TF-BUF 2 S"  +" TF-SAME -> TRUE }T
T{ TF-BUF 80 TF-FID @ READ-LINE -> 4 TRUE 0 }T
T{ TF-BUF 80 TF-FID @ READ-LINE -> 0 FALSE 0 }T   \ the end of the file
T{ TF-FID @ CLOSE-FILE -> 0 }T

\ ==== READ-FILE ====
T{ TF-NAME1 R/O OPEN-FILE SWAP TF-FID ! -> 0 }T
T{ TF-BUF 12 TF-FID @ READ-FILE -> 12 0 }T        \ "first line", LF, "3"
T{ TF-BUF 10 + C@  TF-BUF 11 + C@ -> 10 51 }T
T{ TF-BUF 80 TF-FID @ READ-FILE -> 9 0 }T         \ " 4 +", LF, "last"
T{ TF-BUF 80 TF-FID @ READ-FILE -> 0 0 }T         \ the end of the file
T{ TF-FID @ CLOSE-FILE -> 0 }T

\ ==== Errors ====
T{ S" host/test/none.tmp" R/O OPEN-FILE NIP -> -38 }T
T{ TF-FID @ CLOSE-FILE -> -37 }T                  \ closed already

\ ==== INCLUDE ====
: TF-FILE2 ( -- )   \ a file for INCLUDE: it defines TF-ADD and uses it
  TF-NAME2 W/O CREATE-FILE THROW TF-FID !
  S" : TF-ADD ( n -- n+10 )  10 + ;" TF-LINE THROW
  S" 5 TF-ADD" TF-LINE THROW
  TF-FID @ CLOSE-FILE THROW ;
TF-FILE2

T{ TF-NAME2 INCLUDED -> 15 }T
T{ 1 TF-ADD -> 11 }T
T{ INCLUDE host/test/files2.tmp  2 -> 15 2 }T      \ the rest of the line runs afterwards
T{ S" host/test/none.tmp" ' INCLUDED CATCH NIP NIP -> -38 }T

S" files" .RESULTS
