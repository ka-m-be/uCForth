\ build.fs - build uCForth for the ESP32: build/esp32.bin
\
\ Use (in the folder ucf), with a bootstrap VM with 32-bit cells:
\   build/ucf-c -32 lib/package.fs [PACKAGES ...] [APP ...] targets/freeze.fs host/files.fs \
\       targets/esp32/build.fs </dev/null
\
\ 1. targets/freeze.fs: the running system is the image: base and the
\    files before it, without the package FILES. It is in ucf.hex. The
\    image starts with BOOT, or with ON-BOOT when it has this word.
\ 2. The assembler (asm/xtensa.fs) makes the kernel (kernels/xtensa.fs)
\    and the HAL (hal.fs) at $4008_0000, in SRAM0: the RAM of the
\    instruction bus. The CPU reads the code there in cells of 32 bits
\    (the table of the primitives too), so >DRAM is 0.
\ 3. The tool writes build/esp32.bin in the format of the ROM bootloader,
\    with three segments: the code; VM addresses $100 up to the end of
\    the RAM data; the dictionary from S0. The ESP32 has no free block of
\    256 KB (the ROM keeps its data at $3FFE_0000), so the memory of the
\    VM lies in two parts (targets/esp32/README.md, "Memory"): VM
\    addresses 0 to $FFFF (SYS and the RAM data) at DATA-ADDR + a, in
\    SRAM1; VM addresses $10000 to $3FFFF (TIB, PAD, the stacks, the
\    dictionary) at RAM-ADDR + a, in SRAM2. The map stays the standard
\    map of doc/vm.md, section 3. DATA-OFF tells the assembler (>CHIP,)
\    and the kernel. At the end: the checksum, then the SHA-256 digest of
\    all bytes before it.
\
\ The image goes to the flash at $1000: "ucf flash PORT build/esp32.bin
\ 0x1000". "ucf run PORT build/esp32.bin" loads it into the RAM and
\ starts it.

ONLY FORTH DEFINITIONS  ALSO FILES  S" lib/text.fs" INCLUDED
ONLY FORTH  ALSO FILES  S" lib/ihex.fs" INCLUDED
ONLY FORTH  ALSO FILES  S" host/image.fs" INCLUDED
ONLY FORTH  ALSO FILES  [DEFINED] SHA256 S" lib/sha256.fs" ?INCLUDED
ONLY FORTH  ALSO FILES  S" asm/xtensa.fs" INCLUDED
ONLY FORTH  ALSO FILES  ALSO IHEX  ALSO IMAGE  ALSO SHA256  ALSO XTENSA  ALSO XTENSA.I  ALSO INTERNALS
VOCABULARY E32-BUILD  ALSO E32-BUILD DEFINITIONS

$40080000 CONSTANT CODE-ADDR        \ SRAM0, the RAM of the instruction bus
$3FFA0000 CONSTANT RAM-ADDR         \ VM-BASE in kernels/xtensa.fs: VM address $10000 is at $3FFB0000 (SRAM2)
$3FFE8000 CONSTANT DATA-ADDR        \ VM addresses 0 to $FFFF: SYS and the RAM data (SRAM1)
$3F400000 CONSTANT DROM-ADDR        \ the frozen part in the flash, for the CPU: DROM-ADDR + VM address
$60000 CONSTANT DICT-FLASH          \ the same in the flash: DICT-FLASH + VM address (pages $70000 to $9FFFF)
0 CONSTANT >DRAM                    \ the code is read where it is
DATA-ADDR RAM-ADDR - DATA-OFF !     \ for >CHIP, and the kernel (asm/xtensa.fs)
FLASH-DICT? ON!                    \ the frozen part of the dictionary can lie in the flash
: VM>CHIP ( a -- a' )   DUP $10000 U< IF DATA-ADDR ELSE RAM-ADDR THEN + ;

\ ==== The code ==========================================================

CODE-ADDR ORG
S" kernels/xtensa.fs" INCLUDED
S" targets/esp32/hal.fs" INCLUDED
: CHECK-SLOTS ( -- )   \ slots 120 to 122 of the table hold no primitive (asm/xtensa.fs)
  PRIMS FE-SLOT + @  PRIMS FM-SLOT + @ OR  PRIMS S0-SLOT + @ OR
  ABORT" a primitive has a number from 120 to 122 (asm/xtensa.fs)" ;
FILL-TABLE  CHECK-LABELS  CHECK-SLOTS
S0 TABLE FE-SLOT + T!               \ the end of the frozen part in the flash: S0, none yet
DROM-ADDR TABLE FM-SLOT + T!        \ (FREEZE on the board moves the end, lib/esp32/esp32/bootimg.fs)
S0 TABLE S0-SLOT + T!

\ ==== The image =========================================================

: UMIN ( u1 u2 -- u )   2DUP U< IF DROP ELSE NIP THEN ;
VARIABLE D-END    VARIABLE DICT-START   VARIABLE DICT-END'
: RANGES ( -- )   \ the RAM data: $100 to D-END; the dictionary: DICT-START to DICT-END'
  0 D-END !  -1 DICT-START !  0 DICT-END' !  IMAGE-OPEN
  BEGIN IMAGE-RECORD WHILE                          ( n addr )
    DUP DATA-END U< IF  + D-END @ UMAX D-END !
    ELSE  DUP DICT-START @ UMIN DICT-START !  + DICT-END' @ UMAX DICT-END' !  THEN
  REPEAT  IMAGE-CLOSE ;
: 4ALIGNED ( u -- u' )   3 + -4 AND ;

VARIABLE SUM   VARIABLE SUMMING
: SUM-PUT ( a u -- )   \ ON-PUT: SHA-256 of all bytes; the checksum of the segments
  2DUP SHA-ADD  SUMMING @ 0= IF 2DROP EXIT THEN
  OVER + SWAP ?DO  I C@ SUM @ XOR SUM !  LOOP ;

CREATE LE4 4 ALLOT
: LE, ( x -- )   \ 4 bytes, the low byte first
  4 0 DO  DUP LE4 I + C!  8 RSHIFT  LOOP  DROP  LE4 4 PUT ;

CREATE HEAD 24 ALLOT
: HEADER ( entry -- )   \ the 24 bytes at the start (targets/esp32/README.md)
  HEAD 24 0 FILL  HEAD 4 + !         \ the start address (the host is little endian)
  $E9 HEAD C!  3 HEAD 1+ C!         \ the mark, three segments
  2 HEAD 2 + C!  $20 HEAD 3 + C!    \ SPI mode DIO; 4 MB flash, SPI clock 40 MHz
  $EE HEAD 8 + C!                   \ no WP pin
  0 HEAD 12 + C!                    \ the chip: ESP32
  $FF HEAD 17 + C!  $FF HEAD 18 + C!    \ every revision of the chip
  1 HEAD 23 + C!                    \ a SHA-256 digest follows at the end
  HEAD 24 PUT ;

: FROZEN? ( xt -- xt | 0 )   DUP FROZEN-END @ U< AND ;   \ only a word of the image
: ON-BOOT? ( -- xt | 0 )   S" ON-BOOT" FIND-NAME DUP IF NAME>XT FROZEN? THEN ;
: IN-PACKAGE ( a u a2 u2 -- xt | 0 )   \ the word a u of the package a2 u2 in the image, or 0
  FIND-NAME DUP 0= IF NIP NIP EXIT THEN
  ALSO NAME>XT EXECUTE  FIND-NAME  PREVIOUS
  DUP IF  NAME>XT FROZEN?  THEN ;
: SAVER ( -- xt | 0 )      S" (SAVE-IMAGE)" S" BOOTIMG" IN-PACKAGE ;
: ISR-XT ( -- xt | 0 )     S" (ISR)" S" INTR" IN-PACKAGE ;
: FAULT-XT ( -- xt | 0 )   S" (FAULT)" S" INTR" IN-PACKAGE ;
: FAULT-REGS-AT ( -- a | 0 )   \ the chip address of FAULT-REGS of the package INTR
  S" FAULT-REGS" S" INTR" IN-PACKAGE  DUP IF  EXECUTE VM>CHIP  THEN ;
: HOLD-ADDR ( -- a | 0 )   \ the address of the variable NO-BREAK of the package IDLE
  S" NO-BREAK" S" IDLE" IN-PACKAGE  DUP IF EXECUTE THEN ;

: CODE-START-AT ( -- a | 0 )   \ the value of the constant CODE-START of the package BOOTIMG
  S" CODE-START" S" BOOTIMG" IN-PACKAGE  DUP IF CELL+ THEN ;
: FE-AT-AT ( -- a | 0 )    S" FE-AT" S" BOOTIMG" IN-PACKAGE  DUP IF CELL+ THEN ;
: FE-AT2-AT ( -- a | 0 )   S" FE-AT2" S" BOOTIMG" IN-PACKAGE  DUP IF CELL+ THEN ;
: CHECK-FLASH ( -- )   \ BOOTIMG puts the dictionary where the kernel maps it
  S" DICT-FLASH" S" BOOTIMG" IN-PACKAGE ?DUP IF
    EXECUTE DICT-FLASH <> ABORT" DICT-FLASH of BOOTIMG differs from build.fs"  THEN ;
: PATCH-CELL ( n addr at x -- n addr )   \ the cell at VM address at gets x, if the record has it
  >R  >R  2DUP R> SWAP -  DUP ROT U< IF  IHEX-BUF + R> SWAP !  ELSE  DROP R> DROP  THEN ;
: PATCH ( n addr -- )   \ a record of the image in IHEX-BUF: CODE-START, FE-AT, FE-AT2 for this target
  CODE-START-AT ?DUP IF  CODE-ADDR TDP @ 4ALIGNED +  PATCH-CELL  THEN
  FE-AT-AT ?DUP IF  TABLE FE-SLOT +  PATCH-CELL  THEN
  FE-AT2-AT ?DUP IF  TABLES-ADDR FE-SLOT +  PATCH-CELL  THEN  2DROP ;
VARIABLE SEG-POS   VARIABLE SEG-LO   VARIABLE SEG-LEN
: VM-SEGMENT ( lo hi -- )   \ the image from VM address lo to hi as one segment
  OVER - 4ALIGNED SEG-LEN !  DUP SEG-LO !
  VM>CHIP LE,  SEG-LEN @ LE,
  WRITTEN SEG-POS !  SUMMING ON!
  IMAGE-OPEN
  BEGIN IMAGE-RECORD WHILE                          ( n addr )
    2DUP PATCH
    SEG-LO @ -  DUP SEG-LEN @ U< IF  SEG-POS @ + PUT-ZEROS  IHEX-BUF SWAP PUT  ELSE 2DROP THEN
  REPEAT  IMAGE-CLOSE
  SEG-POS @ SEG-LEN @ + PUT-ZEROS  SUMMING OFF! ;

: WRITE-BIN ( -- )
  CHECK-FLASH
  IMAGE-OPEN  ON-BOOT? ?DUP IF BOOT-XT ! THEN  BOOT-XT @ XT-BOOT T!  QUIT-XT @ XT-QUIT T!  THROW-XT @ XT-THROW T!
  IMAGE-CLOSE  RANGES
  SAVER XT-SAVE T!  TDP @ 4ALIGNED CODE-LEN T!  START ENTRY-A T!
  ISR-XT XT-ISR T!  FAULT-XT XT-FAULT T!  FAULT-REGS-AT FREGS-A T!
  OUT DATA-OFF @ + OUT-V T!  HOLD-ADDR HOLD-V T!   \ OUT: the code adds MEM
  S" build/esp32.bin" WRITE-START  ['] SUM-PUT IS ON-PUT  $EF SUM !  SUMMING OFF!
  SHA-START
  START HEADER
  CODE-ADDR LE,  TDP @ 4ALIGNED LE,
  SUMMING ON!  TARGET TDP @ 4ALIGNED PUT  SUMMING OFF!
  $100 D-END @ VM-SEGMENT
  DICT-START @ DICT-END' @ VM-SEGMENT
  WRITTEN 15 OR PUT-ZEROS                           \ the next byte ends a block of 16
  SUM @ LE4 C!  LE4 1 PUT
  ['] 2DROP IS ON-PUT  SHA-END 32 PUT  WRITE-END ;

: BUILD ( -- )
  WRITE-BIN
  ."   code: " TDP @ . ." bytes at $" CODE-ADDR .HEX ." , start at $" START .HEX CR
  ."   image: $100-$" D-END @ .HEX ."  at $" $100 VM>CHIP .HEX
  ." , $" DICT-START @ .HEX ." -$" DICT-END' @ .HEX ."  at $" DICT-START @ VM>CHIP .HEX CR
  ."   start word: " ON-BOOT? IF ." ON-BOOT" ELSE ." BOOT" THEN CR
  ."   dictionary: up to $" DICT-END .HEX ."  (DICT-END)" CR
  ."   FREEZE on the board: " SAVER IF ." yes" ELSE ." no (without lib/esp32/esp32/bootimg.fs)" THEN CR
  ."   interrupts: " ISR-XT IF ." yes" ELSE ." no (without lib/esp32/intr.fs)" THEN CR
  ."   Ctrl-C waits for NO-BREAK: " HOLD-ADDR IF ." yes" ELSE ." no (without lib/idle.fs)" THEN CR
  ."   written: build/esp32.bin (" WRITTEN 0 .R ."  bytes)" CR ;

BUILD
