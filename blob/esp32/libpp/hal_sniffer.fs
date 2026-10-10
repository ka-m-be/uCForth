\ hal_sniffer.fs - package PP-HAL-SNIFFER: hal_sniffer.o of libpp.a (ESP32)
\
\ Made from hal_sniffer.S by s2fs.py, not by hand. Each function of C is a
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
\
\ Needs XTENSA (asm/xtensa.fs), ASM (lib/esp32/asm.fs) and CLINK
\ (lib/esp32/clink.fs) of ucf; the ESP32.

PACKAGE PP-HAL-SNIFFER  ALSO ASM  ALSO CLINK   \ lib/package.fs
EXPORTS   \ the PUBLIC words are symbols for the other packages

\ ==== Code =============================================================

CODE hal_sniffer_enable   \ .text.hal_sniffer_enable $4
  $0 1 LITERALS
    $3FF730E4 T,
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A10 1 MOVI.N,  A10 A10 15 SLLI,  MEMW,
  A9 A8 0 L32I.N,  A9 A9 A10 OR,  A10 -2 MOVI.N,  MEMW,  A9 A8 0 S32I.N,  MEMW,
  A9 A8 0 L32I.N,  A9 A9 A10 AND,  A10 -3 MOVI.N,  MEMW,  A9 A8 0 S32I.N,  MEMW,
  A9 A8 0 L32I.N,  A9 A9 A10 AND,  A10 -5 MOVI.N,  MEMW,  A9 A8 0 S32I.N,  MEMW,
  A9 A8 0 L32I.N,  A9 A9 A10 AND,  A10 -9 MOVI.N,  MEMW,  A9 A8 0 S32I.N,  MEMW,
  A9 A8 0 L32I.N,  A9 A9 A10 AND,  A10 32 MOVI.N,  MEMW,  A9 A8 0 S32I.N,  MEMW,
  A9 A8 0 L32I.N,  A9 A9 A10 OR,  A10 64 MOVI.N,  MEMW,  A9 A8 0 S32I.N,  MEMW,
  A9 A8 0 L32I.N,  A9 A9 A10 OR,  A10 128 MOVI,  MEMW,  A9 A8 0 S32I.N,  MEMW,
  A9 A8 0 L32I.N,  A9 A9 A10 OR,  MEMW,  A9 A8 0 S32I.N,  MEMW,  RETW.N,
$8B L:  END-CODE

CODE hal_sniffer_disable   \ .text.hal_sniffer_disable $8
  $0 2 LITERALS
    $3FF730E4 T,  $FFFF7FFF T,
$8 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A10 $4 L L32R,  MEMW,  A9 A8 0 L32I.N,
  A9 A9 A10 AND,  A10 1 MOVI.N,  MEMW,  A9 A8 0 S32I.N,  MEMW,  A9 A8 0 L32I.N,
  A9 A9 A10 OR,  A10 2 MOVI.N,  MEMW,  A9 A8 0 S32I.N,  MEMW,  A9 A8 0 L32I.N,
  A9 A9 A10 OR,  A10 4 MOVI.N,  MEMW,  A9 A8 0 S32I.N,  MEMW,  A9 A8 0 L32I.N,
  A9 A9 A10 OR,  A10 8 MOVI.N,  MEMW,  A9 A8 0 S32I.N,  MEMW,  A9 A8 0 L32I.N,
  A9 A9 A10 OR,  A10 -33 MOVI,  MEMW,  A9 A8 0 S32I.N,  MEMW,  A9 A8 0 L32I.N,
  A9 A9 A10 AND,  A10 -65 MOVI,  MEMW,  A9 A8 0 S32I.N,  MEMW,  A9 A8 0 L32I.N,
  A9 A9 A10 AND,  A10 -129 MOVI,  MEMW,  A9 A8 0 S32I.N,  MEMW,  A9 A8 0 L32I.N,
  A9 A9 A10 AND,  MEMW,  A9 A8 0 S32I.N,  MEMW,  RETW.N,
$8F L:  END-CODE

CODE hal_sniffer_rx_set_promis   \ .text.hal_sniffer_rx_set_promis $10
  $0 4 LITERALS
    $3FF73C40 T,  $3FF73104 T,  $3FF730F8 T,  $3FF730FC T,
$10 L:
  A1 32 ENTRY,  A3 A3 0 8 EXTUI,  A2 $20 L BEQZ.N,
  A2 1 $35 L BEQI,
  $C1 L J,
  2 ZEROS,
$20 L:
  A8 $0 L L32R,  A4 -5 MOVI.N,  MEMW,  A9 A8 0 L32I.N,  A4 A9 A4 AND,
  A3 $BC L BEQZ,
  A4 4 MOVI.N,  $B9 L J,
$35 L:
  A8 $4 L L32R,  MEMW,  A9 A8 0 L32I.N,  A9 A9 0 16 EXTUI,  MEMW,  A9 A8 0 S32I.N,
  A3 $7D L BEQZ.N,
  MEMW,  A9 A8 0 L32I.N,  A9 A9 A4 OR,  MEMW,  A9 A8 0 S32I.N,  A9 $8 L L32R,  MEMW,
  A8 A9 0 L32I.N,  A8 A8 A4 OR,  MEMW,  A8 A9 0 S32I.N,  A9 $C L L32R,  MEMW,
  A8 A9 0 L32I.N,  A8 A8 A4 OR,  MEMW,  A8 A9 0 S32I.N,  MEMW,  $C1 L J,
  3 ZEROS,
$7D L:
  A9 $8 L L32R,  A8 -1 MOVI.N,  A4 A8 A4 XOR,  MEMW,  A10 A9 0 L32I.N,
  A10 A10 A4 AND,  A8 $C L L32R,  MEMW,  A10 A9 0 S32I.N,  MEMW,  A2 A8 0 L32I.N,
  A4 A4 A2 AND,  MEMW,  A4 A8 0 S32I.N,  A4 5 MOVI.N,  A4 A4 24 SLLI,  MEMW,
  A10 A9 0 L32I.N,  A10 A10 A4 OR,  MEMW,  A10 A9 0 S32I.N,  MEMW,  A9 A8 0 L32I.N,
$B9 L:
  A4 A9 A4 OR,
$BC L:
  MEMW,  A4 A8 0 S32I.N,
$C1 L:
  MEMW,  RETW.N,
$C6 L:  END-CODE

CODE hal_sniffer_rx_clr_statistics   \ .text.hal_sniffer_rx_clr_statistics $8
  $0 2 LITERALS
    $3FF73D7C T,  $7FFFFFFF T,
$8 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A10 -1 MOVI.N,  A10 A10 31 SLLI,  MEMW,
  A9 A8 0 L32I.N,  A9 A9 A10 OR,  MEMW,  A9 A8 0 S32I.N,  MEMW,  A9 A8 0 L32I.N,
  A10 $4 L L32R,  A9 A9 A10 AND,  MEMW,  A9 A8 0 S32I.N,  MEMW,  RETW.N,
$35 L:  END-CODE

LINK-SYMBOLS   \ the cells of other packages that wait for these names
END-PACKAGE  PREVIOUS   \ not in the search order: ALSO name gives its words (README.md)
