\ build.fs - build uCForth for a PC with a BIOS: build/ucf-pc.img
\
\ Use (in the folder ucf), with a bootstrap VM with 16-bit cells (the
\ default), for example:
\   build/ucf-c lib/package.fs lib/pc/bios.fs [APP ...] targets/freeze.fs host/files.fs targets/pc-bios/build.fs </dev/null
\
\ 1. targets/freeze.fs: the running system is the image: base, the
\    package BIOS and the application files, without the package FILES
\    (the PC has no files). It is in ucf.hex.
\ 2. The assembler (asm/8086.fs) makes the boot sector (bootsect.fs) and
\    the code: the kernel (kernels/8086.fs) and the HAL (hal.fs).
\ 3. It writes build/ucf-pc.img, a floppy disk of 1.44 MB (2880 sectors
\    of 512 bytes):
\      sector 0         the boot sector
\      sectors 1-16     the code (8 KB)
\      sectors 17-144   the memory of the VM (64 KB): the image at its
\                       addresses, else 0; FREEZE on the PC writes here
\      sectors 145-162  two FATs: an empty FAT12 file system
\      sectors 163-...  its root folder and the free space
\    The same file goes onto a floppy disk, a USB stick or a hard disk
\    (doc/disks.md).
\ The image starts with BOOT, or with ON-BOOT when the image has this word.

ONLY FORTH DEFINITIONS  ALSO FILES  S" lib/text.fs" INCLUDED
ONLY FORTH  ALSO FILES  S" lib/ihex.fs" INCLUDED
ONLY FORTH  ALSO FILES  S" host/image.fs" INCLUDED
ONLY FORTH  ALSO FILES  S" asm/8086.fs" INCLUDED
ONLY FORTH  ALSO FILES  ALSO TEXT  ALSO IHEX  ALSO IMAGE  ALSO I8086  ALSO INTERNALS
VOCABULARY PC-BUILD  ALSO PC-BUILD DEFINITIONS

\ ==== The disk and the memory ===========================================

$0800 CONSTANT CODESEG             \ the code: physical address $8000
$1000 CONSTANT VMSEG               \ the VM: physical addresses $10000 to $1FFFF
16 CONSTANT CODE-SECTORS           \ the code on the disk: sectors 1 to 16
17 CONSTANT VM-LBA                 \ the memory of the VM: sectors 17 to 144
VM-LBA 128 + CONSTANT RESERVED     \ the sectors before the first FAT
2880 CONSTANT DISK-SECTORS

\ ==== The boot sector ===================================================

$7C00 ORG
S" targets/pc-bios/bootsect.fs" INCLUDED
CHECK-LABELS
: CHECK-BOOT ( -- )
  B-START $7C3E <> ABORT" B-START must be at $7C3E: the jump at byte 0 goes there"
  THERE $7DBE U> ABORT" the boot sector is too long" ;
CHECK-BOOT
CREATE BOOT-SECTOR 512 ALLOT
BOOT-SECTOR 512 0 FILL  TARGET BOOT-SECTOR THERE $7C00 - MOVE
$55 BOOT-SECTOR 510 + C!  $AA BOOT-SECTOR 511 + C!   \ the mark of a boot sector

\ ==== The code ==========================================================

0 ORG
S" kernels/8086.fs" INCLUDED
S" targets/pc-bios/hal.fs" INCLUDED
FILL-TABLE  CHECK-LABELS
TDP @ CONSTANT CODE-SIZE

: START-WORDS ( -- )   \ the xts and addresses that the code needs
  IMAGE-OPEN
  S" ON-BOOT" FIND-NAME ?DUP IF NAME>XT BOOT-XT ! THEN   \ the start word
  BOOT-XT @ XT-BOOT T!  QUIT-XT @ XT-QUIT T!  THROW-XT @ XT-THROW T!
  IMAGE-CLOSE
  ECHO ECHO-A T!  OUT OUT-A T! ;   \ the image has the addresses of this system
START-WORDS

\ ==== build/ucf-pc.img ==================================================
\ The file has more than 64 KB, and a cell has 16 bits: so the parts go
\ out as sectors, and no number holds a place in the file.

CREATE SECTOR 512 ALLOT
: PUT-SECTOR ( -- )   SECTOR 512 PUT ;
: EMPTY-SECTOR ( -- )   SECTOR 512 0 FILL ;
: PUT-EMPTY ( n -- )   EMPTY-SECTOR  0 ?DO PUT-SECTOR LOOP ;

VARIABLE CUR                       \ the sector of the VM in SECTOR now: 0 to 127
VARIABLE IMAGE-BYTES
: NEXT-SECTOR ( -- )   PUT-SECTOR  EMPTY-SECTOR  1 CUR +! ;
: VM-BYTE ( c a -- )   \ the byte c at the VM address a
  DUP 9 RSHIFT CUR @ < ABORT" the records of ucf.hex are not in the order of their addresses"
  BEGIN  DUP 9 RSHIFT CUR @ > WHILE  NEXT-SECTOR  REPEAT
  511 AND SECTOR + C! ;
: PUT-VM ( -- )   \ the memory of the VM, 128 sectors: the image at its addresses, else 0
  0 CUR !  0 IMAGE-BYTES !  EMPTY-SECTOR  IMAGE-OPEN
  BEGIN IMAGE-RECORD WHILE                             ( n addr )
    OVER IMAGE-BYTES +!
    SWAP 0 ?DO  IHEX-BUF I + C@  OVER I + VM-BYTE  LOOP  DROP
  REPEAT  IMAGE-CLOSE
  BEGIN  CUR @ 128 < WHILE  NEXT-SECTOR  REPEAT ;
: PUT-FAT ( -- )   \ an empty FAT12 of 9 sectors: the media byte, then each cluster free
  EMPTY-SECTOR  $F0 SECTOR C!  $FF SECTOR 1+ C!  $FF SECTOR 2 + C!  PUT-SECTOR
  8 PUT-EMPTY ;

: WRITE-DISK ( -- )
  S" build/ucf-pc.img" WRITE-START
  BOOT-SECTOR 512 PUT                                  \ sector 0
  TARGET CODE-SIZE PUT  VM-LBA 512 * PUT-ZEROS          \ the code, then 0 up to sector 17
  PUT-VM                                               \ sectors 17-144
  PUT-FAT  PUT-FAT                                     \ sectors 145-162
  DISK-SECTORS RESERVED - 18 - PUT-EMPTY               \ the root folder and the free space
  WRITE-END ;

: BUILD ( -- )
  WRITE-DISK
  ."   code: " CODE-SIZE . ." bytes, image: " IMAGE-BYTES @ . ." bytes" CR
  ."   written: build/ucf-pc.img (" DISK-SECTORS 0 .R ."  sectors, 1440 KB)" CR ;

BUILD
