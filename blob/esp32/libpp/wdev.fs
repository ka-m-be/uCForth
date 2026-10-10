\ wdev.fs - package PP-WDEV: wdev.o of libpp.a (ESP32)
\
\ Made from wdev.S by s2fs.py, not by hand. Each function of C is a
\ CODE word: its bytes are those of the compiler, at the same offsets of
\ its section (L gives the address of an offset); its literals are at the
\ start of the word, after a jump. The data come first, then the
\ functions, each after those that it calls. The global symbols are
\ PUBLIC, the static ones PRIVATE. lib/esp32/clink.fs explains the words.
\
\ 65 functions (65 global), 43 data objects.
\ A function runs as C code with the windowed calls of the Xtensa: call
\ it with CALL-C ( x1 .. xn n xt -- x ), never by its name alone.
\
\ Names of other packages: a literal waits until its package comes
\ (LINK-SYMBOLS); a name outside the libraries until EXTERN gives it.
\ Uses:
\   PHY-PHY-CHIP-V7  bb_wdt_get_status bb_wdt_timeout_clear
\                    read_hw_noisefloor
\   PP-ESF-BUF  esf_buf_alloc esf_buf_recycle
\   PP-HAL-AMPDU  hal_ba_session_restore hal_ba_session_store
\   PP-HAL-CRYPTO  hal_crypto_clr_key_entry hal_crypto_disable
\                  hal_crypto_enable hal_crypto_mgmt_rx_enabled
\                  hal_crypto_set_key_entry
\   PP-HAL-MAC  hal_disable_mac hal_enable_mac hal_get_tsf_time
\               hal_mac_disable_low_rate hal_mac_enable_low_rate
\               hal_mac_interrupt_clr_event
\               hal_mac_interrupt_clr_watchdog
\               hal_mac_interrupt_get_event hal_mac_is_low_rate_enabled
\               hal_mac_rx_read_rxdscrnext hal_mac_rx_set_base
\               hal_mac_tsf_reset hal_mac_tsf_set_time
\   PP-HAL-MAC-RX  hal_mac_rx_get_end_info hal_mac_rx_get_end_state
\                  hal_mac_rx_get_last_dscr hal_mac_rx_is_dscr_reload
\                  hal_mac_rx_set_dscr_reload
\   PP-HAL-SNIFFER  hal_sniffer_disable hal_sniffer_enable
\                   hal_sniffer_rx_clr_statistics
\                   hal_sniffer_rx_set_promis
\   PP-IF-HWCTRL  ic_interface_enabled
\   PP-LMAC  lmacPostTxComplete lmacProcessAllTxTimeout
\            lmacProcessCollisions lmacProcessRxSucData lmacRxDone
\   PP-PP  pTxRx pp_post
\   PP-PP-DEBUG  g_lmac_cnt wifi_gpio_debug
\ Outside the libraries (../README.md):
\   NAN_NETWORK_ID NAN_STANDARD_BSSID g_intr_lock_mux g_osi_funcs_p
\   g_wifi_menuconfig memcmp memcpy memset roundup2 wifi_log
\   wifi_sta_rx_probe_req
\ C names with more than 31 characters (Forth takes 31): the first 26,
\ ~ and 4 hex digits of their CRC-32 (../README.md):
\   g_wdev_is_nan_pkt_in_valid~1547 = g_wdev_is_nan_pkt_in_valid_slot_cb
\   wDev_ProcessRxData_NAN_Int~2C06 = wDev_ProcessRxData_NAN_Interface_Hook
\
\ Needs XTENSA (asm/xtensa.fs), ASM (lib/esp32/asm.fs) and CLINK
\ (lib/esp32/clink.fs) of ucf; the ESP32.

PACKAGE PP-WDEV  ALSO ASM  ALSO CLINK   \ lib/package.fs
EXPORTS   \ the PUBLIC words are symbols for the other packages

\ ==== Data =============================================================

PRIVATE
CREATE rodata.str1.1   \ .rodata.str1.1, 7 bytes
  ," wdev.c"  0 C,
CREATE rodata.__FUNCTION__$0   \ .rodata.__FUNCTION__$0, 20 bytes
  ," wDevProcessDirtyBuf"  0 C,
CREATE rodata.__FUNCTION__$1   \ .rodata.__FUNCTION__$1, 20 bytes
  ," wDev_AppendRxBlocks"  0 C,
CREATE rodata.__FUNCTION__$2   \ .rodata.__FUNCTION__$2, 24 bytes
  ," wdevProcessRxSucDataAll"  0 C,
CREATE rodata.__FUNCTION__$3   \ .rodata.__FUNCTION__$3, 22 bytes
  ," wDev_ProcessRxSucData"  0 C,
CREATE rodata_wlog_error.55   \ .rodata_wlog_error.55, 10 bytes
  ," %s %s %u"  10 C,  0 C,
CREATE rodata_wlog_error.54   \ .rodata_wlog_error.54, 34 bytes
  ," dirty buf %08x: %08x, %08x: %08x"  10 C,  0 C,
CREATE rodata_wlog_error.53   \ .rodata_wlog_error.53, 10 bytes
  ," %s %s %u"  10 C,  0 C,
CREATE rodata_wlog_info.51   \ .rodata_wlog_info.51, 21 bytes
  ," (%x, %x, %x, %x, %x)"  0 C,
CREATE rodata_wlog_info.50   \ .rodata_wlog_info.50, 24 bytes
  ," %u (%x, %x, %x, %x, %x)"  0 C,
CREATE rodata_wlog_info.49   \ .rodata_wlog_info.49, 15 bytes
  ," reload(%x, %x)"  0 C,
PUBLIC
132 DATA: s_record_list   \ .dram1.46
PRIVATE
1 DATA: dram1.43   \ .dram1.43
  $01  1 dram1.43 INIT
28 DATA: dram1.42   \ .dram1.42
8 DATA: dram1.41   \ .dram1.41
16 DATA: dram1.40   \ .dram1.40
CREATE rodata_wlog_info.39   \ .rodata_wlog_info.39, 27 bytes
  ," Deinit lldesc rx mblock:%d"  0 C,
CREATE rodata_wlog_error.38   \ .rodata_wlog_error.38, 44 bytes
  ," Expected to init %d rx buffer, actual is %d"  0 C,
CREATE rodata_wlog_info.37   \ .rodata_wlog_info.37, 31 bytes
  ," Init dynamic rx buffer num: %d"  0 C,
CREATE rodata_wlog_info.36   \ .rodata_wlog_info.36, 30 bytes
  ," Init static rx buffer num: %d"  0 C,
CREATE rodata_wlog_info.35   \ .rodata_wlog_info.35, 31 bytes
  ," Init static rx buffer size: %d"  0 C,
CREATE rodata_wlog_error.34   \ .rodata_wlog_error.34, 47 bytes
  ," Failed to init rx desc space, buffer number=%d"  0 C,
CREATE rodata_wlog_warning.33   \ .rodata_wlog_warning.33, 19 bytes
  ," malloc buffer fail"  0 C,
CREATE rodata_wlog_error.31   \ .rodata_wlog_error.31, 10 bytes
  ," %s %s %u"  10 C,  0 C,
CREATE rodata_wlog_error.30   \ .rodata_wlog_error.30, 10 bytes
  ," %s %s %u"  10 C,  0 C,
CREATE rodata_wlog_error.28   \ .rodata_wlog_error.28, 10 bytes
  ," %s %s %u"  10 C,  0 C,
CREATE rodata_wlog_warning.22   \ .rodata_wlog_warning.22, 21 bytes
  ," rx control blocks=%d"  0 C,
CREATE rodata_wlog_info.20   \ .rodata_wlog_info.20, 19 bytes
  ," ic_disable_sniffer"  0 C,
CREATE rodata_wlog_info.19   \ .rodata_wlog_info.19, 18 bytes
  ," ic_enable_sniffer"  0 C,
CREATE rodata_wlog_error.18   \ .rodata_wlog_error.18, 26 bytes
  ," promis buf: out of memory"  0 C,
4 DATA: dram1.16   \ .dram1.16
CREATE rodata_wlog_warning.15   \ .rodata_wlog_warning.15, 17 bytes
  ," unknown csi bug!"  0 C,
CREATE rodata_wlog_info.10   \ .rodata_wlog_info.10, 48 bytes
  ," Allocate MAC register memory base addr(0x%08x)."  0 C,
CREATE rodata_wlog_warning.9   \ .rodata_wlog_warning.9, 34 bytes
  ," Allocate MAC register memory fail"  0 C,
PUBLIC
4 DATA: BcnInterval   \ .data.BcnInterval
  $00  $90  $01  $00  4 BcnInterval INIT
4 DATA: BcnSendTick   \ .bss.BcnSendTick
PRIVATE
4 DATA: dram1.2   \ .dram1.2
PUBLIC
112 DATA: wDevMacSleep   \ .bss.wDevMacSleep
56 DATA: wDevCtrl   \ .data.wDevCtrl
  $00  $00  $00  $00  $00  $00  $00  $00  $00  $00  $00  $00  $00  $00  $60  $17  16 wDevCtrl $20 + INIT
  $00  $00  $00  $00  $05  $00  $00  $00  8 wDevCtrl $30 + INIT
4 DATA: g_wdev_csi_rx_ctx   \ .bss.g_wdev_csi_rx_ctx
4 DATA: g_wdev_csi_rx   \ .bss.g_wdev_csi_rx
4 DATA: g_wdev_is_nan_pkt_in_valid~1547   \ .bss.g_wdev_is_nan_pkt_in_valid_slot_cb
1 DATA: g_wdev_last_desc_reset   \ .bss.g_wdev_last_desc_reset

\ ==== Code =============================================================

CODE wDev_SetCurChannel   \ .text.wDev_SetCurChannel $4
  $0 1 LITERALS
    SYMBOL wDevCtrl
$4 L:
  A1 32 ENTRY,  A9 A2 0 L8UI,  A8 $0 L L32R,  A9 A8 44 S8I,  MEMW,  A2 A2 1 L8UI,
  A2 A8 45 S8I,  MEMW,  MEMW,  RETW.N,
$21 L:  END-CODE

CODE wDev_SetAuthed   \ .text.wDev_SetAuthed $0
  $0 ORIGIN
  A1 32 ENTRY,  RETW.N,
$5 L:  END-CODE

CODE wDev_Set_Beacon_Int   \ .text.wDev_Set_Beacon_Int $4
  $0 1 LITERALS
    SYMBOL BcnInterval
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A8 0 S32I.N,  MEMW,  RETW.N,
$11 L:  END-CODE

CODE wDev_Reset_TBTT   \ .text.wDev_Reset_TBTT $4
  $0 1 LITERALS
    SYMBOL hal_mac_tsf_reset
$4 L:
  A1 32 ENTRY,  A10 0 MOVI,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$12 L:  END-CODE

CODE wDev_reset_bcnSendTick   \ .text.wDev_reset_bcnSendTick $4
  $0 1 LITERALS
    SYMBOL BcnSendTick
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A9 0 MOVI.N,  A9 A8 0 S32I.N,  MEMW,  RETW.N,
$13 L:  END-CODE

CODE wDev_Mesh_Enable_Tsf   \ .text.wDev_Mesh_Enable_Tsf $4
  $0 1 LITERALS
    SYMBOL hal_mac_tsf_reset
$4 L:
  A1 32 ENTRY,  A10 1 MOVI,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$12 L:  END-CODE

CODE wDev_Mesh_Set_TBTT   \ .text.wDev_Mesh_Set_TBTT $4
  $0 1 LITERALS
    SYMBOL hal_mac_tsf_set_time
$4 L:
  A1 32 ENTRY,  A12 A2 MOV.N,  A13 A3 MOV.N,  A10 1 MOVI,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$16 L:  END-CODE

CODE wDev_Get_Next_TBTT   \ .text.wDev_Get_Next_TBTT $10
  $0 4 LITERALS
    SYMBOL BcnSendTick  SYMBOL BcnInterval  $10624DD3 T,  SYMBOL hal_get_tsf_time
$10 L:
  A1 32 ENTRY,  A4 $0 L L32R,
$16 L:
  A10 1 MOVI.N,  A3 $4 L L32R,  A2 A4 0 L32I.N,  A8 A3 0 L32I.N,  A2 A2 A8 ADD.N,
  A2 A4 0 S32I.N,  MEMW,  A8 $C L L32R,  A8 CALLX8,
  A10 A2 A10 SUB,  A2 A3 0 L32I.N,  A2 A10 $16 L BLTU,
  A2 $8 L L32R,  A2 A10 A2 MULUH,  A2 A2 6 SRLI,  A2 A2 1 ADDI.N,  RETW.N,
$41 L:  END-CODE

CODE wdev_bank_store   \ .wifi0iram.3 $0
  $0 ORIGIN
  A1 32 ENTRY,  A8 A3 MOV.N,  $16 L J,
$8 L:
  MEMW,  A10 A8 0 L32I.N,  A9 A8 A3 SUB,  A9 A2 A9 ADD.N,  A8 A8 4 ADDI.N,
  A10 A9 0 S32I.N,
$16 L:
  A4 A4 -1 ADDI.N,  A4 -1 $8 L BNEI,
  MEMW,  RETW.N,
$20 L:  END-CODE

CODE wdev_bank_load   \ .wifi0iram.4 $0
  $0 ORIGIN
  A1 32 ENTRY,  A2 A2 A3 SUB,  $12 L J,
$9 L:
  A9 A3 0 L32I.N,  A3 A3 4 ADDI.N,  MEMW,  A9 A8 0 S32I.N,
$12 L:
  A4 A4 -1 ADDI.N,  A8 A3 A2 ADD.N,  A4 -1 $9 L BNEI,
  MEMW,  RETW.N,
$1E L:  END-CODE

CODE wdev_mac_reg_load   \ .wifi0iram.5 $30
  $0 12 LITERALS
    $3FF73000 T,  $3FF73400 T,  $3FF73800 T,  $3FF73C00 T,  $3FF74000 T,
    $3FF74400 T,  SYMBOL wdev_bank_load  SYMBOL wdev_bank_load
    SYMBOL wdev_bank_load  SYMBOL wdev_bank_load  SYMBOL wdev_bank_load
    SYMBOL wdev_bank_load
$30 L:
  A1 32 ENTRY,  A10 $0 L L32R,  A11 A2 MOV.N,  A12 188 MOVI,  A8 $18 L L32R,
  A8 CALLX8,
  A11 $2F0 MOVI,  A10 $4 L L32R,  A11 A2 A11 ADD.N,  A12 16 MOVI.N,  A8 $1C L L32R,
  A8 CALLX8,
  A11 $330 MOVI,  A10 $8 L L32R,  A11 A2 A11 ADD.N,  A12 17 MOVI.N,  A8 $20 L L32R,
  A8 CALLX8,
  A11 $374 MOVI,  A10 $C L L32R,  A11 A2 A11 ADD.N,  A12 120 MOVI,  A8 $24 L L32R,
  A8 CALLX8,
  A11 $554 MOVI,  A10 $10 L L32R,  A11 A2 A11 ADD,  A12 210 MOVI,  A8 $28 L L32R,
  A8 CALLX8,
  A11 $227 MOVI,  A10 $14 L L32R,  A12 250 MOVI,  A11 A11 A2 ADDX4,  A8 $2C L L32R,
  A8 CALLX8,
  RETW.N,
$98 L:  END-CODE

CODE wdev_mac_reg_store   \ .wifi0iram.6 $30
  $0 12 LITERALS
    $3FF73000 T,  $3FF73400 T,  $3FF73800 T,  $3FF73C00 T,  $3FF74000 T,
    $3FF74400 T,  SYMBOL wdev_bank_store  SYMBOL wdev_bank_store
    SYMBOL wdev_bank_store  SYMBOL wdev_bank_store  SYMBOL wdev_bank_store
    SYMBOL wdev_bank_store
$30 L:
  A1 32 ENTRY,  A11 $0 L L32R,  A10 A2 MOV.N,  A12 188 MOVI,  A8 $18 L L32R,
  A8 CALLX8,
  A10 $2F0 MOVI,  A11 $4 L L32R,  A10 A2 A10 ADD.N,  A12 16 MOVI.N,  A8 $1C L L32R,
  A8 CALLX8,
  A10 $330 MOVI,  A11 $8 L L32R,  A10 A2 A10 ADD.N,  A12 17 MOVI.N,  A8 $20 L L32R,
  A8 CALLX8,
  A10 $374 MOVI,  A11 $C L L32R,  A10 A2 A10 ADD.N,  A12 120 MOVI,  A8 $24 L L32R,
  A8 CALLX8,
  A10 $554 MOVI,  A11 $10 L L32R,  A10 A2 A10 ADD,  A12 210 MOVI,  A8 $28 L L32R,
  A8 CALLX8,
  A10 $227 MOVI,  A11 $14 L L32R,  A12 250 MOVI,  A10 A10 A2 ADDX4,  A8 $2C L L32R,
  A8 CALLX8,
  RETW.N,
$98 L:  END-CODE

CODE wdev_mac_special_reg_load   \ .wifi0iram.7 $10
  $0 4 LITERALS
    $50 SYMBOL+ wDevMacSleep  $30 SYMBOL+ wDevMacSleep  $10 SYMBOL+ wDevMacSleep
    SYMBOL hal_ba_session_restore
$10 L:
  A1 32 ENTRY,  A12 $0 L L32R,  A11 $4 L L32R,  A10 $8 L L32R,  A8 $C L L32R,
  A8 CALLX8,
  RETW.N,
$24 L:  END-CODE

CODE wdev_mac_special_reg_store   \ .wifi0iram.8 $10
  $0 4 LITERALS
    $50 SYMBOL+ wDevMacSleep  $30 SYMBOL+ wDevMacSleep  $10 SYMBOL+ wDevMacSleep
    SYMBOL hal_ba_session_store
$10 L:
  A1 32 ENTRY,  A12 $0 L L32R,  A11 $4 L L32R,  A10 $8 L L32R,  A8 $C L L32R,
  A8 CALLX8,
  RETW.N,
$24 L:  END-CODE

CODE wdev_mac_sleep_init   \ .text.wdev_mac_sleep_init $18
  $0 6 LITERALS
    SYMBOL wDevMacSleep  SYMBOL g_osi_funcs_p  SYMBOL rodata_wlog_warning.9
    SYMBOL rodata_wlog_info.10  SYMBOL wifi_log  SYMBOL wifi_log
$18 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A8 A2 4 L32I.N,  A8 $60 L BNEZ.N,
  A10 $321 MOVI,  A10 A10 2 SLLI,  A8 $4 L L32R,  A8 A8 0 L32I,  A8 A8 $15C L32I,
  A8 CALLX8,
  A11 1 MOVI.N,  A11 A11 12 SLLI,  A10 A2 4 S32I.N,  A10 $51 L BNEZ.N,
  A12 2 MOVI.N,  A10 6 MOVI.N,  A13 $8 L L32R,  MEMW,  A8 $10 L L32R,  A8 CALLX8,
  $60 L J,
  1 ZEROS,
$51 L:
  A13 $C L L32R,  A14 A10 MOV.N,  A12 3 MOVI.N,  A10 6 MOVI.N,  A8 $14 L L32R,
  A8 CALLX8,
$60 L:
  RETW.N,
$62 L:  END-CODE

CODE wdev_mac_sleep_deinit   \ .text.wdev_mac_sleep_deinit $8
  $0 2 LITERALS
    SYMBOL wDevMacSleep  SYMBOL g_osi_funcs_p
$8 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A10 A2 4 L32I.N,  A10 $21 L BEQZ.N,
  A8 $4 L L32R,  A8 A8 0 L32I.N,  A8 A8 176 L32I,  A8 CALLX8,
  A8 0 MOVI.N,  A8 A2 4 S32I.N,
$21 L:
  MEMW,  RETW.N,
$26 L:  END-CODE

CODE wdev_mac_wakeup   \ .wifi0iram.11 $20
  $0 8 LITERALS
    SYMBOL wDevMacSleep  SYMBOL g_osi_funcs_p  $3FF73C04 T,  $3FF73084 T,
    $7FFFFFFF T,  SYMBOL wdev_mac_reg_load  SYMBOL hal_enable_mac
    SYMBOL wdev_mac_special_reg_load
$20 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A3 A2 0 L8UI,  A3 $39 L BEQZ.N,
  A3 0 MOVI.N,  A3 A2 0 S8I,  MEMW,  MEMW,  $AC L J,
$39 L:
  A8 A2 1 L8UI,  A8 $AC L BEQZ,
  A3 A2 1 S8I,  MEMW,  A3 A2 4 L32I.N,  A3 $AC L BEQZ,
  A3 $4 L L32R,  A3 A3 0 L32I.N,  A3 A3 $10C L32I,  MEMW,  A3 CALLX8,
  A9 240 MOVI,  A8 $8 L L32R,  MEMW,  A3 A8 0 L32I.N,  A3 A3 A9 OR,  MEMW,
  A3 A8 0 S32I.N,  A3 $C L L32R,  A9 $10 L L32R,  MEMW,  A8 A3 0 L32I.N,
  A8 A8 A9 AND,  A10 A2 4 L32I.N,  MEMW,  A8 A3 0 S32I,  MEMW,  A8 $14 L L32R,
  A8 CALLX8,
  A11 A2 12 L32I.N,  A10 A2 8 L32I.N,  A8 $18 L L32R,  A8 CALLX8,
  A8 $1C L L32R,  A8 CALLX8,
  A8 -1 MOVI.N,  MEMW,  A2 A3 0 L32I.N,  A8 A8 31 SLLI,  A2 A2 A8 OR,  MEMW,
  A2 A3 0 S32I.N,
$AC L:
  MEMW,  RETW.N,
$B1 L:  END-CODE

CODE wdev_is_data_in_rxlist   \ .wifi0iram.12 $8
  $0 2 LITERALS
    SYMBOL wDevCtrl  SYMBOL hal_mac_rx_get_last_dscr
$8 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A3 A2 0 L32I.N,  A8 $4 L L32R,  A8 CALLX8,
  $2B L J,
$19 L:
  MEMW,  A2 A3 0 L32I.N,  A2 A2 30 1 EXTUI,  A8 A3 8 L32I.N,  A2 1 $30 L BEQI,
  A3 A10 $30 L BEQ,
  A3 A8 MOV.N,
$2B L:
  A3 $19 L BNEZ,
  A2 A3 MOV.N,
$30 L:
  RETW.N,
$32 L:  END-CODE

CODE wdev_mac_sleep   \ .wifi0iram.13 $24
  $0 9 LITERALS
    SYMBOL wDevMacSleep  $3FF73084 T,  $7FFFFFFF T,  SYMBOL wDevCtrl
    SYMBOL g_osi_funcs_p  SYMBOL wdev_is_data_in_rxlist
    SYMBOL wdev_mac_special_reg_store  SYMBOL hal_disable_mac
    SYMBOL wdev_mac_reg_store
$24 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A3 A2 4 L32I.N,  A3 $A5 L BEQZ,
  A3 $4 L L32R,  A9 $8 L L32R,  MEMW,  A8 A3 0 L32I.N,  A8 A8 A9 AND,  MEMW,
  A8 A3 0 S32I,  MEMW,  A8 $14 L L32R,  A8 CALLX8,
  A10 A2 0 S8I,  MEMW,  A10 $6C L BEQZ.N,
  A8 -1 MOVI.N,  A8 A8 31 SLLI,  MEMW,  A2 A3 0 L32I.N,  A2 A2 A8 OR,  MEMW,
  A2 A3 0 S32I.N,  MEMW,  $A5 L J,
$6C L:
  A3 1 MOVI.N,  A3 A2 1 S8I,  MEMW,  MEMW,  A8 $18 L L32R,  A8 CALLX8,
  A8 $1C L L32R,  A8 CALLX8,
  A10 A2 8 S32I.N,  A10 A2 4 L32I.N,  MEMW,  A8 $20 L L32R,  A8 CALLX8,
  A3 $C L L32R,  A3 A3 0 L32I.N,  A3 A2 12 S32I.N,  A2 $10 L L32R,  A2 A2 0 L32I.N,
  A2 A2 $108 L32I,  MEMW,  A2 CALLX8,
$A5 L:
  RETW.N,
$A7 L:  END-CODE

CODE wdev_csi_len_align   \ .wifislpiram.14 $0
  $0 ORIGIN
  A1 32 ENTRY,  A8 A2 20 L32I.N,  A2 $3FF MOVI,  A2 A2 8 SLLI,  A2 A8 A2 AND,
  A2 $1C L BEQZ.N,
  A2 A8 8 10 EXTUI,  A2 A2 3 ADDI,  A2 A2 2 SRAI,  A2 A2 2 SLLI,
$1C L:
  RETW.N,
$1E L:  END-CODE

CODE wdev_csi_hw_bug_check   \ .text.wdev_csi_hw_bug_check $C
  $0 3 LITERALS
    SYMBOL rodata_wlog_warning.15  SYMBOL wDevCtrl  SYMBOL wifi_log
$C L:
  A1 32 ENTRY,  A9 A2 0 L32I.N,  A8 A9 0 12 EXTUI,  A9 A9 12 12 EXTUI,
  A2 A3 0 L32I.N,  A10 A2 0 12 EXTUI,  A8 A9 $21 L BNE,
  A8 $29 L BNEZ.N,
$21 L:
  A9 A9 8 ADDI.N,  A9 A8 $38 L BNE,
  $6B L J,
$29 L:
  A8 A8 A10 SUB,  A9 1 MOVI.N,  A2 2 MOVI.N,  A2 A9 A8 MOVEQZ,  $75 L J,
  2 ZEROS,
$38 L:
  A8 A2 12 12 EXTUI,  A2 0 MOVI.N,  A10 A8 $75 L BEQ,
  A8 A8 8 ADDI.N,  A8 A10 $75 L BEQ,
  A13 $0 L L32R,  A12 2 MOVI.N,  A11 32 MOVI.N,  A10 6 MOVI.N,  A8 $8 L L32R,
  A8 CALLX8,
  A2 $4 L L32R,  A2 A2 0 L32I.N,  $5E L J,
$5C L:
  A2 A2 8 L32I.N,
$5E L:
  MEMW,  A8 A2 0 L32I.N,  A8 30 $5C L BBCI,
  A2 3 MOVI.N,  $75 L J,
$6B L:
  A8 A8 A10 SUB,  A2 1 MOVI.N,  A9 2 MOVI.N,  A2 A9 A8 MOVNEZ,
$75 L:
  RETW.N,
$77 L:  END-CODE

CODE wdev_set_promis_misc_pkt   \ .text.wdev_set_promis_misc_pkt $14
  $0 5 LITERALS
    SYMBOL wDevCtrl  $3FF730F4 T,  $FFFF00FF T,  SYMBOL hal_sniffer_rx_set_promis
    SYMBOL hal_sniffer_rx_set_promis
$14 L:
  A1 32 ENTRY,  A2 A2 0 8 EXTUI,  A3 $0 L L32R,  A2 $5C L BEQZ.N,
  A8 A3 48 L8UI,  A8 $5C L BEQZ.N,
  A8 A3 52 L32I.N,  A8 3 $5C L BBCI,
  A12 0 MOVI.N,  A11 1 MOVI.N,  A10 A12 MOV.N,  A8 $C L L32R,  A8 CALLX8,
  A3 A3 52 L32I.N,  A3 6 $80 L BBCI,
  A9 $4 L L32R,  A3 $8 L L32R,  MEMW,  A8 A9 0 L32I.N,  A8 A8 A3 AND,  A3 65 MOVI.N,
  A3 A3 8 SLLI,  A8 A8 A3 OR,  MEMW,  A8 A9 0 S32I.N,  MEMW,  $80 L J,
  1 ZEROS,
$5C L:
  A12 0 MOVI.N,  A11 A12 MOV.N,  A10 A12 MOV.N,  A8 $10 L L32R,  A8 CALLX8,
  A3 A3 52 L32I.N,  A3 6 $80 L BBCI,
  A8 $4 L L32R,  A9 $8 L L32R,  MEMW,  A3 A8 0 L32I.N,  A3 A3 A9 AND,  MEMW,
  A3 A8 0 S32I.N,
$80 L:
  A2 0 MOVI.N,  MEMW,  RETW.N,
$87 L:  END-CODE

CODE wdev_push_promis_misc_buf   \ .iram1.17 $8
  $0 2 LITERALS
    SYMBOL dram1.16  SYMBOL memcpy
$8 L:
  A1 32 ENTRY,  A3 $0 L L32R,  A11 A2 MOV.N,  A2 -1 MOVI.N,  A10 A3 0 L32I.N,
  A10 $64 L BEQZ,
  A13 9 MOVI.N,  A8 A10 0 L8UI,  A3 A8 A8 ADDX8,  A3 A3 A8 SUBX8,  A9 A3 2 SLLI,
  A3 A3 A10 ADDX4,  A12 A3 6 L8UI,  A13 A12 $64 L BLTU,
  A9 A9 8 ADDI.N,  A12 28 MOVI.N,  A8 A3 4 L8UI,  A8 A8 A8 SUBX8,  A8 A8 A9 ADDX4,
  A10 A10 A8 ADD,  A8 $4 L L32R,  A8 CALLX8,
  A8 A3 4 L8UI,  A8 A8 1 ADDI.N,  A8 A8 0 8 EXTUI,  A8 10 $51 L BNEI,
  A8 0 MOVI.N,
$51 L:
  A8 A3 4 S8I,  MEMW,  A2 0 MOVI.N,  A8 A3 6 L8UI,  A8 A8 1 ADDI.N,  A8 A3 6 S8I,
  MEMW,
$64 L:
  MEMW,  RETW.N,
$69 L:  END-CODE

CODE wdev_pop_promis_misc_buf   \ .text.wdev_pop_promis_misc_buf $8
  $0 2 LITERALS
    SYMBOL dram1.16  SYMBOL memcpy
$8 L:
  A1 32 ENTRY,  A3 $0 L L32R,  A10 A2 MOV.N,  A2 -1 MOVI.N,  A11 A3 0 L32I.N,
  A11 $68 L BEQZ,
  A8 1 MOVI.N,  A3 A11 0 L8UI,  A8 A8 A3 SUB,  A3 A8 A8 ADDX8,  A3 A3 A8 SUBX8,
  A9 A3 2 SLLI,  A3 A3 A11 ADDX4,  A12 A3 6 L8UI,  A12 $68 L BEQZ.N,
  A9 A9 8 ADDI.N,  A12 28 MOVI.N,  A8 A3 5 L8UI,  A8 A8 A8 SUBX8,  A8 A8 A9 ADDX4,
  A11 A11 A8 ADD.N,  A8 $4 L L32R,  A8 CALLX8,
  A8 A3 5 L8UI,  A8 A8 1 ADDI,  A8 A8 0 8 EXTUI,  A8 10 $54 L BNEI,
  A8 0 MOVI,
$54 L:
  A8 A3 5 S8I,  MEMW,  A2 0 MOVI.N,  A8 A3 6 L8UI,  A8 A8 -1 ADDI,  A8 A3 6 S8I,
  MEMW,
$68 L:
  MEMW,  RETW.N,
$6D L:  END-CODE

CODE wdev_process_misc_pkt   \ .text.wdev_process_misc_pkt $14
  $0 5 LITERALS
    SYMBOL dram1.16  SYMBOL g_osi_funcs_p  SYMBOL memcpy  SYMBOL pp_post
    SYMBOL wdev_pop_promis_misc_buf
$14 L:
  A1 64 ENTRY,  A2 $0 L L32R,  A2 A2 0 L32I.N,  A2 $98 L BEQZ,
  A3 1 MOVI.N,  A4 A2 0 L8UI,  A3 A3 A4 SUB,  A3 A2 0 S8I,  MEMW,  A3 $4 L L32R,
  MEMW,  $8D L J,
$36 L:
  A2 A3 0 L32I.N,  A10 8 MOVI,  A2 A2 $16C L32I,  A2 CALLX8,
  A2 A10 MOV.N,  A10 $8D L BEQZ,
  A4 A3 0 L32I.N,  A10 28 MOVI.N,  A4 A4 $16C L32I,  A4 CALLX8,
  A4 A10 MOV.N,  A10 $58 L BNEZ.N,
  $82 L J,
  1 ZEROS,
$58 L:
  A11 A1 MOV.N,  A12 28 MOVI.N,  A8 $8 L L32R,  A8 CALLX8,
  A8 3 MOVI.N,  A8 A2 0 S32I.N,  A11 A2 MOV.N,  A10 13 MOVI.N,  A4 A2 4 S32I.N,
  MEMW,  A8 $C L L32R,  A8 CALLX8,
  A10 1 $8D L BNEI,
  A10 A4 MOV.N,  A8 A3 0 L32I.N,  A8 A8 176 L32I,  A8 CALLX8,
$82 L:
  A10 A2 MOV.N,  A4 A3 0 L32I,  A4 A4 176 L32I,  A4 CALLX8,
$8D L:
  A10 A1 MOV.N,  A8 $10 L L32R,  A8 CALLX8,
  A10 -1 $36 L BNEI,
$98 L:
  A2 0 MOVI.N,  RETW.N,
$9C L:  END-CODE

CODE wdev_set_promis_misc_buf   \ .text.wdev_set_promis_misc_buf $14
  $0 5 LITERALS
    SYMBOL dram1.16  SYMBOL g_osi_funcs_p  SYMBOL rodata_wlog_error.18
    SYMBOL wifi_log  SYMBOL memset
$14 L:
  A1 32 ENTRY,  A3 $0 L L32R,  A2 A2 0 8 EXTUI,  A10 A3 0 L32I,  A2 $6D L BEQZ,
  A2 $4 L L32R,  A10 $31 L BEQZ.N,
  A8 A2 0 L32I,  A8 A8 176 L32I,  A8 CALLX8,
$31 L:
  A10 $23C MOVI,  A2 A2 0 L32I.N,  A2 A2 $15C L32I,  A2 CALLX8,
  A10 A3 0 S32I.N,  A10 $5D L BNEZ.N,
  A12 1 MOVI,  A11 A12 11 SLLI,  A10 6 MOVI,  A13 $8 L L32R,  A2 $101 MOVI,  MEMW,
  A8 $C L L32R,  A8 CALLX8,
  $7C L J,
  2 ZEROS,
$5D L:
  A12 $23C MOVI,  A11 0 MOVI.N,  A8 $10 L L32R,  A8 CALLX8,
  A2 0 MOVI.N,  $7C L J,
$6D L:
  A10 $7C L BEQZ.N,
  A8 $4 L L32R,  A8 A8 0 L32I.N,  A8 A8 176 L32I,  A8 CALLX8,
  A2 A3 0 S32I.N,
$7C L:
  MEMW,  RETW.N,
$81 L:  END-CODE

CODE wdev_set_promis_ctrl_pkt   \ .text.wdev_set_promis_ctrl_pkt $C
  $0 3 LITERALS
    SYMBOL wDevCtrl  SYMBOL dram1.2  SYMBOL hal_sniffer_rx_set_promis
$C L:
  A1 32 ENTRY,  A2 A2 0 8 EXTUI,  A2 $2D L BEQZ.N,
  A8 $0 L L32R,  A9 A8 48 L8UI,  A9 $2D L BEQZ.N,
  A8 A8 52 L32I.N,  A8 1 $2D L BBCI,
  A11 1 MOVI.N,  A10 A11 MOV.N,  A8 $4 L L32R,  A12 A8 0 L32I.N,  $33 L J,
$2D L:
  A12 0 MOVI.N,  A11 A12 MOV.N,  A10 1 MOVI.N,
$33 L:
  A8 $8 L L32R,  A8 CALLX8,
  A2 0 MOVI.N,  RETW.N,
$3D L:  END-CODE

CODE wdev_set_promis   \ .text.wdev_set_promis $30
  $0 12 LITERALS
    SYMBOL rodata_wlog_info.19  SYMBOL wDevCtrl  SYMBOL rodata_wlog_info.20
    SYMBOL wifi_log  SYMBOL hal_sniffer_enable  SYMBOL wdev_set_promis_misc_buf
    SYMBOL wdev_set_promis_misc_pkt  SYMBOL wifi_log  SYMBOL hal_sniffer_disable
    SYMBOL wdev_set_promis_misc_buf  SYMBOL wdev_set_promis_misc_pkt
    SYMBOL wdev_set_promis_ctrl_pkt
$30 L:
  A1 32 ENTRY,  A11 1 MOVI.N,  A2 A2 0 8 EXTUI,  A3 $4 L L32R,  A11 A11 11 SLLI,
  A2 $7A L BEQZ.N,
  A12 3 MOVI.N,  A10 6 MOVI,  A13 $0 L L32R,  A2 1 MOVI,  A8 $C L L32R,  A8 CALLX8,
  A2 A3 48 S8I,  MEMW,  MEMW,  A8 $10 L L32R,  A8 CALLX8,
  A10 1 MOVI.N,  A8 $14 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  A10 $B0 L BNEZ,
  A10 1 MOVI.N,  A8 $18 L L32R,  A8 CALLX8,
  A10 1 MOVI.N,  $AA L J,
$7A L:
  A13 $8 L L32R,  A12 3 MOVI.N,  A10 6 MOVI,  A8 $1C L L32R,  A8 CALLX8,
  A2 A3 48 S8I,  MEMW,  MEMW,  A8 $20 L L32R,  A8 CALLX8,
  A10 A2 A2 OR,  A8 $24 L L32R,  A8 CALLX8,
  A10 A2 MOV.N,  A8 $28 L L32R,  A8 CALLX8,
  A10 A2 MOV.N,
$AA L:
  A8 $2C L L32R,  A8 CALLX8,
$B0 L:
  RETW.N,
$B2 L:  END-CODE

CODE wdev_set_promis_filter   \ .text.wdev_set_promis_filter $8
  $0 2 LITERALS
    $34 SYMBOL+ wDevCtrl  SYMBOL wdev_set_promis_misc_pkt
$8 L:
  A1 32 ENTRY,  A9 A2 0 L8UI,  A8 $0 L L32R,  A10 A2 1 L8UI,  A9 A8 0 S8I,  MEMW,
  A9 A2 2 L8UI,  A10 A8 1 S8I,  MEMW,  A10 1 MOVI.N,  A2 A2 3 L8UI,  A9 A8 2 S8I,
  A2 A8 3 S8I,  MEMW,  MEMW,  A8 $4 L L32R,  A8 CALLX8,
  A2 0 MOVI.N,  RETW.N,
$3E L:  END-CODE

CODE wdev_get_promis_filter   \ .text.wdev_get_promis_filter $4
  $0 1 LITERALS
    $34 SYMBOL+ wDevCtrl
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A9 A8 0 L8UI,  A10 A8 1 L8UI,  A9 A2 0 S8I,  MEMW,
  A9 A8 2 L8UI,  A10 A2 1 S8I,  MEMW,  A8 A8 3 L8UI,  A9 A2 2 S8I,  A8 A2 3 S8I,
  MEMW,  A2 0 MOVI.N,  MEMW,  RETW.N,
$32 L:  END-CODE

CODE wdev_set_promis_ctrl_filter   \ .text.wdev_set_promis_ctrl_filter $8
  $0 2 LITERALS
    SYMBOL dram1.2  SYMBOL wdev_set_promis_ctrl_pkt
$8 L:
  A1 32 ENTRY,  A9 A2 0 L8UI,  A8 $0 L L32R,  A10 A2 1 L8UI,  A9 A8 0 S8I,  MEMW,
  A9 A2 2 L8UI,  A10 A8 1 S8I,  MEMW,  A10 1 MOVI.N,  A2 A2 3 L8UI,  A9 A8 2 S8I,
  A2 A8 3 S8I,  MEMW,  MEMW,  A8 $4 L L32R,  A8 CALLX8,
  A2 0 MOVI.N,  RETW.N,
$3E L:  END-CODE

CODE wdev_get_promis_ctrl_filter   \ .text.wdev_get_promis_ctrl_filter $4
  $0 1 LITERALS
    SYMBOL dram1.2
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A9 A8 0 L8UI,  A10 A8 1 L8UI,  A9 A2 0 S8I,  MEMW,
  A9 A8 2 L8UI,  A10 A2 1 S8I,  MEMW,  A8 A8 3 L8UI,  A9 A2 2 S8I,  A8 A2 3 S8I,
  MEMW,  A2 0 MOVI.N,  MEMW,  RETW.N,
$32 L:  END-CODE

CODE wDev_SnifferRxAmpdu   \ .text.wDev_SnifferRxAmpdu $10
  $0 4 LITERALS
    SYMBOL wDevCtrl  SYMBOL g_osi_funcs_p  SYMBOL memcpy  SYMBOL pp_post
$10 L:
  A1 32 ENTRY,  A3 $0 L L32R,  A4 36 MOVI.N,  A3 A3 52 L32I.N,  A4 A3 $84 L BNONE,
  A10 8 MOVI.N,  A2 A2 4 L32I.N,  A4 $4 L L32R,  A6 A2 4 L32I.N,  A2 A4 0 L32I.N,
  A2 A2 $16C L32I,  A2 CALLX8,
  A3 A10 MOV.N,  A2 A6 24 L32I.N,  A2 A2 0 12 EXTUI,  A2 A2 28 ADDI,
  A2 A2 0 16 EXTUI,  A10 $84 L BEQZ,
  A5 A4 0 L32I.N,  A10 A2 MOV.N,  A5 A5 $16C L32I,  A5 CALLX8,
  A5 A10 MOV.N,  A10 $50 L BNEZ.N,
  $7A L J,
  1 ZEROS,
$50 L:
  A12 A2 MOV.N,  A11 A6 MOV.N,  A8 $8 L L32R,  A8 CALLX8,
  A2 2 MOVI.N,  A2 A3 0 S32I.N,  A11 A3 MOV.N,  A10 13 MOVI.N,  A5 A3 4 S32I.N,
  MEMW,  A8 $C L L32R,  A8 CALLX8,
  A10 1 $84 L BNEI,
  A10 A5 MOV.N,  A2 A4 0 L32I.N,  A2 A2 176 L32I,  A2 CALLX8,
$7A L:
  A10 A3 MOV.N,  A2 A4 0 L32I.N,  A2 A2 176 L32I,  A2 CALLX8,
$84 L:
  RETW.N,
$86 L:  END-CODE

CODE wDev_RxSecError   \ .text.wDev_RxSecError $0
  $0 ORIGIN
  A1 32 ENTRY,  A2 A2 0 8 EXTUI,  A9 A2 31 ADDI,  A9 A9 0 8 EXTUI,  A8 1 MOVI,
  A9 6 $15 L BLTUI,
  A8 0 MOVI,
$15 L:
  A2 A2 16 ADDI,  A2 A2 0 8 EXTUI,  A10 12 MOVI.N,  A9 1 MOVI.N,  A10 A2 $24 L BGEU,
  A9 0 MOVI.N,
$24 L:
  A2 A8 A9 OR,  A2 A2 0 1 EXTUI,  RETW.N,
$2C L:  END-CODE

CODE wDev_GetNoiseFloor   \ .wifiorslpiram.25 $8
  $0 2 LITERALS
    SYMBOL wDevCtrl  SYMBOL read_hw_noisefloor
$8 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A8 A2 47 L8UI,  A8 A8 7 SEXT,  A8 1 $28 L BLTI,
  A8 $4 L L32R,  A8 CALLX8,
  A10 A10 2 ADDI.N,  A10 A10 2 SRAI,  A10 A2 47 S8I,  MEMW,
$28 L:
  A2 A2 47 L8UI,  A2 A2 7 SEXT,  MEMW,  RETW.N,
$33 L:  END-CODE

CODE wdev_csi_rx_process   \ .text.wdev_csi_rx_process $24
  $0 9 LITERALS
    SYMBOL g_wdev_csi_rx  SYMBOL g_osi_funcs_p  SYMBOL wDevCtrl
    SYMBOL g_wdev_csi_rx_ctx  SYMBOL memcpy  SYMBOL memset  SYMBOL memcpy
    SYMBOL memcpy  SYMBOL wDev_GetNoiseFloor
$24 L:
  A1 48 ENTRY,  A3 A3 0 16 EXTUI,  A3 A1 0 S32I.N,  A3 A6 0 16 EXTUI,
  A7 3 $35 L BNEI,
  $134 L J,
$35 L:
  A6 $0 L L32R,  A8 A6 0 L32I.N,  A8 $134 L BEQZ,
  A10 64 MOVI.N,  A6 $4 L L32R,  A8 A6 0 L32I.N,  A8 A8 $16C L32I,  MEMW,
  A8 CALLX8,
  A6 A10 MOV.N,  A10 $134 L BEQZ,
  A9 15 MOVI.N,  A10 A10 28 ADDI,  A12 6 MOVI.N,  A9 A3 $68 L BGEU,
  A11 A5 10 ADDI.N,  A8 $10 L L32R,  A8 CALLX8,
  $70 L J,
  1 ZEROS,
$68 L:
  A11 0 MOVI.N,  A8 $14 L L32R,  A8 CALLX8,
$70 L:
  A12 6 MOVI.N,  A11 A5 4 ADDI.N,  A10 A6 34 ADDI,  A8 $18 L L32R,  A8 CALLX8,
  A10 $8 L L32R,  A11 A4 MOV.N,  A12 28 MOVI.N,  A7 A7 -1 ADDI,  A9 A10 45 L8UI,
  A9 A9 4 SLLI,  A10 A10 44 L8UI,  A10 A10 0 4 EXTUI,  A9 A9 A10 OR,  A10 A6 A6 OR,
  A9 A4 10 S8I,  MEMW,  MEMW,  A8 $1C L L32R,  A8 CALLX8,
  A8 $20 L L32R,  A8 CALLX8,
  A9 1 MOVI.N,  A10 A6 8 S8I,  MEMW,  A10 0 MOVI.N,  A11 A10 MOV.N,
  A11 A9 A7 MOVEQZ,  A2 A6 44 S32I.N,  A2 A1 0 L32I.N,  A2 A6 48 S16I,
  A2 A5 24 ADDI,  A3 A6 60 S16I,  A3 12 MOVI.N,  A11 A6 40 S8I,  MEMW,
  A5 A6 52 S32I.N,  A2 A6 56 S32I.N,  A2 A5 0 L8UI,  A2 A2 A3 AND,  A2 4 $E7 L BNEI,
  A2 -1 MOVI.N,  MEMW,  $10E L J,
$E7 L:
  A2 A4 27 L8UI,  A4 -198 MOVI,  A3 A10 MOV.N,  A4 A2 A4 ADD.N,  A3 A9 A2 MOVEQZ,
  A10 A9 A4 MOVEQZ,  A3 A3 A10 OR,  A3 $108 L BNEZ.N,
  A4 -245 MOVI,  A2 A2 A4 ADD,  A9 A3 A2 MOVNEZ,  A9 $114 L BEQZ,
$108 L:
  A2 A5 22 L16UI,  A2 A2 4 SRLI,
$10E L:
  A2 A6 62 S16I,  MEMW,
$114 L:
  A11 A6 MOV.N,  A3 $0 L L32R,  A2 A3 0 L32I.N,  A3 $C L L32R,  A10 A3 0 L32I.N,
  MEMW,  A2 CALLX8,
  A10 A6 MOV.N,  A3 $4 L L32R,  A2 A3 0 L32I,  A2 A2 176 L32I,  A2 CALLX8,
$134 L:
  RETW.N,
$136 L:  END-CODE

CODE wDev_SnifferRxData   \ .text.wDev_SnifferRxData $34
  $0 13 LITERALS
    SYMBOL wDevCtrl  SYMBOL g_osi_funcs_p  SYMBOL wdev_csi_len_align
    SYMBOL wdev_csi_hw_bug_check  SYMBOL memcpy  SYMBOL memcpy  SYMBOL memcpy
    SYMBOL memcpy  SYMBOL memcpy  SYMBOL memcpy  SYMBOL memcpy
    SYMBOL wdev_csi_rx_process  SYMBOL pp_post
$34 L:
  A1 48 ENTRY,  A6 $0 L L32R,  A3 A3 0 8 EXTUI,  A2 A1 8 S32I.N,  A2 A6 0 L32I.N,
  A4 A1 12 S32I.N,  A4 A2 4 L32I.N,  A2 A1 12 L32I.N,  A2 64 $4D L BNEI,
  $28E L J,
$4D L:
  A2 98 MOVI,  A5 A1 12 L32I,  A5 A2 $59 L BNE,
  $10A L J,
$59 L:
  A10 A4 MOV.N,  MEMW,  A8 $8 L L32R,  A8 CALLX8,
  A8 0 MOVI.N,  A2 A10 0 16 EXTUI,  A8 A1 0 S32I.N,  A2 A8 $BC L BEQ,
  A5 A10 0 16 EXTUI,  A5 A5 24 ADDI,  A5 A4 A5 ADD.N,  A7 A4 24 ADDI,  A11 A5 MOV.N,
  A10 A7 MOV.N,  MEMW,  A8 $C L L32R,  A8 CALLX8,
  A10 A1 0 S32I.N,  A10 1 $B2 L BNEI,
  A10 A4 24 L8UI,  A11 A4 25 L8UI,  A10 A5 0 S8I,  MEMW,  A10 A4 26 L8UI,
  A11 A5 1 S8I,  MEMW,  A8 A4 27 L8UI,  A10 A5 2 S8I,  A8 A5 3 S8I,  MEMW,  MEMW,
  $BC L J,
$B2 L:
  A9 A1 0 L32I.N,  A8 0 MOVI.N,  A5 A9 -2 ADDI,  A2 A8 A5 MOVEQZ,
$BC L:
  A5 A4 A2 ADD.N,  A10 A5 28 L16UI,  A5 0 MOVI.N,  A8 A6 0 L32I.N,  $D7 L J,
$C8 L:
  MEMW,  A11 A8 0 L32I.N,  A11 A11 12 12 EXTUI,  A5 A11 A5 ADD.N,  A5 A5 0 16 EXTUI,
  A8 A8 8 L32I.N,
$D7 L:
  A9 A1 8 L32I.N,  A8 A9 $C8 L BNE,
  A8 A5 A2 SUB,  A5 A10 0 4 EXTUI,  MEMW,  A11 A9 0 L32I.N,  A10 1 MOVI.N,
  A11 A11 12 12 EXTUI,  A8 A8 A11 ADD.N,  A7 A8 0 16 EXTUI,  A5 A1 4 S32I.N,
  A5 $116 L BEQZ.N,
  A11 2 MOVI.N,  A5 A5 -4 ADDI,  A10 A11 A5 MOVNEZ,  A10 A1 4 S32I.N,
  A10 20 MOVI.N,  A10 A11 A5 MOVEQZ,  MEMW,  $116 L J,
$10A L:
  A2 0 MOVI.N,  A5 3 MOVI.N,  A2 A1 0 S32I.N,  A7 28 MOVI.N,  A10 8 MOVI.N,
  A5 A1 4 S32I.N,
$116 L:
  A5 A6 52 L32I.N,  A10 A5 $11E L BANY,
  $28E L J,
$11E L:
  A10 8 MOVI.N,  A8 $4 L L32R,  A5 A8 0 L32I.N,  A5 A5 $16C L32I,  MEMW,  A5 CALLX8,
  A5 A10 MOV.N,  A10 $28E L BEQZ,
  A9 $4 L L32R,  A10 A9 0 L32I.N,  A11 A10 $16C L32I,  A10 A7 MOV.N,  A11 CALLX8,
  A7 A10 MOV.N,  A10 $14A L BNEZ.N,
  A3 $4 L L32R,  $284 L J,
$14A L:
  A8 A1 12 L32I.N,  A10 98 MOVI,  A8 A10 $191 L BNE,
  A2 A2 24 ADDI,  A2 A4 A2 ADD.N,  A11 A4 MOV.N,  A12 24 MOVI.N,  A10 A7 A7 OR,
  A8 $10 L L32R,  A8 CALLX8,
  A4 A2 0 L8UI,  A3 A7 24 ADDI,  A6 A2 1 L8UI,  A4 A7 24 S8I,  MEMW,  A4 A2 2 L8UI,
  A6 A7 25 S8I,  MEMW,  A2 A2 3 L8UI,  A4 A7 26 S8I,  A2 A7 27 S8I,  MEMW,  MEMW,
  $260 L J,
  3 ZEROS,
$191 L:
  A3 2 $1C4 L BGEUI,
  A11 A4 MOV.N,  A12 24 MOVI,  A10 A7 A7 OR,  A8 $14 L L32R,  A8 CALLX8,
  A3 A6 0 L32I.N,  A11 A2 24 ADDI,  A11 A4 A11 ADD.N,  A10 A7 24 ADDI,  MEMW,
  A12 A3 0 L32I.N,  A12 A12 12 12 EXTUI,  A12 A12 -24 ADDI,  A12 A12 A2 SUB,
  A8 $18 L L32R,  A8 CALLX8,
  $242 L J,
  1 ZEROS,
$1C4 L:
  A4 A6 0 L32I.N,  A3 A7 MOV.N,  A10 1 MOVI.N,  $226 L J,
$1CD L:
  A10 $208 L BEQZ.N,
  A12 24 MOVI.N,  A10 A3 MOV.N,  A8 $1C L L32R,  A8 CALLX8,
  MEMW,  A10 A4 0 L32I.N,  A9 -24 MOVI.N,  A10 A10 12 12 EXTUI,  A12 A9 A2 SUB,
  A11 A2 24 ADDI,  A13 A4 4 L32I.N,  A12 A12 A10 ADD.N,  A11 A13 A11 ADD.N,
  A10 A3 24 ADDI,  A8 $20 L L32R,  A8 CALLX8,
  MEMW,  A10 A4 0 L32I.N,  A10 A10 12 12 EXTUI,  A10 A10 A2 SUB,  $220 L J,
  2 ZEROS,
$208 L:
  MEMW,  A12 A4 0 L32I.N,  A10 A3 MOV.N,  A12 A12 12 12 EXTUI,  A8 $24 L L32R,
  A8 CALLX8,
  MEMW,  A10 A4 0 L32I.N,  A10 A10 12 12 EXTUI,
$220 L:
  A3 A3 A10 ADD.N,  A10 0 MOVI.N,  A4 A4 8 L32I.N,
$226 L:
  A8 A1 8 L32I.N,  A11 A4 4 L32I.N,  A4 A8 $1CD L BNE,
  A10 A3 MOV.N,  MEMW,  A12 A4 0 L32I.N,  A12 A12 12 12 EXTUI,  A8 $28 L L32R,
  A8 CALLX8,
  MEMW,  A3 A4 0 L32I.N,
$242 L:
  A2 $260 L BEQZ.N,
  A13 A7 28 ADDI,  A12 A7 MOV.N,  A11 A2 MOV.N,  A3 A6 0 L32I.N,  A14 A7 24 L32I.N,
  A14 A14 0 12 EXTUI,  A10 A3 4 L32I,  A10 A10 24 ADDI,  A15 A1 0 L32I.N,
  A8 $2C L L32R,  A8 CALLX8,
$260 L:
  A11 A5 MOV.N,  A10 13 MOVI.N,  A9 A1 4 L32I.N,  A7 A5 4 S32I,  A9 A5 0 S32I.N,
  MEMW,  A8 $30 L L32R,  A8 CALLX8,
  A10 1 $28E L BNEI,
  A10 A7 MOV.N,  A3 $4 L L32R,  A2 A3 0 L32I.N,  A2 A2 176 L32I,  A2 CALLX8,
$284 L:
  A10 A5 MOV.N,  A2 A3 0 L32I.N,  A2 A2 176 L32I,  A2 CALLX8,
$28E L:
  RETW.N,
$290 L:  END-CODE

CODE wDev_IndicateCtrlFrame   \ .text.wDev_IndicateCtrlFrame $24
  $0 9 LITERALS
    SYMBOL rodata_wlog_warning.22  SYMBOL wDevCtrl  SYMBOL g_osi_funcs_p
    SYMBOL wifi_log  SYMBOL wdev_csi_len_align  SYMBOL wdev_csi_hw_bug_check
    SYMBOL memcpy  SYMBOL memcpy  SYMBOL wdev_csi_rx_process
$24 L:
  A1 32 ENTRY,  A14 A4 0 16 EXTUI,  A14 1 $3F L BEQI,
  A13 $0 L L32R,  A12 2 MOVI.N,  A11 32 MOVI.N,  A10 6 MOVI.N,  A8 $C L L32R,
  A8 CALLX8,
  $114 L J,
$3F L:
  A2 $4 L L32R,  A3 A2 0 L32I,  A5 A3 4 L32I,  A10 A5 MOV.N,  A8 $10 L L32R,
  A8 CALLX8,
  A4 A10 0 16 EXTUI,  A4 $114 L BEQZ,
  A2 A2 48 L8UI,  A2 1 $61 L BNEI,
  A2 A5 0 L32I.N,  A2 $114 L BLTZ,
$61 L:
  A2 A10 0 16 EXTUI,  A2 A2 24 ADDI,  A6 A5 24 ADDI,  A2 A5 A2 ADD.N,  A11 A2 MOV.N,
  A10 A6 MOV.N,  A8 $14 L L32R,  A8 CALLX8,
  A7 A10 MOV.N,  A10 1 $A4 L BNEI,
  A8 A5 24 L8UI,  A5 A5 25 L8UI,  A8 A2 0 S8I,  MEMW,  A8 A6 2 L8UI,  A5 A2 1 S8I,
  MEMW,  A5 A6 3 L8UI,  A8 A2 2 S8I,  A5 A2 3 S8I,  MEMW,  MEMW,  $AC L J,
  2 ZEROS,
$A4 L:
  A2 A10 -2 ADDI,  A5 0 MOVI.N,  A4 A5 A2 MOVEQZ,
$AC L:
  A5 $8 L L32R,  MEMW,  A10 A3 0 L32I.N,  A10 A10 12 12 EXTUI,  A10 A10 A4 SUB,
  A2 A5 0 L32I.N,  A2 A2 $16C L32I,  A2 CALLX8,
  A2 A10 MOV.N,  A10 $114 L BEQZ,
  A11 A3 4 L32I.N,  A12 24 MOVI.N,  A8 $18 L L32R,  A8 CALLX8,
  A6 A4 24 ADDI,  A10 A2 24 ADDI,  MEMW,  A12 A3 0 L32I.N,  A12 A12 12 12 EXTUI,
  A12 A12 A4 SUB,  A12 A12 -24 ADDI,  A11 A3 4 L32I.N,  A11 A11 A6 ADD,
  A8 $1C L L32R,  A8 CALLX8,
  A15 A7 MOV.N,  A10 A3 4 L32I.N,  A13 A2 28 ADDI,  A10 A10 24 ADDI,  A12 A2 MOV.N,
  A11 A4 MOV.N,  A14 A2 24 L32I,  A14 A14 0 12 EXTUI,  A8 $20 L L32R,  A8 CALLX8,
  A10 A2 MOV.N,  A3 A5 0 L32I.N,  A3 A3 176 L32I,  A3 CALLX8,
$114 L:
  A2 1 MOVI.N,  RETW.N,
$118 L:  END-CODE

CODE wDev_record_ftm_data   \ .wifirxiram.26 $0
  $0 ORIGIN
  A1 32 ENTRY,  RETW.N,
$5 L:  END-CODE

CODE wdev_nan_set_callbacks   \ .text.wdev_nan_set_callbacks $4
  $0 1 LITERALS
    SYMBOL g_wdev_is_nan_pkt_in_valid~1547
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A8 0 S32I.N,  A2 0 MOVI.N,  MEMW,  RETW.N,
$13 L:  END-CODE

CODE wDev_isNANPktInValidSlot   \ .text.wDev_isNANPktInValidSlot $4
  $0 1 LITERALS
    SYMBOL g_wdev_is_nan_pkt_in_valid~1547
$4 L:
  A1 32 ENTRY,  A8 A2 44 L32I.N,  A9 1 MOVI.N,  A9 A9 19 SLLI,  A10 A2 MOV.N,
  A2 1 MOVI.N,  A11 A8 16 L32I.N,  A8 3 MOVI.N,  A8 A8 18 SLLI,  A11 A11 A8 AND,
  A11 A9 $2E L BNE,
  A8 $0 L L32R,  A2 0 MOVI.N,  A9 A8 0 L32I.N,  A9 A2 $2E L BEQ,
  A9 CALLX8,
  A2 A10 MOV.N,
$2E L:
  RETW.N,
$30 L:  END-CODE

CODE wDev_ProcessRxData_NAN_Int~2C06   \ wDev_ProcessRxData_NAN_Interface_Hook: .text.wDev_ProcessRxData_NAN_Interface_Hook $14
  $0 5 LITERALS
    SYMBOL NAN_STANDARD_BSSID  $BFFFFFFF T,  SYMBOL NAN_NETWORK_ID  SYMBOL memcmp
    SYMBOL memcmp
$14 L:
  A1 32 ENTRY,  A8 A3 0 L16UI,  A4 240 MOVI,  A8 A8 A4 AND,  A8 128 $64 L BNEI,
  A12 4 MOVI,  A11 $0 L L32R,  A10 A3 16 ADDI,  A8 $C L L32R,  A8 CALLX8,
  A10 $50 L BNEZ.N,
$34 L:
  A3 -113 MOVI,  A8 A2 3 L8UI,  A4 1 MOVI.N,  A8 A8 A3 AND,  A3 64 MOVI.N,
  A8 A8 A3 OR,  A8 A2 3 S8I,  MEMW,  MEMW,  $7C L J,
$50 L:
  A3 A2 0 L32I.N,  A4 $4 L L32R,  A3 A3 A4 AND,  A4 0 MOVI.N,  A3 A2 0 S32I.N,
  MEMW,  $7C L J,
  2 ZEROS,
$64 L:
  A9 208 MOVI,  A4 1 MOVI.N,  A8 A9 $7C L BNE,
  A11 $8 L L32R,  A12 6 MOVI.N,  A10 A3 4 ADDI.N,  A8 $10 L L32R,  A8 CALLX8,
  A10 $34 L BEQZ,
$7C L:
  A2 A4 MOV.N,  RETW.N,
$80 L:  END-CODE

CODE wDev_FetchFirstDesc   \ .text.wDev_FetchFirstDesc $0
  $0 ORIGIN
  A1 32 ENTRY,  A2 0 MOVI.N,  RETW.N,
$7 L:  END-CODE

CODE wDev_Rxbuf_Deinit   \ .text.wDev_Rxbuf_Deinit $10
  $0 4 LITERALS
    SYMBOL wDevCtrl  SYMBOL g_osi_funcs_p  SYMBOL rodata_wlog_info.39
    SYMBOL wifi_log
$10 L:
  A1 32 ENTRY,  A3 $0 L L32R,  A4 0 MOVI.N,  A5 A3 0 L32I.N,  A2 A5 MOV.N,
  A7 $4 L L32R,  $3D L J,
$22 L:
  A10 A2 4 L32I.N,  A6 A2 8 L32I.N,  A10 $36 L BEQZ,
  A8 A7 0 L32I,  A8 A8 176 L32I,  A8 CALLX8,
  A8 0 MOVI.N,  A8 A2 4 S32I.N,
$36 L:
  A4 A4 1 ADDI.N,  A5 A6 $40 L BEQ,
  A2 A6 MOV.N,
$3D L:
  A2 $22 L BNEZ,
$40 L:
  A13 $8 L L32R,  A10 6 MOVI.N,  A2 0 MOVI.N,  A14 A4 MOV.N,  A12 3 MOVI.N,
  A11 1 MOVI,  A2 A3 0 S32I,  MEMW,  A8 $C L L32R,  A8 CALLX8,
  A10 A3 8 L32I.N,  A10 A2 $6C L BEQ,
  A4 $4 L L32R,  A4 A4 0 L32I.N,  A4 A4 176 L32I,  A4 CALLX8,
  A2 A3 8 S32I.N,
$6C L:
  MEMW,  RETW.N,
$71 L:  END-CODE

CODE wDev_Rxbuf_Init   \ .text.wDev_Rxbuf_Init $5C
  $0 23 LITERALS
    SYMBOL g_osi_funcs_p  SYMBOL wDevCtrl  SYMBOL rodata_wlog_error.34
    SYMBOL pTxRx  SYMBOL g_wifi_menuconfig  $8A4 T,  SYMBOL rodata_wlog_info.35
    SYMBOL rodata_wlog_warning.33  $FF000FFF T,  $BFFFFFFF T,  $DFFFFFFF T,
    SYMBOL rodata_wlog_info.36  SYMBOL rodata_wlog_info.37
    SYMBOL rodata_wlog_error.38  $DEADBEEF T,  SYMBOL wifi_log  SYMBOL wifi_log
    SYMBOL wifi_log  SYMBOL memset  SYMBOL wifi_log  SYMBOL wifi_log
    SYMBOL wifi_log  SYMBOL wDev_Rxbuf_Deinit
$5C L:
  A1 64 ENTRY,  A4 $0 L L32R,  A10 A2 A2 ADDX2,  A10 A10 2 SLLI,
  A10 A10 0 16 EXTUI,  A3 A4 0 L32I,  A3 A3 $168 L32I,  A3 CALLX8,
  A3 $4 L L32R,  A10 A3 8 S32I.N,  A10 $94 L BNEZ.N,
  A12 1 MOVI.N,  A13 $8 L L32R,  A14 A2 MOV.N,  A11 A12 MOV.N,  A10 6 MOVI,  MEMW,
  A8 $3C L L32R,  A8 CALLX8,
  $1DE L J,
  1 ZEROS,
$94 L:
  A5 $10 L L32R,  A4 $C L L32R,  A8 A5 24 L32I.N,  A7 A4 0 L32I.N,  A8 $A6 L BEQZ.N,
  A8 $14 L L32R,  A8 A7 $3C4 S32I,
$A6 L:
  A12 3 MOVI.N,  A11 1 MOVI.N,  A10 6 MOVI.N,  A5 0 MOVI.N,  A14 A7 $3C4 L32I,
  A7 A1 A1 OR,  A13 $18 L L32R,  MEMW,  A8 $40 L L32R,  A8 CALLX8,
  A9 A2 0 16 EXTUI,  A4 A4 0 L32I.N,  A4 A4 $3C4 L32I,  A4 A4 4 ADDI.N,
  A6 A4 0 16 EXTUI,  A4 A4 0 12 EXTUI,  A8 A3 8 L32I.N,  A9 A1 16 S32I.N,  MEMW,
  $186 L J,
  2 ZEROS,
$DC L:
  A10 $0 L L32R,  A9 A10 0 L32I.N,  A10 A6 MOV.N,  A9 A9 $15C L32I,
  A8 A1 20 S32I.N,  MEMW,  A9 CALLX8,
  A8 A1 20 L32I.N,  A10 $106 L BNEZ.N,
  A13 $1C L L32R,  A12 2 MOVI.N,  A11 1 MOVI.N,  A10 6 MOVI.N,  A8 $44 L L32R,
  A8 CALLX8,
  $198 L J,
  2 ZEROS,
$106 L:
  A11 0 MOVI.N,  A12 A6 MOV.N,  A8 A1 20 S32I.N,  MEMW,  A8 $48 L L32R,  A8 CALLX8,
  A5 A5 1 ADDI.N,  A8 A1 20 L32I.N,  MEMW,  A9 A8 0 L32I.N,  A10 A8 4 S32I.N,
  A10 -1 MOVI.N,  A10 A10 12 SLLI,  A9 A9 A10 AND,  A9 A9 A4 OR,  MEMW,
  A9 A8 0 S32I.N,  A10 -1 MOVI.N,  A10 A10 31 SLLI,  MEMW,  A9 A8 0 L32I.N,
  A9 A9 A10 OR,  MEMW,  A9 A8 0 S32I.N,  MEMW,  A10 A8 0 L32I.N,
  A10 A10 0 12 EXTUI,  A10 A10 12 SLLI,  MEMW,  A9 A8 0 L32I.N,  A11 $20 L L32R,
  A9 A9 A11 AND,  A9 A9 A10 OR,  MEMW,  A9 A8 0 S32I.N,  MEMW,  A9 A8 0 L32I.N,
  A10 $24 L L32R,  A9 A9 A10 AND,  MEMW,  A9 A8 0 S32I.N,  MEMW,  A9 A8 0 L32I.N,
  A10 $28 L L32R,  A9 A9 A10 AND,  MEMW,  A9 A8 0 S32I.N,  A8 A7 8 S32I.N,
  A7 A8 MOV.N,  A8 A8 12 ADDI.N,
$186 L:
  A9 A1 16 L32I.N,  A5 A9 $18E L BGE,
  $DC L J,
$18E L:
  A4 0 MOVI.N,  A4 A7 8 S32I.N,  A4 A1 8 L32I.N,  A7 A3 4 S32I.N,  A4 A3 0 S32I.N,
$198 L:
  A14 A5 MOV.N,  A12 3 MOVI.N,  A11 1 MOVI.N,  A10 6 MOVI.N,  A13 $2C L L32R,  MEMW,
  A8 $4C L L32R,  A8 CALLX8,
  A12 3 MOVI.N,  A11 1 MOVI.N,  A10 $10 L L32R,  A13 $30 L L32R,  A14 A10 4 L32I.N,
  A10 6 MOVI.N,  A8 $50 L L32R,  A8 CALLX8,
  A5 A2 $1E4 L BGEU,
  A12 1 MOVI.N,  A13 $34 L L32R,  A15 A5 MOV.N,  A14 A2 MOV.N,  A11 A12 A12 OR,
  A10 6 MOVI,  A8 $54 L L32R,  A8 CALLX8,
  A8 $58 L L32R,  A8 CALLX8,
$1DE L:
  A2 $101 MOVI,  $249 L J,
$1E4 L:
  A2 A3 0 L32I.N,  A7 $20 L L32R,  A6 $38 L L32R,  $240 L J,
  1 ZEROS,
$1F0 L:
  MEMW,  A4 A2 0 L32I.N,  A8 -1 MOVI.N,  A4 A4 0 12 EXTUI,  A4 A4 -4 ADDI,
  A8 A8 12 SLLI,  MEMW,  A5 A2 0 L32I.N,  A4 A4 0 12 EXTUI,  A5 A5 A8 AND,
  A4 A5 A4 OR,  MEMW,  A4 A2 0 S32I.N,  MEMW,  A5 A2 0 L32I.N,  A5 A5 0 12 EXTUI,
  A5 A5 12 SLLI,  MEMW,  A4 A2 0 L32I.N,  A4 A4 A7 AND,  A4 A4 A5 OR,  MEMW,
  A4 A2 0 S32I.N,  A4 A2 4 L32I.N,  A6 A4 0 S32I.N,  MEMW,  A5 A2 0 L32I.N,
  A5 A5 0 12 EXTUI,  A4 A4 A5 ADD.N,  A6 A4 0 S32I.N,  A2 A2 8 L32I.N,
$240 L:
  A2 $1F0 L BNEZ,
  A2 A3 48 S8I,  MEMW,
$249 L:
  MEMW,  RETW.N,
$24E L:  END-CODE

CODE wDev_disable_low_rate   \ .text.wDev_disable_low_rate $4
  $0 1 LITERALS
    SYMBOL hal_mac_disable_low_rate
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$F L:  END-CODE

CODE wDev_enable_low_rate   \ .text.wDev_enable_low_rate $4
  $0 1 LITERALS
    SYMBOL hal_mac_enable_low_rate
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$F L:  END-CODE

CODE wDev_is_low_rate_enable   \ .text.wDev_is_low_rate_enable $4
  $0 1 LITERALS
    SYMBOL hal_mac_is_low_rate_enabled
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  RETW.N,
$11 L:  END-CODE

CODE wdev_process_panic_watchdog   \ .iram1.44 $1C
  $0 7 LITERALS
    $3FF00024 T,  SYMBOL hal_mac_rx_get_end_state  SYMBOL bb_wdt_get_status
    $3FF40078 T,  $3FF40078 T,  SYMBOL bb_wdt_timeout_clear
    SYMBOL hal_mac_interrupt_clr_watchdog
$1C L:
  A1 32 ENTRY,  A8 $4 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  A8 $8 L L32R,  A8 CALLX8,
  A8 1 MOVI.N,  A8 A8 29 SLLI,  A8 A10 A8 AND,  A8 $81 L BNEZ,
  A8 240 MOVI,  A10 A10 A8 AND,  A10 32 $81 L BNEI,
  A2 A2 -100 ADDI,  A2 $81 L BNEZ.N,
  A9 $0 L L32R,  A8 A9 MOV.N,  A2 $C L L32R,  A10 3 RSIL,  A2 A2 0 L32I.N,
  A8 A8 0 L32I.N,  A10 PS WSR,  RSYNC,  A2 1 MOVI.N,  A8 A8 A2 OR,  MEMW,
  A8 A9 0 S32I.N,  A8 A9 MOV.N,  A2 $10 L L32R,  A10 3 RSIL,  A2 A2 0 L32I.N,
  A8 A8 0 L32I.N,  A10 PS WSR,  RSYNC,  A2 -2 MOVI.N,  A8 A8 A2 AND,  MEMW,
  A8 A9 0 S32I.N,
$81 L:
  MEMW,  A8 $14 L L32R,  A8 CALLX8,
  A8 $18 L L32R,  A8 CALLX8,
  RETW.N,
$92 L:  END-CODE

CODE wDev_ProcessFiq   \ .iram1.45 $74
  $0 29 LITERALS
    SYMBOL g_osi_funcs_p  $1000024 T,  SYMBOL wDevCtrl  $4 SYMBOL+ dram1.40
    $8 SYMBOL+ dram1.40  $C SYMBOL+ dram1.40  SYMBOL dram1.40  $4 SYMBOL+ dram1.41
    SYMBOL dram1.41  $6001C06C T,  SYMBOL dram1.43  SYMBOL dram1.42  $FF0000FF T,
    $FFF T,  $FFFF3FFF T,  $F7FFFF00 T,  $FFF000 T,
    SYMBOL hal_mac_interrupt_get_event  SYMBOL hal_mac_interrupt_clr_event
    SYMBOL wdev_process_panic_watchdog  SYMBOL pp_post
    SYMBOL hal_mac_rx_get_end_info  SYMBOL hal_sniffer_rx_clr_statistics
    SYMBOL wdev_push_promis_misc_buf  SYMBOL pp_post  SYMBOL lmacProcessRxSucData
    SYMBOL lmacPostTxComplete  SYMBOL lmacProcessAllTxTimeout
    SYMBOL lmacProcessCollisions
$74 L:
  A1 32 ENTRY,  A3 $0 L L32R,  A2 A3 0 L32I.N,  A2 A2 216 L32I,  A2 CALLX8,
$82 L:
  A8 $44 L L32R,  A8 CALLX8,
  A2 A10 A10 OR,  A10 $208 L BEQZ,
  A8 $48 L L32R,  A8 CALLX8,
  A2 11 $9D L BBCI,
  A8 $4C L L32R,  A8 CALLX8,
$9D L:
  A4 3 MOVI.N,  A4 A4 21 SLLI,  A2 A4 $B0 L BNONE,
  A11 1 MOVI,  A10 14 MOVI.N,  A8 $50 L L32R,  A8 CALLX8,
$B0 L:
  A4 $4 L L32R,  A2 A4 $B9 L BANY,
  $1E7 L J,
$B9 L:
  A4 $8 L L32R,  A5 A4 48 L8UI,  A5 $1E1 L BEQZ,
  A4 A4 52 L32I.N,  A4 3 $CA L BBSI,
  $1E1 L J,
$CA L:
  A7 $18 L L32R,  A12 A7 MOV.N,  A5 $14 L L32R,  A13 A5 MOV.N,  A4 $1C L L32R,
  A11 A4 A4 OR,  A6 $20 L L32R,  A10 A6 A6 OR,  A15 $C L L32R,  A14 $10 L L32R,
  A8 $54 L L32R,  A8 CALLX8,
  A10 98 MOVI,  A8 $24 L L32R,  A7 A7 0 L32I.N,  MEMW,  A8 A8 0 L32I.N,
  A7 A10 $FF L BEQ,
  $1E1 L J,
$FF L:
  A7 $28 L L32R,  A10 A7 0 L8UI,  A6 A6 0 L32I.N,  A10 $120 L BNEZ.N,
  A4 A4 0 L32I.N,  A4 A6 $111 L BEQ,
  $132 L J,
$111 L:
  A4 1 MOVI.N,  A4 A7 0 S8I,  MEMW,  MEMW,  $1E1 L J,
  1 ZEROS,
$120 L:
  A10 -17 MOVI.N,  A10 A6 $12A L BLTU,
  A9 A4 0 L32I.N,  A9 A6 $141 L BGEU,
$12A L:
  A4 0 MOVI.N,  A4 A7 0 S8I,  MEMW,
$132 L:
  MEMW,  A8 $58 L L32R,  A8 CALLX8,
  $1E1 L J,
  3 ZEROS,
$141 L:
  A9 A6 $147 L BEQ,
  $1E1 L J,
$147 L:
  A10 $2C L L32R,  A7 A5 0 L32I.N,  A5 A7 16 16 EXTUI,  A9 A5 8 SLLI,
  A6 A10 4 L32I.N,  A11 $30 L L32R,  A6 A6 A11 AND,  A6 A6 A9 OR,  A9 $34 L L32R,
  A6 A10 4 S32I.N,  A9 A5 $1E1 L BLTU,
  A8 A8 8 SRLI,  A5 A5 0 12 EXTUI,  A4 $C L L32R,  A8 A10 0 S8I,  MEMW,
  A8 A4 0 L32I.N,  A4 $10 L L32R,  A9 A4 0 L32I.N,  A9 A9 30 2 EXTUI,
  A9 A9 14 SLLI,  A4 A10 0 L32I.N,  A11 $38 L L32R,  A4 A4 A11 AND,  A4 A4 A9 OR,
  A4 A10 0 S32I.N,  A4 A7 15 SRLI,  A4 A4 7 SLLI,  A7 A7 8 7 EXTUI,
  A4 A4 0 8 EXTUI,  A4 A4 A7 OR,  A7 A8 26 SLLI,  A8 1 MOVI.N,  A8 A8 27 SLLI,
  A7 A7 A8 AND,  A4 A4 A7 OR,  A7 $3C L L32R,  A6 A6 A7 AND,  A4 A4 A6 OR,
  A4 A10 4 S32I.N,  A4 49 MOVI.N,  A4 A4 25 SLLI,  A5 A5 A4 OR,  A6 $40 L L32R,
  A4 A10 24 L32I,  A4 A4 A6 AND,  A5 A5 A4 OR,  A5 A10 24 S32I,  MEMW,
  A8 $5C L L32R,  A8 CALLX8,
  A11 0 MOVI.N,  A10 26 MOVI,  A8 $60 L L32R,  A8 CALLX8,
$1E1 L:
  A8 $64 L L32R,  A8 CALLX8,
$1E7 L:
  A2 7 $1F0 L BBCI,
  A8 $68 L L32R,  A8 CALLX8,
$1F0 L:
  A2 19 $1F9 L BBCI,
  A8 $6C L L32R,  A8 CALLX8,
$1F9 L:
  A2 8 $1FF L BBSI,
  $82 L J,
$1FF L:
  A8 $70 L L32R,  A8 CALLX8,
  $82 L J,
$208 L:
  A2 A3 0 L32I.N,  A2 A2 220 L32I,  A2 CALLX8,
  RETW.N,
$212 L:  END-CODE

CODE wdev_record_rx_linked_list   \ .iram1.47 $24
  $0 9 LITERALS
    SYMBOL g_osi_funcs_p  SYMBOL s_record_list  $3FF73088 T,  $3FF73090 T,
    $3FF7308C T,  $3FF73098 T,  SYMBOL wDevCtrl  $3FF732F4 T,  $3FF732FC T,
$24 L:
  A1 32 ENTRY,  A2 3 $90 L BGEUI,
  A8 $0 L L32R,  A8 A8 0 L32I.N,  A10 A8 $110 L32I,  A10 CALLX8,
  A8 A2 A2 ADDX4,  A8 A8 A2 ADDX2,  A2 $4 L L32R,  A8 A8 A2 ADDX4,  A2 $8 L L32R,
  A10 A8 0 S32I.N,  MEMW,  A2 A2 0 L32I.N,  A2 A8 4 S32I.N,  A2 $C L L32R,  MEMW,
  A2 A2 0 L32I.N,  A2 A8 8 S32I.N,  A2 $10 L L32R,  MEMW,  A2 A2 0 L32I.N,
  A2 A8 12 S32I.N,  A2 $14 L L32R,  MEMW,  A2 A2 0 L32I.N,  A2 A8 20 S32I.N,
  A2 $18 L L32R,  A9 A2 0 L32I.N,  A2 A2 4 L32I.N,  A2 A8 28 S32I.N,
  A2 A2 8 L32I.N,  A2 A8 32 S32I.N,  A2 $1C L L32R,  A9 A8 24 S32I.N,  MEMW,
  A2 A2 0 L32I.N,  A2 A8 36 S32I.N,  A2 $20 L L32R,  MEMW,  A2 A2 0 L32I,
  A2 A8 40 S32I,
$90 L:
  MEMW,  RETW.N,
$95 L:  END-CODE

CODE wdev_dump_rx_linked_list   \ .iram1.48 $20
  $0 8 LITERALS
    SYMBOL rodata_wlog_info.49  SYMBOL s_record_list  SYMBOL rodata_wlog_info.50
    SYMBOL rodata_wlog_info.51  SYMBOL wdev_record_rx_linked_list  SYMBOL wifi_log
    SYMBOL wifi_log  SYMBOL wifi_log
$20 L:
  A1 48 ENTRY,  A10 2 MOVI,  A8 $10 L L32R,  A8 CALLX8,
  A13 $0 L L32R,  A15 A3 MOV.N,  A14 A2 MOV.N,  A12 3 MOVI.N,  A11 32 MOVI,
  A10 1 MOVI.N,  A8 $14 L L32R,  A8 CALLX8,
  A2 $4 L L32R,  A3 132 MOVI,  A3 A2 A3 ADD.N,
$48 L:
  A12 3 MOVI.N,  A11 32 MOVI.N,  A8 A2 20 L32I.N,  A10 1 MOVI.N,  A8 A1 12 S32I.N,
  A8 A2 16 L32I.N,  A8 A1 8 S32I.N,  A8 A2 12 L32I.N,  A8 A1 4 S32I.N,
  A8 A2 8 L32I.N,  A8 A1 0 S32I.N,  A15 A2 4 L32I.N,  A14 A2 0 L32I.N,
  A13 $8 L L32R,  MEMW,  A8 $18 L L32R,  A8 CALLX8,
  A12 3 MOVI.N,  A11 32 MOVI.N,  A8 A2 40 L32I.N,  A10 1 MOVI.N,  A8 A1 8 S32I.N,
  A8 A2 36 L32I.N,  A8 A1 4 S32I.N,  A8 A2 32 L32I.N,  A8 A1 0 S32I.N,
  A15 A2 28 L32I.N,  A14 A2 24 L32I,  A2 A2 44 ADDI,  A13 $C L L32R,  MEMW,
  A8 $1C L L32R,  A8 CALLX8,
  A2 A3 $48 L BNE,
  RETW.N,
$99 L:  END-CODE

CODE wDev_AppendRxBlocks   \ .wifislprxiram.52 $54
  $0 21 LITERALS
    $186A1 T,  SYMBOL rodata.__FUNCTION__$1  SYMBOL rodata.str1.1
    SYMBOL rodata_wlog_error.53  $CE5 T,  $BFFFFFFF T,  $DFFFFFFF T,  $FF000FFF T,
    $DEADBEEF T,  SYMBOL wDevCtrl  SYMBOL g_intr_lock_mux  SYMBOL g_osi_funcs_p
    SYMBOL wifi_log  SYMBOL wdev_record_rx_linked_list
    SYMBOL hal_mac_rx_set_dscr_reload  SYMBOL wdev_record_rx_linked_list
    SYMBOL wdev_dump_rx_linked_list  SYMBOL hal_mac_rx_is_dscr_reload
    SYMBOL hal_mac_rx_read_rxdscrnext  SYMBOL hal_mac_rx_get_last_dscr
    SYMBOL hal_mac_rx_set_base
$54 L:
  A1 64 ENTRY,  A4 A2 MOV.N,  A2 $6C L BEQZ.N,
  A13 $14 L L32R,  A12 $18 L L32R,  A11 $1C L L32R,  A10 $20 L L32R,  $90 L J,
  2 ZEROS,
$6C L:
  A2 $10 L L32R,  A12 1 MOVI.N,  A11 A12 11 SLLI,  A10 6 MOVI,  A15 $4 L L32R,
  A14 $8 L L32R,  A13 $C L L32R,  A2 A1 0 S32I,  MEMW,  A8 $30 L L32R,  A8 CALLX8,
$8C L:
  $8C L J,
  1 ZEROS,
$90 L:
  MEMW,  A5 A4 0 L32I.N,  A8 -1 MOVI.N,  A8 A8 31 SLLI,  A5 A5 A8 OR,  MEMW,
  A5 A4 0 S32I.N,  MEMW,  A5 A4 0 L32I.N,  A5 A5 A13 AND,  MEMW,  A5 A4 0 S32I.N,
  MEMW,  A5 A4 0 L32I.N,  A5 A5 A12 AND,  MEMW,  A5 A4 0 S32I.N,  MEMW,
  A9 A4 0 L32I.N,  A9 A9 0 12 EXTUI,  A9 A9 12 SLLI,  MEMW,  A8 A4 0 L32I.N,
  A8 A8 A11 AND,  A8 A8 A9 OR,  A5 A4 4 L32I.N,  MEMW,  A8 A4 0 S32I.N,
  A10 A5 0 S32I.N,  MEMW,  A8 A4 0 L32I.N,  A8 A8 0 12 EXTUI,  A5 A5 A8 ADD.N,
  A10 A5 0 S32I.N,  A4 A4 8 L32I.N,  A4 $90 L BNEZ,
  A5 $24 L L32R,  A6 A5 0 L32I.N,  A6 $178 L BEQZ,
  A7 $2C L L32R,  A6 $28 L L32R,  A8 A7 0 L32I.N,  A10 A6 0 L32I.N,
  A8 A8 40 L32I.N,  MEMW,  A8 CALLX8,
  A11 A10 MOV.N,  A10 A4 MOV.N,  A11 A1 16 S32I.N,  MEMW,  A8 $34 L L32R,
  A8 CALLX8,
  A4 A5 4 L32I.N,  A2 A4 8 S32I.N,  MEMW,  A8 $38 L L32R,  A8 CALLX8,
  A10 1 MOVI.N,  A8 $3C L L32R,  A8 CALLX8,
  A4 A7 0 L32I.N,  A11 A1 16 L32I.N,  A4 A4 44 L32I.N,  A10 A6 0 L32I.N,  A4 CALLX8,
  A4 $0 L L32R,  $150 L J,
  3 ZEROS,
$13E L:
  A4 A4 -1 ADDI.N,  A4 $150 L BNEZ.N,
  A11 A3 MOV.N,  A10 A2 MOV.N,  A8 $40 L L32R,  A8 CALLX8,
  $159 L J,
  1 ZEROS,
$150 L:
  A8 $44 L L32R,  A8 CALLX8,
  A10 $13E L BNEZ,
$159 L:
  A2 $3FF MOVI,  A8 $48 L L32R,  A8 CALLX8,
  A2 A2 20 SLLI,  A10 A2 $185 L BNE,
  A8 $4C L L32R,  A8 CALLX8,
  A3 A10 $185 L BEQ,
  A10 A10 8 L32I.N,  A10 $185 L BEQZ.N,
  $17C L J,
$178 L:
  A2 A5 0 S32I.N,  A10 A2 MOV.N,
$17C L:
  MEMW,  A8 $50 L L32R,  A8 CALLX8,
$185 L:
  A3 A5 4 S32I.N,  MEMW,  RETW.N,
$18C L:  END-CODE

CODE wDev_DiscardFrame   \ .wifislpiram.24 $8
  $0 2 LITERALS
    SYMBOL wDevCtrl  SYMBOL wDev_AppendRxBlocks
$8 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A11 A2 MOV.N,  A12 A3 0 16 EXTUI,  A9 A2 8 L32I.N,
  A10 A8 0 L32I.N,  A9 A8 0 S32I.N,  A8 0 MOVI.N,  A8 A2 8 S32I.N,  MEMW,
  A8 $4 L L32R,  A8 CALLX8,
  RETW.N,
$28 L:  END-CODE

CODE wDev_IndicateFrame   \ .wifislprxiram.21 $4C
  $0 19 LITERALS
    SYMBOL wDevCtrl  SYMBOL pTxRx  $FF000FFF T,  SYMBOL esf_buf_alloc
    SYMBOL esf_buf_alloc  SYMBOL wifi_gpio_debug  SYMBOL wifi_gpio_debug
    SYMBOL wDev_DiscardFrame  SYMBOL wdev_csi_len_align
    SYMBOL wdev_csi_hw_bug_check  SYMBOL memcpy  SYMBOL memcpy  SYMBOL memcpy
    SYMBOL memcpy  SYMBOL memcpy  SYMBOL memcpy  SYMBOL wdev_csi_rx_process
    SYMBOL wDev_DiscardFrame  SYMBOL lmacRxDone
$4C L:
  A1 64 ENTRY,  A3 A3 0 8 EXTUI,  A6 A1 12 S32I.N,  A2 A2 0 8 EXTUI,
  A5 A5 0 16 EXTUI,  A3 A1 8 S32I.N,  A2 $A5 L BEQZ,
  A5 1 $68 L BEQI,
  MEMW,  $D6 L J,
$68 L:
  A3 $0 L L32R,  A7 $1F4 MOVI,  A6 A3 0 L32I.N,  MEMW,  A3 A6 0 L32I.N,
  A3 A3 12 12 EXTUI,  A7 A3 $8C L BGEU,
$7B L:
  A11 7 MOVI.N,  A3 $4 L L32R,  A10 0 MOVI.N,  A3 A3 0 L32I.N,  A12 A3 $3C4 L32I,
  $B4 L J,
  2 ZEROS,
$8C L:
  MEMW,  A12 A6 0 L32I.N,  A11 8 MOVI.N,  A12 A12 12 12 EXTUI,  A10 0 MOVI.N,
  A8 $C L L32R,  A8 CALLX8,
  A7 A10 MOV.N,  A10 $BC L BNEZ.N,
  $7B L J,
$A5 L:
  A3 $4 L L32R,  A11 7 MOVI.N,  A10 A2 MOV.N,  A3 A3 0 L32I.N,  A12 A3 $3C4 L32I,
  A12 A5 A12 MULL,
$B4 L:
  A8 $10 L L32R,  A8 CALLX8,
  A7 A10 MOV.N,
$BC L:
  A2 A2 2 ADDI.N,  A11 A2 A2 OR,  A10 11 MOVI,  A8 $14 L L32R,  A8 CALLX8,
  A7 $E5 L BNEZ.N,
  A11 A2 MOV.N,  A10 12 MOVI.N,  A8 $18 L L32R,  A8 CALLX8,
$D6 L:
  A11 A5 MOV.N,  A10 A4 MOV.N,  A8 $1C L L32R,  A8 CALLX8,
  $2E4 L J,
  2 ZEROS,
$E5 L:
  A2 $0 L L32R,  A3 A2 0 L32I.N,  A6 A3 4 L32I.N,  A10 A6 MOV.N,  A8 $20 L L32R,
  A8 CALLX8,
  A8 0 MOVI.N,  A2 A10 0 16 EXTUI,  A8 A1 0 S32I.N,  A2 A8 $154 L BEQ,
  A9 A10 0 16 EXTUI,  A9 A9 24 ADDI,  A12 A6 24 ADDI,  A9 A6 A9 ADD.N,
  A11 A9 MOV.N,  A10 A12 MOV.N,  A9 A1 20 S32I.N,  A12 A1 16 S32I.N,  MEMW,
  A8 $24 L L32R,  A8 CALLX8,
  A10 A1 0 S32I.N,  A9 A1 20 L32I.N,  A12 A1 16 L32I.N,  A10 1 $14A L BNEI,
  A11 A6 24 L8UI,  A10 A12 1 L8UI,  A11 A9 0 S8I,  MEMW,  A11 A12 2 L8UI,
  A10 A9 1 S8I,  MEMW,  A10 A12 3 L8UI,  A11 A9 2 S8I,  A10 A9 3 S8I,  MEMW,  MEMW,
  $154 L J,
$14A L:
  A8 A1 0 L32I.N,  A10 0 MOVI.N,  A9 A8 -2 ADDI,  A2 A10 A9 MOVEQZ,
$154 L:
  A10 1 MOVI.N,  A9 A7 4 L32I.N,  A11 A3 4 L32I.N,  A9 A9 4 L32I.N,
  A4 A3 $16E L BEQ,
  A11 A2 24 ADDI,  A11 A11 0 16 EXTUI,  A11 A1 4 S32I.N,  MEMW,  $1AE L J,
  1 ZEROS,
$16E L:
  A12 24 MOVI.N,  A10 A9 MOV.N,  A8 $28 L L32R,  A8 CALLX8,
  MEMW,  A12 A4 0 L32I.N,  A11 A2 24 ADDI,  A12 A12 12 12 EXTUI,  A12 A12 -24 ADDI,
  A12 A12 A2 SUB,  A3 A4 4 L32I,  A10 A10 24 ADDI,  A11 A3 A11 ADD,  A8 $2C L L32R,
  A8 CALLX8,
  MEMW,  A3 A4 0 L32I.N,  MEMW,  A3 A4 0 L32I.N,  A3 A3 12 12 EXTUI,  A3 A3 A2 SUB,
  A10 A7 4 L32I.N,  $23D L J,
  1 ZEROS,
$1AE L:
  A11 A3 4 L32I.N,  A10 $1EC L BEQZ.N,
  A12 24 MOVI.N,  A10 A9 MOV.N,  A8 $30 L L32R,  A8 CALLX8,
  A8 $4 L L32R,  A9 A10 24 ADDI,  A10 A9 MOV.N,  A13 A8 0 L32I.N,  A6 A3 4 L32I.N,
  A12 A13 $3C4 L32I,  A8 A1 4 L32I.N,  A12 A12 A8 SUB,  A11 A6 A8 ADD.N,
  A13 A1 16 S32I.N,  MEMW,  A8 $34 L L32R,  A8 CALLX8,
  A9 A10 MOV.N,  A13 A1 16 L32I.N,  A8 A1 4 L32I.N,  A10 A13 $3C4 L32I,
  A10 A10 A8 SUB,  $206 L J,
$1EC L:
  A8 $4 L L32R,  A10 A8 0 L32I.N,  A12 A10 $3C4 L32I,  A10 A9 MOV.N,  A8 $38 L L32R,
  A8 CALLX8,
  A9 A10 MOV.N,  A8 $4 L L32R,  A10 A8 0 L32I.N,  A10 A10 $3C4 L32I,
$206 L:
  A9 A9 A10 ADD.N,  A10 0 MOVI.N,  A3 A3 8 L32I.N,  A4 A3 $1AE L BNE,
  A10 A9 MOV.N,  MEMW,  A12 A4 0 L32I.N,  A12 A12 12 12 EXTUI,  A11 A4 4 L32I.N,
  A8 $3C L L32R,  A8 CALLX8,
  A11 A5 -1 ADDI.N,  A8 $4 L L32R,  A9 A8 0 L32I.N,  A9 A9 $3C4 L32I,
  A9 A9 A11 MUL16U,  A9 A9 A2 SUB,  MEMW,  A3 A4 0 L32I.N,  A3 A3 12 12 EXTUI,
  A3 A3 A9 ADD.N,  A10 A7 4 L32I.N,
$23D L:
  A3 A3 0 12 EXTUI,  A3 A3 12 SLLI,  MEMW,  A9 A10 0 L32I.N,  A11 $8 L L32R,
  A9 A9 A11 AND,  A3 A9 A3 OR,  MEMW,  A3 A10 0 S32I.N,  A3 $0 L L32R,
  A9 A3 48 L8UI,  A9 1 $264 L BNEI,
  A6 A6 0 L32I.N,  A6 $284 L BLTZ,
$264 L:
  A2 $284 L BEQZ.N,
  A11 A2 MOV.N,  A12 A7 16 L32I.N,  A13 A12 28 ADDI,  A6 A3 0 L32I.N,
  A14 A12 24 L32I.N,  A14 A14 0 12 EXTUI,  A10 A6 4 L32I.N,  A10 A10 24 ADDI,
  A15 A1 0 L32I.N,  MEMW,  A8 $40 L L32R,  A8 CALLX8,
$284 L:
  A10 A4 MOV.N,  A11 A5 MOV.N,  A8 $44 L L32R,  A8 CALLX8,
  A8 A1 12 L32I.N,  A4 A7 44 L32I.N,  A8 A4 4 S32I.N,  A2 A3 44 L8UI,  A2 A4 8 S8I,
  MEMW,  A2 A3 45 L8UI,  A3 $100 MOVI,  A2 A4 9 S8I,  MEMW,  A2 A4 0 L32I.N,
  A2 A2 0 12 EXTUI,  A5 1 $2B4 L BEQI,
  A3 128 MOVI,
$2B4 L:
  A8 A1 8 L32I.N,  A2 A2 A3 OR,  A2 A2 0 16 EXTUI,  A3 A4 0 L32I.N,
  A8 $2C6 L BNEZ.N,
  MEMW,  $2CB L J,
$2C6 L:
  A5 16 MOVI.N,  A2 A2 A5 OR,
$2CB L:
  A5 -1 MOVI.N,  A5 A5 12 SLLI,  A3 A3 A5 AND,  A3 A3 A2 OR,  A3 A4 0 S32I.N,
  A10 A7 A7 OR,  MEMW,  A8 $48 L L32R,  A8 CALLX8,
$2E4 L:
  RETW.N,
$2E6 L:  END-CODE

CODE wDev_IndicateAmpdu   \ .wifirxiram.23 $48
  $0 18 LITERALS
    SYMBOL wDevCtrl  SYMBOL wdev_csi_len_align  SYMBOL wdev_csi_hw_bug_check
    SYMBOL wDev_RxSecError  SYMBOL roundup2  SYMBOL esf_buf_alloc  SYMBOL memcpy
    SYMBOL memcpy  SYMBOL esf_buf_recycle  SYMBOL memcpy  SYMBOL roundup2
    SYMBOL memcpy  SYMBOL roundup2  SYMBOL wdev_csi_rx_process
    SYMBOL wDev_SnifferRxAmpdu  SYMBOL lmacRxDone  SYMBOL esf_buf_recycle
    SYMBOL wDev_DiscardFrame
$48 L:
  A1 64 ENTRY,  A2 A1 12 S32I.N,  A3 A3 0 16 EXTUI,  A5 0 MOVI.N,  A2 $0 L L32R,
  A2 A2 0 L32I.N,  A3 A1 16 S32I.N,  A3 A2 4 L32I.N,  A10 A3 MOV.N,
  A4 A1 20 S32I.N,  MEMW,  A8 $4 L L32R,  A8 CALLX8,
  A4 A10 0 16 EXTUI,  A8 A10 0 16 EXTUI,  A7 A8 24 ADDI,  A4 A1 0 S32I.N,
  A5 A1 4 S32I.N,  A4 A5 $7B L BNE,
  $2D2 L J,
$7B L:
  A6 A3 24 ADDI,  A4 A3 A7 ADD.N,  A11 A4 MOV.N,  A10 A6 MOV.N,  MEMW,
  A8 $8 L L32R,  A8 CALLX8,
  A10 A1 4 S32I.N,  A10 1 $B9 L BNEI,
  A9 A3 24 L8UI,  A10 A3 25 L8UI,  A9 A4 0 S8I,  MEMW,  A9 A3 26 L8UI,
  A10 A4 1 S8I,  MEMW,  A6 A3 27 L8UI,  A9 A4 2 S8I,  A6 A4 3 S8I,  MEMW,  MEMW,
  $2D2 L J,
$B9 L:
  A6 A1 4 L32I.N,  A4 A6 -2 ADDI,  A6 0 MOVI.N,  A8 A1 0 L32I.N,  A8 A6 A4 MOVEQZ,
  A6 24 MOVI.N,  A7 A6 A4 MOVEQZ,  A8 A1 0 S32I.N,  MEMW,  $2D2 L J,
$D2 L:
  A13 A2 4 L32I.N,  A9 $627 MOVI,  A13 A13 A7 ADD.N,  A12 A13 0 L32I.N,
  A5 A12 0 12 EXTUI,  A6 A5 -24 ADDI,  A4 A5 MOV.N,  A9 A6 $E9 L BGEU,
  $30D L J,
$E9 L:
  MEMW,  A9 A2 0 L32I.N,  A6 A7 4 ADDI.N,  A9 A9 12 12 EXTUI,  A6 A9 $FD L BGEU,
  A9 A13 4 L16UI,  $104 L J,
  1 ZEROS,
$FD L:
  A9 A2 8 L32I.N,  A9 A9 4 L32I.N,  A9 A9 0 L16UI,
$104 L:
  A9 14 $114 L BBCI,
  A9 7 MOVI.N,  A9 A9 28 SLLI,  A10 A3 0 L32I.N,  A10 A9 $114 L BNONE,
  A4 A5 -8 ADDI,
$114 L:
  A10 A13 3 L8UI,  A10 $15C L BEQZ,
  A13 A1 28 S32I.N,  MEMW,  A8 $C L L32R,  A8 CALLX8,
  A13 A1 28 L32I.N,  A10 $15C L BNEZ.N,
  A10 A4 A7 ADD.N,  MEMW,  A4 A2 0 L32I.N,  A4 A4 12 12 EXTUI,  A10 A4 $14E L BLTU,
  MEMW,  A4 A2 0 L32I.N,  A4 30 $141 L BBCI,
  $308 L J,
$141 L:
  MEMW,  A4 A2 0 L32I.N,  A4 A4 12 12 EXTUI,  A10 A10 A4 SUB,  A2 A2 8 L32I.N,
$14E L:
  A11 4 MOVI.N,  A8 $10 L L32R,  A8 CALLX8,
  A7 A10 MOV.N,  $2E4 L J,
  1 ZEROS,
$15C L:
  A12 A5 28 ADDI,  A12 A12 0 12 EXTUI,  A11 7 MOVI.N,  A10 0 MOVI.N,
  A13 A1 28 S32I.N,  MEMW,  A8 $14 L L32R,  A8 CALLX8,
  A5 A10 MOV.N,  A10 $30D L BEQZ,
  A12 24 MOVI.N,  A10 A10 4 L32I.N,  A11 A3 MOV.N,  A10 A10 4 L32I.N,
  A8 $18 L L32R,  A8 CALLX8,
  A13 A1 28 L32I.N,  A10 A5 4 L32I.N,  A12 A13 0 L8UI,  A11 A10 4 L32I.N,
  A10 A11 24 ADDI,  A14 A13 1 L8UI,  A12 A11 24 S8I,  MEMW,  A12 A13 2 L8UI,
  A14 A11 25 S8I,  MEMW,  A11 A13 3 L8UI,  A12 A10 2 S8I,  A11 A10 3 S8I,  MEMW,
  A11 A4 A7 ADD.N,  MEMW,  A10 A2 0 L32I.N,  A10 A10 12 12 EXTUI,
  A11 A10 $228 L BLTU,
  MEMW,  A11 A2 0 L32I.N,  A11 A11 12 12 EXTUI,  A13 A11 A7 SUB,  A11 A7 A11 SUB,
  A10 A5 4 L32I.N,  A7 A11 A4 ADD.N,  A12 A13 -4 ADDI,  A10 A10 4 L32I.N,
  A10 A10 28 ADDI,  A11 A2 4 L32I.N,  A11 A11 A6 ADD.N,  A13 A1 28 S32I.N,  MEMW,
  A8 $1C L L32R,  A8 CALLX8,
  MEMW,  A6 A2 0 L32I.N,  A6 A6 30 1 EXTUI,  A13 A1 28 L32I.N,  A6 1 $200 L BNEI,
  A7 $24A L BEQZ,
  A10 A5 MOV.N,  A8 $20 L L32R,  A8 CALLX8,
  $308 L J,
  1 ZEROS,
$200 L:
  A2 A2 8 L32I.N,  A7 $24A L BEQZ,
  A13 A13 24 ADDI,  A12 A7 MOV.N,  A10 A5 4 L32I.N,  A10 A10 4 L32I.N,
  A10 A10 A13 ADD.N,  A11 A2 4 L32I.N,  A8 $24 L L32R,  A8 CALLX8,
  A10 A7 MOV.N,  A11 4 MOVI.N,  A8 $28 L L32R,  A8 CALLX8,
  A7 A10 MOV.N,  $24A L J,
  1 ZEROS,
$228 L:
  A10 A5 4 L32I.N,  A12 A4 -4 ADDI,  A10 A10 4 L32I.N,  A10 A10 28 ADDI,
  A11 A2 4 L32I.N,  A11 A11 A6 ADD.N,  A8 $2C L L32R,  A8 CALLX8,
  A11 4 MOVI.N,  A10 A4 MOV.N,  A8 $30 L L32R,  A8 CALLX8,
  A7 A7 A10 ADD.N,  A6 0 MOVI.N,
$24A L:
  A8 A1 24 L32I.N,  A8 $272 L BEQZ.N,
  A8 A1 8 L32I.N,  A8 $272 L BEQZ.N,
  A14 A4 MOV.N,  A4 0 MOVI.N,  A10 $0 L L32R,  A10 A10 0 L32I.N,  A12 A5 16 L32I.N,
  A13 A12 28 ADDI,  A10 A10 4 L32I.N,  A10 A10 24 ADDI,  A15 A1 4 L32I,
  A11 A1 0 L32I.N,  A8 $34 L L32R,  A8 CALLX8,
  A4 A1 8 S32I.N,
$272 L:
  A12 16 MOVI.N,  A4 A5 44 L32I.N,  A8 A1 20 L32I.N,  A10 $0 L L32R,
  A8 A4 4 S32I.N,  A11 A10 44 L8UI,  A11 A4 8 S8I,  MEMW,  A11 A10 45 L8UI,
  A11 A4 9 S8I,  MEMW,  A11 A4 0 L32I.N,  A11 A11 A12 OR,  A11 A4 0 S32I.N,
  A4 A10 48 L8UI,  A4 1 $2AE L BNEI,
  A4 A3 0 L32I,  A4 $2AE L BGEZ,
  A10 A5 A5 OR,  MEMW,  A8 $38 L L32R,  A8 CALLX8,
$2AE L:
  A4 7 MOVI.N,  A10 A3 0 L32I.N,  A4 A4 28 SLLI,  A10 A4 $2C4 L BNONE,
  A10 A5 MOV.N,  A8 $3C L L32R,  A8 CALLX8,
  $2CC L J,
  1 ZEROS,
$2C4 L:
  A10 A5 MOV.N,  A8 $40 L L32R,  A8 CALLX8,
$2CC L:
  A6 $2E4 L BEQZ.N,
  $308 L J,
  1 ZEROS,
$2D2 L:
  A4 1 MOVI.N,  A4 A1 8 S32I.N,  A4 0 MOVI.N,  A5 A1 0 L32I.N,  A6 A1 8 L32I.N,
  A4 A6 A5 MOVNEZ,  A4 A4 0 8 EXTUI,  A4 A1 24 S32I.N,
$2E4 L:
  MEMW,  A4 A2 0 L32I.N,  A4 30 $2EF L BBSI,
  $D2 L J,
$2EF L:
  MEMW,  A4 A2 0 L32I.N,  A4 30 $308 L BBCI,
  A6 A7 24 ADDI,  MEMW,  A4 A2 0 L32I.N,  A4 A4 12 12 EXTUI,  A6 A4 $308 L BGEU,
  $D2 L J,
$308 L:
  MEMW,  A2 A2 0 L32I.N,
$30D L:
  A11 A1 16 L32I.N,  A10 A1 12 L32I.N,  MEMW,  A8 $44 L L32R,  A8 CALLX8,
  RETW.N,
$31C L:  END-CODE

CODE wDev_ProcessRxSucData   \ .wifislprxiram.27 $68
  $0 26 LITERALS
    SYMBOL wDevCtrl  SYMBOL g_lmac_cnt  SYMBOL wifi_sta_rx_probe_req
    SYMBOL g_osi_funcs_p  SYMBOL rodata.__FUNCTION__$3  SYMBOL rodata.str1.1
    SYMBOL rodata_wlog_error.28  $904 T,  SYMBOL wDev_GetNoiseFloor
    SYMBOL wdev_csi_len_align  SYMBOL wdev_csi_hw_bug_check
    SYMBOL wDev_SnifferRxData  SYMBOL wDev_SnifferRxData
    SYMBOL ic_interface_enabled  SYMBOL ic_interface_enabled
    SYMBOL wDev_ProcessRxData_NAN_Int~2C06  SYMBOL ic_interface_enabled
    SYMBOL wDev_ProcessRxData_NAN_Int~2C06  SYMBOL wDev_IndicateCtrlFrame
    SYMBOL ic_interface_enabled  SYMBOL ic_interface_enabled
    SYMBOL ic_interface_enabled  SYMBOL wDev_DiscardFrame  SYMBOL wifi_log
    SYMBOL wDev_IndicateAmpdu  SYMBOL wDev_IndicateFrame
$68 L:
  A1 64 ENTRY,  A9 $0 L L32R,  A3 A3 0 16 EXTUI,  A4 A1 20 S32I.N,  A4 A9 0 L32I,
  A5 A4 4 L32I.N,  A6 A9 46 L8UI,  A4 A5 0 L8UI,  A4 A4 A6 SUB,  A4 A5 0 S8I,  MEMW,
  A4 A9 45 L8UI,  A4 A4 4 SLLI,  A6 A9 44 L8UI,  A6 A6 0 4 EXTUI,  A4 A4 A6 OR,
  A4 A5 10 S8I,  MEMW,  A9 A1 28 S32I,  MEMW,  A8 $20 L L32R,  A8 CALLX8,
  A10 A5 8 S8I,  MEMW,  A10 A5 MOV.N,  MEMW,  A8 $24 L L32R,  A8 CALLX8,
  A6 A10 0 16 EXTUI,  A9 A1 28 L32I.N,  A6 $EF L BEQZ.N,
  A4 A9 0 L32I.N,  A7 A4 4 L32I.N,  A4 A10 0 16 EXTUI,  A4 A4 24 ADDI,
  A4 A7 A4 ADD.N,  A11 A4 MOV.N,  A12 A7 27 L8UI,  A10 A7 24 ADDI,  A12 A1 24 S32I,
  MEMW,  A8 $28 L L32R,  A8 CALLX8,
  A9 A1 28 L32I.N,  A12 A1 24 L32I.N,  A10 2 $F6 L BEQI,
  A10 $F8 L BNEZ.N,
  A12 A4 3 L8UI,  $F8 L J,
$EF L:
  A12 A5 27 L8UI,  $F8 L J,
  1 ZEROS,
$F6 L:
  A6 0 MOVI.N,
$F8 L:
  A7 0 MOVI.N,  A10 -245 MOVI,  A8 1 MOVI.N,  A4 A7 MOV.N,  A10 A12 A10 ADD.N,
  A4 A8 A12 MOVNEZ,  A7 A8 A10 MOVNEZ,  A4 A4 A7 AND,  A4 $138 L BEQZ.N,
  A4 A12 -65 ADDI,  A4 $119 L BEQZ.N,
  A4 A12 -98 ADDI,  A4 $35E L BNEZ,
$119 L:
  A4 A9 48 L8UI,  A4 $35E L BEQZ,
  A4 A9 52 L32I.N,  A4 3 $129 L BBSI,
  $35E L J,
  2 ZEROS,
$129 L:
  A11 A3 0 8 EXTUI,  A10 A2 MOV.N,  A8 $2C L L32R,  A8 CALLX8,
  $35E L J,
  1 ZEROS,
$138 L:
  A7 A5 4 L32I.N,  A10 A8 27 SLLI,  A7 A7 A10 AND,  A10 A4 MOV.N,  A6 A6 28 ADDI,
  A11 A9 48 L8UI,  A6 A5 A6 ADD.N,  A11 A11 -1 ADDI.N,  A10 A8 A11 MOVEQZ,
  A10 A10 0 8 EXTUI,  A13 A6 0 L16UI,  A10 $17A L BEQZ.N,
  A8 A4 A7 MOVNEZ,  A8 A8 0 8 EXTUI,  A8 $17A L BEQZ.N,
  A8 A5 0 L32I.N,  A8 $17A L BGEZ,
  A11 A3 0 8 EXTUI,  A10 A2 MOV.N,  A9 A1 28 S32I.N,  A13 A1 24 S32I.N,  MEMW,
  A8 $30 L L32R,  A8 CALLX8,
  A13 A1 24 L32I.N,  A9 A1 28 L32I.N,
$17A L:
  A8 4 MOVI.N,  A10 A6 1 L8UI,  A8 A10 A8 AND,  A8 A1 16 S32I.N,  A10 A5 0 L32I.N,
  A7 $1C2 L BEQZ.N,
  A6 3 MOVI.N,  A6 A6 28 SLLI,  A6 A10 A6 AND,  A8 1 MOVI.N,  A11 0 MOVI.N,
  A9 A9 48 L8UI,  A8 A11 A6 MOVNEZ,  A8 A8 0 8 EXTUI,  A6 A4 A4 OR,
  A9 1 $1AC L BNEI,
  A6 A9 A9 OR,  A8 A4 A10 MOVLTZ,  A6 A4 A10 MOVGEZ,
$1AC L:
  A10 $4 L L32R,  A9 A10 160 L32I,  A9 A9 1 ADDI.N,  A9 A10 160 S32I,
  A6 $371 L BNEZ,
  MEMW,  $31D L J,
  2 ZEROS,
$1C2 L:
  A8 7 MOVI.N,  A8 A8 28 SLLI,  A10 A8 $1CD L BANY,
  $3D4 L J,
$1CD L:
  A9 A13 0 4 EXTUI,  A9 4 $1D6 L BNEI,
  $28D L J,
$1D6 L:
  A9 8 $1DC L BNEI,
  $2ED L J,
$1DC L:
  A8 1 MOVI.N,  A9 $31F L BNEZ,
  A8 240 MOVI,  A8 A13 A8 AND,  A8 64 $228 L BNEI,
  A8 A5 3 L8UI,  A9 -49 MOVI,  A8 A8 A9 AND,  A9 32 MOVI.N,  A8 A8 A9 OR,
  A8 A5 3 S8I,  MEMW,  A10 A7 MOV.N,  MEMW,  A8 $34 L L32R,  A8 CALLX8,
  A10 $278 L BEQZ,
  A8 $8 L L32R,  A8 A8 0 L32I.N,  A8 $278 L BEQZ,
  A10 A6 MOV.N,  A12 A5 0 L8UI,  A12 A12 7 SEXT,  A11 A5 24 L32I.N,
  A11 A11 0 12 EXTUI,  A8 CALLX8,
  $278 L J,
  1 ZEROS,
$228 L:
  A8 128 $234 L BEQI,
  A9 208 MOVI,  A8 A9 $278 L BNE,
  $264 L J,
$234 L:
  A10 2 MOVI.N,  A8 $38 L L32R,  A8 CALLX8,
  A10 $278 L BEQZ.N,
  A11 A6 MOV.N,  A10 A5 MOV.N,  A8 $3C L L32R,  A8 CALLX8,
  A10 $278 L BNEZ.N,
  A6 A5 3 L8UI,  A8 -49 MOVI,  A6 A6 A8 AND,  A8 16 MOVI.N,  A6 A6 A8 OR,
  A6 A5 3 S8I,  MEMW,  MEMW,  $278 L J,
$264 L:
  A10 2 MOVI.N,  A8 $40 L L32R,  A8 CALLX8,
  A10 $278 L BEQZ.N,
  A11 A6 MOV.N,  A10 A5 MOV.N,  A8 $44 L L32R,  A8 CALLX8,
$278 L:
  A8 $4 L L32R,  A6 A8 168 L32I,  A6 A6 1 ADDI.N,  A6 A8 168 S32I,  A8 A4 MOV.N,
  A6 1 MOVI.N,  MEMW,  $3D8 L J,
$28D L:
  A8 $4 L L32R,  A6 A8 172 L32I,  A6 A6 1 ADDI.N,  A6 A8 172 S32I,  A6 240 MOVI,
  A6 A13 A6 AND,  A8 176 MOVI,  A6 A8 $2A7 L BNE,
  $3C8 L J,
$2A7 L:
  A8 A6 $2C5 L BLTU,
  A8 160 MOVI,  A6 A8 $2B3 L BNE,
  $3E4 L J,
$2B3 L:
  A6 224 MOVI,  A6 A13 A6 AND,  A6 128 $2BF L BNEI,
  $3E4 L J,
$2BF L:
  MEMW,  $2D8 L J,
$2C5 L:
  A6 224 MOVI,  A6 A13 A6 AND,  A10 192 MOVI,  A9 A4 MOV.N,  A8 A4 MOV.N,
  A6 A10 $2D8 L BNE,
  $3CC L J,
$2D8 L:
  A12 A3 MOV.N,  A11 A2 MOV.N,  A10 A13 MOV.N,  A8 $48 L L32R,  A8 CALLX8,
  A8 A10 MOV.N,  A9 A4 MOV.N,  $3CC L J,
  2 ZEROS,
$2ED L:
  A8 112 MOVI,  A13 A13 A8 AND,  A9 1 MOVI.N,  A13 64 $30C L BEQI,
  A8 $C L L32R,  A8 A8 0 L32I.N,  A10 A8 4 L32I.N,  A10 CALLX8,
  A9 A4 MOV.N,  A10 $30C L BNEZ.N,
  A9 A6 4 L8UI,  A9 A9 0 1 EXTUI,
$30C L:
  A8 $4 L L32R,  A6 A8 164 L32I,  A6 A6 1 ADDI.N,  A6 A8 164 S32I,  MEMW,  $3CA L J,
$31D L:
  A4 A6 MOV.N,
$31F L:
  A10 1 MOVI.N,  A8 A1 24 S32I.N,  MEMW,  A8 $4C L L32R,  A8 CALLX8,
  A8 A1 24 L32I.N,  A10 $335 L BEQZ.N,
  A6 A5 0 L32I.N,  A6 29 $371 L BBSI,
$335 L:
  A10 0 MOVI.N,  A8 A1 24 S32I.N,  MEMW,  A8 $50 L L32R,  A8 CALLX8,
  A8 A1 24 L32I.N,  A10 $34D L BEQZ,
  A6 A5 0 L32I,  A6 28 $371 L BBSI,
$34D L:
  A10 2 MOVI.N,  A8 A1 24 S32I.N,  MEMW,  A8 $54 L L32R,  A8 CALLX8,
  A8 A1 24 L32I.N,  A10 $36C L BNEZ.N,
$35E L:
  A11 A3 MOV.N,  A10 A2 MOV.N,  A8 $58 L L32R,  A8 CALLX8,
  $3F2 L J,
  1 ZEROS,
$36C L:
  A5 A5 0 L32I.N,  A5 30 $35E L BBCI,
$371 L:
  A8 $35E L BNEZ,
  MEMW,  A5 A2 0 L32I.N,  A5 A5 12 12 EXTUI,  A5 $3A2 L BNEZ.N,
  A12 1 MOVI.N,  A2 $1C L L32R,  A11 A12 11 SLLI,  A10 6 MOVI.N,  A15 $10 L L32R,
  A14 $14 L L32R,  A13 $18 L L32R,  A2 A1 0 S32I.N,  MEMW,  A8 $5C L L32R,
  A8 CALLX8,
$39C L:
  $39C L J,
  3 ZEROS,
$3A2 L:
  A7 $3B4 L BEQZ.N,
  A12 A1 20 L32I.N,  A11 A3 MOV.N,  A10 A2 MOV.N,  A8 $60 L L32R,  A8 CALLX8,
  $3F2 L J,
  1 ZEROS,
$3B4 L:
  A14 A1 20 L32I.N,  A13 A3 MOV.N,  A12 A2 MOV.N,  A11 A7 MOV.N,  A10 A4 MOV.N,
  A8 $64 L L32R,  A8 CALLX8,
  $3F2 L J,
  1 ZEROS,
$3C8 L:
  A9 A4 MOV.N,
$3CA L:
  A8 A4 MOV.N,
$3CC L:
  A6 A1 16 L32I.N,  A6 $31F L BNEZ,
  $3DF L J,
$3D4 L:
  A6 A4 MOV.N,  A8 1 MOVI.N,
$3D8 L:
  A9 A1 16 L32I.N,  A9 $31D L BEQZ,
  A9 A4 MOV.N,
$3DF L:
  A4 A9 MOV.N,  $31F L J,
$3E4 L:
  A9 A1 16 L32I.N,  A8 0 MOVI.N,  A6 1 MOVI.N,  A6 A8 A9 MOVNEZ,  A8 A4 MOV.N,
  $31D L J,
$3F2 L:
  RETW.N,
$3F4 L:  END-CODE

CODE wdevProcessRxSucDataAll   \ .wifislprxiram.29 $3C
  $0 15 LITERALS
    SYMBOL wDevCtrl  SYMBOL g_wdev_last_desc_reset  SYMBOL rodata.__FUNCTION__$2
    SYMBOL rodata.str1.1  SYMBOL rodata_wlog_error.30  $937 T,
    SYMBOL g_intr_lock_mux  SYMBOL g_osi_funcs_p  SYMBOL rodata_wlog_error.31
    $95A T,  SYMBOL hal_mac_rx_get_last_dscr  SYMBOL wifi_log
    SYMBOL wDev_ProcessRxSucData  SYMBOL hal_mac_rx_get_last_dscr  SYMBOL wifi_log
$3C L:
  A1 64 ENTRY,  A8 $28 L L32R,  A8 CALLX8,
  A3 $0 L L32R,  A7 A10 MOV.N,  A4 $4 L L32R,  A5 A3 0 L32I.N,  A3 A4 0 L8UI,
  A3 $64 L BEQZ.N,
  A10 $89 L BEQZ.N,
  A3 0 MOVI.N,  A3 A4 0 S8I,  MEMW,  MEMW,  $89 L J,
$64 L:
  A10 $89 L BNEZ.N,
  A2 $14 L L32R,  A12 1 MOVI.N,  A11 A12 11 SLLI,  A10 6 MOVI.N,  A15 $8 L L32R,
  A14 $C L L32R,  A13 $10 L L32R,  A2 A1 0 S32I.N,  MEMW,  A8 $2C L L32R,
  A8 CALLX8,
$84 L:
  $84 L J,
  2 ZEROS,
$89 L:
  A4 $1C L L32R,  A3 $18 L L32R,  A6 A4 0 L32I.N,  A10 A3 0 L32I.N,
  A6 A6 40 L32I.N,  A6 CALLX8,
  A11 A10 MOV.N,  A6 0 MOVI.N,  $122 L J,
$9F L:
  A8 A5 8 L32I.N,  MEMW,  A10 A5 0 L32I.N,  A8 A1 16 S32I.N,  A8 A6 1 ADDI.N,
  A6 A8 0 16 EXTUI,  A10 30 $D8 L BBCI,
  A10 A4 0 L32I.N,  A12 A10 44 L32I.N,  A10 A3 0 L32I,  MEMW,  A12 CALLX8,
  A11 A6 MOV.N,  A12 A2 MOV.N,  A10 A5 MOV.N,  A8 $30 L L32R,  A8 CALLX8,
  A8 A4 0 L32I.N,  A6 0 MOVI.N,  A8 A8 40 L32I,  A10 A3 0 L32I,  A8 CALLX8,
  A11 A10 MOV.N,
$D8 L:
  A5 A7 $125 L BEQ,
  A11 A1 20 S32I.N,  MEMW,  A8 $34 L L32R,  A8 CALLX8,
  A7 A10 MOV.N,  A10 1 MOVI.N,  A5 $4 L L32R,  A9 A5 0 L8UI,  A9 A9 A10 XOR,
  A9 A9 0 8 EXTUI,  A11 A1 20 L32I.N,  A9 $120 L BEQZ.N,
  A9 1 MOVI.N,  A7 $120 L BNEZ.N,
  A2 $24 L L32R,  A12 A9 MOV.N,  A11 A9 11 SLLI,  A10 6 MOVI.N,  A15 $8 L L32R,
  A14 $C L L32R,  A13 $20 L L32R,  A2 A1 0 S32I.N,  MEMW,  A8 $38 L L32R,
  A8 CALLX8,
$11C L:
  $11C L J,
  1 ZEROS,
$120 L:
  A5 A1 16 L32I.N,
$122 L:
  A5 $9F L BNEZ,
$125 L:
  A2 A4 0 L32I.N,  A10 A3 0 L32I.N,  A2 A2 44 L32I.N,  A2 CALLX8,
  RETW.N,
$130 L:  END-CODE

CODE wDev_Insert_KeyEntry   \ .text.wDev_Insert_KeyEntry $10
  $0 4 LITERALS
    SYMBOL wDevCtrl  SYMBOL memcpy  SYMBOL hal_crypto_set_key_entry
    SYMBOL hal_crypto_enable
$10 L:
  A1 64 ENTRY,  A8 A1 72 L8UI,  A9 A7 MOV.N,  A12 6 MOVI.N,  A11 A5 MOV.N,
  A3 A3 0 8 EXTUI,  A10 A1 3 ADDI.N,  A8 A1 20 S32I.N,  A9 A1 16 S32I.N,
  A7 A1 68 L8UI,  A2 A1 1 S8I,  A3 A1 0 S8I,  A4 A1 2 S8I,  MEMW,  MEMW,
  A8 $4 L L32R,  A8 CALLX8,
  A13 A1 MOV.N,  A10 A6 MOV.N,  A9 A1 16 L32I.N,  A11 A9 MOV.N,  A12 A1 64 L32I,
  A8 $8 L L32R,  A8 CALLX8,
  A8 A1 20 L32I.N,  A3 3 $6D L BGEUI,
  A6 3 $6D L BLTI,
  A3 A3 4 ADDI.N,  A9 $0 L L32R,  A3 A3 A9 ADDX4,  A9 1 MOVI.N,  A6 SSL,  A9 A9 SLL,
  A10 A3 12 L32I.N,  A10 A10 A9 OR,  A10 A3 12 S32I.N,
$6D L:
  A13 A8 MOV.N,  A12 A7 MOV.N,  A11 A1 1 L8UI,  A10 A1 0 L8UI,  MEMW,  A8 $C L L32R,
  A8 CALLX8,
  RETW.N,
$82 L:  END-CODE

CODE wDev_remove_KeyEntry   \ .text.wDev_remove_KeyEntry $8
  $0 2 LITERALS
    SYMBOL wDevCtrl  SYMBOL hal_crypto_clr_key_entry
$8 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A9 -2 MOVI.N,  A2 SSL,  A9 A9 A9 SRC,  A10 A2 MOV.N,
  A11 A8 28 L32I.N,  A11 A11 A9 AND,  A11 A8 28 S32I.N,  A11 A8 32 L32I.N,
  A11 A11 A9 AND,  A11 A8 32 S32I.N,  A11 A8 36 L32I.N,  A11 A11 A9 AND,
  A11 A8 36 S32I.N,  A11 A8 40 L32I.N,  A9 A11 A9 AND,  A9 A8 40 S32I.N,  MEMW,
  A8 $4 L L32R,  A8 CALLX8,
  RETW.N,
$3F L:  END-CODE

CODE wDev_remove_KeyEntry_all_cnx   \ .text.wDev_remove_KeyEntry_all_cnx $8
  $0 2 LITERALS
    SYMBOL wDevCtrl  SYMBOL wDev_remove_KeyEntry
$8 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A2 0 8 EXTUI,  A2 A2 A8 ADDX4,  A8 -1 MOVI.N,
  A3 A8 A3 XOR,  A2 A2 28 L32I.N,  A3 A3 A2 AND,  A2 0 MOVI,
$21 L:
  A3 A2 $2C L BBC,
  A10 A2 MOV.N,  A8 $4 L L32R,  A8 CALLX8,
$2C L:
  A2 A2 1 ADDI.N,  A8 25 MOVI.N,  A2 A8 $21 L BNE,
  RETW.N,
$35 L:  END-CODE

CODE wDev_Crypto_Disable   \ .text.wDev_Crypto_Disable $8
  $0 2 LITERALS
    SYMBOL wDevCtrl  SYMBOL hal_crypto_disable
$8 L:
  A1 32 ENTRY,  A9 $0 L L32R,  A10 A2 0 8 EXTUI,  A8 A10 4 ADDI.N,  A8 A8 A9 ADDX4,
  A9 0 MOVI.N,  A11 A8 12 L32I.N,  A9 A8 12 S32I.N,  MEMW,  A8 $4 L L32R,
  A8 CALLX8,
  RETW.N,
$27 L:  END-CODE

CODE wDevCheckBlockError   \ .wifislprxiram.56 $24
  $0 9 LITERALS
    $DEADBEEF T,  $FFC T,  SYMBOL rodata_wlog_error.54
    SYMBOL rodata.__FUNCTION__$0  SYMBOL rodata.str1.1
    SYMBOL rodata_wlog_error.55  $D78 T,  SYMBOL wifi_log  SYMBOL wifi_log
$24 L:
  A1 48 ENTRY,  A8 A2 44 L32I.N,  A8 A8 0 L32I.N,  A8 4 $9E L BBCI,
  A8 A2 4 L32I.N,  A12 $0 L L32R,  A13 $4 L L32R,  $9B L J,
$39 L:
  MEMW,  A9 A8 0 L32I.N,  A9 A9 0 12 EXTUI,  A14 A8 4 L32I.N,  A9 A14 A9 ADD.N,
  A11 A9 0 L32I.N,  A11 A12 $94 L BEQ,
  A15 A13 0 16 EXTUI,  MEMW,  A10 A8 0 L32I.N,  A10 A10 0 12 EXTUI,
  A10 A15 $94 L BEQ,
  A12 1 MOVI.N,  A11 A1 4 S32I.N,  A10 6 MOVI.N,  A11 32 MOVI.N,  A9 A1 0 S32I,
  A15 A14 0 L32I,  A13 $8 L L32R,  MEMW,  A8 $1C L L32R,  A8 CALLX8,
  A12 1 MOVI.N,  A11 A12 11 SLLI,  A2 $18 L L32R,  A10 6 MOVI.N,  A15 $C L L32R,
  A14 $10 L L32R,  A13 $14 L L32R,  A2 A1 0 S32I.N,  MEMW,  A8 $20 L L32R,
  A8 CALLX8,
$90 L:
  $90 L J,
  1 ZEROS,
$94 L:
  A9 A2 8 L32I.N,  A9 A8 $9E L BEQ,
  A8 A8 8 L32I.N,
$9B L:
  A8 $39 L BNEZ,
$9E L:
  RETW.N,
$A0 L:  END-CODE

CODE wdev_set_csi_rx_cb   \ .text.wdev_set_csi_rx_cb $8
  $0 2 LITERALS
    SYMBOL g_wdev_csi_rx  SYMBOL g_wdev_csi_rx_ctx
$8 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A8 0 S32I.N,  A2 0 MOVI.N,  A8 $4 L L32R,
  A3 A8 0 S32I.N,  MEMW,  RETW.N,
$1C L:  END-CODE

CODE wdev_is_mgmt_hwdecr_enabled   \ .text.wdev_is_mgmt_hwdecr_enabled $4
  $0 1 LITERALS
    SYMBOL hal_crypto_mgmt_rx_enabled
$4 L:
  A1 32 ENTRY,  A10 A2 0 8 EXTUI,  A8 $0 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  RETW.N,
$14 L:  END-CODE

LINK-SYMBOLS   \ the cells of other packages that wait for these names
END-PACKAGE  PREVIOUS   \ not in the search order: ALSO name gives its words (README.md)
