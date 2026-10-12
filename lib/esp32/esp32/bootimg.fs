\ bootimg.fs - package BOOTIMG: FREEZE on the board writes the frozen part and the boot image to the flash (ESP32)
\
\ The ROM bootloader of the ESP32 starts the image at flash address
\ $1000, in the format of targets/esp32/README.md. On the board, FREEZE
\ runs SAVE-IMAGE (HAL 85). With this package in the image, the kernel
\ then runs (SAVE-IMAGE) ( a u entry -- ior ): a u is the code of the
\ kernel (in SRAM0, at its address on the instruction bus), entry its
\ start address.
\
\ The frozen part of the dictionary lies in the flash, from S0 up to
\ FLASH-END: VM address a at flash address DICT-FLASH + a, in the pages
\ $7_0000 to $9_FFFF. The kernel reads it through the cache
\ (targets/esp32/README.md, "Memory"). The boot image must end before
\ $7_0000 (444 KB from $1000; FREEZE checks it). An image of build.fs
\ has FLASH-END = S0: all of its dictionary is in the RAM. (SAVE-IMAGE)
\ has three steps:
\
\ 1. The dictionary from FLASH-END up to HERE goes to the flash after
\    the frozen part; then FLASH-END moves to HERE (to the cell that
\    holds HERE, when HERE is not a multiple of 4). This step only
\    writes: the flash after FLASH-END is erased. Only when FLASH-END is
\    S0, it first erases the flash of the whole dictionary (192 KB). It
\    reads the new bytes back through the cache, before FLASH-END moves.
\    (Each call of the ROM for the flash flushes the cache: FLASH-SYSCALL.
\    One read more before the check: in QEMU the flash file gets the
\    bytes a little later, and a flush right after the write can still
\    find the old bytes.)
\ 2. With XDATA-ON, the end of the RAM data behind the frozen part
\    moves with FLASH-END.
\ 3. The boot image, with these segments:
\      the code of the kernel, and the code of the CODE words after it
\      the RAM data: VM addresses $100 up to VP
\      the dictionary from FLASH-END up to HERE (0 to 3 bytes)
\      with XDATA-ON: the RAM data behind the frozen part
\    Then it reads the flash back and puts the checksum of these bytes
\    at the end. (The words of the save change their own variables,
\    which are in the RAM data: so the checksum must come from the bytes
\    in the flash, not from the RAM.) The ROM of the ESP32 does not check
\    a SHA-256 digest at the end, so the image has none (byte 23 of the
\    header is 0).
\
\ The system runs on after FREEZE. A store into the frozen part gives
\ -9 (the kernel: >CHIP!, of asm/xtensa.fs).
\
\ The RAM behind the frozen part is free. XDATA-ON gives it to the RAM
\ data (core.fs: XVP, XDATA-END): VARIABLE, VALUE, BUFFER: and DEFER
\ then take their memory there (from S0 + 32 up to FLASH-END in the RAM)
\ and no longer in the RAM data at VP. FREEZE saves the setting:
\
\   ALSO BOOTIMG  FREEZE  XDATA-ON  FREEZE
\
\ XDATA-FREEZE does the first two steps, but only while XDATA is still off:
\ so a loader can name it, and a second run of the loader does nothing.
\ ?FREEZE freezes only when the dictionary has grown since the last FREEZE.
\
\ The code of the CODE words lies in SRAM0 after the code of the kernel
\ (lib/esp32/esp32/asm.fs):
\
\   CODE-START ( -- a )   the end of the code of the kernel: there the
\                         code of the CODE words starts (build.fs puts
\                         the value into the image)
\   CODE-HERE ( -- a )    a variable: the end of the code of the CODE
\                         words; 0: none yet
\
\ The CPU reads SRAM0 only in cells: the code goes to the flash through a
\ buffer in the RAM for data. After a reset, the board starts the new
\ image.
\
\ build.fs tells the kernel where (SAVE-IMAGE) is: put this file and the
\ packages it needs before targets/freeze.fs on the command line
\ (targets/esp32/README.md).
\
\ Careful: FREEZE erases the old boot image after step 2. If the power
\ fails then, the board does not start; then load an image with "ucf
\ flash" (the download mode of the ROM always works). A power failure in
\ step 1 or 2 leaves the old image as it was, with one exception: when
\ FLASH-END is S0 (after "ucf run", for example) but the boot image in
\ the flash has its frozen part in the flash, step 1 erases that boot
\ image first, then the flash of the dictionary.

PACKAGE BOOTIMG  ALSO ROM  ALSO FLASH   \ lib/package.fs

PUBLIC
0 CONSTANT CODE-START               \ build.fs: the end of the code of the kernel in SRAM0
VARIABLE CODE-HERE                  \ the end of the code of the CODE words; 0: none
0 CONSTANT FE-AT                    \ build.fs: slot 120 of the table of the primitives, FE (asm/xtensa.fs)
0 CONSTANT FE-AT2                   \ build.fs: the same slot in its copy for TBL (SRAM1, targets/esp32/hal.fs)
$60000 CONSTANT DICT-FLASH          \ the frozen part in the flash: VM address a at DICT-FLASH + a
: FLASH-END ( -- a )   FE-AT2 @ ;   \ the end of the frozen part in the flash; S0: none
PRIVATE
$1000 CONSTANT FBASE                \ the boot image in the flash
256 BUFFER: BUF
VARIABLE FPOS                       \ the next address in the flash
VARIABLE SUM                        \ the checksum: XOR of the bytes of the segments, from $EF
VARIABLE CODE-A   VARIABLE CODE-U   \ the code of the kernel
VARIABLE FA   VARIABLE FU           \ the part of the flash that HASH or DICT>FLASH works on

: 4ALIGNED ( u -- u' )   3 + -4 AND ;
: RAM-BASE ( -- a )   $10000 >CHIP $10000 - ;      \ VM address a of the dictionary in the RAM: RAM-BASE + a
: DROM ( a -- a' )   FE-AT2 CELL+ @ + ;              \ and in the flash, for the CPU (slot 121)
: XSTART ( -- a )   S0 32 + RAM-BASE + ;             \ the RAM behind the frozen part, after the 8 cells of the guard
: FLASH-END! ( a -- )   \ the table in the code (for the image), its copy, the copy of the break table
  DUP FE-AT !  DUP FE-AT2 !  FE-AT2 512 + ! ;

\ ---- 1. the dictionary to the flash ---------------------------------------

: BLANK? ( a u -- f )   \ VM addresses a to a + u in the flash: all bytes $FF?
  OVER + SWAP ?DO  I DROM @ -1 <> IF FALSE UNLOOP EXIT THEN  4 +LOOP  TRUE ;
: SAME? ( a u -- f )   \ VM addresses a to a + u: the same bytes in the flash and in the RAM?
  OVER + SWAP ?DO  I DROM @  I RAM-BASE + @  <> IF FALSE UNLOOP EXIT THEN  4 +LOOP  TRUE ;
: OLD-FROZEN? ( -- f )   \ the boot image in the flash has a frozen part there (its slot 120 is not S0)
  FBASE 512 + BUF 4 FLASH-READ THROW  BUF @ S0 <> ;   \ header 24, segment 8, then the table
: NEW-START ( -- )   \ erase the flash of VM addresses $10000 to $3FFFF
  OLD-FROZEN? IF  FBASE 4096 FLASH-ERASE THROW  THEN   \ that boot image needs this flash: it goes first
  DICT-FLASH $10000 +  $30000  FLASH-ERASE THROW ;
: DICT>FLASH ( -- )   \ the dictionary from FLASH-END up to HERE; FLASH-END moves
  FLASH-END FA !  HERE -4 AND  FA @ -  FU !
  FU @ 1 < IF EXIT THEN
  FA @ S0 = IF NEW-START THEN
  FA @ FU @ BLANK? 0= ABORT" FREEZE: the flash after the frozen part is not erased"
  FA @ RAM-BASE +  FU @  DICT-FLASH FA @ +  FLASH-WRITE THROW
  DICT-FLASH FA @ +  BUF 4 FLASH-READ THROW   \ the cache once more, for QEMU (see above)
  FA @ FU @ SAME? 0= ABORT" FREEZE: the flash has other bytes than the RAM"
  FA @ FU @ + FLASH-END! ;

\ ---- 2. the RAM data behind the frozen part ------------------------------

: XDATA-U ( -- u )   \ the bytes of RAM data behind the frozen part
  XDATA-END @ 0= IF 0 EXIT THEN  XVP @ XSTART -  0 MAX 4ALIGNED ;

\ ---- 3. the boot image ----------------------------------------------------

: DATA-U ( -- u )   VP @ $100 - 4ALIGNED ;
: TAIL-U ( -- u )   HERE FLASH-END - 4ALIGNED ;   \ the dictionary after the frozen part
: #SEGMENTS ( -- n )   2  TAIL-U 0<> -  XDATA-U 0<> - ;
: SIZE ( -- u )   \ up to the checksum: header 24, 8 for each segment
  24  #SEGMENTS 8 * +  CODE-U @ +  DATA-U +  TAIL-U +  XDATA-U + ;
: PAD# ( -- n )     15 SIZE - 15 AND ;           \ zeros: the checksum ends a block of 16

: >FLASH ( a u -- )   \ to the flash at FPOS; a and u are multiples of 4
  DUP >R  FPOS @ FLASH-WRITE THROW  R> FPOS +! ;
: IMAGE-HEAD ( entry -- )   \ as build.fs: DIO, 4 MB, 40 MHz, ESP32, no digest
  BUF 24 0 FILL  BUF 4 + !
  $E9 BUF C!  #SEGMENTS BUF 1+ C!  2 BUF 2 + C!  $20 BUF 3 + C!
  $EE BUF 8 + C!  $FF BUF 17 + C!  $FF BUF 18 + C!
  BUF 24 >FLASH ;
: SEGMENT ( a u ta -- )   \ the address on the chip and the length, then the bytes from a
  BUF !  DUP BUF CELL+ !  BUF 8 >FLASH  >FLASH ;
VARIABLE SRC   VARIABLE LEFT
: CELLS>BUF ( n -- )   \ n bytes from SRC to BUF, a cell at a time
  0 ?DO  SRC @ I + @  BUF I + !  4 +LOOP ;
: CELLS>FLASH ( a u -- )   \ u bytes from SRAM0 at a: through BUF
  LEFT !  SRC !
  BEGIN LEFT @ WHILE
    LEFT @ 256 MIN  DUP CELLS>BUF  BUF OVER >FLASH
    DUP SRC +!  NEGATE LEFT +!
  REPEAT ;
: WRITE ( entry -- )
  FBASE FPOS !  IMAGE-HEAD
  CODE-A @ BUF !  CODE-U @ BUF CELL+ !  BUF 8 >FLASH  CODE-A @ CODE-U @ CELLS>FLASH
  $100 >CHIP DATA-U  OVER SEGMENT
  TAIL-U IF  FLASH-END RAM-BASE +  TAIL-U  OVER SEGMENT  THEN
  XDATA-U IF  XSTART XDATA-U  OVER SEGMENT  THEN ;

: +SUM ( a u -- )   OVER + SWAP ?DO  I C@ SUM @ XOR SUM !  LOOP ;
: CHUNK ( -- n )   FU @ 256 MIN ;
: HASH ( faddr u data? -- )   \ read the flash; data?: into the checksum
  >R  FU !  FA !
  BEGIN FU @ WHILE
    FA @ BUF CHUNK FLASH-READ THROW
    R@ IF BUF CHUNK +SUM THEN
    CHUNK DUP FA +!  NEGATE FU +!
  REPEAT  R> DROP ;
: PART ( u data? -- )   OVER >R  FPOS @ ROT ROT HASH  R> FPOS +! ;
: CHECK ( -- )
  FBASE FPOS !  $EF SUM !
  32 FALSE PART  CODE-U @ TRUE PART
  8 FALSE PART   DATA-U TRUE PART
  TAIL-U IF   8 FALSE PART  TAIL-U TRUE PART   THEN
  XDATA-U IF  8 FALSE PART  XDATA-U TRUE PART  THEN ;
: TAIL ( -- )   \ zeros, then the checksum
  BUF 16 0 FILL  SUM @ BUF PAD# + C!
  BUF PAD# 1+ 4ALIGNED  FBASE SIZE + FLASH-WRITE THROW ;

: (SAVE) ( entry -- )
  DICT>FLASH
  XDATA-END @ IF  FLASH-END RAM-BASE + XDATA-END !  THEN
  FBASE SIZE + PAD# + 1+  DICT-FLASH $10000 +  U> ABORT" FREEZE: the boot image is too big"
  FBASE  SIZE PAD# + 1+  FLASH-ERASE THROW
  WRITE  CHECK  TAIL ;

PUBLIC
: (SAVE-IMAGE) ( a u entry -- ior )   \ SAVE-IMAGE (HAL 85) of the board
  >R  CODE-U !  CODE-A !  R>
  CODE-HERE @ ?DUP IF  CODE-A @ -  CODE-U @ MAX  CODE-U !  THEN   \ the CODE words too
  ['] (SAVE) CATCH  DUP IF NIP THEN ;
: XDATA-ON ( -- )   \ the RAM data grow into the RAM behind the frozen part (core.fs: XVP, XDATA-END)
  FLASH-END S0 36 + U< ABORT" XDATA-ON: no frozen part in the flash yet (FREEZE first)"
  FLASH-END RAM-BASE + XDATA-END !
  XVP @ XSTART -  XDATA-END @ XSTART -  U> IF  XSTART XVP !  THEN ;   \ XVP of a frozen image stays
: XDATA-FREEZE ( -- )   \ only the first time: FREEZE, then XDATA-ON (a loader of an application uses it)
  XDATA-END @ IF EXIT THEN  FREEZE XDATA-ON ;
: ?FREEZE ( -- )   \ FREEZE, if the dictionary grew since the last FREEZE
  HERE FROZEN-END @ <> IF FREEZE THEN ;

END-PACKAGE
