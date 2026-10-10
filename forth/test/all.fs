\ all.fs - the sum of all tests, for a target without files
\
\ host/send.fs sends the test files to a target line by line, and this
\ file at the end. With a failed test, the target shows an error.

: ALL-RESULTS ( -- )
  ." all: " TOTAL-TESTS @ . ." tests, " TOTAL-ERRORS @ . ." errors" CR
  TOTAL-ERRORS @ ABORT" some tests failed" ;
ALL-RESULTS
