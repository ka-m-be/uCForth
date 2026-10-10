\ vm.fs - tests of the package VM (lib/vm.fs)
\
\ Load (in the folder ucf), after the package FILES and the tester:
\   ... host/files.fs forth/test/tester.fs lib/test/vm.fs
\ The tests put small words into the memory of the target by hand and
\ run them. The full test of the VM is tools/genesis.fs: it builds the
\ reference images with it.

ONLY FORTH DEFINITIONS  ALSO FILES
S" lib/vm.fs" INCLUDED
ONLY FORTH DEFINITIONS  ALSO INTERNALS  ALSO VM
DECIMAL

\ A table of xts for the primitives: the xt of primitive n is the cell
\ VT-PRIMS + n cells, and this cell holds n.
$3000 CONSTANT VT-PRIMS
: VT-SETUP ( -- )   128 0 DO  I VT-PRIMS I CELLS + T!  LOOP ;
: P ( n -- txt )   CELLS VT-PRIMS + ;
VT-SETUP

VARIABLE VT-HERE   \ the next free target address for a test word
: VT-WORD ( -- txt )   $3200 VT-HERE !  0 VT-HERE @ T!  CELL VT-HERE +!  $3200 ;
: C: ( x -- )   VT-HERE @ T!  CELL VT-HERE +! ;   \ a cell of the body
: VT-RESET ( -- )   S0 T SP !  R0 T RP ! ;
: VT-RUN ( txt -- )   VT-RESET TRUN ;
: VT-DEPTH ( -- n )   S0 T SP @ - CELL / ;

\ LIT 3 LIT 4 UM+ DROP EXIT
VT-WORD 5 P C: 3 C: 5 P C: 4 C: 28 P C: 14 P C: 4 P C:  CONSTANT VT-ADD
T{ VT-ADD VT-RUN VT-DEPTH TPOP -> 1 7 }T

\ UM+ with a carry: -1 1 UM+ gives 0 1
VT-WORD 5 P C: -1 C: 5 P C: 1 C: 28 P C: 4 P C:  CONSTANT VT-CARRY
T{ VT-CARRY VT-RUN TPOP TPOP -> 1 0 }T

\ ?BRANCH: 0 takes the branch, else the next cell
VARIABLE VT-TARGET
: VT-IF ( flag -- txt )   \ flag ?BRANCH to "LIT 2", else "LIT 1"
  VT-WORD SWAP 5 P C: C:  7 P C: VT-HERE @ 4 CELLS + C:
  5 P C: 1 C: 4 P C:  5 P C: 2 C: 4 P C: ;
T{ 0 VT-IF VT-RUN TPOP -> 2 }T
T{ -1 VT-IF VT-RUN TPOP -> 1 }T

\ C! and C@: the byte is the low byte of its cell (little endian)
VT-WORD 5 P C: 65 C: 5 P C: $3401 C: 12 P C: 5 P C: $3401 C: 11 P C: 4 P C:
CONSTANT VT-BYTE
T{ 0 $3400 T!  VT-BYTE VT-RUN TPOP $3400 T@ -> 65 65 256 * }T

\ A constant (code field 2) and EXECUTE
$3300 CONSTANT VT-CON   2 VT-CON T!  1234 VT-CON CELL+ T!
VT-WORD 5 P C: VT-CON C: 8 P C: 4 P C:  CONSTANT VT-EXEC
T{ VT-EXEC VT-RUN TPOP VT-DEPTH -> 1234 0 }T

\ SP@ with an empty stack gives S0
T{ 20 P VT-RUN TPOP -> S0 }T

\ >R R@ R>: back on the data stack
VT-WORD 5 P C: 9 C: 17 P C: 19 P C: 18 P C: 4 P C:  CONSTANT VT-RSTACK
T{ VT-RSTACK VT-RUN TPOP TPOP -> 9 9 }T

\ An unknown primitive: the VM runs THROW of the target with -21.
\ Here THROW is a word that only returns, so -21 stays on the stack.
$3380 CONSTANT VT-THROW   0 VT-THROW T!  4 P VT-THROW CELL+ T!
T{ VT-THROW T-THROW !  70 P VT-RUN TPOP -> -21 }T

S" vm" .RESULTS
