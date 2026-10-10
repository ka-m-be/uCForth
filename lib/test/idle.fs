\ idle.fs - tests of the package IDLE (lib/idle.fs)
\
\ Load (in the folder ucf), after the package FILES and the tester:
\   ... host/files.fs forth/test/tester.fs lib/test/idle.fs
\ At the end 'KEY? is as before: the sandbox of tools/test.fs takes the
\ words of IDLE away again.

ONLY FORTH DEFINITIONS  ALSO FILES  ALSO INTERNALS
S" lib/idle.fs" INCLUDED
ALSO IDLE  ALSO IDLE.I  DECIMAL

'KEY? @ CONSTANT KEY0                 \ the vector before the tests
VARIABLE RUNS-A   VARIABLE RUNS-B   VARIABLE SEEN
: ID-A ( -- )      1 RUNS-A +! ;
: ID-B ( -- )      1 RUNS-B +! ;
: ID-BAD ( -- )    1 RUNS-B +!  -77 THROW ;
: ID-BASE ( -- )   HEX ;
: ID-DEEP ( -- )   1 RUNS-A +!  IDLE-RUN ;       \ IDLE-RUN inside itself does nothing
: ID-SEE ( -- )    NO-BREAK @ SEEN ! ;
: SINK ( c -- )    DROP ;
: QUIET-RUN ( -- )   \ IDLE-RUN without the output of .STOPPED
  'EMIT @ >R  ['] SINK 'EMIT !  IDLE-RUN  R> 'EMIT ! ;

\ ---- the chain
T{ 0 RUNS-A !  IDLE-RUN  RUNS-A @ -> 0 }T                     \ an empty chain
T{ 'KEY? @ KEY0 = -> <TRUE> }T                             \ the vector stays until IDLE+
T{ ' ID-A IDLE+  IDLE-RUN IDLE-RUN  RUNS-A @ -> 2 }T
T{ 'KEY? @ ' IDLE-KEY? = -> <TRUE> }T
T{ ' ID-A IDLE+  0 RUNS-A !  IDLE-RUN  RUNS-A @ -> 1 }T        \ a word is in the chain once
T{ ' ID-B IDLE+  0 RUNS-A !  0 RUNS-B !  IDLE-RUN  RUNS-A @ RUNS-B @ -> 1 1 }T
T{ ' ID-A IDLE-  0 RUNS-A !  0 RUNS-B !  IDLE-RUN  RUNS-A @ RUNS-B @ -> 0 1 }T
T{ ' ID-B IDLE-  0 RUNS-B !  IDLE-RUN  RUNS-B @ -> 0 }T
T{ 0 IDLE-  ' ID-A IDLE- -> }T                             \ not in the chain: nothing

\ ---- an error takes the word out of the chain; the others go on
T{ ' ID-BAD IDLE+  ' ID-A IDLE+  0 RUNS-A !  0 RUNS-B !  QUIET-RUN QUIET-RUN
   RUNS-A @ RUNS-B @ -> 2 1 }T
T{ ' ID-A IDLE- -> }T

\ ---- BASE stays; IDLE-RUN inside itself does nothing
T{ ' ID-BASE IDLE+  IDLE-RUN  BASE @  ' ID-BASE IDLE- -> 10 }T
T{ ' ID-DEEP IDLE+  0 RUNS-A !  IDLE-RUN  RUNS-A @  ' ID-DEEP IDLE- -> 1 }T

\ ---- KEY? runs the chain once if no key waits
: NO-KEY ( -- c true | false )   FALSE ;
T{ ' NO-KEY OLD-KEY? !  ' ID-A IDLE+  0 RUNS-A !  KEY? KEY?  RUNS-A @  ' ID-A IDLE- -> <FALSE> <FALSE> 2 }T

\ ---- 8 words at most
: NOTHING ( "name" -- )   CREATE  DOES> DROP ;
NOTHING W1  NOTHING W2  NOTHING W3  NOTHING W4  NOTHING W5  NOTHING W6  NOTHING W7  NOTHING W8  NOTHING W9
: ALL+ ( -- )   ['] W1 IDLE+  ['] W2 IDLE+  ['] W3 IDLE+  ['] W4 IDLE+  ['] W5 IDLE+  ['] W6 IDLE+  ['] W7 IDLE+  ['] W8 IDLE+ ;
: ALL- ( -- )   ['] W1 IDLE-  ['] W2 IDLE-  ['] W3 IDLE-  ['] W4 IDLE-  ['] W5 IDLE-  ['] W6 IDLE-  ['] W7 IDLE-  ['] W8 IDLE- ;
T{ ALL+  ' W9 ' IDLE+ CATCH NIP  ALL- -> -2 }T
T{ ' W9 IDLE+  ' W9 IDLE- -> }T

\ ---- -BREAK and +BREAK count, never below 0; the chain runs inside them
T{ 0 NO-BREAK !  -BREAK -BREAK NO-BREAK @  +BREAK NO-BREAK @  +BREAK +BREAK NO-BREAK @ -> 2 1 0 }T
T{ ' ID-SEE IDLE+  0 SEEN !  IDLE-RUN  SEEN @  NO-BREAK @  ' ID-SEE IDLE- -> 1 0 }T

KEY0 'KEY? !                         \ KEY as before the tests
S" idle" .RESULTS
