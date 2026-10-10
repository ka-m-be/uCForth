.type   phy_chan_pwr_index, @object
        .comm   phy_chan_pwr_index, 17, 4
        .weak   set_xpd_sar
        .type   phy_chan_target_power, @object
        .comm   phy_chan_target_power, 17, 4
        .type   phy_chan_gain_table, @object
        .comm   phy_chan_gain_table, 8, 4

        .section .iram1,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_40_0, chip7_phy_init_ctrl
        .literal .Lsec_40_4, .Lsec_83_0
.Lsec_40_8:
        .global get_target_power_offset
        .type   get_target_power_offset, @function
get_target_power_offset:
        entry    a1, 32
        extui    a8, a2, 0, 8
        bgeui    a8, 8, .Lsec_40_21
        l32r     a3, .Lsec_40_0
        movi.n   a2, 0
        l8ui     a8, a3, 28
        beq      a8, a2, .Lsec_40_4c
        l8ui     a2, a3, 29
        retw.n
.Lsec_40_21:
        movi.n   a2, 15
        bltu     a2, a8, .Lsec_40_3d
        l32r     a9, .Lsec_40_4
        addi     a8, a8, -8
        extui    a8, a8, 0, 8
        add.n    a9, a9, a8
        l8ui     a8, a9, 0
        add.n    a8, a3, a8
        l8ui     a2, a8, 0
        retw.n
        .byte    0x00
        .byte    0x00
.Lsec_40_3d:
        movi.n   a9, 23
        movi.n   a2, 0
        bltu     a9, a8, .Lsec_40_4c
        add.n    a8, a3, a8
        addi     a8, a8, -16
        l8ui     a2, a8, 0
.Lsec_40_4c:
        retw.n
        .size   get_target_power_offset, .-get_target_power_offset
.Lsec_40_4e:
        .byte    0x00
        .byte    0x00
.Lsec_40_50:
        .global ram_txbbgain_to_index
        .type   ram_txbbgain_to_index, @function
ram_txbbgain_to_index:
        entry    a1, 32
        extui    a8, a2, 0, 16
        movi.n   a9, 48
        beq      a8, a9, .Lsec_40_7c
        bltu     a9, a8, .Lsec_40_68
        movi.n   a2, 1
        beqi     a8, 32, .Lsec_40_82
        j        .Lsec_40_80
        .byte    0x00
        .byte    0x00
.Lsec_40_68:
        movi     a9, 160
        movi.n   a2, 2
        beq      a8, a9, .Lsec_40_82
        movi     a9, 0x120
        movi.n   a2, 3
        beq      a8, a9, .Lsec_40_82
        j        .Lsec_40_80
        .byte    0x00
.Lsec_40_7c:
        movi.n   a2, 4
        retw.n
.Lsec_40_80:
        movi.n   a2, 0
.Lsec_40_82:
        retw.n
        .size   ram_txbbgain_to_index, .-ram_txbbgain_to_index
        .literal_position
        .literal .Lsec_40_84, 0x66666667
        .literal .Lsec_40_88, 0x2e8ba2e9
.Lsec_40_8c:
        .global ram_set_chan_cal_interp
        .type   ram_set_chan_cal_interp, @function
ram_set_chan_cal_interp:
        entry    a1, 32
        addi.n   a3, a3, -1
        extui    a3, a3, 0, 8
        bgeui    a3, 6, .Lsec_40_a4
        l8ui     a8, a2, 0
        sext     a9, a8, 7
        l8ui     a2, a2, 1
        j        .Lsec_40_b5
        .byte    0x00
.Lsec_40_a4:
        movi.n   a8, 10
        bltu     a8, a3, .Lsec_40_d4
        l8ui     a8, a2, 1
        addi     a3, a3, -5
        sext     a9, a8, 7
        l8ui     a2, a2, 2
.Lsec_40_b5:
        sext     a2, a2, 7
        sub      a2, a2, a9
        mull     a3, a2, a3
        l32r     a9, .Lsec_40_84
        srai     a2, a3, 31
        mulsh    a3, a3, a9
        srai     a3, a3, 1
        sub      a2, a3, a2
        add.n    a2, a2, a8
        extui    a2, a2, 0, 8
        retw.n
.Lsec_40_d4:
        movi.n   a8, 12
        bltu     a8, a3, .Lsec_40_108
        l8ui     a9, a2, 2
        addi     a8, a3, -10
        sext     a3, a9, 7
        l8ui     a2, a2, 3
        sext     a2, a2, 7
        sub      a3, a2, a3
        mull     a3, a3, a8
        l32r     a2, .Lsec_40_88
        addx4    a3, a3, a3
        mulsh    a2, a3, a2
        srai     a3, a3, 31
        srai     a2, a2, 2
        sub      a2, a2, a3
        add.n    a2, a2, a9
        extui    a2, a2, 0, 8
        retw.n
        .byte    0x00
.Lsec_40_108:
        l8ui     a2, a2, 3
        retw.n
        .size   ram_set_chan_cal_interp, .-ram_set_chan_cal_interp
.Lsec_40_10d:
        .byte    0x00
        .byte    0x00
        .byte    0x00
        .literal_position
        .literal .Lsec_40_110, 0x3ff4504c
        .literal .Lsec_40_114, 0x3ff4506c
        .literal .Lsec_40_118, 0xfedcba98
        .literal .Lsec_40_11c, 0x3ff45070
        .literal .Lsec_40_120, 0x3ff45074
        .literal .Lsec_40_124, 0x3ff45078
        .literal .Lsec_40_128, 0x3ff4503c
        .literal .Lsec_40_12c, 0x77777777
        .literal .Lsec_40_130, 0x3ff45040
        .literal .Lsec_40_134, 0x3ff45044
        .literal .Lsec_40_138, 0x3ff45048
        .literal .Lsec_40_13c, phy_chan_pwr_index
        .literal .Lsec_40_140, 0x3ff4505c
        .literal .Lsec_40_144, lr_enable
        .literal .Lsec_40_148, 0x77770000
        .literal .Lsec_40_14c, 0x3ff45050
        .literal .Lsec_40_150, 0xffff0000
        .literal .Lsec_40_154, 0x00007777
        .literal .Lsec_40_158, esp_dport_access_reg_read
        .literal .Lsec_40_15c, esp_dport_access_reg_read
.Lsec_40_160:
        .global write_txrate_power_offset
        .type   write_txrate_power_offset, @function
write_txrate_power_offset:
        entry    a1, 32
        extui    a2, a2, 0, 8
        bnez     a2, .Lsec_40_215
        l32r     a4, .Lsec_40_118
        movi.n   a3, -1
        l32r     a2, .Lsec_40_114
        memw
        s32i.n   a4, a2, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a2, .Lsec_40_11c
        memw
        s32i.n   a3, a2, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a2, .Lsec_40_120
        l32r     a4, .Lsec_40_124
        memw
        s32i.n   a3, a2, 0
        memw
        s32i.n   a3, a4, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a2, .Lsec_40_12c
        l32r     a3, .Lsec_40_128
        memw
        s32i.n   a2, a3, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a3, .Lsec_40_130
        memw
        s32i.n   a2, a3, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a3, .Lsec_40_134
        memw
        s32i.n   a2, a3, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a3, .Lsec_40_138
        memw
        s32i.n   a2, a3, 0
        retw.n
        .byte    0x00
.Lsec_40_1e4:
        extui    a10, a2, 2, 6
        add.n    a10, a3, a10
        mov.n    a11, a4
        extui    a10, a10, 0, 8
        call8    get_target_power_offset
        slli     a8, a2, 2
        ssl      a8
        sll      a10, a10
        addi.n   a2, a2, 1
        add.n    a7, a7, a10
        bnei     a2, 8, .Lsec_40_1e4
        memw
        s32i.n   a7, a5, 0
        addi.n   a3, a3, 2
        addi.n   a5, a5, 4
        extui    a3, a3, 0, 8
        bne      a5, a6, .Lsec_40_220
        j        .Lsec_40_227
        .byte    0x00
        .byte    0x00
.Lsec_40_215:
        l32r     a5, .Lsec_40_110
        movi.n   a3, 8
        l32r     a4, .Lsec_40_13c
        l32r     a6, .Lsec_40_140
.Lsec_40_220:
        movi.n   a2, 0
        mov.n    a7, a2
        j        .Lsec_40_1e4
.Lsec_40_227:
        l32r     a2, .Lsec_40_144
        l8ui     a2, a2, 0
        beqz.n   a2, .Lsec_40_265
        l32r     a10, .Lsec_40_110
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_40_158
        callx8   a8
        extui    a10, a10, 0, 16
        l32r     a2, .Lsec_40_148
        or       a2, a10, a2
        l32r     a3, .Lsec_40_14c
        mov.n    a10, a3
        l32r     a6, .Lsec_40_110
        memw
        s32i.n   a2, a6, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_40_15c
        callx8   a8
        l32r     a2, .Lsec_40_150
        and      a10, a10, a2
        l32r     a2, .Lsec_40_154
        or       a2, a10, a2
        memw
        s32i.n   a2, a3, 0
.Lsec_40_265:
        movi.n   a11, 0
        j        .Lsec_40_2af
        .byte    0x00
        .byte    0x00
.Lsec_40_26c:
        srli     a2, a8, 2
        extui    a9, a8, 0, 2
        add.n    a2, a10, a2
        bnei     a9, 3, .Lsec_40_28c
        srli     a2, a8, 2
        add.n    a2, a10, a2
        l8ui     a2, a2, 8
        ssl      a3
        sll      a2, a2
        add.n    a7, a7, a2
        j        .Lsec_40_297
        .byte    0x00
        .byte    0x00
.Lsec_40_28c:
        l8ui     a2, a2, 0
        ssl      a3
        sll      a2, a2
        add.n    a7, a7, a2
.Lsec_40_297:
        addi.n   a8, a8, 1
        extui    a8, a8, 0, 8
        addi.n   a3, a3, 4
        addi.n   a6, a6, -1
        bnez     a6, .Lsec_40_26c
        memw
        s32i.n   a7, a5, 0
        addi.n   a11, a11, 2
        addi.n   a5, a5, 4
        beqi     a11, 8, .Lsec_40_2bc
.Lsec_40_2af:
        movi.n   a3, 0
        mov.n    a7, a3
        mov.n    a8, a3
        add.n    a10, a4, a11
        movi.n   a6, 8
        j        .Lsec_40_26c
.Lsec_40_2bc:
        retw.n
        .size   write_txrate_power_offset, .-write_txrate_power_offset
.Lsec_40_2be:
        .byte    0x00
        .byte    0x00
        .literal_position
        .literal .Lsec_40_2c0, chip7_sleep_params
        .literal .Lsec_40_2c4, g_phyFuns
        .literal .Lsec_40_2c8, chip7_sleep_params+0x5a
        .literal .Lsec_40_2cc, target_power_backoff_qdb
.Lsec_40_2d0:
        .global get_phy_target_power
        .type   get_phy_target_power, @function
get_phy_target_power:
        entry    a1, 48
        l32r     a4, .Lsec_40_2c4
        mov.n    a10, a1
        sext     a3, a3, 15
        extui    a3, a3, 0, 8
        l32r     a8, .Lsec_40_2c0
        l32i     a9, a4, 0
        l16ui    a12, a8, 10
        l16ui    a15, a8, 4
        l16ui    a14, a8, 6
        l16ui    a13, a8, 8
        l8ui     a11, a8, 81
        l32i     a9, a9, 0x168
        s8i      a15, a1, 0
        s8i      a14, a1, 1
        s8i      a13, a1, 2
        s8i      a12, a1, 3
        memw
        callx8   a9
        sext     a12, a10, 7
        mov.n    a10, a2
        l32i.n   a4, a4, 0
        l32r     a11, .Lsec_40_2c8
        l32i     a4, a4, 0x18c
        callx8   a4
        movi.n   a10, 6
        l32r     a11, .Lsec_40_2cc
        loop     a10, .Lsec_40_333
        l8ui     a8, a11, 0
        sub      a9, a3, a8
        l8ui     a8, a2, 0
        add.n    a8, a9, a8
        s8i      a8, a2, 0
        memw
        addi.n   a2, a2, 1
.Lsec_40_333:
        retw.n
        .size   get_phy_target_power, .-get_phy_target_power
.Lsec_40_335:
        .byte    0x00
        .byte    0x00
        .byte    0x00
        .literal_position
        .literal .Lsec_40_338, 0x3ff460a0
        .literal .Lsec_40_33c, 0xfffff0ff
        .literal .Lsec_40_340, 0x00000800
        .literal .Lsec_40_344, 0x00000a00
        .literal .Lsec_40_348, esp_dport_access_reg_read
        .literal .Lsec_40_34c, ets_delay_us
        .literal .Lsec_40_350, esp_dport_access_reg_read
        .literal .Lsec_40_354, ets_delay_us
        .literal .Lsec_40_358, esp_dport_access_reg_read
        .literal .Lsec_40_35c, ets_delay_us
        .literal .Lsec_40_360, esp_dport_access_reg_read
        .literal .Lsec_40_364, ets_delay_us
.Lsec_40_368:
        .global force_txrx_off
        .type   force_txrx_off, @function
force_txrx_off:
        entry    a1, 32
        extui    a2, a2, 0, 8
        beqz     a2, .Lsec_40_3b6
        l32r     a2, .Lsec_40_338
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_40_348
        callx8   a8
        l32r     a3, .Lsec_40_33c
        and      a9, a10, a3
        movi.n   a10, 1
        l32r     a8, .Lsec_40_340
        or       a8, a9, a8
        memw
        s32i.n   a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_40_34c
        callx8   a8
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_40_350
        callx8   a8
        and      a3, a10, a3
        l32r     a8, .Lsec_40_344
        movi.n   a10, 1
        or       a3, a3, a8
        memw
        s32i     a3, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_40_354
        callx8   a8
        retw.n
.Lsec_40_3b6:
        l32r     a2, .Lsec_40_338
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_40_358
        callx8   a8
        movi     a8, 0x200
        l32r     a3, .Lsec_40_33c
        and      a10, a10, a3
        or       a8, a10, a8
        movi.n   a10, 1
        memw
        s32i.n   a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_40_35c
        callx8   a8
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_40_360
        callx8   a8
        and      a10, a10, a3
        memw
        s32i.n   a10, a2, 0
        movi.n   a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_40_364
        callx8   a8
        retw.n
        .size   force_txrx_off, .-force_txrx_off
        .literal_position
        .literal .Lsec_40_3f4, 0x3ff4e060
        .literal .Lsec_40_3f8, phy_pwdet_onetime_flag
        .literal .Lsec_40_3fc, pwdet_sar2_init
        .literal .Lsec_40_400, esp_dport_access_reg_read
        .literal .Lsec_40_404, esp_dport_access_reg_read
.Lsec_40_408:
        .global phy_pwdet_onetime_en
        .type   phy_pwdet_onetime_en, @function
phy_pwdet_onetime_en:
        entry    a1, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, pwdet_sar2_init
        l32r     a8, .Lsec_40_3fc
        callx8   a8
        l32r     a2, .Lsec_40_3f4
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_40_400
        callx8   a8
        movi.n   a8, -2
        and      a10, a10, a8
        memw
        s32i.n   a10, a2, 0
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_40_404
        callx8   a8
        movi.n   a8, 1
        or       a10, a10, a8
        memw
        s32i.n   a10, a2, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a2, .Lsec_40_3f8
        s8i      a8, a2, 0
        memw
        retw.n
        .size   phy_pwdet_onetime_en, .-phy_pwdet_onetime_en
.Lsec_40_44b:
        .byte    0x00
        .literal_position
        .literal .Lsec_40_44c, 0x3ff4e080
        .literal .Lsec_40_450, esp_dport_access_reg_read
.Lsec_40_454:
        .global ram_read_sar_dout
        .type   ram_read_sar_dout, @function
ram_read_sar_dout:
        entry    a1, 32
        l32r     a4, .Lsec_40_44c
        movi     a3, 0
        movi     a5, -1
.Lsec_40_460:
        addx4    a10, a3, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_40_450
        callx8   a8
        xor      a10, a5, a10
        extui    a10, a10, 0, 12
        s16i     a10, a2, 0
        memw
        addi.n   a3, a3, 1
        addi.n   a2, a2, 2
        bnei     a3, 8, .Lsec_40_460
        retw.n
        .size   ram_read_sar_dout, .-ram_read_sar_dout
.Lsec_40_47e:
        .byte    0x00
        .byte    0x00
        .literal_position
        .literal .Lsec_40_480, chip7_phy_init_ctrl
        .literal .Lsec_40_484, .Lsec_46_0
        .literal .Lsec_40_488, .Lsec_46_24
.Lsec_40_48c:
        .global get_rate_fcc_index
        .type   get_rate_fcc_index, @function
get_rate_fcc_index:
        entry    a1, 32
        l32r     a10, .Lsec_40_480
        extui    a2, a2, 0, 8
        l8ui     a8, a10, 31
        bnei     a8, 2, .Lsec_40_514
        add.n    a8, a10, a2
        addi     a2, a2, -3
        l8ui     a8, a8, 31
        extui    a2, a2, 0, 8
        movi.n   a11, 8
        srli     a9, a8, 4
        extui    a8, a8, 0, 4
        bltu     a11, a2, .Lsec_40_50d
        l32r     a11, .Lsec_40_484
        addx4    a2, a2, a11
        l32i.n   a2, a2, 0
        jx       a2
.Lsec_40_4bc:
        l8ui     a2, a10, 46
        extui    a2, a2, 0, 4
        j        .Lsec_40_5b0
.Lsec_40_4c5:
        l8ui     a2, a10, 46
        srli     a2, a2, 4
        j        .Lsec_40_5b0
.Lsec_40_4ce:
        l8ui     a2, a10, 47
        extui    a2, a2, 0, 4
        j        .Lsec_40_5b0
.Lsec_40_4d7:
        l8ui     a2, a10, 47
        srli     a2, a2, 4
        j        .Lsec_40_5b0
.Lsec_40_4e0:
        l8ui     a2, a10, 48
        extui    a2, a2, 0, 4
        j        .Lsec_40_5b0
.Lsec_40_4e9:
        l8ui     a2, a10, 48
        srli     a2, a2, 4
        j        .Lsec_40_5b0
.Lsec_40_4f2:
        l8ui     a2, a10, 49
        extui    a2, a2, 0, 4
        j        .Lsec_40_5b0
.Lsec_40_4fb:
        l8ui     a2, a10, 49
        srli     a2, a2, 4
        j        .Lsec_40_5b0
.Lsec_40_504:
        l8ui     a2, a10, 50
        extui    a2, a2, 0, 4
        j        .Lsec_40_5b0
.Lsec_40_50d:
        movi.n   a2, 0
        j        .Lsec_40_5b0
        .byte    0x00
        .byte    0x00
.Lsec_40_514:
        beqi     a8, 3, .Lsec_40_51a
        j        .Lsec_40_5aa
.Lsec_40_51a:
        addi.n   a8, a2, -1
        addi     a2, a2, -3
        add.n    a8, a10, a8
        extui    a2, a2, 0, 8
        movi.n   a11, 8
        l8ui     a9, a8, 32
        l8ui     a8, a8, 80
        bltu     a11, a2, .Lsec_40_59d
        l32r     a11, .Lsec_40_488
        addx4    a2, a2, a11
        l32i.n   a2, a2, 0
        jx       a2
.Lsec_40_53a:
        l8ui     a10, a10, 46
        movi.n   a2, 25
        minu     a2, a10, a2
        j        .Lsec_40_59f
.Lsec_40_545:
        l8ui     a10, a10, 47
        movi.n   a2, 25
        minu     a2, a10, a2
        j        .Lsec_40_59f
.Lsec_40_550:
        l8ui     a10, a10, 48
        movi.n   a2, 25
        minu     a2, a10, a2
        j        .Lsec_40_59f
.Lsec_40_55b:
        l8ui     a10, a10, 49
        movi.n   a2, 25
        minu     a2, a10, a2
        j        .Lsec_40_59f
.Lsec_40_566:
        l8ui     a10, a10, 50
        movi.n   a2, 25
        minu     a2, a10, a2
        j        .Lsec_40_59f
.Lsec_40_571:
        l8ui     a10, a10, 52
        movi.n   a2, 25
        minu     a2, a10, a2
        j        .Lsec_40_59f
.Lsec_40_57c:
        l8ui     a10, a10, 53
        movi.n   a2, 25
        minu     a2, a10, a2
        j        .Lsec_40_59f
.Lsec_40_587:
        l8ui     a10, a10, 54
        movi.n   a2, 25
        minu     a2, a10, a2
        j        .Lsec_40_59f
.Lsec_40_592:
        l8ui     a10, a10, 55
        movi.n   a2, 25
        minu     a2, a10, a2
        j        .Lsec_40_59f
.Lsec_40_59d:
        movi.n   a2, 0
.Lsec_40_59f:
        movi.n   a10, 25
        minu     a9, a9, a10
        minu     a8, a8, a10
        j        .Lsec_40_5b0
.Lsec_40_5aa:
        movi.n   a2, 0
        mov.n    a8, a2
        mov.n    a9, a2
.Lsec_40_5b0:
        s8i      a8, a3, 0
        s8i      a9, a3, 1
        s8i      a2, a3, 2
        memw
        retw.n
        .size   get_rate_fcc_index, .-get_rate_fcc_index
.Lsec_40_5be:
        .byte    0x00
        .byte    0x00
        .literal_position
        .literal .Lsec_40_5c0, .Lsec_83_0
        .literal .Lsec_40_5c4, phy_chan_target_power
.Lsec_40_5c8:
        .global get_rate_target_power
        .type   get_rate_target_power, @function
get_rate_target_power:
        entry    a1, 32
        extui    a2, a2, 0, 8
        srli     a9, a2, 2
        movi.n   a8, 16
        bltui    a9, 8, .Lsec_40_605
        movi.n   a8, 15
        bltu     a8, a9, .Lsec_40_5ec
        addi     a9, a9, -8
        l32r     a2, .Lsec_40_5c0
        extui    a9, a9, 0, 8
        add.n    a9, a2, a9
        l8ui     a8, a9, 0
        j        .Lsec_40_605
.Lsec_40_5ec:
        movi.n   a10, 23
        movi.n   a8, 0
        bltu     a10, a9, .Lsec_40_605
        addi     a8, a9, -16
        extui    a2, a2, 0, 2
        extui    a8, a8, 0, 8
        bnei     a2, 3, .Lsec_40_605
        addi     a8, a9, -8
        extui    a8, a8, 0, 8
.Lsec_40_605:
        l32r     a2, .Lsec_40_5c4
        add.n    a8, a2, a8
        l8ui     a2, a8, 0
        retw.n
        .size   get_rate_target_power, .-get_rate_target_power
.Lsec_40_60f:
        .byte    0x00
        .literal_position
        .literal .Lsec_40_610, phy_chan_pwr_index
        .literal .Lsec_40_614, chip7_phy_init_ctrl
        .literal .Lsec_40_618, phy_set_most_tpw_index
.Lsec_40_61c:
        .global get_chan_pwr_index
        .type   get_chan_pwr_index, @function
get_chan_pwr_index:
        entry    a1, 32
        l32r     a9, .Lsec_40_614
        movi.n   a2, 0
        mov.n    a10, a9
        mov.n    a8, a2
        movi.n   a12, 17
        l8ui     a11, a10, 28
        l8ui     a9, a9, 29
        moveqz   a9, a2, a11
        extui    a2, a9, 0, 8
        l32r     a9, .Lsec_40_618
        l32r     a15, .Lsec_40_610
        mov.n    a11, a15
        l8ui     a14, a9, 0
        loop     a12, .Lsec_40_690
        extui    a9, a8, 0, 8
        bgeui    a8, 8, .Lsec_40_658
        mov.n    a9, a8
        l8ui     a13, a10, 20
        s8i      a13, a11, 0
        memw
        j        .Lsec_40_67f
.Lsec_40_658:
        beqi     a9, 16, .Lsec_40_669
        l8ui     a13, a10, 12
        mov.n    a9, a8
        s8i      a13, a11, 0
        memw
        j        .Lsec_40_67f
.Lsec_40_669:
        s8i      a2, a15, 16
        memw
        nop.n
        nop.n
        nop.n
        nop.n
        l8ui     a13, a11, 0
        bltu     a13, a14, .Lsec_40_682
        retw.n
.Lsec_40_67f:
        bgeu     a13, a14, .Lsec_40_68a
.Lsec_40_682:
        add.n    a9, a15, a9
        s8i      a14, a9, 0
        memw
.Lsec_40_68a:
        addi.n   a8, a8, 1
        addi.n   a11, a11, 1
        addi.n   a10, a10, 1
.Lsec_40_690:
        retw.n
        .size   get_chan_pwr_index, .-get_chan_pwr_index
.Lsec_40_692:
        .byte    0x00
        .byte    0x00
        .literal_position
        .literal .Lsec_40_694, 0x3ff46004
        .literal .Lsec_40_698, 0x3ff46008
.Lsec_40_69c:
        .global write_wifi_dig_gain
        .type   write_wifi_dig_gain, @function
write_wifi_dig_gain:
        entry    a1, 32
        l8ui     a9, a2, 1
        slli     a9, a9, 8
        l8ui     a8, a2, 0
        or       a10, a9, a8
        l8ui     a8, a2, 2
        slli     a8, a8, 16
        or       a9, a8, a10
        l8ui     a8, a2, 3
        slli     a8, a8, 24
        or       a8, a8, a9
        l32r     a9, .Lsec_40_694
        memw
        s32i.n   a8, a9, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l8ui     a8, a2, 5
        slli     a8, a8, 8
        l8ui     a9, a2, 4
        or       a9, a8, a9
        l8ui     a8, a2, 6
        slli     a8, a8, 16
        or       a8, a8, a9
        l8ui     a2, a2, 7
        slli     a2, a2, 24
        or       a2, a2, a8
        l32r     a8, .Lsec_40_698
        memw
        s32i.n   a2, a8, 0
        retw.n
        .size   write_wifi_dig_gain, .-write_wifi_dig_gain
.Lsec_40_6f5:
        .byte    0x00
        .byte    0x00
        .byte    0x00
        .literal_position
        .literal .Lsec_40_6f8, .Lsec_46_48
        .literal .Lsec_40_6fc, .Lsec_46_5c
        .literal .Lsec_40_700, memcpy
        .literal .Lsec_40_704, memcpy
.Lsec_40_708:
        .global correct_rf_ana_gain_new
        .type   correct_rf_ana_gain_new, @function
correct_rf_ana_gain_new:
        entry    a1, 96
        addi     a7, a1, 34
        l32r     a11, .Lsec_40_6f8
        movi.n   a12, 17
        or       a10, a7, a7
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_40_700
        callx8   a8
        movi.n   a12, 34
        l32r     a11, .Lsec_40_6fc
        mov.n    a10, a1
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_40_704
        callx8   a8
        extui    a5, a5, 0, 8
        or       a8, a7, a7
        l16ui    a12, a3, 0
        movi     a9, 17
        loop     a9, .Lsec_40_74c
        sub      a11, a8, a7
        l8ui     a10, a8, 0
        bne      a10, a12, .Lsec_40_74a
        addx2    a11, a11, a1
        l16si    a6, a11, 0
        j        .Lsec_40_74c
.Lsec_40_74a:
        addi.n   a8, a8, 1
.Lsec_40_74c:
        extui    a6, a6, 0, 16
        mov.n    a11, a1
        mov.n    a8, a1
        movi.n   a10, 17
        loop     a10, .Lsec_40_766
        l16ui    a9, a8, 0
        sub      a9, a9, a6
        s16i     a9, a8, 0
        memw
        addi.n   a8, a8, 2
.Lsec_40_766:
        nop.n
        nop.n
        nop.n
        l16si    a12, a2, 0
        movi.n   a8, 0
        movi.n   a6, 17
        loop     a6, .Lsec_40_783
        addi.n   a10, a8, 1
        l16si    a9, a11, 0
        blt      a9, a12, .Lsec_40_7dc
        extui    a8, a10, 0, 8
        addi.n   a11, a11, 2
.Lsec_40_783:
        j        .Lsec_40_7f8
.Lsec_40_786:
        addi.n   a6, a8, -1
        extui    a6, a6, 0, 8
        addx2    a8, a6, a1
        l16ui    a9, a8, 0
        add.n    a7, a7, a6
        sext     a8, a9, 15
        l8ui     a7, a7, 0
        s16i     a9, a2, 0
        s16i     a7, a3, 0
        memw
        bnez     a5, .Lsec_40_80d
        bnez.n   a6, .Lsec_40_7cc
        nop.n
        nop.n
        nop.n
        nop.n
        l16si    a7, a1, 0
.Lsec_40_7b2:
        movi.n   a5, 8
        l16si    a3, a2, 0
        sub      a3, a3, a7
        bge      a5, a3, .Lsec_40_7cc
        movi.n   a3, 48
        s16i     a3, a4, 0
        add.n    a7, a7, a5
        s16i     a7, a2, 0
        memw
        retw.n
.Lsec_40_7cc:
        movi     a3, 0x120
        s16i     a3, a4, 0
        s16i     a8, a2, 0
        memw
        retw.n
        .byte    0x00
        .byte    0x00
.Lsec_40_7dc:
        bnez     a8, .Lsec_40_786
        l16ui    a6, a1, 0
        sext     a8, a6, 15
        mov.n    a7, a8
        l8ui     a9, a1, 34
        s16i     a6, a2, 0
        s16i     a9, a3, 0
        memw
        beqz     a5, .Lsec_40_7b2
        retw.n
.Lsec_40_7f8:
        l16ui    a6, a1, 30
        sext     a8, a6, 15
        l8ui     a7, a1, 49
        s16i     a6, a2, 0
        s16i     a7, a3, 0
        memw
        beqz     a5, .Lsec_40_7cc
.Lsec_40_80d:
        retw.n
        .size   correct_rf_ana_gain_new, .-correct_rf_ana_gain_new
.Lsec_40_80f:
        .byte    0x00
        .literal_position
        .literal .Lsec_40_810, phy_chan_gain_table
        .literal .Lsec_40_814, chip7_sleep_params
        .literal .Lsec_40_818, tx_rf_ana_gain
        .literal .Lsec_40_81c, phy_chan_gain_table+0x8
        .literal .Lsec_40_820, g_phyFuns
        .literal .Lsec_40_824, force_tx_gain_en
        .literal .Lsec_40_828, set_tx_gain_table
.Lsec_40_82c:
        .global tx_gain_table_set
        .type   tx_gain_table_set, @function
tx_gain_table_set:
        entry    a1, 48
        l32r     a3, .Lsec_40_814
        movi.n   a13, 0
        addi.n   a12, a1, 10
        addi.n   a11, a1, 8
        addi.n   a10, a1, 12
        l8ui     a2, a3, 184
        sext     a2, a2, 7
        l16ui    a3, a3, 182
        sub      a2, a2, a3
        extui    a2, a2, 0, 16
        l32r     a5, .Lsec_40_810
        l8ui     a3, a5, 5
        sext     a3, a3, 7
        addi     a3, a3, 20
        add.n    a3, a2, a3
        l32r     a4, .Lsec_40_818
        l16ui    a6, a4, 0
        l16ui    a4, a4, 2
        s16i     a3, a1, 12
        s16i     a6, a1, 8
        mov.n    a6, a1
        s16i     a4, a1, 10
        memw
        call8    correct_rf_ana_gain_new
        l16ui    a3, a1, 12
        sub      a2, a2, a3
        extui    a2, a2, 0, 16
        l32r     a4, .Lsec_40_81c
        l32r     a3, .Lsec_40_820
.Lsec_40_880:
        movi     a12, -127
        movi.n   a11, 30
        l8ui     a10, a5, 0
        addi.n   a5, a5, 1
        sext     a10, a10, 7
        add.n    a10, a2, a10
        sext     a10, a10, 15
        l32i.n   a8, a3, 0
        l32i.n   a8, a8, 44
        callx8   a8
        s8i      a10, a6, 0
        memw
        addi.n   a6, a6, 1
        bne      a5, a4, .Lsec_40_880
        nop.n
        nop.n
        nop.n
        l32r     a2, .Lsec_40_824
        l8ui     a2, a2, 0
        bnez.n   a2, .Lsec_40_8ce
        movi     a10, 1
        call8    force_txrx_off
        l16ui    a11, a1, 10
        l16ui    a10, a1, 8
        .reloc  ., R_XTENSA_ASM_EXPAND, set_tx_gain_table
        l32r     a8, .Lsec_40_828
        callx8   a8
        mov.n    a10, a1
        call8    write_wifi_dig_gain
        mov.n    a10, a2
        call8    force_txrx_off
.Lsec_40_8ce:
        retw.n
        .size   tx_gain_table_set, .-tx_gain_table_set
        .literal_position
        .literal .Lsec_40_8d0, phy_chan_pwr_index
        .literal .Lsec_40_8d4, phy_chan_target_power
        .literal .Lsec_40_8d8, phy_chan_gain_table
        .literal .Lsec_40_8dc, chip7_sleep_params
        .literal .Lsec_40_8e0, phy_in_vdd33_offset
        .literal .Lsec_40_8e4, g_phyFuns
        .literal .Lsec_40_8e8, chip7_phy_init_ctrl
.Lsec_40_8ec:
        .global set_chan_dig_gain
        .type   set_chan_dig_gain, @function
set_chan_dig_gain:
        entry    a1, 80
        l32r     a3, .Lsec_40_8e0
        addi     a10, a1, 6
        extui    a2, a2, 0, 8
        movi     a7, 0
        l8ui     a11, a3, 0
        sext     a11, a11, 7
        call8    get_phy_target_power
        l32r     a5, .Lsec_40_8e4
        l32r     a3, .Lsec_40_8d8
        j        .Lsec_40_943
.Lsec_40_90d:
        l8ui     a9, a4, 0
        addi.n   a4, a4, 6
        s8i      a9, a8, 0
        memw
        addi.n   a8, a8, 1
        addi.n   a6, a6, -1
        bnez     a6, .Lsec_40_90d
        mov.n    a11, a2
        addi.n   a10, a1, 12
        l32i.n   a4, a5, 0
        add.n    a6, a1, a7
        l32i     a4, a4, 0x168
        callx8   a4
        neg      a10, a10
        extui    a10, a10, 0, 8
        add.n    a4, a3, a7
        s8i      a10, a6, 0
        addi.n   a7, a7, 1
        s8i      a10, a4, 0
        memw
        beqi     a7, 6, .Lsec_40_959
.Lsec_40_943:
        addi     a4, a7, 96
        addi.n   a8, a1, 12
        nop.n
        nop.n
        l32r     a6, .Lsec_40_8dc
        add.n    a4, a6, a4
        movi.n   a6, 4
        j        .Lsec_40_90d
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_40_959:
        call8    get_chan_pwr_index
        l32r     a5, .Lsec_40_8d0
        movi.n   a11, 17
        mov.n    a4, a5
        mov.n    a10, a5
        l8ui     a8, a3, 5
        l8ui     a6, a5, 16
        add.n    a6, a3, a6
        s8i      a8, a3, 6
        memw
        nop.n
        nop.n
        nop.n
        nop.n
        l8ui     a8, a6, 0
        addi.n   a8, a8, 6
        l32r     a6, .Lsec_40_8d4
        mov.n    a9, a6
        s8i      a8, a3, 7
        memw
        loop     a11, .Lsec_40_9a8
        addi.n   a8, a1, 6
        nop.n
        nop.n
        nop.n
        l8ui     a7, a10, 0
        addi.n   a10, a10, 1
        add.n    a7, a8, a7
        l8ui     a7, a7, 0
        s8i      a7, a9, 0
        memw
        addi.n   a9, a9, 1
.Lsec_40_9a8:
        nop.n
        nop.n
        nop.n
        l32r     a8, .Lsec_40_8e8
        l8ui     a8, a8, 31
        addi     a8, a8, -2
        extui    a8, a8, 0, 8
        bltui    a8, 2, .Lsec_40_9c0
        j        .Lsec_40_b77
.Lsec_40_9c0:
        addi     a11, a1, 19
        mov.n    a10, a2
        call8    get_rate_fcc_index
        addi     a8, a2, -3
        extui    a8, a8, 0, 8
        bgeui    a8, 2, .Lsec_40_a2d
        l32r     a9, .Lsec_40_8dc
        l8ui     a8, a9, 83
        bnei     a8, 2, .Lsec_40_a2d
        addi     a10, a2, -2
.Lsec_40_9dd:
        addi     a11, a1, 16
        extui    a10, a10, 0, 8
        call8    get_rate_fcc_index
        l8ui     a2, a1, 16
        s8i      a2, a1, 19
        memw
        nop.n
        nop.n
        nop.n
        nop.n
        l8ui     a2, a1, 17
        s8i      a2, a1, 20
        memw
.Lsec_40_a00:
        movi.n   a10, 0
        mov.n    a8, a10
        movi.n   a11, 17
        nop.n
        l8ui     a2, a1, 5
        addi.n   a7, a2, 6
        extui    a12, a7, 0, 8
        s32i.n   a12, a1, 40
        nop.n
        nop.n
        nop.n
        nop.n
        l8ui     a9, a1, 20
        l8ui     a12, a1, 11
        l8ui     a15, a1, 19
        l8ui     a14, a1, 21
        s32i.n   a9, a1, 36
        s32i.n   a12, a1, 32
        j        .Lsec_40_a44
.Lsec_40_a2d:
        addi     a8, a2, -10
        extui    a8, a8, 0, 8
        bgeui    a8, 2, .Lsec_40_a00
        l32r     a8, .Lsec_40_8dc
        addi.n   a10, a2, 2
        l8ui     a7, a8, 83
        beqi     a7, 3, .Lsec_40_9dd
        j        .Lsec_40_a00
.Lsec_40_a44:
        bltui    a8, 8, .Lsec_40_a58
        bnei     a8, 16, .Lsec_40_a5d
        l8ui     a2, a4, 0
        l32i.n   a9, a1, 36
        bltu     a2, a9, .Lsec_40_a55
        j        .Lsec_40_b77
.Lsec_40_a55:
        j        .Lsec_40_b6f
.Lsec_40_a58:
        mov.n    a9, a15
        j        .Lsec_40_a5f
.Lsec_40_a5d:
        mov.n    a9, a14
.Lsec_40_a5f:
        l8ui     a7, a4, 0
        bltu     a7, a9, .Lsec_40_a68
        j        .Lsec_40_b09
.Lsec_40_a68:
        bgeui    a9, 6, .Lsec_40_a9c
        addi.n   a12, a1, 6
        add.n    a7, a12, a9
        l8ui     a13, a7, 0
        add.n    a12, a6, a10
        add.n    a7, a5, a10
        s8i      a13, a12, 0
        s8i      a9, a7, 0
        memw
        beqi     a8, 16, .Lsec_40_a85
        j        .Lsec_40_b09
.Lsec_40_a85:
        add.n    a9, a1, a9
        nop.n
        nop.n
        nop.n
        l8ui     a2, a9, 0
        addi.n   a2, a2, 6
        s8i      a2, a3, 7
        memw
        j        .Lsec_40_b77
        .byte    0x00
.Lsec_40_a9c:
        bgeui    a8, 8, .Lsec_40_ad0
        addi     a7, a9, -5
        add.n    a12, a6, a10
        s32i.n   a12, a1, 44
        slli     a7, a7, 2
        extui    a7, a7, 0, 8
        add.n    a9, a5, a10
        nop.n
        l32i.n   a12, a1, 32
        sub      a13, a12, a7
        movi.n   a12, 5
        s8i      a12, a9, 0
        memw
        movi.n   a9, 5
        nop.n
        nop.n
        nop.n
        l32i.n   a12, a1, 44
        s8i      a13, a12, 0
        memw
        j        .Lsec_40_afe
.Lsec_40_ad0:
        beqi     a8, 16, .Lsec_40_b35
        addi     a7, a9, -5
        add.n    a12, a6, a10
        s32i.n   a12, a1, 44
        slli     a7, a7, 2
        extui    a7, a7, 0, 8
        add.n    a9, a5, a10
        nop.n
        l32i.n   a12, a1, 32
        sub      a13, a12, a7
        movi.n   a12, 6
        s8i      a12, a9, 0
        memw
        movi.n   a9, 6
        nop.n
        nop.n
        nop.n
        l32i.n   a12, a1, 44
        s8i      a13, a12, 0
.Lsec_40_afe:
        add.n    a9, a3, a9
        sub      a7, a2, a7
        s8i      a7, a9, 0
        memw
.Lsec_40_b09:
        addi     a7, a8, -8
        extui    a7, a7, 0, 8
        bgeui    a7, 8, .Lsec_40_b24
        add.n    a7, a5, a10
        nop.n
        l8ui     a9, a7, 0
        bnei     a9, 5, .Lsec_40_b24
        movi.n   a9, 6
        s8i      a9, a7, 0
        memw
.Lsec_40_b24:
        addi.n   a8, a8, 1
        extui    a8, a8, 0, 8
        addi.n   a10, a10, 1
        addi.n   a4, a4, 1
        addi.n   a11, a11, -1
        bnez     a11, .Lsec_40_a44
        j        .Lsec_40_b77
.Lsec_40_b35:
        addi     a2, a9, -5
        slli     a2, a2, 2
        l32i.n   a12, a1, 32
        extui    a2, a2, 0, 8
        sub      a4, a12, a2
        add.n    a6, a6, a10
        l32i.n   a5, a1, 40
        sub      a2, a5, a2
        s8i      a4, a6, 0
        s8i      a2, a3, 7
        memw
        j        .Lsec_40_b77
.Lsec_40_b56:
        l32i.n   a9, a1, 36
        addi.n   a8, a1, 6
        add.n    a2, a8, a9
        add.n    a6, a6, a10
        add.n    a5, a5, a10
        l8ui     a2, a2, 0
        s8i      a9, a5, 0
        s8i      a2, a6, 0
        memw
        j        .Lsec_40_a85
.Lsec_40_b6f:
        l32i.n   a9, a1, 36
        bltui    a9, 6, .Lsec_40_b56
        j        .Lsec_40_b35
.Lsec_40_b77:
        movi.n   a10, 1
        call8    write_txrate_power_offset
        call8    tx_gain_table_set
        movi.n   a2, 0
        retw.n
        .size   set_chan_dig_gain, .-set_chan_dig_gain
.Lsec_40_b83:
        .byte    0x00
        .literal_position
        .literal .Lsec_40_b84, 0x0000fffe
        .literal .Lsec_40_b88, 0x3ff4e060
        .literal .Lsec_40_b8c, g_phyFuns
        .literal .Lsec_40_b90, pwrdet_offset
        .literal .Lsec_40_b94, .Lsec_83_0
        .literal .Lsec_40_b98, phy_chan_target_power
        .literal .Lsec_40_b9c, phy_tx_pwr_error
        .literal .Lsec_40_ba0, tx_pwctrl_track_num
        .literal .Lsec_40_ba4, chip7_sleep_params
        .literal .Lsec_40_ba8, phy_tx_power_out
        .literal .Lsec_40_bac, btpwr_tsen_init
        .literal .Lsec_40_bb0, phy_tx_pwr_correct_old
        .literal .Lsec_40_bb4, phy_tx_pwr_error_old
        .literal .Lsec_40_bb8, tx_rf_ana_gain
        .literal .Lsec_40_bbc, 0x3ff46008
        .literal .Lsec_40_bc0, .Lsec_47_0
        .literal .Lsec_40_bc4, .Lsec_47_8c
        .literal .Lsec_40_bc8, lr_enable
        .literal .Lsec_40_bcc, esp_dport_access_reg_read
        .literal .Lsec_40_bd0, tsens_code_read
        .literal .Lsec_40_bd4, get_temp_cal
        .literal .Lsec_40_bd8, esp_dport_access_reg_read
        .literal .Lsec_40_bdc, phy_printf
        .literal .Lsec_40_be0, phy_printf
.Lsec_40_be4:
        .global tx_pwctrl_cal
        .type   tx_pwctrl_cal, @function
tx_pwctrl_cal:
        entry    a1, 96
        l32r     a10, .Lsec_40_b88
        extui    a5, a5, 0, 8
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_40_bcc
        callx8   a8
        extui    a2, a10, 24, 8
        extui    a3, a2, 0, 7
        l32r     a7, .Lsec_40_b8c
        extui    a6, a6, 0, 8
        l32i     a9, a7, 0
        l32r     a10, .Lsec_40_b90
        l32i     a9, a9, 0x19c
        l16ui    a10, a10, 0
        s32i.n   a3, a1, 32
        callx8   a9
        srli     a3, a3, 2
        extui    a9, a2, 0, 2
        bgeui    a3, 8, .Lsec_40_c1c
        j        .Lsec_40_f19
.Lsec_40_c1c:
        movi.n   a2, 23
        bltu     a2, a3, .Lsec_40_c2a
        l32r     a13, .Lsec_40_b84
        bnei     a9, 3, .Lsec_40_c2a
        j        .Lsec_40_f2d
.Lsec_40_c2a:
        movi.n   a13, 0
        j        .Lsec_40_f2d
        .byte    0x00
.Lsec_40_c30:
        movi.n   a2, 15
        bltu     a2, a3, .Lsec_40_c46
        addi     a2, a3, -8
        l32r     a9, .Lsec_40_b94
        extui    a2, a2, 0, 8
        add.n    a2, a9, a2
        l8ui     a2, a2, 0
        j        .Lsec_40_c65
.Lsec_40_c46:
        movi.n   a10, 23
        movi.n   a2, 0
        bltu     a10, a3, .Lsec_40_c65
        addi     a2, a3, -16
        extui    a2, a2, 0, 8
        bnei     a9, 3, .Lsec_40_c65
        addi     a2, a3, -8
        extui    a2, a2, 0, 8
        j        .Lsec_40_c65
.Lsec_40_c5f:
        movi.n   a8, 0
        movi.n   a2, 16
        s32i.n   a8, a1, 32
.Lsec_40_c65:
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a9, .Lsec_40_b98
        add.n    a2, a9, a2
        l8ui     a9, a2, 0
        sext     a9, a9, 7
        slli     a2, a9, 2
        sub      a13, a13, a2
        l32r     a2, .Lsec_40_b9c
        sext     a13, a13, 15
        s16i     a13, a2, 0
        memw
        movi.n   a2, 31
        blt      a2, a9, .Lsec_40_c92
        j        .Lsec_40_ee8
.Lsec_40_c92:
        movi.n   a2, -32
        movi.n   a10, 32
        max      a2, a13, a2
        min      a2, a2, a10
        l32r     a10, .Lsec_40_ba0
        l8ui     a10, a10, 0
        bgeui    a10, 4, .Lsec_40_ce8
        blti     a2, 1, .Lsec_40_cbc
        srai     a7, a2, 2
        add.n    a2, a7, a2
        srai     a2, a2, 2
        neg      a2, a2
        extui    a2, a2, 0, 8
        sext     a10, a2, 7
        j        .Lsec_40_d1d
.Lsec_40_cbc:
        movi.n   a7, -16
        blt      a2, a7, .Lsec_40_cc4
        j        .Lsec_40_efd
.Lsec_40_cc4:
        srai     a7, a2, 2
        sub      a2, a2, a7
        l32r     a7, .Lsec_40_ba4
        srai     a2, a2, 2
        neg      a2, a2
        movi.n   a10, 19
        extui    a2, a2, 0, 8
        l8ui     a7, a7, 184
        sext     a7, a7, 7
        blt      a10, a7, .Lsec_40_ce4
        j        .Lsec_40_f6a
.Lsec_40_ce4:
        j        .Lsec_40_f04
        .byte    0x00
.Lsec_40_ce8:
        mov.n    a10, a2
.Lsec_40_cea:
        l32i.n   a2, a7, 0
        movi.n   a13, 1
        movi.n   a12, 4
        mov.n    a11, a4
        l32i     a2, a2, 0x1a0
        s32i.n   a9, a1, 60
        s32i.n   a14, a1, 48
        callx8   a2
        extui    a2, a10, 0, 8
        sext     a10, a2, 7
        l32i.n   a9, a1, 60
        l32i.n   a14, a1, 48
        blti     a10, 1, .Lsec_40_d1d
        movi.n   a11, 19
        l32r     a7, .Lsec_40_ba4
        l8ui     a7, a7, 184
        sext     a7, a7, 7
        bge      a11, a7, .Lsec_40_d1a
        j        .Lsec_40_f04
.Lsec_40_d1a:
        j        .Lsec_40_d8a
.Lsec_40_d1d:
        movi.n   a7, 1
        movi.n   a11, 0
        moveqz   a11, a7, a10
        extui    a7, a11, 0, 8
.Lsec_40_d27:
        l16si    a10, a14, 0
        movi     a11, 0x130
        blt      a11, a10, .Lsec_40_d33
        j        .Lsec_40_f64
.Lsec_40_d33:
        beqz     a7, .Lsec_40_f64
.Lsec_40_d36:
        s32i.n   a9, a1, 60
        movi     a7, 0
        s32i     a14, a1, 48
        .reloc  ., R_XTENSA_ASM_EXPAND, tsens_code_read
        l32r     a8, .Lsec_40_bd0
        callx8   a8
        mov.n    a11, a10
        l32r     a2, .Lsec_40_bac
        l8ui     a10, a2, 0
        mov.n    a2, a7
        .reloc  ., R_XTENSA_ASM_EXPAND, get_temp_cal
        l32r     a8, .Lsec_40_bd4
        callx8   a8
        neg      a10, a10
        sext     a10, a10, 15
        l32i.n   a9, a1, 60
        l32i.n   a14, a1, 48
        blt      a10, a7, .Lsec_40_d64
        j        .Lsec_40_f10
.Lsec_40_d64:
        movi.n   a11, 1
        mov.n    a8, a7
        movnez   a8, a11, a5
        l32r     a2, .Lsec_40_ba4
        l8ui     a2, a2, 184
        sext     a2, a2, 7
        sub      a2, a10, a2
        srai     a2, a2, 31
        extui    a2, a2, 0, 8
        sext     a10, a2, 7
        movnez   a7, a11, a10
        and      a7, a7, a8
        j        .Lsec_40_f10
        .byte    0x00
.Lsec_40_d8a:
        l32r     a8, .Lsec_40_b9c
        movi.n   a7, -12
        l16si    a13, a8, 0
        blt      a13, a7, .Lsec_40_d9c
        movi.n   a11, 1
        movi.n   a7, 0
        beq      a10, a11, .Lsec_40_db0
.Lsec_40_d9c:
        movi.n   a7, 0
        movi.n   a11, 1
        mov.n    a8, a7
        movnez   a8, a11, a10
        moveqz   a11, a7, a5
        mov.n    a5, a11
        and      a7, a8, a11
        j        .Lsec_40_de9
.Lsec_40_db0:
        l32r     a2, .Lsec_40_bb0
        l8ui     a2, a2, 0
        sext     a2, a2, 7
        beqi     a2, 1, .Lsec_40_dcd
        mov.n    a2, a7
        movnez   a2, a11, a5
        mov.n    a5, a2
        extui    a7, a2, 0, 8
        mov.n    a2, a11
        j        .Lsec_40_de9
        .byte    0x00
        .byte    0x00
.Lsec_40_dcd:
        l32r     a10, .Lsec_40_bb4
        l16si    a10, a10, 0
        addi.n   a10, a10, 1
        blt      a10, a13, .Lsec_40_dda
        mov.n    a2, a7
.Lsec_40_dda:
        movi.n   a7, 1
        movi.n   a10, 0
        movnez   a10, a7, a5
        extui    a2, a2, 0, 8
        mov.n    a5, a10
        and      a7, a2, a10
.Lsec_40_de9:
        beqz     a6, .Lsec_40_e8c
        l32r     a5, .Lsec_40_ba4
        l8ui     a10, a5, 184
        sext     a10, a10, 7
        s32i.n   a10, a1, 36
        nop.n
        nop.n
        nop.n
        nop.n
        l8ui     a10, a5, 81
        sext     a10, a10, 7
        l16si    a12, a14, 0
        l16si    a15, a4, 0
        l16si    a5, a5, 182
        l32r     a11, .Lsec_40_bb8
        s32i.n   a10, a1, 44
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a10, .Lsec_40_bbc
        l16ui    a4, a11, 2
        s32i.n   a5, a1, 40
        nop.n
        nop.n
        nop.n
        nop.n
        l16ui    a5, a11, 0
        s32i.n   a9, a1, 60
        s32i.n   a12, a1, 48
        s32i.n   a13, a1, 56
        s32i     a15, a1, 52
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_40_bd8
        callx8   a8
        srli     a10, a10, 8
        sext     a10, a10, 7
        neg      a10, a10
        l32i.n   a9, a1, 60
        sext     a14, a2, 7
        mov.n    a11, a9
        l32i.n   a8, a1, 44
        s32i.n   a3, a1, 20
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a9, a1, 32
        s32i.n   a5, a1, 8
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a3, a1, 40
        l32i.n   a5, a1, 36
        s32i.n   a10, a1, 28
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a15, a1, 52
        l32i.n   a13, a1, 56
        l32i.n   a12, a1, 48
        l32r     a10, .Lsec_40_bc0
        s32i.n   a8, a1, 24
        s32i.n   a9, a1, 16
        s32i.n   a4, a1, 12
        s32i.n   a3, a1, 4
        s32i.n   a5, a1, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_40_bdc
        callx8   a8
.Lsec_40_e8c:
        beqz     a7, .Lsec_40_f70
        extui    a7, a2, 0, 8
        l32r     a3, .Lsec_40_ba4
        l32r     a9, .Lsec_40_b9c
        l8ui     a5, a3, 184
        add.n    a5, a7, a5
        l8ui     a4, a3, 185
        add.n    a4, a7, a4
        l16ui    a8, a9, 0
        l32r     a7, .Lsec_40_bb4
        s16i     a8, a7, 0
        memw
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a7, .Lsec_40_bb0
        s8i      a5, a3, 184
        s8i      a2, a7, 0
        s8i      a4, a3, 185
        memw
        call8    tx_gain_table_set
        beqz     a6, .Lsec_40_f70
        l32r     a2, .Lsec_40_bb8
        l8ui     a11, a3, 184
        sext     a11, a11, 7
        l16si    a14, a3, 182
        l16ui    a13, a2, 2
        l16ui    a12, a2, 0
        l32r     a10, .Lsec_40_bc4
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_40_be0
        callx8   a8
        retw.n
.Lsec_40_ee8:
        l32r     a2, .Lsec_40_ba0
        movi.n   a10, 0
        l8ui     a2, a2, 0
        bltui    a2, 4, .Lsec_40_ef6
        j        .Lsec_40_cea
.Lsec_40_ef6:
        movi.n   a7, 1
        mov.n    a2, a10
        j        .Lsec_40_d27
.Lsec_40_efd:
        movi.n   a7, 1
        movi.n   a2, 0
        j        .Lsec_40_d27
.Lsec_40_f04:
        l16si    a2, a14, 0
        movi     a7, 0x130
        bge      a7, a2, .Lsec_40_f48
        j        .Lsec_40_d36
.Lsec_40_f10:
        l32r     a5, .Lsec_40_b9c
        l16si    a13, a5, 0
        j        .Lsec_40_de9
.Lsec_40_f19:
        addi     a13, a10, -8
        l32r     a14, .Lsec_40_ba8
        extui    a13, a13, 0, 16
        movi.n   a2, 16
        s16i     a13, a14, 0
        memw
        j        .Lsec_40_c65
.Lsec_40_f2d:
        l32r     a2, .Lsec_40_bc8
        sub      a13, a10, a13
        extui    a13, a13, 0, 16
        l32r     a14, .Lsec_40_ba8
        l8ui     a2, a2, 0
        s16i     a13, a14, 0
        memw
        beqz     a2, .Lsec_40_c30
        j        .Lsec_40_f55
.Lsec_40_f48:
        l32r     a8, .Lsec_40_b9c
        movi.n   a7, 0
        mov.n    a2, a7
        l16si    a13, a8, 0
        j        .Lsec_40_de9
.Lsec_40_f55:
        addi     a2, a3, -9
        extui    a2, a2, 0, 8
        bltui    a2, 2, .Lsec_40_f61
        j        .Lsec_40_c30
.Lsec_40_f61:
        j        .Lsec_40_c5f
.Lsec_40_f64:
        l32r     a8, .Lsec_40_b9c
        l16si    a13, a8, 0
.Lsec_40_f6a:
        sext     a10, a2, 7
        j        .Lsec_40_d9c
.Lsec_40_f70:
        retw.n
        .size   tx_pwctrl_cal, .-tx_pwctrl_cal
.Lsec_40_f72:
        .byte    0x00
        .byte    0x00
        .literal_position
        .literal .Lsec_40_f74, phy_set_most_tpw_disbg
        .literal .Lsec_40_f78, phy_dis_pwdet_one
        .literal .Lsec_40_f7c, 0x3ff4e050
        .literal .Lsec_40_f80, phy_pwdet_onetime_flag
        .literal .Lsec_40_f84, phy_in_vdd33_offset
        .literal .Lsec_40_f88, chip7_sleep_params+0xb4
        .literal .Lsec_40_f8c, chip7_phy_init_ctrl+0x14
        .literal .Lsec_40_f90, tx_pwctrl_track_num
        .literal .Lsec_40_f94, phy_tx_power_out
        .literal .Lsec_40_f98, phy_enter_critical
        .literal .Lsec_40_f9c, esp_dport_access_reg_read
        .literal .Lsec_40_fa0, esp_dport_access_reg_read
        .literal .Lsec_40_fa4, esp_dport_access_reg_read
        .literal .Lsec_40_fa8, esp_dport_access_reg_read
        .literal .Lsec_40_fac, esp_dport_access_reg_read
        .literal .Lsec_40_fb0, wifi_track_pll_cap
        .literal .Lsec_40_fb4, phy_exit_critical
.Lsec_40_fb8:
        .global tx_pwctrl_background
        .type   tx_pwctrl_background, @function
tx_pwctrl_background:
        entry    a1, 48
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_enter_critical
        l32r     a8, .Lsec_40_f98
        callx8   a8
        l32r     a4, .Lsec_40_f74
        mov.n    a7, a10
        extui    a2, a2, 0, 8
        extui    a3, a3, 0, 8
        l8ui     a4, a4, 0
        beqz.n   a4, .Lsec_40_fdd
        l32r     a4, .Lsec_40_f78
        l32r     a5, .Lsec_40_f7c
        l32r     a6, .Lsec_40_f80
        j        .Lsec_40_105c
.Lsec_40_fdd:
        l32r     a4, .Lsec_40_f78
        l8ui     a8, a4, 0
        l32r     a5, .Lsec_40_f7c
        l32r     a6, .Lsec_40_f80
        bnez     a8, .Lsec_40_105c
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_40_f9c
        callx8   a8
        bltz     a10, .Lsec_40_100e
        l8ui     a8, a6, 0
        bnei     a8, 1, .Lsec_40_100e
.Lsec_40_ffd:
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_40_fa0
        callx8   a8
        extui    a10, a10, 24, 3
        bnez     a10, .Lsec_40_1051
        j        .Lsec_40_101c
.Lsec_40_100e:
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_40_fa4
        callx8   a8
        bbsi     a10, 0, .Lsec_40_ffd
        j        .Lsec_40_1051
.Lsec_40_101c:
        l32r     a8, .Lsec_40_f84
        mov.n    a10, a1
        l8ui     a11, a8, 0
        sext     a11, a11, 7
        call8    get_phy_target_power
        mov.n    a14, a3
        l32r     a12, .Lsec_40_f88
        mov.n    a13, a2
        mov.n    a10, a1
        l32r     a11, .Lsec_40_f8c
        call8    tx_pwctrl_cal
        movi.n   a8, 19
        l32r     a3, .Lsec_40_f90
        l8ui     a2, a3, 0
        bltu     a8, a2, .Lsec_40_105c
        addi.n   a2, a2, 1
        s8i      a2, a3, 0
        memw
        j        .Lsec_40_105c
        .byte    0x00
        .byte    0x00
.Lsec_40_1051:
        l32r     a2, .Lsec_40_f94
        movi.n   a3, 0
        s16i     a3, a2, 0
        memw
.Lsec_40_105c:
        mov.n    a10, a5
        nop.n
        nop.n
        nop.n
        l8ui     a8, a6, 0
        bnei     a8, 1, .Lsec_40_1080
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_40_fa8
        callx8   a8
        bltz     a10, .Lsec_40_108c
        movi.n   a2, 0
        s8i      a2, a6, 0
        memw
        j        .Lsec_40_108c
        .byte    0x00
        .byte    0x00
.Lsec_40_1080:
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_40_fac
        callx8   a8
        bbsi     a10, 0, .Lsec_40_108c
        call8    phy_pwdet_onetime_en
.Lsec_40_108c:
        l8ui     a5, a4, 0
        beqz.n   a5, .Lsec_40_1099
        movi.n   a5, 0
        s8i      a5, a4, 0
        memw
.Lsec_40_1099:
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_track_pll_cap
        l32r     a8, .Lsec_40_fb0
        callx8   a8
        or       a10, a7, a7
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_exit_critical
        l32r     a8, .Lsec_40_fb4
        callx8   a8
        retw.n
        .size   tx_pwctrl_background, .-tx_pwctrl_background
.Lsec_40_10aa:
        .byte    0x00
        .byte    0x00
        .literal_position
        .literal .Lsec_40_10ac, .Lsec_84_0
.Lsec_40_10b0:
        .global ram_get_rf_gain_qdb
        .type   ram_get_rf_gain_qdb, @function
ram_get_rf_gain_qdb:
        entry    a1, 32
        addi.n   a2, a2, -1
        extui    a8, a2, 0, 8
        movi.n   a2, 5
        bgeui    a8, 6, .Lsec_40_10c5
        l32r     a2, .Lsec_40_10ac
        add.n    a8, a2, a8
        l8ui     a2, a8, 0
.Lsec_40_10c5:
        retw.n
        .size   ram_get_rf_gain_qdb, .-ram_get_rf_gain_qdb
.Lsec_40_10c7:
        .byte    0x00
        .literal_position
        .literal .Lsec_40_10c8, .Lsec_83_0
.Lsec_40_10cc:
        .global wifi_11g_rate_chg
        .type   wifi_11g_rate_chg, @function
wifi_11g_rate_chg:
        entry    a1, 32
        addi     a2, a2, -8
        extui    a8, a2, 0, 8
        movi.n   a2, 0
        bgeui    a8, 8, .Lsec_40_10e2
        l32r     a2, .Lsec_40_10c8
        add.n    a8, a2, a8
        l8ui     a2, a8, 0
.Lsec_40_10e2:
        retw.n
        .size   wifi_11g_rate_chg, .-wifi_11g_rate_chg
        .literal_position
        .literal .Lsec_40_10e4, g_phyFuns
.Lsec_40_10e8:
        .global ram_set_txcap_reg
        .type   ram_set_txcap_reg, @function
ram_set_txcap_reg:
        entry    a1, 48
        extui    a3, a3, 2, 6
        addi.n   a12, a1, 4
        addi.n   a14, a2, 3
        add.n    a13, a1, a3
        j        .Lsec_40_111c
        .byte    0x00
.Lsec_40_10f8:
        l8ui     a11, a9, 0
        addi.n   a9, a9, 3
        s8i      a11, a10, 0
        memw
        addi.n   a10, a10, 1
        addi.n   a8, a8, -1
        bnez     a8, .Lsec_40_10f8
        addi.n   a2, a2, 1
        nop.n
        l8ui     a8, a13, 0
        s8i      a8, a12, 0
        memw
        addi.n   a12, a12, 1
        beq      a2, a14, .Lsec_40_1125
.Lsec_40_111c:
        mov.n    a10, a1
        mov.n    a9, a2
        movi.n   a8, 4
        j        .Lsec_40_10f8
.Lsec_40_1125:
        l32r     a3, .Lsec_40_10e4
        movi     a2, 96
        movi.n   a12, 1
        movi.n   a11, 2
        movi     a10, 107
        l32i     a8, a3, 0
        l8ui     a13, a1, 4
        or       a13, a13, a2
        extui    a13, a13, 0, 8
        l32i     a2, a8, 160
        callx8   a2
        movi.n   a12, 2
        mov.n    a11, a12
        l8ui     a2, a1, 6
        movi     a10, 107
        sext     a2, a2, 7
        slli     a2, a2, 4
        l32i.n   a3, a3, 0
        l8ui     a13, a1, 5
        or       a13, a2, a13
        extui    a13, a13, 0, 8
        l32i     a2, a3, 160
        callx8   a2
        retw.n
        .size   ram_set_txcap_reg, .-ram_set_txcap_reg
.Lsec_40_1167:

        .section .phyiram.24,"ax",@progbits
        .align  4
.Lsec_41_0:
        .global get_rate_pwctrl_offset
        .type   get_rate_pwctrl_offset, @function
get_rate_pwctrl_offset:
        entry    a1, 32
        extui    a8, a2, 0, 8
        srli     a9, a8, 2
        movi.n   a2, 8
        bltu     a9, a2, .Lsec_41_1e
        extui    a8, a8, 0, 2
        bnei     a8, 3, .Lsec_41_1c
        movi     a8, 23
        movi.n   a2, -2
        bgeu     a8, a9, .Lsec_41_1e
.Lsec_41_1c:
        movi.n   a2, 0
.Lsec_41_1e:
        retw.n
        .size   get_rate_pwctrl_offset, .-get_rate_pwctrl_offset
.Lsec_41_20:

        .section .phyiram.25,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_42_0, 0x3ff5ccb8
        .literal .Lsec_42_4, 0x1fffffff
        .literal .Lsec_42_8, 0x20000000
        .literal .Lsec_42_c, 0x3ff5c85c
        .literal .Lsec_42_10, esp_dport_access_reg_read
        .literal .Lsec_42_14, esp_dport_access_reg_read
.Lsec_42_18:
        .global phy_set_bbfreq_init
        .type   phy_set_bbfreq_init, @function
phy_set_bbfreq_init:
        entry    a1, 32
        l32r     a3, .Lsec_42_0
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_42_10
        callx8   a8
        l32r     a8, .Lsec_42_4
        and      a10, a10, a8
        l32r     a8, .Lsec_42_8
        or       a8, a10, a8
        l32r     a2, .Lsec_42_c
        mov.n    a10, a2
        memw
        s32i.n   a8, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_42_14
        callx8   a8
        movi.n   a8, -16
        and      a10, a10, a8
        memw
        s32i.n   a10, a2, 0
        retw.n
        .size   phy_set_bbfreq_init, .-phy_set_bbfreq_init
.Lsec_42_4e:

        .section .phyiram.26,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_43_0, g_phyFuns
        .literal .Lsec_43_4, 0x3ff4e060
        .literal .Lsec_43_8, esp_dport_access_reg_read
.Lsec_43_c:
        .global ram_tx_pwctrl_bg_init
        .type   ram_tx_pwctrl_bg_init, @function
ram_tx_pwctrl_bg_init:
        entry    a1, 32
        l32r     a2, .Lsec_43_0
        l32i.n   a2, a2, 0
        l32i     a2, a2, 0x11c
        callx8   a2
        l32r     a2, .Lsec_43_4
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_43_8
        callx8   a8
        movi.n   a8, 2
        or       a10, a10, a8
        memw
        s32i.n   a10, a2, 0
        retw.n
        .size   ram_tx_pwctrl_bg_init, .-ram_tx_pwctrl_bg_init
.Lsec_43_31:

        .section .phyiram.27,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_44_0, 0x3ff48890
        .literal .Lsec_44_4, 0x08000000
        .literal .Lsec_44_8, 0x3ff66010
        .literal .Lsec_44_c, 0xefffffff
        .literal .Lsec_44_10, 0x3ff48894
        .literal .Lsec_44_14, 0x80000000
        .literal .Lsec_44_18, 0x8007ffff
        .literal .Lsec_44_1c, 0x3ff4882c
        .literal .Lsec_44_20, 0x3ff48838
        .literal .Lsec_44_24, 0x3fffffff
        .literal .Lsec_44_28, set_xpd_sar
        .literal .Lsec_44_2c, esp_dport_access_reg_read
        .literal .Lsec_44_30, esp_dport_access_reg_read
        .literal .Lsec_44_34, esp_dport_access_reg_read
        .literal .Lsec_44_38, esp_dport_access_reg_read
        .literal .Lsec_44_3c, esp_dport_access_reg_read
        .literal .Lsec_44_40, esp_dport_access_reg_read
        .literal .Lsec_44_44, esp_dport_access_reg_read
        .literal .Lsec_44_48, esp_dport_access_reg_read
        .literal .Lsec_44_4c, esp_dport_access_reg_read
.Lsec_44_50:
        .global pwdet_sar2_init
        .type   pwdet_sar2_init, @function
pwdet_sar2_init:
        entry    a1, 32
        movi     a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, set_xpd_sar
        l32r     a8, .Lsec_44_28
        callx8   a8
        l32r     a2, .Lsec_44_0
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_44_2c
        callx8   a8
        l32r     a3, .Lsec_44_4
        or       a3, a10, a3
        l32r     a4, .Lsec_44_8
        mov.n    a10, a4
        memw
        s32i.n   a3, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_44_30
        callx8   a8
        movi.n   a3, -5
        and      a3, a10, a3
        mov.n    a10, a2
        memw
        s32i     a3, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_44_34
        callx8   a8
        l32r     a8, .Lsec_44_c
        and      a8, a10, a8
        l32r     a3, .Lsec_44_10
        or       a10, a3, a3
        memw
        s32i     a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_44_38
        callx8   a8
        l32r     a2, .Lsec_44_14
        or       a2, a10, a2
        mov.n    a10, a3
        memw
        s32i.n   a2, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_44_3c
        callx8   a8
        l32r     a8, .Lsec_44_18
        and      a8, a10, a8
        l32r     a2, .Lsec_44_1c
        mov.n    a10, a2
        memw
        s32i.n   a8, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_44_40
        callx8   a8
        movi.n   a8, 12
        or       a8, a10, a8
        l32r     a3, .Lsec_44_20
        or       a10, a3, a3
        memw
        s32i     a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_44_44
        callx8   a8
        l32r     a8, .Lsec_44_24
        and      a8, a10, a8
        or       a10, a3, a3
        memw
        s32i     a8, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_44_48
        callx8   a8
        movi.n   a8, -4
        and      a8, a10, a8
        mov.n    a10, a2
        memw
        s32i.n   a8, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_44_4c
        callx8   a8
        movi     a3, -225
        and      a10, a10, a3
        movi     a3, 128
        or       a10, a10, a3
        memw
        s32i.n   a10, a2, 0
        retw.n
        .size   pwdet_sar2_init, .-pwdet_sar2_init
.Lsec_44_123:

        .section .phyiram.28,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_45_0, 0x3ff4e05c
        .literal .Lsec_45_4, 0xff7fffff
        .literal .Lsec_45_8, 0xffdfffff
        .literal .Lsec_45_c, 0x3ff4e050
        .literal .Lsec_45_10, 0x3ff460c0
        .literal .Lsec_45_14, esp_dport_access_reg_read
        .literal .Lsec_45_18, esp_dport_access_reg_read
        .literal .Lsec_45_1c, esp_dport_access_reg_read
        .literal .Lsec_45_20, esp_dport_access_reg_read
        .literal .Lsec_45_24, esp_dport_access_reg_read
        .literal .Lsec_45_28, pwdet_sar2_init
.Lsec_45_2c:
        .global ram_en_pwdet
        .type   ram_en_pwdet, @function
ram_en_pwdet:
        entry    a1, 32
        l32r     a3, .Lsec_45_0
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_45_14
        callx8   a8
        l32r     a2, .Lsec_45_4
        and      a2, a10, a2
        mov.n    a10, a3
        memw
        s32i.n   a2, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_45_18
        callx8   a8
        l32r     a8, .Lsec_45_8
        and      a8, a10, a8
        l32r     a2, .Lsec_45_c
        mov.n    a10, a2
        memw
        s32i     a8, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_45_1c
        callx8   a8
        movi.n   a3, -2
        and      a3, a10, a3
        l32r     a4, .Lsec_45_10
        mov.n    a10, a4
        memw
        s32i.n   a3, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_45_20
        callx8   a8
        movi     a3, 128
        or       a3, a10, a3
        mov.n    a10, a2
        memw
        s32i.n   a3, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_45_24
        callx8   a8
        movi     a3, 192
        or       a10, a10, a3
        memw
        s32i.n   a10, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, pwdet_sar2_init
        l32r     a8, .Lsec_45_28
        callx8   a8
        retw.n
        .size   ram_en_pwdet, .-ram_en_pwdet
.Lsec_45_9f:

        .section .rodata,"a",@progbits
        .align  4
.Lsec_46_0:
        .word   .Lsec_40_4bc
        .word   .Lsec_40_4c5
        .word   .Lsec_40_4ce
        .word   .Lsec_40_4d7
        .word   .Lsec_40_4e0
        .word   .Lsec_40_4e9
        .word   .Lsec_40_4f2
        .word   .Lsec_40_4fb
        .word   .Lsec_40_504
.Lsec_46_24:
        .word   .Lsec_40_53a
        .word   .Lsec_40_545
        .word   .Lsec_40_550
        .word   .Lsec_40_55b
        .word   .Lsec_40_566
        .word   .Lsec_40_571
        .word   .Lsec_40_57c
        .word   .Lsec_40_587
        .word   .Lsec_40_592
.Lsec_46_48:
        .byte   0x7f, 0x6f, 0x5f, 0x7a, 0x5a, 0x69, 0x75, 0x55, 0x74, 0x54, 0x44, 0x60, 0x40, 0x20, 0x10, 0x00
        .byte   0x00, 0x00, 0x00, 0x00
.Lsec_46_5c:
        .byte   0x0a, 0x00, 0x05, 0x00, 0x00, 0x00, 0xf8, 0xff, 0xee, 0xff, 0xe7, 0xff, 0xde, 0xff, 0xd4, 0xff
        .byte   0xc9, 0xff, 0xbf, 0xff, 0xb9, 0xff, 0xac, 0xff, 0xa0, 0xff, 0x8f, 0xff, 0x81, 0xff, 0x69, 0xff
        .byte   0x61, 0xff

        .section .rodata.str1.4,"aMS",@progbits,1
        .align  4
.Lsec_47_0:
        .ascii  "1,target_power,%"
        .ascii  "d, %d, p_error,%"
        .ascii  "d, correct_v,%d,"
        .ascii  " error_accum, %d"
        .ascii  ", tot_p,%d, coat"
        .ascii  "t,%d, rf_g,%x,%x"
        .ascii  ", txq_rate,%d, %"
        .ascii  "d, chan,%d, rate"
        .ascii  "_att,%d\012\000\000\000\000"
.Lsec_47_8c:
        .ascii  "T:%d,0x%x,0x%x,%"
        .ascii  "d\012\000\000"
.Lsec_47_a0:
        .ascii  " (%d,%d) \000\000\000"
.Lsec_47_ac:
        .ascii  "%d,%d \000\000"
.Lsec_47_b4:
        .ascii  "stage %d: CGAIN="
        .ascii  "%d FGAIN=%d, (%d"
        .ascii  ",%d) %d; \000\000\000"
.Lsec_47_e0:
        .ascii  "\012\000\000\000"
.Lsec_47_e4:
        .ascii  "%d, %d; \000\000\000\000"
.Lsec_47_f0:
        .ascii  "reg_i=%d, indata"
        .ascii  "=%d\012\000\000\000\000"
.Lsec_47_108:
        .ascii  "i=%02d  acc=%03d"
        .ascii  "  tgt=%03d  err="
        .ascii  "%03d  ser=%03d a"
        .ascii  "tt=%03d\012\000"

        .section .text.ram_index_to_txbbgain,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_48_0, .Lsec_82_0
.Lsec_48_4:
        .global ram_index_to_txbbgain
        .type   ram_index_to_txbbgain, @function
ram_index_to_txbbgain:
        entry    a1, 32
        addi     a2, a2, -1
        extui    a8, a2, 0, 8
        movi     a2, 0x100
        bgeui    a8, 4, .Lsec_48_1c
        l32r     a2, .Lsec_48_0
        addx2    a8, a8, a2
        l16ui    a2, a8, 0
.Lsec_48_1c:
        retw.n
        .size   ram_index_to_txbbgain, .-ram_index_to_txbbgain
.Lsec_48_1e:

        .section .text.ram_txdc_cal_v70,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_49_0, g_phyFuns
        .literal .Lsec_49_4, 0x3ff4e04c
        .literal .Lsec_49_8, 0xff000000
        .literal .Lsec_49_c, 0x00113cf1
        .literal .Lsec_49_10, 0x00113cf3
        .literal .Lsec_49_14, 0x01000000
        .literal .Lsec_49_18, 0x40000000
        .literal .Lsec_49_1c, 0x00113cf0
        .literal .Lsec_49_20, ets_delay_us
        .literal .Lsec_49_24, esp_dport_access_reg_read
        .literal .Lsec_49_28, esp_dport_access_reg_read
        .literal .Lsec_49_2c, esp_dport_access_reg_read
        .literal .Lsec_49_30, esp_dport_access_reg_read
        .literal .Lsec_49_34, esp_dport_access_reg_read
        .literal .Lsec_49_38, esp_dport_access_reg_read
.Lsec_49_3c:
        .global ram_txdc_cal_v70
        .type   ram_txdc_cal_v70, @function
ram_txdc_cal_v70:
        entry    a1, 64
        l32r     a4, .Lsec_49_0
        movi.n   a11, 2
        mov.n    a10, a11
        movi     a12, 0x100
        movi     a5, 1
        l32i     a3, a4, 0
        l32i     a3, a3, 184
        callx8   a3
        movi     a12, 0x100
        movi.n   a11, 2
        l32i.n   a3, a4, 0
        movi.n   a10, 3
        l32i     a3, a3, 184
        callx8   a3
        s32i.n   a2, a1, 12
        s32i.n   a5, a1, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a7, .Lsec_49_4
        l32r     a6, .Lsec_49_14
        j        .Lsec_49_1bd
.Lsec_49_79:
        l32r     a9, .Lsec_49_0
        extui    a3, a3, 0, 16
        mov.n    a12, a3
        movi.n   a10, 3
        extui    a2, a2, 0, 16
        l32i.n   a8, a9, 0
        l32i.n   a11, a1, 0
        l32i     a8, a8, 184
        callx8   a8
        mov.n    a12, a2
        l32r     a10, .Lsec_49_0
        l32i.n   a8, a10, 0
        movi.n   a10, 2
        l32i.n   a11, a1, 0
        l32i     a8, a8, 184
        callx8   a8
        movi.n   a10, 2
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_49_20
        callx8   a8
        mov.n    a10, a7
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_49_24
        callx8   a8
        l32r     a8, .Lsec_49_8
        and      a10, a10, a8
        l32r     a9, .Lsec_49_c
        or       a10, a10, a9
        memw
        s32i     a10, a7, 0
        or       a10, a7, a7
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_49_28
        callx8   a8
        l32r     a8, .Lsec_49_8
        and      a10, a10, a8
        l32r     a9, .Lsec_49_10
        or       a10, a10, a9
        memw
        s32i.n   a10, a7, 0
.Lsec_49_dd:
        mov.n    a10, a7
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_49_2c
        callx8   a8
        bnone    a10, a6, .Lsec_49_dd
        mov.n    a10, a7
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_49_30
        callx8   a8
        mov.n    a8, a10
        mov.n    a10, a7
        s32i.n   a8, a1, 16
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_49_34
        callx8   a8
        l32r     a9, .Lsec_49_18
        and      a10, a10, a9
        l32i.n   a8, a1, 16
        bltz     a8, .Lsec_49_118
        extui    a8, a5, 0, 16
        add.n    a2, a2, a8
        sext     a2, a2, 15
        movi     a9, 0x1ff
        min      a2, a2, a9
        j        .Lsec_49_126
.Lsec_49_118:
        extui    a8, a5, 0, 16
        sub      a2, a2, a8
        sext     a2, a2, 15
        movi.n   a9, 0
        max      a2, a2, a9
.Lsec_49_126:
        bnez.n   a10, .Lsec_49_138
        add.n    a3, a3, a8
        sext     a3, a3, 15
        movi     a10, 0x1ff
        min      a3, a3, a10
        j        .Lsec_49_143
        .byte    0x00
        .byte    0x00
.Lsec_49_138:
        sub      a3, a3, a8
        sext     a3, a3, 15
        movi.n   a8, 0
        max      a3, a3, a8
.Lsec_49_143:
        beqi     a5, 2, .Lsec_49_14e
        srai     a5, a5, 1
        addi.n   a5, a5, 1
        j        .Lsec_49_150
.Lsec_49_14e:
        movi.n   a5, 1
.Lsec_49_150:
        bltui    a4, 8, .Lsec_49_166
        l32i.n   a8, a1, 8
        add.n    a9, a8, a2
        sext     a9, a9, 15
        l32i.n   a10, a1, 4
        add.n    a8, a10, a3
        sext     a8, a8, 15
        s32i.n   a9, a1, 8
        s32i     a8, a1, 4
.Lsec_49_166:
        addi.n   a4, a4, 1
        extui    a4, a4, 0, 8
        beqi     a4, 12, .Lsec_49_171
        j        .Lsec_49_79
.Lsec_49_171:
        nop.n
        nop.n
        l32r     a2, .Lsec_49_0
        movi.n   a10, 3
        l32i.n   a4, a1, 4
        l32i.n   a3, a2, 0
        addi.n   a2, a4, 2
        srai     a2, a2, 2
        extui    a12, a2, 0, 16
        l32i     a4, a3, 184
        l32i.n   a11, a1, 0
        l32i     a5, a1, 8
        callx8   a4
        addi.n   a3, a5, 2
        srai     a3, a3, 2
        extui    a12, a3, 0, 16
        l32r     a8, .Lsec_49_0
        movi.n   a10, 2
        l32i.n   a4, a8, 0
        l32i.n   a11, a1, 0
        addi.n   a9, a11, 1
        l32i     a4, a4, 184
        s32i.n   a9, a1, 0
        callx8   a4
        l32i.n   a4, a1, 12
        l32i.n   a5, a1, 0
        s16i     a3, a4, 0
        s16i     a2, a4, 2
        addi.n   a4, a4, 4
        s32i.n   a4, a1, 12
        beqi     a5, 3, .Lsec_49_1d0
.Lsec_49_1bd:
        movi.n   a8, 0
        movi     a2, 0x100
        movi     a5, 124
        s32i.n   a8, a1, 4
        mov.n    a4, a8
        mov.n    a3, a2
        s32i.n   a8, a1, 8
        j        .Lsec_49_79
.Lsec_49_1d0:
        mov.n    a10, a7
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_49_38
        callx8   a8
        l32r     a9, .Lsec_49_8
        and      a10, a10, a9
        l32r     a2, .Lsec_49_1c
        or       a10, a10, a2
        memw
        s32i.n   a10, a7, 0
        retw.n
        .size   ram_txdc_cal_v70, .-ram_txdc_cal_v70
.Lsec_49_1eb:

        .section .text.txcal_debuge_mode,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_50_0, g_phyFuns
        .literal .Lsec_50_4, tx_rf_ana_gain
        .literal .Lsec_50_8, chip7_sleep_params
.Lsec_50_c:
        .global txcal_debuge_mode
        .type   txcal_debuge_mode, @function
txcal_debuge_mode:
        entry    a1, 32
        l32r     a3, .Lsec_50_0
        extui    a2, a2, 0, 8
        slli     a2, a2, 8
        l32i     a4, a3, 0
        l32i     a4, a4, 192
        callx8   a4
        l32r     a4, .Lsec_50_4
        l32i     a8, a3, 0
        l16ui    a10, a4, 0
        or       a10, a2, a10
        l16ui    a11, a4, 2
        l32i     a2, a8, 216
        callx8   a2
        l32i.n   a2, a3, 0
        l16ui    a10, a4, 2
        l32i     a2, a2, 0x10c
        callx8   a2
        extui    a10, a10, 0, 16
        l32i.n   a2, a3, 0
        l32r     a4, .Lsec_50_8
        addx8    a10, a10, a4
        addi.n   a10, a10, 12
        l32i     a2, a2, 220
        callx8   a2
        movi.n   a10, 1
        l32i.n   a2, a3, 0
        l32i.n   a2, a2, 60
        callx8   a2
        l32i.n   a2, a3, 0
        l32i     a2, a2, 0x11c
        callx8   a2
        retw.n
        .size   txcal_debuge_mode, .-txcal_debuge_mode
.Lsec_50_67:

        .section .text.ram_txcal_work_mode,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_51_0, g_phyFuns
.Lsec_51_4:
        .global ram_txcal_work_mode
        .type   ram_txcal_work_mode, @function
ram_txcal_work_mode:
        entry    a1, 32
        l32r     a2, .Lsec_51_0
        movi.n   a10, 1
        l32i     a8, a2, 0
        l32i     a8, a8, 76
        callx8   a8
        movi.n   a10, 0
        l32i.n   a8, a2, 0
        l32i.n   a8, a8, 60
        callx8   a8
        movi.n   a10, 0
        l32i.n   a8, a2, 0
        l32i     a8, a8, 208
        callx8   a8
        l32i.n   a2, a2, 0
        l32i     a2, a2, 196
        callx8   a2
        retw.n
        .size   ram_txcal_work_mode, .-ram_txcal_work_mode
.Lsec_51_32:

        .section .text.ram_get_fm_sar_dout,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_52_0, 0x3ff4e050
        .literal .Lsec_52_4, g_phyFuns
        .literal .Lsec_52_8, esp_dport_access_reg_read
        .literal .Lsec_52_c, esp_dport_access_reg_read
        .literal .Lsec_52_10, ets_delay_us
        .literal .Lsec_52_14, esp_dport_access_reg_read
.Lsec_52_18:
        .global ram_get_fm_sar_dout
        .type   ram_get_fm_sar_dout, @function
ram_get_fm_sar_dout:
        entry    a1, 48
        l32r     a5, .Lsec_52_0
        movi.n   a4, -3
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_52_8
        callx8   a8
        and      a10, a10, a4
        memw
        s32i.n   a10, a5, 0
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_52_c
        callx8   a8
        movi.n   a4, 2
        or       a10, a10, a4
        memw
        s32i.n   a10, a5, 0
        movi.n   a10, 10
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_52_10
        callx8   a8
.Lsec_52_4a:
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_52_14
        callx8   a8
        extui    a4, a10, 24, 3
        bnez     a4, .Lsec_52_4a
        l32r     a5, .Lsec_52_4
        mov.n    a10, a1
        l32i.n   a5, a5, 0
        l32i     a5, a5, 0x12c
        callx8   a5
        l16ui    a9, a1, 12
        l16ui    a5, a1, 14
        add.n    a5, a9, a5
        extui    a5, a5, 0, 16
        l16ui    a12, a1, 6
        l16ui    a8, a1, 4
        add.n    a8, a12, a8
        extui    a8, a8, 0, 16
        l16ui    a13, a1, 8
        l16ui    a11, a1, 10
        add      a10, a13, a11
        extui    a10, a10, 0, 16
        bltu     a8, a5, .Lsec_52_90
        sub      a4, a8, a5
        sext     a4, a4, 15
.Lsec_52_90:
        s16i     a4, a2, 0
        memw
        movi.n   a4, 0
        bltu     a10, a5, .Lsec_52_a1
        sub      a5, a10, a5
        sext     a4, a5, 15
.Lsec_52_a1:
        add.n    a2, a12, a13
        add.n    a2, a2, a11
        add.n    a2, a2, a9
        srai     a2, a2, 2
        s16i     a4, a3, 0
        memw
        sext     a2, a2, 15
        retw.n
        .size   ram_get_fm_sar_dout, .-ram_get_fm_sar_dout
.Lsec_52_b5:

        .section .text.ram_txiq_get_mis_pwr,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_53_0, 0x3ff460b8
        .literal .Lsec_53_4, 0x002c0000
        .literal .Lsec_53_8, 0x0fffffff
        .literal .Lsec_53_c, 0xf0000000
        .literal .Lsec_53_10, 0x3ff450a8
        .literal .Lsec_53_14, g_phyFuns
        .literal .Lsec_53_18, 0xf0ffffff
        .literal .Lsec_53_1c, esp_dport_access_reg_read
        .literal .Lsec_53_20, esp_dport_access_reg_read
        .literal .Lsec_53_24, ets_delay_us
        .literal .Lsec_53_28, esp_dport_access_reg_read
        .literal .Lsec_53_2c, ets_delay_us
.Lsec_53_30:
        .global ram_txiq_get_mis_pwr
        .type   ram_txiq_get_mis_pwr, @function
ram_txiq_get_mis_pwr:
        entry    a1, 48
        mov.n    a9, a7
        l32r     a7, .Lsec_53_0
        or       a8, a5, a5
        or       a10, a7, a7
        extui    a5, a9, 0, 8
        sext     a4, a4, 15
        s32i     a8, a1, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_53_1c
        callx8   a8
        extui    a3, a3, 0, 8
        extui    a2, a2, 0, 8
        srai     a11, a4, 2
        l32r     a9, .Lsec_53_4
        neg      a3, a3
        or       a11, a11, a9
        extui    a3, a3, 0, 8
        slli     a9, a2, 26
        or       a9, a11, a9
        slli     a3, a3, 10
        l32r     a11, .Lsec_53_c
        or       a3, a9, a3
        and      a10, a10, a11
        extui    a4, a4, 0, 2
        l32r     a9, .Lsec_53_8
        and      a3, a3, a9
        or       a9, a3, a10
        l32r     a3, .Lsec_53_10
        mov.n    a10, a3
        memw
        s32i.n   a9, a7, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_53_20
        callx8   a8
        movi.n   a9, -4
        and      a10, a10, a9
        or       a4, a10, a4
        memw
        s32i.n   a4, a3, 0
        movi.n   a10, 10
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_53_24
        callx8   a8
        mov.n    a10, a5
        l32r     a4, .Lsec_53_14
        l32i.n   a3, a4, 0
        l32i     a3, a3, 0x134
        callx8   a3
        movi.n   a3, 1
        xor      a3, a2, a3
        l32i     a8, a1, 0
        extui    a3, a3, 0, 1
        slli     a2, a2, 3
        or       a2, a3, a2
        extui    a2, a2, 0, 4
        s16i     a10, a8, 0
        memw
        or       a10, a7, a7
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_53_28
        callx8   a8
        slli     a2, a2, 24
        l32r     a3, .Lsec_53_18
        and      a10, a10, a3
        or       a2, a2, a10
        movi.n   a10, 10
        memw
        s32i.n   a2, a7, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_53_2c
        callx8   a8
        mov.n    a10, a5
        l32i.n   a2, a4, 0
        l32i     a2, a2, 0x134
        callx8   a2
        s16i     a10, a6, 0
        memw
        retw.n
        .size   ram_txiq_get_mis_pwr, .-ram_txiq_get_mis_pwr
.Lsec_53_100:

        .section .text.ram_txiq_cover,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_54_0, g_phyFuns
.Lsec_54_4:
        .global ram_txiq_cover
        .type   ram_txiq_cover, @function
ram_txiq_cover:
        entry    a1, 80
        extui    a2, a2, 0, 8
        s32i.n   a2, a1, 28
        addi     a2, a2, -12
        movi.n   a7, 0
        sext     a2, a2, 7
        sext     a3, a3, 15
        max      a2, a2, a7
        extui    a5, a5, 0, 8
        l32r     a6, .Lsec_54_0
        s32i.n   a3, a1, 16
        mov.n    a3, a7
        s32i.n   a2, a1, 32
        mov.n    a2, a7
        s32i     a7, a1, 24
        s32i.n   a5, a1, 20
        s32i     a7, a1, 36
.Lsec_54_30:
        bgeui    a7, 4, .Lsec_54_58
        sext     a10, a2, 7
        movi.n   a11, 1
        nop.n
        nop.n
        l32i     a5, a6, 0
        l32i     a2, a5, 0x128
        callx8   a2
        extui    a2, a10, 0, 8
        sext     a10, a3, 7
        l32i.n   a5, a6, 0
        movi.n   a11, 0
        l32i     a3, a5, 0x128
        callx8   a3
        extui    a3, a10, 0, 8
.Lsec_54_58:
        addi.n   a13, a1, 2
        l32i.n   a5, a6, 0
        mov.n    a14, a1
        movi.n   a10, 1
        l32i.n   a15, a1, 20
        l32i.n   a12, a1, 16
        l32i     a11, a1, 32
        l32i     a5, a5, 0x138
        callx8   a5
        movi.n   a10, 0
        mov.n    a14, a1
        l16si    a9, a1, 0
        addi.n   a13, a1, 2
        l16si    a5, a1, 2
        min      a8, a9, a5
        sub      a5, a9, a5
        movi.n   a9, 1
        moveqz   a8, a9, a8
        slli     a5, a5, 11
        quos     a8, a5, a8
        addi     a8, a8, 16
        srai     a8, a8, 5
        s8i      a8, a4, 0
        memw
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a5, a6, 0
        l32i.n   a15, a1, 20
        l32i     a5, a5, 0x138
        l32i.n   a12, a1, 16
        l32i.n   a11, a1, 28
        callx8   a5
        l16ui    a9, a1, 0
        sext     a10, a9, 15
        l16ui    a5, a1, 2
        sext     a8, a5, 15
        add.n    a5, a5, a9
        sub      a8, a8, a10
        sext     a5, a5, 15
        movi.n   a9, 1
        slli     a8, a8, 12
        moveqz   a5, a9, a5
        quos     a5, a8, a5
        addi     a5, a5, 16
        srai     a5, a5, 5
        s8i      a5, a4, 1
        memw
        sub      a8, a3, a5
        bgeui    a7, 3, .Lsec_54_f0
        extui    a3, a8, 0, 8
        nop.n
        nop.n
        l8ui     a5, a4, 0
        sub      a2, a2, a5
        extui    a2, a2, 0, 8
        j        .Lsec_54_159
.Lsec_54_f0:
        l32i.n   a9, a1, 24
        add.n    a5, a5, a9
        extui    a5, a5, 0, 8
        l8ui     a10, a4, 0
        l32i.n   a9, a1, 36
        l32i.n   a8, a6, 0
        s32i.n   a5, a1, 24
        add.n    a5, a10, a9
        extui    a5, a5, 0, 8
        sext     a10, a10, 7
        s32i.n   a5, a1, 36
        nop.n
        nop.n
        nop.n
        nop
        l32i     a5, a8, 0x140
        callx8   a5
        blti     a10, 2, .Lsec_54_122
.Lsec_54_11c:
        bnei     a7, 6, .Lsec_54_159
        j        .Lsec_54_136
.Lsec_54_122:
        l32i.n   a5, a6, 0
        l8ui     a10, a4, 1
        sext     a10, a10, 7
        l32i     a5, a5, 0x140
        callx8   a5
        bgei     a10, 2, .Lsec_54_11c
        j        .Lsec_54_164
.Lsec_54_136:
        l32i.n   a5, a1, 36
        sext     a7, a5, 7
        addi.n   a7, a7, 2
        srai     a7, a7, 2
        l32i.n   a8, a1, 24
        sub      a2, a2, a7
        sext     a5, a8, 7
        addi.n   a5, a5, 2
        srai     a5, a5, 2
        sub      a3, a3, a5
        extui    a2, a2, 0, 8
        extui    a3, a3, 0, 8
        j        .Lsec_54_164
.Lsec_54_159:
        addi.n   a7, a7, 1
        extui    a7, a7, 0, 8
        beqi     a7, 7, .Lsec_54_164
        j        .Lsec_54_30
.Lsec_54_164:
        l32i.n   a5, a6, 0
        movi.n   a11, 1
        sext     a10, a2, 7
        l32i     a5, a5, 0x128
        callx8   a5
        movi     a11, 0
        sext     a10, a3, 7
        l32i     a5, a6, 0
        l32i     a5, a5, 0x128
        callx8   a5
        s8i      a2, a4, 0
        s8i      a3, a4, 1
        memw
        retw.n
        .size   ram_txiq_cover, .-ram_txiq_cover
.Lsec_54_18b:

        .section .text.rfcal_txiq,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_55_0, 0x3ff450dc
        .literal .Lsec_55_4, 0x00000800
        .literal .Lsec_55_8, 0xffffefff
        .literal .Lsec_55_c, g_phyFuns
        .literal .Lsec_55_10, 0x3ff460b8
        .literal .Lsec_55_14, 0x00001000
        .literal .Lsec_55_18, esp_dport_access_reg_read
        .literal .Lsec_55_1c, esp_dport_access_reg_read
        .literal .Lsec_55_20, txcal_debuge_mode
        .literal .Lsec_55_24, esp_dport_access_reg_read
        .literal .Lsec_55_28, ets_delay_us
        .literal .Lsec_55_2c, ram_txiq_cover
        .literal .Lsec_55_30, esp_dport_access_reg_read
.Lsec_55_34:
        .global rfcal_txiq
        .type   rfcal_txiq, @function
rfcal_txiq:
        entry    a1, 64
        l32r     a10, .Lsec_55_0
        extui    a7, a7, 0, 8
        extui    a2, a2, 0, 16
        extui    a5, a5, 0, 8
        s32i     a4, a1, 20
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_55_18
        callx8   a8
        extui    a6, a6, 0, 8
        l32r     a4, .Lsec_55_4
        or       a4, a10, a4
        l32r     a10, .Lsec_55_0
        memw
        s32i     a4, a10, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_55_1c
        callx8   a8
        movi.n   a4, 1
        addi.n   a8, a7, -1
        movi     a9, 0
        moveqz   a9, a4, a8
        l32r     a8, .Lsec_55_8
        and      a8, a10, a8
        or       a10, a9, a9
        l32r     a9, .Lsec_55_0
        memw
        s32i.n   a8, a9, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, txcal_debuge_mode
        l32r     a8, .Lsec_55_20
        callx8   a8
        mov.n    a12, a2
        movi     a11, 2
        or       a10, a4, a4
        l32r     a2, .Lsec_55_c
        l32i.n   a8, a2, 0
        l32i     a8, a8, 184
        callx8   a8
        bne      a7, a4, .Lsec_55_bc
        mov.n    a11, a7
        l32i.n   a4, a2, 0
        mov.n    a10, a7
        l32i     a8, a4, 188
        l32i     a4, a4, 184
        callx8   a8
        movi.n   a12, 2
        or       a12, a10, a12
        extui    a12, a12, 0, 16
        mov.n    a11, a7
        mov.n    a10, a7
        callx8   a4
        j        .Lsec_55_da
.Lsec_55_bc:
        bnei     a7, 2, .Lsec_55_da
        l32i.n   a8, a2, 0
        or       a10, a4, a4
        l32i     a4, a8, 40
        callx8   a4
        mov.n    a10, a3
        l32i.n   a3, a2, 0
        l32i     a3, a3, 0x118
        callx8   a3
        j        .Lsec_55_e5
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_55_da:
        l32i.n   a4, a2, 0
        or       a10, a3, a3
        l32i     a3, a4, 220
        callx8   a3
.Lsec_55_e5:
        movi.n   a4, 4
        l32r     a10, .Lsec_55_10
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_55_24
        callx8   a8
        s32i.n   a10, a1, 16
.Lsec_55_f2:
        movi.n   a15, 0
        extui    a3, a6, 0, 8
        mov.n    a12, a3
        mov.n    a14, a15
        l32i.n   a8, a2, 0
        mov.n    a13, a15
        mov.n    a11, a5
        movi     a10, 1
        l32i     a8, a8, 68
        callx8   a8
        movi.n   a10, 2
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_55_28
        callx8   a8
        l32i.n   a8, a2, 0
        movi.n   a10, 0
        l32i     a8, a8, 0x134
        callx8   a8
        srai     a10, a10, 2
        sext     a10, a10, 15
        movi     a9, 0x3e8
        addi.n   a8, a3, 4
        addi     a3, a3, -4
        bge      a9, a10, .Lsec_55_134
        extui    a6, a8, 0, 8
        j        .Lsec_55_13d
        .byte    0x00
.Lsec_55_134:
        movi     a8, 0x257
        blt      a8, a10, .Lsec_55_145
        extui    a6, a3, 0, 8
.Lsec_55_13d:
        addi.n   a4, a4, -1
        extui    a4, a4, 0, 8
        bnez     a4, .Lsec_55_f2
.Lsec_55_145:
        movi     a3, 120
        sext     a6, a6, 7
        min      a6, a6, a3
        movi.n   a10, 0
        or       a13, a10, a10
        or       a12, a1, a1
        max      a10, a6, a10
        or       a11, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, ram_txiq_cover
        l32r     a8, .Lsec_55_2c
        callx8   a8
        l32i.n   a3, a2, 0
        movi.n   a6, 15
        l32i     a3, a3, 0x124
        callx8   a3
        l8ui     a3, a1, 0
        sext     a4, a3, 7
        bge      a6, a4, .Lsec_55_181
        movi     a5, 0x3c0
        s8i      a6, a1, 0
        memw
        j        .Lsec_55_19d
.Lsec_55_181:
        movi.n   a5, -15
        blt      a4, a5, .Lsec_55_194
        extui    a3, a3, 0, 5
        slli     a3, a3, 6
        sext     a5, a3, 15
        j        .Lsec_55_19d
        .byte    0x00
        .byte    0x00
.Lsec_55_194:
        s8i      a5, a1, 0
        memw
        movi     a5, 0x440
.Lsec_55_19d:
        movi.n   a4, 31
        nop.n
        nop.n
        l8ui     a3, a1, 1
        sext     a6, a3, 7
        bge      a4, a6, .Lsec_55_1b8
        mov.n    a3, a4
        s8i      a4, a1, 1
        memw
        j        .Lsec_55_1ce
        .byte    0x00
.Lsec_55_1b8:
        movi.n   a4, -31
        blt      a6, a4, .Lsec_55_1c6
        extui    a4, a3, 0, 6
        sext     a3, a4, 15
        j        .Lsec_55_1ce
.Lsec_55_1c6:
        s8i      a4, a1, 1
        memw
        movi.n   a3, 33
.Lsec_55_1ce:
        or       a3, a5, a3
        nop.n
        nop.n
        l32i.n   a9, a1, 20
        l32i.n   a4, a1, 16
        s16i     a3, a9, 0
        memw
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a3, .Lsec_55_10
        memw
        s32i.n   a4, a3, 0
        bnei     a7, 2, .Lsec_55_201
        movi.n   a10, 0
        nop.n
        nop.n
        nop.n
        l32i.n   a2, a2, 0
        l32i.n   a2, a2, 40
        callx8   a2
.Lsec_55_201:
        l32r     a10, .Lsec_55_0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_55_30
        callx8   a8
        l32r     a2, .Lsec_55_14
        or       a10, a10, a2
        l32r     a8, .Lsec_55_0
        memw
        s32i.n   a10, a8, 0
        retw.n
        .size   rfcal_txiq, .-rfcal_txiq
.Lsec_55_21a:

        .section .text.ram_iq_est_enable,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_56_0, 0x3ff46060
        .literal .Lsec_56_4, 0xf3ffffff
        .literal .Lsec_56_8, 0x04000000
        .literal .Lsec_56_c, 0x3ff4607c
        .literal .Lsec_56_10, 0xffe7ffff
        .literal .Lsec_56_14, 0x00100000
        .literal .Lsec_56_18, 0xfffe0003
        .literal .Lsec_56_1c, esp_dport_access_reg_read
        .literal .Lsec_56_20, esp_dport_access_reg_read
        .literal .Lsec_56_24, esp_dport_access_reg_read
        .literal .Lsec_56_28, esp_dport_access_reg_read
        .literal .Lsec_56_2c, ets_delay_us
        .literal .Lsec_56_30, esp_dport_access_reg_read
        .literal .Lsec_56_34, esp_dport_access_reg_read
.Lsec_56_38:
        .global ram_iq_est_enable
        .type   ram_iq_est_enable, @function
ram_iq_est_enable:
        entry    a1, 32
        l32r     a4, .Lsec_56_0
        extui    a3, a3, 0, 16
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_1c
        callx8   a8
        l32r     a8, .Lsec_56_4
        and      a10, a10, a8
        l32r     a2, .Lsec_56_8
        or       a8, a10, a2
        l32r     a2, .Lsec_56_c
        mov.n    a10, a2
        memw
        s32i.n   a8, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_20
        callx8   a8
        l32r     a8, .Lsec_56_10
        and      a9, a10, a8
        or       a10, a2, a2
        l32r     a8, .Lsec_56_14
        or       a8, a9, a8
        memw
        s32i     a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_24
        callx8   a8
        extui    a8, a3, 0, 15
        slli     a8, a8, 2
        movi.n   a3, 1
        l32r     a9, .Lsec_56_18
        and      a10, a10, a9
        or       a8, a8, a10
        or       a10, a2, a2
        memw
        s32i     a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_28
        callx8   a8
        or       a4, a10, a3
        mov.n    a10, a3
        movi.n   a3, 2
        memw
        s32i.n   a4, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_56_2c
        callx8   a8
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_30
        callx8   a8
        or       a10, a10, a3
        memw
        s32i.n   a10, a2, 0
.Lsec_56_c2:
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_56_34
        callx8   a8
        bgez     a10, .Lsec_56_c2
        retw.n
        .size   ram_iq_est_enable, .-ram_iq_est_enable
.Lsec_56_cf:

        .section .text.ram_iq_est_disable,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_57_0, 0x3ff4607c
        .literal .Lsec_57_4, esp_dport_access_reg_read
        .literal .Lsec_57_8, ets_delay_us
        .literal .Lsec_57_c, esp_dport_access_reg_read
.Lsec_57_10:
        .global ram_iq_est_disable
        .type   ram_iq_est_disable, @function
ram_iq_est_disable:
        entry    a1, 32
        l32r     a2, .Lsec_57_0
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_57_4
        callx8   a8
        movi.n   a8, -3
        and      a10, a10, a8
        memw
        s32i.n   a10, a2, 0
        movi.n   a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_57_8
        callx8   a8
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_57_c
        callx8   a8
        movi.n   a8, -2
        and      a10, a10, a8
        memw
        s32i.n   a10, a2, 0
        retw.n
        .size   ram_iq_est_disable, .-ram_iq_est_disable
.Lsec_57_44:

        .section .text.ram_dc_iq_est,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_58_0, g_phyFuns
        .literal .Lsec_58_4, 0x3ff460dc
        .literal .Lsec_58_8, 0x3ff460e0
        .literal .Lsec_58_c, 0x3ff460e4
        .literal .Lsec_58_10, esp_dport_access_reg_read
        .literal .Lsec_58_14, esp_dport_access_reg_read
        .literal .Lsec_58_18, esp_dport_access_reg_read
.Lsec_58_1c:
        .global ram_dc_iq_est
        .type   ram_dc_iq_est, @function
ram_dc_iq_est:
        entry    a1, 32
        l32r     a5, .Lsec_58_0
        extui    a3, a3, 0, 16
        mov.n    a11, a3
        extui    a10, a2, 0, 8
        addi.n   a3, a3, 1
        l32i.n   a8, a5, 0
        l32i     a8, a8, 0x144
        callx8   a8
        l32r     a10, .Lsec_58_4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_58_10
        callx8   a8
        srai     a10, a10, 6
        quos     a8, a10, a3
        l32r     a10, .Lsec_58_8
        s32i.n   a8, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_58_14
        callx8   a8
        srai     a10, a10, 6
        quos     a8, a10, a3
        l32r     a10, .Lsec_58_c
        s32i     a8, a4, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_58_18
        callx8   a8
        quos     a3, a10, a3
        movi.n   a10, 0
        mov.n    a11, a10
        l32i.n   a8, a4, 0
        mull     a9, a8, a8
        l32i.n   a8, a4, 4
        mull     a8, a8, a8
        add.n    a8, a9, a8
        l32i.n   a9, a5, 0
        subx8    a3, a3, a8
        max      a10, a3, a10
        l32i     a3, a9, 0x174
        callx8   a3
        addi.n   a10, a10, 8
        srai     a10, a10, 4
        l32i.n   a3, a5, 0
        l32i     a3, a3, 0x148
        s32i.n   a10, a4, 8
        callx8   a3
        retw.n
        .size   ram_dc_iq_est, .-ram_dc_iq_est
.Lsec_58_92:

        .section .text.ram_pbus_rx_dco_cal,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_59_0, g_phyFuns
        .literal .Lsec_59_4, .Lsec_47_a0
        .literal .Lsec_59_8, .Lsec_47_ac
        .literal .Lsec_59_c, .Lsec_85_0
        .literal .Lsec_59_10, .Lsec_47_b4
        .literal .Lsec_59_14, .Lsec_47_e0
        .literal .Lsec_59_18, ets_delay_us
        .literal .Lsec_59_1c, phy_printf
        .literal .Lsec_59_20, phy_printf
        .literal .Lsec_59_24, phy_printf
        .literal .Lsec_59_28, phy_printf
        .literal .Lsec_59_2c, phy_printf
.Lsec_59_30:
        .global ram_pbus_rx_dco_cal
        .type   ram_pbus_rx_dco_cal, @function
ram_pbus_rx_dco_cal:
        entry    a1, 128
        l32r     a7, .Lsec_59_0
        extui    a2, a2, 0, 16
        extui    a4, a4, 0, 16
        movi.n   a11, 2
        movi.n   a10, 1
        s32i.n   a2, a1, 48
        extui    a5, a5, 0, 8
        extui    a6, a6, 0, 8
        nop.n
        nop.n
        l32i.n   a2, a7, 0
        l32i     a2, a2, 188
        s32i     a4, a1, 52
        s32i     a3, a1, 68
        s32i     a5, a1, 64
        s32i     a6, a1, 40
        callx8   a2
        extui    a2, a10, 0, 6
        mov.n    a4, a10
        beqz     a2, .Lsec_59_2f6
        movi.n   a3, 0
        s32i     a3, a1, 60
.Lsec_59_6d:
        extui    a5, a2, 0, 1
        add.n    a3, a3, a5
        srli     a2, a2, 1
        extui    a3, a3, 0, 8
        bnez     a2, .Lsec_59_6d
        l32i.n   a2, a7, 0
        movi.n   a11, 2
        mov.n    a10, a11
        movi     a12, 0x100
        extui    a4, a4, 6, 8
        l32i     a2, a2, 184
        movi.n   a5, 15
        s32i     a3, a1, 60
        s32i     a4, a1, 76
        callx8   a2
        movi     a12, 0x100
        movi.n   a11, 2
        movi.n   a10, 3
        l32i     a2, a7, 0
        l32i     a2, a2, 184
        callx8   a2
        bgeui    a3, 3, .Lsec_59_aa
        movi.n   a5, 5
.Lsec_59_aa:
        l32i.n   a4, a1, 60
        addi.n   a2, a4, 2
        extui    a3, a2, 0, 8
        movi.n   a2, 4
        bltui    a4, 3, .Lsec_59_b8
        mov.n    a2, a3
.Lsec_59_b8:
        extui    a2, a2, 0, 8
        addi.n   a2, a2, 2
        l32i.n   a8, a1, 60
        movi.n   a3, 5
        s32i.n   a2, a1, 56
        bgeui    a8, 4, .Lsec_59_c8
        movi.n   a3, 2
.Lsec_59_c8:
        extui    a3, a3, 0, 8
        nop.n
        nop.n
        l32i.n   a9, a1, 60
        addi.n   a2, a9, 6
        extui    a2, a2, 0, 8
        addi.n   a2, a2, 2
        s32i     a3, a1, 72
        movi.n   a3, 0
        nop.n
        nop.n
        nop.n
        l32i     a6, a1, 68
        s32i.n   a3, a1, 36
        s32i     a2, a1, 80
        j        .Lsec_59_f5
.Lsec_59_ee:
        l32i.n   a4, a1, 32
        l32i     a5, a1, 72
        s32i.n   a4, a1, 36
.Lsec_59_f5:
        movi.n   a8, 4
        movi     a2, 0x200
        mov.n    a3, a2
        nop.n
        l32i.n   a9, a1, 36
        s32i.n   a8, a1, 44
        bnez.n   a9, .Lsec_59_125
        nop.n
        nop.n
        nop.n
        nop.n
        l32i     a8, a1, 68
        movi.n   a9, 12
        l16si    a3, a8, 0
        slli     a3, a3, 1
        sext     a3, a3, 15
        l16si    a2, a8, 2
        slli     a2, a2, 1
        sext     a2, a2, 15
        s32i.n   a9, a1, 44
.Lsec_59_125:
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a4, a1, 36
        addi.n   a4, a4, 1
        s32i.n   a4, a1, 32
        movi     a4, 0
.Lsec_59_136:
        addi.n   a8, a3, 1
        srai     a8, a8, 1
        extui    a12, a8, 0, 16
        l32i.n   a9, a7, 0
        movi.n   a10, 2
        l32i     a9, a9, 184
        l32i.n   a11, a1, 32
        s16i     a8, a6, 0
        memw
        callx8   a9
        addi.n   a8, a2, 1
        srai     a8, a8, 1
        extui    a12, a8, 0, 16
        l32i.n   a9, a7, 0
        movi.n   a10, 3
        l32i     a9, a9, 184
        l32i.n   a11, a1, 32
        s16i     a8, a6, 2
        memw
        callx8   a9
        l32i.n   a10, a1, 52
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_59_18
        callx8   a8
        addi     a12, a1, 16
        movi.n   a10, 1
        l32i.n   a8, a7, 0
        l32i     a11, a1, 48
        l32i     a8, a8, 0x14c
        callx8   a8
        l32i.n   a8, a1, 40
        beqz.n   a8, .Lsec_59_1a2
        l16si    a12, a6, 2
        l16si    a11, a6, 0
        l32r     a10, .Lsec_59_4
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_59_1c
        callx8   a8
        l32i.n   a12, a1, 20
        l32i.n   a11, a1, 16
        l32r     a10, .Lsec_59_8
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_59_20
        callx8   a8
.Lsec_59_1a2:
        l32i.n   a8, a7, 0
        l32i     a10, a1, 16
        l32i     a8, a8, 0x140
        callx8   a8
        bge      a5, a10, .Lsec_59_1ba
.Lsec_59_1b0:
        l32i.n   a9, a1, 36
        bnez     a9, .Lsec_59_236
        j        .Lsec_59_1ca
        .byte    0x00
        .byte    0x00
.Lsec_59_1ba:
        l32i.n   a8, a7, 0
        l32i.n   a10, a1, 20
        l32i     a8, a8, 0x140
        callx8   a8
        blt      a5, a10, .Lsec_59_1b0
        j        .Lsec_59_2a5
.Lsec_59_1ca:
        l32i.n   a8, a7, 0
        l32i.n   a10, a1, 16
        l32i     a8, a8, 0x140
        callx8   a8
        bge      a5, a10, .Lsec_59_1fe
        l32i     a9, a1, 76
        l32r     a8, .Lsec_59_c
        add      a8, a8, a9
        l8ui     a9, a8, 0
        l32i     a8, a1, 16
        mull     a8, a9, a8
        addx2    a8, a8, a8
        l32i     a9, a1, 80
        slli     a8, a8, 1
        ssr      a9
        sra      a8, a8
        sub      a3, a3, a8
        sext     a3, a3, 15
.Lsec_59_1fe:
        l32i.n   a8, a7, 0
        l32i.n   a10, a1, 20
        l32i     a8, a8, 0x140
        callx8   a8
        bge      a5, a10, .Lsec_59_284
        l32i     a9, a1, 76
        l32r     a8, .Lsec_59_c
        add.n    a8, a8, a9
        l8ui     a9, a8, 0
        l32i.n   a8, a1, 20
        mull     a8, a9, a8
        addx2    a8, a8, a8
        l32i     a9, a1, 80
        slli     a8, a8, 1
        ssr      a9
        sra      a8, a8
        sub      a2, a2, a8
        sext     a2, a2, 15
        j        .Lsec_59_284
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_59_236:
        l32i.n   a8, a7, 0
        l32i.n   a10, a1, 16
        l32i     a8, a8, 0x140
        callx8   a8
        bge      a5, a10, .Lsec_59_25e
        l32i     a8, a1, 16
        addx4    a8, a8, a8
        subx8    a8, a8, a8
        slli     a8, a8, 3
        l32i     a9, a1, 56
        ssr      a9
        sra      a8, a8
        sub      a3, a3, a8
        sext     a3, a3, 15
.Lsec_59_25e:
        l32i.n   a8, a7, 0
        l32i.n   a10, a1, 20
        l32i     a8, a8, 0x140
        callx8   a8
        bge      a5, a10, .Lsec_59_284
        l32i.n   a8, a1, 20
        addx4    a8, a8, a8
        subx8    a8, a8, a8
        slli     a8, a8, 3
        l32i.n   a9, a1, 56
        ssr      a9
        sra      a8, a8
        sub      a2, a2, a8
        sext     a2, a2, 15
.Lsec_59_284:
        movi     a8, 0x3fe
        min      a3, a3, a8
        min      a2, a2, a8
        addi.n   a4, a4, 1
        l32i.n   a8, a1, 44
        movi.n   a9, 0
        extui    a4, a4, 0, 8
        max      a3, a3, a9
        max      a2, a2, a9
        beq      a8, a4, .Lsec_59_2a2
        j        .Lsec_59_136
.Lsec_59_2a2:
        j        .Lsec_59_2a7
.Lsec_59_2a5:
        s32i.n   a4, a1, 44
.Lsec_59_2a7:
        nop.n
        nop.n
        nop.n
        nop.n
        l32i     a9, a1, 64
        beqz.n   a9, .Lsec_59_2cc
        l32i.n   a2, a1, 44
        l32i     a13, a1, 76
        l32i.n   a12, a1, 60
        l32i.n   a11, a1, 32
        l32r     a10, .Lsec_59_10
        l32i.n   a15, a1, 20
        l32i.n   a14, a1, 16
        s32i.n   a2, a1, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_59_24
        callx8   a8
.Lsec_59_2cc:
        l32i.n   a3, a1, 40
        beqz.n   a3, .Lsec_59_2d9
        l32r     a10, .Lsec_59_14
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_59_28
        callx8   a8
.Lsec_59_2d9:
        addi.n   a6, a6, 4
        l32i.n   a4, a1, 32
        beqi     a4, 2, .Lsec_59_2e3
        j        .Lsec_59_ee
.Lsec_59_2e3:
        l32i     a8, a1, 64
        beqz.n   a8, .Lsec_59_321
        l32r     a10, .Lsec_59_14
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_59_2c
        callx8   a8
        retw.n
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_59_2f6:
        l32i.n   a3, a7, 0
        movi.n   a11, 2
        mov.n    a10, a11
        movi     a12, 0x100
        extui    a4, a4, 6, 8
        l32i     a3, a3, 184
        movi.n   a5, 5
        s32i     a4, a1, 76
        callx8   a3
        movi     a12, 0x100
        movi.n   a11, 2
        movi.n   a10, 3
        l32i.n   a3, a7, 0
        l32i     a3, a3, 184
        callx8   a3
        s32i.n   a2, a1, 60
        j        .Lsec_59_aa
.Lsec_59_321:
        retw.n
        .size   ram_pbus_rx_dco_cal, .-ram_pbus_rx_dco_cal
.Lsec_59_323:

        .section .text.rxdc_est_min,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_60_0, g_phyFuns
.Lsec_60_4:
        .global rxdc_est_min
        .type   rxdc_est_min, @function
rxdc_est_min:
        entry    a1, 48
        l32r     a5, .Lsec_60_0
        extui    a2, a2, 0, 16
        movi     a3, 100
        movi.n   a7, 0
        movi.n   a6, 47
.Lsec_60_14:
        l32i.n   a9, a5, 0
        mov.n    a12, a1
        mov.n    a11, a2
        movi.n   a10, 1
        l32i     a9, a9, 0x14c
        callx8   a9
        l32i.n   a9, a1, 8
        blt      a9, a3, .Lsec_60_29
        bnez.n   a7, .Lsec_60_60
.Lsec_60_29:
        l32i.n   a3, a1, 0
        movi.n   a10, 1
        s32i.n   a3, a4, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a3, a1, 4
        s32i.n   a9, a4, 8
        s32i.n   a3, a4, 4
        bge      a6, a9, .Lsec_60_42
        movi.n   a10, 0
.Lsec_60_42:
        movi.n   a3, 1
        bgeui    a7, 2, .Lsec_60_49
        movi.n   a3, 0
.Lsec_60_49:
        and      a3, a10, a3
        bbsi     a3, 0, .Lsec_60_66
        movi.n   a3, 35
        bge      a3, a9, .Lsec_60_66
        mov.n    a3, a9
.Lsec_60_56:
        addi.n   a8, a7, 1
        extui    a7, a8, 0, 8
        bnei     a7, 10, .Lsec_60_14
        retw.n
.Lsec_60_60:
        blt      a6, a3, .Lsec_60_56
        bltui    a7, 2, .Lsec_60_56
.Lsec_60_66:
        retw.n
        .size   rxdc_est_min, .-rxdc_est_min
.Lsec_60_68:

        .section .text.pbus_rx_dco_cal_1step,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_61_0, g_phyFuns
        .literal .Lsec_61_4, ets_delay_us
        .literal .Lsec_61_8, rxdc_est_min
        .literal .Lsec_61_c, ets_delay_us
        .literal .Lsec_61_10, rxdc_est_min
        .literal .Lsec_61_14, ets_delay_us
        .literal .Lsec_61_18, rxdc_est_min
.Lsec_61_1c:
        .global pbus_rx_dco_cal_1step
        .type   pbus_rx_dco_cal_1step, @function
pbus_rx_dco_cal_1step:
        entry    a1, 128
        extui    a3, a3, 0, 8
        s32i.n   a3, a1, 52
        movi.n   a2, 0
        movi.n   a11, 2
        extui    a4, a4, 0, 16
        movi.n   a10, 1
        l32i.n   a9, a1, 52
        addi.n   a8, a9, -1
        movi.n   a9, 16
        s32i.n   a5, a1, 40
        movi.n   a5, 8
        movnez   a5, a9, a8
        s32i     a7, a1, 88
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a7, .Lsec_61_0
        s32i     a5, a1, 68
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a3, a7, 0
        l32i.n   a5, a1, 40
        l32i.n   a8, a1, 40
        l32i     a3, a3, 188
        l16si    a8, a8, 2
        l16si    a5, a5, 0
        s32i.n   a2, a1, 12
        s32i.n   a2, a1, 16
        s32i.n   a2, a1, 20
        s32i.n   a2, a1, 0
        s32i.n   a2, a1, 4
        s32i.n   a2, a1, 8
        s32i     a4, a1, 64
        s32i     a8, a1, 36
        s32i     a5, a1, 32
        callx8   a3
        mov.n    a4, a10
        movi.n   a11, 1
        mov.n    a10, a2
        l32i.n   a3, a7, 0
        l32i     a3, a3, 188
        callx8   a3
        mov.n    a3, a2
        s32i     a10, a1, 72
        extui    a10, a4, 0, 8
        movi.n   a4, 6
        s32i     a2, a1, 76
        loop     a4, .Lsec_61_a8
        ssr      a3
        sra      a5, a10
        extui    a5, a5, 0, 1
        add.n    a2, a2, a5
        extui    a2, a2, 0, 8
        addi.n   a3, a3, 1
.Lsec_61_a8:
        l32i.n   a9, a1, 52
        movi.n   a4, 1
        addi.n   a3, a9, -1
        s32i     a2, a1, 76
        movi.n   a2, 0
        moveqz   a2, a4, a3
        extui    a2, a2, 0, 8
        nop.n
        l32i     a3, a1, 76
        bltui    a3, 3, .Lsec_61_cd
        beqz.n   a2, .Lsec_61_cd
        addi.n   a3, a3, -1
        extui    a5, a3, 0, 8
        j        .Lsec_61_cf
        .byte    0x00
        .byte    0x00
.Lsec_61_cd:
        movi.n   a5, 1
.Lsec_61_cf:
        movi.n   a4, 0
        addi.n   a2, a2, 1
        extui    a2, a2, 0, 8
        s32i     a4, a1, 84
        sext     a5, a5, 7
        nop.n
        nop.n
        nop.n
        l32i     a8, a1, 76
        addi.n   a3, a8, -1
        s32i.n   a2, a1, 56
        nop.n
        nop.n
        nop.n
        nop.n
        l32i     a2, a1, 84
        l32i     a9, a1, 88
        s32i.n   a2, a1, 44
        extui    a2, a3, 0, 8
        s8i      a4, a9, 0
        s32i     a2, a1, 80
.Lsec_61_102:
        movi.n   a10, 2
        nop.n
        nop.n
        nop.n
        l32i.n   a3, a1, 32
        extui    a3, a3, 0, 16
        mov.n    a12, a3
        l32i.n   a2, a7, 0
        l32i.n   a9, a1, 32
        l32i.n   a8, a1, 40
        s32i.n   a3, a1, 48
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a3, a1, 36
        l32i     a2, a2, 184
        l32i.n   a11, a1, 56
        s16i     a9, a8, 0
        s16i     a3, a8, 2
        memw
        callx8   a2
        movi.n   a10, 3
        l32i.n   a2, a7, 0
        l32i.n   a8, a1, 40
        l32i.n   a11, a1, 56
        l16ui    a12, a8, 2
        l32i     a2, a2, 184
        callx8   a2
        l32i.n   a9, a1, 52
        bnei     a9, 1, .Lsec_61_179
        movi.n   a10, 10
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_61_4
        callx8   a8
        l32i.n   a11, a1, 52
        or       a12, a6, a6
        l32i     a10, a1, 64
        .reloc  ., R_XTENSA_ASM_EXPAND, rxdc_est_min
        l32r     a8, .Lsec_61_8
        callx8   a8
        l32i.n   a2, a6, 8
        l32i     a3, a1, 76
        s32i.n   a2, a1, 60
        bgeui    a3, 2, .Lsec_61_1e1
        movi.n   a2, 2
        movi.n   a4, 0
        nop.n
        nop.n
        l32i.n   a10, a6, 0
        j        .Lsec_61_1e8
        .byte    0x00
        .byte    0x00
.Lsec_61_179:
        l32i.n   a2, a7, 0
        movi.n   a11, 2
        movi.n   a12, 0
        movi.n   a10, 1
        movi.n   a4, 0
        l32i     a2, a2, 184
        callx8   a2
        movi.n   a10, 10
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_61_c
        callx8   a8
        l32i     a10, a1, 64
        addi.n   a12, a1, 12
        movi.n   a11, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, rxdc_est_min
        l32r     a8, .Lsec_61_10
        callx8   a8
        movi.n   a11, 2
        l32i.n   a2, a7, 0
        movi.n   a12, 32
        movi.n   a10, 1
        l32i     a2, a2, 184
        callx8   a2
        movi.n   a10, 10
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_61_14
        callx8   a8
        l32i     a10, a1, 64
        mov.n    a12, a1
        movi.n   a11, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, rxdc_est_min
        l32r     a8, .Lsec_61_18
        callx8   a8
        movi.n   a2, 1
        l32i.n   a3, a1, 0
        l32i.n   a10, a1, 12
        sub      a10, a3, a10
        l32i.n   a8, a1, 16
        l32i.n   a3, a1, 4
        sub      a3, a3, a8
        l32i.n   a9, a1, 8
        l32i.n   a8, a1, 20
        max      a8, a9, a8
        s32i.n   a8, a1, 60
        s32i.n   a10, a6, 0
        s32i.n   a3, a6, 4
        j        .Lsec_61_1e8
.Lsec_61_1e1:
        l32i.n   a10, a6, 0
        movi.n   a2, 2
        l32i     a4, a1, 80
.Lsec_61_1e8:
        l32i.n   a3, a7, 0
        l32i     a3, a3, 0x140
        callx8   a3
        bge      a10, a5, .Lsec_61_1f6
        j        .Lsec_61_44a
.Lsec_61_1f6:
        l32i.n   a3, a6, 0
        mull     a3, a2, a3
        ssr      a4
        sra      a3, a3
        l32i     a8, a7, 0
        sext     a3, a3, 15
        l32i     a8, a8, 0x140
        l32i     a10, a6, 4
        callx8   a8
        bge      a10, a5, .Lsec_61_216
        j        .Lsec_61_430
.Lsec_61_216:
        l32i.n   a8, a6, 4
        mull     a2, a2, a8
        ssr      a4
        sra      a2, a2
        sext     a2, a2, 15
        bnez.n   a3, .Lsec_61_258
        j        .Lsec_61_22b
.Lsec_61_229:
        mov.n    a2, a3
.Lsec_61_22b:
        l32i.n   a3, a7, 0
        l32i.n   a10, a1, 12
        l32i     a3, a3, 0x140
        callx8   a3
        movi.n   a8, 49
        blt      a8, a10, .Lsec_61_24d
        l32i.n   a8, a6, 0
        movi.n   a3, 1
        bge      a8, a3, .Lsec_61_258
        movi.n   a9, 0
        moveqz   a3, a9, a8
        neg      a3, a3
        j        .Lsec_61_258
        .byte    0x00
.Lsec_61_24d:
        l32i.n   a3, a1, 12
        ssr      a4
        sra      a3, a3
        sext     a3, a3, 15
.Lsec_61_258:
        bnez.n   a2, .Lsec_61_288
.Lsec_61_25a:
        l32i.n   a2, a7, 0
        l32i.n   a10, a1, 16
        l32i     a2, a2, 0x140
        callx8   a2
        movi.n   a9, 49
        blt      a9, a10, .Lsec_61_27d
        l32i.n   a4, a6, 4
        movi.n   a2, 1
        bge      a4, a2, .Lsec_61_288
        movi.n   a8, 0
        moveqz   a2, a8, a4
        neg      a2, a2
        j        .Lsec_61_288
        .byte    0x00
        .byte    0x00
.Lsec_61_27d:
        l32i.n   a2, a1, 16
        ssr      a4
        sra      a2, a2
        sext     a2, a2, 15
.Lsec_61_288:
        l32i.n   a8, a1, 52
        beqi     a8, 1, .Lsec_61_2ec
        movi.n   a4, 44
        l32i.n   a9, a1, 60
        blt      a4, a9, .Lsec_61_2be
        l32i.n   a4, a7, 0
        l32i.n   a10, a6, 0
        l32i     a4, a4, 0x140
        callx8   a4
        movi.n   a4, 9
        blt      a4, a10, .Lsec_61_2c2
        l32i.n   a8, a7, 0
        l32i.n   a10, a6, 4
        l32i     a8, a8, 0x140
        callx8   a8
        blt      a4, a10, .Lsec_61_2c2
        l32i     a8, a1, 84
        addi.n   a4, a8, 1
        extui    a4, a4, 0, 8
        s32i     a4, a1, 84
        j        .Lsec_61_2c2
.Lsec_61_2be:
        movi.n   a2, 0
        mov.n    a3, a2
.Lsec_61_2c2:
        l32i     a9, a1, 72
        movi     a4, 0x186
        bgeu     a4, a9, .Lsec_61_2ec
        mov.n    a10, a3
        movi.n   a12, -5
        l32i.n   a4, a7, 0
        movi.n   a11, 5
        l32i     a3, a4, 44
        callx8   a3
        sext     a3, a10, 15
        mov.n    a10, a2
        l32i.n   a4, a7, 0
        movi.n   a12, -5
        movi.n   a11, 5
        l32i.n   a2, a4, 44
        callx8   a2
        sext     a2, a10, 15
.Lsec_61_2ec:
        l32i.n   a4, a7, 0
        l32i.n   a10, a6, 0
        l32i     a4, a4, 0x140
        callx8   a4
        blt      a5, a10, .Lsec_61_31c
        l32i.n   a4, a7, 0
        l32i     a10, a6, 4
        l32i     a4, a4, 0x140
        callx8   a4
        blt      a5, a10, .Lsec_61_31c
        movi.n   a4, 45
        l32i.n   a8, a1, 60
        blt      a4, a8, .Lsec_61_31c
        movi.n   a2, 1
        l32i     a9, a1, 88
        s8i      a2, a9, 0
        memw
        j        .Lsec_61_3b0
.Lsec_61_31c:
        l32i.n   a4, a7, 0
        l32i.n   a10, a6, 0
        l32i     a4, a4, 0x140
        callx8   a4
        bge      a5, a10, .Lsec_61_334
        l32i.n   a4, a1, 48
        sub      a3, a4, a3
        sext     a3, a3, 15
        s32i     a3, a1, 32
.Lsec_61_334:
        nop.n
        nop.n
        nop.n
        nop
        l32i.n   a3, a7, 0
        l32i     a10, a6, 4
        l32i     a3, a3, 0x140
        callx8   a3
        bge      a5, a10, .Lsec_61_355
        l32i.n   a8, a1, 36
        sub      a2, a8, a2
        sext     a2, a2, 15
        s32i.n   a2, a1, 36
.Lsec_61_355:
        movi     a2, 0x1ff
        movi.n   a4, 0
        nop.n
        nop.n
        l32i.n   a9, a1, 44
        addi.n   a3, a9, 1
        extui    a3, a3, 0, 8
        l32i.n   a8, a1, 32
        l32i.n   a9, a1, 36
        s32i.n   a3, a1, 44
        min      a3, a8, a2
        min      a2, a9, a2
        max      a2, a2, a4
        max      a3, a3, a4
        l32i.n   a9, a1, 44
        sext     a8, a9, 7
        s32i.n   a2, a1, 36
        nop.n
        nop.n
        nop.n
        nop.n
        l32i     a2, a1, 68
        s32i.n   a3, a1, 32
        bge      a8, a2, .Lsec_61_391
        j        .Lsec_61_102
.Lsec_61_391:
        nop.n
        nop.n
        nop.n
        nop.n
        l32i     a8, a1, 88
        l8ui     a2, a8, 0
        bne      a2, a4, .Lsec_61_3b0
        l32i     a9, a1, 84
        beq      a9, a4, .Lsec_61_3b0
        movi.n   a2, 2
        s8i      a2, a8, 0
        memw
.Lsec_61_3b0:
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a2, a1, 40
        l16si    a12, a2, 0
        bgez     a12, .Lsec_61_3d0
        movi.n   a2, 0
        movi.n   a12, 0
        l32i.n   a3, a1, 40
        s16i     a2, a3, 0
        memw
        j        .Lsec_61_3e0
        .byte    0x00
.Lsec_61_3d0:
        movi     a2, 0x1ff
        bge      a2, a12, .Lsec_61_3e0
        l32i.n   a5, a1, 40
        mov.n    a12, a2
        s16i     a2, a5, 0
        memw
.Lsec_61_3e0:
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a8, a1, 40
        l16si    a2, a8, 2
        bgez     a2, .Lsec_61_3fc
        movi.n   a2, 0
        s16i     a2, a8, 2
        memw
        j        .Lsec_61_40a
        .byte    0x00
.Lsec_61_3fc:
        movi     a3, 0x1ff
        bge      a3, a2, .Lsec_61_40a
        l32i.n   a9, a1, 40
        s16i     a3, a9, 2
        memw
.Lsec_61_40a:
        movi.n   a10, 2
        nop.n
        nop.n
        nop.n
        l32i.n   a2, a7, 0
        l32i.n   a11, a1, 56
        l32i     a2, a2, 184
        callx8   a2
        movi.n   a10, 3
        l32i.n   a2, a7, 0
        l32i.n   a3, a1, 40
        l32i.n   a11, a1, 56
        l16ui    a12, a3, 2
        l32i     a2, a2, 184
        callx8   a2
        retw.n
        .byte    0x00
.Lsec_61_430:
        bnez     a3, .Lsec_61_25a
        j        .Lsec_61_229
.Lsec_61_436:
        l32i.n   a3, a6, 4
        mull     a2, a2, a3
        ssr      a4
        sra      a2, a2
        sext     a2, a2, 15
        j        .Lsec_61_22b
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_61_44a:
        l32i.n   a3, a7, 0
        l32i.n   a10, a6, 4
        l32i     a3, a3, 0x140
        callx8   a3
        bge      a10, a5, .Lsec_61_436
        movi.n   a2, 0
        j        .Lsec_61_22b
        .size   pbus_rx_dco_cal_1step, .-pbus_rx_dco_cal_1step
.Lsec_61_45c:

        .section .text.rc_cal,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_62_0, chip7_sleep_params
        .literal .Lsec_62_4, 0x00800000
        .literal .Lsec_62_8, g_phyFuns
        .literal .Lsec_62_c, chip7_phy_init_ctrl
        .literal .Lsec_62_10, wifi_txband
        .literal .Lsec_62_14, 0xcccccccd
        .literal .Lsec_62_18, 0x58ed2309
        .literal .Lsec_62_1c, wifi_txband_ht40
        .literal .Lsec_62_20, 0x50150151
        .literal .Lsec_62_24, 0x88888889
        .literal .Lsec_62_28, ets_delay_us
        .literal .Lsec_62_2c, bb_bss_cbw40_ana
.Lsec_62_30:
        .global rc_cal
        .type   rc_cal, @function
rc_cal:
        entry    a1, 32
        l32r     a2, .Lsec_62_0
        l32r     a4, .Lsec_62_4
        l32i.n   a5, a2, 0
        and      a5, a5, a4
        l32r     a3, .Lsec_62_8
        bnez     a5, .Lsec_62_26c
        movi.n   a15, 2
        mov.n    a14, a5
        mov.n    a11, a15
        l32i.n   a6, a3, 0
        movi.n   a13, 4
        movi.n   a12, 6
        movi     a10, 106
        l32i     a6, a6, 168
        callx8   a6
        movi.n   a15, 2
        mov.n    a12, a5
        l32i.n   a6, a3, 0
        mov.n    a11, a15
        movi.n   a14, 4
        movi.n   a13, 5
        movi     a10, 106
        l32i     a5, a6, 168
        callx8   a5
        movi.n   a15, 7
        l32r     a5, .Lsec_62_c
        l8ui     a5, a5, 1
        beqi     a5, 1, .Lsec_62_85
        addi     a15, a5, -2
        movi.n   a6, 11
        movi.n   a5, 6
        moveqz   a6, a5, a15
        mov.n    a15, a6
.Lsec_62_85:
        l32i.n   a5, a3, 0
        movi.n   a14, 4
        mov.n    a12, a14
        movi.n   a13, 7
        movi.n   a11, 2
        l32i     a5, a5, 168
        movi     a10, 106
        callx8   a5
        movi.n   a15, 1
        movi.n   a14, 5
        l32i.n   a5, a3, 0
        mov.n    a13, a14
        mov.n    a12, a15
        movi.n   a11, 3
        movi     a10, 104
        l32i     a5, a5, 168
        callx8   a5
        movi.n   a14, 0
        mov.n    a13, a14
        l32i.n   a5, a3, 0
        movi.n   a15, 1
        movi.n   a12, 4
        movi.n   a11, 2
        movi     a10, 106
        l32i     a5, a5, 168
        callx8   a5
        movi.n   a14, 3
        mov.n    a13, a14
        l32i.n   a5, a3, 0
        movi     a15, 0
        movi     a12, 4
        movi     a11, 2
        movi     a10, 106
        l32i     a5, a5, 168
        callx8   a5
        movi.n   a14, 3
        movi.n   a15, 1
        l32i.n   a5, a3, 0
        mov.n    a13, a14
        movi     a12, 4
        movi     a11, 2
        movi     a10, 106
        l32i     a5, a5, 168
        callx8   a5
        movi     a10, 100
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_62_28
        callx8   a8
        l32i.n   a5, a3, 0
        movi.n   a13, 5
        mov.n    a12, a13
        movi.n   a14, 0
        movi.n   a11, 2
        l32i     a5, a5, 164
        movi     a10, 106
        callx8   a5
        mov.n    a5, a10
        movi.n   a14, 5
        l32i.n   a6, a3, 0
        mov.n    a13, a14
        movi.n   a15, 0
        movi.n   a12, 1
        movi.n   a11, 3
        l32i     a6, a6, 168
        movi     a10, 104
        s8i      a5, a2, 80
        memw
        callx8   a6
        movi.n   a15, 0
        mov.n    a14, a15
        mov.n    a13, a15
        l32i.n   a6, a3, 0
        movi.n   a12, 4
        movi.n   a11, 2
        movi     a10, 106
        addi     a5, a5, 56
        l32i     a6, a6, 168
        extui    a5, a5, 0, 8
        callx8   a6
        movi     a9, 0x2ee
        mull     a9, a5, a9
        l32r     a13, .Lsec_62_18
        addx4    a12, a5, a5
        muluh    a13, a9, a13
        slli     a8, a12, 5
        sub      a8, a8, a12
        l32r     a6, .Lsec_62_10
        slli     a8, a8, 2
        sub      a10, a9, a13
        srli     a10, a10, 1
        add.n    a10, a10, a13
        l32r     a11, .Lsec_62_20
        addx4    a12, a12, a12
        muluh    a11, a8, a11
        slli     a12, a12, 4
        sub      a5, a8, a11
        l8ui     a6, a6, 0
        srli     a5, a5, 1
        quou     a9, a9, a6
        add.n    a5, a5, a11
        srli     a10, a10, 6
        l32r     a13, .Lsec_62_24
        srli     a5, a5, 7
        muluh    a12, a12, a13
        addi     a9, a9, -57
        srli     a12, a12, 4
        l32r     a6, .Lsec_62_1c
        addi     a10, a10, -57
        addi     a5, a5, -57
        addi     a12, a12, -57
        movi     a11, 127
        l8ui     a6, a6, 0
        quou     a8, a8, a6
        addi     a8, a8, -57
        l32r     a13, .Lsec_62_14
        muluh    a9, a9, a13
        muluh    a10, a10, a13
        muluh    a8, a8, a13
        muluh    a5, a5, a13
        muluh    a12, a12, a13
        srli     a9, a9, 3
        srli     a10, a10, 3
        srli     a8, a8, 3
        srli     a5, a5, 3
        srli     a12, a12, 3
        sext     a7, a9, 15
        sext     a15, a10, 15
        sext     a6, a8, 15
        sext     a14, a5, 15
        sext     a13, a12, 15
        blt      a11, a7, .Lsec_62_1e8
        movi     a11, 2
        blt      a7, a11, .Lsec_62_1e8
        extui    a11, a9, 0, 8
.Lsec_62_1e8:
        s8i      a11, a2, 84
        memw
        movi     a9, 127
        blt      a9, a6, .Lsec_62_1fc
        movi.n   a9, 2
        blt      a6, a9, .Lsec_62_1fc
        extui    a9, a8, 0, 8
.Lsec_62_1fc:
        s8i      a9, a2, 86
        memw
        movi     a8, 127
        blt      a8, a15, .Lsec_62_214
        movi     a6, 13
        movi     a8, 14
        bge      a6, a15, .Lsec_62_214
        extui    a8, a10, 0, 8
.Lsec_62_214:
        s8i      a8, a2, 85
        memw
        movi     a8, 127
        blt      a8, a14, .Lsec_62_22c
        movi     a6, 13
        movi     a8, 14
        bge      a6, a14, .Lsec_62_22c
        extui    a8, a5, 0, 8
.Lsec_62_22c:
        s8i      a8, a2, 87
        memw
        movi     a5, 127
        blt      a5, a13, .Lsec_62_244
        movi.n   a5, 2
        blt      a13, a5, .Lsec_62_250
        extui    a5, a12, 0, 8
        j        .Lsec_62_250
        .byte    0x00
.Lsec_62_244:
        movi     a6, 127
        s8i      a6, a2, 88
        memw
        j        .Lsec_62_25e
.Lsec_62_250:
        s8i      a5, a2, 88
        memw
        movi.n   a5, 2
        blt      a13, a5, .Lsec_62_25e
        extui    a5, a12, 0, 8
.Lsec_62_25e:
        nop.n
        nop.n
        l32i.n   a6, a2, 0
        or       a4, a6, a4
        s8i      a5, a2, 89
        s32i.n   a4, a2, 0
.Lsec_62_26c:
        movi.n   a10, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, bb_bss_cbw40_ana
        l32r     a8, .Lsec_62_2c
        callx8   a8
        movi.n   a12, 7
        movi.n   a11, 1
        l32i.n   a4, a3, 0
        movi     a10, 103
        l8ui     a13, a2, 88
        addi     a13, a13, -2
        extui    a13, a13, 0, 8
        l32i     a4, a4, 160
        callx8   a4
        movi.n   a12, 8
        movi.n   a11, 1
        l32i.n   a4, a3, 0
        movi     a10, 103
        l8ui     a13, a2, 88
        l32i     a4, a4, 160
        callx8   a4
        movi.n   a12, 5
        movi.n   a11, 1
        l32i.n   a4, a3, 0
        movi     a10, 103
        l8ui     a13, a2, 89
        addi     a13, a13, -2
        extui    a13, a13, 0, 8
        l32i     a4, a4, 160
        callx8   a4
        movi.n   a12, 6
        movi.n   a11, 1
        l32i.n   a3, a3, 0
        movi     a10, 103
        l32i     a3, a3, 160
        l8ui     a13, a2, 89
        callx8   a3
        retw.n
        .size   rc_cal, .-rc_cal
.Lsec_62_2ca:

        .section .text.ram_rfcal_txcap,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_63_0, g_phyFuns
        .literal .Lsec_63_4, .Lsec_47_e4
        .literal .Lsec_63_8, .Lsec_47_f0
        .literal .Lsec_63_c, phy_printf
        .literal .Lsec_63_10, phy_printf
.Lsec_63_14:
        .global ram_rfcal_txcap
        .type   ram_rfcal_txcap, @function
ram_rfcal_txcap:
        entry    a1, 64
        l32r     a6, .Lsec_63_0
        movi.n   a15, 0
        extui    a11, a2, 0, 8
        extui    a12, a3, 0, 8
        mov.n    a14, a15
        l32i.n   a7, a6, 0
        mov.n    a13, a15
        movi.n   a10, 1
        extui    a4, a4, 0, 8
        movi.n   a3, 0
        l32i     a2, a7, 68
        mov.n    a7, a5
        s32i.n   a4, a1, 4
        callx8   a2
        movi.n   a2, 1
        s32i.n   a2, a1, 20
        s32i.n   a3, a1, 16
        j        .Lsec_63_53
.Lsec_63_42:
        l32i.n   a4, a1, 20
        addi.n   a7, a7, 1
        addi.n   a2, a4, 1
        extui    a2, a2, 0, 8
        l32i.n   a8, a1, 16
        addi.n   a8, a8, 1
        s32i.n   a8, a1, 16
        s32i.n   a2, a1, 20
.Lsec_63_53:
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a2, a1, 16
        extui    a5, a2, 0, 8
        l32i.n   a2, a6, 0
        beqi     a5, 1, .Lsec_63_82
        beqi     a5, 2, .Lsec_63_9d
        movi.n   a14, 0
        movi.n   a13, 3
        l32i     a2, a2, 164
        movi.n   a12, 1
        movi.n   a11, 2
        movi     a10, 107
        callx8   a2
        movi.n   a3, 15
        s32i.n   a10, a1, 8
        s32i.n   a3, a1, 0
        j        .Lsec_63_b4
.Lsec_63_82:
        movi.n   a12, 2
        l32i     a2, a2, 164
        movi.n   a14, 0
        movi.n   a13, 3
        or       a11, a12, a12
        movi     a10, 107
        callx8   a2
        movi.n   a4, 15
        s32i.n   a10, a1, 8
        s32i.n   a4, a1, 0
        j        .Lsec_63_b4
.Lsec_63_9d:
        l32i     a2, a2, 164
        movi.n   a14, 4
        movi.n   a13, 6
        mov.n    a12, a5
        mov.n    a11, a5
        movi     a10, 107
        callx8   a2
        movi.n   a8, 7
        s32i.n   a10, a1, 8
        s32i.n   a8, a1, 0
.Lsec_63_b4:
        movi.n   a2, 0
        nop.n
        nop.n
        nop.n
        l32i.n   a3, a1, 8
        s32i.n   a2, a1, 12
        s8i      a3, a7, 0
        memw
        mov.n    a3, a2
        j        .Lsec_63_d5
        .byte    0x00
.Lsec_63_cc:
        l32i.n   a4, a1, 8
        addi.n   a8, a4, 1
        extui    a8, a8, 0, 8
        s32i.n   a8, a1, 8
.Lsec_63_d5:
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a8, a1, 0
        l32i.n   a4, a1, 8
        bgeu     a8, a4, .Lsec_63_ea
        j        .Lsec_63_184
        .byte    0x00
.Lsec_63_e8:
        mov.n    a2, a9
.Lsec_63_ea:
        l32i.n   a9, a6, 0
        mov.n    a15, a4
        beqi     a5, 1, .Lsec_63_108
        beqi     a5, 2, .Lsec_63_11c
        movi.n   a14, 0
        l32i     a9, a9, 168
        movi.n   a13, 3
        movi.n   a12, 1
        movi.n   a11, 2
        movi     a10, 107
        callx8   a9
        j        .Lsec_63_12e
.Lsec_63_108:
        movi.n   a12, 2
        l32i     a9, a9, 168
        movi.n   a14, 0
        movi.n   a13, 3
        mov.n    a11, a12
        movi     a10, 107
        callx8   a9
        j        .Lsec_63_12e
.Lsec_63_11c:
        l32i     a9, a9, 168
        movi.n   a14, 4
        movi.n   a13, 6
        mov.n    a12, a5
        or       a11, a5, a5
        movi     a10, 107
        callx8   a9
.Lsec_63_12e:
        l32i.n   a9, a6, 0
        movi.n   a10, 0
        l32i     a9, a9, 0x134
        callx8   a9
        mov.n    a9, a10
        l32i.n   a8, a1, 4
        beqz.n   a8, .Lsec_63_150
        mov.n    a12, a10
        mov.n    a11, a4
        l32r     a10, .Lsec_63_4
        s32i     a9, a1, 24
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_63_c
        callx8   a8
        l32i.n   a9, a1, 24
.Lsec_63_150:
        bge      a2, a9, .Lsec_63_184
        s8i      a4, a7, 0
        memw
        bnez.n   a3, .Lsec_63_175
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a2, a1, 8
        bgeu     a4, a2, .Lsec_63_16c
        movi.n   a8, 1
        s32i.n   a8, a1, 12
.Lsec_63_16c:
        addi.n   a8, a4, -1
        extui    a4, a8, 0, 8
        j        .Lsec_63_17a
        .byte    0x00
.Lsec_63_175:
        addi.n   a8, a4, 1
        extui    a4, a8, 0, 8
.Lsec_63_17a:
        l32i.n   a2, a1, 0
        bltu     a2, a4, .Lsec_63_182
        j        .Lsec_63_e8
.Lsec_63_182:
        mov.n    a2, a9
.Lsec_63_184:
        l32i.n   a4, a1, 12
        beqi     a4, 1, .Lsec_63_194
        addi.n   a3, a3, 1
        extui    a3, a3, 0, 8
        beqi     a3, 2, .Lsec_63_194
        j        .Lsec_63_cc
.Lsec_63_194:
        l32i.n   a8, a1, 4
        beqz.n   a8, .Lsec_63_1a6
        l8ui     a12, a7, 0
        l32i.n   a11, a1, 16
        l32r     a10, .Lsec_63_8
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_63_10
        callx8   a8
.Lsec_63_1a6:
        l32i.n   a2, a6, 0
        beqi     a5, 1, .Lsec_63_1cd
        beqi     a5, 2, .Lsec_63_1e4
        movi.n   a14, 0
        movi.n   a13, 3
        l32i     a2, a2, 168
        movi.n   a12, 1
        movi.n   a11, 2
        movi     a10, 107
        l8ui     a15, a7, 0
        callx8   a2
        l32i.n   a2, a1, 20
        beqi     a2, 3, .Lsec_63_1ca
        j        .Lsec_63_42
.Lsec_63_1ca:
        j        .Lsec_63_1f8
.Lsec_63_1cd:
        movi.n   a12, 2
        l32i     a2, a2, 168
        movi.n   a14, 0
        movi.n   a13, 3
        mov.n    a11, a12
        movi     a10, 107
        l8ui     a15, a7, 0
        callx8   a2
        j        .Lsec_63_42
.Lsec_63_1e4:
        l32i     a2, a2, 168
        movi.n   a14, 4
        movi.n   a13, 6
        mov.n    a12, a5
        mov.n    a11, a5
        l8ui     a15, a7, 0
        movi     a10, 107
        callx8   a2
.Lsec_63_1f8:
        movi.n   a10, 1
        l32i.n   a2, a6, 0
        l32i     a2, a2, 76
        callx8   a2
        retw.n
        .size   ram_rfcal_txcap, .-ram_rfcal_txcap
.Lsec_63_204:

        .section .text.tx_cap_init,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_64_0, chip7_sleep_params
        .literal .Lsec_64_4, 0x00040000
        .literal .Lsec_64_8, g_phyFuns
        .literal .Lsec_64_c, chip7_sleep_params+0x40
        .literal .Lsec_64_10, .Lsec_81_0
        .literal .Lsec_64_14, chip7_phy_init_ctrl
        .literal .Lsec_64_18, pwrdet_offset
        .literal .Lsec_64_1c, rfcal_bb_atten_init
        .literal .Lsec_64_20, txcal_debuge_mode
        .literal .Lsec_64_24, set_channel_rfpll_freq
        .literal .Lsec_64_28, ets_delay_us
.Lsec_64_2c:
        .global tx_cap_init
        .type   tx_cap_init, @function
tx_cap_init:
        entry    a1, 48
        l32r     a2, .Lsec_64_0
        l32r     a3, .Lsec_64_4
        l32i.n   a4, a2, 0
        and      a4, a4, a3
        bnez     a4, .Lsec_64_1a6
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, txcal_debuge_mode
        l32r     a8, .Lsec_64_20
        callx8   a8
        l32r     a3, .Lsec_64_8
        mov.n    a14, a4
        movi.n   a15, 15
        movi.n   a13, 3
        movi.n   a12, 1
        l32i.n   a5, a3, 0
        movi.n   a11, 2
        movi     a10, 107
        movi     a7, 1
        l32i     a5, a5, 168
        callx8   a5
        mov.n    a14, a4
        movi.n   a12, 2
        l32i.n   a4, a3, 0
        mov.n    a11, a12
        movi.n   a15, 7
        movi.n   a13, 3
        movi     a10, 107
        l32i     a4, a4, 168
        movi     a5, 80
        callx8   a4
        movi.n   a12, 2
        movi     a10, 107
        l32i.n   a4, a3, 0
        movi.n   a15, 3
        movi.n   a14, 4
        movi.n   a13, 6
        mov.n    a11, a12
        l32i     a4, a4, 168
        callx8   a4
        mov.n    a10, a7
        l32r     a4, .Lsec_64_c
        l32r     a6, .Lsec_64_14
        j        .Lsec_64_a7
.Lsec_64_98:
        l32r     a8, .Lsec_64_10
        addi.n   a9, a7, 1
        add.n    a8, a8, a7
        addi.n   a4, a4, 3
        extui    a7, a9, 0, 8
        l8ui     a10, a8, 0
.Lsec_64_a7:
        movi.n   a12, 0
        sext     a10, a10, 7
        l8ui     a11, a6, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, set_channel_rfpll_freq
        l32r     a8, .Lsec_64_24
        callx8   a8
        beqi     a7, 1, .Lsec_64_bb
        j        .Lsec_64_168
.Lsec_64_bb:
        movi.n   a8, 4
        s32i.n   a4, a1, 0
        mov.n    a4, a8
.Lsec_64_c1:
        movi.n   a15, 0
        movi     a11, 128
        mov.n    a12, a5
        l32i.n   a9, a3, 0
        mov.n    a14, a15
        mov.n    a13, a15
        movi.n   a10, 1
        l32i     a9, a9, 68
        callx8   a9
        movi.n   a10, 2
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_64_28
        callx8   a8
        l32i.n   a9, a3, 0
        l32r     a8, .Lsec_64_18
        l32i     a9, a9, 0x178
        l16ui    a10, a8, 0
        callx8   a9
        extui    a10, a10, 2, 16
        addi     a9, a10, -40
        addi     a10, a10, -37
        extui    a9, a9, 0, 16
        extui    a10, a10, 0, 16
        add.n    a11, a5, a9
        sext     a9, a9, 15
        bltui    a10, 7, .Lsec_64_145
        blti     a9, 1, .Lsec_64_10c
        extui    a5, a11, 0, 8
        j        .Lsec_64_11c
.Lsec_64_10c:
        addx2    a9, a9, a9
        addi.n   a10, a9, 3
        movltz   a9, a10, a9
        srai     a9, a9, 2
        add.n    a5, a5, a9
        extui    a5, a5, 0, 8
.Lsec_64_11c:
        movi     a8, 120
        sext     a5, a5, 7
        min      a5, a5, a8
        addi.n   a4, a4, -1
        movi.n   a8, 0
        max      a5, a5, a8
        extui    a4, a4, 0, 8
        extui    a5, a5, 0, 8
        bne      a4, a8, .Lsec_64_13d
        l32i.n   a4, a1, 0
        extui    a5, a5, 0, 8
        j        .Lsec_64_147
.Lsec_64_13d:
        extui    a5, a5, 0, 8
        j        .Lsec_64_c1
        .byte    0x00
        .byte    0x00
.Lsec_64_145:
        l32i.n   a4, a1, 0
.Lsec_64_147:
        addi     a8, a5, -8
        mov.n    a13, a4
        movi.n   a12, 0
        mov.n    a11, a5
        l32i.n   a9, a3, 0
        movi     a10, 128
        l32r     a14, .Lsec_64_1c
        l32i     a9, a9, 0x170
        s8i      a8, a14, 0
        memw
        callx8   a9
        j        .Lsec_64_98
        .byte    0x00
.Lsec_64_168:
        l32i.n   a8, a3, 0
        mov.n    a13, a4
        movi.n   a12, 0
        mov.n    a11, a5
        movi     a10, 128
        l32i     a8, a8, 0x170
        callx8   a8
        beqi     a7, 3, .Lsec_64_17f
        j        .Lsec_64_98
.Lsec_64_17f:
        l32i.n   a3, a3, 0
        l8ui     a4, a2, 72
        l8ui     a6, a2, 70
        l8ui     a5, a2, 71
        l32i     a3, a3, 0x124
        s8i      a4, a2, 75
        s8i      a6, a2, 73
        s8i      a5, a2, 74
        memw
        callx8   a3
        l32i.n   a3, a2, 0
        l32r     a4, .Lsec_64_4
        or       a3, a3, a4
        s32i.n   a3, a2, 0
.Lsec_64_1a6:
        retw.n
        .size   tx_cap_init, .-tx_cap_init
.Lsec_64_1a8:

        .section .text.ram_meas_tone_pwr_db,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_65_0, g_phyFuns
        .literal .Lsec_65_4, pwrdet_offset
.Lsec_65_8:
        .global ram_meas_tone_pwr_db
        .type   ram_meas_tone_pwr_db, @function
ram_meas_tone_pwr_db:
        entry    a1, 32
        l32r     a5, .Lsec_65_0
        extui    a12, a2, 0, 8
        movi.n   a15, 0
        mov.n    a14, a15
        mov.n    a13, a15
        l32i     a2, a5, 0
        movi     a11, 128
        movi     a10, 1
        movi     a3, 0
        l32i     a2, a2, 68
        callx8   a2
        movi.n   a2, 4
        l32r     a4, .Lsec_65_4
.Lsec_65_2e:
        addi.n   a2, a2, -1
        l32i     a8, a5, 0
        l16ui    a10, a4, 0
        l32i     a8, a8, 0x178
        callx8   a8
        add.n    a10, a10, a3
        sext     a3, a10, 15
        bnez     a2, .Lsec_65_2e
        l32i.n   a2, a5, 0
        movi.n   a10, 1
        l32i     a2, a2, 76
        callx8   a2
        addi.n   a2, a3, 4
        addi.n   a3, a3, 11
        movltz   a2, a3, a2
        srai     a2, a2, 3
        sext     a2, a2, 15
        retw.n
        .size   ram_meas_tone_pwr_db, .-ram_meas_tone_pwr_db
.Lsec_65_5d:

        .section .text.ram_rfcal_pwrctrl,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_66_0, g_phyFuns
        .literal .Lsec_66_4, .Lsec_47_108
        .literal .Lsec_66_8, ets_delay_us
        .literal .Lsec_66_c, phy_printf
.Lsec_66_10:
        .global ram_rfcal_pwrctrl
        .type   ram_rfcal_pwrctrl, @function
ram_rfcal_pwrctrl:
        entry    a1, 112
        extui    a4, a4, 0, 8
        s32i     a4, a1, 64
        addi.n   a4, a4, -1
        extui    a8, a4, 0, 8
        extui    a2, a2, 0, 8
        sext     a9, a8, 7
        s32i.n   a3, a1, 56
        extui    a5, a5, 0, 8
        nop.n
        nop.n
        nop.n
        l8ui     a3, a1, 116
        s32i.n   a2, a1, 24
        extui    a2, a7, 0, 16
        s32i.n   a2, a1, 16
        s32i.n   a4, a1, 52
        s32i.n   a8, a1, 44
        s32i.n   a9, a1, 36
        s32i.n   a6, a1, 48
        s32i.n   a5, a1, 60
        s32i.n   a3, a1, 32
        nop.n
        nop.n
        nop.n
        nop.n
        l8ui     a2, a1, 112
        bgez     a9, .Lsec_66_7c
        l32r     a7, .Lsec_66_0
.Lsec_66_56:
        l32i.n   a4, a1, 48
        l32i     a8, a1, 64
        add.n    a2, a4, a8
        addi.n   a2, a2, -1
        l32i.n   a9, a1, 48
        l8ui     a4, a2, 0
        sext     a3, a4, 7
        l8ui     a2, a9, 0
        sext     a2, a2, 7
        sub      a2, a3, a2
        movi.n   a3, 50
        bge      a3, a2, .Lsec_66_78
        j        .Lsec_66_205
.Lsec_66_78:
        j        .Lsec_66_210
        .byte    0x00
.Lsec_66_7c:
        movi.n   a5, 0
        sext     a2, a2, 7
        l32r     a7, .Lsec_66_0
        mov.n    a6, a5
        s32i.n   a2, a1, 28
.Lsec_66_88:
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a3, a1, 56
        l32i.n   a4, a1, 36
        add.n    a2, a3, a4
        l8ui     a8, a2, 0
        l32i.n   a9, a1, 52
        s32i.n   a8, a1, 40
        blt      a4, a9, .Lsec_66_a8
        extui    a2, a8, 0, 16
        s32i.n   a2, a1, 20
        j        .Lsec_66_ba
.Lsec_66_a8:
        l32i.n   a3, a1, 40
        extui    a3, a3, 0, 16
        l8ui     a2, a2, 1
        sub      a2, a2, a3
        add.n    a8, a2, a5
        sext     a5, a8, 15
        s32i.n   a3, a1, 20
.Lsec_66_ba:
        movi.n   a9, 0
        movi.n   a4, 2
        mov.n    a2, a9
        mov.n    a9, a4
        mov.n    a4, a2
        j        .Lsec_66_ca
        .byte    0x00
.Lsec_66_c8:
        mov.n    a9, a3
.Lsec_66_ca:
        l32i.n   a8, a1, 28
        neg      a2, a5
        sext     a2, a2, 7
        sub      a2, a8, a2
        max      a2, a2, a6
        l32i.n   a3, a7, 0
        movi     a8, 100
        movi.n   a15, 0
        min      a2, a2, a8
        mov.n    a14, a15
        l32i.n   a11, a1, 24
        mov.n    a13, a15
        mov.n    a12, a2
        movi     a10, 1
        l32i     a3, a3, 68
        s32i     a9, a1, 68
        callx8   a3
        movi.n   a10, 2
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_66_8
        callx8   a8
        l32i.n   a3, a7, 0
        l32i.n   a10, a1, 16
        l32i     a3, a3, 0x178
        callx8   a3
        mov.n    a3, a10
        l32i.n   a11, a7, 0
        l32i.n   a10, a1, 16
        l32i     a11, a11, 0x178
        callx8   a11
        add.n    a10, a3, a10
        sext     a3, a10, 15
        addi.n   a3, a3, 4
        srai     a3, a3, 3
        l32i     a9, a1, 68
        bltz     a3, .Lsec_66_12e
        sext     a12, a3, 15
        extui    a3, a3, 0, 16
        j        .Lsec_66_132
        .byte    0x00
.Lsec_66_12e:
        movi.n   a3, 0
        mov.n    a12, a3
.Lsec_66_132:
        l32i.n   a8, a1, 20
        sub      a3, a3, a8
        sext     a3, a3, 15
        movi.n   a8, 24
        bge      a8, a3, .Lsec_66_142
        j        .Lsec_66_222
.Lsec_66_142:
        movi.n   a8, -24
        bge      a3, a8, .Lsec_66_14a
        j        .Lsec_66_228
.Lsec_66_14a:
        l32i.n   a8, a1, 32
        beqz.n   a8, .Lsec_66_168
.Lsec_66_14e:
        mov.n    a15, a5
        mov.n    a14, a3
        l32i.n   a13, a1, 40
        mov.n    a11, a4
        l32r     a10, .Lsec_66_4
        s32i     a2, a1, 0
        s32i     a9, a1, 68
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_printf
        l32r     a8, .Lsec_66_c
        callx8   a8
        l32i     a9, a1, 68
.Lsec_66_168:
        bnez.n   a3, .Lsec_66_176
        sext     a10, a4, 7
        srai     a10, a10, 7
        sub      a10, a10, a4
        bbsi     a10, 7, .Lsec_66_1c8
.Lsec_66_176:
        movi.n   a11, 1
        add.n    a12, a3, a11
        mov.n    a8, a6
        moveqz   a8, a11, a12
        addi.n   a10, a9, -1
        or       a12, a8, a8
        or       a8, a6, a6
        moveqz   a8, a11, a10
        and      a10, a12, a8
        bnez.n   a10, .Lsec_66_1c8
        moveqz   a10, a11, a9
        bnez.n   a10, .Lsec_66_1c8
        movnez   a11, a10, a2
        beqz.n   a11, .Lsec_66_19c
        bltz     a3, .Lsec_66_1ce
.Lsec_66_19c:
        addi     a9, a2, -100
        bnez.n   a9, .Lsec_66_1aa
        srai     a9, a3, 15
        sub      a9, a9, a3
        bbsi     a9, 15, .Lsec_66_1d3
.Lsec_66_1aa:
        extui    a9, a3, 0, 16
        addi.n   a10, a9, 2
        extui    a10, a10, 0, 16
        add.n    a8, a9, a5
        bltui    a10, 5, .Lsec_66_1bd
        srai     a9, a3, 2
        sub      a8, a8, a9
.Lsec_66_1bd:
        addi.n   a4, a4, 1
        sext     a5, a8, 15
        beqi     a4, 10, .Lsec_66_1c8
        j        .Lsec_66_c8
.Lsec_66_1c8:
        extui    a2, a2, 0, 8
        j        .Lsec_66_1d6
.Lsec_66_1ce:
        mov.n    a2, a10
        j        .Lsec_66_1d6
.Lsec_66_1d3:
        movi     a2, 100
.Lsec_66_1d6:
        l32i.n   a8, a1, 44
        addi.n   a4, a8, -1
        extui    a4, a4, 0, 8
        l32i.n   a9, a1, 60
        sub      a2, a2, a9
        sext     a2, a2, 7
        l32i.n   a9, a1, 48
        l32i.n   a8, a1, 36
        add.n    a3, a9, a8
        movi.n   a9, -24
        max      a2, a2, a9
        s32i.n   a4, a1, 44
        sext     a4, a4, 7
        s8i      a2, a3, 0
        s32i.n   a4, a1, 36
        beqi     a4, -1, .Lsec_66_200
        j        .Lsec_66_88
.Lsec_66_200:
        j        .Lsec_66_56
        .byte    0x00
        .byte    0x00
.Lsec_66_205:
        l32i.n   a8, a1, 48
        addi     a4, a4, -50
        s8i      a4, a8, 0
        memw
.Lsec_66_210:
        movi.n   a10, 1
        nop.n
        nop.n
        nop.n
        l32i.n   a2, a7, 0
        l32i     a2, a2, 76
        callx8   a2
        retw.n
.Lsec_66_222:
        movi.n   a3, 24
        j        .Lsec_66_22a
        .byte    0x00
.Lsec_66_228:
        movi.n   a3, -24
.Lsec_66_22a:
        l32i.n   a8, a1, 32
        beqz     a8, .Lsec_66_176
        j        .Lsec_66_14e
        .size   ram_rfcal_pwrctrl, .-ram_rfcal_pwrctrl
.Lsec_66_232:

        .section .text.ram_tx_pwr_backoff,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_67_0, pwrdet_offset
        .literal .Lsec_67_4, rfcal_bb_atten_init
        .literal .Lsec_67_8, ram_rfcal_pwrctrl
        .literal .Lsec_67_c, ram_meas_tone_pwr_db
        .literal .Lsec_67_10, ram_meas_tone_pwr_db
.Lsec_67_14:
        .global ram_tx_pwr_backoff
        .type   ram_tx_pwr_backoff, @function
ram_tx_pwr_backoff:
        entry    a1, 96
        s32i.n   a2, a1, 40
        nop.n
        nop.n
        nop.n
        nop.n
        l8ui     a2, a2, 0
        slli     a4, a2, 1
        sext     a5, a4, 15
        s32i.n   a3, a1, 44
        movi.n   a3, 76
        s32i.n   a4, a1, 36
        s32i.n   a5, a1, 32
        bgeu     a3, a2, .Lsec_67_45
        movi.n   a2, 50
        movi.n   a6, 1
        s8i      a2, a1, 16
        memw
        j        .Lsec_67_50
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_67_45:
        addi     a2, a2, -26
        s8i      a2, a1, 16
        memw
        movi.n   a6, 0
.Lsec_67_50:
        movi.n   a5, 0
        addi     a14, a1, 17
        movi.n   a13, 43
        l32r     a2, .Lsec_67_4
        movi.n   a12, 1
        addi     a11, a1, 16
        movi     a10, 128
        movi.n   a4, 1
        l8ui     a3, a2, 0
        sext     a3, a3, 7
        l32r     a7, .Lsec_67_0
        l16ui    a15, a7, 0
        mov.n    a7, a5
        s32i.n   a5, a1, 4
        s32i.n   a3, a1, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, ram_rfcal_pwrctrl
        l32r     a8, .Lsec_67_8
        callx8   a8
        mov.n    a13, a5
        mov.n    a14, a5
        mov.n    a5, a6
        l8ui     a8, a1, 17
        mov.n    a6, a13
        addi     a3, a8, 43
        extui    a3, a3, 0, 8
        addi     a8, a8, 17
        s8i      a3, a2, 0
        extui    a8, a8, 0, 8
        s8i      a3, a1, 17
        memw
.Lsec_67_9c:
        sext     a8, a8, 7
        max      a8, a8, a14
        sext     a3, a3, 7
        movi     a10, 123
        extui    a8, a8, 0, 8
        bge      a10, a3, .Lsec_67_b4
        movi     a2, 123
        s8i      a2, a1, 17
.Lsec_67_b4:
        sext     a13, a8, 7
        mov.n    a10, a13
        s32i.n   a8, a1, 52
        s32i.n   a13, a1, 56
        s32i.n   a14, a1, 60
        .reloc  ., R_XTENSA_ASM_EXPAND, ram_meas_tone_pwr_db
        l32r     a8, .Lsec_67_c
        callx8   a8
        mov.n    a11, a10
        l8ui     a10, a1, 17
        sext     a10, a10, 7
        s32i.n   a11, a1, 48
        .reloc  ., R_XTENSA_ASM_EXPAND, ram_meas_tone_pwr_db
        l32r     a8, .Lsec_67_10
        callx8   a8
        l32i.n   a14, a1, 60
        mov.n    a3, a14
        moveqz   a3, a4, a7
        extui    a3, a3, 0, 8
        l32i.n   a11, a1, 48
        sub      a9, a11, a10
        sext     a9, a9, 15
        l32i.n   a2, a1, 32
        l32i.n   a8, a1, 52
        l32i.n   a13, a1, 56
        bge      a11, a2, .Lsec_67_fa
        beqz.n   a3, .Lsec_67_fa
        movi.n   a2, 44
        bge      a2, a9, .Lsec_67_fa
        bbci     a5, 0, .Lsec_67_100
.Lsec_67_fa:
        beqz.n   a5, .Lsec_67_114
        j        .Lsec_67_216
        .byte    0x00
.Lsec_67_100:
        l32i.n   a5, a1, 32
        movi.n   a2, 1
        bge      a11, a5, .Lsec_67_109
        movi.n   a2, 0
.Lsec_67_109:
        movi.n   a12, 44
        extui    a2, a2, 0, 8
        blt      a12, a9, .Lsec_67_166
        beqz     a2, .Lsec_67_166
.Lsec_67_114:
        movi.n   a2, 44
        bge      a2, a9, .Lsec_67_146
.Lsec_67_119:
        movi.n   a3, 48
        mov.n    a7, a11
        movi.n   a2, 1
        blt      a3, a9, .Lsec_67_124
        movi.n   a2, 0
.Lsec_67_124:
        l32i.n   a3, a1, 44
        l32i.n   a4, a1, 40
        s8i      a2, a3, 0
        memw
        nop.n
        nop.n
        nop.n
        nop.n
        l8ui     a2, a4, 0
        slli     a2, a2, 1
        bge      a7, a2, .Lsec_67_142
        j        .Lsec_67_1da
.Lsec_67_142:
        movi.n   a2, 0
        retw.n
.Lsec_67_146:
        movi.n   a12, 1
        bge      a6, a9, .Lsec_67_14d
        movi.n   a12, 0
.Lsec_67_14d:
        movi.n   a6, 43
        movi     a2, 1
        blt      a6, a9, .Lsec_67_158
        movi     a2, 0
.Lsec_67_158:
        and      a2, a12, a2
        extui    a2, a2, 0, 8
        bnez     a2, .Lsec_67_119
        mov.n    a5, a2
        j        .Lsec_67_168
.Lsec_67_166:
        movi.n   a5, 1
.Lsec_67_168:
        movi.n   a6, 80
        bge      a6, a10, .Lsec_67_119
        bnez     a2, .Lsec_67_119
        l8ui     a6, a1, 17
        mov.n    a10, a4
        sext     a6, a6, 7
        addi     a6, a6, -123
        movnez   a10, a2, a6
        mov.n    a6, a10
        bnez     a10, .Lsec_67_119
        moveqz   a2, a4, a13
        bnez     a2, .Lsec_67_119
        beqz     a5, .Lsec_67_200
        movi.n   a2, 48
        bge      a2, a9, .Lsec_67_194
        bnez     a3, .Lsec_67_1a1
.Lsec_67_194:
        l32i.n   a2, a1, 36
        movi.n   a6, -8
        sub      a3, a11, a2
        movi     a2, 252
        j        .Lsec_67_1b9
.Lsec_67_1a1:
        l32i.n   a2, a1, 36
        sub      a3, a11, a2
        extui    a2, a3, 31, 1
        add.n    a2, a2, a3
        srai     a2, a2, 1
        addi.n   a2, a2, -1
        extui    a2, a2, 0, 8
        sext     a6, a2, 7
        slli     a6, a6, 1
.Lsec_67_1b9:
        bge      a6, a3, .Lsec_67_1e8
        addi.n   a3, a3, -1
        extui    a2, a3, 31, 1
        add.n    a3, a2, a3
        extui    a2, a3, 1, 8
        j        .Lsec_67_1e8
.Lsec_67_1c9:
        addi.n   a7, a7, 1
        extui    a7, a7, 0, 8
        bnei     a7, 10, .Lsec_67_1d4
        j        .Lsec_67_119
.Lsec_67_1d4:
        l8ui     a3, a1, 17
        j        .Lsec_67_9c
.Lsec_67_1da:
        sub      a2, a2, a7
        addi.n   a2, a2, 1
        srai     a2, a2, 1
        sext     a2, a2, 15
        retw.n
        .byte    0x00
.Lsec_67_1e8:
        add.n    a8, a8, a2
        extui    a8, a8, 0, 8
        addi     a2, a8, 26
        s8i      a2, a1, 17
        memw
        extui    a8, a8, 0, 8
        movi.n   a6, 0
        j        .Lsec_67_1c9
        .byte    0x00
        .byte    0x00
.Lsec_67_200:
        extui    a2, a8, 0, 8
        addi.n   a8, a2, 2
        addi     a2, a2, 28
        extui    a8, a8, 0, 8
        s8i      a2, a1, 17
        memw
        mov.n    a6, a9
        j        .Lsec_67_1c9
.Lsec_67_216:
        movi.n   a5, 0
        movi.n   a15, 43
        mov.n    a2, a5
        blt      a15, a9, .Lsec_67_222
        j        .Lsec_67_168
.Lsec_67_222:
        j        .Lsec_67_100
        .size   ram_tx_pwr_backoff, .-ram_tx_pwr_backoff
.Lsec_67_225:

        .section .text.cal_rf_ana_gain,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_68_0, g_phyFuns
        .literal .Lsec_68_4, tx_rf_ana_gain
        .literal .Lsec_68_8, chip7_sleep_params
        .literal .Lsec_68_c, pwrdet_offset
        .literal .Lsec_68_10, chip7_sleep_params+0x5f
        .literal .Lsec_68_14, rfcal_bb_atten_init
        .literal .Lsec_68_18, tx_rf_ana_gain+0x2
        .literal .Lsec_68_1c, ram_rfcal_pwrctrl
        .literal .Lsec_68_20, correct_rf_ana_gain_new
.Lsec_68_24:
        .global cal_rf_ana_gain
        .type   cal_rf_ana_gain, @function
cal_rf_ana_gain:
        entry    a1, 64
        l32r     a2, .Lsec_68_0
        movi.n   a11, 1
        movi     a10, 5
        l32r     a3, .Lsec_68_4
        l32i     a4, a2, 0
        l16ui    a12, a3, 0
        l32i     a4, a4, 184
        callx8   a4
        movi.n   a11, 2
        movi.n   a10, 1
        l32i.n   a4, a2, 0
        l16ui    a12, a3, 2
        l32i     a4, a4, 184
        callx8   a4
        l32i.n   a4, a2, 0
        l16ui    a10, a3, 2
        l32i     a4, a4, 0x10c
        callx8   a4
        extui    a10, a10, 0, 16
        l32r     a5, .Lsec_68_8
        addx8    a10, a10, a5
        addi.n   a10, a10, 12
        movi.n   a5, 0
        l32i.n   a4, a2, 0
        l32i     a4, a4, 220
        callx8   a4
        movi     a10, 128
        addi     a14, a1, 18
        l32r     a4, .Lsec_68_14
        movi.n   a13, 43
        movi.n   a12, 1
        l8ui     a8, a4, 0
        sext     a8, a8, 7
        l32r     a9, .Lsec_68_c
        l32r     a11, .Lsec_68_10
        l16ui    a15, a9, 0
        s32i.n   a8, a1, 0
        s32i     a5, a1, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, ram_rfcal_pwrctrl
        l32r     a8, .Lsec_68_1c
        callx8   a8
        movi.n   a11, 16
        l8ui     a8, a1, 18
        extui    a10, a8, 0, 8
        addi     a9, a10, -12
        extui    a9, a9, 0, 8
        bgeu     a11, a9, .Lsec_68_f4
        or       a13, a5, a5
        l32r     a12, .Lsec_68_18
        movi     a5, 20
        sext     a8, a8, 7
        sub      a8, a5, a8
        or       a11, a3, a3
        addi     a10, a1, 16
        s16i     a8, a1, 16
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, correct_rf_ana_gain_new
        l32r     a8, .Lsec_68_20
        callx8   a8
        movi.n   a11, 1
        movi.n   a10, 5
        l32i.n   a5, a2, 0
        l16ui    a12, a3, 0
        l32i     a5, a5, 184
        callx8   a5
        movi.n   a11, 2
        movi.n   a10, 1
        l32i.n   a2, a2, 0
        l16ui    a12, a3, 2
        l32i     a2, a2, 184
        callx8   a2
        movi.n   a2, 69
        l8ui     a3, a1, 16
        sub      a2, a2, a3
        s8i      a2, a4, 0
        memw
        retw.n
.Lsec_68_f4:
        addi     a10, a10, 49
        s8i      a10, a4, 0
        memw
        retw.n
        .size   cal_rf_ana_gain, .-cal_rf_ana_gain
.Lsec_68_ff:

        .section .text.tx_pwctrl_init_cal,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_69_0, 0x00ffffff
        .literal .Lsec_69_4, chip7_phy_init_ctrl
        .literal .Lsec_69_8, g_phyFuns
        .literal .Lsec_69_c, chip7_sleep_params+0x40
        .literal .Lsec_69_10, chip7_sleep_params+0x5a
        .literal .Lsec_69_14, .Lsec_80_0
        .literal .Lsec_69_18, chip7_sleep_params
        .literal .Lsec_69_1c, 0x3ff460f8
        .literal .Lsec_69_20, rfcal_bb_atten_init
        .literal .Lsec_69_24, pwrdet_offset
        .literal .Lsec_69_28, set_channel_rfpll_freq
        .literal .Lsec_69_2c, cal_rf_ana_gain
        .literal .Lsec_69_30, ram_tx_pwr_backoff
        .literal .Lsec_69_34, ram_tx_pwr_backoff
        .literal .Lsec_69_38, esp_dport_access_reg_read
        .literal .Lsec_69_3c, ram_rfcal_pwrctrl
.Lsec_69_40:
        .global tx_pwctrl_init_cal
        .type   tx_pwctrl_init_cal, @function
tx_pwctrl_init_cal:
        entry    a1, 80
        extui    a4, a2, 0, 8
        l32r     a2, .Lsec_69_4
        movi.n   a12, 0
        sext     a10, a4, 7
        extui    a3, a3, 0, 8
        l8ui     a11, a2, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, set_channel_rfpll_freq
        l32r     a8, .Lsec_69_28
        callx8   a8
        or       a11, a4, a4
        l32r     a7, .Lsec_69_8
        l32i     a2, a7, 0
        l32r     a10, .Lsec_69_c
        l32i     a2, a2, 0x16c
        callx8   a2
        bnei     a4, 6, .Lsec_69_ac
        .reloc  ., R_XTENSA_ASM_EXPAND, cal_rf_ana_gain
        l32r     a8, .Lsec_69_2c
        callx8   a8
        beqi     a3, 1, .Lsec_69_7b
        j        .Lsec_69_1a6
.Lsec_69_7b:
        l32r     a5, .Lsec_69_10
        addi     a11, a1, 22
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, ram_tx_pwr_backoff
        l32r     a8, .Lsec_69_30
        callx8   a8
        extui    a2, a10, 0, 16
        bnez     a10, .Lsec_69_95
        l8ui     a3, a1, 22
        beqi     a3, 1, .Lsec_69_d2
.Lsec_69_95:
        addi     a11, a1, 22
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, ram_tx_pwr_backoff
        l32r     a8, .Lsec_69_34
        callx8   a8
        add.n    a2, a2, a10
        sext     a2, a2, 15
        beqz.n   a10, .Lsec_69_b5
        j        .Lsec_69_bb
        .byte    0x00
        .byte    0x00
.Lsec_69_ac:
        l32r     a5, .Lsec_69_10
        bnei     a3, 1, .Lsec_69_d2
        j        .Lsec_69_7b
.Lsec_69_b5:
        l8ui     a3, a1, 22
        beqi     a3, 1, .Lsec_69_d2
.Lsec_69_bb:
        addi.n   a2, a2, 1
        extui    a12, a2, 31, 1
        add.n    a12, a12, a2
        srai     a12, a12, 1
        extui    a3, a12, 0, 8
        sext     a2, a12, 15
        s32i.n   a3, a1, 32
        mov.n    a6, a3
        j        .Lsec_69_da
.Lsec_69_d2:
        movi.n   a6, 0
        s32i.n   a6, a1, 32
        mov.n    a12, a6
        mov.n    a2, a6
.Lsec_69_da:
        addi     a3, a1, 16
        mov.n    a11, a5
        l32i.n   a7, a7, 0
        mov.n    a10, a3
        addi.n   a4, a4, -1
        extui    a4, a4, 0, 8
        l32i     a7, a7, 0x18c
        callx8   a7
        movi.n   a7, 10
        bltu     a7, a4, .Lsec_69_11c
.Lsec_69_f3:
        l32r     a7, .Lsec_69_14
        movi     a11, 255
        add.n    a4, a7, a4
        l8ui     a7, a4, 0
        extui    a14, a7, 0, 16
        slli     a5, a14, 3
        addx2    a14, a14, a14
        ssl      a5
        sll      a4, a11
        slli     a14, a14, 1
        movi.n   a11, -1
        xor      a11, a11, a4
        addi.n   a8, a14, 5
        j        .Lsec_69_127
        .byte    0x00
        .byte    0x00
.Lsec_69_11c:
        l32r     a11, .Lsec_69_0
        movi.n   a8, 23
        movi.n   a14, 18
        movi.n   a5, 24
        movi.n   a7, 3
.Lsec_69_127:
        l32r     a4, .Lsec_69_18
        addx2    a12, a7, a4
        l32r     a7, .Lsec_69_1c
        mov.n    a10, a7
        s16i     a2, a12, 4
        s32i.n   a8, a1, 44
        s32i.n   a11, a1, 36
        s32i.n   a14, a1, 40
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_69_38
        callx8   a8
        movi.n   a13, 43
        movi.n   a12, 6
        l32i.n   a11, a1, 36
        and      a10, a10, a11
        mov.n    a11, a3
        l32i.n   a9, a1, 32
        ssl      a5
        sll      a5, a9
        or       a9, a10, a5
        movi     a10, 128
        l32r     a5, .Lsec_69_20
        l32i.n   a14, a1, 40
        addi     a14, a14, 96
        add.n    a14, a4, a14
        memw
        s32i.n   a9, a7, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l8ui     a3, a5, 0
        add.n    a6, a6, a3
        extui    a6, a6, 0, 8
        sext     a3, a6, 7
        l32r     a7, .Lsec_69_24
        l16ui    a15, a7, 0
        movi.n   a7, 0
        s32i.n   a3, a1, 0
        s32i.n   a7, a1, 4
        s8i      a6, a5, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, ram_rfcal_pwrctrl
        l32r     a8, .Lsec_69_3c
        callx8   a8
        l32i.n   a8, a1, 44
        add.n    a4, a4, a8
        l8ui     a3, a4, 96
        addi     a3, a3, 43
        s8i      a3, a5, 0
        memw
        retw.n
.Lsec_69_1a6:
        l32i.n   a2, a7, 0
        addi     a3, a1, 16
        movi.n   a6, 0
        movi.n   a12, 0
        mov.n    a10, a3
        l32i     a2, a2, 0x18c
        movi.n   a4, 5
        l32r     a11, .Lsec_69_10
        callx8   a2
        mov.n    a2, a6
        s32i.n   a6, a1, 32
        j        .Lsec_69_f3
        .size   tx_pwctrl_init_cal, .-tx_pwctrl_init_cal
.Lsec_69_1c3:

        .section .text.tx_pwctrl_init,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_70_0, chip7_sleep_params+0x6c
        .literal .Lsec_70_4, chip7_sleep_params
        .literal .Lsec_70_8, 0x00100000
        .literal .Lsec_70_c, g_phyFuns
        .literal .Lsec_70_10, tx_rf_ana_gain
        .literal .Lsec_70_14, txcal_debuge_mode
        .literal .Lsec_70_18, tx_pwctrl_init_cal
        .literal .Lsec_70_1c, tx_pwctrl_init_cal
        .literal .Lsec_70_20, tx_pwctrl_init_cal
.Lsec_70_24:
        .global tx_pwctrl_init
        .type   tx_pwctrl_init, @function
tx_pwctrl_init:
        entry    a1, 32
        l32r     a3, .Lsec_70_4
        extui    a2, a2, 0, 8
        l32i.n   a10, a3, 0
        l32r     a4, .Lsec_70_8
        and      a10, a10, a4
        bnez     a10, .Lsec_70_b4
        .reloc  ., R_XTENSA_ASM_EXPAND, txcal_debuge_mode
        l32r     a8, .Lsec_70_14
        callx8   a8
        mov.n    a11, a2
        movi.n   a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, tx_pwctrl_init_cal
        l32r     a8, .Lsec_70_18
        callx8   a8
        mov.n    a11, a2
        movi.n   a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, tx_pwctrl_init_cal
        l32r     a8, .Lsec_70_1c
        callx8   a8
        mov.n    a11, a2
        movi.n   a10, 11
        .reloc  ., R_XTENSA_ASM_EXPAND, tx_pwctrl_init_cal
        l32r     a8, .Lsec_70_20
        callx8   a8
        l16ui    a2, a3, 8
        movi.n   a9, 6
        l32r     a8, .Lsec_70_0
        s16i     a2, a3, 10
        memw
        loop     a9, .Lsec_70_80
        nop.n
        nop.n
        nop.n
        nop.n
        l8ui     a10, a8, 0
        addi.n   a8, a8, 1
        s8i      a10, a8, 5
        memw
.Lsec_70_80:
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a2, .Lsec_70_c
        l32i     a2, a2, 0
        l32i     a2, a2, 0x124
        callx8   a2
        l32i.n   a8, a3, 0
        or       a4, a8, a4
        l32r     a2, .Lsec_70_10
        l16ui    a8, a2, 0
        l16ui    a2, a2, 2
        s16i     a2, a3, 190
        movi.n   a2, 14
        s16i     a8, a3, 188
        s32i.n   a4, a3, 0
        s8i      a2, a3, 81
        memw
        retw.n
.Lsec_70_b4:
        l32r     a4, .Lsec_70_10
        l16ui    a8, a3, 188
        l16ui    a2, a3, 190
        s16i     a8, a4, 0
        s16i     a2, a4, 2
        memw
        retw.n
        .size   tx_pwctrl_init, .-tx_pwctrl_init
.Lsec_70_c8:

        .section .text.bt_tx_pwctrl_init,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_71_0, .Lsec_79_0
        .literal .Lsec_71_4, chip7_sleep_params
        .literal .Lsec_71_8, 0x00008000
        .literal .Lsec_71_c, chip7_sleep_params+0x4
        .literal .Lsec_71_10, chip7_phy_init_ctrl
        .literal .Lsec_71_14, g_phyFuns
        .literal .Lsec_71_18, chip7_sleep_params+0x40
        .literal .Lsec_71_1c, bt_tx_pa_gain
        .literal .Lsec_71_20, chip7_sleep_params+0xa0
        .literal .Lsec_71_24, pwrdet_offset
        .literal .Lsec_71_28, set_channel_rfpll_freq
        .literal .Lsec_71_2c, correct_rf_ana_gain_new
        .literal .Lsec_71_30, bt_index_to_bb
        .literal .Lsec_71_34, txcal_debuge_mode
        .literal .Lsec_71_38, ram_rfcal_pwrctrl
        .literal .Lsec_71_3c, bt_index_to_bb
        .literal .Lsec_71_40, txcal_debuge_mode
        .literal .Lsec_71_44, ram_rfcal_pwrctrl
.Lsec_71_48:
        .global bt_tx_pwctrl_init
        .type   bt_tx_pwctrl_init, @function
bt_tx_pwctrl_init:
        entry    a1, 80
        l32r     a4, .Lsec_71_4
        l32r     a3, .Lsec_71_8
        l32i.n   a2, a4, 0
        bnone    a2, a3, .Lsec_71_59
        j        .Lsec_71_23c
.Lsec_71_59:
        mov.n    a3, a4
        movi     a7, 7
        l32r     a6, .Lsec_71_0
        l32r     a2, .Lsec_71_14
        l32r     a5, .Lsec_71_1c
.Lsec_71_67:
        movi     a12, 0
        sext     a10, a7, 7
        l32r     a8, .Lsec_71_10
        l8ui     a11, a8, 1
        movi.n   a8, 46
        s8i      a8, a1, 20
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, set_channel_rfpll_freq
        l32r     a8, .Lsec_71_28
        callx8   a8
        or       a11, a7, a7
        l32i     a8, a2, 0
        l32r     a10, .Lsec_71_18
        l32i     a8, a8, 0x16c
        callx8   a8
        beqi     a7, 7, .Lsec_71_96
        j        .Lsec_71_178
.Lsec_71_96:
        movi.n   a7, 0
        s32i.n   a3, a1, 32
        j        .Lsec_71_c8
.Lsec_71_9d:
        l8ui     a7, a1, 21
        movi.n   a14, 20
        extui    a9, a7, 0, 8
        addi     a8, a9, -16
        sext     a7, a7, 7
        sub      a7, a14, a7
        extui    a8, a8, 0, 8
        movi     a14, 8
        bltu     a14, a8, .Lsec_71_ba
        j        .Lsec_71_15e
.Lsec_71_ba:
        s16i     a7, a1, 16
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, correct_rf_ana_gain_new
        l32r     a8, .Lsec_71_2c
        callx8   a8
        movi.n   a7, 1
.Lsec_71_c8:
        movi.n   a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, bt_index_to_bb
        l32r     a8, .Lsec_71_30
        callx8   a8
        s16i     a10, a1, 18
        memw
        movi.n   a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, txcal_debuge_mode
        l32r     a8, .Lsec_71_34
        callx8   a8
        movi.n   a11, 1
        movi.n   a10, 5
        l32i     a8, a2, 0
        l16ui    a12, a5, 0
        l32i     a8, a8, 184
        callx8   a8
        movi.n   a11, 2
        movi.n   a10, 1
        l32i     a8, a2, 0
        l16ui    a12, a1, 18
        l32i     a8, a8, 184
        callx8   a8
        movi.n   a11, 1
        mov.n    a10, a11
        l32i     a8, a2, 0
        l32i     a9, a8, 188
        l32i     a3, a8, 184
        callx8   a9
        movi.n   a8, 2
        or       a10, a10, a8
        movi.n   a11, 1
        extui    a12, a10, 0, 16
        mov.n    a10, a11
        callx8   a3
        l32i.n   a8, a2, 0
        l32r     a10, .Lsec_71_20
        l32i     a8, a8, 220
        callx8   a8
        movi.n   a8, 0
        addi     a11, a1, 20
        l32r     a3, .Lsec_71_24
        movi.n   a13, 11
        movi.n   a12, 1
        movi.n   a10, 32
        addi     a14, a1, 21
        l16ui    a15, a3, 0
        movi.n   a3, 35
        s32i.n   a8, a1, 4
        s32i.n   a3, a1, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, ram_rfcal_pwrctrl
        l32r     a8, .Lsec_71_38
        callx8   a8
        movi.n   a13, 1
        addi     a12, a1, 18
        mov.n    a11, a5
        addi     a10, a1, 16
        beqz     a7, .Lsec_71_9d
        l32i.n   a3, a1, 32
        l8ui     a9, a1, 21
        j        .Lsec_71_160
        .byte    0x00
.Lsec_71_15e:
        l32i.n   a3, a1, 32
.Lsec_71_160:
        l32i.n   a7, a2, 0
        l16ui    a8, a1, 18
        l32i     a7, a7, 0x124
        s16i     a8, a4, 144
        s8i      a9, a4, 138
        memw
        callx8   a7
        j        .Lsec_71_218
        .byte    0x00
.Lsec_71_178:
        movi.n   a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, bt_index_to_bb
        l32r     a8, .Lsec_71_3c
        callx8   a8
        s16i     a10, a1, 18
        memw
        movi.n   a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, txcal_debuge_mode
        l32r     a8, .Lsec_71_40
        callx8   a8
        movi.n   a11, 1
        movi.n   a10, 5
        l32i     a7, a2, 0
        l16ui    a12, a5, 0
        l32i     a7, a7, 184
        callx8   a7
        movi.n   a11, 2
        movi.n   a10, 1
        l32i     a7, a2, 0
        l16ui    a12, a1, 18
        l32i     a7, a7, 184
        callx8   a7
        movi.n   a11, 1
        mov.n    a10, a11
        l32i     a7, a2, 0
        l32i     a8, a7, 188
        l32i     a7, a7, 184
        callx8   a8
        movi.n   a8, 2
        or       a10, a10, a8
        movi.n   a11, 1
        extui    a12, a10, 0, 16
        or       a10, a11, a11
        callx8   a7
        l32i.n   a7, a2, 0
        l32r     a10, .Lsec_71_20
        l32i     a7, a7, 220
        callx8   a7
        movi.n   a7, 0
        addi     a14, a1, 21
        l32r     a8, .Lsec_71_24
        movi.n   a13, 11
        movi.n   a12, 1
        addi     a11, a1, 20
        movi.n   a10, 32
        l16ui    a15, a8, 0
        movi.n   a8, 25
        s32i.n   a7, a1, 4
        s32i.n   a8, a1, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, ram_rfcal_pwrctrl
        l32r     a8, .Lsec_71_44
        callx8   a8
        l8ui     a8, a4, 138
        l8ui     a7, a1, 21
        sub      a7, a7, a8
        l32i.n   a8, a2, 0
        s8i      a7, a3, 139
        memw
        nop.n
        nop.n
        nop.n
        nop.n
        l32i     a7, a8, 0x124
        callx8   a7
.Lsec_71_218:
        addi.n   a3, a3, 1
        addi.n   a6, a6, 1
        l32r     a8, .Lsec_71_c
        beq      a8, a3, .Lsec_71_229
        l8ui     a7, a6, 0
        j        .Lsec_71_67
        .byte    0x00
.Lsec_71_229:
        l32i.n   a2, a4, 0
        l32r     a3, .Lsec_71_8
        or       a2, a2, a3
        l8ui     a3, a4, 142
        s32i.n   a2, a4, 0
        s8i      a3, a4, 143
        memw
.Lsec_71_23c:
        retw.n
        .size   bt_tx_pwctrl_init, .-bt_tx_pwctrl_init
.Lsec_71_23e:

        .section .text.set_bt_chan_cal_interp,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_72_0, chip7_sleep_params
        .literal .Lsec_72_4, 0x66666667
        .literal .Lsec_72_8, 0x38e38e39
.Lsec_72_c:
        .global set_bt_chan_cal_interp
        .type   set_bt_chan_cal_interp, @function
set_bt_chan_cal_interp:
        entry    a1, 32
        extui    a2, a2, 0, 8
        movi.n   a8, 10
        bltu     a8, a2, .Lsec_72_20
        l32r     a2, .Lsec_72_0
        l8ui     a2, a2, 140
        retw.n
        .byte    0x00
.Lsec_72_20:
        movi.n   a8, 20
        bltu     a8, a2, .Lsec_72_46
        l32r     a10, .Lsec_72_0
        addi     a2, a2, -10
        l8ui     a8, a10, 140
        sext     a11, a8, 7
        l8ui     a9, a10, 141
        sext     a9, a9, 7
        sub      a9, a9, a11
        mull     a2, a9, a2
        l32r     a9, .Lsec_72_4
        srai     a10, a2, 31
        j        .Lsec_72_b3
.Lsec_72_46:
        movi.n   a8, 40
        bltu     a8, a2, .Lsec_72_5d
        l32r     a10, .Lsec_72_0
        addi     a2, a2, -20
        l8ui     a8, a10, 141
        sext     a11, a8, 7
        l8ui     a9, a10, 139
        j        .Lsec_72_71
.Lsec_72_5d:
        movi.n   a8, 60
        bltu     a8, a2, .Lsec_72_90
        l32r     a10, .Lsec_72_0
        addi     a2, a2, -40
        l8ui     a8, a10, 139
        sext     a11, a8, 7
        l8ui     a9, a10, 142
.Lsec_72_71:
        sext     a9, a9, 7
        sub      a9, a9, a11
        mull     a2, a9, a2
        l32r     a9, .Lsec_72_4
        srai     a10, a2, 31
        mulsh    a2, a2, a9
        srai     a2, a2, 3
        sub      a2, a2, a10
        add.n    a2, a2, a8
        extui    a2, a2, 0, 8
        retw.n
.Lsec_72_90:
        movi.n   a8, 78
        bltu     a8, a2, .Lsec_72_c4
        l32r     a10, .Lsec_72_0
        addi     a2, a2, -60
        l8ui     a8, a10, 142
        sext     a11, a8, 7
        l8ui     a9, a10, 143
        sext     a9, a9, 7
        sub      a9, a9, a11
        mull     a2, a9, a2
        l32r     a9, .Lsec_72_8
        srai     a10, a2, 31
.Lsec_72_b3:
        mulsh    a2, a2, a9
        srai     a2, a2, 2
        sub      a2, a2, a10
        add.n    a2, a2, a8
        extui    a2, a2, 0, 8
        retw.n
        .byte    0x00
.Lsec_72_c4:
        l32r     a2, .Lsec_72_0
        l8ui     a2, a2, 143
        retw.n
        .size   set_bt_chan_cal_interp, .-set_bt_chan_cal_interp
.Lsec_72_cc:

        .section .text.phy_set_bt_dig_gain,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_73_0, chip7_sleep_params
        .literal .Lsec_73_4, set_bt_chan_cal_interp
        .literal .Lsec_73_8, wr_bt_tx_atten
.Lsec_73_c:
        .global phy_set_bt_dig_gain
        .type   phy_set_bt_dig_gain, @function
phy_set_bt_dig_gain:
        entry    a1, 48
        extui    a10, a2, 0, 8
        .reloc  ., R_XTENSA_ASM_EXPAND, set_bt_chan_cal_interp
        l32r     a8, .Lsec_73_4
        callx8   a8
        l32r     a12, .Lsec_73_0
        extui    a10, a10, 0, 8
        mov.n    a11, a1
        movi.n   a9, 8
        loop     a9, .Lsec_73_34
        addi.n   a12, a12, 1
        l8ui     a8, a12, 128
        add.n    a8, a10, a8
        s8i      a8, a11, 0
        memw
        addi.n   a11, a11, 1
.Lsec_73_34:
        mov.n    a10, a1
        .reloc  ., R_XTENSA_ASM_EXPAND, wr_bt_tx_atten
        l32r     a8, .Lsec_73_8
        callx8   a8
        movi.n   a2, 0
        retw.n
        .size   phy_set_bt_dig_gain, .-phy_set_bt_dig_gain
.Lsec_73_40:

        .section .text.ram_phy_get_vdd33,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_74_0, 0x3ff4882c
        .literal .Lsec_74_4, 0x3ff460c0
        .literal .Lsec_74_8, 0x3ff48890
        .literal .Lsec_74_c, 0x08000000
        .literal .Lsec_74_10, g_phyFuns
        .literal .Lsec_74_14, 0x3ff48838
        .literal .Lsec_74_18, 0x3fffffff
        .literal .Lsec_74_1c, 0x3ff4e05c
        .literal .Lsec_74_20, 0x00800000
        .literal .Lsec_74_24, 0xffdfffff
        .literal .Lsec_74_28, 0x3ff4e050
        .literal .Lsec_74_2c, 0xff7fffff
        .literal .Lsec_74_30, esp_dport_access_reg_read
        .literal .Lsec_74_34, esp_dport_access_reg_read
        .literal .Lsec_74_38, set_xpd_sar
        .literal .Lsec_74_3c, esp_dport_access_reg_read
        .literal .Lsec_74_40, esp_dport_access_reg_read
        .literal .Lsec_74_44, esp_dport_access_reg_read
        .literal .Lsec_74_48, esp_dport_access_reg_read
        .literal .Lsec_74_4c, esp_dport_access_reg_read
        .literal .Lsec_74_50, esp_dport_access_reg_read
        .literal .Lsec_74_54, esp_dport_access_reg_read
        .literal .Lsec_74_58, esp_dport_access_reg_read
        .literal .Lsec_74_5c, ets_delay_us
        .literal .Lsec_74_60, esp_dport_access_reg_read
        .literal .Lsec_74_64, esp_dport_access_reg_read
        .literal .Lsec_74_68, esp_dport_access_reg_read
.Lsec_74_6c:
        .global ram_phy_get_vdd33
        .type   ram_phy_get_vdd33, @function
ram_phy_get_vdd33:
        entry    a1, 48
        l32r     a5, .Lsec_74_0
        movi     a2, -225
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_74_30
        callx8   a8
        movi     a3, 128
        l32r     a4, .Lsec_74_4
        and      a10, a10, a2
        or       a2, a10, a3
        or       a10, a4, a4
        memw
        s32i     a2, a5, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_74_34
        callx8   a8
        or       a3, a10, a3
        movi.n   a10, 1
        memw
        s32i     a3, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, set_xpd_sar
        l32r     a8, .Lsec_74_38
        callx8   a8
        l32r     a3, .Lsec_74_8
        or       a10, a3, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_74_3c
        callx8   a8
        movi.n   a14, 7
        l32r     a2, .Lsec_74_c
        movi.n   a15, 1
        or       a2, a10, a2
        mov.n    a13, a14
        movi.n   a12, 9
        l32r     a6, .Lsec_74_10
        movi.n   a11, 2
        movi     a10, 107
        memw
        s32i.n   a2, a3, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a2, a6, 0
        l32i     a2, a2, 168
        callx8   a2
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_74_40
        callx8   a8
        movi.n   a2, 16
        l32r     a3, .Lsec_74_14
        or       a2, a10, a2
        mov.n    a10, a3
        memw
        s32i.n   a2, a5, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_74_44
        callx8   a8
        l32r     a2, .Lsec_74_18
        and      a2, a10, a2
        l32r     a4, .Lsec_74_1c
        or       a10, a4, a4
        memw
        s32i     a2, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_74_48
        callx8   a8
        l32r     a2, .Lsec_74_20
        or       a2, a10, a2
        mov.n    a10, a4
        memw
        s32i.n   a2, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_74_4c
        callx8   a8
        l32r     a2, .Lsec_74_24
        and      a10, a10, a2
        l32r     a3, .Lsec_74_28
        memw
        s32i.n   a10, a4, 0
.Lsec_74_136:
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_74_50
        callx8   a8
        extui    a10, a10, 24, 3
        bnez     a10, .Lsec_74_136
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_74_54
        callx8   a8
        movi.n   a2, -3
        and      a10, a10, a2
        memw
        s32i.n   a10, a3, 0
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_74_58
        callx8   a8
        movi.n   a2, 2
        or       a10, a10, a2
        memw
        s32i.n   a10, a3, 0
        movi     a10, 10
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_74_5c
        callx8   a8
.Lsec_74_171:
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_74_60
        callx8   a8
        extui    a2, a10, 24, 3
        bnez     a2, .Lsec_74_171
        l32i.n   a3, a6, 0
        mov.n    a10, a1
        l32i     a3, a3, 0x12c
        callx8   a3
        mov.n    a9, a1
        movi.n   a3, 8
        loop     a3, .Lsec_74_19a
        l16ui    a8, a9, 0
        addi.n   a9, a9, 2
        add.n    a2, a2, a8
        extui    a2, a2, 0, 16
.Lsec_74_19a:
        l32i.n   a3, a6, 0
        movi.n   a14, 7
        movi.n   a15, 0
        mov.n    a13, a14
        movi     a12, 9
        l32i     a3, a3, 168
        movi.n   a11, 2
        movi     a10, 107
        callx8   a3
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_74_64
        callx8   a8
        movi.n   a3, -17
        and      a10, a10, a3
        memw
        s32i.n   a10, a5, 0
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_74_68
        callx8   a8
        addi.n   a2, a2, 4
        extui    a2, a2, 3, 16
        l32r     a3, .Lsec_74_2c
        and      a10, a10, a3
        memw
        s32i.n   a10, a4, 0
        retw.n
        .size   ram_phy_get_vdd33, .-ram_phy_get_vdd33
.Lsec_74_1dc:

        .section .text.txpwr_offset,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_75_0, chip7_phy_init_ctrl+0xe
        .literal .Lsec_75_4, chip7_sleep_params+0x5a
        .literal .Lsec_75_8, chip7_sleep_params
        .literal .Lsec_75_c, 0x00400000
        .literal .Lsec_75_10, g_phyFuns
        .literal .Lsec_75_14, 0x00000d32
        .literal .Lsec_75_18, 0x00000d33
        .literal .Lsec_75_1c, temprature_sens_read
.Lsec_75_20:
        .global txpwr_offset
        .type   txpwr_offset, @function
txpwr_offset:
        entry    a1, 32
        l32r     a4, .Lsec_75_8
        l32r     a3, .Lsec_75_c
        l32i.n   a2, a4, 0
        and      a2, a2, a3
        bnez     a2, .Lsec_75_a2
        l32r     a6, .Lsec_75_10
        l32i.n   a5, a6, 0
        l32i     a10, a5, 0x198
        callx8   a10
        mov.n    a5, a10
        l32r     a8, .Lsec_75_14
        bltu     a8, a10, .Lsec_75_6e
        movi.n   a11, 3
        l32i.n   a2, a6, 0
        l32i     a2, a2, 0x174
        callx8   a2
        mov.n    a2, a10
        movi.n   a11, 3
        l32i.n   a6, a6, 0
        l32r     a10, .Lsec_75_18
        l32i     a6, a6, 0x174
        callx8   a6
        sub      a10, a2, a10
        slli     a2, a10, 1
        sext     a2, a2, 7
        addi.n   a2, a2, 2
        extui    a2, a2, 2, 8
        slli     a2, a2, 8
.Lsec_75_6e:
        .reloc  ., R_XTENSA_ASM_EXPAND, temprature_sens_read
        l32r     a8, .Lsec_75_1c
        callx8   a8
        l32r     a11, .Lsec_75_0
        movi     a9, 6
        l32r     a8, .Lsec_75_4
        loop     a9, .Lsec_75_8d
        l8ui     a12, a11, 0
        addi.n   a11, a11, 1
        s8i      a12, a8, 0
        memw
        addi.n   a8, a8, 1
.Lsec_75_8d:
        slli     a5, a5, 16
        or       a5, a5, a10
        nop.n
        l32i.n   a6, a4, 0
        or       a2, a5, a2
        or       a3, a6, a3
        s32i     a2, a4, 192
        s32i.n   a3, a4, 0
.Lsec_75_a2:
        retw.n
        .size   txpwr_offset, .-txpwr_offset
.Lsec_75_a4:

        .section .text.phy_get_bb_freqoffset,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_76_0, 0x600330a8
        .literal .Lsec_76_4, 0x3ff5c83c
        .literal .Lsec_76_8, 0x2aaaaaab
        .literal .Lsec_76_c, 0x3ff5cc88
        .literal .Lsec_76_10, 0x00003fff
        .literal .Lsec_76_14, 0x4ec4ec4f
        .literal .Lsec_76_18, esp_dport_access_reg_read
        .literal .Lsec_76_1c, esp_dport_access_reg_read
        .literal .Lsec_76_20, esp_dport_access_reg_read
.Lsec_76_24:
        .global phy_get_bb_freqoffset
        .type   phy_get_bb_freqoffset, @function
phy_get_bb_freqoffset:
        entry    a1, 32
        l32r     a10, .Lsec_76_0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_76_18
        callx8   a8
        extui    a10, a10, 16, 4
        bgeui    a10, 8, .Lsec_76_58
        l32r     a10, .Lsec_76_4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_76_1c
        callx8   a8
        sext     a10, a10, 15
        srai     a2, a10, 31
        l32r     a8, .Lsec_76_8
        mulsh    a10, a10, a8
        srai     a10, a10, 3
        sub      a2, a10, a2
        sext     a2, a2, 15
        retw.n
        .byte    0x00
        .byte    0x00
.Lsec_76_58:
        l32r     a10, .Lsec_76_c
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_76_20
        callx8   a8
        extui    a10, a10, 0, 15
        sext     a10, a10, 15
        l32r     a2, .Lsec_76_10
        bge      a2, a10, .Lsec_76_73
        addmi    a10, a10, 0xffff8000
        sext     a10, a10, 15
.Lsec_76_73:
        l32r     a8, .Lsec_76_14
        srai     a2, a10, 31
        mulsh    a10, a10, a8
        srai     a10, a10, 3
        sub      a2, a10, a2
        sext     a2, a2, 15
        retw.n
        .size   phy_get_bb_freqoffset, .-phy_get_bb_freqoffset
.Lsec_76_87:

        .section .text.phy_pwdet_always_en,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_77_0, phy_dis_pwdet_one
        .literal .Lsec_77_4, 0x3ff4e050
        .literal .Lsec_77_8, esp_dport_access_reg_read
        .literal .Lsec_77_c, ets_delay_us
        .literal .Lsec_77_10, esp_dport_access_reg_read
        .literal .Lsec_77_14, esp_dport_access_reg_read
.Lsec_77_18:
        .global phy_pwdet_always_en
        .type   phy_pwdet_always_en, @function
phy_pwdet_always_en:
        entry    a1, 32
        extui    a2, a2, 0, 8
        bnez.n   a2, .Lsec_77_58
        l32r     a2, .Lsec_77_4
        movi.n   a8, 1
        mov.n    a10, a2
        l32r     a3, .Lsec_77_0
        s8i      a8, a3, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_77_8
        callx8   a8
        movi.n   a8, -2
        and      a8, a10, a8
        movi.n   a10, 1
        memw
        s32i.n   a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_77_c
        callx8   a8
.Lsec_77_48:
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_77_10
        callx8   a8
        extui    a10, a10, 24, 3
        bnez     a10, .Lsec_77_48
        retw.n
.Lsec_77_58:
        l32r     a3, .Lsec_77_4
        movi.n   a2, 1
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_77_14
        callx8   a8
        or       a10, a10, a2
        memw
        s32i.n   a10, a3, 0
        retw.n
        .size   phy_pwdet_always_en, .-phy_pwdet_always_en
.Lsec_77_6f:

        .section .text.dpd_scale_set,"ax",@progbits
        .align  4
.Lsec_78_0:
        .global dpd_scale_set
        .type   dpd_scale_set, @function
dpd_scale_set:
        entry    a1, 32
        retw.n
        .size   dpd_scale_set, .-dpd_scale_set
.Lsec_78_5:

        .section .rodata.CSWTCH$289,"a",@progbits
        .align  4
.Lsec_79_0:
        .type   CSWTCH$289, @object
        .size   CSWTCH$289, 4
CSWTCH$289:
        .byte   0x07, 0x01, 0x03, 0x0b

        .section .rodata.CSWTCH$285,"a",@progbits
        .align  4
.Lsec_80_0:
        .type   CSWTCH$285, @object
        .size   CSWTCH$285, 11
CSWTCH$285:
        .byte   0x00, 0x03, 0x03, 0x03, 0x03, 0x01, 0x03, 0x03, 0x03, 0x03, 0x02

        .section .rodata.CSWTCH$279,"a",@progbits
        .align  4
.Lsec_81_0:
        .type   CSWTCH$279, @object
        .size   CSWTCH$279, 3
CSWTCH$279:
        .byte   0x01, 0x06, 0x0b

        .section .rodata.CSWTCH$262,"a",@progbits
        .align  4
.Lsec_82_0:
        .type   CSWTCH$262, @object
        .size   CSWTCH$262, 8
CSWTCH$262:
        .byte   0x20, 0x00, 0xa0, 0x00, 0x20, 0x01, 0x30, 0x00

        .section .rodata.CSWTCH$252,"a",@progbits
        .align  4
.Lsec_83_0:
        .type   CSWTCH$252, @object
        .size   CSWTCH$252, 8
CSWTCH$252:
        .byte   0x05, 0x03, 0x01, 0x00, 0x06, 0x04, 0x02, 0x00

        .section .rodata.CSWTCH$250,"a",@progbits
        .align  4
.Lsec_84_0:
        .type   CSWTCH$250, @object
        .size   CSWTCH$250, 6
CSWTCH$250:
        .byte   0x0e, 0x0a, 0x08, 0x07, 0x06, 0x05

        .section .rodata.dco_gain_coarse$4714,"a",@progbits
        .align  4
.Lsec_85_0:
        .type   dco_gain_coarse$4714, @object
        .size   dco_gain_coarse$4714, 6
dco_gain_coarse$4714:
        .byte   0x50, 0x47, 0x3f, 0x38, 0x32, 0x2c

        .section .data.wifi_txband_ht40,"aw",@progbits
        .align  1
.Lsec_86_0:
        .global wifi_txband_ht40
        .type   wifi_txband_ht40, @object
        .size   wifi_txband_ht40, 1
wifi_txband_ht40:
        .byte   0xc3

        .section .data.wifi_txband,"aw",@progbits
        .align  1
.Lsec_87_0:
        .global wifi_txband
        .type   wifi_txband, @object
        .size   wifi_txband, 1
wifi_txband:
        .byte   0x5f

        .section .bss.force_tx_gain_en,"aw",@nobits
        .align  1
.Lsec_88_0:
        .global force_tx_gain_en
        .type   force_tx_gain_en, @object
        .size   force_tx_gain_en, 1
force_tx_gain_en:
        .skip   1

        .section .bss.phy_pwdet_onetime_flag,"aw",@nobits
        .align  1
.Lsec_89_0:
        .global phy_pwdet_onetime_flag
        .type   phy_pwdet_onetime_flag, @object
        .size   phy_pwdet_onetime_flag, 1
phy_pwdet_onetime_flag:
        .skip   1

        .section .bss.phy_dis_pwdet_one,"aw",@nobits
        .align  1
.Lsec_90_0:
        .global phy_dis_pwdet_one
        .type   phy_dis_pwdet_one, @object
        .size   phy_dis_pwdet_one, 1
phy_dis_pwdet_one:
        .skip   1

        .section .bss.phy_tx_pwr_correct_old,"aw",@nobits
        .align  1
.Lsec_91_0:
        .global phy_tx_pwr_correct_old
        .type   phy_tx_pwr_correct_old, @object
        .size   phy_tx_pwr_correct_old, 1
phy_tx_pwr_correct_old:
        .skip   1

        .section .bss.phy_tx_pwr_error_old,"aw",@nobits
        .align  2
.Lsec_92_0:
        .global phy_tx_pwr_error_old
        .type   phy_tx_pwr_error_old, @object
        .size   phy_tx_pwr_error_old, 2
phy_tx_pwr_error_old:
        .skip   2

        .section .bss.phy_tx_pwr_error,"aw",@nobits
        .align  2
.Lsec_93_0:
        .global phy_tx_pwr_error
        .type   phy_tx_pwr_error, @object
        .size   phy_tx_pwr_error, 2
phy_tx_pwr_error:
        .skip   2

        .section .bss.phy_in_vdd33_offset,"aw",@nobits
        .align  1
.Lsec_94_0:
        .global phy_in_vdd33_offset
        .type   phy_in_vdd33_offset, @object
        .size   phy_in_vdd33_offset, 1
phy_in_vdd33_offset:
        .skip   1

        .section .bss.tx_pwctrl_track_num,"aw",@nobits
        .align  1
.Lsec_95_0:
        .global tx_pwctrl_track_num
        .type   tx_pwctrl_track_num, @object
        .size   tx_pwctrl_track_num, 1
tx_pwctrl_track_num:
        .skip   1

        .section .bss.phy_tx_power_out,"aw",@nobits
        .align  2
.Lsec_96_0:
        .global phy_tx_power_out
        .type   phy_tx_power_out, @object
        .size   phy_tx_power_out, 2
phy_tx_power_out:
        .skip   2

        .section .data.rfcal_bb_atten_init,"aw",@progbits
        .align  1
.Lsec_97_0:
        .global rfcal_bb_atten_init
        .type   rfcal_bb_atten_init, @object
        .size   rfcal_bb_atten_init, 1
rfcal_bb_atten_init:
        .byte   0x3c
