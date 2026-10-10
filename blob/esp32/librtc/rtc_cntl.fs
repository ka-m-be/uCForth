\ rtc_cntl.fs - package RTC-RTC-CNTL: rtc_cntl.o of librtc.a (ESP32)
\
\ Made from rtc_cntl.S by s2fs.py, not by hand. Each function of C is a
\ CODE word: its bytes are those of the compiler, at the same offsets of
\ its section (L gives the address of an offset); its literals are at the
\ start of the word, after a jump. The data come first, then the
\ functions, each after those that it calls. The global symbols are
\ PUBLIC, the static ones PRIVATE. lib/esp32/clink.fs explains the words.
\
\ 4 functions (4 global), 0 data objects.
\ A function runs as C code with the windowed calls of the Xtensa: call
\ it with CALL-C ( x1 .. xn n xt -- x ), never by its name alone.
\
\ Names of other packages: a literal waits until its package comes
\ (LINK-SYMBOLS); a name outside the libraries until EXTERN gives it.
\ Outside the libraries (../README.md):
\   esp_dport_access_reg_read rtc_dbias_cfg
\
\ Needs XTENSA (asm/xtensa.fs), ASM (lib/esp32/asm.fs) and CLINK
\ (lib/esp32/clink.fs) of ucf; the ESP32.

PACKAGE RTC-RTC-CNTL  ALSO ASM  ALSO CLINK   \ lib/package.fs
EXPORTS   \ the PUBLIC words are symbols for the other packages

\ ==== Code =============================================================

CODE rtc_wifi_force_pd_off   \ .text.rtc_wifi_force_pd_off $28
  $0 10 LITERALS
    $3FF48084 T,  $FFFDFFFF T,  $3FF48088 T,  $EFFFFFFF T,  $3FF000D0 T,
    $3FF000CC T,  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read
$28 L:
  A1 32 ENTRY,  A3 $0 L L32R,  A10 A3 MOV.N,  A8 $18 L L32R,  A8 CALLX8,
  A8 $4 L L32R,  A8 A10 A8 AND,  A2 $8 L L32R,  A10 A2 MOV.N,  MEMW,
  A8 A3 0 S32I.N,  A8 $1C L L32R,  A8 CALLX8,
  A8 $C L L32R,  A8 A10 A8 AND,  A3 $10 L L32R,  A10 A3 MOV.N,  MEMW,
  A8 A2 0 S32I.N,  A8 $20 L L32R,  A8 CALLX8,
  A8 -2 MOVI.N,  A8 A10 A8 AND,  A2 $14 L L32R,  A10 A2 MOV.N,  MEMW,  A8 A3 0 S32I,
  A8 $24 L L32R,  A8 CALLX8,
  A8 $40F MOVI,  A10 A10 A8 OR,  MEMW,  A10 A2 0 S32I.N,  RETW.N,
$85 L:  END-CODE

CODE rtc_digital_lp_mode_off_stg1   \ .text.rtc_digital_lp_mode_off_stg1 $44
  $0 17 LITERALS
    $3FF48000 T,  $FFDFFFFF T,  $FFFBFFFF T,  $FFFFEFFF T,  $3FF48084 T,
    $FFFF555F T,  $3FF48088 T,  $FAAAFFFF T,  $3FF000C0 T,  $3FF000CC T,
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL rtc_wifi_force_pd_off
    SYMBOL rtc_dbias_cfg
$44 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A10 A2 MOV.N,  A8 $28 L L32R,  A8 CALLX8,
  A8 $4 L L32R,  A8 A10 A8 AND,  A10 A2 MOV.N,  MEMW,  A8 A2 0 S32I.N,
  A8 $2C L L32R,  A8 CALLX8,
  A8 $8 L L32R,  A8 A10 A8 AND,  A10 A2 MOV.N,  MEMW,  A8 A2 0 S32I.N,
  A8 $30 L L32R,  A8 CALLX8,
  A8 $C L L32R,  A8 A10 A8 AND,  A4 $10 L L32R,  A10 A4 A4 OR,  MEMW,  A8 A2 0 S32I,
  A8 $34 L L32R,  A8 CALLX8,
  A2 $14 L L32R,  A2 A10 A2 AND,  A3 $18 L L32R,  A10 A3 MOV.N,  MEMW,
  A2 A4 0 S32I.N,  A8 $38 L L32R,  A8 CALLX8,
  A8 -1 MOVI.N,  A2 $1C L L32R,  A10 A10 A2 AND,  A2 $20 L L32R,  MEMW,
  A10 A3 0 S32I.N,  MEMW,  A8 A2 0 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,
  A2 $24 L L32R,  MEMW,  A8 A2 0 S32I,  A8 $3C L L32R,  A8 CALLX8,
  A13 0 MOVI.N,  A12 A13 MOV.N,  A11 A13 MOV.N,  A10 4 MOVI.N,  A8 $40 L L32R,
  A8 CALLX8,
  RETW.N,
$E2 L:  END-CODE

CODE rtc_digital_lp_mode_off_stg2   \ .text.rtc_digital_lp_mode_off_stg2 $8
  $0 2 LITERALS
    $3FF48000 T,  SYMBOL esp_dport_access_reg_read
$8 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A10 A2 MOV.N,  A8 $4 L L32R,  A8 CALLX8,
  A8 -1281 MOVI,  A10 A10 A8 AND,  MEMW,  A10 A2 0 S32I.N,  RETW.N,
$23 L:  END-CODE

CODE rtc_sar_sleep_timer_start   \ .text.rtc_sar_sleep_timer_start $18
  $0 6 LITERALS
    $3FF4882C T,  $3FF48818 T,  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read
$18 L:
  A1 32 ENTRY,  A3 $0 L L32R,  A10 A3 MOV.N,  A8 $8 L L32R,  A8 CALLX8,
  A8 $100 MOVI,  A10 A10 A8 OR,  MEMW,  A10 A3 0 S32I.N,  A10 A3 MOV.N,
  A8 $C L L32R,  A8 CALLX8,
  A8 -513 MOVI,  A10 A10 A8 AND,  MEMW,  A10 A3 0 S32I.N,  A10 A3 MOV.N,
  A8 $10 L L32R,  A8 CALLX8,
  A8 $200 MOVI,  A8 A10 A8 OR,  MEMW,  A8 A3 0 S32I.N,  A10 A3 MOV.N,  NOP.N,
  NOP.N,  NOP.N,  A8 $4 L L32R,  MEMW,  A2 A8 0 S32I.N,  A8 $14 L L32R,  A8 CALLX8,
  A8 -257 MOVI,  A10 A10 A8 AND,  MEMW,  A10 A3 0 S32I.N,  RETW.N,
$7A L:  END-CODE

LINK-SYMBOLS   \ the cells of other packages that wait for these names
END-PACKAGE  PREVIOUS   \ not in the search order: ALSO name gives its words (README.md)
