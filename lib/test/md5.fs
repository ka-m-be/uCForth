\ md5.fs - tests of the package MD5 (lib/md5.fs)
\
\ Load (in the folder ucf, with 32-bit cells), after the package FILES
\ and the tester:
\   ... -32 host/files.fs forth/test/tester.fs lib/test/md5.fs
\ The expected digests are from RFC 1321 (appendix A.5) and from "md5"
\ (the text of 1000 bytes).

ONLY FORTH DEFINITIONS  ALSO FILES
S" lib/text.fs" INCLUDED  S" lib/md5.fs" INCLUDED
ONLY FORTH DEFINITIONS  ALSO INTERNALS  ALSO TEXT  ALSO MD5
DECIMAL

: DIGEST$ ( a u -- a' u' )   MD5-INIT MD5-UPDATE MD5-FINAL DROP MD5-HEX ;
: T-EMPTY ( -- f )   S" " DIGEST$  S" d41d8cd98f00b204e9800998ecf8427e" S= ;
: T-ABC   ( -- f )   S" abc" DIGEST$  S" 900150983cd24fb0d6963f7d28e17f72" S= ;
: T-MSG   ( -- f )   S" message digest" DIGEST$  S" f96b697d7cb7938d525a2f31aaf161d0" S= ;
T{ T-EMPTY T-ABC T-MSG -> <TRUE> <TRUE> <TRUE> }T

\ 1000 bytes (32 + i mod 90), added in 7 parts of different length
CREATE TEXT1000 1000 ALLOT
: FILL1000 ( -- )   1000 0 DO  I 90 MOD 32 +  TEXT1000 I + C!  LOOP ;
: T-1000 ( -- f )
  FILL1000  MD5-INIT
  TEXT1000 1 MD5-UPDATE  TEXT1000 1+ 63 MD5-UPDATE  TEXT1000 64 + 100 MD5-UPDATE
  TEXT1000 164 + 500 MD5-UPDATE  TEXT1000 664 + 300 MD5-UPDATE  TEXT1000 964 + 36 MD5-UPDATE
  MD5-FINAL DROP MD5-HEX  S" a29108fca0d583d8af1e167447deff90" S= ;
T{ T-1000 -> <TRUE> }T

S" md5" .RESULTS
