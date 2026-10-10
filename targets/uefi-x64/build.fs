\ build.fs - build uCForth for a PC with UEFI (x86-64): build/ucf-x64.efi
\
\ Use (in the folder ucf), with a bootstrap VM with 32-bit cells:
\   build/ucf-c -32 host/files.fs [APP ...] targets/uefi-x64/build.fs </dev/null
\
\ 1. FREEZE: the running system is the image: base, the package FILES
\    and the application files before build.fs. It goes to ucf.hex.
\    An application may define ON-BOOT (the image starts with it instead
\    of BOOT).
\ 2. The assembler (asm/x64.fs) makes the kernel (kernels/x64.fs) and
\    the HAL of this target (hal.fs).
\ 3. The tool writes the PE32+ file (package PE): the headers, the
\    code, then the memory of the VM, 256 KB, with the image. FREEZE on
\    the PC writes this part of the file again (SAVE-IMAGE).
\
\ On a USB stick with FAT32 the file is EFI/BOOT/BOOTX64.EFI (README.md).
\
\ The program in memory (UEFI chooses the address A, the image base):
\   A           the headers (512 bytes)
\   A + $1000   the code: read and execute
\   A + $10000  the memory of the VM, 256 KB: VM address 0, the image; read and write
\   A + $50000  the scratch area of the HAL, 16 KB

FREEZE

ONLY FORTH DEFINITIONS  ALSO FILES  S" lib/text.fs" INCLUDED
ONLY FORTH  ALSO FILES  S" lib/ihex.fs" INCLUDED
ONLY FORTH  ALSO FILES  S" host/image.fs" INCLUDED
ONLY FORTH  ALSO FILES  S" lib/windows/pe.fs" INCLUDED
ONLY FORTH  ALSO FILES  S" asm/x64.fs" INCLUDED
ONLY FORTH  ALSO FILES  ALSO TEXT  ALSO IMAGE  ALSO PE  ALSO X64  ALSO INTERNALS
VOCABULARY UEFI-X64  ALSO UEFI-X64 DEFINITIONS

$1000   CONSTANT CODE-RVA              \ the section .text
$10000  CONSTANT MEM-AT                \ the section .data: VM address 0 (RVA)
$40000  CONSTANT MEM-SIZE              \ VM addresses 0 to $3FFFF (doc/vm.md, section 3)
$4000   CONSTANT SCR-SIZE              \ the scratch area of the HAL, after the VM
$200    CONSTANT CODE-POS              \ the code in the file, after the headers
MEM-SIZE               CONSTANT SCR    \ its VM address
MEM-SIZE SCR-SIZE +    CONSTANT VM-END \ the VM may use the addresses below it
SCR                    CONSTANT KCELLS \ 4 cells for the kernel

\ ==== The code ==========================================================
\ The addresses of the assembler are RVAs: UEFI maps the file from A on,
\ and the code reaches everything relative to itself.

CODE-RVA ORG
S" kernels/x64.fs" INCLUDED
S" targets/uefi-x64/hal.fs" INCLUDED
FILL-TABLE  CHECK-LABELS
: FITS? ( -- )   THERE MEM-AT U> ABORT" the code does not fit into 60 KB" ;
FITS?
: ROUND512 ( n -- n' )   511 +  511 INVERT AND ;
CODE-POS TDP @ ROUND512 +  CONSTANT DATA-OFF   \ VM address 0 in the file
DATA-OFF DATA-POS T!                   \ SAVE-IMAGE writes there

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
ECHO ECHO-ADDR T!                      \ ACCEPT shows the keys

\ ==== The PE32+ file ====================================================

: OUTPUT ( -- a u )   S" build/ucf-x64.efi" ;

: WRITE-EFI ( -- )
  IMAGE-OPEN
  S" ON-BOOT" FIND-NAME ?DUP IF NAME>XT BOOT-XT ! THEN   \ an application may start itself
  BOOT-XT @ XT-BOOT T!  QUIT-XT @ XT-QUIT T!  THROW-XT @ XT-THROW T!
  IMAGE-CLOSE
  IMAGE-SIZE MEM-SIZE U> ABORT" the image is bigger than the memory of the VM"
  START EFI-START
  S" .text"  CODE-RVA  TDP @  CODE-POS  TDP @ ROUND512  CODE-SECTION PE-SECTION
  S" .data"  MEM-AT  MEM-SIZE SCR-SIZE +  DATA-OFF  MEM-SIZE  DATA-SECTION PE-SECTION
  OUTPUT WRITE-START
  PE-HDR PE-HDR-SIZE PUT
  TARGET TDP @ PUT  DATA-OFF PUT-ZEROS
  DATA-OFF PUT-IMAGE  DATA-OFF MEM-SIZE + PUT-ZEROS
  WRITE-END ;

: BUILD ( -- )
  WRITE-EFI
  ."   code: " TDP @ . ." bytes, image: " IMAGE-SIZE . ." bytes" CR
  ."   written: " OUTPUT TYPE ."  (" WRITTEN 0 .R ."  bytes)" CR ;

BUILD
