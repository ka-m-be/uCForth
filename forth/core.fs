\ core.fs - part 1 of the base vocabulary of uCForth: the interpreter
\
\ Genesis, the small outer interpreter of a bootstrap VM, compiles this
\ file (doc/genesis.md).
\ Genesis supplies the primitives, the host primitives PARSE-NAME PARSE
\ HEADER REVEAL IMMEDIATE , C, [ ] and constants for the system
\ variables. This file first defines Forth versions of the host
\ primitives, so that no other word uses a host primitive. At the end
\ the Forth interpreter QUIT is complete. Genesis then removes the host
\ primitives, and QUIT compiles forth/base.fs.
\
\ Stack comments: ( before -- after ). Flags are -1 (true) or 0.
\ "?:" defines a word only if the VM does not supply it already.
\ The Forth version is then the reference for the meaning of the word.

\ ==== 1  ':' and ';' by hand =========================================
\ Line 1 makes the header ':', sets its code field to DOCOL (0) and
\ compiles its body. Line 2 makes ';' in the same way. Section 16
\ replaces both with Forth versions.

PARSE-NAME : HEADER  0 ,  ]  PARSE-NAME HEADER  0 ,  ]  EXIT  [  REVEAL
PARSE-NAME ; HEADER  0 ,  ]  LIT EXIT ,  REVEAL  0 STATE !  EXIT  [  REVEAL IMMEDIATE

\ ==== 2  Places for new words, simple accelerators ====================

WL-INTERNALS CURRENT !
: >INTERNALS ( -- )  WL-INTERNALS CURRENT ! ;   \ next words go to INTERNALS
: >FORTH  ( -- )  WL-FORTH CURRENT ! ;       \ next words go to FORTH
: NOOP    ( -- ) ;
>FORTH

?: +       ( n1 n2 -- n3 )           UM+ DROP ;
?: INVERT  ( x -- x' )               -1 XOR ;
?: NEGATE  ( n -- -n )               INVERT 1 + ;
?: -       ( n1 n2 -- n3 )           NEGATE + ;
?: 1+      ( n -- n+1 )              1 + ;
?: 1-      ( n -- n-1 )              1 - ;
?: 2*      ( x -- 2x )               DUP + ;
?: NIP     ( x1 x2 -- x2 )           SWAP DROP ;
?: ROT     ( x1 x2 x3 -- x2 x3 x1 )  >R SWAP R> SWAP ;
?: 2DUP    ( x1 x2 -- x1 x2 x1 x2 )  OVER OVER ;
?: 2DROP   ( x1 x2 -- )              DROP DROP ;
?: COUNT   ( a -- a+1 c )            DUP 1+ SWAP C@ ;
?: +!      ( n a -- )                SWAP OVER @ + SWAP ! ;

: CELL+    ( a -- a' )    CELL + ;
: HERE     ( -- a )       DP @ ;

\ ==== 3  The compiler: , C, [ ] IMMEDIATE ============================
\ Forth versions of host primitives. From here on no word uses a host
\ primitive in its body.

: ,  ( x -- )   HERE !  CELL DP +! ;
: C, ( c -- )   HERE C!  1 DP +! ;
: [  ( -- )     0 STATE ! ;  IMMEDIATE
: ]  ( -- )     -1 STATE ! ;

>INTERNALS
: FLAG!        ( mask -- )  LATEST @ CELL+ DUP C@ ROT OR SWAP C! ;
>FORTH
: IMMEDIATE    ( -- )  128 FLAG! ;
>INTERNALS
: COMPILE-ONLY ( -- )  64 FLAG! ;   \ the prompt refuses the word
: UNCHECKED    ( -- )  32 FLAG! ;   \ no return stack frame check
>FORTH

\ ==== 4  Control structures and comments ==============================
\ Branches carry an absolute target address in the next cell.

: IF      ( -- a )       LIT ?BRANCH ,  HERE 0 , ;                 IMMEDIATE COMPILE-ONLY
: THEN    ( a -- )       HERE SWAP ! ;                              IMMEDIATE COMPILE-ONLY
: ELSE    ( a1 -- a2 )   LIT BRANCH ,  HERE 0 ,  SWAP  HERE SWAP ! ; IMMEDIATE COMPILE-ONLY
: BEGIN   ( -- a )       HERE ;                                     IMMEDIATE COMPILE-ONLY
: UNTIL   ( a -- )       LIT ?BRANCH ,  , ;                         IMMEDIATE COMPILE-ONLY
: AGAIN   ( a -- )       LIT BRANCH ,  , ;                          IMMEDIATE COMPILE-ONLY
: WHILE   ( a -- a2 a )  LIT ?BRANCH ,  HERE 0 ,  SWAP ;            IMMEDIATE COMPILE-ONLY
: REPEAT  ( a2 a -- )    LIT BRANCH ,  ,  HERE SWAP ! ;             IMMEDIATE COMPILE-ONLY
: \       ( -- )         #TIB @ >IN ! ;                             IMMEDIATE

\ ==== 5  Comparison and arithmetic ====================================

?: ?DUP    ( x -- 0 | x x )  DUP IF DUP THEN ;
?: 0=      ( x -- f )        IF 0 ELSE -1 THEN ;
?: =       ( x1 x2 -- f )    XOR 0= ;
?: U<      ( u1 u2 -- f )    2DUP XOR 0< IF NIP 0< EXIT THEN  - 0< ;
?: <       ( n1 n2 -- f )    2DUP XOR 0< IF DROP 0< EXIT THEN  - 0< ;

: CELLS   ( n -- n' )           2*  CELL 4 = IF 2* THEN ;
: 0<>     ( x -- f )            0= 0= ;
: <>      ( x1 x2 -- f )        = 0= ;
: >       ( n1 n2 -- f )        SWAP < ;
: U>      ( u1 u2 -- f )        SWAP U< ;
: WITHIN  ( n lo hi -- f )      OVER - >R - R> U< ;
: MIN     ( n1 n2 -- n )        2DUP > IF SWAP THEN DROP ;
: MAX     ( n1 n2 -- n )        2DUP < IF SWAP THEN DROP ;
: ABS     ( n -- u )            DUP 0< IF NEGATE THEN ;
: TUCK    ( x1 x2 -- x2 x1 x2 ) SWAP OVER ;

\ Multiplication and division after eForth 1.0 (UM+ as the only
\ arithmetic). The loops run once for each bit of a cell.
?: UM*    ( u1 u2 -- ud )
  0 SWAP  CELL 2* 2* 2* >R
  BEGIN
    DUP UM+ >R >R  DUP UM+ R> + R>
    IF >R OVER UM+ R> + THEN
    R> 1- DUP >R 0=
  UNTIL  R> DROP  ROT DROP ;
?: UM/MOD ( ud u -- urem uquot )     \ -1 -1 for an overflow or u = 0
  2DUP U< IF
    NEGATE  CELL 2* 2* 2* >R
    BEGIN
      >R DUP UM+ >R >R DUP UM+ R> + DUP
      R> R@ SWAP >R UM+ R> OR
      IF >R DROP 1+ R> ELSE DROP THEN  R>
      R> 1- DUP >R 0=
    UNTIL  R> DROP
    DROP SWAP EXIT
  THEN  DROP 2DROP -1 DUP ;
?: *      ( n1 n2 -- n3 )   UM* DROP ;
?: LSHIFT ( x u -- x' )     BEGIN DUP WHILE >R 2* R> 1- REPEAT DROP ;
?: RSHIFT ( x u -- x' )     BEGIN DUP WHILE >R 0 2 UM/MOD NIP R> 1- REPEAT DROP ;
?: 2/     ( n -- n' )       DUP 1 RSHIFT  SWAP 0< IF -1 1 RSHIFT INVERT OR THEN ;
?: FILL   ( a u c -- )
  SWAP >R SWAP  BEGIN R@ WHILE  2DUP C! 1+  R> 1- >R  REPEAT  2DROP R> DROP ;
?: MOVE   ( a1 a2 u -- )    \ correct also for overlapping areas
  >R  2DUP U< IF
    R@ + SWAP R@ + SWAP
    BEGIN R@ WHILE  1- SWAP 1- SWAP  OVER C@ OVER C!  R> 1- >R  REPEAT
  ELSE
    BEGIN R@ WHILE  OVER C@ OVER C!  1+ SWAP 1+ SWAP  R> 1- >R  REPEAT
  THEN  2DROP R> DROP ;

\ ==== 6  CATCH and THROW (eForth), UNUSED, ALLOT ======================

: CATCH  ( xt -- 0 | n )
  SP@ >R  HANDLER @ >R  RP@ HANDLER !
  EXECUTE
  R> HANDLER !  R> DROP  0 ;
: THROW  ( n -- )
  ?DUP IF  HANDLER @ RP!  R> HANDLER !  R> SWAP >R  SP! DROP  R>  THEN ;
: ABORT  ( -- )  -1 THROW ;
\ The temp store: the headers of the PRIVATE words of a package, from
\ DICT-END down, while the package loads (lib/package.fs, doc/vm.md 6).
>INTERNALS
PARSE-NAME TEMP-START HEADER  2 ,  VP @ ,  CELL VP +!  REVEAL   \ its lowest byte; DICT-END: empty
DICT-END TEMP-START !
: TEMP?  ( a -- f )   TEMP-START @ DICT-END WITHIN ;   \ a lies in the temp store
: LIMIT  ( -- a )     TEMP-START @ ;   \ the end of the free dictionary
>FORTH
: UNUSED ( -- u )    LIMIT HERE - ;   \ the free bytes of the dictionary
: ALLOT  ( n -- )    \ -8 THROW if the dictionary is full
  DUP 0< 0= IF  DUP UNUSED U> IF -8 THROW THEN  THEN  DP +! ;

\ ==== 7  Names ========================================================

>INTERNALS
: UPC      ( c -- c' )    DUP 97 123 WITHIN IF 32 - THEN ;
: SAME?    ( a1 a2 u -- f )   \ compare names, case rule from CASE-SENSE
  BEGIN DUP WHILE
    >R  OVER C@ OVER C@  CASE-SENSE @ 0= IF UPC SWAP UPC THEN
    <> IF R> DROP 2DROP 0 EXIT THEN
    1+ SWAP 1+ SWAP  R> 1-
  REPEAT  DROP 2DROP -1 ;
: WL-FIND  ( a u wl -- a u nt | a u 0 )
  @ BEGIN DUP WHILE
    >R  R@ CELL+ C@ 31 AND  OVER = IF
      2DUP  R@ CELL+ 1+ SWAP  SAME? IF R> EXIT THEN
    THEN  R> @
  REPEAT ;
?: FIND-NAME ( a u -- nt | 0 )
  #ORDER @ BEGIN DUP WHILE 1-
    >R  R@ CELLS CONTEXT + @ WL-FIND
    ?DUP IF R> DROP NIP NIP EXIT THEN  R>
  REPEAT  DROP 2DROP 0 ;
: ALIGNED       ( a -- a' )   CELL 1- +  CELL NEGATE AND ;
: ALIGN         ( -- )        BEGIN HERE CELL 1- AND WHILE 0 C, REPEAT ;
: NAME>XT       ( nt -- xt )  CELL+ COUNT 31 AND + ALIGNED  DUP TEMP? IF @ THEN ;
: IMMEDIATE?    ( nt -- f )   CELL+ C@ 128 AND 0<> ;
: COMPILE-ONLY? ( nt -- f )   CELL+ C@ 64 AND 0<> ;
: NAME-THROW    ( a u n -- )  >R  ERRMSG CELL+ !  ERRMSG !  R> THROW ;
: UNDEFINED     ( a u -- )    -13 NAME-THROW ;
>FORTH

\ ==== 8  Terminal output ==============================================
\ Three variables by hand, because VARIABLE needs HEADER and HEADER
\ needs these. Each line does what VARIABLE does: a header, the code
\ field DOCON (2) and the address of a new cell in the RAM data region.

>INTERNALS
PARSE-NAME 'EMIT   HEADER  2 ,  VP @ ,  CELL VP +!  REVEAL   \ output vector
PARSE-NAME OUT     HEADER  2 ,  VP @ ,  CELL VP +!  REVEAL   \ output column
PARSE-NAME DISCARD HEADER  2 ,  VP @ ,  CELL VP +!  REVEAL   \ ";" drops the word (?:)
PARSE-NAME TX! FIND-NAME NAME>XT  'EMIT !
>FORTH

: EMIT   ( c -- )    DUP 13 = OVER 10 = OR IF 0 OUT ! ELSE 1 OUT +! THEN  'EMIT @ EXECUTE ;
: TYPE   ( a u -- )  BEGIN DUP WHILE >R COUNT EMIT R> 1- REPEAT 2DROP ;
: CR     ( -- )      13 EMIT 10 EMIT ;
: SPACE  ( -- )      32 EMIT ;
: SPACES ( n -- )    BEGIN DUP 0 > WHILE SPACE 1- REPEAT DROP ;

\ ==== 9  Parser and strings ===========================================
\ The input line is TIB with the length #TIB. >IN is the parse position.

>INTERNALS
: SOURCE     ( -- a u )   TIB #TIB @ ;
: PARSE      ( c -- a u )    \ the text up to c; >IN moves past c
  >R  TIB >IN @ +  #TIB @ >IN @ - 0 MAX  2DUP
  BEGIN DUP IF OVER C@ R@ <> ELSE 0 THEN WHILE  1- SWAP 1+ SWAP  REPEAT
  R> DROP  NIP  DUP 0<> NEGATE >R  -  DUP R> + >IN +! ;
: PARSE-NAME ( -- a u )      \ the next word; blanks before it are skipped
  TIB >IN @ +  #TIB @ >IN @ - 0 MAX
  BEGIN DUP IF OVER C@ 33 < ELSE 0 THEN WHILE  1- SWAP 1+ SWAP  REPEAT
  2DUP BEGIN DUP IF OVER C@ 32 > ELSE 0 THEN WHILE  1- SWAP 1+ SWAP  REPEAT
  NIP  DUP 0<> NEGATE >R  -  2DUP + TIB -  R> +  >IN ! ;
>FORTH
: (      ( "text" -- )    41 PARSE 2DROP ;  IMMEDIATE

>INTERNALS
: (S")   ( -- a u )   R> COUNT 2DUP + ALIGNED >R ;
: S,     ( a u -- )   DUP C,  BEGIN DUP WHILE >R COUNT C, R> 1- REPEAT 2DROP  ALIGN ;
>FORTH
: S"     ( "text" -- a u )
  34 PARSE  STATE @ IF LIT (S") , S, EXIT THEN
  80 MIN >R  PAD 128 + R@ MOVE  PAD 128 + R> ;  IMMEDIATE
: ."     ( "text" -- )
  34 PARSE  STATE @ IF LIT (S") , S,  LIT TYPE , EXIT THEN  TYPE ;  IMMEDIATE
: .(     ( "text" -- )    41 PARSE TYPE ;  IMMEDIATE

\ ==== 10  Headers ======================================================
\ A header is: link, count byte with flags, name, filler to the next
\ cell. The code field follows (doc/vm.md, section 6). A temp header
\ (a PRIVATE word of a package) lies in the temp store and has the xt
\ in the cell after the name instead; END-PACKAGE gives it back.

>INTERNALS
: REVEALED? ( -- f )   \ true if LATEST heads a wordlist
  LATEST @  VOC-LINK @  BEGIN DUP WHILE
    2DUP @ = IF 2DROP -1 EXIT THEN  CELL+ @
  REPEAT  2DROP 0 ;
: TEMP-SIZE ( u -- n )   1+ ALIGNED 2 CELLS + ;   \ a temp header: link, count byte, name, xt
: FORGET-UNFINISHED ( -- )   \ give back the space of a header that is not visible
  REVEALED? 0=  LATEST @ FROZEN-END @ U< 0= AND IF
    LATEST @ TEMP? IF  LATEST @ NAME>XT DP !  LATEST @ DUP CELL+ C@ 31 AND TEMP-SIZE + TEMP-START !
    ELSE  LATEST @ DP !  THEN
    CURRENT @ @ LATEST !
  THEN ;
: TEMP-HEADER ( a u -- )   \ the header in the temp store; the code follows at HERE
  DUP TEMP-SIZE  DUP HERE + 32 + TEMP-START @ U> IF -8 THROW THEN
  TEMP-START @ SWAP -  DUP TEMP-START !  DUP LATEST !      ( a u nt )
  CURRENT @ @ OVER !  2DUP CELL+ C!                        \ the link, the count byte
  CELL+ 1+ SWAP  2DUP + ALIGNED HERE SWAP !  MOVE ;        \ the xt, the name
: HEADER ( a u -- )   \ a new header in CURRENT, not yet visible
  DUP 0= IF -16 THROW THEN  DUP 31 > IF -19 THROW THEN
  FORGET-UNFINISHED
  DISCARD @ 0= IF
    2DUP CURRENT @ WL-FIND NIP NIP IF
      OUT @ IF CR THEN  ."   redefined " 2DUP TYPE CR
    THEN
  THEN
  ALIGN  CURRENT @ TEMP? IF TEMP-HEADER EXIT THEN   \ a wordlist in the temp store: PRIVATE
  DUP HERE + 32 + LIMIT U> IF -8 THROW THEN
  HERE >R  CURRENT @ @ ,  DUP C,  HERE SWAP DUP ALLOT MOVE  ALIGN  R> LATEST ! ;
: REVEAL ( -- )   \ make the header LATEST visible
  LATEST @ ?DUP IF CURRENT @ ! THEN ;
>FORTH

\ ==== 11  Defining words and the compiler ==============================
\ Data that changes lives in the RAM data region (VP): VARIABLE,
\ BUFFER:, later VALUE and DEFER. CREATE makes tables in the
\ dictionary. After FREEZE these tables can only be read (doc/vm.md).
\ A target can give the RAM data an extension, for example the PSRAM of
\ a board: XDATA-END is its end (0: none), XVP its next free byte. Then
\ VALLOT takes the extension; DATA-ALLOT always takes the RAM data region.

: CONSTANT ( x "name" -- )   PARSE-NAME HEADER  2 , ,  REVEAL ;
-1 CONSTANT TRUE
 0 CONSTANT FALSE
>INTERNALS
PARSE-NAME XVP       HEADER  2 ,  VP @ ,  CELL VP +!  REVEAL   \ the next free byte of the extension
PARSE-NAME XDATA-END HEADER  2 ,  VP @ ,  CELL VP +!  REVEAL   \ its end; 0: no extension
: DATA-ALLOT ( n -- a )  \ reserve n bytes in the RAM data region; -8 if it is full
  DUP DATA-END VP @ - U> IF -8 THROW THEN  ALIGNED  VP @ SWAP VP +! ;
: VALLOT   ( n -- a )    \ n bytes in the extension, if there is one, else in the RAM data region
  XDATA-END @ 0=  XVP @ 0= OR IF DATA-ALLOT EXIT THEN
  DUP XDATA-END @ XVP @ - U> IF -8 THROW THEN  ALIGNED  XVP @ SWAP XVP +! ;
>FORTH
: CREATE   ( "name" -- )    PARSE-NAME HEADER  1 , 0 ,  REVEAL ;
: VARIABLE ( "name" -- )    CELL VALLOT  0 OVER !  CONSTANT ;
: BUFFER:  ( u "name" -- )  VALLOT CONSTANT ;

: '        ( "name" -- xt )
  PARSE-NAME 2DUP FIND-NAME ?DUP IF NIP NIP NAME>XT EXIT THEN  UNDEFINED ;
: [']      ( "name" -- )    ' LIT LIT , , ;                 IMMEDIATE COMPILE-ONLY
: LITERAL  ( x -- )         LIT LIT , , ;                   IMMEDIATE COMPILE-ONLY
: POSTPONE ( "name" -- )
  PARSE-NAME 2DUP FIND-NAME ?DUP 0= IF UNDEFINED THEN  NIP NIP
  DUP NAME>XT SWAP IMMEDIATE? IF , EXIT THEN
  LIT LIT , ,  LIT , , ;                                    IMMEDIATE COMPILE-ONLY
: RECURSE  ( -- )           LATEST @ NAME>XT , ;            IMMEDIATE COMPILE-ONLY

>INTERNALS
: (ABORT") ( f a u -- )     ROT IF ERRMSG CELL+ !  ERRMSG !  -2 THROW THEN 2DROP ;
>FORTH
: ABORT"   ( "text" -- )    POSTPONE S"  LIT (ABORT") , ;   IMMEDIATE COMPILE-ONLY

\ ==== 12  Terminal input ===============================================

>INTERNALS
VARIABLE 'KEY?   ' ?RX 'KEY? !     \ input vector ( -- c -1 | 0 )
VARIABLE KEYBUF  -1 KEYBUF !       \ one character that KEY? has read
VARIABLE ECHO                      \ not 0: ACCEPT echoes the input
VARIABLE LAST-CR                   \ the last line ended with CR
>FORTH

: KEY?   ( -- f )
  KEYBUF @ 0< 0= IF TRUE EXIT THEN
  'KEY? @ EXECUTE IF KEYBUF ! TRUE EXIT THEN  FALSE ;
: KEY    ( -- c )    BEGIN KEY? UNTIL  KEYBUF @  -1 KEYBUF ! ;
: ACCEPT ( a u -- u' )   \ read a line; backspace works; CR LF is one line end
  OVER + >R  DUP                                 ( a p ) ( R: end )
  KEY  DUP 10 = LAST-CR @ AND IF DROP KEY THEN
  BEGIN  DUP 13 <> OVER 10 <> AND  WHILE
    DUP 8 = OVER 127 = OR IF
      DROP  2DUP U< IF 1- ECHO @ IF 8 EMIT SPACE 8 EMIT THEN THEN
    ELSE
      OVER R@ U< IF ECHO @ IF DUP EMIT THEN  OVER C! 1+ ELSE DROP THEN
    THEN  KEY
  REPEAT  13 = LAST-CR !  ECHO @ IF SPACE THEN  R> DROP  SWAP - ;

\ ==== 13  Number output ================================================

: HEX     ( -- )   16 BASE ! ;
: DECIMAL ( -- )   10 BASE ! ;
>INTERNALS
VARIABLE HLD                       \ number output: next free place
: DIGIT    ( u -- c )   DUP 9 > IF 7 + THEN 48 + ;
: HOLD-END ( -- a )     PAD 256 + ;
>FORTH
: <#    ( -- )             HOLD-END HLD ! ;
: HOLD  ( c -- )           -1 HLD +!  HLD @ C! ;
: #     ( ud -- ud' )      0 BASE @ UM/MOD >R  BASE @ UM/MOD  SWAP DIGIT HOLD  R> ;
: #S    ( ud -- 0 0 )      BEGIN # 2DUP OR 0= UNTIL ;
: SIGN  ( n -- )           0< IF 45 HOLD THEN ;
: #>    ( ud -- a u )      2DROP  HLD @  HOLD-END OVER - ;
: .     ( n -- )           DUP ABS 0 <# #S ROT SIGN #> TYPE SPACE ;
: U.    ( u -- )           0 <# #S #> TYPE SPACE ;
: .R    ( n w -- )         >R DUP ABS 0 <# #S ROT SIGN #> R> OVER - SPACES TYPE ;

\ ==== 14  Number input =================================================
\ NUMBER? reads BASE, the prefixes $ # %, 'c' and a minus before or
\ after the prefix, as in the handbook. The number must fit a cell.

>INTERNALS
: DIGIT?  ( c base -- u f )   \ the value of c as a digit; f true if it is one
  >R  UPC  DUP 58 < IF 48 - ELSE DUP 65 < IF DROP -1 ELSE 55 - THEN THEN  DUP R> U< ;
: PREFIX  ( c -- base | 0 )   \ the base for the prefix $ # %, else 0
  DUP 36 = 16 AND  OVER 35 = 10 AND OR  SWAP 37 = 2 AND OR ;
: DIGITS  ( a u base -- n 1 | 0 0 | 0 -1 )   \ 1: a number, 0: no digit, -1: too big
  >R  0 ROT ROT  DUP 0= IF 2DROP R> 2DROP 0 0 EXIT THEN
  BEGIN DUP WHILE
    OVER C@ R@ DIGIT? 0= IF 2DROP 2DROP R> DROP 0 0 EXIT THEN
    >R ROT R> SWAP  R@ UM*  IF 2DROP 2DROP R> DROP 0 -1 EXIT THEN
    UM+ IF 2DROP DROP R> DROP 0 -1 EXIT THEN
    ROT ROT  1- SWAP 1+ SWAP
  REPEAT  2DROP  R> DROP  1 ;
: NUMBER? ( a u -- n true | false )   \ -24 THROW if the number does not fit
  OVER C@ 39 =  OVER 3 = AND IF  OVER 2 + C@ 39 = IF DROP 1+ C@ TRUE EXIT THEN  THEN
  2DUP  0 >R                                         ( a u a u ) ( R: minus )
  OVER C@ 45 =  OVER 1 > AND IF
    OVER 1+ C@ PREFIX IF  1- SWAP 1+ SWAP  R> INVERT >R  THEN
  THEN
  DUP IF OVER C@ PREFIX ELSE 0 THEN
  ?DUP IF >R 1- SWAP 1+ SWAP R> ELSE BASE @ THEN     ( a u a' u' base )
  >R  DUP IF OVER C@ 45 = ELSE 0 THEN
  IF  1- SWAP 1+ SWAP  R> R> INVERT >R >R  THEN  R>
  DIGITS                                             ( a u n status )
  DUP 0= IF 2DROP 2DROP R> DROP FALSE EXIT THEN
  0< IF DROP R> DROP -24 NAME-THROW THEN
  R> IF  DUP 0< IF DUP NEGATE 0< 0= IF DROP -24 NAME-THROW THEN THEN  NEGATE  THEN
  NIP NIP TRUE ;
>FORTH

\ ==== 15  Error messages ===============================================

>INTERNALS
CREATE MESSAGES   \ THROW code and counted string; 0 ends the table
  -3 , S" data stack overflow" S,      -4 , S" data stack underflow" S,
  -8 , S" dictionary overflow" S,      -9 , S" invalid memory address" S,
 -10 , S" division by zero" S,        -13 , S" undefined word" S,
 -14 , S" compile-only word" S,       -16 , S" missing name" S,
 -19 , S" name too long" S,           -22 , S" control structure mismatch" S,
 -24 , S" number out of range" S,     -28 , S" user interrupt" S,
 -37 , S" file I/O error" S,          -38 , S" non-existent file" S,
 -49 , S" search order overflow" S,
   0 ,
: MESSAGE ( n -- a u )   \ the text for a THROW code; u = 0 if there is none
  MESSAGES BEGIN DUP @ WHILE
    2DUP @ = IF NIP CELL+ COUNT EXIT THEN  CELL+ COUNT + ALIGNED
  REPEAT  2DROP 0 0 ;
: .ERROR ( n -- )   \ show the message for a THROW code; -1 and -56 stay silent
  DUP -1 = OVER -56 = OR IF DROP EXIT THEN
  OUT @ IF CR THEN  ."   error: "
  DUP -2 = IF  DROP ERRMSG @ ERRMSG CELL+ @ TYPE  ELSE
    DUP MESSAGE ?DUP IF TYPE ELSE DROP ." error " DUP 0 .R THEN
    DUP -9 = OVER -13 = OR OVER -14 = OR SWAP -24 = OR  ERRMSG CELL+ @ AND IF
      ." : " ERRMSG @ ERRMSG CELL+ @ TYPE
    THEN
  THEN  CR  0 ERRMSG CELL+ ! ;
>FORTH

\ ==== 16  ':' and ';' in Forth, ?: ====================================
\ From here on Forth builds the definitions. Genesis removes the ':'
\ and ';' of section 1 when this file is complete.

: ;  ( -- )  LIT EXIT ,  HERE LIMIT U> IF -8 THROW THEN   \ the code ran into the temp store or past DICT-END
  DISCARD @ IF 0 DISCARD ! ELSE REVEAL THEN  0 STATE ! ;  IMMEDIATE COMPILE-ONLY
: :  ( "name" -- )  PARSE-NAME HEADER  0 ,  ] ;
>INTERNALS
: ?: ( "name" -- )   \ like ':', but ';' drops the word if name is known
  >IN @  PARSE-NAME FIND-NAME 0<> DISCARD !  >IN !  : ;
>FORTH

\ ==== 17  The interpreter ==============================================
\ The console protocol is in doc/vm.md, section 9: the prompt stands at
\ the start of a line, and the terminal sends a line only after it.

>INTERNALS
: ?STACK    ( -- )   SP@ S0 U> IF -4 THROW THEN ;   \ like STACK? in CollapseOS
: INTERPRET ( -- )   \ execute or compile each word of the input line
  BEGIN  PARSE-NAME  DUP WHILE
    2DUP FIND-NAME ?DUP IF
      STATE @ IF
        DUP IMMEDIATE? IF NIP NIP NAME>XT EXECUTE ELSE NIP NIP NAME>XT , THEN
      ELSE
        DUP COMPILE-ONLY? IF DROP -14 NAME-THROW THEN  NIP NIP NAME>XT EXECUTE
      THEN
    ELSE
      2DUP NUMBER? 0= IF UNDEFINED THEN  NIP NIP  STATE @ IF LIT LIT , , THEN
    THEN
    ?STACK
  REPEAT  2DROP ;
: PROMPT    ( -- )   OUT @ IF CR THEN  STATE @ IF ." c> " ELSE ." ok> " THEN ;
: REFILL    ( -- f )   \ the next line from the console into TIB
  TIB 256 ACCEPT #TIB !  0 >IN !  ECHO @ 0= IF 0 OUT ! THEN  TRUE ;
>FORTH
: QUIT ( -- )   \ empty the return stack, then interpret lines from the console
  R0 RP!  0 HANDLER !  0 STATE !  0 DISCARD !  FORGET-UNFINISHED
  BEGIN  PROMPT REFILL DROP  ['] INTERPRET CATCH  ?DUP UNTIL
  .ERROR  S0 SP!  RECURSE ;
