\ genesis.fs - uCForth builds its reference images itself
\
\ Use (in the folder ucf):
\   LC_ALL=C awk -f bootstrap/awk/ucf.awk -- host/files.fs tools/genesis.fs
\   LC_ALL=C awk -f bootstrap/awk/ucf.awk -- -32 host/files.fs tools/genesis.fs
\
\ The tool runs a second uCForth, the target, in the package VM
\ (lib/vm.fs). It does what a bootstrap VM does (doc/genesis.md):
\
\ 1. Genesis in Forth: the start dictionary with only the mandatory
\    primitives and the HAL, then core.fs line by line. The result is
\    the seed.
\ 2. QUIT of the target compiles base.fs. The result is the base image.
\
\ It writes each image as Intel HEX (lib/ihex.fs) and compares the text
\ with seed/core16.hex and seed/base16.hex (32 bit: ...32.hex), line
\ for line. The target has the cell size of the host. At the end: the
\ exit status 0 if both images are the same as the reference.

ONLY FORTH DEFINITIONS  ALSO FILES
S" lib/ihex.fs" INCLUDED
S" lib/vm.fs" INCLUDED
ONLY FORTH  ALSO FILES  ALSO IHEX  ALSO VM  ALSO INTERNALS
VOCABULARY GENESIS  ALSO GENESIS DEFINITIONS

WL-FORTH CONSTANT FW   WL-INTERNALS CONSTANT IW   $100 CONSTANT SYS
: T+! ( n ta -- )   MEM + +! ;
: T,  ( x -- )      DP T@ T!  CELL DP T+! ;
: SKIP1 ( a u -- a+1 u-1 )   1- SWAP 1+ SWAP ;
: SAME$ ( a1 u1 a2 u2 -- f )   \ the same text, with the same case
  ROT OVER <> IF 2DROP DROP FALSE EXIT THEN
  0 ?DO  OVER I + C@ OVER I + C@ <> IF 2DROP UNLOOP FALSE EXIT THEN  LOOP
  2DROP TRUE ;
: SAME-CI$ ( a1 u1 a2 u2 -- f )   \ the same text, a-z = A-Z
  ROT OVER <> IF 2DROP DROP FALSE EXIT THEN
  0 ?DO  OVER I + C@ UPC OVER I + C@ UPC <> IF 2DROP UNLOOP FALSE EXIT THEN  LOOP
  2DROP TRUE ;
VARIABLE HA   VARIABLE HN
: HAS$ ( a u a2 u2 -- f )   \ true if the text a u holds the text a2 u2
  HN !  HA !
  BEGIN  DUP HN @ < 0= WHILE
    OVER HN @ HA @ HN @ SAME$ IF 2DROP TRUE EXIT THEN  SKIP1
  REPEAT  2DROP FALSE ;

\ ==== Lines of a file (doc/genesis.md, section 2) ======================
\ The text between two LF is a line; the text after the last LF is one
\ more line, also when it is empty. A CR at the end goes away.

CREATE CHUNK 128 ALLOT   VARIABLE CPOS   VARIABLE CLEN
VARIABLE LFID   VARIABLE LEOF   VARIABLE LNO   VARIABLE LLONG
CREATE LBUF 256 ALLOT    VARIABLE LLEN
CREATE LNAME 64 ALLOT    VARIABLE LNAME#   \ the name of the file

: L-OPEN ( a u -- )
  63 MIN DUP LNAME# !  LNAME SWAP MOVE
  LNAME LNAME# @ R/O OPEN-FILE ?DUP IF
    ."   cannot read " LNAME LNAME# @ TYPE CR  1 (BYE)
  THEN  LFID !  0 CPOS !  0 CLEN !  FALSE LEOF !  0 LNO ! ;
: L-CLOSE ( -- )   LFID @ CLOSE-FILE DROP ;
: GETC ( -- c | -1 )
  CPOS @ CLEN @ = IF
    CHUNK 128 LFID @ READ-FILE THROW CLEN !  0 CPOS !
    CLEN @ 0= IF -1 EXIT THEN
  THEN  CHUNK CPOS @ + C@  1 CPOS +! ;
: L-NEXT ( -- f )   \ the next line into LBUF LLEN; false after the last one
  LEOF @ IF FALSE EXIT THEN
  0 LLEN !  FALSE LLONG !  1 LNO +!
  BEGIN  GETC DUP 10 <> OVER -1 <> AND  WHILE
    LLEN @ 256 < IF LBUF LLEN @ + C!  1 LLEN +! ELSE DROP TRUE LLONG ! THEN
  REPEAT
  -1 = IF TRUE LEOF ! THEN
  LLEN @ IF  LBUF LLEN @ + 1- C@ 13 = IF -1 LLEN +! THEN  THEN  TRUE ;

\ ==== Errors ============================================================

: STOPPED ( -- )   \ the end of the tool after an error
  ."   " LNAME LNAME# @ TYPE ." :" LNO @ 0 .R ." : loading stopped" CR
  1 (BYE) ;
VARIABLE WA   VARIABLE WN             \ the word that Genesis handles now
: .CODE ( n -- )   DUP MESSAGE ?DUP IF TYPE DROP ELSE DROP ." error " . THEN ;
: G-FAIL ( n -- )   \ a THROW code during Genesis
  ."   error: " .CODE  STOPPED ;
: G-FAIL-WORD ( n -- )   \ the same, with the word
  ."   error: " .CODE ." : " WA @ WN @ TYPE CR  STOPPED ;

\ ==== Search in the target ==============================================
\ NKEY holds the name, in upper case if ANYCASE is true.

CREATE NKEY 32 ALLOT   VARIABLE KN   VARIABLE ANYCASE
: UPC? ( c -- c' )   ANYCASE @ IF UPC THEN ;
: KEY! ( a u -- )   DUP KN !  0 ?DO  DUP I + C@ UPC? NKEY I + C!  LOOP DROP ;
: G-KEY ( a u -- )   CASE-SENSE T@ 0= ANYCASE !  KEY! ;
: TXT ( tnt -- txt )   DUP CELL+ TC@ 31 AND + CELL+ 1+ ALIGNED ;
: NKEY? ( tnt -- f )   \ true if the name of tnt is NKEY
  DUP CELL+ TC@ 31 AND KN @ <> IF DROP FALSE EXIT THEN
  CELL+ 1+ T  KN @ 0 ?DO
    DUP I + C@ UPC? NKEY I + C@ <> IF DROP UNLOOP FALSE EXIT THEN
  LOOP  DROP TRUE ;
: WL-SEARCH ( twl -- tnt | 0 )   \ the newest word NKEY in a wordlist
  T@ BEGIN DUP WHILE DUP NKEY? IF EXIT THEN T@ REPEAT ;
: T-FIND ( -- tnt | 0 )   \ NKEY in the search order of the target
  KN @ 0=  KN @ 31 >  OR IF 0 EXIT THEN
  #ORDER T@ 0 ?DO
    CONTEXT #ORDER T@ 1- I - CELLS + T@ WL-SEARCH ?DUP IF UNLOOP EXIT THEN
  LOOP  0 ;
: OLDEST ( twl -- tnt | 0 )   \ the oldest word NKEY in a wordlist
  0 SWAP T@ BEGIN ?DUP WHILE DUP NKEY? IF NIP DUP THEN T@ REPEAT ;
: SYSXT ( a u -- txt | 0 )   \ the oldest word with exactly this name
  FALSE ANYCASE !  KEY!
  FW OLDEST ?DUP 0= IF IW OLDEST THEN  DUP IF TXT THEN ;

VARIABLE BEST   VARIABLE X
: BODY-END ( txt -- ta )   \ the next header in memory after txt
  X !  -1 BEST !
  VOC-LINK T@ BEGIN ?DUP WHILE
    DUP T@ BEGIN ?DUP WHILE
      DUP X @ U>  OVER BEST @ U<  AND IF DUP BEST ! THEN  T@
    REPEAT  CELL+ T@
  REPEAT  BEST @ ;

\ ==== The host primitives of Genesis (doc/genesis.md, section 3) =======

VARIABLE PA   VARIABLE PN             \ the word in TIB: target address, length
: G-PARSE-NAME ( -- )
  >IN T@
  BEGIN DUP #TIB T@ < IF DUP TIB + TC@ 33 < ELSE 0 THEN WHILE 1+ REPEAT
  DUP TIB + PA !
  BEGIN DUP #TIB T@ < IF DUP TIB + TC@ 32 > ELSE 0 THEN WHILE 1+ REPEAT
  DUP PA @ TIB - - PN !
  DUP #TIB T@ < IF 1+ THEN  >IN T! ;
: G-PARSE ( c -- )
  >R  >IN T@ DUP TIB + PA !
  BEGIN DUP #TIB T@ < IF DUP TIB + TC@ R@ <> ELSE 0 THEN WHILE 1+ REPEAT
  R> DROP  DUP PA @ TIB - - PN !
  DUP #TIB T@ < IF 1+ THEN  >IN T! ;

CREATE GNT 16 CELLS ALLOT   VARIABLE #GNT   \ the headers that go away at the end
: GNT+ ( tnt -- )   GNT #GNT @ CELLS + !  1 #GNT +! ;
: GNT? ( tnt -- f )
  #GNT @ 0 ?DO  DUP GNT I CELLS + @ = IF DROP UNLOOP TRUE EXIT THEN  LOOP
  DROP FALSE ;

VARIABLE GA   VARIABLE GN   VARIABLE NT   VARIABLE COLON   VARIABLE SEMI
: G-HEADER ( ta u -- )   \ a new header in CURRENT, not visible yet
  GN !  GA !
  GN @ 0= IF -16 G-FAIL THEN  GN @ 31 > IF -19 G-FAIL THEN
  GA @ T GN @ G-KEY  CURRENT T@ WL-SEARCH ?DUP IF
    GNT? 0= IF ."   redefined " GA @ T GN @ TYPE CR THEN
  THEN
  BEGIN DP T@ CELL 1- AND WHILE  0 DP T@ TC!  1 DP T+!  REPEAT
  DP T@ NT !
  NT @ CELL+ 1+ GN @ + ALIGNED                        ( txt )
  DUP 4 CELLS +  DUP DICT-END U>  SWAP WINDOW U> OR IF -8 G-FAIL THEN
  CURRENT T@ T@ NT @ T!  GN @ NT @ CELL+ TC!
  GA @ T  NT @ CELL+ 1+ T  GN @ MOVE
  DUP  NT @ CELL+ 1+ GN @ +  TUCK - SWAP T SWAP 0 FILL
  DP T!  NT @ LATEST T!
  GN @ 1 = IF
    GA @ TC@ 58 = COLON @ 0= AND IF NT @ DUP COLON ! GNT+ THEN
    GA @ TC@ 59 = SEMI @ 0= AND IF NT @ DUP SEMI ! GNT+ THEN
  THEN ;

: P-PARSE-NAME   G-PARSE-NAME  PA @ TPUSH  PN @ TPUSH ;
: P-PARSE        TPOP 255 AND G-PARSE  PA @ TPUSH  PN @ TPUSH ;
: P-HEADER       TPOP TPOP SWAP G-HEADER ;
: P-REVEAL       LATEST T@ ?DUP IF CURRENT T@ T! THEN ;
: P-IMMEDIATE    LATEST T@ CELL+ DUP TC@ 128 OR SWAP TC! ;
: P-,            TPOP T, ;
: P-C,           TPOP DP T@ TC!  1 DP T+! ;
: P-[            0 STATE T! ;
: P-]            TRUE STATE T! ;

' P-PARSE-NAME 100 OP   ' P-PARSE 101 OP   ' P-HEADER 102 OP
' P-REVEAL 103 OP   ' P-IMMEDIATE 104 OP   ' P-, 105 OP   ' P-C, 106 OP
' P-[ 107 OP   ' P-] 108 OP

\ ==== The start dictionary (doc/genesis.md, section 1) =================

VARIABLE HWL   VARIABLE HFL   VARIABLE XT-LIT
: MKHEAD ( a u -- txt )   \ a visible header in HWL with the flags HFL
  DP T@ NT !  HWL @ T@ NT @ T!
  DUP HFL @ OR NT @ CELL+ TC!
  TUCK NT @ CELL+ 1+ T SWAP MOVE
  NT @ HWL @ T!  NT @ LATEST T!
  NT @ CELL+ 1+ + ALIGNED  DUP DP T! ;
: PRIM ( a u op flags twl -- )   \ a header for a primitive
  HWL !  HFL !  >R MKHEAD R>             ( txt op )
  2DUP SWAP T!  OVER CELL+ DP T!
  DUP 5 = IF OVER XT-LIT ! THEN
  100 < 0= IF LATEST T@ GNT+ THEN  DROP ;
: SV ( x a u twl -- )   \ a constant for a system variable
  HWL !  0 HFL !  MKHEAD  2 OVER T!  TUCK CELL+ T!  2 CELLS + DP T! ;

: PRIMS ( -- )   \ 4-31, 80-88, 100-108: only the mandatory ones (-pure)
  S" EXIT" 4 64 FW PRIM         S" LIT" 5 64 IW PRIM
  S" BRANCH" 6 64 IW PRIM       S" ?BRANCH" 7 64 IW PRIM
  S" EXECUTE" 8 0 FW PRIM       S" @" 9 0 FW PRIM
  S" !" 10 0 FW PRIM            S" C@" 11 0 FW PRIM
  S" C!" 12 0 FW PRIM           S" DUP" 13 0 FW PRIM
  S" DROP" 14 0 FW PRIM         S" SWAP" 15 0 FW PRIM
  S" OVER" 16 0 FW PRIM         S" >R" 17 64 FW PRIM
  S" R>" 18 64 FW PRIM          S" R@" 19 64 FW PRIM
  S" SP@" 20 0 IW PRIM          S" SP!" 21 0 IW PRIM
  S" RP@" 22 0 IW PRIM          S" RP!" 23 0 IW PRIM
  S" AND" 24 0 FW PRIM          S" OR" 25 0 FW PRIM
  S" XOR" 26 0 FW PRIM          S" 0<" 27 0 FW PRIM
  S" UM+" 28 0 IW PRIM          S" ?RX" 29 0 IW PRIM
  S" TX!" 30 0 IW PRIM          S" !IO" 31 0 IW PRIM
  S" MILLIS" 80 0 FW PRIM       S" MS" 81 0 FW PRIM
  S" US" 82 0 FW PRIM           S" BIS!" 83 0 FW PRIM
  S" BIC!" 84 0 FW PRIM         S" SAVE-IMAGE" 85 0 IW PRIM
  S" ERASE-IMAGE" 86 0 IW PRIM  S" REBOOT" 87 0 IW PRIM
  S" BYE" 88 0 FW PRIM
  S" PARSE-NAME" 100 0 IW PRIM  S" PARSE" 101 0 IW PRIM
  S" HEADER" 102 0 IW PRIM      S" REVEAL" 103 0 IW PRIM
  S" IMMEDIATE" 104 0 FW PRIM   S" ," 105 0 FW PRIM
  S" C," 106 0 FW PRIM          S" [" 107 128 FW PRIM
  S" ]" 108 0 FW PRIM ;

: SVS ( -- )   \ the constants of the system variables (doc/vm.md, section 4)
  DP S" DP" IW SV               VP S" VP" IW SV
  LATEST S" LATEST" IW SV       STATE S" STATE" IW SV
  BASE S" BASE" FW SV           >IN S" >IN" IW SV
  #TIB S" #TIB" IW SV           CURRENT S" CURRENT" IW SV
  #ORDER S" #ORDER" IW SV       CONTEXT S" CONTEXT" IW SV
  VOC-LINK S" VOC-LINK" IW SV   CASE-SENSE S" CASE-SENSE" IW SV
  BASE-END S" BASE-END" IW SV   FROZEN-END S" FROZEN-END" IW SV
  AUTORUN S" AUTORUN" IW SV     ERRMSG S" ERRMSG" IW SV
  HANDLER S" HANDLER" IW SV
  TIB S" TIB" IW SV             PAD S" PAD" IW SV
  S0 S" S0" IW SV               R0 S" R0" IW SV
  DATA-END S" DATA-END" IW SV   WL-FORTH S" WL-FORTH" IW SV
  WL-INTERNALS S" WL-INTERNALS" IW SV
  CELL S" CELL" IW SV           DICT-END S" DICT-END" IW SV ;

: G-START ( -- )
  127 HALT-XT T!  HALT-XT HALT-XT CELL+ T!
  S0 8 CELLS + DP T!  IW 3 CELLS + VP T!  10 BASE T!   \ 8 free cells above S0: doc/vm.md 3
  0 FW CELL+ T!  FW IW CELL+ T!  IW VOC-LINK T!
  S0 T SP !  R0 T RP !
  PRIMS  SVS
  2 #ORDER T!  FW CONTEXT T!  IW CONTEXT CELL+ T!  FW CURRENT T! ;

\ ==== Genesis: one line of core.fs (doc/genesis.md, section 2) =========

VARIABLE XT-CATCH   VARIABLE GSKIP   VARIABLE GSKIPF

: G-EXEC ( txt -- )   \ run a target word; through CATCH once core.fs has it
  XT-CATCH @ IF
    TPUSH  XT-CATCH @ TRUN  TPOP ?DUP IF G-FAIL THEN
  ELSE  TRUN  THEN ;

1 CELL 8 * 1- LSHIFT CONSTANT SIGNBIT
VARIABLE NEG   VARIABLE NB   VARIABLE NV   VARIABLE OVFL
: ACC ( d -- )   \ NV = NV * NB + d; OVFL if it does not fit a cell
  OVFL @ IF DROP EXIT THEN
  NV @ NB @ UM* IF 2DROP TRUE OVFL ! EXIT THEN
  UM+ IF DROP TRUE OVFL ! EXIT THEN  NV ! ;
: G-NUMBER? ( -- n true | false )   \ the word WA WN as a number (section 4)
  WA @ WN @
  DUP 3 = IF OVER C@ 39 = IF OVER 2 + C@ 39 = IF DROP 1+ C@ TRUE EXIT THEN THEN THEN
  FALSE NEG !  BASE T@ NB !  0 NV !  FALSE OVFL !
  DUP 1 > IF OVER C@ 45 = IF OVER 1+ C@ PREFIX IF SKIP1 TRUE NEG ! THEN THEN THEN
  DUP IF OVER C@ PREFIX ?DUP IF NB ! SKIP1 THEN THEN
  DUP IF OVER C@ 45 = IF SKIP1 NEG @ 0= NEG ! THEN THEN
  DUP 0=  NB @ 2 < OR  NB @ 36 > OR IF 2DROP FALSE EXIT THEN
  BEGIN DUP WHILE
    OVER C@ NB @ DIGIT? 0= IF 2DROP DROP FALSE EXIT THEN  ACC  SKIP1
  REPEAT  2DROP
  OVFL @ IF -24 G-FAIL-WORD THEN
  NV @  NEG @ IF DUP SIGNBIT U> IF -24 G-FAIL-WORD THEN NEGATE THEN  TRUE ;

: G-SKIP ( -- )   \ "?:" skips a definition up to ";"
  WA @ WN @ S" \" SAME$ IF #TIB T@ >IN T! EXIT THEN
  WA @ WN @ S" (" SAME$ IF 41 G-PARSE EXIT THEN
  WA @ WN @ S" ;" SAME$ IF FALSE GSKIP !  TRUE GSKIPF ! THEN ;
: FLAG-WORD? ( -- f )   \ a flag after a skipped definition
  WA @ WN @ S" IMMEDIATE" SAME-CI$
  WA @ WN @ S" UNCHECKED" SAME-CI$ OR
  WA @ WN @ S" COMPILE-ONLY" SAME-CI$ OR ;
: G-?: ( -- )
  >IN T@ >R  G-PARSE-NAME  PN @ 0= IF -16 G-FAIL THEN
  PA @ T PN @ G-KEY  T-FIND IF R> DROP TRUE GSKIP ! EXIT THEN
  R> >IN T!  S" :" G-KEY T-FIND TXT G-EXEC ;
: G-DO ( tnt -- )   \ execute or compile a word that the target has
  DUP TXT SWAP CELL+ TC@                             ( txt flags )
  STATE T@ 0=  OVER 128 AND  OR IF
    STATE T@ 0= SWAP 64 AND 0<> AND IF -14 G-FAIL-WORD THEN  G-EXEC
  ELSE  DROP T,  THEN ;
: G-WORD ( -- )   \ one word of core.fs: WA WN
  GSKIP @ IF G-SKIP EXIT THEN
  GSKIPF @ IF FLAG-WORD? IF EXIT THEN  FALSE GSKIPF ! THEN
  WA @ WN @ S" ?:" SAME$ IF G-?: EXIT THEN
  WA @ WN @ G-KEY T-FIND ?DUP IF G-DO EXIT THEN
  WA @ WN @ S" \" SAME$ IF #TIB T@ >IN T! EXIT THEN   \ comments until core.fs has them
  WA @ WN @ S" (" SAME$ IF 41 G-PARSE EXIT THEN
  G-NUMBER? 0= IF -13 G-FAIL-WORD THEN
  STATE T@ IF XT-LIT @ T, T, ELSE TPUSH THEN ;
: G-LINE ( -- )   \ interpret one line of core.fs: LBUF LLEN
  LLONG @ IF ."   error: line too long (at most 256 bytes)" CR STOPPED THEN
  LBUF TIB T LLEN @ MOVE  LLEN @ #TIB T!  0 >IN T!
  BEGIN  G-PARSE-NAME  PN @ WHILE
    PA @ T WA !  PN @ WN !  G-WORD
  REPEAT ;

VARIABLE WLV   VARIABLE PREV   VARIABLE NX
: UNLINK ( -- )   \ remove the host primitives and the first ":" ";"
  VOC-LINK T@ BEGIN ?DUP WHILE
    DUP WLV !  0 PREV !  T@
    BEGIN ?DUP WHILE
      DUP T@ NX !
      DUP GNT? IF  DROP  NX @ PREV @ ?DUP IF T! ELSE WLV @ T! THEN
      ELSE  PREV !  THEN  NX @
    REPEAT  WLV @ CELL+ T@
  REPEAT ;

: G-CORE ( a u -- )   \ Genesis: compile core.fs (sections 1, 2 and 5)
  G-START  L-OPEN
  BEGIN L-NEXT WHILE
    G-LINE
    XT-CATCH @ 0= IF  S" CATCH" G-KEY T-FIND ?DUP IF TXT XT-CATCH ! THEN  THEN
  REPEAT  L-CLOSE  UNLINK ;

\ ==== Images: write and compare =========================================

VARIABLE XT-QUIT   VARIABLE XT-BOOT   VARIABLE FAILS
CREATE HLINE 100 ALLOT   VARIABLE HLEN
: +S ( a u -- )   TUCK HLINE HLEN @ + SWAP MOVE  HLEN +! ;
: +N ( u -- )   BASE @ >R DECIMAL  0 <# #S #> +S  R> BASE ! ;
: META ( boot -- a u )   \ the first line of an image (doc/vm.md, section 10)
  0 HLEN !  S" ; ucf image bits=" +S  CELL 8 * +N
  S"  boot=" +S +N  S"  quit=" +S XT-QUIT @ +N  S"  throw=" +S T-THROW @ +N
  S"  prims=0-31,80-88" +S  HLINE HLEN @ ;
: IMAGE ( boot -- )   \ the image as lines, through IHEX-LINE
  META IHEX-LINE  IHEX-START
  SYS T  FW SYS -  SYS IHEX-DATA
  FW T  VP T@ FW -  FW IHEX-DATA
  S0 T  DP T@ S0 -  S0 IHEX-DATA  IHEX-END ;

CREATE FNAME 32 ALLOT   VARIABLE FLEN
VARIABLE SFID   CREATE SBUF 100 ALLOT   VARIABLE DIFFS   VARIABLE NLINE   VARIABLE FIRST
: CHECK-LINE ( a u -- )   \ compare a line of the image with the next line of SFID
  1 NLINE +!
  SBUF 100 SFID @ READ-LINE THROW  IF SBUF SWAP SAME$ ELSE DROP 2DROP FALSE THEN
  0= IF  DIFFS @ 0= IF NLINE @ FIRST ! THEN  1 DIFFS +!  THEN ;
: CHECK ( boot a u -- )   \ compare the image with the file seed/<a u><bits>.hex
  0 HLEN !  S" seed/" +S  2DUP +S  CELL 8 * +N  S" .hex" +S
  HLINE FNAME HLEN @ MOVE  HLEN @ FLEN !
  ."   check " TYPE CELL 8 * 0 .R ." : "
  FNAME FLEN @ R/O OPEN-FILE IF  DROP ." cannot read " FNAME FLEN @ TYPE CR  DROP 1 FAILS +! EXIT  THEN
  SFID !  0 DIFFS !  0 NLINE !
  ['] CHECK-LINE IS IHEX-LINE  IMAGE
  SBUF 100 SFID @ READ-LINE THROW NIP IF  1 DIFFS +!  THEN   \ the file has more lines
  SFID @ CLOSE-FILE DROP
  DIFFS @ IF
    ." not the same as " FNAME FLEN @ TYPE ." , first difference in line " FIRST @ 0 .R CR
    1 FAILS +!
  ELSE  ." the same as " FNAME FLEN @ TYPE CR  THEN ;

\ ==== QUIT of the target compiles base.fs (sections 5 and 6) ===========
\ The target reads a line only when its QUIT waits in REFILL (doc/vm.md,
\ section 9). Its output lines appear on the host; a line with "error:"
\ stops the tool. After the last line of base.fs the target stops.

CREATE IBUF 260 ALLOT   VARIABLE ILEN   VARIABLE IPOS
CREATE OBUF 256 ALLOT   VARIABLE OLEN   VARIABLE ERRSEEN
VARIABLE XT-REFILL   VARIABLE REFILL-END

: REFILLING? ( -- f )   \ a return address of the target points into REFILL
  RP @ T>  DUP 8 CELLS + R0 MIN  SWAP ?DO
    I T@ XT-REFILL @ 1+ REFILL-END @ WITHIN IF UNLOOP TRUE EXIT THEN
  CELL +LOOP  FALSE ;

: BUILD-END ( -- )   \ base.fs is complete: the base image
  DP T@ DUP BASE-END T! FROZEN-END T!
  1 #ORDER T!  FW CONTEXT T!  FW CURRENT T!  0 STATE T!
  S" BOOT" SYSXT XT-BOOT !
  XT-BOOT @ S" base" CHECK ;

: NEXT-INPUT ( -- f )   \ the next line of base.fs for the target; false at the end
  ERRSEEN @ IF STOPPED THEN
  0 OLEN !                          \ the prompt of QUIT is not wanted
  L-NEXT 0= IF  BUILD-END  HALT-XT CELL+ T IP !  FALSE EXIT  THEN
  LBUF IBUF LLEN @ MOVE  10 IBUF LLEN @ + C!  LLEN @ 1+ ILEN !  0 IPOS !  TRUE ;

: G-RX ( -- c true | false )
  IPOS @ ILEN @ < 0= IF
    REFILLING? 0= IF FALSE EXIT THEN
    NEXT-INPUT 0= IF FALSE EXIT THEN
  THEN  IBUF IPOS @ + C@  1 IPOS +!  TRUE ;

: G-TX ( c -- )
  255 AND  DUP 10 = IF
    DROP  OBUF OLEN @ TYPE CR
    OBUF OLEN @ S" error:" HAS$ IF TRUE ERRSEEN ! THEN  0 OLEN !  EXIT
  THEN
  DUP 13 = OVER 0= OR  OLEN @ 255 = OR IF DROP EXIT THEN
  OBUF OLEN @ + C!  1 OLEN +! ;

\ ==== The tool ==========================================================

: MAIN ( -- )
  ['] G-TX IS T-TX
  S" forth/core.fs" G-CORE
  S" THROW" SYSXT T-THROW !  S" QUIT" SYSXT XT-QUIT !
  S" REFILL" SYSXT DUP XT-REFILL !  BODY-END REFILL-END !
  XT-QUIT @ S" core" CHECK
  S" forth/base.fs" L-OPEN  0 ILEN !  0 IPOS !  ['] G-RX IS T-RX
  XT-QUIT @ TRUN  L-CLOSE
  FAILS @ IF ."   the images are not the same" CR  1 ELSE 0 THEN  (BYE) ;

MAIN
