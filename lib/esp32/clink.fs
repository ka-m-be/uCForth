\ clink.fs - package CLINK: C code of the ESP32 in CODE words
\
\ The code of a library of C, rewritten as CODE words for the assembler
\ XTENSA (blob/esp32: one package for each object of libphy, libpp and
\ librtc; lib/esp32/esp32/phy*.fs: the functions that the radio needs).
\ Each function of C is a CODE word: the bytes of its code are those of
\ the compiler, at the same offsets. This package gives them what the
\ assembler and CODE do not have:
\
\ The offsets of the original section. A CODE word holds one function
\ and the literals before it (L32R loads only from before the code), so
\ it starts with a jump over them; >CODE, the address of the function for
\ CALL8, CALLX8 and CALL-C, is this jump. L gives the address of an offset
\ of the section, so a jump needs no label:
\   $854 3 LITERALS         \ jump over 3 literals; they are at $854
\   ... T,  SYMBOL name     \ the 3 literals
\   $860 L:                 \ check: the next byte is at offset $860
\   A2 $885 L BEQZ,         \ beqz a2, .Lsec_135_885
\   A3 $854 L L32R,         \ l32r a3, .Lsec_135_854
\ A function without literals starts with ORIGIN: off ORIGIN.
\
\ The symbols. SYMBOL puts the address of a function or of data into a
\ literal, SYMBOL! into a cell of data, by its name. A name is looked for
\ in the package that loads (its PUBLIC and PRIVATE words), then in the
\ PUBLIC words of each package with EXPORTS, in each wordlist that EXPORT
\ gives, and in the names of EXTERN. If it is not there yet, the cell
\ waits for it: LINK-SYMBOLS at the end of each package, and EXTERN (the
\ address of a function outside: the ROM, ESP-IDF), fill the cells that
\ wait. .UNRESOLVED shows the names that cells still wait for, UNRESOLVED?
\ tells if there is one. FUNC gives the code of a function of the package
\ for CALL8, (it must be there already); L!+ puts the address of an
\ offset into a table of jumps.
\
\ Data: DATA: u bytes in the RAM data region (.data .bss COMMON), INIT
\ their start values, DATA-ALIGN for 8 bytes; tables that do not change
\ (.rodata) are made with CREATE, C, and ," in the dictionary. The code
\ keeps the chip addresses that the link gives. On the ESP32 the
\ dictionary can lie in the flash after FREEZE, at another chip address
\ (lib/esp32/esp32/bootimg.fs): so a package moves its tables to the RAM
\ data before the link (ESP32-PHY: TABLES>RAM in phy.fs).
\
\ A C function runs with the windowed calls of the Xtensa, as a function
\ of the ROM: CALL-C calls it with SYSCALL. A CODE word of C must not run
\ as a word of Forth (its name alone): its code is no primitive of the VM.
\ FPU-ON switches on the floating point coprocessor, which C code uses.
\   FPU-ON  6 0 2 ' RFChannelSel CALL-C DROP
\
\ The package is from blob/esp32 (README.md there), where
\ it links all 43 packages of the three libraries; here EXPORT is public,
\ so the data of a package that loaded before (ESP32-PHY) can be found.
\
\ Needs TEXT (lib/text.fs), XTENSA (asm/xtensa.fs), ROM and ASM
\ (lib/esp32/asm.fs); the ESP32 (CODE! writes a cell of the code or of
\ the data with !). On the board, in ucf term:
\   #require FAIL lib/text.fs
\   #require ADD, asm/xtensa.fs
\   #include lib/esp32/asm.fs
\   #include lib/esp32/clink.fs
\ On the host, lib/esp32/test/mock.fs stands for ROM and ASM.

PACKAGE CLINK  ALSO XTENSA  ALSO ROM  ALSO ASM   \ lib/package.fs

\ ==== The offsets of the original section ==============================

VARIABLE ORG0                       \ the address of offset 0 of the section
PUBLIC
: ORIGIN ( off -- )   THERE SWAP - ORG0 ! ;    \ the next byte has the offset off
: L ( off -- ta )   ORG0 @ + ;                 \ the address of the offset off
: L: ( off -- )   L THERE <> ABORT" the code is not at its offset" ;
: LITERALS ( off n -- )   \ jump over n literals at the start of a CODE word; the first is at off
  THERE 3 AND ABORT" LITERALS: the code does not start at a cell"
  CELLS THERE 4 + + J,  TALIGN  ORIGIN ;
: ZEROS, ( n -- )   0 ?DO 0 TC, LOOP ;         \ n bytes 0: the filler after a jump

\ ==== Symbols and the linker ===========================================
\ A cell that waits for a name has a record in the RAM data: the link,
\ the chain of its sites, the two wordlists of the package that wants
\ the name, the name as a counted string. A site: the link, the chip
\ address of the cell, the number to add.

PRIVATE
66 CONSTANT DOCODE                  \ the code field of a CODE word
VARIABLE PENDING                    \ the newest record; 0: none
VARIABLE EXPORTED                   \ the newest entry: the link, a wordlist
WORDLIST CONSTANT WL-EXTERN         \ the names of EXTERN
VARIABLE KEY-A  VARIABLE KEY-U  VARIABLE KEY-WP  VARIABLE KEY-WQ   \ the name in work

: KEY! ( a u wlp wlq -- )   KEY-WQ ! KEY-WP ! KEY-U ! KEY-A ! ;
: KEY@ ( -- a u wlp wlq )   KEY-A @ KEY-U @ KEY-WP @ KEY-WQ @ ;
: OWN ( -- wlp wlq )   \ the PRIVATE and the PUBLIC wordlist of the package that loads now
  CURRENT @  PRIVATE CURRENT @  PUBLIC CURRENT @  ROT CURRENT ! ;
: >ADDR ( xt -- a )   \ the chip address of a function or of data
  DUP @ DOCODE = IF >CODE EXIT THEN
  DUP @ 1 3 WITHIN 0= ABORT" a symbol is a CODE word, a constant or CREATE"
  EXECUTE >CHIP ;
\ The search: FIND-NAME (a primitive of the kernels) in a search order of
\ its own, with at most 16 wordlists at a time; then the old order again.
16 CELLS BUFFER: SAVED              \ the search order of the user
VARIABLE #SAVED
: SAVE-ORDER ( -- )   CONTEXT SAVED 16 CELLS MOVE  #ORDER @ #SAVED ! ;
: RESTORE-ORDER ( -- )   SAVED CONTEXT 16 CELLS MOVE  #SAVED @ #ORDER ! ;
: FIND2 ( a u wl1 wl2 -- nt | 0 )   \ the name in wl1, else in wl2
  SAVE-ORDER  CONTEXT !  CONTEXT CELL+ !  2 #ORDER !  FIND-NAME  RESTORE-ORDER ;
: EXPORTED-FIND ( a u -- nt | 0 )   \ the name in the wordlists of EXPORTS and EXTERN
  SAVE-ORDER  EXPORTED @
  BEGIN DUP WHILE                                  ( a u entry )
    0 #ORDER !
    BEGIN DUP #ORDER @ 16 < AND WHILE  DUP CELL+ @ CONTEXT #ORDER @ CELLS + !  1 #ORDER +!  @  REPEAT
    >R  2DUP FIND-NAME  R> SWAP  ?DUP IF  >R DROP 2DROP R>  RESTORE-ORDER EXIT THEN
  REPEAT  DROP 2DROP  RESTORE-ORDER 0 ;
: LOOKUP ( a u wlp wlq -- nt | 0 )   \ in the package of wlp wlq, else in EXPORTS
  >R >R  2DUP R> R> FIND2  ?DUP IF NIP NIP EXIT THEN  EXPORTED-FIND ;
: REC-NAME ( rec -- a u )   4 CELLS + COUNT ;
: KEY= ( rec -- f )   \ the record is for the name and the package of KEY
  DUP 2 CELLS + @ KEY-WP @ <> IF DROP FALSE EXIT THEN
  REC-NAME  DUP KEY-U @ <> IF 2DROP FALSE EXIT THEN  KEY-A @ SWAP SAME? ;
: FIND-RECORD ( -- rec | 0 )   PENDING @ BEGIN DUP WHILE DUP KEY= IF EXIT THEN @ REPEAT ;
: NEW-RECORD ( -- rec )   \ for KEY
  KEY-U @ 4 CELLS + 1+ DATA-ALLOT >R
  PENDING @ R@ !  0 R@ CELL+ !  KEY-WP @ R@ 2 CELLS + !  KEY-WQ @ R@ 3 CELLS + !
  KEY-U @ R@ 4 CELLS + C!  KEY-A @ R@ 4 CELLS + 1+ KEY-U @ MOVE
  R@ PENDING !  R> ;
: LATER ( n site -- )   \ the cell at the chip address site waits for KEY; n is added
  FIND-RECORD ?DUP 0= IF NEW-RECORD THEN           ( n site rec )
  3 CELLS DATA-ALLOT >R
  DUP CELL+ @ R@ !  R@ SWAP CELL+ !  R@ CELL+ !  R> 2 CELLS + ! ;
: REFER ( n site -- x )   \ the address of KEY plus n; 0 while it is not there (the site waits)
  KEY@ LOOKUP ?DUP IF  NIP NAME>XT >ADDR +  EXIT THEN  LATER  0 ;
: #SITES ( rec -- n )   0 SWAP CELL+ @ BEGIN ?DUP WHILE SWAP 1+ SWAP @ REPEAT ;
PUBLIC
: EXPORT ( wl -- )   \ the linker looks for names in wl too
  2 CELLS DATA-ALLOT  EXPORTED @ OVER !  TUCK CELL+ !  EXPORTED ! ;
DEFER CODE! ( x a -- )   \ write the cell at the chip address a: on the ESP32 a plain !
' ! IS CODE!

: EXPORTS ( -- )   \ the PUBLIC words of this package are symbols for the others
  PUBLIC  CURRENT @ EXPORT ;
PRIVATE
: PATCH ( a rec -- )   \ each site of the record gets a plus its number
  CELL+ @ BEGIN DUP WHILE
    >R  DUP R@ 2 CELLS + @ +  R@ CELL+ @ CODE!  R> @
  REPEAT  2DROP ;
PUBLIC
: LINK-SYMBOLS ( -- )   \ the cells that wait for a name that is there now get its address
  PENDING  BEGIN DUP @ WHILE
    DUP @ >R  R@ REC-NAME  R@ 2 CELLS + @  R@ 3 CELLS + @  LOOKUP
    ?DUP IF  NAME>XT >ADDR  R@ PATCH  R> @ OVER !
    ELSE  R> DROP  @  THEN
  REPEAT  DROP ;
: .UNRESOLVED ( -- )   \ the names that cells wait for, with the number of cells
  PENDING @ BEGIN ?DUP WHILE
    DUP REC-NAME TYPE  ." ("  DUP #SITES 0 .R  ." ) "  @
  REPEAT ;
: UNRESOLVED? ( -- f )   PENDING @ 0<> ;   \ true if a cell still waits for a name
: EXTERN ( a "name" -- )   \ name is the function (or data) at the chip address a
  CURRENT @ >R  WL-EXTERN CURRENT !  CONSTANT  R> CURRENT !  LINK-SYMBOLS ;

: SYMBOL+ ( n "name" -- )   \ a literal: the address of name plus n
  PARSE-NAME OWN KEY!  THERE REFER T, ;
: SYMBOL ( "name" -- )   0 SYMBOL+ ;
: SYMBOL! ( n a "name" -- )   \ the cell of data at a: the address of name plus n
  PARSE-NAME OWN KEY!  DUP >R >CHIP REFER  R> ! ;
: FUNC ( "name" -- ta )   \ the code of the function name of this package, which is there already
  PARSE-NAME  2DUP OWN FIND2  ?DUP 0= IF UNDEFINED THEN  NIP NIP NAME>XT
  DUP @ DOCODE <> ABORT" FUNC: not a CODE word"  >CODE ;
: L!+ ( a off -- a' )   \ the address of the offset off into the cell at a: a table of jumps
  L OVER !  CELL+ ;

\ ==== Data =============================================================

: DATA: ( u "name" -- )   \ u bytes in the RAM data region, 0 at the start; name gives them
  DUP DATA-ALLOT  DUP ROT 0 FILL  CONSTANT ;
: DATA-ALIGN ( n -- )   \ the next DATA: starts at a multiple of n (8)
  BEGIN VP @ OVER 1- AND WHILE  CELL DATA-ALLOT DROP  REPEAT  DROP ;
: INIT ( c1 .. cn n a -- )   \ the bytes c1 to cn to a, c1 first
  OVER + 1-  SWAP 0 ?DO  TUCK C!  1-  LOOP  DROP ;
: ," ( "text" -- )   \ the characters up to " into the dictionary, without a count
  34 PARSE  0 ?DO  COUNT C,  LOOP  DROP ;

\ ==== Calls ============================================================

: CALL-C ( x1 .. xn n xt -- x )   \ call the C function of a CODE word with n arguments
  >CODE SYSCALL ;
CODE FPU-ON ( -- )   \ the floating point coprocessor on (bit 0 of CPENABLE)
  A2 1 MOVI.N,  A2 CPENABLE WSR,  RSYNC,  NEXT,
END-CODE

PRIVATE
WL-EXTERN EXPORT

END-PACKAGE
