\ flmock.fs - CHIP and FLASH as mocks on the host: 16 KB of flash from $3FC000, faults on request
\
\ For the tests of FLASH-PAIR, WLSAVE and the settings of the IR bridge:
\ the last four sectors of the 4-MB flash of the WROOM. A write counts in
\ WRITES; the write number FAIL-WRITE fails with -37, and with OMIT-COMMIT
\ on the second write (the commit of a record) does nothing, as a power
\ loss would. AT gives the place of a flash address in the RAM (FLASH.I).
PACKAGE CHIP
PUBLIC $400000 CONSTANT FLASH-SIZE
END-PACKAGE
PACKAGE FLASH
16384 BUFFER: STORAGE
STORAGE 16384 $FF FILL
: AT ( faddr -- a ) $3FC000 - STORAGE + ;
PUBLIC
VARIABLE WRITES VARIABLE FAIL-WRITE VARIABLE OMIT-COMMIT
: FLASH-READ ( faddr a n -- ior ) >R SWAP AT SWAP R> MOVE 0 ;
: FLASH-ERASE ( faddr n -- ior ) SWAP AT SWAP $FF FILL 0 ;
: FLASH-WRITE ( a n faddr -- ior )
  1 WRITES +! WRITES @ FAIL-WRITE @ = IF DROP 2DROP -37 EXIT THEN
  WRITES @ 2 = OMIT-COMMIT @ AND IF DROP 2DROP 0 EXIT THEN
  AT SWAP MOVE 0 ;
END-PACKAGE
