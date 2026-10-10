\ build.fs - build uCForth for macOS on a Mac with Intel: build/ucf-macos-x64
\
\ Use (in the folder ucf), with a bootstrap VM with 32-bit cells:
\   build/ucf-c -32 host/files.fs [APP ...] targets/macos-x64/build.fs </dev/null
\   chmod +x build/ucf-macos-x64
\
\ 1. FREEZE: the running system is the image: base, the package FILES
\    and the application files before build.fs. It goes to ucf.hex.
\    An application may define ON-BOOT (the image starts with it instead
\    of BOOT) and PROGRAM ( -- a u ), the name of the file; build.fs
\    adds -intel to it (tools/ucf.fs: build/ucf-intel).
\ 2. The assembler (asm/x64.fs) makes the kernel (kernels/x64.fs) and
\    the HAL of this target (hal.fs).
\ 3. The tool writes the Mach-O file (package MACHO, for x86-64): the
\    headers and the code, at 16 KB the memory of the VM with the image,
\    then the code signature. It hashes each page while it writes.
\
\ The program in memory (macOS chooses the address A of __TEXT):
\   A          the headers (472 bytes), then the code at A + $200: read and execute
\   A + $4000  the memory of the VM, 256 KB: VM address 0, the image; read and write
\   A + $44000 the scratch area of the HAL, 16 KB

FREEZE

ONLY FORTH DEFINITIONS  ALSO FILES  S" lib/text.fs" INCLUDED
ONLY FORTH  ALSO FILES  S" lib/ihex.fs" INCLUDED
ONLY FORTH  ALSO FILES  S" host/image.fs" INCLUDED
ONLY FORTH  ALSO FILES  S" lib/sha256.fs" INCLUDED
ONLY FORTH  ALSO FILES  S" lib/macos/macho.fs" INCLUDED
ONLY FORTH  ALSO FILES  S" asm/x64.fs" INCLUDED
ONLY FORTH  ALSO FILES  ALSO TEXT  ALSO IMAGE  ALSO MACHO  ALSO X64  ALSO INTERNALS
VOCABULARY MACOS-X64  ALSO MACOS-X64 DEFINITIONS

$4000   CONSTANT MEM-AT                \ __DATA: VM address 0, after 16 KB of __TEXT
$200    CONSTANT CODE-START            \ the code, after the headers (472 bytes)
$40000  CONSTANT MEM-SIZE              \ VM addresses 0 to $3FFFF (doc/vm.md, section 3)
$4000   CONSTANT SCR-SIZE              \ the scratch area of the HAL, after the VM
MEM-SIZE               CONSTANT SCR    \ its VM address
MEM-SIZE SCR-SIZE +    CONSTANT VM-END \ the VM may use the addresses below it
SCR                    CONSTANT KCELLS \ 4 cells for the kernel

\ ==== The code ==========================================================
\ The addresses of the assembler are places in the file: macOS maps the
\ file from A on, and the code reaches everything relative to itself.

CODE-START ORG
S" kernels/x64.fs" INCLUDED
S" targets/macos-x64/hal.fs" INCLUDED
FILL-TABLE  CHECK-LABELS
TDP @ CODE-BYTES T!                    \ ON-SEGV: faults in the code only
: FITS? ( -- )   THERE MEM-AT U> ABORT" the code does not fit into 16 KB" ;
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

\ ==== The Mach-O file ===================================================

CREATE FNAME 80 ALLOT
: OUTPUT ( -- a u )   \ the name of the file: PROGRAM-intel, or build/ucf-macos-x64
  S" PROGRAM" FIND-NAME ?DUP IF
    NAME>XT EXECUTE FNAME PLACE  S" -intel" FNAME +PLACE  FNAME COUNT EXIT
  THEN  S" build/ucf-macos-x64" ;
: ROUND16K ( n -- n' )   $3FFF +  $3FFF INVERT AND ;

: WRITE-MACHO ( -- )
  IMAGE-OPEN
  S" ON-BOOT" FIND-NAME ?DUP IF NAME>XT BOOT-XT ! THEN   \ an application may start itself
  BOOT-XT @ XT-BOOT T!  QUIT-XT @ XT-QUIT T!  THROW-XT @ XT-THROW T!
  IMAGE-CLOSE
  MACHO-X86-64
  MEM-AT TEXT-SIZE !  IMAGE-SIZE ROUND16K DATA-FILE !
  MEM-SIZE SCR-SIZE + DATA-SIZE !
  CODE-START CODE-OFF !  TDP @ CODE-LEN !  START ENTRY !
  OUTPUT WRITE-START  SIGN-START
  MACHO-HDR PUT
  CODE-START PUT-ZEROS  TARGET TDP @ PUT
  MEM-AT PUT-IMAGE  SIG-OFFSET PUT-ZEROS
  SIGNATURE PUT  WRITE-END ;

: BUILD ( -- )
  WRITE-MACHO
  ."   code: " TDP @ . ." bytes, image: " IMAGE-SIZE . ." bytes" CR
  ."   written: " OUTPUT TYPE ."  (" WRITTEN 0 .R ."  bytes)" CR ;

BUILD
