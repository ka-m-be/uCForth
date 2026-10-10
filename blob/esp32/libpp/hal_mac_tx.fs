\ hal_mac_tx.fs - package PP-HAL-MAC-TX: hal_mac_tx.o of libpp.a (ESP32)
\
\ Made from hal_mac_tx.S by s2fs.py, not by hand. Each function of C is a
\ CODE word: its bytes are those of the compiler, at the same offsets of
\ its section (L gives the address of an offset); its literals are at the
\ start of the word, after a jump. The data come first, then the
\ functions, each after those that it calls. The global symbols are
\ PUBLIC, the static ones PRIVATE. lib/esp32/clink.fs explains the words.
\
\ 9 functions (9 global), 2 data objects.
\ A function runs as C code with the windowed calls of the Xtensa: call
\ it with CALL-C ( x1 .. xn n xt -- x ), never by its name alone.
\
\ Names of other packages: a literal waits until its package comes
\ (LINK-SYMBOLS); a name outside the libraries until EXTERN gives it.
\ Outside the libraries (../README.md):
\   wifi_log
\
\ Needs XTENSA (asm/xtensa.fs), ASM (lib/esp32/asm.fs) and CLINK
\ (lib/esp32/clink.fs) of ucf; the ESP32.

PACKAGE PP-HAL-MAC-TX  ALSO ASM  ALSO CLINK   \ lib/package.fs
EXPORTS   \ the PUBLIC words are symbols for the other packages

\ ==== Data =============================================================

PRIVATE
CREATE rodata_wlog_warning.3   \ .rodata_wlog_warning.3, 9 bytes
  ," ipencil"  10 C,  0 C,
CREATE rodata_wlog_warning.2   \ .rodata_wlog_warning.2, 47 bytes
  ," <antenna>invalid antenna:%d, eb:%p, tx_desc:%p"  0 C,

\ ==== Code =============================================================

PUBLIC
CODE mac_tx_set_plcp1   \ .text.mac_tx_set_plcp1 $4
  $0 1 LITERALS
    $3FF74258 T,
$4 L:
  A1 32 ENTRY,  A12 A2 0 L32I.N,  A10 15 MOVI.N,  A8 1 MOVI.N,  A9 A12 44 L32I.N,
  A13 A9 12 L8UI,  A11 A13 -16 ADDI,  A11 A11 0 8 EXTUI,  A14 A9 0 L32I.N,
  A10 A11 $20 L BGEU,
  A8 0 MOVI,
$20 L:
  A8 A8 9 SLLI,  A15 31 MOVI.N,  A10 A9 16 L8UI,  A10 A10 A10 ADD.N,  A8 A8 A10 OR,
  A8 A8 16 SLLI,  A10 A13 A15 AND,  A15 A13 $39 L BGEU,
  A10 A11 A15 AND,
$39 L:
  A10 A10 12 SLLI,  A8 A8 A10 OR,  A14 14 $50 L BBCI,
  A9 A9 8 L32I.N,  A9 15 $50 L BBCI,
  A9 1 MOVI,  A9 A9 28 SLLI,  A8 A8 A9 OR,
$50 L:
  A10 A12 20 L16UI,  A9 A12 22 L16UI,  A9 A9 A10 ADD.N,  A10 -1 MOVI.N,
  A10 A10 12 SLLI,  A9 A9 0 12 EXTUI,  A10 A8 A10 AND,  A8 A10 A9 OR,  A9 A2 4 L8UI,
  A2 A9 4 SLLI,  A9 A9 A2 SUB,  A2 $0 L L32R,  A9 A9 A2 ADDX4,  A2 0 MOVI.N,  MEMW,
  A8 A9 0 S32I.N,  MEMW,  RETW.N,
$81 L:  END-CODE

CODE mac_tx_set_plcp2   \ .text.mac_tx_set_plcp2 $14
  $0 5 LITERALS
    SYMBOL rodata_wlog_warning.2  SYMBOL rodata_wlog_warning.3  $3FF7425C T,
    SYMBOL wifi_log  SYMBOL wifi_log
$14 L:
  A1 48 ENTRY,  A15 A2 0 L32I.N,  A9 7 MOVI.N,  A8 15 MOVI.N,  A9 A9 13 SLLI,
  A8 A8 12 SLLI,  A11 A15 44 L32I.N,  A14 A11 16 L32I.N,  A10 A14 A9 AND,
  A8 A14 A8 AND,  A10 $5C L BEQZ.N,
  A13 $0 L L32R,  A14 A14 12 4 EXTUI,  A12 2 MOVI.N,  A10 6 MOVI.N,
  A11 A1 0 S32I.N,  A11 64 MOVI.N,  MEMW,  A8 $C L L32R,  A8 CALLX8,
  A11 1 MOVI.N,  A12 2 MOVI.N,  A13 $4 L L32R,  A11 A11 13 SLLI,  A10 6 MOVI.N,
  A8 $10 L L32R,  A8 CALLX8,
  $85 L J,
  1 ZEROS,
$5C L:
  A9 A8 -4096 ADDMI,  A8 1 MOVI.N,  A8 A10 A9 MOVNEZ,  A8 A8 4 SLLI,  A9 32 MOVI.N,
  A8 A8 A9 OR,  A9 48 MOVI.N,  A8 A8 A9 AND,  A9 A2 4 L8UI,  A2 A9 4 SLLI,
  A9 A9 A2 SUB,  A2 $8 L L32R,  A9 A9 A2 ADDX4,  MEMW,  A8 A9 0 S32I.N,
$85 L:
  A2 0 MOVI.N,  MEMW,  RETW.N,
$8C L:  END-CODE

CODE mac_tx_set_htsig   \ .text.mac_tx_set_htsig $14
  $0 5 LITERALS
    $FF0000FF T,  $FFFFFF T,  $3FF74260 T,  $FFF3FFFF T,  $3FF74264 T,
$14 L:
  A1 48 ENTRY,  A8 A2 0 L32I.N,  A4 15 MOVI.N,  A11 A8 44 L32I.N,  A13 A11 12 L8UI,
  A9 A13 -16 ADDI,  A9 A9 0 8 EXTUI,  A4 A9 $2C L BGEU,
  $132 L J,
$2C L:
  A9 A8 36 L32I.N,  A10 A11 16 L32I,  A10 A10 20 4 EXTUI,  A4 A11 0 L32I,
  A14 A4 22 1 EXTUI,  A12 A8 22 L16UI,  A15 A8 20 L16UI,  A8 0 MOVI.N,
  A9 A8 $54 L BEQ,
  A9 A9 130 L8UI,  A9 A9 -4 ADDI,  A9 A9 0 8 EXTUI,  A9 2 $54 L BGEUI,
  A8 128 MOVI,
$54 L:
  A4 14 $62 L BBCI,
  A4 A11 8 L32I.N,  A4 15 $62 L BBCI,
  A4 128 MOVI,  A8 A8 A4 OR,
$62 L:
  A11 -128 MOVI,  A9 A13 0 3 EXTUI,  A11 A8 A11 AND,  A12 A12 A15 ADD.N,
  A8 A11 A9 OR,  A9 A12 0 16 EXTUI,  A11 A9 A9 OR,  A14 $84 L BEQZ.N,
  A11 A10 A10 ADDX2,  A11 A11 4 SLLI,  A11 A3 A11 ADD,  A11 A11 68 L16UI,
$84 L:
  A11 A11 8 SLLI,  A4 7 MOVI.N,  A15 $0 L L32R,  A8 A8 A15 AND,  A8 A8 A11 OR,
  A11 A14 3 SLLI,  A11 A11 A4 OR,  A4 23 MOVI.N,  A15 1 MOVI.N,  A4 A13 $A1 L BLTU,
  A15 0 MOVI.N,
$A1 L:
  A13 A8 24 8 EXTUI,  A15 A15 7 SLLI,  A4 112 MOVI,  A13 A13 A4 AND,
  A11 A11 A15 OR,  A11 A11 A13 OR,  A13 $4 L L32R,  A11 A11 24 SLLI,  A8 A8 A13 AND,
  A8 A8 A11 OR,  A13 A2 4 L8UI,  A2 A13 4 SLLI,  A13 A13 A2 SUB,  A11 A13 2 SLLI,
  A2 $8 L L32R,  A13 A13 A2 ADDX4,  A2 0 MOVI.N,  MEMW,  A8 A13 0 S32I.N,
  A2 A1 0 S16I,  MEMW,  A14 $120 L BEQZ.N,
  A10 A10 A10 ADDX2,  A10 A10 4 SLLI,  A3 A3 A10 ADD.N,  A8 A3 65 L8UI,
  A2 12 MOVI.N,  A8 A8 2 SLLI,  A8 A8 A2 AND,  A2 A3 64 L8UI,  A2 A2 0 2 EXTUI,
  A8 A8 A2 OR,  A8 A8 0 4 EXTUI,  A2 A3 68 L32I,  A8 A8 16 SLLI,  A8 A9 A8 OR,
  A2 A12 $128 L BNE,
  A2 $C L L32R,  A8 A8 A2 AND,  A2 1 MOVI.N,  A2 A2 18 SLLI,  A8 A8 A2 OR,  MEMW,
  $128 L J,
  1 ZEROS,
$120 L:
  A2 5 MOVI.N,  A2 A2 16 SLLI,  A8 A9 A2 OR,
$128 L:
  A2 $10 L L32R,  A11 A11 A2 ADD.N,  MEMW,  A8 A11 0 S32I.N,
$132 L:
  A2 0 MOVI.N,  MEMW,  RETW.N,
$139 L:  END-CODE

CODE hal_mac_tx_is_cbw40   \ .wifi0iram.4 $4
  $0 1 LITERALS
    $3FF74270 T,
$4 L:
  A1 32 ENTRY,  A2 A2 0 8 EXTUI,  A8 A2 4 SLLI,  A2 A2 A8 SUB,  A8 $0 L L32R,
  A2 A2 A8 ADDX4,  MEMW,  A2 A2 0 L32I.N,  A2 A2 25 1 EXTUI,  RETW.N,
$20 L:  END-CODE

CODE hal_mac_get_txq_state   \ .text.hal_mac_get_txq_state $8
  $0 2 LITERALS
    $3FF73CC0 T,  $3FF73CC8 T,
$8 L:
  A1 32 ENTRY,  A2 1 $24 L BEQI,
  A2 2 $35 L BEQI,
  A8 0 MOVI.N,  A2 A8 $40 L BNE,
  A2 $0 L L32R,  MEMW,  A8 A2 0 L32I.N,  A8 A8 0 11 EXTUI,  $40 L J,
$24 L:
  A2 $0 L L32R,  MEMW,  A8 A2 0 L32I.N,  A8 A8 16 11 EXTUI,  $40 L J,
  3 ZEROS,
$35 L:
  A2 $4 L L32R,  MEMW,  A8 A2 0 L32I.N,  A8 A8 0 4 EXTUI,
$40 L:
  A2 A8 MOV.N,  RETW.N,
$44 L:  END-CODE

CODE hal_mac_clr_txq_state   \ .text.hal_mac_clr_txq_state $C
  $0 3 LITERALS
    $3FF73CBC T,  $F800FFFF T,  $3FF73CC4 T,
$C L:
  A1 32 ENTRY,  A2 1 $2C L BEQI,
  A2 2 $54 L BEQI,
  A2 $64 L BNEZ,
  A2 $0 L L32R,  A9 -2048 MOVI,  A3 A3 0 11 EXTUI,  MEMW,  A8 A2 0 L32I.N,
  A8 A8 A9 AND,  $5C L J,
$2C L:
  A2 $0 L L32R,  A9 $7FF MOVI,  A9 A9 16 SLLI,  A3 A3 16 SLLI,  A3 A3 A9 AND,  MEMW,
  A8 A2 0 L32I.N,  A9 $4 L L32R,  A8 A8 A9 AND,  A3 A3 A8 OR,  MEMW,
  A3 A2 0 S32I.N,  MEMW,  $64 L J,
$54 L:
  A2 $8 L L32R,  MEMW,  A8 A2 0 L32I.N,
$5C L:
  A8 A8 A3 OR,  MEMW,  A8 A2 0 S32I.N,
$64 L:
  A2 0 MOVI.N,  MEMW,  RETW.N,
$6B L:  END-CODE

CODE hal_mac_get_txq_pmd   \ .text.hal_mac_get_txq_pmd $4
  $0 1 LITERALS
    $3FF74270 T,
$4 L:
  A1 48 ENTRY,  A2 A2 0 8 EXTUI,  A8 A2 4 SLLI,  A2 A2 A8 SUB,  A8 $0 L L32R,
  A2 A2 A8 ADDX4,  MEMW,  A8 A2 0 L32I.N,  A2 A8 24 8 EXTUI,  A10 A8 8 8 EXTUI,
  A9 A8 16 8 EXTUI,  A2 A3 3 S8I,  A2 127 MOVI,  A8 A3 0 S8I,  A10 A3 1 S8I,
  A9 A3 2 S8I,  MEMW,  A8 24 $3C L BBCI,
  A2 A9 A9 OR,
$3C L:
  A2 A3 2 S8I,  MEMW,  A2 0 MOVI.N,  MEMW,  RETW.N,
$49 L:  END-CODE

CODE hal_attenna_init   \ .text.hal_attenna_init $8
  $0 2 LITERALS
    $3FF7425C T,  $3FF73284 T,
$8 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A11 -8 MOVI.N,  A9 8 MOVI.N,  A9 $25 L LOOP,
  A8 A8 -60 ADDI,  MEMW,  A10 A8 60 L32I.N,  A10 A10 A11 AND,  MEMW,
  A10 A8 60 S32I.N,
$25 L:
  A13 -9 MOVI.N,  A11 32 MOVI.N,  A12 -17 MOVI.N,  A8 $0 L L32R,  A9 8 MOVI,
  A9 $5E L LOOP,
  A8 A8 -60 ADDI,  MEMW,  A10 A8 60 L32I.N,  A10 A10 A13 AND,  MEMW,
  A10 A8 60 S32I.N,  MEMW,  A10 A8 60 L32I.N,  A10 A10 A11 OR,  MEMW,
  A10 A8 60 S32I.N,  MEMW,  A10 A8 60 L32I.N,  A10 A10 A12 AND,  MEMW,
  A10 A8 60 S32I.N,
$5E L:
  A10 -8 MOVI.N,  A8 $4 L L32R,  MEMW,  A9 A8 0 L32I.N,  A9 A9 A10 AND,  MEMW,
  A9 A8 0 S32I.N,  MEMW,  A9 A8 0 L32I.N,  A9 A9 A11 OR,  MEMW,  A9 A8 0 S32I.N,
  MEMW,  RETW.N,
$82 L:  END-CODE

CODE hal_mac_rate_autoack_init   \ .text.hal_mac_rate_autoack_init $34
  $0 13 LITERALS
    $3FF73404 T,  $3FF73400 T,  $3FF7340C T,  $3020100 T,  $3FF73408 T,
    $7060500 T,  $3FF73414 T,  $B0B0B0B T,  $3FF73410 T,  $3FF7341C T,
    $90B0B0B T,  $3FF73418 T,  $9090909 T,
$34 L:
  A1 32 ENTRY,  A9 $0 L L32R,  A8 0 MOVI.N,  MEMW,  A8 A9 0 S32I.N,  A9 $4 L L32R,
  MEMW,  A8 A9 0 S32I.N,  A8 $8 L L32R,  A9 $C L L32R,  MEMW,  A9 A8 0 S32I.N,
  A8 $10 L L32R,  A9 $14 L L32R,  MEMW,  A9 A8 0 S32I.N,  A8 $1C L L32R,
  A9 $18 L L32R,  MEMW,  A8 A9 0 S32I.N,  A9 $20 L L32R,  MEMW,  A8 A9 0 S32I.N,
  A8 $24 L L32R,  A9 $28 L L32R,  MEMW,  A9 A8 0 S32I.N,  A8 $2C L L32R,
  A9 $30 L L32R,  MEMW,  A9 A8 0 S32I.N,  MEMW,  RETW.N,
$8D L:  END-CODE

LINK-SYMBOLS   \ the cells of other packages that wait for these names
END-PACKAGE  PREVIOUS   \ not in the search order: ALSO name gives its words (README.md)
