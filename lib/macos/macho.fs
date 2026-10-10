\ macho.fs - package MACHO: a program file for macOS (Apple silicon or Intel)
\
\ macOS starts a program from a Mach-O file. On Apple silicon it starts
\ only a program that dyld loads, at an address that changes with each
\ start ("position independent"), and with a code signature. The package
\ writes the smallest form that macOS takes (tested with macOS 26), for
\ the processor in CPU-TYPE: arm64 (the default), or x86-64 for a Mac
\ with Intel (MACHO-X86-64; on Apple silicon Rosetta runs it):
\
\   __PAGEZERO   4 GB without access, from address 0
\   __TEXT       the headers and the code, read and execute, at $1_0000_0000
\   __DATA       read and write: the image, then memory without file bytes
\   __LINKEDIT   the code signature
\   the load commands LC_LOAD_DYLINKER, LC_MAIN and LC_CODE_SIGNATURE
\
\ The signature is "ad hoc": no certificate, only a SHA-256 digest of
\ each page of 4 KB up to the signature (package SHA256). "codesign -v"
\ says "valid on disk".
\
\ Use, with the package IMAGE:
\   set TEXT-SIZE DATA-FILE DATA-SIZE CODE-OFF CODE-LEN ENTRY
\   MACHO-HDR ( -- a u )    the header and the load commands
\   SIGN-START              from now on, hash each byte that PUT writes
\   ... PUT the file up to SIG-OFFSET ...
\   SIGNATURE ( -- a u )    the code signature: the end of the file
\ The package needs cells of 32 bits, and the packages IMAGE and SHA256.

PACKAGE MACHO  ALSO IMAGE  ALSO SHA256   \ lib/package.fs

PUBLIC
VARIABLE TEXT-SIZE   \ header and code in file and memory; a multiple of 16 KB
VARIABLE DATA-FILE   \ the bytes of __DATA in the file; a multiple of 16 KB
VARIABLE DATA-SIZE   \ __DATA in memory; a multiple of 16 KB
VARIABLE CODE-OFF    \ the code in the file (the section __text)
VARIABLE CODE-LEN
VARIABLE ENTRY       \ the start code: its place in the file
VARIABLE CPU-TYPE    $0100000C CPU-TYPE !   \ arm64
VARIABLE CPU-SUB     0 CPU-SUB !            \ all arm64 processors
: MACHO-X86-64 ( -- )   $01000007 CPU-TYPE !  3 CPU-SUB ! ;   \ x86-64, all of them
: SIG-OFFSET ( -- n )   TEXT-SIZE @ DATA-FILE @ + ;

PRIVATE
$4000 CONSTANT PAGE      \ a page of macOS on Apple silicon
4096 CONSTANT CPAGE      \ a page of the code signature
64 CONSTANT MAX-PAGES
CREATE MH 512 ALLOT   VARIABLE MH#
CREATE HASHES MAX-PAGES 32 * ALLOT   VARIABLE NPAGES   VARIABLE PAGE-POS
CREATE SIG MAX-PAGES 32 * 128 + ALLOT   VARIABLE SIG#

: ROUND-PAGE ( n -- n' )   PAGE 1- +  PAGE NEGATE AND ;
: CPAGES ( -- n )   SIG-OFFSET CPAGE / ;
: CD-LEN ( -- n )   88 4 +  CPAGES 32 * + ;       \ the CodeDirectory
: SIG-LEN ( -- n )   20 CD-LEN + ;                \ the SuperBlob
: SIG-SIZE ( -- n )   SIG-LEN 15 + -16 AND ;

\ The header is little endian
: L, ( x n -- )   \ the n low bytes of x into MH, the lowest first
  0 ?DO  DUP MH MH# @ + C!  8 RSHIFT  1 MH# +!  LOOP  DROP ;
: W, ( x -- )   4 L, ;
: Q, ( lo hi -- )   SWAP W, W, ;                 \ 64 bits
: ADDR, ( x -- )   1 Q, ;                        \ the address $1_0000_0000 + x
: FIELD, ( a u n -- )   \ the text a u in a field of n bytes, the rest 0
  >R  MH MH# @ + R@ 0 FILL  MH MH# @ + SWAP MOVE  R> MH# +! ;
: NAME, ( a u -- )   16 FIELD, ;

: PAGEZERO, ( -- )
  $19 W, 72 W, S" __PAGEZERO" NAME,
  0 0 Q,  0 1 Q,  0 0 Q,  0 0 Q,  0 W, 0 W, 0 W, 0 W, ;
: TEXT, ( -- )   \ with the section __text
  $19 W, 152 W, S" __TEXT" NAME,
  0 ADDR,  TEXT-SIZE @ 0 Q,  0 0 Q,  TEXT-SIZE @ 0 Q,  5 W, 5 W, 1 W, 0 W,
  S" __text" NAME,  S" __TEXT" NAME,  CODE-OFF @ ADDR,  CODE-LEN @ 0 Q,
  CODE-OFF @ W,  2 W,  0 W,  0 W,  $80000400 W,  0 W, 0 W, 0 W, ;
: DATA, ( -- )
  $19 W, 72 W, S" __DATA" NAME,
  TEXT-SIZE @ ADDR,  DATA-SIZE @ 0 Q,  TEXT-SIZE @ 0 Q,  DATA-FILE @ 0 Q,
  3 W, 3 W, 0 W, 0 W, ;
: LINKEDIT, ( -- )
  $19 W, 72 W, S" __LINKEDIT" NAME,
  TEXT-SIZE @ DATA-SIZE @ + ADDR,  SIG-SIZE ROUND-PAGE 0 Q,  SIG-OFFSET 0 Q,  SIG-SIZE 0 Q,
  1 W, 1 W, 0 W, 0 W, ;
: DYLINKER, ( -- )   $E W, 32 W, 12 W,  S" /usr/lib/dyld" 20 FIELD, ;
: MAIN, ( -- )   $80000028 W, 24 W,  ENTRY @ 0 Q,  0 0 Q, ;
: CODESIG, ( -- )   $1D W, 16 W,  SIG-OFFSET W,  SIG-SIZE W, ;

\ The signature is big endian
: B, ( x -- )   \ 4 bytes into SIG, the most significant first
  4 0 DO  DUP 24 RSHIFT SIG SIG# @ + C!  8 LSHIFT  1 SIG# +!  LOOP  DROP ;

: SIGN-PUT ( a u -- )   \ hash the bytes, a digest for each page of 4 KB
  BEGIN DUP WHILE
    CPAGE PAGE-POS @ -  OVER MIN                ( a u n )
    >R  OVER R@ SHA-ADD  R@ PAGE-POS +!  SWAP R@ + SWAP R> -
    PAGE-POS @ CPAGE = IF
      NPAGES @ MAX-PAGES < 0= ABORT" the program is too big for the signature"
      SHA-END  HASHES NPAGES @ 32 * +  32 MOVE  1 NPAGES +!  0 PAGE-POS !  SHA-START
    THEN
  REPEAT  2DROP ;

PUBLIC
: MACHO-HDR ( -- a u )
  0 MH# !
  $FEEDFACF W,  CPU-TYPE @ W,  CPU-SUB @ W,  2 W,   \ 64 bit, the processor, an executable
  7 W,  440 W,  $00200085 W,  0 W,             \ 7 commands, 440 bytes; PIE, dyld
  PAGEZERO, TEXT, DATA, LINKEDIT, DYLINKER, MAIN, CODESIG,
  MH# @ 472 <> ABORT" the Mach-O header is not right"
  MH MH# @ ;

: SIGN-START ( -- )
  0 NPAGES !  0 PAGE-POS !  SHA-START  ['] SIGN-PUT IS ON-PUT ;

: SIGNATURE ( -- a u )
  ['] 2DROP IS ON-PUT
  PAGE-POS @ 0<>  NPAGES @ CPAGES <> OR ABORT" the file before the signature is not right"
  0 SIG#  !  SIG SIG-SIZE 0 FILL
  $FADE0CC0 B,  SIG-LEN B,  1 B,  0 B,  20 B,  \ SuperBlob: one blob at 20
  $FADE0C02 B,  CD-LEN B,  $20400 B,  $20002 B, \ CodeDirectory: ad hoc, linker-signed
  92 B,  88 B,  0 B,  CPAGES B,  SIG-OFFSET B,  \ hashes, name, slots, code limit
  $2002000C B,                                 \ hash: 32 bytes, SHA-256; platform 0; page 2^12
  0 B, 0 B, 0 B, 0 B,                          \ spare, scatter, team, spare
  0 B, 0 B,  0 B, 0 B,  0 B, TEXT-SIZE @ B,  0 B, 1 B,   \ exec segment: main program
  $75636600 B,                                 \ the name "ucf"
  HASHES SIG SIG# @ +  CPAGES 32 * MOVE
  SIG SIG-SIZE ;

END-PACKAGE
