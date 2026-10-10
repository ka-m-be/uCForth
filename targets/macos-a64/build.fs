\ build.fs - build uCForth for macOS on Apple silicon: build/ucf-macos-a64
\
\ Use (in the folder ucf), with a bootstrap VM with 32-bit cells:
\   LC_ALL=C awk -f bootstrap/awk/ucf.awk -- -32 host/files.fs [APP ...] targets/macos-a64/build.fs </dev/null
\   chmod +x build/ucf-macos-a64
\
\ 1. FREEZE: the running system is the image: base, the package FILES
\    and the application files before build.fs. It goes to ucf.hex.
\    An application may define ON-BOOT (the image starts with it instead
\    of BOOT) and PROGRAM ( -- a u ), the name of the file (else
\    build/ucf-macos-a64); tools/ucf.fs does so.
\ 2. The assembler (asm/aarch64.fs) makes the kernel (kernels/aarch64.fs)
\    and the HAL of this target (hal.fs).
\ 3. The tool writes the Mach-O file (package MACHO): the headers and the
\    code, at 16 KB the memory of the VM with the image, then the code
\    signature. It hashes each page while it writes (about 30 s in awk).

FREEZE

ONLY FORTH DEFINITIONS  ALSO FILES  S" lib/ihex.fs" INCLUDED
ONLY FORTH  ALSO FILES  S" host/image.fs" INCLUDED
ONLY FORTH  ALSO FILES  S" lib/sha256.fs" INCLUDED
ONLY FORTH  ALSO FILES  S" lib/macos/macho.fs" INCLUDED
ONLY FORTH  ALSO FILES  S" asm/aarch64.fs" INCLUDED
ONLY FORTH  ALSO FILES  ALSO IMAGE  ALSO MACHO  ALSO AARCH64  ALSO INTERNALS
VOCABULARY MACOS-A64  ALSO MACOS-A64 DEFINITIONS

$4000   CONSTANT MEM-OFF    \ __DATA: VM address 0, after 16 KB of __TEXT
$200    CONSTANT CODE-START \ the code, after the headers (472 bytes)
$40000  CONSTANT MEM-SIZE   \ VM addresses 0 to $3FFFF (doc/vm.md, section 3)
$4000   CONSTANT SCR-SIZE   \ the scratch area of the HAL, after the VM

\ ==== The code ==========================================================

CODE-START START-CODE
S" kernels/aarch64.fs" INCLUDED
S" targets/macos-a64/hal.fs" INCLUDED
FILL-TABLE  END-CODE
: FITS? ( -- )   THERE MEM-OFF U< 0= ABORT" the code does not fit into 16 KB" ;
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
' REFILL DUP XT-REFILL @ A!  BODY-END REFILL-END @ A!
OUT OUT-ADDR @ A!   \ BREAK sets the column to 0

\ ==== The Mach-O file ===================================================

: OUTPUT ( -- a u )   \ the name of the file: the application may give it (PROGRAM)
  S" PROGRAM" FIND-NAME ?DUP IF NAME>XT EXECUTE EXIT THEN  S" build/ucf-macos-a64" ;
: ROUND16K ( n -- n' )   $3FFF +  $3FFF INVERT AND ;

: WRITE-MACHO ( -- )
  IMAGE-OPEN
  S" ON-BOOT" FIND-NAME ?DUP IF NAME>XT BOOT-XT ! THEN   \ an application may start itself
  BOOT-XT @ XT-BOOT @ A!  QUIT-XT @ XT-QUIT @ A!  THROW-XT @ XT-THROW @ A!
  IMAGE-CLOSE
  MEM-OFF TEXT-SIZE !  IMAGE-SIZE ROUND16K DATA-FILE !
  MEM-SIZE SCR-SIZE + DATA-SIZE !
  CODE-START CODE-OFF !  CODE# @ CODE-LEN !  START @ ENTRY !
  OUTPUT WRITE-START  SIGN-START
  MACHO-HDR PUT
  CODE-START PUT-ZEROS  CODE CODE# @ PUT
  MEM-OFF PUT-IMAGE  SIG-OFFSET PUT-ZEROS
  SIGNATURE PUT  WRITE-END ;

: BUILD ( -- )
  WRITE-MACHO
  ."   code: " CODE# @ . ." bytes, image: " IMAGE-SIZE . ." bytes" CR
  ."   written: " OUTPUT TYPE ."  (" WRITTEN 0 .R ."  bytes)" CR ;

BUILD
