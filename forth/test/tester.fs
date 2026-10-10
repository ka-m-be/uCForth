\ tester.fs - a small Hayes-style tester for uCForth
\
\ Use:   T{ code -> expected results }T
\
\ T{ records the stack depth. -> saves the results of the code. }T
\ compares them with the expected results and counts the test. If they
\ are not the same, }T shows a message and the source line and counts
\ an error. S" name" .RESULTS shows "name: n tests, e errors", adds the
\ counters to TOTAL-TESTS and TOTAL-ERRORS, and sets them back to 0.
\
\ The idea and the messages come from the tester of John Hayes (1995).
\ This file is a new implementation. It also supplies the constants
\ that make tests independent of the cell size (after Hayes, core.fr).
\ A test file loads after this file, for example forth/test/hayes.fs.

ONLY FORTH DEFINITIONS  ALSO INTERNALS    \ TIB and #TIB give the source line
DECIMAL

VARIABLE #TESTS                  \ tests since the last .RESULTS
VARIABLE #ERRORS                 \ failed tests since the last .RESULTS
VARIABLE TOTAL-TESTS             \ all tests (tools/test.fs)
VARIABLE TOTAL-ERRORS            \ all failed tests
VARIABLE START-DEPTH             \ stack depth at T{
VARIABLE ACTUAL-DEPTH            \ number of results at ->
32 CELLS BUFFER: ACTUAL-RESULTS  \ the results at ->, the top first

: TEST-ERROR ( i*x a u -- )      \ show the message and the line, clean the stack
  1 #ERRORS +!  TYPE  TIB #TIB @ TYPE CR
  BEGIN DEPTH START-DEPTH @ > WHILE DROP REPEAT
  BEGIN DEPTH START-DEPTH @ < WHILE 0 REPEAT ;

: T{ ( -- )   DEPTH START-DEPTH ! ;

: -> ( i*x -- )                  \ save the results, at most 32
  DEPTH START-DEPTH @ -  DUP ACTUAL-DEPTH !
  0 MAX 0 ?DO  ACTUAL-RESULTS I CELLS + !  LOOP ;

: }T ( i*x -- )                  \ compare the expected results with the saved ones
  1 #TESTS +!
  DEPTH START-DEPTH @ -  ACTUAL-DEPTH @ <> IF
    S" WRONG NUMBER OF RESULTS: " TEST-ERROR EXIT
  THEN
  ACTUAL-DEPTH @ 0 MAX 0 ?DO
    ACTUAL-RESULTS I CELLS + @ <> IF
      S" INCORRECT RESULT: " TEST-ERROR UNLOOP EXIT
    THEN
  LOOP ;

: .RESULTS ( a u -- )            \ show the counters, add them to the totals, set them to 0
  TYPE ." : " #TESTS @ . ." tests, " #ERRORS @ . ." errors" CR
  #TESTS @ TOTAL-TESTS +!  #ERRORS @ TOTAL-ERRORS +!
  0 #TESTS !  0 #ERRORS ! ;

\ ==== Constants for tests with any cell size ====
0 INVERT        CONSTANT MAX-UINT     \ all bits set
MAX-UINT 1 RSHIFT CONSTANT MAX-INT    \ all bits except the most significant bit
MAX-INT INVERT  CONSTANT MIN-INT      \ only the most significant bit
MAX-INT         CONSTANT MID-UINT
MIN-INT         CONSTANT MID-UINT+1
MIN-INT         CONSTANT MSB
0               CONSTANT 0S
MAX-UINT        CONSTANT 1S
0S              CONSTANT <FALSE>
1S              CONSTANT <TRUE>

: COUNT-BITS ( -- n )   0  0 INVERT BEGIN DUP WHILE >R 1+ R> 2* REPEAT DROP ;
COUNT-BITS      CONSTANT #BITS        \ bits in a cell
