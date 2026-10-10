\ build.fs - build uCForth for Windows (32 bit): build/ucf-win32.exe
\
\ Use (in the folder ucf), with a bootstrap VM with 32-bit cells:
\   build/ucf-c -32 host/files.fs [APP ...] targets/win32/build.fs </dev/null
\
\ 1. FREEZE: the running system is the image: base, the package FILES
\    and the application files before build.fs. It goes to ucf.hex.
\    An application may define ON-BOOT (the image starts with it instead
\    of BOOT) and PROGRAM ( -- a u ), the name of the file without .exe
\    (else build/ucf-win32); tools/ucf.fs does so.
\ 2. The assembler (asm/x86.fs) makes the kernel (kernels/x86.fs) and
\    the HAL of this target (hal.fs); the package PE (lib/windows/pe.fs)
\    makes the headers and the imports.
\ 3. The tool writes the program NAME.exe, and its bytes as text in
\    NAME.exe.hex: certutil -decodehex makes the program of it, on a
\    Windows that gets only text (README.md).
\
\ The program in memory (the RVA is the address minus IMAGE-BASE, $10000000):
\   $10001000  the imports (one page)
\   $10002000  the code, at most 56 KB
\   $10010000  the memory of the VM, 256 KB: VM address 0, the image
\   $10050000  the scratch area of the HAL, 16 KB

FREEZE

ONLY FORTH DEFINITIONS  ALSO FILES  S" lib/text.fs" INCLUDED
ONLY FORTH  ALSO FILES  S" lib/ihex.fs" INCLUDED
ONLY FORTH  ALSO FILES  S" host/image.fs" INCLUDED
ONLY FORTH  ALSO FILES  S" lib/windows/pe.fs" INCLUDED
ONLY FORTH  ALSO FILES  S" asm/x86.fs" INCLUDED
ONLY FORTH  ALSO FILES  ALSO TEXT  ALSO IMAGE  ALSO PE  ALSO X86  ALSO INTERNALS
VOCABULARY WIN32  ALSO WIN32 DEFINITIONS

$1000  CONSTANT IDATA-RVA              \ the imports
$2000  CONSTANT CODE-RVA               \ the code
$10000 CONSTANT DATA-RVA               \ the memory of the VM
$40000 CONSTANT MEM-SIZE               \ VM addresses 0 to $3FFFF (doc/vm.md, section 3)
$4000  CONSTANT SCR-SIZE               \ the scratch area of the HAL, after the VM
IMAGE-BASE DATA-RVA +  CONSTANT MEM    \ VM address 0
MEM MEM-SIZE +         CONSTANT SCR
MEM-SIZE SCR-SIZE +    CONSTANT VM-END \ the VM may use the addresses below it
SCR                    CONSTANT KCELLS \ 4 cells for the kernel
IMAGE-BASE CODE-RVA +  CONSTANT CODE-START

\ ==== The code ==========================================================

IDATA-RVA IMPORTS-AT
CODE-START ORG
S" kernels/x86.fs" INCLUDED
S" targets/win32/hal.fs" INCLUDED
FILL-TABLE  CHECK-LABELS
THERE CODE-END T!                      \ ON-FAULT: faults in the code only
: FITS? ( -- )   THERE IMAGE-BASE DATA-RVA + U> ABORT" the code does not fit into 56 KB" ;
FITS?

\ The image is the frozen host itself, so the xts of the host are the xts
\ of the image. ?RX needs the xt of REFILL and the end of its body.
VARIABLE BX   VARIABLE BEST
: BODY-END ( xt -- a )   \ the next header in memory after xt
  BX !  -1 BEST !
  VOC-LINK @ BEGIN ?DUP WHILE
    DUP @ BEGIN ?DUP WHILE
      DUP BX @ U>  OVER BEST @ U<  AND IF DUP BEST ! THEN  @
    REPEAT  CELL+ @
  REPEAT  BEST @ ;
' REFILL DUP XT-REFILL T!  BODY-END REFILL-END T!
OUT OUT-ADDR T!                        \ BREAK sets the column to 0

\ ==== The file ==========================================================

: OUTPUT ( -- a u )   \ the name without .exe: the application may give it (PROGRAM)
  S" PROGRAM" FIND-NAME ?DUP IF NAME>XT EXECUTE EXIT THEN  S" build/ucf-win32" ;
CREATE FNAME 80 ALLOT
: NAME. ( a u -- a' u' )   OUTPUT FNAME PLACE  FNAME +PLACE  FNAME COUNT ;   \ OUTPUT with .exe or .exe.hex
: ROUND512 ( n -- n' )   511 +  511 INVERT AND ;

VARIABLE IDATA-A   VARIABLE IDATA-U
: CODE-OFF ( -- off )   PE-HDR-SIZE  IDATA-U @ ROUND512 + ;
: DATA-OFF ( -- off )   CODE-OFF  TDP @ ROUND512 + ;

\ ==== The same bytes as text for certutil =============================
\ PUT gives each part of the file to ON-PUT (host/image.fs): so the text
\ comes while the file is written. Each line: 16 bytes as hex digits
\ with a space between them, then CR LF.

VARIABLE HFID   CREATE HLINE 64 ALLOT   VARIABLE HL#   VARIABLE HB#   VARIABLE #HEX-LINES
: HC ( c -- )   HLINE HL# @ + C!  1 HL# +! ;
: HEX-DIGIT ( n -- )   S" 0123456789abcdef" DROP + C@ HC ;
: HEX-LINE ( -- )   \ the line so far into the file
  HL# @ IF
    13 HC  10 HC  HLINE HL# @ HFID @ WRITE-FILE THROW
    0 HL# !  0 HB# !  1 #HEX-LINES +!
  THEN ;
: HEX-BYTE ( c -- )
  HB# @ IF 32 HC THEN  DUP 4 RSHIFT HEX-DIGIT  15 AND HEX-DIGIT
  1 HB# +!  HB# @ 16 = IF HEX-LINE THEN ;
: HEX-PUT ( a u -- )   0 ?DO  DUP I + C@ HEX-BYTE  LOOP  DROP ;
' HEX-PUT IS ON-PUT
: HEX-START ( -- )
  S" .exe.hex" NAME. W/O CREATE-FILE THROW HFID !  0 HL# !  0 HB# !  0 #HEX-LINES ! ;
: HEX-END ( -- )   HEX-LINE  HFID @ CLOSE-FILE THROW ;

: WRITE-EXE ( -- )
  IMAGE-OPEN
  S" ON-BOOT" FIND-NAME ?DUP IF NAME>XT BOOT-XT ! THEN   \ an application may start itself
  BOOT-XT @ XT-BOOT T!  QUIT-XT @ XT-QUIT T!  THROW-XT @ XT-THROW T!
  IMAGE-CLOSE
  START IMAGE-BASE - PE-START
  IMPORT-DATA  IDATA-U !  IDATA-A !            \ it sets two entries of the header
  IDATA-U @ $1000 > ABORT" the imports do not fit into 4 KB"
  S" .idata" IDATA-RVA IDATA-U @  PE-HDR-SIZE IDATA-U @ ROUND512  DATA-SECTION PE-SECTION
  S" .text"  CODE-RVA  DATA-RVA CODE-RVA -  CODE-OFF TDP @ ROUND512  CODE-SECTION PE-SECTION
  S" .data"  DATA-RVA  MEM-SIZE SCR-SIZE +  DATA-OFF IMAGE-SIZE ROUND512  DATA-SECTION PE-SECTION
  HEX-START  S" .exe" NAME. WRITE-START
  PE-HDR PE-HDR-SIZE PUT
  IDATA-A @ IDATA-U @ PUT  CODE-OFF PUT-ZEROS
  TARGET TDP @ PUT  DATA-OFF PUT-ZEROS
  DATA-OFF PUT-IMAGE  DATA-OFF IMAGE-SIZE ROUND512 + PUT-ZEROS
  WRITE-END  HEX-END ;

: BUILD ( -- )
  WRITE-EXE
  ."   code: " TDP @ . ." bytes, image: " IMAGE-SIZE . ." bytes" CR
  ."   written: " S" .exe" NAME. TYPE ."  (" WRITTEN 0 .R ."  bytes), "
  S" .exe.hex" NAME. TYPE ."  (" #HEX-LINES @ 0 .R ."  lines)" CR ;

BUILD
