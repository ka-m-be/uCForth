\ ihex.fs - tests of the package IHEX (lib/ihex.fs)
\
\ Load (in the folder ucf), after the package FILES and the tester:
\   ... host/files.fs forth/test/tester.fs lib/test/ihex.fs
\ The expected records are computed by hand: the checksum makes the sum
\ of all bytes of a record 0 (modulo 256).

ONLY FORTH DEFINITIONS  ALSO FILES
S" lib/ihex.fs" INCLUDED
ONLY FORTH DEFINITIONS  ALSO INTERNALS  ALSO IHEX
DECIMAL

CREATE IH-OUT 200 ALLOT   VARIABLE IH-LEN
CREATE IH-DATA 20 ALLOT   IH-DATA 20 0 FILL

: IH-LINE ( a u -- )   \ collect the lines, each with "|" after it
  TUCK IH-OUT IH-LEN @ + SWAP MOVE  IH-LEN +!
  124 IH-OUT IH-LEN @ + C!  1 IH-LEN +! ;
' IH-LINE IS IHEX-LINE

: IH-SAME ( a u -- f )   \ true if the collected lines are the text a u
  DUP IH-LEN @ <> IF 2DROP FALSE EXIT THEN
  0 ?DO  DUP I + C@  IH-OUT I + C@ <> IF DROP UNLOOP FALSE EXIT THEN  LOOP
  DROP TRUE ;
: IH-RUN ( a u addr -- )   0 IH-LEN !  IHEX-START IHEX-DATA IHEX-END ;

\ 20 bytes 0 at $0100: one record of 16 bytes, one of 4
T{ IH-DATA 20 $0100 IH-RUN -> }T
T{ S" :1001000000000000000000000000000000000000EF|:0401100000000000EB|:00000001FF|" IH-SAME -> TRUE }T

\ 3 bytes at $1234: 03 + 12 + 34 + 00 + 01 + 02 + 03 = $4F, checksum $B1
1 IH-DATA C!  2 IH-DATA 1+ C!  3 IH-DATA 2 + C!
T{ IH-DATA 3 $1234 IH-RUN -> }T
T{ S" :03123400010203B1|:00000001FF|" IH-SAME -> TRUE }T

\ No data: only the last record
T{ IH-DATA 0 $0100 IH-RUN -> }T
T{ S" :00000001FF|" IH-SAME -> TRUE }T

\ 32 bit: an address above $FFFF needs a record with type 04 first
: IH-HIGH ( -- f )
  CELL 4 <> IF TRUE EXIT THEN
  $AB IH-DATA C!  IH-DATA 1 [ $FFFF 1+ ] LITERAL IH-RUN
  S" :020000040001F9|:01000000AB54|:00000001FF|" IH-SAME ;
T{ IH-HIGH -> TRUE }T

\ Reading: IHEX> gives count, address and type, the data in IHEX-BUF
T{ S" :03123400010203B1" IHEX> -> 3 $1234 0 }T
T{ IHEX-BUF C@  IHEX-BUF 2 + C@ -> 1 3 }T
T{ S" :00000001FF" IHEX> -> 0 0 1 }T
T{ S" :020000040001F9" IHEX> IHEX-BUF 1+ C@ -> 2 0 4 1 }T

' TYPE IS IHEX-LINE
S" ihex" .RESULTS
