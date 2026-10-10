\ pp_timer.fs - package PP-PP-TIMER: pp_timer.o of libpp.a (ESP32)
\
\ Made from pp_timer.S by s2fs.py, not by hand. Each function of C is a
\ CODE word: its bytes are those of the compiler, at the same offsets of
\ its section (L gives the address of an offset); its literals are at the
\ start of the word, after a jump. The data come first, then the
\ functions, each after those that it calls. The global symbols are
\ PUBLIC, the static ones PRIVATE. lib/esp32/clink.fs explains the words.
\
\ 17 functions (3 global; 14 static without symbol, named after their
\ section), 4 data objects.
\ A function runs as C code with the windowed calls of the Xtensa: call
\ it with CALL-C ( x1 .. xn n xt -- x ), never by its name alone.
\
\ Names of other packages: a literal waits until its package comes
\ (LINK-SYMBOLS); a name outside the libraries until EXTERN gives it.
\ Uses:
\   PP-PM  pm_active_timeout_process pm_beacon_monitor_tbtt_tim~C271
\          pm_beacon_monitor_timeout_~294B
\          pm_coex_restart_timeout_process pm_coex_slice_timeout_process
\          pm_connectionless_wake_int~299D
\          pm_connectionless_wake_win~1498
\          pm_disconnected_sleep_dela~E633 pm_dream_timeout_process
\          pm_max_idle_timeout_process pm_noise_check_process
\          pm_sleep_delay_timeout_process pm_tbtt_process
\   PP-PM-BEACON-OFFSET  ptr_beacon_offset_funcs
\ Outside the libraries (../README.md):
\   g_osi_funcs_p wifi_log
\ C names with more than 31 characters (Forth takes 31): the first 26,
\ ~ and 4 hex digits of their CRC-32 (../README.md):
\   pm_beacon_monitor_tbtt_tim~C271 = pm_beacon_monitor_tbtt_timeout_process
\   pm_beacon_monitor_timeout_~294B = pm_beacon_monitor_timeout_process
\   pm_connectionless_wake_int~299D = pm_connectionless_wake_interval_timeout_process
\   pm_connectionless_wake_win~1498 = pm_connectionless_wake_window_timeout_process
\   pm_disconnected_sleep_dela~E633 = pm_disconnected_sleep_delay_timeout_process
\   pp_timer_connectionless_wa~49BF = pp_timer_connectionless_wake_interval
\   pp_timer_connectionless_wa~BB8F = pp_timer_connectionless_wake_window
\   pp_timer_disconnected_slee~8534 = pp_timer_disconnected_sleep_delay
\
\ Needs XTENSA (asm/xtensa.fs), ASM (lib/esp32/asm.fs) and CLINK
\ (lib/esp32/clink.fs) of ucf; the ESP32.

PACKAGE PP-PP-TIMER  ALSO ASM  ALSO CLINK   \ lib/package.fs
EXPORTS   \ the PUBLIC words are symbols for the other packages

\ ==== Data =============================================================

PRIVATE
CREATE rodata_wlog_warning.5   \ .rodata_wlog_warning.5, 17 bytes
  ," no timer handle"  10 C,  0 C,
CREATE rodata_wlog_warning.4   \ .rodata_wlog_warning.4, 9 bytes
  ," timer e"  10 C,  0 C,
PUBLIC
120 DATA: g_pp_timer_info   \ .data.g_pp_timer_info
  $00  $00  $00  $00  $00  $00  $00  $00  $01  $00  $00  $00  $00  $00  $00  $00  16 g_pp_timer_info INIT
  $02  $00  $00  $00  $00  $00  $00  $00  $03  $00  $00  $00  $00  $00  $00  $00  16 g_pp_timer_info $10 + INIT
  $04  $00  $00  $00  $00  $00  $00  $00  $05  $00  $00  $00  $00  $00  $00  $00  16 g_pp_timer_info $20 + INIT
  $06  $00  $00  $00  $00  $00  $00  $00  $07  $00  $00  $00  $00  $00  $00  $00  16 g_pp_timer_info $30 + INIT
  $08  $00  $00  $00  $00  $00  $00  $00  $09  $00  $00  $00  $00  $00  $00  $00  16 g_pp_timer_info $40 + INIT
  $0A  $00  $00  $00  $00  $00  $00  $00  $0B  $00  $00  $00  $00  $00  $00  $00  16 g_pp_timer_info $50 + INIT
  $0C  $00  $00  $00  $00  $00  $00  $00  $0D  $00  $00  $00  $00  $00  $00  $00  16 g_pp_timer_info $60 + INIT
PRIVATE
4 DATA: bss.s_timer_post   \ .bss.s_timer_post

\ ==== Code =============================================================

CODE pp_timer_resample   \ .text.pp_timer_resample $4, static, without symbol
  $0 1 LITERALS
    SYMBOL ptr_beacon_offset_funcs
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A8 A8 0 L32I.N,  A8 A8 24 L32I.N,  A8 $15 L BEQZ.N,
  A10 A2 4 L32I.N,  A8 CALLX8,
$15 L:
  A2 1 MOVI.N,  RETW.N,
$19 L:  END-CODE

CODE pp_timer_coex_restart   \ .text.pp_timer_coex_restart $4, static, without symbol
  $0 1 LITERALS
    SYMBOL pm_coex_restart_timeout_process
$4 L:
  A1 32 ENTRY,  A10 A2 4 L32I.N,  A2 1 MOVI.N,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$13 L:  END-CODE

CODE pp_timer_max_idle   \ .text.pp_timer_max_idle $4, static, without symbol
  $0 1 LITERALS
    SYMBOL pm_max_idle_timeout_process
$4 L:
  A1 32 ENTRY,  A10 A2 4 L32I.N,  A2 1 MOVI.N,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$13 L:  END-CODE

CODE pp_timer_beacon_monitor_tbtt   \ .text.pp_timer_beacon_monitor_tbtt $4, static, without symbol
  $0 1 LITERALS
    SYMBOL pm_beacon_monitor_tbtt_tim~C271
$4 L:
  A1 32 ENTRY,  A10 A2 4 L32I.N,  A2 1 MOVI.N,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$13 L:  END-CODE

CODE pp_timer_beacon_monitor   \ .text.pp_timer_beacon_monitor $4, static, without symbol
  $0 1 LITERALS
    SYMBOL pm_beacon_monitor_timeout_~294B
$4 L:
  A1 32 ENTRY,  A10 A2 4 L32I.N,  A2 1 MOVI.N,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$13 L:  END-CODE

CODE pp_timer_connectionless_wa~BB8F   \ .text.pp_timer_connectionless_wake_window $4, static, without symbol
  $0 1 LITERALS
    SYMBOL pm_connectionless_wake_win~1498
$4 L:
  A1 32 ENTRY,  A10 A2 4 L32I.N,  A2 1 MOVI.N,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$13 L:  END-CODE

CODE pp_timer_connectionless_wa~49BF   \ .text.pp_timer_connectionless_wake_interval $4, static, without symbol
  $0 1 LITERALS
    SYMBOL pm_connectionless_wake_int~299D
$4 L:
  A1 32 ENTRY,  A10 A2 4 L32I.N,  A2 1 MOVI.N,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$13 L:  END-CODE

CODE pp_timer_disconnected_slee~8534   \ .text.pp_timer_disconnected_sleep_delay $4, static, without symbol
  $0 1 LITERALS
    SYMBOL pm_disconnected_sleep_dela~E633
$4 L:
  A1 32 ENTRY,  A10 A2 4 L32I.N,  A2 1 MOVI.N,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$13 L:  END-CODE

CODE pp_timer_sleep_delay   \ .text.pp_timer_sleep_delay $4, static, without symbol
  $0 1 LITERALS
    SYMBOL pm_sleep_delay_timeout_process
$4 L:
  A1 32 ENTRY,  A10 A2 4 L32I.N,  A2 1 MOVI.N,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$13 L:  END-CODE

CODE pp_timer_coex_slice   \ .text.pp_timer_coex_slice $4, static, without symbol
  $0 1 LITERALS
    SYMBOL pm_coex_slice_timeout_process
$4 L:
  A1 32 ENTRY,  A10 A2 4 L32I.N,  A2 1 MOVI.N,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$13 L:  END-CODE

CODE pp_timer_active   \ .text.pp_timer_active $4, static, without symbol
  $0 1 LITERALS
    SYMBOL pm_active_timeout_process
$4 L:
  A1 32 ENTRY,  A10 A2 4 L32I.N,  A2 1 MOVI.N,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$13 L:  END-CODE

CODE pp_timer_dream   \ .text.pp_timer_dream $4, static, without symbol
  $0 1 LITERALS
    SYMBOL pm_dream_timeout_process
$4 L:
  A1 32 ENTRY,  A10 A2 4 L32I.N,  A2 1 MOVI.N,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$13 L:  END-CODE

CODE wifislpiram.2   \ .wifislpiram.2 $4, static, without symbol
  $0 1 LITERALS
    SYMBOL pm_tbtt_process
$4 L:
  A1 32 ENTRY,  A10 A2 4 L32I.N,  A2 1 MOVI.N,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$13 L:  END-CODE

CODE pp_timer_noise_check   \ .text.pp_timer_noise_check $4, static, without symbol
  $0 1 LITERALS
    SYMBOL pm_noise_check_process
$4 L:
  A1 32 ENTRY,  A10 A2 4 L32I.N,  A2 1 MOVI.N,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$13 L:  END-CODE

PUBLIC
CODE pp_timer_do_process   \ .wifislpiram.3 $18
  $0 6 LITERALS
    SYMBOL rodata_wlog_warning.4  SYMBOL g_pp_timer_info
    SYMBOL rodata_wlog_warning.5  SYMBOL g_osi_funcs_p  SYMBOL wifi_log
    SYMBOL wifi_log
$18 L:
  A1 32 ENTRY,  A2 $34 L BNEZ.N,
  A13 $0 L L32R,  A11 1 MOVI.N,  A12 2 MOVI.N,  A11 A11 11 SLLI,  A10 6 MOVI,
  A8 $10 L L32R,  A8 CALLX8,
  $A8 L J,
  1 ZEROS,
$34 L:
  A12 A2 0 L8UI,  A9 $4 L L32R,  A8 A12 A9 ADDX8,  A8 A8 0 L8UI,  A8 A12 $5C L BEQ,
  A11 A9 MOV.N,  A8 0 MOVI.N,  A10 14 MOVI.N,  A10 $59 L LOOP,
  A13 A11 0 L8UI,  A13 A12 $61 L BEQ,
  A8 A8 1 ADDI.N,  A8 A8 0 8 EXTUI,  A11 A11 8 ADDI.N,
$59 L:
  $70 L J,
$5C L:
  A10 13 MOVI.N,  A10 A8 $70 L BLTU,
$61 L:
  A8 A8 A9 ADDX8,  A8 A8 4 L32I.N,  A8 $99 L BEQZ.N,
  A10 A2 MOV.N,  A8 CALLX8,
  $99 L J,
$70 L:
  A13 $8 L L32R,  A11 1 MOVI,  A12 2 MOVI,  A11 A11 11 SLLI,  A10 6 MOVI,
  A8 $14 L L32R,  A8 CALLX8,
  A8 $C L L32R,  A10 A2 MOV.N,  A2 0 MOVI.N,  A8 A8 0 L32I.N,  A8 A8 176 L32I,
  A8 CALLX8,
  $A8 L J,
  2 ZEROS,
$99 L:
  A8 $C L L32R,  A10 A2 MOV.N,  A2 1 MOVI.N,  A8 A8 0 L32I.N,  A8 A8 176 L32I,
  A8 CALLX8,
$A8 L:
  RETW.N,
$AA L:  END-CODE

CODE pp_timer_register_post_cb   \ .text.pp_timer_register_post_cb $4
  $0 1 LITERALS
    SYMBOL bss.s_timer_post
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A8 0 S32I.N,  A2 0 MOVI.N,  MEMW,  RETW.N,
$13 L:  END-CODE

CODE pp_timer_process   \ .text.pp_timer_process $4
  $0 1 LITERALS
    SYMBOL bss.s_timer_post
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A12 A3 MOV.N,  A11 A2 0 8 EXTUI,  A10 -1 MOVI.N,
  A8 A8 0 L32I.N,  A8 $1A L BEQZ.N,
  A10 8 MOVI.N,  A8 CALLX8,
$1A L:
  A2 A10 MOV.N,  RETW.N,
$1E L:  END-CODE

\ ==== Addresses in the data ============================================

0 g_pp_timer_info $4 + SYMBOL! pp_timer_noise_check
0 g_pp_timer_info $C + SYMBOL! wifislpiram.2
0 g_pp_timer_info $14 + SYMBOL! pp_timer_dream
0 g_pp_timer_info $1C + SYMBOL! pp_timer_active
0 g_pp_timer_info $24 + SYMBOL! pp_timer_coex_slice
0 g_pp_timer_info $2C + SYMBOL! pp_timer_sleep_delay
0 g_pp_timer_info $34 + SYMBOL! pp_timer_disconnected_slee~8534
0 g_pp_timer_info $3C + SYMBOL! pp_timer_connectionless_wa~49BF
0 g_pp_timer_info $44 + SYMBOL! pp_timer_connectionless_wa~BB8F
0 g_pp_timer_info $4C + SYMBOL! pp_timer_beacon_monitor
0 g_pp_timer_info $54 + SYMBOL! pp_timer_beacon_monitor_tbtt
0 g_pp_timer_info $5C + SYMBOL! pp_timer_max_idle
0 g_pp_timer_info $64 + SYMBOL! pp_timer_resample
0 g_pp_timer_info $6C + SYMBOL! pp_timer_coex_restart

LINK-SYMBOLS   \ the cells of other packages that wait for these names
END-PACKAGE  PREVIOUS   \ not in the search order: ALSO name gives its words (README.md)
