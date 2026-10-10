\ bootimg.fs - package BOOTIMG: FREEZE on the board writes the boot image to the flash
\
\ The ROM bootloader starts the image at flash address 0, in the format
\ of targets/esp32s3/README.md. On the board, FREEZE runs SAVE-IMAGE
\ (HAL 85). With this package in the image, the kernel then runs
\ (SAVE-IMAGE) ( a u entry -- ior ): a u is the code of the kernel (a is
\ its address on the data bus; the CPU runs it at a + $6F0000), entry
\ its start address. (SAVE-IMAGE) writes four segments, as build.fs:
\
\   the code of the kernel
\   the RAM data: VM addresses $100 up to VP
\   the dictionary: VM addresses S0 up to HERE
\   the code again, at its address on the data bus (for QEMU)
\
\ Then it reads the flash back and puts the checksum and the SHA-256
\ digest of these bytes at the end. (The words of the save change their
\ own variables, which are in the RAM data: so the digest must come from
\ the bytes in the flash, not from the RAM.) The digest comes from the
\ SHA unit of the chip, over the functions of the ROM (ets_sha_...): SHA-256
\ in Forth takes 1.8 s for 8 KB at 40 MHz. After a reset, the board
\ starts the new image.
\
\ The RAM data in the PSRAM (package PSRAM: XVP of core.fs) go to the
\ flash too, before the image: at XFLASH (512 KB) a header of 64 bytes
\ (the mark "XDAT", the length, the address, the SHA-256 digest), then
\ the bytes. XDATA-LOAD puts them back into the PSRAM after a start
\ (targets/esp32s3/boot.fs). The bytes go in parts of 4 KB through a
\ buffer inside the chip: the flash and the PSRAM share the memory SPI
\ (package FLASH).
\
\ build.fs tells the kernel where (SAVE-IMAGE) is: put this file and the
\ packages it needs before targets/freeze.fs on the command line
\ (targets/esp32s3/README.md).
\
\ Careful: FREEZE erases the old image first. If the power fails during
\ FREEZE, the board does not start; then load an image with "ucf flash"
\ (the download mode of the ROM always works).

PACKAGE BOOTIMG  ALSO ROM  ALSO FLASH  ALSO PSRAM   \ lib/package.fs

$6F0000 CONSTANT >IRAM              \ data bus address + this = the same byte for the CPU
256 BUFFER: BUF
VARIABLE FPOS                       \ the next address in the flash
VARIABLE SUM                        \ the checksum: XOR of the bytes of the segments, from $EF
VARIABLE CODE-A   VARIABLE CODE-U   \ the code of the kernel
VARIABLE FA   VARIABLE FU           \ the part of the flash that HASH reads
256 BUFFER: CTX                     \ SHA_CTX of the ROM (216 bytes)
32 BUFFER: DIGEST

\ ---- SHA-256 with the SHA unit, over the ROM ----------------------------

$40001CEC CONSTANT 'SHA-ENABLE      \ ets_sha_enable ( -- )
$40001CF8 CONSTANT 'SHA-DISABLE     \ ets_sha_disable ( -- )
$40001D10 CONSTANT 'SHA-INIT        \ ets_sha_init ( ctx type -- r ); type 2: SHA-256
$40001D34 CONSTANT 'SHA-UPDATE      \ ets_sha_update ( ctx a u update-ctx -- )
$40001D40 CONSTANT 'SHA-FINISH      \ ets_sha_finish ( ctx digest -- r )
: DIGEST-START ( -- )
  0 'SHA-ENABLE SYSCALL DROP  CTX >CHIP 2  2 'SHA-INIT SYSCALL DROP ;
: DIGEST-ADD ( a u -- )
  >R >CHIP  CTX >CHIP SWAP  R> 0  4 'SHA-UPDATE SYSCALL DROP ;
: DIGEST-END ( -- a )   \ 32 bytes at a
  CTX >CHIP DIGEST >CHIP  2 'SHA-FINISH SYSCALL DROP
  0 'SHA-DISABLE SYSCALL DROP  DIGEST ;

: 4ALIGNED ( u -- u' )   3 + -4 AND ;
: DATA-U ( -- u )   VP @ $100 - 4ALIGNED ;
: DICT-U ( -- u )   HERE S0 - 4ALIGNED ;         \ the dictionary starts at S0 (doc/vm.md 3)
: SIZE ( -- u )     56 CODE-U @ 2* + DATA-U + DICT-U + ;   \ up to the checksum: header 24, 4 x 8
: PAD# ( -- n )     15 SIZE - 15 AND ;           \ zeros: the checksum ends a block of 16

\ ---- 1. write the segments ----------------------------------------------

: >FLASH ( a u -- )   \ to the flash at FPOS; a and u are multiples of 4
  DUP >R  FPOS @ FLASH-WRITE THROW  R> FPOS +! ;
: IMAGE-HEAD ( entry -- )   \ as build.fs: 4 segments, DIO, 16 MB, 80 MHz, ESP32-S3, a digest
  BUF 24 0 FILL  BUF 4 + !
  $E9 BUF C!  4 BUF 1+ C!  2 BUF 2 + C!  $4F BUF 3 + C!
  $EE BUF 8 + C!  9 BUF 12 + C!  $FF BUF 17 + C!  $FF BUF 18 + C!  1 BUF 23 + C!
  BUF 24 >FLASH ;
: SEGMENT ( a u ta -- )   \ the address on the chip and the length, then the bytes from a
  BUF !  DUP BUF CELL+ !  BUF 8 >FLASH  >FLASH ;
: WRITE ( entry -- )
  0 FPOS !  IMAGE-HEAD
  CODE-A @ CODE-U @  OVER >IRAM +  SEGMENT
  $100 >CHIP DATA-U  OVER SEGMENT
  S0 >CHIP DICT-U  OVER SEGMENT
  CODE-A @ CODE-U @  OVER SEGMENT ;

\ ---- 2. the checksum and the digest of the bytes in the flash ----------

: +SUM ( a u -- )   OVER + SWAP ?DO  I C@ SUM @ XOR SUM !  LOOP ;
: CHUNK ( -- n )   FU @ 256 MIN ;
: HASH ( faddr u data? -- )   \ read the flash; into the digest, and data? into the checksum
  >R  FU !  FA !
  BEGIN FU @ WHILE
    FA @ BUF CHUNK FLASH-READ THROW
    BUF CHUNK DIGEST-ADD
    R@ IF BUF CHUNK +SUM THEN
    CHUNK DUP FA +!  NEGATE FU +!
  REPEAT  R> DROP ;
: PART ( u data? -- )   OVER >R  FPOS @ ROT ROT HASH  R> FPOS +! ;
: CHECK ( -- )
  0 FPOS !  $EF SUM !  DIGEST-START
  32 FALSE PART  CODE-U @ TRUE PART
  8 FALSE PART   DATA-U TRUE PART
  8 FALSE PART   DICT-U TRUE PART
  8 FALSE PART   CODE-U @ TRUE PART ;
: TAIL ( -- )   \ zeros, the checksum, the digest of all bytes before it
  BUF 64 0 FILL  SUM @ BUF PAD# + C!
  BUF PAD# 1+ DIGEST-ADD  DIGEST-END  BUF PAD# 1+ + 32 MOVE
  BUF PAD# 33 +  SIZE FLASH-WRITE THROW ;

\ ---- 3. the RAM data in the PSRAM -----------------------------------------

$80000 CONSTANT XFLASH              \ the flash address of the saved PSRAM data
$54414458 CONSTANT MARK             \ "XDAT"
: XDATA-U ( -- u )   \ the bytes of RAM data in the PSRAM
  XDATA-END @ 0= IF 0 EXIT THEN  XVP @ PSRAM-ADDR -  DUP PSRAM-SIZE U< AND  4ALIGNED ;
4096 BUFFER: XBUF                   \ in the RAM inside the chip (this package loads before PSRAM-ON)
VARIABLE XPOS
: XCHUNK ( u -- n )   XPOS @ - 4096 MIN ;
: XSAVE ( u -- )   \ u bytes from the PSRAM to the flash at XFLASH + 64
  0 XPOS !  BEGIN DUP XPOS @ > WHILE
    DUP XCHUNK >R
    PSRAM-ADDR XPOS @ +  XBUF R@ MOVE
    XBUF R@  XFLASH 64 + XPOS @ +  FLASH-WRITE THROW
    R> XPOS +!
  REPEAT DROP ;
: XLOAD ( u -- )   \ u bytes from the flash at XFLASH + 64 to the PSRAM
  0 XPOS !  BEGIN DUP XPOS @ > WHILE
    DUP XCHUNK >R
    XFLASH 64 + XPOS @ +  XBUF R@  FLASH-READ THROW
    XBUF  PSRAM-ADDR XPOS @ +  R@ MOVE
    R> XPOS +!
  REPEAT DROP ;
: SAVE-XDATA ( -- )
  XFLASH  64 XDATA-U +  FLASH-ERASE THROW       \ also without data: no old header stays
  XDATA-U ?DUP 0= IF EXIT THEN  >R
  R@ XSAVE
  DIGEST-START  XFLASH 64 + R@ FALSE HASH  DIGEST-END
  BUF 64 0 FILL  MARK BUF !  R> BUF CELL+ !  PSRAM-ADDR BUF 2 CELLS + !
  BUF 16 + 32 MOVE  BUF 64 XFLASH FLASH-WRITE THROW ;
: SAME? ( a1 a2 u -- f )   \ u bytes, a multiple of 4
  0 ?DO  OVER I + @  OVER I + @  <> IF 2DROP FALSE UNLOOP EXIT THEN  4 +LOOP  2DROP TRUE ;

: (SAVE) ( entry -- )
  SAVE-XDATA
  0  SIZE PAD# + 33 +  FLASH-ERASE THROW
  WRITE  CHECK  TAIL ;

PUBLIC
: (SAVE-IMAGE) ( a u entry -- ior )   \ SAVE-IMAGE (HAL 85) of the board
  >R  CODE-U !  CODE-A !  R>  ['] (SAVE) CATCH  DUP IF NIP THEN ;
: XDATA-LOAD ( -- )   \ after PSRAM-ON: the RAM data in the PSRAM, as FREEZE saved them
  XDATA-U ?DUP 0= IF EXIT THEN
  XFLASH BUF 64 FLASH-READ THROW
  BUF CELL+ @ OVER =  BUF @ MARK = AND  BUF 2 CELLS + @ PSRAM-ADDR = AND
  0= IF DROP ." no saved data for the PSRAM" CR EXIT THEN
  >R  R@ XLOAD
  DIGEST-START  PSRAM-ADDR R> DIGEST-ADD  DIGEST-END
  BUF 16 + 32 SAME? 0= IF ." the saved data for the PSRAM are damaged" CR THEN ;

END-PACKAGE
