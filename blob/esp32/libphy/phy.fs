\ phy.fs - package PHY-PHY: phy.o of libphy.a (ESP32)
\
\ Made from phy.S by s2fs.py, not by hand. Each function of C is a
\ CODE word: its bytes are those of the compiler, at the same offsets of
\ its section (L gives the address of an offset); its literals are at the
\ start of the word, after a jump. The data come first, then the
\ functions, each after those that it calls. The global symbols are
\ PUBLIC, the static ones PRIVATE. lib/esp32/clink.fs explains the words.
\
\ 3 functions (3 global), 0 data objects.
\ A function runs as C code with the windowed calls of the Xtensa: call
\ it with CALL-C ( x1 .. xn n xt -- x ), never by its name alone.
\
\ Names of other packages: a literal waits until its package comes
\ (LINK-SYMBOLS); a name outside the libraries until EXTERN gives it.
\ Uses:
\   PHY-PHY-CHIP-V7  set_chanfreq set_chanfreq_nomac
\   PHY-PHY-CHIP-V7-ANA  chip_v7_set_chan
\
\ Needs XTENSA (asm/xtensa.fs), ASM (lib/esp32/asm.fs) and CLINK
\ (lib/esp32/clink.fs) of ucf; the ESP32.

PACKAGE PHY-PHY  ALSO ASM  ALSO CLINK   \ lib/package.fs
EXPORTS   \ the PUBLIC words are symbols for the other packages

\ ==== Code =============================================================

CODE RFChannelSel   \ .text.RFChannelSel $4
  $0 1 LITERALS
    SYMBOL chip_v7_set_chan
$4 L:
  A1 32 ENTRY,  A11 A3 7 SEXT,  A10 A2 7 SEXT,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$15 L:  END-CODE

CODE phy_change_channel   \ .text.phy_change_channel $4
  $0 1 LITERALS
    SYMBOL set_chanfreq
$4 L:
  A1 32 ENTRY,  A10 A2 0 16 EXTUI,  A11 A5 7 SEXT,  A8 $0 L L32R,  A8 CALLX8,
  A2 0 MOVI.N,  RETW.N,
$17 L:  END-CODE

CODE phy_change_channel_nomac   \ .text.phy_change_channel_nomac $4
  $0 1 LITERALS
    SYMBOL set_chanfreq_nomac
$4 L:
  A1 32 ENTRY,  A10 A2 0 16 EXTUI,  A11 A5 7 SEXT,  A8 $0 L L32R,  A8 CALLX8,
  A2 0 MOVI.N,  RETW.N,
$17 L:  END-CODE

LINK-SYMBOLS   \ the cells of other packages that wait for these names
END-PACKAGE  PREVIOUS   \ not in the search order: ALSO name gives its words (README.md)
