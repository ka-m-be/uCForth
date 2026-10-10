\ build.fs - build uCForth for the Raspberry Pi 4 and 400, bare metal: build/kernel8.img
\
\ Use (in the folder ucf), with a bootstrap VM with 32-bit cells:
\   LC_ALL=C awk -f bootstrap/awk/ucf.awk -- -32 [APP ...] targets/freeze.fs host/files.fs targets/rpi4/build.fs
\
\ 1. targets/freeze.fs: the running system is the image: base and the
\    application files, without the package FILES. It is in ucf.hex.
\ 2. The assembler (asm/aarch64.fs) makes the kernel (kernels/aarch64.fs)
\    and the HAL of this target (hal.fs), at CODE-ADDR.
\ The image starts with BOOT, or with ON-BOOT when the image has this
\ word (for example targets/rpi4/screen.fs): ON-BOOT starts drivers,
\ then runs BOOT.
\
\ 3. The tool reads ucf.hex (package IMAGE) and writes kernel8.img, the
\    memory from address 0: at 0 a jump to START, at $100 the image, at
\    CODE-ADDR the code.

ONLY FORTH DEFINITIONS  ALSO FILES  S" lib/ihex.fs" INCLUDED
ONLY FORTH  ALSO FILES  S" host/image.fs" INCLUDED
ONLY FORTH  ALSO FILES  S" asm/aarch64.fs" INCLUDED
ONLY FORTH  ALSO FILES  ALSO IMAGE  ALSO AARCH64  ALSO INTERNALS
VOCABULARY RPI4  ALSO RPI4 DEFINITIONS

$40000 CONSTANT CODE-ADDR   \ after the memory of the VM (doc/vm.md, section 3)

\ ==== The code ==========================================================

CODE-ADDR START-CODE
S" kernels/aarch64.fs" INCLUDED
S" targets/rpi4/hal.fs" INCLUDED
FILL-TABLE  END-CODE
ECHO ECHO-ADDR @ A!   \ START sets ECHO: the terminal does not echo

\ ==== kernel8.img =======================================================

VARIABLE JUMP
: WRITE-KERNEL ( -- )
  IMAGE-OPEN
  S" ON-BOOT" FIND-NAME ?DUP IF NAME>XT BOOT-XT ! THEN   \ the start word
  BOOT-XT @ XT-BOOT @ A!  QUIT-XT @ XT-QUIT @ A!  THROW-XT @ XT-THROW @ A!
  IMAGE-CLOSE
  S" build/kernel8.img" WRITE-START
  START @ 2/ 2/ $14000000 OR JUMP !  JUMP 4 PUT        \ b START
  0 PUT-IMAGE
  CODE-ADDR PUT-ZEROS  CODE CODE# @ PUT  WRITE-END ;

: BUILD ( -- )
  WRITE-KERNEL
  ."   code: " CODE# @ . ." bytes, image: " IMAGE-SIZE . ." bytes" CR
  ."   written: build/kernel8.img (" WRITTEN 0 .R ."  bytes)" CR ;

BUILD
