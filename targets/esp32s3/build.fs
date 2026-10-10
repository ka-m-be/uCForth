\ build.fs - build uCForth for the ESP32-S3: build/esp32s3.bin
\
\ Use (in the folder ucf), with a bootstrap VM with 32-bit cells:
\   LC_ALL=C awk -f bootstrap/awk/ucf.awk -- -32 lib/package.fs lib/idle.fs lib/esp32/rom.fs lib/esp32/esp32s3/chip.fs \
\       lib/esp32/intr.fs lib/esp32/gpio.fs lib/esp32/timers.fs lib/esp32/flash.fs lib/esp32/esp32s3/clock.fs \
\       lib/esp32/esp32s3/psram.fs lib/esp32/esp32s3/bootimg.fs targets/esp32s3/boot.fs [APP ...] \
\       targets/freeze.fs host/files.fs targets/esp32s3/build.fs </dev/null
\
\ 1. targets/freeze.fs: the running system is the image: base and the
\    files before it, without the package FILES. It is in ucf.hex. With
\    lib/esp32/esp32s3/bootimg.fs in the image, FREEZE works on the board too:
\    SAVE-IMAGE runs (SAVE-IMAGE) of the package BOOTIMG. The image
\    starts with BOOT, or with ON-BOOT when it has this word
\    (targets/esp32s3/boot.fs: 240 MHz, the PSRAM with its RAM data, then BOOT).
\    With lib/esp32/intr.fs in the image, the kernel runs its words
\    (ISR) and (FAULT) for interrupts and exceptions; with lib/idle.fs,
\    Ctrl-C waits while its variable NO-BREAK is not 0.
\ 2. The assembler (asm/xtensa.fs) makes the kernel (kernels/xtensa.fs)
\    and the HAL (hal.fs) at $4037_8000 (IRAM).
\ 3. The tool writes build/esp32s3.bin in the format of the ROM
\    bootloader, with four segments: the code; VM addresses $100 up to
\    the end of the RAM data; the dictionary from S0; the code again, at
\    its address on the data bus. VM address a is at VM-BASE + a in the
\    RAM of the chip. The chip has one RAM on both buses, so the fourth
\    segment writes the same bytes again; QEMU has two memories there,
\    and the kernel reads its table on the data bus (doc/qemu.md). At the
\    end: the checksum, then the SHA-256 digest of all bytes before it
\    (the ROM checks it at the start from the flash).
\
\ "ucf run PORT build/esp32s3.bin" loads it into the RAM and starts it.

ONLY FORTH DEFINITIONS  ALSO FILES  S" lib/text.fs" INCLUDED
ONLY FORTH  ALSO FILES  S" lib/ihex.fs" INCLUDED
ONLY FORTH  ALSO FILES  S" host/image.fs" INCLUDED
ONLY FORTH  ALSO FILES  [DEFINED] SHA256 S" lib/sha256.fs" ?INCLUDED
ONLY FORTH  ALSO FILES  S" asm/xtensa.fs" INCLUDED
ONLY FORTH  ALSO FILES  ALSO IHEX  ALSO IMAGE  ALSO SHA256  ALSO XTENSA  ALSO XTENSA.I  ALSO INTERNALS
VOCABULARY S3-BUILD  ALSO S3-BUILD DEFINITIONS

$40378000 CONSTANT CODE-ADDR        \ IRAM
$3FC90000 CONSTANT RAM-ADDR         \ VM-BASE in kernels/xtensa.fs
$6F0000 CONSTANT >DRAM              \ IRAM address - this = the same byte on the data bus

\ ==== The code ==========================================================

CODE-ADDR ORG
S" kernels/xtensa.fs" INCLUDED
S" targets/esp32s3/hal.fs" INCLUDED
FILL-TABLE  CHECK-LABELS

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
: HEADER ( entry -- )   \ the 24 bytes at the start (targets/esp32s3/README.md)
  HEAD 24 0 FILL  HEAD 4 + !         \ the start address (the host is little endian)
  $E9 HEAD C!  4 HEAD 1+ C!         \ the mark, four segments
  2 HEAD 2 + C!  $4F HEAD 3 + C!    \ SPI mode DIO; 16 MB flash, SPI clock 80 MHz
  $EE HEAD 8 + C!                   \ no WP pin
  9 HEAD 12 + C!                    \ the chip: ESP32-S3
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
  S" FAULT-REGS" S" INTR" IN-PACKAGE  DUP IF  EXECUTE RAM-ADDR +  THEN ;
: HOLD-ADDR ( -- a | 0 )   \ the address of the variable NO-BREAK of the package IDLE
  S" NO-BREAK" S" IDLE" IN-PACKAGE  DUP IF EXECUTE THEN ;

VARIABLE SEG-POS   VARIABLE SEG-LO   VARIABLE SEG-LEN
: VM-SEGMENT ( lo hi -- )   \ the image from VM address lo to hi as one segment
  OVER - 4ALIGNED SEG-LEN !  DUP SEG-LO !
  RAM-ADDR + LE,  SEG-LEN @ LE,
  WRITTEN SEG-POS !  TRUE SUMMING !
  IMAGE-OPEN
  BEGIN IMAGE-RECORD WHILE                          ( n addr )
    SEG-LO @ -  DUP SEG-LEN @ U< IF  SEG-POS @ + PUT-ZEROS  IHEX-BUF SWAP PUT  ELSE 2DROP THEN
  REPEAT  IMAGE-CLOSE
  SEG-POS @ SEG-LEN @ + PUT-ZEROS  FALSE SUMMING ! ;

: WRITE-BIN ( -- )
  IMAGE-OPEN  ON-BOOT? ?DUP IF BOOT-XT ! THEN  BOOT-XT @ XT-BOOT T!  QUIT-XT @ XT-QUIT T!  THROW-XT @ XT-THROW T!
  IMAGE-CLOSE  RANGES
  SAVER XT-SAVE T!  TDP @ 4ALIGNED CODE-LEN T!  START ENTRY-A T!
  ISR-XT XT-ISR T!  FAULT-XT XT-FAULT T!  FAULT-REGS-AT FREGS-A T!
  OUT OUT-V T!  HOLD-ADDR HOLD-V T!
  S" build/esp32s3.bin" WRITE-START  ['] SUM-PUT IS ON-PUT  $EF SUM !  FALSE SUMMING !
  SHA-START
  START HEADER
  CODE-ADDR LE,  TDP @ 4ALIGNED LE,
  TRUE SUMMING !  TARGET TDP @ 4ALIGNED PUT  FALSE SUMMING !
  $100 D-END @ VM-SEGMENT
  DICT-START @ DICT-END' @ VM-SEGMENT
  CODE-ADDR >DRAM - LE,  TDP @ 4ALIGNED LE,          \ the code again, on the data bus
  TRUE SUMMING !  TARGET TDP @ 4ALIGNED PUT  FALSE SUMMING !
  WRITTEN 15 OR PUT-ZEROS                           \ the next byte ends a block of 16
  SUM @ LE4 C!  LE4 1 PUT
  ['] 2DROP IS ON-PUT  SHA-END 32 PUT  WRITE-END ;

: BUILD ( -- )
  WRITE-BIN
  ."   code: " TDP @ . ." bytes at $" CODE-ADDR .HEX ." , start at $" START .HEX CR
  ."   image: $100-$" D-END @ .HEX ."  and $" DICT-START @ .HEX ." -$" DICT-END' @ .HEX
  ." , at $" RAM-ADDR .HEX ."  + VM address" CR
  ."   start word: " ON-BOOT? IF ." ON-BOOT" ELSE ." BOOT" THEN CR
  ."   FREEZE on the board: " SAVER IF ." yes" ELSE ." no (without lib/esp32/esp32s3/bootimg.fs)" THEN CR
  ."   interrupts: " ISR-XT IF ." yes" ELSE ." no (without lib/esp32/intr.fs)" THEN CR
  ."   Ctrl-C waits for NO-BREAK: " HOLD-ADDR IF ." yes" ELSE ." no (without lib/idle.fs)" THEN CR
  ."   written: build/esp32s3.bin (" WRITTEN 0 .R ."  bytes)" CR ;

BUILD
