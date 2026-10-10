\ rtc_analog.fs - package RTC-RTC-ANALOG: rtc_analog.o of librtc.a (ESP32)
\
\ Made from rtc_analog.S by s2fs.py, not by hand. Each function of C is a
\ CODE word: its bytes are those of the compiler, at the same offsets of
\ its section (L gives the address of an offset); its literals are at the
\ start of the word, after a jump. The data come first, then the
\ functions, each after those that it calls. The global symbols are
\ PUBLIC, the static ones PRIVATE. lib/esp32/clink.fs explains the words.
\
\ 19 functions (19 global), 2 data objects.
\ A function runs as C code with the windowed calls of the Xtensa: call
\ it with CALL-C ( x1 .. xn n xt -- x ), never by its name alone.
\
\ Names of other packages: a literal waits until its package comes
\ (LINK-SYMBOLS); a name outside the libraries until EXTERN gives it.
\ Uses:
\   PHY-PHY-CHIP-V7  g_phyFuns
\   RTC-RTC  rtc_pads_funie rtc_pads_muxsel rtc_pads_pd rtc_pads_pu
\            rtc_pads_slpie rtc_pads_slpoe
\ Outside the libraries (../README.md):
\   esp_dport_access_reg_read ets_delay_us
\
\ Needs XTENSA (asm/xtensa.fs), ASM (lib/esp32/asm.fs) and CLINK
\ (lib/esp32/clink.fs) of ucf; the ESP32.

PACKAGE RTC-RTC-ANALOG  ALSO ASM  ALSO CLINK   \ lib/package.fs
EXPORTS   \ the PUBLIC words are symbols for the other packages

\ ==== Data =============================================================

PRIVATE
CREATE CSWTCH$19   \ .rodata.CSWTCH$19, 40 bytes
  $00 C,  $04 C,  $00 C,  $00 C,  $00 C,  $08 C,  $00 C,  $00 C,  $00 C,  $10 C,
  $00 C,  $00 C,  $00 C,  $20 C,  $00 C,  $00 C,  $00 C,  $40 C,  $00 C,  $00 C,
  $00 C,  $80 C,  $00 C,  $00 C,  $00 C,  $00 C,  $01 C,  $00 C,  $00 C,  $00 C,
  $02 C,  $00 C,  $40 C,  $00 C,  $00 C,  $00 C,  $80 C,  $00 C,  $00 C,  $00 C,
CREATE CSWTCH$13   \ .rodata.CSWTCH$13, 32 bytes
  $01 C,  $00 C,  $00 C,  $00 C,  $02 C,  $00 C,  $00 C,  $00 C,  $04 C,  $00 C,
  $00 C,  $00 C,  $08 C,  $00 C,  $00 C,  $00 C,  $00 C,  $02 C,  $00 C,  $00 C,
  $00 C,  $01 C,  $00 C,  $00 C,  $10 C,  $00 C,  $00 C,  $00 C,  $20 C,  $00 C,
  $00 C,  $00 C,

\ ==== Code =============================================================

PUBLIC
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

CODE dac_out   \ .text.dac_out $B8
  $0 46 LITERALS
    $3FF48898 T,  $FFFF0000 T,  $10000 T,  $FFFEFFFF T,  $3FF4889C T,  $1000000 T,
    $FFFCFFFF T,  $FFCFFFFF T,  $200000 T,  $FEFFFFFF T,  $3FF48484 T,
    $F807FFFF T,  $2000000 T,  $FFF3FFFF T,  $FFFF00FF T,  $FF3FFFFF T,
    $800000 T,  $FDFFFFFF T,  $3FF48488 T,  $20000 T,  $E7FFFFFF T,  $40400 T,
    $FFFBFBFF T,  SYMBOL esp_dport_access_reg_read
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
$B8 L:
  A1 48 ENTRY,  A2 A2 0 8 EXTUI,  A2 A1 0 S32I,  A2 A6 0 16 EXTUI,
  A4 A4 0 16 EXTUI,  A3 A3 0 8 EXTUI,  A7 A4 0 8 EXTUI,  A6 $0 L L32R,
  A5 A5 0 16 EXTUI,  A4 A4 8 SRLI,  A10 A6 MOV.N,  A3 $109 L BEQZ.N,
  A8 $5C L L32R,  A8 CALLX8,
  A9 $4 L L32R,  A10 A10 A9 AND,  A8 A10 A2 OR,  A10 A6 A6 OR,  MEMW,  A8 A6 0 S32I,
  A8 $60 L L32R,  A8 CALLX8,
  A8 $8 L L32R,  A10 A10 A8 OR,  MEMW,  A10 A6 0 S32I.N,  A3 0 $11D L BBSI,
  $18F L J,
$109 L:
  A8 $64 L L32R,  A8 CALLX8,
  A8 $C L L32R,  A10 A10 A8 AND,  MEMW,  A10 A6 0 S32I.N,  $18F L J,
$11D L:
  A2 $10 L L32R,  A10 A2 A2 OR,  A8 $68 L L32R,  A8 CALLX8,
  A6 $14 L L32R,  A6 A10 A6 OR,  A10 A2 MOV.N,  MEMW,  A6 A2 0 S32I.N,
  A8 $6C L L32R,  A8 CALLX8,
  A6 A5 0 2 EXTUI,  A6 A6 16 SLLI,  A9 $18 L L32R,  A10 A10 A9 AND,  A6 A6 A10 OR,
  A10 A2 MOV.N,  MEMW,  A6 A2 0 S32I.N,  A8 $70 L L32R,  A8 CALLX8,
  A6 -256 MOVI,  A10 A10 A6 AND,  A7 A10 A7 OR,  MEMW,  A7 A2 0 S32I,  A10 A2 A2 OR,
  A8 $74 L L32R,  A8 CALLX8,
  A6 $1C L L32R,  A10 A10 A6 AND,  A6 $20 L L32R,  A10 A10 A6 OR,  MEMW,
  A10 A2 0 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A6 $28 L L32R,  $1C2 L J,
$18F L:
  A2 $10 L L32R,  A7 A7 19 SLLI,  A10 A2 MOV.N,  A8 $78 L L32R,  A8 CALLX8,
  A9 $24 L L32R,  A9 A10 A9 AND,  A6 $28 L L32R,  A10 A6 A6 OR,  MEMW,
  A9 A2 0 S32I.N,  A8 $7C L L32R,  A8 CALLX8,
  A9 $2C L L32R,  A10 A10 A9 AND,  A7 A10 A7 OR,  MEMW,  A7 A6 0 S32I.N,
$1C2 L:
  A10 A2 MOV.N,  A3 1 $239 L BBCI,
  A8 $80 L L32R,  A8 CALLX8,
  A5 A5 8 2 EXTUI,  A3 $30 L L32R,  A5 A5 18 SLLI,  A3 A10 A3 OR,  A10 A2 MOV.N,
  A4 A4 8 SLLI,  MEMW,  A3 A2 0 S32I.N,  A8 $84 L L32R,  A8 CALLX8,
  A3 $34 L L32R,  A10 A10 A3 AND,  A5 A5 A10 OR,  A10 A2 A2 OR,  MEMW,
  A5 A2 0 S32I.N,  A8 $88 L L32R,  A8 CALLX8,
  A3 $38 L L32R,  A10 A10 A3 AND,  A4 A10 A4 OR,  A10 A2 A2 OR,  MEMW,
  A4 A2 0 S32I,  A8 $8C L L32R,  A8 CALLX8,
  A3 $3C L L32R,  A10 A10 A3 AND,  A3 $40 L L32R,  A10 A10 A3 OR,  MEMW,
  A10 A2 0 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A3 $48 L L32R,  $266 L J,
  2 ZEROS,
$239 L:
  A8 $90 L L32R,  A8 CALLX8,
  A5 $44 L L32R,  A4 A4 19 SLLI,  A5 A10 A5 AND,  A3 $48 L L32R,  A10 A3 MOV.N,
  MEMW,  A5 A2 0 S32I.N,  A8 $94 L L32R,  A8 CALLX8,
  A5 $2C L L32R,  A10 A10 A5 AND,  A4 A10 A4 OR,  MEMW,  A4 A3 0 S32I.N,
$266 L:
  A10 A6 MOV.N,  NOP.N,  NOP.N,  NOP.N,  A2 A1 0 L32I.N,  A2 0 $2B0 L BBCI,
  A8 $98 L L32R,  A8 CALLX8,
  A4 $4C L L32R,  A4 A10 A4 OR,  A10 A6 MOV.N,  MEMW,  A4 A6 0 S32I,  A8 $9C L L32R,
  A8 CALLX8,
  A4 $50 L L32R,  A4 A10 A4 AND,  A10 A6 MOV.N,  MEMW,  A4 A6 0 S32I.N,
  A8 $A0 L L32R,  A8 CALLX8,
  A4 $54 L L32R,  A10 A10 A4 OR,  MEMW,  A10 A6 0 S32I.N,  $2C1 L J,
  2 ZEROS,
$2B0 L:
  A8 $A4 L L32R,  A8 CALLX8,
  A4 $58 L L32R,  A10 A10 A4 AND,  MEMW,  A10 A6 0 S32I.N,
$2C1 L:
  A10 A3 MOV.N,  NOP.N,  NOP.N,  NOP.N,  A2 A1 0 L32I,  A2 1 $309 L BBCI,
  A8 $A8 L L32R,  A8 CALLX8,
  A2 $4C L L32R,  A2 A10 A2 OR,  A10 A3 MOV.N,  MEMW,  A2 A3 0 S32I,  A8 $AC L L32R,
  A8 CALLX8,
  A2 $50 L L32R,  A2 A10 A2 AND,  A10 A3 MOV.N,  MEMW,  A2 A3 0 S32I.N,
  A8 $B0 L L32R,  A8 CALLX8,
  A2 $54 L L32R,  A10 A10 A2 OR,  MEMW,  A10 A3 0 S32I.N,  RETW.N,
$309 L:
  A8 $B4 L L32R,  A8 CALLX8,
  A2 $58 L L32R,  A10 A10 A2 AND,  MEMW,  A10 A3 0 S32I.N,  RETW.N,
$31C L:  END-CODE

CODE vdd33_init   \ .text.vdd33_init $74
  $0 29 LITERALS
    $3FF4882C T,  $3FF460C0 T,  $3FF4E050 T,  SYMBOL g_phyFuns  $3FF48890 T,
    $8000000 T,  $3FF48030 T,  $3FF4880C T,  $C0000 T,  $3FF4E05C T,  $800000 T,
    $FFDFFFFF T,  $3FF48894 T,  $8007FFFF T,  $3FF48838 T,  $3FFFFFFF T,
    $3FF66010 T,  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read
$74 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A10 A2 MOV.N,  A8 $44 L L32R,  A8 CALLX8,
  A8 -225 MOVI,  A3 $4 L L32R,  A8 A10 A8 AND,  A10 A3 MOV.N,  MEMW,  A8 A2 0 S32I,
  A8 $48 L L32R,  A8 CALLX8,
  A8 128 MOVI,  A8 A10 A8 OR,  A10 A2 MOV.N,  MEMW,  A8 A3 0 S32I.N,  A8 $4C L L32R,
  A8 CALLX8,
  A8 12 MOVI.N,  A8 A10 A8 OR,  A3 $8 L L32R,  A10 A3 MOV.N,  MEMW,  A8 A2 0 S32I.N,
  A8 $50 L L32R,  A8 CALLX8,
  A2 192 MOVI,  A10 A10 A2 OR,  A15 0 MOVI.N,  A2 $C L L32R,  A13 2 MOVI.N,
  A14 A15 MOV.N,  A12 9 MOVI.N,  A11 A13 MOV.N,  A2 A2 0 L32I.N,  A2 A2 168 L32I,
  MEMW,  A10 A3 0 S32I,  A10 107 MOVI,  A2 CALLX8,
  A3 $10 L L32R,  A10 A3 A3 OR,  A8 $54 L L32R,  A8 CALLX8,
  A2 $14 L L32R,  A8 A10 A2 OR,  A4 $18 L L32R,  A10 A4 A4 OR,  MEMW,  A8 A3 0 S32I,
  A8 $58 L L32R,  A8 CALLX8,
  A2 A10 A2 OR,  A3 $1C L L32R,  A10 A3 MOV.N,  MEMW,  A2 A4 0 S32I,  A8 $5C L L32R,
  A8 CALLX8,
  A8 $20 L L32R,  A8 A10 A8 OR,  A2 $24 L L32R,  A10 A2 A2 OR,  MEMW,  A8 A3 0 S32I,
  A8 $60 L L32R,  A8 CALLX8,
  A8 $28 L L32R,  A8 A10 A8 OR,  A10 A2 MOV.N,  MEMW,  A8 A2 0 S32I.N,
  A8 $64 L L32R,  A8 CALLX8,
  A8 $2C L L32R,  A8 A10 A8 AND,  A3 $30 L L32R,  A10 A3 A3 OR,  MEMW,
  A8 A2 0 S32I,  A8 $68 L L32R,  A8 CALLX8,
  A2 $34 L L32R,  A2 A10 A2 AND,  A4 $38 L L32R,  A10 A4 A4 OR,  MEMW,
  A2 A3 0 S32I,  A8 $6C L L32R,  A8 CALLX8,
  A2 $3C L L32R,  A2 A10 A2 AND,  A3 $40 L L32R,  A10 A3 MOV.N,  MEMW,
  A2 A4 0 S32I.N,  A8 $70 L L32R,  A8 CALLX8,
  A2 -5 MOVI.N,  A10 A10 A2 AND,  MEMW,  A10 A3 0 S32I.N,  RETW.N,
$19A L:  END-CODE

CODE get_vdd33   \ .text.get_vdd33 $4C
  $0 19 LITERALS
    $3FF4E080 T,  SYMBOL g_phyFuns  $3FF4882C T,  $3FF4E05C T,  $800000 T,
    $3FF4E050 T,  $3FF4E0A0 T,  $FF7FFFFF T,  $FFF T,
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL ets_delay_us
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
$4C L:
  A1 32 ENTRY,  A5 $4 L L32R,  A14 7 MOVI.N,  A15 1 MOVI.N,  A13 A14 MOV.N,
  A12 9 MOVI,  A2 A5 0 L32I,  A11 2 MOVI.N,  A10 107 MOVI,  A2 A2 168 L32I,
  A2 CALLX8,
  A2 16 MOVI,  A4 $8 L L32R,  A10 A4 A4 OR,  A8 $24 L L32R,  A8 CALLX8,
  A2 A10 A2 OR,  A3 $C L L32R,  A10 A3 A3 OR,  MEMW,  A2 A4 0 S32I.N,
  A8 $28 L L32R,  A8 CALLX8,
  A2 $10 L L32R,  A10 A10 A2 OR,  A2 $14 L L32R,  MEMW,  A10 A3 0 S32I.N,
$9A L:
  A10 A2 MOV.N,  A8 $2C L L32R,  A8 CALLX8,
  A10 A10 24 3 EXTUI,  A10 $9A L BNEZ,
  A10 A2 MOV.N,  A8 $30 L L32R,  A8 CALLX8,
  A6 -3 MOVI.N,  A10 A10 A6 AND,  MEMW,  A10 A2 0 S32I.N,  A10 A2 MOV.N,
  A8 $34 L L32R,  A8 CALLX8,
  A6 2 MOVI.N,  A10 A10 A6 OR,  MEMW,  A10 A2 0 S32I.N,  A10 10 MOVI.N,
  A8 $38 L L32R,  A8 CALLX8,
$D4 L:
  A10 A2 MOV.N,  A8 $3C L L32R,  A8 CALLX8,
  A10 A10 24 3 EXTUI,  A10 $D4 L BNEZ,
  A2 $0 L L32R,  A6 A10 MOV.N,  A7 $18 L L32R,
$EA L:
  A10 A2 MOV.N,  A8 $40 L L32R,  A8 CALLX8,
  A10 A10 0 12 EXTUI,  A2 A2 4 ADDI.N,  A6 A6 A10 ADD.N,  A2 A7 $EA L BNE,
  A2 A5 0 L32I.N,  A14 7 MOVI.N,  A15 0 MOVI.N,  A13 A14 MOV.N,  A12 9 MOVI.N,
  A2 A2 168 L32I,  A11 2 MOVI,  A10 107 MOVI,  A2 CALLX8,
  A10 A4 MOV.N,  A8 $44 L L32R,  A8 CALLX8,
  A2 -17 MOVI.N,  A10 A10 A2 AND,  MEMW,  A10 A4 0 S32I.N,  A10 A3 MOV.N,
  A8 $48 L L32R,  A8 CALLX8,
  A6 A6 4 ADDI.N,  A6 A6 3 SRLI,  A2 $1C L L32R,  A10 A10 A2 AND,  A2 $20 L L32R,
  A2 A2 A6 SUB,  A2 A2 0 16 EXTUI,  MEMW,  A10 A3 0 S32I.N,  RETW.N,
$147 L:  END-CODE

CODE adc1_read_test   \ .text.adc1_read_test $B0
  $0 44 LITERALS
    $3FF4882C T,  $3FF48800 T,  $30000 T,  $3FF48854 T,  $40000 T,  $10000000 T,
    $3FF4880C T,  $FFFCFFFF T,  $20000 T,  $3FF48834 T,  $8007FFFF T,
    $3FF488A0 T,  $3FF48810 T,  $FFFFF0FF T,  $FFFF0FFF T,  $3FF48808 T,
    $FFFF0000 T,  $10000 T,  $80000000 T,  $3FF4883C T,  $1C00000 T,  $FFFDFFFF T,
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
$B0 L:
  A1 48 ENTRY,  A5 $0 L L32R,  A7 15 MOVI.N,  A10 A5 MOV.N,  A8 $58 L L32R,
  A8 CALLX8,
  A7 A10 A7 OR,  A6 $4 L L32R,  A8 A4 0 16 EXTUI,  A10 A6 MOV.N,  A2 A2 0 8 EXTUI,
  A3 A3 0 8 EXTUI,  MEMW,  A7 A5 0 S32I.N,  A3 A3 0 2 EXTUI,  A8 A1 0 S32I.N,
  A8 $5C L L32R,  A8 CALLX8,
  A4 $8 L L32R,  A4 A10 A4 OR,  A5 $C L L32R,  A10 A5 A5 OR,  MEMW,  A4 A6 0 S32I,
  A8 $60 L L32R,  A8 CALLX8,
  A4 $10 L L32R,  A4 A10 A4 OR,  A10 A6 MOV.N,  MEMW,  A4 A5 0 S32I.N,
  A8 $64 L L32R,  A8 CALLX8,
  A4 $14 L L32R,  A4 A10 A4 OR,  A7 $18 L L32R,  A10 A7 MOV.N,  MEMW,
  A4 A6 0 S32I.N,  A8 $68 L L32R,  A8 CALLX8,
  A6 1 MOVI.N,  A9 $1C L L32R,  A10 A10 A9 AND,  A4 $20 L L32R,  A9 A10 A4 OR,
  A4 $24 L L32R,  A10 A4 MOV.N,  MEMW,  A9 A7 0 S32I.N,  A8 $6C L L32R,  A8 CALLX8,
  A11 A2 1 SLLI,  A9 3 MOVI.N,  A12 -1 MOVI.N,  A11 SSL,  A9 A9 SLL,  A9 A12 A9 XOR,
  A10 A9 A10 AND,  A11 SSL,  A3 A3 SLL,  A3 A10 A3 OR,  MEMW,  A3 A4 0 S32I.N,
  A10 A5 MOV.N,  A8 $70 L L32R,  A8 CALLX8,
  A2 SSL,  A2 A6 SLL,  A2 A2 0 12 EXTUI,  A3 $28 L L32R,  A2 A2 19 SLLI,
  A10 A10 A3 AND,  A2 A2 A10 OR,  A4 -16 MOVI.N,  A3 $2C L L32R,  A10 A3 MOV.N,
  MEMW,  A2 A5 0 S32I,  A8 $74 L L32R,  A8 CALLX8,
  A11 A10 A4 AND,  A2 $30 L L32R,  A10 A2 MOV.N,  MEMW,  A11 A3 0 S32I,
  A8 $78 L L32R,  A8 CALLX8,
  A4 A10 A4 AND,  A10 A2 MOV.N,  A3 -241 MOVI,  MEMW,  A4 A2 0 S32I,  A8 $7C L L32R,
  A8 CALLX8,
  A3 A10 A3 AND,  A10 A2 MOV.N,  MEMW,  A3 A2 0 S32I.N,  A8 $80 L L32R,  A8 CALLX8,
  A3 $34 L L32R,  A3 A10 A3 AND,  A10 A2 MOV.N,  MEMW,  A3 A2 0 S32I,
  A8 $84 L L32R,  A8 CALLX8,
  A3 $38 L L32R,  A3 A10 A3 AND,  A4 $3C L L32R,  A10 A4 MOV.N,  MEMW,
  A3 A2 0 S32I,  A8 $88 L L32R,  A8 CALLX8,
  A3 $40 L L32R,  A10 A10 A3 AND,  A8 A1 0 L32I.N,  A8 A10 A8 OR,  A10 A4 MOV.N,
  MEMW,  A8 A4 0 S32I,  A8 $8C L L32R,  A8 CALLX8,
  A8 A10 0 16 EXTUI,  A10 A7 MOV.N,  A2 $44 L L32R,  A8 A8 A2 OR,  MEMW,
  A8 A4 0 S32I,  A8 $90 L L32R,  A8 CALLX8,
  A10 A10 A3 AND,  A9 A10 A6 OR,  A10 A5 MOV.N,  MEMW,  A9 A7 0 S32I,
  A8 $94 L L32R,  A8 CALLX8,
  A3 $48 L L32R,  A10 A10 A3 OR,  A4 $4C L L32R,  A3 $50 L L32R,  MEMW,
  A10 A5 0 S32I.N,
$249 L:
  A10 A4 MOV.N,  A8 $98 L L32R,  A8 CALLX8,
  A10 A10 A3 AND,  A10 A10 22 10 EXTUI,  A10 $249 L BNEZ,
  A10 A5 MOV.N,  A8 $9C L L32R,  A8 CALLX8,
  A3 $54 L L32R,  A3 A10 A3 AND,  A10 A5 MOV.N,  MEMW,  A3 A5 0 S32I.N,
  A8 $A0 L L32R,  A8 CALLX8,
  A4 $20 L L32R,  A10 A10 A4 OR,  MEMW,  A10 A5 0 S32I,
$281 L:
  A10 A5 MOV.N,  A8 $A4 L L32R,  A8 CALLX8,
  A10 A2 $281 L BNONE,
  A10 A5 MOV.N,  A8 $A8 L L32R,  A8 CALLX8,
  A2 A10 A10 OR,  A10 A5 A5 OR,  A8 $AC L L32R,  A8 CALLX8,
  A3 $28 L L32R,  A2 A2 0 16 EXTUI,  A10 A10 A3 AND,  MEMW,  A10 A5 0 S32I.N,
  RETW.N,
$2B0 L:  END-CODE

CODE adc1_amp_read_full   \ .text.adc1_amp_read_full $B0
  $0 44 LITERALS
    $3FF4882C T,  $3FF48800 T,  $30000 T,  $3FF48854 T,  $40000 T,  $10000000 T,
    $3FF48834 T,  $8007FFFF T,  $80000000 T,  $3FF48810 T,  $FFFF0FFF T,
    $FFFFF0FF T,  $FF0FFFFF T,  $300000 T,  $F0000 T,  $3FF48808 T,  $FFFF0000 T,
    $3FF4880C T,  $FFFDFFFF T,  $20000 T,  $10000 T,  $FFFBFFFF T,
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL ets_delay_us  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
$B0 L:
  A1 48 ENTRY,  A7 $0 L L32R,  A2 A2 0 8 EXTUI,  A10 A7 A7 OR,  A8 $58 L L32R,
  A8 CALLX8,
  A9 15 MOVI.N,  A11 A10 A9 OR,  A10 $4 L L32R,  A14 A4 0 16 EXTUI,
  A3 A3 0 8 EXTUI,  A5 A5 0 16 EXTUI,  A5 A5 16 SLLI,  MEMW,  A11 A7 0 S32I.N,
  A6 A6 0 16 EXTUI,  A14 A1 4 S32I.N,  A9 A1 0 S32I.N,  A8 $5C L L32R,  A8 CALLX8,
  A4 $8 L L32R,  A4 A10 A4 OR,  A7 $C L L32R,  A10 A7 MOV.N,  A8 $4 L L32R,  MEMW,
  A4 A8 0 S32I.N,  A8 $60 L L32R,  A8 CALLX8,
  A4 $10 L L32R,  A4 A10 A4 OR,  A10 $4 L L32R,  MEMW,  A4 A7 0 S32I.N,
  A8 $64 L L32R,  A8 CALLX8,
  A11 $14 L L32R,  A11 A10 A11 OR,  A4 $18 L L32R,  A10 A4 A4 OR,  A8 $4 L L32R,
  MEMW,  A11 A8 0 S32I,  A8 $68 L L32R,  A8 CALLX8,
  A12 A2 1 SLLI,  A11 3 MOVI.N,  A12 SSL,  A11 A11 SLL,  A13 -1 MOVI.N,
  A11 A13 A11 XOR,  A8 A3 0 2 EXTUI,  A10 A11 A10 AND,  A12 SSL,  A8 A8 SLL,
  A8 A10 A8 OR,  MEMW,  A8 A4 0 S32I.N,  A10 A7 A7 OR,  A8 $6C L L32R,  A8 CALLX8,
  A8 1 MOVI.N,  A2 SSL,  A2 A8 SLL,  A4 $1C L L32R,  A2 A2 0 12 EXTUI,
  A10 A10 A4 AND,  A2 A2 19 SLLI,  A2 A2 A10 OR,  MEMW,  A2 A7 0 S32I.N,
  A10 A7 MOV.N,  A8 $70 L L32R,  A8 CALLX8,
  A4 $20 L L32R,  A4 A10 A4 OR,  A2 $24 L L32R,  A10 A2 A2 OR,  MEMW,  A4 A7 0 S32I,
  A8 $74 L L32R,  A8 CALLX8,
  A4 $28 L L32R,  A4 A10 A4 AND,  A10 A2 MOV.N,  MEMW,  A4 A2 0 S32I.N,
  A8 $78 L L32R,  A8 CALLX8,
  A4 $2C L L32R,  A10 A10 A4 AND,  A4 $300 MOVI,  A4 A10 A4 OR,  MEMW,
  A4 A2 0 S32I.N,  A10 A2 MOV.N,  A8 $7C L L32R,  A8 CALLX8,
  A4 $30 L L32R,  A8 A10 A4 AND,  A10 A2 A2 OR,  A4 $34 L L32R,  A4 A8 A4 OR,  MEMW,
  A4 A2 0 S32I,  A8 $80 L L32R,  A8 CALLX8,
  A4 -241 MOVI,  A10 A10 A4 AND,  A4 128 MOVI,  A10 A10 A4 OR,  MEMW,
  A10 A2 0 S32I.N,  A10 A2 A2 OR,  A8 $84 L L32R,  A8 CALLX8,
  A9 A1 0 L32I.N,  A9 A10 A9 OR,  A10 A2 MOV.N,  MEMW,  A9 A2 0 S32I,
  A8 $88 L L32R,  A8 CALLX8,
  A8 $38 L L32R,  A8 A10 A8 OR,  A4 $3C L L32R,  A10 A4 MOV.N,  MEMW,  A8 A2 0 S32I,
  A8 $8C L L32R,  A8 CALLX8,
  A2 $40 L L32R,  A10 A10 A2 AND,  A14 A1 4 L32I.N,  A3 A10 A14 OR,  A10 A4 MOV.N,
  MEMW,  A3 A4 0 S32I,  A8 $90 L L32R,  A8 CALLX8,
  A10 A10 0 16 EXTUI,  A5 A10 A5 OR,  A3 $44 L L32R,  A10 A3 MOV.N,  MEMW,
  A5 A4 0 S32I,  A8 $94 L L32R,  A8 CALLX8,
  A10 A10 A2 AND,  A6 A10 A6 OR,  A10 A7 A7 OR,  MEMW,  A6 A3 0 S32I,
  A8 $98 L L32R,  A8 CALLX8,
  A2 $48 L L32R,  A2 A10 A2 AND,  A10 A7 MOV.N,  MEMW,  A2 A7 0 S32I.N,
  A8 $9C L L32R,  A8 CALLX8,
  A2 $4C L L32R,  A10 A10 A2 OR,  MEMW,  A10 A7 0 S32I.N,  NOP.N,  NOP.N,  NOP.N,
  NOP.N,  A2 $50 L L32R,  $29D L J,
$295 L:
  A10 1 MOVI.N,  A8 $A0 L L32R,  A8 CALLX8,
$29D L:
  A10 A7 MOV.N,  A8 $A4 L L32R,  A8 CALLX8,
  A10 A2 $295 L BNONE,
  A10 A7 MOV.N,  A8 $A8 L L32R,  A8 CALLX8,
  A2 A10 A10 OR,  A10 A7 A7 OR,  A8 $AC L L32R,  A8 CALLX8,
  A3 $54 L L32R,  A2 A2 0 16 EXTUI,  A10 A10 A3 AND,  MEMW,  A10 A7 0 S32I.N,
  RETW.N,
$2CC L:  END-CODE

CODE hall_sens_read_full   \ .text.hall_sens_read_full $54
  $0 21 LITERALS
    $3FF48858 T,  $4000000 T,  $3FF48478 T,  $80000000 T,  $8000000 T,
    $BFFFFFFF T,  $40000000 T,  $FBFFFFFF T,  $F7FFFFFF T,
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL ets_delay_us  SYMBOL adc1_read_test  SYMBOL adc1_read_test
    SYMBOL esp_dport_access_reg_read  SYMBOL adc1_read_test  SYMBOL adc1_read_test
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
$54 L:
  A1 32 ENTRY,  A4 $0 L L32R,  A10 A4 MOV.N,  A8 $24 L L32R,  A8 CALLX8,
  A2 $4 L L32R,  A2 A10 A2 OR,  A5 $8 L L32R,  A10 A5 MOV.N,  MEMW,  A2 A4 0 S32I,
  A8 $28 L L32R,  A8 CALLX8,
  A2 $C L L32R,  A2 A10 A2 OR,  A10 A4 MOV.N,  MEMW,  A2 A5 0 S32I,  A8 $2C L L32R,
  A8 CALLX8,
  A2 $10 L L32R,  A2 A10 A2 OR,  A10 A5 MOV.N,  MEMW,  A2 A4 0 S32I.N,
  A8 $30 L L32R,  A8 CALLX8,
  A2 $14 L L32R,  A2 A10 A2 AND,  A10 100 MOVI,  MEMW,  A2 A5 0 S32I.N,
  A8 $34 L L32R,  A8 CALLX8,
  A11 0 MOVI.N,  A12 2 MOVI.N,  A10 A11 MOV.N,  A8 $38 L L32R,  A8 CALLX8,
  A3 A10 MOV.N,  A12 2 MOVI.N,  A11 0 MOVI.N,  A10 3 MOVI.N,  A8 $3C L L32R,
  A8 CALLX8,
  A2 A10 MOV.N,  A10 A5 A5 OR,  A8 $40 L L32R,  A8 CALLX8,
  A8 $18 L L32R,  A11 0 MOVI.N,  A8 A10 A8 OR,  A12 2 MOVI.N,  A10 A11 MOV.N,  MEMW,
  A8 A5 0 S32I.N,  A8 $44 L L32R,  A8 CALLX8,
  A12 2 MOVI.N,  A11 0 MOVI.N,  A5 A10 A10 OR,  A10 3 MOVI,  A8 $48 L L32R,
  A8 CALLX8,
  A10 A3 A10 ADD.N,  A10 A10 A5 SUB,  A2 A10 A2 SUB,  A10 A4 MOV.N,  A8 $4C L L32R,
  A8 CALLX8,
  A3 $1C L L32R,  A2 A2 0 16 EXTUI,  A3 A10 A3 AND,  A10 A4 A4 OR,  MEMW,
  A3 A4 0 S32I,  A8 $50 L L32R,  A8 CALLX8,
  A3 $20 L L32R,  A10 A10 A3 AND,  MEMW,  A10 A4 0 S32I.N,  RETW.N,
$135 L:  END-CODE

CODE hall_sens_amp_read_full   \ .text.hall_sens_amp_read_full $4C
  $0 19 LITERALS
    $3FF48858 T,  $4000000 T,  $3FF48478 T,  $80000000 T,  $8000000 T,
    $BFFFFFFF T,  $40000000 T,  $FBFFFFFF T,  $F7FFFFFF T,
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL ets_delay_us  SYMBOL adc1_amp_read_full
    SYMBOL esp_dport_access_reg_read  SYMBOL adc1_amp_read_full
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
$4C L:
  A1 32 ENTRY,  A5 $0 L L32R,  A2 A2 0 16 EXTUI,  A10 A5 MOV.N,  A8 $24 L L32R,
  A8 CALLX8,
  A3 A3 0 16 EXTUI,  A7 $4 L L32R,  A4 A4 0 16 EXTUI,  A7 A10 A7 OR,  A6 $8 L L32R,
  A10 A6 MOV.N,  MEMW,  A7 A5 0 S32I.N,  A8 $28 L L32R,  A8 CALLX8,
  A7 $C L L32R,  A7 A10 A7 OR,  A10 A5 MOV.N,  MEMW,  A7 A6 0 S32I,  A8 $2C L L32R,
  A8 CALLX8,
  A7 $10 L L32R,  A7 A10 A7 OR,  A10 A6 MOV.N,  MEMW,  A7 A5 0 S32I,  A8 $30 L L32R,
  A8 CALLX8,
  A7 $14 L L32R,  A7 A10 A7 AND,  A10 100 MOVI,  MEMW,  A7 A6 0 S32I.N,
  A8 $34 L L32R,  A8 CALLX8,
  A11 0 MOVI.N,  A14 A4 MOV.N,  A13 A3 MOV.N,  A12 A2 MOV.N,  A10 A11 A11 OR,
  A8 $38 L L32R,  A8 CALLX8,
  A7 A10 MOV.N,  A10 A6 A6 OR,  A8 $3C L L32R,  A8 CALLX8,
  A8 $18 L L32R,  A11 0 MOVI.N,  A10 A10 A8 OR,  A12 A2 MOV.N,  A14 A4 MOV.N,
  A13 A3 MOV.N,  MEMW,  A10 A6 0 S32I.N,  A10 A11 A11 OR,  A8 $40 L L32R,
  A8 CALLX8,
  A2 A7 A10 SUB,  A10 A5 MOV.N,  A8 $44 L L32R,  A8 CALLX8,
  A6 $1C L L32R,  A2 A2 0 16 EXTUI,  A6 A10 A6 AND,  A10 A5 A5 OR,  MEMW,
  A6 A5 0 S32I,  A8 $48 L L32R,  A8 CALLX8,
  A6 $20 L L32R,  A10 A10 A6 AND,  MEMW,  A10 A5 0 S32I.N,  RETW.N,
$11D L:  END-CODE

CODE adc2_read_test   \ .text.adc2_read_test $94
  $0 37 LITERALS
    $3FF4882C T,  $3FF48890 T,  $30000 T,  $20000000 T,  $3FF48894 T,  $40000 T,
    $3FF48838 T,  $8007FFFF T,  $3FF4E050 T,  $F7FFFFFF T,  $80000000 T,
    $3FF4883C T,  $E000000 T,  $FFFDFFFF T,  $20000 T,  $10000 T,  $8000000 T,
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
$94 L:
  A1 32 ENTRY,  A4 $0 L L32R,  A5 -225 MOVI,  A10 A4 MOV.N,  A8 $44 L L32R,
  A8 CALLX8,
  A10 A10 A5 AND,  MEMW,  A10 A4 0 S32I.N,  A10 A4 A4 OR,  A8 $48 L L32R,
  A8 CALLX8,
  A6 15 MOVI.N,  A6 A10 A6 OR,  A5 $4 L L32R,  A2 A2 0 8 EXTUI,  A10 A5 MOV.N,
  A3 A3 0 8 EXTUI,  A3 A3 0 2 EXTUI,  MEMW,  A6 A4 0 S32I,  A8 $4C L L32R,
  A8 CALLX8,
  A4 $8 L L32R,  A4 A10 A4 OR,  A10 A5 MOV.N,  MEMW,  A4 A5 0 S32I,  A8 $50 L L32R,
  A8 CALLX8,
  A6 $C L L32R,  A6 A10 A6 OR,  A4 $10 L L32R,  A10 A4 A4 OR,  MEMW,  A6 A5 0 S32I,
  A8 $54 L L32R,  A8 CALLX8,
  A7 $14 L L32R,  A7 A10 A7 OR,  A6 $18 L L32R,  A10 A6 MOV.N,  MEMW,  A7 A4 0 S32I,
  A8 $58 L L32R,  A8 CALLX8,
  A8 A2 1 SLLI,  A7 3 MOVI.N,  A8 SSL,  A7 A7 SLL,  A9 -1 MOVI.N,  A7 A9 A7 XOR,
  A10 A7 A10 AND,  A8 SSL,  A3 A3 SLL,  A3 A10 A3 OR,  MEMW,  A3 A6 0 S32I.N,
  A10 A4 MOV.N,  A3 1 MOVI,  A8 $5C L L32R,  A8 CALLX8,
  A2 SSL,  A2 A3 SLL,  A7 $1C L L32R,  A2 A2 0 12 EXTUI,  A2 A2 19 SLLI,
  A10 A10 A7 AND,  A2 A2 A10 OR,  A3 $20 L L32R,  MEMW,  A2 A4 0 S32I.N,
$161 L:
  A10 A3 MOV.N,  A8 $60 L L32R,  A8 CALLX8,
  A10 A10 24 3 EXTUI,  A10 $161 L BNEZ,
  A10 A3 A3 OR,  A8 $64 L L32R,  A8 CALLX8,
  A2 -2 MOVI,  A10 A10 A2 AND,  MEMW,  A10 A3 0 S32I,  A10 A5 A5 OR,  A8 $68 L L32R,
  A8 CALLX8,
  A2 $24 L L32R,  A2 A10 A2 AND,  A10 A4 MOV.N,  MEMW,  A2 A5 0 S32I.N,
  A8 $6C L L32R,  A8 CALLX8,
  A2 $28 L L32R,  A10 A10 A2 OR,  A6 $2C L L32R,  A2 $30 L L32R,  MEMW,
  A10 A4 0 S32I.N,
$1B1 L:
  A10 A6 MOV.N,  A8 $70 L L32R,  A8 CALLX8,
  A10 A10 A2 AND,  A10 A10 22 10 EXTUI,  A10 $1B1 L BNEZ,
  A10 A4 MOV.N,  A8 $74 L L32R,  A8 CALLX8,
  A6 $34 L L32R,  A2 A10 A6 AND,  A10 A4 MOV.N,  MEMW,  A2 A4 0 S32I.N,
  A8 $78 L L32R,  A8 CALLX8,
  A2 $38 L L32R,  A10 A10 A2 OR,  A2 $3C L L32R,  MEMW,  A10 A4 0 S32I,
$1EC L:
  A10 A4 MOV.N,  A8 $7C L L32R,  A8 CALLX8,
  A10 A2 $1EC L BNONE,
  A10 A4 A4 OR,  A8 $80 L L32R,  A8 CALLX8,
  A2 A10 A10 OR,  A10 A4 A4 OR,  A8 $84 L L32R,  A8 CALLX8,
  A10 A10 A6 AND,  MEMW,  A10 A4 0 S32I.N,  A10 A4 MOV.N,  A8 $88 L L32R,
  A8 CALLX8,
  A7 A10 A7 AND,  A10 A3 A3 OR,  MEMW,  A7 A4 0 S32I,  A8 $8C L L32R,  A8 CALLX8,
  A4 1 MOVI.N,  A4 A10 A4 OR,  A10 A5 MOV.N,  MEMW,  A4 A3 0 S32I.N,  A8 $90 L L32R,
  A8 CALLX8,
  A2 A2 0 16 EXTUI,  A3 $40 L L32R,  A10 A10 A3 OR,  MEMW,  A10 A5 0 S32I.N,
  RETW.N,
$250 L:  END-CODE

CODE adc1_pad_init   \ .text.adc1_pad_init $1C
  $0 7 LITERALS
    SYMBOL CSWTCH$13  SYMBOL rtc_pads_muxsel  SYMBOL rtc_pads_pu
    SYMBOL rtc_pads_pd  SYMBOL rtc_pads_funie  SYMBOL rtc_pads_slpie
    SYMBOL rtc_pads_slpoe
$1C L:
  A1 32 ENTRY,  A2 A2 0 8 EXTUI,  A3 0 MOVI,  A2 8 $30 L BGEUI,
  A3 $0 L L32R,  A2 A2 A3 ADDX4,  A3 A2 0 L32I.N,
$30 L:
  A10 A3 MOV.N,  A11 1 MOVI.N,  A8 $4 L L32R,  A8 CALLX8,
  A10 A3 MOV.N,  A11 0 MOVI.N,  A8 $8 L L32R,  A8 CALLX8,
  A10 A3 MOV.N,  A11 0 MOVI.N,  A8 $C L L32R,  A8 CALLX8,
  A10 A3 MOV.N,  A11 0 MOVI.N,  A8 $10 L L32R,  A8 CALLX8,
  A10 A3 MOV.N,  A11 0 MOVI.N,  A8 $14 L L32R,  A8 CALLX8,
  A11 0 MOVI.N,  A10 A3 MOV.N,  A8 $18 L L32R,  A8 CALLX8,
  RETW.N,
$6E L:  END-CODE

CODE adc1_read   \ .text.adc1_read $8
  $0 2 LITERALS
    SYMBOL adc1_pad_init  SYMBOL adc1_read_test
$8 L:
  A1 32 ENTRY,  A2 A2 0 8 EXTUI,  A10 A2 MOV.N,  A8 $0 L L32R,  A8 CALLX8,
  A11 A3 0 8 EXTUI,  A12 2 MOVI.N,  A10 A2 A2 OR,  A8 $4 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  RETW.N,
$28 L:  END-CODE

CODE adc1_amp_read   \ .text.adc1_amp_read $8
  $0 2 LITERALS
    SYMBOL adc1_pad_init  SYMBOL adc1_amp_read_full
$8 L:
  A1 32 ENTRY,  A10 0 MOVI,  A8 $0 L L32R,  A8 CALLX8,
  A11 0 MOVI.N,  A14 4 MOVI.N,  A13 1 MOVI.N,  A12 $400 MOVI,  A10 A11 MOV.N,
  A8 $4 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  RETW.N,
$29 L:  END-CODE

CODE hall_sens_read   \ .text.hall_sens_read $C
  $0 3 LITERALS
    SYMBOL adc1_pad_init  SYMBOL adc1_pad_init  SYMBOL hall_sens_read_full
$C L:
  A1 32 ENTRY,  A10 0 MOVI,  A8 $0 L L32R,  A8 CALLX8,
  A10 3 MOVI.N,  A8 $4 L L32R,  A8 CALLX8,
  A8 $8 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  RETW.N,
$2A L:  END-CODE

CODE hall_sens_amp_read   \ .text.hall_sens_amp_read $8
  $0 2 LITERALS
    SYMBOL adc1_pad_init  SYMBOL hall_sens_amp_read_full
$8 L:
  A1 32 ENTRY,  A10 0 MOVI,  A8 $0 L L32R,  A8 CALLX8,
  A12 100 MOVI,  A11 1 MOVI.N,  A10 $3E8 MOVI,  A8 $4 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  RETW.N,
$26 L:  END-CODE

CODE adc2_pad_init   \ .text.adc2_pad_init $1C
  $0 7 LITERALS
    SYMBOL CSWTCH$19  SYMBOL rtc_pads_muxsel  SYMBOL rtc_pads_pu
    SYMBOL rtc_pads_pd  SYMBOL rtc_pads_funie  SYMBOL rtc_pads_slpie
    SYMBOL rtc_pads_slpoe
$1C L:
  A1 32 ENTRY,  A2 A2 0 8 EXTUI,  A8 9 MOVI.N,  A3 0 MOVI.N,  A8 A2 $32 L BLTU,
  A3 $0 L L32R,  A2 A2 A3 ADDX4,  A3 A2 0 L32I,
$32 L:
  A10 A3 MOV.N,  A11 1 MOVI.N,  A8 $4 L L32R,  A8 CALLX8,
  A10 A3 MOV.N,  A11 0 MOVI.N,  A8 $8 L L32R,  A8 CALLX8,
  A10 A3 MOV.N,  A11 0 MOVI.N,  A8 $C L L32R,  A8 CALLX8,
  A10 A3 MOV.N,  A11 0 MOVI.N,  A8 $10 L L32R,  A8 CALLX8,
  A10 A3 MOV.N,  A11 0 MOVI.N,  A8 $14 L L32R,  A8 CALLX8,
  A11 0 MOVI.N,  A10 A3 MOV.N,  A8 $18 L L32R,  A8 CALLX8,
  RETW.N,
$70 L:  END-CODE

CODE adc2_read   \ .text.adc2_read $8
  $0 2 LITERALS
    SYMBOL adc2_pad_init  SYMBOL adc2_read_test
$8 L:
  A1 32 ENTRY,  A2 A2 0 8 EXTUI,  A10 A2 MOV.N,  A8 $0 L L32R,  A8 CALLX8,
  A3 A3 0 8 EXTUI,  A11 A3 MOV.N,  A10 A2 A2 OR,  A8 $4 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  RETW.N,
$28 L:  END-CODE

CODE adc_pad_int   \ .text.adc_pad_int $30
  $0 12 LITERALS
    $800 T,  $1000 T,  $2000 T,  $4000 T,  $8000 T,  $10000 T,  $20000 T,
    SYMBOL rtc_pads_pu  SYMBOL rtc_pads_pd  SYMBOL rtc_pads_funie
    SYMBOL rtc_pads_slpie  SYMBOL rtc_pads_slpoe
$30 L:
  A1 32 ENTRY,  A2 A2 0 8 EXTUI,  A5 0 MOVI.N,  A2 A5 $78 L BBC,
  A9 2 MOVI.N,  A11 A3 A9 AND,  A8 4 MOVI.N,  A5 A3 0 1 EXTUI,  A10 8 MOVI.N,
  A5 A9 A11 MOVNEZ,  A11 A3 A8 AND,  A5 A8 A11 MOVNEZ,  A9 16 MOVI.N,
  A11 A3 A10 AND,  A5 A10 A11 MOVNEZ,  A8 32 MOVI.N,  A11 A3 A9 AND,  A10 $200 MOVI,
  A5 A10 A11 MOVNEZ,  A11 A3 A8 AND,  A10 $100 MOVI,  A5 A10 A11 MOVNEZ,
  A10 A3 6 1 EXTUI,  A5 A9 A10 MOVNEZ,  A3 A3 7 1 EXTUI,  A5 A8 A3 MOVNEZ,
$78 L:
  A2 1 $D4 L BBCI,
  A3 A4 0 1 EXTUI,  A2 $400 MOVI,  A5 A2 A3 MOVNEZ,  A2 $0 L L32R,  A3 A4 1 1 EXTUI,
  A5 A2 A3 MOVNEZ,  A3 A4 2 1 EXTUI,  A8 A4 3 1 EXTUI,  A2 $4 L L32R,
  A9 A4 4 1 EXTUI,  A5 A2 A3 MOVNEZ,  A12 A4 5 1 EXTUI,  A3 64 MOVI.N,
  A2 $8 L L32R,  A11 A4 A3 AND,  A5 A2 A8 MOVNEZ,  A2 128 MOVI,  A10 A4 A2 AND,
  A8 $C L L32R,  A5 A8 A9 MOVNEZ,  A8 A4 8 1 EXTUI,  A4 A4 9 1 EXTUI,
  A9 $10 L L32R,  A5 A9 A12 MOVNEZ,  A9 $14 L L32R,  A5 A9 A11 MOVNEZ,
  A9 $18 L L32R,  A5 A9 A10 MOVNEZ,  A5 A3 A8 MOVNEZ,  A5 A2 A4 MOVNEZ,
$D4 L:
  A10 A5 MOV.N,  A11 0 MOVI.N,  A8 $1C L L32R,  A8 CALLX8,
  A10 A5 MOV.N,  A11 0 MOVI.N,  A8 $20 L L32R,  A8 CALLX8,
  A10 A5 MOV.N,  A11 0 MOVI.N,  A8 $24 L L32R,  A8 CALLX8,
  A10 A5 MOV.N,  A11 0 MOVI.N,  A8 $28 L L32R,  A8 CALLX8,
  A11 0 MOVI.N,  A10 A5 MOV.N,  A8 $2C L L32R,  A8 CALLX8,
  RETW.N,
$108 L:  END-CODE

CODE adc_pad_init   \ .text.adc_pad_init $84
  $0 33 LITERALS
    $3FF4847C T,  $8000000 T,  $FFF7FFFF T,  $4000000 T,  $FFFFBFFF T,
    $2000000 T,  $1000000 T,  $3FF4848C T,  $20000 T,  $FF3FFFDF T,  $40000 T,
    $E7FFF7FF T,  $3FF48480 T,  $20000000 T,  $FF7FFFFF T,  $10000000 T,
    $FFFBFFFF T,  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read
$84 L:
  A1 32 ENTRY,  A2 0 $B4 L BBCI,
  A3 $0 L L32R,  A10 A3 MOV.N,  A8 $44 L L32R,  A8 CALLX8,
  A8 $4 L L32R,  A8 A10 A8 OR,  A10 A3 MOV.N,  MEMW,  A8 A3 0 S32I,  A8 $48 L L32R,
  A8 CALLX8,
  A8 $8 L L32R,  A10 A10 A8 AND,  MEMW,  A10 A3 0 S32I.N,
$B4 L:
  A2 1 $EC L BBCI,
  NOP.N,  NOP.N,  NOP,  NOP,  A3 $0 L L32R,  A10 A3 A3 OR,  A8 $4C L L32R,
  A8 CALLX8,
  A8 $C L L32R,  A8 A10 A8 OR,  A10 A3 MOV.N,  MEMW,  A8 A3 0 S32I,  A8 $50 L L32R,
  A8 CALLX8,
  A8 $10 L L32R,  A10 A10 A8 AND,  MEMW,  A10 A3 0 S32I.N,
$EC L:
  A2 2 $124 L BBCI,
  NOP.N,  NOP.N,  NOP,  NOP,  A3 $0 L L32R,  A10 A3 A3 OR,  A8 $54 L L32R,
  A8 CALLX8,
  A8 $14 L L32R,  A8 A10 A8 OR,  A10 A3 MOV.N,  MEMW,  A8 A3 0 S32I,  A8 $58 L L32R,
  A8 CALLX8,
  A8 -513 MOVI,  A10 A10 A8 AND,  MEMW,  A10 A3 0 S32I.N,
$124 L:
  A2 3 $15C L BBCI,
  NOP.N,  NOP.N,  NOP,  NOP,  A3 $0 L L32R,  A10 A3 A3 OR,  A8 $5C L L32R,
  A8 CALLX8,
  A8 $18 L L32R,  A8 A10 A8 OR,  A10 A3 A3 OR,  MEMW,  A8 A3 0 S32I,  A8 $60 L L32R,
  A8 CALLX8,
  A8 -17 MOVI.N,  A10 A10 A8 AND,  MEMW,  A10 A3 0 S32I.N,
$15C L:
  A2 4 $194 L BBCI,
  NOP.N,  NOP.N,  NOP,  NOP,  A3 $1C L L32R,  A10 A3 A3 OR,  A8 $64 L L32R,
  A8 CALLX8,
  A8 $20 L L32R,  A8 A10 A8 OR,  A10 A3 MOV.N,  MEMW,  A8 A3 0 S32I,  A8 $68 L L32R,
  A8 CALLX8,
  A8 $24 L L32R,  A10 A10 A8 AND,  MEMW,  A10 A3 0 S32I.N,
$194 L:
  A2 5 $1CC L BBCI,
  NOP.N,  NOP.N,  NOP,  NOP,  A3 $1C L L32R,  A10 A3 A3 OR,  A8 $6C L L32R,
  A8 CALLX8,
  A8 $28 L L32R,  A8 A10 A8 OR,  A10 A3 MOV.N,  MEMW,  A8 A3 0 S32I,  A8 $70 L L32R,
  A8 CALLX8,
  A8 $2C L L32R,  A10 A10 A8 AND,  MEMW,  A10 A3 0 S32I.N,
$1CC L:
  A2 6 $204 L BBCI,
  NOP.N,  NOP.N,  NOP,  NOP,  A3 $30 L L32R,  A10 A3 A3 OR,  A8 $74 L L32R,
  A8 CALLX8,
  A8 $34 L L32R,  A8 A10 A8 OR,  A10 A3 MOV.N,  MEMW,  A8 A3 0 S32I,  A8 $78 L L32R,
  A8 CALLX8,
  A8 $38 L L32R,  A10 A10 A8 AND,  MEMW,  A10 A3 0 S32I.N,
$204 L:
  A2 7 $23C L BBCI,
  NOP.N,  NOP.N,  NOP,  NOP,  A2 $30 L L32R,  A10 A2 A2 OR,  A8 $7C L L32R,
  A8 CALLX8,
  A8 $3C L L32R,  A8 A10 A8 OR,  A10 A2 MOV.N,  MEMW,  A8 A2 0 S32I,  A8 $80 L L32R,
  A8 CALLX8,
  A8 $40 L L32R,  A10 A10 A8 AND,  MEMW,  A10 A2 0 S32I.N,
$23C L:
  RETW.N,
$23E L:  END-CODE

CODE dac_pad_init   \ .text.dac_pad_init $44
  $0 17 LITERALS
    $3FF4889C T,  $FCFFFFFF T,  $3FF48898 T,  $10000 T,  $3FF48484 T,  $20000 T,
    $E7FFFFFF T,  $F807FFFF T,  $3FF48488 T,  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read
$44 L:
  A1 32 ENTRY,  A4 $0 L L32R,  A2 A2 0 8 EXTUI,  A10 A4 MOV.N,  A8 $24 L L32R,
  A8 CALLX8,
  A3 A3 0 8 EXTUI,  A8 $4 L L32R,  A8 A10 A8 AND,  A5 $8 L L32R,  A10 A5 A5 OR,
  MEMW,  A8 A4 0 S32I,  A8 $28 L L32R,  A8 CALLX8,
  A8 $C L L32R,  A10 A10 A8 OR,  MEMW,  A10 A5 0 S32I.N,  A2 0 $C9 L BBCI,
  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A4 $10 L L32R,  A10 A4 MOV.N,  A8 $2C L L32R,
  A8 CALLX8,
  A5 $14 L L32R,  A5 A10 A5 OR,  A10 A4 MOV.N,  MEMW,  A5 A4 0 S32I,  A8 $30 L L32R,
  A8 CALLX8,
  A5 $18 L L32R,  A5 A10 A5 AND,  A10 A4 MOV.N,  MEMW,  A5 A4 0 S32I.N,
  A8 $34 L L32R,  A8 CALLX8,
  A5 A3 19 SLLI,  A8 $1C L L32R,  A10 A10 A8 AND,  A5 A10 A5 OR,  MEMW,
  A5 A4 0 S32I.N,
$C9 L:
  A2 1 $116 L BBCI,
  A3 A3 19 SLLI,  NOP.N,  NOP.N,  NOP.N,  A2 $20 L L32R,  A10 A2 A2 OR,
  A8 $38 L L32R,  A8 CALLX8,
  A4 $14 L L32R,  A4 A10 A4 OR,  A10 A2 MOV.N,  MEMW,  A4 A2 0 S32I,  A8 $3C L L32R,
  A8 CALLX8,
  A4 $18 L L32R,  A4 A10 A4 AND,  A10 A2 MOV.N,  MEMW,  A4 A2 0 S32I.N,
  A8 $40 L L32R,  A8 CALLX8,
  A4 $1C L L32R,  A10 A10 A4 AND,  A3 A10 A3 OR,  MEMW,  A3 A2 0 S32I.N,
$116 L:
  RETW.N,
$118 L:  END-CODE

LINK-SYMBOLS   \ the cells of other packages that wait for these names
END-PACKAGE  PREVIOUS   \ not in the search order: ALSO name gives its words (README.md)
