\ phydata.fs - data for ESP32-PHY, made by tools/pick.fs
\
\ tools/pick.fs made this file from the pick list
\ lib/esp32/esp32/phypick.fs: do not change it by hand. It has the
\ data that the functions of the pick list need, each line as in its
\ package file of the quarry. Changes: the frames of the packages are
\ not here, and a name that two objects have gets the tag of its object
\ (tag:name).
\
\ From:
\   blob/esp32/libphy/phy_chip_v7.fs:
\     phy_chip_v7.o of libphy.a (ESP32), 34 of its 50 data objects
\   blob/esp32/libphy/phy_chip_v7_ana.fs:
\     phy_chip_v7_ana.o of libphy.a (ESP32), 18 of its 26 data objects
\   blob/esp32/libphy/phy_chip_v7_cal.fs:
\     phy_chip_v7_cal.o of libphy.a (ESP32), 24 of its 24 data objects
\ SHA-256 of the archives:
\   blob/esp32/libphy/libphy.a
\     c2521071695734afa8f6f1ca3d3b604a1d08dd63fe52060449a11cfe8bac5740
\ Copyright Espressif Systems (Shanghai) Co., Ltd., Apache License 2.0
\ (blob/esp32/LICENSE). blob/esp32/s2fs.py rewrote the code of the
\ objects as CODE words for the assembler XTENSA (blob/esp32/README.md).
\ New names: chip:rodata.str1.4 chip:rodata ana:rodata ana:rodata.str1.4
\   cal:rodata.str1.4 cal:rodata

\ ==== phy_chip_v7.o of libphy.a (ESP32) ===============================
196 DATA: chip7_sleep_params
94 DATA: chip7_phy_init_ctrl
1 DATA: set_most_tpw
16 DATA: phy_rxbb_dc
136 DATA: phy_rxrf_dc
2 DATA: pwrdet_offset
CREATE chip:rodata   \ .rodata, 168 bytes
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
CREATE chip:rodata.str1.4   \ .rodata.str1.4, 1489 bytes
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
CREATE CSWTCH$356   \ .rodata.CSWTCH$356, 6 bytes
  $00 C,  $00 C,  $20 C,  $00 C,  $30 C,  $00 C,
4 DATA: g_phyFuns   \ .bss.g_phyFuns
1 DATA: bt_dig_gain_force   \ .bss.bt_dig_gain_force
1 DATA: init_wifi_disable   \ .bss.init_wifi_disable
1 DATA: corr_cca_thr   \ .bss.corr_cca_thr
1 DATA: lr_enable   \ .bss.lr_enable
1 DATA: chan14_mic_en   \ .bss.chan14_mic_en
1 DATA: re_entry   \ .bss.re_entry
1 DATA: force_hw_set_freq_flag   \ .bss.force_hw_set_freq_flag
1 DATA: phy_init_flag   \ .bss.phy_init_flag
1 DATA: phy_set_most_tpw_index   \ .bss.phy_set_most_tpw_index
2 DATA: target_power_backoff_qdb   \ .bss.target_power_backoff_qdb
1 DATA: phy_set_most_tpw_flag   \ .bss.phy_set_most_tpw_flag
1 DATA: phy_set_most_tpw_disbg   \ .bss.phy_set_most_tpw_disbg
1 DATA: chan14_mic_most_power   \ .data.chan14_mic_most_power
  $30  1 chan14_mic_most_power INIT
1 DATA: phy_in_most_power_bk   \ .data.phy_in_most_power_bk
  $4E  1 phy_in_most_power_bk INIT
1 DATA: phy_in_most_power   \ .data.phy_in_most_power
  $4E  1 phy_in_most_power INIT
2 DATA: phy_meas_noise_floor   \ .bss.phy_meas_noise_floor
1 DATA: sw_scan_mode   \ .bss.sw_scan_mode
2 DATA: bt_tx_pa_gain   \ .data.bt_tx_pa_gain
  $7A  $00  2 bt_tx_pa_gain INIT
4 DATA: tx_rf_ana_gain   \ .data.tx_rf_ana_gain
  $5F  $00  $20  $01  4 tx_rf_ana_gain INIT
1 DATA: wifi_bbgain_max   \ .data.wifi_bbgain_max
  $16  1 wifi_bbgain_max INIT
9 DATA: bt_rx_gain_swp   \ .data.bt_rx_gain_swp
  $00  $04  $30  $34  $38  $40  $44  $50  $54  9 bt_rx_gain_swp INIT
1 DATA: rfbb_gain_swp   \ .data.rfbb_gain_swp
  $5C  1 rfbb_gain_swp INIT
12 DATA: wifi_rx_gain_swp   \ .data.wifi_rx_gain_swp
  $00  $04  $30  $34  $38  $40  $44  $48  $50  $54  $58  $5C  12 wifi_rx_gain_swp INIT
\ ==== phy_chip_v7_ana.o of libphy.a (ESP32) ===========================
11 DATA: freq_i2c_addr
20 DATA: bt_wifi_chan_data
CREATE ana:rodata.str1.4   \ .rodata.str1.4, 90 bytes
  ," set_chan error! %d"  10 C,  0 C,
  ," debug mode FAIL"  10 C,  0 C,  0 C,  0 C,  0 C,
  ," error: pll_cal exceeds 2ms!!!"  10 C,  0 C,  0 C,
  ," i=%d, "  0 C,  0 C,
  ," 0x%x, "  0 C,  0 C,
  10 C,  0 C,
CREATE ana:rodata   \ .rodata, 12 bytes
  $00 C,  $01 C,  $00 C,  $01 C,  $00 C,  $01 C,  $00 C,  $01 C,
  $30 C,  $80 C,  $08 C,  $71 C,
CREATE rodata.bt_i2c_write_set   \ .rodata.bt_i2c_write_set, 36 bytes
  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,   \ 9 jumps into bt_i2c_write_set
CREATE rodata.bt_get_i2c_data   \ .rodata.bt_get_i2c_data, 44 bytes
  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,   \ 11 jumps into bt_get_i2c_data
CREATE CSWTCH$169   \ .rodata.CSWTCH$169, 16 bytes
  $00 C,  $00 C,  $D0 C,  $41 C,  $00 C,  $00 C,  $C0 C,  $41 C,  $00 C,  $00 C,
  $20 C,  $42 C,  $00 C,  $00 C,  $00 C,  $42 C,
CREATE CSWTCH$133   \ .rodata.CSWTCH$133, 48 bytes
  $00 C,  $08 C,  $00 C,  $00 C,  $00 C,  $02 C,  $00 C,  $00 C,  $00 C,  $01 C,
  $00 C,  $00 C,  $00 C,  $00 C,  $01 C,  $00 C,  $00 C,  $00 C,  $02 C,  $00 C,
  $00 C,  $04 C,  $00 C,  $00 C,  $00 C,  $80 C,  $00 C,  $00 C,  $00 C,  $00 C,
  $00 C,  $00 C,  $00 C,  $20 C,  $00 C,  $00 C,  $00 C,  $10 C,  $00 C,  $00 C,
  $00 C,  $00 C,  $00 C,  $00 C,  $00 C,  $40 C,  $00 C,  $00 C,
1 DATA: dreg_1p2_set   \ .data.dreg_1p2_set
  $26  1 dreg_1p2_set INIT
1 DATA: btpwr_atten_offset   \ .bss.btpwr_atten_offset
1 DATA: btpwr_tsen_old   \ .bss.btpwr_tsen_old
1 DATA: btpwr_tsen_init   \ .bss.btpwr_tsen_init
2 DATA: rfpll_offset_delta   \ .bss.rfpll_offset_delta
1 DATA: phy_wifi_pll_track_en   \ .data.phy_wifi_pll_track_en
  $01  1 phy_wifi_pll_track_en INIT
1 DATA: phy_sw_set_chan_en   \ .bss.phy_sw_set_chan_en
1 DATA: phy_force_wifi_chan_en   \ .bss.phy_force_wifi_chan_en
1 DATA: phy_freq_wifi_only   \ .bss.phy_freq_wifi_only
2 DATA: phy_freq_offset   \ .bss.phy_freq_offset
\ ==== phy_chip_v7_cal.o of libphy.a (ESP32) ===========================
17 DATA: phy_chan_pwr_index
17 DATA: phy_chan_target_power
8 DATA: phy_chan_gain_table
CREATE cal:rodata   \ .rodata, 126 bytes
  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,   \ 9 jumps into get_rate_fcc_index
  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,   \ 9 jumps into get_rate_fcc_index
  $7F C,  $6F C,  $5F C,  $7A C,  $5A C,  $69 C,  $75 C,  $55 C,  $74 C,  $54 C,
  $44 C,  $60 C,  $40 C,  $20 C,  $10 C,  $00 C,  $00 C,  $00 C,  $00 C,  $00 C,
  $0A C,  $00 C,  $05 C,  $00 C,  $00 C,  $00 C,  $F8 C,  $FF C,  $EE C,  $FF C,
  $E7 C,  $FF C,  $DE C,  $FF C,  $D4 C,  $FF C,  $C9 C,  $FF C,  $BF C,  $FF C,
  $B9 C,  $FF C,  $AC C,  $FF C,  $A0 C,  $FF C,  $8F C,  $FF C,  $81 C,  $FF C,
  $69 C,  $FF C,  $61 C,  $FF C,
CREATE cal:rodata.str1.4   \ .rodata.str1.4, 321 bytes
  ," 1,target_power,%d, %d, p_error,%d, correct_v,%d, error_accum"
  ," , %d, tot_p,%d, coatt,%d, rf_g,%x,%x, txq_rate,%d, %d, chan,"
  ," %d, rate_att,%d"  10 C,  0 C,  0 C,  0 C,  0 C,
  ," T:%d,0x%x,0x%x,%d"  10 C,  0 C,  0 C,
  ,"  (%d,%d) "  0 C,  0 C,  0 C,
  ," %d,%d "  0 C,  0 C,
  ," stage %d: CGAIN=%d FGAIN=%d, (%d,%d) %d; "  0 C,  0 C,  0 C,
  10 C,  0 C,  0 C,  0 C,
  ," %d, %d; "  0 C,  0 C,  0 C,  0 C,
  ," reg_i=%d, indata=%d"  10 C,  0 C,  0 C,  0 C,  0 C,
  ," i=%02d  acc=%03d  tgt=%03d  err=%03d  ser=%03d att=%03d"  10 C,  0 C,
CREATE CSWTCH$289   \ .rodata.CSWTCH$289, 4 bytes
  $07 C,  $01 C,  $03 C,  $0B C,
CREATE CSWTCH$285   \ .rodata.CSWTCH$285, 11 bytes
  $00 C,  $03 C,  $03 C,  $03 C,  $03 C,  $01 C,  $03 C,  $03 C,  $03 C,  $03 C,
  $02 C,
CREATE CSWTCH$279   \ .rodata.CSWTCH$279, 3 bytes
  $01 C,  $06 C,  $0B C,
CREATE CSWTCH$262   \ .rodata.CSWTCH$262, 8 bytes
  $20 C,  $00 C,  $A0 C,  $00 C,  $20 C,  $01 C,  $30 C,  $00 C,
CREATE CSWTCH$252   \ .rodata.CSWTCH$252, 8 bytes
  $05 C,  $03 C,  $01 C,  $00 C,  $06 C,  $04 C,  $02 C,  $00 C,
CREATE CSWTCH$250   \ .rodata.CSWTCH$250, 6 bytes
  $0E C,  $0A C,  $08 C,  $07 C,  $06 C,  $05 C,
CREATE dco_gain_coarse$4714   \ .rodata.dco_gain_coarse$4714, 6 bytes
  $50 C,  $47 C,  $3F C,  $38 C,  $32 C,  $2C C,
1 DATA: wifi_txband_ht40   \ .data.wifi_txband_ht40
  $C3  1 wifi_txband_ht40 INIT
1 DATA: wifi_txband   \ .data.wifi_txband
  $5F  1 wifi_txband INIT
1 DATA: force_tx_gain_en   \ .bss.force_tx_gain_en
1 DATA: phy_pwdet_onetime_flag   \ .bss.phy_pwdet_onetime_flag
1 DATA: phy_dis_pwdet_one   \ .bss.phy_dis_pwdet_one
1 DATA: phy_tx_pwr_correct_old   \ .bss.phy_tx_pwr_correct_old
2 DATA: phy_tx_pwr_error_old   \ .bss.phy_tx_pwr_error_old
2 DATA: phy_tx_pwr_error   \ .bss.phy_tx_pwr_error
1 DATA: phy_in_vdd33_offset   \ .bss.phy_in_vdd33_offset
1 DATA: tx_pwctrl_track_num   \ .bss.tx_pwctrl_track_num
2 DATA: phy_tx_power_out   \ .bss.phy_tx_power_out
1 DATA: rfcal_bb_atten_init   \ .data.rfcal_bb_atten_init
  $3C  1 rfcal_bb_atten_init INIT
