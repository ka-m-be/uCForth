\ elf.fs - package ELF: the headers of a program file in the ELF format
\
\ Linux starts a program from an ELF file (Executable and Linkable
\ Format). The file starts with a header, then a program header for each
\ segment. A segment tells Linux: copy filesz bytes from this offset in
\ the file to this virtual address, give it memsz bytes (the rest is 0),
\ with these rights. The machine sets the form: aarch64 (183) and x86-64
\ (62) take the 64-bit form (a header of 64 bytes, a program header of
\ 56 bytes), i386 (3) the 32-bit form (52 and 32 bytes).
\
\   machine entry ELF-START              a new header
\   flags offset vaddr filesz memsz ELF-SEGMENT
\   ELF-HDR ELF-SIZE                     the headers, to write first
\
\ flags: 4 read, 2 write, 1 execute. The package needs cells of 32
\ bits; the upper halves of the 64-bit fields are 0. Segments are
\ aligned on 64 KB.

PACKAGE ELF   \ lib/package.fs

PRIVATE
4 CONSTANT MAX-SEGMENTS
VARIABLE PHNUM
VARIABLE WIDE                       \ true: the 64-bit form
: EHSIZE ( -- n )   WIDE @ IF 64 ELSE 52 THEN ;
: PHSIZE ( -- n )   WIDE @ IF 56 ELSE 32 THEN ;

PUBLIC
CREATE ELF-HDR  64 56 MAX-SEGMENTS * + ALLOT

PRIVATE

: H! ( x offset n -- )   \ the n low bytes of x at the offset, the lowest first
  SWAP ELF-HDR +  SWAP 0 ?DO  2DUP C!  SWAP 8 RSHIFT SWAP 1+  LOOP  2DROP ;

PUBLIC
: ELF-SIZE ( -- u )   EHSIZE PHSIZE PHNUM @ * + ;

: ELF-START ( machine entry -- )
  OVER 3 <> WIDE !
  ELF-HDR  64 56 MAX-SEGMENTS * +  0 FILL  0 PHNUM !
  $464C457F 0 4 H!               \ 7F "ELF"
  WIDE @ IF 2 ELSE 1 THEN 4 1 H!   \ 64 or 32 bit,
  1 5 1 H!  1 6 1 H!             \ little endian, version 1
  2 16 2 H!                      \ an executable file
  SWAP 18 2 H!  1 20 4 H!  24 4 H!   \ machine, version, entry
  WIDE @ IF                      \ the program headers follow the header
    EHSIZE 32 4 H!  EHSIZE 52 2 H!  PHSIZE 54 2 H!
  ELSE
    EHSIZE 28 4 H!  EHSIZE 40 2 H!  PHSIZE 42 2 H!
  THEN ;

: ELF-SEGMENT ( flags offset vaddr filesz memsz -- )
  PHNUM @ MAX-SEGMENTS < 0= ABORT" too many segments"
  EHSIZE PHSIZE PHNUM @ * + >R
  WIDE @ IF
    R@ 40 + 4 H!  R@ 32 + 4 H!                \ memsz, filesz
    DUP R@ 16 + 4 H!  R@ 24 + 4 H!            \ vaddr, paddr
    R@ 8 + 4 H!  R@ 4 + 4 H!                  \ offset, flags
    $10000 R@ 48 + 4 H!                       \ align
  ELSE
    R@ 20 + 4 H!  R@ 16 + 4 H!                \ memsz, filesz
    DUP R@ 8 + 4 H!  R@ 12 + 4 H!             \ vaddr, paddr
    R@ 4 + 4 H!  R@ 24 + 4 H!                 \ offset, flags
    $10000 R@ 28 + 4 H!                       \ align
  THEN
  1 R> 4 H!                                   \ PT_LOAD
  1 PHNUM +!  PHNUM @  WIDE @ IF 56 ELSE 44 THEN  2 H! ;

END-PACKAGE
