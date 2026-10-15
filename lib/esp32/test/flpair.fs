\ flpair.fs - tests of FLASH-PAIR (lib/esp32/flpair.fs) on the host, over the mock of the flash
\
\ Load (in the folder ucf), after the package FILES and the tester, with
\ 32-bit cells (SHA1):
\   ... -32 host/files.fs forth/test/tester.fs lib/esp32/test/flpair.fs
\ The lines "FLASH-PAIR: ..." during the tests are right: they come with
\ the records that are not whole.

ONLY FORTH DEFINITIONS  ALSO FILES
ONLY FORTH ALSO FILES  [DEFINED] SHA1   S" lib/sha1.fs"       ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] BYTES  S" lib/net/bytes.fs"  ?INCLUDED
ONLY FORTH DEFINITIONS ALSO FILES  INCLUDE lib/esp32/test/flmock.fs
ONLY FORTH DEFINITIONS ALSO FILES  INCLUDE lib/esp32/flpair.fs
ONLY FORTH DEFINITIONS  ALSO BYTES  ALSO FLASH  ALSO FLASH.I  ALSO FLASH-PAIR  ALSO FLASH-PAIR.I
DECIMAL

$3FC000 $3FD000 $31425249 PAIR P
CREATE IN 70 ALLOT   CREATE OUT 68 ALLOT
: IN! ( c -- )   IN 68 ROT FILL ;
: WRITE ( -- )   0 WRITES !  IN 68 P PAIR-WRITE ;
: READ ( -- f )   OUT 68 0 FILL  OUT 68 P PAIR-READ ;
: NUMBER@ ( faddr -- n )   AT 4 + @ ;
: FLIP ( faddr -- )   AT DUP C@ 1 XOR SWAP C! ;
: CLEAR? ( -- f )   \ the buffers of FLASH-PAIR hold nothing
  TRUE 128 0 DO  REC-A I + C@ REC-B I + C@ OR WORK I + C@ OR 0= AND  LOOP ;

\ ---- empty, then records in turn, each with a number one higher
T{ READ CLEAR? -> FALSE TRUE }T
T{ 1 IN! WRITE READ  OUT IN 68 BYTES=  CLEAR? -> TRUE TRUE TRUE }T
T{ $3FC000 AT @  $3FC000 NUMBER@  $3FC060 AT @ -> $31425249 1 COMMIT }T   \ magic, number, commit
T{ 2 IN! WRITE READ OUT C@  $3FD000 NUMBER@ -> TRUE 2 2 }T
T{ 3 IN! WRITE READ OUT C@  $3FC000 NUMBER@ -> TRUE 3 3 }T
T{ OUT 68 0 FILL  OUT 4 P PAIR-READ  OUT 3 + C@  OUT 4 + C@ -> TRUE 3 0 }T   \ fewer bytes than the record

\ ---- a power loss: in the erase or the write, or before the commit
T{ 4 IN!  1 FAIL-WRITE !  ' WRITE CATCH  READ OUT C@  CLEAR? -> -37 TRUE 3 TRUE }T
T{ 2 FAIL-WRITE !  ' WRITE CATCH  READ OUT C@ -> -37 TRUE 3 }T
T{ 0 FAIL-WRITE !  OMIT-COMMIT ON!  ' WRITE CATCH  READ OUT C@ -> -432 TRUE 3 }T
OMIT-COMMIT OFF!
T{ WRITE READ OUT C@  $3FD000 NUMBER@ -> TRUE 4 4 }T

\ ---- a damaged record: the older one counts; both damaged: -430
T{ $3FD050 FLIP  READ OUT C@ -> TRUE 3 }T
T{ $3FC050 FLIP  ' READ CATCH  CLEAR? -> -430 TRUE }T
T{ P PAIR-ERASE  READ -> FALSE }T
T{ 6 IN! WRITE  $3FC050 FLIP  ' READ CATCH -> -430 }T                      \ damaged, the other empty
T{ P PAIR-ERASE  READ -> FALSE }T
T{ 5 IN! WRITE  $3FC000 AT $3FD000 AT 128 MOVE  ' READ CATCH -> -430 }T    \ two with the same number
T{ P PAIR-ERASE  5 IN! WRITE  0 $3FD000 AT !  READ OUT C@ -> TRUE 5 }T     \ a foreign record: ignored
T{ P PAIR-ERASE  $BAD00BAD $3FC000 AT !  $BAD00BAD $3FD000 AT !  READ -> FALSE }T   \ old bytes in both
T{ 7 IN! WRITE  $3FD000 AT @  READ OUT C@ -> -1 TRUE 7 }T                     \ the first write erases both

\ ---- more than 68 bytes
T{ IN 69 P ' PAIR-WRITE CATCH NIP NIP NIP  CLEAR? -> -2 TRUE }T

P PAIR-ERASE
S" flpair" .RESULTS
