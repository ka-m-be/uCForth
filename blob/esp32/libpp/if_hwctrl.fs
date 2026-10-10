\ if_hwctrl.fs - package PP-IF-HWCTRL: if_hwctrl.o of libpp.a (ESP32)
\
\ Made from if_hwctrl.S by s2fs.py, not by hand. Each function of C is a
\ CODE word: its bytes are those of the compiler, at the same offsets of
\ its section (L gives the address of an offset); its literals are at the
\ start of the word, after a jump. The data come first, then the
\ functions, each after those that it calls. The global symbols are
\ PUBLIC, the static ones PRIVATE. lib/esp32/clink.fs explains the words.
\
\ 103 functions (103 global), 11 data objects.
\ A function runs as C code with the windowed calls of the Xtensa: call
\ it with CALL-C ( x1 .. xn n xt -- x ), never by its name alone.
\
\ Names of other packages: a literal waits until its package comes
\ (LINK-SYMBOLS); a name outside the libraries until EXTERN gives it.
\ Uses:
\   PHY-PHY-CHIP-V7  phy_chan_dump_cfg phy_chan_filt_set
\   PP-ESF-BUF  esf_buf_alloc esf_buf_recycle
\   PP-HAL-AMPDU  hal_agreement_add_rx_ba hal_agreement_clr_rx_ba
\                 hal_agreement_del_rx_ba
\   PP-HAL-CRYPTO  hal_crypto_get_key_entry hal_crypto_is_key_valid
\   PP-HAL-MAC  hal_deinit hal_init hal_mac_deinit hal_mac_init
\               hal_mac_rx_set_policy hal_mac_set_addr hal_mac_set_bssid
\               hal_mac_set_csi hal_mac_set_rxq_policy hal_random
\               hal_rx_disable_bssid_check hal_rx_enable_bssid_check
\   PP-LMAC  lmacInit lmacIsIdle lmacSetAcParam lmac_stop_hw_txq
\   PP-PM  pm_attach pm_beacon_monitor_configure pm_deattach
\          pm_enable_sta_disconnected~B345 pm_force_scan_unlock
\          pm_get_keep_alive_time pm_is_open pm_register_pm_tx_null_cb
\          pm_scale_listen_interval pm_set_keep_alive_time
\          pm_set_sleep_min_active_time pm_set_sleep_wait_broadcas~0D70
\          pm_update_light_sleep_defa~2A79
\          pm_update_modem_sleep_defa~4980
\   PP-PM-BEACON-OFFSET  ptr_beacon_offset_funcs
\   PP-PP  ppGetTaskHdl ppRecycleRxPkt ppRegisterPromisRxCallback
\          ppRegisterRxCallback ppRegisterTxCallback
\          ppRegisterTxDoneUserAction~1FF4 ppSetBarRate ppSetInterface
\          ppTxPkt ppTxqEmpty pp_attach pp_create_task pp_deattach
\          pp_delete_task pp_post pp_register_config_cb
\          pp_register_michael_mic_fa~B450 pp_register_net80211_tx_cb
\          pp_register_timer_cb pp_stop_sw_txq
\   PP-PP-TIMER  pp_timer_register_post_cb
\   PP-TRC  rcUpdateAMPDUParam rcUpdatePhyMode rc_disable_trc
\           rc_disable_trc_by_interface rc_enable_trc rc_get_G6M_sched
\           rc_get_default_sched rc_get_fix_rate rc_get_trc
\           rc_get_trc_by_index rc_set_fix_rate trc_deinit
\           trc_get_80211_tx_rate trc_get_80211_tx_rate_config
\           trc_get_espnow_rate trc_init trc_onAmpduOp
\           trc_set_80211_tx_rate trc_set_80211_tx_rate_config
\           trc_set_espnow_rate trc_set_per_pkt_rate trc_update_def_rate
\           trc_update_ifx_phy_mode
\   PP-WDEV  wDevCtrl wDev_Crypto_Disable wDev_Get_Next_TBTT
\            wDev_Insert_KeyEntry wDev_ProcessFiq wDev_Reset_TBTT
\            wDev_Rxbuf_Deinit wDev_Rxbuf_Init wDev_SetAuthed
\            wDev_SetCurChannel wDev_Set_Beacon_Int wDev_remove_KeyEntry
\            wDev_remove_KeyEntry_all_cnx wdev_get_promis_ctrl_filter
\            wdev_get_promis_filter wdev_is_mgmt_hwdecr_enabled
\            wdev_nan_set_callbacks wdev_set_csi_rx_cb wdev_set_promis
\            wdev_set_promis_ctrl_filter wdev_set_promis_filter
\ Outside the libraries (../README.md):
\   esp_wifi_internal_on_coex_~BEAB esp_wifi_internal_on_coex_start
\   g_mesh_is_root g_mesh_is_started g_osi_funcs_p g_wifi_menuconfig memcmp
\   memcpy memset wifi_log wifi_nvs_set_sta_listen_in~6741
\   wifi_set_rx_policy
\ C names with more than 31 characters (Forth takes 31): the first 26,
\ ~ and 4 hex digits of their CRC-32 (../README.md):
\   esp_wifi_internal_free_rx_~3A6F = esp_wifi_internal_free_rx_buffer
\   esp_wifi_internal_on_coex_~BEAB = esp_wifi_internal_on_coex_schm_phase
\   ic_beacon_offset_set_rx_be~A378 = ic_beacon_offset_set_rx_beacon_standard
\   ic_register_michael_mic_fa~C193 = ic_register_michael_mic_failure_cb
\   ic_set_sleep_wait_broadcas~AD7E = ic_set_sleep_wait_broadcast_data_time
\   ic_update_light_sleep_defa~F6E8 = ic_update_light_sleep_default_params
\   ic_update_modem_sleep_defa~9511 = ic_update_modem_sleep_default_params
\   pm_enable_sta_disconnected~B345 = pm_enable_sta_disconnected_power_management
\   pm_set_sleep_wait_broadcas~0D70 = pm_set_sleep_wait_broadcast_data_time
\   pm_update_light_sleep_defa~2A79 = pm_update_light_sleep_default_params
\   pm_update_modem_sleep_defa~4980 = pm_update_modem_sleep_default_params
\   ppRegisterTxDoneUserAction~1FF4 = ppRegisterTxDoneUserActionCallback
\   pp_register_michael_mic_fa~B450 = pp_register_michael_mic_failure_cb
\   wifi_nvs_set_sta_listen_in~6741 = wifi_nvs_set_sta_listen_interval
\
\ Needs XTENSA (asm/xtensa.fs), ASM (lib/esp32/asm.fs) and CLINK
\ (lib/esp32/clink.fs) of ucf; the ESP32.

PACKAGE PP-IF-HWCTRL  ALSO ASM  ALSO CLINK   \ lib/package.fs
EXPORTS   \ the PUBLIC words are symbols for the other packages

\ ==== Data =============================================================

PRIVATE
CREATE rodata_wlog_info.11   \ .rodata_wlog_info.11, 12 bytes
  ," stop sw txq"  0 C,
CREATE rodata_wlog_info.10   \ .rodata_wlog_info.10, 20 bytes
  ," bb noise check init"  0 C,
PUBLIC
1 DATA: s_is_6m   \ .bss.s_is_6m
PRIVATE
CREATE rodata_wlog_error.9   \ .rodata_wlog_error.9, 26 bytes
  ," set vif: invalid index=%d"  0 C,
CREATE rodata_wlog_debug.8   \ .rodata_wlog_debug.8, 25 bytes
  ," filter: set rx policy=%d"  0 C,
CREATE rodata_wlog_debug.7   \ .rodata_wlog_debug.7, 25 bytes
  ," filter: set rx policy=%d"  0 C,
CREATE rodata_wlog_debug.6   \ .rodata_wlog_debug.6, 25 bytes
  ," filter: set rx policy=%d"  0 C,
CREATE rodata_wlog_debug.5   \ .rodata_wlog_debug.5, 25 bytes
  ," filter: set rx policy=%d"  0 C,
CREATE rodata_wlog_debug.4   \ .rodata_wlog_debug.4, 25 bytes
  ," filter: set rx policy=%d"  0 C,
CREATE rodata_wlog_debug.3   \ .rodata_wlog_debug.3, 25 bytes
  ," filter: set rx policy=%d"  0 C,
PUBLIC
40 DATA: if_ctrl   \ .bss.if_ctrl

\ ==== Code =============================================================

CODE bb_intr_handl   \ .text.bb_intr_handl $4
  $0 1 LITERALS
    $3FF5D04C T,
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  MEMW,  A8 A8 0 L32I.N,  RETW.N,
$11 L:  END-CODE

CODE ic_get_addr   \ .text.ic_get_addr $4
  $0 1 LITERALS
    SYMBOL if_ctrl
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A2 0 8 EXTUI,  A2 A2 A2 ADDX4,  A2 A2 A8 ADDX2,
  RETW.N,
$15 L:  END-CODE

CODE ic_interface_enabled   \ .wifislprxiram.2 $4
  $0 1 LITERALS
    SYMBOL wDevCtrl
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A8 A8 49 L8UI,  A2 SSR,  A2 A8 SRL,  A2 A2 0 1 EXTUI,
  RETW.N,
$18 L:  END-CODE

CODE ic_is_pure_sta   \ .text.ic_is_pure_sta $4
  $0 1 LITERALS
    SYMBOL if_ctrl
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A2 0 8 EXTUI,  A2 A2 A2 ADDX4,  A8 A2 A8 ADDX2,
  A2 0 MOVI,  A9 A8 6 L8UI,  A9 1 $28 L BEQI,
  A2 A8 7 L8UI,  A8 1 MOVI,  A2 A2 A8 XOR,  A2 A2 0 8 EXTUI,
$28 L:
  A2 A2 0 1 EXTUI,  RETW.N,
$2D L:  END-CODE

CODE ic_get_ptk_alg   \ .text.ic_get_ptk_alg $4
  $0 1 LITERALS
    SYMBOL if_ctrl
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A2 0 8 EXTUI,  A2 A2 A2 ADDX4,  A2 A2 A8 ADDX2,
  A2 A2 8 L8UI,  RETW.N,
$18 L:  END-CODE

CODE ic_disable_crypto   \ .text.ic_disable_crypto $8
  $0 2 LITERALS
    SYMBOL if_ctrl  SYMBOL wDev_Crypto_Disable
$8 L:
  A1 32 ENTRY,  A3 A3 0 8 EXTUI,  A10 A2 0 8 EXTUI,  A3 $2E L BNEZ.N,
  A9 $0 L L32R,  A8 A10 A10 ADDX4,  A8 A8 A9 ADDX2,  A3 A8 8 S8I,  A3 A8 9 S8I,
  MEMW,  MEMW,  A8 $4 L L32R,  A8 CALLX8,
$2E L:
  RETW.N,
$30 L:  END-CODE

CODE ic_del_ptk_and_gtk_alg   \ .text.ic_del_ptk_and_gtk_alg $4
  $0 1 LITERALS
    SYMBOL if_ctrl
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A2 0 8 EXTUI,  A2 A2 A2 ADDX4,  A2 A2 A8 ADDX2,
  A8 0 MOVI.N,  A8 A2 8 S8I,  A8 A2 9 S8I,  MEMW,  MEMW,  RETW.N,
$23 L:  END-CODE

CODE ic_set_key   \ .text.ic_set_key $C
  $0 3 LITERALS
    SYMBOL if_ctrl  SYMBOL g_osi_funcs_p  SYMBOL wDev_Insert_KeyEntry
$C L:
  A1 64 ENTRY,  A2 A2 0 8 EXTUI,  A12 A4 0 8 EXTUI,  A8 A2 2 SLLI,  A4 $0 L L32R,
  A8 A8 A2 ADD.N,  A6 A6 0 8 EXTUI,  A3 A3 0 8 EXTUI,  A9 A1 64 L8UI,
  A8 A8 A4 ADDX2,  A10 A1 68 L8UI,  A11 A1 72 L8UI,  A6 4 $40 L BLTUI,
  A3 A8 8 S8I,  MEMW,  MEMW,  $4B L J,
  2 ZEROS,
$40 L:
  A3 $45 L BNEZ.N,
  A3 A8 8 S8I,
$45 L:
  A3 A8 9 S8I,  MEMW,
$4B L:
  A4 $4 L L32R,  A8 A4 0 L32I.N,  A8 A8 216 L32I,  A9 A1 20 S32I.N,
  A10 A1 24 S32I.N,  A11 A1 28 S32I.N,  A12 A1 16 S32I.N,  MEMW,  A8 CALLX8,
  A15 A7 MOV.N,  A14 A6 MOV.N,  A13 A5 MOV.N,  A11 A1 28 L32I.N,  A10 A1 24 L32I.N,
  A9 A1 20 L32I.N,  A12 A1 16 L32I.N,  A11 A1 8 S32I.N,  A11 A2 MOV.N,
  A10 A1 4 S32I.N,  A10 A3 MOV.N,  A9 A1 0 S32I.N,  MEMW,  A8 $8 L L32R,  A8 CALLX8,
  A2 A4 0 L32I.N,  A2 A2 220 L32I,  A2 CALLX8,
  RETW.N,
$8C L:  END-CODE

CODE ic_get_key   \ .text.ic_get_key $C
  $0 3 LITERALS
    SYMBOL memset  SYMBOL hal_crypto_get_key_entry  SYMBOL memcpy
$C L:
  A1 64 ENTRY,  A8 A1 64 L8UI,  A12 9 MOVI.N,  A11 0 MOVI.N,  A10 A1 MOV.N,
  A6 A6 0 8 EXTUI,  A8 A1 16 S32I.N,  MEMW,  A8 $0 L L32R,  A8 CALLX8,
  A13 A1 MOV.N,  A11 A7 MOV.N,  A10 A6 MOV.N,  A8 A1 16 L32I.N,  A12 A8 MOV.N,
  A8 $4 L L32R,  A8 CALLX8,
  A12 6 MOVI.N,  A8 A1 0 L8UI,  A11 A1 3 ADDI.N,  A10 A5 MOV.N,  A8 A2 0 S8I,  MEMW,
  A2 0 MOVI.N,  A8 A1 1 L8UI,  A8 A3 0 S32I.N,  A8 A1 2 L8UI,  A8 A4 0 S32I.N,
  MEMW,  A8 $8 L L32R,  A8 CALLX8,
  RETW.N,
$5C L:  END-CODE

CODE ic_obtain_key   \ .text.ic_obtain_key $C
  $0 3 LITERALS
    SYMBOL hal_crypto_is_key_valid  SYMBOL ic_get_key  SYMBOL memcmp
$C L:
  A1 96 ENTRY,  A8 3 MOVI.N,  A3 A3 0 8 EXTUI,  A2 A2 0 8 EXTUI,  A8 A1 32 S8I,
  A9 3 MOVI.N,  A8 0 MOVI.N,  A4 A4 0 8 EXTUI,  A3 A1 52 S32I.N,  A3 4 MOVI.N,
  A2 A1 48 S32I.N,  A8 A1 28 S32I.N,  A9 A1 24 S32I,  A8 A1 16 S32I,  A8 A1 20 S16I,
  MEMW,
$35 L:
  A10 A3 MOV.N,  MEMW,  A8 $0 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  A10 $50 L BNEZ.N,
$44 L:
  A3 A3 1 ADDI.N,  A9 25 MOVI.N,  A3 A9 $35 L BNE,
  A2 0 MOVI.N,  $94 L J,
$50 L:
  A10 A7 0 8 EXTUI,  A10 A1 0 S32I,  A15 A6 MOV.N,  A14 A3 A3 OR,  A13 A1 16 ADDI,
  A12 A1 24 ADDI,  A11 A1 28 ADDI,  A10 A1 32 ADDI,  MEMW,  A8 $4 L L32R,
  A8 CALLX8,
  A10 A1 32 L8UI,  A8 A1 48 L32I.N,  A10 A8 $44 L BNE,
  A10 A1 28 L32I.N,  A8 A1 52 L32I.N,  A8 A10 $44 L BNE,
  A10 A1 24 L32I.N,  A4 A10 $44 L BNE,
  A12 6 MOVI.N,  A11 A5 MOV.N,  A10 A1 16 ADDI,  A8 $8 L L32R,  A8 CALLX8,
  A10 $44 L BNEZ,
$94 L:
  RETW.N,
$96 L:  END-CODE

CODE ic_get_rssi   \ .text.ic_get_rssi $8
  $0 2 LITERALS
    SYMBOL wDevCtrl  SYMBOL rc_get_trc
$8 L:
  A1 32 ENTRY,  A10 A2 0 8 EXTUI,  A11 A3 MOV.N,  A8 $4 L L32R,  A8 CALLX8,
  A2 31 MOVI.N,  A10 $29 L BEQZ.N,
  A8 $0 L L32R,  A2 A10 3 L8UI,  A8 A8 46 L8UI,  A2 A2 A8 SUB,  A2 A2 0 8 EXTUI,
$29 L:
  RETW.N,
$2B L:  END-CODE

CODE ic_tx_pkt   \ .text.ic_tx_pkt $4
  $0 1 LITERALS
    SYMBOL ppTxPkt
$4 L:
  A1 32 ENTRY,  A11 1 MOVI.N,  A10 A2 MOV.N,  A8 $0 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  RETW.N,
$15 L:  END-CODE

CODE ic_ebuf_alloc   \ .text.ic_ebuf_alloc $4
  $0 1 LITERALS
    SYMBOL esf_buf_alloc
$4 L:
  A1 32 ENTRY,  A12 A4 MOV.N,  A11 A3 MOV.N,  A10 A2 A2 OR,  A8 $0 L L32R,
  A8 CALLX8,
  A2 A10 MOV.N,  RETW.N,
$18 L:  END-CODE

CODE ic_ebuf_recycle_tx   \ .text.ic_ebuf_recycle_tx $4
  $0 1 LITERALS
    SYMBOL esf_buf_recycle
$4 L:
  A1 32 ENTRY,  A10 A2 A2 OR,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$12 L:  END-CODE

CODE ic_ebuf_recycle_rx   \ .text.ic_ebuf_recycle_rx $4
  $0 1 LITERALS
    SYMBOL ppRecycleRxPkt
$4 L:
  A1 32 ENTRY,  A10 A2 A2 OR,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$12 L:  END-CODE

CODE esp_wifi_internal_free_rx_~3A6F   \ esp_wifi_internal_free_rx_buffer: .text.esp_wifi_internal_free_rx_buffer $4
  $0 1 LITERALS
    SYMBOL ppRecycleRxPkt
$4 L:
  A1 32 ENTRY,  A10 A2 A2 OR,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$12 L:  END-CODE

CODE ic_register_tx_cb   \ .text.ic_register_tx_cb $4
  $0 1 LITERALS
    SYMBOL ppRegisterTxCallback
$4 L:
  A1 32 ENTRY,  A12 A4 0 8 EXTUI,  A11 A3 0 8 EXTUI,  A10 A2 MOV.N,  A8 $0 L L32R,
  A8 CALLX8,
  A2 A10 MOV.N,  RETW.N,
$19 L:  END-CODE

CODE ic_register_rx_cb   \ .text.ic_register_rx_cb $4
  $0 1 LITERALS
    SYMBOL ppRegisterRxCallback
$4 L:
  A1 32 ENTRY,  A11 A3 0 8 EXTUI,  A10 A2 MOV.N,  A8 $0 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  RETW.N,
$16 L:  END-CODE

CODE ic_register_timer_post_cb   \ .text.ic_register_timer_post_cb $4
  $0 1 LITERALS
    SYMBOL pp_timer_register_post_cb
$4 L:
  A1 32 ENTRY,  A10 A2 A2 OR,  A8 $0 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  RETW.N,
$14 L:  END-CODE

CODE ic_register_michael_mic_fa~C193   \ ic_register_michael_mic_failure_cb: .text.ic_register_michael_mic_failure_cb $4
  $0 1 LITERALS
    SYMBOL pp_register_michael_mic_fa~B450
$4 L:
  A1 32 ENTRY,  A10 A2 A2 OR,  A8 $0 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  RETW.N,
$14 L:  END-CODE

CODE ic_register_pp_tx_done_cb   \ .text.ic_register_pp_tx_done_cb $4
  $0 1 LITERALS
    SYMBOL ppRegisterTxDoneUserAction~1FF4
$4 L:
  A1 32 ENTRY,  A10 A2 A2 OR,  A8 $0 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  RETW.N,
$14 L:  END-CODE

CODE ic_set_promis_filter   \ .text.ic_set_promis_filter $4
  $0 1 LITERALS
    SYMBOL wdev_set_promis_filter
$4 L:
  A1 32 ENTRY,  A10 A2 A2 OR,  A8 $0 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  RETW.N,
$14 L:  END-CODE

CODE ic_get_promis_filter   \ .text.ic_get_promis_filter $4
  $0 1 LITERALS
    SYMBOL wdev_get_promis_filter
$4 L:
  A1 32 ENTRY,  A10 A2 A2 OR,  A8 $0 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  RETW.N,
$14 L:  END-CODE

CODE ic_set_promis_ctrl_filter   \ .text.ic_set_promis_ctrl_filter $4
  $0 1 LITERALS
    SYMBOL wdev_set_promis_ctrl_filter
$4 L:
  A1 32 ENTRY,  A10 A2 A2 OR,  A8 $0 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  RETW.N,
$14 L:  END-CODE

CODE ic_get_promis_ctrl_filter   \ .text.ic_get_promis_ctrl_filter $4
  $0 1 LITERALS
    SYMBOL wdev_get_promis_ctrl_filter
$4 L:
  A1 32 ENTRY,  A10 A2 A2 OR,  A8 $0 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  RETW.N,
$14 L:  END-CODE

CODE ic_register_promis_rx_cb   \ .text.ic_register_promis_rx_cb $4
  $0 1 LITERALS
    SYMBOL ppRegisterPromisRxCallback
$4 L:
  A1 32 ENTRY,  A10 A2 A2 OR,  A8 $0 L L32R,  A8 CALLX8,
  A8 0 MOVI.N,  A2 1 MOVI.N,  A2 A8 A10 MOVEQZ,  RETW.N,
$19 L:  END-CODE

CODE ic_register_csi_rx_cb   \ .text.ic_register_csi_rx_cb $4
  $0 1 LITERALS
    SYMBOL wdev_set_csi_rx_cb
$4 L:
  A1 32 ENTRY,  A11 A3 MOV.N,  A10 A2 MOV.N,  A8 $0 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  RETW.N,
$15 L:  END-CODE

CODE ic_register_config_cb   \ .text.ic_register_config_cb $4
  $0 1 LITERALS
    SYMBOL pp_register_config_cb
$4 L:
  A1 32 ENTRY,  A10 A2 MOV.N,  A2 0 MOVI.N,  A10 A2 $16 L BEQ,
  A8 $0 L L32R,  A8 CALLX8,
  A2 1 MOVI.N,
$16 L:
  RETW.N,
$18 L:  END-CODE

CODE ic_is_mgmt_hwdecr_enabled   \ .text.ic_is_mgmt_hwdecr_enabled $4
  $0 1 LITERALS
    SYMBOL wdev_is_mgmt_hwdecr_enabled
$4 L:
  A1 32 ENTRY,  A10 A2 0 8 EXTUI,  A8 $0 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  RETW.N,
$14 L:  END-CODE

CODE ic_register_pm_tx_null_cb   \ .text.ic_register_pm_tx_null_cb $4
  $0 1 LITERALS
    SYMBOL pm_register_pm_tx_null_cb
$4 L:
  A1 32 ENTRY,  A10 A2 MOV.N,  A2 0 MOVI.N,  A10 A2 $16 L BEQ,
  A8 $0 L L32R,  A8 CALLX8,
  A2 1 MOVI.N,
$16 L:
  RETW.N,
$18 L:  END-CODE

CODE ic_register_net80211_tx_cb   \ .text.ic_register_net80211_tx_cb $4
  $0 1 LITERALS
    SYMBOL pp_register_net80211_tx_cb
$4 L:
  A1 32 ENTRY,  A10 A2 MOV.N,  A2 0 MOVI.N,  A10 A2 $16 L BEQ,
  A8 $0 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,
$16 L:
  RETW.N,
$18 L:  END-CODE

CODE ic_register_timer_cb   \ .text.ic_register_timer_cb $4
  $0 1 LITERALS
    SYMBOL pp_register_timer_cb
$4 L:
  A1 32 ENTRY,  A10 A2 MOV.N,  A2 0 MOVI.N,  A10 A2 $16 L BEQ,
  A8 $0 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,
$16 L:
  RETW.N,
$18 L:  END-CODE

CODE ic_register_ftm_callbacks   \ .text.ic_register_ftm_callbacks $0
  $0 ORIGIN
  A1 32 ENTRY,  A2 0 MOVI.N,  RETW.N,
$7 L:  END-CODE

CODE ic_register_nan_callbacks   \ .text.ic_register_nan_callbacks $4
  $0 1 LITERALS
    SYMBOL wdev_nan_set_callbacks
$4 L:
  A1 32 ENTRY,  A10 A2 A2 OR,  A8 $0 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  RETW.N,
$14 L:  END-CODE

CODE ic_pp_post   \ .text.ic_pp_post $4
  $0 1 LITERALS
    SYMBOL pp_post
$4 L:
  A1 32 ENTRY,  A11 A3 MOV.N,  A10 A2 MOV.N,  A8 $0 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  RETW.N,
$15 L:  END-CODE

CODE ic_enable_sniffer   \ .text.ic_enable_sniffer $4
  $0 1 LITERALS
    SYMBOL wdev_set_promis
$4 L:
  A1 32 ENTRY,  A10 1 MOVI,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$12 L:  END-CODE

CODE ic_disable_sniffer   \ .text.ic_disable_sniffer $4
  $0 1 LITERALS
    SYMBOL wdev_set_promis
$4 L:
  A1 32 ENTRY,  A10 0 MOVI,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$12 L:  END-CODE

CODE ic_set_vif   \ .text.ic_set_vif $68
  $0 26 LITERALS
    SYMBOL if_ctrl  SYMBOL rodata_wlog_debug.3  SYMBOL rodata_wlog_debug.4
    SYMBOL rodata_wlog_debug.5  SYMBOL wDevCtrl  $3FF73084 T,  $7FFFFFFF T,
    SYMBOL rodata_wlog_debug.6  SYMBOL rodata_wlog_debug.7
    SYMBOL rodata_wlog_debug.8  SYMBOL rodata_wlog_error.9  SYMBOL wifi_log
    SYMBOL wifi_log  SYMBOL wifi_set_rx_policy  SYMBOL wifi_log
    SYMBOL ic_enable_sniffer  SYMBOL memcpy  SYMBOL ic_disable_crypto
    SYMBOL rc_disable_trc_by_interface  SYMBOL pm_force_scan_unlock
    SYMBOL wifi_log  SYMBOL wifi_log  SYMBOL wifi_log  SYMBOL wifi_set_rx_policy
    SYMBOL ic_disable_sniffer  SYMBOL wifi_log
$68 L:
  A1 32 ENTRY,  A2 A2 0 8 EXTUI,  A6 A6 0 8 EXTUI,  A7 A7 0 8 EXTUI,
  A3 2 $90 L BNEI,
  A3 $0 L L32R,  A2 A2 A2 ADDX4,  A2 A2 A3 ADDX2,  A5 A2 6 S8I,  A6 A2 7 S8I,  MEMW,
  MEMW,  $211 L J,
  1 ZEROS,
$90 L:
  A3 $153 L BNEZ,
  A2 $AC L BNEZ.N,
  A14 1 MOVI.N,  A13 $4 L L32R,  A10 A14 MOV.N,  A12 4 MOVI.N,  A11 12 MOVI.N,
  A8 $2C L L32R,  A8 CALLX8,
  A10 1 MOVI.N,  $C2 L J,
  1 ZEROS,
$AC L:
  A2 2 $CC L BNEI,
  A13 $8 L L32R,  A10 1 MOVI.N,  A14 11 MOVI.N,  A12 4 MOVI.N,  A11 12 MOVI.N,
  A8 $30 L L32R,  A8 CALLX8,
  A10 11 MOVI.N,
$C2 L:
  A8 $34 L L32R,  A8 CALLX8,
  $EE L J,
  1 ZEROS,
$CC L:
  A2 1 $E5 L BNEI,
  A13 $C L L32R,  A10 A2 MOV.N,  A14 9 MOVI.N,  A12 4 MOVI.N,  A11 12 MOVI.N,
  A8 $38 L L32R,  A8 CALLX8,
  A10 9 MOVI.N,  $C2 L J,
$E5 L:
  A2 3 $EE L BNEI,
  A8 $3C L L32R,  A8 CALLX8,
$EE L:
  A3 $0 L L32R,  A11 A4 A4 OR,  A10 A2 A2 ADDX4,  A4 A2 A2 ADDX4,  A10 A10 A3 ADDX2,
  A12 6 MOVI.N,  A3 A4 A3 ADDX2,  A8 $40 L L32R,  A8 CALLX8,
  A5 A3 6 S8I,  A8 1 MOVI.N,  A2 SSL,  A8 A8 SLL,  A6 A3 7 S8I,  MEMW,
  A3 $10 L L32R,  A4 A3 49 L8UI,  A8 A8 A4 OR,  A8 A8 0 8 EXTUI,  A8 A3 49 S8I,
  MEMW,  A8 $142 L BEQZ.N,
  A4 -1 MOVI.N,  A4 A4 31 SLLI,  A5 $14 L L32R,  MEMW,  A3 A5 0 L32I.N,
  A3 A3 A4 OR,  MEMW,  A3 A5 0 S32I.N,
$142 L:
  A11 A7 MOV.N,  A10 A2 A2 OR,  MEMW,  A8 $44 L L32R,  A8 CALLX8,
  $211 L J,
$153 L:
  A3 1 $159 L BEQI,
  $200 L J,
$159 L:
  A5 $10 L L32R,  A4 A5 49 L8UI,  A4 A2 $181 L BBC,
  A2 SSL,  A3 A3 SLL,  A6 -1 MOVI.N,  A3 A6 A3 XOR,  A4 A4 A3 AND,  A10 A2 MOV.N,
  A4 A5 49 S8I,  MEMW,  MEMW,  A8 $48 L L32R,  A8 CALLX8,
$181 L:
  A3 A5 49 L8UI,  A3 $1A2 L BNEZ.N,
  A4 $14 L L32R,  A5 $18 L L32R,  MEMW,  A3 A4 0 L32I.N,  A3 A3 A5 AND,  MEMW,
  A3 A4 0 S32I.N,  MEMW,  A8 $4C L L32R,  A8 CALLX8,
$1A2 L:
  A2 $1BC L BNEZ.N,
  A10 1 MOVI.N,  A14 2 MOVI.N,  A13 $1C L L32R,  A12 4 MOVI.N,  A11 12 MOVI.N,
  A8 $50 L L32R,  A8 CALLX8,
  A10 2 MOVI.N,  $1EB L J,
  2 ZEROS,
$1BC L:
  A2 2 $1D5 L BNEI,
  A13 $20 L L32R,  A10 1 MOVI.N,  A14 13 MOVI.N,  A12 4 MOVI.N,  A11 12 MOVI.N,
  A8 $54 L L32R,  A8 CALLX8,
  A10 13 MOVI.N,  $1EB L J,
$1D5 L:
  A2 1 $1F4 L BNEI,
  A13 $24 L L32R,  A10 A2 MOV.N,  A14 10 MOVI.N,  A12 4 MOVI.N,  A11 12 MOVI.N,
  A8 $58 L L32R,  A8 CALLX8,
  A10 10 MOVI.N,
$1EB L:
  A8 $5C L L32R,  A8 CALLX8,
  $211 L J,
$1F4 L:
  A2 3 $211 L BNEI,
  A8 $60 L L32R,  A8 CALLX8,
  $211 L J,
$200 L:
  A12 1 MOVI.N,  A13 $28 L L32R,  A14 A2 MOV.N,  A11 A12 MOV.N,  A10 6 MOVI.N,
  A8 $64 L L32R,  A8 CALLX8,
$211 L:
  RETW.N,
$213 L:  END-CODE

CODE ic_get_next_tbtt   \ .text.ic_get_next_tbtt $4
  $0 1 LITERALS
    SYMBOL wDev_Get_Next_TBTT
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  RETW.N,
$11 L:  END-CODE

CODE ic_del_rx_ba   \ .text.ic_del_rx_ba $4
  $0 1 LITERALS
    SYMBOL hal_agreement_del_rx_ba
$4 L:
  A1 32 ENTRY,  A10 A2 0 8 EXTUI,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$12 L:  END-CODE

CODE ic_reset_rx_ba   \ .text.ic_reset_rx_ba $4
  $0 1 LITERALS
    SYMBOL hal_agreement_clr_rx_ba
$4 L:
  A1 32 ENTRY,  A13 A5 0 8 EXTUI,  A12 A4 0 16 EXTUI,  A11 A3 0 8 EXTUI,
  A10 A2 0 8 EXTUI,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$1B L:  END-CODE

CODE ic_add_rx_ba   \ .text.ic_add_rx_ba $4
  $0 1 LITERALS
    SYMBOL hal_agreement_add_rx_ba
$4 L:
  A1 32 ENTRY,  A15 A7 0 8 EXTUI,  A14 A6 0 16 EXTUI,  A13 A5 0 8 EXTUI,
  A12 A4 MOV.N,  A11 A2 0 8 EXTUI,  A10 A3 0 8 EXTUI,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$20 L:  END-CODE

CODE ic_reset_tbtt   \ .text.ic_reset_tbtt $4
  $0 1 LITERALS
    SYMBOL wDev_Reset_TBTT
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$F L:  END-CODE

CODE ic_del_key_all   \ .text.ic_del_key_all $4
  $0 1 LITERALS
    SYMBOL wDev_remove_KeyEntry_all_cnx
$4 L:
  A1 32 ENTRY,  A11 A3 MOV.N,  A10 A2 0 8 EXTUI,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$14 L:  END-CODE

CODE ic_del_key   \ .text.ic_del_key $8
  $0 2 LITERALS
    SYMBOL g_osi_funcs_p  SYMBOL wDev_remove_KeyEntry
$8 L:
  A1 32 ENTRY,  A3 $0 L L32R,  A8 A3 0 L32I.N,  A8 A8 216 L32I,  A8 CALLX8,
  A10 A2 MOV.N,  A8 $4 L L32R,  A8 CALLX8,
  A3 A3 0 L32I.N,  A3 A3 220 L32I,  A3 CALLX8,
  RETW.N,
$28 L:  END-CODE

CODE ic_set_ac_param   \ .text.ic_set_ac_param $4
  $0 1 LITERALS
    SYMBOL lmacSetAcParam
$4 L:
  A1 32 ENTRY,  A14 A6 0 16 EXTUI,  A13 A5 0 8 EXTUI,  A12 A4 0 8 EXTUI,
  A11 A3 0 8 EXTUI,  A10 A2 0 8 EXTUI,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$1E L:  END-CODE

CODE ic_ampdu_op   \ .text.ic_ampdu_op $8
  $0 2 LITERALS
    SYMBOL rc_get_trc_by_index  SYMBOL trc_onAmpduOp
$8 L:
  A1 32 ENTRY,  A11 A3 0 8 EXTUI,  A10 A2 0 8 EXTUI,  A8 $0 L L32R,  A8 CALLX8,
  A4 A4 0 8 EXTUI,  A12 A4 MOV.N,  A11 2 MOVI,  A5 1 $24 L BEQI,
  A11 1 MOVI.N,
$24 L:
  A8 $4 L L32R,  A8 CALLX8,
  RETW.N,
$2C L:  END-CODE

CODE esp_mesh_set_6m_rate   \ .text.esp_mesh_set_6m_rate $4
  $0 1 LITERALS
    SYMBOL s_is_6m
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A8 0 S8I,  MEMW,  A2 0 MOVI.N,  MEMW,  RETW.N,
$17 L:  END-CODE

CODE ic_set_trc   \ .text.ic_set_trc $20
  $0 8 LITERALS
    SYMBOL if_ctrl  SYMBOL g_mesh_is_started  SYMBOL s_is_6m
    SYMBOL g_mesh_is_root  SYMBOL rc_enable_trc  SYMBOL rcUpdateAMPDUParam
    SYMBOL rcUpdatePhyMode  SYMBOL rc_disable_trc
$20 L:
  A1 32 ENTRY,  A3 A2 0 L8UI,  A10 A2 1 L8UI,  A11 A2 4 L32I,  A12 A2 8 L8UI,
  A3 $C8 L BEQZ,
  A13 A2 10 L8UI,  A4 A2 20 L32I,  A8 $10 L L32R,  A8 CALLX8,
  A3 A10 MOV.N,  A10 $CE L BEQZ,
  A8 A2 1 L8UI,  A9 A8 A8 ADDX4,  A5 $0 L L32R,  A9 A9 A5 ADDX2,  A5 A9 7 L8UI,
  A9 $4 L L32R,  A9 A9 0 L8UI,  A9 $74 L BEQZ.N,
  A8 $65 L BEQZ.N,
$5C L:
  A5 $8 L L32R,  A5 A5 0 L8UI,  $74 L J,
$65 L:
  A8 $C L L32R,  MEMW,  A8 A8 0 L8UI,  A8 A8 0 8 EXTUI,  A8 $5C L BEQZ,
$74 L:
  A10 A3 MOV.N,  A12 A2 26 L8UI,  A13 A12 2 3 EXTUI,  A12 A12 0 2 EXTUI,
  A11 A2 12 L8UI,  A8 $14 L L32R,  A8 CALLX8,
  A8 A2 12 L8UI,  A8 A3 130 S8I,  MEMW,  A4 $A9 L BEQZ.N,
  A8 A4 1 L8UI,  A8 A3 135 S8I,  MEMW,  A4 A4 0 L8UI,  A4 $A9 L BEQZ.N,
  A4 6 MOVI.N,  A4 A3 130 S8I,  MEMW,
$A9 L:
  A12 A5 MOV.N,  A10 A3 MOV.N,  A11 A2 16 L32I.N,  A11 A11 96 ADDI,  A11 A11 7 SEXT,
  A14 A2 25 L8UI,  A13 A2 24 L8UI,  MEMW,  A8 $18 L L32R,  A8 CALLX8,
  $CE L J,
  1 ZEROS,
$C8 L:
  A8 $1C L L32R,  A8 CALLX8,
$CE L:
  RETW.N,
$D0 L:  END-CODE

CODE ic_bb_check_noise_init   \ .text.ic_bb_check_noise_init $10
  $0 4 LITERALS
    SYMBOL rodata_wlog_info.10  SYMBOL bb_intr_handl  SYMBOL g_osi_funcs_p
    SYMBOL wifi_log
$10 L:
  A1 32 ENTRY,  A13 $0 L L32R,  A11 1 MOVI.N,  A12 3 MOVI.N,  A11 A11 11 SLLI,
  A10 6 MOVI.N,  A8 $C L L32R,  A8 CALLX8,
  A2 $8 L L32R,  A12 0 MOVI.N,  A10 9 MOVI.N,  A8 A2 0 L32I,  A11 $4 L L32R,
  A8 A8 16 L32I,  A8 CALLX8,
  A10 $200 MOVI,  A2 A2 0 L32I.N,  A2 A2 20 L32I.N,  A2 CALLX8,
  RETW.N,
$44 L:  END-CODE

CODE ic_deinit   \ .text.ic_deinit $10
  $0 4 LITERALS
    SYMBOL pm_deattach  SYMBOL wDev_Rxbuf_Deinit  SYMBOL pp_deattach
    SYMBOL trc_deinit
$10 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A8 CALLX8,
  A8 $4 L L32R,  A8 CALLX8,
  A8 $8 L L32R,  A8 CALLX8,
  A8 $C L L32R,  A8 CALLX8,
  A2 0 MOVI.N,  RETW.N,
$2F L:  END-CODE

CODE ic_init   \ .text.ic_init $2C
  $0 11 LITERALS
    SYMBOL esp_wifi_internal_on_coex_start  SYMBOL g_osi_funcs_p
    SYMBOL esp_wifi_internal_on_coex_~BEAB  SYMBOL g_wifi_menuconfig
    SYMBOL trc_init  SYMBOL lmacInit  SYMBOL pp_attach  SYMBOL wDev_Rxbuf_Init
    SYMBOL pm_attach  SYMBOL pm_enable_sta_disconnected~B345  SYMBOL ic_deinit
$2C L:
  A1 32 ENTRY,  A10 A4 0 8 EXTUI,  A8 $10 L L32R,  A8 CALLX8,
  A8 $14 L L32R,  A8 CALLX8,
  A10 A3 MOV.N,  A8 $18 L L32R,  A8 CALLX8,
  A5 A2 MOV.N,  A2 A10 A10 OR,  A10 $90 L BNEZ,
  A10 A5 MOV.N,  A8 $1C L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  A10 $90 L BNEZ.N,
  A8 $20 L L32R,  A8 CALLX8,
  A3 $4 L L32R,  A8 A3 0 L32I.N,  A10 $0 L L32R,  A8 A8 $1D0 L32I,  A8 CALLX8,
  A10 A2 MOV.N,  A3 A3 0 L32I,  A11 $8 L L32R,  A3 A3 $1CC L32I,  A3 CALLX8,
  A3 $C L L32R,  A3 A3 72 L8UI,  A3 $96 L BEQZ.N,
  A10 1 MOVI.N,  A8 $24 L L32R,  A8 CALLX8,
  $96 L J,
  1 ZEROS,
$90 L:
  A8 $28 L L32R,  A8 CALLX8,
$96 L:
  RETW.N,
$98 L:  END-CODE

CODE ic_set_interrupt_handler   \ .text.ic_set_interrupt_handler $10
  $0 4 LITERALS
    SYMBOL g_wifi_menuconfig  SYMBOL g_osi_funcs_p  SYMBOL wDev_ProcessFiq
    SYMBOL hal_init
$10 L:
  A1 32 ENTRY,  A8 $C L L32R,  A8 CALLX8,
  A2 $4 L L32R,  A12 0 MOVI.N,  A13 1 MOVI.N,  A11 A12 MOV.N,  A9 $0 L L32R,
  A8 A2 0 L32I.N,  A10 A9 52 L32I.N,  A8 A8 8 L32I.N,  A8 CALLX8,
  A12 0 MOVI.N,  A10 A12 MOV.N,  A8 A2 0 L32I.N,  A11 $8 L L32R,  A8 A8 16 L32I.N,
  A8 CALLX8,
  A10 1 MOVI.N,  A2 A2 0 L32I.N,  A2 A2 20 L32I.N,  A2 CALLX8,
  RETW.N,
$47 L:  END-CODE

CODE ic_clear_interrupt_handler   \ .text.ic_clear_interrupt_handler $8
  $0 2 LITERALS
    SYMBOL g_osi_funcs_p  SYMBOL hal_deinit
$8 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A10 1 MOVI.N,  A8 A2 0 L32I.N,  A8 A8 24 L32I,
  A8 CALLX8,
  A12 0 MOVI.N,  A11 A12 MOV.N,  A8 A2 0 L32I.N,  A10 A12 MOV.N,  A8 A8 16 L32I.N,
  A8 CALLX8,
  A11 0 MOVI.N,  A10 A11 MOV.N,  A2 A2 0 L32I.N,  A2 A2 12 L32I.N,  A2 CALLX8,
  A8 $4 L L32R,  A8 CALLX8,
  RETW.N,
$38 L:  END-CODE

CODE ic_enable_rx   \ .text.ic_enable_rx $4
  $0 1 LITERALS
    $3FF73084 T,
$4 L:
  A1 32 ENTRY,  A10 $0 L L32R,  A9 -1 MOVI.N,  A9 A9 31 SLLI,  MEMW,
  A8 A10 0 L32I.N,  A8 A8 A9 OR,  MEMW,  A8 A10 0 S32I.N,  MEMW,  RETW.N,
$21 L:  END-CODE

CODE ic_disable_rx   \ .text.ic_disable_rx $8
  $0 2 LITERALS
    $3FF73084 T,  $7FFFFFFF T,
$8 L:
  A1 32 ENTRY,  A9 $0 L L32R,  A10 $4 L L32R,  MEMW,  A8 A9 0 L32I.N,
  A8 A8 A10 AND,  MEMW,  A8 A9 0 S32I.N,  MEMW,  RETW.N,
$23 L:  END-CODE

CODE ic_set_beacon_int   \ .text.ic_set_beacon_int $4
  $0 1 LITERALS
    SYMBOL wDev_Set_Beacon_Int
$4 L:
  A1 32 ENTRY,  A10 A2 A2 OR,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$12 L:  END-CODE

CODE ic_set_mac   \ .text.ic_set_mac $4
  $0 1 LITERALS
    SYMBOL hal_mac_set_addr
$4 L:
  A1 32 ENTRY,  A11 A3 MOV.N,  A10 A2 0 8 EXTUI,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$14 L:  END-CODE

CODE ic_set_bssid   \ .text.ic_set_bssid $4
  $0 1 LITERALS
    SYMBOL hal_mac_set_bssid
$4 L:
  A1 32 ENTRY,  A11 A3 MOV.N,  A10 A2 0 8 EXTUI,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$14 L:  END-CODE

CODE ic_set_current_channel   \ .text.ic_set_current_channel $4
  $0 1 LITERALS
    SYMBOL wDev_SetCurChannel
$4 L:
  A1 32 ENTRY,  A10 A2 MOV.N,  A2 $11 L BEQZ.N,
  A8 $0 L L32R,  A8 CALLX8,
$11 L:
  RETW.N,
$13 L:  END-CODE

CODE ic_get_random   \ .text.ic_get_random $4
  $0 1 LITERALS
    SYMBOL hal_random
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  RETW.N,
$11 L:  END-CODE

CODE ic_get_trc   \ .text.ic_get_trc $4
  $0 1 LITERALS
    SYMBOL rc_get_trc_by_index
$4 L:
  A1 32 ENTRY,  A11 A3 0 8 EXTUI,  A10 A2 0 8 EXTUI,  A8 $0 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  RETW.N,
$17 L:  END-CODE

CODE ic_tx_is_idle   \ .text.ic_tx_is_idle $4
  $0 1 LITERALS
    SYMBOL lmacIsIdle
$4 L:
  A1 32 ENTRY,  A10 A2 0 8 EXTUI,  A8 $0 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  RETW.N,
$14 L:  END-CODE

CODE ic_get_pp_hdl   \ .text.ic_get_pp_hdl $4
  $0 1 LITERALS
    SYMBOL ppGetTaskHdl
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  RETW.N,
$11 L:  END-CODE

CODE ic_set_rx_policy_ubssid_check   \ .text.ic_set_rx_policy_ubssid_check $4
  $0 1 LITERALS
    SYMBOL hal_mac_set_rxq_policy
$4 L:
  A1 32 ENTRY,  A10 A2 0 8 EXTUI,  A11 A3 0 8 EXTUI,  A2 0 MOVI.N,
  A10 4 $1A L BGEUI,
  A8 $0 L L32R,  A8 CALLX8,
  A2 1 MOVI.N,
$1A L:
  RETW.N,
$1C L:  END-CODE

CODE ic_set_rx_policy   \ .text.ic_set_rx_policy $4
  $0 1 LITERALS
    SYMBOL hal_mac_rx_set_policy
$4 L:
  A1 32 ENTRY,  A13 A5 0 8 EXTUI,  A12 A4 0 8 EXTUI,  A11 A3 0 8 EXTUI,
  A10 A2 0 8 EXTUI,  A8 $0 L L32R,  A8 CALLX8,
  A2 1 MOVI.N,  RETW.N,
$1D L:  END-CODE

CODE ic_set_sta_auth_flag   \ .text.ic_set_sta_auth_flag $4
  $0 1 LITERALS
    SYMBOL wDev_SetAuthed
$4 L:
  A1 32 ENTRY,  A10 A2 0 8 EXTUI,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$12 L:  END-CODE

CODE ic_set_interface   \ .text.ic_set_interface $4
  $0 1 LITERALS
    SYMBOL ppSetInterface
$4 L:
  A1 32 ENTRY,  A11 A3 MOV.N,  A10 A2 0 8 EXTUI,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$14 L:  END-CODE

CODE ic_trc_set_per_pkt_rate   \ .text.ic_trc_set_per_pkt_rate $4
  $0 1 LITERALS
    SYMBOL trc_set_per_pkt_rate
$4 L:
  A1 32 ENTRY,  A11 A3 MOV.N,  A10 A2 MOV.N,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$13 L:  END-CODE

CODE ic_trc_update_ifx_phy_mode   \ .text.ic_trc_update_ifx_phy_mode $4
  $0 1 LITERALS
    SYMBOL trc_update_ifx_phy_mode
$4 L:
  A1 32 ENTRY,  A11 A3 MOV.N,  A10 A2 MOV.N,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$13 L:  END-CODE

CODE ic_stop_hw_txq   \ .text.ic_stop_hw_txq $4
  $0 1 LITERALS
    SYMBOL lmac_stop_hw_txq
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  RETW.N,
$11 L:  END-CODE

CODE ic_stop_sw_txq   \ .text.ic_stop_sw_txq $C
  $0 3 LITERALS
    SYMBOL rodata_wlog_info.11  SYMBOL wifi_log  SYMBOL pp_stop_sw_txq
$C L:
  A1 32 ENTRY,  A13 $0 L L32R,  A12 3 MOVI.N,  A11 1 MOVI,  A10 6 MOVI,
  A8 $4 L L32R,  A8 CALLX8,
  A8 $8 L L32R,  A8 CALLX8,
  A2 0 MOVI.N,  RETW.N,
$2A L:  END-CODE

CODE ic_txq_empty   \ .text.ic_txq_empty $4
  $0 1 LITERALS
    SYMBOL ppTxqEmpty
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  RETW.N,
$11 L:  END-CODE

CODE ic_set_fix_rate   \ .text.ic_set_fix_rate $4
  $0 1 LITERALS
    SYMBOL rc_set_fix_rate
$4 L:
  A1 32 ENTRY,  A12 A4 MOV.N,  A11 A3 0 8 EXTUI,  A10 A2 MOV.N,  A8 $0 L L32R,
  A8 CALLX8,
  A2 A10 MOV.N,  RETW.N,
$18 L:  END-CODE

CODE ic_get_fix_rate   \ .text.ic_get_fix_rate $4
  $0 1 LITERALS
    SYMBOL rc_get_fix_rate
$4 L:
  A1 32 ENTRY,  A12 A4 MOV.N,  A11 A3 MOV.N,  A10 A2 A2 OR,  A8 $0 L L32R,
  A8 CALLX8,
  A2 A10 MOV.N,  RETW.N,
$18 L:  END-CODE

CODE ic_create_wifi_task   \ .text.ic_create_wifi_task $4
  $0 1 LITERALS
    SYMBOL pp_create_task
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  RETW.N,
$11 L:  END-CODE

CODE ic_delete_wifi_task   \ .text.ic_delete_wifi_task $4
  $0 1 LITERALS
    SYMBOL pp_delete_task
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  RETW.N,
$11 L:  END-CODE

CODE ic_set_csi   \ .text.ic_set_csi $4
  $0 1 LITERALS
    SYMBOL hal_mac_set_csi
$4 L:
  A1 32 ENTRY,  A10 A2 0 8 EXTUI,  A8 $0 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  RETW.N,
$14 L:  END-CODE

CODE ic_mac_init   \ .wifislprxiram.12 $4
  $0 1 LITERALS
    SYMBOL hal_mac_init
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A8 CALLX8,
  A2 0 MOVI.N,  RETW.N,
$11 L:  END-CODE

CODE ic_mac_deinit   \ .wifislprxiram.13 $4
  $0 1 LITERALS
    SYMBOL hal_mac_deinit
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A8 CALLX8,
  A2 0 MOVI.N,  RETW.N,
$11 L:  END-CODE

CODE ic_csi_set_config   \ .text.ic_csi_set_config $14
  $0 5 LITERALS
    $3FF730F8 T,  $3FF730FC T,  $DFFFFFFF T,  SYMBOL phy_chan_dump_cfg
    SYMBOL phy_chan_filt_set
$14 L:
  A1 32 ENTRY,  A14 A2 2 L8UI,  A13 A2 1 L8UI,  A12 A2 0 L8UI,  A11 A2 6 L8UI,
  A10 A2 5 L8UI,  A8 $C L L32R,  A8 CALLX8,
  A11 A2 3 L8UI,  A10 A2 4 L8UI,  A8 $10 L L32R,  A8 CALLX8,
  A8 A2 7 L8UI,  A2 $0 L L32R,  A8 $68 L BEQZ.N,
  A8 1 MOVI.N,  A8 A8 29 SLLI,  MEMW,  A9 A2 0 L32I.N,  A9 A9 A8 OR,  MEMW,
  A9 A2 0 S32I.N,  A9 $4 L L32R,  MEMW,  A2 A9 0 L32I.N,  A2 A2 A8 OR,  MEMW,
  A2 A9 0 S32I.N,  MEMW,  $88 L J,
$68 L:
  MEMW,  A8 A2 0 L32I.N,  A9 $8 L L32R,  A8 A8 A9 AND,  MEMW,  A8 A2 0 S32I.N,
  A8 $4 L L32R,  MEMW,  A2 A8 0 L32I.N,  A2 A2 A9 AND,  MEMW,  A2 A8 0 S32I.N,
$88 L:
  A2 0 MOVI.N,  MEMW,  RETW.N,
$8F L:  END-CODE

CODE ic_update_modem_sleep_defa~9511   \ ic_update_modem_sleep_default_params: .text.ic_update_modem_sleep_default_params $4
  $0 1 LITERALS
    SYMBOL pm_update_modem_sleep_defa~4980
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$F L:  END-CODE

CODE ic_update_light_sleep_defa~F6E8   \ ic_update_light_sleep_default_params: .text.ic_update_light_sleep_default_params $4
  $0 1 LITERALS
    SYMBOL pm_update_light_sleep_defa~2A79
$4 L:
  A1 32 ENTRY,  A11 A3 MOV.N,  A10 A2 MOV.N,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$13 L:  END-CODE

CODE ic_set_sleep_min_active_time   \ .text.ic_set_sleep_min_active_time $4
  $0 1 LITERALS
    SYMBOL pm_set_sleep_min_active_time
$4 L:
  A1 32 ENTRY,  A10 A2 A2 OR,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$12 L:  END-CODE

CODE ic_set_keep_alive_time   \ .text.ic_set_keep_alive_time $4
  $0 1 LITERALS
    SYMBOL pm_set_keep_alive_time
$4 L:
  A1 32 ENTRY,  A10 A2 A2 OR,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$12 L:  END-CODE

CODE ic_get_keep_alive_time   \ .text.ic_get_keep_alive_time $4
  $0 1 LITERALS
    SYMBOL pm_get_keep_alive_time
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  RETW.N,
$11 L:  END-CODE

CODE ic_set_sleep_wait_broadcas~AD7E   \ ic_set_sleep_wait_broadcast_data_time: .text.ic_set_sleep_wait_broadcast_data_time $4
  $0 1 LITERALS
    SYMBOL pm_set_sleep_wait_broadcas~0D70
$4 L:
  A1 32 ENTRY,  A10 A2 A2 OR,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$12 L:  END-CODE

CODE ic_beacon_monitor_configure   \ .text.ic_beacon_monitor_configure $4
  $0 1 LITERALS
    SYMBOL pm_beacon_monitor_configure
$4 L:
  A1 32 ENTRY,  A10 A2 A2 OR,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$12 L:  END-CODE

CODE ic_beacon_offset_configure   \ .text.ic_beacon_offset_configure $4
  $0 1 LITERALS
    SYMBOL ptr_beacon_offset_funcs
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A10 A2 MOV.N,  A8 A8 0 L32I.N,  A8 A8 20 L32I.N,
  A8 $15 L BEQZ.N,
  A8 CALLX8,
$15 L:
  RETW.N,
$17 L:  END-CODE

CODE ic_beacon_offset_set_rx_be~A378   \ ic_beacon_offset_set_rx_beacon_standard: .text.ic_beacon_offset_set_rx_beacon_standard $4
  $0 1 LITERALS
    SYMBOL ptr_beacon_offset_funcs
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A10 A2 0 8 EXTUI,  A8 A8 0 L32I.N,  A8 A8 28 L32I.N,
  A8 $16 L BEQZ.N,
  A8 CALLX8,
$16 L:
  RETW.N,
$18 L:  END-CODE

CODE ic_update_listen_interval   \ .text.ic_update_listen_interval $C
  $0 3 LITERALS
    SYMBOL wifi_nvs_set_sta_listen_in~6741  SYMBOL pm_is_open
    SYMBOL pm_scale_listen_interval
$C L:
  A1 32 ENTRY,  A10 A2 0 16 EXTUI,  A8 $0 L L32R,  A8 CALLX8,
  A8 $4 L L32R,  A8 CALLX8,
  A10 $26 L BEQZ.N,
  A8 $8 L L32R,  A8 CALLX8,
$26 L:
  RETW.N,
$28 L:  END-CODE

CODE ic_rx_disable_bssid_check   \ .text.ic_rx_disable_bssid_check $4
  $0 1 LITERALS
    SYMBOL hal_rx_disable_bssid_check
$4 L:
  A1 32 ENTRY,  A10 A2 0 8 EXTUI,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$12 L:  END-CODE

CODE ic_rx_enable_bssid_check   \ .text.ic_rx_enable_bssid_check $4
  $0 1 LITERALS
    SYMBOL hal_rx_enable_bssid_check
$4 L:
  A1 32 ENTRY,  A10 A2 0 8 EXTUI,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$12 L:  END-CODE

CODE ic_set_bar_rate   \ .text.ic_set_bar_rate $4
  $0 1 LITERALS
    SYMBOL ppSetBarRate
$4 L:
  A1 32 ENTRY,  A10 A2 0 8 EXTUI,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$12 L:  END-CODE

CODE ic_trc_update_def_rate   \ .text.ic_trc_update_def_rate $4
  $0 1 LITERALS
    SYMBOL trc_update_def_rate
$4 L:
  A1 32 ENTRY,  A12 A4 0 8 EXTUI,  A11 A3 MOV.N,  A10 A2 0 8 EXTUI,  A8 $0 L L32R,
  A8 CALLX8,
  RETW.N,
$17 L:  END-CODE

CODE ic_get_espnow_rate   \ .text.ic_get_espnow_rate $4
  $0 1 LITERALS
    SYMBOL trc_get_espnow_rate
$4 L:
  A1 32 ENTRY,  A10 A2 0 8 EXTUI,  A8 $0 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  RETW.N,
$14 L:  END-CODE

CODE ic_get_80211_tx_rate   \ .text.ic_get_80211_tx_rate $4
  $0 1 LITERALS
    SYMBOL trc_get_80211_tx_rate
$4 L:
  A1 32 ENTRY,  A10 A2 0 8 EXTUI,  A8 $0 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  RETW.N,
$14 L:  END-CODE

CODE ic_get_80211_tx_rate_config   \ .text.ic_get_80211_tx_rate_config $4
  $0 1 LITERALS
    SYMBOL trc_get_80211_tx_rate_config
$4 L:
  A1 32 ENTRY,  A11 A3 MOV.N,  A10 A2 0 8 EXTUI,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$14 L:  END-CODE

CODE ic_set_espnow_rate   \ .text.ic_set_espnow_rate $4
  $0 1 LITERALS
    SYMBOL trc_set_espnow_rate
$4 L:
  A1 32 ENTRY,  A11 A3 MOV.N,  A10 A2 0 8 EXTUI,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$14 L:  END-CODE

CODE ic_set_80211_tx_rate   \ .text.ic_set_80211_tx_rate $4
  $0 1 LITERALS
    SYMBOL trc_set_80211_tx_rate
$4 L:
  A1 32 ENTRY,  A11 A3 MOV.N,  A10 A2 0 8 EXTUI,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$14 L:  END-CODE

CODE ic_set_80211_tx_rate_config   \ .text.ic_set_80211_tx_rate_config $4
  $0 1 LITERALS
    SYMBOL trc_set_80211_tx_rate_config
$4 L:
  A1 32 ENTRY,  A11 A3 MOV.N,  A10 A2 0 8 EXTUI,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$14 L:  END-CODE

CODE ic_get_default_sched   \ .text.ic_get_default_sched $4
  $0 1 LITERALS
    SYMBOL rc_get_default_sched
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  RETW.N,
$11 L:  END-CODE

CODE ic_get_G6M_sched   \ .text.ic_get_G6M_sched $4
  $0 1 LITERALS
    SYMBOL rc_get_G6M_sched
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  RETW.N,
$11 L:  END-CODE

LINK-SYMBOLS   \ the cells of other packages that wait for these names
END-PACKAGE  PREVIOUS   \ not in the search order: ALSO name gives its words (README.md)
