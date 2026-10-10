\ lmac.fs - package PP-LMAC: lmac.o of libpp.a (ESP32)
\
\ Made from lmac.S by s2fs.py, not by hand. Each function of C is a
\ CODE word: its bytes are those of the compiler, at the same offsets of
\ its section (L gives the address of an offset); its literals are at the
\ start of the word, after a jump. The data come first, then the
\ functions, each after those that it calls. The global symbols are
\ PUBLIC, the static ones PRIVATE. lib/esp32/clink.fs explains the words.
\
\ 57 functions (56 global; 1 static without symbol, named after their
\ section), 58 data objects.
\ A function runs as C code with the windowed calls of the Xtensa: call
\ it with CALL-C ( x1 .. xn n xt -- x ), never by its name alone.
\
\ Names of other packages: a literal waits until its package comes
\ (LINK-SYMBOLS); a name outside the libraries until EXTERN gives it.
\ Uses:
\   PHY-PHY-CHIP-V7-CAL  tx_pwctrl_background
\   PP-ESF-BUF  esf_buf_recycle
\   PP-HAL-MAC  hal_mac_is_txq_valid hal_mac_set_txq_invalid
\               hal_mac_tx_config_edca hal_mac_tx_config_timeout
\               hal_mac_tx_get_blockack hal_mac_tx_set_cca
\               hal_mac_tx_set_ppdu hal_mac_txq_disable
\               hal_mac_txq_enable hal_random
\   PP-HAL-MAC-TX  hal_mac_clr_txq_state hal_mac_get_txq_pmd
\                  hal_mac_get_txq_state hal_mac_tx_is_cbw40
\   PP-PM  pm_is_sleeping
\   PP-PP  pTxRx ppCalFrameTimes ppDequeueTxQ ppDirectRecycleAmpdu
\          ppEnqueueRxq ppEnqueueTxDone ppFillAMPDUBar ppProcTxCallback
\          ppProcessLifeTime ppProcessTxQ ppReSendBar ppRecordBarRRC
\          ppRecycleAmpdu ppRegressAmpdu ppResortTxAMPDU
\          ppTxqUpdateBitmap pp_post
\   PP-PP-DEBUG  g_lmac_cnt wifi_gpio_debug
\   PP-RATE-CONTROL  RC_SetBasicRate
\   PP-TRC  rcAttach rcGetRate rcReachRetryLimit rcUpdateTxDone
\           trc_isTxAmpduOperational
\   PP-WDEV  wDev_isNANPktInValidSlot wDev_is_low_rate_enable
\ Outside the libraries (../README.md):
\   ets_delay_us g_mesh_init_ps_type g_mesh_is_root g_mesh_is_started
\   g_osi_funcs_p g_wifi_mac_time_delta memcpy memset offchan_tx_progress_in
\   wifi_log
\ C names with more than 31 characters (Forth takes 31): the first 26,
\ ~ and 4 hex digits of their CRC-32 (../README.md):
\   esp_wifi_internal_set_msdu~671D = esp_wifi_internal_set_msdu_lifetime
\   esp_wifi_internal_set_retr~3DF8 = esp_wifi_internal_set_retry_counter
\   lmacDiscardFrameExchangeSe~E31C = lmacDiscardFrameExchangeSequence
\   rodata.lmacProcessTxopSucc~5667 = rodata.lmacProcessTxopSuccess.str1.1
\
\ Needs XTENSA (asm/xtensa.fs), ASM (lib/esp32/asm.fs) and CLINK
\ (lib/esp32/clink.fs) of ucf; the ESP32.

PACKAGE PP-LMAC  ALSO ASM  ALSO CLINK   \ lib/package.fs
EXPORTS   \ the PUBLIC words are symbols for the other packages

\ ==== Data =============================================================

PRIVATE
CREATE rodata.lmacProcessTxopSucc~5667   \ .rodata.lmacProcessTxopSuccess.str1.1, 7 bytes
  ," lmac.c"  0 C,
CREATE rodata   \ .rodata, 24 bytes
  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,   \ 6 jumps into lmacProcessTxComplete
CREATE rodata.__FUNCTION__$0   \ .rodata.__FUNCTION__$0, 11 bytes
  ," lmacTxDone"  0 C,
CREATE rodata.__FUNCTION__$1   \ .rodata.__FUNCTION__$1, 12 bytes
  ," lmacTxFrame"  0 C,
CREATE rodata.__FUNCTION__$2   \ .rodata.__FUNCTION__$2, 19 bytes
  ," lmacProcessTxError"  0 C,
CREATE rodata.__FUNCTION__$3   \ .rodata.__FUNCTION__$3, 22 bytes
  ," lmacProcessTxRtsError"  0 C,
CREATE rodata.__FUNCTION__$4   \ .rodata.__FUNCTION__$4, 22 bytes
  ," lmacProcessAckTimeout"  0 C,
CREATE rodata.__FUNCTION__$5   \ .rodata.__FUNCTION__$5, 22 bytes
  ," lmacProcessCtsTimeout"  0 C,
CREATE rodata.__FUNCTION__$6   \ .rodata.__FUNCTION__$6, 25 bytes
  ," lmacProcessLongRetryFail"  0 C,
CREATE rodata.__FUNCTION__$7   \ .rodata.__FUNCTION__$7, 26 bytes
  ," lmacProcessShortRetryFail"  0 C,
CREATE rodata.__FUNCTION__$8   \ .rodata.__FUNCTION__$8, 22 bytes
  ," lmacProcessTxComplete"  0 C,
CREATE rodata.__FUNCTION__$9   \ .rodata.__FUNCTION__$9, 21 bytes
  ," lmacProcessCollision"  0 C,
CREATE rodata.__FUNCTION__$10   \ .rodata.__FUNCTION__$10, 29 bytes
  ," lmacEndFrameExchangeSequence"  0 C,
CREATE rodata.__FUNCTION__$11   \ .rodata.__FUNCTION__$11, 33 bytes
  ," lmacDiscardFrameExchangeSequence"  0 C,
CREATE rodata.__FUNCTION__$12   \ .rodata.__FUNCTION__$12, 21 bytes
  ," lmacProcessTxSuccess"  0 C,
CREATE rodata.__FUNCTION__$13   \ .rodata.__FUNCTION__$13, 21 bytes
  ," lmacProcessTBSuccess"  0 C,
CREATE rodata.__FUNCTION__$14   \ .rodata.__FUNCTION__$14, 23 bytes
  ," lmacProcessTxopSuccess"  0 C,
CREATE rodata_wlog_warning.62   \ .rodata_wlog_warning.62, 33 bytes
  ," sw txq[%d] state(%d) is not idle"  0 C,
CREATE rodata_wlog_info.61   \ .rodata_wlog_info.61, 17 bytes
  ," lmac stop hw txq"  0 C,
CREATE rodata_wlog_error.60   \ .rodata_wlog_error.60, 10 bytes
  ," %s %s %u"  10 C,  0 C,
CREATE rodata_wlog_error.57   \ .rodata_wlog_error.57, 10 bytes
  ," %s %s %u"  10 C,  0 C,
CREATE rodata_wlog_error.56   \ .rodata_wlog_error.56, 10 bytes
  ," %s %s %u"  10 C,  0 C,
CREATE rodata_wlog_debug.55   \ .rodata_wlog_debug.55, 13 bytes
  ," discard flag"  0 C,
CREATE rodata_wlog_info.52   \ .rodata_wlog_info.52, 58 bytes
  ," threshold: %d mpdu_retry: %d long_ampdu:%d short_ampdu:%d"  0 C,
CREATE rodata_wlog_error.51   \ .rodata_wlog_error.51, 10 bytes
  ," %s %s %u"  10 C,  0 C,
CREATE rodata_wlog_error.50   \ .rodata_wlog_error.50, 10 bytes
  ," %s %s %u"  10 C,  0 C,
CREATE rodata_wlog_warning.49   \ .rodata_wlog_warning.49, 19 bytes
  ," tx rts error 0x%x"  10 C,  0 C,
CREATE rodata_wlog_error.48   \ .rodata_wlog_error.48, 10 bytes
  ," %s %s %u"  10 C,  0 C,
CREATE rodata_wlog_error.47   \ .rodata_wlog_error.47, 10 bytes
  ," %s %s %u"  10 C,  0 C,
CREATE rodata_wlog_error.46   \ .rodata_wlog_error.46, 10 bytes
  ," %s %s %u"  10 C,  0 C,
CREATE rodata_wlog_error.45   \ .rodata_wlog_error.45, 10 bytes
  ," %s %s %u"  10 C,  0 C,
CREATE rodata_wlog_error.44   \ .rodata_wlog_error.44, 10 bytes
  ," %s %s %u"  10 C,  0 C,
CREATE rodata_wlog_error.43   \ .rodata_wlog_error.43, 10 bytes
  ," %s %s %u"  10 C,  0 C,
CREATE rodata_wlog_error.42   \ .rodata_wlog_error.42, 10 bytes
  ," %s %s %u"  10 C,  0 C,
CREATE rodata_wlog_info.41   \ .rodata_wlog_info.41, 13 bytes
  ," discard flag"  0 C,
CREATE rodata_wlog_error.40   \ .rodata_wlog_error.40, 10 bytes
  ," %s %s %u"  10 C,  0 C,
CREATE rodata_wlog_error.39   \ .rodata_wlog_error.39, 10 bytes
  ," %s %s %u"  10 C,  0 C,
CREATE rodata_wlog_error.38   \ .rodata_wlog_error.38, 10 bytes
  ," %s %s %u"  10 C,  0 C,
CREATE rodata_wlog_error.36   \ .rodata_wlog_error.36, 10 bytes
  ," %s %s %u"  10 C,  0 C,
CREATE rodata_wlog_info.35   \ .rodata_wlog_info.35, 19 bytes
  ," int: state=%d i=%d"  0 C,
PUBLIC
1 DATA: g_pp_tx_pkt_num   \ .bss.g_pp_tx_pkt_num
PRIVATE
CREATE rodata_wlog_error.30   \ .rodata_wlog_error.30, 10 bytes
  ," %s %s %u"  10 C,  0 C,
CREATE rodata_wlog_error.29   \ .rodata_wlog_error.29, 10 bytes
  ," %s %s %u"  10 C,  0 C,
CREATE rodata_wlog_error.28   \ .rodata_wlog_error.28, 10 bytes
  ," %s %s %u"  10 C,  0 C,
CREATE rodata_wlog_error.26   \ .rodata_wlog_error.26, 10 bytes
  ," %s %s %u"  10 C,  0 C,
CREATE rodata_wlog_error.25   \ .rodata_wlog_error.25, 10 bytes
  ," %s %s %u"  10 C,  0 C,
CREATE rodata_wlog_error.24   \ .rodata_wlog_error.24, 10 bytes
  ," %s %s %u"  10 C,  0 C,
CREATE rodata_wlog_error.23   \ .rodata_wlog_error.23, 10 bytes
  ," %s %s %u"  10 C,  0 C,
CREATE rodata_wlog_error.22   \ .rodata_wlog_error.22, 10 bytes
  ," %s %s %u"  10 C,  0 C,
CREATE rodata_wlog_error.20   \ .rodata_wlog_error.20, 10 bytes
  ," %s %s %u"  10 C,  0 C,
CREATE rodata_wlog_error.15   \ .rodata_wlog_error.15, 10 bytes
  ," %s %s %u"  10 C,  0 C,
CREATE rodata_wlog_error.13   \ .rodata_wlog_error.13, 10 bytes
  ," %s %s %u"  10 C,  0 C,
CREATE rodata_wlog_error.11   \ .rodata_wlog_error.11, 10 bytes
  ," %s %s %u"  10 C,  0 C,
CREATE rodata_wlog_error.9   \ .rodata_wlog_error.9, 10 bytes
  ," %s %s %u"  10 C,  0 C,
CREATE rodata_wlog_error.8   \ .rodata_wlog_error.8, 10 bytes
  ," %s %s %u"  10 C,  0 C,
PUBLIC
1 DATA: txopstart_index   \ .data.txopstart_index
  $05  1 txopstart_index INIT
48 DATA: lmacConfMib   \ .data.lmacConfMib
  $00  $00  $00  $00  $00  $00  $00  $00  $00  $01  $00  $00  $00  $00  $00  $00  16 lmacConfMib $10 + INIT
  $00  $00  $00  $00  $00  $00  $00  $00  $00  $00  $00  $01  $01  $00  $00  $00  16 lmacConfMib $20 + INIT
PRIVATE
180 DATA: bss.our_instances   \ .bss.our_instances

\ ==== Code =============================================================

CODE offchan_in_progress   \ .text.offchan_in_progress $4, static, without symbol
  $0 1 LITERALS
    SYMBOL offchan_tx_progress_in
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A8 0 L8UI,  RETW.N,
$F L:  END-CODE

PUBLIC
CODE lmacIsIdle   \ .text.lmacIsIdle $4
  $0 1 LITERALS
    SYMBOL bss.our_instances
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A2 0 8 EXTUI,  A2 A2 A2 ADDX8,  A2 A2 A8 ADDX4,
  A8 0 MOVI.N,  A9 A2 18 L8UI,  A2 1 MOVI.N,  A2 A8 A9 MOVNEZ,  RETW.N,
$1F L:  END-CODE

CODE lmacGetTxFrame   \ .text.lmacGetTxFrame $4
  $0 1 LITERALS
    SYMBOL bss.our_instances
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A2 0 8 EXTUI,  A2 A2 A2 ADDX8,  A8 A2 A8 ADDX4,
  A2 0 MOVI.N,  A9 A8 18 L8UI,  A9 A2 $1D L BEQ,
  A2 A8 0 L32I.N,
$1D L:
  RETW.N,
$1F L:  END-CODE

CODE lmacReachShortLimit   \ .text.lmacReachShortLimit $4
  $0 1 LITERALS
    SYMBOL lmacConfMib
$4 L:
  A1 32 ENTRY,  A9 $0 L L32R,  A2 A2 0 8 EXTUI,  A8 1 MOVI.N,  A9 A9 21 L8UI,
  A2 A9 $18 L BGEU,
  A8 0 MOVI,
$18 L:
  A2 A8 0 1 EXTUI,  RETW.N,
$1D L:  END-CODE

CODE lmacReachLongLimit   \ .text.lmacReachLongLimit $4
  $0 1 LITERALS
    SYMBOL lmacConfMib
$4 L:
  A1 32 ENTRY,  A9 $0 L L32R,  A2 A2 0 8 EXTUI,  A8 1 MOVI.N,  A9 A9 20 L8UI,
  A2 A9 $18 L BGEU,
  A8 0 MOVI,
$18 L:
  A2 A8 0 1 EXTUI,  RETW.N,
$1D L:  END-CODE

CODE lmacIsLongFrame   \ .text.lmacIsLongFrame $4
  $0 1 LITERALS
    SYMBOL lmacConfMib
$4 L:
  A1 32 ENTRY,  A8 A2 22 L16UI,  A2 A2 20 L16UI,  A8 A8 A2 ADD.N,  A2 $0 L L32R,
  A9 A2 22 L16UI,  A2 1 MOVI.N,  A9 A8 $1C L BLT,
  A2 0 MOVI.N,
$1C L:
  A2 A2 0 1 EXTUI,  RETW.N,
$21 L:  END-CODE

CODE lmacProcessLongFrameSuccess   \ .wifi0iram.2 $0
  $0 ORIGIN
  A1 32 ENTRY,  A8 A2 9 L8UI,  A8 A2 8 S8I,  A8 0 MOVI.N,  A8 A2 12 S8I,  MEMW,
  MEMW,  RETW.N,
$16 L:  END-CODE

CODE lmacProcessShortFrameSuccess   \ .wifi0iram.3 $0
  $0 ORIGIN
  A1 32 ENTRY,  A8 A2 9 L8UI,  A8 A2 8 S8I,  A8 0 MOVI.N,  A8 A2 11 S8I,  MEMW,
  MEMW,  RETW.N,
$16 L:  END-CODE

CODE lmacSetTxFrame   \ .wifiextrairam.4 $34
  $0 13 LITERALS
    $FF00FF00 T,  SYMBOL lmacConfMib  SYMBOL g_mesh_is_started
    SYMBOL g_mesh_is_root  SYMBOL g_mesh_init_ps_type  SYMBOL g_osi_funcs_p
    $3FF73C00 T,  SYMBOL g_wifi_mac_time_delta  SYMBOL pTxRx
    SYMBOL wDev_is_low_rate_enable  SYMBOL ppProcessLifeTime
    SYMBOL hal_mac_tx_config_timeout  SYMBOL hal_mac_tx_set_ppdu
$34 L:
  A1 48 ENTRY,  A3 A3 0 8 EXTUI,  A3 $180 L BNEZ,
  A4 A2 0 L32I.N,  A3 A4 44 L32I.N,  A5 A4 4 L32I.N,  A6 A3 0 L32I.N,
  A5 A5 4 L32I.N,  A6 18 $4C L BBCI,
  A5 A5 4 ADDI.N,
$4C L:
  A6 A3 10 L16UI,  A8 $24 L L32R,  A8 CALLX8,
  A9 A4 44 L32I.N,  A10 $74 L BEQZ.N,
  A8 9 MOVI.N,  A3 A9 12 L8UI,  A3 A8 $69 L BNE,
  A3 14 MOVI.N,  $6E L J,
  3 ZEROS,
$69 L:
  A3 10 $74 L BNEI,
  A3 15 MOVI.N,
$6E L:
  A3 A9 12 S8I,  MEMW,
$74 L:
  A3 A9 0 L32I.N,  A3 22 $80 L BBCI,
  MEMW,  $96 L J,
  1 ZEROS,
$80 L:
  A8 A2 12 L32I.N,  A3 $0 L L32R,  A8 A3 $9C L BANY,
  A8 15 MOVI.N,  A3 A9 12 L8UI,  A3 A3 -16 ADDI,  A3 A3 0 8 EXTUI,
  A8 A3 $9C L BLTU,
$96 L:
  A6 A5 2 S16I,  MEMW,
$9C L:
  A5 $4 L L32R,  A3 A5 46 L8UI,  A3 $CC L BEQZ.N,
  A3 A5 45 L8UI,  A8 A9 12 L8UI,  A3 $BB L BEQZ.N,
  A8 A8 -4 ADDI,  A8 A8 0 8 EXTUI,  A8 4 $CC L BGEUI,
  MEMW,  $C6 L J,
$BB L:
  A3 A8 -1 ADDI.N,  A3 A3 0 8 EXTUI,  A3 3 $CC L BGEUI,
  A8 A8 4 ADDI,
$C6 L:
  A8 A9 12 S8I,  MEMW,
$CC L:
  A6 $8 L L32R,  A3 A6 0 L8UI,  A3 $FD L BEQZ.N,
  A3 $C L L32R,  MEMW,  A3 A3 0 L8UI,  A3 A3 0 8 EXTUI,  A3 $FD L BNEZ.N,
  A3 $10 L L32R,  A3 A3 0 L32I.N,  A3 $FD L BEQZ.N,
  A3 $14 L L32R,  A3 A3 0 L32I,  A10 A3 $110 L32I,  MEMW,  A10 CALLX8,
  A3 A10 MOV.N,  $10C L J,
$FD L:
  A3 $18 L L32R,  A8 $1C L L32R,  MEMW,  A3 A3 0 L32I.N,  A8 A8 0 L32I.N,
  A3 A3 A8 ADD.N,
$10C L:
  A6 A6 0 L8UI,  A8 A4 44 L32I.N,  A6 $12C L BEQZ.N,
  A10 A8 24 L32I.N,  A10 A3 A10 SUB,  A10 A10 10 SRLI,  A11 A5 8 L32I.N,
  A10 A11 A10 SUB,  A11 A10 $168 L BLTU,
  A11 1 MOVI.N,  A11 A10 A11 MAXU,  $168 L J,
  1 ZEROS,
$12C L:
  A6 A8 0 L32I.N,  A6 22 $136 L BBCI,
  A8 A5 0 L32I.N,  $138 L J,
$136 L:
  A8 A5 8 L32I.N,
$138 L:
  A10 A4 MOV.N,  A8 A8 10 SLLI,  A11 A3 MOV.N,  A12 A1 MOV.N,  A8 A1 0 S32I,  MEMW,
  A8 $28 L L32R,  A8 CALLX8,
  A11 10 MOVI.N,  A4 A4 44 L32I.N,  A5 A1 0 L32I.N,  A10 A4 24 L32I.N,
  A10 A5 A10 ADD.N,  A10 A10 A3 SUB,  A10 A5 $168 L BGEU,
  A11 SSR,  A11 A10 SRL,  A3 1 MOVI.N,  A11 A11 A3 MAXU,
$168 L:
  A10 A2 MOV.N,  A8 $2C L L32R,  A8 CALLX8,
  A3 $20 L L32R,  A10 A2 MOV.N,  A11 A3 0 L32I.N,  A8 $30 L L32R,  A8 CALLX8,
  RETW.N,
  1 ZEROS,
$180 L:
  A2 0 MOVI.N,  MEMW,  A2 A2 44 L32I.N,  1 15 BREAK,
$18A L:  END-CODE

CODE lmacSetAcParam   \ .text.lmacSetAcParam $4
  $0 1 LITERALS
    SYMBOL bss.our_instances
$4 L:
  A1 32 ENTRY,  A2 A2 0 8 EXTUI,  A10 $0 L L32R,  A8 A2 A2 ADDX8,  A8 A8 A10 ADDX4,
  A4 A4 0 8 EXTUI,  A5 A5 0 8 EXTUI,  A11 A8 8 L8UI,  A9 A2 3 SLLI,  A3 A8 5 S8I,
  A4 A8 9 S8I,  A5 A8 10 S8I,  MEMW,  A5 A11 $3C L BGEU,
  A5 A8 8 S8I,  MEMW,  MEMW,  $42 L J,
  2 ZEROS,
$3C L:
  A11 A4 $42 L BGEU,
  A4 A8 8 S8I,
$42 L:
  A9 A9 A2 ADD.N,  A9 A9 A10 ADDX4,  A2 0 MOVI.N,  A2 A9 24 S16I,  MEMW,  MEMW,
  RETW.N,
$54 L:  END-CODE

CODE lmacInitAc   \ .text.lmacInitAc $4
  $0 1 LITERALS
    SYMBOL bss.our_instances
$4 L:
  A1 32 ENTRY,  A2 A2 0 8 EXTUI,  A9 $0 L L32R,  A8 A2 A2 ADDX8,  A8 A8 A9 ADDX4,
  A4 A4 0 8 EXTUI,  A9 0 MOVI.N,  A2 A8 4 S8I,  A2 0 MOVI.N,  A3 A8 5 S8I,
  A4 A8 9 S8I,  A5 A8 10 S8I,  A4 A8 8 S8I,  A9 A8 24 S16I,  A9 A8 11 S8I,
  A9 A8 12 S8I,  A9 A8 18 S8I,  MEMW,  A2 A8 28 S32I.N,  A2 A8 32 S32I.N,  MEMW,
  RETW.N,
$41 L:  END-CODE

CODE lmacInit   \ .text.lmacInit $28
  $0 10 LITERALS
    SYMBOL lmacConfMib  $92A2020 T,  $FFF015F T,  SYMBOL lmacInitAc
    SYMBOL lmacInitAc  SYMBOL lmacInitAc  SYMBOL lmacInitAc  SYMBOL lmacInitAc
    SYMBOL RC_SetBasicRate  SYMBOL rcAttach
$28 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A9 9 MOVI.N,  A14 0 MOVI.N,  A13 10 MOVI.N,
  A12 4 MOVI.N,  A9 A8 28 S16I,  MEMW,  A9 $600 MOVI,  A11 3 MOVI.N,  A10 2 MOVI.N,
  A9 A8 0 S32I.N,  A9 $400 MOVI,  A9 A8 4 S32I.N,  A9 A8 8 S32I.N,  A9 $200 MOVI,
  A9 A8 12 S32I.N,  A9 A8 16 S32I.N,  A9 $4 L L32R,  A9 A8 20 S32I.N,  A9 $100 MOVI,
  A9 A8 24 S16I,  MEMW,  A9 $8 L L32R,  A9 A8 36 S32I,  MEMW,  A8 $C L L32R,
  A8 CALLX8,
  A14 0 MOVI.N,  A13 10 MOVI.N,  A12 4 MOVI.N,  A11 7 MOVI.N,  A10 3 MOVI,
  A8 $10 L L32R,  A8 CALLX8,
  A14 47 MOVI.N,  A14 A14 6 SLLI,  A13 4 MOVI.N,  A12 3 MOVI.N,  A11 2 MOVI.N,
  A10 1 MOVI.N,  A8 $14 L L32R,  A8 CALLX8,
  A12 2 MOVI.N,  A11 A12 MOV.N,  A14 $5E0 MOVI,  A13 3 MOVI.N,  A10 0 MOVI.N,
  A8 $18 L L32R,  A8 CALLX8,
  A14 0 MOVI.N,  A13 A14 MOV.N,  A12 A14 MOV.N,  A11 1 MOVI.N,  A10 4 MOVI.N,
  A8 $1C L L32R,  A8 CALLX8,
  A11 1 MOVI.N,  A10 $15F MOVI,  A8 $20 L L32R,  A8 CALLX8,
  A8 $24 L L32R,  A8 CALLX8,
  RETW.N,
$C8 L:  END-CODE

CODE lmacProcessTxopSuccess   \ .text.lmacProcessTxopSuccess $24
  $0 9 LITERALS
    SYMBOL rodata.__FUNCTION__$14  SYMBOL rodata.lmacProcessTxopSucc~5667
    SYMBOL rodata_wlog_error.8  SYMBOL bss.our_instances
    SYMBOL rodata_wlog_error.9  SYMBOL rodata_wlog_error.11  SYMBOL wifi_log
    SYMBOL wifi_log  SYMBOL wifi_log
$24 L:
  A1 48 ENTRY,  A2 A2 0 8 EXTUI,  A2 10 $38 L BNEI,
  A2 0 MOVI.N,  MEMW,  A2 A2 44 L32I,  1 15 BREAK,
$38 L:
  A15 $0 L L32R,  A14 $4 L L32R,  A2 5 $5C L BLTUI,
  A2 $27B MOVI,  A12 1 MOVI.N,  A11 A12 11 SLLI,  A13 $8 L L32R,  A10 6 MOVI.N,
  A2 A1 0 S32I.N,  MEMW,  A8 $18 L L32R,  A8 CALLX8,
$59 L:
  $59 L J,
$5C L:
  A8 $C L L32R,  A2 A2 A2 ADDX8,  A2 A2 A8 ADDX4,  A11 1 MOVI.N,  A11 A11 11 SLLI,
  A2 A2 18 L8UI,  A2 2 $88 L BEQI,
  A2 $27E MOVI,  A12 1 MOVI.N,  A13 $10 L L32R,  A10 6 MOVI.N,  A2 A1 0 S32I.N,
  MEMW,  A8 $1C L L32R,  A8 CALLX8,
$85 L:
  $85 L J,
$88 L:
  A2 $280 MOVI,  A13 $14 L L32R,  A12 1 MOVI.N,  A10 6 MOVI.N,  A2 A1 0 S32I.N,
  MEMW,  A8 $20 L L32R,  A8 CALLX8,
$9D L:
  $9D L J,
$A0 L:  END-CODE

CODE GetAccess   \ .wifi0iram.16 $4
  $0 1 LITERALS
    SYMBOL bss.our_instances
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A2 0 8 EXTUI,  A2 A2 A2 ADDX8,  A2 A2 A8 ADDX4,
  RETW.N,
$15 L:  END-CODE

CODE lmacEndRetryAMPDUFail   \ .text.lmacEndRetryAMPDUFail $14
  $0 5 LITERALS
    SYMBOL trc_isTxAmpduOperational  SYMBOL ppRecordBarRRC  SYMBOL ppFillAMPDUBar
    SYMBOL ppReSendBar  SYMBOL ppRegressAmpdu
$14 L:
  A1 32 ENTRY,  A4 0 MOVI,  A4 A2 18 S8I,  MEMW,  A10 A3 36 L32I,  MEMW,
  A8 $0 L L32R,  A8 CALLX8,
  A10 $78 L BEQZ,
  A8 A3 44 L32I.N,  A4 A3 4 L32I.N,  A9 A8 0 L32I.N,  A4 A4 4 L32I.N,
  A9 18 $3C L BBCI,
  A4 A4 4 ADDI.N,
$3C L:
  A11 1 MOVI.N,  A10 A8 16 L32I.N,  A10 A10 20 4 EXTUI,  A8 $4 L L32R,  A8 CALLX8,
  A11 A4 4 ADDI.N,  A8 A3 44 L32I.N,  A12 A4 10 ADDI,  A13 A4 22 L16UI,
  A13 A13 4 SRLI,  A10 A8 4 L32I,  A10 A10 0 4 EXTUI,  A8 $8 L L32R,  A8 CALLX8,
  A3 A3 44 L32I.N,  A11 A3 16 L32I.N,  A11 A11 20 4 EXTUI,  A10 A3 4 L32I.N,
  A10 A10 0 4 EXTUI,  A8 $C L L32R,  A8 CALLX8,
  $82 L J,
  1 ZEROS,
$78 L:
  A11 1 MOVI.N,  A10 A3 MOV.N,  A8 $10 L L32R,  A8 CALLX8,
$82 L:
  A3 8 MOVI.N,  A3 A2 19 S8I,  MEMW,  MEMW,  RETW.N,
$8F L:  END-CODE

CODE lmacProcessCollisions   \ .iram1.27 $4
  $0 1 LITERALS
    SYMBOL pp_post
$4 L:
  A1 32 ENTRY,  A11 0 MOVI.N,  A10 24 MOVI.N,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$13 L:  END-CODE

CODE lmacProcessAllTxTimeout   \ .iram1.31 $4
  $0 1 LITERALS
    SYMBOL pp_post
$4 L:
  A1 32 ENTRY,  A11 0 MOVI.N,  A10 22 MOVI.N,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$13 L:  END-CODE

CODE lmacPostTxComplete   \ .iram1.32 $4
  $0 1 LITERALS
    SYMBOL pp_post
$4 L:
  A1 32 ENTRY,  A11 0 MOVI.N,  A10 23 MOVI.N,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$13 L:  END-CODE

CODE lmacProcessRxSucData   \ .iram1.33 $1C
  $0 7 LITERALS
    SYMBOL g_mesh_is_started  SYMBOL g_mesh_is_root  SYMBOL g_mesh_init_ps_type
    SYMBOL g_osi_funcs_p  $3FF73C00 T,  SYMBOL g_wifi_mac_time_delta
    SYMBOL pp_post
$1C L:
  A1 32 ENTRY,  A8 $0 L L32R,  A8 A8 0 L8UI,  A8 $4C L BEQZ.N,
  A8 $4 L L32R,  MEMW,  A8 A8 0 L8UI,  A8 A8 0 8 EXTUI,  A8 $4C L BNEZ.N,
  A8 $8 L L32R,  A8 A8 0 L32I.N,  A8 $4C L BEQZ.N,
  A8 $C L L32R,  A8 A8 0 L32I,  A10 A8 $110 L32I,  A10 CALLX8,
  $5B L J,
  1 ZEROS,
$4C L:
  A8 $10 L L32R,  MEMW,  A10 A8 0 L32I.N,  A8 $14 L L32R,  A8 A8 0 L32I.N,
  A10 A10 A8 ADD.N,
$5B L:
  A11 A10 MOV.N,  A10 25 MOVI.N,  A8 $18 L L32R,  A8 CALLX8,
  RETW.N,
$67 L:  END-CODE

CODE lmacMSDUAged   \ .wifi0iram.37 $20
  $0 8 LITERALS
    SYMBOL g_mesh_is_started  SYMBOL g_mesh_is_root  SYMBOL g_mesh_init_ps_type
    SYMBOL g_osi_funcs_p  $3FF73C00 T,  SYMBOL g_wifi_mac_time_delta
    SYMBOL lmacConfMib  SYMBOL ppProcessLifeTime
$20 L:
  A1 48 ENTRY,  A3 $0 L L32R,  A3 A3 0 L8UI,  A3 $51 L BEQZ.N,
  A3 $4 L L32R,  MEMW,  A3 A3 0 L8UI,  A3 A3 0 8 EXTUI,  A3 $51 L BNEZ.N,
  A3 $8 L L32R,  A3 A3 0 L32I.N,  A3 $51 L BEQZ.N,
  A3 $C L L32R,  A3 A3 0 L32I,  A10 A3 $110 L32I,  A10 CALLX8,
  A3 A10 MOV.N,  $60 L J,
$51 L:
  A3 $10 L L32R,  A8 $14 L L32R,  MEMW,  A3 A3 0 L32I.N,  A8 A8 0 L32I.N,
  A3 A3 A8 ADD.N,
$60 L:
  A8 A2 44 L32I.N,  A9 A8 0 L32I.N,  A8 $18 L L32R,  A9 22 $70 L BBCI,
  A8 A8 0 L32I.N,  $72 L J,
  1 ZEROS,
$70 L:
  A8 A8 8 L32I.N,
$72 L:
  A8 A8 10 SLLI,  A11 A3 MOV.N,  A10 A2 MOV.N,  A12 A1 MOV.N,  A8 A1 0 S32I.N,
  MEMW,  A8 $1C L L32R,  A8 CALLX8,
  A2 A2 44 L32I.N,  A2 A2 24 L32I.N,  A3 A3 A2 SUB,  A2 1 MOVI.N,  A8 A1 0 L32I.N,
  A8 A3 $A0 L BLTU,
  A8 A8 -5120 ADDMI,  A8 A3 $9D L BLTU,
  A2 0 MOVI,
$9D L:
  A2 A2 0 8 EXTUI,
$A0 L:
  RETW.N,
$A2 L:  END-CODE

CODE dbg_cnt_lmac_drop   \ .text.lmacDebugTxDrop $C
  $0 3 LITERALS
    SYMBOL lmacConfMib  SYMBOL g_lmac_cnt  SYMBOL lmacMSDUAged
$C L:
  A1 32 ENTRY,  A10 A2 MOV.N,  A2 $88 L BEQZ,
  A8 A2 44 L32I.N,  A3 A8 4 L32I.N,  A3 A3 4 4 EXTUI,  A3 5 $88 L BGEI,
  A3 A3 14 ADDI.N,  A9 $0 L L32R,  A12 A8 6 L8UI,  A11 A9 21 L8UI,  A4 $4 L L32R,
  A12 A11 $44 L BLTU,
  A3 A3 A4 ADDX8,  A4 A3 13 L8UI,  A4 A4 1 ADDI.N,  A4 A3 13 S8I,  MEMW,  MEMW,
  $88 L J,
  1 ZEROS,
$44 L:
  A11 A8 7 L8UI,  A8 A9 20 L8UI,  A11 A8 $61 L BLTU,
  A3 A3 A4 ADDX8,  A4 A3 12 L8UI,  A4 A4 1 ADDI.N,  A4 A3 12 S8I,  MEMW,  MEMW,
  $88 L J,
$61 L:
  A8 $8 L L32R,  A8 CALLX8,
  A3 A3 A4 ADDX8,  A10 $7D L BEQZ.N,
  A4 A3 14 L8UI,  A4 A4 1 ADDI.N,  A4 A3 14 S8I,  MEMW,  MEMW,  $88 L J,
$7D L:
  A4 A3 15 L8UI,  A4 A4 1 ADDI.N,  A4 A3 15 S8I,  MEMW,
$88 L:
  MEMW,  RETW.N,
$8D L:  END-CODE

CODE esp_wifi_internal_set_retr~3DF8   \ esp_wifi_internal_set_retry_counter: .text.esp_wifi_internal_set_retry_counter $4
  $0 1 LITERALS
    SYMBOL lmacConfMib
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A8 21 S8I,  A2 0 MOVI.N,  A3 A8 20 S8I,  MEMW,
  MEMW,  RETW.N,
$1A L:  END-CODE

CODE esp_wifi_internal_set_msdu~671D   \ esp_wifi_internal_set_msdu_lifetime: .text.esp_wifi_internal_set_msdu_lifetime $4
  $0 1 LITERALS
    SYMBOL lmacConfMib
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A8 16 S32I.N,  A2 0 MOVI.N,  A3 A8 8 S32I.N,
  MEMW,  RETW.N,
$15 L:  END-CODE

CODE esp_wifi_internal_get_mib   \ .text.esp_wifi_internal_get_mib $8
  $0 2 LITERALS
    SYMBOL lmacConfMib  SYMBOL memcpy
$8 L:
  A1 32 ENTRY,  A10 A2 MOV.N,  A2 $102 MOVI,  A10 $20 L BEQZ.N,
  A11 $0 L L32R,  A12 48 MOVI,  A8 $4 L L32R,  A8 CALLX8,
  A2 0 MOVI.N,
$20 L:
  RETW.N,
$22 L:  END-CODE

CODE esp_wifi_internal_set_rts   \ .text.esp_wifi_internal_set_rts $C
  $0 3 LITERALS
    SYMBOL lmacConfMib  SYMBOL rodata_wlog_info.52  SYMBOL wifi_log
$C L:
  A1 48 ENTRY,  A8 A2 MOV.N,  A2 $102 MOVI,  A8 $55 L BEQZ.N,
  A14 A8 0 L16UI,  A12 3 MOVI.N,  A11 1 MOVI.N,  A10 6 MOVI.N,  A9 $0 L L32R,
  A14 A9 22 S16I,  MEMW,  A15 A8 2 L8UI,  A15 A9 42 S8I,  MEMW,  A2 A8 3 L8UI,
  A2 A9 43 S8I,  MEMW,  A8 A8 4 L8UI,  A13 $4 L L32R,  A8 A9 44 S8I,  MEMW,
  A2 A1 0 S32I.N,  A2 0 MOVI.N,  A8 A1 4 S32I.N,  MEMW,  A8 $8 L L32R,  A8 CALLX8,
$55 L:
  RETW.N,
$57 L:  END-CODE

CODE esp_wifi_internal_get_rts   \ .text.esp_wifi_internal_get_rts $4
  $0 1 LITERALS
    SYMBOL lmacConfMib
$4 L:
  A1 32 ENTRY,  A8 A2 MOV.N,  A2 $102 MOVI,  A8 $38 L BEQZ.N,
  A9 $0 L L32R,  A2 A9 22 L16UI,  A2 A8 0 S16I,  MEMW,  A2 A9 42 L8UI,  A2 A8 2 S8I,
  MEMW,  A2 A9 43 L8UI,  A2 A8 3 S8I,  MEMW,  A2 A9 44 L8UI,  A2 A8 4 S8I,  MEMW,
  A2 0 MOVI,
$38 L:
  MEMW,  RETW.N,
$3D L:  END-CODE

CODE lmacAdjustTimestamp   \ .wifi0iram.53 $10
  $0 4 LITERALS
    SYMBOL g_mesh_is_started  $3FF73C00 T,  SYMBOL g_wifi_mac_time_delta
    SYMBOL lmacConfMib
$10 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A8 A8 0 L8UI,  A8 $55 L BNEZ.N,
  A11 A2 44 L32I.N,  A8 A11 0 L32I.N,  A8 5 $55 L BBSI,
  A9 $4 L L32R,  A10 $8 L L32R,  MEMW,  A9 A9 0 L32I.N,  A10 A10 0 L32I.N,
  A9 A9 A10 ADD.N,  A10 $C L L32R,  A8 22 $3E L BBCI,
  A8 A10 0 L32I.N,  A10 A10 4 L32I.N,  $42 L J,
$3E L:
  A8 A10 8 L32I.N,  A10 A10 12 L32I.N,
$42 L:
  A8 A8 A10 SUB,  A8 A8 10 SLLI,  A10 A11 24 L32I.N,  A10 A9 A10 SUB,
  A8 A10 $55 L BGEU,
  A9 A9 A8 SUB,  A9 A11 24 S32I.N,
$55 L:
  MEMW,  RETW.N,
$5A L:  END-CODE

CODE lmac_record_txtime   \ .text.lmac_record_txtime $0
  $0 ORIGIN
  A1 32 ENTRY,  RETW.N,
$5 L:  END-CODE

CODE lmac_update_tx_statistic   \ .text.lmac_update_tx_statistic $0
  $0 ORIGIN
  A1 32 ENTRY,  RETW.N,
$5 L:  END-CODE

CODE lmacTxDone   \ .wifi0iram.59 $30
  $0 12 LITERALS
    $400008 T,  SYMBOL bss.our_instances  SYMBOL rodata.__FUNCTION__$0
    SYMBOL rodata.lmacProcessTxopSucc~5667  SYMBOL rodata_wlog_error.60  $98A T,
    SYMBOL ppProcTxCallback  SYMBOL ppEnqueueTxDone  SYMBOL rcUpdateTxDone
    SYMBOL pp_post  SYMBOL wifi_log  SYMBOL ppProcessTxQ
$30 L:
  A1 48 ENTRY,  A4 A2 44 L32I.N,  A11 1 MOVI.N,  A10 A2 MOV.N,  A3 A3 0 8 EXTUI,
  A4 A4 4 L32I.N,  A8 $18 L L32R,  A8 CALLX8,
  A10 A2 MOV.N,  A8 $1C L L32R,  A8 CALLX8,
  A4 A4 4 4 EXTUI,  A11 A2 44 L32I.N,  A9 $0 L L32R,  A8 A11 0 L32I.N,
  A8 A8 A9 AND,  A8 8 $64 L BNEI,
  A10 A2 36 L32I.N,  A8 $20 L L32R,  A8 CALLX8,
$64 L:
  A11 0 MOVI.N,  A10 16 MOVI.N,  A8 $24 L L32R,  A8 CALLX8,
  A3 $A8 L BEQZ.N,
  A2 $4 L L32R,  A8 A4 A4 ADDX8,  A8 A8 A2 ADDX4,  A2 A8 18 L8UI,  A2 $A0 L BEQZ.N,
  A12 1 MOVI.N,  A11 A12 11 SLLI,  A2 $14 L L32R,  A10 6 MOVI.N,  A15 $8 L L32R,
  A14 $C L L32R,  A13 $10 L L32R,  A2 A1 0 S32I.N,  MEMW,  A8 $28 L L32R,
  A8 CALLX8,
$9C L:
  $9C L J,
  1 ZEROS,
$A0 L:
  A10 A4 MOV.N,  A8 $2C L L32R,  A8 CALLX8,
$A8 L:
  RETW.N,
$AA L:  END-CODE

CODE lmacDiscardMSDU   \ .text.lmacDiscardMSDU $10
  $0 4 LITERALS
    SYMBOL dbg_cnt_lmac_drop  SYMBOL lmacTxDone  SYMBOL ppDequeueTxQ
    SYMBOL lmacTxDone
$10 L:
  A1 32 ENTRY,  A5 A5 0 8 EXTUI,  A7 192 MOVI,
$19 L:
  A10 A3 MOV.N,  A8 $0 L L32R,  A8 CALLX8,
  A6 A2 32 L32I.N,  A6 A6 1 ADDI.N,  A8 A3 44 L32I.N,  A6 A2 32 S32I.N,
  A4 A8 19 S8I,  MEMW,  A6 A8 16 L32I.N,  A6 A6 20 4 EXTUI,  A8 A8 0 L32I.N,
  A8 A7 A8 AND,  A8 128 $5A L BNEI,
  A10 A3 MOV.N,  A11 0 MOVI.N,  MEMW,  A8 $4 L L32R,  A8 CALLX8,
  A10 A6 MOV.N,  A8 $8 L L32R,  A8 CALLX8,
  A3 A10 MOV.N,  A10 $19 L BNEZ,
  $64 L J,
  1 ZEROS,
$5A L:
  A11 A5 MOV.N,  A10 A3 MOV.N,  A8 $C L L32R,  A8 CALLX8,
$64 L:
  RETW.N,
$66 L:  END-CODE

CODE lmacDiscardAgedMSDU   \ .wifi0iram.17 $4
  $0 1 LITERALS
    SYMBOL lmacDiscardMSDU
$4 L:
  A1 32 ENTRY,  A13 A4 0 8 EXTUI,  A12 4 MOVI.N,  A11 A3 MOV.N,  A10 A2 MOV.N,
  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$18 L:  END-CODE

CODE lmacTxFrame   \ .wifi0iram.54 $70
  $0 28 LITERALS
    SYMBOL bss.our_instances  SYMBOL rodata_wlog_debug.55
    SYMBOL rodata.__FUNCTION__$1  SYMBOL rodata.lmacProcessTxopSucc~5667
    SYMBOL rodata_wlog_error.56  $8B6 T,  SYMBOL g_osi_funcs_p
    SYMBOL rodata_wlog_error.57  $8BE T,  $2102 T,  $FFFFEFFF T,
    SYMBOL lmacConfMib  SYMBOL offchan_in_progress  SYMBOL wifi_log
    SYMBOL wDev_isNANPktInValidSlot  SYMBOL lmacDiscardAgedMSDU
    SYMBOL ppRecycleAmpdu  SYMBOL pp_post  SYMBOL wifi_log  SYMBOL wifi_log
    SYMBOL lmacIsLongFrame  SYMBOL lmacSetTxFrame  SYMBOL lmacIsLongFrame
    SYMBOL hal_random  SYMBOL hal_random  SYMBOL hal_mac_tx_config_edca
    SYMBOL wifi_gpio_debug  SYMBOL hal_mac_txq_enable
$70 L:
  A1 64 ENTRY,  A4 A2 44 L32I,  A5 A3 0 8 EXTUI,  A3 A5 A5 ADDX8,  A6 $0 L L32R,
  A3 A3 A6 ADDX4,  A7 A4 2 L16UI,  A4 A5 3 SLLI,  A7 0 $C6 L BBCI,
  A8 $30 L L32R,  A8 CALLX8,
  A10 $C6 L BNEZ.N,
$93 L:
  A4 A4 A5 ADD.N,  A4 A4 A6 ADDX4,  A13 $4 L L32R,  A6 0 MOVI.N,  A12 4 MOVI.N,
  A11 64 MOVI.N,  A10 6 MOVI.N,  A6 A4 0 S32I.N,  A6 A4 18 S8I,  MEMW,  MEMW,
  A8 $34 L L32R,  A8 CALLX8,
  A4 A2 44 L32I.N,  A12 A4 0 L32I.N,  A4 1 MOVI.N,  A4 A4 22 SLLI,  A12 A12 A4 AND,
  A12 A6 $D6 L BEQ,
  $E4 L J,
$C6 L:
  A10 A2 MOV.N,  A8 $38 L L32R,  A8 CALLX8,
  A10 $FB L BNEZ.N,
  $93 L J,
  3 ZEROS,
$D6 L:
  A11 A2 MOV.N,  A10 A3 MOV.N,  A8 $3C L L32R,  A8 CALLX8,
  $EE L J,
  1 ZEROS,
$E4 L:
  A11 A6 MOV.N,  A10 A2 MOV.N,  A8 $40 L L32R,  A8 CALLX8,
$EE L:
  A11 0 MOVI.N,  A10 A5 MOV.N,  A8 $44 L L32R,  A8 CALLX8,
  $27C L J,
$FB L:
  A7 A4 A5 ADD.N,  A7 A7 A6 ADDX4,  A8 A7 18 L8UI,  A7 A8 -3 ADDI,  A7 A7 0 8 EXTUI,
  A7 2 $135 L BLTUI,
  A11 1 MOVI.N,  A8 $135 L BEQZ.N,
  A2 $14 L L32R,  A12 A11 MOV.N,  A10 6 MOVI,  A11 A11 11 SLLI,  A15 $8 L L32R,
  A14 $C L L32R,  A13 $10 L L32R,  A2 A1 0 S32I,  MEMW,  A8 $48 L L32R,  A8 CALLX8,
$130 L:
  $130 L J,
  2 ZEROS,
$135 L:
  A9 $18 L L32R,  A8 A9 0 L32I.N,  A8 A8 216 L32I,  A8 CALLX8,
  A8 A4 A5 ADD.N,  A8 A8 A6 ADDX4,  A11 A8 18 L8UI,  A11 4 $174 L BNEI,
  A8 A8 0 L32I.N,  A8 A2 $153 L BNE,
  $1F5 L J,
$153 L:
  A12 1 MOVI.N,  A11 A12 11 SLLI,  A2 $20 L L32R,  A10 6 MOVI.N,  A15 $8 L L32R,
  A14 $C L L32R,  A13 $1C L L32R,  A2 A1 0 S32I.N,  MEMW,  A8 $4C L L32R,
  A8 CALLX8,
$171 L:
  $171 L J,
$174 L:
  A7 A2 44 L32I.N,  A10 1 MOVI.N,  A10 A10 13 SLLI,  A9 A7 0 L32I.N,
  A12 $24 L L32R,  A12 A9 A12 AND,  A2 A8 0 S32I.N,  A12 A10 $192 L BNE,
  A10 1 MOVI.N,  A10 A10 12 SLLI,  A9 A9 A10 OR,  A9 A7 0 S32I.N,
$192 L:
  A10 A2 MOV.N,  A11 A1 16 S32I,  MEMW,  A8 $50 L L32R,  A8 CALLX8,
  A11 A1 16 L32I.N,  A10 $1B8 L BEQZ.N,
  A9 A7 0 L32I.N,  A9 1 $1B8 L BBSI,
  A10 $28 L L32R,  A9 A9 A10 AND,  A10 $100 MOVI,  A9 A9 A10 OR,  A9 A7 0 S32I,
$1B8 L:
  A9 A7 0 L32I.N,  A9 12 $1E8 L BBCI,
  A11 3 $1E8 L BNEI,
  A10 $2C L L32R,  A11 A7 5 L8UI,  A10 A10 42 L8UI,  A11 A10 $1E8 L BLTU,
  A10 $28 L L32R,  A9 A9 A10 AND,  A10 $100 MOVI,  A9 A9 A10 OR,  A9 A7 0 S32I.N,
  A9 A7 6 L8UI,  A9 A7 7 S8I,  A9 0 MOVI.N,  A9 A7 6 S8I,  MEMW,
$1E8 L:
  A11 0 MOVI.N,  A10 A3 MOV.N,  MEMW,  A8 $54 L L32R,  A8 CALLX8,
$1F5 L:
  A8 A4 A5 ADD.N,  A7 A8 A6 ADDX4,  A9 A7 9 L8UI,  A9 A9 A9 ADD.N,  A10 A7 8 L8UI,
  A9 A10 $21D L BGE,
  A10 A2 MOV.N,  A8 $58 L L32R,  A8 CALLX8,
  A10 $21D L BNEZ.N,
  A8 $5C L L32R,  A8 CALLX8,
  A2 A7 9 L8UI,  A2 A2 A2 ADD.N,  $22B L J,
$21D L:
  A2 A4 A5 ADD.N,  A2 A2 A6 ADDX4,  A8 $60 L L32R,  A8 CALLX8,
  A2 A2 8 L8UI,
$22B L:
  A8 -1 MOVI.N,  A2 SSL,  A2 A8 SLL,  A8 A8 A2 XOR,  A10 A8 A10 AND,
  A4 A4 A5 ADD.N,  A4 A4 A6 ADDX4,  A10 A10 0 16 EXTUI,  A10 A4 6 S16I,  MEMW,
  A10 A3 MOV.N,  MEMW,  A8 $64 L L32R,  A8 CALLX8,
  A2 1 MOVI.N,  A11 2 MOVI.N,  A2 A4 18 S8I,  MEMW,  A10 9 MOVI,  MEMW,
  A8 $68 L L32R,  A8 CALLX8,
  A10 A4 4 L8UI,  A8 $6C L L32R,  A8 CALLX8,
  A3 $18 L L32R,  A2 A3 0 L32I.N,  A2 A2 220 L32I,  A2 CALLX8,
$27C L:
  RETW.N,
$27E L:  END-CODE

CODE lmacRetryTxFrame   \ .text.lmacRetryTxFrame $1C
  $0 7 LITERALS
    SYMBOL rodata_wlog_info.41  SYMBOL rcGetRate  SYMBOL ppCalFrameTimes
    SYMBOL lmacTxFrame  SYMBOL wifi_log  SYMBOL lmacDiscardAgedMSDU
    SYMBOL ppRecycleAmpdu
$1C L:
  A1 32 ENTRY,  A4 A2 0 L32I.N,  A3 A3 0 8 EXTUI,  A11 A4 44 L32I.N,
  A5 A4 4 L32I.N,  A9 A11 0 L32I,  A5 A5 4 L32I,  A9 17 $89 L BBSI,
  A3 $C6 L BNEZ,
  A8 A2 13 L8UI,  A8 $C6 L BNEZ,
  A10 A2 15 L8UI,  A10 $49 L BEQZ,
  A8 A2 15 S8I,  A8 A2 13 S8I,  MEMW,
$49 L:
  A9 18 $4E L BBCI,
  A5 A5 4 ADDI.N,
$4E L:
  A10 A4 36 L32I.N,  MEMW,  A8 $4 L L32R,  A8 CALLX8,
  A10 A2 0 L32I.N,  A4 A10 44 L32I.N,  A4 A4 0 L32I.N,  A4 6 $75 L BBCI,
  A8 $8 L L32R,  A8 CALLX8,
  A4 A2 0 L32I.N,  A4 A4 44 L32I.N,  A4 A4 10 L16UI,  A4 A5 2 S16I,  MEMW,
$75 L:
  A11 A2 4 L8UI,  A10 A2 0 L32I,  MEMW,  A8 $C L L32R,  A8 CALLX8,
  $C6 L J,
  2 ZEROS,
$89 L:
  A5 0 MOVI.N,  A13 $0 L L32R,  A12 3 MOVI.N,  A11 64 MOVI.N,  A10 6 MOVI.N,
  A5 A2 0 S32I.N,  A5 A2 18 S8I,  MEMW,  MEMW,  A8 $10 L L32R,  A8 CALLX8,
  A5 A4 44 L32I.N,  A5 A5 0 L32I.N,  A5 22 $BC L BBSI,
  A12 1 MOVI.N,  A11 A4 MOV.N,  A10 A2 MOV.N,  A8 $14 L L32R,  A8 CALLX8,
  $C6 L J,
  1 ZEROS,
$BC L:
  A11 1 MOVI.N,  A10 A4 MOV.N,  A8 $18 L L32R,  A8 CALLX8,
$C6 L:
  RETW.N,
$C8 L:  END-CODE

CODE lmacDiscardFrameExchangeSe~E31C   \ lmacDiscardFrameExchangeSequence: .wifiextrairam.19 $24
  $0 9 LITERALS
    SYMBOL rodata.__FUNCTION__$11  SYMBOL rodata.lmacProcessTxopSucc~5667
    SYMBOL rodata_wlog_error.20  SYMBOL lmacConfMib  SYMBOL wifi_log
    SYMBOL ppRecordBarRRC  SYMBOL ppResortTxAMPDU  SYMBOL ppRecycleAmpdu
    SYMBOL lmacDiscardMSDU
$24 L:
  A1 48 ENTRY,  A10 A2 MOV.N,  A8 A10 18 L8UI,  A2 A2 0 L32I.N,  A8 6 $55 L BEQI,
  A2 $32F MOVI,  A12 1 MOVI.N,  A11 A12 11 SLLI,  A15 $0 L L32R,  A10 6 MOVI.N,
  A14 $4 L L32R,  A13 $8 L L32R,  A2 A1 0 S32I,  MEMW,  A8 $10 L L32R,  A8 CALLX8,
$50 L:
  $50 L J,
  2 ZEROS,
$55 L:
  A8 0 MOVI.N,  A8 A10 18 S8I,  MEMW,  A8 0 MOVI.N,  A13 1 MOVI.N,  A8 A10 0 S32I.N,
  A8 A2 44 L32I.N,  A9 A8 0 L32I.N,  A9 21 $94 L BBCI,
  A11 A8 5 L8UI,  A11 A11 2 ADDI,  A11 A11 0 8 EXTUI,  A10 A8 16 L32I.N,
  A10 A10 20 4 EXTUI,  MEMW,  A8 $14 L L32R,  A8 CALLX8,
  A2 A2 44 L32I,  A10 A2 16 L32I,  A10 A10 20 4 EXTUI,  A8 $18 L L32R,  A8 CALLX8,
  $CA L J,
  1 ZEROS,
$94 L:
  A9 22 $A4 L BBCI,
  A11 A13 MOV.N,  A10 A2 MOV.N,  A8 $1C L L32R,  A8 CALLX8,
  $CA L J,
$A4 L:
  A9 $C L L32R,  A12 A8 6 L8UI,  A11 A9 21 L8UI,  A12 A11 $B5 L BLTU,
  A12 2 MOVI.N,  $C2 L J,
$B5 L:
  A11 A8 7 L8UI,  A12 3 MOVI.N,  A8 A9 20 L8UI,  A11 A8 $C2 L BGEU,
  A12 4 MOVI.N,
$C2 L:
  A11 A2 MOV.N,  A8 $20 L L32R,  A8 CALLX8,
$CA L:
  RETW.N,
$CC L:  END-CODE

CODE lmacRecycleMPDU   \ .wifi0iram.18 $4
  $0 1 LITERALS
    SYMBOL lmacTxDone
$4 L:
  A1 32 ENTRY,  A8 A2 28 L32I.N,  A10 A3 MOV.N,  A8 A8 1 ADDI.N,  A11 A4 0 8 EXTUI,
  A8 A2 28 S32I.N,  A8 1 MOVI.N,  A2 A3 44 L32I.N,  A8 A2 19 S8I,  MEMW,  MEMW,
  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$27 L:  END-CODE

CODE lmacEndFrameExchangeSequence   \ .wifi0iram.21 $74
  $0 29 LITERALS
    SYMBOL rodata.__FUNCTION__$10  SYMBOL rodata.lmacProcessTxopSucc~5667
    SYMBOL rodata_wlog_error.22  SYMBOL lmacConfMib  SYMBOL rodata_wlog_error.23
    SYMBOL pTxRx  SYMBOL rodata_wlog_error.24  SYMBOL rodata_wlog_error.25
    SYMBOL rodata_wlog_error.26  SYMBOL wifi_log  SYMBOL ppRecordBarRRC
    SYMBOL ppResortTxAMPDU  SYMBOL ppRecycleAmpdu  SYMBOL lmacDiscardMSDU
    SYMBOL wifi_log  SYMBOL memset  SYMBOL hal_mac_tx_get_blockack
    SYMBOL hal_mac_tx_is_cbw40  SYMBOL ppTxqUpdateBitmap  SYMBOL ppRecycleAmpdu
    SYMBOL ppResortTxAMPDU  SYMBOL wifi_log  SYMBOL lmacRecycleMPDU
    SYMBOL wifi_log  SYMBOL wifi_log  SYMBOL lmacRetryTxFrame
    SYMBOL lmacIsLongFrame  SYMBOL lmacRetryTxFrame  SYMBOL lmacEndRetryAMPDUFail
$74 L:
  A1 64 ENTRY,  A4 A4 0 8 EXTUI,  A10 A2 18 L8UI,  A11 A3 0 8 EXTUI,
  A4 $27E L BNEZ,
  A10 6 $89 L BEQI,
  $130 L J,
$89 L:
  A4 A2 18 S8I,  MEMW,  A5 A2 0 L32I.N,  A11 $BA L BNEZ.N,
  A3 9 MOVI.N,  A5 $BC L BNEZ.N,
  A2 $48A MOVI,  A12 1 MOVI.N,  A15 $0 L L32R,  A11 A12 11 SLLI,  A14 $4 L L32R,
  A13 $8 L L32R,  A2 A1 0 S32I,  MEMW,  A8 $24 L L32R,  A8 CALLX8,
$B4 L:
  $B4 L J,
  3 ZEROS,
$BA L:
  A3 10 MOVI.N,
$BC L:
  A3 A2 19 S8I,  MEMW,  A13 1 MOVI.N,  A3 A5 44 L32I.N,  A4 A3 0 L32I.N,
  A4 21 $F4 L BBCI,
  A11 A3 5 L8UI,  A11 A11 2 ADDI.N,  A11 A11 0 8 EXTUI,  A10 A3 16 L32I.N,
  A10 A10 20 4 EXTUI,  MEMW,  A8 $28 L L32R,  A8 CALLX8,
  A2 A5 44 L32I,  A10 A2 16 L32I,  A10 A10 20 4 EXTUI,  A8 $2C L L32R,  A8 CALLX8,
  $316 L J,
  1 ZEROS,
$F4 L:
  A4 22 $105 L BBCI,
  A11 A13 MOV.N,  A10 A5 MOV.N,  A8 $30 L L32R,  A8 CALLX8,
  $316 L J,
  1 ZEROS,
$105 L:
  A4 $C L L32R,  A12 2 MOVI.N,  A8 A3 6 L8UI,  A6 A4 21 L8UI,  A8 A6 $122 L BGEU,
  A12 4 MOVI,  A6 A3 7 L8UI,  A3 A4 20 L8UI,  A6 A3 $122 L BLTU,
  A12 3 MOVI,
$122 L:
  A11 A5 MOV.N,  A10 A2 MOV.N,  A8 $34 L L32R,  A8 CALLX8,
  $316 L J,
  1 ZEROS,
$130 L:
  A10 5 $136 L BEQI,
  $258 L J,
$136 L:
  A11 A2 0 L32I.N,  A3 1 MOVI.N,  A4 A2 18 S8I,  MEMW,  A11 $166 L BNEZ.N,
  A2 $4AA MOVI,  A12 A3 MOV.N,  A11 A3 11 SLLI,  A15 $0 L L32R,  A10 6 MOVI.N,
  A14 $4 L L32R,  A13 $10 L L32R,  A2 A1 0 S32I.N,  MEMW,  A8 $38 L L32R,
  A8 CALLX8,
$160 L:
  $160 L J,
  3 ZEROS,
$166 L:
  A6 A11 44 L32I.N,  A5 A6 0 L32I.N,  A5 19 $170 L BBSI,
  $219 L J,
$170 L:
  A11 A4 MOV.N,  A12 12 MOVI.N,  A10 A1 16 ADDI,  A8 $3C L L32R,  A8 CALLX8,
  A4 A6 4 L32I.N,  A11 A1 16 ADDI,  A4 A4 0 4 EXTUI,  A3 A3 21 SLLI,  A10 A2 4 L8UI,
  A4 A1 16 S8I,  MEMW,  MEMW,  A8 $40 L L32R,  A8 CALLX8,
  A4 A2 0 L32I.N,  A4 A4 44 L32I.N,  A5 A4 0 L32I.N,  A5 A5 A3 AND,
  A10 A4 16 L32I.N,  A5 $1D5 L BNEZ.N,
  A3 $14 L L32R,  A4 A3 0 L32I.N,  A3 A10 20 4 EXTUI,  A3 A3 A3 ADDX2,
  A3 A3 4 SLLI,  A4 A4 A3 ADD.N,  A10 A2 4 L8UI,  A8 $44 L L32R,  A8 CALLX8,
  A14 A5 MOV.N,  A3 A2 0 L32I.N,  A3 A3 44 L32I.N,  A10 A4 77 S8I,  MEMW,
  A10 A3 16 L32I.N,  MEMW,  $1DD L J,
  1 ZEROS,
$1D5 L:
  A14 A4 5 L8UI,  A14 A14 1 ADDI.N,  A14 A14 0 8 EXTUI,
$1DD L:
  A10 A10 20 4 EXTUI,  A13 A1 24 L32I.N,  A12 A1 20 L32I.N,  A11 A1 18 L16UI,
  A8 $48 L L32R,  A8 CALLX8,
  A10 A2 0 L32I.N,  A3 A10 44 L32I.N,  A4 A3 0 L32I.N,  A4 17 $209 L BBCI,
  A4 A10 26 L8UI,  A4 5 $209 L BEQI,
  A11 1 MOVI.N,  A8 $4C L L32R,  A8 CALLX8,
  $214 L J,
  2 ZEROS,
$209 L:
  A10 A3 16 L32I.N,  A10 A10 20 4 EXTUI,  A8 $50 L L32R,  A8 CALLX8,
$214 L:
  A3 1 MOVI.N,  $2FF L J,
$219 L:
  A5 22 $23F L BBCI,
  A2 $4C9 MOVI,  A15 $0 L L32R,  A12 A3 MOV.N,  A11 A3 11 SLLI,  A10 6 MOVI,
  A14 $4 L L32R,  A13 $18 L L32R,  A2 A1 0 S32I,  MEMW,  A8 $54 L L32R,  A8 CALLX8,
$23C L:
  $23C L J,
$23F L:
  A4 3 MOVI.N,  A4 A2 19 S8I,  MEMW,  A12 A3 MOV.N,  A10 A2 MOV.N,  MEMW,
  A8 $58 L L32R,  A8 CALLX8,
  $316 L J,
  1 ZEROS,
$258 L:
  A2 $4D8 MOVI,  A12 1 MOVI.N,  A15 $0 L L32R,  A11 A12 11 SLLI,  A10 6 MOVI,
  A14 $4 L L32R,  A13 $1C L L32R,  A2 A1 0 S32I,  MEMW,  A8 $5C L L32R,  A8 CALLX8,
$278 L:
  $278 L J,
  3 ZEROS,
$27E L:
  A10 A10 -1 ADDI.N,  A10 A10 0 8 EXTUI,  A10 2 $2A8 L BLTUI,
  A2 $4DB MOVI,  A12 1 MOVI.N,  A15 $0 L L32R,  A11 A12 11 SLLI,  A10 6 MOVI.N,
  A14 $4 L L32R,  A13 $20 L L32R,  A2 A1 0 S32I.N,  MEMW,  A8 $60 L L32R,
  A8 CALLX8,
$2A4 L:
  $2A4 L J,
  1 ZEROS,
$2A8 L:
  A11 $2C2 L BNEZ.N,
  A3 3 MOVI.N,  A3 A2 18 S8I,  MEMW,  A10 A2 MOV.N,  MEMW,  A8 $64 L L32R,
  A8 CALLX8,
  A3 5 MOVI.N,  $2FF L J,
$2C2 L:
  A3 A2 0 L32I.N,  A4 A3 44 L32I.N,  A5 A4 0 L32I.N,  A5 22 $2E8 L BBCI,
  A5 $C L L32R,  A4 A4 5 L8UI,  A6 A5 43 L8UI,  A4 A6 $2E8 L BLTU,
  A10 A3 A3 OR,  A8 $68 L L32R,  A8 CALLX8,
  A10 $30C L BNEZ.N,
  A5 A5 44 L8UI,  A4 A5 $30C L BGEU,
$2E8 L:
  A3 3 MOVI.N,  A11 0 MOVI.N,  A3 A2 18 S8I,  MEMW,  A10 A2 MOV.N,  MEMW,
  A8 $6C L L32R,  A8 CALLX8,
  A3 7 MOVI.N,
$2FF L:
  A3 A2 19 S8I,  MEMW,  MEMW,  $316 L J,
  1 ZEROS,
$30C L:
  A11 A3 MOV.N,  A10 A2 MOV.N,  A8 $70 L L32R,  A8 CALLX8,
$316 L:
  RETW.N,
$318 L:  END-CODE

CODE lmacProcessTBSuccess   \ .wifi0iram.12 $18
  $0 6 LITERALS
    SYMBOL rodata.__FUNCTION__$13  SYMBOL rodata.lmacProcessTxopSucc~5667
    SYMBOL rodata_wlog_error.13  SYMBOL bss.our_instances  SYMBOL wifi_log
    SYMBOL lmacEndFrameExchangeSequence
$18 L:
  A1 48 ENTRY,  A2 A2 0 8 EXTUI,  A2 4 $45 L BLTUI,
  A2 $29D MOVI,  A12 1 MOVI.N,  A15 $0 L L32R,  A11 A12 11 SLLI,  A10 6 MOVI.N,
  A14 $4 L L32R,  A13 $8 L L32R,  A2 A1 0 S32I,  MEMW,  A8 $10 L L32R,  A8 CALLX8,
$40 L:
  $40 L J,
  2 ZEROS,
$45 L:
  A10 $C L L32R,  A8 A2 A2 ADDX8,  A8 A8 A10 ADDX4,  A9 5 MOVI.N,  A12 0 MOVI.N,
  A9 A8 18 S8I,  MEMW,  A11 1 MOVI.N,  A10 A8 MOV.N,  A2 A2 A2 ADDX8,  MEMW,
  A8 $14 L L32R,  A8 CALLX8,
  RETW.N,
$6A L:  END-CODE

CODE lmacProcessTxSuccess   \ .wifiextrairam.14 $1C
  $0 7 LITERALS
    SYMBOL rodata.__FUNCTION__$12  SYMBOL rodata.lmacProcessTxopSucc~5667
    SYMBOL rodata_wlog_error.15  SYMBOL bss.our_instances  SYMBOL wifi_log
    SYMBOL lmacIsLongFrame  SYMBOL lmacEndFrameExchangeSequence
$1C L:
  A1 48 ENTRY,  A2 A2 0 8 EXTUI,  A14 A3 0 8 EXTUI,  A2 5 $4C L BLTUI,
  A2 $2AC MOVI,  A12 1 MOVI.N,  A15 $0 L L32R,  A11 A12 11 SLLI,  A10 6 MOVI,
  A14 $4 L L32R,  A13 $8 L L32R,  A2 A1 0 S32I,  MEMW,  A8 $10 L L32R,  A8 CALLX8,
$48 L:
  $48 L J,
  1 ZEROS,
$4C L:
  A6 $C L L32R,  A5 A2 A2 ADDX8,  A5 A5 A6 ADDX4,  A11 $402 MOVI,  A4 A2 3 SLLI,
  A10 A5 0 L32I.N,  A7 A10 44 L32I.N,  A9 A7 0 L32I.N,  A11 A9 A11 AND,
  A3 A5 9 L8UI,  A11 $79 L BNEZ.N,
  A12 9 MOVI.N,  A12 A12 19 SLLI,  A13 1 MOVI.N,  A12 A9 A12 AND,  A13 A13 22 SLLI,
  A12 A13 $AB L BNE,
$79 L:
  A8 $14 L L32R,  A8 CALLX8,
  A8 A4 A2 ADD.N,  A8 A8 A6 ADDX4,  A3 A8 8 S8I,  MEMW,  A10 $9C L BEQZ.N,
  A3 0 MOVI.N,  A3 A8 12 S8I,  MEMW,  MEMW,  $A2 L J,
  2 ZEROS,
$9C L:
  A10 A8 11 S8I,  MEMW,
$A2 L:
  A14 127 MOVI,  MEMW,  $CB L J,
$AB L:
  A10 $100 MOVI,  A9 A9 A10 AND,  A3 A5 8 S8I,  MEMW,  A9 $C5 L BNEZ.N,
  A9 A5 11 S8I,  MEMW,  MEMW,  $CB L J,
$C5 L:
  A11 A5 11 S8I,  A11 A5 12 S8I,
$CB L:
  A4 A4 A2 ADD.N,  A4 A4 A6 ADDX4,  A2 5 MOVI.N,  A12 0 MOVI.N,  A14 A7 13 S8I,
  A11 1 MOVI.N,  A10 A5 MOV.N,  A2 A4 18 S8I,  A2 1 MOVI,  A2 A7 19 S8I,  MEMW,
  MEMW,  A8 $18 L L32R,  A8 CALLX8,
  RETW.N,
$F2 L:  END-CODE

CODE lmacProcessShortRetryFail   \ .text.lmacProcessShortRetryFail $48
  $0 18 LITERALS
    SYMBOL rodata.__FUNCTION__$7  SYMBOL rodata.lmacProcessTxopSucc~5667
    SYMBOL rodata_wlog_error.38  SYMBOL lmacConfMib  SYMBOL rodata_wlog_error.39
    $400100 T,  $600002 T,  SYMBOL rodata_wlog_error.40  SYMBOL wifi_log
    SYMBOL wifi_log  SYMBOL rcReachRetryLimit  SYMBOL lmacMSDUAged
    SYMBOL lmacIsLongFrame  SYMBOL lmacEndRetryAMPDUFail
    SYMBOL lmacDiscardFrameExchangeSe~E31C  SYMBOL lmacEndFrameExchangeSequence
    SYMBOL wifi_log  SYMBOL lmacRetryTxFrame
$48 L:
  A1 48 ENTRY,  A3 A3 0 8 EXTUI,  A4 A4 0 8 EXTUI,  A5 A5 0 8 EXTUI,  A6 1 MOVI.N,
  A7 A4 A3 OR,  A5 A6 $5E L BNE,
  A7 $62 L BEQZ.N,
$5E L:
  A5 A2 0 L32I.N,  A5 $84 L BNEZ.N,
$62 L:
  A2 $64D MOVI,  A12 1 MOVI.N,  A15 $0 L L32R,  A11 A12 11 SLLI,  A10 6 MOVI.N,
  A14 $4 L L32R,  A13 $8 L L32R,  A2 A1 0 S32I.N,  MEMW,  A8 $20 L L32R,  A8 CALLX8,
$80 L:
  $80 L J,
  1 ZEROS,
$84 L:
  A6 $C L L32R,  A8 A2 11 L8UI,  A9 A6 21 L8UI,  A8 A9 $B1 L BGEU,
  A8 A8 1 ADDI.N,  A8 A8 0 8 EXTUI,  A8 A2 11 S8I,  MEMW,  A8 A9 $B1 L BGEU,
  A8 A2 8 L8UI,  A9 A2 10 L8UI,  A8 A9 $BA L BGEU,
  A8 A8 1 ADDI.N,  MEMW,  $B4 L J,
  2 ZEROS,
$B1 L:
  A8 A2 9 L8UI,
$B4 L:
  A8 A2 8 S8I,  MEMW,
$BA L:
  A10 A5 44 L32I.N,  A8 A10 6 L8UI,  A8 A8 1 ADDI.N,  A8 A10 6 S8I,  MEMW,
  A7 $D4 L BNEZ.N,
  A8 A10 5 L8UI,  A8 A8 1 ADDI.N,  A8 A10 5 S8I,  MEMW,
$D4 L:
  A8 A2 18 L8UI,  A8 A8 -1 ADDI.N,  A8 A8 0 8 EXTUI,  A8 2 $100 L BLTUI,
  A2 $672 MOVI,  A12 1 MOVI.N,  A15 $0 L L32R,  A11 A12 11 SLLI,  A10 6 MOVI.N,
  A14 $4 L L32R,  A13 $10 L L32R,  A2 A1 0 S32I.N,  MEMW,  A8 $24 L L32R,
  A8 CALLX8,
$FD L:
  $FD L J,
$100 L:
  A8 $28 L L32R,  A8 CALLX8,
  A10 $119 L BEQZ.N,
$108 L:
  A7 A5 44 L32I.N,  A8 $14 L L32R,  A4 A7 0 L32I.N,  A4 A4 A8 AND,
  A4 A8 $172 L BNE,
  $141 L J,
  1 ZEROS,
$119 L:
  A7 A5 44 L32I.N,  A8 A7 6 L8UI,  A7 A6 21 L8UI,  A8 A7 $108 L BGEU,
  A10 A5 MOV.N,  A8 $2C L L32R,  A8 CALLX8,
  A10 $108 L BNEZ,
  A7 A5 44 L32I.N,  A11 1 MOVI.N,  A11 A11 28 SLLI,  A8 A7 0 L32I.N,
  A11 A8 A11 AND,  A11 $108 L BNEZ,
  $18F L J,
$141 L:
  A10 A5 MOV.N,  A8 $30 L L32R,  A8 CALLX8,
  A10 $151 L BEQZ.N,
  A3 A6 43 L8UI,  $154 L J,
$151 L:
  A3 A6 44 L8UI,
$154 L:
  A11 A5 MOV.N,  A10 A2 MOV.N,  A3 A7 5 S8I,  MEMW,  A3 0 MOVI.N,  A3 A2 0 S32I,
  MEMW,  A8 $34 L L32R,  A8 CALLX8,
  $22C L J,
  3 ZEROS,
$172 L:
  A4 6 MOVI.N,  A4 A2 18 S8I,  MEMW,  A12 A3 MOV.N,  A11 1 MOVI.N,  A3 $1E1 L BEQZ,
  A10 A2 MOV.N,  MEMW,  A8 $38 L L32R,  A8 CALLX8,
  $22C L J,
$18F L:
  A3 $1F0 L BNEZ,
  A4 $1DD L BNEZ,
  A3 $18 L L32R,  A8 A3 $1B8 L BANY,
  A3 A5 4 L32I.N,  A3 A3 4 L32I.N,  A8 18 $1A4 L BBCI,
  A3 A3 4 ADDI.N,
$1A4 L:
  A5 8 MOVI.N,  A4 A3 1 L8UI,  A4 A4 A5 OR,  A4 A3 1 S8I,  MEMW,  MEMW,  $1DD L J,
$1B8 L:
  A8 22 $1DD L BBCI,
  A6 8 MOVI.N,
$1BD L:
  A4 A5 44 L32I.N,  A3 A5 4 L32I.N,  A4 A4 0 L32I,  A3 A3 4 L32I.N,
  A4 18 $1CC L BBCI,
  A3 A3 4 ADDI,
$1CC L:
  A4 A3 1 L8UI,  A4 A4 A6 OR,  A4 A3 1 S8I,  MEMW,  A5 A5 40 L32I.N,
  A5 $1BD L BNEZ,
$1DD L:
  A12 1 MOVI.N,  A11 A12 MOV.N,
$1E1 L:
  A10 A2 MOV.N,  MEMW,  A8 $3C L L32R,  A8 CALLX8,
  $22C L J,
  1 ZEROS,
$1F0 L:
  A3 A2 18 L8UI,  A3 1 $219 L BEQI,
  A2 $6B9 MOVI,  A12 1 MOVI.N,  A15 $0 L L32R,  A11 A12 11 SLLI,  A10 6 MOVI.N,
  A14 $4 L L32R,  A13 $1C L L32R,  A2 A1 0 S32I.N,  MEMW,  A8 $40 L L32R,
  A8 CALLX8,
$214 L:
  $214 L J,
  2 ZEROS,
$219 L:
  A3 4 MOVI.N,  A3 A2 18 S8I,  MEMW,  A10 A2 MOV.N,  MEMW,  A8 $44 L L32R,
  A8 CALLX8,
$22C L:
  RETW.N,
$22E L:  END-CODE

CODE lmacProcessCtsTimeout   \ .text.lmacProcessCtsTimeout $18
  $0 6 LITERALS
    SYMBOL bss.our_instances  SYMBOL rodata.__FUNCTION__$5
    SYMBOL rodata.lmacProcessTxopSucc~5667  SYMBOL rodata_wlog_error.44
    SYMBOL wifi_log  SYMBOL lmacProcessShortRetryFail
$18 L:
  A1 48 ENTRY,  A2 A2 0 8 EXTUI,  A13 A3 0 8 EXTUI,  A2 10 $30 L BNEI,
  A2 0 MOVI,  MEMW,  A2 A2 44 L32I,  1 15 BREAK,
$30 L:
  A10 $0 L L32R,  A8 A2 A2 ADDX8,  A8 A8 A10 ADDX4,  A8 A8 18 L8UI,
  A8 A8 -1 ADDI.N,  A8 A8 0 8 EXTUI,  A8 2 $68 L BLTUI,
  A2 $789 MOVI,  A12 1 MOVI.N,  A15 $4 L L32R,  A11 A12 11 SLLI,  A10 6 MOVI,
  A14 $8 L L32R,  A13 $C L L32R,  A2 A1 0 S32I,  MEMW,  A8 $10 L L32R,  A8 CALLX8,
$64 L:
  $64 L J,
  1 ZEROS,
$68 L:
  A2 A2 A2 ADDX8,  A12 1 MOVI.N,  A11 0 MOVI.N,  A10 A2 A10 ADDX4,  A8 $14 L L32R,
  A8 CALLX8,
  RETW.N,
$7A L:  END-CODE

CODE lmacProcessLongRetryFail   \ .text.lmacProcessLongRetryFail $34
  $0 13 LITERALS
    SYMBOL lmacConfMib  SYMBOL rodata.__FUNCTION__$6
    SYMBOL rodata.lmacProcessTxopSucc~5667  SYMBOL rodata_wlog_error.42
    $400002 T,  SYMBOL rodata_wlog_error.43  SYMBOL wifi_log
    SYMBOL rcReachRetryLimit  SYMBOL lmacMSDUAged
    SYMBOL lmacDiscardFrameExchangeSe~E31C  SYMBOL lmacEndFrameExchangeSequence
    SYMBOL wifi_log  SYMBOL lmacRetryTxFrame
$34 L:
  A1 48 ENTRY,  A4 A4 0 8 EXTUI,  A5 0 MOVI.N,  A3 A3 0 8 EXTUI,  A4 A5 $44 L BNE,
  A5 A2 0 L32I.N,
$44 L:
  A6 $0 L L32R,  A8 A2 12 L8UI,  A9 A6 20 L8UI,  A8 A9 $71 L BGEU,
  A8 A8 1 ADDI.N,  A8 A8 0 8 EXTUI,  A8 A2 12 S8I,  MEMW,  A8 A9 $71 L BGEU,
  A8 A2 8 L8UI,  A9 A2 10 L8UI,  A8 A9 $7A L BGEU,
  A8 A8 1 ADDI.N,  MEMW,  $74 L J,
  2 ZEROS,
$71 L:
  A8 A2 9 L8UI,
$74 L:
  A8 A2 8 S8I,  MEMW,
$7A L:
  A10 A5 44 L32I.N,  A8 A10 7 L8UI,  A8 A8 1 ADDI.N,  A8 A10 7 S8I,  MEMW,
  A3 $94 L BNEZ.N,
  A8 A10 5 L8UI,  A8 A8 1 ADDI.N,  A8 A10 5 S8I,  MEMW,
$94 L:
  A8 A2 18 L8UI,  A8 A8 -1 ADDI.N,  A8 A8 0 8 EXTUI,  A8 2 $C0 L BLTUI,
  A2 $747 MOVI,  A12 1 MOVI.N,  A15 $4 L L32R,  A11 A12 11 SLLI,  A10 6 MOVI.N,
  A14 $8 L L32R,  A13 $C L L32R,  A2 A1 0 S32I.N,  MEMW,  A8 $18 L L32R,  A8 CALLX8,
$BD L:
  $BD L J,
$C0 L:
  A8 $1C L L32R,  A8 CALLX8,
  A10 $D9 L BEQZ.N,
$C8 L:
  A5 6 MOVI.N,  A5 A2 18 S8I,  MEMW,  A3 $F2 L BEQZ.N,
  MEMW,  $FC L J,
  1 ZEROS,
$D9 L:
  A8 A5 44 L32I.N,  A6 A6 20 L8UI,  A8 A8 7 L8UI,  A8 A6 $C8 L BGEU,
  A10 A5 MOV.N,  A8 $20 L L32R,  A8 CALLX8,
  A10 $C8 L BNEZ,
  $107 L J,
$F2 L:
  A12 A4 MOV.N,  A11 1 MOVI.N,  A4 $15D L BEQZ,
  $1A9 L J,
$FC L:
  A10 A2 MOV.N,  A8 $24 L L32R,  A8 CALLX8,
  $1A9 L J,
$107 L:
  A3 $16C L BNEZ,
  A3 A5 44 L32I.N,  A6 $10 L L32R,  A3 A3 0 L32I.N,  A3 A6 $131 L BANY,
  A5 A5 4 L32I.N,  A5 A5 4 L32I.N,  A3 18 $11D L BBCI,
  A5 A5 4 ADDI.N,
$11D L:
  A6 8 MOVI.N,  A3 A5 1 L8UI,  A3 A3 A6 OR,  A3 A5 1 S8I,  MEMW,  MEMW,  $155 L J,
$131 L:
  A3 22 $155 L BBCI,
  A8 8 MOVI.N,
$136 L:
  A3 A5 4 L32I.N,  A6 A3 4 L32I.N,  A3 A5 44 L32I.N,  A3 A3 0 L32I.N,
  A3 18 $144 L BBCI,
  A6 A6 4 ADDI,
$144 L:
  A3 A6 1 L8UI,  A3 A3 A8 OR,  A3 A6 1 S8I,  MEMW,  A5 A5 40 L32I.N,
  A5 $136 L BNEZ,
$155 L:
  A4 $1A9 L BNEZ,
  A12 1 MOVI.N,  A11 A12 A12 OR,
$15D L:
  A10 A2 MOV.N,  MEMW,  A8 $28 L L32R,  A8 CALLX8,
  $1A9 L J,
  1 ZEROS,
$16C L:
  A3 A2 18 L8UI,  A3 1 $194 L BEQI,
  A2 $776 MOVI,  A12 1 MOVI.N,  A15 $4 L L32R,  A11 A12 11 SLLI,  A10 6 MOVI.N,
  A14 $8 L L32R,  A13 $14 L L32R,  A2 A1 0 S32I.N,  MEMW,  A8 $2C L L32R,
  A8 CALLX8,
$190 L:
  $190 L J,
  1 ZEROS,
$194 L:
  A3 4 MOVI.N,  A11 A10 MOV.N,  A3 A2 18 S8I,  MEMW,  A10 A2 MOV.N,  MEMW,
  A8 $30 L L32R,  A8 CALLX8,
$1A9 L:
  RETW.N,
$1AB L:  END-CODE

CODE lmacProcessCollision   \ .text.lmacProcessCollision $30
  $0 12 LITERALS
    SYMBOL rodata.__FUNCTION__$9  SYMBOL rodata.lmacProcessTxopSucc~5667
    SYMBOL rodata_wlog_error.28  SYMBOL bss.our_instances
    SYMBOL rodata_wlog_error.29  SYMBOL rodata_wlog_error.30  SYMBOL wifi_log
    SYMBOL wifi_log  SYMBOL wifi_log  SYMBOL lmacIsLongFrame
    SYMBOL lmacProcessShortRetryFail  SYMBOL lmacProcessLongRetryFail
$30 L:
  A1 48 ENTRY,  A2 A2 0 8 EXTUI,  A2 5 $5D L BLTUI,
  A2 $52F MOVI,  A12 1 MOVI.N,  A15 $0 L L32R,  A11 A12 11 SLLI,  A10 6 MOVI.N,
  A14 $4 L L32R,  A13 $8 L L32R,  A2 A1 0 S32I,  MEMW,  A8 $18 L L32R,  A8 CALLX8,
$58 L:
  $58 L J,
  2 ZEROS,
$5D L:
  A8 $C L L32R,  A3 A2 A2 ADDX8,  A3 A3 A8 ADDX4,  A10 A3 0 L32I.N,
  A10 $8C L BNEZ.N,
  A2 $531 MOVI,  A12 1 MOVI.N,  A15 $0 L L32R,  A11 A12 11 SLLI,  A10 6 MOVI.N,
  A14 $4 L L32R,  A13 $10 L L32R,  A2 A1 0 S32I.N,  MEMW,  A8 $1C L L32R,
  A8 CALLX8,
$88 L:
  $88 L J,
  1 ZEROS,
$8C L:
  A4 A3 18 L8UI,  A4 1 $B4 L BEQI,
  A2 $532 MOVI,  A12 1 MOVI.N,  A15 $0 L L32R,  A11 A12 11 SLLI,  A10 6 MOVI.N,
  A14 $4 L L32R,  A13 $14 L L32R,  A2 A1 0 S32I.N,  MEMW,  A8 $20 L L32R,
  A8 CALLX8,
$B0 L:
  $B0 L J,
  1 ZEROS,
$B4 L:
  A3 A10 44 L32I.N,  A2 A2 A2 ADDX8,  A2 A2 A8 ADDX4,  A8 A3 0 L32I.N,
  A3 $300 MOVI,  A3 A3 A8 AND,  A3 $CE L BNEZ.N,
  A8 $24 L L32R,  A8 CALLX8,
  A10 $E0 L BNEZ.N,
$CE L:
  A12 1 MOVI.N,  A13 0 MOVI.N,  A11 A12 MOV.N,  A10 A2 MOV.N,  A8 $28 L L32R,
  A8 CALLX8,
  $EC L J,
  1 ZEROS,
$E0 L:
  A12 A3 MOV.N,  A11 A4 MOV.N,  A10 A2 MOV.N,  A8 $2C L L32R,  A8 CALLX8,
$EC L:
  RETW.N,
$EE L:  END-CODE

CODE lmacProcessCollisions_task   \ .text.lmacProcessCollisions_task $10
  $0 4 LITERALS
    SYMBOL hal_mac_get_txq_state  SYMBOL hal_mac_txq_disable
    SYMBOL hal_mac_clr_txq_state  SYMBOL lmacProcessCollision
$10 L:
  A1 32 ENTRY,  A10 0 MOVI,  A8 $0 L L32R,  A8 CALLX8,
  A3 A10 A10 OR,  A10 $60 L BEQZ.N,
  A4 1 MOVI,
$24 L:
  A10 A3 NEG,  A10 A10 A3 AND,  A10 A10 NSAU,  A10 A10 NEG,  A2 A10 31 ADDI,
  A10 A2 A2 OR,  A8 $4 L L32R,  A8 CALLX8,
  A2 SSL,  A5 A4 SLL,  A11 A5 MOV.N,  A10 0 MOVI,  A8 $8 L L32R,  A8 CALLX8,
  A10 A2 MOV.N,  A8 $C L L32R,  A8 CALLX8,
  A11 -1 MOVI.N,  A11 A11 A5 XOR,  A3 A3 A11 AND,  A3 $24 L BNEZ,
$60 L:
  RETW.N,
$62 L:  END-CODE

CODE lmacProcessAckTimeout   \ .text.lmacProcessAckTimeout $2C
  $0 11 LITERALS
    SYMBOL rodata.__FUNCTION__$4  SYMBOL rodata.lmacProcessTxopSucc~5667
    SYMBOL rodata_wlog_error.45  SYMBOL rodata_wlog_error.46
    SYMBOL bss.our_instances  SYMBOL rodata_wlog_error.47  SYMBOL wifi_log
    SYMBOL wifi_log  SYMBOL wifi_log  SYMBOL lmacProcessLongRetryFail
    SYMBOL lmacProcessShortRetryFail
$2C L:
  A1 48 ENTRY,  A2 A2 0 8 EXTUI,  A3 A3 0 8 EXTUI,  A2 10 $5B L BNEI,
  A2 $799 MOVI,  A12 1 MOVI.N,  A15 $0 L L32R,  A11 A12 11 SLLI,  A10 6 MOVI,
  A14 $4 L L32R,  A13 $8 L L32R,  A2 A1 0 S32I,  MEMW,  A8 $18 L L32R,  A8 CALLX8,
$58 L:
  $58 L J,
$5B L:
  A2 5 $80 L BLTUI,
  A2 $79C MOVI,  A12 1 MOVI.N,  A15 $0 L L32R,  A11 A12 11 SLLI,  A10 6 MOVI.N,
  A14 $4 L L32R,  A13 $C L L32R,  A2 A1 0 S32I.N,  MEMW,  A8 $1C L L32R,  A8 CALLX8,
$7C L:
  $7C L J,
  1 ZEROS,
$80 L:
  A8 $10 L L32R,  A10 A2 A2 ADDX8,  A10 A10 A8 ADDX4,  A3 $91 L BNEZ.N,
  A2 A10 MOV.N,  A8 A10 0 L32I.N,  A8 $B4 L BNEZ.N,
$91 L:
  A2 $7A7 MOVI,  A12 1 MOVI.N,  A15 $0 L L32R,  A11 A12 11 SLLI,  A10 6 MOVI.N,
  A14 $4 L L32R,  A13 $14 L L32R,  A2 A1 0 S32I,  MEMW,  A8 $20 L L32R,  A8 CALLX8,
$B0 L:
  $B0 L J,
  1 ZEROS,
$B4 L:
  A8 A8 44 L32I.N,  A9 A8 0 L32I.N,  A8 $100 MOVI,  A11 A8 A9 AND,
  A11 $DC L BEQZ.N,
  A12 A3 MOV.N,  A8 A10 9 L8UI,  A11 A3 MOV.N,  A8 A10 8 S8I,  A3 A10 11 S8I,  MEMW,
  MEMW,  A8 $24 L L32R,  A8 CALLX8,
  $E6 L J,
$DC L:
  A13 A11 MOV.N,  A12 A11 MOV.N,  A8 $28 L L32R,  A8 CALLX8,
$E6 L:
  RETW.N,
$E8 L:  END-CODE

CODE lmacProcessTxRtsError   \ .text.lmacProcessTxRtsError $2C
  $0 11 LITERALS
    SYMBOL rodata.__FUNCTION__$3  SYMBOL rodata.lmacProcessTxopSucc~5667
    SYMBOL rodata_wlog_error.48  SYMBOL bss.our_instances
    SYMBOL rodata_wlog_warning.49  SYMBOL rodata_wlog_error.50  SYMBOL wifi_log
    SYMBOL lmacProcessCollision  SYMBOL lmacEndFrameExchangeSequence
    SYMBOL wifi_log  SYMBOL wifi_log
$2C L:
  A1 48 ENTRY,  A10 A3 0 8 EXTUI,  A14 A2 0 8 EXTUI,  A12 A4 0 8 EXTUI,
  A10 10 $45 L BNEI,
  A8 0 MOVI.N,  MEMW,  A8 A8 44 L32I.N,  1 15 BREAK,
$45 L:
  A10 5 $6C L BLTUI,
  A8 $7CB MOVI,  A12 1 MOVI.N,  A15 $0 L L32R,  A11 A12 11 SLLI,  A10 6 MOVI,
  A14 $4 L L32R,  A13 $8 L L32R,  A8 A1 0 S32I,  MEMW,  A8 $18 L L32R,  A8 CALLX8,
$68 L:
  $68 L J,
  1 ZEROS,
$6C L:
  A14 6 $78 L BGEUI,
  A14 3 $81 L BGEUI,
  A14 1 $81 L BEQI,
  $B1 L J,
$78 L:
  A8 192 MOVI,  A14 A8 $8C L BEQ,
  $B1 L J,
$81 L:
  A8 $1C L L32R,  A8 CALLX8,
  $E4 L J,
  2 ZEROS,
$8C L:
  A9 $C L L32R,  A8 A10 A10 ADDX8,  A8 A8 A9 ADDX4,  A11 6 MOVI.N,  A11 A8 18 S8I,
  MEMW,  A12 $E4 L BNEZ,
  A11 1 MOVI.N,  A10 A8 MOV.N,  MEMW,  A8 $20 L L32R,  A8 CALLX8,
  $E4 L J,
  1 ZEROS,
$B1 L:
  A13 $10 L L32R,  A12 2 MOVI.N,  A11 64 MOVI.N,  A10 6 MOVI.N,  A8 $24 L L32R,
  A8 CALLX8,
  A8 $7F0 MOVI,  A12 1 MOVI.N,  A15 $0 L L32R,  A11 A12 11 SLLI,  A10 6 MOVI,
  A14 $4 L L32R,  A13 $14 L L32R,  A8 A1 0 S32I,  MEMW,  A8 $28 L L32R,  A8 CALLX8,
$E0 L:
  $E0 L J,
  1 ZEROS,
$E4 L:
  RETW.N,
$E6 L:  END-CODE

CODE lmacProcessTxseckiderr   \ .text.lmacProcessTxseckiderr $4
  $0 1 LITERALS
    SYMBOL lmacEndFrameExchangeSequence
$4 L:
  A1 32 ENTRY,  A12 0 MOVI.N,  A11 1 MOVI.N,  A10 A2 A2 OR,  A8 $0 L L32R,
  A8 CALLX8,
  RETW.N,
$16 L:  END-CODE

CODE lmacProcessTxError   \ .text.lmacProcessTxError $28
  $0 10 LITERALS
    SYMBOL rodata.__FUNCTION__$2  SYMBOL rodata.lmacProcessTxopSucc~5667
    SYMBOL rodata_wlog_error.51  $809 T,  SYMBOL bss.our_instances
    SYMBOL wifi_log  SYMBOL lmacProcessCollision  SYMBOL lmacProcessTxseckiderr
    SYMBOL lmacProcessCtsTimeout  SYMBOL lmacProcessAckTimeout
$28 L:
  A1 48 ENTRY,  A10 A2 0 8 EXTUI,  A3 A3 0 8 EXTUI,  A11 A4 0 8 EXTUI,
  A10 10 $41 L BNEI,
  A3 0 MOVI.N,  MEMW,  A3 A3 44 L32I.N,  1 15 BREAK,
$41 L:
  A10 5 $68 L BLTUI,
  A3 $C L L32R,  A12 1 MOVI.N,  A11 A12 11 SLLI,  A10 6 MOVI,  A15 $0 L L32R,
  A14 $4 L L32R,  A13 $8 L L32R,  A3 A1 0 S32I,  MEMW,  A8 $14 L L32R,  A8 CALLX8,
$64 L:
  $64 L J,
  1 ZEROS,
$68 L:
  A3 6 $78 L BGEUI,
  A3 3 $81 L BGEUI,
  A3 $B8 L BEQZ,
  A3 1 $81 L BEQI,
  $C2 L J,
  1 ZEROS,
$78 L:
  A8 192 MOVI,  A3 A8 $91 L BEQ,
  $C2 L J,
$81 L:
  A11 $8C L BNEZ.N,
  A8 $18 L L32R,  A8 CALLX8,
  $C8 L J,
$8C L:
  A11 1 MOVI.N,  $C2 L J,
$91 L:
  A8 $10 L L32R,  A3 A10 A10 ADDX8,  A3 A3 A8 ADDX4,  A9 6 MOVI.N,  A9 A3 18 S8I,
  MEMW,  A11 $C8 L BNEZ.N,
  A3 A10 A10 ADDX8,  A11 A10 MOV.N,  A10 A3 A8 ADDX4,  MEMW,  A8 $1C L L32R,
  A8 CALLX8,
  $C8 L J,
$B8 L:
  A8 $20 L L32R,  A8 CALLX8,
  $C8 L J,
  1 ZEROS,
$C2 L:
  A8 $24 L L32R,  A8 CALLX8,
$C8 L:
  RETW.N,
$CA L:  END-CODE

CODE lmacProcessTxComplete   \ .wifiextrairam.34 $58
  $0 22 LITERALS
    SYMBOL g_osi_funcs_p  SYMBOL g_pp_tx_pkt_num  SYMBOL bss.our_instances
    SYMBOL rodata_wlog_info.35  SYMBOL rodata  SYMBOL rodata.__FUNCTION__$8
    SYMBOL rodata.lmacProcessTxopSucc~5667  SYMBOL rodata_wlog_error.36
    SYMBOL hal_mac_get_txq_state  SYMBOL pm_is_sleeping
    SYMBOL tx_pwctrl_background  SYMBOL hal_mac_clr_txq_state  SYMBOL wifi_log
    SYMBOL hal_mac_get_txq_pmd  SYMBOL hal_mac_clr_txq_state
    SYMBOL wifi_gpio_debug  SYMBOL lmacProcessTxSuccess
    SYMBOL lmacProcessTxRtsError  SYMBOL lmacProcessCtsTimeout
    SYMBOL lmacProcessAckTimeout  SYMBOL lmacProcessTxError  SYMBOL wifi_log
$58 L:
  A1 64 ENTRY,  A10 2 MOVI,  A8 $20 L L32R,  A8 CALLX8,
  A2 $0 L L32R,  A3 A10 MOV.N,  A2 A2 0 L32I.N,  A10 A2 4 L32I.N,  A10 CALLX8,
  A10 $A8 L BEQZ.N,
  A8 $24 L L32R,  A8 CALLX8,
  A10 $A8 L BNEZ.N,
  A4 $4 L L32R,  A2 A4 0 L8UI,  A2 A2 1 ADDI.N,  A2 A2 0 8 EXTUI,  A2 5 $95 L BGEUI,
  A2 A4 0 S8I,  MEMW,  MEMW,  $A8 L J,
  1 ZEROS,
$95 L:
  A10 A4 0 S8I,  MEMW,  A11 A10 MOV.N,  A10 1 MOVI.N,  MEMW,  A8 $28 L L32R,
  A8 CALLX8,
$A8 L:
  A2 0 MOVI.N,  A2 A1 16 S32I.N,  A4 1 MOVI.N,  MEMW,  $1BF L J,
$B4 L:
  A2 A3 NEG,  A2 A2 A3 AND,  A2 A2 NSAU,  A2 A2 NEG,  A2 A2 31 ADDI,  A2 SSL,
  A7 A4 SLL,  A5 -1 MOVI.N,  A5 A5 A7 XOR,  A3 A3 A5 AND,  A6 $8 L L32R,
  A5 A2 3 SLLI,  A2 5 $E5 L BGEI,
  A8 A5 A2 ADD.N,  A8 A8 A6 ADDX4,  A8 A8 18 L8UI,  A8 1 $10D L BEQI,
$E5 L:
  A11 A7 MOV.N,  A5 A5 A2 ADD.N,  A10 2 MOVI.N,  A8 $2C L L32R,  A8 CALLX8,
  A5 A5 A6 ADDX4,  A14 A5 18 L8UI,  A15 A2 MOV.N,  A12 3 MOVI.N,  A11 64 MOVI.N,
  A10 6 MOVI.N,  A13 $C L L32R,  A8 $30 L L32R,  A8 CALLX8,
  $1BF L J,
  2 ZEROS,
$10D L:
  A11 A1 16 ADDI,  A10 A2 MOV.N,  A8 $34 L L32R,  A8 CALLX8,
  A11 A7 MOV.N,  A10 2 MOVI.N,  A8 $38 L L32R,  A8 CALLX8,
  A8 15 MOVI.N,  A5 A1 16 L32I.N,  A8 A8 12 SLLI,  A5 A5 A8 AND,  A11 3 MOVI.N,
  A6 2 MOVI.N,  A11 A6 A5 MOVEQZ,  A10 10 MOVI,  A8 $3C L L32R,  A8 CALLX8,
  A8 A1 16 L32I.N,  A8 A8 12 4 EXTUI,  A8 6 $19C L BGEUI,
  A5 $10 L L32R,  A8 A8 A5 ADDX4,  A5 A8 0 L32I.N,  A5 JX,
  2 ZEROS,
$151 L:
  A11 A1 18 L8UI,  A10 A2 MOV.N,  A8 $40 L L32R,  A8 CALLX8,
  $1BF L J,
$15F L:
  A10 A1 16 L8UI,  A12 0 MOVI.N,  A11 A2 MOV.N,  A8 $44 L L32R,  A8 CALLX8,
  $1BF L J,
$16F L:
  A11 0 MOVI.N,  A10 A2 MOV.N,  A8 $48 L L32R,  A8 CALLX8,
  $1BF L J,
  2 ZEROS,
$17E L:
  A11 0 MOVI.N,  A10 A2 MOV.N,  A8 $4C L L32R,  A8 CALLX8,
  $1BF L J,
$18B L:
  A11 A1 16 L8UI,  A12 0 MOVI.N,  A10 A2 MOV.N,  A8 $50 L L32R,  A8 CALLX8,
  $1BF L J,
  1 ZEROS,
$19C L:
  A2 $627 MOVI,  A12 1 MOVI.N,  A15 $14 L L32R,  A11 A12 11 SLLI,  A10 6 MOVI,
  A14 $18 L L32R,  A13 $1C L L32R,  A2 A1 0 S32I,  MEMW,  A8 $54 L L32R,  A8 CALLX8,
$1BC L:
  $1BC L J,
$1BF L:
  A3 $B4 L BNEZ,
  RETW.N,
  rodata  $151 L!+  $15F L!+  $16F L!+  $19C L!+  $18B L!+  $17E L!+  DROP   \ the table of jumps
$1C4 L:  END-CODE

CODE lmacRxDone   \ .text.lmacRxDone $8
  $0 2 LITERALS
    SYMBOL ppEnqueueRxq  SYMBOL pp_post
$8 L:
  A1 32 ENTRY,  A10 A2 A2 OR,  A8 $0 L L32R,  A8 CALLX8,
  A11 0 MOVI.N,  A10 17 MOVI.N,  A8 $4 L L32R,  A8 CALLX8,
  RETW.N,
$20 L:  END-CODE

CODE lmacDisableTransmit   \ .text.lmacDisableTransmit $20
  $0 8 LITERALS
    SYMBOL bss.our_instances  SYMBOL hal_mac_tx_set_cca  SYMBOL ets_delay_us
    SYMBOL hal_mac_is_txq_valid  SYMBOL hal_mac_set_txq_invalid
    SYMBOL hal_mac_tx_set_cca  SYMBOL hal_mac_txq_disable
    SYMBOL lmacDiscardFrameExchangeSe~E31C
$20 L:
  A1 32 ENTRY,  A10 3 MOVI,  A8 $4 L L32R,  A8 CALLX8,
  A10 16 MOVI,  A2 A2 0 8 EXTUI,  A8 $8 L L32R,  A8 CALLX8,
  A10 A2 MOV.N,  A8 $C L L32R,  A8 CALLX8,
  A5 A10 MOV.N,  A10 A2 MOV.N,  A8 $10 L L32R,  A8 CALLX8,
  A10 0 MOVI.N,  A8 $14 L L32R,  A8 CALLX8,
  A3 A2 3 SLLI,  A4 $0 L L32R,  A5 $87 L BEQZ.N,
  A10 A2 MOV.N,  A8 $18 L L32R,  A8 CALLX8,
  A3 A3 A2 ADD.N,  A3 A3 A4 ADDX4,  A8 6 MOVI,  A8 A3 18 S8I,  MEMW,  A3 A3 0 L32I,
  A3 $A0 L BEQZ.N,
  A2 A2 A2 ADDX8,  A10 A2 A4 ADDX4,  MEMW,  A8 $1C L L32R,  A8 CALLX8,
  $A0 L J,
$87 L:
  A3 A3 A2 ADD.N,  A3 A3 A4 ADDX4,  A2 A3 0 L32I.N,  A2 $A0 L BEQZ.N,
  A3 1 MOVI.N,  A3 A3 17 SLLI,  A4 A2 44 L32I.N,  A2 A4 0 L32I,  A2 A2 A3 OR,
  A2 A4 0 S32I,
$A0 L:
  MEMW,  RETW.N,
$A5 L:  END-CODE

CODE lmacProcessTxTimeout   \ .text.lmacProcessTxTimeout $C
  $0 3 LITERALS
    SYMBOL hal_mac_get_txq_state  SYMBOL lmacDisableTransmit
    SYMBOL hal_mac_clr_txq_state
$C L:
  A1 32 ENTRY,  A10 1 MOVI,  A8 $0 L L32R,  A8 CALLX8,
  A3 A10 A10 OR,  A10 $54 L BEQZ.N,
  A4 1 MOVI,
$20 L:
  A10 A3 NEG,  A10 A10 A3 AND,  A10 A10 NSAU,  A10 A10 NEG,  A2 A10 31 ADDI,
  A10 A2 A2 OR,  A8 $4 L L32R,  A8 CALLX8,
  A2 SSL,  A2 A4 SLL,  A11 A2 MOV.N,  A10 1 MOVI,  A8 $8 L L32R,  A8 CALLX8,
  A8 -1 MOVI.N,  A2 A8 A2 XOR,  A3 A3 A2 AND,  A3 $20 L BNEZ,
$54 L:
  RETW.N,
$56 L:  END-CODE

CODE lmacStopTransmit   \ .text.lmacStopTransmit $2C
  $0 11 LITERALS
    SYMBOL bss.our_instances  SYMBOL pTxRx  SYMBOL hal_mac_tx_set_cca
    SYMBOL ets_delay_us  SYMBOL hal_mac_is_txq_valid
    SYMBOL hal_mac_set_txq_invalid  SYMBOL hal_mac_tx_set_cca
    SYMBOL hal_mac_txq_disable  SYMBOL ppDirectRecycleAmpdu
    SYMBOL esf_buf_recycle  SYMBOL ppDirectRecycleAmpdu
$2C L:
  A1 32 ENTRY,  A10 3 MOVI,  A8 $8 L L32R,  A8 CALLX8,
  A10 16 MOVI,  A2 A2 0 8 EXTUI,  A8 $C L L32R,  A8 CALLX8,
  A10 A2 MOV.N,  A8 $10 L L32R,  A8 CALLX8,
  A6 A10 MOV.N,  A10 A2 MOV.N,  A8 $14 L L32R,  A8 CALLX8,
  A10 0 MOVI.N,  A8 $18 L L32R,  A8 CALLX8,
  A4 $0 L L32R,  A5 A2 A2 ADDX8,  A5 A5 A4 ADDX4,  A3 A2 3 SLLI,  A5 A5 0 L32I,
  A6 $C9 L BEQZ,
  A10 A2 MOV.N,  A8 $1C L L32R,  A8 CALLX8,
  A5 $100 L BEQZ,
  A8 A5 44 L32I.N,  A10 A5 MOV.N,  A9 A8 0 L32I.N,  A9 22 $A4 L BBSI,
  A9 A5 26 L8UI,  A9 5 $AD L BNEI,
  A8 A8 16 L32I.N,  A8 A8 20 4 EXTUI,  A8 A8 1 ADDI.N,  A8 A8 A8 ADDX2,
  A5 $4 L L32R,  A8 A8 4 SLLI,  A5 A5 0 L32I.N,  A5 A5 A8 ADD.N,  A10 A5 4 L32I,
  A10 $B3 L BEQZ,
$A4 L:
  A8 $20 L L32R,  A8 CALLX8,
  $B3 L J,
$AD L:
  A8 $24 L L32R,  A8 CALLX8,
$B3 L:
  A3 A3 A2 ADD.N,  A3 A3 A4 ADDX4,  A2 0 MOVI.N,  A2 A3 0 S32I.N,  A2 A3 18 S8I,
  MEMW,  MEMW,  $100 L J,
  1 ZEROS,
$C9 L:
  A5 $100 L BEQZ.N,
  A3 A5 44 L32I.N,  A4 1 MOVI.N,  A4 A4 17 SLLI,  A2 A3 0 L32I.N,  A2 A2 A4 OR,
  A2 A3 0 S32I.N,  A2 A5 26 L8UI,  A2 5 $100 L BNEI,
  A8 A3 16 L32I.N,  A8 A8 20 4 EXTUI,  A8 A8 1 ADDI.N,  A8 A8 A8 ADDX2,
  A2 $4 L L32R,  A8 A8 4 SLLI,  A2 A2 0 L32I.N,  A2 A2 A8 ADD.N,  A10 A2 4 L32I.N,
  A10 $100 L BEQZ.N,
  MEMW,  A8 $28 L L32R,  A8 CALLX8,
$100 L:
  RETW.N,
$102 L:  END-CODE

CODE dbg_lmac_get_acs   \ .text.dbg_lmac_get_acs $4
  $0 1 LITERALS
    SYMBOL bss.our_instances
$4 L:
  A1 32 ENTRY,  A2 $0 L L32R,  RETW.N,
$C L:  END-CODE

CODE lmac_stop_hw_txq   \ .text.lmac_stop_hw_txq $18
  $0 6 LITERALS
    SYMBOL rodata_wlog_info.61  SYMBOL bss.our_instances
    SYMBOL rodata_wlog_warning.62  SYMBOL wifi_log  SYMBOL wifi_log
    SYMBOL lmacStopTransmit
$18 L:
  A1 32 ENTRY,  A13 $0 L L32R,  A12 3 MOVI,  A11 1 MOVI.N,  A10 6 MOVI,
  A8 $C L L32R,  A8 CALLX8,
  A3 $4 L L32R,  A2 0 MOVI.N,
$31 L:
  A15 A3 18 L8UI,  A15 $50 L BEQZ.N,
  A10 6 MOVI.N,  A14 A2 MOV.N,  A13 $8 L L32R,  A12 2 MOVI.N,  A11 64 MOVI,
  A8 $10 L L32R,  A8 CALLX8,
  A10 A2 MOV.N,  A8 $14 L L32R,  A8 CALLX8,
$50 L:
  A2 A2 1 ADDI.N,  A3 A3 36 ADDI,  A2 5 $31 L BNEI,
  A2 0 MOVI.N,  RETW.N,
$5C L:  END-CODE

CODE is_lmac_idle   \ .wifi0iram.63 $4
  $0 1 LITERALS
    SYMBOL bss.our_instances
$4 L:
  A1 32 ENTRY,  A9 $0 L L32R,  A8 5 MOVI,  A8 $18 L LOOP,
  A2 A9 18 L8UI,  A2 $1D L BNEZ.N,
  A9 A9 36 ADDI,
$18 L:
  A2 1 MOVI.N,  $1F L J,
$1D L:
  A2 0 MOVI.N,
$1F L:
  RETW.N,
$21 L:  END-CODE

CODE esp_wifi_set_11b_tx_plcp   \ .text.esp_wifi_set_11b_tx_plcp $4
  $0 1 LITERALS
    SYMBOL lmacConfMib
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A3 A8 45 S8I,  A2 A8 46 S8I,  MEMW,  MEMW,  RETW.N,
$18 L:  END-CODE

LINK-SYMBOLS   \ the cells of other packages that wait for these names
END-PACKAGE  PREVIOUS   \ not in the search order: ALSO name gives its words (README.md)
