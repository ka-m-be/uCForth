\ phyhost.fs - the test of ESP32-PHY on the host: phy.fs loads and links
\
\ lib/esp32/esp32/phy.fs is a file for ucf term. On the board it loads
\ the code of C for the radio, links it, and forgets the tools and the
\ names of the code. Here, on the host, mock.fs stands for ROM and ASM,
\ and #include and #require of ucf term are words of Forth (paths
\ relative to lib/esp32/esp32/). CLINK is there before phy.fs, so that
\ its CODE! can be HOST! of mock.fs; on the board it comes after the
\ marker. The calls of the code of C need the chip: the board tests them.
\ Each system with files and 32-bit cells runs it; in the folder ucf:
\   build/ucf-c -32 host/files.fs lib/esp32/test/phyhost.fs </dev/null
\ At the end: "phyhost: 22 tests, 0 errors".

ONLY FORTH DEFINITIONS  ALSO FILES
INCLUDE forth/test/tester.fs
ONLY FORTH DEFINITIONS  ALSO FILES
S" lib/text.fs" INCLUDED  S" asm/xtensa.fs" INCLUDED
S" lib/esp32/test/mock.fs" INCLUDED  QUIET ON!
S" lib/esp32/clink.fs" INCLUDED
ALSO INTERNALS  ' HOST! IS CODE!  PREVIOUS
ONLY FORTH DEFINITIONS  ALSO FILES  ALSO TEXT  ALSO XTENSA  ALSO ROM  ALSO ASM  ALSO CLINK  ALSO INTERNALS

CREATE PHY-FOLDER  64 ALLOT   S" lib/esp32/esp32/" PHY-FOLDER PLACE
CREATE PHY-PATH  128 ALLOT
: #include ( "file" -- )   \ the file, relative to the folder of phy.fs
  PHY-FOLDER COUNT PHY-PATH PLACE  PARSE-NAME PHY-PATH +PLACE  PHY-PATH COUNT INCLUDED ;
: #require ( "word" "file" -- )   \ the file, if the word is not there
  PARSE-NAME FIND-NAME IF  PARSE-NAME 2DROP EXIT  THEN  #include ;
: IN-SRAM0? ( a -- f )   $40080000 $400A0000 WITHIN ;

VP @ CONSTANT VP0  HERE CONSTANT H0  CODE-SIZE CONSTANT CODE0
INCLUDE lib/esp32/esp32/phy.fs
ALSO INTERNALS  ALSO ESP32-PHY.I  ALSO ASM  ALSO CLINK  ALSO TEXT

\ The link: no name waits; the names of the code, PHY-C and the marker
\ are gone; the data and the words of ESP32-PHY stay.
T{ UNRESOLVED? -> FALSE }T
T{ [DEFINED] register_chipv7_phy  [DEFINED] fpu_on  [DEFINED] hal_init -> FALSE FALSE FALSE }T
T{ [DEFINED] PHY-C  [DEFINED] LINKED  [DEFINED] PHY-TOOLS -> FALSE FALSE FALSE }T
T{ [DEFINED] g_phyFuns  [DEFINED] phy_init_flag  [DEFINED] cal:rodata -> TRUE TRUE TRUE }T
T{ [DEFINED] PHY-INIT  [DEFINED] PHY-CHANNEL!  [DEFINED] PHY-TRACK -> TRUE TRUE TRUE }T
T{ [DEFINED] PHY-ON  [DEFINED] PHY-OFF  [DEFINED] HAL-INIT -> TRUE TRUE FALSE }T

\ The addresses of the functions of C: in SRAM0, as the link gave them.
T{ 'REGISTER IN-SRAM0?  'WAKEUP IN-SRAM0?  'CLOSE IN-SRAM0? -> TRUE TRUE TRUE }T
T{ 'CHANNEL IN-SRAM0?  'AGC-OFF IN-SRAM0?  'AGC-ON IN-SRAM0? -> TRUE TRUE TRUE }T
T{ 'TRACK IN-SRAM0?  'FPU-ON IN-SRAM0? -> TRUE TRUE }T
T{ 'LOW-OFF IN-SRAM0? -> TRUE }T
T{ 'FPU-ON  $40082000 CODE0 + $30 +  = -> TRUE }T   \ fpu_on: the sixth function of phyidf.fs, at $30

\ The memory: the dictionary keeps about 7 KB, the RAM data the data of
\ C, the calibration and the copies of the tables of .rodata; the code
\ is in SRAM0.
T{ HERE H0 -  7500 < -> TRUE }T
T{ VP @ VP0 -  #C-DATA 1904 + #TABLES @ + - 100 < -> TRUE }T
T{ CODE-SIZE CODE0 -  47484 = -> TRUE }T

\ The tables of .rodata: constants with the address of their copy in the
\ RAM data, with the bytes of phydata.fs.
T{ ' ana:rodata @  ana:rodata DATA-END U< -> 2 TRUE }T
T{ ana:rodata C@  ana:rodata 8 + C@  ana:rodata 11 + C@ -> 0 $30 $71 }T

\ The parameters: 128 bytes; byte 44 is 78, as in ESP-IDF.
T{ INIT-DATA C@  INIT-DATA 44 + C@  INIT-DATA 57 + C@  INIT-DATA 127 + C@ -> 3 78 5 0 }T

\ The start values: SAVE-START at the link; RESET-DATA puts them back.
T{ START-VALUES #C-DATA C-DATA #C-DATA STR= -> TRUE }T
T{ C-DATA C@  $5A C-DATA C!  RESET-DATA  C-DATA C@ = -> TRUE }T
T{ phy_init_flag C@  1 phy_init_flag C!  RESET-DATA  phy_init_flag C@ = -> TRUE }T

\ The calls: on the host SYSCALL stops (mock.fs); PHY-INIT calls fpu_on
\ first, so it touches no register of the chip here.
T{ ' PHY-INIT CATCH -> -2 }T
T{ ' PHY-LOW-OFF CATCH -> -2 }T

S" phyhost" .RESULTS
