\ phy_chip_v7.fs - package PHY-PHY-CHIP-V7: phy_chip_v7.o of libphy.a (ESP32)
\
\ Made from phy_chip_v7.S by s2fs.py, not by hand. Each function of C is a
\ CODE word: its bytes are those of the compiler, at the same offsets of
\ its section (L gives the address of an offset); its literals are at the
\ start of the word, after a jump. The data come first, then the
\ functions, each after those that it calls. The global symbols are
\ PUBLIC, the static ones PRIVATE. lib/esp32/clink.fs explains the words.
\
\ 153 functions (149 global), 50 data objects.
\ A function runs as C code with the windowed calls of the Xtensa: call
\ it with CALL-C ( x1 .. xn n xt -- x ), never by its name alone.
\
\ Names of other packages: a literal waits until its package comes
\ (LINK-SYMBOLS); a name outside the libraries until EXTERN gives it.
\ Uses:
\   PHY-PHY-CHIP-V7-ANA  bb_bss_cbw40_ana btpwr_atten_offset
\                        btpwr_tsen_init btpwr_tsen_old chan_to_freq
\                        chip_v7_set_chan chip_v7_set_chan_ana
\                        chip_v7_set_chan_nomac chip_v7_set_chan_offset
\                        force_txrxoff get_rfrx_dcap_bt phy_bt_ifs_set
\                        phy_bt_power_track_en phy_dis_hw_set_freq
\                        phy_en_hw_set_freq phy_freq_correct_opt
\                        phy_freq_wifi_only phy_i2c_init
\                        ram_chip_i2c_readReg ram_chip_i2c_writeReg
\                        ram_pbus_debugmode ram_pbus_force_mode
\                        ram_pbus_force_test ram_pbus_xpd_tx_on
\                        ram_restart_cal ram_rfpll_reset
\                        ram_rfpll_set_freq ram_wait_rfpll_cal_end
\                        rf_init set_chan_freq_sw_start tsens_code_read
\                        tsens_read_init write_freq_mem_all
\   PHY-PHY-CHIP-V7-CAL  bt_tx_pwctrl_init correct_rf_ana_gain_new
\                        force_tx_gain_en force_txrx_off
\                        get_phy_target_power pbus_rx_dco_cal_1step
\                        phy_chan_gain_table phy_in_vdd33_offset
\                        phy_set_bbfreq_init phy_tx_pwr_error
\                        ram_dc_iq_est ram_en_pwdet ram_get_fm_sar_dout
\                        ram_get_rf_gain_qdb ram_index_to_txbbgain
\                        ram_iq_est_disable ram_iq_est_enable
\                        ram_meas_tone_pwr_db ram_pbus_rx_dco_cal
\                        ram_phy_get_vdd33 ram_read_sar_dout
\                        ram_rfcal_pwrctrl ram_rfcal_txcap
\                        ram_set_chan_cal_interp ram_set_txcap_reg
\                        ram_tx_pwctrl_bg_init ram_txbbgain_to_index
\                        ram_txcal_work_mode ram_txdc_cal_v70
\                        ram_txiq_get_mis_pwr rc_cal rfcal_txiq
\                        set_chan_dig_gain tx_cap_init tx_gain_table_set
\                        tx_pwctrl_init txpwr_offset wifi_txband
\                        wifi_txband_ht40 write_txrate_power_offset
\ Outside the libraries (../README.md):
\   __divdi3 esp_dport_access_reg_read ets_delay_us memcpy
\   phy_enter_critical phy_exit_critical phy_get_romfuncs phy_printf
\   rtc_get_xtal sprintf
\
\ Needs XTENSA (asm/xtensa.fs), ASM (lib/esp32/asm.fs) and CLINK
\ (lib/esp32/clink.fs) of ucf; the ESP32.

PACKAGE PHY-PHY-CHIP-V7  ALSO ASM  ALSO CLINK   \ lib/package.fs
EXPORTS   \ the PUBLIC words are symbols for the other packages

\ ==== Data =============================================================

\ ---- COMMON (.comm): data of C without a start value
196 DATA: chip7_sleep_params
94 DATA: chip7_phy_init_ctrl
1 DATA: set_most_tpw
16 DATA: phy_rxbb_dc
136 DATA: phy_rxrf_dc
40 DATA: phy_version_str
2 DATA: pwrdet_offset
4 DATA: adc_ana_conf_org
1 DATA: phy_dac_org
1 DATA: phy_dcur_org
15 DATA: chip7_phy_api_ctrl

PRIVATE
CREATE rodata   \ .rodata, 168 bytes
  $FF C,  $09 C,  $07 C,  $00 C,  $FF C,  $13 C,  $17 C,  $00 C,  $00 C,  $00 C,
  $F5 C,  $00 C,  $00 C,  $00 C,  $F6 C,  $00 C,
  $FF C,  $01 C,  $04 C,  $00 C,  $FF C,  $01 C,  $14 C,  $00 C,  $FF C,  $01 C,
  $18 C,  $00 C,
  $FF C,  $03 C,  $04 C,  $00 C,  $FF C,  $F9 C,  $14 C,  $00 C,  $00 C,  $00 C,
  $F0 C,  $00 C,  $00 C,  $00 C,  $F1 C,  $00 C,  $00 C,  $00 C,  $F2 C,  $00 C,
  $FF C,  $09 C,  $07 C,  $00 C,  $FF C,  $17 C,  $17 C,  $00 C,  $00 C,  $00 C,
  $F5 C,  $00 C,  $00 C,  $00 C,  $F6 C,  $00 C,
  $FF C,  $FF C,  $44 C,  $00 C,  $00 C,  $00 C,  $F3 C,  $00 C,
  $FF C,  $01 C,  $44 C,  $00 C,  $FF C,  $01 C,  $54 C,  $00 C,
  $FF C,  $03 C,  $04 C,  $00 C,  $FF C,  $FD C,  $14 C,  $00 C,  $00 C,  $00 C,
  $F0 C,  $00 C,  $00 C,  $00 C,  $F1 C,  $00 C,  $00 C,  $00 C,  $F2 C,  $00 C,
  $00 C,  $00 C,  $01 C,  $00 C,  $05 C,  $00 C,  $0D C,  $00 C,  $1D C,  $00 C,
  $00 C,  $00 C,
  $0D C,  $0C C,  $04 C,  $00 C,  $00 C,  $00 C,  $00 C,  $00 C,
  $00 C,  $00 C,  $20 C,  $00 C,  $28 C,  $00 C,  $2C C,  $00 C,  $2E C,  $00 C,
  $00 C,  $00 C,
  $00 C,  $01 C,  $00 C,  $01 C,
  $05 C,  $09 C,  $06 C,  $05 C,  $03 C,  $06 C,  $05 C,  $04 C,  $06 C,  $04 C,
  $05 C,  $00 C,
  $05 C,  $09 C,  $06 C,  $05 C,  $03 C,  $06 C,  $09 C,  $0C C,  $00 C,  $00 C,
  $00 C,  $00 C,
  $00 C,  $01 C,  $00 C,  $01 C,  $00 C,  $01 C,  $00 C,  $01 C,
CREATE rodata.ram_set_pbus_mem   \ .rodata.ram_set_pbus_mem, 44 bytes
  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,   \ 11 jumps into ram_set_pbus_mem
CREATE rodata.str1.4   \ .rodata.str1.4, 1489 bytes
  ," i2c_master_reset %d"  10 C,  0 C,  0 C,  0 C,  0 C,
  ," index: %d value: 0x%x 0x%x 0x%x %d %d %d %d"  10 C,  0 C,  0 C,  0 C,  0 C,
  ," max_gain: %d"  10 C,  0 C,  0 C,  0 C,
  ," total_pwr=%ld, min=%ld, max=%ld, rftx=0x%x, att=%d, dc_i=%d,"  ,"  dc_q=%d"
  10 C,  0 C,  0 C,  0 C,
  ," rxiq: rftx=0x%x, rfrx=x%x, att=%d, bb=0x%x, %d, %d"  10 C,  0 C,
  ," 14:30:06"  0 C,  0 C,  0 C,  0 C,
  ," Oct 28 2025"  0 C,
  ," a3a4459"  0 C,
  ," %d,%s,%s,%s"  0 C,
  ," phy_version: %d, %s, %s, %s"  10 C,  0 C,  0 C,  0 C,  0 C,
  ," %d, %d, "  0 C,  0 C,  0 C,  0 C,
  ," cal_rf_ana_gain, rf_gain=0x%x, ana_gain=0x%x"  10 C,  0 C,  0 C,  0 C,
  ," TX_POWER_BACKOFF, "  0 C,  0 C,
  ," %d, "  0 C,  0 C,  0 C,  0 C,
  10 C,  0 C,  0 C,  0 C,
  ," TX_PWRCTRL_ATTEN, "  0 C,  0 C,
  ," BT_PA_GAIN, "  0 C,  0 C,  0 C,  0 C,
  ," 0x%x, "  0 C,  0 C,
  ," BT_DIG_ATTEN, "  0 C,  0 C,
  ," BT_TX_BB, 0x%x"  10 C,  0 C,
  ," bt_txpwr_atten, %d"  10 C,  0 C,
  ," total_power=%lld, sig_power=%lld, dc_power=%lld, noise_power"  ," =%lld"  10 C,
  0 C,  0 C,
  ," step=%d, %d"  10 C,  0 C,  0 C,  0 C,  0 C,
  ," %d-%d,"  0 C,  0 C,
  ," rfrx:0x%x;"  0 C,  0 C,
  ," rftx:0x%x,0x%x;"  0 C,
  ," bb:0x%x,0x%x;"  0 C,  0 C,  0 C,
  ," dco:%d,%d,%d,%d"  10 C,  0 C,  0 C,  0 C,  0 C,
  ," RTC 0x%x: 0x%x"  10 C,  0 C,
  ," SAR 0x%x: 0x%x"  10 C,  0 C,
  ," APB 0x%x: 0x%x"  10 C,  0 C,
  ," DPORT 0x%x: 0x%x"  10 C,  0 C,  0 C,  0 C,
  ," fe 0x%x: 0x%x"  10 C,  0 C,  0 C,
  ," fe2 0x%x: 0x%x"  10 C,  0 C,
  ," agc 0x%x: 0x%x"  10 C,  0 C,
  ," bb 0x%x: 0x%x"  10 C,  0 C,  0 C,
  ," bbtx 0x%x: 0x%x"  10 C,  0 C,  0 C,  0 C,  0 C,
  ," brx 0x%x: 0x%x"  10 C,  0 C,
  ," nrx 0x%x: 0x%x"  10 C,  0 C,
  ," bt_reg 0x%x: 0x%x"  10 C,  0 C,  0 C,
  ," i2c_mst 0x%x: 0x%x"  10 C,  0 C,
  ," i2c_bias %02d: 0x%x"  10 C,  0 C,  0 C,  0 C,  0 C,
  ," i2c_bbtop %02d: 0x%x"  10 C,  0 C,  0 C,  0 C,
  ," i2c_bbpll %02d: 0x%x"  10 C,  0 C,  0 C,  0 C,
  ," i2c_txrf %02d: 0x%x"  10 C,  0 C,  0 C,  0 C,  0 C,
  ," i2c_rfrx %02d: 0x%x"  10 C,  0 C,  0 C,  0 C,  0 C,
  ," i2c_rfpll %02d: 0x%x"  10 C,  0 C,  0 C,  0 C,
  ," i2c_rfpll_sdm %02d: 0x%x"  10 C,  0 C,  0 C,  0 C,
  ," i2c_ckgen %02d: 0x%x"  10 C,  0 C,  0 C,  0 C,
  ," i2c_xtal %02d: 0x%x"  10 C,  0 C,  0 C,  0 C,  0 C,
  ," i2c_plla %02d: 0x%x"  10 C,  0 C,  0 C,  0 C,  0 C,
  ," CHIP_ID: 0x%x, 0x%x"  10 C,  10 C,  0 C,  0 C,  0 C,
  ," param_flag: 0x%x"  10 C,  0 C,  0 C,  0 C,
  ," vdd33=%d, temp_code=%d, offset=%d"  10 C,  0 C,  0 C,
  ," rc_dout, %d; wifi: %d, %d, %d, %d, bt: %d, %d"  10 C,  0 C,  0 C,
  ," RX_NOISEFLOOR, %d"  10 C,  0 C,  0 C,
  ," TXCAP_TMX2G_CCT_LOAD, "  0 C,  0 C,
  ," TXCAP_PA2G_CCT_STG1, "  0 C,  0 C,  0 C,
  ," TXCAP_PA2G_CCT_STG2, "  0 C,  0 C,  0 C,
  ," TXIQ,"  0 C,  0 C,  0 C,
  ," %d, %d; "  0 C,  0 C,  0 C,  0 C,
  ," TXDC, "  0 C,  0 C,
  ," %d, %d, %d, %d; "  0 C,  0 C,  0 C,  0 C,
  ," delta_bt_atten, "  0 C,  0 C,  0 C,  0 C,
  ," BT_TXIQ,"  0 C,  0 C,  0 C,  0 C,
  ," BT_TXDC, "  0 C,  0 C,  0 C,
  ," RXIQ, "  0 C,  0 C,
  ," RXDC_RFRX_BT, "  0 C,  0 C,
  ," RXDC_RFRX_WIFI, "  0 C,  0 C,  0 C,  0 C,
  ," RXDC_RXBB_WIFI, "  0 C,  0 C,  0 C,  0 C,
  ," TX_VDD33=%d"  10 C,  0 C,
CREATE CSWTCH$369   \ .rodata.CSWTCH$369, 11 bytes
  $F2 C,  $F2 C,  $F2 C,  $F2 C,  $F2 C,  $F2 C,  $F3 C,  $F2 C,  $F2 C,  $F0 C,
  $F1 C,
CREATE CSWTCH$368   \ .rodata.CSWTCH$368, 22 bytes
  $00 C,  $00 C,  $00 C,  $00 C,  $00 C,  $00 C,  $00 C,  $00 C,  $03 C,  $00 C,
  $03 C,  $00 C,  $03 C,  $00 C,  $07 C,  $00 C,  $07 C,  $00 C,  $0F C,  $00 C,
  $0F C,  $00 C,
CREATE CSWTCH$367   \ .rodata.CSWTCH$367, 22 bytes
  $03 C,  $00 C,  $06 C,  $00 C,  $09 C,  $00 C,  $0C C,  $00 C,  $04 C,  $00 C,
  $07 C,  $00 C,  $0A C,  $00 C,  $07 C,  $00 C,  $0A C,  $00 C,  $07 C,  $00 C,
  $0A C,  $00 C,
CREATE CSWTCH$356   \ .rodata.CSWTCH$356, 6 bytes
  $00 C,  $00 C,  $20 C,  $00 C,  $30 C,  $00 C,
PUBLIC
4 DATA: g_phyFuns   \ .bss.g_phyFuns
1 DATA: bt_dig_gain_force   \ .bss.bt_dig_gain_force
1 DATA: init_wifi_disable   \ .bss.init_wifi_disable
1 DATA: rfrx_dcap_enable   \ .bss.rfrx_dcap_enable
2 DATA: rfrx_vga_lna_dcap   \ .bss.rfrx_vga_lna_dcap
1 DATA: corr_cca_thr   \ .bss.corr_cca_thr
1 DATA: bt_txpwr_backoff   \ .bss.bt_txpwr_backoff
1 DATA: lr_enable   \ .bss.lr_enable
1 DATA: chan14_mic_en   \ .bss.chan14_mic_en
1 DATA: re_entry   \ .bss.re_entry
1 DATA: force_hw_set_freq_flag   \ .bss.force_hw_set_freq_flag
1 DATA: phy_init_flag   \ .bss.phy_init_flag
1 DATA: phy_set_most_tpw_index   \ .bss.phy_set_most_tpw_index
2 DATA: target_power_backoff_qdb   \ .bss.target_power_backoff_qdb
1 DATA: phy_set_most_tpw_flag   \ .bss.phy_set_most_tpw_flag
1 DATA: phy_set_most_tpw_disbg   \ .bss.phy_set_most_tpw_disbg
1 DATA: chan14_mic_flag   \ .bss.chan14_mic_flag
1 DATA: chan14_mic_most_power   \ .data.chan14_mic_most_power
  $30  1 chan14_mic_most_power INIT
1 DATA: phy_in_most_power_bk   \ .data.phy_in_most_power_bk
  $4E  1 phy_in_most_power_bk INIT
1 DATA: phy_in_most_power   \ .data.phy_in_most_power
  $4E  1 phy_in_most_power INIT
2 DATA: phy_meas_noise_floor   \ .bss.phy_meas_noise_floor
1 DATA: sw_scan_mode   \ .bss.sw_scan_mode
1 DATA: bt_mode_wifibb   \ .bss.bt_mode_wifibb
2 DATA: bt_tx_pa_gain   \ .data.bt_tx_pa_gain
  $7A  $00  2 bt_tx_pa_gain INIT
4 DATA: tx_rf_ana_gain   \ .data.tx_rf_ana_gain
  $5F  $00  $20  $01  4 tx_rf_ana_gain INIT
1 DATA: rxmax_ext_level   \ .bss.rxmax_ext_level
1 DATA: noise_timeout_flag   \ .bss.noise_timeout_flag
6 DATA: noise_array   \ .data.noise_array
  $00  $FD  $00  $FD  $00  $FD  6 noise_array INIT
1 DATA: wifi_bbgain_max   \ .data.wifi_bbgain_max
  $16  1 wifi_bbgain_max INIT
PRIVATE
9 DATA: bt_rx_gain_swp   \ .data.bt_rx_gain_swp
  $00  $04  $30  $34  $38  $40  $44  $50  $54  9 bt_rx_gain_swp INIT
1 DATA: rfbb_gain_swp   \ .data.rfbb_gain_swp
  $5C  1 rfbb_gain_swp INIT
12 DATA: wifi_rx_gain_swp   \ .data.wifi_rx_gain_swp
  $00  $04  $30  $34  $38  $40  $44  $48  $50  $54  $58  $5C  12 wifi_rx_gain_swp INIT

\ ==== Code =============================================================

PUBLIC
CODE ram_disable_agc   \ .iram1 $8
  $0 2 LITERALS
    $3FF5C030 T,  SYMBOL esp_dport_access_reg_read
$8 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A10 A2 MOV.N,  A8 $4 L L32R,  A8 CALLX8,
  A8 $100 MOVI,  A10 A10 A8 OR,  MEMW,  A10 A2 0 S32I.N,  RETW.N,
$23 L:  END-CODE

CODE ram_enable_agc   \ .iram1 $2C
  $24 2 LITERALS
    $3FF5C030 T,  SYMBOL esp_dport_access_reg_read
$2C L:
  A1 32 ENTRY,  A2 $24 L L32R,  A10 A2 MOV.N,  A8 $28 L L32R,  A8 CALLX8,
  A8 -257 MOVI,  A10 A10 A8 AND,  MEMW,  A10 A2 0 S32I.N,  RETW.N,
$47 L:  END-CODE

CODE ram_write_gain_mem   \ .iram1 $6C
  $48 9 LITERALS
    $3FF45038 T,  $3FF45034 T,  $FFFC0000 T,  $100F0 T,  $1000F T,  $FFFCFFFF T,
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read
$6C L:
  A1 32 ENTRY,  A5 $4C L L32R,  A4 A4 0 8 EXTUI,  A10 A5 MOV.N,  A4 A4 8 SLLI,
  A6 $48 L L32R,  MEMW,  A3 A6 0 S32I.N,  A8 $60 L L32R,  A8 CALLX8,
  A9 $54 L L32R,  A9 A4 A9 ADD.N,  A3 $50 L L32R,  A8 A10 A3 AND,  A9 A8 A9 OR,
  A10 A5 MOV.N,  MEMW,  A9 A5 0 S32I.N,  MEMW,  A2 A6 0 S32I.N,  A8 $64 L L32R,
  A8 CALLX8,
  A3 A10 A3 AND,  A10 A5 MOV.N,  A8 $58 L L32R,  A4 A4 A8 ADD.N,  A8 A3 A4 OR,
  MEMW,  A8 A5 0 S32I.N,  A8 $68 L L32R,  A8 CALLX8,
  A3 $5C L L32R,  A10 A10 A3 AND,  MEMW,  A10 A5 0 S32I.N,  RETW.N,
$CD L:  END-CODE

CODE ram_set_txclk_en   \ .iram1 $E0
  $D0 4 LITERALS
    $30000 T,  $3FF460A0 T,  $FFFCFFFF T,  SYMBOL esp_dport_access_reg_read
$E0 L:
  A1 32 ENTRY,  A3 $D4 L L32R,  A2 A2 0 8 EXTUI,  A10 A3 MOV.N,  A8 $DC L L32R,
  A8 CALLX8,
  A9 0 MOVI.N,  A8 $D8 L L32R,  A8 A10 A8 AND,  A10 $D0 L L32R,  A10 A9 A2 MOVEQZ,
  A10 A10 A8 OR,  MEMW,  A10 A3 0 S32I.N,  RETW.N,
$109 L:  END-CODE

CODE ram_set_rxclk_en   \ .iram1 $11C
  $10C 4 LITERALS
    $C000 T,  $3FF460A0 T,  $FFFF3FFF T,  SYMBOL esp_dport_access_reg_read
$11C L:
  A1 32 ENTRY,  A3 $110 L L32R,  A2 A2 0 8 EXTUI,  A10 A3 MOV.N,  A8 $118 L L32R,
  A8 CALLX8,
  A9 0 MOVI.N,  A8 $114 L L32R,  A8 A10 A8 AND,  A10 $10C L L32R,  A10 A9 A2 MOVEQZ,
  A10 A10 A8 OR,  MEMW,  A10 A3 0 S32I.N,  RETW.N,
$145 L:  END-CODE

CODE disable_wifi_agc   \ .iram1 $174
  $148 11 LITERALS
    $3FF5C01C T,  $FF00FFFF T,  $7F0000 T,  $3FF5C038 T,  $4000000 T,
    $3FF5C030 T,  $3FF5C080 T,  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read
$174 L:
  A1 32 ENTRY,  A3 $148 L L32R,  A10 A3 MOV.N,  A8 $164 L L32R,  A8 CALLX8,
  A8 $14C L L32R,  A10 A10 A8 AND,  A8 $150 L L32R,  A8 A10 A8 OR,  A2 $154 L L32R,
  A10 A2 A2 OR,  MEMW,  A8 A3 0 S32I,  A8 $168 L L32R,  A8 CALLX8,
  A8 $158 L L32R,  A8 A10 A8 OR,  A3 $15C L L32R,  A10 A3 A3 OR,  MEMW,
  A8 A2 0 S32I,  A8 $16C L L32R,  A8 CALLX8,
  A8 -49 MOVI,  A8 A10 A8 AND,  A2 $160 L L32R,  A10 A2 MOV.N,  MEMW,
  A8 A3 0 S32I.N,  A8 $170 L L32R,  A8 CALLX8,
  A8 1 MOVI.N,  A10 A10 A8 OR,  MEMW,  A10 A2 0 S32I.N,  RETW.N,
$1DA L:  END-CODE

CODE enable_wifi_agc   \ .iram1 $208
  $1DC 11 LITERALS
    $3FF5C080 T,  $3FF5C030 T,  $3FF5C01C T,  $FF00FFFF T,  $C0000 T,
    $3FF5C038 T,  $4000000 T,  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read
$208 L:
  A1 32 ENTRY,  A4 $1DC L L32R,  A2 -2 MOVI.N,  A10 A4 A4 OR,  A8 $1F8 L L32R,
  A8 CALLX8,
  A2 A10 A2 AND,  A3 $1E0 L L32R,  A10 A3 MOV.N,  MEMW,  A2 A4 0 S32I,
  A8 $1FC L L32R,  A8 CALLX8,
  A8 -49 MOVI,  A10 A10 A8 AND,  A8 16 MOVI.N,  A2 $1E4 L L32R,  A8 A10 A8 OR,
  A10 A2 MOV.N,  MEMW,  A8 A3 0 S32I.N,  A8 $200 L L32R,  A8 CALLX8,
  A8 $1E8 L L32R,  A10 A10 A8 AND,  A8 $1EC L L32R,  A8 A10 A8 OR,  A3 $1F0 L L32R,
  A10 A3 MOV.N,  MEMW,  A8 A2 0 S32I.N,  A8 $204 L L32R,  A8 CALLX8,
  A8 $1F4 L L32R,  A10 A10 A8 OR,  MEMW,  A10 A3 0 S32I.N,  RETW.N,
$271 L:  END-CODE

CODE wr_bt_tx_atten   \ .iram1 $280
  $274 3 LITERALS
    SYMBOL bt_dig_gain_force  $3FF4600C T,  $3FF46010 T,
$280 L:
  A1 48 ENTRY,  A10 A1 A1 OR,  A9 8 MOVI,  A9 $29C L LOOP,
  A8 A2 0 L8UI,  A2 A2 1 ADDI.N,  A8 A8 NEG,  A8 A10 0 S8I,  MEMW,
  A10 A10 1 ADDI.N,
$29C L:
  NOP.N,  NOP.N,  NOP.N,  A2 $274 L L32R,  A2 A2 0 L8UI,  A2 $2C6 L BNEZ.N,
  A8 A1 0 L32I.N,  A2 $278 L L32R,  MEMW,  A8 A2 0 S32I.N,  NOP.N,  NOP.N,  NOP.N,
  NOP.N,  A8 A1 4 L32I.N,  A2 $27C L L32R,  MEMW,  A8 A2 0 S32I.N,
$2C6 L:
  RETW.N,
$2C8 L:  END-CODE

CODE wr_bt_tx_gain_mem   \ .iram1 $2E0
  $2C8 6 LITERALS
    SYMBOL chip7_sleep_params  $3FF46000 T,  $3FC0000 T,
    $8 SYMBOL+ chip7_sleep_params  SYMBOL g_phyFuns
    SYMBOL esp_dport_access_reg_read
$2E0 L:
  A1 64 ENTRY,  A10 $2CC L L32R,  A2 A2 0 8 EXTUI,  A8 $2DC L L32R,  A8 CALLX8,
  A3 A3 0 8 EXTUI,  A4 255 MOVI,  A8 $2D0 L L32R,  A10 A10 A8 AND,
  A8 A10 18 8 EXTUI,  A2 A1 16 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,
  A9 $2C8 L L32R,  A3 A4 $31C L BEQ,
  A12 A3 0 6 EXTUI,  A11 A3 6 SRLI,  A5 A9 144 L16UI,  $325 L J,
  2 ZEROS,
$31C L:
  A5 A9 144 L16UI,  A12 A5 0 6 EXTUI,  A11 A5 6 8 EXTUI,
$325 L:
  A4 A5 MOV.N,  A5 $33D L BEQZ.N,
  A3 A5 -48 ADDI,  A4 8 MOVI.N,  A5 4 MOVI.N,  A5 A4 A3 MOVEQZ,  A6 2 MOVI.N,
  A4 1 MOVI.N,  A4 A6 A3 MOVEQZ,  $33D L J,
$33D L:
  A4 A4 A9 ADDX2,  A5 A5 76 ADDI,  A4 A4 146 L16UI,  A5 A5 A9 ADDX2,  A7 A1 MOV.N,
  A6 4 MOVI.N,  A6 $35D L LOOP,
  A10 A5 0 L16UI,  A5 A5 2 ADDI.N,  A10 A7 0 S16I,  MEMW,  A7 A7 2 ADDI.N,
$35D L:
  A14 A4 21 SLLI,  A8 A8 8 ADDI.N,  NOP.N,  A3 A1 16 L32I.N,  A12 A12 3 SLLI,
  A3 A3 0 16 EXTUI,  A2 A8 0 8 EXTUI,  A7 A9 MOV.N,  A4 A1 4 L16UI,  A13 A4 26 SLLI,
  A4 A4 6 SRLI,  A5 A1 0 L16UI,  A5 A5 12 SLLI,  A3 A1 20 S32I.N,  NOP.N,  NOP,
  NOP.N,  NOP,  A6 A1 2 L16UI,  A6 A6 3 SLLI,  A5 A5 A6 OR,  A4 A5 A4 OR,
  A3 A1 6 L16UI,  A4 A4 A14 OR,  A3 A3 17 SLLI,  A3 A3 A13 OR,  A11 A3 A11 OR,
  A6 $2D4 L L32R,  A3 A11 A12 OR,  A5 $2D8 L L32R,
$3B0 L:
  A9 255 MOVI,  A8 A1 16 L32I.N,  A11 A1 20 L32I.N,  A8 A9 $3BD L BNE,
  A11 A7 120 L8UI,
$3BD L:
  A11 A11 10 SLLI,  A12 A2 MOV.N,  A11 A11 A3 OR,  A10 A4 MOV.N,  A13 A5 0 L32I.N,
  A7 A7 1 ADDI.N,  A13 A13 52 L32I.N,  A13 CALLX8,
  A8 A2 1 ADDI.N,  A2 A8 0 8 EXTUI,  A6 A7 $3B0 L BNE,
  RETW.N,
$3DA L:  END-CODE

CODE set_tx_gain_table   \ .iram1 $3F4
  $3DC 6 LITERALS
    $3FF46000 T,  $FC03FFFF T,  $1B80000 T,  SYMBOL g_phyFuns
    SYMBOL chip7_sleep_params  SYMBOL esp_dport_access_reg_read
$3F4 L:
  A1 48 ENTRY,  A5 $3DC L L32R,  A7 A2 0 16 EXTUI,  A10 A5 MOV.N,  A8 $3F0 L L32R,
  A8 CALLX8,
  A3 A3 0 16 EXTUI,  A2 $3E0 L L32R,  A6 A1 MOV.N,  A4 A10 A2 AND,  A10 A3 MOV.N,
  A2 $3E4 L L32R,  A2 A4 A2 OR,  A4 $3E8 L L32R,  MEMW,  A2 A5 0 S32I.N,
  A5 4 MOVI.N,  NOP.N,  NOP.N,  NOP.N,  A2 A4 0 L32I.N,  A2 A2 $10C L32I,
  A2 CALLX8,
  A8 A10 0 16 EXTUI,  A8 A8 3 SLLI,  A2 $3EC L L32R,  A8 A8 12 ADDI.N,
  A8 A2 A8 ADD.N,  A5 $44D L LOOP,
  A9 A8 0 L16UI,  A8 A8 2 ADDI.N,  A9 A6 0 S16I,  MEMW,  A6 A6 2 ADDI.N,
$44D L:
  A11 A3 6 8 EXTUI,  A3 A3 0 6 EXTUI,  NOP.N,  A5 A1 4 L16UI,  A6 A3 3 SLLI,
  A12 A5 26 SLLI,  A7 A7 10 SLLI,  A8 A5 6 SRLI,  A3 A1 6 L16UI,  A9 A2 MOV.N,
  A3 A3 17 SLLI,  A3 A3 A12 OR,  A7 A3 A7 OR,  A3 A7 A11 OR,  A5 A1 0 L16UI,
  A3 A3 A6 OR,  A5 A5 12 SLLI,  A2 1 MOVI.N,  A6 A1 2 L16UI,  A6 A6 3 SLLI,
  A5 A5 A6 OR,  A5 A5 A8 OR,  A10 4 $491 L BGEUI,
  A2 0 MOVI.N,
$491 L:
  A2 A2 24 ADDI,  A2 A2 A9 ADDX2,  A6 A2 4 L16UI,  A2 110 MOVI,  A6 A6 21 SLLI,
  A5 A5 A6 OR,  A6 118 MOVI,
$4A6 L:
  A8 A4 0 L32I.N,  A12 A2 MOV.N,  A11 A3 MOV.N,  A2 A2 1 ADDI.N,  A10 A5 MOV.N,
  A8 A8 52 L32I.N,  A8 CALLX8,
  A2 A6 $4A6 L BNE,
  RETW.N,
$4BA L:  END-CODE

CODE set_most_pwr_reg   \ .iram1 $4D8
  $4BC 7 LITERALS
    SYMBOL phy_in_vdd33_offset  SYMBOL phy_in_most_power
    SYMBOL phy_set_most_tpw_disbg  SYMBOL set_most_tpw  SYMBOL tx_rf_ana_gain
    SYMBOL chip7_sleep_params  SYMBOL get_phy_target_power
$4D8 L:
  A1 48 ENTRY,  A2 $4BC L L32R,  A10 A1 MOV.N,  A11 A2 0 L8UI,  A11 A11 7 SEXT,
  A8 $4D4 L L32R,  A8 CALLX8,
  A2 $4C0 L L32R,  A8 A2 0 L8UI,  A2 A8 7 SEXT,  A9 A1 5 L8UI,  A2 A9 $4FE L BLT,
  $5BD L J,
$4FE L:
  A8 A9 A8 SUB,  A10 1 MOVI.N,  A2 $4C4 L L32R,  A8 A8 0 8 EXTUI,  A10 A2 0 S8I,
  MEMW,  A2 8 MOVI.N,  A2 A8 $528 L BLTU,
  A9 A9 -8 ADDI,  A11 14 MOVI.N,  NOP.N,  A2 $4C8 L L32R,  A9 A2 0 S8I,  MEMW,
  $668 L J,
  1 ZEROS,
$528 L:
  A2 18 MOVI.N,  A2 A8 $53E L BLTU,
  A2 $4C8 L L32R,  A9 A9 -18 ADDI,  A11 10 MOVI.N,  A9 A2 0 S8I,  MEMW,  $668 L J,
$53E L:
  A2 24 MOVI.N,  A2 A8 $554 L BLTU,
  A2 $4C8 L L32R,  A9 A9 -24 ADDI,  A11 7 MOVI.N,  A9 A2 0 S8I,  MEMW,  $668 L J,
$554 L:
  A2 32 MOVI.N,  A2 A8 $56A L BLTU,
  A2 $4C8 L L32R,  A9 A9 -32 ADDI,  A11 6 MOVI.N,  A9 A2 0 S8I,  MEMW,  $668 L J,
$56A L:
  A2 44 MOVI.N,  A2 A8 $580 L BLTU,
  A2 $4C8 L L32R,  A9 A9 -44 ADDI,  A11 5 MOVI.N,  A9 A2 0 S8I,  MEMW,  $668 L J,
$580 L:
  A2 56 MOVI.N,  A2 A8 $596 L BLTU,
  A2 $4C8 L L32R,  A9 A9 -56 ADDI,  A11 2 MOVI.N,  A9 A2 0 S8I,  MEMW,  $668 L J,
$596 L:
  A2 68 MOVI.N,  A2 A8 $5AC L BLTU,
  A2 $4C8 L L32R,  A9 A9 -68 ADDI,  A11 1 MOVI.N,  A9 A2 0 S8I,  MEMW,  $668 L J,
$5AC L:
  A2 $4C8 L L32R,  A9 A9 -92 ADDI,  A11 0 MOVI.N,  A9 A2 0 S8I,  MEMW,  $668 L J,
$5BD L:
  A10 $4C4 L L32R,  A11 0 MOVI.N,  A8 A1 4 L8UI,  A11 A10 0 S8I,  MEMW,
  A2 A8 $5E4 L BGE,
  A2 5 MOVI.N,  NOP.N,  NOP.N,  NOP.N,  A8 $4C8 L L32R,  A9 A8 0 S8I,  MEMW,
  $651 L J,
  2 ZEROS,
$5E4 L:
  A9 A1 3 L8UI,  A2 A9 $63C L BGE,
  A2 4 MOVI.N,  A9 $4C8 L L32R,  A8 A9 0 S8I,  MEMW,  $651 L J,
$5F8 L:
  A8 $4C8 L L32R,  A2 3 MOVI.N,  A9 A8 0 S8I,  MEMW,  $651 L J,
  2 ZEROS,
$608 L:
  A9 $4C8 L L32R,  A2 2 MOVI.N,  A8 A9 0 S8I,  MEMW,  $651 L J,
  2 ZEROS,
$618 L:
  A8 $4C8 L L32R,  A2 1 MOVI.N,  A9 A8 0 S8I,  MEMW,  $651 L J,
$626 L:
  A9 $4C8 L L32R,  A2 0 MOVI.N,  A8 A9 0 S8I,  MEMW,  $651 L J,
$634 L:
  A11 A8 2 L16UI,  FUNC set_tx_gain_table CALL8,
  RETW.N,
$63C L:
  A8 A1 2 L8UI,  A2 A8 $5F8 L BLT,
  A9 A1 1 L8UI,  A2 A9 $608 L BLT,
  A8 A1 0 L8UI,  A2 A8 $618 L BLT,
  $626 L J,
$651 L:
  A8 $4D0 L L32R,  A10 A8 188 L8UI,  A8 15 MOVI.N,  A10 A10 A8 OR,  A8 $4CC L L32R,
  A10 A8 0 S16I,  MEMW,  $634 L J,
$668 L:
  A8 $4D0 L L32R,  A2 5 MOVI.N,  A10 A8 188 L16UI,  A8 240 MOVI,  A10 A10 A8 AND,
  A10 A11 A10 OR,  A10 A10 0 16 EXTUI,  A8 $4CC L L32R,  A10 A8 0 S16I,  MEMW,
  $634 L J,
$688 L:  END-CODE

CODE set_xpd_sar   \ .iram1 $698
  $688 4 LITERALS
    $C0000 T,  $3FF4880C T,  $FFF3FFFF T,  SYMBOL esp_dport_access_reg_read
$698 L:
  A1 32 ENTRY,  A3 $68C L L32R,  A2 A2 0 8 EXTUI,  A10 A3 MOV.N,  A8 $694 L L32R,
  A8 CALLX8,
  A9 0 MOVI.N,  A8 $690 L L32R,  A8 A10 A8 AND,  A10 $688 L L32R,  A10 A9 A2 MOVEQZ,
  A10 A10 A8 OR,  MEMW,  A10 A3 0 S32I.N,  RETW.N,
$6C1 L:  END-CODE

CODE bb_wdt_rst_enable   \ .iram1 $6D0
  $6C4 3 LITERALS
    $3FF5D040 T,  $7FFFFFFF T,  SYMBOL esp_dport_access_reg_read
$6D0 L:
  A1 32 ENTRY,  A3 $6C4 L L32R,  A2 A2 0 8 EXTUI,  A10 A3 MOV.N,  A8 $6CC L L32R,
  A8 CALLX8,
  A2 A2 31 SLLI,  A8 $6C8 L L32R,  A10 A10 A8 AND,  A2 A10 A2 OR,  MEMW,
  A2 A3 0 S32I.N,  RETW.N,
$6F4 L:  END-CODE

CODE bb_wdt_int_enable   \ .iram1 $704
  $6F4 4 LITERALS
    $3FF5D040 T,  $40000000 T,  $BFFFFFFF T,  SYMBOL esp_dport_access_reg_read
$704 L:
  A1 32 ENTRY,  A3 $6F4 L L32R,  A2 A2 0 8 EXTUI,  A10 A3 MOV.N,  A8 $700 L L32R,
  A8 CALLX8,
  A2 A2 30 SLLI,  A8 $6F8 L L32R,  A2 A2 A8 AND,  A8 $6FC L L32R,  A10 A10 A8 AND,
  A10 A2 A10 OR,  MEMW,  A10 A3 0 S32I.N,  RETW.N,
$72E L:  END-CODE

CODE bb_wdt_timeout_clear   \ .iram1 $73C
  $730 3 LITERALS
    $3FF5D040 T,  $20000000 T,  SYMBOL esp_dport_access_reg_read
$73C L:
  A1 32 ENTRY,  A2 $730 L L32R,  A10 A2 MOV.N,  A8 $738 L L32R,  A8 CALLX8,
  A8 $734 L L32R,  A10 A10 A8 OR,  MEMW,  A10 A2 0 S32I.N,  RETW.N,
$757 L:  END-CODE

CODE bb_wdt_get_status   \ .iram1 $760
  $758 2 LITERALS
    $3FF5D008 T,  SYMBOL esp_dport_access_reg_read
$760 L:
  A1 32 ENTRY,  A10 $758 L L32R,  A8 $75C L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  RETW.N,
$770 L:  END-CODE

CODE bt_tx_gain_cal   \ .iram1 $780
  $770 4 LITERALS
    SYMBOL chip7_sleep_params  SYMBOL btpwr_atten_offset  SYMBOL bt_tx_pa_gain
    SYMBOL correct_rf_ana_gain_new
$780 L:
  A1 48 ENTRY,  A2 $774 L L32R,  A7 -76 MOVI,  A6 160 MOVI,  A5 $770 L L32R,
  A3 A5 MOV.N,  A2 A2 0 L8UI,  A2 A2 7 SEXT,  A4 A5 138 L8UI,  A2 A4 A2 ADD.N,
  A7 A7 A2 SUB,  A7 A7 0 16 EXTUI,  A4 $778 L L32R,  A2 A2 0 8 EXTUI,
$7A8 L:
  A11 A1 MOV.N,  A10 A1 4 ADDI.N,  A13 1 MOVI.N,  A8 A5 144 L16UI,  A12 A1 2 ADDI.N,
  A15 A4 0 L16UI,  A8 A1 2 S16I,  A7 A1 4 S16I,  A15 A1 0 S16I,  MEMW,
  A8 $77C L L32R,  A8 CALLX8,
  A8 A1 4 L8UI,  A8 A2 A8 ADD.N,  A8 A8 A6 SUB,  A8 A8 0 8 EXTUI,  A11 A1 0 L16UI,
  A10 A8 7 SEXT,  A11 A3 120 S8I,  MEMW,  A10 $7ED L BGEZ,
  A8 0 MOVI.N,  A8 A3 129 S8I,  MEMW,  $7FA L J,
$7ED L:
  A9 64 MOVI.N,  A9 A10 $7F4 L BGE,
  A8 64 MOVI.N,
$7F4 L:
  A8 A3 129 S8I,  MEMW,
$7FA L:
  A6 A6 12 ADDI.N,  A7 A7 12 ADDI.N,  A6 A6 0 8 EXTUI,  A7 A7 0 16 EXTUI,
  A3 A3 1 ADDI.N,  A6 $7A8 L BNEZ,
  RETW.N,
$80B L:  END-CODE

PRIVATE
CODE bt_tx_gain_cal_set$part$4   \ .iram1 $818, static
  $80C 3 LITERALS
    $81 SYMBOL+ chip7_sleep_params  SYMBOL force_txrx_off  SYMBOL force_txrx_off
$818 L:
  A1 32 ENTRY,  FUNC bt_tx_gain_cal CALL8,
  A10 1 MOVI.N,  A8 $810 L L32R,  A8 CALLX8,
  A11 255 MOVI,  A10 A11 MOV.N,  FUNC wr_bt_tx_gain_mem CALL8,
  A10 $80C L L32R,  FUNC wr_bt_tx_atten CALL8,
  A10 0 MOVI.N,  A8 $814 L L32R,  A8 CALLX8,
  RETW.N,
$83E L:  END-CODE

PUBLIC
CODE bt_tx_gain_cal_set   \ .iram1 $844
  $840 1 LITERALS
    SYMBOL bt_dig_gain_force
$844 L:
  A1 32 ENTRY,  A8 $840 L L32R,  A8 A8 0 L8UI,  A8 $852 L BNEZ.N,
  FUNC bt_tx_gain_cal_set$part$4 CALL8,
$852 L:
  RETW.N,
$854 L:  END-CODE

CODE phy_wifi_enable_set   \ .iram1 $860
  $854 3 LITERALS
    $3FF5C080 T,  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read
$860 L:
  A1 32 ENTRY,  A2 A2 0 8 EXTUI,  A2 $885 L BEQZ,
  A3 $854 L L32R,  A2 -193 MOVI,  A10 A3 A3 OR,  A8 $858 L L32R,  A8 CALLX8,
  A10 A10 A2 AND,  MEMW,  A10 A3 0 S32I.N,  RETW.N,
  3 ZEROS,
$885 L:
  A2 $854 L L32R,  A10 A2 MOV.N,  A8 $85C L L32R,  A8 CALLX8,
  A8 192 MOVI,  A10 A10 A8 OR,  MEMW,  A10 A2 0 S32I.N,  RETW.N,
$89D L:  END-CODE

CODE set_rx_sense   \ .text.set_rx_sense $8C
  $0 35 LITERALS
    SYMBOL corr_cca_thr  $3FF5C018 T,  $3FF5C01C T,  $FF00FFFF T,  $120000 T,
    $3FF5C010 T,  $7FFFFF T,  $6000000 T,  $3FF5C014 T,  $1000000 T,  $3FF5C0CC T,
    $FFF003FF T,  $A3800 T,  $C00FFFFF T,  $2A800000 T,  $3FF5C044 T,
    $3FF5C124 T,  $200000 T,  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
$8C L:
  A1 48 ENTRY,  A4 $0 L L32R,  A2 A2 0 8 EXTUI,  A6 1 MOVI.N,  A10 $4 L L32R,
  A2 A4 0 S8I,  MEMW,  A8 $48 L L32R,  A8 CALLX8,
  A10 A10 5 10 EXTUI,  A10 A10 -1024 ADDMI,  A10 A10 1 ADDI.N,  A5 A10 31 1 EXTUI,
  A3 A4 0 L8UI,  A10 A5 A10 ADD.N,  A2 A2 7 SEXT,  A3 A3 7 SEXT,  A10 A10 1 SRAI,
  A4 A2 $100 ADDMI,  A10 A3 A10 SUBX2,  A5 0 MOVI.N,  A5 A6 A2 MOVEQZ,
  A3 A10 0 16 EXTUI,  A4 A4 2 SLLI,  A4 A4 0 16 EXTUI,  A2 A5 0 8 EXTUI,
  A6 A3 15 SEXT,  A5 18 MOVI,  A7 A4 15 SEXT,  A5 A6 $E5 L BGE,
  A2 $111 L BEQZ,
$E5 L:
  A5 $8 L L32R,  A10 A5 MOV.N,  A8 $4C L L32R,  A8 CALLX8,
  A8 $C L L32R,  A10 A10 A8 AND,  A8 A1 4 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,
  A8 $10 L L32R,  A10 A10 A8 OR,  MEMW,  A10 A5 0 S32I.N,  $132 L J,
  3 ZEROS,
$111 L:
  A5 $8 L L32R,  A10 A5 MOV.N,  A8 $50 L L32R,  A8 CALLX8,
  A8 A3 0 8 EXTUI,  A9 $C L L32R,  A8 A8 16 SLLI,  A10 A10 A9 AND,  A8 A8 A10 OR,
  A9 A1 4 S32I.N,  MEMW,  A8 A5 0 S32I.N,
$132 L:
  A5 12 MOVI.N,  A5 A6 $139 L BGE,
  A2 $160 L BEQZ.N,
$139 L:
  NOP.N,  NOP.N,  NOP,  A5 $14 L L32R,  A10 A5 A5 OR,  A8 $54 L L32R,  A8 CALLX8,
  A8 $18 L L32R,  A10 A10 A8 AND,  A9 $1C L L32R,  A10 A10 A9 OR,  MEMW,
  A10 A5 0 S32I.N,  $17D L J,
$160 L:
  A5 $14 L L32R,  A10 A5 A5 OR,  A8 $58 L L32R,  A8 CALLX8,
  A9 A3 23 SLLI,  A8 $18 L L32R,  A10 A10 A8 AND,  A9 A10 A9 OR,  MEMW,
  A9 A5 0 S32I.N,
$17D L:
  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A5 $20 L L32R,  A10 A5 MOV.N,  A8 A1 8 S32I.N,
  A6 3 $191 L BLTI,
  A2 $1AC L BEQZ.N,
$191 L:
  A8 $5C L L32R,  A8 CALLX8,
  A8 A1 8 L32I.N,  A10 A10 A8 AND,  A9 $24 L L32R,  A10 A10 A9 OR,  MEMW,
  A10 A5 0 S32I.N,  $1C2 L J,
  2 ZEROS,
$1AC L:
  A8 $60 L L32R,  A8 CALLX8,
  A8 A1 8 L32I.N,  A10 A10 A8 AND,  A8 A3 23 SLLI,  A10 A10 A8 OR,  MEMW,
  A10 A5 0 S32I.N,
$1C2 L:
  A8 $2A8 MOVI,  A5 1 MOVI.N,  A8 A7 $1CC L BGE,
  A5 0 MOVI.N,
$1CC L:
  A5 A2 A5 OR,  A5 A5 0 8 EXTUI,  A5 A1 0 S32I.N,  A5 $1FD L BEQZ.N,
  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A5 $28 L L32R,  A10 A5 MOV.N,  A8 $64 L L32R,
  A8 CALLX8,
  A9 -1024 MOVI,  A10 A10 A9 AND,  A9 $2A8 MOVI,  A10 A10 A9 OR,  MEMW,
  A10 A5 0 S32I.N,  $21A L J,
$1FD L:
  A5 $28 L L32R,  A10 A5 A5 OR,  A8 $68 L L32R,  A8 CALLX8,
  A9 -1024 MOVI,  A10 A10 A9 AND,  A9 A4 0 10 EXTUI,  A10 A10 A9 OR,  MEMW,
  A10 A5 0 S32I.N,
$21A L:
  A9 $28E MOVI,  A10 A5 MOV.N,  A9 A7 $224 L BGE,
  A2 $240 L BEQZ.N,
$224 L:
  A8 $6C L L32R,  A8 CALLX8,
  A7 $2C L L32R,  A10 A10 A7 AND,  A7 $30 L L32R,  A10 A10 A7 OR,  MEMW,
  A10 A5 0 S32I.N,  $25A L J,
  2 ZEROS,
$240 L:
  A8 $70 L L32R,  A8 CALLX8,
  A7 $2C L L32R,  A9 A4 0 10 EXTUI,  A9 A9 10 SLLI,  A10 A10 A7 AND,  A10 A9 A10 OR,
  MEMW,  A10 A5 0 S32I.N,
$25A L:
  A10 A5 MOV.N,  NOP.N,  NOP.N,  NOP.N,  A8 A1 0 L32I.N,  A8 $280 L BEQZ.N,
  A8 $74 L L32R,  A8 CALLX8,
  A4 $34 L L32R,  A10 A10 A4 AND,  A4 $38 L L32R,  A10 A10 A4 OR,  MEMW,
  A10 A5 0 S32I.N,  $29A L J,
$280 L:
  A8 $78 L L32R,  A8 CALLX8,
  A7 $34 L L32R,  A4 A4 0 10 EXTUI,  A4 A4 20 SLLI,  A10 A10 A7 AND,  A4 A4 A10 OR,
  MEMW,  A4 A5 0 S32I.N,
$29A L:
  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A4 $3C L L32R,  A10 A4 MOV.N,  A6 -1 $2AC L BLTI,
  A2 $2C8 L BEQZ.N,
$2AC L:
  A8 $7C L L32R,  A8 CALLX8,
  A5 -256 MOVI,  A10 A10 A5 AND,  A5 254 MOVI,  A10 A10 A5 OR,  MEMW,
  A10 A4 0 S32I.N,  $2DF L J,
  2 ZEROS,
$2C8 L:
  A8 $80 L L32R,  A8 CALLX8,
  A5 -256 MOVI,  A10 A10 A5 AND,  A5 A3 0 8 EXTUI,  A10 A10 A5 OR,  MEMW,
  A10 A4 0 S32I.N,
$2DF L:
  A4 32 MOVI.N,  A4 A6 $2E6 L BGE,
  A2 $30C L BEQZ.N,
$2E6 L:
  NOP.N,  NOP.N,  NOP.N,  A3 $40 L L32R,  A10 A3 A3 OR,  A8 $84 L L32R,  A8 CALLX8,
  A9 A1 4 L32I.N,  A10 A10 A9 AND,  A2 $44 L L32R,  A10 A10 A2 OR,  MEMW,
  A10 A3 0 S32I.N,  RETW.N,
  2 ZEROS,
$30C L:
  A2 $40 L L32R,  A3 A3 0 8 EXTUI,  A10 A2 MOV.N,  A8 $88 L L32R,  A8 CALLX8,
  A3 A3 16 SLLI,  A4 A1 4 L32I.N,  A10 A10 A4 AND,  A3 A3 A10 OR,  MEMW,
  A3 A2 0 S32I.N,  RETW.N,
$32C L:  END-CODE

CODE ram_set_noise_floor   \ .iram1 $8BC
  $8A0 7 LITERALS
    $5400 T,  $3FF5C018 T,  $FFFF801F T,  SYMBOL corr_cca_thr
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL set_rx_sense
$8BC L:
  A1 32 ENTRY,  A2 A2 15 SEXT,  A8 -351 MOVI,  A3 $8A0 L L32R,  A2 A8 $8D7 L BGE,
  A3 -392 MOVI,  A3 A2 A3 MAX,  A3 A3 0 10 EXTUI,  A3 A3 5 SLLI,
$8D7 L:
  A2 $8A4 L L32R,  A10 A2 MOV.N,  A8 $8B0 L L32R,  A8 CALLX8,
  A8 $8A8 L L32R,  A10 A10 A8 AND,  A3 A10 A3 OR,  A10 A2 MOV.N,  MEMW,
  A3 A2 0 S32I,  A8 $8B4 L L32R,  A8 CALLX8,
  A3 16 MOVI.N,  A10 A10 A3 OR,  MEMW,  A10 A2 0 S32I.N,  NOP.N,  NOP.N,  NOP.N,
  NOP.N,  A2 $8AC L L32R,  A10 A2 0 L8UI,  A10 $91C L BEQZ.N,
  A10 A10 7 SEXT,  A8 $8B8 L L32R,  A8 CALLX8,
$91C L:
  RETW.N,
$91E L:  END-CODE

CODE noise_check_loop   \ .text.noise_check_loop $40
  $0 16 LITERALS
    $3FF5D044 T,  $3FF5D050 T,  $3FF5C018 T,  $8000000 T,  $F7FFFFFF T,
    SYMBOL chip7_sleep_params  $3FF5C050 T,  SYMBOL phy_meas_noise_floor
    SYMBOL sw_scan_mode  SYMBOL g_phyFuns  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read
$40 L:
  A1 32 ENTRY,  A4 $0 L L32R,  A2 -2 MOVI.N,  A10 A4 A4 OR,  A8 $28 L L32R,
  A8 CALLX8,
  A2 A10 A2 AND,  A6 $4 L L32R,  A5 1 MOVI.N,  A10 A6 MOV.N,  A3 A3 0 8 EXTUI,
  MEMW,  A2 A4 0 S32I.N,  A8 $2C L L32R,  A8 CALLX8,
  A5 A10 A5 OR,  A2 $8 L L32R,  A10 A2 MOV.N,  MEMW,  A5 A6 0 S32I,  A8 $30 L L32R,
  A8 CALLX8,
  A5 $C L L32R,  A5 A10 A5 OR,  A10 A2 MOV.N,  MEMW,  A5 A2 0 S32I.N,
  A8 $34 L L32R,  A8 CALLX8,
  A8 $10 L L32R,  A8 A10 A8 AND,  A5 $14 L L32R,  A10 $18 L L32R,  MEMW,
  A8 A2 0 S32I.N,  NOP.N,  NOP.N,  NOP,  NOP,  A2 A5 178 L16UI,  A8 $38 L L32R,
  A8 CALLX8,
  A10 A10 0 10 EXTUI,  A9 A10 -1024 ADDMI,  A8 -621 MOVI,  A6 $1C L L32R,
  A10 A10 A8 ADD.N,  A9 A9 15 SEXT,  A10 A10 0 16 EXTUI,  A8 62 MOVI.N,
  A9 A6 0 S16I,  MEMW,  A6 A2 15 SEXT,  A8 A10 $D9 L BGEU,
  $184 L J,
$D9 L:
  A8 -351 MOVI,  A10 -352 MOVI,  A9 A8 $E8 L BGE,
  A8 -392 MOVI,  A10 A9 A8 MAX,
$E8 L:
  A6 A10 $F3 L BGE,
  A6 A6 A6 ADDX2,  A10 A6 A10 ADD.N,  $116 L J,
$F3 L:
  A10 A6 $FC L BLT,
  A8 A5 178 L16SI,  $12A L J,
$FC L:
  A8 A6 -16 ADDI,  A10 A8 $111 L BGE,
  A8 A2 -12 ADDI,  A8 A8 15 SEXT,  A8 A5 178 S16I,  MEMW,  $12A L J,
$111 L:
  A2 A10 A10 ADDX2,  A10 A2 A6 ADD.N,
$116 L:
  A8 A10 -2 ADDI,  A10 A10 1 ADDI.N,  A8 A10 A8 MOVLTZ,  A8 A8 2 SRAI,
  A8 A8 15 SEXT,  A8 A5 178 S16I,  MEMW,
$12A L:
  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A2 $20 L L32R,  A2 A2 0 L8UI,  A2 $158 L BEQZ.N,
  A2 -383 MOVI,  A8 A2 $166 L BLT,
  A2 -384 MOVI,  A8 A8 A2 ADD.N,  A2 A8 31 1 EXTUI,  A8 A2 A8 ADD.N,  A8 A8 1 SRAI,
  A8 A5 78 S16I,  MEMW,  $16C L J,
  2 ZEROS,
$158 L:
  A2 A5 78 L16SI,  A6 A2 4 ADDI.N,  A6 A8 $166 L BLT,
  A2 A2 -4 ADDI,  A8 A2 $184 L BGE,
$166 L:
  A8 A5 78 S16I,  MEMW,
$16C L:
  A3 $184 L BEQZ.N,
  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A2 $24 L L32R,  A2 A2 0 L32I.N,  A10 A5 78 L16SI,
  A2 A2 132 L32I,  A2 CALLX8,
$184 L:
  A10 A4 MOV.N,  A8 $3C L L32R,  A8 CALLX8,
  A2 1 MOVI.N,  A10 A10 A2 OR,  MEMW,  A10 A4 0 S32I.N,  RETW.N,
$198 L:  END-CODE

CODE phy_close_rf   \ .iram1 $948
  $920 10 LITERALS
    $3FF00024 T,  SYMBOL g_phyFuns  $3FF48030 T,  $27FFFFFF T,
    SYMBOL phy_enter_critical  SYMBOL noise_check_loop
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL set_xpd_sar  SYMBOL phy_exit_critical
$948 L:
  A1 32 ENTRY,  A8 $930 L L32R,  A8 CALLX8,
  A11 1 MOVI.N,  A4 A10 MOV.N,  A10 A11 MOV.N,  A8 $934 L L32R,  A8 CALLX8,
  FUNC disable_wifi_agc CALL8,
  A3 $920 L L32R,  A10 A3 A3 OR,  A8 $938 L L32R,  A8 CALLX8,
  A8 -3 MOVI.N,  A2 $924 L L32R,  A10 A10 A8 AND,  MEMW,  A10 A3 0 S32I.N,  NOP.N,
  NOP.N,  NOP.N,  NOP.N,  A3 A2 0 L32I,  A3 A3 192 L32I,  A3 CALLX8,
  A11 1 MOVI.N,  A10 A11 MOV.N,  A3 A2 0 L32I.N,  A12 0 MOVI,  A3 A3 184 L32I,
  A3 CALLX8,
  A2 A2 0 L32I.N,  A2 A2 196 L32I,  A2 CALLX8,
  A2 $928 L L32R,  A10 A2 A2 OR,  A8 $93C L L32R,  A8 CALLX8,
  A8 $92C L L32R,  A8 A10 A8 AND,  A10 0 MOVI.N,  MEMW,  A8 A2 0 S32I.N,
  A8 $940 L L32R,  A8 CALLX8,
  A10 A4 MOV.N,  A8 $944 L L32R,  A8 CALLX8,
  RETW.N,
$9CA L:  END-CODE

CODE ram_bb_bss_cbw40_dig   \ .phyiram.0 $8
  $0 2 LITERALS
    $3FF00024 T,  SYMBOL esp_dport_access_reg_read
$8 L:
  A1 32 ENTRY,  A3 $0 L L32R,  A2 A2 0 1 EXTUI,  A10 A3 MOV.N,  A8 $4 L L32R,
  A8 CALLX8,
  A8 -13 MOVI.N,  A2 A2 2 SLLI,  A10 A10 A8 AND,  A10 A2 A10 OR,  MEMW,
  A10 A3 0 S32I.N,  RETW.N,
$2B L:  END-CODE

CODE ram_set_pbus_mem   \ .text.ram_set_pbus_mem $64
  $0 25 LITERALS
    $FFFF0000 T,  $3FF46030 T,  $FFFF T,  $3FF46034 T,  $3FF46038 T,  $3FF4603C T,
    $3FF46040 T,  $3FF46044 T,  SYMBOL rodata  $10 SYMBOL+ rodata
    $1C SYMBOL+ rodata  $40 SYMBOL+ rodata  $48 SYMBOL+ rodata  $30 SYMBOL+ rodata
    $50 SYMBOL+ rodata  SYMBOL rodata.ram_set_pbus_mem  $3FF45038 T,  $3FF45034 T,
    $FFFC00FF T,  $FFFCFFFF T,  SYMBOL memcpy  SYMBOL memcpy
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read
$64 L:
  A1 208 ENTRY,  A2 $20 L L32R,  A12 20 MOVI.N,  A10 A1 A12 ADD.N,  A3 $24 L L32R,
  A8 A2 0 L32I.N,  A6 A3 0 L32I.N,  A5 A3 4 L32I.N,  A7 A2 4 L32I.N,
  A3 A3 8 L32I.N,  A4 A2 8 L32I.N,  A11 $28 L L32R,  A2 A2 12 L32I.N,
  A8 A1 56 S32I.N,  A7 A1 60 S32I,  A4 A1 64 S32I,  A2 A1 68 S32I,  A2 128 MOVI,
  A2 A1 A2 ADD,  A6 A1 108 S32I,  A3 A1 116 S32I,  A5 A1 112 S32I,  A8 $50 L L32R,
  A8 CALLX8,
  A12 20 MOVI.N,  A10 A1 MOV.N,  A4 $34 L L32R,  A8 $2C L L32R,  A7 $30 L L32R,
  A11 A4 8 L32I.N,  A15 A8 0 L32I.N,  A14 A8 4 L32I.N,  A8 A4 0 L32I.N,
  A13 A7 0 L32I.N,  A9 A7 4 L32I.N,  A11 A1 144 S32I,  NOP.N,  NOP.N,  NOP.N,
  NOP.N,  A7 A4 4 L32I.N,  A4 A4 12 L32I.N,  A8 A1 40 S32I.N,  NOP.N,  NOP.N,
  NOP.N,  NOP.N,  A11 $38 L L32R,  A8 A1 144 L32I,  A4 A1 52 S32I.N,  A4 0 MOVI.N,
  A6 A1 96 S32I,  A3 A1 104 S32I,  A7 A1 44 S32I.N,  A6 A1 84 S32I,  A3 A1 92 S32I,
  A5 A1 100 S32I,  A15 A2 0 S32I.N,  A14 A2 4 S32I.N,  A13 A1 120 S32I,
  A9 A1 124 S32I,  A8 A1 48 S32I.N,  A5 A1 88 S32I,  A8 $54 L L32R,  A8 CALLX8,
  A6 A1 72 S32I,  A3 A1 80 S32I,  A4 A1 144 S32I,  A5 A1 76 S32I,  NOP.N,  NOP.N,
  NOP.N,  NOP.N,  A6 $40 L L32R,  A3 $44 L L32R,  A7 $4C L L32R,  $127 L J,
$124 L:
  A4 A1 148 L32I,
$127 L:
  A9 10 MOVI.N,  A11 A1 144 L32I,  A9 A11 $132 L BGEU,
  $248 L J,
$132 L:
  A2 $3C L L32R,  A2 A11 A2 ADDX4,  A2 A2 0 L32I.N,  A2 JX,
$13D L:
  A2 $0 L L32R,  A5 A1 56 ADDI,  A12 4 MOVI.N,  A2 A1 152 S32I,  A2 0 MOVI.N,
  NOP.N,  NOP.N,  NOP.N,  A13 $4 L L32R,  $271 L J,
$156 L:
  A5 $0 L L32R,  A12 5 MOVI.N,  A2 0 MOVI.N,  A5 A1 152 S32I,  A5 A1 20 ADDI,
  NOP.N,  NOP.N,  NOP.N,  A13 $C L L32R,  $271 L J,
$16F L:
  A8 $8 L L32R,  A5 A1 96 ADDI,  A12 3 MOVI.N,  A2 16 MOVI.N,  A8 A1 152 S32I,
  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A13 $C L L32R,  $271 L J,
$18A L:
  A9 $0 L L32R,  A5 128 MOVI,  A5 A5 A1 ADD.N,  A12 2 MOVI.N,  A2 0 MOVI.N,
  A9 A1 152 S32I,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A13 $10 L L32R,  $271 L J,
$1A7 L:
  A11 $8 L L32R,  A5 A1 120 ADDI,  A12 2 MOVI.N,  A2 16 MOVI.N,  A11 A1 152 S32I,
  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A13 $10 L L32R,  $271 L J,
$1C2 L:
  A2 $0 L L32R,  A5 A1 40 ADDI,  A12 4 MOVI.N,  A2 A1 152 S32I,  A2 0 MOVI.N,
  NOP.N,  NOP.N,  NOP.N,  A13 $14 L L32R,  $271 L J,
$1DB L:
  A5 $8 L L32R,  A12 3 MOVI.N,  A2 16 MOVI.N,  A5 A1 152 S32I,  A5 A1 84 ADDI,
  NOP.N,  NOP.N,  NOP.N,  A13 $14 L L32R,  $271 L J,
$1F4 L:
  A8 $0 L L32R,  A5 A1 MOV.N,  A12 5 MOVI.N,  A2 0 MOVI.N,  A8 A1 152 S32I,  NOP.N,
  NOP.N,  NOP.N,  NOP.N,  A13 $18 L L32R,  $271 L J,
$20E L:
  A9 $8 L L32R,  A5 A1 72 ADDI,  A12 3 MOVI.N,  A2 16 MOVI.N,  A9 A1 152 S32I,
  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A13 $18 L L32R,  $271 L J,
$229 L:
  A11 $0 L L32R,  A5 128 MOVI,  A5 A5 A1 ADD.N,  A12 2 MOVI.N,  A2 0 MOVI.N,
  A11 A1 152 S32I,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A13 $1C L L32R,  $271 L J,
  2 ZEROS,
$248 L:
  A2 $8 L L32R,  A5 A1 120 ADDI,  A12 2 MOVI.N,  A2 A1 152 S32I,  A2 16 MOVI.N,
  NOP.N,  NOP.N,  NOP.N,  A13 $1C L L32R,  $271 L J,
$261 L:
  A5 $8 L L32R,  A12 3 MOVI.N,  A2 16 MOVI.N,  A13 $4 L L32R,  A5 A1 152 S32I,
  A5 A1 108 ADDI,
$271 L:
  A8 A12 A4 ADD.N,  A10 A13 MOV.N,  A8 A1 148 S32I,  A12 A1 156 S32I,
  A13 A1 160 S32I,  A8 $58 L L32R,  A8 CALLX8,
  A9 A1 148 L32I,  A8 A9 -1 ADDI.N,  A8 A8 8 SLLI,  A8 A8 A4 OR,  A11 A1 152 L32I,
  A8 A8 0 16 EXTUI,  A2 SSL,  A8 A8 SLL,  A10 A10 A11 AND,  A2 A8 A10 OR,
  A13 A1 160 L32I,  A12 A1 156 L32I,  MEMW,  A2 A13 0 S32I.N,  A2 A4 $200 ADDMI,
  A4 A12 A5 ADDX4,
$2B2 L:
  A10 A3 MOV.N,  NOP.N,  A8 A5 0 L32I.N,  A5 A5 4 ADDI.N,  MEMW,  A8 A6 0 S32I.N,
  A8 $5C L L32R,  A8 CALLX8,
  A8 A2 0 10 EXTUI,  A8 A8 8 SLLI,  A2 A2 1 ADDI.N,  A9 $48 L L32R,  A10 A10 A9 AND,
  A10 A8 A10 OR,  MEMW,  A10 A3 0 S32I.N,  A10 A3 A3 OR,  A8 $60 L L32R,  A8 CALLX8,
  A10 A10 A7 AND,  MEMW,  A10 A3 0 S32I.N,  A4 A5 $2B2 L BNE,
  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A11 A1 144 L32I,  A11 A11 1 ADDI.N,
  A11 A1 144 S32I,  A11 12 $305 L BEQI,
  $124 L J,
$305 L:
  RETW.N,
  rodata.ram_set_pbus_mem  $13D L!+  $261 L!+  $156 L!+  $16F L!+  $18A L!+  $1A7 L!+
  $1C2 L!+  $1DB L!+  $1F4 L!+  $20E L!+  $229 L!+  DROP   \ the table of jumps
$307 L:  END-CODE

CODE ram_start_tx_tone   \ .text.ram_start_tx_tone $10
  $0 4 LITERALS
    $3FF460B8 T,  $66666667 T,  SYMBOL g_phyFuns  SYMBOL esp_dport_access_reg_read
$10 L:
  A1 32 ENTRY,  A10 $0 L L32R,  A8 $C L L32R,  A8 CALLX8,
  A8 $4 L L32R,  A10 29 $49 L BBCI,
  A3 A3 7 SLLI,  A6 A6 7 SLLI,  A11 A3 A8 MULSH,  A8 A6 A8 MULSH,  A9 A11 1 SRAI,
  A8 A8 1 SRAI,  A11 A3 31 SRAI,  A14 A6 31 SRAI,  A11 A9 A11 SUB,  A14 A8 A14 SUB,
  A11 A11 0 12 EXTUI,  A14 A14 0 12 EXTUI,  $6D L J,
$49 L:
  A3 A3 5 SLLI,  A6 A6 5 SLLI,  A11 A3 A8 MULSH,  A8 A6 A8 MULSH,  A9 A11 1 SRAI,
  A8 A8 1 SRAI,  A11 A3 31 SRAI,  A14 A6 31 SRAI,  A11 A9 A11 SUB,  A14 A8 A14 SUB,
  A11 A11 0 10 EXTUI,  A14 A14 0 10 EXTUI,
$6D L:
  A3 $8 L L32R,  A10 A2 0 8 EXTUI,  A15 A7 0 8 EXTUI,  A13 A5 0 8 EXTUI,
  A12 A4 0 8 EXTUI,  A3 A3 0 L32I.N,  A2 A3 68 L32I,  A2 CALLX8,
  RETW.N,
$86 L:  END-CODE

CODE ram_bb_tx_ht20_cen   \ .text.ram_bb_tx_ht20_cen $C
  $0 3 LITERALS
    $60033C6C T,  $F7FFFFFF T,  SYMBOL esp_dport_access_reg_read
$C L:
  A1 32 ENTRY,  A3 $0 L L32R,  A2 A2 0 1 EXTUI,  A10 A3 MOV.N,  A8 $8 L L32R,
  A8 CALLX8,
  A2 A2 27 SLLI,  A8 $4 L L32R,  A10 A10 A8 AND,  A10 A2 A10 OR,  MEMW,
  A10 A3 0 S32I.N,  RETW.N,
$30 L:  END-CODE

CODE ram_phy_get_noisefloor   \ .text.ram_phy_get_noisefloor $10
  $0 4 LITERALS
    $3FF5C050 T,  $3FF5D050 T,  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read
$10 L:
  A1 32 ENTRY,  A10 $0 L L32R,  A2 -351 MOVI,  A8 $8 L L32R,  A8 CALLX8,
  A10 A10 0 10 EXTUI,  A10 A10 -1024 ADDMI,  A10 A10 15 SEXT,  A10 A2 $34 L BGE,
  A2 -392 MOVI,  A2 A10 A2 MAX,  $37 L J,
$34 L:
  A2 -352 MOVI,
$37 L:
  A3 $4 L L32R,  A10 A3 MOV.N,  A8 $C L L32R,  A8 CALLX8,
  A8 1 MOVI.N,  A10 A10 A8 OR,  MEMW,  A10 A3 0 S32I.N,  RETW.N,
$4E L:  END-CODE

CODE ram_check_noise_floor   \ .text.ram_check_noise_floor $40
  $0 16 LITERALS
    $3FF5C018 T,  $FF7FFFFF T,  $2000000 T,  $FDFFFFFF T,  $800000 T,
    $60033C00 T,  $2710 T,  $1000000 T,  $3FF5C050 T,
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read
$40 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A7 0 MOVI.N,  A10 A2 A2 OR,  A8 $24 L L32R,
  A8 CALLX8,
  A3 $4 L L32R,  A3 A10 A3 AND,  A10 A2 MOV.N,  MEMW,  A3 A2 0 S32I,  A8 $28 L L32R,
  A8 CALLX8,
  A3 $8 L L32R,  A3 A10 A3 OR,  A10 A2 MOV.N,  MEMW,  A3 A2 0 S32I,  A8 $2C L L32R,
  A8 CALLX8,
  A3 $C L L32R,  A3 A10 A3 AND,  A10 A2 MOV.N,  MEMW,  A3 A2 0 S32I.N,
  A8 $30 L L32R,  A8 CALLX8,
  A3 $10 L L32R,  A10 A10 A3 OR,  A4 $14 L L32R,  MEMW,  A10 A2 0 S32I.N,  NOP.N,
  NOP.N,  NOP.N,  NOP.N,  MEMW,  A6 A4 0 L32I.N,  A3 $1C L L32R,  A5 $18 L L32R,
  $C4 L J,
  1 ZEROS,
$B1 L:
  MEMW,  A8 A4 0 L32I.N,  A9 1 MOVI.N,  A8 A8 A6 SUB,  A5 A8 $C1 L BLTU,
  A9 A7 A7 OR,
$C1 L:
  A7 A9 0 8 EXTUI,
$C4 L:
  A10 A2 MOV.N,  A8 $34 L L32R,  A8 CALLX8,
  A10 A10 A3 AND,  A10 A10 24 8 EXTUI,  A10 $DC L BNEZ.N,
  A7 $B1 L BEQZ,
  $108 L J,
  2 ZEROS,
$DC L:
  A10 $20 L L32R,  A4 -351 MOVI,  A8 $38 L L32R,  A8 CALLX8,
  A10 A10 0 10 EXTUI,  A3 A10 -1024 ADDMI,  A3 A3 15 SEXT,  A2 -352 MOVI,
  A3 A4 $FD L BGE,
$F7 L:
  A2 -392 MOVI,  A2 A3 A2 MAX,
$FD L:
  A7 $128 L BEQZ.N,
  A2 A2 NEG,  A2 A2 15 SEXT,  RETW.N,
  1 ZEROS,
$108 L:
  A10 $20 L L32R,  A4 -351 MOVI,  A8 $3C L L32R,  A8 CALLX8,
  A10 A10 0 10 EXTUI,  A3 A10 -1024 ADDMI,  A3 A3 15 SEXT,  A2 $160 MOVI,
  A3 A4 $128 L BGE,
  A7 1 MOVI.N,  $F7 L J,
$128 L:
  RETW.N,
$12A L:  END-CODE

CODE ram_cbw2040_cfg   \ .phyiram.1 $20
  $0 8 LITERALS
    $60033C6C T,  $8000000 T,  $3FF5D000 T,  $F7FFFFFF T,
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
$20 L:
  A1 32 ENTRY,  A2 A2 0 8 EXTUI,  A2 $59 L BEQZ,
  A4 $0 L L32R,  A10 A4 MOV.N,  A8 $10 L L32R,  A8 CALLX8,
  A2 $4 L L32R,  A2 A10 A2 OR,  A3 $8 L L32R,  A10 A3 MOV.N,  MEMW,  A2 A4 0 S32I.N,
  A8 $14 L L32R,  A8 CALLX8,
  A2 64 MOVI.N,  A10 A10 A2 OR,  MEMW,  A10 A3 0 S32I.N,  RETW.N,
  3 ZEROS,
$59 L:
  A3 $0 L L32R,  A10 A3 MOV.N,  A8 $18 L L32R,  A8 CALLX8,
  A8 $C L L32R,  A8 A10 A8 AND,  A2 $8 L L32R,  A10 A2 A2 OR,  MEMW,  A8 A3 0 S32I,
  A8 $1C L L32R,  A8 CALLX8,
  A8 -65 MOVI,  A10 A10 A8 AND,  MEMW,  A10 A2 0 S32I.N,  RETW.N,
$89 L:  END-CODE

CODE ram_bb_bss_bw_40_en   \ .text.ram_bb_bss_bw_40_en $8
  $0 2 LITERALS
    $3FF00024 T,  SYMBOL esp_dport_access_reg_read
$8 L:
  A1 32 ENTRY,  A2 $28 L BEQZ.N,
  A2 $0 L L32R,  A10 A2 A2 OR,  A8 $4 L L32R,  A8 CALLX8,
  A8 -13 MOVI.N,  A10 A10 A8 AND,  A8 4 MOVI.N,  A10 A10 A8 OR,  MEMW,
  A10 A2 0 S32I.N,
$28 L:
  RETW.N,
$2A L:  END-CODE

PRIVATE
CODE bt_txdc_cal$part$5   \ .text.bt_txdc_cal$part$5 $1C, static
  $0 7 LITERALS
    SYMBOL g_phyFuns  SYMBOL tx_rf_ana_gain  SYMBOL CSWTCH$356
    $98 SYMBOL+ chip7_sleep_params  $6 SYMBOL+ CSWTCH$356
    SYMBOL chip7_sleep_params  $1000 T,
$1C L:
  A1 48 ENTRY,  A4 $0 L L32R,  A2 A4 0 L32I.N,  A2 A2 192 L32I,  A2 CALLX8,
  A2 $4 L L32R,  A3 A4 0 L32I,  A11 A2 2 L16UI,  A10 A2 0 L16UI,  A3 A3 216 L32I,
  A3 CALLX8,
  A11 1 MOVI.N,  A10 A11 A11 OR,  A2 A4 0 L32I,  A3 A2 188 L32I,  A2 A2 184 L32I,
  A3 CALLX8,
  A12 2 MOVI.N,  A12 A10 A12 OR,  A11 1 MOVI.N,  A12 A12 0 16 EXTUI,
  A10 A11 A11 OR,  A2 CALLX8,
  A2 $8 L L32R,  A12 0 MOVI.N,  A5 $C L L32R,  A3 $10 L L32R,
$68 L:
  A11 2 MOVI.N,  A10 1 MOVI.N,  A8 A4 0 L32I.N,  A8 A8 184 L32I,  A8 CALLX8,
  A10 A1 MOV.N,  A8 A4 0 L32I.N,  A8 A8 $118 L32I,  A8 CALLX8,
  A10 A1 MOV.N,  A9 A5 MOV.N,  A8 4 MOVI,  A8 $95 L LOOP,
  A11 A10 0 L16UI,  A10 A10 2 ADDI.N,  A11 A9 0 S16I,  MEMW,  A9 A9 2 ADDI.N,
$95 L:
  A2 A2 2 ADDI.N,  A5 A5 8 ADDI.N,  A2 A3 $A4 L BEQ,
  NOP.N,  A12 A2 0 L16UI,  $68 L J,
$A4 L:
  A2 A4 0 L32I.N,  A10 0 MOVI.N,  A2 A2 208 L32I,  A2 CALLX8,
  A2 A4 0 L32I.N,  A2 A2 196 L32I,  A2 CALLX8,
  A3 $14 L L32R,  A2 $18 L L32R,  A4 A3 0 L32I.N,  A2 A4 A2 OR,  A2 A3 0 S32I.N,
  RETW.N,
$C5 L:  END-CODE

CODE bt_txiq_cal$part$6   \ .text.bt_txiq_cal$part$6 $20, static
  $0 8 LITERALS
    SYMBOL chip7_sleep_params  SYMBOL CSWTCH$356  $98 SYMBOL+ chip7_sleep_params
    $92 SYMBOL+ chip7_sleep_params  $6 SYMBOL+ CSWTCH$356  SYMBOL tx_rf_ana_gain
    $800 T,  SYMBOL rfcal_txiq
$20 L:
  A1 32 ENTRY,  A4 $0 L L32R,  A10 0 MOVI.N,  A5 120 MOVI,  A3 A4 116 L8UI,
  A3 A3 7 SEXT,  A3 A3 43 ADDI,  A3 A3 0 16 EXTUI,  A2 $4 L L32R,  A7 $8 L L32R,
  A6 $C L L32R,
$40 L:
  A8 0 MOVI.N,  A12 A10 6 8 EXTUI,  A14 A10 0 6 EXTUI,  A11 A8 MOV.N,  A9 6 MOVI,
  A9 $5A L LOOP,
  A13 A11 6 ADDI.N,  A14 A8 $58 L BBC,
  A11 A13 0 8 EXTUI,
$58 L:
  A8 A8 1 ADDI.N,
$5A L:
  A8 $14 L L32R,  A11 A12 A11 ADD.N,  A12 A11 0 8 EXTUI,  A9 6 MOVI.N,
  A13 A8 2 L16UI,  A8 0 MOVI.N,  A14 A13 6 8 EXTUI,  A11 A8 MOV.N,
  A13 A13 0 6 EXTUI,  A9 $7E L LOOP,
  A15 A11 6 ADDI.N,  A13 A8 $7C L BBC,
  A11 A15 0 8 EXTUI,
$7C L:
  A8 A8 1 ADDI.N,
$7E L:
  A11 A14 A11 ADD.N,  A11 A11 0 8 EXTUI,  A11 A12 A11 SUB,  A11 A11 A3 ADDX4,
  A11 A11 15 SEXT,  A14 0 MOVI.N,  A11 A14 $9A L BLT,
  A14 120 MOVI,  A5 A11 $9A L BLT,
  A14 A11 A11 OR,
$9A L:
  A12 A6 MOV.N,  A11 A7 MOV.N,  A15 1 MOVI.N,  A13 32 MOVI.N,  A8 $1C L L32R,
  A8 CALLX8,
  A8 $10 L L32R,  A2 A2 2 ADDI.N,  A7 A7 8 ADDI.N,  A6 A6 2 ADDI.N,
  A8 A2 $BA L BEQ,
  A10 A2 0 L16UI,  $40 L J,
$BA L:
  A3 A4 0 L32I.N,  A2 $18 L L32R,  A2 A3 A2 OR,  A2 A4 0 S32I.N,  RETW.N,
$C6 L:  END-CODE

CODE spur_cal$part$8   \ .text.spur_cal$part$8 $0, static
  $0 ORIGIN
  A1 32 ENTRY,  A8 A5 A2 MUL16U,  A3 A3 7 SEXT,  A8 A8 A4 QUOS,  A8 A8 A4 MULL,
  A10 A2 A2 ADDX4,  A9 A8 A8 ADDX4,  A9 A9 1 SLLI,  A9 A9 A5 QUOS,  A3 A3 A3 ADDX4,
  A9 A10 A9 SUBX2,  A3 A3 1 SLLI,  A9 A3 $2C L BGE,
  A2 A9 NEG,  RETW.N,
$2C L:
  A8 A4 A8 ADD.N,  A9 A2 29 SLLI,  A8 A8 A8 ADDX4,  A8 A8 1 SLLI,  A9 A9 A2 SUB,
  A5 A8 A5 QUOS,  A2 A9 A2 SUBX4,  A2 A2 A5 ADDX2,  A2 A3 $48 L BLT,
  A2 0 MOVI.N,
$48 L:
  RETW.N,
$4A L:  END-CODE

PUBLIC
CODE ram_spur_coef_cfg   \ .phyiram.2 $54
  $0 21 LITERALS
    SYMBOL chip7_phy_init_ctrl  $3FF5D014 T,  $FFFFDFFF T,  $51EB851F T,
    SYMBOL g_phyFuns  $3FF5D018 T,  $3FF5D01C T,  $3FF5D020 T,  $3FF5CC48 T,
    $FF000000 T,  $FFFFFF T,  SYMBOL rtc_get_xtal
    SYMBOL esp_dport_access_reg_read  SYMBOL spur_cal$part$8
    SYMBOL esp_dport_access_reg_read  SYMBOL spur_cal$part$8
    SYMBOL esp_dport_access_reg_read  SYMBOL spur_cal$part$8
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read
$54 L:
  A1 48 ENTRY,  A4 A4 7 SEXT,  A2 A2 0 8 EXTUI,  A3 A3 0 16 EXTUI,  A7 10 MOVI,
  A4 2 $69 L BLTI,
  A7 20 MOVI,
$69 L:
  A4 $0 L L32R,  A5 A4 1 L8UI,  A5 3 $78 L BNEI,
  A8 $2C L L32R,  A8 CALLX8,
$78 L:
  A6 $4 L L32R,  A10 A6 A6 OR,  A8 $30 L L32R,  A8 CALLX8,
  A5 $8 L L32R,  A10 A10 A5 AND,  A8 A4 64 L16UI,  MEMW,  A10 A6 0 S32I.N,
  A8 $F9 L BEQZ,
  A9 1 MOVI.N,  NOP.N,  NOP.N,  NOP.N,  A13 A4 62 L8UI,  A12 A4 60 L16UI,
  A13 $F9 L BEQZ,
  A12 $F9 L BEQZ,
  A11 A7 MOV.N,  A10 A3 MOV.N,  A8 A1 0 S32I.N,  A9 A1 4 S32I,  A8 $34 L L32R,
  A8 CALLX8,
  A11 A10 10 SLLI,  A13 A2 31 ADDI,  A6 $C L L32R,  A6 A11 A6 MULSH,
  A11 A11 31 SRAI,  A6 A6 5 SRAI,  A8 A1 0 L32I.N,  A11 A6 A11 SUB,  A12 A8 14 SRLI,
  A13 SSR,  A8 A8 SRA,  A13 0 MOVI.N,  A9 A1 4 L32I.N,  A8 A8 A12 AND,
  A13 A9 A10 MOVNEZ,  A8 A13 $F9 L BNONE,
  A11 $F9 L BEQZ.N,
  A6 $10 L L32R,  A10 A9 MOV.N,  A6 A6 0 L32I.N,  A6 A6 96 L32I,  A6 CALLX8,
  $10C L J,
  2 ZEROS,
$F9 L:
  A6 $14 L L32R,  A10 A6 MOV.N,  A8 $38 L L32R,  A8 CALLX8,
  A10 A10 A5 AND,  MEMW,  A10 A6 0 S32I.N,
$10C L:
  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A6 A4 70 L16UI,  A6 $171 L BEQZ,
  A13 A4 68 L8UI,  A12 A4 66 L16UI,  A13 $171 L BEQZ,
  A12 $171 L BEQZ,
  A11 A7 MOV.N,  A10 A3 MOV.N,  A8 $3C L L32R,  A8 CALLX8,
  A11 $C L L32R,  A9 A10 10 SLLI,  A11 A9 A11 MULSH,  A12 A2 31 ADDI,  A12 SSR,
  A12 A6 SRA,  A8 A11 5 SRAI,  A6 A6 14 SRLI,  A11 A9 31 SRAI,  A13 1 MOVI.N,
  A9 0 MOVI.N,  A9 A13 A10 MOVNEZ,  A6 A12 A6 AND,  A11 A8 A11 SUB,
  A6 A9 $171 L BNONE,
  A11 $171 L BEQZ.N,
  A6 $10 L L32R,  A10 2 MOVI,  A6 A6 0 L32I,  A6 A6 96 L32I,  A6 CALLX8,
  $184 L J,
  2 ZEROS,
$171 L:
  A6 $18 L L32R,  A10 A6 MOV.N,  A8 $40 L L32R,  A8 CALLX8,
  A10 A10 A5 AND,  MEMW,  A10 A6 0 S32I.N,
$184 L:
  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A6 A4 76 L16UI,  A6 $1E9 L BEQZ,
  A13 A4 74 L8UI,  A12 A4 72 L16UI,  A13 $1E9 L BEQZ,
  A12 $1E9 L BEQZ,
  A11 A7 MOV.N,  A10 A3 MOV.N,  A8 $44 L L32R,  A8 CALLX8,
  A11 $C L L32R,  A7 A10 10 SLLI,  A11 A7 A11 MULSH,  A2 A2 31 ADDI,  A8 A6 14 SRLI,
  A4 A11 5 SRAI,  A2 SSR,  A6 A6 SRA,  A11 A7 31 SRAI,  A2 0 MOVI.N,  A7 1 MOVI.N,
  A6 A6 A8 AND,  A2 A7 A10 MOVNEZ,  A11 A4 A11 SUB,  A6 A2 $1E9 L BNONE,
  A11 $1E9 L BEQZ.N,
  A2 $10 L L32R,  A10 3 MOVI,  A2 A2 0 L32I,  A2 A2 96 L32I,  A2 CALLX8,
  $1FD L J,
  2 ZEROS,
$1E9 L:
  A2 $1C L L32R,  A10 A2 A2 OR,  A8 $48 L L32R,  A8 CALLX8,
  A10 A10 A5 AND,  MEMW,  A10 A2 0 S32I.N,
$1FD L:
  A4 80 MOVI.N,  NOP.N,  NOP.N,  NOP.N,  A5 $20 L L32R,  A10 A5 MOV.N,
  A8 $4C L L32R,  A8 CALLX8,
  A10 A10 24 5 EXTUI,  A10 SSL,  A4 A4 SLL,  A10 A5 MOV.N,  A8 $50 L L32R,
  A8 CALLX8,
  A3 A4 A3 QUOS,  A4 $24 L L32R,  A10 A10 A4 AND,  A4 $28 L L32R,  A3 A3 A4 AND,
  A10 A10 A3 OR,  MEMW,  A10 A5 0 S32I.N,  RETW.N,
$23A L:  END-CODE

CODE set_chan_rxcomp   \ .phyiram.3 $28
  $0 10 LITERALS
    SYMBOL chip7_sleep_params  SYMBOL chip7_phy_init_ctrl  $3FF5C02C T,
    $3FF5C0A0 T,  $FFFFFF T,  $3FF5C0D0 T,  $FFFE01FF T,
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read
$28 L:
  A1 32 ENTRY,  A3 $0 L L32R,  A2 A2 0 8 EXTUI,  A4 0 MOVI.N,  A3 A3 81 L8UI,
  A3 4 $45 L BLTUI,
  A5 8 MOVI,  A4 2 MOVI,  A5 A3 $45 L BLTU,
  A4 1 MOVI,
$45 L:
  A3 A2 7 SEXT,  A2 $4 L L32R,  A4 A2 A4 ADD.N,  A6 $8 L L32R,  A10 A6 A6 OR,
  A2 A4 2 L8UI,  A8 $1C L L32R,  A8 CALLX8,
  A2 A2 7 SEXT,  A2 A2 A3 ADD.N,  A3 -256 MOVI,  A5 $C L L32R,  A10 A10 A3 AND,
  A3 A2 0 8 EXTUI,  A3 A10 A3 OR,  A10 A5 MOV.N,  MEMW,  A3 A6 0 S32I.N,
  A8 $20 L L32R,  A8 CALLX8,
  A3 A2 24 SLLI,  A8 $10 L L32R,  A10 A10 A8 AND,  A8 A10 A3 OR,  A3 $14 L L32R,
  A10 A3 MOV.N,  MEMW,  A8 A5 0 S32I.N,  A8 $24 L L32R,  A8 CALLX8,
  A4 A4 11 L8UI,  A4 A4 7 SEXT,  A2 A4 A2 ADD.N,  A2 A2 -18 ADDI,  A8 $18 L L32R,
  A2 A2 0 8 EXTUI,  A2 A2 9 SLLI,  A10 A10 A8 AND,  A2 A2 A10 OR,  MEMW,
  A2 A3 0 S32I.N,  RETW.N,
$BA L:  END-CODE

CODE phy_ant_init   \ .phyiram.5 $20
  $0 8 LITERALS
    $3FF5C030 T,  $FFFC07FF T,  $1A000 T,  $3FF5C11C T,  $E000 T,  $3FF5C120 T,
    $1E101E1C T,  SYMBOL esp_dport_access_reg_read
$20 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A10 A2 MOV.N,  A8 $1C L L32R,  A8 CALLX8,
  A8 $4 L L32R,  A10 A10 A8 AND,  A8 $8 L L32R,  A10 A10 A8 OR,  MEMW,
  A10 A2 0 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A8 $10 L L32R,  A2 $C L L32R,
  MEMW,  A8 A2 0 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A8 $18 L L32R,
  A2 $14 L L32R,  MEMW,  A8 A2 0 S32I.N,  RETW.N,
$67 L:  END-CODE

CODE tx_delay_cfg   \ .phyiram.6 $34
  $0 13 LITERALS
    $3FF5D030 T,  $3FF450F0 T,  $3FF5D000 T,  $E07FFFFF T,  $A000000 T,
    $F000000 T,  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL phy_bt_ifs_set
$34 L:
  A1 32 ENTRY,  A2 A2 0 8 EXTUI,  A2 $95 L BEQZ,
  A3 $0 L L32R,  A2 -1024 MOVI,  A10 A3 A3 OR,  A8 $18 L L32R,  A8 CALLX8,
  A10 A10 A2 AND,  A4 $4 L L32R,  A2 135 MOVI,  A2 A10 A2 OR,  A10 A4 MOV.N,  MEMW,
  A2 A3 0 S32I.N,  A8 $1C L L32R,  A8 CALLX8,
  A2 -512 MOVI,  A10 A10 A2 AND,  A2 240 MOVI,  A3 $8 L L32R,  A2 A10 A2 OR,
  A10 A3 MOV.N,  MEMW,  A2 A4 0 S32I.N,  A8 $20 L L32R,  A8 CALLX8,
  A2 $C L L32R,  A10 A10 A2 AND,  A2 $10 L L32R,  A10 A10 A2 OR,  MEMW,
  A10 A3 0 S32I.N,  $EA L J,
$95 L:
  A4 $0 L L32R,  A2 100 MOVI,  A10 A4 A4 OR,  A8 $24 L L32R,  A8 CALLX8,
  A8 -1024 MOVI,  A3 $4 L L32R,  A10 A10 A8 AND,  A8 A10 A2 OR,  A10 A3 MOV.N,
  MEMW,  A8 A4 0 S32I.N,  A8 $28 L L32R,  A8 CALLX8,
  A4 -512 MOVI,  A10 A10 A4 AND,  A2 A10 A2 OR,  A4 $8 L L32R,  A10 A4 A4 OR,  MEMW,
  A2 A3 0 S32I,  A8 $2C L L32R,  A8 CALLX8,
  A2 $C L L32R,  A10 A10 A2 AND,  A2 $14 L L32R,  A10 A10 A2 OR,  MEMW,
  A10 A4 0 S32I,
$EA L:
  A8 $30 L L32R,  A8 CALLX8,
  RETW.N,
$F2 L:  END-CODE

CODE bb_bss_cbw40   \ .phyiram.4 $34
  $0 13 LITERALS
    SYMBOL chip7_sleep_params  SYMBOL g_phyFuns  $3FF5C450 T,  $3FF5CC0C T,
    SYMBOL bb_bss_cbw40_ana  SYMBOL tx_delay_cfg  SYMBOL esp_dport_access_reg_read
    SYMBOL bb_bss_cbw40_ana  SYMBOL bb_bss_cbw40_ana
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL tx_delay_cfg  SYMBOL esp_dport_access_reg_read
$34 L:
  A1 32 ENTRY,  A2 A2 0 8 EXTUI,  A2 $7D L BNEZ.N,
  A3 $4 L L32R,  A10 A2 MOV.N,  A3 A3 0 L32I.N,  A8 $0 L L32R,  A3 A3 112 L32I,
  A2 A8 82 S8I,  MEMW,  A3 CALLX8,
  A10 A2 MOV.N,  A8 $10 L L32R,  A8 CALLX8,
  A10 A2 MOV.N,  A8 $14 L L32R,  A8 CALLX8,
  A3 $8 L L32R,  A2 -13 MOVI.N,  A10 A3 A3 OR,  A8 $18 L L32R,  A8 CALLX8,
  A10 A10 A2 AND,  MEMW,  A10 A3 0 S32I.N,  RETW.N,
  3 ZEROS,
$7D L:
  A3 $4 L L32R,  A11 1 MOVI.N,  A2 A2 7 SEXT,  A10 1 MOVI.N,  A8 A3 0 L32I.N,
  A9 $0 L L32R,  A8 A8 112 L32I,  A11 A9 82 S8I,  MEMW,  A8 CALLX8,
  A2 1 $B0 L BNEI,
  A10 A2 MOV.N,  A2 A3 0 L32I.N,  A2 A2 116 L32I,  A2 CALLX8,
  A10 0 MOVI.N,  A8 $1C L L32R,  A8 CALLX8,
  $E0 L J,
$B0 L:
  A3 A3 0 L32I.N,  A10 0 MOVI.N,  A3 A3 116 L32I,  A3 CALLX8,
  A10 1 MOVI.N,  A8 $20 L L32R,  A8 CALLX8,
  A2 3 $E0 L BNEI,
  A3 $C L L32R,  A2 1 MOVI,  A10 A3 A3 OR,  A8 $24 L L32R,  A8 CALLX8,
  A10 A10 A2 OR,  MEMW,  A10 A3 0 S32I.N,  $F5 L J,
  1 ZEROS,
$E0 L:
  A3 $C L L32R,  A2 -2 MOVI.N,  A10 A3 MOV.N,  A8 $28 L L32R,  A8 CALLX8,
  A10 A10 A2 AND,  MEMW,  A10 A3 0 S32I.N,
$F5 L:
  A10 1 MOVI.N,  A8 $2C L L32R,  A8 CALLX8,
  A2 12 MOVI,  A3 $8 L L32R,  A10 A3 A3 OR,  A8 $30 L L32R,  A8 CALLX8,
  A10 A10 A2 OR,  MEMW,  A10 A3 0 S32I.N,  RETW.N,
$116 L:  END-CODE

CODE tx_paon_set   \ .phyiram.7 $50
  $0 20 LITERALS
    $3FF5D000 T,  $E00007FF T,  $F7E8000 T,  $3FF46048 T,  $FFFF00FF T,  $A400 T,
    $3FF5C400 T,  $FFF8FFFF T,  $3FF450D0 T,  $BFFFFFFF T,  $3FF450C4 T,
    $F4240 T,  $3FF450C8 T,  $3FF4E054 T,  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL tx_delay_cfg
$50 L:
  A1 32 ENTRY,  A3 $0 L L32R,  A10 A3 MOV.N,  A8 $38 L L32R,  A8 CALLX8,
  A8 $4 L L32R,  A10 A10 A8 AND,  A8 $8 L L32R,  A8 A10 A8 OR,  A2 $C L L32R,
  A10 A2 A2 OR,  MEMW,  A8 A3 0 S32I,  A8 $3C L L32R,  A8 CALLX8,
  A8 $10 L L32R,  A10 A10 A8 AND,  A8 $14 L L32R,  A8 A10 A8 OR,  A3 $18 L L32R,
  A10 A3 MOV.N,  MEMW,  A8 A2 0 S32I.N,  A8 $40 L L32R,  A8 CALLX8,
  A8 $1C L L32R,  A8 A10 A8 AND,  A2 $20 L L32R,  A10 A2 A2 OR,  MEMW,
  A8 A3 0 S32I,  A8 $44 L L32R,  A8 CALLX8,
  A8 $24 L L32R,  A10 A10 A8 AND,  A3 $28 L L32R,  A8 $2C L L32R,  MEMW,
  A10 A2 0 S32I.N,  MEMW,  A8 A3 0 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,
  A2 $34 L L32R,  A10 A2 MOV.N,  A3 $30 L L32R,  MEMW,  A8 A3 0 S32I.N,
  A8 $48 L L32R,  A8 CALLX8,
  A8 -256 MOVI,  A10 A10 A8 AND,  A8 100 MOVI,  A10 A10 A8 OR,  MEMW,
  A10 A2 0 S32I.N,  A10 0 MOVI.N,  A8 $4C L L32R,  A8 CALLX8,
  RETW.N,
$FC L:  END-CODE

CODE agc_reg_init   \ .phyiram.8 $B0
  $0 44 LITERALS
    $3FF5C004 T,  $FFFEFFFF T,  $3FF5C0A4 T,  $FFFFC07F T,  $1E00 T,  $3FF5C0C4 T,
    $6450403F T,  $3FF5C030 T,  $3C1A4D5 T,  $3FF5C094 T,  $1B8DD03 T,
    $3FF5C02C T,  $324048FE T,  $800000 T,  $FF7FFFFF T,  $3FF5C018 T,
    $40000000 T,  $3FF5C01C T,  $3FF5C028 T,  $3FF5C0F8 T,  $FFFC01FF T,
    $37400 T,  $3FF5C038 T,  $F7FFFFFF T,  $3FF5C088 T,  $3FF5C104 T,
    $FFFF7FFF T,  $3FF5C07C T,  $FF00FFFF T,  $9C0000 T,
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
$B0 L:
  A1 32 ENTRY,  A4 $0 L L32R,  A10 A4 MOV.N,  A8 $78 L L32R,  A8 CALLX8,
  A3 $4 L L32R,  A3 A10 A3 AND,  A2 $8 L L32R,  A10 A2 MOV.N,  MEMW,
  A3 A4 0 S32I.N,  A8 $7C L L32R,  A8 CALLX8,
  A3 $C L L32R,  A4 A10 A3 AND,  A10 A2 MOV.N,  A3 $10 L L32R,  A3 A4 A3 OR,  MEMW,
  A3 A2 0 S32I.N,  A8 $80 L L32R,  A8 CALLX8,
  A3 -128 MOVI,  A10 A10 A3 AND,  A3 60 MOVI.N,  A10 A10 A3 OR,  MEMW,
  A10 A2 0 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A3 $18 L L32R,  A2 $14 L L32R,
  A4 $20 L L32R,  MEMW,  A3 A2 0 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,
  A3 $1C L L32R,  MEMW,  A4 A3 0 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,
  A4 $28 L L32R,  A3 $24 L L32R,  A2 $2C L L32R,  A10 A2 MOV.N,  MEMW,
  A4 A3 0 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A3 $30 L L32R,  MEMW,
  A3 A2 0 S32I.N,  A8 $84 L L32R,  A8 CALLX8,
  A3 $34 L L32R,  A3 A10 A3 OR,  A10 A2 MOV.N,  MEMW,  A3 A2 0 S32I.N,
  A8 $88 L L32R,  A8 CALLX8,
  A4 $38 L L32R,  A4 A10 A4 AND,  A3 $3C L L32R,  A10 A3 MOV.N,  MEMW,
  A4 A2 0 S32I.N,  A8 $8C L L32R,  A8 CALLX8,
  A2 -8 MOVI.N,  A10 A10 A2 AND,  A2 4 MOVI.N,  A10 A10 A2 OR,  MEMW,
  A10 A3 0 S32I.N,  A10 A3 MOV.N,  A8 $90 L L32R,  A8 CALLX8,
  A2 $40 L L32R,  A2 A10 A2 OR,  A4 $44 L L32R,  A10 A4 MOV.N,  MEMW,  A2 A3 0 S32I,
  A8 $94 L L32R,  A8 CALLX8,
  A2 -256 MOVI,  A10 A10 A2 AND,  A3 191 MOVI,  A5 $48 L L32R,  A3 A10 A3 OR,
  A10 A5 MOV.N,  MEMW,  A3 A4 0 S32I.N,  A8 $98 L L32R,  A8 CALLX8,
  A3 -512 MOVI,  A10 A10 A3 AND,  A3 $1BA MOVI,  A4 $4C L L32R,  A3 A10 A3 OR,
  A10 A4 MOV.N,  MEMW,  A3 A5 0 S32I.N,  A8 $9C L L32R,  A8 CALLX8,
  A3 $50 L L32R,  A10 A10 A3 AND,  A3 $54 L L32R,  A3 A10 A3 OR,  A5 $58 L L32R,
  A10 A5 MOV.N,  MEMW,  A3 A4 0 S32I.N,  A8 $A0 L L32R,  A8 CALLX8,
  A3 $5C L L32R,  A3 A10 A3 AND,  A4 $60 L L32R,  A10 A4 A4 OR,  MEMW,
  A3 A5 0 S32I,  A8 $A4 L L32R,  A8 CALLX8,
  A10 A10 A2 AND,  A2 5 MOVI.N,  A2 A10 A2 OR,  A5 $64 L L32R,  A10 A5 MOV.N,  MEMW,
  A2 A4 0 S32I.N,  A8 $A8 L L32R,  A8 CALLX8,
  A2 $68 L L32R,  A2 A10 A2 AND,  A3 $6C L L32R,  A10 A3 A3 OR,  MEMW,
  A2 A5 0 S32I,  A8 $AC L L32R,  A8 CALLX8,
  A2 $70 L L32R,  A10 A10 A2 AND,  A2 $74 L L32R,  A10 A10 A2 OR,  MEMW,
  A10 A3 0 S32I.N,  RETW.N,
$257 L:  END-CODE

CODE bb_reg_init   \ .phyiram.9 $7C
  $0 31 LITERALS
    $3FF5CC48 T,  $170433AF T,  $3FF5CCE4 T,  $FFBFFFFF T,  $3FF5C400 T,
    $FFFF9F00 T,  $60A5 T,  $3FF5CD04 T,  $51510556 T,  $3FF5CD08 T,  $3FF5CC0C T,
    $1241FC T,  $3FF5CC08 T,  $90B4D20 T,  $3FF5CCDC T,  $195C83C T,  $3FF5C044 T,
    $FE00FFFF T,  $1FB0000 T,  $3FF5C024 T,  $FFFC01FF T,  $36800 T,  $3FF5C094 T,
    $1B8DD03 T,  $3FF5CC04 T,  $3FF5CCD8 T,  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
$7C L:
  A1 32 ENTRY,  A4 $8 L L32R,  A10 A4 MOV.N,  A3 $4 L L32R,  A2 $0 L L32R,  MEMW,
  A3 A2 0 S32I.N,  A8 $68 L L32R,  A8 CALLX8,
  A2 $C L L32R,  A2 A10 A2 AND,  A3 $10 L L32R,  A10 A3 MOV.N,  MEMW,  A2 A4 0 S32I,
  A8 $6C L L32R,  A8 CALLX8,
  A2 $14 L L32R,  A10 A10 A2 AND,  A2 $18 L L32R,  A10 A10 A2 OR,  MEMW,
  A10 A3 0 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A2 $20 L L32R,  A3 $1C L L32R,
  MEMW,  A2 A3 0 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A3 $24 L L32R,  MEMW,
  A2 A3 0 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A3 $2C L L32R,  A2 $28 L L32R,
  MEMW,  A3 A2 0 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A3 $34 L L32R,
  A2 $30 L L32R,  A4 $40 L L32R,  A10 A4 MOV.N,  MEMW,  A3 A2 0 S32I.N,  NOP.N,
  NOP.N,  NOP.N,  NOP.N,  A3 $3C L L32R,  A2 $38 L L32R,  MEMW,  A3 A2 0 S32I,
  A8 $70 L L32R,  A8 CALLX8,
  A2 $44 L L32R,  A10 A10 A2 AND,  A2 $48 L L32R,  A2 A10 A2 OR,  A3 $4C L L32R,
  A10 A3 MOV.N,  MEMW,  A2 A4 0 S32I.N,  A8 $74 L L32R,  A8 CALLX8,
  A8 $50 L L32R,  A10 A10 A8 AND,  A8 $54 L L32R,  A8 A10 A8 OR,  A2 $60 L L32R,
  A10 A2 MOV.N,  MEMW,  A8 A3 0 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,
  A4 $5C L L32R,  A3 $58 L L32R,  MEMW,  A4 A3 0 S32I.N,  A8 $78 L L32R,  A8 CALLX8,
  A8 -509 MOVI,  A10 A10 A8 AND,  A8 44 MOVI.N,  A10 A10 A8 OR,  MEMW,
  A10 A2 0 S32I.N,  A3 $186 MOVI,  NOP.N,  NOP.N,  NOP.N,  A2 $64 L L32R,  MEMW,
  A3 A2 0 S32I.N,  RETW.N,
$193 L:  END-CODE

CODE mac_enable_bb   \ .phyiram.10 $24
  $0 9 LITERALS
    $3FF00024 T,  $10000000 T,  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
$24 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A4 -3 MOVI.N,  A10 A2 A2 OR,  A8 $8 L L32R,
  A8 CALLX8,
  A3 $4 L L32R,  A3 A10 A3 OR,  A10 A2 MOV.N,  MEMW,  A3 A2 0 S32I.N,  A8 $C L L32R,
  A8 CALLX8,
  A3 A10 A4 AND,  A10 A2 A2 OR,  MEMW,  A3 A2 0 S32I,  A8 $10 L L32R,  A8 CALLX8,
  A3 2 MOVI.N,  A8 A10 A3 OR,  A10 A2 MOV.N,  MEMW,  A8 A2 0 S32I.N,  A8 $14 L L32R,
  A8 CALLX8,
  A8 A10 A4 AND,  A10 A2 MOV.N,  MEMW,  A8 A2 0 S32I.N,  A8 $18 L L32R,  A8 CALLX8,
  A8 A10 A3 OR,  A10 A2 MOV.N,  MEMW,  A8 A2 0 S32I.N,  A8 $1C L L32R,  A8 CALLX8,
  A4 A10 A4 AND,  A10 A2 MOV.N,  MEMW,  A4 A2 0 S32I.N,  A8 $20 L L32R,  A8 CALLX8,
  A10 A10 A3 OR,  MEMW,  A10 A2 0 S32I.N,  RETW.N,
$A6 L:  END-CODE

CODE bb_wdg_cfg   \ .phyiram.11 $14
  $0 5 LITERALS
    $3FF5D040 T,  $80000000 T,  $3FF5D03C T,  $401800AA T,
    SYMBOL esp_dport_access_reg_read
$14 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A10 A2 MOV.N,  A8 $10 L L32R,  A8 CALLX8,
  A8 $4 L L32R,  A10 A10 A8 OR,  MEMW,  A10 A2 0 S32I.N,  NOP.N,  NOP.N,  NOP.N,
  NOP.N,  A8 $C L L32R,  A2 $8 L L32R,  MEMW,  A8 A2 0 S32I.N,  RETW.N,
$42 L:  END-CODE

CODE rx_11b_opt   \ .phyiram.13 $6C
  $0 27 LITERALS
    $3FF5C044 T,  $1FF0000 T,  $FFFFC0FF T,  $2100 T,  $3FF5C124 T,  $FFFF03FF T,
    $8400 T,  $3FF5C804 T,  $FFFF0FFF T,  $9000 T,  $3FF5C104 T,  $FE00FFFF T,
    $1FB0000 T,  $1800 T,  $6000 T,  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read
$6C L:
  A1 32 ENTRY,  A2 A2 0 8 EXTUI,  A2 $115 L BEQZ,
  A3 $0 L L32R,  A10 A3 A3 OR,  A8 $3C L L32R,  A8 CALLX8,
  A2 $4 L L32R,  A2 A10 A2 OR,  A10 A3 MOV.N,  MEMW,  A2 A3 0 S32I,  A8 $40 L L32R,
  A8 CALLX8,
  A4 $8 L L32R,  A10 A10 A4 AND,  A2 $C L L32R,  A4 A10 A2 OR,  A2 $10 L L32R,
  A10 A2 MOV.N,  MEMW,  A4 A3 0 S32I.N,  A8 $44 L L32R,  A8 CALLX8,
  A3 $14 L L32R,  A4 A10 A3 AND,  A10 A2 MOV.N,  A3 $18 L L32R,  A3 A4 A3 OR,  MEMW,
  A3 A2 0 S32I.N,  A8 $48 L L32R,  A8 CALLX8,
  A3 -16 MOVI.N,  A10 A10 A3 AND,  A3 3 MOVI.N,  A4 $1C L L32R,  A3 A10 A3 OR,
  A10 A4 MOV.N,  MEMW,  A3 A2 0 S32I.N,  A8 $4C L L32R,  A8 CALLX8,
  A2 $20 L L32R,  A10 A10 A2 AND,  A2 $24 L L32R,  A2 A10 A2 OR,  A3 $28 L L32R,
  A10 A3 MOV.N,  MEMW,  A2 A4 0 S32I.N,  A8 $50 L L32R,  A8 CALLX8,
  A2 -512 MOVI,  A10 A10 A2 AND,  A2 $1E2 MOVI,  A10 A10 A2 OR,  MEMW,
  A10 A3 0 S32I.N,  RETW.N,
  2 ZEROS,
$115 L:
  A2 $0 L L32R,  A10 A2 MOV.N,  A8 $54 L L32R,  A8 CALLX8,
  A3 $2C L L32R,  A4 A10 A3 AND,  A10 A2 MOV.N,  A3 $30 L L32R,  A3 A4 A3 OR,  MEMW,
  A3 A2 0 S32I.N,  A8 $58 L L32R,  A8 CALLX8,
  A4 $8 L L32R,  A10 A10 A4 AND,  A3 $34 L L32R,  A4 A10 A3 OR,  A3 $10 L L32R,
  A10 A3 MOV.N,  MEMW,  A4 A2 0 S32I.N,  A8 $5C L L32R,  A8 CALLX8,
  A8 $14 L L32R,  A8 A10 A8 AND,  A10 A3 MOV.N,  A2 $38 L L32R,  A8 A8 A2 OR,  MEMW,
  A8 A3 0 S32I.N,  A8 $60 L L32R,  A8 CALLX8,
  A8 -16 MOVI.N,  A10 A10 A8 AND,  A8 4 MOVI.N,  A4 $1C L L32R,  A8 A10 A8 OR,
  A10 A4 MOV.N,  MEMW,  A8 A3 0 S32I.N,  A8 $64 L L32R,  A8 CALLX8,
  A3 $20 L L32R,  A10 A10 A3 AND,  A2 A10 A2 OR,  A3 $28 L L32R,  A10 A3 MOV.N,
  MEMW,  A2 A4 0 S32I.N,  A8 $68 L L32R,  A8 CALLX8,
  A2 -512 MOVI,  A10 A10 A2 AND,  A2 $1D2 MOVI,  A10 A10 A2 OR,  MEMW,
  A10 A3 0 S32I.N,  RETW.N,
$1B4 L:  END-CODE

CODE opt_11b_resart   \ .phyiram.15 $40
  $0 16 LITERALS
    $3FF5C0A0 T,  $FF00FFFF T,  $800000 T,  $3FF5C01C T,  $C0000 T,  $3FF5C094 T,
    $FFFF01FF T,  $DC00 T,  $3FF5C0F0 T,  $3FF5C0B8 T,  $800 T,
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read
$40 L:
  A1 32 ENTRY,  A5 $0 L L32R,  A10 A5 MOV.N,  A8 $2C L L32R,  A8 CALLX8,
  A2 $4 L L32R,  A10 A10 A2 AND,  A3 $8 L L32R,  A3 A10 A3 OR,  A4 $C L L32R,
  A10 A4 A4 OR,  MEMW,  A3 A5 0 S32I,  A8 $30 L L32R,  A8 CALLX8,
  A10 A10 A2 AND,  A2 $10 L L32R,  A2 A10 A2 OR,  A3 $14 L L32R,  A10 A3 MOV.N,
  MEMW,  A2 A4 0 S32I.N,  A8 $34 L L32R,  A8 CALLX8,
  A2 $18 L L32R,  A10 A10 A2 AND,  A2 $1C L L32R,  A2 A10 A2 OR,  A4 $20 L L32R,
  A10 A4 MOV.N,  MEMW,  A2 A3 0 S32I.N,  A8 $38 L L32R,  A8 CALLX8,
  A2 -256 MOVI,  A10 A10 A2 AND,  A2 128 MOVI,  A3 $24 L L32R,  A2 A10 A2 OR,
  A10 A3 MOV.N,  MEMW,  A2 A4 0 S32I.N,  A8 $3C L L32R,  A8 CALLX8,
  A2 $28 L L32R,  A10 A10 A2 OR,  MEMW,  A10 A3 0 S32I.N,  RETW.N,
$CA L:  END-CODE

CODE phy_reg_init   \ .phyiram.14 $5C
  $0 23 LITERALS
    SYMBOL tx_rf_ana_gain  SYMBOL chip7_sleep_params  $3FF5D040 T,  $80000000 T,
    $3FF5D03C T,  $401800AA T,  SYMBOL g_phyFuns  $3FF5C030 T,  $FFFC07FF T,
    $1A000 T,  $3FF5C11C T,  $E000 T,  $3FF5C120 T,  $1E101E1C T,
    SYMBOL tx_paon_set  SYMBOL bb_reg_init  SYMBOL agc_reg_init
    SYMBOL esp_dport_access_reg_read  SYMBOL mac_enable_bb
    SYMBOL phy_set_bbfreq_init  SYMBOL esp_dport_access_reg_read
    SYMBOL opt_11b_resart  SYMBOL rx_11b_opt
$5C L:
  A1 32 ENTRY,  A8 $4 L L32R,  A2 $0 L L32R,  A9 A8 188 L16UI,  A8 A8 190 L16UI,
  A9 A2 0 S16I,  A8 A2 2 S16I,  MEMW,  A8 $38 L L32R,  A8 CALLX8,
  A8 $3C L L32R,  A8 CALLX8,
  A8 $40 L L32R,  A8 CALLX8,
  A2 $8 L L32R,  A10 A2 MOV.N,  A8 $44 L L32R,  A8 CALLX8,
  A8 $C L L32R,  A10 A10 A8 OR,  MEMW,  A10 A2 0 S32I.N,  NOP.N,  NOP.N,  NOP.N,
  NOP.N,  A8 $14 L L32R,  A2 $10 L L32R,  MEMW,  A8 A2 0 S32I.N,  A8 $48 L L32R,
  A8 CALLX8,
  A10 1 MOVI.N,  A8 $4C L L32R,  A8 CALLX8,
  A2 $18 L L32R,  A8 A2 0 L32I,  A8 A8 $1A4 L32I,  A8 CALLX8,
  A2 A2 0 L32I.N,  A2 A2 120 L32I,  A2 CALLX8,
  A2 $1C L L32R,  A10 A2 MOV.N,  A8 $50 L L32R,  A8 CALLX8,
  A8 $20 L L32R,  A10 A10 A8 AND,  A8 $24 L L32R,  A10 A10 A8 OR,  MEMW,
  A10 A2 0 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A8 $2C L L32R,  A2 $28 L L32R,
  MEMW,  A8 A2 0 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A8 $34 L L32R,
  A2 $30 L L32R,  MEMW,  A8 A2 0 S32I.N,  A8 $54 L L32R,  A8 CALLX8,
  A10 1 MOVI.N,  A8 $58 L L32R,  A8 CALLX8,
  RETW.N,
$123 L:  END-CODE

CODE set_chan_reg   \ .phyiram.16 $70
  $0 28 LITERALS
    SYMBOL chip7_sleep_params  $3FF4E0C4 T,  $E7FFFFFF T,  $10000000 T,
    $FFF0FFFF T,  $FF0FFFFF T,  SYMBOL g_phyFuns  $40 SYMBOL+ chip7_sleep_params
    SYMBOL chip7_phy_init_ctrl  SYMBOL chan_to_freq
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL chan_to_freq  SYMBOL set_chan_rxcomp  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL bb_bss_cbw40
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL phy_freq_correct_opt
$70 L:
  A1 48 ENTRY,  A7 $0 L L32R,  A14 A7 81 L8UI,  A4 A14 7 SEXT,  A10 A4 MOV.N,
  A14 A1 8 S32I.N,  A8 $24 L L32R,  A8 CALLX8,
  A3 A10 MOV.N,  A3 A3 -96 ADDI,  A3 A3 0 8 EXTUI,  A2 $4 L L32R,  A3 A3 1 SLLI,
  A10 A2 A2 OR,  A8 $28 L L32R,  A8 CALLX8,
  A3 A3 0 8 EXTUI,  A5 $8 L L32R,  A6 A10 A5 AND,  A10 A2 MOV.N,  A5 $C L L32R,
  A5 A6 A5 OR,  MEMW,  A5 A2 0 S32I.N,  A8 $2C L L32R,  A8 CALLX8,
  A5 A10 16 4 EXTUI,  A10 A2 A2 OR,  A8 $30 L L32R,  A8 CALLX8,
  A6 A10 20 4 EXTUI,  A10 A2 A2 OR,  A8 $34 L L32R,  A8 CALLX8,
  A8 $10 L L32R,  A9 A10 A8 AND,  A10 A2 MOV.N,  MEMW,  A9 A2 0 S32I,
  A8 $38 L L32R,  A8 CALLX8,
  A8 $14 L L32R,  A9 A10 A8 AND,  A10 A2 MOV.N,  MEMW,  A9 A2 0 S32I,
  A8 $3C L L32R,  A8 CALLX8,
  A9 -257 MOVI,  A9 A10 A9 AND,  A10 A2 MOV.N,  MEMW,  A9 A2 0 S32I,  A8 $40 L L32R,
  A8 CALLX8,
  A9 -256 MOVI,  A10 A10 A9 AND,  A3 A3 A10 OR,  MEMW,  A3 A2 0 S32I.N,
  A10 A2 A2 OR,  A8 $44 L L32R,  A8 CALLX8,
  A9 $100 MOVI,  A11 A10 A9 OR,  A3 $18 L L32R,  A10 A4 MOV.N,  MEMW,
  A11 A2 0 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A11 A3 0 L32I,
  A13 A11 100 L32I,  A9 A1 4 S32I,  A13 A1 0 S32I,  A8 $48 L L32R,  A8 CALLX8,
  A11 A10 MOV.N,  A10 A4 MOV.N,  A12 A7 83 L8UI,  A12 A12 7 SEXT,  A13 A1 0 L32I,
  A13 CALLX8,
  A8 A3 0 L32I.N,  A10 A7 78 L16SI,  A8 A8 132 L32I,  A8 CALLX8,
  A10 0 MOVI.N,  A8 $4C L L32R,  A8 CALLX8,
  A3 A3 0 L32I.N,  A14 A1 8 L32I.N,  A11 A14 MOV.N,  A3 A3 $16C L32I,
  A10 $1C L L32R,  A3 CALLX8,
  A3 A2 MOV.N,  A9 A1 4 L32I.N,  A4 A9 MOV.N,
$186 L:
  A10 A3 MOV.N,  A8 $50 L L32R,  A8 CALLX8,
  A10 $202 L BGEZ,
$191 L:
  A10 A2 MOV.N,  A8 $54 L L32R,  A8 CALLX8,
  A3 -257 MOVI,  A4 A7 83 L8UI,  A3 A10 A3 AND,  A10 A4 7 SEXT,  A5 A5 16 SLLI,
  A6 A6 20 SLLI,  MEMW,  A3 A2 0 S32I.N,  A8 $58 L L32R,  A8 CALLX8,
  A10 A2 MOV.N,  A8 $5C L L32R,  A8 CALLX8,
  A3 $10 L L32R,  A10 A10 A3 AND,  A10 A10 A5 OR,  MEMW,  A10 A2 0 S32I.N,
  A10 A2 MOV.N,  A8 $60 L L32R,  A8 CALLX8,
  A5 $14 L L32R,  A10 A10 A5 AND,  A10 A10 A6 OR,  MEMW,  A10 A2 0 S32I,
  A10 A2 A2 OR,  A8 $64 L L32R,  A8 CALLX8,
  A8 $8 L L32R,  A10 A10 A8 AND,  A3 $20 L L32R,  A3 A3 78 L8UI,  MEMW,
  A10 A2 0 S32I.N,  A3 $219 L BEQZ.N,
  $213 L J,
$202 L:
  A10 A3 MOV.N,  A8 $68 L L32R,  A8 CALLX8,
  A10 A4 $210 L BNONE,
  $186 L J,
$210 L:
  $191 L J,
$213 L:
  A8 $6C L L32R,  A8 CALLX8,
$219 L:
  RETW.N,
$21B L:  END-CODE

CODE i2c_master_reset   \ .phyiram.18 $1C
  $0 7 LITERALS
    $3FF4E000 T,  $60033C00 T,  SYMBOL rodata.str1.4  $2000000 T,
    SYMBOL phy_printf  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read
$1C L:
  A1 32 ENTRY,  A6 0 MOVI.N,  A2 $0 L L32R,  A7 A6 MOV.N,  A4 $4 L L32R,
  A3 $C L L32R,
$2C L:
  MEMW,  A5 A4 0 L32I.N,  $54 L J,
$34 L:
  MEMW,  A8 A4 0 L32I,  A8 A8 A5 SUB,  A8 3 $54 L BLTUI,
  A10 $8 L L32R,  A11 A6 A6 OR,  A8 $10 L L32R,  A8 CALLX8,
  MEMW,  A7 A2 0 S32I.N,  $68 L J,
$54 L:
  A10 A2 MOV.N,  A8 $14 L L32R,  A8 CALLX8,
  A10 A3 $34 L BANY,
$5F L:
  A6 A6 1 ADDI.N,  A2 A2 4 ADDI.N,  A6 5 $2C L BNEI,
  RETW.N,
$68 L:
  A10 A2 MOV.N,  A8 $18 L L32R,  A8 CALLX8,
  A10 A3 $68 L BANY,
  $5F L J,
$76 L:  END-CODE

CODE ram_gen_rx_gain_table   \ .text.ram_gen_rx_gain_table $1C
  $0 7 LITERALS
    $64 SYMBOL+ rodata  $2AAAAAAB T,  $18 SYMBOL+ rodata.str1.4
    $48 SYMBOL+ rodata.str1.4  SYMBOL memcpy  SYMBOL phy_printf  SYMBOL phy_printf
$1C L:
  A1 112 ENTRY,  A9 A1 112 L8UI,  A8 A7 0 8 EXTUI,  A12 10 MOVI.N,  A10 A1 16 ADDI,
  A6 A6 0 8 EXTUI,  A11 $0 L L32R,  A7 0 MOVI.N,  A3 A3 0 8 EXTUI,  A8 A1 56 S32I.N,
  A2 A1 36 S32I.N,  A4 A1 44 S32I.N,  A4 A7 MOV.N,  A9 A1 32 S32I.N,  A8 $10 L L32R,
  A8 CALLX8,
  A8 A1 56 L32I.N,  A9 $4 L L32R,  A2 A8 A9 MULSH,  A6 A1 48 S32I.N,
  A6 A2 A2 ADDX2,  A6 A6 1 SLLI,  A6 A8 A6 SUB,  A2 A2 1 SLLI,  A9 A1 48 L32I.N,
  A8 A9 -1 ADDI.N,  A6 A1 60 S32I.N,  A6 A6 0 16 EXTUI,  A3 A1 40 S32I.N,
  A3 A7 MOV.N,  A6 A1 64 S32I,  A6 A5 MOV.N,  A5 A1 52 S32I.N,  A5 A8 MOV.N,
  A2 A1 68 S32I,
$74 L:
  A2 A7 0 8 EXTUI,  NOP.N,  NOP.N,  NOP.N,  A9 A6 0 L8UI,  A9 A9 7 SEXT,
  A4 A9 $89 L BNE,
  A3 A5 $A2 L BLT,
$89 L:
  A8 $4 L L32R,  A9 A4 A8 MULSH,  A15 A9 A9 ADDX2,  A15 A15 1 SLLI,  A15 A4 A15 SUB,
  A9 A9 1 SLLI,  A12 A15 0 16 EXTUI,  $DA L J,
  1 ZEROS,
$A2 L:
  A3 A3 1 ADDI.N,  A9 A1 52 L32I.N,  A3 A3 0 8 EXTUI,  A6 A9 A3 ADD.N,
  A4 A6 0 L8UI,  A4 $CC L BNEZ.N,
  A3 A5 $B6 L BLT,
  $16C L J,
$B6 L:
  A8 A9 MOV.N,
$B8 L:
  A3 A3 1 ADDI.N,  A3 A3 0 8 EXTUI,  A6 A8 A3 ADD.N,  A4 A6 0 L8UI,
  A4 $CC L BNEZ.N,
  A3 A5 $B8 L BLT,
  $16C L J,
  2 ZEROS,
$CC L:
  A3 A5 $D2 L BLT,
  $16C L J,
$D2 L:
  A12 0 MOVI.N,  A15 A12 MOV.N,  A4 A12 MOV.N,  A9 A12 MOV.N,
$DA L:
  A8 A1 16 ADDI,  A9 A8 A9 ADD.N,  A14 A9 0 L16UI,  A14 A14 3 SLLI,
  A14 A14 0 16 EXTUI,  A12 A14 A12 ADD.N,  A9 A1 44 L32I.N,  A13 A9 A3 ADD.N,
  A9 A13 0 L8UI,  A9 A9 8 SLLI,  A12 A9 A12 ADD.N,  A9 A2 7 SEXT,  A8 A1 36 L32I.N,
  A9 A9 1 SRAI,  A12 A12 0 16 EXTUI,  A9 A9 A8 ADDX4,  A2 0 $114 L BBCI,
  A11 A9 0 L32I.N,  A10 A12 16 SLLI,  A10 A11 A10 ADD.N,  A10 A9 0 S32I.N,
  $116 L J,
  1 ZEROS,
$114 L:
  A12 A9 0 S32I.N,
$116 L:
  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A9 A1 32 L32I.N,  A9 $13E L BEQZ.N,
  A11 A7 MOV.N,  A9 A6 0 L8UI,  A9 A9 7 SEXT,  A8 A1 48 L32I.N,  A13 A13 0 L8UI,
  A10 $8 L L32R,  A9 A1 8 S32I.N,  A3 A1 4 S32I.N,  A8 A1 0 S32I.N,  A8 $14 L L32R,
  A8 CALLX8,
$13E L:
  A9 A1 40 L32I.N,  A9 A4 $156 L BGEU,
  A4 A1 32 L32I,  A4 $179 L BEQZ.N,
  A11 A7 0 8 EXTUI,  A10 $C L L32R,  A8 $18 L L32R,  A8 CALLX8,
  RETW.N,
$156 L:
  A4 A4 1 ADDI.N,  A7 A7 1 ADDI.N,  A2 127 MOVI,  A4 A4 0 8 EXTUI,
  A7 A2 $166 L BEQ,
  $74 L J,
$166 L:
  A2 85 MOVI.N,  RETW.N,
  2 ZEROS,
$16C L:
  A12 A1 64 L32I,  A15 A1 60 L32I.N,  A4 A1 56 L32I.N,  A9 A1 68 L32I,  $DA L J,
$179 L:
  RETW.N,
$17B L:  END-CODE

CODE set_rx_gain_cal_iq   \ .text.set_rx_gain_cal_iq $78
  $0 30 LITERALS
    $70 SYMBOL+ rodata  $78 SYMBOL+ rodata  SYMBOL g_phyFuns  $3FF450DC T,
    $8000000 T,  $EFFFFFFF T,  $FFFFEFFF T,  SYMBOL chip7_sleep_params  $FA0 T,
    $3FF460E4 T,  $3FF460DC T,  $3FF460E0 T,  $20000 T,  $1000 T,
    $58 SYMBOL+ rodata.str1.4  $FFF T,  $A0 SYMBOL+ rodata.str1.4  $10000000 T,
    SYMBOL memcpy  SYMBOL memcpy  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL phy_printf  SYMBOL phy_printf
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
$78 L:
  A1 128 ENTRY,  A11 $0 L L32R,  A12 5 MOVI,  A10 A1 34 ADDI,  A8 $48 L L32R,
  A8 CALLX8,
  A12 10 MOVI.N,  A11 $4 L L32R,  A10 A1 16 ADDI,  A8 $4C L L32R,  A8 CALLX8,
  A2 A2 0 8 EXTUI,  A10 1 MOVI.N,  A6 $8 L L32R,  A3 A3 15 SEXT,  A5 A5 0 8 EXTUI,
  A2 A1 76 S32I,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A2 A6 0 L32I.N,  A2 A2 40 L32I.N,
  A3 A1 68 S32I,  A5 A1 60 S32I.N,  A2 CALLX8,
  A14 7 MOVI.N,  A15 1 MOVI.N,  A13 A14 MOV.N,  A2 A6 0 L32I,  A12 4 MOVI.N,
  A11 0 MOVI,  A10 100 MOVI,  A2 A2 168 L32I,  A2 CALLX8,
  A10 $C L L32R,  A8 $50 L L32R,  A8 CALLX8,
  A2 $10 L L32R,  A2 A10 A2 OR,  A10 $C L L32R,  A3 A10 A10 OR,  MEMW,
  A2 A10 0 S32I,  A8 $54 L L32R,  A8 CALLX8,
  A2 $14 L L32R,  A2 A10 A2 AND,  A10 A3 MOV.N,  MEMW,  A2 A3 0 S32I,
  A8 $58 L L32R,  A8 CALLX8,
  A2 $18 L L32R,  A3 A10 A2 AND,  A2 $1C L L32R,  A2 A2 76 L16UI,  A5 A2 6 5 EXTUI,
  A10 A5 7 SEXT,  A8 $C L L32R,  MEMW,  A3 A8 0 S32I.N,  A3 15 MOVI,
  A3 A10 $12C L BGE,
  A10 A5 -32 ADDI,
$12C L:
  A2 A2 0 6 EXTUI,  A3 A2 7 SEXT,  A5 31 MOVI.N,  A5 A3 $13A L BGE,
  A3 A2 -64 ADDI,
$13A L:
  A2 A6 0 L32I.N,  A9 A1 33 ADDI,  A11 1 MOVI.N,  A2 A2 $128 L32I,  A9 A1 56 S32I,
  A2 CALLX8,
  A10 A3 MOV.N,  A11 0 MOVI.N,  A3 0 MOVI.N,  A2 A6 0 L32I.N,  A2 A2 $128 L32I,
  A2 CALLX8,
  A2 $100 MOVI,  A3 A1 48 S32I.N,  A4 A1 64 S32I,  A2 A1 26 S16I,  A2 A1 28 S16I,
  MEMW,
$169 L:
  A5 A1 16 ADDI,  A8 128 MOVI,  A11 $104 MOVI,  NOP.N,  A4 A1 48 L32I.N,
  A2 A4 A5 ADDX2,  A2 A2 0 L16UI,  A2 A2 A8 ADD.N,  A2 A2 0 16 EXTUI,  A12 A2 MOV.N,
  A9 A1 56 L32I.N,  A2 A1 52 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,
  A2 A6 0 L32I.N,  A4 A9 1 L8UI,  A10 A4 A4 OR,  A4 A4 0 16 EXTUI,  A2 A2 32 L32I,
  A2 CALLX8,
  A2 A1 76 L32I,  A2 $1C4 L BEQZ.N,
  A11 1 MOVI.N,  A10 A11 MOV.N,  A2 A6 0 L32I,  A3 A2 188 L32I,  A2 A2 184 L32I,
  A3 CALLX8,
  A3 2 MOVI.N,  A10 A10 A3 OR,  A11 1 MOVI.N,  A12 A10 0 16 EXTUI,  A10 A11 MOV.N,
  A2 CALLX8,
$1C4 L:
  A2 A6 0 L32I.N,  A14 0 MOVI.N,  A13 A14 MOV.N,  A12 10 MOVI.N,  A11 A1 26 ADDI,
  A2 A2 $150 L32I,  A3 0 MOVI.N,  A10 $20 L L32R,  A2 CALLX8,
  A9 A3 MOV.N,  A2 24 MOVI.N,  A8 A1 48 L32I,  A5 A8 0 8 EXTUI,  A8 1 MOVI.N,
  A9 A8 A5 MOVNEZ,  A9 A1 72 S32I,
$1EC L:
  A11 1 MOVI.N,  A10 A11 MOV.N,  A12 $1F1 MOVI,  A5 A2 0 8 EXTUI,  A7 A6 0 L32I.N,
  A7 A7 184 L32I,  A7 CALLX8,
  A11 1 MOVI.N,  A10 A11 MOV.N,  A7 A6 0 L32I.N,  A12 $1F9 MOVI,  A7 A7 184 L32I,
  A7 CALLX8,
  A15 0 MOVI.N,  A14 A15 MOV.N,  A7 A6 0 L32I.N,  A13 A15 A15 OR,  A12 A5 A5 OR,
  A10 1 MOVI.N,  A11 A1 68 L32I,  A7 A7 68 L32I,  A7 CALLX8,
  A11 $3FF MOVI,  A10 1 MOVI.N,  A7 A6 0 L32I.N,  A7 A7 $144 L32I,  A7 CALLX8,
  A10 $24 L L32R,  A8 $5C L L32R,  A8 CALLX8,
  A7 A10 7 SRAI,  A9 A1 60 L32I.N,  A9 $282 L BEQZ.N,
  A10 $28 L L32R,  A8 $60 L L32R,  A8 CALLX8,
  A8 A10 MOV.N,  A10 $2C L L32R,  A8 A1 80 S32I,  A8 $64 L L32R,  A8 CALLX8,
  A10 A10 16 SRAI,  A15 A2 MOV.N,  A14 A4 MOV.N,  A8 A1 80 L32I,  A11 A7 MOV.N,
  A8 A8 16 SRAI,  A10 A1 4 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A13 $30 L L32R,
  A12 $34 L L32R,  A10 $38 L L32R,  A8 A1 0 S32I.N,  A8 $68 L L32R,  A8 CALLX8,
$282 L:
  A8 A6 0 L32I.N,  A8 A8 $148 L32I,  A8 CALLX8,
  A10 1 MOVI.N,  A8 A6 0 L32I.N,  A8 A8 76 L32I,  A8 CALLX8,
  A8 $30 L L32R,  A8 A7 $2D0 L BGE,
  A3 0 $2B2 L BBSI,
  A4 $2B2 L BEQZ.N,
  A5 120 MOVI,  A5 A2 A5 MIN,  A9 A1 56 L32I.N,  A5 A5 15 SEXT,  A2 A5 MOV.N,
  A4 A9 2 L8UI,  $3AA L J,
$2B2 L:
  A5 A6 0 L32I.N,  A2 A2 24 ADDI,  A12 A4 MOV.N,  A11 1 MOVI.N,  A10 5 MOVI.N,
  A5 A5 184 L32I,  A7 A2 15 SEXT,  A2 0 MOVI.N,  A5 CALLX8,
  A5 A2 MOV.N,  A7 A2 $2FE L BLT,
  $2F6 L J,
$2D0 L:
  A8 $3C L L32R,  A8 A7 $309 L BLT,
  A3 0 $2DC L BBCI,
  $3A6 L J,
$2DC L:
  A5 A1 72 L32I,  A5 $3A6 L BEQZ,
  A5 120 MOVI,  A5 A2 A5 MIN,  A8 A1 56 L32I.N,  A5 A5 15 SEXT,  A2 A5 MOV.N,
  A4 A8 0 L8UI,  $3AA L J,
  1 ZEROS,
$2F6 L:
  A2 120 MOVI,  A2 A7 A2 MIN,  A5 A2 MOV.N,
$2FE L:
  A3 A3 1 ADDI.N,  A3 A3 0 8 EXTUI,  A3 2 $309 L BEQI,
  $1EC L J,
$309 L:
  A9 A1 60 L32I.N,  A9 $326 L BEQZ.N,
  A13 A2 MOV.N,  A2 2 MOVI.N,  A15 A1 48 L32I.N,  A12 $104 MOVI,  A11 A4 MOV.N,
  A14 A1 52 L32I.N,  A10 $40 L L32R,  A2 A1 0 S32I,  A8 $6C L L32R,  A8 CALLX8,
$326 L:
  A11 A5 MOV.N,  A2 A6 0 L32I.N,  A3 A1 48 L32I.N,  A3 A3 1 ADDI.N,
  A4 A1 56 L32I.N,  A4 A4 1 ADDI.N,  A12 A1 60 L32I.N,  A10 A1 68 L32I,
  A2 A2 $164 L32I,  A3 A1 48 S32I.N,  A4 A1 56 S32I.N,  A2 CALLX8,
  A5 A1 64 L32I,  A10 A5 0 S16I,  A5 A5 2 ADDI.N,  A5 A1 64 S32I,  A3 4 $352 L BEQI,
  $169 L J,
$352 L:
  A10 0 MOVI.N,  NOP.N,  NOP.N,  NOP.N,  A2 A6 0 L32I.N,  A2 A2 40 L32I,  A2 CALLX8,
  A15 0 MOVI.N,  A14 7 MOVI.N,  A2 A6 0 L32I.N,  A13 A14 MOV.N,  A11 A15 MOV.N,
  A10 100 MOVI,  A12 A1 48 L32I,  A2 A2 168 L32I,  A2 CALLX8,
  A10 $C L L32R,  A8 $70 L L32R,  A8 CALLX8,
  A2 $44 L L32R,  A2 A10 A2 OR,  A10 $C L L32R,  MEMW,  A2 A10 0 S32I.N,
  A8 $74 L L32R,  A8 CALLX8,
  A9 $34 L L32R,  A10 A10 A9 OR,  A2 $C L L32R,  MEMW,  A10 A2 0 S32I.N,  RETW.N,
  1 ZEROS,
$3A6 L:
  A5 0 MOVI.N,  A2 A5 MOV.N,
$3AA L:
  A7 A6 0 L32I.N,  A12 A4 MOV.N,  A11 1 MOVI.N,  A10 5 MOVI.N,  A7 A7 184 L32I,
  A7 CALLX8,
  $2FE L J,
$3BB L:  END-CODE

CODE rx_chan_dc_sort   \ .text.rx_chan_dc_sort $4
  $0 1 LITERALS
    SYMBOL g_phyFuns
$4 L:
  A1 64 ENTRY,  A2 A1 12 S32I.N,  A4 A3 MOV.N,  A8 0 MOVI.N,  A2 14 MOVI.N,
  A3 A1 4 S32I.N,  A2 $2B L LOOP,
  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A3 A4 0 L8UI,  A4 A4 1 ADDI.N,  A3 1 $29 L BNEI,
  A8 A8 1 ADDI.N,  A8 A8 0 8 EXTUI,
$29 L:
  NOP.N,
$2B L:
  A2 1 MOVI.N,  A5 2 MOVI.N,  A14 A1 12 L32I.N,  A5 A2 A8 MOVNEZ,  A7 0 MOVI.N,
  A12 A7 MOV.N,  A3 14 MOVI.N,  A7 A5 MOV.N,  A5 A14 MOV.N,
$3E L:
  A4 A1 4 L32I.N,  A9 A12 0 8 EXTUI,  A2 A4 A12 ADD.N,  A2 A2 0 L8UI,
  A9 A1 0 S32I.N,  A2 1 $AD L BEQI,
  A6 20 MOVI.N,  A2 0 MOVI.N,  A5 A1 8 S32I.N,  A5 A6 MOV.N,
$55 L:
  A6 A2 0 8 EXTUI,  NOP.N,  NOP,  A8 A4 0 L8UI,  A8 A7 $86 L BNE,
  A10 A12 A2 SUB,  A9 $0 L L32R,  A8 A9 0 L32I,  A8 A8 $140 L32I,  A12 A1 16 S32I.N,
  A8 CALLX8,
  A10 A10 0 8 EXTUI,  A8 A5 7 SEXT,  A9 A10 7 SEXT,  A12 A1 16 L32I.N,
  A9 A8 $86 L BGE,
  A5 A10 MOV.N,  A6 A1 0 S32I.N,
$86 L:
  A2 A2 1 ADDI.N,  A4 A4 1 ADDI.N,  A2 A3 $55 L BNE,
  NOP.N,  NOP.N,  A2 A1 0 L32I.N,  A6 A2 7 SEXT,  A4 A1 12 L32I.N,  A6 A6 A4 ADDX4,
  A2 A6 0 L32I.N,  A4 A2 16 SRAI,  A4 A4 16 SLLI,  A2 A2 0 9 EXTUI,  A5 A1 8 L32I.N,
  A2 A4 A2 OR,  A2 A5 0 S32I.N,
$AD L:
  A12 A12 1 ADDI.N,  A5 A5 4 ADDI.N,  A12 A3 $3E L BNE,
  RETW.N,
$B6 L:  END-CODE

CODE set_rx_gain_cal_dc   \ .text.set_rx_gain_cal_dc $38
  $0 14 LITERALS
    $84 SYMBOL+ rodata  $64 SYMBOL+ rodata  SYMBOL g_phyFuns  $800 T,
    SYMBOL memcpy  SYMBOL chip_v7_set_chan_ana  SYMBOL chip_v7_set_chan_ana
    SYMBOL get_rfrx_dcap_bt  SYMBOL get_rfrx_dcap_bt  SYMBOL pbus_rx_dco_cal_1step
    SYMBOL pbus_rx_dco_cal_1step  SYMBOL pbus_rx_dco_cal_1step
    SYMBOL rx_chan_dc_sort  SYMBOL chip_v7_set_chan_ana
$38 L:
  A1 144 ENTRY,  A7 A1 76 S32I,  A2 A2 0 8 EXTUI,  A12 10 MOVI.N,  A4 A4 0 8 EXTUI,
  A10 A1 28 ADDI,  A7 $0 L L32R,  A8 A7 0 L16UI,  A11 $4 L L32R,  A6 A1 96 S32I,
  A6 A3 0 8 EXTUI,  A2 A1 48 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A3 A7 2 L16UI,
  A2 A1 148 L8UI,  A8 A1 38 S16I,  A2 A1 100 S32I,  A5 A1 72 S32I,  A4 A1 88 S32I,
  A3 A1 40 S16I,  MEMW,  A8 $10 L L32R,  A8 CALLX8,
  A10 1 MOVI.N,  A7 $8 L L32R,  A2 A7 0 L32I.N,  A2 A2 64 L32I,  A2 CALLX8,
  A10 1 MOVI.N,  A2 A7 0 L32I.N,  A2 A2 60 L32I.N,  A2 CALLX8,
  A10 14 MOVI.N,  A8 $14 L L32R,  A8 CALLX8,
  A6 A4 $A9 L BLTU,
$A1 L:
  A3 A1 48 L32I.N,  A3 $368 L BNEZ,
  $347 L J,
$A9 L:
  A4 A1 48 L32I.N,  A2 0 MOVI.N,  A3 9 MOVI.N,  A3 A2 A4 MOVNEZ,  A3 A3 0 8 EXTUI,
  A5 A1 72 L32I,  A8 A1 100 L32I,  A2 A5 A8 ADD.N,  A2 A2 -1 ADDI.N,  A3 A1 92 S32I,
  A2 A1 104 S32I,
$C5 L:
  A6 1 $D4 L BEQI,
  A6 $376 L BEQZ,
  A6 2 $D1 L BEQI,
  $39D L J,
$D1 L:
  $3CC L J,
$D4 L:
  A5 A1 96 L32I,  A11 A6 MOV.N,  A10 2 MOVI.N,  A2 A7 0 L32I.N,  A12 A5 42 L16UI,
  A2 A2 184 L32I,  A2 CALLX8,
  A11 A6 MOV.N,  A10 3 MOVI.N,  A2 A7 0 L32I.N,  A12 A5 40 L16UI,  A2 A2 184 L32I,
  A2 CALLX8,
  A8 4 MOVI.N,  A8 A1 64 S32I,
$FA L:
  A3 $100 MOVI,  A2 1 MOVI.N,  A2 A1 68 S32I,  A3 A1 38 S16I,  A3 A1 40 S16I,  MEMW,
  $124 L J,
$10E L:
  A5 A1 96 L32I,  A8 1 MOVI.N,  A2 A5 40 L32I.N,  A3 A2 16 16 EXTUI,  A8 A1 64 S32I,
  A3 A1 38 S16I,  A2 A1 40 S16I,  MEMW,
$124 L:
  A5 1 MOVI.N,  NOP.N,  NOP.N,  NOP.N,  A2 A1 64 L32I,  A4 A2 -1 ADDI.N,
  A4 A4 0 8 EXTUI,  A4 A4 1 ADDI.N,  A8 A1 72 L32I,  A2 0 MOVI.N,  A4 A8 A4 ADD.N,
  A3 A1 92 L32I,  A5 A1 56 S32I,  A3 A1 80 S32I,  A2 A1 84 S32I,  A4 A1 52 S32I,
$14C L:
  A6 2 $161 L BEQI,
  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A3 A1 56 L32I.N,  A3 A3 0 8 EXTUI,
  A3 A1 60 S32I.N,  $170 L J,
$161 L:
  A5 A1 56 L32I.N,  A4 A5 0 8 EXTUI,  A10 A5 MOV.N,  A8 $18 L L32R,  A8 CALLX8,
  A4 A1 60 S32I.N,
$170 L:
  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A8 A1 48 L32I.N,  A8 $1B0 L BEQZ.N,
  A10 4 MOVI.N,  A4 A7 0 L32I.N,  A4 A4 160 L32I,  A8 $1C L L32R,  A8 CALLX8,
  A13 A10 MOV.N,  A12 4 MOVI.N,  A11 0 MOVI.N,  A10 100 MOVI,  A4 CALLX8,
  A4 A7 0 L32I.N,  A10 7 MOVI.N,  A4 A4 160 L32I,  A8 $20 L L32R,  A8 CALLX8,
  A13 A10 MOV.N,  A12 7 MOVI,  A11 0 MOVI,  A10 100 MOVI,  A4 CALLX8,
$1B0 L:
  A3 A1 64 L32I,  A3 $31E L BEQZ,
  A4 A1 28 ADDI,  A5 A1 72 L32I,  A3 A5 MOV.N,  A5 A4 MOV.N,  A4 A3 MOV.N,
$1C2 L:
  A11 1 MOVI.N,  A12 $184 MOVI,  A10 0 MOVI.N,  A6 A11 $236 L BNE,
  A8 A1 72 L32I,  A11 A6 A6 OR,  A15 A8 1 L8UI,  A15 A15 8 SLLI,  A13 A5 0 L16UI,
  A13 A13 3 SLLI,  A13 A15 A13 OR,  A3 A13 0 16 EXTUI,  A14 A7 0 L32I.N,
  A14 A14 184 L32I,  A14 CALLX8,
  A10 A3 MOV.N,  A11 A7 0 L32I.N,  A11 A11 200 L32I,  A11 CALLX8,
  A11 A6 MOV.N,  A15 A1 A2 ADD.N,  A12 $C L L32R,  A14 A1 16 ADDI,  A13 A1 38 ADDI,
  A10 A1 48 L32I,  A8 $24 L L32R,  A8 CALLX8,
  A10 A1 38 L16SI,  A11 A10 16 SLLI,  A3 A1 84 L32I,  A10 A3 1 ADDI.N,
  A10 A10 0 8 EXTUI,  A8 A1 76 L32I,  A12 A3 A8 ADDX4,  A10 A1 84 S32I,  NOP.N,
  NOP.N,  NOP.N,  NOP.N,  A10 A1 40 L16SI,  A10 A11 A10 OR,  A10 A12 0 S32I.N,
  $30E L J,
$236 L:
  A6 2 $29C L BNEI,
  A13 A7 0 L32I.N,  A12 $184 MOVI,  A11 1 MOVI.N,  A10 0 MOVI,  A3 A1 104 L32I,
  A14 A13 184 L32I,  A13 A3 0 L8UI,  A3 A13 8 SLLI,  A14 CALLX8,
  A10 A3 MOV.N,  A11 A7 0 L32I.N,  A11 A11 200 L32I,  A11 CALLX8,
  A11 A6 MOV.N,  A10 A6 MOV.N,  A8 A1 76 L32I,  A13 A7 0 L32I.N,  A12 A8 6 L16UI,
  A13 A13 184 L32I,  A13 CALLX8,
  A11 A6 MOV.N,  A10 3 MOVI.N,  A8 A1 76 L32I,  A13 A7 0 L32I.N,  A12 A8 4 L32I.N,
  A12 A12 0 9 EXTUI,  A13 A13 184 L32I,  A13 CALLX8,
  A15 A1 A2 ADD.N,  A14 A1 16 ADDI,  A12 $C L L32R,  A13 A1 38 ADDI,  A11 A6 MOV.N,
  A10 A1 48 L32I,  A8 $28 L L32R,  A8 CALLX8,
  $2F5 L J,
  1 ZEROS,
$29C L:
  A14 A7 0 L32I.N,  A13 A4 0 L8UI,  A3 A13 8 SLLI,  A14 A14 184 L32I,  A14 CALLX8,
  A10 A3 MOV.N,  A11 A7 0 L32I.N,  A11 A11 200 L32I,  A11 CALLX8,
  A15 A1 A2 ADD.N,  A14 A1 16 ADDI,  A12 $C L L32R,  A13 A1 38 ADDI,  A11 A6 MOV.N,
  A10 A1 48 L32I.N,  A8 $2C L L32R,  A8 CALLX8,
  A6 $2F5 L BNEZ.N,
  A3 A1 80 L32I,  A10 A1 38 L16SI,  A11 A10 16 SLLI,  A10 A3 1 ADDI.N,
  A10 A10 0 8 EXTUI,  A8 A1 96 L32I,  A12 A3 A8 ADDX4,  A10 A1 80 S32I,  NOP.N,
  NOP.N,  NOP.N,  NOP.N,  A10 A1 40 L16SI,  A10 A11 A10 OR,  A10 A12 0 S32I.N,
  $30E L J,
$2F5 L:
  A10 A1 38 L16SI,  A11 A10 16 SLLI,  A3 A1 144 L32I,  A12 A2 A3 ADDX4,
  A2 A2 1 ADDI.N,  A2 A2 0 8 EXTUI,  A10 A1 40 L16SI,  A10 A11 A10 OR,
  A10 A12 0 S32I.N,
$30E L:
  A4 A4 1 ADDI.N,  A5 A5 2 ADDI.N,  NOP.N,  NOP.N,  A8 A1 52 L32I.N,
  A8 A4 $31E L BEQ,
  $1C2 L J,
$31E L:
  A3 A1 56 L32I.N,  A3 A3 1 ADDI.N,  A4 A1 60 L32I.N,  A5 A1 68 L32I,
  A3 A1 56 S32I.N,  A4 A5 $32F L BGEU,
  $14C L J,
$32F L:
  A6 A6 1 ADDI.N,  NOP.N,  NOP.N,  NOP.N,  A8 A1 88 L32I,  A6 A6 0 8 EXTUI,
  A6 A8 $343 L BGEU,
  $C5 L J,
$343 L:
  $A1 L J,
  1 ZEROS,
$347 L:
  A10 A1 144 L32I,  A11 A1 MOV.N,  A8 $30 L L32R,  A8 CALLX8,
$352 L:
  A10 0 MOVI.N,  A2 A7 0 L32I.N,  A2 A2 64 L32I,  A2 CALLX8,
  A10 0 MOVI.N,  A2 A7 0 L32I.N,  A2 A2 60 L32I.N,  A2 CALLX8,
  RETW.N,
  1 ZEROS,
$368 L:
  A10 14 MOVI.N,  A8 $34 L L32R,  A8 CALLX8,
  $352 L J,
  3 ZEROS,
$376 L:
  A2 A7 0 L32I.N,  A11 2 MOVI.N,  A10 A11 MOV.N,  A12 $100 MOVI,  A2 A2 184 L32I,
  A2 CALLX8,
  A12 $100 MOVI,  A11 2 MOVI.N,  A2 A7 0 L32I.N,  A10 3 MOVI.N,  A2 A2 184 L32I,
  A2 CALLX8,
  A2 A1 100 L32I,  A2 A1 64 S32I,  $FA L J,
$39D L:
  A8 A1 76 L32I,  A11 2 MOVI.N,  A10 A11 MOV.N,  A2 A7 0 L32I,  A12 A8 6 L16UI,
  A2 A2 184 L32I,  A2 CALLX8,
  A11 2 MOVI.N,  A10 3 MOVI.N,  A8 A1 76 L32I,  A2 A7 0 L32I.N,  A12 A8 4 L32I.N,
  A12 A12 0 9 EXTUI,  A2 A2 184 L32I,  A2 CALLX8,
  A2 1 MOVI.N,  A2 A1 68 S32I,  $10E L J,
$3CC L:
  A8 A1 76 L32I,  A11 A6 MOV.N,  A10 A6 MOV.N,  A2 A7 0 L32I.N,  A12 A8 6 L16UI,
  A2 A2 184 L32I,  A2 CALLX8,
  A11 A6 MOV.N,  A10 3 MOVI.N,  A8 A1 76 L32I,  A2 A7 0 L32I.N,  A12 A8 4 L32I.N,
  A12 A12 0 9 EXTUI,  A2 A2 184 L32I,  A2 CALLX8,
  A2 14 MOVI.N,  A2 A1 68 S32I,  $10E L J,
$3FA L:  END-CODE

CODE wr_rx_gain_mem   \ .text.wr_rx_gain_mem $1C
  $0 7 LITERALS
    $200000 T,  $64 SYMBOL+ rodata  SYMBOL wifi_bbgain_max  $AAAAAAAB T,
    SYMBOL chip7_sleep_params  SYMBOL g_phyFuns  SYMBOL memcpy
$1C L:
  A1 96 ENTRY,  A2 A2 0 8 EXTUI,  A2 A1 16 S32I.N,  A12 10 MOVI.N,  A10 A1 MOV.N,
  A3 A3 0 8 EXTUI,  NOP.N,  A2 A1 96 L8UI,  A8 A1 16 L32I.N,  A7 A1 44 S32I.N,
  A7 0 MOVI.N,  A2 A1 20 S32I.N,  A2 128 MOVI,  A2 A7 A8 MOVEQZ,  NOP.N,  NOP.N,
  A11 $4 L L32R,  A6 A1 40 S32I.N,  A2 A1 28 S32I.N,  A4 A1 32 S32I.N,
  A5 A1 36 S32I,  A8 $18 L L32R,  A8 CALLX8,
  A2 $8 L L32R,  A9 A1 20 L32I.N,  A2 A2 0 L8UI,  A2 A9 A2 SUB,  A2 A2 -2 ADDI,
  A2 A2 0 8 EXTUI,  A6 A2 MOV.N,  A2 A1 24 S32I.N,  A6 A7 A3 MOVEQZ,
  A6 A9 $72 L BLTU,
  $1ED L J,
$72 L:
  A4 1 MOVI.N,  NOP.N,  NOP.N,  A2 A1 16 L32I.N,  A4 A7 A2 MOVEQZ,  A4 A4 0 8 EXTUI,
  A2 A7 MOV.N,  A5 $14 L L32R,  A4 A1 48 S32I.N,  A4 A7 MOV.N,
$89 L:
  A3 $BD L BEQZ.N,
  NOP.N,  NOP.N,  NOP.N,  A8 $C L L32R,  A7 A2 A8 MULUH,  A7 A7 2 SRLI,
  A8 A7 A1 ADDX2,  A7 A7 A7 ADDX2,  A8 A8 0 L8UI,  A7 A7 1 SLLI,  A7 A2 A7 SUB,
  A7 A8 A7 ADDX8,  A7 A7 0 8 EXTUI,  A9 A1 32 L32I.N,  A8 A9 0 L8UI,  A8 A8 8 SLLI,
  A8 A7 A8 OR,  $100 L J,
$BD L:
  A2 A6 1 SRLI,  A6 0 $D0 L BBCI,
  A8 A1 100 L32I,  A2 A2 A8 ADDX4,  A8 A2 2 L16UI,  $D9 L J,
  1 ZEROS,
$D0 L:
  A9 A1 100 L32I,  A2 A2 A9 ADDX4,  A8 A2 0 L16UI,
$D9 L:
  A10 3 MOVI.N,  A2 0 MOVI,  A9 6 MOVI,  A9 $F4 L LOOP,
  A10 SSR,  A7 A8 SRA,  A7 A7 0 1 EXTUI,  A2 A2 A7 ADD.N,  A2 A2 0 8 EXTUI,
  A10 A10 1 ADDI.N,
$F4 L:
  A2 A2 A2 ADDX2,  A7 A8 0 3 EXTUI,  A2 A2 A7 ADDX2,  A2 A2 0 8 EXTUI,
$100 L:
  A9 A1 24 L32I.N,  A6 A9 $109 L BLTU,
  A9 A1 48 L32I.N,  A9 $13E L BEQZ.N,
$109 L:
  A9 A1 32 L32I.N,  A7 A9 A4 ADD.N,  A9 -256 MOVI,  A9 A8 A9 AND,  A7 A7 0 L8UI,
  A7 A7 8 SLLI,  A9 A7 $121 L BEQ,
  A4 A4 1 ADDI.N,  A4 A4 0 8 EXTUI,
$121 L:
  A9 A1 36 L32I.N,  A14 0 MOVI.N,  A7 A4 A9 ADDX4,  A15 128 MOVI,  A7 A7 0 L32I.N,
  A9 A7 16 16 EXTUI,  A7 A7 0 16 EXTUI,  A10 A1 16 L32I.N,  A13 $0 L L32R,
  A10 A14 $17D L BNE,
  $159 L J,
$13E L:
  A7 $10 L L32R,  A7 A7 81 L8UI,  A7 A7 7 SEXT,  A7 A7 -1 ADDI.N,  A7 A7 0 16 EXTUI,
  A9 A1 44 L32I.N,  A7 A7 A9 ADDX4,  A7 A7 0 L32I.N,  A9 A7 16 16 EXTUI,
  A7 A7 0 16 EXTUI,
$159 L:
  A11 $C L L32R,  A10 A2 A11 MULUH,  A11 A10 2 SRLI,  A10 3 MOVI.N,
  A10 A11 A10 MINU,  A11 A1 40 L32I.N,  A10 A10 A11 ADDX4,  A13 A10 0 L32I.N,
  A14 A13 16 16 EXTUI,  A13 A13 0 16 EXTUI,  A15 A14 1 SRAI,  A13 A13 13 SLLI,
  A14 A14 31 SLLI,
$17D L:
  A10 A1 16 L32I.N,  A11 0 MOVI.N,  A10 A11 $1B8 L BNE,
  A10 A8 3 5 EXTUI,  A12 A10 0 16 EXTUI,  A11 A12 4 SRLI,  A10 A10 0 1 EXTUI,
  A10 A11 A10 ADD.N,  A11 A12 3 1 EXTUI,  A10 A10 A11 ADD.N,  A11 A12 2 1 EXTUI,
  A10 A10 A11 ADD.N,  A12 A12 1 1 EXTUI,  A10 A10 A12 ADD.N,  A11 3 MOVI.N,
  A10 A10 0 8 EXTUI,  A10 A10 A11 MINU,  A11 $10 L L32R,  A10 A10 A11 ADDX2,
  A11 A10 56 L16UI,  A11 A11 0 11 EXTUI,  A11 A11 2 SLLI,
$1B8 L:
  A7 A7 22 SLLI,  A14 A7 A14 ADD.N,  A10 A9 8 SLLI,  A7 A5 0 L32I.N,  A8 A8 17 SLLI,
  A13 A14 A13 ADD.N,  A10 A8 A10 ADD.N,  A11 A13 A11 ADD.N,  A9 A1 28 L32I.N,
  A10 A10 A15 ADD.N,  A12 A6 A9 ADD.N,  A12 A12 0 8 EXTUI,  A7 A7 52 L32I.N,
  A7 CALLX8,
  A3 $1E0 L BEQZ.N,
  A2 A2 1 ADDI.N,  A2 A2 0 8 EXTUI,
$1E0 L:
  A6 A6 1 ADDI.N,  A8 A1 20 L32I.N,  A6 A6 0 8 EXTUI,  A6 A8 $1ED L BGEU,
  $89 L J,
$1ED L:
  RETW.N,
$1EF L:  END-CODE

CODE set_rx_gain_testchip_70   \ .text.set_rx_gain_testchip_70 $5C
  $0 23 LITERALS
    $38 SYMBOL+ chip7_sleep_params  SYMBOL chip7_sleep_params  SYMBOL g_phyFuns
    $3FF460A0 T,  $4000 T,  $8000 T,  $3FF450DC T,  $F7FFFFFF T,
    SYMBOL phy_rxbb_dc  SYMBOL phy_rxrf_dc  $50 SYMBOL+ phy_rxrf_dc  $FFFF7FFF T,
    SYMBOL set_rx_gain_cal_iq  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL set_rx_gain_cal_dc  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL set_rx_gain_cal_dc
    SYMBOL esp_dport_access_reg_read
$5C L:
  A1 64 ENTRY,  A2 A2 0 8 EXTUI,  A7 A7 0 8 EXTUI,  A2 $84 L BNEZ.N,
  $1BE L J,
  2 ZEROS,
$6C L:
  A12 $0 L L32R,  A13 A10 MOV.N,  A11 128 MOVI,  A8 $30 L L32R,  A8 CALLX8,
  A8 A6 0 L32I.N,  A2 A8 A2 OR,  A2 A6 0 S32I.N,  $20A L J,
$84 L:
  A2 $8 L L32R,  A14 0 MOVI.N,  A13 2 MOVI.N,  A12 15 MOVI.N,  A11 1 MOVI.N,
  A3 A2 0 L32I.N,  A10 103 MOVI,  A3 A3 164 L32I,  A3 CALLX8,
  A8 A10 MOV.N,  A15 0 MOVI.N,  A3 A2 0 L32I.N,  A14 A15 MOV.N,  A13 2 MOVI.N,
  A12 15 MOVI.N,  A11 1 MOVI,  A3 A3 168 L32I,  A10 103 MOVI,  A8 A1 16 S32I,
  A3 CALLX8,
  A3 $C L L32R,  A10 A3 A3 OR,  A8 $34 L L32R,  A8 CALLX8,
  A5 $10 L L32R,  A5 A10 A5 OR,  A10 A3 MOV.N,  MEMW,  A5 A3 0 S32I,  A8 $38 L L32R,
  A8 CALLX8,
  A5 $14 L L32R,  A10 A10 A5 OR,  A5 A2 0 L32I.N,  A5 A5 192 L32I,  MEMW,
  A10 A3 0 S32I.N,  A5 CALLX8,
  A10 0 MOVI.N,  A5 A2 0 L32I.N,  A5 A5 208 L32I,  A5 CALLX8,
  A11 1 MOVI.N,  A10 A11 MOV.N,  A5 A2 0 L32I,  A9 A5 188 L32I,  A6 A5 184 L32I,
  A9 CALLX8,
  A5 2 MOVI.N,  A11 1 MOVI.N,  A10 A10 A5 OR,  A12 A10 0 16 EXTUI,  A10 A11 MOV.N,
  A6 CALLX8,
  A6 $18 L L32R,  A10 A6 MOV.N,  A8 $3C L L32R,  A8 CALLX8,
  A9 $1C L L32R,  A9 A10 A9 AND,  A10 0 MOVI.N,  MEMW,  A9 A6 0 S32I.N,  NOP.N,
  NOP.N,  NOP.N,  NOP.N,  A6 A2 0 L32I.N,  A6 A6 208 L32I,  A6 CALLX8,
  A11 1 MOVI.N,  A10 A11 MOV.N,  A6 A2 0 L32I.N,  A9 A6 188 L32I,  A6 A6 184 L32I,
  A9 CALLX8,
  A10 A10 A5 OR,  A11 1 MOVI.N,  A12 A10 0 16 EXTUI,  A10 A11 MOV.N,  A6 CALLX8,
  A13 A4 MOV.N,  A4 $28 L L32R,  A5 0 MOVI.N,  A12 1 MOVI.N,  A11 A5 MOV.N,
  A10 A12 MOV.N,  A15 $20 L L32R,  A14 $24 L L32R,  A4 A1 0 S32I.N,
  A5 A1 12 S32I.N,  A5 A1 8 S32I.N,  A7 A1 4 S32I.N,  A8 $40 L L32R,  A8 CALLX8,
  A10 A3 MOV.N,  A8 $44 L L32R,  A8 CALLX8,
  A4 $2C L L32R,  A4 A10 A4 AND,  A10 A5 MOV.N,  MEMW,  A4 A3 0 S32I.N,  NOP.N,
  NOP.N,  NOP.N,  NOP.N,  A3 A2 0 L32I,  A3 A3 208 L32I,  A3 CALLX8,
  A3 A2 0 L32I,  A3 A3 196 L32I,  A3 CALLX8,
  A14 A5 MOV.N,  A13 2 MOVI.N,  A2 A2 0 L32I.N,  A12 15 MOVI.N,  A11 1 MOVI.N,
  A10 103 MOVI,  A8 A1 16 L32I.N,  A15 A8 MOV.N,  A2 A2 168 L32I,  A2 CALLX8,
  RETW.N,
$1BE L:
  A3 $C L L32R,  A10 A3 MOV.N,  A8 $48 L L32R,  A8 CALLX8,
  A5 $10 L L32R,  A5 A10 A5 OR,  A10 A3 MOV.N,  MEMW,  A5 A3 0 S32I.N,
  A8 $4C L L32R,  A8 CALLX8,
  A5 $14 L L32R,  A10 A10 A5 OR,  A5 $8 L L32R,  A6 A5 0 L32I.N,  A6 A6 192 L32I,
  MEMW,  A10 A3 0 S32I.N,  A6 CALLX8,
  A10 A2 MOV.N,  A2 A5 0 L32I.N,  A2 A2 208 L32I,  A2 CALLX8,
  A2 $400 MOVI,  A6 $4 L L32R,  A10 A6 0 L32I.N,  A10 A2 A10 AND,  A10 $6C L BEQZ,
$20A L:
  A6 $18 L L32R,  A10 A6 MOV.N,  A8 $50 L L32R,  A8 CALLX8,
  A2 $1C L L32R,  A2 A10 A2 AND,  A10 0 MOVI.N,  MEMW,  A2 A6 0 S32I.N,  NOP.N,
  NOP.N,  NOP.N,  NOP.N,  A2 A5 0 L32I.N,  A2 A2 208 L32I,  A2 CALLX8,
  A2 14 MOVI.N,  A11 0 MOVI.N,  A2 A1 12 S32I.N,  A2 4 MOVI.N,  A10 A11 MOV.N,
  A13 A4 MOV.N,  A12 3 MOVI.N,  A2 A1 8 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,
  A2 $28 L L32R,  A15 $20 L L32R,  A14 $24 L L32R,  A2 A1 0 S32I.N,  A7 A1 4 S32I.N,
  A8 $54 L L32R,  A8 CALLX8,
  A10 A3 MOV.N,  A8 $58 L L32R,  A8 CALLX8,
  A2 $2C L L32R,  A2 A10 A2 AND,  A10 0 MOVI.N,  MEMW,  A2 A3 0 S32I.N,  NOP.N,
  NOP.N,  NOP.N,  NOP.N,  A2 A5 0 L32I.N,  A2 A2 208 L32I,  A2 CALLX8,
  A2 A5 0 L32I.N,  A2 A2 196 L32I,  A2 CALLX8,
  RETW.N,
$28C L:  END-CODE

CODE bt_correct_bbgain   \ .text.bt_correct_bbgain $4
  $0 1 LITERALS
    SYMBOL CSWTCH$356
$4 L:
  A1 48 ENTRY,  A11 A1 MOV.N,  A9 A3 8 ADDI.N,  A10 A3 A3 OR,  A8 8 MOVI,
  A8 $21 L LOOP,
  A12 A10 0 L8UI,  A10 A10 1 ADDI.N,  A12 A11 0 S8I,  MEMW,  A11 A11 1 ADDI.N,
$21 L:
  A12 0 MOVI.N,  NOP.N,  NOP.N,  A10 A2 0 L16UI,  A10 A12 $39 L BEQ,
  A4 1 MOVI.N,  A10 A10 -48 ADDI,  A8 2 MOVI.N,  A12 A4 MOV.N,  A12 A8 A10 MOVEQZ,
$39 L:
  A13 A1 0 L8UI,  A8 2 MOVI.N,  A10 0 MOVI.N,  A14 48 MOVI.N,  A15 11 MOVI.N,
$44 L:
  A11 A13 7 SEXT,  A14 A11 $61 L BGE,
  A5 A10 -24 ADDI,  A4 A13 -24 ADDI,  A11 A12 -1 ADDI.N,  A12 $78 L BEQZ.N,
  A10 A5 0 8 EXTUI,  A13 A4 0 8 EXTUI,  A12 A11 0 8 EXTUI,  $78 L J,
  1 ZEROS,
$61 L:
  A15 A11 $78 L BLT,
  A11 A12 1 ADDI.N,  A5 A10 24 ADDI,  A4 A13 24 ADDI,  A12 3 $78 L BGEUI,
  A12 A11 0 8 EXTUI,  A10 A5 0 8 EXTUI,  A13 A4 0 8 EXTUI,
$78 L:
  A8 A8 -1 ADDI.N,  A8 A8 0 8 EXTUI,  A8 $44 L BNEZ,
  A8 32 MOVI.N,  A12 3 $8E L BGEUI,
  A8 $0 L L32R,  A8 A12 A8 ADDX2,  A8 A8 0 L16UI,
$8E L:
  A10 A10 0 8 EXTUI,  A9 A9 A3 SUB,  A8 A2 0 S16I,  MEMW,  A9 $B2 L LOOP,
  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A8 A3 0 L8UI,  A8 A10 A8 ADD.N,  A8 A3 0 S8I,
  MEMW,  A3 A3 1 ADDI.N,
$B2 L:
  RETW.N,
$B4 L:  END-CODE

CODE bt_index_to_bb   \ .text.bt_index_to_bb $4
  $0 1 LITERALS
    SYMBOL CSWTCH$356
$4 L:
  A1 32 ENTRY,  A8 A2 0 16 EXTUI,  A2 32 MOVI.N,  A8 3 $18 L BGEUI,
  A2 $0 L L32R,  A8 A8 A2 ADDX2,  A2 A8 0 L16UI,
$18 L:
  RETW.N,
$1A L:  END-CODE

CODE bt_bb_to_index   \ .text.bt_bb_to_index $0
  $0 ORIGIN
  A1 32 ENTRY,  A8 A2 0 16 EXTUI,  A2 0 MOVI.N,  A8 A2 $18 L BEQ,
  A2 A8 -48 ADDI,  A9 1 MOVI.N,  A8 2 MOVI.N,  A9 A8 A2 MOVEQZ,  A2 A9 A9 OR,
$18 L:
  RETW.N,
$1A L:  END-CODE

CODE bt_txdc_cal   \ .text.bt_txdc_cal $8
  $0 2 LITERALS
    SYMBOL chip7_sleep_params  SYMBOL bt_txdc_cal$part$5
$8 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A8 A8 0 L32I.N,  A8 12 $19 L BBSI,
  A8 $4 L L32R,  A8 CALLX8,
$19 L:
  RETW.N,
$1B L:  END-CODE

CODE get_bbgain_db   \ .text.get_bbgain_db $0
  $0 ORIGIN
  A1 32 ENTRY,  A11 A2 0 16 EXTUI,  A8 0 MOVI.N,  A2 A11 6 8 EXTUI,  A10 A8 MOV.N,
  A11 A11 0 6 EXTUI,  A9 6 MOVI.N,  A9 $1F L LOOP,
  A12 A10 6 ADDI.N,  A11 A8 $1D L BBC,
  A10 A12 0 8 EXTUI,
$1D L:
  A8 A8 1 ADDI.N,
$1F L:
  A2 A10 A2 ADD.N,  A2 A2 0 8 EXTUI,  RETW.N,
$26 L:  END-CODE

CODE bt_txiq_cal   \ .text.bt_txiq_cal $8
  $0 2 LITERALS
    SYMBOL chip7_sleep_params  SYMBOL bt_txiq_cal$part$6
$8 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A8 A8 0 L32I.N,  A8 11 $19 L BBSI,
  A8 $4 L L32R,  A8 CALLX8,
$19 L:
  RETW.N,
$1B L:  END-CODE

CODE force_bttx_gain   \ .text.force_bttx_gain $1C
  $0 7 LITERALS
    SYMBOL bt_dig_gain_force  SYMBOL phy_bt_power_track_en
    $81 SYMBOL+ chip7_sleep_params  SYMBOL wr_bt_tx_gain_mem
    SYMBOL wr_bt_tx_atten  SYMBOL wr_bt_tx_gain_mem  SYMBOL wr_bt_tx_atten
$1C L:
  A1 48 ENTRY,  A2 A2 0 8 EXTUI,  A10 A3 0 8 EXTUI,  A11 A4 0 8 EXTUI,
  A5 A5 0 8 EXTUI,  A2 $75 L BEQZ,
  A12 A5 0 8 EXTUI,  A9 A1 MOV.N,  A8 8 MOVI.N,  A8 $40 L LOOP,
  A12 A9 0 S8I,  MEMW,  A9 A9 1 ADDI.N,
$40 L:
  A8 $C L L32R,  A8 CALLX8,
  A2 0 MOVI.N,  A5 $0 L L32R,  A10 A1 MOV.N,  A2 A5 0 S8I,  MEMW,  A8 $10 L L32R,
  A8 CALLX8,
  A8 1 MOVI.N,  A8 A5 0 S8I,  MEMW,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A5 $4 L L32R,
  A2 A5 0 S8I,  MEMW,  RETW.N,
  1 ZEROS,
$75 L:
  A5 $4 L L32R,  A8 1 MOVI.N,  A10 255 MOVI,  A11 A10 MOV.N,  A8 A5 0 S8I,  MEMW,
  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A5 $0 L L32R,  A2 A5 0 S8I,  MEMW,  A8 $14 L L32R,
  A8 CALLX8,
  A10 $8 L L32R,  A8 $18 L L32R,  A8 CALLX8,
  RETW.N,
$A7 L:  END-CODE

CODE phy_bttx_low_power   \ .text.phy_bttx_low_power $10
  $0 4 LITERALS
    SYMBOL g_phyFuns  SYMBOL chip7_sleep_params  $40 SYMBOL+ chip7_sleep_params
    SYMBOL force_bttx_gain
$10 L:
  A1 32 ENTRY,  A3 A3 0 8 EXTUI,  A2 A2 0 8 EXTUI,  A3 6 $98 L BGEUI,
  A4 0 MOVI.N,  A2 A4 $51 L BEQ,
  A3 A4 $51 L BNE,
  A3 $0 L L32R,  A12 1 MOVI.N,  A13 96 MOVI,  A11 2 MOVI.N,  A10 107 MOVI,
  A8 A3 0 L32I.N,  A8 A8 160 L32I,  A8 CALLX8,
  A12 2 MOVI.N,  A11 A12 MOV.N,  A3 A3 0 L32I.N,  A13 A4 MOV.N,  A10 107 MOVI,
  A3 A3 160 L32I,  A3 CALLX8,
  A11 A4 MOV.N,  $89 L J,
  2 ZEROS,
$51 L:
  A4 $0 L L32R,  A8 $4 L L32R,  A4 A4 0 L32I.N,  A11 A8 81 L8UI,  A4 A4 $16C L32I,
  A10 $8 L L32R,  A4 CALLX8,
  A9 A3 -1 ADDI.N,  A11 0 MOVI.N,  A9 1 $89 L BLTI,
$6C L:
  A4 0 MOVI.N,  A8 A4 MOV.N,  A11 A4 MOV.N,  A3 1 MOVI.N,
$74 L:
  A4 SSL,  A4 A3 SLL,  A8 A8 1 ADDI.N,  A11 A11 A4 ADD.N,  A8 A8 0 8 EXTUI,
  A11 A11 0 8 EXTUI,  A4 A8 MOV.N,  A8 A9 $74 L BLT,
$89 L:
  A13 40 MOVI.N,  A12 0 MOVI.N,  A10 A2 MOV.N,  A8 $C L L32R,  A8 CALLX8,
  RETW.N,
  1 ZEROS,
$98 L:
  A3 $0 L L32R,  A4 $4 L L32R,  A3 A3 0 L32I.N,  A11 A4 81 L8UI,  A3 A3 $16C L32I,
  A10 $8 L L32R,  A3 CALLX8,
  A9 4 MOVI.N,  $6C L J,
$B1 L:  END-CODE

CODE phy_wifitx_low_power   \ .text.phy_wifitx_low_power $2C
  $0 11 LITERALS
    SYMBOL phy_chan_gain_table  SYMBOL CSWTCH$367  SYMBOL CSWTCH$368
    SYMBOL CSWTCH$369  $AAAAAAAB T,  $3FF46004 T,  $3FF46008 T,
    SYMBOL phy_set_most_tpw_disbg  SYMBOL force_tx_gain_en
    SYMBOL set_tx_gain_table  SYMBOL tx_gain_table_set
$2C L:
  A1 48 ENTRY,  A3 A3 0 8 EXTUI,  A8 10 MOVI.N,  A2 A2 0 8 EXTUI,  A4 A4 0 8 EXTUI,
  A8 A3 $40 L BGEU,
  A3 11 MOVI,
$40 L:
  A8 A4 7 SEXT,  A8 7 $4E L BGEI,
  A9 -5 MOVI.N,  A8 A9 $54 L BLT,
  $57 L J,
$4E L:
  A4 6 MOVI.N,  $57 L J,
  1 ZEROS,
$54 L:
  A4 251 MOVI,
$57 L:
  A2 $155 L BEQZ,
  A3 A3 -1 ADDI.N,  A3 A3 0 8 EXTUI,  A2 10 MOVI.N,  A2 A3 $D5 L BLTU,
  A2 $4 L L32R,  A9 A3 1 SLLI,  A2 A2 A9 ADD.N,  A13 1 MOVI.N,  A8 0 MOVI.N,
  A2 A2 0 L16UI,  A11 A8 MOV.N,  A14 5 MOVI.N,  A12 $10 L L32R,  A12 A2 A12 MULUH,
  A12 A12 2 SRLI,  A10 A12 A12 ADDX2,  A13 SSL,  A10 A10 SLL,  A15 $8 L L32R,
  A2 A2 A10 SUB,  A9 A15 A9 ADD.N,  A12 A12 0 16 EXTUI,  A2 A2 0 16 EXTUI,
  A10 $C L L32R,  A3 A10 A3 ADD.N,  A15 A3 0 L8UI,  A10 A9 0 L16UI,
  A12 A8 $B1 L BNE,
  A11 A2 6 SLLI,  A11 A11 0 16 EXTUI,  A3 A15 0 8 EXTUI,  $DC L J,
$B1 L:
  A9 A14 A8 SUB,  A9 SSL,  A9 A13 SLL,  A8 A8 1 ADDI.N,  A11 A11 A9 ADD.N,
  A8 A8 0 8 EXTUI,  A11 A11 0 16 EXTUI,  A8 A12 $B1 L BLTU,
  A8 A2 6 SLLI,  A11 A11 A8 ADD.N,  A11 A11 0 16 EXTUI,  A3 A15 0 8 EXTUI,  $DC L J,
$D5 L:
  A11 0 MOVI.N,  A3 242 MOVI,  A10 A11 MOV.N,
$DC L:
  A8 $24 L L32R,  A8 CALLX8,
  A2 $0 L L32R,  A12 A4 0 8 EXTUI,  A11 A2 MOV.N,  A10 A1 A1 OR,  A9 8 MOVI,
  A4 A2 5 L8UI,  A4 A4 A3 SUB,  A4 A4 0 8 EXTUI,  A9 $10F L LOOP,
  A8 A11 0 L8UI,  A11 A11 1 ADDI.N,  A8 A8 A4 SUB,  A8 A8 A12 SUB,  A8 A10 0 S8I,
  MEMW,  A10 A10 1 ADDI.N,
$10F L:
  A2 1 MOVI.N,  NOP.N,  NOP.N,  A4 A1 0 L32I.N,  A3 $14 L L32R,  MEMW,
  A4 A3 0 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A4 A1 4 L32I.N,  A3 $18 L L32R,
  MEMW,  A4 A3 0 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A3 $1C L L32R,
  A2 A3 0 S8I,  MEMW,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A3 $20 L L32R,  A2 A3 0 S8I,
  MEMW,  RETW.N,
$155 L:
  A3 $1C L L32R,  A2 A3 0 S8I,  MEMW,  NOP.N,  NOP.N,  NOP.N,  NOP.N,
  A3 $20 L L32R,  A2 A3 0 S8I,  MEMW,  A8 $28 L L32R,  A8 CALLX8,
  RETW.N,
$177 L:  END-CODE

CODE set_tx_gain_table_bt   \ .text.set_tx_gain_table_bt $18
  $0 6 LITERALS
    SYMBOL chip7_sleep_params  SYMBOL bt_dig_gain_force  SYMBOL bt_txdc_cal$part$5
    SYMBOL bt_txiq_cal$part$6  SYMBOL bt_tx_pwctrl_init
    SYMBOL bt_tx_gain_cal_set$part$4
$18 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A8 A2 0 L32I,  A8 12 $2C L BBSI,
  A8 $8 L L32R,  A8 CALLX8,
  A8 A2 0 L32I.N,
$2C L:
  A8 11 $35 L BBSI,
  A8 $C L L32R,  A8 CALLX8,
$35 L:
  A8 $10 L L32R,  A8 CALLX8,
  A2 $4 L L32R,  A2 A2 0 L8UI,  A2 $49 L BNEZ.N,
  A8 $14 L L32R,  A8 CALLX8,
$49 L:
  RETW.N,
$4B L:  END-CODE

CODE set_tx_dig_gain   \ .text.set_tx_dig_gain $C
  $0 3 LITERALS
    $3FF46000 T,  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read
$C L:
  A1 32 ENTRY,  A4 $0 L L32R,  A3 A3 0 8 EXTUI,  A10 A4 MOV.N,  A8 $4 L L32R,
  A8 CALLX8,
  A8 -1021 MOVI,  A10 A10 A8 AND,  A3 A3 2 SLLI,  A10 A3 A10 OR,  MEMW,
  A10 A4 0 S32I.N,  A10 A4 MOV.N,  A8 $8 L L32R,  A8 CALLX8,
  A3 -2 MOVI.N,  A10 A10 A3 AND,  A2 A2 0 1 EXTUI,  A2 A10 A2 OR,  MEMW,
  A2 A4 0 S32I.N,  RETW.N,
$48 L:  END-CODE

CODE spur_cal   \ .text.spur_cal $4
  $0 1 LITERALS
    SYMBOL spur_cal$part$8
$4 L:
  A1 32 ENTRY,  A13 A5 0 8 EXTUI,  A10 A2 0 16 EXTUI,  A3 A3 0 8 EXTUI,
  A12 A4 0 16 EXTUI,  A13 $24 L BEQZ.N,
  A12 $24 L BEQZ.N,
  A11 A3 7 SEXT,  A8 $0 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  RETW.N,
$24 L:
  A2 0 MOVI.N,  RETW.N,
$28 L:  END-CODE

CODE set_chanfreq   \ .text.set_chanfreq $8
  $0 2 LITERALS
    SYMBOL g_phyFuns  SYMBOL chip_v7_set_chan
$8 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A11 128 MOVI,  A10 A2 0 16 EXTUI,  A3 A3 0 8 EXTUI,
  A2 0 MOVI.N,  A8 A8 0 L32I.N,  A8 A8 108 L32I,  A8 CALLX8,
  A11 A3 7 SEXT,  A10 A10 7 SEXT,  A8 $4 L L32R,  A8 CALLX8,
  RETW.N,
$2F L:  END-CODE

CODE set_chanfreq_nomac   \ .text.set_chanfreq_nomac $8
  $0 2 LITERALS
    SYMBOL g_phyFuns  SYMBOL chip_v7_set_chan_nomac
$8 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A11 128 MOVI,  A10 A2 0 16 EXTUI,  A3 A3 0 8 EXTUI,
  A2 0 MOVI.N,  A8 A8 0 L32I.N,  A8 A8 108 L32I,  A8 CALLX8,
  A11 A3 7 SEXT,  A10 A10 7 SEXT,  A8 $4 L L32R,  A8 CALLX8,
  RETW.N,
$2F L:  END-CODE

CODE chip_sleep_prot_en   \ .text.chip_sleep_prot_en $2C
  $0 11 LITERALS
    $60033CB8 T,  $8000 T,  $6000 T,  $7FF8000 T,  SYMBOL g_phyFuns
    SYMBOL esp_dport_access_reg_read  SYMBOL ets_delay_us
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL ets_delay_us  SYMBOL esp_dport_access_reg_read
$2C L:
  A1 32 ENTRY,  A2 $0 L L32R,  A10 A2 MOV.N,  A8 $14 L L32R,  A8 CALLX8,
  A3 $4 L L32R,  A10 A10 A3 OR,  MEMW,  A10 A2 0 S32I.N,  NOP.N,  NOP.N,  NOP.N,
  NOP.N,  A3 $8 L L32R,  $5D L J,
  2 ZEROS,
$55 L:
  A10 2 MOVI.N,  A8 $18 L L32R,  A8 CALLX8,
$5D L:
  A10 A2 MOV.N,  A8 $1C L L32R,  A8 CALLX8,
  A10 A3 $55 L BANY,
  A10 A2 MOV.N,  A8 $20 L L32R,  A8 CALLX8,
  A8 $C L L32R,  A10 A10 A8 OR,  MEMW,  A10 A2 0 S32I.N,  $86 L J,
$7E L:
  A10 2 MOVI.N,  A8 $24 L L32R,  A8 CALLX8,
$86 L:
  A10 A2 MOV.N,  A8 $28 L L32R,  A8 CALLX8,
  A10 A3 $7E L BANY,
  A2 $10 L L32R,  A2 A2 0 L32I.N,  A2 A2 8 L32I,  A2 CALLX8,
  RETW.N,
$9E L:  END-CODE

CODE chip_sleep_prot_dis   \ .text.chip_sleep_prot_dis $10
  $0 4 LITERALS
    SYMBOL g_phyFuns  $60033CB8 T,  $F8007FFF T,  SYMBOL esp_dport_access_reg_read
$10 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A2 A2 0 L32I.N,  A2 A2 12 L32I.N,  A2 CALLX8,
  A2 $4 L L32R,  A10 A2 MOV.N,  A8 $C L L32R,  A8 CALLX8,
  A8 $8 L L32R,  A10 A10 A8 AND,  MEMW,  A10 A2 0 S32I.N,  RETW.N,
$35 L:  END-CODE

CODE chip_v7_rxmax_ext_dig   \ .text.chip_v7_rxmax_ext_dig $0
  $0 ORIGIN
  A1 32 ENTRY,  RETW.N,
$5 L:  END-CODE

CODE chip_v7_rxmax_ext   \ .text.chip_v7_rxmax_ext $0
  $0 ORIGIN
  A1 32 ENTRY,  RETW.N,
$5 L:  END-CODE

CODE set_cca   \ .text.set_cca $18
  $0 6 LITERALS
    $3FF5C018 T,  $40000000 T,  $3FF5C01C T,  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
$18 L:
  A1 32 ENTRY,  A4 $0 L L32R,  A2 A2 0 8 EXTUI,  A10 A4 A4 OR,  A8 $C L L32R,
  A8 CALLX8,
  A9 1 MOVI.N,  A8 $4 L L32R,  A3 A3 0 8 EXTUI,  A10 A10 A8 OR,  A8 0 MOVI.N,
  A8 A9 A2 MOVEQZ,  A2 A8 0 8 EXTUI,  MEMW,  A10 A4 0 S32I.N,  A2 $4E L BNEZ,
  A3 A3 7 SEXT,  A2 A9 A3 MOVEQZ,  A2 $70 L BEQZ,
$4E L:
  NOP.N,  NOP.N,  A3 $8 L L32R,  A2 -256 MOVI,  A10 A3 MOV.N,  A8 $10 L L32R,
  A8 CALLX8,
  A10 A10 A2 AND,  A2 191 MOVI,  A10 A10 A2 OR,  MEMW,  A10 A3 0 S32I.N,  RETW.N,
$70 L:
  A4 $8 L L32R,  A2 -256 MOVI,  A10 A4 MOV.N,  A8 $14 L L32R,  A8 CALLX8,
  A10 A10 A2 AND,  A3 A3 0 8 EXTUI,  A3 A10 A3 OR,  MEMW,  A3 A4 0 S32I.N,  RETW.N,
$8E L:  END-CODE

CODE phy_get_cca   \ .text.phy_get_cca $8
  $0 2 LITERALS
    $3FF5C01C T,  SYMBOL esp_dport_access_reg_read
$8 L:
  A1 32 ENTRY,  A10 $0 L L32R,  A8 $4 L L32R,  A8 CALLX8,
  A2 A10 0 8 EXTUI,  RETW.N,
$19 L:  END-CODE

CODE phy_set_cca_cnt   \ .text.phy_set_cca_cnt $18
  $0 6 LITERALS
    $3FF5D058 T,  $F8000000 T,  $7FFFFFF T,  $18000000 T,
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
$18 L:
  A1 32 ENTRY,  A4 $0 L L32R,  A3 A3 0 8 EXTUI,  A10 A4 MOV.N,  A8 $10 L L32R,
  A8 CALLX8,
  A8 $4 L L32R,  A10 A10 A8 AND,  A8 $8 L L32R,  A2 A2 A8 AND,  A10 A10 A2 OR,
  MEMW,  A10 A4 0 S32I.N,  A3 $54 L BEQZ,
  A10 A4 A4 OR,  A8 $14 L L32R,  A8 CALLX8,
  A2 $C L L32R,  A10 A10 A2 OR,  MEMW,  A10 A4 0 S32I.N,
$54 L:
  RETW.N,
$56 L:  END-CODE

CODE phy_get_cca_cnt   \ .text.phy_get_cca_cnt $1C
  $0 7 LITERALS
    $3FF5D05C T,  $7FFFFFF T,  $3FF5D060 T,  $8000000 T,
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read
$1C L:
  A1 32 ENTRY,  A4 $0 L L32R,  A10 A4 MOV.N,  A8 $10 L L32R,  A8 CALLX8,
  A3 $4 L L32R,  A8 A10 A3 AND,  A10 $8 L L32R,  A8 A2 0 S32I,  A8 $14 L L32R,
  A8 CALLX8,
  A3 A10 A3 AND,  A10 A4 MOV.N,  A3 A2 4 S32I.N,  A8 $18 L L32R,  A8 CALLX8,
  A2 $C L L32R,  A2 A10 A2 AND,  A2 A2 27 5 EXTUI,  RETW.N,
$54 L:  END-CODE

CODE phy_get_noise_floor   \ .text.phy_get_noise_floor $8
  $0 2 LITERALS
    $3FF5C050 T,  SYMBOL esp_dport_access_reg_read
$8 L:
  A1 32 ENTRY,  A10 $0 L L32R,  A8 $4 L L32R,  A8 CALLX8,
  A10 A10 0 10 EXTUI,  A2 A10 -1024 ADDMI,  RETW.N,
$1C L:  END-CODE

CODE phy_get_rssi   \ .text.phy_get_rssi $14
  $0 5 LITERALS
    $3FF5C050 T,  $3FF5C06C T,  $FF00 T,  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read
$14 L:
  A1 32 ENTRY,  A10 $0 L L32R,  A8 $C L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  A2 A2 0 10 EXTUI,  A10 $4 L L32R,  A2 A2 -1024 ADDMI,
  A8 $10 L L32R,  A8 CALLX8,
  A2 A2 2 SRAI,  A8 $8 L L32R,  A10 A10 A8 AND,  A10 A10 8 SRLI,  A2 A2 A10 ADD.N,
  A2 A2 15 SEXT,  RETW.N,
$44 L:  END-CODE

CODE read_hw_noisefloor   \ .text.read_hw_noisefloor $8
  $0 2 LITERALS
    $3FF5C018 T,  SYMBOL esp_dport_access_reg_read
$8 L:
  A1 32 ENTRY,  A10 $0 L L32R,  A8 $4 L L32R,  A8 CALLX8,
  A10 A10 5 10 EXTUI,  A2 A10 -1024 ADDMI,  RETW.N,
$1C L:  END-CODE

CODE phy_get_txpwr_param   \ .text.phy_get_txpwr_param $8
  $0 2 LITERALS
    SYMBOL chip7_sleep_params  SYMBOL phy_tx_pwr_error
$8 L:
  A1 32 ENTRY,  A12 $0 L L32R,  A10 A12 MOV.N,  A9 A12 MOV.N,  A8 4 MOVI,
  A8 $35 L LOOP,
  A11 A10 4 L16UI,  A9 A9 6 ADDI.N,  A10 A10 2 ADDI.N,  A11 A2 0 S8I,  MEMW,
  A2 A2 1 ADDI.N,  NOP.N,  NOP.N,  NOP.N,  A11 A9 95 L8UI,  A11 A5 0 S8I,
  A5 A5 1 ADDI.N,
$35 L:
  A2 0 MOVI.N,  A2 A3 0 S8I,  A2 A3 1 S8I,  MEMW,  NOP.N,  NOP.N,  NOP.N,  NOP.N,
  A2 A12 188 L16UI,  A3 A2 9 SLLI,  A2 A12 190 L16UI,  A2 A3 A2 OR,  A2 A4 0 S16I,
  MEMW,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A2 A12 184 L8UI,  A2 A6 0 S8I,  MEMW,
  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A2 $4 L L32R,  A2 A2 0 L16UI,  A2 A7 0 S16I,
  MEMW,  RETW.N,
$81 L:  END-CODE

CODE noise_init   \ .text.noise_init $28
  $0 10 LITERALS
    $3FF5C018 T,  $10000000 T,  SYMBOL chip7_sleep_params  $200000 T,
    SYMBOL noise_array  SYMBOL g_phyFuns  SYMBOL noise_timeout_flag
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL chip_v7_set_chan_nomac
$28 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A10 A2 MOV.N,  A8 $1C L L32R,  A8 CALLX8,
  A3 $4 L L32R,  A3 A10 A3 OR,  A10 A2 MOV.N,  MEMW,  A3 A2 0 S32I.N,
  A8 $20 L L32R,  A8 CALLX8,
  A3 -8 MOVI.N,  A10 A10 A3 AND,  A3 4 MOVI.N,  A7 $8 L L32R,  A10 A10 A3 OR,  MEMW,
  A10 A2 0 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A8 A7 0 L32I.N,  A2 $C L L32R,
  A8 A8 A2 AND,  A8 $78 L BEQZ.N,
  A10 A7 78 L16SI,  A2 $14 L L32R,  $FE L J,
  2 ZEROS,
$78 L:
  A5 $10 L L32R,  A3 A8 MOV.N,  A4 1 MOVI.N,  A6 A8 MOV.N,  A2 $14 L L32R,
$84 L:
  A11 10 MOVI.N,  A10 A4 MOV.N,  A6 A7 83 S8I,  MEMW,  A8 $24 L L32R,  A8 CALLX8,
  A8 0 MOVI.N,  A10 A2 0 L32I.N,  A10 A10 128 L32I,  A8 A5 0 S16I,  MEMW,
  A10 CALLX8,
  A11 A10 NEG,  A11 A11 15 SEXT,  A10 1 $110 L BLTI,
  A10 A5 0 L16SI,  A11 A10 $119 L BLT,
$B3 L:
  A8 1 MOVI.N,  A3 SSL,  A10 A8 SLL,  A8 $18 L L32R,  A11 A8 0 L8UI,
  A10 A10 A11 OR,  A10 A8 0 S8I,  MEMW,
$CA L:
  A3 A3 1 ADDI.N,  A4 A4 5 ADDI.N,  A5 A5 2 ADDI.N,  A3 3 $84 L BNEI,
  NOP.N,  A3 $10 L L32R,  A4 0 MOVI.N,  A5 A3 0 L16SI,  A5 A5 A4 MIN,
  A4 A3 2 L16SI,  A5 A5 A4 MIN,  A6 $C L L32R,  A4 A7 0 L32I.N,  A4 A4 A6 OR,
  A3 A3 4 L16SI,  A3 A5 A3 MIN,  A10 A3 MOV.N,  A3 A7 78 S16I,  A3 A7 178 S16I,
  A4 A7 0 S32I.N,
$FE L:
  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A2 A2 0 L32I.N,  A2 A2 132 L32I,  A2 CALLX8,
  RETW.N,
$110 L:
  A11 A5 0 L16SI,  A10 A11 $CA L BGE,
  $122 L J,
$119 L:
  A11 A5 0 S16I,  MEMW,  $B3 L J,
$122 L:
  A10 A5 0 S16I,  MEMW,  $CA L J,
$12B L:  END-CODE

CODE target_power_backoff   \ .text.target_power_backoff $C
  $0 3 LITERALS
    SYMBOL chip7_sleep_params  SYMBOL target_power_backoff_qdb
    SYMBOL set_chan_dig_gain
$C L:
  A1 32 ENTRY,  A9 $0 L L32R,  A2 A2 7 SEXT,  A11 $4 L L32R,  A8 A9 182 L16UI,
  A10 A11 0 L16UI,  A8 A8 A10 SUB,  A8 A2 A8 ADD.N,  A10 A9 81 L8UI,
  A8 A9 182 S16I,  A2 A11 0 S16I,  MEMW,  A8 $8 L L32R,  A8 CALLX8,
  RETW.N,
$37 L:  END-CODE

CODE phy_set_rfrx_dcap   \ .text.phy_set_rfrx_dcap $14
  $0 5 LITERALS
    SYMBOL rfrx_dcap_enable  SYMBOL g_phyFuns  SYMBOL rfrx_vga_lna_dcap
    SYMBOL set_chan_rxcomp  SYMBOL set_chan_rxcomp
$14 L:
  A1 32 ENTRY,  A3 $0 L L32R,  A4 0 MOVI.N,  A2 A2 0 8 EXTUI,  A8 A3 0 L8UI,
  A8 A4 $66 L BNE,
  A2 1 $66 L BNEI,
  A14 A4 MOV.N,  A2 $4 L L32R,  A11 A4 MOV.N,  A13 3 MOVI.N,  A12 7 MOVI.N,
  A10 100 MOVI,  A5 A2 0 L32I.N,  A5 A5 164 L32I,  A5 CALLX8,
  A14 A4 MOV.N,  A11 A4 MOV.N,  A5 A2 0 L32I.N,  A13 3 MOVI,  A12 4 MOVI,
  A4 $8 L L32R,  A5 A5 164 L32I,  A10 A4 0 S8I,  MEMW,  A10 100 MOVI,  A5 CALLX8,
  A10 A4 1 S8I,  MEMW,  $6C L J,
  1 ZEROS,
$66 L:
  A2 $AA L BEQZ,
  A2 $4 L L32R,
$6C L:
  A15 0 MOVI.N,  A14 A15 MOV.N,  A11 A15 MOV.N,  A13 3 MOVI.N,  A4 A2 0 L32I.N,
  A12 7 MOVI.N,  A10 100 MOVI,  A4 A4 168 L32I,  A4 CALLX8,
  A15 0 MOVI.N,  A14 A15 MOV.N,  A2 A2 0 L32I.N,  A13 3 MOVI.N,  A12 4 MOVI.N,
  A11 A15 MOV.N,  A10 100 MOVI,  A2 A2 168 L32I,  A2 CALLX8,
  A10 -28 MOVI.N,  A8 $C L L32R,  A8 CALLX8,
  A2 1 MOVI.N,  A2 A3 0 S8I,  MEMW,  RETW.N,
  2 ZEROS,
$AA L:
  A8 $E6 L BEQZ.N,
  A5 $4 L L32R,  A14 A2 MOV.N,  A11 A2 MOV.N,  A13 3 MOVI.N,  A12 7 MOVI.N,
  A4 $8 L L32R,  A10 100 MOVI,  A8 A5 0 L32I.N,  A15 A4 0 L8UI,  A8 A8 168 L32I,
  A8 CALLX8,
  A10 100 MOVI,  A14 A2 MOV.N,  A5 A5 0 L32I.N,  A13 3 MOVI.N,  A12 4 MOVI.N,
  A11 A2 MOV.N,  A5 A5 168 L32I,  A15 A4 1 L8UI,  A5 CALLX8,
  A10 A2 MOV.N,  A8 $10 L L32R,  A8 CALLX8,
$E6 L:
  A2 0 MOVI.N,  A2 A3 0 S8I,  MEMW,  RETW.N,
$F0 L:  END-CODE

CODE chip_v7_set_chan_misc   \ .text.chip_v7_set_chan_misc $30
  $0 12 LITERALS
    SYMBOL g_phyFuns  SYMBOL chip7_sleep_params  SYMBOL re_entry
    SYMBOL phy_init_flag  $50 SYMBOL+ phy_rxrf_dc  SYMBOL phy_rxbb_dc
    $24 SYMBOL+ phy_rxrf_dc  SYMBOL rfbb_gain_swp  $40 SYMBOL+ chip7_sleep_params
    SYMBOL wr_rx_gain_mem  SYMBOL set_chan_rxcomp  SYMBOL set_chan_dig_gain
$30 L:
  A1 48 ENTRY,  A3 $0 L L32R,  A2 A2 0 8 EXTUI,  A4 A3 0 L32I,  A5 $4 L L32R,
  A4 A4 132 L32I,  A10 A5 78 L16SI,  A4 CALLX8,
  A4 $8 L L32R,  A8 A4 0 L8UI,  A8 $58 L BEQZ.N,
  A8 $C L L32R,  A8 A8 0 L8UI,  A8 $7C L BNEZ.N,
$58 L:
  A11 1 MOVI.N,  A9 0 MOVI.N,  A8 A5 176 L8UI,  A10 A9 MOV.N,  A8 A8 A11 ADD.N,
  A8 A8 0 8 EXTUI,  A15 $10 L L32R,  A14 $14 L L32R,  A13 $18 L L32R,
  A12 $1C L L32R,  A9 A1 4 S32I.N,  A8 A1 0 S32I.N,  A8 $24 L L32R,  A8 CALLX8,
$7C L:
  A10 0 MOVI.N,  A8 $28 L L32R,  A8 CALLX8,
  A2 A2 0 8 EXTUI,  A3 A3 0 L32I.N,  A11 A2 MOV.N,  A3 A3 $16C L32I,
  A10 $20 L L32R,  A3 CALLX8,
  A3 A4 0 L8UI,  A3 $A1 L BEQZ.N,
  A3 $C L L32R,  A3 A3 0 L8UI,  A3 $A9 L BNEZ.N,
$A1 L:
  A10 A2 MOV.N,  A8 $2C L L32R,  A8 CALLX8,
$A9 L:
  RETW.N,
$AB L:  END-CODE

CODE set_rx_gain_table   \ .text.set_rx_gain_table $94
  $0 37 LITERALS
    SYMBOL wifi_bbgain_max  $88 SYMBOL+ rodata  $94 SYMBOL+ rodata
    SYMBOL rfbb_gain_swp  SYMBOL wifi_rx_gain_swp  SYMBOL chip7_sleep_params
    SYMBOL g_phyFuns  SYMBOL chip7_phy_init_ctrl  SYMBOL bt_rx_gain_swp  $20000 T,
    $50 SYMBOL+ phy_rxrf_dc  SYMBOL phy_rxbb_dc  SYMBOL phy_rxrf_dc
    $24 SYMBOL+ phy_rxrf_dc  $3FF5C02C T,  $FFFF80FF T,  $3FF5C0A4 T,
    $FFC07FFF T,  $3FF450DC T,  $800 T,  $1000 T,  $8000000 T,  $10000000 T,
    SYMBOL memcpy  SYMBOL memcpy  SYMBOL ram_gen_rx_gain_table
    SYMBOL ram_gen_rx_gain_table  SYMBOL set_rx_gain_testchip_70
    SYMBOL set_rx_gain_testchip_70  SYMBOL wr_rx_gain_mem  SYMBOL wr_rx_gain_mem
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
$94 L:
  A1 $1A0 ENTRY,  A3 A3 0 8 EXTUI,  A5 12 MOVI.N,  A5 A5 A3 SUB,  A2 A2 0 16 EXTUI,
  A5 A5 0 8 EXTUI,  A4 22 MOVI.N,  A3 3 $AC L BLTUI,
  A4 28 MOVI.N,
$AC L:
  A6 A1 16 ADDI,  A3 A6 $100 ADDMI,  A9 $0 L L32R,  A12 12 MOVI.N,  A10 A3 96 ADDI,
  A7 $200 MOVI,  A11 $4 L L32R,  A4 A9 0 S8I,  MEMW,  A8 $5C L L32R,  A8 CALLX8,
  A10 A3 84 ADDI,  A12 12 MOVI.N,  A11 $8 L L32R,  A8 $60 L L32R,  A8 CALLX8,
  A3 $10 L L32R,  A8 A3 A5 ADD.N,  A8 A8 -1 ADDI.N,  A4 $14 L L32R,  A10 A8 0 L8UI,
  A11 A4 0 L32I.N,  A6 A11 A7 AND,  A9 $C L L32R,  A10 A9 0 S8I,  MEMW,
  A6 $15C L BNEZ,
  A11 A2 MOV.N,  A12 A6 MOV.N,  NOP.N,  NOP.N,  A2 $18 L L32R,  A9 $1C L L32R,
  A2 A2 0 L32I.N,  A10 A9 1 L8UI,  A2 A2 252 L32I,  A2 CALLX8,
  A13 $154 MOVI,  A2 A1 16 ADDI,  A12 $20 L L32R,  A13 A2 A13 ADD.N,  A15 6 MOVI.N,
  A14 9 MOVI.N,  A11 28 MOVI.N,  A10 A2 MOV.N,  A6 A1 0 S32I.N,  A8 $64 L L32R,
  A8 CALLX8,
  A2 A1 16 ADDI,  A13 $160 MOVI,  A15 A6 MOV.N,  A9 $0 L L32R,  A14 A5 MOV.N,
  A13 A2 A13 ADD.N,  A12 A3 MOV.N,  A11 A9 0 L8UI,  A10 A4 177 S8I,  A10 160 MOVI,
  A10 A2 A10 ADD,  A6 A1 0 S32I,  A8 $68 L L32R,  A8 CALLX8,
  A11 A4 0 L32I.N,  A11 A7 A11 OR,  A10 A4 176 S8I,  A11 A4 0 S32I.N,
$15C L:
  A2 $100 MOVI,  A11 A2 $186 L BANY,
  A11 A4 MOV.N,  A15 9 MOVI.N,  NOP.N,  A13 A4 177 L8UI,  A14 A1 16 ADDI,
  A13 A13 1 ADDI.N,  A13 A13 0 8 EXTUI,  A10 1 MOVI.N,  A12 $20 L L32R,
  A8 $6C L L32R,  A8 CALLX8,
  A11 A4 0 L32I.N,  A11 A2 A11 OR,  A11 A4 0 S32I,
$186 L:
  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A2 $24 L L32R,  A10 A11 A2 AND,
  A10 $1BA L BNEZ.N,
  A15 A5 MOV.N,  A13 A4 176 L8UI,  A14 160 MOVI,  A13 A13 1 ADDI.N,  A5 A1 16 ADDI,
  A11 A4 MOV.N,  A14 A5 A14 ADD.N,  A13 A13 0 8 EXTUI,  A12 A3 MOV.N,
  A8 $70 L L32R,  A8 CALLX8,
  A11 A4 0 L32I.N,  A11 A11 A2 OR,  A11 A4 0 S32I,
$1BA L:
  A5 32 MOVI.N,  A2 A11 A5 AND,  A2 $21E L BNEZ,
  A10 1 MOVI.N,  NOP.N,  A8 A4 177 L8UI,  A9 A1 16 ADDI,  A8 A8 A10 ADD.N,
  A8 A8 0 8 EXTUI,  A11 A2 MOV.N,  A6 $28 L L32R,  A15 A6 MOV.N,  A7 $2C L L32R,
  A14 A7 MOV.N,  A13 $30 L L32R,  A12 $20 L L32R,  A9 A1 4 S32I.N,  A8 A1 0 S32I,
  A8 $74 L L32R,  A8 CALLX8,
  A15 A6 MOV.N,  A9 A1 16 ADDI,  A8 160 MOVI,  A6 A4 176 L8UI,  A8 A9 A8 ADD.N,
  A6 A6 1 ADDI.N,  A6 A6 0 8 EXTUI,  A11 A2 MOV.N,  A13 $34 L L32R,  A10 A2 MOV.N,
  A14 A7 MOV.N,  A12 A3 MOV.N,  A8 A1 4 S32I.N,  A6 A1 0 S32I,  A8 $78 L L32R,
  A8 CALLX8,
  A2 A4 0 L32I.N,  A5 A2 A5 OR,  A5 A4 0 S32I,
$21E L:
  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A6 $38 L L32R,  A10 A6 MOV.N,  A8 $7C L L32R,
  A8 CALLX8,
  A2 A4 176 L8UI,  A2 A2 0 7 EXTUI,  A2 A2 8 SLLI,  A3 $3C L L32R,  A10 A10 A3 AND,
  A2 A2 A10 OR,  A5 $40 L L32R,  A10 A5 A5 OR,  MEMW,  A2 A6 0 S32I,  A8 $80 L L32R,
  A8 CALLX8,
  A3 A4 177 L8UI,  A3 A3 0 7 EXTUI,  A3 A3 15 SLLI,  A2 $44 L L32R,  A10 A10 A2 AND,
  A3 A3 A10 OR,  A2 $48 L L32R,  A10 A2 MOV.N,  MEMW,  A3 A5 0 S32I,  A8 $84 L L32R,
  A8 CALLX8,
  A3 $4C L L32R,  A3 A10 A3 OR,  A10 A2 A2 OR,  MEMW,  A3 A2 0 S32I,  A8 $88 L L32R,
  A8 CALLX8,
  A3 $50 L L32R,  A3 A10 A3 OR,  A10 A2 MOV.N,  MEMW,  A3 A2 0 S32I,  A8 $8C L L32R,
  A8 CALLX8,
  A3 $54 L L32R,  A3 A10 A3 OR,  A10 A2 MOV.N,  MEMW,  A3 A2 0 S32I.N,
  A8 $90 L L32R,  A8 CALLX8,
  A3 $58 L L32R,  A10 A10 A3 OR,  MEMW,  A10 A2 0 S32I.N,  RETW.N,
$2C1 L:  END-CODE

CODE rx_blocking_set   \ .text.rx_blocking_set $10
  $0 4 LITERALS
    SYMBOL chip7_sleep_params  $FFFDFDDF T,  $985 T,  SYMBOL set_rx_gain_table
$10 L:
  A1 32 ENTRY,  A9 $0 L L32R,  A11 A2 0 8 EXTUI,  A10 A9 0 L32I.N,  A8 $4 L L32R,
  A8 A10 A8 AND,  A10 $8 L L32R,  A8 A9 0 S32I.N,  A8 $C L L32R,  A8 CALLX8,
  RETW.N,
$2E L:  END-CODE

CODE rx_gain_level   \ .text.rx_gain_level $4
  $0 1 LITERALS
    SYMBOL rx_blocking_set
$4 L:
  A1 32 ENTRY,  A10 A2 0 8 EXTUI,  A10 $10 L BEQZ,
  A10 3 MOVI,
$10 L:
  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$18 L:  END-CODE

CODE txiq_cal_init   \ .text.txiq_cal_init $18
  $0 6 LITERALS
    SYMBOL chip7_sleep_params  $4000 T,  SYMBOL tx_rf_ana_gain
    $4C SYMBOL+ chip7_sleep_params  SYMBOL rfcal_txiq  SYMBOL rfcal_txiq
$18 L:
  A1 64 ENTRY,  A7 $0 L L32R,  A3 $4 L L32R,  A2 A7 0 L32I.N,  A2 A2 A3 AND,
  A2 $F2 L BNEZ,
  A5 16 MOVI.N,  A3 A7 116 L8UI,  A4 4 MOVI.N,  A3 A3 7 SEXT,  A3 A3 43 ADDI,
  A3 A3 A2 MAX,  A3 A3 0 16 EXTUI,  A3 A1 16 S32I.N,
$3E L:
  A3 0 MOVI.N,  A11 A4 MOV.N,  A6 32 MOVI.N,  A8 48 MOVI.N,  A11 A5 A2 MOVNEZ,
  A6 A8 A2 MOVNEZ,  A10 A3 MOV.N,  A9 6 MOVI,  A9 $5E L LOOP,
  A8 A10 6 ADDI.N,  A6 A3 $5C L BBC,
  A10 A8 0 8 EXTUI,
$5C L:
  A3 A3 1 ADDI.N,
$5E L:
  A3 $8 L L32R,  A9 6 MOVI.N,  A13 A3 2 L16UI,  A3 0 MOVI.N,  A14 A13 6 8 EXTUI,
  A12 A3 A3 OR,  A13 A13 0 6 EXTUI,  A9 $7E L LOOP,
  A8 A12 6 ADDI.N,  A13 A3 $7C L BBC,
  A12 A8 0 8 EXTUI,
$7C L:
  A3 A3 1 ADDI.N,
$7E L:
  A3 A14 A12 ADD.N,  A3 A3 0 8 EXTUI,  A8 A1 16 L32I.N,  A3 A10 A3 SUB,
  A3 A3 A8 ADDX4,  A3 A3 15 SEXT,  A3 $9C L BLTZ,
  A14 120 MOVI,  A14 A3 $A5 L BLT,
  A14 A3 MOV.N,  $A7 L J,
$9C L:
  A14 0 MOVI.N,  A3 A14 MOV.N,  $A7 L J,
  2 ZEROS,
$A5 L:
  A3 A14 MOV.N,
$A7 L:
  A12 A2 26 ADDI,  A11 A11 A7 ADDX2,  A2 A2 1 ADDI.N,  A15 0 MOVI.N,  A13 128 MOVI,
  A12 A12 A7 ADDX2,  A11 A11 12 ADDI.N,  A10 A6 MOV.N,  A8 $10 L L32R,  A8 CALLX8,
  A2 2 $C7 L BEQI,
  $3E L J,
$C7 L:
  A8 A3 -40 ADDI,  A4 A6 MOV.N,  A8 A8 15 SEXT,  A6 A2 MOV.N,  A12 $C L L32R,
  A14 0 MOVI.N,  A15 A6 MOV.N,  A14 A8 A14 MAX,  A13 128 MOVI,  A11 A1 MOV.N,
  A10 A4 MOV.N,  A8 $14 L L32R,  A8 CALLX8,
  A2 A7 0 L32I.N,  A3 $4 L L32R,  A2 A2 A3 OR,  A2 A7 0 S32I.N,
$F2 L:
  RETW.N,
$F4 L:  END-CODE

CODE phy_rx11blr_cfg   \ .text.phy_rx11blr_cfg $1C
  $0 7 LITERALS
    $3FF5C860 T,  $800 T,  $FFFFF7FF T,  $3FF5C87C T,
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read
$1C L:
  A1 32 ENTRY,  A3 $0 L L32R,  A2 A2 0 8 EXTUI,  A10 A3 MOV.N,  A8 $10 L L32R,
  A8 CALLX8,
  A5 A2 10 SLLI,  A8 -1025 MOVI,  A4 $400 MOVI,  A10 A10 A8 AND,  A8 A5 A4 AND,
  A10 A8 A10 OR,  MEMW,  A10 A3 0 S32I.N,  A10 A3 MOV.N,  A8 $14 L L32R,  A8 CALLX8,
  A2 A2 11 SLLI,  A4 $8 L L32R,  A10 A10 A4 AND,  A8 $4 L L32R,  A2 A2 A8 AND,
  A8 A10 A2 OR,  A5 $C L L32R,  A10 A5 A5 OR,  MEMW,  A8 A3 0 S32I,  A8 $18 L L32R,
  A8 CALLX8,
  A10 A10 A4 AND,  A2 A10 A2 OR,  MEMW,  A2 A5 0 S32I.N,  RETW.N,
$7D L:  END-CODE

CODE analog_gain_init   \ .text.analog_gain_init $8
  $0 2 LITERALS
    $A0 SYMBOL+ rodata  SYMBOL g_phyFuns
$8 L:
  A1 48 ENTRY,  A2 $4 L L32R,  A3 $0 L L32R,  A12 A2 0 L32I.N,  A9 A3 4 L16UI,
  A8 A3 6 L16UI,  A11 A3 0 L16UI,  A10 A3 2 L16UI,  A3 A12 192 L32I,  A9 A1 4 S16I,
  A8 A1 6 S16I,  A11 A1 0 S16I,  A10 A1 2 S16I,  MEMW,  A3 CALLX8,
  A12 0 MOVI.N,  A11 1 MOVI.N,  A10 4 MOVI.N,  A3 A2 0 L32I.N,  A3 A3 184 L32I,
  A3 CALLX8,
  A12 0 MOVI.N,  A11 1 MOVI.N,  A3 A2 0 L32I.N,  A10 5 MOVI,  A3 A3 184 L32I,
  A3 CALLX8,
  A12 $184 MOVI,  A11 1 MOVI.N,  A3 A2 0 L32I.N,  A10 0 MOVI.N,  A3 A3 184 L32I,
  A3 CALLX8,
  A3 1 MOVI.N,  $6A L J,
$65 L:
  A3 A3 1 ADDI.N,  A3 A3 0 8 EXTUI,
$6A L:
  A8 A2 0 L32I.N,  A11 1 MOVI.N,  A12 $18B MOVI,  A10 A11 MOV.N,  A8 A8 184 L32I,
  A3 A11 $A2 L BEQ,
  A8 CALLX8,
  A10 1 MOVI.N,  A12 0 MOVI.N,  A8 A2 0 L32I.N,  A11 2 MOVI.N,  A8 A8 184 L32I,
  A8 CALLX8,
  A10 A1 MOV.N,  A8 A2 0 L32I,  A8 A8 220 L32I,  A8 CALLX8,
  A3 2 $65 L BNEI,
  A2 A2 0 L32I.N,  A2 A2 196 L32I,  A2 CALLX8,
  RETW.N,
$A2 L:
  A11 A3 MOV.N,  A10 A3 MOV.N,  A12 $189 MOVI,  A8 CALLX8,
  A8 A2 0 L32I.N,  A10 A3 MOV.N,  A12 0 MOVI.N,  A11 2 MOVI.N,  A8 A8 184 L32I,
  A8 CALLX8,
  A10 A1 MOV.N,  A8 A2 0 L32I.N,  A8 A8 220 L32I,  A8 CALLX8,
  $65 L J,
$C7 L:  END-CODE

CODE phy_param_set   \ .text.phy_param_set $8
  $0 2 LITERALS
    SYMBOL wifi_txband  SYMBOL wifi_txband_ht40
$8 L:
  A1 32 ENTRY,  A2 A2 0 8 EXTUI,  A2 $30 L BNEZ.N,
  A2 $0 L L32R,  A8 95 MOVI.N,  A8 A2 0 S8I,  MEMW,  A8 -61 MOVI,  NOP.N,  NOP.N,
  NOP.N,  A2 $4 L L32R,  A8 A2 0 S8I,  MEMW,  RETW.N,
  1 ZEROS,
$30 L:
  A2 $0 L L32R,  A8 75 MOVI.N,  A8 A2 0 S8I,  MEMW,  A8 -106 MOVI,  NOP.N,  NOP.N,
  NOP.N,  A2 $4 L L32R,  A8 A2 0 S8I,  MEMW,  RETW.N,
$4F L:  END-CODE

CODE bb_init   \ .text.bb_init $5C
  $0 23 LITERALS
    SYMBOL chip7_sleep_params  $10000 T,  SYMBOL g_phyFuns  $80000 T,
    SYMBOL tx_rf_ana_gain  $C SYMBOL+ chip7_sleep_params  SYMBOL re_entry
    SYMBOL phy_init_flag  $985 T,  SYMBOL rc_cal  SYMBOL tx_cap_init
    SYMBOL txpwr_offset  SYMBOL tx_pwctrl_init  SYMBOL txiq_cal_init
    SYMBOL set_tx_gain_table  SYMBOL write_txrate_power_offset
    SYMBOL set_tx_gain_table_bt  SYMBOL tsens_read_init  SYMBOL set_rx_gain_table
    SYMBOL analog_gain_init  SYMBOL force_txrxoff  SYMBOL phy_reg_init
    SYMBOL chip_v7_set_chan
$5C L:
  A1 32 ENTRY,  A2 $0 L L32R,  A3 $4 L L32R,  A8 A2 0 L32I.N,  A8 A3 $7C L BANY,
  A8 $8 L L32R,  A8 A8 0 L32I.N,  A8 A8 48 L32I,  A8 CALLX8,
  A8 A2 0 L32I.N,  A3 A8 A3 OR,  A3 A2 0 S32I.N,
$7C L:
  A8 $24 L L32R,  A8 CALLX8,
  A13 A2 0 L32I.N,  A3 $C L L32R,  A13 A13 A3 AND,  A13 $AC L BNEZ,
  A9 $8 L L32R,  A8 $10 L L32R,  A9 A9 0 L32I,  A12 A8 2 L16UI,  A11 A8 0 L16UI,
  A9 A9 $114 L32I,  A10 $14 L L32R,  A9 CALLX8,
  A8 A2 0 L32I.N,  A3 A8 A3 OR,  A3 A2 0 S32I.N,
$AC L:
  A8 $28 L L32R,  A8 CALLX8,
  A10 0 MOVI.N,  A8 $2C L L32R,  A8 CALLX8,
  A10 1 MOVI.N,  A8 $30 L L32R,  A8 CALLX8,
  A8 $34 L L32R,  A8 CALLX8,
  A3 $18 L L32R,  A3 A3 0 L8UI,  A3 $D9 L BEQZ.N,
  A3 $1C L L32R,  A3 A3 0 L8UI,  A3 $F7 L BNEZ,
$D9 L:
  A3 $10 L L32R,  A11 A3 2 L16UI,  A10 A3 0 L16UI,  A8 $38 L L32R,  A8 CALLX8,
  A10 0 MOVI,  A8 $3C L L32R,  A8 CALLX8,
  A8 $40 L L32R,  A8 CALLX8,
$F7 L:
  A8 $44 L L32R,  A8 CALLX8,
  A10 $20 L L32R,  A11 3 MOVI.N,  A8 $48 L L32R,  A8 CALLX8,
  A8 $4C L L32R,  A8 CALLX8,
  A10 0 MOVI.N,  A8 $50 L L32R,  A8 CALLX8,
  A8 $54 L L32R,  A8 CALLX8,
  A8 -384 MOVI,  A11 0 MOVI.N,  A10 11 MOVI.N,  A8 A2 78 S16I,  A8 A2 178 S16I,
  MEMW,  A8 $58 L L32R,  A8 CALLX8,
  RETW.N,
$134 L:  END-CODE

CODE wifi_rifs_mode_en   \ .text.wifi_rifs_mode_en $8
  $0 2 LITERALS
    $3FF5C0F4 T,  SYMBOL esp_dport_access_reg_read
$8 L:
  A1 32 ENTRY,  A3 $0 L L32R,  A2 A2 0 8 EXTUI,  A10 A3 MOV.N,  A8 $4 L L32R,
  A8 CALLX8,
  A8 -2 MOVI.N,  A10 A10 A8 AND,  A2 A2 0 1 EXTUI,  A2 A10 A2 OR,  MEMW,
  A2 A3 0 S32I.N,  RETW.N,
$2B L:  END-CODE

CODE phy_chan_filt_set   \ .text.phy_chan_filt_set $48
  $0 18 LITERALS
    $3FF5CC0C T,  $FFDFFFFF T,  $FFBFFFFF T,  $200000 T,  $400000 T,  $3FF5CD08 T,
    $3FF5CD04 T,  $3FF5C074 T,  $FFFFDFFF T,  $2000 T,
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
$48 L:
  A1 32 ENTRY,  A2 A2 0 8 EXTUI,  A3 A3 0 8 EXTUI,  A2 $85 L BEQZ,
  A2 $0 L L32R,  A10 A2 A2 OR,  A8 $28 L L32R,  A8 CALLX8,
  A8 $4 L L32R,  A8 A10 A8 AND,  A10 A2 MOV.N,  MEMW,  A8 A2 0 S32I,  A8 $2C L L32R,
  A8 CALLX8,
  A8 $8 L L32R,  A10 A10 A8 AND,  MEMW,  A10 A2 0 S32I.N,  $D8 L J,
  3 ZEROS,
$85 L:
  A2 $0 L L32R,  A10 A2 A2 OR,  A8 $30 L L32R,  A8 CALLX8,
  A8 $C L L32R,  A8 A10 A8 OR,  A10 A2 MOV.N,  MEMW,  A8 A2 0 S32I.N,
  A8 $34 L L32R,  A8 CALLX8,
  A8 $10 L L32R,  A8 A10 A8 OR,  A5 $14 L L32R,  A10 A5 MOV.N,  MEMW,
  A8 A2 0 S32I.N,  A8 $38 L L32R,  A8 CALLX8,
  A2 -8 MOVI.N,  A8 A10 A2 AND,  A4 $18 L L32R,  A10 A4 MOV.N,  MEMW,  A8 A5 0 S32I,
  A8 $3C L L32R,  A8 CALLX8,
  A10 A10 A2 AND,  MEMW,  A10 A4 0 S32I.N,
$D8 L:
  A3 $FD L BEQZ.N,
  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A3 $1C L L32R,  A10 A3 MOV.N,  A8 $40 L L32R,
  A8 CALLX8,
  A2 $20 L L32R,  A10 A10 A2 AND,  MEMW,  A10 A3 0 S32I.N,  RETW.N,
  3 ZEROS,
$FD L:
  A3 $1C L L32R,  A10 A3 MOV.N,  A8 $44 L L32R,  A8 CALLX8,
  A2 $24 L L32R,  A10 A10 A2 OR,  MEMW,  A10 A3 0 S32I.N,  RETW.N,
$115 L:  END-CODE

CODE phy_get_tx_rate   \ .text.phy_get_tx_rate $8
  $0 2 LITERALS
    $3FF4E060 T,  SYMBOL esp_dport_access_reg_read
$8 L:
  A1 32 ENTRY,  A10 $0 L L32R,  A8 $4 L L32R,  A8 CALLX8,
  A2 A10 24 7 EXTUI,  RETW.N,
$19 L:  END-CODE

CODE register_chipv7_phy_init_param   \ .text.register_chipv7_phy_init_param $14
  $0 5 LITERALS
    SYMBOL chip7_phy_init_ctrl  $E SYMBOL+ chip7_phy_init_ctrl
    $20 SYMBOL+ chip7_phy_init_ctrl  $50 SYMBOL+ chip7_phy_init_ctrl
    SYMBOL rtc_get_xtal
$14 L:
  A1 32 ENTRY,  A3 $0 L L32R,  A8 A2 0 L8UI,  A4 A2 1 L8UI,  A8 A3 0 S8I,  MEMW,
  A4 3 $48 L BNEI,
  A8 $10 L L32R,  A8 CALLX8,
  A10 A10 0 8 EXTUI,  A8 24 MOVI.N,  A4 2 MOVI.N,  A10 A8 $48 L BEQ,
  A10 A10 -26 ADDI,  A4 0 MOVI,  A8 1 MOVI,  A4 A8 A10 MOVEQZ,  A4 A4 0 8 EXTUI,
$48 L:
  A4 A3 1 S8I,  MEMW,  A10 A2 32 ADDI,  A11 A3 MOV.N,  A13 A2 35 ADDI,  A9 A3 A3 OR,
  A12 -30 MOVI,
$5C L:
  A8 A10 0 L8UI,  A9 A9 1 ADDI.N,  A10 A10 1 ADDI.N,  A8 A9 1 S8I,  MEMW,  NOP.N,
  NOP.N,  NOP.N,  NOP.N,  A4 A10 2 L8UI,  A4 A8 A4 ADD.N,  A4 A9 4 S8I,
  A12 A9 7 S8I,  MEMW,  A10 A13 $5C L BNE,
  A10 A2 44 ADDI,  A8 A2 50 ADDI,  A9 6 MOVI.N,  NOP,  A14 A2 41 L8UI,
  A13 A2 42 L8UI,  A12 A2 43 L8UI,  A4 $4 L L32R,  A14 A3 11 S8I,  A13 A3 12 S8I,
  A12 A3 13 S8I,  MEMW,  A9 $BD L LOOP,
  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A12 A10 0 L8UI,  A10 A10 1 ADDI.N,  A12 A4 0 S8I,
  MEMW,  A4 A4 1 ADDI.N,
$BD L:
  A4 A2 58 ADDI,  A9 A11 MOV.N,  A4 A4 A8 SUB,  A4 $D5 L LOOP,
  A10 A8 0 L8UI,  A9 A9 1 ADDI.N,  A8 A8 1 ADDI.N,  A10 A9 19 S8I,  MEMW,
$D5 L:
  A9 A2 62 ADDI,  A8 14 MOVI.N,  NOP.N,  NOP,  A14 A2 58 L8UI,  A13 A2 59 L8UI,
  A12 A2 60 L8UI,  A10 A2 61 L8UI,  A4 $8 L L32R,  A14 A3 28 S8I,  A13 A3 29 S8I,
  A12 A3 30 S8I,  A10 A3 31 S8I,  MEMW,  A8 $115 L LOOP,
  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A10 A9 0 L8UI,  A9 A9 1 ADDI.N,  A10 A4 0 S8I,
  MEMW,  A4 A4 1 ADDI.N,
$115 L:
  A9 A2 76 ADDI,  A8 A11 MOV.N,  A4 5 MOVI,  A4 $12D L LOOP,
  A10 A9 0 L8UI,  A8 A8 1 ADDI.N,  A9 A9 1 ADDI.N,  A10 A8 45 S8I,  MEMW,
$12D L:
  A8 A2 82 ADDI,  A4 4 MOVI.N,  NOP.N,  NOP.N,  A9 A2 81 L8UI,  A9 A3 51 S8I,  MEMW,
  A4 $155 L LOOP,
  A11 A11 1 ADDI.N,  NOP.N,  NOP.N,  NOP.N,  A9 A8 0 L8UI,  A8 A8 1 ADDI.N,
  A9 A11 51 S8I,  MEMW,
$155 L:
  A10 A2 107 ADDI,  A9 14 MOVI.N,  NOP.N,  NOP.N,  A11 A2 103 L8UI,
  A11 A11 0 7 EXTUI,  A8 A11 8 SLLI,  A4 A2 90 L8UI,  A4 A4 7 SEXT,  A4 A4 8 SLLI,
  A11 A2 91 L8UI,  A4 A4 A11 OR,  A15 A2 93 L8UI,  A15 A15 0 7 EXTUI,
  A15 A15 8 SLLI,  A11 A2 94 L8UI,  A15 A15 A11 OR,  A14 A2 95 L8UI,
  A14 A14 7 SEXT,  A14 A14 8 SLLI,  A11 A2 96 L8UI,  A14 A14 A11 OR,
  A13 A2 98 L8UI,  A13 A13 0 7 EXTUI,  A13 A13 8 SLLI,  A5 A2 92 L8UI,
  A11 A2 99 L8UI,  A13 A13 A11 OR,  A12 A2 100 L8UI,  A12 A12 7 SEXT,
  A12 A12 8 SLLI,  A5 A3 62 S8I,  MEMW,  NOP.N,  NOP.N,  NOP.N,  NOP.N,
  A5 A2 97 L8UI,  A11 A2 101 L8UI,  A12 A12 A11 OR,  A5 A3 68 S8I,  MEMW,  NOP.N,
  NOP.N,  NOP.N,  NOP.N,  A5 A2 102 L8UI,  A11 A2 104 L8UI,  A11 A8 A11 OR,
  A5 A3 74 S8I,  MEMW,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A5 A2 105 L8UI,
  A2 A2 106 L8UI,  A8 $C L L32R,  A5 A3 78 S8I,  A4 A3 60 S16I,  A15 A3 64 S16I,
  A14 A3 66 S16I,  A13 A3 70 S16I,  A12 A3 72 S16I,  A11 A3 76 S16I,  A2 A3 79 S8I,
  MEMW,  A9 $227 L LOOP,
  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A2 A10 0 L8UI,  A10 A10 1 ADDI.N,  A2 A8 0 S8I,
  MEMW,  A8 A8 1 ADDI.N,
$227 L:
  RETW.N,
$229 L:  END-CODE

CODE uart_wait_idle   \ .text.uart_wait_idle $10
  $0 4 LITERALS
    $3FF4 T,  $FF0000 T,  SYMBOL esp_dport_access_reg_read  SYMBOL ets_delay_us
$10 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A2 0 8 EXTUI,  A2 A2 A8 ADD.N,  A2 A2 16 SLLI,
  A2 A2 28 ADDI,  A3 $4 L L32R,
$24 L:
  A10 A2 MOV.N,  A8 $8 L L32R,  A8 CALLX8,
  A10 A3 $24 L BANY,
  A10 100 MOVI,  A8 $C L L32R,  A8 CALLX8,
  RETW.N,
$3A L:  END-CODE

CODE phy_get_romfunc_addr   \ .text.phy_get_romfunc_addr $BC
  $0 47 LITERALS
    SYMBOL g_phyFuns  SYMBOL ram_pbus_force_mode  SYMBOL ram_rfpll_reset
    SYMBOL ram_restart_cal  SYMBOL ram_wait_rfpll_cal_end
    SYMBOL ram_phy_get_vdd33  SYMBOL ram_set_chan_cal_interp
    SYMBOL ram_pbus_rx_dco_cal  SYMBOL ram_txdc_cal_v70  SYMBOL ram_spur_coef_cfg
    SYMBOL ram_set_pbus_mem  SYMBOL ram_write_gain_mem  SYMBOL ram_bb_bss_bw_40_en
    SYMBOL ram_bb_tx_ht20_cen  SYMBOL ram_bb_bss_cbw40_dig  SYMBOL ram_cbw2040_cfg
    SYMBOL ram_check_noise_floor  SYMBOL ram_phy_get_noisefloor
    SYMBOL ram_set_noise_floor  SYMBOL ram_disable_agc  SYMBOL ram_enable_agc
    SYMBOL ram_dc_iq_est  SYMBOL ram_tx_pwctrl_bg_init  SYMBOL ram_start_tx_tone
    SYMBOL ram_get_fm_sar_dout  SYMBOL ram_txiq_get_mis_pwr
    SYMBOL ram_iq_est_enable  SYMBOL ram_iq_est_disable  SYMBOL ram_rfcal_pwrctrl
    SYMBOL ram_en_pwdet  SYMBOL ram_read_sar_dout  SYMBOL ram_pbus_xpd_tx_on
    SYMBOL ram_rfcal_txcap  SYMBOL ram_txbbgain_to_index
    SYMBOL ram_index_to_txbbgain  SYMBOL ram_chip_i2c_readReg
    SYMBOL ram_chip_i2c_writeReg  SYMBOL ram_txcal_work_mode
    SYMBOL ram_set_txcap_reg  SYMBOL ram_meas_tone_pwr_db
    SYMBOL ram_pbus_debugmode  SYMBOL ram_pbus_force_test  SYMBOL ram_set_txclk_en
    SYMBOL ram_set_rxclk_en  SYMBOL ram_rfpll_set_freq  SYMBOL ram_get_rf_gain_qdb
    SYMBOL phy_get_romfuncs
$BC L:
  A1 32 ENTRY,  A8 $B8 L L32R,  A8 CALLX8,
  A8 $0 L L32R,  A10 A8 0 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A8 $4 L L32R,
  A8 A10 172 S32I,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A8 $8 L L32R,  A8 A10 224 S32I,
  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A8 $C L L32R,  A8 A10 228 S32I,  NOP.N,  NOP.N,
  NOP.N,  NOP.N,  A8 $10 L L32R,  A8 A10 236 S32I,  NOP.N,  NOP.N,  NOP.N,  NOP.N,
  A8 $14 L L32R,  A8 A10 $198 S32I,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A8 $18 L L32R,
  A8 A10 $168 S32I,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A8 $1C L L32R,
  A8 A10 $150 S32I,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A8 $20 L L32R,
  A8 A10 $118 S32I,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A8 $24 L L32R,
  A8 A10 100 S32I,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A8 $28 L L32R,
  A8 A10 48 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A8 $2C L L32R,
  A8 A10 52 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A8 $30 L L32R,  A8 A10 80 S32I,
  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A8 $34 L L32R,  A8 A10 92 S32I,  NOP.N,  NOP.N,
  NOP.N,  NOP.N,  A8 $38 L L32R,  A8 A10 112 S32I,  NOP.N,  NOP.N,  NOP.N,  NOP.N,
  A8 $3C L L32R,  A8 A10 116 S32I,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A9 $4C L L32R,
  A8 $50 L L32R,  A8 A10 12 S32I.N,  A8 A10 4 S32I.N,  NOP.N,  NOP.N,  NOP.N,
  NOP.N,  A8 $54 L L32R,  A8 A10 $14C S32I,  NOP.N,  NOP.N,  NOP.N,  NOP.N,
  A8 $58 L L32R,  A8 A10 $1A4 S32I,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A8 $5C L L32R,
  A8 A10 72 S32I,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A8 $60 L L32R,  A8 A10 $130 S32I,
  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A8 $64 L L32R,  A8 A10 $138 S32I,  NOP.N,  NOP.N,
  NOP.N,  NOP.N,  A8 $68 L L32R,  A8 A10 $144 S32I,  NOP.N,  NOP.N,  NOP.N,  NOP.N,
  A8 $6C L L32R,  A11 $40 L L32R,  A8 A10 $148 S32I,  NOP.N,  NOP.N,  NOP.N,  NOP.N,
  A8 $70 L L32R,  A11 A10 128 S32I,  A8 A10 $184 S32I,  NOP.N,  NOP.N,  NOP.N,
  NOP.N,  A11 $44 L L32R,  A8 $74 L L32R,  A11 A10 124 S32I,  A8 A10 $11C S32I,
  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A11 $48 L L32R,  A8 $78 L L32R,  A11 A10 132 S32I,
  A9 A10 8 S32I.N,  A9 A10 0 S32I.N,  A8 A10 $12C S32I,  NOP.N,  NOP.N,  NOP.N,
  NOP.N,  A8 $7C L L32R,  A8 A10 216 S32I,  NOP.N,  NOP.N,  NOP.N,  NOP.N,
  A8 $80 L L32R,  A8 A10 $170 S32I,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A8 $84 L L32R,
  A8 A10 $10C S32I,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A8 $88 L L32R,
  A8 A10 $110 S32I,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A8 $8C L L32R,
  A8 A10 148 S32I,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A8 $90 L L32R,  A8 A10 156 S32I,
  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A8 $94 L L32R,  A8 A10 $124 S32I,  NOP.N,  NOP.N,
  NOP.N,  NOP.N,  A8 $98 L L32R,  A8 A10 $16C S32I,  NOP.N,  NOP.N,  NOP.N,  NOP.N,
  A8 $9C L L32R,  A8 A10 $17C S32I,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A8 $A0 L L32R,
  A8 A10 192 S32I,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A8 $A4 L L32R,  A8 A10 184 S32I,
  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A8 $A8 L L32R,  A8 A10 60 S32I.N,  NOP.N,  NOP.N,
  NOP.N,  NOP.N,  A8 $AC L L32R,  A8 A10 64 S32I,  NOP.N,  NOP.N,  NOP.N,  NOP.N,
  A8 $B0 L L32R,  A8 A10 240 S32I,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A8 $B4 L L32R,
  A8 A10 $190 S32I,  RETW.N,
$321 L:  END-CODE

CODE phy_byte_to_word   \ .text.phy_byte_to_word $0
  $0 ORIGIN
  A1 32 ENTRY,  A8 A2 1 L8UI,  A8 A8 8 SLLI,  A9 A2 0 L8UI,  A9 A8 A9 OR,
  A8 A2 2 L8UI,  A8 A8 16 SLLI,  A8 A8 A9 OR,  A2 A2 3 L8UI,  A2 A2 24 SLLI,
  A2 A2 A8 OR,  RETW.N,
$23 L:  END-CODE

CODE rf_cal_data_recovery   \ .text.rf_cal_data_recovery $C
  $0 3 LITERALS
    SYMBOL chip7_sleep_params  SYMBOL phy_rxrf_dc  SYMBOL phy_rxbb_dc
$C L:
  A1 32 ENTRY,  A11 $40C MOVI,  A12 $0 L L32R,  A11 A2 A11 ADD.N,  A13 49 MOVI.N,
  A13 $40 L LOOP,
  A9 A11 1 L8UI,  A9 A9 8 SLLI,  A8 A11 0 L8UI,  A10 A9 A8 OR,  A8 A11 2 L8UI,
  A8 A8 16 SLLI,  A9 A8 A10 OR,  A8 A11 3 L8UI,  A11 A11 4 ADDI.N,  A8 A8 24 SLLI,
  A8 A8 A9 OR,  A8 A12 0 S32I.N,  A12 A12 4 ADDI.N,
$40 L:
  A11 $4D0 MOVI,  NOP.N,  NOP.N,  A12 $4 L L32R,  A11 A2 A11 ADD.N,  A13 34 MOVI.N,
  A13 $75 L LOOP,
  A9 A11 1 L8UI,  A9 A9 8 SLLI,  A8 A11 0 L8UI,  A10 A9 A8 OR,  A8 A11 2 L8UI,
  A8 A8 16 SLLI,  A9 A8 A10 OR,  A8 A11 3 L8UI,  A11 A11 4 ADDI.N,  A8 A8 24 SLLI,
  A8 A8 A9 OR,  A8 A12 0 S32I.N,  A12 A12 4 ADDI.N,
$75 L:
  A8 $558 MOVI,  NOP.N,  NOP.N,  A11 $8 L L32R,  A2 A2 A8 ADD,  A12 4 MOVI,
  A12 $AC L LOOP,
  A9 A2 1 L8UI,  A9 A9 8 SLLI,  A8 A2 0 L8UI,  A10 A9 A8 OR,  A8 A2 2 L8UI,
  A8 A8 16 SLLI,  A9 A8 A10 OR,  A8 A2 3 L8UI,  A2 A2 4 ADDI.N,  A8 A8 24 SLLI,
  A8 A8 A9 OR,  A8 A11 0 S32I.N,  A11 A11 4 ADDI.N,
$AC L:
  RETW.N,
$AE L:  END-CODE

CODE rf_cal_data_backup   \ .text.rf_cal_data_backup $2C
  $0 11 LITERALS
    SYMBOL phy_freq_wifi_only  $3FF4E0C4 T,  $3FF4E0C0 T,
    SYMBOL chip7_sleep_params  $C4 SYMBOL+ chip7_sleep_params  SYMBOL phy_rxrf_dc
    $88 SYMBOL+ phy_rxrf_dc  SYMBOL phy_rxbb_dc  $10 SYMBOL+ phy_rxbb_dc
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
$2C L:
  A1 48 ENTRY,  A4 $0 L L32R,  A3 12 MOVI.N,  A5 14 MOVI.N,  A6 -256 MOVI,
  A7 A4 0 L8UI,  A3 A1 0 S32I.N,  A3 85 MOVI.N,  A3 A5 A7 MOVNEZ,  A5 A7 MOV.N,
  A2 A1 4 S32I.N,  A2 0 MOVI.N,  A3 A1 8 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,
  A3 $4 L L32R,  $5F L J,
$59 L:
  A8 $0 L L32R,  A5 A8 0 L8UI,
$5F L:
  A5 $7C L BEQZ.N,
  A4 12 MOVI.N,  A4 A2 $8D L BLT,
  A5 A2 1 ADDI.N,  A8 A1 0 L32I.N,  A14 A8 1 ADDI.N,  A14 A14 A14 ADDX2,
  A13 A8 A8 ADDX2,  A14 A14 2 SLLI,  A4 A13 0 8 EXTUI,  $95 L J,
  1 ZEROS,
$7C L:
  A5 A2 1 ADDI.N,  A14 A5 A5 ADDX2,  A2 A2 A2 ADDX2,  A14 A14 2 SLLI,
  A4 A2 0 8 EXTUI,  $95 L J,
$8D L:
  A4 252 MOVI,  A14 $3FC MOVI,  A5 A2 1 ADDI.N,
$95 L:
  A8 A1 4 L32I.N,  A2 A4 3 ADDI.N,  A7 A8 A14 ADD.N,  A2 A2 0 8 EXTUI,
$9E L:
  A10 A3 MOV.N,  A8 $24 L L32R,  A8 CALLX8,
  A10 A10 A6 AND,  A10 A10 A4 OR,  MEMW,  A10 A3 0 S32I.N,  NOP.N,  NOP.N,  NOP.N,
  NOP.N,  A10 $8 L L32R,  A8 $28 L L32R,  A8 CALLX8,
  A11 A7 MOV.N,  A9 0 MOVI.N,  A8 4 MOVI,  A8 $DC L LOOP,
  A9 SSR,  A12 A10 SRA,  A12 A11 0 S8I,  MEMW,  A9 A9 8 ADDI.N,  A11 A11 1 ADDI.N,
$DC L:
  A4 A4 1 ADDI.N,  A4 A4 0 8 EXTUI,  A7 A7 4 ADDI.N,  A4 A2 $9E L BNE,
  A8 A1 0 L32I.N,  A2 A5 MOV.N,  A4 A8 5 ADDI.N,  A4 A4 0 8 EXTUI,  A4 A1 0 S32I.N,
  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A4 A1 8 L32I.N,  A5 A4 $101 L BEQ,
  $59 L J,
$101 L:
  A8 A1 4 L32I.N,  A7 $40C MOVI,  A7 A8 A7 ADD.N,  A6 $C L L32R,  A8 $10 L L32R,
$10E L:
  A5 A7 MOV.N,  A4 0 MOVI.N,  A3 4 MOVI,  A3 $12A L LOOP,
  A2 A6 0 L32I.N,  A4 SSR,  A2 A2 SRA,  A4 A4 8 ADDI.N,  A2 A5 0 S8I,  MEMW,
  A5 A5 1 ADDI.N,
$12A L:
  A6 A6 4 ADDI.N,  A7 A7 4 ADDI.N,  A6 A8 $10E L BNE,
  $155 L J,
$134 L:
  A2 A6 0 L32I.N,  A4 SSR,  A2 A2 SRL,  A4 A4 8 ADDI.N,  A2 A5 0 S8I,  MEMW,
  A5 A5 1 ADDI.N,  A3 A3 -1 ADDI.N,  A3 $134 L BNEZ,
  A6 A6 4 ADDI.N,  A7 A7 4 ADDI.N,  A6 A8 $162 L BNE,
  $18B L J,
$155 L:
  A2 A1 4 L32I.N,  A7 $4D0 MOVI,  A7 A2 A7 ADD.N,  A6 $14 L L32R,  A8 $18 L L32R,
$162 L:
  A5 A7 MOV.N,  A4 0 MOVI.N,  A3 4 MOVI.N,  $134 L J,
$16B L:
  A2 A6 0 L32I.N,  A4 SSR,  A2 A2 SRL,  A4 A4 8 ADDI.N,  A2 A5 0 S8I,  MEMW,
  A5 A5 1 ADDI.N,  A3 A3 -1 ADDI.N,  A3 $16B L BNEZ,
  A6 A6 4 ADDI.N,  A7 A7 4 ADDI.N,  A6 A8 $198 L BNE,
  RETW.N,
$18B L:
  A3 A1 4 L32I.N,  A7 $558 MOVI,  A7 A3 A7 ADD.N,  A6 $1C L L32R,  A8 $20 L L32R,
$198 L:
  A5 A7 MOV.N,  A4 0 MOVI.N,  A3 4 MOVI.N,  $16B L J,
$1A1 L:  END-CODE

CODE phy_get_rf_cal_version   \ .text.phy_get_rf_cal_version $4
  $0 1 LITERALS
    $12FF T,
$4 L:
  A1 32 ENTRY,  A2 $0 L L32R,  RETW.N,
$C L:  END-CODE

CODE phy_rfcal_data_check   \ .text.phy_rfcal_data_check $0
  $0 ORIGIN
  A1 32 ENTRY,  A9 -1 MOVI.N,  A8 0 MOVI.N,  A9 A3 0 S8I,  A9 18 MOVI.N,
  A2 A2 0 8 EXTUI,  A11 A3 MOV.N,  A13 0 MOVI.N,  A12 $15A MOVI,  A9 A3 1 S8I,
  A8 A3 2 S8I,  A8 A3 3 S8I,  MEMW,  A12 $4F L LOOP,
  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A9 A11 1 L8UI,  A9 A9 8 SLLI,  A8 A11 0 L8UI,
  A10 A9 A8 OR,  A8 A11 2 L8UI,  A8 A8 16 SLLI,  A9 A8 A10 OR,  A8 A11 3 L8UI,
  A11 A11 4 ADDI.N,  A8 A8 24 SLLI,  A8 A8 A9 OR,  A13 A13 A8 ADD.N,
$4F L:
  A11 32 MOVI.N,  A11 $76 L LOOP,
  A9 A4 1 L8UI,  A9 A9 8 SLLI,  A8 A4 0 L8UI,  A10 A9 A8 OR,  A8 A4 2 L8UI,
  A8 A8 16 SLLI,  A9 A8 A10 OR,  A8 A4 3 L8UI,  A4 A4 4 ADDI.N,  A8 A8 24 SLLI,
  A8 A8 A9 OR,  A13 A13 A8 ADD.N,
$76 L:
  A3 A3 $500 ADDMI,  A4 A3 105 L8UI,  A8 A4 8 SLLI,  A4 -1 MOVI.N,  A13 A4 A13 XOR,
  A4 A3 104 L8UI,  A4 A8 A4 OR,  A8 A3 106 L8UI,  A8 A8 16 SLLI,  A4 A8 A4 OR,
  A8 A3 107 L8UI,  A8 A8 24 SLLI,  A4 A8 A4 OR,  A2 $AD L BEQZ.N,
  A4 A13 A4 SUB,  A3 1 MOVI.N,  A2 0 MOVI.N,  A2 A3 A4 MOVNEZ,  A2 A2 0 8 EXTUI,
  RETW.N,
$AD L:
  A9 A13 8 SRLI,  A8 A13 16 16 EXTUI,  A4 A13 24 8 EXTUI,  A13 A3 104 S8I,
  A9 A3 105 S8I,  A8 A3 106 S8I,  A4 A3 107 S8I,  MEMW,  RETW.N,
$C7 L:  END-CODE

CODE i2cmst_reg_init   \ .text.i2cmst_reg_init $40
  $0 16 LITERALS
    $3FF4E0C4 T,  $FFF0FFFF T,  $20000 T,  $FF0FFFFF T,  $400000 T,  $1000000 T,
    $40000000 T,  $DFFFFFFF T,  $FFFF83FF T,  $2C00 T,
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
$40 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A10 A2 MOV.N,  A8 $28 L L32R,  A8 CALLX8,
  A8 $4 L L32R,  A9 A10 A8 AND,  A10 A2 MOV.N,  A8 $8 L L32R,  A8 A9 A8 OR,  MEMW,
  A8 A2 0 S32I,  A8 $2C L L32R,  A8 CALLX8,
  A8 $C L L32R,  A9 A10 A8 AND,  A10 A2 MOV.N,  A8 $10 L L32R,  A8 A9 A8 OR,  MEMW,
  A8 A2 0 S32I.N,  A8 $30 L L32R,  A8 CALLX8,
  A8 $14 L L32R,  A8 A10 A8 OR,  A10 A2 MOV.N,  MEMW,  A8 A2 0 S32I,  A8 $34 L L32R,
  A8 CALLX8,
  A8 $18 L L32R,  A8 A10 A8 OR,  A10 A2 MOV.N,  MEMW,  A8 A2 0 S32I,  A8 $38 L L32R,
  A8 CALLX8,
  A8 $1C L L32R,  A8 A10 A8 AND,  A10 A2 MOV.N,  MEMW,  A8 A2 0 S32I.N,
  A8 $3C L L32R,  A8 CALLX8,
  A8 $20 L L32R,  A10 A10 A8 AND,  A8 $24 L L32R,  A10 A10 A8 OR,  MEMW,
  A10 A2 0 S32I.N,  RETW.N,
$CF L:  END-CODE

CODE fe_reg_init   \ .text.fe_reg_init $54
  $0 21 LITERALS
    $3FF460A0 T,  $1000000 T,  $3FF45114 T,  $3FF450DC T,  $C0000000 T,
    $3FF450D8 T,  $2000000 T,  $3FF460B8 T,  $20000000 T,  $3FF4609C T,  $4000 T,
    $3FF4604C T,  $FFFF00FF T,  $C800 T,  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
$54 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A10 A2 MOV.N,  A8 $38 L L32R,  A8 CALLX8,
  A8 $4 L L32R,  A8 A10 A8 OR,  A3 $8 L L32R,  A10 A3 MOV.N,  MEMW,  A8 A2 0 S32I.N,
  A8 $3C L L32R,  A8 CALLX8,
  A8 $200 MOVI,  A8 A10 A8 OR,  A2 $C L L32R,  A10 A2 A2 OR,  MEMW,  A8 A3 0 S32I,
  A8 $40 L L32R,  A8 CALLX8,
  A8 $10 L L32R,  A8 A10 A8 OR,  A3 $14 L L32R,  A10 A3 A3 OR,  MEMW,  A8 A2 0 S32I,
  A8 $44 L L32R,  A8 CALLX8,
  A8 $18 L L32R,  A8 A10 A8 OR,  A2 $1C L L32R,  A10 A2 A2 OR,  MEMW,  A8 A3 0 S32I,
  A8 $48 L L32R,  A8 CALLX8,
  A8 $20 L L32R,  A8 A10 A8 OR,  A3 $24 L L32R,  A10 A3 A3 OR,  MEMW,  A8 A2 0 S32I,
  A8 $4C L L32R,  A8 CALLX8,
  A8 $28 L L32R,  A8 A10 A8 OR,  A2 $2C L L32R,  A10 A2 A2 OR,  MEMW,  A8 A3 0 S32I,
  A8 $50 L L32R,  A8 CALLX8,
  A8 $30 L L32R,  A10 A10 A8 AND,  A8 $34 L L32R,  A10 A10 A8 OR,  MEMW,
  A10 A2 0 S32I.N,  RETW.N,
$103 L:  END-CODE

CODE reg_init_begin   \ .phyiram.12 $20
  $0 8 LITERALS
    SYMBOL g_phyFuns  $3FF48030 T,  $F8000000 T,  $3FF66000 T,
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL fe_reg_init  SYMBOL i2cmst_reg_init
$20 L:
  A1 32 ENTRY,  A3 $0 L L32R,  A13 0 MOVI.N,  A12 12 MOVI,  A11 4 MOVI,
  A10 102 MOVI,  A3 A3 0 L32I,  A2 A2 0 8 EXTUI,  A3 A3 160 L32I,  A3 CALLX8,
  A4 $4 L L32R,  A10 A4 A4 OR,  A8 $10 L L32R,  A8 CALLX8,
  A8 $8 L L32R,  A8 A10 A8 OR,  A3 $C L L32R,  A10 A3 A3 OR,  MEMW,  A8 A4 0 S32I,
  A8 $14 L L32R,  A8 CALLX8,
  A8 $400 MOVI,  A10 A10 A8 OR,  MEMW,  A10 A3 0 S32I.N,  A2 $7A L BNEZ.N,
  A8 $18 L L32R,  A8 CALLX8,
  A8 $1C L L32R,  A8 CALLX8,
$7A L:
  RETW.N,
$7C L:  END-CODE

CODE phy_wakeup_init   \ .phyiram.17 $54
  $0 21 LITERALS
    SYMBOL phy_init_flag  SYMBOL force_hw_set_freq_flag  $3FF460A0 T,
    $FFFFF0FF T,  $A00 T,  SYMBOL g_phyFuns  SYMBOL init_wifi_disable
    $3FF5C080 T,  SYMBOL phy_enter_critical  SYMBOL esp_dport_access_reg_read
    SYMBOL phy_dis_hw_set_freq  SYMBOL i2c_master_reset  SYMBOL reg_init_begin
    SYMBOL phy_i2c_init  SYMBOL phy_reg_init  SYMBOL set_chan_reg
    SYMBOL esp_dport_access_reg_read  SYMBOL enable_wifi_agc
    SYMBOL phy_en_hw_set_freq  SYMBOL esp_dport_access_reg_read
    SYMBOL phy_exit_critical
$54 L:
  A1 32 ENTRY,  A8 $20 L L32R,  A8 CALLX8,
  A2 $8 L L32R,  A3 1 MOVI.N,  A6 A10 A10 OR,  A10 A2 A2 OR,  A5 $0 L L32R,
  A4 $4 L L32R,  A3 A5 0 S8I,  A3 A4 0 S8I,  MEMW,  A8 $24 L L32R,  A8 CALLX8,
  A3 $C L L32R,  A10 A10 A3 AND,  A8 $10 L L32R,  A10 A10 A8 OR,  MEMW,
  A10 A2 0 S32I.N,  A8 $28 L L32R,  A8 CALLX8,
  A8 $2C L L32R,  A8 CALLX8,
  A10 1 MOVI.N,  A8 $30 L L32R,  A8 CALLX8,
  A8 $34 L L32R,  A8 CALLX8,
  A8 $38 L L32R,  A8 CALLX8,
  A8 $3C L L32R,  A8 CALLX8,
  A10 A2 MOV.N,  A8 $40 L L32R,  A8 CALLX8,
  A3 A10 A3 AND,  MEMW,  A3 A2 0 S32I.N,  A12 0 MOVI.N,  A11 A12 MOV.N,
  A10 99 MOVI,  NOP.N,  A2 $14 L L32R,  A2 A2 0 L32I,  A2 A2 152 L32I,  A2 CALLX8,
  A2 0 MOVI.N,  A8 $44 L L32R,  A8 CALLX8,
  A2 A5 0 S8I,  A2 A4 0 S8I,  MEMW,  A8 $48 L L32R,  A8 CALLX8,
  A2 $18 L L32R,  A2 A2 0 L8UI,  A2 $110 L BEQZ,
  A2 192 MOVI,  A3 $1C L L32R,  A10 A3 A3 OR,  A8 $4C L L32R,  A8 CALLX8,
  A10 A10 A2 OR,  MEMW,  A10 A3 0 S32I.N,
$110 L:
  A10 A6 MOV.N,  A8 $50 L L32R,  A8 CALLX8,
  RETW.N,
$11A L:  END-CODE

CODE get_phy_version_str   \ .text.get_phy_version_str $1C
  $0 7 LITERALS
    $D4 SYMBOL+ rodata.str1.4  $E0 SYMBOL+ rodata.str1.4
    $EC SYMBOL+ rodata.str1.4  $12FF T,  $F4 SYMBOL+ rodata.str1.4
    SYMBOL phy_version_str  SYMBOL sprintf
$1C L:
  A1 32 ENTRY,  A2 $14 L L32R,  A10 A2 MOV.N,  A15 $0 L L32R,  A14 $4 L L32R,
  A13 $8 L L32R,  A12 $C L L32R,  A11 $10 L L32R,  A8 $18 L L32R,  A8 CALLX8,
  RETW.N,
$3B L:  END-CODE

CODE phy_version_print   \ .text.phy_version_print $18
  $0 6 LITERALS
    $D4 SYMBOL+ rodata.str1.4  $E0 SYMBOL+ rodata.str1.4
    $EC SYMBOL+ rodata.str1.4  $12FF T,  $100 SYMBOL+ rodata.str1.4
    SYMBOL phy_printf
$18 L:
  A1 32 ENTRY,  A14 $0 L L32R,  A13 $4 L L32R,  A12 $8 L L32R,  A11 $C L L32R,
  A10 $10 L L32R,  A8 $14 L L32R,  A8 CALLX8,
  RETW.N,
$32 L:  END-CODE

CODE get_iq_value   \ .text.get_iq_value $0
  $0 ORIGIN
  A1 32 ENTRY,  A3 A3 0 16 EXTUI,  A8 A3 6 5 EXTUI,  A9 15 MOVI.N,  A9 A8 $11 L BGE,
  A8 A8 -32 ADDI,
$11 L:
  A8 A2 0 S8I,  MEMW,  A3 A3 0 6 EXTUI,  A8 31 MOVI.N,  A8 A3 $22 L BGE,
  A3 A3 -64 ADDI,
$22 L:
  A3 A2 1 S8I,  MEMW,  RETW.N,
$2A L:  END-CODE

CODE phy_rfcal_data_check_value   \ .text.phy_rfcal_data_check_value $28
  $0 10 LITERALS
    SYMBOL chip7_sleep_params  $60 SYMBOL+ chip7_sleep_params  SYMBOL g_phyFuns
    $28 SYMBOL+ chip7_sleep_params  $18 SYMBOL+ chip7_sleep_params
    $8 SYMBOL+ chip7_sleep_params  $6 SYMBOL+ chip7_sleep_params
    $78 SYMBOL+ chip7_sleep_params  $9 SYMBOL+ chip7_sleep_params  SYMBOL memcpy
$28 L:
  A1 64 ENTRY,  A4 A4 0 8 EXTUI,  A3 A1 20 S32I.N,  A4 $7E L BNEZ,
  A11 $40C MOVI,  A11 A2 A11 ADD.N,  A12 196 MOVI,  A10 A3 A3 OR,  A8 $24 L L32R,
  A8 CALLX8,
  A2 A4 MOV.N,  RETW.N,
  1 ZEROS,
$49 L:
  A13 A5 0 L32I.N,  A8 A8 2 ADDI.N,  A4 A4 2 ADDI,  A14 A8 10 L16UI,
  A10 A4 10 L16UI,  A10 A14 A10 SUB,  A13 A13 $140 L32I,  A8 A1 24 S32I,
  A13 CALLX8,
  A13 A2 1 ADDI.N,  A8 A1 24 L32I.N,  A10 5 $6C L BLTI,
  A2 A13 0 16 EXTUI,
$6C L:
  A3 A8 $49 L BNE,
  A7 A7 8 ADDI.N,  A4 $C L L32R,  A3 A3 8 ADDI.N,  A7 A4 $90 L BNE,
  $CB L J,
  2 ZEROS,
$7E L:
  A8 A1 20 L32I.N,  A2 0 MOVI.N,  A8 A8 4 ADDI.N,  A3 A8 MOV.N,  A6 $0 L L32R,
  A7 A6 MOV.N,  A5 $8 L L32R,  A8 A1 16 S32I.N,
$90 L:
  A8 A3 -4 ADDI,  A4 A7 MOV.N,  $49 L J,
  1 ZEROS,
$99 L:
  A12 A5 0 L32I.N,  A8 A8 2 ADDI.N,  A7 A7 2 ADDI.N,  A13 A8 150 L16UI,
  A10 A7 150 L16UI,  A10 A13 A10 SUB,  A12 A12 $140 L32I,  A8 A1 24 S32I.N,
  A12 CALLX8,
  A12 A2 1 ADDI.N,  A8 A1 24 L32I.N,  A10 5 $BA L BLTI,
  A2 A12 0 16 EXTUI,
$BA L:
  A9 A1 16 L32I.N,  A9 A8 $99 L BNE,
  A9 A9 8 ADDI.N,  A3 A3 8 ADDI.N,  A9 A1 16 S32I.N,  A3 A4 $D0 L BNE,
  $DA L J,
$CB L:
  A4 $10 L L32R,  A3 A6 MOV.N,
$D0 L:
  A7 A3 MOV.N,  A9 A1 16 L32I.N,  A8 A9 -4 ADDI,  $99 L J,
$DA L:
  A4 A1 20 L32I.N,  A3 A4 52 ADDI,  A7 A4 56 ADDI,  A4 A6 MOV.N,
$E4 L:
  A8 A3 0 L16UI,  A9 15 MOVI.N,  A11 A8 6 5 EXTUI,  A10 A11 7 SEXT,
  A9 A10 $F5 L BGE,
  A10 A11 -32 ADDI,
$F5 L:
  A8 A8 0 6 EXTUI,  A9 31 MOVI.N,  A9 A8 $100 L BGE,
  A8 A8 -64 ADDI,
$100 L:
  A8 A1 3 S8I,  MEMW,  A9 15 MOVI.N,  NOP.N,  NOP.N,  NOP.N,  A8 A4 52 L16UI,
  A12 A8 6 5 EXTUI,  A11 A12 7 SEXT,  A9 A11 $11D L BGE,
  A11 A12 -32 ADDI,
$11D L:
  A8 A8 0 6 EXTUI,  A9 31 MOVI,  A9 A8 $129 L BGE,
  A8 A8 -64 ADDI,
$129 L:
  A8 A1 1 S8I,  MEMW,  A10 A10 A11 SUB,  NOP.N,  NOP.N,  NOP.N,  A8 A5 0 L32I,
  A8 A8 $140 L32I,  A8 CALLX8,
  A10 5 $149 L BLTI,
  A2 A2 1 ADDI.N,  A2 A2 0 16 EXTUI,
$149 L:
  A10 A1 3 L8UI,  A10 A10 7 SEXT,  A8 A1 1 L8UI,  A8 A8 7 SEXT,  A10 A10 A8 SUB,
  A11 A5 0 L32I,  A8 A11 $140 L32I,  A8 CALLX8,
  A10 5 $16A L BLTI,
  A2 A2 1 ADDI,  A2 A2 0 16 EXTUI,
$16A L:
  A3 A3 2 ADDI.N,  A4 A4 2 ADDI.N,  A3 A7 $174 L BEQ,
  $E4 L J,
$174 L:
  A3 A6 MOV.N,  A4 15 MOVI,
$179 L:
  A8 A7 0 L16UI,  A11 A8 6 5 EXTUI,  A10 A11 7 SEXT,  A4 A10 $188 L BGE,
  A10 A11 -32 ADDI,
$188 L:
  A8 A8 0 6 EXTUI,  A9 31 MOVI,  A9 A8 $194 L BGE,
  A8 A8 -64 ADDI,
$194 L:
  A8 A1 3 S8I,  MEMW,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A8 A3 56 L16UI,
  A12 A8 6 5 EXTUI,  A11 A12 7 SEXT,  A4 A11 $1B1 L BGE,
  A11 A12 -32 ADDI,
$1B1 L:
  A8 A8 0 6 EXTUI,  A9 31 MOVI,  A9 A8 $1BD L BGE,
  A8 A8 -64 ADDI,
$1BD L:
  A8 A1 1 S8I,  MEMW,  A10 A10 A11 SUB,  NOP.N,  NOP.N,  NOP.N,  A8 A5 0 L32I,
  A8 A8 $140 L32I,  A8 CALLX8,
  A10 5 $1DD L BLTI,
  A2 A2 1 ADDI.N,  A2 A2 0 16 EXTUI,
$1DD L:
  A10 A1 3 L8UI,  A10 A10 7 SEXT,  A8 A1 1 L8UI,  A8 A8 7 SEXT,  A10 A10 A8 SUB,
  A11 A5 0 L32I,  A8 A11 $140 L32I,  A8 CALLX8,
  A10 5 $1FD L BLTI,
  A2 A2 1 ADDI.N,  A2 A2 0 16 EXTUI,
$1FD L:
  A8 $14 L L32R,  A3 A3 2 ADDI.N,  A7 A7 2 ADDI.N,  A8 A3 $20A L BEQ,
  $179 L J,
$20A L:
  A7 146 MOVI,  A9 A1 20 L32I.N,  A3 A6 MOV.N,  A7 A9 A7 ADD.N,  A4 15 MOVI.N,
$215 L:
  A8 A7 0 L16UI,  A11 A8 6 5 EXTUI,  A10 A11 7 SEXT,  A4 A10 $224 L BGE,
  A10 A11 -32 ADDI,
$224 L:
  A8 A8 0 6 EXTUI,  A9 31 MOVI,  A9 A8 $230 L BGE,
  A8 A8 -64 ADDI,
$230 L:
  A8 A1 3 S8I,  MEMW,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A8 A3 146 L16UI,
  A12 A8 6 5 EXTUI,  A11 A12 7 SEXT,  A4 A11 $24D L BGE,
  A11 A12 -32 ADDI,
$24D L:
  A8 A8 0 6 EXTUI,  A9 31 MOVI,  A9 A8 $259 L BGE,
  A8 A8 -64 ADDI,
$259 L:
  A8 A1 1 S8I,  MEMW,  A10 A10 A11 SUB,  NOP.N,  NOP.N,  NOP.N,  A8 A5 0 L32I,
  A8 A8 $140 L32I,  A8 CALLX8,
  A10 5 $279 L BLTI,
  A2 A2 1 ADDI.N,  A2 A2 0 16 EXTUI,
$279 L:
  A10 A1 3 L8UI,  A10 A10 7 SEXT,  A8 A1 1 L8UI,  A8 A8 7 SEXT,  A10 A10 A8 SUB,
  A11 A5 0 L32I,  A8 A11 $140 L32I,  A8 CALLX8,
  A10 5 $299 L BLTI,
  A2 A2 1 ADDI.N,  A2 A2 0 16 EXTUI,
$299 L:
  A8 $18 L L32R,  A3 A3 2 ADDI.N,  A7 A7 2 ADDI.N,  A8 A3 $2A6 L BEQ,
  $215 L J,
$2A6 L:
  A9 A1 20 L32I.N,  A3 A9 96 ADDI,  A7 $4 L L32R,  A4 $1C L L32R,
$2B1 L:
  A10 A3 0 L8UI,  A3 A3 1 ADDI.N,  A10 A10 7 SEXT,  A8 A7 0 L8UI,  A7 A7 1 ADDI.N,
  A8 A8 7 SEXT,  A10 A10 A8 SUB,  A8 A5 0 L32I.N,  A8 A8 $140 L32I,  A8 CALLX8,
  A8 A2 1 ADDI.N,  A10 5 $2D4 L BLTI,
  A2 A8 0 16 EXTUI,
$2D4 L:
  A7 A4 $2B1 L BNE,
  A3 A1 20 L32I.N,  A8 $0 L L32R,  A4 A5 0 L32I.N,  A10 A3 138 L8UI,
  A7 A8 138 L8UI,  A10 A10 A7 SUB,  A4 A4 $140 L32I,  A4 CALLX8,
  A10 5 $2F5 L BLTI,
  A2 A2 1 ADDI.N,  A2 A2 0 16 EXTUI,
$2F5 L:
  A9 A1 20 L32I.N,  A3 A9 64 ADDI,  A4 $20 L L32R,
$2FD L:
  A6 A6 1 ADDI.N,  A7 A5 0 L32I.N,  A10 A3 0 L8UI,  A3 A3 1 ADDI.N,  A8 A6 63 L8UI,
  A10 A10 A8 SUB,  A7 A7 $140 L32I,  A7 CALLX8,
  A7 A2 1 ADDI.N,  A10 5 $31A L BLTI,
  A2 A7 0 16 EXTUI,
$31A L:
  A4 A6 $2FD L BNE,
  RETW.N,
$31F L:  END-CODE

CODE tx_cont_en   \ .text.tx_cont_en $8
  $0 2 LITERALS
    SYMBOL chip7_sleep_params  SYMBOL g_phyFuns
$8 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A3 A2 82 L8UI,  A3 $51 L BNEZ.N,
  A14 A3 MOV.N,  A13 6 MOVI.N,  A4 $4 L L32R,  A12 3 MOVI.N,  A11 1 MOVI.N,
  A10 103 MOVI,  A8 A4 0 L32I.N,  A15 A2 84 L8UI,  A15 A15 22 ADDI,
  A15 A15 0 8 EXTUI,  A8 A8 168 L32I,  A8 CALLX8,
  A14 A3 MOV.N,  A13 6 MOVI.N,  A3 A4 0 L32I.N,  A12 4 MOVI.N,  A11 1 MOVI.N,
  A10 103 MOVI,  A15 A2 84 L8UI,  A15 A15 24 ADDI,  A15 A15 0 8 EXTUI,
  A2 A3 168 L32I,  A2 CALLX8,
  RETW.N,
  1 ZEROS,
$51 L:
  A3 $4 L L32R,  A14 0 MOVI.N,  A13 6 MOVI.N,  A12 3 MOVI.N,  A11 1 MOVI.N,
  A8 A3 0 L32I.N,  A10 103 MOVI,  A15 A2 86 L8UI,  A15 A15 18 ADDI,
  A15 A15 0 8 EXTUI,  A8 A8 168 L32I,  A8 CALLX8,
  A14 0 MOVI.N,  A13 6 MOVI.N,  A3 A3 0 L32I.N,  A12 4 MOVI.N,  A11 1 MOVI.N,
  A10 103 MOVI,  A15 A2 86 L8UI,  A15 A15 17 ADDI,  A15 A15 0 8 EXTUI,
  A2 A3 168 L32I,  A2 CALLX8,
  RETW.N,
$8E L:  END-CODE

CODE tx_cont_dis   \ .text.tx_cont_dis $8
  $0 2 LITERALS
    SYMBOL chip7_sleep_params  SYMBOL g_phyFuns
$8 L:
  A1 32 ENTRY,  A4 $0 L L32R,  A3 0 MOVI.N,  A2 2 MOVI.N,  A14 A3 MOV.N,
  A13 6 MOVI.N,  A8 A4 82 L8UI,  A12 3 MOVI.N,  A2 A3 A8 MOVEQZ,  A2 A4 A2 ADD.N,
  A11 1 MOVI.N,  A4 $4 L L32R,  A10 103 MOVI,  A8 A4 0 L32I,  A15 A2 84 L8UI,
  A15 A15 -2 ADDI,  A15 A15 0 8 EXTUI,  A8 A8 168 L32I,  A8 CALLX8,
  A14 A3 MOV.N,  A13 6 MOVI.N,  A3 A4 0 L32I.N,  A12 4 MOVI.N,  A11 1 MOVI.N,
  A10 103 MOVI,  A3 A3 168 L32I,  A15 A2 84 L8UI,  A3 CALLX8,
  RETW.N,
$52 L:  END-CODE

CODE tx_cont_cfg   \ .text.tx_cont_cfg $8
  $0 2 LITERALS
    SYMBOL tx_cont_en  SYMBOL tx_cont_dis
$8 L:
  A1 32 ENTRY,  A2 A2 0 8 EXTUI,  A2 1 $19 L BNEI,
  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$19 L:
  A8 $4 L L32R,  A8 CALLX8,
  RETW.N,
$21 L:  END-CODE

CODE phy_get_tx_pwr   \ .text.phy_get_tx_pwr $C
  $0 3 LITERALS
    SYMBOL g_phyFuns  $120 SYMBOL+ rodata.str1.4  SYMBOL phy_printf
$C L:
  A1 48 ENTRY,  A2 $0 L L32R,  A10 A1 MOV.N,  A2 A2 0 L32I.N,  A2 A2 $12C L32I,
  A2 CALLX8,
  A8 A1 14 L16UI,  A12 A1 12 L16UI,  A12 A12 A8 ADD.N,  A9 A12 0 16 EXTUI,
  A8 A1 6 L16UI,  A11 A1 4 L16UI,  A11 A11 A8 ADD,  A11 A11 A9 SUB,
  A11 A11 15 SEXT,  A12 A1 8 L16UI,  A2 A11 10 SLLI,  A8 A1 10 L16UI,
  A12 A12 A8 ADD,  A12 A12 A9 SUB,  A12 A12 15 SEXT,  A10 $4 L L32R,
  A2 A2 A12 QUOS,  A8 $8 L L32R,  A8 CALLX8,
  A2 A2 15 SEXT,  RETW.N,
$59 L:  END-CODE

CODE phy_init_pwr_print   \ .text.phy_init_pwr_print $78
  $0 30 LITERALS
    SYMBOL chip7_sleep_params  $78 SYMBOL+ chip7_sleep_params
    $60 SYMBOL+ chip7_sleep_params  SYMBOL tx_rf_ana_gain
    $12C SYMBOL+ rodata.str1.4  $15C SYMBOL+ rodata.str1.4
    $8 SYMBOL+ chip7_sleep_params  $170 SYMBOL+ rodata.str1.4
    $178 SYMBOL+ rodata.str1.4  $17C SYMBOL+ rodata.str1.4
    $190 SYMBOL+ rodata.str1.4  $1A0 SYMBOL+ rodata.str1.4
    $1A8 SYMBOL+ rodata.str1.4  $1B8 SYMBOL+ rodata.str1.4
    $1C8 SYMBOL+ rodata.str1.4  SYMBOL phy_printf  SYMBOL phy_printf
    SYMBOL phy_printf  SYMBOL phy_printf  SYMBOL phy_printf  SYMBOL phy_printf
    SYMBOL phy_printf  SYMBOL phy_printf  SYMBOL phy_printf  SYMBOL phy_printf
    SYMBOL phy_printf  SYMBOL phy_printf  SYMBOL phy_printf  SYMBOL phy_printf
    SYMBOL phy_printf
$78 L:
  A1 32 ENTRY,  A2 $C L L32R,  A10 $10 L L32R,  A12 A2 2 L16UI,  A11 A2 0 L16UI,
  A8 $3C L L32R,  A8 CALLX8,
  A10 $14 L L32R,  A8 $40 L L32R,  A8 CALLX8,
  A3 $0 L L32R,  A4 A3 MOV.N,  A5 $18 L L32R,  A2 $1C L L32R,
$A1 L:
  A4 A4 2 ADDI.N,  A10 A2 MOV.N,  A11 A4 2 L16SI,  A8 $44 L L32R,  A8 CALLX8,
  A5 A4 $A1 L BNE,
  A7 $20 L L32R,  A10 A7 MOV.N,  A8 $48 L L32R,  A8 CALLX8,
  A10 $24 L L32R,  A8 $4C L L32R,  A8 CALLX8,
  A6 $8 L L32R,  A4 $4 L L32R,
$CB L:
  A10 A2 MOV.N,  A11 A6 0 L8UI,  A6 A6 1 ADDI,  A11 A11 7 SEXT,  A8 $50 L L32R,
  A8 CALLX8,
  A4 A6 $CB L BNE,
  A10 A7 A7 OR,  A8 $54 L L32R,  A8 CALLX8,
  A10 $28 L L32R,  A6 A3 A3 OR,  A8 $58 L L32R,  A8 CALLX8,
  A4 $2C L L32R,
$F7 L:
  A6 A6 1 ADDI.N,  A10 A4 A4 OR,  A11 A6 119 L8UI,  A8 $5C L L32R,  A8 CALLX8,
  A5 A6 $F7 L BNE,
  A10 A7 MOV.N,  A8 $60 L L32R,  A8 CALLX8,
  A10 $30 L L32R,  A8 $64 L L32R,  A8 CALLX8,
$119 L:
  A3 A3 1 ADDI.N,  A10 A2 MOV.N,  A11 A3 128 L8UI,  A11 A11 7 SEXT,  A8 $68 L L32R,
  A8 CALLX8,
  A3 A5 $119 L BNE,
  A10 A7 A7 OR,  A8 $6C L L32R,  A8 CALLX8,
  A2 $0 L L32R,  A10 $34 L L32R,  A11 A2 144 L16UI,  A8 $70 L L32R,  A8 CALLX8,
  A11 A2 138 L8UI,  A10 $38 L L32R,  A8 $74 L L32R,  A8 CALLX8,
  RETW.N,
$152 L:  END-CODE

CODE phy_get_rx_freq   \ .text.phy_get_rx_freq $8
  $0 2 LITERALS
    $2AAAAAAB T,  $4EC4EC4F T,
$8 L:
  A1 32 ENTRY,  A2 A2 0 8 EXTUI,  A2 8 $28 L BGEUI,
  A2 $0 L L32R,  A3 A3 15 SEXT,  A2 A3 A2 MULSH,  A3 A3 31 SRAI,  A2 A2 3 SRAI,
  A3 A2 A3 SUB,  A2 A3 15 SEXT,  RETW.N,
$28 L:
  A3 A3 10 10 EXTUI,  A3 A3 15 SEXT,  A2 $1FF MOVI,  A2 A3 $3A L BGE,
  A3 A3 -1024 ADDMI,  A3 A3 15 SEXT,
$3A L:
  A2 $4 L L32R,  A3 A3 4 SLLI,  A2 A3 A2 MULSH,  A3 A3 31 SRAI,  A2 A2 2 SRAI,
  A3 A2 A3 SUB,  A2 A3 15 SEXT,  RETW.N,
$51 L:  END-CODE

CODE reset_rf_dig   \ .text.reset_rf_dig $30
  $0 12 LITERALS
    $3FF00024 T,  $3FF000D0 T,  $3FF48030 T,  $27FFFFFF T,
    SYMBOL disable_wifi_agc  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL ets_delay_us
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL ets_delay_us  SYMBOL force_txrxoff
$30 L:
  A1 32 ENTRY,  A8 $10 L L32R,  A8 CALLX8,
  A3 $0 L L32R,  A10 A3 A3 OR,  A8 $14 L L32R,  A8 CALLX8,
  A8 -3 MOVI.N,  A2 $4 L L32R,  A8 A10 A8 AND,  A10 A2 MOV.N,  MEMW,
  A8 A3 0 S32I.N,  A8 $18 L L32R,  A8 CALLX8,
  A8 3 MOVI.N,  A8 A10 A8 OR,  A10 1 MOVI.N,  MEMW,  A8 A2 0 S32I.N,  A8 $1C L L32R,
  A8 CALLX8,
  A10 A2 MOV.N,  A8 $20 L L32R,  A8 CALLX8,
  A8 -4 MOVI.N,  A3 $8 L L32R,  A8 A10 A8 AND,  A10 A3 MOV.N,  MEMW,
  A8 A2 0 S32I.N,  A8 $24 L L32R,  A8 CALLX8,
  A2 $C L L32R,  A2 A10 A2 AND,  A10 1 MOVI.N,  MEMW,  A2 A3 0 S32I.N,
  A8 $28 L L32R,  A8 CALLX8,
  A10 1 MOVI.N,  A8 $2C L L32R,  A8 CALLX8,
  RETW.N,
$A6 L:  END-CODE

CODE register_chipv7_phy   \ .text.register_chipv7_phy $A8
  $0 42 LITERALS
    $200008 T,  $60033C00 T,  SYMBOL re_entry  SYMBOL phy_init_flag
    SYMBOL force_hw_set_freq_flag  $3FF4E0C4 T,  SYMBOL chip7_sleep_params
    SYMBOL chip7_phy_init_ctrl  SYMBOL pwrdet_offset  SYMBOL phy_in_most_power
    $FFFEDDD7 T,  SYMBOL g_phyFuns  SYMBOL btpwr_tsen_init  SYMBOL btpwr_tsen_old
    SYMBOL init_wifi_disable  $3FF5C080 T,  SYMBOL phy_wakeup_init
    SYMBOL phy_get_romfunc_addr  SYMBOL reset_rf_dig  SYMBOL phy_dis_hw_set_freq
    SYMBOL esp_dport_access_reg_read  SYMBOL i2c_master_reset
    SYMBOL phy_rfcal_data_check  SYMBOL phy_rfcal_data_check_value
    SYMBOL register_chipv7_phy_init_param  SYMBOL rf_cal_data_recovery
    SYMBOL write_freq_mem_all  SYMBOL rf_init  SYMBOL bb_init
    SYMBOL phy_rfcal_data_check_value  SYMBOL rf_cal_data_backup
    SYMBOL phy_rfcal_data_check  SYMBOL chip_v7_set_chan_nomac
    SYMBOL force_txrxoff  SYMBOL chip_v7_set_chan_nomac
    SYMBOL chip_v7_set_chan_offset  SYMBOL tsens_code_read
    SYMBOL phy_en_hw_set_freq  SYMBOL enable_wifi_agc
    SYMBOL esp_dport_access_reg_read  SYMBOL rf_init  SYMBOL bb_init
$A8 L:
  A1 $100 ENTRY,  A5 $4 L L32R,  A4 A4 0 8 EXTUI,  MEMW,  A5 A5 0 L32I.N,
  A2 $C1 L BEQZ.N,
  A5 $8 L L32R,  A6 A5 0 L8UI,  A6 1 $CC L BNEI,
$C1 L:
  A8 $40 L L32R,  A8 CALLX8,
  A2 0 MOVI.N,  RETW.N,
  1 ZEROS,
$CC L:
  A8 $C L L32R,  A7 1 MOVI.N,  A9 $10 L L32R,  A7 A8 0 S8I,  A7 A9 0 S8I,  MEMW,
  A6 $EB L BNEZ.N,
  A8 $44 L L32R,  A8 CALLX8,
  A8 $48 L L32R,  A8 CALLX8,
$EB L:
  A8 $4C L L32R,  A8 CALLX8,
  A6 $14 L L32R,  A7 -257 MOVI,  A10 A6 A6 OR,  A8 $50 L L32R,  A8 CALLX8,
  A10 A10 A7 AND,  MEMW,  A10 A6 0 S32I.N,  A8 $54 L L32R,  A8 CALLX8,
  A7 A5 0 L8UI,  A7 $331 L BNEZ,
  A12 A2 MOV.N,  A11 A3 MOV.N,  A10 1 MOVI.N,  A8 $58 L L32R,  A8 CALLX8,
  A6 A10 MOV.N,  A9 1 MOVI.N,  A4 2 $129 L BLTUI,
  A9 A7 MOV.N,
$129 L:
  A8 0 MOVI.N,  A7 A9 0 8 EXTUI,  A10 1 MOVI.N,  A9 A8 MOV.N,  A9 A10 A6 MOVNEZ,
  A9 A7 A9 AND,  A9 A1 208 S32I,  A9 A8 $141 L BEQ,
  $310 L J,
$141 L:
  A8 A4 -1 ADDI.N,  A9 A10 A8 MOVNEZ,  A9 A1 208 S32I,  $314 L J,
  1 ZEROS,
$14D L:
  A12 A8 MOV.N,  A11 A1 MOV.N,  A10 A3 MOV.N,  A8 A1 212 S32I,  A8 $5C L L32R,
  A8 CALLX8,
  A9 $18 L L32R,  A8 A1 212 L32I,  A8 A9 0 S32I.N,
$164 L:
  A10 A2 MOV.N,  A8 $60 L L32R,  A8 CALLX8,
  A10 $1C L L32R,  A8 A10 51 L8UI,  A8 1 $184 L BNEI,
  A9 $138 MOVI,  A8 $20 L L32R,  A9 A8 0 S16I,  MEMW,  $190 L J,
$184 L:
  A8 $20 L L32R,  A9 $120 MOVI,  A9 A8 0 S16I,  MEMW,
$190 L:
  A11 0 MOVI.N,  A8 1 MOVI.N,  NOP.N,  NOP.N,  A9 A5 0 L8UI,  A8 A11 A9 MOVNEZ,
  A8 A7 A8 AND,  A11 A10 14 L8UI,  A10 $24 L L32R,  A11 A10 0 S8I,  MEMW,
  A8 $1C4 L BEQZ,
  A10 A3 A3 OR,  A8 A1 212 S32I,  A8 $64 L L32R,  A8 CALLX8,
  A8 A1 212 L32I,  A7 A8 MOV.N,  A9 A5 0 L8UI,
$1C4 L:
  A9 $1EE L BNEZ.N,
  A9 $18 L L32R,  A8 $28 L L32R,  A10 A9 0 L32I.N,  A8 A10 A8 AND,  A8 A9 0 S32I.N,
  A4 $355 L BEQZ,
  A11 8 MOVI.N,  A4 2 $1E1 L BGEUI,
  $328 L J,
$1DE L:
  A11 A8 A11 OR,
$1E1 L:
  A11 A9 0 S32I.N,  $1EE L J,
$1E6 L:
  A10 A3 12 ADDI.N,  A8 $68 L L32R,  A8 CALLX8,
$1EE L:
  A8 $6C L L32R,  A8 CALLX8,
  A8 $70 L L32R,  A8 CALLX8,
  A8 A5 0 L8UI,  A8 $234 L BNEZ.N,
  A4 A1 208 L32I,  A4 $234 L BEQZ.N,
  A6 $21D L BNEZ.N,
$206 L:
  A12 1 MOVI.N,  A11 A1 MOV.N,  A10 A3 MOV.N,  A8 $74 L L32R,  A8 CALLX8,
  A4 0 MOVI.N,  A6 1 MOVI,  A6 A4 A10 MOVEQZ,  A6 A6 0 8 EXTUI,
$21D L:
  A10 A3 MOV.N,  A8 $78 L L32R,  A8 CALLX8,
  A12 A2 MOV.N,  A11 A3 MOV.N,  A10 0 MOVI.N,  A8 $7C L L32R,  A8 CALLX8,
  A8 A5 0 L8UI,
$234 L:
  A8 1 $26A L BNEI,
  A13 99 MOVI,  A12 0 MOVI.N,  A11 4 MOVI.N,  A2 $2C L L32R,  A10 101 MOVI,
  A3 A2 0 L32I.N,  A3 A3 160 L32I,  A3 CALLX8,
  A11 10 MOVI.N,  A3 $18 L L32R,  A10 A3 81 L8UI,  A10 A10 7 SEXT,  A8 $80 L L32R,
  A8 CALLX8,
  A10 0 MOVI.N,  A8 $84 L L32R,  A8 CALLX8,
  $292 L J,
  2 ZEROS,
$26A L:
  A7 $27D L BEQZ.N,
  A2 $18 L L32R,  A11 10 MOVI.N,  A10 A2 81 L8UI,  A10 A10 7 SEXT,  A8 $88 L L32R,
  A8 CALLX8,
$27D L:
  A2 $1C L L32R,  A3 A2 78 L8UI,  A2 $2C L L32R,  A3 $292 L BEQZ,
  A10 0 MOVI,  A8 $8C L L32R,  A8 CALLX8,
$292 L:
  A12 0 MOVI.N,  A2 A2 0 L32I.N,  A11 A12 MOV.N,  A10 99 MOVI,  A2 A2 152 L32I,
  A2 CALLX8,
  A2 A5 0 L8UI,  A2 $2C6 L BNEZ.N,
  A8 $90 L L32R,  A8 CALLX8,
  A2 $30 L L32R,  A10 A2 0 S8I,  MEMW,  NOP.N,  NOP.N,  NOP.N,  NOP.N,
  A2 $34 L L32R,  A10 A2 0 S8I,  MEMW,
$2C6 L:
  A2 0 MOVI.N,  A3 1 MOVI.N,  NOP,  NOP,  A8 $C L L32R,  A9 $10 L L32R,
  A2 A8 0 S8I,  A2 A9 0 S8I,  A3 A5 0 S8I,  MEMW,  A8 $94 L L32R,  A8 CALLX8,
  A8 $98 L L32R,  A8 CALLX8,
  A2 $38 L L32R,  A2 A2 0 L8UI,  A2 $30C L BEQZ.N,
  A2 192 MOVI,  A3 $3C L L32R,  A10 A3 MOV.N,  A8 $9C L L32R,  A8 CALLX8,
  A10 A10 A2 OR,  MEMW,  A10 A3 0 S32I.N,
$30C L:
  A2 A6 MOV.N,  RETW.N,
$310 L:
  A7 A8 MOV.N,  A4 2 MOVI.N,
$314 L:
  A8 0 MOVI.N,  A6 A8 $31C L BEQ,
  $164 L J,
$31C L:
  A9 A1 208 L32I,  A9 A8 $325 L BNE,
  $164 L J,
$325 L:
  $14D L J,
$328 L:
  A10 4 $32E L BBCI,
  $1E6 L J,
$32E L:
  $1DE L J,
$331 L:
  A7 1 MOVI.N,  A4 2 $338 L BLTUI,
  A7 0 MOVI.N,
$338 L:
  A8 $A0 L L32R,  A8 CALLX8,
  A8 $A4 L L32R,  A8 CALLX8,
  A7 A7 0 8 EXTUI,  A8 A5 0 L8UI,  A4 1 $350 L BEQI,
  A8 $206 L BEQZ,
$350 L:
  A6 0 MOVI.N,  $234 L J,
$355 L:
  A11 $0 L L32R,  $1E1 L J,
$35B L:  END-CODE

CODE phy_set_most_tpw   \ .text.phy_set_most_tpw $18
  $0 6 LITERALS
    SYMBOL phy_in_most_power  SYMBOL phy_set_most_tpw_index
    SYMBOL chip7_sleep_params  SYMBOL phy_set_most_tpw_flag
    SYMBOL set_most_pwr_reg  SYMBOL set_chan_dig_gain
$18 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A8 0 S8I,  MEMW,  A8 $10 L L32R,  A8 CALLX8,
  A8 $4 L L32R,  A10 A8 0 S8I,  MEMW,  NOP.N,  NOP.N,  NOP.N,  NOP,  A8 $8 L L32R,
  A10 A8 81 L8UI,  A8 $14 L L32R,  A8 CALLX8,
  A9 1 MOVI.N,  A8 $C L L32R,  A9 A8 0 S8I,  MEMW,  RETW.N,
$55 L:  END-CODE

CODE phy_get_most_tpw   \ .text.phy_get_most_tpw $C
  $0 3 LITERALS
    SYMBOL chip7_phy_init_ctrl  SYMBOL phy_set_most_tpw_flag  SYMBOL set_most_tpw
$C L:
  A1 32 ENTRY,  A2 $4 L L32R,  A8 A2 0 L8UI,  A2 $0 L L32R,  A2 A2 14 L8UI,
  A8 1 $24 L BEQI,
  A2 A2 0 8 EXTUI,  RETW.N,
  1 ZEROS,
$24 L:
  A2 $8 L L32R,  A2 A2 0 L8UI,  RETW.N,
$2C L:  END-CODE

CODE phy_rx_sense_set   \ .text.phy_rx_sense_set $28
  $0 10 LITERALS
    $3FF5C010 T,  $7FFFFF T,  $3FF5C014 T,  $3FF5C044 T,  $3FF5C108 T,
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read
$28 L:
  A1 32 ENTRY,  A6 $0 L L32R,  A2 A2 0 8 EXTUI,  A10 A6 MOV.N,  A8 $14 L L32R,
  A8 CALLX8,
  A3 A2 23 SLLI,  A4 $4 L L32R,  A10 A10 A4 AND,  A8 A10 A3 OR,  A5 $8 L L32R,
  A10 A5 MOV.N,  MEMW,  A8 A6 0 S32I.N,  A8 $18 L L32R,  A8 CALLX8,
  A10 A10 A4 AND,  A3 A10 A3 OR,  A4 $C L L32R,  A10 A4 A4 OR,  MEMW,  A3 A5 0 S32I,
  A8 $1C L L32R,  A8 CALLX8,
  A8 -256 MOVI,  A10 A10 A8 AND,  A10 A10 A2 OR,  A3 $10 L L32R,  MEMW,
  A10 A4 0 S32I.N,  A10 A3 MOV.N,  A2 $95 L BNEZ.N,
  A8 $20 L L32R,  A8 CALLX8,
  A2 $200 MOVI,  A10 A10 A2 OR,  MEMW,  A10 A3 0 S32I.N,  RETW.N,
$95 L:
  A8 $24 L L32R,  A8 CALLX8,
  A2 -513 MOVI,  A10 A10 A2 AND,  MEMW,  A10 A3 0 S32I.N,  RETW.N,
$A8 L:  END-CODE

CODE ant_dft_cfg   \ .text.ant_dft_cfg $C
  $0 3 LITERALS
    $3FF5C11C T,  $FFFFF7FF T,  SYMBOL esp_dport_access_reg_read
$C L:
  A1 32 ENTRY,  A3 $0 L L32R,  A2 A2 0 8 EXTUI,  A10 A3 MOV.N,  A8 $8 L L32R,
  A8 CALLX8,
  A2 A2 0 1 EXTUI,  A8 $4 L L32R,  A2 A2 11 SLLI,  A10 A10 A8 AND,  A10 A2 A10 OR,
  MEMW,  A10 A3 0 S32I.N,  RETW.N,
$33 L:  END-CODE

CODE ant_wifitx_cfg   \ .text.ant_wifitx_cfg $14
  $0 5 LITERALS
    $3FF45104 T,  $FFFFF0FF T,  $FFF0FFFF T,  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read
$14 L:
  A1 32 ENTRY,  A4 $0 L L32R,  A2 A2 0 8 EXTUI,  A10 A4 MOV.N,  A8 $C L L32R,
  A8 CALLX8,
  A8 A2 0 4 EXTUI,  A9 $4 L L32R,  A8 A8 8 SLLI,  A10 A10 A9 AND,  A8 A8 A10 OR,
  A10 A4 MOV.N,  MEMW,  A8 A4 0 S32I.N,  A8 $10 L L32R,  A8 CALLX8,
  A3 A3 0 8 EXTUI,  A3 A3 0 4 EXTUI,  A3 A3 16 SLLI,  A8 $8 L L32R,  A10 A10 A8 AND,
  A10 A3 A10 OR,  MEMW,  A10 A4 0 S32I.N,  RETW.N,
$5A L:  END-CODE

CODE ant_wifirx_cfg   \ .text.ant_wifirx_cfg $2C
  $0 11 LITERALS
    $3FF5C11C T,  $3FF45104 T,  $F0FFFFFF T,  $3FF45108 T,  $FFFFF0FF T,
    $FFF0FFFF T,  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
$2C L:
  A1 32 ENTRY,  A6 $0 L L32R,  A2 A2 0 8 EXTUI,  A10 A6 A6 OR,  A8 $18 L L32R,
  A8 CALLX8,
  A8 -3 MOVI.N,  A2 A2 0 1 EXTUI,  A10 A10 A8 AND,  A5 $4 L L32R,  A2 A2 1 SLLI,
  A2 A2 A10 OR,  A10 A5 MOV.N,  A3 A3 0 8 EXTUI,  MEMW,  A2 A6 0 S32I,
  A8 $1C L L32R,  A8 CALLX8,
  A3 A3 0 4 EXTUI,  A2 A3 24 SLLI,  A4 A4 0 8 EXTUI,  A6 $8 L L32R,
  A4 A4 0 4 EXTUI,  A10 A10 A6 AND,  A6 A10 A2 OR,  A2 $C L L32R,  A10 A2 MOV.N,
  MEMW,  A6 A5 0 S32I.N,  A8 $20 L L32R,  A8 CALLX8,
  A5 -16 MOVI.N,  A10 A10 A5 AND,  A3 A10 A3 OR,  MEMW,  A3 A2 0 S32I,
  A10 A2 A2 OR,  A8 $24 L L32R,  A8 CALLX8,
  A5 A4 8 SLLI,  A4 A4 16 SLLI,  A3 $10 L L32R,  A10 A10 A3 AND,  A3 A10 A5 OR,
  A10 A2 MOV.N,  MEMW,  A3 A2 0 S32I.N,  A8 $28 L L32R,  A8 CALLX8,
  A3 $14 L L32R,  A10 A10 A3 AND,  A4 A10 A4 OR,  MEMW,  A4 A2 0 S32I.N,  RETW.N,
$C8 L:  END-CODE

CODE ant_bttx_cfg   \ .text.ant_bttx_cfg $14
  $0 5 LITERALS
    $3FF45108 T,  $F0FFFFFF T,  $3FF4510C T,  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read
$14 L:
  A1 32 ENTRY,  A4 $0 L L32R,  A2 A2 0 8 EXTUI,  A10 A4 MOV.N,  A8 $C L L32R,
  A8 CALLX8,
  A2 A2 0 4 EXTUI,  A8 $4 L L32R,  A3 A2 24 SLLI,  A10 A10 A8 AND,  A8 A10 A3 OR,
  A3 $8 L L32R,  A10 A3 MOV.N,  MEMW,  A8 A4 0 S32I.N,  A8 $10 L L32R,  A8 CALLX8,
  A8 -16 MOVI.N,  A10 A10 A8 AND,  A2 A10 A2 OR,  MEMW,  A2 A3 0 S32I.N,  RETW.N,
$53 L:  END-CODE

CODE ant_btrx_cfg   \ .text.ant_btrx_cfg $2C
  $0 11 LITERALS
    $3FF5C11C T,  $3FF4510C T,  $FFFFF0FF T,  $FFF0FFFF T,  $F0FFFFFF T,
    $3FF45110 T,  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
$2C L:
  A1 32 ENTRY,  A6 $0 L L32R,  A2 A2 0 8 EXTUI,  A10 A6 A6 OR,  A8 $18 L L32R,
  A8 CALLX8,
  A8 -9 MOVI.N,  A2 A2 0 1 EXTUI,  A5 $4 L L32R,  A10 A10 A8 AND,  A2 A2 3 SLLI,
  A8 A2 A10 OR,  A10 A5 MOV.N,  MEMW,  A8 A6 0 S32I.N,  A8 $1C L L32R,  A8 CALLX8,
  A3 A3 0 8 EXTUI,  A3 A3 0 4 EXTUI,  A2 A3 8 SLLI,  A6 $8 L L32R,  A3 A3 16 SLLI,
  A10 A10 A6 AND,  A2 A10 A2 OR,  A10 A5 A5 OR,  MEMW,  A2 A5 0 S32I,
  A8 $20 L L32R,  A8 CALLX8,
  A4 A4 0 8 EXTUI,  A4 A4 0 4 EXTUI,  A2 $C L L32R,  A10 A10 A2 AND,  A3 A10 A3 OR,
  A10 A5 MOV.N,  MEMW,  A3 A5 0 S32I.N,  A8 $24 L L32R,  A8 CALLX8,
  A3 A4 24 SLLI,  A2 $10 L L32R,  A10 A10 A2 AND,  A2 A10 A3 OR,  A3 $14 L L32R,
  A10 A3 MOV.N,  MEMW,  A2 A5 0 S32I.N,  A8 $28 L L32R,  A8 CALLX8,
  A2 -16 MOVI.N,  A10 A10 A2 AND,  A4 A10 A4 OR,  MEMW,  A4 A3 0 S32I.N,  RETW.N,
$C7 L:  END-CODE

CODE ant_tx_cfg   \ .text.ant_tx_cfg $8
  $0 2 LITERALS
    SYMBOL ant_wifitx_cfg  SYMBOL ant_bttx_cfg
$8 L:
  A1 32 ENTRY,  A2 A2 0 8 EXTUI,  A10 A2 MOV.N,  A11 A2 MOV.N,  A8 $0 L L32R,
  A8 CALLX8,
  A10 A2 MOV.N,  A8 $4 L L32R,  A8 CALLX8,
  RETW.N,
$22 L:  END-CODE

CODE ant_rx_cfg   \ .text.ant_rx_cfg $8
  $0 2 LITERALS
    SYMBOL ant_wifirx_cfg  SYMBOL ant_btrx_cfg
$8 L:
  A1 32 ENTRY,  A2 A2 0 8 EXTUI,  A3 A3 0 8 EXTUI,  A4 A4 0 8 EXTUI,  A12 A4 MOV.N,
  A11 A3 MOV.N,  A10 A2 MOV.N,  A8 $0 L L32R,  A8 CALLX8,
  A12 A4 MOV.N,  A11 A3 MOV.N,  A10 A2 MOV.N,  A8 $4 L L32R,  A8 CALLX8,
  RETW.N,
$2E L:  END-CODE

CODE esp_tx_state_out   \ .text.esp_tx_state_out $3C
  $0 15 LITERALS
    $40000000 T,  $80000000 T,  $FFD114C T,  $3FF45104 T,  $3F3F3F3F T,
    $404000 T,  $3FF45108 T,  $3FF4510C T,  $3FF45110 T,
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
$3C L:
  A1 32 ENTRY,  A4 $8 L L32R,  A2 A2 0 8 EXTUI,  A5 A2 A4 ADD,  A5 A5 2 SLLI,
  A10 A5 A5 OR,  A8 $24 L L32R,  A8 CALLX8,
  A6 -512 MOVI,  A10 A10 A6 AND,  A8 222 MOVI,  A10 A10 A8 OR,  A3 A3 0 8 EXTUI,
  MEMW,  A10 A5 0 S32I.N,  A2 A3 $95 L BEQ,
  A4 A3 A4 ADD.N,  A4 A4 2 SLLI,  A10 A4 MOV.N,  A8 $28 L L32R,  A8 CALLX8,
  A10 A10 A6 AND,  A6 223 MOVI,  A10 A10 A6 OR,  MEMW,  A10 A4 0 S32I.N,
  A5 128 MOVI,  NOP.N,  NOP.N,  NOP.N,  A3 $4 L L32R,  $9A L J,
$95 L:
  A3 $0 L L32R,  A5 64 MOVI.N,
$9A L:
  A6 $C L L32R,  A10 A6 MOV.N,  A8 $2C L L32R,  A8 CALLX8,
  A2 $10 L L32R,  A10 A10 A2 AND,  A4 $14 L L32R,  A4 A10 A4 OR,  A7 $18 L L32R,
  A10 A7 MOV.N,  MEMW,  A4 A6 0 S32I.N,  A8 $30 L L32R,  A8 CALLX8,
  A10 A10 A2 AND,  A3 A10 A3 OR,  A4 $1C L L32R,  A10 A4 A4 OR,  MEMW,
  A3 A7 0 S32I.N,  A8 $34 L L32R,  A8 CALLX8,
  A10 A10 A2 AND,  A2 A10 A5 OR,  A3 $20 L L32R,  A10 A3 A3 OR,  MEMW,
  A2 A4 0 S32I,  A8 $38 L L32R,  A8 CALLX8,
  A2 -193 MOVI,  A10 A10 A2 AND,  MEMW,  A10 A3 0 S32I.N,  RETW.N,
$FD L:  END-CODE

CODE phy_chan_dump_cfg   \ .text.phy_chan_dump_cfg $18
  $0 6 LITERALS
    $3FF5CD0C T,  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
$18 L:
  A1 32 ENTRY,  A7 $0 L L32R,  A2 A2 0 8 EXTUI,  A10 A7 A7 OR,  A8 $4 L L32R,
  A8 CALLX8,
  A8 -9 MOVI.N,  A2 A2 0 1 EXTUI,  A10 A10 A8 AND,  A2 A2 3 SLLI,  A10 A2 A10 OR,
  MEMW,  A10 A7 0 S32I.N,  A10 A7 MOV.N,  A8 $8 L L32R,  A8 CALLX8,
  A3 A3 0 8 EXTUI,  A8 -241 MOVI,  A3 A3 4 SLLI,  A10 A10 A8 AND,  A3 A3 0 8 EXTUI,
  A10 A3 A10 OR,  MEMW,  A10 A7 0 S32I.N,  A10 A7 MOV.N,  A8 $C L L32R,  A8 CALLX8,
  A4 A4 0 8 EXTUI,  A8 -3 MOVI,  A4 A4 0 1 EXTUI,  A10 A10 A8 AND,  A4 A4 1 SLLI,
  A10 A4 A10 OR,  MEMW,  A10 A7 0 S32I,  A10 A7 A7 OR,  A8 $10 L L32R,  A8 CALLX8,
  A5 A5 0 8 EXTUI,  A2 -2 MOVI.N,  A10 A10 A2 AND,  A5 A5 0 1 EXTUI,  A5 A10 A5 OR,
  MEMW,  A5 A7 0 S32I.N,  A10 A7 MOV.N,  A8 $14 L L32R,  A8 CALLX8,
  A6 A6 0 8 EXTUI,  A6 A6 0 1 EXTUI,  A8 -5 MOVI.N,  A6 A6 2 SLLI,  A10 A10 A8 AND,
  A10 A6 A10 OR,  MEMW,  A10 A7 0 S32I.N,  RETW.N,
$B8 L:  END-CODE

CODE chan14_mic_cfg   \ .text.chan14_mic_cfg $30
  $0 12 LITERALS
    $3FF5C400 T,  $FFFF9FFF T,  $2000 T,  SYMBOL g_phyFuns
    SYMBOL chan14_mic_most_power  $6000 T,  SYMBOL chip7_sleep_params
    SYMBOL phy_in_most_power_bk  SYMBOL esp_dport_access_reg_read
    SYMBOL phy_set_most_tpw  SYMBOL esp_dport_access_reg_read
    SYMBOL phy_set_most_tpw
$30 L:
  A1 32 ENTRY,  A2 A2 0 8 EXTUI,  A2 1 $9E L BNEI,
  A4 $0 L L32R,  A10 A4 MOV.N,  A8 $20 L L32R,  A8 CALLX8,
  A11 A2 MOV.N,  A3 $4 L L32R,  A15 76 MOVI.N,  A10 A10 A3 AND,  A14 0 MOVI.N,
  A13 6 MOVI.N,  A3 $8 L L32R,  A12 3 MOVI.N,  A3 A10 A3 OR,  A10 103 MOVI,
  A5 $C L L32R,  MEMW,  A3 A4 0 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,
  A3 A5 0 L32I.N,  A3 A3 168 L32I,  A3 CALLX8,
  A11 A2 MOV.N,  A15 78 MOVI.N,  A3 A5 0 L32I.N,  A14 0 MOVI,  A13 6 MOVI,
  A12 4 MOVI,  A10 103 MOVI,  A2 A3 168 L32I,  A2 CALLX8,
  A2 $10 L L32R,  A10 A2 0 L8UI,  A10 A10 7 SEXT,  A8 $24 L L32R,  A8 CALLX8,
  RETW.N,
$9E L:
  A5 $0 L L32R,  A4 0 MOVI,  A10 A5 MOV.N,  A8 $28 L L32R,  A8 CALLX8,
  A15 2 MOVI.N,  A2 $18 L L32R,  A6 A15 MOV.N,  A3 1 MOVI.N,  A14 A4 MOV.N,
  A11 A3 MOV.N,  A8 A2 82 L8UI,  A13 6 MOVI.N,  A6 A4 A8 MOVEQZ,  A2 A2 A6 ADD.N,
  A15 A3 A8 MOVNEZ,  A9 $14 L L32R,  A8 A15 MOV.N,  A9 A10 A9 OR,  A12 3 MOVI.N,
  A10 103 MOVI,  A6 $C L L32R,  MEMW,  A9 A5 0 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP,
  A15 A2 84 L8UI,  A15 A15 A8 SUB,  A15 A15 0 8 EXTUI,  A5 A6 0 L32I,
  A5 A5 168 L32I,  A5 CALLX8,
  A14 A4 MOV.N,  A11 A3 MOV.N,  A4 A6 0 L32I.N,  A10 103 MOVI,  A13 6 MOVI.N,
  A12 4 MOVI,  A15 A2 84 L8UI,  A3 A4 168 L32I,  A3 CALLX8,
  A2 $1C L L32R,  A10 A2 0 L8UI,  A10 A10 7 SEXT,  A8 $2C L L32R,  A8 CALLX8,
  RETW.N,
$11E L:  END-CODE

CODE chan14_mic_enable   \ .text.chan14_mic_enable $18
  $0 6 LITERALS
    SYMBOL chan14_mic_en  SYMBOL chan14_mic_flag  SYMBOL phy_in_most_power_bk
    SYMBOL phy_in_most_power  SYMBOL chan14_mic_most_power  SYMBOL chan14_mic_cfg
$18 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A10 A2 0 8 EXTUI,  A3 A3 0 8 EXTUI,  A10 A8 0 S8I,
  MEMW,  A10 $35 L BNEZ.N,
  A8 $14 L L32R,  A8 CALLX8,
  RETW.N,
  1 ZEROS,
$35 L:
  A8 $4 L L32R,  A9 A8 0 L8UI,  A9 $5C L BNEZ.N,
  A10 1 MOVI.N,  A9 $C L L32R,  A10 A8 0 S8I,  MEMW,  NOP.N,  NOP.N,  NOP.N,  NOP.N,
  A9 A9 0 L8UI,  A8 $8 L L32R,  A9 A8 0 S8I,  MEMW,
$5C L:
  A8 48 MOVI.N,  A3 A3 7 SEXT,  A3 A3 A8 MIN,  NOP.N,  A8 $10 L L32R,  A3 A8 0 S8I,
  MEMW,  RETW.N,
$71 L:  END-CODE

CODE phy_get_adc_rand   \ .text.phy_get_adc_rand $34
  $0 13 LITERALS
    $A0 SYMBOL+ rodata  $3FF48030 T,  SYMBOL adc_ana_conf_org  SYMBOL g_phyFuns
    $F8000000 T,  $3FF460A0 T,  $1000000 T,  $3FF66000 T,  $FA0 T,
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
$34 L:
  A1 64 ENTRY,  A4 $0 L L32R,  A2 A2 0 8 EXTUI,  A3 $4 L L32R,  A10 A3 MOV.N,
  A7 A4 0 L16UI,  A6 A4 2 L16UI,  A5 A4 4 L16UI,  A4 A4 6 L16UI,  A7 A1 8 S16I,
  A4 A1 16 S16I,  A4 A1 14 S16I,  A6 A1 10 S16I,  A5 A1 12 S16I,  MEMW,
  A8 $24 L L32R,  A8 CALLX8,
  A4 $8 L L32R,  A10 A4 0 S32I.N,  A2 $141 L BEQZ,
  A10 1 MOVI.N,  NOP.N,  NOP.N,  NOP.N,  A4 $C L L32R,  A2 A4 0 L32I.N,
  A2 A2 64 L32I,  A2 CALLX8,
  A10 A3 MOV.N,  A8 $28 L L32R,  A8 CALLX8,
  A8 $10 L L32R,  A8 A10 A8 OR,  A2 $14 L L32R,  A10 A2 A2 OR,  MEMW,
  A8 A3 0 S32I.N,  A8 $2C L L32R,  A8 CALLX8,
  A8 $18 L L32R,  A8 A10 A8 OR,  A3 $1C L L32R,  A10 A3 A3 OR,  MEMW,  A8 A2 0 S32I,
  A8 $30 L L32R,  A8 CALLX8,
  A2 $400 MOVI,  A10 A10 A2 OR,  A14 7 MOVI.N,  A2 A4 0 L32I.N,  A12 1 MOVI.N,
  A13 A14 MOV.N,  A11 A12 MOV.N,  A15 0 MOVI.N,  A2 A2 168 L32I,  MEMW,
  A10 A3 0 S32I.N,  A10 103 MOVI,  A2 CALLX8,
  A14 7 MOVI.N,  A13 A14 MOV.N,  A2 A4 0 L32I.N,  A15 0 MOVI.N,  A12 2 MOVI.N,
  A11 1 MOVI,  A10 103 MOVI,  A2 A2 168 L32I,  A2 CALLX8,
  A2 A4 0 L32I.N,  A2 A2 192 L32I,  A2 CALLX8,
  A10 A1 MOV.N,  A2 A4 0 L32I.N,  A3 A1 16 L16UI,  A2 A2 220 L32I,  A7 A1 0 S16I,
  A6 A1 2 S16I,  A5 A1 4 S16I,  A3 A1 6 S16I,  MEMW,  A2 CALLX8,
  A10 0 MOVI.N,  A2 A4 0 L32I.N,  A2 A2 208 L32I,  A2 CALLX8,
  A12 0 MOVI.N,  A10 A12 MOV.N,  A2 A4 0 L32I.N,  A11 1 MOVI.N,  A2 A2 184 L32I,
  A2 CALLX8,
  A14 0 MOVI.N,  A13 A14 MOV.N,  A2 A4 0 L32I.N,  A12 10 MOVI.N,  A11 A1 8 ADDI.N,
  A2 A2 $150 L32I,  A10 $20 L L32R,  A2 CALLX8,
  RETW.N,
  2 ZEROS,
$141 L:
  A5 $C L L32R,  A6 A5 0 L32I.N,  A6 A6 204 L32I,  A6 CALLX8,
  A6 A5 0 L32I.N,  A6 A6 196 L32I,  A6 CALLX8,
  A15 1 MOVI.N,  A14 7 MOVI.N,  A6 A5 0 L32I.N,  A13 A14 MOV.N,  A12 A15 MOV.N,
  A11 A15 MOV.N,  A10 103 MOVI,  A6 A6 168 L32I,  A6 CALLX8,
  A15 1 MOVI.N,  A14 7 MOVI.N,  A6 A5 0 L32I.N,  A10 103 MOVI,  A13 A14 MOV.N,
  A12 2 MOVI.N,  A11 A15 MOV.N,  A6 A6 168 L32I,  A6 CALLX8,
  A10 A2 MOV.N,  A2 A4 0 L32I.N,  MEMW,  A2 A3 0 S32I.N,  NOP.N,  NOP.N,  NOP.N,
  NOP.N,  A2 A5 0 L32I,  A2 A2 64 L32I,  A2 CALLX8,
  RETW.N,
$19A L:  END-CODE

CODE phy_enable_low_rate   \ .text.phy_enable_low_rate $20
  $0 8 LITERALS
    SYMBOL lr_enable  $3FF5C860 T,  $800 T,  $3FF5C87C T,
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL write_txrate_power_offset
$20 L:
  A1 32 ENTRY,  A3 $4 L L32R,  A4 1 MOVI.N,  A10 A3 MOV.N,  A2 $0 L L32R,
  A4 A2 0 S8I,  MEMW,  A8 $10 L L32R,  A8 CALLX8,
  A2 $400 MOVI,  A2 A10 A2 OR,  A10 A3 MOV.N,  MEMW,  A2 A3 0 S32I.N,
  A8 $14 L L32R,  A8 CALLX8,
  A2 $8 L L32R,  A8 A10 A2 OR,  A4 $C L L32R,  A10 A4 A4 OR,  MEMW,  A8 A3 0 S32I,
  A8 $18 L L32R,  A8 CALLX8,
  A2 A10 A2 OR,  A10 1 MOVI.N,  MEMW,  A2 A4 0 S32I.N,  A8 $1C L L32R,  A8 CALLX8,
  RETW.N,
$76 L:  END-CODE

CODE phy_disable_low_rate   \ .text.phy_disable_low_rate $20
  $0 8 LITERALS
    SYMBOL lr_enable  $3FF5C860 T,  $FFFFF7FF T,  $3FF5C87C T,
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL write_txrate_power_offset
$20 L:
  A1 32 ENTRY,  A3 $4 L L32R,  A4 0 MOVI.N,  A10 A3 MOV.N,  A2 $0 L L32R,
  A4 A2 0 S8I,  MEMW,  A8 $10 L L32R,  A8 CALLX8,
  A2 -1025 MOVI,  A2 A10 A2 AND,  A10 A3 MOV.N,  MEMW,  A2 A3 0 S32I.N,
  A8 $14 L L32R,  A8 CALLX8,
  A2 $8 L L32R,  A8 A10 A2 AND,  A4 $C L L32R,  A10 A4 A4 OR,  MEMW,  A8 A3 0 S32I,
  A8 $18 L L32R,  A8 CALLX8,
  A2 A10 A2 AND,  A10 1 MOVI.N,  MEMW,  A2 A4 0 S32I.N,  A8 $1C L L32R,  A8 CALLX8,
  RETW.N,
$76 L:  END-CODE

CODE phy_close_pa   \ .text.phy_close_pa $28
  $0 10 LITERALS
    $3FF460A0 T,  $FFFFF3FF T,  $800 T,  $3FF4609C T,
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
$28 L:
  A1 32 ENTRY,  A2 A2 0 8 EXTUI,  A2 $79 L BEQZ,
  A4 $0 L L32R,  A10 A4 MOV.N,  A8 $10 L L32R,  A8 CALLX8,
  A3 $4 L L32R,  A10 A10 A3 AND,  A2 $8 L L32R,  A3 A10 A2 OR,  A2 $C L L32R,
  A10 A2 MOV.N,  MEMW,  A3 A4 0 S32I.N,  A8 $14 L L32R,  A8 CALLX8,
  A3 -5 MOVI.N,  A3 A10 A3 AND,  A10 A2 MOV.N,  MEMW,  A3 A2 0 S32I.N,
  A8 $18 L L32R,  A8 CALLX8,
  A3 -17 MOVI.N,  A10 A10 A3 AND,  MEMW,  A10 A2 0 S32I.N,  RETW.N,
  3 ZEROS,
$79 L:
  A2 $C L L32R,  A10 A2 MOV.N,  A8 $1C L L32R,  A8 CALLX8,
  A8 4 MOVI.N,  A10 A10 A8 OR,  MEMW,  A10 A2 0 S32I.N,  A10 A2 MOV.N,
  A8 $20 L L32R,  A8 CALLX8,
  A8 16 MOVI.N,  A8 A10 A8 OR,  A3 $0 L L32R,  A10 A3 MOV.N,  MEMW,  A8 A2 0 S32I,
  A8 $24 L L32R,  A8 CALLX8,
  A2 $4 L L32R,  A10 A10 A2 AND,  MEMW,  A10 A3 0 S32I.N,  RETW.N,
$B9 L:  END-CODE

CODE btpwr_backoff   \ .text.btpwr_backoff $10
  $0 4 LITERALS
    $81 SYMBOL+ chip7_sleep_params  SYMBOL chip7_sleep_params
    SYMBOL bt_txpwr_backoff  SYMBOL wr_bt_tx_atten
$10 L:
  A1 32 ENTRY,  A3 $8 L L32R,  A2 A2 0 8 EXTUI,  A13 64 MOVI.N,  A14 0 MOVI,
  A10 8 MOVI,  A12 A3 0 L8UI,  A12 A2 A12 SUB,  A12 A12 0 8 EXTUI,  A9 $4 L L32R,
  A10 $5C L LOOP,
  A8 A9 129 L8UI,  A8 A12 A8 ADD.N,  A8 A8 0 8 EXTUI,  A11 A8 7 SEXT,
  A11 $48 L BGEZ,
  A14 A9 129 S8I,  MEMW,  $5A L J,
  1 ZEROS,
$48 L:
  A13 A11 $54 L BLT,
  A8 A9 129 S8I,  MEMW,  $5A L J,
$54 L:
  A13 A9 129 S8I,  MEMW,
$5A L:
  A9 A9 1 ADDI.N,
$5C L:
  NOP.N,  NOP.N,  NOP,  A10 $0 L L32R,  A8 $C L L32R,  A8 CALLX8,
  A2 A3 0 S8I,  MEMW,  RETW.N,
$74 L:  END-CODE

CODE phy_dig_reg_backup   \ .text.phy_dig_reg_backup $A8
  $0 42 LITERALS
    $3FF5CD0C T,  $3FF5CC0C T,  $3FF5CD08 T,  $3FF5CD04 T,  $3FF5C074 T,
    $3FF5C11C T,  $3FF45104 T,  $3FF45108 T,  $3FF4510C T,  $3FF45110 T,
    $3FF5C860 T,  $3FF5C87C T,  $3FF5C02C T,  $3FF5C018 T,  $3FF5C01C T,
    $3FF5C010 T,  $3FF5C014 T,  $3FF5C044 T,  $3FF5C108 T,  $3FF5C0CC T,
    $3FF5C124 T,  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
$A8 L:
  A1 32 ENTRY,  A2 A2 0 8 EXTUI,  A2 $24E L BEQZ,
  A10 $0 L L32R,  A8 $54 L L32R,  A8 CALLX8,
  A10 A3 0 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP,  A10 $4 L L32R,  A8 $58 L L32R,
  A8 CALLX8,
  A10 A3 4 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A10 $8 L L32R,  A8 $5C L L32R,
  A8 CALLX8,
  A10 A3 8 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP,  A10 $C L L32R,  A8 $60 L L32R,
  A8 CALLX8,
  A10 A3 12 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP,  A10 $10 L L32R,  A8 $64 L L32R,
  A8 CALLX8,
  A10 A3 16 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP,  A10 $14 L L32R,  A8 $68 L L32R,
  A8 CALLX8,
  A10 A3 20 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP,  A10 $18 L L32R,  A8 $6C L L32R,
  A8 CALLX8,
  A10 A3 24 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP,  A10 $1C L L32R,  A8 $70 L L32R,
  A8 CALLX8,
  A10 A3 28 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP,  A10 $20 L L32R,  A8 $74 L L32R,
  A8 CALLX8,
  A10 A3 32 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP,  A10 $24 L L32R,  A8 $78 L L32R,
  A8 CALLX8,
  A10 A3 36 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP,  A10 $28 L L32R,  A8 $7C L L32R,
  A8 CALLX8,
  A10 A3 40 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP,  A10 $2C L L32R,  A8 $80 L L32R,
  A8 CALLX8,
  A10 A3 44 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP,  A10 $30 L L32R,  A8 $84 L L32R,
  A8 CALLX8,
  A10 A3 48 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP,  A10 $34 L L32R,  A8 $88 L L32R,
  A8 CALLX8,
  A10 A3 52 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP,  A10 $38 L L32R,  A8 $8C L L32R,
  A8 CALLX8,
  A10 A3 56 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP,  A10 $3C L L32R,  A8 $90 L L32R,
  A8 CALLX8,
  A10 A3 60 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A10 $40 L L32R,  A8 $94 L L32R,
  A8 CALLX8,
  A10 A3 64 S32I,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A10 $44 L L32R,  A8 $98 L L32R,
  A8 CALLX8,
  A10 A3 68 S32I,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A10 $48 L L32R,  A8 $9C L L32R,
  A8 CALLX8,
  A10 A3 72 S32I,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A10 $4C L L32R,  A8 $A0 L L32R,
  A8 CALLX8,
  A10 A3 76 S32I,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A10 $50 L L32R,  A8 $A4 L L32R,
  A8 CALLX8,
  A10 A3 80 S32I,  $3C5 L J,
$24E L:
  A8 A3 0 L32I.N,  A2 $0 L L32R,  A9 A3 4 L32I.N,  MEMW,  A8 A2 0 S32I.N,  NOP.N,
  NOP.N,  NOP.N,  NOP.N,  A2 $4 L L32R,  A8 A3 8 L32I.N,  MEMW,  A9 A2 0 S32I.N,
  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A2 $8 L L32R,  A9 A3 12 L32I.N,  MEMW,
  A8 A2 0 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A2 $C L L32R,  A8 A3 16 L32I.N,
  MEMW,  A9 A2 0 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A2 $10 L L32R,
  A9 A3 20 L32I.N,  MEMW,  A8 A2 0 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,
  A2 $14 L L32R,  A8 A3 24 L32I.N,  MEMW,  A9 A2 0 S32I.N,  NOP.N,  NOP.N,  NOP.N,
  NOP.N,  A2 $18 L L32R,  A9 A3 28 L32I.N,  MEMW,  A8 A2 0 S32I.N,  NOP.N,  NOP.N,
  NOP.N,  NOP.N,  A2 $1C L L32R,  A8 A3 32 L32I.N,  MEMW,  A9 A2 0 S32I.N,  NOP.N,
  NOP.N,  NOP.N,  NOP.N,  A2 $20 L L32R,  A9 A3 36 L32I.N,  MEMW,  A8 A2 0 S32I.N,
  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A2 $24 L L32R,  A8 A3 40 L32I.N,  MEMW,
  A9 A2 0 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A2 $28 L L32R,  MEMW,
  A8 A2 0 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A8 A3 44 L32I.N,  A2 $2C L L32R,
  A9 A3 48 L32I.N,  MEMW,  A8 A2 0 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,
  A2 $30 L L32R,  A8 A3 52 L32I.N,  MEMW,  A9 A2 0 S32I.N,  NOP.N,  NOP.N,  NOP.N,
  NOP.N,  A2 $34 L L32R,  A9 A3 56 L32I.N,  MEMW,  A8 A2 0 S32I.N,  NOP.N,  NOP.N,
  NOP.N,  NOP.N,  A2 $38 L L32R,  A8 A3 60 L32I.N,  MEMW,  A9 A2 0 S32I.N,  NOP.N,
  NOP.N,  NOP.N,  NOP.N,  A2 $3C L L32R,  A9 A3 64 L32I,  MEMW,  A8 A2 0 S32I.N,
  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A2 $40 L L32R,  A8 A3 68 L32I,  MEMW,
  A9 A2 0 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A2 $44 L L32R,  A9 A3 72 L32I,
  MEMW,  A8 A2 0 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A2 $48 L L32R,
  A8 A3 76 L32I,  MEMW,  A9 A2 0 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,
  A2 $4C L L32R,  A3 A3 80 L32I,  MEMW,  A8 A2 0 S32I.N,  NOP.N,  NOP.N,  NOP.N,
  NOP.N,  A2 $50 L L32R,  MEMW,  A3 A2 0 S32I.N,
$3C5 L:
  A2 21 MOVI.N,  RETW.N,
$3C9 L:  END-CODE

CODE get_iq_est_snr_1   \ .text.get_iq_est_snr_1 $A8
  $0 42 LITERALS
    $3FF46080 T,  $3FF4608C T,  $7FE03 T,  $0 T,  $3FF46084 T,  $3FF46088 T,
    $3FF460CC T,  $3FF460D8 T,  $3FF460D0 T,  $3FF460D4 T,  $3FF460DC T,
    $3FF460E0 T,  $3FF460E4 T,  $1DC SYMBOL+ rodata.str1.4
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL __divdi3  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL __divdi3
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL __divdi3  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL __divdi3
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL __divdi3  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL __divdi3
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL __divdi3  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL __divdi3
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL phy_printf
$A8 L:
  A1 96 ENTRY,  A10 $0 L L32R,  A3 A3 0 8 EXTUI,  A8 $38 L L32R,  A8 CALLX8,
  A5 A10 A10 OR,  A4 A4 0 8 EXTUI,  A10 $4 L L32R,  A3 A1 20 S32I,  A8 $3C L L32R,
  A8 CALLX8,
  A10 A5 A10 ADD.N,  A3 A10 31 SRAI,  A11 A10 18 14 EXTUI,  A12 $8 L L32R,
  A3 A3 14 SLLI,  A11 A11 A3 OR,  A10 A10 14 SLLI,  A13 $C L L32R,  A8 $40 L L32R,
  A8 CALLX8,
  A7 A10 MOV.N,  A10 $10 L L32R,  A11 A1 44 S32I.N,  A4 A1 16 S32I,  A8 $44 L L32R,
  A8 CALLX8,
  A3 A10 MOV.N,  A10 $14 L L32R,  A8 $48 L L32R,  A8 CALLX8,
  A10 A3 A10 SUB,  A3 A10 31 SRAI,  A12 $8 L L32R,  A11 A10 18 14 EXTUI,
  A3 A3 14 SLLI,  A11 A11 A3 OR,  A10 A10 14 SLLI,  A13 $C L L32R,  A8 $4C L L32R,
  A8 CALLX8,
  A5 A10 MOV.N,  A4 A11 A11 OR,  A10 $18 L L32R,  A8 $50 L L32R,  A8 CALLX8,
  A3 A10 MOV.N,  A10 $1C L L32R,  A8 $54 L L32R,  A8 CALLX8,
  A10 A3 A10 ADD.N,  A3 A10 31 SRAI,  A12 $8 L L32R,  A11 A10 18 14 EXTUI,
  A3 A3 14 SLLI,  A11 A11 A3 OR,  A10 A10 14 SLLI,  A13 $C L L32R,  A8 $58 L L32R,
  A8 CALLX8,
  A6 A10 MOV.N,  A3 A11 MOV.N,  A10 $20 L L32R,  A8 $5C L L32R,  A8 CALLX8,
  A11 A10 MOV.N,  A10 $24 L L32R,  A11 A1 36 S32I,  A8 $60 L L32R,  A8 CALLX8,
  A11 A1 36 L32I,  A10 A11 A10 SUB,  A11 A10 31 SRAI,  A14 A10 18 14 EXTUI,
  A12 $8 L L32R,  A11 A11 14 SLLI,  A10 A10 14 SLLI,  A11 A14 A11 OR,
  A13 $C L L32R,  A8 $64 L L32R,  A8 CALLX8,
  A13 A7 A7 MULL,  A12 A5 A5 MULL,  A9 A1 44 L32I.N,  A8 A4 A5 MULL,  A9 A9 A7 MULL,
  A5 A5 A5 MULUH,  A7 A7 A7 MULUH,  A12 A13 A12 ADD.N,  A5 A8 A5 ADDX2,
  A7 A9 A7 ADDX2,  A8 1 MOVI.N,  A12 A13 $1B4 L BLTU,
  A8 0 MOVI.N,
$1B4 L:
  A9 A6 A6 MULL,  A3 A3 A6 MULL,  A6 A6 A6 MULUH,  A7 A7 A5 ADD.N,  A9 A12 A9 ADD.N,
  A7 A8 A7 ADD.N,  A6 A3 A6 ADDX2,  A13 1 MOVI.N,  A9 A12 $1CD L BLTU,
  A13 0 MOVI.N,
$1CD L:
  A8 A10 A10 MULL,  A11 A11 A10 MULL,  A10 A10 A10 MULUH,  A7 A7 A6 ADD.N,
  A8 A9 A8 ADD.N,  A7 A13 A7 ADD.N,  A10 A11 A10 ADDX2,  A3 1 MOVI.N,
  A8 A9 $1E6 L BLTU,
  A3 0 MOVI.N,
$1E6 L:
  A10 A7 A10 ADD.N,  A3 A3 A10 ADD.N,  A10 $0 L L32R,  A8 A2 32 S32I.N,
  A3 A2 36 S32I.N,  A8 A1 48 S32I,  A8 $68 L L32R,  A8 CALLX8,
  A5 A10 MOV.N,  A10 $4 L L32R,  A8 $6C L L32R,  A8 CALLX8,
  A10 A5 A10 SUB,  A5 A10 31 SRAI,  A12 $8 L L32R,  A11 A10 18 14 EXTUI,
  A5 A5 14 SLLI,  A11 A11 A5 OR,  A10 A10 14 SLLI,  A13 $C L L32R,  A8 $70 L L32R,
  A8 CALLX8,
  A7 A10 MOV.N,  A10 $10 L L32R,  A11 A1 40 S32I,  A8 $74 L L32R,  A8 CALLX8,
  A5 A10 MOV.N,  A10 $14 L L32R,  A8 $78 L L32R,  A8 CALLX8,
  A10 A5 A10 ADD.N,  A5 A10 31 SRAI,  A12 $8 L L32R,  A11 A10 18 14 EXTUI,
  A5 A5 14 SLLI,  A11 A11 A5 OR,  A10 A10 14 SLLI,  A13 $C L L32R,  A8 $7C L L32R,
  A8 CALLX8,
  A6 A10 MOV.N,  A10 $18 L L32R,  A11 A1 36 S32I.N,  A8 $80 L L32R,  A8 CALLX8,
  A5 A10 MOV.N,  A10 $1C L L32R,  A8 $84 L L32R,  A8 CALLX8,
  A10 A5 A10 SUB,  A5 A10 31 SRAI,  A12 $8 L L32R,  A11 A10 18 14 EXTUI,
  A5 A5 14 SLLI,  A11 A11 A5 OR,  A10 A10 14 SLLI,  A13 $C L L32R,  A8 $88 L L32R,
  A8 CALLX8,
  A5 A10 MOV.N,  A10 $20 L L32R,  A11 A1 44 S32I,  A8 $8C L L32R,  A8 CALLX8,
  A4 A10 MOV.N,  A10 $24 L L32R,  A8 $90 L L32R,  A8 CALLX8,
  A10 A4 A10 ADD.N,  A4 A10 31 SRAI,  A11 A10 18 14 EXTUI,  A4 A4 14 SLLI,
  A12 $8 L L32R,  A11 A11 A4 OR,  A10 A10 14 SLLI,  A4 A6 A6 MULL,  A13 $C L L32R,
  A8 $94 L L32R,  A8 CALLX8,
  A12 A7 A7 MULL,  A13 1 MOVI.N,  A15 A1 40 L32I.N,  A4 A12 A4 ADD.N,
  A15 A15 A7 MULL,  A7 A7 A7 MULUH,  A14 A1 36 L32I.N,  A7 A15 A7 ADDX2,
  A14 A14 A6 MULL,  A6 A6 A6 MULUH,  A8 A1 48 L32I,  A6 A14 A6 ADDX2,
  A9 A1 44 L32I,  A4 A12 $2F0 L BLTU,
  A13 0 MOVI.N,
$2F0 L:
  A12 A5 A5 MULL,  A9 A9 A5 MULL,  A5 A5 A5 MULUH,  A7 A7 A6 ADD.N,
  A12 A4 A12 ADD.N,  A7 A13 A7 ADD.N,  A5 A9 A5 ADDX2,  A6 1 MOVI.N,
  A12 A4 $309 L BLTU,
  A6 0 MOVI.N,
$309 L:
  A4 A10 A10 MULL,  A11 A11 A10 MULL,  A10 A10 A10 MULUH,  A7 A7 A5 ADD.N,
  A5 A12 A4 ADD.N,  A7 A6 A7 ADD.N,  A10 A11 A10 ADDX2,  A4 1 MOVI.N,
  A5 A12 $322 L BLTU,
  A4 0 MOVI.N,
$322 L:
  A10 A7 A10 ADD.N,  A10 A4 A10 ADD.N,  A5 A2 24 S32I.N,  A5 A8 A5 ADD.N,
  A4 1 MOVI,  A10 A2 28 S32I,  A5 A8 $336 L BLTU,
  A4 0 MOVI,
$336 L:
  A10 A3 A10 ADD.N,  A4 A4 A10 ADD.N,  NOP.N,  A10 $28 L L32R,  A8 $98 L L32R,
  A8 CALLX8,
  A6 A10 4 SRAI,  A10 $2C L L32R,  A8 $9C L L32R,  A8 CALLX8,
  A10 A10 4 SRAI,  A8 A6 A6 MULL,  A3 A10 A10 MULL,  A6 A6 A6 MULSH,
  A3 A8 A3 ADD.N,  A7 A10 A10 MULSH,  A9 1 MOVI.N,  A3 A8 $369 L BLTU,
  A9 0 MOVI.N,
$369 L:
  A6 A6 A7 ADD.N,  A10 $30 L L32R,  A6 A9 A6 ADD.N,  A6 A1 24 S32I.N,
  A8 $A0 L L32R,  A8 CALLX8,
  A11 -1 MOVI.N,  A7 A10 31 SRAI,  A12 A10 1 SRLI,  A6 A1 20 L32I.N,  A9 32 MOVI.N,
  A8 A6 7 ADDI.N,  A8 A8 0 8 EXTUI,  A6 A8 A11 XOR,  A6 SSR,  A12 A12 SRL,  A8 SSL,
  A7 A7 SLL,  A15 A1 16 L32I.N,  A8 SSL,  A6 A10 SLL,  A13 A8 A9 AND,  A10 0 MOVI.N,
  A7 A12 A7 OR,  A7 A6 A13 MOVNEZ,  A6 A10 A13 MOVNEZ,  A15 A10 $3B4 L BNE,
  $4BE L J,
$3B4 L:
  A15 A1 24 L32I.N,  A8 A8 11 ADDI.N,  A11 A8 A11 XOR,  A13 A15 1 SLLI,
  A14 A4 1 SLLI,  A12 A7 1 SLLI,  A11 SSL,  A14 A14 SLL,  A11 SSL,  A12 A12 SLL,
  A11 SSL,  A13 A13 SLL,  A8 SSR,  A11 A3 SRL,  A11 A1 52 S32I.N,  A8 SSR,
  A11 A10 SRA,  A9 A8 A9 AND,  A8 SSR,  A15 A4 SRL,  A11 A1 20 S32I.N,  NOP.N,
  NOP.N,  NOP.N,  NOP.N,  A11 A1 24 L32I.N,  A14 A1 16 S32I.N,  A8 SSR,  A14 A5 SRL,
  A12 A1 28 S32I.N,  A8 SSR,  A12 A6 SRL,  A13 A1 32 S32I.N,  A8 SSR,  A13 A7 SRL,
  A8 SSR,  A8 A11 SRL,  A8 A1 56 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,
  A8 A1 16 L32I.N,  A14 A8 A14 OR,  A8 A10 31 SRAI,  A14 A15 A9 MOVNEZ,
  A11 A1 28 L32I.N,  A15 A10 A9 MOVNEZ,  A12 A11 A12 OR,  A12 A13 A9 MOVNEZ,
  A13 A10 A9 MOVNEZ,  A11 A1 52 L32I.N,  A8 A1 28 S32I.N,  NOP.N,  NOP.N,  NOP.N,
  NOP.N,  A8 A1 32 L32I.N,  A11 A8 A11 OR,  A8 A1 20 L32I.N,  A8 A10 A9 MOVEQZ,
  A11 A1 52 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A11 A1 52 L32I.N,
  A8 A1 16 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A8 A1 56 L32I.N,
  A8 A11 A9 MOVEQZ,  A8 A1 52 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,
  A11 A1 28 L32I.N,  A8 A1 20 L32I.N,  A8 A11 A9 MOVNEZ,  A8 A1 20 S32I.N,  NOP.N,
  NOP.N,  NOP.N,  NOP.N,  A8 A1 56 L32I.N,  A8 A10 A9 MOVNEZ,  A11 A1 16 L32I.N,
  A8 A1 56 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A8 A1 20 L32I.N,
  A11 A1 8 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A11 A1 52 L32I.N,
  A8 A1 12 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A10 $34 L L32R,
  A8 A1 56 L32I.N,  A11 A1 0 S32I.N,  A8 A1 4 S32I.N,  A8 $A4 L L32R,  A8 CALLX8,
$4BE L:
  A11 A1 24 L32I.N,  A6 A2 0 S32I.N,  A7 A2 4 S32I.N,  A5 A2 8 S32I.N,
  A4 A2 12 S32I.N,  A3 A2 16 S32I.N,  A11 A2 20 S32I.N,  RETW.N,
$4CE L:  END-CODE

CODE freq_offset_get_pwr_1   \ .text.freq_offset_get_pwr_1 $3C
  $0 15 LITERALS
    $0 T,  $0 T,  SYMBOL g_phyFuns  $3FF450A8 T,  $3FF460B8 T,  $FDBFFFF T,
    $240000 T,  $F0000000 T,  $220 SYMBOL+ rodata.str1.4  $FFFBFFFF T,
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL get_iq_est_snr_1  SYMBOL phy_printf  SYMBOL esp_dport_access_reg_read
$3C L:
  A1 144 ENTRY,  A6 $8 L L32R,  A3 A3 15 SEXT,  A2 A2 0 8 EXTUI,  A10 1 MOVI.N,
  A4 A4 0 8 EXTUI,  A3 A1 88 S32I,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A3 A6 0 L32I.N,
  A2 A1 84 S32I,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A2 A3 60 L32I.N,  A3 -4 MOVI,
  A5 A1 92 S32I,  A5 A7 0 8 EXTUI,  A5 A1 64 S32I,  A4 A1 76 S32I,  A2 CALLX8,
  A2 $C L L32R,  A10 A2 MOV.N,  A8 $28 L L32R,  A8 CALLX8,
  A10 A10 A3 AND,  A8 A1 88 L32I,  A3 A8 0 2 EXTUI,  A3 A10 A3 OR,  A10 $10 L L32R,
  MEMW,  A3 A2 0 S32I.N,  A8 $2C L L32R,  A8 CALLX8,
  A3 A1 88 L32I,  A4 A3 2 SRAI,  A3 1 MOVI.N,  A2 $14 L L32R,  A4 A4 A2 AND,
  A2 $18 L L32R,  A4 A4 A2 OR,  A5 A1 84 L32I,  A5 SSL,  A5 A3 SLL,  A2 $1C L L32R,
  A10 A10 A2 AND,  A10 A4 A10 OR,  A8 $10 L L32R,  A5 A1 72 S32I,  MEMW,
  A10 A8 0 S32I.N,  A5 A3 $D5 L BGE,
  $1C9 L J,
$D5 L:
  A5 0 MOVI.N,  A7 A5 MOV.N,  NOP.N,  NOP.N,  A4 A1 76 L32I,  A4 SSL,  A2 A3 SLL,
  A2 A2 0 16 EXTUI,  A3 A5 MOV.N,  A2 A1 80 S32I,  A4 A5 MOV.N,  A2 A5 MOV.N,
  A5 A1 68 S32I,
$F5 L:
  A10 1 MOVI.N,  NOP.N,  NOP.N,  NOP.N,  A8 A6 0 L32I.N,  A11 A1 80 L32I,
  A8 A8 $144 L32I,  A8 CALLX8,
  A10 A1 MOV.N,  A12 A1 64 L32I,  A11 A1 76 L32I,  A8 $30 L L32R,  A8 CALLX8,
  A13 1 MOVI.N,  A10 A13 MOV.N,  A9 A1 8 L32I.N,  A9 A7 A9 ADD.N,  A8 A1 0 L32I.N,
  A8 A5 A8 ADD.N,  A11 A6 0 L32I.N,  A12 A1 12 L32I,  A9 A7 $12C L BLTU,
  A13 0 MOVI.N,
$12C L:
  A7 A9 MOV.N,  A4 A4 A12 ADD.N,  A9 A1 4 L32I.N,  A4 A13 A4 ADD.N,  A3 A3 A9 ADD,
  A8 A5 $13C L BLTU,
  A10 0 MOVI.N,
$13C L:
  A9 A11 $148 L32I,  A5 A8 A8 OR,  A3 A10 A3 ADD,  A9 CALLX8,
  A8 A1 64 L32I,  A8 $15C L BEQZ.N,
  A12 A1 68 L32I,  A11 A1 88 L32I,  A10 $20 L L32R,  A8 $34 L L32R,  A8 CALLX8,
$15C L:
  A2 A2 1 ADDI.N,  A2 A2 0 8 EXTUI,  A8 A1 72 L32I,  A2 A1 68 S32I,
  A2 A8 $F5 L BLT,
  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A8 A1 84 L32I,  A2 -1 MOVI.N,  A2 A8 A2 XOR,
  A10 A3 1 SLLI,  A11 A4 1 SLLI,  A8 A1 84 L32I,  A2 SSL,  A11 A11 SLL,  A8 SSR,
  A12 A7 SRL,  A2 SSL,  A2 A10 SLL,  A8 SSR,  A8 A5 SRL,  A5 A1 84 L32I,
  A8 A2 A8 OR,  A2 32 MOVI.N,  A2 A5 A2 AND,  A5 SSR,  A9 A3 SRL,  A5 SSR,
  A13 A4 SRL,  A3 0 MOVI.N,  A12 A11 A12 OR,  A12 A13 A2 MOVNEZ,  A8 A9 A2 MOVNEZ,
  A13 A3 A2 MOVNEZ,  A9 A3 A2 MOVNEZ,  $1D3 L J,
  3 ZEROS,
$1C9 L:
  A8 $0 L L32R,  A12 A8 MOV.N,  A9 $4 L L32R,  A13 A9 MOV.N,
$1D3 L:
  A5 A1 92 L32I,  A10 $10 L L32R,  A8 A5 0 S32I.N,  A9 A5 4 S32I.N,  A12 A1 96 S32I,
  A13 A1 100 S32I,  A8 $38 L L32R,  A8 CALLX8,
  A2 $24 L L32R,  A2 A10 A2 AND,  A10 0 MOVI.N,  A8 $10 L L32R,  MEMW,
  A2 A8 0 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A2 A6 0 L32I.N,  A2 A2 60 L32I.N,
  A2 CALLX8,
  A12 A1 96 L32I,  A2 A12 MOV.N,  A13 A1 100 L32I,  A3 A13 MOV.N,  RETW.N,
$214 L:  END-CODE

CODE get_spur4m_pwr   \ .text.get_spur4m_pwr $34
  $0 13 LITERALS
    $3FF5C02C T,  $FFFFFF T,  $3C000000 T,  $800000 T,  $32000000 T,  $FF7FFFFF T,
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL freq_offset_get_pwr_1  SYMBOL freq_offset_get_pwr_1
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read
$34 L:
  A1 48 ENTRY,  A3 $0 L L32R,  A10 A3 MOV.N,  A8 $18 L L32R,  A8 CALLX8,
  A5 $4 L L32R,  A6 A10 A5 AND,  A10 A3 MOV.N,  A4 $8 L L32R,  A4 A6 A4 OR,  MEMW,
  A4 A3 0 S32I,  A8 $1C L L32R,  A8 CALLX8,
  A15 0 MOVI.N,  A14 A15 MOV.N,  A13 A1 MOV.N,  A4 $C L L32R,  A12 10 MOVI.N,
  A6 A10 A4 OR,  A11 -103 MOVI,  A10 5 MOVI.N,  MEMW,  A6 A3 0 S32I.N,
  A8 $20 L L32R,  A8 CALLX8,
  A15 0 MOVI.N,  A7 A10 MOV.N,  A6 A11 MOV.N,  A14 A15 MOV.N,  A13 A1 MOV.N,
  A12 10 MOVI.N,  A11 -102 MOVI,  A10 5 MOVI.N,  A8 $24 L L32R,  A8 CALLX8,
  A10 A7 A10 ADD.N,  A8 1 MOVI.N,  A10 A7 $9A L BLTU,
  A8 0 MOVI.N,
$9A L:
  A11 A6 A11 ADD.N,  A9 A1 4 L32I.N,  A11 A8 A11 ADD.N,  A11 A11 15 SLLI,
  A6 A9 15 SLLI,  A10 A10 17 15 EXTUI,  A8 A1 0 L32I.N,  A10 A11 A10 OR,
  A8 A8 17 15 EXTUI,  A8 A6 A8 OR,  A9 A9 17 15 EXTUI,  A8 A2 4 S32I.N,
  A10 A2 0 S32I.N,  A10 A3 MOV.N,  A8 A1 0 S32I,  A9 A1 4 S32I.N,  A8 $28 L L32R,
  A8 CALLX8,
  A5 A10 A5 AND,  A10 A3 A3 OR,  A2 $10 L L32R,  A5 A5 A2 OR,  MEMW,  A5 A3 0 S32I,
  A8 $2C L L32R,  A8 CALLX8,
  A4 A10 A4 OR,  A10 A3 MOV.N,  MEMW,  A4 A3 0 S32I.N,  A8 $30 L L32R,  A8 CALLX8,
  A2 $14 L L32R,  A10 A10 A2 AND,  MEMW,  A10 A3 0 S32I.N,  RETW.N,
$FD L:  END-CODE

CODE rx_spur_cal   \ .text.rx_spur_cal $14
  $0 5 LITERALS
    SYMBOL g_phyFuns  $230 SYMBOL+ rodata.str1.4  SYMBOL ets_delay_us
    SYMBOL get_spur4m_pwr  SYMBOL phy_printf
$14 L:
  A1 64 ENTRY,  A3 A3 0 8 EXTUI,  A8 8 MOVI.N,  A2 A2 0 8 EXTUI,  A4 A1 28 S32I.N,
  A4 11 MOVI.N,  A5 5 MOVI.N,  A4 A8 A2 MOVNEZ,  A8 A3 MOV.N,  A7 $0 L L32R,
  A3 A5 MOV.N,  A4 A1 16 S32I.N,  A4 A5 MOV.N,  A5 A8 MOV.N,
$35 L:
  A15 A3 MOV.N,  A12 A2 MOV.N,  A14 2 MOVI.N,  A13 5 MOVI.N,  A11 3 MOVI.N,
  A10 102 MOVI,  A9 A7 0 L32I.N,  A2 $58 L BEQZ.N,
  A14 0 MOVI.N,  A13 2 MOVI.N,  A12 5 MOVI.N,  A9 A9 168 L32I,  A11 4 MOVI.N,
  A9 CALLX8,
  $61 L J,
  1 ZEROS,
$58 L:
  A9 A9 168 L32I,  A10 104 MOVI,  A9 CALLX8,
$61 L:
  A10 100 MOVI,  A8 $8 L L32R,  A8 CALLX8,
  A10 A1 MOV.N,  A8 $C L L32R,  A8 CALLX8,
  A3 5 $80 L BNEI,
  A8 A1 4 L32I.N,  A6 A1 0 L32I.N,  A3 A1 20 S32I.N,  A8 A1 24 S32I.N,  $8D L J,
$80 L:
  A9 A1 0 L32I.N,  A9 A6 $8D L BGEU,
  A6 A9 MOV.N,  A8 A1 4 L32I.N,  A4 A1 20 S32I.N,  A8 A1 24 S32I.N,
$8D L:
  A5 $A2 L BEQZ.N,
  A11 A3 MOV.N,  NOP.N,  NOP.N,  NOP.N,  A10 $4 L L32R,  A12 A1 0 L32I.N,
  A8 $10 L L32R,  A8 CALLX8,
$A2 L:
  A3 A3 1 ADDI.N,  A4 A3 0 8 EXTUI,  A8 A1 16 L32I.N,  A4 A8 $35 L BLTU,
  A4 A1 28 L32I.N,  A8 A1 24 L32I.N,  A6 A4 0 S32I.N,  A8 A4 4 S32I.N,
  A2 $D0 L BEQZ.N,
  A14 0 MOVI.N,  A13 2 MOVI.N,  A12 5 MOVI.N,  NOP.N,  A2 A7 0 L32I.N,
  A11 4 MOVI.N,  A10 102 MOVI,  A15 A1 20 L32I.N,  A2 A2 168 L32I,  A2 CALLX8,
  $E5 L J,
$D0 L:
  A3 A7 0 L32I.N,  A12 A2 MOV.N,  A14 2 MOVI.N,  A13 5 MOVI.N,  A11 3 MOVI.N,
  A15 A1 20 L32I.N,  A10 104 MOVI,  A2 A3 168 L32I,  A2 CALLX8,
$E5 L:
  A2 A1 20 L32I.N,  RETW.N,
$E9 L:  END-CODE

CODE bt_opt_write_mem   \ .text.bt_opt_write_mem $1C
  $0 7 LITERALS
    $3FF4E0C4 T,  $3FF4E0C0 T,  $3FF4E148 T,  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read
$1C L:
  A1 32 ENTRY,  A5 $0 L L32R,  A2 A2 0 8 EXTUI,  A10 A5 MOV.N,  A8 $C L L32R,
  A8 CALLX8,
  A2 A2 A2 ADDX2,  A8 -256 MOVI,  A2 A2 2 ADDI.N,  A10 A10 A8 AND,  A2 A2 0 8 EXTUI,
  A2 A2 A10 OR,  A10 $4 L L32R,  A3 A3 0 8 EXTUI,  A4 A4 0 8 EXTUI,  A3 A3 24 SLLI,
  A4 A4 16 SLLI,  MEMW,  A2 A5 0 S32I.N,  A8 $10 L L32R,  A8 CALLX8,
  A10 A10 0 16 EXTUI,  A3 A3 A4 OR,  A3 A3 A10 OR,  A2 $8 L L32R,  A10 A5 MOV.N,
  MEMW,  A3 A2 0 S32I.N,  A8 $14 L L32R,  A8 CALLX8,
  A2 $200 MOVI,  A2 A10 A2 OR,  A10 A5 MOV.N,  MEMW,  A2 A5 0 S32I.N,
  A8 $18 L L32R,  A8 CALLX8,
  A2 -513 MOVI,  A10 A10 A2 AND,  MEMW,  A10 A5 0 S32I.N,  RETW.N,
$91 L:  END-CODE

CODE bt_rx_spur_opt   \ .text.bt_rx_spur_opt $3C
  $0 15 LITERALS
    SYMBOL g_phyFuns  SYMBOL phy_dac_org  SYMBOL phy_dcur_org
    SYMBOL chip7_phy_init_ctrl  $3FF4E0C4 T,  $FFFF83FF T,  $2C00 T,
    SYMBOL set_chan_freq_sw_start  SYMBOL rx_spur_cal  SYMBOL bt_opt_write_mem
    SYMBOL set_chan_freq_sw_start  SYMBOL rx_spur_cal  SYMBOL rx_spur_cal
    SYMBOL bt_opt_write_mem  SYMBOL esp_dport_access_reg_read
$3C L:
  A1 48 ENTRY,  A12 A2 0 8 EXTUI,  A2 $0 L L32R,  A12 $141 L BEQZ,
  A12 0 MOVI.N,  A11 3 MOVI,  A10 104 MOVI,  A3 A2 0 L32I.N,  A3 A3 152 L32I,
  A3 CALLX8,
  A12 5 MOVI.N,  A11 4 MOVI.N,  A3 A2 0 L32I,  A4 $4 L L32R,  A3 A3 152 L32I,
  A10 A4 0 S8I,  MEMW,  A10 102 MOVI,  A3 CALLX8,
  A11 0 MOVI.N,  A5 $C L L32R,  A3 $8 L L32R,  A12 A5 1 L8UI,  A10 A3 0 S8I,  MEMW,
  A10 44 MOVI.N,  A8 $1C L L32R,  A8 CALLX8,
  A11 0 MOVI.N,  A12 A1 MOV.N,  A10 A11 MOV.N,  A8 $20 L L32R,  A8 CALLX8,
  A8 A2 0 L32I.N,  A12 0 MOVI.N,  A11 3 MOVI.N,  A10 104 MOVI,  A8 A8 152 L32I,
  A8 CALLX8,
  A11 A10 MOV.N,  A10 42 MOVI.N,  A12 A3 0 L8UI,  A8 $24 L L32R,  A8 CALLX8,
  A15 0 MOVI.N,  A14 6 MOVI.N,  A8 A2 0 L32I.N,  A13 7 MOVI,  A12 5 MOVI.N,
  A11 4 MOVI,  A10 102 MOVI,  A8 A8 168 L32I,  A8 CALLX8,
  A11 0 MOVI.N,  A10 84 MOVI.N,  A12 A5 1 L8UI,  A8 $28 L L32R,  A8 CALLX8,
  A11 0 MOVI.N,  A12 A1 MOV.N,  A10 A11 MOV.N,  A8 $2C L L32R,  A8 CALLX8,
  A5 A2 0 L32I.N,  A11 3 MOVI.N,  A12 0 MOVI,  A10 104 MOVI,  A5 A5 152 L32I,
  A5 CALLX8,
  A5 A10 MOV.N,  A12 A1 MOV.N,  A11 0 MOVI,  A10 1 MOVI,  A8 $30 L L32R,  A8 CALLX8,
  A8 A2 0 L32I.N,  A12 5 MOVI.N,  A11 4 MOVI,  A10 102 MOVI,  A8 A8 152 L32I,
  A8 CALLX8,
  A12 A10 MOV.N,  A11 A5 MOV.N,  A10 82 MOVI.N,  A8 $34 L L32R,  A8 CALLX8,
  A5 A2 0 L32I.N,  A12 0 MOVI.N,  A11 3 MOVI.N,  A10 104 MOVI,  A5 A5 160 L32I,
  A13 A4 0 L8UI,  A5 CALLX8,
  A12 5 MOVI.N,  A11 4 MOVI.N,  A2 A2 0 L32I.N,  A10 102 MOVI,  A2 A2 160 L32I,
  $163 L J,
  2 ZEROS,
$141 L:
  A3 A2 0 L32I.N,  A11 3 MOVI.N,  A10 104 MOVI,  A8 $4 L L32R,  A3 A3 160 L32I,
  A13 A8 0 L8UI,  A3 CALLX8,
  A12 5 MOVI.N,  A11 4 MOVI.N,  A2 A2 0 L32I.N,  A10 102 MOVI,  A2 A2 160 L32I,
  A3 $8 L L32R,
$163 L:
  A13 A3 0 L8UI,  A2 CALLX8,
  A3 $10 L L32R,  A10 A3 MOV.N,  A8 $38 L L32R,  A8 CALLX8,
  A2 $14 L L32R,  A10 A10 A2 AND,  A2 $18 L L32R,  A10 A10 A2 OR,  MEMW,
  A10 A3 0 S32I.N,  RETW.N,
$187 L:  END-CODE

CODE phy_init_param_set   \ .text.phy_init_param_set $4
  $0 1 LITERALS
    SYMBOL init_wifi_disable
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A2 0 1 EXTUI,  A2 A8 0 S8I,  MEMW,  RETW.N,
$15 L:  END-CODE

CODE pbus_print   \ .text.pbus_print $24
  $0 9 LITERALS
    SYMBOL g_phyFuns  $238 SYMBOL+ rodata.str1.4  $244 SYMBOL+ rodata.str1.4
    $254 SYMBOL+ rodata.str1.4  $264 SYMBOL+ rodata.str1.4  SYMBOL phy_printf
    SYMBOL phy_printf  SYMBOL phy_printf  SYMBOL phy_printf
$24 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A11 1 MOVI,  A10 0 MOVI,  A3 A2 0 L32I,
  A3 A3 188 L32I,  A3 CALLX8,
  A11 A10 MOV.N,  A10 $4 L L32R,  A8 $14 L L32R,  A8 CALLX8,
  A11 1 MOVI.N,  A10 4 MOVI,  A3 A2 0 L32I,  A3 A3 188 L32I,  A3 CALLX8,
  A3 A10 MOV.N,  A11 1 MOVI.N,  A4 A2 0 L32I.N,  A10 5 MOVI.N,  A4 A4 188 L32I,
  A4 CALLX8,
  A12 A10 MOV.N,  A11 A3 A3 OR,  A10 $8 L L32R,  A8 $18 L L32R,  A8 CALLX8,
  A11 1 MOVI.N,  A10 A11 MOV.N,  A3 A2 0 L32I.N,  A3 A3 188 L32I,  A3 CALLX8,
  A3 A10 MOV.N,  A11 2 MOVI.N,  A4 A2 0 L32I.N,  A10 1 MOVI.N,  A4 A4 188 L32I,
  A4 CALLX8,
  A12 A10 MOV.N,  A11 A3 A3 OR,  A10 $C L L32R,  A8 $1C L L32R,  A8 CALLX8,
  A11 1 MOVI.N,  A10 2 MOVI.N,  A3 A2 0 L32I.N,  A3 A3 188 L32I,  A3 CALLX8,
  A3 A10 MOV.N,  A11 1 MOVI.N,  A4 A2 0 L32I.N,  A10 3 MOVI.N,  A4 A4 188 L32I,
  A4 CALLX8,
  A11 2 MOVI.N,  A4 A10 MOV.N,  A5 A2 0 L32I.N,  A10 A11 MOV.N,  A5 A5 188 L32I,
  A5 CALLX8,
  A5 A10 MOV.N,  A11 2 MOVI.N,  A2 A2 0 L32I.N,  A10 3 MOVI.N,  A2 A2 188 L32I,
  A2 CALLX8,
  A14 A10 MOV.N,  A13 A5 MOV.N,  A10 $10 L L32R,  A12 A4 MOV.N,  A11 A3 MOV.N,
  A8 $20 L L32R,  A8 CALLX8,
  RETW.N,
$DF L:  END-CODE

CODE phy_reg_check   \ .text.phy_reg_check $D0
  $0 52 LITERALS
    $3FF48000 T,  $278 SYMBOL+ rodata.str1.4  $3FF48800 T,
    $288 SYMBOL+ rodata.str1.4  $3FF66000 T,  $298 SYMBOL+ rodata.str1.4
    $3FF00000 T,  $2A8 SYMBOL+ rodata.str1.4  $3FF46000 T,
    $2BC SYMBOL+ rodata.str1.4  $3FF45000 T,  $2CC SYMBOL+ rodata.str1.4
    $3FF5C000 T,  $2DC SYMBOL+ rodata.str1.4  $3FF5D000 T,
    $2EC SYMBOL+ rodata.str1.4  $3FF5C400 T,  $2FC SYMBOL+ rodata.str1.4
    $3FF5C800 T,  $310 SYMBOL+ rodata.str1.4  $3FF5CC00 T,
    $320 SYMBOL+ rodata.str1.4  $3FF51000 T,  $330 SYMBOL+ rodata.str1.4
    $3FF4E000 T,  $344 SYMBOL+ rodata.str1.4  SYMBOL esp_dport_access_reg_read
    SYMBOL phy_printf  SYMBOL esp_dport_access_reg_read  SYMBOL phy_printf
    SYMBOL esp_dport_access_reg_read  SYMBOL phy_printf
    SYMBOL esp_dport_access_reg_read  SYMBOL phy_printf
    SYMBOL esp_dport_access_reg_read  SYMBOL phy_printf
    SYMBOL esp_dport_access_reg_read  SYMBOL phy_printf
    SYMBOL esp_dport_access_reg_read  SYMBOL phy_printf
    SYMBOL esp_dport_access_reg_read  SYMBOL phy_printf
    SYMBOL esp_dport_access_reg_read  SYMBOL phy_printf
    SYMBOL esp_dport_access_reg_read  SYMBOL phy_printf
    SYMBOL esp_dport_access_reg_read  SYMBOL phy_printf
    SYMBOL esp_dport_access_reg_read  SYMBOL phy_printf
    SYMBOL esp_dport_access_reg_read  SYMBOL phy_printf
$D0 L:
  A1 32 ENTRY,  A5 $0 L L32R,  A2 0 MOVI.N,  A4 216 MOVI,  A3 $4 L L32R,
$DE L:
  A10 A2 A5 ADD.N,  A8 $68 L L32R,  A8 CALLX8,
  A12 A10 MOV.N,  A11 A2 MOV.N,  A10 A3 MOV.N,  A2 A2 4 ADDI.N,  A8 $6C L L32R,
  A8 CALLX8,
  A2 A4 $DE L BNE,
  A5 $8 L L32R,  A2 0 MOVI.N,  A4 164 MOVI,  A3 $C L L32R,
$102 L:
  A10 A2 A5 ADD.N,  A8 $70 L L32R,  A8 CALLX8,
  A12 A10 MOV.N,  A11 A2 MOV.N,  A10 A3 MOV.N,  A2 A2 4 ADDI,  A8 $74 L L32R,
  A8 CALLX8,
  A2 A4 $102 L BNE,
  A5 $10 L L32R,  A2 0 MOVI.N,  A4 60 MOVI.N,  A3 $14 L L32R,
$126 L:
  A10 A2 A5 ADD.N,  A8 $78 L L32R,  A8 CALLX8,
  A12 A10 MOV.N,  A11 A2 MOV.N,  A10 A3 MOV.N,  A2 A2 4 ADDI.N,  A8 $7C L L32R,
  A8 CALLX8,
  A2 A4 $126 L BNE,
  A5 $18 L L32R,  A2 0 MOVI.N,  A4 $5BC MOVI,  A3 $1C L L32R,
$14A L:
  A10 A2 A5 ADD.N,  A8 $80 L L32R,  A8 CALLX8,
  A12 A10 MOV.N,  A11 A2 MOV.N,  A10 A3 MOV.N,  A2 A2 4 ADDI.N,  A8 $84 L L32R,
  A8 CALLX8,
  A2 A4 $14A L BNE,
  A5 $20 L L32R,  A2 0 MOVI.N,  A4 252 MOVI,  A3 $24 L L32R,
$16E L:
  A10 A2 A5 ADD.N,  A8 $88 L L32R,  A8 CALLX8,
  A12 A10 MOV.N,  A11 A2 MOV.N,  A10 A3 MOV.N,  A2 A2 4 ADDI.N,  A8 $8C L L32R,
  A8 CALLX8,
  A2 A4 $16E L BNE,
  A5 $28 L L32R,  A2 0 MOVI.N,  A4 $11C MOVI,  A3 $2C L L32R,
$192 L:
  A10 A2 A5 ADD.N,  A8 $90 L L32R,  A8 CALLX8,
  A12 A10 MOV.N,  A11 A2 MOV.N,  A10 A3 MOV.N,  A2 A2 4 ADDI.N,  A8 $94 L L32R,
  A8 CALLX8,
  A2 A4 $192 L BNE,
  A5 $30 L L32R,  A2 0 MOVI.N,  A4 $128 MOVI,  A3 $34 L L32R,
$1B6 L:
  A10 A2 A5 ADD.N,  A8 $98 L L32R,  A8 CALLX8,
  A12 A10 MOV.N,  A11 A2 MOV.N,  A10 A3 MOV.N,  A2 A2 4 ADDI.N,  A8 $9C L L32R,
  A8 CALLX8,
  A2 A4 $1B6 L BNE,
  A5 $38 L L32R,  A2 0 MOVI.N,  A4 108 MOVI,  A3 $3C L L32R,
$1DA L:
  A10 A2 A5 ADD.N,  A8 $A0 L L32R,  A8 CALLX8,
  A12 A10 MOV.N,  A11 A2 MOV.N,  A10 A3 MOV.N,  A2 A2 4 ADDI.N,  A8 $A4 L L32R,
  A8 CALLX8,
  A2 A4 $1DA L BNE,
  A5 $40 L L32R,  A2 0 MOVI.N,  A4 100 MOVI,  A3 $44 L L32R,
$1FE L:
  A10 A2 A5 ADD.N,  A8 $A8 L L32R,  A8 CALLX8,
  A12 A10 MOV.N,  A11 A2 MOV.N,  A10 A3 MOV.N,  A2 A2 4 ADDI.N,  A8 $AC L L32R,
  A8 CALLX8,
  A2 A4 $1FE L BNE,
  A5 $48 L L32R,  A2 0 MOVI.N,  A4 156 MOVI,  A3 $4C L L32R,
$222 L:
  A10 A2 A5 ADD.N,  A8 $B0 L L32R,  A8 CALLX8,
  A12 A10 MOV.N,  A11 A2 MOV.N,  A10 A3 MOV.N,  A2 A2 4 ADDI.N,  A8 $B4 L L32R,
  A8 CALLX8,
  A2 A4 $222 L BNE,
  A5 $50 L L32R,  A2 0 MOVI.N,  A4 $118 MOVI,  A3 $54 L L32R,
$246 L:
  A10 A2 A5 ADD.N,  A8 $B8 L L32R,  A8 CALLX8,
  A12 A10 MOV.N,  A11 A2 MOV.N,  A10 A3 MOV.N,  A2 A2 4 ADDI.N,  A8 $BC L L32R,
  A8 CALLX8,
  A2 A4 $246 L BNE,
  A5 $58 L L32R,  A2 0 MOVI.N,  A4 $12C MOVI,  A3 $5C L L32R,
$26A L:
  A10 A2 A5 ADD.N,  A8 $C0 L L32R,  A8 CALLX8,
  A12 A10 MOV.N,  A11 A2 MOV.N,  A10 A3 MOV.N,  A2 A2 4 ADDI.N,  A8 $C4 L L32R,
  A8 CALLX8,
  A2 A4 $26A L BNE,
  A5 $60 L L32R,  A2 0 MOVI.N,  A4 $16C MOVI,  A3 $64 L L32R,
$28E L:
  A10 A2 A5 ADD.N,  A8 $C8 L L32R,  A8 CALLX8,
  A12 A10 MOV.N,  A11 A2 MOV.N,  A10 A3 MOV.N,  A2 A2 4 ADDI.N,  A8 $CC L L32R,
  A8 CALLX8,
  A2 A4 $28E L BNE,
  RETW.N,
$2A9 L:  END-CODE

CODE phy_i2c_check   \ .text.phy_i2c_check $54
  $0 21 LITERALS
    SYMBOL g_phyFuns  $358 SYMBOL+ rodata.str1.4  $370 SYMBOL+ rodata.str1.4
    $388 SYMBOL+ rodata.str1.4  $3A0 SYMBOL+ rodata.str1.4
    $3B8 SYMBOL+ rodata.str1.4  $3D0 SYMBOL+ rodata.str1.4
    $3E8 SYMBOL+ rodata.str1.4  $404 SYMBOL+ rodata.str1.4
    $41C SYMBOL+ rodata.str1.4  $434 SYMBOL+ rodata.str1.4  SYMBOL phy_printf
    SYMBOL phy_printf  SYMBOL phy_printf  SYMBOL phy_printf  SYMBOL phy_printf
    SYMBOL phy_printf  SYMBOL phy_printf  SYMBOL phy_printf  SYMBOL phy_printf
    SYMBOL phy_printf
$54 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A3 0 MOVI,  A4 $4 L L32R,
$60 L:
  A12 A3 MOV.N,  A11 2 MOVI.N,  A10 106 MOVI,  A8 A2 0 L32I,  A8 A8 152 L32I,
  A8 CALLX8,
  A12 A10 MOV.N,  A11 A3 A3 OR,  A10 A4 A4 OR,  A3 A3 1 ADDI,  A8 $2C L L32R,
  A8 CALLX8,
  A3 10 $60 L BNEI,
  A4 $8 L L32R,  A3 0 MOVI.N,
$89 L:
  A12 A3 MOV.N,  A11 1 MOVI,  A10 103 MOVI,  A8 A2 0 L32I,  A8 A8 152 L32I,
  A8 CALLX8,
  A12 A10 MOV.N,  A11 A3 MOV.N,  A10 A4 MOV.N,  A3 A3 1 ADDI.N,  A8 $30 L L32R,
  A8 CALLX8,
  A3 16 $89 L BNEI,
  A5 $C L L32R,  A3 0 MOVI.N,  A4 13 MOVI.N,
$B2 L:
  A12 A3 MOV.N,  A11 4 MOVI.N,  A8 A2 0 L32I,  A10 102 MOVI,  A8 A8 152 L32I,
  A8 CALLX8,
  A12 A10 MOV.N,  A11 A3 MOV.N,  A10 A5 MOV.N,  A3 A3 1 ADDI,  A8 $34 L L32R,
  A8 CALLX8,
  A3 A4 $B2 L BNE,
  A4 $10 L L32R,  A3 1 MOVI.N,
$D9 L:
  A12 A3 MOV.N,  A11 2 MOVI,  A10 107 MOVI,  A8 A2 0 L32I,  A8 A8 152 L32I,
  A8 CALLX8,
  A12 A10 MOV.N,  A11 A3 MOV.N,  A10 A4 MOV.N,  A3 A3 1 ADDI.N,  A8 $38 L L32R,
  A8 CALLX8,
  A3 12 $D9 L BNEI,
  A5 $14 L L32R,  A3 0 MOVI.N,  A4 11 MOVI.N,
$102 L:
  A12 A3 MOV.N,  A11 0 MOVI.N,  A8 A2 0 L32I,  A10 100 MOVI,  A8 A8 152 L32I,
  A8 CALLX8,
  A12 A10 MOV.N,  A11 A3 MOV.N,  A10 A5 MOV.N,  A3 A3 1 ADDI,  A8 $3C L L32R,
  A8 CALLX8,
  A3 A4 $102 L BNE,
  A4 $18 L L32R,  A3 0 MOVI.N,
$129 L:
  A12 A3 MOV.N,  A11 1 MOVI,  A10 98 MOVI,  A8 A2 0 L32I,  A8 A8 152 L32I,
  A8 CALLX8,
  A12 A10 MOV.N,  A11 A3 MOV.N,  A10 A4 MOV.N,  A3 A3 1 ADDI,  A8 $40 L L32R,
  A8 CALLX8,
  A3 12 $129 L BNEI,
  A5 $1C L L32R,  A3 0 MOVI.N,
$151 L:
  A12 A3 MOV.N,  A11 0 MOVI,  A10 99 MOVI,  A4 A2 0 L32I,  A4 A4 152 L32I,
  A4 CALLX8,
  A12 A10 MOV.N,  A11 A3 MOV.N,  A10 A5 MOV.N,  A3 A3 1 ADDI,  A8 $44 L L32R,
  A8 CALLX8,
  A3 6 $151 L BNEI,
  A5 $20 L L32R,  A3 0 MOVI.N,
$179 L:
  A12 A3 MOV.N,  A11 4 MOVI,  A10 101 MOVI,  A4 A2 0 L32I,  A4 A4 152 L32I,
  A4 CALLX8,
  A12 A10 MOV.N,  A11 A3 MOV.N,  A10 A5 MOV.N,  A3 A3 1 ADDI,  A8 $48 L L32R,
  A8 CALLX8,
  A3 7 $179 L BNEI,
  A5 $24 L L32R,  A3 0 MOVI.N,
$1A1 L:
  A12 A3 MOV.N,  A11 3 MOVI,  A10 104 MOVI,  A4 A2 0 L32I,  A4 A4 152 L32I,
  A4 CALLX8,
  A12 A10 MOV.N,  A11 A3 MOV.N,  A10 A5 MOV.N,  A3 A3 1 ADDI,  A8 $4C L L32R,
  A8 CALLX8,
  A3 3 $1A1 L BNEI,
  A4 $28 L L32R,  A3 0 MOVI.N,
$1C9 L:
  A12 A3 MOV.N,  A11 3 MOVI,  A10 109 MOVI,  A8 A2 0 L32I,  A8 A8 152 L32I,
  A8 CALLX8,
  A12 A10 MOV.N,  A11 A3 MOV.N,  A10 A4 MOV.N,  A3 A3 1 ADDI.N,  A8 $50 L L32R,
  A8 CALLX8,
  A3 10 $1C9 L BNEI,
  RETW.N,
$1ED L:  END-CODE

CODE phy_cal_print   \ .text.phy_cal_print $1C4
  $0 113 LITERALS
    $D4 SYMBOL+ rodata.str1.4  $E0 SYMBOL+ rodata.str1.4
    $EC SYMBOL+ rodata.str1.4  $12FF T,  $100 SYMBOL+ rodata.str1.4  $3FF5A008 T,
    $3FF5A004 T,  $44C SYMBOL+ rodata.str1.4  SYMBOL chip7_sleep_params
    $464 SYMBOL+ rodata.str1.4  $478 SYMBOL+ rodata.str1.4
    $49C SYMBOL+ rodata.str1.4  SYMBOL tx_rf_ana_gain  $12C SYMBOL+ rodata.str1.4
    SYMBOL noise_array  $6 SYMBOL+ noise_array  $4CC SYMBOL+ rodata.str1.4
    $4E0 SYMBOL+ rodata.str1.4  $C SYMBOL+ chip7_sleep_params
    $170 SYMBOL+ rodata.str1.4  $178 SYMBOL+ rodata.str1.4
    $4F8 SYMBOL+ rodata.str1.4  $510 SYMBOL+ rodata.str1.4
    $15C SYMBOL+ rodata.str1.4  $8 SYMBOL+ chip7_sleep_params
    $17C SYMBOL+ rodata.str1.4  $60 SYMBOL+ chip7_sleep_params
    $78 SYMBOL+ chip7_sleep_params  $528 SYMBOL+ rodata.str1.4
    $4 SYMBOL+ chip7_sleep_params  $530 SYMBOL+ rodata.str1.4
    $53C SYMBOL+ rodata.str1.4  $28 SYMBOL+ chip7_sleep_params
    $544 SYMBOL+ rodata.str1.4  $190 SYMBOL+ rodata.str1.4
    $1A0 SYMBOL+ rodata.str1.4  $1A8 SYMBOL+ rodata.str1.4
    $1B8 SYMBOL+ rodata.str1.4  $558 SYMBOL+ rodata.str1.4
    $5 SYMBOL+ chip7_sleep_params  $56C SYMBOL+ rodata.str1.4
    $6 SYMBOL+ chip7_sleep_params  $578 SYMBOL+ rodata.str1.4
    $18 SYMBOL+ chip7_sleep_params  $584 SYMBOL+ rodata.str1.4
    $58C SYMBOL+ rodata.str1.4  SYMBOL phy_rxrf_dc  $24 SYMBOL+ phy_rxrf_dc
    $59C SYMBOL+ rodata.str1.4  $88 SYMBOL+ phy_rxrf_dc
    $5B0 SYMBOL+ rodata.str1.4  SYMBOL phy_rxbb_dc  $10 SYMBOL+ phy_rxbb_dc
    $5C4 SYMBOL+ rodata.str1.4  SYMBOL phy_printf
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL phy_printf  SYMBOL phy_printf  SYMBOL phy_printf  SYMBOL phy_printf
    SYMBOL phy_printf  SYMBOL phy_printf  SYMBOL phy_printf  SYMBOL phy_printf
    SYMBOL phy_printf  SYMBOL phy_printf  SYMBOL phy_printf  SYMBOL phy_printf
    SYMBOL phy_printf  SYMBOL phy_printf  SYMBOL phy_printf  SYMBOL phy_printf
    SYMBOL phy_printf  SYMBOL phy_printf  SYMBOL phy_printf  SYMBOL phy_printf
    SYMBOL phy_printf  SYMBOL phy_printf  SYMBOL phy_printf  SYMBOL phy_printf
    SYMBOL phy_printf  SYMBOL phy_printf  SYMBOL phy_printf  SYMBOL phy_printf
    SYMBOL phy_printf  SYMBOL phy_printf  SYMBOL phy_printf  SYMBOL phy_printf
    SYMBOL phy_printf  SYMBOL phy_printf  SYMBOL phy_printf  SYMBOL phy_printf
    SYMBOL phy_printf  SYMBOL phy_printf  SYMBOL phy_printf  SYMBOL phy_printf
    SYMBOL phy_printf  SYMBOL phy_printf  SYMBOL phy_printf  SYMBOL phy_printf
    SYMBOL phy_printf  SYMBOL phy_printf  SYMBOL phy_printf  SYMBOL phy_printf
    SYMBOL phy_printf  SYMBOL phy_printf  SYMBOL phy_printf  SYMBOL phy_printf
    SYMBOL phy_printf  SYMBOL phy_printf  SYMBOL phy_printf  SYMBOL phy_printf
$1C4 L:
  A1 64 ENTRY,  A14 $0 L L32R,  A13 $4 L L32R,  A12 $8 L L32R,  A11 $C L L32R,
  A10 $10 L L32R,  A8 $D8 L L32R,  A8 CALLX8,
  A10 $14 L L32R,  A8 $DC L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  A10 $18 L L32R,  A8 $E0 L L32R,  A8 CALLX8,
  A12 A10 MOV.N,  A11 A2 MOV.N,  A10 $1C L L32R,  A8 $E4 L L32R,  A8 CALLX8,
  A2 $20 L L32R,  A3 A2 MOV.N,  A11 A2 0 L32I.N,  A10 $24 L L32R,  A2 A2 192 L32I,
  A8 $E8 L L32R,  A8 CALLX8,
  A4 A2 16 16 EXTUI,  A13 A2 8 SRLI,  A11 A4 MOV.N,  A10 $28 L L32R,
  A12 A2 0 8 EXTUI,  A13 A13 7 SEXT,  A4 A1 16 S32I.N,  A8 $EC L L32R,  A8 CALLX8,
  A8 $20 L L32R,  A15 A3 87 L8UI,  A14 A3 86 L8UI,  A13 A3 85 L8UI,  A12 A3 84 L8UI,
  A11 A3 80 L8UI,  A2 A8 88 L8UI,  A3 A8 89 L8UI,  A10 $2C L L32R,  A3 A1 4 S32I,
  A2 A1 0 S32I,  A8 $F0 L L32R,  A8 CALLX8,
  A2 $30 L L32R,  A10 $34 L L32R,  A12 A2 2 L16UI,  A11 A2 0 L16UI,  A8 $F4 L L32R,
  A8 CALLX8,
  A2 $38 L L32R,  A4 $3C L L32R,  A3 $40 L L32R,
$26B L:
  A10 A3 MOV.N,  A11 A2 0 L16SI,  A2 A2 2 ADDI,  A8 $F8 L L32R,  A8 CALLX8,
  A4 A2 $26B L BNE,
  A10 $44 L L32R,  A8 $FC L L32R,  A8 CALLX8,
  A4 $20 L L32R,  A5 A4 MOV.N,  A3 $48 L L32R,  A2 $4C L L32R,
$290 L:
  A5 A5 3 ADDI.N,  A10 A2 MOV.N,  A11 A5 61 L8UI,  A8 $100 L L32R,  A8 CALLX8,
  A5 A3 $290 L BNE,
  A5 A4 MOV.N,  A6 $50 L L32R,  A10 A6 MOV.N,  A8 $104 L L32R,  A8 CALLX8,
  A10 $54 L L32R,  A8 $108 L L32R,  A8 CALLX8,
$2B6 L:
  A5 A5 3 ADDI.N,  A10 A2 A2 OR,  A11 A5 62 L8UI,  A8 $10C L L32R,  A8 CALLX8,
  A3 A5 $2B6 L BNE,
  A10 A6 A6 OR,  A8 $110 L L32R,  A8 CALLX8,
  A10 $58 L L32R,  A5 A4 A4 OR,  A8 $114 L L32R,  A8 CALLX8,
$2DC L:
  A5 A5 3 ADDI.N,  A10 A2 MOV.N,  A11 A5 63 L8UI,  A8 $118 L L32R,  A8 CALLX8,
  A3 A5 $2DC L BNE,
  A10 A6 A6 OR,  A8 $11C L L32R,  A8 CALLX8,
  A10 $5C L L32R,  A3 A4 A4 OR,  A8 $120 L L32R,  A8 CALLX8,
  A5 $60 L L32R,
$304 L:
  A3 A3 2 ADDI.N,  A10 A2 MOV.N,  A11 A3 2 L16SI,  A8 $124 L L32R,  A8 CALLX8,
  A5 A3 $304 L BNE,
  A10 A6 MOV.N,  A8 $128 L L32R,  A8 CALLX8,
  A10 $64 L L32R,  A8 $12C L L32R,  A8 CALLX8,
  A7 $68 L L32R,  A3 $6C L L32R,
$32B L:
  A10 A2 MOV.N,  A11 A7 0 L8UI,  A7 A7 1 ADDI,  A11 A11 7 SEXT,  A8 $130 L L32R,
  A8 CALLX8,
  A3 A7 $32B L BNE,
  A10 A6 A6 OR,  A8 $134 L L32R,  A8 CALLX8,
  A10 $70 L L32R,  A7 A4 A4 OR,  A8 $138 L L32R,  A8 CALLX8,
  A3 $78 L L32R,
$357 L:
  A13 15 MOVI.N,  A10 A3 MOV.N,  A7 A7 2 ADDI.N,  A8 A7 50 L16UI,  A9 A8 6 5 EXTUI,
  A11 A9 7 SEXT,  A8 A8 0 6 EXTUI,  A12 A8 7 SEXT,  A13 A11 $372 L BGE,
  A11 A9 -32 ADDI,
$372 L:
  A9 31 MOVI.N,  A9 A12 $37A L BGE,
  A12 A8 -64 ADDI,
$37A L:
  A8 $13C L L32R,  A8 CALLX8,
  A13 $74 L L32R,  A7 A13 $357 L BNE,
  A10 A6 MOV.N,  A8 $140 L L32R,  A8 CALLX8,
  A10 $7C L L32R,  A7 A4 A4 OR,  A8 $144 L L32R,  A8 CALLX8,
$39A L:
  A7 A7 8 ADDI.N,  A10 $84 L L32R,  A14 A7 10 L16UI,  A13 A7 8 L16UI,
  A12 A7 6 L16UI,  A11 A7 4 L16UI,  A8 $148 L L32R,  A8 CALLX8,
  A8 $80 L L32R,  A8 A7 $39A L BNE,
  A10 A6 A6 OR,  A8 $14C L L32R,  A8 CALLX8,
  A10 $88 L L32R,  A7 A4 A4 OR,  A8 $150 L L32R,  A8 CALLX8,
$3CC L:
  A7 A7 1 ADDI.N,  A10 $8C L L32R,  A11 A7 119 L8UI,  A8 $154 L L32R,  A8 CALLX8,
  A5 A7 $3CC L BNE,
  A10 A6 MOV.N,  A8 $158 L L32R,  A8 CALLX8,
  A10 $90 L L32R,  A7 A4 A4 OR,  A8 $15C L L32R,  A8 CALLX8,
$3F1 L:
  A7 A7 1 ADDI.N,  A10 A2 MOV.N,  A11 A7 128 L8UI,  A11 A11 7 SEXT,  A8 $160 L L32R,
  A8 CALLX8,
  A5 A7 $3F1 L BNE,
  A10 A6 MOV.N,  A8 $164 L L32R,  A8 CALLX8,
  A9 $20 L L32R,  A7 A4 MOV.N,  A11 A9 144 L16UI,  A10 $94 L L32R,  A8 $168 L L32R,
  A8 CALLX8,
  A10 $98 L L32R,  A8 $16C L L32R,  A8 CALLX8,
$426 L:
  A7 A7 1 ADDI.N,  A10 A2 MOV.N,  A11 A7 138 L8UI,  A11 A11 7 SEXT,  A8 $170 L L32R,
  A8 CALLX8,
  A13 $9C L L32R,  A13 A7 $426 L BNE,
  A10 A6 A6 OR,  A8 $174 L L32R,  A8 CALLX8,
  A10 $A0 L L32R,  A2 A4 MOV.N,  A8 $178 L L32R,  A8 CALLX8,
  A7 31 MOVI.N,
$452 L:
  A13 15 MOVI.N,  A8 A2 146 L16UI,  A10 A3 MOV.N,  A9 A8 6 5 EXTUI,  A11 A9 7 SEXT,
  A8 A8 0 6 EXTUI,  A12 A8 7 SEXT,  A2 A2 2 ADDI.N,  A13 A11 $46D L BGE,
  A11 A9 -32 ADDI,
$46D L:
  A7 A12 $473 L BGE,
  A12 A8 -64 ADDI,
$473 L:
  A8 $17C L L32R,  A8 CALLX8,
  A8 $A4 L L32R,  A2 A8 $452 L BNE,
  A10 A6 A6 OR,  A8 $180 L L32R,  A8 CALLX8,
  A10 $A8 L L32R,  A7 A4 A4 OR,  A8 $184 L L32R,  A8 CALLX8,
  A2 $AC L L32R,
$497 L:
  A7 A7 8 ADDI.N,  A10 $84 L L32R,  A14 A7 150 L16UI,  A13 A7 148 L16UI,
  A12 A7 146 L16UI,  A11 A7 144 L16UI,  A8 $188 L L32R,  A8 CALLX8,
  A2 A7 $497 L BNE,
  A10 A6 MOV.N,  A8 $18C L L32R,  A8 CALLX8,
  A10 $B0 L L32R,  A7 15 MOVI.N,  A8 $190 L L32R,  A8 CALLX8,
  A2 31 MOVI.N,
$4C6 L:
  A10 A3 MOV.N,  A8 A4 56 L16UI,  A4 A4 2 ADDI,  A9 A8 6 5 EXTUI,  A11 A9 7 SEXT,
  A8 A8 0 6 EXTUI,  A12 A8 7 SEXT,  A7 A11 $4E0 L BGE,
  A11 A9 -32 ADDI,
$4E0 L:
  A2 A12 $4E6 L BGE,
  A12 A8 -64 ADDI,
$4E6 L:
  A8 $194 L L32R,  A8 CALLX8,
  A5 A4 $4C6 L BNE,
  A10 A6 A6 OR,  A8 $198 L L32R,  A8 CALLX8,
  A10 $B4 L L32R,  A8 $19C L L32R,  A8 CALLX8,
  A4 $B8 L L32R,  A2 $BC L L32R,
$507 L:
  A10 A3 MOV.N,  A11 A4 0 L32I.N,  A4 A4 4 ADDI.N,  A12 A11 15 SEXT,
  A11 A11 16 SRAI,  A8 $1A0 L L32R,  A8 CALLX8,
  A2 A4 $507 L BNE,
  A10 A6 MOV.N,  A8 $1A4 L L32R,  A8 CALLX8,
  A10 $C0 L L32R,  A8 $1A8 L L32R,  A8 CALLX8,
  A4 $C4 L L32R,
$530 L:
  A12 $100 MOVI,  A11 A12 MOV.N,  A5 A2 0 L32I.N,  A5 $542 L BEQZ.N,
  A11 A5 16 SRAI,  A12 A5 15 SEXT,  $542 L J,
$542 L:
  A2 A2 4 ADDI.N,  A10 A3 A3 OR,  A8 $1AC L L32R,  A8 CALLX8,
  A4 A2 $530 L BNE,
  A10 A6 MOV.N,  A8 $1B0 L L32R,  A8 CALLX8,
  A10 $C8 L L32R,  A8 $1B4 L L32R,  A8 CALLX8,
  A2 $CC L L32R,  A4 $D0 L L32R,
$567 L:
  A10 A3 MOV.N,  A11 A2 0 L32I.N,  A2 A2 4 ADDI.N,  A12 A11 15 SEXT,
  A11 A11 16 SRAI,  A8 $1B8 L L32R,  A8 CALLX8,
  A4 A2 $567 L BNE,
  A10 A6 A6 OR,  A8 $1BC L L32R,  A8 CALLX8,
  A11 A1 16 L32I.N,  A10 $D4 L L32R,  A8 $1C0 L L32R,  A8 CALLX8,
  RETW.N,
$592 L:  END-CODE

CODE phy_wifi_bb_reset   \ .text.phy_wifi_bb_reset $10
  $0 4 LITERALS
    $3FF00024 T,  SYMBOL esp_dport_access_reg_read  SYMBOL ets_delay_us
    SYMBOL esp_dport_access_reg_read
$10 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A10 A2 MOV.N,  A8 $4 L L32R,  A8 CALLX8,
  A8 1 MOVI.N,  A10 A10 A8 OR,  MEMW,  A10 A2 0 S32I.N,  A10 A8 MOV.N,
  A8 $8 L L32R,  A8 CALLX8,
  A10 A2 MOV.N,  A8 $C L L32R,  A8 CALLX8,
  A8 -2 MOVI.N,  A10 A10 A8 AND,  MEMW,  A10 A2 0 S32I.N,  RETW.N,
$44 L:  END-CODE

LINK-SYMBOLS   \ the cells of other packages that wait for these names
END-PACKAGE  PREVIOUS   \ not in the search order: ALSO name gives its words (README.md)
