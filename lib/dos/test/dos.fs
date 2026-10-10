\ dos.fs - tests of the package DOS (lib/dos/dos.fs), only on DOS
\
\ Load (on DOS, in the folder of uCForth), after the package FILES and
\ the tester, for example:
\   UCF -image SEED.HEX FORTH\BASE.FS HOST\FILES.FS FORTH\TEST\TESTER.FS LIB\DOS\TEST\DOS.FS
\ targets/dos/README.md runs it in FreeDOS.

ONLY FORTH DEFINITIONS  ALSO FILES
S" lib/dos/dos.fs" INCLUDED
ONLY FORTH DEFINITIONS  ALSO DOS
DECIMAL

\ INT 21h: the version (AH 30h), a date (AH 2Ah), a file that is not there
T{ $3000 1 DOS NIP -> FALSE }T                       \ no carry
T{ $3000 1 DOS DROP 255 AND 2 < -> FALSE }T          \ DOS 2 or newer
T{ $2A00 1 DOS 2DROP  REGS 4 + @ 1980 < -> FALSE }T  \ CX: the year
CREATE NONE  S" NONE.XYZ" HERE SWAP DUP ALLOT MOVE  0 C,
T{ $3D00 0 0 NONE 4 DOS -> 2 TRUE }T                 \ AX 2: the file is not there
T{ REGS @ -> 2 }T

\ Ports: the mask of the interrupt controller (21h), back as it was; a
\ word from 20h has the byte of 21h in its upper half
T{ $21 PC@ DUP $21 PC!  $21 PC@ = -> TRUE }T
T{ $20 PW@ 8 RSHIFT  $21 PC@ = -> TRUE }T

S" dos" .RESULTS
