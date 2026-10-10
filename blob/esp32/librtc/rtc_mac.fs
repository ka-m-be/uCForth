\ rtc_mac.fs - package RTC-RTC-MAC: rtc_mac.o of librtc.a (ESP32)
\
\ Made from rtc_mac.S by s2fs.py, not by hand. Each function of C is a
\ CODE word: its bytes are those of the compiler, at the same offsets of
\ its section (L gives the address of an offset); its literals are at the
\ start of the word, after a jump. The data come first, then the
\ functions, each after those that it calls. The global symbols are
\ PUBLIC, the static ones PRIVATE. lib/esp32/clink.fs explains the words.
\
\ 2 functions (2 global), 1 data objects.
\ A function runs as C code with the windowed calls of the Xtensa: call
\ it with CALL-C ( x1 .. xn n xt -- x ), never by its name alone.
\
\ Names of other packages: a literal waits until its package comes
\ (LINK-SYMBOLS); a name outside the libraries until EXTERN gives it.
\ Outside the libraries (../README.md):
\   esp_dport_access_reg_read
\
\ Needs XTENSA (asm/xtensa.fs), ASM (lib/esp32/asm.fs) and CLINK
\ (lib/esp32/clink.fs) of ucf; the ESP32.

PACKAGE RTC-RTC-MAC  ALSO ASM  ALSO CLINK   \ lib/package.fs
EXPORTS   \ the PUBLIC words are symbols for the other packages

\ ==== Data =============================================================

\ ---- COMMON (.comm): data of C without a start value
12 DATA: tx_link


\ ==== Code =============================================================

CODE rtc_mac_tx_init   \ .text.rtc_mac_tx_init $110
  $0 68 LITERALS
    $B000 T,  $60033D24 T,  $60033C6C T,  $60033C78 T,  $600330D8 T,  $600330DC T,
    $600330E0 T,  $600330E4 T,  $10000000 T,  $60033040 T,  $60033044 T,
    $60033C40 T,  $60033C4C T,  SYMBOL tx_link  $C0000000 T,  $3FF5C860 T,
    $C00 T,  $3FF5C87C T,  $800 T,  $FFFFF3FF T,  $FFFFF7FF T,  $600340F0 T,
    $F9FFFFFF T,  $2000000 T,  $80000000 T,  $FFFE0FFF T,  $FFFFF000 T,
    $600340F8 T,  $600340FC T,  $FFFF0000 T,  $60033CEC T,  $F0FFFFFF T,
    $FFC00FFF T,  $60033CF0 T,  $8000000 T,  $F8FFFFFF T,  $FFF00000 T,  $FFFFF T,
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
$110 L:
  A1 32 ENTRY,  A3 $4 L L32R,  A4 2 MOVI.N,  A10 A3 MOV.N,  A8 $98 L L32R,
  A8 CALLX8,
  A10 A10 A4 OR,  MEMW,  A10 A3 0 S32I.N,
$128 L:
  A10 A3 MOV.N,  A8 $9C L L32R,  A8 CALLX8,
  A10 0 $128 L BBCI,
  A3 $8 L L32R,  A5 -3 MOVI.N,  A10 A3 MOV.N,  A8 $A0 L L32R,  A8 CALLX8,
  A10 A10 A5 AND,  MEMW,  A10 A3 0 S32I.N,  A10 A3 MOV.N,  A8 $A4 L L32R,
  A8 CALLX8,
  A6 -17 MOVI.N,  A6 A10 A6 AND,  A5 $C L L32R,  A4 7 MOVI.N,  A10 A5 MOV.N,
  A7 128 MOVI,  MEMW,  A6 A3 0 S32I.N,  A8 $A8 L L32R,  A8 CALLX8,
  A8 2 MOVI.N,  A8 A10 A8 OR,  A6 $10 L L32R,  A10 A6 A6 OR,  MEMW,  A8 A5 0 S32I,
  A8 $AC L L32R,  A8 CALLX8,
  A8 A10 A4 OR,  A5 $14 L L32R,  A10 A5 MOV.N,  MEMW,  A8 A6 0 S32I,  A8 $B0 L L32R,
  A8 CALLX8,
  A8 A10 A4 OR,  A6 $18 L L32R,  A10 A6 MOV.N,  MEMW,  A8 A5 0 S32I,  A8 $B4 L L32R,
  A8 CALLX8,
  A8 A10 A4 OR,  A5 $1C L L32R,  A10 A5 MOV.N,  MEMW,  A8 A6 0 S32I,  A8 $B8 L L32R,
  A8 CALLX8,
  A4 A10 A4 OR,  A10 A3 MOV.N,  MEMW,  A4 A5 0 S32I.N,  A8 $BC L L32R,  A8 CALLX8,
  A5 $20 L L32R,  A4 A10 A5 OR,  A10 A2 8 L32I.N,  A10 A10 8 ADDI.N,  MEMW,
  A4 A3 0 S32I.N,  A8 $C0 L L32R,  A8 CALLX8,
  A3 A10 MOV.N,  A3 A3 16 16 EXTUI,  A10 A2 8 L32I.N,  A10 A10 12 ADDI.N,
  A8 $C4 L L32R,  A8 CALLX8,
  A10 A10 16 SLLI,  A3 A3 A10 OR,  A4 $24 L L32R,  A10 A2 8 L32I.N,
  A10 A10 12 ADDI.N,  MEMW,  A3 A4 0 S32I.N,  A8 $C8 L L32R,  A8 CALLX8,
  A6 A10 16 16 EXTUI,  A3 $2C L L32R,  A10 A3 MOV.N,  A4 $28 L L32R,  MEMW,
  A6 A4 0 S32I,  A8 $CC L L32R,  A8 CALLX8,
  A4 A10 A7 OR,  A6 $30 L L32R,  A10 A6 MOV.N,  MEMW,  A4 A3 0 S32I,  A8 $D0 L L32R,
  A8 CALLX8,
  A10 A10 A7 OR,  A3 A2 0 L32I.N,  A3 A3 8 12 EXTUI,  A7 A3 12 SLLI,  A4 $38 L L32R,
  A4 A7 A4 ADD.N,  A3 A4 A3 ADD.N,  A7 A2 8 L32I.N,  A4 $34 L L32R,  MEMW,
  A10 A6 0 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A6 A2 0 L32I.N,  A3 A4 0 S32I.N,
  A3 0 MOVI.N,  A7 A4 4 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A7 $3C L L32R,
  A10 A7 MOV.N,  A3 A4 8 S32I.N,  A6 21 $2A0 L BBCI,
  A8 $D4 L L32R,  A8 CALLX8,
  A3 $40 L L32R,  A3 A10 A3 OR,  A6 $44 L L32R,  A10 A6 A6 OR,  MEMW,  A3 A7 0 S32I,
  A8 $D8 L L32R,  A8 CALLX8,
  A3 $48 L L32R,  A10 A10 A3 OR,  MEMW,  A10 A6 0 S32I.N,  $2C7 L J,
  2 ZEROS,
$2A0 L:
  A8 $DC L L32R,  A8 CALLX8,
  A3 $4C L L32R,  A3 A10 A3 AND,  A6 $44 L L32R,  A10 A6 MOV.N,  MEMW,
  A3 A7 0 S32I.N,  A8 $E0 L L32R,  A8 CALLX8,
  A3 $50 L L32R,  A10 A10 A3 AND,  MEMW,  A10 A6 0 S32I.N,
$2C7 L:
  A6 31 MOVI.N,  NOP.N,  NOP.N,  NOP.N,  A3 A2 0 L32I.N,  A3 A3 0 6 EXTUI,
  A6 A3 $2E8 L BGE,
  A3 A3 -16 ADDI,  A3 A3 A6 AND,  A3 A3 12 SLLI,  A6 0 MOVI.N,  A7 $54 L L32R,
  $3A0 L J,
$2E8 L:
  A6 23 MOVI.N,  A6 A3 $340 L BGE,
  A7 $54 L L32R,  A10 A7 MOV.N,  A8 $E4 L L32R,  A8 CALLX8,
  A3 A2 0 L32I.N,  A8 A3 22 1 EXTUI,  A6 A3 8 12 EXTUI,  A6 A6 8 SLLI,
  A9 $58 L L32R,  A8 A8 30 SLLI,  A10 A10 A9 AND,  A8 A8 A6 OR,  A6 A3 20 1 EXTUI,
  A9 $60 L L32R,  A6 A6 7 SLLI,  A8 A8 A9 OR,  A6 A8 A6 OR,  A3 A3 0 6 EXTUI,
  A8 $5C L L32R,  A3 A3 -24 ADDI,  A8 A10 A8 OR,  A6 A6 A3 OR,  MEMW,
  A8 A7 0 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A3 $0 L L32R,  $3A0 L J,
$340 L:
  A6 15 MOVI.N,  A6 A3 $395 L BGE,
  A7 $54 L L32R,  A10 A7 MOV.N,  A8 $E8 L L32R,  A8 CALLX8,
  A3 A2 0 L32I.N,  A11 A3 22 1 EXTUI,  A8 A3 8 12 EXTUI,  A6 A3 20 1 EXTUI,
  A9 $58 L L32R,  A11 A11 30 SLLI,  A10 A10 A9 AND,  A8 A8 8 SLLI,  A8 A11 A8 OR,
  A9 $5C L L32R,  A6 A6 7 SLLI,  A3 A3 0 6 EXTUI,  A3 A3 -16 ADDI,  A9 A10 A9 OR,
  A6 A8 A6 OR,  A6 A6 A3 OR,  MEMW,  A9 A7 0 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,
  A3 $0 L L32R,  $3A0 L J,
  3 ZEROS,
$395 L:
  A3 A3 0 5 EXTUI,  A7 $54 L L32R,  A3 A3 12 SLLI,  A6 0 MOVI.N,
$3A0 L:
  A10 A7 A7 OR,  A8 $EC L L32R,  A8 CALLX8,
  A8 $64 L L32R,  A10 A10 A8 AND,  A3 A10 A3 OR,  A10 A7 A7 OR,  MEMW,
  A3 A7 0 S32I,  A8 $F0 L L32R,  A8 CALLX8,
  A8 $68 L L32R,  A10 A10 A8 AND,  A3 A2 0 L32I.N,  A3 A3 8 12 EXTUI,  A3 A10 A3 OR,
  MEMW,  A3 A7 0 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A7 $6C L L32R,
  A3 A2 0 L32I.N,  MEMW,  A6 A7 0 S32I.N,  A3 20 $412 L BBCI,
  NOP.N,  NOP.N,  NOP.N,  NOP,  A6 $70 L L32R,  A10 A6 A6 OR,  A8 $F4 L L32R,
  A8 CALLX8,
  A3 A2 0 L32I.N,  A3 A3 8 12 EXTUI,  A7 $74 L L32R,  A10 A10 A7 AND,  A3 A10 A3 OR,
  MEMW,  A3 A6 0 S32I,
$412 L:
  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A6 $78 L L32R,  A10 A6 A6 OR,  A8 $F8 L L32R,
  A8 CALLX8,
  A3 A2 4 L32I.N,  A3 A3 12 4 EXTUI,  A3 A3 24 SLLI,  A7 $7C L L32R,
  A10 A10 A7 AND,  A3 A3 A10 OR,  A10 A6 A6 OR,  MEMW,  A3 A6 0 S32I,
  A8 $FC L L32R,  A8 CALLX8,
  A7 A2 4 L32I.N,  A7 A7 0 10 EXTUI,  A7 A7 12 SLLI,  A3 $80 L L32R,
  A10 A10 A3 AND,  A10 A7 A10 OR,  A7 A2 0 L32I.N,  MEMW,  A10 A6 0 S32I.N,  NOP.N,
  NOP.N,  NOP.N,  NOP.N,  A3 $84 L L32R,  A7 23 $481 L BBCI,
  A10 A3 MOV.N,  A8 $100 L L32R,  A8 CALLX8,
  A6 $88 L L32R,  A10 A10 A6 OR,  A7 A2 0 L32I.N,  MEMW,  A10 A3 0 S32I.N,
$481 L:
  A7 24 $494 L BBCI,
  A10 A3 MOV.N,  A8 $104 L L32R,  A8 CALLX8,
  A10 A10 A5 OR,  MEMW,  A10 A3 0 S32I.N,
$494 L:
  A10 A3 MOV.N,  A8 $108 L L32R,  A8 CALLX8,
  A2 A2 0 L32I.N,  A2 A2 25 1 EXTUI,  A2 A2 24 SLLI,  A5 $8C L L32R,
  A10 A10 A5 AND,  A2 A2 A10 OR,  A10 A3 A3 OR,  MEMW,  A2 A3 0 S32I,
  A8 $10C L L32R,  A8 CALLX8,
  A2 $90 L L32R,  A10 A10 A2 AND,  A2 $94 L L32R,  A4 A4 A2 AND,  A4 A10 A4 OR,
  MEMW,  A4 A3 0 S32I.N,  RETW.N,
$4D2 L:  END-CODE

CODE rtc_mac_tx   \ .text.rtc_mac_tx $34
  $0 13 LITERALS
    $60033C4C T,  $60033CF0 T,  $C0000000 T,  $60033C48 T,  $60033CC8 T,
    $60033CC4 T,  $60034108 T,  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read
$34 L:
  A1 32 ENTRY,  A3 $0 L L32R,  A2 128 MOVI,  A10 A3 MOV.N,  A8 $1C L L32R,
  A8 CALLX8,
  A8 A10 A2 OR,  A4 $4 L L32R,  A10 A4 MOV.N,  MEMW,  A8 A3 0 S32I.N,
  A8 $20 L L32R,  A8 CALLX8,
  A3 $8 L L32R,  A10 A10 A3 OR,  A3 $C L L32R,  MEMW,  A10 A4 0 S32I.N,
$66 L:
  A10 A3 MOV.N,  A8 $24 L L32R,  A8 CALLX8,
  A10 A2 $66 L BNONE,
  A10 $10 L L32R,  A3 64 MOVI,  A8 $28 L L32R,  A8 CALLX8,
  A2 -1 MOVI.N,  A10 A3 $A0 L BNONE,
  A2 $14 L L32R,  A10 A2 MOV.N,  A8 $2C L L32R,  A8 CALLX8,
  A3 A10 A3 OR,  A10 $18 L L32R,  MEMW,  A3 A2 0 S32I.N,  A8 $30 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,
$A0 L:
  RETW.N,
$A2 L:  END-CODE

LINK-SYMBOLS   \ the cells of other packages that wait for these names
END-PACKAGE  PREVIOUS   \ not in the search order: ALSO name gives its words (README.md)
