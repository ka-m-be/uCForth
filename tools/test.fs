\ test.fs - run all tests of uCForth; the exit status is 0 if all pass
\
\ Use (in the folder ucf):
\   bash bootstrap/bash/ucf.sh host/files.fs tools/test.fs
\
\ The tester comes first, then t0.fs (the virtual machine), hayes.fs
\ (the core words), host/test/files.fs (the package FILES) and the
\ tests of the packages in lib/ and asm/ (8086 with each cell size;
\ elf, aarch64, sha256, md5, n3d, the WLAN and IR packages of the ESP32, MQTT, the IR bridge,
\ xtensa, disxt, disx32, disx64, disa64, x86 and x64 only with 32-bit cells). Each file runs in a sandbox (TEST). At
\ the end: the line "all: n tests, e errors", then the exit status 0 (no
\ error) or 1. MAIN runs everything in one line: a bootstrap VM stops a
\ file after a line with an error message, but the line itself runs to
\ its end.

ONLY FORTH DEFINITIONS  ALSO FILES
S" forth/test/tester.fs" INCLUDED
ALSO FILES

\ Each test file runs in a sandbox: afterwards the dictionary, the data
\ and the wordlists are as before (as MARKER does it), so the files do
\ not fill the memory. A file that stops counts as an error.
ALSO INTERNALS
CREATE SAVED  64 CELLS ALLOT        \ HERE VP VOC-LINK TEMP-START, then wordlist, head ... 0
VARIABLE P
: SNAPSHOT ( -- )
  HERE SAVED !  VP @ SAVED CELL+ !  VOC-LINK @ SAVED 2 CELLS + !  TEMP-START @ SAVED 3 CELLS + !
  SAVED 4 CELLS + P !
  VOC-LINK @ BEGIN ?DUP WHILE
    DUP P @ !  DUP @ P @ CELL+ !  2 CELLS P +!  CELL+ @
  REPEAT  0 P @ ! ;
: RESTORE ( -- )
  SAVED @ DP !  SAVED CELL+ @ VP !  SAVED 2 CELLS + @ VOC-LINK !  SAVED 3 CELLS + @ TEMP-START !
  SAVED 4 CELLS +  BEGIN DUP @ ?DUP WHILE  OVER CELL+ @ SWAP !  2 CELLS +  REPEAT  DROP
  CURRENT @ @ LATEST !  TEMP-HEADS OFF! ;
VARIABLE TA   VARIABLE TU            \ the name of the file: not on the stack (DEPTH)
: TEST ( a u -- )   \ run the test file a u in a sandbox
  TU !  TA !  ONLY FORTH DEFINITIONS ALSO FILES  SNAPSHOT
  TA @ TU @ ['] INCLUDED CATCH IF
    2DROP  1 TOTAL-ERRORS +!  ."   error: the test file stopped: " TA @ TU @ TYPE CR
  THEN  ONLY FORTH DEFINITIONS ALSO FILES  RESTORE ;

: TEST-FILES ( -- )
  S" forth/test/t0.fs"    TEST
  S" forth/test/hayes.fs" TEST
  S" host/test/files.fs"  TEST
  S" lib/test/ihex.fs"    TEST
  S" lib/test/vm.fs"      TEST
  S" lib/test/fbcon.fs"   TEST
  S" lib/test/text.fs"    TEST
  S" lib/test/package.fs" TEST
  S" lib/test/idle.fs"    TEST
  S" lib/test/slip.fs"    TEST
  S" lib/test/ircode.fs"  TEST
  S" lib/test/kelon168.fs" TEST
  S" lib/test/rty3.fs"    TEST
  S" lib/test/irsym.fs"   TEST
  S" apps/irbridge/test/boot.fs" TEST
  S" lib/test/json.fs"    TEST
  S" lib/net/test/bytes.fs" TEST
  S" tools/test/shadows.fs" TEST
  S" asm/test/8086.fs"    TEST
  S" asm/monitor/test/disx86.fs" TEST
  CELL 4 = IF                       \ these need 32-bit cells
    S" lib/linux/test/elf.fs" TEST
    S" asm/test/aarch64.fs"   TEST
    S" lib/test/sha256.fs"    TEST
    S" lib/test/md5.fs"       TEST
    S" lib/test/n3d.fs"       TEST
    S" lib/net/test/wpasta.fs" TEST
    S" lib/esp32/test/flpair.fs" TEST
    S" lib/esp32/test/wlsave.fs" TEST
    S" lib/net/test/wlsetup.fs" TEST
    S" lib/net/test/wifi.fs"    TEST    \ the WLAN, the setup AP and IR-QUEUE of the ESP32
    S" lib/net/test/hwrekey.fs" TEST    \ wpasta wlink hwlink rekey hwrekey
    S" lib/net/test/linksel.fs" TEST
    S" lib/net/test/loop.fs"    TEST    \ our TCP/IP over NO-LINK
    S" lib/net/test/aphttp.fs"  TEST    \ dhcps aphttp
    S" lib/net/test/netdown.fs" TEST    \ ping netdown
    S" lib/net/test/mqtt.fs"    TEST    \ MQTT with a broker in Forth, over NO-LINK
    S" lib/net/test/http.fs"    TEST    \ HTTP with a client in Forth, over NO-LINK
    S" apps/irbridge/test/bridge.fs"    TEST   \ the IR bridge, with mocks
    S" apps/irbridge/test/settings.fs"  TEST   \ its broker in the flash
    S" apps/irbridge/test/web.fs"       TEST   \ its web pages
    S" apps/irbridge/test/setup.fs"     TEST   \ its setup without a terminal
    S" lib/esp32/test/ackhost.fs" TEST  \ radiohost txhost txdone ackhost retry
    S" lib/esp32/test/ccmpreg.fs" TEST  \ radiohost stahost starx ccmpreg
    S" lib/esp32/test/setup.fs"  TEST   \ radiohost stahost aphost setup
    S" lib/esp32/test/wlback.fs" TEST
    S" lib/esp32/test/irqueue.fs" TEST
    S" lib/test/heads.fs"        TEST   \ the packages of the host with TEMP-HEADS on
    S" asm/test/xtensa.fs"    TEST
    S" asm/monitor/test/disxt.fs" TEST
    S" asm/monitor/test/disx32.fs" TEST
    S" asm/monitor/test/disx64.fs" TEST
    S" asm/monitor/test/disa64.fs" TEST
    S" asm/test/x86.fs"       TEST
    S" asm/test/x64.fs"       TEST
  THEN ;

: MAIN ( -- )
  ['] TEST-FILES CATCH  TOTAL-ERRORS @ OR  0<> 1 AND   ( status )
  ." all: " TOTAL-TESTS @ . ." tests, " TOTAL-ERRORS @ . ." errors" CR
  (BYE) ;

MAIN
