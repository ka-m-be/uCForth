\ rate_control.fs - package PP-RATE-CONTROL: rate_control.o of libpp.a (ESP32)
\
\ Made from rate_control.S by s2fs.py, not by hand. Each function of C is a
\ CODE word: its bytes are those of the compiler, at the same offsets of
\ its section (L gives the address of an offset); its literals are at the
\ start of the word, after a jump. The data come first, then the
\ functions, each after those that it calls. The global symbols are
\ PUBLIC, the static ones PRIVATE. lib/esp32/clink.fs explains the words.
\
\ 7 functions (6 global; 1 static without symbol, named after their
\ section), 1 data objects.
\ A function runs as C code with the windowed calls of the Xtensa: call
\ it with CALL-C ( x1 .. xn n xt -- x ), never by its name alone.
\
\ Names of other packages: a literal waits until its package comes
\ (LINK-SYMBOLS); a name outside the libraries until EXTERN gives it.
\
\ Needs XTENSA (asm/xtensa.fs), ASM (lib/esp32/asm.fs) and CLINK
\ (lib/esp32/clink.fs) of ucf; the ESP32.

PACKAGE PP-RATE-CONTROL  ALSO ASM  ALSO CLINK   \ lib/package.fs
EXPORTS   \ the PUBLIC words are symbols for the other packages

\ ==== Data =============================================================

PRIVATE
CREATE rodata.our_controls   \ .rodata.our_controls, 344 bytes
  $00 C,  $00 C,  $04 C,  $00 C,  $3A C,  $01 C,  $00 C,  $00 C,  $00 C,  $00 C,
  $02 C,  $00 C,  $02 C,  $01 C,  $05 C,  $00 C,  $2E C,  $BA C,  $00 C,  $00 C,
  $DF C,  $00 C,  $06 C,  $00 C,  $17 C,  $5D C,  $00 C,  $00 C,  $D5 C,  $00 C,
  $07 C,  $00 C,  $00 C,  $00 C,  $04 C,  $00 C,  $3A C,  $01 C,  $04 C,  $00 C,
  $00 C,  $00 C,  $02 C,  $00 C,  $A2 C,  $00 C,  $05 C,  $00 C,  $2E C,  $BA C,
  $00 C,  $00 C,  $7F C,  $00 C,  $06 C,  $00 C,  $17 C,  $5D C,  $00 C,  $00 C,
  $75 C,  $00 C,  $07 C,  $00 C,  $55 C,  $15 C,  $00 C,  $00 C,  $2C C,  $00 C,
  $09 C,  $00 C,  $AA C,  $2A C,  $00 C,  $00 C,  $2C C,  $00 C,  $09 C,  $00 C,
  $55 C,  $55 C,  $00 C,  $00 C,  $30 C,  $00 C,  $0A C,  $00 C,  $AA C,  $AA C,
  $00 C,  $00 C,  $3C C,  $00 C,  $0B C,  $00 C,  $F6 C,  $12 C,  $00 C,  $00 C,
  $2C C,  $00 C,  $09 C,  $00 C,  $71 C,  $1C C,  $00 C,  $00 C,  $2C C,  $00 C,
  $09 C,  $00 C,  $E3 C,  $38 C,  $00 C,  $00 C,  $30 C,  $00 C,  $0A C,  $00 C,
  $C7 C,  $71 C,  $00 C,  $00 C,  $3C C,  $00 C,  $0B C,  $00 C,  $89 C,  $9D C,
  $00 C,  $00 C,  $3C C,  $00 C,  $0B C,  $00 C,  $C4 C,  $4E C,  $00 C,  $00 C,
  $30 C,  $00 C,  $0A C,  $00 C,  $83 C,  $34 C,  $00 C,  $00 C,  $30 C,  $00 C,
  $0A C,  $00 C,  $62 C,  $27 C,  $00 C,  $00 C,  $2C C,  $00 C,  $09 C,  $00 C,
  $41 C,  $1A C,  $00 C,  $00 C,  $2C C,  $00 C,  $09 C,  $00 C,  $B1 C,  $13 C,
  $00 C,  $00 C,  $2C C,  $00 C,  $09 C,  $00 C,  $81 C,  $11 C,  $00 C,  $00 C,
  $2C C,  $00 C,  $09 C,  $00 C,  $C0 C,  $0F C,  $00 C,  $00 C,  $2C C,  $00 C,
  $09 C,  $00 C,  $C8 C,  $8D C,  $00 C,  $00 C,  $3C C,  $00 C,  $0B C,  $00 C,
  $E4 C,  $46 C,  $00 C,  $00 C,  $30 C,  $00 C,  $0A C,  $00 C,  $42 C,  $2F C,
  $00 C,  $00 C,  $30 C,  $00 C,  $0A C,  $00 C,  $72 C,  $23 C,  $00 C,  $00 C,
  $2C C,  $00 C,  $09 C,  $00 C,  $A1 C,  $17 C,  $00 C,  $00 C,  $2C C,  $00 C,
  $09 C,  $00 C,  $B9 C,  $11 C,  $00 C,  $00 C,  $2C C,  $00 C,  $09 C,  $00 C,
  $C0 C,  $0F C,  $00 C,  $00 C,  $2C C,  $00 C,  $09 C,  $00 C,  $2D C,  $0E C,
  $00 C,  $00 C,  $2C C,  $00 C,  $09 C,  $00 C,  $00 C,  $00 C,  $08 C,  $00 C,
  $0A C,  $02 C,  $2A C,  $00 C,  $00 C,  $00 C,  $10 C,  $00 C,  $EA C,  $02 C,
  $29 C,  $00 C,  $00 C,  $00 C,  $00 C,  $00 C,  $00 C,  $00 C,  $00 C,  $00 C,
  $00 C,  $00 C,  $00 C,  $00 C,  $00 C,  $00 C,  $00 C,  $00 C,  $00 C,  $00 C,
  $00 C,  $00 C,  $00 C,  $00 C,  $00 C,  $00 C,  $00 C,  $00 C,  $00 C,  $00 C,
  $00 C,  $00 C,  $00 C,  $00 C,  $00 C,  $00 C,  $00 C,  $00 C,  $00 C,  $00 C,
  $00 C,  $00 C,  $00 C,  $00 C,  $00 C,  $00 C,  $00 C,  $00 C,  $00 C,  $00 C,
  $00 C,  $00 C,  $00 C,  $00 C,  $00 C,  $00 C,  $00 C,  $00 C,  $00 C,  $00 C,
  $00 C,  $00 C,  $00 C,  $00 C,  $00 C,  $00 C,  $00 C,  $00 C,  $00 C,  $00 C,
  $00 C,  $00 C,  $00 C,  $00 C,

\ ==== Code =============================================================

CODE RC_GetDuration   \ .text.RC_GetDuration $C, static, without symbol
  $0 3 LITERALS
    SYMBOL rodata.our_controls  $3FFF T,  $3FFFF T,
$C L:
  A1 32 ENTRY,  A3 A3 3 SLLI,  A2 8 $18 L BLTUI,
  A3 A3 22 ADDI,
$18 L:
  A8 $0 L L32R,  A8 A2 A8 ADDX8,  A9 $4 L L32R,  A8 A8 0 L32I.N,  A9 A3 $35 L BLTU,
  A3 A3 A8 MULL,  A8 $8 L L32R,  A3 A3 A8 ADD.N,  A3 A3 18 14 EXTUI,  $5E L J,
  1 ZEROS,
$35 L:
  A10 A3 0 9 EXTUI,  A9 A8 0 9 EXTUI,  A3 A3 9 SRLI,  A8 A8 9 16 EXTUI,
  A11 A3 A8 MULL,  A3 A9 A3 MULL,  A8 A10 A8 MULL,  A9 A9 A10 MULL,  A3 A3 A8 ADD.N,
  A3 A3 9 SLLI,  A8 $8 L L32R,  A3 A3 A9 ADD.N,  A3 A3 A8 ADD.N,  A3 A3 18 SRAI,
  A3 A3 A11 ADD.N,
$5E L:
  A2 8 $68 L BLTUI,
  A3 A3 3 ADDI.N,  A2 -4 MOVI.N,  A3 A3 A2 AND,
$68 L:
  A2 A3 0 16 EXTUI,  RETW.N,
$6D L:  END-CODE

PUBLIC
CODE RC_SetBasicRate   \ .text.RC_SetBasicRate $0
  $0 ORIGIN
  A1 32 ENTRY,  A2 0 MOVI.N,  RETW.N,
$7 L:  END-CODE

CODE RC_GetAckRate   \ .text.RC_GetAckRate $4
  $0 1 LITERALS
    SYMBOL rodata.our_controls
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A2 0 8 EXTUI,  A2 A2 A8 ADDX8,  A2 A2 6 L8UI,
  RETW.N,
$15 L:  END-CODE

CODE RC_GetRtsRate   \ .text.RC_GetRtsRate $4
  $0 1 LITERALS
    SYMBOL rodata.our_controls
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A2 0 8 EXTUI,  A2 A2 A8 ADDX8,  A2 A2 6 L8UI,
  RETW.N,
$15 L:  END-CODE

CODE RC_GetAckTime   \ .text.RC_GetAckTime $4
  $0 1 LITERALS
    SYMBOL rodata.our_controls
$4 L:
  A1 32 ENTRY,  A9 $0 L L32R,  A2 A2 0 8 EXTUI,  A2 A2 A9 ADDX8,  A8 A2 6 L8UI,
  A8 A8 A9 ADDX8,  A2 A8 4 L16UI,  RETW.N,
$1B L:  END-CODE

CODE RC_GetCtsTime   \ .text.RC_GetCtsTime $4
  $0 1 LITERALS
    SYMBOL RC_GetDuration
$4 L:
  A1 32 ENTRY,  A10 A2 0 8 EXTUI,  A11 A3 0 16 EXTUI,  A4 A4 0 16 EXTUI,
  A8 10 MOVI.N,  A10 8 $18 L BLTUI,
  A8 16 MOVI,
$18 L:
  A9 A10 -41 ADDI,  A9 A9 0 8 EXTUI,  A5 $120 MOVI,  A9 2 $39 L BLTUI,
  A5 192 MOVI,  A10 5 $39 L BLTUI,
  A5 96 MOVI,  A10 8 $39 L BLTUI,
  A9 15 MOVI.N,  A5 36 MOVI.N,  A9 A10 $39 L BLTU,
  A5 20 MOVI.N,
$39 L:
  A8 A8 A4 ADD.N,  A4 A8 0 16 EXTUI,  A5 A5 A4 ADD.N,  A8 $0 L L32R,  A8 CALLX8,
  A5 A10 A5 ADD.N,  A2 A5 0 16 EXTUI,  RETW.N,
$4D L:  END-CODE

CODE RC_GetBlockAckTime   \ .text.RC_GetBlockAckTime $8
  $0 2 LITERALS
    SYMBOL rodata.our_controls  SYMBOL RC_GetDuration
$8 L:
  A1 32 ENTRY,  A3 $0 L L32R,  A2 A2 0 8 EXTUI,  A2 A2 A3 ADDX8,  A10 A2 6 L8UI,
  A2 10 MOVI,  A10 8 $20 L BLTUI,
  A2 16 MOVI,
$20 L:
  A8 A10 -41 ADDI,  A8 A8 0 8 EXTUI,  A3 $120 MOVI,  A8 2 $41 L BLTUI,
  A3 192 MOVI,  A10 5 $41 L BLTUI,
  A3 96 MOVI,  A10 8 $41 L BLTUI,
  A8 15 MOVI.N,  A3 36 MOVI.N,  A8 A10 $41 L BLTU,
  A3 20 MOVI.N,
$41 L:
  A11 30 MOVI.N,  A8 $4 L L32R,  A8 CALLX8,
  A10 A10 A3 ADD.N,  A2 A2 A10 ADD.N,  A2 A2 0 16 EXTUI,  RETW.N,
$52 L:  END-CODE

LINK-SYMBOLS   \ the cells of other packages that wait for these names
END-PACKAGE  PREVIOUS   \ not in the search order: ALSO name gives its words (README.md)
