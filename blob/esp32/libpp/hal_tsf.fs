\ hal_tsf.fs - package PP-HAL-TSF: hal_tsf.o of libpp.a (ESP32)
\
\ Made from hal_tsf.S by s2fs.py, not by hand. Each function of C is a
\ CODE word: its bytes are those of the compiler, at the same offsets of
\ its section (L gives the address of an offset); its literals are at the
\ start of the word, after a jump. The data come first, then the
\ functions, each after those that it calls. The global symbols are
\ PUBLIC, the static ones PRIVATE. lib/esp32/clink.fs explains the words.
\
\ 5 functions (5 global), 1 data objects.
\ A function runs as C code with the windowed calls of the Xtensa: call
\ it with CALL-C ( x1 .. xn n xt -- x ), never by its name alone.
\
\ Names of other packages: a literal waits until its package comes
\ (LINK-SYMBOLS); a name outside the libraries until EXTERN gives it.
\
\ Needs XTENSA (asm/xtensa.fs), ASM (lib/esp32/asm.fs) and CLINK
\ (lib/esp32/clink.fs) of ucf; the ESP32.

PACKAGE PP-HAL-TSF  ALSO ASM  ALSO CLINK   \ lib/package.fs
EXPORTS   \ the PUBLIC words are symbols for the other packages

\ ==== Data =============================================================

8 DATA-ALIGN
8 DATA: s_tbttstart   \ .bss.s_tbttstart

\ ==== Code =============================================================

CODE hal_enable_sta_tsf   \ .text.hal_enable_sta_tsf $4
  $0 1 LITERALS
    $3FF75020 T,
$4 L:
  A1 32 ENTRY,  A10 $0 L L32R,  A9 -13 MOVI.N,  A9 A9 27 SLLI,  MEMW,
  A8 A10 0 L32I.N,  A8 A8 A9 OR,  MEMW,  A8 A10 0 S32I.N,  MEMW,  RETW.N,
$21 L:  END-CODE

CODE hal_disable_sta_tsf   \ .text.hal_disable_sta_tsf $8
  $0 2 LITERALS
    $3FF75020 T,  $67FFFFFF T,
$8 L:
  A1 32 ENTRY,  A9 $0 L L32R,  A10 $4 L L32R,  MEMW,  A8 A9 0 L32I.N,
  A8 A8 A10 AND,  MEMW,  A8 A9 0 S32I.N,  MEMW,  RETW.N,
$23 L:  END-CODE

CODE hal_enable_nan_tsf   \ .text.hal_enable_nan_tsf $4
  $0 1 LITERALS
    $3FF75098 T,
$4 L:
  A1 32 ENTRY,  A10 $0 L L32R,  A9 -13 MOVI.N,  A9 A9 27 SLLI,  MEMW,
  A8 A10 0 L32I.N,  A8 A8 A9 OR,  MEMW,  A8 A10 0 S32I.N,  MEMW,  RETW.N,
$21 L:  END-CODE

CODE hal_disable_nan_tsf   \ .text.hal_disable_nan_tsf $8
  $0 2 LITERALS
    $3FF75098 T,  $67FFFFFF T,
$8 L:
  A1 32 ENTRY,  A9 $0 L L32R,  A10 $4 L L32R,  MEMW,  A8 A9 0 L32I.N,
  A8 A8 A10 AND,  MEMW,  A8 A9 0 S32I.N,  MEMW,  RETW.N,
$23 L:  END-CODE

CODE hal_disable_softap_tsf   \ .text.hal_disable_softap_tsf $8
  $0 2 LITERALS
    $3FF7505C T,  $3DFFFFFF T,
$8 L:
  A1 32 ENTRY,  A9 $0 L L32R,  A10 $4 L L32R,  MEMW,  A8 A9 0 L32I.N,
  A8 A8 A10 AND,  MEMW,  A8 A9 0 S32I.N,  MEMW,  RETW.N,
$23 L:  END-CODE

LINK-SYMBOLS   \ the cells of other packages that wait for these names
END-PACKAGE  PREVIOUS   \ not in the search order: ALSO name gives its words (README.md)
