\ pm_coex.fs - package PP-PM-COEX: pm_coex.o of libpp.a (ESP32)
\
\ Made from pm_coex.S by s2fs.py, not by hand. Each function of C is a
\ CODE word: its bytes are those of the compiler, at the same offsets of
\ its section (L gives the address of an offset); its literals are at the
\ start of the word, after a jump. The data come first, then the
\ functions, each after those that it calls. The global symbols are
\ PUBLIC, the static ones PRIVATE. lib/esp32/clink.fs explains the words.
\
\ 8 functions (8 global), 3 data objects.
\ A function runs as C code with the windowed calls of the Xtensa: call
\ it with CALL-C ( x1 .. xn n xt -- x ), never by its name alone.
\
\ Names of other packages: a literal waits until its package comes
\ (LINK-SYMBOLS); a name outside the libraries until EXTERN gives it.
\ Uses:
\   PP-PM  g_pm g_pm_cfg
\ Outside the libraries (../README.md):
\   __umoddi3 g_mesh_init_ps_type g_mesh_is_root g_mesh_is_started
\   g_osi_funcs_p g_wifi_mac_time_delta wifi_log
\ C names with more than 31 characters (Forth takes 31): the first 26,
\ ~ and 4 hex digits of their CRC-32 (../README.md):
\   pm_coex_recalculate_wifi_t~9C16 = pm_coex_recalculate_wifi_time_slice
\   pm_coex_update_wifi_time_s~6DA5 = pm_coex_update_wifi_time_slice_state
\   rodata.pm_coex_recalculate~4BEF = rodata.pm_coex_recalculate_wifi_time_slice.str1.1
\
\ Needs XTENSA (asm/xtensa.fs), ASM (lib/esp32/asm.fs) and CLINK
\ (lib/esp32/clink.fs) of ucf; the ESP32.

PACKAGE PP-PM-COEX  ALSO ASM  ALSO CLINK   \ lib/package.fs
EXPORTS   \ the PUBLIC words are symbols for the other packages

\ ==== Data =============================================================

PRIVATE
CREATE rodata.pm_coex_recalculate~4BEF   \ .rodata.pm_coex_recalculate_wifi_time_slice.str1.1, 10 bytes
  ," pm_coex.c"  0 C,
CREATE rodata_wlog_error.6   \ .rodata_wlog_error.6, 6 bytes
  ," %s %d"  0 C,
CREATE rodata_wlog_info.3   \ .rodata_wlog_info.3, 58 bytes
  ," Coexist: Wi-Fi connect fail, apply reconnect coex policy"  10 C,  0 C,

\ ==== Code =============================================================

PUBLIC
CODE pm_coex_reconnect_policy   \ .text.pm_coex_reconnect_policy $4
  $0 1 LITERALS
    SYMBOL g_pm
$4 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A8 1 MOVI.N,  A2 A2 $100 ADDMI,  A2 A2 201 L8UI,
  A2 A2 A8 XOR,  A2 A2 0 8 EXTUI,  RETW.N,
$1A L:  END-CODE

CODE pm_coex_set_reconnect_policy   \ .text.pm_coex_set_reconnect_policy $10
  $0 4 LITERALS
    SYMBOL g_osi_funcs_p  SYMBOL g_pm  SYMBOL rodata_wlog_info.3  SYMBOL wifi_log
$10 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A8 A8 0 L32I.N,  A10 A8 $194 L32I,  A10 CALLX8,
  A10 $44 L BEQZ.N,
  A11 1 MOVI.N,  A8 $4 L L32R,  A9 0 MOVI.N,  A8 A8 $100 ADDMI,  A12 3 MOVI.N,
  A11 A11 12 SLLI,  A13 $8 L L32R,  A10 6 MOVI,  A9 A8 201 S8I,  MEMW,  MEMW,
  A8 $C L L32R,  A8 CALLX8,
$44 L:
  RETW.N,
$46 L:  END-CODE

CODE pm_coex_schm_overall_period_get   \ .wifislprxiram.4 $4
  $0 1 LITERALS
    SYMBOL g_osi_funcs_p
$4 L:
  A1 32 ENTRY,  A3 $0 L L32R,  A2 A3 0 L32I.N,  A10 A2 $1C0 L32I,  A10 CALLX8,
  A2 A10 MOV.N,  A3 A3 0 L32I.N,  A10 A3 $1D8 L32I,  A10 CALLX8,
  A2 A2 A10 MAXU,  A2 A2 0 8 EXTUI,  RETW.N,
$24 L:  END-CODE

CODE pm_get_null_max_tx_time   \ .iram1.2 $24
  $0 9 LITERALS
    SYMBOL g_pm  SYMBOL g_osi_funcs_p  $FFFF00 T,  SYMBOL g_mesh_is_started
    SYMBOL g_mesh_is_root  SYMBOL g_mesh_init_ps_type  $3FF73C00 T,
    SYMBOL g_wifi_mac_time_delta  SYMBOL pm_coex_schm_overall_period_get
$24 L:
  A1 32 ENTRY,  A4 $4 L L32R,  A6 A2 0 8 EXTUI,  A8 A4 0 L32I,  A5 $0 L L32R,
  A10 A8 $194 L32I,  A2 A5 44 L32I,  A10 CALLX8,
  A10 $BA L BEQZ,
  A8 $8 L L32R,  A9 A5 12 L32I.N,  A9 A9 A8 AND,  A8 $101 MOVI,  A8 A8 8 SLLI,
  A9 A8 $BA L BNE,
  A6 $58 L BEQZ.N,
  A2 A5 $1B0 L32I,  $BA L J,
$58 L:
  A2 $C L L32R,  A2 A2 0 L8UI,  A2 $81 L BEQZ.N,
  A2 $10 L L32R,  MEMW,  A2 A2 0 L8UI,  A2 A2 0 8 EXTUI,  A2 $81 L BNEZ.N,
  A2 $14 L L32R,  A2 A2 0 L32I.N,  A2 $81 L BEQZ.N,
  A2 A4 0 L32I.N,  A10 A2 $110 L32I,  A10 CALLX8,
  $90 L J,
  1 ZEROS,
$81 L:
  A2 $18 L L32R,  A8 $1C L L32R,  MEMW,  A2 A2 0 L32I.N,  A8 A8 0 L32I.N,
  A10 A2 A8 ADD.N,
$90 L:
  A2 A10 A3 SUB,  A8 $20 L L32R,  A8 CALLX8,
  A3 A5 44 L32I.N,  A3 A10 A3 MULL,  A4 A4 0 L32I.N,  A8 A5 $1B0 L32I,
  A3 A3 A8 SUB,  A10 A4 $110 L32I,  A10 CALLX8,
  A8 A5 104 L32I,  A2 A3 $BA L BGEU,
  A10 A8 $BA L BGEU,
  A8 A8 A2 ADD.N,  A2 A8 A10 SUB,
$BA L:
  RETW.N,
$BC L:  END-CODE

CODE pm_coex_pwr_update   \ .wifislprxiram.5 $10
  $0 4 LITERALS
    SYMBOL g_pm  SYMBOL g_pm_cfg  SYMBOL g_osi_funcs_p  $100 SYMBOL+ g_pm
$10 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A9 A8 36 L32I.N,  A10 $C L L32R,  A9 $29 L BNEZ.N,
  A9 A10 213 S8I,  MEMW,  MEMW,  $74 L J,
$29 L:
  A11 A8 13 L8UI,  A11 $3C L BNEZ.N,
  A11 A10 213 S8I,  MEMW,  MEMW,  $74 L J,
  2 ZEROS,
$3C L:
  A11 $4 L L32R,  A11 A11 50 L8UI,  A11 A10 213 S8I,  MEMW,  A11 $74 L BEQZ.N,
  A9 2 $60 L BNEI,
  A9 $8 L L32R,  A9 A9 0 L32I.N,  A10 A8 32 L8UI,  A9 A9 $1D4 L32I,  MEMW,  $6E L J,
  2 ZEROS,
$60 L:
  A9 1 $81 L BNEI,
  A9 $8 L L32R,  A9 A9 0 L32I.N,  A10 A8 11 L8UI,  A9 A9 $1D4 L32I,
$6E L:
  A9 CALLX8,
  $81 L J,
$74 L:
  A8 $8 L L32R,  A10 1 MOVI.N,  A8 A8 0 L32I.N,  A8 A8 $1D4 L32I,  A8 CALLX8,
$81 L:
  RETW.N,
$83 L:  END-CODE

CODE pm_coex_pwr_configure   \ .text.pm_coex_pwr_configure $8
  $0 2 LITERALS
    SYMBOL g_pm_cfg  SYMBOL pm_coex_pwr_update
$8 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A8 50 S8I,  MEMW,  MEMW,  A8 $4 L L32R,
  A8 CALLX8,
  RETW.N,
$1F L:  END-CODE

CODE pm_coex_recalculate_wifi_t~9C16   \ pm_coex_recalculate_wifi_time_slice: .text.pm_coex_recalculate_wifi_time_slice $38
  $0 14 LITERALS
    SYMBOL g_osi_funcs_p  SYMBOL g_pm  SYMBOL g_mesh_is_started
    SYMBOL g_mesh_is_root  SYMBOL g_mesh_init_ps_type  $3FF73C00 T,
    SYMBOL g_wifi_mac_time_delta  SYMBOL rodata.pm_coex_recalculate~4BEF
    SYMBOL rodata_wlog_error.6  $F4 SYMBOL+ g_pm
    SYMBOL pm_coex_schm_overall_period_get  SYMBOL __umoddi3  SYMBOL wifi_log
    SYMBOL pm_coex_schm_overall_period_get
$38 L:
  A1 48 ENTRY,  A6 $0 L L32R,  A4 A4 0 8 EXTUI,  A5 A6 0 L32I.N,  A10 A5 $194 L32I,
  A10 CALLX8,
  A10 $208 L BEQZ,
  A5 $4 L L32R,  A4 $79 L BNEZ.N,
  A7 1 MOVI.N,  A10 A5 96 L32I,  A8 A5 $1B0 L32I,  A8 A10 A8 ADD.N,  A9 A5 100 L32I,
  A11 A5 $1B4 L32I,  A8 A10 $66 L BLTU,
  A7 A4 MOV.N,
$66 L:
  A9 A9 A11 ADD.N,  A7 A7 A9 ADD.N,  A3 A7 $70 L BGEU,
  $208 L J,
$70 L:
  A7 A3 $79 L BNE,
  A2 A8 $79 L BGEU,
  $208 L J,
$79 L:
  A8 $28 L L32R,  A8 CALLX8,
  A7 A6 0 L32I.N,  A4 A10 MOV.N,  A10 A7 $1BC L32I,  A10 CALLX8,
  A10 A4 A10 MULL,  A7 $8 L L32R,  A10 A10 A10 ADDX4,  A10 A10 A10 ADDX4,
  A4 A10 2 SLLI,  A7 A7 0 L8UI,  A7 $C1 L BEQZ.N,
  A7 $C L L32R,  MEMW,  A7 A7 0 L8UI,  A7 A7 0 8 EXTUI,  A7 $C1 L BNEZ.N,
  A7 $10 L L32R,  A7 A7 0 L32I.N,  A7 $C1 L BEQZ.N,
  A7 A6 0 L32I.N,  A10 A7 $110 L32I,  A10 CALLX8,
  A7 A10 MOV.N,  $D0 L J,
  2 ZEROS,
$C1 L:
  A7 $14 L L32R,  A8 $18 L L32R,  MEMW,  A7 A7 0 L32I.N,  A8 A8 0 L32I.N,
  A7 A7 A8 ADD.N,
$D0 L:
  A11 1 MOVI.N,  A10 A5 80 L32I,  A10 A7 A10 SUB,  A8 A5 84 L32I,
  A7 A10 $E0 L BLTU,
  A11 0 MOVI.N,
$E0 L:
  A8 A8 NEG,  A12 A4 MOV.N,  A13 0 MOVI.N,  A11 A8 A11 SUB,  A8 $2C L L32R,
  A8 CALLX8,
  A8 A6 0 L32I.N,  A7 A10 MOV.N,  A10 0 MOVI.N,  A4 A11 MOV.N,  A8 A8 $1DC L32I,
  A8 CALLX8,
  A8 A10 MOV.N,  A10 $11D L BNEZ.N,
  A12 1 MOVI.N,  A14 $1C L L32R,  A15 88 MOVI.N,  A11 A12 12 SLLI,  A10 6 MOVI,
  A13 $20 L L32R,  A8 $30 L L32R,  A8 CALLX8,
$118 L:
  $118 L J,
  2 ZEROS,
$11D L:
  A9 A6 0 L32I.N,  A7 A2 A7 SUB,  A10 A9 $1C0 L32I,  A8 A1 4 S32I.N,  MEMW,
  A10 CALLX8,
  A9 A10 MOV.N,  A10 A6 0 L32I.N,  A10 A10 $1BC L32I,  A9 A1 0 S32I.N,  MEMW,
  A10 CALLX8,
  A8 A1 4 L32I.N,  A8 A8 0 L8UI,  A8 A8 A10 MULL,  A9 A1 0 L32I.N,  A8 A8 A9 MULL,
  A9 1 MOVI.N,  A2 A7 $151 L BLTU,
  A9 0 MOVI,
$151 L:
  A4 A3 A4 SUB,  A4 A4 A9 SUB,  A9 A7 A8 ADD.N,  A11 1 MOVI,  A9 A7 $162 L BLTU,
  A11 0 MOVI,
$162 L:
  A11 A11 A4 ADD.N,  A7 A5 13 L8UI,  A4 $3E8 MOVI,  A9 A5 96 S32I,  A11 A5 100 S32I,
  A7 $174 L BEQZ.N,
  A4 A5 56 L32I.N,
$174 L:
  A4 A9 A4 SUB,  A7 1 MOVI,  A9 A4 $180 L BLTU,
  A7 0 MOVI,
$180 L:
  A11 A11 A7 SUB,  A4 A5 104 S32I,  A11 A5 108 S32I,  A8 A1 4 S32I.N,  MEMW,
  A8 $34 L L32R,  A8 CALLX8,
  A4 A10 A10 OR,  A7 A6 0 L32I,  A10 A7 $1BC L32I,  A10 CALLX8,
  A10 A4 A10 MULL,  A4 1 MOVI.N,  A8 A1 4 L32I.N,  A10 A10 A10 ADDX4,
  A10 A10 A10 ADDX4,  A7 A10 2 SLLI,  A10 A10 A8 SUBX4,  A7 A10 $1B8 L BLTU,
  A4 0 MOVI.N,
$1B8 L:
  A7 A6 0 L32I.N,  A4 A4 NEG,  A4 A5 $1B4 S32I,  A4 $24 L L32R,  A7 A7 236 L32I,
  A10 A5 $1B0 S32I,  A10 A4 MOV.N,  MEMW,  A7 CALLX8,
  A7 A5 108 L32I,  A11 A5 104 L32I,  A3 A7 $1E0 L BLTU,
  A7 A3 $200 L BNE,
  A2 A11 $200 L BGEU,
$1E0 L:
  A3 1 MOVI.N,  A3 A5 24 S8I,  MEMW,  A12 0 MOVI.N,  A11 A11 A2 SUB,  A10 A4 A4 OR,
  A3 A6 0 L32I,  A3 A3 248 L32I,  MEMW,  A3 CALLX8,
  $208 L J,
  1 ZEROS,
$200 L:
  A2 0 MOVI.N,  A2 A5 24 S8I,  MEMW,
$208 L:
  MEMW,  RETW.N,
$20D L:  END-CODE

CODE pm_coex_update_wifi_time_s~6DA5   \ pm_coex_update_wifi_time_slice_state: .text.pm_coex_update_wifi_time_slice_state $10
  $0 4 LITERALS
    SYMBOL g_osi_funcs_p  SYMBOL g_pm  $1387 T,
    SYMBOL pm_coex_recalculate_wifi_t~9C16
$10 L:
  A1 32 ENTRY,  A12 A4 0 8 EXTUI,  A10 A2 MOV.N,  A11 A3 A3 OR,  A8 $C L L32R,
  A8 CALLX8,
  A4 $0 L L32R,  A8 A4 0 L32I.N,  A10 A8 $194 L32I,  A10 CALLX8,
  A9 $4 L L32R,  A10 $58 L BEQZ.N,
  A8 A9 108 L32I,  A10 A9 104 L32I,  A3 A8 $40 L BLTU,
  A8 A3 $70 L BNE,
  A2 A10 $70 L BGEU,
$40 L:
  A11 A10 A2 SUB,  A12 1 MOVI,  A10 A11 $4C L BLTU,
  A12 0 MOVI,
$4C L:
  A8 A8 A3 SUB,  A8 A12 $58 L BNE,
  A3 $8 L L32R,  A3 A11 $70 L BGEU,
$58 L:
  A11 0 MOVI.N,  A10 1 MOVI,  A3 A4 0 L32I,  A12 A9 96 L32I,  A12 A12 A2 SUB,
  A3 A3 $19C L32I,  A3 CALLX8,
  $7A L J,
  1 ZEROS,
$70 L:
  A2 A4 0 L32I.N,  A10 1 MOVI.N,  A2 A2 $1A0 L32I,  A2 CALLX8,
$7A L:
  RETW.N,
$7C L:  END-CODE

LINK-SYMBOLS   \ the cells of other packages that wait for these names
END-PACKAGE  PREVIOUS   \ not in the search order: ALSO name gives its words (README.md)
