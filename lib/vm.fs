\ vm.fs - package VM: the virtual machine of uCForth, in Forth
\
\ This package runs a second uCForth (the *target*) inside the running
\ one (the *host*). The target has its own memory, stacks and registers
\ and the primitives of doc/vm.md. Tools use it to build images:
\ tools/genesis.fs builds the reference images in seed/.
\
\ The target has the cell size of the host. Its memory is a window of
\ WINDOW bytes in the dictionary of the host: the target address ta is
\ at the host address MEM + ta. So a cell of the target is a cell of the
\ host, and T@ is only "MEM + @".
\
\ The registers IP, WR (the W register), SP and RP hold host addresses.
\ The words convert when a target address goes into or out of target
\ memory: T> makes a target address, "MEM +" a host address.
\
\ Primitives: 0-31 (mandatory), 80-88 (HAL, minimal) and 127 (HALT).
\ A tool adds more with OP, for example the host primitives of Genesis.
\ The console of the target is the vectors T-RX and T-TX.

PACKAGE VM   \ lib/package.fs

PUBLIC

\ ==== Memory ============================================================

$5000 CELL 4 = $6C00 AND 4 * +  CONSTANT WINDOW   \ 20 KB; 32 bit: 128 KB
ALIGN  HERE WINDOW ALLOT  CONSTANT MEM
MEM WINDOW 0 FILL

: T   ( ta -- a )   MEM + ;         \ target address -> host address
: T>  ( a -- ta )   MEM - ;         \ host address -> target address
: T@  ( ta -- x )   MEM + @ ;
: T!  ( x ta -- )   MEM + ! ;
: TC@ ( ta -- c )   MEM + C@ ;
: TC! ( c ta -- )   MEM + C! ;

WL-FORTH 16 -  CONSTANT HALT-XT     \ the code field 127 in SYS (doc/vm.md 3)
$E0            CONSTANT RUN-CELLS   \ two cells in the free area: xt, HALT-XT
127 HALT-XT T!  HALT-XT HALT-XT CELL+ T!   \ the code field HALT, a thread cell for it

\ ==== Registers and stacks ==============================================

VARIABLE IP   VARIABLE WR   VARIABLE SP   VARIABLE RP
VARIABLE T-THROW                    \ the xt of THROW in the target, or 0

: TPUSH ( x -- )   CELL NEGATE SP +!  SP @ ! ;
: TPOP  ( -- x )   SP @ @  CELL SP +! ;
: RPUSH ( x -- )   CELL NEGATE RP +!  RP @ ! ;
: RPOP  ( -- x )   RP @ @  CELL RP +! ;

\ ==== The table of primitives ==========================================

PRIVATE
CELL 4 = 1 AND 1+  CONSTANT SHIFT   \ op -> offset in OPS
CREATE OPS  128 CELLS ALLOT

PUBLIC
: OP ( xt n -- )   CELLS OPS + ! ;   \ the host word xt is primitive n

: TTHROW ( n -- )   \ THROW n in the target: the next word is its THROW
  TPUSH  T-THROW @ RUN-CELLS 2 CELLS + DUP >R T!
  HALT-XT R@ CELL+ T!  R> T IP ! ;

PRIVATE
: P-UNKNOWN ( -- )   -21 TTHROW ;
: P-HALT ( -- )   R> DROP ;          \ leave RUN (see there)

PUBLIC
: RUN ( -- )   \ NEXT until primitive 127 (HALT)
  BEGIN
    IP @ @ MEM + DUP WR !  CELL IP +!
    @ SHIFT LSHIFT OPS + @ EXECUTE   \ P-HALT drops the return into RUN
  AGAIN ;

: TRUN ( txt -- )   \ run the target word txt; return when it is complete
  RUN-CELLS T!  HALT-XT RUN-CELLS CELL+ T!  RUN-CELLS T IP !  RUN ;

PRIVATE

\ ==== Primitives 0-31 ===================================================

: P-DOCOL   IP @ T> RPUSH  WR @ CELL+ IP ! ;
: P-DOVAR   WR @ 2 CELLS + T> TPUSH ;
: P-DOCON   WR @ CELL+ @ TPUSH ;
: P-DODOES  WR @ 2 CELLS + T> TPUSH  IP @ T> RPUSH  WR @ CELL+ @ MEM + IP ! ;
: P-EXIT    RPOP MEM + IP ! ;
: P-LIT     IP @ @ TPUSH  CELL IP +! ;
: P-BRANCH  IP @ @ MEM + IP ! ;
: P-?BRANCH TPOP IF CELL IP +! ELSE IP @ @ MEM + IP ! THEN ;
: P-EXECUTE TPOP MEM + DUP WR !  @ SHIFT LSHIFT OPS + @ EXECUTE ;
: P-@       SP @ DUP @ MEM + @ SWAP ! ;
: P-!       TPOP MEM + TPOP SWAP ! ;
: P-C@      SP @ DUP @ MEM + C@ SWAP ! ;
: P-C!      TPOP MEM + TPOP SWAP C! ;
: P-DUP     SP @ @ TPUSH ;
: P-DROP    CELL SP +! ;
: P-SWAP    SP @ DUP >R @  R@ CELL+ @ R@ !  R> CELL+ ! ;
: P-OVER    SP @ CELL+ @ TPUSH ;
: P->R      TPOP RPUSH ;
: P-R>      RPOP TPUSH ;
: P-R@      RP @ @ TPUSH ;
: P-SP@     SP @ T> TPUSH ;
: P-SP!     SP @ @ MEM + SP ! ;
: P-RP@     RP @ T> TPUSH ;
: P-RP!     TPOP MEM + RP ! ;
: P-AND     TPOP SP @ DUP >R @ AND R> ! ;
: P-OR      TPOP SP @ DUP >R @ OR R> ! ;
: P-XOR     TPOP SP @ DUP >R @ XOR R> ! ;
: P-0<      SP @ DUP @ 0< SWAP ! ;
: P-UM+     SP @ DUP >R CELL+ @ R@ @ UM+ R@ ! R> CELL+ ! ;

PUBLIC
DEFER T-RX ( -- c true | false )    \ the console of the target
DEFER T-TX ( c -- )
' FALSE IS T-RX   ' DROP IS T-TX

PRIVATE
: P-?RX     T-RX IF TPUSH TRUE ELSE FALSE THEN TPUSH ;
: P-TX!     TPOP T-TX ;

\ ==== HAL 80-88 =========================================================
\ The target has no clock and no image store: MILLIS is 0, MS and US do
\ not wait, SAVE-IMAGE gives the ior -21. A tool can put its own words.

: P-MILLIS  0 TPUSH ;
: P-BIS!    TPOP MEM + TPOP OVER C@ OR SWAP C! ;
: P-BIC!    TPOP MEM + TPOP INVERT OVER C@ AND SWAP C! ;

: SET-OPS ( -- )   \ the primitives of this package
  128 0 DO ['] P-UNKNOWN I OP LOOP
  ['] P-DOCOL 0 OP    ['] P-DOVAR 1 OP    ['] P-DOCON 2 OP    ['] P-DODOES 3 OP
  ['] P-EXIT 4 OP     ['] P-LIT 5 OP      ['] P-BRANCH 6 OP   ['] P-?BRANCH 7 OP
  ['] P-EXECUTE 8 OP  ['] P-@ 9 OP        ['] P-! 10 OP       ['] P-C@ 11 OP
  ['] P-C! 12 OP      ['] P-DUP 13 OP     ['] P-DROP 14 OP    ['] P-SWAP 15 OP
  ['] P-OVER 16 OP    ['] P->R 17 OP      ['] P-R> 18 OP      ['] P-R@ 19 OP
  ['] P-SP@ 20 OP     ['] P-SP! 21 OP     ['] P-RP@ 22 OP     ['] P-RP! 23 OP
  ['] P-AND 24 OP     ['] P-OR 25 OP      ['] P-XOR 26 OP     ['] P-0< 27 OP
  ['] P-UM+ 28 OP     ['] P-?RX 29 OP     ['] P-TX! 30 OP     ['] NOOP 31 OP
  ['] P-MILLIS 80 OP  ['] P-DROP 81 OP    ['] P-DROP 82 OP    ['] P-BIS! 83 OP
  ['] P-BIC! 84 OP    ['] NOOP 86 OP      ['] P-HALT 127 OP ;
SET-OPS

: P-SAVE-IMAGE  -21 TPUSH ;   ' P-SAVE-IMAGE 85 OP

END-PACKAGE
