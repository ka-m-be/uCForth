\ hal_mac_rx.fs - package PP-HAL-MAC-RX: hal_mac_rx.o of libpp.a (ESP32)
\
\ Made from hal_mac_rx.S by s2fs.py, not by hand. Each function of C is a
\ CODE word: its bytes are those of the compiler, at the same offsets of
\ its section (L gives the address of an offset); its literals are at the
\ start of the word, after a jump. The data come first, then the
\ functions, each after those that it calls. The global symbols are
\ PUBLIC, the static ones PRIVATE. lib/esp32/clink.fs explains the words.
\
\ 5 functions (5 global), 0 data objects.
\ A function runs as C code with the windowed calls of the Xtensa: call
\ it with CALL-C ( x1 .. xn n xt -- x ), never by its name alone.
\
\ Names of other packages: a literal waits until its package comes
\ (LINK-SYMBOLS); a name outside the libraries until EXTERN gives it.
\
\ Needs XTENSA (asm/xtensa.fs), ASM (lib/esp32/asm.fs) and CLINK
\ (lib/esp32/clink.fs) of ucf; the ESP32.

PACKAGE PP-HAL-MAC-RX  ALSO ASM  ALSO CLINK   \ lib/package.fs
EXPORTS   \ the PUBLIC words are symbols for the other packages

\ ==== Code =============================================================

CODE hal_mac_rx_get_end_state   \ .iram1.2 $4
  $0 1 LITERALS
    $3FF730A4 T,
$4 L:
  A1 32 ENTRY,  A2 $0 L L32R,  MEMW,  A2 A2 0 L32I.N,  A2 A2 0 8 EXTUI,  RETW.N,
$14 L:  END-CODE

CODE hal_mac_rx_get_last_dscr   \ .wifislprxiram.3 $4
  $0 1 LITERALS
    $3FF73090 T,
$4 L:
  A1 32 ENTRY,  A2 $0 L L32R,  MEMW,  A2 A2 0 L32I.N,  RETW.N,
$11 L:  END-CODE

CODE hal_mac_rx_is_dscr_reload   \ .wifislprxiram.4 $4
  $0 1 LITERALS
    $3FF73084 T,
$4 L:
  A1 32 ENTRY,  A2 $0 L L32R,  MEMW,  A2 A2 0 L32I.N,  A2 A2 0 1 EXTUI,  RETW.N,
$14 L:  END-CODE

CODE hal_mac_rx_set_dscr_reload   \ .wifislprxiram.5 $4
  $0 1 LITERALS
    $3FF73084 T,
$4 L:
  A1 32 ENTRY,  A9 $0 L L32R,  A10 1 MOVI.N,  MEMW,  A8 A9 0 L32I.N,  A8 A8 A10 OR,
  MEMW,  A8 A9 0 S32I.N,  MEMW,  RETW.N,
$1E L:  END-CODE

CODE hal_mac_rx_get_end_info   \ .iram1.6 $18
  $0 6 LITERALS
    $3FF73D84 T,  $3FF73D88 T,  $3FF730A4 T,  $3FF732F8 T,  $3FF732F4 T,
    $3FF732F0 T,
$18 L:
  A1 32 ENTRY,  A8 $0 L L32R,  MEMW,  A8 A8 0 L32I.N,  A8 A2 0 S32I.N,
  A8 $4 L L32R,  MEMW,  A8 A8 0 L32I.N,  A8 A3 0 S32I.N,  A8 $8 L L32R,  MEMW,
  A8 A8 0 L32I.N,  A8 A8 0 8 EXTUI,  A8 A4 0 S32I.N,  A8 $C L L32R,  MEMW,
  A8 A8 0 L32I.N,  A8 A5 0 S32I.N,  A8 $10 L L32R,  MEMW,  A8 A8 0 L32I.N,
  A8 A6 0 S32I.N,  A8 $14 L L32R,  MEMW,  A8 A8 0 L32I.N,  A8 A7 0 S32I.N,  MEMW,
  RETW.N,
$5F L:  END-CODE

LINK-SYMBOLS   \ the cells of other packages that wait for these names
END-PACKAGE  PREVIOUS   \ not in the search order: ALSO name gives its words (README.md)
