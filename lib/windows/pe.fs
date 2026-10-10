\ pe.fs - package PE: a program file for Windows (PE32) or UEFI (PE32+)
\
\ Windows starts a program from a PE file (Portable Executable, the form
\ with 32 bits): each program file of Windows (.exe, .dll) has this
\ format. UEFI starts an application from a PE file too, in the form
\ with 64 bits (PE32+, EFI-START). This package makes its headers and
\ its import section, the minimum that a program for the console needs:
\
\   offset 0     the header of DOS ("MZ") and a small DOS program: on DOS
\                the file says "This program needs Windows."
\   $80          "PE", the file header (COFF, 20 bytes), the optional
\                header (224 bytes), and a section header (40 bytes) for
\                each section
\   $200 ...     the sections, each at a multiple of 512 bytes
\
\ Windows maps each section to IMAGE-BASE + its RVA (relative virtual
\ address, a multiple of 4 KB). These are virtual addresses: each program
\ has addresses of its own, and Windows maps their pages to the physical
\ memory. The file has no relocations (no list of the places in the code
\ that hold an address), so Windows loads it at IMAGE-BASE or not at all,
\ and the code may use fixed addresses. IMAGE-BASE is $10000000, not the
\ usual $400000: that place must be free, and in Wine under an emulator
\ (QEMU) the loader of Wine itself lies at $400000.
\
\   entry PE-START           a new header; entry: the RVA of the first instruction
\   entry EFI-START          the same for UEFI on x86-64: PE32+, without imports.
\                            UEFI loads the program at an address of its own
\                            choice; the file has no relocations, so the code
\                            must not depend on its address (kernels/x64.fs).
\   a u rva vsize offset size flags PE-SECTION
\                            a section: name a u (at most 8 characters),
\                            vsize bytes in memory at the RVA, size bytes
\                            from the offset in the file (the rest is 0)
\   CODE-SECTION DATA-SECTION   flags: code (read, execute), data (read, write)
\   PE-HDR PE-HDR-SIZE       the headers (512 bytes), to write first
\
\ Imports: a program calls Windows through the functions of a DLL, for
\ example WriteFile of KERNEL32.dll. The import section names them;
\ Windows writes the address of each function into its cell of the IAT
\ (import address table) before the start. A call is "call [cell]".
\
\   rva IMPORTS-AT           the import section starts at this RVA
\   DLL name                 the functions after it are in this DLL
\   IMPORT name              a function of the DLL: the constant name is
\                            the address of its cell in the IAT
\   IMPORT-DATA ( -- a u )   the bytes of the import section, after all
\                            DLL and IMPORT
\
\ The package needs cells of 32 bits. The fields are described in the
\ "PE Format" of Microsoft (learn.microsoft.com, Windows, PE Format).

PACKAGE PE   \ lib/package.fs

PUBLIC
$10000000 CONSTANT IMAGE-BASE
512 CONSTANT PE-HDR-SIZE
CREATE PE-HDR  PE-HDR-SIZE ALLOT
$60000020 CONSTANT CODE-SECTION     \ code, read, execute
$C0000040 CONSTANT DATA-SECTION     \ initialized data, read, write

PRIVATE
$80 CONSTANT PE-OFF                 \ "PE", after the DOS part
$98 CONSTANT OPT                    \ the optional header
VARIABLE #SECTIONS
VARIABLE WIDE                       \ true: PE32+ (UEFI)
: SECTIONS ( -- offset )   WIDE @ IF $188 ELSE $178 THEN ;   \ the section headers
: DIRS ( -- offset )   OPT  WIDE @ IF 112 ELSE 96 THEN  + ;   \ the data directories

: H! ( x offset n -- )   \ the n low bytes of x at the offset, the lowest first
  SWAP PE-HDR +  SWAP 0 ?DO  2DUP C!  SWAP 8 RSHIFT SWAP 1+  LOOP  2DROP ;
: H@ ( offset -- x )   \ 32 bits at the offset
  PE-HDR +  0  4 0 DO  8 LSHIFT  OVER 3 I - + C@ OR  LOOP  NIP ;
: ALIGN4K ( n -- n' )   $FFF +  $FFF INVERT AND ;
: PICK ( xu ... x0 u -- xu ... x0 xu )   1+ CELLS SP@ + @ ;   \ doc/vm.md 5: the stack

\ The DOS program at $40: push cs; pop ds; mov dx,000E; mov ah,9;
\ int 21h (show the text at $0E, up to "$"); mov ax,4C01; int 21h.
CREATE DOS-CODE  $0E C, $1F C, $BA C, $0E C, $00 C, $B4 C, $09 C, $CD C, $21 C,
                 $B8 C, $01 C, $4C C, $CD C, $21 C,
: DOS-PART ( a u -- )   \ a u: the text that DOS shows
  $5A4D 0 2 H!                      \ "MZ"
  $80 2 2 H!  1 4 2 H!              \ 128 bytes in the one page of the file
  4 8 2 H!                          \ the header: 4 paragraphs (64 bytes)
  $10 10 2 H!  $FFFF 12 2 H!        \ 16 paragraphs more memory, at most all
  $B8 16 2 H!  $40 24 2 H!          \ SP; the place of the (empty) relocation table
  PE-OFF $3C 4 H!                   \ the place of "PE"
  DOS-CODE PE-HDR $40 + 14 MOVE
  TUCK PE-HDR $4E + SWAP MOVE
  $4E +  13 OVER 1 H!  10 OVER 1+ 1 H!  '$' SWAP 2 + 1 H! ;

PUBLIC
: PE-START ( entry -- )
  PE-HDR PE-HDR-SIZE 0 FILL  0 #SECTIONS !  FALSE WIDE !
  S" This program needs Windows." DOS-PART
  $00004550 PE-OFF 4 H!             \ "PE" 0 0
  $14C $84 2 H!                     \ machine: i386
  224 $94 2 H!                      \ the size of the optional header
  $10F $96 2 H!                     \ no relocations, a program, no symbols, 32 bits
  $10B OPT 2 H!                     \ PE32
  OPT 16 + 4 H!                     \ the entry point
  IMAGE-BASE OPT 28 + 4 H!
  $1000 OPT 32 + 4 H!  $200 OPT 36 + 4 H!   \ section alignment, file alignment
  4 OPT 40 + 2 H!  4 OPT 48 + 2 H!  \ Windows 4.0 (95, NT 4) and later
  PE-HDR-SIZE OPT 60 + 4 H!         \ the size of the headers
  3 OPT 68 + 2 H!                   \ subsystem: console
  $8100 OPT 70 + 2 H!               \ data not executable, terminal server aware
  $100000 OPT 72 + 4 H!  $1000 OPT 76 + 4 H!   \ stack: 1 MB reserved, 4 KB at once
  $100000 OPT 80 + 4 H!  $1000 OPT 84 + 4 H!   \ heap
  16 OPT 92 + 4 H! ;                \ 16 data directories

: EFI-START ( entry -- )   \ PE32+: the optional header has 240 bytes, some fields 64 bits
  PE-HDR PE-HDR-SIZE 0 FILL  0 #SECTIONS !  TRUE WIDE !
  S" This program needs UEFI." DOS-PART
  $00004550 PE-OFF 4 H!             \ "PE" 0 0
  $8664 $84 2 H!                    \ machine: x86-64
  240 $94 2 H!                      \ the size of the optional header
  $22 $96 2 H!                      \ a program, more than 2 GB of addresses
  $20B OPT 2 H!                     \ PE32+
  OPT 16 + 4 H!                     \ the entry point
  IMAGE-BASE OPT 24 + 4 H!          \ 64 bits: the upper half is 0
  $1000 OPT 32 + 4 H!  $200 OPT 36 + 4 H!   \ section alignment, file alignment
  PE-HDR-SIZE OPT 60 + 4 H!         \ the size of the headers
  10 OPT 68 + 2 H!                  \ subsystem: an EFI application
  $100000 OPT 72 + 4 H!  $1000 OPT 80 + 4 H!   \ stack, heap: 64 bits each
  $100000 OPT 88 + 4 H!  $1000 OPT 96 + 4 H!
  16 OPT 108 + 4 H! ;               \ 16 data directories

: PE-DIRECTORY ( rva size n -- )   \ the data directory n (1 imports, 12 IAT)
  8 * DIRS +  TUCK 4 + 4 H!  4 H! ;

: PE-SECTION ( a u rva vsize offset size flags -- )
  #SECTIONS @ 40 * SECTIONS +  >R                ( a u rva vsize offset size flags )
  DUP R@ 36 + 4 H!
  DUP $20 AND IF                                   \ code
    OVER OPT 4 + H@ + OPT 4 + 4 H!                 \ the size of the code
    OPT 20 + H@ 0= IF  4 PICK OPT 20 + 4 H!  THEN        \ the first code
  THEN
  $40 AND IF                                       \ initialized data
    DUP OPT 8 + H@ + OPT 8 + 4 H!                  \ the size of the data
    WIDE @ 0=  OPT 24 + H@ 0= AND IF  3 PICK OPT 24 + 4 H!  THEN   \ the first data (PE32 only)
  THEN
  R@ 16 + 4 H!  R@ 20 + 4 H!                       ( a u rva vsize )
  2DUP + ALIGN4K  OPT 56 + H@ MAX  OPT 56 + 4 H!   \ the size of the image in memory
  R@ 8 + 4 H!  R@ 12 + 4 H!                        ( a u )
  8 MIN  PE-HDR R> + SWAP MOVE
  1 #SECTIONS +!  #SECTIONS @ $86 2 H! ;

\ ==== Imports ===========================================================
\ LIST holds the names in their order: a byte 0 (a DLL) or 1 (a
\ function), then the name as a counted string. #IAT counts the cells of
\ the IAT: one for each function, and a 0 after the functions of each DLL.

PRIVATE
2048 CONSTANT LIST-SIZE
CREATE LIST  LIST-SIZE ALLOT   VARIABLE LIST#
VARIABLE IDATA-RVA   VARIABLE #IAT   VARIABLE #DLLS
4096 CONSTANT IDATA-SIZE
CREATE IDATA  IDATA-SIZE ALLOT

: ENTRY+ ( kind a u -- )   \ one name into LIST
  DUP LIST# @ + 2 + LIST-SIZE > ABORT" too many imports"
  ROT LIST LIST# @ + C!  DUP LIST LIST# @ + 1+ C!
  LIST LIST# @ + 2 + SWAP DUP >R MOVE  R> 2 + LIST# +! ;
: CLOSE-DLL ( -- )   #DLLS @ IF  1 #IAT +!  THEN ;   \ the 0 after the functions of a DLL

PUBLIC
: IMPORTS-AT ( rva -- )   IDATA-RVA !  0 LIST# !  0 #IAT !  0 #DLLS ! ;
: DLL ( "name" -- )   CLOSE-DLL  0 PARSE-NAME ENTRY+  1 #DLLS +! ;
: IMPORT ( "name" -- )
  #DLLS @ 0= ABORT" IMPORT needs a DLL before it"
  >IN @  PARSE-NAME 1 ROT ROT ENTRY+  >IN !
  IMAGE-BASE IDATA-RVA @ + #IAT @ CELLS + CONSTANT  1 #IAT +! ;

PRIVATE
VARIABLE P      \ the next free place for a name in IDATA
VARIABLE T      \ the offset of the next cell in the IAT (and the ILT)
VARIABLE D      \ the offset of the next entry of the import directory
VARIABLE DIR0   \ the offset of the import directory: after the IAT
VARIABLE ILT    \ the offset of the lookup table: after the directory
: I! ( x offset -- )   IDATA + 4 0 DO  2DUP C!  SWAP 8 RSHIFT SWAP 1+  LOOP  2DROP ;
: RVA ( offset -- rva )   IDATA-RVA @ + ;
: NAME+ ( a u hint? -- offset )   \ the name at P (after 2 bytes for the hint, if hint?), even
  P @ >R  IF 2 P +! THEN
  TUCK IDATA P @ + SWAP MOVE  P +!  1 P +!  P @ 1 AND P +!  R> ;

PUBLIC
: IMPORT-DATA ( -- a u )
  CLOSE-DLL  IDATA IDATA-SIZE 0 FILL
  #IAT @ CELLS  DUP DIR0 !  DUP D !
  #DLLS @ 1+ 20 * +  DUP ILT !
  #IAT @ CELLS + P !  0 T !
  0 BEGIN  DUP LIST# @ < WHILE                        ( i )
    LIST OVER + DUP 2 + SWAP DUP 1+ C@ SWAP C@        ( i a u kind )
    IF                                                \ a function: its name in the IAT and the ILT
      1 NAME+ RVA  DUP T @ I!  ILT @ T @ + I!  4 T +!
    ELSE                                              \ a DLL: its entry in the directory
      D @ DIR0 @ <> IF  4 T +!  THEN                  \ the 0 after the functions of the DLL before
      0 NAME+ RVA  D @ 12 + I!                        \ its name
      ILT @ T @ + RVA  D @ I!                         \ its part of the ILT
      T @ RVA  D @ 16 + I!                            \ its part of the IAT
      20 D +!
    THEN
    LIST OVER + 1+ C@ 2 + +
  REPEAT  DROP
  P @ IDATA-SIZE > ABORT" the import section is too big"
  0 RVA  #IAT @ CELLS  12 PE-DIRECTORY
  DIR0 @ RVA  #DLLS @ 1+ 20 *  1 PE-DIRECTORY
  IDATA P @ ;

END-PACKAGE
