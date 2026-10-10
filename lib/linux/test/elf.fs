\ elf.fs - tests of the package ELF (lib/linux/elf.fs)
\
\ Load (in the folder ucf, with 32-bit cells), after the package FILES
\ and the tester:
\   ... -32 host/files.fs forth/test/tester.fs lib/linux/test/elf.fs
\ The values are those of build/ucf-linux-a64 and build/ucf-linux-i386,
\ as readelf shows them.

ONLY FORTH DEFINITIONS  ALSO FILES
S" lib/linux/elf.fs" INCLUDED
ONLY FORTH DEFINITIONS  ALSO INTERNALS  ALSO ELF
DECIMAL

: EL@ ( offset -- x )   \ 32 bits of the header, the lowest byte first
  ELF-HDR +  0 SWAP 3 +  4 0 DO  SWAP 8 LSHIFT  OVER C@ OR  SWAP 1-  LOOP  DROP ;
: EW@ ( offset -- x )   ELF-HDR + DUP C@ SWAP 1+ C@ 8 LSHIFT OR ;

183 $401014 ELF-START
5 0 $400000 $164C $164C ELF-SEGMENT
6 $10000 $410000 $6BC8 $21000 ELF-SEGMENT

T{ 0 EL@ -> $464C457F }T                 \ 7F "ELF"
T{ 4 ELF-HDR + C@  5 ELF-HDR + C@ -> 2 1 }T   \ 64 bit, little endian
T{ 16 EW@  18 EW@ -> 2 183 }T            \ an executable file for aarch64
T{ 24 EL@  28 EL@ -> $401014 0 }T        \ the entry, 64 bits
T{ 32 EL@  54 EW@  56 EW@ -> 64 56 2 }T  \ program headers: offset, size, number
T{ ELF-SIZE -> 176 }T
T{ 64 EL@  68 EL@ -> 1 5 }T              \ segment 1: PT_LOAD, read and execute
T{ 120 EL@  124 EL@  128 EL@ -> 1 6 $10000 }T   \ segment 2: PT_LOAD, read and write, offset
T{ 136 EL@  152 EL@  160 EL@  168 EL@ -> $410000 $6BC8 $21000 $10000 }T   \ vaddr filesz memsz align

\ The 32-bit form (i386), as for build/ucf-linux-i386
3 $402160 ELF-START
5 0 $400000 $21F1 $21F1 ELF-SEGMENT
6 $10000 $410000 $15328 $44000 ELF-SEGMENT

T{ 4 ELF-HDR + C@  18 EW@ -> 1 3 }T     \ 32 bit, i386
T{ 24 EL@  28 EL@ -> $402160 52 }T       \ the entry, the program headers
T{ 40 EW@  42 EW@  44 EW@  ELF-SIZE -> 52 32 2 116 }T   \ the sizes, the number
T{ 52 EL@  56 EL@  60 EL@  68 EL@  76 EL@  80 EL@ -> 1 0 $400000 $21F1 5 $10000 }T   \ segment 1
T{ 88 EL@  92 EL@  100 EL@  104 EL@  108 EL@ -> $10000 $410000 $15328 $44000 6 }T    \ segment 2

S" elf" .RESULTS
