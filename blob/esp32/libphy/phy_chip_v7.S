.type   chip7_sleep_params, @object
        .comm   chip7_sleep_params, 196, 4
        .type   chip7_phy_init_ctrl, @object
        .comm   chip7_phy_init_ctrl, 94, 4
        .type   set_most_tpw, @object
        .comm   set_most_tpw, 1, 1
        .type   phy_rxbb_dc, @object
        .comm   phy_rxbb_dc, 16, 4
        .type   phy_rxrf_dc, @object
        .comm   phy_rxrf_dc, 136, 4
        .type   phy_version_str, @object
        .comm   phy_version_str, 40, 4
        .type   pwrdet_offset, @object
        .comm   pwrdet_offset, 2, 2
        .type   adc_ana_conf_org, @object
        .comm   adc_ana_conf_org, 4, 4
        .type   phy_dac_org, @object
        .comm   phy_dac_org, 1, 1
        .type   phy_dcur_org, @object
        .comm   phy_dcur_org, 1, 1
        .type   chip7_phy_api_ctrl, @object
        .comm   chip7_phy_api_ctrl, 15, 4

        .section .iram1,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_135_0, 0x3ff5c030
        .literal .Lsec_135_4, esp_dport_access_reg_read
.Lsec_135_8:
        .global ram_disable_agc
        .type   ram_disable_agc, @function
ram_disable_agc:
        entry    a1, 32
        l32r     a2, .Lsec_135_0
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_135_4
        callx8   a8
        movi     a8, 0x100
        or       a10, a10, a8
        memw
        s32i.n   a10, a2, 0
        retw.n
        .size   ram_disable_agc, .-ram_disable_agc
.Lsec_135_23:
        .byte    0x00
        .literal_position
        .literal .Lsec_135_24, 0x3ff5c030
        .literal .Lsec_135_28, esp_dport_access_reg_read
.Lsec_135_2c:
        .global ram_enable_agc
        .type   ram_enable_agc, @function
ram_enable_agc:
        entry    a1, 32
        l32r     a2, .Lsec_135_24
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_135_28
        callx8   a8
        movi     a8, 0xfffffeff
        and      a10, a10, a8
        memw
        s32i.n   a10, a2, 0
        retw.n
        .size   ram_enable_agc, .-ram_enable_agc
.Lsec_135_47:
        .byte    0x00
        .literal_position
        .literal .Lsec_135_48, 0x3ff45038
        .literal .Lsec_135_4c, 0x3ff45034
        .literal .Lsec_135_50, 0xfffc0000
        .literal .Lsec_135_54, 0x000100f0
        .literal .Lsec_135_58, 0x0001000f
        .literal .Lsec_135_5c, 0xfffcffff
        .literal .Lsec_135_60, esp_dport_access_reg_read
        .literal .Lsec_135_64, esp_dport_access_reg_read
        .literal .Lsec_135_68, esp_dport_access_reg_read
.Lsec_135_6c:
        .global ram_write_gain_mem
        .type   ram_write_gain_mem, @function
ram_write_gain_mem:
        entry    a1, 32
        l32r     a5, .Lsec_135_4c
        extui    a4, a4, 0, 8
        mov.n    a10, a5
        slli     a4, a4, 8
        l32r     a6, .Lsec_135_48
        memw
        s32i.n   a3, a6, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_135_60
        callx8   a8
        l32r     a9, .Lsec_135_54
        add.n    a9, a4, a9
        l32r     a3, .Lsec_135_50
        and      a8, a10, a3
        or       a9, a8, a9
        mov.n    a10, a5
        memw
        s32i.n   a9, a5, 0
        memw
        s32i.n   a2, a6, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_135_64
        callx8   a8
        and      a3, a10, a3
        mov.n    a10, a5
        l32r     a8, .Lsec_135_58
        add.n    a4, a4, a8
        or       a8, a3, a4
        memw
        s32i.n   a8, a5, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_135_68
        callx8   a8
        l32r     a3, .Lsec_135_5c
        and      a10, a10, a3
        memw
        s32i.n   a10, a5, 0
        retw.n
        .size   ram_write_gain_mem, .-ram_write_gain_mem
.Lsec_135_cd:
        .byte    0x00
        .byte    0x00
        .byte    0x00
        .literal_position
        .literal .Lsec_135_d0, 0x00030000
        .literal .Lsec_135_d4, 0x3ff460a0
        .literal .Lsec_135_d8, 0xfffcffff
        .literal .Lsec_135_dc, esp_dport_access_reg_read
.Lsec_135_e0:
        .global ram_set_txclk_en
        .type   ram_set_txclk_en, @function
ram_set_txclk_en:
        entry    a1, 32
        l32r     a3, .Lsec_135_d4
        extui    a2, a2, 0, 8
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_135_dc
        callx8   a8
        movi.n   a9, 0
        l32r     a8, .Lsec_135_d8
        and      a8, a10, a8
        l32r     a10, .Lsec_135_d0
        moveqz   a10, a9, a2
        or       a10, a10, a8
        memw
        s32i.n   a10, a3, 0
        retw.n
        .size   ram_set_txclk_en, .-ram_set_txclk_en
.Lsec_135_109:
        .byte    0x00
        .byte    0x00
        .byte    0x00
        .literal_position
        .literal .Lsec_135_10c, 0x0000c000
        .literal .Lsec_135_110, 0x3ff460a0
        .literal .Lsec_135_114, 0xffff3fff
        .literal .Lsec_135_118, esp_dport_access_reg_read
.Lsec_135_11c:
        .global ram_set_rxclk_en
        .type   ram_set_rxclk_en, @function
ram_set_rxclk_en:
        entry    a1, 32
        l32r     a3, .Lsec_135_110
        extui    a2, a2, 0, 8
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_135_118
        callx8   a8
        movi.n   a9, 0
        l32r     a8, .Lsec_135_114
        and      a8, a10, a8
        l32r     a10, .Lsec_135_10c
        moveqz   a10, a9, a2
        or       a10, a10, a8
        memw
        s32i.n   a10, a3, 0
        retw.n
        .size   ram_set_rxclk_en, .-ram_set_rxclk_en
.Lsec_135_145:
        .byte    0x00
        .byte    0x00
        .byte    0x00
        .literal_position
        .literal .Lsec_135_148, 0x3ff5c01c
        .literal .Lsec_135_14c, 0xff00ffff
        .literal .Lsec_135_150, 0x007f0000
        .literal .Lsec_135_154, 0x3ff5c038
        .literal .Lsec_135_158, 0x04000000
        .literal .Lsec_135_15c, 0x3ff5c030
        .literal .Lsec_135_160, 0x3ff5c080
        .literal .Lsec_135_164, esp_dport_access_reg_read
        .literal .Lsec_135_168, esp_dport_access_reg_read
        .literal .Lsec_135_16c, esp_dport_access_reg_read
        .literal .Lsec_135_170, esp_dport_access_reg_read
.Lsec_135_174:
        .global disable_wifi_agc
        .type   disable_wifi_agc, @function
disable_wifi_agc:
        entry    a1, 32
        l32r     a3, .Lsec_135_148
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_135_164
        callx8   a8
        l32r     a8, .Lsec_135_14c
        and      a10, a10, a8
        l32r     a8, .Lsec_135_150
        or       a8, a10, a8
        l32r     a2, .Lsec_135_154
        or       a10, a2, a2
        memw
        s32i     a8, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_135_168
        callx8   a8
        l32r     a8, .Lsec_135_158
        or       a8, a10, a8
        l32r     a3, .Lsec_135_15c
        or       a10, a3, a3
        memw
        s32i     a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_135_16c
        callx8   a8
        movi     a8, -49
        and      a8, a10, a8
        l32r     a2, .Lsec_135_160
        mov.n    a10, a2
        memw
        s32i.n   a8, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_135_170
        callx8   a8
        movi.n   a8, 1
        or       a10, a10, a8
        memw
        s32i.n   a10, a2, 0
        retw.n
        .size   disable_wifi_agc, .-disable_wifi_agc
.Lsec_135_1da:
        .byte    0x00
        .byte    0x00
        .literal_position
        .literal .Lsec_135_1dc, 0x3ff5c080
        .literal .Lsec_135_1e0, 0x3ff5c030
        .literal .Lsec_135_1e4, 0x3ff5c01c
        .literal .Lsec_135_1e8, 0xff00ffff
        .literal .Lsec_135_1ec, 0x000c0000
        .literal .Lsec_135_1f0, 0x3ff5c038
        .literal .Lsec_135_1f4, 0x04000000
        .literal .Lsec_135_1f8, esp_dport_access_reg_read
        .literal .Lsec_135_1fc, esp_dport_access_reg_read
        .literal .Lsec_135_200, esp_dport_access_reg_read
        .literal .Lsec_135_204, esp_dport_access_reg_read
.Lsec_135_208:
        .global enable_wifi_agc
        .type   enable_wifi_agc, @function
enable_wifi_agc:
        entry    a1, 32
        l32r     a4, .Lsec_135_1dc
        movi.n   a2, -2
        or       a10, a4, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_135_1f8
        callx8   a8
        and      a2, a10, a2
        l32r     a3, .Lsec_135_1e0
        mov.n    a10, a3
        memw
        s32i     a2, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_135_1fc
        callx8   a8
        movi     a8, -49
        and      a10, a10, a8
        movi.n   a8, 16
        l32r     a2, .Lsec_135_1e4
        or       a8, a10, a8
        mov.n    a10, a2
        memw
        s32i.n   a8, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_135_200
        callx8   a8
        l32r     a8, .Lsec_135_1e8
        and      a10, a10, a8
        l32r     a8, .Lsec_135_1ec
        or       a8, a10, a8
        l32r     a3, .Lsec_135_1f0
        mov.n    a10, a3
        memw
        s32i.n   a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_135_204
        callx8   a8
        l32r     a8, .Lsec_135_1f4
        or       a10, a10, a8
        memw
        s32i.n   a10, a3, 0
        retw.n
        .size   enable_wifi_agc, .-enable_wifi_agc
.Lsec_135_271:
        .byte    0x00
        .byte    0x00
        .byte    0x00
        .literal_position
        .literal .Lsec_135_274, bt_dig_gain_force
        .literal .Lsec_135_278, 0x3ff4600c
        .literal .Lsec_135_27c, 0x3ff46010
.Lsec_135_280:
        .global wr_bt_tx_atten
        .type   wr_bt_tx_atten, @function
wr_bt_tx_atten:
        entry    a1, 48
        or       a10, a1, a1
        movi     a9, 8
        loop     a9, .Lsec_135_29c
        l8ui     a8, a2, 0
        addi.n   a2, a2, 1
        neg      a8, a8
        s8i      a8, a10, 0
        memw
        addi.n   a10, a10, 1
.Lsec_135_29c:
        nop.n
        nop.n
        nop.n
        l32r     a2, .Lsec_135_274
        l8ui     a2, a2, 0
        bnez.n   a2, .Lsec_135_2c6
        l32i.n   a8, a1, 0
        l32r     a2, .Lsec_135_278
        memw
        s32i.n   a8, a2, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a8, a1, 4
        l32r     a2, .Lsec_135_27c
        memw
        s32i.n   a8, a2, 0
.Lsec_135_2c6:
        retw.n
        .size   wr_bt_tx_atten, .-wr_bt_tx_atten
        .literal_position
        .literal .Lsec_135_2c8, chip7_sleep_params
        .literal .Lsec_135_2cc, 0x3ff46000
        .literal .Lsec_135_2d0, 0x03fc0000
        .literal .Lsec_135_2d4, chip7_sleep_params+0x8
        .literal .Lsec_135_2d8, g_phyFuns
        .literal .Lsec_135_2dc, esp_dport_access_reg_read
.Lsec_135_2e0:
        .global wr_bt_tx_gain_mem
        .type   wr_bt_tx_gain_mem, @function
wr_bt_tx_gain_mem:
        entry    a1, 64
        l32r     a10, .Lsec_135_2cc
        extui    a2, a2, 0, 8
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_135_2dc
        callx8   a8
        extui    a3, a3, 0, 8
        movi     a4, 255
        l32r     a8, .Lsec_135_2d0
        and      a10, a10, a8
        extui    a8, a10, 18, 8
        s32i.n   a2, a1, 16
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a9, .Lsec_135_2c8
        beq      a3, a4, .Lsec_135_31c
        extui    a12, a3, 0, 6
        srli     a11, a3, 6
        l16ui    a5, a9, 144
        j        .Lsec_135_325
        .byte    0x00
        .byte    0x00
.Lsec_135_31c:
        l16ui    a5, a9, 144
        extui    a12, a5, 0, 6
        extui    a11, a5, 6, 8
.Lsec_135_325:
        mov.n    a4, a5
        beqz.n   a5, .Lsec_135_33d
        addi     a3, a5, -48
        movi.n   a4, 8
        movi.n   a5, 4
        moveqz   a5, a4, a3
        movi.n   a6, 2
        movi.n   a4, 1
        moveqz   a4, a6, a3
        j        .Lsec_135_33d
.Lsec_135_33d:
        addx2    a4, a4, a9
        addi     a5, a5, 76
        l16ui    a4, a4, 146
        addx2    a5, a5, a9
        mov.n    a7, a1
        movi.n   a6, 4
        loop     a6, .Lsec_135_35d
        l16ui    a10, a5, 0
        addi.n   a5, a5, 2
        s16i     a10, a7, 0
        memw
        addi.n   a7, a7, 2
.Lsec_135_35d:
        slli     a14, a4, 21
        addi.n   a8, a8, 8
        nop.n
        l32i.n   a3, a1, 16
        slli     a12, a12, 3
        extui    a3, a3, 0, 16
        extui    a2, a8, 0, 8
        mov.n    a7, a9
        l16ui    a4, a1, 4
        slli     a13, a4, 26
        srli     a4, a4, 6
        l16ui    a5, a1, 0
        slli     a5, a5, 12
        s32i.n   a3, a1, 20
        nop.n
        nop
        nop.n
        nop
        l16ui    a6, a1, 2
        slli     a6, a6, 3
        or       a5, a5, a6
        or       a4, a5, a4
        l16ui    a3, a1, 6
        or       a4, a4, a14
        slli     a3, a3, 17
        or       a3, a3, a13
        or       a11, a3, a11
        l32r     a6, .Lsec_135_2d4
        or       a3, a11, a12
        l32r     a5, .Lsec_135_2d8
.Lsec_135_3b0:
        movi     a9, 255
        l32i.n   a8, a1, 16
        l32i.n   a11, a1, 20
        bne      a8, a9, .Lsec_135_3bd
        l8ui     a11, a7, 120
.Lsec_135_3bd:
        slli     a11, a11, 10
        mov.n    a12, a2
        or       a11, a11, a3
        mov.n    a10, a4
        l32i.n   a13, a5, 0
        addi.n   a7, a7, 1
        l32i.n   a13, a13, 52
        callx8   a13
        addi.n   a8, a2, 1
        extui    a2, a8, 0, 8
        bne      a6, a7, .Lsec_135_3b0
        retw.n
        .size   wr_bt_tx_gain_mem, .-wr_bt_tx_gain_mem
.Lsec_135_3da:
        .byte    0x00
        .byte    0x00
        .literal_position
        .literal .Lsec_135_3dc, 0x3ff46000
        .literal .Lsec_135_3e0, 0xfc03ffff
        .literal .Lsec_135_3e4, 0x01b80000
        .literal .Lsec_135_3e8, g_phyFuns
        .literal .Lsec_135_3ec, chip7_sleep_params
        .literal .Lsec_135_3f0, esp_dport_access_reg_read
.Lsec_135_3f4:
        .global set_tx_gain_table
        .type   set_tx_gain_table, @function
set_tx_gain_table:
        entry    a1, 48
        l32r     a5, .Lsec_135_3dc
        extui    a7, a2, 0, 16
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_135_3f0
        callx8   a8
        extui    a3, a3, 0, 16
        l32r     a2, .Lsec_135_3e0
        mov.n    a6, a1
        and      a4, a10, a2
        mov.n    a10, a3
        l32r     a2, .Lsec_135_3e4
        or       a2, a4, a2
        l32r     a4, .Lsec_135_3e8
        memw
        s32i.n   a2, a5, 0
        movi.n   a5, 4
        nop.n
        nop.n
        nop.n
        l32i.n   a2, a4, 0
        l32i     a2, a2, 0x10c
        callx8   a2
        extui    a8, a10, 0, 16
        slli     a8, a8, 3
        l32r     a2, .Lsec_135_3ec
        addi.n   a8, a8, 12
        add.n    a8, a2, a8
        loop     a5, .Lsec_135_44d
        l16ui    a9, a8, 0
        addi.n   a8, a8, 2
        s16i     a9, a6, 0
        memw
        addi.n   a6, a6, 2
.Lsec_135_44d:
        extui    a11, a3, 6, 8
        extui    a3, a3, 0, 6
        nop.n
        l16ui    a5, a1, 4
        slli     a6, a3, 3
        slli     a12, a5, 26
        slli     a7, a7, 10
        srli     a8, a5, 6
        l16ui    a3, a1, 6
        mov.n    a9, a2
        slli     a3, a3, 17
        or       a3, a3, a12
        or       a7, a3, a7
        or       a3, a7, a11
        l16ui    a5, a1, 0
        or       a3, a3, a6
        slli     a5, a5, 12
        movi.n   a2, 1
        l16ui    a6, a1, 2
        slli     a6, a6, 3
        or       a5, a5, a6
        or       a5, a5, a8
        bgeui    a10, 4, .Lsec_135_491
        movi.n   a2, 0
.Lsec_135_491:
        addi     a2, a2, 24
        addx2    a2, a2, a9
        l16ui    a6, a2, 4
        movi     a2, 110
        slli     a6, a6, 21
        or       a5, a5, a6
        movi     a6, 118
.Lsec_135_4a6:
        l32i.n   a8, a4, 0
        mov.n    a12, a2
        mov.n    a11, a3
        addi.n   a2, a2, 1
        mov.n    a10, a5
        l32i.n   a8, a8, 52
        callx8   a8
        bne      a2, a6, .Lsec_135_4a6
        retw.n
        .size   set_tx_gain_table, .-set_tx_gain_table
.Lsec_135_4ba:
        .byte    0x00
        .byte    0x00
        .literal_position
        .literal .Lsec_135_4bc, phy_in_vdd33_offset
        .literal .Lsec_135_4c0, phy_in_most_power
        .literal .Lsec_135_4c4, phy_set_most_tpw_disbg
        .literal .Lsec_135_4c8, set_most_tpw
        .literal .Lsec_135_4cc, tx_rf_ana_gain
        .literal .Lsec_135_4d0, chip7_sleep_params
        .literal .Lsec_135_4d4, get_phy_target_power
.Lsec_135_4d8:
        .global set_most_pwr_reg
        .type   set_most_pwr_reg, @function
set_most_pwr_reg:
        entry    a1, 48
        l32r     a2, .Lsec_135_4bc
        mov.n    a10, a1
        l8ui     a11, a2, 0
        sext     a11, a11, 7
        .reloc  ., R_XTENSA_ASM_EXPAND, get_phy_target_power
        l32r     a8, .Lsec_135_4d4
        callx8   a8
        l32r     a2, .Lsec_135_4c0
        l8ui     a8, a2, 0
        sext     a2, a8, 7
        l8ui     a9, a1, 5
        blt      a2, a9, .Lsec_135_4fe
        j        .Lsec_135_5bd
.Lsec_135_4fe:
        sub      a8, a9, a8
        movi.n   a10, 1
        l32r     a2, .Lsec_135_4c4
        extui    a8, a8, 0, 8
        s8i      a10, a2, 0
        memw
        movi.n   a2, 8
        bltu     a2, a8, .Lsec_135_528
        addi     a9, a9, -8
        movi.n   a11, 14
        nop.n
        l32r     a2, .Lsec_135_4c8
        s8i      a9, a2, 0
        memw
        j        .Lsec_135_668
        .byte    0x00
.Lsec_135_528:
        movi.n   a2, 18
        bltu     a2, a8, .Lsec_135_53e
        l32r     a2, .Lsec_135_4c8
        addi     a9, a9, -18
        movi.n   a11, 10
        s8i      a9, a2, 0
        memw
        j        .Lsec_135_668
.Lsec_135_53e:
        movi.n   a2, 24
        bltu     a2, a8, .Lsec_135_554
        l32r     a2, .Lsec_135_4c8
        addi     a9, a9, -24
        movi.n   a11, 7
        s8i      a9, a2, 0
        memw
        j        .Lsec_135_668
.Lsec_135_554:
        movi.n   a2, 32
        bltu     a2, a8, .Lsec_135_56a
        l32r     a2, .Lsec_135_4c8
        addi     a9, a9, -32
        movi.n   a11, 6
        s8i      a9, a2, 0
        memw
        j        .Lsec_135_668
.Lsec_135_56a:
        movi.n   a2, 44
        bltu     a2, a8, .Lsec_135_580
        l32r     a2, .Lsec_135_4c8
        addi     a9, a9, -44
        movi.n   a11, 5
        s8i      a9, a2, 0
        memw
        j        .Lsec_135_668
.Lsec_135_580:
        movi.n   a2, 56
        bltu     a2, a8, .Lsec_135_596
        l32r     a2, .Lsec_135_4c8
        addi     a9, a9, -56
        movi.n   a11, 2
        s8i      a9, a2, 0
        memw
        j        .Lsec_135_668
.Lsec_135_596:
        movi.n   a2, 68
        bltu     a2, a8, .Lsec_135_5ac
        l32r     a2, .Lsec_135_4c8
        addi     a9, a9, -68
        movi.n   a11, 1
        s8i      a9, a2, 0
        memw
        j        .Lsec_135_668
.Lsec_135_5ac:
        l32r     a2, .Lsec_135_4c8
        addi     a9, a9, -92
        movi.n   a11, 0
        s8i      a9, a2, 0
        memw
        j        .Lsec_135_668
.Lsec_135_5bd:
        l32r     a10, .Lsec_135_4c4
        movi.n   a11, 0
        l8ui     a8, a1, 4
        s8i      a11, a10, 0
        memw
        bge      a2, a8, .Lsec_135_5e4
        movi.n   a2, 5
        nop.n
        nop.n
        nop.n
        l32r     a8, .Lsec_135_4c8
        s8i      a9, a8, 0
        memw
        j        .Lsec_135_651
        .byte    0x00
        .byte    0x00
.Lsec_135_5e4:
        l8ui     a9, a1, 3
        bge      a2, a9, .Lsec_135_63c
        movi.n   a2, 4
        l32r     a9, .Lsec_135_4c8
        s8i      a8, a9, 0
        memw
        j        .Lsec_135_651
.Lsec_135_5f8:
        l32r     a8, .Lsec_135_4c8
        movi.n   a2, 3
        s8i      a9, a8, 0
        memw
        j        .Lsec_135_651
        .byte    0x00
        .byte    0x00
.Lsec_135_608:
        l32r     a9, .Lsec_135_4c8
        movi.n   a2, 2
        s8i      a8, a9, 0
        memw
        j        .Lsec_135_651
        .byte    0x00
        .byte    0x00
.Lsec_135_618:
        l32r     a8, .Lsec_135_4c8
        movi.n   a2, 1
        s8i      a9, a8, 0
        memw
        j        .Lsec_135_651
.Lsec_135_626:
        l32r     a9, .Lsec_135_4c8
        movi.n   a2, 0
        s8i      a8, a9, 0
        memw
        j        .Lsec_135_651
.Lsec_135_634:
        l16ui    a11, a8, 2
        call8    set_tx_gain_table
        retw.n
.Lsec_135_63c:
        l8ui     a8, a1, 2
        blt      a2, a8, .Lsec_135_5f8
        l8ui     a9, a1, 1
        blt      a2, a9, .Lsec_135_608
        l8ui     a8, a1, 0
        blt      a2, a8, .Lsec_135_618
        j        .Lsec_135_626
.Lsec_135_651:
        l32r     a8, .Lsec_135_4d0
        l8ui     a10, a8, 188
        movi.n   a8, 15
        or       a10, a10, a8
        l32r     a8, .Lsec_135_4cc
        s16i     a10, a8, 0
        memw
        j        .Lsec_135_634
.Lsec_135_668:
        l32r     a8, .Lsec_135_4d0
        movi.n   a2, 5
        l16ui    a10, a8, 188
        movi     a8, 240
        and      a10, a10, a8
        or       a10, a11, a10
        extui    a10, a10, 0, 16
        l32r     a8, .Lsec_135_4cc
        s16i     a10, a8, 0
        memw
        j        .Lsec_135_634
        .size   set_most_pwr_reg, .-set_most_pwr_reg
        .literal_position
        .literal .Lsec_135_688, 0x000c0000
        .literal .Lsec_135_68c, 0x3ff4880c
        .literal .Lsec_135_690, 0xfff3ffff
        .literal .Lsec_135_694, esp_dport_access_reg_read
.Lsec_135_698:
        .weak   set_xpd_sar
        .type   set_xpd_sar, @function
set_xpd_sar:
        entry    a1, 32
        l32r     a3, .Lsec_135_68c
        extui    a2, a2, 0, 8
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_135_694
        callx8   a8
        movi.n   a9, 0
        l32r     a8, .Lsec_135_690
        and      a8, a10, a8
        l32r     a10, .Lsec_135_688
        moveqz   a10, a9, a2
        or       a10, a10, a8
        memw
        s32i.n   a10, a3, 0
        retw.n
        .size   set_xpd_sar, .-set_xpd_sar
.Lsec_135_6c1:
        .byte    0x00
        .byte    0x00
        .byte    0x00
        .literal_position
        .literal .Lsec_135_6c4, 0x3ff5d040
        .literal .Lsec_135_6c8, 0x7fffffff
        .literal .Lsec_135_6cc, esp_dport_access_reg_read
.Lsec_135_6d0:
        .global bb_wdt_rst_enable
        .type   bb_wdt_rst_enable, @function
bb_wdt_rst_enable:
        entry    a1, 32
        l32r     a3, .Lsec_135_6c4
        extui    a2, a2, 0, 8
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_135_6cc
        callx8   a8
        slli     a2, a2, 31
        l32r     a8, .Lsec_135_6c8
        and      a10, a10, a8
        or       a2, a10, a2
        memw
        s32i.n   a2, a3, 0
        retw.n
        .size   bb_wdt_rst_enable, .-bb_wdt_rst_enable
        .literal_position
        .literal .Lsec_135_6f4, 0x3ff5d040
        .literal .Lsec_135_6f8, 0x40000000
        .literal .Lsec_135_6fc, 0xbfffffff
        .literal .Lsec_135_700, esp_dport_access_reg_read
.Lsec_135_704:
        .global bb_wdt_int_enable
        .type   bb_wdt_int_enable, @function
bb_wdt_int_enable:
        entry    a1, 32
        l32r     a3, .Lsec_135_6f4
        extui    a2, a2, 0, 8
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_135_700
        callx8   a8
        slli     a2, a2, 30
        l32r     a8, .Lsec_135_6f8
        and      a2, a2, a8
        l32r     a8, .Lsec_135_6fc
        and      a10, a10, a8
        or       a10, a2, a10
        memw
        s32i.n   a10, a3, 0
        retw.n
        .size   bb_wdt_int_enable, .-bb_wdt_int_enable
.Lsec_135_72e:
        .byte    0x00
        .byte    0x00
        .literal_position
        .literal .Lsec_135_730, 0x3ff5d040
        .literal .Lsec_135_734, 0x20000000
        .literal .Lsec_135_738, esp_dport_access_reg_read
.Lsec_135_73c:
        .global bb_wdt_timeout_clear
        .type   bb_wdt_timeout_clear, @function
bb_wdt_timeout_clear:
        entry    a1, 32
        l32r     a2, .Lsec_135_730
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_135_738
        callx8   a8
        l32r     a8, .Lsec_135_734
        or       a10, a10, a8
        memw
        s32i.n   a10, a2, 0
        retw.n
        .size   bb_wdt_timeout_clear, .-bb_wdt_timeout_clear
.Lsec_135_757:
        .byte    0x00
        .literal_position
        .literal .Lsec_135_758, 0x3ff5d008
        .literal .Lsec_135_75c, esp_dport_access_reg_read
.Lsec_135_760:
        .global bb_wdt_get_status
        .type   bb_wdt_get_status, @function
bb_wdt_get_status:
        entry    a1, 32
        l32r     a10, .Lsec_135_758
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_135_75c
        callx8   a8
        mov.n    a2, a10
        retw.n
        .size   bb_wdt_get_status, .-bb_wdt_get_status
        .literal_position
        .literal .Lsec_135_770, chip7_sleep_params
        .literal .Lsec_135_774, btpwr_atten_offset
        .literal .Lsec_135_778, bt_tx_pa_gain
        .literal .Lsec_135_77c, correct_rf_ana_gain_new
.Lsec_135_780:
        .global bt_tx_gain_cal
        .type   bt_tx_gain_cal, @function
bt_tx_gain_cal:
        entry    a1, 48
        l32r     a2, .Lsec_135_774
        movi     a7, -76
        movi     a6, 160
        l32r     a5, .Lsec_135_770
        mov.n    a3, a5
        l8ui     a2, a2, 0
        sext     a2, a2, 7
        l8ui     a4, a5, 138
        add.n    a2, a4, a2
        sub      a7, a7, a2
        extui    a7, a7, 0, 16
        l32r     a4, .Lsec_135_778
        extui    a2, a2, 0, 8
.Lsec_135_7a8:
        mov.n    a11, a1
        addi.n   a10, a1, 4
        movi.n   a13, 1
        l16ui    a8, a5, 144
        addi.n   a12, a1, 2
        l16ui    a15, a4, 0
        s16i     a8, a1, 2
        s16i     a7, a1, 4
        s16i     a15, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, correct_rf_ana_gain_new
        l32r     a8, .Lsec_135_77c
        callx8   a8
        l8ui     a8, a1, 4
        add.n    a8, a2, a8
        sub      a8, a8, a6
        extui    a8, a8, 0, 8
        l16ui    a11, a1, 0
        sext     a10, a8, 7
        s8i      a11, a3, 120
        memw
        bgez     a10, .Lsec_135_7ed
        movi.n   a8, 0
        s8i      a8, a3, 129
        memw
        j        .Lsec_135_7fa
.Lsec_135_7ed:
        movi.n   a9, 64
        bge      a9, a10, .Lsec_135_7f4
        movi.n   a8, 64
.Lsec_135_7f4:
        s8i      a8, a3, 129
        memw
.Lsec_135_7fa:
        addi.n   a6, a6, 12
        addi.n   a7, a7, 12
        extui    a6, a6, 0, 8
        extui    a7, a7, 0, 16
        addi.n   a3, a3, 1
        bnez     a6, .Lsec_135_7a8
        retw.n
        .size   bt_tx_gain_cal, .-bt_tx_gain_cal
.Lsec_135_80b:
        .byte    0x00
        .literal_position
        .literal .Lsec_135_80c, chip7_sleep_params+0x81
        .literal .Lsec_135_810, force_txrx_off
        .literal .Lsec_135_814, force_txrx_off
.Lsec_135_818:
        .type   bt_tx_gain_cal_set$part$4, @function
bt_tx_gain_cal_set$part$4:
        entry    a1, 32
        call8    bt_tx_gain_cal
        movi.n   a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, force_txrx_off
        l32r     a8, .Lsec_135_810
        callx8   a8
        movi     a11, 255
        mov.n    a10, a11
        call8    wr_bt_tx_gain_mem
        l32r     a10, .Lsec_135_80c
        call8    wr_bt_tx_atten
        movi.n   a10, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, force_txrx_off
        l32r     a8, .Lsec_135_814
        callx8   a8
        retw.n
        .size   bt_tx_gain_cal_set$part$4, .-bt_tx_gain_cal_set$part$4
.Lsec_135_83e:
        .byte    0x00
        .byte    0x00
        .literal_position
        .literal .Lsec_135_840, bt_dig_gain_force
.Lsec_135_844:
        .global bt_tx_gain_cal_set
        .type   bt_tx_gain_cal_set, @function
bt_tx_gain_cal_set:
        entry    a1, 32
        l32r     a8, .Lsec_135_840
        l8ui     a8, a8, 0
        bnez.n   a8, .Lsec_135_852
        call8    .Lsec_135_818
.Lsec_135_852:
        retw.n
        .size   bt_tx_gain_cal_set, .-bt_tx_gain_cal_set
        .literal_position
        .literal .Lsec_135_854, 0x3ff5c080
        .literal .Lsec_135_858, esp_dport_access_reg_read
        .literal .Lsec_135_85c, esp_dport_access_reg_read
.Lsec_135_860:
        .global phy_wifi_enable_set
        .type   phy_wifi_enable_set, @function
phy_wifi_enable_set:
        entry    a1, 32
        extui    a2, a2, 0, 8
        beqz     a2, .Lsec_135_885
        l32r     a3, .Lsec_135_854
        movi     a2, -193
        or       a10, a3, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_135_858
        callx8   a8
        and      a10, a10, a2
        memw
        s32i.n   a10, a3, 0
        retw.n
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_135_885:
        l32r     a2, .Lsec_135_854
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_135_85c
        callx8   a8
        movi     a8, 192
        or       a10, a10, a8
        memw
        s32i.n   a10, a2, 0
        retw.n
        .size   phy_wifi_enable_set, .-phy_wifi_enable_set
.Lsec_135_89d:
        .byte    0x00
        .byte    0x00
        .byte    0x00
        .literal_position
        .literal .Lsec_135_8a0, 0x00005400
        .literal .Lsec_135_8a4, 0x3ff5c018
        .literal .Lsec_135_8a8, 0xffff801f
        .literal .Lsec_135_8ac, corr_cca_thr
        .literal .Lsec_135_8b0, esp_dport_access_reg_read
        .literal .Lsec_135_8b4, esp_dport_access_reg_read
        .literal .Lsec_135_8b8, set_rx_sense
.Lsec_135_8bc:
        .global ram_set_noise_floor
        .type   ram_set_noise_floor, @function
ram_set_noise_floor:
        entry    a1, 32
        sext     a2, a2, 15
        movi     a8, 0xfffffea1
        l32r     a3, .Lsec_135_8a0
        bge      a2, a8, .Lsec_135_8d7
        movi     a3, 0xfffffe78
        max      a3, a2, a3
        extui    a3, a3, 0, 10
        slli     a3, a3, 5
.Lsec_135_8d7:
        l32r     a2, .Lsec_135_8a4
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_135_8b0
        callx8   a8
        l32r     a8, .Lsec_135_8a8
        and      a10, a10, a8
        or       a3, a10, a3
        mov.n    a10, a2
        memw
        s32i     a3, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_135_8b4
        callx8   a8
        movi.n   a3, 16
        or       a10, a10, a3
        memw
        s32i.n   a10, a2, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a2, .Lsec_135_8ac
        l8ui     a10, a2, 0
        beqz.n   a10, .Lsec_135_91c
        sext     a10, a10, 7
        .reloc  ., R_XTENSA_ASM_EXPAND, set_rx_sense
        l32r     a8, .Lsec_135_8b8
        callx8   a8
.Lsec_135_91c:
        retw.n
        .size   ram_set_noise_floor, .-ram_set_noise_floor
.Lsec_135_91e:
        .byte    0x00
        .byte    0x00
        .literal_position
        .literal .Lsec_135_920, 0x3ff00024
        .literal .Lsec_135_924, g_phyFuns
        .literal .Lsec_135_928, 0x3ff48030
        .literal .Lsec_135_92c, 0x27ffffff
        .literal .Lsec_135_930, phy_enter_critical
        .literal .Lsec_135_934, noise_check_loop
        .literal .Lsec_135_938, esp_dport_access_reg_read
        .literal .Lsec_135_93c, esp_dport_access_reg_read
        .literal .Lsec_135_940, set_xpd_sar
        .literal .Lsec_135_944, phy_exit_critical
.Lsec_135_948:
        .global phy_close_rf
        .type   phy_close_rf, @function
phy_close_rf:
        entry    a1, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_enter_critical
        l32r     a8, .Lsec_135_930
        callx8   a8
        movi.n   a11, 1
        mov.n    a4, a10
        mov.n    a10, a11
        .reloc  ., R_XTENSA_ASM_EXPAND, noise_check_loop
        l32r     a8, .Lsec_135_934
        callx8   a8
        call8    disable_wifi_agc
        l32r     a3, .Lsec_135_920
        or       a10, a3, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_135_938
        callx8   a8
        movi.n   a8, -3
        l32r     a2, .Lsec_135_924
        and      a10, a10, a8
        memw
        s32i.n   a10, a3, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32i     a3, a2, 0
        l32i     a3, a3, 192
        callx8   a3
        movi.n   a11, 1
        mov.n    a10, a11
        l32i.n   a3, a2, 0
        movi     a12, 0
        l32i     a3, a3, 184
        callx8   a3
        l32i.n   a2, a2, 0
        l32i     a2, a2, 196
        callx8   a2
        l32r     a2, .Lsec_135_928
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_135_93c
        callx8   a8
        l32r     a8, .Lsec_135_92c
        and      a8, a10, a8
        movi.n   a10, 0
        memw
        s32i.n   a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, set_xpd_sar
        l32r     a8, .Lsec_135_940
        callx8   a8
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_exit_critical
        l32r     a8, .Lsec_135_944
        callx8   a8
        retw.n
        .size   phy_close_rf, .-phy_close_rf
.Lsec_135_9ca:

        .section .phyiram.0,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_136_0, 0x3ff00024
        .literal .Lsec_136_4, esp_dport_access_reg_read
.Lsec_136_8:
        .global ram_bb_bss_cbw40_dig
        .type   ram_bb_bss_cbw40_dig, @function
ram_bb_bss_cbw40_dig:
        entry    a1, 32
        l32r     a3, .Lsec_136_0
        extui    a2, a2, 0, 1
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_136_4
        callx8   a8
        movi.n   a8, -13
        slli     a2, a2, 2
        and      a10, a10, a8
        or       a10, a2, a10
        memw
        s32i.n   a10, a3, 0
        retw.n
        .size   ram_bb_bss_cbw40_dig, .-ram_bb_bss_cbw40_dig
.Lsec_136_2b:

        .section .rodata,"a",@progbits
        .align  4
.Lsec_137_0:
        .byte   0xff, 0x09, 0x07, 0x00, 0xff, 0x13, 0x17, 0x00, 0x00, 0x00, 0xf5, 0x00, 0x00, 0x00, 0xf6, 0x00
.Lsec_137_10:
        .byte   0xff, 0x01, 0x04, 0x00, 0xff, 0x01, 0x14, 0x00, 0xff, 0x01, 0x18, 0x00
.Lsec_137_1c:
        .byte   0xff, 0x03, 0x04, 0x00, 0xff, 0xf9, 0x14, 0x00, 0x00, 0x00, 0xf0, 0x00, 0x00, 0x00, 0xf1, 0x00
        .byte   0x00, 0x00, 0xf2, 0x00
.Lsec_137_30:
        .byte   0xff, 0x09, 0x07, 0x00, 0xff, 0x17, 0x17, 0x00, 0x00, 0x00, 0xf5, 0x00, 0x00, 0x00, 0xf6, 0x00
.Lsec_137_40:
        .byte   0xff, 0xff, 0x44, 0x00, 0x00, 0x00, 0xf3, 0x00
.Lsec_137_48:
        .byte   0xff, 0x01, 0x44, 0x00, 0xff, 0x01, 0x54, 0x00
.Lsec_137_50:
        .byte   0xff, 0x03, 0x04, 0x00, 0xff, 0xfd, 0x14, 0x00, 0x00, 0x00, 0xf0, 0x00, 0x00, 0x00, 0xf1, 0x00
        .byte   0x00, 0x00, 0xf2, 0x00
.Lsec_137_64:
        .byte   0x00, 0x00, 0x01, 0x00, 0x05, 0x00, 0x0d, 0x00, 0x1d, 0x00, 0x00, 0x00
.Lsec_137_70:
        .byte   0x0d, 0x0c, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00
.Lsec_137_78:
        .byte   0x00, 0x00, 0x20, 0x00, 0x28, 0x00, 0x2c, 0x00, 0x2e, 0x00, 0x00, 0x00
.Lsec_137_84:
        .byte   0x00, 0x01, 0x00, 0x01
.Lsec_137_88:
        .byte   0x05, 0x09, 0x06, 0x05, 0x03, 0x06, 0x05, 0x04, 0x06, 0x04, 0x05, 0x00
.Lsec_137_94:
        .byte   0x05, 0x09, 0x06, 0x05, 0x03, 0x06, 0x09, 0x0c, 0x00, 0x00, 0x00, 0x00
.Lsec_137_a0:
        .byte   0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01

        .section .text.ram_set_pbus_mem,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_138_0, 0xffff0000
        .literal .Lsec_138_4, 0x3ff46030
        .literal .Lsec_138_8, 0x0000ffff
        .literal .Lsec_138_c, 0x3ff46034
        .literal .Lsec_138_10, 0x3ff46038
        .literal .Lsec_138_14, 0x3ff4603c
        .literal .Lsec_138_18, 0x3ff46040
        .literal .Lsec_138_1c, 0x3ff46044
        .literal .Lsec_138_20, .Lsec_137_0
        .literal .Lsec_138_24, .Lsec_137_10
        .literal .Lsec_138_28, .Lsec_137_1c
        .literal .Lsec_138_2c, .Lsec_137_40
        .literal .Lsec_138_30, .Lsec_137_48
        .literal .Lsec_138_34, .Lsec_137_30
        .literal .Lsec_138_38, .Lsec_137_50
        .literal .Lsec_138_3c, .Lsec_139_0
        .literal .Lsec_138_40, 0x3ff45038
        .literal .Lsec_138_44, 0x3ff45034
        .literal .Lsec_138_48, 0xfffc00ff
        .literal .Lsec_138_4c, 0xfffcffff
        .literal .Lsec_138_50, memcpy
        .literal .Lsec_138_54, memcpy
        .literal .Lsec_138_58, esp_dport_access_reg_read
        .literal .Lsec_138_5c, esp_dport_access_reg_read
        .literal .Lsec_138_60, esp_dport_access_reg_read
.Lsec_138_64:
        .global ram_set_pbus_mem
        .type   ram_set_pbus_mem, @function
ram_set_pbus_mem:
        entry    a1, 208
        l32r     a2, .Lsec_138_20
        movi.n   a12, 20
        add.n    a10, a1, a12
        l32r     a3, .Lsec_138_24
        l32i.n   a8, a2, 0
        l32i.n   a6, a3, 0
        l32i.n   a5, a3, 4
        l32i.n   a7, a2, 4
        l32i.n   a3, a3, 8
        l32i.n   a4, a2, 8
        l32r     a11, .Lsec_138_28
        l32i.n   a2, a2, 12
        s32i.n   a8, a1, 56
        s32i     a7, a1, 60
        s32i     a4, a1, 64
        s32i     a2, a1, 68
        movi     a2, 128
        add      a2, a1, a2
        s32i     a6, a1, 108
        s32i     a3, a1, 116
        s32i     a5, a1, 112
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_138_50
        callx8   a8
        movi.n   a12, 20
        mov.n    a10, a1
        l32r     a4, .Lsec_138_34
        l32r     a8, .Lsec_138_2c
        l32r     a7, .Lsec_138_30
        l32i.n   a11, a4, 8
        l32i.n   a15, a8, 0
        l32i.n   a14, a8, 4
        l32i.n   a8, a4, 0
        l32i.n   a13, a7, 0
        l32i.n   a9, a7, 4
        s32i     a11, a1, 144
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a7, a4, 4
        l32i.n   a4, a4, 12
        s32i.n   a8, a1, 40
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a11, .Lsec_138_38
        l32i     a8, a1, 144
        s32i.n   a4, a1, 52
        movi.n   a4, 0
        s32i     a6, a1, 96
        s32i     a3, a1, 104
        s32i.n   a7, a1, 44
        s32i     a6, a1, 84
        s32i     a3, a1, 92
        s32i     a5, a1, 100
        s32i.n   a15, a2, 0
        s32i.n   a14, a2, 4
        s32i     a13, a1, 120
        s32i     a9, a1, 124
        s32i.n   a8, a1, 48
        s32i     a5, a1, 88
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_138_54
        callx8   a8
        s32i     a6, a1, 72
        s32i     a3, a1, 80
        s32i     a4, a1, 144
        s32i     a5, a1, 76
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a6, .Lsec_138_40
        l32r     a3, .Lsec_138_44
        l32r     a7, .Lsec_138_4c
        j        .Lsec_138_127
.Lsec_138_124:
        l32i     a4, a1, 148
.Lsec_138_127:
        movi.n   a9, 10
        l32i     a11, a1, 144
        bgeu     a9, a11, .Lsec_138_132
        j        .Lsec_138_248
.Lsec_138_132:
        l32r     a2, .Lsec_138_3c
        addx4    a2, a11, a2
        l32i.n   a2, a2, 0
        jx       a2
.Lsec_138_13d:
        l32r     a2, .Lsec_138_0
        addi     a5, a1, 56
        movi.n   a12, 4
        s32i     a2, a1, 152
        movi.n   a2, 0
        nop.n
        nop.n
        nop.n
        l32r     a13, .Lsec_138_4
        j        .Lsec_138_271
.Lsec_138_156:
        l32r     a5, .Lsec_138_0
        movi.n   a12, 5
        movi.n   a2, 0
        s32i     a5, a1, 152
        addi     a5, a1, 20
        nop.n
        nop.n
        nop.n
        l32r     a13, .Lsec_138_c
        j        .Lsec_138_271
.Lsec_138_16f:
        l32r     a8, .Lsec_138_8
        addi     a5, a1, 96
        movi.n   a12, 3
        movi.n   a2, 16
        s32i     a8, a1, 152
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a13, .Lsec_138_c
        j        .Lsec_138_271
.Lsec_138_18a:
        l32r     a9, .Lsec_138_0
        movi     a5, 128
        add.n    a5, a5, a1
        movi.n   a12, 2
        movi.n   a2, 0
        s32i     a9, a1, 152
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a13, .Lsec_138_10
        j        .Lsec_138_271
.Lsec_138_1a7:
        l32r     a11, .Lsec_138_8
        addi     a5, a1, 120
        movi.n   a12, 2
        movi.n   a2, 16
        s32i     a11, a1, 152
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a13, .Lsec_138_10
        j        .Lsec_138_271
.Lsec_138_1c2:
        l32r     a2, .Lsec_138_0
        addi     a5, a1, 40
        movi.n   a12, 4
        s32i     a2, a1, 152
        movi.n   a2, 0
        nop.n
        nop.n
        nop.n
        l32r     a13, .Lsec_138_14
        j        .Lsec_138_271
.Lsec_138_1db:
        l32r     a5, .Lsec_138_8
        movi.n   a12, 3
        movi.n   a2, 16
        s32i     a5, a1, 152
        addi     a5, a1, 84
        nop.n
        nop.n
        nop.n
        l32r     a13, .Lsec_138_14
        j        .Lsec_138_271
.Lsec_138_1f4:
        l32r     a8, .Lsec_138_0
        mov.n    a5, a1
        movi.n   a12, 5
        movi.n   a2, 0
        s32i     a8, a1, 152
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a13, .Lsec_138_18
        j        .Lsec_138_271
.Lsec_138_20e:
        l32r     a9, .Lsec_138_8
        addi     a5, a1, 72
        movi.n   a12, 3
        movi.n   a2, 16
        s32i     a9, a1, 152
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a13, .Lsec_138_18
        j        .Lsec_138_271
.Lsec_138_229:
        l32r     a11, .Lsec_138_0
        movi     a5, 128
        add.n    a5, a5, a1
        movi.n   a12, 2
        movi.n   a2, 0
        s32i     a11, a1, 152
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a13, .Lsec_138_1c
        j        .Lsec_138_271
        .byte    0x00
        .byte    0x00
.Lsec_138_248:
        l32r     a2, .Lsec_138_8
        addi     a5, a1, 120
        movi.n   a12, 2
        s32i     a2, a1, 152
        movi.n   a2, 16
        nop.n
        nop.n
        nop.n
        l32r     a13, .Lsec_138_1c
        j        .Lsec_138_271
.Lsec_138_261:
        l32r     a5, .Lsec_138_8
        movi.n   a12, 3
        movi.n   a2, 16
        l32r     a13, .Lsec_138_4
        s32i     a5, a1, 152
        addi     a5, a1, 108
.Lsec_138_271:
        add.n    a8, a12, a4
        mov.n    a10, a13
        s32i     a8, a1, 148
        s32i     a12, a1, 156
        s32i     a13, a1, 160
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_138_58
        callx8   a8
        l32i     a9, a1, 148
        addi.n   a8, a9, -1
        slli     a8, a8, 8
        or       a8, a8, a4
        l32i     a11, a1, 152
        extui    a8, a8, 0, 16
        ssl      a2
        sll      a8, a8
        and      a10, a10, a11
        or       a2, a8, a10
        l32i     a13, a1, 160
        l32i     a12, a1, 156
        memw
        s32i.n   a2, a13, 0
        addmi    a2, a4, 0x200
        addx4    a4, a12, a5
.Lsec_138_2b2:
        mov.n    a10, a3
        nop.n
        l32i.n   a8, a5, 0
        addi.n   a5, a5, 4
        memw
        s32i.n   a8, a6, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_138_5c
        callx8   a8
        extui    a8, a2, 0, 10
        slli     a8, a8, 8
        addi.n   a2, a2, 1
        l32r     a9, .Lsec_138_48
        and      a10, a10, a9
        or       a10, a8, a10
        memw
        s32i.n   a10, a3, 0
        or       a10, a3, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_138_60
        callx8   a8
        and      a10, a10, a7
        memw
        s32i.n   a10, a3, 0
        bne      a4, a5, .Lsec_138_2b2
        nop.n
        nop.n
        nop.n
        nop.n
        l32i     a11, a1, 144
        addi.n   a11, a11, 1
        s32i     a11, a1, 144
        beqi     a11, 12, .Lsec_138_305
        j        .Lsec_138_124
.Lsec_138_305:
        retw.n
        .size   ram_set_pbus_mem, .-ram_set_pbus_mem
.Lsec_138_307:

        .section .rodata.ram_set_pbus_mem,"a",@progbits
        .align  4
.Lsec_139_0:
        .word   .Lsec_138_13d
        .word   .Lsec_138_261
        .word   .Lsec_138_156
        .word   .Lsec_138_16f
        .word   .Lsec_138_18a
        .word   .Lsec_138_1a7
        .word   .Lsec_138_1c2
        .word   .Lsec_138_1db
        .word   .Lsec_138_1f4
        .word   .Lsec_138_20e
        .word   .Lsec_138_229

        .section .text.ram_start_tx_tone,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_140_0, 0x3ff460b8
        .literal .Lsec_140_4, 0x66666667
        .literal .Lsec_140_8, g_phyFuns
        .literal .Lsec_140_c, esp_dport_access_reg_read
.Lsec_140_10:
        .global ram_start_tx_tone
        .type   ram_start_tx_tone, @function
ram_start_tx_tone:
        entry    a1, 32
        l32r     a10, .Lsec_140_0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_140_c
        callx8   a8
        l32r     a8, .Lsec_140_4
        bbci     a10, 29, .Lsec_140_49
        slli     a3, a3, 7
        slli     a6, a6, 7
        mulsh    a11, a3, a8
        mulsh    a8, a6, a8
        srai     a9, a11, 1
        srai     a8, a8, 1
        srai     a11, a3, 31
        srai     a14, a6, 31
        sub      a11, a9, a11
        sub      a14, a8, a14
        extui    a11, a11, 0, 12
        extui    a14, a14, 0, 12
        j        .Lsec_140_6d
.Lsec_140_49:
        slli     a3, a3, 5
        slli     a6, a6, 5
        mulsh    a11, a3, a8
        mulsh    a8, a6, a8
        srai     a9, a11, 1
        srai     a8, a8, 1
        srai     a11, a3, 31
        srai     a14, a6, 31
        sub      a11, a9, a11
        sub      a14, a8, a14
        extui    a11, a11, 0, 10
        extui    a14, a14, 0, 10
.Lsec_140_6d:
        l32r     a3, .Lsec_140_8
        extui    a10, a2, 0, 8
        extui    a15, a7, 0, 8
        extui    a13, a5, 0, 8
        extui    a12, a4, 0, 8
        l32i.n   a3, a3, 0
        l32i     a2, a3, 68
        callx8   a2
        retw.n
        .size   ram_start_tx_tone, .-ram_start_tx_tone
.Lsec_140_86:

        .section .text.ram_bb_tx_ht20_cen,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_141_0, 0x60033c6c
        .literal .Lsec_141_4, 0xf7ffffff
        .literal .Lsec_141_8, esp_dport_access_reg_read
.Lsec_141_c:
        .global ram_bb_tx_ht20_cen
        .type   ram_bb_tx_ht20_cen, @function
ram_bb_tx_ht20_cen:
        entry    a1, 32
        l32r     a3, .Lsec_141_0
        extui    a2, a2, 0, 1
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_141_8
        callx8   a8
        slli     a2, a2, 27
        l32r     a8, .Lsec_141_4
        and      a10, a10, a8
        or       a10, a2, a10
        memw
        s32i.n   a10, a3, 0
        retw.n
        .size   ram_bb_tx_ht20_cen, .-ram_bb_tx_ht20_cen
.Lsec_141_30:

        .section .text.ram_phy_get_noisefloor,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_142_0, 0x3ff5c050
        .literal .Lsec_142_4, 0x3ff5d050
        .literal .Lsec_142_8, esp_dport_access_reg_read
        .literal .Lsec_142_c, esp_dport_access_reg_read
.Lsec_142_10:
        .global ram_phy_get_noisefloor
        .type   ram_phy_get_noisefloor, @function
ram_phy_get_noisefloor:
        entry    a1, 32
        l32r     a10, .Lsec_142_0
        movi     a2, 0xfffffea1
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_142_8
        callx8   a8
        extui    a10, a10, 0, 10
        addmi    a10, a10, 0xfffffc00
        sext     a10, a10, 15
        bge      a10, a2, .Lsec_142_34
        movi     a2, 0xfffffe78
        max      a2, a10, a2
        j        .Lsec_142_37
.Lsec_142_34:
        movi     a2, 0xfffffea0
.Lsec_142_37:
        l32r     a3, .Lsec_142_4
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_142_c
        callx8   a8
        movi.n   a8, 1
        or       a10, a10, a8
        memw
        s32i.n   a10, a3, 0
        retw.n
        .size   ram_phy_get_noisefloor, .-ram_phy_get_noisefloor
.Lsec_142_4e:

        .section .text.ram_check_noise_floor,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_143_0, 0x3ff5c018
        .literal .Lsec_143_4, 0xff7fffff
        .literal .Lsec_143_8, 0x02000000
        .literal .Lsec_143_c, 0xfdffffff
        .literal .Lsec_143_10, 0x00800000
        .literal .Lsec_143_14, 0x60033c00
        .literal .Lsec_143_18, 0x00002710
        .literal .Lsec_143_1c, 0x01000000
        .literal .Lsec_143_20, 0x3ff5c050
        .literal .Lsec_143_24, esp_dport_access_reg_read
        .literal .Lsec_143_28, esp_dport_access_reg_read
        .literal .Lsec_143_2c, esp_dport_access_reg_read
        .literal .Lsec_143_30, esp_dport_access_reg_read
        .literal .Lsec_143_34, esp_dport_access_reg_read
        .literal .Lsec_143_38, esp_dport_access_reg_read
        .literal .Lsec_143_3c, esp_dport_access_reg_read
.Lsec_143_40:
        .global ram_check_noise_floor
        .type   ram_check_noise_floor, @function
ram_check_noise_floor:
        entry    a1, 32
        l32r     a2, .Lsec_143_0
        movi.n   a7, 0
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_143_24
        callx8   a8
        l32r     a3, .Lsec_143_4
        and      a3, a10, a3
        mov.n    a10, a2
        memw
        s32i     a3, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_143_28
        callx8   a8
        l32r     a3, .Lsec_143_8
        or       a3, a10, a3
        mov.n    a10, a2
        memw
        s32i     a3, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_143_2c
        callx8   a8
        l32r     a3, .Lsec_143_c
        and      a3, a10, a3
        mov.n    a10, a2
        memw
        s32i.n   a3, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_143_30
        callx8   a8
        l32r     a3, .Lsec_143_10
        or       a10, a10, a3
        l32r     a4, .Lsec_143_14
        memw
        s32i.n   a10, a2, 0
        nop.n
        nop.n
        nop.n
        nop.n
        memw
        l32i.n   a6, a4, 0
        l32r     a3, .Lsec_143_1c
        l32r     a5, .Lsec_143_18
        j        .Lsec_143_c4
        .byte    0x00
.Lsec_143_b1:
        memw
        l32i.n   a8, a4, 0
        movi.n   a9, 1
        sub      a8, a8, a6
        bltu     a5, a8, .Lsec_143_c1
        or       a9, a7, a7
.Lsec_143_c1:
        extui    a7, a9, 0, 8
.Lsec_143_c4:
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_143_34
        callx8   a8
        and      a10, a10, a3
        extui    a10, a10, 24, 8
        bnez.n   a10, .Lsec_143_dc
        beqz     a7, .Lsec_143_b1
        j        .Lsec_143_108
        .byte    0x00
        .byte    0x00
.Lsec_143_dc:
        l32r     a10, .Lsec_143_20
        movi     a4, 0xfffffea1
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_143_38
        callx8   a8
        extui    a10, a10, 0, 10
        addmi    a3, a10, 0xfffffc00
        sext     a3, a3, 15
        movi     a2, 0xfffffea0
        bge      a3, a4, .Lsec_143_fd
.Lsec_143_f7:
        movi     a2, 0xfffffe78
        max      a2, a3, a2
.Lsec_143_fd:
        beqz.n   a7, .Lsec_143_128
        neg      a2, a2
        sext     a2, a2, 15
        retw.n
        .byte    0x00
.Lsec_143_108:
        l32r     a10, .Lsec_143_20
        movi     a4, 0xfffffea1
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_143_3c
        callx8   a8
        extui    a10, a10, 0, 10
        addmi    a3, a10, 0xfffffc00
        sext     a3, a3, 15
        movi     a2, 0x160
        bge      a3, a4, .Lsec_143_128
        movi.n   a7, 1
        j        .Lsec_143_f7
.Lsec_143_128:
        retw.n
        .size   ram_check_noise_floor, .-ram_check_noise_floor
.Lsec_143_12a:

        .section .phyiram.1,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_144_0, 0x60033c6c
        .literal .Lsec_144_4, 0x08000000
        .literal .Lsec_144_8, 0x3ff5d000
        .literal .Lsec_144_c, 0xf7ffffff
        .literal .Lsec_144_10, esp_dport_access_reg_read
        .literal .Lsec_144_14, esp_dport_access_reg_read
        .literal .Lsec_144_18, esp_dport_access_reg_read
        .literal .Lsec_144_1c, esp_dport_access_reg_read
.Lsec_144_20:
        .global ram_cbw2040_cfg
        .type   ram_cbw2040_cfg, @function
ram_cbw2040_cfg:
        entry    a1, 32
        extui    a2, a2, 0, 8
        beqz     a2, .Lsec_144_59
        l32r     a4, .Lsec_144_0
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_144_10
        callx8   a8
        l32r     a2, .Lsec_144_4
        or       a2, a10, a2
        l32r     a3, .Lsec_144_8
        mov.n    a10, a3
        memw
        s32i.n   a2, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_144_14
        callx8   a8
        movi.n   a2, 64
        or       a10, a10, a2
        memw
        s32i.n   a10, a3, 0
        retw.n
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_144_59:
        l32r     a3, .Lsec_144_0
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_144_18
        callx8   a8
        l32r     a8, .Lsec_144_c
        and      a8, a10, a8
        l32r     a2, .Lsec_144_8
        or       a10, a2, a2
        memw
        s32i     a8, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_144_1c
        callx8   a8
        movi     a8, -65
        and      a10, a10, a8
        memw
        s32i.n   a10, a2, 0
        retw.n
        .size   ram_cbw2040_cfg, .-ram_cbw2040_cfg
.Lsec_144_89:

        .section .text.ram_bb_bss_bw_40_en,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_145_0, 0x3ff00024
        .literal .Lsec_145_4, esp_dport_access_reg_read
.Lsec_145_8:
        .global ram_bb_bss_bw_40_en
        .type   ram_bb_bss_bw_40_en, @function
ram_bb_bss_bw_40_en:
        entry    a1, 32
        beqz.n   a2, .Lsec_145_28
        l32r     a2, .Lsec_145_0
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_145_4
        callx8   a8
        movi.n   a8, -13
        and      a10, a10, a8
        movi.n   a8, 4
        or       a10, a10, a8
        memw
        s32i.n   a10, a2, 0
.Lsec_145_28:
        retw.n
        .size   ram_bb_bss_bw_40_en, .-ram_bb_bss_bw_40_en
.Lsec_145_2a:

        .section .text.bt_txdc_cal$part$5,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_146_0, g_phyFuns
        .literal .Lsec_146_4, tx_rf_ana_gain
        .literal .Lsec_146_8, .Lsec_273_0
        .literal .Lsec_146_c, chip7_sleep_params+0x98
        .literal .Lsec_146_10, .Lsec_273_6
        .literal .Lsec_146_14, chip7_sleep_params
        .literal .Lsec_146_18, 0x00001000
.Lsec_146_1c:
        .type   bt_txdc_cal$part$5, @function
bt_txdc_cal$part$5:
        entry    a1, 48
        l32r     a4, .Lsec_146_0
        l32i.n   a2, a4, 0
        l32i     a2, a2, 192
        callx8   a2
        l32r     a2, .Lsec_146_4
        l32i     a3, a4, 0
        l16ui    a11, a2, 2
        l16ui    a10, a2, 0
        l32i     a3, a3, 216
        callx8   a3
        movi.n   a11, 1
        or       a10, a11, a11
        l32i     a2, a4, 0
        l32i     a3, a2, 188
        l32i     a2, a2, 184
        callx8   a3
        movi.n   a12, 2
        or       a12, a10, a12
        movi.n   a11, 1
        extui    a12, a12, 0, 16
        or       a10, a11, a11
        callx8   a2
        l32r     a2, .Lsec_146_8
        movi.n   a12, 0
        l32r     a5, .Lsec_146_c
        l32r     a3, .Lsec_146_10
.Lsec_146_68:
        movi.n   a11, 2
        movi.n   a10, 1
        l32i.n   a8, a4, 0
        l32i     a8, a8, 184
        callx8   a8
        mov.n    a10, a1
        l32i.n   a8, a4, 0
        l32i     a8, a8, 0x118
        callx8   a8
        mov.n    a10, a1
        mov.n    a9, a5
        movi     a8, 4
        loop     a8, .Lsec_146_95
        l16ui    a11, a10, 0
        addi.n   a10, a10, 2
        s16i     a11, a9, 0
        memw
        addi.n   a9, a9, 2
.Lsec_146_95:
        addi.n   a2, a2, 2
        addi.n   a5, a5, 8
        beq      a2, a3, .Lsec_146_a4
        nop.n
        l16ui    a12, a2, 0
        j        .Lsec_146_68
.Lsec_146_a4:
        l32i.n   a2, a4, 0
        movi.n   a10, 0
        l32i     a2, a2, 208
        callx8   a2
        l32i.n   a2, a4, 0
        l32i     a2, a2, 196
        callx8   a2
        l32r     a3, .Lsec_146_14
        l32r     a2, .Lsec_146_18
        l32i.n   a4, a3, 0
        or       a2, a4, a2
        s32i.n   a2, a3, 0
        retw.n
        .size   bt_txdc_cal$part$5, .-bt_txdc_cal$part$5
.Lsec_146_c5:

        .section .text.bt_txiq_cal$part$6,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_147_0, chip7_sleep_params
        .literal .Lsec_147_4, .Lsec_273_0
        .literal .Lsec_147_8, chip7_sleep_params+0x98
        .literal .Lsec_147_c, chip7_sleep_params+0x92
        .literal .Lsec_147_10, .Lsec_273_6
        .literal .Lsec_147_14, tx_rf_ana_gain
        .literal .Lsec_147_18, 0x00000800
        .literal .Lsec_147_1c, rfcal_txiq
.Lsec_147_20:
        .type   bt_txiq_cal$part$6, @function
bt_txiq_cal$part$6:
        entry    a1, 32
        l32r     a4, .Lsec_147_0
        movi.n   a10, 0
        movi     a5, 120
        l8ui     a3, a4, 116
        sext     a3, a3, 7
        addi     a3, a3, 43
        extui    a3, a3, 0, 16
        l32r     a2, .Lsec_147_4
        l32r     a7, .Lsec_147_8
        l32r     a6, .Lsec_147_c
.Lsec_147_40:
        movi.n   a8, 0
        extui    a12, a10, 6, 8
        extui    a14, a10, 0, 6
        mov.n    a11, a8
        movi     a9, 6
        loop     a9, .Lsec_147_5a
        addi.n   a13, a11, 6
        bbc      a14, a8, .Lsec_147_58
        extui    a11, a13, 0, 8
.Lsec_147_58:
        addi.n   a8, a8, 1
.Lsec_147_5a:
        l32r     a8, .Lsec_147_14
        add.n    a11, a12, a11
        extui    a12, a11, 0, 8
        movi.n   a9, 6
        l16ui    a13, a8, 2
        movi.n   a8, 0
        extui    a14, a13, 6, 8
        mov.n    a11, a8
        extui    a13, a13, 0, 6
        loop     a9, .Lsec_147_7e
        addi.n   a15, a11, 6
        bbc      a13, a8, .Lsec_147_7c
        extui    a11, a15, 0, 8
.Lsec_147_7c:
        addi.n   a8, a8, 1
.Lsec_147_7e:
        add.n    a11, a14, a11
        extui    a11, a11, 0, 8
        sub      a11, a12, a11
        addx4    a11, a11, a3
        sext     a11, a11, 15
        movi.n   a14, 0
        blt      a11, a14, .Lsec_147_9a
        movi     a14, 120
        blt      a5, a11, .Lsec_147_9a
        or       a14, a11, a11
.Lsec_147_9a:
        mov.n    a12, a6
        mov.n    a11, a7
        movi.n   a15, 1
        movi.n   a13, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, rfcal_txiq
        l32r     a8, .Lsec_147_1c
        callx8   a8
        l32r     a8, .Lsec_147_10
        addi.n   a2, a2, 2
        addi.n   a7, a7, 8
        addi.n   a6, a6, 2
        beq      a8, a2, .Lsec_147_ba
        l16ui    a10, a2, 0
        j        .Lsec_147_40
.Lsec_147_ba:
        l32i.n   a3, a4, 0
        l32r     a2, .Lsec_147_18
        or       a2, a3, a2
        s32i.n   a2, a4, 0
        retw.n
        .size   bt_txiq_cal$part$6, .-bt_txiq_cal$part$6
.Lsec_147_c6:

        .section .text.spur_cal$part$8,"ax",@progbits
        .align  4
.Lsec_148_0:
        .type   spur_cal$part$8, @function
spur_cal$part$8:
        entry    a1, 32
        mul16u   a8, a5, a2
        sext     a3, a3, 7
        quos     a8, a8, a4
        mull     a8, a8, a4
        addx4    a10, a2, a2
        addx4    a9, a8, a8
        slli     a9, a9, 1
        quos     a9, a9, a5
        addx4    a3, a3, a3
        subx2    a9, a10, a9
        slli     a3, a3, 1
        bge      a9, a3, .Lsec_148_2c
        neg      a2, a9
        retw.n
.Lsec_148_2c:
        add.n    a8, a4, a8
        slli     a9, a2, 29
        addx4    a8, a8, a8
        slli     a8, a8, 1
        sub      a9, a9, a2
        quos     a5, a8, a5
        subx4    a2, a9, a2
        addx2    a2, a2, a5
        blt      a2, a3, .Lsec_148_48
        movi.n   a2, 0
.Lsec_148_48:
        retw.n
        .size   spur_cal$part$8, .-spur_cal$part$8
.Lsec_148_4a:

        .section .phyiram.2,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_149_0, chip7_phy_init_ctrl
        .literal .Lsec_149_4, 0x3ff5d014
        .literal .Lsec_149_8, 0xffffdfff
        .literal .Lsec_149_c, 0x51eb851f
        .literal .Lsec_149_10, g_phyFuns
        .literal .Lsec_149_14, 0x3ff5d018
        .literal .Lsec_149_18, 0x3ff5d01c
        .literal .Lsec_149_1c, 0x3ff5d020
        .literal .Lsec_149_20, 0x3ff5cc48
        .literal .Lsec_149_24, 0xff000000
        .literal .Lsec_149_28, 0x00ffffff
        .literal .Lsec_149_2c, rtc_get_xtal
        .literal .Lsec_149_30, esp_dport_access_reg_read
        .literal .Lsec_149_34, .Lsec_148_0
        .literal .Lsec_149_38, esp_dport_access_reg_read
        .literal .Lsec_149_3c, .Lsec_148_0
        .literal .Lsec_149_40, esp_dport_access_reg_read
        .literal .Lsec_149_44, .Lsec_148_0
        .literal .Lsec_149_48, esp_dport_access_reg_read
        .literal .Lsec_149_4c, esp_dport_access_reg_read
        .literal .Lsec_149_50, esp_dport_access_reg_read
.Lsec_149_54:
        .global ram_spur_coef_cfg
        .type   ram_spur_coef_cfg, @function
ram_spur_coef_cfg:
        entry    a1, 48
        sext     a4, a4, 7
        extui    a2, a2, 0, 8
        extui    a3, a3, 0, 16
        movi     a7, 10
        blti     a4, 2, .Lsec_149_69
        movi     a7, 20
.Lsec_149_69:
        l32r     a4, .Lsec_149_0
        l8ui     a5, a4, 1
        bnei     a5, 3, .Lsec_149_78
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_get_xtal
        l32r     a8, .Lsec_149_2c
        callx8   a8
.Lsec_149_78:
        l32r     a6, .Lsec_149_4
        or       a10, a6, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_149_30
        callx8   a8
        l32r     a5, .Lsec_149_8
        and      a10, a10, a5
        l16ui    a8, a4, 64
        memw
        s32i.n   a10, a6, 0
        beqz     a8, .Lsec_149_f9
        movi.n   a9, 1
        nop.n
        nop.n
        nop.n
        l8ui     a13, a4, 62
        l16ui    a12, a4, 60
        beqz     a13, .Lsec_149_f9
        beqz     a12, .Lsec_149_f9
        mov.n    a11, a7
        mov.n    a10, a3
        s32i.n   a8, a1, 0
        s32i     a9, a1, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, .Lsec_148_0
        l32r     a8, .Lsec_149_34
        callx8   a8
        slli     a11, a10, 10
        addi     a13, a2, 31
        l32r     a6, .Lsec_149_c
        mulsh    a6, a11, a6
        srai     a11, a11, 31
        srai     a6, a6, 5
        l32i.n   a8, a1, 0
        sub      a11, a6, a11
        srli     a12, a8, 14
        ssr      a13
        sra      a8, a8
        movi.n   a13, 0
        l32i.n   a9, a1, 4
        and      a8, a8, a12
        movnez   a13, a9, a10
        bnone    a8, a13, .Lsec_149_f9
        beqz.n   a11, .Lsec_149_f9
        l32r     a6, .Lsec_149_10
        mov.n    a10, a9
        l32i.n   a6, a6, 0
        l32i     a6, a6, 96
        callx8   a6
        j        .Lsec_149_10c
        .byte    0x00
        .byte    0x00
.Lsec_149_f9:
        l32r     a6, .Lsec_149_14
        mov.n    a10, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_149_38
        callx8   a8
        and      a10, a10, a5
        memw
        s32i.n   a10, a6, 0
.Lsec_149_10c:
        nop.n
        nop.n
        nop.n
        nop.n
        l16ui    a6, a4, 70
        beqz     a6, .Lsec_149_171
        l8ui     a13, a4, 68
        l16ui    a12, a4, 66
        beqz     a13, .Lsec_149_171
        beqz     a12, .Lsec_149_171
        mov.n    a11, a7
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, .Lsec_148_0
        l32r     a8, .Lsec_149_3c
        callx8   a8
        l32r     a11, .Lsec_149_c
        slli     a9, a10, 10
        mulsh    a11, a9, a11
        addi     a12, a2, 31
        ssr      a12
        sra      a12, a6
        srai     a8, a11, 5
        srli     a6, a6, 14
        srai     a11, a9, 31
        movi.n   a13, 1
        movi.n   a9, 0
        movnez   a9, a13, a10
        and      a6, a12, a6
        sub      a11, a8, a11
        bnone    a6, a9, .Lsec_149_171
        beqz.n   a11, .Lsec_149_171
        l32r     a6, .Lsec_149_10
        movi     a10, 2
        l32i     a6, a6, 0
        l32i     a6, a6, 96
        callx8   a6
        j        .Lsec_149_184
        .byte    0x00
        .byte    0x00
.Lsec_149_171:
        l32r     a6, .Lsec_149_18
        mov.n    a10, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_149_40
        callx8   a8
        and      a10, a10, a5
        memw
        s32i.n   a10, a6, 0
.Lsec_149_184:
        nop.n
        nop.n
        nop.n
        nop.n
        l16ui    a6, a4, 76
        beqz     a6, .Lsec_149_1e9
        l8ui     a13, a4, 74
        l16ui    a12, a4, 72
        beqz     a13, .Lsec_149_1e9
        beqz     a12, .Lsec_149_1e9
        mov.n    a11, a7
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, .Lsec_148_0
        l32r     a8, .Lsec_149_44
        callx8   a8
        l32r     a11, .Lsec_149_c
        slli     a7, a10, 10
        mulsh    a11, a7, a11
        addi     a2, a2, 31
        srli     a8, a6, 14
        srai     a4, a11, 5
        ssr      a2
        sra      a6, a6
        srai     a11, a7, 31
        movi.n   a2, 0
        movi.n   a7, 1
        and      a6, a6, a8
        movnez   a2, a7, a10
        sub      a11, a4, a11
        bnone    a6, a2, .Lsec_149_1e9
        beqz.n   a11, .Lsec_149_1e9
        l32r     a2, .Lsec_149_10
        movi     a10, 3
        l32i     a2, a2, 0
        l32i     a2, a2, 96
        callx8   a2
        j        .Lsec_149_1fd
        .byte    0x00
        .byte    0x00
.Lsec_149_1e9:
        l32r     a2, .Lsec_149_1c
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_149_48
        callx8   a8
        and      a10, a10, a5
        memw
        s32i.n   a10, a2, 0
.Lsec_149_1fd:
        movi.n   a4, 80
        nop.n
        nop.n
        nop.n
        l32r     a5, .Lsec_149_20
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_149_4c
        callx8   a8
        extui    a10, a10, 24, 5
        ssl      a10
        sll      a4, a4
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_149_50
        callx8   a8
        quos     a3, a4, a3
        l32r     a4, .Lsec_149_24
        and      a10, a10, a4
        l32r     a4, .Lsec_149_28
        and      a3, a3, a4
        or       a10, a10, a3
        memw
        s32i.n   a10, a5, 0
        retw.n
        .size   ram_spur_coef_cfg, .-ram_spur_coef_cfg
.Lsec_149_23a:

        .section .phyiram.3,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_150_0, chip7_sleep_params
        .literal .Lsec_150_4, chip7_phy_init_ctrl
        .literal .Lsec_150_8, 0x3ff5c02c
        .literal .Lsec_150_c, 0x3ff5c0a0
        .literal .Lsec_150_10, 0x00ffffff
        .literal .Lsec_150_14, 0x3ff5c0d0
        .literal .Lsec_150_18, 0xfffe01ff
        .literal .Lsec_150_1c, esp_dport_access_reg_read
        .literal .Lsec_150_20, esp_dport_access_reg_read
        .literal .Lsec_150_24, esp_dport_access_reg_read
.Lsec_150_28:
        .global set_chan_rxcomp
        .type   set_chan_rxcomp, @function
set_chan_rxcomp:
        entry    a1, 32
        l32r     a3, .Lsec_150_0
        extui    a2, a2, 0, 8
        movi.n   a4, 0
        l8ui     a3, a3, 81
        bltui    a3, 4, .Lsec_150_45
        movi     a5, 8
        movi     a4, 2
        bltu     a5, a3, .Lsec_150_45
        movi     a4, 1
.Lsec_150_45:
        sext     a3, a2, 7
        l32r     a2, .Lsec_150_4
        add.n    a4, a2, a4
        l32r     a6, .Lsec_150_8
        or       a10, a6, a6
        l8ui     a2, a4, 2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_150_1c
        callx8   a8
        sext     a2, a2, 7
        add.n    a2, a2, a3
        movi     a3, 0xffffff00
        l32r     a5, .Lsec_150_c
        and      a10, a10, a3
        extui    a3, a2, 0, 8
        or       a3, a10, a3
        mov.n    a10, a5
        memw
        s32i.n   a3, a6, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_150_20
        callx8   a8
        slli     a3, a2, 24
        l32r     a8, .Lsec_150_10
        and      a10, a10, a8
        or       a8, a10, a3
        l32r     a3, .Lsec_150_14
        mov.n    a10, a3
        memw
        s32i.n   a8, a5, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_150_24
        callx8   a8
        l8ui     a4, a4, 11
        sext     a4, a4, 7
        add.n    a2, a4, a2
        addi     a2, a2, -18
        l32r     a8, .Lsec_150_18
        extui    a2, a2, 0, 8
        slli     a2, a2, 9
        and      a10, a10, a8
        or       a2, a2, a10
        memw
        s32i.n   a2, a3, 0
        retw.n
        .size   set_chan_rxcomp, .-set_chan_rxcomp
.Lsec_150_ba:

        .section .phyiram.5,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_151_0, 0x3ff5c030
        .literal .Lsec_151_4, 0xfffc07ff
        .literal .Lsec_151_8, 0x0001a000
        .literal .Lsec_151_c, 0x3ff5c11c
        .literal .Lsec_151_10, 0x0000e000
        .literal .Lsec_151_14, 0x3ff5c120
        .literal .Lsec_151_18, 0x1e101e1c
        .literal .Lsec_151_1c, esp_dport_access_reg_read
.Lsec_151_20:
        .global phy_ant_init
        .type   phy_ant_init, @function
phy_ant_init:
        entry    a1, 32
        l32r     a2, .Lsec_151_0
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_151_1c
        callx8   a8
        l32r     a8, .Lsec_151_4
        and      a10, a10, a8
        l32r     a8, .Lsec_151_8
        or       a10, a10, a8
        memw
        s32i.n   a10, a2, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a8, .Lsec_151_10
        l32r     a2, .Lsec_151_c
        memw
        s32i.n   a8, a2, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a8, .Lsec_151_18
        l32r     a2, .Lsec_151_14
        memw
        s32i.n   a8, a2, 0
        retw.n
        .size   phy_ant_init, .-phy_ant_init
.Lsec_151_67:

        .section .phyiram.6,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_152_0, 0x3ff5d030
        .literal .Lsec_152_4, 0x3ff450f0
        .literal .Lsec_152_8, 0x3ff5d000
        .literal .Lsec_152_c, 0xe07fffff
        .literal .Lsec_152_10, 0x0a000000
        .literal .Lsec_152_14, 0x0f000000
        .literal .Lsec_152_18, esp_dport_access_reg_read
        .literal .Lsec_152_1c, esp_dport_access_reg_read
        .literal .Lsec_152_20, esp_dport_access_reg_read
        .literal .Lsec_152_24, esp_dport_access_reg_read
        .literal .Lsec_152_28, esp_dport_access_reg_read
        .literal .Lsec_152_2c, esp_dport_access_reg_read
        .literal .Lsec_152_30, phy_bt_ifs_set
.Lsec_152_34:
        .global tx_delay_cfg
        .type   tx_delay_cfg, @function
tx_delay_cfg:
        entry    a1, 32
        extui    a2, a2, 0, 8
        beqz     a2, .Lsec_152_95
        l32r     a3, .Lsec_152_0
        movi     a2, 0xfffffc00
        or       a10, a3, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_152_18
        callx8   a8
        and      a10, a10, a2
        l32r     a4, .Lsec_152_4
        movi     a2, 135
        or       a2, a10, a2
        mov.n    a10, a4
        memw
        s32i.n   a2, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_152_1c
        callx8   a8
        movi     a2, 0xfffffe00
        and      a10, a10, a2
        movi     a2, 240
        l32r     a3, .Lsec_152_8
        or       a2, a10, a2
        mov.n    a10, a3
        memw
        s32i.n   a2, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_152_20
        callx8   a8
        l32r     a2, .Lsec_152_c
        and      a10, a10, a2
        l32r     a2, .Lsec_152_10
        or       a10, a10, a2
        memw
        s32i.n   a10, a3, 0
        j        .Lsec_152_ea
.Lsec_152_95:
        l32r     a4, .Lsec_152_0
        movi     a2, 100
        or       a10, a4, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_152_24
        callx8   a8
        movi     a8, 0xfffffc00
        l32r     a3, .Lsec_152_4
        and      a10, a10, a8
        or       a8, a10, a2
        mov.n    a10, a3
        memw
        s32i.n   a8, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_152_28
        callx8   a8
        movi     a4, 0xfffffe00
        and      a10, a10, a4
        or       a2, a10, a2
        l32r     a4, .Lsec_152_8
        or       a10, a4, a4
        memw
        s32i     a2, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_152_2c
        callx8   a8
        l32r     a2, .Lsec_152_c
        and      a10, a10, a2
        l32r     a2, .Lsec_152_14
        or       a10, a10, a2
        memw
        s32i     a10, a4, 0
.Lsec_152_ea:
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_bt_ifs_set
        l32r     a8, .Lsec_152_30
        callx8   a8
        retw.n
        .size   tx_delay_cfg, .-tx_delay_cfg
.Lsec_152_f2:

        .section .phyiram.4,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_153_0, chip7_sleep_params
        .literal .Lsec_153_4, g_phyFuns
        .literal .Lsec_153_8, 0x3ff5c450
        .literal .Lsec_153_c, 0x3ff5cc0c
        .literal .Lsec_153_10, bb_bss_cbw40_ana
        .literal .Lsec_153_14, tx_delay_cfg
        .literal .Lsec_153_18, esp_dport_access_reg_read
        .literal .Lsec_153_1c, bb_bss_cbw40_ana
        .literal .Lsec_153_20, bb_bss_cbw40_ana
        .literal .Lsec_153_24, esp_dport_access_reg_read
        .literal .Lsec_153_28, esp_dport_access_reg_read
        .literal .Lsec_153_2c, tx_delay_cfg
        .literal .Lsec_153_30, esp_dport_access_reg_read
.Lsec_153_34:
        .global bb_bss_cbw40
        .type   bb_bss_cbw40, @function
bb_bss_cbw40:
        entry    a1, 32
        extui    a2, a2, 0, 8
        bnez.n   a2, .Lsec_153_7d
        l32r     a3, .Lsec_153_4
        mov.n    a10, a2
        l32i.n   a3, a3, 0
        l32r     a8, .Lsec_153_0
        l32i     a3, a3, 112
        s8i      a2, a8, 82
        memw
        callx8   a3
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, bb_bss_cbw40_ana
        l32r     a8, .Lsec_153_10
        callx8   a8
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, tx_delay_cfg
        l32r     a8, .Lsec_153_14
        callx8   a8
        l32r     a3, .Lsec_153_8
        movi.n   a2, -13
        or       a10, a3, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_153_18
        callx8   a8
        and      a10, a10, a2
        memw
        s32i.n   a10, a3, 0
        retw.n
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_153_7d:
        l32r     a3, .Lsec_153_4
        movi.n   a11, 1
        sext     a2, a2, 7
        movi.n   a10, 1
        l32i.n   a8, a3, 0
        l32r     a9, .Lsec_153_0
        l32i     a8, a8, 112
        s8i      a11, a9, 82
        memw
        callx8   a8
        bnei     a2, 1, .Lsec_153_b0
        mov.n    a10, a2
        l32i.n   a2, a3, 0
        l32i     a2, a2, 116
        callx8   a2
        movi.n   a10, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, bb_bss_cbw40_ana
        l32r     a8, .Lsec_153_1c
        callx8   a8
        j        .Lsec_153_e0
.Lsec_153_b0:
        l32i.n   a3, a3, 0
        movi.n   a10, 0
        l32i     a3, a3, 116
        callx8   a3
        movi.n   a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, bb_bss_cbw40_ana
        l32r     a8, .Lsec_153_20
        callx8   a8
        bnei     a2, 3, .Lsec_153_e0
        l32r     a3, .Lsec_153_c
        movi     a2, 1
        or       a10, a3, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_153_24
        callx8   a8
        or       a10, a10, a2
        memw
        s32i.n   a10, a3, 0
        j        .Lsec_153_f5
        .byte    0x00
.Lsec_153_e0:
        l32r     a3, .Lsec_153_c
        movi.n   a2, -2
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_153_28
        callx8   a8
        and      a10, a10, a2
        memw
        s32i.n   a10, a3, 0
.Lsec_153_f5:
        movi.n   a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, tx_delay_cfg
        l32r     a8, .Lsec_153_2c
        callx8   a8
        movi     a2, 12
        l32r     a3, .Lsec_153_8
        or       a10, a3, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_153_30
        callx8   a8
        or       a10, a10, a2
        memw
        s32i.n   a10, a3, 0
        retw.n
        .size   bb_bss_cbw40, .-bb_bss_cbw40
.Lsec_153_116:

        .section .phyiram.7,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_154_0, 0x3ff5d000
        .literal .Lsec_154_4, 0xe00007ff
        .literal .Lsec_154_8, 0x0f7e8000
        .literal .Lsec_154_c, 0x3ff46048
        .literal .Lsec_154_10, 0xffff00ff
        .literal .Lsec_154_14, 0x0000a400
        .literal .Lsec_154_18, 0x3ff5c400
        .literal .Lsec_154_1c, 0xfff8ffff
        .literal .Lsec_154_20, 0x3ff450d0
        .literal .Lsec_154_24, 0xbfffffff
        .literal .Lsec_154_28, 0x3ff450c4
        .literal .Lsec_154_2c, 0x000f4240
        .literal .Lsec_154_30, 0x3ff450c8
        .literal .Lsec_154_34, 0x3ff4e054
        .literal .Lsec_154_38, esp_dport_access_reg_read
        .literal .Lsec_154_3c, esp_dport_access_reg_read
        .literal .Lsec_154_40, esp_dport_access_reg_read
        .literal .Lsec_154_44, esp_dport_access_reg_read
        .literal .Lsec_154_48, esp_dport_access_reg_read
        .literal .Lsec_154_4c, tx_delay_cfg
.Lsec_154_50:
        .global tx_paon_set
        .type   tx_paon_set, @function
tx_paon_set:
        entry    a1, 32
        l32r     a3, .Lsec_154_0
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_154_38
        callx8   a8
        l32r     a8, .Lsec_154_4
        and      a10, a10, a8
        l32r     a8, .Lsec_154_8
        or       a8, a10, a8
        l32r     a2, .Lsec_154_c
        or       a10, a2, a2
        memw
        s32i     a8, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_154_3c
        callx8   a8
        l32r     a8, .Lsec_154_10
        and      a10, a10, a8
        l32r     a8, .Lsec_154_14
        or       a8, a10, a8
        l32r     a3, .Lsec_154_18
        mov.n    a10, a3
        memw
        s32i.n   a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_154_40
        callx8   a8
        l32r     a8, .Lsec_154_1c
        and      a8, a10, a8
        l32r     a2, .Lsec_154_20
        or       a10, a2, a2
        memw
        s32i     a8, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_154_44
        callx8   a8
        l32r     a8, .Lsec_154_24
        and      a10, a10, a8
        l32r     a3, .Lsec_154_28
        l32r     a8, .Lsec_154_2c
        memw
        s32i.n   a10, a2, 0
        memw
        s32i.n   a8, a3, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a2, .Lsec_154_34
        mov.n    a10, a2
        l32r     a3, .Lsec_154_30
        memw
        s32i.n   a8, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_154_48
        callx8   a8
        movi     a8, 0xffffff00
        and      a10, a10, a8
        movi     a8, 100
        or       a10, a10, a8
        memw
        s32i.n   a10, a2, 0
        movi.n   a10, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, tx_delay_cfg
        l32r     a8, .Lsec_154_4c
        callx8   a8
        retw.n
        .size   tx_paon_set, .-tx_paon_set
.Lsec_154_fc:

        .section .phyiram.8,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_155_0, 0x3ff5c004
        .literal .Lsec_155_4, 0xfffeffff
        .literal .Lsec_155_8, 0x3ff5c0a4
        .literal .Lsec_155_c, 0xffffc07f
        .literal .Lsec_155_10, 0x00001e00
        .literal .Lsec_155_14, 0x3ff5c0c4
        .literal .Lsec_155_18, 0x6450403f
        .literal .Lsec_155_1c, 0x3ff5c030
        .literal .Lsec_155_20, 0x03c1a4d5
        .literal .Lsec_155_24, 0x3ff5c094
        .literal .Lsec_155_28, 0x01b8dd03
        .literal .Lsec_155_2c, 0x3ff5c02c
        .literal .Lsec_155_30, 0x324048fe
        .literal .Lsec_155_34, 0x00800000
        .literal .Lsec_155_38, 0xff7fffff
        .literal .Lsec_155_3c, 0x3ff5c018
        .literal .Lsec_155_40, 0x40000000
        .literal .Lsec_155_44, 0x3ff5c01c
        .literal .Lsec_155_48, 0x3ff5c028
        .literal .Lsec_155_4c, 0x3ff5c0f8
        .literal .Lsec_155_50, 0xfffc01ff
        .literal .Lsec_155_54, 0x00037400
        .literal .Lsec_155_58, 0x3ff5c038
        .literal .Lsec_155_5c, 0xf7ffffff
        .literal .Lsec_155_60, 0x3ff5c088
        .literal .Lsec_155_64, 0x3ff5c104
        .literal .Lsec_155_68, 0xffff7fff
        .literal .Lsec_155_6c, 0x3ff5c07c
        .literal .Lsec_155_70, 0xff00ffff
        .literal .Lsec_155_74, 0x009c0000
        .literal .Lsec_155_78, esp_dport_access_reg_read
        .literal .Lsec_155_7c, esp_dport_access_reg_read
        .literal .Lsec_155_80, esp_dport_access_reg_read
        .literal .Lsec_155_84, esp_dport_access_reg_read
        .literal .Lsec_155_88, esp_dport_access_reg_read
        .literal .Lsec_155_8c, esp_dport_access_reg_read
        .literal .Lsec_155_90, esp_dport_access_reg_read
        .literal .Lsec_155_94, esp_dport_access_reg_read
        .literal .Lsec_155_98, esp_dport_access_reg_read
        .literal .Lsec_155_9c, esp_dport_access_reg_read
        .literal .Lsec_155_a0, esp_dport_access_reg_read
        .literal .Lsec_155_a4, esp_dport_access_reg_read
        .literal .Lsec_155_a8, esp_dport_access_reg_read
        .literal .Lsec_155_ac, esp_dport_access_reg_read
.Lsec_155_b0:
        .global agc_reg_init
        .type   agc_reg_init, @function
agc_reg_init:
        entry    a1, 32
        l32r     a4, .Lsec_155_0
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_155_78
        callx8   a8
        l32r     a3, .Lsec_155_4
        and      a3, a10, a3
        l32r     a2, .Lsec_155_8
        mov.n    a10, a2
        memw
        s32i.n   a3, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_155_7c
        callx8   a8
        l32r     a3, .Lsec_155_c
        and      a4, a10, a3
        mov.n    a10, a2
        l32r     a3, .Lsec_155_10
        or       a3, a4, a3
        memw
        s32i.n   a3, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_155_80
        callx8   a8
        movi     a3, -128
        and      a10, a10, a3
        movi.n   a3, 60
        or       a10, a10, a3
        memw
        s32i.n   a10, a2, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a3, .Lsec_155_18
        l32r     a2, .Lsec_155_14
        l32r     a4, .Lsec_155_20
        memw
        s32i.n   a3, a2, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a3, .Lsec_155_1c
        memw
        s32i.n   a4, a3, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a4, .Lsec_155_28
        l32r     a3, .Lsec_155_24
        l32r     a2, .Lsec_155_2c
        mov.n    a10, a2
        memw
        s32i.n   a4, a3, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a3, .Lsec_155_30
        memw
        s32i.n   a3, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_155_84
        callx8   a8
        l32r     a3, .Lsec_155_34
        or       a3, a10, a3
        mov.n    a10, a2
        memw
        s32i.n   a3, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_155_88
        callx8   a8
        l32r     a4, .Lsec_155_38
        and      a4, a10, a4
        l32r     a3, .Lsec_155_3c
        mov.n    a10, a3
        memw
        s32i.n   a4, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_155_8c
        callx8   a8
        movi.n   a2, -8
        and      a10, a10, a2
        movi.n   a2, 4
        or       a10, a10, a2
        memw
        s32i.n   a10, a3, 0
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_155_90
        callx8   a8
        l32r     a2, .Lsec_155_40
        or       a2, a10, a2
        l32r     a4, .Lsec_155_44
        mov.n    a10, a4
        memw
        s32i     a2, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_155_94
        callx8   a8
        movi     a2, 0xffffff00
        and      a10, a10, a2
        movi     a3, 191
        l32r     a5, .Lsec_155_48
        or       a3, a10, a3
        mov.n    a10, a5
        memw
        s32i.n   a3, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_155_98
        callx8   a8
        movi     a3, 0xfffffe00
        and      a10, a10, a3
        movi     a3, 0x1ba
        l32r     a4, .Lsec_155_4c
        or       a3, a10, a3
        mov.n    a10, a4
        memw
        s32i.n   a3, a5, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_155_9c
        callx8   a8
        l32r     a3, .Lsec_155_50
        and      a10, a10, a3
        l32r     a3, .Lsec_155_54
        or       a3, a10, a3
        l32r     a5, .Lsec_155_58
        mov.n    a10, a5
        memw
        s32i.n   a3, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_155_a0
        callx8   a8
        l32r     a3, .Lsec_155_5c
        and      a3, a10, a3
        l32r     a4, .Lsec_155_60
        or       a10, a4, a4
        memw
        s32i     a3, a5, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_155_a4
        callx8   a8
        and      a10, a10, a2
        movi.n   a2, 5
        or       a2, a10, a2
        l32r     a5, .Lsec_155_64
        mov.n    a10, a5
        memw
        s32i.n   a2, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_155_a8
        callx8   a8
        l32r     a2, .Lsec_155_68
        and      a2, a10, a2
        l32r     a3, .Lsec_155_6c
        or       a10, a3, a3
        memw
        s32i     a2, a5, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_155_ac
        callx8   a8
        l32r     a2, .Lsec_155_70
        and      a10, a10, a2
        l32r     a2, .Lsec_155_74
        or       a10, a10, a2
        memw
        s32i.n   a10, a3, 0
        retw.n
        .size   agc_reg_init, .-agc_reg_init
.Lsec_155_257:

        .section .phyiram.9,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_156_0, 0x3ff5cc48
        .literal .Lsec_156_4, 0x170433af
        .literal .Lsec_156_8, 0x3ff5cce4
        .literal .Lsec_156_c, 0xffbfffff
        .literal .Lsec_156_10, 0x3ff5c400
        .literal .Lsec_156_14, 0xffff9f00
        .literal .Lsec_156_18, 0x000060a5
        .literal .Lsec_156_1c, 0x3ff5cd04
        .literal .Lsec_156_20, 0x51510556
        .literal .Lsec_156_24, 0x3ff5cd08
        .literal .Lsec_156_28, 0x3ff5cc0c
        .literal .Lsec_156_2c, 0x001241fc
        .literal .Lsec_156_30, 0x3ff5cc08
        .literal .Lsec_156_34, 0x090b4d20
        .literal .Lsec_156_38, 0x3ff5ccdc
        .literal .Lsec_156_3c, 0x0195c83c
        .literal .Lsec_156_40, 0x3ff5c044
        .literal .Lsec_156_44, 0xfe00ffff
        .literal .Lsec_156_48, 0x01fb0000
        .literal .Lsec_156_4c, 0x3ff5c024
        .literal .Lsec_156_50, 0xfffc01ff
        .literal .Lsec_156_54, 0x00036800
        .literal .Lsec_156_58, 0x3ff5c094
        .literal .Lsec_156_5c, 0x01b8dd03
        .literal .Lsec_156_60, 0x3ff5cc04
        .literal .Lsec_156_64, 0x3ff5ccd8
        .literal .Lsec_156_68, esp_dport_access_reg_read
        .literal .Lsec_156_6c, esp_dport_access_reg_read
        .literal .Lsec_156_70, esp_dport_access_reg_read
        .literal .Lsec_156_74, esp_dport_access_reg_read
        .literal .Lsec_156_78, esp_dport_access_reg_read
.Lsec_156_7c:
        .global bb_reg_init
        .type   bb_reg_init, @function
bb_reg_init:
        entry    a1, 32
        l32r     a4, .Lsec_156_8
        mov.n    a10, a4
        l32r     a3, .Lsec_156_4
        l32r     a2, .Lsec_156_0
        memw
        s32i.n   a3, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_156_68
        callx8   a8
        l32r     a2, .Lsec_156_c
        and      a2, a10, a2
        l32r     a3, .Lsec_156_10
        mov.n    a10, a3
        memw
        s32i     a2, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_156_6c
        callx8   a8
        l32r     a2, .Lsec_156_14
        and      a10, a10, a2
        l32r     a2, .Lsec_156_18
        or       a10, a10, a2
        memw
        s32i.n   a10, a3, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a2, .Lsec_156_20
        l32r     a3, .Lsec_156_1c
        memw
        s32i.n   a2, a3, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a3, .Lsec_156_24
        memw
        s32i.n   a2, a3, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a3, .Lsec_156_2c
        l32r     a2, .Lsec_156_28
        memw
        s32i.n   a3, a2, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a3, .Lsec_156_34
        l32r     a2, .Lsec_156_30
        l32r     a4, .Lsec_156_40
        mov.n    a10, a4
        memw
        s32i.n   a3, a2, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a3, .Lsec_156_3c
        l32r     a2, .Lsec_156_38
        memw
        s32i     a3, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_156_70
        callx8   a8
        l32r     a2, .Lsec_156_44
        and      a10, a10, a2
        l32r     a2, .Lsec_156_48
        or       a2, a10, a2
        l32r     a3, .Lsec_156_4c
        mov.n    a10, a3
        memw
        s32i.n   a2, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_156_74
        callx8   a8
        l32r     a8, .Lsec_156_50
        and      a10, a10, a8
        l32r     a8, .Lsec_156_54
        or       a8, a10, a8
        l32r     a2, .Lsec_156_60
        mov.n    a10, a2
        memw
        s32i.n   a8, a3, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a4, .Lsec_156_5c
        l32r     a3, .Lsec_156_58
        memw
        s32i.n   a4, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_156_78
        callx8   a8
        movi     a8, 0xfffffe03
        and      a10, a10, a8
        movi.n   a8, 44
        or       a10, a10, a8
        memw
        s32i.n   a10, a2, 0
        movi     a3, 0x186
        nop.n
        nop.n
        nop.n
        l32r     a2, .Lsec_156_64
        memw
        s32i.n   a3, a2, 0
        retw.n
        .size   bb_reg_init, .-bb_reg_init
.Lsec_156_193:

        .section .phyiram.10,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_157_0, 0x3ff00024
        .literal .Lsec_157_4, 0x10000000
        .literal .Lsec_157_8, esp_dport_access_reg_read
        .literal .Lsec_157_c, esp_dport_access_reg_read
        .literal .Lsec_157_10, esp_dport_access_reg_read
        .literal .Lsec_157_14, esp_dport_access_reg_read
        .literal .Lsec_157_18, esp_dport_access_reg_read
        .literal .Lsec_157_1c, esp_dport_access_reg_read
        .literal .Lsec_157_20, esp_dport_access_reg_read
.Lsec_157_24:
        .global mac_enable_bb
        .type   mac_enable_bb, @function
mac_enable_bb:
        entry    a1, 32
        l32r     a2, .Lsec_157_0
        movi.n   a4, -3
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_157_8
        callx8   a8
        l32r     a3, .Lsec_157_4
        or       a3, a10, a3
        mov.n    a10, a2
        memw
        s32i.n   a3, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_157_c
        callx8   a8
        and      a3, a10, a4
        or       a10, a2, a2
        memw
        s32i     a3, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_157_10
        callx8   a8
        movi.n   a3, 2
        or       a8, a10, a3
        mov.n    a10, a2
        memw
        s32i.n   a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_157_14
        callx8   a8
        and      a8, a10, a4
        mov.n    a10, a2
        memw
        s32i.n   a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_157_18
        callx8   a8
        or       a8, a10, a3
        mov.n    a10, a2
        memw
        s32i.n   a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_157_1c
        callx8   a8
        and      a4, a10, a4
        mov.n    a10, a2
        memw
        s32i.n   a4, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_157_20
        callx8   a8
        or       a10, a10, a3
        memw
        s32i.n   a10, a2, 0
        retw.n
        .size   mac_enable_bb, .-mac_enable_bb
.Lsec_157_a6:

        .section .phyiram.11,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_158_0, 0x3ff5d040
        .literal .Lsec_158_4, 0x80000000
        .literal .Lsec_158_8, 0x3ff5d03c
        .literal .Lsec_158_c, 0x401800aa
        .literal .Lsec_158_10, esp_dport_access_reg_read
.Lsec_158_14:
        .global bb_wdg_cfg
        .type   bb_wdg_cfg, @function
bb_wdg_cfg:
        entry    a1, 32
        l32r     a2, .Lsec_158_0
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_158_10
        callx8   a8
        l32r     a8, .Lsec_158_4
        or       a10, a10, a8
        memw
        s32i.n   a10, a2, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a8, .Lsec_158_c
        l32r     a2, .Lsec_158_8
        memw
        s32i.n   a8, a2, 0
        retw.n
        .size   bb_wdg_cfg, .-bb_wdg_cfg
.Lsec_158_42:

        .section .phyiram.13,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_159_0, 0x3ff5c044
        .literal .Lsec_159_4, 0x01ff0000
        .literal .Lsec_159_8, 0xffffc0ff
        .literal .Lsec_159_c, 0x00002100
        .literal .Lsec_159_10, 0x3ff5c124
        .literal .Lsec_159_14, 0xffff03ff
        .literal .Lsec_159_18, 0x00008400
        .literal .Lsec_159_1c, 0x3ff5c804
        .literal .Lsec_159_20, 0xffff0fff
        .literal .Lsec_159_24, 0x00009000
        .literal .Lsec_159_28, 0x3ff5c104
        .literal .Lsec_159_2c, 0xfe00ffff
        .literal .Lsec_159_30, 0x01fb0000
        .literal .Lsec_159_34, 0x00001800
        .literal .Lsec_159_38, 0x00006000
        .literal .Lsec_159_3c, esp_dport_access_reg_read
        .literal .Lsec_159_40, esp_dport_access_reg_read
        .literal .Lsec_159_44, esp_dport_access_reg_read
        .literal .Lsec_159_48, esp_dport_access_reg_read
        .literal .Lsec_159_4c, esp_dport_access_reg_read
        .literal .Lsec_159_50, esp_dport_access_reg_read
        .literal .Lsec_159_54, esp_dport_access_reg_read
        .literal .Lsec_159_58, esp_dport_access_reg_read
        .literal .Lsec_159_5c, esp_dport_access_reg_read
        .literal .Lsec_159_60, esp_dport_access_reg_read
        .literal .Lsec_159_64, esp_dport_access_reg_read
        .literal .Lsec_159_68, esp_dport_access_reg_read
.Lsec_159_6c:
        .global rx_11b_opt
        .type   rx_11b_opt, @function
rx_11b_opt:
        entry    a1, 32
        extui    a2, a2, 0, 8
        beqz     a2, .Lsec_159_115
        l32r     a3, .Lsec_159_0
        or       a10, a3, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_159_3c
        callx8   a8
        l32r     a2, .Lsec_159_4
        or       a2, a10, a2
        mov.n    a10, a3
        memw
        s32i     a2, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_159_40
        callx8   a8
        l32r     a4, .Lsec_159_8
        and      a10, a10, a4
        l32r     a2, .Lsec_159_c
        or       a4, a10, a2
        l32r     a2, .Lsec_159_10
        mov.n    a10, a2
        memw
        s32i.n   a4, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_159_44
        callx8   a8
        l32r     a3, .Lsec_159_14
        and      a4, a10, a3
        mov.n    a10, a2
        l32r     a3, .Lsec_159_18
        or       a3, a4, a3
        memw
        s32i.n   a3, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_159_48
        callx8   a8
        movi.n   a3, -16
        and      a10, a10, a3
        movi.n   a3, 3
        l32r     a4, .Lsec_159_1c
        or       a3, a10, a3
        mov.n    a10, a4
        memw
        s32i.n   a3, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_159_4c
        callx8   a8
        l32r     a2, .Lsec_159_20
        and      a10, a10, a2
        l32r     a2, .Lsec_159_24
        or       a2, a10, a2
        l32r     a3, .Lsec_159_28
        mov.n    a10, a3
        memw
        s32i.n   a2, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_159_50
        callx8   a8
        movi     a2, 0xfffffe00
        and      a10, a10, a2
        movi     a2, 0x1e2
        or       a10, a10, a2
        memw
        s32i.n   a10, a3, 0
        retw.n
        .byte    0x00
        .byte    0x00
.Lsec_159_115:
        l32r     a2, .Lsec_159_0
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_159_54
        callx8   a8
        l32r     a3, .Lsec_159_2c
        and      a4, a10, a3
        mov.n    a10, a2
        l32r     a3, .Lsec_159_30
        or       a3, a4, a3
        memw
        s32i.n   a3, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_159_58
        callx8   a8
        l32r     a4, .Lsec_159_8
        and      a10, a10, a4
        l32r     a3, .Lsec_159_34
        or       a4, a10, a3
        l32r     a3, .Lsec_159_10
        mov.n    a10, a3
        memw
        s32i.n   a4, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_159_5c
        callx8   a8
        l32r     a8, .Lsec_159_14
        and      a8, a10, a8
        mov.n    a10, a3
        l32r     a2, .Lsec_159_38
        or       a8, a8, a2
        memw
        s32i.n   a8, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_159_60
        callx8   a8
        movi.n   a8, -16
        and      a10, a10, a8
        movi.n   a8, 4
        l32r     a4, .Lsec_159_1c
        or       a8, a10, a8
        mov.n    a10, a4
        memw
        s32i.n   a8, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_159_64
        callx8   a8
        l32r     a3, .Lsec_159_20
        and      a10, a10, a3
        or       a2, a10, a2
        l32r     a3, .Lsec_159_28
        mov.n    a10, a3
        memw
        s32i.n   a2, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_159_68
        callx8   a8
        movi     a2, 0xfffffe00
        and      a10, a10, a2
        movi     a2, 0x1d2
        or       a10, a10, a2
        memw
        s32i.n   a10, a3, 0
        retw.n
        .size   rx_11b_opt, .-rx_11b_opt
.Lsec_159_1b4:

        .section .phyiram.15,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_160_0, 0x3ff5c0a0
        .literal .Lsec_160_4, 0xff00ffff
        .literal .Lsec_160_8, 0x00800000
        .literal .Lsec_160_c, 0x3ff5c01c
        .literal .Lsec_160_10, 0x000c0000
        .literal .Lsec_160_14, 0x3ff5c094
        .literal .Lsec_160_18, 0xffff01ff
        .literal .Lsec_160_1c, 0x0000dc00
        .literal .Lsec_160_20, 0x3ff5c0f0
        .literal .Lsec_160_24, 0x3ff5c0b8
        .literal .Lsec_160_28, 0x00000800
        .literal .Lsec_160_2c, esp_dport_access_reg_read
        .literal .Lsec_160_30, esp_dport_access_reg_read
        .literal .Lsec_160_34, esp_dport_access_reg_read
        .literal .Lsec_160_38, esp_dport_access_reg_read
        .literal .Lsec_160_3c, esp_dport_access_reg_read
.Lsec_160_40:
        .global opt_11b_resart
        .type   opt_11b_resart, @function
opt_11b_resart:
        entry    a1, 32
        l32r     a5, .Lsec_160_0
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_160_2c
        callx8   a8
        l32r     a2, .Lsec_160_4
        and      a10, a10, a2
        l32r     a3, .Lsec_160_8
        or       a3, a10, a3
        l32r     a4, .Lsec_160_c
        or       a10, a4, a4
        memw
        s32i     a3, a5, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_160_30
        callx8   a8
        and      a10, a10, a2
        l32r     a2, .Lsec_160_10
        or       a2, a10, a2
        l32r     a3, .Lsec_160_14
        mov.n    a10, a3
        memw
        s32i.n   a2, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_160_34
        callx8   a8
        l32r     a2, .Lsec_160_18
        and      a10, a10, a2
        l32r     a2, .Lsec_160_1c
        or       a2, a10, a2
        l32r     a4, .Lsec_160_20
        mov.n    a10, a4
        memw
        s32i.n   a2, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_160_38
        callx8   a8
        movi     a2, 0xffffff00
        and      a10, a10, a2
        movi     a2, 128
        l32r     a3, .Lsec_160_24
        or       a2, a10, a2
        mov.n    a10, a3
        memw
        s32i.n   a2, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_160_3c
        callx8   a8
        l32r     a2, .Lsec_160_28
        or       a10, a10, a2
        memw
        s32i.n   a10, a3, 0
        retw.n
        .size   opt_11b_resart, .-opt_11b_resart
.Lsec_160_ca:

        .section .phyiram.14,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_161_0, tx_rf_ana_gain
        .literal .Lsec_161_4, chip7_sleep_params
        .literal .Lsec_161_8, 0x3ff5d040
        .literal .Lsec_161_c, 0x80000000
        .literal .Lsec_161_10, 0x3ff5d03c
        .literal .Lsec_161_14, 0x401800aa
        .literal .Lsec_161_18, g_phyFuns
        .literal .Lsec_161_1c, 0x3ff5c030
        .literal .Lsec_161_20, 0xfffc07ff
        .literal .Lsec_161_24, 0x0001a000
        .literal .Lsec_161_28, 0x3ff5c11c
        .literal .Lsec_161_2c, 0x0000e000
        .literal .Lsec_161_30, 0x3ff5c120
        .literal .Lsec_161_34, 0x1e101e1c
        .literal .Lsec_161_38, tx_paon_set
        .literal .Lsec_161_3c, bb_reg_init
        .literal .Lsec_161_40, agc_reg_init
        .literal .Lsec_161_44, esp_dport_access_reg_read
        .literal .Lsec_161_48, mac_enable_bb
        .literal .Lsec_161_4c, phy_set_bbfreq_init
        .literal .Lsec_161_50, esp_dport_access_reg_read
        .literal .Lsec_161_54, opt_11b_resart
        .literal .Lsec_161_58, rx_11b_opt
.Lsec_161_5c:
        .global phy_reg_init
        .type   phy_reg_init, @function
phy_reg_init:
        entry    a1, 32
        l32r     a8, .Lsec_161_4
        l32r     a2, .Lsec_161_0
        l16ui    a9, a8, 188
        l16ui    a8, a8, 190
        s16i     a9, a2, 0
        s16i     a8, a2, 2
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, tx_paon_set
        l32r     a8, .Lsec_161_38
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, bb_reg_init
        l32r     a8, .Lsec_161_3c
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, agc_reg_init
        l32r     a8, .Lsec_161_40
        callx8   a8
        l32r     a2, .Lsec_161_8
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_161_44
        callx8   a8
        l32r     a8, .Lsec_161_c
        or       a10, a10, a8
        memw
        s32i.n   a10, a2, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a8, .Lsec_161_14
        l32r     a2, .Lsec_161_10
        memw
        s32i.n   a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, mac_enable_bb
        l32r     a8, .Lsec_161_48
        callx8   a8
        movi.n   a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_set_bbfreq_init
        l32r     a8, .Lsec_161_4c
        callx8   a8
        l32r     a2, .Lsec_161_18
        l32i     a8, a2, 0
        l32i     a8, a8, 0x1a4
        callx8   a8
        l32i.n   a2, a2, 0
        l32i     a2, a2, 120
        callx8   a2
        l32r     a2, .Lsec_161_1c
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_161_50
        callx8   a8
        l32r     a8, .Lsec_161_20
        and      a10, a10, a8
        l32r     a8, .Lsec_161_24
        or       a10, a10, a8
        memw
        s32i.n   a10, a2, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a8, .Lsec_161_2c
        l32r     a2, .Lsec_161_28
        memw
        s32i.n   a8, a2, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a8, .Lsec_161_34
        l32r     a2, .Lsec_161_30
        memw
        s32i.n   a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, opt_11b_resart
        l32r     a8, .Lsec_161_54
        callx8   a8
        movi.n   a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, rx_11b_opt
        l32r     a8, .Lsec_161_58
        callx8   a8
        retw.n
        .size   phy_reg_init, .-phy_reg_init
.Lsec_161_123:

        .section .phyiram.16,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_162_0, chip7_sleep_params
        .literal .Lsec_162_4, 0x3ff4e0c4
        .literal .Lsec_162_8, 0xe7ffffff
        .literal .Lsec_162_c, 0x10000000
        .literal .Lsec_162_10, 0xfff0ffff
        .literal .Lsec_162_14, 0xff0fffff
        .literal .Lsec_162_18, g_phyFuns
        .literal .Lsec_162_1c, chip7_sleep_params+0x40
        .literal .Lsec_162_20, chip7_phy_init_ctrl
        .literal .Lsec_162_24, chan_to_freq
        .literal .Lsec_162_28, esp_dport_access_reg_read
        .literal .Lsec_162_2c, esp_dport_access_reg_read
        .literal .Lsec_162_30, esp_dport_access_reg_read
        .literal .Lsec_162_34, esp_dport_access_reg_read
        .literal .Lsec_162_38, esp_dport_access_reg_read
        .literal .Lsec_162_3c, esp_dport_access_reg_read
        .literal .Lsec_162_40, esp_dport_access_reg_read
        .literal .Lsec_162_44, esp_dport_access_reg_read
        .literal .Lsec_162_48, chan_to_freq
        .literal .Lsec_162_4c, set_chan_rxcomp
        .literal .Lsec_162_50, esp_dport_access_reg_read
        .literal .Lsec_162_54, esp_dport_access_reg_read
        .literal .Lsec_162_58, bb_bss_cbw40
        .literal .Lsec_162_5c, esp_dport_access_reg_read
        .literal .Lsec_162_60, esp_dport_access_reg_read
        .literal .Lsec_162_64, esp_dport_access_reg_read
        .literal .Lsec_162_68, esp_dport_access_reg_read
        .literal .Lsec_162_6c, phy_freq_correct_opt
.Lsec_162_70:
        .global set_chan_reg
        .type   set_chan_reg, @function
set_chan_reg:
        entry    a1, 48
        l32r     a7, .Lsec_162_0
        l8ui     a14, a7, 81
        sext     a4, a14, 7
        mov.n    a10, a4
        s32i.n   a14, a1, 8
        .reloc  ., R_XTENSA_ASM_EXPAND, chan_to_freq
        l32r     a8, .Lsec_162_24
        callx8   a8
        mov.n    a3, a10
        addi     a3, a3, -96
        extui    a3, a3, 0, 8
        l32r     a2, .Lsec_162_4
        slli     a3, a3, 1
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_162_28
        callx8   a8
        extui    a3, a3, 0, 8
        l32r     a5, .Lsec_162_8
        and      a6, a10, a5
        mov.n    a10, a2
        l32r     a5, .Lsec_162_c
        or       a5, a6, a5
        memw
        s32i.n   a5, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_162_2c
        callx8   a8
        extui    a5, a10, 16, 4
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_162_30
        callx8   a8
        extui    a6, a10, 20, 4
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_162_34
        callx8   a8
        l32r     a8, .Lsec_162_10
        and      a9, a10, a8
        mov.n    a10, a2
        memw
        s32i     a9, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_162_38
        callx8   a8
        l32r     a8, .Lsec_162_14
        and      a9, a10, a8
        mov.n    a10, a2
        memw
        s32i     a9, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_162_3c
        callx8   a8
        movi     a9, 0xfffffeff
        and      a9, a10, a9
        mov.n    a10, a2
        memw
        s32i     a9, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_162_40
        callx8   a8
        movi     a9, 0xffffff00
        and      a10, a10, a9
        or       a3, a3, a10
        memw
        s32i.n   a3, a2, 0
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_162_44
        callx8   a8
        movi     a9, 0x100
        or       a11, a10, a9
        l32r     a3, .Lsec_162_18
        mov.n    a10, a4
        memw
        s32i.n   a11, a2, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32i     a11, a3, 0
        l32i     a13, a11, 100
        s32i     a9, a1, 4
        s32i     a13, a1, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, chan_to_freq
        l32r     a8, .Lsec_162_48
        callx8   a8
        mov.n    a11, a10
        mov.n    a10, a4
        l8ui     a12, a7, 83
        sext     a12, a12, 7
        l32i     a13, a1, 0
        callx8   a13
        l32i.n   a8, a3, 0
        l16si    a10, a7, 78
        l32i     a8, a8, 132
        callx8   a8
        movi.n   a10, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, set_chan_rxcomp
        l32r     a8, .Lsec_162_4c
        callx8   a8
        l32i.n   a3, a3, 0
        l32i.n   a14, a1, 8
        mov.n    a11, a14
        l32i     a3, a3, 0x16c
        l32r     a10, .Lsec_162_1c
        callx8   a3
        mov.n    a3, a2
        l32i.n   a9, a1, 4
        mov.n    a4, a9
.Lsec_162_186:
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_162_50
        callx8   a8
        bgez     a10, .Lsec_162_202
.Lsec_162_191:
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_162_54
        callx8   a8
        movi     a3, 0xfffffeff
        l8ui     a4, a7, 83
        and      a3, a10, a3
        sext     a10, a4, 7
        slli     a5, a5, 16
        slli     a6, a6, 20
        memw
        s32i.n   a3, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, bb_bss_cbw40
        l32r     a8, .Lsec_162_58
        callx8   a8
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_162_5c
        callx8   a8
        l32r     a3, .Lsec_162_10
        and      a10, a10, a3
        or       a10, a10, a5
        memw
        s32i.n   a10, a2, 0
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_162_60
        callx8   a8
        l32r     a5, .Lsec_162_14
        and      a10, a10, a5
        or       a10, a10, a6
        memw
        s32i     a10, a2, 0
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_162_64
        callx8   a8
        l32r     a8, .Lsec_162_8
        and      a10, a10, a8
        l32r     a3, .Lsec_162_20
        l8ui     a3, a3, 78
        memw
        s32i.n   a10, a2, 0
        beqz.n   a3, .Lsec_162_219
        j        .Lsec_162_213
.Lsec_162_202:
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_162_68
        callx8   a8
        bnone    a10, a4, .Lsec_162_210
        j        .Lsec_162_186
.Lsec_162_210:
        j        .Lsec_162_191
.Lsec_162_213:
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_freq_correct_opt
        l32r     a8, .Lsec_162_6c
        callx8   a8
.Lsec_162_219:
        retw.n
        .size   set_chan_reg, .-set_chan_reg
.Lsec_162_21b:

        .section .rodata.str1.4,"aMS",@progbits,1
        .align  4
.Lsec_163_0:
        .ascii  "i2c_master_reset"
        .ascii  " %d\012\000\000\000\000"
.Lsec_163_18:
        .ascii  "index: %d value:"
        .ascii  " 0x%x 0x%x 0x%x "
        .ascii  "%d %d %d %d\012\000\000\000\000"
.Lsec_163_48:
        .ascii  "max_gain: %d\012\000\000\000"
.Lsec_163_58:
        .ascii  "total_pwr=%ld, m"
        .ascii  "in=%ld, max=%ld,"
        .ascii  " rftx=0x%x, att="
        .ascii  "%d, dc_i=%d, dc_"
        .ascii  "q=%d\012\000\000\000"
.Lsec_163_a0:
        .ascii  "rxiq: rftx=0x%x,"
        .ascii  " rfrx=x%x, att=%"
        .ascii  "d, bb=0x%x, %d, "
        .ascii  "%d\012\000"
.Lsec_163_d4:
        .ascii  "14:30:06\000\000\000\000"
.Lsec_163_e0:
        .ascii  "Oct 28 2025\000"
.Lsec_163_ec:
        .ascii  "a3a4459\000"
.Lsec_163_f4:
        .ascii  "%d,%s,%s,%s\000"
.Lsec_163_100:
        .ascii  "phy_version: %d,"
        .ascii  " %s, %s, %s\012\000\000\000\000"
.Lsec_163_120:
        .ascii  "%d, %d, \000\000\000\000"
.Lsec_163_12c:
        .ascii  "cal_rf_ana_gain,"
        .ascii  " rf_gain=0x%x, a"
        .ascii  "na_gain=0x%x\012\000\000\000"
.Lsec_163_15c:
        .ascii  "TX_POWER_BACKOFF"
        .ascii  ", \000\000"
.Lsec_163_170:
        .ascii  "%d, \000\000\000\000"
.Lsec_163_178:
        .ascii  "\012\000\000\000"
.Lsec_163_17c:
        .ascii  "TX_PWRCTRL_ATTEN"
        .ascii  ", \000\000"
.Lsec_163_190:
        .ascii  "BT_PA_GAIN, \000\000\000\000"
.Lsec_163_1a0:
        .ascii  "0x%x, \000\000"
.Lsec_163_1a8:
        .ascii  "BT_DIG_ATTEN, \000\000"
.Lsec_163_1b8:
        .ascii  "BT_TX_BB, 0x%x\012\000"
.Lsec_163_1c8:
        .ascii  "bt_txpwr_atten, "
        .ascii  "%d\012\000"
.Lsec_163_1dc:
        .ascii  "total_power=%lld"
        .ascii  ", sig_power=%lld"
        .ascii  ", dc_power=%lld,"
        .ascii  " noise_power=%ll"
        .ascii  "d\012\000\000"
.Lsec_163_220:
        .ascii  "step=%d, %d\012\000\000\000\000"
.Lsec_163_230:
        .ascii  "%d-%d,\000\000"
.Lsec_163_238:
        .ascii  "rfrx:0x%x;\000\000"
.Lsec_163_244:
        .ascii  "rftx:0x%x,0x%x;\000"
.Lsec_163_254:
        .ascii  "bb:0x%x,0x%x;\000\000\000"
.Lsec_163_264:
        .ascii  "dco:%d,%d,%d,%d\012"
        .ascii  "\000\000\000\000"
.Lsec_163_278:
        .ascii  "RTC 0x%x: 0x%x\012\000"
.Lsec_163_288:
        .ascii  "SAR 0x%x: 0x%x\012\000"
.Lsec_163_298:
        .ascii  "APB 0x%x: 0x%x\012\000"
.Lsec_163_2a8:
        .ascii  "DPORT 0x%x: 0x%x"
        .ascii  "\012\000\000\000"
.Lsec_163_2bc:
        .ascii  "fe 0x%x: 0x%x\012\000\000"
.Lsec_163_2cc:
        .ascii  "fe2 0x%x: 0x%x\012\000"
.Lsec_163_2dc:
        .ascii  "agc 0x%x: 0x%x\012\000"
.Lsec_163_2ec:
        .ascii  "bb 0x%x: 0x%x\012\000\000"
.Lsec_163_2fc:
        .ascii  "bbtx 0x%x: 0x%x\012"
        .ascii  "\000\000\000\000"
.Lsec_163_310:
        .ascii  "brx 0x%x: 0x%x\012\000"
.Lsec_163_320:
        .ascii  "nrx 0x%x: 0x%x\012\000"
.Lsec_163_330:
        .ascii  "bt_reg 0x%x: 0x%"
        .ascii  "x\012\000\000"
.Lsec_163_344:
        .ascii  "i2c_mst 0x%x: 0x"
        .ascii  "%x\012\000"
.Lsec_163_358:
        .ascii  "i2c_bias %02d: 0"
        .ascii  "x%x\012\000\000\000\000"
.Lsec_163_370:
        .ascii  "i2c_bbtop %02d: "
        .ascii  "0x%x\012\000\000\000"
.Lsec_163_388:
        .ascii  "i2c_bbpll %02d: "
        .ascii  "0x%x\012\000\000\000"
.Lsec_163_3a0:
        .ascii  "i2c_txrf %02d: 0"
        .ascii  "x%x\012\000\000\000\000"
.Lsec_163_3b8:
        .ascii  "i2c_rfrx %02d: 0"
        .ascii  "x%x\012\000\000\000\000"
.Lsec_163_3d0:
        .ascii  "i2c_rfpll %02d: "
        .ascii  "0x%x\012\000\000\000"
.Lsec_163_3e8:
        .ascii  "i2c_rfpll_sdm %0"
        .ascii  "2d: 0x%x\012\000\000\000"
.Lsec_163_404:
        .ascii  "i2c_ckgen %02d: "
        .ascii  "0x%x\012\000\000\000"
.Lsec_163_41c:
        .ascii  "i2c_xtal %02d: 0"
        .ascii  "x%x\012\000\000\000\000"
.Lsec_163_434:
        .ascii  "i2c_plla %02d: 0"
        .ascii  "x%x\012\000\000\000\000"
.Lsec_163_44c:
        .ascii  "CHIP_ID: 0x%x, 0"
        .ascii  "x%x\012\012\000\000\000"
.Lsec_163_464:
        .ascii  "param_flag: 0x%x"
        .ascii  "\012\000\000\000"
.Lsec_163_478:
        .ascii  "vdd33=%d, temp_c"
        .ascii  "ode=%d, offset=%"
        .ascii  "d\012\000\000"
.Lsec_163_49c:
        .ascii  "rc_dout, %d; wif"
        .ascii  "i: %d, %d, %d, %"
        .ascii  "d, bt: %d, %d\012\000\000"
.Lsec_163_4cc:
        .ascii  "RX_NOISEFLOOR, %"
        .ascii  "d\012\000\000"
.Lsec_163_4e0:
        .ascii  "TXCAP_TMX2G_CCT_"
        .ascii  "LOAD, \000\000"
.Lsec_163_4f8:
        .ascii  "TXCAP_PA2G_CCT_S"
        .ascii  "TG1, \000\000\000"
.Lsec_163_510:
        .ascii  "TXCAP_PA2G_CCT_S"
        .ascii  "TG2, \000\000\000"
.Lsec_163_528:
        .ascii  "TXIQ,\000\000\000"
.Lsec_163_530:
        .ascii  "%d, %d; \000\000\000\000"
.Lsec_163_53c:
        .ascii  "TXDC, \000\000"
.Lsec_163_544:
        .ascii  "%d, %d, %d, %d; "
        .ascii  "\000\000\000\000"
.Lsec_163_558:
        .ascii  "delta_bt_atten, "
        .ascii  "\000\000\000\000"
.Lsec_163_56c:
        .ascii  "BT_TXIQ,\000\000\000\000"
.Lsec_163_578:
        .ascii  "BT_TXDC, \000\000\000"
.Lsec_163_584:
        .ascii  "RXIQ, \000\000"
.Lsec_163_58c:
        .ascii  "RXDC_RFRX_BT, \000\000"
.Lsec_163_59c:
        .ascii  "RXDC_RFRX_WIFI, "
        .ascii  "\000\000\000\000"
.Lsec_163_5b0:
        .ascii  "RXDC_RXBB_WIFI, "
        .ascii  "\000\000\000\000"
.Lsec_163_5c4:
        .ascii  "TX_VDD33=%d\012\000"

        .section .phyiram.18,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_164_0, 0x3ff4e000
        .literal .Lsec_164_4, 0x60033c00
        .literal .Lsec_164_8, .Lsec_163_0
        .literal .Lsec_164_c, 0x02000000
        .literal .Lsec_164_10, phy_printf
        .literal .Lsec_164_14, esp_dport_access_reg_read
        .literal .Lsec_164_18, esp_dport_access_reg_read
.Lsec_164_1c:
        .global i2c_master_reset
        .type   i2c_master_reset, @function
i2c_master_reset:
        entry    a1, 32
        movi.n   a6, 0
        l32r     a2, .Lsec_164_0
        mov.n    a7, a6
        l32r     a4, .Lsec_164_4
        l32r     a3, .Lsec_164_c
.Lsec_164_2c:
        memw
        l32i.n   a5, a4, 0
        j        .Lsec_164_54
.Lsec_164_34:
        memw
        l32i     a8, a4, 0
        sub      a8, a8, a5
        bltui    a8, 3, .Lsec_164_54
        l32r     a10, .Lsec_164_8
        or       a11, a6, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_164_10
        callx8   a8
        memw
        s32i.n   a7, a2, 0
        j        .Lsec_164_68
.Lsec_164_54:
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_164_14
        callx8   a8
        bany     a10, a3, .Lsec_164_34
.Lsec_164_5f:
        addi.n   a6, a6, 1
        addi.n   a2, a2, 4
        bnei     a6, 5, .Lsec_164_2c
        retw.n
.Lsec_164_68:
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_164_18
        callx8   a8
        bany     a10, a3, .Lsec_164_68
        j        .Lsec_164_5f
        .size   i2c_master_reset, .-i2c_master_reset
.Lsec_164_76:

        .section .text.ram_gen_rx_gain_table,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_165_0, .Lsec_137_64
        .literal .Lsec_165_4, 0x2aaaaaab
        .literal .Lsec_165_8, .Lsec_163_18
        .literal .Lsec_165_c, .Lsec_163_48
        .literal .Lsec_165_10, memcpy
        .literal .Lsec_165_14, phy_printf
        .literal .Lsec_165_18, phy_printf
.Lsec_165_1c:
        .global ram_gen_rx_gain_table
        .type   ram_gen_rx_gain_table, @function
ram_gen_rx_gain_table:
        entry    a1, 112
        l8ui     a9, a1, 112
        extui    a8, a7, 0, 8
        movi.n   a12, 10
        addi     a10, a1, 16
        extui    a6, a6, 0, 8
        l32r     a11, .Lsec_165_0
        movi.n   a7, 0
        extui    a3, a3, 0, 8
        s32i.n   a8, a1, 56
        s32i.n   a2, a1, 36
        s32i.n   a4, a1, 44
        mov.n    a4, a7
        s32i.n   a9, a1, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_165_10
        callx8   a8
        l32i.n   a8, a1, 56
        l32r     a9, .Lsec_165_4
        mulsh    a2, a8, a9
        s32i.n   a6, a1, 48
        addx2    a6, a2, a2
        slli     a6, a6, 1
        sub      a6, a8, a6
        slli     a2, a2, 1
        l32i.n   a9, a1, 48
        addi.n   a8, a9, -1
        s32i.n   a6, a1, 60
        extui    a6, a6, 0, 16
        s32i.n   a3, a1, 40
        mov.n    a3, a7
        s32i     a6, a1, 64
        mov.n    a6, a5
        s32i.n   a5, a1, 52
        mov.n    a5, a8
        s32i     a2, a1, 68
.Lsec_165_74:
        extui    a2, a7, 0, 8
        nop.n
        nop.n
        nop.n
        l8ui     a9, a6, 0
        sext     a9, a9, 7
        bne      a4, a9, .Lsec_165_89
        blt      a3, a5, .Lsec_165_a2
.Lsec_165_89:
        l32r     a8, .Lsec_165_4
        mulsh    a9, a4, a8
        addx2    a15, a9, a9
        slli     a15, a15, 1
        sub      a15, a4, a15
        slli     a9, a9, 1
        extui    a12, a15, 0, 16
        j        .Lsec_165_da
        .byte    0x00
.Lsec_165_a2:
        addi.n   a3, a3, 1
        l32i.n   a9, a1, 52
        extui    a3, a3, 0, 8
        add.n    a6, a9, a3
        l8ui     a4, a6, 0
        bnez.n   a4, .Lsec_165_cc
        blt      a3, a5, .Lsec_165_b6
        j        .Lsec_165_16c
.Lsec_165_b6:
        mov.n    a8, a9
.Lsec_165_b8:
        addi.n   a3, a3, 1
        extui    a3, a3, 0, 8
        add.n    a6, a8, a3
        l8ui     a4, a6, 0
        bnez.n   a4, .Lsec_165_cc
        blt      a3, a5, .Lsec_165_b8
        j        .Lsec_165_16c
        .byte    0x00
        .byte    0x00
.Lsec_165_cc:
        blt      a3, a5, .Lsec_165_d2
        j        .Lsec_165_16c
.Lsec_165_d2:
        movi.n   a12, 0
        mov.n    a15, a12
        mov.n    a4, a12
        mov.n    a9, a12
.Lsec_165_da:
        addi     a8, a1, 16
        add.n    a9, a8, a9
        l16ui    a14, a9, 0
        slli     a14, a14, 3
        extui    a14, a14, 0, 16
        add.n    a12, a14, a12
        l32i.n   a9, a1, 44
        add.n    a13, a9, a3
        l8ui     a9, a13, 0
        slli     a9, a9, 8
        add.n    a12, a9, a12
        sext     a9, a2, 7
        l32i.n   a8, a1, 36
        srai     a9, a9, 1
        extui    a12, a12, 0, 16
        addx4    a9, a9, a8
        bbci     a2, 0, .Lsec_165_114
        l32i.n   a11, a9, 0
        slli     a10, a12, 16
        add.n    a10, a11, a10
        s32i.n   a10, a9, 0
        j        .Lsec_165_116
        .byte    0x00
.Lsec_165_114:
        s32i.n   a12, a9, 0
.Lsec_165_116:
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a9, a1, 32
        beqz.n   a9, .Lsec_165_13e
        mov.n    a11, a7
        l8ui     a9, a6, 0
        sext     a9, a9, 7
        l32i.n   a8, a1, 48
        l8ui     a13, a13, 0
        l32r     a10, .Lsec_165_8
        s32i.n   a9, a1, 8
        s32i.n   a3, a1, 4
        s32i.n   a8, a1, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_165_14
        callx8   a8
.Lsec_165_13e:
        l32i.n   a9, a1, 40
        bgeu     a9, a4, .Lsec_165_156
        l32i     a4, a1, 32
        beqz.n   a4, .Lsec_165_179
        extui    a11, a7, 0, 8
        l32r     a10, .Lsec_165_c
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_165_18
        callx8   a8
        retw.n
.Lsec_165_156:
        addi.n   a4, a4, 1
        addi.n   a7, a7, 1
        movi     a2, 127
        extui    a4, a4, 0, 8
        beq      a7, a2, .Lsec_165_166
        j        .Lsec_165_74
.Lsec_165_166:
        movi.n   a2, 85
        retw.n
        .byte    0x00
        .byte    0x00
.Lsec_165_16c:
        l32i     a12, a1, 64
        l32i.n   a15, a1, 60
        l32i.n   a4, a1, 56
        l32i     a9, a1, 68
        j        .Lsec_165_da
.Lsec_165_179:
        retw.n
        .size   ram_gen_rx_gain_table, .-ram_gen_rx_gain_table
.Lsec_165_17b:

        .section .text.set_rx_gain_cal_iq,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_166_0, .Lsec_137_70
        .literal .Lsec_166_4, .Lsec_137_78
        .literal .Lsec_166_8, g_phyFuns
        .literal .Lsec_166_c, 0x3ff450dc
        .literal .Lsec_166_10, 0x08000000
        .literal .Lsec_166_14, 0xefffffff
        .literal .Lsec_166_18, 0xffffefff
        .literal .Lsec_166_1c, chip7_sleep_params
        .literal .Lsec_166_20, 0x00000fa0
        .literal .Lsec_166_24, 0x3ff460e4
        .literal .Lsec_166_28, 0x3ff460dc
        .literal .Lsec_166_2c, 0x3ff460e0
        .literal .Lsec_166_30, 0x00020000
        .literal .Lsec_166_34, 0x00001000
        .literal .Lsec_166_38, .Lsec_163_58
        .literal .Lsec_166_3c, 0x00000fff
        .literal .Lsec_166_40, .Lsec_163_a0
        .literal .Lsec_166_44, 0x10000000
        .literal .Lsec_166_48, memcpy
        .literal .Lsec_166_4c, memcpy
        .literal .Lsec_166_50, esp_dport_access_reg_read
        .literal .Lsec_166_54, esp_dport_access_reg_read
        .literal .Lsec_166_58, esp_dport_access_reg_read
        .literal .Lsec_166_5c, esp_dport_access_reg_read
        .literal .Lsec_166_60, esp_dport_access_reg_read
        .literal .Lsec_166_64, esp_dport_access_reg_read
        .literal .Lsec_166_68, phy_printf
        .literal .Lsec_166_6c, phy_printf
        .literal .Lsec_166_70, esp_dport_access_reg_read
        .literal .Lsec_166_74, esp_dport_access_reg_read
.Lsec_166_78:
        .global set_rx_gain_cal_iq
        .type   set_rx_gain_cal_iq, @function
set_rx_gain_cal_iq:
        entry    a1, 128
        l32r     a11, .Lsec_166_0
        movi     a12, 5
        addi     a10, a1, 34
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_166_48
        callx8   a8
        movi.n   a12, 10
        l32r     a11, .Lsec_166_4
        addi     a10, a1, 16
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_166_4c
        callx8   a8
        extui    a2, a2, 0, 8
        movi.n   a10, 1
        l32r     a6, .Lsec_166_8
        sext     a3, a3, 15
        extui    a5, a5, 0, 8
        s32i     a2, a1, 76
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a2, a6, 0
        l32i.n   a2, a2, 40
        s32i     a3, a1, 68
        s32i.n   a5, a1, 60
        callx8   a2
        movi.n   a14, 7
        movi.n   a15, 1
        mov.n    a13, a14
        l32i     a2, a6, 0
        movi.n   a12, 4
        movi     a11, 0
        movi     a10, 100
        l32i     a2, a2, 168
        callx8   a2
        l32r     a10, .Lsec_166_c
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_166_50
        callx8   a8
        l32r     a2, .Lsec_166_10
        or       a2, a10, a2
        l32r     a10, .Lsec_166_c
        or       a3, a10, a10
        memw
        s32i     a2, a10, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_166_54
        callx8   a8
        l32r     a2, .Lsec_166_14
        and      a2, a10, a2
        mov.n    a10, a3
        memw
        s32i     a2, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_166_58
        callx8   a8
        l32r     a2, .Lsec_166_18
        and      a3, a10, a2
        l32r     a2, .Lsec_166_1c
        l16ui    a2, a2, 76
        extui    a5, a2, 6, 5
        sext     a10, a5, 7
        l32r     a8, .Lsec_166_c
        memw
        s32i.n   a3, a8, 0
        movi     a3, 15
        bge      a3, a10, .Lsec_166_12c
        addi     a10, a5, -32
.Lsec_166_12c:
        extui    a2, a2, 0, 6
        sext     a3, a2, 7
        movi.n   a5, 31
        bge      a5, a3, .Lsec_166_13a
        addi     a3, a2, -64
.Lsec_166_13a:
        l32i.n   a2, a6, 0
        addi     a9, a1, 33
        movi.n   a11, 1
        l32i     a2, a2, 0x128
        s32i     a9, a1, 56
        callx8   a2
        mov.n    a10, a3
        movi.n   a11, 0
        movi.n   a3, 0
        l32i.n   a2, a6, 0
        l32i     a2, a2, 0x128
        callx8   a2
        movi     a2, 0x100
        s32i.n   a3, a1, 48
        s32i     a4, a1, 64
        s16i     a2, a1, 26
        s16i     a2, a1, 28
        memw
.Lsec_166_169:
        addi     a5, a1, 16
        movi     a8, 128
        movi     a11, 0x104
        nop.n
        l32i.n   a4, a1, 48
        addx2    a2, a4, a5
        l16ui    a2, a2, 0
        add.n    a2, a2, a8
        extui    a2, a2, 0, 16
        mov.n    a12, a2
        l32i.n   a9, a1, 56
        s32i.n   a2, a1, 52
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a2, a6, 0
        l8ui     a4, a9, 1
        or       a10, a4, a4
        extui    a4, a4, 0, 16
        l32i     a2, a2, 32
        callx8   a2
        l32i     a2, a1, 76
        beqz.n   a2, .Lsec_166_1c4
        movi.n   a11, 1
        mov.n    a10, a11
        l32i     a2, a6, 0
        l32i     a3, a2, 188
        l32i     a2, a2, 184
        callx8   a3
        movi.n   a3, 2
        or       a10, a10, a3
        movi.n   a11, 1
        extui    a12, a10, 0, 16
        mov.n    a10, a11
        callx8   a2
.Lsec_166_1c4:
        l32i.n   a2, a6, 0
        movi.n   a14, 0
        mov.n    a13, a14
        movi.n   a12, 10
        addi     a11, a1, 26
        l32i     a2, a2, 0x150
        movi.n   a3, 0
        l32r     a10, .Lsec_166_20
        callx8   a2
        mov.n    a9, a3
        movi.n   a2, 24
        l32i     a8, a1, 48
        extui    a5, a8, 0, 8
        movi.n   a8, 1
        movnez   a9, a8, a5
        s32i     a9, a1, 72
.Lsec_166_1ec:
        movi.n   a11, 1
        mov.n    a10, a11
        movi     a12, 0x1f1
        extui    a5, a2, 0, 8
        l32i.n   a7, a6, 0
        l32i     a7, a7, 184
        callx8   a7
        movi.n   a11, 1
        mov.n    a10, a11
        l32i.n   a7, a6, 0
        movi     a12, 0x1f9
        l32i     a7, a7, 184
        callx8   a7
        movi.n   a15, 0
        mov.n    a14, a15
        l32i.n   a7, a6, 0
        or       a13, a15, a15
        or       a12, a5, a5
        movi.n   a10, 1
        l32i     a11, a1, 68
        l32i     a7, a7, 68
        callx8   a7
        movi     a11, 0x3ff
        movi.n   a10, 1
        l32i.n   a7, a6, 0
        l32i     a7, a7, 0x144
        callx8   a7
        l32r     a10, .Lsec_166_24
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_166_5c
        callx8   a8
        srai     a7, a10, 7
        l32i.n   a9, a1, 60
        beqz.n   a9, .Lsec_166_282
        l32r     a10, .Lsec_166_28
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_166_60
        callx8   a8
        mov.n    a8, a10
        l32r     a10, .Lsec_166_2c
        s32i     a8, a1, 80
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_166_64
        callx8   a8
        srai     a10, a10, 16
        mov.n    a15, a2
        mov.n    a14, a4
        l32i     a8, a1, 80
        mov.n    a11, a7
        srai     a8, a8, 16
        s32i.n   a10, a1, 4
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a13, .Lsec_166_30
        l32r     a12, .Lsec_166_34
        l32r     a10, .Lsec_166_38
        s32i.n   a8, a1, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_166_68
        callx8   a8
.Lsec_166_282:
        l32i.n   a8, a6, 0
        l32i     a8, a8, 0x148
        callx8   a8
        movi.n   a10, 1
        l32i.n   a8, a6, 0
        l32i     a8, a8, 76
        callx8   a8
        l32r     a8, .Lsec_166_30
        bge      a8, a7, .Lsec_166_2d0
        bbsi     a3, 0, .Lsec_166_2b2
        beqz.n   a4, .Lsec_166_2b2
        movi     a5, 120
        min      a5, a2, a5
        l32i.n   a9, a1, 56
        sext     a5, a5, 15
        mov.n    a2, a5
        l8ui     a4, a9, 2
        j        .Lsec_166_3aa
.Lsec_166_2b2:
        l32i.n   a5, a6, 0
        addi     a2, a2, 24
        mov.n    a12, a4
        movi.n   a11, 1
        movi.n   a10, 5
        l32i     a5, a5, 184
        sext     a7, a2, 15
        movi.n   a2, 0
        callx8   a5
        mov.n    a5, a2
        blt      a7, a2, .Lsec_166_2fe
        j        .Lsec_166_2f6
.Lsec_166_2d0:
        l32r     a8, .Lsec_166_3c
        blt      a8, a7, .Lsec_166_309
        bbci     a3, 0, .Lsec_166_2dc
        j        .Lsec_166_3a6
.Lsec_166_2dc:
        l32i     a5, a1, 72
        beqz     a5, .Lsec_166_3a6
        movi     a5, 120
        min      a5, a2, a5
        l32i.n   a8, a1, 56
        sext     a5, a5, 15
        mov.n    a2, a5
        l8ui     a4, a8, 0
        j        .Lsec_166_3aa
        .byte    0x00
.Lsec_166_2f6:
        movi     a2, 120
        min      a2, a7, a2
        mov.n    a5, a2
.Lsec_166_2fe:
        addi.n   a3, a3, 1
        extui    a3, a3, 0, 8
        beqi     a3, 2, .Lsec_166_309
        j        .Lsec_166_1ec
.Lsec_166_309:
        l32i.n   a9, a1, 60
        beqz.n   a9, .Lsec_166_326
        mov.n    a13, a2
        movi.n   a2, 2
        l32i.n   a15, a1, 48
        movi     a12, 0x104
        mov.n    a11, a4
        l32i.n   a14, a1, 52
        l32r     a10, .Lsec_166_40
        s32i     a2, a1, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_166_6c
        callx8   a8
.Lsec_166_326:
        mov.n    a11, a5
        l32i.n   a2, a6, 0
        l32i.n   a3, a1, 48
        addi.n   a3, a3, 1
        l32i.n   a4, a1, 56
        addi.n   a4, a4, 1
        l32i.n   a12, a1, 60
        l32i     a10, a1, 68
        l32i     a2, a2, 0x164
        s32i.n   a3, a1, 48
        s32i.n   a4, a1, 56
        callx8   a2
        l32i     a5, a1, 64
        s16i     a10, a5, 0
        addi.n   a5, a5, 2
        s32i     a5, a1, 64
        beqi     a3, 4, .Lsec_166_352
        j        .Lsec_166_169
.Lsec_166_352:
        movi.n   a10, 0
        nop.n
        nop.n
        nop.n
        l32i.n   a2, a6, 0
        l32i     a2, a2, 40
        callx8   a2
        movi.n   a15, 0
        movi.n   a14, 7
        l32i.n   a2, a6, 0
        mov.n    a13, a14
        mov.n    a11, a15
        movi     a10, 100
        l32i     a12, a1, 48
        l32i     a2, a2, 168
        callx8   a2
        l32r     a10, .Lsec_166_c
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_166_70
        callx8   a8
        l32r     a2, .Lsec_166_44
        or       a2, a10, a2
        l32r     a10, .Lsec_166_c
        memw
        s32i.n   a2, a10, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_166_74
        callx8   a8
        l32r     a9, .Lsec_166_34
        or       a10, a10, a9
        l32r     a2, .Lsec_166_c
        memw
        s32i.n   a10, a2, 0
        retw.n
        .byte    0x00
.Lsec_166_3a6:
        movi.n   a5, 0
        mov.n    a2, a5
.Lsec_166_3aa:
        l32i.n   a7, a6, 0
        mov.n    a12, a4
        movi.n   a11, 1
        movi.n   a10, 5
        l32i     a7, a7, 184
        callx8   a7
        j        .Lsec_166_2fe
        .size   set_rx_gain_cal_iq, .-set_rx_gain_cal_iq
.Lsec_166_3bb:

        .section .text.rx_chan_dc_sort,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_167_0, g_phyFuns
.Lsec_167_4:
        .global rx_chan_dc_sort
        .type   rx_chan_dc_sort, @function
rx_chan_dc_sort:
        entry    a1, 64
        s32i.n   a2, a1, 12
        mov.n    a4, a3
        movi.n   a8, 0
        movi.n   a2, 14
        s32i.n   a3, a1, 4
        loop     a2, .Lsec_167_2b
        nop.n
        nop.n
        nop.n
        nop.n
        l8ui     a3, a4, 0
        addi.n   a4, a4, 1
        bnei     a3, 1, .Lsec_167_29
        addi.n   a8, a8, 1
        extui    a8, a8, 0, 8
.Lsec_167_29:
        nop.n
.Lsec_167_2b:
        movi.n   a2, 1
        movi.n   a5, 2
        l32i.n   a14, a1, 12
        movnez   a5, a2, a8
        movi.n   a7, 0
        mov.n    a12, a7
        movi.n   a3, 14
        mov.n    a7, a5
        mov.n    a5, a14
.Lsec_167_3e:
        l32i.n   a4, a1, 4
        extui    a9, a12, 0, 8
        add.n    a2, a4, a12
        l8ui     a2, a2, 0
        s32i.n   a9, a1, 0
        beqi     a2, 1, .Lsec_167_ad
        movi.n   a6, 20
        movi.n   a2, 0
        s32i.n   a5, a1, 8
        mov.n    a5, a6
.Lsec_167_55:
        extui    a6, a2, 0, 8
        nop.n
        nop
        l8ui     a8, a4, 0
        bne      a8, a7, .Lsec_167_86
        sub      a10, a12, a2
        l32r     a9, .Lsec_167_0
        l32i     a8, a9, 0
        l32i     a8, a8, 0x140
        s32i.n   a12, a1, 16
        callx8   a8
        extui    a10, a10, 0, 8
        sext     a8, a5, 7
        sext     a9, a10, 7
        l32i.n   a12, a1, 16
        bge      a9, a8, .Lsec_167_86
        mov.n    a5, a10
        s32i.n   a6, a1, 0
.Lsec_167_86:
        addi.n   a2, a2, 1
        addi.n   a4, a4, 1
        bne      a2, a3, .Lsec_167_55
        nop.n
        nop.n
        l32i.n   a2, a1, 0
        sext     a6, a2, 7
        l32i.n   a4, a1, 12
        addx4    a6, a6, a4
        l32i.n   a2, a6, 0
        srai     a4, a2, 16
        slli     a4, a4, 16
        extui    a2, a2, 0, 9
        l32i.n   a5, a1, 8
        or       a2, a4, a2
        s32i.n   a2, a5, 0
.Lsec_167_ad:
        addi.n   a12, a12, 1
        addi.n   a5, a5, 4
        bne      a12, a3, .Lsec_167_3e
        retw.n
        .size   rx_chan_dc_sort, .-rx_chan_dc_sort
.Lsec_167_b6:

        .section .text.set_rx_gain_cal_dc,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_168_0, .Lsec_137_84
        .literal .Lsec_168_4, .Lsec_137_64
        .literal .Lsec_168_8, g_phyFuns
        .literal .Lsec_168_c, 0x00000800
        .literal .Lsec_168_10, memcpy
        .literal .Lsec_168_14, chip_v7_set_chan_ana
        .literal .Lsec_168_18, chip_v7_set_chan_ana
        .literal .Lsec_168_1c, get_rfrx_dcap_bt
        .literal .Lsec_168_20, get_rfrx_dcap_bt
        .literal .Lsec_168_24, pbus_rx_dco_cal_1step
        .literal .Lsec_168_28, pbus_rx_dco_cal_1step
        .literal .Lsec_168_2c, pbus_rx_dco_cal_1step
        .literal .Lsec_168_30, rx_chan_dc_sort
        .literal .Lsec_168_34, chip_v7_set_chan_ana
.Lsec_168_38:
        .global set_rx_gain_cal_dc
        .type   set_rx_gain_cal_dc, @function
set_rx_gain_cal_dc:
        entry    a1, 144
        s32i     a7, a1, 76
        extui    a2, a2, 0, 8
        movi.n   a12, 10
        extui    a4, a4, 0, 8
        addi     a10, a1, 28
        l32r     a7, .Lsec_168_0
        l16ui    a8, a7, 0
        l32r     a11, .Lsec_168_4
        s32i     a6, a1, 96
        extui    a6, a3, 0, 8
        s32i.n   a2, a1, 48
        nop.n
        nop.n
        nop.n
        nop.n
        l16ui    a3, a7, 2
        l8ui     a2, a1, 148
        s16i     a8, a1, 38
        s32i     a2, a1, 100
        s32i     a5, a1, 72
        s32i     a4, a1, 88
        s16i     a3, a1, 40
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_168_10
        callx8   a8
        movi.n   a10, 1
        l32r     a7, .Lsec_168_8
        l32i.n   a2, a7, 0
        l32i     a2, a2, 64
        callx8   a2
        movi.n   a10, 1
        l32i.n   a2, a7, 0
        l32i.n   a2, a2, 60
        callx8   a2
        movi.n   a10, 14
        .reloc  ., R_XTENSA_ASM_EXPAND, chip_v7_set_chan_ana
        l32r     a8, .Lsec_168_14
        callx8   a8
        bltu     a6, a4, .Lsec_168_a9
.Lsec_168_a1:
        l32i.n   a3, a1, 48
        bnez     a3, .Lsec_168_368
        j        .Lsec_168_347
.Lsec_168_a9:
        l32i.n   a4, a1, 48
        movi.n   a2, 0
        movi.n   a3, 9
        movnez   a3, a2, a4
        extui    a3, a3, 0, 8
        l32i     a5, a1, 72
        l32i     a8, a1, 100
        add.n    a2, a5, a8
        addi.n   a2, a2, -1
        s32i     a3, a1, 92
        s32i     a2, a1, 104
.Lsec_168_c5:
        beqi     a6, 1, .Lsec_168_d4
        beqz     a6, .Lsec_168_376
        beqi     a6, 2, .Lsec_168_d1
        j        .Lsec_168_39d
.Lsec_168_d1:
        j        .Lsec_168_3cc
.Lsec_168_d4:
        l32i     a5, a1, 96
        mov.n    a11, a6
        movi.n   a10, 2
        l32i.n   a2, a7, 0
        l16ui    a12, a5, 42
        l32i     a2, a2, 184
        callx8   a2
        mov.n    a11, a6
        movi.n   a10, 3
        l32i.n   a2, a7, 0
        l16ui    a12, a5, 40
        l32i     a2, a2, 184
        callx8   a2
        movi.n   a8, 4
        s32i     a8, a1, 64
.Lsec_168_fa:
        movi     a3, 0x100
        movi.n   a2, 1
        s32i     a2, a1, 68
        s16i     a3, a1, 38
        s16i     a3, a1, 40
        memw
        j        .Lsec_168_124
.Lsec_168_10e:
        l32i     a5, a1, 96
        movi.n   a8, 1
        l32i.n   a2, a5, 40
        extui    a3, a2, 16, 16
        s32i     a8, a1, 64
        s16i     a3, a1, 38
        s16i     a2, a1, 40
        memw
.Lsec_168_124:
        movi.n   a5, 1
        nop.n
        nop.n
        nop.n
        l32i     a2, a1, 64
        addi.n   a4, a2, -1
        extui    a4, a4, 0, 8
        addi.n   a4, a4, 1
        l32i     a8, a1, 72
        movi.n   a2, 0
        add.n    a4, a8, a4
        l32i     a3, a1, 92
        s32i     a5, a1, 56
        s32i     a3, a1, 80
        s32i     a2, a1, 84
        s32i     a4, a1, 52
.Lsec_168_14c:
        beqi     a6, 2, .Lsec_168_161
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a3, a1, 56
        extui    a3, a3, 0, 8
        s32i.n   a3, a1, 60
        j        .Lsec_168_170
.Lsec_168_161:
        l32i.n   a5, a1, 56
        extui    a4, a5, 0, 8
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, chip_v7_set_chan_ana
        l32r     a8, .Lsec_168_18
        callx8   a8
        s32i.n   a4, a1, 60
.Lsec_168_170:
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a8, a1, 48
        beqz.n   a8, .Lsec_168_1b0
        movi.n   a10, 4
        l32i.n   a4, a7, 0
        l32i     a4, a4, 160
        .reloc  ., R_XTENSA_ASM_EXPAND, get_rfrx_dcap_bt
        l32r     a8, .Lsec_168_1c
        callx8   a8
        mov.n    a13, a10
        movi.n   a12, 4
        movi.n   a11, 0
        movi     a10, 100
        callx8   a4
        l32i.n   a4, a7, 0
        movi.n   a10, 7
        l32i     a4, a4, 160
        .reloc  ., R_XTENSA_ASM_EXPAND, get_rfrx_dcap_bt
        l32r     a8, .Lsec_168_20
        callx8   a8
        mov.n    a13, a10
        movi     a12, 7
        movi     a11, 0
        movi     a10, 100
        callx8   a4
.Lsec_168_1b0:
        l32i     a3, a1, 64
        beqz     a3, .Lsec_168_31e
        addi     a4, a1, 28
        l32i     a5, a1, 72
        mov.n    a3, a5
        mov.n    a5, a4
        mov.n    a4, a3
.Lsec_168_1c2:
        movi.n   a11, 1
        movi     a12, 0x184
        movi.n   a10, 0
        bne      a6, a11, .Lsec_168_236
        l32i     a8, a1, 72
        or       a11, a6, a6
        l8ui     a15, a8, 1
        slli     a15, a15, 8
        l16ui    a13, a5, 0
        slli     a13, a13, 3
        or       a13, a15, a13
        extui    a3, a13, 0, 16
        l32i.n   a14, a7, 0
        l32i     a14, a14, 184
        callx8   a14
        mov.n    a10, a3
        l32i.n   a11, a7, 0
        l32i     a11, a11, 200
        callx8   a11
        mov.n    a11, a6
        add.n    a15, a1, a2
        l32r     a12, .Lsec_168_c
        addi     a14, a1, 16
        addi     a13, a1, 38
        l32i     a10, a1, 48
        .reloc  ., R_XTENSA_ASM_EXPAND, pbus_rx_dco_cal_1step
        l32r     a8, .Lsec_168_24
        callx8   a8
        l16si    a10, a1, 38
        slli     a11, a10, 16
        l32i     a3, a1, 84
        addi.n   a10, a3, 1
        extui    a10, a10, 0, 8
        l32i     a8, a1, 76
        addx4    a12, a3, a8
        s32i     a10, a1, 84
        nop.n
        nop.n
        nop.n
        nop.n
        l16si    a10, a1, 40
        or       a10, a11, a10
        s32i.n   a10, a12, 0
        j        .Lsec_168_30e
.Lsec_168_236:
        bnei     a6, 2, .Lsec_168_29c
        l32i.n   a13, a7, 0
        movi     a12, 0x184
        movi.n   a11, 1
        movi     a10, 0
        l32i     a3, a1, 104
        l32i     a14, a13, 184
        l8ui     a13, a3, 0
        slli     a3, a13, 8
        callx8   a14
        mov.n    a10, a3
        l32i.n   a11, a7, 0
        l32i     a11, a11, 200
        callx8   a11
        mov.n    a11, a6
        mov.n    a10, a6
        l32i     a8, a1, 76
        l32i.n   a13, a7, 0
        l16ui    a12, a8, 6
        l32i     a13, a13, 184
        callx8   a13
        mov.n    a11, a6
        movi.n   a10, 3
        l32i     a8, a1, 76
        l32i.n   a13, a7, 0
        l32i.n   a12, a8, 4
        extui    a12, a12, 0, 9
        l32i     a13, a13, 184
        callx8   a13
        add.n    a15, a1, a2
        addi     a14, a1, 16
        l32r     a12, .Lsec_168_c
        addi     a13, a1, 38
        mov.n    a11, a6
        l32i     a10, a1, 48
        .reloc  ., R_XTENSA_ASM_EXPAND, pbus_rx_dco_cal_1step
        l32r     a8, .Lsec_168_28
        callx8   a8
        j        .Lsec_168_2f5
        .byte    0x00
.Lsec_168_29c:
        l32i.n   a14, a7, 0
        l8ui     a13, a4, 0
        slli     a3, a13, 8
        l32i     a14, a14, 184
        callx8   a14
        mov.n    a10, a3
        l32i.n   a11, a7, 0
        l32i     a11, a11, 200
        callx8   a11
        add.n    a15, a1, a2
        addi     a14, a1, 16
        l32r     a12, .Lsec_168_c
        addi     a13, a1, 38
        mov.n    a11, a6
        l32i.n   a10, a1, 48
        .reloc  ., R_XTENSA_ASM_EXPAND, pbus_rx_dco_cal_1step
        l32r     a8, .Lsec_168_2c
        callx8   a8
        bnez.n   a6, .Lsec_168_2f5
        l32i     a3, a1, 80
        l16si    a10, a1, 38
        slli     a11, a10, 16
        addi.n   a10, a3, 1
        extui    a10, a10, 0, 8
        l32i     a8, a1, 96
        addx4    a12, a3, a8
        s32i     a10, a1, 80
        nop.n
        nop.n
        nop.n
        nop.n
        l16si    a10, a1, 40
        or       a10, a11, a10
        s32i.n   a10, a12, 0
        j        .Lsec_168_30e
.Lsec_168_2f5:
        l16si    a10, a1, 38
        slli     a11, a10, 16
        l32i     a3, a1, 144
        addx4    a12, a2, a3
        addi.n   a2, a2, 1
        extui    a2, a2, 0, 8
        l16si    a10, a1, 40
        or       a10, a11, a10
        s32i.n   a10, a12, 0
.Lsec_168_30e:
        addi.n   a4, a4, 1
        addi.n   a5, a5, 2
        nop.n
        nop.n
        l32i.n   a8, a1, 52
        beq      a8, a4, .Lsec_168_31e
        j        .Lsec_168_1c2
.Lsec_168_31e:
        l32i.n   a3, a1, 56
        addi.n   a3, a3, 1
        l32i.n   a4, a1, 60
        l32i     a5, a1, 68
        s32i.n   a3, a1, 56
        bgeu     a4, a5, .Lsec_168_32f
        j        .Lsec_168_14c
.Lsec_168_32f:
        addi.n   a6, a6, 1
        nop.n
        nop.n
        nop.n
        l32i     a8, a1, 88
        extui    a6, a6, 0, 8
        bgeu     a6, a8, .Lsec_168_343
        j        .Lsec_168_c5
.Lsec_168_343:
        j        .Lsec_168_a1
        .byte    0x00
.Lsec_168_347:
        l32i     a10, a1, 144
        mov.n    a11, a1
        .reloc  ., R_XTENSA_ASM_EXPAND, rx_chan_dc_sort
        l32r     a8, .Lsec_168_30
        callx8   a8
.Lsec_168_352:
        movi.n   a10, 0
        l32i.n   a2, a7, 0
        l32i     a2, a2, 64
        callx8   a2
        movi.n   a10, 0
        l32i.n   a2, a7, 0
        l32i.n   a2, a2, 60
        callx8   a2
        retw.n
        .byte    0x00
.Lsec_168_368:
        movi.n   a10, 14
        .reloc  ., R_XTENSA_ASM_EXPAND, chip_v7_set_chan_ana
        l32r     a8, .Lsec_168_34
        callx8   a8
        j        .Lsec_168_352
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_168_376:
        l32i.n   a2, a7, 0
        movi.n   a11, 2
        mov.n    a10, a11
        movi     a12, 0x100
        l32i     a2, a2, 184
        callx8   a2
        movi     a12, 0x100
        movi.n   a11, 2
        l32i.n   a2, a7, 0
        movi.n   a10, 3
        l32i     a2, a2, 184
        callx8   a2
        l32i     a2, a1, 100
        s32i     a2, a1, 64
        j        .Lsec_168_fa
.Lsec_168_39d:
        l32i     a8, a1, 76
        movi.n   a11, 2
        mov.n    a10, a11
        l32i     a2, a7, 0
        l16ui    a12, a8, 6
        l32i     a2, a2, 184
        callx8   a2
        movi.n   a11, 2
        movi.n   a10, 3
        l32i     a8, a1, 76
        l32i.n   a2, a7, 0
        l32i.n   a12, a8, 4
        extui    a12, a12, 0, 9
        l32i     a2, a2, 184
        callx8   a2
        movi.n   a2, 1
        s32i     a2, a1, 68
        j        .Lsec_168_10e
.Lsec_168_3cc:
        l32i     a8, a1, 76
        mov.n    a11, a6
        mov.n    a10, a6
        l32i.n   a2, a7, 0
        l16ui    a12, a8, 6
        l32i     a2, a2, 184
        callx8   a2
        mov.n    a11, a6
        movi.n   a10, 3
        l32i     a8, a1, 76
        l32i.n   a2, a7, 0
        l32i.n   a12, a8, 4
        extui    a12, a12, 0, 9
        l32i     a2, a2, 184
        callx8   a2
        movi.n   a2, 14
        s32i     a2, a1, 68
        j        .Lsec_168_10e
        .size   set_rx_gain_cal_dc, .-set_rx_gain_cal_dc
.Lsec_168_3fa:

        .section .text.wr_rx_gain_mem,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_169_0, 0x00200000
        .literal .Lsec_169_4, .Lsec_137_64
        .literal .Lsec_169_8, wifi_bbgain_max
        .literal .Lsec_169_c, 0xaaaaaaab
        .literal .Lsec_169_10, chip7_sleep_params
        .literal .Lsec_169_14, g_phyFuns
        .literal .Lsec_169_18, memcpy
.Lsec_169_1c:
        .global wr_rx_gain_mem
        .type   wr_rx_gain_mem, @function
wr_rx_gain_mem:
        entry    a1, 96
        extui    a2, a2, 0, 8
        s32i.n   a2, a1, 16
        movi.n   a12, 10
        mov.n    a10, a1
        extui    a3, a3, 0, 8
        nop.n
        l8ui     a2, a1, 96
        l32i.n   a8, a1, 16
        s32i.n   a7, a1, 44
        movi.n   a7, 0
        s32i.n   a2, a1, 20
        movi     a2, 128
        moveqz   a2, a7, a8
        nop.n
        nop.n
        l32r     a11, .Lsec_169_4
        s32i.n   a6, a1, 40
        s32i.n   a2, a1, 28
        s32i.n   a4, a1, 32
        s32i     a5, a1, 36
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_169_18
        callx8   a8
        l32r     a2, .Lsec_169_8
        l32i.n   a9, a1, 20
        l8ui     a2, a2, 0
        sub      a2, a9, a2
        addi     a2, a2, -2
        extui    a2, a2, 0, 8
        mov.n    a6, a2
        s32i.n   a2, a1, 24
        moveqz   a6, a7, a3
        bltu     a6, a9, .Lsec_169_72
        j        .Lsec_169_1ed
.Lsec_169_72:
        movi.n   a4, 1
        nop.n
        nop.n
        l32i.n   a2, a1, 16
        moveqz   a4, a7, a2
        extui    a4, a4, 0, 8
        mov.n    a2, a7
        l32r     a5, .Lsec_169_14
        s32i.n   a4, a1, 48
        mov.n    a4, a7
.Lsec_169_89:
        beqz.n   a3, .Lsec_169_bd
        nop.n
        nop.n
        nop.n
        l32r     a8, .Lsec_169_c
        muluh    a7, a2, a8
        srli     a7, a7, 2
        addx2    a8, a7, a1
        addx2    a7, a7, a7
        l8ui     a8, a8, 0
        slli     a7, a7, 1
        sub      a7, a2, a7
        addx8    a7, a8, a7
        extui    a7, a7, 0, 8
        l32i.n   a9, a1, 32
        l8ui     a8, a9, 0
        slli     a8, a8, 8
        or       a8, a7, a8
        j        .Lsec_169_100
.Lsec_169_bd:
        srli     a2, a6, 1
        bbci     a6, 0, .Lsec_169_d0
        l32i     a8, a1, 100
        addx4    a2, a2, a8
        l16ui    a8, a2, 2
        j        .Lsec_169_d9
        .byte    0x00
.Lsec_169_d0:
        l32i     a9, a1, 100
        addx4    a2, a2, a9
        l16ui    a8, a2, 0
.Lsec_169_d9:
        movi.n   a10, 3
        movi     a2, 0
        movi     a9, 6
        loop     a9, .Lsec_169_f4
        ssr      a10
        sra      a7, a8
        extui    a7, a7, 0, 1
        add.n    a2, a2, a7
        extui    a2, a2, 0, 8
        addi.n   a10, a10, 1
.Lsec_169_f4:
        addx2    a2, a2, a2
        extui    a7, a8, 0, 3
        addx2    a2, a2, a7
        extui    a2, a2, 0, 8
.Lsec_169_100:
        l32i.n   a9, a1, 24
        bltu     a6, a9, .Lsec_169_109
        l32i.n   a9, a1, 48
        beqz.n   a9, .Lsec_169_13e
.Lsec_169_109:
        l32i.n   a9, a1, 32
        add.n    a7, a9, a4
        movi     a9, 0xffffff00
        and      a9, a8, a9
        l8ui     a7, a7, 0
        slli     a7, a7, 8
        beq      a9, a7, .Lsec_169_121
        addi.n   a4, a4, 1
        extui    a4, a4, 0, 8
.Lsec_169_121:
        l32i.n   a9, a1, 36
        movi.n   a14, 0
        addx4    a7, a4, a9
        movi     a15, 128
        l32i.n   a7, a7, 0
        extui    a9, a7, 16, 16
        extui    a7, a7, 0, 16
        l32i.n   a10, a1, 16
        l32r     a13, .Lsec_169_0
        bne      a10, a14, .Lsec_169_17d
        j        .Lsec_169_159
.Lsec_169_13e:
        l32r     a7, .Lsec_169_10
        l8ui     a7, a7, 81
        sext     a7, a7, 7
        addi.n   a7, a7, -1
        extui    a7, a7, 0, 16
        l32i.n   a9, a1, 44
        addx4    a7, a7, a9
        l32i.n   a7, a7, 0
        extui    a9, a7, 16, 16
        extui    a7, a7, 0, 16
.Lsec_169_159:
        l32r     a11, .Lsec_169_c
        muluh    a10, a2, a11
        srli     a11, a10, 2
        movi.n   a10, 3
        minu     a10, a11, a10
        l32i.n   a11, a1, 40
        addx4    a10, a10, a11
        l32i.n   a13, a10, 0
        extui    a14, a13, 16, 16
        extui    a13, a13, 0, 16
        srai     a15, a14, 1
        slli     a13, a13, 13
        slli     a14, a14, 31
.Lsec_169_17d:
        l32i.n   a10, a1, 16
        movi.n   a11, 0
        bne      a10, a11, .Lsec_169_1b8
        extui    a10, a8, 3, 5
        extui    a12, a10, 0, 16
        srli     a11, a12, 4
        extui    a10, a10, 0, 1
        add.n    a10, a11, a10
        extui    a11, a12, 3, 1
        add.n    a10, a10, a11
        extui    a11, a12, 2, 1
        add.n    a10, a10, a11
        extui    a12, a12, 1, 1
        add.n    a10, a10, a12
        movi.n   a11, 3
        extui    a10, a10, 0, 8
        minu     a10, a10, a11
        l32r     a11, .Lsec_169_10
        addx2    a10, a10, a11
        l16ui    a11, a10, 56
        extui    a11, a11, 0, 11
        slli     a11, a11, 2
.Lsec_169_1b8:
        slli     a7, a7, 22
        add.n    a14, a7, a14
        slli     a10, a9, 8
        l32i.n   a7, a5, 0
        slli     a8, a8, 17
        add.n    a13, a14, a13
        add.n    a10, a8, a10
        add.n    a11, a13, a11
        l32i.n   a9, a1, 28
        add.n    a10, a10, a15
        add.n    a12, a6, a9
        extui    a12, a12, 0, 8
        l32i.n   a7, a7, 52
        callx8   a7
        beqz.n   a3, .Lsec_169_1e0
        addi.n   a2, a2, 1
        extui    a2, a2, 0, 8
.Lsec_169_1e0:
        addi.n   a6, a6, 1
        l32i.n   a8, a1, 20
        extui    a6, a6, 0, 8
        bgeu     a6, a8, .Lsec_169_1ed
        j        .Lsec_169_89
.Lsec_169_1ed:
        retw.n
        .size   wr_rx_gain_mem, .-wr_rx_gain_mem
.Lsec_169_1ef:

        .section .text.set_rx_gain_testchip_70,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_170_0, chip7_sleep_params+0x38
        .literal .Lsec_170_4, chip7_sleep_params
        .literal .Lsec_170_8, g_phyFuns
        .literal .Lsec_170_c, 0x3ff460a0
        .literal .Lsec_170_10, 0x00004000
        .literal .Lsec_170_14, 0x00008000
        .literal .Lsec_170_18, 0x3ff450dc
        .literal .Lsec_170_1c, 0xf7ffffff
        .literal .Lsec_170_20, phy_rxbb_dc
        .literal .Lsec_170_24, phy_rxrf_dc
        .literal .Lsec_170_28, phy_rxrf_dc+0x50
        .literal .Lsec_170_2c, 0xffff7fff
        .literal .Lsec_170_30, set_rx_gain_cal_iq
        .literal .Lsec_170_34, esp_dport_access_reg_read
        .literal .Lsec_170_38, esp_dport_access_reg_read
        .literal .Lsec_170_3c, esp_dport_access_reg_read
        .literal .Lsec_170_40, set_rx_gain_cal_dc
        .literal .Lsec_170_44, esp_dport_access_reg_read
        .literal .Lsec_170_48, esp_dport_access_reg_read
        .literal .Lsec_170_4c, esp_dport_access_reg_read
        .literal .Lsec_170_50, esp_dport_access_reg_read
        .literal .Lsec_170_54, set_rx_gain_cal_dc
        .literal .Lsec_170_58, esp_dport_access_reg_read
.Lsec_170_5c:
        .global set_rx_gain_testchip_70
        .type   set_rx_gain_testchip_70, @function
set_rx_gain_testchip_70:
        entry    a1, 64
        extui    a2, a2, 0, 8
        extui    a7, a7, 0, 8
        bnez.n   a2, .Lsec_170_84
        j        .Lsec_170_1be
        .byte    0x00
        .byte    0x00
.Lsec_170_6c:
        l32r     a12, .Lsec_170_0
        mov.n    a13, a10
        movi     a11, 128
        .reloc  ., R_XTENSA_ASM_EXPAND, set_rx_gain_cal_iq
        l32r     a8, .Lsec_170_30
        callx8   a8
        l32i.n   a8, a6, 0
        or       a2, a8, a2
        s32i.n   a2, a6, 0
        j        .Lsec_170_20a
.Lsec_170_84:
        l32r     a2, .Lsec_170_8
        movi.n   a14, 0
        movi.n   a13, 2
        movi.n   a12, 15
        movi.n   a11, 1
        l32i.n   a3, a2, 0
        movi     a10, 103
        l32i     a3, a3, 164
        callx8   a3
        mov.n    a8, a10
        movi.n   a15, 0
        l32i.n   a3, a2, 0
        mov.n    a14, a15
        movi.n   a13, 2
        movi.n   a12, 15
        movi     a11, 1
        l32i     a3, a3, 168
        movi     a10, 103
        s32i     a8, a1, 16
        callx8   a3
        l32r     a3, .Lsec_170_c
        or       a10, a3, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_170_34
        callx8   a8
        l32r     a5, .Lsec_170_10
        or       a5, a10, a5
        mov.n    a10, a3
        memw
        s32i     a5, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_170_38
        callx8   a8
        l32r     a5, .Lsec_170_14
        or       a10, a10, a5
        l32i.n   a5, a2, 0
        l32i     a5, a5, 192
        memw
        s32i.n   a10, a3, 0
        callx8   a5
        movi.n   a10, 0
        l32i.n   a5, a2, 0
        l32i     a5, a5, 208
        callx8   a5
        movi.n   a11, 1
        mov.n    a10, a11
        l32i     a5, a2, 0
        l32i     a9, a5, 188
        l32i     a6, a5, 184
        callx8   a9
        movi.n   a5, 2
        movi.n   a11, 1
        or       a10, a10, a5
        extui    a12, a10, 0, 16
        mov.n    a10, a11
        callx8   a6
        l32r     a6, .Lsec_170_18
        mov.n    a10, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_170_3c
        callx8   a8
        l32r     a9, .Lsec_170_1c
        and      a9, a10, a9
        movi.n   a10, 0
        memw
        s32i.n   a9, a6, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a6, a2, 0
        l32i     a6, a6, 208
        callx8   a6
        movi.n   a11, 1
        mov.n    a10, a11
        l32i.n   a6, a2, 0
        l32i     a9, a6, 188
        l32i     a6, a6, 184
        callx8   a9
        or       a10, a10, a5
        movi.n   a11, 1
        extui    a12, a10, 0, 16
        mov.n    a10, a11
        callx8   a6
        mov.n    a13, a4
        l32r     a4, .Lsec_170_28
        movi.n   a5, 0
        movi.n   a12, 1
        mov.n    a11, a5
        mov.n    a10, a12
        l32r     a15, .Lsec_170_20
        l32r     a14, .Lsec_170_24
        s32i.n   a4, a1, 0
        s32i.n   a5, a1, 12
        s32i.n   a5, a1, 8
        s32i.n   a7, a1, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, set_rx_gain_cal_dc
        l32r     a8, .Lsec_170_40
        callx8   a8
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_170_44
        callx8   a8
        l32r     a4, .Lsec_170_2c
        and      a4, a10, a4
        mov.n    a10, a5
        memw
        s32i.n   a4, a3, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32i     a3, a2, 0
        l32i     a3, a3, 208
        callx8   a3
        l32i     a3, a2, 0
        l32i     a3, a3, 196
        callx8   a3
        mov.n    a14, a5
        movi.n   a13, 2
        l32i.n   a2, a2, 0
        movi.n   a12, 15
        movi.n   a11, 1
        movi     a10, 103
        l32i.n   a8, a1, 16
        mov.n    a15, a8
        l32i     a2, a2, 168
        callx8   a2
        retw.n
.Lsec_170_1be:
        l32r     a3, .Lsec_170_c
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_170_48
        callx8   a8
        l32r     a5, .Lsec_170_10
        or       a5, a10, a5
        mov.n    a10, a3
        memw
        s32i.n   a5, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_170_4c
        callx8   a8
        l32r     a5, .Lsec_170_14
        or       a10, a10, a5
        l32r     a5, .Lsec_170_8
        l32i.n   a6, a5, 0
        l32i     a6, a6, 192
        memw
        s32i.n   a10, a3, 0
        callx8   a6
        mov.n    a10, a2
        l32i.n   a2, a5, 0
        l32i     a2, a2, 208
        callx8   a2
        movi     a2, 0x400
        l32r     a6, .Lsec_170_4
        l32i.n   a10, a6, 0
        and      a10, a2, a10
        beqz     a10, .Lsec_170_6c
.Lsec_170_20a:
        l32r     a6, .Lsec_170_18
        mov.n    a10, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_170_50
        callx8   a8
        l32r     a2, .Lsec_170_1c
        and      a2, a10, a2
        movi.n   a10, 0
        memw
        s32i.n   a2, a6, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a2, a5, 0
        l32i     a2, a2, 208
        callx8   a2
        movi.n   a2, 14
        movi.n   a11, 0
        s32i.n   a2, a1, 12
        movi.n   a2, 4
        mov.n    a10, a11
        mov.n    a13, a4
        movi.n   a12, 3
        s32i.n   a2, a1, 8
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a2, .Lsec_170_28
        l32r     a15, .Lsec_170_20
        l32r     a14, .Lsec_170_24
        s32i.n   a2, a1, 0
        s32i.n   a7, a1, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, set_rx_gain_cal_dc
        l32r     a8, .Lsec_170_54
        callx8   a8
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_170_58
        callx8   a8
        l32r     a2, .Lsec_170_2c
        and      a2, a10, a2
        movi.n   a10, 0
        memw
        s32i.n   a2, a3, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a2, a5, 0
        l32i     a2, a2, 208
        callx8   a2
        l32i.n   a2, a5, 0
        l32i     a2, a2, 196
        callx8   a2
        retw.n
        .size   set_rx_gain_testchip_70, .-set_rx_gain_testchip_70
.Lsec_170_28c:

        .section .text.bt_correct_bbgain,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_171_0, .Lsec_273_0
.Lsec_171_4:
        .global bt_correct_bbgain
        .type   bt_correct_bbgain, @function
bt_correct_bbgain:
        entry    a1, 48
        mov.n    a11, a1
        addi.n   a9, a3, 8
        or       a10, a3, a3
        movi     a8, 8
        loop     a8, .Lsec_171_21
        l8ui     a12, a10, 0
        addi.n   a10, a10, 1
        s8i      a12, a11, 0
        memw
        addi.n   a11, a11, 1
.Lsec_171_21:
        movi.n   a12, 0
        nop.n
        nop.n
        l16ui    a10, a2, 0
        beq      a10, a12, .Lsec_171_39
        movi.n   a4, 1
        addi     a10, a10, -48
        movi.n   a8, 2
        mov.n    a12, a4
        moveqz   a12, a8, a10
.Lsec_171_39:
        l8ui     a13, a1, 0
        movi.n   a8, 2
        movi.n   a10, 0
        movi.n   a14, 48
        movi.n   a15, 11
.Lsec_171_44:
        sext     a11, a13, 7
        bge      a14, a11, .Lsec_171_61
        addi     a5, a10, -24
        addi     a4, a13, -24
        addi.n   a11, a12, -1
        beqz.n   a12, .Lsec_171_78
        extui    a10, a5, 0, 8
        extui    a13, a4, 0, 8
        extui    a12, a11, 0, 8
        j        .Lsec_171_78
        .byte    0x00
.Lsec_171_61:
        blt      a15, a11, .Lsec_171_78
        addi.n   a11, a12, 1
        addi     a5, a10, 24
        addi     a4, a13, 24
        bgeui    a12, 3, .Lsec_171_78
        extui    a12, a11, 0, 8
        extui    a10, a5, 0, 8
        extui    a13, a4, 0, 8
.Lsec_171_78:
        addi.n   a8, a8, -1
        extui    a8, a8, 0, 8
        bnez     a8, .Lsec_171_44
        movi.n   a8, 32
        bgeui    a12, 3, .Lsec_171_8e
        l32r     a8, .Lsec_171_0
        addx2    a8, a12, a8
        l16ui    a8, a8, 0
.Lsec_171_8e:
        extui    a10, a10, 0, 8
        sub      a9, a9, a3
        s16i     a8, a2, 0
        memw
        loop     a9, .Lsec_171_b2
        nop.n
        nop.n
        nop.n
        nop.n
        l8ui     a8, a3, 0
        add.n    a8, a10, a8
        s8i      a8, a3, 0
        memw
        addi.n   a3, a3, 1
.Lsec_171_b2:
        retw.n
        .size   bt_correct_bbgain, .-bt_correct_bbgain
.Lsec_171_b4:

        .section .text.bt_index_to_bb,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_172_0, .Lsec_273_0
.Lsec_172_4:
        .global bt_index_to_bb
        .type   bt_index_to_bb, @function
bt_index_to_bb:
        entry    a1, 32
        extui    a8, a2, 0, 16
        movi.n   a2, 32
        bgeui    a8, 3, .Lsec_172_18
        l32r     a2, .Lsec_172_0
        addx2    a8, a8, a2
        l16ui    a2, a8, 0
.Lsec_172_18:
        retw.n
        .size   bt_index_to_bb, .-bt_index_to_bb
.Lsec_172_1a:

        .section .text.bt_bb_to_index,"ax",@progbits
        .align  4
.Lsec_173_0:
        .global bt_bb_to_index
        .type   bt_bb_to_index, @function
bt_bb_to_index:
        entry    a1, 32
        extui    a8, a2, 0, 16
        movi.n   a2, 0
        beq      a8, a2, .Lsec_173_18
        addi     a2, a8, -48
        movi.n   a9, 1
        movi.n   a8, 2
        moveqz   a9, a8, a2
        or       a2, a9, a9
.Lsec_173_18:
        retw.n
        .size   bt_bb_to_index, .-bt_bb_to_index
.Lsec_173_1a:

        .section .text.bt_txdc_cal,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_174_0, chip7_sleep_params
        .literal .Lsec_174_4, .Lsec_146_1c
.Lsec_174_8:
        .global bt_txdc_cal
        .type   bt_txdc_cal, @function
bt_txdc_cal:
        entry    a1, 32
        l32r     a8, .Lsec_174_0
        l32i.n   a8, a8, 0
        bbsi     a8, 12, .Lsec_174_19
        .reloc  ., R_XTENSA_ASM_EXPAND, .Lsec_146_1c
        l32r     a8, .Lsec_174_4
        callx8   a8
.Lsec_174_19:
        retw.n
        .size   bt_txdc_cal, .-bt_txdc_cal
.Lsec_174_1b:

        .section .text.get_bbgain_db,"ax",@progbits
        .align  4
.Lsec_175_0:
        .global get_bbgain_db
        .type   get_bbgain_db, @function
get_bbgain_db:
        entry    a1, 32
        extui    a11, a2, 0, 16
        movi.n   a8, 0
        extui    a2, a11, 6, 8
        mov.n    a10, a8
        extui    a11, a11, 0, 6
        movi.n   a9, 6
        loop     a9, .Lsec_175_1f
        addi.n   a12, a10, 6
        bbc      a11, a8, .Lsec_175_1d
        extui    a10, a12, 0, 8
.Lsec_175_1d:
        addi.n   a8, a8, 1
.Lsec_175_1f:
        add.n    a2, a10, a2
        extui    a2, a2, 0, 8
        retw.n
        .size   get_bbgain_db, .-get_bbgain_db
.Lsec_175_26:

        .section .text.bt_txiq_cal,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_176_0, chip7_sleep_params
        .literal .Lsec_176_4, .Lsec_147_20
.Lsec_176_8:
        .global bt_txiq_cal
        .type   bt_txiq_cal, @function
bt_txiq_cal:
        entry    a1, 32
        l32r     a8, .Lsec_176_0
        l32i.n   a8, a8, 0
        bbsi     a8, 11, .Lsec_176_19
        .reloc  ., R_XTENSA_ASM_EXPAND, .Lsec_147_20
        l32r     a8, .Lsec_176_4
        callx8   a8
.Lsec_176_19:
        retw.n
        .size   bt_txiq_cal, .-bt_txiq_cal
.Lsec_176_1b:

        .section .text.force_bttx_gain,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_177_0, bt_dig_gain_force
        .literal .Lsec_177_4, phy_bt_power_track_en
        .literal .Lsec_177_8, chip7_sleep_params+0x81
        .literal .Lsec_177_c, wr_bt_tx_gain_mem
        .literal .Lsec_177_10, wr_bt_tx_atten
        .literal .Lsec_177_14, wr_bt_tx_gain_mem
        .literal .Lsec_177_18, wr_bt_tx_atten
.Lsec_177_1c:
        .global force_bttx_gain
        .type   force_bttx_gain, @function
force_bttx_gain:
        entry    a1, 48
        extui    a2, a2, 0, 8
        extui    a10, a3, 0, 8
        extui    a11, a4, 0, 8
        extui    a5, a5, 0, 8
        beqz     a2, .Lsec_177_75
        extui    a12, a5, 0, 8
        mov.n    a9, a1
        movi.n   a8, 8
        loop     a8, .Lsec_177_40
        s8i      a12, a9, 0
        memw
        addi.n   a9, a9, 1
.Lsec_177_40:
        .reloc  ., R_XTENSA_ASM_EXPAND, wr_bt_tx_gain_mem
        l32r     a8, .Lsec_177_c
        callx8   a8
        movi.n   a2, 0
        l32r     a5, .Lsec_177_0
        mov.n    a10, a1
        s8i      a2, a5, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wr_bt_tx_atten
        l32r     a8, .Lsec_177_10
        callx8   a8
        movi.n   a8, 1
        s8i      a8, a5, 0
        memw
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a5, .Lsec_177_4
        s8i      a2, a5, 0
        memw
        retw.n
        .byte    0x00
.Lsec_177_75:
        l32r     a5, .Lsec_177_4
        movi.n   a8, 1
        movi     a10, 255
        mov.n    a11, a10
        s8i      a8, a5, 0
        memw
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a5, .Lsec_177_0
        s8i      a2, a5, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wr_bt_tx_gain_mem
        l32r     a8, .Lsec_177_14
        callx8   a8
        l32r     a10, .Lsec_177_8
        .reloc  ., R_XTENSA_ASM_EXPAND, wr_bt_tx_atten
        l32r     a8, .Lsec_177_18
        callx8   a8
        retw.n
        .size   force_bttx_gain, .-force_bttx_gain
.Lsec_177_a7:

        .section .text.phy_bttx_low_power,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_178_0, g_phyFuns
        .literal .Lsec_178_4, chip7_sleep_params
        .literal .Lsec_178_8, chip7_sleep_params+0x40
        .literal .Lsec_178_c, force_bttx_gain
.Lsec_178_10:
        .global phy_bttx_low_power
        .type   phy_bttx_low_power, @function
phy_bttx_low_power:
        entry    a1, 32
        extui    a3, a3, 0, 8
        extui    a2, a2, 0, 8
        bgeui    a3, 6, .Lsec_178_98
        movi.n   a4, 0
        beq      a2, a4, .Lsec_178_51
        bne      a3, a4, .Lsec_178_51
        l32r     a3, .Lsec_178_0
        movi.n   a12, 1
        movi     a13, 96
        movi.n   a11, 2
        movi     a10, 107
        l32i.n   a8, a3, 0
        l32i     a8, a8, 160
        callx8   a8
        movi.n   a12, 2
        mov.n    a11, a12
        l32i.n   a3, a3, 0
        mov.n    a13, a4
        movi     a10, 107
        l32i     a3, a3, 160
        callx8   a3
        mov.n    a11, a4
        j        .Lsec_178_89
        .byte    0x00
        .byte    0x00
.Lsec_178_51:
        l32r     a4, .Lsec_178_0
        l32r     a8, .Lsec_178_4
        l32i.n   a4, a4, 0
        l8ui     a11, a8, 81
        l32i     a4, a4, 0x16c
        l32r     a10, .Lsec_178_8
        callx8   a4
        addi.n   a9, a3, -1
        movi.n   a11, 0
        blti     a9, 1, .Lsec_178_89
.Lsec_178_6c:
        movi.n   a4, 0
        mov.n    a8, a4
        mov.n    a11, a4
        movi.n   a3, 1
.Lsec_178_74:
        ssl      a4
        sll      a4, a3
        addi.n   a8, a8, 1
        add.n    a11, a11, a4
        extui    a8, a8, 0, 8
        extui    a11, a11, 0, 8
        mov.n    a4, a8
        blt      a8, a9, .Lsec_178_74
.Lsec_178_89:
        movi.n   a13, 40
        movi.n   a12, 0
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, force_bttx_gain
        l32r     a8, .Lsec_178_c
        callx8   a8
        retw.n
        .byte    0x00
.Lsec_178_98:
        l32r     a3, .Lsec_178_0
        l32r     a4, .Lsec_178_4
        l32i.n   a3, a3, 0
        l8ui     a11, a4, 81
        l32i     a3, a3, 0x16c
        l32r     a10, .Lsec_178_8
        callx8   a3
        movi.n   a9, 4
        j        .Lsec_178_6c
        .size   phy_bttx_low_power, .-phy_bttx_low_power
.Lsec_178_b1:

        .section .text.phy_wifitx_low_power,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_179_0, phy_chan_gain_table
        .literal .Lsec_179_4, .Lsec_272_0
        .literal .Lsec_179_8, .Lsec_271_0
        .literal .Lsec_179_c, .Lsec_270_0
        .literal .Lsec_179_10, 0xaaaaaaab
        .literal .Lsec_179_14, 0x3ff46004
        .literal .Lsec_179_18, 0x3ff46008
        .literal .Lsec_179_1c, phy_set_most_tpw_disbg
        .literal .Lsec_179_20, force_tx_gain_en
        .literal .Lsec_179_24, set_tx_gain_table
        .literal .Lsec_179_28, tx_gain_table_set
.Lsec_179_2c:
        .global phy_wifitx_low_power
        .type   phy_wifitx_low_power, @function
phy_wifitx_low_power:
        entry    a1, 48
        extui    a3, a3, 0, 8
        movi.n   a8, 10
        extui    a2, a2, 0, 8
        extui    a4, a4, 0, 8
        bgeu     a8, a3, .Lsec_179_40
        movi     a3, 11
.Lsec_179_40:
        sext     a8, a4, 7
        bgei     a8, 7, .Lsec_179_4e
        movi.n   a9, -5
        blt      a8, a9, .Lsec_179_54
        j        .Lsec_179_57
.Lsec_179_4e:
        movi.n   a4, 6
        j        .Lsec_179_57
        .byte    0x00
.Lsec_179_54:
        movi     a4, 251
.Lsec_179_57:
        beqz     a2, .Lsec_179_155
        addi.n   a3, a3, -1
        extui    a3, a3, 0, 8
        movi.n   a2, 10
        bltu     a2, a3, .Lsec_179_d5
        l32r     a2, .Lsec_179_4
        slli     a9, a3, 1
        add.n    a2, a2, a9
        movi.n   a13, 1
        movi.n   a8, 0
        l16ui    a2, a2, 0
        mov.n    a11, a8
        movi.n   a14, 5
        l32r     a12, .Lsec_179_10
        muluh    a12, a2, a12
        srli     a12, a12, 2
        addx2    a10, a12, a12
        ssl      a13
        sll      a10, a10
        l32r     a15, .Lsec_179_8
        sub      a2, a2, a10
        add.n    a9, a15, a9
        extui    a12, a12, 0, 16
        extui    a2, a2, 0, 16
        l32r     a10, .Lsec_179_c
        add.n    a3, a10, a3
        l8ui     a15, a3, 0
        l16ui    a10, a9, 0
        bne      a12, a8, .Lsec_179_b1
        slli     a11, a2, 6
        extui    a11, a11, 0, 16
        extui    a3, a15, 0, 8
        j        .Lsec_179_dc
.Lsec_179_b1:
        sub      a9, a14, a8
        ssl      a9
        sll      a9, a13
        addi.n   a8, a8, 1
        add.n    a11, a11, a9
        extui    a8, a8, 0, 8
        extui    a11, a11, 0, 16
        bltu     a8, a12, .Lsec_179_b1
        slli     a8, a2, 6
        add.n    a11, a11, a8
        extui    a11, a11, 0, 16
        extui    a3, a15, 0, 8
        j        .Lsec_179_dc
.Lsec_179_d5:
        movi.n   a11, 0
        movi     a3, 242
        mov.n    a10, a11
.Lsec_179_dc:
        .reloc  ., R_XTENSA_ASM_EXPAND, set_tx_gain_table
        l32r     a8, .Lsec_179_24
        callx8   a8
        l32r     a2, .Lsec_179_0
        extui    a12, a4, 0, 8
        mov.n    a11, a2
        or       a10, a1, a1
        movi     a9, 8
        l8ui     a4, a2, 5
        sub      a4, a4, a3
        extui    a4, a4, 0, 8
        loop     a9, .Lsec_179_10f
        l8ui     a8, a11, 0
        addi.n   a11, a11, 1
        sub      a8, a8, a4
        sub      a8, a8, a12
        s8i      a8, a10, 0
        memw
        addi.n   a10, a10, 1
.Lsec_179_10f:
        movi.n   a2, 1
        nop.n
        nop.n
        l32i.n   a4, a1, 0
        l32r     a3, .Lsec_179_14
        memw
        s32i.n   a4, a3, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a4, a1, 4
        l32r     a3, .Lsec_179_18
        memw
        s32i.n   a4, a3, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a3, .Lsec_179_1c
        s8i      a2, a3, 0
        memw
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a3, .Lsec_179_20
        s8i      a2, a3, 0
        memw
        retw.n
.Lsec_179_155:
        l32r     a3, .Lsec_179_1c
        s8i      a2, a3, 0
        memw
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a3, .Lsec_179_20
        s8i      a2, a3, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, tx_gain_table_set
        l32r     a8, .Lsec_179_28
        callx8   a8
        retw.n
        .size   phy_wifitx_low_power, .-phy_wifitx_low_power
.Lsec_179_177:

        .section .text.set_tx_gain_table_bt,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_180_0, chip7_sleep_params
        .literal .Lsec_180_4, bt_dig_gain_force
        .literal .Lsec_180_8, .Lsec_146_1c
        .literal .Lsec_180_c, .Lsec_147_20
        .literal .Lsec_180_10, bt_tx_pwctrl_init
        .literal .Lsec_180_14, .Lsec_135_818
.Lsec_180_18:
        .global set_tx_gain_table_bt
        .type   set_tx_gain_table_bt, @function
set_tx_gain_table_bt:
        entry    a1, 32
        l32r     a2, .Lsec_180_0
        l32i     a8, a2, 0
        bbsi     a8, 12, .Lsec_180_2c
        .reloc  ., R_XTENSA_ASM_EXPAND, .Lsec_146_1c
        l32r     a8, .Lsec_180_8
        callx8   a8
        l32i.n   a8, a2, 0
.Lsec_180_2c:
        bbsi     a8, 11, .Lsec_180_35
        .reloc  ., R_XTENSA_ASM_EXPAND, .Lsec_147_20
        l32r     a8, .Lsec_180_c
        callx8   a8
.Lsec_180_35:
        .reloc  ., R_XTENSA_ASM_EXPAND, bt_tx_pwctrl_init
        l32r     a8, .Lsec_180_10
        callx8   a8
        l32r     a2, .Lsec_180_4
        l8ui     a2, a2, 0
        bnez.n   a2, .Lsec_180_49
        .reloc  ., R_XTENSA_ASM_EXPAND, .Lsec_135_818
        l32r     a8, .Lsec_180_14
        callx8   a8
.Lsec_180_49:
        retw.n
        .size   set_tx_gain_table_bt, .-set_tx_gain_table_bt
.Lsec_180_4b:

        .section .text.set_tx_dig_gain,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_181_0, 0x3ff46000
        .literal .Lsec_181_4, esp_dport_access_reg_read
        .literal .Lsec_181_8, esp_dport_access_reg_read
.Lsec_181_c:
        .global set_tx_dig_gain
        .type   set_tx_dig_gain, @function
set_tx_dig_gain:
        entry    a1, 32
        l32r     a4, .Lsec_181_0
        extui    a3, a3, 0, 8
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_181_4
        callx8   a8
        movi     a8, 0xfffffc03
        and      a10, a10, a8
        slli     a3, a3, 2
        or       a10, a3, a10
        memw
        s32i.n   a10, a4, 0
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_181_8
        callx8   a8
        movi.n   a3, -2
        and      a10, a10, a3
        extui    a2, a2, 0, 1
        or       a2, a10, a2
        memw
        s32i.n   a2, a4, 0
        retw.n
        .size   set_tx_dig_gain, .-set_tx_dig_gain
.Lsec_181_48:

        .section .text.spur_cal,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_182_0, .Lsec_148_0
.Lsec_182_4:
        .global spur_cal
        .type   spur_cal, @function
spur_cal:
        entry    a1, 32
        extui    a13, a5, 0, 8
        extui    a10, a2, 0, 16
        extui    a3, a3, 0, 8
        extui    a12, a4, 0, 16
        beqz.n   a13, .Lsec_182_24
        beqz.n   a12, .Lsec_182_24
        sext     a11, a3, 7
        .reloc  ., R_XTENSA_ASM_EXPAND, .Lsec_148_0
        l32r     a8, .Lsec_182_0
        callx8   a8
        mov.n    a2, a10
        retw.n
.Lsec_182_24:
        movi.n   a2, 0
        retw.n
        .size   spur_cal, .-spur_cal
.Lsec_182_28:

        .section .text.set_chanfreq,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_183_0, g_phyFuns
        .literal .Lsec_183_4, chip_v7_set_chan
.Lsec_183_8:
        .global set_chanfreq
        .type   set_chanfreq, @function
set_chanfreq:
        entry    a1, 32
        l32r     a8, .Lsec_183_0
        movi     a11, 128
        extui    a10, a2, 0, 16
        extui    a3, a3, 0, 8
        movi.n   a2, 0
        l32i.n   a8, a8, 0
        l32i     a8, a8, 108
        callx8   a8
        sext     a11, a3, 7
        sext     a10, a10, 7
        .reloc  ., R_XTENSA_ASM_EXPAND, chip_v7_set_chan
        l32r     a8, .Lsec_183_4
        callx8   a8
        retw.n
        .size   set_chanfreq, .-set_chanfreq
.Lsec_183_2f:

        .section .text.set_chanfreq_nomac,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_184_0, g_phyFuns
        .literal .Lsec_184_4, chip_v7_set_chan_nomac
.Lsec_184_8:
        .global set_chanfreq_nomac
        .type   set_chanfreq_nomac, @function
set_chanfreq_nomac:
        entry    a1, 32
        l32r     a8, .Lsec_184_0
        movi     a11, 128
        extui    a10, a2, 0, 16
        extui    a3, a3, 0, 8
        movi.n   a2, 0
        l32i.n   a8, a8, 0
        l32i     a8, a8, 108
        callx8   a8
        sext     a11, a3, 7
        sext     a10, a10, 7
        .reloc  ., R_XTENSA_ASM_EXPAND, chip_v7_set_chan_nomac
        l32r     a8, .Lsec_184_4
        callx8   a8
        retw.n
        .size   set_chanfreq_nomac, .-set_chanfreq_nomac
.Lsec_184_2f:

        .section .text.chip_sleep_prot_en,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_185_0, 0x60033cb8
        .literal .Lsec_185_4, 0x00008000
        .literal .Lsec_185_8, 0x00006000
        .literal .Lsec_185_c, 0x07ff8000
        .literal .Lsec_185_10, g_phyFuns
        .literal .Lsec_185_14, esp_dport_access_reg_read
        .literal .Lsec_185_18, ets_delay_us
        .literal .Lsec_185_1c, esp_dport_access_reg_read
        .literal .Lsec_185_20, esp_dport_access_reg_read
        .literal .Lsec_185_24, ets_delay_us
        .literal .Lsec_185_28, esp_dport_access_reg_read
.Lsec_185_2c:
        .global chip_sleep_prot_en
        .type   chip_sleep_prot_en, @function
chip_sleep_prot_en:
        entry    a1, 32
        l32r     a2, .Lsec_185_0
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_185_14
        callx8   a8
        l32r     a3, .Lsec_185_4
        or       a10, a10, a3
        memw
        s32i.n   a10, a2, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a3, .Lsec_185_8
        j        .Lsec_185_5d
        .byte    0x00
        .byte    0x00
.Lsec_185_55:
        movi.n   a10, 2
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_185_18
        callx8   a8
.Lsec_185_5d:
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_185_1c
        callx8   a8
        bany     a10, a3, .Lsec_185_55
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_185_20
        callx8   a8
        l32r     a8, .Lsec_185_c
        or       a10, a10, a8
        memw
        s32i.n   a10, a2, 0
        j        .Lsec_185_86
.Lsec_185_7e:
        movi.n   a10, 2
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_185_24
        callx8   a8
.Lsec_185_86:
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_185_28
        callx8   a8
        bany     a10, a3, .Lsec_185_7e
        l32r     a2, .Lsec_185_10
        l32i.n   a2, a2, 0
        l32i     a2, a2, 8
        callx8   a2
        retw.n
        .size   chip_sleep_prot_en, .-chip_sleep_prot_en
.Lsec_185_9e:

        .section .text.chip_sleep_prot_dis,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_186_0, g_phyFuns
        .literal .Lsec_186_4, 0x60033cb8
        .literal .Lsec_186_8, 0xf8007fff
        .literal .Lsec_186_c, esp_dport_access_reg_read
.Lsec_186_10:
        .global chip_sleep_prot_dis
        .type   chip_sleep_prot_dis, @function
chip_sleep_prot_dis:
        entry    a1, 32
        l32r     a2, .Lsec_186_0
        l32i.n   a2, a2, 0
        l32i.n   a2, a2, 12
        callx8   a2
        l32r     a2, .Lsec_186_4
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_186_c
        callx8   a8
        l32r     a8, .Lsec_186_8
        and      a10, a10, a8
        memw
        s32i.n   a10, a2, 0
        retw.n
        .size   chip_sleep_prot_dis, .-chip_sleep_prot_dis
.Lsec_186_35:

        .section .text.chip_v7_rxmax_ext_dig,"ax",@progbits
        .align  4
.Lsec_187_0:
        .global chip_v7_rxmax_ext_dig
        .type   chip_v7_rxmax_ext_dig, @function
chip_v7_rxmax_ext_dig:
        entry    a1, 32
        retw.n
        .size   chip_v7_rxmax_ext_dig, .-chip_v7_rxmax_ext_dig
.Lsec_187_5:

        .section .text.chip_v7_rxmax_ext,"ax",@progbits
        .align  4
.Lsec_188_0:
        .global chip_v7_rxmax_ext
        .type   chip_v7_rxmax_ext, @function
chip_v7_rxmax_ext:
        entry    a1, 32
        retw.n
        .size   chip_v7_rxmax_ext, .-chip_v7_rxmax_ext
.Lsec_188_5:

        .section .text.set_cca,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_189_0, 0x3ff5c018
        .literal .Lsec_189_4, 0x40000000
        .literal .Lsec_189_8, 0x3ff5c01c
        .literal .Lsec_189_c, esp_dport_access_reg_read
        .literal .Lsec_189_10, esp_dport_access_reg_read
        .literal .Lsec_189_14, esp_dport_access_reg_read
.Lsec_189_18:
        .global set_cca
        .type   set_cca, @function
set_cca:
        entry    a1, 32
        l32r     a4, .Lsec_189_0
        extui    a2, a2, 0, 8
        or       a10, a4, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_189_c
        callx8   a8
        movi.n   a9, 1
        l32r     a8, .Lsec_189_4
        extui    a3, a3, 0, 8
        or       a10, a10, a8
        movi.n   a8, 0
        moveqz   a8, a9, a2
        extui    a2, a8, 0, 8
        memw
        s32i.n   a10, a4, 0
        bnez     a2, .Lsec_189_4e
        sext     a3, a3, 7
        moveqz   a2, a9, a3
        beqz     a2, .Lsec_189_70
.Lsec_189_4e:
        nop.n
        nop.n
        l32r     a3, .Lsec_189_8
        movi     a2, 0xffffff00
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_189_10
        callx8   a8
        and      a10, a10, a2
        movi     a2, 191
        or       a10, a10, a2
        memw
        s32i.n   a10, a3, 0
        retw.n
.Lsec_189_70:
        l32r     a4, .Lsec_189_8
        movi     a2, 0xffffff00
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_189_14
        callx8   a8
        and      a10, a10, a2
        extui    a3, a3, 0, 8
        or       a3, a10, a3
        memw
        s32i.n   a3, a4, 0
        retw.n
        .size   set_cca, .-set_cca
.Lsec_189_8e:

        .section .text.phy_get_cca,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_190_0, 0x3ff5c01c
        .literal .Lsec_190_4, esp_dport_access_reg_read
.Lsec_190_8:
        .global phy_get_cca
        .type   phy_get_cca, @function
phy_get_cca:
        entry    a1, 32
        l32r     a10, .Lsec_190_0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_190_4
        callx8   a8
        extui    a2, a10, 0, 8
        retw.n
        .size   phy_get_cca, .-phy_get_cca
.Lsec_190_19:

        .section .text.phy_set_cca_cnt,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_191_0, 0x3ff5d058
        .literal .Lsec_191_4, 0xf8000000
        .literal .Lsec_191_8, 0x07ffffff
        .literal .Lsec_191_c, 0x18000000
        .literal .Lsec_191_10, esp_dport_access_reg_read
        .literal .Lsec_191_14, esp_dport_access_reg_read
.Lsec_191_18:
        .global phy_set_cca_cnt
        .type   phy_set_cca_cnt, @function
phy_set_cca_cnt:
        entry    a1, 32
        l32r     a4, .Lsec_191_0
        extui    a3, a3, 0, 8
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_191_10
        callx8   a8
        l32r     a8, .Lsec_191_4
        and      a10, a10, a8
        l32r     a8, .Lsec_191_8
        and      a2, a2, a8
        or       a10, a10, a2
        memw
        s32i.n   a10, a4, 0
        beqz     a3, .Lsec_191_54
        or       a10, a4, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_191_14
        callx8   a8
        l32r     a2, .Lsec_191_c
        or       a10, a10, a2
        memw
        s32i.n   a10, a4, 0
.Lsec_191_54:
        retw.n
        .size   phy_set_cca_cnt, .-phy_set_cca_cnt
.Lsec_191_56:

        .section .text.phy_get_cca_cnt,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_192_0, 0x3ff5d05c
        .literal .Lsec_192_4, 0x07ffffff
        .literal .Lsec_192_8, 0x3ff5d060
        .literal .Lsec_192_c, 0x08000000
        .literal .Lsec_192_10, esp_dport_access_reg_read
        .literal .Lsec_192_14, esp_dport_access_reg_read
        .literal .Lsec_192_18, esp_dport_access_reg_read
.Lsec_192_1c:
        .global phy_get_cca_cnt
        .type   phy_get_cca_cnt, @function
phy_get_cca_cnt:
        entry    a1, 32
        l32r     a4, .Lsec_192_0
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_192_10
        callx8   a8
        l32r     a3, .Lsec_192_4
        and      a8, a10, a3
        l32r     a10, .Lsec_192_8
        s32i     a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_192_14
        callx8   a8
        and      a3, a10, a3
        mov.n    a10, a4
        s32i.n   a3, a2, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_192_18
        callx8   a8
        l32r     a2, .Lsec_192_c
        and      a2, a10, a2
        extui    a2, a2, 27, 5
        retw.n
        .size   phy_get_cca_cnt, .-phy_get_cca_cnt
.Lsec_192_54:

        .section .text.phy_get_noise_floor,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_193_0, 0x3ff5c050
        .literal .Lsec_193_4, esp_dport_access_reg_read
.Lsec_193_8:
        .global phy_get_noise_floor
        .type   phy_get_noise_floor, @function
phy_get_noise_floor:
        entry    a1, 32
        l32r     a10, .Lsec_193_0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_193_4
        callx8   a8
        extui    a10, a10, 0, 10
        addmi    a2, a10, 0xfffffc00
        retw.n
        .size   phy_get_noise_floor, .-phy_get_noise_floor
.Lsec_193_1c:

        .section .text.phy_get_rssi,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_194_0, 0x3ff5c050
        .literal .Lsec_194_4, 0x3ff5c06c
        .literal .Lsec_194_8, 0x0000ff00
        .literal .Lsec_194_c, esp_dport_access_reg_read
        .literal .Lsec_194_10, esp_dport_access_reg_read
.Lsec_194_14:
        .global phy_get_rssi
        .type   phy_get_rssi, @function
phy_get_rssi:
        entry    a1, 32
        l32r     a10, .Lsec_194_0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_194_c
        callx8   a8
        mov.n    a2, a10
        extui    a2, a2, 0, 10
        l32r     a10, .Lsec_194_4
        addmi    a2, a2, 0xfffffc00
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_194_10
        callx8   a8
        srai     a2, a2, 2
        l32r     a8, .Lsec_194_8
        and      a10, a10, a8
        srli     a10, a10, 8
        add.n    a2, a2, a10
        sext     a2, a2, 15
        retw.n
        .size   phy_get_rssi, .-phy_get_rssi
.Lsec_194_44:

        .section .text.set_rx_sense,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_195_0, corr_cca_thr
        .literal .Lsec_195_4, 0x3ff5c018
        .literal .Lsec_195_8, 0x3ff5c01c
        .literal .Lsec_195_c, 0xff00ffff
        .literal .Lsec_195_10, 0x00120000
        .literal .Lsec_195_14, 0x3ff5c010
        .literal .Lsec_195_18, 0x007fffff
        .literal .Lsec_195_1c, 0x06000000
        .literal .Lsec_195_20, 0x3ff5c014
        .literal .Lsec_195_24, 0x01000000
        .literal .Lsec_195_28, 0x3ff5c0cc
        .literal .Lsec_195_2c, 0xfff003ff
        .literal .Lsec_195_30, 0x000a3800
        .literal .Lsec_195_34, 0xc00fffff
        .literal .Lsec_195_38, 0x2a800000
        .literal .Lsec_195_3c, 0x3ff5c044
        .literal .Lsec_195_40, 0x3ff5c124
        .literal .Lsec_195_44, 0x00200000
        .literal .Lsec_195_48, esp_dport_access_reg_read
        .literal .Lsec_195_4c, esp_dport_access_reg_read
        .literal .Lsec_195_50, esp_dport_access_reg_read
        .literal .Lsec_195_54, esp_dport_access_reg_read
        .literal .Lsec_195_58, esp_dport_access_reg_read
        .literal .Lsec_195_5c, esp_dport_access_reg_read
        .literal .Lsec_195_60, esp_dport_access_reg_read
        .literal .Lsec_195_64, esp_dport_access_reg_read
        .literal .Lsec_195_68, esp_dport_access_reg_read
        .literal .Lsec_195_6c, esp_dport_access_reg_read
        .literal .Lsec_195_70, esp_dport_access_reg_read
        .literal .Lsec_195_74, esp_dport_access_reg_read
        .literal .Lsec_195_78, esp_dport_access_reg_read
        .literal .Lsec_195_7c, esp_dport_access_reg_read
        .literal .Lsec_195_80, esp_dport_access_reg_read
        .literal .Lsec_195_84, esp_dport_access_reg_read
        .literal .Lsec_195_88, esp_dport_access_reg_read
.Lsec_195_8c:
        .global set_rx_sense
        .type   set_rx_sense, @function
set_rx_sense:
        entry    a1, 48
        l32r     a4, .Lsec_195_0
        extui    a2, a2, 0, 8
        movi.n   a6, 1
        l32r     a10, .Lsec_195_4
        s8i      a2, a4, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_195_48
        callx8   a8
        extui    a10, a10, 5, 10
        addmi    a10, a10, 0xfffffc00
        addi.n   a10, a10, 1
        extui    a5, a10, 31, 1
        l8ui     a3, a4, 0
        add.n    a10, a5, a10
        sext     a2, a2, 7
        sext     a3, a3, 7
        srai     a10, a10, 1
        addmi    a4, a2, 0x100
        subx2    a10, a3, a10
        movi.n   a5, 0
        moveqz   a5, a6, a2
        extui    a3, a10, 0, 16
        slli     a4, a4, 2
        extui    a4, a4, 0, 16
        extui    a2, a5, 0, 8
        sext     a6, a3, 15
        movi     a5, 18
        sext     a7, a4, 15
        bge      a5, a6, .Lsec_195_e5
        beqz     a2, .Lsec_195_111
.Lsec_195_e5:
        l32r     a5, .Lsec_195_8
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_195_4c
        callx8   a8
        l32r     a8, .Lsec_195_c
        and      a10, a10, a8
        s32i.n   a8, a1, 4
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a8, .Lsec_195_10
        or       a10, a10, a8
        memw
        s32i.n   a10, a5, 0
        j        .Lsec_195_132
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_195_111:
        l32r     a5, .Lsec_195_8
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_195_50
        callx8   a8
        extui    a8, a3, 0, 8
        l32r     a9, .Lsec_195_c
        slli     a8, a8, 16
        and      a10, a10, a9
        or       a8, a8, a10
        s32i.n   a9, a1, 4
        memw
        s32i.n   a8, a5, 0
.Lsec_195_132:
        movi.n   a5, 12
        bge      a5, a6, .Lsec_195_139
        beqz.n   a2, .Lsec_195_160
.Lsec_195_139:
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_195_14
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_195_54
        callx8   a8
        l32r     a8, .Lsec_195_18
        and      a10, a10, a8
        l32r     a9, .Lsec_195_1c
        or       a10, a10, a9
        memw
        s32i.n   a10, a5, 0
        j        .Lsec_195_17d
.Lsec_195_160:
        l32r     a5, .Lsec_195_14
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_195_58
        callx8   a8
        slli     a9, a3, 23
        l32r     a8, .Lsec_195_18
        and      a10, a10, a8
        or       a9, a10, a9
        memw
        s32i.n   a9, a5, 0
.Lsec_195_17d:
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a5, .Lsec_195_20
        mov.n    a10, a5
        s32i.n   a8, a1, 8
        blti     a6, 3, .Lsec_195_191
        beqz.n   a2, .Lsec_195_1ac
.Lsec_195_191:
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_195_5c
        callx8   a8
        l32i.n   a8, a1, 8
        and      a10, a10, a8
        l32r     a9, .Lsec_195_24
        or       a10, a10, a9
        memw
        s32i.n   a10, a5, 0
        j        .Lsec_195_1c2
        .byte    0x00
        .byte    0x00
.Lsec_195_1ac:
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_195_60
        callx8   a8
        l32i.n   a8, a1, 8
        and      a10, a10, a8
        slli     a8, a3, 23
        or       a10, a10, a8
        memw
        s32i.n   a10, a5, 0
.Lsec_195_1c2:
        movi     a8, 0x2a8
        movi.n   a5, 1
        bge      a8, a7, .Lsec_195_1cc
        movi.n   a5, 0
.Lsec_195_1cc:
        or       a5, a2, a5
        extui    a5, a5, 0, 8
        s32i.n   a5, a1, 0
        beqz.n   a5, .Lsec_195_1fd
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a5, .Lsec_195_28
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_195_64
        callx8   a8
        movi     a9, 0xfffffc00
        and      a10, a10, a9
        movi     a9, 0x2a8
        or       a10, a10, a9
        memw
        s32i.n   a10, a5, 0
        j        .Lsec_195_21a
.Lsec_195_1fd:
        l32r     a5, .Lsec_195_28
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_195_68
        callx8   a8
        movi     a9, 0xfffffc00
        and      a10, a10, a9
        extui    a9, a4, 0, 10
        or       a10, a10, a9
        memw
        s32i.n   a10, a5, 0
.Lsec_195_21a:
        movi     a9, 0x28e
        mov.n    a10, a5
        bge      a9, a7, .Lsec_195_224
        beqz.n   a2, .Lsec_195_240
.Lsec_195_224:
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_195_6c
        callx8   a8
        l32r     a7, .Lsec_195_2c
        and      a10, a10, a7
        l32r     a7, .Lsec_195_30
        or       a10, a10, a7
        memw
        s32i.n   a10, a5, 0
        j        .Lsec_195_25a
        .byte    0x00
        .byte    0x00
.Lsec_195_240:
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_195_70
        callx8   a8
        l32r     a7, .Lsec_195_2c
        extui    a9, a4, 0, 10
        slli     a9, a9, 10
        and      a10, a10, a7
        or       a10, a9, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_195_25a:
        mov.n    a10, a5
        nop.n
        nop.n
        nop.n
        l32i.n   a8, a1, 0
        beqz.n   a8, .Lsec_195_280
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_195_74
        callx8   a8
        l32r     a4, .Lsec_195_34
        and      a10, a10, a4
        l32r     a4, .Lsec_195_38
        or       a10, a10, a4
        memw
        s32i.n   a10, a5, 0
        j        .Lsec_195_29a
.Lsec_195_280:
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_195_78
        callx8   a8
        l32r     a7, .Lsec_195_34
        extui    a4, a4, 0, 10
        slli     a4, a4, 20
        and      a10, a10, a7
        or       a4, a4, a10
        memw
        s32i.n   a4, a5, 0
.Lsec_195_29a:
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a4, .Lsec_195_3c
        mov.n    a10, a4
        blti     a6, -1, .Lsec_195_2ac
        beqz.n   a2, .Lsec_195_2c8
.Lsec_195_2ac:
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_195_7c
        callx8   a8
        movi     a5, 0xffffff00
        and      a10, a10, a5
        movi     a5, 254
        or       a10, a10, a5
        memw
        s32i.n   a10, a4, 0
        j        .Lsec_195_2df
        .byte    0x00
        .byte    0x00
.Lsec_195_2c8:
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_195_80
        callx8   a8
        movi     a5, 0xffffff00
        and      a10, a10, a5
        extui    a5, a3, 0, 8
        or       a10, a10, a5
        memw
        s32i.n   a10, a4, 0
.Lsec_195_2df:
        movi.n   a4, 32
        bge      a4, a6, .Lsec_195_2e6
        beqz.n   a2, .Lsec_195_30c
.Lsec_195_2e6:
        nop.n
        nop.n
        nop.n
        l32r     a3, .Lsec_195_40
        or       a10, a3, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_195_84
        callx8   a8
        l32i.n   a9, a1, 4
        and      a10, a10, a9
        l32r     a2, .Lsec_195_44
        or       a10, a10, a2
        memw
        s32i.n   a10, a3, 0
        retw.n
        .byte    0x00
        .byte    0x00
.Lsec_195_30c:
        l32r     a2, .Lsec_195_40
        extui    a3, a3, 0, 8
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_195_88
        callx8   a8
        slli     a3, a3, 16
        l32i.n   a4, a1, 4
        and      a10, a10, a4
        or       a3, a3, a10
        memw
        s32i.n   a3, a2, 0
        retw.n
        .size   set_rx_sense, .-set_rx_sense
.Lsec_195_32c:

        .section .text.read_hw_noisefloor,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_196_0, 0x3ff5c018
        .literal .Lsec_196_4, esp_dport_access_reg_read
.Lsec_196_8:
        .global read_hw_noisefloor
        .type   read_hw_noisefloor, @function
read_hw_noisefloor:
        entry    a1, 32
        l32r     a10, .Lsec_196_0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_196_4
        callx8   a8
        extui    a10, a10, 5, 10
        addmi    a2, a10, 0xfffffc00
        retw.n
        .size   read_hw_noisefloor, .-read_hw_noisefloor
.Lsec_196_1c:

        .section .text.phy_get_txpwr_param,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_197_0, chip7_sleep_params
        .literal .Lsec_197_4, phy_tx_pwr_error
.Lsec_197_8:
        .global phy_get_txpwr_param
        .type   phy_get_txpwr_param, @function
phy_get_txpwr_param:
        entry    a1, 32
        l32r     a12, .Lsec_197_0
        mov.n    a10, a12
        mov.n    a9, a12
        movi     a8, 4
        loop     a8, .Lsec_197_35
        l16ui    a11, a10, 4
        addi.n   a9, a9, 6
        addi.n   a10, a10, 2
        s8i      a11, a2, 0
        memw
        addi.n   a2, a2, 1
        nop.n
        nop.n
        nop.n
        l8ui     a11, a9, 95
        s8i      a11, a5, 0
        addi.n   a5, a5, 1
.Lsec_197_35:
        movi.n   a2, 0
        s8i      a2, a3, 0
        s8i      a2, a3, 1
        memw
        nop.n
        nop.n
        nop.n
        nop.n
        l16ui    a2, a12, 188
        slli     a3, a2, 9
        l16ui    a2, a12, 190
        or       a2, a3, a2
        s16i     a2, a4, 0
        memw
        nop.n
        nop.n
        nop.n
        nop.n
        l8ui     a2, a12, 184
        s8i      a2, a6, 0
        memw
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a2, .Lsec_197_4
        l16ui    a2, a2, 0
        s16i     a2, a7, 0
        memw
        retw.n
        .size   phy_get_txpwr_param, .-phy_get_txpwr_param
.Lsec_197_81:

        .section .text.noise_check_loop,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_198_0, 0x3ff5d044
        .literal .Lsec_198_4, 0x3ff5d050
        .literal .Lsec_198_8, 0x3ff5c018
        .literal .Lsec_198_c, 0x08000000
        .literal .Lsec_198_10, 0xf7ffffff
        .literal .Lsec_198_14, chip7_sleep_params
        .literal .Lsec_198_18, 0x3ff5c050
        .literal .Lsec_198_1c, phy_meas_noise_floor
        .literal .Lsec_198_20, sw_scan_mode
        .literal .Lsec_198_24, g_phyFuns
        .literal .Lsec_198_28, esp_dport_access_reg_read
        .literal .Lsec_198_2c, esp_dport_access_reg_read
        .literal .Lsec_198_30, esp_dport_access_reg_read
        .literal .Lsec_198_34, esp_dport_access_reg_read
        .literal .Lsec_198_38, esp_dport_access_reg_read
        .literal .Lsec_198_3c, esp_dport_access_reg_read
.Lsec_198_40:
        .global noise_check_loop
        .type   noise_check_loop, @function
noise_check_loop:
        entry    a1, 32
        l32r     a4, .Lsec_198_0
        movi.n   a2, -2
        or       a10, a4, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_198_28
        callx8   a8
        and      a2, a10, a2
        l32r     a6, .Lsec_198_4
        movi.n   a5, 1
        mov.n    a10, a6
        extui    a3, a3, 0, 8
        memw
        s32i.n   a2, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_198_2c
        callx8   a8
        or       a5, a10, a5
        l32r     a2, .Lsec_198_8
        mov.n    a10, a2
        memw
        s32i     a5, a6, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_198_30
        callx8   a8
        l32r     a5, .Lsec_198_c
        or       a5, a10, a5
        mov.n    a10, a2
        memw
        s32i.n   a5, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_198_34
        callx8   a8
        l32r     a8, .Lsec_198_10
        and      a8, a10, a8
        l32r     a5, .Lsec_198_14
        l32r     a10, .Lsec_198_18
        memw
        s32i.n   a8, a2, 0
        nop.n
        nop.n
        nop
        nop
        l16ui    a2, a5, 178
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_198_38
        callx8   a8
        extui    a10, a10, 0, 10
        addmi    a9, a10, 0xfffffc00
        movi     a8, 0xfffffd93
        l32r     a6, .Lsec_198_1c
        add.n    a10, a10, a8
        sext     a9, a9, 15
        extui    a10, a10, 0, 16
        movi.n   a8, 62
        s16i     a9, a6, 0
        memw
        sext     a6, a2, 15
        bgeu     a8, a10, .Lsec_198_d9
        j        .Lsec_198_184
.Lsec_198_d9:
        movi     a8, 0xfffffea1
        movi     a10, 0xfffffea0
        bge      a9, a8, .Lsec_198_e8
        movi     a8, 0xfffffe78
        max      a10, a9, a8
.Lsec_198_e8:
        bge      a6, a10, .Lsec_198_f3
        addx2    a6, a6, a6
        add.n    a10, a6, a10
        j        .Lsec_198_116
.Lsec_198_f3:
        blt      a10, a6, .Lsec_198_fc
        l16si    a8, a5, 178
        j        .Lsec_198_12a
.Lsec_198_fc:
        addi     a8, a6, -16
        bge      a10, a8, .Lsec_198_111
        addi     a8, a2, -12
        sext     a8, a8, 15
        s16i     a8, a5, 178
        memw
        j        .Lsec_198_12a
.Lsec_198_111:
        addx2    a2, a10, a10
        add.n    a10, a2, a6
.Lsec_198_116:
        addi     a8, a10, -2
        addi.n   a10, a10, 1
        movltz   a8, a10, a8
        srai     a8, a8, 2
        sext     a8, a8, 15
        s16i     a8, a5, 178
        memw
.Lsec_198_12a:
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a2, .Lsec_198_20
        l8ui     a2, a2, 0
        beqz.n   a2, .Lsec_198_158
        movi     a2, 0xfffffe81
        blt      a8, a2, .Lsec_198_166
        movi     a2, 0xfffffe80
        add.n    a8, a8, a2
        extui    a2, a8, 31, 1
        add.n    a8, a2, a8
        srai     a8, a8, 1
        s16i     a8, a5, 78
        memw
        j        .Lsec_198_16c
        .byte    0x00
        .byte    0x00
.Lsec_198_158:
        l16si    a2, a5, 78
        addi.n   a6, a2, 4
        blt      a6, a8, .Lsec_198_166
        addi     a2, a2, -4
        bge      a8, a2, .Lsec_198_184
.Lsec_198_166:
        s16i     a8, a5, 78
        memw
.Lsec_198_16c:
        beqz.n   a3, .Lsec_198_184
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a2, .Lsec_198_24
        l32i.n   a2, a2, 0
        l16si    a10, a5, 78
        l32i     a2, a2, 132
        callx8   a2
.Lsec_198_184:
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_198_3c
        callx8   a8
        movi.n   a2, 1
        or       a10, a10, a2
        memw
        s32i.n   a10, a4, 0
        retw.n
        .size   noise_check_loop, .-noise_check_loop
.Lsec_198_198:

        .section .text.noise_init,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_199_0, 0x3ff5c018
        .literal .Lsec_199_4, 0x10000000
        .literal .Lsec_199_8, chip7_sleep_params
        .literal .Lsec_199_c, 0x00200000
        .literal .Lsec_199_10, noise_array
        .literal .Lsec_199_14, g_phyFuns
        .literal .Lsec_199_18, noise_timeout_flag
        .literal .Lsec_199_1c, esp_dport_access_reg_read
        .literal .Lsec_199_20, esp_dport_access_reg_read
        .literal .Lsec_199_24, chip_v7_set_chan_nomac
.Lsec_199_28:
        .global noise_init
        .type   noise_init, @function
noise_init:
        entry    a1, 32
        l32r     a2, .Lsec_199_0
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_199_1c
        callx8   a8
        l32r     a3, .Lsec_199_4
        or       a3, a10, a3
        mov.n    a10, a2
        memw
        s32i.n   a3, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_199_20
        callx8   a8
        movi.n   a3, -8
        and      a10, a10, a3
        movi.n   a3, 4
        l32r     a7, .Lsec_199_8
        or       a10, a10, a3
        memw
        s32i.n   a10, a2, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a8, a7, 0
        l32r     a2, .Lsec_199_c
        and      a8, a8, a2
        beqz.n   a8, .Lsec_199_78
        l16si    a10, a7, 78
        l32r     a2, .Lsec_199_14
        j        .Lsec_199_fe
        .byte    0x00
        .byte    0x00
.Lsec_199_78:
        l32r     a5, .Lsec_199_10
        mov.n    a3, a8
        movi.n   a4, 1
        mov.n    a6, a8
        l32r     a2, .Lsec_199_14
.Lsec_199_84:
        movi.n   a11, 10
        mov.n    a10, a4
        s8i      a6, a7, 83
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, chip_v7_set_chan_nomac
        l32r     a8, .Lsec_199_24
        callx8   a8
        movi.n   a8, 0
        l32i.n   a10, a2, 0
        l32i     a10, a10, 128
        s16i     a8, a5, 0
        memw
        callx8   a10
        neg      a11, a10
        sext     a11, a11, 15
        blti     a10, 1, .Lsec_199_110
        l16si    a10, a5, 0
        blt      a11, a10, .Lsec_199_119
.Lsec_199_b3:
        movi.n   a8, 1
        ssl      a3
        sll      a10, a8
        l32r     a8, .Lsec_199_18
        l8ui     a11, a8, 0
        or       a10, a10, a11
        s8i      a10, a8, 0
        memw
.Lsec_199_ca:
        addi.n   a3, a3, 1
        addi.n   a4, a4, 5
        addi.n   a5, a5, 2
        bnei     a3, 3, .Lsec_199_84
        nop.n
        l32r     a3, .Lsec_199_10
        movi.n   a4, 0
        l16si    a5, a3, 0
        min      a5, a5, a4
        l16si    a4, a3, 2
        min      a5, a5, a4
        l32r     a6, .Lsec_199_c
        l32i.n   a4, a7, 0
        or       a4, a4, a6
        l16si    a3, a3, 4
        min      a3, a5, a3
        mov.n    a10, a3
        s16i     a3, a7, 78
        s16i     a3, a7, 178
        s32i.n   a4, a7, 0
.Lsec_199_fe:
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a2, a2, 0
        l32i     a2, a2, 132
        callx8   a2
        retw.n
.Lsec_199_110:
        l16si    a11, a5, 0
        bge      a10, a11, .Lsec_199_ca
        j        .Lsec_199_122
.Lsec_199_119:
        s16i     a11, a5, 0
        memw
        j        .Lsec_199_b3
.Lsec_199_122:
        s16i     a10, a5, 0
        memw
        j        .Lsec_199_ca
        .size   noise_init, .-noise_init
.Lsec_199_12b:

        .section .text.target_power_backoff,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_200_0, chip7_sleep_params
        .literal .Lsec_200_4, target_power_backoff_qdb
        .literal .Lsec_200_8, set_chan_dig_gain
.Lsec_200_c:
        .global target_power_backoff
        .type   target_power_backoff, @function
target_power_backoff:
        entry    a1, 32
        l32r     a9, .Lsec_200_0
        sext     a2, a2, 7
        l32r     a11, .Lsec_200_4
        l16ui    a8, a9, 182
        l16ui    a10, a11, 0
        sub      a8, a8, a10
        add.n    a8, a2, a8
        l8ui     a10, a9, 81
        s16i     a8, a9, 182
        s16i     a2, a11, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, set_chan_dig_gain
        l32r     a8, .Lsec_200_8
        callx8   a8
        retw.n
        .size   target_power_backoff, .-target_power_backoff
.Lsec_200_37:

        .section .text.phy_set_rfrx_dcap,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_201_0, rfrx_dcap_enable
        .literal .Lsec_201_4, g_phyFuns
        .literal .Lsec_201_8, rfrx_vga_lna_dcap
        .literal .Lsec_201_c, set_chan_rxcomp
        .literal .Lsec_201_10, set_chan_rxcomp
.Lsec_201_14:
        .global phy_set_rfrx_dcap
        .type   phy_set_rfrx_dcap, @function
phy_set_rfrx_dcap:
        entry    a1, 32
        l32r     a3, .Lsec_201_0
        movi.n   a4, 0
        extui    a2, a2, 0, 8
        l8ui     a8, a3, 0
        bne      a8, a4, .Lsec_201_66
        bnei     a2, 1, .Lsec_201_66
        mov.n    a14, a4
        l32r     a2, .Lsec_201_4
        mov.n    a11, a4
        movi.n   a13, 3
        movi.n   a12, 7
        movi     a10, 100
        l32i.n   a5, a2, 0
        l32i     a5, a5, 164
        callx8   a5
        mov.n    a14, a4
        mov.n    a11, a4
        l32i.n   a5, a2, 0
        movi     a13, 3
        movi     a12, 4
        l32r     a4, .Lsec_201_8
        l32i     a5, a5, 164
        s8i      a10, a4, 0
        memw
        movi     a10, 100
        callx8   a5
        s8i      a10, a4, 1
        memw
        j        .Lsec_201_6c
        .byte    0x00
.Lsec_201_66:
        beqz     a2, .Lsec_201_aa
        l32r     a2, .Lsec_201_4
.Lsec_201_6c:
        movi.n   a15, 0
        mov.n    a14, a15
        mov.n    a11, a15
        movi.n   a13, 3
        l32i.n   a4, a2, 0
        movi.n   a12, 7
        movi     a10, 100
        l32i     a4, a4, 168
        callx8   a4
        movi.n   a15, 0
        mov.n    a14, a15
        l32i.n   a2, a2, 0
        movi.n   a13, 3
        movi.n   a12, 4
        mov.n    a11, a15
        movi     a10, 100
        l32i     a2, a2, 168
        callx8   a2
        movi.n   a10, -28
        .reloc  ., R_XTENSA_ASM_EXPAND, set_chan_rxcomp
        l32r     a8, .Lsec_201_c
        callx8   a8
        movi.n   a2, 1
        s8i      a2, a3, 0
        memw
        retw.n
        .byte    0x00
        .byte    0x00
.Lsec_201_aa:
        beqz.n   a8, .Lsec_201_e6
        l32r     a5, .Lsec_201_4
        mov.n    a14, a2
        mov.n    a11, a2
        movi.n   a13, 3
        movi.n   a12, 7
        l32r     a4, .Lsec_201_8
        movi     a10, 100
        l32i.n   a8, a5, 0
        l8ui     a15, a4, 0
        l32i     a8, a8, 168
        callx8   a8
        movi     a10, 100
        mov.n    a14, a2
        l32i.n   a5, a5, 0
        movi.n   a13, 3
        movi.n   a12, 4
        mov.n    a11, a2
        l32i     a5, a5, 168
        l8ui     a15, a4, 1
        callx8   a5
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, set_chan_rxcomp
        l32r     a8, .Lsec_201_10
        callx8   a8
.Lsec_201_e6:
        movi.n   a2, 0
        s8i      a2, a3, 0
        memw
        retw.n
        .size   phy_set_rfrx_dcap, .-phy_set_rfrx_dcap
.Lsec_201_f0:

        .section .text.chip_v7_set_chan_misc,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_202_0, g_phyFuns
        .literal .Lsec_202_4, chip7_sleep_params
        .literal .Lsec_202_8, re_entry
        .literal .Lsec_202_c, phy_init_flag
        .literal .Lsec_202_10, phy_rxrf_dc+0x50
        .literal .Lsec_202_14, phy_rxbb_dc
        .literal .Lsec_202_18, phy_rxrf_dc+0x24
        .literal .Lsec_202_1c, .Lsec_304_0
        .literal .Lsec_202_20, chip7_sleep_params+0x40
        .literal .Lsec_202_24, wr_rx_gain_mem
        .literal .Lsec_202_28, set_chan_rxcomp
        .literal .Lsec_202_2c, set_chan_dig_gain
.Lsec_202_30:
        .global chip_v7_set_chan_misc
        .type   chip_v7_set_chan_misc, @function
chip_v7_set_chan_misc:
        entry    a1, 48
        l32r     a3, .Lsec_202_0
        extui    a2, a2, 0, 8
        l32i     a4, a3, 0
        l32r     a5, .Lsec_202_4
        l32i     a4, a4, 132
        l16si    a10, a5, 78
        callx8   a4
        l32r     a4, .Lsec_202_8
        l8ui     a8, a4, 0
        beqz.n   a8, .Lsec_202_58
        l32r     a8, .Lsec_202_c
        l8ui     a8, a8, 0
        bnez.n   a8, .Lsec_202_7c
.Lsec_202_58:
        movi.n   a11, 1
        movi.n   a9, 0
        l8ui     a8, a5, 176
        mov.n    a10, a9
        add.n    a8, a8, a11
        extui    a8, a8, 0, 8
        l32r     a15, .Lsec_202_10
        l32r     a14, .Lsec_202_14
        l32r     a13, .Lsec_202_18
        l32r     a12, .Lsec_202_1c
        s32i.n   a9, a1, 4
        s32i.n   a8, a1, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, wr_rx_gain_mem
        l32r     a8, .Lsec_202_24
        callx8   a8
.Lsec_202_7c:
        movi.n   a10, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, set_chan_rxcomp
        l32r     a8, .Lsec_202_28
        callx8   a8
        extui    a2, a2, 0, 8
        l32i.n   a3, a3, 0
        mov.n    a11, a2
        l32i     a3, a3, 0x16c
        l32r     a10, .Lsec_202_20
        callx8   a3
        l8ui     a3, a4, 0
        beqz.n   a3, .Lsec_202_a1
        l32r     a3, .Lsec_202_c
        l8ui     a3, a3, 0
        bnez.n   a3, .Lsec_202_a9
.Lsec_202_a1:
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, set_chan_dig_gain
        l32r     a8, .Lsec_202_2c
        callx8   a8
.Lsec_202_a9:
        retw.n
        .size   chip_v7_set_chan_misc, .-chip_v7_set_chan_misc
.Lsec_202_ab:

        .section .text.set_rx_gain_table,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_203_0, wifi_bbgain_max
        .literal .Lsec_203_4, .Lsec_137_88
        .literal .Lsec_203_8, .Lsec_137_94
        .literal .Lsec_203_c, .Lsec_304_0
        .literal .Lsec_203_10, .Lsec_305_0
        .literal .Lsec_203_14, chip7_sleep_params
        .literal .Lsec_203_18, g_phyFuns
        .literal .Lsec_203_1c, chip7_phy_init_ctrl
        .literal .Lsec_203_20, .Lsec_303_0
        .literal .Lsec_203_24, 0x00020000
        .literal .Lsec_203_28, phy_rxrf_dc+0x50
        .literal .Lsec_203_2c, phy_rxbb_dc
        .literal .Lsec_203_30, phy_rxrf_dc
        .literal .Lsec_203_34, phy_rxrf_dc+0x24
        .literal .Lsec_203_38, 0x3ff5c02c
        .literal .Lsec_203_3c, 0xffff80ff
        .literal .Lsec_203_40, 0x3ff5c0a4
        .literal .Lsec_203_44, 0xffc07fff
        .literal .Lsec_203_48, 0x3ff450dc
        .literal .Lsec_203_4c, 0x00000800
        .literal .Lsec_203_50, 0x00001000
        .literal .Lsec_203_54, 0x08000000
        .literal .Lsec_203_58, 0x10000000
        .literal .Lsec_203_5c, memcpy
        .literal .Lsec_203_60, memcpy
        .literal .Lsec_203_64, ram_gen_rx_gain_table
        .literal .Lsec_203_68, ram_gen_rx_gain_table
        .literal .Lsec_203_6c, set_rx_gain_testchip_70
        .literal .Lsec_203_70, set_rx_gain_testchip_70
        .literal .Lsec_203_74, wr_rx_gain_mem
        .literal .Lsec_203_78, wr_rx_gain_mem
        .literal .Lsec_203_7c, esp_dport_access_reg_read
        .literal .Lsec_203_80, esp_dport_access_reg_read
        .literal .Lsec_203_84, esp_dport_access_reg_read
        .literal .Lsec_203_88, esp_dport_access_reg_read
        .literal .Lsec_203_8c, esp_dport_access_reg_read
        .literal .Lsec_203_90, esp_dport_access_reg_read
.Lsec_203_94:
        .global set_rx_gain_table
        .type   set_rx_gain_table, @function
set_rx_gain_table:
        entry    a1, 0x1a0
        extui    a3, a3, 0, 8
        movi.n   a5, 12
        sub      a5, a5, a3
        extui    a2, a2, 0, 16
        extui    a5, a5, 0, 8
        movi.n   a4, 22
        bltui    a3, 3, .Lsec_203_ac
        movi.n   a4, 28
.Lsec_203_ac:
        addi     a6, a1, 16
        addmi    a3, a6, 0x100
        l32r     a9, .Lsec_203_0
        movi.n   a12, 12
        addi     a10, a3, 96
        movi     a7, 0x200
        l32r     a11, .Lsec_203_4
        s8i      a4, a9, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_203_5c
        callx8   a8
        addi     a10, a3, 84
        movi.n   a12, 12
        l32r     a11, .Lsec_203_8
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_203_60
        callx8   a8
        l32r     a3, .Lsec_203_10
        add.n    a8, a3, a5
        addi.n   a8, a8, -1
        l32r     a4, .Lsec_203_14
        l8ui     a10, a8, 0
        l32i.n   a11, a4, 0
        and      a6, a11, a7
        l32r     a9, .Lsec_203_c
        s8i      a10, a9, 0
        memw
        bnez     a6, .Lsec_203_15c
        mov.n    a11, a2
        mov.n    a12, a6
        nop.n
        nop.n
        l32r     a2, .Lsec_203_18
        l32r     a9, .Lsec_203_1c
        l32i.n   a2, a2, 0
        l8ui     a10, a9, 1
        l32i     a2, a2, 252
        callx8   a2
        movi     a13, 0x154
        addi     a2, a1, 16
        l32r     a12, .Lsec_203_20
        add.n    a13, a2, a13
        movi.n   a15, 6
        movi.n   a14, 9
        movi.n   a11, 28
        mov.n    a10, a2
        s32i.n   a6, a1, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, ram_gen_rx_gain_table
        l32r     a8, .Lsec_203_64
        callx8   a8
        addi     a2, a1, 16
        movi     a13, 0x160
        mov.n    a15, a6
        l32r     a9, .Lsec_203_0
        mov.n    a14, a5
        add.n    a13, a2, a13
        mov.n    a12, a3
        l8ui     a11, a9, 0
        s8i      a10, a4, 177
        movi     a10, 160
        add      a10, a2, a10
        s32i     a6, a1, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, ram_gen_rx_gain_table
        l32r     a8, .Lsec_203_68
        callx8   a8
        l32i.n   a11, a4, 0
        or       a11, a7, a11
        s8i      a10, a4, 176
        s32i.n   a11, a4, 0
.Lsec_203_15c:
        movi     a2, 0x100
        bany     a11, a2, .Lsec_203_186
        mov.n    a11, a4
        movi.n   a15, 9
        nop.n
        l8ui     a13, a4, 177
        addi     a14, a1, 16
        addi.n   a13, a13, 1
        extui    a13, a13, 0, 8
        movi.n   a10, 1
        l32r     a12, .Lsec_203_20
        .reloc  ., R_XTENSA_ASM_EXPAND, set_rx_gain_testchip_70
        l32r     a8, .Lsec_203_6c
        callx8   a8
        l32i.n   a11, a4, 0
        or       a11, a2, a11
        s32i     a11, a4, 0
.Lsec_203_186:
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a2, .Lsec_203_24
        and      a10, a11, a2
        bnez.n   a10, .Lsec_203_1ba
        mov.n    a15, a5
        l8ui     a13, a4, 176
        movi     a14, 160
        addi.n   a13, a13, 1
        addi     a5, a1, 16
        mov.n    a11, a4
        add.n    a14, a5, a14
        extui    a13, a13, 0, 8
        mov.n    a12, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, set_rx_gain_testchip_70
        l32r     a8, .Lsec_203_70
        callx8   a8
        l32i.n   a11, a4, 0
        or       a11, a11, a2
        s32i     a11, a4, 0
.Lsec_203_1ba:
        movi.n   a5, 32
        and      a2, a11, a5
        bnez     a2, .Lsec_203_21e
        movi.n   a10, 1
        nop.n
        l8ui     a8, a4, 177
        addi     a9, a1, 16
        add.n    a8, a8, a10
        extui    a8, a8, 0, 8
        mov.n    a11, a2
        l32r     a6, .Lsec_203_28
        mov.n    a15, a6
        l32r     a7, .Lsec_203_2c
        mov.n    a14, a7
        l32r     a13, .Lsec_203_30
        l32r     a12, .Lsec_203_20
        s32i.n   a9, a1, 4
        s32i     a8, a1, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, wr_rx_gain_mem
        l32r     a8, .Lsec_203_74
        callx8   a8
        mov.n    a15, a6
        addi     a9, a1, 16
        movi     a8, 160
        l8ui     a6, a4, 176
        add.n    a8, a9, a8
        addi.n   a6, a6, 1
        extui    a6, a6, 0, 8
        mov.n    a11, a2
        l32r     a13, .Lsec_203_34
        mov.n    a10, a2
        mov.n    a14, a7
        mov.n    a12, a3
        s32i.n   a8, a1, 4
        s32i     a6, a1, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, wr_rx_gain_mem
        l32r     a8, .Lsec_203_78
        callx8   a8
        l32i.n   a2, a4, 0
        or       a5, a2, a5
        s32i     a5, a4, 0
.Lsec_203_21e:
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a6, .Lsec_203_38
        mov.n    a10, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_203_7c
        callx8   a8
        l8ui     a2, a4, 176
        extui    a2, a2, 0, 7
        slli     a2, a2, 8
        l32r     a3, .Lsec_203_3c
        and      a10, a10, a3
        or       a2, a2, a10
        l32r     a5, .Lsec_203_40
        or       a10, a5, a5
        memw
        s32i     a2, a6, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_203_80
        callx8   a8
        l8ui     a3, a4, 177
        extui    a3, a3, 0, 7
        slli     a3, a3, 15
        l32r     a2, .Lsec_203_44
        and      a10, a10, a2
        or       a3, a3, a10
        l32r     a2, .Lsec_203_48
        mov.n    a10, a2
        memw
        s32i     a3, a5, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_203_84
        callx8   a8
        l32r     a3, .Lsec_203_4c
        or       a3, a10, a3
        or       a10, a2, a2
        memw
        s32i     a3, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_203_88
        callx8   a8
        l32r     a3, .Lsec_203_50
        or       a3, a10, a3
        mov.n    a10, a2
        memw
        s32i     a3, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_203_8c
        callx8   a8
        l32r     a3, .Lsec_203_54
        or       a3, a10, a3
        mov.n    a10, a2
        memw
        s32i.n   a3, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_203_90
        callx8   a8
        l32r     a3, .Lsec_203_58
        or       a10, a10, a3
        memw
        s32i.n   a10, a2, 0
        retw.n
        .size   set_rx_gain_table, .-set_rx_gain_table
.Lsec_203_2c1:

        .section .text.rx_blocking_set,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_204_0, chip7_sleep_params
        .literal .Lsec_204_4, 0xfffdfddf
        .literal .Lsec_204_8, 0x00000985
        .literal .Lsec_204_c, set_rx_gain_table
.Lsec_204_10:
        .global rx_blocking_set
        .type   rx_blocking_set, @function
rx_blocking_set:
        entry    a1, 32
        l32r     a9, .Lsec_204_0
        extui    a11, a2, 0, 8
        l32i.n   a10, a9, 0
        l32r     a8, .Lsec_204_4
        and      a8, a10, a8
        l32r     a10, .Lsec_204_8
        s32i.n   a8, a9, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, set_rx_gain_table
        l32r     a8, .Lsec_204_c
        callx8   a8
        retw.n
        .size   rx_blocking_set, .-rx_blocking_set
.Lsec_204_2e:

        .section .text.rx_gain_level,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_205_0, rx_blocking_set
.Lsec_205_4:
        .global rx_gain_level
        .type   rx_gain_level, @function
rx_gain_level:
        entry    a1, 32
        extui    a10, a2, 0, 8
        beqz     a10, .Lsec_205_10
        movi     a10, 3
.Lsec_205_10:
        .reloc  ., R_XTENSA_ASM_EXPAND, rx_blocking_set
        l32r     a8, .Lsec_205_0
        callx8   a8
        retw.n
        .size   rx_gain_level, .-rx_gain_level
.Lsec_205_18:

        .section .text.txiq_cal_init,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_206_0, chip7_sleep_params
        .literal .Lsec_206_4, 0x00004000
        .literal .Lsec_206_8, tx_rf_ana_gain
        .literal .Lsec_206_c, chip7_sleep_params+0x4c
        .literal .Lsec_206_10, rfcal_txiq
        .literal .Lsec_206_14, rfcal_txiq
.Lsec_206_18:
        .global txiq_cal_init
        .type   txiq_cal_init, @function
txiq_cal_init:
        entry    a1, 64
        l32r     a7, .Lsec_206_0
        l32r     a3, .Lsec_206_4
        l32i.n   a2, a7, 0
        and      a2, a2, a3
        bnez     a2, .Lsec_206_f2
        movi.n   a5, 16
        l8ui     a3, a7, 116
        movi.n   a4, 4
        sext     a3, a3, 7
        addi     a3, a3, 43
        max      a3, a3, a2
        extui    a3, a3, 0, 16
        s32i.n   a3, a1, 16
.Lsec_206_3e:
        movi.n   a3, 0
        mov.n    a11, a4
        movi.n   a6, 32
        movi.n   a8, 48
        movnez   a11, a5, a2
        movnez   a6, a8, a2
        mov.n    a10, a3
        movi     a9, 6
        loop     a9, .Lsec_206_5e
        addi.n   a8, a10, 6
        bbc      a6, a3, .Lsec_206_5c
        extui    a10, a8, 0, 8
.Lsec_206_5c:
        addi.n   a3, a3, 1
.Lsec_206_5e:
        l32r     a3, .Lsec_206_8
        movi.n   a9, 6
        l16ui    a13, a3, 2
        movi.n   a3, 0
        extui    a14, a13, 6, 8
        or       a12, a3, a3
        extui    a13, a13, 0, 6
        loop     a9, .Lsec_206_7e
        addi.n   a8, a12, 6
        bbc      a13, a3, .Lsec_206_7c
        extui    a12, a8, 0, 8
.Lsec_206_7c:
        addi.n   a3, a3, 1
.Lsec_206_7e:
        add.n    a3, a14, a12
        extui    a3, a3, 0, 8
        l32i.n   a8, a1, 16
        sub      a3, a10, a3
        addx4    a3, a3, a8
        sext     a3, a3, 15
        bltz     a3, .Lsec_206_9c
        movi     a14, 120
        blt      a14, a3, .Lsec_206_a5
        mov.n    a14, a3
        j        .Lsec_206_a7
.Lsec_206_9c:
        movi.n   a14, 0
        mov.n    a3, a14
        j        .Lsec_206_a7
        .byte    0x00
        .byte    0x00
.Lsec_206_a5:
        mov.n    a3, a14
.Lsec_206_a7:
        addi     a12, a2, 26
        addx2    a11, a11, a7
        addi.n   a2, a2, 1
        movi.n   a15, 0
        movi     a13, 128
        addx2    a12, a12, a7
        addi.n   a11, a11, 12
        mov.n    a10, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, rfcal_txiq
        l32r     a8, .Lsec_206_10
        callx8   a8
        beqi     a2, 2, .Lsec_206_c7
        j        .Lsec_206_3e
.Lsec_206_c7:
        addi     a8, a3, -40
        mov.n    a4, a6
        sext     a8, a8, 15
        mov.n    a6, a2
        l32r     a12, .Lsec_206_c
        movi.n   a14, 0
        mov.n    a15, a6
        max      a14, a8, a14
        movi     a13, 128
        mov.n    a11, a1
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, rfcal_txiq
        l32r     a8, .Lsec_206_14
        callx8   a8
        l32i.n   a2, a7, 0
        l32r     a3, .Lsec_206_4
        or       a2, a2, a3
        s32i.n   a2, a7, 0
.Lsec_206_f2:
        retw.n
        .size   txiq_cal_init, .-txiq_cal_init
.Lsec_206_f4:

        .section .text.phy_rx11blr_cfg,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_207_0, 0x3ff5c860
        .literal .Lsec_207_4, 0x00000800
        .literal .Lsec_207_8, 0xfffff7ff
        .literal .Lsec_207_c, 0x3ff5c87c
        .literal .Lsec_207_10, esp_dport_access_reg_read
        .literal .Lsec_207_14, esp_dport_access_reg_read
        .literal .Lsec_207_18, esp_dport_access_reg_read
.Lsec_207_1c:
        .global phy_rx11blr_cfg
        .type   phy_rx11blr_cfg, @function
phy_rx11blr_cfg:
        entry    a1, 32
        l32r     a3, .Lsec_207_0
        extui    a2, a2, 0, 8
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_207_10
        callx8   a8
        slli     a5, a2, 10
        movi     a8, 0xfffffbff
        movi     a4, 0x400
        and      a10, a10, a8
        and      a8, a5, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a3, 0
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_207_14
        callx8   a8
        slli     a2, a2, 11
        l32r     a4, .Lsec_207_8
        and      a10, a10, a4
        l32r     a8, .Lsec_207_4
        and      a2, a2, a8
        or       a8, a10, a2
        l32r     a5, .Lsec_207_c
        or       a10, a5, a5
        memw
        s32i     a8, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_207_18
        callx8   a8
        and      a10, a10, a4
        or       a2, a10, a2
        memw
        s32i.n   a2, a5, 0
        retw.n
        .size   phy_rx11blr_cfg, .-phy_rx11blr_cfg
.Lsec_207_7d:

        .section .text.analog_gain_init,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_208_0, .Lsec_137_a0
        .literal .Lsec_208_4, g_phyFuns
.Lsec_208_8:
        .global analog_gain_init
        .type   analog_gain_init, @function
analog_gain_init:
        entry    a1, 48
        l32r     a2, .Lsec_208_4
        l32r     a3, .Lsec_208_0
        l32i.n   a12, a2, 0
        l16ui    a9, a3, 4
        l16ui    a8, a3, 6
        l16ui    a11, a3, 0
        l16ui    a10, a3, 2
        l32i     a3, a12, 192
        s16i     a9, a1, 4
        s16i     a8, a1, 6
        s16i     a11, a1, 0
        s16i     a10, a1, 2
        memw
        callx8   a3
        movi.n   a12, 0
        movi.n   a11, 1
        movi.n   a10, 4
        l32i.n   a3, a2, 0
        l32i     a3, a3, 184
        callx8   a3
        movi.n   a12, 0
        movi.n   a11, 1
        l32i.n   a3, a2, 0
        movi     a10, 5
        l32i     a3, a3, 184
        callx8   a3
        movi     a12, 0x184
        movi.n   a11, 1
        l32i.n   a3, a2, 0
        movi.n   a10, 0
        l32i     a3, a3, 184
        callx8   a3
        movi.n   a3, 1
        j        .Lsec_208_6a
.Lsec_208_65:
        addi.n   a3, a3, 1
        extui    a3, a3, 0, 8
.Lsec_208_6a:
        l32i.n   a8, a2, 0
        movi.n   a11, 1
        movi     a12, 0x18b
        mov.n    a10, a11
        l32i     a8, a8, 184
        beq      a3, a11, .Lsec_208_a2
        callx8   a8
        movi.n   a10, 1
        movi.n   a12, 0
        l32i.n   a8, a2, 0
        movi.n   a11, 2
        l32i     a8, a8, 184
        callx8   a8
        mov.n    a10, a1
        l32i     a8, a2, 0
        l32i     a8, a8, 220
        callx8   a8
        bnei     a3, 2, .Lsec_208_65
        l32i.n   a2, a2, 0
        l32i     a2, a2, 196
        callx8   a2
        retw.n
.Lsec_208_a2:
        mov.n    a11, a3
        mov.n    a10, a3
        movi     a12, 0x189
        callx8   a8
        l32i.n   a8, a2, 0
        mov.n    a10, a3
        movi.n   a12, 0
        movi.n   a11, 2
        l32i     a8, a8, 184
        callx8   a8
        mov.n    a10, a1
        l32i.n   a8, a2, 0
        l32i     a8, a8, 220
        callx8   a8
        j        .Lsec_208_65
        .size   analog_gain_init, .-analog_gain_init
.Lsec_208_c7:

        .section .text.phy_param_set,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_209_0, wifi_txband
        .literal .Lsec_209_4, wifi_txband_ht40
.Lsec_209_8:
        .global phy_param_set
        .type   phy_param_set, @function
phy_param_set:
        entry    a1, 32
        extui    a2, a2, 0, 8
        bnez.n   a2, .Lsec_209_30
        l32r     a2, .Lsec_209_0
        movi.n   a8, 95
        s8i      a8, a2, 0
        memw
        movi     a8, -61
        nop.n
        nop.n
        nop.n
        l32r     a2, .Lsec_209_4
        s8i      a8, a2, 0
        memw
        retw.n
        .byte    0x00
.Lsec_209_30:
        l32r     a2, .Lsec_209_0
        movi.n   a8, 75
        s8i      a8, a2, 0
        memw
        movi     a8, -106
        nop.n
        nop.n
        nop.n
        l32r     a2, .Lsec_209_4
        s8i      a8, a2, 0
        memw
        retw.n
        .size   phy_param_set, .-phy_param_set
.Lsec_209_4f:

        .section .text.bb_init,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_210_0, chip7_sleep_params
        .literal .Lsec_210_4, 0x00010000
        .literal .Lsec_210_8, g_phyFuns
        .literal .Lsec_210_c, 0x00080000
        .literal .Lsec_210_10, tx_rf_ana_gain
        .literal .Lsec_210_14, chip7_sleep_params+0xc
        .literal .Lsec_210_18, re_entry
        .literal .Lsec_210_1c, phy_init_flag
        .literal .Lsec_210_20, 0x00000985
        .literal .Lsec_210_24, rc_cal
        .literal .Lsec_210_28, tx_cap_init
        .literal .Lsec_210_2c, txpwr_offset
        .literal .Lsec_210_30, tx_pwctrl_init
        .literal .Lsec_210_34, txiq_cal_init
        .literal .Lsec_210_38, set_tx_gain_table
        .literal .Lsec_210_3c, write_txrate_power_offset
        .literal .Lsec_210_40, set_tx_gain_table_bt
        .literal .Lsec_210_44, tsens_read_init
        .literal .Lsec_210_48, set_rx_gain_table
        .literal .Lsec_210_4c, analog_gain_init
        .literal .Lsec_210_50, force_txrxoff
        .literal .Lsec_210_54, phy_reg_init
        .literal .Lsec_210_58, chip_v7_set_chan
.Lsec_210_5c:
        .global bb_init
        .type   bb_init, @function
bb_init:
        entry    a1, 32
        l32r     a2, .Lsec_210_0
        l32r     a3, .Lsec_210_4
        l32i.n   a8, a2, 0
        bany     a8, a3, .Lsec_210_7c
        l32r     a8, .Lsec_210_8
        l32i.n   a8, a8, 0
        l32i     a8, a8, 48
        callx8   a8
        l32i.n   a8, a2, 0
        or       a3, a8, a3
        s32i.n   a3, a2, 0
.Lsec_210_7c:
        .reloc  ., R_XTENSA_ASM_EXPAND, rc_cal
        l32r     a8, .Lsec_210_24
        callx8   a8
        l32i.n   a13, a2, 0
        l32r     a3, .Lsec_210_c
        and      a13, a13, a3
        bnez     a13, .Lsec_210_ac
        l32r     a9, .Lsec_210_8
        l32r     a8, .Lsec_210_10
        l32i     a9, a9, 0
        l16ui    a12, a8, 2
        l16ui    a11, a8, 0
        l32i     a9, a9, 0x114
        l32r     a10, .Lsec_210_14
        callx8   a9
        l32i.n   a8, a2, 0
        or       a3, a8, a3
        s32i.n   a3, a2, 0
.Lsec_210_ac:
        .reloc  ., R_XTENSA_ASM_EXPAND, tx_cap_init
        l32r     a8, .Lsec_210_28
        callx8   a8
        movi.n   a10, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, txpwr_offset
        l32r     a8, .Lsec_210_2c
        callx8   a8
        movi.n   a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, tx_pwctrl_init
        l32r     a8, .Lsec_210_30
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, txiq_cal_init
        l32r     a8, .Lsec_210_34
        callx8   a8
        l32r     a3, .Lsec_210_18
        l8ui     a3, a3, 0
        beqz.n   a3, .Lsec_210_d9
        l32r     a3, .Lsec_210_1c
        l8ui     a3, a3, 0
        bnez     a3, .Lsec_210_f7
.Lsec_210_d9:
        l32r     a3, .Lsec_210_10
        l16ui    a11, a3, 2
        l16ui    a10, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, set_tx_gain_table
        l32r     a8, .Lsec_210_38
        callx8   a8
        movi     a10, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, write_txrate_power_offset
        l32r     a8, .Lsec_210_3c
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, set_tx_gain_table_bt
        l32r     a8, .Lsec_210_40
        callx8   a8
.Lsec_210_f7:
        .reloc  ., R_XTENSA_ASM_EXPAND, tsens_read_init
        l32r     a8, .Lsec_210_44
        callx8   a8
        l32r     a10, .Lsec_210_20
        movi.n   a11, 3
        .reloc  ., R_XTENSA_ASM_EXPAND, set_rx_gain_table
        l32r     a8, .Lsec_210_48
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, analog_gain_init
        l32r     a8, .Lsec_210_4c
        callx8   a8
        movi.n   a10, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, force_txrxoff
        l32r     a8, .Lsec_210_50
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_reg_init
        l32r     a8, .Lsec_210_54
        callx8   a8
        movi     a8, 0xfffffe80
        movi.n   a11, 0
        movi.n   a10, 11
        s16i     a8, a2, 78
        s16i     a8, a2, 178
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, chip_v7_set_chan
        l32r     a8, .Lsec_210_58
        callx8   a8
        retw.n
        .size   bb_init, .-bb_init
.Lsec_210_134:

        .section .text.wifi_rifs_mode_en,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_211_0, 0x3ff5c0f4
        .literal .Lsec_211_4, esp_dport_access_reg_read
.Lsec_211_8:
        .global wifi_rifs_mode_en
        .type   wifi_rifs_mode_en, @function
wifi_rifs_mode_en:
        entry    a1, 32
        l32r     a3, .Lsec_211_0
        extui    a2, a2, 0, 8
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_211_4
        callx8   a8
        movi.n   a8, -2
        and      a10, a10, a8
        extui    a2, a2, 0, 1
        or       a2, a10, a2
        memw
        s32i.n   a2, a3, 0
        retw.n
        .size   wifi_rifs_mode_en, .-wifi_rifs_mode_en
.Lsec_211_2b:

        .section .text.phy_chan_filt_set,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_212_0, 0x3ff5cc0c
        .literal .Lsec_212_4, 0xffdfffff
        .literal .Lsec_212_8, 0xffbfffff
        .literal .Lsec_212_c, 0x00200000
        .literal .Lsec_212_10, 0x00400000
        .literal .Lsec_212_14, 0x3ff5cd08
        .literal .Lsec_212_18, 0x3ff5cd04
        .literal .Lsec_212_1c, 0x3ff5c074
        .literal .Lsec_212_20, 0xffffdfff
        .literal .Lsec_212_24, 0x00002000
        .literal .Lsec_212_28, esp_dport_access_reg_read
        .literal .Lsec_212_2c, esp_dport_access_reg_read
        .literal .Lsec_212_30, esp_dport_access_reg_read
        .literal .Lsec_212_34, esp_dport_access_reg_read
        .literal .Lsec_212_38, esp_dport_access_reg_read
        .literal .Lsec_212_3c, esp_dport_access_reg_read
        .literal .Lsec_212_40, esp_dport_access_reg_read
        .literal .Lsec_212_44, esp_dport_access_reg_read
.Lsec_212_48:
        .global phy_chan_filt_set
        .type   phy_chan_filt_set, @function
phy_chan_filt_set:
        entry    a1, 32
        extui    a2, a2, 0, 8
        extui    a3, a3, 0, 8
        beqz     a2, .Lsec_212_85
        l32r     a2, .Lsec_212_0
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_212_28
        callx8   a8
        l32r     a8, .Lsec_212_4
        and      a8, a10, a8
        mov.n    a10, a2
        memw
        s32i     a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_212_2c
        callx8   a8
        l32r     a8, .Lsec_212_8
        and      a10, a10, a8
        memw
        s32i.n   a10, a2, 0
        j        .Lsec_212_d8
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_212_85:
        l32r     a2, .Lsec_212_0
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_212_30
        callx8   a8
        l32r     a8, .Lsec_212_c
        or       a8, a10, a8
        mov.n    a10, a2
        memw
        s32i.n   a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_212_34
        callx8   a8
        l32r     a8, .Lsec_212_10
        or       a8, a10, a8
        l32r     a5, .Lsec_212_14
        mov.n    a10, a5
        memw
        s32i.n   a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_212_38
        callx8   a8
        movi.n   a2, -8
        and      a8, a10, a2
        l32r     a4, .Lsec_212_18
        mov.n    a10, a4
        memw
        s32i     a8, a5, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_212_3c
        callx8   a8
        and      a10, a10, a2
        memw
        s32i.n   a10, a4, 0
.Lsec_212_d8:
        beqz.n   a3, .Lsec_212_fd
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a3, .Lsec_212_1c
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_212_40
        callx8   a8
        l32r     a2, .Lsec_212_20
        and      a10, a10, a2
        memw
        s32i.n   a10, a3, 0
        retw.n
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_212_fd:
        l32r     a3, .Lsec_212_1c
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_212_44
        callx8   a8
        l32r     a2, .Lsec_212_24
        or       a10, a10, a2
        memw
        s32i.n   a10, a3, 0
        retw.n
        .size   phy_chan_filt_set, .-phy_chan_filt_set
.Lsec_212_115:

        .section .text.phy_get_tx_rate,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_213_0, 0x3ff4e060
        .literal .Lsec_213_4, esp_dport_access_reg_read
.Lsec_213_8:
        .global phy_get_tx_rate
        .type   phy_get_tx_rate, @function
phy_get_tx_rate:
        entry    a1, 32
        l32r     a10, .Lsec_213_0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_213_4
        callx8   a8
        extui    a2, a10, 24, 7
        retw.n
        .size   phy_get_tx_rate, .-phy_get_tx_rate
.Lsec_213_19:

        .section .text.register_chipv7_phy_init_param,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_214_0, chip7_phy_init_ctrl
        .literal .Lsec_214_4, chip7_phy_init_ctrl+0xe
        .literal .Lsec_214_8, chip7_phy_init_ctrl+0x20
        .literal .Lsec_214_c, chip7_phy_init_ctrl+0x50
        .literal .Lsec_214_10, rtc_get_xtal
.Lsec_214_14:
        .global register_chipv7_phy_init_param
        .type   register_chipv7_phy_init_param, @function
register_chipv7_phy_init_param:
        entry    a1, 32
        l32r     a3, .Lsec_214_0
        l8ui     a8, a2, 0
        l8ui     a4, a2, 1
        s8i      a8, a3, 0
        memw
        bnei     a4, 3, .Lsec_214_48
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_get_xtal
        l32r     a8, .Lsec_214_10
        callx8   a8
        extui    a10, a10, 0, 8
        movi.n   a8, 24
        movi.n   a4, 2
        beq      a10, a8, .Lsec_214_48
        addi     a10, a10, -26
        movi     a4, 0
        movi     a8, 1
        moveqz   a4, a8, a10
        extui    a4, a4, 0, 8
.Lsec_214_48:
        s8i      a4, a3, 1
        memw
        addi     a10, a2, 32
        mov.n    a11, a3
        addi     a13, a2, 35
        or       a9, a3, a3
        movi     a12, -30
.Lsec_214_5c:
        l8ui     a8, a10, 0
        addi.n   a9, a9, 1
        addi.n   a10, a10, 1
        s8i      a8, a9, 1
        memw
        nop.n
        nop.n
        nop.n
        nop.n
        l8ui     a4, a10, 2
        add.n    a4, a8, a4
        s8i      a4, a9, 4
        s8i      a12, a9, 7
        memw
        bne      a10, a13, .Lsec_214_5c
        addi     a10, a2, 44
        addi     a8, a2, 50
        movi.n   a9, 6
        nop
        l8ui     a14, a2, 41
        l8ui     a13, a2, 42
        l8ui     a12, a2, 43
        l32r     a4, .Lsec_214_4
        s8i      a14, a3, 11
        s8i      a13, a3, 12
        s8i      a12, a3, 13
        memw
        loop     a9, .Lsec_214_bd
        nop.n
        nop.n
        nop.n
        nop.n
        l8ui     a12, a10, 0
        addi.n   a10, a10, 1
        s8i      a12, a4, 0
        memw
        addi.n   a4, a4, 1
.Lsec_214_bd:
        addi     a4, a2, 58
        mov.n    a9, a11
        sub      a4, a4, a8
        loop     a4, .Lsec_214_d5
        l8ui     a10, a8, 0
        addi.n   a9, a9, 1
        addi.n   a8, a8, 1
        s8i      a10, a9, 19
        memw
.Lsec_214_d5:
        addi     a9, a2, 62
        movi.n   a8, 14
        nop.n
        nop
        l8ui     a14, a2, 58
        l8ui     a13, a2, 59
        l8ui     a12, a2, 60
        l8ui     a10, a2, 61
        l32r     a4, .Lsec_214_8
        s8i      a14, a3, 28
        s8i      a13, a3, 29
        s8i      a12, a3, 30
        s8i      a10, a3, 31
        memw
        loop     a8, .Lsec_214_115
        nop.n
        nop.n
        nop.n
        nop.n
        l8ui     a10, a9, 0
        addi.n   a9, a9, 1
        s8i      a10, a4, 0
        memw
        addi.n   a4, a4, 1
.Lsec_214_115:
        addi     a9, a2, 76
        mov.n    a8, a11
        movi     a4, 5
        loop     a4, .Lsec_214_12d
        l8ui     a10, a9, 0
        addi.n   a8, a8, 1
        addi.n   a9, a9, 1
        s8i      a10, a8, 45
        memw
.Lsec_214_12d:
        addi     a8, a2, 82
        movi.n   a4, 4
        nop.n
        nop.n
        l8ui     a9, a2, 81
        s8i      a9, a3, 51
        memw
        loop     a4, .Lsec_214_155
        addi.n   a11, a11, 1
        nop.n
        nop.n
        nop.n
        l8ui     a9, a8, 0
        addi.n   a8, a8, 1
        s8i      a9, a11, 51
        memw
.Lsec_214_155:
        addi     a10, a2, 107
        movi.n   a9, 14
        nop.n
        nop.n
        l8ui     a11, a2, 103
        extui    a11, a11, 0, 7
        slli     a8, a11, 8
        l8ui     a4, a2, 90
        sext     a4, a4, 7
        slli     a4, a4, 8
        l8ui     a11, a2, 91
        or       a4, a4, a11
        l8ui     a15, a2, 93
        extui    a15, a15, 0, 7
        slli     a15, a15, 8
        l8ui     a11, a2, 94
        or       a15, a15, a11
        l8ui     a14, a2, 95
        sext     a14, a14, 7
        slli     a14, a14, 8
        l8ui     a11, a2, 96
        or       a14, a14, a11
        l8ui     a13, a2, 98
        extui    a13, a13, 0, 7
        slli     a13, a13, 8
        l8ui     a5, a2, 92
        l8ui     a11, a2, 99
        or       a13, a13, a11
        l8ui     a12, a2, 100
        sext     a12, a12, 7
        slli     a12, a12, 8
        s8i      a5, a3, 62
        memw
        nop.n
        nop.n
        nop.n
        nop.n
        l8ui     a5, a2, 97
        l8ui     a11, a2, 101
        or       a12, a12, a11
        s8i      a5, a3, 68
        memw
        nop.n
        nop.n
        nop.n
        nop.n
        l8ui     a5, a2, 102
        l8ui     a11, a2, 104
        or       a11, a8, a11
        s8i      a5, a3, 74
        memw
        nop.n
        nop.n
        nop.n
        nop.n
        l8ui     a5, a2, 105
        l8ui     a2, a2, 106
        l32r     a8, .Lsec_214_c
        s8i      a5, a3, 78
        s16i     a4, a3, 60
        s16i     a15, a3, 64
        s16i     a14, a3, 66
        s16i     a13, a3, 70
        s16i     a12, a3, 72
        s16i     a11, a3, 76
        s8i      a2, a3, 79
        memw
        loop     a9, .Lsec_214_227
        nop.n
        nop.n
        nop.n
        nop.n
        l8ui     a2, a10, 0
        addi.n   a10, a10, 1
        s8i      a2, a8, 0
        memw
        addi.n   a8, a8, 1
.Lsec_214_227:
        retw.n
        .size   register_chipv7_phy_init_param, .-register_chipv7_phy_init_param
.Lsec_214_229:

        .section .text.uart_wait_idle,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_215_0, 0x00003ff4
        .literal .Lsec_215_4, 0x00ff0000
        .literal .Lsec_215_8, esp_dport_access_reg_read
        .literal .Lsec_215_c, ets_delay_us
.Lsec_215_10:
        .global uart_wait_idle
        .type   uart_wait_idle, @function
uart_wait_idle:
        entry    a1, 32
        l32r     a8, .Lsec_215_0
        extui    a2, a2, 0, 8
        add.n    a2, a2, a8
        slli     a2, a2, 16
        addi     a2, a2, 28
        l32r     a3, .Lsec_215_4
.Lsec_215_24:
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_215_8
        callx8   a8
        bany     a10, a3, .Lsec_215_24
        movi     a10, 100
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_215_c
        callx8   a8
        retw.n
        .size   uart_wait_idle, .-uart_wait_idle
.Lsec_215_3a:

        .section .text.phy_get_romfunc_addr,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_216_0, g_phyFuns
        .literal .Lsec_216_4, ram_pbus_force_mode
        .literal .Lsec_216_8, ram_rfpll_reset
        .literal .Lsec_216_c, ram_restart_cal
        .literal .Lsec_216_10, ram_wait_rfpll_cal_end
        .literal .Lsec_216_14, ram_phy_get_vdd33
        .literal .Lsec_216_18, ram_set_chan_cal_interp
        .literal .Lsec_216_1c, ram_pbus_rx_dco_cal
        .literal .Lsec_216_20, ram_txdc_cal_v70
        .literal .Lsec_216_24, ram_spur_coef_cfg
        .literal .Lsec_216_28, ram_set_pbus_mem
        .literal .Lsec_216_2c, ram_write_gain_mem
        .literal .Lsec_216_30, ram_bb_bss_bw_40_en
        .literal .Lsec_216_34, ram_bb_tx_ht20_cen
        .literal .Lsec_216_38, ram_bb_bss_cbw40_dig
        .literal .Lsec_216_3c, ram_cbw2040_cfg
        .literal .Lsec_216_40, ram_check_noise_floor
        .literal .Lsec_216_44, ram_phy_get_noisefloor
        .literal .Lsec_216_48, ram_set_noise_floor
        .literal .Lsec_216_4c, ram_disable_agc
        .literal .Lsec_216_50, ram_enable_agc
        .literal .Lsec_216_54, ram_dc_iq_est
        .literal .Lsec_216_58, ram_tx_pwctrl_bg_init
        .literal .Lsec_216_5c, ram_start_tx_tone
        .literal .Lsec_216_60, ram_get_fm_sar_dout
        .literal .Lsec_216_64, ram_txiq_get_mis_pwr
        .literal .Lsec_216_68, ram_iq_est_enable
        .literal .Lsec_216_6c, ram_iq_est_disable
        .literal .Lsec_216_70, ram_rfcal_pwrctrl
        .literal .Lsec_216_74, ram_en_pwdet
        .literal .Lsec_216_78, ram_read_sar_dout
        .literal .Lsec_216_7c, ram_pbus_xpd_tx_on
        .literal .Lsec_216_80, ram_rfcal_txcap
        .literal .Lsec_216_84, ram_txbbgain_to_index
        .literal .Lsec_216_88, ram_index_to_txbbgain
        .literal .Lsec_216_8c, ram_chip_i2c_readReg
        .literal .Lsec_216_90, ram_chip_i2c_writeReg
        .literal .Lsec_216_94, ram_txcal_work_mode
        .literal .Lsec_216_98, ram_set_txcap_reg
        .literal .Lsec_216_9c, ram_meas_tone_pwr_db
        .literal .Lsec_216_a0, ram_pbus_debugmode
        .literal .Lsec_216_a4, ram_pbus_force_test
        .literal .Lsec_216_a8, ram_set_txclk_en
        .literal .Lsec_216_ac, ram_set_rxclk_en
        .literal .Lsec_216_b0, ram_rfpll_set_freq
        .literal .Lsec_216_b4, ram_get_rf_gain_qdb
        .literal .Lsec_216_b8, phy_get_romfuncs
.Lsec_216_bc:
        .global phy_get_romfunc_addr
        .type   phy_get_romfunc_addr, @function
phy_get_romfunc_addr:
        entry    a1, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_get_romfuncs
        l32r     a8, .Lsec_216_b8
        callx8   a8
        l32r     a8, .Lsec_216_0
        s32i.n   a10, a8, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a8, .Lsec_216_4
        s32i     a8, a10, 172
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a8, .Lsec_216_8
        s32i     a8, a10, 224
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a8, .Lsec_216_c
        s32i     a8, a10, 228
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a8, .Lsec_216_10
        s32i     a8, a10, 236
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a8, .Lsec_216_14
        s32i     a8, a10, 0x198
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a8, .Lsec_216_18
        s32i     a8, a10, 0x168
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a8, .Lsec_216_1c
        s32i     a8, a10, 0x150
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a8, .Lsec_216_20
        s32i     a8, a10, 0x118
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a8, .Lsec_216_24
        s32i     a8, a10, 100
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a8, .Lsec_216_28
        s32i.n   a8, a10, 48
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a8, .Lsec_216_2c
        s32i.n   a8, a10, 52
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a8, .Lsec_216_30
        s32i     a8, a10, 80
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a8, .Lsec_216_34
        s32i     a8, a10, 92
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a8, .Lsec_216_38
        s32i     a8, a10, 112
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a8, .Lsec_216_3c
        s32i     a8, a10, 116
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a9, .Lsec_216_4c
        l32r     a8, .Lsec_216_50
        s32i.n   a8, a10, 12
        s32i.n   a8, a10, 4
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a8, .Lsec_216_54
        s32i     a8, a10, 0x14c
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a8, .Lsec_216_58
        s32i     a8, a10, 0x1a4
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a8, .Lsec_216_5c
        s32i     a8, a10, 72
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a8, .Lsec_216_60
        s32i     a8, a10, 0x130
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a8, .Lsec_216_64
        s32i     a8, a10, 0x138
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a8, .Lsec_216_68
        s32i     a8, a10, 0x144
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a8, .Lsec_216_6c
        l32r     a11, .Lsec_216_40
        s32i     a8, a10, 0x148
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a8, .Lsec_216_70
        s32i     a11, a10, 128
        s32i     a8, a10, 0x184
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a11, .Lsec_216_44
        l32r     a8, .Lsec_216_74
        s32i     a11, a10, 124
        s32i     a8, a10, 0x11c
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a11, .Lsec_216_48
        l32r     a8, .Lsec_216_78
        s32i     a11, a10, 132
        s32i.n   a9, a10, 8
        s32i.n   a9, a10, 0
        s32i     a8, a10, 0x12c
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a8, .Lsec_216_7c
        s32i     a8, a10, 216
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a8, .Lsec_216_80
        s32i     a8, a10, 0x170
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a8, .Lsec_216_84
        s32i     a8, a10, 0x10c
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a8, .Lsec_216_88
        s32i     a8, a10, 0x110
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a8, .Lsec_216_8c
        s32i     a8, a10, 148
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a8, .Lsec_216_90
        s32i     a8, a10, 156
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a8, .Lsec_216_94
        s32i     a8, a10, 0x124
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a8, .Lsec_216_98
        s32i     a8, a10, 0x16c
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a8, .Lsec_216_9c
        s32i     a8, a10, 0x17c
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a8, .Lsec_216_a0
        s32i     a8, a10, 192
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a8, .Lsec_216_a4
        s32i     a8, a10, 184
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a8, .Lsec_216_a8
        s32i.n   a8, a10, 60
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a8, .Lsec_216_ac
        s32i     a8, a10, 64
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a8, .Lsec_216_b0
        s32i     a8, a10, 240
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a8, .Lsec_216_b4
        s32i     a8, a10, 0x190
        retw.n
        .size   phy_get_romfunc_addr, .-phy_get_romfunc_addr
.Lsec_216_321:

        .section .text.phy_byte_to_word,"ax",@progbits
        .align  4
.Lsec_217_0:
        .global phy_byte_to_word
        .type   phy_byte_to_word, @function
phy_byte_to_word:
        entry    a1, 32
        l8ui     a8, a2, 1
        slli     a8, a8, 8
        l8ui     a9, a2, 0
        or       a9, a8, a9
        l8ui     a8, a2, 2
        slli     a8, a8, 16
        or       a8, a8, a9
        l8ui     a2, a2, 3
        slli     a2, a2, 24
        or       a2, a2, a8
        retw.n
        .size   phy_byte_to_word, .-phy_byte_to_word
.Lsec_217_23:

        .section .text.rf_cal_data_recovery,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_218_0, chip7_sleep_params
        .literal .Lsec_218_4, phy_rxrf_dc
        .literal .Lsec_218_8, phy_rxbb_dc
.Lsec_218_c:
        .global rf_cal_data_recovery
        .type   rf_cal_data_recovery, @function
rf_cal_data_recovery:
        entry    a1, 32
        movi     a11, 0x40c
        l32r     a12, .Lsec_218_0
        add.n    a11, a2, a11
        movi.n   a13, 49
        loop     a13, .Lsec_218_40
        l8ui     a9, a11, 1
        slli     a9, a9, 8
        l8ui     a8, a11, 0
        or       a10, a9, a8
        l8ui     a8, a11, 2
        slli     a8, a8, 16
        or       a9, a8, a10
        l8ui     a8, a11, 3
        addi.n   a11, a11, 4
        slli     a8, a8, 24
        or       a8, a8, a9
        s32i.n   a8, a12, 0
        addi.n   a12, a12, 4
.Lsec_218_40:
        movi     a11, 0x4d0
        nop.n
        nop.n
        l32r     a12, .Lsec_218_4
        add.n    a11, a2, a11
        movi.n   a13, 34
        loop     a13, .Lsec_218_75
        l8ui     a9, a11, 1
        slli     a9, a9, 8
        l8ui     a8, a11, 0
        or       a10, a9, a8
        l8ui     a8, a11, 2
        slli     a8, a8, 16
        or       a9, a8, a10
        l8ui     a8, a11, 3
        addi.n   a11, a11, 4
        slli     a8, a8, 24
        or       a8, a8, a9
        s32i.n   a8, a12, 0
        addi.n   a12, a12, 4
.Lsec_218_75:
        movi     a8, 0x558
        nop.n
        nop.n
        l32r     a11, .Lsec_218_8
        add      a2, a2, a8
        movi     a12, 4
        loop     a12, .Lsec_218_ac
        l8ui     a9, a2, 1
        slli     a9, a9, 8
        l8ui     a8, a2, 0
        or       a10, a9, a8
        l8ui     a8, a2, 2
        slli     a8, a8, 16
        or       a9, a8, a10
        l8ui     a8, a2, 3
        addi.n   a2, a2, 4
        slli     a8, a8, 24
        or       a8, a8, a9
        s32i.n   a8, a11, 0
        addi.n   a11, a11, 4
.Lsec_218_ac:
        retw.n
        .size   rf_cal_data_recovery, .-rf_cal_data_recovery
.Lsec_218_ae:

        .section .text.rf_cal_data_backup,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_219_0, phy_freq_wifi_only
        .literal .Lsec_219_4, 0x3ff4e0c4
        .literal .Lsec_219_8, 0x3ff4e0c0
        .literal .Lsec_219_c, chip7_sleep_params
        .literal .Lsec_219_10, chip7_sleep_params+0xc4
        .literal .Lsec_219_14, phy_rxrf_dc
        .literal .Lsec_219_18, phy_rxrf_dc+0x88
        .literal .Lsec_219_1c, phy_rxbb_dc
        .literal .Lsec_219_20, phy_rxbb_dc+0x10
        .literal .Lsec_219_24, esp_dport_access_reg_read
        .literal .Lsec_219_28, esp_dport_access_reg_read
.Lsec_219_2c:
        .global rf_cal_data_backup
        .type   rf_cal_data_backup, @function
rf_cal_data_backup:
        entry    a1, 48
        l32r     a4, .Lsec_219_0
        movi.n   a3, 12
        movi.n   a5, 14
        movi     a6, 0xffffff00
        l8ui     a7, a4, 0
        s32i.n   a3, a1, 0
        movi.n   a3, 85
        movnez   a3, a5, a7
        mov.n    a5, a7
        s32i.n   a2, a1, 4
        movi.n   a2, 0
        s32i.n   a3, a1, 8
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a3, .Lsec_219_4
        j        .Lsec_219_5f
.Lsec_219_59:
        l32r     a8, .Lsec_219_0
        l8ui     a5, a8, 0
.Lsec_219_5f:
        beqz.n   a5, .Lsec_219_7c
        movi.n   a4, 12
        blt      a4, a2, .Lsec_219_8d
        addi.n   a5, a2, 1
        l32i.n   a8, a1, 0
        addi.n   a14, a8, 1
        addx2    a14, a14, a14
        addx2    a13, a8, a8
        slli     a14, a14, 2
        extui    a4, a13, 0, 8
        j        .Lsec_219_95
        .byte    0x00
.Lsec_219_7c:
        addi.n   a5, a2, 1
        addx2    a14, a5, a5
        addx2    a2, a2, a2
        slli     a14, a14, 2
        extui    a4, a2, 0, 8
        j        .Lsec_219_95
.Lsec_219_8d:
        movi     a4, 252
        movi     a14, 0x3fc
        addi.n   a5, a2, 1
.Lsec_219_95:
        l32i.n   a8, a1, 4
        addi.n   a2, a4, 3
        add.n    a7, a8, a14
        extui    a2, a2, 0, 8
.Lsec_219_9e:
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_219_24
        callx8   a8
        and      a10, a10, a6
        or       a10, a10, a4
        memw
        s32i.n   a10, a3, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a10, .Lsec_219_8
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_219_28
        callx8   a8
        mov.n    a11, a7
        movi.n   a9, 0
        movi     a8, 4
        loop     a8, .Lsec_219_dc
        ssr      a9
        sra      a12, a10
        s8i      a12, a11, 0
        memw
        addi.n   a9, a9, 8
        addi.n   a11, a11, 1
.Lsec_219_dc:
        addi.n   a4, a4, 1
        extui    a4, a4, 0, 8
        addi.n   a7, a7, 4
        bne      a4, a2, .Lsec_219_9e
        l32i.n   a8, a1, 0
        mov.n    a2, a5
        addi.n   a4, a8, 5
        extui    a4, a4, 0, 8
        s32i.n   a4, a1, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a4, a1, 8
        beq      a5, a4, .Lsec_219_101
        j        .Lsec_219_59
.Lsec_219_101:
        l32i.n   a8, a1, 4
        movi     a7, 0x40c
        add.n    a7, a8, a7
        l32r     a6, .Lsec_219_c
        l32r     a8, .Lsec_219_10
.Lsec_219_10e:
        mov.n    a5, a7
        movi.n   a4, 0
        movi     a3, 4
        loop     a3, .Lsec_219_12a
        l32i.n   a2, a6, 0
        ssr      a4
        sra      a2, a2
        addi.n   a4, a4, 8
        s8i      a2, a5, 0
        memw
        addi.n   a5, a5, 1
.Lsec_219_12a:
        addi.n   a6, a6, 4
        addi.n   a7, a7, 4
        bne      a6, a8, .Lsec_219_10e
        j        .Lsec_219_155
.Lsec_219_134:
        l32i.n   a2, a6, 0
        ssr      a4
        srl      a2, a2
        addi.n   a4, a4, 8
        s8i      a2, a5, 0
        memw
        addi.n   a5, a5, 1
        addi.n   a3, a3, -1
        bnez     a3, .Lsec_219_134
        addi.n   a6, a6, 4
        addi.n   a7, a7, 4
        bne      a6, a8, .Lsec_219_162
        j        .Lsec_219_18b
.Lsec_219_155:
        l32i.n   a2, a1, 4
        movi     a7, 0x4d0
        add.n    a7, a2, a7
        l32r     a6, .Lsec_219_14
        l32r     a8, .Lsec_219_18
.Lsec_219_162:
        mov.n    a5, a7
        movi.n   a4, 0
        movi.n   a3, 4
        j        .Lsec_219_134
.Lsec_219_16b:
        l32i.n   a2, a6, 0
        ssr      a4
        srl      a2, a2
        addi.n   a4, a4, 8
        s8i      a2, a5, 0
        memw
        addi.n   a5, a5, 1
        addi.n   a3, a3, -1
        bnez     a3, .Lsec_219_16b
        addi.n   a6, a6, 4
        addi.n   a7, a7, 4
        bne      a6, a8, .Lsec_219_198
        retw.n
.Lsec_219_18b:
        l32i.n   a3, a1, 4
        movi     a7, 0x558
        add.n    a7, a3, a7
        l32r     a6, .Lsec_219_1c
        l32r     a8, .Lsec_219_20
.Lsec_219_198:
        mov.n    a5, a7
        movi.n   a4, 0
        movi.n   a3, 4
        j        .Lsec_219_16b
        .size   rf_cal_data_backup, .-rf_cal_data_backup
.Lsec_219_1a1:

        .section .text.phy_get_rf_cal_version,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_220_0, 0x000012ff
.Lsec_220_4:
        .global phy_get_rf_cal_version
        .type   phy_get_rf_cal_version, @function
phy_get_rf_cal_version:
        entry    a1, 32
        l32r     a2, .Lsec_220_0
        retw.n
        .size   phy_get_rf_cal_version, .-phy_get_rf_cal_version
.Lsec_220_c:

        .section .text.phy_rfcal_data_check,"ax",@progbits
        .align  4
.Lsec_221_0:
        .global phy_rfcal_data_check
        .type   phy_rfcal_data_check, @function
phy_rfcal_data_check:
        entry    a1, 32
        movi.n   a9, -1
        movi.n   a8, 0
        s8i      a9, a3, 0
        movi.n   a9, 18
        extui    a2, a2, 0, 8
        mov.n    a11, a3
        movi.n   a13, 0
        movi     a12, 0x15a
        s8i      a9, a3, 1
        s8i      a8, a3, 2
        s8i      a8, a3, 3
        memw
        loop     a12, .Lsec_221_4f
        nop.n
        nop.n
        nop.n
        nop.n
        l8ui     a9, a11, 1
        slli     a9, a9, 8
        l8ui     a8, a11, 0
        or       a10, a9, a8
        l8ui     a8, a11, 2
        slli     a8, a8, 16
        or       a9, a8, a10
        l8ui     a8, a11, 3
        addi.n   a11, a11, 4
        slli     a8, a8, 24
        or       a8, a8, a9
        add.n    a13, a13, a8
.Lsec_221_4f:
        movi.n   a11, 32
        loop     a11, .Lsec_221_76
        l8ui     a9, a4, 1
        slli     a9, a9, 8
        l8ui     a8, a4, 0
        or       a10, a9, a8
        l8ui     a8, a4, 2
        slli     a8, a8, 16
        or       a9, a8, a10
        l8ui     a8, a4, 3
        addi.n   a4, a4, 4
        slli     a8, a8, 24
        or       a8, a8, a9
        add.n    a13, a13, a8
.Lsec_221_76:
        addmi    a3, a3, 0x500
        l8ui     a4, a3, 105
        slli     a8, a4, 8
        movi.n   a4, -1
        xor      a13, a4, a13
        l8ui     a4, a3, 104
        or       a4, a8, a4
        l8ui     a8, a3, 106
        slli     a8, a8, 16
        or       a4, a8, a4
        l8ui     a8, a3, 107
        slli     a8, a8, 24
        or       a4, a8, a4
        beqz.n   a2, .Lsec_221_ad
        sub      a4, a13, a4
        movi.n   a3, 1
        movi.n   a2, 0
        movnez   a2, a3, a4
        extui    a2, a2, 0, 8
        retw.n
.Lsec_221_ad:
        srli     a9, a13, 8
        extui    a8, a13, 16, 16
        extui    a4, a13, 24, 8
        s8i      a13, a3, 104
        s8i      a9, a3, 105
        s8i      a8, a3, 106
        s8i      a4, a3, 107
        memw
        retw.n
        .size   phy_rfcal_data_check, .-phy_rfcal_data_check
.Lsec_221_c7:

        .section .text.i2cmst_reg_init,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_222_0, 0x3ff4e0c4
        .literal .Lsec_222_4, 0xfff0ffff
        .literal .Lsec_222_8, 0x00020000
        .literal .Lsec_222_c, 0xff0fffff
        .literal .Lsec_222_10, 0x00400000
        .literal .Lsec_222_14, 0x01000000
        .literal .Lsec_222_18, 0x40000000
        .literal .Lsec_222_1c, 0xdfffffff
        .literal .Lsec_222_20, 0xffff83ff
        .literal .Lsec_222_24, 0x00002c00
        .literal .Lsec_222_28, esp_dport_access_reg_read
        .literal .Lsec_222_2c, esp_dport_access_reg_read
        .literal .Lsec_222_30, esp_dport_access_reg_read
        .literal .Lsec_222_34, esp_dport_access_reg_read
        .literal .Lsec_222_38, esp_dport_access_reg_read
        .literal .Lsec_222_3c, esp_dport_access_reg_read
.Lsec_222_40:
        .global i2cmst_reg_init
        .type   i2cmst_reg_init, @function
i2cmst_reg_init:
        entry    a1, 32
        l32r     a2, .Lsec_222_0
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_222_28
        callx8   a8
        l32r     a8, .Lsec_222_4
        and      a9, a10, a8
        mov.n    a10, a2
        l32r     a8, .Lsec_222_8
        or       a8, a9, a8
        memw
        s32i     a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_222_2c
        callx8   a8
        l32r     a8, .Lsec_222_c
        and      a9, a10, a8
        mov.n    a10, a2
        l32r     a8, .Lsec_222_10
        or       a8, a9, a8
        memw
        s32i.n   a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_222_30
        callx8   a8
        l32r     a8, .Lsec_222_14
        or       a8, a10, a8
        mov.n    a10, a2
        memw
        s32i     a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_222_34
        callx8   a8
        l32r     a8, .Lsec_222_18
        or       a8, a10, a8
        mov.n    a10, a2
        memw
        s32i     a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_222_38
        callx8   a8
        l32r     a8, .Lsec_222_1c
        and      a8, a10, a8
        mov.n    a10, a2
        memw
        s32i.n   a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_222_3c
        callx8   a8
        l32r     a8, .Lsec_222_20
        and      a10, a10, a8
        l32r     a8, .Lsec_222_24
        or       a10, a10, a8
        memw
        s32i.n   a10, a2, 0
        retw.n
        .size   i2cmst_reg_init, .-i2cmst_reg_init
.Lsec_222_cf:

        .section .text.fe_reg_init,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_223_0, 0x3ff460a0
        .literal .Lsec_223_4, 0x01000000
        .literal .Lsec_223_8, 0x3ff45114
        .literal .Lsec_223_c, 0x3ff450dc
        .literal .Lsec_223_10, 0xc0000000
        .literal .Lsec_223_14, 0x3ff450d8
        .literal .Lsec_223_18, 0x02000000
        .literal .Lsec_223_1c, 0x3ff460b8
        .literal .Lsec_223_20, 0x20000000
        .literal .Lsec_223_24, 0x3ff4609c
        .literal .Lsec_223_28, 0x00004000
        .literal .Lsec_223_2c, 0x3ff4604c
        .literal .Lsec_223_30, 0xffff00ff
        .literal .Lsec_223_34, 0x0000c800
        .literal .Lsec_223_38, esp_dport_access_reg_read
        .literal .Lsec_223_3c, esp_dport_access_reg_read
        .literal .Lsec_223_40, esp_dport_access_reg_read
        .literal .Lsec_223_44, esp_dport_access_reg_read
        .literal .Lsec_223_48, esp_dport_access_reg_read
        .literal .Lsec_223_4c, esp_dport_access_reg_read
        .literal .Lsec_223_50, esp_dport_access_reg_read
.Lsec_223_54:
        .global fe_reg_init
        .type   fe_reg_init, @function
fe_reg_init:
        entry    a1, 32
        l32r     a2, .Lsec_223_0
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_223_38
        callx8   a8
        l32r     a8, .Lsec_223_4
        or       a8, a10, a8
        l32r     a3, .Lsec_223_8
        mov.n    a10, a3
        memw
        s32i.n   a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_223_3c
        callx8   a8
        movi     a8, 0x200
        or       a8, a10, a8
        l32r     a2, .Lsec_223_c
        or       a10, a2, a2
        memw
        s32i     a8, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_223_40
        callx8   a8
        l32r     a8, .Lsec_223_10
        or       a8, a10, a8
        l32r     a3, .Lsec_223_14
        or       a10, a3, a3
        memw
        s32i     a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_223_44
        callx8   a8
        l32r     a8, .Lsec_223_18
        or       a8, a10, a8
        l32r     a2, .Lsec_223_1c
        or       a10, a2, a2
        memw
        s32i     a8, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_223_48
        callx8   a8
        l32r     a8, .Lsec_223_20
        or       a8, a10, a8
        l32r     a3, .Lsec_223_24
        or       a10, a3, a3
        memw
        s32i     a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_223_4c
        callx8   a8
        l32r     a8, .Lsec_223_28
        or       a8, a10, a8
        l32r     a2, .Lsec_223_2c
        or       a10, a2, a2
        memw
        s32i     a8, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_223_50
        callx8   a8
        l32r     a8, .Lsec_223_30
        and      a10, a10, a8
        l32r     a8, .Lsec_223_34
        or       a10, a10, a8
        memw
        s32i.n   a10, a2, 0
        retw.n
        .size   fe_reg_init, .-fe_reg_init
.Lsec_223_103:

        .section .phyiram.12,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_224_0, g_phyFuns
        .literal .Lsec_224_4, 0x3ff48030
        .literal .Lsec_224_8, 0xf8000000
        .literal .Lsec_224_c, 0x3ff66000
        .literal .Lsec_224_10, esp_dport_access_reg_read
        .literal .Lsec_224_14, esp_dport_access_reg_read
        .literal .Lsec_224_18, fe_reg_init
        .literal .Lsec_224_1c, i2cmst_reg_init
.Lsec_224_20:
        .global reg_init_begin
        .type   reg_init_begin, @function
reg_init_begin:
        entry    a1, 32
        l32r     a3, .Lsec_224_0
        movi.n   a13, 0
        movi     a12, 12
        movi     a11, 4
        movi     a10, 102
        l32i     a3, a3, 0
        extui    a2, a2, 0, 8
        l32i     a3, a3, 160
        callx8   a3
        l32r     a4, .Lsec_224_4
        or       a10, a4, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_224_10
        callx8   a8
        l32r     a8, .Lsec_224_8
        or       a8, a10, a8
        l32r     a3, .Lsec_224_c
        or       a10, a3, a3
        memw
        s32i     a8, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_224_14
        callx8   a8
        movi     a8, 0x400
        or       a10, a10, a8
        memw
        s32i.n   a10, a3, 0
        bnez.n   a2, .Lsec_224_7a
        .reloc  ., R_XTENSA_ASM_EXPAND, fe_reg_init
        l32r     a8, .Lsec_224_18
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, i2cmst_reg_init
        l32r     a8, .Lsec_224_1c
        callx8   a8
.Lsec_224_7a:
        retw.n
        .size   reg_init_begin, .-reg_init_begin
.Lsec_224_7c:

        .section .phyiram.17,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_225_0, phy_init_flag
        .literal .Lsec_225_4, force_hw_set_freq_flag
        .literal .Lsec_225_8, 0x3ff460a0
        .literal .Lsec_225_c, 0xfffff0ff
        .literal .Lsec_225_10, 0x00000a00
        .literal .Lsec_225_14, g_phyFuns
        .literal .Lsec_225_18, init_wifi_disable
        .literal .Lsec_225_1c, 0x3ff5c080
        .literal .Lsec_225_20, phy_enter_critical
        .literal .Lsec_225_24, esp_dport_access_reg_read
        .literal .Lsec_225_28, phy_dis_hw_set_freq
        .literal .Lsec_225_2c, i2c_master_reset
        .literal .Lsec_225_30, reg_init_begin
        .literal .Lsec_225_34, phy_i2c_init
        .literal .Lsec_225_38, phy_reg_init
        .literal .Lsec_225_3c, set_chan_reg
        .literal .Lsec_225_40, esp_dport_access_reg_read
        .literal .Lsec_225_44, enable_wifi_agc
        .literal .Lsec_225_48, phy_en_hw_set_freq
        .literal .Lsec_225_4c, esp_dport_access_reg_read
        .literal .Lsec_225_50, phy_exit_critical
.Lsec_225_54:
        .global phy_wakeup_init
        .type   phy_wakeup_init, @function
phy_wakeup_init:
        entry    a1, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_enter_critical
        l32r     a8, .Lsec_225_20
        callx8   a8
        l32r     a2, .Lsec_225_8
        movi.n   a3, 1
        or       a6, a10, a10
        or       a10, a2, a2
        l32r     a5, .Lsec_225_0
        l32r     a4, .Lsec_225_4
        s8i      a3, a5, 0
        s8i      a3, a4, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_225_24
        callx8   a8
        l32r     a3, .Lsec_225_c
        and      a10, a10, a3
        l32r     a8, .Lsec_225_10
        or       a10, a10, a8
        memw
        s32i.n   a10, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_dis_hw_set_freq
        l32r     a8, .Lsec_225_28
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, i2c_master_reset
        l32r     a8, .Lsec_225_2c
        callx8   a8
        movi.n   a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, reg_init_begin
        l32r     a8, .Lsec_225_30
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_i2c_init
        l32r     a8, .Lsec_225_34
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_reg_init
        l32r     a8, .Lsec_225_38
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, set_chan_reg
        l32r     a8, .Lsec_225_3c
        callx8   a8
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_225_40
        callx8   a8
        and      a3, a10, a3
        memw
        s32i.n   a3, a2, 0
        movi.n   a12, 0
        mov.n    a11, a12
        movi     a10, 99
        nop.n
        l32r     a2, .Lsec_225_14
        l32i     a2, a2, 0
        l32i     a2, a2, 152
        callx8   a2
        movi.n   a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, enable_wifi_agc
        l32r     a8, .Lsec_225_44
        callx8   a8
        s8i      a2, a5, 0
        s8i      a2, a4, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_en_hw_set_freq
        l32r     a8, .Lsec_225_48
        callx8   a8
        l32r     a2, .Lsec_225_18
        l8ui     a2, a2, 0
        beqz     a2, .Lsec_225_110
        movi     a2, 192
        l32r     a3, .Lsec_225_1c
        or       a10, a3, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_225_4c
        callx8   a8
        or       a10, a10, a2
        memw
        s32i.n   a10, a3, 0
.Lsec_225_110:
        mov.n    a10, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_exit_critical
        l32r     a8, .Lsec_225_50
        callx8   a8
        retw.n
        .size   phy_wakeup_init, .-phy_wakeup_init
.Lsec_225_11a:

        .section .text.get_phy_version_str,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_226_0, .Lsec_163_d4
        .literal .Lsec_226_4, .Lsec_163_e0
        .literal .Lsec_226_8, .Lsec_163_ec
        .literal .Lsec_226_c, 0x000012ff
        .literal .Lsec_226_10, .Lsec_163_f4
        .literal .Lsec_226_14, phy_version_str
        .literal .Lsec_226_18, sprintf
.Lsec_226_1c:
        .global get_phy_version_str
        .type   get_phy_version_str, @function
get_phy_version_str:
        entry    a1, 32
        l32r     a2, .Lsec_226_14
        mov.n    a10, a2
        l32r     a15, .Lsec_226_0
        l32r     a14, .Lsec_226_4
        l32r     a13, .Lsec_226_8
        l32r     a12, .Lsec_226_c
        l32r     a11, .Lsec_226_10
        .reloc  ., R_XTENSA_ASM_EXPAND, sprintf
        l32r     a8, .Lsec_226_18
        callx8   a8
        retw.n
        .size   get_phy_version_str, .-get_phy_version_str
.Lsec_226_3b:

        .section .text.phy_version_print,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_227_0, .Lsec_163_d4
        .literal .Lsec_227_4, .Lsec_163_e0
        .literal .Lsec_227_8, .Lsec_163_ec
        .literal .Lsec_227_c, 0x000012ff
        .literal .Lsec_227_10, .Lsec_163_100
        .literal .Lsec_227_14, phy_printf
.Lsec_227_18:
        .global phy_version_print
        .type   phy_version_print, @function
phy_version_print:
        entry    a1, 32
        l32r     a14, .Lsec_227_0
        l32r     a13, .Lsec_227_4
        l32r     a12, .Lsec_227_8
        l32r     a11, .Lsec_227_c
        l32r     a10, .Lsec_227_10
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_227_14
        callx8   a8
        retw.n
        .size   phy_version_print, .-phy_version_print
.Lsec_227_32:

        .section .text.get_iq_value,"ax",@progbits
        .align  4
.Lsec_228_0:
        .global get_iq_value
        .type   get_iq_value, @function
get_iq_value:
        entry    a1, 32
        extui    a3, a3, 0, 16
        extui    a8, a3, 6, 5
        movi.n   a9, 15
        bge      a9, a8, .Lsec_228_11
        addi     a8, a8, -32
.Lsec_228_11:
        s8i      a8, a2, 0
        memw
        extui    a3, a3, 0, 6
        movi.n   a8, 31
        bge      a8, a3, .Lsec_228_22
        addi     a3, a3, -64
.Lsec_228_22:
        s8i      a3, a2, 1
        memw
        retw.n
        .size   get_iq_value, .-get_iq_value
.Lsec_228_2a:

        .section .text.phy_rfcal_data_check_value,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_229_0, chip7_sleep_params
        .literal .Lsec_229_4, chip7_sleep_params+0x60
        .literal .Lsec_229_8, g_phyFuns
        .literal .Lsec_229_c, chip7_sleep_params+0x28
        .literal .Lsec_229_10, chip7_sleep_params+0x18
        .literal .Lsec_229_14, chip7_sleep_params+0x8
        .literal .Lsec_229_18, chip7_sleep_params+0x6
        .literal .Lsec_229_1c, chip7_sleep_params+0x78
        .literal .Lsec_229_20, chip7_sleep_params+0x9
        .literal .Lsec_229_24, memcpy
.Lsec_229_28:
        .global phy_rfcal_data_check_value
        .type   phy_rfcal_data_check_value, @function
phy_rfcal_data_check_value:
        entry    a1, 64
        extui    a4, a4, 0, 8
        s32i.n   a3, a1, 20
        bnez     a4, .Lsec_229_7e
        movi     a11, 0x40c
        add.n    a11, a2, a11
        movi     a12, 196
        or       a10, a3, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_229_24
        callx8   a8
        mov.n    a2, a4
        retw.n
        .byte    0x00
.Lsec_229_49:
        l32i.n   a13, a5, 0
        addi.n   a8, a8, 2
        addi     a4, a4, 2
        l16ui    a14, a8, 10
        l16ui    a10, a4, 10
        sub      a10, a14, a10
        l32i     a13, a13, 0x140
        s32i     a8, a1, 24
        callx8   a13
        addi.n   a13, a2, 1
        l32i.n   a8, a1, 24
        blti     a10, 5, .Lsec_229_6c
        extui    a2, a13, 0, 16
.Lsec_229_6c:
        bne      a3, a8, .Lsec_229_49
        addi.n   a7, a7, 8
        l32r     a4, .Lsec_229_c
        addi.n   a3, a3, 8
        bne      a7, a4, .Lsec_229_90
        j        .Lsec_229_cb
        .byte    0x00
        .byte    0x00
.Lsec_229_7e:
        l32i.n   a8, a1, 20
        movi.n   a2, 0
        addi.n   a8, a8, 4
        mov.n    a3, a8
        l32r     a6, .Lsec_229_0
        mov.n    a7, a6
        l32r     a5, .Lsec_229_8
        s32i.n   a8, a1, 16
.Lsec_229_90:
        addi     a8, a3, -4
        mov.n    a4, a7
        j        .Lsec_229_49
        .byte    0x00
.Lsec_229_99:
        l32i.n   a12, a5, 0
        addi.n   a8, a8, 2
        addi.n   a7, a7, 2
        l16ui    a13, a8, 150
        l16ui    a10, a7, 150
        sub      a10, a13, a10
        l32i     a12, a12, 0x140
        s32i.n   a8, a1, 24
        callx8   a12
        addi.n   a12, a2, 1
        l32i.n   a8, a1, 24
        blti     a10, 5, .Lsec_229_ba
        extui    a2, a12, 0, 16
.Lsec_229_ba:
        l32i.n   a9, a1, 16
        bne      a9, a8, .Lsec_229_99
        addi.n   a9, a9, 8
        addi.n   a3, a3, 8
        s32i.n   a9, a1, 16
        bne      a3, a4, .Lsec_229_d0
        j        .Lsec_229_da
.Lsec_229_cb:
        l32r     a4, .Lsec_229_10
        mov.n    a3, a6
.Lsec_229_d0:
        mov.n    a7, a3
        l32i.n   a9, a1, 16
        addi     a8, a9, -4
        j        .Lsec_229_99
.Lsec_229_da:
        l32i.n   a4, a1, 20
        addi     a3, a4, 52
        addi     a7, a4, 56
        mov.n    a4, a6
.Lsec_229_e4:
        l16ui    a8, a3, 0
        movi.n   a9, 15
        extui    a11, a8, 6, 5
        sext     a10, a11, 7
        bge      a9, a10, .Lsec_229_f5
        addi     a10, a11, -32
.Lsec_229_f5:
        extui    a8, a8, 0, 6
        movi.n   a9, 31
        bge      a9, a8, .Lsec_229_100
        addi     a8, a8, -64
.Lsec_229_100:
        s8i      a8, a1, 3
        memw
        movi.n   a9, 15
        nop.n
        nop.n
        nop.n
        l16ui    a8, a4, 52
        extui    a12, a8, 6, 5
        sext     a11, a12, 7
        bge      a9, a11, .Lsec_229_11d
        addi     a11, a12, -32
.Lsec_229_11d:
        extui    a8, a8, 0, 6
        movi     a9, 31
        bge      a9, a8, .Lsec_229_129
        addi     a8, a8, -64
.Lsec_229_129:
        s8i      a8, a1, 1
        memw
        sub      a10, a10, a11
        nop.n
        nop.n
        nop.n
        l32i     a8, a5, 0
        l32i     a8, a8, 0x140
        callx8   a8
        blti     a10, 5, .Lsec_229_149
        addi.n   a2, a2, 1
        extui    a2, a2, 0, 16
.Lsec_229_149:
        l8ui     a10, a1, 3
        sext     a10, a10, 7
        l8ui     a8, a1, 1
        sext     a8, a8, 7
        sub      a10, a10, a8
        l32i     a11, a5, 0
        l32i     a8, a11, 0x140
        callx8   a8
        blti     a10, 5, .Lsec_229_16a
        addi     a2, a2, 1
        extui    a2, a2, 0, 16
.Lsec_229_16a:
        addi.n   a3, a3, 2
        addi.n   a4, a4, 2
        beq      a3, a7, .Lsec_229_174
        j        .Lsec_229_e4
.Lsec_229_174:
        mov.n    a3, a6
        movi     a4, 15
.Lsec_229_179:
        l16ui    a8, a7, 0
        extui    a11, a8, 6, 5
        sext     a10, a11, 7
        bge      a4, a10, .Lsec_229_188
        addi     a10, a11, -32
.Lsec_229_188:
        extui    a8, a8, 0, 6
        movi     a9, 31
        bge      a9, a8, .Lsec_229_194
        addi     a8, a8, -64
.Lsec_229_194:
        s8i      a8, a1, 3
        memw
        nop.n
        nop.n
        nop.n
        nop.n
        l16ui    a8, a3, 56
        extui    a12, a8, 6, 5
        sext     a11, a12, 7
        bge      a4, a11, .Lsec_229_1b1
        addi     a11, a12, -32
.Lsec_229_1b1:
        extui    a8, a8, 0, 6
        movi     a9, 31
        bge      a9, a8, .Lsec_229_1bd
        addi     a8, a8, -64
.Lsec_229_1bd:
        s8i      a8, a1, 1
        memw
        sub      a10, a10, a11
        nop.n
        nop.n
        nop.n
        l32i     a8, a5, 0
        l32i     a8, a8, 0x140
        callx8   a8
        blti     a10, 5, .Lsec_229_1dd
        addi.n   a2, a2, 1
        extui    a2, a2, 0, 16
.Lsec_229_1dd:
        l8ui     a10, a1, 3
        sext     a10, a10, 7
        l8ui     a8, a1, 1
        sext     a8, a8, 7
        sub      a10, a10, a8
        l32i     a11, a5, 0
        l32i     a8, a11, 0x140
        callx8   a8
        blti     a10, 5, .Lsec_229_1fd
        addi.n   a2, a2, 1
        extui    a2, a2, 0, 16
.Lsec_229_1fd:
        l32r     a8, .Lsec_229_14
        addi.n   a3, a3, 2
        addi.n   a7, a7, 2
        beq      a8, a3, .Lsec_229_20a
        j        .Lsec_229_179
.Lsec_229_20a:
        movi     a7, 146
        l32i.n   a9, a1, 20
        mov.n    a3, a6
        add.n    a7, a9, a7
        movi.n   a4, 15
.Lsec_229_215:
        l16ui    a8, a7, 0
        extui    a11, a8, 6, 5
        sext     a10, a11, 7
        bge      a4, a10, .Lsec_229_224
        addi     a10, a11, -32
.Lsec_229_224:
        extui    a8, a8, 0, 6
        movi     a9, 31
        bge      a9, a8, .Lsec_229_230
        addi     a8, a8, -64
.Lsec_229_230:
        s8i      a8, a1, 3
        memw
        nop.n
        nop.n
        nop.n
        nop.n
        l16ui    a8, a3, 146
        extui    a12, a8, 6, 5
        sext     a11, a12, 7
        bge      a4, a11, .Lsec_229_24d
        addi     a11, a12, -32
.Lsec_229_24d:
        extui    a8, a8, 0, 6
        movi     a9, 31
        bge      a9, a8, .Lsec_229_259
        addi     a8, a8, -64
.Lsec_229_259:
        s8i      a8, a1, 1
        memw
        sub      a10, a10, a11
        nop.n
        nop.n
        nop.n
        l32i     a8, a5, 0
        l32i     a8, a8, 0x140
        callx8   a8
        blti     a10, 5, .Lsec_229_279
        addi.n   a2, a2, 1
        extui    a2, a2, 0, 16
.Lsec_229_279:
        l8ui     a10, a1, 3
        sext     a10, a10, 7
        l8ui     a8, a1, 1
        sext     a8, a8, 7
        sub      a10, a10, a8
        l32i     a11, a5, 0
        l32i     a8, a11, 0x140
        callx8   a8
        blti     a10, 5, .Lsec_229_299
        addi.n   a2, a2, 1
        extui    a2, a2, 0, 16
.Lsec_229_299:
        l32r     a8, .Lsec_229_18
        addi.n   a3, a3, 2
        addi.n   a7, a7, 2
        beq      a8, a3, .Lsec_229_2a6
        j        .Lsec_229_215
.Lsec_229_2a6:
        l32i.n   a9, a1, 20
        addi     a3, a9, 96
        l32r     a7, .Lsec_229_4
        l32r     a4, .Lsec_229_1c
.Lsec_229_2b1:
        l8ui     a10, a3, 0
        addi.n   a3, a3, 1
        sext     a10, a10, 7
        l8ui     a8, a7, 0
        addi.n   a7, a7, 1
        sext     a8, a8, 7
        sub      a10, a10, a8
        l32i.n   a8, a5, 0
        l32i     a8, a8, 0x140
        callx8   a8
        addi.n   a8, a2, 1
        blti     a10, 5, .Lsec_229_2d4
        extui    a2, a8, 0, 16
.Lsec_229_2d4:
        bne      a7, a4, .Lsec_229_2b1
        l32i.n   a3, a1, 20
        l32r     a8, .Lsec_229_0
        l32i.n   a4, a5, 0
        l8ui     a10, a3, 138
        l8ui     a7, a8, 138
        sub      a10, a10, a7
        l32i     a4, a4, 0x140
        callx8   a4
        blti     a10, 5, .Lsec_229_2f5
        addi.n   a2, a2, 1
        extui    a2, a2, 0, 16
.Lsec_229_2f5:
        l32i.n   a9, a1, 20
        addi     a3, a9, 64
        l32r     a4, .Lsec_229_20
.Lsec_229_2fd:
        addi.n   a6, a6, 1
        l32i.n   a7, a5, 0
        l8ui     a10, a3, 0
        addi.n   a3, a3, 1
        l8ui     a8, a6, 63
        sub      a10, a10, a8
        l32i     a7, a7, 0x140
        callx8   a7
        addi.n   a7, a2, 1
        blti     a10, 5, .Lsec_229_31a
        extui    a2, a7, 0, 16
.Lsec_229_31a:
        bne      a4, a6, .Lsec_229_2fd
        retw.n
        .size   phy_rfcal_data_check_value, .-phy_rfcal_data_check_value
.Lsec_229_31f:

        .section .text.tx_cont_en,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_230_0, chip7_sleep_params
        .literal .Lsec_230_4, g_phyFuns
.Lsec_230_8:
        .global tx_cont_en
        .type   tx_cont_en, @function
tx_cont_en:
        entry    a1, 32
        l32r     a2, .Lsec_230_0
        l8ui     a3, a2, 82
        bnez.n   a3, .Lsec_230_51
        mov.n    a14, a3
        movi.n   a13, 6
        l32r     a4, .Lsec_230_4
        movi.n   a12, 3
        movi.n   a11, 1
        movi     a10, 103
        l32i.n   a8, a4, 0
        l8ui     a15, a2, 84
        addi     a15, a15, 22
        extui    a15, a15, 0, 8
        l32i     a8, a8, 168
        callx8   a8
        mov.n    a14, a3
        movi.n   a13, 6
        l32i.n   a3, a4, 0
        movi.n   a12, 4
        movi.n   a11, 1
        movi     a10, 103
        l8ui     a15, a2, 84
        addi     a15, a15, 24
        extui    a15, a15, 0, 8
        l32i     a2, a3, 168
        callx8   a2
        retw.n
        .byte    0x00
.Lsec_230_51:
        l32r     a3, .Lsec_230_4
        movi.n   a14, 0
        movi.n   a13, 6
        movi.n   a12, 3
        movi.n   a11, 1
        l32i.n   a8, a3, 0
        movi     a10, 103
        l8ui     a15, a2, 86
        addi     a15, a15, 18
        extui    a15, a15, 0, 8
        l32i     a8, a8, 168
        callx8   a8
        movi.n   a14, 0
        movi.n   a13, 6
        l32i.n   a3, a3, 0
        movi.n   a12, 4
        movi.n   a11, 1
        movi     a10, 103
        l8ui     a15, a2, 86
        addi     a15, a15, 17
        extui    a15, a15, 0, 8
        l32i     a2, a3, 168
        callx8   a2
        retw.n
        .size   tx_cont_en, .-tx_cont_en
.Lsec_230_8e:

        .section .text.tx_cont_dis,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_231_0, chip7_sleep_params
        .literal .Lsec_231_4, g_phyFuns
.Lsec_231_8:
        .global tx_cont_dis
        .type   tx_cont_dis, @function
tx_cont_dis:
        entry    a1, 32
        l32r     a4, .Lsec_231_0
        movi.n   a3, 0
        movi.n   a2, 2
        mov.n    a14, a3
        movi.n   a13, 6
        l8ui     a8, a4, 82
        movi.n   a12, 3
        moveqz   a2, a3, a8
        add.n    a2, a4, a2
        movi.n   a11, 1
        l32r     a4, .Lsec_231_4
        movi     a10, 103
        l32i     a8, a4, 0
        l8ui     a15, a2, 84
        addi     a15, a15, -2
        extui    a15, a15, 0, 8
        l32i     a8, a8, 168
        callx8   a8
        mov.n    a14, a3
        movi.n   a13, 6
        l32i.n   a3, a4, 0
        movi.n   a12, 4
        movi.n   a11, 1
        movi     a10, 103
        l32i     a3, a3, 168
        l8ui     a15, a2, 84
        callx8   a3
        retw.n
        .size   tx_cont_dis, .-tx_cont_dis
.Lsec_231_52:

        .section .text.tx_cont_cfg,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_232_0, tx_cont_en
        .literal .Lsec_232_4, tx_cont_dis
.Lsec_232_8:
        .global tx_cont_cfg
        .type   tx_cont_cfg, @function
tx_cont_cfg:
        entry    a1, 32
        extui    a2, a2, 0, 8
        bnei     a2, 1, .Lsec_232_19
        .reloc  ., R_XTENSA_ASM_EXPAND, tx_cont_en
        l32r     a8, .Lsec_232_0
        callx8   a8
        retw.n
.Lsec_232_19:
        .reloc  ., R_XTENSA_ASM_EXPAND, tx_cont_dis
        l32r     a8, .Lsec_232_4
        callx8   a8
        retw.n
        .size   tx_cont_cfg, .-tx_cont_cfg
.Lsec_232_21:

        .section .text.phy_get_tx_pwr,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_233_0, g_phyFuns
        .literal .Lsec_233_4, .Lsec_163_120
        .literal .Lsec_233_8, phy_printf
.Lsec_233_c:
        .global phy_get_tx_pwr
        .type   phy_get_tx_pwr, @function
phy_get_tx_pwr:
        entry    a1, 48
        l32r     a2, .Lsec_233_0
        mov.n    a10, a1
        l32i.n   a2, a2, 0
        l32i     a2, a2, 0x12c
        callx8   a2
        l16ui    a8, a1, 14
        l16ui    a12, a1, 12
        add.n    a12, a12, a8
        extui    a9, a12, 0, 16
        l16ui    a8, a1, 6
        l16ui    a11, a1, 4
        add      a11, a11, a8
        sub      a11, a11, a9
        sext     a11, a11, 15
        l16ui    a12, a1, 8
        slli     a2, a11, 10
        l16ui    a8, a1, 10
        add      a12, a12, a8
        sub      a12, a12, a9
        sext     a12, a12, 15
        l32r     a10, .Lsec_233_4
        quos     a2, a2, a12
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_233_8
        callx8   a8
        sext     a2, a2, 15
        retw.n
        .size   phy_get_tx_pwr, .-phy_get_tx_pwr
.Lsec_233_59:

        .section .text.phy_init_pwr_print,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_234_0, chip7_sleep_params
        .literal .Lsec_234_4, chip7_sleep_params+0x78
        .literal .Lsec_234_8, chip7_sleep_params+0x60
        .literal .Lsec_234_c, tx_rf_ana_gain
        .literal .Lsec_234_10, .Lsec_163_12c
        .literal .Lsec_234_14, .Lsec_163_15c
        .literal .Lsec_234_18, chip7_sleep_params+0x8
        .literal .Lsec_234_1c, .Lsec_163_170
        .literal .Lsec_234_20, .Lsec_163_178
        .literal .Lsec_234_24, .Lsec_163_17c
        .literal .Lsec_234_28, .Lsec_163_190
        .literal .Lsec_234_2c, .Lsec_163_1a0
        .literal .Lsec_234_30, .Lsec_163_1a8
        .literal .Lsec_234_34, .Lsec_163_1b8
        .literal .Lsec_234_38, .Lsec_163_1c8
        .literal .Lsec_234_3c, phy_printf
        .literal .Lsec_234_40, phy_printf
        .literal .Lsec_234_44, phy_printf
        .literal .Lsec_234_48, phy_printf
        .literal .Lsec_234_4c, phy_printf
        .literal .Lsec_234_50, phy_printf
        .literal .Lsec_234_54, phy_printf
        .literal .Lsec_234_58, phy_printf
        .literal .Lsec_234_5c, phy_printf
        .literal .Lsec_234_60, phy_printf
        .literal .Lsec_234_64, phy_printf
        .literal .Lsec_234_68, phy_printf
        .literal .Lsec_234_6c, phy_printf
        .literal .Lsec_234_70, phy_printf
        .literal .Lsec_234_74, phy_printf
.Lsec_234_78:
        .global phy_init_pwr_print
        .type   phy_init_pwr_print, @function
phy_init_pwr_print:
        entry    a1, 32
        l32r     a2, .Lsec_234_c
        l32r     a10, .Lsec_234_10
        l16ui    a12, a2, 2
        l16ui    a11, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_234_3c
        callx8   a8
        l32r     a10, .Lsec_234_14
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_234_40
        callx8   a8
        l32r     a3, .Lsec_234_0
        mov.n    a4, a3
        l32r     a5, .Lsec_234_18
        l32r     a2, .Lsec_234_1c
.Lsec_234_a1:
        addi.n   a4, a4, 2
        mov.n    a10, a2
        l16si    a11, a4, 2
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_234_44
        callx8   a8
        bne      a5, a4, .Lsec_234_a1
        l32r     a7, .Lsec_234_20
        mov.n    a10, a7
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_234_48
        callx8   a8
        l32r     a10, .Lsec_234_24
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_234_4c
        callx8   a8
        l32r     a6, .Lsec_234_8
        l32r     a4, .Lsec_234_4
.Lsec_234_cb:
        mov.n    a10, a2
        l8ui     a11, a6, 0
        addi     a6, a6, 1
        sext     a11, a11, 7
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_234_50
        callx8   a8
        bne      a4, a6, .Lsec_234_cb
        or       a10, a7, a7
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_234_54
        callx8   a8
        l32r     a10, .Lsec_234_28
        or       a6, a3, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_234_58
        callx8   a8
        l32r     a4, .Lsec_234_2c
.Lsec_234_f7:
        addi.n   a6, a6, 1
        or       a10, a4, a4
        l8ui     a11, a6, 119
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_234_5c
        callx8   a8
        bne      a5, a6, .Lsec_234_f7
        mov.n    a10, a7
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_234_60
        callx8   a8
        l32r     a10, .Lsec_234_30
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_234_64
        callx8   a8
.Lsec_234_119:
        addi.n   a3, a3, 1
        mov.n    a10, a2
        l8ui     a11, a3, 128
        sext     a11, a11, 7
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_234_68
        callx8   a8
        bne      a3, a5, .Lsec_234_119
        or       a10, a7, a7
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_234_6c
        callx8   a8
        l32r     a2, .Lsec_234_0
        l32r     a10, .Lsec_234_34
        l16ui    a11, a2, 144
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_234_70
        callx8   a8
        l8ui     a11, a2, 138
        l32r     a10, .Lsec_234_38
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_234_74
        callx8   a8
        retw.n
        .size   phy_init_pwr_print, .-phy_init_pwr_print
.Lsec_234_152:

        .section .text.phy_get_rx_freq,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_235_0, 0x2aaaaaab
        .literal .Lsec_235_4, 0x4ec4ec4f
.Lsec_235_8:
        .global phy_get_rx_freq
        .type   phy_get_rx_freq, @function
phy_get_rx_freq:
        entry    a1, 32
        extui    a2, a2, 0, 8
        bgeui    a2, 8, .Lsec_235_28
        l32r     a2, .Lsec_235_0
        sext     a3, a3, 15
        mulsh    a2, a3, a2
        srai     a3, a3, 31
        srai     a2, a2, 3
        sub      a3, a2, a3
        sext     a2, a3, 15
        retw.n
.Lsec_235_28:
        extui    a3, a3, 10, 10
        sext     a3, a3, 15
        movi     a2, 0x1ff
        bge      a2, a3, .Lsec_235_3a
        addmi    a3, a3, 0xfffffc00
        sext     a3, a3, 15
.Lsec_235_3a:
        l32r     a2, .Lsec_235_4
        slli     a3, a3, 4
        mulsh    a2, a3, a2
        srai     a3, a3, 31
        srai     a2, a2, 2
        sub      a3, a2, a3
        sext     a2, a3, 15
        retw.n
        .size   phy_get_rx_freq, .-phy_get_rx_freq
.Lsec_235_51:

        .section .text.reset_rf_dig,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_236_0, 0x3ff00024
        .literal .Lsec_236_4, 0x3ff000d0
        .literal .Lsec_236_8, 0x3ff48030
        .literal .Lsec_236_c, 0x27ffffff
        .literal .Lsec_236_10, disable_wifi_agc
        .literal .Lsec_236_14, esp_dport_access_reg_read
        .literal .Lsec_236_18, esp_dport_access_reg_read
        .literal .Lsec_236_1c, ets_delay_us
        .literal .Lsec_236_20, esp_dport_access_reg_read
        .literal .Lsec_236_24, esp_dport_access_reg_read
        .literal .Lsec_236_28, ets_delay_us
        .literal .Lsec_236_2c, force_txrxoff
.Lsec_236_30:
        .global reset_rf_dig
        .type   reset_rf_dig, @function
reset_rf_dig:
        entry    a1, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, disable_wifi_agc
        l32r     a8, .Lsec_236_10
        callx8   a8
        l32r     a3, .Lsec_236_0
        or       a10, a3, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_236_14
        callx8   a8
        movi.n   a8, -3
        l32r     a2, .Lsec_236_4
        and      a8, a10, a8
        mov.n    a10, a2
        memw
        s32i.n   a8, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_236_18
        callx8   a8
        movi.n   a8, 3
        or       a8, a10, a8
        movi.n   a10, 1
        memw
        s32i.n   a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_236_1c
        callx8   a8
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_236_20
        callx8   a8
        movi.n   a8, -4
        l32r     a3, .Lsec_236_8
        and      a8, a10, a8
        mov.n    a10, a3
        memw
        s32i.n   a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_236_24
        callx8   a8
        l32r     a2, .Lsec_236_c
        and      a2, a10, a2
        movi.n   a10, 1
        memw
        s32i.n   a2, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_236_28
        callx8   a8
        movi.n   a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, force_txrxoff
        l32r     a8, .Lsec_236_2c
        callx8   a8
        retw.n
        .size   reset_rf_dig, .-reset_rf_dig
.Lsec_236_a6:

        .section .text.register_chipv7_phy,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_237_0, 0x00200008
        .literal .Lsec_237_4, 0x60033c00
        .literal .Lsec_237_8, re_entry
        .literal .Lsec_237_c, phy_init_flag
        .literal .Lsec_237_10, force_hw_set_freq_flag
        .literal .Lsec_237_14, 0x3ff4e0c4
        .literal .Lsec_237_18, chip7_sleep_params
        .literal .Lsec_237_1c, chip7_phy_init_ctrl
        .literal .Lsec_237_20, pwrdet_offset
        .literal .Lsec_237_24, phy_in_most_power
        .literal .Lsec_237_28, 0xfffeddd7
        .literal .Lsec_237_2c, g_phyFuns
        .literal .Lsec_237_30, btpwr_tsen_init
        .literal .Lsec_237_34, btpwr_tsen_old
        .literal .Lsec_237_38, init_wifi_disable
        .literal .Lsec_237_3c, 0x3ff5c080
        .literal .Lsec_237_40, phy_wakeup_init
        .literal .Lsec_237_44, phy_get_romfunc_addr
        .literal .Lsec_237_48, reset_rf_dig
        .literal .Lsec_237_4c, phy_dis_hw_set_freq
        .literal .Lsec_237_50, esp_dport_access_reg_read
        .literal .Lsec_237_54, i2c_master_reset
        .literal .Lsec_237_58, phy_rfcal_data_check
        .literal .Lsec_237_5c, phy_rfcal_data_check_value
        .literal .Lsec_237_60, register_chipv7_phy_init_param
        .literal .Lsec_237_64, rf_cal_data_recovery
        .literal .Lsec_237_68, write_freq_mem_all
        .literal .Lsec_237_6c, rf_init
        .literal .Lsec_237_70, bb_init
        .literal .Lsec_237_74, phy_rfcal_data_check_value
        .literal .Lsec_237_78, rf_cal_data_backup
        .literal .Lsec_237_7c, phy_rfcal_data_check
        .literal .Lsec_237_80, chip_v7_set_chan_nomac
        .literal .Lsec_237_84, force_txrxoff
        .literal .Lsec_237_88, chip_v7_set_chan_nomac
        .literal .Lsec_237_8c, chip_v7_set_chan_offset
        .literal .Lsec_237_90, tsens_code_read
        .literal .Lsec_237_94, phy_en_hw_set_freq
        .literal .Lsec_237_98, enable_wifi_agc
        .literal .Lsec_237_9c, esp_dport_access_reg_read
        .literal .Lsec_237_a0, rf_init
        .literal .Lsec_237_a4, bb_init
.Lsec_237_a8:
        .global register_chipv7_phy
        .type   register_chipv7_phy, @function
register_chipv7_phy:
        entry    a1, 0x100
        l32r     a5, .Lsec_237_4
        extui    a4, a4, 0, 8
        memw
        l32i.n   a5, a5, 0
        beqz.n   a2, .Lsec_237_c1
        l32r     a5, .Lsec_237_8
        l8ui     a6, a5, 0
        bnei     a6, 1, .Lsec_237_cc
.Lsec_237_c1:
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_wakeup_init
        l32r     a8, .Lsec_237_40
        callx8   a8
        movi.n   a2, 0
        retw.n
        .byte    0x00
.Lsec_237_cc:
        l32r     a8, .Lsec_237_c
        movi.n   a7, 1
        l32r     a9, .Lsec_237_10
        s8i      a7, a8, 0
        s8i      a7, a9, 0
        memw
        bnez.n   a6, .Lsec_237_eb
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_get_romfunc_addr
        l32r     a8, .Lsec_237_44
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, reset_rf_dig
        l32r     a8, .Lsec_237_48
        callx8   a8
.Lsec_237_eb:
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_dis_hw_set_freq
        l32r     a8, .Lsec_237_4c
        callx8   a8
        l32r     a6, .Lsec_237_14
        movi     a7, 0xfffffeff
        or       a10, a6, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_237_50
        callx8   a8
        and      a10, a10, a7
        memw
        s32i.n   a10, a6, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, i2c_master_reset
        l32r     a8, .Lsec_237_54
        callx8   a8
        l8ui     a7, a5, 0
        bnez     a7, .Lsec_237_331
        mov.n    a12, a2
        mov.n    a11, a3
        movi.n   a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_rfcal_data_check
        l32r     a8, .Lsec_237_58
        callx8   a8
        mov.n    a6, a10
        movi.n   a9, 1
        bltui    a4, 2, .Lsec_237_129
        mov.n    a9, a7
.Lsec_237_129:
        movi.n   a8, 0
        extui    a7, a9, 0, 8
        movi.n   a10, 1
        mov.n    a9, a8
        movnez   a9, a10, a6
        and      a9, a7, a9
        s32i     a9, a1, 208
        beq      a9, a8, .Lsec_237_141
        j        .Lsec_237_310
.Lsec_237_141:
        addi.n   a8, a4, -1
        movnez   a9, a10, a8
        s32i     a9, a1, 208
        j        .Lsec_237_314
        .byte    0x00
.Lsec_237_14d:
        mov.n    a12, a8
        mov.n    a11, a1
        mov.n    a10, a3
        s32i     a8, a1, 212
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_rfcal_data_check_value
        l32r     a8, .Lsec_237_5c
        callx8   a8
        l32r     a9, .Lsec_237_18
        l32i     a8, a1, 212
        s32i.n   a8, a9, 0
.Lsec_237_164:
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, register_chipv7_phy_init_param
        l32r     a8, .Lsec_237_60
        callx8   a8
        l32r     a10, .Lsec_237_1c
        l8ui     a8, a10, 51
        bnei     a8, 1, .Lsec_237_184
        movi     a9, 0x138
        l32r     a8, .Lsec_237_20
        s16i     a9, a8, 0
        memw
        j        .Lsec_237_190
.Lsec_237_184:
        l32r     a8, .Lsec_237_20
        movi     a9, 0x120
        s16i     a9, a8, 0
        memw
.Lsec_237_190:
        movi.n   a11, 0
        movi.n   a8, 1
        nop.n
        nop.n
        l8ui     a9, a5, 0
        movnez   a8, a11, a9
        and      a8, a7, a8
        l8ui     a11, a10, 14
        l32r     a10, .Lsec_237_24
        s8i      a11, a10, 0
        memw
        beqz     a8, .Lsec_237_1c4
        or       a10, a3, a3
        s32i     a8, a1, 212
        .reloc  ., R_XTENSA_ASM_EXPAND, rf_cal_data_recovery
        l32r     a8, .Lsec_237_64
        callx8   a8
        l32i     a8, a1, 212
        mov.n    a7, a8
        l8ui     a9, a5, 0
.Lsec_237_1c4:
        bnez.n   a9, .Lsec_237_1ee
        l32r     a9, .Lsec_237_18
        l32r     a8, .Lsec_237_28
        l32i.n   a10, a9, 0
        and      a8, a10, a8
        s32i.n   a8, a9, 0
        beqz     a4, .Lsec_237_355
        movi.n   a11, 8
        bgeui    a4, 2, .Lsec_237_1e1
        j        .Lsec_237_328
.Lsec_237_1de:
        or       a11, a8, a11
.Lsec_237_1e1:
        s32i.n   a11, a9, 0
        j        .Lsec_237_1ee
.Lsec_237_1e6:
        addi.n   a10, a3, 12
        .reloc  ., R_XTENSA_ASM_EXPAND, write_freq_mem_all
        l32r     a8, .Lsec_237_68
        callx8   a8
.Lsec_237_1ee:
        .reloc  ., R_XTENSA_ASM_EXPAND, rf_init
        l32r     a8, .Lsec_237_6c
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, bb_init
        l32r     a8, .Lsec_237_70
        callx8   a8
        l8ui     a8, a5, 0
        bnez.n   a8, .Lsec_237_234
        l32i     a4, a1, 208
        beqz.n   a4, .Lsec_237_234
        bnez.n   a6, .Lsec_237_21d
.Lsec_237_206:
        movi.n   a12, 1
        mov.n    a11, a1
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_rfcal_data_check_value
        l32r     a8, .Lsec_237_74
        callx8   a8
        movi.n   a4, 0
        movi     a6, 1
        moveqz   a6, a4, a10
        extui    a6, a6, 0, 8
.Lsec_237_21d:
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, rf_cal_data_backup
        l32r     a8, .Lsec_237_78
        callx8   a8
        mov.n    a12, a2
        mov.n    a11, a3
        movi.n   a10, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_rfcal_data_check
        l32r     a8, .Lsec_237_7c
        callx8   a8
        l8ui     a8, a5, 0
.Lsec_237_234:
        bnei     a8, 1, .Lsec_237_26a
        movi     a13, 99
        movi.n   a12, 0
        movi.n   a11, 4
        l32r     a2, .Lsec_237_2c
        movi     a10, 101
        l32i.n   a3, a2, 0
        l32i     a3, a3, 160
        callx8   a3
        movi.n   a11, 10
        l32r     a3, .Lsec_237_18
        l8ui     a10, a3, 81
        sext     a10, a10, 7
        .reloc  ., R_XTENSA_ASM_EXPAND, chip_v7_set_chan_nomac
        l32r     a8, .Lsec_237_80
        callx8   a8
        movi.n   a10, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, force_txrxoff
        l32r     a8, .Lsec_237_84
        callx8   a8
        j        .Lsec_237_292
        .byte    0x00
        .byte    0x00
.Lsec_237_26a:
        beqz.n   a7, .Lsec_237_27d
        l32r     a2, .Lsec_237_18
        movi.n   a11, 10
        l8ui     a10, a2, 81
        sext     a10, a10, 7
        .reloc  ., R_XTENSA_ASM_EXPAND, chip_v7_set_chan_nomac
        l32r     a8, .Lsec_237_88
        callx8   a8
.Lsec_237_27d:
        l32r     a2, .Lsec_237_1c
        l8ui     a3, a2, 78
        l32r     a2, .Lsec_237_2c
        beqz     a3, .Lsec_237_292
        movi     a10, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, chip_v7_set_chan_offset
        l32r     a8, .Lsec_237_8c
        callx8   a8
.Lsec_237_292:
        movi.n   a12, 0
        l32i.n   a2, a2, 0
        mov.n    a11, a12
        movi     a10, 99
        l32i     a2, a2, 152
        callx8   a2
        l8ui     a2, a5, 0
        bnez.n   a2, .Lsec_237_2c6
        .reloc  ., R_XTENSA_ASM_EXPAND, tsens_code_read
        l32r     a8, .Lsec_237_90
        callx8   a8
        l32r     a2, .Lsec_237_30
        s8i      a10, a2, 0
        memw
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a2, .Lsec_237_34
        s8i      a10, a2, 0
        memw
.Lsec_237_2c6:
        movi.n   a2, 0
        movi.n   a3, 1
        nop
        nop
        l32r     a8, .Lsec_237_c
        l32r     a9, .Lsec_237_10
        s8i      a2, a8, 0
        s8i      a2, a9, 0
        s8i      a3, a5, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_en_hw_set_freq
        l32r     a8, .Lsec_237_94
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, enable_wifi_agc
        l32r     a8, .Lsec_237_98
        callx8   a8
        l32r     a2, .Lsec_237_38
        l8ui     a2, a2, 0
        beqz.n   a2, .Lsec_237_30c
        movi     a2, 192
        l32r     a3, .Lsec_237_3c
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_237_9c
        callx8   a8
        or       a10, a10, a2
        memw
        s32i.n   a10, a3, 0
.Lsec_237_30c:
        mov.n    a2, a6
        retw.n
.Lsec_237_310:
        mov.n    a7, a8
        movi.n   a4, 2
.Lsec_237_314:
        movi.n   a8, 0
        beq      a6, a8, .Lsec_237_31c
        j        .Lsec_237_164
.Lsec_237_31c:
        l32i     a9, a1, 208
        bne      a9, a8, .Lsec_237_325
        j        .Lsec_237_164
.Lsec_237_325:
        j        .Lsec_237_14d
.Lsec_237_328:
        bbci     a10, 4, .Lsec_237_32e
        j        .Lsec_237_1e6
.Lsec_237_32e:
        j        .Lsec_237_1de
.Lsec_237_331:
        movi.n   a7, 1
        bltui    a4, 2, .Lsec_237_338
        movi.n   a7, 0
.Lsec_237_338:
        .reloc  ., R_XTENSA_ASM_EXPAND, rf_init
        l32r     a8, .Lsec_237_a0
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, bb_init
        l32r     a8, .Lsec_237_a4
        callx8   a8
        extui    a7, a7, 0, 8
        l8ui     a8, a5, 0
        beqi     a4, 1, .Lsec_237_350
        beqz     a8, .Lsec_237_206
.Lsec_237_350:
        movi.n   a6, 0
        j        .Lsec_237_234
.Lsec_237_355:
        l32r     a11, .Lsec_237_0
        j        .Lsec_237_1e1
        .size   register_chipv7_phy, .-register_chipv7_phy
.Lsec_237_35b:

        .section .text.phy_set_most_tpw,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_238_0, phy_in_most_power
        .literal .Lsec_238_4, phy_set_most_tpw_index
        .literal .Lsec_238_8, chip7_sleep_params
        .literal .Lsec_238_c, phy_set_most_tpw_flag
        .literal .Lsec_238_10, set_most_pwr_reg
        .literal .Lsec_238_14, set_chan_dig_gain
.Lsec_238_18:
        .global phy_set_most_tpw
        .type   phy_set_most_tpw, @function
phy_set_most_tpw:
        entry    a1, 32
        l32r     a8, .Lsec_238_0
        s8i      a2, a8, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, set_most_pwr_reg
        l32r     a8, .Lsec_238_10
        callx8   a8
        l32r     a8, .Lsec_238_4
        s8i      a10, a8, 0
        memw
        nop.n
        nop.n
        nop.n
        nop
        l32r     a8, .Lsec_238_8
        l8ui     a10, a8, 81
        .reloc  ., R_XTENSA_ASM_EXPAND, set_chan_dig_gain
        l32r     a8, .Lsec_238_14
        callx8   a8
        movi.n   a9, 1
        l32r     a8, .Lsec_238_c
        s8i      a9, a8, 0
        memw
        retw.n
        .size   phy_set_most_tpw, .-phy_set_most_tpw
.Lsec_238_55:

        .section .text.phy_get_most_tpw,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_239_0, chip7_phy_init_ctrl
        .literal .Lsec_239_4, phy_set_most_tpw_flag
        .literal .Lsec_239_8, set_most_tpw
.Lsec_239_c:
        .global phy_get_most_tpw
        .type   phy_get_most_tpw, @function
phy_get_most_tpw:
        entry    a1, 32
        l32r     a2, .Lsec_239_4
        l8ui     a8, a2, 0
        l32r     a2, .Lsec_239_0
        l8ui     a2, a2, 14
        beqi     a8, 1, .Lsec_239_24
        extui    a2, a2, 0, 8
        retw.n
        .byte    0x00
.Lsec_239_24:
        l32r     a2, .Lsec_239_8
        l8ui     a2, a2, 0
        retw.n
        .size   phy_get_most_tpw, .-phy_get_most_tpw
.Lsec_239_2c:

        .section .text.phy_rx_sense_set,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_240_0, 0x3ff5c010
        .literal .Lsec_240_4, 0x007fffff
        .literal .Lsec_240_8, 0x3ff5c014
        .literal .Lsec_240_c, 0x3ff5c044
        .literal .Lsec_240_10, 0x3ff5c108
        .literal .Lsec_240_14, esp_dport_access_reg_read
        .literal .Lsec_240_18, esp_dport_access_reg_read
        .literal .Lsec_240_1c, esp_dport_access_reg_read
        .literal .Lsec_240_20, esp_dport_access_reg_read
        .literal .Lsec_240_24, esp_dport_access_reg_read
.Lsec_240_28:
        .global phy_rx_sense_set
        .type   phy_rx_sense_set, @function
phy_rx_sense_set:
        entry    a1, 32
        l32r     a6, .Lsec_240_0
        extui    a2, a2, 0, 8
        mov.n    a10, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_240_14
        callx8   a8
        slli     a3, a2, 23
        l32r     a4, .Lsec_240_4
        and      a10, a10, a4
        or       a8, a10, a3
        l32r     a5, .Lsec_240_8
        mov.n    a10, a5
        memw
        s32i.n   a8, a6, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_240_18
        callx8   a8
        and      a10, a10, a4
        or       a3, a10, a3
        l32r     a4, .Lsec_240_c
        or       a10, a4, a4
        memw
        s32i     a3, a5, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_240_1c
        callx8   a8
        movi     a8, 0xffffff00
        and      a10, a10, a8
        or       a10, a10, a2
        l32r     a3, .Lsec_240_10
        memw
        s32i.n   a10, a4, 0
        mov.n    a10, a3
        bnez.n   a2, .Lsec_240_95
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_240_20
        callx8   a8
        movi     a2, 0x200
        or       a10, a10, a2
        memw
        s32i.n   a10, a3, 0
        retw.n
.Lsec_240_95:
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_240_24
        callx8   a8
        movi     a2, 0xfffffdff
        and      a10, a10, a2
        memw
        s32i.n   a10, a3, 0
        retw.n
        .size   phy_rx_sense_set, .-phy_rx_sense_set
.Lsec_240_a8:

        .section .text.ant_dft_cfg,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_241_0, 0x3ff5c11c
        .literal .Lsec_241_4, 0xfffff7ff
        .literal .Lsec_241_8, esp_dport_access_reg_read
.Lsec_241_c:
        .global ant_dft_cfg
        .type   ant_dft_cfg, @function
ant_dft_cfg:
        entry    a1, 32
        l32r     a3, .Lsec_241_0
        extui    a2, a2, 0, 8
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_241_8
        callx8   a8
        extui    a2, a2, 0, 1
        l32r     a8, .Lsec_241_4
        slli     a2, a2, 11
        and      a10, a10, a8
        or       a10, a2, a10
        memw
        s32i.n   a10, a3, 0
        retw.n
        .size   ant_dft_cfg, .-ant_dft_cfg
.Lsec_241_33:

        .section .text.ant_wifitx_cfg,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_242_0, 0x3ff45104
        .literal .Lsec_242_4, 0xfffff0ff
        .literal .Lsec_242_8, 0xfff0ffff
        .literal .Lsec_242_c, esp_dport_access_reg_read
        .literal .Lsec_242_10, esp_dport_access_reg_read
.Lsec_242_14:
        .global ant_wifitx_cfg
        .type   ant_wifitx_cfg, @function
ant_wifitx_cfg:
        entry    a1, 32
        l32r     a4, .Lsec_242_0
        extui    a2, a2, 0, 8
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_242_c
        callx8   a8
        extui    a8, a2, 0, 4
        l32r     a9, .Lsec_242_4
        slli     a8, a8, 8
        and      a10, a10, a9
        or       a8, a8, a10
        mov.n    a10, a4
        memw
        s32i.n   a8, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_242_10
        callx8   a8
        extui    a3, a3, 0, 8
        extui    a3, a3, 0, 4
        slli     a3, a3, 16
        l32r     a8, .Lsec_242_8
        and      a10, a10, a8
        or       a10, a3, a10
        memw
        s32i.n   a10, a4, 0
        retw.n
        .size   ant_wifitx_cfg, .-ant_wifitx_cfg
.Lsec_242_5a:

        .section .text.ant_wifirx_cfg,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_243_0, 0x3ff5c11c
        .literal .Lsec_243_4, 0x3ff45104
        .literal .Lsec_243_8, 0xf0ffffff
        .literal .Lsec_243_c, 0x3ff45108
        .literal .Lsec_243_10, 0xfffff0ff
        .literal .Lsec_243_14, 0xfff0ffff
        .literal .Lsec_243_18, esp_dport_access_reg_read
        .literal .Lsec_243_1c, esp_dport_access_reg_read
        .literal .Lsec_243_20, esp_dport_access_reg_read
        .literal .Lsec_243_24, esp_dport_access_reg_read
        .literal .Lsec_243_28, esp_dport_access_reg_read
.Lsec_243_2c:
        .global ant_wifirx_cfg
        .type   ant_wifirx_cfg, @function
ant_wifirx_cfg:
        entry    a1, 32
        l32r     a6, .Lsec_243_0
        extui    a2, a2, 0, 8
        or       a10, a6, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_243_18
        callx8   a8
        movi.n   a8, -3
        extui    a2, a2, 0, 1
        and      a10, a10, a8
        l32r     a5, .Lsec_243_4
        slli     a2, a2, 1
        or       a2, a2, a10
        mov.n    a10, a5
        extui    a3, a3, 0, 8
        memw
        s32i     a2, a6, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_243_1c
        callx8   a8
        extui    a3, a3, 0, 4
        slli     a2, a3, 24
        extui    a4, a4, 0, 8
        l32r     a6, .Lsec_243_8
        extui    a4, a4, 0, 4
        and      a10, a10, a6
        or       a6, a10, a2
        l32r     a2, .Lsec_243_c
        mov.n    a10, a2
        memw
        s32i.n   a6, a5, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_243_20
        callx8   a8
        movi.n   a5, -16
        and      a10, a10, a5
        or       a3, a10, a3
        memw
        s32i     a3, a2, 0
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_243_24
        callx8   a8
        slli     a5, a4, 8
        slli     a4, a4, 16
        l32r     a3, .Lsec_243_10
        and      a10, a10, a3
        or       a3, a10, a5
        mov.n    a10, a2
        memw
        s32i.n   a3, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_243_28
        callx8   a8
        l32r     a3, .Lsec_243_14
        and      a10, a10, a3
        or       a4, a10, a4
        memw
        s32i.n   a4, a2, 0
        retw.n
        .size   ant_wifirx_cfg, .-ant_wifirx_cfg
.Lsec_243_c8:

        .section .text.ant_bttx_cfg,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_244_0, 0x3ff45108
        .literal .Lsec_244_4, 0xf0ffffff
        .literal .Lsec_244_8, 0x3ff4510c
        .literal .Lsec_244_c, esp_dport_access_reg_read
        .literal .Lsec_244_10, esp_dport_access_reg_read
.Lsec_244_14:
        .global ant_bttx_cfg
        .type   ant_bttx_cfg, @function
ant_bttx_cfg:
        entry    a1, 32
        l32r     a4, .Lsec_244_0
        extui    a2, a2, 0, 8
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_244_c
        callx8   a8
        extui    a2, a2, 0, 4
        l32r     a8, .Lsec_244_4
        slli     a3, a2, 24
        and      a10, a10, a8
        or       a8, a10, a3
        l32r     a3, .Lsec_244_8
        mov.n    a10, a3
        memw
        s32i.n   a8, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_244_10
        callx8   a8
        movi.n   a8, -16
        and      a10, a10, a8
        or       a2, a10, a2
        memw
        s32i.n   a2, a3, 0
        retw.n
        .size   ant_bttx_cfg, .-ant_bttx_cfg
.Lsec_244_53:

        .section .text.ant_btrx_cfg,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_245_0, 0x3ff5c11c
        .literal .Lsec_245_4, 0x3ff4510c
        .literal .Lsec_245_8, 0xfffff0ff
        .literal .Lsec_245_c, 0xfff0ffff
        .literal .Lsec_245_10, 0xf0ffffff
        .literal .Lsec_245_14, 0x3ff45110
        .literal .Lsec_245_18, esp_dport_access_reg_read
        .literal .Lsec_245_1c, esp_dport_access_reg_read
        .literal .Lsec_245_20, esp_dport_access_reg_read
        .literal .Lsec_245_24, esp_dport_access_reg_read
        .literal .Lsec_245_28, esp_dport_access_reg_read
.Lsec_245_2c:
        .global ant_btrx_cfg
        .type   ant_btrx_cfg, @function
ant_btrx_cfg:
        entry    a1, 32
        l32r     a6, .Lsec_245_0
        extui    a2, a2, 0, 8
        or       a10, a6, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_245_18
        callx8   a8
        movi.n   a8, -9
        extui    a2, a2, 0, 1
        l32r     a5, .Lsec_245_4
        and      a10, a10, a8
        slli     a2, a2, 3
        or       a8, a2, a10
        mov.n    a10, a5
        memw
        s32i.n   a8, a6, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_245_1c
        callx8   a8
        extui    a3, a3, 0, 8
        extui    a3, a3, 0, 4
        slli     a2, a3, 8
        l32r     a6, .Lsec_245_8
        slli     a3, a3, 16
        and      a10, a10, a6
        or       a2, a10, a2
        or       a10, a5, a5
        memw
        s32i     a2, a5, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_245_20
        callx8   a8
        extui    a4, a4, 0, 8
        extui    a4, a4, 0, 4
        l32r     a2, .Lsec_245_c
        and      a10, a10, a2
        or       a3, a10, a3
        mov.n    a10, a5
        memw
        s32i.n   a3, a5, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_245_24
        callx8   a8
        slli     a3, a4, 24
        l32r     a2, .Lsec_245_10
        and      a10, a10, a2
        or       a2, a10, a3
        l32r     a3, .Lsec_245_14
        mov.n    a10, a3
        memw
        s32i.n   a2, a5, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_245_28
        callx8   a8
        movi.n   a2, -16
        and      a10, a10, a2
        or       a4, a10, a4
        memw
        s32i.n   a4, a3, 0
        retw.n
        .size   ant_btrx_cfg, .-ant_btrx_cfg
.Lsec_245_c7:

        .section .text.ant_tx_cfg,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_246_0, ant_wifitx_cfg
        .literal .Lsec_246_4, ant_bttx_cfg
.Lsec_246_8:
        .global ant_tx_cfg
        .type   ant_tx_cfg, @function
ant_tx_cfg:
        entry    a1, 32
        extui    a2, a2, 0, 8
        mov.n    a10, a2
        mov.n    a11, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, ant_wifitx_cfg
        l32r     a8, .Lsec_246_0
        callx8   a8
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, ant_bttx_cfg
        l32r     a8, .Lsec_246_4
        callx8   a8
        retw.n
        .size   ant_tx_cfg, .-ant_tx_cfg
.Lsec_246_22:

        .section .text.ant_rx_cfg,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_247_0, ant_wifirx_cfg
        .literal .Lsec_247_4, ant_btrx_cfg
.Lsec_247_8:
        .global ant_rx_cfg
        .type   ant_rx_cfg, @function
ant_rx_cfg:
        entry    a1, 32
        extui    a2, a2, 0, 8
        extui    a3, a3, 0, 8
        extui    a4, a4, 0, 8
        mov.n    a12, a4
        mov.n    a11, a3
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, ant_wifirx_cfg
        l32r     a8, .Lsec_247_0
        callx8   a8
        mov.n    a12, a4
        mov.n    a11, a3
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, ant_btrx_cfg
        l32r     a8, .Lsec_247_4
        callx8   a8
        retw.n
        .size   ant_rx_cfg, .-ant_rx_cfg
.Lsec_247_2e:

        .section .text.esp_tx_state_out,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_248_0, 0x40000000
        .literal .Lsec_248_4, 0x80000000
        .literal .Lsec_248_8, 0x0ffd114c
        .literal .Lsec_248_c, 0x3ff45104
        .literal .Lsec_248_10, 0x3f3f3f3f
        .literal .Lsec_248_14, 0x00404000
        .literal .Lsec_248_18, 0x3ff45108
        .literal .Lsec_248_1c, 0x3ff4510c
        .literal .Lsec_248_20, 0x3ff45110
        .literal .Lsec_248_24, esp_dport_access_reg_read
        .literal .Lsec_248_28, esp_dport_access_reg_read
        .literal .Lsec_248_2c, esp_dport_access_reg_read
        .literal .Lsec_248_30, esp_dport_access_reg_read
        .literal .Lsec_248_34, esp_dport_access_reg_read
        .literal .Lsec_248_38, esp_dport_access_reg_read
.Lsec_248_3c:
        .global esp_tx_state_out
        .type   esp_tx_state_out, @function
esp_tx_state_out:
        entry    a1, 32
        l32r     a4, .Lsec_248_8
        extui    a2, a2, 0, 8
        add      a5, a2, a4
        slli     a5, a5, 2
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_248_24
        callx8   a8
        movi     a6, 0xfffffe00
        and      a10, a10, a6
        movi     a8, 222
        or       a10, a10, a8
        extui    a3, a3, 0, 8
        memw
        s32i.n   a10, a5, 0
        beq      a2, a3, .Lsec_248_95
        add.n    a4, a3, a4
        slli     a4, a4, 2
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_248_28
        callx8   a8
        and      a10, a10, a6
        movi     a6, 223
        or       a10, a10, a6
        memw
        s32i.n   a10, a4, 0
        movi     a5, 128
        nop.n
        nop.n
        nop.n
        l32r     a3, .Lsec_248_4
        j        .Lsec_248_9a
.Lsec_248_95:
        l32r     a3, .Lsec_248_0
        movi.n   a5, 64
.Lsec_248_9a:
        l32r     a6, .Lsec_248_c
        mov.n    a10, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_248_2c
        callx8   a8
        l32r     a2, .Lsec_248_10
        and      a10, a10, a2
        l32r     a4, .Lsec_248_14
        or       a4, a10, a4
        l32r     a7, .Lsec_248_18
        mov.n    a10, a7
        memw
        s32i.n   a4, a6, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_248_30
        callx8   a8
        and      a10, a10, a2
        or       a3, a10, a3
        l32r     a4, .Lsec_248_1c
        or       a10, a4, a4
        memw
        s32i.n   a3, a7, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_248_34
        callx8   a8
        and      a10, a10, a2
        or       a2, a10, a5
        l32r     a3, .Lsec_248_20
        or       a10, a3, a3
        memw
        s32i     a2, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_248_38
        callx8   a8
        movi     a2, -193
        and      a10, a10, a2
        memw
        s32i.n   a10, a3, 0
        retw.n
        .size   esp_tx_state_out, .-esp_tx_state_out
.Lsec_248_fd:

        .section .text.phy_chan_dump_cfg,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_249_0, 0x3ff5cd0c
        .literal .Lsec_249_4, esp_dport_access_reg_read
        .literal .Lsec_249_8, esp_dport_access_reg_read
        .literal .Lsec_249_c, esp_dport_access_reg_read
        .literal .Lsec_249_10, esp_dport_access_reg_read
        .literal .Lsec_249_14, esp_dport_access_reg_read
.Lsec_249_18:
        .global phy_chan_dump_cfg
        .type   phy_chan_dump_cfg, @function
phy_chan_dump_cfg:
        entry    a1, 32
        l32r     a7, .Lsec_249_0
        extui    a2, a2, 0, 8
        or       a10, a7, a7
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_249_4
        callx8   a8
        movi.n   a8, -9
        extui    a2, a2, 0, 1
        and      a10, a10, a8
        slli     a2, a2, 3
        or       a10, a2, a10
        memw
        s32i.n   a10, a7, 0
        mov.n    a10, a7
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_249_8
        callx8   a8
        extui    a3, a3, 0, 8
        movi     a8, -241
        slli     a3, a3, 4
        and      a10, a10, a8
        extui    a3, a3, 0, 8
        or       a10, a3, a10
        memw
        s32i.n   a10, a7, 0
        mov.n    a10, a7
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_249_c
        callx8   a8
        extui    a4, a4, 0, 8
        movi     a8, -3
        extui    a4, a4, 0, 1
        and      a10, a10, a8
        slli     a4, a4, 1
        or       a10, a4, a10
        memw
        s32i     a10, a7, 0
        or       a10, a7, a7
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_249_10
        callx8   a8
        extui    a5, a5, 0, 8
        movi.n   a2, -2
        and      a10, a10, a2
        extui    a5, a5, 0, 1
        or       a5, a10, a5
        memw
        s32i.n   a5, a7, 0
        mov.n    a10, a7
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_249_14
        callx8   a8
        extui    a6, a6, 0, 8
        extui    a6, a6, 0, 1
        movi.n   a8, -5
        slli     a6, a6, 2
        and      a10, a10, a8
        or       a10, a6, a10
        memw
        s32i.n   a10, a7, 0
        retw.n
        .size   phy_chan_dump_cfg, .-phy_chan_dump_cfg
.Lsec_249_b8:

        .section .text.chan14_mic_cfg,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_250_0, 0x3ff5c400
        .literal .Lsec_250_4, 0xffff9fff
        .literal .Lsec_250_8, 0x00002000
        .literal .Lsec_250_c, g_phyFuns
        .literal .Lsec_250_10, chan14_mic_most_power
        .literal .Lsec_250_14, 0x00006000
        .literal .Lsec_250_18, chip7_sleep_params
        .literal .Lsec_250_1c, phy_in_most_power_bk
        .literal .Lsec_250_20, esp_dport_access_reg_read
        .literal .Lsec_250_24, phy_set_most_tpw
        .literal .Lsec_250_28, esp_dport_access_reg_read
        .literal .Lsec_250_2c, phy_set_most_tpw
.Lsec_250_30:
        .global chan14_mic_cfg
        .type   chan14_mic_cfg, @function
chan14_mic_cfg:
        entry    a1, 32
        extui    a2, a2, 0, 8
        bnei     a2, 1, .Lsec_250_9e
        l32r     a4, .Lsec_250_0
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_250_20
        callx8   a8
        mov.n    a11, a2
        l32r     a3, .Lsec_250_4
        movi.n   a15, 76
        and      a10, a10, a3
        movi.n   a14, 0
        movi.n   a13, 6
        l32r     a3, .Lsec_250_8
        movi.n   a12, 3
        or       a3, a10, a3
        movi     a10, 103
        l32r     a5, .Lsec_250_c
        memw
        s32i.n   a3, a4, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a3, a5, 0
        l32i     a3, a3, 168
        callx8   a3
        mov.n    a11, a2
        movi.n   a15, 78
        l32i.n   a3, a5, 0
        movi     a14, 0
        movi     a13, 6
        movi     a12, 4
        movi     a10, 103
        l32i     a2, a3, 168
        callx8   a2
        l32r     a2, .Lsec_250_10
        l8ui     a10, a2, 0
        sext     a10, a10, 7
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_set_most_tpw
        l32r     a8, .Lsec_250_24
        callx8   a8
        retw.n
.Lsec_250_9e:
        l32r     a5, .Lsec_250_0
        movi     a4, 0
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_250_28
        callx8   a8
        movi.n   a15, 2
        l32r     a2, .Lsec_250_18
        mov.n    a6, a15
        movi.n   a3, 1
        mov.n    a14, a4
        mov.n    a11, a3
        l8ui     a8, a2, 82
        movi.n   a13, 6
        moveqz   a6, a4, a8
        add.n    a2, a2, a6
        movnez   a15, a3, a8
        l32r     a9, .Lsec_250_14
        mov.n    a8, a15
        or       a9, a10, a9
        movi.n   a12, 3
        movi     a10, 103
        l32r     a6, .Lsec_250_c
        memw
        s32i.n   a9, a5, 0
        nop.n
        nop.n
        nop.n
        nop
        l8ui     a15, a2, 84
        sub      a15, a15, a8
        extui    a15, a15, 0, 8
        l32i     a5, a6, 0
        l32i     a5, a5, 168
        callx8   a5
        mov.n    a14, a4
        mov.n    a11, a3
        l32i.n   a4, a6, 0
        movi     a10, 103
        movi.n   a13, 6
        movi     a12, 4
        l8ui     a15, a2, 84
        l32i     a3, a4, 168
        callx8   a3
        l32r     a2, .Lsec_250_1c
        l8ui     a10, a2, 0
        sext     a10, a10, 7
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_set_most_tpw
        l32r     a8, .Lsec_250_2c
        callx8   a8
        retw.n
        .size   chan14_mic_cfg, .-chan14_mic_cfg
.Lsec_250_11e:

        .section .text.chan14_mic_enable,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_251_0, chan14_mic_en
        .literal .Lsec_251_4, chan14_mic_flag
        .literal .Lsec_251_8, phy_in_most_power_bk
        .literal .Lsec_251_c, phy_in_most_power
        .literal .Lsec_251_10, chan14_mic_most_power
        .literal .Lsec_251_14, chan14_mic_cfg
.Lsec_251_18:
        .global chan14_mic_enable
        .type   chan14_mic_enable, @function
chan14_mic_enable:
        entry    a1, 32
        l32r     a8, .Lsec_251_0
        extui    a10, a2, 0, 8
        extui    a3, a3, 0, 8
        s8i      a10, a8, 0
        memw
        bnez.n   a10, .Lsec_251_35
        .reloc  ., R_XTENSA_ASM_EXPAND, chan14_mic_cfg
        l32r     a8, .Lsec_251_14
        callx8   a8
        retw.n
        .byte    0x00
.Lsec_251_35:
        l32r     a8, .Lsec_251_4
        l8ui     a9, a8, 0
        bnez.n   a9, .Lsec_251_5c
        movi.n   a10, 1
        l32r     a9, .Lsec_251_c
        s8i      a10, a8, 0
        memw
        nop.n
        nop.n
        nop.n
        nop.n
        l8ui     a9, a9, 0
        l32r     a8, .Lsec_251_8
        s8i      a9, a8, 0
        memw
.Lsec_251_5c:
        movi.n   a8, 48
        sext     a3, a3, 7
        min      a3, a3, a8
        nop.n
        l32r     a8, .Lsec_251_10
        s8i      a3, a8, 0
        memw
        retw.n
        .size   chan14_mic_enable, .-chan14_mic_enable
.Lsec_251_71:

        .section .text.phy_get_adc_rand,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_252_0, .Lsec_137_a0
        .literal .Lsec_252_4, 0x3ff48030
        .literal .Lsec_252_8, adc_ana_conf_org
        .literal .Lsec_252_c, g_phyFuns
        .literal .Lsec_252_10, 0xf8000000
        .literal .Lsec_252_14, 0x3ff460a0
        .literal .Lsec_252_18, 0x01000000
        .literal .Lsec_252_1c, 0x3ff66000
        .literal .Lsec_252_20, 0x00000fa0
        .literal .Lsec_252_24, esp_dport_access_reg_read
        .literal .Lsec_252_28, esp_dport_access_reg_read
        .literal .Lsec_252_2c, esp_dport_access_reg_read
        .literal .Lsec_252_30, esp_dport_access_reg_read
.Lsec_252_34:
        .global phy_get_adc_rand
        .type   phy_get_adc_rand, @function
phy_get_adc_rand:
        entry    a1, 64
        l32r     a4, .Lsec_252_0
        extui    a2, a2, 0, 8
        l32r     a3, .Lsec_252_4
        mov.n    a10, a3
        l16ui    a7, a4, 0
        l16ui    a6, a4, 2
        l16ui    a5, a4, 4
        l16ui    a4, a4, 6
        s16i     a7, a1, 8
        s16i     a4, a1, 16
        s16i     a4, a1, 14
        s16i     a6, a1, 10
        s16i     a5, a1, 12
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_252_24
        callx8   a8
        l32r     a4, .Lsec_252_8
        s32i.n   a10, a4, 0
        beqz     a2, .Lsec_252_141
        movi.n   a10, 1
        nop.n
        nop.n
        nop.n
        l32r     a4, .Lsec_252_c
        l32i.n   a2, a4, 0
        l32i     a2, a2, 64
        callx8   a2
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_252_28
        callx8   a8
        l32r     a8, .Lsec_252_10
        or       a8, a10, a8
        l32r     a2, .Lsec_252_14
        or       a10, a2, a2
        memw
        s32i.n   a8, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_252_2c
        callx8   a8
        l32r     a8, .Lsec_252_18
        or       a8, a10, a8
        l32r     a3, .Lsec_252_1c
        or       a10, a3, a3
        memw
        s32i     a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_252_30
        callx8   a8
        movi     a2, 0x400
        or       a10, a10, a2
        movi.n   a14, 7
        l32i.n   a2, a4, 0
        movi.n   a12, 1
        mov.n    a13, a14
        mov.n    a11, a12
        movi.n   a15, 0
        l32i     a2, a2, 168
        memw
        s32i.n   a10, a3, 0
        movi     a10, 103
        callx8   a2
        movi.n   a14, 7
        mov.n    a13, a14
        l32i.n   a2, a4, 0
        movi.n   a15, 0
        movi.n   a12, 2
        movi     a11, 1
        movi     a10, 103
        l32i     a2, a2, 168
        callx8   a2
        l32i.n   a2, a4, 0
        l32i     a2, a2, 192
        callx8   a2
        mov.n    a10, a1
        l32i.n   a2, a4, 0
        l16ui    a3, a1, 16
        l32i     a2, a2, 220
        s16i     a7, a1, 0
        s16i     a6, a1, 2
        s16i     a5, a1, 4
        s16i     a3, a1, 6
        memw
        callx8   a2
        movi.n   a10, 0
        l32i.n   a2, a4, 0
        l32i     a2, a2, 208
        callx8   a2
        movi.n   a12, 0
        mov.n    a10, a12
        l32i.n   a2, a4, 0
        movi.n   a11, 1
        l32i     a2, a2, 184
        callx8   a2
        movi.n   a14, 0
        mov.n    a13, a14
        l32i.n   a2, a4, 0
        movi.n   a12, 10
        addi.n   a11, a1, 8
        l32i     a2, a2, 0x150
        l32r     a10, .Lsec_252_20
        callx8   a2
        retw.n
        .byte    0x00
        .byte    0x00
.Lsec_252_141:
        l32r     a5, .Lsec_252_c
        l32i.n   a6, a5, 0
        l32i     a6, a6, 204
        callx8   a6
        l32i.n   a6, a5, 0
        l32i     a6, a6, 196
        callx8   a6
        movi.n   a15, 1
        movi.n   a14, 7
        l32i.n   a6, a5, 0
        mov.n    a13, a14
        mov.n    a12, a15
        mov.n    a11, a15
        movi     a10, 103
        l32i     a6, a6, 168
        callx8   a6
        movi.n   a15, 1
        movi.n   a14, 7
        l32i.n   a6, a5, 0
        movi     a10, 103
        mov.n    a13, a14
        movi.n   a12, 2
        mov.n    a11, a15
        l32i     a6, a6, 168
        callx8   a6
        mov.n    a10, a2
        l32i.n   a2, a4, 0
        memw
        s32i.n   a2, a3, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32i     a2, a5, 0
        l32i     a2, a2, 64
        callx8   a2
        retw.n
        .size   phy_get_adc_rand, .-phy_get_adc_rand
.Lsec_252_19a:

        .section .text.phy_enable_low_rate,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_253_0, lr_enable
        .literal .Lsec_253_4, 0x3ff5c860
        .literal .Lsec_253_8, 0x00000800
        .literal .Lsec_253_c, 0x3ff5c87c
        .literal .Lsec_253_10, esp_dport_access_reg_read
        .literal .Lsec_253_14, esp_dport_access_reg_read
        .literal .Lsec_253_18, esp_dport_access_reg_read
        .literal .Lsec_253_1c, write_txrate_power_offset
.Lsec_253_20:
        .global phy_enable_low_rate
        .type   phy_enable_low_rate, @function
phy_enable_low_rate:
        entry    a1, 32
        l32r     a3, .Lsec_253_4
        movi.n   a4, 1
        mov.n    a10, a3
        l32r     a2, .Lsec_253_0
        s8i      a4, a2, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_253_10
        callx8   a8
        movi     a2, 0x400
        or       a2, a10, a2
        mov.n    a10, a3
        memw
        s32i.n   a2, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_253_14
        callx8   a8
        l32r     a2, .Lsec_253_8
        or       a8, a10, a2
        l32r     a4, .Lsec_253_c
        or       a10, a4, a4
        memw
        s32i     a8, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_253_18
        callx8   a8
        or       a2, a10, a2
        movi.n   a10, 1
        memw
        s32i.n   a2, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, write_txrate_power_offset
        l32r     a8, .Lsec_253_1c
        callx8   a8
        retw.n
        .size   phy_enable_low_rate, .-phy_enable_low_rate
.Lsec_253_76:

        .section .text.phy_disable_low_rate,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_254_0, lr_enable
        .literal .Lsec_254_4, 0x3ff5c860
        .literal .Lsec_254_8, 0xfffff7ff
        .literal .Lsec_254_c, 0x3ff5c87c
        .literal .Lsec_254_10, esp_dport_access_reg_read
        .literal .Lsec_254_14, esp_dport_access_reg_read
        .literal .Lsec_254_18, esp_dport_access_reg_read
        .literal .Lsec_254_1c, write_txrate_power_offset
.Lsec_254_20:
        .global phy_disable_low_rate
        .type   phy_disable_low_rate, @function
phy_disable_low_rate:
        entry    a1, 32
        l32r     a3, .Lsec_254_4
        movi.n   a4, 0
        mov.n    a10, a3
        l32r     a2, .Lsec_254_0
        s8i      a4, a2, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_254_10
        callx8   a8
        movi     a2, 0xfffffbff
        and      a2, a10, a2
        mov.n    a10, a3
        memw
        s32i.n   a2, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_254_14
        callx8   a8
        l32r     a2, .Lsec_254_8
        and      a8, a10, a2
        l32r     a4, .Lsec_254_c
        or       a10, a4, a4
        memw
        s32i     a8, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_254_18
        callx8   a8
        and      a2, a10, a2
        movi.n   a10, 1
        memw
        s32i.n   a2, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, write_txrate_power_offset
        l32r     a8, .Lsec_254_1c
        callx8   a8
        retw.n
        .size   phy_disable_low_rate, .-phy_disable_low_rate
.Lsec_254_76:

        .section .text.phy_close_pa,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_255_0, 0x3ff460a0
        .literal .Lsec_255_4, 0xfffff3ff
        .literal .Lsec_255_8, 0x00000800
        .literal .Lsec_255_c, 0x3ff4609c
        .literal .Lsec_255_10, esp_dport_access_reg_read
        .literal .Lsec_255_14, esp_dport_access_reg_read
        .literal .Lsec_255_18, esp_dport_access_reg_read
        .literal .Lsec_255_1c, esp_dport_access_reg_read
        .literal .Lsec_255_20, esp_dport_access_reg_read
        .literal .Lsec_255_24, esp_dport_access_reg_read
.Lsec_255_28:
        .global phy_close_pa
        .type   phy_close_pa, @function
phy_close_pa:
        entry    a1, 32
        extui    a2, a2, 0, 8
        beqz     a2, .Lsec_255_79
        l32r     a4, .Lsec_255_0
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_255_10
        callx8   a8
        l32r     a3, .Lsec_255_4
        and      a10, a10, a3
        l32r     a2, .Lsec_255_8
        or       a3, a10, a2
        l32r     a2, .Lsec_255_c
        mov.n    a10, a2
        memw
        s32i.n   a3, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_255_14
        callx8   a8
        movi.n   a3, -5
        and      a3, a10, a3
        mov.n    a10, a2
        memw
        s32i.n   a3, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_255_18
        callx8   a8
        movi.n   a3, -17
        and      a10, a10, a3
        memw
        s32i.n   a10, a2, 0
        retw.n
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_255_79:
        l32r     a2, .Lsec_255_c
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_255_1c
        callx8   a8
        movi.n   a8, 4
        or       a10, a10, a8
        memw
        s32i.n   a10, a2, 0
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_255_20
        callx8   a8
        movi.n   a8, 16
        or       a8, a10, a8
        l32r     a3, .Lsec_255_0
        mov.n    a10, a3
        memw
        s32i     a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_255_24
        callx8   a8
        l32r     a2, .Lsec_255_4
        and      a10, a10, a2
        memw
        s32i.n   a10, a3, 0
        retw.n
        .size   phy_close_pa, .-phy_close_pa
.Lsec_255_b9:

        .section .text.btpwr_backoff,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_256_0, chip7_sleep_params+0x81
        .literal .Lsec_256_4, chip7_sleep_params
        .literal .Lsec_256_8, bt_txpwr_backoff
        .literal .Lsec_256_c, wr_bt_tx_atten
.Lsec_256_10:
        .global btpwr_backoff
        .type   btpwr_backoff, @function
btpwr_backoff:
        entry    a1, 32
        l32r     a3, .Lsec_256_8
        extui    a2, a2, 0, 8
        movi.n   a13, 64
        movi     a14, 0
        movi     a10, 8
        l8ui     a12, a3, 0
        sub      a12, a2, a12
        extui    a12, a12, 0, 8
        l32r     a9, .Lsec_256_4
        loop     a10, .Lsec_256_5c
        l8ui     a8, a9, 129
        add.n    a8, a12, a8
        extui    a8, a8, 0, 8
        sext     a11, a8, 7
        bgez     a11, .Lsec_256_48
        s8i      a14, a9, 129
        memw
        j        .Lsec_256_5a
        .byte    0x00
.Lsec_256_48:
        blt      a13, a11, .Lsec_256_54
        s8i      a8, a9, 129
        memw
        j        .Lsec_256_5a
.Lsec_256_54:
        s8i      a13, a9, 129
        memw
.Lsec_256_5a:
        addi.n   a9, a9, 1
.Lsec_256_5c:
        nop.n
        nop.n
        nop
        l32r     a10, .Lsec_256_0
        .reloc  ., R_XTENSA_ASM_EXPAND, wr_bt_tx_atten
        l32r     a8, .Lsec_256_c
        callx8   a8
        s8i      a2, a3, 0
        memw
        retw.n
        .size   btpwr_backoff, .-btpwr_backoff
.Lsec_256_74:

        .section .text.phy_dig_reg_backup,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_257_0, 0x3ff5cd0c
        .literal .Lsec_257_4, 0x3ff5cc0c
        .literal .Lsec_257_8, 0x3ff5cd08
        .literal .Lsec_257_c, 0x3ff5cd04
        .literal .Lsec_257_10, 0x3ff5c074
        .literal .Lsec_257_14, 0x3ff5c11c
        .literal .Lsec_257_18, 0x3ff45104
        .literal .Lsec_257_1c, 0x3ff45108
        .literal .Lsec_257_20, 0x3ff4510c
        .literal .Lsec_257_24, 0x3ff45110
        .literal .Lsec_257_28, 0x3ff5c860
        .literal .Lsec_257_2c, 0x3ff5c87c
        .literal .Lsec_257_30, 0x3ff5c02c
        .literal .Lsec_257_34, 0x3ff5c018
        .literal .Lsec_257_38, 0x3ff5c01c
        .literal .Lsec_257_3c, 0x3ff5c010
        .literal .Lsec_257_40, 0x3ff5c014
        .literal .Lsec_257_44, 0x3ff5c044
        .literal .Lsec_257_48, 0x3ff5c108
        .literal .Lsec_257_4c, 0x3ff5c0cc
        .literal .Lsec_257_50, 0x3ff5c124
        .literal .Lsec_257_54, esp_dport_access_reg_read
        .literal .Lsec_257_58, esp_dport_access_reg_read
        .literal .Lsec_257_5c, esp_dport_access_reg_read
        .literal .Lsec_257_60, esp_dport_access_reg_read
        .literal .Lsec_257_64, esp_dport_access_reg_read
        .literal .Lsec_257_68, esp_dport_access_reg_read
        .literal .Lsec_257_6c, esp_dport_access_reg_read
        .literal .Lsec_257_70, esp_dport_access_reg_read
        .literal .Lsec_257_74, esp_dport_access_reg_read
        .literal .Lsec_257_78, esp_dport_access_reg_read
        .literal .Lsec_257_7c, esp_dport_access_reg_read
        .literal .Lsec_257_80, esp_dport_access_reg_read
        .literal .Lsec_257_84, esp_dport_access_reg_read
        .literal .Lsec_257_88, esp_dport_access_reg_read
        .literal .Lsec_257_8c, esp_dport_access_reg_read
        .literal .Lsec_257_90, esp_dport_access_reg_read
        .literal .Lsec_257_94, esp_dport_access_reg_read
        .literal .Lsec_257_98, esp_dport_access_reg_read
        .literal .Lsec_257_9c, esp_dport_access_reg_read
        .literal .Lsec_257_a0, esp_dport_access_reg_read
        .literal .Lsec_257_a4, esp_dport_access_reg_read
.Lsec_257_a8:
        .global phy_dig_reg_backup
        .type   phy_dig_reg_backup, @function
phy_dig_reg_backup:
        entry    a1, 32
        extui    a2, a2, 0, 8
        beqz     a2, .Lsec_257_24e
        l32r     a10, .Lsec_257_0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_257_54
        callx8   a8
        s32i.n   a10, a3, 0
        nop.n
        nop.n
        nop.n
        nop
        l32r     a10, .Lsec_257_4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_257_58
        callx8   a8
        s32i.n   a10, a3, 4
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a10, .Lsec_257_8
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_257_5c
        callx8   a8
        s32i.n   a10, a3, 8
        nop.n
        nop.n
        nop.n
        nop
        l32r     a10, .Lsec_257_c
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_257_60
        callx8   a8
        s32i.n   a10, a3, 12
        nop.n
        nop.n
        nop.n
        nop
        l32r     a10, .Lsec_257_10
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_257_64
        callx8   a8
        s32i.n   a10, a3, 16
        nop.n
        nop.n
        nop.n
        nop
        l32r     a10, .Lsec_257_14
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_257_68
        callx8   a8
        s32i.n   a10, a3, 20
        nop.n
        nop.n
        nop.n
        nop
        l32r     a10, .Lsec_257_18
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_257_6c
        callx8   a8
        s32i.n   a10, a3, 24
        nop.n
        nop.n
        nop.n
        nop
        l32r     a10, .Lsec_257_1c
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_257_70
        callx8   a8
        s32i.n   a10, a3, 28
        nop.n
        nop.n
        nop.n
        nop
        l32r     a10, .Lsec_257_20
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_257_74
        callx8   a8
        s32i.n   a10, a3, 32
        nop.n
        nop.n
        nop.n
        nop
        l32r     a10, .Lsec_257_24
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_257_78
        callx8   a8
        s32i.n   a10, a3, 36
        nop.n
        nop.n
        nop.n
        nop
        l32r     a10, .Lsec_257_28
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_257_7c
        callx8   a8
        s32i.n   a10, a3, 40
        nop.n
        nop.n
        nop.n
        nop
        l32r     a10, .Lsec_257_2c
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_257_80
        callx8   a8
        s32i.n   a10, a3, 44
        nop.n
        nop.n
        nop.n
        nop
        l32r     a10, .Lsec_257_30
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_257_84
        callx8   a8
        s32i.n   a10, a3, 48
        nop.n
        nop.n
        nop.n
        nop
        l32r     a10, .Lsec_257_34
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_257_88
        callx8   a8
        s32i.n   a10, a3, 52
        nop.n
        nop.n
        nop.n
        nop
        l32r     a10, .Lsec_257_38
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_257_8c
        callx8   a8
        s32i.n   a10, a3, 56
        nop.n
        nop.n
        nop.n
        nop
        l32r     a10, .Lsec_257_3c
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_257_90
        callx8   a8
        s32i.n   a10, a3, 60
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a10, .Lsec_257_40
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_257_94
        callx8   a8
        s32i     a10, a3, 64
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a10, .Lsec_257_44
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_257_98
        callx8   a8
        s32i     a10, a3, 68
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a10, .Lsec_257_48
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_257_9c
        callx8   a8
        s32i     a10, a3, 72
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a10, .Lsec_257_4c
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_257_a0
        callx8   a8
        s32i     a10, a3, 76
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a10, .Lsec_257_50
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_257_a4
        callx8   a8
        s32i     a10, a3, 80
        j        .Lsec_257_3c5
.Lsec_257_24e:
        l32i.n   a8, a3, 0
        l32r     a2, .Lsec_257_0
        l32i.n   a9, a3, 4
        memw
        s32i.n   a8, a2, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a2, .Lsec_257_4
        l32i.n   a8, a3, 8
        memw
        s32i.n   a9, a2, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a2, .Lsec_257_8
        l32i.n   a9, a3, 12
        memw
        s32i.n   a8, a2, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a2, .Lsec_257_c
        l32i.n   a8, a3, 16
        memw
        s32i.n   a9, a2, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a2, .Lsec_257_10
        l32i.n   a9, a3, 20
        memw
        s32i.n   a8, a2, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a2, .Lsec_257_14
        l32i.n   a8, a3, 24
        memw
        s32i.n   a9, a2, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a2, .Lsec_257_18
        l32i.n   a9, a3, 28
        memw
        s32i.n   a8, a2, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a2, .Lsec_257_1c
        l32i.n   a8, a3, 32
        memw
        s32i.n   a9, a2, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a2, .Lsec_257_20
        l32i.n   a9, a3, 36
        memw
        s32i.n   a8, a2, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a2, .Lsec_257_24
        l32i.n   a8, a3, 40
        memw
        s32i.n   a9, a2, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a2, .Lsec_257_28
        memw
        s32i.n   a8, a2, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a8, a3, 44
        l32r     a2, .Lsec_257_2c
        l32i.n   a9, a3, 48
        memw
        s32i.n   a8, a2, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a2, .Lsec_257_30
        l32i.n   a8, a3, 52
        memw
        s32i.n   a9, a2, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a2, .Lsec_257_34
        l32i.n   a9, a3, 56
        memw
        s32i.n   a8, a2, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a2, .Lsec_257_38
        l32i.n   a8, a3, 60
        memw
        s32i.n   a9, a2, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a2, .Lsec_257_3c
        l32i     a9, a3, 64
        memw
        s32i.n   a8, a2, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a2, .Lsec_257_40
        l32i     a8, a3, 68
        memw
        s32i.n   a9, a2, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a2, .Lsec_257_44
        l32i     a9, a3, 72
        memw
        s32i.n   a8, a2, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a2, .Lsec_257_48
        l32i     a8, a3, 76
        memw
        s32i.n   a9, a2, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a2, .Lsec_257_4c
        l32i     a3, a3, 80
        memw
        s32i.n   a8, a2, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a2, .Lsec_257_50
        memw
        s32i.n   a3, a2, 0
.Lsec_257_3c5:
        movi.n   a2, 21
        retw.n
        .size   phy_dig_reg_backup, .-phy_dig_reg_backup
.Lsec_257_3c9:

        .section .text.get_iq_est_snr_1,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_258_0, 0x3ff46080
        .literal .Lsec_258_4, 0x3ff4608c
        .literal .Lsec_258_8, 0x0007fe03
        .literal .Lsec_258_c, 0x00000000
        .literal .Lsec_258_10, 0x3ff46084
        .literal .Lsec_258_14, 0x3ff46088
        .literal .Lsec_258_18, 0x3ff460cc
        .literal .Lsec_258_1c, 0x3ff460d8
        .literal .Lsec_258_20, 0x3ff460d0
        .literal .Lsec_258_24, 0x3ff460d4
        .literal .Lsec_258_28, 0x3ff460dc
        .literal .Lsec_258_2c, 0x3ff460e0
        .literal .Lsec_258_30, 0x3ff460e4
        .literal .Lsec_258_34, .Lsec_163_1dc
        .literal .Lsec_258_38, esp_dport_access_reg_read
        .literal .Lsec_258_3c, esp_dport_access_reg_read
        .literal .Lsec_258_40, __divdi3
        .literal .Lsec_258_44, esp_dport_access_reg_read
        .literal .Lsec_258_48, esp_dport_access_reg_read
        .literal .Lsec_258_4c, __divdi3
        .literal .Lsec_258_50, esp_dport_access_reg_read
        .literal .Lsec_258_54, esp_dport_access_reg_read
        .literal .Lsec_258_58, __divdi3
        .literal .Lsec_258_5c, esp_dport_access_reg_read
        .literal .Lsec_258_60, esp_dport_access_reg_read
        .literal .Lsec_258_64, __divdi3
        .literal .Lsec_258_68, esp_dport_access_reg_read
        .literal .Lsec_258_6c, esp_dport_access_reg_read
        .literal .Lsec_258_70, __divdi3
        .literal .Lsec_258_74, esp_dport_access_reg_read
        .literal .Lsec_258_78, esp_dport_access_reg_read
        .literal .Lsec_258_7c, __divdi3
        .literal .Lsec_258_80, esp_dport_access_reg_read
        .literal .Lsec_258_84, esp_dport_access_reg_read
        .literal .Lsec_258_88, __divdi3
        .literal .Lsec_258_8c, esp_dport_access_reg_read
        .literal .Lsec_258_90, esp_dport_access_reg_read
        .literal .Lsec_258_94, __divdi3
        .literal .Lsec_258_98, esp_dport_access_reg_read
        .literal .Lsec_258_9c, esp_dport_access_reg_read
        .literal .Lsec_258_a0, esp_dport_access_reg_read
        .literal .Lsec_258_a4, phy_printf
.Lsec_258_a8:
        .global get_iq_est_snr_1
        .type   get_iq_est_snr_1, @function
get_iq_est_snr_1:
        entry    a1, 96
        l32r     a10, .Lsec_258_0
        extui    a3, a3, 0, 8
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_258_38
        callx8   a8
        or       a5, a10, a10
        extui    a4, a4, 0, 8
        l32r     a10, .Lsec_258_4
        s32i     a3, a1, 20
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_258_3c
        callx8   a8
        add.n    a10, a5, a10
        srai     a3, a10, 31
        extui    a11, a10, 18, 14
        l32r     a12, .Lsec_258_8
        slli     a3, a3, 14
        or       a11, a11, a3
        slli     a10, a10, 14
        l32r     a13, .Lsec_258_c
        .reloc  ., R_XTENSA_ASM_EXPAND, __divdi3
        l32r     a8, .Lsec_258_40
        callx8   a8
        mov.n    a7, a10
        l32r     a10, .Lsec_258_10
        s32i.n   a11, a1, 44
        s32i     a4, a1, 16
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_258_44
        callx8   a8
        mov.n    a3, a10
        l32r     a10, .Lsec_258_14
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_258_48
        callx8   a8
        sub      a10, a3, a10
        srai     a3, a10, 31
        l32r     a12, .Lsec_258_8
        extui    a11, a10, 18, 14
        slli     a3, a3, 14
        or       a11, a11, a3
        slli     a10, a10, 14
        l32r     a13, .Lsec_258_c
        .reloc  ., R_XTENSA_ASM_EXPAND, __divdi3
        l32r     a8, .Lsec_258_4c
        callx8   a8
        mov.n    a5, a10
        or       a4, a11, a11
        l32r     a10, .Lsec_258_18
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_258_50
        callx8   a8
        mov.n    a3, a10
        l32r     a10, .Lsec_258_1c
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_258_54
        callx8   a8
        add.n    a10, a3, a10
        srai     a3, a10, 31
        l32r     a12, .Lsec_258_8
        extui    a11, a10, 18, 14
        slli     a3, a3, 14
        or       a11, a11, a3
        slli     a10, a10, 14
        l32r     a13, .Lsec_258_c
        .reloc  ., R_XTENSA_ASM_EXPAND, __divdi3
        l32r     a8, .Lsec_258_58
        callx8   a8
        mov.n    a6, a10
        mov.n    a3, a11
        l32r     a10, .Lsec_258_20
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_258_5c
        callx8   a8
        mov.n    a11, a10
        l32r     a10, .Lsec_258_24
        s32i     a11, a1, 36
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_258_60
        callx8   a8
        l32i     a11, a1, 36
        sub      a10, a11, a10
        srai     a11, a10, 31
        extui    a14, a10, 18, 14
        l32r     a12, .Lsec_258_8
        slli     a11, a11, 14
        slli     a10, a10, 14
        or       a11, a14, a11
        l32r     a13, .Lsec_258_c
        .reloc  ., R_XTENSA_ASM_EXPAND, __divdi3
        l32r     a8, .Lsec_258_64
        callx8   a8
        mull     a13, a7, a7
        mull     a12, a5, a5
        l32i.n   a9, a1, 44
        mull     a8, a4, a5
        mull     a9, a9, a7
        muluh    a5, a5, a5
        muluh    a7, a7, a7
        add.n    a12, a13, a12
        addx2    a5, a8, a5
        addx2    a7, a9, a7
        movi.n   a8, 1
        bltu     a12, a13, .Lsec_258_1b4
        movi.n   a8, 0
.Lsec_258_1b4:
        mull     a9, a6, a6
        mull     a3, a3, a6
        muluh    a6, a6, a6
        add.n    a7, a7, a5
        add.n    a9, a12, a9
        add.n    a7, a8, a7
        addx2    a6, a3, a6
        movi.n   a13, 1
        bltu     a9, a12, .Lsec_258_1cd
        movi.n   a13, 0
.Lsec_258_1cd:
        mull     a8, a10, a10
        mull     a11, a11, a10
        muluh    a10, a10, a10
        add.n    a7, a7, a6
        add.n    a8, a9, a8
        add.n    a7, a13, a7
        addx2    a10, a11, a10
        movi.n   a3, 1
        bltu     a8, a9, .Lsec_258_1e6
        movi.n   a3, 0
.Lsec_258_1e6:
        add.n    a10, a7, a10
        add.n    a3, a3, a10
        l32r     a10, .Lsec_258_0
        s32i.n   a8, a2, 32
        s32i.n   a3, a2, 36
        s32i     a8, a1, 48
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_258_68
        callx8   a8
        mov.n    a5, a10
        l32r     a10, .Lsec_258_4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_258_6c
        callx8   a8
        sub      a10, a5, a10
        srai     a5, a10, 31
        l32r     a12, .Lsec_258_8
        extui    a11, a10, 18, 14
        slli     a5, a5, 14
        or       a11, a11, a5
        slli     a10, a10, 14
        l32r     a13, .Lsec_258_c
        .reloc  ., R_XTENSA_ASM_EXPAND, __divdi3
        l32r     a8, .Lsec_258_70
        callx8   a8
        mov.n    a7, a10
        l32r     a10, .Lsec_258_10
        s32i     a11, a1, 40
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_258_74
        callx8   a8
        mov.n    a5, a10
        l32r     a10, .Lsec_258_14
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_258_78
        callx8   a8
        add.n    a10, a5, a10
        srai     a5, a10, 31
        l32r     a12, .Lsec_258_8
        extui    a11, a10, 18, 14
        slli     a5, a5, 14
        or       a11, a11, a5
        slli     a10, a10, 14
        l32r     a13, .Lsec_258_c
        .reloc  ., R_XTENSA_ASM_EXPAND, __divdi3
        l32r     a8, .Lsec_258_7c
        callx8   a8
        mov.n    a6, a10
        l32r     a10, .Lsec_258_18
        s32i.n   a11, a1, 36
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_258_80
        callx8   a8
        mov.n    a5, a10
        l32r     a10, .Lsec_258_1c
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_258_84
        callx8   a8
        sub      a10, a5, a10
        srai     a5, a10, 31
        l32r     a12, .Lsec_258_8
        extui    a11, a10, 18, 14
        slli     a5, a5, 14
        or       a11, a11, a5
        slli     a10, a10, 14
        l32r     a13, .Lsec_258_c
        .reloc  ., R_XTENSA_ASM_EXPAND, __divdi3
        l32r     a8, .Lsec_258_88
        callx8   a8
        mov.n    a5, a10
        l32r     a10, .Lsec_258_20
        s32i     a11, a1, 44
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_258_8c
        callx8   a8
        mov.n    a4, a10
        l32r     a10, .Lsec_258_24
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_258_90
        callx8   a8
        add.n    a10, a4, a10
        srai     a4, a10, 31
        extui    a11, a10, 18, 14
        slli     a4, a4, 14
        l32r     a12, .Lsec_258_8
        or       a11, a11, a4
        slli     a10, a10, 14
        mull     a4, a6, a6
        l32r     a13, .Lsec_258_c
        .reloc  ., R_XTENSA_ASM_EXPAND, __divdi3
        l32r     a8, .Lsec_258_94
        callx8   a8
        mull     a12, a7, a7
        movi.n   a13, 1
        l32i.n   a15, a1, 40
        add.n    a4, a12, a4
        mull     a15, a15, a7
        muluh    a7, a7, a7
        l32i.n   a14, a1, 36
        addx2    a7, a15, a7
        mull     a14, a14, a6
        muluh    a6, a6, a6
        l32i     a8, a1, 48
        addx2    a6, a14, a6
        l32i     a9, a1, 44
        bltu     a4, a12, .Lsec_258_2f0
        movi.n   a13, 0
.Lsec_258_2f0:
        mull     a12, a5, a5
        mull     a9, a9, a5
        muluh    a5, a5, a5
        add.n    a7, a7, a6
        add.n    a12, a4, a12
        add.n    a7, a13, a7
        addx2    a5, a9, a5
        movi.n   a6, 1
        bltu     a12, a4, .Lsec_258_309
        movi.n   a6, 0
.Lsec_258_309:
        mull     a4, a10, a10
        mull     a11, a11, a10
        muluh    a10, a10, a10
        add.n    a7, a7, a5
        add.n    a5, a12, a4
        add.n    a7, a6, a7
        addx2    a10, a11, a10
        movi.n   a4, 1
        bltu     a5, a12, .Lsec_258_322
        movi.n   a4, 0
.Lsec_258_322:
        add.n    a10, a7, a10
        add.n    a10, a4, a10
        s32i.n   a5, a2, 24
        add.n    a5, a8, a5
        movi     a4, 1
        s32i     a10, a2, 28
        bltu     a5, a8, .Lsec_258_336
        movi     a4, 0
.Lsec_258_336:
        add.n    a10, a3, a10
        add.n    a4, a4, a10
        nop.n
        l32r     a10, .Lsec_258_28
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_258_98
        callx8   a8
        srai     a6, a10, 4
        l32r     a10, .Lsec_258_2c
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_258_9c
        callx8   a8
        srai     a10, a10, 4
        mull     a8, a6, a6
        mull     a3, a10, a10
        mulsh    a6, a6, a6
        add.n    a3, a8, a3
        mulsh    a7, a10, a10
        movi.n   a9, 1
        bltu     a3, a8, .Lsec_258_369
        movi.n   a9, 0
.Lsec_258_369:
        add.n    a6, a6, a7
        l32r     a10, .Lsec_258_30
        add.n    a6, a9, a6
        s32i.n   a6, a1, 24
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_258_a0
        callx8   a8
        movi.n   a11, -1
        srai     a7, a10, 31
        srli     a12, a10, 1
        l32i.n   a6, a1, 20
        movi.n   a9, 32
        addi.n   a8, a6, 7
        extui    a8, a8, 0, 8
        xor      a6, a8, a11
        ssr      a6
        srl      a12, a12
        ssl      a8
        sll      a7, a7
        l32i.n   a15, a1, 16
        ssl      a8
        sll      a6, a10
        and      a13, a8, a9
        movi.n   a10, 0
        or       a7, a12, a7
        movnez   a7, a6, a13
        movnez   a6, a10, a13
        bne      a15, a10, .Lsec_258_3b4
        j        .Lsec_258_4be
.Lsec_258_3b4:
        l32i.n   a15, a1, 24
        addi.n   a8, a8, 11
        xor      a11, a8, a11
        slli     a13, a15, 1
        slli     a14, a4, 1
        slli     a12, a7, 1
        ssl      a11
        sll      a14, a14
        ssl      a11
        sll      a12, a12
        ssl      a11
        sll      a13, a13
        ssr      a8
        srl      a11, a3
        s32i.n   a11, a1, 52
        ssr      a8
        sra      a11, a10
        and      a9, a8, a9
        ssr      a8
        srl      a15, a4
        s32i.n   a11, a1, 20
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a11, a1, 24
        s32i.n   a14, a1, 16
        ssr      a8
        srl      a14, a5
        s32i.n   a12, a1, 28
        ssr      a8
        srl      a12, a6
        s32i.n   a13, a1, 32
        ssr      a8
        srl      a13, a7
        ssr      a8
        srl      a8, a11
        s32i.n   a8, a1, 56
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a8, a1, 16
        or       a14, a8, a14
        srai     a8, a10, 31
        movnez   a14, a15, a9
        l32i.n   a11, a1, 28
        movnez   a15, a10, a9
        or       a12, a11, a12
        movnez   a12, a13, a9
        movnez   a13, a10, a9
        l32i.n   a11, a1, 52
        s32i.n   a8, a1, 28
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a8, a1, 32
        or       a11, a8, a11
        l32i.n   a8, a1, 20
        moveqz   a8, a10, a9
        s32i.n   a11, a1, 52
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a11, a1, 52
        s32i.n   a8, a1, 16
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a8, a1, 56
        moveqz   a8, a11, a9
        s32i.n   a8, a1, 52
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a11, a1, 28
        l32i.n   a8, a1, 20
        movnez   a8, a11, a9
        s32i.n   a8, a1, 20
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a8, a1, 56
        movnez   a8, a10, a9
        l32i.n   a11, a1, 16
        s32i.n   a8, a1, 56
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a8, a1, 20
        s32i.n   a11, a1, 8
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a11, a1, 52
        s32i.n   a8, a1, 12
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a10, .Lsec_258_34
        l32i.n   a8, a1, 56
        s32i.n   a11, a1, 0
        s32i.n   a8, a1, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_258_a4
        callx8   a8
.Lsec_258_4be:
        l32i.n   a11, a1, 24
        s32i.n   a6, a2, 0
        s32i.n   a7, a2, 4
        s32i.n   a5, a2, 8
        s32i.n   a4, a2, 12
        s32i.n   a3, a2, 16
        s32i.n   a11, a2, 20
        retw.n
        .size   get_iq_est_snr_1, .-get_iq_est_snr_1
.Lsec_258_4ce:

        .section .text.freq_offset_get_pwr_1,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_259_0, 0x00000000
        .literal .Lsec_259_4, 0x00000000
        .literal .Lsec_259_8, g_phyFuns
        .literal .Lsec_259_c, 0x3ff450a8
        .literal .Lsec_259_10, 0x3ff460b8
        .literal .Lsec_259_14, 0x0fdbffff
        .literal .Lsec_259_18, 0x00240000
        .literal .Lsec_259_1c, 0xf0000000
        .literal .Lsec_259_20, .Lsec_163_220
        .literal .Lsec_259_24, 0xfffbffff
        .literal .Lsec_259_28, esp_dport_access_reg_read
        .literal .Lsec_259_2c, esp_dport_access_reg_read
        .literal .Lsec_259_30, get_iq_est_snr_1
        .literal .Lsec_259_34, phy_printf
        .literal .Lsec_259_38, esp_dport_access_reg_read
.Lsec_259_3c:
        .global freq_offset_get_pwr_1
        .type   freq_offset_get_pwr_1, @function
freq_offset_get_pwr_1:
        entry    a1, 144
        l32r     a6, .Lsec_259_8
        sext     a3, a3, 15
        extui    a2, a2, 0, 8
        movi.n   a10, 1
        extui    a4, a4, 0, 8
        s32i     a3, a1, 88
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a3, a6, 0
        s32i     a2, a1, 84
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a2, a3, 60
        movi     a3, -4
        s32i     a5, a1, 92
        extui    a5, a7, 0, 8
        s32i     a5, a1, 64
        s32i     a4, a1, 76
        callx8   a2
        l32r     a2, .Lsec_259_c
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_259_28
        callx8   a8
        and      a10, a10, a3
        l32i     a8, a1, 88
        extui    a3, a8, 0, 2
        or       a3, a10, a3
        l32r     a10, .Lsec_259_10
        memw
        s32i.n   a3, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_259_2c
        callx8   a8
        l32i     a3, a1, 88
        srai     a4, a3, 2
        movi.n   a3, 1
        l32r     a2, .Lsec_259_14
        and      a4, a4, a2
        l32r     a2, .Lsec_259_18
        or       a4, a4, a2
        l32i     a5, a1, 84
        ssl      a5
        sll      a5, a3
        l32r     a2, .Lsec_259_1c
        and      a10, a10, a2
        or       a10, a4, a10
        l32r     a8, .Lsec_259_10
        s32i     a5, a1, 72
        memw
        s32i.n   a10, a8, 0
        bge      a5, a3, .Lsec_259_d5
        j        .Lsec_259_1c9
.Lsec_259_d5:
        movi.n   a5, 0
        mov.n    a7, a5
        nop.n
        nop.n
        l32i     a4, a1, 76
        ssl      a4
        sll      a2, a3
        extui    a2, a2, 0, 16
        mov.n    a3, a5
        s32i     a2, a1, 80
        mov.n    a4, a5
        mov.n    a2, a5
        s32i     a5, a1, 68
.Lsec_259_f5:
        movi.n   a10, 1
        nop.n
        nop.n
        nop.n
        l32i.n   a8, a6, 0
        l32i     a11, a1, 80
        l32i     a8, a8, 0x144
        callx8   a8
        mov.n    a10, a1
        l32i     a12, a1, 64
        l32i     a11, a1, 76
        .reloc  ., R_XTENSA_ASM_EXPAND, get_iq_est_snr_1
        l32r     a8, .Lsec_259_30
        callx8   a8
        movi.n   a13, 1
        mov.n    a10, a13
        l32i.n   a9, a1, 8
        add.n    a9, a7, a9
        l32i.n   a8, a1, 0
        add.n    a8, a5, a8
        l32i.n   a11, a6, 0
        l32i     a12, a1, 12
        bltu     a9, a7, .Lsec_259_12c
        movi.n   a13, 0
.Lsec_259_12c:
        mov.n    a7, a9
        add.n    a4, a4, a12
        l32i.n   a9, a1, 4
        add.n    a4, a13, a4
        add      a3, a3, a9
        bltu     a8, a5, .Lsec_259_13c
        movi.n   a10, 0
.Lsec_259_13c:
        l32i     a9, a11, 0x148
        or       a5, a8, a8
        add      a3, a10, a3
        callx8   a9
        l32i     a8, a1, 64
        beqz.n   a8, .Lsec_259_15c
        l32i     a12, a1, 68
        l32i     a11, a1, 88
        l32r     a10, .Lsec_259_20
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_259_34
        callx8   a8
.Lsec_259_15c:
        addi.n   a2, a2, 1
        extui    a2, a2, 0, 8
        l32i     a8, a1, 72
        s32i     a2, a1, 68
        blt      a2, a8, .Lsec_259_f5
        nop.n
        nop.n
        nop.n
        nop.n
        l32i     a8, a1, 84
        movi.n   a2, -1
        xor      a2, a8, a2
        slli     a10, a3, 1
        slli     a11, a4, 1
        l32i     a8, a1, 84
        ssl      a2
        sll      a11, a11
        ssr      a8
        srl      a12, a7
        ssl      a2
        sll      a2, a10
        ssr      a8
        srl      a8, a5
        l32i     a5, a1, 84
        or       a8, a2, a8
        movi.n   a2, 32
        and      a2, a5, a2
        ssr      a5
        srl      a9, a3
        ssr      a5
        srl      a13, a4
        movi.n   a3, 0
        or       a12, a11, a12
        movnez   a12, a13, a2
        movnez   a8, a9, a2
        movnez   a13, a3, a2
        movnez   a9, a3, a2
        j        .Lsec_259_1d3
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_259_1c9:
        l32r     a8, .Lsec_259_0
        mov.n    a12, a8
        l32r     a9, .Lsec_259_4
        mov.n    a13, a9
.Lsec_259_1d3:
        l32i     a5, a1, 92
        l32r     a10, .Lsec_259_10
        s32i.n   a8, a5, 0
        s32i.n   a9, a5, 4
        s32i     a12, a1, 96
        s32i     a13, a1, 100
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_259_38
        callx8   a8
        l32r     a2, .Lsec_259_24
        and      a2, a10, a2
        movi.n   a10, 0
        l32r     a8, .Lsec_259_10
        memw
        s32i.n   a2, a8, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a2, a6, 0
        l32i.n   a2, a2, 60
        callx8   a2
        l32i     a12, a1, 96
        mov.n    a2, a12
        l32i     a13, a1, 100
        mov.n    a3, a13
        retw.n
        .size   freq_offset_get_pwr_1, .-freq_offset_get_pwr_1
.Lsec_259_214:

        .section .text.get_spur4m_pwr,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_260_0, 0x3ff5c02c
        .literal .Lsec_260_4, 0x00ffffff
        .literal .Lsec_260_8, 0x3c000000
        .literal .Lsec_260_c, 0x00800000
        .literal .Lsec_260_10, 0x32000000
        .literal .Lsec_260_14, 0xff7fffff
        .literal .Lsec_260_18, esp_dport_access_reg_read
        .literal .Lsec_260_1c, esp_dport_access_reg_read
        .literal .Lsec_260_20, freq_offset_get_pwr_1
        .literal .Lsec_260_24, freq_offset_get_pwr_1
        .literal .Lsec_260_28, esp_dport_access_reg_read
        .literal .Lsec_260_2c, esp_dport_access_reg_read
        .literal .Lsec_260_30, esp_dport_access_reg_read
.Lsec_260_34:
        .global get_spur4m_pwr
        .type   get_spur4m_pwr, @function
get_spur4m_pwr:
        entry    a1, 48
        l32r     a3, .Lsec_260_0
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_260_18
        callx8   a8
        l32r     a5, .Lsec_260_4
        and      a6, a10, a5
        mov.n    a10, a3
        l32r     a4, .Lsec_260_8
        or       a4, a6, a4
        memw
        s32i     a4, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_260_1c
        callx8   a8
        movi.n   a15, 0
        mov.n    a14, a15
        mov.n    a13, a1
        l32r     a4, .Lsec_260_c
        movi.n   a12, 10
        or       a6, a10, a4
        movi     a11, -103
        movi.n   a10, 5
        memw
        s32i.n   a6, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, freq_offset_get_pwr_1
        l32r     a8, .Lsec_260_20
        callx8   a8
        movi.n   a15, 0
        mov.n    a7, a10
        mov.n    a6, a11
        mov.n    a14, a15
        mov.n    a13, a1
        movi.n   a12, 10
        movi     a11, -102
        movi.n   a10, 5
        .reloc  ., R_XTENSA_ASM_EXPAND, freq_offset_get_pwr_1
        l32r     a8, .Lsec_260_24
        callx8   a8
        add.n    a10, a7, a10
        movi.n   a8, 1
        bltu     a10, a7, .Lsec_260_9a
        movi.n   a8, 0
.Lsec_260_9a:
        add.n    a11, a6, a11
        l32i.n   a9, a1, 4
        add.n    a11, a8, a11
        slli     a11, a11, 15
        slli     a6, a9, 15
        extui    a10, a10, 17, 15
        l32i.n   a8, a1, 0
        or       a10, a11, a10
        extui    a8, a8, 17, 15
        or       a8, a6, a8
        extui    a9, a9, 17, 15
        s32i.n   a8, a2, 4
        s32i.n   a10, a2, 0
        mov.n    a10, a3
        s32i     a8, a1, 0
        s32i.n   a9, a1, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_260_28
        callx8   a8
        and      a5, a10, a5
        or       a10, a3, a3
        l32r     a2, .Lsec_260_10
        or       a5, a5, a2
        memw
        s32i     a5, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_260_2c
        callx8   a8
        or       a4, a10, a4
        mov.n    a10, a3
        memw
        s32i.n   a4, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_260_30
        callx8   a8
        l32r     a2, .Lsec_260_14
        and      a10, a10, a2
        memw
        s32i.n   a10, a3, 0
        retw.n
        .size   get_spur4m_pwr, .-get_spur4m_pwr
.Lsec_260_fd:

        .section .text.rx_spur_cal,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_261_0, g_phyFuns
        .literal .Lsec_261_4, .Lsec_163_230
        .literal .Lsec_261_8, ets_delay_us
        .literal .Lsec_261_c, get_spur4m_pwr
        .literal .Lsec_261_10, phy_printf
.Lsec_261_14:
        .global rx_spur_cal
        .type   rx_spur_cal, @function
rx_spur_cal:
        entry    a1, 64
        extui    a3, a3, 0, 8
        movi.n   a8, 8
        extui    a2, a2, 0, 8
        s32i.n   a4, a1, 28
        movi.n   a4, 11
        movi.n   a5, 5
        movnez   a4, a8, a2
        mov.n    a8, a3
        l32r     a7, .Lsec_261_0
        mov.n    a3, a5
        s32i.n   a4, a1, 16
        mov.n    a4, a5
        mov.n    a5, a8
.Lsec_261_35:
        mov.n    a15, a3
        mov.n    a12, a2
        movi.n   a14, 2
        movi.n   a13, 5
        movi.n   a11, 3
        movi     a10, 102
        l32i.n   a9, a7, 0
        beqz.n   a2, .Lsec_261_58
        movi.n   a14, 0
        movi.n   a13, 2
        movi.n   a12, 5
        l32i     a9, a9, 168
        movi.n   a11, 4
        callx8   a9
        j        .Lsec_261_61
        .byte    0x00
.Lsec_261_58:
        l32i     a9, a9, 168
        movi     a10, 104
        callx8   a9
.Lsec_261_61:
        movi     a10, 100
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_261_8
        callx8   a8
        mov.n    a10, a1
        .reloc  ., R_XTENSA_ASM_EXPAND, get_spur4m_pwr
        l32r     a8, .Lsec_261_c
        callx8   a8
        bnei     a3, 5, .Lsec_261_80
        l32i.n   a8, a1, 4
        l32i.n   a6, a1, 0
        s32i.n   a3, a1, 20
        s32i.n   a8, a1, 24
        j        .Lsec_261_8d
.Lsec_261_80:
        l32i.n   a9, a1, 0
        bgeu     a9, a6, .Lsec_261_8d
        mov.n    a6, a9
        l32i.n   a8, a1, 4
        s32i.n   a4, a1, 20
        s32i.n   a8, a1, 24
.Lsec_261_8d:
        beqz.n   a5, .Lsec_261_a2
        mov.n    a11, a3
        nop.n
        nop.n
        nop.n
        l32r     a10, .Lsec_261_4
        l32i.n   a12, a1, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_261_10
        callx8   a8
.Lsec_261_a2:
        addi.n   a3, a3, 1
        extui    a4, a3, 0, 8
        l32i.n   a8, a1, 16
        bltu     a4, a8, .Lsec_261_35
        l32i.n   a4, a1, 28
        l32i.n   a8, a1, 24
        s32i.n   a6, a4, 0
        s32i.n   a8, a4, 4
        beqz.n   a2, .Lsec_261_d0
        movi.n   a14, 0
        movi.n   a13, 2
        movi.n   a12, 5
        nop.n
        l32i.n   a2, a7, 0
        movi.n   a11, 4
        movi     a10, 102
        l32i.n   a15, a1, 20
        l32i     a2, a2, 168
        callx8   a2
        j        .Lsec_261_e5
.Lsec_261_d0:
        l32i.n   a3, a7, 0
        mov.n    a12, a2
        movi.n   a14, 2
        movi.n   a13, 5
        movi.n   a11, 3
        l32i.n   a15, a1, 20
        movi     a10, 104
        l32i     a2, a3, 168
        callx8   a2
.Lsec_261_e5:
        l32i.n   a2, a1, 20
        retw.n
        .size   rx_spur_cal, .-rx_spur_cal
.Lsec_261_e9:

        .section .text.bt_opt_write_mem,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_262_0, 0x3ff4e0c4
        .literal .Lsec_262_4, 0x3ff4e0c0
        .literal .Lsec_262_8, 0x3ff4e148
        .literal .Lsec_262_c, esp_dport_access_reg_read
        .literal .Lsec_262_10, esp_dport_access_reg_read
        .literal .Lsec_262_14, esp_dport_access_reg_read
        .literal .Lsec_262_18, esp_dport_access_reg_read
.Lsec_262_1c:
        .global bt_opt_write_mem
        .type   bt_opt_write_mem, @function
bt_opt_write_mem:
        entry    a1, 32
        l32r     a5, .Lsec_262_0
        extui    a2, a2, 0, 8
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_262_c
        callx8   a8
        addx2    a2, a2, a2
        movi     a8, 0xffffff00
        addi.n   a2, a2, 2
        and      a10, a10, a8
        extui    a2, a2, 0, 8
        or       a2, a2, a10
        l32r     a10, .Lsec_262_4
        extui    a3, a3, 0, 8
        extui    a4, a4, 0, 8
        slli     a3, a3, 24
        slli     a4, a4, 16
        memw
        s32i.n   a2, a5, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_262_10
        callx8   a8
        extui    a10, a10, 0, 16
        or       a3, a3, a4
        or       a3, a3, a10
        l32r     a2, .Lsec_262_8
        mov.n    a10, a5
        memw
        s32i.n   a3, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_262_14
        callx8   a8
        movi     a2, 0x200
        or       a2, a10, a2
        mov.n    a10, a5
        memw
        s32i.n   a2, a5, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_262_18
        callx8   a8
        movi     a2, 0xfffffdff
        and      a10, a10, a2
        memw
        s32i.n   a10, a5, 0
        retw.n
        .size   bt_opt_write_mem, .-bt_opt_write_mem
.Lsec_262_91:

        .section .text.bt_rx_spur_opt,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_263_0, g_phyFuns
        .literal .Lsec_263_4, phy_dac_org
        .literal .Lsec_263_8, phy_dcur_org
        .literal .Lsec_263_c, chip7_phy_init_ctrl
        .literal .Lsec_263_10, 0x3ff4e0c4
        .literal .Lsec_263_14, 0xffff83ff
        .literal .Lsec_263_18, 0x00002c00
        .literal .Lsec_263_1c, set_chan_freq_sw_start
        .literal .Lsec_263_20, rx_spur_cal
        .literal .Lsec_263_24, bt_opt_write_mem
        .literal .Lsec_263_28, set_chan_freq_sw_start
        .literal .Lsec_263_2c, rx_spur_cal
        .literal .Lsec_263_30, rx_spur_cal
        .literal .Lsec_263_34, bt_opt_write_mem
        .literal .Lsec_263_38, esp_dport_access_reg_read
.Lsec_263_3c:
        .global bt_rx_spur_opt
        .type   bt_rx_spur_opt, @function
bt_rx_spur_opt:
        entry    a1, 48
        extui    a12, a2, 0, 8
        l32r     a2, .Lsec_263_0
        beqz     a12, .Lsec_263_141
        movi.n   a12, 0
        movi     a11, 3
        movi     a10, 104
        l32i.n   a3, a2, 0
        l32i     a3, a3, 152
        callx8   a3
        movi.n   a12, 5
        movi.n   a11, 4
        l32i     a3, a2, 0
        l32r     a4, .Lsec_263_4
        l32i     a3, a3, 152
        s8i      a10, a4, 0
        memw
        movi     a10, 102
        callx8   a3
        movi.n   a11, 0
        l32r     a5, .Lsec_263_c
        l32r     a3, .Lsec_263_8
        l8ui     a12, a5, 1
        s8i      a10, a3, 0
        memw
        movi.n   a10, 44
        .reloc  ., R_XTENSA_ASM_EXPAND, set_chan_freq_sw_start
        l32r     a8, .Lsec_263_1c
        callx8   a8
        movi.n   a11, 0
        mov.n    a12, a1
        mov.n    a10, a11
        .reloc  ., R_XTENSA_ASM_EXPAND, rx_spur_cal
        l32r     a8, .Lsec_263_20
        callx8   a8
        l32i.n   a8, a2, 0
        movi.n   a12, 0
        movi.n   a11, 3
        movi     a10, 104
        l32i     a8, a8, 152
        callx8   a8
        mov.n    a11, a10
        movi.n   a10, 42
        l8ui     a12, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, bt_opt_write_mem
        l32r     a8, .Lsec_263_24
        callx8   a8
        movi.n   a15, 0
        movi.n   a14, 6
        l32i.n   a8, a2, 0
        movi     a13, 7
        movi.n   a12, 5
        movi     a11, 4
        movi     a10, 102
        l32i     a8, a8, 168
        callx8   a8
        movi.n   a11, 0
        movi.n   a10, 84
        l8ui     a12, a5, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, set_chan_freq_sw_start
        l32r     a8, .Lsec_263_28
        callx8   a8
        movi.n   a11, 0
        mov.n    a12, a1
        mov.n    a10, a11
        .reloc  ., R_XTENSA_ASM_EXPAND, rx_spur_cal
        l32r     a8, .Lsec_263_2c
        callx8   a8
        l32i.n   a5, a2, 0
        movi.n   a11, 3
        movi     a12, 0
        movi     a10, 104
        l32i     a5, a5, 152
        callx8   a5
        mov.n    a5, a10
        mov.n    a12, a1
        movi     a11, 0
        movi     a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, rx_spur_cal
        l32r     a8, .Lsec_263_30
        callx8   a8
        l32i.n   a8, a2, 0
        movi.n   a12, 5
        movi     a11, 4
        movi     a10, 102
        l32i     a8, a8, 152
        callx8   a8
        mov.n    a12, a10
        mov.n    a11, a5
        movi.n   a10, 82
        .reloc  ., R_XTENSA_ASM_EXPAND, bt_opt_write_mem
        l32r     a8, .Lsec_263_34
        callx8   a8
        l32i.n   a5, a2, 0
        movi.n   a12, 0
        movi.n   a11, 3
        movi     a10, 104
        l32i     a5, a5, 160
        l8ui     a13, a4, 0
        callx8   a5
        movi.n   a12, 5
        movi.n   a11, 4
        l32i.n   a2, a2, 0
        movi     a10, 102
        l32i     a2, a2, 160
        j        .Lsec_263_163
        .byte    0x00
        .byte    0x00
.Lsec_263_141:
        l32i.n   a3, a2, 0
        movi.n   a11, 3
        movi     a10, 104
        l32r     a8, .Lsec_263_4
        l32i     a3, a3, 160
        l8ui     a13, a8, 0
        callx8   a3
        movi.n   a12, 5
        movi.n   a11, 4
        l32i.n   a2, a2, 0
        movi     a10, 102
        l32i     a2, a2, 160
        l32r     a3, .Lsec_263_8
.Lsec_263_163:
        l8ui     a13, a3, 0
        callx8   a2
        l32r     a3, .Lsec_263_10
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_263_38
        callx8   a8
        l32r     a2, .Lsec_263_14
        and      a10, a10, a2
        l32r     a2, .Lsec_263_18
        or       a10, a10, a2
        memw
        s32i.n   a10, a3, 0
        retw.n
        .size   bt_rx_spur_opt, .-bt_rx_spur_opt
.Lsec_263_187:

        .section .text.phy_init_param_set,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_264_0, init_wifi_disable
.Lsec_264_4:
        .global phy_init_param_set
        .type   phy_init_param_set, @function
phy_init_param_set:
        entry    a1, 32
        l32r     a8, .Lsec_264_0
        extui    a2, a2, 0, 1
        s8i      a2, a8, 0
        memw
        retw.n
        .size   phy_init_param_set, .-phy_init_param_set
.Lsec_264_15:

        .section .text.pbus_print,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_265_0, g_phyFuns
        .literal .Lsec_265_4, .Lsec_163_238
        .literal .Lsec_265_8, .Lsec_163_244
        .literal .Lsec_265_c, .Lsec_163_254
        .literal .Lsec_265_10, .Lsec_163_264
        .literal .Lsec_265_14, phy_printf
        .literal .Lsec_265_18, phy_printf
        .literal .Lsec_265_1c, phy_printf
        .literal .Lsec_265_20, phy_printf
.Lsec_265_24:
        .global pbus_print
        .type   pbus_print, @function
pbus_print:
        entry    a1, 32
        l32r     a2, .Lsec_265_0
        movi     a11, 1
        movi     a10, 0
        l32i     a3, a2, 0
        l32i     a3, a3, 188
        callx8   a3
        mov.n    a11, a10
        l32r     a10, .Lsec_265_4
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_265_14
        callx8   a8
        movi.n   a11, 1
        movi     a10, 4
        l32i     a3, a2, 0
        l32i     a3, a3, 188
        callx8   a3
        mov.n    a3, a10
        movi.n   a11, 1
        l32i.n   a4, a2, 0
        movi.n   a10, 5
        l32i     a4, a4, 188
        callx8   a4
        mov.n    a12, a10
        or       a11, a3, a3
        l32r     a10, .Lsec_265_8
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_265_18
        callx8   a8
        movi.n   a11, 1
        mov.n    a10, a11
        l32i.n   a3, a2, 0
        l32i     a3, a3, 188
        callx8   a3
        mov.n    a3, a10
        movi.n   a11, 2
        l32i.n   a4, a2, 0
        movi.n   a10, 1
        l32i     a4, a4, 188
        callx8   a4
        mov.n    a12, a10
        or       a11, a3, a3
        l32r     a10, .Lsec_265_c
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_265_1c
        callx8   a8
        movi.n   a11, 1
        movi.n   a10, 2
        l32i.n   a3, a2, 0
        l32i     a3, a3, 188
        callx8   a3
        mov.n    a3, a10
        movi.n   a11, 1
        l32i.n   a4, a2, 0
        movi.n   a10, 3
        l32i     a4, a4, 188
        callx8   a4
        movi.n   a11, 2
        mov.n    a4, a10
        l32i.n   a5, a2, 0
        mov.n    a10, a11
        l32i     a5, a5, 188
        callx8   a5
        mov.n    a5, a10
        movi.n   a11, 2
        l32i.n   a2, a2, 0
        movi.n   a10, 3
        l32i     a2, a2, 188
        callx8   a2
        mov.n    a14, a10
        mov.n    a13, a5
        l32r     a10, .Lsec_265_10
        mov.n    a12, a4
        mov.n    a11, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_265_20
        callx8   a8
        retw.n
        .size   pbus_print, .-pbus_print
.Lsec_265_df:

        .section .text.phy_reg_check,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_266_0, 0x3ff48000
        .literal .Lsec_266_4, .Lsec_163_278
        .literal .Lsec_266_8, 0x3ff48800
        .literal .Lsec_266_c, .Lsec_163_288
        .literal .Lsec_266_10, 0x3ff66000
        .literal .Lsec_266_14, .Lsec_163_298
        .literal .Lsec_266_18, 0x3ff00000
        .literal .Lsec_266_1c, .Lsec_163_2a8
        .literal .Lsec_266_20, 0x3ff46000
        .literal .Lsec_266_24, .Lsec_163_2bc
        .literal .Lsec_266_28, 0x3ff45000
        .literal .Lsec_266_2c, .Lsec_163_2cc
        .literal .Lsec_266_30, 0x3ff5c000
        .literal .Lsec_266_34, .Lsec_163_2dc
        .literal .Lsec_266_38, 0x3ff5d000
        .literal .Lsec_266_3c, .Lsec_163_2ec
        .literal .Lsec_266_40, 0x3ff5c400
        .literal .Lsec_266_44, .Lsec_163_2fc
        .literal .Lsec_266_48, 0x3ff5c800
        .literal .Lsec_266_4c, .Lsec_163_310
        .literal .Lsec_266_50, 0x3ff5cc00
        .literal .Lsec_266_54, .Lsec_163_320
        .literal .Lsec_266_58, 0x3ff51000
        .literal .Lsec_266_5c, .Lsec_163_330
        .literal .Lsec_266_60, 0x3ff4e000
        .literal .Lsec_266_64, .Lsec_163_344
        .literal .Lsec_266_68, esp_dport_access_reg_read
        .literal .Lsec_266_6c, phy_printf
        .literal .Lsec_266_70, esp_dport_access_reg_read
        .literal .Lsec_266_74, phy_printf
        .literal .Lsec_266_78, esp_dport_access_reg_read
        .literal .Lsec_266_7c, phy_printf
        .literal .Lsec_266_80, esp_dport_access_reg_read
        .literal .Lsec_266_84, phy_printf
        .literal .Lsec_266_88, esp_dport_access_reg_read
        .literal .Lsec_266_8c, phy_printf
        .literal .Lsec_266_90, esp_dport_access_reg_read
        .literal .Lsec_266_94, phy_printf
        .literal .Lsec_266_98, esp_dport_access_reg_read
        .literal .Lsec_266_9c, phy_printf
        .literal .Lsec_266_a0, esp_dport_access_reg_read
        .literal .Lsec_266_a4, phy_printf
        .literal .Lsec_266_a8, esp_dport_access_reg_read
        .literal .Lsec_266_ac, phy_printf
        .literal .Lsec_266_b0, esp_dport_access_reg_read
        .literal .Lsec_266_b4, phy_printf
        .literal .Lsec_266_b8, esp_dport_access_reg_read
        .literal .Lsec_266_bc, phy_printf
        .literal .Lsec_266_c0, esp_dport_access_reg_read
        .literal .Lsec_266_c4, phy_printf
        .literal .Lsec_266_c8, esp_dport_access_reg_read
        .literal .Lsec_266_cc, phy_printf
.Lsec_266_d0:
        .global phy_reg_check
        .type   phy_reg_check, @function
phy_reg_check:
        entry    a1, 32
        l32r     a5, .Lsec_266_0
        movi.n   a2, 0
        movi     a4, 216
        l32r     a3, .Lsec_266_4
.Lsec_266_de:
        add.n    a10, a2, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_266_68
        callx8   a8
        mov.n    a12, a10
        mov.n    a11, a2
        mov.n    a10, a3
        addi.n   a2, a2, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_266_6c
        callx8   a8
        bne      a2, a4, .Lsec_266_de
        l32r     a5, .Lsec_266_8
        movi.n   a2, 0
        movi     a4, 164
        l32r     a3, .Lsec_266_c
.Lsec_266_102:
        add.n    a10, a2, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_266_70
        callx8   a8
        mov.n    a12, a10
        mov.n    a11, a2
        mov.n    a10, a3
        addi     a2, a2, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_266_74
        callx8   a8
        bne      a2, a4, .Lsec_266_102
        l32r     a5, .Lsec_266_10
        movi.n   a2, 0
        movi.n   a4, 60
        l32r     a3, .Lsec_266_14
.Lsec_266_126:
        add.n    a10, a2, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_266_78
        callx8   a8
        mov.n    a12, a10
        mov.n    a11, a2
        mov.n    a10, a3
        addi.n   a2, a2, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_266_7c
        callx8   a8
        bne      a2, a4, .Lsec_266_126
        l32r     a5, .Lsec_266_18
        movi.n   a2, 0
        movi     a4, 0x5bc
        l32r     a3, .Lsec_266_1c
.Lsec_266_14a:
        add.n    a10, a2, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_266_80
        callx8   a8
        mov.n    a12, a10
        mov.n    a11, a2
        mov.n    a10, a3
        addi.n   a2, a2, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_266_84
        callx8   a8
        bne      a2, a4, .Lsec_266_14a
        l32r     a5, .Lsec_266_20
        movi.n   a2, 0
        movi     a4, 252
        l32r     a3, .Lsec_266_24
.Lsec_266_16e:
        add.n    a10, a2, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_266_88
        callx8   a8
        mov.n    a12, a10
        mov.n    a11, a2
        mov.n    a10, a3
        addi.n   a2, a2, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_266_8c
        callx8   a8
        bne      a2, a4, .Lsec_266_16e
        l32r     a5, .Lsec_266_28
        movi.n   a2, 0
        movi     a4, 0x11c
        l32r     a3, .Lsec_266_2c
.Lsec_266_192:
        add.n    a10, a2, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_266_90
        callx8   a8
        mov.n    a12, a10
        mov.n    a11, a2
        mov.n    a10, a3
        addi.n   a2, a2, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_266_94
        callx8   a8
        bne      a2, a4, .Lsec_266_192
        l32r     a5, .Lsec_266_30
        movi.n   a2, 0
        movi     a4, 0x128
        l32r     a3, .Lsec_266_34
.Lsec_266_1b6:
        add.n    a10, a2, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_266_98
        callx8   a8
        mov.n    a12, a10
        mov.n    a11, a2
        mov.n    a10, a3
        addi.n   a2, a2, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_266_9c
        callx8   a8
        bne      a2, a4, .Lsec_266_1b6
        l32r     a5, .Lsec_266_38
        movi.n   a2, 0
        movi     a4, 108
        l32r     a3, .Lsec_266_3c
.Lsec_266_1da:
        add.n    a10, a2, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_266_a0
        callx8   a8
        mov.n    a12, a10
        mov.n    a11, a2
        mov.n    a10, a3
        addi.n   a2, a2, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_266_a4
        callx8   a8
        bne      a2, a4, .Lsec_266_1da
        l32r     a5, .Lsec_266_40
        movi.n   a2, 0
        movi     a4, 100
        l32r     a3, .Lsec_266_44
.Lsec_266_1fe:
        add.n    a10, a2, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_266_a8
        callx8   a8
        mov.n    a12, a10
        mov.n    a11, a2
        mov.n    a10, a3
        addi.n   a2, a2, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_266_ac
        callx8   a8
        bne      a2, a4, .Lsec_266_1fe
        l32r     a5, .Lsec_266_48
        movi.n   a2, 0
        movi     a4, 156
        l32r     a3, .Lsec_266_4c
.Lsec_266_222:
        add.n    a10, a2, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_266_b0
        callx8   a8
        mov.n    a12, a10
        mov.n    a11, a2
        mov.n    a10, a3
        addi.n   a2, a2, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_266_b4
        callx8   a8
        bne      a2, a4, .Lsec_266_222
        l32r     a5, .Lsec_266_50
        movi.n   a2, 0
        movi     a4, 0x118
        l32r     a3, .Lsec_266_54
.Lsec_266_246:
        add.n    a10, a2, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_266_b8
        callx8   a8
        mov.n    a12, a10
        mov.n    a11, a2
        mov.n    a10, a3
        addi.n   a2, a2, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_266_bc
        callx8   a8
        bne      a2, a4, .Lsec_266_246
        l32r     a5, .Lsec_266_58
        movi.n   a2, 0
        movi     a4, 0x12c
        l32r     a3, .Lsec_266_5c
.Lsec_266_26a:
        add.n    a10, a2, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_266_c0
        callx8   a8
        mov.n    a12, a10
        mov.n    a11, a2
        mov.n    a10, a3
        addi.n   a2, a2, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_266_c4
        callx8   a8
        bne      a2, a4, .Lsec_266_26a
        l32r     a5, .Lsec_266_60
        movi.n   a2, 0
        movi     a4, 0x16c
        l32r     a3, .Lsec_266_64
.Lsec_266_28e:
        add.n    a10, a2, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_266_c8
        callx8   a8
        mov.n    a12, a10
        mov.n    a11, a2
        mov.n    a10, a3
        addi.n   a2, a2, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_266_cc
        callx8   a8
        bne      a2, a4, .Lsec_266_28e
        retw.n
        .size   phy_reg_check, .-phy_reg_check
.Lsec_266_2a9:

        .section .text.phy_i2c_check,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_267_0, g_phyFuns
        .literal .Lsec_267_4, .Lsec_163_358
        .literal .Lsec_267_8, .Lsec_163_370
        .literal .Lsec_267_c, .Lsec_163_388
        .literal .Lsec_267_10, .Lsec_163_3a0
        .literal .Lsec_267_14, .Lsec_163_3b8
        .literal .Lsec_267_18, .Lsec_163_3d0
        .literal .Lsec_267_1c, .Lsec_163_3e8
        .literal .Lsec_267_20, .Lsec_163_404
        .literal .Lsec_267_24, .Lsec_163_41c
        .literal .Lsec_267_28, .Lsec_163_434
        .literal .Lsec_267_2c, phy_printf
        .literal .Lsec_267_30, phy_printf
        .literal .Lsec_267_34, phy_printf
        .literal .Lsec_267_38, phy_printf
        .literal .Lsec_267_3c, phy_printf
        .literal .Lsec_267_40, phy_printf
        .literal .Lsec_267_44, phy_printf
        .literal .Lsec_267_48, phy_printf
        .literal .Lsec_267_4c, phy_printf
        .literal .Lsec_267_50, phy_printf
.Lsec_267_54:
        .global phy_i2c_check
        .type   phy_i2c_check, @function
phy_i2c_check:
        entry    a1, 32
        l32r     a2, .Lsec_267_0
        movi     a3, 0
        l32r     a4, .Lsec_267_4
.Lsec_267_60:
        mov.n    a12, a3
        movi.n   a11, 2
        movi     a10, 106
        l32i     a8, a2, 0
        l32i     a8, a8, 152
        callx8   a8
        mov.n    a12, a10
        or       a11, a3, a3
        or       a10, a4, a4
        addi     a3, a3, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_267_2c
        callx8   a8
        bnei     a3, 10, .Lsec_267_60
        l32r     a4, .Lsec_267_8
        movi.n   a3, 0
.Lsec_267_89:
        mov.n    a12, a3
        movi     a11, 1
        movi     a10, 103
        l32i     a8, a2, 0
        l32i     a8, a8, 152
        callx8   a8
        mov.n    a12, a10
        mov.n    a11, a3
        mov.n    a10, a4
        addi.n   a3, a3, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_267_30
        callx8   a8
        bnei     a3, 16, .Lsec_267_89
        l32r     a5, .Lsec_267_c
        movi.n   a3, 0
        movi.n   a4, 13
.Lsec_267_b2:
        mov.n    a12, a3
        movi.n   a11, 4
        l32i     a8, a2, 0
        movi     a10, 102
        l32i     a8, a8, 152
        callx8   a8
        mov.n    a12, a10
        mov.n    a11, a3
        mov.n    a10, a5
        addi     a3, a3, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_267_34
        callx8   a8
        bne      a3, a4, .Lsec_267_b2
        l32r     a4, .Lsec_267_10
        movi.n   a3, 1
.Lsec_267_d9:
        mov.n    a12, a3
        movi     a11, 2
        movi     a10, 107
        l32i     a8, a2, 0
        l32i     a8, a8, 152
        callx8   a8
        mov.n    a12, a10
        mov.n    a11, a3
        mov.n    a10, a4
        addi.n   a3, a3, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_267_38
        callx8   a8
        bnei     a3, 12, .Lsec_267_d9
        l32r     a5, .Lsec_267_14
        movi.n   a3, 0
        movi.n   a4, 11
.Lsec_267_102:
        mov.n    a12, a3
        movi.n   a11, 0
        l32i     a8, a2, 0
        movi     a10, 100
        l32i     a8, a8, 152
        callx8   a8
        mov.n    a12, a10
        mov.n    a11, a3
        mov.n    a10, a5
        addi     a3, a3, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_267_3c
        callx8   a8
        bne      a3, a4, .Lsec_267_102
        l32r     a4, .Lsec_267_18
        movi.n   a3, 0
.Lsec_267_129:
        mov.n    a12, a3
        movi     a11, 1
        movi     a10, 98
        l32i     a8, a2, 0
        l32i     a8, a8, 152
        callx8   a8
        mov.n    a12, a10
        mov.n    a11, a3
        mov.n    a10, a4
        addi     a3, a3, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_267_40
        callx8   a8
        bnei     a3, 12, .Lsec_267_129
        l32r     a5, .Lsec_267_1c
        movi.n   a3, 0
.Lsec_267_151:
        mov.n    a12, a3
        movi     a11, 0
        movi     a10, 99
        l32i     a4, a2, 0
        l32i     a4, a4, 152
        callx8   a4
        mov.n    a12, a10
        mov.n    a11, a3
        mov.n    a10, a5
        addi     a3, a3, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_267_44
        callx8   a8
        bnei     a3, 6, .Lsec_267_151
        l32r     a5, .Lsec_267_20
        movi.n   a3, 0
.Lsec_267_179:
        mov.n    a12, a3
        movi     a11, 4
        movi     a10, 101
        l32i     a4, a2, 0
        l32i     a4, a4, 152
        callx8   a4
        mov.n    a12, a10
        mov.n    a11, a3
        mov.n    a10, a5
        addi     a3, a3, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_267_48
        callx8   a8
        bnei     a3, 7, .Lsec_267_179
        l32r     a5, .Lsec_267_24
        movi.n   a3, 0
.Lsec_267_1a1:
        mov.n    a12, a3
        movi     a11, 3
        movi     a10, 104
        l32i     a4, a2, 0
        l32i     a4, a4, 152
        callx8   a4
        mov.n    a12, a10
        mov.n    a11, a3
        mov.n    a10, a5
        addi     a3, a3, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_267_4c
        callx8   a8
        bnei     a3, 3, .Lsec_267_1a1
        l32r     a4, .Lsec_267_28
        movi.n   a3, 0
.Lsec_267_1c9:
        mov.n    a12, a3
        movi     a11, 3
        movi     a10, 109
        l32i     a8, a2, 0
        l32i     a8, a8, 152
        callx8   a8
        mov.n    a12, a10
        mov.n    a11, a3
        mov.n    a10, a4
        addi.n   a3, a3, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_267_50
        callx8   a8
        bnei     a3, 10, .Lsec_267_1c9
        retw.n
        .size   phy_i2c_check, .-phy_i2c_check
.Lsec_267_1ed:

        .section .text.phy_cal_print,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_268_0, .Lsec_163_d4
        .literal .Lsec_268_4, .Lsec_163_e0
        .literal .Lsec_268_8, .Lsec_163_ec
        .literal .Lsec_268_c, 0x000012ff
        .literal .Lsec_268_10, .Lsec_163_100
        .literal .Lsec_268_14, 0x3ff5a008
        .literal .Lsec_268_18, 0x3ff5a004
        .literal .Lsec_268_1c, .Lsec_163_44c
        .literal .Lsec_268_20, chip7_sleep_params
        .literal .Lsec_268_24, .Lsec_163_464
        .literal .Lsec_268_28, .Lsec_163_478
        .literal .Lsec_268_2c, .Lsec_163_49c
        .literal .Lsec_268_30, tx_rf_ana_gain
        .literal .Lsec_268_34, .Lsec_163_12c
        .literal .Lsec_268_38, noise_array
        .literal .Lsec_268_3c, noise_array+0x6
        .literal .Lsec_268_40, .Lsec_163_4cc
        .literal .Lsec_268_44, .Lsec_163_4e0
        .literal .Lsec_268_48, chip7_sleep_params+0xc
        .literal .Lsec_268_4c, .Lsec_163_170
        .literal .Lsec_268_50, .Lsec_163_178
        .literal .Lsec_268_54, .Lsec_163_4f8
        .literal .Lsec_268_58, .Lsec_163_510
        .literal .Lsec_268_5c, .Lsec_163_15c
        .literal .Lsec_268_60, chip7_sleep_params+0x8
        .literal .Lsec_268_64, .Lsec_163_17c
        .literal .Lsec_268_68, chip7_sleep_params+0x60
        .literal .Lsec_268_6c, chip7_sleep_params+0x78
        .literal .Lsec_268_70, .Lsec_163_528
        .literal .Lsec_268_74, chip7_sleep_params+0x4
        .literal .Lsec_268_78, .Lsec_163_530
        .literal .Lsec_268_7c, .Lsec_163_53c
        .literal .Lsec_268_80, chip7_sleep_params+0x28
        .literal .Lsec_268_84, .Lsec_163_544
        .literal .Lsec_268_88, .Lsec_163_190
        .literal .Lsec_268_8c, .Lsec_163_1a0
        .literal .Lsec_268_90, .Lsec_163_1a8
        .literal .Lsec_268_94, .Lsec_163_1b8
        .literal .Lsec_268_98, .Lsec_163_558
        .literal .Lsec_268_9c, chip7_sleep_params+0x5
        .literal .Lsec_268_a0, .Lsec_163_56c
        .literal .Lsec_268_a4, chip7_sleep_params+0x6
        .literal .Lsec_268_a8, .Lsec_163_578
        .literal .Lsec_268_ac, chip7_sleep_params+0x18
        .literal .Lsec_268_b0, .Lsec_163_584
        .literal .Lsec_268_b4, .Lsec_163_58c
        .literal .Lsec_268_b8, phy_rxrf_dc
        .literal .Lsec_268_bc, phy_rxrf_dc+0x24
        .literal .Lsec_268_c0, .Lsec_163_59c
        .literal .Lsec_268_c4, phy_rxrf_dc+0x88
        .literal .Lsec_268_c8, .Lsec_163_5b0
        .literal .Lsec_268_cc, phy_rxbb_dc
        .literal .Lsec_268_d0, phy_rxbb_dc+0x10
        .literal .Lsec_268_d4, .Lsec_163_5c4
        .literal .Lsec_268_d8, phy_printf
        .literal .Lsec_268_dc, esp_dport_access_reg_read
        .literal .Lsec_268_e0, esp_dport_access_reg_read
        .literal .Lsec_268_e4, phy_printf
        .literal .Lsec_268_e8, phy_printf
        .literal .Lsec_268_ec, phy_printf
        .literal .Lsec_268_f0, phy_printf
        .literal .Lsec_268_f4, phy_printf
        .literal .Lsec_268_f8, phy_printf
        .literal .Lsec_268_fc, phy_printf
        .literal .Lsec_268_100, phy_printf
        .literal .Lsec_268_104, phy_printf
        .literal .Lsec_268_108, phy_printf
        .literal .Lsec_268_10c, phy_printf
        .literal .Lsec_268_110, phy_printf
        .literal .Lsec_268_114, phy_printf
        .literal .Lsec_268_118, phy_printf
        .literal .Lsec_268_11c, phy_printf
        .literal .Lsec_268_120, phy_printf
        .literal .Lsec_268_124, phy_printf
        .literal .Lsec_268_128, phy_printf
        .literal .Lsec_268_12c, phy_printf
        .literal .Lsec_268_130, phy_printf
        .literal .Lsec_268_134, phy_printf
        .literal .Lsec_268_138, phy_printf
        .literal .Lsec_268_13c, phy_printf
        .literal .Lsec_268_140, phy_printf
        .literal .Lsec_268_144, phy_printf
        .literal .Lsec_268_148, phy_printf
        .literal .Lsec_268_14c, phy_printf
        .literal .Lsec_268_150, phy_printf
        .literal .Lsec_268_154, phy_printf
        .literal .Lsec_268_158, phy_printf
        .literal .Lsec_268_15c, phy_printf
        .literal .Lsec_268_160, phy_printf
        .literal .Lsec_268_164, phy_printf
        .literal .Lsec_268_168, phy_printf
        .literal .Lsec_268_16c, phy_printf
        .literal .Lsec_268_170, phy_printf
        .literal .Lsec_268_174, phy_printf
        .literal .Lsec_268_178, phy_printf
        .literal .Lsec_268_17c, phy_printf
        .literal .Lsec_268_180, phy_printf
        .literal .Lsec_268_184, phy_printf
        .literal .Lsec_268_188, phy_printf
        .literal .Lsec_268_18c, phy_printf
        .literal .Lsec_268_190, phy_printf
        .literal .Lsec_268_194, phy_printf
        .literal .Lsec_268_198, phy_printf
        .literal .Lsec_268_19c, phy_printf
        .literal .Lsec_268_1a0, phy_printf
        .literal .Lsec_268_1a4, phy_printf
        .literal .Lsec_268_1a8, phy_printf
        .literal .Lsec_268_1ac, phy_printf
        .literal .Lsec_268_1b0, phy_printf
        .literal .Lsec_268_1b4, phy_printf
        .literal .Lsec_268_1b8, phy_printf
        .literal .Lsec_268_1bc, phy_printf
        .literal .Lsec_268_1c0, phy_printf
.Lsec_268_1c4:
        .global phy_cal_print
        .type   phy_cal_print, @function
phy_cal_print:
        entry    a1, 64
        l32r     a14, .Lsec_268_0
        l32r     a13, .Lsec_268_4
        l32r     a12, .Lsec_268_8
        l32r     a11, .Lsec_268_c
        l32r     a10, .Lsec_268_10
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_d8
        callx8   a8
        l32r     a10, .Lsec_268_14
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_268_dc
        callx8   a8
        mov.n    a2, a10
        l32r     a10, .Lsec_268_18
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_268_e0
        callx8   a8
        mov.n    a12, a10
        mov.n    a11, a2
        l32r     a10, .Lsec_268_1c
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_e4
        callx8   a8
        l32r     a2, .Lsec_268_20
        mov.n    a3, a2
        l32i.n   a11, a2, 0
        l32r     a10, .Lsec_268_24
        l32i     a2, a2, 192
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_e8
        callx8   a8
        extui    a4, a2, 16, 16
        srli     a13, a2, 8
        mov.n    a11, a4
        l32r     a10, .Lsec_268_28
        extui    a12, a2, 0, 8
        sext     a13, a13, 7
        s32i.n   a4, a1, 16
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_ec
        callx8   a8
        l32r     a8, .Lsec_268_20
        l8ui     a15, a3, 87
        l8ui     a14, a3, 86
        l8ui     a13, a3, 85
        l8ui     a12, a3, 84
        l8ui     a11, a3, 80
        l8ui     a2, a8, 88
        l8ui     a3, a8, 89
        l32r     a10, .Lsec_268_2c
        s32i     a3, a1, 4
        s32i     a2, a1, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_f0
        callx8   a8
        l32r     a2, .Lsec_268_30
        l32r     a10, .Lsec_268_34
        l16ui    a12, a2, 2
        l16ui    a11, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_f4
        callx8   a8
        l32r     a2, .Lsec_268_38
        l32r     a4, .Lsec_268_3c
        l32r     a3, .Lsec_268_40
.Lsec_268_26b:
        mov.n    a10, a3
        l16si    a11, a2, 0
        addi     a2, a2, 2
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_f8
        callx8   a8
        bne      a4, a2, .Lsec_268_26b
        l32r     a10, .Lsec_268_44
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_fc
        callx8   a8
        l32r     a4, .Lsec_268_20
        mov.n    a5, a4
        l32r     a3, .Lsec_268_48
        l32r     a2, .Lsec_268_4c
.Lsec_268_290:
        addi.n   a5, a5, 3
        mov.n    a10, a2
        l8ui     a11, a5, 61
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_100
        callx8   a8
        bne      a5, a3, .Lsec_268_290
        mov.n    a5, a4
        l32r     a6, .Lsec_268_50
        mov.n    a10, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_104
        callx8   a8
        l32r     a10, .Lsec_268_54
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_108
        callx8   a8
.Lsec_268_2b6:
        addi.n   a5, a5, 3
        or       a10, a2, a2
        l8ui     a11, a5, 62
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_10c
        callx8   a8
        bne      a3, a5, .Lsec_268_2b6
        or       a10, a6, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_110
        callx8   a8
        l32r     a10, .Lsec_268_58
        or       a5, a4, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_114
        callx8   a8
.Lsec_268_2dc:
        addi.n   a5, a5, 3
        mov.n    a10, a2
        l8ui     a11, a5, 63
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_118
        callx8   a8
        bne      a3, a5, .Lsec_268_2dc
        or       a10, a6, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_11c
        callx8   a8
        l32r     a10, .Lsec_268_5c
        or       a3, a4, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_120
        callx8   a8
        l32r     a5, .Lsec_268_60
.Lsec_268_304:
        addi.n   a3, a3, 2
        mov.n    a10, a2
        l16si    a11, a3, 2
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_124
        callx8   a8
        bne      a5, a3, .Lsec_268_304
        mov.n    a10, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_128
        callx8   a8
        l32r     a10, .Lsec_268_64
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_12c
        callx8   a8
        l32r     a7, .Lsec_268_68
        l32r     a3, .Lsec_268_6c
.Lsec_268_32b:
        mov.n    a10, a2
        l8ui     a11, a7, 0
        addi     a7, a7, 1
        sext     a11, a11, 7
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_130
        callx8   a8
        bne      a3, a7, .Lsec_268_32b
        or       a10, a6, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_134
        callx8   a8
        l32r     a10, .Lsec_268_70
        or       a7, a4, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_138
        callx8   a8
        l32r     a3, .Lsec_268_78
.Lsec_268_357:
        movi.n   a13, 15
        mov.n    a10, a3
        addi.n   a7, a7, 2
        l16ui    a8, a7, 50
        extui    a9, a8, 6, 5
        sext     a11, a9, 7
        extui    a8, a8, 0, 6
        sext     a12, a8, 7
        bge      a13, a11, .Lsec_268_372
        addi     a11, a9, -32
.Lsec_268_372:
        movi.n   a9, 31
        bge      a9, a12, .Lsec_268_37a
        addi     a12, a8, -64
.Lsec_268_37a:
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_13c
        callx8   a8
        l32r     a13, .Lsec_268_74
        bne      a7, a13, .Lsec_268_357
        mov.n    a10, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_140
        callx8   a8
        l32r     a10, .Lsec_268_7c
        or       a7, a4, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_144
        callx8   a8
.Lsec_268_39a:
        addi.n   a7, a7, 8
        l32r     a10, .Lsec_268_84
        l16ui    a14, a7, 10
        l16ui    a13, a7, 8
        l16ui    a12, a7, 6
        l16ui    a11, a7, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_148
        callx8   a8
        l32r     a8, .Lsec_268_80
        bne      a8, a7, .Lsec_268_39a
        or       a10, a6, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_14c
        callx8   a8
        l32r     a10, .Lsec_268_88
        or       a7, a4, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_150
        callx8   a8
.Lsec_268_3cc:
        addi.n   a7, a7, 1
        l32r     a10, .Lsec_268_8c
        l8ui     a11, a7, 119
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_154
        callx8   a8
        bne      a5, a7, .Lsec_268_3cc
        mov.n    a10, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_158
        callx8   a8
        l32r     a10, .Lsec_268_90
        or       a7, a4, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_15c
        callx8   a8
.Lsec_268_3f1:
        addi.n   a7, a7, 1
        mov.n    a10, a2
        l8ui     a11, a7, 128
        sext     a11, a11, 7
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_160
        callx8   a8
        bne      a5, a7, .Lsec_268_3f1
        mov.n    a10, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_164
        callx8   a8
        l32r     a9, .Lsec_268_20
        mov.n    a7, a4
        l16ui    a11, a9, 144
        l32r     a10, .Lsec_268_94
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_168
        callx8   a8
        l32r     a10, .Lsec_268_98
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_16c
        callx8   a8
.Lsec_268_426:
        addi.n   a7, a7, 1
        mov.n    a10, a2
        l8ui     a11, a7, 138
        sext     a11, a11, 7
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_170
        callx8   a8
        l32r     a13, .Lsec_268_9c
        bne      a13, a7, .Lsec_268_426
        or       a10, a6, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_174
        callx8   a8
        l32r     a10, .Lsec_268_a0
        mov.n    a2, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_178
        callx8   a8
        movi.n   a7, 31
.Lsec_268_452:
        movi.n   a13, 15
        l16ui    a8, a2, 146
        mov.n    a10, a3
        extui    a9, a8, 6, 5
        sext     a11, a9, 7
        extui    a8, a8, 0, 6
        sext     a12, a8, 7
        addi.n   a2, a2, 2
        bge      a13, a11, .Lsec_268_46d
        addi     a11, a9, -32
.Lsec_268_46d:
        bge      a7, a12, .Lsec_268_473
        addi     a12, a8, -64
.Lsec_268_473:
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_17c
        callx8   a8
        l32r     a8, .Lsec_268_a4
        bne      a2, a8, .Lsec_268_452
        or       a10, a6, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_180
        callx8   a8
        l32r     a10, .Lsec_268_a8
        or       a7, a4, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_184
        callx8   a8
        l32r     a2, .Lsec_268_ac
.Lsec_268_497:
        addi.n   a7, a7, 8
        l32r     a10, .Lsec_268_84
        l16ui    a14, a7, 150
        l16ui    a13, a7, 148
        l16ui    a12, a7, 146
        l16ui    a11, a7, 144
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_188
        callx8   a8
        bne      a2, a7, .Lsec_268_497
        mov.n    a10, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_18c
        callx8   a8
        l32r     a10, .Lsec_268_b0
        movi.n   a7, 15
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_190
        callx8   a8
        movi.n   a2, 31
.Lsec_268_4c6:
        mov.n    a10, a3
        l16ui    a8, a4, 56
        addi     a4, a4, 2
        extui    a9, a8, 6, 5
        sext     a11, a9, 7
        extui    a8, a8, 0, 6
        sext     a12, a8, 7
        bge      a7, a11, .Lsec_268_4e0
        addi     a11, a9, -32
.Lsec_268_4e0:
        bge      a2, a12, .Lsec_268_4e6
        addi     a12, a8, -64
.Lsec_268_4e6:
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_194
        callx8   a8
        bne      a5, a4, .Lsec_268_4c6
        or       a10, a6, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_198
        callx8   a8
        l32r     a10, .Lsec_268_b4
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_19c
        callx8   a8
        l32r     a4, .Lsec_268_b8
        l32r     a2, .Lsec_268_bc
.Lsec_268_507:
        mov.n    a10, a3
        l32i.n   a11, a4, 0
        addi.n   a4, a4, 4
        sext     a12, a11, 15
        srai     a11, a11, 16
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_1a0
        callx8   a8
        bne      a2, a4, .Lsec_268_507
        mov.n    a10, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_1a4
        callx8   a8
        l32r     a10, .Lsec_268_c0
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_1a8
        callx8   a8
        l32r     a4, .Lsec_268_c4
.Lsec_268_530:
        movi     a12, 0x100
        mov.n    a11, a12
        l32i.n   a5, a2, 0
        beqz.n   a5, .Lsec_268_542
        srai     a11, a5, 16
        sext     a12, a5, 15
        j        .Lsec_268_542
.Lsec_268_542:
        addi.n   a2, a2, 4
        or       a10, a3, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_1ac
        callx8   a8
        bne      a4, a2, .Lsec_268_530
        mov.n    a10, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_1b0
        callx8   a8
        l32r     a10, .Lsec_268_c8
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_1b4
        callx8   a8
        l32r     a2, .Lsec_268_cc
        l32r     a4, .Lsec_268_d0
.Lsec_268_567:
        mov.n    a10, a3
        l32i.n   a11, a2, 0
        addi.n   a2, a2, 4
        sext     a12, a11, 15
        srai     a11, a11, 16
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_1b8
        callx8   a8
        bne      a4, a2, .Lsec_268_567
        or       a10, a6, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_1bc
        callx8   a8
        l32i.n   a11, a1, 16
        l32r     a10, .Lsec_268_d4
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_268_1c0
        callx8   a8
        retw.n
        .size   phy_cal_print, .-phy_cal_print
.Lsec_268_592:

        .section .text.phy_wifi_bb_reset,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_269_0, 0x3ff00024
        .literal .Lsec_269_4, esp_dport_access_reg_read
        .literal .Lsec_269_8, ets_delay_us
        .literal .Lsec_269_c, esp_dport_access_reg_read
.Lsec_269_10:
        .global phy_wifi_bb_reset
        .type   phy_wifi_bb_reset, @function
phy_wifi_bb_reset:
        entry    a1, 32
        l32r     a2, .Lsec_269_0
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_269_4
        callx8   a8
        movi.n   a8, 1
        or       a10, a10, a8
        memw
        s32i.n   a10, a2, 0
        mov.n    a10, a8
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_269_8
        callx8   a8
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_269_c
        callx8   a8
        movi.n   a8, -2
        and      a10, a10, a8
        memw
        s32i.n   a10, a2, 0
        retw.n
        .size   phy_wifi_bb_reset, .-phy_wifi_bb_reset
.Lsec_269_44:

        .section .rodata.CSWTCH$369,"a",@progbits
        .align  4
.Lsec_270_0:
        .type   CSWTCH$369, @object
        .size   CSWTCH$369, 11
CSWTCH$369:
        .byte   0xf2, 0xf2, 0xf2, 0xf2, 0xf2, 0xf2, 0xf3, 0xf2, 0xf2, 0xf0, 0xf1

        .section .rodata.CSWTCH$368,"a",@progbits
        .align  4
.Lsec_271_0:
        .type   CSWTCH$368, @object
        .size   CSWTCH$368, 22
CSWTCH$368:
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x00, 0x03, 0x00, 0x03, 0x00, 0x07, 0x00
        .byte   0x07, 0x00, 0x0f, 0x00, 0x0f, 0x00

        .section .rodata.CSWTCH$367,"a",@progbits
        .align  4
.Lsec_272_0:
        .type   CSWTCH$367, @object
        .size   CSWTCH$367, 22
CSWTCH$367:
        .byte   0x03, 0x00, 0x06, 0x00, 0x09, 0x00, 0x0c, 0x00, 0x04, 0x00, 0x07, 0x00, 0x0a, 0x00, 0x07, 0x00
        .byte   0x0a, 0x00, 0x07, 0x00, 0x0a, 0x00

        .section .rodata.CSWTCH$356,"a",@progbits
        .align  4
.Lsec_273_0:
        .type   CSWTCH$356, @object
        .size   CSWTCH$356, 6
CSWTCH$356:
        .byte   0x00, 0x00, 0x20, 0x00, 0x30, 0x00
.Lsec_273_6:

        .section .bss.g_phyFuns,"aw",@nobits
        .align  4
.Lsec_274_0:
        .global g_phyFuns
        .type   g_phyFuns, @object
        .size   g_phyFuns, 4
g_phyFuns:
        .skip   4

        .section .bss.bt_dig_gain_force,"aw",@nobits
        .align  1
.Lsec_275_0:
        .global bt_dig_gain_force
        .type   bt_dig_gain_force, @object
        .size   bt_dig_gain_force, 1
bt_dig_gain_force:
        .skip   1

        .section .bss.init_wifi_disable,"aw",@nobits
        .align  1
.Lsec_276_0:
        .global init_wifi_disable
        .type   init_wifi_disable, @object
        .size   init_wifi_disable, 1
init_wifi_disable:
        .skip   1

        .section .bss.rfrx_dcap_enable,"aw",@nobits
        .align  1
.Lsec_277_0:
        .global rfrx_dcap_enable
        .type   rfrx_dcap_enable, @object
        .size   rfrx_dcap_enable, 1
rfrx_dcap_enable:
        .skip   1

        .section .bss.rfrx_vga_lna_dcap,"aw",@nobits
        .align  4
.Lsec_278_0:
        .global rfrx_vga_lna_dcap
        .type   rfrx_vga_lna_dcap, @object
        .size   rfrx_vga_lna_dcap, 2
rfrx_vga_lna_dcap:
        .skip   2

        .section .bss.corr_cca_thr,"aw",@nobits
        .align  1
.Lsec_279_0:
        .global corr_cca_thr
        .type   corr_cca_thr, @object
        .size   corr_cca_thr, 1
corr_cca_thr:
        .skip   1

        .section .bss.bt_txpwr_backoff,"aw",@nobits
        .align  1
.Lsec_280_0:
        .global bt_txpwr_backoff
        .type   bt_txpwr_backoff, @object
        .size   bt_txpwr_backoff, 1
bt_txpwr_backoff:
        .skip   1

        .section .bss.lr_enable,"aw",@nobits
        .align  1
.Lsec_281_0:
        .global lr_enable
        .type   lr_enable, @object
        .size   lr_enable, 1
lr_enable:
        .skip   1

        .section .bss.chan14_mic_en,"aw",@nobits
        .align  1
.Lsec_282_0:
        .global chan14_mic_en
        .type   chan14_mic_en, @object
        .size   chan14_mic_en, 1
chan14_mic_en:
        .skip   1

        .section .bss.re_entry,"aw",@nobits
        .align  1
.Lsec_283_0:
        .global re_entry
        .type   re_entry, @object
        .size   re_entry, 1
re_entry:
        .skip   1

        .section .bss.force_hw_set_freq_flag,"aw",@nobits
        .align  1
.Lsec_284_0:
        .global force_hw_set_freq_flag
        .type   force_hw_set_freq_flag, @object
        .size   force_hw_set_freq_flag, 1
force_hw_set_freq_flag:
        .skip   1

        .section .bss.phy_init_flag,"aw",@nobits
        .align  1
.Lsec_285_0:
        .global phy_init_flag
        .type   phy_init_flag, @object
        .size   phy_init_flag, 1
phy_init_flag:
        .skip   1

        .section .bss.phy_set_most_tpw_index,"aw",@nobits
        .align  1
.Lsec_286_0:
        .global phy_set_most_tpw_index
        .type   phy_set_most_tpw_index, @object
        .size   phy_set_most_tpw_index, 1
phy_set_most_tpw_index:
        .skip   1

        .section .bss.target_power_backoff_qdb,"aw",@nobits
        .align  2
.Lsec_287_0:
        .global target_power_backoff_qdb
        .type   target_power_backoff_qdb, @object
        .size   target_power_backoff_qdb, 2
target_power_backoff_qdb:
        .skip   2

        .section .bss.phy_set_most_tpw_flag,"aw",@nobits
        .align  1
.Lsec_288_0:
        .global phy_set_most_tpw_flag
        .type   phy_set_most_tpw_flag, @object
        .size   phy_set_most_tpw_flag, 1
phy_set_most_tpw_flag:
        .skip   1

        .section .bss.phy_set_most_tpw_disbg,"aw",@nobits
        .align  1
.Lsec_289_0:
        .global phy_set_most_tpw_disbg
        .type   phy_set_most_tpw_disbg, @object
        .size   phy_set_most_tpw_disbg, 1
phy_set_most_tpw_disbg:
        .skip   1

        .section .bss.chan14_mic_flag,"aw",@nobits
        .align  1
.Lsec_290_0:
        .global chan14_mic_flag
        .type   chan14_mic_flag, @object
        .size   chan14_mic_flag, 1
chan14_mic_flag:
        .skip   1

        .section .data.chan14_mic_most_power,"aw",@progbits
        .align  1
.Lsec_291_0:
        .global chan14_mic_most_power
        .type   chan14_mic_most_power, @object
        .size   chan14_mic_most_power, 1
chan14_mic_most_power:
        .byte   0x30

        .section .data.phy_in_most_power_bk,"aw",@progbits
        .align  1
.Lsec_292_0:
        .global phy_in_most_power_bk
        .type   phy_in_most_power_bk, @object
        .size   phy_in_most_power_bk, 1
phy_in_most_power_bk:
        .byte   0x4e

        .section .data.phy_in_most_power,"aw",@progbits
        .align  1
.Lsec_293_0:
        .global phy_in_most_power
        .type   phy_in_most_power, @object
        .size   phy_in_most_power, 1
phy_in_most_power:
        .byte   0x4e

        .section .bss.phy_meas_noise_floor,"aw",@nobits
        .align  2
.Lsec_294_0:
        .global phy_meas_noise_floor
        .type   phy_meas_noise_floor, @object
        .size   phy_meas_noise_floor, 2
phy_meas_noise_floor:
        .skip   2

        .section .bss.sw_scan_mode,"aw",@nobits
        .align  1
.Lsec_295_0:
        .global sw_scan_mode
        .type   sw_scan_mode, @object
        .size   sw_scan_mode, 1
sw_scan_mode:
        .skip   1

        .section .bss.bt_mode_wifibb,"aw",@nobits
        .align  1
.Lsec_296_0:
        .global bt_mode_wifibb
        .type   bt_mode_wifibb, @object
        .size   bt_mode_wifibb, 1
bt_mode_wifibb:
        .skip   1

        .section .data.bt_tx_pa_gain,"aw",@progbits
        .align  2
.Lsec_297_0:
        .global bt_tx_pa_gain
        .type   bt_tx_pa_gain, @object
        .size   bt_tx_pa_gain, 2
bt_tx_pa_gain:
        .byte   0x7a, 0x00

        .section .data.tx_rf_ana_gain,"aw",@progbits
        .align  4
.Lsec_298_0:
        .global tx_rf_ana_gain
        .type   tx_rf_ana_gain, @object
        .size   tx_rf_ana_gain, 4
tx_rf_ana_gain:
        .byte   0x5f, 0x00, 0x20, 0x01

        .section .bss.rxmax_ext_level,"aw",@nobits
        .align  1
.Lsec_299_0:
        .global rxmax_ext_level
        .type   rxmax_ext_level, @object
        .size   rxmax_ext_level, 1
rxmax_ext_level:
        .skip   1

        .section .bss.noise_timeout_flag,"aw",@nobits
        .align  1
.Lsec_300_0:
        .global noise_timeout_flag
        .type   noise_timeout_flag, @object
        .size   noise_timeout_flag, 1
noise_timeout_flag:
        .skip   1

        .section .data.noise_array,"aw",@progbits
        .align  4
.Lsec_301_0:
        .global noise_array
        .type   noise_array, @object
        .size   noise_array, 6
noise_array:
        .byte   0x00, 0xfd, 0x00, 0xfd, 0x00, 0xfd

        .section .data.wifi_bbgain_max,"aw",@progbits
        .align  1
.Lsec_302_0:
        .global wifi_bbgain_max
        .type   wifi_bbgain_max, @object
        .size   wifi_bbgain_max, 1
wifi_bbgain_max:
        .byte   0x16

        .section .data.bt_rx_gain_swp,"aw",@progbits
        .align  4
.Lsec_303_0:
        .type   bt_rx_gain_swp, @object
        .size   bt_rx_gain_swp, 9
bt_rx_gain_swp:
        .byte   0x00, 0x04, 0x30, 0x34, 0x38, 0x40, 0x44, 0x50, 0x54

        .section .data.rfbb_gain_swp,"aw",@progbits
        .align  1
.Lsec_304_0:
        .type   rfbb_gain_swp, @object
        .size   rfbb_gain_swp, 1
rfbb_gain_swp:
        .byte   0x5c

        .section .data.wifi_rx_gain_swp,"aw",@progbits
        .align  4
.Lsec_305_0:
        .type   wifi_rx_gain_swp, @object
        .size   wifi_rx_gain_swp, 12
wifi_rx_gain_swp:
        .byte   0x00, 0x04, 0x30, 0x34, 0x38, 0x40, 0x44, 0x48, 0x50, 0x54, 0x58, 0x5c
