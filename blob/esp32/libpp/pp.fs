\ pp.fs - package PP-PP: pp.o of libpp.a (ESP32)
\
\ Made from pp.S by s2fs.py, not by hand. Each function of C is a
\ CODE word: its bytes are those of the compiler, at the same offsets of
\ its section (L gives the address of an offset); its literals are at the
\ start of the word, after a jump. The data come first, then the
\ functions, each after those that it calls. The global symbols are
\ PUBLIC, the static ones PRIVATE. lib/esp32/clink.fs explains the words.
\
\ 109 functions (108 global; 1 static without symbol, named after their
\ section), 58 data objects.
\ A function runs as C code with the windowed calls of the Xtensa: call
\ it with CALL-C ( x1 .. xn n xt -- x ), never by its name alone.
\
\ Names of other packages: a literal waits until its package comes
\ (LINK-SYMBOLS); a name outside the libraries until EXTERN gives it.
\ Uses:
\   PP-ESF-BUF  esf_buf_alloc esf_buf_recycle esf_buf_setdown
\               esf_buf_setup
\   PP-IF-HWCTRL  ic_get_ptk_alg ic_interface_enabled ic_obtain_key
\   PP-LMAC  GetAccess is_lmac_idle lmacAdjustTimestamp lmacConfMib
\            lmacDiscardAgedMSDU lmacGetTxFrame lmacIsIdle lmacMSDUAged
\            lmacProcessCollisions_task lmacProcessTxComplete
\            lmacProcessTxTimeout lmacRecycleMPDU lmacTxFrame
\   PP-PM  pm_allow_tx pm_is_waked pm_on_beacon_rx pm_on_data_rx
\          pm_on_data_tx pm_on_data_tx_done pm_set_beacon_duration
\          pm_sleep_for
\   PP-PM-COEX  pm_coex_reconnect_policy pm_get_null_max_tx_time
\   PP-PP-DEBUG  dbg_lmac_init g_lmac_cnt wifi_gpio_debug
\   PP-PP-TIMER  pp_timer_do_process
\   PP-RATE-CONTROL  RC_GetAckTime RC_GetBlockAckTime RC_GetCtsTime
\   PP-TRC  rcGetAmpduSched rcGetRate rcGetSched rcSetBarRate
\           rcUpdateRxDone rcUpdateTxDoneAmpdu2 rc_get_trc
\           rc_get_trc_by_index trc_isTxAmpduOperational trc_onPPTxDone
\           trc_tid_isTxAmpduOperational
\   PP-WDEV  wDevCheckBlockError wDevCtrl wDev_AppendRxBlocks
\            wdevProcessRxSucDataAll wdev_process_misc_pkt
\ Outside the libraries (../README.md):
\   chm_get_current_band config_get_wifi_task_stack_size
\   config_is_cache_tx_buf_enabled g_intr_lock_mux g_mesh_init_ps_type
\   g_mesh_is_root g_mesh_is_started g_offchan_packet_lifetime g_osi_funcs_p
\   g_wifi_mac_time_delta g_wifi_menuconfig memcmp memcpy memset roundup2
\   wifi_log
\ C names with more than 31 characters (Forth takes 31): the first 26,
\ ~ and 4 hex digits of their CRC-32 (../README.md):
\   ppRegisterTxDoneUserAction~1FF4 = ppRegisterTxDoneUserActionCallback
\   pp_register_michael_mic_fa~B450 = pp_register_michael_mic_failure_cb
\   rodata.pp_default_event_ha~8FBB = rodata.pp_default_event_handler.str1.1
\
\ Needs XTENSA (asm/xtensa.fs), ASM (lib/esp32/asm.fs) and CLINK
\ (lib/esp32/clink.fs) of ucf; the ESP32.

PACKAGE PP-PP  ALSO ASM  ALSO CLINK   \ lib/package.fs
EXPORTS   \ the PUBLIC words are symbols for the other packages

\ ==== Data =============================================================

PRIVATE
CREATE rodata.pp_default_event_ha~8FBB   \ .rodata.pp_default_event_handler.str1.1, 5 bytes
  ," pp.c"  0 C,
CREATE rodata.pp_create_task.str1.1   \ .rodata.pp_create_task.str1.1, 5 bytes
  ," wifi"  0 C,
CREATE rodata   \ .rodata, 108 bytes
  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,
  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,   \ 27 jumps into ppTask
CREATE rodata.CSWTCH$606   \ .rodata.CSWTCH$606, 4 bytes
  $08 C,  $10 C,  $0C C,  $14 C,
1 DATA: bss.current_ifidx$0   \ .bss.current_ifidx$0
CREATE rodata_wlog_error.75   \ .rodata_wlog_error.75, 7 bytes
  ," %s %u"  10 C,  0 C,
CREATE rodata_wlog_error.69   \ .rodata_wlog_error.69, 7 bytes
  ," %s %u"  10 C,  0 C,
CREATE rodata_wlog_warning.67   \ .rodata_wlog_warning.67, 27 bytes
  ," pp task q full: s=%d p=%x"  10 C,  0 C,
CREATE rodata_wlog_info.58   \ .rodata_wlog_info.58, 49 bytes
  ," wifi driver task: %x, prio:%d, stack:%d, core=%d"  0 C,
CREATE rodata_wlog_error.57   \ .rodata_wlog_error.57, 40 bytes
  ," create wifi task: failed to create task"  0 C,
CREATE rodata_wlog_error.56   \ .rodata_wlog_error.56, 39 bytes
  ," create wifi task: failed to create sem"  0 C,
CREATE rodata_wlog_error.55   \ .rodata_wlog_error.55, 41 bytes
  ," create wifi task: failed to create queue"  0 C,
CREATE rodata_wlog_error.54   \ .rodata_wlog_error.54, 41 bytes
  ," create wifi task: failed to create queue"  0 C,
CREATE rodata_wlog_warning.53   \ .rodata_wlog_warning.53, 33 bytes
  ," create wifi task: queue not null"  0 C,
CREATE rodata_wlog_warning.52   \ .rodata_wlog_warning.52, 32 bytes
  ," create wifi task: task not null"  0 C,
CREATE rodata_wlog_error.48   \ .rodata_wlog_error.48, 7 bytes
  ," %s %u"  10 C,  0 C,
CREATE rodata_wlog_error.45   \ .rodata_wlog_error.45, 7 bytes
  ," %s %u"  10 C,  0 C,
CREATE rodata_wlog_error.42   \ .rodata_wlog_error.42, 7 bytes
  ," %s %u"  10 C,  0 C,
CREATE rodata_wlog_error.39   \ .rodata_wlog_error.39, 7 bytes
  ," %s %u"  10 C,  0 C,
CREATE rodata_wlog_warning.31   \ .rodata_wlog_warning.31, 13 bytes
  ," bar eb fail"  10 C,  0 C,
CREATE rodata_wlog_error.30   \ .rodata_wlog_error.30, 31 bytes
  ," invalid micahel mic failure cb"  0 C,
CREATE rodata_wlog_warning.27   \ .rodata_wlog_warning.27, 26 bytes
  ," reject paintext fragment"  10 C,  0 C,
CREATE rodata_wlog_info.25   \ .rodata_wlog_info.25, 18 bytes
  ," clear rx fragment"  0 C,
PUBLIC
8 DATA-ALIGN
16 DATA: s_fragment   \ .bss.s_fragment
PRIVATE
CREATE rodata_wlog_info.21   \ .rodata_wlog_info.21, 17 bytes
  ," Ignore BAR free!"  0 C,
CREATE rodata_wlog_error.17   \ .rodata_wlog_error.17, 7 bytes
  ," %s %u"  10 C,  0 C,
CREATE rodata_wlog_warning.15   \ .rodata_wlog_warning.15, 10 bytes
  ," map fail"  10 C,  0 C,
CREATE rodata_wlog_error.14   \ .rodata_wlog_error.14, 7 bytes
  ," %s %u"  10 C,  0 C,
CREATE rodata_wlog_warning.13   \ .rodata_wlog_warning.13, 31 bytes
  ," lmac if%d stop, discard packet"  0 C,
CREATE rodata_wlog_error.10   \ .rodata_wlog_error.10, 16 bytes
  ," wifi assert: %d"  0 C,
CREATE rodata_wlog_error.8   \ .rodata_wlog_error.8, 7 bytes
  ," %s %u"  10 C,  0 C,
PUBLIC
1 DATA: CanDoFreqCal   \ .data.CanDoFreqCal
  $01  1 CanDoFreqCal INIT
20 DATA: DefFreqCalTimer   \ .bss.DefFreqCalTimer
2 DATA: LowestFreqOffsetInOneChk   \ .bss.LowestFreqOffsetInOneChk
2 DATA: HighestFreqOffsetInOneChk   \ .bss.HighestFreqOffsetInOneChk
2 DATA: NoiseTimerInterval   \ .data.NoiseTimerInterval
  $64  $00  2 NoiseTimerInterval INIT
1 DATA: pend_flag_periodic_cal   \ .bss.pend_flag_periodic_cal
2 DATA: CurSigIdxToBeUse   \ .bss.CurSigIdxToBeUse
2 DATA: CurFreeSigIdx   \ .bss.CurFreeSigIdx
400 DATA: SigInMacISR   \ .bss.SigInMacISR
4 DATA: xphyQueue   \ .dram1.4
1 DATA: pp_allow_cut_sevt   \ .bss.pp_allow_cut_sevt
1 DATA: pp_need_cut_rx   \ .bss.pp_need_cut_rx
144 DATA: bars   \ .dram1.3
4 DATA: pTxRx   \ .data.pTxRx
972 DATA: TxRxCxt   \ .data.TxRxCxt
  $00  $00  $00  $00  $40  $06  $00  $00  $00  $00  $00  $00  12 TxRxCxt $3C0 + INIT
4 DATA: s_wifi_queue   \ .bss.s_wifi_queue
PRIVATE
4 DATA: bss.s_michael_mic_failure_cb   \ .bss.s_michael_mic_failure_cb
4 DATA: bss.s_pp_task_create_sem   \ .bss.s_pp_task_create_sem
4 DATA: bss.s_pp_task_del_sem   \ .bss.s_pp_task_del_sem
4 DATA: bss.pp_task_hdl   \ .bss.pp_task_hdl
4 DATA: bss.g_config_func   \ .bss.g_config_func
4 DATA: bss.g_timer_func   \ .bss.g_timer_func
4 DATA: bss.g_net80211_tx_func   \ .bss.g_net80211_tx_func
4 DATA: bss.g_tx_done_cb_func   \ .bss.g_tx_done_cb_func
PUBLIC
30 DATA: pp_sig_cnt   \ .dram1.2
PRIVATE
CREATE rodata.str1.1   \ .rodata.str1.1, 8 bytes
  ," 4df78f2"  0 C,
PUBLIC
4 DATA: libpp_reversion_git   \ .data.libpp_reversion_git

\ ==== Code =============================================================

PRIVATE
CODE pp_delete_task_manually   \ .text.pp_delete_task_manually $C, static, without symbol
  $0 3 LITERALS
    SYMBOL bss.pp_task_hdl  SYMBOL g_osi_funcs_p  SYMBOL s_wifi_queue
$C L:
  A1 32 ENTRY,  A2 $0 L L32R,  A10 A2 0 L32I.N,  A10 $25 L BEQZ.N,
  A8 $4 L L32R,  A8 A8 0 L32I.N,  A8 A8 152 L32I,  A8 CALLX8,
  A8 0 MOVI.N,  A8 A2 0 S32I.N,
$25 L:
  A2 $8 L L32R,  A10 A2 0 L32I.N,  A10 $3E L BEQZ.N,
  A8 $4 L L32R,  A8 A8 0 L32I.N,  A8 A8 $180 L32I,  MEMW,  A8 CALLX8,
  A8 0 MOVI.N,  A8 A2 0 S32I.N,
$3E L:
  A2 0 MOVI.N,  MEMW,  RETW.N,
$45 L:  END-CODE

PUBLIC
CODE pp_hdrsize   \ .wifislpiram.5 $0
  $0 ORIGIN
  A1 32 ENTRY,  A9 A2 1 L8UI,  A10 A2 MOV.N,  A9 A9 0 2 EXTUI,  A9 A9 -3 ADDI,
  A8 24 MOVI.N,  A2 30 MOVI.N,  A2 A8 A9 MOVNEZ,  A8 A10 0 L8UI,  A9 140 MOVI,
  A8 A8 A9 AND,  A9 136 MOVI,  A8 A9 $26 L BNE,
  A2 A2 2 ADDI.N,
$26 L:
  RETW.N,
$28 L:  END-CODE

CODE getle32   \ .text.getle32 $0
  $0 ORIGIN
  A1 32 ENTRY,  A9 A2 1 L8UI,  A9 A9 8 SLLI,  A8 A2 0 L8UI,  A9 A9 A8 OR,
  A8 A2 2 L8UI,  A8 A8 16 SLLI,  A8 A8 A9 OR,  A2 A2 3 L8UI,  A2 A2 24 SLLI,
  A2 A2 A8 OR,  RETW.N,
$23 L:  END-CODE

CODE putle32   \ .text.putle32 $0
  $0 ORIGIN
  A1 32 ENTRY,  A8 A3 8 SRLI,  A3 A2 0 S8I,  A8 A2 1 S8I,  A8 A3 16 16 EXTUI,
  A3 A3 24 8 EXTUI,  A8 A2 2 S8I,  A3 A2 3 S8I,  MEMW,  MEMW,  RETW.N,
$20 L:  END-CODE

CODE rotl   \ .text.rotl $0
  $0 ORIGIN
  A1 32 ENTRY,  A3 SSL,  A2 A2 A2 SRC,  RETW.N,
$B L:  END-CODE

CODE rotr   \ .text.rotr $0
  $0 ORIGIN
  A1 32 ENTRY,  A3 SSR,  A2 A2 A2 SRC,  RETW.N,
$B L:  END-CODE

CODE xswap   \ .text.xswap $8
  $0 2 LITERALS
    $FF00FF00 T,  $FF00FF T,
$8 L:
  A1 32 ENTRY,  A9 $0 L L32R,  A8 A2 8 SLLI,  A8 A8 A9 AND,  A2 A2 8 SRLI,
  A9 $4 L L32R,  A2 A2 A9 AND,  A2 A8 A2 OR,  RETW.N,
$22 L:  END-CODE

CODE pp_set_cut_rx   \ .text.pp_set_cut_rx $4
  $0 1 LITERALS
    SYMBOL pp_need_cut_rx
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A8 0 S8I,  MEMW,  MEMW,  RETW.N,
$15 L:  END-CODE

CODE pp_set_cut_evt   \ .text.pp_set_cut_evt $4
  $0 1 LITERALS
    SYMBOL pp_allow_cut_sevt
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A8 0 S8I,  MEMW,  MEMW,  RETW.N,
$15 L:  END-CODE

CODE pp_can_cut_sevt   \ .text.pp_can_cut_sevt $4
  $0 1 LITERALS
    SYMBOL pp_allow_cut_sevt
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A8 0 L8UI,  RETW.N,
$F L:  END-CODE

CODE pp_can_cut_evt   \ .text.pp_can_cut_evt $8
  $0 2 LITERALS
    SYMBOL pp_need_cut_rx  SYMBOL pp_allow_cut_sevt
$8 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A8 A2 0 L8UI,  A2 0 MOVI.N,  A8 A2 $1C L BEQ,
  A2 $4 L L32R,  A2 A2 0 L8UI,
$1C L:
  A2 A2 0 1 EXTUI,  RETW.N,
$21 L:  END-CODE

CODE RxNodeNum   \ .text.RxNodeNum $4
  $0 1 LITERALS
    SYMBOL wDevCtrl
$4 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A8 A2 0 L32I.N,  A2 0 MOVI.N,  A9 A2 MOV.N,  $26 L J,
  2 ZEROS,
$15 L:
  MEMW,  A10 A8 0 L32I.N,  A2 A2 1 ADDI.N,  A2 A2 0 8 EXTUI,  A10 30 $24 L BBCI,
  A2 A9 MOV.N,
$24 L:
  A8 A8 8 L32I.N,
$26 L:
  A8 $15 L BNEZ,
  RETW.N,
$2B L:  END-CODE

CODE TxNodeNum   \ .text.TxNodeNum $0
  $0 ORIGIN
  A1 32 ENTRY,  A2 0 MOVI.N,  RETW.N,
$7 L:  END-CODE

CODE SigSpaceMalloc   \ .text.SigSpaceMalloc $C
  $0 3 LITERALS
    SYMBOL pTxRx  SYMBOL CurFreeSigIdx  SYMBOL SigInMacISR
$C L:
  A1 32 ENTRY,  A2 $0 L L32R,  A10 50 MOVI.N,  A2 A2 0 L32I.N,  A8 A2 $3C0 L32I,
  A9 A8 1 ADDI.N,  A9 A2 $3C0 S32I,  A10 A9 $2C L BGEU,
  A8 A2 $3C0 S32I,  A2 0 MOVI.N,  MEMW,  $4D L J,
$2C L:
  A9 $4 L L32R,  A10 49 MOVI.N,  A8 A9 0 L16UI,  A2 $8 L L32R,  A2 A8 A2 ADDX8,
  A8 A8 1 ADDI.N,  A8 A8 0 16 EXTUI,  A10 A8 $45 L BLTU,
  $47 L J,
$45 L:
  A8 0 MOVI.N,
$47 L:
  A8 A9 0 S16I,  MEMW,
$4D L:
  MEMW,  RETW.N,
$52 L:  END-CODE

CODE SigSpaceFree   \ .text.SigSpaceFree $4
  $0 1 LITERALS
    SYMBOL pTxRx
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A9 A8 0 L32I.N,  A8 A9 $3C0 L32I,  A8 A8 -1 ADDI.N,
  A8 A9 $3C0 S32I,  MEMW,  RETW.N,
$19 L:  END-CODE

CODE DefFreqCalTimerCB   \ .text.DefFreqCalTimerCB $4
  $0 1 LITERALS
    SYMBOL CanDoFreqCal
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A9 1 MOVI.N,  A9 A8 0 S8I,  MEMW,  MEMW,  RETW.N,
$17 L:  END-CODE

CODE ppProcessRxPktHdr   \ .wifirxiram.6 $8
  $0 2 LITERALS
    SYMBOL pTxRx  SYMBOL g_osi_funcs_p
$8 L:
  A1 32 ENTRY,  A3 $0 L L32R,  A3 A3 0 L32I.N,  A3 A3 $3BC L32I,  A3 $1D L BEQZ.N,
  A11 A2 0 L32I.N,  A10 A2 4 L32I,  A3 CALLX8,
$1D L:
  A3 $4 L L32R,  A8 A3 0 L32I.N,  A10 A2 4 L32I.N,  A8 A8 176 L32I,  A8 CALLX8,
  A10 A2 MOV.N,  A3 A3 0 L32I.N,  A3 A3 176 L32I,  A3 CALLX8,
  RETW.N,
$36 L:  END-CODE

CODE pp_register_net80211_tx_cb   \ .text.pp_register_net80211_tx_cb $4
  $0 1 LITERALS
    SYMBOL bss.g_net80211_tx_func
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A8 0 S32I.N,  A2 1 MOVI.N,  MEMW,  RETW.N,
$13 L:  END-CODE

CODE pp_register_config_cb   \ .text.pp_register_config_cb $4
  $0 1 LITERALS
    SYMBOL bss.g_config_func
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A8 0 S32I.N,  A2 1 MOVI.N,  MEMW,  RETW.N,
$13 L:  END-CODE

CODE pp_register_timer_cb   \ .text.pp_register_timer_cb $4
  $0 1 LITERALS
    SYMBOL bss.g_timer_func
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A8 0 S32I.N,  A2 1 MOVI.N,  MEMW,  RETW.N,
$13 L:  END-CODE

CODE pp_register_michael_mic_fa~B450   \ pp_register_michael_mic_failure_cb: .text.pp_register_michael_mic_failure_cb $4
  $0 1 LITERALS
    SYMBOL bss.s_michael_mic_failure_cb
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A8 0 S32I.N,  A2 0 MOVI.N,  MEMW,  RETW.N,
$13 L:  END-CODE

CODE pp_process_hmac_waiting_txq   \ .wifi0iram.7 $8
  $0 2 LITERALS
    SYMBOL bss.g_net80211_tx_func  SYMBOL config_is_cache_tx_buf_enabled
$8 L:
  A1 32 ENTRY,  A8 $4 L L32R,  A8 CALLX8,
  A10 $1C L BEQZ.N,
  A8 $0 L L32R,  A10 A8 0 L32I,  A10 CALLX8,
$1C L:
  RETW.N,
$1E L:  END-CODE

CODE pp_default_event_handler   \ .text.pp_default_event_handler $C
  $0 3 LITERALS
    SYMBOL rodata.pp_default_event_ha~8FBB  SYMBOL rodata_wlog_error.8
    SYMBOL wifi_log
$C L:
  A1 32 ENTRY,  A12 1 MOVI.N,  A14 $0 L L32R,  A15 $178 MOVI,  A11 A12 11 SLLI,
  A10 6 MOVI.N,  A13 $4 L L32R,  A8 $8 L L32R,  A8 CALLX8,
$25 L:
  $25 L J,
$28 L:  END-CODE

CODE ppTxProtoProc   \ .wifi0iram.11 $0
  $0 ORIGIN
  A1 32 ENTRY,  A9 A2 44 L32I.N,  A2 A2 4 L32I.N,  A8 A9 0 L32I.N,  A13 A2 4 L32I,
  A12 A13 MOV.N,  A8 18 $14 L BBCI,
  A12 A13 4 ADDI,
$14 L:
  A2 A12 4 L8UI,  A2 0 $21 L BBCI,
  A2 2 MOVI.N,  A8 A8 A2 OR,  A8 A9 0 S32I.N,
$21 L:
  A11 12 MOVI.N,  A10 A12 0 L8UI,  A11 A10 A11 AND,  A8 A9 0 L32I.N,
  A11 8 $48 L BNEI,
  A2 112 MOVI,  A10 A10 A2 AND,  A10 64 $40 L BEQI,
  A8 A8 A11 OR,  MEMW,  $6F L J,
$40 L:
  A2 -9 MOVI.N,  A8 A8 A2 AND,  $6F L J,
$48 L:
  A11 $71 L BNEZ.N,
  A2 -16 MOVI.N,  A10 A10 A2 AND,  A2 80 MOVI.N,  A10 A2 $61 L BNE,
  A10 1 MOVI.N,  A10 A10 27 SLLI,  A8 1 $6C L BBCI,
  $71 L J,
  2 ZEROS,
$61 L:
  A10 64 $71 L BNEI,
  A8 1 $71 L BBSI,
  A10 1 MOVI.N,  A10 A10 11 SLLI,
$6C L:
  A8 A8 A10 OR,
$6F L:
  A8 A9 0 S32I.N,
$71 L:
  A2 A9 0 L32I.N,  A2 18 $82 L BBCI,
  A8 A12 22 L16UI,  A8 A8 4 SRLI,  A8 A13 3 S8I,  MEMW,
$82 L:
  MEMW,  RETW.N,
$87 L:  END-CODE

CODE ppAddTimCount   \ .text.ppAddTimCount $4
  $0 1 LITERALS
    SYMBOL pp_hdrsize
$4 L:
  A1 32 ENTRY,  A4 A2 12 L16UI,  A4 3 $10 L BLTUI,
  $18A L J,
$10 L:
  A8 A2 44 L32I.N,  A3 A2 4 L32I.N,  A8 A8 0 L32I.N,  A5 A3 4 L32I.N,
  A8 18 $1D L BBCI,
  A5 A5 4 ADDI.N,
$1D L:
  A10 A5 MOV.N,  A8 $0 L L32R,  A8 CALLX8,
  A9 A2 22 L16UI,  A9 A9 -4 ADDI,  A9 A9 A10 SUB,  A9 A9 0 16 EXTUI,  A9 A9 24 ADDI,
  A8 A2 16 L32I.N,  A9 A5 A9 ADD.N,  A9 A8 $48 L BLTU,
  MEMW,  A10 A3 0 L32I.N,  A10 A10 0 12 EXTUI,  A10 A8 A10 ADD.N,
  A9 A10 $4E L BLTU,
$48 L:
  A4 1 $4E L BNEI,
  $18A L J,
$4E L:
  A10 A5 36 ADDI,  A13 1 MOVI.N,  A12 0 MOVI.N,  $CA L J,
$58 L:
  A4 2 $91 L BNEI,
  A13 $91 L BEQZ.N,
  A10 A8 $6D L BLTU,
  MEMW,  A11 A3 0 L32I.N,  A11 A11 0 12 EXTUI,  A11 A8 A11 ADD.N,
  A10 A11 $91 L BLTU,
$6D L:
  MEMW,  A11 A3 0 L32I.N,  A11 A11 0 12 EXTUI,  A11 A8 A11 ADD.N,  A10 A10 A11 SUB,
  MEMW,  A11 A3 0 L32I.N,  A11 A11 0 12 EXTUI,  A11 A8 A11 ADD.N,  A9 A9 A11 SUB,
  A5 A2 8 L32I.N,  A13 A5 4 L32I.N,  A10 A13 A10 ADD.N,  A9 A13 A9 ADD.N,
  A13 A12 MOV.N,
$91 L:
  A5 A10 0 L8UI,  A14 A12 MOV.N,  A5 A5 -5 ADDI,  A14 A10 A5 MOVEQZ,
  A13 $B0 L BEQZ.N,
  A5 A10 1 ADDI.N,  A5 A8 $B6 L BLTU,
  MEMW,  A11 A3 0 L32I.N,  A11 A11 0 12 EXTUI,  A11 A8 A11 ADD.N,
  A5 A11 $B6 L BGEU,
$B0 L:
  A11 A10 1 L8UI,  $C3 L J,
$B6 L:
  A5 A2 8 L32I.N,  A5 A5 4 L32I.N,  MEMW,  A11 A5 0 L8UI,  A11 A11 0 8 EXTUI,
$C3 L:
  A14 $170 L BNEZ,
  A11 A11 2 ADDI.N,  A10 A10 A11 ADD.N,
$CA L:
  A5 A9 A10 SUB,  A5 2 $58 L BGEI,
  $18A L J,
$D3 L:
  A4 2 $144 L BNEI,
  A10 A8 $E9 L BLTU,
  MEMW,  A4 A3 0 L32I.N,  A4 A4 0 12 EXTUI,  A4 A8 A4 ADD.N,  A10 A4 $E9 L BGEU,
  $184 L J,
$E9 L:
  MEMW,  A4 A3 0 L32I.N,  A4 A4 0 12 EXTUI,  A4 A8 A4 ADD.N,  A4 A10 A4 SUB,
  A5 A2 8 L32I.N,  A10 A5 4 L32I.N,  A10 A10 A4 ADD.N,  $184 L J,
$FF L:
  MEMW,  A4 A3 0 L32I.N,  A4 A4 0 12 EXTUI,  A4 A8 A4 ADD.N,  A11 A4 $17C L BLTU,
$10C L:
  MEMW,  A4 A3 0 L32I.N,  A4 A4 0 12 EXTUI,  A4 A8 A4 ADD.N,  A4 A11 A4 SUB,
  A5 A2 8 L32I.N,  A11 A5 4 L32I.N,  A11 A11 A4 ADD.N,  $17C L J,
  2 ZEROS,
$124 L:
  MEMW,  A4 A3 0 L32I.N,  A4 A4 0 12 EXTUI,  A4 A8 A4 ADD.N,  A9 A4 $144 L BLTU,
$131 L:
  MEMW,  A3 A3 0 L32I.N,  A3 A3 0 12 EXTUI,  A3 A8 A3 ADD.N,  A3 A9 A3 SUB,
  A2 A2 8 L32I.N,  A9 A2 4 L32I.N,  A9 A9 A3 ADD.N,
$144 L:
  A8 A10 0 L8UI,  A8 $14C L BEQZ.N,
  $14F L J,
$14C L:
  A8 A11 0 L8UI,
$14F L:
  A8 A8 -1 ADDI.N,  A8 A8 0 8 EXTUI,  A8 A10 0 S8I,  MEMW,  A8 $18A L BNEZ.N,
  A3 1 MOVI.N,  A2 A9 0 L8UI,  A2 A2 A3 OR,  A2 A9 0 S8I,  MEMW,  MEMW,  $18A L J,
$170 L:
  A10 A14 2 ADDI.N,  A11 A14 3 ADDI.N,  A9 A14 4 ADDI.N,  A13 $D3 L BNEZ,
  $144 L J,
$17C L:
  A9 A8 $131 L BLTU,
  $124 L J,
  2 ZEROS,
$184 L:
  A11 A8 $10C L BLTU,
  $FF L J,
$18A L:
  RETW.N,
$18C L:  END-CODE

CODE ppRegisterPromisRxCallback   \ .text.ppRegisterPromisRxCallback $4
  $0 1 LITERALS
    SYMBOL pTxRx
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A8 A8 0 L32I.N,  A2 A8 $3BC S32I,  A2 0 MOVI.N,
  MEMW,  RETW.N,
$16 L:  END-CODE

CODE ppRegisterTxDoneUserAction~1FF4   \ ppRegisterTxDoneUserActionCallback: .text.ppRegisterTxDoneUserActionCallback $4
  $0 1 LITERALS
    SYMBOL bss.g_tx_done_cb_func
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A8 0 S32I.N,  A2 0 MOVI.N,  MEMW,  RETW.N,
$13 L:  END-CODE

CODE ppRegisterRxCallback   \ .text.ppRegisterRxCallback $4
  $0 1 LITERALS
    SYMBOL pTxRx
$4 L:
  A1 32 ENTRY,  A3 A3 0 8 EXTUI,  A8 1 MOVI.N,  A3 3 $1C L BGEUI,
  A8 $0 L L32R,  A8 A8 0 L32I.N,  A3 A3 A8 ADDX4,  A8 0 MOVI.N,  A2 A3 $3B0 S32I,
$1C L:
  A2 A8 MOV.N,  MEMW,  RETW.N,
$23 L:  END-CODE

CODE ppRegisterTxCallback   \ .text.ppRegisterTxCallback $4
  $0 1 LITERALS
    SYMBOL pTxRx
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A11 1 MOVI.N,  A12 A3 0 8 EXTUI,  A3 SSL,
  A3 A11 SLL,  A4 A4 0 8 EXTUI,  A8 A8 0 L32I.N,  A9 A11 MOV.N,  A10 A8 $35C L32I,
  A10 A3 $4C L BANY,
  A13 A8 $3C8 L32I,  A9 A3 A13 AND,  A9 $4A L BNEZ.N,
  A4 $38 L BEQZ.N,
  A3 A3 A13 OR,  A3 A8 $3C8 S32I,  MEMW,  $3E L J,
$38 L:
  A10 A10 A3 OR,  A10 A8 $35C S32I,
$3E L:
  A12 A12 A8 ADDX4,  A2 A12 $364 S32I,  MEMW,  $4C L J,
$4A L:
  A9 A11 MOV.N,
$4C L:
  A2 A9 MOV.N,  RETW.N,
$50 L:  END-CODE

CODE ppUnregisterTxCallback   \ .text.ppUnregisterTxCallback $4
  $0 1 LITERALS
    SYMBOL pTxRx
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A9 A2 0 8 EXTUI,  A10 A8 0 L32I.N,  A8 1 MOVI.N,
  A2 SSL,  A13 A8 SLL,  A2 A8 MOV.N,  A11 A10 $35C L32I,  A12 A10 $3C8 L32I,
  A14 A11 A12 OR,  A14 A13 $44 L BNONE,
  A8 -1 MOVI.N,  A2 A8 A13 XOR,  A11 A11 A2 AND,  A2 A2 A12 AND,  A2 A10 $3C8 S32I,
  A2 216 MOVI,  A9 A9 A2 ADD.N,  A9 A9 A10 ADDX4,  A2 0 MOVI,  A11 A10 $35C S32I,
  A2 A9 4 S32I,
$44 L:
  MEMW,  RETW.N,
$49 L:  END-CODE

CODE pp_register_tx_cb   \ .text.pp_register_tx_cb $4
  $0 1 LITERALS
    SYMBOL ppRegisterTxCallback
$4 L:
  A1 32 ENTRY,  A12 A4 0 8 EXTUI,  A11 A3 0 8 EXTUI,  A10 A2 MOV.N,  A8 $0 L L32R,
  A8 CALLX8,
  A2 A10 MOV.N,  RETW.N,
$19 L:  END-CODE

CODE pp_unregister_tx_cb   \ .text.pp_unregister_tx_cb $4
  $0 1 LITERALS
    SYMBOL ppUnregisterTxCallback
$4 L:
  A1 32 ENTRY,  A10 A2 0 8 EXTUI,  A8 $0 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  RETW.N,
$14 L:  END-CODE

CODE ppRecycleRxPkt   \ .wifiextrairam.16 $18
  $0 6 LITERALS
    SYMBOL pTxRx  SYMBOL rodata.pp_default_event_ha~8FBB
    SYMBOL rodata_wlog_error.17  SYMBOL wifi_log  SYMBOL wDev_AppendRxBlocks
    SYMBOL esf_buf_recycle
$18 L:
  A1 32 ENTRY,  A8 A2 4 L32I.N,  A9 A2 16 L32I.N,  A9 A8 4 S32I.N,  A10 A2 26 L8UI,
  A10 6 $61 L BNEI,
  A11 $0 L L32R,  A11 A11 0 L32I.N,  MEMW,  A9 A8 0 L32I.N,  A9 A9 0 12 EXTUI,
  A11 A11 $3C4 L32I,  A9 A11 $54 L BEQ,
  A12 1 MOVI.N,  A15 $451 MOVI,  A14 $4 L L32R,  A11 A12 11 SLLI,  A13 $8 L L32R,
  MEMW,  A8 $C L L32R,  A8 CALLX8,
$51 L:
  $51 L J,
$54 L:
  A12 A2 12 L16UI,  A10 A8 MOV.N,  A11 A2 8 L32I.N,  A8 $10 L L32R,  A8 CALLX8,
$61 L:
  A10 A2 MOV.N,  A8 $14 L L32R,  A8 CALLX8,
  RETW.N,
$6B L:  END-CODE

CODE ppRecycleTxBuf   \ .text.ppRecycleTxBuf $0
  $0 ORIGIN
  A1 32 ENTRY,  A2 1 MOVI.N,  RETW.N,
$7 L:  END-CODE

CODE ppCheckTxQIdle   \ .text.ppCheckTxQIdle $4
  $0 1 LITERALS
    SYMBOL lmacIsIdle
$4 L:
  A1 32 ENTRY,  A3 0 MOVI.N,  A2 A3 MOV.N,  A4 1 MOVI.N,
$D L:
  A10 A3 MOV.N,  A8 $0 L L32R,  A8 CALLX8,
  A3 A3 1 ADDI.N,  A2 A4 A10 MOVEQZ,  A3 5 $D L BNEI,
  RETW.N,
$1F L:  END-CODE

CODE ppCheckIsConnTraffic   \ .text.ppCheckIsConnTraffic $0
  $0 ORIGIN
  A1 32 ENTRY,  A10 A2 44 L32I.N,  A8 0 MOVI.N,  A9 A10 16 L32I.N,
  A9 A9 18 2 EXTUI,  A9 A8 $4A L BNE,
  A8 1 MOVI.N,  A10 A10 0 L32I.N,  A8 A8 21 SLLI,  A9 A10 A8 AND,  A8 1 MOVI.N,
  A9 $4A L BNEZ.N,
  A11 A2 36 L32I.N,  A8 A9 MOV.N,  A11 $4A L BEQZ.N,
  A9 A11 11 L8UI,  A9 7 $4A L BBSI,
  A8 1 MOVI.N,  A10 13 $4A L BBSI,
  A2 A2 4 L32I.N,  A8 A2 4 L32I,  A10 18 $38 L BBCI,
  A8 A8 4 ADDI.N,
$38 L:
  A2 12 MOVI.N,  A9 A8 0 L8UI,  A8 1 MOVI.N,  A9 A9 A2 AND,  A2 0 MOVI.N,
  A8 A2 A9 MOVNEZ,  A8 A8 0 8 EXTUI,
$4A L:
  A2 A8 MOV.N,  RETW.N,
$4E L:  END-CODE

CODE ppCheckTxConnTrafficIdle   \ .text.ppCheckTxConnTrafficIdle $1C
  $0 7 LITERALS
    SYMBOL pTxRx  SYMBOL lmacGetTxFrame  SYMBOL ppCheckIsConnTraffic
    SYMBOL ppCheckIsConnTraffic  SYMBOL ppCheckIsConnTraffic
    SYMBOL ppCheckIsConnTraffic  SYMBOL ppCheckIsConnTraffic
$1C L:
  A1 32 ENTRY,  A2 0 MOVI.N,
$21 L:
  A10 A2 MOV.N,  A8 $4 L L32R,  A8 CALLX8,
  A10 $34 L BEQZ.N,
  A8 $8 L L32R,  A8 CALLX8,
  A10 $E4 L BNEZ,
$34 L:
  A2 A2 1 ADDI.N,  A2 5 $21 L BNEI,
  A2 $0 L L32R,  A4 1 MOVI.N,  A5 -1 MOVI.N,  A3 A2 0 L32I.N,  A2 A3 0 L32I.N,
  $98 L J,
  1 ZEROS,
$48 L:
  A8 A2 NEG,  A8 A8 A2 AND,  A8 A8 NSAU,  A8 A8 NEG,  A6 A8 31 ADDI,
  A8 A6 A6 ADDX2,  A8 A8 4 SLLI,  A8 A3 A8 ADD.N,  A7 A8 32 L32I.N,  $71 L J,
$64 L:
  A10 A7 MOV.N,  A8 $C L L32R,  A8 CALLX8,
  A10 $E4 L BNEZ,
  A7 A7 40 L32I.N,
$71 L:
  A7 $64 L BNEZ,
  A9 A6 1 ADDI.N,  A9 A9 A9 ADDX2,  A9 A9 4 SLLI,  A9 A3 A9 ADD.N,  A10 A9 4 L32I.N,
  A10 $8C L BEQZ,
  A8 $10 L L32R,  A8 CALLX8,
  A10 $E4 L BNEZ,
$8C L:
  A6 SSL,  A8 A4 SLL,  A8 A5 A8 XOR,  A2 A2 A8 AND,
$98 L:
  A2 $48 L BNEZ,
  A4 $334 MOVI,  A6 $34C MOVI,  A4 A3 A4 ADD.N,  A6 A3 A6 ADD.N,
$A5 L:
  A5 A4 0 L32I.N,  $B6 L J,
$AA L:
  A10 A5 MOV.N,  A8 $14 L L32R,  A8 CALLX8,
  A10 $E4 L BNEZ.N,
  A5 A5 40 L32I.N,
$B6 L:
  A5 $AA L BNEZ,
  A4 A4 8 ADDI.N,  A6 A4 $A5 L BNE,
  A5 224 MOVI,  A4 A3 32 ADDI,  A3 A3 A5 ADD.N,
$C6 L:
  A5 A4 0 L32I.N,  $D8 L J,
  1 ZEROS,
$CC L:
  A10 A5 MOV.N,  A8 $18 L L32R,  A8 CALLX8,
  A10 $E4 L BNEZ.N,
  A5 A5 40 L32I.N,
$D8 L:
  A5 $CC L BNEZ,
  A4 A4 48 ADDI,  A4 A3 $C6 L BNE,
  $E6 L J,
$E4 L:
  A2 1 MOVI.N,
$E6 L:
  RETW.N,
$E8 L:  END-CODE

CODE pp_coex_tx_release   \ .wifiextrairam.18 $4
  $0 1 LITERALS
    SYMBOL g_osi_funcs_p
$4 L:
  A1 32 ENTRY,  A8 A2 4 L32I.N,  A10 12 MOVI.N,  A8 A8 4 L32I.N,  A9 A8 0 L8UI,
  A8 -16 MOVI.N,  A10 A9 A10 AND,  A8 A9 A8 AND,  A10 $44 L BNEZ.N,
  A8 128 $2A L BNEI,
  A8 $0 L L32R,  A10 4 MOVI.N,  A8 A8 0 L32I.N,  A8 A8 $1A0 L32I,  $5A L J,
$2A L:
  A10 $44 L BNEZ.N,
  A8 A8 96 ADDI,  A9 208 MOVI,  A8 A9 $5D L BANY,
  A8 $0 L L32R,  A10 7 MOVI.N,  A8 A8 0 L32I.N,  A8 A8 $1A0 L32I,  $5A L J,
  2 ZEROS,
$44 L:
  A10 8 $5D L BNEI,
  A8 112 MOVI,  A9 A9 A8 AND,  A9 64 $5D L BNEI,
  A8 $0 L L32R,  A10 9 MOVI.N,  A8 A8 0 L32I.N,  A8 A8 $1A0 L32I,
$5A L:
  A8 CALLX8,
$5D L:
  RETW.N,
$5F L:  END-CODE

CODE ppProcTxCallback   \ .wifi0iram.19 $4
  $0 1 LITERALS
    SYMBOL pTxRx
$4 L:
  A1 32 ENTRY,  A4 A2 44 L32I.N,  A3 A3 0 8 EXTUI,  A4 A4 20 L32I.N,  $50 L J,
$11 L:
  A8 A4 NEG,  A8 A8 A4 AND,  A5 $0 L L32R,  A8 A8 NSAU,  A8 A8 NEG,  A8 A8 31 ADDI,
  A9 A5 0 L32I.N,  A5 1 MOVI.N,  A8 SSL,  A5 A5 SLL,  A3 $35 L BEQZ.N,
  A10 A9 $3C8 L32I,  $38 L J,
$35 L:
  A10 A9 $35C L32I,
$38 L:
  A5 A10 $48 L BNONE,
  A8 A8 A9 ADDX4,  A8 A8 $364 L32I,  A8 $48 L BEQZ.N,
  A10 A2 MOV.N,  A8 CALLX8,
$48 L:
  A8 -1 MOVI.N,  A5 A8 A5 XOR,  A4 A4 A5 AND,
$50 L:
  A4 $11 L BNEZ,
  RETW.N,
$55 L:  END-CODE

CODE ppSetInterface   \ .text.ppSetInterface $0
  $0 ORIGIN
  A1 32 ENTRY,  RETW.N,
$5 L:  END-CODE

CODE pp_coex_tx_request   \ .wifi0iram.22 $10
  $0 4 LITERALS
    SYMBOL g_osi_funcs_p  SYMBOL pm_coex_reconnect_policy
    SYMBOL chm_get_current_band  SYMBOL pm_coex_reconnect_policy
$10 L:
  A1 48 ENTRY,  A3 A2 4 L32I.N,  A4 0 MOVI.N,  A8 -16 MOVI.N,  A3 A3 4 L32I.N,
  A4 A1 0 S32I.N,  A9 A3 0 L8UI,  A3 12 MOVI.N,  A3 A9 A3 AND,  A8 A9 A8 AND,
  A3 A4 $79 L BNE,
  A8 128 $4E L BNEI,
  A2 $0 L L32R,  A11 A1 MOV.N,  A10 4 MOVI.N,  A3 A2 0 L32I.N,  A3 A3 $1A8 L32I,
  MEMW,  A3 CALLX8,
  A11 A4 MOV.N,  A10 4 MOVI.N,  A2 A2 0 L32I.N,  A12 A1 0 L32I.N,  A2 A2 $19C L32I,
  $11F L J,
$4E L:
  A3 $79 L BNEZ.N,
  A10 A8 96 ADDI,  A4 208 MOVI,  A10 A4 $A4 L BANY,
  A2 $0 L L32R,  A11 A1 MOV.N,  A10 7 MOVI.N,  A4 A2 0 L32I.N,  A4 A4 $1A8 L32I,
  A4 CALLX8,
  A11 A3 MOV.N,  A10 7 MOVI.N,  A2 A2 0 L32I.N,  A12 A1 0 L32I.N,  A2 A2 $19C L32I,
  $11F L J,
  3 ZEROS,
$79 L:
  A3 8 $DA L BNEI,
  A10 112 MOVI,  A10 A9 A10 AND,  A10 64 $DA L BNEI,
  A2 $0 L L32R,  A11 A1 MOV.N,  A10 9 MOVI.N,  A3 A2 0 L32I.N,  A3 A3 $1A8 L32I,
  A3 CALLX8,
  A11 0 MOVI.N,  A10 9 MOVI.N,  A2 A2 0 L32I.N,  A12 A1 0 L32I.N,  A2 A2 $19C L32I,
  $11F L J,
  2 ZEROS,
$A4 L:
  A8 64 $DA L BNEI,
  A8 $4 L L32R,  A8 CALLX8,
  A10 $CC L BEQZ.N,
$AF L:
  A2 $0 L L32R,  A11 A1 MOV.N,  A10 39 MOVI.N,  A3 A2 0 L32I.N,  A3 A3 $1A8 L32I,
  A3 CALLX8,
  A11 0 MOVI.N,  A10 39 MOVI.N,  A2 A2 0 L32I.N,  A12 A1 0 L32I.N,  A2 A2 $19C L32I,
  $11F L J,
$CC L:
  A8 $8 L L32R,  A8 CALLX8,
  A10 2 $122 L BNEI,
  $AF L J,
  2 ZEROS,
$DA L:
  A2 A2 44 L32I.N,  A2 A2 0 L32I.N,  A2 2 $FC L BBSI,
  A3 $122 L BNEZ.N,
  A2 -48 MOVI,  A9 A9 A2 AND,  A10 1 MOVI.N,  A2 A3 MOV.N,  A2 A10 A9 MOVEQZ,
  A2 $FC L BNEZ.N,
  A2 -176 MOVI,  A8 A8 A2 ADD.N,  A10 A3 A8 MOVNEZ,  A10 $122 L BEQZ.N,
$FC L:
  A8 $C L L32R,  A8 CALLX8,
  A10 $122 L BEQZ.N,
  A3 $0 L L32R,  A11 A1 MOV.N,  A10 40 MOVI,  A2 A3 0 L32I.N,  A2 A2 $1A8 L32I,
  A2 CALLX8,
  A11 0 MOVI.N,  A10 40 MOVI.N,  A2 A3 0 L32I.N,  A12 A1 0 L32I.N,  A2 A2 $19C L32I,
$11F L:
  A2 CALLX8,
$122 L:
  RETW.N,
$124 L:  END-CODE

CODE esp_wifi_enable_easy_fragment   \ .text.esp_wifi_enable_easy_fragment $0
  $0 ORIGIN
  A1 32 ENTRY,  RETW.N,
$5 L:  END-CODE

CODE ppRxProtoProc   \ .wifislprxiram.24 $1C
  $0 7 LITERALS
    SYMBOL pm_on_data_rx  SYMBOL pp_hdrsize  SYMBOL pm_sleep_for
    SYMBOL pm_set_beacon_duration  SYMBOL pm_on_beacon_rx  SYMBOL rc_get_trc
    SYMBOL rcUpdateRxDone
$1C L:
  A1 48 ENTRY,  A4 A2 4 L32I.N,  A6 -16 MOVI.N,  A9 12 MOVI.N,  A5 A4 4 L32I.N,
  A4 A5 0 L8UI,  A8 A4 A6 AND,  A9 A4 A9 AND,  A4 1 MOVI.N,  A7 A3 0 L32I.N,
  A4 A4 29 SLLI,  A6 A7 A4 AND,  A7 28 $48 L BBCI,
  A4 3 MOVI.N,  A7 0 MOVI.N,  A4 A7 A6 MOVEQZ,  $59 L J,
  1 ZEROS,
$48 L:
  A4 1 MOVI.N,  A6 $59 L BNEZ.N,
  A4 A4 30 SLLI,  A7 A7 A4 AND,  A6 2 MOVI.N,  A4 3 MOVI.N,  A4 A6 A7 MOVNEZ,
$59 L:
  A9 8 $6F L BNEI,
  A13 A5 1 L8UI,  A12 A5 4 ADDI.N,  A11 A4 MOV.N,  A10 A5 10 ADDI,  A8 $0 L L32R,
  A8 CALLX8,
  $FC L J,
$6F L:
  A9 $FC L BNEZ,
  A8 128 $78 L BEQI,
  $FC L J,
$78 L:
  A6 A2 44 L32I.N,  A7 16 MOVI.N,  A6 A6 0 L32I.N,  A6 A6 A7 AND,  A6 $118 L BNEZ,
  A10 A5 MOV.N,  A8 $4 L L32R,  A8 CALLX8,
  A8 A3 24 L32I.N,  A7 A10 0 16 EXTUI,  A8 A8 0 12 EXTUI,  A7 A7 3 ADDI.N,
  A7 A8 $A2 L BGE,
  A8 A8 -4 ADDI,  A8 A8 A10 SUB,  A6 A8 0 16 EXTUI,
$A2 L:
  A10 A5 10 ADDI.N,  A8 $8 L L32R,  A8 CALLX8,
  A10 $FC L BEQZ,
  A7 A2 12 L16UI,  A7 1 $FC L BNEI,
  A9 31 MOVI.N,  A9 A9 8 SLLI,  A8 A3 0 L32I.N,  A11 A5 24 ADDI,  A8 A8 A9 AND,
  A8 A8 -2816 ADDMI,  A9 6 MOVI.N,  A10 A3 24 L32I.N,  A7 A9 A8 MOVEQZ,
  A10 A10 12 12 EXTUI,  A10 A10 3 SLLI,  A10 A10 A7 QUOU,  A11 A1 0 S32I.N,  MEMW,
  A8 $C L L32R,  A8 CALLX8,
  A13 0 MOVI.N,  A8 A3 24 L32I.N,  A8 A8 0 12 EXTUI,  A8 A8 3 SLLI,  A8 A8 A7 QUOU,
  A9 A2 44 L32I.N,  A11 A1 0 L32I.N,  A12 A11 A6 ADD.N,  A10 A9 4 L32I.N,
  A10 A10 A8 SUB,  A8 $10 L L32R,  A8 CALLX8,
$FC L:
  A4 3 $118 L BEQI,
  A11 A5 10 ADDI.N,  A10 A4 MOV.N,  A8 $14 L L32R,  A8 CALLX8,
  A10 A2 36 S32I.N,  A10 $118 L BEQZ.N,
  A11 A3 MOV.N,  MEMW,  A8 $18 L L32R,  A8 CALLX8,
$118 L:
  A2 0 MOVI.N,  RETW.N,
$11C L:  END-CODE

CODE pp_gettid   \ .text.pp_gettid $0
  $0 ORIGIN
  A1 32 ENTRY,  A9 A2 0 L8UI,  A8 140 MOVI,  A9 A9 A8 AND,  A10 136 MOVI,
  A8 16 MOVI.N,  A9 A10 $2A L BNE,
  A8 A2 1 L8UI,  A8 A8 0 2 EXTUI,  A8 3 $24 L BNEI,
  A8 A2 30 L8UI,  $27 L J,
  1 ZEROS,
$24 L:
  A8 A2 24 L8UI,
$27 L:
  A8 A8 0 4 EXTUI,
$2A L:
  A2 A8 MOV.N,  RETW.N,
$2E L:  END-CODE

CODE ppAssembleMicHdr   \ .text.ppAssembleMicHdr $14
  $0 5 LITERALS
    SYMBOL memcpy  SYMBOL memcpy  SYMBOL memcpy  SYMBOL memcpy  SYMBOL pp_gettid
$14 L:
  A1 32 ENTRY,  A8 A2 1 L8UI,  A4 A3 6 ADDI.N,  A8 A8 0 2 EXTUI,  A8 2 $40 L BEQI,
  A8 3 $54 L BEQI,
  A5 A2 10 ADDI.N,  A12 6 MOVI.N,  A11 A2 4 ADDI.N,  A8 1 $31 L BNEI,
  A11 A2 16 ADDI,
$31 L:
  A10 A3 MOV.N,  A8 $0 L L32R,  A8 CALLX8,
  A12 6 MOVI.N,  A11 A5 MOV.N,  $66 L J,
$40 L:
  A12 6 MOVI.N,  A11 A2 4 ADDI.N,  A10 A3 MOV.N,  A8 $4 L L32R,  A8 CALLX8,
  A12 6 MOVI.N,  A11 A2 16 ADDI,  $66 L J,
$54 L:
  A12 6 MOVI.N,  A11 A2 16 ADDI,  A10 A3 MOV.N,  A8 $8 L L32R,  A8 CALLX8,
  A12 6 MOVI.N,  A11 A2 24 ADDI,
$66 L:
  A10 A4 MOV.N,  A8 $C L L32R,  A8 CALLX8,
  A10 A2 MOV.N,  A8 $10 L L32R,  A8 CALLX8,
  A10 16 $7C L BNEI,
  A10 0 MOVI,
$7C L:
  A2 0 MOVI.N,  A10 A3 12 S8I,  A2 A3 13 S8I,  A2 A3 14 S8I,  A2 A3 15 S8I,  MEMW,
  MEMW,  RETW.N,
$92 L:  END-CODE

CODE ppCalTkipMic   \ .text.ppCalTkipMic $28
  $0 10 LITERALS
    SYMBOL s_fragment  SYMBOL roundup2  SYMBOL ppAssembleMicHdr  SYMBOL memcpy
    SYMBOL getle32  SYMBOL getle32  SYMBOL getle32  SYMBOL xswap  SYMBOL putle32
    SYMBOL putle32
$28 L:
  A1 48 ENTRY,  A12 A3 MOV.N,  A3 $0 L L32R,  A14 A4 0 16 EXTUI,  A7 A1 MOV.N,
  A11 4 MOVI.N,  A8 A3 4 L16UI,  A4 A8 A14 SUB,  A10 A4 13 ADDI.N,  A3 A4 8 ADDI.N,
  A14 A7 8 S32I.N,  A12 A7 4 S32I.N,  MEMW,  A8 $4 L L32R,  A8 CALLX8,
  A9 A10 15 ADDI.N,  A9 A9 4 SRLI,  A9 A9 4 SLLI,  A9 A1 A9 SUB,  A1 A9 MOVSP,
  A12 A7 4 L32I.N,  A13 A10 A3 SUB,  A13 A13 -1 ADDI.N,  A11 A1 MOV.N,
  A6 A10 MOV.N,  A9 A12 4 L32I.N,  A3 A1 A3 ADD.N,  A9 A9 4 L32I.N,  A10 A9 MOV.N,
  A13 A7 12 S32I.N,  A9 A7 4 S32I,  A1 A7 0 S32I,  MEMW,  A8 $8 L L32R,  A8 CALLX8,
  A12 A4 -8 ADDI,  A10 A1 16 ADDI,  A9 A7 4 L32I.N,  A14 A7 8 L32I.N,
  A11 A9 A14 ADD.N,  A8 $C L L32R,  A8 CALLX8,
  A9 90 MOVI.N,  A9 A3 0 S8I,  MEMW,  A3 0 MOVI.N,  A10 A3 MOV.N,  A13 A7 12 L32I.N,
  MEMW,  $B7 L J,
$A6 L:
  A8 A7 0 L32I.N,  A9 A8 A3 ADD.N,  A9 A9 A4 ADD.N,  A3 A3 1 ADDI.N,  A10 A9 9 S8I,
  MEMW,  A3 A3 0 8 EXTUI,
$B7 L:
  A3 A13 $A6 L BLTU,
  A10 A2 16 ADDI,  MEMW,  A8 $10 L L32R,  A8 CALLX8,
  A3 A10 MOV.N,  A10 A2 20 ADDI,  A8 $14 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  A8 0 MOVI.N,  $11F L J,
  1 ZEROS,
$D9 L:
  A4 A7 0 L32I.N,  A10 A4 A8 ADD.N,  A8 A7 4 S32I.N,  MEMW,  A8 $18 L L32R,
  A8 CALLX8,
  A3 A3 A10 XOR,  15 SSAI,  A4 A3 A3 SRC,  A2 A4 A2 XOR,  A3 A3 A2 ADD,
  A10 A3 A3 OR,  A8 $1C L L32R,  A8 CALLX8,
  A10 A2 A10 XOR,  A3 A3 A10 ADD.N,  29 SSAI,  A2 A3 A3 SRC,  A2 A2 A10 XOR,
  A3 A3 A2 ADD.N,  2 SSAI,  A9 A3 A3 SRC,  A8 A7 4 L32I.N,  A2 A9 A2 XOR,
  A3 A3 A2 ADD.N,  A8 A8 4 ADDI.N,
$11F L:
  A8 A6 $D9 L BLTU,
  A11 A3 MOV.N,  A10 A5 MOV.N,  A8 $20 L L32R,  A8 CALLX8,
  A11 A2 MOV.N,  A10 A5 4 ADDI.N,  A8 $24 L L32R,  A8 CALLX8,
  RETW.N,
$138 L:  END-CODE

CODE ppClearRxFragment   \ .text.ppClearRxFragment $10
  $0 4 LITERALS
    SYMBOL s_fragment  SYMBOL rodata_wlog_info.25  SYMBOL wifi_log
    SYMBOL ppRecycleRxPkt
$10 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A8 A2 0 L32I.N,  A8 $35 L BEQZ.N,
  A12 3 MOVI.N,  A11 32 MOVI.N,  A13 $4 L L32R,  A10 6 MOVI.N,  A8 $8 L L32R,
  A8 CALLX8,
  A10 A2 0 L32I.N,  A8 $C L L32R,  A8 CALLX8,
  A8 0 MOVI.N,  A8 A2 0 S32I.N,
$35 L:
  MEMW,  RETW.N,
$3A L:  END-CODE

CODE ppRxFragmentProc   \ .wifirxiram.26 $7C
  $0 31 LITERALS
    SYMBOL s_fragment  SYMBOL g_mesh_is_started  SYMBOL g_mesh_is_root
    SYMBOL g_mesh_init_ps_type  SYMBOL g_osi_funcs_p  $3FF73C00 T,
    SYMBOL g_wifi_mac_time_delta  SYMBOL lmacConfMib  SYMBOL g_lmac_cnt
    SYMBOL rodata_wlog_warning.27  SYMBOL pTxRx  $FF0000FF T,  $F000FFFF T,
    SYMBOL bss.s_michael_mic_failure_cb  SYMBOL ppRecycleRxPkt
    SYMBOL ic_get_ptk_alg  SYMBOL pp_hdrsize  SYMBOL wifi_log
    SYMBOL ppRecycleRxPkt  SYMBOL pp_gettid  SYMBOL pp_gettid  SYMBOL memcmp
    SYMBOL memcpy  SYMBOL ppRecycleRxPkt  SYMBOL memcpy  SYMBOL ic_obtain_key
    SYMBOL ppCalTkipMic  SYMBOL memcmp  SYMBOL ppRecycleRxPkt
    SYMBOL ppRecycleRxPkt  SYMBOL ppRecycleRxPkt
$7C L:
  A1 128 ENTRY,  A4 $0 L L32R,  A3 A4 0 L32I.N,  A3 $EC L BEQZ,
  A3 $4 L L32R,  A3 A3 0 L8UI,  A3 $B3 L BEQZ.N,
  A3 $8 L L32R,  MEMW,  A3 A3 0 L8UI,  A3 A3 0 8 EXTUI,  A3 $B3 L BNEZ.N,
  A3 $C L L32R,  A3 A3 0 L32I.N,  A3 $B3 L BEQZ.N,
  A3 $10 L L32R,  A3 A3 0 L32I,  A10 A3 $110 L32I,  A10 CALLX8,
  $C2 L J,
$B3 L:
  A3 $14 L L32R,  MEMW,  A10 A3 0 L32I.N,  A3 $18 L L32R,  A3 A3 0 L32I.N,
  A10 A10 A3 ADD.N,
$C2 L:
  A6 A4 0 L32I.N,  A5 $1C L L32R,  A3 A6 44 L32I.N,  A5 A5 16 L32I.N,
  A5 A5 10 SLLI,  A3 A3 4 L32I.N,  A3 A10 A3 SUB,  A5 A3 $EC L BGEU,
  A3 0 MOVI.N,  A10 A6 MOV.N,  A8 $38 L L32R,  A8 CALLX8,
  A3 A4 0 S32I.N,  A3 A4 4 S16I,  MEMW,  A3 A4 8 S32I.N,  A3 A4 12 S32I.N,
$EC L:
  A2 $F4 L BNEZ.N,
  MEMW,  $4BA L J,
$F4 L:
  A3 A2 4 L32I.N,  A4 0 MOVI.N,  A9 A3 4 L32I.N,  A3 A2 36 L32I.N,  A4 A1 56 S32I.N,
  A3 A4 $106 L BEQ,
  A3 A3 129 L8UI,  A3 A1 56 S32I.N,
$106 L:
  A4 4 MOVI.N,  A10 A1 56 L32I.N,  A9 A1 72 S32I,  MEMW,  A8 $3C L L32R,  A8 CALLX8,
  A6 A10 MOV.N,  A9 A1 72 L32I,  A3 A9 0 L8UI,  A3 A3 A4 AND,  A3 $4BC L BNEZ,
  A13 A3 MOV.N,  A7 A9 1 L8UI,  A4 A7 A4 AND,  A5 A9 22 L16UI,  A8 A5 0 4 EXTUI,
  A4 A1 48 S32I.N,  A4 1 MOVI.N,  A13 A4 A8 MOVNEZ,  A5 A1 52 S32I.N,
  A5 A1 48 L32I.N,  A4 A3 A5 MOVEQZ,  A13 A13 A4 OR,  A8 A1 60 S32I.N,
  A13 $4BC L BEQZ,
  A10 A9 MOV.N,  A4 $20 L L32R,  A3 A4 176 L16UI,  A3 A3 1 ADDI.N,  A3 A4 176 S16I,
  MEMW,  A5 A2 16 L32I.N,  A9 A1 72 S32I,  A13 A1 76 S32I,  MEMW,  A8 $40 L L32R,
  A8 CALLX8,
  A4 A10 0 16 EXTUI,  A3 A2 4 L32I.N,  A8 A3 4 L32I.N,  A3 64 MOVI.N,  A3 A7 A3 AND,
  A9 A1 72 L32I,  A13 A1 76 L32I,  A3 $239 L BEQZ,
  A7 A10 0 16 EXTUI,  A10 32 MOVI.N,  A3 A5 24 L32I.N,  A5 A9 A7 ADD.N,
  A3 A3 0 12 EXTUI,  A5 A5 3 L8UI,  A5 A5 A10 AND,  A5 $224 L BEQZ,
  A10 A8 A7 ADD.N,  A8 A10 7 L8UI,  A8 A8 8 SLLI,  A5 A10 4 L8UI,  A5 A5 16 SLLI,
  A7 A10 5 L8UI,  A7 A7 24 SLLI,  A11 A10 6 L8UI,  A8 A8 A11 OR,  A6 2 $1D0 L BNEI,
  A4 A4 8 ADDI.N,  A7 A5 A7 OR,  A11 A10 2 L8UI,  A4 A4 0 16 EXTUI,  A3 A3 -8 ADDI,
  A3 A3 A4 SUB,  A3 A3 0 16 EXTUI,  A5 A10 0 L8UI,  A7 A11 A7 OR,  A5 A5 8 SLLI,
  $21D L J,
$1D0 L:
  A6 4 $200 L BNEI,
  A5 A10 3 L8UI,  A4 A4 18 ADDI,  A5 A5 8 SLLI,  A4 A4 0 16 EXTUI,  A3 A3 -20 ADDI,
  A7 A10 2 L8UI,  A3 A3 A4 SUB,  A5 A5 A7 OR,  A3 A3 0 16 EXTUI,  A7 A10 4 L8UI,
  A7 A7 16 SLLI,  A7 A7 A5 OR,  A5 A10 5 L8UI,  A5 A5 24 SLLI,  $21D L J,
$200 L:
  A4 A4 8 ADDI.N,  A5 A5 A7 OR,  A11 A10 0 L8UI,  A4 A4 0 16 EXTUI,  A3 A3 -12 ADDI,
  A3 A3 A4 SUB,  A3 A3 0 16 EXTUI,  A7 A10 1 L8UI,  A5 A11 A5 OR,  A7 A7 8 SLLI,
$21D L:
  A5 A5 A7 OR,  $268 L J,
  1 ZEROS,
$224 L:
  A4 A4 4 ADDI.N,  A4 A4 0 16 EXTUI,  A3 A3 -8 ADDI,  A3 A3 A4 SUB,
  A3 A3 0 16 EXTUI,  A13 A5 MOV.N,  A8 A5 MOV.N,  $268 L J,
$239 L:
  A6 $255 L BNEZ.N,
  A3 A5 24 L32I.N,  A13 A6 MOV.N,  A3 A3 0 12 EXTUI,  A3 A3 -4 ADDI,  A3 A3 A4 SUB,
  A3 A3 0 16 EXTUI,  A5 A6 MOV.N,  A8 A6 MOV.N,  $268 L J,
  3 ZEROS,
$255 L:
  A13 $24 L L32R,  A12 2 MOVI.N,  A11 32 MOVI.N,  A10 6 MOVI.N,  A8 $44 L L32R,
  A8 CALLX8,
  $274 L J,
  1 ZEROS,
$268 L:
  A10 $0 L L32R,  A14 A10 0 L32I.N,  A14 $281 L BNEZ.N,
  A6 A1 60 L32I.N,  A6 $4A7 L BEQZ,
$274 L:
  A10 A2 MOV.N,  A8 $48 L L32R,  A8 CALLX8,
  $4BA L J,
  2 ZEROS,
$281 L:
  A7 A14 4 L32I.N,  A10 A9 MOV.N,  A7 A7 4 L32I.N,  A9 A1 72 S32I,  A8 A1 80 S32I,
  A13 A1 76 S32I,  A14 A1 68 S32I,  MEMW,  A8 $4C L L32R,  A8 CALLX8,
  A11 A10 MOV.N,  A10 A7 MOV.N,  A11 A1 64 S32I,  MEMW,  A8 $50 L L32R,  A8 CALLX8,
  A11 A1 64 L32I,  A9 A1 72 L32I,  A11 A10 $274 L BNE,
  A10 A9 4 ADDI.N,  A12 12 MOVI.N,  A11 A7 4 ADDI.N,  A8 $54 L L32R,  A8 CALLX8,
  A8 A1 80 L32I,  A9 A1 72 L32I,  A13 A1 76 L32I,  A14 A1 68 L32I,  A10 $274 L BNEZ,
  A12 A1 52 L32I.N,  A10 A7 22 L16UI,  A11 A10 A12 XOR,  A12 -16 MOVI.N,
  A11 A11 A12 AND,  A11 $464 L BNEZ,
  A12 A1 52 L32I.N,  A10 A10 1 ADDI.N,  A12 A10 $2EA L BEQ,
  $45C L J,
$2EA L:
  A13 $30C L BEQZ.N,
  A10 $0 L L32R,  A13 A10 8 L32I.N,  A12 A13 1 ADDI.N,  A15 A10 12 L32I.N,
  A10 1 MOVI,  A12 A13 $2FE L BLTU,
  A10 A11 A11 OR,
$2FE L:
  A10 A10 A15 ADD.N,  A12 A5 $306 L BEQ,
  $468 L J,
$306 L:
  A10 A8 $30C L BEQ,
  $468 L J,
$30C L:
  A11 $28 L L32R,  A13 $0 L L32R,  A11 A11 0 L32I.N,  A10 A13 4 L16UI,
  A12 A10 A3 ADD.N,  A11 A11 $3C4 L32I,  A11 A11 -28 ADDI,  A12 A11 $3A2 L BGEU,
  A11 A9 A4 ADD.N,  A14 A14 16 L32I.N,  A12 A3 A3 OR,  A10 A7 A10 ADD,
  A8 A1 80 S32I,  A13 A1 76 S32I,  A14 A1 68 S32I,  MEMW,  A8 $58 L L32R,
  A8 CALLX8,
  A11 -1 MOVI.N,  A11 A11 12 SLLI,  A13 A1 76 L32I,  A9 A13 4 L16UI,
  A9 A3 A9 ADD.N,  A14 A1 68 L32I,  A9 A13 4 S16I,  MEMW,  A9 A14 24 L32I.N,
  A10 A9 A3 ADD.N,  A10 A10 0 12 EXTUI,  A9 A9 A11 AND,  A9 A9 A10 OR,
  A10 A14 4 L32I.N,  A9 A14 24 S32I.N,  A9 A10 8 16 EXTUI,  A9 A3 A9 ADD.N,
  A11 A9 0 16 EXTUI,  A9 A11 8 SLLI,  A11 $2C L L32R,  A10 A10 A11 AND,
  A9 A10 A9 OR,  A9 A14 4 S32I.N,  A9 A14 0 L32I.N,  A10 A9 16 12 EXTUI,
  A3 A3 A10 ADD.N,  A10 A3 0 12 EXTUI,  A3 A10 16 SLLI,  A10 $30 L L32R,
  A9 A9 A10 AND,  A3 A9 A3 OR,  A3 A14 0 S32I.N,  A3 A1 52 L32I.N,  A8 A1 80 L32I,
  A3 A7 22 S16I,  MEMW,  A5 A13 8 S32I.N,  A8 A13 12 S32I.N,
$3A2 L:
  A10 A2 MOV.N,  MEMW,  A8 $5C L L32R,  A8 CALLX8,
  A5 A1 48 L32I.N,  A5 $4BA L BNEZ,
  A5 -16 MOVI.N,  A8 A1 52 L32I.N,  A5 A8 A5 AND,  A8 -5 MOVI.N,  A5 A7 22 S16I,
  MEMW,  A5 A7 1 L8UI,  A5 A5 A8 AND,  A3 $0 L L32R,  A2 A3 0 L32I.N,  A5 A7 1 S8I,
  MEMW,  A6 2 $44C L BNEI,
  A12 8 MOVI.N,  A10 A1 40 ADDI,  A5 A1 48 L32I.N,  A11 A3 4 L16UI,
  A11 A11 -8 ADDI,  A11 A11 0 16 EXTUI,  A5 A1 32 S32I.N,  A5 A1 36 S32I.N,
  A5 A2 4 L32I.N,  A5 A5 4 L32I,  A11 A5 A11 ADD,  MEMW,  A8 $60 L L32R,  A8 CALLX8,
  A15 32 MOVI.N,  A12 A1 48 L32I.N,  A14 A1 MOV.N,  A13 A5 10 ADDI.N,  A11 A6 MOV.N,
  A10 A1 56 L32I.N,  A8 $64 L L32R,  A8 CALLX8,
  A10 $470 L BEQZ,
  A12 A4 MOV.N,  A11 A2 MOV.N,  A10 A1 MOV.N,  A13 A1 32 ADDI,  A8 $68 L L32R,
  A8 CALLX8,
  A12 8 MOVI.N,  A11 A1 40 ADDI,  A10 A1 32 ADDI,  A8 $6C L L32R,  A8 CALLX8,
  A10 $44C L BEQZ.N,
  A2 $34 L L32R,  A10 1 MOVI.N,  A3 A2 0 L32I.N,  A6 A1 56 L32I.N,  A2 A1 48 L32I.N,
  A2 A10 A6 MOVEQZ,  A2 $470 L BEQZ.N,
  A2 A1 48 L32I.N,  A2 A10 A3 MOVNEZ,  A2 $470 L BEQZ.N,
  A3 CALLX8,
  $470 L J,
  1 ZEROS,
$44C L:
  A4 0 MOVI.N,  A4 A3 0 S32I.N,  A4 A3 4 S16I,  MEMW,  MEMW,  $489 L J,
$45C L:
  A8 A1 52 L32I.N,  A10 A8 $468 L BLT,
  $274 L J,
$464 L:
  A6 A1 60 L32I.N,  A6 $496 L BEQZ.N,
$468 L:
  A10 A2 MOV.N,  A8 $70 L L32R,  A8 CALLX8,
$470 L:
  A3 $0 L L32R,  A10 A3 0 L32I.N,  A10 $47D L BEQZ.N,
  A8 $74 L L32R,  A8 CALLX8,
$47D L:
  A2 0 MOVI.N,  A2 A3 0 S32I.N,  A4 A2 MOV.N,  A2 A3 4 S16I,  MEMW,
$489 L:
  A5 A4 MOV.N,  A4 A3 8 S32I.N,  A4 A3 12 S32I.N,  MEMW,  $4BC L J,
  1 ZEROS,
$496 L:
  A10 A14 MOV.N,  A8 A1 80 S32I,  MEMW,  A8 $78 L L32R,  A8 CALLX8,
  A8 A1 80 L32I,
$4A7 L:
  A4 A4 A3 ADD.N,  A6 $0 L L32R,  A2 A6 0 S32I.N,  A4 A6 4 S16I,  MEMW,
  A5 A6 8 S32I,  A8 A6 12 S32I,
$4BA L:
  A2 0 MOVI.N,
$4BC L:
  MEMW,  RETW.N,
$4C1 L:  END-CODE

CODE is_fragmented_pkt   \ .wifirxiram.28 $0
  $0 ORIGIN
  A1 32 ENTRY,  A2 A2 4 L32I.N,  A10 4 MOVI.N,  A9 A2 4 L32I.N,  A2 0 MOVI.N,
  A8 A9 0 L8UI,  A8 A10 $24 L BANY,
  A2 1 MOVI.N,  A8 A9 1 L8UI,  A8 A8 A10 AND,  A8 $24 L BNEZ.N,
  A9 A9 22 L16UI,  A9 A9 0 4 EXTUI,  A2 A8 A9 MOVEQZ,
$24 L:
  RETW.N,
$26 L:  END-CODE

CODE ppGetTxQFirstAvail_Locked   \ .text.ppGetTxQFirstAvail_Locked $4
  $0 1 LITERALS
    SYMBOL pTxRx
$4 L:
  A1 32 ENTRY,  A8 A2 0 8 EXTUI,  A9 15 MOVI.N,  A2 0 MOVI.N,  A9 A8 $3C L BLTU,
  A9 $0 L L32R,  A10 A9 0 L32I.N,  A9 A8 1 ADDI.N,  A9 A9 A9 ADDX2,  A9 A9 4 SLLI,
  A9 A10 A9 ADD.N,  A9 A9 0 L8UI,  A9 A2 $3C L BNE,
  A8 A8 A8 ADDX2,  A8 A8 4 SLLI,  A10 A10 A8 ADD.N,  A2 A10 32 L32I.N,
  A2 $3C L BEQZ.N,
  A8 A2 44 L32I.N,  A8 A8 0 L32I.N,  A8 A8 18 1 EXTUI,  A2 A9 A8 MOVNEZ,
$3C L:
  RETW.N,
$3E L:  END-CODE

CODE ppFetchTxQFirstAvail   \ .text.ppFetchTxQFirstAvail $4
  $0 1 LITERALS
    SYMBOL pTxRx
$4 L:
  A1 32 ENTRY,  A8 A2 0 8 EXTUI,  A9 15 MOVI.N,  A2 0 MOVI.N,  A9 A8 $58 L BLTU,
  A9 $0 L L32R,  A10 A9 0 L32I.N,  A9 A8 1 ADDI.N,  A9 A9 A9 ADDX2,  A9 A9 4 SLLI,
  A9 A10 A9 ADD.N,  A11 A9 0 L8UI,  A11 A2 $58 L BNE,
  A9 A8 A8 ADDX2,  A9 A9 4 SLLI,  A9 A10 A9 ADD.N,  A2 A9 32 L32I.N,
  A2 $58 L BEQZ.N,
  A12 A2 44 L32I.N,  A12 A12 0 L32I.N,  A12 18 $56 L BBSI,
  A11 A2 40 L32I.N,  A11 A9 32 S32I.N,  A11 $4C L BNEZ.N,
  A8 A8 A8 ADDX2,  A8 A8 4 SLLI,  A8 A8 32 ADDI,  A10 A10 A8 ADD.N,
  A10 A9 36 S32I.N,
$4C L:
  A8 0 MOVI.N,  A8 A2 40 S32I.N,  MEMW,  $58 L J,
$56 L:
  A2 A11 MOV.N,
$58 L:
  RETW.N,
$5A L:  END-CODE

CODE ppDequeueTxQ   \ .text.ppDequeueTxQ $4
  $0 1 LITERALS
    SYMBOL pTxRx
$4 L:
  A1 32 ENTRY,  A8 A2 0 8 EXTUI,  A9 15 MOVI.N,  A2 0 MOVI.N,  A9 A8 $39 L BLTU,
  A2 $0 L L32R,  A9 A8 A8 ADDX2,  A9 A9 4 SLLI,  A10 A2 0 L32I.N,  A9 A10 A9 ADD.N,
  A2 A9 32 L32I.N,  A2 $39 L BEQZ.N,
  A11 A2 40 L32I.N,  A11 A9 32 S32I.N,  A11 $35 L BNEZ.N,
  A8 A8 A8 ADDX2,  A8 A8 4 SLLI,  A8 A8 32 ADDI,  A10 A10 A8 ADD.N,
  A10 A9 36 S32I.N,
$35 L:
  A8 0 MOVI.N,  A8 A2 40 S32I.N,
$39 L:
  MEMW,  RETW.N,
$3E L:  END-CODE

CODE ppRollBackTxQ   \ .text.ppRollBackTxQ $4
  $0 1 LITERALS
    SYMBOL pTxRx
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A11 A8 0 L32I.N,  A8 A2 44 L32I.N,  A8 A8 16 L32I.N,
  A8 A8 20 4 EXTUI,  A9 A8 A8 ADDX2,  A9 A9 4 SLLI,  A9 A11 A9 ADD.N,
  A12 A9 32 L32I.N,  A10 A8 A8 ADD.N,  A12 A2 40 S32I.N,  A12 $28 L BNEZ.N,
  A12 A2 40 ADDI,  A12 A9 36 S32I.N,
$28 L:
  A8 A10 A8 ADD.N,  A8 A8 4 SLLI,  A11 A11 A8 ADD.N,  A2 A11 32 S32I.N,  MEMW,
  RETW.N,
$36 L:  END-CODE

CODE ppSetBarRate   \ .text.ppSetBarRate $8
  $0 2 LITERALS
    SYMBOL pTxRx  SYMBOL rcSetBarRate
$8 L:
  A1 32 ENTRY,  A4 $0 L L32R,  A2 A2 0 8 EXTUI,  A3 0 MOVI,
$14 L:
  A11 A2 MOV.N,  A8 A4 0 L32I.N,  A8 A3 A8 ADDX4,  A3 A3 1 ADDI.N,  A8 A8 $320 L32I,
  A10 A8 44 L32I.N,  A8 $4 L L32R,  A8 CALLX8,
  A3 4 $14 L BNEI,
  RETW.N,
$2D L:  END-CODE

CODE ppFillAMPDUBar   \ .text.ppFillAMPDUBar $18
  $0 6 LITERALS
    SYMBOL pTxRx  SYMBOL bars  $DFFFFFFF T,  $FF000FFF T,  SYMBOL memcpy
    SYMBOL memcpy
$18 L:
  A1 48 ENTRY,  A7 A2 0 8 EXTUI,  A11 A3 MOV.N,  A8 4 MOVI.N,  A3 A5 0 16 EXTUI,
  A7 8 $44 L BGEUI,
  A7 $42 L BEQZ.N,
  A7 3 $42 L BEQI,
  A8 3 MOVI.N,  A7 A8 $44 L BLTU,
  A8 1 MOVI.N,  A7 6 $39 L BLTUI,
  A8 0 MOVI.N,
$39 L:
  A8 A8 0 8 EXTUI,  $44 L J,
  3 ZEROS,
$42 L:
  A8 2 MOVI.N,
$44 L:
  A2 $0 L L32R,  A10 -16 MOVI.N,  A9 A7 0 4 EXTUI,  A13 -1 MOVI.N,  A13 A13 31 SLLI,
  A2 A2 0 L32I.N,  A3 A3 4 SLLI,  A2 A8 A2 ADDX4,  A2 A2 $320 L32I,
  A6 A2 44 L32I.N,  A5 A6 4 L32I.N,  A5 A5 A10 AND,  A5 A5 A9 OR,  A9 1 MOVI.N,
  A5 A6 4 S32I.N,  A9 A9 19 SLLI,  A5 A6 0 L32I.N,  A5 A5 A9 OR,  A5 A6 0 S32I.N,
  A5 A8 A8 ADDX8,  A5 A5 2 SLLI,  A10 A5 16 ADDI,  A9 $4 L L32R,  A10 A10 A9 ADD.N,
  A6 A2 4 L32I.N,  A10 A6 4 S32I.N,  MEMW,  A10 A6 0 L32I.N,  A10 A10 A13 OR,
  A13 1 MOVI.N,  A13 A13 30 SLLI,  A12 A2 22 L16UI,  A12 A12 0 12 EXTUI,
  A12 A12 12 SLLI,  MEMW,  A10 A6 0 S32I.N,  MEMW,  A10 A6 0 L32I.N,
  A10 A10 A13 OR,  MEMW,  A10 A6 0 S32I.N,  MEMW,  A10 A6 0 L32I.N,  A13 $8 L L32R,
  A10 A10 A13 AND,  MEMW,  A10 A6 0 S32I.N,  MEMW,  A10 A6 0 L32I.N,  A13 $C L L32R,
  A10 A10 A13 AND,  A10 A10 A12 OR,  A12 6 MOVI.N,  MEMW,  A10 A6 0 S32I.N,
  A6 A8 A8 ADDX8,  A6 A6 A9 ADDX4,  A10 -9 MOVI.N,  A8 A6 17 L8UI,  A8 A8 A10 AND,
  A10 A5 20 ADDI,  A10 A9 A10 ADD.N,  A8 A6 17 S8I,  MEMW,  A9 A1 0 S32I,  MEMW,
  A8 $10 L L32R,  A8 CALLX8,
  A10 A5 26 ADDI,  A11 A4 MOV.N,  A12 6 MOVI.N,  A9 A1 0 L32I.N,  A4 4 MOVI.N,
  A10 A9 A10 ADD,  A8 $14 L L32R,  A8 CALLX8,
  A8 A7 12 SLLI,  A8 A8 A4 OR,  A8 A6 32 S16I,  A3 A6 34 S16I,  MEMW,  MEMW,
  RETW.N,
$120 L:  END-CODE

CODE ppPauseTxAMPDU   \ .wifiextrairam.32 $4
  $0 1 LITERALS
    SYMBOL pTxRx
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A2 0 8 EXTUI,  A2 A2 A2 ADDX2,  A2 A2 4 SLLI,
  A8 A8 0 L32I.N,  A8 A8 A2 ADD.N,  A2 1 MOVI.N,  A2 A8 41 S8I,  MEMW,  MEMW,
  RETW.N,
$24 L:  END-CODE

CODE ppResumeTxAMPDU   \ .wifi0iram.33 $4
  $0 1 LITERALS
    SYMBOL pTxRx
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A2 0 8 EXTUI,  A2 A2 A2 ADDX2,  A2 A2 4 SLLI,
  A8 A8 0 L32I.N,  A8 A8 A2 ADD.N,  A2 0 MOVI.N,  A2 A8 40 S16I,  MEMW,  MEMW,
  RETW.N,
$24 L:  END-CODE

CODE ppCalSubFrameLength   \ .wifi0iram.34 $4
  $0 1 LITERALS
    SYMBOL roundup2
$4 L:
  A1 32 ENTRY,  A2 A2 4 L32I.N,  A11 4 MOVI.N,  A2 A2 4 L32I.N,  A10 A2 0 L32I.N,
  A10 A10 0 12 EXTUI,  A8 $0 L L32R,  A8 CALLX8,
  A2 A2 0 L32I.N,  A2 A2 12 7 EXTUI,  A2 A2 1 ADDI.N,  A2 A2 A10 ADDX4,
  A2 A2 0 16 EXTUI,  RETW.N,
$27 L:  END-CODE

CODE ppEmptyDelimiterLength   \ .wifi0iram.35 $0
  $0 ORIGIN
  A1 32 ENTRY,  A2 A2 4 L32I.N,  A2 A2 4 L32I.N,  A2 A2 0 L32I.N,  A8 A2 0 2 EXTUI,
  A8 $18 L BEQZ,
  A9 4 MOVI,  A8 A9 A8 SUB,  A8 A8 0 8 EXTUI,
$18 L:
  A2 A2 12 7 EXTUI,  A2 A2 A8 ADDX4,  RETW.N,
$20 L:  END-CODE

CODE ppCheckTxAMPDUlength   \ .text.ppCheckTxAMPDUlength $8
  $0 2 LITERALS
    SYMBOL pTxRx  SYMBOL roundup2
$8 L:
  A1 32 ENTRY,  A7 A5 4 L32I.N,  A11 4 MOVI,  A4 A4 0 8 EXTUI,  A7 A7 4 L32I,
  A10 A7 0 L32I,  A10 A10 0 12 EXTUI,  A8 $4 L L32R,  A8 CALLX8,
  A5 A5 44 L32I.N,  A8 A5 16 L32I.N,  A5 A10 0 16 EXTUI,  A3 A5 A3 ADD.N,
  A8 A8 20 4 EXTUI,  A5 A2 122 L16UI,  A5 A3 $3D L BLTU,
$34 L:
  A2 A2 124 L16UI,  A2 A3 $5D L BGEU,
  $55 L J,
$3D L:
  A5 $0 L L32R,  A11 A8 A8 ADDX2,  A11 A11 4 SLLI,  A9 A5 0 L32I.N,  A5 1 MOVI.N,
  A9 A9 A11 ADD.N,  A5 A9 76 S8I,  MEMW,  A6 $34 L BNEZ,
$55 L:
  A2 0 MOVI.N,  MEMW,  $86 L J,
$5D L:
  A2 $0 L L32R,  A8 A8 A8 ADDX2,  A8 A8 4 SLLI,  A3 A2 0 L32I.N,  A3 A3 A8 ADD.N,
  A2 A7 3 L8UI,  A2 A2 A4 SUB,  A2 A2 0 8 EXTUI,  A3 A3 47 L8UI,  A2 A3 $55 L BGEU,
  A2 A7 0 L32I.N,  A2 A2 12 7 EXTUI,  A2 A2 1 ADDI.N,  A2 A2 A10 ADDX4,
  A2 A2 0 16 EXTUI,
$86 L:
  RETW.N,
$88 L:  END-CODE

CODE ppAdd2AMPDUTail   \ .wifiextrairam.36 $0
  $0 ORIGIN
  A1 32 ENTRY,  A3 A2 40 S32I.N,  A8 A3 4 L32I.N,  A2 A2 8 L32I.N,  A8 A2 8 S32I.N,
  A2 0 MOVI.N,  A2 A3 40 S32I.N,  MEMW,  RETW.N,
$14 L:  END-CODE

CODE ppSubFromAMPDU   \ .wifi0iram.37 $0
  $0 ORIGIN
  A1 32 ENTRY,  A9 A3 8 L32I.N,  A8 A2 8 L32I.N,  A10 A9 8 L32I.N,  A10 A8 8 S32I.N,
  A8 0 MOVI.N,  A8 A9 8 S32I.N,  A9 A3 40 L32I.N,  A9 A2 40 S32I.N,
  A8 A3 40 S32I.N,  MEMW,  RETW.N,
$1A L:  END-CODE

CODE ppReSendBar   \ .text.ppReSendBar $34
  $0 13 LITERALS
    SYMBOL pTxRx  $FF0FFFFF T,  SYMBOL g_mesh_is_started  SYMBOL g_mesh_is_root
    SYMBOL g_mesh_init_ps_type  SYMBOL g_osi_funcs_p  $3FF73C00 T,
    SYMBOL g_wifi_mac_time_delta  SYMBOL bars  $DFFFFFFF T,  $FF000FFF T,
    SYMBOL rcGetRate  SYMBOL lmacTxFrame
$34 L:
  A1 32 ENTRY,  A2 A2 0 8 EXTUI,  A3 A3 0 8 EXTUI,  A4 4 MOVI.N,  A2 8 $5C L BGEUI,
  A2 $5A L BEQZ.N,
  A2 3 $5A L BEQI,
  A4 3 MOVI.N,  A2 A4 $5C L BLTU,
  A4 1 MOVI.N,  A2 6 $54 L BLTUI,
  A4 0 MOVI,
$54 L:
  A4 A4 0 8 EXTUI,  $5C L J,
$5A L:
  A4 2 MOVI.N,
$5C L:
  A2 $0 L L32R,  A3 A3 0 4 EXTUI,  A3 A3 20 SLLI,  A2 A2 0 L32I.N,  A2 A4 A2 ADDX4,
  A2 A2 $320 L32I,  A9 A2 44 L32I.N,  A8 A9 4 L8UI,  A10 $4 L L32R,  A8 A9 4 S32I.N,
  A8 A9 16 L32I.N,  A8 A8 A10 AND,  A8 A8 A3 OR,  A3 $8 L L32R,  A8 A9 16 S32I.N,
  A3 A3 0 L8UI,  A3 $AF L BEQZ.N,
  A3 $C L L32R,  MEMW,  A3 A3 0 L8UI,  A3 A3 0 8 EXTUI,  A3 $AF L BNEZ.N,
  A3 $10 L L32R,  A3 A3 0 L32I.N,  A3 $AF L BEQZ.N,
  A3 $14 L L32R,  A3 A3 0 L32I.N,  A10 A3 $110 L32I,  MEMW,  A10 CALLX8,
  $BE L J,
$AF L:
  A3 $18 L L32R,  MEMW,  A10 A3 0 L32I.N,  A3 $1C L L32R,  A3 A3 0 L32I.N,
  A10 A10 A3 ADD.N,
$BE L:
  A4 A4 A4 ADDX8,  A4 A4 2 SLLI,  A11 A2 44 L32I.N,  A4 A4 16 ADDI,
  A10 A11 24 S32I.N,  A10 A2 36 L32I.N,  MEMW,  A8 $2C L L32R,  A8 CALLX8,
  A10 A2 MOV.N,  A3 $20 L L32R,  A4 A4 A3 ADD.N,  A8 A2 4 L32I.N,  MEMW,
  A3 A8 0 L32I.N,  A4 A8 4 S32I.N,  A4 -1 MOVI.N,  A4 A4 31 SLLI,  A3 A3 A4 OR,
  A4 1 MOVI.N,  A9 A2 22 L16UI,  A4 A4 30 SLLI,  A9 A9 0 12 EXTUI,  A9 A9 12 SLLI,
  MEMW,  A3 A8 0 S32I.N,  MEMW,  A3 A8 0 L32I.N,  A3 A3 A4 OR,  MEMW,
  A3 A8 0 S32I.N,  MEMW,  A3 A8 0 L32I.N,  A4 $24 L L32R,  A3 A3 A4 AND,  MEMW,
  A3 A8 0 S32I.N,  MEMW,  A4 A8 0 L32I.N,  A3 $28 L L32R,  A4 A4 A3 AND,
  A4 A4 A9 OR,  MEMW,  A4 A8 0 S32I.N,  A3 A2 44 L32I.N,  A11 A3 4 L32I.N,
  A11 A11 4 4 EXTUI,  MEMW,  A8 $30 L L32R,  A8 CALLX8,
  RETW.N,
$143 L:  END-CODE

CODE ppRecordBarRRC   \ .text.ppRecordBarRRC $4
  $0 1 LITERALS
    SYMBOL pTxRx
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A2 0 8 EXTUI,  A2 A2 A2 ADDX2,  A2 A2 4 SLLI,
  A8 A8 0 L32I.N,  A8 A8 A2 ADD.N,  A3 A8 40 S8I,  MEMW,  MEMW,  RETW.N,
$22 L:  END-CODE

CODE ppTxqUpdateBitmap   \ .wifiextrairam.43 $4
  $0 1 LITERALS
    SYMBOL pTxRx
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A2 0 8 EXTUI,  A9 A8 0 L32I.N,  A8 A2 A2 ADDX2,
  A8 A8 4 SLLI,  A8 A9 A8 ADD.N,  A2 A8 40 L8UI,  A6 A6 A2 ADD.N,  A3 A8 42 S16I,
  A5 A8 56 S32I.N,  A4 A8 60 S32I.N,  A6 A8 40 S8I,  MEMW,  MEMW,  RETW.N,
$2E L:  END-CODE

CODE ppEnqueueTxDone   \ .wifiextrairam.47 $10
  $0 4 LITERALS
    SYMBOL rodata.pp_default_event_ha~8FBB  SYMBOL rodata_wlog_error.48
    SYMBOL pTxRx  SYMBOL wifi_log
$10 L:
  A1 32 ENTRY,  A2 $34 L BNEZ.N,
  A15 111 MOVI,  A12 1 MOVI,  A14 $0 L L32R,  A15 A15 5 SLLI,  A11 A12 11 SLLI,
  A10 6 MOVI,  A13 $4 L L32R,  A8 $C L L32R,  A8 CALLX8,
$30 L:
  $30 L J,
  1 ZEROS,
$34 L:
  A9 A2 44 L32I.N,  A10 1 MOVI.N,  A10 A10 26 SLLI,  A9 A9 0 L32I.N,  A9 A9 A10 AND,
  A8 $8 L L32R,  A8 A8 0 L32I.N,  A9 $58 L BEQZ.N,
  A2 A8 31 L8UI,  A2 A2 -1 ADDI.N,  A2 A8 31 S8I,  MEMW,  MEMW,  $65 L J,
$58 L:
  A9 A2 40 S32I.N,  A9 A8 $350 L32I,  A2 A9 0 S32I.N,  A2 A2 40 ADDI,
  A2 A8 $350 S32I,
$65 L:
  MEMW,  RETW.N,
$6A L:  END-CODE

CODE ppDequeueTxDone_Locked   \ .wifiextrairam.49 $4
  $0 1 LITERALS
    SYMBOL pTxRx
$4 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A8 A2 0 L32I.N,  A2 A8 $34C L32I,  A2 $24 L BEQZ.N,
  A9 A2 40 L32I.N,  A9 A8 $34C S32I,  A9 $20 L BNEZ.N,
  A9 $34C MOVI,  A9 A8 A9 ADD.N,  A9 A8 $350 S32I,
$20 L:
  A8 0 MOVI.N,  A8 A2 40 S32I.N,
$24 L:
  MEMW,  RETW.N,
$29 L:  END-CODE

CODE ppProcTxDone   \ .wifiextrairam.20 $38
  $0 14 LITERALS
    SYMBOL g_mesh_is_started  $80008 T,  SYMBOL bss.g_tx_done_cb_func
    $FF000FFF T,  $FF7FFFFF T,  SYMBOL rodata_wlog_info.21
    SYMBOL ppProcTxCallback  SYMBOL pp_coex_tx_release  SYMBOL trc_onPPTxDone
    SYMBOL wifi_log  SYMBOL esf_buf_recycle  SYMBOL ppDequeueTxDone_Locked
    SYMBOL pp_process_hmac_waiting_txq  SYMBOL pm_on_data_tx_done
$38 L:
  A1 48 ENTRY,  A2 0 MOVI.N,  A4 A2 MOV.N,  A2 A1 0 S32I.N,  A3 A2 MOV.N,
  A6 255 MOVI,  A5 1 MOVI.N,  MEMW,  $148 L J,
$4E L:
  A11 0 MOVI.N,  A10 A7 MOV.N,  A8 $18 L L32R,  A8 CALLX8,
  A10 A7 MOV.N,  A8 $1C L L32R,  A8 CALLX8,
  A11 A7 44 L32I.N,  A9 A11 0 L32I.N,  A9 13 $DB L BBCI,
  A10 A7 36 L32I.N,  A10 $B0 L BEQZ,
  A12 A10 11 L8UI,  A12 7 $B0 L BBSI,
  A12 A11 16 L32I.N,  A6 A12 18 2 EXTUI,  A6 $16C L BEQZ,
  $9E L J,
$7D L:
  A3 $0 L L32R,  A3 A3 0 L8UI,  A3 $9A L BEQZ.N,
  A4 15 MOVI.N,  A4 A4 8 SLLI,  A12 A12 A4 AND,  A11 A6 A12 MOVEQZ,
  A11 A1 0 S32I.N,  A4 A3 MOV.N,  MEMW,  $9E L J,
$9A L:
  A4 A5 MOV.N,  A3 A11 MOV.N,
$9E L:
  A11 $4 L L32R,  A9 A9 A11 AND,  A9 8 $B0 L BNEI,
  A11 A7 A7 OR,  A8 $20 L L32R,  A8 CALLX8,
$B0 L:
  A9 $8 L L32R,  A9 A9 0 L32I.N,  A9 $138 L BEQZ,
  A10 A7 44 L32I.N,  A11 A7 4 L32I.N,  A12 A10 0 L32I.N,  A11 A11 4 L32I.N,
  A12 18 $C6 L BBCI,
  A11 A11 4 ADDI,
$C6 L:
  A13 0 MOVI.N,  A12 A7 22 ADDI,  A10 A10 19 L8UI,  A10 A10 -1 ADDI.N,
  A13 A5 A10 MOVEQZ,  A10 A6 MOV.N,  A9 CALLX8,
  $138 L J,
$DB L:
  A9 23 $120 L BBCI,
  A10 A7 22 L16UI,  A10 A10 -4 ADDI,  A10 A7 22 S16I,  MEMW,  A10 A7 8 L32I.N,
  MEMW,  A8 A10 0 L32I.N,  A8 A8 12 12 EXTUI,  A8 A8 -4 ADDI,  A8 A8 0 12 EXTUI,
  MEMW,  A12 A10 0 L32I.N,  A8 A8 12 SLLI,  A13 $C L L32R,  A12 A12 A13 AND,
  A8 A12 A8 OR,  MEMW,  A8 A10 0 S32I.N,  A8 $10 L L32R,  A8 A9 A8 AND,
  A8 A11 0 S32I.N,  MEMW,  $148 L J,
  2 ZEROS,
$120 L:
  A9 A7 26 L8UI,  A9 5 $138 L BNEI,
  A12 3 MOVI.N,  A11 64 MOVI.N,  A13 $14 L L32R,  A10 6 MOVI.N,  A8 $24 L L32R,
  A8 CALLX8,
  $148 L J,
$138 L:
  A9 A7 26 L8UI,  A10 A7 MOV.N,  A9 A9 -1 ADDI.N,  A2 A5 A9 MOVEQZ,  A8 $28 L L32R,
  A8 CALLX8,
$148 L:
  A8 $2C L L32R,  A8 CALLX8,
  A7 A10 MOV.N,  A10 $4E L BNEZ,
  A2 $15C L BEQZ,
  A8 $30 L L32R,  A8 CALLX8,
$15C L:
  A11 A1 0 L32I.N,  A12 A4 MOV.N,  A10 A3 MOV.N,  A8 $34 L L32R,  A8 CALLX8,
  RETW.N,
  2 ZEROS,
$16C L:
  A11 A11 19 L8UI,  A4 A5 MOV.N,  A11 1 $177 L BEQI,
  $9E L J,
$177 L:
  $7D L J,
$17A L:  END-CODE

CODE ppEnqueueRxq   \ .wifislprxiram.50 $4
  $0 1 LITERALS
    SYMBOL pTxRx
$4 L:
  A1 32 ENTRY,  A8 0 MOVI.N,  A8 A2 40 S32I.N,  A8 $0 L L32R,  A8 A8 0 L32I.N,
  A9 A8 $358 L32I,  A2 A9 0 S32I.N,  A2 A2 40 ADDI,  A2 A8 $358 S32I,  MEMW,
  RETW.N,
$20 L:  END-CODE

CODE ppDequeueRxq_Locked   \ .wifislprxiram.51 $4
  $0 1 LITERALS
    SYMBOL pTxRx
$4 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A8 A2 0 L32I.N,  A2 A8 $354 L32I,  A2 $24 L BEQZ.N,
  A9 A2 40 L32I.N,  A9 A8 $354 S32I,  A9 $20 L BNEZ.N,
  A9 $354 MOVI,  A9 A8 A9 ADD.N,  A9 A8 $358 S32I,
$20 L:
  A8 0 MOVI.N,  A8 A2 40 S32I.N,
$24 L:
  MEMW,  RETW.N,
$29 L:  END-CODE

CODE ppRxPkt   \ .wifislprxiram.29 $2C
  $0 11 LITERALS
    SYMBOL pTxRx  SYMBOL bss.s_michael_mic_failure_cb  SYMBOL rodata_wlog_error.30
    SYMBOL wDevCheckBlockError  SYMBOL ppRxProtoProc  SYMBOL ppRxFragmentProc
    SYMBOL ppRxProtoProc  SYMBOL is_fragmented_pkt  SYMBOL wifi_log
    SYMBOL ppRecycleRxPkt  SYMBOL ppDequeueRxq_Locked
$2C L:
  A1 32 ENTRY,  A3 1 MOVI.N,  A6 $0 L L32R,  A4 16 MOVI.N,  A5 A3 29 SLLI,
  $13C L J,
$3C L:
  A10 A2 MOV.N,  A8 $C L L32R,  A8 CALLX8,
  A7 A2 16 L32I.N,  A10 A2 MOV.N,  A9 A7 28 ADDI,  A11 A7 MOV.N,  A8 A2 4 L32I.N,
  A9 A8 4 S32I.N,  A8 A2 44 L32I.N,  A8 A8 0 L32I.N,  A8 A8 A4 AND,
  A8 4 $78 L BBSI,
  MEMW,  A8 $10 L L32R,  A8 CALLX8,
  A10 $69 L BEQZ.N,
  $134 L J,
$69 L:
  A10 A2 MOV.N,  A8 $14 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  A10 $91 L BNEZ.N,
  $13C L J,
$78 L:
  A8 $18 L L32R,  A8 CALLX8,
  A10 $84 L BEQZ.N,
  $134 L J,
  1 ZEROS,
$84 L:
  A10 A2 MOV.N,  A8 $1C L L32R,  A8 CALLX8,
  A10 $91 L BEQZ.N,
  $134 L J,
$91 L:
  A8 A7 0 L32I.N,  A12 1 MOVI.N,  A12 A12 28 SLLI,  A12 A8 A12 AND,
  A12 $100 L BEQZ,
  A8 A7 27 L8UI,  A8 $AD L BEQZ.N,
  A9 -245 MOVI,  A9 A8 A9 ADD.N,  A9 $AD L BEQZ.N,
  $134 L J,
$AD L:
  A9 $0 L L32R,  A9 A9 0 L32I.N,  A9 A9 $3B0 L32I,  A9 $134 L BEQZ,
  A10 245 MOVI,  A8 A10 $ED L BNE,
  A8 $4 L L32R,  A8 A8 0 L32I.N,  A8 $D9 L BEQZ.N,
  A10 -1 MOVI,  A7 A7 8 L32I,  A10 A10 A7 XOR,  A10 A10 31 1 EXTUI,  A8 CALLX8,
  $134 L J,
  2 ZEROS,
$D9 L:
  A13 $8 L L32R,  A12 A3 MOV.N,  A11 32 MOVI.N,  A10 6 MOVI.N,  A8 $20 L L32R,
  A8 CALLX8,
  $134 L J,
  2 ZEROS,
$ED L:
  A7 A2 16 L32I.N,  A12 0 MOVI.N,  A10 A2 MOV.N,  A11 A7 0 L8UI,  A11 A11 7 SEXT,
  A9 CALLX8,
  $13C L J,
  1 ZEROS,
$100 L:
  A9 A8 A5 AND,  A9 $118 L BEQZ.N,
  A7 $0 L L32R,  A7 A7 0 L32I.N,  A7 A7 $3B4 L32I,  A7 $134 L BEQZ.N,
  A8 A2 16 L32I.N,  A11 A8 0 L8UI,  $129 L J,
  1 ZEROS,
$118 L:
  A8 30 $134 L BBCI,
  A7 A6 0 L32I.N,  A7 A7 $3B8 L32I,  A7 $134 L BEQZ.N,
  A12 A9 MOV.N,  A8 A2 16 L32I.N,  A11 A8 0 L8UI,
$129 L:
  A11 A11 7 SEXT,  A10 A2 MOV.N,  A7 CALLX8,
  $13C L J,
$134 L:
  A10 A2 MOV.N,  A8 $24 L L32R,  A8 CALLX8,
$13C L:
  A8 $28 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  A10 $3C L BNEZ,
  RETW.N,
$149 L:  END-CODE

CODE emul_timeout   \ .text.emul_timeout $0
  $0 ORIGIN
  A1 32 ENTRY,  RETW.N,
$5 L:  END-CODE

CODE ppGetTxframe   \ .wifiextrairam.59 $C
  $0 3 LITERALS
    SYMBOL pTxRx  SYMBOL GetAccess  SYMBOL lmacAdjustTimestamp
$C L:
  A1 32 ENTRY,  A4 $0 L L32R,  A3 A2 0 8 EXTUI,  A9 A3 A3 ADDX2,  A9 A9 4 SLLI,
  A2 A3 A3 ADD.N,  A8 A4 0 L32I.N,  A10 4 MOVI.N,  A8 A8 A9 ADD.N,  A8 A8 44 L8UI,
  A8 8 $40 L BGEUI,
  A8 $3E L BEQZ.N,
  A8 3 $3E L BEQI,
  A10 3 MOVI.N,  A8 A10 $40 L BLTU,
  A10 1 MOVI.N,  A8 6 $40 L BLTUI,
  A10 0 MOVI.N,  $40 L J,
  1 ZEROS,
$3E L:
  A10 2 MOVI.N,
$40 L:
  A8 $4 L L32R,  A8 CALLX8,
  A9 A4 0 L32I.N,  A8 A2 A3 ADD.N,  A8 A8 4 SLLI,  A8 A9 A8 ADD.N,  A2 0 MOVI.N,
  A4 A8 41 L8UI,  A4 A2 $94 L BNE,
  A2 A3 1 ADDI.N,  A2 A2 A2 ADDX2,  A2 A2 4 SLLI,  A2 A9 A2 ADD.N,  A2 A2 4 L32I.N,
  A2 $94 L BNEZ.N,
  $8F L J,
  1 ZEROS,
$69 L:
  A10 A2 40 L32I.N,  A10 A8 32 S32I.N,  A10 $7D L BNEZ.N,
  A3 A3 A3 ADDX2,  A3 A3 4 SLLI,  A3 A3 32 ADDI,  A3 A9 A3 ADD.N,  A3 A8 36 S32I,
$7D L:
  A3 0 MOVI.N,  A10 A2 MOV.N,  A3 A2 40 S32I.N,  MEMW,  A8 $8 L L32R,  A8 CALLX8,
  $94 L J,
$8F L:
  A2 A8 32 L32I.N,  A2 $69 L BNEZ,
$94 L:
  RETW.N,
$96 L:  END-CODE

CODE ppEnableQueue   \ .text.ppEnableQueue $4
  $0 1 LITERALS
    SYMBOL pTxRx
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A2 0 8 EXTUI,  A10 0 MOVI.N,  A6 A6 0 8 EXTUI,
  A9 A8 0 L32I.N,  A8 A2 A2 ADDX2,  A8 A8 4 SLLI,  A8 A9 A8 ADD.N,  A6 A6 A9 ADDX4,
  A5 A8 44 S8I,  A4 A8 45 S8I,  A3 A8 46 S8I,  A8 A2 1 ADDI.N,  A8 A8 A8 ADDX2,
  A8 A8 4 SLLI,  A8 A9 A8 ADD.N,  A10 A8 1 S8I,  MEMW,  A8 1 MOVI.N,  A2 SSL,
  A8 A8 SLL,  A2 A6 4 L32I.N,  A2 A2 A8 OR,  A2 A6 4 S32I.N,  A2 A9 0 L32I.N,
  A2 A2 A8 OR,  A2 A9 0 S32I.N,  MEMW,  RETW.N,
$53 L:  END-CODE

CODE ppSearchTxQueue   \ .wifi0iram.62 $C
  $0 3 LITERALS
    SYMBOL pTxRx  $FFFF T,  SYMBOL ppEnableQueue
$C L:
  A1 32 ENTRY,  A11 A2 0 8 EXTUI,  A2 $0 L L32R,  A14 A5 0 8 EXTUI,
  A13 A4 0 8 EXTUI,  A12 A3 0 8 EXTUI,  A15 1 MOVI.N,  A8 A2 0 L32I.N,
  A10 -1 MOVI.N,  A2 A14 A8 ADDX4,  A4 A2 4 L32I.N,  A9 A8 0 L32I.N,
  A4 $38 L BNEZ.N,
  A2 A8 A14 ADD.N,  A2 A2 24 L8UI,  A9 A2 $78 L BBS,
  $F8 L J,
$38 L:
  A2 A4 NEG,  A2 A2 A4 AND,  A2 A2 NSAU,  A2 A2 NEG,  A2 A2 31 ADDI,
  A3 A2 A2 ADDX2,  A3 A3 4 SLLI,  A3 A8 A3 ADD.N,  A5 A3 44 L8UI,  A5 A13 $69 L BNE,
  A5 A3 45 L8UI,  A5 A12 $69 L BNE,
  A3 A3 46 L8UI,  A3 A11 $69 L BNE,
  A2 A2 0 8 EXTUI,  $100 L J,
  2 ZEROS,
$69 L:
  A2 SSL,  A2 A15 SLL,  A2 A10 A2 XOR,  A4 A4 A2 AND,  A4 $38 L BNEZ,
$78 L:
  A8 A8 A14 ADD.N,  A15 A8 24 L8UI,  A10 15 MOVI.N,  A8 -1 MOVI.N,  A10 A9 A10 OR,
  A15 SSL,  A15 A8 SLL,  A4 $4 L L32R,  A8 A8 A15 XOR,  A3 A10 A4 XOR,
  A8 A8 A3 AND,  A2 16 MOVI.N,  A10 A4 $100 L BEQ,
  A10 31 MOVI.N,  A8 $A8 L BEQZ.N,
  A8 A8 NSAU,  A10 A10 A8 SUB,  $B1 L J,
$A8 L:
  A15 A15 A3 AND,  A15 A15 NSAU,  A10 A10 A15 SUB,
$B1 L:
  A2 -1 MOVI.N,  A10 SSL,  A4 A2 SLL,  A2 A2 A4 XOR,  A2 A2 A9 AND,  A3 31 MOVI.N,
  A2 $D8 L BEQZ.N,
  A2 A2 NSAU,  A3 A3 A2 SUB,  A10 A10 1 ADDI.N,  A10 A10 A3 ADD.N,
  A2 A10 1 8 EXTUI,  A10 A10 1 SRAI,  $FA L J,
  2 ZEROS,
$D8 L:
  A4 A4 A9 AND,  A4 A4 NSAU,  A10 A10 12 ADDI.N,  A3 A3 A4 SUB,  A8 A10 A3 ADD,
  A10 A8 1 ADDI,  A2 30 MOVI,  A10 A10 1 SRAI,  A2 A8 $F5 L BGE,
  A10 A10 -12 ADDI,
$F5 L:
  A2 A10 0 8 EXTUI,
$F8 L:
  A10 A2 MOV.N,
$FA L:
  A8 $8 L L32R,  A8 CALLX8,
$100 L:
  RETW.N,
$102 L:  END-CODE

CODE pp_post   \ .iram1.66 $1C
  $0 7 LITERALS
    SYMBOL g_osi_funcs_p  SYMBOL g_intr_lock_mux  SYMBOL pp_sig_cnt
    SYMBOL xphyQueue  SYMBOL pTxRx  SYMBOL rodata_wlog_warning.67  SYMBOL wifi_log
$1C L:
  A1 48 ENTRY,  A5 $0 L L32R,  A9 17 MOVI.N,  A4 A5 0 L32I.N,  A6 $4 L L32R,
  A2 A1 0 S32I.N,  A3 A1 4 S32I.N,  A8 A4 40 L32I.N,  A10 A6 0 L32I.N,
  A4 $8 L L32R,  A9 A2 $9D L BGEU,
  A4 A4 A2 ADD,  MEMW,  A8 CALLX8,
  A11 A10 MOV.N,  MEMW,  A2 A4 0 L8UI,  A2 A2 0 8 EXTUI,  A3 A5 0 L32I.N,
  A2 $55 L BEQZ.N,
  A2 A3 44 L32I.N,  $BA L J,
  1 ZEROS,
$55 L:
  MEMW,  A2 A4 0 L8UI,  A2 A2 1 ADDI.N,  A2 A2 0 8 EXTUI,  A10 A6 0 L32I.N,  MEMW,
  A2 A4 0 S8I,  A2 A3 44 L32I.N,  MEMW,  A2 CALLX8,
  A12 A1 8 ADDI.N,  A11 A1 MOV.N,  A3 $C L L32R,  A2 A5 0 L32I.N,  A10 A3 0 L32I.N,
  A2 A2 104 L32I,  A2 CALLX8,
  A2 A10 MOV.N,  A3 A1 8 L32I.N,  A3 $8E L BEQZ.N,
  A3 A5 0 L32I.N,  A3 A3 48 L32I.N,  A3 CALLX8,
$8E L:
  A10 A2 -1 ADDI.N,  A8 1 MOVI.N,  A2 0 MOVI.N,  A8 A2 A10 MOVEQZ,  $150 L J,
  3 ZEROS,
$9D L:
  A8 CALLX8,
  A8 A4 A2 ADD.N,  MEMW,  A8 A8 0 L8UI,  A11 A10 MOV.N,  A8 A8 0 8 EXTUI,
  A8 $C6 L BEQZ.N,
  A8 A2 -6 ADDI,  A8 3 $C6 L BLTUI,
  A2 A5 0 L32I.N,  A2 A2 44 L32I,
$BA L:
  A10 A6 0 L32I,  A2 CALLX8,
  $10F L J,
  3 ZEROS,
$C6 L:
  A8 13 MOVI.N,  A2 A8 $DE L BEQ,
  A4 A4 A2 ADD.N,  MEMW,  A8 A4 0 L8UI,  A8 A8 1 ADDI.N,  A8 A8 0 8 EXTUI,  MEMW,
  A8 A4 0 S8I,
$DE L:
  A4 A5 0 L32I.N,  A10 A6 0 L32I.N,  A6 13 MOVI.N,  A4 A4 44 L32I.N,  MEMW,
  A4 CALLX8,
  A4 $C L L32R,  A2 A6 $116 L BEQ,
$F2 L:
  A10 10 MOVI.N,  A5 A5 0 L32I.N,  A6 A5 100 L32I,  A5 A5 160 L32I,  A4 A4 0 L32I.N,
  A5 CALLX8,
  A12 A10 MOV.N,  A11 A1 A1 OR,  A10 A4 A4 OR,  A6 CALLX8,
  A10 1 $138 L BNEI,
$10F L:
  A8 0 MOVI.N,  $150 L J,
  2 ZEROS,
$116 L:
  A6 A5 0 L32I.N,  A10 A4 0 L32I.N,  A6 A6 120 L32I,  A6 CALLX8,
  A6 149 MOVI,  A8 1 MOVI.N,  A6 A10 $150 L BLTU,
  A6 $10 L L32R,  A9 48 MOVI.N,  A6 A6 0 L32I.N,  A6 A6 $3C0 L32I,
  A9 A6 $F2 L BGEU,
  $150 L J,
$138 L:
  A13 $14 L L32R,  A11 1 MOVI.N,  A15 A3 MOV.N,  A14 A2 MOV.N,  A12 2 MOVI.N,
  A11 A11 11 SLLI,  A10 6 MOVI.N,  A8 $18 L L32R,  A8 CALLX8,
  A8 1 MOVI.N,
$150 L:
  A2 A8 MOV.N,  RETW.N,
$154 L:  END-CODE

CODE ppMapWaitTxq   \ .wifiextrairam.63 $14
  $0 5 LITERALS
    SYMBOL pTxRx  $FFFF T,  $FF0FFFFF T,  SYMBOL ppSearchTxQueue  SYMBOL pp_post
$14 L:
  A1 48 ENTRY,  A5 A2 0 8 EXTUI,  A2 $0 L L32R,  A4 15 MOVI.N,  A7 A2 0 L32I.N,
  A2 A7 0 L32I.N,  A4 A4 A2 OR,  A2 0 MOVI.N,  A6 $4 L L32R,  A4 A6 $31 L BNE,
  $F5 L J,
$31 L:
  A6 A5 A7 ADDX8,  A4 A5 3 SLLI,  A3 A6 $334 L32I,  A3 A2 $40 L BNE,
  $F5 L J,
$40 L:
  A5 A5 102 ADDI,  A5 A5 A7 ADDX8,  A5 A5 4 ADDI.N,  A5 A6 $338 S32I,  A5 12 MOVI,
  A2 A6 $334 S32I,
$51 L:
  A6 A3 MOV.N,  A2 A6 4 L32I.N,  A7 A6 44 L32I.N,  A10 A2 4 L32I.N,  A2 A7 0 L32I.N,
  A3 A3 40 L32I.N,  A11 A6 36 L32I.N,  A2 18 $64 L BBCI,
  A10 A10 4 ADDI.N,
$64 L:
  A9 A6 40 ADDI,  A8 $0 L L32R,  A12 A10 0 L8UI,  A2 A8 0 L32I.N,  A10 A7 16 L32I.N,
  A12 A5 $7D L BANY,
  A10 A10 20 4 EXTUI,  MEMW,  $B0 L J,
$7D L:
  A12 A7 4 L32I.N,  A10 A10 18 2 EXTUI,  A13 A12 4 4 EXTUI,  A12 A12 0 4 EXTUI,
  A11 A11 128 L8UI,  A9 A1 0 S32I.N,  MEMW,  A8 $C L L32R,  A8 CALLX8,
  A9 A1 0 L32I.N,  A10 16 $B0 L BNEI,
  A7 0 MOVI.N,  A2 A2 A4 ADD.N,  A7 A6 40 S32I.N,  A7 A2 $338 L32I,  A6 A7 0 S32I.N,
  A9 A2 $338 S32I,  MEMW,  $F0 L J,
  1 ZEROS,
$B0 L:
  A11 A7 16 L32I.N,  A12 A10 0 4 EXTUI,  A10 A12 20 SLLI,  A12 A12 A12 ADDX2,
  A12 A12 4 SLLI,  A13 $8 L L32R,  A12 A2 A12 ADD.N,  A11 A11 A13 AND,
  A10 A11 A10 OR,  A11 0 MOVI.N,  A10 A7 16 S32I.N,  A10 A12 36 L32I.N,
  A11 A6 40 S32I.N,  A6 A10 0 S32I.N,  A6 A7 16 L32I.N,  A6 A6 20 4 EXTUI,
  A6 A6 A6 ADDX2,  A6 A6 4 SLLI,  A10 A7 4 L32I.N,  A2 A2 A6 ADD.N,
  A10 A10 4 4 EXTUI,  A9 A2 36 S32I.N,  MEMW,  A8 $10 L L32R,  A8 CALLX8,
$F0 L:
  A3 $51 L BNEZ,
  A2 1 MOVI.N,
$F5 L:
  RETW.N,
$F7 L:  END-CODE

CODE ppProcessWaitingQueue   \ .wifiextrairam.64 $C
  $0 3 LITERALS
    SYMBOL bss.current_ifidx$0  SYMBOL pm_allow_tx  SYMBOL ppMapWaitTxq
$C L:
  A1 32 ENTRY,  A4 $0 L L32R,  A3 -1 MOVI.N,  A5 0 MOVI,  A2 A4 0 L8UI,
$1A L:
  A10 A2 MOV.N,  A8 $4 L L32R,  A8 CALLX8,
  A10 $30 L BEQZ.N,
  A10 A2 A2 OR,  A8 $8 L L32R,  A8 CALLX8,
  A3 A2 A10 MOVNEZ,
$30 L:
  A2 A2 1 ADDI.N,  A2 A2 0 8 EXTUI,  A2 3 $3A L BNEI,
  A2 A5 MOV.N,
$3A L:
  A8 A4 0 L8UI,  A8 A2 $1A L BNE,
  A3 -1 $60 L BEQI,
  A3 A3 1 ADDI.N,  A3 A3 0 8 EXTUI,  A3 3 $58 L BEQI,
  A3 A4 0 S8I,  MEMW,  MEMW,  $60 L J,
  1 ZEROS,
$58 L:
  A2 0 MOVI.N,  A2 A4 0 S8I,  MEMW,
$60 L:
  MEMW,  RETW.N,
$65 L:  END-CODE

CODE ppDisableQueue   \ .text.ppDisableQueue $8
  $0 2 LITERALS
    SYMBOL pTxRx  SYMBOL ppProcessWaitingQueue
$8 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A3 A3 0 8 EXTUI,  A9 A8 0 L32I.N,  A8 -2 MOVI.N,
  A3 A3 A9 ADDX4,  A2 SSL,  A2 A8 A8 SRC,  A8 A3 4 L32I.N,  A8 A8 A2 AND,
  A8 A3 4 S32I.N,  A3 A9 0 L32I.N,  A3 A3 A2 AND,  A3 A9 0 S32I.N,  MEMW,
  A8 $4 L L32R,  A8 CALLX8,
  RETW.N,
$37 L:  END-CODE

CODE ppSelectNextQueue   \ .wifiextrairam.61 $8
  $0 2 LITERALS
    SYMBOL pTxRx  SYMBOL ppDisableQueue
$8 L:
  A1 32 ENTRY,  A4 $0 L L32R,  A2 A2 0 8 EXTUI,  A5 A2 2 SLLI,  A9 A4 0 L32I.N,
  A3 A9 A2 ADD.N,  A8 A2 A9 ADDX4,  A3 A3 24 L8UI,  A8 A8 4 L32I.N,
  A8 A3 $9C L BBC,
  A8 A3 A3 ADDX2,  A8 A8 4 SLLI,  A8 A9 A8 ADD.N,  A10 A8 49 L8UI,  A10 $9C L BEQZ,
  A10 0 MOVI.N,  A9 A8 32 L32I.N,  A10 A8 49 S8I,  MEMW,  A9 $4E L BNEZ.N,
  A8 A8 52 L32I.N,  A8 $4E L BNEZ.N,
  A11 A2 MOV.N,  A10 A3 MOV.N,  MEMW,  A8 $4 L L32R,  A8 CALLX8,
$4E L:
  A11 A4 0 L32I.N,  A5 A11 A5 ADD.N,  A4 A5 4 L32I.N,  A4 $9C L BEQZ,
  A9 -1 MOVI.N,  A10 1 MOVI.N,  A3 SSL,  A5 A9 SLL,  A3 SSL,  A10 A10 SLL,
  A8 A4 A5 AND,  A10 A9 A10 XOR,  A10 A10 A8 AND,  A10 $7D L BEQZ.N,
  A8 A10 NEG,  A8 A8 A10 AND,  $8B L J,
  2 ZEROS,
$7D L:
  A9 A9 A5 XOR,  A9 A9 A4 AND,  A9 $9C L BEQZ.N,
  A8 A9 NEG,  A8 A8 A9 AND,
$8B L:
  A8 A8 NSAU,  A8 A8 NEG,  A8 A8 31 ADDI,  A11 A11 A2 ADD.N,  A8 A11 24 S8I,  MEMW,
$9C L:
  MEMW,  RETW.N,
$A1 L:  END-CODE

CODE ppSearchTxframe   \ .wifi0iram.60 $18
  $0 6 LITERALS
    SYMBOL pTxRx  SYMBOL ppGetTxframe  SYMBOL ppSelectNextQueue
    SYMBOL ppGetTxframe  SYMBOL ppGetTxframe  SYMBOL ppGetTxframe
$18 L:
  A1 48 ENTRY,  A4 A2 0 8 EXTUI,  A4 4 $2C L BNEI,
  A10 2 MOVI.N,  A8 $4 L L32R,  A8 CALLX8,
  $131 L J,
$2C L:
  A10 A4 MOV.N,  A8 $8 L L32R,  A8 CALLX8,
  A6 $0 L L32R,  A2 A6 0 L32I.N,  A3 A2 A4 ADD.N,  A5 A3 24 L8UI,  A4 $66 L BNEZ.N,
  A3 A2 4 L32I.N,  A3 A5 $54 L BBC,
  A3 A5 1 ADDI.N,  A3 A3 A3 ADDX2,  A3 A3 4 SLLI,  A2 A2 A3 ADD.N,  A2 A2 1 L8UI,
  A2 $66 L BNEZ.N,
$54 L:
  A2 A4 MOV.N,
$56 L:
  A10 A2 MOV.N,  A8 $C L L32R,  A8 CALLX8,
  A10 $131 L BNEZ,
  A2 A2 1 ADDI.N,  A2 3 $56 L BNEI,
$66 L:
  A2 A6 0 L32I.N,  A7 -1 MOVI.N,  A2 A4 A2 ADDX4,  A5 SSL,  A7 A7 SLL,  A9 1 MOVI.N,
  A8 A2 4 L32I.N,  A2 A8 A7 AND,  $D2 L J,
$7D L:
  A3 A2 NEG,  A3 A3 A2 AND,  A3 A3 NSAU,  A3 A3 NEG,  A5 A3 31 ADDI,  A10 A5 MOV.N,
  A8 A1 0 S32I.N,  A9 A1 4 S32I,  MEMW,  A8 $10 L L32R,  A8 CALLX8,
  A8 A1 0 L32I.N,  A9 A1 4 L32I.N,  A10 $C4 L BEQZ.N,
  A2 A6 0 L32I.N,  A3 A3 32 ADDI,  A3 A3 A3 ADDX2,  A4 A2 A4 ADD.N,  A3 A3 4 SLLI,
  A2 A2 A3 ADD.N,  A5 A4 24 S8I,  A3 1 MOVI.N,  A3 A2 1 S8I,  MEMW,  MEMW,
  $131 L J,
  2 ZEROS,
$C4 L:
  A5 SSL,  A5 A9 SLL,  A3 -1 MOVI.N,  A3 A3 A5 XOR,  A2 A2 A3 AND,
$D2 L:
  A2 $7D L BNEZ,
  A5 -1 MOVI.N,  A5 A5 A7 XOR,  A5 A5 A8 AND,  A7 1 MOVI.N,  $12C L J,
  3 ZEROS,
$E5 L:
  A3 A5 NEG,  A3 A3 A5 AND,  A3 A3 NSAU,  A3 A3 NEG,  A2 A3 31 ADDI,  A10 A2 MOV.N,
  A8 $14 L L32R,  A8 CALLX8,
  A10 $11E L BEQZ.N,
  A5 A6 0 L32I.N,  A3 A3 32 ADDI,  A3 A3 A3 ADDX2,  A4 A5 A4 ADD.N,  A3 A3 4 SLLI,
  A2 A4 24 S8I,  A5 A5 A3 ADD.N,  A2 1 MOVI.N,  A2 A5 1 S8I,  MEMW,  MEMW,
  $131 L J,
$11E L:
  A2 SSL,  A2 A7 SLL,  A3 -1 MOVI.N,  A2 A3 A2 XOR,  A5 A5 A2 AND,
$12C L:
  A5 $E5 L BNEZ,
  A10 A5 MOV.N,
$131 L:
  A2 A10 MOV.N,  RETW.N,
$135 L:  END-CODE

CODE ppCalFrameTimes   \ .wifi0iram.70 $C
  $0 3 LITERALS
    SYMBOL RC_GetBlockAckTime  SYMBOL RC_GetAckTime  SYMBOL RC_GetCtsTime
$C L:
  A1 32 ENTRY,  A8 A2 44 L32I.N,  A12 1 MOVI.N,  A12 A12 19 SLLI,  A3 A8 0 L32I,
  A12 A3 A12 AND,  A10 A8 12 L8UI,  A12 $2C L BEQZ,
  A8 $0 L L32R,  A8 CALLX8,
  $38 L J,
  1 ZEROS,
$2C L:
  A8 $402 MOVI,  A3 A8 $3A L BANY,
  A8 $4 L L32R,  A8 CALLX8,
$38 L:
  A12 A10 MOV.N,
$3A L:
  A3 A2 44 L32I.N,  A11 A2 22 L16UI,  A2 A2 20 L16UI,  A11 A11 A2 ADD.N,
  A11 A11 0 16 EXTUI,  A10 A3 12 L8UI,  A12 A3 10 S16I,  MEMW,  MEMW,  A8 $8 L L32R,
  A8 CALLX8,
  A10 A3 14 S16I,  MEMW,  MEMW,  RETW.N,
$64 L:  END-CODE

CODE ppAMPDU2Normal   \ .wifiextrairam.40 $14
  $0 5 LITERALS
    $DFFFFFFF T,  $FF000FFF T,  $FFB3FEFF T,  SYMBOL rcGetSched
    SYMBOL ppCalFrameTimes
$14 L:
  A1 32 ENTRY,  A9 A2 4 L32I.N,  A11 A9 4 L32I.N,  A8 A2 20 L16UI,  A8 A8 -4 ADDI,
  A8 A8 0 16 EXTUI,  A10 A11 0 L32I.N,  A11 A11 4 ADDI.N,  A10 A10 0 12 EXTUI,
  A8 A2 20 S16I,  A8 A10 A8 SUB,  A8 A2 22 S16I,  MEMW,  MEMW,  A8 A9 0 L32I.N,
  A10 $0 L L32R,  A8 A8 A10 AND,  MEMW,  A8 A9 0 S32I.N,  MEMW,  A8 A9 0 L32I.N,
  A8 A8 12 12 EXTUI,  A8 A8 -4 ADDI,  A8 A8 0 12 EXTUI,  MEMW,  A10 A9 0 L32I.N,
  A8 A8 12 SLLI,  A12 $4 L L32R,  A10 A10 A12 AND,  A8 A10 A8 OR,  MEMW,
  A8 A9 0 S32I.N,  A11 A9 4 S32I.N,  A11 A2 44 L32I.N,  A8 A11 4 L8UI,
  A9 $8 L L32R,  A8 A11 4 S32I.N,  A8 A11 0 L32I.N,  A8 A8 A9 AND,
  A10 A2 36 L32I.N,  A8 A11 0 S32I.N,  MEMW,  A8 $C L L32R,  A8 CALLX8,
  A10 A2 MOV.N,  A8 $10 L L32R,  A8 CALLX8,
  A8 1 MOVI.N,  A9 A2 8 L32I.N,  A8 A8 30 SLLI,  MEMW,  A2 A9 0 L32I.N,
  A2 A2 A8 OR,  MEMW,  A2 A9 0 S32I.N,  MEMW,  RETW.N,
$AA L:  END-CODE

CODE ppAssembleAMPDU   \ .wifiextrairam.38 $14
  $0 5 LITERALS
    $B47 T,  SYMBOL rodata.pp_default_event_ha~8FBB  SYMBOL rodata_wlog_error.39
    SYMBOL wifi_log  SYMBOL ppCalFrameTimes
$14 L:
  A1 32 ENTRY,  A8 A2 44 L32I.N,  A10 9 MOVI.N,  A10 A10 19 SLLI,  A9 A8 0 L32I.N,
  A9 A9 A10 OR,  A9 A8 0 S32I.N,  A9 A2 20 L16UI,  A4 A4 A9 SUB,  A4 A2 22 S16I,
  MEMW,  A4 A8 4 L8UI,  A4 A8 4 S32I.N,  A4 15 MOVI.N,  A8 A8 12 L8UI,
  A8 A8 -16 ADDI,  A8 A8 0 8 EXTUI,  A4 A8 $60 L BGEU,
  A12 1 MOVI.N,  A15 $0 L L32R,  A11 A12 11 SLLI,  A10 6 MOVI.N,  A14 $4 L L32R,
  A13 $8 L L32R,  MEMW,  A8 $C L L32R,  A8 CALLX8,
$5D L:
  $5D L J,
$60 L:
  A10 A2 MOV.N,  A8 $10 L L32R,  A8 CALLX8,
  A4 A3 8 L32I.N,  A9 1 MOVI.N,  A9 A9 30 SLLI,  MEMW,  A8 A4 0 L32I.N,
  A8 A8 A9 OR,  MEMW,  A8 A4 0 S32I.N,  A3 A3 44 L32I.N,  A2 A2 44 L32I.N,
  A3 A3 24 L32I.N,  A3 A2 24 S32I.N,  MEMW,  RETW.N,
$89 L:  END-CODE

CODE ppCalTxAMPDULength   \ .wifi0iram.41 $44
  $0 17 LITERALS
    SYMBOL pTxRx  $BFFFFFFF T,  $C03 T,  SYMBOL rodata.pp_default_event_ha~8FBB
    SYMBOL rodata_wlog_error.42  SYMBOL rcGetAmpduSched
    SYMBOL ppCheckTxAMPDUlength  SYMBOL ppEmptyDelimiterLength
    SYMBOL ppCalSubFrameLength  SYMBOL ppCalSubFrameLength  SYMBOL wifi_log
    SYMBOL lmacAdjustTimestamp  SYMBOL ppCheckTxAMPDUlength
    SYMBOL ppEmptyDelimiterLength  SYMBOL ppEmptyDelimiterLength
    SYMBOL ppAssembleAMPDU  SYMBOL ppPauseTxAMPDU
$44 L:
  A1 112 ENTRY,  A11 A2 44 L32I.N,  A7 A2 MOV.N,  A2 A11 0 L32I.N,
  A2 14 $58 L BBSI,
  A10 A7 36 L32I.N,  A8 $14 L L32R,  A8 CALLX8,
$58 L:
  A3 0 MOVI.N,  A2 A7 44 L32I.N,  A4 A2 16 L32I.N,  A4 A4 20 4 EXTUI,
  A2 A7 36 L32I.N,  A3 A1 48 S32I.N,  A2 A1 52 S32I.N,  A2 A3 $7C L BEQ,
  A3 1 MOVI.N,  A2 A2 130 L8UI,  A2 A2 -4 ADDI,  A2 A2 0 8 EXTUI,  A2 2 $7A L BLTUI,
  A3 A1 48 L32I.N,
$7A L:
  A3 A1 48 S32I.N,
$7C L:
  A3 0 MOVI.N,  A6 A1 MOV.N,  A2 $0 L L32R,  A5 A2 0 L32I.N,  A2 A4 A4 ADDX2,
  A2 A2 4 SLLI,  A5 A5 A2 ADD.N,  A3 A5 76 S8I,  A3 $101 MOVI,  A3 A5 64 S16I,
  MEMW,  A3 0 MOVI.N,  A3 A5 68 S32I,  A8 A7 27 L8UI,  A8 A5 47 S8I,  MEMW,
  A5 A7 4 L32I.N,  A5 A5 4 L32I.N,  A5 A5 3 L8UI,  A7 A1 40 S32I.N,
  A5 A1 56 S32I.N,  A5 1 MOVI.N,  MEMW,  $18D L J,
$BA L:
  A11 A15 8 L32I.N,  A13 A15 MOV.N,  MEMW,  A10 A11 0 L32I.N,  A8 $4 L L32R,
  A10 A10 A8 AND,  MEMW,  A10 A11 0 S32I.N,  A11 A3 MOV.N,  A12 A1 56 L32I.N,
  A14 A1 48 L32I.N,  A10 A1 52 L32I.N,  A15 A1 76 S32I,  MEMW,  A8 $18 L L32R,
  A8 CALLX8,
  A12 A10 MOV.N,  A10 $0 L L32R,  A11 A10 0 L32I.N,  A13 A11 A2 ADD.N,
  A10 A13 76 L8UI,  A15 A1 76 L32I,  A10 $14A L BEQZ,
  A8 A1 48 L32I.N,  A10 A5 A8 AND,  A10 $14A L BEQZ,
  A5 A10 MOV.N,  A1 A6 $14A L BEQ,
  A14 A6 8 L32I.N,  A10 1 MOVI.N,  A10 A10 30 SLLI,  MEMW,  A5 A14 0 L32I.N,
  A5 A5 A10 OR,  A10 A6 MOV.N,  MEMW,  A5 A14 0 S32I.N,  A5 2 MOVI,  A5 A13 65 S8I,
  MEMW,  A5 0 MOVI,  A11 A1 64 S32I,  A12 A1 68 S32I,  A13 A1 72 S32I,
  A15 A1 76 S32I,  MEMW,  A8 $1C L L32R,  A8 CALLX8,
  A10 A3 A10 SUB,  A13 A1 72 L32I,  A15 A1 76 L32I,  A12 A1 68 L32I,
  A11 A1 64 L32I,  A10 A13 68 S32I,
$14A L:
  A12 $189 L BNEZ.N,
  A2 A6 8 L32I.N,  A10 A6 MOV.N,  A12 A2 8 S32I.N,  MEMW,  $15E L J,
$158 L:
  A10 A2 8 L32I.N,  A12 A10 8 S32I.N,  A10 A2 MOV.N,
$15E L:
  A2 A10 40 L32I.N,  A2 $158 L BNEZ,
  A12 A4 A4 ADD.N,  A2 A6 40 S32I.N,  A2 A4 A4 ADDX2,  A2 A2 4 SLLI,
  A2 A11 A2 ADD.N,  A13 A2 32 L32I.N,  A13 A10 40 S32I.N,  A13 $17A L BNEZ.N,
  A10 A10 40 ADDI,  A10 A2 36 S32I.N,
$17A L:
  A2 A12 A4 ADD.N,  A2 A2 4 SLLI,  A11 A11 A2 ADD.N,  A15 A11 32 S32I.N,  MEMW,
  $192 L J,
$189 L:
  A3 A3 A12 ADD.N,  A6 A15 MOV.N,
$18D L:
  A15 A6 40 L32I.N,  A15 $BA L BNEZ,
$192 L:
  A2 A7 44 L32I.N,  A2 A2 0 L32I.N,  A2 22 $1AA L BBCI,
$199 L:
  A15 A4 A4 ADDX2,  A15 A15 4 SLLI,  A2 A15 32 ADDI,  A2 A1 60 S32I.N,  MEMW,
  $295 L J,
$1AA L:
  A10 A7 MOV.N,  A8 $20 L L32R,  A8 CALLX8,
  A10 A7 MOV.N,  A8 $24 L L32R,  A8 CALLX8,
  A10 A3 $199 L BEQ,
  A12 1 MOVI.N,  A15 $8 L L32R,  A11 A12 11 SLLI,  A10 6 MOVI,  A14 $C L L32R,
  A13 $10 L L32R,  A8 $28 L L32R,  A8 CALLX8,
$1D4 L:
  $1D4 L J,
  1 ZEROS,
$1D8 L:
  A10 A2 44 L32I.N,  A10 A10 0 L32I.N,  A10 18 $1E2 L BBSI,
  $2A2 L J,
$1E2 L:
  A10 A2 MOV.N,  A15 A1 76 S32I,  MEMW,  A8 $2C L L32R,  A8 CALLX8,
  A13 A2 MOV.N,  A11 A2 8 L32I.N,  A12 $4 L L32R,  MEMW,  A10 A11 0 L32I.N,
  A10 A10 A12 AND,  MEMW,  A10 A11 0 S32I.N,  A11 A3 MOV.N,  A12 A1 56 L32I.N,
  A14 A1 48 L32I.N,  A10 A1 52 L32I.N,  MEMW,  A8 $30 L L32R,  A8 CALLX8,
  A13 A10 MOV.N,  A8 $0 L L32R,  A15 A1 76 L32I,  A11 A8 0 L32I.N,
  A12 A11 A15 ADD.N,  A10 A12 76 L8UI,  A10 $273 L BEQZ,
  A8 A1 48 L32I.N,  A5 A8 $273 L BNONE,
  A10 1 MOVI.N,  A10 A10 30 SLLI,  A14 A6 8 L32I.N,  MEMW,  A5 A14 0 L32I.N,
  A5 A5 A10 OR,  A10 A6 MOV.N,  MEMW,  A5 A14 0 S32I.N,  A5 2 MOVI.N,
  A5 A12 65 S8I,  MEMW,  A5 0 MOVI.N,  A11 A1 64 S32I,  A12 A1 68 S32I,
  A13 A1 72 S32I,  A15 A1 76 S32I,  MEMW,  A8 $34 L L32R,  A8 CALLX8,
  A10 A3 A10 SUB,  A12 A1 68 L32I,  A15 A1 76 L32I,  A13 A1 72 L32I,
  A11 A1 64 L32I,  A10 A12 68 S32I,
$273 L:
  A13 $2A2 L BEQZ.N,
  A10 A11 A15 ADD.N,  A12 A10 32 L32I.N,  A12 A12 40 L32I.N,  A12 A10 32 S32I.N,
  A12 $285 L BNEZ.N,
  A8 A1 60 L32I.N,  A11 A11 A8 ADD.N,  A11 A10 36 S32I.N,
$285 L:
  A3 A3 A13 ADD.N,  A8 A6 8 L32I.N,  A10 A2 4 L32I.N,  A2 A6 40 S32I.N,
  A6 A2 MOV.N,  A10 A8 8 S32I.N,  A8 0 MOVI.N,  A8 A2 40 S32I.N,
$295 L:
  A8 $0 L L32R,  A2 A8 0 L32I.N,  A2 A2 A15 ADD.N,  A2 A2 32 L32I,  A2 $1D8 L BNEZ,
$2A2 L:
  A10 A6 MOV.N,  MEMW,  A8 $38 L L32R,  A8 CALLX8,
  A8 $0 L L32R,  A10 A3 A10 SUB,  A5 1 $2C4 L BNEI,
  A3 A4 A4 ADDX2,  A3 A3 4 SLLI,  A2 A8 0 L32I.N,  A2 A2 A3 ADD,  A10 A2 68 S32I,
$2C4 L:
  A12 A10 0 16 EXTUI,  A11 A6 MOV.N,  A10 A7 MOV.N,  A2 A4 1 ADDI.N,  A8 A1 64 S32I,
  MEMW,  A8 $3C L L32R,  A8 CALLX8,
  A2 A2 A2 ADDX2,  A2 A2 4 SLLI,  A10 A4 MOV.N,  A8 A1 64 L32I,  A3 A8 0 L32I.N,
  A2 A3 A2 ADD.N,  A7 A2 4 S32I,  MEMW,  A8 $40 L L32R,  A8 CALLX8,
  A2 A7 MOV.N,  RETW.N,
$2F8 L:  END-CODE

CODE ppProcessTxQ   \ .wifi0iram.23 $30
  $0 12 LITERALS
    SYMBOL g_mesh_is_started  SYMBOL g_mesh_is_root  SYMBOL pTxRx
    SYMBOL lmacIsIdle  SYMBOL ppSearchTxframe  SYMBOL pm_is_waked
    SYMBOL ppAMPDU2Normal  SYMBOL trc_isTxAmpduOperational
    SYMBOL trc_tid_isTxAmpduOperational  SYMBOL ppCalTxAMPDULength
    SYMBOL pp_coex_tx_request  SYMBOL lmacTxFrame
$30 L:
  A1 32 ENTRY,  A4 A2 0 8 EXTUI,  A10 A4 MOV.N,  A8 $C L L32R,  A8 CALLX8,
  A10 $45 L BNEZ.N,
$40 L:
  A2 -1 MOVI.N,  $144 L J,
$45 L:
  A2 $0 L L32R,  A2 A2 0 L8UI,  A2 $60 L BNEZ.N,
$4D L:
  A10 A4 MOV.N,  A8 $10 L L32R,  A8 CALLX8,
  A3 A10 MOV.N,  A2 -2 MOVI.N,  A10 $7C L BNEZ.N,
  $144 L J,
  2 ZEROS,
$60 L:
  A2 $4 L L32R,  MEMW,  A2 A2 0 L8UI,  A2 A2 0 8 EXTUI,  A2 $4D L BNEZ,
  A8 $14 L L32R,  A8 CALLX8,
  A10 $4D L BNEZ,
  $40 L J,
  1 ZEROS,
$7C L:
  A2 A10 44 L32I.N,  A2 A2 0 L32I.N,  A2 18 $86 L BBSI,
  $12D L J,
$86 L:
  A10 A10 36 L32I.N,  A10 $99 L BNEZ.N,
$8A L:
  A10 A3 MOV.N,  A8 $18 L L32R,  A8 CALLX8,
  A11 A3 40 L32I.N,  A11 $C2 L BNEZ.N,
  $115 L J,
$99 L:
  A8 $1C L L32R,  A8 CALLX8,
  A10 $8A L BEQZ,
  A2 A3 44 L32I.N,  A11 A2 4 L32I.N,  A11 A11 0 4 EXTUI,  A10 A3 36 L32I.N,
  A8 $20 L L32R,  A8 CALLX8,
  A10 $8A L BEQZ,
  A10 A3 MOV.N,  A8 $24 L L32R,  A8 CALLX8,
  A10 $8A L BEQZ,
  $12D L J,
$C2 L:
  A8 A3 8 L32I.N,  A2 0 MOVI.N,  A2 A3 40 S32I.N,  A2 A8 8 S32I.N,  A8 A11 MOV.N,
$CC L:
  A13 A8 MOV.N,  A9 A8 8 L32I.N,  A8 A8 40 L32I.N,  A2 A9 8 S32I.N,  A8 $CC L BNEZ,
  A2 $8 L L32R,  A12 A2 0 L32I.N,  A2 A3 44 L32I.N,  A8 A2 16 L32I.N,
  A8 A8 20 4 EXTUI,  A10 A8 A8 ADDX2,  A10 A10 4 SLLI,  A10 A12 A10 ADD.N,
  A2 A10 32 L32I.N,  A9 A8 A8 ADD.N,  A2 A13 40 S32I.N,  A2 $F8 L BNEZ.N,
  A13 A13 40 ADDI,  A13 A10 36 S32I.N,
$F8 L:
  A9 A9 A8 ADD.N,  A8 A8 1 ADDI.N,  A9 A9 4 SLLI,  A8 A8 A8 ADDX2,  A9 A12 A9 ADD.N,
  A8 A8 4 SLLI,  A11 A9 32 S32I.N,  A12 A12 A8 ADD.N,  A2 0 MOVI.N,
  A2 A12 4 S32I.N,  MEMW,  $12D L J,
$115 L:
  A8 A3 44 L32I.N,  A8 A8 16 L32I.N,  A8 A8 20 4 EXTUI,  A8 A8 1 ADDI.N,
  A8 A8 A8 ADDX2,  A2 $8 L L32R,  A8 A8 4 SLLI,  A2 A2 0 L32I.N,  A2 A2 A8 ADD.N,
  A11 A2 4 S32I.N,
$12D L:
  A10 A3 MOV.N,  MEMW,  A8 $28 L L32R,  A8 CALLX8,
  A11 A4 MOV.N,  A10 A3 MOV.N,  A8 $2C L L32R,  A8 CALLX8,
  A2 0 MOVI.N,
$144 L:
  RETW.N,
$146 L:  END-CODE

CODE ppResortTxAMPDU   \ .wifi0iram.44 $4C
  $0 19 LITERALS
    SYMBOL pTxRx  SYMBOL rodata.pp_default_event_ha~8FBB
    SYMBOL rodata_wlog_error.45  SYMBOL GetAccess  SYMBOL wifi_log  SYMBOL memcpy
    SYMBOL lmacMSDUAged  SYMBOL ppCalSubFrameLength  SYMBOL ppSubFromAMPDU
    SYMBOL lmacRecycleMPDU  SYMBOL lmacDiscardAgedMSDU
    SYMBOL rcUpdateTxDoneAmpdu2  SYMBOL trc_isTxAmpduOperational
    SYMBOL trc_tid_isTxAmpduOperational  SYMBOL ppAMPDU2Normal
    SYMBOL ppFillAMPDUBar  SYMBOL ppReSendBar  SYMBOL ppResumeTxAMPDU
    SYMBOL ppProcessTxQ
$4C L:
  A1 192 ENTRY,  A6 $0 L L32R,  A2 A2 0 8 EXTUI,  A5 A2 A2 ADDX2,  A5 A5 4 SLLI,
  A4 A2 A2 ADD.N,  A3 A6 0 L32I.N,  A10 4 MOVI.N,  A3 A3 A5 ADD.N,  A3 A3 44 L8UI,
  A3 8 $7F L BGEUI,
  A3 $7D L BEQZ.N,
  A3 3 $7D L BEQI,
  A10 3 MOVI.N,  A3 A10 $7F L BLTU,
  A10 1 MOVI.N,  A3 6 $7F L BLTUI,
  A10 0 MOVI.N,  $7F L J,
$7D L:
  A10 2 MOVI.N,
$7F L:
  A8 $C L L32R,  A8 CALLX8,
  A9 A6 0 L32I.N,  A8 A2 A2 ADDX2,  A8 A8 4 SLLI,  A8 A9 A8 ADD.N,  A7 A8 52 L32I.N,
  A10 A1 116 S32I,  A7 $415 L BEQZ,
  A5 A4 A2 ADD.N,  A5 A5 4 SLLI,  A5 A9 A5 ADD.N,  A6 A5 41 L8UI,  A6 $C4 L BNEZ.N,
  A15 $669 MOVI,  A12 1 MOVI.N,  A14 $4 L L32R,  A15 A15 A15 ADD,  A11 A12 11 SLLI,
  A10 6 MOVI,  A13 $8 L L32R,  MEMW,  A8 $10 L L32R,  A8 CALLX8,
$C0 L:
  $C0 L J,
  1 ZEROS,
$C4 L:
  A6 A7 44 L32I.N,  A12 32 MOVI.N,  A11 A6 MOV.N,  A10 A1 48 ADDI,  A8 A1 148 S32I,
  A9 A1 152 S32I,  MEMW,  A8 $14 L L32R,  A8 CALLX8,
  A10 0 MOVI.N,  A10 A1 54 S16I,  A10 5 MOVI.N,  A10 A1 67 S8I,  MEMW,
  A10 A1 80 ADDI,  A10 A1 8 S32I.N,  A10 A7 4 L32I.N,  A10 A1 88 S32I,
  A10 A5 77 L8UI,  A7 A1 40 S32I.N,  A8 A1 148 L32I,  A9 A1 152 L32I,
  A10 $110 L BNEZ.N,
  A10 A5 65 L8UI,  A10 2 $110 L BNEI,
  A5 A5 68 L32I,  MEMW,  $118 L J,
  1 ZEROS,
$110 L:
  A10 A7 22 L16UI,  A5 A7 20 L16UI,  A5 A10 A5 ADD.N,
$118 L:
  A4 A4 A2 ADD.N,  A4 A4 4 SLLI,  A9 A9 A4 ADD.N,  A4 A9 42 L16UI,  A4 A1 124 S32I,
  A4 1 MOVI.N,  A3 SSL,  A4 A4 SLL,  A5 A1 112 S32I,  A5 A8 60 L32I.N,
  A8 A8 56 L32I.N,  A4 A1 100 S32I,  A4 -1 MOVI.N,  A8 A1 132 S32I,  A8 A1 100 L32I,
  A4 A4 A8 XOR,  A8 A2 A2 ADDX2,  A4 A4 0 8 EXTUI,  A9 A9 40 L8UI,  A8 A8 4 SLLI,
  A6 A6 5 L8UI,  A9 A1 136 S32I,  A9 0 MOVI.N,  A6 A1 104 S32I,  A5 A1 128 S32I,
  A4 A1 120 S32I,  A4 A9 MOV.N,  A6 A7 36 L32I.N,  A5 $0 L L32R,  A9 A1 108 S32I,
  A1 A1 96 S32I,  A8 A1 140 S32I,  MEMW,  $2E8 L J,
$175 L:
  A10 A5 0 L32I.N,  A13 A1 140 L32I,  A10 A10 A13 ADD.N,  A11 A10 77 L8UI,
  A11 $1A9 L BNEZ.N,
  A10 A10 65 L8UI,  A10 2 $1A9 L BNEI,
  A4 $194 L BNEZ.N,
  A10 A1 55 L8UI,  A10 A10 1 ADDI.N,  A10 A1 55 S8I,  MEMW,
$194 L:
  A10 A8 8 L32I.N,  MEMW,  A10 A10 0 L32I.N,  A10 30 $1B4 L BBCI,
  A4 A4 1 ADDI.N,  A4 A4 0 8 EXTUI,  MEMW,  $1B4 L J,
$1A9 L:
  A10 A1 55 L8UI,  A10 A10 1 ADDI.N,  A10 A1 55 S8I,  MEMW,
$1B4 L:
  A11 192 MOVI,  A10 A8 4 L32I,  A10 A10 4 L32I,  A13 A1 124 L32I,  A10 A10 3 L8UI,
  A10 A10 A13 SUB,  A10 A11 $1EE L BANY,
  A12 224 MOVI,  A13 A1 128 L32I,  A11 A10 0 8 EXTUI,  A10 A12 $1D8 L BNONE,
  A13 A1 132 L32I,
$1D8 L:
  A13 A11 $1EE L BBC,
  A12 1 MOVI.N,  A10 A1 54 L8UI,  A10 A10 1 ADDI.N,  A10 A1 54 S8I,  MEMW,  MEMW,
  $249 L J,
$1EE L:
  A10 A8 MOV.N,  A8 A1 148 S32I,  A9 A1 152 S32I,  MEMW,  A8 $18 L L32R,  A8 CALLX8,
  A12 0 MOVI.N,  A8 A1 148 L32I,  A9 A1 152 L32I,  A10 A12 $249 L BNE,
  A4 A12 $21A L BEQ,
  A4 1 $238 L BNEI,
  A10 A8 8 L32I.N,  MEMW,  A10 A10 0 L32I.N,  A10 30 $238 L BBCI,
$21A L:
  A10 A8 4 L32I.N,  A11 A10 4 L32I.N,  A10 A8 44 L32I,  A10 A10 0 L32I,
  A10 18 $22A L BBCI,
  A11 A11 4 ADDI,
$22A L:
  A12 8 MOVI.N,  A10 A11 1 L8UI,  A10 A10 A12 OR,  A10 A11 1 S8I,  MEMW,
$238 L:
  A13 A1 96 L32I,  A13 A13 40 L32I.N,  A8 A1 108 S32I,  A13 A1 96 S32I,  MEMW,
  $2E8 L J,
$249 L:
  A10 A8 MOV.N,  A9 A1 152 S32I,  A12 A1 144 S32I,  A8 A1 148 S32I,  MEMW,
  A8 $1C L L32R,  A8 CALLX8,
  A8 A1 148 L32I,  A11 A8 A8 OR,  A10 A1 96 L32I,  A8 $20 L L32R,  A8 CALLX8,
  A10 1 MOVI.N,  A10 A10 22 SLLI,  A8 A1 148 L32I,  A14 A8 44 L32I.N,
  A11 A14 0 L32I.N,  A10 A11 A10 OR,  A12 A1 144 L32I,  A10 A14 0 S32I.N,
  A9 A1 152 L32I,  A12 $2C4 L BEQZ.N,
  A10 A6 38 L8UI,  A13 A1 100 L32I,  A12 A10 A13 AND,  A13 A12 $2AD L BNE,
  A9 0 MOVI.N,  A11 19 $2A1 L BBCI,
  A9 A8 4 L32I.N,  A9 A9 4 L32I,  A11 18 $2A1 L BBCI,
  A9 A9 4 ADDI,
$2A1 L:
  A13 A1 120 L32I,  A10 A10 A13 AND,  A10 A6 38 S8I,  MEMW,
$2AD L:
  A12 0 MOVI.N,  A11 A8 MOV.N,  A10 A1 116 L32I,  A9 A1 152 S32I,  MEMW,
  A8 $24 L L32R,  A8 CALLX8,
  $2E5 L J,
  1 ZEROS,
$2C4 L:
  A9 A12 MOV.N,  A11 19 $2D2 L BBCI,
  A9 A8 4 L32I.N,  A9 A9 4 L32I.N,  A11 18 $2D2 L BBCI,
  A9 A9 4 ADDI.N,
$2D2 L:
  A12 0 MOVI.N,  A11 A8 MOV.N,  A10 A1 116 L32I,  A9 A1 152 S32I,  MEMW,
  A8 $28 L L32R,  A8 CALLX8,
$2E5 L:
  A9 A1 152 L32I,
$2E8 L:
  A13 A1 96 L32I,  A8 A13 40 L32I.N,  A8 $175 L BNEZ,
  A4 A1 136 L32I,  A4 $311 L BNEZ.N,
  A4 1 MOVI.N,  A4 A1 67 S8I,  MEMW,  A4 A7 44 L32I.N,  A4 A4 13 L8UI,
  A4 A1 61 S8I,  MEMW,  MEMW,  $31C L J,
  3 ZEROS,
$311 L:
  A8 A1 104 L32I,  A4 A8 1 ADDI.N,  A4 A4 0 8 EXTUI,  A4 A1 104 S32I,
$31C L:
  A11 A1 48 ADDI,  A10 A6 MOV.N,  A4 A1 112 L32I,  A12 A4 0 16 EXTUI,
  A4 A2 1 ADDI.N,  A13 A1 104 L32I,  A9 A1 152 S32I,  A13 A1 53 S8I,  MEMW,  MEMW,
  A8 $2C L L32R,  A8 CALLX8,
  A10 A6 MOV.N,  A7 A1 40 L32I,  A8 $30 L L32R,  A8 CALLX8,
  A9 A1 152 L32I,  A10 $380 L BEQZ.N,
  A11 A3 MOV.N,  A10 A6 MOV.N,  A8 $34 L L32R,  A8 CALLX8,
  A9 A1 152 L32I,  A10 $380 L BEQZ.N,
  A4 A4 A4 ADDX2,  A4 A4 4 SLLI,  A5 A5 0 L32I.N,  A4 A5 A4 ADD.N,  A7 A4 4 S32I.N,
  A7 $3EA L BEQZ,
  A5 9 MOVI.N,  A5 A5 19 SLLI,  A6 A7 44 L32I.N,  A4 A6 0 L32I.N,  A4 A4 A5 OR,
  A4 A6 0 S32I.N,  MEMW,  $3EA L J,
$380 L:
  A7 $3DC L BEQZ,
  A6 A5 0 L32I.N,  A8 A4 A4 ADDX2,  A8 A8 4 SLLI,  A6 A6 A8 ADD.N,  A6 A6 4 L32I.N,
  A6 A7 $3A4 L BNE,
  A10 A7 A7 OR,  A9 A1 152 S32I,  MEMW,  A8 $38 L L32R,  A8 CALLX8,
  A9 A1 152 L32I,
$3A4 L:
  A6 A7 MOV.N,  A8 0 MOVI.N,  $3B4 L J,
  3 ZEROS,
$3AE L:
  A10 A6 8 L32I.N,  A6 A6 40 L32I.N,  A8 A10 8 S32I.N,
$3B4 L:
  A13 A1 108 L32I,  A6 A13 $3AE L BNE,
  A8 A2 A2 ADDX2,  A8 A8 4 SLLI,  A10 A5 0 L32I.N,  A11 A2 A2 ADD.N,
  A8 A10 A8 ADD.N,  A12 A8 32 L32I.N,  A12 A6 40 S32I.N,  A12 $3D2 L BNEZ.N,
  A6 A6 40 ADDI,  A6 A8 36 S32I,
$3D2 L:
  A6 A11 A2 ADD.N,  A6 A6 4 SLLI,  A10 A10 A6 ADD,  A7 A10 32 S32I.N,
$3DC L:
  A4 A4 A4 ADDX2,  A4 A4 4 SLLI,  A5 A5 0 L32I.N,  A4 A5 A4 ADD.N,  A5 0 MOVI.N,
  A5 A4 4 S32I.N,
$3EA L:
  A9 $415 L BEQZ.N,
  A11 A9 4 ADDI.N,  A12 A9 10 ADDI.N,  A10 A3 MOV.N,  A13 A9 22 L16UI,
  A13 A13 4 SRLI,  A13 A13 1 ADDI.N,  A13 A13 0 12 EXTUI,  MEMW,  A8 $3C L L32R,
  A8 CALLX8,
  A11 A2 MOV.N,  A10 A3 MOV.N,  A8 $40 L L32R,  A8 CALLX8,
  A2 0 MOVI.N,  $440 L J,
$415 L:
  A10 A2 MOV.N,  A8 $44 L L32R,  A8 CALLX8,
  A10 4 MOVI.N,  A3 8 $438 L BGEUI,
  A3 $436 L BEQZ.N,
  A3 3 $436 L BEQI,
  A10 3 MOVI.N,  A3 A10 $438 L BLTU,
  A10 1 MOVI.N,  A3 6 $438 L BLTUI,
  A10 0 MOVI.N,  $438 L J,
$436 L:
  A10 2 MOVI.N,
$438 L:
  A8 $48 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,
$440 L:
  RETW.N,
$442 L:  END-CODE

CODE ppTask   \ .wifi0iram.9 $6C
  $0 27 LITERALS
    SYMBOL bss.s_pp_task_create_sem  SYMBOL g_osi_funcs_p  SYMBOL xphyQueue
    SYMBOL g_intr_lock_mux  SYMBOL pp_sig_cnt  SYMBOL rodata
    SYMBOL bss.g_timer_func  SYMBOL bss.g_config_func
    SYMBOL bss.g_net80211_tx_func  SYMBOL rodata_wlog_error.10
    SYMBOL bss.s_pp_task_del_sem  SYMBOL s_wifi_queue  SYMBOL bss.pp_task_hdl
    SYMBOL pp_timer_do_process  SYMBOL ppProcessTxQ  SYMBOL ppProcTxDone
    SYMBOL ppRxPkt  SYMBOL ppResortTxAMPDU  SYMBOL ppProcessRxPktHdr
    SYMBOL lmacProcessTxTimeout  SYMBOL lmacProcessTxComplete
    SYMBOL lmacProcessCollisions_task  SYMBOL wifi_gpio_debug
    SYMBOL wdevProcessRxSucDataAll  SYMBOL wdev_process_misc_pkt  SYMBOL wifi_log
    SYMBOL pp_default_event_handler
$6C L:
  A1 48 ENTRY,  A2 $4 L L32R,  A4 $0 L L32R,  A3 A2 0 L32I.N,  A10 A4 0 L32I.N,
  A4 0 MOVI.N,  A3 A3 64 L32I,  A3 CALLX8,
  A3 $8 L L32R,  $8A L J,
  1 ZEROS,
$88 L:
  A4 1 MOVI.N,
$8A L:
  A5 A2 0 L32I.N,  A12 -1 MOVI.N,  A11 A1 MOV.N,  A5 A5 116 L32I,  A10 A3 0 L32I,
  A5 CALLX8,
  A10 1 $9F L BEQI,
  $224 L J,
$9F L:
  A8 29 MOVI.N,  A5 A1 0 L32I.N,  A8 A5 $E6 L BLTU,
  A5 A5 -13 ADDI,  A8 0 MOVI.N,  A10 A8 A5 MOVEQZ,  A10 A10 0 8 EXTUI,
  A10 A8 $E6 L BEQ,
  A5 $C L L32R,  A8 A2 0 L32I.N,  A10 A5 0 L32I.N,  A8 A8 40 L32I.N,  A8 CALLX8,
  A11 A10 MOV.N,  A8 A1 0 L32I.N,  A9 $10 L L32R,  A9 A9 A8 ADD.N,  MEMW,
  A8 A9 0 L8UI,  A8 A8 -1 ADDI.N,  A8 A8 0 8 EXTUI,  MEMW,  A8 A9 0 S8I,
  A8 A2 0 L32I.N,  A10 A5 0 L32I.N,  A8 A8 44 L32I.N,  MEMW,  A8 CALLX8,
$E6 L:
  A5 26 MOVI.N,  A10 A1 0 L32I.N,  A5 A10 $F0 L BGEU,
  $21C L J,
$F0 L:
  A5 $14 L L32R,  A5 A10 A5 ADDX4,  A5 A5 0 L32I.N,  A5 JX,
  1 ZEROS,
$FC L:
  A5 A2 0 L32I.N,  A5 A5 216 L32I,  A5 CALLX8,
  A10 A1 4 L32I.N,  A8 $34 L L32R,  A8 CALLX8,
  $128 L J,
  2 ZEROS,
$111 L:
  A8 A2 0 L32I.N,  A5 $18 L L32R,
$116 L:
  A9 A5 0 L32I.N,  A9 $224 L BEQZ,
  A8 A8 216 L32I,  A8 CALLX8,
  A10 A1 4 L32I.N,  A5 A5 0 L32I.N,  A5 CALLX8,
$128 L:
  A5 A2 0 L32I.N,  A5 A5 220 L32I,  A5 CALLX8,
  $224 L J,
  1 ZEROS,
$134 L:
  A8 A2 0 L32I.N,  A5 $1C L L32R,  $116 L J,
$13C L:
  A10 4 MOVI.N,
$13E L:
  A8 $38 L L32R,  A8 CALLX8,
  $224 L J,
  1 ZEROS,
$148 L:
  A5 $20 L L32R,  A10 A5 0 L32I.N,  A10 CALLX8,
  $224 L J,
  1 ZEROS,
$154 L:
  A10 1 MOVI.N,  A8 $3C L L32R,  A8 CALLX8,
  $224 L J,
  1 ZEROS,
$160 L:
  A5 A2 0 L32I.N,  A5 A5 216 L32I,  A5 CALLX8,
  A8 $40 L L32R,  A8 CALLX8,
  $128 L J,
$171 L:
  A5 A2 0 L32I.N,  A5 A5 216 L32I,  A5 CALLX8,
  A10 A1 4 L8UI,  A8 $44 L L32R,  A8 CALLX8,
  $128 L J,
$185 L:
  A5 A2 0 L32I.N,  A5 A5 216 L32I,  A5 CALLX8,
  A10 A1 4 L32I.N,  A8 $48 L L32R,  A8 CALLX8,
  $128 L J,
$198 L:
  A5 A2 0 L32I.N,  A5 A5 216 L32I,  A5 CALLX8,
  A8 $4C L L32R,  A8 CALLX8,
  $1CA L J,
$1A9 L:
  A5 A2 0 L32I.N,  A5 A5 216 L32I,  A5 CALLX8,
  A8 $50 L L32R,  A8 CALLX8,
  $128 L J,
  2 ZEROS,
$1BC L:
  A5 A2 0 L32I.N,  A5 A5 216 L32I,  A5 CALLX8,
  A8 $54 L L32R,  A8 CALLX8,
$1CA L:
  A5 A2 0 L32I.N,  A5 A5 220 L32I,  A5 CALLX8,
  A11 4 MOVI.N,  A10 10 MOVI.N,  A8 $58 L L32R,  A8 CALLX8,
  $224 L J,
  1 ZEROS,
$1E0 L:
  A5 A2 0 L32I.N,  A5 A5 216 L32I,  A5 CALLX8,
  A10 A1 4 L32I.N,  A8 $5C L L32R,  A8 CALLX8,
  $128 L J,
  1 ZEROS,
$1F4 L:
  A5 A2 0 L32I.N,  A5 A5 216 L32I,  A5 CALLX8,
  A8 $60 L L32R,  A8 CALLX8,
  $128 L J,
  1 ZEROS,
$206 L:
  A12 1 MOVI.N,  A14 A1 4 L32I.N,  A11 A12 11 SLLI,  A10 6 MOVI.N,  A13 $24 L L32R,
  A8 $64 L L32R,  A8 CALLX8,
$218 L:
  $218 L J,
  1 ZEROS,
$21C L:
  A11 A1 4 L32I.N,  A8 $68 L L32R,  A8 CALLX8,
$224 L:
  A4 $8A L BEQZ,
$227 L:
  A4 A2 0 L32I.N,  A10 A3 0 L32I.N,  A4 A4 120 L32I,  A4 CALLX8,
  A10 $88 L BNEZ,
  A3 $28 L L32R,  A10 A3 0 L32I.N,  A10 $244 L BEQZ.N,
  A3 A2 0 L32I,  A3 A3 64 L32I,  A3 CALLX8,
$244 L:
  A3 $2C L L32R,  A4 A2 0 L32I.N,  A10 A3 0 L32I.N,  A4 A4 $180 L32I,  A4 CALLX8,
  A10 0 MOVI.N,  A2 A2 0 L32I.N,  A10 A3 0 S32I.N,  A3 $30 L L32R,  A2 A2 152 L32I,
  A10 A3 0 S32I.N,  MEMW,  A2 CALLX8,
  RETW.N,
  rodata  $13E L!+  $13E L!+  $13E L!+  $13E L!+  $13C L!+  $148 L!+  $134 L!+
  $111 L!+  $FC L!+  $21C L!+  $21C L!+  $21C L!+  $21C L!+  $185 L!+  $206 L!+
  $227 L!+  $154 L!+  $160 L!+  $171 L!+  $224 L!+  $224 L!+  $224 L!+  $198 L!+
  $1A9 L!+  $1BC L!+  $1E0 L!+  $1F4 L!+  DROP   \ the table of jumps
$267 L:  END-CODE

CODE pp_create_task   \ .text.pp_create_task $58
  $0 22 LITERALS
    SYMBOL g_wifi_menuconfig  SYMBOL g_osi_funcs_p  SYMBOL bss.pp_task_hdl
    SYMBOL rodata_wlog_warning.52  SYMBOL s_wifi_queue
    SYMBOL rodata_wlog_warning.53  SYMBOL rodata_wlog_error.54  SYMBOL xphyQueue
    SYMBOL rodata_wlog_error.55  SYMBOL bss.s_pp_task_create_sem
    SYMBOL rodata_wlog_error.56  SYMBOL rodata.pp_create_task.str1.1
    SYMBOL ppTask  SYMBOL rodata_wlog_error.57  SYMBOL rodata_wlog_info.58
    SYMBOL wifi_log  SYMBOL wifi_log  SYMBOL wifi_log
    SYMBOL config_get_wifi_task_stack_size  SYMBOL wifi_log
    SYMBOL config_get_wifi_task_stack_size  SYMBOL wifi_log
$58 L:
  A1 64 ENTRY,  A2 $0 L L32R,  A6 A2 52 L32I.N,  A2 $4 L L32R,  A3 A2 0 L32I,
  A10 A3 168 L32I,  A10 CALLX8,
  A5 A10 MOV.N,  A3 $8 L L32R,  A4 A3 0 L32I.N,  A4 $90 L BEQZ.N,
  A10 6 MOVI.N,  A12 2 MOVI,  A13 $C L L32R,  A11 1 MOVI,  A8 $3C L L32R,
  A8 CALLX8,
  A4 A2 0 L32I.N,  A10 A3 0 L32I.N,  A4 A4 152 L32I,  A4 CALLX8,
$90 L:
  A4 $10 L L32R,  A7 A4 0 L32I.N,  A7 $B1 L BEQZ.N,
  A10 6 MOVI.N,  A12 2 MOVI.N,  A13 $14 L L32R,  A11 1 MOVI.N,  A8 $40 L L32R,
  A8 CALLX8,
  A7 A2 0 L32I.N,  A10 A4 0 L32I,  A7 A7 $180 L32I,  A7 CALLX8,
$B1 L:
  A11 8 MOVI.N,  A10 200 MOVI,  A7 A2 0 L32I.N,  A7 A7 $17C L32I,  A7 CALLX8,
  A10 A4 0 S32I.N,  A10 $D9 L BNEZ.N,
  A12 1 MOVI.N,  A13 $18 L L32R,  A11 A12 MOV.N,  A10 6 MOVI.N,  MEMW,
  A8 $44 L L32R,  A8 CALLX8,
  $1CA L J,
  2 ZEROS,
$D9 L:
  A4 A10 0 L32I.N,  A7 $1C L L32R,  A4 A7 0 S32I.N,  A13 $20 L L32R,
  A4 $13C L BEQZ,
  A11 0 MOVI.N,  A10 1 MOVI.N,  A4 A2 0 L32I.N,  A4 A4 52 L32I,  MEMW,  A4 CALLX8,
  A4 $24 L L32R,  A10 A4 0 S32I.N,  A13 $28 L L32R,  A10 $13C L BEQZ.N,
  A7 A2 0 L32I,  A7 A7 144 L32I,  MEMW,  A8 $48 L L32R,  A8 CALLX8,
  A12 A10 MOV.N,  A8 A2 0 L32I.N,  A10 A8 168 L32I,  A12 A1 16 S32I.N,  MEMW,
  A10 CALLX8,
  A14 A10 -2 ADDI,  A15 A3 MOV.N,  A13 0 MOVI.N,  A12 A1 16 L32I.N,  A11 $2C L L32R,
  A10 $30 L L32R,  A6 A1 0 S32I.N,  MEMW,  A7 CALLX8,
  A7 A10 MOV.N,  A10 1 $14D L BEQI,
  A13 $34 L L32R,
$13C L:
  A12 1 MOVI.N,  A11 A12 11 SLLI,  A10 6 MOVI.N,  A8 $4C L L32R,  A8 CALLX8,
  $19F L J,
  1 ZEROS,
$14D L:
  A8 A2 0 L32I.N,  A11 -1 MOVI.N,  A8 A8 60 L32I.N,  A10 A4 0 L32I.N,  A8 CALLX8,
  A10 A7 MOV.N,  A8 A2 0 L32I.N,  A8 A8 156 L32I,  A8 CALLX8,
  A2 A2 0 L32I.N,  A10 A4 0 L32I,  A2 A2 56 L32I,  A2 CALLX8,
  A2 0 MOVI.N,  A3 A3 0 L32I.N,  A2 A4 0 S32I.N,  MEMW,  A8 $50 L L32R,  A8 CALLX8,
  A15 A5 -2 ADDI,  A15 A15 0 8 EXTUI,  A14 A3 MOV.N,  A13 $38 L L32R,  A12 3 MOVI.N,
  A11 A7 MOV.N,  A10 A1 0 S32I.N,  A10 6 MOVI,  A6 A1 4 S32I,  MEMW,  A8 $54 L L32R,
  A8 CALLX8,
  $1CD L J,
$19F L:
  A4 $10 L L32R,  A10 A4 0 L32I,  A10 $1B5 L BEQZ,
  A5 A2 0 L32I,  A5 A5 $180 L32I,  A5 CALLX8,
  A5 0 MOVI.N,  A5 A4 0 S32I.N,
$1B5 L:
  A4 $24 L L32R,  A10 A4 0 L32I.N,  A10 $1C6 L BEQZ.N,
  A2 A2 0 L32I.N,  A2 A2 56 L32I.N,  MEMW,  A2 CALLX8,
$1C6 L:
  A2 0 MOVI.N,  A2 A3 0 S32I.N,
$1CA L:
  A2 $101 MOVI,
$1CD L:
  MEMW,  RETW.N,
$1D2 L:  END-CODE

CODE pp_deattach   \ .text.pp_deattach $C
  $0 3 LITERALS
    SYMBOL pTxRx  SYMBOL esf_buf_recycle  SYMBOL esf_buf_setdown
$C L:
  A1 32 ENTRY,  A3 $0 L L32R,  A2 0 MOVI,
$15 L:
  A8 200 MOVI,  A8 A2 A8 ADD.N,  A4 A8 2 SLLI,  A9 A3 0 L32I.N,  A8 A8 A9 ADDX4,
  A10 A8 0 L32I.N,  A10 $34 L BEQZ.N,
  A8 $4 L L32R,  A8 CALLX8,
  A9 0 MOVI.N,  A8 A3 0 L32I.N,  A8 A8 A4 ADD.N,  A9 A8 0 S32I.N,
$34 L:
  A2 A2 1 ADDI.N,  A2 4 $15 L BNEI,
  MEMW,  A8 $8 L L32R,  A8 CALLX8,
  RETW.N,
$44 L:  END-CODE

CODE ppInitTxq   \ .text.ppInitTxq $8
  $0 2 LITERALS
    SYMBOL pTxRx  $D0A0704 T,
$8 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A9 0 MOVI.N,  A11 16 MOVI.N,  A8 A8 0 L32I.N,
  A10 A8 32 ADDI,  A11 $27 L LOOP,
  A9 A10 0 S32I.N,  A10 A10 4 S32I.N,  A9 A10 16 S8I,  MEMW,  A10 A10 48 ADDI,
$27 L:
  A10 1 MOVI.N,  A9 A8 0 S32I.N,  A11 7 MOVI.N,  A9 A8 4 S32I.N,  A9 A8 8 S32I.N,
  A9 A8 12 S32I.N,  A9 A8 16 S32I.N,  A9 A8 20 S32I.N,  A9 $4 L L32R,
  A10 A8 48 S8I,  A10 A8 96 S8I,  A10 A8 144 S8I,  A10 A8 192 S8I,  A10 8 MOVI.N,
  A11 A8 44 S8I,  A11 A8 92 S8I,  A11 A8 140 S8I,  A10 A8 188 S8I,  MEMW,
  A9 A8 24 S32I.N,  MEMW,  RETW.N,
$5E L:  END-CODE

CODE ppTxqEmpty   \ .text.ppTxqEmpty $8
  $0 2 LITERALS
    SYMBOL pTxRx  SYMBOL is_lmac_idle
$8 L:
  A1 32 ENTRY,  A8 $4 L L32R,  A8 CALLX8,
  A10 $50 L BEQZ.N,
  A8 $0 L L32R,  A9 A8 0 L32I.N,  A11 A9 $334 L32I,  A11 $48 L BNEZ.N,
  A8 A9 $33C L32I,  A8 $48 L BNEZ.N,
  A11 A9 $344 L32I,  A11 $48 L BNEZ.N,
  A8 A9 32 ADDI,  A11 16 MOVI,  A11 $45 L LOOP,
  A12 A8 9 L8UI,  A12 $39 L BNEZ.N,
  A13 A8 20 L32I.N,  A13 $4E L BNEZ.N,
$39 L:
  A12 A8 0 L32I.N,  A12 $48 L BNEZ.N,
  A13 A9 $34C L32I,  A13 $4E L BNEZ.N,
  A8 A8 48 ADDI,
$45 L:
  $50 L J,
$48 L:
  A10 0 MOVI.N,  $50 L J,
  1 ZEROS,
$4E L:
  A10 A12 MOV.N,
$50 L:
  A2 A10 MOV.N,  RETW.N,
$54 L:  END-CODE

CODE pp_delete_task   \ .text.pp_delete_task $1C
  $0 7 LITERALS
    SYMBOL bss.pp_task_hdl  SYMBOL s_wifi_queue  SYMBOL g_osi_funcs_p
    SYMBOL bss.s_pp_task_del_sem  SYMBOL pp_post  SYMBOL pp_delete_task_manually
    SYMBOL pp_delete_task_manually
$1C L:
  A1 32 ENTRY,  A2 $0 L L32R,  A2 A2 0 L32I.N,  A2 $8C L BEQZ,
  A2 $4 L L32R,  A2 A2 0 L32I.N,  A2 $8C L BEQZ,
  A11 0 MOVI.N,  A10 1 MOVI.N,  A3 $8 L L32R,  A2 A3 0 L32I.N,  A2 A2 52 L32I.N,
  A2 CALLX8,
  A2 $C L L32R,  A10 A2 0 S32I.N,  A10 $4A L BNEZ.N,
  MEMW,  $8C L J,
$4A L:
  A11 0 MOVI.N,  A10 15 MOVI.N,  A8 $10 L L32R,  A8 CALLX8,
  A10 $61 L BEQZ.N,
  A8 $14 L L32R,  A8 CALLX8,
  $6C L J,
  2 ZEROS,
$61 L:
  A8 A3 0 L32I.N,  A11 -1 MOVI.N,  A8 A8 60 L32I.N,  A10 A2 0 L32I.N,  A8 CALLX8,
$6C L:
  A10 1 MOVI.N,  A8 A3 0 L32I.N,  A8 A8 156 L32I,  A8 CALLX8,
  A3 A3 0 L32I.N,  A10 A2 0 L32I.N,  A3 A3 56 L32I,  A3 CALLX8,
  A10 0 MOVI.N,  A10 A2 0 S32I.N,  MEMW,  $92 L J,
  2 ZEROS,
$8C L:
  A8 $18 L L32R,  A8 CALLX8,
$92 L:
  A2 A10 MOV.N,  RETW.N,
$96 L:  END-CODE

CODE ppProcessWaitQ   \ .text.ppProcessWaitQ $4
  $0 1 LITERALS
    SYMBOL ppProcessWaitingQueue
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$F L:  END-CODE

CODE ppCheckTxIdle   \ .text.ppCheckTxIdle $C
  $0 3 LITERALS
    SYMBOL pTxRx  SYMBOL lmacIsIdle  SYMBOL ppDisableQueue
$C L:
  A1 32 ENTRY,  A2 0 MOVI,
$12 L:
  A10 A2 MOV.N,  A8 $4 L L32R,  A8 CALLX8,
  A10 $21 L BNEZ.N,
$1C L:
  A2 1 MOVI.N,  $BC L J,
$21 L:
  A2 A2 1 ADDI.N,  A2 5 $12 L BNEI,
  $8E L J,
$29 L:
  A8 A2 NEG,  A8 A8 A2 AND,  A8 A8 NSAU,  A8 A8 NEG,  A10 A8 31 ADDI,
  A11 A10 A10 ADDX2,  A11 A11 4 SLLI,  A11 A9 A11 ADD.N,  A2 A11 32 L32I.N,
  A2 $1C L BNEZ,
  A8 A8 32 ADDI,  A8 A8 A8 ADDX2,  A8 A8 4 SLLI,  A8 A9 A8 ADD.N,  A2 A8 4 L32I.N,
  A2 $1C L BNEZ,
  A8 A11 44 L8UI,  A11 4 MOVI.N,  A8 8 $88 L BGEUI,
  A9 1 MOVI.N,  A11 A2 MOV.N,  A11 A9 A8 MOVNEZ,  A11 A11 0 8 EXTUI,
  A11 $86 L BEQZ.N,
  A12 A8 -3 ADDI,  A11 A2 MOV.N,  A11 A9 A12 MOVNEZ,  A11 A11 0 8 EXTUI,
  A11 $86 L BEQZ.N,
  A11 3 MOVI.N,  A8 A11 $88 L BLTU,
  A11 A9 MOV.N,  A8 6 $88 L BLTUI,
  A11 A2 MOV.N,  $88 L J,
  1 ZEROS,
$86 L:
  A11 2 MOVI.N,
$88 L:
  A8 $8 L L32R,  A8 CALLX8,
$8E L:
  A2 $0 L L32R,  A9 A2 0 L32I.N,  A2 A9 0 L32I.N,  A2 $29 L BNEZ,
  A8 A9 32 ADDI,  A2 4 MOVI.N,  A2 $A8 L LOOP,
  A10 A8 0 L32I.N,  A10 $1C L BNEZ,
  A8 A8 48 ADDI,
$A8 L:
  A2 A9 $334 L32I,  A2 $1C L BNEZ,
  A8 A9 $33C L32I,  A8 $1C L BNEZ,
  A2 1 MOVI.N,  A9 A9 $344 L32I,  A2 A8 A9 MOVEQZ,
$BC L:
  RETW.N,
$BE L:  END-CODE

CODE ppMapTxQueue   \ .wifi0iram.65 $20
  $0 8 LITERALS
    $FF0FFFFF T,  SYMBOL g_mesh_is_started  SYMBOL ppProcessWaitingQueue
    SYMBOL pm_on_data_tx  SYMBOL pm_on_data_tx  SYMBOL ppSearchTxQueue
    SYMBOL pm_on_data_tx  SYMBOL ppSearchTxQueue
$20 L:
  A1 48 ENTRY,  A4 A2 MOV.N,  A7 A2 36 L32I.N,  A2 A2 4 L32I.N,  A9 A2 4 L32I.N,
  A2 A4 44 L32I.N,  A2 A2 0 L32I.N,  A2 18 $34 L BBCI,
  A9 A9 4 ADDI.N,
$34 L:
  A2 240 MOVI,  A9 A1 0 S32I.N,  MEMW,  A8 $8 L L32R,  A8 CALLX8,
  A3 A4 44 L32I.N,  A12 A3 4 L32I.N,  A2 A2 A12 AND,  A9 A1 0 L32I.N,
  A2 64 $66 L BNEI,
  A2 A3 16 L32I.N,  A4 $0 L L32R,  A2 A2 A4 AND,  A4 3 MOVI.N,  A4 A4 20 SLLI,
  A2 A2 A4 OR,  A2 A3 16 S32I.N,  MEMW,  $A0 L J,
$66 L:
  A10 A3 16 L32I.N,  A8 A10 18 2 EXTUI,  A5 A8 MOV.N,  A6 A9 0 L8UI,
  A7 $78 L BEQZ.N,
  A2 A7 11 L8UI,  A2 7 $A6 L BBCI,
$78 L:
  A2 7 MOVI.N,  A8 A8 0 2 EXTUI,  A4 $0 L L32R,  A8 A8 20 SLLI,  A11 0 MOVI.N,
  A10 A5 MOV.N,  A2 A3 4 S8I,  MEMW,  A2 A3 16 L32I.N,  A2 A2 A4 AND,  A2 A2 A8 OR,
  A2 A3 16 S32I.N,  MEMW,  A8 $C L L32R,  A8 CALLX8,
$A0 L:
  A2 0 MOVI.N,  $1B1 L J,
  1 ZEROS,
$A6 L:
  A7 12 MOVI.N,  A7 A6 A7 AND,  A7 $112 L BNEZ,
  A2 7 MOVI.N,  A2 A3 4 S8I,  MEMW,  A8 A8 0 2 EXTUI,  A8 A8 20 SLLI,  A11 1 MOVI.N,
  A2 $0 L L32R,  A2 A10 A2 AND,  A2 A2 A8 OR,  A10 A5 MOV.N,  A2 A3 16 S32I.N,
  A2 A7 MOV.N,  A9 A1 0 S32I.N,  MEMW,  A8 $10 L L32R,  A8 CALLX8,
  A9 A1 0 L32I.N,  A10 $1B1 L BEQZ,
  A3 $4 L L32R,  A3 A3 0 L8UI,  A3 $10C L BNEZ.N,
  A3 -16 MOVI.N,  A3 A6 A3 AND,  A3 64 $101 L BNEI,
  A3 A9 1 L8UI,  A4 16 MOVI.N,  A3 A3 A4 OR,  A3 A9 1 S8I,  MEMW,  MEMW,  $1B1 L J,
$101 L:
  A3 A4 44 L32I.N,  A3 A3 2 L16UI,  A3 0 $10C L BBCI,
  $1B1 L J,
$10C L:
  A2 1 MOVI.N,  $1B1 L J,
  1 ZEROS,
$112 L:
  A2 A3 0 L32I.N,  A9 1 MOVI.N,  A9 A9 13 SLLI,  A2 A2 A9 AND,  A2 $159 L BEQZ.N,
  A2 A4 36 L32I.N,  A13 A12 4 4 EXTUI,  A10 A8 MOV.N,  A12 A12 0 4 EXTUI,
  A11 A2 128 L8UI,  A8 $14 L L32R,  A8 CALLX8,
  A10 16 $10C L BEQI,
  A10 A10 0 4 EXTUI,  A2 A3 16 L32I.N,  A10 A10 20 SLLI,  A11 1 MOVI.N,
  A4 $0 L L32R,  A2 A2 A4 AND,  A2 A2 A10 OR,  A10 A5 MOV.N,  A2 A3 16 S32I.N,
  MEMW,  A8 $18 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  $1B1 L J,
$159 L:
  A7 8 $185 L BNEI,
  A7 112 MOVI,  A6 A6 A7 AND,  A6 64 $185 L BNEI,
  A4 7 MOVI.N,  A4 A3 4 S8I,  MEMW,  A9 A8 0 2 EXTUI,  A9 A9 20 SLLI,  A4 $0 L L32R,
  A8 A10 A4 AND,  A8 A8 A9 OR,  A8 A3 16 S32I.N,  MEMW,  $1B1 L J,
  1 ZEROS,
$185 L:
  A4 A4 36 L32I.N,  A13 A12 4 4 EXTUI,  A10 A5 MOV.N,  A12 A12 0 4 EXTUI,
  A11 A4 128 L8UI,  A8 $1C L L32R,  A8 CALLX8,
  A10 16 $19E L BNEI,
  $10C L J,
$19E L:
  A10 A10 0 4 EXTUI,  A4 A3 16 L32I.N,  A10 A10 20 SLLI,  A5 $0 L L32R,
  A4 A4 A5 AND,  A4 A4 A10 OR,  A4 A3 16 S32I.N,
$1B1 L:
  MEMW,  RETW.N,
$1B6 L:  END-CODE

CODE ppDiscardMPDU   \ .text.ppDiscardMPDU $8
  $0 2 LITERALS
    SYMBOL ppEnqueueTxDone  SYMBOL pp_post
$8 L:
  A1 32 ENTRY,  A8 A2 44 L32I.N,  A3 0 MOVI.N,  A9 4 MOVI.N,  A10 A2 MOV.N,
  A3 A8 20 S32I.N,  A9 A8 19 S8I,  MEMW,  MEMW,  A8 $0 L L32R,  A8 CALLX8,
  A11 A3 MOV.N,  A10 16 MOVI.N,  A8 $4 L L32R,  A8 CALLX8,
  RETW.N,
$30 L:  END-CODE

CODE ppProcTxSecFrame   \ .wifi0iram.68 $1C
  $0 7 LITERALS
    SYMBOL rodata.CSWTCH$606  $FF000FFF T,  $1166 T,
    SYMBOL rodata.pp_default_event_ha~8FBB  SYMBOL rodata_wlog_error.69
    $FFC80FFF T,  SYMBOL wifi_log
$1C L:
  A1 32 ENTRY,  A12 A2 44 L32I.N,  A11 4 MOVI.N,  A8 A12 16 L32I.N,
  A8 A8 8 4 EXTUI,  A8 A8 -1 ADDI.N,  A8 A8 0 8 EXTUI,  A10 A2 36 L32I,
  A8 A11 $3C L BGEU,
  A9 $0 L L32R,  A8 A9 A8 ADD,  A11 A8 0 L8UI,
$3C L:
  A8 A2 22 L16UI,  A8 A11 A8 ADD.N,  A8 A8 0 16 EXTUI,  A13 A2 8 L32I.N,  MEMW,
  A9 A13 0 L32I.N,  A9 A9 12 12 EXTUI,  A9 A9 A11 ADD.N,  A9 A9 0 12 EXTUI,
  A8 A2 22 S16I,  MEMW,  A9 A9 12 SLLI,  MEMW,  A11 A13 0 L32I.N,  A14 $4 L L32R,
  A11 A11 A14 AND,  A9 A11 A9 OR,  A11 1 MOVI.N,  MEMW,  A9 A13 0 S32I.N,
  A9 17 MOVI.N,  A9 A9 14 SLLI,  A14 A12 0 L32I.N,  A14 A14 A9 AND,  A9 1 MOVI.N,
  A9 A9 18 SLLI,  A14 A9 $86 L BEQ,
  $122 L J,
$86 L:
  A10 $13A L BEQZ,
  A14 A2 4 L32I.N,  A13 A11 29 SLLI,  MEMW,  A9 A14 0 L32I.N,  A9 A9 A13 OR,
  A13 11 MOVI.N,  A13 A13 8 SLLI,  MEMW,  A9 A14 0 S32I.N,  A9 15 MOVI.N,
  A9 A9 8 SLLI,  A12 A12 16 L32I.N,  A13 A12 A13 AND,  A9 A12 A9 AND,  A12 0 MOVI.N,
  A15 A12 MOV.N,  A15 A11 A13 MOVNEZ,  A13 A9 -768 ADDMI,  A12 A11 A13 MOVNEZ,
  A15 A12 $E0 L BNONE,
  A9 A9 -2304 ADDMI,  A9 $E0 L BEQZ.N,
  A15 $8 L L32R,  A12 A11 MOV.N,  A10 6 MOVI,  A11 A11 11 SLLI,  A14 $C L L32R,
  A13 $10 L L32R,  MEMW,  A8 $18 L L32R,  A8 CALLX8,
$DC L:
  $DC L J,
  1 ZEROS,
$E0 L:
  A2 A2 20 L16UI,  A11 -1 MOVI.N,  A2 A2 -4 ADDI,  A8 A8 A2 ADD.N,  A11 A11 12 SLLI,
  A9 A14 4 L32I.N,  A12 A8 0 12 EXTUI,  A8 0 MOVI.N,  A2 A9 0 L32I.N,
  A2 A2 A11 AND,  A2 A2 A12 OR,  A2 A9 0 S32I.N,  A2 A10 126 L16UI,
  A10 A12 3 ADDI.N,  A10 A2 $10C L BGE,
  A2 A2 A12 SUB,  A8 A2 2 7 EXTUI,
$10C L:
  A2 A9 0 L32I.N,  A8 A8 12 SLLI,  A10 $14 L L32R,  A2 A2 A10 AND,  A2 A2 A8 OR,
  A2 A9 0 S32I.N,  MEMW,  $132 L J,
$122 L:
  MEMW,  A8 A13 0 L32I.N,  A11 A11 30 SLLI,  A8 A8 A11 OR,  MEMW,  A8 A13 0 S32I.N,
$132 L:
  A2 0 MOVI.N,  MEMW,  $13C L J,
$13A L:
  A2 A11 MOV.N,
$13C L:
  RETW.N,
$13E L:  END-CODE

CODE ppTxPkt   \ .wifi0iram.12 $64
  $0 25 LITERALS
    SYMBOL rodata_wlog_warning.13  SYMBOL rodata.pp_default_event_ha~8FBB
    SYMBOL rodata_wlog_error.14  SYMBOL g_lmac_cnt  SYMBOL g_mesh_is_started
    SYMBOL g_mesh_is_root  SYMBOL g_mesh_init_ps_type  SYMBOL g_osi_funcs_p
    $3FF73C00 T,  SYMBOL g_wifi_mac_time_delta  SYMBOL pTxRx
    SYMBOL rodata_wlog_warning.15  SYMBOL ic_interface_enabled  SYMBOL wifi_log
    SYMBOL wifi_log  SYMBOL ppTxProtoProc  SYMBOL ppProcTxSecFrame
    SYMBOL rcGetSched  SYMBOL ppCalFrameTimes  SYMBOL ppMapTxQueue
    SYMBOL lmacIsIdle  SYMBOL pp_post  SYMBOL wifi_log  SYMBOL esf_buf_recycle
    SYMBOL pp_process_hmac_waiting_txq
$64 L:
  A1 32 ENTRY,  A4 A2 44 L32I.N,  A3 A3 0 8 EXTUI,  A10 A4 16 L32I,
  A10 A10 18 2 EXTUI,  A8 $30 L L32R,  A8 CALLX8,
  A4 A2 44 L32I.N,  A10 $94 L BNEZ.N,
  A12 2 MOVI.N,  A11 64 MOVI.N,  A10 6 MOVI.N,  A14 A4 16 L32I.N,
  A14 A14 18 2 EXTUI,  A13 $0 L L32R,  A8 $34 L L32R,  A8 CALLX8,
  $238 L J,
  1 ZEROS,
$94 L:
  A10 A4 4 L32I.N,  A8 4 MOVI.N,  A4 A10 0 4 EXTUI,  A4 8 $B9 L BGEUI,
  A9 9 MOVI.N,  A4 SSR,  A9 A9 SRL,  A9 A9 0 1 EXTUI,  A8 2 MOVI.N,
  A9 $B9 L BNEZ.N,
  A8 3 MOVI.N,  A4 A8 $B9 L BLTU,
  A8 1 MOVI.N,  A4 6 $B9 L BLTUI,
  A8 A9 MOV.N,
$B9 L:
  A10 A10 4 4 EXTUI,  A10 A8 $D8 L BEQ,
  A12 1 MOVI.N,  A14 $4 L L32R,  A15 $38F MOVI,  A11 A12 11 SLLI,  A10 6 MOVI.N,
  A13 $8 L L32R,  A8 $38 L L32R,  A8 CALLX8,
$D5 L:
  $D5 L J,
$D8 L:
  A4 $C L L32R,  A8 A8 14 ADDI.N,  A8 A8 A4 ADDX8,  A10 A2 MOV.N,  A4 A8 8 L32I.N,
  A4 A4 1 ADDI.N,  A4 A8 8 S32I.N,  MEMW,  A8 $3C L L32R,  A8 CALLX8,
  A10 A2 MOV.N,  A8 $40 L L32R,  A8 CALLX8,
  A10 1 $FF L BNEI,
  $238 L J,
$FF L:
  A11 A2 44 L32I.N,  A10 A2 36 L32I.N,  A8 $44 L L32R,  A8 CALLX8,
  A10 A2 MOV.N,  A8 $48 L L32R,  A8 CALLX8,
  A10 A2 MOV.N,  A8 $4C L L32R,  A8 CALLX8,
  A10 $1B5 L BNEZ,
  A4 $10 L L32R,  A4 A4 0 L8UI,  A4 $148 L BEQZ.N,
  A4 $14 L L32R,  MEMW,  A4 A4 0 L8UI,  A4 A4 0 8 EXTUI,  A4 $148 L BNEZ.N,
  A4 $18 L L32R,  A4 A4 0 L32I.N,  A4 $148 L BEQZ.N,
  A4 $1C L L32R,  A4 A4 0 L32I.N,  A10 A4 $110 L32I,  A10 CALLX8,
  $157 L J,
  1 ZEROS,
$148 L:
  A4 $20 L L32R,  MEMW,  A10 A4 0 L32I.N,  A4 $24 L L32R,  A4 A4 0 L32I.N,
  A10 A10 A4 ADD.N,
$157 L:
  A4 0 MOVI.N,  A9 A2 44 L32I.N,  A8 $28 L L32R,  A10 A9 24 S32I.N,
  A10 A8 0 L32I.N,  A8 A9 16 L32I.N,  A8 A8 20 4 EXTUI,  A8 A8 A8 ADDX2,
  A8 A8 4 SLLI,  A8 A10 A8 ADD.N,  A8 A8 36 L32I.N,  A4 A2 40 S32I.N,
  A2 A8 0 S32I.N,  A8 A9 16 L32I.N,  A8 A8 20 4 EXTUI,  A8 A8 A8 ADDX2,
  A8 A8 4 SLLI,  A10 A10 A8 ADD.N,  A8 A2 40 ADDI,  A8 A10 36 S32I.N,
  A3 A4 $192 L BNE,
$18A L:
  A2 0 MOVI.N,  MEMW,  $24E L J,
$192 L:
  A10 A9 4 L32I.N,  A10 A10 4 4 EXTUI,  A8 $50 L L32R,  A8 CALLX8,
  A10 $18A L BEQZ,
  A2 A2 44 L32I.N,  A11 A4 MOV.N,  A10 A2 4 L32I,  A10 A10 4 4 EXTUI,
  A8 $54 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  $24E L J,
$1B5 L:
  A10 1 $221 L BNEI,
  A3 $10 L L32R,  A3 A3 0 L8UI,  A3 $1E4 L BEQZ.N,
  A3 $14 L L32R,  MEMW,  A3 A3 0 L8UI,  A3 A3 0 8 EXTUI,  A3 $1E4 L BNEZ.N,
  A3 $18 L L32R,  A3 A3 0 L32I.N,  A3 $1E4 L BEQZ.N,
  A3 $1C L L32R,  A3 A3 0 L32I.N,  A10 A3 $110 L32I,  A10 CALLX8,
  $1F3 L J,
  1 ZEROS,
$1E4 L:
  A3 $20 L L32R,  MEMW,  A10 A3 0 L32I.N,  A3 $24 L L32R,  A3 A3 0 L32I.N,
  A10 A10 A3 ADD.N,
$1F3 L:
  A4 A2 44 L32I.N,  A3 $28 L L32R,  A8 A3 0 L32I.N,  A3 A4 16 L32I.N,
  A3 A3 18 2 EXTUI,  A3 A3 A8 ADDX8,  A10 A4 24 S32I.N,  A10 0 MOVI.N,
  A3 A3 $338 L32I,  A10 A2 40 S32I.N,  A2 A3 0 S32I.N,  A2 A2 40 ADDI,
  A3 A4 16 L32I.N,  A3 A3 18 2 EXTUI,  A3 A3 A8 ADDX8,  A2 A3 $338 S32I,  MEMW,
  $18A L J,
$221 L:
  A10 2 $227 L BEQI,
  $18A L J,
$227 L:
  A13 $2C L L32R,  A12 A10 A10 OR,  A11 64 MOVI,  A10 6 MOVI.N,  A8 $58 L L32R,
  A8 CALLX8,
$238 L:
  A3 A2 26 L8UI,  A10 A2 MOV.N,  A2 1 MOVI.N,  A8 $5C L L32R,  A8 CALLX8,
  A3 A2 $24E L BNE,
  A8 $60 L L32R,  A8 CALLX8,
$24E L:
  RETW.N,
$250 L:  END-CODE

CODE ppPrepareBarFrame   \ .text.ppPrepareBarFrame $1C
  $0 7 LITERALS
    SYMBOL rodata_wlog_warning.31  SYMBOL bars  SYMBOL esf_buf_alloc
    SYMBOL wifi_log  SYMBOL rc_get_trc_by_index  SYMBOL rcGetSched
    SYMBOL ppCalFrameTimes
$1C L:
  A1 32 ENTRY,  A12 0 MOVI.N,  A11 5 MOVI,  A10 A12 MOV.N,  A8 $8 L L32R,
  A8 CALLX8,
  A3 A2 0 8 EXTUI,  A2 A10 MOV.N,  A10 $48 L BNEZ.N,
  A13 $0 L L32R,  A12 2 MOVI.N,  A11 64 MOVI,  A10 6 MOVI,  A8 $C L L32R,
  A8 CALLX8,
  $D3 L J,
  1 ZEROS,
$48 L:
  A8 $4 L L32R,  A9 A3 A3 ADDX8,  A9 A9 A8 ADDX4,  A4 24 MOVI.N,  A3 A3 0 4 EXTUI,
  A4 A10 22 S16I,  MEMW,  A4 A9 MOV.N,  A3 A3 4 SLLI,  A11 0 MOVI.N,
  A9 A10 4 S32I.N,  A10 -1 MOVI.N,  A10 A10 12 SLLI,  MEMW,  A8 A9 0 L32I.N,
  A8 A8 A10 AND,  A10 24 MOVI.N,  A8 A8 A10 OR,  MEMW,  A8 A9 0 S32I.N,
  A8 1 MOVI.N,  A10 -241 MOVI,  A9 A2 8 S32I.N,  A9 A2 44 L32I.N,  A8 A2 12 S16I,
  MEMW,  A8 A9 4 L32I.N,  A8 A8 A10 AND,  A3 A8 A3 OR,  A10 A11 MOV.N,
  A3 A9 4 S32I.N,  MEMW,  A8 $10 L L32R,  A8 CALLX8,
  A11 A2 44 L32I.N,  A10 A2 36 S32I.N,  MEMW,  A8 $14 L L32R,  A8 CALLX8,
  A10 A2 MOV.N,  A8 $18 L L32R,  A8 CALLX8,
  A9 -7 MOVI.N,  A3 A2 22 L16UI,  A9 A9 19 SLLI,  A3 A3 0 12 EXTUI,
  A8 A4 12 L32I.N,  A8 A8 A9 AND,  A3 A3 A8 OR,  A3 A4 12 S32I.N,  A3 132 MOVI,
  A3 A4 16 S16I,  MEMW,
$D3 L:
  MEMW,  RETW.N,
$D8 L:  END-CODE

CODE pp_attach   \ .text.pp_attach $30
  $0 12 LITERALS
    SYMBOL pp_sig_cnt  SYMBOL bars  SYMBOL TxRxCxt  SYMBOL pTxRx  SYMBOL memset
    SYMBOL memset  SYMBOL memset  SYMBOL dbg_lmac_init  SYMBOL ppInitTxq
    SYMBOL esf_buf_setup  SYMBOL pp_deattach  SYMBOL ppPrepareBarFrame
$30 L:
  A1 32 ENTRY,  A10 $0 L L32R,  A12 30 MOVI.N,  A11 0 MOVI,  A8 $10 L L32R,
  A8 CALLX8,
  A12 144 MOVI,  A10 $4 L L32R,  A11 0 MOVI,  A8 $14 L L32R,  A8 CALLX8,
  A12 $3CC MOVI,  A11 0 MOVI.N,  A10 $8 L L32R,  A5 $640 MOVI,  A8 $18 L L32R,
  A8 CALLX8,
  A6 200 MOVI,  A4 $C L L32R,  A3 A4 0 L32I.N,  A5 A3 $3C4 S32I,  MEMW,
  A8 $1C L L32R,  A8 CALLX8,
  A8 $20 L L32R,  A8 CALLX8,
  A9 $334 MOVI,  A3 0 MOVI.N,  A8 A4 0 L32I.N,  A10 A2 MOV.N,  A9 A8 A9 ADD.N,
  A9 A8 $338 S32I,  A9 $33C MOVI,  A9 A8 A9 ADD.N,  A9 A8 $340 S32I,  A9 $344 MOVI,
  A9 A8 A9 ADD.N,  A9 A8 $348 S32I,  A9 $34C MOVI,  A9 A8 A9 ADD.N,
  A9 A8 $350 S32I,  A9 $354 MOVI,  A9 A8 A9 ADD.N,  A3 A8 $334 S32I,
  A3 A8 $33C S32I,  A3 A8 $344 S32I,  A3 A8 $34C S32I,  A3 A8 $354 S32I,
  A9 A8 $358 S32I,  MEMW,  A8 $24 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  A10 A3 $D0 L BEQ,
  A8 $28 L L32R,  A8 CALLX8,
  $101 L J,
  1 ZEROS,
$D0 L:
  A10 A3 MOV.N,  A5 A4 0 L32I.N,  A8 $2C L L32R,  A8 CALLX8,
  A8 A3 A6 ADD.N,  A8 A8 A5 ADDX4,  A3 A3 1 ADDI.N,  A10 A8 0 S32I.N,
  A3 4 $D0 L BNEI,
  A9 $100 MOVI,  A3 A4 0 L32I.N,  A4 1 MOVI.N,  A8 A3 $300 ADDMI,  A4 A3 29 S8I,
  A4 0 MOVI.N,  A4 A3 $330 S32I,  A9 A8 96 S16I,  A4 A3 30 S8I,  MEMW,
$101 L:
  MEMW,  RETW.N,
$106 L:  END-CODE

CODE ppDirectRecycleAmpdu   \ .wifiextrairam.71 $C
  $0 3 LITERALS
    SYMBOL pTxRx  SYMBOL ppResumeTxAMPDU  SYMBOL esf_buf_recycle
$C L:
  A1 32 ENTRY,  A3 A2 44 L32I.N,  A10 A3 16 L32I.N,  A10 A10 20 4 EXTUI,
  A8 A10 1 ADDI.N,  A8 A8 A8 ADDX2,  A3 $0 L L32R,  A8 A8 4 SLLI,  A3 A3 0 L32I.N,
  A8 A3 A8 ADD.N,  A3 0 MOVI.N,  A3 A8 4 S32I.N,  MEMW,  A8 $4 L L32R,  A8 CALLX8,
$32 L:
  A10 A2 MOV.N,  A9 1 MOVI.N,  A9 A9 22 SLLI,  A8 A10 8 L32I.N,  A11 A10 44 L32I.N,
  A2 A2 40 L32I.N,  A3 A10 40 S32I.N,  A3 A8 8 S32I.N,  A8 A11 0 L32I.N,
  A8 A8 A9 OR,  A8 A11 0 S32I,  MEMW,  A8 $8 L L32R,  A8 CALLX8,
  A2 $32 L BNEZ,
  RETW.N,
$59 L:  END-CODE

CODE ppClearTxq   \ .text.ppClearTxq $10
  $0 4 LITERALS
    SYMBOL pTxRx  SYMBOL ppDirectRecycleAmpdu  SYMBOL esf_buf_recycle
    SYMBOL ppDequeueTxQ
$10 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A2 0 8 EXTUI,  A9 A8 0 L32I.N,  A8 A2 A2 ADDX2,
  A8 A8 4 SLLI,  A8 A9 A8 ADD.N,  A8 A8 41 L8UI,  A8 $45 L BNEZ.N,
  A8 A2 1 ADDI.N,  A8 A8 A8 ADDX2,  A8 A8 4 SLLI,  A9 A9 A8 ADD.N,  A10 A9 4 L32I.N,
  A10 $45 L BEQZ.N,
  A8 $4 L L32R,  A8 CALLX8,
  $45 L J,
$3F L:
  A8 $8 L L32R,  A8 CALLX8,
$45 L:
  A10 A2 MOV.N,  A8 $C L L32R,  A8 CALLX8,
  A10 $3F L BNEZ,
  RETW.N,
$52 L:  END-CODE

CODE pp_stop_sw_txq   \ .text.pp_stop_sw_txq $18
  $0 6 LITERALS
    SYMBOL pTxRx  SYMBOL esf_buf_recycle  SYMBOL ppClearTxq
    SYMBOL esf_buf_recycle  SYMBOL ppDequeueTxDone_Locked
    SYMBOL ppClearRxFragment
$18 L:
  A1 32 ENTRY,  A2 0 MOVI.N,  A4 $334 MOVI,  $4F L J,
  1 ZEROS,
$24 L:
  A11 A10 40 L32I.N,  A11 A9 $334 S32I,  A11 $30 L BNEZ.N,
  A8 A8 A3 ADD.N,  A8 A9 $338 S32I,
$30 L:
  A8 0 MOVI.N,  A8 A10 40 S32I.N,  MEMW,  A8 $4 L L32R,  A8 CALLX8,
$3D L:
  A6 $0 L L32R,  A8 A6 0 L32I.N,  A9 A8 A5 ADD.N,  A10 A9 $334 L32I,
  A10 $24 L BNEZ,
  A2 A2 1 ADDI.N,  A2 3 $58 L BEQI,
$4F L:
  A5 A2 3 SLLI,  A3 A2 A4 ADDX8,  $3D L J,
$58 L:
  A2 A10 MOV.N,
$5A L:
  A10 A2 MOV.N,  A2 A2 1 ADDI.N,  A8 $8 L L32R,  A8 CALLX8,
  A2 16 $5A L BNEI,
  $72 L J,
  2 ZEROS,
$6C L:
  A8 $C L L32R,  A8 CALLX8,
$72 L:
  A8 $10 L L32R,  A8 CALLX8,
  A10 $6C L BNEZ,
  A8 $14 L L32R,  A8 CALLX8,
  RETW.N,
$83 L:  END-CODE

CODE ppRecycleAmpdu   \ .wifiextrairam.72 $14
  $0 5 LITERALS
    SYMBOL pTxRx  SYMBOL GetAccess  SYMBOL ppResumeTxAMPDU
    SYMBOL lmacDiscardAgedMSDU  SYMBOL lmacDiscardAgedMSDU
$14 L:
  A1 32 ENTRY,  A4 A2 44 L32I.N,  A3 A3 0 8 EXTUI,  A10 A4 4 L32I.N,
  A10 A10 4 4 EXTUI,  A5 A4 16 L32I.N,  A8 $4 L L32R,  A8 CALLX8,
  A5 A5 20 4 EXTUI,  A8 A5 1 ADDI.N,  A8 A8 A8 ADDX2,  A4 $0 L L32R,  A8 A8 4 SLLI,
  A6 A10 MOV.N,  A10 A5 MOV.N,  A4 A4 0 L32I.N,  A8 A4 A8 ADD.N,  A4 0 MOVI.N,
  A4 A8 4 S32I,  MEMW,  A8 $8 L L32R,  A8 CALLX8,
$4D L:
  A8 1 MOVI.N,  A8 A8 22 SLLI,  A5 A2 8 L32I.N,  A9 A2 44 L32I.N,  A7 A2 40 L32I.N,
  A4 A2 40 S32I.N,  A4 A5 8 S32I.N,  A5 A9 0 L32I.N,  A5 A5 A8 OR,  A5 A9 0 S32I.N,
  A7 $79 L BEQZ.N,
  A11 A2 MOV.N,  A12 A4 MOV.N,  A10 A6 MOV.N,  MEMW,  A8 $C L L32R,  A8 CALLX8,
  A2 A7 MOV.N,  $4D L J,
$79 L:
  A12 A3 MOV.N,  A11 A2 MOV.N,  A10 A6 MOV.N,  A8 $10 L L32R,  A8 CALLX8,
  RETW.N,
$87 L:  END-CODE

CODE ppRegressAmpdu   \ .wifiextrairam.73 $10
  $0 4 LITERALS
    SYMBOL pTxRx  SYMBOL ppAMPDU2Normal  SYMBOL ppResumeTxAMPDU  SYMBOL pp_post
$10 L:
  A1 32 ENTRY,  A4 A2 44 L32I.N,  A3 A3 0 8 EXTUI,  A5 A2 MOV.N,  A9 0 MOVI.N,
  A4 A4 16 L32I.N,  A4 A4 20 4 EXTUI,  $2C L J,
  2 ZEROS,
$26 L:
  A5 A5 8 L32I.N,  A9 A5 8 S32I.N,  A5 A8 MOV.N,
$2C L:
  A8 A5 40 L32I.N,  A8 $26 L BNEZ,
  A10 A2 MOV.N,  MEMW,  A8 $4 L L32R,  A8 CALLX8,
  A8 $0 L L32R,  A9 A4 A4 ADDX2,  A9 A9 4 SLLI,  A10 A8 0 L32I.N,  A8 A4 A4 ADD.N,
  A9 A10 A9 ADD.N,  A11 A9 32 L32I.N,  A11 A5 40 S32I.N,  A11 $56 L BNEZ.N,
  A5 A5 40 ADDI,  A5 A9 36 S32I.N,
$56 L:
  A8 A8 A4 ADD.N,  A5 A4 1 ADDI.N,  A8 A8 4 SLLI,  A5 A5 A5 ADDX2,  A8 A10 A8 ADD.N,
  A5 A5 4 SLLI,  A10 A10 A5 ADD.N,  A2 A8 32 S32I.N,  A5 0 MOVI.N,  A5 A10 4 S32I.N,
  A10 A4 MOV.N,  MEMW,  A8 $8 L L32R,  A8 CALLX8,
  A3 A5 $8A L BEQ,
  A11 A5 MOV.N,  A2 A2 44 L32I.N,  A10 A2 4 L32I.N,  A10 A10 4 4 EXTUI,
  A8 $C L L32R,  A8 CALLX8,
$8A L:
  RETW.N,
$8C L:  END-CODE

CODE ppGetTaskHdl   \ .text.ppGetTaskHdl $4
  $0 1 LITERALS
    SYMBOL bss.pp_task_hdl
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A8 0 L32I.N,  RETW.N,
$E L:  END-CODE

CODE ppMessageInQ   \ .text.ppMessageInQ $8
  $0 2 LITERALS
    SYMBOL xphyQueue  SYMBOL g_osi_funcs_p
$8 L:
  A1 32 ENTRY,  A8 $4 L L32R,  A9 $0 L L32R,  A8 A8 0 L32I.N,  A10 A9 0 L32I,
  A8 A8 120 L32I,  A8 CALLX8,
  A2 A10 MOV.N,  RETW.N,
$20 L:  END-CODE

CODE ppProcessLifeTime   \ .wifi0iram.74 $1C
  $0 7 LITERALS
    SYMBOL lmacConfMib  $10020 T,  SYMBOL g_offchan_packet_lifetime
    SYMBOL rodata.pp_default_event_ha~8FBB  SYMBOL rodata_wlog_error.75
    SYMBOL pm_get_null_max_tx_time  SYMBOL wifi_log
$1C L:
  A1 32 ENTRY,  A13 A2 44 L32I.N,  A8 A13 0 L32I.N,  A9 $0 L L32R,
  A8 22 $2E L BBCI,
  A10 A9 0 L32I.N,  $30 L J,
$2E L:
  A10 A9 8 L32I.N,
$30 L:
  A10 A10 10 SLLI,  A9 $4 L L32R,  A8 A9 $AD L BNONE,
  A2 A2 4 L32I.N,  A11 A2 4 L32I.N,  A8 18 $42 L BBCI,
  A11 A11 4 ADDI.N,
$42 L:
  A2 -16 MOVI.N,  A12 12 MOVI.N,  A8 A11 0 L8UI,  A9 1 MOVI.N,  A12 A8 A12 AND,
  A8 A8 A2 AND,  A2 0 MOVI,  A9 A2 A12 MOVNEZ,  A9 A9 0 8 EXTUI,  A8 64 $60 L BNEI,
  A9 A2 $A8 L BNE,
$60 L:
  A12 8 $74 L BNEI,
  A10 A11 1 L8UI,  A10 A10 4 1 EXTUI,  A11 A13 24 L32I.N,  A8 $14 L L32R,
  A8 CALLX8,
  $AD L J,
$74 L:
  A2 -208 MOVI,  A8 A8 A2 ADD.N,  A8 $8C L BNEZ.N,
  A9 $8C L BEQZ.N,
  A2 $8 L L32R,  A8 A2 0 L32I.N,  A8 $AD L BEQZ.N,
  A10 A8 10 SLLI,  $AD L J,
  2 ZEROS,
$8C L:
  A15 $491 MOVI,  A12 1 MOVI.N,  A14 $C L L32R,  A15 A15 2 SLLI,  A11 A12 11 SLLI,
  A10 6 MOVI.N,  A13 $10 L L32R,  A8 $18 L L32R,  A8 CALLX8,
$A5 L:
  $A5 L J,
$A8 L:
  A10 15 MOVI.N,  A10 A10 13 SLLI,
$AD L:
  A10 A4 0 S32I.N,  MEMW,  RETW.N,
$B4 L:  END-CODE

\ ==== Addresses in the data ============================================

0 pTxRx SYMBOL! TxRxCxt
0 libpp_reversion_git SYMBOL! rodata.str1.1

LINK-SYMBOLS   \ the cells of other packages that wait for these names
END-PACKAGE  PREVIOUS   \ not in the search order: ALSO name gives its words (README.md)
