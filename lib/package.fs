\ package.fs - the frame of a package: PACKAGE PUBLIC PRIVATE END-PACKAGE
\
\ A package has two vocabularies (lib/README.md): name for the words it
\ gives to its users, name.I for its helpers (I: internals). These words
\ make the frame around a package file:
\
\   PACKAGE name    make the vocabularies name and name.I; the
\                   search order gets INTERNALS name name.I; the
\                   next words go to name.I
\   PUBLIC          the next words go to name
\   PRIVATE         the next words go to name.I
\   END-PACKAGE     the search order and CURRENT as before PACKAGE, then
\                   ALSO name
\
\ After PACKAGE, ALSO adds the other packages that this one uses;
\ END-PACKAGE takes them away again. A package file can load another
\ package file: at most 4 packages inside each other. The idea is from
\ SwiftForth (PACKAGE PRIVATE PUBLIC END-PACKAGE); the base has >FORTH and
\ >INTERNALS for its own two wordlists.
\
\ Each package file needs these four words, whatever the search order
\ is (as it needs VOCABULARY). So they go into FORTH; only the helpers
\ are in PACKAGES.I. host/files.fs loads this file.
\
\ Example:
\   PACKAGE CLOCK
\   VARIABLE TICKS                  \ a helper, in CLOCK.I
\   PUBLIC
\   : TICK ( -- )  1 TICKS +! ;     \ in CLOCK
\   : TIME ( -- n )  TICKS @ ;
\   END-PACKAGE

\ The frame of this file is by hand: the words are not there yet.
VOCABULARY PACKAGES.I
ALSO INTERNALS  CURRENT @  ALSO PACKAGES.I DEFINITIONS
4 CONSTANT MAX-NEST
VARIABLE NEST
MAX-NEST 4 * CELLS BUFFER: FRAMES   \ each: CURRENT, #ORDER, the two wordlists
: FRAME ( -- a )   NEST @ 0= ABORT" not in a package"  NEST @ 1- 4 * CELLS FRAMES + ;
: PKG-WL ( -- wl )   FRAME 2 CELLS + @ ;
: INT-WL ( -- wl )   FRAME 3 CELLS + @ ;
: +ORDER ( wl -- )   ALSO TOP ! ;
32 BUFFER: NAME2
: NEW-VOC ( a u -- wl )   \ a vocabulary with the name a u, as VOCABULARY makes it
  WORDLIST >R  HEADER  3 ,  ['] FORTH CELL+ @ ,  R@ ,
  LATEST @ R@ 2 CELLS + !  REVEAL  R> ;

WL-FORTH CURRENT !
: PACKAGE ( "name" -- )
  NEST @ MAX-NEST < 0= ABORT" packages nest too deep"
  PARSE-NAME  DUP 0= IF -16 THROW THEN
  DUP 29 > ABORT" the name of a package has at most 29 characters"
  1 NEST +!  CURRENT @ FRAME !  #ORDER @ FRAME CELL+ !
  2DUP NEW-VOC FRAME 2 CELLS + !
  DUP >R  NAME2 SWAP MOVE  S" .I" NAME2 R@ + SWAP MOVE
  NAME2 R> 2 + NEW-VOC FRAME 3 CELLS + !
  WL-INTERNALS +ORDER  PKG-WL +ORDER  INT-WL +ORDER  INT-WL CURRENT ! ;
: PUBLIC ( -- )     PKG-WL CURRENT ! ;
: PRIVATE ( -- )   INT-WL CURRENT ! ;
: END-PACKAGE ( -- )
  FRAME @ CURRENT !  FRAME CELL+ @ #ORDER !  PKG-WL  -1 NEST +!  +ORDER ;

PREVIOUS CURRENT !  PREVIOUS
