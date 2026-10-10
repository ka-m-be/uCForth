\ base.fs - part 2 of the base vocabulary of uCForth
\
\ The Forth interpreter QUIT compiles this file after forth/core.fs.
\ Genesis is over: every line goes through INTERPRET, as later on a
\ board. Words that the VM may supply as primitives
\ are defined with "?:"; ';' then drops the Forth version.

\ ==== 1  DOES>, VALUE, DEFER ===========================================

>INTERNALS
: >BODY   ( xt -- a )   2 CELLS + ;
: (DOES>) ( -- )        R> LATEST @ NAME>XT  3 OVER !  CELL+ ! ;  UNCHECKED
>FORTH
: DOES>    ( -- )           LIT (DOES>) , ;                 IMMEDIATE COMPILE-ONLY
: VALUE    ( x "name" -- )  CELL VALLOT  TUCK !  CREATE ,  DOES> @ @ ;
: TO       ( x "name" -- )
  ' >BODY @  STATE @ IF POSTPONE LITERAL POSTPONE ! EXIT THEN  ! ;  IMMEDIATE

>INTERNALS
: DEFER    ( "name" -- )    CELL VALLOT  ['] NOOP OVER !  CREATE ,  DOES> @ @ EXECUTE ;
: IS       ( xt "name" -- ) POSTPONE TO ;  IMMEDIATE
>FORTH

\ ==== 2  Counted loops and CASE =======================================
\ (DO) and (?DO) carry the exit address in the next cell and put three
\ cells on the return stack: exit, limit, index. (LOOP) and (+LOOP)
\ carry the address of the loop start.

>INTERNALS
?: (DO)    ( limit start -- )  R> DUP @ >R  ROT >R  SWAP >R  CELL+ >R ;  UNCHECKED
?: (?DO)   ( limit start -- )
  2DUP = IF 2DROP R> @ >R EXIT THEN
  R> DUP @ >R  ROT >R  SWAP >R  CELL+ >R ;  UNCHECKED
?: (LOOP)  ( -- )
  R> R> 1+ R@ OVER = IF DROP R> DROP R> DROP CELL+ >R EXIT THEN
  >R @ >R ;  UNCHECKED
?: (+LOOP) ( n -- )     \ ends when the index crosses limit-1 / limit
  R> SWAP  R> R@ -
  2DUP + DUP R@ + >R
  OVER XOR  ROT ROT XOR  AND 0<
  IF R> DROP R> DROP R> DROP CELL+ >R EXIT THEN
  @ >R ;  UNCHECKED
>FORTH
?: I       ( -- n )   R> R@ SWAP >R ;
?: J       ( -- n )   RP@ 4 CELLS + @ ;
?: UNLOOP  ( -- )     R> R> DROP R> DROP R> DROP >R ;  UNCHECKED

: LEAVE    ( -- )        R> DROP R> DROP R> DROP ;    UNCHECKED COMPILE-ONLY
: DO       ( -- a1 a2 )  LIT (DO) ,   HERE 0 ,  HERE ;  IMMEDIATE COMPILE-ONLY
: ?DO      ( -- a1 a2 )  LIT (?DO) ,  HERE 0 ,  HERE ;  IMMEDIATE COMPILE-ONLY
: LOOP     ( a1 a2 -- )  LIT (LOOP) ,   ,  HERE SWAP ! ;  IMMEDIATE COMPILE-ONLY
: +LOOP    ( a1 a2 -- )  LIT (+LOOP) ,  ,  HERE SWAP ! ;  IMMEDIATE COMPILE-ONLY

: CASE     ( -- 0 )      0 ;                                IMMEDIATE COMPILE-ONLY
: OF       ( -- a )      LIT OVER ,  LIT = ,  POSTPONE IF  LIT DROP , ;  IMMEDIATE COMPILE-ONLY
: ENDOF    ( a -- a' )   POSTPONE ELSE ;                    IMMEDIATE COMPILE-ONLY
: ENDCASE  ( 0 a.. -- )  LIT DROP ,  BEGIN ?DUP WHILE POSTPONE THEN REPEAT ;  IMMEDIATE COMPILE-ONLY

\ ==== 3  Division =====================================================
\ Signed division rounds toward zero, as in C.

>INTERNALS
: S>D     ( n -- d )       DUP 0< ;
: DNEGATE ( d -- d' )      INVERT >R INVERT 1 UM+ R> + ;
: DABS    ( d -- ud )      DUP 0< IF DNEGATE THEN ;
: M*      ( n1 n2 -- d )   2DUP XOR >R  ABS SWAP ABS UM*  R> 0< IF DNEGATE THEN ;
: SM/REM  ( d n -- rem quot )
  2DUP XOR >R  OVER >R  ABS >R DABS R> UM/MOD
  SWAP R> 0< IF NEGATE THEN  SWAP R> 0< IF NEGATE THEN ;
>FORTH
: /MOD  ( n1 n2 -- rem quot )  >R S>D R> SM/REM ;
: /     ( n1 n2 -- quot )      /MOD NIP ;
: MOD   ( n1 n2 -- rem )       /MOD DROP ;
: */    ( n1 n2 n3 -- n4 )     >R M* R> SM/REM NIP ;
: DEPTH ( -- n )               SP@ S0 SWAP - CELL / ;

\ ==== 4  Wordlists and the search order (ONLY/ALSO) ==================
\ A wordlist record in the RAM data region: head, link, nt of the name.

>INTERNALS
: WORDLIST ( -- wl )
  3 CELLS VALLOT  0 OVER !  VOC-LINK @ OVER CELL+ !  0 OVER 2 CELLS + !  DUP VOC-LINK ! ;
: TOP      ( -- a )     #ORDER @ 1- CELLS CONTEXT + ;
: VOC      ( wl "name" -- )    \ give a wordlist a name
  CREATE DUP ,  LATEST @ SWAP 2 CELLS + !
  DOES> @  #ORDER @ 0= IF 1 #ORDER ! THEN  TOP ! ;
>FORTH

: VOCABULARY  ( "name" -- )  WORDLIST VOC ;
WL-FORTH VOC FORTH
WL-INTERNALS VOC INTERNALS
: ALSO        ( -- )  #ORDER @ 16 < 0= IF -49 THROW THEN  TOP @  1 #ORDER +!  TOP ! ;
: ONLY        ( -- )  1 #ORDER !  WL-FORTH CONTEXT ! ;
: PREVIOUS    ( -- )  #ORDER @ 1 > IF -1 #ORDER +! THEN ;
: DEFINITIONS ( -- )  TOP @ CURRENT ! ;

>INTERNALS
: .ID  ( nt -- )  CELL+ COUNT 31 AND TYPE ;
: .WL  ( wl -- )  2 CELLS + @ ?DUP IF .ID ELSE ." ?" THEN SPACE ;
>FORTH
: ORDER ( -- )
  #ORDER @ BEGIN DUP WHILE 1- DUP CELLS CONTEXT + @ .WL REPEAT DROP
  ."   current: " CURRENT @ .WL ;
: WORDS ( -- )    \ the words of the first wordlist in the search order
  CR 0  TOP @ @
  BEGIN ?DUP WHILE
    DUP CELL+ C@ 31 AND  ROT  2DUP + 70 > IF CR DROP 0 THEN  + 1+  SWAP
    DUP .ID SPACE  @
  REPEAT  DROP ;
: VOCS ( -- )     \ all wordlists, the newest first
  VOC-LINK @ BEGIN ?DUP WHILE
    DUP 2 CELLS + @ ?DUP IF CELL+ C@ 31 AND ELSE 1 THEN  OUT @ + 70 > IF CR THEN
    DUP .WL  CELL+ @
  REPEAT ;

\ ==== 5  Tools =======================================================

>INTERNALS
: .H2  ( c -- )  BASE @ >R HEX 0 <# # # #> TYPE R> BASE ! ;
: .HEX ( u -- )  BASE @ >R HEX 0 <# # # # # CELL 4 = IF # # # # THEN #> TYPE R> BASE ! ;
: >NAME ( xt -- nt | 0 )
  >R  VOC-LINK @
  BEGIN DUP WHILE
    DUP @ BEGIN DUP WHILE
      DUP NAME>XT R@ = IF NIP R> DROP EXIT THEN  @
    REPEAT DROP  CELL+ @
  REPEAT  R> DROP ;
: .NAME    ( xt -- )  >NAME ?DUP IF .ID ELSE ." ?" THEN ;
: UMAX     ( u1 u2 -- u )  2DUP U< IF SWAP THEN DROP ;
: BRANCHY? ( xt -- f )    \ words with an address in the next cell
  DUP ['] BRANCH = OVER ['] ?BRANCH = OR  OVER ['] (DO) = OR
  OVER ['] (?DO) = OR  OVER ['] (LOOP) = OR  SWAP ['] (+LOOP) = OR ;
: SEE-BODY ( a -- )       \ decompile until the last EXIT
  DUP >R
  BEGIN
    CR 2 SPACES DUP .HEX SPACE  DUP @  DUP .NAME SPACE
    DUP ['] LIT = IF DROP CELL+ DUP @ . ELSE
    DUP BRANCHY? IF DROP CELL+ DUP @ DUP .HEX  R> UMAX >R ELSE
    DUP ['] (S") = IF DROP CELL+ DUP COUNT 34 EMIT TYPE 34 EMIT  COUNT + ALIGNED CELL - ELSE
    ['] EXIT = IF DUP R@ U< 0= IF DROP R> DROP EXIT THEN THEN
    THEN THEN THEN
    CELL+
  AGAIN ;
>FORTH

: SEE  ( "name" -- )
  ' CR DUP .NAME SPACE  DUP @
  DUP 0= IF DROP ." colon" CELL+ SEE-BODY EXIT THEN
  DUP 1 = IF DROP ." create, data at " 2 CELLS + .HEX EXIT THEN
  DUP 2 = IF DROP ." constant " CELL+ @ . EXIT THEN
  DUP 3 = IF DROP ." does>, data at " DUP 2 CELLS + .HEX  ." , code:" CELL+ @ SEE-BODY EXIT THEN
  ." primitive " . DROP ;
: .S   ( -- )
  SP@ >R  ." <" S0 R@ - CELL / 0 .R ." > "
  S0 BEGIN CELL - DUP R@ U< 0= WHILE DUP @ . REPEAT DROP  R> DROP ;
: DUMP ( a u -- )
  OVER + SWAP
  BEGIN 2DUP U> WHILE
    CR DUP .HEX ." : "
    16 0 DO DUP I + C@ .H2 SPACE LOOP
    16 0 DO DUP I + C@ DUP 32 127 WITHIN 0= IF DROP 46 THEN EMIT LOOP
    16 +
  REPEAT 2DROP ;
: [DEFINED] ( "name" -- f )   \ true if the search order finds the name
  PARSE-NAME FIND-NAME 0<> ;  IMMEDIATE

\ ==== 6  Application, freezing, MARKER ===============================
\ 'SETUP runs once, then 'LOOP again and again (doc/vm.md, section 10).

VARIABLE 'SETUP   ' NOOP 'SETUP !
VARIABLE 'LOOP    ' NOOP 'LOOP !
: RUN     ( -- )  'SETUP @ EXECUTE  BEGIN 'LOOP @ EXECUTE AGAIN ;
: TURNKEY ( -- )  TRUE AUTORUN ! ;
: FREEZE  ( -- )  HERE FROZEN-END !  SAVE-IMAGE THROW ;
: EMPTY   ( -- )  FALSE REBOOT ;
: FACTORY ( -- )  ERASE-IMAGE  FALSE REBOOT ;
: COLD    ( -- )  TRUE REBOOT ;
: MARKER  ( "name" -- )   \ the name later forgets itself and all newer words
  XVP @  VP @  VOC-LINK @  CREATE  LATEST @ , , , ,
  VOC-LINK @ BEGIN ?DUP WHILE DUP , DUP @ , CELL+ @ REPEAT  0 ,
  DOES>
    DUP @ FROZEN-END @ U< ABORT" marker is frozen"
    DUP @ >R  CELL+ DUP @ VOC-LINK !  CELL+ DUP @ VP !  CELL+ DUP @ XVP !  CELL+
    BEGIN DUP @ ?DUP WHILE
      OVER CELL+ @  DUP R@ = IF DROP R@ @ THEN  SWAP !  2 CELLS +
    REPEAT DROP
    R> DP !  CURRENT @ @ LATEST !  ONLY DEFINITIONS ;

\ HIDE works on the chains of the wordlists (doc/vm.md 6). There is no
\ FORGET: MARKER forgets words, data and wordlists (as Forth-2012 advises).
>INTERNALS
: NAMED ( "name" -- nt )   PARSE-NAME 2DUP FIND-NAME ?DUP 0= IF UNDEFINED THEN NIP NIP ;
: UNLINK ( nt c -- f )   \ c holds the newest nt of a chain: take nt out of it
  BEGIN DUP @ WHILE
    2DUP @ = IF
      DUP S0 U< 0=  OVER FROZEN-END @ U< AND ABORT" the word before it is frozen"
      SWAP @ SWAP !  TRUE EXIT
    THEN  @
  REPEAT  2DROP FALSE ;
>FORTH
: HIDE ( "name" -- )   \ the search does not find the word any more
  NAMED  VOC-LINK @ BEGIN ?DUP WHILE  2DUP UNLINK IF 2DROP  CURRENT @ @ LATEST !  EXIT THEN  CELL+ @  REPEAT
  DROP ;

>INTERNALS
: BOOT ( -- )   \ the start: ONLY FORTH; with TURNKEY RUN after 1 s, then QUIT
  ONLY FORTH DEFINITIONS
  AUTORUN @ IF
    ." autostart in 1 s (Ctrl-C: prompt)" CR
    MILLIS BEGIN
      KEY? IF KEY DUP 27 = SWAP 3 = OR IF DROP QUIT THEN THEN
      MILLIS OVER - 1000 U< 0=
    UNTIL DROP
    ['] RUN CATCH ?DUP IF .ERROR THEN
  THEN  QUIT ;
: CASE-SENSITIVE   ( -- )  TRUE CASE-SENSE ! ;
: CASE-INSENSITIVE ( -- )  FALSE CASE-SENSE ! ;
>FORTH
