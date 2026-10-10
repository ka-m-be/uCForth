\ pm.fs - package PP-PM: pm.o of libpp.a (ESP32)
\
\ Made from pm.S by s2fs.py, not by hand. Each function of C is a
\ CODE word: its bytes are those of the compiler, at the same offsets of
\ its section (L gives the address of an offset); its literals are at the
\ start of the word, after a jump. The data come first, then the
\ functions, each after those that it calls. The global symbols are
\ PUBLIC, the static ones PRIVATE. lib/esp32/clink.fs explains the words.
\
\ 157 functions (139 global; 18 static without symbol, named after their
\ section), 29 data objects.
\ A function runs as C code with the windowed calls of the Xtensa: call
\ it with CALL-C ( x1 .. xn n xt -- x ), never by its name alone.
\
\ Names of other packages: a literal waits until its package comes
\ (LINK-SYMBOLS); a name outside the libraries until EXTERN gives it.
\ Uses:
\   PHY-PHY-CHIP-V7  noise_check_loop read_hw_noisefloor
\   PP-PM-BEACON-OFFSET  ptr_beacon_offset_funcs
\   PP-PM-COEX  pm_coex_pwr_update pm_coex_schm_overall_period_get
\   PP-PP  ppCheckTxConnTrafficIdle ppCheckTxIdle ppCheckTxQIdle
\          ppProcessWaitingQueue ppRegisterTxCallback
\   PP-PP-DEBUG  dbg_lmac_ps_statis_reset g_pm_cnt wifi_gpio_debug
\   PP-PP-TIMER  pp_timer_process
\   PP-WDEV  wDevCtrl wdev_mac_sleep wdev_mac_sleep_deinit
\            wdev_mac_sleep_init wdev_mac_wakeup
\ Outside the libraries (../README.md):
\   __divsf3 __extendsfdf2 __floatundisf __udivdi3 __umoddi3
\   esp_mesh_get_running_activ~0BB2 esp_mesh_quick_funcs esp_wifi_get_mode
\   g_intr_lock_mux g_mesh_init_ps_type g_mesh_is_root g_mesh_is_started
\   g_osi_funcs_p g_wifi_mac_time_delta ieee80211_hostapd_beacon_txcb memcmp
\   memcpy memset sta_reset_beacon_timeout wifi_event_post wifi_log
\   wifi_nvs_get_sta_listen_in~409E wifi_rf_phy_disable wifi_rf_phy_enable
\ C names with more than 31 characters (Forth takes 31): the first 26,
\ ~ and 4 hex digits of their CRC-32 (../README.md):
\   esp_mesh_get_running_activ~0BB2 = esp_mesh_get_running_active_duty_cycle
\   pm_beacon_miss_exceeded_wa~D8EB = pm_beacon_miss_exceeded_wakeup_disabled
\   pm_beacon_monitor_tbtt_tim~C271 = pm_beacon_monitor_tbtt_timeout_process
\   pm_beacon_monitor_timeout_~294B = pm_beacon_monitor_timeout_process
\   pm_coex_separate_connectio~659C = pm_coex_separate_connectionless_window
\   pm_connectionless_module_s~04A2 = pm_connectionless_module_set_wake_interval
\   pm_connectionless_wake_int~B6C6 = pm_connectionless_wake_interval_timeout
\   pm_connectionless_wake_int~299D = pm_connectionless_wake_interval_timeout_process
\   pm_connectionless_wake_win~1EF1 = pm_connectionless_wake_window_timeout
\   pm_connectionless_wake_win~1498 = pm_connectionless_wake_window_timeout_process
\   pm_disable_disconnected_sl~DBBF = pm_disable_disconnected_sleep_delay_timer
\   pm_disconnected_sleep_dela~50E1 = pm_disconnected_sleep_delay_timeout
\   pm_disconnected_sleep_dela~E633 = pm_disconnected_sleep_delay_timeout_process
\   pm_enable_disconnected_sle~30EA = pm_enable_disconnected_sleep_delay_timer
\   pm_enable_send_wake_null_o~C1AF = pm_enable_send_wake_null_on_channel
\   pm_enable_sta_disconnected~B345 = pm_enable_sta_disconnected_power_management
\   pm_register_connectionless~CFAC = pm_register_connectionless_wake_window
\   pm_set_sleep_wait_broadcas~0D70 = pm_set_sleep_wait_broadcast_data_time
\   pm_tx_null_data_done_quick~6418 = pm_tx_null_data_done_quick_wake_process
\   pm_unregister_connectionle~9CF6 = pm_unregister_connectionless_wake_window
\   pm_update_by_connectionles~3F93 = pm_update_by_connectionless_status
\   pm_update_light_sleep_defa~2A79 = pm_update_light_sleep_default_params
\   pm_update_modem_sleep_defa~4980 = pm_update_modem_sleep_default_params
\   wifi_nvs_get_sta_listen_in~409E = wifi_nvs_get_sta_listen_interval
\
\ Needs XTENSA (asm/xtensa.fs), ASM (lib/esp32/asm.fs) and CLINK
\ (lib/esp32/clink.fs) of ucf; the ESP32.

PACKAGE PP-PM  ALSO ASM  ALSO CLINK   \ lib/package.fs
EXPORTS   \ the PUBLIC words are symbols for the other packages

\ ==== Data =============================================================

PRIVATE
CREATE rodata.str1.1   \ .rodata.str1.1, 5 bytes
  ," pm.c"  0 C,
20 DATA: bss.beacon_monitor_data$0   \ .bss.beacon_monitor_data$0
CREATE rodata_wlog_debug.63   \ .rodata_wlog_debug.63, 17 bytes
  ," bcnd: %lu -> %lu"  0 C,
CREATE rodata_wlog_info.61   \ .rodata_wlog_info.61, 63 bytes
  ," Coexist!!! Wi-Fi station would only keep waked when availabl"  ," e"  10 C,
  0 C,
CREATE rodata_wlog_info.60   \ .rodata_wlog_info.60, 30 bytes
  ," Set ps type: %d, coexist: %d"  10 C,  0 C,
CREATE rodata_wlog_info.59   \ .rodata_wlog_info.59, 86 bytes
  ," (mesh)pm stop, sleep:%lld secs, total:%lld secs, current act"
  ," ive set:%d, running:%.02f"  0 C,
CREATE rodata_wlog_info.58   \ .rodata_wlog_info.58, 46 bytes
  ," pm stop, total sleep time: %llu us / %llu us"  10 C,  0 C,
CREATE rodata_wlog_info.57   \ .rodata_wlog_info.57, 20 bytes
  ," pm start, type: %d"  10 C,  0 C,
CREATE rodata_wlog_info.53   \ .rodata_wlog_info.53, 63 bytes
  ," Coexist!!! Wi-Fi station would only keep waked when availabl"  ," e"  10 C,
  0 C,
CREATE rodata_wlog_warning.41   \ .rodata_wlog_warning.41, 142 bytes
  ," Next TBTT incorrect! last beacon:%u, offset:%u, next beacon:"
  ," %u, beacon interval:%u, dtim period:%u, dtim count:%u, liste"
  ," n interval:%u, now:%u"  0 C,
CREATE rodata_wlog_error.39   \ .rodata_wlog_error.39, 6 bytes
  ," %s %d"  0 C,
CREATE rodata_wlog_warning.37   \ .rodata_wlog_warning.37, 80 bytes
  ," TBTT delay too much! entry:%llu, now:%llu, next beacon:%llu,"
  ,"  beacon interval:%u"  0 C,
CREATE rodata_wlog_debug.35   \ .rodata_wlog_debug.35, 31 bytes
  ," Executing offchan operation %p"  0 C,
CREATE rodata_wlog_error.34   \ .rodata_wlog_error.34, 26 bytes
  ," failed to send NULL frame"  0 C,
CREATE rodata_wlog_info.18   \ .rodata_wlog_info.18, 47 bytes
  ," AP's beacon interval = %u us, DTIM period = %u"  0 C,
CREATE rodata_wlog_info.17   \ .rodata_wlog_info.17, 46 bytes
  ," And use another outer/mobile-hotspot instead."  0 C,
CREATE rodata_wlog_info.16   \ .rodata_wlog_info.16, 72 bytes
  ," Please report this bug to the manufaturer of the router/mobi"  ," le-hotspot."
  0 C,
CREATE rodata_wlog_info.15   \ .rodata_wlog_info.15, 58 bytes
  ," The router/mobile-hotspot does not follow Wi-Fi protocol."  0 C,
CREATE rodata_wlog_info.14   \ .rodata_wlog_info.14, 36 bytes
  ," There is no TIM IE in Beacon frame."  0 C,
CREATE rodata_wlog_info.13   \ .rodata_wlog_info.13, 46 bytes
  ," And use another outer/mobile-hotspot instead."  0 C,
CREATE rodata_wlog_info.12   \ .rodata_wlog_info.12, 72 bytes
  ," Please report this bug to the manufaturer of the router/mobi"  ," le-hotspot."
  0 C,
CREATE rodata_wlog_info.11   \ .rodata_wlog_info.11, 58 bytes
  ," The router/mobile-hotspot does not follow Wi-Fi protocol."  0 C,
CREATE rodata_wlog_info.10   \ .rodata_wlog_info.10, 41 bytes
  ," Beacon interval IE of Beacon frame is 0."  0 C,
CREATE rodata_wlog_info.6   \ .rodata_wlog_info.6, 66 bytes
  ," dp: %u, bi: %u, li: %u, scale listen interval from %u us to "  ," %u us"  0 C,
4 DATA: bss.g_offchan_ready_ctx   \ .bss.g_offchan_ready_ctx
4 DATA: bss.g_offchan_ready_cb   \ .bss.g_offchan_ready_cb
4 DATA: bss.g_pm_tx_null_func   \ .bss.g_pm_tx_null_func
PUBLIC
8 DATA-ALIGN
544 DATA: g_pm   \ .bss.g_pm
52 DATA: g_pm_cfg   \ .data.g_pm_cfg
  $00  $00  $00  $00  $B8  $0B  $00  $00  $AC  $0D  $00  $00  $A8  $61  $00  $00  16 g_pm_cfg INIT
  $80  $96  $98  $00  $50  $C3  $00  $00  $88  $13  $00  $00  $00  $00  $00  $00  16 g_pm_cfg $10 + INIT
  $00  $00  $00  $00  $00  $19  $03  $00  $00  $00  $00  $00  $98  $3A  $00  $00  16 g_pm_cfg $20 + INIT
  $3C  $00  $00  $01  4 g_pm_cfg $30 + INIT

\ ==== Code =============================================================

PRIVATE
CODE hal_pm_unblock_txq   \ .text.hal_pm_unblock_txq $8, static, without symbol
  $0 2 LITERALS
    $3FF73CB8 T,  $FFF1FFFF T,
$8 L:
  A1 32 ENTRY,  A9 $0 L L32R,  A10 $4 L L32R,  MEMW,  A8 A9 0 L32I.N,
  A8 A8 A10 AND,  MEMW,  A8 A9 0 S32I.N,  MEMW,  RETW.N,
$23 L:  END-CODE

CODE pm_disable_max_idle_timer   \ .text.pm_disable_max_idle_timer $8, static, without symbol
  $0 2 LITERALS
    $1EC SYMBOL+ g_pm  SYMBOL g_osi_funcs_p
$8 L:
  A1 32 ENTRY,  A8 $4 L L32R,  A8 A8 0 L32I.N,  A10 $0 L L32R,  A8 A8 236 L32I,
  A8 CALLX8,
  RETW.N,
$1B L:  END-CODE

CODE pm_incr_coex_active_cnt   \ .text.pm_incr_coex_active_cnt $4, static, without symbol
  $0 1 LITERALS
    SYMBOL g_pm
$4 L:
  A1 32 ENTRY,  A9 $0 L L32R,  A10 1 MOVI.N,  A8 A9 64 L32I,  A8 A8 1 ADDI.N,
  A8 A8 A10 MAXU,  A8 A9 64 S32I,  MEMW,  RETW.N,
$1C L:  END-CODE

CODE pm_disable_active_timer   \ .text.pm_disable_active_timer $8, static, without symbol
  $0 2 LITERALS
    $CC SYMBOL+ g_pm  SYMBOL g_osi_funcs_p
$8 L:
  A1 32 ENTRY,  A8 $4 L L32R,  A8 A8 0 L32I.N,  A10 $0 L L32R,  A8 A8 236 L32I,
  A8 CALLX8,
  RETW.N,
$1B L:  END-CODE

CODE pm_set_state   \ .text.pm_set_state $C, static, without symbol
  $0 3 LITERALS
    SYMBOL g_pm  SYMBOL wifi_gpio_debug  SYMBOL wifi_gpio_debug
$C L:
  A1 32 ENTRY,  A3 $0 L L32R,  A10 A3 1 L8UI,  A10 A2 $30 L BEQ,
  A4 5 MOVI.N,  A11 1 MOVI.N,  A10 A4 A10 SUB,  A8 $4 L L32R,  A8 CALLX8,
  A11 0 MOVI.N,  A10 A4 A2 SUB,  A8 $8 L L32R,  A8 CALLX8,
$30 L:
  A2 A3 1 S8I,  MEMW,  MEMW,  RETW.N,
$3B L:  END-CODE

CODE pm_beacon_monitor_tbtt_timeout   \ .text.pm_beacon_monitor_tbtt_timeout $4, static, without symbol
  $0 1 LITERALS
    SYMBOL pp_timer_process
$4 L:
  A1 32 ENTRY,  A11 A2 MOV.N,  A10 10 MOVI.N,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$13 L:  END-CODE

CODE pm_coex_restart_timeout   \ .text.pm_coex_restart_timeout $4, static, without symbol
  $0 1 LITERALS
    SYMBOL pp_timer_process
$4 L:
  A1 32 ENTRY,  A11 0 MOVI.N,  A10 13 MOVI.N,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$13 L:  END-CODE

CODE pm_max_idle_timeout   \ .text.pm_max_idle_timeout $4, static, without symbol
  $0 1 LITERALS
    SYMBOL pp_timer_process
$4 L:
  A1 32 ENTRY,  A11 0 MOVI.N,  A10 11 MOVI.N,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$13 L:  END-CODE

CODE pm_beacon_monitor_timeout   \ .text.pm_beacon_monitor_timeout $4, static, without symbol
  $0 1 LITERALS
    SYMBOL pp_timer_process
$4 L:
  A1 32 ENTRY,  A11 A2 MOV.N,  A10 9 MOVI.N,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$13 L:  END-CODE

CODE pm_connectionless_wake_win~1EF1   \ .text.pm_connectionless_wake_window_timeout $4, static, without symbol
  $0 1 LITERALS
    SYMBOL pp_timer_process
$4 L:
  A1 32 ENTRY,  A11 0 MOVI.N,  A10 8 MOVI.N,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$13 L:  END-CODE

CODE pm_connectionless_wake_int~B6C6   \ .text.pm_connectionless_wake_interval_timeout $4, static, without symbol
  $0 1 LITERALS
    SYMBOL pp_timer_process
$4 L:
  A1 32 ENTRY,  A11 0 MOVI.N,  A10 7 MOVI.N,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$13 L:  END-CODE

CODE pm_disconnected_sleep_dela~50E1   \ .text.pm_disconnected_sleep_delay_timeout $4, static, without symbol
  $0 1 LITERALS
    SYMBOL pp_timer_process
$4 L:
  A1 32 ENTRY,  A11 0 MOVI.N,  A10 6 MOVI.N,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$13 L:  END-CODE

CODE pm_coex_slice_wifi_timeout   \ .text.pm_coex_slice_wifi_timeout $4, static, without symbol
  $0 1 LITERALS
    SYMBOL pp_timer_process
$4 L:
  A1 32 ENTRY,  A11 0 MOVI.N,  A10 4 MOVI.N,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$13 L:  END-CODE

CODE pm_sleep_delay_timeout   \ .text.pm_sleep_delay_timeout $4, static, without symbol
  $0 1 LITERALS
    SYMBOL pp_timer_process
$4 L:
  A1 32 ENTRY,  A11 0 MOVI.N,  A10 5 MOVI.N,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$13 L:  END-CODE

CODE pm_active_timeout   \ .text.pm_active_timeout $4, static, without symbol
  $0 1 LITERALS
    SYMBOL pp_timer_process
$4 L:
  A1 32 ENTRY,  A11 0 MOVI.N,  A10 3 MOVI.N,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$13 L:  END-CODE

CODE pm_dream_timeout   \ .text.pm_dream_timeout $4, static, without symbol
  $0 1 LITERALS
    SYMBOL pp_timer_process
$4 L:
  A1 32 ENTRY,  A11 0 MOVI.N,  A10 2 MOVI.N,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$13 L:  END-CODE

CODE pm_tbtt_timeout   \ .text.pm_tbtt_timeout $4, static, without symbol
  $0 1 LITERALS
    SYMBOL pp_timer_process
$4 L:
  A1 32 ENTRY,  A11 0 MOVI.N,  A10 1 MOVI.N,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$13 L:  END-CODE

PUBLIC
CODE wifi_apb80m_request   \ .text.wifi_apb80m_request $0
  $0 ORIGIN
  A1 32 ENTRY,  RETW.N,
$5 L:  END-CODE

CODE wifi_apb80m_release   \ .text.wifi_apb80m_release $0
  $0 ORIGIN
  A1 32 ENTRY,  RETW.N,
$5 L:  END-CODE

CODE pm_is_waked   \ .wifislprxiram.2 $4
  $0 1 LITERALS
    SYMBOL g_pm
$4 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A8 0 MOVI.N,  A9 A2 1 L8UI,  A2 1 MOVI.N,
  A2 A8 A9 MOVNEZ,  RETW.N,
$16 L:  END-CODE

CODE pm_is_dream   \ .text.pm_is_dream $4
  $0 1 LITERALS
    SYMBOL g_pm
$4 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A9 0 MOVI.N,  A8 A2 1 L8UI,  A2 1 MOVI.N,
  A8 A8 -1 ADDI.N,  A2 A9 A8 MOVNEZ,  RETW.N,
$18 L:  END-CODE

CODE pm_is_sleeping   \ .text.pm_is_sleeping $4
  $0 1 LITERALS
    SYMBOL g_pm
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 1 MOVI.N,  A9 A8 1 L8UI,  A9 2 $20 L BEQI,
  A8 A8 $100 ADDMI,  A9 0 MOVI.N,  A8 A8 29 L8UI,  A8 A8 -3 ADDI,  A2 A9 A8 MOVNEZ,
$20 L:
  RETW.N,
$22 L:  END-CODE

CODE pm_is_open   \ .text.pm_is_open $4
  $0 1 LITERALS
    SYMBOL g_pm
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A8 13 L8UI,  RETW.N,
$F L:  END-CODE

CODE pm_sleep_for   \ .wifislprxiram.3 $1C
  $0 7 LITERALS
    SYMBOL g_mesh_is_started  SYMBOL g_mesh_is_root  SYMBOL g_mesh_init_ps_type
    $3 SYMBOL+ g_pm  SYMBOL g_pm  SYMBOL memcmp  SYMBOL memcmp
$1C L:
  A1 32 ENTRY,  A10 A2 MOV.N,  A2 $0 L L32R,  A2 A2 0 L8UI,  A2 $55 L BEQZ.N,
  A2 $4 L L32R,  MEMW,  A3 A2 0 L8UI,  A3 A3 0 8 EXTUI,  A3 $55 L BNEZ.N,
  A2 $8 L L32R,  A2 A2 0 L32I.N,  A2 $55 L BEQZ.N,
  A12 6 MOVI.N,  A2 1 MOVI,  A11 $C L L32R,  A8 $14 L L32R,  A8 CALLX8,
  A2 A3 A10 MOVNEZ,  $72 L J,
  3 ZEROS,
$55 L:
  A2 $10 L L32R,  A3 0 MOVI.N,  A2 A2 13 L8UI,  A2 A3 $70 L BEQ,
  A12 6 MOVI.N,  A2 1 MOVI.N,  A11 $C L L32R,  A8 $18 L L32R,  A8 CALLX8,
  A3 A2 A10 MOVEQZ,
$70 L:
  A2 A3 MOV.N,
$72 L:
  RETW.N,
$74 L:  END-CODE

CODE pm_allow_tx   \ .text.pm_allow_tx $10
  $0 4 LITERALS
    SYMBOL g_pm  SYMBOL g_mesh_is_started  SYMBOL g_mesh_is_root
    SYMBOL g_mesh_init_ps_type
$10 L:
  A1 32 ENTRY,  A11 A2 0 8 EXTUI,  A2 $4 L L32R,  A9 $0 L L32R,  A2 A2 0 L8UI,
  A10 A9 1 L8UI,  A2 $59 L BEQZ.N,
  A12 $8 L L32R,  MEMW,  A8 A12 0 L8UI,  A8 A8 0 8 EXTUI,  A8 $59 L BNEZ.N,
  A2 $C L L32R,  A2 A2 0 L32I.N,  A2 $59 L BEQZ.N,
  A2 1 MOVI.N,  A10 $68 L BEQZ.N,
  MEMW,  A10 A12 0 L8UI,  A2 A8 MOV.N,  A10 A10 0 8 EXTUI,  A10 $68 L BEQZ.N,
  A8 A9 2 L8UI,  A9 1 MOVI.N,  A8 A8 A11 SUB,  A2 A9 A8 MOVNEZ,  $68 L J,
  1 ZEROS,
$59 L:
  A2 1 MOVI.N,  A10 $68 L BEQZ.N,
  A8 A9 2 L8UI,  A9 0 MOVI.N,  A8 A8 A11 SUB,  A2 A9 A8 MOVEQZ,
$68 L:
  RETW.N,
$6A L:  END-CODE

CODE pm_allow_to_enable_bcn_filter   \ .text.pm_allow_to_enable_bcn_filter $0
  $0 ORIGIN
  A1 32 ENTRY,  A2 0 MOVI.N,  RETW.N,
$7 L:  END-CODE

CODE pm_get_connectionless_status   \ .wifislpiram.4 $4
  $0 1 LITERALS
    SYMBOL g_pm
$4 L:
  A1 32 ENTRY,  A10 $0 L L32R,  A8 A10 $160 L32I,  A2 A8 MOV.N,  A8 $29 L BEQZ.N,
  A9 A8 0 L32I.N,  A2 A9 MOV.N,  A9 $29 L BEQZ.N,
  A2 -2 MOVI.N,  A8 A10 $18A L16UI,  A8 $29 L BEQZ.N,
  A2 A9 0 L16UI,  A8 A8 A2 SUB,  A8 6 $29 L BGEI,
  A2 -1 MOVI.N,
$29 L:
  RETW.N,
$2B L:  END-CODE

CODE pm_parse_beacon_trace   \ .iram1.7 $0
  $0 ORIGIN
  A1 32 ENTRY,  RETW.N,
$5 L:  END-CODE

CODE pm_mac_sleep_init   \ .text.pm_mac_sleep_init $4
  $0 1 LITERALS
    SYMBOL wdev_mac_sleep_init
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$F L:  END-CODE

CODE pm_mac_sleep_deinit   \ .text.pm_mac_sleep_deinit $4
  $0 1 LITERALS
    SYMBOL wdev_mac_sleep_deinit
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$F L:  END-CODE

CODE pm_enable_max_idle_timer   \ .text.pm_enable_max_idle_timer $10
  $0 4 LITERALS
    SYMBOL g_pm_cfg  SYMBOL g_pm  $1EC SYMBOL+ g_pm  SYMBOL g_osi_funcs_p
$10 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A3 A2 29 L8UI,  A3 $44 L BEQZ.N,
  A3 $4 L L32R,  A3 A3 36 L32I.N,  A3 $44 L BNEZ.N,
  A4 $C L L32R,  A8 A4 0 L32I.N,  A5 A2 48 L16UI,  A2 $8 L L32R,  A10 A2 MOV.N,
  A8 A8 236 L32I,  A8 CALLX8,
  A12 A3 MOV.N,  A11 A5 9 SLLI,  A4 A4 0 L32I.N,  A10 A2 MOV.N,  A4 A4 232 L32I,
  A4 CALLX8,
$44 L:
  RETW.N,
$46 L:  END-CODE

CODE pm_is_sending_keep_alive   \ .text.pm_is_sending_keep_alive $4
  $0 1 LITERALS
    SYMBOL g_pm
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A8 A8 $100 ADDMI,  A2 A8 232 L8UI,  RETW.N,
$12 L:  END-CODE

CODE pm_set_bss_max_idle   \ .text.pm_set_bss_max_idle $C
  $0 3 LITERALS
    SYMBOL g_pm_cfg  SYMBOL pm_enable_max_idle_timer
    SYMBOL pm_disable_max_idle_timer
$C L:
  A1 32 ENTRY,  A2 A2 0 16 EXTUI,  A8 $0 L L32R,  A2 $41 L BEQZ.N,
  A9 1 MOVI.N,  A9 A8 29 S8I,  MEMW,  A2 5 $2D L BLTUI,
  A9 A2 -4 ADDI,  A9 A9 A9 ADD.N,  A2 A9 A2 MAX,  A2 A2 0 16 EXTUI,
$2D L:
  A2 A8 48 S16I,  MEMW,  MEMW,  A8 $4 L L32R,  A8 CALLX8,
  $50 L J,
  2 ZEROS,
$41 L:
  A2 A8 29 S8I,  MEMW,  MEMW,  A8 $8 L L32R,  A8 CALLX8,
$50 L:
  RETW.N,
$52 L:  END-CODE

CODE pm_noise_check_disable   \ .text.pm_noise_check_disable $4
  $0 1 LITERALS
    SYMBOL g_pm
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A9 0 MOVI.N,  A9 A8 21 S8I,  MEMW,  MEMW,  RETW.N,
$17 L:  END-CODE

CODE pm_noise_check_enable   \ .text.pm_noise_check_enable $4
  $0 1 LITERALS
    SYMBOL g_pm
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A9 1 MOVI.N,  A9 A8 21 S8I,  MEMW,  MEMW,  RETW.N,
$17 L:  END-CODE

CODE pm_enable_active_timer   \ .wifirxiram.19 $20
  $0 8 LITERALS
    $2DC6C0 T,  SYMBOL g_pm_cfg  SYMBOL g_pm  $CC SYMBOL+ g_pm
    SYMBOL g_osi_funcs_p  SYMBOL g_mesh_is_started  SYMBOL g_mesh_is_root
    SYMBOL g_mesh_init_ps_type
$20 L:
  A1 32 ENTRY,  A5 $4 L L32R,  A2 A5 29 L8UI,  A3 $8 L L32R,  A2 $34 L BEQZ.N,
  A2 A3 36 L32I,  A2 $9C L BEQZ,
$34 L:
  A4 $10 L L32R,  A8 A4 0 L32I.N,  A2 $C L L32R,  A10 A2 A2 OR,  A8 A8 236 L32I,
  A8 CALLX8,
  A9 A3 36 L32I.N,  A8 $14 L L32R,  A9 $74 L BNEZ.N,
  A3 A8 0 L8UI,  A11 A5 16 L32I.N,  A3 $64 L BEQZ.N,
  A3 $18 L L32R,  MEMW,  A3 A3 0 L8UI,  A3 A3 0 8 EXTUI,  A3 $64 L BNEZ.N,
  A11 $0 L L32R,
$64 L:
  A12 0 MOVI.N,  A10 A2 MOV.N,  A3 A4 0 L32I.N,  A3 A3 248 L32I,  A3 CALLX8,
  $9C L J,
  1 ZEROS,
$74 L:
  A5 A8 0 L8UI,  A5 $8E L BEQZ.N,
  A5 $18 L L32R,  MEMW,  A8 A5 0 L8UI,  A8 A8 0 8 EXTUI,  A8 $8E L BNEZ.N,
  A5 $1C L L32R,  A5 A5 0 L32I.N,  A5 $9C L BNEZ.N,
$8E L:
  A12 0 MOVI.N,  A10 A2 MOV.N,  A4 A4 0 L32I.N,  A11 A3 48 L32I.N,  A4 A4 248 L32I,
  A4 CALLX8,
$9C L:
  RETW.N,
$9E L:  END-CODE

PRIVATE
CODE pm_reset_keep_alive   \ .text.pm_reset_keep_alive $C, static, without symbol
  $0 3 LITERALS
    SYMBOL g_pm  SYMBOL g_osi_funcs_p  SYMBOL pm_enable_active_timer
$C L:
  A1 32 ENTRY,  A2 $0 L L32R,  A3 A2 13 L8UI,  A3 $40 L BEQZ.N,
  A3 $4 L L32R,  A8 A3 0 L32I.N,  A10 A8 $110 L32I,  A10 CALLX8,
  A10 A2 72 S32I,  A11 A2 76 S32I,  A2 A2 36 L32I.N,  A2 $40 L BNEZ.N,
  A2 A3 0 L32I,  A10 A2 $194 L32I,  MEMW,  A10 CALLX8,
  A10 $40 L BNEZ.N,
  A8 $8 L L32R,  A8 CALLX8,
$40 L:
  RETW.N,
$42 L:  END-CODE

PUBLIC
CODE pm_disable_dream_timer   \ .wifislpiram.20 $8
  $0 2 LITERALS
    $B8 SYMBOL+ g_pm  SYMBOL g_osi_funcs_p
$8 L:
  A1 32 ENTRY,  A8 $4 L L32R,  A8 A8 0 L32I.N,  A10 $0 L L32R,  A8 A8 236 L32I,
  A8 CALLX8,
  RETW.N,
$1B L:  END-CODE

CODE pm_enable_dream_timer   \ .wifislpiram.21 $C
  $0 3 LITERALS
    $B8 SYMBOL+ g_pm  SYMBOL g_osi_funcs_p  SYMBOL g_pm
$C L:
  A1 32 ENTRY,  A3 $4 L L32R,  A8 A3 0 L32I,  A2 $0 L L32R,  A10 A2 MOV.N,
  A8 A8 236 L32I,  A8 CALLX8,
  A12 0 MOVI.N,  A10 A2 MOV.N,  A8 $8 L L32R,  A3 A3 0 L32I.N,  A11 A8 $1C4 L32I,
  A3 A3 248 L32I,  A3 CALLX8,
  RETW.N,
$34 L:  END-CODE

CODE pm_disable_sleep_delay_timer   \ .wifislprxiram.22 $C
  $0 3 LITERALS
    SYMBOL g_pm  $108 SYMBOL+ g_pm  SYMBOL g_osi_funcs_p
$C L:
  A1 32 ENTRY,  A8 $0 L L32R,  A9 0 MOVI.N,  A8 A8 $100 ADDMI,  A9 A8 184 S8I,
  MEMW,  A8 $8 L L32R,  A8 A8 0 L32I.N,  A10 $4 L L32R,  A8 A8 236 L32I,  MEMW,
  A8 CALLX8,
  RETW.N,
$30 L:  END-CODE

CODE pm_enable_sleep_delay_timer   \ .text.pm_enable_sleep_delay_timer $C
  $0 3 LITERALS
    $108 SYMBOL+ g_pm  SYMBOL g_osi_funcs_p  SYMBOL g_pm
$C L:
  A1 32 ENTRY,  A4 $4 L L32R,  A2 A4 0 L32I,  A3 $0 L L32R,  A10 A3 MOV.N,
  A2 A2 236 L32I,  A2 CALLX8,
  A10 A3 MOV.N,  A12 0 MOVI.N,  A2 $8 L L32R,  A3 1 MOVI.N,  A4 A4 0 L32I,
  A11 A2 56 L32I,  A2 A2 $100 ADDMI,  A4 A4 248 L32I,  A4 CALLX8,
  A3 A2 184 S8I,  MEMW,  MEMW,  RETW.N,
$43 L:  END-CODE

CODE pm_disable_disconnected_sl~DBBF   \ pm_disable_disconnected_sleep_delay_timer: .text.pm_disable_disconnected_sleep_delay_timer $C
  $0 3 LITERALS
    SYMBOL g_pm  $124 SYMBOL+ g_pm  SYMBOL g_osi_funcs_p
$C L:
  A1 32 ENTRY,  A8 $0 L L32R,  A9 0 MOVI.N,  A8 A8 $100 ADDMI,  A9 A8 184 S8I,
  MEMW,  A8 $8 L L32R,  A8 A8 0 L32I.N,  A10 $4 L L32R,  A8 A8 236 L32I,  MEMW,
  A8 CALLX8,
  RETW.N,
$30 L:  END-CODE

CODE pm_enable_disconnected_sle~30EA   \ pm_enable_disconnected_sleep_delay_timer: .text.pm_enable_disconnected_sleep_delay_timer $10
  $0 4 LITERALS
    SYMBOL g_pm  SYMBOL g_mesh_is_started  $124 SYMBOL+ g_pm  SYMBOL g_osi_funcs_p
$10 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A2 A2 $100 ADDMI,  A3 A2 29 L8UI,  A3 2 $52 L BNEI,
  A3 $4 L L32R,  A4 A3 0 L8UI,  A4 $52 L BNEZ,
  A5 $C L L32R,  A8 A5 0 L32I,  A3 $8 L L32R,  A10 A3 MOV.N,  A8 A8 236 L32I,
  A8 CALLX8,
  A10 A3 MOV.N,  A12 A4 MOV.N,  A5 A5 0 L32I,  A11 $3E8 MOVI,  A3 1 MOVI,
  A5 A5 248 L32I,  A5 CALLX8,
  A3 A2 184 S8I,  MEMW,
$52 L:
  MEMW,  RETW.N,
$57 L:  END-CODE

CODE pm_noise_check   \ .text.pm_noise_check $10
  $0 4 LITERALS
    SYMBOL g_pm  SYMBOL wDevCtrl  SYMBOL noise_check_loop
    SYMBOL read_hw_noisefloor
$10 L:
  A1 32 ENTRY,  A9 $0 L L32R,  A2 -2 MOVI.N,  A8 A9 21 L8UI,  A8 $70 L BEQZ,
  A2 A9 36 L32I.N,  A8 A9 44 L32I.N,  A2 $36 L BEQZ.N,
  A2 1 $2D L BNEI,
  A2 A9 11 L8UI,  $33 L J,
$2D L:
  A2 2 $36 L BNEI,
  A2 A9 32 L16UI,
$33 L:
  A8 A2 A8 MULL,
$36 L:
  A2 A9 28 L16UI,  A9 1 MOVI.N,  A9 A9 18 SLLI,  A9 A8 A9 ADD.N,  A9 A9 A8 QUOS,
  A2 A2 A9 REMS,  A2 $6E L BNEZ.N,
  A11 1 MOVI,  A10 A11 A11 OR,  A8 $8 L L32R,  A8 CALLX8,
  A8 $C L L32R,  A8 CALLX8,
  A8 $4 L L32R,  A10 A10 2 ADDI.N,  A10 A10 2 SRAI,  A10 A8 47 S8I,  MEMW,  MEMW,
  $70 L J,
$6E L:
  A2 0 MOVI.N,
$70 L:
  RETW.N,
$72 L:  END-CODE

CODE pm_pend_noise_check   \ .text.pm_pend_noise_check $0
  $0 ORIGIN
  A1 32 ENTRY,  RETW.N,
$5 L:  END-CODE

CODE pm_noise_check_process   \ .text.pm_noise_check_process $0
  $0 ORIGIN
  A1 32 ENTRY,  RETW.N,
$5 L:  END-CODE

CODE pm_register_pm_tx_null_cb   \ .text.pm_register_pm_tx_null_cb $4
  $0 1 LITERALS
    SYMBOL bss.g_pm_tx_null_func
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A8 0 S32I.N,  A2 1 MOVI.N,  MEMW,  RETW.N,
$13 L:  END-CODE

CODE pm_send_nullfunc   \ .text.pm_send_nullfunc $C
  $0 3 LITERALS
    SYMBOL g_pm  SYMBOL bss.g_pm_tx_null_func  SYMBOL wifi_gpio_debug
$C L:
  A1 32 ENTRY,  A3 $0 L L32R,  A2 A2 0 8 EXTUI,  A8 1 MOVI.N,  A5 A3 20 L8UI,
  A4 A3 19 L8UI,  A2 $30 L BEQZ.N,
  A8 A3 20 S8I,  A8 0 MOVI.N,  A8 A3 19 S8I,  MEMW,  MEMW,  $39 L J,
$30 L:
  A8 A3 19 S8I,  A2 A3 20 S8I,  MEMW,
$39 L:
  A11 A2 2 ADDI.N,  A10 6 MOVI.N,  MEMW,  A8 $8 L L32R,  A8 CALLX8,
  A12 0 MOVI.N,  A8 $4 L L32R,  A11 A12 MOV.N,  A10 A2 MOV.N,  A8 A8 0 L32I,
  A8 CALLX8,
  A10 $60 L BNEZ.N,
  A5 A3 20 S8I,  A4 A3 19 S8I,  MEMW,
$60 L:
  MEMW,  RETW.N,
$65 L:  END-CODE

CODE pm_mac_wakeup   \ .text.pm_mac_wakeup $4
  $0 1 LITERALS
    SYMBOL wdev_mac_wakeup
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$F L:  END-CODE

CODE pm_mac_sleep   \ .text.pm_mac_sleep $4
  $0 1 LITERALS
    SYMBOL wdev_mac_sleep
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$F L:  END-CODE

CODE pm_get_tbtt_count   \ .wifislprxiram.24 $10
  $0 4 LITERALS
    SYMBOL g_osi_funcs_p  SYMBOL g_pm  SYMBOL __udivdi3
    SYMBOL pm_coex_schm_overall_period_get
$10 L:
  A1 32 ENTRY,  A5 $0 L L32R,  A4 A5 0 L32I.N,  A10 A4 $110 L32I,  A10 CALLX8,
  A8 1 MOVI.N,  A4 $4 L L32R,  A12 A4 44 L32I.N,  A9 A4 88 L32I,  A9 A12 A9 ADD.N,
  A6 A4 92 L32I,  A9 A12 $32 L BLTU,
  A8 0 MOVI.N,
$32 L:
  A13 A8 A6 ADD.N,  A8 1 MOVI.N,  A6 A4 40 L32I.N,  A10 A6 A10 ADD.N,
  A10 A6 $40 L BLTU,
  A8 0 MOVI,
$40 L:
  A10 A9 A10 SUB,  A8 A8 A11 ADD.N,  A11 1 MOVI.N,  A9 A10 $4C L BLTU,
  A11 0 MOVI.N,
$4C L:
  A8 A13 A8 SUB,  A11 A8 A11 SUB,  A13 0 MOVI.N,  A8 $8 L L32R,  A8 CALLX8,
  A5 A5 0 L32I.N,  A6 A10 A10 OR,  A10 A5 $194 L32I,  A10 CALLX8,
  A10 $78 L BEQZ.N,
  A10 1 MOVI.N,  A4 A4 14 L8UI,  A4 $8C L BEQZ.N,
  A8 $C L L32R,  A8 CALLX8,
  $8C L J,
  1 ZEROS,
$78 L:
  A5 A4 36 L32I.N,  A5 1 $84 L BNEI,
  A10 A4 11 L8UI,  $8C L J,
  1 ZEROS,
$84 L:
  A10 1 MOVI.N,  A5 2 $8C L BNEI,
  A10 A4 32 L16UI,
$8C L:
  A10 A3 0 S32I.N,  A3 0 MOVI.N,  A10 A6 $99 L BLTU,
  A3 A10 A6 SUB,  A3 A3 A10 REMU,
$99 L:
  A3 A2 0 S32I.N,  MEMW,  RETW.N,
$A0 L:  END-CODE

CODE pm_save_tbtt_info   \ .wifislprxiram.25 $4
  $0 1 LITERALS
    SYMBOL pm_get_tbtt_count
$4 L:
  A1 48 ENTRY,  A11 A1 MOV.N,  A10 A1 4 ADDI.N,  A8 $0 L L32R,  A8 CALLX8,
  A8 A1 4 L32I.N,  A2 A2 $100 ADDMI,  A8 A2 214 S8I,  MEMW,  A8 A1 0 L32I.N,
  A8 A2 215 S8I,  MEMW,  MEMW,  RETW.N,
$29 L:  END-CODE

CODE pm_beacon_miss_exceeded_wa~D8EB   \ pm_beacon_miss_exceeded_wakeup_disabled: .wifislprxiram.26 $8
  $0 2 LITERALS
    SYMBOL g_pm_cfg  SYMBOL g_pm
$8 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A8 A2 51 L8UI,  A2 1 MOVI.N,  A8 2 $24 L BEQI,
  A2 0 MOVI.N,  A8 1 $24 L BNEI,
  A2 $4 L L32R,  A2 A2 $200 ADDMI,  A2 A2 28 L8UI,
$24 L:
  A2 A2 0 1 EXTUI,  RETW.N,
$29 L:  END-CODE

CODE pm_beacon_monitor_tbtt_allowed   \ .wifislprxiram.27 $8
  $0 2 LITERALS
    SYMBOL ptr_beacon_offset_funcs  SYMBOL pm_beacon_miss_exceeded_wa~D8EB
$8 L:
  A1 32 ENTRY,  A8 $4 L L32R,  A8 CALLX8,
  A8 A2 $100 ADDMI,  A3 1 MOVI.N,  A10 $25 L BEQZ.N,
  A9 A2 $182 L16SI,  A9 A9 -1 ADDI.N,  A10 A8 214 L8UI,  A10 A9 $25 L BLT,
  A3 0 MOVI.N,
$25 L:
  A9 A8 100 L8UI,  A9 $31 L BNEZ.N,
$2A L:
  A3 0 MOVI.N,  $58 L J,
  2 ZEROS,
$31 L:
  A9 A8 212 L8UI,  A9 $2A L BNEZ,
  A2 A2 13 L8UI,  A2 $2A L BEQZ,
  A2 A8 214 L8UI,  A8 A8 215 L8UI,  A8 A8 -1 ADDI.N,  A2 A8 $2A L BEQ,
  A2 $0 L L32R,  A2 A2 0 L32I.N,  A10 A2 12 L32I.N,  A10 $58 L BEQZ,
  A10 CALLX8,
  A10 $2A L BNEZ,
$58 L:
  A2 A3 0 1 EXTUI,  RETW.N,
$5D L:  END-CODE

CODE pm_beacon_monitor_tbtt_start   \ .wifislprxiram.28 $14
  $0 5 LITERALS
    SYMBOL ptr_beacon_offset_funcs  SYMBOL g_osi_funcs_p  $1387 T,  SYMBOL g_pm
    SYMBOL __udivdi3
$14 L:
  A1 48 ENTRY,  A3 $0 L L32R,  A3 A3 0 L32I.N,  A10 A3 16 L32I.N,  A10 $35 L BNEZ.N,
$20 L:
  A3 A2 $100 ADDMI,  A5 A3 126 L8UI,  A5 A5 5 SLLI,  A3 A2 $180 L16SI,
  A5 A5 A3 MULL,  $3D L J,
  3 ZEROS,
$35 L:
  A10 CALLX8,
  A10 $20 L BEQZ,
  A5 0 MOVI.N,
$3D L:
  A6 $4 L L32R,  A3 A6 0 L32I,  A10 A3 $110 L32I,  A10 CALLX8,
  A8 A10 MOV.N,  A7 A11 A11 OR,  A4 A2 92 L32I,  A3 A2 88 L32I,  A11 A4 $5D L BLTU,
  A4 A11 $A4 L BNE,
  A10 A3 $A4 L BGEU,
$5D L:
  A10 A3 A10 SUB,  A9 A2 44 L32I.N,  A14 1 MOVI.N,  A3 A10 $69 L BLTU,
  A14 0 MOVI.N,
$69 L:
  A11 A4 A11 SUB,  A12 A9 MOV.N,  A13 0 MOVI.N,  A11 A11 A14 SUB,  A8 A1 4 S32I,
  A9 A1 0 S32I.N,  MEMW,  A8 $10 L L32R,  A8 CALLX8,
  A9 A1 0 L32I.N,  A12 A10 A9 MULL,  A11 A11 A9 MULL,  A10 A10 A9 MULUH,
  A9 A3 A12 SUB,  A10 A11 A10 ADD.N,  A8 A1 4 L32I.N,  A11 1 MOVI,
  A3 A9 $9C L BLTU,
  A11 0 MOVI,
$9C L:
  A10 A4 A10 SUB,  A3 A9 MOV.N,  A4 A10 A11 SUB,
$A4 L:
  A13 $8 L L32R,  A10 1 MOVI.N,  A11 0 MOVI.N,  $BD L J,
$AE L:
  A9 A2 44 L32I.N,  A12 A10 MOV.N,  A9 A3 A9 ADD.N,  A9 A3 $B9 L BLTU,
  A12 A11 MOV.N,
$B9 L:
  A3 A9 MOV.N,  A4 A12 A4 ADD.N,
$BD L:
  A7 A4 $C6 L BLTU,
  A4 A7 $AE L BNE,
  A8 A3 $AE L BGEU,
$C6 L:
  A9 A3 A8 SUB,  A12 A10 MOV.N,  A3 A9 $D0 L BLTU,
  A12 A11 MOV.N,
$D0 L:
  A14 A4 A7 SUB,  A14 A12 $D9 L BNE,
  A13 A9 $AE L BGEU,
$D9 L:
  A4 A2 $182 L16SI,  A3 A3 A5 SUB,  A4 A4 -1 ADDI.N,  A5 A2 $180 L16SI,
  A5 A4 $F0 L BNE,
  A4 $C L L32R,  A4 A4 $1CC L32I,  A3 A3 A4 SUB,
$F0 L:
  A4 A6 0 L32I.N,  A10 A4 $110 L32I,  A2 A2 $184 L32I,  A5 A4 248 L32I,  A10 CALLX8,
  A11 A3 A10 SUB,  A12 0 MOVI.N,  A10 A2 MOV.N,  A5 CALLX8,
  RETW.N,
$10A L:  END-CODE

CODE pm_beacon_monitor_tbtt_stop   \ .wifislprxiram.29 $4
  $0 1 LITERALS
    SYMBOL g_osi_funcs_p
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A8 A8 0 L32I.N,  A10 A2 $184 L32I,  A8 A8 236 L32I,
  A8 CALLX8,
  RETW.N,
$17 L:  END-CODE

CODE pm_disable_beacon_monitor_timer   \ .wifislprxiram.31 $8
  $0 2 LITERALS
    SYMBOL g_osi_funcs_p  SYMBOL pm_beacon_monitor_tbtt_stop
$8 L:
  A1 32 ENTRY,  A3 A2 $100 ADDMI,  A4 A3 100 L8UI,  A4 $3E L BEQZ.N,
  A4 A3 212 L8UI,  A4 $3E L BNEZ.N,
  A8 A2 13 L8UI,  A8 $3E L BEQZ.N,
  A10 $168 MOVI,  A10 A2 A10 ADD.N,  A8 $0 L L32R,  A8 A8 0 L32I.N,  A8 A8 236 L32I,
  A8 CALLX8,
  A10 A2 MOV.N,  A8 $4 L L32R,  A8 CALLX8,
  A4 A2 $180 S16I,  A4 A3 125 S8I,  MEMW,
$3E L:
  MEMW,  RETW.N,
$43 L:  END-CODE

CODE pm_beacon_monitor_init   \ .text.pm_beacon_monitor_init $14
  $0 5 LITERALS
    SYMBOL g_pm_cfg  SYMBOL g_pm  SYMBOL bss.beacon_monitor_data$0
    SYMBOL pm_beacon_monitor_tbtt_timeout  SYMBOL g_osi_funcs_p
$14 L:
  A1 32 ENTRY,  A9 $0 L L32R,  A12 $4 L L32R,  A8 A12 $100 ADDMI,  A10 A9 36 L8UI,
  A10 A8 100 S8I,  MEMW,  A10 $51 L BEQZ.N,
  A10 A9 37 L8UI,  A10 A8 124 S8I,  A10 0 MOVI.N,  A10 A8 125 S8I,  MEMW,
  A10 A9 38 L8UI,  A10 A12 $182 S16I,  MEMW,  A10 A9 39 L8UI,  A9 A9 40 L8UI,
  A10 A8 126 S8I,  A9 A8 127 S8I,  MEMW,
$51 L:
  A8 $10 L L32R,  A8 A8 0 L32I.N,  A10 $8 L L32R,  A8 A8 244 L32I,  A11 $C L L32R,
  A10 A12 $184 S32I,  MEMW,  A8 CALLX8,
  RETW.N,
$6A L:  END-CODE

CODE pm_dream   \ .wifislprxiram.32 $2C
  $0 11 LITERALS
    SYMBOL g_pm  SYMBOL g_mesh_is_started  SYMBOL g_mesh_is_root
    SYMBOL g_mesh_init_ps_type  SYMBOL g_osi_funcs_p  SYMBOL g_pm_cnt
    $F4 SYMBOL+ g_pm  SYMBOL wifi_rf_phy_enable  SYMBOL esp_wifi_get_mode
    SYMBOL wifi_rf_phy_enable  SYMBOL pm_set_state
$2C L:
  A1 48 ENTRY,  A2 $0 L L32R,  A3 A2 1 L8UI,  A3 2 $3B L BEQI,
  $F9 L J,
$3B L:
  A10 0 MOVI,  A8 $1C L L32R,  A8 CALLX8,
  A3 $4 L L32R,  A3 A3 0 L8UI,  A3 $7D L BEQZ.N,
  A3 $8 L L32R,  MEMW,  A3 A3 0 L8UI,  A3 A3 0 8 EXTUI,  A3 $7D L BNEZ.N,
  A4 $C L L32R,  A4 A4 0 L32I.N,  A4 $7D L BEQZ.N,
  A10 A1 MOV.N,  A3 A1 0 S32I.N,  MEMW,  A8 $20 L L32R,  A8 CALLX8,
  A10 $7D L BNEZ.N,
  A3 A1 0 L32I.N,  A3 3 $7D L BNEI,
  A10 1 MOVI.N,  A8 $24 L L32R,  A8 CALLX8,
$7D L:
  A3 $10 L L32R,  A3 A3 0 L32I.N,  A10 A3 $110 L32I,  A10 CALLX8,
  A5 A10 MOV.N,  A8 $14 L L32R,  A3 A8 44 L32I.N,  A3 A3 1 ADDI.N,  A3 A8 44 S32I.N,
  A3 A2 112 L32I,  A10 A2 120 S32I,  A10 A10 A3 SUB,  A3 1 MOVI.N,  A11 A2 124 S32I,
  A4 A2 116 L32I,  A5 A10 $A9 L BLTU,
  A3 0 MOVI.N,
$A9 L:
  A9 A11 A4 SUB,  A9 A9 A3 SUB,  A5 A8 48 L32I.N,  A3 1 MOVI.N,  A4 A5 A10 ADD.N,
  A11 A8 52 L32I.N,  A4 A5 $BD L BLTU,
  A3 0 MOVI,
$BD L:
  A11 A11 A9 ADD.N,  A3 A3 A11 ADD.N,  A5 A8 64 L32I,  A4 A8 48 S32I.N,
  A4 A5 A10 ADD.N,  A3 A8 52 S32I.N,  A3 1 MOVI.N,  A11 A8 68 L32I,
  A4 A5 $D4 L BLTU,
  A3 0 MOVI.N,
$D4 L:
  A11 A11 A9 ADD.N,  A3 A3 A11 ADD.N,  A4 A8 64 S32I,  A4 A2 136 L32I,
  A10 A4 A10 ADD.N,  A3 A8 68 S32I,  A3 1 MOVI.N,  A8 A2 140 L32I,
  A10 A4 $EE L BLTU,
  A3 0 MOVI,
$EE L:
  A8 A8 A9 ADD.N,  A3 A3 A8 ADD,  A10 A2 136 S32I,  A3 A2 140 S32I,
$F9 L:
  A3 A2 $100 ADDMI,  A3 A3 213 L8UI,  A3 $159 L BEQZ,
  A3 $10 L L32R,  A4 A3 0 L32I,  A10 A4 $194 L32I,  MEMW,  A10 CALLX8,
  A10 $159 L BEQZ,
  A4 A3 0 L32I.N,  A10 A4 $110 L32I,  A10 CALLX8,
  A5 A10 MOV.N,  A4 A2 108 L32I,  A11 A4 $12D L BLTU,
  A4 A11 $151 L BNE,
  A4 A2 104 L32I,  A10 A4 $151 L BGEU,
$12D L:
  A8 A3 0 L32I.N,  A4 $18 L L32R,  A10 A4 MOV.N,  A8 A8 236 L32I,  A8 CALLX8,
  A12 0 MOVI.N,  A10 A4 MOV.N,  A11 A2 104 L32I,  A11 A11 A5 SUB,  A2 A3 0 L32I.N,
  A2 A2 248 L32I,  A2 CALLX8,
  $159 L J,
  2 ZEROS,
$151 L:
  A3 0 MOVI.N,  A3 A2 24 S8I,  MEMW,
$159 L:
  A10 1 MOVI.N,  MEMW,  A8 $28 L L32R,  A8 CALLX8,
  RETW.N,
$166 L:  END-CODE

CODE pm_disconnected_wake   \ .text.pm_disconnected_wake $1C
  $0 7 LITERALS
    SYMBOL g_pm  SYMBOL g_mesh_is_started  SYMBOL wifi_rf_phy_enable
    SYMBOL pm_set_state  SYMBOL wifi_gpio_debug  SYMBOL wifi_gpio_debug
    SYMBOL wifi_gpio_debug
$1C L:
  A1 32 ENTRY,  A3 $0 L L32R,  A3 A3 $100 ADDMI,  A4 A3 29 L8UI,  A4 3 $6E L BNEI,
  A2 $4 L L32R,  A2 A2 0 L8UI,  A2 $6E L BNEZ.N,
  A10 A2 A2 OR,  A8 $8 L L32R,  A8 CALLX8,
  A8 2 MOVI,  A10 A2 MOV.N,  A8 A3 29 S8I,  MEMW,  MEMW,  A8 $C L L32R,  A8 CALLX8,
  A11 1 MOVI.N,  A10 A4 MOV.N,  A8 $10 L L32R,  A8 CALLX8,
  A11 1 MOVI.N,  A10 4 MOVI.N,  A8 $14 L L32R,  A8 CALLX8,
  A11 A2 MOV.N,  A10 5 MOVI.N,  A8 $18 L L32R,  A8 CALLX8,
$6E L:
  RETW.N,
$70 L:  END-CODE

CODE pm_sleep   \ .wifislprxiram.33 $34
  $0 13 LITERALS
    SYMBOL g_pm  SYMBOL g_mesh_is_started  SYMBOL g_mesh_is_root
    SYMBOL g_mesh_init_ps_type  SYMBOL g_osi_funcs_p  $F4 SYMBOL+ g_pm
    SYMBOL g_pm_cnt  SYMBOL ppCheckTxQIdle  SYMBOL ppCheckTxIdle
    SYMBOL wifi_rf_phy_disable  SYMBOL esp_wifi_get_mode
    SYMBOL wifi_rf_phy_disable  SYMBOL pm_set_state
$34 L:
  A1 48 ENTRY,  A3 $4 L L32R,  A2 $0 L L32R,  A4 A3 0 L8UI,  A8 A2 16 L8UI,
  A4 $84 L BEQZ.N,
  A4 $8 L L32R,  MEMW,  A4 A4 0 L8UI,  A4 A4 0 8 EXTUI,  A4 $84 L BNEZ.N,
  A4 $C L L32R,  A4 A4 0 L32I.N,  A4 $84 L BEQZ.N,
  A8 $60 L BNEZ.N,
  $86 L J,
  1 ZEROS,
$60 L:
  A4 A2 18 L8UI,  A4 $86 L BNEZ.N,
  A4 A2 19 L8UI,  A4 $86 L BNEZ.N,
  A4 A2 20 L8UI,  A4 $86 L BNEZ.N,
  A4 A2 14 L8UI,  A4 $86 L BEQZ.N,
  A4 A2 $120 L32I,  A4 $86 L BNEZ.N,
  A10 1 MOVI.N,  A8 $1C L L32R,  A8 CALLX8,
  $BE L J,
$84 L:
  A8 $8C L BNEZ.N,
$86 L:
  A8 0 MOVI.N,  $DC L J,
  1 ZEROS,
$8C L:
  A4 A2 17 L8UI,  A4 $86 L BNEZ,
  A4 A2 18 L8UI,  A4 $86 L BNEZ,
  A4 A2 19 L8UI,  A4 $86 L BNEZ,
  A4 A2 20 L8UI,  A4 $86 L BNEZ,
  A4 A2 14 L8UI,  A4 $86 L BEQZ,
  A4 A2 15 L8UI,  A4 $86 L BNEZ,
  A4 A2 $120 L32I,  A4 $86 L BNEZ,
  A10 1 MOVI.N,  A8 $20 L L32R,  A8 CALLX8,
$BE L:
  A8 1 MOVI.N,  A10 $DC L BEQZ.N,
  A4 $10 L L32R,  A4 A4 0 L32I.N,  A10 A4 $194 L32I,  A10 CALLX8,
  A10 $86 L BEQZ,
  A4 1 MOVI,  A8 A2 24 L8UI,  A8 A8 A4 XOR,  A8 A8 0 8 EXTUI,
$DC L:
  A8 A8 0 1 EXTUI,  A8 $1DD L BEQZ,
  A4 $10 L L32R,  A8 A4 0 L32I.N,  A10 A8 $110 L32I,  A10 CALLX8,
  A8 A4 0 L32I,  A8 A8 $1A0 L32I,  A11 A2 116 S32I,  A10 A2 112 S32I,  A10 1 MOVI,
  MEMW,  A8 CALLX8,
  A10 2 MOVI.N,  A8 A4 0 L32I.N,  A8 A8 $1A0 L32I,  A8 CALLX8,
  A10 38 MOVI.N,  A8 A4 0 L32I.N,  A8 A8 $1A0 L32I,  A8 CALLX8,
  A10 0 MOVI.N,  A8 A4 0 L32I.N,  A8 A8 $1A0 L32I,  A8 CALLX8,
  A8 A2 $100 ADDMI,  A8 A8 213 L8UI,  A8 $13D L BEQZ.N,
  A8 A4 0 L32I.N,  A10 A8 $194 L32I,  A10 CALLX8,
  A10 $13D L BEQZ.N,
  A4 A4 0 L32I.N,  A10 $14 L L32R,  A4 A4 236 L32I,  A4 CALLX8,
$13D L:
  A10 0 MOVI.N,  A8 $24 L L32R,  A8 CALLX8,
  A3 A3 0 L8UI,  A3 $17D L BEQZ.N,
  A3 $8 L L32R,  MEMW,  A3 A3 0 L8UI,  A3 A3 0 8 EXTUI,  A3 $17D L BNEZ.N,
  A4 $C L L32R,  A4 A4 0 L32I.N,  A4 $17D L BEQZ.N,
  A10 A1 MOV.N,  A3 A1 0 S32I,  MEMW,  A8 $28 L L32R,  A8 CALLX8,
  A10 $17D L BNEZ.N,
  A3 A1 0 L32I.N,  A3 3 $17D L BNEI,
  A10 1 MOVI,  A8 $2C L L32R,  A8 CALLX8,
$17D L:
  A3 $18 L L32R,  A4 A3 40 L32I.N,  A4 A4 1 ADDI.N,  A8 A2 120 L32I,
  A9 A2 112 L32I,  A10 A2 124 L32I,  A4 A3 40 S32I.N,  A4 A2 116 L32I,
  A2 A9 A8 SUB,  A8 1 MOVI.N,  A9 A2 $19D L BLTU,
  A8 0 MOVI,
$19D L:
  A11 A3 56 L32I.N,  A4 A4 A10 SUB,  A10 A11 A2 ADD.N,  A4 A4 A8 SUB,  A8 1 MOVI.N,
  A9 A3 60 L32I.N,  A10 A11 $1B1 L BLTU,
  A8 0 MOVI,
$1B1 L:
  A9 A9 A4 ADD.N,  A8 A8 A9 ADD.N,  A10 A3 56 S32I.N,  A10 A3 64 L32I,
  A2 A10 A2 ADD.N,  A8 A3 60 S32I.N,  A8 1 MOVI.N,  A9 A3 68 L32I,
  A2 A10 $1C8 L BLTU,
  A8 0 MOVI.N,
$1C8 L:
  A4 A9 A4 ADD.N,  A8 A8 A4 ADD.N,  A10 2 MOVI.N,  A2 A3 64 S32I,  A8 A3 68 S32I,
  MEMW,  A8 $30 L L32R,  A8 CALLX8,
$1DD L:
  RETW.N,
$1DF L:  END-CODE

CODE pm_disconnected_sleep   \ .text.pm_disconnected_sleep $20
  $0 8 LITERALS
    SYMBOL g_pm  SYMBOL g_osi_funcs_p  SYMBOL g_mesh_is_started
    SYMBOL ppCheckTxIdle  SYMBOL wifi_rf_phy_disable  SYMBOL wifi_gpio_debug
    SYMBOL wifi_gpio_debug  SYMBOL wifi_gpio_debug
$20 L:
  A1 32 ENTRY,  A10 1 MOVI,  A8 $C L L32R,  A8 CALLX8,
  A10 $39 L BNEZ.N,
$2E L:
  A2 $0 L L32R,  A3 A2 $120 L32I,  A3 $54 L BEQZ.N,
  $AA L J,
$39 L:
  A2 $4 L L32R,  A2 A2 0 L32I.N,  A10 A2 $194 L32I,  A10 CALLX8,
  A10 $AA L BEQZ,
  A2 $0 L L32R,  A2 A2 24 L8UI,  A2 $AA L BNEZ,
  $2E L J,
  1 ZEROS,
$54 L:
  A3 $8 L L32R,  A3 A3 0 L8UI,  A3 $AA L BNEZ,
  A10 1 MOVI.N,  A2 A2 $100 ADDMI,  A4 $4 L L32R,  A8 A4 0 L32I,  A8 A8 $1A0 L32I,
  A8 CALLX8,
  A10 38 MOVI.N,  A4 A4 0 L32I,  A4 A4 $1A0 L32I,  A4 CALLX8,
  A10 A3 MOV.N,  A8 $10 L L32R,  A8 CALLX8,
  A11 A3 MOV.N,  A10 3 MOVI.N,  A4 3 MOVI.N,  A4 A2 29 S8I,  MEMW,  MEMW,
  A8 $14 L L32R,  A8 CALLX8,
  A11 1 MOVI.N,  A10 4 MOVI.N,  A8 $18 L L32R,  A8 CALLX8,
  A11 1 MOVI.N,  A10 5 MOVI.N,  A8 $1C L L32R,  A8 CALLX8,
$AA L:
  RETW.N,
$AC L:  END-CODE

CODE is_off_channel   \ .text.is_off_channel $4
  $0 1 LITERALS
    SYMBOL g_pm
$4 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A8 1 MOVI.N,  A2 A2 14 L8UI,  A2 A2 A8 XOR,
  A2 A2 0 8 EXTUI,  RETW.N,
$17 L:  END-CODE

CODE pm_enable_send_wake_null_o~C1AF   \ pm_enable_send_wake_null_on_channel: .text.pm_enable_send_wake_null_on_channel $4
  $0 1 LITERALS
    SYMBOL g_pm
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A9 1 MOVI.N,  A8 A8 $100 ADDMI,  A9 A8 28 S8I,  MEMW,
  MEMW,  RETW.N,
$1A L:  END-CODE

CODE pm_offchan_is_waiting   \ .text.pm_offchan_is_waiting $4
  $0 1 LITERALS
    SYMBOL bss.g_offchan_ready_cb
$4 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A8 0 MOVI.N,  A9 A2 0 L32I.N,  A2 1 MOVI.N,
  A2 A8 A9 MOVEQZ,  RETW.N,
$15 L:  END-CODE

CODE pm_offchan_execute_ready_cb   \ .text.pm_offchan_execute_ready_cb $10
  $0 4 LITERALS
    SYMBOL bss.g_offchan_ready_cb  SYMBOL rodata_wlog_debug.35
    SYMBOL bss.g_offchan_ready_ctx  SYMBOL wifi_log
$10 L:
  A1 32 ENTRY,  A3 $0 L L32R,  A14 A3 0 L32I.N,  A14 $42 L BEQZ.N,
  A11 1 MOVI.N,  A12 4 MOVI.N,  A13 $4 L L32R,  A11 A11 12 SLLI,  A10 6 MOVI.N,
  A8 $C L L32R,  A8 CALLX8,
  A9 0 MOVI.N,  A8 $8 L L32R,  A11 A2 0 8 EXTUI,  A12 A3 0 L32I.N,  A10 A8 0 L32I.N,
  A9 A3 0 S32I.N,  A9 A8 0 S32I.N,  MEMW,  A12 CALLX8,
$42 L:
  RETW.N,
$44 L:  END-CODE

CODE pm_wake_up   \ .text.pm_wake_up $14
  $0 5 LITERALS
    SYMBOL g_pm  SYMBOL pm_disable_sleep_delay_timer  SYMBOL pm_dream
    SYMBOL pm_disable_disconnected_sl~DBBF  SYMBOL pm_disconnected_wake
$14 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A8 A2 13 L8UI,  A8 $34 L BEQZ.N,
  A8 $4 L L32R,  A8 CALLX8,
  A8 A2 1 L8UI,  A8 2 $40 L BNEI,
  A8 $8 L L32R,  A8 CALLX8,
  $40 L J,
$34 L:
  A8 $C L L32R,  A8 CALLX8,
  A8 $10 L L32R,  A8 CALLX8,
$40 L:
  A8 A2 $120 L32I,  A8 A8 1 ADDI.N,  A8 A2 $120 S32I,  MEMW,  RETW.N,
$4D L:  END-CODE

CODE pm_send_probe_start   \ .text.pm_send_probe_start $C
  $0 3 LITERALS
    SYMBOL g_pm  SYMBOL pm_disable_active_timer  SYMBOL pm_wake_up
$C L:
  A1 32 ENTRY,  A2 $0 L L32R,  A8 A2 $100 ADDMI,  A8 A8 29 L8UI,  A8 2 $21 L BGEUI,
  A8 A2 13 L8UI,  A8 $3A L BEQZ,
$21 L:
  A8 A2 15 L8UI,  A8 $3A L BNEZ.N,
  A8 $4 L L32R,  A8 CALLX8,
  A8 $8 L L32R,  A8 CALLX8,
  A8 1 MOVI.N,  A8 A2 15 S8I,  MEMW,
$3A L:
  MEMW,  RETW.N,
$3F L:  END-CODE

CODE pm_off_channel   \ .text.pm_off_channel $24
  $0 9 LITERALS
    SYMBOL g_pm  SYMBOL g_osi_funcs_p  $3FF73CB8 T,
    SYMBOL pm_disable_active_timer  SYMBOL pm_wake_up  SYMBOL pm_dream
    SYMBOL pm_send_nullfunc  SYMBOL pm_dream  SYMBOL pm_wake_up
$24 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A3 A2 $100 ADDMI,  A4 A3 29 L8UI,  A4 2 $38 L BGEUI,
  A4 A2 13 L8UI,  A4 $3D L BEQZ.N,
$38 L:
  A4 A2 14 L8UI,  A4 $4B L BNEZ.N,
$3D L:
  A3 0 MOVI.N,  A3 A2 14 S8I,  MEMW,  MEMW,  $107 L J,
$4B L:
  A8 $C L L32R,  A8 CALLX8,
  A4 A2 13 L8UI,  A4 $69 L BNEZ.N,
  A8 $10 L L32R,  A8 CALLX8,
  A4 A2 14 S8I,  MEMW,  MEMW,  $107 L J,
  1 ZEROS,
$69 L:
  A5 A2 $200 ADDMI,  A6 1 MOVI.N,  A4 A2 1 L8UI,  A6 A5 30 S8I,  MEMW,
  A4 $C9 L BNEZ,
  A4 $4 L L32R,  A4 A4 0 L32I.N,  A10 A4 $194 L32I,  MEMW,  A10 CALLX8,
  A10 $8D L BEQZ.N,
  A6 A2 25 S8I,
$8D L:
  A4 1 MOVI.N,  A4 A3 28 S8I,  MEMW,  MEMW,  A8 $14 L L32R,  A8 CALLX8,
  A4 7 MOVI.N,  A4 A4 17 SLLI,  A8 $8 L L32R,  MEMW,  A3 A8 0 L32I.N,  A3 A3 A4 OR,
  MEMW,  A3 A8 0 S32I.N,
$B3 L:
  A3 A2 20 L8UI,  A3 $F6 L BNEZ.N,
  A10 1 MOVI,  MEMW,  A8 $18 L L32R,  A8 CALLX8,
  $F6 L J,
  2 ZEROS,
$C9 L:
  A4 1 $F0 L BNEI,
  A6 A2 19 L8UI,  A6 $F6 L BEQZ.N,
  A6 $4 L L32R,  A6 A6 0 L32I.N,  A10 A6 $194 L32I,  A10 CALLX8,
  A10 $E1 L BEQZ.N,
  A4 A2 25 S8I,
$E1 L:
  A4 1 MOVI.N,  A4 A3 28 S8I,  MEMW,  MEMW,  $B3 L J,
  1 ZEROS,
$F0 L:
  A8 $1C L L32R,  A8 CALLX8,
$F6 L:
  A3 0 MOVI.N,  A8 $20 L L32R,  A8 CALLX8,
  A3 A2 14 S8I,  A3 A5 30 S8I,  MEMW,
$107 L:
  MEMW,  RETW.N,
$10C L:  END-CODE

CODE pm_offchan_ready_or_defer   \ .text.pm_offchan_ready_or_defer $18
  $0 6 LITERALS
    SYMBOL g_pm  SYMBOL bss.g_offchan_ready_cb  SYMBOL bss.g_offchan_ready_ctx
    SYMBOL rodata_wlog_error.34  SYMBOL pm_off_channel  SYMBOL wifi_log
$18 L:
  A1 32 ENTRY,  A4 $0 L L32R,  A8 2 MOVI,  A9 A4 14 L8UI,  A9 $64 L BEQZ.N,
  A8 $10 L L32R,  A8 CALLX8,
  A8 0 MOVI.N,  A9 A4 13 L8UI,  A9 A8 $64 L BEQ,
  A9 A4 20 L8UI,  A9 A8 $4D L BEQ,
  A8 1 MOVI.N,  A4 $4 L L32R,  A2 A4 0 S32I.N,  A4 $8 L L32R,  A3 A4 0 S32I.N,
  MEMW,  $64 L J,
  1 ZEROS,
$4D L:
  A4 A4 1 L8UI,  A4 $64 L BNEZ.N,
  A12 1 MOVI.N,  A11 A12 12 SLLI,  A13 $C L L32R,  A10 6 MOVI.N,  A8 $14 L L32R,
  A8 CALLX8,
  A8 2 MOVI.N,
$64 L:
  A2 A8 MOV.N,  RETW.N,
$68 L:  END-CODE

CODE pm_wake_done   \ .text.pm_wake_done $C
  $0 3 LITERALS
    SYMBOL g_pm  SYMBOL pm_sleep  SYMBOL pm_disconnected_sleep
$C L:
  A1 32 ENTRY,  A8 $0 L L32R,  A9 A8 $120 L32I,  A9 $1C L BEQZ.N,
  A9 A9 -1 ADDI.N,  A9 A8 $120 S32I,
$1C L:
  A10 A8 13 L8UI,  A9 A8 $120 L32I,  A10 $40 L BEQZ.N,
  A9 $56 L BNEZ.N,
  A9 A8 1 L8UI,  A9 1 $56 L BNEI,
  A8 A8 $100 ADDMI,  A8 A8 184 L8UI,  A8 $56 L BNEZ.N,
  MEMW,  A8 $4 L L32R,  A8 CALLX8,
  $56 L J,
$40 L:
  A9 $56 L BNEZ.N,
  A8 A8 $100 ADDMI,  A9 A8 29 L8UI,  A9 2 $56 L BNEI,
  A8 A8 184 L8UI,  A8 $56 L BNEZ.N,
  A8 $8 L L32R,  A8 CALLX8,
$56 L:
  RETW.N,
$58 L:  END-CODE

CODE pm_coex_separate_connectio~659C   \ pm_coex_separate_connectionless_window: .text.pm_coex_separate_connectionless_window $24
  $0 9 LITERALS
    SYMBOL g_osi_funcs_p  SYMBOL g_pm  $14C SYMBOL+ g_pm  $138 SYMBOL+ g_pm
    SYMBOL pm_coex_schm_overall_period_get  SYMBOL pm_wake_up
    SYMBOL wifi_gpio_debug  SYMBOL wifi_gpio_debug  SYMBOL pm_wake_done
$24 L:
  A1 64 ENTRY,  A3 $0 L L32R,  A2 A3 0 L32I.N,  A10 A2 $194 L32I,  A10 CALLX8,
  A10 $2DA L BEQZ,
  A6 $4 L L32R,  A4 A6 $100 ADDMI,  A2 A4 29 L8UI,  A2 2 $47 L BGEUI,
  A2 A6 13 L8UI,  A2 $2DA L BEQZ,
$47 L:
  A2 A3 0 L32I,  A10 A2 $110 L32I,  A10 CALLX8,
  A5 A10 MOV.N,  A13 A11 MOV.N,  A2 A4 136 L8UI,  A2 $AB L BEQZ,
  A2 A6 $18A L16UI,  A2 $AB L BEQZ,
  A7 1 MOVI,  A2 A3 0 L32I,  A10 $8 L L32R,  A2 A2 236 L32I,  A11 A1 16 S32I,  MEMW,
  A2 CALLX8,
  A9 A6 $1A8 L32I,  A8 A6 $1A0 L32I,  A8 A9 A8 ADD.N,  A2 A6 $1AC L32I,
  A10 A6 $1A4 L32I,  A13 A1 16 L32I.N,  A8 A9 $8A L BLTU,
  A7 0 MOVI.N,
$8A L:
  A2 A2 A10 ADD.N,  A2 A7 A2 ADD.N,  A7 A8 A5 SUB,  A9 1 MOVI.N,  A8 A7 $98 L BLTU,
  A9 0 MOVI.N,
$98 L:
  A2 A2 A13 SUB,  A2 A2 A9 SUB,  A2 $A5 L BGEZ,
  A7 0 MOVI.N,  A2 A7 MOV.N,
$A5 L:
  A7 A6 $1A8 S32I,  A2 A6 $1AC S32I,
$AB L:
  A7 A6 $1A8 L32I,  A8 A6 $1AC L32I,  A9 A7 A8 OR,  A14 A6 96 L32I,  A2 A6 100 L32I,
  A9 $2B9 L BEQZ,
  A12 A6 $18A L16UI,  A12 $14C L BNEZ,
  A8 A3 0 L32I.N,  A7 $C L L32R,  A10 A7 MOV.N,  A8 A8 236 L32I,  A13 A1 16 S32I.N,
  A14 A1 8 S32I.N,  A12 A1 4 S32I,  MEMW,  A8 CALLX8,
  A8 A3 0 L32I.N,  A8 A8 248 L32I,  A8 A1 12 S32I.N,  MEMW,  A8 $10 L L32R,
  A8 CALLX8,
  A11 A10 MOV.N,  A9 A3 0 L32I.N,  A10 A9 $1BC L32I,  A11 A1 0 S32I.N,  MEMW,
  A10 CALLX8,
  A11 A1 0 L32I.N,  A10 A11 A10 MULL,  A10 A10 A10 ADDX4,  A10 A10 A10 ADDX4,
  A12 A1 4 L32I.N,  A11 A10 2 SLLI,  A10 A7 MOV.N,  A8 A1 12 L32I,  A8 CALLX8,
  A13 A1 16 L32I.N,  A14 A1 8 L32I.N,  A13 A2 $124 L BLTU,
  A2 A13 $11E L BEQ,
  $2B9 L J,
$11E L:
  A5 A14 $124 L BLTU,
  $2B9 L J,
$124 L:
  A2 1 MOVI.N,  A7 A6 96 L32I,  A5 A7 A5 SUB,  A8 A6 100 L32I,  A7 A5 $134 L BLTU,
  A2 0 MOVI.N,
$134 L:
  A8 A8 A13 SUB,  A8 A8 A2 SUB,  A2 A3 0 L32I.N,  A7 A5 MOV.N,  A12 A5 MOV.N,
  A11 0 MOVI.N,  A10 1 MOVI.N,  A2 A2 $19C L32I,  $260 L J,
  2 ZEROS,
$14C L:
  A13 A2 $155 L BLTU,
  A2 A13 $181 L BNE,
  A5 A14 $181 L BGEU,
$155 L:
  A12 A14 A5 SUB,  A5 1 MOVI,  A14 A12 $161 L BLTU,
  A5 0 MOVI,
$161 L:
  A2 A2 A13 SUB,  A2 A2 A5 SUB,  A2 A8 $170 L BLTU,
  A8 A2 $172 L BNE,
  A12 A7 $172 L BGEU,
$170 L:
  A7 A12 MOV.N,
$172 L:
  A2 A3 0 L32I.N,  A11 0 MOVI.N,  A10 1 MOVI.N,  A2 A2 $19C L32I,  $201 L J,
  3 ZEROS,
$181 L:
  A9 A12 5 SLLI,  A10 A6 $198 L32I,  A9 A9 A12 SUB,  A12 A9 A12 ADDX4,
  A15 A12 3 SLLI,  A9 A12 31 SRAI,  A12 A12 A10 ADDX8,  A11 A6 $19C L32I,
  A10 1 MOVI,  A12 A15 $1A2 L BLTU,
  A10 0 MOVI,
$1A2 L:
  A9 A9 A11 ADD.N,  A9 A10 A9 ADD.N,  A10 A12 A5 SUB,  A11 1 MOVI.N,
  A12 A10 $1B0 L BLTU,
  A11 0 MOVI.N,
$1B0 L:
  A9 A9 A13 SUB,  A9 A9 A11 SUB,  A9 $1BD L BGEZ,
  A10 0 MOVI.N,  A9 A10 MOV.N,
$1BD L:
  A11 A6 $1B0 L32I,  A12 1 MOVI.N,  A11 A14 A11 ADD,  A15 A6 $1B4 L32I,
  A11 A14 $1CE L BLTU,
  A12 0 MOVI,
$1CE L:
  A2 A2 A15 ADD.N,  A5 A11 A5 SUB,  A2 A12 A2 ADD.N,  A12 1 MOVI.N,
  A11 A5 $1DC L BLTU,
  A12 0 MOVI.N,
$1DC L:
  A2 A2 A13 SUB,  A2 A2 A12 SUB,  A2 $1EC L BGEZ,
  A5 0 MOVI.N,  A2 A5 MOV.N,  $1EF L J,
$1EC L:
  A9 A2 $1F5 L BLTU,
$1EF L:
  A2 A9 $209 L BNE,
  A10 A5 $209 L BGEU,
$1F5 L:
  A2 A3 0 L32I.N,  A12 A7 MOV.N,  A11 0 MOVI.N,  A10 38 MOVI,  A2 A2 $19C L32I,
$201 L:
  A2 CALLX8,
  $271 L J,
  2 ZEROS,
$209 L:
  A12 A10 A5 SUB,  A13 1 MOVI.N,  A10 A12 $214 L BLTU,
  A13 0 MOVI,
$214 L:
  A11 A9 A2 SUB,  A11 A11 A13 SUB,  A11 A11 A11 ADD.N,  A13 A12 31 1 EXTUI,
  A11 A13 A11 OR,  A12 A12 A12 ADD,  A11 A8 $234 L BLTU,
  A8 A11 $22E L BEQ,
  $2B9 L J,
$22E L:
  A12 A7 $234 L BLTU,
  $2B9 L J,
$234 L:
  A5 A7 A5 ADD.N,  A11 1 MOVI.N,  A5 A7 $23E L BLTU,
  A11 0 MOVI,
$23E L:
  A2 A8 A2 ADD.N,  A10 A5 A10 SUB,  A2 A11 A2 ADD.N,  A8 1 MOVI.N,
  A5 A10 $24D L BLTU,
  A8 0 MOVI,
$24D L:
  A2 A2 A9 SUB,  A8 A2 A8 SUB,  A2 A3 0 L32I.N,  A7 A10 MOV.N,  A12 A10 MOV.N,
  A11 0 MOVI.N,  A10 38 MOVI.N,  A2 A2 $19C L32I,
$260 L:
  A8 A1 12 S32I,  MEMW,  A2 CALLX8,
  A8 A1 12 L32I.N,  A8 A7 A8 OR,  A8 $2B9 L BEQZ,
$271 L:
  A2 A4 136 L8UI,  A2 $291 L BNEZ.N,
  A8 $14 L L32R,  A8 CALLX8,
  A5 1 MOVI.N,  A11 A2 MOV.N,  A10 17 MOVI.N,  A5 A4 136 S8I,  MEMW,  MEMW,
  A8 $18 L L32R,  A8 CALLX8,
$291 L:
  A2 A6 $18A L16UI,  A2 $2A5 L BEQZ.N,
  A12 0 MOVI.N,  A11 A7 MOV.N,  A2 A3 0 L32I.N,  A10 $8 L L32R,  A2 A2 248 L32I,
  A2 CALLX8,
$2A5 L:
  A2 A3 0 L32I.N,  A10 A2 $110 L32I,  A10 CALLX8,
  A10 A6 $1A0 S32I,  A11 A6 $1A4 S32I,  MEMW,  $2DA L J,
$2B9 L:
  A2 A4 136 L8UI,  A2 $2DA L BEQZ.N,
  A2 0 MOVI.N,  A11 1 MOVI,  A10 17 MOVI.N,  A2 A4 136 S8I,  MEMW,  MEMW,
  A8 $1C L L32R,  A8 CALLX8,
  A8 $20 L L32R,  A8 CALLX8,
$2DA L:
  RETW.N,
$2DC L:  END-CODE

CODE pm_update_by_connectionles~3F93   \ pm_update_by_connectionless_status: .wifislpiram.23 $34
  $0 13 LITERALS
    $FFFF T,  $138 SYMBOL+ g_pm  SYMBOL g_osi_funcs_p  $14C SYMBOL+ g_pm
    SYMBOL g_pm  SYMBOL wifi_gpio_debug  SYMBOL pm_wake_done
    SYMBOL pm_get_connectionless_status  SYMBOL wifi_event_post
    SYMBOL wifi_gpio_debug  SYMBOL pm_coex_separate_connectio~659C
    SYMBOL pm_wake_up  SYMBOL wifi_gpio_debug
$34 L:
  A1 32 ENTRY,  A3 $8 L L32R,  A6 0 MOVI.N,  A2 A3 0 L32I.N,  A7 $4 L L32R,
  A10 A7 MOV.N,  A2 A2 236 L32I,  A2 CALLX8,
  A2 A3 0 L32I,  A5 $C L L32R,  A10 A5 A5 OR,  A2 A2 236 L32I,  A2 CALLX8,
  A2 $10 L L32R,  A4 A2 $100 ADDMI,  A8 A4 136 L8UI,  A6 A4 140 S8I,  MEMW,
  A8 $82 L BEQZ.N,
  A11 1 MOVI.N,  A10 17 MOVI.N,  A6 A4 136 S8I,  MEMW,  MEMW,  A8 $14 L L32R,
  A8 CALLX8,
  A8 $18 L L32R,  A8 CALLX8,
$82 L:
  A6 A4 29 L8UI,  A6 2 $8E L BGEUI,
  A6 A2 13 L8UI,  A6 $170 L BEQZ,
$8E L:
  A8 $1C L L32R,  A8 CALLX8,
  A6 A10 MOV.N,  A8 A2 $18A L16UI,  A8 $EA L BEQZ,
  A12 0 MOVI.N,  A11 A12 MOV.N,  A10 22 MOVI,  A8 $20 L L32R,  A8 CALLX8,
  A8 A2 $18A L16UI,  A12 0 MOVI.N,  A11 A8 5 SLLI,  A11 A11 A8 SUB,
  A11 A11 A8 ADDX4,  A8 A3 0 L32I,  A10 A7 A7 OR,  A11 A11 3 SLLI,  A8 A8 248 L32I,
  A8 CALLX8,
  A7 A3 0 L32I.N,  A10 A7 $110 L32I,  A7 1 MOVI,  A10 CALLX8,
  A10 A2 $198 S32I,  A10 16 MOVI.N,  A11 A2 $19C S32I,  A11 2 MOVI.N,  MEMW,
  A8 $24 L L32R,  A8 CALLX8,
  A7 A4 140 S8I,  MEMW,
$EA L:
  A9 -2 MOVI.N,  A7 0 MOVI.N,  A8 $0 L L32R,  A6 A9 $114 L BEQ,
  A7 A6 0 16 EXTUI,  A6 -1 $105 L BNEI,
  A7 A2 $18A L16UI,  MEMW,  $105 L J,
  2 ZEROS,
$105 L:
  A8 A7 5 SLLI,  A8 A8 A7 SUB,  A8 A8 A7 ADDX4,  A8 A8 3 SLLI,  A7 A8 31 SRAI,
$114 L:
  A8 A2 $1A8 S32I,  A7 A2 $1AC S32I,  A2 A3 0 L32I,  A10 A2 $194 L32I,  MEMW,
  A10 CALLX8,
  A2 A10 MOV.N,  A10 $134 L BEQZ.N,
  A8 $28 L L32R,  A8 CALLX8,
  $170 L J,
  1 ZEROS,
$134 L:
  A6 $170 L BEQZ.N,
  A8 $2C L L32R,  A8 CALLX8,
  A7 1 MOVI.N,  A11 A2 MOV.N,  A10 17 MOVI.N,  A7 A4 136 S8I,  MEMW,  MEMW,
  A8 $30 L L32R,  A8 CALLX8,
  A6 1 $170 L BLTI,
  A6 A6 0 16 EXTUI,  A11 A6 5 SLLI,  A3 A3 0 L32I.N,  A11 A11 A6 SUB,
  A11 A11 A6 ADDX4,  A12 A2 MOV.N,  A11 A11 3 SLLI,  A3 A3 248 L32I,  A10 A5 A5 OR,
  A3 CALLX8,
$170 L:
  RETW.N,
$172 L:  END-CODE

CODE pm_check_state   \ .wifislprxiram.36 $10
  $0 4 LITERALS
    SYMBOL g_pm  SYMBOL g_osi_funcs_p  SYMBOL pm_dream  SYMBOL pm_set_state
$10 L:
  A1 32 ENTRY,  A3 $0 L L32R,  A2 A3 36 L32I,  A2 $25 L BEQZ,
$1C L:
  A2 A3 13 L8UI,  A2 $50 L BNEZ.N,
  $3A L J,
  1 ZEROS,
$25 L:
  A2 $4 L L32R,  A2 A2 0 L32I,  A10 A2 $194 L32I,  A10 CALLX8,
  A10 $1C L BNEZ,
  A2 A3 14 L8UI,  A2 $1C L BEQZ,
$3A L:
  A2 0 MOVI.N,  A3 A3 1 L8UI,  A3 A2 $50 L BEQ,
  A8 $8 L L32R,  A8 CALLX8,
  A10 A2 MOV.N,  A8 $C L L32R,  A8 CALLX8,
$50 L:
  RETW.N,
$52 L:  END-CODE

CODE pm_go_to_sleep   \ .text.pm_go_to_sleep $28
  $0 10 LITERALS
    SYMBOL g_pm  SYMBOL g_mesh_is_started  SYMBOL g_mesh_is_root
    SYMBOL g_mesh_init_ps_type  $13 SYMBOL+ g_pm  $14 SYMBOL+ g_pm
    SYMBOL esp_mesh_quick_funcs  SYMBOL pm_check_state  SYMBOL pm_set_state
    SYMBOL pm_send_nullfunc
$28 L:
  A1 32 ENTRY,  A8 $1C L L32R,  A8 CALLX8,
  A10 $85 L BEQZ,
  A2 $0 L L32R,  A8 A2 1 L8UI,  A8 2 $85 L BEQI,
  A10 1 MOVI.N,  A8 $20 L L32R,  A8 CALLX8,
  A8 $4 L L32R,  A8 A8 0 L8UI,  A8 $78 L BEQZ.N,
  A8 $8 L L32R,  MEMW,  A8 A8 0 L8UI,  A8 A8 0 8 EXTUI,  A8 $78 L BNEZ.N,
  A8 $C L L32R,  A8 A8 0 L32I.N,  A8 $78 L BEQZ.N,
  A10 1 MOVI,  A2 $18 L L32R,  A12 $10 L L32R,  A2 A2 136 L32I,  A11 $14 L L32R,
  A2 CALLX8,
  $85 L J,
  1 ZEROS,
$78 L:
  A2 A2 20 L8UI,  A2 $85 L BNEZ.N,
  A10 1 MOVI.N,  A8 $24 L L32R,  A8 CALLX8,
$85 L:
  RETW.N,
$87 L:  END-CODE

CODE pm_set_next_tbtt   \ .text.pm_set_next_tbtt $24
  $0 9 LITERALS
    SYMBOL g_osi_funcs_p  SYMBOL g_pm  SYMBOL g_intr_lock_mux  $1387 T,
    $A4 SYMBOL+ g_pm  SYMBOL rodata_wlog_warning.37
    SYMBOL pm_coex_schm_overall_period_get  SYMBOL __udivdi3  SYMBOL wifi_log
$24 L:
  A1 96 ENTRY,  A4 $0 L L32R,  A2 A4 0 L32I.N,  A10 A2 $110 L32I,  A2 1 MOVI,
  A10 CALLX8,
  A6 $4 L L32R,  A3 A6 40 L32I.N,  A10 A3 A10 ADD.N,  A10 A3 $41 L BLTU,
  A2 0 MOVI.N,
$41 L:
  A3 A6 92 L32I,  A2 A2 A11 ADD,  A2 A3 $4D L BGEU,
  $250 L J,
$4D L:
  A3 A2 $59 L BNE,
  A2 A6 88 L32I,  A10 A2 $59 L BGEU,
  $250 L J,
$59 L:
  A2 A4 0 L32I.N,  A10 A2 $110 L32I,  A10 CALLX8,
  A2 A4 0 L32I.N,  A10 A1 32 S32I.N,  A10 A2 $194 L32I,  A11 A1 36 S32I.N,  MEMW,
  A10 CALLX8,
  A10 $AA L BEQZ.N,
  A5 A6 14 L8UI,  A5 $94 L BEQZ.N,
  A8 $18 L L32R,  A8 CALLX8,
  A3 1 MOVI.N,  A2 A6 44 L32I.N,  A10 A10 A2 MULL,  A2 A6 88 L32I,  A2 A10 A2 ADD.N,
  A5 A6 92 L32I,  A2 A10 $F6 L BLTU,
  A3 0 MOVI.N,  $F6 L J,
$94 L:
  A7 A6 44 L32I.N,  A3 1 MOVI.N,  A2 A6 88 L32I,  A2 A7 A2 ADD.N,  A8 A6 92 L32I,
  A2 A7 $A5 L BLTU,
  A3 A5 MOV.N,
$A5 L:
  A3 A3 A8 ADD.N,  $F8 L J,
$AA L:
  A5 A6 36 L32I.N,  A2 A6 88 L32I,  A3 A6 92 L32I,  A8 A6 44 L32I.N,
  A5 $C4 L BNEZ.N,
  A2 A8 A2 ADD.N,  A7 1 MOVI.N,  A2 A8 $E8 L BLTU,
  A7 A5 MOV.N,  $E8 L J,
  2 ZEROS,
$C4 L:
  A5 1 $D5 L BNEI,
  A7 A6 11 L8UI,  A7 A7 A8 MULL,  A2 A7 A2 ADD.N,  A2 A7 $F6 L BLTU,
  $F4 L J,
$D5 L:
  A5 2 $ED L BNEI,
  A5 A6 32 L16UI,  A7 1 MOVI.N,  A5 A5 A8 MULL,  A2 A5 A2 ADD.N,  A2 A5 $E8 L BLTU,
  A7 A10 A10 OR,
$E8 L:
  A3 A7 A3 ADD.N,  $F8 L J,
$ED L:
  A2 A8 A2 ADD.N,  A5 1 MOVI.N,  A2 A8 $F6 L BLTU,
$F4 L:
  A5 A10 MOV.N,
$F6 L:
  A3 A5 A3 ADD.N,
$F8 L:
  A2 A6 88 S32I,  A3 A6 92 S32I,  A2 A4 0 L32I.N,  A3 $8 L L32R,  A2 A2 40 L32I.N,
  A10 A3 0 L32I.N,  MEMW,  A2 CALLX8,
  A2 A4 0 L32I.N,  A10 A1 40 S32I.N,  A10 A2 $110 L32I,  MEMW,  A10 CALLX8,
  A9 A10 MOV.N,  A7 A11 MOV.N,  A2 A6 88 L32I,  A11 1 MOVI,  A10 A2 A10 SUB,
  A5 A6 92 L32I,  A2 A10 $130 L BLTU,
  A11 0 MOVI,
$130 L:
  A12 A5 A7 SUB,  A3 A12 A11 SUB,  A12 A11 $162 L BNE,
  A11 $C L L32R,  A11 A10 $15A L BLTU,
  A10 1 MOVI.N,  A11 A6 44 L32I.N,  A2 A11 A2 ADD.N,  A2 A11 $14A L BLTU,
  A10 A3 MOV.N,
$14A L:
  A10 A10 A5 ADD.N,  A2 A6 88 S32I,  A3 0 MOVI.N,  A10 A6 92 S32I,  MEMW,  $1D2 L J,
$15A L:
  A11 -1 MOVI.N,  A11 A11 31 SLLI,  A11 A10 $1D2 L BGEU,
$162 L:
  A3 $271 MOVI,  A3 A3 3 SLLI,  A12 A3 A2 SUB,  A6 A6 44 L32I.N,  A10 1 MOVI.N,
  A3 A12 $175 L BLTU,
  A10 0 MOVI,
$175 L:
  A3 A5 NEG,  A3 A3 A10 SUB,  A10 A12 A9 ADD.N,  A11 1 MOVI,  A10 A12 $186 L BLTU,
  A11 0 MOVI,
$186 L:
  A3 A3 A7 ADD.N,  A12 A6 MOV.N,  A13 0 MOVI.N,  A11 A11 A3 ADD.N,  A9 A1 52 S32I,
  MEMW,  A8 $1C L L32R,  A8 CALLX8,
  A3 A10 MOV.N,  A10 A10 1 ADDI.N,  A13 A11 MOV.N,  A14 $4 L L32R,  A12 1 MOVI.N,
  A9 A1 52 L32I.N,  A10 A3 $1AC L BLTU,
  A12 0 MOVI.N,
$1AC L:
  A12 A12 A11 ADD.N,  A11 A10 A6 MULL,  A12 A12 A6 MULL,  A10 A10 A6 MULUH,
  A2 A11 A2 ADD.N,  A10 A12 A10 ADD.N,  A6 1 MOVI.N,  A2 A11 $1C2 L BLTU,
  A6 0 MOVI.N,
$1C2 L:
  A10 A10 A5 ADD.N,  A6 A6 A10 ADD.N,  A2 A14 88 S32I,  A6 A14 92 S32I,  MEMW,
  $1D4 L J,
$1D2 L:
  A13 A3 MOV.N,
$1D4 L:
  A2 A4 0 L32I.N,  A5 $10 L L32R,  A10 A5 MOV.N,  A2 A2 236 L32I,  A9 A1 52 S32I.N,
  A13 A1 48 S32I.N,  MEMW,  A2 CALLX8,
  A2 $4 L L32R,  A10 A4 0 L32I.N,  A14 A2 88 L32I,  A6 A10 248 L32I,
  A10 A10 $110 L32I,  A14 A1 44 S32I.N,  MEMW,  A10 CALLX8,
  A12 0 MOVI.N,  A14 A1 44 L32I.N,  A11 A14 A10 SUB,  A10 A5 MOV.N,  A6 CALLX8,
  A4 A4 0 L32I.N,  A5 $8 L L32R,  A4 A4 44 L32I.N,  A11 A1 40 L32I.N,
  A10 A5 0 L32I,  A4 CALLX8,
  A13 A1 48 L32I.N,  A9 A1 52 L32I.N,  A13 $225 L BNEZ.N,
  A4 100 MOVI,  A4 A3 $250 L BGEU,
$225 L:
  A11 1 MOVI.N,  A3 A2 44 L32I.N,  A12 2 MOVI.N,  A11 A11 12 SLLI,  A10 6 MOVI.N,
  A3 A1 16 S32I.N,  A3 A2 92 L32I,  A2 A2 88 L32I,  A14 A1 32 L32I.N,
  A15 A1 36 L32I.N,  A13 $14 L L32R,  A2 A1 8 S32I.N,  A3 A1 12 S32I.N,
  A9 A1 0 S32I.N,  A7 A1 4 S32I.N,  MEMW,  A8 $20 L L32R,  A8 CALLX8,
$250 L:
  RETW.N,
$252 L:  END-CODE

CODE pm_mesh_set_next_tbtt   \ .text.pm_mesh_set_next_tbtt $1C
  $0 7 LITERALS
    SYMBOL g_mesh_is_started  SYMBOL g_mesh_is_root  SYMBOL g_mesh_init_ps_type
    SYMBOL g_pm  SYMBOL esp_mesh_quick_funcs  $A4 SYMBOL+ g_pm
    SYMBOL g_osi_funcs_p
$1C L:
  A1 48 ENTRY,  A2 $0 L L32R,  A2 A2 0 L8UI,  A2 $84 L BEQZ,
  A3 $4 L L32R,  MEMW,  A3 A3 0 L8UI,  A3 A3 0 8 EXTUI,  A3 $7C L BNEZ,
  A4 $8 L L32R,  A8 A4 0 L32I.N,  A8 $82 L BEQZ,
  A12 A1 MOV.N,  A4 $C L L32R,  A5 $10 L L32R,  A11 A4 11 L8UI,  A5 A5 120 L32I,
  A10 A4 44 L32I.N,  A3 A1 0 S32I,  MEMW,  A5 CALLX8,
  A5 $18 L L32R,  A8 A5 0 L32I,  A4 $14 L L32R,  A10 A4 A4 OR,  A8 A8 236 L32I,
  A8 CALLX8,
  A12 A3 MOV.N,  A10 A4 MOV.N,  A5 A5 0 L32I.N,  A11 A1 0 L32I.N,  A5 A5 248 L32I,
  A5 CALLX8,
  $84 L J,
  1 ZEROS,
$7C L:
  A2 0 MOVI.N,  $84 L J,
  1 ZEROS,
$82 L:
  A2 A8 MOV.N,
$84 L:
  RETW.N,
$86 L:  END-CODE

CODE pm_update_at_next_beacon   \ .text.pm_update_at_next_beacon $0
  $0 ORIGIN
  A1 32 ENTRY,  RETW.N,
$5 L:  END-CODE

CODE pm_handle_tbtt_interval   \ .wifislpiram.38 $14
  $0 5 LITERALS
    SYMBOL g_pm  $817 T,  SYMBOL rodata.str1.1  SYMBOL rodata_wlog_error.39
    SYMBOL wifi_log
$14 L:
  A1 32 ENTRY,  A9 $0 L L32R,  A3 $5C L BEQZ.N,
  A10 A9 11 L8UI,  A8 A3 3 L8UI,  A8 A10 $40 L BEQ,
  A12 1 MOVI.N,  A11 A12 12 SLLI,  A15 $4 L L32R,  A10 6 MOVI,  A14 $8 L L32R,
  A13 $C L L32R,  A8 $10 L L32R,  A8 CALLX8,
$3C L:
  $3C L J,
  1 ZEROS,
$40 L:
  A10 A2 A8 REMU,  A10 $4C L BEQZ,
  A8 A8 A2 REMU,  A8 $61 L BNEZ,
$4C L:
  A8 A3 2 L8UI,  A8 A8 A2 REMU,  A8 $61 L BEQZ.N,
  A2 A9 44 L32I.N,  A2 A8 A2 MULL,  $66 L J,
$5C L:
  A2 A9 44 L32I.N,  $66 L J,
$61 L:
  A3 A9 44 L32I.N,  A2 A2 A3 MULL,
$66 L:
  RETW.N,
$68 L:  END-CODE

CODE pm_update_next_tbtt   \ .wifislpiram.40 $48
  $0 18 LITERALS
    SYMBOL g_osi_funcs_p  SYMBOL g_pm_cfg  SYMBOL g_pm
    SYMBOL ptr_beacon_offset_funcs  SYMBOL g_intr_lock_mux
    SYMBOL g_mesh_is_started  SYMBOL g_mesh_is_root  SYMBOL g_mesh_init_ps_type
    $3FF73C00 T,  SYMBOL g_wifi_mac_time_delta  $1387 T,
    SYMBOL rodata_wlog_warning.41  $A4 SYMBOL+ g_pm  SYMBOL esp_mesh_quick_funcs
    SYMBOL __umoddi3  SYMBOL pm_coex_schm_overall_period_get
    SYMBOL pm_handle_tbtt_interval  SYMBOL wifi_log
$48 L:
  A1 96 ENTRY,  A5 $0 L L32R,  A6 A5 0 L32I.N,  A10 A6 $194 L32I,  A3 A1 52 S32I,
  MEMW,  A10 CALLX8,
  A13 0 MOVI.N,  A7 $4 L L32R,  A3 $8 L L32R,  A8 A1 52 L32I.N,  A6 A7 4 L32I.N,
  A7 A7 8 L32I.N,  A6 A7 A10 MOVNEZ,  A12 A3 44 L32I.N,  A10 A8 0 L32I.N,
  A11 A8 4 L32I.N,  A6 A3 40 S32I.N,  A6 A3 $100 ADDMI,  MEMW,  A8 $38 L L32R,
  A8 CALLX8,
  A7 A6 100 L8UI,  A10 A1 48 S32I.N,  A7 $A2 L BEQZ.N,
  A6 A6 212 L8UI,  A6 $A2 L BNEZ.N,
  A6 A3 13 L8UI,  A6 $A2 L BEQZ.N,
  A6 $C L L32R,  A6 A6 0 L32I.N,  A6 A6 4 L32I.N,  A6 $A2 L BEQZ,
  MEMW,  A6 CALLX8,
$A2 L:
  A6 A1 48 L32I.N,  A2 A2 A6 SUB,  A6 A5 0 L32I,  A10 A6 $194 L32I,  A10 CALLX8,
  A10 $C2 L BEQZ.N,
  A6 A3 14 L8UI,  A6 $E9 L BEQZ.N,
  A8 $3C L L32R,  A8 CALLX8,
  A11 A4 MOV.N,  $D2 L J,
$C2 L:
  A6 A3 36 L32I.N,  A6 $C9 L BNEZ.N,
  $E9 L J,
$C9 L:
  A6 1 $DD L BNEI,
  A10 A3 11 L8UI,  A11 A4 A4 OR,
$D2 L:
  A8 $40 L L32R,  A8 CALLX8,
  A9 A10 A2 ADD.N,  $ED L J,
$DD L:
  A6 2 $E9 L BNEI,
  A10 A3 32 L16UI,  A11 A4 MOV.N,  $D2 L J,
  1 ZEROS,
$E9 L:
  A9 A3 44 L32I.N,  A9 A2 A9 ADD.N,
$ED L:
  A6 A3 40 L32I.N,  A7 A3 $1CC L32I,  A7 A7 A6 SUB,  A7 A7 A9 ADD.N,
  A6 A5 0 L32I.N,  A10 $10 L L32R,  A6 A6 40 L32I.N,  A10 A10 0 L32I.N,
  A9 A1 56 S32I.N,  MEMW,  A6 CALLX8,
  A6 A10 MOV.N,  A10 $14 L L32R,  A10 A10 0 L8UI,  A9 A1 56 L32I.N,
  A10 $138 L BEQZ.N,
  A10 $18 L L32R,  MEMW,  A10 A10 0 L8UI,  A10 A10 0 8 EXTUI,  A10 $138 L BNEZ.N,
  A10 $1C L L32R,  A10 A10 0 L32I.N,  A10 $138 L BEQZ.N,
  A10 A5 0 L32I.N,  A10 A10 $110 L32I,  A10 CALLX8,
  A9 A1 56 L32I.N,  $147 L J,
  2 ZEROS,
$138 L:
  A10 $20 L L32R,  A11 $24 L L32R,  MEMW,  A10 A10 0 L32I.N,  A11 A11 0 L32I.N,
  A10 A10 A11 ADD.N,
$147 L:
  A13 A7 A10 SUB,  A11 $28 L L32R,  A12 A5 0 L32I.N,  A11 A13 $15A L BLTU,
  A2 A3 44 L32I.N,  A9 A9 A2 ADD.N,  $215 L J,
  1 ZEROS,
$15A L:
  A11 -1 MOVI.N,  A11 A11 31 SLLI,  A11 A13 $165 L BLTU,
  $215 L J,
$165 L:
  A11 $271 MOVI,  A13 A3 44 L32I.N,  A7 A11 A7 SUBX8,  A10 A7 A10 ADD.N,
  A10 A10 A13 QUOU,  A7 A10 1 ADDI.N,  A7 A7 A13 MULL,  A9 A9 A7 ADD.N,
  A7 100 MOVI,  A7 A10 $182 L BLTU,
  $215 L J,
$182 L:
  A11 A6 MOV.N,  A6 $10 L L32R,  A7 A12 44 L32I.N,  A10 A6 0 L32I.N,  A6 A4 MOV.N,
  A9 A1 56 S32I,  MEMW,  A7 CALLX8,
  A7 A4 MOV.N,  A12 A3 44 L32I.N,  A9 A1 56 L32I.N,  A4 $1A5 L BEQZ,
  A6 A4 3 L8UI,  A7 A4 2 L8UI,
$1A5 L:
  A4 $14 L L32R,  A4 A4 0 L8UI,  A3 A3 32 L16UI,  A4 $1DB L BEQZ.N,
  A4 $18 L L32R,  MEMW,  A4 A4 0 L8UI,  A4 A4 0 8 EXTUI,  A4 $1DB L BNEZ.N,
  A4 $1C L L32R,  A4 A4 0 L32I.N,  A4 $1DB L BEQZ.N,
  A4 A5 0 L32I.N,  A10 A4 $110 L32I,  A9 A1 56 S32I.N,  A12 A1 52 S32I.N,  MEMW,
  A10 CALLX8,
  A9 A1 56 L32I.N,  A12 A1 52 L32I.N,  $1EA L J,
$1DB L:
  A4 $20 L L32R,  MEMW,  A10 A4 0 L32I.N,  A4 $24 L L32R,  A4 A4 0 L32I.N,
  A10 A10 A4 ADD.N,
$1EA L:
  A11 1 MOVI.N,  A14 A2 MOV.N,  A15 A1 48 L32I.N,  A11 A11 12 SLLI,  A13 $2C L L32R,
  A10 A1 20 S32I.N,  A10 6 MOVI.N,  A12 A1 4 S32I.N,  A12 2 MOVI.N,
  A3 A1 16 S32I.N,  A7 A1 12 S32I.N,  A6 A1 8 S32I.N,  A9 A1 0 S32I,  MEMW,
  A8 $44 L L32R,  A8 CALLX8,
  $324 L J,
  2 ZEROS,
$215 L:
  A4 A3 40 L32I.N,  A2 A3 $1CC L32I,  A2 A2 A4 SUB,  A2 A2 A9 ADD.N,  A4 $30 L L32R,
  A10 A4 MOV.N,  A7 A12 236 L32I,  A7 CALLX8,
  A7 A5 0 L32I.N,  A10 A7 $110 L32I,  A10 CALLX8,
  A9 A10 MOV.N,  A7 A11 MOV.N,  A8 $14 L L32R,  A10 A8 0 L8UI,  A10 $265 L BEQZ.N,
  A10 $18 L L32R,  MEMW,  A10 A10 0 L8UI,  A10 A10 0 8 EXTUI,  A10 $265 L BNEZ.N,
  A10 $1C L L32R,  A10 A10 0 L32I.N,  A10 $265 L BEQZ.N,
  A10 A5 0 L32I.N,  A10 A10 $110 L32I,  A9 A1 56 S32I.N,  MEMW,  A10 CALLX8,
  A9 A1 56 L32I.N,  $274 L J,
$265 L:
  A10 $20 L L32R,  A11 $24 L L32R,  MEMW,  A10 A10 0 L32I.N,  A11 A11 0 L32I.N,
  A10 A10 A11 ADD.N,
$274 L:
  A10 A2 A10 SUB,  A11 A10 A9 ADD.N,  A9 1 MOVI.N,  A11 A10 $281 L BLTU,
  A9 0 MOVI,
$281 L:
  A8 $14 L L32R,  A9 A9 A7 ADD.N,  A9 A3 92 S32I,  A9 A8 0 L8UI,  A11 A3 88 S32I,
  A9 $2D1 L BEQZ.N,
  A7 $18 L L32R,  MEMW,  A7 A7 0 L8UI,  A7 A7 0 8 EXTUI,  A7 $2D1 L BNEZ.N,
  A10 $1C L L32R,  A10 A10 0 L32I.N,  A10 $2D1 L BEQZ.N,
  A13 A1 32 ADDI,  A2 $34 L L32R,  A12 A1 48 L32I.N,  A11 A3 11 L8UI,
  A2 A2 124 L32I,  A10 A3 44 L32I.N,  A7 A1 32 S32I.N,  MEMW,  A2 CALLX8,
  A12 A7 MOV.N,  A10 A4 MOV.N,  A2 A5 0 L32I.N,  A11 A1 32 L32I.N,  A2 A2 248 L32I,
  A2 CALLX8,
  $316 L J,
  2 ZEROS,
$2D1 L:
  A7 A5 0 L32I.N,  A8 A7 248 L32I,  A9 $2FD L BEQZ.N,
  A3 $18 L L32R,  MEMW,  A3 A3 0 L8UI,  A3 A3 0 8 EXTUI,  A3 $2FD L BNEZ.N,
  A3 $1C L L32R,  A3 A3 0 L32I.N,  A3 $2FD L BEQZ.N,
  A10 A7 $110 L32I,  A8 A1 52 S32I.N,  MEMW,  A10 CALLX8,
  A8 A1 52 L32I.N,  $30C L J,
$2FD L:
  A3 $20 L L32R,  MEMW,  A10 A3 0 L32I.N,  A3 $24 L L32R,  A3 A3 0 L32I.N,
  A10 A10 A3 ADD.N,
$30C L:
  A11 A2 A10 SUB,  A12 0 MOVI.N,  A10 A4 MOV.N,  A8 CALLX8,
$316 L:
  A2 A5 0 L32I.N,  A11 A6 MOV.N,  A3 $10 L L32R,  A2 A2 44 L32I.N,  A10 A3 0 L32I.N,
  A2 CALLX8,
$324 L:
  RETW.N,
$326 L:  END-CODE

CODE pm_beacon_timestamp_statistic   \ .text.pm_beacon_timestamp_statistic $C
  $0 3 LITERALS
    SYMBOL g_pm_cnt  SYMBOL g_pm  SYMBOL __umoddi3
$C L:
  A1 32 ENTRY,  A2 $34 L BEQZ.N,
  A8 $4 L L32R,  A13 0 MOVI.N,  A3 $0 L L32R,  A10 A2 0 L32I.N,  A11 A2 4 L32I.N,
  A12 A8 44 L32I,  A8 $8 L L32R,  A8 CALLX8,
  A4 A3 36 L32I.N,  A10 A10 A4 SUB,  A2 A3 4 L32I.N,  A10 A10 A2 QUOU,
  A10 A10 A4 ADD.N,  A10 A3 36 S32I.N,
$34 L:
  MEMW,  RETW.N,
$39 L:  END-CODE

CODE pm_beacon_misc_process   \ .text.pm_beacon_misc_process $4
  $0 1 LITERALS
    SYMBOL pm_beacon_timestamp_statistic
$4 L:
  A1 32 ENTRY,  A10 A2 A2 OR,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$12 L:  END-CODE

CODE pm_is_in_wifi_slice_threshold   \ .text.pm_is_in_wifi_slice_threshold $8
  $0 2 LITERALS
    SYMBOL g_osi_funcs_p  SYMBOL g_pm
$8 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A8 A8 0 L32I.N,  A10 A8 $194 L32I,  A10 CALLX8,
  A8 1 MOVI.N,  A10 $50 L BEQZ.N,
  A8 $4 L L32R,  A10 A8 104 L32I,  A8 A8 108 L32I,  A3 A8 $2C L BLTU,
  A8 A3 $4E L BNE,
  A2 A10 $4E L BGEU,
$2C L:
  A9 A10 A2 SUB,  A11 0 MOVI.N,  A2 1 MOVI.N,  A10 A9 $38 L BLTU,
  A2 A11 MOV.N,
$38 L:
  A8 A8 A3 SUB,  A8 A8 A2 SUB,  A2 1 MOVI.N,  A11 A8 $48 L BNE,
  A9 A4 $48 L BGEU,
  A2 0 MOVI.N,
$48 L:
  A8 A2 MOV.N,  $50 L J,
  1 ZEROS,
$4E L:
  A8 0 MOVI.N,
$50 L:
  A2 A8 0 1 EXTUI,  RETW.N,
$55 L:  END-CODE

CODE pm_keep_alive   \ .wifislprxiram.42 $18
  $0 6 LITERALS
    $2DC6C0 T,  SYMBOL g_pm  SYMBOL g_osi_funcs_p  SYMBOL g_pm_cfg
    SYMBOL g_mesh_is_started  SYMBOL g_mesh_is_root
$18 L:
  A1 32 ENTRY,  A3 $4 L L32R,  A2 0 MOVI,  A8 A3 36 L32I,  A8 A2 $2A L BNE,
  $C0 L J,
$2A L:
  A8 $8 L L32R,  A8 A8 0 L32I,  A10 A8 $110 L32I,  A10 CALLX8,
  A13 A10 MOV.N,  A9 $10 L L32R,  A8 $C L L32R,  A9 A9 0 L8UI,  A12 A8 16 L32I.N,
  A9 A2 $58 L BEQ,
  A9 $14 L L32R,  MEMW,  A9 A9 0 L8UI,  A9 A9 0 8 EXTUI,  A9 A2 $58 L BNE,
  A12 $0 L L32R,
$58 L:
  A9 A8 29 L8UI,  A9 $9D L BEQZ.N,
  A2 A3 $1E0 L32I,  A10 A10 A2 SUB,  A2 1 MOVI.N,  A9 A3 $1E4 L32I,
  A13 A10 $6D L BLTU,
  A2 0 MOVI.N,
$6D L:
  A9 A11 A9 SUB,  A9 A9 A2 SUB,  A8 A8 48 L16UI,  A2 A8 9 SLLI,  A8 A8 14 SLLI,
  A8 A8 A2 SUB,  A8 A8 A2 ADDX4,  A8 A8 3 SLLI,  A3 A8 31 SRAI,  A2 1 MOVI,
  A9 A3 $94 L BLTU,
  A3 A9 $96 L BNE,
  A10 A8 $96 L BGEU,
$94 L:
  A2 0 MOVI.N,
$96 L:
  A2 A2 0 8 EXTUI,  $C0 L J,
  1 ZEROS,
$9D L:
  A14 A3 72 L32I,  A10 A10 A14 SUB,  A8 A3 76 L32I,  A3 1 MOVI.N,
  A13 A10 $AD L BLTU,
  A3 A9 MOV.N,
$AD L:
  A11 A11 A8 SUB,  A11 A11 A3 SUB,  A3 1 MOVI.N,  A2 A11 $BD L BNE,
  A10 A12 $BD L BGEU,
  A3 0 MOVI.N,
$BD L:
  A2 A3 0 8 EXTUI,
$C0 L:
  RETW.N,
$C2 L:  END-CODE

CODE pm_coex_schm_process   \ .text.pm_coex_schm_process $30
  $0 12 LITERALS
    SYMBOL g_osi_funcs_p  SYMBOL g_pm  $F4 SYMBOL+ g_pm  $3FF73CB8 T,
    SYMBOL ppCheckTxIdle  SYMBOL pm_disconnected_wake
    SYMBOL pm_coex_schm_overall_period_get  SYMBOL pm_coex_separate_connectio~659C
    SYMBOL pm_is_sleeping  SYMBOL hal_pm_unblock_txq  SYMBOL wifi_gpio_debug
    SYMBOL pm_is_sleeping
$30 L:
  A1 48 ENTRY,  A3 $0 L L32R,  A2 A3 0 L32I.N,  A10 A2 $1C4 L32I,  A10 CALLX8,
  A6 A10 MOV.N,  A10 $201 L BEQZ,
  A2 A3 0 L32I.N,  A5 1 MOVI.N,  A10 A2 $110 L32I,  A10 CALLX8,
  A7 A10 MOV.N,  A2 A3 0 L32I,  A10 A2 $1C0 L32I,  A11 A1 4 S32I,  MEMW,
  A10 CALLX8,
  A4 A10 MOV.N,  A2 A3 0 L32I.N,  A10 A2 $1BC L32I,  A10 CALLX8,
  A2 A6 0 L8UI,  A10 A2 A10 MULL,  A10 A10 A4 MULL,  A11 A1 4 L32I.N,
  A4 A7 A10 ADD.N,  A10 A1 0 S32I,  A4 A7 $7E L BLTU,
  A5 0 MOVI,
$7E L:
  A5 A5 A11 ADD.N,  A2 A6 1 L8UI,  A2 1 $89 L BBSI,
  $152 L J,
$89 L:
  A7 -65 MOVI,  A2 $4 L L32R,  A8 A2 $190 L32I,  A7 A7 A8 AND,  A7 $D6 L BNEZ.N,
  A10 1 MOVI.N,  MEMW,  A8 $10 L L32R,  A8 CALLX8,
  A10 $C6 L BEQZ.N,
  A8 A3 0 L32I.N,  A9 A8 $19C L32I,  A10 A8 $110 L32I,  A9 A1 4 S32I.N,  MEMW,
  A10 CALLX8,
  A12 A4 A10 SUB,  A11 A7 MOV.N,  A10 1 MOVI.N,  A9 A1 4 L32I.N,  A9 CALLX8,
  A8 $14 L L32R,  A8 CALLX8,
$C6 L:
  A7 1 MOVI.N,  A7 A2 24 S8I,  MEMW,  MEMW,  $ED L J,
  2 ZEROS,
$D6 L:
  A8 A3 0 L32I.N,  A10 A8 $110 L32I,  A7 A8 $19C L32I,  A10 CALLX8,
  A12 A4 A10 SUB,  A11 0 MOVI,  A10 1 MOVI,  A7 CALLX8,
$ED L:
  A4 A2 96 S32I,  A5 A2 100 S32I,  MEMW,  A8 $18 L L32R,  A8 CALLX8,
  A7 A10 A10 OR,  A8 A3 0 L32I,  A10 A8 $1BC L32I,  A10 CALLX8,
  A10 A7 A10 MULL,  A7 1 MOVI.N,  A9 A1 0 L32I.N,  A10 A10 A10 ADDX4,
  A10 A10 A10 ADDX4,  A8 A10 2 SLLI,  A10 A10 A9 SUBX4,  A8 A10 $120 L BLTU,
  A7 0 MOVI.N,
$120 L:
  A7 A7 NEG,  A10 A2 $1B0 S32I,  A7 A2 $1B4 S32I,  MEMW,  A8 $1C L L32R,  A8 CALLX8,
  A8 $20 L L32R,  A8 CALLX8,
  A10 $152 L BNEZ.N,
  A7 A2 13 L8UI,  A7 $14C L BEQZ.N,
  A7 A2 14 L8UI,  A7 $152 L BEQZ.N,
  A2 A2 $200 ADDMI,  A2 A2 30 L8UI,  A2 $152 L BNEZ.N,
$14C L:
  A8 $24 L L32R,  A8 CALLX8,
$152 L:
  A2 A6 1 L8UI,  A2 0 $1C4 L BBCI,
  A11 0 MOVI,  A10 7 MOVI,  A8 $28 L L32R,  A8 CALLX8,
  A8 $4 L L32R,  A9 A8 13 L8UI,  A9 $181 L BEQZ.N,
  A7 1 MOVI.N,  A2 A8 56 L32I.N,  A2 A4 A2 SUB,  A4 A2 $178 L BLTU,
  A7 0 MOVI.N,
$178 L:
  A5 A5 A7 SUB,  $191 L J,
  3 ZEROS,
$181 L:
  A2 -1000 MOVI,  A2 A4 A2 ADD.N,  A7 1 MOVI.N,  A2 A4 $18D L BLTU,
  A7 A9 MOV.N,
$18D L:
  A5 A5 -1 ADDI.N,  A5 A7 A5 ADD.N,
$191 L:
  A5 A8 108 S32I,  A5 A3 0 L32I.N,  A2 A8 104 S32I,  A2 $8 L L32R,  A10 A2 MOV.N,
  A5 A5 236 L32I,  A8 A1 4 S32I.N,  MEMW,  A5 CALLX8,
  A9 A3 0 L32I.N,  A8 A1 4 L32I.N,  A10 A9 $110 L32I,  A5 A8 104 L32I,
  A7 A9 248 L32I,  A10 CALLX8,
  A11 A5 A10 SUB,  A12 0 MOVI.N,  A10 A2 A2 OR,  A7 CALLX8,
$1C4 L:
  A2 A6 1 L8UI,  A2 2 $201 L BBCI,
  A8 $2C L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  A10 $1EC L BNEZ,
  A3 A3 0 L32I,  A10 A3 $110 L32I,  A5 A3 $19C L32I,  A10 CALLX8,
  A12 A4 A10 SUB,  A11 A2 MOV.N,  A10 1 MOVI,  A5 CALLX8,
$1EC L:
  A4 $C L L32R,  A3 7 MOVI.N,  A3 A3 17 SLLI,  MEMW,  A2 A4 0 L32I.N,  A2 A2 A3 OR,
  MEMW,  A2 A4 0 S32I.N,
$201 L:
  MEMW,  RETW.N,
$206 L:  END-CODE

CODE pm_coex_schm_process_restart   \ .text.pm_coex_schm_process_restart $4
  $0 1 LITERALS
    SYMBOL g_osi_funcs_p
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A8 A8 0 L32I.N,  A10 A8 $1C8 L32I,  A10 CALLX8,
  RETW.N,
$14 L:  END-CODE

CODE pm_coex_slice_timeout_process   \ .text.pm_coex_slice_timeout_process $2C
  $0 11 LITERALS
    SYMBOL g_osi_funcs_p  SYMBOL g_pm  $200 SYMBOL+ g_pm  $3FF73CB8 T,
    SYMBOL g_pm_cfg  SYMBOL wifi_gpio_debug  SYMBOL ppCheckTxIdle
    SYMBOL pm_enable_disconnected_sle~30EA  SYMBOL ppCheckTxConnTrafficIdle
    SYMBOL pm_go_to_sleep  SYMBOL pm_enable_sleep_delay_timer
$2C L:
  A1 32 ENTRY,  A5 $0 L L32R,  A2 A5 0 L32I.N,  A10 A2 $110 L32I,  A10 CALLX8,
  A2 $4 L L32R,  A3 A2 14 L8UI,  A3 $131 L BEQZ,
  A3 A2 15 L8UI,  A3 $131 L BNEZ,
  A10 7 MOVI.N,  A11 1 MOVI,  A8 $14 L L32R,  A8 CALLX8,
  A4 A5 0 L32I.N,  A10 A4 $194 L32I,  A10 CALLX8,
  A10 $131 L BEQZ,
  A8 -65 MOVI,  A4 A2 $190 L32I,  A8 A4 A8 AND,  A8 $B8 L BNEZ,
  A8 A2 24 S8I,  MEMW,  A4 $A1 L BNEZ.N,
  A10 1 MOVI.N,  MEMW,  A8 $18 L L32R,  A8 CALLX8,
  A10 $A1 L BEQZ.N,
  A8 A5 0 L32I.N,  A3 $8 L L32R,  A10 A3 MOV.N,  A8 A8 236 L32I,  A8 CALLX8,
  A11 $3E8 MOVI,  A12 A4 MOV.N,  A5 A5 0 L32I.N,  A10 A3 MOV.N,  A8 A2 $1B0 L32I,
  A11 A11 A8 ADD.N,  A5 A5 248 L32I,  A5 CALLX8,
$A1 L:
  A2 A2 $100 ADDMI,  A2 A2 29 L8UI,  A2 2 $AD L BEQI,
  $131 L J,
$AD L:
  A8 $1C L L32R,  A8 CALLX8,
  $131 L J,
  2 ZEROS,
$B8 L:
  A4 4 $131 L BNEI,
  A5 A2 17 L8UI,  A4 A2 $200 ADDMI,  A5 A4 29 S8I,  MEMW,  A4 7 MOVI.N,
  A4 A4 17 SLLI,  A5 $C L L32R,  A3 A2 24 S8I,  A3 A2 17 S8I,  A3 A2 18 S8I,  MEMW,
  MEMW,  A3 A5 0 L32I.N,  A3 A3 A4 OR,  MEMW,  A3 A5 0 S32I,  A3 A2 1 L8UI,
  A3 $F4 L BEQZ,
  A4 A2 19 L8UI,  A4 $128 L BEQZ.N,
$F4 L:
  A3 $10 L L32R,  A3 A3 0 L8UI,  A3 $10D L BEQZ.N,
$FC L:
  A3 1 MOVI.N,  A3 A2 25 S8I,  MEMW,  MEMW,  $11F L J,
  3 ZEROS,
$10D L:
  A3 A2 64 L32I,  A3 $FC L BNEZ,
  A10 1 MOVI,  A8 $20 L L32R,  A8 CALLX8,
  A10 $FC L BNEZ,
$11F L:
  A8 $24 L L32R,  A8 CALLX8,
  $131 L J,
$128 L:
  A3 1 $131 L BNEI,
  A8 $28 L L32R,  A8 CALLX8,
$131 L:
  RETW.N,
$133 L:  END-CODE

CODE pm_dream_timeout_process   \ .text.pm_dream_timeout_process $8
  $0 2 LITERALS
    SYMBOL g_pm  SYMBOL pm_enable_sleep_delay_timer
$8 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A9 0 MOVI,  A9 A8 18 S8I,  MEMW,  MEMW,
  A8 $4 L L32R,  A8 CALLX8,
  RETW.N,
$22 L:  END-CODE

CODE pm_sleep_delay_timeout_process   \ .text.pm_sleep_delay_timeout_process $8
  $0 2 LITERALS
    SYMBOL g_pm  SYMBOL pm_sleep
$8 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A10 0 MOVI.N,  A9 A8 $100 ADDMI,  A8 A8 1 L8UI,
  A10 A9 184 S8I,  MEMW,  A8 1 $28 L BNEI,
  MEMW,  A8 $4 L L32R,  A8 CALLX8,
$28 L:
  RETW.N,
$2A L:  END-CODE

CODE pm_disconnected_sleep_dela~E633   \ pm_disconnected_sleep_delay_timeout_process: .text.pm_disconnected_sleep_delay_timeout_process $8
  $0 2 LITERALS
    SYMBOL g_pm  SYMBOL pm_disconnected_sleep
$8 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A9 0 MOVI.N,  A8 A8 $100 ADDMI,  A9 A8 184 S8I,
  MEMW,  A8 A8 29 L8UI,  A8 2 $28 L BNEI,
  MEMW,  A8 $4 L L32R,  A8 CALLX8,
$28 L:
  RETW.N,
$2A L:  END-CODE

CODE pm_connectionless_wake_int~299D   \ pm_connectionless_wake_interval_timeout_process: .wifislpiram.46 $C
  $0 3 LITERALS
    SYMBOL g_pm  SYMBOL g_osi_funcs_p  SYMBOL pm_update_by_connectionles~3F93
$C L:
  A1 32 ENTRY,  A3 $0 L L32R,  A4 0 MOVI,  A2 A3 $100 ADDMI,  A4 A2 140 S8I,  MEMW,
  A2 A3 13 L8UI,  A2 $30 L BNEZ,
$24 L:
  MEMW,  A8 $8 L L32R,  A8 CALLX8,
  $68 L J,
$30 L:
  A8 $4 L L32R,  A8 A8 0 L32I.N,  A10 A8 $110 L32I,  A4 A3 88 L32I,  A2 A3 92 L32I,
  A10 CALLX8,
  A10 A4 A10 SUB,  A8 A3 $18A L16UI,  A3 $3E8 MOVI,  A9 A8 A3 MULL,  A8 A8 A3 MULUH,
  A3 1 MOVI,  A4 A10 $59 L BLTU,
  A3 0 MOVI,
$59 L:
  A11 A2 A11 SUB,  A11 A11 A3 SUB,  A11 A8 $68 L BLTU,
  A8 A11 $24 L BNE,
  A10 A9 $24 L BGEU,
$68 L:
  RETW.N,
$6A L:  END-CODE

CODE pm_connectionless_wake_win~1498   \ pm_connectionless_wake_window_timeout_process: .wifislpiram.47 $18
  $0 6 LITERALS
    SYMBOL g_pm  SYMBOL g_osi_funcs_p  SYMBOL pm_coex_separate_connectio~659C
    SYMBOL pm_get_connectionless_status  SYMBOL wifi_gpio_debug
    SYMBOL pm_wake_done
$18 L:
  A1 32 ENTRY,  A3 $0 L L32R,  A2 A3 $100 ADDMI,  A8 A2 29 L8UI,  A8 2 $2D L BGEUI,
  A3 A3 13 L8UI,  A3 $72 L BEQZ,
$2D L:
  A3 $4 L L32R,  A3 A3 0 L32I.N,  A10 A3 $194 L32I,  A10 CALLX8,
  A3 A10 A10 OR,  A10 $49 L BEQZ,
  A8 $8 L L32R,  A8 CALLX8,
  $72 L J,
  2 ZEROS,
$49 L:
  A8 A2 136 L8UI,  A8 $72 L BEQZ.N,
  A8 $C L L32R,  A8 CALLX8,
  A10 1 $72 L BLTI,
  A11 1 MOVI,  A10 17 MOVI,  A3 A2 136 S8I,  MEMW,  MEMW,  A8 $10 L L32R,
  A8 CALLX8,
  A8 $14 L L32R,  A8 CALLX8,
$72 L:
  RETW.N,
$74 L:  END-CODE

CODE pm_coex_restart_timeout_process   \ .text.pm_coex_restart_timeout_process $C
  $0 3 LITERALS
    SYMBOL g_osi_funcs_p  SYMBOL g_pm  SYMBOL pm_coex_schm_process_restart
$C L:
  A1 32 ENTRY,  A8 $0 L L32R,  A8 A8 0 L32I.N,  A10 A8 $194 L32I,  A10 CALLX8,
  A10 $2A L BEQZ.N,
  A8 $4 L L32R,  A8 A8 $190 L32I,  A8 $2A L BNEZ.N,
  A8 $8 L L32R,  A8 CALLX8,
$2A L:
  RETW.N,
$2C L:  END-CODE

CODE pm_rx_data_process   \ .wifirxiram.51 $1C
  $0 7 LITERALS
    SYMBOL g_pm  SYMBOL pm_check_state  SYMBOL pm_incr_coex_active_cnt
    SYMBOL pm_enable_dream_timer  SYMBOL pm_disable_dream_timer
    SYMBOL pm_enable_sleep_delay_timer  SYMBOL pm_enable_active_timer
$1C L:
  A1 32 ENTRY,  A2 A2 0 8 EXTUI,  A3 A3 0 8 EXTUI,  A8 $4 L L32R,  A8 CALLX8,
  A10 $94 L BEQZ,
  A8 $8 L L32R,  A8 CALLX8,
  A8 $0 L L32R,  A9 A8 1 L8UI,  A9 1 $8C L BNEI,
  A10 A8 24 L8UI,  A10 $94 L BEQZ,
  A2 $71 L BEQZ.N,
  A3 $59 L BEQZ.N,
  A9 A8 18 S8I,  MEMW,  MEMW,  A8 $C L L32R,  A8 CALLX8,
  $94 L J,
$59 L:
  A3 A8 18 S8I,  MEMW,  MEMW,  A8 $10 L L32R,  A8 CALLX8,
  A8 $14 L L32R,  A8 CALLX8,
  $94 L J,
$71 L:
  A3 $80 L BEQZ.N,
  A9 A8 18 S8I,  MEMW,  MEMW,  $94 L J,
  1 ZEROS,
$80 L:
  A3 A8 18 S8I,  MEMW,  MEMW,  $94 L J,
$8C L:
  A9 $94 L BNEZ.N,
  A8 $18 L L32R,  A8 CALLX8,
$94 L:
  RETW.N,
$96 L:  END-CODE

CODE pm_hmac_rx_data_process   \ .wifirxiram.52 $0
  $0 ORIGIN
  A1 32 ENTRY,  RETW.N,
$5 L:  END-CODE

CODE pm_tx_data_done_process   \ .text.pm_tx_data_done_process $34
  $0 13 LITERALS
    SYMBOL g_mesh_is_started  SYMBOL g_pm_cfg  SYMBOL g_pm  SYMBOL g_osi_funcs_p
    SYMBOL pm_reset_keep_alive  SYMBOL sta_reset_beacon_timeout
    SYMBOL pm_enable_max_idle_timer  SYMBOL ppCheckTxIdle
    SYMBOL pm_enable_sleep_delay_timer  SYMBOL pm_enable_disconnected_sle~30EA
    SYMBOL pm_check_state  SYMBOL pm_incr_coex_active_cnt
    SYMBOL pm_enable_active_timer
$34 L:
  A1 32 ENTRY,  A2 A2 0 8 EXTUI,  A3 A3 0 8 EXTUI,  A4 A4 0 8 EXTUI,
  A2 $82 L BEQZ.N,
  A2 $0 L L32R,  A2 A2 0 L8UI,  A2 $4D L BEQZ.N,
  A3 $59 L BNEZ,
$4D L:
  A8 $10 L L32R,  A8 CALLX8,
  A8 $14 L L32R,  A8 CALLX8,
$59 L:
  A2 $4 L L32R,  A2 A2 29 L8UI,  A2 $82 L BEQZ.N,
  A4 $82 L BEQZ.N,
  A2 $8 L L32R,  A8 A2 36 L32I.N,  A8 $71 L BNEZ,
  A8 $18 L L32R,  A8 CALLX8,
$71 L:
  A8 $C L L32R,  A8 A8 0 L32I.N,  A10 A8 $110 L32I,  A10 CALLX8,
  A10 A2 $1E0 S32I,  A11 A2 $1E4 S32I,
$82 L:
  A10 1 MOVI.N,  MEMW,  A8 $1C L L32R,  A8 CALLX8,
  A10 $AE L BNEZ.N,
  A8 $8 L L32R,  A2 A8 13 L8UI,  A2 $A8 L BEQZ,
  A2 A8 1 L8UI,  A2 1 $AE L BNEI,
  A8 $20 L L32R,  A8 CALLX8,
  $AE L J,
  1 ZEROS,
$A8 L:
  A8 $24 L L32R,  A8 CALLX8,
$AE L:
  A8 $28 L L32R,  A8 CALLX8,
  A10 $DE L BEQZ.N,
  A4 $DE L BEQZ.N,
  A8 $2C L L32R,  A8 CALLX8,
  A8 $8 L L32R,  A2 A8 24 L8UI,  A2 $D0 L BNEZ,
  A2 1 MOVI,  A2 A8 25 S8I,  MEMW,
$D0 L:
  A2 A8 1 L8UI,  A2 $DE L BNEZ.N,
  MEMW,  A8 $30 L L32R,  A8 CALLX8,
$DE L:
  RETW.N,
$E0 L:  END-CODE

CODE pm_tx_null_data_done_quick~6418   \ pm_tx_null_data_done_quick_wake_process: .text.pm_tx_null_data_done_quick_wake_process $14
  $0 5 LITERALS
    SYMBOL g_pm  SYMBOL g_osi_funcs_p  SYMBOL sta_reset_beacon_timeout
    SYMBOL ppCheckTxIdle  SYMBOL pm_send_nullfunc
$14 L:
  A1 32 ENTRY,  A3 $0 L L32R,  A2 A2 0 8 EXTUI,  A8 A3 $100 ADDMI,  A8 A8 212 L8UI,
  A8 $2A L BNEZ.N,
$25 L:
  A2 1 MOVI.N,  $72 L J,
$2A L:
  A2 $32 L BEQZ.N,
  A8 $8 L L32R,  A8 CALLX8,
$32 L:
  A2 A3 17 L8UI,  A2 $54 L BEQZ.N,
  A8 0 MOVI.N,  A10 0 MOVI.N,  A8 A3 17 S8I,  MEMW,  A3 $4 L L32R,  A3 A3 0 L32I,
  A3 A3 $1A0 L32I,  MEMW,  A3 CALLX8,
  $72 L J,
  1 ZEROS,
$54 L:
  A2 A3 18 L8UI,  A2 $25 L BNEZ,
  A10 1 MOVI.N,  A8 $C L L32R,  A8 CALLX8,
  A10 $25 L BNEZ,
  A3 A3 20 L8UI,  A3 $72 L BNEZ.N,
  A10 1 MOVI.N,  A8 $10 L L32R,  A8 CALLX8,
$72 L:
  RETW.N,
$74 L:  END-CODE

CODE pm_tx_null_data_done_process   \ .text.pm_tx_null_data_done_process $58
  $0 22 LITERALS
    SYMBOL g_mesh_is_started  SYMBOL g_mesh_is_root  SYMBOL g_mesh_init_ps_type
    SYMBOL g_pm  SYMBOL esp_mesh_quick_funcs  SYMBOL g_pm_cfg
    SYMBOL g_osi_funcs_p  SYMBOL g_pm_cnt  SYMBOL pm_reset_keep_alive
    SYMBOL pm_check_state  SYMBOL pm_dream  SYMBOL pm_enable_max_idle_timer
    SYMBOL pm_tx_null_data_done_quick~6418  SYMBOL pm_set_state
    SYMBOL pm_enable_active_timer  SYMBOL ppProcessWaitingQueue  SYMBOL pm_sleep
    SYMBOL pm_enable_sleep_delay_timer  SYMBOL pm_is_in_wifi_slice_threshold
    SYMBOL pm_send_nullfunc  SYMBOL pm_enable_sleep_delay_timer
    SYMBOL pm_offchan_execute_ready_cb
$58 L:
  A1 32 ENTRY,  A6 $0 L L32R,  A5 A6 0 L8UI,  A4 $C L L32R,  A5 $9C L BEQZ.N,
  A5 $4 L L32R,  MEMW,  A5 A5 0 L8UI,  A5 A5 0 8 EXTUI,  A5 $9C L BNEZ.N,
  A5 $8 L L32R,  A5 A5 0 L32I.N,  A5 $9C L BEQZ.N,
  A3 $8C L BNEZ.N,
$7D L:
  A3 1 MOVI.N,  A8 $20 L L32R,  A8 CALLX8,
  A5 A3 MOV.N,  $A1 L J,
  2 ZEROS,
$8C L:
  A5 $10 L L32R,  A11 A3 MOV.N,  A10 A2 MOV.N,  A5 A5 132 L32I,  A12 A4 9 L8UI,
  A5 CALLX8,
$9C L:
  A3 1 $7D L BEQI,
  A5 0 MOVI.N,
$A1 L:
  A8 A4 14 L8UI,  A8 $B8 L BEQZ.N,
  A9 A4 15 L8UI,  A9 $BD L BNEZ.N,
  A2 $C4 L BNEZ.N,
  A10 15 MOVI.N,  A9 A4 9 L8UI,  A10 A9 $C2 L BLTU,
  $C4 L J,
$B8 L:
  A5 1 MOVI.N,  $C4 L J,
$BD L:
  A5 A9 MOV.N,  $C4 L J,
$C2 L:
  A5 A8 MOV.N,
$C4 L:
  A8 A6 0 L8UI,  A6 1 MOVI.N,  A8 A8 A6 XOR,  A8 A8 0 8 EXTUI,  A8 $D4 L BNEZ,
  A5 $E9 L BEQZ.N,
$D4 L:
  A8 $24 L L32R,  A8 CALLX8,
  A10 $E9 L BNEZ.N,
  A6 A4 13 L8UI,  A6 $E9 L BNEZ.N,
$E1 L:
  A10 0 MOVI.N,  $28C L J,
  3 ZEROS,
$E9 L:
  A6 A4 19 L8UI,  A6 $F5 L BNEZ,
  A8 A4 20 L8UI,  A8 $292 L BEQZ,
$F5 L:
  A5 $1FD L BEQZ,
  A5 A4 1 L8UI,  A5 2 $104 L BNEI,
  A8 $28 L L32R,  A8 CALLX8,
$104 L:
  A5 $14 L L32R,  A5 A5 29 L8UI,  A5 $13C L BEQZ.N,
  A5 A4 $100 ADDMI,  A6 A5 232 L8UI,  A6 $13C L BEQZ.N,
  A6 A4 36 L32I.N,  A6 $121 L BNEZ.N,
  A8 $2C L L32R,  A8 CALLX8,
  $133 L J,
$121 L:
  A6 $18 L L32R,  A6 A6 0 L32I,  A10 A6 $110 L32I,  A10 CALLX8,
  A10 A4 $1E0 S32I,  A11 A4 $1E4 S32I,
$133 L:
  A6 0 MOVI,  A6 A5 232 S8I,  MEMW,
$13C L:
  A2 $1AC L BNEZ,
  A5 A4 19 L8UI,  A2 A4 9 S8I,  MEMW,  A5 $292 L BEQZ,
  A5 A4 $100 ADDMI,  A6 A5 200 L8UI,  A6 $156 L BEQZ.N,
  A2 A5 200 S8I,
$156 L:
  A2 0 MOVI.N,  A2 A4 19 S8I,  MEMW,  A2 A4 24 L8UI,  A2 $23C L BEQZ,
  A2 0 MOVI,  A3 A3 -1 ADDI,  A10 1 MOVI,  A10 A2 A3 MOVNEZ,  MEMW,  A8 $30 L L32R,
  A8 CALLX8,
  A10 A2 $17F L BNE,
  $292 L J,
$17F L:
  A10 A2 MOV.N,  A2 A4 18 S8I,  MEMW,  MEMW,  A8 $34 L L32R,  A8 CALLX8,
  A8 $38 L L32R,  A8 CALLX8,
  A3 $1C L L32R,  A2 A3 32 L32I.N,  A2 A2 1 ADDI.N,  A2 A3 32 S32I.N,  MEMW,
  A8 $3C L L32R,  A8 CALLX8,
  $292 L J,
  1 ZEROS,
$1AC L:
  A2 0 MOVI.N,  A3 A4 20 L8UI,  A2 A4 10 S8I,  MEMW,  A3 $E1 L BEQZ,
  A2 A4 20 S8I,  MEMW,  A2 A4 $100 ADDMI,  A2 A2 212 L8UI,  A2 $1E8 L BEQZ.N,
  A2 $18 L L32R,  A2 A2 0 L32I.N,  A10 A2 $194 L32I,  MEMW,  A10 CALLX8,
  A10 $1DD L BEQZ.N,
  A2 A4 25 L8UI,  A2 $1E8 L BNEZ.N,
$1DD L:
  A8 $40 L L32R,  A8 CALLX8,
  $1EE L J,
  2 ZEROS,
$1E8 L:
  A8 $44 L L32R,  A8 CALLX8,
$1EE L:
  A3 $1C L L32R,  A2 A3 28 L32I.N,  A2 A2 1 ADDI.N,  A2 A3 28 S32I.N,  MEMW,
  $E1 L J,
$1FD L:
  A2 $24C L BNEZ,
  A6 $292 L BEQZ,
  A3 $18 L L32R,  A3 A3 0 L32I,  A10 A3 $110 L32I,  A2 A4 19 S8I,  MEMW,  MEMW,
  A10 CALLX8,
  A12 $271 MOVI,  A12 A12 3 SLLI,  A8 $48 L L32R,  A8 CALLX8,
  A10 $239 L BEQZ.N,
  A3 A4 9 L8UI,  A3 A3 1 ADDI.N,  A3 A4 9 S8I,  MEMW,  MEMW,  $26A L J,
  2 ZEROS,
$239 L:
  A2 A4 9 S8I,
$23C L:
  A2 1 MOVI.N,  A2 A4 25 S8I,  MEMW,  MEMW,  $292 L J,
  2 ZEROS,
$24C L:
  A2 1 $292 L BNEI,
  A3 A4 20 L8UI,  A3 $292 L BEQZ.N,
  A3 A4 24 L8UI,  A5 A4 20 S8I,  MEMW,  A3 $278 L BEQZ.N,
  A3 A4 10 L8UI,  A3 A3 1 ADDI.N,  A3 A4 10 S8I,  MEMW,
$26A L:
  A10 A2 MOV.N,  MEMW,  A8 $4C L L32R,  A8 CALLX8,
  $292 L J,
$278 L:
  A5 A4 10 S8I,  A2 A4 25 S8I,  MEMW,  MEMW,  A8 $50 L L32R,  A8 CALLX8,
  A10 A2 MOV.N,
$28C L:
  A8 $54 L L32R,  A8 CALLX8,
$292 L:
  RETW.N,
$294 L:  END-CODE

CODE pm_go_to_wake   \ .text.pm_go_to_wake $38
  $0 14 LITERALS
    SYMBOL g_pm  SYMBOL g_mesh_is_started  SYMBOL g_mesh_is_root
    SYMBOL g_mesh_init_ps_type  $13 SYMBOL+ g_pm  $14 SYMBOL+ g_pm
    SYMBOL esp_mesh_quick_funcs  SYMBOL g_osi_funcs_p  SYMBOL pm_dream
    SYMBOL ppCheckTxConnTrafficIdle  SYMBOL pm_tx_null_data_done_process
    SYMBOL pm_is_in_wifi_slice_threshold  SYMBOL pm_send_nullfunc
    SYMBOL pm_tx_null_data_done_process
$38 L:
  A1 32 ENTRY,  A4 $0 L L32R,  A5 A2 0 8 EXTUI,  A2 0 MOVI.N,  A3 A3 0 8 EXTUI,
  A8 A4 13 L8UI,  A8 A2 $4F L BNE,
  $12A L J,
$4F L:
  A8 A4 14 L8UI,  A8 A2 $58 L BNE,
  $12A L J,
$58 L:
  A8 A4 15 L8UI,  A8 A2 $61 L BEQ,
  $12A L J,
$61 L:
  A2 A4 24 L8UI,  A2 $78 L BNEZ.N,
$66 L:
  A2 1 MOVI.N,  A2 A4 25 S8I,  MEMW,  A2 1 MOVI.N,  MEMW,  $12A L J,
  2 ZEROS,
$78 L:
  A2 A4 1 L8UI,  A2 2 $84 L BNEI,
  A8 $20 L L32R,  A8 CALLX8,
$84 L:
  A5 $B8 L BEQZ.N,
  A2 $4 L L32R,  A2 A2 0 L8UI,  A2 $B8 L BEQZ.N,
  A2 $8 L L32R,  MEMW,  A10 A2 0 L8UI,  A10 A10 0 8 EXTUI,  A10 $B8 L BNEZ.N,
  A2 $C L L32R,  A2 A2 0 L32I,  A2 $B8 L BEQZ,
  A2 $18 L L32R,  A12 $10 L L32R,  A2 A2 140 L32I,  A11 $14 L L32R,  A2 CALLX8,
  $128 L J,
  1 ZEROS,
$B8 L:
  A2 A4 19 L8UI,  A2 $112 L BNEZ,
  A3 $112 L BNEZ,
  A10 1 MOVI.N,  A8 $24 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  A10 $E5 L BEQZ.N,
  A8 1 MOVI,  A11 1 MOVI,  A10 A3 A3 OR,  A8 A4 19 S8I,  MEMW,  MEMW,
  A8 $28 L L32R,  A8 CALLX8,
$E5 L:
  A3 $1C L L32R,  A3 A3 0 L32I.N,  A10 A3 $110 L32I,  A10 CALLX8,
  A12 $271 MOVI,  A12 A12 3 SLLI,  A8 $2C L L32R,  A8 CALLX8,
  A10 $10C L BEQZ.N,
  A10 0 MOVI.N,  A8 $30 L L32R,  A8 CALLX8,
  A2 3 MOVI.N,  $12A L J,
  1 ZEROS,
$10C L:
  A2 $66 L BEQZ,
  $128 L J,
$112 L:
  A2 1 MOVI.N,  A11 1 MOVI,  A10 0 MOVI.N,  A2 A4 19 S8I,  MEMW,  MEMW,
  A8 $34 L L32R,  A8 CALLX8,
$128 L:
  A2 2 MOVI.N,
$12A L:
  RETW.N,
$12C L:  END-CODE

CODE pm_send_probe_stop   \ .text.pm_send_probe_stop $14
  $0 5 LITERALS
    SYMBOL g_pm  SYMBOL pm_go_to_wake  SYMBOL ppCheckTxConnTrafficIdle
    SYMBOL pm_enable_active_timer  SYMBOL pm_wake_done
$14 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A9 A8 $100 ADDMI,  A10 A9 29 L8UI,
  A10 2 $29 L BGEUI,
  A10 A8 13 L8UI,  A10 $6B L BEQZ,
$29 L:
  A10 A8 15 L8UI,  A10 $6B L BEQZ.N,
  A10 0 MOVI.N,  A10 A8 15 S8I,  MEMW,  A8 A9 28 L8UI,  A8 $4E L BEQZ,
$3C L:
  A11 0 MOVI.N,  A10 A11 MOV.N,  MEMW,  A8 $4 L L32R,  A8 CALLX8,
  $5F L J,
  2 ZEROS,
$4E L:
  A8 A9 200 L8UI,  A8 $3C L BNEZ,
  A10 1 MOVI.N,  A8 $8 L L32R,  A8 CALLX8,
  A10 $3C L BNEZ,
$5F L:
  A8 $C L L32R,  A8 CALLX8,
  A8 $10 L L32R,  A8 CALLX8,
$6B L:
  RETW.N,
$6D L:  END-CODE

CODE pm_on_channel   \ .text.pm_on_channel $1C
  $0 7 LITERALS
    SYMBOL g_pm  SYMBOL pm_wake_done  SYMBOL hal_pm_unblock_txq
    SYMBOL pm_go_to_wake  SYMBOL ppCheckTxConnTrafficIdle  SYMBOL pm_wake_done
    SYMBOL pm_enable_active_timer
$1C L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A8 $100 ADDMI,  A9 A2 29 L8UI,  A9 2 $30 L BGEUI,
  A9 A8 13 L8UI,  A9 $35 L BEQZ.N,
$30 L:
  A9 A8 14 L8UI,  A9 $46 L BEQZ.N,
$35 L:
  A2 1 MOVI.N,  A2 A8 14 S8I,  MEMW,  MEMW,  $AA L J,
  3 ZEROS,
$46 L:
  A9 1 MOVI.N,  A9 A8 14 S8I,  MEMW,  A9 A8 13 L8UI,  A9 $61 L BNEZ.N,
  MEMW,  A8 $4 L L32R,  A8 CALLX8,
  $AA L J,
  2 ZEROS,
$61 L:
  A8 A8 24 L8UI,  A8 $6D L BEQZ,
  A8 $8 L L32R,  A8 CALLX8,
$6D L:
  A8 A2 28 L8UI,  A8 $81 L BEQZ.N,
$72 L:
  A11 0 MOVI.N,  A10 A11 MOV.N,  A8 $C L L32R,  A8 CALLX8,
  A10 $93 L BNEZ.N,
  $9B L J,
$81 L:
  A8 A2 200 L8UI,  A8 $72 L BNEZ,
  A10 1 MOVI,  A8 $10 L L32R,  A8 CALLX8,
  A10 $72 L BNEZ,
$93 L:
  A8 0 MOVI.N,  A8 A2 28 S8I,  MEMW,
$9B L:
  MEMW,  A8 $14 L L32R,  A8 CALLX8,
  A8 $18 L L32R,  A8 CALLX8,
$AA L:
  RETW.N,
$AC L:  END-CODE

CODE pm_update_params   \ .text.pm_update_params $14
  $0 5 LITERALS
    SYMBOL g_osi_funcs_p  SYMBOL g_pm  SYMBOL g_pm_cfg
    SYMBOL pm_coex_schm_overall_period_get  SYMBOL pm_go_to_wake
$14 L:
  A1 32 ENTRY,  A4 $0 L L32R,  A2 A4 0 L32I.N,  A10 A2 $194 L32I,  A10 CALLX8,
  A2 $4 L L32R,  A3 $8 L L32R,  A10 $38 L BNEZ.N,
  A8 A3 20 L32I.N,  A3 A3 4 L32I.N,  A8 A2 48 S32I.N,  MEMW,  $54 L J,
  2 ZEROS,
$38 L:
  A8 A2 $100 ADDMI,  A8 A8 213 L8UI,  A8 $45 L BEQZ.N,
  A10 A3 20 L32I.N,  $50 L J,
$45 L:
  A8 $C L L32R,  A8 CALLX8,
  A8 A2 44 L32I.N,  A10 A10 A8 MULL,
$50 L:
  A3 A3 8 L32I.N,  A10 A2 48 S32I.N,
$54 L:
  A8 A2 40 L32I,  A8 A3 $5D L BEQ,
  A3 A2 40 S32I,
$5D L:
  A3 A4 0 L32I.N,  A10 A3 $194 L32I,  MEMW,  A10 CALLX8,
  A3 A2 12 L8UI,  A10 A3 $97 L BEQ,
  A3 1 $89 L BNEI,
  A10 A2 36 L32I.N,  A10 $89 L BNEZ.N,
  A8 A2 $100 ADDMI,  A11 A10 MOV.N,  A3 A8 200 S8I,  MEMW,  MEMW,  A8 $10 L L32R,
  A8 CALLX8,
$89 L:
  A3 A4 0 L32I.N,  A10 A3 $194 L32I,  A10 CALLX8,
  A10 A2 12 S8I,  MEMW,
$97 L:
  MEMW,  RETW.N,
$9C L:  END-CODE

CODE pm_coex_tbtt_process   \ .text.pm_coex_tbtt_process $30
  $0 12 LITERALS
    $F4 SYMBOL+ g_pm  SYMBOL g_osi_funcs_p  SYMBOL g_pm  $51EB851F T,
    SYMBOL pm_coex_schm_overall_period_get  SYMBOL pm_coex_schm_process_restart
    SYMBOL pm_is_sleeping  SYMBOL hal_pm_unblock_txq
    SYMBOL ppCheckTxConnTrafficIdle  SYMBOL pm_is_sleeping
    SYMBOL hal_pm_unblock_txq  SYMBOL pm_go_to_wake
$30 L:
  A1 32 ENTRY,  A5 $4 L L32R,  A4 A5 0 L32I,  A10 $0 L L32R,  A4 A4 236 L32I,
  A4 CALLX8,
  A8 1 MOVI.N,  A4 $8 L L32R,  A8 A4 24 S8I,  MEMW,  A8 A5 0 L32I.N,
  A10 A8 $194 L32I,  MEMW,  A10 CALLX8,
  A10 $C8 L BEQZ,
  A8 A4 14 L8UI,  A8 $87 L BEQZ.N,
  A8 $10 L L32R,  A8 CALLX8,
  A9 1 MOVI.N,  A8 A4 44 L32I.N,  A10 A10 A8 MULL,  A2 A10 A2 ADD.N,
  A2 A10 $74 L BLTU,
  A9 0 MOVI.N,
$74 L:
  A9 A9 A3 ADD.N,  A2 A4 104 S32I,  A2 A4 14 L8UI,  A9 A4 108 S32I,
  A2 $A0 L BNEZ.N,
  MEMW,  $106 L J,
$87 L:
  A9 A4 44 L32I.N,  A5 1 MOVI.N,  A2 A9 A2 ADD.N,  A2 A9 $92 L BLTU,
  A5 A8 MOV.N,
$92 L:
  A5 A5 A3 ADD.N,  A2 A4 104 S32I,  A5 A4 108 S32I,  MEMW,  $106 L J,
$A0 L:
  A2 $C L L32R,  A10 A8 A2 MULUH,  A10 A10 5 SRLI,  A2 A5 0 L32I.N,
  A2 A2 $1B8 L32I,  A2 CALLX8,
  A8 $14 L L32R,  A8 CALLX8,
  A11 0 MOVI.N,  A2 A5 0 L32I.N,  A10 A11 MOV.N,  A2 A2 $19C L32I,
  A12 A4 52 L32I.N,  A2 CALLX8,
  $106 L J,
$C8 L:
  A8 $18 L L32R,  A8 CALLX8,
  A10 $14D L BNEZ,
  A2 A4 13 L8UI,  A2 $E5 L BEQZ.N,
  A2 A4 14 L8UI,  A2 $14D L BEQZ,
  A2 A4 $200 ADDMI,  A2 A2 30 L8UI,  A2 $14D L BNEZ,
$E5 L:
  A8 $1C L L32R,  A8 CALLX8,
  A10 1 MOVI,  A8 $20 L L32R,  A8 CALLX8,
  A10 $14D L BEQZ,
  A2 A4 14 L8UI,  A11 1 MOVI.N,  A10 0 MOVI.N,  A2 $144 L BNEZ,
  $14D L J,
  2 ZEROS,
$106 L:
  A2 0 MOVI.N,  A2 A4 64 S32I,  MEMW,  A8 $24 L L32R,  A8 CALLX8,
  A10 A2 $12F L BNE,
  A2 A4 13 L8UI,  A2 $129 L BEQZ.N,
  A2 A4 14 L8UI,  A2 $12F L BEQZ.N,
  A2 A4 $200 ADDMI,  A2 A2 30 L8UI,  A2 $12F L BNEZ.N,
$129 L:
  A8 $28 L L32R,  A8 CALLX8,
$12F L:
  A2 A4 36 L32I.N,  A2 $138 L BEQZ.N,
  A2 A4 25 L8UI,  A2 $14D L BEQZ.N,
$138 L:
  A2 0 MOVI.N,  A11 0 MOVI.N,  A2 A4 25 S8I,  MEMW,  A10 1 MOVI.N,
$144 L:
  MEMW,  A8 $2C L L32R,  A8 CALLX8,
$14D L:
  RETW.N,
$14F L:  END-CODE

CODE pm_active_timeout_process   \ .text.pm_active_timeout_process $1C
  $0 7 LITERALS
    SYMBOL g_pm_cnt  SYMBOL g_pm  SYMBOL g_pm_cfg  SYMBOL pm_go_to_wake
    SYMBOL pm_enable_active_timer  SYMBOL ppCheckTxIdle  SYMBOL pm_go_to_sleep
$1C L:
  A1 32 ENTRY,  A9 $0 L L32R,  A8 A9 20 L32I.N,  A8 A8 1 ADDI.N,  A8 A9 20 S32I.N,
  A8 $4 L L32R,  A9 A8 13 L8UI,  A9 $85 L BEQZ,
  A9 A8 14 L8UI,  A9 $85 L BEQZ,
  A9 A8 15 L8UI,  A9 $85 L BNEZ,
  A9 $8 L L32R,  A9 A9 29 L8UI,  A10 A8 36 L32I.N,  A9 $52 L BEQZ.N,
  A10 $60 L BNEZ.N,
  MEMW,  $85 L J,
  3 ZEROS,
$52 L:
  A10 $60 L BNEZ.N,
  A11 A10 MOV.N,  A8 $C L L32R,  A8 CALLX8,
  $6A L J,
  1 ZEROS,
$60 L:
  A9 A8 1 L8UI,  A9 $85 L BNEZ.N,
  A8 A8 18 L8UI,  A8 $74 L BEQZ.N,
$6A L:
  A8 $10 L L32R,  A8 CALLX8,
  $85 L J,
  1 ZEROS,
$74 L:
  A10 1 MOVI.N,  A8 $14 L L32R,  A8 CALLX8,
  A10 $6A L BNEZ,
  A8 $18 L L32R,  A8 CALLX8,
$85 L:
  RETW.N,
$87 L:  END-CODE

CODE pm_process_tim   \ .wifislprxiram.43 $3C
  $0 15 LITERALS
    SYMBOL g_pm  SYMBOL g_osi_funcs_p  SYMBOL g_pm_cnt  SYMBOL g_pm_cfg
    SYMBOL pm_keep_alive  SYMBOL pm_is_in_wifi_slice_threshold
    SYMBOL pm_go_to_wake  SYMBOL pm_is_in_wifi_slice_threshold
    SYMBOL pm_enable_dream_timer  SYMBOL pm_send_nullfunc
    SYMBOL pm_disable_sleep_delay_timer  SYMBOL pm_sleep
    SYMBOL pm_disable_active_timer  SYMBOL pm_active_timeout_process
    SYMBOL pm_send_nullfunc
$3C L:
  A1 48 ENTRY,  A8 $10 L L32R,  A8 CALLX8,
  A4 $0 L L32R,  A2 A2 0 8 EXTUI,  A3 A3 0 8 EXTUI,  A5 A10 A10 OR,  A7 A4 1 L8UI,
  A7 1 $5A L BEQI,
  $1BA L J,
$5A L:
  A2 $C9 L BEQZ,
  A5 $4 L L32R,  A2 A5 0 L32I,  A10 A2 $110 L32I,  A10 CALLX8,
  A12 $271 MOVI,  A12 A12 3 SLLI,  A2 A10 MOV.N,  A3 A11 MOV.N,  A8 $14 L L32R,
  A8 CALLX8,
  A10 $BC L BEQZ.N,
  A12 A4 96 L32I,  A7 A4 18 S8I,  MEMW,  A4 A4 100 L32I,  A3 A4 $90 L BLTU,
  A4 A3 $A2 L BNE,
  A2 A12 $A2 L BGEU,
$90 L:
  A3 A5 0 L32I.N,  A12 A12 A2 SUB,  A11 0 MOVI.N,  A10 1 MOVI.N,  A3 A3 $19C L32I,
  MEMW,  A3 CALLX8,
$A2 L:
  A11 0 MOVI.N,  A10 1 MOVI.N,  A8 $18 L L32R,  A8 CALLX8,
  A3 $8 L L32R,  A2 A3 8 L32I.N,  A2 A2 1 ADDI.N,  A2 A3 8 S32I.N,  MEMW,  $208 L J,
  1 ZEROS,
$BC L:
  A7 A4 25 S8I,  MEMW,  MEMW,  $208 L J,
  1 ZEROS,
$C9 L:
  A3 $17B L BEQZ,
  A2 $4 L L32R,  A3 A2 0 L32I,  A10 A3 $110 L32I,  A10 CALLX8,
  A12 $3E8 MOVI,  A6 A10 MOV.N,  A2 A11 MOV.N,  A10 A1 0 S32I.N,  A3 A11 A11 OR,
  MEMW,  A8 $1C L L32R,  A8 CALLX8,
  A10 $208 L BEQZ,
  A7 A4 18 S8I,  MEMW,  MEMW,  A8 $20 L L32R,  A8 CALLX8,
  A8 A4 100 L32I,  A7 A4 96 L32I,  A2 A8 $110 L BLTU,
  A8 A2 $13C L BNE,
  A3 A1 0 L32I.N,  A3 A7 $13C L BGEU,
$110 L:
  A9 A7 A6 SUB,  A3 0 MOVI.N,  A12 A4 52 L32I,  A6 1 MOVI,  A7 A9 $121 L BLTU,
  A6 A3 A3 OR,
$121 L:
  A8 A8 A2 SUB,  A8 A8 A6 SUB,  A3 A8 $12D L BNE,
  A12 A12 A9 MINU,
$12D L:
  A3 $4 L L32R,  A11 0 MOVI.N,  A10 2 MOVI.N,  A2 A3 0 L32I.N,  A2 A2 $19C L32I,
  A2 CALLX8,
$13C L:
  A2 A4 19 L8UI,  A2 $16C L BNEZ.N,
  A3 1 MOVI.N,  A2 A4 20 L8UI,  A2 A2 A3 XOR,  A2 A2 0 8 EXTUI,  A2 $16C L BEQZ.N,
  A5 $16C L BEQZ.N,
  A2 $C L L32R,  A2 A2 29 L8UI,  A2 $161 L BEQZ.N,
  A4 A4 $100 ADDMI,  A3 A4 232 S8I,  MEMW,
$161 L:
  A10 1 MOVI.N,  MEMW,  A8 $24 L L32R,  A8 CALLX8,
$16C L:
  A3 $8 L L32R,  A2 A3 12 L32I.N,  A2 A2 1 ADDI.N,  A2 A3 12 S32I.N,  MEMW,
  $208 L J,
$17B L:
  A2 A4 19 L8UI,  A3 A4 18 S8I,  MEMW,  A2 $208 L BNEZ,
  A2 A4 20 L8UI,  A2 $208 L BNEZ,
  MEMW,  A8 $28 L L32R,  A8 CALLX8,
  A5 $1B1 L BEQZ.N,
  A2 $C L L32R,  A2 A2 29 L8UI,  A2 $1A9 L BEQZ.N,
  A4 A4 $100 ADDMI,  A7 A4 232 S8I,  MEMW,
$1A9 L:
  A10 1 MOVI.N,  MEMW,  $1FF L J,
$1B1 L:
  A8 $2C L L32R,  A8 CALLX8,
  $208 L J,
$1BA L:
  A6 1 MOVI.N,  A8 0 MOVI.N,  A6 A8 A7 MOVEQZ,  A3 A3 A6 OR,  A3 A8 $208 L BNE,
  A2 A8 $208 L BNE,
  A2 A4 18 S8I,  MEMW,  A4 A4 $100 ADDMI,  A3 A4 212 L8UI,  A3 A8 $1EA L BEQ,
  MEMW,  A8 $30 L L32R,  A8 CALLX8,
  A10 A2 MOV.N,  A8 $34 L L32R,  A8 CALLX8,
$1EA L:
  A5 $208 L BEQZ.N,
  A2 $C L L32R,  A2 A2 29 L8UI,  A2 $1FD L BEQZ.N,
  A2 1 MOVI,  A2 A4 232 S8I,  MEMW,
$1FD L:
  A10 0 MOVI.N,
$1FF L:
  MEMW,  A8 $38 L L32R,  A8 CALLX8,
$208 L:
  RETW.N,
$20A L:  END-CODE

CODE pm_rx_beacon_process   \ .wifislprxiram.49 $18
  $0 6 LITERALS
    SYMBOL g_pm  SYMBOL g_osi_funcs_p  SYMBOL g_pm_cfg  SYMBOL wifi_gpio_debug
    SYMBOL sta_reset_beacon_timeout  SYMBOL pm_process_tim
$18 L:
  A1 32 ENTRY,  A6 $0 L L32R,  A4 0 MOVI.N,  A5 A6 $200 ADDMI,  A3 A3 0 8 EXTUI,
  A4 A5 29 S8I,  A4 A6 17 S8I,  MEMW,  A4 A6 14 L8UI,  A5 $4 L L32R,
  A4 $3E L BNEZ.N,
  MEMW,  $AE L J,
  1 ZEROS,
$3E L:
  A11 2 MOVI.N,  A10 1 MOVI.N,  A8 $C L L32R,  A8 CALLX8,
  A3 $59 L BEQZ.N,
  A4 0 MOVI.N,  A8 $10 L L32R,  A8 CALLX8,
  A3 A4 MOV.N,  $A4 L J,
  2 ZEROS,
$59 L:
  A2 $A4 L BEQZ,
  A11 A6 26 L16UI,  A4 1 MOVI.N,  A8 A11 0 14 EXTUI,  A10 A2 4 L8UI,
  A8 $99 L BEQZ.N,
  A4 -2 MOVI.N,  A6 A10 A4 AND,  A8 A8 3 SRLI,  A4 A3 MOV.N,  A8 A6 $99 L BLTU,
  A9 A2 1 L8UI,  A9 A9 -4 ADDI,  A9 A6 A9 ADD.N,  A9 A9 0 8 EXTUI,
  A9 A8 $99 L BLTU,
  A8 A8 A6 SUB,  A2 A2 A8 ADD,  A4 A2 5 L8UI,  A11 A11 0 3 EXTUI,  A11 SSR,
  A4 A4 SRA,  A4 A4 0 1 EXTUI,
$99 L:
  A2 $8 L L32R,  A2 A2 28 L8UI,  A2 $A4 L BNEZ.N,
  A3 A10 0 1 EXTUI,
$A4 L:
  A11 A3 MOV.N,  A10 A4 MOV.N,  A8 $14 L L32R,  A8 CALLX8,
$AE L:
  A2 A5 0 L32I.N,  A10 0 MOVI.N,  A2 A2 $1A0 L32I,  A2 CALLX8,
  RETW.N,
$BA L:  END-CODE

CODE pm_max_idle_timeout_process   \ .text.pm_max_idle_timeout_process $14
  $0 5 LITERALS
    SYMBOL g_pm  SYMBOL pm_enable_max_idle_timer  SYMBOL pm_go_to_wake
    SYMBOL pm_dream  SYMBOL pm_send_nullfunc
$14 L:
  A1 32 ENTRY,  A8 $4 L L32R,  A8 CALLX8,
  A8 $0 L L32R,  A9 A8 13 L8UI,  A9 $62 L BEQZ.N,
  A9 A8 14 L8UI,  A9 $62 L BEQZ.N,
  A9 A8 15 L8UI,  A9 $62 L BNEZ.N,
  A10 1 MOVI.N,  A9 A8 $100 ADDMI,  A10 A9 232 S8I,  MEMW,  A10 A8 36 L32I.N,
  A10 $4C L BNEZ.N,
  A11 A10 MOV.N,  MEMW,  A8 $8 L L32R,  A8 CALLX8,
  $62 L J,
$4C L:
  A10 A8 1 L8UI,  A10 $54 L BNEZ.N,
  $5C L J,
$54 L:
  A8 $C L L32R,  A8 CALLX8,
  A10 1 MOVI.N,
$5C L:
  A8 $10 L L32R,  A8 CALLX8,
$62 L:
  RETW.N,
$64 L:  END-CODE

CODE pm_tx_data_process   \ .wifiextrairam.50 $50
  $0 20 LITERALS
    SYMBOL g_mesh_is_started  SYMBOL g_mesh_is_root  SYMBOL g_mesh_init_ps_type
    SYMBOL esp_mesh_quick_funcs  SYMBOL g_pm  SYMBOL g_osi_funcs_p
    $200 SYMBOL+ g_pm  SYMBOL g_pm_cnt  SYMBOL pm_check_state
    SYMBOL pm_check_state  SYMBOL pm_is_in_wifi_slice_threshold
    SYMBOL pm_disconnected_wake  SYMBOL pm_coex_schm_process_restart
    SYMBOL pm_is_in_wifi_slice_threshold  SYMBOL pm_go_to_wake
    SYMBOL pm_is_in_wifi_slice_threshold  SYMBOL pm_dream  SYMBOL pm_go_to_wake
    SYMBOL pm_enable_active_timer  SYMBOL pm_incr_coex_active_cnt
$50 L:
  A1 48 ENTRY,  A6 A2 0 8 EXTUI,  A2 0 MOVI.N,  A2 A1 0 S8I,  MEMW,
  A3 A3 0 8 EXTUI,  A2 $0 L L32R,  A5 A2 0 L8UI,  A5 $91 L BEQZ.N,
  A2 $4 L L32R,  MEMW,  A4 A2 0 L8UI,  A4 A4 0 8 EXTUI,  A4 $91 L BNEZ.N,
  A2 $8 L L32R,  A2 A2 0 L32I.N,  A2 $91 L BEQZ.N,
  A10 A1 MOV.N,  A2 $C L L32R,  A2 A2 128 L32I,  MEMW,  A2 CALLX8,
  $24A L J,
  2 ZEROS,
$91 L:
  A4 $10 L L32R,  A2 0 MOVI.N,  A7 A4 2 L8UI,  A7 A6 $9F L BEQ,
  $24D L J,
$9F L:
  A5 A2 $AE L BEQ,
  A8 $20 L L32R,  A8 CALLX8,
  A10 A2 $B4 L BNE,
  $24A L J,
$AE L:
  A8 $24 L L32R,  A8 CALLX8,
$B4 L:
  A2 A4 13 L8UI,  A2 $145 L BNEZ,
  A6 $14 L L32R,  A2 A6 0 L32I.N,  A10 A2 $110 L32I,  A10 CALLX8,
  A12 $271 MOVI,  A12 A12 3 SLLI,  A2 A10 MOV.N,  A5 A11 MOV.N,  A8 $28 L L32R,
  A8 CALLX8,
  A12 A4 96 L32I,  A8 A4 100 L32I,  A10 $101 L BEQZ.N,
  A5 A8 $E6 L BLTU,
  A8 A5 $F5 L BNE,
  A2 A12 $F5 L BGEU,
$E6 L:
  A3 A6 0 L32I.N,  A12 A12 A2 SUB,  A11 0 MOVI.N,  A10 1 MOVI.N,  A3 A3 $19C L32I,
  A3 CALLX8,
$F5 L:
  A8 $2C L L32R,  A8 CALLX8,
  $24A L J,
  3 ZEROS,
$101 L:
  A3 A4 $1B0 L32I,  A3 A12 A3 ADD.N,  A7 A4 $1B4 L32I,  A4 1 MOVI.N,
  A3 A12 $110 L BLTU,
  A4 A10 MOV.N,
$110 L:
  A8 A8 A7 ADD.N,  A4 A4 A8 ADD.N,  A5 A4 $11D L BLTU,
  A4 A5 $13C L BNE,
  A2 A3 $13C L BGEU,
$11D L:
  A5 A6 0 L32I.N,  A4 $18 L L32R,  A10 A4 MOV.N,  A5 A5 236 L32I,  A5 CALLX8,
  A12 0 MOVI.N,  A11 A3 A2 SUB,  A5 A6 0 L32I.N,  A10 A4 MOV.N,  A5 A5 248 L32I,
  A5 CALLX8,
  $24A L J,
$13C L:
  A8 $30 L L32R,  A8 CALLX8,
  $24A L J,
$145 L:
  A2 A4 1 L8UI,  A2 1 $1B4 L BNEI,
  A7 $14 L L32R,  A5 A7 0 L32I.N,  A10 A5 $110 L32I,  A10 CALLX8,
  A12 $271 MOVI,  A12 A12 3 SLLI,  A5 A10 MOV.N,  A6 A11 MOV.N,  A8 $34 L L32R,
  A8 CALLX8,
  A10 $1A1 L BEQZ.N,
  A2 A4 100 L32I,  A12 A4 96 L32I,  A6 A2 $177 L BLTU,
  A2 A6 $18C L BNE,
  A5 A12 $18C L BGEU,
$177 L:
  A2 A4 14 L8UI,  A2 $18C L BEQZ.N,
  A12 A12 A5 SUB,  A11 0 MOVI.N,  A2 A7 0 L32I.N,  A10 1 MOVI,  A2 A2 $19C L32I,
  A2 CALLX8,
$18C L:
  A3 $24A L BEQZ,
  A11 1 MOVI.N,  A10 0 MOVI.N,  A8 $38 L L32R,  A8 CALLX8,
  A2 1 MOVI.N,  A10 2 $1A7 L BLTUI,
  $244 L J,
$1A1 L:
  A3 $24A L BEQZ,
  A2 A4 25 S8I,
$1A7 L:
  A2 A1 0 S8I,  MEMW,  MEMW,  $244 L J,
  1 ZEROS,
$1B4 L:
  A2 2 $22E L BNEI,
  A6 $14 L L32R,  A2 A6 0 L32I.N,  A10 A2 $110 L32I,  A10 CALLX8,
  A12 $271 MOVI,  A12 A12 3 SLLI,  A2 A10 MOV.N,  A5 A11 MOV.N,  A8 $3C L L32R,
  A8 CALLX8,
  A10 $212 L BEQZ.N,
  A8 A4 100 L32I,  A12 A4 96 L32I,  A5 A8 $1E3 L BLTU,
  A8 A5 $1F8 L BNE,
  A2 A12 $1F8 L BGEU,
$1E3 L:
  A4 A4 14 L8UI,  A4 $1F8 L BEQZ.N,
  A12 A12 A2 SUB,  A11 0 MOVI.N,  A4 A6 0 L32I.N,  A10 1 MOVI,  A4 A4 $19C L32I,
  A4 CALLX8,
$1F8 L:
  A8 $40 L L32R,  A8 CALLX8,
  A3 $21F L BEQZ.N,
  A11 1 MOVI.N,  A10 0 MOVI.N,  A8 $44 L L32R,  A8 CALLX8,
  A2 1 MOVI.N,  A10 2 $219 L BLTUI,
  $21F L J,
$212 L:
  A3 $21F L BEQZ.N,
  A2 1 MOVI.N,  A2 A4 25 S8I,
$219 L:
  A2 A1 0 S8I,  MEMW,
$21F L:
  A4 $1C L L32R,  A2 A4 24 L32I.N,  A2 A2 1 ADDI.N,  A2 A4 24 S32I.N,  MEMW,
  $242 L J,
$22E L:
  A2 $242 L BNEZ.N,
  A3 $24A L BEQZ.N,
  A2 A4 36 L32I.N,  A2 $244 L BEQZ.N,
  A8 $48 L L32R,  A8 CALLX8,
  $244 L J,
  3 ZEROS,
$242 L:
  A3 $24A L BEQZ.N,
$244 L:
  A8 $4C L L32R,  A8 CALLX8,
$24A L:
  A2 A1 0 L8UI,
$24D L:
  RETW.N,
$24F L:  END-CODE

CODE pm_send_sleep_null_cb   \ .text.pm_send_sleep_null_cb $4
  $0 1 LITERALS
    SYMBOL pm_tx_null_data_done_process
$4 L:
  A1 32 ENTRY,  A8 A2 44 L32I.N,  A10 1 MOVI.N,  A11 A8 19 L8UI,  A8 $0 L L32R,
  A8 CALLX8,
  RETW.N,
$16 L:  END-CODE

CODE pm_send_wake_null_cb   \ .text.pm_send_wake_null_cb $4
  $0 1 LITERALS
    SYMBOL pm_tx_null_data_done_process
$4 L:
  A1 32 ENTRY,  A8 A2 44 L32I.N,  A10 0 MOVI.N,  A11 A8 19 L8UI,  A8 $0 L L32R,
  A8 CALLX8,
  RETW.N,
$16 L:  END-CODE

CODE pm_on_coex_schm_process_restart   \ .text.pm_on_coex_schm_process_restart $4
  $0 1 LITERALS
    SYMBOL pm_coex_schm_process_restart
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$F L:  END-CODE

CODE pm_on_coex_schm_status_config   \ .text.pm_on_coex_schm_status_config $14
  $0 5 LITERALS
    $FFFF T,  SYMBOL g_osi_funcs_p  SYMBOL g_pm  $51EB851F T,
    SYMBOL pm_get_connectionless_status
$14 L:
  A1 32 ENTRY,  A2 $28 L BNEZ,
  A8 $10 L L32R,  A8 CALLX8,
  A3 1 MOVI.N,  A2 A3 A10 MOVNEZ,  A2 A2 6 SLLI,
$28 L:
  A3 $4 L L32R,  A10 0 MOVI.N,  A8 A3 0 L32I.N,  A11 $0 L L32R,  A8 A8 $1B0 L32I,
  A8 CALLX8,
  A11 A2 MOV.N,  A10 0 MOVI.N,  A8 A3 0 L32I.N,  A8 A8 $1B4 L32I,  A8 CALLX8,
  A9 -65 MOVI,  A8 $8 L L32R,  A2 A8 $190 S32I,  A2 A9 $60 L BANY,
  A10 $3E8 MOVI,  A2 A3 0 L32I.N,  A2 A2 $1B8 L32I,  MEMW,  $9F L J,
  2 ZEROS,
$60 L:
  A2 1 $70 L BNEI,
  A2 A3 0 L32I.N,  A10 100 MOVI,  A2 A2 $1B8 L32I,  $9F L J,
  2 ZEROS,
$70 L:
  A2 2 $8C L BNEI,
  A8 A8 $100 ADDMI,  A10 A8 201 L8UI,  A8 100 MOVI,  A2 A3 0 L32I.N,  A3 240 MOVI,
  A3 A8 A10 MOVNEZ,  A10 A3 MOV.N,  A2 A2 $1B8 L32I,  $9F L J,
$8C L:
  A2 4 $A2 L BNEI,
  A2 $C L L32R,  A10 A8 44 L32I.N,  A10 A10 A2 MULUH,  A10 A10 5 SRLI,
  A2 A3 0 L32I.N,  A2 A2 $1B8 L32I,
$9F L:
  A2 CALLX8,
$A2 L:
  RETW.N,
$A4 L:  END-CODE

CODE pm_on_coex_start   \ .text.pm_on_coex_start $18
  $0 6 LITERALS
    SYMBOL g_pm  SYMBOL rodata_wlog_info.53  SYMBOL pm_on_coex_schm_status_config
    SYMBOL pm_coex_schm_process_restart  SYMBOL pm_update_by_connectionles~3F93
    SYMBOL wifi_log
$18 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A10 A2 $190 L32I,  A10 $29 L BNEZ.N,
  A8 $8 L L32R,  A8 CALLX8,
$29 L:
  A8 $C L L32R,  A8 CALLX8,
  A8 $10 L L32R,  A8 CALLX8,
  A3 A2 36 L32I.N,  A3 $56 L BNEZ.N,
  A11 1 MOVI.N,  A12 3 MOVI,  A13 $4 L L32R,  A11 A11 12 SLLI,  A10 6 MOVI,
  A2 A2 $100 ADDMI,  A8 $14 L L32R,  A8 CALLX8,
  A3 A2 200 S8I,  MEMW,
$56 L:
  MEMW,  RETW.N,
$5B L:  END-CODE

CODE pm_on_data_rx   \ .wifirxiram.55 $8
  $0 2 LITERALS
    SYMBOL pm_sleep_for  SYMBOL pm_rx_data_process
$8 L:
  A1 32 ENTRY,  A10 A2 MOV.N,  A5 A5 0 8 EXTUI,  A8 $0 L L32R,  A8 CALLX8,
  A10 $28 L BEQZ,
  A10 A4 0 L8UI,  A11 A5 5 1 EXTUI,  A10 A10 0 1 EXTUI,  A8 $4 L L32R,  A8 CALLX8,
$28 L:
  RETW.N,
$2A L:  END-CODE

CODE pm_on_data_tx   \ .wifiextrairam.56 $4
  $0 1 LITERALS
    SYMBOL pm_tx_data_process
$4 L:
  A1 32 ENTRY,  A11 A3 0 8 EXTUI,  A10 A2 0 8 EXTUI,  A8 $0 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  RETW.N,
$17 L:  END-CODE

CODE pm_on_data_tx_done   \ .text.pm_on_data_tx_done $4
  $0 1 LITERALS
    SYMBOL pm_tx_data_done_process
$4 L:
  A1 32 ENTRY,  A12 A4 0 8 EXTUI,  A11 A3 0 8 EXTUI,  A10 A2 0 8 EXTUI,
  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$18 L:  END-CODE

CODE pm_on_sample_beacon   \ .text.pm_on_sample_beacon $8
  $0 2 LITERALS
    SYMBOL g_pm  SYMBOL ptr_beacon_offset_funcs
$8 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A8 13 L8UI,  A2 $35 L BEQZ.N,
  A8 A8 $100 ADDMI,  A2 A8 100 L8UI,  A2 $35 L BEQZ.N,
  A2 A8 212 L8UI,  A2 $35 L BNEZ.N,
  A2 $4 L L32R,  A8 A2 0 L32I.N,  A8 A8 0 L32I.N,  A8 $2C L BEQZ.N,
  A8 CALLX8,
$2C L:
  A2 A2 0 L32I.N,  A10 A2 12 L32I.N,  A10 $35 L BEQZ.N,
  A10 CALLX8,
$35 L:
  RETW.N,
$37 L:  END-CODE

CODE pm_scale_listen_interval   \ .wifislprxiram.5 $1C
  $0 7 LITERALS
    SYMBOL g_pm  SYMBOL rodata_wlog_info.6  SYMBOL wifi_nvs_get_sta_listen_in~409E
    SYMBOL wifi_nvs_get_sta_listen_in~409E  SYMBOL wifi_log
    SYMBOL pm_coex_pwr_update  SYMBOL pm_on_sample_beacon
$1C L:
  A1 48 ENTRY,  A8 $8 L L32R,  A8 CALLX8,
  A2 $0 L L32R,  A10 A10 A10 ADDX4,  A8 A10 A10 ADDX4,  A8 A8 12 SLLI,  A4 1 MOVI.N,
  A3 A2 44 L32I.N,  A8 A8 A3 QUOU,  A8 A8 0 16 EXTUI,  A8 A8 A4 MAXU,
  A5 A2 11 L8UI,  A8 A5 $50 L BLTU,
  $6B L J,
$47 L:
  A9 A9 1 ADDI.N,  A9 A9 0 16 EXTUI,  $52 L J,
  1 ZEROS,
$50 L:
  A9 A8 MOV.N,
$52 L:
  A4 A5 A9 REMS,  A4 $47 L BNEZ,
  A4 A8 MOV.N,  $62 L J,
$5D L:
  A4 A4 -1 ADDI.N,  A4 A4 0 16 EXTUI,
$62 L:
  A10 A5 A4 REMS,  A10 $5D L BNEZ,
  $79 L J,
$6B L:
  A4 A8 A5 REMS,  A4 A8 A4 SUB,  A4 A4 0 16 EXTUI,  A9 A5 A4 ADD.N,
  A9 A9 0 16 EXTUI,
$79 L:
  A10 A9 A8 SUB,  A8 A8 A4 SUB,  A8 A10 $84 L BLT,
  A4 A9 MOV.N,
$84 L:
  A8 A2 32 L16UI,  A8 A4 $C3 L BEQ,
  A8 $C L L32R,  A8 CALLX8,
  A10 A10 A10 ADDX4,  A8 A2 44 L32I.N,  A10 A10 A10 ADDX4,  A8 A4 A8 MULL,
  A10 A10 12 SLLI,  A11 1 MOVI.N,  A13 $4 L L32R,  A15 A3 MOV.N,  A14 A5 MOV.N,
  A12 3 MOVI.N,  A11 A11 12 SLLI,  A10 A1 4 S32I.N,  A10 6 MOVI.N,  A8 A1 8 S32I.N,
  A4 A1 0 S32I.N,  MEMW,  A8 $10 L L32R,  A8 CALLX8,
  A4 A2 32 S16I,  MEMW,
$C3 L:
  MEMW,  A8 $14 L L32R,  A8 CALLX8,
  A8 $18 L L32R,  A8 CALLX8,
  RETW.N,
$D4 L:  END-CODE

CODE pm_parse_beacon   \ .wifislprxiram.9 $60
  $0 24 LITERALS
    SYMBOL g_pm  SYMBOL rodata_wlog_info.10  SYMBOL rodata_wlog_info.11
    SYMBOL rodata_wlog_info.12  SYMBOL rodata_wlog_info.13
    SYMBOL rodata_wlog_info.14  SYMBOL rodata_wlog_info.15
    SYMBOL rodata_wlog_info.16  SYMBOL rodata_wlog_info.17  SYMBOL g_osi_funcs_p
    $51EB851F T,  SYMBOL rodata_wlog_info.18  SYMBOL wifi_log  SYMBOL wifi_log
    SYMBOL wifi_log  SYMBOL wifi_log  SYMBOL wifi_log  SYMBOL wifi_log
    SYMBOL wifi_log  SYMBOL wifi_log  SYMBOL pm_scale_listen_interval
    SYMBOL pm_scale_listen_interval  SYMBOL pm_scale_listen_interval
    SYMBOL wifi_log
$60 L:
  A1 32 ENTRY,  A9 A3 A2 SUB,  A10 11 MOVI.N,  A8 0 MOVI.N,  A10 A9 $70 L BLT,
  $238 L J,
$70 L:
  A8 A2 8 L16UI,  A2 A5 0 S32I.N,  A5 A8 10 SLLI,  A8 $D4 L BNEZ,
  A5 $0 L L32R,  A8 A5 16 L8UI,  A5 25 MOVI.N,  A5 A5 12 SLLI,  A8 $D4 L BNEZ,
  A5 1 MOVI.N,  A5 A5 12 SLLI,  A13 $4 L L32R,  A11 A5 MOV.N,  A12 3 MOVI.N,
  A10 6 MOVI,  MEMW,  A8 $30 L L32R,  A8 CALLX8,
  A13 $8 L L32R,  A11 A5 MOV.N,  A12 3 MOVI.N,  A10 6 MOVI,  A8 $34 L L32R,
  A8 CALLX8,
  A13 $C L L32R,  A11 A5 MOV.N,  A12 3 MOVI.N,  A10 6 MOVI.N,  A8 $38 L L32R,
  A8 CALLX8,
  A13 $10 L L32R,  A11 A5 MOV.N,  A12 3 MOVI.N,  A10 6 MOVI.N,  A5 25 MOVI.N,
  A8 $3C L L32R,  A8 CALLX8,
  A5 A5 12 SLLI,
$D4 L:
  A2 A2 12 ADDI.N,  $F6 L J,
$D9 L:
  A8 A2 1 L8UI,  A10 A8 1 ADDI.N,  A10 A9 $FC L BGE,
  A9 A2 0 L8UI,  A9 5 $F2 L BNEI,
  A2 A4 0 S32I.N,  MEMW,  $15C L J,
  3 ZEROS,
$F2 L:
  A8 A8 2 ADDI.N,  A2 A2 A8 ADD.N,
$F6 L:
  A9 A3 A2 SUB,  A9 2 $D9 L BGEI,
$FC L:
  A2 $0 L L32R,  A3 A4 0 L32I.N,  A2 A2 16 L8UI,  A3 $15C L BNEZ,
  A2 $118 L BEQZ.N,
$109 L:
  A2 $0 L L32R,  A3 A2 44 L32I.N,  A3 A5 $114 L BNE,
  $198 L J,
$114 L:
  $16A L J,
  1 ZEROS,
$118 L:
  A2 1 MOVI.N,  A2 A2 12 SLLI,  A13 $14 L L32R,  A12 3 MOVI.N,  A11 A2 MOV.N,
  A10 6 MOVI,  A8 $40 L L32R,  A8 CALLX8,
  A13 $18 L L32R,  A12 3 MOVI.N,  A11 A2 MOV.N,  A10 6 MOVI,  A8 $44 L L32R,
  A8 CALLX8,
  A13 $1C L L32R,  A12 3 MOVI.N,  A11 A2 MOV.N,  A10 6 MOVI,  A8 $48 L L32R,
  A8 CALLX8,
  A13 $20 L L32R,  A12 3 MOVI.N,  A11 A2 MOV.N,  A10 6 MOVI.N,  A8 $4C L L32R,
  A8 CALLX8,
$15C L:
  A2 $0 L L32R,  A3 A2 16 L8UI,  A3 $1CA L BEQZ,
  $109 L J,
  2 ZEROS,
$16A L:
  A5 A2 44 S32I.N,  MEMW,  A8 $50 L L32R,  A8 CALLX8,
  A3 $24 L L32R,  A8 A3 0 L32I.N,  A10 A8 $194 L32I,  A10 CALLX8,
  A10 $198 L BEQZ.N,
  A8 A2 14 L8UI,  A8 $198 L BEQZ.N,
  A10 $28 L L32R,  A10 A5 A10 MULUH,  A10 A10 5 SRLI,  A3 A3 0 L32I.N,
  A3 A3 $1B8 L32I,  A3 CALLX8,
$198 L:
  A4 A4 0 L32I.N,  A4 $1A0 L BNEZ.N,
  $236 L J,
  1 ZEROS,
$1A0 L:
  A3 A4 3 L8UI,  A5 A2 11 L8UI,  A3 A5 $1B5 L BNE,
  A8 A2 17 L8UI,  A8 $236 L BEQZ,
  A4 A4 2 L8UI,  A4 $236 L BEQZ,
$1B5 L:
  A3 A2 11 S8I,  MEMW,  A3 A5 $236 L BEQ,
  MEMW,  A8 $54 L L32R,  A8 CALLX8,
  $236 L J,
$1CA L:
  A3 A4 0 L32I.N,  A5 A2 44 S32I.N,  A3 $1D9 L BEQZ.N,
  A3 A3 3 L8UI,  A3 A2 11 S8I,  MEMW,
$1D9 L:
  A3 A2 11 L8UI,  A3 $1E8 L BNEZ,
  A3 1 MOVI,  A3 A2 11 S8I,  MEMW,
$1E8 L:
  MEMW,  A8 $58 L L32R,  A8 CALLX8,
  A3 $24 L L32R,  A4 A3 0 L32I.N,  A10 A4 $194 L32I,  A10 CALLX8,
  A10 $214 L BEQZ.N,
  A4 A2 14 L8UI,  A4 $214 L BEQZ.N,
  A4 $28 L L32R,  A10 A5 A4 MULUH,  A10 A10 5 SRLI,  A3 A3 0 L32I.N,
  A3 A3 $1B8 L32I,  A3 CALLX8,
$214 L:
  A11 1 MOVI.N,  A3 1 MOVI.N,  A15 A2 11 L8UI,  A12 3 MOVI.N,  A11 A11 12 SLLI,
  A10 6 MOVI.N,  A14 A2 44 L32I.N,  A13 $2C L L32R,  A3 A2 16 S8I,  MEMW,  MEMW,
  A8 $5C L L32R,  A8 CALLX8,
$236 L:
  A8 1 MOVI.N,
$238 L:
  A2 A8 MOV.N,  RETW.N,
$23C L:  END-CODE

CODE pm_on_beacon_rx   \ .wifislprxiram.54 $38
  $0 14 LITERALS
    SYMBOL g_pm  SYMBOL g_osi_funcs_p  SYMBOL g_mesh_is_root
    SYMBOL g_mesh_init_ps_type  SYMBOL esp_mesh_quick_funcs  SYMBOL g_pm_cnt
    SYMBOL ptr_beacon_offset_funcs  SYMBOL pm_disable_beacon_monitor_timer
    SYMBOL pm_coex_schm_overall_period_get  SYMBOL pm_beacon_monitor_tbtt_stop
    SYMBOL pm_parse_beacon  SYMBOL pm_update_next_tbtt
    SYMBOL pm_rx_beacon_process  SYMBOL pm_beacon_timestamp_statistic
$38 L:
  A1 48 ENTRY,  A6 0 MOVI.N,  A6 A1 4 S32I,  A5 A5 0 8 EXTUI,  A6 A1 0 S32I,
  A6 $0 L L32R,  A10 A6 A6 OR,  MEMW,  A8 $1C L L32R,  A8 CALLX8,
  A8 $4 L L32R,  A8 A8 0 L32I,  A10 A8 $194 L32I,  A10 CALLX8,
  A10 $74 L BEQZ.N,
  A8 $20 L L32R,  A8 CALLX8,
  A10 1 $74 L BEQI,
  A10 A6 MOV.N,  A8 $24 L L32R,  A8 CALLX8,
$74 L:
  A8 $8 L L32R,  MEMW,  A8 A8 0 L8UI,  A8 A8 0 8 EXTUI,  A8 $94 L BEQZ.N,
  A8 $C L L32R,  A8 A8 0 L32I.N,  A8 $94 L BEQZ.N,
  A8 $10 L L32R,  A10 A8 148 L32I,  A10 $94 L BEQZ.N,
  A10 CALLX8,
$94 L:
  A8 A6 13 L8UI,  A8 $F4 L BEQZ,
  A5 $B8 L BNEZ.N,
  A13 A1 4 ADDI.N,  A12 A1 MOV.N,  A11 A4 MOV.N,  A10 A3 MOV.N,  A8 $28 L L32R,
  A8 CALLX8,
  A10 $B8 L BEQZ.N,
  A12 A1 0 L32I.N,  A10 A2 MOV.N,  A11 A1 4 L32I.N,  A8 $2C L L32R,  A8 CALLX8,
$B8 L:
  A11 A5 MOV.N,  A8 A6 30 L16UI,  A8 A8 1 ADDI.N,  A3 $14 L L32R,  A8 A6 30 S16I,
  MEMW,  A6 A3 4 L32I.N,  A6 A6 1 ADDI.N,  A10 A1 0 L32I.N,  A6 A3 4 S32I,  MEMW,
  A8 $30 L L32R,  A8 CALLX8,
  A4 A1 4 L32I.N,  A10 A4 MOV.N,  A8 $34 L L32R,  A8 CALLX8,
  A3 $18 L L32R,  A3 A3 0 L32I.N,  A3 A3 44 L32I.N,  A3 $F4 L BEQZ.N,
  A11 A2 MOV.N,  A10 A4 MOV.N,  A3 CALLX8,
$F4 L:
  RETW.N,
$F6 L:  END-CODE

CODE pm_on_probe_resp_rx   \ .text.pm_on_probe_resp_rx $14
  $0 5 LITERALS
    SYMBOL g_pm  SYMBOL g_osi_funcs_p  $51EB851F T,
    SYMBOL pm_scale_listen_interval  SYMBOL pm_update_next_tbtt
$14 L:
  A1 48 ENTRY,  A6 $0 L L32R,  A2 A2 0 16 EXTUI,  A8 A6 13 L8UI,  A8 $70 L BEQZ,
  A8 A2 10 SLLI,  A2 $2D L BNEZ.N,
  A8 25 MOVI.N,  A8 A8 12 SLLI,
$2D L:
  A2 A6 44 L32I.N,  A2 A8 $3D L BEQ,
  A8 A6 44 S32I.N,  MEMW,  A8 $C L L32R,  A8 CALLX8,
$3D L:
  A2 $4 L L32R,  A8 A2 0 L32I.N,  A10 A8 $194 L32I,  A10 CALLX8,
  A10 $5D L BEQZ.N,
  A10 A6 44 L32I.N,  A6 $8 L L32R,  A10 A10 A6 MULUH,  A10 A10 5 SRLI,
  A2 A2 0 L32I.N,  A2 A2 $1B8 L32I,  A2 CALLX8,
$5D L:
  A12 0 MOVI.N,  A11 A1 MOV.N,  A10 A3 MOV.N,  A4 A1 0 S32I.N,  A5 A1 4 S32I.N,
  MEMW,  A8 $10 L L32R,  A8 CALLX8,
$70 L:
  RETW.N,
$72 L:  END-CODE

CODE pm_disconnected_start   \ .text.pm_disconnected_start $24
  $0 9 LITERALS
    SYMBOL g_pm  SYMBOL g_osi_funcs_p  SYMBOL pm_on_coex_schm_status_config
    SYMBOL pm_coex_schm_process_restart  SYMBOL pm_disable_disconnected_sl~DBBF
    SYMBOL wifi_gpio_debug  SYMBOL wifi_gpio_debug  SYMBOL wifi_gpio_debug
    SYMBOL pm_update_by_connectionles~3F93
$24 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A10 0 MOVI,  A10 A2 $120 S32I,  MEMW,  A8 $8 L L32R,
  A8 CALLX8,
  A3 $4 L L32R,  A3 A3 0 L32I.N,  A10 A3 $194 L32I,  A10 CALLX8,
  A10 $4C L BEQZ.N,
  A8 $C L L32R,  A8 CALLX8,
$4C L:
  A3 A2 $120 L32I,  A3 A3 1 ADDI.N,  A3 A2 $120 S32I,  A3 A2 $100 ADDMI,
  A8 A3 29 L8UI,  A8 $A2 L BEQZ,
  MEMW,  A8 $10 L L32R,  A8 CALLX8,
  A8 1 MOVI.N,  A8 A2 14 S16I,  A2 0 MOVI.N,  A11 1 MOVI.N,  A10 3 MOVI.N,
  A2 A3 136 S8I,  A2 2 MOVI,  A2 A3 29 S8I,  MEMW,  MEMW,  A8 $14 L L32R,
  A8 CALLX8,
  A11 1 MOVI,  A10 4 MOVI,  A8 $18 L L32R,  A8 CALLX8,
  A11 0 MOVI.N,  A10 5 MOVI.N,  A8 $1C L L32R,  A8 CALLX8,
  A8 $20 L L32R,  A8 CALLX8,
$A2 L:
  RETW.N,
$A4 L:  END-CODE

CODE pm_disconnected_stop   \ .text.pm_disconnected_stop $20
  $0 8 LITERALS
    SYMBOL g_pm  $F4 SYMBOL+ g_pm  SYMBOL g_osi_funcs_p
    SYMBOL pm_disable_disconnected_sl~DBBF  SYMBOL wifi_gpio_debug
    SYMBOL wifi_gpio_debug  SYMBOL wifi_gpio_debug
    SYMBOL pm_update_by_connectionles~3F93
$20 L:
  A1 32 ENTRY,  A8 $8 L L32R,  A3 0 MOVI,  A8 A8 0 L32I,  A2 $0 L L32R,
  A2 A2 $100 ADDMI,  A8 A8 236 L32I,  A10 $4 L L32R,  A3 A2 32 S32I,  MEMW,
  A8 CALLX8,
  A8 A2 29 L8UI,  A8 A3 $7E L BEQ,
  A8 $C L L32R,  A8 CALLX8,
  A8 1 MOVI.N,  A11 A3 A3 OR,  A10 3 MOVI,  A8 A2 29 S8I,  MEMW,  MEMW,
  A8 $10 L L32R,  A8 CALLX8,
  A11 1 MOVI.N,  A10 4 MOVI.N,  A8 $14 L L32R,  A8 CALLX8,
  A11 1 MOVI.N,  A10 5 MOVI.N,  A8 $18 L L32R,  A8 CALLX8,
  A8 $1C L L32R,  A8 CALLX8,
$7E L:
  RETW.N,
$80 L:  END-CODE

CODE pm_tbtt_quick_wake_configure   \ .text.pm_tbtt_quick_wake_configure $8
  $0 2 LITERALS
    SYMBOL g_pm  SYMBOL ptr_beacon_offset_funcs
$8 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A2 0 8 EXTUI,  A8 A8 $100 ADDMI,  A2 A8 212 S8I,
  MEMW,  A2 $2C L BEQZ.N,
  A2 $4 L L32R,  A2 A2 0 L32I.N,  A8 A2 8 L32I.N,  A8 $2C L BEQZ,
  MEMW,  A8 CALLX8,
$2C L:
  RETW.N,
$2E L:  END-CODE

CODE pm_start   \ .text.pm_start $4C
  $0 19 LITERALS
    SYMBOL g_pm  SYMBOL ptr_beacon_offset_funcs  SYMBOL rodata_wlog_info.57
    SYMBOL g_osi_funcs_p  $51EB851F T,  SYMBOL bss.g_offchan_ready_cb
    SYMBOL bss.g_offchan_ready_ctx  SYMBOL wifi_log  SYMBOL memcpy
    SYMBOL pm_scale_listen_interval  SYMBOL dbg_lmac_ps_statis_reset
    SYMBOL pm_update_next_tbtt  SYMBOL pm_beacon_monitor_init
    SYMBOL pm_enable_active_timer  SYMBOL pm_on_coex_schm_status_config
    SYMBOL wifi_gpio_debug  SYMBOL wifi_gpio_debug  SYMBOL wifi_gpio_debug
    SYMBOL pm_update_by_connectionles~3F93
$4C L:
  A1 64 ENTRY,  A7 $0 L L32R,  A2 A2 0 8 EXTUI,  A3 A3 0 16 EXTUI,
  A5 A5 0 16 EXTUI,  A8 A7 13 L8UI,  A8 $219 L BNEZ,
  A8 $4 L L32R,  A8 A8 0 L32I.N,  A8 A8 48 L32I.N,  A8 $6D L BEQZ.N,
  A8 CALLX8,
$6D L:
  A11 1 MOVI.N,  A14 A7 36 L32I.N,  A12 3 MOVI.N,  A11 A11 12 SLLI,  A10 6 MOVI.N,
  A13 $8 L L32R,  A8 $1C L L32R,  A8 CALLX8,
  A8 1 MOVI.N,  A12 6 MOVI.N,  A8 A7 14 S16I,  A8 0 MOVI.N,  A11 A4 MOV.N,
  A10 A7 3 ADDI.N,  A3 A7 26 S16I,  A3 A7 $100 ADDMI,  A8 A3 192 S8I,  A2 A7 2 S8I,
  MEMW,  A2 0 MOVI.N,  A8 A1 16 S32I.N,  MEMW,  A8 $20 L L32R,  A8 CALLX8,
  A8 A1 16 L32I.N,  A2 A7 60 S32I.N,  A2 A7 64 S32I,  A2 A7 136 S32I,
  A2 A7 140 S32I,  A2 $C L L32R,  A4 A2 0 L32I,  A10 A4 $110 L32I,  A8 A7 19 S8I,
  A8 A7 20 S8I,  A8 A3 232 S8I,  A8 A7 9 S8I,  A8 A7 10 S8I,  MEMW,  MEMW,
  A10 CALLX8,
  A4 A2 0 L32I.N,  A10 A7 128 S32I,  A10 A4 $110 L32I,  A11 A7 132 S32I,  MEMW,
  A10 CALLX8,
  A4 A2 0 L32I.N,  A10 A7 112 S32I,  A10 A4 $110 L32I,  A11 A7 116 S32I,  MEMW,
  A10 CALLX8,
  A4 A2 0 L32I.N,  A10 A7 120 S32I,  A10 A4 $110 L32I,  A4 A5 10 SLLI,
  A11 A7 124 S32I,  MEMW,  A10 CALLX8,
  A10 A7 72 S32I,  A11 A7 76 S32I,  A5 $11C L BNEZ.N,
  A4 25 MOVI,  A4 A4 12 SLLI,
$11C L:
  A4 A7 44 S32I.N,  A4 A2 0 L32I.N,  A10 A4 $194 L32I,  MEMW,  A10 CALLX8,
  A10 $13E L BEQZ.N,
  A4 $10 L L32R,  A10 A7 44 L32I.N,  A10 A10 A4 MULUH,  A10 A10 5 SRLI,
  A4 A2 0 L32I.N,  A4 A4 $1B8 L32I,  A4 CALLX8,
$13E L:
  A5 1 MOVI.N,  A4 0 MOVI,  A5 A7 11 S8I,  MEMW,  MEMW,  A8 $24 L L32R,  A8 CALLX8,
  A13 1 MOVI.N,  A2 A2 0 L32I.N,  A13 A7 24 S16I,  MEMW,  A10 A2 $194 L32I,
  A4 A3 28 S8I,  MEMW,  A13 A1 16 S32I.N,  MEMW,  A10 CALLX8,
  A8 0 MOVI.N,  A2 A8 MOV.N,  A4 A3 200 S8I,  A4 A7 $200 ADDMI,  A10 A7 12 S8I,
  A8 A7 80 S32I,  A8 A7 84 S32I,  A8 A7 $1D0 S32I,  A8 A4 28 S8I,  MEMW,  MEMW,
  A8 $28 L L32R,  A8 CALLX8,
  A12 A2 MOV.N,  A11 A1 MOV.N,  A10 A6 MOV.N,  A8 A1 64 L32I,  A9 A1 68 L32I,
  A8 A1 0 S32I,  A9 A1 4 S32I,  MEMW,  A8 $2C L L32R,  A8 CALLX8,
  A8 $30 L L32R,  A8 CALLX8,
  A8 $34 L L32R,  A8 CALLX8,
  A10 4 MOVI,  A8 $38 L L32R,  A8 CALLX8,
  A13 A1 16 L32I.N,  A2 A3 29 L8UI,  A13 A3 201 S8I,  MEMW,  A2 $1D5 L BEQZ.N,
  A5 A3 29 S8I,  MEMW,
$1D5 L:
  A11 1 MOVI.N,  A10 3 MOVI.N,  MEMW,  A8 $3C L L32R,  A8 CALLX8,
  A11 1 MOVI.N,  A10 4 MOVI.N,  A8 $40 L L32R,  A8 CALLX8,
  A11 0 MOVI.N,  A10 5 MOVI.N,  A8 $44 L L32R,  A8 CALLX8,
  A2 1 MOVI.N,  A3 $14 L L32R,  A2 A7 13 S8I,  MEMW,  A2 0 MOVI.N,  A2 A3 0 S32I.N,
  A3 $18 L L32R,  A2 A4 30 S8I,  MEMW,  A2 A3 0 S32I.N,  MEMW,  A8 $48 L L32R,
  A8 CALLX8,
$219 L:
  RETW.N,
$21B L:  END-CODE

CODE pm_stop   \ .text.pm_stop $80
  $0 32 LITERALS
    SYMBOL g_pm  $A4 SYMBOL+ g_pm  SYMBOL g_osi_funcs_p  $B8 SYMBOL+ g_pm
    SYMBOL rodata_wlog_info.58  $30000 T,  SYMBOL ptr_beacon_offset_funcs
    SYMBOL g_mesh_is_started  SYMBOL g_mesh_is_root  SYMBOL g_mesh_init_ps_type
    SYMBOL rodata_wlog_info.59  SYMBOL hal_pm_unblock_txq
    SYMBOL pm_disable_sleep_delay_timer  SYMBOL pm_disable_active_timer
    SYMBOL pm_dream  SYMBOL pm_set_state  SYMBOL ppProcessWaitingQueue
    SYMBOL wifi_log  SYMBOL pm_wake_done  SYMBOL pm_coex_pwr_update
    SYMBOL pm_on_coex_schm_status_config  SYMBOL pm_coex_schm_process_restart
    SYMBOL pm_enable_disconnected_sle~30EA  SYMBOL pm_update_by_connectionles~3F93
    SYMBOL esp_mesh_get_running_activ~0BB2  SYMBOL __floatundisf
    SYMBOL __floatundisf  SYMBOL __divsf3  SYMBOL __extendsfdf2  SYMBOL wifi_log
    SYMBOL ieee80211_hostapd_beacon_txcb  SYMBOL pm_set_bss_max_idle
$80 L:
  A1 80 ENTRY,  A2 $0 L L32R,  A3 A2 13 L8UI,  A3 $292 L BEQZ,
  A8 $2C L L32R,  A8 CALLX8,
  A8 $30 L L32R,  A8 CALLX8,
  A8 $34 L L32R,  A8 CALLX8,
  A5 $8 L L32R,  A3 A5 0 L32I,  A10 $4 L L32R,  A3 A3 236 L32I,  A3 CALLX8,
  A3 A5 0 L32I.N,  A10 $C L L32R,  A3 A3 236 L32I,  A3 CALLX8,
  A3 0 MOVI.N,  A3 A2 26 S16I,  MEMW,  A3 A2 1 L8UI,  A3 $DC L BEQZ.N,
  MEMW,  A8 $38 L L32R,  A8 CALLX8,
  A10 0 MOVI.N,  A8 $3C L L32R,  A8 CALLX8,
  A8 $40 L L32R,  A8 CALLX8,
$DC L:
  A3 A5 0 L32I.N,  A6 1 MOVI,  A14 A2 136 L32I,  A15 A2 140 L32I,  A10 A3 $110 L32I,
  A14 A1 32 S32I,  A15 A1 36 S32I,  MEMW,  A10 CALLX8,
  A3 A11 MOV.N,  A11 1 MOVI.N,  A11 A11 12 SLLI,  A4 A2 128 L32I,  A4 A10 A4 SUB,
  A7 A2 132 L32I,  A14 A1 32 L32I.N,  A15 A1 36 L32I,  A10 A4 $111 L BLTU,
  A6 0 MOVI,
$111 L:
  A3 A3 A7 SUB,  A3 A3 A6 SUB,  A13 $10 L L32R,  A12 3 MOVI.N,  A10 6 MOVI.N,
  A3 A1 4 S32I,  A4 A1 0 S32I,  MEMW,  A8 $44 L L32R,  A8 CALLX8,
  A3 A2 15 L8UI,  A3 $138 L BEQZ.N,
  A8 $48 L L32R,  A8 CALLX8,
$138 L:
  A4 1 MOVI.N,  A6 $14 L L32R,  A3 0 MOVI.N,  A4 A2 11 S8I,  A4 1 MOVI.N,
  A4 A2 24 S16I,  A4 A2 $100 ADDMI,  A6 A2 $180 S32I,  A3 A2 15 S8I,  A3 A2 16 S8I,
  A3 A2 13 S8I,  A3 A4 100 S8I,  A3 A4 125 S8I,  A3 A4 193 S8I,  A3 A4 200 S8I,
  MEMW,  MEMW,  A8 $4C L L32R,  A8 CALLX8,
  A10 0 MOVI.N,  A8 $50 L L32R,  A8 CALLX8,
  A3 A5 0 L32I.N,  A10 A3 $194 L32I,  A10 CALLX8,
  A10 $186 L BEQZ.N,
  A8 $54 L L32R,  A8 CALLX8,
$186 L:
  A3 $18 L L32R,  A3 A3 0 L32I.N,  A3 A3 52 L32I,  A3 $194 L BEQZ,
  A3 CALLX8,
$194 L:
  A3 A4 29 L8UI,  A3 $1B1 L BEQZ.N,
  A3 2 MOVI,  A3 A4 29 S8I,  MEMW,  A3 A2 $120 L32I,  A3 $1B1 L BNEZ,
  MEMW,  A8 $58 L L32R,  A8 CALLX8,
$1B1 L:
  A8 $5C L L32R,  A8 CALLX8,
  A3 $1C L L32R,  A3 A3 0 L8UI,  A3 $280 L BEQZ,
  A3 $20 L L32R,  MEMW,  A6 A3 0 L8UI,  A6 A6 0 8 EXTUI,  A6 $280 L BNEZ,
  A3 $24 L L32R,  A3 A3 0 L32I.N,  A3 $280 L BEQZ,
  A7 1 MOVI.N,  A3 A5 0 L32I.N,  A10 A3 $110 L32I,  A10 CALLX8,
  A5 A2 128 L32I,  A5 A10 A5 SUB,  A3 A2 132 L32I,  A10 A5 $1F0 L BLTU,
  A7 A6 A6 OR,
$1F0 L:
  A3 A11 A3 SUB,  A3 A3 A7 SUB,  A15 A2 140 L32I,  A7 A15 12 SLLI,
  A15 A15 20 12 EXTUI,  A6 A2 136 L32I,  A6 A6 20 12 EXTUI,  A6 A7 A6 OR,
  A7 1 MOVI.N,  A15 A1 36 S32I.N,  MEMW,  A8 $60 L L32R,  A8 CALLX8,
  A8 A10 A10 OR,  A7 A7 12 SLLI,  A11 A2 140 L32I,  A10 A2 136 L32I,
  A8 A1 32 S32I.N,  MEMW,  A8 $64 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  A11 A3 A3 OR,  A10 A5 A5 OR,  A8 $68 L L32R,  A8 CALLX8,
  A11 A10 MOV.N,  A10 A2 MOV.N,  A8 $6C L L32R,  A8 CALLX8,
  A8 $70 L L32R,  A8 CALLX8,
  A8 A1 32 L32I.N,  A2 A3 12 SLLI,  A3 A3 20 12 EXTUI,  A14 A6 MOV.N,  A12 3 MOVI.N,
  A10 A1 16 S32I.N,  A10 A5 20 12 EXTUI,  A10 A2 A10 OR,  A15 A1 36 L32I.N,
  A13 $28 L L32R,  A11 A1 20 S32I.N,  A11 A7 MOV.N,  A10 A1 0 S32I.N,  A10 6 MOVI.N,
  A8 A1 8 S32I.N,  A3 A1 4 S32I.N,  MEMW,  A8 $74 L L32R,  A8 CALLX8,
  A10 0 MOVI.N,  A8 $78 L L32R,  A8 CALLX8,
$280 L:
  A10 0 MOVI,  A2 0 MOVI,  A8 $7C L L32R,  A8 CALLX8,
  A2 A4 193 S8I,  MEMW,
$292 L:
  MEMW,  RETW.N,
$297 L:  END-CODE

CODE pm_attach   \ .text.pm_attach $84
  $0 33 LITERALS
    SYMBOL g_pm  SYMBOL g_pm_cfg  $19000 T,  $30000 T,
    SYMBOL pm_send_sleep_null_cb  SYMBOL pm_send_wake_null_cb
    SYMBOL pm_tbtt_timeout  $A4 SYMBOL+ g_pm  SYMBOL g_osi_funcs_p
    SYMBOL pm_dream_timeout  $B8 SYMBOL+ g_pm  SYMBOL pm_active_timeout
    $CC SYMBOL+ g_pm  SYMBOL pm_sleep_delay_timeout  $108 SYMBOL+ g_pm
    SYMBOL pm_coex_slice_wifi_timeout  $F4 SYMBOL+ g_pm
    SYMBOL pm_disconnected_sleep_dela~50E1  $124 SYMBOL+ g_pm
    SYMBOL pm_connectionless_wake_int~B6C6  $138 SYMBOL+ g_pm
    SYMBOL pm_connectionless_wake_win~1EF1  $14C SYMBOL+ g_pm
    SYMBOL pm_beacon_monitor_timeout  $168 SYMBOL+ g_pm
    SYMBOL pm_max_idle_timeout  $1EC SYMBOL+ g_pm  SYMBOL pm_coex_restart_timeout
    $200 SYMBOL+ g_pm  SYMBOL memset  SYMBOL pm_set_state
    SYMBOL ppRegisterTxCallback  SYMBOL ppRegisterTxCallback
$84 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A12 $220 MOVI,  A11 0 MOVI,  A10 A2 A2 OR,
  A8 $74 L L32R,  A8 CALLX8,
  A5 1 MOVI.N,  A10 0 MOVI.N,  A5 A2 36 S32I.N,  MEMW,  A8 $78 L L32R,  A8 CALLX8,
  A6 A2 $100 ADDMI,  A11 5 MOVI.N,  A3 $4 L L32R,  A4 A3 4 L32I.N,  A4 A2 40 S32I.N,
  A4 A3 12 L32I.N,  A4 A2 52 S32I.N,  A4 $8 L L32R,  A4 A2 44 S32I.N,
  A4 A3 20 L32I.N,  A4 A2 48 S32I.N,  A4 A3 44 L32I.N,  A3 A3 24 L32I.N,
  A4 A2 $1C4 S32I,  A4 $C L L32R,  A3 A2 56 S32I.N,  A3 0 MOVI.N,  A12 A3 MOV.N,
  A10 $10 L L32R,  A4 A2 $180 S32I,  A5 A2 24 S16I,  A3 A2 $17E S16I,  A5 A2 11 S8I,
  A5 A2 32 S16I,  A3 A2 60 S32I.N,  A3 A2 21 S8I,  A3 A6 125 S8I,  A5 A2 14 S8I,
  A3 A6 212 S8I,  MEMW,  MEMW,  A8 $7C L L32R,  A8 CALLX8,
  A12 A3 MOV.N,  A11 6 MOVI.N,  A10 $14 L L32R,  A8 $80 L L32R,  A8 CALLX8,
  A12 A3 MOV.N,  A4 $20 L L32R,  A8 A4 0 L32I.N,  A11 $18 L L32R,  A8 A8 244 L32I,
  A10 $1C L L32R,  A8 CALLX8,
  A12 A3 MOV.N,  A8 A4 0 L32I.N,  A11 $24 L L32R,  A8 A8 244 L32I,  A10 $28 L L32R,
  A8 CALLX8,
  A12 A3 MOV.N,  A8 A4 0 L32I.N,  A11 $2C L L32R,  A8 A8 244 L32I,  A10 $30 L L32R,
  A8 CALLX8,
  A12 A3 MOV.N,  A8 A4 0 L32I.N,  A11 $34 L L32R,  A8 A8 244 L32I,  A10 $38 L L32R,
  A8 CALLX8,
  A12 A3 MOV.N,  A8 A4 0 L32I.N,  A11 $3C L L32R,  A8 A8 244 L32I,  A10 $40 L L32R,
  A8 CALLX8,
  A12 A3 MOV.N,  A8 A4 0 L32I.N,  A11 $44 L L32R,  A8 A8 244 L32I,  A10 $48 L L32R,
  A8 CALLX8,
  A12 A3 MOV.N,  A8 A4 0 L32I.N,  A11 $4C L L32R,  A8 A8 244 L32I,  A10 $50 L L32R,
  A8 CALLX8,
  A12 A3 MOV.N,  A8 A4 0 L32I.N,  A11 $54 L L32R,  A8 A8 244 L32I,  A10 $58 L L32R,
  A8 CALLX8,
  A12 A2 MOV.N,  A8 A4 0 L32I.N,  A11 $5C L L32R,  A8 A8 244 L32I,  A10 $60 L L32R,
  A8 CALLX8,
  A12 A3 MOV.N,  A8 A4 0 L32I.N,  A11 $64 L L32R,  A8 A8 244 L32I,  A10 $68 L L32R,
  A8 CALLX8,
  A12 A3 MOV.N,  A4 A4 0 L32I.N,  A11 $6C L L32R,  A4 A4 244 L32I,  A10 $70 L L32R,
  A4 CALLX8,
  A3 A2 $18A S16I,  A3 A2 $1A8 S32I,  A3 A2 $1AC S32I,  A3 A6 29 S8I,
  A5 A6 201 S8I,  A5 A2 0 S8I,  MEMW,  MEMW,  RETW.N,
$1D7 L:  END-CODE

CODE pm_deattach   \ .text.pm_deattach $34
  $0 13 LITERALS
    SYMBOL g_pm  $A4 SYMBOL+ g_pm  SYMBOL g_osi_funcs_p  $B8 SYMBOL+ g_pm
    $CC SYMBOL+ g_pm  $108 SYMBOL+ g_pm  $F4 SYMBOL+ g_pm  $124 SYMBOL+ g_pm
    $138 SYMBOL+ g_pm  $14C SYMBOL+ g_pm  $168 SYMBOL+ g_pm  $1EC SYMBOL+ g_pm
    $200 SYMBOL+ g_pm
$34 L:
  A1 32 ENTRY,  A3 $0 L L32R,  A2 A3 0 L8UI,  A2 1 $43 L BEQI,
  $D0 L J,
$43 L:
  A2 $8 L L32R,  A8 A2 0 L32I,  A10 $4 L L32R,  A8 A8 240 L32I,  A8 CALLX8,
  A8 A2 0 L32I,  A10 $C L L32R,  A8 A8 240 L32I,  A8 CALLX8,
  A8 A2 0 L32I,  A10 $10 L L32R,  A8 A8 240 L32I,  A8 CALLX8,
  A8 A2 0 L32I,  A10 $14 L L32R,  A8 A8 240 L32I,  A8 CALLX8,
  A8 A2 0 L32I,  A10 $18 L L32R,  A8 A8 240 L32I,  A8 CALLX8,
  A8 A2 0 L32I,  A10 $1C L L32R,  A8 A8 240 L32I,  A8 CALLX8,
  A8 A2 0 L32I,  A10 $20 L L32R,  A8 A8 240 L32I,  A8 CALLX8,
  A8 A2 0 L32I,  A10 $24 L L32R,  A8 A8 240 L32I,  A8 CALLX8,
  A8 A2 0 L32I,  A10 $28 L L32R,  A8 A8 240 L32I,  A8 CALLX8,
  A8 A2 0 L32I.N,  A10 $2C L L32R,  A8 A8 240 L32I,  A8 CALLX8,
  A2 A2 0 L32I.N,  A10 $30 L L32R,  A2 A2 240 L32I,  A2 CALLX8,
  A2 0 MOVI.N,  A2 A3 0 S8I,  MEMW,
$D0 L:
  MEMW,  RETW.N,
$D5 L:  END-CODE

CODE pm_set_sleep_type   \ .text.pm_set_sleep_type $38
  $0 14 LITERALS
    SYMBOL g_osi_funcs_p  SYMBOL rodata_wlog_info.60  SYMBOL rodata_wlog_info.61
    SYMBOL g_pm  SYMBOL wifi_log  SYMBOL wifi_log  SYMBOL pm_enable_active_timer
    SYMBOL pm_disable_max_idle_timer  SYMBOL pm_dream  SYMBOL pm_set_state
    SYMBOL pm_go_to_wake  SYMBOL pm_enable_active_timer
    SYMBOL pm_enable_max_idle_timer  SYMBOL pm_coex_pwr_update
$38 L:
  A1 32 ENTRY,  A5 $0 L L32R,  A3 A5 0 L32I.N,  A10 A3 $194 L32I,  A3 1 MOVI.N,
  A10 CALLX8,
  A3 A3 12 SLLI,  A15 A10 MOV.N,  A13 $4 L L32R,  A14 A2 MOV.N,  A12 3 MOVI.N,
  A11 A3 MOV.N,  A10 6 MOVI.N,  A8 $10 L L32R,  A8 CALLX8,
  A2 $79 L BNEZ.N,
  A4 A5 0 L32I.N,  A10 A4 $194 L32I,  A10 CALLX8,
  A10 $79 L BEQZ.N,
  A12 3 MOVI.N,  A13 $8 L L32R,  A11 A3 MOV.N,  A10 6 MOVI.N,  A8 $14 L L32R,
  A8 CALLX8,
$79 L:
  A3 $C L L32R,  A8 A3 13 L8UI,  A4 A3 36 L32I.N,  A2 A3 36 S32I.N,  A8 $DC L BEQZ,
  A5 A5 0 L32I.N,  A10 A5 $194 L32I,  MEMW,  A10 CALLX8,
  A10 1 $DC L BEQI,
  A4 $AA L BNEZ.N,
  A2 A2 -1 ADDI.N,  A2 2 $DC L BGEUI,
  A8 $18 L L32R,  A8 CALLX8,
  A8 $1C L L32R,  A8 CALLX8,
  $DC L J,
$AA L:
  A2 $DC L BNEZ.N,
  A4 A4 -1 ADDI,  A4 2 $DC L BGEUI,
  A3 A3 1 L8UI,  A3 $D0 L BEQZ,
  A8 $20 L L32R,  A8 CALLX8,
  A10 A2 MOV.N,  A8 $24 L L32R,  A8 CALLX8,
  A11 A2 MOV.N,  A10 A2 MOV.N,  A8 $28 L L32R,  A8 CALLX8,
$D0 L:
  A8 $2C L L32R,  A8 CALLX8,
  A8 $30 L L32R,  A8 CALLX8,
$DC L:
  A8 $34 L L32R,  A8 CALLX8,
  RETW.N,
$E4 L:  END-CODE

CODE pm_get_sleep_type   \ .text.pm_get_sleep_type $4
  $0 1 LITERALS
    SYMBOL g_pm
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A8 36 L32I.N,  RETW.N,
$E L:  END-CODE

CODE pm_beacon_monitor_configure   \ .text.pm_beacon_monitor_configure $4
  $0 1 LITERALS
    SYMBOL g_pm_cfg
$4 L:
  A1 32 ENTRY,  A9 A2 0 L8UI,  A8 $0 L L32R,  A9 A8 36 S8I,  MEMW,  A9 $41 L BEQZ.N,
  A9 A2 1 L8UI,  A9 A8 37 S8I,  MEMW,  A9 A2 2 L8UI,  A9 A8 38 S8I,  MEMW,
  A9 A2 3 L8UI,  A9 A8 39 S8I,  MEMW,  A9 A2 4 L8UI,  A9 A8 40 S8I,  MEMW,
  A2 A2 8 L32I.N,  A2 A8 51 S8I,  MEMW,
$41 L:
  MEMW,  RETW.N,
$46 L:  END-CODE

CODE pm_update_modem_sleep_defa~4980   \ pm_update_modem_sleep_default_params: .text.pm_update_modem_sleep_default_params $4
  $0 1 LITERALS
    SYMBOL g_pm_cfg
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A9 $1F4 MOVI,  A9 A8 4 S32I.N,  MEMW,  RETW.N,
$14 L:  END-CODE

CODE pm_update_light_sleep_defa~2A79   \ pm_update_light_sleep_default_params: .text.pm_update_light_sleep_default_params $4
  $0 1 LITERALS
    SYMBOL g_pm_cfg
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A9 $1F4 MOVI,  A9 A8 4 S32I.N,  MEMW,  RETW.N,
$14 L:  END-CODE

CODE pm_set_sleep_min_active_time   \ .text.pm_set_sleep_min_active_time $4
  $0 1 LITERALS
    SYMBOL g_pm_cfg
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A8 20 S32I.N,  MEMW,  RETW.N,
$11 L:  END-CODE

CODE pm_set_keep_alive_time   \ .text.pm_set_keep_alive_time $4
  $0 1 LITERALS
    SYMBOL g_pm_cfg
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A8 16 S32I.N,  MEMW,  RETW.N,
$11 L:  END-CODE

CODE pm_get_keep_alive_time   \ .text.pm_get_keep_alive_time $4
  $0 1 LITERALS
    SYMBOL g_pm_cfg
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A8 16 L32I.N,  RETW.N,
$E L:  END-CODE

CODE pm_set_sleep_wait_broadcas~0D70   \ pm_set_sleep_wait_broadcast_data_time: .text.pm_set_sleep_wait_broadcast_data_time $4
  $0 1 LITERALS
    SYMBOL g_pm_cfg
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A8 44 S32I.N,  MEMW,  RETW.N,
$11 L:  END-CODE

CODE pm_scan_lock   \ .text.pm_scan_lock $0
  $0 ORIGIN
  A1 32 ENTRY,  RETW.N,
$5 L:  END-CODE

CODE pm_scan_unlock   \ .text.pm_scan_unlock $0
  $0 ORIGIN
  A1 32 ENTRY,  RETW.N,
$5 L:  END-CODE

CODE pm_try_scan_unlock   \ .text.pm_try_scan_unlock $0
  $0 ORIGIN
  A1 32 ENTRY,  RETW.N,
$5 L:  END-CODE

CODE pm_force_scan_unlock   \ .text.pm_force_scan_unlock $0
  $0 ORIGIN
  A1 32 ENTRY,  RETW.N,
$5 L:  END-CODE

CODE pm_get_idle_wait_time   \ .text.pm_get_idle_wait_time $0
  $0 ORIGIN
  A1 32 ENTRY,  A2 0 MOVI.N,  RETW.N,
$7 L:  END-CODE

CODE pm_enable_sta_disconnected~B345   \ pm_enable_sta_disconnected_power_management: .text.pm_enable_sta_disconnected_power_management $4
  $0 1 LITERALS
    SYMBOL g_pm
$4 L:
  A1 32 ENTRY,  A9 $0 L L32R,  A2 A2 0 8 EXTUI,  A8 A9 $100 ADDMI,  A10 A8 29 L8UI,
  A10 2 $3A L BGEUI,
  A9 A9 13 L8UI,  A9 $3A L BNEZ.N,
  A2 $2C L BEQZ.N,
  A9 1 MOVI.N,  A9 A8 29 S8I,  MEMW,  MEMW,  $3C L J,
  1 ZEROS,
$2C L:
  A2 A8 29 S8I,  MEMW,  A2 1 MOVI.N,  MEMW,  $3C L J,
$3A L:
  A2 0 MOVI.N,
$3C L:
  RETW.N,
$3E L:  END-CODE

CODE pm_register_connectionless~CFAC   \ pm_register_connectionless_wake_window: .text.pm_register_connectionless_wake_window $14
  $0 5 LITERALS
    SYMBOL g_osi_funcs_p  SYMBOL g_pm  SYMBOL pm_update_by_connectionles~3F93
    SYMBOL pm_on_coex_schm_status_config  SYMBOL pm_coex_schm_process_restart
$14 L:
  A1 48 ENTRY,  A2 $8C L BEQZ,
  A3 A2 0 L16UI,  A3 $8C L BEQZ,
  A10 8 MOVI.N,  A4 $0 L L32R,  A3 A4 0 L32I.N,  A3 A3 $16C L32I,  A3 CALLX8,
  A10 $8C L BEQZ,
  A3 0 MOVI.N,  A3 A10 4 S32I.N,  A3 $4 L L32R,  A8 A3 $160 L32I,  A2 A10 0 S32I.N,
  A8 A1 4 S32I.N,  A8 A1 MOV.N,  MEMW,  $5A L J,
$46 L:
  A11 A8 0 L32I.N,  A12 A11 0 L16UI,  A11 A2 0 L16UI,  A12 A11 $5A L BGEU,
  A8 A10 4 S32I.N,  MEMW,  $61 L J,
  1 ZEROS,
$5A L:
  A9 A8 MOV.N,  A8 A8 4 L32I.N,  A8 $46 L BNEZ,
$61 L:
  A10 A9 4 S32I.N,  A2 A1 4 L32I.N,  A2 A3 $160 S32I,  MEMW,  A8 $8 L L32R,
  A8 CALLX8,
  A10 A3 13 L8UI,  A10 $8C L BNEZ.N,
  A8 $C L L32R,  A8 CALLX8,
  A2 A4 0 L32I.N,  A10 A2 $194 L32I,  A10 CALLX8,
  A10 $8C L BEQZ.N,
  A8 $10 L L32R,  A8 CALLX8,
$8C L:
  RETW.N,
$8E L:  END-CODE

CODE pm_unregister_connectionle~9CF6   \ pm_unregister_connectionless_wake_window: .text.pm_unregister_connectionless_wake_window $14
  $0 5 LITERALS
    SYMBOL g_pm  SYMBOL g_osi_funcs_p  SYMBOL pm_update_by_connectionles~3F93
    SYMBOL pm_on_coex_schm_status_config  SYMBOL pm_coex_schm_process_restart
$14 L:
  A1 48 ENTRY,  A2 $78 L BEQZ,
  A3 $0 L L32R,  A10 A1 MOV.N,  A8 A3 $160 L32I,  A8 A1 4 S32I.N,  MEMW,  $44 L J,
$2A L:
  A9 A10 0 L32I.N,  A9 A2 $44 L BNE,
  A2 A10 4 L32I.N,  A2 A8 4 S32I.N,  A2 $4 L L32R,  A2 A2 0 L32I.N,  A2 A2 176 L32I,
  MEMW,  A2 CALLX8,
  $4B L J,
$44 L:
  A8 A10 MOV.N,  A10 A10 4 L32I.N,  A10 $2A L BNEZ,
$4B L:
  A2 A1 4 L32I.N,  A2 A3 $160 S32I,  MEMW,  A8 $8 L L32R,  A8 CALLX8,
  A10 A3 13 L8UI,  A10 $78 L BNEZ,
  A8 $C L L32R,  A8 CALLX8,
  A2 $4 L L32R,  A2 A2 0 L32I.N,  A10 A2 $194 L32I,  A10 CALLX8,
  A10 $78 L BEQZ.N,
  A8 $10 L L32R,  A8 CALLX8,
$78 L:
  RETW.N,
$7A L:  END-CODE

CODE pm_connectionless_module_s~04A2   \ pm_connectionless_module_set_wake_interval: .text.pm_connectionless_module_set_wake_interval $8
  $0 2 LITERALS
    SYMBOL g_pm  SYMBOL pm_update_by_connectionles~3F93
$8 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A8 $18A S16I,  MEMW,  MEMW,  A8 $4 L L32R,
  A8 CALLX8,
  RETW.N,
$1F L:  END-CODE

CODE pm_set_beacon_duration   \ .wifislprxiram.62 $10
  $0 4 LITERALS
    SYMBOL g_pm  SYMBOL rodata_wlog_debug.63  SYMBOL pm_on_sample_beacon
    SYMBOL wifi_log
$10 L:
  A1 32 ENTRY,  A3 $0 L L32R,  A8 A3 $1D0 L32I,  A8 A2 $40 L BEQ,
  A8 $25 L BNEZ,
  A8 $8 L L32R,  A8 CALLX8,
$25 L:
  A14 A3 $1D0 L32I,  A11 1 MOVI.N,  A15 A2 MOV.N,  A12 4 MOVI.N,  A11 A11 12 SLLI,
  A13 $4 L L32R,  A10 1 MOVI,  A8 $C L L32R,  A8 CALLX8,
  A2 A3 $1D0 S32I,
$40 L:
  MEMW,  RETW.N,
$45 L:  END-CODE

CODE pm_get_beacon_duration   \ .text.pm_get_beacon_duration $4
  $0 1 LITERALS
    SYMBOL g_pm
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A8 $1D0 L32I,  RETW.N,
$F L:  END-CODE

CODE pm_beacon_add_total_counter   \ .text.pm_beacon_add_total_counter $8
  $0 2 LITERALS
    SYMBOL g_pm  SYMBOL ptr_beacon_offset_funcs
$8 L:
  A1 32 ENTRY,  A9 $0 L L32R,  A8 A9 $214 L32I,  A8 A8 1 ADDI.N,  A8 A9 $214 S32I,
  A8 $4 L L32R,  A8 A8 0 L32I.N,  A8 A8 36 L32I.N,  A8 $25 L BEQZ.N,
  MEMW,  A8 CALLX8,
$25 L:
  RETW.N,
$27 L:  END-CODE

CODE pm_enable_beacon_monitor_timer   \ .wifislprxiram.30 $1C
  $0 7 LITERALS
    SYMBOL ptr_beacon_offset_funcs  SYMBOL g_osi_funcs_p  SYMBOL __udivdi3
    SYMBOL pm_beacon_miss_exceeded_wa~D8EB  SYMBOL pm_beacon_add_total_counter
    SYMBOL pm_beacon_monitor_tbtt_allowed  SYMBOL pm_beacon_monitor_tbtt_start
$1C L:
  A1 48 ENTRY,  A8 $0 L L32R,  A6 A2 $100 ADDMI,  A4 A8 0 L32I.N,  A3 A6 124 L8UI,
  A3 A3 10 SLLI,  A10 A4 16 L32I.N,  A7 A6 214 L8UI,  A5 A6 215 L8UI,
  A10 $45 L BNEZ,
$38 L:
  A4 A6 125 L8UI,  A4 A4 10 SLLI,  A3 A4 A3 ADD.N,  $AB L J,
  2 ZEROS,
$45 L:
  A10 CALLX8,
  A10 $38 L BEQZ,
  A4 $4 L L32R,  A4 A4 0 L32I.N,  A10 A4 $110 L32I,  A10 CALLX8,
  A14 A10 A10 OR,  A12 A2 92 L32I,  A4 A2 88 L32I,  A11 A12 $68 L BLTU,
  A12 A11 $99 L BNE,
  A10 A4 $99 L BGEU,
$68 L:
  A10 A4 A14 SUB,  A9 A2 44 L32I.N,  A15 1 MOVI.N,  A4 A10 $75 L BLTU,
  A15 0 MOVI,
$75 L:
  A11 A12 A11 SUB,  A13 0 MOVI.N,  A12 A9 MOV.N,  A11 A11 A15 SUB,  A9 A1 4 S32I.N,
  A14 A1 0 S32I,  MEMW,  A8 $8 L L32R,  A8 CALLX8,
  A10 A10 1 ADDI.N,  A9 A1 4 L32I.N,  A10 A10 A9 MULL,  A4 A4 A10 SUB,
  A14 A1 0 L32I.N,
$99 L:
  A10 A14 A4 SUB,  A11 0 MOVI.N,  A9 A2 40 L32I.N,  A10 A9 $A9 L BGEU,
  A9 A9 A14 SUB,  A11 A9 A4 ADD,
$A9 L:
  A3 A3 A11 ADD.N,
$AB L:
  A4 A6 100 L8UI,  A4 $B5 L BNEZ.N,
$B0 L:
  A3 0 MOVI.N,  $132 L J,
$B5 L:
  A8 A6 212 L8UI,  A8 $B0 L BNEZ,
  A4 A2 13 L8UI,  A4 $B0 L BEQZ,
  A8 A1 0 S32I.N,  MEMW,  A8 $C L L32R,  A8 CALLX8,
  A9 A2 $180 L16SI,  A4 A2 $182 L16SI,  A8 A1 0 L32I.N,  A10 $EB L BEQZ.N,
  A9 A4 $E5 L BGE,
$D9 L:
  A6 $0 L L32R,  A4 A6 0 L32I.N,  A10 A4 12 L32I.N,  A10 $121 L BNEZ.N,
  $F7 L J,
$E5 L:
  A8 A2 $180 S16I,  MEMW,
$EB L:
  A8 A2 $180 L16SI,  A8 A4 $B0 L BGE,
  MEMW,  $D9 L J,
$F7 L:
  A8 $10 L L32R,  A8 CALLX8,
  A4 $4 L L32R,  A10 $168 MOVI,  A11 A3 MOV.N,  A12 0 MOVI.N,  A10 A2 A10 ADD.N,
  A4 A4 0 L32I.N,  A4 A4 248 L32I,  A4 CALLX8,
  A10 A2 MOV.N,  A8 $14 L L32R,  A8 CALLX8,
  A3 A10 MOV.N,  A10 $B0 L BEQZ,
  $12A L J,
$121 L:
  A10 CALLX8,
  A10 $B0 L BNEZ,
  $F7 L J,
$12A L:
  A10 A2 MOV.N,  A8 $18 L L32R,  A8 CALLX8,
$132 L:
  A5 A5 -1 ADDI.N,  A5 A5 A7 SUB,  A2 1 MOVI.N,  A2 A3 A5 MOVNEZ,  RETW.N,
$13E L:  END-CODE

CODE pm_tbtt_process   \ .wifislprxiram.48 $64
  $0 25 LITERALS
    SYMBOL g_osi_funcs_p  SYMBOL g_pm  $A4 SYMBOL+ g_pm  $F4 SYMBOL+ g_pm
    SYMBOL g_pm_cnt  SYMBOL g_mesh_is_started  SYMBOL g_mesh_is_root
    SYMBOL g_mesh_init_ps_type  $3FF73C00 T,  SYMBOL g_wifi_mac_time_delta
    SYMBOL pm_check_state  SYMBOL hal_pm_unblock_txq  SYMBOL pm_save_tbtt_info
    SYMBOL pm_enable_beacon_monitor_timer  SYMBOL pm_coex_schm_overall_period_get
    SYMBOL pm_beacon_monitor_tbtt_start  SYMBOL wifi_gpio_debug  SYMBOL pm_dream
    SYMBOL pm_coex_tbtt_process  SYMBOL pm_noise_check
    SYMBOL pm_mesh_set_next_tbtt  SYMBOL pm_set_next_tbtt  SYMBOL pm_go_to_wake
    SYMBOL pm_update_params  SYMBOL pm_update_by_connectionles~3F93
$64 L:
  A1 32 ENTRY,  A7 $0 L L32R,  A2 A7 0 L32I.N,  A10 A2 $110 L32I,  A10 CALLX8,
  A4 A10 MOV.N,  A5 A11 A11 OR,  A8 $28 L L32R,  A8 CALLX8,
  A2 $4 L L32R,  A3 A2 13 L8UI,  A3 $B0 L BNEZ.N,
  A3 A7 0 L32I,  A10 $8 L L32R,  A3 A3 236 L32I,  A3 CALLX8,
  A3 A7 0 L32I.N,  A10 $C L L32R,  A3 A3 236 L32I,  A3 CALLX8,
  A3 1 MOVI.N,  A3 A2 24 S8I,  MEMW,  MEMW,  A8 $2C L L32R,  A8 CALLX8,
  $1E1 L J,
$B0 L:
  A10 A2 MOV.N,  A6 1 MOVI.N,  A3 A2 17 L8UI,  A6 A2 17 S8I,  MEMW,  MEMW,
  A8 $30 L L32R,  A8 CALLX8,
  A10 A2 MOV.N,  A8 $34 L L32R,  A8 CALLX8,
  A10 $EC L BNEZ.N,
  A6 A7 0 L32I,  A10 A6 $194 L32I,  A10 CALLX8,
  A10 $EC L BEQZ.N,
  A8 $38 L L32R,  A8 CALLX8,
  A10 1 $EC L BEQI,
  A10 A2 MOV.N,  A8 $3C L L32R,  A8 CALLX8,
$EC L:
  A11 2 MOVI.N,  A10 0 MOVI.N,  A8 $40 L L32R,  A8 CALLX8,
  A8 A2 1 L8UI,  A6 $10 L L32R,  A8 2 $116 L BNEI,
  A8 $44 L L32R,  A8 CALLX8,
  A8 0 MOVI.N,  A8 A2 18 S8I,  MEMW,  A8 A6 16 L32I,  A8 A8 1 ADDI,  A8 A6 16 S32I,
$116 L:
  A3 $124 L BEQZ.N,
$118 L:
  A3 A2 14 L8UI,  A3 $182 L BNEZ,
  MEMW,  $12E L J,
$124 L:
  A3 A7 0 L32I.N,  A10 A3 $194 L32I,  A10 CALLX8,
  A10 $139 L BNEZ.N,
$12E L:
  A3 $14 L L32R,  A3 A3 0 L8UI,  A3 $145 L BNEZ.N,
  $169 L J,
$139 L:
  A3 A2 $200 ADDMI,  A3 A3 29 L8UI,  A3 $118 L BNEZ,
  $12E L J,
$145 L:
  A3 $18 L L32R,  MEMW,  A3 A3 0 L8UI,  A3 A3 0 8 EXTUI,  A3 $169 L BNEZ.N,
  A8 $1C L L32R,  A8 A8 0 L32I.N,  A8 $169 L BEQZ,
  A7 A7 0 L32I,  A10 A7 $110 L32I,  A10 CALLX8,
  $17C L J,
  2 ZEROS,
$169 L:
  A3 $20 L L32R,  MEMW,  A10 A3 0 L32I.N,  A3 $24 L L32R,  A3 A3 0 L32I.N,
  A10 A10 A3 ADD,  A3 0 MOVI,
$17C L:
  A10 A2 80 S32I,  A3 A2 84 S32I,
$182 L:
  A11 A5 MOV.N,  A10 A4 MOV.N,  A3 A2 28 L16UI,  A3 A3 1 ADDI.N,  A3 A2 28 S16I,
  MEMW,  A3 A6 0 L32I.N,  A3 A3 1 ADDI.N,  A3 A6 0 S32I,  MEMW,  A8 $48 L L32R,
  A8 CALLX8,
  A8 $4C L L32R,  A8 CALLX8,
  A8 $50 L L32R,  A8 CALLX8,
  A10 $1E1 L BNEZ.N,
  A3 A2 $100 ADDMI,  A8 $54 L L32R,  A8 CALLX8,
  A4 A3 212 L8UI,  A4 $1D0 L BEQZ.N,
  A4 A2 14 L8UI,  A4 $1D0 L BEQZ.N,
  A10 A2 15 L8UI,  A10 $1D0 L BNEZ,
  A11 A10 MOV.N,  A8 $58 L L32R,  A8 CALLX8,
$1D0 L:
  A8 $5C L L32R,  A8 CALLX8,
  A2 A3 140 L8UI,  A2 $1E1 L BNEZ.N,
  A8 $60 L L32R,  A8 CALLX8,
$1E1 L:
  RETW.N,
$1E3 L:  END-CODE

CODE pm_beacon_monitor_tbtt_tim~C271   \ pm_beacon_monitor_tbtt_timeout_process: .wifislprxiram.45 $8
  $0 2 LITERALS
    SYMBOL g_osi_funcs_p  SYMBOL pm_tbtt_process
$8 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A8 A8 0 L32I.N,  A10 A2 $184 L32I,  A8 A8 236 L32I,
  A8 CALLX8,
  A10 0 MOVI.N,  A8 $4 L L32R,  A8 CALLX8,
  RETW.N,
$23 L:  END-CODE

CODE pm_beacon_add_loss_counter   \ .text.pm_beacon_add_loss_counter $8
  $0 2 LITERALS
    SYMBOL g_pm  SYMBOL ptr_beacon_offset_funcs
$8 L:
  A1 32 ENTRY,  A9 $0 L L32R,  A8 A9 $218 L32I,  A8 A8 1 ADDI.N,  A8 A9 $218 S32I,
  A8 $4 L L32R,  A8 A8 0 L32I.N,  A8 A8 40 L32I.N,  A8 $25 L BEQZ.N,
  MEMW,  A8 CALLX8,
$25 L:
  RETW.N,
$27 L:  END-CODE

CODE pm_beacon_monitor_timeout_~294B   \ pm_beacon_monitor_timeout_process: .wifislprxiram.44 $10
  $0 4 LITERALS
    SYMBOL g_osi_funcs_p  SYMBOL pm_beacon_add_loss_counter  SYMBOL pm_sleep
    SYMBOL wifi_gpio_debug
$10 L:
  A1 32 ENTRY,  A3 A2 $100 ADDMI,  A4 A3 100 L8UI,  A4 $7C L BEQZ,
  A4 A3 212 L8UI,  A4 $7C L BNEZ,
  A8 A2 13 L8UI,  A8 $7C L BEQZ,
  A8 $4 L L32R,  A8 CALLX8,
  A8 A2 1 L8UI,  A8 1 $72 L BNEI,
  A8 A2 $180 L16UI,  A10 A8 15 SEXT,  A8 A8 1 ADDI.N,  A8 A8 0 16 EXTUI,
  A9 A2 $182 L16SI,  A8 A2 $180 S16I,  MEMW,  A10 A9 $72 L BGE,
  A10 A4 A4 OR,  A9 A3 127 L8UI,  A8 A9 A8 MUL16U,  A8 A3 125 S8I,  A4 A2 17 S8I,
  MEMW,  A2 $0 L L32R,  A2 A2 0 L32I,  A2 A2 $1A0 L32I,  MEMW,  A2 CALLX8,
  A8 $8 L L32R,  A8 CALLX8,
$72 L:
  A11 2 MOVI.N,  A10 A11 MOV.N,  A8 $C L L32R,  A8 CALLX8,
$7C L:
  RETW.N,
$7E L:  END-CODE

CODE pm_beacon_loss_info_get   \ .text.pm_beacon_loss_info_get $4
  $0 1 LITERALS
    SYMBOL g_pm
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A9 A8 $214 L32I,  A9 A2 0 S32I.N,  A8 A8 $218 L32I,
  A8 A3 0 S32I.N,  MEMW,  RETW.N,
$19 L:  END-CODE

CODE pm_beacon_loss_info_clean   \ .text.pm_beacon_loss_info_clean $4
  $0 1 LITERALS
    SYMBOL g_pm
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A9 0 MOVI.N,  A9 A8 $218 S32I,  A9 A8 $214 S32I,
  MEMW,  RETW.N,
$17 L:  END-CODE

CODE pm_get_tx_blocks_retention_mask   \ .text.pm_get_tx_blocks_retention_mask $8
  $0 2 LITERALS
    $FFF1FFFF T,  SYMBOL g_pm
$8 L:
  A1 32 ENTRY,  A8 $4 L L32R,  A2 -1 MOVI.N,  A9 A8 13 L8UI,  A9 $2A L BEQZ.N,
  A9 A8 14 L8UI,  A2 $0 L L32R,  A9 $2A L BEQZ.N,
  A8 A8 $200 ADDMI,  A2 -1 MOVI.N,  A8 A8 30 L8UI,  A8 $2A L BEQZ.N,
  A2 $0 L L32R,
$2A L:
  RETW.N,
$2C L:  END-CODE

LINK-SYMBOLS   \ the cells of other packages that wait for these names
END-PACKAGE  PREVIOUS   \ not in the search order: ALSO name gives its words (README.md)
