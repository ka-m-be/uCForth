\ phyrtc.fs - functions for ESP32-PHY, made by tools/pick.fs
\
\ tools/pick.fs made this file from the pick list
\ lib/esp32/esp32/phypick.fs: do not change it by hand. It has the
\ functions that the entries of the pick list need, each line as in its
\ package file of the quarry; their data are in phydata.fs. Changes:
\ the frames of the packages are not here, and a name that two objects
\ have gets the tag of its object (tag:name).
\
\ From:
\   blob/esp32/librtc/rtc_analog.fs:
\     rtc_analog.o of librtc.a (ESP32), 1 of its 19 functions
\ SHA-256 of the archives:
\   blob/esp32/librtc/librtc.a
\     e3411aeeb48d1da8acd304808290196baa0d6a2397db1a0c7741abee85dade61
\ Copyright Espressif Systems (Shanghai) Co., Ltd., Apache License 2.0
\ (blob/esp32/LICENSE). blob/esp32/s2fs.py rewrote the code of the
\ objects as CODE words for the assembler XTENSA (blob/esp32/README.md).
\ New names: chip:rodata.str1.4 chip:rodata ana:rodata ana:rodata.str1.4
\   cal:rodata.str1.4 cal:rodata

\ ==== rtc_analog.o of librtc.a (ESP32) ================================

CODE temprature_sens_read   \ .text.temprature_sens_read $58
  $0 22 LITERALS
    $3FF4884C T,  $FF00FFFF T,  $20000 T,  $2000000 T,  $FEFFFFFF T,  $1000000 T,
    $4000000 T,  $3FF48844 T,  $FBFFFFFF T,  $FDFFFFFF T,  $3FC00000 T,
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL ets_delay_us  SYMBOL esp_dport_access_reg_read  SYMBOL ets_delay_us
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
$58 L:
  A1 32 ENTRY,  A3 $0 L L32R,  A10 A3 MOV.N,  A8 $2C L L32R,  A8 CALLX8,
  A2 $4 L L32R,  A4 A10 A2 AND,  A10 A3 A3 OR,  A2 $8 L L32R,  A2 A4 A2 OR,  MEMW,
  A2 A3 0 S32I,  A8 $30 L L32R,  A8 CALLX8,
  A2 $C L L32R,  A2 A10 A2 OR,  A10 A3 MOV.N,  MEMW,  A2 A3 0 S32I,  A8 $34 L L32R,
  A8 CALLX8,
  A4 $10 L L32R,  A2 A10 A4 AND,  A10 A3 MOV.N,  MEMW,  A2 A3 0 S32I,
  A8 $38 L L32R,  A8 CALLX8,
  A2 $14 L L32R,  A2 A10 A2 OR,  A10 100 MOVI,  MEMW,  A2 A3 0 S32I.N,
  A8 $3C L L32R,  A8 CALLX8,
  A10 A3 MOV.N,  A8 $40 L L32R,  A8 CALLX8,
  A2 $18 L L32R,  A2 A10 A2 OR,  A10 2 MOVI.N,  MEMW,  A2 A3 0 S32I,  A8 $44 L L32R,
  A8 CALLX8,
  A10 $1C L L32R,  A8 $48 L L32R,  A8 CALLX8,
  A5 A10 MOV.N,  A10 A3 A3 OR,  A8 $4C L L32R,  A8 CALLX8,
  A8 $20 L L32R,  A8 A10 A8 AND,  A10 A3 MOV.N,  MEMW,  A8 A3 0 S32I.N,
  A8 $50 L L32R,  A8 CALLX8,
  A4 A10 A4 AND,  A10 A3 MOV.N,  MEMW,  A4 A3 0 S32I.N,  A8 $54 L L32R,  A8 CALLX8,
  A4 $24 L L32R,  A10 A10 A4 AND,  A2 $28 L L32R,  A2 A5 A2 AND,  A2 A2 22 8 EXTUI,
  MEMW,  A10 A3 0 S32I.N,  RETW.N,
$126 L:  END-CODE

