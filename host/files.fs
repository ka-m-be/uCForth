\ files.fs - package FILES: files on a host, and INCLUDE
\
\ A host (a bootstrap VM, later a kernel for an operating system)
\ supplies the HAL primitives 89-93, 98 and 99 (doc/vm.md, section 8).
\ This package gives them their headers and adds INCLUDE. It does not
\ change the base vocabulary: loading it puts FILES into the search order.
\
\ INCLUDE reads a file line by line into TIB and interprets each line.
\ ?INCLUDED loads a file only if a flag is false: [DEFINED] WORD S" file" ?INCLUDED.
\ It saves the line with INCLUDE: the rest of that line runs afterwards.
\ At most 4 files are open inside each other. An error in a file shows
\ the message, then the file name and the line number of each level.

VOCABULARY FILES  VOCABULARY FILES.I   \ lib/README.md: packages
ALSO INTERNALS  CURRENT @  ALSO FILES  ALSO FILES.I

ALSO FILES.I DEFINITIONS PREVIOUS

: PRIM ( n "name" -- )   \ a header for the primitive n
  PARSE-NAME HEADER ,  REVEAL ;

ALSO FILES DEFINITIONS PREVIOUS
89 PRIM OPEN-FILE    ( a u fam -- fid ior )
90 PRIM CREATE-FILE  ( a u fam -- fid ior )
91 PRIM CLOSE-FILE   ( fid -- ior )
92 PRIM READ-FILE    ( a u fid -- u2 ior )
93 PRIM WRITE-FILE   ( a u fid -- ior )
98 PRIM READ-LINE    ( a u fid -- u2 f ior )
97 PRIM ARG          ( n -- a u )   \ the argument n of the command line; u = 0: none
99 PRIM (BYE)        ( n -- )   \ end the program with the exit status n

0 CONSTANT R/O   1 CONSTANT W/O   2 CONSTANT R/W
: BIN ( fam -- fam ) ;   \ all files are binary

\ ==== INCLUDE ==========================================================

ALSO FILES.I DEFINITIONS PREVIOUS
4 CONSTANT MAX-NEST
VARIABLE NEST                       \ files that INCLUDE reads now
MAX-NEST 256 * BUFFER: SAVED-TIBS   \ for each level: the line with INCLUDE
MAX-NEST CELLS BUFFER: FIDS         \ for each level: the file
MAX-NEST CELLS BUFFER: LINE-NOS     \ for each level: the line number
MAX-NEST 64 * BUFFER: FILE-NAMES    \ for each level: the name, counted

: THIS ( a size -- a' )   \ the entry of the current level in a table
  NEST @ 1- * + ;
: FID ( -- fid )   FIDS CELL THIS @ ;

: (INCLUDE) ( -- )   \ interpret the lines of the file FID
  BEGIN  TIB 256 FID READ-LINE THROW  WHILE
    #TIB !  0 >IN !  1 LINE-NOS CELL THIS +!
    INTERPRET
  REPEAT  DROP ;

: .WHERE ( -- )   \ show the file and the line of the current level
  ."   in " FILE-NAMES 64 THIS COUNT TYPE
  ." , line " LINE-NOS CELL THIS @ 0 .R CR ;

: (INCLUDE-FILE) ( a u fid -- )   \ a u: the name of the file
  NEST @ MAX-NEST < 0= ABORT" too many files inside each other"
  1 NEST +!  FIDS CELL THIS !  0 LINE-NOS CELL THIS !
  63 MIN  DUP FILE-NAMES 64 THIS C!  FILE-NAMES 64 THIS 1+ SWAP MOVE
  TIB SAVED-TIBS 256 THIS 256 MOVE  #TIB @ >R  >IN @ >R
  ['] (INCLUDE) CATCH  ( code )
  ?DUP IF  DUP -1 <> IF DUP .ERROR THEN  .WHERE  DROP -1  ELSE 0 THEN
  FID CLOSE-FILE DROP
  SAVED-TIBS 256 THIS TIB 256 MOVE  R> >IN !  R> #TIB !
  -1 NEST +!  THROW ;   \ -1: the message is out already

ALSO FILES DEFINITIONS PREVIOUS
: INCLUDE-FILE ( fid -- )   \ interpret the file fid, then close it
  >R S" ?" R> (INCLUDE-FILE) ;

: INCLUDED ( a u -- )   \ interpret the file with the name a u
  2DUP R/O OPEN-FILE  ( a u fid ior )
  ?DUP IF  >R DROP  ."   cannot open " TYPE CR  R> THROW  THEN
  (INCLUDE-FILE) ;

: INCLUDE ( "name" -- )   \ interpret the file with this name
  PARSE-NAME INCLUDED ;
: ?INCLUDED ( f a u -- )   \ interpret the file a u, if f is false
  ROT IF 2DROP ELSE INCLUDED THEN ;

PREVIOUS PREVIOUS CURRENT !  PREVIOUS  ALSO FILES

\ The tools for packages (lib/package.fs) come with FILES, one time.
[DEFINED] PACKAGE  S" lib/package.fs" ?INCLUDED
