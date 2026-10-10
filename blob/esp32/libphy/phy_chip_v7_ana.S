.type   freq_i2c_addr, @object
        .comm   freq_i2c_addr, 11, 4
        .type   bt_wifi_chan_data, @object
        .comm   bt_wifi_chan_data, 20, 4

        .section .phyiram.23,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_55_0, g_phyFuns
.Lsec_55_4:
        .type   i2c_bbpll_init$part$0, @function
i2c_bbpll_init$part$0:
        entry    a1, 32
        l32r     a2, .Lsec_55_0
        movi.n   a14, 7
        mov.n    a13, a14
        movi.n   a15, 1
        movi.n   a12, 2
        l32i.n   a8, a2, 0
        movi     a11, 4
        movi     a10, 102
        l32i     a8, a8, 168
        callx8   a8
        movi.n   a15, 0
        mov.n    a14, a15
        l32i.n   a8, a2, 0
        movi.n   a13, 2
        movi.n   a12, 5
        movi.n   a11, 4
        movi     a10, 102
        l32i     a8, a8, 168
        callx8   a8
        movi.n   a15, 1
        movi.n   a14, 6
        l32i.n   a2, a2, 0
        movi.n   a13, 7
        movi.n   a12, 5
        movi.n   a11, 4
        movi     a10, 102
        l32i     a2, a2, 168
        callx8   a2
        retw.n
        .size   i2c_bbpll_init$part$0, .-i2c_bbpll_init$part$0
        .literal_position
        .literal .Lsec_55_4c, chip7_phy_init_ctrl
.Lsec_55_50:
        .global i2c_bbpll_init
        .type   i2c_bbpll_init, @function
i2c_bbpll_init:
        entry    a1, 32
        l32r     a8, .Lsec_55_4c
        l8ui     a8, a8, 1
        bnei     a8, 1, .Lsec_55_5f
        call8    .Lsec_55_4
.Lsec_55_5f:
        retw.n
        .size   i2c_bbpll_init, .-i2c_bbpll_init
.Lsec_55_61:

        .section .iram1,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_56_0, .Lsec_112_0
.Lsec_56_4:
        .global get_i2c_read_mask
        .type   get_i2c_read_mask, @function
get_i2c_read_mask:
        entry    a1, 32
        addi     a2, a2, -98
        extui    a8, a2, 0, 8
        movi.n   a9, 11
        movi.n   a2, 0
        bltu     a9, a8, .Lsec_56_1c
        l32r     a2, .Lsec_56_0
        addx4    a8, a8, a2
        l32i.n   a2, a8, 0
.Lsec_56_1c:
        retw.n
        .size   get_i2c_read_mask, .-get_i2c_read_mask
.Lsec_56_1e:
        .byte    0x00
        .byte    0x00
        .literal_position
        .literal .Lsec_56_20, g_phyFuns
        .literal .Lsec_56_24, 0x3ff4e130
        .literal .Lsec_56_28, 0xffff00ff
        .literal .Lsec_56_2c, 0x3ff4e0c4
        .literal .Lsec_56_30, 0x3ff4e0c0
        .literal .Lsec_56_34, 0x00ffff00
        .literal .Lsec_56_38, 0x3ff4e148
        .literal .Lsec_56_3c, esp_dport_access_reg_read
        .literal .Lsec_56_40, esp_dport_access_reg_read
        .literal .Lsec_56_44, esp_dport_access_reg_read
        .literal .Lsec_56_48, esp_dport_access_reg_read
        .literal .Lsec_56_4c, esp_dport_access_reg_read
.Lsec_56_50:
        .global pll_correct_dcap
        .type   pll_correct_dcap, @function
pll_correct_dcap:
        entry    a1, 48
        l32r     a6, .Lsec_56_20
        movi.n   a14, 7
        extui    a5, a2, 0, 8
        mov.n    a13, a14
        movi.n   a12, 0
        l32i.n   a7, a6, 0
        movi     a11, 1
        movi     a10, 98
        extui    a4, a4, 0, 8
        l32i     a2, a7, 164
        callx8   a2
        bnez.n   a10, .Lsec_56_76
.Lsec_56_72:
        movi.n   a2, 0
        retw.n
.Lsec_56_76:
        l32i.n   a2, a6, 0
        movi.n   a14, 0
        movi.n   a13, 7
        movi.n   a12, 5
        movi.n   a11, 1
        l32i     a2, a2, 164
        movi     a10, 98
        callx8   a2
        bbsi     a10, 1, .Lsec_56_b8
        movi.n   a9, 1
        mov.n    a2, a9
        bbsi     a10, 0, .Lsec_56_bd
        j        .Lsec_56_72
        .byte    0x00
        .byte    0x00
.Lsec_56_98:
        l32r     a4, .Lsec_56_24
        slli     a7, a7, 8
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_3c
        callx8   a8
        l32r     a3, .Lsec_56_28
        and      a10, a10, a3
        or       a7, a10, a7
        memw
        s32i.n   a7, a4, 0
        retw.n
        .byte    0x00
        .byte    0x00
.Lsec_56_b8:
        movi     a9, 255
        mov.n    a2, a9
.Lsec_56_bd:
        l32r     a6, .Lsec_56_2c
        movi     a7, 0xffffff00
        mov.n    a10, a6
        addx2    a5, a5, a5
        extui    a5, a5, 0, 8
        s32i     a9, a1, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_40
        callx8   a8
        and      a10, a10, a7
        or       a5, a10, a5
        l32r     a10, .Lsec_56_30
        memw
        s32i     a5, a6, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_44
        callx8   a8
        extui    a8, a10, 24, 8
        l32i.n   a9, a1, 0
        add.n    a7, a10, a9
        add.n    a8, a9, a8
        extui    a7, a7, 0, 8
        l32r     a5, .Lsec_56_34
        slli     a8, a8, 24
        and      a5, a10, a5
        or       a5, a5, a7
        or       a5, a5, a8
        l32r     a8, .Lsec_56_38
        mov.n    a10, a6
        memw
        s32i     a5, a8, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_48
        callx8   a8
        movi     a5, 0x200
        or       a5, a10, a5
        mov.n    a10, a6
        memw
        s32i.n   a5, a6, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_4c
        callx8   a8
        movi     a5, 0xfffffdff
        and      a10, a10, a5
        memw
        s32i.n   a10, a6, 0
        s8i      a7, a3, 0
        memw
        bnez     a4, .Lsec_56_98
        retw.n
        .size   pll_correct_dcap, .-pll_correct_dcap
.Lsec_56_13e:
        .byte    0x00
        .byte    0x00
        .literal_position
        .literal .Lsec_56_140, 0x3ff5d03c
        .literal .Lsec_56_144, 0x40000000
        .literal .Lsec_56_148, 0xbfffffff
        .literal .Lsec_56_14c, 0x3ff5d040
        .literal .Lsec_56_150, 0x7fffffff
        .literal .Lsec_56_154, esp_dport_access_reg_read
        .literal .Lsec_56_158, esp_dport_access_reg_read
.Lsec_56_15c:
        .global bb_rst_en_set
        .type   bb_rst_en_set, @function
bb_rst_en_set:
        entry    a1, 32
        l32r     a3, .Lsec_56_140
        extui    a2, a2, 0, 8
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_154
        callx8   a8
        slli     a8, a2, 30
        l32r     a4, .Lsec_56_144
        slli     a2, a2, 31
        and      a8, a8, a4
        l32r     a9, .Lsec_56_148
        and      a10, a10, a9
        or       a8, a8, a10
        l32r     a4, .Lsec_56_14c
        or       a10, a4, a4
        memw
        s32i     a8, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_158
        callx8   a8
        l32r     a3, .Lsec_56_150
        and      a10, a10, a3
        or       a2, a10, a2
        memw
        s32i.n   a2, a4, 0
        retw.n
        .size   bb_rst_en_set, .-bb_rst_en_set
        .literal_position
        .literal .Lsec_56_1a4, 0x3ff4e168
        .literal .Lsec_56_1a8, 0x3ff4e0c4
        .literal .Lsec_56_1ac, 0x02000000
        .literal .Lsec_56_1b0, esp_dport_access_reg_read
        .literal .Lsec_56_1b4, esp_dport_access_reg_read
        .literal .Lsec_56_1b8, esp_dport_access_reg_read
        .literal .Lsec_56_1bc, ets_delay_us
        .literal .Lsec_56_1c0, esp_dport_access_reg_read
.Lsec_56_1c4:
        .global phy_dis_hw_set_freq
        .type   phy_dis_hw_set_freq, @function
phy_dis_hw_set_freq:
        entry    a1, 32
        l32r     a3, .Lsec_56_1a4
        movi     a4, 0x100
        l32r     a2, .Lsec_56_1a8
.Lsec_56_1d0:
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_1b0
        callx8   a8
        bltz     a10, .Lsec_56_20e
.Lsec_56_1db:
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_1b4
        callx8   a8
        l32r     a3, .Lsec_56_1ac
        or       a3, a10, a3
        or       a10, a2, a2
        memw
        s32i     a3, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_1b8
        callx8   a8
        movi     a3, 0xfffffeff
        and      a3, a10, a3
        movi.n   a10, 2
        memw
        s32i.n   a3, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_56_1bc
        callx8   a8
        retw.n
.Lsec_56_20e:
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_1c0
        callx8   a8
        bnone    a10, a4, .Lsec_56_1d0
        j        .Lsec_56_1db
        .size   phy_dis_hw_set_freq, .-phy_dis_hw_set_freq
        .literal_position
        .literal .Lsec_56_21c, phy_force_wifi_chan_en
        .literal .Lsec_56_220, force_hw_set_freq_flag
        .literal .Lsec_56_224, 0x3ff51040
        .literal .Lsec_56_228, 0x60033c00
        .literal .Lsec_56_22c, 0x3ff5d008
        .literal .Lsec_56_230, 0x20000000
        .literal .Lsec_56_234, 0x3ff4e0c4
        .literal .Lsec_56_238, 0xe7ffffff
        .literal .Lsec_56_23c, 0x10000000
        .literal .Lsec_56_240, phy_sw_set_chan_en
        .literal .Lsec_56_244, phy_enter_critical
        .literal .Lsec_56_248, esp_dport_access_reg_read
        .literal .Lsec_56_24c, esp_dport_access_reg_read
        .literal .Lsec_56_250, esp_dport_access_reg_read
        .literal .Lsec_56_254, esp_dport_access_reg_read
        .literal .Lsec_56_258, esp_dport_access_reg_read
        .literal .Lsec_56_25c, esp_dport_access_reg_read
        .literal .Lsec_56_260, esp_dport_access_reg_read
        .literal .Lsec_56_264, esp_dport_access_reg_read
        .literal .Lsec_56_268, phy_exit_critical
.Lsec_56_26c:
        .global phy_force_wifi_chan
        .type   phy_force_wifi_chan, @function
phy_force_wifi_chan:
        entry    a1, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_enter_critical
        l32r     a8, .Lsec_56_244
        callx8   a8
        l32r     a3, .Lsec_56_21c
        movi.n   a2, 1
        mov.n    a7, a10
        movi.n   a10, 1
        movi     a6, 100
        s8i      a2, a3, 0
        memw
        nop.n
        nop
        nop
        nop.n
        l32r     a3, .Lsec_56_220
        s8i      a2, a3, 0
        memw
        call8    bb_rst_en_set
        movi     a2, -2
        l32r     a3, .Lsec_56_224
        or       a10, a3, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_248
        callx8   a8
        and      a10, a10, a2
        l32r     a2, .Lsec_56_228
        memw
        s32i.n   a10, a3, 0
        nop.n
        nop.n
        nop.n
        nop.n
        memw
        l32i.n   a5, a2, 0
        l32r     a4, .Lsec_56_22c
        l32r     a3, .Lsec_56_230
        j        .Lsec_56_2db
.Lsec_56_2cd:
        memw
        l32i.n   a8, a2, 0
        sub      a8, a8, a5
        bgeu     a6, a8, .Lsec_56_2db
        j        .Lsec_56_379
.Lsec_56_2db:
        or       a10, a4, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_24c
        callx8   a8
        and      a10, a10, a3
        extui    a10, a10, 29, 3
        bnez     a10, .Lsec_56_2cd
        j        .Lsec_56_371
        .byte    0x00
        .byte    0x00
.Lsec_56_2f2:
        l32r     a4, .Lsec_56_234
        movi     a2, 0xfffffeff
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_250
        callx8   a8
        and      a10, a10, a2
        memw
        s32i.n   a10, a4, 0
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_254
        callx8   a8
        l32r     a2, .Lsec_56_238
        and      a3, a10, a2
        mov.n    a10, a4
        l32r     a2, .Lsec_56_23c
        or       a2, a3, a2
        movi     a3, 0x100
        memw
        s32i.n   a2, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_258
        callx8   a8
        or       a10, a10, a3
        mov.n    a2, a4
        memw
        s32i.n   a10, a4, 0
.Lsec_56_336:
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_25c
        callx8   a8
        bgez     a10, .Lsec_56_358
.Lsec_56_341:
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_260
        callx8   a8
        movi     a2, 0xfffffeff
        and      a10, a10, a2
        memw
        s32i.n   a10, a4, 0
        j        .Lsec_56_366
        .byte    0x00
.Lsec_56_358:
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_264
        callx8   a8
        bany     a10, a3, .Lsec_56_336
        j        .Lsec_56_341
.Lsec_56_366:
        mov.n    a10, a7
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_exit_critical
        l32r     a8, .Lsec_56_268
        callx8   a8
        retw.n
        .byte    0x00
.Lsec_56_371:
        call8    phy_dis_hw_set_freq
        j        .Lsec_56_366
        .byte    0x00
        .byte    0x00
.Lsec_56_379:
        call8    phy_dis_hw_set_freq
        l32r     a2, .Lsec_56_240
        l8ui     a2, a2, 0
        bnez     a2, .Lsec_56_366
        j        .Lsec_56_2f2
        .size   phy_force_wifi_chan, .-phy_force_wifi_chan
        .literal_position
        .literal .Lsec_56_388, force_hw_set_freq_flag
        .literal .Lsec_56_38c, 0x3ff4e0c4
        .literal .Lsec_56_390, 0xfdffffff
        .literal .Lsec_56_394, 0x3ff5c07c
        .literal .Lsec_56_398, 0xff00ffff
        .literal .Lsec_56_39c, 0x009c0000
        .literal .Lsec_56_3a0, esp_dport_access_reg_read
        .literal .Lsec_56_3a4, esp_dport_access_reg_read
.Lsec_56_3a8:
        .global phy_en_hw_set_freq
        .type   phy_en_hw_set_freq, @function
phy_en_hw_set_freq:
        entry    a1, 32
        l32r     a2, .Lsec_56_388
        l8ui     a2, a2, 0
        bnez     a2, .Lsec_56_3cc
        l32r     a3, .Lsec_56_38c
        or       a10, a3, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_3a0
        callx8   a8
        l32r     a2, .Lsec_56_390
        and      a10, a10, a2
        memw
        s32i     a10, a3, 0
.Lsec_56_3cc:
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a2, .Lsec_56_394
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_3a4
        callx8   a8
        l32r     a8, .Lsec_56_398
        and      a10, a10, a8
        l32r     a8, .Lsec_56_39c
        or       a10, a10, a8
        memw
        s32i.n   a10, a2, 0
        retw.n
        .size   phy_en_hw_set_freq, .-phy_en_hw_set_freq
.Lsec_56_3f3:
        .byte    0x00
        .literal_position
        .literal .Lsec_56_3f4, phy_init_flag
        .literal .Lsec_56_3f8, 0x3ff4e0c4
        .literal .Lsec_56_3fc, .Lsec_112_0
        .literal .Lsec_56_400, 0x3ff4e044
        .literal .Lsec_56_404, 0xfffc00ff
        .literal .Lsec_56_408, 0x0003f000
        .literal .Lsec_56_40c, 0x0ffd3800
        .literal .Lsec_56_410, 0x02000000
        .literal .Lsec_56_414, phy_enter_critical
        .literal .Lsec_56_418, esp_dport_access_reg_read
        .literal .Lsec_56_41c, esp_dport_access_reg_read
        .literal .Lsec_56_420, esp_dport_access_reg_read
        .literal .Lsec_56_424, esp_dport_access_reg_read
        .literal .Lsec_56_428, esp_dport_access_reg_read
        .literal .Lsec_56_42c, esp_dport_access_reg_read
        .literal .Lsec_56_430, phy_exit_critical
.Lsec_56_434:
        .global ram_chip_i2c_readReg
        .type   ram_chip_i2c_readReg, @function
ram_chip_i2c_readReg:
        entry    a1, 48
        extui    a4, a4, 0, 8
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_enter_critical
        l32r     a8, .Lsec_56_414
        callx8   a8
        extui    a3, a3, 0, 8
        s32i.n   a4, a1, 0
        mov.n    a5, a10
        extui    a2, a2, 0, 8
        movi.n   a6, 1
        bltui    a3, 2, .Lsec_56_451
        movi.n   a6, 0
.Lsec_56_451:
        l32r     a8, .Lsec_56_3f4
        extui    a6, a6, 0, 8
        l8ui     a4, a8, 0
        bnez.n   a4, .Lsec_56_479
        beqz.n   a6, .Lsec_56_479
        call8    phy_dis_hw_set_freq
        l32r     a4, .Lsec_56_3f8
        movi     a7, 0xfffffeff
        or       a10, a4, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_418
        callx8   a8
        and      a10, a10, a7
        memw
        s32i     a10, a4, 0
.Lsec_56_479:
        addi     a4, a2, -98
        extui    a4, a4, 0, 8
        movi.n   a8, 11
        movi.n   a7, -1
        bltu     a8, a4, .Lsec_56_491
        l32r     a8, .Lsec_56_3fc
        addx4    a4, a4, a8
        l32i.n   a4, a4, 0
        xor      a7, a7, a4
.Lsec_56_491:
        l32r     a4, .Lsec_56_400
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_41c
        callx8   a8
        l32r     a8, .Lsec_56_404
        and      a9, a10, a8
        mov.n    a10, a4
        l32r     a8, .Lsec_56_408
        or       a8, a9, a8
        memw
        s32i.n   a8, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_420
        callx8   a8
        and      a10, a10, a7
        l32r     a7, .Lsec_56_40c
        add.n    a3, a3, a7
        slli     a3, a3, 2
        l32r     a7, .Lsec_56_410
        memw
        s32i.n   a10, a4, 0
.Lsec_56_4c8:
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_424
        callx8   a8
        bany     a10, a7, .Lsec_56_4c8
        l32i.n   a8, a1, 0
        slli     a4, a8, 8
        or       a2, a4, a2
        memw
        s32i.n   a2, a3, 0
.Lsec_56_4e0:
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_428
        callx8   a8
        and      a2, a10, a7
        bnez     a2, .Lsec_56_4e0
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_42c
        callx8   a8
        l32r     a4, .Lsec_56_3f4
        l8ui     a3, a4, 0
        movi.n   a4, 1
        moveqz   a2, a4, a3
        extui    a3, a2, 0, 8
        extui    a2, a10, 16, 8
        beqz.n   a3, .Lsec_56_50e
        beqz.n   a6, .Lsec_56_50e
        call8    phy_en_hw_set_freq
.Lsec_56_50e:
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_exit_critical
        l32r     a8, .Lsec_56_430
        callx8   a8
        retw.n
        .size   ram_chip_i2c_readReg, .-ram_chip_i2c_readReg
        .literal_position
        .literal .Lsec_56_518, phy_init_flag
        .literal .Lsec_56_51c, 0x3ff4e0c4
        .literal .Lsec_56_520, 0x0ffd3800
        .literal .Lsec_56_524, 0x02000000
        .literal .Lsec_56_528, 0x01000000
        .literal .Lsec_56_52c, phy_enter_critical
        .literal .Lsec_56_530, esp_dport_access_reg_read
        .literal .Lsec_56_534, esp_dport_access_reg_read
        .literal .Lsec_56_538, esp_dport_access_reg_read
        .literal .Lsec_56_53c, phy_exit_critical
.Lsec_56_540:
        .global ram_chip_i2c_writeReg
        .type   ram_chip_i2c_writeReg, @function
ram_chip_i2c_writeReg:
        entry    a1, 48
        extui    a5, a5, 0, 8
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_enter_critical
        l32r     a8, .Lsec_56_52c
        callx8   a8
        extui    a3, a3, 0, 8
        s32i.n   a5, a1, 0
        mov.n    a7, a10
        extui    a2, a2, 0, 8
        extui    a4, a4, 0, 8
        movi.n   a5, 1
        bltui    a3, 2, .Lsec_56_561
        movi     a5, 0
.Lsec_56_561:
        l32r     a8, .Lsec_56_518
        extui    a5, a5, 0, 8
        l8ui     a6, a8, 0
        bnez.n   a6, .Lsec_56_589
        beqz.n   a5, .Lsec_56_589
        call8    phy_dis_hw_set_freq
        l32r     a6, .Lsec_56_51c
        or       a10, a6, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_530
        callx8   a8
        movi     a8, 0xfffffeff
        and      a10, a10, a8
        memw
        s32i     a10, a6, 0
.Lsec_56_589:
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a6, .Lsec_56_520
        add      a3, a3, a6
        slli     a3, a3, 2
        l32r     a6, .Lsec_56_524
.Lsec_56_59d:
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_534
        callx8   a8
        bany     a10, a6, .Lsec_56_59d
        l32r     a8, .Lsec_56_528
        slli     a4, a4, 8
        or       a2, a2, a8
        or       a2, a4, a2
        l32i.n   a4, a1, 0
        slli     a8, a4, 16
        or       a2, a2, a8
        memw
        s32i.n   a2, a3, 0
.Lsec_56_5c1:
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_538
        callx8   a8
        and      a10, a10, a6
        bnez     a10, .Lsec_56_5c1
        l32r     a8, .Lsec_56_518
        movi.n   a3, 1
        l8ui     a2, a8, 0
        moveqz   a10, a3, a2
        extui    a10, a10, 0, 8
        beqz.n   a10, .Lsec_56_5e4
        beqz.n   a5, .Lsec_56_5e4
        call8    phy_en_hw_set_freq
.Lsec_56_5e4:
        mov.n    a10, a7
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_exit_critical
        l32r     a8, .Lsec_56_53c
        callx8   a8
        retw.n
        .size   ram_chip_i2c_writeReg, .-ram_chip_i2c_writeReg
.Lsec_56_5ee:
        .byte    0x00
        .byte    0x00
        .literal_position
        .literal .Lsec_56_5f0, phy_force_wifi_chan_en
        .literal .Lsec_56_5f4, force_hw_set_freq_flag
        .literal .Lsec_56_5f8, phy_sw_set_chan_en
        .literal .Lsec_56_5fc, 0x3ff4e168
        .literal .Lsec_56_600, 0x3ff4e0c4
        .literal .Lsec_56_604, 0xe7ffffff
        .literal .Lsec_56_608, 0x3ff51040
        .literal .Lsec_56_60c, phy_enter_critical
        .literal .Lsec_56_610, esp_dport_access_reg_read
        .literal .Lsec_56_614, esp_dport_access_reg_read
        .literal .Lsec_56_618, esp_dport_access_reg_read
        .literal .Lsec_56_61c, esp_dport_access_reg_read
        .literal .Lsec_56_620, phy_exit_critical
.Lsec_56_624:
        .global phy_unforce_wifi_chan
        .type   phy_unforce_wifi_chan, @function
phy_unforce_wifi_chan:
        entry    a1, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_enter_critical
        l32r     a8, .Lsec_56_60c
        callx8   a8
        l32r     a3, .Lsec_56_5f0
        movi.n   a2, 0
        mov.n    a5, a10
        movi.n   a10, 0
        s8i      a2, a3, 0
        memw
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a3, .Lsec_56_5f4
        s8i      a2, a3, 0
        memw
        call8    bb_rst_en_set
        l32r     a2, .Lsec_56_5f8
        l8ui     a2, a2, 0
        bnez.n   a2, .Lsec_56_691
        movi     a4, 0x100
        l32r     a3, .Lsec_56_5fc
        l32r     a2, .Lsec_56_600
.Lsec_56_661:
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_610
        callx8   a8
        bltz     a10, .Lsec_56_682
.Lsec_56_66c:
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_614
        callx8   a8
        l32r     a3, .Lsec_56_604
        and      a10, a10, a3
        memw
        s32i.n   a10, a2, 0
        j        .Lsec_56_691
.Lsec_56_682:
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_618
        callx8   a8
        bnone    a10, a4, .Lsec_56_661
        j        .Lsec_56_66c
        .byte    0x00
.Lsec_56_691:
        call8    phy_en_hw_set_freq
        l32r     a3, .Lsec_56_608
        movi.n   a2, 1
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_61c
        callx8   a8
        or       a10, a10, a2
        memw
        s32i.n   a10, a3, 0
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_exit_critical
        l32r     a8, .Lsec_56_620
        callx8   a8
        retw.n
        .size   phy_unforce_wifi_chan, .-phy_unforce_wifi_chan
.Lsec_56_6b3:
        .byte    0x00
        .literal_position
        .literal .Lsec_56_6b4, phy_wifi_pll_track_en
        .literal .Lsec_56_6b8, 0x3ff5d008
        .literal .Lsec_56_6bc, phy_sw_set_chan_en
        .literal .Lsec_56_6c0, chip7_sleep_params
        .literal .Lsec_56_6c4, g_phyFuns
        .literal .Lsec_56_6c8, phy_force_wifi_chan_en
        .literal .Lsec_56_6cc, phy_enter_critical
        .literal .Lsec_56_6d0, esp_dport_access_reg_read
        .literal .Lsec_56_6d4, phy_exit_critical
.Lsec_56_6d8:
        .global wifi_track_pll_cap
        .type   wifi_track_pll_cap, @function
wifi_track_pll_cap:
        entry    a1, 48
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_enter_critical
        l32r     a8, .Lsec_56_6cc
        callx8   a8
        l32r     a8, .Lsec_56_6b4
        movi.n   a9, 0
        mov.n    a2, a10
        l8ui     a8, a8, 0
        s8i      a9, a1, 0
        memw
        beqz     a8, .Lsec_56_75c
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a10, .Lsec_56_6b8
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_6d0
        callx8   a8
        bbsi     a10, 29, .Lsec_56_75c
        l32r     a8, .Lsec_56_6bc
        l8ui     a8, a8, 0
        bnez     a8, .Lsec_56_75c
        movi.n   a9, 13
        movi.n   a3, 84
        l32r     a8, .Lsec_56_6c0
        l8ui     a8, a8, 81
        sext     a8, a8, 7
        blt      a9, a8, .Lsec_56_729
        addx4    a8, a8, a8
        addi.n   a10, a8, 7
        extui    a3, a10, 0, 8
.Lsec_56_729:
        call8    phy_dis_hw_set_freq
        movi.n   a12, 1
        mov.n    a11, a1
        mov.n    a10, a3
        call8    pll_correct_dcap
        extui    a10, a10, 0, 8
        beqz.n   a10, .Lsec_56_751
        l8ui     a13, a1, 0
        beqz.n   a13, .Lsec_56_751
        movi.n   a12, 1
        mov.n    a11, a12
        l32r     a3, .Lsec_56_6c4
        movi     a10, 98
        l32i.n   a3, a3, 0
        l32i     a3, a3, 160
        callx8   a3
.Lsec_56_751:
        l32r     a3, .Lsec_56_6c8
        l8ui     a3, a3, 0
        bnez.n   a3, .Lsec_56_75c
        call8    phy_en_hw_set_freq
.Lsec_56_75c:
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_exit_critical
        l32r     a8, .Lsec_56_6d4
        callx8   a8
        retw.n
        .size   wifi_track_pll_cap, .-wifi_track_pll_cap
.Lsec_56_766:
        .byte    0x00
        .byte    0x00
        .literal_position
        .literal .Lsec_56_768, 0x3ff450d0
        .literal .Lsec_56_76c, 0x40000000
        .literal .Lsec_56_770, esp_dport_access_reg_read
.Lsec_56_774:
        .global phy_get_fetx_delay
        .type   phy_get_fetx_delay, @function
phy_get_fetx_delay:
        entry    a1, 32
        l32r     a10, .Lsec_56_768
        movi.n   a2, 40
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_770
        callx8   a8
        l32r     a8, .Lsec_56_76c
        and      a10, a10, a8
        extui    a10, a10, 30, 2
        movi.n   a8, 0
        movnez   a2, a8, a10
        retw.n
        .size   phy_get_fetx_delay, .-phy_get_fetx_delay
.Lsec_56_792:
        .byte    0x00
        .byte    0x00
        .literal_position
        .literal .Lsec_56_794, 0x92492493
.Lsec_56_798:
        .global get_temp_cal
        .type   get_temp_cal, @function
get_temp_cal:
        entry    a1, 32
        extui    a2, a2, 0, 8
        extui    a3, a3, 0, 8
        l32r     a9, .Lsec_56_794
        sub      a8, a2, a3
        mulsh    a9, a8, a9
        srai     a2, a8, 31
        add.n    a8, a8, a9
        srai     a8, a8, 2
        sub      a8, a8, a2
        movi.n   a2, 24
        blt      a2, a8, .Lsec_56_7c2
        sext     a8, a8, 15
        movi.n   a2, -24
        max      a2, a8, a2
.Lsec_56_7c2:
        retw.n
        .size   get_temp_cal, .-get_temp_cal
        .literal_position
        .literal .Lsec_56_7c4, bt_pwr_freq_old
        .literal .Lsec_56_7c8, bt_pwr_track_num
        .literal .Lsec_56_7cc, 0x3ff4e0c4
        .literal .Lsec_56_7d0, 0x3ff4e0c0
        .literal .Lsec_56_7d4, bt_pwr_cap_sum
        .literal .Lsec_56_7d8, btpwr_atten_offset
        .literal .Lsec_56_7dc, bt_pwr_cap_sum_old
        .literal .Lsec_56_7e0, esp_dport_access_reg_read
        .literal .Lsec_56_7e4, esp_dport_access_reg_read
.Lsec_56_7e8:
        .global btpwr_pll_track
        .type   btpwr_pll_track, @function
btpwr_pll_track:
        entry    a1, 32
        l32r     a3, .Lsec_56_7c4
        extui    a2, a2, 0, 8
        l8ui     a4, a3, 0
        bne      a4, a2, .Lsec_56_801
        l32r     a9, .Lsec_56_7c8
        l8ui     a8, a9, 0
        j        .Lsec_56_84b
        .byte    0x00
.Lsec_56_801:
        l32r     a4, .Lsec_56_7cc
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_7e0
        callx8   a8
        movi     a8, 0xffffff00
        addx2    a9, a2, a2
        extui    a9, a9, 0, 8
        and      a10, a10, a8
        or       a8, a10, a9
        l32r     a10, .Lsec_56_7d0
        memw
        s32i.n   a8, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_7e4
        callx8   a8
        srai     a10, a10, 24
        l32r     a9, .Lsec_56_7c8
        l32r     a4, .Lsec_56_7d4
        l8ui     a8, a9, 0
        addi.n   a8, a8, 1
        extui    a8, a8, 0, 8
        l16ui    a11, a4, 0
        add.n    a10, a10, a11
        s16i     a10, a4, 0
        s8i      a2, a3, 0
        s8i      a8, a9, 0
        memw
.Lsec_56_84b:
        movi.n   a3, 15
        nop.n
        nop.n
        nop.n
        l32r     a2, .Lsec_56_7d8
        l8ui     a2, a2, 0
        bgeu     a3, a8, .Lsec_56_8af
        l32r     a10, .Lsec_56_7d4
        l32r     a11, .Lsec_56_7dc
        l16si    a8, a10, 0
        addi.n   a8, a8, 8
        srai     a8, a8, 4
        sext     a4, a8, 15
        l8ui     a3, a11, 0
        sext     a3, a3, 7
        beq      a3, a4, .Lsec_56_8a4
        blti     a4, 2, .Lsec_56_882
        slli     a2, a8, 3
        extui    a2, a2, 0, 8
        j        .Lsec_56_890
.Lsec_56_882:
        movi.n   a2, 0
        bgei     a4, -1, .Lsec_56_8a1
        addx2    a2, a8, a8
        slli     a2, a2, 1
        extui    a2, a2, 0, 8
.Lsec_56_890:
        sext     a3, a2, 7
        movi.n   a2, 24
        blt      a2, a3, .Lsec_56_8a1
        movi     a2, -18
        max      a2, a3, a2
        extui    a2, a2, 0, 8
.Lsec_56_8a1:
        s8i      a8, a11, 0
.Lsec_56_8a4:
        movi.n   a3, 0
        s16i     a3, a10, 0
        s8i      a3, a9, 0
        memw
.Lsec_56_8af:
        retw.n
        .size   btpwr_pll_track, .-btpwr_pll_track
.Lsec_56_8b1:
        .byte    0x00
        .byte    0x00
        .byte    0x00
        .literal_position
        .literal .Lsec_56_8b4, 0x001fe000
        .literal .Lsec_56_8b8, 0x001ea000
        .literal .Lsec_56_8bc, 0x3ff450d0
        .literal .Lsec_56_8c0, 0x40000000
        .literal .Lsec_56_8c4, 0x3ff5103c
        .literal .Lsec_56_8c8, 0xffe007ff
        .literal .Lsec_56_8cc, esp_dport_access_reg_read
        .literal .Lsec_56_8d0, esp_dport_access_reg_read
.Lsec_56_8d4:
        .global phy_bt_ifs_set
        .type   phy_bt_ifs_set, @function
phy_bt_ifs_set:
        entry    a1, 32
        l32r     a10, .Lsec_56_8bc
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_8cc
        callx8   a8
        l32r     a2, .Lsec_56_8c0
        and      a10, a10, a2
        extui    a8, a10, 30, 2
        l32r     a3, .Lsec_56_8c4
        mov.n    a10, a3
        l32r     a2, .Lsec_56_8b8
        l32r     a9, .Lsec_56_8b4
        movnez   a2, a9, a8
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_8d0
        callx8   a8
        l32r     a8, .Lsec_56_8c8
        and      a10, a10, a8
        or       a2, a10, a2
        memw
        s32i.n   a2, a3, 0
        retw.n
        .size   phy_bt_ifs_set, .-phy_bt_ifs_set
.Lsec_56_90d:
        .byte    0x00
        .byte    0x00
        .byte    0x00
        .literal_position
        .literal .Lsec_56_910, 0x3ff4884c
        .literal .Lsec_56_914, 0x04000000
        .literal .Lsec_56_918, 0x3ff48844
        .literal .Lsec_56_91c, 0xfbffffff
        .literal .Lsec_56_920, 0xfeffffff
        .literal .Lsec_56_924, 0x01000000
        .literal .Lsec_56_928, 0x3fc00000
        .literal .Lsec_56_92c, esp_dport_access_reg_read
        .literal .Lsec_56_930, ets_delay_us
        .literal .Lsec_56_934, esp_dport_access_reg_read
        .literal .Lsec_56_938, esp_dport_access_reg_read
        .literal .Lsec_56_93c, esp_dport_access_reg_read
        .literal .Lsec_56_940, esp_dport_access_reg_read
.Lsec_56_944:
        .global tsens_code_read
        .type   tsens_code_read, @function
tsens_code_read:
        entry    a1, 32
        l32r     a3, .Lsec_56_910
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_92c
        callx8   a8
        l32r     a2, .Lsec_56_914
        or       a2, a10, a2
        movi.n   a10, 1
        memw
        s32i.n   a2, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_56_930
        callx8   a8
        l32r     a10, .Lsec_56_918
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_934
        callx8   a8
        mov.n    a4, a10
        or       a10, a3, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_938
        callx8   a8
        l32r     a8, .Lsec_56_91c
        and      a8, a10, a8
        mov.n    a10, a3
        memw
        s32i     a8, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_93c
        callx8   a8
        l32r     a8, .Lsec_56_920
        and      a8, a10, a8
        mov.n    a10, a3
        memw
        s32i.n   a8, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_940
        callx8   a8
        l32r     a8, .Lsec_56_924
        or       a10, a10, a8
        l32r     a2, .Lsec_56_928
        and      a2, a4, a2
        extui    a2, a2, 22, 8
        memw
        s32i.n   a10, a3, 0
        retw.n
        .size   tsens_code_read, .-tsens_code_read
.Lsec_56_9b6:
        .byte    0x00
        .byte    0x00
        .literal_position
        .literal .Lsec_56_9b8, btpwr_tsen_old
        .literal .Lsec_56_9bc, btpwr_atten_offset
        .literal .Lsec_56_9c0, btpwr_tsen_flag
        .literal .Lsec_56_9c4, btpwr_tsen_init
        .literal .Lsec_56_9c8, 0x92492493
.Lsec_56_9cc:
        .global btpwr_tsens_track
        .type   btpwr_tsens_track, @function
btpwr_tsens_track:
        entry    a1, 32
        call8    tsens_code_read
        l32r     a12, .Lsec_56_9b8
        movi.n   a9, 1
        l8ui     a8, a12, 0
        sub      a8, a10, a8
        addi.n   a8, a8, 5
        extui    a8, a8, 0, 16
        l32r     a2, .Lsec_56_9c0
        s8i      a9, a2, 0
        memw
        movi.n   a9, 10
        nop.n
        nop.n
        nop.n
        l32r     a2, .Lsec_56_9bc
        l8ui     a2, a2, 0
        bgeu     a9, a8, .Lsec_56_a2b
        movi.n   a2, 24
        l32r     a8, .Lsec_56_9c4
        l8ui     a9, a8, 0
        sub      a11, a9, a10
        srai     a8, a11, 31
        l32r     a9, .Lsec_56_9c8
        mulsh    a9, a11, a9
        add.n    a9, a11, a9
        srai     a9, a9, 2
        s8i      a10, a12, 0
        memw
        sub      a8, a9, a8
        blt      a2, a8, .Lsec_56_a38
        sext     a8, a8, 15
        movi.n   a2, -24
        max      a2, a8, a2
        retw.n
.Lsec_56_a2b:
        sext     a8, a2, 7
        movi.n   a2, 24
        blt      a2, a8, .Lsec_56_a38
        movi.n   a2, -24
        max      a2, a8, a2
.Lsec_56_a38:
        retw.n
        .size   btpwr_tsens_track, .-btpwr_tsens_track
.Lsec_56_a3a:
        .byte    0x00
        .byte    0x00
        .literal_position
        .literal .Lsec_56_a3c, btpwr_tsen_flag
        .literal .Lsec_56_a40, btpwr_atten_offset
        .literal .Lsec_56_a44, bt_tx_gain_cal_set
.Lsec_56_a48:
        .global bt_track_tx_power
        .type   bt_track_tx_power, @function
bt_track_tx_power:
        entry    a1, 32
        call8    btpwr_tsens_track
        extui    a3, a3, 0, 8
        extui    a2, a2, 0, 8
        extui    a10, a10, 0, 8
        beqz.n   a3, .Lsec_56_a6c
        l32r     a8, .Lsec_56_a3c
        l8ui     a8, a8, 0
        bnez.n   a8, .Lsec_56_a6c
        sext     a11, a3, 7
        mov.n    a10, a2
        call8    btpwr_pll_track
        extui    a10, a10, 0, 8
.Lsec_56_a6c:
        l32r     a8, .Lsec_56_a40
        l8ui     a3, a8, 0
        beq      a3, a10, .Lsec_56_a81
        s8i      a10, a8, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, bt_tx_gain_cal_set
        l32r     a8, .Lsec_56_a44
        callx8   a8
.Lsec_56_a81:
        retw.n
        .size   bt_track_tx_power, .-bt_track_tx_power
.Lsec_56_a83:
        .byte    0x00
        .literal_position
        .literal .Lsec_56_a84, phy_bt_pll_track_en
        .literal .Lsec_56_a88, 0x3ff5d008
        .literal .Lsec_56_a8c, phy_force_wifi_chan_en
        .literal .Lsec_56_a90, phy_sw_set_chan_en
        .literal .Lsec_56_a94, 0x3ff4e168
        .literal .Lsec_56_a98, phy_bt_power_track_en
        .literal .Lsec_56_a9c, phy_enter_critical
        .literal .Lsec_56_aa0, esp_dport_access_reg_read
        .literal .Lsec_56_aa4, esp_dport_access_reg_read
        .literal .Lsec_56_aa8, phy_exit_critical
.Lsec_56_aac:
        .global bt_track_pll_cap
        .type   bt_track_pll_cap, @function
bt_track_pll_cap:
        entry    a1, 48
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_enter_critical
        l32r     a8, .Lsec_56_a9c
        callx8   a8
        l32r     a8, .Lsec_56_a84
        mov.n    a2, a10
        l8ui     a8, a8, 0
        beqz     a8, .Lsec_56_b0c
        l32r     a10, .Lsec_56_a88
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_aa0
        callx8   a8
        bbci     a10, 29, .Lsec_56_b0c
        l32r     a8, .Lsec_56_a8c
        l8ui     a8, a8, 0
        bnez.n   a8, .Lsec_56_b0c
        l32r     a3, .Lsec_56_a90
        l8ui     a3, a3, 0
        bnez.n   a3, .Lsec_56_b0c
        l32r     a10, .Lsec_56_a94
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_aa4
        callx8   a8
        extui    a4, a10, 24, 7
        call8    phy_dis_hw_set_freq
        mov.n    a12, a3
        mov.n    a11, a1
        or       a10, a4, a4
        call8    pll_correct_dcap
        l32r     a3, .Lsec_56_a98
        extui    a10, a10, 0, 8
        l8ui     a3, a3, 0
        beqz.n   a3, .Lsec_56_b09
        sext     a11, a10, 7
        or       a10, a4, a4
        call8    bt_track_tx_power
.Lsec_56_b09:
        call8    phy_en_hw_set_freq
.Lsec_56_b0c:
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_exit_critical
        l32r     a8, .Lsec_56_aa8
        callx8   a8
        retw.n
        .size   bt_track_pll_cap, .-bt_track_pll_cap
.Lsec_56_b16:
        .byte    0x00
        .byte    0x00
        .literal_position
        .literal .Lsec_56_b18, 0x0000096c
        .literal .Lsec_56_b1c, 0x000009b4
        .literal .Lsec_56_b20, .Lsec_58_0
        .literal .Lsec_56_b24, phy_printf
.Lsec_56_b28:
        .global chan_to_freq
        .type   chan_to_freq, @function
chan_to_freq:
        entry    a1, 32
        sext     a11, a2, 7
        beqz.n   a11, .Lsec_56_b40
        movi.n   a8, 14
        blt      a8, a11, .Lsec_56_b40
        l32r     a2, .Lsec_56_b1c
        beq      a11, a8, .Lsec_56_b5c
        j        .Lsec_56_b4f
        .byte    0x00
        .byte    0x00
.Lsec_56_b40:
        l32r     a10, .Lsec_56_b20
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_56_b24
        callx8   a8
        l32r     a2, .Lsec_56_b18
        retw.n
        .byte    0x00
.Lsec_56_b4f:
        l32r     a2, .Lsec_56_b18
        addi.n   a11, a11, -1
        addx4    a11, a11, a11
        add.n    a11, a11, a2
        extui    a2, a11, 0, 16
.Lsec_56_b5c:
        retw.n
        .size   chan_to_freq, .-chan_to_freq
.Lsec_56_b5e:
        .byte    0x00
        .byte    0x00
.Lsec_56_b60:
        .global get_i2c_write_data
        .type   get_i2c_write_data, @function
get_i2c_write_data:
        entry    a1, 32
        l32i.n   a9, a1, 32
        extui    a2, a2, 0, 8
        add.n    a9, a9, a2
        add.n    a7, a7, a2
        l32i.n   a8, a1, 36
        add.n    a8, a8, a2
        s8i      a4, a9, 0
        memw
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a10, a1, 40
        add.n    a2, a10, a2
        s8i      a5, a8, 0
        s8i      a3, a7, 0
        s8i      a6, a2, 0
        memw
        retw.n
        .size   get_i2c_write_data, .-get_i2c_write_data
        .literal_position
        .literal .Lsec_56_b90, 0x0ffd3800
        .literal .Lsec_56_b94, 0x01000000
        .literal .Lsec_56_b98, 0x02000000
        .literal .Lsec_56_b9c, esp_dport_access_reg_read
.Lsec_56_ba0:
        .global i2c_write_master
        .type   i2c_write_master, @function
i2c_write_master:
        entry    a1, 32
        extui    a6, a6, 0, 8
        beqz     a6, .Lsec_56_c10
        addi.n   a7, a6, -1
        extui    a7, a7, 0, 8
        addi.n   a7, a7, 1
        add.n    a7, a3, a7
        l32r     a6, .Lsec_56_b90
        mov.n    a11, a3
        sub      a10, a7, a3
        l32r     a12, .Lsec_56_b94
        loop     a10, .Lsec_56_bed
        l8ui     a9, a2, 0
        addi.n   a2, a2, 1
        or       a9, a9, a12
        l8ui     a8, a5, 0
        addi.n   a5, a5, 1
        slli     a8, a8, 16
        or       a13, a9, a8
        l8ui     a8, a11, 0
        addi.n   a11, a11, 1
        add.n    a8, a8, a6
        slli     a8, a8, 2
        l8ui     a9, a4, 0
        addi.n   a4, a4, 1
        slli     a9, a9, 8
        or       a9, a13, a9
        memw
        s32i.n   a9, a8, 0
.Lsec_56_bed:
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a4, .Lsec_56_b98
.Lsec_56_bf8:
        l8ui     a2, a3, 0
        add.n    a2, a2, a6
        slli     a2, a2, 2
.Lsec_56_c00:
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_b9c
        callx8   a8
        bany     a10, a4, .Lsec_56_c00
        addi.n   a3, a3, 1
        bne      a7, a3, .Lsec_56_bf8
.Lsec_56_c10:
        retw.n
        .size   i2c_write_master, .-i2c_write_master
.Lsec_56_c12:
        .byte    0x00
        .byte    0x00
        .literal_position
        .literal .Lsec_56_c14, g_phyFuns
        .literal .Lsec_56_c18, 0x3ff46094
        .literal .Lsec_56_c1c, .Lsec_58_14
        .literal .Lsec_56_c20, esp_dport_access_reg_read
        .literal .Lsec_56_c24, phy_printf
.Lsec_56_c28:
        .global ram_pbus_debugmode
        .type   ram_pbus_debugmode, @function
ram_pbus_debugmode:
        entry    a1, 32
        l32r     a8, .Lsec_56_c14
        movi.n   a10, 1
        l32i.n   a8, a8, 0
        l32i     a8, a8, 172
        callx8   a8
        l32r     a10, .Lsec_56_c18
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_c20
        callx8   a8
        bbsi     a10, 0, .Lsec_56_c4d
        l32r     a10, .Lsec_56_c1c
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_56_c24
        callx8   a8
.Lsec_56_c4d:
        retw.n
        .size   ram_pbus_debugmode, .-ram_pbus_debugmode
.Lsec_56_c4f:
        .byte    0x00
        .literal_position
        .literal .Lsec_56_c50, 0x3ff46094
        .literal .Lsec_56_c54, 0x0001fffc
        .literal .Lsec_56_c58, 0xfffe0001
        .literal .Lsec_56_c5c, 0x3ff460a0
        .literal .Lsec_56_c60, esp_dport_access_reg_read
        .literal .Lsec_56_c64, esp_dport_access_reg_read
        .literal .Lsec_56_c68, esp_dport_access_reg_read
.Lsec_56_c6c:
        .global ram_pbus_force_test
        .type   ram_pbus_force_test, @function
ram_pbus_force_test:
        entry    a1, 32
        l32r     a5, .Lsec_56_c50
        extui    a2, a2, 0, 8
        extui    a4, a4, 0, 16
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_c60
        callx8   a8
        extui    a3, a3, 0, 8
        slli     a4, a4, 6
        slli     a2, a2, 2
        slli     a8, a3, 15
        or       a2, a4, a2
        or       a2, a2, a8
        l32r     a8, .Lsec_56_c54
        and      a2, a2, a8
        movi.n   a8, 2
        or       a8, a2, a8
        l32r     a2, .Lsec_56_c58
        and      a10, a10, a2
        or       a10, a8, a10
        l32r     a2, .Lsec_56_c5c
        memw
        s32i.n   a10, a5, 0
.Lsec_56_cae:
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_c64
        callx8   a8
        bltz     a10, .Lsec_56_cae
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_c68
        callx8   a8
        movi.n   a2, -3
        and      a10, a10, a2
        memw
        s32i.n   a10, a5, 0
        retw.n
        .size   ram_pbus_force_test, .-ram_pbus_force_test
.Lsec_56_ccd:
        .byte    0x00
        .byte    0x00
        .byte    0x00
        .literal_position
        .literal .Lsec_56_cd0, 0x3ff460a0
        .literal .Lsec_56_cd4, 0xffffcfff
        .literal .Lsec_56_cd8, 0x00002000
        .literal .Lsec_56_cdc, 0xfffff3ff
        .literal .Lsec_56_ce0, 0x00000800
        .literal .Lsec_56_ce4, esp_dport_access_reg_read
        .literal .Lsec_56_ce8, ets_delay_us
        .literal .Lsec_56_cec, esp_dport_access_reg_read
        .literal .Lsec_56_cf0, ets_delay_us
        .literal .Lsec_56_cf4, esp_dport_access_reg_read
        .literal .Lsec_56_cf8, ets_delay_us
        .literal .Lsec_56_cfc, esp_dport_access_reg_read
        .literal .Lsec_56_d00, esp_dport_access_reg_read
        .literal .Lsec_56_d04, esp_dport_access_reg_read
.Lsec_56_d08:
        .global force_txrxoff
        .type   force_txrxoff, @function
force_txrxoff:
        entry    a1, 32
        extui    a2, a2, 0, 8
        beqz     a2, .Lsec_56_d7d
        l32r     a2, .Lsec_56_cd0
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_ce4
        callx8   a8
        l32r     a8, .Lsec_56_cd4
        and      a9, a10, a8
        movi.n   a10, 1
        l32r     a8, .Lsec_56_cd8
        or       a8, a9, a8
        memw
        s32i.n   a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_56_ce8
        callx8   a8
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_cec
        callx8   a8
        l32r     a8, .Lsec_56_cdc
        and      a9, a10, a8
        movi.n   a10, 1
        l32r     a8, .Lsec_56_ce0
        or       a8, a9, a8
        memw
        s32i.n   a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_56_cf0
        callx8   a8
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_cf4
        callx8   a8
        movi     a8, 0xfffffcff
        and      a10, a10, a8
        movi     a8, 0x200
        or       a10, a10, a8
        memw
        s32i.n   a10, a2, 0
        movi     a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_56_cf8
        callx8   a8
        retw.n
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_56_d7d:
        l32r     a2, .Lsec_56_cd0
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_cfc
        callx8   a8
        l32r     a8, .Lsec_56_cdc
        and      a8, a10, a8
        mov.n    a10, a2
        memw
        s32i     a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_d00
        callx8   a8
        movi     a8, 0xfffffcff
        and      a8, a10, a8
        mov.n    a10, a2
        memw
        s32i.n   a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_d04
        callx8   a8
        l32r     a8, .Lsec_56_cd4
        and      a10, a10, a8
        memw
        s32i.n   a10, a2, 0
        retw.n
        .size   force_txrxoff, .-force_txrxoff
.Lsec_56_dbd:

        .section .phyiram.21,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_57_0, g_phyFuns
        .literal .Lsec_57_4, chip7_sleep_params
.Lsec_57_8:
        .global bb_bss_cbw40_ana
        .type   bb_bss_cbw40_ana, @function
bb_bss_cbw40_ana:
        entry    a1, 48
        movi.n   a14, 2
        l32r     a3, .Lsec_57_4
        movi.n   a6, 0
        movnez   a6, a14, a2
        add.n    a6, a3, a6
        mov.n    a13, a14
        l8ui     a8, a6, 84
        movi.n   a3, 1
        movnez   a13, a3, a2
        sub      a13, a8, a13
        movi.n   a9, 3
        l32r     a4, .Lsec_57_0
        movi.n   a15, 10
        mov.n    a12, a9
        movi.n   a7, 49
        moveqz   a9, a3, a2
        l32i.n   a8, a4, 0
        movi     a5, 113
        mov.n    a11, a3
        movi     a10, 103
        extui    a13, a13, 0, 8
        l32i     a8, a8, 160
        movnez   a5, a7, a2
        movi.n   a7, 5
        moveqz   a7, a15, a2
        s32i     a9, a1, 4
        s32i     a14, a1, 8
        s32i     a15, a1, 0
        callx8   a8
        movi.n   a12, 4
        mov.n    a11, a3
        movi     a10, 103
        l32i     a8, a4, 0
        l8ui     a13, a6, 84
        l32i     a6, a8, 160
        callx8   a6
        movi.n   a13, 16
        mov.n    a11, a3
        l32i.n   a6, a4, 0
        movi     a10, 103
        l32i.n   a15, a1, 0
        mov.n    a12, a15
        l32i     a6, a6, 160
        callx8   a6
        l32i.n   a9, a1, 4
        l32i.n   a14, a1, 8
        bnez.n   a2, .Lsec_57_b5
        mov.n    a12, a3
        mov.n    a11, a3
        movi     a10, 103
        l32r     a2, .Lsec_57_4
        add.n    a6, a2, a9
        movi     a2, -128
        l8ui     a13, a6, 84
        addi.n   a13, a13, 10
        or       a13, a13, a2
        extui    a13, a13, 0, 8
        l32i.n   a7, a4, 0
        l32i     a7, a7, 160
        callx8   a7
        mov.n    a11, a3
        movi     a10, 103
        l32i.n   a14, a1, 8
        mov.n    a12, a14
        l8ui     a13, a6, 84
        addi.n   a13, a13, 10
        j        .Lsec_57_ec
.Lsec_57_b5:
        l32r     a8, .Lsec_57_4
        movi     a2, -128
        add.n    a6, a8, a9
        mov.n    a12, a3
        mov.n    a11, a3
        l8ui     a13, a6, 84
        movi     a10, 103
        sub      a13, a13, a7
        addi     a13, a13, -4
        or       a13, a13, a2
        l32i.n   a7, a4, 0
        extui    a13, a13, 0, 8
        l32i     a7, a7, 160
        s32i.n   a14, a1, 8
        callx8   a7
        mov.n    a11, a3
        movi     a10, 103
        l32i.n   a14, a1, 8
        mov.n    a12, a14
        l8ui     a13, a6, 84
        addi     a13, a13, -4
.Lsec_57_ec:
        or       a13, a13, a2
        extui    a13, a13, 0, 8
        l32i.n   a2, a4, 0
        l32i     a2, a2, 160
        callx8   a2
        mov.n    a13, a5
        movi.n   a12, 9
        l32i.n   a2, a4, 0
        movi     a11, 1
        movi     a10, 103
        l32i     a2, a2, 160
        callx8   a2
        retw.n
        .size   bb_bss_cbw40_ana, .-bb_bss_cbw40_ana
.Lsec_57_10e:

        .section .rodata.str1.4,"aMS",@progbits,1
        .align  4
.Lsec_58_0:
        .ascii  "set_chan error! "
        .ascii  "%d\012\000"
.Lsec_58_14:
        .ascii  "debug mode FAIL\012"
        .ascii  "\000\000\000\000"
.Lsec_58_28:
        .ascii  "error: pll_cal e"
        .ascii  "xceeds 2ms!!!\012\000\000"
.Lsec_58_48:
        .ascii  "i=%d, \000\000"
.Lsec_58_50:
        .ascii  "0x%x, \000\000"
.Lsec_58_58:
        .ascii  "\012\000"

        .section .phyiram.22,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_59_0, g_phyFuns
        .literal .Lsec_59_4, chip7_sleep_params
.Lsec_59_8:
        .global i2c_bt_filter_set
        .type   i2c_bt_filter_set, @function
i2c_bt_filter_set:
        entry    a1, 32
        l32r     a2, .Lsec_59_0
        movi     a13, 97
        movi.n   a12, 11
        movi.n   a11, 1
        movi     a10, 103
        l32i.n   a3, a2, 0
        l32i     a3, a3, 160
        callx8   a3
        movi.n   a13, 16
        movi.n   a12, 12
        l32i.n   a3, a2, 0
        movi.n   a11, 1
        movi     a10, 103
        l32i     a3, a3, 160
        callx8   a3
        movi     a13, 234
        movi.n   a12, 15
        l32i     a3, a2, 0
        movi     a11, 1
        movi     a10, 103
        l32i     a3, a3, 160
        callx8   a3
        movi.n   a12, 7
        movi.n   a11, 1
        l32r     a3, .Lsec_59_4
        movi     a10, 103
        l32i.n   a8, a2, 0
        l8ui     a13, a3, 88
        addi     a13, a13, -2
        extui    a13, a13, 0, 8
        l32i     a8, a8, 160
        callx8   a8
        movi.n   a12, 8
        movi.n   a11, 1
        l32i.n   a8, a2, 0
        movi     a10, 103
        l8ui     a13, a3, 88
        l32i     a8, a8, 160
        callx8   a8
        movi.n   a12, 5
        movi.n   a11, 1
        l32i.n   a8, a2, 0
        movi     a10, 103
        l8ui     a13, a3, 89
        addi     a13, a13, -2
        extui    a13, a13, 0, 8
        l32i     a8, a8, 160
        callx8   a8
        movi.n   a12, 6
        movi.n   a11, 1
        l32i.n   a2, a2, 0
        movi     a10, 103
        l32i     a2, a2, 160
        l8ui     a13, a3, 89
        callx8   a2
        retw.n
        .size   i2c_bt_filter_set, .-i2c_bt_filter_set
.Lsec_59_9e:

        .section .phyiram.19,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_60_0, g_phyFuns
        .literal .Lsec_60_4, dreg_1p2_set
        .literal .Lsec_60_8, chip7_phy_init_ctrl
        .literal .Lsec_60_c, phy_enter_critical
        .literal .Lsec_60_10, i2c_write_master
        .literal .Lsec_60_14, i2c_write_master
        .literal .Lsec_60_18, i2c_write_master
        .literal .Lsec_60_1c, i2c_write_master
        .literal .Lsec_60_20, i2c_write_master
        .literal .Lsec_60_24, i2c_write_master
        .literal .Lsec_60_28, i2c_write_master
        .literal .Lsec_60_2c, i2c_write_master
        .literal .Lsec_60_30, i2c_bt_filter_set
        .literal .Lsec_60_34, .Lsec_55_4
        .literal .Lsec_60_38, phy_exit_critical
.Lsec_60_3c:
        .global phy_i2c_init
        .type   phy_i2c_init, @function
phy_i2c_init:
        entry    a1, 80
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_enter_critical
        l32r     a8, .Lsec_60_c
        callx8   a8
        l32r     a2, .Lsec_60_0
        movi.n   a12, 10
        movi.n   a11, 4
        movi     a5, 107
        movi     a6, 98
        l32i.n   a3, a2, 0
        l32i     a3, a3, 152
        s32i     a10, a1, 16
        movi     a10, 102
        callx8   a3
        mov.n    a4, a10
        movi.n   a12, 11
        l32i.n   a3, a2, 0
        movi     a11, 4
        movi     a10, 102
        l32i     a3, a3, 152
        callx8   a3
        mov.n    a7, a10
        movi.n   a12, 9
        l32i.n   a2, a2, 0
        movi.n   a11, 4
        movi     a10, 102
        movi.n   a3, 2
        l32i     a2, a2, 152
        callx8   a2
        movi     a8, 99
        movi.n   a2, 1
        s8i      a8, a1, 12
        movi.n   a8, -13
        movi.n   a15, 3
        movi.n   a9, 0
        addi.n   a12, a1, 4
        s8i      a8, a1, 0
        movi     a8, -88
        addi.n   a11, a1, 8
        movi.n   a14, 3
        mov.n    a13, a1
        s32i     a10, a1, 20
        addi     a10, a1, 12
        s8i      a8, a1, 1
        movi     a8, 119
        s8i      a9, a1, 8
        s8i      a2, a1, 4
        s8i      a5, a1, 13
        s8i      a15, a1, 5
        s8i      a6, a1, 14
        s8i      a2, a1, 10
        s8i      a15, a1, 6
        s8i      a8, a1, 2
        s8i      a3, a1, 9
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, i2c_write_master
        l32r     a8, .Lsec_60_10
        callx8   a8
        movi     a15, -49
        and      a15, a4, a15
        movi.n   a13, 16
        or       a13, a15, a13
        movi.n   a15, 8
        s8i      a15, a1, 4
        movi.n   a15, 0
        movi.n   a4, 4
        movi.n   a9, 7
        movi.n   a8, 10
        s32i.n   a13, a1, 32
        addi.n   a12, a1, 4
        addi.n   a11, a1, 8
        addi.n   a10, a1, 12
        movi.n   a14, 3
        s8i      a15, a1, 0
        movi     a15, 102
        mov.n    a13, a1
        s8i      a15, a1, 14
        memw
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a15, a1, 32
        s8i      a9, a1, 1
        s32i.n   a9, a1, 24
        s8i      a6, a1, 12
        s8i      a2, a1, 8
        s8i      a5, a1, 13
        s8i      a8, a1, 6
        s8i      a15, a1, 2
        s32i     a8, a1, 28
        s8i      a3, a1, 9
        s8i      a4, a1, 5
        s8i      a4, a1, 10
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, i2c_write_master
        l32r     a8, .Lsec_60_14
        callx8   a8
        movi     a15, -80
        movi.n   a13, -2
        and      a7, a7, a13
        s8i      a15, a1, 0
        movi.n   a15, 5
        addi.n   a12, a1, 4
        addi.n   a11, a1, 8
        addi.n   a10, a1, 12
        s8i      a15, a1, 5
        memw
        movi.n   a15, 8
        movi.n   a14, 3
        mov.n    a13, a1
        nop.n
        l32i.n   a8, a1, 28
        s8i      a15, a1, 1
        movi     a15, 102
        s8i      a15, a1, 14
        movi.n   a15, 11
        s8i      a8, a1, 4
        s8i      a6, a1, 12
        s8i      a2, a1, 8
        s8i      a5, a1, 13
        s8i      a15, a1, 6
        s8i      a7, a1, 2
        movi     a7, 9
        s8i      a3, a1, 9
        s8i      a4, a1, 10
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, i2c_write_master
        l32r     a8, .Lsec_60_18
        callx8   a8
        movi.n   a15, 6
        addi.n   a12, a1, 4
        addi.n   a11, a1, 8
        l32i.n   a9, a1, 24
        addi.n   a10, a1, 12
        movi.n   a14, 2
        or       a13, a1, a1
        s8i      a15, a1, 5
        movi     a15, -72
        s8i      a6, a1, 12
        s8i      a2, a1, 8
        s8i      a9, a1, 0
        s8i      a5, a1, 13
        s8i      a15, a1, 1
        s8i      a7, a1, 4
        s8i      a3, a1, 9
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, i2c_write_master
        l32r     a8, .Lsec_60_1c
        callx8   a8
        movi.n   a15, 0
        addi.n   a12, a1, 4
        addi.n   a11, a1, 8
        l32i.n   a9, a1, 24
        addi.n   a10, a1, 12
        movi.n   a14, 2
        mov.n    a13, a1
        s8i      a15, a1, 4
        movi.n   a15, 48
        s8i      a9, a1, 5
        movi.n   a9, 93
        s8i      a15, a1, 0
        s8i      a9, a1, 1
        s8i      a6, a1, 12
        s8i      a2, a1, 8
        s8i      a5, a1, 13
        s8i      a3, a1, 9
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, i2c_write_master
        l32r     a8, .Lsec_60_20
        callx8   a8
        addi.n   a12, a1, 4
        addi.n   a11, a1, 8
        addi.n   a10, a1, 12
        l32i.n   a8, a1, 28
        movi.n   a14, 2
        mov.n    a13, a1
        s8i      a5, a1, 13
        movi     a5, 116
        s8i      a6, a1, 12
        movi     a6, -65
        s8i      a5, a1, 1
        movi     a5, 113
        s8i      a2, a1, 8
        s8i      a6, a1, 0
        movi     a6, 104
        s8i      a8, a1, 5
        s8i      a4, a1, 4
        s8i      a3, a1, 9
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, i2c_write_master
        l32r     a8, .Lsec_60_24
        callx8   a8
        movi.n   a15, 0
        movi     a9, 103
        movi     a8, 106
        s8i      a5, a1, 0
        movi.n   a5, 3
        addi.n   a12, a1, 4
        addi.n   a11, a1, 8
        addi.n   a10, a1, 12
        s8i      a5, a1, 9
        movi     a5, 99
        movi.n   a14, 4
        mov.n    a13, a1
        s8i      a5, a1, 1
        movi.n   a5, 8
        s8i      a15, a1, 5
        movi     a15, 102
        s8i      a5, a1, 7
        movi.n   a5, 33
        s8i      a9, a1, 12
        s8i      a2, a1, 8
        s8i      a6, a1, 13
        s8i      a8, a1, 14
        s8i      a6, a1, 2
        s8i      a15, a1, 15
        s8i      a5, a1, 3
        movi.n   a5, -16
        s32i.n   a8, a1, 28
        s32i.n   a9, a1, 24
        s8i      a7, a1, 4
        s8i      a3, a1, 10
        s8i      a3, a1, 6
        s8i      a4, a1, 11
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, i2c_write_master
        l32r     a8, .Lsec_60_28
        callx8   a8
        movi.n   a14, 4
        mov.n    a13, a1
        add.n    a12, a1, a14
        l32i.n   a9, a1, 24
        addi.n   a11, a1, 8
        l32i.n   a15, a1, 20
        and      a5, a15, a5
        or       a5, a5, a4
        l32r     a10, .Lsec_60_4
        l32i.n   a8, a1, 28
        s8i      a9, a1, 12
        memw
        movi.n   a9, 11
        nop.n
        nop.n
        nop.n
        l8ui     a15, a10, 0
        addi.n   a10, a1, 12
        s8i      a9, a1, 4
        movi     a9, 97
        s8i      a2, a1, 8
        s8i      a9, a1, 0
        movi     a9, 102
        s8i      a6, a1, 13
        movi     a6, 3
        s8i      a2, a1, 5
        movi     a2, -128
        s8i      a8, a1, 14
        movi.n   a8, 0
        s8i      a2, a1, 1
        s8i      a8, a1, 6
        s8i      a15, a1, 2
        s8i      a9, a1, 15
        s8i      a6, a1, 9
        s8i      a3, a1, 10
        s8i      a4, a1, 11
        s8i      a7, a1, 7
        s8i      a5, a1, 3
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, i2c_write_master
        l32r     a8, .Lsec_60_2c
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, i2c_bt_filter_set
        l32r     a8, .Lsec_60_30
        callx8   a8
        l32r     a2, .Lsec_60_8
        l8ui     a2, a2, 1
        bnei     a2, 1, .Lsec_60_305
        .reloc  ., R_XTENSA_ASM_EXPAND, .Lsec_55_4
        l32r     a8, .Lsec_60_34
        callx8   a8
.Lsec_60_305:
        l32i.n   a10, a1, 16
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_exit_critical
        l32r     a8, .Lsec_60_38
        callx8   a8
        retw.n
        .size   phy_i2c_init, .-phy_i2c_init
.Lsec_60_30f:

        .section .text.ram_pbus_force_mode,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_61_0, 0x3ff4609c
        .literal .Lsec_61_4, 0xf7ffffff
        .literal .Lsec_61_8, 0x3ff46094
        .literal .Lsec_61_c, 0x3ff00024
        .literal .Lsec_61_10, 0x3ff5c02c
        .literal .Lsec_61_14, 0x00ffffff
        .literal .Lsec_61_18, 0x32000000
        .literal .Lsec_61_1c, 0x00800000
        .literal .Lsec_61_20, 0xff7fffff
        .literal .Lsec_61_24, esp_dport_access_reg_read
        .literal .Lsec_61_28, esp_dport_access_reg_read
        .literal .Lsec_61_2c, esp_dport_access_reg_read
        .literal .Lsec_61_30, esp_dport_access_reg_read
        .literal .Lsec_61_34, esp_dport_access_reg_read
        .literal .Lsec_61_38, ets_delay_us
        .literal .Lsec_61_3c, esp_dport_access_reg_read
        .literal .Lsec_61_40, esp_dport_access_reg_read
        .literal .Lsec_61_44, ets_delay_us
        .literal .Lsec_61_48, esp_dport_access_reg_read
.Lsec_61_4c:
        .global ram_pbus_force_mode
        .type   ram_pbus_force_mode, @function
ram_pbus_force_mode:
        entry    a1, 32
        extui    a2, a2, 0, 8
        beqz     a2, .Lsec_61_85
        l32r     a4, .Lsec_61_0
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_61_24
        callx8   a8
        l32r     a2, .Lsec_61_4
        and      a2, a10, a2
        l32r     a3, .Lsec_61_8
        mov.n    a10, a3
        memw
        s32i.n   a2, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_61_28
        callx8   a8
        movi.n   a2, 1
        or       a10, a10, a2
        memw
        s32i.n   a10, a3, 0
        retw.n
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_61_85:
        l32r     a3, .Lsec_61_8
        or       a10, a3, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_61_2c
        callx8   a8
        movi.n   a8, -2
        l32r     a2, .Lsec_61_0
        and      a8, a10, a8
        mov.n    a10, a2
        memw
        s32i.n   a8, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_61_30
        callx8   a8
        movi.n   a3, 2
        l32r     a8, .Lsec_61_4
        and      a8, a10, a8
        l32r     a10, .Lsec_61_c
        memw
        s32i     a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_61_34
        callx8   a8
        bnone    a10, a3, .Lsec_61_114
        movi     a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_61_38
        callx8   a8
        l32r     a2, .Lsec_61_10
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_61_3c
        callx8   a8
        l32r     a4, .Lsec_61_14
        and      a8, a10, a4
        mov.n    a10, a2
        l32r     a4, .Lsec_61_18
        or       a4, a8, a4
        memw
        s32i.n   a4, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_61_40
        callx8   a8
        l32r     a4, .Lsec_61_1c
        or       a4, a10, a4
        mov.n    a10, a3
        memw
        s32i     a4, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_61_44
        callx8   a8
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_61_48
        callx8   a8
        l32r     a3, .Lsec_61_20
        and      a10, a10, a3
        memw
        s32i.n   a10, a2, 0
.Lsec_61_114:
        retw.n
        .size   ram_pbus_force_mode, .-ram_pbus_force_mode
.Lsec_61_116:

        .section .rodata,"a",@progbits
        .align  4
.Lsec_62_0:
        .byte   0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01
.Lsec_62_8:
        .byte   0x30, 0x80, 0x08, 0x71

        .section .text.ram_pbus_xpd_tx_on,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_63_0, .Lsec_62_0
        .literal .Lsec_63_4, g_phyFuns
.Lsec_63_8:
        .global ram_pbus_xpd_tx_on
        .type   ram_pbus_xpd_tx_on, @function
ram_pbus_xpd_tx_on:
        entry    a1, 48
        l32r     a4, .Lsec_63_4
        movi.n   a12, 1
        mov.n    a11, a12
        movi.n   a10, 0
        extui    a3, a3, 0, 16
        l32r     a8, .Lsec_63_0
        extui    a2, a2, 0, 16
        l32i     a5, a4, 0
        l16ui    a15, a8, 0
        l16ui    a14, a8, 2
        l16ui    a13, a8, 4
        l16ui    a9, a8, 6
        l32i     a8, a5, 184
        s16i     a15, a1, 0
        s16i     a14, a1, 2
        s16i     a13, a1, 4
        s16i     a9, a1, 6
        memw
        callx8   a8
        movi.n   a11, 1
        mov.n    a10, a11
        movi     a12, 124
        l32i.n   a8, a4, 0
        l32i     a8, a8, 184
        callx8   a8
        mov.n    a10, a1
        l32i.n   a8, a4, 0
        l32i     a8, a8, 220
        callx8   a8
        mov.n    a12, a3
        movi.n   a11, 2
        l32i.n   a8, a4, 0
        movi.n   a10, 1
        l32i     a8, a8, 184
        callx8   a8
        bbci     a2, 8, .Lsec_63_89
        movi.n   a11, 1
        l32i.n   a3, a4, 0
        mov.n    a10, a11
        l32i     a8, a3, 188
        l32i     a3, a3, 184
        callx8   a8
        movi.n   a12, 2
        or       a12, a10, a12
        movi.n   a11, 1
        extui    a12, a12, 0, 16
        mov.n    a10, a11
        callx8   a3
.Lsec_63_89:
        l32i.n   a8, a4, 0
        movi     a12, 127
        movi.n   a11, 1
        movi.n   a10, 4
        l32i     a8, a8, 184
        callx8   a8
        mov.n    a12, a2
        movi.n   a11, 1
        l32i.n   a2, a4, 0
        movi.n   a10, 5
        l32i     a2, a2, 184
        callx8   a2
        retw.n
        .size   ram_pbus_xpd_tx_on, .-ram_pbus_xpd_tx_on
.Lsec_63_a8:

        .section .text.i2c_xtal_init,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_64_0, g_phyFuns
.Lsec_64_4:
        .global i2c_xtal_init
        .type   i2c_xtal_init, @function
i2c_xtal_init:
        entry    a1, 32
        l32r     a2, .Lsec_64_0
        movi     a13, 99
        movi.n   a12, 0
        movi.n   a11, 3
        movi     a10, 104
        l32i.n   a8, a2, 0
        l32i     a8, a8, 160
        callx8   a8
        movi     a13, 128
        movi.n   a12, 1
        l32i.n   a2, a2, 0
        movi.n   a11, 3
        movi     a10, 104
        l32i     a2, a2, 160
        callx8   a2
        retw.n
        .size   i2c_xtal_init, .-i2c_xtal_init
.Lsec_64_30:

        .section .text.i2c_rfpll_init,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_65_0, g_phyFuns
.Lsec_65_4:
        .global i2c_rfpll_init
        .type   i2c_rfpll_init, @function
i2c_rfpll_init:
        entry    a1, 32
        l32r     a2, .Lsec_65_0
        movi     a13, 243
        movi.n   a12, 1
        movi.n   a11, 0
        movi     a10, 99
        l32i.n   a8, a2, 0
        l32i     a8, a8, 160
        callx8   a8
        movi.n   a15, 3
        movi.n   a14, 1
        l32i.n   a8, a2, 0
        mov.n    a12, a15
        mov.n    a11, a14
        movi.n   a13, 2
        movi     a10, 98
        l32i     a8, a8, 168
        callx8   a8
        movi.n   a13, 0
        movi     a12, 8
        l32i     a8, a2, 0
        movi.n   a11, 1
        movi     a10, 98
        l32i     a8, a8, 160
        callx8   a8
        movi     a13, 176
        movi.n   a12, 10
        l32i.n   a8, a2, 0
        movi.n   a11, 1
        movi     a10, 98
        l32i     a8, a8, 160
        callx8   a8
        movi.n   a13, 7
        movi     a12, 9
        l32i     a8, a2, 0
        movi.n   a11, 1
        movi     a10, 98
        l32i     a8, a8, 160
        callx8   a8
        movi.n   a13, 63
        movi.n   a12, 0
        l32i     a8, a2, 0
        movi     a11, 1
        movi     a10, 98
        l32i     a8, a8, 160
        callx8   a8
        movi     a13, 191
        movi.n   a12, 4
        l32i.n   a2, a2, 0
        movi.n   a11, 1
        movi     a10, 98
        l32i     a2, a2, 160
        callx8   a2
        retw.n
        .size   i2c_rfpll_init, .-i2c_rfpll_init
.Lsec_65_90:

        .section .text.ram_rfpll_reset,"ax",@progbits
        .align  4
.Lsec_66_0:
        .global ram_rfpll_reset
        .type   ram_rfpll_reset, @function
ram_rfpll_reset:
        entry    a1, 32
        retw.n
        .size   ram_rfpll_reset, .-ram_rfpll_reset
.Lsec_66_5:

        .section .text.ram_restart_cal,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_67_0, g_phyFuns
.Lsec_67_4:
        .global ram_restart_cal
        .type   ram_restart_cal, @function
ram_restart_cal:
        entry    a1, 32
        l32r     a2, .Lsec_67_0
        movi.n   a15, 1
        movi.n   a14, 6
        mov.n    a13, a14
        mov.n    a11, a15
        l32i.n   a8, a2, 0
        movi     a12, 0
        movi     a10, 98
        l32i     a8, a8, 168
        callx8   a8
        movi.n   a15, 0
        movi.n   a14, 5
        l32i.n   a8, a2, 0
        mov.n    a13, a14
        mov.n    a12, a15
        movi.n   a11, 1
        movi     a10, 98
        l32i     a8, a8, 168
        callx8   a8
        movi.n   a15, 1
        movi.n   a14, 5
        l32i.n   a8, a2, 0
        mov.n    a13, a14
        mov.n    a11, a15
        movi.n   a12, 0
        movi     a10, 98
        l32i     a8, a8, 168
        callx8   a8
        movi.n   a15, 0
        movi.n   a14, 6
        l32i.n   a2, a2, 0
        mov.n    a13, a14
        mov.n    a12, a15
        movi     a11, 1
        movi     a10, 98
        l32i     a2, a2, 168
        callx8   a2
        retw.n
        .size   ram_restart_cal, .-ram_restart_cal
.Lsec_67_62:

        .section .text.ram_wait_rfpll_cal_end,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_68_0, g_phyFuns
        .literal .Lsec_68_4, .Lsec_58_28
        .literal .Lsec_68_8, ets_delay_us
        .literal .Lsec_68_c, phy_printf
.Lsec_68_10:
        .global ram_wait_rfpll_cal_end
        .type   ram_wait_rfpll_cal_end, @function
ram_wait_rfpll_cal_end:
        entry    a1, 32
        l32r     a3, .Lsec_68_0
        movi     a2, 100
.Lsec_68_19:
        movi.n   a10, 20
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_68_8
        callx8   a8
        movi.n   a14, 7
        l32i.n   a8, a3, 0
        mov.n    a13, a14
        mov.n    a12, a14
        movi.n   a11, 1
        movi     a10, 98
        l32i     a8, a8, 164
        callx8   a8
        addi.n   a8, a2, -1
        bnez.n   a10, .Lsec_68_4e
        bnei     a2, 1, .Lsec_68_48
        l32r     a10, .Lsec_68_4
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_68_c
        callx8   a8
        retw.n
        .byte    0x00
        .byte    0x00
.Lsec_68_48:
        extui    a2, a8, 0, 8
        j        .Lsec_68_19
.Lsec_68_4e:
        retw.n
        .size   ram_wait_rfpll_cal_end, .-ram_wait_rfpll_cal_end
.Lsec_68_50:

        .section .text.ram_rfpll_set_freq,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_69_0, 0x47200000
        .literal .Lsec_69_4, 0x00000000
        .literal .Lsec_69_8, 0x40080000
        .literal .Lsec_69_c, .Lsec_111_0
        .literal .Lsec_69_10, 0x44800000
        .literal .Lsec_69_14, 0x42000000
        .literal .Lsec_69_18, 0x43800000
        .literal .Lsec_69_1c, __floatunsidf
        .literal .Lsec_69_20, __divdf3
        .literal .Lsec_69_24, __truncdfsf2
        .literal .Lsec_69_28, __divsf3
.Lsec_69_2c:
        .global ram_rfpll_set_freq
        .type   ram_rfpll_set_freq, @function
ram_rfpll_set_freq:
        entry    a1, 32
        slli     a2, a2, 10
        sext     a4, a4, 15
        add.n    a10, a2, a4
        slli     a10, a10, 2
        .reloc  ., R_XTENSA_ASM_EXPAND, __floatunsidf
        l32r     a8, .Lsec_69_1c
        callx8   a8
        l32r     a12, .Lsec_69_4
        extui    a3, a3, 0, 8
        addi.n   a3, a3, -1
        extui    a3, a3, 0, 8
        l32r     a13, .Lsec_69_8
        .reloc  ., R_XTENSA_ASM_EXPAND, __divdf3
        l32r     a8, .Lsec_69_20
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, __truncdfsf2
        l32r     a8, .Lsec_69_24
        callx8   a8
        wfr      f1, a10
        l32r     a2, .Lsec_69_0
        wfr      f0, a2
        bgeui    a3, 4, .Lsec_69_78
        l32r     a2, .Lsec_69_c
        addx4    a3, a3, a2
        l32r     a2, .Lsec_69_10
        wfr      f2, a2
        lsi      f0, a3, 0
        mul.s    f0, f0, f2
.Lsec_69_78:
        rfr      a11, f0
        rfr      a10, f1
        .reloc  ., R_XTENSA_ASM_EXPAND, __divsf3
        l32r     a8, .Lsec_69_28
        callx8   a8
        l32r     a2, .Lsec_69_14
        wfr      f0, a10
        wfr      f2, a2
        sub.s    f1, f0, f2
        l32r     a2, .Lsec_69_18
        utrunc.s a3, f1, 0
        wfr      f2, a2
        extui    a3, a3, 0, 8
        float.s  f0, a3, 0
        s8i      a3, a5, 0
        sub.s    f1, f1, f0
        mul.s    f0, f1, f2
        utrunc.s a3, f0, 0
        extui    a3, a3, 0, 8
        float.s  f1, a3, 0
        s8i      a3, a5, 1
        sub.s    f0, f0, f1
        mul.s    f0, f0, f2
        utrunc.s a3, f0, 0
        s8i      a3, a5, 2
        memw
        retw.n
        .size   ram_rfpll_set_freq, .-ram_rfpll_set_freq
.Lsec_69_c5:

        .section .text.get_lna_vga_dcap_val,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_70_0, g_phyFuns
        .literal .Lsec_70_4, 0x24168e19
        .literal .Lsec_70_8, 0x27932b49
.Lsec_70_c:
        .global get_lna_vga_dcap_val
        .type   get_lna_vga_dcap_val, @function
get_lna_vga_dcap_val:
        entry    a1, 32
        extui    a2, a2, 0, 16
        addx2    a4, a2, a2
        l32r     a9, .Lsec_70_4
        slli     a4, a4, 3
        movi     a8, 0xfffffeac
        add.n    a8, a4, a8
        srai     a10, a8, 31
        l32r     a5, .Lsec_70_0
        mulsh    a8, a8, a9
        movi.n   a12, 0
        srai     a8, a8, 5
        sub      a10, a8, a10
        l32i.n   a9, a5, 0
        movi.n   a11, 15
        sub      a4, a4, a2
        addi     a4, a4, 29
        l32i.n   a8, a9, 44
        callx8   a8
        srai     a9, a4, 31
        movi.n   a12, 0
        l32r     a8, .Lsec_70_8
        movi.n   a11, 15
        mulsh    a4, a4, a8
        srai     a4, a4, 5
        l32i.n   a2, a5, 0
        l32i.n   a2, a2, 44
        s16i     a10, a3, 0
        memw
        sub      a10, a4, a9
        callx8   a2
        s16i     a10, a3, 2
        memw
        retw.n
        .size   get_lna_vga_dcap_val, .-get_lna_vga_dcap_val
.Lsec_70_68:

        .section .text.chip_v7_rxmax_ext_ana,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_71_0, 0x0000fff8
        .literal .Lsec_71_4, g_phyFuns
        .literal .Lsec_71_8, get_lna_vga_dcap_val
.Lsec_71_c:
        .global chip_v7_rxmax_ext_ana
        .type   chip_v7_rxmax_ext_ana, @function
chip_v7_rxmax_ext_ana:
        entry    a1, 48
        l32r     a4, .Lsec_71_4
        movi.n   a13, 7
        movi.n   a14, 0
        movi.n   a12, 5
        movi.n   a11, 1
        l32i     a3, a4, 0
        movi     a10, 98
        extui    a2, a2, 0, 8
        l32i     a3, a3, 164
        callx8   a3
        mov.n    a11, a1
        .reloc  ., R_XTENSA_ASM_EXPAND, get_lna_vga_dcap_val
        l32r     a8, .Lsec_71_8
        callx8   a8
        l16ui    a8, a1, 0
        sext     a13, a8, 15
        l16ui    a3, a1, 2
        sext     a5, a3, 15
        bltui    a2, 3, .Lsec_71_66
        movi.n   a2, 8
        blt      a13, a2, .Lsec_71_48
        l32r     a2, .Lsec_71_0
.Lsec_71_48:
        add.n    a8, a2, a8
        movi.n   a13, 64
        or       a13, a8, a13
        extui    a13, a13, 0, 8
        l32i.n   a2, a4, 0
        movi.n   a12, 4
        movi.n   a11, 0
        movi     a10, 100
        l32i     a2, a2, 160
        callx8   a2
        j        .Lsec_71_80
        .byte    0x00
        .byte    0x00
.Lsec_71_66:
        l32i.n   a9, a4, 0
        movi.n   a8, 64
        or       a13, a13, a8
        extui    a13, a13, 0, 8
        movi.n   a12, 4
        l32i     a8, a9, 160
        movi.n   a11, 0
        movi     a10, 100
        callx8   a8
        bnei     a2, 2, .Lsec_71_8d
.Lsec_71_80:
        movi.n   a13, 8
        blt      a5, a13, .Lsec_71_88
        l32r     a13, .Lsec_71_0
.Lsec_71_88:
        add.n    a3, a13, a3
        sext     a5, a3, 15
.Lsec_71_8d:
        movi.n   a13, 64
        or       a13, a5, a13
        l32i.n   a2, a4, 0
        extui    a13, a13, 0, 8
        movi.n   a12, 7
        movi.n   a11, 0
        movi     a10, 100
        l32i     a2, a2, 160
        callx8   a2
        retw.n
        .size   chip_v7_rxmax_ext_ana, .-chip_v7_rxmax_ext_ana
.Lsec_71_a6:

        .section .text.phy_freq_correct_opt,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_72_0, g_phyFuns
        .literal .Lsec_72_4, phy_freq_offset
        .literal .Lsec_72_8, 0x3ff5c400
        .literal .Lsec_72_c, 0xfff8ffff
        .literal .Lsec_72_10, 0x3ff4e054
        .literal .Lsec_72_14, esp_dport_access_reg_read
        .literal .Lsec_72_18, esp_dport_access_reg_read
.Lsec_72_1c:
        .global phy_freq_correct_opt
        .type   phy_freq_correct_opt, @function
phy_freq_correct_opt:
        entry    a1, 32
        l32r     a2, .Lsec_72_0
        movi     a10, 1
        l32r     a3, .Lsec_72_4
        l32i     a2, a2, 0
        l16si    a11, a3, 0
        l32i     a2, a2, 248
        callx8   a2
        l32r     a3, .Lsec_72_8
        or       a10, a3, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_72_14
        callx8   a8
        l32r     a8, .Lsec_72_c
        and      a8, a10, a8
        l32r     a2, .Lsec_72_10
        or       a10, a2, a2
        memw
        s32i     a8, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_72_18
        callx8   a8
        movi     a8, 0xffffff00
        and      a10, a10, a8
        movi     a8, 200
        or       a10, a10, a8
        memw
        s32i.n   a10, a2, 0
        retw.n
        .size   phy_freq_correct_opt, .-phy_freq_correct_opt
.Lsec_72_6b:

        .section .text.chip_v7_adc_wr_dly,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_73_0, g_phyFuns
.Lsec_73_4:
        .global chip_v7_adc_wr_dly
        .type   chip_v7_adc_wr_dly, @function
chip_v7_adc_wr_dly:
        entry    a1, 48
        l32r     a8, .Lsec_73_0
        extui    a3, a3, 0, 8
        slli     a13, a3, 2
        extui    a2, a2, 0, 8
        extui    a4, a4, 0, 8
        l32i.n   a9, a8, 0
        or       a13, a13, a2
        slli     a4, a4, 4
        extui    a5, a5, 0, 8
        or       a13, a13, a4
        slli     a5, a5, 6
        l32i     a2, a9, 160
        or       a13, a13, a5
        extui    a13, a13, 0, 8
        movi.n   a12, 8
        movi.n   a11, 4
        s32i.n   a8, a1, 0
        movi     a10, 102
        callx8   a2
        extui    a7, a7, 0, 8
        extui    a6, a6, 0, 8
        l32i.n   a8, a1, 0
        slli     a7, a7, 2
        or       a15, a7, a6
        extui    a15, a15, 0, 8
        movi.n   a14, 0
        l32i.n   a2, a8, 0
        movi.n   a13, 3
        movi.n   a12, 9
        movi.n   a11, 4
        movi     a10, 102
        l32i     a2, a2, 168
        callx8   a2
        retw.n
        .size   chip_v7_adc_wr_dly, .-chip_v7_adc_wr_dly
.Lsec_73_62:

        .section .text.i2c_bbtop_init,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_74_0, g_phyFuns
.Lsec_74_4:
        .global i2c_bbtop_init
        .type   i2c_bbtop_init, @function
i2c_bbtop_init:
        entry    a1, 32
        l32r     a2, .Lsec_74_0
        movi.n   a13, 33
        movi.n   a12, 8
        movi     a11, 4
        movi     a10, 102
        l32i.n   a8, a2, 0
        l32i     a8, a8, 160
        callx8   a8
        movi     a13, 132
        movi.n   a12, 9
        l32i     a8, a2, 0
        movi     a11, 4
        movi     a10, 102
        l32i     a8, a8, 160
        callx8   a8
        movi     a13, 113
        movi.n   a12, 9
        l32i     a8, a2, 0
        movi     a11, 1
        movi     a10, 103
        l32i     a8, a8, 160
        callx8   a8
        movi     a13, 97
        movi.n   a12, 11
        l32i.n   a2, a2, 0
        movi.n   a11, 1
        movi     a10, 103
        l32i     a2, a2, 160
        callx8   a2
        retw.n
        .size   i2c_bbtop_init, .-i2c_bbtop_init
.Lsec_74_58:

        .section .text.i2c_rftx_init,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_75_0, g_phyFuns
.Lsec_75_4:
        .global i2c_rftx_init
        .type   i2c_rftx_init, @function
i2c_rftx_init:
        entry    a1, 32
        l32r     a2, .Lsec_75_0
        movi     a13, 136
        movi.n   a12, 3
        movi.n   a11, 2
        movi     a10, 107
        l32i.n   a8, a2, 0
        l32i     a8, a8, 160
        callx8   a8
        movi.n   a13, 6
        movi.n   a12, 4
        l32i.n   a8, a2, 0
        movi.n   a11, 2
        movi     a10, 107
        l32i     a8, a8, 160
        callx8   a8
        movi.n   a13, 8
        movi.n   a12, 5
        l32i     a8, a2, 0
        movi     a11, 2
        movi     a10, 107
        l32i     a8, a8, 160
        callx8   a8
        movi     a13, 248
        movi.n   a12, 6
        l32i.n   a8, a2, 0
        movi.n   a11, 2
        movi     a10, 107
        l32i     a8, a8, 160
        callx8   a8
        movi.n   a13, 93
        movi.n   a12, 7
        l32i     a8, a2, 0
        movi.n   a11, 2
        movi     a10, 107
        l32i     a8, a8, 160
        callx8   a8
        movi     a13, 116
        movi.n   a12, 10
        l32i.n   a2, a2, 0
        movi.n   a11, 2
        movi     a10, 107
        l32i     a2, a2, 160
        callx8   a2
        retw.n
        .size   i2c_rftx_init, .-i2c_rftx_init
.Lsec_75_78:

        .section .text.i2c_bias_init,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_76_0, g_phyFuns
        .literal .Lsec_76_4, dreg_1p2_set
.Lsec_76_8:
        .global i2c_bias_init
        .type   i2c_bias_init, @function
i2c_bias_init:
        entry    a1, 32
        l32r     a2, .Lsec_76_0
        movi.n   a12, 2
        mov.n    a11, a12
        movi     a13, 104
        movi     a10, 106
        l32i.n   a8, a2, 0
        l32i     a8, a8, 160
        callx8   a8
        movi.n   a12, 0
        movi.n   a11, 2
        l32r     a8, .Lsec_76_4
        movi     a10, 106
        l32i.n   a2, a2, 0
        l8ui     a13, a8, 0
        l32i     a2, a2, 160
        callx8   a2
        retw.n
        .size   i2c_bias_init, .-i2c_bias_init
.Lsec_76_37:

        .section .text.rfpll_1p2_opt,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_77_0, .Lsec_62_8
        .literal .Lsec_77_4, g_phyFuns
        .literal .Lsec_77_8, dreg_1p2_set
.Lsec_77_c:
        .global rfpll_1p2_opt
        .type   rfpll_1p2_opt, @function
rfpll_1p2_opt:
        entry    a1, 48
        l32r     a5, .Lsec_77_0
        movi.n   a2, 0
        mov.n    a3, a1
        mov.n    a4, a2
        l8ui     a11, a5, 0
        l8ui     a10, a5, 1
        l8ui     a9, a5, 2
        l8ui     a8, a5, 3
        l32r     a5, .Lsec_77_4
        s8i      a11, a1, 0
        s8i      a10, a1, 1
        s8i      a9, a1, 2
        s8i      a8, a1, 3
        memw
.Lsec_77_36:
        mov.n    a12, a2
        movi     a11, 1
        movi     a10, 98
        add      a2, a2, a11
        l32i     a8, a5, 0
        l32i     a8, a8, 152
        callx8   a8
        addi.n   a9, a4, 1
        l8ui     a8, a3, 0
        addi     a3, a3, 1
        beq      a8, a10, .Lsec_77_58
        extui    a4, a9, 0, 8
.Lsec_77_58:
        bnei     a2, 4, .Lsec_77_36
        beqz.n   a4, .Lsec_77_79
        l32i.n   a2, a5, 0
        movi.n   a4, 42
        movi.n   a13, 42
        movi.n   a12, 0
        movi.n   a11, 2
        l32r     a3, .Lsec_77_8
        movi     a10, 106
        l32i     a2, a2, 160
        s8i      a4, a3, 0
        memw
        callx8   a2
.Lsec_77_79:
        retw.n
        .size   rfpll_1p2_opt, .-rfpll_1p2_opt
.Lsec_77_7b:

        .section .text.get_rf_freq_cap,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_78_0, g_phyFuns
        .literal .Lsec_78_4, chip7_phy_init_ctrl
        .literal .Lsec_78_8, chip_v7_rxmax_ext_ana
.Lsec_78_c:
        .global get_rf_freq_cap
        .type   get_rf_freq_cap, @function
get_rf_freq_cap:
        entry    a1, 32
        l32r     a6, .Lsec_78_0
        extui    a2, a2, 0, 16
        sext     a3, a3, 15
        l32i.n   a8, a6, 0
        l32i     a8, a8, 224
        callx8   a8
        mov.n    a13, a4
        mov.n    a12, a3
        l32r     a9, .Lsec_78_4
        mov.n    a10, a2
        l32i.n   a8, a6, 0
        l8ui     a11, a9, 1
        l32i     a8, a8, 240
        callx8   a8
        mov.n    a10, a4
        l32i.n   a4, a6, 0
        l32i     a4, a4, 232
        callx8   a4
        l32i.n   a4, a6, 0
        l32i     a4, a4, 228
        callx8   a4
        l32i.n   a4, a6, 0
        l32i     a4, a4, 236
        callx8   a4
        movi.n   a10, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, chip_v7_rxmax_ext_ana
        l32r     a8, .Lsec_78_8
        callx8   a8
        l32i.n   a4, a6, 0
        movi.n   a14, 0
        movi.n   a13, 7
        movi.n   a12, 5
        movi.n   a11, 1
        l32i     a4, a4, 164
        movi     a10, 98
        callx8   a4
        movi.n   a12, 2
        movi.n   a11, 1
        s8i      a10, a5, 0
        memw
        movi     a10, 98
        nop.n
        nop.n
        nop.n
        l32i.n   a4, a6, 0
        l32i     a4, a4, 152
        callx8   a4
        mov.n    a4, a10
        movi.n   a14, 0
        l32i.n   a6, a6, 0
        movi.n   a13, 3
        movi.n   a12, 6
        movi.n   a11, 1
        movi     a10, 98
        l32i     a6, a6, 164
        callx8   a6
        movi.n   a6, -16
        and      a8, a4, a6
        movi     a6, -128
        or       a10, a10, a6
        or       a10, a8, a10
        s8i      a10, a5, 1
        memw
        retw.n
        .size   get_rf_freq_cap, .-get_rf_freq_cap
.Lsec_78_af:

        .section .text.correct_rfpll_offset,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_79_0, rfpll_offset_delta
        .literal .Lsec_79_4, phy_freq_wifi_only
        .literal .Lsec_79_8, 0x3ff4e0c4
        .literal .Lsec_79_c, 0x3ff4e0c0
        .literal .Lsec_79_10, 0x00ffffff
        .literal .Lsec_79_14, 0xff000000
        .literal .Lsec_79_18, 0x3ff4e148
        .literal .Lsec_79_1c, esp_dport_access_reg_read
        .literal .Lsec_79_20, esp_dport_access_reg_read
        .literal .Lsec_79_24, esp_dport_access_reg_read
        .literal .Lsec_79_28, esp_dport_access_reg_read
.Lsec_79_2c:
        .global correct_rfpll_offset
        .type   correct_rfpll_offset, @function
correct_rfpll_offset:
        entry    a1, 48
        extui    a3, a3, 0, 8
        sext     a2, a2, 15
        bnez.n   a3, .Lsec_79_45
        addx8    a2, a2, a2
        srai     a2, a2, 2
        sext     a2, a2, 15
        s32i.n   a2, a1, 8
        j        .Lsec_79_53
.Lsec_79_45:
        addx2    a2, a2, a2
        addx8    a2, a2, a2
        srai     a2, a2, 3
        sext     a2, a2, 15
        s32i.n   a2, a1, 8
.Lsec_79_53:
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a2, .Lsec_79_0
        l32i.n   a4, a1, 8
        l16si    a3, a2, 0
        bne      a4, a3, .Lsec_79_69
        j        .Lsec_79_131
.Lsec_79_69:
        sub      a3, a4, a3
        movi.n   a2, 85
        l32r     a8, .Lsec_79_4
        movi.n   a4, 14
        movi.n   a5, 0
        movi     a7, 0xffffff00
        movi     a6, 0x200
        l8ui     a12, a8, 0
        movnez   a2, a4, a12
        s32i.n   a2, a1, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a2, .Lsec_79_8
        s32i.n   a3, a1, 4
        movi.n   a3, 1
.Lsec_79_92:
        mov.n    a10, a2
        mov.n    a4, a3
        beqz.n   a12, .Lsec_79_ad
        movi     a8, 12
        movi     a4, 253
        bltu     a8, a5, .Lsec_79_ad
        slli     a9, a5, 4
        sub      a9, a9, a5
        addi     a9, a9, 37
        extui    a4, a9, 0, 8
.Lsec_79_ad:
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_79_1c
        callx8   a8
        and      a10, a10, a7
        or       a9, a10, a4
        l32r     a10, .Lsec_79_c
        addi.n   a5, a5, 1
        extui    a5, a5, 0, 8
        memw
        s32i     a9, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_79_20
        callx8   a8
        l32r     a4, .Lsec_79_10
        and      a9, a10, a4
        l32r     a8, .Lsec_79_14
        and      a10, a10, a8
        l32i.n   a4, a1, 4
        add.n    a9, a9, a4
        or       a10, a9, a10
        l32r     a8, .Lsec_79_18
        memw
        s32i.n   a10, a8, 0
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_79_24
        callx8   a8
        or       a9, a10, a6
        mov.n    a10, a2
        memw
        s32i.n   a9, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_79_28
        callx8   a8
        movi     a9, 0xfffffdff
        and      a10, a10, a9
        addi.n   a8, a3, 3
        l32i.n   a4, a1, 0
        extui    a3, a8, 0, 8
        memw
        s32i.n   a10, a2, 0
        beq      a4, a5, .Lsec_79_126
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a8, .Lsec_79_4
        l8ui     a12, a8, 0
        j        .Lsec_79_92
.Lsec_79_126:
        l32i.n   a3, a1, 8
        l32r     a2, .Lsec_79_0
        s16i     a3, a2, 0
        memw
.Lsec_79_131:
        retw.n
        .size   correct_rfpll_offset, .-correct_rfpll_offset
.Lsec_79_133:

        .section .text.wr_rf_freq_mem,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_80_0, 0x3ff4e0c4
        .literal .Lsec_80_4, 0x3ff4e148
        .literal .Lsec_80_8, esp_dport_access_reg_read
        .literal .Lsec_80_c, esp_dport_access_reg_read
        .literal .Lsec_80_10, esp_dport_access_reg_read
.Lsec_80_14:
        .global wr_rf_freq_mem
        .type   wr_rf_freq_mem, @function
wr_rf_freq_mem:
        entry    a1, 48
        extui    a2, a2, 0, 8
        addx2    a2, a2, a2
        addi.n   a5, a3, 12
        l32r     a4, .Lsec_80_0
        extui    a2, a2, 0, 8
        movi     a7, 0xffffff00
        movi     a6, 0x200
        s32i.n   a5, a1, 0
.Lsec_80_2d:
        mov.n    a10, a4
        nop.n
        nop.n
        nop.n
        l32i.n   a5, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_80_8
        callx8   a8
        and      a10, a10, a7
        or       a9, a10, a2
        mov.n    a10, a4
        l32r     a8, .Lsec_80_4
        addi.n   a2, a2, 1
        addi.n   a3, a3, 4
        extui    a2, a2, 0, 8
        memw
        s32i     a9, a4, 0
        memw
        s32i     a5, a8, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_80_c
        callx8   a8
        or       a9, a10, a6
        mov.n    a10, a4
        movi     a5, 0xfffffdff
        memw
        s32i.n   a9, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_80_10
        callx8   a8
        and      a10, a10, a5
        l32i.n   a8, a1, 0
        memw
        s32i.n   a10, a4, 0
        bne      a8, a3, .Lsec_80_2d
        retw.n
        .size   wr_rf_freq_mem, .-wr_rf_freq_mem
.Lsec_80_83:

        .section .text.write_freq_mem_all,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_81_0, chip7_sleep_params
        .literal .Lsec_81_4, phy_freq_wifi_only
        .literal .Lsec_81_8, phy_byte_to_word
        .literal .Lsec_81_c, wr_rf_freq_mem
.Lsec_81_10:
        .global write_freq_mem_all
        .type   write_freq_mem_all, @function
write_freq_mem_all:
        entry    a1, 64
        l32r     a4, .Lsec_81_0
        l32i.n   a3, a4, 0
        movi.n   a4, 24
        and      a3, a4, a3
        beqi     a3, 16, .Lsec_81_23
        j        .Lsec_81_a8
.Lsec_81_23:
        movi.n   a7, 14
        l32r     a6, .Lsec_81_4
        movi.n   a5, 12
        movi.n   a4, 0
        l8ui     a3, a6, 0
        movi.n   a6, 85
        movnez   a6, a7, a3
        s32i.n   a6, a1, 20
.Lsec_81_36:
        beqz.n   a3, .Lsec_81_51
        movi.n   a3, 84
        movi.n   a6, 12
        movi     a8, 0x3f0
        s32i.n   a3, a1, 16
        bltu     a6, a4, .Lsec_81_59
        addx2    a8, a5, a5
        s32i.n   a5, a1, 16
        slli     a8, a8, 2
        j        .Lsec_81_59
        .byte    0x00
        .byte    0x00
.Lsec_81_51:
        addx2    a8, a4, a4
        s32i.n   a4, a1, 16
        slli     a8, a8, 2
.Lsec_81_59:
        addi.n   a3, a8, 12
        add.n    a6, a2, a8
        mov.n    a7, a1
        add.n    a3, a2, a3
.Lsec_81_61:
        mov.n    a10, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_byte_to_word
        l32r     a8, .Lsec_81_8
        callx8   a8
        s32i.n   a10, a7, 0
        addi.n   a6, a6, 4
        addi.n   a7, a7, 4
        bne      a3, a6, .Lsec_81_61
        mov.n    a11, a1
        nop.n
        l32i.n   a10, a1, 16
        addi.n   a4, a4, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, wr_rf_freq_mem
        l32r     a8, .Lsec_81_c
        callx8   a8
        addi.n   a5, a5, 5
        extui    a3, a4, 0, 8
        l32i.n   a6, a1, 20
        extui    a5, a5, 0, 8
        bgeu     a3, a6, .Lsec_81_99
        l32r     a6, .Lsec_81_4
        l8ui     a3, a6, 0
        j        .Lsec_81_36
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_81_99:
        l32r     a2, .Lsec_81_0
        l32i.n   a3, a2, 0
        movi.n   a2, 8
        or       a2, a3, a2
        l32r     a3, .Lsec_81_0
        s32i.n   a2, a3, 0
.Lsec_81_a8:
        retw.n
        .size   write_freq_mem_all, .-write_freq_mem_all
.Lsec_81_aa:

        .section .text.get_rfrx_dcap_bt,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_82_0, g_phyFuns
.Lsec_82_4:
        .global get_rfrx_dcap_bt
        .type   get_rfrx_dcap_bt, @function
get_rfrx_dcap_bt:
        entry    a1, 32
        l32r     a4, .Lsec_82_0
        extui    a12, a2, 0, 8
        movi.n   a11, 0
        movi     a10, 100
        l32i.n   a2, a4, 0
        l32i     a2, a2, 152
        callx8   a2
        mov.n    a3, a10
        extui    a10, a10, 0, 4
        l32i.n   a2, a4, 0
        movi.n   a12, 1
        movi.n   a11, 15
        addi     a10, a10, -5
        l32i.n   a2, a2, 44
        callx8   a2
        movi.n   a2, -16
        and      a2, a3, a2
        or       a2, a2, a10
        extui    a2, a2, 0, 8
        retw.n
        .size   get_rfrx_dcap_bt, .-get_rfrx_dcap_bt
.Lsec_82_3a:

        .section .text.get_rf_freq_init$part$2,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_83_0, 0x0000096c
        .literal .Lsec_83_4, 0x000009b4
        .literal .Lsec_83_8, phy_freq_wifi_only
        .literal .Lsec_83_c, g_phyFuns
        .literal .Lsec_83_10, 0x00000960
        .literal .Lsec_83_14, chip7_sleep_params
        .literal .Lsec_83_18, get_rf_freq_cap
        .literal .Lsec_83_1c, get_rfrx_dcap_bt
        .literal .Lsec_83_20, get_rfrx_dcap_bt
        .literal .Lsec_83_24, wr_rf_freq_mem
.Lsec_83_28:
        .type   get_rf_freq_init$part$2, @function
get_rf_freq_init$part$2:
        entry    a1, 80
        l32r     a3, .Lsec_83_c
        movi.n   a15, 0
        movi.n   a14, 7
        movi.n   a5, 85
        mov.n    a13, a14
        l32r     a4, .Lsec_83_8
        mov.n    a12, a15
        movi.n   a11, 1
        movi     a10, 98
        movi.n   a6, 12
        l32i.n   a2, a3, 0
        l8ui     a7, a4, 0
        movi.n   a4, 14
        moveqz   a4, a5, a7
        movi.n   a7, 0
        l32i     a2, a2, 168
        s32i.n   a4, a1, 32
        callx8   a2
        movi.n   a14, 7
        mov.n    a13, a14
        movi.n   a15, 0
        l32i.n   a2, a3, 0
        movi     a12, 2
        movi     a11, 1
        movi     a10, 98
        l32i     a2, a2, 168
        callx8   a2
        movi.n   a12, 0
        movi.n   a11, 3
        l32i.n   a2, a3, 0
        movi     a10, 104
        l32i     a2, a2, 152
        callx8   a2
        mov.n    a2, a10
        movi.n   a12, 5
        l32i.n   a4, a3, 0
        movi.n   a11, 4
        movi     a10, 102
        l32i     a4, a4, 152
        callx8   a4
        slli     a10, a10, 16
        slli     a4, a2, 24
        or       a10, a4, a10
        movi     a13, 110
        l32r     a2, .Lsec_83_0
        mov.n    a5, a13
        s32i.n   a10, a1, 36
.Lsec_83_a0:
        movi.n   a14, 84
        nop.n
        nop.n
        nop.n
        l32r     a8, .Lsec_83_8
        l8ui     a4, a8, 0
        beqz.n   a4, .Lsec_83_c1
        movi.n   a8, 12
        l32r     a4, .Lsec_83_4
        bltu     a8, a7, .Lsec_83_ca
        mov.n    a4, a2
        mov.n    a14, a6
        j        .Lsec_83_ca
        .byte    0x00
        .byte    0x00
.Lsec_83_c1:
        l32r     a8, .Lsec_83_10
        or       a14, a7, a7
        add      a4, a7, a8
.Lsec_83_ca:
        movi.n   a11, 1
        mov.n    a13, a5
        l32i.n   a9, a3, 0
        mov.n    a12, a11
        movi     a10, 98
        add.n    a7, a7, a11
        addi.n   a2, a2, 5
        l32i     a9, a9, 160
        s32i     a14, a1, 40
        callx8   a9
        addi.n   a13, a1, 15
        addi.n   a12, a1, 12
        movi.n   a11, 0
        or       a10, a4, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, get_rf_freq_cap
        l32r     a8, .Lsec_83_18
        callx8   a8
        movi.n   a10, 4
        l8ui     a5, a1, 15
        .reloc  ., R_XTENSA_ASM_EXPAND, get_rfrx_dcap_bt
        l32r     a8, .Lsec_83_1c
        callx8   a8
        mov.n    a4, a10
        movi.n   a10, 7
        .reloc  ., R_XTENSA_ASM_EXPAND, get_rfrx_dcap_bt
        l32r     a8, .Lsec_83_20
        callx8   a8
        l32i.n   a8, a1, 36
        slli     a10, a10, 8
        or       a4, a4, a8
        or       a9, a4, a10
        mov.n    a11, a1
        l32i.n   a14, a1, 40
        mov.n    a10, a14
        l8ui     a12, a1, 12
        slli     a12, a12, 16
        l8ui     a4, a1, 13
        slli     a4, a4, 8
        or       a4, a12, a4
        l8ui     a14, a1, 16
        slli     a14, a14, 8
        l8ui     a12, a1, 15
        or       a14, a14, a12
        l8ui     a12, a1, 14
        or       a4, a4, a12
        s32i.n   a4, a1, 4
        extui    a4, a7, 0, 8
        s32i.n   a14, a1, 0
        s32i     a9, a1, 8
        .reloc  ., R_XTENSA_ASM_EXPAND, wr_rf_freq_mem
        l32r     a8, .Lsec_83_24
        callx8   a8
        addi.n   a8, a6, 5
        extui    a6, a8, 0, 8
        l32i.n   a8, a1, 32
        bgeu     a4, a8, .Lsec_83_155
        j        .Lsec_83_a0
.Lsec_83_155:
        l32r     a3, .Lsec_83_14
        movi.n   a2, 16
        l32i.n   a4, a3, 0
        or       a2, a4, a2
        s32i.n   a2, a3, 0
        retw.n
        .size   get_rf_freq_init$part$2, .-get_rf_freq_init$part$2
.Lsec_83_163:

        .section .text.get_rf_freq_init,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_84_0, chip7_sleep_params
        .literal .Lsec_84_4, .Lsec_83_28
.Lsec_84_8:
        .global get_rf_freq_init
        .type   get_rf_freq_init, @function
get_rf_freq_init:
        entry    a1, 32
        l32r     a8, .Lsec_84_0
        l32i.n   a8, a8, 0
        bbsi     a8, 4, .Lsec_84_19
        .reloc  ., R_XTENSA_ASM_EXPAND, .Lsec_83_28
        l32r     a8, .Lsec_84_4
        callx8   a8
.Lsec_84_19:
        retw.n
        .size   get_rf_freq_init, .-get_rf_freq_init
.Lsec_84_1b:

        .section .text.bt_i2c_read_set,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_85_0, 0x3ff4e14c
        .literal .Lsec_85_4, 0xfff0ffff
        .literal .Lsec_85_8, 0xffff00ff
        .literal .Lsec_85_c, 0xffefffff
        .literal .Lsec_85_10, 0x3ff4e150
        .literal .Lsec_85_14, 0xfffdffff
        .literal .Lsec_85_18, esp_dport_access_reg_read
        .literal .Lsec_85_1c, esp_dport_access_reg_read
        .literal .Lsec_85_20, esp_dport_access_reg_read
        .literal .Lsec_85_24, esp_dport_access_reg_read
        .literal .Lsec_85_28, esp_dport_access_reg_read
        .literal .Lsec_85_2c, esp_dport_access_reg_read
.Lsec_85_30:
        .global bt_i2c_read_set
        .type   bt_i2c_read_set, @function
bt_i2c_read_set:
        entry    a1, 48
        mov.n    a8, a7
        l32r     a7, .Lsec_85_0
        extui    a8, a8, 0, 8
        mov.n    a10, a7
        extui    a2, a2, 0, 8
        extui    a2, a2, 0, 4
        s32i     a8, a1, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_85_18
        callx8   a8
        slli     a2, a2, 16
        extui    a3, a3, 0, 8
        extui    a4, a4, 0, 8
        l32r     a9, .Lsec_85_4
        slli     a4, a4, 8
        and      a10, a10, a9
        or       a10, a2, a10
        extui    a6, a6, 0, 8
        memw
        s32i.n   a10, a7, 0
        mov.n    a10, a7
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_85_1c
        callx8   a8
        movi     a11, 0xffffff00
        and      a10, a10, a11
        or       a10, a10, a3
        memw
        s32i.n   a10, a7, 0
        mov.n    a10, a7
        extui    a5, a5, 0, 8
        s32i     a11, a1, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_85_20
        callx8   a8
        l32r     a2, .Lsec_85_8
        and      a10, a10, a2
        or       a4, a10, a4
        or       a10, a7, a7
        memw
        s32i     a4, a7, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_85_24
        callx8   a8
        extui    a9, a6, 0, 1
        slli     a9, a9, 20
        l32r     a2, .Lsec_85_c
        and      a10, a10, a2
        or       a9, a9, a10
        l32r     a2, .Lsec_85_10
        mov.n    a10, a2
        memw
        s32i.n   a9, a7, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_85_28
        callx8   a8
        l32i.n   a8, a1, 4
        extui    a8, a8, 0, 1
        slli     a8, a8, 17
        l32r     a9, .Lsec_85_14
        and      a10, a10, a9
        or       a8, a8, a10
        mov.n    a10, a2
        memw
        s32i.n   a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_85_2c
        callx8   a8
        l32i.n   a11, a1, 0
        and      a10, a10, a11
        or       a5, a10, a5
        memw
        s32i.n   a5, a2, 0
        retw.n
        .size   bt_i2c_read_set, .-bt_i2c_read_set
.Lsec_85_f1:

        .section .text.bt_i2c_read_mem,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_86_0, .Lsec_58_48
        .literal .Lsec_86_4, 0x3ff4e0c4
        .literal .Lsec_86_8, 0x3ff4e0c0
        .literal .Lsec_86_c, .Lsec_58_50
        .literal .Lsec_86_10, .Lsec_58_58
        .literal .Lsec_86_14, phy_printf
        .literal .Lsec_86_18, esp_dport_access_reg_read
        .literal .Lsec_86_1c, esp_dport_access_reg_read
        .literal .Lsec_86_20, phy_printf
        .literal .Lsec_86_24, phy_printf
        .literal .Lsec_86_28, esp_dport_access_reg_read
        .literal .Lsec_86_2c, esp_dport_access_reg_read
        .literal .Lsec_86_30, phy_printf
.Lsec_86_34:
        .global bt_i2c_read_mem
        .type   bt_i2c_read_mem, @function
bt_i2c_read_mem:
        entry    a1, 32
        l32r     a10, .Lsec_86_0
        movi.n   a11, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_86_14
        callx8   a8
        movi.n   a7, 1
        movi     a6, 0xffffff00
        l32r     a3, .Lsec_86_4
        l32r     a5, .Lsec_86_8
        j        .Lsec_86_57
        .byte    0x00
        .byte    0x00
.Lsec_86_52:
        addi.n   a7, a7, 1
        extui    a7, a7, 0, 8
.Lsec_86_57:
        addi.n   a2, a7, -1
        extui    a2, a2, 0, 8
        movi.n   a4, 0
        mov.n    a10, a3
        beq      a2, a4, .Lsec_86_68
        beqi     a2, 1, .Lsec_86_9a
        movi.n   a4, 2
.Lsec_86_68:
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_86_18
        callx8   a8
        and      a10, a10, a6
        or       a8, a10, a4
        memw
        s32i.n   a8, a3, 0
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_86_1c
        callx8   a8
        mov.n    a11, a10
        l32r     a10, .Lsec_86_c
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_86_20
        callx8   a8
        bnei     a7, 3, .Lsec_86_52
        l32r     a10, .Lsec_86_10
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_86_24
        callx8   a8
        retw.n
.Lsec_86_9a:
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_86_28
        callx8   a8
        and      a10, a10, a6
        or       a2, a10, a2
        memw
        s32i.n   a2, a3, 0
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_86_2c
        callx8   a8
        mov.n    a11, a10
        l32r     a10, .Lsec_86_c
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_86_30
        callx8   a8
        j        .Lsec_86_52
        .size   bt_i2c_read_mem, .-bt_i2c_read_mem
.Lsec_86_c3:

        .section .text.bt_i2c_write_set,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_87_0, 0x3ff4e0c4
        .literal .Lsec_87_4, 0xffff83ff
        .literal .Lsec_87_8, 0x3ff4e100
        .literal .Lsec_87_c, 0x3ff4e104
        .literal .Lsec_87_10, 0x3ff4e108
        .literal .Lsec_87_14, .Lsec_88_0
        .literal .Lsec_87_18, 0x3ff4e0d8
        .literal .Lsec_87_1c, 0x0000ffff
        .literal .Lsec_87_20, 0x3ff4e0dc
        .literal .Lsec_87_24, 0x3ff4e0e0
        .literal .Lsec_87_28, 0x3ff4e0e4
        .literal .Lsec_87_2c, 0x3ff4e0e8
        .literal .Lsec_87_30, 0x3ff4e0ec
        .literal .Lsec_87_34, 0x3ff4e0f0
        .literal .Lsec_87_38, 0x3ff4e0f4
        .literal .Lsec_87_3c, 0x3ff4e10c
        .literal .Lsec_87_40, 0x3ff4e110
        .literal .Lsec_87_44, 0x3ff4e128
        .literal .Lsec_87_48, 0x3ff4e12c
        .literal .Lsec_87_4c, 0x3ff4e0d0
        .literal .Lsec_87_50, 0x3ff4e11c
        .literal .Lsec_87_54, 0x3ff4e0d4
        .literal .Lsec_87_58, 0x3ff4e120
        .literal .Lsec_87_5c, 0x3ff4e124
        .literal .Lsec_87_60, 0x3ff4e0c8
        .literal .Lsec_87_64, 0x3ff4e0cc
        .literal .Lsec_87_68, 0x3ff4e114
        .literal .Lsec_87_6c, 0x3ff4e118
        .literal .Lsec_87_70, 0x3ff4e164
        .literal .Lsec_87_74, esp_dport_access_reg_read
        .literal .Lsec_87_78, esp_dport_access_reg_read
        .literal .Lsec_87_7c, esp_dport_access_reg_read
        .literal .Lsec_87_80, esp_dport_access_reg_read
        .literal .Lsec_87_84, esp_dport_access_reg_read
        .literal .Lsec_87_88, esp_dport_access_reg_read
        .literal .Lsec_87_8c, esp_dport_access_reg_read
        .literal .Lsec_87_90, esp_dport_access_reg_read
        .literal .Lsec_87_94, esp_dport_access_reg_read
        .literal .Lsec_87_98, esp_dport_access_reg_read
        .literal .Lsec_87_9c, esp_dport_access_reg_read
        .literal .Lsec_87_a0, esp_dport_access_reg_read
        .literal .Lsec_87_a4, esp_dport_access_reg_read
        .literal .Lsec_87_a8, esp_dport_access_reg_read
        .literal .Lsec_87_ac, esp_dport_access_reg_read
        .literal .Lsec_87_b0, esp_dport_access_reg_read
        .literal .Lsec_87_b4, esp_dport_access_reg_read
        .literal .Lsec_87_b8, esp_dport_access_reg_read
        .literal .Lsec_87_bc, esp_dport_access_reg_read
.Lsec_87_c0:
        .global bt_i2c_write_set
        .type   bt_i2c_write_set, @function
bt_i2c_write_set:
        entry    a1, 64
        s32i.n   a7, a1, 12
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a7, .Lsec_87_0
        mov.n    a10, a7
        s32i.n   a3, a1, 16
        nop.n
        nop.n
        nop.n
        nop.n
        l8ui     a3, a1, 68
        s32i.n   a4, a1, 20
        extui    a4, a3, 0, 5
        slli     a4, a4, 10
        s32i.n   a3, a1, 0
        s32i.n   a5, a1, 8
        s32i     a6, a1, 24
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_87_74
        callx8   a8
        l32r     a3, .Lsec_87_4
        and      a10, a10, a3
        or       a10, a4, a10
        l32i.n   a4, a1, 0
        memw
        s32i.n   a10, a7, 0
        beqz     a4, .Lsec_87_624
        movi.n   a3, 0
        mov.n    a6, a3
        nop.n
        nop.n
        l32i     a4, a1, 72
        l32i.n   a7, a1, 0
.Lsec_87_114:
        l8ui     a5, a4, 0
        bnei     a5, 1, .Lsec_87_122
        ssl      a3
        sll      a5, a5
        add.n    a6, a6, a5
.Lsec_87_122:
        addi.n   a3, a3, 1
        extui    a5, a3, 0, 8
        addi.n   a4, a4, 1
        bltu     a5, a7, .Lsec_87_114
        j        .Lsec_87_602
        .byte    0x00
.Lsec_87_130:
        extui    a3, a7, 0, 8
        srli     a2, a3, 3
        beqz.n   a2, .Lsec_87_140
        beqi     a2, 1, .Lsec_87_174
        j        .Lsec_87_1a8
        .byte    0x00
        .byte    0x00
.Lsec_87_140:
        l32r     a10, .Lsec_87_8
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_87_78
        callx8   a8
        slli     a9, a7, 2
        ssl      a9
        sll      a3, a5
        l8ui     a2, a6, 0
        xor      a3, a4, a3
        extui    a2, a2, 0, 4
        and      a10, a3, a10
        ssl      a9
        sll      a2, a2
        l32r     a8, .Lsec_87_8
        or       a2, a10, a2
        memw
        s32i.n   a2, a8, 0
        j        .Lsec_87_1da
        .byte    0x00
        .byte    0x00
.Lsec_87_174:
        l32r     a2, .Lsec_87_c
        extui    a3, a3, 0, 3
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_87_7c
        callx8   a8
        slli     a3, a3, 2
        l8ui     a9, a6, 0
        ssl      a3
        sll      a11, a5
        xor      a11, a4, a11
        extui    a9, a9, 0, 4
        and      a10, a11, a10
        ssl      a3
        sll      a3, a9
        or       a3, a10, a3
        memw
        s32i.n   a3, a2, 0
        j        .Lsec_87_1da
.Lsec_87_1a8:
        l32r     a10, .Lsec_87_10
        extui    a3, a3, 0, 3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_87_80
        callx8   a8
        slli     a3, a3, 2
        ssl      a3
        sll      a9, a5
        l8ui     a2, a6, 0
        xor      a9, a4, a9
        extui    a2, a2, 0, 4
        and      a10, a9, a10
        ssl      a3
        sll      a3, a2
        l32r     a9, .Lsec_87_10
        or       a3, a10, a3
        memw
        s32i.n   a3, a9, 0
.Lsec_87_1da:
        addi.n   a7, a7, 1
        addi.n   a6, a6, 1
        nop.n
        nop.n
        l32i.n   a11, a1, 4
        beq      a11, a7, .Lsec_87_1ea
        j        .Lsec_87_130
.Lsec_87_1ea:
        l32i.n   a6, a1, 16
        movi.n   a7, 0
        mov.n    a3, a6
        mov.n    a4, a7
        movi.n   a2, 8
        l32i.n   a5, a1, 20
        or       a6, a5, a5
        or       a5, a3, a3
.Lsec_87_1fc:
        srli     a3, a4, 1
        bgeu     a2, a3, .Lsec_87_205
        j        .Lsec_87_2b8
.Lsec_87_205:
        l32r     a8, .Lsec_87_14
        addx4    a3, a3, a8
        l32i.n   a3, a3, 0
        jx       a3
        .byte    0x00
.Lsec_87_211:
        l32r     a3, .Lsec_87_18
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_87_84
        callx8   a8
        slli     a12, a7, 4
        l32r     a9, .Lsec_87_1c
        movi.n   a8, -1
        ssl      a12
        sll      a4, a9
        xor      a4, a8, a4
        and      a10, a4, a10
        l8ui     a11, a6, 0
        slli     a11, a11, 8
        l8ui     a9, a5, 0
        or       a4, a11, a9
        ssl      a12
        sll      a4, a4
        or       a10, a4, a10
        memw
        s32i.n   a10, a3, 0
        j        .Lsec_87_2f5
        .byte    0x00
        .byte    0x00
.Lsec_87_24f:
        l32r     a3, .Lsec_87_20
        j        .Lsec_87_258
.Lsec_87_255:
        l32r     a3, .Lsec_87_24
.Lsec_87_258:
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_87_88
        callx8   a8
        extui    a4, a4, 0, 1
        slli     a4, a4, 4
        l8ui     a11, a6, 0
        movi.n   a8, -1
        slli     a12, a11, 8
        l32r     a11, .Lsec_87_1c
        ssl      a4
        sll      a9, a11
        xor      a9, a8, a9
        and      a10, a9, a10
        l8ui     a11, a5, 0
        or       a11, a12, a11
        ssl      a4
        sll      a9, a11
        or       a10, a9, a10
        memw
        s32i.n   a10, a3, 0
        j        .Lsec_87_2f5
.Lsec_87_294:
        l32r     a3, .Lsec_87_28
        j        .Lsec_87_258
.Lsec_87_29a:
        l32r     a3, .Lsec_87_2c
        j        .Lsec_87_258
.Lsec_87_2a0:
        l32r     a3, .Lsec_87_30
        j        .Lsec_87_258
.Lsec_87_2a6:
        l32r     a3, .Lsec_87_34
        j        .Lsec_87_258
.Lsec_87_2ac:
        l32r     a3, .Lsec_87_38
        j        .Lsec_87_258
.Lsec_87_2b2:
        l32r     a3, .Lsec_87_3c
        j        .Lsec_87_258
.Lsec_87_2b8:
        l32r     a10, .Lsec_87_40
        extui    a4, a4, 0, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_87_8c
        callx8   a8
        slli     a11, a4, 4
        movi.n   a8, -1
        l8ui     a9, a6, 0
        slli     a4, a9, 8
        l32r     a9, .Lsec_87_1c
        ssl      a11
        sll      a3, a9
        xor      a3, a8, a3
        and      a10, a3, a10
        l8ui     a9, a5, 0
        or       a9, a4, a9
        ssl      a11
        sll      a4, a9
        or       a10, a4, a10
        l32r     a9, .Lsec_87_40
        memw
        s32i.n   a10, a9, 0
.Lsec_87_2f5:
        addi.n   a7, a7, 1
        extui    a4, a7, 0, 8
        addi.n   a6, a6, 1
        addi.n   a5, a5, 1
        l32i.n   a11, a1, 0
        bgeu     a4, a11, .Lsec_87_306
        j        .Lsec_87_1fc
.Lsec_87_306:
        movi.n   a2, 0
        l32i.n   a3, a1, 12
        mov.n    a5, a2
        mov.n    a7, a11
        l32i.n   a4, a1, 8
.Lsec_87_310:
        l32r     a10, .Lsec_87_44
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_87_90
        callx8   a8
        movi.n   a8, 1
        ssl      a2
        sll      a6, a8
        l8ui     a8, a3, 0
        movi.n   a9, -1
        xor      a6, a9, a6
        extui    a8, a8, 4, 1
        and      a10, a10, a6
        l32r     a11, .Lsec_87_44
        ssl      a2
        sll      a8, a8
        or       a8, a8, a10
        l32r     a10, .Lsec_87_48
        memw
        s32i.n   a8, a11, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_87_94
        callx8   a8
        and      a6, a6, a10
        srli     a8, a5, 3
        l8ui     a10, a4, 0
        extui    a10, a10, 4, 1
        ssl      a2
        sll      a10, a10
        or       a10, a10, a6
        l32r     a6, .Lsec_87_48
        memw
        s32i.n   a10, a6, 0
        beqz.n   a8, .Lsec_87_36e
        beqi     a8, 1, .Lsec_87_3c9
        j        .Lsec_87_427
.Lsec_87_36e:
        l32r     a10, .Lsec_87_4c
        slli     a6, a2, 2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_87_98
        callx8   a8
        movi.n   a9, 15
        movi     a11, -1
        l8ui     a8, a3, 0
        ssl      a6
        sll      a5, a9
        xor      a5, a11, a5
        and      a8, a8, a9
        and      a10, a10, a5
        l32r     a9, .Lsec_87_4c
        ssl      a6
        sll      a8, a8
        or       a8, a8, a10
        l32r     a10, .Lsec_87_50
        memw
        s32i     a8, a9, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_87_9c
        callx8   a8
        and      a10, a5, a10
        l8ui     a5, a4, 0
        extui    a5, a5, 0, 4
        ssl      a6
        sll      a5, a5
        or       a10, a5, a10
        l32r     a11, .Lsec_87_50
        memw
        s32i.n   a10, a11, 0
        j        .Lsec_87_48c
.Lsec_87_3c9:
        l32r     a10, .Lsec_87_54
        movi.n   a6, 15
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_87_a0
        callx8   a8
        extui    a8, a5, 0, 3
        slli     a5, a8, 2
        l8ui     a11, a3, 0
        movi.n   a9, -1
        ssl      a5
        sll      a8, a6
        xor      a6, a9, a8
        extui    a11, a11, 0, 4
        and      a10, a10, a6
        l32r     a8, .Lsec_87_54
        ssl      a5
        sll      a11, a11
        or       a11, a11, a10
        l32r     a10, .Lsec_87_58
        memw
        s32i.n   a11, a8, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_87_a4
        callx8   a8
        and      a10, a6, a10
        l8ui     a6, a4, 0
        extui    a6, a6, 0, 4
        ssl      a5
        sll      a8, a6
        or       a10, a8, a10
        l32r     a9, .Lsec_87_58
        memw
        s32i.n   a10, a9, 0
        j        .Lsec_87_48c
        .byte    0x00
        .byte    0x00
.Lsec_87_427:
        l32r     a6, .Lsec_87_5c
        extui    a5, a5, 0, 3
        mov.n    a10, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_87_a8
        callx8   a8
        slli     a5, a5, 2
        l8ui     a8, a3, 0
        movi.n   a11, 15
        ssl      a5
        sll      a9, a11
        movi.n   a11, -1
        xor      a9, a11, a9
        extui    a8, a8, 0, 4
        and      a10, a9, a10
        ssl      a5
        sll      a8, a8
        or       a8, a10, a8
        memw
        s32i.n   a8, a6, 0
        or       a10, a6, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_87_ac
        callx8   a8
        addi     a5, a5, 16
        movi.n   a8, 15
        ssl      a5
        sll      a9, a8
        l8ui     a8, a4, 0
        movi.n   a11, -1
        xor      a9, a11, a9
        extui    a8, a8, 0, 4
        and      a10, a9, a10
        ssl      a5
        sll      a5, a8
        or       a5, a10, a5
        memw
        s32i.n   a5, a6, 0
.Lsec_87_48c:
        addi.n   a2, a2, 1
        extui    a5, a2, 0, 8
        addi.n   a3, a3, 1
        addi.n   a4, a4, 1
        bgeu     a5, a7, .Lsec_87_49b
        j        .Lsec_87_310
.Lsec_87_49b:
        l32i.n   a5, a1, 12
        movi.n   a2, 0
        mov.n    a8, a5
        l32i.n   a4, a1, 8
        mov.n    a5, a4
        mov.n    a4, a8
        l32i.n   a6, a1, 24
        l32i     a3, a1, 64
.Lsec_87_4ac:
        l8ui     a7, a4, 0
        srli     a7, a7, 2
        beqi     a7, 1, .Lsec_87_504
        beqz.n   a7, .Lsec_87_4c8
        bnei     a7, 2, .Lsec_87_4bd
        j        .Lsec_87_540
.Lsec_87_4bd:
        bnei     a7, 3, .Lsec_87_4c3
        j        .Lsec_87_549
.Lsec_87_4c3:
        j        .Lsec_87_57f
        .byte    0x00
        .byte    0x00
.Lsec_87_4c8:
        l32r     a10, .Lsec_87_60
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_87_b0
        callx8   a8
        movi     a8, 255
        l8ui     a7, a4, 0
        extui    a7, a7, 0, 2
        slli     a7, a7, 3
        ssl      a7
        sll      a11, a8
        l8ui     a9, a3, 0
        movi.n   a8, -1
        xor      a11, a8, a11
        ssl      a7
        sll      a7, a9
        and      a10, a11, a10
        l32r     a9, .Lsec_87_60
        or       a7, a10, a7
        memw
        s32i.n   a7, a9, 0
        j        .Lsec_87_57f
        .byte    0x00
        .byte    0x00
.Lsec_87_504:
        l32r     a10, .Lsec_87_64
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_87_b4
        callx8   a8
        movi     a8, 255
        l8ui     a7, a4, 0
        extui    a7, a7, 0, 2
        slli     a7, a7, 3
        ssl      a7
        sll      a11, a8
        l8ui     a9, a3, 0
        movi.n   a8, -1
        xor      a11, a8, a11
        ssl      a7
        sll      a7, a9
        and      a10, a11, a10
        l32r     a9, .Lsec_87_64
        or       a7, a10, a7
        memw
        s32i.n   a7, a9, 0
        j        .Lsec_87_57f
        .byte    0x00
        .byte    0x00
.Lsec_87_540:
        l32r     a7, .Lsec_87_68
        j        .Lsec_87_54c
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_87_549:
        l32r     a7, .Lsec_87_6c
.Lsec_87_54c:
        mov.n    a10, a7
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_87_b8
        callx8   a8
        movi     a11, 255
        movi.n   a8, -1
        l8ui     a9, a4, 0
        extui    a9, a9, 0, 2
        slli     a9, a9, 3
        ssl      a9
        sll      a12, a11
        l8ui     a11, a3, 0
        xor      a12, a8, a12
        and      a10, a12, a10
        ssl      a9
        sll      a9, a11
        or       a9, a10, a9
        memw
        s32i.n   a9, a7, 0
.Lsec_87_57f:
        nop.n
        nop.n
        nop.n
        nop.n
        l8ui     a7, a5, 0
        srli     a7, a7, 2
        beqi     a7, 1, .Lsec_87_5a4
        beqz.n   a7, .Lsec_87_59c
        beqi     a7, 2, .Lsec_87_5ac
        beqi     a7, 3, .Lsec_87_5b5
        j        .Lsec_87_5eb
        .byte    0x00
.Lsec_87_59c:
        l32r     a7, .Lsec_87_60
        j        .Lsec_87_5b8
        .byte    0x00
        .byte    0x00
.Lsec_87_5a4:
        l32r     a7, .Lsec_87_64
        j        .Lsec_87_5b8
        .byte    0x00
        .byte    0x00
.Lsec_87_5ac:
        l32r     a7, .Lsec_87_68
        j        .Lsec_87_5b8
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_87_5b5:
        l32r     a7, .Lsec_87_6c
.Lsec_87_5b8:
        mov.n    a10, a7
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_87_bc
        callx8   a8
        movi     a11, 255
        movi.n   a8, -1
        l8ui     a9, a5, 0
        extui    a9, a9, 0, 2
        slli     a9, a9, 3
        ssl      a9
        sll      a12, a11
        l8ui     a11, a6, 0
        xor      a12, a8, a12
        and      a10, a12, a10
        ssl      a9
        sll      a9, a11
        or       a9, a10, a9
        memw
        s32i.n   a9, a7, 0
.Lsec_87_5eb:
        addi.n   a2, a2, 1
        extui    a2, a2, 0, 8
        addi.n   a4, a4, 1
        addi.n   a5, a5, 1
        l32i.n   a9, a1, 0
        addi.n   a6, a6, 1
        addi.n   a3, a3, 1
        bgeu     a2, a9, .Lsec_87_600
        j        .Lsec_87_4ac
.Lsec_87_600:
        retw.n
.Lsec_87_602:
        l32i.n   a11, a1, 0
        mov.n    a7, a2
        addi.n   a3, a11, -1
        extui    a3, a3, 0, 8
        addi.n   a3, a3, 1
        l32r     a2, .Lsec_87_70
        movi.n   a5, 15
        movi.n   a4, -1
        s32i.n   a3, a1, 4
        memw
        s32i.n   a6, a2, 0
        movi.n   a2, 0
        mov.n    a6, a7
        mov.n    a7, a2
        j        .Lsec_87_130
.Lsec_87_624:
        l32r     a2, .Lsec_87_70
        l32i.n   a3, a1, 0
        memw
        s32i.n   a3, a2, 0
        retw.n
        .size   bt_i2c_write_set, .-bt_i2c_write_set
.Lsec_87_630:

        .section .rodata.bt_i2c_write_set,"a",@progbits
        .align  4
.Lsec_88_0:
        .word   .Lsec_87_211
        .word   .Lsec_87_24f
        .word   .Lsec_87_255
        .word   .Lsec_87_294
        .word   .Lsec_87_29a
        .word   .Lsec_87_2a0
        .word   .Lsec_87_2a6
        .word   .Lsec_87_2ac
        .word   .Lsec_87_2b2

        .section .text.bt_i2c_set_wifi_data,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_89_0, 0x3ff4e130
        .literal .Lsec_89_4, 0x3ff4e134
        .literal .Lsec_89_8, 0x3ff4e138
        .literal .Lsec_89_c, 0x3ff4e13c
        .literal .Lsec_89_10, 0x3ff4e140
        .literal .Lsec_89_14, esp_dport_access_reg_read
        .literal .Lsec_89_18, esp_dport_access_reg_read
        .literal .Lsec_89_1c, esp_dport_access_reg_read
.Lsec_89_20:
        .global bt_i2c_set_wifi_data
        .type   bt_i2c_set_wifi_data, @function
bt_i2c_set_wifi_data:
        entry    a1, 48
        extui    a3, a3, 0, 8
        beqz     a3, .Lsec_89_100
        addi.n   a3, a3, -1
        extui    a3, a3, 0, 8
        addi.n   a3, a3, 1
        mov.n    a7, a2
        s32i.n   a3, a1, 0
        movi.n   a2, 0
        movi     a5, 255
        movi     a4, -1
.Lsec_89_3c:
        extui    a3, a2, 0, 8
        srli     a6, a3, 2
        beqi     a6, 1, .Lsec_89_80
        beqz.n   a6, .Lsec_89_50
        beqi     a6, 2, .Lsec_89_b4
        beqi     a6, 3, .Lsec_89_bc
        j        .Lsec_89_c2
.Lsec_89_50:
        l32r     a10, .Lsec_89_0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_89_14
        callx8   a8
        slli     a12, a2, 3
        ssl      a12
        sll      a8, a5
        l8ui     a6, a7, 0
        xor      a8, a4, a8
        and      a10, a8, a10
        ssl      a12
        sll      a6, a6
        or       a6, a10, a6
        l32r     a3, .Lsec_89_0
        memw
        s32i.n   a6, a3, 0
        j        .Lsec_89_f0
        .byte    0x00
.Lsec_89_80:
        l32r     a10, .Lsec_89_4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_89_18
        callx8   a8
        extui    a8, a3, 0, 2
        slli     a8, a8, 3
        l8ui     a6, a7, 0
        ssl      a8
        sll      a12, a5
        xor      a12, a4, a12
        and      a10, a12, a10
        ssl      a8
        sll      a8, a6
        l32r     a3, .Lsec_89_4
        or       a8, a10, a8
        memw
        s32i.n   a8, a3, 0
        j        .Lsec_89_f0
        .byte    0x00
        .byte    0x00
.Lsec_89_b4:
        l32r     a6, .Lsec_89_8
        j        .Lsec_89_c5
        .byte    0x00
        .byte    0x00
.Lsec_89_bc:
        l32r     a6, .Lsec_89_c
        j        .Lsec_89_c5
.Lsec_89_c2:
        l32r     a6, .Lsec_89_10
.Lsec_89_c5:
        mov.n    a10, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_89_1c
        callx8   a8
        extui    a8, a3, 0, 2
        slli     a8, a8, 3
        l8ui     a12, a7, 0
        ssl      a8
        sll      a13, a5
        xor      a13, a4, a13
        and      a10, a13, a10
        ssl      a8
        sll      a8, a12
        or       a8, a10, a8
        memw
        s32i.n   a8, a6, 0
.Lsec_89_f0:
        addi.n   a2, a2, 1
        addi.n   a7, a7, 1
        nop.n
        nop.n
        l32i.n   a3, a1, 0
        beq      a2, a3, .Lsec_89_100
        j        .Lsec_89_3c
.Lsec_89_100:
        retw.n
        .size   bt_i2c_set_wifi_data, .-bt_i2c_set_wifi_data
.Lsec_89_102:

        .section .text.phy_wifi_pll_track,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_90_0, phy_wifi_pll_track_en
.Lsec_90_4:
        .global phy_wifi_pll_track
        .type   phy_wifi_pll_track, @function
phy_wifi_pll_track:
        entry    a1, 32
        l32r     a8, .Lsec_90_0
        s8i      a2, a8, 0
        memw
        retw.n
        .size   phy_wifi_pll_track, .-phy_wifi_pll_track
.Lsec_90_12:

        .section .text.phy_bt_pll_track,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_91_0, phy_bt_pll_track_en
        .literal .Lsec_91_4, phy_bt_ifs_set
.Lsec_91_8:
        .global phy_bt_pll_track
        .type   phy_bt_pll_track, @function
phy_bt_pll_track:
        entry    a1, 32
        l32r     a8, .Lsec_91_0
        s8i      a2, a8, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_bt_ifs_set
        l32r     a8, .Lsec_91_4
        callx8   a8
        retw.n
        .size   phy_bt_pll_track, .-phy_bt_pll_track
.Lsec_91_1c:

        .section .text.tsens_read_init,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_92_0, 0x3ff4884c
        .literal .Lsec_92_4, 0xff00ffff
        .literal .Lsec_92_8, 0x00020000
        .literal .Lsec_92_c, 0x02000000
        .literal .Lsec_92_10, 0xfeffffff
        .literal .Lsec_92_14, 0x01000000
        .literal .Lsec_92_18, esp_dport_access_reg_read
        .literal .Lsec_92_1c, esp_dport_access_reg_read
        .literal .Lsec_92_20, esp_dport_access_reg_read
        .literal .Lsec_92_24, esp_dport_access_reg_read
.Lsec_92_28:
        .global tsens_read_init
        .type   tsens_read_init, @function
tsens_read_init:
        entry    a1, 32
        l32r     a2, .Lsec_92_0
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_92_18
        callx8   a8
        l32r     a8, .Lsec_92_4
        and      a9, a10, a8
        or       a10, a2, a2
        l32r     a8, .Lsec_92_8
        or       a8, a9, a8
        memw
        s32i     a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_92_1c
        callx8   a8
        l32r     a8, .Lsec_92_c
        or       a8, a10, a8
        mov.n    a10, a2
        memw
        s32i     a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_92_20
        callx8   a8
        l32r     a8, .Lsec_92_10
        and      a8, a10, a8
        mov.n    a10, a2
        memw
        s32i.n   a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_92_24
        callx8   a8
        l32r     a8, .Lsec_92_14
        or       a10, a10, a8
        memw
        s32i.n   a10, a2, 0
        retw.n
        .size   tsens_read_init, .-tsens_read_init
.Lsec_92_85:

        .section .text.phy_bt_power_track,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_93_0, phy_bt_power_track_en
        .literal .Lsec_93_4, tsens_read_init
.Lsec_93_8:
        .global phy_bt_power_track
        .type   phy_bt_power_track, @function
phy_bt_power_track:
        entry    a1, 32
        l32r     a8, .Lsec_93_0
        s8i      a2, a8, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, tsens_read_init
        l32r     a8, .Lsec_93_4
        callx8   a8
        retw.n
        .size   phy_bt_power_track, .-phy_bt_power_track
.Lsec_93_1c:

        .section .text.bt_get_i2c_data,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_94_0, g_phyFuns
        .literal .Lsec_94_4, .Lsec_95_0
.Lsec_94_8:
        .global bt_get_i2c_data
        .type   bt_get_i2c_data, @function
bt_get_i2c_data:
        entry    a1, 48
        l32r     a8, .Lsec_94_0
        movi.n   a15, 1
        movi.n   a14, 7
        mov.n    a11, a15
        mov.n    a13, a14
        s32i.n   a7, a1, 0
        movi.n   a12, 0
        movi     a10, 98
        nop.n
        nop.n
        l32i.n   a7, a8, 0
        l32i     a9, a7, 168
        s32i.n   a8, a1, 8
        nop.n
        nop.n
        nop.n
        nop.n
        l8ui     a7, a1, 52
        callx8   a9
        movi.n   a11, 1
        movi.n   a12, 0
        movi     a10, 98
        l32i.n   a8, a1, 8
        l32i     a8, a8, 0
        l32i     a8, a8, 152
        callx8   a8
        movi     a8, 159
        movi.n   a11, 0
        and      a10, a10, a8
        bne      a7, a11, .Lsec_94_56
        j        .Lsec_94_44e
.Lsec_94_56:
        movi.n   a8, 32
        or       a8, a10, a8
        mov.n    a9, a11
        s32i     a8, a1, 4
.Lsec_94_60:
        movi.n   a12, 10
        nop.n
        nop.n
        nop.n
        l32i.n   a8, a1, 56
        add.n    a10, a8, a11
        mov.n    a8, a11
        s8i      a9, a10, 0
        memw
        bgeu     a12, a11, .Lsec_94_7a
        j        .Lsec_94_3a5
.Lsec_94_7a:
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a13, .Lsec_94_4
        addx4    a12, a11, a13
        l32i.n   a12, a12, 0
        jx       a12
.Lsec_94_8d:
        add.n    a8, a2, a11
        l32i.n   a15, a1, 48
        add.n    a12, a3, a11
        movi     a13, 99
        add.n    a14, a4, a11
        s8i      a9, a8, 0
        movi.n   a8, 15
        s8i      a13, a12, 0
        memw
        add.n    a12, a15, a11
        add.n    a13, a6, a11
        nop.n
        nop.n
        l32i.n   a15, a1, 0
        s32i.n   a14, a1, 12
        add.n    a14, a5, a11
        add.n    a11, a15, a11
        nop.n
        nop.n
        l32i.n   a15, a1, 12
        s8i      a9, a15, 0
        s8i      a8, a14, 0
        s8i      a9, a13, 0
        s8i      a9, a12, 0
        s8i      a8, a11, 0
        movi.n   a8, 1
        s8i      a8, a10, 0
        memw
        bltui    a7, 2, .Lsec_94_d6
        j        .Lsec_94_3c0
.Lsec_94_d6:
        retw.n
.Lsec_94_d8:
        add.n    a10, a2, a8
        movi.n   a11, 1
        l32i.n   a15, a1, 0
        add.n    a12, a3, a8
        movi     a13, 98
        add.n    a14, a5, a8
        s8i      a11, a10, 0
        add.n    a10, a4, a8
        s8i      a13, a12, 0
        memw
        add.n    a12, a15, a8
        add.n    a13, a6, a8
        nop.n
        nop.n
        l32i.n   a15, a1, 48
        add.n    a8, a15, a8
        s32i.n   a10, a1, 12
        movi.n   a10, 16
        nop.n
        nop.n
        nop.n
        l32i.n   a15, a1, 12
        s8i      a11, a15, 0
        s8i      a10, a14, 0
        s8i      a9, a13, 0
        s8i      a10, a12, 0
        s8i      a9, a8, 0
        memw
        bltui    a7, 3, .Lsec_94_120
        j        .Lsec_94_3cd
.Lsec_94_120:
        retw.n
.Lsec_94_122:
        add.n    a10, a2, a8
        movi.n   a11, 1
        l32i.n   a15, a1, 0
        add.n    a14, a4, a8
        add.n    a13, a5, a8
        add.n    a12, a6, a8
        s8i      a11, a10, 0
        add.n    a10, a3, a8
        movi     a11, 98
        s8i      a11, a10, 0
        add.n    a11, a15, a8
        movi.n   a15, 2
        movi.n   a10, 17
        s8i      a15, a14, 0
        s8i      a10, a13, 0
        memw
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a14, a1, 48
        add.n    a8, a14, a8
        s8i      a9, a12, 0
        s8i      a10, a11, 0
        s8i      a9, a8, 0
        memw
        bltui    a7, 4, .Lsec_94_166
        j        .Lsec_94_3da
.Lsec_94_166:
        retw.n
.Lsec_94_168:
        add.n    a10, a2, a8
        s8i      a9, a10, 0
        movi     a11, 99
        add.n    a10, a3, a8
        add.n    a13, a4, a8
        add.n    a12, a5, a8
        s8i      a11, a10, 0
        memw
        add.n    a11, a6, a8
        nop.n
        nop.n
        nop.n
        l32i.n   a15, a1, 0
        add.n    a10, a15, a8
        movi.n   a15, 7
        s8i      a9, a13, 0
        s8i      a9, a12, 0
        memw
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a14, a1, 48
        add.n    a8, a14, a8
        s8i      a15, a11, 0
        s8i      a9, a10, 0
        s8i      a15, a8, 0
        memw
        bltui    a7, 5, .Lsec_94_1b1
        j        .Lsec_94_3e8
.Lsec_94_1b1:
        retw.n
.Lsec_94_1b3:
        add.n    a10, a2, a8
        l32i.n   a15, a1, 0
        movi     a11, 99
        add.n    a14, a4, a8
        add.n    a13, a5, a8
        add.n    a12, a6, a8
        s8i      a9, a10, 0
        add.n    a10, a3, a8
        s8i      a11, a10, 0
        add.n    a11, a15, a8
        movi.n   a15, 3
        movi.n   a10, 22
        s8i      a15, a14, 0
        s8i      a10, a13, 0
        memw
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a14, a1, 48
        add.n    a8, a14, a8
        s8i      a9, a12, 0
        s8i      a10, a11, 0
        s8i      a9, a8, 0
        memw
        bltui    a7, 6, .Lsec_94_1f5
        j        .Lsec_94_3f5
.Lsec_94_1f5:
        retw.n
.Lsec_94_1f7:
        add.n    a10, a2, a8
        l32i.n   a15, a1, 0
        movi     a11, 99
        add.n    a14, a4, a8
        add.n    a13, a5, a8
        add.n    a12, a6, a8
        s8i      a9, a10, 0
        add.n    a10, a3, a8
        s8i      a11, a10, 0
        add.n    a11, a15, a8
        movi.n   a15, 5
        movi.n   a10, 20
        s8i      a15, a14, 0
        s8i      a10, a13, 0
        memw
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a14, a1, 48
        add.n    a8, a14, a8
        s8i      a9, a12, 0
        s8i      a10, a11, 0
        s8i      a9, a8, 0
        memw
        bltui    a7, 7, .Lsec_94_239
        j        .Lsec_94_402
.Lsec_94_239:
        retw.n
.Lsec_94_23b:
        add.n    a10, a2, a8
        l32i.n   a15, a1, 0
        movi     a11, 99
        add.n    a14, a4, a8
        add.n    a13, a5, a8
        add.n    a12, a6, a8
        s8i      a9, a10, 0
        add.n    a10, a3, a8
        s8i      a11, a10, 0
        add.n    a11, a15, a8
        movi.n   a15, 4
        movi.n   a10, 21
        s8i      a15, a14, 0
        s8i      a10, a13, 0
        memw
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a14, a1, 48
        add.n    a8, a14, a8
        s8i      a9, a12, 0
        s8i      a10, a11, 0
        s8i      a9, a8, 0
        memw
        bltui    a7, 8, .Lsec_94_27d
        j        .Lsec_94_410
.Lsec_94_27d:
        retw.n
.Lsec_94_27f:
        add.n    a10, a2, a8
        s8i      a9, a10, 0
        movi     a12, 99
        add.n    a10, a3, a8
        add.n    a14, a4, a8
        add.n    a15, a5, a8
        s8i      a12, a10, 0
        movi.n   a11, 1
        movi.n   a10, 23
        add.n    a13, a6, a8
        s8i      a9, a14, 0
        s32i.n   a15, a1, 12
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a14, a1, 48
        l32i.n   a15, a1, 0
        add.n    a12, a15, a8
        add.n    a15, a14, a8
        movi.n   a8, 8
        l32i.n   a14, a1, 12
        s8i      a11, a14, 0
        s8i      a10, a13, 0
        s8i      a11, a12, 0
        s8i      a10, a15, 0
        memw
        bgeu     a8, a7, .Lsec_94_2c4
        j        .Lsec_94_41d
.Lsec_94_2c4:
        retw.n
.Lsec_94_2c6:
        add.n    a10, a2, a8
        movi.n   a11, 1
        l32i.n   a14, a1, 0
        movi     a12, 98
        add.n    a15, a4, a8
        add.n    a13, a5, a8
        s8i      a11, a10, 0
        add.n    a11, a3, a8
        movi.n   a10, 2
        s8i      a12, a11, 0
        memw
        add.n    a11, a14, a8
        add.n    a12, a6, a8
        nop.n
        nop.n
        l32i.n   a14, a1, 48
        s32i.n   a15, a1, 12
        add.n    a15, a14, a8
        nop.n
        nop.n
        nop.n
        l32i.n   a8, a1, 12
        s8i      a9, a8, 0
        movi.n   a8, 9
        s8i      a10, a13, 0
        memw
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a13, a1, 4
        s8i      a13, a12, 0
        s8i      a10, a11, 0
        s8i      a13, a15, 0
        memw
        bgeu     a8, a7, .Lsec_94_31d
        j        .Lsec_94_428
.Lsec_94_31d:
        retw.n
.Lsec_94_31f:
        add.n    a10, a2, a8
        s8i      a9, a10, 0
        add.n    a11, a3, a8
        movi     a15, 100
        add.n    a14, a4, a8
        add.n    a13, a5, a8
        s8i      a15, a11, 0
        memw
        add.n    a12, a6, a8
        movi.n   a10, 24
        nop.n
        nop.n
        l32i.n   a15, a1, 0
        add.n    a11, a15, a8
        l32i.n   a15, a1, 48
        add.n    a8, a15, a8
        movi.n   a15, 4
        s8i      a15, a14, 0
        s8i      a10, a13, 0
        s8i      a9, a12, 0
        s8i      a10, a11, 0
        s8i      a9, a8, 0
        memw
        movi.n   a8, 10
        bgeu     a8, a7, .Lsec_94_35f
        j        .Lsec_94_434
.Lsec_94_35f:
        retw.n
.Lsec_94_361:
        add.n    a10, a2, a8
        l32i.n   a15, a1, 0
        movi     a14, 100
        add.n    a13, a4, a8
        add.n    a12, a5, a8
        add.n    a11, a6, a8
        s8i      a9, a10, 0
        add.n    a10, a3, a8
        s8i      a14, a10, 0
        memw
        add.n    a10, a15, a8
        nop.n
        nop.n
        nop.n
        l32i.n   a15, a1, 48
        add.n    a14, a15, a8
        movi.n   a15, 7
        movi.n   a8, 11
        s8i      a15, a13, 0
        movi.n   a13, 25
        s8i      a13, a12, 0
        s8i      a9, a11, 0
        s8i      a13, a10, 0
        s8i      a9, a14, 0
        memw
        bgeu     a8, a7, .Lsec_94_3a3
        j        .Lsec_94_441
.Lsec_94_3a3:
        retw.n
.Lsec_94_3a5:
        add.n    a10, a2, a8
        s8i      a9, a10, 0
        add.n    a8, a3, a8
        addi.n   a11, a11, 1
        extui    a11, a11, 0, 8
        s8i      a9, a8, 0
        memw
        bgeu     a11, a7, .Lsec_94_3bd
        j        .Lsec_94_60
.Lsec_94_3bd:
        retw.n
        .byte    0x00
.Lsec_94_3c0:
        l32i.n   a8, a1, 56
        s8i      a9, a8, 1
        memw
        movi.n   a8, 1
        j        .Lsec_94_d8
.Lsec_94_3cd:
        l32i.n   a8, a1, 56
        s8i      a9, a8, 2
        memw
        movi.n   a8, 2
        j        .Lsec_94_122
.Lsec_94_3da:
        l32i.n   a8, a1, 56
        s8i      a9, a8, 3
        memw
        movi.n   a8, 3
        j        .Lsec_94_168
        .byte    0x00
.Lsec_94_3e8:
        l32i.n   a8, a1, 56
        s8i      a9, a8, 4
        memw
        movi.n   a8, 4
        j        .Lsec_94_1b3
.Lsec_94_3f5:
        l32i.n   a8, a1, 56
        s8i      a9, a8, 5
        memw
        movi.n   a8, 5
        j        .Lsec_94_1f7
.Lsec_94_402:
        l32i.n   a8, a1, 56
        s8i      a9, a8, 6
        memw
        movi.n   a8, 6
        j        .Lsec_94_23b
        .byte    0x00
.Lsec_94_410:
        l32i.n   a8, a1, 56
        s8i      a9, a8, 7
        memw
        movi.n   a8, 7
        j        .Lsec_94_27f
.Lsec_94_41d:
        l32i.n   a10, a1, 56
        s8i      a9, a10, 8
        memw
        j        .Lsec_94_2c6
.Lsec_94_428:
        l32i.n   a11, a1, 56
        s8i      a9, a11, 9
        memw
        j        .Lsec_94_31f
        .byte    0x00
.Lsec_94_434:
        l32i.n   a8, a1, 56
        s8i      a9, a8, 10
        memw
        movi.n   a8, 10
        j        .Lsec_94_361
.Lsec_94_441:
        l32i.n   a10, a1, 56
        mov.n    a11, a8
        s8i      a9, a10, 11
        memw
        j        .Lsec_94_3a5
.Lsec_94_44e:
        retw.n
        .size   bt_get_i2c_data, .-bt_get_i2c_data
.Lsec_94_450:

        .section .rodata.bt_get_i2c_data,"a",@progbits
        .align  4
.Lsec_95_0:
        .word   .Lsec_94_8d
        .word   .Lsec_94_d8
        .word   .Lsec_94_122
        .word   .Lsec_94_168
        .word   .Lsec_94_1b3
        .word   .Lsec_94_1f7
        .word   .Lsec_94_23b
        .word   .Lsec_94_27f
        .word   .Lsec_94_2c6
        .word   .Lsec_94_31f
        .word   .Lsec_94_361

        .section .text.write_wifi_chan_data,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_96_0, freq_i2c_addr
        .literal .Lsec_96_4, bt_wifi_chan_data
        .literal .Lsec_96_8, 0x3ff4e0c4
        .literal .Lsec_96_c, 0x3ff4e0c0
        .literal .Lsec_96_10, esp_dport_access_reg_read
        .literal .Lsec_96_14, esp_dport_access_reg_read
        .literal .Lsec_96_18, get_lna_vga_dcap_val
        .literal .Lsec_96_1c, bt_i2c_set_wifi_data
.Lsec_96_20:
        .global write_wifi_chan_data
        .type   write_wifi_chan_data, @function
write_wifi_chan_data:
        entry    a1, 48
        extui    a2, a2, 0, 8
        addx2    a2, a2, a2
        extui    a2, a2, 0, 8
        addi.n   a4, a2, 3
        l32r     a3, .Lsec_96_8
        or       a7, a1, a1
        extui    a4, a4, 0, 8
        movi     a6, 0xffffff00
        l32r     a5, .Lsec_96_c
.Lsec_96_3d:
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_96_10
        callx8   a8
        and      a10, a10, a6
        or       a10, a10, a2
        memw
        s32i.n   a10, a3, 0
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_96_14
        callx8   a8
        addi.n   a2, a2, 1
        extui    a2, a2, 0, 8
        s32i.n   a10, a7, 0
        addi.n   a7, a7, 4
        bne      a2, a4, .Lsec_96_3d
        addi.n   a11, a1, 12
        movi.n   a5, 23
        nop.n
        l32i.n   a2, a1, 0
        extui    a10, a2, 0, 8
        .reloc  ., R_XTENSA_ASM_EXPAND, get_lna_vga_dcap_val
        l32r     a8, .Lsec_96_18
        callx8   a8
        movi.n   a13, 15
        l32r     a15, .Lsec_96_4
        movi.n   a14, 19
        mov.n    a12, a15
        movi.n   a11, 11
        l32i     a4, a1, 4
        l32i     a3, a1, 8
        l32r     a10, .Lsec_96_0
        loop     a11, .Lsec_96_c5
        l8ui     a8, a10, 0
        bgeu     a13, a8, .Lsec_96_c1
        slli     a9, a8, 3
        ssr      a9
        srl      a9, a2
        bgeu     a14, a8, .Lsec_96_b2
        slli     a9, a8, 3
        mov.n    a6, a9
        ssr      a6
        srl      a6, a3
        ssr      a9
        srl      a9, a4
        bltu     a5, a8, .Lsec_96_b8
.Lsec_96_b2:
        extui    a9, a9, 0, 8
        j        .Lsec_96_bb
.Lsec_96_b8:
        extui    a9, a6, 0, 8
.Lsec_96_bb:
        s8i      a9, a12, 0
        memw
.Lsec_96_c1:
        addi.n   a10, a10, 1
        addi.n   a12, a12, 1
.Lsec_96_c5:
        nop.n
        nop.n
        l8ui     a2, a15, 9
        movi.n   a3, -16
        and      a4, a2, a3
        movi.n   a11, 11
        mov.n    a10, a15
        l8ui     a2, a1, 12
        or       a2, a4, a2
        extui    a2, a2, 0, 8
        and      a3, a2, a3
        l8ui     a4, a1, 14
        or       a3, a3, a4
        s8i      a2, a15, 9
        s8i      a3, a15, 10
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, bt_i2c_set_wifi_data
        l32r     a8, .Lsec_96_1c
        callx8   a8
        retw.n
        .size   write_wifi_chan_data, .-write_wifi_chan_data
.Lsec_96_f8:

        .section .text.set_chan_freq_hw_init,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_97_0, bt_wifi_chan_data
        .literal .Lsec_97_4, chip7_sleep_params
        .literal .Lsec_97_8, 0x00002000
        .literal .Lsec_97_c, freq_i2c_addr
        .literal .Lsec_97_10, 0x60033d38
        .literal .Lsec_97_14, 0x0c800000
        .literal .Lsec_97_18, .Lsec_83_28
        .literal .Lsec_97_1c, bt_get_i2c_data
        .literal .Lsec_97_20, bt_i2c_write_set
        .literal .Lsec_97_24, write_wifi_chan_data
        .literal .Lsec_97_28, esp_dport_access_reg_read
        .literal .Lsec_97_2c, i2cmst_reg_init
.Lsec_97_30:
        .global set_chan_freq_hw_init
        .type   set_chan_freq_hw_init, @function
set_chan_freq_hw_init:
        entry    a1, 128
        l32r     a4, .Lsec_97_4
        l32i     a2, a4, 0
        bbsi     a2, 4, .Lsec_97_44
        .reloc  ., R_XTENSA_ASM_EXPAND, .Lsec_83_28
        l32r     a8, .Lsec_97_18
        callx8   a8
        l32i.n   a2, a4, 0
.Lsec_97_44:
        l32r     a3, .Lsec_97_8
        bnone    a2, a3, .Lsec_97_4d
        j        .Lsec_97_d4
.Lsec_97_4d:
        movi.n   a2, 11
        addi     a6, a1, 16
        l32r     a7, .Lsec_97_c
        addi     a5, a1, 27
        addi     a15, a1, 49
        addi     a14, a1, 38
        mov.n    a13, a7
        addi     a12, a1, 60
        addi     a11, a1, 71
        addi     a10, a1, 82
        s32i.n   a2, a1, 4
        s32i.n   a6, a1, 8
        s32i.n   a5, a1, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, bt_get_i2c_data
        l32r     a8, .Lsec_97_1c
        callx8   a8
        addi     a15, a1, 49
        addi     a14, a1, 38
        mov.n    a13, a7
        s32i.n   a2, a1, 4
        addi     a12, a1, 60
        addi     a11, a1, 71
        addi     a10, a1, 82
        s32i.n   a6, a1, 8
        s32i.n   a5, a1, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, bt_i2c_write_set
        l32r     a8, .Lsec_97_20
        callx8   a8
        mov.n    a8, a2
        addi     a9, a1, 38
        l32r     a2, .Lsec_97_0
        loop     a8, .Lsec_97_aa
        l8ui     a10, a9, 0
        addi.n   a9, a9, 1
        s8i      a10, a2, 0
        memw
        addi.n   a2, a2, 1
.Lsec_97_aa:
        movi.n   a10, 12
        .reloc  ., R_XTENSA_ASM_EXPAND, write_wifi_chan_data
        l32r     a8, .Lsec_97_24
        callx8   a8
        l32r     a5, .Lsec_97_10
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_97_28
        callx8   a8
        extui    a10, a10, 0, 16
        l32r     a2, .Lsec_97_14
        or       a10, a10, a2
        l32i.n   a2, a4, 0
        or       a3, a2, a3
        memw
        s32i     a10, a5, 0
        s32i     a3, a4, 0
.Lsec_97_d4:
        .reloc  ., R_XTENSA_ASM_EXPAND, i2cmst_reg_init
        l32r     a8, .Lsec_97_2c
        callx8   a8
        retw.n
        .size   set_chan_freq_hw_init, .-set_chan_freq_hw_init
.Lsec_97_dc:

        .section .text.rf_init,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_98_0, re_entry
        .literal .Lsec_98_4, phy_init_flag
        .literal .Lsec_98_8, g_phyFuns
        .literal .Lsec_98_c, .Lsec_62_0
        .literal .Lsec_98_10, i2c_bias_init
        .literal .Lsec_98_14, reg_init_begin
        .literal .Lsec_98_18, rfpll_1p2_opt
        .literal .Lsec_98_1c, phy_i2c_init
        .literal .Lsec_98_20, set_chan_freq_hw_init
.Lsec_98_24:
        .global rf_init
        .type   rf_init, @function
rf_init:
        entry    a1, 48
        .reloc  ., R_XTENSA_ASM_EXPAND, i2c_bias_init
        l32r     a8, .Lsec_98_10
        callx8   a8
        movi.n   a10, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, reg_init_begin
        l32r     a8, .Lsec_98_14
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, rfpll_1p2_opt
        l32r     a8, .Lsec_98_18
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_i2c_init
        l32r     a8, .Lsec_98_1c
        callx8   a8
        l32r     a2, .Lsec_98_0
        l8ui     a2, a2, 0
        beqz.n   a2, .Lsec_98_52
        l32r     a2, .Lsec_98_4
        l8ui     a2, a2, 0
        bnez     a2, .Lsec_98_9e
.Lsec_98_52:
        l32r     a2, .Lsec_98_8
        l32i     a8, a2, 0
        l32i     a8, a8, 192
        callx8   a8
        mov.n    a10, a1
        l32r     a8, .Lsec_98_c
        l32i.n   a14, a2, 0
        l16ui    a13, a8, 0
        l16ui    a12, a8, 2
        l16ui    a11, a8, 4
        l16ui    a9, a8, 6
        l32i     a8, a14, 220
        s16i     a13, a1, 0
        s16i     a12, a1, 2
        s16i     a11, a1, 4
        s16i     a9, a1, 6
        memw
        callx8   a8
        l32i.n   a8, a2, 0
        l32i     a8, a8, 212
        callx8   a8
        l32i.n   a8, a2, 0
        l32i     a8, a8, 204
        callx8   a8
        l32i.n   a2, a2, 0
        l32i     a2, a2, 196
        callx8   a2
.Lsec_98_9e:
        movi.n   a11, 4
        movi.n   a10, 2
        .reloc  ., R_XTENSA_ASM_EXPAND, set_chan_freq_hw_init
        l32r     a8, .Lsec_98_20
        callx8   a8
        retw.n
        .size   rf_init, .-rf_init
.Lsec_98_aa:

        .section .text.check_rfpll_write_i2c,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_99_0, freq_i2c_addr
        .literal .Lsec_99_4, g_phyFuns
        .literal .Lsec_99_8, .Lsec_58_50
        .literal .Lsec_99_c, .Lsec_58_58
        .literal .Lsec_99_10, bt_get_i2c_data
        .literal .Lsec_99_14, phy_printf
        .literal .Lsec_99_18, phy_printf
.Lsec_99_1c:
        .global check_rfpll_write_i2c
        .type   check_rfpll_write_i2c, @function
check_rfpll_write_i2c:
        entry    a1, 128
        addi     a2, a1, 16
        s32i.n   a2, a1, 8
        movi.n   a2, 11
        addi     a5, a1, 82
        addi     a15, a1, 49
        addi     a14, a1, 38
        s32i.n   a2, a1, 4
        addi     a2, a1, 27
        addi     a12, a1, 60
        addi     a11, a1, 71
        mov.n    a10, a5
        l32r     a13, .Lsec_99_0
        mov.n    a4, a5
        addi     a3, a1, 60
        s32i.n   a2, a1, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, bt_get_i2c_data
        l32r     a8, .Lsec_99_10
        callx8   a8
        addi     a2, a1, 71
        l32r     a7, .Lsec_99_4
        l32r     a6, .Lsec_99_8
.Lsec_99_55:
        l32i.n   a8, a7, 0
        l8ui     a11, a4, 0
        addi.n   a4, a4, 1
        l8ui     a12, a3, 0
        addi     a3, a3, 1
        l8ui     a10, a2, 0
        addi     a2, a2, 1
        l32i     a8, a8, 152
        callx8   a8
        mov.n    a11, a10
        mov.n    a10, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_99_14
        callx8   a8
        bne      a5, a2, .Lsec_99_55
        l32r     a10, .Lsec_99_c
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_99_18
        callx8   a8
        retw.n
        .size   check_rfpll_write_i2c, .-check_rfpll_write_i2c
.Lsec_99_86:

        .section .text.phy_hw_set_freq_enable,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_100_0, .Lsec_113_0
        .literal .Lsec_100_4, 0x3ff4e168
        .literal .Lsec_100_8, 0x3ff4e0c4
        .literal .Lsec_100_c, 0x02000000
        .literal .Lsec_100_10, force_hw_set_freq_flag
        .literal .Lsec_100_14, 0xfdffffff
        .literal .Lsec_100_18, 0x3ff5c07c
        .literal .Lsec_100_1c, 0xff00ffff
        .literal .Lsec_100_20, 0x009c0000
        .literal .Lsec_100_24, esp_dport_access_reg_read
        .literal .Lsec_100_28, esp_dport_access_reg_read
        .literal .Lsec_100_2c, esp_dport_access_reg_read
        .literal .Lsec_100_30, ets_delay_us
        .literal .Lsec_100_34, esp_dport_access_reg_read
        .literal .Lsec_100_38, esp_dport_access_reg_read
        .literal .Lsec_100_3c, esp_dport_access_reg_read
.Lsec_100_40:
        .global phy_hw_set_freq_enable
        .type   phy_hw_set_freq_enable, @function
phy_hw_set_freq_enable:
        entry    a1, 32
        extui    a2, a2, 0, 8
        l32r     a5, .Lsec_100_0
        bnez     a2, .Lsec_100_ad
        l32i.n   a2, a5, 0
        bnez     a2, .Lsec_100_a7
        movi     a4, 0x100
        l32r     a3, .Lsec_100_4
        l32r     a2, .Lsec_100_8
.Lsec_100_5a:
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_100_24
        callx8   a8
        bltz     a10, .Lsec_100_99
.Lsec_100_65:
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_100_28
        callx8   a8
        l32r     a3, .Lsec_100_c
        or       a3, a10, a3
        mov.n    a10, a2
        memw
        s32i     a3, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_100_2c
        callx8   a8
        movi     a3, 0xfffffeff
        and      a3, a10, a3
        movi.n   a10, 2
        memw
        s32i.n   a3, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_100_30
        callx8   a8
        l32i.n   a2, a5, 0
        j        .Lsec_100_a7
.Lsec_100_99:
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_100_34
        callx8   a8
        bnone    a10, a4, .Lsec_100_5a
        j        .Lsec_100_65
.Lsec_100_a7:
        addi.n   a2, a2, 1
        s32i.n   a2, a5, 0
        retw.n
.Lsec_100_ad:
        l32i.n   a2, a5, 0
        bnei     a2, 1, .Lsec_100_f8
        l32r     a2, .Lsec_100_10
        l8ui     a2, a2, 0
        bnez.n   a2, .Lsec_100_d1
        l32r     a3, .Lsec_100_8
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_100_38
        callx8   a8
        l32r     a2, .Lsec_100_14
        and      a10, a10, a2
        memw
        s32i     a10, a3, 0
.Lsec_100_d1:
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a3, .Lsec_100_18
        or       a10, a3, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_100_3c
        callx8   a8
        l32r     a2, .Lsec_100_1c
        and      a10, a10, a2
        l32r     a2, .Lsec_100_20
        or       a10, a10, a2
        l32i.n   a2, a5, 0
        memw
        s32i.n   a10, a3, 0
.Lsec_100_f8:
        addi.n   a2, a2, -1
        s32i.n   a2, a5, 0
        retw.n
        .size   phy_hw_set_freq_enable, .-phy_hw_set_freq_enable
.Lsec_100_fe:

        .section .text.set_chan_freq_sw_start,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_101_0, phy_sw_set_chan_en
        .literal .Lsec_101_4, 0x3ff4e0c4
        .literal .Lsec_101_8, 0xe7ffffff
        .literal .Lsec_101_c, 0x10000000
        .literal .Lsec_101_10, 0xfff0ffff
        .literal .Lsec_101_14, 0xff0fffff
        .literal .Lsec_101_18, phy_force_wifi_chan_en
        .literal .Lsec_101_1c, esp_dport_access_reg_read
        .literal .Lsec_101_20, esp_dport_access_reg_read
        .literal .Lsec_101_24, phy_dis_hw_set_freq
        .literal .Lsec_101_28, esp_dport_access_reg_read
        .literal .Lsec_101_2c, esp_dport_access_reg_read
        .literal .Lsec_101_30, esp_dport_access_reg_read
        .literal .Lsec_101_34, esp_dport_access_reg_read
        .literal .Lsec_101_38, correct_rfpll_offset
        .literal .Lsec_101_3c, write_wifi_chan_data
        .literal .Lsec_101_40, esp_dport_access_reg_read
        .literal .Lsec_101_44, esp_dport_access_reg_read
        .literal .Lsec_101_48, ets_delay_us
        .literal .Lsec_101_4c, esp_dport_access_reg_read
        .literal .Lsec_101_50, esp_dport_access_reg_read
        .literal .Lsec_101_54, ets_delay_us
        .literal .Lsec_101_58, pll_correct_dcap
        .literal .Lsec_101_5c, esp_dport_access_reg_read
        .literal .Lsec_101_60, esp_dport_access_reg_read
        .literal .Lsec_101_64, esp_dport_access_reg_read
        .literal .Lsec_101_68, esp_dport_access_reg_read
        .literal .Lsec_101_6c, phy_en_hw_set_freq
.Lsec_101_70:
        .global set_chan_freq_sw_start
        .type   set_chan_freq_sw_start, @function
set_chan_freq_sw_start:
        entry    a1, 64
        l32r     a5, .Lsec_101_4
        movi     a6, 1
        or       a10, a5, a5
        extui    a4, a4, 0, 8
        sext     a3, a3, 15
        l32r     a8, .Lsec_101_0
        extui    a2, a2, 0, 8
        s8i      a6, a8, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_101_1c
        callx8   a8
        extui    a10, a10, 16, 4
        s32i.n   a10, a1, 16
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_101_20
        callx8   a8
        extui    a10, a10, 20, 4
        s32i.n   a10, a1, 20
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_dis_hw_set_freq
        l32r     a8, .Lsec_101_24
        callx8   a8
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_101_28
        callx8   a8
        movi     a8, 0xfffffeff
        and      a10, a10, a8
        memw
        s32i.n   a10, a5, 0
        mov.n    a10, a5
        s32i.n   a8, a1, 24
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_101_2c
        callx8   a8
        l32r     a9, .Lsec_101_8
        and      a7, a10, a9
        mov.n    a10, a5
        l32r     a6, .Lsec_101_c
        or       a6, a7, a6
        movi.n   a7, 0
        memw
        s32i     a6, a5, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_101_30
        callx8   a8
        l32r     a9, .Lsec_101_10
        and      a6, a10, a9
        mov.n    a10, a5
        memw
        s32i     a6, a5, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_101_34
        callx8   a8
        mov.n    a11, a4
        l32r     a6, .Lsec_101_14
        and      a4, a10, a6
        mov.n    a10, a3
        slli     a3, a2, 1
        memw
        s32i.n   a4, a5, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, correct_rfpll_offset
        l32r     a8, .Lsec_101_38
        callx8   a8
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, write_wifi_chan_data
        l32r     a8, .Lsec_101_3c
        callx8   a8
        mov.n    a10, a5
        extui    a4, a3, 0, 8
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_101_40
        callx8   a8
        movi     a3, 0xffffff00
        and      a10, a10, a3
        or       a3, a4, a10
        memw
        s32i.n   a3, a5, 0
        movi.n   a6, 12
        movi     a4, 0x100
        j        .Lsec_101_13c
.Lsec_101_13a:
        mov.n    a7, a10
.Lsec_101_13c:
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_101_44
        callx8   a8
        or       a10, a10, a4
        memw
        s32i.n   a10, a5, 0
        movi.n   a10, 2
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_101_48
        callx8   a8
        mov.n    a3, a5
.Lsec_101_156:
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_101_4c
        callx8   a8
        bgez     a10, .Lsec_101_190
.Lsec_101_161:
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_101_50
        callx8   a8
        movi     a8, 0xfffffeff
        and      a10, a10, a8
        memw
        s32i.n   a10, a5, 0
        movi.n   a10, 40
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_101_54
        callx8   a8
        movi.n   a12, 1
        mov.n    a11, a1
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, pll_correct_dcap
        l32r     a8, .Lsec_101_58
        callx8   a8
        extui    a10, a10, 0, 8
        bnez.n   a10, .Lsec_101_1a0
        j        .Lsec_101_1bd
.Lsec_101_190:
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_101_5c
        callx8   a8
        bany     a10, a4, .Lsec_101_156
        j        .Lsec_101_161
        .byte    0x00
        .byte    0x00
.Lsec_101_1a0:
        sext     a3, a10, 7
        bnei     a3, 1, .Lsec_101_1ac
        sext     a8, a7, 7
        beqi     a8, -1, .Lsec_101_1bd
.Lsec_101_1ac:
        bnei     a3, -1, .Lsec_101_1b5
        sext     a7, a7, 7
        beqi     a7, 1, .Lsec_101_1bd
.Lsec_101_1b5:
        addi.n   a6, a6, -1
        extui    a6, a6, 0, 8
        bnez     a6, .Lsec_101_13a
.Lsec_101_1bd:
        l32r     a9, .Lsec_101_0
        movi.n   a2, 0
        or       a10, a5, a5
        s8i      a2, a9, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_101_60
        callx8   a8
        l32r     a2, .Lsec_101_10
        and      a10, a10, a2
        l32i.n   a3, a1, 16
        slli     a2, a3, 16
        or       a2, a10, a2
        mov.n    a10, a5
        memw
        s32i     a2, a5, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_101_64
        callx8   a8
        l32i.n   a8, a1, 20
        slli     a2, a8, 20
        l32r     a6, .Lsec_101_14
        and      a10, a10, a6
        or       a2, a10, a2
        mov.n    a10, a5
        memw
        s32i.n   a2, a5, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_101_68
        callx8   a8
        l32r     a9, .Lsec_101_8
        and      a10, a10, a9
        l32r     a2, .Lsec_101_18
        memw
        s32i.n   a10, a5, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l8ui     a2, a2, 0
        bnez.n   a2, .Lsec_101_229
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_en_hw_set_freq
        l32r     a8, .Lsec_101_6c
        callx8   a8
.Lsec_101_229:
        retw.n
        .size   set_chan_freq_sw_start, .-set_chan_freq_sw_start
.Lsec_101_22b:

        .section .text.set_channel_rfpll_freq,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_102_0, 0x0000096c
        .literal .Lsec_102_4, 0x000009b4
        .literal .Lsec_102_8, chip7_sleep_params
        .literal .Lsec_102_c, g_phyFuns
        .literal .Lsec_102_10, .Lsec_58_0
        .literal .Lsec_102_14, 0x3ff5d008
        .literal .Lsec_102_18, 0x20000000
        .literal .Lsec_102_1c, 0x3ff4e168
        .literal .Lsec_102_20, 0x3ff4e0c4
        .literal .Lsec_102_24, phy_printf
        .literal .Lsec_102_28, esp_dport_access_reg_read
        .literal .Lsec_102_2c, set_chan_freq_sw_start
        .literal .Lsec_102_30, esp_dport_access_reg_read
        .literal .Lsec_102_34, write_wifi_chan_data
        .literal .Lsec_102_38, esp_dport_access_reg_read
.Lsec_102_3c:
        .global set_channel_rfpll_freq
        .type   set_channel_rfpll_freq, @function
set_channel_rfpll_freq:
        entry    a1, 32
        l32r     a5, .Lsec_102_8
        extui    a2, a2, 0, 8
        extui    a3, a3, 0, 8
        sext     a4, a4, 15
        l32i.n   a5, a5, 0
        bbsi     a5, 4, .Lsec_102_68
        sext     a10, a2, 7
        mov.n    a12, a4
        l32r     a2, .Lsec_102_c
        mov.n    a11, a3
        l32i.n   a2, a2, 0
        l32i     a2, a2, 244
        callx8   a2
        mov.n    a2, a10
        retw.n
        .byte    0x00
        .byte    0x00
.Lsec_102_68:
        sext     a11, a2, 7
        beqz.n   a11, .Lsec_102_79
        movi.n   a2, 14
        blt      a2, a11, .Lsec_102_79
        beq      a11, a2, .Lsec_102_a8
        j        .Lsec_102_8d
        .byte    0x00
.Lsec_102_79:
        l32r     a10, .Lsec_102_10
        movi.n   a7, 12
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_102_24
        callx8   a8
        mov.n    a6, a7
        l32r     a2, .Lsec_102_0
        j        .Lsec_102_af
        .byte    0x00
.Lsec_102_8d:
        addi.n   a11, a11, -1
        addx4    a11, a11, a11
        extui    a2, a11, 0, 16
        l32r     a5, .Lsec_102_0
        addi.n   a6, a2, 12
        extui    a6, a6, 0, 16
        add.n    a2, a2, a5
        extui    a2, a2, 0, 16
        extui    a7, a6, 0, 8
        j        .Lsec_102_af
.Lsec_102_a8:
        movi.n   a7, 84
        l32r     a2, .Lsec_102_4
        mov.n    a6, a7
.Lsec_102_af:
        l32r     a10, .Lsec_102_14
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_102_28
        callx8   a8
        l32r     a5, .Lsec_102_18
        and      a10, a10, a5
        extui    a10, a10, 29, 3
        l32r     a5, .Lsec_102_1c
        bnez.n   a10, .Lsec_102_d4
        mov.n    a12, a3
        mov.n    a11, a4
        mov.n    a10, a7
        .reloc  ., R_XTENSA_ASM_EXPAND, set_chan_freq_sw_start
        l32r     a8, .Lsec_102_2c
        callx8   a8
        retw.n
.Lsec_102_d4:
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_102_30
        callx8   a8
        bltz     a10, .Lsec_102_d4
        or       a10, a7, a7
        .reloc  ., R_XTENSA_ASM_EXPAND, write_wifi_chan_data
        l32r     a8, .Lsec_102_34
        callx8   a8
        l32r     a4, .Lsec_102_20
        movi     a3, 0xffffff00
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_102_38
        callx8   a8
        slli     a6, a6, 1
        and      a10, a10, a3
        extui    a6, a6, 0, 8
        or       a6, a6, a10
        memw
        s32i.n   a6, a4, 0
        retw.n
        .size   set_channel_rfpll_freq, .-set_channel_rfpll_freq
.Lsec_102_109:

        .section .text.chip_v7_set_chan_nomac,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_103_0, 0x60033c00
        .literal .Lsec_103_4, phy_freq_offset
        .literal .Lsec_103_8, chip7_sleep_params
        .literal .Lsec_103_c, phy_dis_pwdet_one
        .literal .Lsec_103_10, chip7_phy_init_ctrl
        .literal .Lsec_103_14, g_phyFuns
        .literal .Lsec_103_18, chan14_mic_en
        .literal .Lsec_103_1c, disable_wifi_agc
        .literal .Lsec_103_20, phy_enter_critical
        .literal .Lsec_103_24, set_channel_rfpll_freq
        .literal .Lsec_103_28, bb_bss_cbw40
        .literal .Lsec_103_2c, bb_bss_cbw40
        .literal .Lsec_103_30, chip_v7_set_chan_misc
        .literal .Lsec_103_34, phy_exit_critical
        .literal .Lsec_103_38, chan14_mic_cfg
        .literal .Lsec_103_3c, chan14_mic_cfg
        .literal .Lsec_103_40, phy_wifi_bb_reset
        .literal .Lsec_103_44, enable_wifi_agc
.Lsec_103_48:
        .global chip_v7_set_chan_nomac
        .type   chip_v7_set_chan_nomac, @function
chip_v7_set_chan_nomac:
        entry    a1, 48
        l32r     a6, .Lsec_103_8
        extui    a2, a2, 0, 8
        sext     a5, a2, 7
        extui    a3, a3, 0, 8
        sext     a4, a3, 7
        l32r     a7, .Lsec_103_0
        memw
        l32i.n   a7, a7, 0
        movi.n   a7, 1
        s8i      a2, a6, 81
        memw
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a2, .Lsec_103_c
        s8i      a7, a2, 0
        memw
        nop.n
        nop.n
        nop
        nop.n
        l32r     a2, .Lsec_103_4
        l16si    a7, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, disable_wifi_agc
        l32r     a8, .Lsec_103_1c
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_enter_critical
        l32r     a8, .Lsec_103_20
        callx8   a8
        mov.n    a12, a7
        mov.n    a2, a10
        l32r     a7, .Lsec_103_10
        mov.n    a10, a5
        l8ui     a11, a7, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, set_channel_rfpll_freq
        l32r     a8, .Lsec_103_24
        callx8   a8
        mov.n    a7, a10
        blti     a4, 4, .Lsec_103_c1
        l8ui     a10, a6, 83
        sext     a10, a10, 7
        .reloc  ., R_XTENSA_ASM_EXPAND, bb_bss_cbw40
        l32r     a8, .Lsec_103_28
        callx8   a8
        mov.n    a12, a4
        j        .Lsec_103_e5
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_103_c1:
        l8ui     a12, a6, 83
        sext     a12, a12, 7
        beq      a12, a4, .Lsec_103_e5
        mov.n    a10, a4
        s32i.n   a4, a1, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, bb_bss_cbw40
        l32r     a8, .Lsec_103_2c
        callx8   a8
        s8i      a3, a6, 83
        memw
        nop.n
        nop.n
        nop.n
        nop.n
        l32i     a12, a1, 0
.Lsec_103_e5:
        mov.n    a11, a7
        mov.n    a10, a5
        l32r     a3, .Lsec_103_14
        l32i.n   a3, a3, 0
        l32i     a3, a3, 100
        callx8   a3
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, chip_v7_set_chan_misc
        l32r     a8, .Lsec_103_30
        callx8   a8
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_exit_critical
        l32r     a8, .Lsec_103_34
        callx8   a8
        l32r     a2, .Lsec_103_18
        l8ui     a2, a2, 0
        beqz.n   a2, .Lsec_103_124
        movi.n   a2, 14
        bne      a5, a2, .Lsec_103_11c
        movi.n   a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, chan14_mic_cfg
        l32r     a8, .Lsec_103_38
        callx8   a8
        j        .Lsec_103_124
.Lsec_103_11c:
        movi.n   a10, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, chan14_mic_cfg
        l32r     a8, .Lsec_103_3c
        callx8   a8
.Lsec_103_124:
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_wifi_bb_reset
        l32r     a8, .Lsec_103_40
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, enable_wifi_agc
        l32r     a8, .Lsec_103_44
        callx8   a8
        retw.n
        .size   chip_v7_set_chan_nomac, .-chip_v7_set_chan_nomac
.Lsec_103_132:

        .section .text.chip_v7_set_chan,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_104_0, chip_sleep_prot_en
        .literal .Lsec_104_4, chip_v7_set_chan_nomac
        .literal .Lsec_104_8, chip_sleep_prot_dis
.Lsec_104_c:
        .global chip_v7_set_chan
        .type   chip_v7_set_chan, @function
chip_v7_set_chan:
        entry    a1, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, chip_sleep_prot_en
        l32r     a8, .Lsec_104_0
        callx8   a8
        extui    a10, a2, 0, 8
        extui    a11, a3, 0, 8
        sext     a11, a11, 7
        sext     a10, a10, 7
        .reloc  ., R_XTENSA_ASM_EXPAND, chip_v7_set_chan_nomac
        l32r     a8, .Lsec_104_4
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, chip_sleep_prot_dis
        l32r     a8, .Lsec_104_8
        callx8   a8
        retw.n
        .size   chip_v7_set_chan, .-chip_v7_set_chan
.Lsec_104_2f:

        .section .text.chip_v7_set_chan_offset,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_105_0, phy_freq_offset
        .literal .Lsec_105_4, chip7_phy_init_ctrl
        .literal .Lsec_105_8, chip7_sleep_params
        .literal .Lsec_105_c, phy_enter_critical
        .literal .Lsec_105_10, phy_freq_correct_opt
        .literal .Lsec_105_14, set_channel_rfpll_freq
        .literal .Lsec_105_18, bb_bss_cbw40
        .literal .Lsec_105_1c, phy_exit_critical
.Lsec_105_20:
        .global chip_v7_set_chan_offset
        .type   chip_v7_set_chan_offset, @function
chip_v7_set_chan_offset:
        entry    a1, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_enter_critical
        l32r     a8, .Lsec_105_c
        callx8   a8
        l32r     a3, .Lsec_105_4
        sext     a2, a2, 15
        mov.n    a5, a10
        l32r     a4, .Lsec_105_0
        l8ui     a8, a3, 78
        s16i     a2, a4, 0
        memw
        beqz.n   a8, .Lsec_105_59
        nop.n
        nop
        nop
        nop
        l8ui     a8, a3, 79
        sext     a8, a8, 7
        addx8    a2, a8, a2
        s16i     a2, a4, 0
        memw
.Lsec_105_59:
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_freq_correct_opt
        l32r     a8, .Lsec_105_10
        callx8   a8
        l32r     a2, .Lsec_105_8
        l8ui     a10, a2, 81
        sext     a10, a10, 7
        l16si    a12, a4, 0
        l8ui     a11, a3, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, set_channel_rfpll_freq
        l32r     a8, .Lsec_105_14
        callx8   a8
        l8ui     a10, a2, 83
        sext     a10, a10, 7
        .reloc  ., R_XTENSA_ASM_EXPAND, bb_bss_cbw40
        l32r     a8, .Lsec_105_18
        callx8   a8
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_exit_critical
        l32r     a8, .Lsec_105_1c
        callx8   a8
        retw.n
        .size   chip_v7_set_chan_offset, .-chip_v7_set_chan_offset
.Lsec_105_8a:

        .section .text.chip_v7_set_chan_ana,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_106_0, chip7_phy_init_ctrl
        .literal .Lsec_106_4, chip7_sleep_params
        .literal .Lsec_106_8, set_channel_rfpll_freq
.Lsec_106_c:
        .global chip_v7_set_chan_ana
        .type   chip_v7_set_chan_ana, @function
chip_v7_set_chan_ana:
        entry    a1, 32
        l32r     a8, .Lsec_106_0
        extui    a2, a2, 0, 8
        movi     a12, 0
        sext     a10, a2, 7
        l8ui     a11, a8, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, set_channel_rfpll_freq
        l32r     a8, .Lsec_106_8
        callx8   a8
        l32r     a8, .Lsec_106_4
        s8i      a2, a8, 81
        memw
        retw.n
        .size   chip_v7_set_chan_ana, .-chip_v7_set_chan_ana
.Lsec_106_2f:

        .section .text.freq_write_wifi_chan,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_107_0, 0x3ff4e0c4
        .literal .Lsec_107_4, 0xe7ffffff
        .literal .Lsec_107_8, 0x10000000
        .literal .Lsec_107_c, esp_dport_access_reg_read
        .literal .Lsec_107_10, write_wifi_chan_data
        .literal .Lsec_107_14, esp_dport_access_reg_read
        .literal .Lsec_107_18, esp_dport_access_reg_read
        .literal .Lsec_107_1c, ets_delay_us
        .literal .Lsec_107_20, esp_dport_access_reg_read
        .literal .Lsec_107_24, esp_dport_access_reg_read
        .literal .Lsec_107_28, esp_dport_access_reg_read
        .literal .Lsec_107_2c, esp_dport_access_reg_read
.Lsec_107_30:
        .global freq_write_wifi_chan
        .type   freq_write_wifi_chan, @function
freq_write_wifi_chan:
        entry    a1, 32
        l32r     a4, .Lsec_107_0
        extui    a2, a2, 0, 8
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_107_c
        callx8   a8
        l32r     a5, .Lsec_107_4
        and      a8, a10, a5
        mov.n    a10, a2
        slli     a2, a2, 1
        l32r     a3, .Lsec_107_8
        extui    a2, a2, 0, 8
        or       a3, a8, a3
        memw
        s32i     a3, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, write_wifi_chan_data
        l32r     a8, .Lsec_107_10
        callx8   a8
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_107_14
        callx8   a8
        movi     a3, 0xffffff00
        and      a10, a10, a3
        or       a2, a2, a10
        memw
        s32i     a2, a4, 0
        or       a10, a4, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_107_18
        callx8   a8
        movi     a3, 0x100
        or       a10, a10, a3
        memw
        s32i.n   a10, a4, 0
        movi.n   a10, 2
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_107_1c
        callx8   a8
        mov.n    a2, a4
.Lsec_107_96:
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_107_20
        callx8   a8
        bgez     a10, .Lsec_107_c6
.Lsec_107_a1:
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_107_24
        callx8   a8
        movi     a2, 0xfffffeff
        and      a10, a10, a2
        memw
        s32i.n   a10, a4, 0
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_107_28
        callx8   a8
        and      a10, a10, a5
        memw
        s32i.n   a10, a4, 0
        retw.n
.Lsec_107_c6:
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_107_2c
        callx8   a8
        bany     a10, a3, .Lsec_107_96
        j        .Lsec_107_a1
        .size   freq_write_wifi_chan, .-freq_write_wifi_chan
.Lsec_107_d4:

        .section .text.phy_unforce_chan,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_108_0, chip7_sleep_params
        .literal .Lsec_108_4, 0x0000096c
        .literal .Lsec_108_8, phy_enter_critical
        .literal .Lsec_108_c, freq_write_wifi_chan
        .literal .Lsec_108_10, phy_en_hw_set_freq
        .literal .Lsec_108_14, phy_exit_critical
.Lsec_108_18:
        .global phy_unforce_chan
        .type   phy_unforce_chan, @function
phy_unforce_chan:
        entry    a1, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_enter_critical
        l32r     a8, .Lsec_108_8
        callx8   a8
        l32r     a8, .Lsec_108_0
        movi.n   a9, 13
        mov.n    a2, a10
        movi.n   a10, 84
        l8ui     a8, a8, 81
        bltu     a9, a8, .Lsec_108_40
        addi.n   a10, a8, -1
        addx4    a10, a10, a10
        l32r     a8, .Lsec_108_4
        add.n    a10, a10, a8
        addi     a10, a10, -96
        extui    a10, a10, 0, 8
.Lsec_108_40:
        .reloc  ., R_XTENSA_ASM_EXPAND, freq_write_wifi_chan
        l32r     a8, .Lsec_108_c
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_en_hw_set_freq
        l32r     a8, .Lsec_108_10
        callx8   a8
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_exit_critical
        l32r     a8, .Lsec_108_14
        callx8   a8
        retw.n
        .size   phy_unforce_chan, .-phy_unforce_chan
.Lsec_108_56:

        .section .text.phy_force_chan,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_109_0, phy_enter_critical
        .literal .Lsec_109_4, phy_dis_hw_set_freq
        .literal .Lsec_109_8, freq_write_wifi_chan
        .literal .Lsec_109_c, phy_exit_critical
.Lsec_109_10:
        .global phy_force_chan
        .type   phy_force_chan, @function
phy_force_chan:
        entry    a1, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_enter_critical
        l32r     a8, .Lsec_109_0
        callx8   a8
        mov.n    a3, a10
        extui    a2, a2, 0, 8
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_dis_hw_set_freq
        l32r     a8, .Lsec_109_4
        callx8   a8
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, freq_write_wifi_chan
        l32r     a8, .Lsec_109_8
        callx8   a8
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_exit_critical
        l32r     a8, .Lsec_109_c
        callx8   a8
        retw.n
        .size   phy_force_chan, .-phy_force_chan
.Lsec_109_36:

        .section .text.phy_set_wifi_mode_only,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_110_0, phy_freq_wifi_only
.Lsec_110_4:
        .global phy_set_wifi_mode_only
        .type   phy_set_wifi_mode_only, @function
phy_set_wifi_mode_only:
        entry    a1, 32
        l32r     a8, .Lsec_110_0
        s8i      a2, a8, 0
        memw
        retw.n
        .size   phy_set_wifi_mode_only, .-phy_set_wifi_mode_only
.Lsec_110_12:

        .section .rodata.CSWTCH$169,"a",@progbits
        .align  4
.Lsec_111_0:
        .type   CSWTCH$169, @object
        .size   CSWTCH$169, 16
CSWTCH$169:
        .byte   0x00, 0x00, 0xd0, 0x41, 0x00, 0x00, 0xc0, 0x41, 0x00, 0x00, 0x20, 0x42, 0x00, 0x00, 0x00, 0x42

        .section .rodata.CSWTCH$133,"a",@progbits
        .align  4
.Lsec_112_0:
        .type   CSWTCH$133, @object
        .size   CSWTCH$133, 48
CSWTCH$133:
        .byte   0x00, 0x08, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00
        .byte   0x00, 0x00, 0x02, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x20, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00

        .section .bss.flag$5261,"aw",@nobits
        .align  4
.Lsec_113_0:
        .type   flag$5261, @object
        .size   flag$5261, 4
flag$5261:
        .skip   4

        .section .bss.bt_pwr_track_num,"aw",@nobits
        .align  1
.Lsec_114_0:
        .global bt_pwr_track_num
        .type   bt_pwr_track_num, @object
        .size   bt_pwr_track_num, 1
bt_pwr_track_num:
        .skip   1

        .section .bss.bt_pwr_freq_old,"aw",@nobits
        .align  1
.Lsec_115_0:
        .global bt_pwr_freq_old
        .type   bt_pwr_freq_old, @object
        .size   bt_pwr_freq_old, 1
bt_pwr_freq_old:
        .skip   1

        .section .bss.bt_pwr_cap_sum_old,"aw",@nobits
        .align  1
.Lsec_116_0:
        .global bt_pwr_cap_sum_old
        .type   bt_pwr_cap_sum_old, @object
        .size   bt_pwr_cap_sum_old, 1
bt_pwr_cap_sum_old:
        .skip   1

        .section .bss.bt_pwr_cap_sum,"aw",@nobits
        .align  2
.Lsec_117_0:
        .global bt_pwr_cap_sum
        .type   bt_pwr_cap_sum, @object
        .size   bt_pwr_cap_sum, 2
bt_pwr_cap_sum:
        .skip   2

        .section .data.dreg_1p2_set,"aw",@progbits
        .align  1
.Lsec_118_0:
        .global dreg_1p2_set
        .type   dreg_1p2_set, @object
        .size   dreg_1p2_set, 1
dreg_1p2_set:
        .byte   0x26

        .section .bss.btpwr_atten_offset,"aw",@nobits
        .align  1
.Lsec_119_0:
        .global btpwr_atten_offset
        .type   btpwr_atten_offset, @object
        .size   btpwr_atten_offset, 1
btpwr_atten_offset:
        .skip   1

        .section .bss.btpwr_tsen_flag,"aw",@nobits
        .align  1
.Lsec_120_0:
        .global btpwr_tsen_flag
        .type   btpwr_tsen_flag, @object
        .size   btpwr_tsen_flag, 1
btpwr_tsen_flag:
        .skip   1

        .section .bss.btpwr_tsen_old,"aw",@nobits
        .align  1
.Lsec_121_0:
        .global btpwr_tsen_old
        .type   btpwr_tsen_old, @object
        .size   btpwr_tsen_old, 1
btpwr_tsen_old:
        .skip   1

        .section .bss.btpwr_tsen_init,"aw",@nobits
        .align  1
.Lsec_122_0:
        .global btpwr_tsen_init
        .type   btpwr_tsen_init, @object
        .size   btpwr_tsen_init, 1
btpwr_tsen_init:
        .skip   1

        .section .bss.rfpll_offset_delta,"aw",@nobits
        .align  2
.Lsec_123_0:
        .global rfpll_offset_delta
        .type   rfpll_offset_delta, @object
        .size   rfpll_offset_delta, 2
rfpll_offset_delta:
        .skip   2

        .section .data.phy_wifi_pll_track_en,"aw",@progbits
        .align  1
.Lsec_124_0:
        .global phy_wifi_pll_track_en
        .type   phy_wifi_pll_track_en, @object
        .size   phy_wifi_pll_track_en, 1
phy_wifi_pll_track_en:
        .byte   0x01

        .section .data.phy_bt_pll_track_en,"aw",@progbits
        .align  1
.Lsec_125_0:
        .global phy_bt_pll_track_en
        .type   phy_bt_pll_track_en, @object
        .size   phy_bt_pll_track_en, 1
phy_bt_pll_track_en:
        .byte   0x01

        .section .bss.phy_bt_power_track_en,"aw",@nobits
        .align  1
.Lsec_126_0:
        .global phy_bt_power_track_en
        .type   phy_bt_power_track_en, @object
        .size   phy_bt_power_track_en, 1
phy_bt_power_track_en:
        .skip   1

        .section .bss.phy_sw_set_chan_en,"aw",@nobits
        .align  1
.Lsec_127_0:
        .global phy_sw_set_chan_en
        .type   phy_sw_set_chan_en, @object
        .size   phy_sw_set_chan_en, 1
phy_sw_set_chan_en:
        .skip   1

        .section .bss.phy_force_wifi_chan_en,"aw",@nobits
        .align  1
.Lsec_128_0:
        .global phy_force_wifi_chan_en
        .type   phy_force_wifi_chan_en, @object
        .size   phy_force_wifi_chan_en, 1
phy_force_wifi_chan_en:
        .skip   1

        .section .bss.phy_freq_wifi_only,"aw",@nobits
        .align  1
.Lsec_129_0:
        .global phy_freq_wifi_only
        .type   phy_freq_wifi_only, @object
        .size   phy_freq_wifi_only, 1
phy_freq_wifi_only:
        .skip   1

        .section .bss.phy_freq_offset,"aw",@nobits
        .align  2
.Lsec_130_0:
        .global phy_freq_offset
        .type   phy_freq_offset, @object
        .size   phy_freq_offset, 2
phy_freq_offset:
        .skip   2
