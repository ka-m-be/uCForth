\ ucf.fs - the tool ucf: flash and console for boards with uCForth
\
\ An application from packages. Build it into a program for the Mac:
\   LC_ALL=C awk -f bootstrap/awk/ucf.awk -- -32 host/files.fs tools/ucf.fs targets/macos-a64/build.fs </dev/null
\   chmod +x build/ucf
\ or for Windows (build/ucf.exe), Linux (build/ucf-a64, build/ucf-x64,
\ build/ucf-i386) or a Mac with Intel (build/ucf-intel):
\ targets/win32/build.fs, linux-a64/build.fs, linux-x64/build.fs,
\ linux-i386/build.fs or macos-x64/build.fs instead. term works with each board that runs uCForth on a serial
\ line; info, flash, reset and run are for the ESP32-S3 and the ESP32 (tools/README.md).
\
\ Commands (the first argument):
\   ucf info PORT                   what the chip tells about itself
\   ucf flash PORT FILE ADDRESS     write FILE to the flash at ADDRESS
\   ucf reset PORT                  restart the board from the flash
\   ucf term PORT [FILE]            the console to uCForth on the board;
\                                   FILE: #include it first (an application)
\   ucf run PORT KERNEL [IMAGE]     load into the RAM, start, then term
\   ucf                             the prompt of the host Forth (BYE ends it)
\ The exit status is 0, or 1 after an error.

ONLY FORTH DEFINITIONS  ALSO FILES
[DEFINED] TEXT    S" lib/text.fs"            ?INCLUDED
[DEFINED] SERIAL  S" host/serial.fs"         ?INCLUDED
[DEFINED] SLIP    S" lib/slip.fs"            ?INCLUDED
[DEFINED] MD5     S" lib/md5.fs"             ?INCLUDED
[DEFINED] ESPROM  S" lib/esp32/esprom.fs"    ?INCLUDED
[DEFINED] TERM    S" host/term.fs"           ?INCLUDED
[DEFINED] ESPIMG  S" lib/esp32/espimg.fs"    ?INCLUDED

ONLY FORTH DEFINITIONS
PACKAGE UCF  ALSO FILES  ALSO TEXT  ALSO SERIAL  ALSO ESPROM  ALSO TERM  ALSO ESPIMG

: .N ( n -- )  0 .R ;
: USAGE ( -- )
  ." usage: ucf info PORT" CR
  ."        ucf flash PORT FILE ADDRESS    (ADDRESS: 0xFF0000, '$FF0000' or 16711680)" CR
  ."        ucf reset PORT" CR
  ."        ucf term PORT [FILE]           (FILE: #include it first)" CR
  ."        ucf run PORT KERNEL [IMAGE]    (into the RAM, then term)" CR
  ."        ucf                            (the prompt of the host Forth)" CR ;
: NEEDED ( n a u -- a' u' )   \ argument n; the message a u if it is missing
  ROT ARG DUP IF 2SWAP 2DROP EXIT THEN  2DROP  USAGE  TRUE ROT ROT (ABORT") ;
: HEX? ( a u -- a' u' f )   \ without a leading 0x; f: it was there
  DUP 2 > IF  OVER 2 S" 0x" S= IF 2 /STRING TRUE EXIT THEN  THEN  FALSE ;
: ADDRESS ( a u -- u )
  BASE @ >R  HEX? IF HEX ELSE DECIMAL THEN  NUMBER?  R> BASE !
  0= ABORT" the address is not a number" ;
: THE-PORT ( -- )  2 S" the port is missing" NEEDED OPEN-PORT ;

: DO-INFO  ( -- )  THE-PORT START INFO ;
: DO-FLASH ( -- )
  3 S" the file is missing" NEEDED  4 S" the address is missing" NEEDED ADDRESS
  THE-PORT FLASH ;
: DO-RESET ( -- )  THE-PORT HARD-RESET ;
: DO-TERM  ( -- )  THE-PORT  3 ARG DUP IF TERMINAL-INCLUDE ELSE 2DROP TERMINAL THEN ;

: SEND-SEGMENT ( ta u -- )   \ from the open image file to the RAM of the chip
  TUCK SWAP MEM-BEGIN
  BEGIN DUP WHILE  DUP BLOCK-SIZE MIN  BUF OVER READ-DATA  BUF OVER MEM-DATA  -  REPEAT  DROP ;
: LOAD-IMAGE ( a u -- entry )   \ the segments of the image file a u into the RAM of the chip
  2DUP OPEN-IMAGE  IMAGE-CHIP @ CHIP-ID @ <> ABORT" the image is for another chip"
  ." loading " TYPE ." :"
  #SEGMENTS @ 0 ?DO  SEGMENT-HEAD  SPACE DUP .N ."  bytes at $" OVER .HEX  SEND-SEGMENT  LOOP
  CR  CLOSE-IMAGE  ENTRY @ ;
: DO-RUN ( -- )
  3 S" the kernel is missing" NEEDED  THE-PORT START  ATTACH-FLASH
  LINK 4 = IF ." note: the kernel talks over UART0, at the port 'USB to serial'" CR THEN
  LOAD-IMAGE  4 ARG DUP IF LOAD-IMAGE DROP ELSE 2DROP THEN
  DUP MEM-END  ." started at $" .HEX CR  TERMINAL ;
: COMMAND ( a u -- )
  2DUP S" info"  S= IF 2DROP DO-INFO  EXIT THEN
  2DUP S" flash" S= IF 2DROP DO-FLASH EXIT THEN
  2DUP S" reset" S= IF 2DROP DO-RESET EXIT THEN
  2DUP S" term"  S= IF 2DROP DO-TERM  EXIT THEN
  2DUP S" run"   S= IF 2DROP DO-RUN   EXIT THEN
  USAGE  S" unknown command: " 2SWAP FAIL ;

PUBLIC
: MAIN ( -- )   \ the command from the command line; without one, the prompt (BOOT: ONLY FORTH)
  1 ARG  DUP 0= IF  2DROP ." ucf: the host Forth; BYE ends it" CR  BOOT  THEN
  ['] COMMAND CATCH  CLOSE-PORT  ?DUP IF .ERROR 1 (BYE) THEN  0 (BYE) ;

END-PACKAGE

\ For targets/macos-a64/build.fs: the start word and the name of the program
: ON-BOOT ( -- )   MAIN ;
: PROGRAM ( -- a u )   S" build/ucf" ;
