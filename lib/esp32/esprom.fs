\ esprom.fs - package ESPROM: the ROM bootloader of the ESP32-S3 and the ESP32
\
\ The ROM of the chip has a bootloader: after a reset with GPIO 0 low
\ it waits for requests on the serial line (UART0, or USB-Serial/JTAG).
\ A request goes out in a SLIP frame; its response comes back with the
\ same op. The protocol: targets/esp32s3/README.md.
\ Needs FILES TEXT SERIAL SLIP MD5.
\
\   START           reset the board into the bootloader, check the chip
\                   (CHIP-ID: 0 the ESP32, 9 the ESP32-S3)
\   INFO            show what the chip tells about itself
\   a u addr FLASH  write the file a u to the flash at addr, check with MD5
\   HARD-RESET      restart the board from the flash
\   REG@ REG!       registers of the chip
\   u ta MEM-BEGIN  a u MEM-DATA ...  ta MEM-END   load into the RAM, start at ta
\   ATTACH-FLASH    set up the flash for a program in the RAM (as esptool)

PACKAGE ESPROM  ALSO FILES  ALSO TEXT  ALSO SERIAL  ALSO SLIP  ALSO MD5   \ lib/package.fs

\ ---- numbers in little endian -----------------------------------------

: LE! ( x a -- )   \ store 4 bytes, the low byte first
  4 0 DO  2DUP C!  1+  SWAP 8 RSHIFT SWAP  LOOP  2DROP ;
: LE@ ( a -- x )  0  4 0 DO  8 LSHIFT  OVER 3 I - + C@ OR  LOOP  NIP ;
: LE16! ( x a -- )  2DUP C!  SWAP 8 RSHIFT SWAP 1+ C! ;
: LE16@ ( a -- x )  DUP C@  SWAP 1+ C@ 8 LSHIFT OR ;

\ ---- requests and responses (Spec/005 6.3) -----------------------------

PUBLIC
1024 CONSTANT BLOCK-SIZE            \ data per FLASH_DATA, as the ROM wants it
PRIVATE
BLOCK-SIZE 24 + CONSTANT PKT-SIZE   \ header 8, parameters 16, data
PKT-SIZE BUFFER: PKT                \ the request
VARIABLE PKT-N                      \ bytes of data in PKT
PKT-SIZE 2* 2 + BUFFER: WIRE        \ the request as a SLIP frame
3000 CONSTANT TIMEOUT               \ ms, for most requests
VARIABLE DEADLINE

: REQUEST ( op -- )  PKT 8 0 FILL  PKT 1+ C!  0 PKT-N ! ;
: >PKT ( -- a )  PKT 8 + PKT-N @ + ;
: ARG, ( x -- )  >PKT LE!  4 PKT-N +! ;
: CHECKSUM ( a u -- x )   \ XOR of the bytes, the start value is $EF
  $EF ROT ROT  OVER + SWAP ?DO  I C@ XOR  LOOP ;
: SEND-REQUEST ( -- )
  PKT-N @ PKT 2 + LE16!  PKT PKT-N @ 8 + WIRE ENCODE  WIRE SWAP SEND ;

: FRAME? ( -- f )   \ wait until DEADLINE for a frame; true: FRAME holds it
  NEW-FRAME
  BEGIN
    RECV DUP 0< IF DROP 1 MS ELSE  DECODE IF TRUE EXIT THEN  THEN
    DEADLINE @ MILLIS - 0<            \ also when text without frames keeps coming
  UNTIL  FALSE ;
: ANSWER? ( ms -- f )   \ wait for the response to the request in PKT
  MILLIS + DEADLINE !
  BEGIN FRAME? WHILE
    FRAME-N @ 8 < 0=  FRAME C@ 1 = AND  FRAME 1+ C@ PKT 1+ C@ = AND
    IF TRUE EXIT THEN
  REPEAT  FALSE ;
: VAL ( -- x )  FRAME 4 + LE@ ;    \ the value of the response: READ_REG answers with it
: REASON ( n -- a u )   \ the error codes of the ROM
  DUP 5 = IF DROP S" invalid request" EXIT THEN
  DUP 6 = IF DROP S" the ROM could not do it" EXIT THEN
  DUP 7 = IF DROP S" wrong checksum" EXIT THEN
  DUP 8 = IF DROP S" cannot write to the flash" EXIT THEN
  DROP S" unknown error" ;
: ANSWER ( ms -- a u )   \ the data of the response without the 4 status bytes
  ANSWER? 0= ABORT" no answer from the ROM bootloader"
  FRAME 8 +  FRAME 2 + LE16@  FRAME-N @ 8 - MIN
  DUP 4 < ABORT" the answer of the ROM is too short"
  4 -  2DUP + C@ IF  2DUP + 1+ C@ REASON  S" the ROM reports: " 2SWAP FAIL  THEN ;
: RESULT ( ms -- a u )  SEND-REQUEST ANSWER ;

\ ---- into the bootloader (Spec/005 6.1) --------------------------------

: BOOTLOADER-USB ( -- )   \ over the USB-Serial/JTAG of the chip (TRM 33.4)
  FALSE RTS!  FALSE DTR!  100 MS              \ idle, the download flag is clear
  TRUE DTR!  FALSE RTS!  100 MS               \ set the download flag
  TRUE RTS!  FALSE DTR!  TRUE RTS!  100 MS    \ reset, through RTS 1 DTR 1
  FALSE DTR!  FALSE RTS! ;
: BOOTLOADER-UART ( ms -- )   \ over a USB-UART with two transistors (CH343P)
  FALSE DTR!  TRUE RTS!  100 MS               \ EN 0: the chip is in reset
  TRUE DTR!  FALSE RTS!  MS                   \ GPIO 0 is 0 when EN comes back
  FALSE DTR! ;

: SYNC? ( -- f )   \ one SYNC; true if the ROM answers within 0.1 s
  $08 REQUEST  $20120707 ARG,  >PKT 32 $55 FILL  32 PKT-N +!
  SEND-REQUEST  100 ANSWER? ;
PUBLIC
: CONNECT ( -- )   \ reset into the bootloader and SYNC, up to 7 times
  ." connecting "
  7 0 DO
    I 1 AND IF  I 2 AND IF 550 ELSE 50 THEN BOOTLOADER-UART  ELSE  BOOTLOADER-USB  THEN
    ." ."  100 DRAIN                          \ the start text of the ROM
    5 0 DO
      SYNC? IF  100 DRAIN  UNLOOP UNLOOP  ."  ok" CR EXIT  THEN  50 MS
    LOOP
  LOOP  CR          \ a firmware with its own USB driver does not react to DTR and RTS
  TRUE ABORT" no answer from the ROM bootloader. Hold BOOT, press RESET, release BOOT, try again; the port may get a new name." ;

\ ---- registers, chip and link (Spec/005 6.5 and 6.6) -------------------

: REG@ ( a -- x )  $0A REQUEST ARG,  TIMEOUT RESULT 2DROP  VAL ;
: REG! ( x a -- )  $09 REQUEST ARG, ARG,  -1 ARG,  0 ARG,  TIMEOUT RESULT 2DROP ;
PRIVATE
: SECURITY-ID ( -- n )   \ the chip ID from GET_SECURITY_INFO: 9 is the ESP32-S3
  $14 REQUEST  TIMEOUT RESULT  16 < ABORT" the ROM gives no chip ID"  12 + LE@ ;
PUBLIC
VARIABLE CHIP-ID   -1 CHIP-ID !     \ after START: 0 the ESP32, 9 the ESP32-S3
: CHIP ( -- n )   \ the ROM of the ESP32 has $00F01D83 at $40001000, and no GET_SECURITY_INFO
  $40001000 REG@ $00F01D83 = IF 0 ELSE SECURITY-ID THEN ;
: ESP32? ( -- f )  CHIP-ID @ 0= ;
: LINK ( -- n )   \ 4: USB-Serial/JTAG, 3: USB-OTG, else a UART (the ESP32 has only UARTs)
  ESP32? IF 0 EXIT THEN  $3FCEF14C REG@ ;
PRIVATE
: WATCHDOGS-OFF ( -- )   \ else they reset the chip at the USB-Serial/JTAG
  $50D83AA1 $600080B0 REG!  0 $60008098 REG!  0 $600080B0 REG!        \ RTC watchdog off
  $8F1D312A $600080B8 REG!  $600080B4 REG@ $80000000 OR $600080B4 REG!
  0 $600080B8 REG! ;                                                  \ super watchdog feeds itself
PUBLIC
: START ( -- )   \ into the bootloader, check the chip
  CONNECT  CHIP DUP CHIP-ID !
  DUP 0<> SWAP 9 <> AND ABORT" the chip is neither an ESP32 nor an ESP32-S3"
  LINK 4 = IF WATCHDOGS-OFF THEN ;
: IN-ROM? ( -- f )   \ does the ROM bootloader answer now, without a reset?
  3 0 DO  SYNC? IF 100 DRAIN UNLOOP TRUE EXIT THEN  LOOP  FALSE ;
PRIVATE
: LOCK ( -- )  0 $600080B0 REG! ;
: WATCHDOG-RESET ( -- )   \ the RTC watchdog resets the chip; the values come from esptool
  $50D83AA1 $600080B0 REG!  2000 $6000809C REG!  $D0000102 $60008098 REG!
  ['] LOCK CATCH DROP  500 MS ;       \ the reset may come before the answer
PUBLIC
: HARD-RESET ( -- )   \ restart from the flash (Spec/005 6.1)
  IN-ROM? IF  CHIP DUP CHIP-ID !  9 = IF       \ the ESP32-S3; the ESP32 needs only RTS
    $6000812C REG@ -2 AND $6000812C REG!      \ RTC_CNTL_OPTION1: no forced download boot
    LINK 3 5 WITHIN  $60004038 REG@ 8 AND 0=  AND IF   \ USB, and GPIO 0 was 0 at the last reset
      WATCHDOG-RESET  ." restarted by the RTC watchdog" CR EXIT
    THEN
  THEN THEN
  LINES? 0= ABORT" the port has no DTR and RTS: press the reset button"
  PULSE-RTS  ." restarted over RTS" CR ;

\ ---- info (Spec/005 6.5) -----------------------------------------------

PRIVATE
: FUSE ( n -- x )  4 * $60007044 + REG@ ;     \ word n of eFuse block 1
: FIELD ( x shift width -- u )  >R RSHIFT  1 R> LSHIFT 1- AND ;
: .N ( n -- )  0 .R ;
: HEX-DIGIT ( u -- c )  DUP 9 > IF 87 ELSE 48 THEN + ;
: .XX ( c -- )  255 AND  DUP 4 RSHIFT HEX-DIGIT EMIT  15 AND HEX-DIGIT EMIT ;
: .PACKAGE ( -- )
  3 FUSE 21 3 FIELD
  DUP 0= IF DROP ." ESP32-S3 (QFN56)" EXIT THEN
  DUP 1 = IF DROP ." ESP32-S3-PICO-1 (LGA56)" EXIT THEN
  ." ESP32-S3, package " .N ;
: .REVISION ( -- )   \ eFuse fields as esptool reads them; chips v0.0 have old fields
  3 FUSE 18 3 FIELD  5 FUSE 23 1 FIELD 3 LSHIFT OR        ( minor )
  5 FUSE 24 2 FIELD                                       ( minor major )
  OVER 7 AND 0=  $6000706C REG@ 3 AND 1 = AND  3 FUSE 24 3 FIELD 1 = AND
  IF 2DROP 0 0 THEN
  ." v" .N ." ." .N ;
: .FLASH ( n -- )
  DUP 1 = IF DROP ." 8 MB" EXIT THEN  DUP 2 = IF DROP ." 4 MB" EXIT THEN  ." ? (" .N ." )" ;
: .PSRAM ( n -- )
  DUP 1 = IF DROP ." 8 MB" EXIT THEN  DUP 2 = IF DROP ." 2 MB" EXIT THEN
  DUP 3 = IF DROP ." 16 MB" EXIT THEN  DUP 4 = IF DROP ." 4 MB" EXIT THEN  ." ? (" .N ." )" ;
: .EMBEDDED ( -- )   \ flash and PSRAM in the chip package; a module has them beside the chip
  3 FUSE 27 3 FIELD  4 FUSE 3 2 FIELD  5 FUSE 19 1 FIELD 2 LSHIFT OR    ( flash psram )
  2DUP OR 0= IF 2DROP ." none" EXIT THEN
  SWAP ?DUP IF ." flash " .FLASH  DUP IF ." , " THEN THEN
  ?DUP IF ." PSRAM " .PSRAM THEN ;
: .LINK ( -- )
  LINK  DUP 4 = IF DROP ." USB-Serial/JTAG" EXIT THEN
  3 = IF ." USB-OTG" ELSE ." UART" THEN ;
: .MAC ( -- )
  1 FUSE  DUP 8 RSHIFT .XX  ." :" .XX
  0 FUSE  24 BEGIN  ." :"  2DUP RSHIFT .XX  8 -  DUP 0< UNTIL  2DROP ;

\ The ESP32: eFuse block 0 from $3FF5A000, the fields as esptool reads them
: FUSE32 ( n -- x )  4 * $3FF5A000 + REG@ ;
: PKG32 ( -- n )   3 FUSE32  DUP 9 3 FIELD  SWAP 2 1 FIELD 3 LSHIFT OR ;
: MAJOR32 ( -- n )   \ eFuse words 3 and 5 and APB_CTRL_DATE: 0 1 3 7 are the revisions 0 1 2 3
  3 FUSE32 15 1 FIELD  5 FUSE32 20 1 FIELD 1 LSHIFT OR  $3FF6607C REG@ 31 1 FIELD 2 LSHIFT OR
  DUP 3 = IF DROP 2 EXIT THEN  DUP 7 = IF DROP 3 EXIT THEN  DUP 1 > IF DROP 0 THEN ;
: SINGLE? ( -- f )  3 FUSE32 1 AND 0<> ;     \ CHIP_VER_DIS_APP_CPU: one core
: V3? ( -- f )  MAJOR32 3 = ;
: .PACKAGE32 ( -- )
  PKG32
  DUP 0= IF DROP  SINGLE? IF ." ESP32-S0WDQ6" EXIT THEN  ." ESP32-D0WDQ6" V3? IF ." -V3" THEN EXIT THEN
  DUP 1 = IF DROP  SINGLE? IF ." ESP32-S0WD" EXIT THEN  ." ESP32-D0WD" V3? IF ." -V3" THEN EXIT THEN
  DUP 2 = IF DROP ." ESP32-D2WD" EXIT THEN
  DUP 4 = IF DROP ." ESP32-U4WDH" EXIT THEN
  DUP 5 = IF DROP  V3? IF ." ESP32-PICO-V3" ELSE ." ESP32-PICO-D4" THEN EXIT THEN
  DUP 6 = IF DROP ." ESP32-PICO-V3-02" EXIT THEN
  DUP 7 = IF DROP ." ESP32-D0WDR2-V3" EXIT THEN
  ." ESP32, package " .N ;
: .FEATURES32 ( -- )
  ." Wi-Fi"  3 FUSE32 2 AND 0= IF ." , Bluetooth" THEN
  SINGLE? IF ." , one core" ELSE ." , two cores" THEN ;
: .EMBEDDED32 ( -- )   \ the packages 2, 4, 5, 6 have the flash inside, 6 the PSRAM too
  PKG32  DUP 2 = OVER 4 = OR OVER 5 = OR OVER 6 = OR IF ." flash" ELSE ." none" THEN
  6 = IF ." , PSRAM" THEN ;
: .MAC32 ( -- )   \ word 2 bits 15-0, then word 1
  2 FUSE32  DUP 8 RSHIFT .XX  ." :" .XX
  1 FUSE32  24 BEGIN  ." :"  2DUP RSHIFT .XX  8 -  DUP 0< UNTIL  2DROP ;
: .CRYSTAL32 ( -- )   \ the UART divider of the ROM: divider x baud = the crystal
  $3FF40014 REG@ $FFFFF AND  BAUD 1000000 */  33 > IF ." 40 MHz" ELSE ." 26 MHz" THEN ;
: INFO-32 ( -- )
  ." chip        " .PACKAGE32 ." , revision v" MAJOR32 .N ." ." 5 FUSE32 24 2 FIELD .N CR
  ." features    " .FEATURES32 CR
  ." in package  " .EMBEDDED32 CR
  ." link        UART" CR
  ." mac         " .MAC32 CR
  ." crystal     " .CRYSTAL32 CR ;
PUBLIC
: INFO ( -- )   \ after START: what the chip tells about itself
  ESP32? IF INFO-32 EXIT THEN
  ." chip        " .PACKAGE ." , revision " .REVISION CR
  ." features    Wi-Fi, BLE" CR
  ." in package  " .EMBEDDED CR
  ." link        " .LINK CR
  ." mac         " .MAC CR
  ." crystal     40 MHz" CR ;

\ ---- flash (Spec/005 6.7) ----------------------------------------------

: FLASH-SIZE ( -- u )   \ after START: the module N16R8 has 16 MB, the ESP32-WROOM-32 N4 has 4 MB
  ESP32? IF $400000 ELSE $1000000 THEN ;
PRIVATE
VARIABLE FID   VARIABLE SIZE   VARIABLE ADDR   VARIABLE SEQ
PUBLIC
BLOCK-SIZE BUFFER: BUF

PRIVATE
: OPEN-IN ( a u -- )   \ open the file a u for reading
  2DUP R/O OPEN-FILE IF  DROP S" cannot open " 2SWAP FAIL  THEN  FID !  2DROP ;
: CLOSE-IN ( -- )  FID @ CLOSE-FILE DROP ;
: READ-BLOCK ( -- u )   \ the next block of the file into BUF; the rest of BUF is $FF
  BUF BLOCK-SIZE $FF FILL
  BUF BLOCK-SIZE FID @ READ-FILE ABORT" cannot read the file" ;
: 4ALIGNED ( u -- u' )  3 + -4 AND ;
: TIMEOUT/MB ( u ms -- ms' )   \ ms per MB for u bytes, at least TIMEOUT
  SWAP 1023 + 1024 / * 1024 / TIMEOUT MAX ;

: ATTACH ( -- )  $0D REQUEST  0 ARG, 0 ARG,  TIMEOUT RESULT 2DROP ;
: SET-PARAMS ( size -- )
  $0B REQUEST  0 ARG,  ARG,  $10000 ARG,  $1000 ARG,  256 ARG,  $FFFF ARG,  TIMEOUT RESULT 2DROP ;
: FLASH-BEGIN ( -- )   \ the ROM erases the area first: up to 30 s per MB
  $02 REQUEST  SIZE @ ARG,  SIZE @ BLOCK-SIZE 1- + BLOCK-SIZE / ARG,  BLOCK-SIZE ARG,
  ADDR @ ARG,  ESP32? 0= IF 0 ARG, THEN           \ the ESP32 takes 4 words (no flag for encryption)
  SIZE @ 30000 TIMEOUT/MB RESULT 2DROP ;
: WRITE-BLOCK ( -- )  TIMEOUT ANSWER 2DROP ;
: FLASH-DATA ( u -- )   \ BUF as the next block; its first u bytes belong to the file
  $03 REQUEST  BLOCK-SIZE ARG,  SEQ @ ARG,  0 ARG,  0 ARG,
  BUF >PKT BLOCK-SIZE MOVE  BLOCK-SIZE PKT-N +!
  BUF BLOCK-SIZE CHECKSUM PKT 4 + LE!
  SEND-REQUEST  BUF SWAP 4ALIGNED MD5-UPDATE      \ MD5 works while the ROM writes
  3 0 DO
    I IF ." (again) " SEND-REQUEST THEN
    ['] WRITE-BLOCK CATCH ?DUP 0= IF UNLOOP 1 SEQ +! EXIT THEN
    I 2 = IF THROW THEN DROP
  LOOP ;
: FLASH-MD5 ( -- a u )   \ the MD5 of the area, from the ROM: 32 hex digits
  $13 REQUEST  ADDR @ ARG,  SIZE @ ARG,  0 ARG,  0 ARG,  SIZE @ 8000 TIMEOUT/MB RESULT ;
: .KB ( u -- )  1023 + 1024 / .N ."  KB" ;
: .TIME ( ms -- )  100 / 10 /MOD .N ." ." .N ."  s" ;

PUBLIC
: ATTACH-FLASH ( -- )   \ set up the flash, as the ROM does when it starts from it
  ATTACH  FLASH-SIZE SET-PARAMS ;
: FLASH ( a u addr -- )   \ write the file a u to the flash at addr and check it with MD5
  ADDR !  2DUP OPEN-IN  0 BEGIN READ-BLOCK ?DUP WHILE + REPEAT  4ALIGNED SIZE !  CLOSE-IN
  ADDR @ 4095 AND ABORT" the address must be a multiple of 4096"
  SIZE @ 0= ABORT" the file is empty"
  START
  FLASH-SIZE ADDR @ SIZE @ + U< ABORT" the file does not fit into the flash"
  ATTACH  FLASH-SIZE SET-PARAMS  MILLIS >R
  ." erasing " SIZE @ .KB ."  at $" BASE @ HEX ADDR @ .N BASE ! ."  ..." CR
  FLASH-BEGIN  ." writing "
  OPEN-IN  MD5-INIT  0 SEQ !
  BEGIN READ-BLOCK ?DUP WHILE  FLASH-DATA  SEQ @ 15 AND 0= IF ." ." THEN  REPEAT
  CLOSE-IN  ."  ok" CR
  ." checking MD5 ... "
  FLASH-MD5  32 <> ABORT" the ROM gives no MD5"
  MD5-FINAL DROP  32 MD5-HEX S= 0= ABORT" the MD5 differs: the flash holds other data"
  ." ok" CR  SIZE @ .KB ."  in " MILLIS R> - .TIME CR
  ." The board waits in the bootloader; reset starts it." CR ;

\ ---- RAM (Spec/006 section 5) ------------------------------------------

: MEM-BEGIN ( u ta -- )   \ u bytes for the RAM at ta follow
  $05 REQUEST  OVER ARG,  SWAP BLOCK-SIZE 1- + BLOCK-SIZE / ARG,  BLOCK-SIZE ARG,  ARG,
  TIMEOUT RESULT 2DROP  0 SEQ ! ;
: MEM-DATA ( a u -- )   \ the next block of at most BLOCK-SIZE bytes
  $07 REQUEST  DUP ARG,  SEQ @ ARG,  0 ARG,  0 ARG,
  2DUP >PKT SWAP MOVE  DUP PKT-N +!
  CHECKSUM PKT 4 + LE!  TIMEOUT RESULT 2DROP  1 SEQ +! ;
: MEM-END ( ta -- )   \ the ROM jumps to ta; its answer may not come
  $06 REQUEST  0 ARG,  ARG,  SEND-REQUEST  200 ANSWER? DROP ;

END-PACKAGE
