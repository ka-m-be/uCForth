\ phy.fs - package ESP32-PHY: the radio of the ESP32 (calibration, channel, power)
\
\ The radio of the ESP32 needs a calibration before it sends or receives.
\ Espressif gives it only as code of C in a library (libphy). The
\ functions that we need are in phychip.fs, phyana.fs, phycal.fs and
\ phyrtc.fs (tools/pick.fs made them from phypick.fs), their
\ data in phydata.fs, the 12 functions they call outside in phyidf.fs.
\ This package calls them as the reference does (esp-wifi-hal 0.3.3 with
\ esp-phy 0.2.0):
\
\   PHY-INIT ( -- n )           the radio on, with a full calibration;
\                               n is 1 if CAL-DATA held no valid data
\                               of an earlier calibration (so the first
\                               time), else 0; both times it calibrates
\   ch PHY-CHANNEL! ( ch -- )   the channel 1 to 13
\   PHY-TRACK ( -- )            adjust the power of the sender (after a
\                               few frames, as the reference does)
\   PHY-ON ( -- )  PHY-OFF ( -- )   the radio on again, off
\   PHY-LOW-OFF ( -- )          PHY rate and calibrated power tables
\
\ PHY-INIT first switches on the floating point coprocessor (fpu_on: the
\ code of C calculates with it; without the code, PHY-INIT stops here).
\ Then it does what the reference does before the calibration: the
\ clocks of the modem and of Wi-Fi and Bluetooth on (DPORT), no forced
\ power down and no isolation of Wi-Fi (RTC_CNTL; so after a reset). The
\ data of C get their start values again (a FREEZE after a calibration
\ saved changed data), and register_chipv7_phy calibrates with the
\ parameters in INIT-DATA. It writes the result of the calibration to
\ CAL-DATA and gives 1 if the data there before had a wrong check sum or
\ were old (ESP-IDF: ESP_CAL_DATA_CHECK_FAIL; ESP-IDF then saves the new
\ data), else 0.
\
\ INIT-DATA: the 128 bytes of parameters of ESP-IDF (phy_init_data.c,
\ Apache 2.0, Espressif Systems (Shanghai) Co., Ltd.) with a maximum power
\ of 20 dBm, as esp-phy 0.2.0 has them (phy_init_data_esp32.rs, MIT or
\ Apache 2.0). Only byte 44, the limit of the power at the lowest rate, is
\ that of ESP-IDF today: 78 (the table of the reference has 80).
\
\ Load it in ucf term, in the folder ucf, right after a reset (no tools
\ loaded: a CLINK that was there before would stay after the marker,
\ with pointers into freed RAM data):
\   #include lib/esp32/esp32/phy.fs
\ It loads the data of C into ESP32-PHY, then the tools and the code of C
\ of the selected PHY functions, links them, and forgets the tools and the names of
\ the code (the section "The link"). FREEZE then keeps ESP32-PHY in the
\ image. After MAC-code cleanup, ESP32-PHY keeps 7 432 dictionary bytes
\ and 5 172 RAM-data bytes; data of C 732 bytes, CAL-DATA 1 904 bytes,
\ the tables of .rodata 2 464 bytes (TABLES>RAM, below), code 47 484
\ bytes of SRAM0. No libpp MAC code is selected.
\ The test on the host (with mock.fs):
\ lib/esp32/test/phyhost.fs.

PACKAGE ESP32-PHY  ALSO ROM   \ lib/package.fs
PROTECTED   \ PHY-C (below) links the code of C with these words: CLINK finds them in ESP32-PHY.I

\ ==== The data of the code of C ========================================
\ CLINK (lib/esp32/clink.fs) has these four words too. ESP32-PHY has them
\ itself, because its data load before CLINK: the data stay, the tools
\ go (lib/esp32/README.md).

: DATA: ( u "name" -- )   \ u bytes in the RAM data region, 0 at the start; name gives them
  DUP DATA-ALLOT  DUP ROT 0 FILL  CONSTANT ;
: DATA-ALIGN ( n -- )   \ the next DATA: starts at a multiple of n (8)
  BEGIN VP @ OVER 1- AND WHILE  CELL DATA-ALLOT DROP  REPEAT  DROP ;
: INIT ( c1 .. cn n a -- )   \ the bytes c1 to cn to a, c1 first
  OVER + 1-  SWAP 0 ?DO  TUCK C!  1-  LOOP  DROP ;
: ," ( "text" -- )   \ the characters up to " into the dictionary, without a count
  34 PARSE  0 ?DO  COUNT C,  LOOP  DROP ;

VP @ CONSTANT C-DATA   \ the data of C in the RAM data region: from here
HERE CONSTANT TABLES-FROM   \ the words of phydata.fs: from here
#include phydata.fs
VP @ C-DATA - CONSTANT #C-DATA   \ their length

\ The tables of .rodata in phydata.fs (CREATE) go to the RAM data too: the
\ link puts their chip addresses into the code of C, and after FREEZE the
\ dictionary can lie in the flash, at another chip address (ESP32,
\ lib/esp32/esp32/bootimg.fs). Each table becomes a constant with the
\ address of its copy (code field 2, doc/vm.md section 6); its bytes stay
\ in the dictionary too.
VARIABLE T-END   VARIABLE T-RAM
VARIABLE #TABLES                    \ the bytes of the copies
: TABLE>RAM ( xt -- )   \ the body of a CREATE up to T-END: a copy in the RAM data; the word gives it
  DUP 2 CELLS +  T-END @ OVER -                     ( xt body u )
  DUP #TABLES +!  DUP VALLOT T-RAM !  T-RAM @ SWAP MOVE   ( xt )
  2 OVER !  T-RAM @ SWAP CELL+ ! ;
: TABLES>RAM ( -- )   \ the words from TABLES-FROM on, from the newest: each CREATE
  HERE T-END !  CURRENT @ @
  BEGIN DUP TABLES-FROM U< 0= WHILE
    DUP NAME>XT  DUP @ 1 = IF TABLE>RAM ELSE DROP THEN
    DUP T-END !  @                                  \ the next older word ends where this one starts
  REPEAT  DROP ;
TABLES>RAM
CREATE START-VALUES  #C-DATA ALLOT   \ their start values, from SAVE-START
: SAVE-START ( -- )   C-DATA START-VALUES #C-DATA MOVE ;   \ after the link, before a call
: RESET-DATA ( -- )   START-VALUES C-DATA #C-DATA MOVE ;   \ the data of C as after the link

\ ==== The parameters, the calibration, the functions of C =============

CREATE INIT-DATA   \ the 128 bytes of parameters for register_chipv7_phy
  $03 C, $03 C, $05 C, $09 C, $06 C, $05 C, $03 C, $06 C,
  $05 C, $04 C, $06 C, $04 C, $05 C, $00 C, $00 C, $00 C,
  $00 C, $05 C, $09 C, $06 C, $05 C, $03 C, $06 C, $05 C,
  $00 C, $00 C, $00 C, $00 C, $00 C, $00 C, $00 C, $00 C,
  $FC C, $FC C, $FE C, $F0 C, $F0 C, $F0 C, $E0 C, $E0 C,
  $E0 C, $18 C, $18 C, $18 C, $4E C, $48 C, $42 C, $3C C,   \ $4E ... $34: the power limits
  $38 C, $34 C, $00 C, $01 C, $01 C, $02 C, $02 C, $03 C,
  $04 C, $05 C,  HERE 70 0 FILL  70 ALLOT   \ 70 bytes 0
1904 BUFFER: CAL-DATA   \ the calibration data: register_chipv7_phy fills them
CAL-DATA 1904 0 FILL     \ no data yet, as in the reference (the RAM keeps old bytes)

\ The chip addresses of the functions of C; the link puts them here.
0 VALUE 'REGISTER   \ register_chipv7_phy ( init cal mode -- n )
0 VALUE 'WAKEUP     \ phy_wakeup_init ( -- )
0 VALUE 'CLOSE      \ phy_close_rf ( -- )
0 VALUE 'CHANNEL    \ chip_v7_set_chan_nomac ( ch 0 -- )
0 VALUE 'AGC-OFF    \ disable_wifi_agc ( -- )
0 VALUE 'AGC-ON     \ enable_wifi_agc ( -- )
0 VALUE 'TRACK      \ tx_pwctrl_background ( 1 0 -- )
0 VALUE 'LOW-OFF    \ phy_disable_low_rate ( -- )
0 VALUE 'FPU-ON     \ fpu_on ( -- ), phyidf.fs

: C-CALL ( x1 .. xn n a -- x )   \ call the function of C at a with n arguments
  DUP 0= ABORT" ESP32-PHY: the code of C is not linked"  SYSCALL ;

$3FF000CC CONSTANT WIFI-CLK-EN   \ DPORT: the clocks of Wi-Fi and Bluetooth
$406 CONSTANT MODEM-CLOCKS       \ the modem
$3C9 CONSTANT COMMON-CLOCKS      \ Wi-Fi and Bluetooth together
$3FF48084 CONSTANT DIG-PWC       \ RTC_CNTL: the power of the digital parts
$3FF48088 CONSTANT DIG-ISO       \ RTC_CNTL: their isolation

PUBLIC
: PHY-INIT ( -- n )   \ the radio on, with a full calibration; n: 1 if CAL-DATA was not valid
  0 'FPU-ON C-CALL DROP
  MODEM-CLOCKS COMMON-CLOCKS OR WIFI-CLK-EN BIS!
  1 17 LSHIFT DIG-PWC BIC!  1 28 LSHIFT DIG-ISO BIC!   \ Wi-Fi: no forced power down, no isolation
  RESET-DATA
  INIT-DATA >CHIP  CAL-DATA >CHIP  2  3 'REGISTER C-CALL ;   \ 2: a full calibration
: PHY-CHANNEL! ( ch -- )   \ the channel 1 to 13
  0 'AGC-OFF C-CALL DROP  0 2 'CHANNEL C-CALL DROP  0 'AGC-ON C-CALL DROP ;
: PHY-TRACK ( -- )   1 0 2 'TRACK C-CALL DROP ;   \ adjust the power of the sender
: PHY-LOW-OFF ( -- )   0 'LOW-OFF C-CALL DROP ;   \ PHY rate/power tables, not MAC setup
: PHY-ON ( -- )   0 'WAKEUP C-CALL DROP ;          \ the radio on again
: PHY-OFF ( -- )   0 'CLOSE C-CALL DROP ;          \ the radio off
END-PACKAGE

\ ==== The link =========================================================
\ The code of C loads after the marker PHY-TOOLS, with the tools that are
\ not there yet. When no cell waits for a name, the addresses of the
\ functions go to the VALUEs of ESP32-PHY and the data of C keep their
\ start values. Then the marker forgets the tools and the names of the
\ code; the code stays in SRAM0 (lib/esp32/esp32/asm.fs). A name without
\ code stops the load before the marker: then all names are still there.

MARKER PHY-TOOLS
#require FAIL ../../text.fs
#require ADD, ../../../asm/xtensa.fs
#require >CODE ../asm.fs
#require LINK-SYMBOLS ../clink.fs

PACKAGE PHY-C  ALSO ASM  ALSO CLINK  ALSO ESP32-PHY.I   \ lib/package.fs
' ESP32-PHY.I >BODY @ EXPORT   \ CLINK finds the data of C there
#include phyidf.fs
#include phyana.fs
#include phycal.fs
#include phychip.fs
#include phyrtc.fs
LINK-SYMBOLS
: LINKED ( -- )   \ stop if a cell still waits for a name
  UNRESOLVED? IF  CR .UNRESOLVED  TRUE ABORT" ESP32-PHY: these names have no code"  THEN ;
LINKED
' register_chipv7_phy >CODE TO 'REGISTER
' phy_wakeup_init >CODE TO 'WAKEUP
' phy_close_rf >CODE TO 'CLOSE
' chip_v7_set_chan_nomac >CODE TO 'CHANNEL
' disable_wifi_agc >CODE TO 'AGC-OFF
' enable_wifi_agc >CODE TO 'AGC-ON
' tx_pwctrl_background >CODE TO 'TRACK
' phy_disable_low_rate >CODE TO 'LOW-OFF
' fpu_on >CODE TO 'FPU-ON
SAVE-START
END-PACKAGE

PHY-TOOLS  ALSO ESP32-PHY   \ the tools and the names of the code go
