\ pick.fs - package PICK: only the functions that an application needs
\
\ A quarry (blob/esp32, its README.md) holds libraries of C as packages
\ of CODE words: one package file for each object of a library, one CODE
\ word for each function of C (lib/esp32/clink.fs). An application needs
\ only a few of these functions. PICK reads package files of the quarry,
\ follows the references from the functions that the application calls,
\ and writes a copy of only the functions and data that they need.
\
\ A pick list tells PICK what to do (lib/esp32/esp32/phypick.fs):
\   PICK-FROM file tag out   read this package file; the functions that
\                            PICK takes from it go to the file out
\   PICK-DATA out            the data of all files go to the file out
\   PICK-OUTSIDE names       names outside the package files (the ROM,
\                            our own functions): the rest of the line
\   PICK-ENTRY names         the functions that the application calls
\   PICK-FOR name file       the copies are for the package name; file
\                            is the pick list (both for the heads)
\   PICK-LICENSE text        a line of the license for the heads
\   PICK-SHA256 file hex     the SHA-256 that the archive file must have
\   PICK-RUN                 read the files, check them, find the hull
\   PICK-WRITE               after PICK-RUN: write the copies
\   PICK-LIST                after PICK-RUN: the names in the hull
\ The tool needs cells of 32 bits. In the folder ucf:
\   build/ucf-c -32 host/files.fs lib/esp32/esp32/phypick.fs </dev/null
\
\ What PICK reads (blob/esp32/README.md, section 2): the objects of each
\ package file, each with its lines.
\   CODE name ... END-CODE   a function; the line after CODE is
\                            "$off n LITERALS" or "$off ORIGIN", the last
\                            one "$end L:  END-CODE"
\   n DATA: name             n bytes of data in RAM; the lines with INIT
\                            after it, a line "8 DATA-ALIGN" before it
\   CREATE name              data in the dictionary; the lines after it
\                            that start with a blank
\ PUBLIC, PRIVATE and EXPORTS (which is PUBLIC) give the visibility of
\ the next objects. A line "0 x SYMBOL! name" belongs to the data x. The
\ head of each package gives its numbers ("\ 76 functions (74 global),
\ 26 data objects."), and PICK checks them. Each other line is an error:
\ PICK stops and shows the file and the line. Names are compared with
\ the case, as in C.
\
\ The hull. An object needs the objects that it names: a function the
\ name after SYMBOL, SYMBOL+ and FUNC, and the first word of a line with
\ L!+ (a table of jumps); a line "0 x SYMBOL! name" makes the data x
\ need name. PICK looks for a name as CLINK does: first in the package
\ of the object, then in the PUBLIC names of all package files. Each
\ other name is outside; the hull may need only those of PICK-OUTSIDE.
\ The hull: the entries, the objects that they need, the objects that
\ these need, and so on.
\
\ The copies. Each line of an object of the hull goes to a copy as it
\ is, in the order of its package file: the lines of a function to the
\ file of its PICK-FROM, the lines of data to the file of PICK-DATA, a
\ line with SYMBOL! to the file of the functions. The frames of the
\ packages stay out (PACKAGE, PUBLIC, PRIVATE, EXPORTS, LINK-SYMBOLS,
\ END-PACKAGE); the file that loads the copies gives the frame. When two
\ objects of the hull have the same name (upper and lower case count as
\ the same, as in a search of Forth), each of them gets the tag of its
\ PICK-FROM, everywhere: tag:name. Each copy starts with a head: its
\ package files, the SHA-256 of their archives (lib/sha256.fs), the
\ lines of PICK-LICENSE and the new names. Each line ends with LF.
\
\ Before it writes, PICK-WRITE makes the SHA-256 of each archive and
\ stops if it is not the one of PICK-SHA256: then the archive is another
\ one, or the VM reads files only as text and loses bytes of a binary
\ file (the bootstrap VMs in awk and bash do so).

ONLY FORTH DEFINITIONS  ALSO FILES
[DEFINED] TEXT    S" lib/text.fs"    ?INCLUDED
[DEFINED] SHA256  S" lib/sha256.fs"  ?INCLUDED

ONLY FORTH DEFINITIONS
PACKAGE PICK  ALSO FILES  ALSO TEXT  ALSO SHA256   \ lib/package.fs

: NEED-32 ( -- )   CELL 4 <> ABORT" PICK needs cells of 32 bits: start the VM with -32" ;
NEED-32

\ ==== The pick list ====================================================
\ A source: the package file, the tag and the output file of PICK-FROM;
\ the name of the package and the wordlist of its names; 6 numbers as
\ read (functions, global functions, data objects; the bytes of code,
\ RAM data and dictionary data); the first 3 again, as its head says;
\ its object and archive, as line 1 of the file says.

64 CONSTANT MAX-SOURCES
: S-FILE ( s -- ca ) ;                \ the package file, a counted string
: S-TAG ( s -- ca )   64 + ;          \ the tag, at most 7 characters
: S-OUT ( s -- ca )   72 + ;          \ the file for its functions
: S-PACKAGE ( s -- ca )   136 + ;     \ the name of its package
: S-WL ( s -- a )   168 + ;           \ the wordlist of its names
: S-COUNTS ( s -- a )   172 + ;       \ 6 cells: the numbers as read
: S-HEAD ( s -- a )   196 + ;         \ 3 cells: the numbers of the head
: S-OBJECT ( s -- ca )   208 + ;      \ "x.o of lib.a (ESP32)", from line 1
272 CONSTANT /SOURCE
CREATE SOURCES  MAX-SOURCES /SOURCE * ALLOT
VARIABLE #SOURCES
: SRC ( i -- s )   /SOURCE * SOURCES + ;

: TEXT! ( ca n -- )   \ the next word of the input as a counted string at ca, at most n characters
  >R PARSE-NAME  DUP 0= ABORT" PICK: a word is missing"
  DUP R> > ABORT" PICK: a word is too long"  ROT PLACE ;

\ A list of names: the newest node, each node a link and a counted string.
VARIABLE ALLOWED-NAMES   \ the names of PICK-OUTSIDE
VARIABLE ENTRY-NAMES     \ the names of PICK-ENTRY
: NAMES, ( list -- )   \ the names on the rest of the line into the list
  >R  BEGIN PARSE-NAME DUP WHILE
    DUP 31 > ABORT" a name has at most 31 characters"
    R@ @  ALIGN HERE R@ !  ,  S,
  REPEAT  2DROP  R> DROP ;

CREATE DATA-OUT  64 ALLOT    \ the file for the data (PICK-DATA)
CREATE FOR-NAME  32 ALLOT    \ the package that the copies are for (PICK-FOR)
CREATE FOR-LIST  64 ALLOT    \ the pick list (PICK-FOR)
8 CONSTANT MAX-LICENSE
CREATE LICENSE  MAX-LICENSE 80 * ALLOT   \ the lines of PICK-LICENSE, counted strings
VARIABLE #LICENSE
VARIABLE EXPECTED   \ the archives of PICK-SHA256: the newest node (link, path, SHA-256 as text)

PUBLIC
: PICK-FROM ( "file" "tag" "out" -- )   \ read this package file; its functions go to out
  #SOURCES @ MAX-SOURCES < 0= ABORT" PICK-FROM: too many files"
  #SOURCES @ SRC  DUP /SOURCE 0 FILL
  DUP S-FILE 63 TEXT!  DUP S-TAG 7 TEXT!  S-OUT 63 TEXT!  1 #SOURCES +! ;
: PICK-DATA ( "file" -- )   DATA-OUT 63 TEXT! ;           \ the data go to this file
: PICK-OUTSIDE ( "names" -- )   ALLOWED-NAMES NAMES, ;   \ names outside the package files
: PICK-ENTRY ( "names" -- )   ENTRY-NAMES NAMES, ;       \ the functions that the application calls
: PICK-FOR ( "name" "file" -- )   \ the copies are for the package name; file: the pick list
  FOR-NAME 31 TEXT!  FOR-LIST 63 TEXT! ;
: PICK-LICENSE ( "text" -- )   \ the rest of the line: a line of the license for the heads
  #LICENSE @ MAX-LICENSE < 0= ABORT" PICK-LICENSE: too many lines"
  0 PARSE -LEADING -TRAILING  DUP 79 > ABORT" PICK-LICENSE: the line is too long"
  #LICENSE @ 80 * LICENSE + PLACE  1 #LICENSE +! ;
: PICK-SHA256 ( "file" "hex" -- )   \ the SHA-256 that the archive file must have: 64 hex digits
  ALIGN HERE  EXPECTED @ ,  EXPECTED !
  HERE 63 TEXT!  64 ALLOT
  HERE 64 TEXT!  HERE C@ 64 <> ABORT" PICK-SHA256: 64 hex digits expected"  65 ALLOT ;

\ ==== Lines and words ==================================================

PRIVATE
VARIABLE IN-SOURCE   \ the source that PICK reads now
VARIABLE IN-FID      \ its file
VARIABLE OPEN?       \ true while the file is open
VARIABLE LINE#       \ the number of the line in LINE
256 BUFFER: LINE     \ the line
VARIABLE #LINE       \ its length
VARIABLE REST-A  VARIABLE REST-U   \ the part of LINE that is not read yet

: .WHERE ( -- )   \ the file and the line that PICK reads now
  ."   in " IN-SOURCE @ S-FILE COUNT TYPE  ." , line " LINE# @ 0 .R CR ;
: STOP ( a1 u1 a2 u2 -- )   \ show the message a1 u1 a2 u2 and the place, then stop
  2SWAP CR TYPE TYPE CR .WHERE  -1 THROW ;

: OPEN ( s -- )   \ open the package file of the source s
  DUP IN-SOURCE !  0 LINE# !
  S-FILE COUNT R/O OPEN-FILE IF  DROP S" cannot open the file" S" " STOP  THEN
  IN-FID !  OPEN? ON! ;
: CLOSE ( -- )   OPEN? @ IF  IN-FID @ CLOSE-FILE DROP  OPEN? OFF!  THEN ;
: READ ( -- f )   \ the next line into LINE; false at the end of the file
  LINE 256 IN-FID @ READ-LINE THROW
  OVER 256 = IF  S" the line is too long" S" " STOP  THEN
  SWAP #LINE !  1 LINE# +!  LINE REST-A !  #LINE @ REST-U ! ;

: RAW ( -- a u )   \ the next word of LINE; u = 0 at the end
  REST-A @ REST-U @ SPLIT  2SWAP REST-U ! REST-A ! ;
: TOKEN ( -- a u )   \ the same, but a comment (\) ends the line
  RAW  2DUP S" \" S= IF  2DROP  0 REST-U !  LINE 0  THEN ;
: FIRST ( -- a u )   \ the first word of LINE, as TOKEN
  LINE REST-A !  #LINE @ REST-U !  TOKEN ;
: IS? ( a u a2 u2 -- a u f )   \ true if a u is the word a2 u2
  >R >R 2DUP R> R> S= ;
: HAS? ( a u -- f )   \ true if LINE has the word a u (not in a comment)
  >R >R  LINE REST-A !  #LINE @ REST-U !
  BEGIN TOKEN DUP WHILE
    R> R> 2DUP >R >R  S= IF  R> R> 2DROP TRUE EXIT  THEN
  REPEAT  2DROP  R> R> 2DROP FALSE ;
: NUMBER ( a u -- n )   \ the word a u as a number
  2DUP NUMBER? IF NIP NIP EXIT THEN  S" not a number: " 2SWAP STOP ;
: NAMED? ( a u -- )   \ stop if the word a u is empty
  NIP 0= IF  S" a name is missing" S" " STOP  THEN ;

CREATE TAIL  8 CELLS ALLOT   \ the last 4 words of LINE, each a u; the last word at the end
: LAST-WORDS ( -- )   \ the last 4 words of LINE into TAIL
  TAIL 8 CELLS 0 FILL  LINE REST-A !  #LINE @ REST-U !
  BEGIN TOKEN DUP WHILE
    6 0 DO  TAIL I 2 + CELLS + @  TAIL I CELLS + !  LOOP
    TAIL 7 CELLS + !  TAIL 6 CELLS + !
  REPEAT  2DROP ;
: TAIL$ ( i -- a u )   \ the word i from the end of LINE (0: the last one)
  3 SWAP - 2 * CELLS TAIL +  DUP @ SWAP CELL+ @ ;

\ ==== Objects ==========================================================
\ An object of a package (a function or data) has a record in the
\ dictionary and a header in the wordlist of its source: a constant with
\ the address of the record. A PUBLIC object has a second header in
\ PUBLICS. A name outside the package files has a record of the kind 0
\ and a header in OUTSIDES.

: R-SOURCE ( rec -- a ) ;               \ the source; 0 outside
: R-KIND ( rec -- a )   CELL+ ;         \ FUNCTION, RAM-DATA, DICT-DATA; 0 outside
: R-FLAGS ( rec -- a )   2 CELLS + ;    \ GLOBAL, KEPT, ALLOWED, RENAMED
: R-SIZE ( rec -- a )   3 CELLS + ;     \ bytes; outside: the literals of the hull with the name
: R-NT ( rec -- a )   4 CELLS + ;       \ the header in the wordlist of the source or in OUTSIDES
: R-LINES ( rec -- a )   5 CELLS + ;    \ the lines of the object in its file
: R-REFS ( rec -- a )   6 CELLS + ;     \ the newest reference: link, the object it needs
: R-NEXT ( rec -- a )   7 CELLS + ;     \ the next object to follow; outside: the first user
: R-OUT ( rec -- a )   8 CELLS + ;      \ the lines of the object in the copies
9 CELLS CONSTANT /RECORD
1 CONSTANT FUNCTION   2 CONSTANT RAM-DATA   3 CONSTANT DICT-DATA
1 CONSTANT GLOBAL     2 CONSTANT KEPT       4 CONSTANT ALLOWED     8 CONSTANT RENAMED

VARIABLE PUBLICS     \ the wordlist of the PUBLIC names of all sources
VARIABLE OUTSIDES    \ the wordlist of the names outside the package files
VARIABLE PUBLIC?     \ the visibility of the next object
VARIABLE IN-CODE     \ the function whose lines PICK reads; 0: none
VARIABLE IN-DATA     \ the data object whose lines PICK reads; 0: none
VARIABLE ORG         \ the offset where the CODE word starts (-4 for $0 n LITERALS)
VARIABLE ORG?        \ true when ORG is known
VARIABLE ALIGNED?    \ the line before was "8 DATA-ALIGN"

: NAME$ ( rec -- a u )   R-NT @ CELL+ COUNT 31 AND ;
: FLAG? ( rec f -- f' )   SWAP R-FLAGS @ AND 0<> ;
: FLAG+ ( rec f -- )   OVER R-FLAGS @ OR SWAP R-FLAGS ! ;
: FIND-IN ( a u wl -- rec | 0 )   \ the object with the name a u in the wordlist wl
  WL-FIND NIP NIP  DUP IF NAME>XT CELL+ @ THEN ;
: ADD-NAME ( a u wl rec -- )   \ a header a u in wl: a constant with the value rec
  CURRENT @ >R  SWAP CURRENT !  >R HEADER  2 , R> ,  REVEAL  R> CURRENT ! ;
: TALLY ( n i -- )   CELLS IN-SOURCE @ S-COUNTS + +! ;   \ add n to the number i
: RECORD ( kind -- rec )   \ a new record of the kind, for the source IN-SOURCE
  HERE /RECORD ALLOT  DUP /RECORD 0 FILL  TUCK R-KIND !  IN-SOURCE @ OVER R-SOURCE ! ;

: NEW-OBJECT ( a u kind -- rec )   \ an object with the name a u in the source IN-SOURCE
  >R  2DUP IN-SOURCE @ S-WL @ FIND-IN IF  S" the name is twice in the package: " 2SWAP STOP  THEN
  PUBLIC? @ IF  2DUP PUBLICS @ FIND-IN IF  S" the PUBLIC name is in two packages: " 2SWAP STOP  THEN  THEN
  R> RECORD  1 OVER R-LINES !  PUBLIC? @ IF  DUP GLOBAL FLAG+  THEN
  >R  2DUP IN-SOURCE @ S-WL @ R@ ADD-NAME  LATEST @ R@ R-NT !
  PUBLIC? @ IF  PUBLICS @ R@ ADD-NAME  ELSE  2DROP  THEN  R> ;
: OUTSIDE ( a u -- rec )   \ the name a u outside the package files; a new record if it is not there
  2DUP OUTSIDES @ FIND-IN ?DUP IF  NIP NIP EXIT  THEN
  0 RECORD  0 OVER R-SOURCE !  >R  OUTSIDES @ R@ ADD-NAME  LATEST @ R@ R-NT !  R> ;

\ ==== The objects of a package file ====================================

: NO-ALIGN ( -- )   \ stop if the line before was "8 DATA-ALIGN"
  ALIGNED? @ IF  S" no DATA: after 8 DATA-ALIGN" S" " STOP  THEN ;

: NEW-FUNCTION ( -- )   \ CODE name
  NO-ALIGN  TOKEN 2DUP NAMED?  FUNCTION NEW-OBJECT IN-CODE !  ORG? OFF!
  1 0 TALLY  PUBLIC? @ IF 1 1 TALLY THEN ;
: CODE-LINE ( -- )   \ a line of the function IN-CODE
  1 IN-CODE @ R-LINES +!
  FIRST  S" CODE" IS? IF  S" CODE inside a function" S" " STOP  THEN
  ORG? @ 0= IF   \ the line after CODE: $off n LITERALS or $off ORIGIN
    ORG? ON!
    NUMBER  TOKEN S" ORIGIN" IS? IF  2DROP ORG !  EXIT  THEN
    NUMBER DROP  TOKEN S" LITERALS" IS? NIP NIP 0= IF
      S" LITERALS or ORIGIN expected" S" " STOP  THEN
    4 - ORG !  EXIT                   \ the jump over the literals: 4 bytes
  THEN
  TOKEN S" L:" IS? NIP NIP IF         \ $end L:  END-CODE
    TOKEN S" END-CODE" IS? NIP NIP IF
      NUMBER ORG @ -  DUP IN-CODE @ R-SIZE !  3 TALLY  0 IN-CODE !  EXIT
    THEN
  THEN  2DROP ;

: NEW-DATA ( n -- )   \ n DATA: name
  TOKEN 2DUP NAMED?  RAM-DATA NEW-OBJECT
  ALIGNED? @ IF  1 OVER R-LINES +!  THEN   \ the line 8 DATA-ALIGN
  2DUP R-SIZE !  IN-DATA !  4 TALLY  1 2 TALLY  ALIGNED? OFF! ;
: NEW-TABLE ( -- )   \ CREATE name   \ .section, n bytes
  NO-ALIGN  TOKEN 2DUP NAMED?  DICT-DATA NEW-OBJECT
  RAW S" \" IS? NIP NIP  RAW 2DROP  RAW NUMBER  RAW S" bytes" IS? NIP NIP
  ROT AND 0= IF  S" CREATE name   \ .section, n bytes expected" S" " STOP  THEN
  2DUP SWAP R-SIZE !  5 TALLY  IN-DATA !  1 2 TALLY ;

: INIT? ( -- f )   \ the line ends with "name INIT" or "name $off + INIT"; name: IN-DATA
  LAST-WORDS  0 TAIL$ S" INIT" S= 0= IF FALSE EXIT THEN
  1 TAIL$ IN-DATA @ NAME$ S= IF TRUE EXIT THEN
  1 TAIL$ S" +" S=  3 TAIL$ IN-DATA @ NAME$ S=  AND ;
: DATA-LINE ( -- )   \ a line that starts with a blank: a line of the data IN-DATA
  NO-ALIGN
  IN-DATA @ 0= IF  S" a line with a blank, but no data before it" S" " STOP  THEN
  1 IN-DATA @ R-LINES +!
  IN-DATA @ R-KIND @ RAM-DATA = IF
    INIT? 0= IF  S" INIT for the data before expected" S" " STOP  THEN
  THEN ;

: POINTER ( a u -- )   \ the data a u: the rest of "n x SYMBOL! name" or "n x $off + SYMBOL! name"
  2DUP IN-SOURCE @ S-WL @ FIND-IN  DUP 0= IF  DROP S" SYMBOL!: no object of the package: " 2SWAP STOP  THEN
  DUP R-KIND @ FUNCTION = IF  DROP S" SYMBOL!: not data: " 2SWAP STOP  THEN
  1 SWAP R-LINES +!  2DROP
  TOKEN S" SYMBOL!" IS? IF  2DROP
  ELSE  NUMBER DROP  TOKEN S" +" S=  TOKEN S" SYMBOL!" S=  AND 0= IF
    S" n x $off + SYMBOL! name expected" S" " STOP  THEN
  THEN  TOKEN NAMED? ;
: NUMBER-LINE ( -- )   \ n DATA: name, 8 DATA-ALIGN, or a line with SYMBOL!
  FIRST NUMBER  TOKEN
  S" DATA:" IS? IF  2DROP NEW-DATA  EXIT  THEN
  S" DATA-ALIGN" IS? IF  2DROP DROP  NO-ALIGN  ALIGNED? ON!  EXIT  THEN
  NO-ALIGN  ROT DROP  POINTER ;

: OBJECT-LINE ( -- )   \ line 1, "\ x.fs - package P: x.o of lib.a (ESP32)": the text after ": "
  LINE #LINE @  BEGIN DUP 1 > WHILE
    OVER DUP C@ ':' =  SWAP 1+ C@ 32 =  AND IF
      2 /STRING -TRAILING  DUP 63 > IF  S" line 1: the object is too long" S" " STOP  THEN
      IN-SOURCE @ S-OBJECT PLACE  EXIT
    THEN  1 /STRING
  REPEAT  2DROP  S" line 1: no object after a colon" S" " STOP ;

VARIABLE HEAD-DATA?   \ the head gives the number of data objects on a later line
: HEAD? ( -- )   \ the numbers of the head, in one of these two forms:
  \ "\ 76 functions (74 global), 26 data objects."
  \ "\ 57 functions (56 global; 1 static ..." and later "\ section), 58 data objects."
  LINE REST-A !  #LINE @ REST-U !
  RAW S" \" S= 0= IF EXIT THEN
  RAW  RAW S" functions" IS? IF  2DROP
    NUMBER  IN-SOURCE @ S-HEAD !
    RAW 1 /STRING NUMBER  IN-SOURCE @ S-HEAD CELL+ !       \ (74
    RAW S" global)," IS? NIP NIP IF  RAW NUMBER  IN-SOURCE @ S-HEAD 2 CELLS + !
    ELSE  HEAD-DATA? ON!  THEN  EXIT
  THEN
  2SWAP S" section)," S=  HEAD-DATA? @ AND IF
    NUMBER  IN-SOURCE @ S-HEAD 2 CELLS + !  HEAD-DATA? OFF!
  ELSE  2DROP  THEN ;

: TOP-LINE ( -- )   \ a line outside a function
  #LINE @ IF  LINE C@ 32 = IF  DATA-LINE EXIT  THEN  THEN
  0 IN-DATA !  FIRST DUP 0= IF  2DROP NO-ALIGN EXIT  THEN    \ empty, or a comment
  S" CODE" IS? IF  2DROP NEW-FUNCTION  EXIT  THEN
  S" CREATE" IS? IF  2DROP NEW-TABLE  EXIT  THEN
  NUMBER? IF  DROP NUMBER-LINE  EXIT  THEN
  FIRST NO-ALIGN
  S" PUBLIC" IS? >R  S" EXPORTS" IS? R> OR IF  2DROP PUBLIC? ON!  EXIT  THEN
  S" PRIVATE" IS? IF  2DROP PUBLIC? OFF!  EXIT  THEN
  S" PACKAGE" IS? IF  2DROP TOKEN 2DUP NAMED?
    DUP 29 > IF  S" the name of a package has at most 29 characters: " 2SWAP STOP  THEN
    IN-SOURCE @ S-PACKAGE PLACE  EXIT  THEN
  S" LINK-SYMBOLS" IS? >R  S" END-PACKAGE" IS? R> OR IF  2DROP EXIT  THEN
  S" a line that PICK does not know: " 2SWAP STOP ;

: READ-SOURCE ( s -- )   \ read the objects of the package file of s
  OPEN  PUBLIC? OFF!  0 IN-CODE !  0 IN-DATA !  ALIGNED? OFF!  HEAD-DATA? OFF!
  BEGIN READ WHILE
    LINE# @ 1 = IF  OBJECT-LINE  THEN
    IN-CODE @ IF  CODE-LINE  ELSE
      IN-SOURCE @ S-PACKAGE C@ 0= IF HEAD? THEN  TOP-LINE
    THEN
  REPEAT
  IN-CODE @ IF  S" the file ends inside a function" S" " STOP  THEN
  NO-ALIGN  CLOSE ;

: CHECK-HEAD ( -- )   \ the numbers of IN-SOURCE as read are those of its head
  3 0 DO
    IN-SOURCE @ S-COUNTS I CELLS + @  IN-SOURCE @ S-HEAD I CELLS + @  <> IF
      S" the numbers are not those of the head" S" " STOP  THEN
  LOOP ;

\ ==== The references ===================================================
\ A second pass over each package file: the references of its objects.
\ The first pass has checked the lines already.

: RESOLVE ( a u -- rec )   \ the object with the name a u, seen from the package IN-SOURCE
  2DUP IN-SOURCE @ S-WL @ FIND-IN ?DUP IF  NIP NIP EXIT  THEN
  2DUP PUBLICS @ FIND-IN ?DUP IF  NIP NIP EXIT  THEN
  OUTSIDE ;
: OWN-FUNCTION ( a u -- rec )   \ the function a u of the package IN-SOURCE, for FUNC
  2DUP IN-SOURCE @ S-WL @ FIND-IN  DUP 0= IF  DROP S" FUNC: no function of the package: " 2SWAP STOP  THEN
  DUP R-KIND @ FUNCTION <> IF  DROP S" FUNC: not a function: " 2SWAP STOP  THEN  NIP NIP ;
: REFER ( from to -- )   \ the object from needs the object to
  HERE >R  OVER R-REFS @ ,  ,  R> SWAP R-REFS ! ;

: CODE-REFS ( -- )   \ the references in a line of the function IN-CODE
  S" L!+" HAS? IF   \ a table of jumps: its name is the first word
    FIRST 2DUP NUMBER? IF  DROP 2DROP  ELSE  RESOLVE IN-CODE @ SWAP REFER  THEN
  THEN
  LINE REST-A !  #LINE @ REST-U !
  BEGIN TOKEN DUP WHILE
    S" SYMBOL" IS? >R  S" SYMBOL+" IS? R> OR IF
      2DROP TOKEN 2DUP NAMED?  RESOLVE IN-CODE @ SWAP REFER
    ELSE  S" FUNC" IS? IF
      2DROP TOKEN 2DUP NAMED?  OWN-FUNCTION IN-CODE @ SWAP REFER
    ELSE  S" END-CODE" IS? IF  0 IN-CODE !  THEN  2DROP
    THEN THEN
  REPEAT  2DROP ;
: POINTER-REFS ( -- )   \ "n x SYMBOL! name" or "n x $off + SYMBOL! name": the data x needs name
  LAST-WORDS  FIRST 2DROP  TOKEN IN-SOURCE @ S-WL @ FIND-IN  0 TAIL$ RESOLVE REFER ;

: READ-REFS ( s -- )   \ the references of the objects of the package file of s
  OPEN  0 IN-CODE !
  BEGIN READ WHILE
    IN-CODE @ IF  CODE-REFS  ELSE
      #LINE @ IF  LINE C@ 32 <>  ELSE  FALSE  THEN IF   \ not a line of data
        FIRST S" CODE" IS? IF  2DROP TOKEN IN-SOURCE @ S-WL @ FIND-IN IN-CODE !
        ELSE  DUP IF
            NUMBER? IF  DROP  S" SYMBOL!" HAS? IF  POINTER-REFS  THEN  THEN
          ELSE  2DROP  THEN
        THEN
      THEN
    THEN
  REPEAT  CLOSE ;

\ ==== The hull =========================================================

VARIABLE TODO   \ the objects of the hull whose references PICK has not followed yet
: KEEP ( rec -- )   \ take the object into the hull, if it is not there yet
  DUP KEPT FLAG? IF  DROP EXIT  THEN
  DUP KEPT FLAG+  TODO @ OVER R-NEXT !  TODO ! ;
: USE-OUTSIDE ( from rec -- )   \ the object from needs the name rec outside: count it
  DUP R-SIZE @ 0= IF  TUCK R-NEXT !  ELSE  NIP  THEN  1 SWAP R-SIZE +! ;
: FOLLOW ( rec -- )   \ the objects that rec needs go into the hull
  DUP R-REFS @  BEGIN ?DUP WHILE
    2DUP CELL+ @  DUP R-KIND @ IF  NIP KEEP  ELSE  USE-OUTSIDE  THEN  @
  REPEAT  DROP ;
: HULL ( -- )   \ follow the references until no object is left
  BEGIN TODO @ ?DUP WHILE  DUP R-NEXT @ TODO !  FOLLOW  REPEAT ;

VARIABLE #ENTRIES
: TAKE-ENTRIES ( -- )   \ the functions of PICK-ENTRY go into the hull
  0 #ENTRIES !  ENTRY-NAMES @  BEGIN ?DUP WHILE
    DUP CELL+ COUNT  2DUP PUBLICS @ FIND-IN
    DUP 0= IF  DROP S" PICK-ENTRY: no PUBLIC function " 2SWAP FAIL  THEN
    DUP R-KIND @ FUNCTION <> IF  DROP S" PICK-ENTRY: not a function: " 2SWAP FAIL  THEN
    NIP NIP KEEP  1 #ENTRIES +!  @
  REPEAT ;
: ALLOW ( -- )   \ the names of PICK-OUTSIDE: records outside, allowed
  ALLOWED-NAMES @  BEGIN ?DUP WHILE  DUP CELL+ COUNT OUTSIDE ALLOWED FLAG+  @  REPEAT ;

: .USER ( rec -- )   \ the first object of the hull that needs the name rec outside
  R-NEXT @  DUP NAME$ TYPE  ."  of " R-SOURCE @ S-PACKAGE COUNT TYPE ;
VARIABLE #BAD
: CHECK-OUTSIDE ( -- )   \ the hull needs only the names outside that PICK-OUTSIDE gives
  0 #BAD !  OUTSIDES @ @  BEGIN ?DUP WHILE
    DUP NAME>XT CELL+ @  DUP R-SIZE @ 0<>  OVER ALLOWED FLAG? 0=  AND IF
      CR ." not in PICK-OUTSIDE: " DUP NAME$ TYPE  ."  (for " DUP .USER ." )"  1 #BAD +!
    THEN  DROP  @
  REPEAT
  #BAD @ IF  CR S" names outside the package files that are not in PICK-OUTSIDE: " #BAD @ N>S FAIL  THEN ;

VARIABLE ACTION   \ the xt for EACH-KEPT
: EACH-KEPT ( xt -- )   \ run xt ( rec -- ) for each object of the hull
  ACTION !  #SOURCES @ 0 ?DO
    I SRC S-WL @ @  BEGIN ?DUP WHILE
      DUP NAME>XT CELL+ @  DUP KEPT FLAG? IF  ACTION @ EXECUTE  ELSE  DROP  THEN  @
    REPEAT
  LOOP ;

\ ==== Report ===========================================================

CREATE SUMS  6 CELLS ALLOT     \ 6 numbers of one source
CREATE TOTALS  6 CELLS ALLOT   \ the same for all sources
: .TEXT ( a u n -- )   OVER - >R TYPE R> 0 MAX SPACES ;   \ a u in n columns
: .COL ( a u -- )   7 OVER - 0 MAX SPACES TYPE ;         \ a title in 7 columns, to the right
: .ROW ( a u a6 -- )   \ a line of a table: the name a u, the 6 numbers at a6
  ROT ROT 22 .TEXT  6 0 DO  DUP I CELLS + @ 7 .R  LOOP  DROP CR ;
: ADD6 ( a6 b6 -- )   \ add the 6 numbers at a6 to those at b6
  6 0 DO  OVER I CELLS + @  OVER I CELLS + +!  LOOP  2DROP ;

: REPORT ( -- )   \ the table of the objects that PICK read
  CR S" package" 22 .TEXT  S" funcs" .COL  S" global" .COL  S" data" .COL
  S" code" .COL  S" RAM" .COL  S" dict" .COL  CR
  TOTALS 6 CELLS 0 FILL
  #SOURCES @ 0 ?DO  I SRC DUP S-PACKAGE COUNT ROT S-COUNTS  DUP TOTALS ADD6  .ROW  LOOP
  S" all" TOTALS .ROW ;

: SUM+ ( n i -- )   CELLS SUMS + +! ;
: OBJECT-SUMS ( rec -- )   \ add the object rec of the hull to SUMS
  DUP R-KIND @ FUNCTION = IF  1 0 SUM+  DUP R-SIZE @ 2 SUM+
  ELSE  1 1 SUM+  DUP R-SIZE @  OVER R-KIND @ RAM-DATA = IF 3 ELSE 4 THEN SUM+  THEN
  R-LINES @ 5 SUM+ ;
: HULL-SUMS ( s -- )   \ the numbers of the hull in the source s into SUMS
  SUMS 6 CELLS 0 FILL
  S-WL @ @  BEGIN ?DUP WHILE
    DUP NAME>XT CELL+ @  DUP KEPT FLAG? IF  OBJECT-SUMS  ELSE  DROP  THEN  @
  REPEAT ;
: .OUTSIDE ( -- )   \ the names outside that the hull needs, then those it does not need
  OUTSIDES @ @  BEGIN ?DUP WHILE
    DUP NAME>XT CELL+ @  DUP R-SIZE @ IF
      ."   " DUP NAME$ TYPE  ."  (" DUP R-SIZE @ 0 .R ." )" CR  THEN  DROP  @
  REPEAT
  OUTSIDES @ @  BEGIN ?DUP WHILE
    DUP NAME>XT CELL+ @  DUP R-SIZE @ 0=  OVER ALLOWED FLAG?  AND IF
      ." in PICK-OUTSIDE, but not needed: " DUP NAME$ TYPE CR  THEN  DROP  @
  REPEAT ;
: HULL-REPORT ( -- )   \ the table of the hull
  CR ." the hull of " #ENTRIES @ 0 .R ."  entries:" CR
  S" package" 22 .TEXT  S" funcs" .COL  S" data" .COL  S" code" .COL
  S" RAM" .COL  S" dict" .COL  S" lines" .COL  CR
  TOTALS 6 CELLS 0 FILL
  #SOURCES @ 0 ?DO  I SRC DUP HULL-SUMS  S-PACKAGE COUNT SUMS  SUMS TOTALS ADD6  .ROW  LOOP
  S" all" TOTALS .ROW
  ." names outside, with the number of literals:" CR  .OUTSIDE ;

\ ==== The new names ====================================================
\ Two objects of the hull with the same name would be one name in the
\ package of the copies. Each of them gets the tag of its source.

VARIABLE KEPT-NAMES   \ the names of the hull and the new names
VARIABLE CLASHES      \ the old names of the renamed objects
VARIABLE #RENAMED
64 BUFFER: NEWBUF

: SAME-NAME ( rec -- )   \ rec and an object of the hull before it with the same name: both renamed
  DUP NAME$ KEPT-NAMES @ FIND-IN ?DUP IF  RENAMED FLAG+  RENAMED FLAG+  EXIT  THEN
  >R  R@ NAME$ KEPT-NAMES @ R> ADD-NAME ;
: NEW-NAME ( rec -- a u )   \ the name of rec in the copies (in NEWBUF if it is a new one)
  DUP RENAMED FLAG? 0= IF  NAME$ EXIT  THEN
  DUP R-SOURCE @ S-TAG COUNT NEWBUF PLACE  S" :" NEWBUF +PLACE  NAME$ NEWBUF +PLACE
  NEWBUF COUNT ;
: NEW-NAME? ( rec -- )   \ check the new name of a renamed object
  DUP RENAMED FLAG? 0= IF  DROP EXIT  THEN
  1 #RENAMED +!  DUP NEW-NAME
  DUP 31 > IF  S" the new name is too long: " 2SWAP FAIL  THEN
  2DUP KEPT-NAMES @ FIND-IN IF  S" the new name is there already: " 2SWAP FAIL  THEN
  ROT KEPT-NAMES @ SWAP ADD-NAME ;
: OLD-NAME ( rec -- )   \ remember the old name of a renamed object
  DUP RENAMED FLAG? 0= IF  DROP EXIT  THEN
  DUP NAME$ CLASHES @ FIND-IN IF  DROP EXIT  THEN
  >R  R@ NAME$ CLASHES @ R> ADD-NAME ;
: NO-OUT ( rec -- )   0 SWAP R-OUT ! ;
: RENAMES ( -- )   \ the objects of the hull with the same name get new names
  WORDLIST KEPT-NAMES !  WORDLIST CLASHES !  0 #RENAMED !
  CASE-SENSE OFF!  ['] SAME-NAME EACH-KEPT  ['] NEW-NAME? EACH-KEPT
  CASE-SENSE ON!  ['] OLD-NAME EACH-KEPT  ['] NO-OUT EACH-KEPT ;

\ ==== Writing ==========================================================

VARIABLE OUT-FID     \ the copy that PICK writes
VARIABLE OUT-OPEN?   \ true while it is open
VARIABLE #OUT-LINES  \ the lines written to it
256 BUFFER: OUTBUF   \ the next line of the copy
VARIABLE #OUT        \ its length
VARIABLE COPIED      \ LINE is in OUTBUF up to this address

: +OUT ( a u -- )   \ add a u to the line in OUTBUF
  DUP #OUT @ + 255 > IF  S" a line of the copy is too long" S" " STOP  THEN
  TUCK OUTBUF #OUT @ + SWAP MOVE  #OUT +! ;
: OUT-LINE ( -- )   \ write the line in OUTBUF and LF; start a new line
  10 OUTBUF #OUT @ + C!  OUTBUF #OUT @ 1+ OUT-FID @ WRITE-FILE THROW
  0 #OUT !  1 #OUT-LINES +! ;
: OUT-TEXT ( a u -- )   +OUT OUT-LINE ;
: OUT-OPEN ( a u -- )   \ make the file a u for a copy
  2DUP W/O CREATE-FILE IF  DROP S" cannot make the file " 2SWAP FAIL  THEN
  NIP NIP OUT-FID !  OUT-OPEN? ON!  0 #OUT-LINES !  0 #OUT ! ;
: OUT-CLOSE ( -- )   OUT-OPEN? @ IF  OUT-FID @ CLOSE-FILE DROP  OUT-OPEN? OFF!  THEN ;

: UP-TO ( a -- )   \ the text of LINE from COPIED up to a into OUTBUF
  COPIED @ SWAP OVER - +OUT ;
: OBJECT-HERE ( a u -- rec )   \ the object with the name a u, seen from IN-SOURCE
  2DUP IN-SOURCE @ S-WL @ FIND-IN ?DUP IF  NIP NIP EXIT  THEN
  2DUP PUBLICS @ FIND-IN ?DUP IF  NIP NIP EXIT  THEN
  S" a renamed name, but no object: " 2SWAP STOP ;
: LINE>OUT ( -- )   \ LINE into OUTBUF; each name that PICK renames gets its new name
  LINE COPIED !  LINE REST-A !  #LINE @ REST-U !
  BEGIN TOKEN DUP WHILE
    2DUP CLASHES @ FIND-IN IF
      OVER UP-TO  2DUP + COPIED !  OBJECT-HERE NEW-NAME +OUT
    ELSE  2DROP  THEN
  REPEAT  2DROP  LINE #LINE @ + UP-TO ;

: COUNT-OUT ( rec -- )   1 SWAP R-OUT +! ;
: TAKE ( rec -- )   \ write LINE with the new names, if rec is in the hull
  DUP KEPT FLAG? IF  COUNT-OUT  LINE>OUT OUT-LINE  ELSE  DROP  THEN ;
: TAKE-RAW ( rec -- )   \ write LINE as it is, if rec is in the hull
  DUP KEPT FLAG? IF  COUNT-OUT  LINE #LINE @ OUT-TEXT  ELSE  DROP  THEN ;

\ A third pass over a package file: each line of an object of the hull
\ goes to the copy, the lines of functions in CODE-PASS, the lines of
\ data in DATA-PASS.
1 CONSTANT CODE-PASS   2 CONSTANT DATA-PASS
VARIABLE WRITING     \ CODE-PASS or DATA-PASS
CREATE SAVED-ALIGN  256 ALLOT  VARIABLE #SAVED-ALIGN   \ a line "8 DATA-ALIGN" for the next DATA:
: CODE? ( -- f )   WRITING @ CODE-PASS = ;
: DATA? ( -- f )   WRITING @ DATA-PASS = ;
: OBJECT-OF ( a u -- rec )   IN-SOURCE @ S-WL @ FIND-IN ;   \ the object of the package IN-SOURCE

: COPY-CODE-LINE ( -- )   \ a line of the function IN-CODE
  CODE? IF  IN-CODE @ TAKE  THEN
  S" END-CODE" HAS? IF
    CODE?  IN-CODE @ KEPT FLAG?  AND IF  OUT-LINE  THEN   \ an empty line after the function
    0 IN-CODE !
  THEN ;
: COPY-DATA-LINE ( -- )   \ a line that starts with a blank: a line of the data IN-DATA
  IN-DATA @ ?DUP 0= IF  EXIT  THEN  DATA? 0= IF  DROP EXIT  THEN
  DUP R-KIND @ DICT-DATA = IF  TAKE-RAW  ELSE  TAKE  THEN ;
: COPY-ALIGN ( rec -- )   \ the saved line "8 DATA-ALIGN" before the DATA: of rec
  #SAVED-ALIGN @ 0= IF  DROP EXIT  THEN
  DUP KEPT FLAG? IF  COUNT-OUT  SAVED-ALIGN #SAVED-ALIGN @ OUT-TEXT  ELSE  DROP  THEN ;
: COPY-NUMBER-LINE ( -- )   \ n DATA: name, 8 DATA-ALIGN, or a line with SYMBOL!
  FIRST 2DROP TOKEN
  S" DATA:" IS? IF  2DROP TOKEN OBJECT-OF  DUP IN-DATA !
    DATA? IF  DUP COPY-ALIGN TAKE  ELSE  DROP  THEN  0 #SAVED-ALIGN !  EXIT  THEN
  S" DATA-ALIGN" IS? IF  2DROP  LINE SAVED-ALIGN #LINE @ MOVE  #LINE @ #SAVED-ALIGN !  EXIT  THEN
  OBJECT-OF  CODE? IF  TAKE  ELSE  DROP  THEN ;   \ the data x of "n x SYMBOL! name"
: COPY-LINE ( -- )   \ a line of the package file: write it if it belongs to the hull
  IN-CODE @ IF  COPY-CODE-LINE EXIT  THEN
  #LINE @ IF  LINE C@ 32 = IF  COPY-DATA-LINE EXIT  THEN  THEN
  0 IN-DATA !  FIRST DUP 0= IF  2DROP EXIT  THEN
  S" CODE" IS? IF  2DROP TOKEN OBJECT-OF IN-CODE !  CODE? IF  IN-CODE @ TAKE  THEN  EXIT  THEN
  S" CREATE" IS? IF  2DROP TOKEN OBJECT-OF  DUP IN-DATA !  DATA? IF  TAKE  ELSE  DROP  THEN  EXIT  THEN
  NUMBER? IF  DROP COPY-NUMBER-LINE  THEN ;    \ PUBLIC, PRIVATE and the frame stay out
: COPY-SOURCE ( s pass -- )   \ write the lines of the hull of the package file of s
  WRITING !  OPEN  0 IN-CODE !  0 IN-DATA !  0 #SAVED-ALIGN !
  BEGIN READ WHILE  COPY-LINE  REPEAT  CLOSE ;

\ ==== The heads ========================================================

VARIABLE PASS-KIND   \ the copy that PICK writes now: 0 functions, 1 data
VARIABLE CUR-S       \ the first source of the copy of functions that PICK writes now
VARIABLE MEMBER-XT   \ the xt for EACH-MEMBER

: KEPT# ( s kind -- n )   \ the functions (kind 0) or data objects (kind 1) of s in the hull
  SWAP HULL-SUMS  CELLS SUMS + @ ;
: ALL# ( s kind -- n )   \ all functions (kind 0) or data objects (kind 1) of s
  2 * CELLS SWAP S-COUNTS + @ ;
: SAME-OUT? ( s1 s2 -- f )   S-OUT COUNT ROT S-OUT COUNT S= ;
: MEMBER? ( s -- f )   \ s gives lines to the copy that PICK writes now
  PASS-KIND @ IF  1 KEPT# 0<>  EXIT  THEN
  DUP CUR-S @ SAME-OUT?  SWAP 0 KEPT# 0<>  AND ;
: EACH-MEMBER ( xt -- )   \ run xt ( s -- ) for each source that gives lines to the copy
  MEMBER-XT !  #SOURCES @ 0 ?DO  I SRC DUP MEMBER? IF  MEMBER-XT @ EXECUTE  ELSE  DROP  THEN  LOOP ;

: FOLDER ( a u -- a u' )   \ the folders of the path a u, with the last /
  BEGIN DUP WHILE  2DUP + 1- C@ '/' = IF EXIT THEN  1-  REPEAT ;
: FILE-NAME ( a u -- a' u' )   2DUP FOLDER NIP /STRING ;   \ the path without its folders

64 BUFFER: PATHBUF
: ARCHIVE ( s -- a u )   \ the archive of s: the folder of its package file, the name after "of"
  DUP S-FILE COUNT FOLDER PATHBUF PLACE
  S-OBJECT COUNT SPLIT 2DROP  SPLIT S" of" S= 0= ABORT" PICK: no archive after 'of' in line 1"
  SPLIT 2SWAP 2DROP  PATHBUF +PLACE  PATHBUF COUNT ;
CREATE CHUNK  4096 ALLOT
: FILE-SHA ( a u -- a' )   \ the SHA-256 of the file a u: 32 bytes at a'
  2DUP R/O OPEN-FILE IF  DROP S" cannot read the archive " 2SWAP FAIL  THEN  NIP NIP  >R
  SHA-START
  BEGIN  CHUNK 4096 R@ READ-FILE THROW  ?DUP WHILE  CHUNK SWAP SHA-ADD  REPEAT
  R> CLOSE-FILE DROP  SHA-END ;
VARIABLE DIGESTS   \ the SHA-256 of the archives: the newest node (link, path, 32 bytes)
: DIGEST ( a u -- a' )   \ the SHA-256 of the file a u (32 bytes at a'), made once for each file
  DIGESTS @  BEGIN ?DUP WHILE
    >R  2DUP R@ CELL+ COUNT S= IF  2DROP R> CELL+ 64 + EXIT  THEN  R> @
  REPEAT
  ALIGN HERE >R  DIGESTS @ ,  2DUP HERE PLACE  64 ALLOT
  FILE-SHA  HERE DUP >R  32 MOVE  32 ALLOT  R>  R> DIGESTS ! ;
CREATE DIGEST$  64 ALLOT   \ a SHA-256 as 64 hex digits
: HEX-CHAR ( n -- c )   S" 0123456789abcdef" DROP + C@ ;
: >HEX$ ( a -- a' u )   \ the 32 bytes at a as 64 hex digits
  32 0 DO
    DUP I + C@  DUP 4 RSHIFT HEX-CHAR  DIGEST$ I 2 * + C!
    15 AND HEX-CHAR  DIGEST$ I 2 * 1+ + C!
  LOOP  DROP  DIGEST$ 64 ;
: EXPECTED$ ( a u -- a' u' )   \ the SHA-256 that PICK-SHA256 gives for the archive a u
  EXPECTED @  BEGIN ?DUP WHILE
    >R  2DUP R@ CELL+ COUNT S= IF  2DROP R> CELL+ 64 + COUNT EXIT  THEN  R> @
  REPEAT
  S" PICK-SHA256 is missing for " 2SWAP FAIL ;
: CHECK-ARCHIVE ( s -- )   \ the archive of s has the SHA-256 of PICK-SHA256
  ARCHIVE 2DROP
  PATHBUF COUNT EXPECTED$  PATHBUF COUNT DIGEST >HEX$  S= IF EXIT THEN
  CR ." archive  " PATHBUF COUNT TYPE
  CR ." SHA-256  " DIGEST$ 64 TYPE
  CR ." expected " PATHBUF COUNT EXPECTED$ TYPE CR
  S" another archive, or a VM that reads files only as text (awk, bash): " PATHBUF COUNT FAIL ;
: CHECK-ARCHIVES ( -- )   \ the archives of all sources with objects in the hull
  #SOURCES @ 0 ?DO
    I SRC  DUP 0 KEPT#  OVER 1 KEPT# +  IF  CHECK-ARCHIVE  ELSE  DROP  THEN
  LOOP ;

: +WORD ( a u -- )   \ a blank and a u into OUTBUF; a new line if the line gets too long
  DUP #OUT @ + 1+ 72 > IF  OUT-LINE  S" \  " +OUT  THEN  S"  " +OUT  +OUT ;
: OUT-RULE ( a u -- )   \ the line "\ ==== text ======...", 72 characters
  S" \ ==== " +OUT  +OUT  S"  " +OUT  BEGIN #OUT @ 72 < WHILE  S" =" +OUT  REPEAT  OUT-LINE ;
: WHAT ( -- a u )   PASS-KIND @ IF  S" data objects"  ELSE  S" functions"  THEN ;
: OUT-SOURCE ( s -- )   \ the package file s in the list of the head
  S" \   " +OUT  DUP S-FILE COUNT +OUT  S" :" OUT-TEXT
  S" \     " +OUT  DUP S-OBJECT COUNT +OUT  S" , " +OUT
  DUP PASS-KIND @ KEPT# N>S +OUT  S"  of its " +OUT  PASS-KIND @ ALL# N>S +OUT
  S"  " +OUT  WHAT OUT-TEXT ;
CREATE LAST-ARCHIVE  64 ALLOT
: OUT-ARCHIVE ( s -- )   \ the archive of s and its SHA-256, if the line before is not the same
  ARCHIVE  2DUP LAST-ARCHIVE COUNT S= IF  2DROP EXIT  THEN
  2DUP LAST-ARCHIVE PLACE  S" \   " +OUT  2DUP OUT-TEXT
  S" \     " +OUT  DIGEST >HEX$ OUT-TEXT ;
: OUT-LICENSE ( -- )   #LICENSE @ 0 ?DO  S" \ " +OUT  I 80 * LICENSE + COUNT OUT-TEXT  LOOP ;
: +RENAMED ( rec -- )   DUP RENAMED FLAG? IF  NEW-NAME +WORD  ELSE  DROP  THEN ;
: OUT-NEW-NAMES ( -- )   \ the new names, if there are any
  #RENAMED @ 0= IF EXIT THEN
  S" \ New names:" +OUT  ['] +RENAMED EACH-KEPT  OUT-LINE ;

: OUT-HEAD ( a u -- )   \ the head of the copy with the path a u
  S" \ " +OUT  FILE-NAME +OUT  S"  - " +OUT
  PASS-KIND @ IF  S" data"  ELSE  S" functions"  THEN +OUT
  S"  for " +OUT  FOR-NAME COUNT +OUT  S" , made by tools/pick.fs" OUT-TEXT
  S" \" OUT-TEXT
  S" \ tools/pick.fs made this file from the pick list" OUT-TEXT
  S" \ " +OUT  FOR-LIST COUNT +OUT  S" : do not change it by hand. It has the" OUT-TEXT
  PASS-KIND @ IF
    S" \ data that the functions of the pick list need, each line as in its" OUT-TEXT
    S" \ package file of the quarry. Changes: the frames of the packages are" OUT-TEXT
    S" \ not here, and a name that two objects have gets the tag of its object" OUT-TEXT
    S" \ (tag:name)." OUT-TEXT
  ELSE
    S" \ functions that the entries of the pick list need, each line as in its" OUT-TEXT
    S" \ package file of the quarry; their data are in " +OUT
    DATA-OUT COUNT FILE-NAME +OUT  S" . Changes:" OUT-TEXT
    S" \ the frames of the packages are not here, and a name that two objects" OUT-TEXT
    S" \ have gets the tag of its object (tag:name)." OUT-TEXT
  THEN
  S" \" OUT-TEXT
  S" \ From:" OUT-TEXT  ['] OUT-SOURCE EACH-MEMBER
  S" \ SHA-256 of the archives:" OUT-TEXT  0 LAST-ARCHIVE C!  ['] OUT-ARCHIVE EACH-MEMBER
  OUT-LICENSE  OUT-NEW-NAMES  OUT-LINE ;

: OUT-BODY ( s -- )   \ the lines of the hull of s in the copy
  DUP S-OBJECT COUNT OUT-RULE
  PASS-KIND @ IF  DATA-PASS  ELSE  OUT-LINE CODE-PASS  THEN  COPY-SOURCE ;
: WRITE-COPY ( a u -- )   \ write the copy with the path a u
  2DUP OUT-OPEN  2DUP OUT-HEAD  ['] OUT-BODY EACH-MEMBER  OUT-CLOSE
  ."   " TYPE ." : " #OUT-LINES @ 0 .R ."  lines" CR ;
: FIRST-MEMBER? ( i -- f )   \ the source i is the first one with functions for its copy
  DUP SRC CUR-S !  0 PASS-KIND !
  DUP SRC MEMBER? 0= IF  DROP FALSE EXIT  THEN
  TRUE SWAP 0 ?DO  I SRC MEMBER? IF  DROP FALSE LEAVE  THEN  LOOP ;
: WRITE-ALL ( -- )   \ the copies of the functions, then the copy of the data
  #SOURCES @ 0 ?DO  I FIRST-MEMBER? IF  I SRC S-OUT COUNT WRITE-COPY  THEN  LOOP
  1 PASS-KIND !  DATA-OUT COUNT WRITE-COPY ;

VARIABLE #WRONG
: WRITTEN? ( rec -- )   \ each line of the object went to a copy, once
  DUP R-OUT @  OVER R-LINES @ <> IF
    CR ." not written as read: " DUP NAME$ TYPE  1 #WRONG +!  THEN  DROP ;
: CHECK-WRITTEN ( -- )   \ each object of the hull is in the copies with all its lines
  0 #WRONG !  ['] WRITTEN? EACH-KEPT
  #WRONG @ IF  CR S" objects not written as read: " #WRONG @ N>S FAIL  THEN ;

\ ==== PICK-RUN and PICK-WRITE ==========================================

: (RUN) ( -- )
  WORDLIST PUBLICS !  WORDLIST OUTSIDES !  0 TODO !
  #SOURCES @ 0 ?DO  I SRC S-PACKAGE  /SOURCE 136 -  0 FILL  WORDLIST I SRC S-WL !  LOOP
  #SOURCES @ 0 ?DO  I SRC READ-SOURCE  CHECK-HEAD  LOOP
  REPORT
  ALLOW  #SOURCES @ 0 ?DO  I SRC READ-REFS  LOOP
  TAKE-ENTRIES  HULL  HULL-REPORT  CHECK-OUTSIDE ;
: (WRITE) ( -- )
  DATA-OUT C@ 0= IF  S" PICK-WRITE: " S" PICK-DATA is missing" FAIL  THEN
  FOR-NAME C@ 0= IF  S" PICK-WRITE: " S" PICK-FOR is missing" FAIL  THEN
  CHECK-ARCHIVES  RENAMES  CR ." the copies:" CR  WRITE-ALL  CHECK-WRITTEN ;
: CASED ( xt -- )   \ run xt; names compared with the case; the files closed after an error
  CASE-SENSE @ >R  CASE-SENSE ON!
  CATCH  CLOSE  OUT-CLOSE  R> CASE-SENSE !  THROW ;

PUBLIC
: PICK-RUN ( -- )   \ read the package files, check them, find the hull
  MILLIS  HERE  ['] (RUN) CASED  HERE SWAP -  MILLIS ROT -
  ." done in " 0 .R ." ms; " 0 .R ."  bytes of the dictionary" CR ;
: PICK-WRITE ( -- )   \ after PICK-RUN: write the copies
  MILLIS  ['] (WRITE) CASED  MILLIS SWAP -
  ." written in " 0 .R ." ms; " #RENAMED @ 0 .R ."  names renamed" CR ;
: PICK-LIST ( -- )   \ after PICK-RUN: each object of the hull, as "package name"
  #SOURCES @ 0 ?DO
    I SRC S-WL @ @  BEGIN ?DUP WHILE
      DUP NAME>XT CELL+ @ KEPT FLAG? IF
        I SRC S-PACKAGE COUNT TYPE SPACE  DUP CELL+ COUNT 31 AND TYPE CR
      THEN  @
    REPEAT
  LOOP ;

END-PACKAGE
