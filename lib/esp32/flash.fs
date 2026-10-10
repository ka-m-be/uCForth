\ flash.fs - package FLASH: the SPI flash of the ESP32 chips, over the ROM
\
\ The functions of the ROM read, write and erase the flash (package ROM;
\ their addresses come from the layer of the chip, ALSO CHIP). Flash
\ addresses start at 0; the boot image of uCForth is at 0 on the
\ ESP32-S3, at $1000 on the ESP32 (the README of the target). A write
\ needs erased flash (all bytes $FF). Erase works on sectors of 4 KB.
\ Flash addresses, buffer addresses and lengths must be multiples of 4.
\
\   faddr a u FLASH-READ  ( -- ior )   u bytes from the flash at faddr to a
\   a u faddr FLASH-WRITE ( -- ior )   u bytes from a to the flash at faddr
\   faddr u FLASH-ERASE   ( -- ior )   the sectors from faddr to faddr + u
\
\ ior is 0, or -37 when the ROM reports an error or a time-out.
\
\ The calls go through FLASH-SYSCALL of the layer of the chip: on the
\ ESP32 it switches the cache of the flash off meanwhile (the frozen
\ part of the dictionary lies in the flash, targets/esp32/README.md,
\ "Memory"); on the ESP32-S3 it is SYSCALL.
\
\ The function of the ROM for a write counts the pages of 256 bytes in 8
\ bits: with more than 255 pages in one call it does not stop at the end
\ of the data and writes over the rest of the flash. So FLASH-WRITE
\ calls it for 32 KB at a time.
\
\ The buffer a must be in the RAM inside the chip (SRAM-BUFFER:,
\ package GPIO), not in the PSRAM: the flash and the PSRAM share the
\ memory SPI, and the ROM would read the PSRAM while the flash works.
\ So nothing may use the PSRAM while a word of FLASH runs.
\
\ Each word first tells the ROM the size of the flash (FLASH-SIZE). The ROM
\ sets up the flash when it starts from it. After "ucf run" the tool
\ has done it over the download mode (SPI_ATTACH, as esptool). Without
\ it, the functions of the ROM wait for the flash forever. (The function
\ spi_flash_attach of the ROM would do it too, but it disturbs the
\ settings of the PSRAM, package PSRAM.)
\
\ On the board, in ucf term:  #include lib/esp32/flash.fs

PACKAGE FLASH  ALSO CHIP  ALSO ROM   \ lib/package.fs

4096 CONSTANT SECTOR
: IOR ( r -- ior )   IF -37 ELSE 0 THEN ;   \ r: 0 done, 1 error, 2 time-out
: PARAMS ( -- )   \ the size of the flash, of its blocks, sectors and pages
  0 FLASH-SIZE $10000 SECTOR 256 $FFFF 6 'FLASH-PARAMS SYSCALL DROP ;

: INSIDE ( a -- a' )   \ the chip address of a buffer in the RAM inside the chip
  >CHIP  DUP EXT-RAM EXT-RAM-END WITHIN ABORT" FLASH: the buffer is in the PSRAM" ;
VARIABLE NEXT                       \ the next sector to erase
: ERASE1 ( end -- ior )   \ a block of 64 KB when it fits, else a sector
  NEXT @ 15 AND 0=  OVER NEXT @ - 16 < 0= AND IF
    DROP  NEXT @ 16 / 1 'FLASH-ERASE-BLOCK FLASH-SYSCALL IOR  16 NEXT +!  EXIT
  THEN  DROP  NEXT @ 1 'FLASH-ERASE FLASH-SYSCALL IOR  1 NEXT +! ;
32768 CONSTANT /PART                \ the bytes of one write of the ROM: 128 pages
VARIABLE PART-A   VARIABLE PART-U   VARIABLE PART-F   \ the buffer, the bytes left, the flash address
: WRITE-PART ( -- ior )   \ the next part, at most /PART bytes
  PART-F @  PART-A @  PART-U @ /PART MIN  DUP >R  3 'FLASH-WRITE FLASH-SYSCALL IOR
  R@ PART-A +!  R@ PART-F +!  R> NEGATE PART-U +! ;

PUBLIC
: FLASH-READ ( faddr a u -- ior )   PARAMS  SWAP INSIDE SWAP  3 'FLASH-READ FLASH-SYSCALL IOR ;
: FLASH-WRITE ( a u faddr -- ior )   \ in parts of 32 KB
  PARAMS  PART-F !  PART-U !  INSIDE PART-A !
  BEGIN PART-U @ WHILE  WRITE-PART ?DUP IF EXIT THEN  REPEAT  0 ;
: FLASH-ERASE ( faddr u -- ior )   \ blocks of 64 KB where they fit: faster than sectors
  PARAMS  'FLASH-UNLOCK IF  0 'FLASH-UNLOCK FLASH-SYSCALL IF 2DROP -37 EXIT THEN  THEN
  OVER + SECTOR 1- + SECTOR /  SWAP SECTOR / NEXT !   ( sector-end )
  BEGIN DUP NEXT @ > WHILE  DUP ERASE1 IF DROP -37 EXIT THEN  REPEAT  DROP 0 ;

END-PACKAGE
