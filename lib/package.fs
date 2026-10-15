\ package.fs - the frame of a package: PACKAGE PUBLIC PROTECTED PRIVATE END-PACKAGE
\
\ A package has a vocabulary name for the words it gives to its users,
\ and two parts for its other words (lib/README.md):
\
\   PACKAGE name    make the vocabulary name; the search order gets
\                   INTERNALS name and the private words; the next
\                   words are PRIVATE
\   PUBLIC          the next words go to name: for all users
\   PROTECTED       the next words go to name.I: for the packages and
\                   tests that ask with ALSO name.I; the first
\                   PROTECTED makes name.I
\   PRIVATE         the next words are for this package only
\   END-PACKAGE     the search order and CURRENT as before PACKAGE, then
\                   ALSO name
\
\ The variable TEMP-HEADS (in FORTH) decides what PRIVATE does (doc/vm.md,
\ section 6):
\
\   TEMP-HEADS OFF!  (at the start) PRIVATE does what PROTECTED does: its
\                    headers go to name.I, and tests reach them there
\   TEMP-HEADS ON!   the headers of PRIVATE go to the temp store at the
\                    top of the dictionary; END-PACKAGE gives it back.
\                    The code stays: the words of the package still call
\                    them, but nothing finds them by name any more. For
\                    the build of an image (apps/irbridge/app.fs)
\
\ After PACKAGE, ALSO adds the other packages that this one uses;
\ END-PACKAGE takes them away again. A package file can load another
\ package file: at most 4 packages inside each other, but not in the
\ PRIVATE part of one with TEMP-HEADS on. The idea is from SwiftForth
\ (PACKAGE PRIVATE PUBLIC END-PACKAGE), the three parts are those of
\ C++; the base has >FORTH and >INTERNALS for its own two wordlists.
\
\ Each package file needs these words, whatever the search order is (as
\ it needs VOCABULARY). So they go into FORTH; only the helpers are in
\ PACKAGES.I. host/files.fs loads this file.
\
\ Example:
\   PACKAGE CLOCK
\   VARIABLE TICKS                  \ PRIVATE
\   PROTECTED
\   VARIABLE ALARM                  \ in CLOCK.I, for ALSO CLOCK.I
\   PUBLIC
\   : TICK ( -- )  1 TICKS +! ;     \ in CLOCK
\   : TIME ( -- n )  TICKS @ ;
\   END-PACKAGE

\ The frame of this file is by hand: the words are not there yet.
VOCABULARY PACKAGES.I
ALSO INTERNALS  CURRENT @  ALSO PACKAGES.I DEFINITIONS
4 CONSTANT MAX-NEST
VARIABLE NEST
6 CONSTANT /FRAME   \ CURRENT, #ORDER, name, the PRIVATE wordlist, name.I (0: none yet), TEMP-START
MAX-NEST /FRAME * CELLS BUFFER: FRAMES
: FRAME ( -- a )   NEST @ 0= ABORT" not in a package"  NEST @ 1- /FRAME * CELLS FRAMES + ;
: PKG-WL ( -- wl )   FRAME 2 CELLS + @ ;
: INT-WL ( -- wl )   FRAME 3 CELLS + @ ;   \ the PRIVATE wordlist
: PROT-WL ( -- wl | 0 )   FRAME 4 CELLS + @ ;
: +ORDER ( wl -- )   ALSO TOP ! ;
: INSERT ( wl i -- )   \ wl into the search order at the place i; the places from i on move up
  #ORDER @ 16 < 0= IF -49 THROW THEN
  DUP CELLS CONTEXT +  SWAP #ORDER @ SWAP - CELLS      ( wl a u )
  >R  DUP DUP CELL+ R> MOVE  !  1 #ORDER +! ;
32 BUFFER: NAME2
: NEW-VOC ( a u -- wl )   \ a vocabulary with the name a u, as VOCABULARY makes it
  WORDLIST >R  HEADER  3 ,  ['] FORTH CELL+ @ ,  R@ ,
  LATEST @ R@ 2 CELLS + !  REVEAL  R> ;
: NAME.I ( -- a u )   \ the name of the package and .I
  PKG-WL 2 CELLS + @ CELL+ COUNT 31 AND  >R  NAME2 R@ MOVE  S" .I" NAME2 R@ + SWAP MOVE  NAME2 R> 2 + ;
: MAKE-I ( -- )   \ name.I, where PACKAGE made name; in the order below the PRIVATE words
  CURRENT @ >R  FRAME @ CURRENT !  NAME.I NEW-VOC  R> CURRENT !
  DUP FRAME 4 CELLS + !  FRAME CELL+ @ 2 + INSERT ;
: TEMP-WL ( -- wl )   \ a wordlist without a name, its record in the temp store
  TEMP-START @ 3 CELLS -  DUP HERE 32 + U< IF -8 THROW THEN  DUP TEMP-START !  WL-INIT ;
: UNLINK-WL ( wl -- )   \ take the record wl out of the chain of VOC-LINK
  VOC-LINK BEGIN  2DUP @ <>  WHILE  @ CELL+  REPEAT  SWAP CELL+ @ SWAP ! ;
: OWN-TEMP? ( a -- f )   TEMP-START @  FRAME 5 CELLS + @  WITHIN ;   \ in the temp store of this package
: END-TEMP ( -- )   \ give the temp store of this package back
  INT-WL UNLINK-WL
  VOC-LINK @ BEGIN ?DUP WHILE                       \ a wordlist named by a PRIVATE word loses the name
    DUP 2 CELLS + @ OWN-TEMP? IF  0 OVER 2 CELLS + !  THEN  CELL+ @
  REPEAT
  LATEST @ OWN-TEMP? IF  FRAME @ @ LATEST !  THEN   \ else HEADER takes it as unfinished
  FRAME 5 CELLS + @ TEMP-START ! ;

WL-FORTH CURRENT !
VARIABLE TEMP-HEADS   \ true: the headers of PRIVATE go to the temp store; false: to name.I
: PROTECTED ( -- )   PROT-WL 0= IF MAKE-I THEN  PROT-WL CURRENT ! ;
: PRIVATE ( -- )   INT-WL CURRENT ! ;
: PUBLIC ( -- )    PKG-WL CURRENT ! ;
: PACKAGE ( "name" -- )
  NEST @ MAX-NEST < 0= ABORT" packages nest too deep"
  CURRENT @ TEMP? ABORT" a package in the PRIVATE part of a package: PUBLIC first"
  PARSE-NAME  DUP 0= IF -16 THROW THEN
  DUP 29 > ABORT" the name of a package has at most 29 characters"
  1 NEST +!  CURRENT @ FRAME !  #ORDER @ FRAME CELL+ !  0 FRAME 4 CELLS + !  TEMP-START @ FRAME 5 CELLS + !
  NEW-VOC FRAME 2 CELLS + !
  WL-INTERNALS +ORDER  PKG-WL +ORDER
  TEMP-HEADS @ IF  TEMP-WL DUP +ORDER  ELSE  PROTECTED PROT-WL  THEN  FRAME 3 CELLS + !
  PRIVATE ;
: END-PACKAGE ( -- )
  INT-WL TEMP? IF END-TEMP THEN
  FRAME @ CURRENT !  FRAME CELL+ @ #ORDER !  PKG-WL  -1 NEST +!  +ORDER ;

PREVIOUS CURRENT !  PREVIOUS
