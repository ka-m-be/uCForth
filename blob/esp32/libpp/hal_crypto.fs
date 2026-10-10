\ hal_crypto.fs - package PP-HAL-CRYPTO: hal_crypto.o of libpp.a (ESP32)
\
\ Made from hal_crypto.S by s2fs.py, not by hand. Each function of C is a
\ CODE word: its bytes are those of the compiler, at the same offsets of
\ its section (L gives the address of an offset); its literals are at the
\ start of the word, after a jump. The data come first, then the
\ functions, each after those that it calls. The global symbols are
\ PUBLIC, the static ones PRIVATE. lib/esp32/clink.fs explains the words.
\
\ 9 functions (9 global), 3 data objects.
\ A function runs as C code with the windowed calls of the Xtensa: call
\ it with CALL-C ( x1 .. xn n xt -- x ), never by its name alone.
\
\ Names of other packages: a literal waits until its package comes
\ (LINK-SYMBOLS); a name outside the libraries until EXTERN gives it.
\ Outside the libraries (../README.md):
\   free g_osi_funcs_p memcpy memset wifi_log
\
\ Needs XTENSA (asm/xtensa.fs), ASM (lib/esp32/asm.fs) and CLINK
\ (lib/esp32/clink.fs) of ucf; the ESP32.

PACKAGE PP-HAL-CRYPTO  ALSO ASM  ALSO CLINK   \ lib/package.fs
EXPORTS   \ the PUBLIC words are symbols for the other packages

\ ==== Data =============================================================

PRIVATE
CREATE rodata_wlog_warning.4   \ .rodata_wlog_warning.4, 9 bytes
  ," ipencil"  10 C,  0 C,
CREATE rodata_wlog_warning.3   \ .rodata_wlog_warning.3, 9 bytes
  ," ipencil"  10 C,  0 C,
CREATE rodata_wlog_warning.2   \ .rodata_wlog_warning.2, 9 bytes
  ," ipencil"  10 C,  0 C,

\ ==== Code =============================================================

PUBLIC
CODE hal_crypto_clr_key_entry   \ .text.hal_crypto_clr_key_entry $2C
  $0 11 LITERALS
    $3FF73814 T,  $3FF74400 T,  $3FF74404 T,  $3FF74408 T,  $3FF7440C T,
    $3FF74410 T,  $3FF74414 T,  $3FF74418 T,  $3FF7441C T,  $3FF74420 T,
    $3FF74424 T,
$2C L:
  A1 32 ENTRY,  A9 $0 L L32R,  A8 -2 MOVI.N,  A2 SSL,  A8 A8 A8 SRC,
  A2 A2 A2 ADDX4,  MEMW,  A10 A9 0 L32I.N,  A8 A8 A10 AND,  MEMW,  A8 A9 0 S32I.N,
  A8 0 MOVI.N,  A9 $4 L L32R,  A9 A2 A9 ADDX8,  MEMW,  A8 A9 0 S32I.N,
  A9 $8 L L32R,  A9 A2 A9 ADDX8,  MEMW,  A8 A9 0 S32I.N,  A9 $C L L32R,
  A9 A2 A9 ADDX8,  MEMW,  A8 A9 0 S32I.N,  A9 $10 L L32R,  A9 A2 A9 ADDX8,  MEMW,
  A8 A9 0 S32I.N,  A9 $14 L L32R,  A9 A2 A9 ADDX8,  MEMW,  A8 A9 0 S32I.N,
  A9 $18 L L32R,  A9 A2 A9 ADDX8,  MEMW,  A8 A9 0 S32I.N,  A9 $1C L L32R,
  A9 A2 A9 ADDX8,  MEMW,  A8 A9 0 S32I.N,  A9 $20 L L32R,  A9 A2 A9 ADDX8,  MEMW,
  A8 A9 0 S32I.N,  A9 $24 L L32R,  A9 A2 A9 ADDX8,  MEMW,  A8 A9 0 S32I.N,
  A9 $28 L L32R,  A2 A2 A9 ADDX8,  MEMW,  A8 A2 0 S32I.N,  MEMW,  RETW.N,
$BF L:  END-CODE

CODE hal_crypto_get_using_key_idx   \ .text.hal_crypto_get_using_key_idx $4
  $0 1 LITERALS
    $3FF7442C T,
$4 L:
  A1 32 ENTRY,  A2 $0 L L32R,  MEMW,  A2 A2 0 L32I.N,  A2 A2 30 2 EXTUI,  RETW.N,
$14 L:  END-CODE

CODE hal_crypto_set_key_entry   \ .text.hal_crypto_set_key_entry $34
  $0 13 LITERALS
    SYMBOL g_osi_funcs_p  SYMBOL rodata_wlog_warning.2
    SYMBOL rodata_wlog_warning.3  $3C1F T,  $3FF74400 T,  $3FF74404 T,
    $3FF73814 T,  $3FF74408 T,  SYMBOL memset  SYMBOL memcpy  SYMBOL wifi_log
    SYMBOL memcpy  SYMBOL free
$34 L:
  A1 48 ENTRY,  A7 -4 MOVI.N,  A6 A4 3 ADDI.N,  A6 A6 A7 AND,  A7 A3 0 2 EXTUI,
  A7 $76 L BEQZ.N,
  A7 $0 L L32R,  A10 A6 MOV.N,  A7 A7 0 L32I.N,  A7 A7 $16C L32I,  A7 CALLX8,
  A7 A10 MOV.N,  A10 $70 L BEQZ.N,
  A12 A6 MOV.N,  A11 0 MOVI.N,  A8 $20 L L32R,  A8 CALLX8,
  A12 A4 MOV.N,  A11 A3 A3 OR,  A10 A7 A7 OR,  A8 $24 L L32R,  A8 CALLX8,
  $78 L J,
  1 ZEROS,
$70 L:
  A13 $4 L L32R,  $80 L J,
$76 L:
  A7 A3 MOV.N,
$78 L:
  A4 A7 0 2 EXTUI,  A4 $90 L BEQZ.N,
  A13 $8 L L32R,
$80 L:
  A11 1 MOVI.N,  A12 2 MOVI.N,  A11 A11 13 SLLI,  A10 6 MOVI,  A8 $28 L L32R,
  A8 CALLX8,
$90 L:
  A4 0 MOVI.N,  A4 A1 0 S16I,  MEMW,  A11 A5 6 L8UI,  A11 A11 24 SLLI,
  A8 A5 5 L8UI,  A8 A8 16 SLLI,  A11 A11 A8 ADD.N,  A8 A5 4 L8UI,  A8 A8 8 SLLI,
  A11 A11 A8 ADD.N,  A9 A5 8 L8UI,  A9 A9 8 SLLI,  A8 A5 7 L8UI,  A8 A8 A9 ADD.N,
  A8 A8 0 16 EXTUI,  A4 A5 3 L8UI,  A11 A11 A4 ADD.N,  A9 A5 1 L8UI,
  A9 5 $D0 L BNEI,
  A9 A9 16 SLLI,  MEMW,  $D6 L J,
$D0 L:
  A9 A9 0 3 EXTUI,  A9 A9 18 SLLI,
$D6 L:
  A8 A8 A9 OR,  A9 7 MOVI.N,  A9 A9 18 SLLI,  A12 1 MOVI.N,  A9 A8 A9 AND,
  A12 A12 18 SLLI,  A10 7 MOVI,  A9 A12 $F4 L BEQ,
  A10 3 MOVI.N,  A2 4 $F4 L BGEI,
  A10 6 MOVI,
$F4 L:
  A9 A5 0 L8UI,  A10 A10 5 SLLI,  A9 A9 0 2 EXTUI,  A9 A9 8 SLLI,  A9 A9 A10 OR,
  A5 A5 2 L8UI,  A12 A6 MOV.N,  A5 A5 14 SLLI,  A9 A9 A5 OR,  A5 A8 16 16 EXTUI,
  A4 $C L L32R,  A8 A8 0 16 EXTUI,  A5 A5 A4 AND,  A9 A9 A5 OR,  A9 A9 16 SLLI,
  A5 $10 L L32R,  A8 A8 A9 OR,  A9 A2 A2 ADDX4,  A5 A9 A5 ADDX8,  MEMW,
  A11 A5 0 S32I.N,  A11 A7 MOV.N,  A5 $14 L L32R,  A5 A9 A5 ADDX8,  MEMW,
  A8 A5 0 S32I.N,  A5 1 MOVI.N,  A2 SSL,  A5 A5 SLL,  A8 $18 L L32R,  MEMW,
  A4 A8 0 L32I.N,  A5 A5 A4 OR,  A10 $1C L L32R,  A10 A9 A10 ADDX8,  MEMW,
  A5 A8 0 S32I.N,  MEMW,  A8 $2C L L32R,  A8 CALLX8,
  A7 A3 $170 L BEQ,
  A10 A7 MOV.N,  A8 $30 L L32R,  A8 CALLX8,
$170 L:
  RETW.N,
$172 L:  END-CODE

CODE hal_crypto_is_key_valid   \ .text.hal_crypto_is_key_valid $4
  $0 1 LITERALS
    $3FF73814 T,
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  MEMW,  A9 A8 0 L32I.N,  A8 1 MOVI.N,  A2 SSL,
  A2 A8 SLL,  A9 A2 A9 AND,  A2 0 MOVI.N,  A2 A8 A9 MOVNEZ,  RETW.N,
$21 L:  END-CODE

CODE hal_crypto_get_key_entry   \ .text.hal_crypto_get_key_entry $18
  $0 6 LITERALS
    SYMBOL rodata_wlog_warning.4  $3FF74400 T,  $3FF74404 T,  $3FF74408 T,
    SYMBOL wifi_log  SYMBOL memcpy
$18 L:
  A1 48 ENTRY,  A8 A3 0 2 EXTUI,  A8 $34 L BEQZ.N,
  A11 1 MOVI.N,  A13 $0 L L32R,  A12 2 MOVI,  A11 A11 13 SLLI,  A10 6 MOVI,
  A8 $10 L L32R,  A8 CALLX8,
$34 L:
  A8 $4 L L32R,  A2 A2 A2 ADDX4,  A8 A2 A8 ADDX8,  A10 A2 3 SLLI,  A9 A8 0 L32I.N,
  A14 A9 8 8 EXTUI,  A13 A9 16 8 EXTUI,  A12 A9 24 8 EXTUI,  A8 $8 L L32R,
  A2 A2 A8 ADDX8,  A2 A2 0 L32I.N,  A8 A2 24 2 EXTUI,  A11 A2 8 8 EXTUI,
  A8 A5 0 S8I,  A8 A2 30 2 EXTUI,  A8 A5 2 S8I,  A8 31 MOVI.N,  A8 A8 16 SLLI,
  A8 A2 A8 AND,  A9 A1 4 S32I.N,  A9 A5 3 S8I,  A9 5 MOVI.N,  A9 A9 16 SLLI,
  A11 A5 8 S8I,  A11 5 MOVI.N,  A2 A1 0 S32I.N,  A14 A5 4 S8I,  A13 A5 5 S8I,
  A12 A5 6 S8I,  A2 A5 7 S8I,  MEMW,  A8 A9 $90 L BEQ,
  A11 A2 18 3 EXTUI,
$90 L:
  A4 A4 3 ADDI.N,  A12 -4 MOVI.N,  A11 A5 1 S8I,  MEMW,  A12 A4 A12 AND,
  A11 $C L L32R,  A11 A10 A11 ADD.N,  A10 A3 MOV.N,  MEMW,  A8 $14 L L32R,
  A8 CALLX8,
  RETW.N,
$AF L:  END-CODE

CODE hal_crypto_init   \ .text.hal_crypto_init $18
  $0 6 LITERALS
    $3FF73800 T,  $30000 T,  $3FF73804 T,  $3FF73808 T,  $3FF7380C T,
    $3FF73810 T,
$18 L:
  A1 32 ENTRY,  A8 $4 L L32R,  A9 $0 L L32R,  MEMW,  A8 A9 0 S32I.N,  A9 $8 L L32R,
  MEMW,  A8 A9 0 S32I.N,  A8 0 MOVI.N,  A9 $C L L32R,  MEMW,  A8 A9 0 S32I.N,
  A9 $10 L L32R,  MEMW,  A8 A9 0 S32I.N,  A9 $14 L L32R,  MEMW,  A8 A9 0 S32I.N,
  MEMW,  RETW.N,
$4D L:  END-CODE

CODE hal_crypto_mgmt_rx_enabled   \ .text.hal_crypto_mgmt_rx_enabled $8
  $0 2 LITERALS
    $3FF73804 T,  $3FF73800 T,
$8 L:
  A1 32 ENTRY,  A2 A2 0 8 EXTUI,  A8 $4 L L32R,  A2 1 $17 L BNEI,
  A8 $0 L L32R,
$17 L:
  MEMW,  A2 A8 0 L32I.N,  A2 A2 17 1 EXTUI,  RETW.N,
$21 L:  END-CODE

CODE hal_crypto_enable   \ .text.hal_crypto_enable $20
  $0 8 LITERALS
    $3FF73804 T,  $3FF73800 T,  $10103 T,  $10010103 T,  $3FF73810 T,  $FFFF00 T,
    $3FFFFFFF T,  $FF0000FF T,
$20 L:
  A1 32 ENTRY,  A2 A2 0 8 EXTUI,  A8 $4 L L32R,  A3 A3 0 8 EXTUI,  A4 A4 0 8 EXTUI,
  A5 A5 0 8 EXTUI,  A2 1 $38 L BNEI,
  A8 $0 L L32R,
$38 L:
  A10 -5 MOVI.N,  A10 A3 A10 AND,  A9 $C L L32R,  A10 1 $46 L BEQI,
  A9 $8 L L32R,
$46 L:
  A4 $50 L BEQZ.N,
  A10 1 MOVI.N,  A10 A10 17 SLLI,  A9 A9 A10 OR,
$50 L:
  A5 $58 L BEQZ.N,
  A10 $200 MOVI,  A9 A9 A10 OR,
$58 L:
  MEMW,  A9 A8 0 S32I.N,  A9 $10 L L32R,  MEMW,  A10 A9 0 L32I.N,  A3 4 $8C L BNEI,
  A3 $14 L L32R,  A10 A10 A3 OR,  MEMW,  A10 A9 0 S32I.N,  A10 -1 MOVI.N,
  A10 A10 31 SLLI,  MEMW,  A9 A8 0 L32I.N,  A3 $18 L L32R,  A9 A9 A3 AND,
  A9 A9 A10 OR,  MEMW,  $A2 L J,
$8C L:
  A3 $1C L L32R,  A10 A10 A3 AND,  MEMW,  A10 A9 0 S32I.N,  MEMW,  A9 A8 0 L32I.N,
  A3 $18 L L32R,  A9 A9 A3 AND,
$A2 L:
  MEMW,  A9 A8 0 S32I.N,  MEMW,  RETW.N,
$AC L:  END-CODE

CODE hal_crypto_disable   \ .text.hal_crypto_disable $10
  $0 4 LITERALS
    $3FF73800 T,  $30000 T,  $3FF73804 T,  $3FF73814 T,
$10 L:
  A1 32 ENTRY,  A2 A2 0 8 EXTUI,  A2 $20 L BNEZ.N,
  A2 $0 L L32R,  $26 L J,
  2 ZEROS,
$20 L:
  A2 1 $2E L BNEI,
  A2 $8 L L32R,
$26 L:
  A8 $4 L L32R,  MEMW,  A8 A2 0 S32I.N,
$2E L:
  A8 -1 MOVI.N,  A8 A8 A3 XOR,  A9 $C L L32R,  MEMW,  A2 A9 0 L32I.N,  A8 A8 A2 AND,
  MEMW,  A8 A9 0 S32I.N,  MEMW,  RETW.N,
$48 L:  END-CODE

LINK-SYMBOLS   \ the cells of other packages that wait for these names
END-PACKAGE  PREVIOUS   \ not in the search order: ALSO name gives its words (README.md)
