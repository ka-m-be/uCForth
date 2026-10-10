\ hal_ampdu.fs - package PP-HAL-AMPDU: hal_ampdu.o of libpp.a (ESP32)
\
\ Made from hal_ampdu.S by s2fs.py, not by hand. Each function of C is a
\ CODE word: its bytes are those of the compiler, at the same offsets of
\ its section (L gives the address of an offset); its literals are at the
\ start of the word, after a jump. The data come first, then the
\ functions, each after those that it calls. The global symbols are
\ PUBLIC, the static ones PRIVATE. lib/esp32/clink.fs explains the words.
\
\ 6 functions (6 global), 3 data objects.
\ A function runs as C code with the windowed calls of the Xtensa: call
\ it with CALL-C ( x1 .. xn n xt -- x ), never by its name alone.
\
\ Names of other packages: a literal waits until its package comes
\ (LINK-SYMBOLS); a name outside the libraries until EXTERN gives it.
\ Outside the libraries (../README.md):
\   wifi_log
\ C names with more than 31 characters (Forth takes 31): the first 26,
\ ~ and 4 hex digits of their CRC-32 (../README.md):
\   hal_ba_session_restore_by_~73A5 = hal_ba_session_restore_by_hw_index
\
\ Needs XTENSA (asm/xtensa.fs), ASM (lib/esp32/asm.fs) and CLINK
\ (lib/esp32/clink.fs) of ucf; the ESP32.

PACKAGE PP-HAL-AMPDU  ALSO ASM  ALSO CLINK   \ lib/package.fs
EXPORTS   \ the PUBLIC words are symbols for the other packages

\ ==== Data =============================================================

PRIVATE
CREATE rodata_wlog_info.4   \ .rodata_wlog_info.4, 23 bytes
  ," <ba-del>idx:%d, tid:%d"  0 C,
CREATE rodata_wlog_info.3   \ .rodata_wlog_info.3, 43 bytes
  ," <ba-clr>idx:%d, tid:%d, ssn:%d, winSize:%d"  0 C,
CREATE rodata_wlog_info.2   \ .rodata_wlog_info.2, 83 bytes
  ," <ba-add>idx:%d (ifx:%d, %02x:%02x:%02x:%02x:%02x:%02x), tid:"
  ," %d, ssn:%d, winSize:%d"  0 C,

\ ==== Code =============================================================

PUBLIC
CODE hal_agreement_add_rx_ba   \ .text.hal_agreement_add_rx_ba $28
  $0 10 LITERALS
    SYMBOL rodata_wlog_info.2  $3FF73258 T,  $3FF73254 T,  $3FF7325C T,
    $3FF73250 T,  $7FFFFFFF T,  $1FFB993 T,  $3FF73264 T,  $C0000005 T,
    SYMBOL wifi_log
$28 L:
  A1 80 ENTRY,  A5 A5 0 8 EXTUI,  A6 A6 0 16 EXTUI,  A7 A7 0 8 EXTUI,
  A7 A1 32 S32I.N,  A2 A2 0 8 EXTUI,  A3 A3 0 8 EXTUI,  A11 1 MOVI.N,  A15 A3 MOV.N,
  A6 A1 28 S32I.N,  A14 A2 MOV.N,  A10 5 MOVI.N,  A12 3 MOVI.N,  A11 A11 13 SLLI,
  A5 A1 24 S32I.N,  A3 A3 16 SLLI,  A7 A7 13 SLLI,  A6 A6 0 12 EXTUI,  A8 A4 5 L8UI,
  A8 A1 20 S32I.N,  A8 A4 4 L8UI,  A8 A1 16 S32I.N,  A8 A4 3 L8UI,  A8 A1 12 S32I.N,
  A8 A4 2 L8UI,  A8 A1 8 S32I.N,  A8 A4 1 L8UI,  A8 A1 4 S32I.N,  A8 A4 0 L8UI,
  A13 $0 L L32R,  A8 A1 0 S32I.N,  MEMW,  A8 $24 L L32R,  A8 CALLX8,
  A9 A4 1 L8UI,  A9 A9 8 SLLI,  A8 A4 0 L8UI,  A9 A9 A8 OR,  A8 A4 2 L8UI,
  A8 A8 16 SLLI,  A8 A8 A9 OR,  A9 A4 3 L8UI,  A9 A9 24 SLLI,  A9 A9 A8 OR,
  A8 A2 5 SLLI,  A10 $4 L L32R,  A10 A10 A8 SUB,  MEMW,  A9 A10 0 S32I.N,
  A9 3 MOVI.N,  A9 A9 16 SLLI,  A3 A3 A9 AND,  A9 A4 4 L8UI,  A4 A4 5 L8UI,
  A4 A4 8 SLLI,  A4 A4 A9 OR,  A3 A3 A4 OR,  A4 $8 L L32R,  A4 A4 A8 SUB,  MEMW,
  A3 A4 0 S32I.N,  A3 127 MOVI,  A3 A3 13 SLLI,  A7 A7 A3 AND,  A7 A7 A6 OR,
  A3 $C L L32R,  A3 A3 A8 SUB,  MEMW,  A7 A3 0 S32I.N,  A3 $10 L L32R,
  A3 A3 A8 SUB,  MEMW,  A4 A3 0 L32I.N,  A6 $14 L L32R,  A4 A4 A6 AND,  A6 0 MOVI.N,
  MEMW,  A4 A3 0 S32I.N,  A4 $18 L L32R,  A4 A4 A2 SUB,  A4 A4 5 SLLI,
  A2 $1C L L32R,  A2 A2 A8 SUB,  MEMW,  A6 A4 0 S32I.N,  A4 A5 12 SLLI,
  A4 A4 0 16 EXTUI,  MEMW,  A6 A2 0 S32I.N,  A2 $20 L L32R,  A4 A4 A2 OR,  MEMW,
  A4 A3 0 S32I.N,  MEMW,  RETW.N,
$12C L:  END-CODE

CODE hal_agreement_clr_rx_ba   \ .text.hal_agreement_clr_rx_ba $1C
  $0 7 LITERALS
    SYMBOL rodata_wlog_info.3  $3FF73250 T,  $3FF7325C T,  $1FFB993 T,
    $3FF73264 T,  $C0000005 T,  SYMBOL wifi_log
$1C L:
  A1 48 ENTRY,  A2 A2 0 8 EXTUI,  A3 A3 0 8 EXTUI,  A4 A4 0 16 EXTUI,
  A5 A5 0 8 EXTUI,  A13 $0 L L32R,  A11 1 MOVI.N,  A15 A3 MOV.N,  A14 A2 MOV.N,
  A11 A11 13 SLLI,  A5 A1 4 S32I.N,  A10 5 MOVI.N,  A12 3 MOVI,  A5 A5 13 SLLI,
  A3 A3 12 SLLI,  A4 A1 0 S32I,  MEMW,  A8 $18 L L32R,  A8 CALLX8,
  A11 127 MOVI,  A11 A11 13 SLLI,  A4 A4 0 12 EXTUI,  A8 $4 L L32R,  A5 A5 A11 AND,
  A10 A2 5 SLLI,  A5 A5 A4 OR,  A9 0 MOVI.N,  A4 $8 L L32R,  A8 A8 A10 SUB,
  A4 A4 A10 SUB,  A3 A3 0 16 EXTUI,  MEMW,  A9 A8 0 S32I.N,  MEMW,  A5 A4 0 S32I.N,
  A5 $C L L32R,  A5 A5 A2 SUB,  A5 A5 5 SLLI,  A2 $10 L L32R,  A2 A2 A10 SUB,  MEMW,
  A9 A5 0 S32I.N,  MEMW,  A9 A2 0 S32I.N,  A2 $14 L L32R,  A3 A3 A2 OR,  MEMW,
  A3 A8 0 S32I.N,  MEMW,  RETW.N,
$A6 L:  END-CODE

CODE hal_agreement_del_rx_ba   \ .text.hal_agreement_del_rx_ba $18
  $0 6 LITERALS
    $3FF73250 T,  SYMBOL rodata_wlog_info.4  $7FFFFFFF T,  $1FFB993 T,
    $3FF73264 T,  SYMBOL wifi_log
$18 L:
  A1 32 ENTRY,  A4 A2 0 8 EXTUI,  A3 $0 L L32R,  A2 A4 5 SLLI,  A3 A3 A2 SUB,
  A11 1 MOVI.N,  A14 A4 MOV.N,  MEMW,  A15 A3 0 L32I.N,  A12 3 MOVI.N,
  A15 A15 12 4 EXTUI,  A11 A11 13 SLLI,  A10 5 MOVI,  A13 $4 L L32R,  A8 $14 L L32R,
  A8 CALLX8,
  MEMW,  A8 A3 0 L32I.N,  A9 $8 L L32R,  A8 A8 A9 AND,  A14 $C L L32R,
  A14 A14 A4 SUB,  A14 A14 5 SLLI,  A9 $10 L L32R,  A2 A9 A2 SUB,  A9 -1 MOVI.N,
  A9 A9 31 SLLI,  MEMW,  A8 A3 0 S32I.N,  A8 0 MOVI.N,  MEMW,  A8 A14 0 S32I.N,
  MEMW,  A8 A2 0 S32I.N,  MEMW,  A2 A3 0 L32I.N,  A2 A2 A9 OR,  MEMW,
  A2 A3 0 S32I.N,  MEMW,  A8 A3 0 S32I.N,  MEMW,  RETW.N,
$8B L:  END-CODE

CODE hal_ba_session_store   \ .text.hal_ba_session_store $4
  $0 1 LITERALS
    $3FF73250 T,
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A9 0 MOVI.N,  A10 8 MOVI.N,  A10 $3D L LOOP,
  MEMW,  A11 A8 0 L32I.N,  A11 $38 L BGEZ,
  A12 A2 A9 ADD.N,  MEMW,  A11 A8 12 L32I.N,  A11 A11 20 12 EXTUI,
  A11 A12 0 S32I.N,  A11 A3 A9 ADD.N,  MEMW,  A12 A8 24 L32I.N,  A12 A11 0 S32I.N,
  A11 A4 A9 ADD.N,  MEMW,  A12 A8 28 L32I.N,  A12 A11 0 S32I,
$38 L:
  A8 A8 -32 ADDI,  A9 A9 4 ADDI.N,
$3D L:
  MEMW,  RETW.N,
$42 L:  END-CODE

CODE hal_ba_session_restore   \ .text.hal_ba_session_restore $8
  $0 2 LITERALS
    $3FF73250 T,  $7FFFFFFF T,
$8 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A9 0 MOVI.N,  A11 8 MOVI.N,  A13 $4 L L32R,
  A11 $72 L LOOP,
  MEMW,  A10 A8 0 L32I.N,  A10 $6D L BGEZ,
  A14 -1 MOVI.N,  A14 A14 12 SLLI,  MEMW,  A10 A8 0 L32I.N,  A10 A10 A13 AND,  MEMW,
  A10 A8 0 S32I.N,  A10 A2 A9 ADD.N,  MEMW,  A12 A8 12 L32I.N,  A12 A12 A14 AND,
  A10 A10 0 L32I.N,  A10 A10 0 12 EXTUI,  A10 A10 A12 OR,  A12 -1 MOVI.N,  MEMW,
  A10 A8 12 S32I.N,  A10 A3 A9 ADD.N,  A12 A12 31 SLLI,  A10 A10 0 L32I.N,  MEMW,
  A10 A8 16 S32I.N,  A10 A4 A9 ADD.N,  A10 A10 0 L32I.N,  MEMW,  A10 A8 20 S32I.N,
  MEMW,  A10 A8 0 L32I.N,  A10 A10 A12 OR,  MEMW,  A10 A8 0 S32I.N,
$6D L:
  A8 A8 -32 ADDI,  A9 A9 4 ADDI.N,
$72 L:
  MEMW,  RETW.N,
$77 L:  END-CODE

CODE hal_ba_session_restore_by_~73A5   \ hal_ba_session_restore_by_hw_index: .text.hal_ba_session_restore_by_hw_index $14
  $0 5 LITERALS
    $3FF73250 T,  $7FFFFFFF T,  $1FFB993 T,  $3FF73264 T,  $3FF7325C T,
$14 L:
  A1 32 ENTRY,  A11 A2 0 8 EXTUI,  A8 $0 L L32R,  A10 A11 5 SLLI,  A8 A8 A10 SUB,
  A2 A11 MOV.N,  MEMW,  A9 A8 0 L32I.N,  A9 $95 L BGEZ,
  A4 A11 A4 ADDX4,  A5 A11 A5 ADDX4,  MEMW,  A9 A8 0 L32I.N,  A2 A11 A3 ADDX4,
  A12 $4 L L32R,  A9 A9 A12 AND,  MEMW,  A9 A8 0 S32I.N,  A9 $8 L L32R,
  A9 A9 A11 SUB,  A9 A9 5 SLLI,  A11 -1 MOVI.N,  A4 A4 0 L32I.N,  A11 A11 12 SLLI,
  MEMW,  A4 A9 0 S32I.N,  A9 $C L L32R,  A9 A9 A10 SUB,  A4 A5 0 L32I.N,  MEMW,
  A4 A9 0 S32I.N,  A9 $10 L L32R,  A9 A9 A10 SUB,  MEMW,  A10 A9 0 L32I.N,
  A10 A10 A11 AND,  A2 A2 0 L32I.N,  A2 A2 0 12 EXTUI,  A2 A2 A10 OR,  MEMW,
  A2 A9 0 S32I.N,  A9 -1 MOVI.N,  A9 A9 31 SLLI,  MEMW,  A2 A8 0 L32I.N,
  A2 A2 A9 OR,  MEMW,  A2 A8 0 S32I.N,
$95 L:
  MEMW,  RETW.N,
$9A L:  END-CODE

LINK-SYMBOLS   \ the cells of other packages that wait for these names
END-PACKAGE  PREVIOUS   \ not in the search order: ALSO name gives its words (README.md)
