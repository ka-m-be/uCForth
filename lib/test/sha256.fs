\ sha256.fs - tests of the package SHA256 (lib/sha256.fs)
\
\ Load (in the folder ucf, with 32-bit cells), after the package FILES
\ and the tester:
\   ... -32 host/files.fs forth/test/tester.fs lib/test/sha256.fs
\ The expected digests are from FIPS 180-4 (appendix B) and from
\ "shasum -a 256" (the text of 1000 bytes).

ONLY FORTH DEFINITIONS  ALSO FILES
S" lib/sha256.fs" INCLUDED
ONLY FORTH DEFINITIONS  ALSO INTERNALS  ALSO SHA256  ALSO SHA256.I
DECIMAL

: D@ ( a i -- x )   4 * + BE@ ;          \ word i of a digest
VARIABLE EXP
: DIGEST= ( a exp -- f )   \ the digest at a is the 8 cells at exp
  EXP !  TRUE  8 0 DO  OVER I D@  EXP @ I CELLS + @ =  AND  LOOP  NIP ;

: T-EMPTY ( -- a )   S" " SHA-DIGEST ;
: T-ABC   ( -- a )   S" abc" SHA-DIGEST ;
: T-TWO   ( -- a )   \ 56 bytes: the padding needs a second block
  S" abcdbcdecdefdefgefghfghighijhijkijkljklmklmnlmnomnopnopq" SHA-DIGEST ;

CREATE E-EMPTY $E3B0C442 , $98FC1C14 , $9AFBF4C8 , $996FB924 , $27AE41E4 , $649B934C , $A495991B , $7852B855 ,
CREATE E-ABC   $BA7816BF , $8F01CFEA , $414140DE , $5DAE2223 , $B00361A3 , $96177A9C , $B410FF61 , $F20015AD ,
CREATE E-TWO   $248D6A61 , $D20638B8 , $E5C02693 , $0C3E6039 , $A33CE459 , $64FF2167 , $F6ECEDD4 , $19DB06C1 ,
CREATE E-1000  $E5030BDD , $5C14F2CF , $A4476C02 , $1451DE0F , $DFE7710F , $5E8E990C , $D3332176 , $D1BACFD5 ,

T{ T-EMPTY E-EMPTY DIGEST= -> <TRUE> }T
T{ T-ABC   E-ABC   DIGEST= -> <TRUE> }T
T{ T-TWO   E-TWO   DIGEST= -> <TRUE> }T
T{ T-ABC   E-EMPTY DIGEST= -> <FALSE> }T

\ 1000 bytes (32 + i mod 90), added in 10 parts of 100 bytes
CREATE TEXT1000 1000 ALLOT
: FILL1000 ( -- )   1000 0 DO  I 90 MOD 32 +  TEXT1000 I + C!  LOOP ;
: T-1000 ( -- a )
  FILL1000  SHA-START  10 0 DO  TEXT1000 I 100 * + 100 SHA-ADD  LOOP  SHA-END ;
T{ T-1000 E-1000 DIGEST= -> <TRUE> }T

S" sha256" .RESULTS
