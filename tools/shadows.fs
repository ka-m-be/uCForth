\ shadows.fs - a tool: the words of a package that the package itself does not find
\
\ Inside a package, the packages of its PACKAGE line come first in the
\ search order, before its own words (lib/package.fs). A word of the
\ package with the name of a word of such a package is then found by the
\ later words of the package only as that other word. This tool puts a
\ check before END-PACKAGE: at the end of each package that loads after
\ it, a line for each such name, for example
\   IRBRIDGE: FRAME is also in IR-QUEUE
\ It changes nothing else. .SHADOWS gives the number of the lines so far.
\ On the host, before the files of the packages, in the folder ucf:
\   ALSO FILES  INCLUDE tools/shadows.fs  INCLUDE lib/text.fs ...
\ or before all tests:
\   ALSO FILES  INCLUDE tools/shadows.fs  INCLUDE tools/test.fs
\ Test: tools/test/shadows.fs.

ONLY FORTH DEFINITIONS  ALSO INTERNALS  ALSO PACKAGES.I
VARIABLE #SHADOWS                     \ the lines so far
: OWN-AT ( -- i )   \ the place of the PRIVATE words of the package in the search order; -1: none
  #ORDER @ 0 DO  I CELLS CONTEXT + @ INT-WL = IF I UNLOOP EXIT THEN  LOOP  -1 ;
: ABOVE ( a u i -- wl | 0 )   \ a wordlist above the place i in the search order with the name a u
  >R  #ORDER @ 1- BEGIN  DUP R@ >  WHILE
    >R  R@ CELLS CONTEXT + @ WL-FIND  ?DUP IF  NIP NIP DROP  R> CELLS CONTEXT + @  R> DROP EXIT  THEN
    R> 1-
  REPEAT  DROP 2DROP  R> DROP 0 ;
: .VOC ( wl -- )   2 CELLS + @ ?DUP IF .ID ELSE ." ?" THEN ;
: CHECK-WL ( wl i -- )   \ each word of the wordlist wl that a wordlist above the place i has too
  SWAP @ BEGIN ?DUP WHILE                              ( i nt )
    2DUP CELL+ COUNT 31 AND ROT ABOVE ?DUP IF
      ."   " PKG-WL .VOC ." : " OVER .ID ."  is also in " .VOC CR  1 #SHADOWS +!
    THEN  @
  REPEAT  DROP ;
: CHECK ( -- )   \ the PRIVATE, the PROTECTED (when it is another wordlist) and the PUBLIC words
  OWN-AT  DUP 0< IF DROP EXIT THEN
  INT-WL OVER CHECK-WL  PROT-WL DUP INT-WL <> AND ?DUP IF OVER CHECK-WL THEN  PKG-WL SWAP CHECK-WL ;
: .SHADOWS ( -- )   #SHADOWS @ . ;
: END-PACKAGE ( -- )   CHECK  END-PACKAGE ;   \ the check, then END-PACKAGE of lib/package.fs
ONLY FORTH DEFINITIONS
