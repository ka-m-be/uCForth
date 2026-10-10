\ build.fs - build uCForth for Linux on a PC with 64 bits (x86-64): build/ucf-linux-x64
\
\ Use (in the folder ucf), with a bootstrap VM with 32-bit cells:
\   build/ucf-c -32 host/files.fs [APP ...] targets/linux-x64/build.fs </dev/null
\   chmod +x build/ucf-linux-x64
\
\ 1. FREEZE: the running system is the image: base, the package FILES
\    and the application files before build.fs. It goes to ucf.hex.
\    An application may define ON-BOOT (the image starts with it instead
\    of BOOT) and PROGRAM ( -- a u ), the name of the file; build.fs
\    adds -x64 to it (tools/ucf.fs: build/ucf-x64).
\ 2. The assembler (asm/x64.fs) makes the kernel (kernels/x64.fs) and
\    the HAL of this target (hal.fs).
\ 3. The tool writes the ELF file (package ELF, the 64-bit form): the
\    headers and the code, then at 64 KB the memory of the VM with the
\    image.
\
\ The program in memory:
\   $400000  the headers (176 bytes), then the code: read and execute
\   $410000  the memory of the VM, 256 KB: VM address 0, the image; read and write
\   $450000  the scratch area of the HAL, 16 KB

FREEZE

ONLY FORTH DEFINITIONS  ALSO FILES  S" lib/text.fs" INCLUDED
ONLY FORTH  ALSO FILES  S" lib/ihex.fs" INCLUDED
ONLY FORTH  ALSO FILES  S" host/image.fs" INCLUDED
ONLY FORTH  ALSO FILES  S" lib/linux/elf.fs" INCLUDED
ONLY FORTH  ALSO FILES  S" asm/x64.fs" INCLUDED
ONLY FORTH  ALSO FILES  ALSO TEXT  ALSO IMAGE  ALSO ELF  ALSO X64  ALSO INTERNALS
VOCABULARY LINUX-X64  ALSO LINUX-X64 DEFINITIONS

$400000 CONSTANT CODE-VADDR            \ the first segment: headers and code
$410000 CONSTANT MEM-AT                \ the second segment: VM address 0
$10000  CONSTANT MEM-OFFSET            \ its place in the file
$40000  CONSTANT MEM-SIZE              \ VM addresses 0 to $3FFFF (doc/vm.md, section 3)
$4000   CONSTANT SCR-SIZE              \ the scratch area of the HAL, after the VM
176     CONSTANT HDR-SIZE              \ the ELF header and two program headers
MEM-SIZE               CONSTANT SCR    \ its VM address
MEM-SIZE SCR-SIZE +    CONSTANT VM-END \ the VM may use the addresses below it
SCR                    CONSTANT KCELLS \ 4 cells for the kernel
CODE-VADDR HDR-SIZE +  CONSTANT CODE-START

\ ==== The code ==========================================================

CODE-START ORG
S" kernels/x64.fs" INCLUDED
S" targets/linux-x64/hal.fs" INCLUDED
FILL-TABLE  CHECK-LABELS
TDP @ CODE-BYTES T!                    \ ON-SEGV: faults in the code only

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

\ ==== The ELF file ======================================================

CREATE FNAME 80 ALLOT
: OUTPUT ( -- a u )   \ the name of the file: PROGRAM-x64, or build/ucf-linux-x64
  S" PROGRAM" FIND-NAME ?DUP IF
    NAME>XT EXECUTE FNAME PLACE  S" -x64" FNAME +PLACE  FNAME COUNT EXIT
  THEN  S" build/ucf-linux-x64" ;

: WRITE-ELF ( -- )
  IMAGE-OPEN
  S" ON-BOOT" FIND-NAME ?DUP IF NAME>XT BOOT-XT ! THEN   \ an application may start itself
  BOOT-XT @ XT-BOOT T!  QUIT-XT @ XT-QUIT T!  THROW-XT @ XT-THROW T!
  IMAGE-CLOSE
  OUTPUT WRITE-START
  62 START ELF-START
  5 0 CODE-VADDR  HDR-SIZE TDP @ +  DUP ELF-SEGMENT
  6 MEM-OFFSET MEM-AT  IMAGE-SIZE  MEM-SIZE SCR-SIZE +  ELF-SEGMENT
  ELF-SIZE HDR-SIZE <> ABORT" the headers do not fit"
  ELF-HDR ELF-SIZE PUT  TARGET TDP @ PUT
  MEM-OFFSET PUT-IMAGE  WRITE-END ;

: BUILD ( -- )
  HDR-SIZE TDP @ +  MEM-OFFSET U> ABORT" the code does not fit into 64 KB"
  WRITE-ELF
  ."   code: " TDP @ . ." bytes, image: " IMAGE-SIZE . ." bytes" CR
  ."   written: " OUTPUT TYPE ."  (" WRITTEN 0 .R ."  bytes)" CR ;

BUILD
