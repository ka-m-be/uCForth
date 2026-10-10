\ build.fs - build uCForth for Linux on aarch64: build/ucf-linux-a64
\
\ Use (in the folder ucf), with a bootstrap VM with 32-bit cells:
\   LC_ALL=C awk -f bootstrap/awk/ucf.awk -- -32 host/files.fs [APP ...] targets/linux-a64/build.fs
\   chmod +x build/ucf-linux-a64
\
\ 1. FREEZE: the running system is the image: base, the package FILES
\    and the application files before build.fs. SAVE-IMAGE writes it to
\    ucf.hex (doc/vm.md, section 10). An application may define ON-BOOT
\    (the image starts with it instead of BOOT) and PROGRAM ( -- a u ),
\    the name of the file; build.fs adds -a64 to it (tools/ucf.fs:
\    build/ucf-a64).
\ 2. The assembler (asm/aarch64.fs) makes the kernel (kernels/aarch64.fs)
\    and the HAL of this target (hal.fs).
\ 3. The tool reads ucf.hex again (package IMAGE, host/image.fs) and
\    writes the ELF file: the headers and the code, then at 64 KB the
\    memory of the VM with the image.
\
\ The memory of the VM (256 KB) is at MEM-VADDR, then 16 KB for the HAL.

FREEZE

ONLY FORTH DEFINITIONS  ALSO FILES  S" lib/text.fs" INCLUDED
ONLY FORTH  ALSO FILES  S" lib/ihex.fs" INCLUDED
ONLY FORTH  ALSO FILES  S" host/image.fs" INCLUDED
ONLY FORTH  ALSO FILES  S" lib/linux/elf.fs" INCLUDED
ONLY FORTH  ALSO FILES  S" asm/aarch64.fs" INCLUDED
ONLY FORTH  ALSO FILES  ALSO TEXT  ALSO IMAGE  ALSO ELF  ALSO AARCH64  ALSO INTERNALS
VOCABULARY LINUX-A64  ALSO LINUX-A64 DEFINITIONS

$400000 CONSTANT CODE-VADDR   \ the first segment: headers and code, read and execute
$410000 CONSTANT MEM-VADDR    \ the second segment: the memory of the VM, read and write
$10000  CONSTANT MEM-OFFSET   \ its place in the file
$40000  CONSTANT MEM-SIZE     \ VM addresses 0 to $3FFFF (doc/vm.md, section 3)
$4000   CONSTANT SCR-SIZE     \ the scratch area of the HAL
176     CONSTANT HDR-SIZE     \ the ELF header and two program headers

\ ==== The code ==========================================================

CODE-VADDR HDR-SIZE + START-CODE
S" kernels/aarch64.fs" INCLUDED
S" targets/linux-a64/hal.fs" INCLUDED
FILL-TABLE  END-CODE

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

\ ==== The ELF file ======================================================

CREATE FNAME 80 ALLOT
: OUTPUT ( -- a u )   \ the name of the file: PROGRAM-a64, or build/ucf-linux-a64
  S" PROGRAM" FIND-NAME ?DUP IF
    NAME>XT EXECUTE FNAME PLACE  S" -a64" FNAME +PLACE  FNAME COUNT EXIT
  THEN  S" build/ucf-linux-a64" ;

: WRITE-ELF ( -- )
  IMAGE-OPEN
  S" ON-BOOT" FIND-NAME ?DUP IF NAME>XT BOOT-XT ! THEN   \ an application may start itself
  BOOT-XT @ XT-BOOT @ A!  QUIT-XT @ XT-QUIT @ A!  THROW-XT @ XT-THROW @ A!
  IMAGE-CLOSE
  OUTPUT WRITE-START
  183 START @ ELF-START
  5 0 CODE-VADDR  HDR-SIZE CODE# @ +  DUP ELF-SEGMENT
  6 MEM-OFFSET MEM-VADDR  IMAGE-SIZE  MEM-SIZE SCR-SIZE +  ELF-SEGMENT
  ELF-SIZE HDR-SIZE <> ABORT" the headers do not fit"
  ELF-HDR ELF-SIZE PUT  CODE CODE# @ PUT
  MEM-OFFSET PUT-IMAGE  WRITE-END ;

: BUILD ( -- )
  WRITE-ELF
  ."   code: " CODE# @ . ." bytes, image: " IMAGE-SIZE . ." bytes" CR
  ."   written: " OUTPUT TYPE ."  (" WRITTEN 0 .R ."  bytes)" CR ;

BUILD
