.section .text.mac_tx_set_plcp0,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_51_0, 0x000fffff
        .literal .Lsec_51_4, 0x002fffff
        .literal .Lsec_51_8, 0xf8ffffff
        .literal .Lsec_51_c, 0x00ffffff
        .literal .Lsec_51_10, 0x07fee7a4
.Lsec_51_14:
        .global mac_tx_set_plcp0
        .type   mac_tx_set_plcp0, @function
mac_tx_set_plcp0:
        entry    a1, 32
        l32i.n   a10, a2, 0
        l32r     a9, .Lsec_51_0
        l32i.n   a8, a10, 4
        and      a8, a8, a9
        movi.n   a9, 1
        slli     a9, a9, 21
        l32i.n   a11, a10, 44
        or       a8, a8, a9
        l16si    a10, a2, 20
        l32r     a9, .Lsec_51_4
        and      a8, a8, a9
        l32i.n   a9, a11, 0
        bgei     a10, 1, .Lsec_51_58
        movi     a10, 192
        and      a10, a9, a10
        beqi     a10, 128, .Lsec_51_58
        l8ui     a10, a11, 12
        movi.n   a11, 15
        addi     a10, a10, -16
        extui    a10, a10, 0, 8
        bgeu     a11, a10, .Lsec_51_58
        movi.n   a10, 1
        slli     a10, a10, 22
        or       a8, a8, a10
.Lsec_51_58:
        movi     a10, 0x402
        bany     a9, a10, .Lsec_51_93
        movi.n   a10, 9
        slli     a10, a10, 19
        movi.n   a12, 1
        and      a10, a9, a10
        slli     a12, a12, 22
        movi.n   a11, 1
        beq      a10, a12, .Lsec_51_93
        l32r     a10, .Lsec_51_8
        and      a10, a8, a10
        bbci     a9, 20, .Lsec_51_84
        movi.n   a8, 3
        slli     a8, a8, 24
        or       a8, a10, a8
        j        .Lsec_51_93
.Lsec_51_84:
        bbci     a9, 19, .Lsec_51_8d
        slli     a11, a11, 25
        j        .Lsec_51_90
.Lsec_51_8d:
        slli     a11, a11, 24
.Lsec_51_90:
        or       a8, a10, a11
.Lsec_51_93:
        extui    a10, a9, 9, 1
        extui    a9, a9, 8, 1
        slli     a10, a10, 4
        slli     a9, a9, 3
        or       a9, a10, a9
        movi.n   a11, -25
        extui    a10, a8, 24, 8
        and      a10, a10, a11
        or       a9, a9, a10
        l32r     a10, .Lsec_51_c
        slli     a9, a9, 24
        and      a8, a8, a10
        or       a8, a8, a9
        l8ui     a9, a2, 4
        l32r     a2, .Lsec_51_10
        sub      a2, a2, a9
        slli     a2, a2, 3
        memw
        s32i.n   a8, a2, 0
        movi.n   a2, 0
        memw
        retw.n
        .size   mac_tx_set_plcp0, .-mac_tx_set_plcp0
.Lsec_51_d1:

        .section .text.mac_tx_set_duration,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_52_0, 0x3ff74268
.Lsec_52_4:
        .global mac_tx_set_duration
        .type   mac_tx_set_duration, @function
mac_tx_set_duration:
        entry    a1, 32
        l32i.n   a9, a2, 0
        l32i.n   a8, a9, 4
        l32i.n   a9, a9, 44
        l32i.n   a9, a9, 0
        l32i     a8, a8, 4
        bbci     a9, 18, .Lsec_52_18
        addi     a8, a8, 4
.Lsec_52_18:
        l16ui    a10, a8, 2
        slli     a10, a10, 16
        l16ui    a9, a8, 2
        or       a9, a9, a10
        l8ui     a8, a2, 4
        slli     a2, a8, 4
        sub      a8, a8, a2
        l32r     a2, .Lsec_52_0
        addx4    a8, a8, a2
        movi.n   a2, 0
        memw
        s32i.n   a9, a8, 0
        memw
        retw.n
        .size   mac_tx_set_duration, .-mac_tx_set_duration
.Lsec_52_3f:

        .section .text.hal_mac_tx_set_ppdu,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_53_0, mac_tx_set_plcp0
        .literal .Lsec_53_4, mac_tx_set_plcp1
        .literal .Lsec_53_8, mac_tx_set_plcp2
        .literal .Lsec_53_c, mac_tx_set_duration
        .literal .Lsec_53_10, mac_tx_set_htsig
.Lsec_53_14:
        .global hal_mac_tx_set_ppdu
        .type   hal_mac_tx_set_ppdu, @function
hal_mac_tx_set_ppdu:
        entry    a1, 32
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, mac_tx_set_plcp0
        l32r     a8, .Lsec_53_0
        callx8   a8
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, mac_tx_set_plcp1
        l32r     a8, .Lsec_53_4
        callx8   a8
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, mac_tx_set_plcp2
        l32r     a8, .Lsec_53_8
        callx8   a8
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, mac_tx_set_duration
        l32r     a8, .Lsec_53_c
        callx8   a8
        mov.n    a11, a3
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, mac_tx_set_htsig
        l32r     a8, .Lsec_53_10
        callx8   a8
        movi.n   a2, 0
        retw.n
        .size   hal_mac_tx_set_ppdu, .-hal_mac_tx_set_ppdu
.Lsec_53_46:

        .section .wifi0iram.2,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_54_0, 0x3ff73d1c
.Lsec_54_4:
        .global hal_mac_tx_config_timeout
        .type   hal_mac_tx_config_timeout, @function
hal_mac_tx_config_timeout:
        entry    a1, 32
        l8ui     a8, a2, 4
        movi.n   a10, -1
        slli     a8, a8, 3
        slli     a10, a10, 12
        extui    a3, a3, 0, 12
        l32r     a9, .Lsec_54_0
        movi.n   a2, 0
        sub      a9, a9, a8
        memw
        l32i.n   a8, a9, 0
        and      a8, a8, a10
        or       a8, a8, a3
        memw
        s32i.n   a8, a9, 0
        memw
        retw.n
        .size   hal_mac_tx_config_timeout, .-hal_mac_tx_config_timeout
.Lsec_54_32:

        .section .wifi0iram.3,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_55_0, 0x3ff73d1c
        .literal .Lsec_55_4, 0xf0ffffff
        .literal .Lsec_55_8, 0xffc00fff
.Lsec_55_c:
        .global hal_mac_tx_config_edca
        .type   hal_mac_tx_config_edca, @function
hal_mac_tx_config_edca:
        entry    a1, 32
        l8ui     a8, a2, 4
        movi.n   a11, 15
        slli     a8, a8, 3
        slli     a11, a11, 24
        l32r     a9, .Lsec_55_0
        sub      a9, a9, a8
        l8ui     a8, a2, 5
        slli     a8, a8, 24
        and      a8, a8, a11
        memw
        l32i.n   a10, a9, 0
        l32r     a11, .Lsec_55_4
        and      a10, a10, a11
        or       a8, a8, a10
        movi     a11, 0x3ff
        memw
        s32i.n   a8, a9, 0
        slli     a11, a11, 12
        l16ui    a8, a2, 6
        slli     a8, a8, 12
        and      a8, a8, a11
        memw
        l32i.n   a10, a9, 0
        l32r     a2, .Lsec_55_8
        and      a10, a10, a2
        or       a8, a8, a10
        movi.n   a2, 0
        memw
        s32i.n   a8, a9, 0
        memw
        retw.n
        .size   hal_mac_tx_config_edca, .-hal_mac_tx_config_edca
.Lsec_55_65:

        .section .wifi0iram.4,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_56_0, 0x07fee7a4
.Lsec_56_4:
        .global hal_mac_txq_enable
        .type   hal_mac_txq_enable, @function
hal_mac_txq_enable:
        entry    a1, 32
        l32r     a8, .Lsec_56_0
        extui    a2, a2, 0, 8
        sub      a8, a8, a2
        slli     a8, a8, 3
        movi.n   a10, -1
        memw
        l32i.n   a9, a8, 0
        slli     a10, a10, 30
        or       a9, a9, a10
        memw
        s32i.n   a9, a8, 0
        memw
        retw.n
        .size   hal_mac_txq_enable, .-hal_mac_txq_enable
.Lsec_56_2a:

        .section .wifiextrairam.5,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_57_0, 0x07fee7a4
        .literal .Lsec_57_4, 0x3fffffff
.Lsec_57_8:
        .global hal_mac_txq_disable
        .type   hal_mac_txq_disable, @function
hal_mac_txq_disable:
        entry    a1, 32
        l32r     a8, .Lsec_57_0
        extui    a2, a2, 0, 8
        sub      a8, a8, a2
        slli     a8, a8, 3
        memw
        l32i.n   a9, a8, 0
        l32r     a10, .Lsec_57_4
        and      a9, a9, a10
        memw
        s32i.n   a9, a8, 0
        memw
        retw.n
        .size   hal_mac_txq_disable, .-hal_mac_txq_disable
.Lsec_57_2c:

        .section .wifi0iram.6,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_58_0, 0x3ff74284
        .literal .Lsec_58_4, 0x3ff74278
        .literal .Lsec_58_8, 0x3ff74274
.Lsec_58_c:
        .global hal_mac_tx_get_blockack
        .type   hal_mac_tx_get_blockack, @function
hal_mac_tx_get_blockack:
        entry    a1, 32
        extui    a2, a2, 0, 8
        slli     a8, a2, 4
        sub      a2, a2, a8
        l32r     a8, .Lsec_58_0
        addx4    a8, a2, a8
        memw
        l32i.n   a9, a8, 0
        extui    a9, a9, 12, 4
        s8i      a9, a3, 0
        memw
        memw
        l32i.n   a8, a8, 0
        extui    a8, a8, 0, 12
        s16i     a8, a3, 2
        memw
        l32r     a8, .Lsec_58_4
        addx4    a8, a2, a8
        memw
        l32i.n   a8, a8, 0
        s32i.n   a8, a3, 4
        l32r     a8, .Lsec_58_8
        addx4    a2, a2, a8
        memw
        l32i.n   a2, a2, 0
        s32i.n   a2, a3, 8
        movi.n   a2, 0
        memw
        retw.n
        .size   hal_mac_tx_get_blockack, .-hal_mac_tx_get_blockack
.Lsec_58_5b:

        .section .text.hal_mac_set_txq_invalid,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_59_0, 0x07fee7a4
        .literal .Lsec_59_4, 0xbfffffff
.Lsec_59_8:
        .global hal_mac_set_txq_invalid
        .type   hal_mac_set_txq_invalid, @function
hal_mac_set_txq_invalid:
        entry    a1, 32
        l32r     a8, .Lsec_59_0
        extui    a2, a2, 0, 8
        sub      a8, a8, a2
        slli     a8, a8, 3
        memw
        l32i.n   a9, a8, 0
        l32r     a10, .Lsec_59_4
        and      a9, a9, a10
        memw
        s32i.n   a9, a8, 0
        memw
        retw.n
        .size   hal_mac_set_txq_invalid, .-hal_mac_set_txq_invalid
.Lsec_59_2c:

        .section .text.hal_mac_is_txq_valid,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_60_0, 0x07fee7a4
.Lsec_60_4:
        .global hal_mac_is_txq_valid
        .type   hal_mac_is_txq_valid, @function
hal_mac_is_txq_valid:
        entry    a1, 32
        l32r     a8, .Lsec_60_0
        extui    a2, a2, 0, 8
        sub      a8, a8, a2
        slli     a8, a8, 3
        memw
        l32i.n   a2, a8, 0
        extui    a2, a2, 30, 1
        retw.n
        .size   hal_mac_is_txq_valid, .-hal_mac_is_txq_valid
.Lsec_60_1d:

        .section .text.hal_mac_is_txq_enabled,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_61_0, 0x07fee7a4
.Lsec_61_4:
        .global hal_mac_is_txq_enabled
        .type   hal_mac_is_txq_enabled, @function
hal_mac_is_txq_enabled:
        entry    a1, 32
        l32r     a8, .Lsec_61_0
        extui    a2, a2, 0, 8
        sub      a8, a8, a2
        slli     a8, a8, 3
        memw
        l32i.n   a2, a8, 0
        extui    a2, a2, 31, 1
        retw.n
        .size   hal_mac_is_txq_enabled, .-hal_mac_is_txq_enabled
.Lsec_61_1d:

        .section .wifi0iram.7,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_62_0, 0x3ff75144
.Lsec_62_4:
        .global hal_random
        .type   hal_random, @function
hal_random:
        entry    a1, 32
        l32r     a2, .Lsec_62_0
        memw
        l32i.n   a2, a2, 0
        retw.n
        .size   hal_random, .-hal_random
.Lsec_62_11:

        .section .text.hal_now,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_63_0, g_mesh_is_started
        .literal .Lsec_63_4, g_mesh_is_root
        .literal .Lsec_63_8, g_mesh_init_ps_type
        .literal .Lsec_63_c, g_osi_funcs_p
        .literal .Lsec_63_10, 0x3ff73c00
        .literal .Lsec_63_14, g_wifi_mac_time_delta
.Lsec_63_18:
        .global hal_now
        .type   hal_now, @function
hal_now:
        entry    a1, 32
        l32r     a8, .Lsec_63_0
        l8ui     a8, a8, 0
        beqz.n   a8, .Lsec_63_48
        l32r     a8, .Lsec_63_4
        memw
        l8ui     a8, a8, 0
        extui    a8, a8, 0, 8
        bnez.n   a8, .Lsec_63_48
        l32r     a8, .Lsec_63_8
        l32i.n   a8, a8, 0
        beqz.n   a8, .Lsec_63_48
        l32r     a8, .Lsec_63_c
        l32i     a8, a8, 0
        l32i     a10, a8, 0x110
        callx8   a10
        j        .Lsec_63_57
        .byte    0x00
.Lsec_63_48:
        l32r     a8, .Lsec_63_10
        memw
        l32i.n   a10, a8, 0
        l32r     a8, .Lsec_63_14
        l32i.n   a8, a8, 0
        add.n    a10, a10, a8
.Lsec_63_57:
        mov.n    a2, a10
        retw.n
        .size   hal_now, .-hal_now
.Lsec_63_5b:

        .section .text.hal_mac_tx_set_cca,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_64_0, 0x3ff73c58
        .literal .Lsec_64_4, 0x3fffffff
.Lsec_64_8:
        .global hal_mac_tx_set_cca
        .type   hal_mac_tx_set_cca, @function
hal_mac_tx_set_cca:
        entry    a1, 32
        l32r     a9, .Lsec_64_0
        slli     a2, a2, 30
        memw
        l32i.n   a8, a9, 0
        l32r     a10, .Lsec_64_4
        and      a8, a8, a10
        or       a8, a8, a2
        movi.n   a2, 0
        memw
        s32i.n   a8, a9, 0
        memw
        retw.n
        .size   hal_mac_tx_set_cca, .-hal_mac_tx_set_cca
.Lsec_64_2b:

        .section .text.hal_mac_disable_low_rate,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_65_0, 0x3ff7341c
        .literal .Lsec_65_4, 0x090b0b0b
        .literal .Lsec_65_8, 0x3ff73418
        .literal .Lsec_65_c, 0x09090909
        .literal .Lsec_65_10, 0x6001c860
        .literal .Lsec_65_14, 0xfffff7ff
        .literal .Lsec_65_18, 0x6001c87c
        .literal .Lsec_65_1c, phy_disable_low_rate
.Lsec_65_20:
        .global hal_mac_disable_low_rate
        .type   hal_mac_disable_low_rate, @function
hal_mac_disable_low_rate:
        entry    a1, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_disable_low_rate
        l32r     a8, .Lsec_65_1c
        callx8   a8
        l32r     a8, .Lsec_65_0
        movi     a10, 0xfffffbff
        l32r     a9, .Lsec_65_4
        memw
        s32i.n   a9, a8, 0
        l32r     a8, .Lsec_65_8
        l32r     a9, .Lsec_65_c
        memw
        s32i.n   a9, a8, 0
        l32r     a8, .Lsec_65_10
        memw
        l32i.n   a9, a8, 0
        and      a9, a9, a10
        memw
        s32i.n   a9, a8, 0
        memw
        l32i.n   a9, a8, 0
        l32r     a10, .Lsec_65_14
        and      a9, a9, a10
        memw
        s32i.n   a9, a8, 0
        l32r     a9, .Lsec_65_18
        memw
        l32i.n   a8, a9, 0
        and      a8, a8, a10
        memw
        s32i.n   a8, a9, 0
        memw
        retw.n
        .size   hal_mac_disable_low_rate, .-hal_mac_disable_low_rate
.Lsec_65_77:

        .section .text.hal_mac_enable_low_rate,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_66_0, 0x3ff7341c
        .literal .Lsec_66_4, 0x0b0b0b0b
        .literal .Lsec_66_8, 0x3ff73418
        .literal .Lsec_66_c, 0x6001c860
        .literal .Lsec_66_10, 0x6001c87c
        .literal .Lsec_66_14, phy_enable_low_rate
.Lsec_66_18:
        .global hal_mac_enable_low_rate
        .type   hal_mac_enable_low_rate, @function
hal_mac_enable_low_rate:
        entry    a1, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_enable_low_rate
        l32r     a8, .Lsec_66_14
        callx8   a8
        l32r     a8, .Lsec_66_4
        movi     a10, 0x400
        l32r     a9, .Lsec_66_0
        memw
        s32i.n   a8, a9, 0
        l32r     a9, .Lsec_66_8
        memw
        s32i.n   a8, a9, 0
        l32r     a9, .Lsec_66_c
        memw
        l32i.n   a8, a9, 0
        or       a8, a8, a10
        memw
        s32i.n   a8, a9, 0
        movi.n   a8, 1
        slli     a8, a8, 11
        memw
        l32i.n   a10, a9, 0
        or       a10, a10, a8
        memw
        s32i.n   a10, a9, 0
        l32r     a10, .Lsec_66_10
        memw
        l32i.n   a9, a10, 0
        or       a8, a9, a8
        memw
        s32i.n   a8, a10, 0
        memw
        retw.n
        .size   hal_mac_enable_low_rate, .-hal_mac_enable_low_rate
.Lsec_66_6e:

        .section .wifi0iram.8,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_67_0, 0x6001c860
        .literal .Lsec_67_4, 0x6001c87c
.Lsec_67_8:
        .global hal_mac_is_low_rate_enabled
        .type   hal_mac_is_low_rate_enabled, @function
hal_mac_is_low_rate_enabled:
        entry    a1, 32
        l32r     a2, .Lsec_67_0
        movi.n   a8, 3
        slli     a8, a8, 10
        memw
        l32i.n   a9, a2, 0
        movi.n   a2, 0
        and      a9, a9, a8
        bne      a9, a8, .Lsec_67_2c
        l32r     a2, .Lsec_67_4
        memw
        l32i     a2, a2, 0
        extui    a2, a2, 11, 1
.Lsec_67_2c:
        retw.n
        .size   hal_mac_is_low_rate_enabled, .-hal_mac_is_low_rate_enabled
.Lsec_67_2e:

        .section .text.mac_rxbuf_init,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_68_0, 0x3ff7311c
        .literal .Lsec_68_4, 0x3ffae000
        .literal .Lsec_68_8, 0x3ff73124
        .literal .Lsec_68_c, 0x3ff73118
        .literal .Lsec_68_10, 0x400a0000
        .literal .Lsec_68_14, 0x3ff73120
        .literal .Lsec_68_18, 0x3ff73080
        .literal .Lsec_68_1c, wDevCtrl
        .literal .Lsec_68_20, 0x3ff73088
        .literal .Lsec_68_24, 0xdfffffff
.Lsec_68_28:
        .global mac_rxbuf_init
        .type   mac_rxbuf_init, @function
mac_rxbuf_init:
        entry    a1, 32
        l32r     a8, .Lsec_68_4
        movi     a10, 0xffffff00
        l32r     a9, .Lsec_68_0
        memw
        s32i.n   a8, a9, 0
        l32r     a9, .Lsec_68_8
        memw
        s32i.n   a8, a9, 0
        l32r     a8, .Lsec_68_10
        l32r     a9, .Lsec_68_c
        memw
        s32i.n   a8, a9, 0
        l32r     a9, .Lsec_68_14
        memw
        s32i.n   a8, a9, 0
        l32r     a8, .Lsec_68_18
        memw
        l32i.n   a9, a8, 0
        and      a9, a9, a10
        memw
        s32i.n   a9, a8, 0
        l32r     a9, .Lsec_68_1c
        l32i.n   a10, a9, 0
        l32r     a9, .Lsec_68_20
        memw
        s32i.n   a10, a9, 0
        memw
        l32i.n   a9, a8, 0
        l32r     a10, .Lsec_68_24
        and      a9, a9, a10
        memw
        s32i.n   a9, a8, 0
        memw
        retw.n
        .size   mac_rxbuf_init, .-mac_rxbuf_init
.Lsec_68_86:

        .section .text.hal_disable_mac,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_69_0, 0x3ff73c04
        .literal .Lsec_69_4, 0x3ff73c40
        .literal .Lsec_69_8, 0x3ff73c44
        .literal .Lsec_69_c, 0x3ff73c30
.Lsec_69_10:
        .global hal_disable_mac
        .type   hal_disable_mac, @function
hal_disable_mac:
        entry    a1, 32
        l32r     a9, .Lsec_69_0
        movi     a2, 240
        memw
        l32i.n   a8, a9, 0
        or       a8, a8, a2
        memw
        s32i.n   a8, a9, 0
        movi.n   a8, 0
        l32r     a9, .Lsec_69_4
        memw
        l32i.n   a2, a9, 0
        memw
        s32i.n   a8, a9, 0
        l32r     a9, .Lsec_69_8
        memw
        s32i.n   a8, a9, 0
        l32r     a9, .Lsec_69_c
        memw
        s32i.n   a8, a9, 0
        memw
        retw.n
        .size   hal_disable_mac, .-hal_disable_mac
.Lsec_69_4a:

        .section .text.hal_enable_mac,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_70_0, 0x3ff73c04
        .literal .Lsec_70_4, 0x3ff73c40
        .literal .Lsec_70_8, 0x3ff73088
.Lsec_70_c:
        .global hal_enable_mac
        .type   hal_enable_mac, @function
hal_enable_mac:
        entry    a1, 32
        l32r     a9, .Lsec_70_0
        movi     a10, -241
        memw
        l32i.n   a8, a9, 0
        and      a8, a8, a10
        memw
        s32i.n   a8, a9, 0
        l32r     a8, .Lsec_70_4
        memw
        s32i.n   a2, a8, 0
        l32r     a8, .Lsec_70_8
        memw
        s32i.n   a3, a8, 0
        memw
        retw.n
        .size   hal_enable_mac, .-hal_enable_mac
.Lsec_70_37:

        .section .wifislpiram.9,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_71_0, 0x3ff73090
.Lsec_71_4:
        .global hal_mac_rx_read_rxdscrlast
        .type   hal_mac_rx_read_rxdscrlast, @function
hal_mac_rx_read_rxdscrlast:
        entry    a1, 32
        l32r     a2, .Lsec_71_0
        memw
        l32i.n   a2, a2, 0
        retw.n
        .size   hal_mac_rx_read_rxdscrlast, .-hal_mac_rx_read_rxdscrlast
.Lsec_71_11:

        .section .wifislprxiram.10,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_72_0, 0x3ff7308c
.Lsec_72_4:
        .global hal_mac_rx_read_rxdscrnext
        .type   hal_mac_rx_read_rxdscrnext, @function
hal_mac_rx_read_rxdscrnext:
        entry    a1, 32
        l32r     a2, .Lsec_72_0
        memw
        l32i.n   a2, a2, 0
        retw.n
        .size   hal_mac_rx_read_rxdscrnext, .-hal_mac_rx_read_rxdscrnext
.Lsec_72_11:

        .section .wifislprxiram.11,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_73_0, 0x3ff73088
.Lsec_73_4:
        .global hal_mac_rx_set_base
        .type   hal_mac_rx_set_base, @function
hal_mac_rx_set_base:
        entry    a1, 32
        l32r     a8, .Lsec_73_0
        memw
        s32i.n   a2, a8, 0
        memw
        retw.n
        .size   hal_mac_rx_set_base, .-hal_mac_rx_set_base
.Lsec_73_14:

        .section .text.mac_txrx_init,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_74_0, 0x3ff730d8
        .literal .Lsec_74_4, 0x3ff73c6c
        .literal .Lsec_74_8, 0x80802000
        .literal .Lsec_74_c, 0x3ff73c74
        .literal .Lsec_74_10, 0x3ff730f8
        .literal .Lsec_74_14, 0x3ff730fc
        .literal .Lsec_74_18, 0x3ff73100
        .literal .Lsec_74_1c, 0x3ff73104
        .literal .Lsec_74_20, 0x3ff7310c
        .literal .Lsec_74_24, 0x3ff73114
        .literal .Lsec_74_28, 0xf00fffff
        .literal .Lsec_74_2c, 0x3ff73c78
        .literal .Lsec_74_30, 0x3ff73d78
        .literal .Lsec_74_34, 0x3ff73c54
        .literal .Lsec_74_38, 0xffff00ff
        .literal .Lsec_74_3c, 0x3ff73c1c
        .literal .Lsec_74_40, 0x3ff73c20
        .literal .Lsec_74_44, 0x3ff73c24
        .literal .Lsec_74_48, 0x3ff73cac
        .literal .Lsec_74_4c, 0x3ff73c5c
        .literal .Lsec_74_50, 0x7fff0000
        .literal .Lsec_74_54, 0x3ff73c88
        .literal .Lsec_74_58, 0xf0ffffff
        .literal .Lsec_74_5c, 0x3ff73288
        .literal .Lsec_74_60, 0x3ff73084
        .literal .Lsec_74_64, 0x7fffffff
.Lsec_74_68:
        .global mac_txrx_init
        .type   mac_txrx_init, @function
mac_txrx_init:
        entry    a1, 32
        l32r     a8, .Lsec_74_4
        movi.n   a11, 64
        movi     a12, -33
        memw
        l32i.n   a9, a8, 0
        l32r     a10, .Lsec_74_8
        or       a9, a9, a10
        movi     a10, 0x100
        memw
        s32i.n   a9, a8, 0
        memw
        l32i.n   a9, a8, 0
        or       a9, a9, a10
        memw
        s32i.n   a9, a8, 0
        movi     a9, 4
        l32r     a8, .Lsec_74_0
        loop     a9, .Lsec_74_b8
        memw
        l32i.n   a10, a8, 0
        or       a10, a10, a11
        memw
        s32i.n   a10, a8, 0
        memw
        l32i.n   a10, a8, 0
        and      a10, a10, a12
        memw
        s32i.n   a10, a8, 0
        addi.n   a8, a8, 4
.Lsec_74_b8:
        movi.n   a10, 8
        movi     a13, -241
        l32r     a9, .Lsec_74_c
        movi     a14, 240
        movi.n   a15, 1
        slli     a15, a15, 30
        memw
        l32i.n   a8, a9, 0
        or       a8, a8, a10
        memw
        s32i.n   a8, a9, 0
        l32r     a9, .Lsec_74_10
        memw
        l32i.n   a8, a9, 0
        extui    a8, a8, 0, 16
        memw
        s32i.n   a8, a9, 0
        l32r     a8, .Lsec_74_14
        memw
        l32i.n   a10, a8, 0
        extui    a10, a10, 0, 16
        l32r     a12, .Lsec_74_18
        memw
        s32i.n   a10, a8, 0
        memw
        l32i.n   a10, a12, 0
        extui    a10, a10, 0, 16
        memw
        s32i.n   a10, a12, 0
        l32r     a12, .Lsec_74_1c
        memw
        l32i.n   a10, a12, 0
        extui    a10, a10, 0, 16
        memw
        s32i.n   a10, a12, 0
        movi.n   a10, 1
        slli     a10, a10, 24
        memw
        l32i.n   a12, a9, 0
        or       a12, a12, a10
        memw
        s32i.n   a12, a9, 0
        memw
        l32i.n   a12, a8, 0
        or       a10, a12, a10
        memw
        s32i.n   a10, a8, 0
        movi.n   a10, 1
        slli     a10, a10, 26
        memw
        l32i.n   a12, a9, 0
        or       a12, a12, a10
        memw
        s32i.n   a12, a9, 0
        memw
        l32i.n   a9, a8, 0
        or       a9, a9, a10
        memw
        s32i.n   a9, a8, 0
        l32r     a9, .Lsec_74_20
        memw
        l32i.n   a8, a9, 0
        and      a8, a8, a13
        l32r     a10, .Lsec_74_24
        memw
        s32i.n   a8, a9, 0
        movi.n   a9, -1
        slli     a9, a9, 31
        memw
        l32i.n   a8, a10, 0
        or       a8, a8, a9
        memw
        s32i.n   a8, a10, 0
        memw
        l32i.n   a8, a10, 0
        l32r     a12, .Lsec_74_28
        and      a8, a8, a12
        movi.n   a12, 27
        slli     a12, a12, 20
        or       a8, a8, a12
        memw
        s32i.n   a8, a10, 0
        movi.n   a12, 3
        l32r     a10, .Lsec_74_2c
        memw
        l32i.n   a8, a10, 0
        or       a8, a8, a12
        movi     a12, 0xffffff00
        memw
        s32i.n   a8, a10, 0
        l32r     a10, .Lsec_74_30
        memw
        l32i.n   a8, a10, 0
        and      a8, a8, a13
        memw
        s32i.n   a8, a10, 0
        l32r     a10, .Lsec_74_34
        memw
        l32i.n   a8, a10, 0
        and      a8, a8, a12
        movi.n   a12, 4
        or       a8, a8, a12
        memw
        s32i.n   a8, a10, 0
        memw
        l32i.n   a8, a10, 0
        l32r     a12, .Lsec_74_38
        and      a8, a8, a12
        movi     a12, 0x400
        or       a8, a8, a12
        memw
        s32i.n   a8, a10, 0
        movi.n   a10, -1
        slli     a10, a10, 12
        l32r     a8, .Lsec_74_3c
        memw
        l32i.n   a12, a8, 0
        and      a12, a12, a10
        or       a12, a12, a14
        memw
        s32i.n   a12, a8, 0
        memw
        l32i.n   a12, a8, 0
        or       a12, a12, a9
        memw
        s32i.n   a12, a8, 0
        memw
        l32i.n   a12, a8, 0
        or       a12, a12, a15
        memw
        s32i.n   a12, a8, 0
        l32r     a12, .Lsec_74_40
        memw
        l32i.n   a8, a12, 0
        and      a8, a8, a10
        or       a8, a8, a14
        memw
        s32i.n   a8, a12, 0
        l32r     a12, .Lsec_74_44
        memw
        l32i.n   a8, a12, 0
        and      a8, a8, a10
        or       a8, a8, a14
        l32r     a10, .Lsec_74_48
        memw
        s32i.n   a8, a12, 0
        memw
        l32i.n   a8, a10, 0
        and      a8, a8, a13
        or       a8, a8, a11
        memw
        s32i.n   a8, a10, 0
        l32r     a8, .Lsec_74_4c
        l32r     a11, .Lsec_74_50
        memw
        l32i.n   a10, a8, 0
        or       a10, a10, a11
        memw
        s32i.n   a10, a8, 0
        memw
        l32i.n   a10, a8, 0
        or       a9, a10, a9
        memw
        s32i.n   a9, a8, 0
        l32r     a9, .Lsec_74_54
        l32r     a10, .Lsec_74_58
        memw
        l32i.n   a8, a9, 0
        and      a8, a8, a10
        movi.n   a10, 2
        memw
        s32i.n   a8, a9, 0
        l32r     a9, .Lsec_74_5c
        memw
        l32i.n   a8, a9, 0
        or       a8, a8, a10
        memw
        s32i.n   a8, a9, 0
        l32r     a9, .Lsec_74_60
        l32r     a10, .Lsec_74_64
        memw
        l32i.n   a8, a9, 0
        and      a8, a8, a10
        memw
        s32i.n   a8, a9, 0
        memw
        retw.n
        .size   mac_txrx_init, .-mac_txrx_init
.Lsec_74_2a8:

        .section .text.hal_mac_set_rxq_policy,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_75_0, 0x0ffdcc36
.Lsec_75_4:
        .global hal_mac_set_rxq_policy
        .type   hal_mac_set_rxq_policy, @function
hal_mac_set_rxq_policy:
        entry    a1, 32
        l32r     a8, .Lsec_75_0
        extui    a2, a2, 0, 8
        add.n    a2, a2, a8
        slli     a2, a2, 2
        extui    a3, a3, 0, 8
        memw
        l32i.n   a8, a2, 0
        beqz.n   a3, .Lsec_75_24
        movi.n   a9, 2
        or       a8, a8, a9
        j        .Lsec_75_29
.Lsec_75_24:
        movi.n   a9, -3
        and      a8, a8, a9
.Lsec_75_29:
        memw
        s32i.n   a8, a2, 0
        memw
        retw.n
        .size   hal_mac_set_rxq_policy, .-hal_mac_set_rxq_policy
.Lsec_75_33:

        .section .text.mac_last_rxbuf_init,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_76_0, 0x3ff7314c
        .literal .Lsec_76_4, 0x00003000
        .literal .Lsec_76_8, 0x3ff73158
        .literal .Lsec_76_c, 0x3ff73164
        .literal .Lsec_76_10, 0x3ff73148
        .literal .Lsec_76_14, 0x3ff7309c
.Lsec_76_18:
        .global mac_last_rxbuf_init
        .type   mac_last_rxbuf_init, @function
mac_last_rxbuf_init:
        entry    a1, 32
        l32r     a8, .Lsec_76_0
        movi.n   a10, 1
        slli     a10, a10, 11
        l32r     a9, .Lsec_76_4
        memw
        s32i.n   a9, a8, 0
        movi.n   a9, 0
        l32r     a8, .Lsec_76_8
        memw
        s32i.n   a9, a8, 0
        movi.n   a9, 12
        l32r     a8, .Lsec_76_c
        memw
        s32i.n   a9, a8, 0
        l32r     a8, .Lsec_76_10
        memw
        l32i.n   a9, a8, 0
        or       a9, a9, a10
        movi     a10, 0x100
        memw
        s32i.n   a9, a8, 0
        memw
        l32i.n   a9, a8, 0
        or       a9, a9, a10
        l32r     a10, .Lsec_76_14
        memw
        s32i.n   a9, a8, 0
        movi.n   a9, 1
        slli     a9, a9, 27
        memw
        l32i.n   a8, a10, 0
        or       a8, a8, a9
        memw
        s32i.n   a8, a10, 0
        memw
        retw.n
        .size   mac_last_rxbuf_init, .-mac_last_rxbuf_init
.Lsec_76_79:

        .section .text.mac_last_rxbuf_deinit,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_77_0, 0x3ff7314c
        .literal .Lsec_77_4, 0x3ff73158
        .literal .Lsec_77_8, 0x3ff73164
        .literal .Lsec_77_c, 0x3ff73148
        .literal .Lsec_77_10, 0xfffff7ff
        .literal .Lsec_77_14, 0x3ff7309c
        .literal .Lsec_77_18, 0xf7ffffff
.Lsec_77_1c:
        .global mac_last_rxbuf_deinit
        .type   mac_last_rxbuf_deinit, @function
mac_last_rxbuf_deinit:
        entry    a1, 32
        l32r     a9, .Lsec_77_0
        movi.n   a8, 0
        memw
        s32i.n   a8, a9, 0
        l32r     a9, .Lsec_77_4
        memw
        s32i.n   a8, a9, 0
        l32r     a9, .Lsec_77_8
        memw
        s32i.n   a8, a9, 0
        l32r     a8, .Lsec_77_c
        l32r     a10, .Lsec_77_10
        memw
        l32i.n   a9, a8, 0
        and      a9, a9, a10
        movi     a10, 0xfffffeff
        memw
        s32i.n   a9, a8, 0
        memw
        l32i.n   a9, a8, 0
        and      a9, a9, a10
        memw
        s32i.n   a9, a8, 0
        l32r     a9, .Lsec_77_14
        l32r     a10, .Lsec_77_18
        memw
        l32i.n   a8, a9, 0
        and      a8, a8, a10
        memw
        s32i.n   a8, a9, 0
        memw
        retw.n
        .size   mac_last_rxbuf_deinit, .-mac_last_rxbuf_deinit
.Lsec_77_74:

        .section .text.mac_rxbuf_disable,"ax",@progbits
        .align  4
.Lsec_78_0:
        .global mac_rxbuf_disable
        .type   mac_rxbuf_disable, @function
mac_rxbuf_disable:
        entry    a1, 32
        retw.n
        .size   mac_rxbuf_disable, .-mac_rxbuf_disable
.Lsec_78_5:

        .section .text.hal_deinit,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_79_0, 0x3ff73288
        .literal .Lsec_79_4, 0x3ff73c40
        .literal .Lsec_79_8, 0xfe17c61f
        .literal .Lsec_79_c, 0x3ff73c4c
        .literal .Lsec_79_10, 0x3ff73d24
.Lsec_79_14:
        .global hal_deinit
        .type   hal_deinit, @function
hal_deinit:
        entry    a1, 32
        l32r     a9, .Lsec_79_0
        movi.n   a10, 1
        memw
        l32i.n   a8, a9, 0
        or       a8, a8, a10
        memw
        s32i.n   a8, a9, 0
        l32r     a8, .Lsec_79_4
        l32r     a10, .Lsec_79_8
        memw
        l32i.n   a9, a8, 0
        and      a9, a9, a10
        movi.n   a10, 2
        memw
        s32i.n   a9, a8, 0
        movi.n   a9, 0
        memw
        s32i.n   a9, a8, 0
        movi.n   a9, -1
        l32r     a8, .Lsec_79_c
        memw
        s32i.n   a9, a8, 0
        l32r     a8, .Lsec_79_10
        memw
        l32i.n   a9, a8, 0
        or       a9, a9, a10
        memw
        s32i     a9, a8, 0
.Lsec_79_60:
        memw
        l32i.n   a9, a8, 0
        bbci     a9, 0, .Lsec_79_60
        memw
        retw.n
        .size   hal_deinit, .-hal_deinit
.Lsec_79_6d:

        .section .iram1.12,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_80_0, 0x3ff73c48
.Lsec_80_4:
        .global hal_mac_interrupt_get_event
        .type   hal_mac_interrupt_get_event, @function
hal_mac_interrupt_get_event:
        entry    a1, 32
        l32r     a2, .Lsec_80_0
        memw
        l32i.n   a2, a2, 0
        retw.n
        .size   hal_mac_interrupt_get_event, .-hal_mac_interrupt_get_event
.Lsec_80_11:

        .section .iram1.13,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_81_0, 0x3ff73c4c
.Lsec_81_4:
        .global hal_mac_interrupt_clr_event
        .type   hal_mac_interrupt_clr_event, @function
hal_mac_interrupt_clr_event:
        entry    a1, 32
        l32r     a8, .Lsec_81_0
        memw
        s32i.n   a2, a8, 0
        memw
        retw.n
        .size   hal_mac_interrupt_clr_event, .-hal_mac_interrupt_clr_event
.Lsec_81_14:

        .section .iram1.14,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_82_0, 0x3ff73c4c
.Lsec_82_4:
        .global hal_mac_interrupt_clr_watchdog
        .type   hal_mac_interrupt_clr_watchdog, @function
hal_mac_interrupt_clr_watchdog:
        entry    a1, 32
        l32r     a10, .Lsec_82_0
        movi.n   a9, 1
        slli     a9, a9, 11
        memw
        l32i.n   a8, a10, 0
        or       a8, a8, a9
        memw
        s32i.n   a8, a10, 0
        memw
        retw.n
        .size   hal_mac_interrupt_clr_watchdog, .-hal_mac_interrupt_clr_watchdog
.Lsec_82_21:

        .section .text.hal_mac_set_addr,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_83_0, 0x07fee608
        .literal .Lsec_83_4, 0x0000ffff
.Lsec_83_8:
        .global hal_mac_set_addr
        .type   hal_mac_set_addr, @function
hal_mac_set_addr:
        entry    a1, 32
        l8ui     a8, a3, 1
        extui    a2, a2, 0, 8
        slli     a8, a8, 8
        l8ui     a9, a3, 0
        or       a8, a8, a9
        l8ui     a9, a3, 2
        slli     a9, a9, 16
        or       a9, a9, a8
        l8ui     a8, a3, 3
        slli     a8, a8, 24
        or       a8, a8, a9
        l32r     a9, .Lsec_83_0
        add.n    a2, a2, a9
        slli     a2, a2, 3
        memw
        s32i.n   a8, a2, 0
        l8ui     a8, a3, 4
        l8ui     a3, a3, 5
        slli     a3, a3, 8
        or       a3, a3, a8
        movi.n   a8, 1
        memw
        s32i.n   a3, a2, 4
        movi.n   a3, -1
        slli     a8, a8, 16
        memw
        s32i.n   a3, a2, 32
        l32r     a3, .Lsec_83_4
        memw
        s32i.n   a3, a2, 36
        memw
        l32i.n   a3, a2, 36
        or       a3, a3, a8
        memw
        s32i.n   a3, a2, 36
        memw
        retw.n
        .size   hal_mac_set_addr, .-hal_mac_set_addr
.Lsec_83_70:

        .section .text.hal_mac_set_bssid,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_84_0, 0x3ff73024
        .literal .Lsec_84_4, 0xfffeffff
        .literal .Lsec_84_8, 0x3ff73000
        .literal .Lsec_84_c, 0x3ff73004
        .literal .Lsec_84_10, 0x3ff73020
        .literal .Lsec_84_14, 0x0000ffff
.Lsec_84_18:
        .global hal_mac_set_bssid
        .type   hal_mac_set_bssid, @function
hal_mac_set_bssid:
        entry    a1, 32
        l8ui     a9, a3, 1
        extui    a2, a2, 0, 8
        slli     a9, a9, 8
        l8ui     a8, a3, 0
        or       a9, a9, a8
        l8ui     a8, a3, 2
        slli     a8, a8, 16
        or       a8, a8, a9
        l8ui     a9, a3, 3
        slli     a9, a9, 24
        or       a9, a9, a8
        l32r     a8, .Lsec_84_0
        addx8    a8, a2, a8
        memw
        l32i.n   a10, a8, 0
        l32r     a11, .Lsec_84_4
        and      a10, a10, a11
        movi.n   a11, -1
        slli     a11, a11, 16
        memw
        s32i.n   a10, a8, 0
        l32r     a10, .Lsec_84_8
        addx8    a10, a2, a10
        memw
        s32i.n   a9, a10, 0
        l32r     a10, .Lsec_84_c
        addx8    a10, a2, a10
        memw
        l32i.n   a9, a10, 0
        and      a9, a9, a11
        l8ui     a11, a3, 4
        l8ui     a3, a3, 5
        slli     a3, a3, 8
        or       a3, a3, a11
        or       a9, a9, a3
        l32r     a3, .Lsec_84_10
        addx8    a2, a2, a3
        movi.n   a3, -1
        memw
        s32i.n   a9, a10, 0
        memw
        s32i.n   a3, a2, 0
        movi.n   a3, 1
        slli     a3, a3, 16
        l32r     a2, .Lsec_84_14
        memw
        s32i.n   a2, a8, 0
        memw
        l32i.n   a2, a8, 0
        or       a2, a2, a3
        memw
        s32i.n   a2, a8, 0
        memw
        retw.n
        .size   hal_mac_set_bssid, .-hal_mac_set_bssid
.Lsec_84_b0:

        .section .rodata.hal_mac_rx_set_policy.str1.1,"aMS",@progbits,1
        .align  1
.Lsec_85_0:
        .ascii  "\377\377\377\377\377\377\000"

        .section .text.hal_mac_rx_set_policy,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_86_0, 0x0ffdcc36
        .literal .Lsec_86_4, 0x3ff73024
        .literal .Lsec_86_8, 0xfffeffff
        .literal .Lsec_86_c, .Lsec_85_0
        .literal .Lsec_86_10, 0x3ff73064
        .literal .Lsec_86_14, memcpy
        .literal .Lsec_86_18, hal_mac_set_bssid
.Lsec_86_1c:
        .global hal_mac_rx_set_policy
        .type   hal_mac_rx_set_policy, @function
hal_mac_rx_set_policy:
        entry    a1, 48
        extui    a2, a2, 0, 8
        bltui    a2, 3, .Lsec_86_28
        j        .Lsec_86_da
.Lsec_86_28:
        l32r     a8, .Lsec_86_0
        add.n    a8, a2, a8
        slli     a8, a8, 2
        bgeui    a3, 2, .Lsec_86_41
        memw
        l32i.n   a3, a8, 0
        movi     a9, 0xfffffeef
        and      a3, a3, a9
        j        .Lsec_86_4c
.Lsec_86_41:
        memw
        l32i.n   a3, a8, 0
        movi     a9, 0x110
        or       a3, a3, a9
.Lsec_86_4c:
        memw
        s32i.n   a3, a8, 0
        slli     a3, a2, 3
        bnez.n   a4, .Lsec_86_71
        l32r     a2, .Lsec_86_4
        add.n    a2, a3, a2
        memw
        l32i.n   a4, a2, 0
        l32r     a8, .Lsec_86_8
        and      a4, a4, a8
        memw
        s32i.n   a4, a2, 0
        memw
        j        .Lsec_86_a8
.Lsec_86_71:
        bnei     a4, 1, .Lsec_86_91
        l32r     a2, .Lsec_86_4
        slli     a4, a4, 16
        add.n    a2, a3, a2
        memw
        l32i.n   a8, a2, 0
        or       a8, a8, a4
        memw
        s32i.n   a8, a2, 0
        memw
        j        .Lsec_86_a8
        .byte    0x00
        .byte    0x00
.Lsec_86_91:
        l32r     a11, .Lsec_86_c
        movi.n   a12, 6
        mov.n    a10, a1
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_86_14
        callx8   a8
        mov.n    a11, a1
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_set_bssid
        l32r     a8, .Lsec_86_18
        callx8   a8
.Lsec_86_a8:
        l32r     a2, .Lsec_86_10
        add.n    a8, a3, a2
        memw
        l32i.n   a2, a8, 0
        bnez.n   a5, .Lsec_86_c5
        l32r     a3, .Lsec_86_8
        and      a2, a2, a3
        memw
        s32i.n   a2, a8, 0
        memw
        j        .Lsec_86_dc
.Lsec_86_c5:
        movi.n   a3, 1
        slli     a3, a3, 16
        or       a2, a2, a3
        memw
        s32i.n   a2, a8, 0
        movi.n   a5, 0
        memw
        j        .Lsec_86_dc
.Lsec_86_da:
        movi.n   a5, -1
.Lsec_86_dc:
        mov.n    a2, a5
        retw.n
        .size   hal_mac_rx_set_policy, .-hal_mac_rx_set_policy
.Lsec_86_e0:

        .section .text.hal_init,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_87_0, 0x3ff73d24
        .literal .Lsec_87_4, 0x3ff73c40
        .literal .Lsec_87_8, 0x3ff73c4c
        .literal .Lsec_87_c, 0x3ff730d8
        .literal .Lsec_87_10, 0xfffff6ff
        .literal .Lsec_87_14, 0x01e839e0
        .literal .Lsec_87_18, 0x3ff7309c
        .literal .Lsec_87_1c, mac_txrx_init
        .literal .Lsec_87_20, hal_mac_rx_set_policy
        .literal .Lsec_87_24, mac_rxbuf_init
        .literal .Lsec_87_28, mac_last_rxbuf_init
        .literal .Lsec_87_2c, hal_mac_rate_autoack_init
        .literal .Lsec_87_30, hal_mac_disable_low_rate
        .literal .Lsec_87_34, hal_crypto_init
        .literal .Lsec_87_38, hal_attenna_init
        .literal .Lsec_87_3c, bb_wdt_int_enable
.Lsec_87_40:
        .global hal_init
        .type   hal_init, @function
hal_init:
        entry    a1, 32
        l32r     a2, .Lsec_87_0
        movi.n   a8, 2
        memw
        l32i.n   a3, a2, 0
        or       a3, a3, a8
        memw
        s32i.n   a3, a2, 0
.Lsec_87_55:
        memw
        l32i.n   a3, a2, 0
        bbci     a3, 0, .Lsec_87_55
        movi.n   a2, 0
        movi.n   a8, -1
        l32r     a3, .Lsec_87_4
        memw
        s32i.n   a2, a3, 0
        l32r     a3, .Lsec_87_8
        memw
        s32i.n   a8, a3, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, mac_txrx_init
        l32r     a8, .Lsec_87_1c
        callx8   a8
        mov.n    a3, a2
.Lsec_87_7c:
        movi.n   a10, 5
        mov.n    a13, a3
        l32r     a8, .Lsec_87_c
        mov.n    a12, a3
        addx4    a8, a2, a8
        mov.n    a11, a3
        memw
        l32i.n   a9, a8, 0
        or       a9, a9, a10
        memw
        s32i.n   a9, a8, 0
        memw
        l32i.n   a9, a8, 0
        l32r     a10, .Lsec_87_10
        and      a9, a9, a10
        mov.n    a10, a2
        addi.n   a2, a2, 1
        memw
        s32i.n   a9, a8, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_rx_set_policy
        l32r     a8, .Lsec_87_20
        callx8   a8
        bnei     a2, 4, .Lsec_87_7c
        .reloc  ., R_XTENSA_ASM_EXPAND, mac_rxbuf_init
        l32r     a8, .Lsec_87_24
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, mac_last_rxbuf_init
        l32r     a8, .Lsec_87_28
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_rate_autoack_init
        l32r     a8, .Lsec_87_2c
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_disable_low_rate
        l32r     a8, .Lsec_87_30
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_crypto_init
        l32r     a8, .Lsec_87_34
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_attenna_init
        l32r     a8, .Lsec_87_38
        callx8   a8
        l32r     a2, .Lsec_87_4
        movi.n   a10, 1
        l32r     a3, .Lsec_87_14
        memw
        s32i.n   a3, a2, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, bb_wdt_int_enable
        l32r     a8, .Lsec_87_3c
        callx8   a8
        movi     a8, 0xffffff00
        l32r     a3, .Lsec_87_18
        memw
        l32i.n   a2, a3, 0
        and      a2, a2, a8
        movi.n   a8, 2
        or       a2, a2, a8
        memw
        s32i.n   a2, a3, 0
        memw
        retw.n
        .size   hal_init, .-hal_init
.Lsec_87_10e:

        .section .text.hal_mac_clr_bssid,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_88_0, 0x3ff73024
        .literal .Lsec_88_4, 0xfffeffff
.Lsec_88_8:
        .global hal_mac_clr_bssid
        .type   hal_mac_clr_bssid, @function
hal_mac_clr_bssid:
        entry    a1, 32
        l32r     a8, .Lsec_88_0
        extui    a2, a2, 0, 8
        addx8    a2, a2, a8
        memw
        l32i.n   a8, a2, 0
        l32r     a9, .Lsec_88_4
        and      a8, a8, a9
        memw
        s32i.n   a8, a2, 0
        memw
        retw.n
        .size   hal_mac_clr_bssid, .-hal_mac_clr_bssid
.Lsec_88_29:

        .section .text.hal_mac_tsf_get_time,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_89_0, 0x3ff75010
        .literal .Lsec_89_4, 0x3ff75094
        .literal .Lsec_89_8, 0x3ff75090
        .literal .Lsec_89_c, 0x3ff75054
.Lsec_89_10:
        .global hal_mac_tsf_get_time
        .type   hal_mac_tsf_get_time, @function
hal_mac_tsf_get_time:
        entry    a1, 32
        mov.n    a3, a2
        beqi     a2, 1, .Lsec_89_4c
        beqi     a2, 2, .Lsec_89_4c
        beqz.n   a2, .Lsec_89_24
        movi.n   a2, 0
        mov.n    a3, a2
        j        .Lsec_89_7d
.Lsec_89_24:
        l32r     a8, .Lsec_89_0
        movi.n   a2, 2
        movi.n   a10, -3
        memw
        l32i.n   a9, a8, 0
        or       a9, a9, a2
        l32r     a2, .Lsec_89_c
        memw
        s32i.n   a9, a8, 0
        memw
        l32i.n   a2, a2, 0
        memw
        l32i.n   a9, a8, 0
        memw
        j        .Lsec_89_75
        .byte    0x00
.Lsec_89_4c:
        l32r     a8, .Lsec_89_0
        movi.n   a2, 4
        movi.n   a10, -5
        memw
        l32i.n   a3, a8, 0
        or       a3, a3, a2
        l32r     a2, .Lsec_89_4
        memw
        s32i.n   a3, a8, 0
        memw
        l32i.n   a3, a2, 0
        l32r     a2, .Lsec_89_8
        memw
        l32i.n   a2, a2, 0
        memw
        l32i.n   a9, a8, 0
.Lsec_89_75:
        and      a9, a9, a10
        memw
        s32i.n   a9, a8, 0
.Lsec_89_7d:
        memw
        retw.n
        .size   hal_mac_tsf_get_time, .-hal_mac_tsf_get_time
.Lsec_89_82:

        .section .text.wDev_Mesh_Disable_Tsf,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_90_0, hal_disable_sta_tsf
.Lsec_90_4:
        .global wDev_Mesh_Disable_Tsf
        .type   wDev_Mesh_Disable_Tsf, @function
wDev_Mesh_Disable_Tsf:
        entry    a1, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_disable_sta_tsf
        l32r     a8, .Lsec_90_0
        callx8   a8
        retw.n
        .size   wDev_Mesh_Disable_Tsf, .-wDev_Mesh_Disable_Tsf
.Lsec_90_f:

        .section .iram1.15,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_91_0, 0x3ff75010
        .literal .Lsec_91_4, 0x3ff75018
        .literal .Lsec_91_8, 0x3ff75014
        .literal .Lsec_91_c, 0x3ff75058
        .literal .Lsec_91_10, 0x3ff75054
.Lsec_91_14:
        .global hal_get_tsf_time
        .type   hal_get_tsf_time, @function
hal_get_tsf_time:
        entry    a1, 32
        l32r     a8, .Lsec_91_0
        memw
        l32i.n   a3, a8, 0
        bnez.n   a2, .Lsec_91_48
        movi.n   a2, 1
        or       a3, a3, a2
        movi.n   a10, -2
        l32r     a2, .Lsec_91_4
        memw
        s32i.n   a3, a8, 0
        memw
        l32i.n   a3, a2, 0
        l32r     a2, .Lsec_91_8
        memw
        l32i.n   a2, a2, 0
        memw
        l32i.n   a9, a8, 0
        memw
        j        .Lsec_91_69
.Lsec_91_48:
        movi.n   a2, 2
        or       a3, a3, a2
        l32r     a2, .Lsec_91_c
        movi.n   a10, -3
        memw
        s32i.n   a3, a8, 0
        memw
        l32i.n   a3, a2, 0
        l32r     a2, .Lsec_91_10
        memw
        l32i.n   a2, a2, 0
        memw
        l32i.n   a9, a8, 0
.Lsec_91_69:
        and      a9, a9, a10
        memw
        s32i.n   a9, a8, 0
        memw
        retw.n
        .size   hal_get_tsf_time, .-hal_get_tsf_time
.Lsec_91_76:

        .section .text.hal_mac_tsf_set_time,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_92_0, 0x3ff7509c
        .literal .Lsec_92_4, 0x3ff750a0
        .literal .Lsec_92_8, 0x3ff75010
        .literal .Lsec_92_c, 0x3ff75060
        .literal .Lsec_92_10, 0x3ff75064
.Lsec_92_14:
        .global hal_mac_tsf_set_time
        .type   hal_mac_tsf_set_time, @function
hal_mac_tsf_set_time:
        entry    a1, 32
        beqi     a2, 1, .Lsec_92_49
        bnei     a2, 2, .Lsec_92_6c
        l32r     a2, .Lsec_92_0
        memw
        s32i.n   a4, a2, 0
        l32r     a2, .Lsec_92_4
        l32r     a4, .Lsec_92_8
        memw
        s32i.n   a5, a2, 0
        movi     a2, 0x100
        memw
        l32i.n   a8, a4, 0
        or       a8, a8, a2
        memw
        s32i.n   a8, a4, 0
        memw
        j        .Lsec_92_6c
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_92_49:
        l32r     a2, .Lsec_92_c
        memw
        s32i.n   a4, a2, 0
        l32r     a2, .Lsec_92_10
        l32r     a9, .Lsec_92_8
        memw
        s32i.n   a5, a2, 0
        movi     a2, 128
        memw
        l32i.n   a8, a9, 0
        or       a8, a8, a2
        memw
        s32i.n   a8, a9, 0
.Lsec_92_6c:
        memw
        retw.n
        .size   hal_mac_tsf_set_time, .-hal_mac_tsf_set_time
.Lsec_92_71:

        .section .text.hal_mac_tsf_reset,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_93_0, 0x3ff75098
        .literal .Lsec_93_4, 0x7fffffff
        .literal .Lsec_93_8, 0x3ff7509c
        .literal .Lsec_93_c, 0x3ff750a0
        .literal .Lsec_93_10, 0x3ff75010
        .literal .Lsec_93_14, 0x3ff7505c
        .literal .Lsec_93_18, 0x3dffffff
        .literal .Lsec_93_1c, 0x3ff75060
        .literal .Lsec_93_20, 0x3ff75064
        .literal .Lsec_93_24, 0x3ff75020
        .literal .Lsec_93_28, 0x3ff75024
        .literal .Lsec_93_2c, 0x3ff75028
        .literal .Lsec_93_30, wDev_reset_bcnSendTick
.Lsec_93_34:
        .global hal_mac_tsf_reset
        .type   hal_mac_tsf_reset, @function
hal_mac_tsf_reset:
        entry    a1, 32
        bnei     a2, 1, .Lsec_93_3d
        j        .Lsec_93_e8
.Lsec_93_3d:
        beqi     a2, 2, .Lsec_93_46
        bnez     a2, .Lsec_93_170
        j        .Lsec_93_90
.Lsec_93_46:
        l32r     a2, .Lsec_93_0
        movi     a9, 0x100
        memw
        l32i.n   a3, a2, 0
        l32r     a8, .Lsec_93_4
        and      a3, a3, a8
        l32r     a8, .Lsec_93_8
        memw
        s32i.n   a3, a2, 0
        movi.n   a3, 0
        memw
        s32i.n   a3, a8, 0
        l32r     a8, .Lsec_93_c
        memw
        s32i.n   a3, a8, 0
        l32r     a8, .Lsec_93_10
        memw
        l32i.n   a3, a8, 0
        or       a3, a3, a9
        memw
        s32i.n   a3, a8, 0
        movi.n   a8, -1
        slli     a8, a8, 31
        memw
        l32i.n   a3, a2, 0
        memw
        j        .Lsec_93_167
        .byte    0x00
        .byte    0x00
.Lsec_93_90:
        l32r     a3, .Lsec_93_14
        l32r     a9, .Lsec_93_18
        memw
        l32i.n   a8, a3, 0
        and      a8, a8, a9
        memw
        s32i.n   a8, a3, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wDev_reset_bcnSendTick
        l32r     a8, .Lsec_93_30
        callx8   a8
        movi     a9, 128
        l32r     a8, .Lsec_93_1c
        memw
        s32i.n   a2, a8, 0
        l32r     a8, .Lsec_93_20
        memw
        s32i.n   a2, a8, 0
        l32r     a8, .Lsec_93_10
        memw
        l32i.n   a2, a8, 0
        or       a2, a2, a9
        memw
        s32i.n   a2, a8, 0
        movi.n   a8, -31
        slli     a8, a8, 25
        memw
        l32i.n   a2, a3, 0
        or       a2, a2, a8
        memw
        s32i.n   a2, a3, 0
        memw
        j        .Lsec_93_170
        .byte    0x00
.Lsec_93_e8:
        l32r     a2, .Lsec_93_24
        movi.n   a10, 0
        movi.n   a8, 64
        memw
        l32i.n   a3, a2, 0
        l32r     a11, .Lsec_93_4
        and      a3, a3, a11
        memw
        s32i.n   a3, a2, 0
        l32r     a3, .Lsec_93_28
        memw
        s32i.n   a10, a3, 0
        l32r     a3, .Lsec_93_2c
        l32r     a9, .Lsec_93_10
        memw
        s32i.n   a10, a3, 0
        memw
        l32i.n   a3, a9, 0
        or       a3, a3, a8
        movi.n   a8, -1
        slli     a8, a8, 31
        memw
        s32i.n   a3, a9, 0
        memw
        l32i.n   a3, a2, 0
        or       a3, a3, a8
        memw
        s32i.n   a3, a2, 0
        l32r     a2, .Lsec_93_14
        memw
        l32i.n   a3, a2, 0
        and      a3, a3, a11
        memw
        s32i.n   a3, a2, 0
        l32r     a3, .Lsec_93_1c
        memw
        s32i.n   a10, a3, 0
        l32r     a3, .Lsec_93_20
        memw
        s32i.n   a10, a3, 0
        movi     a10, 128
        memw
        l32i.n   a3, a9, 0
        or       a3, a3, a10
        memw
        s32i.n   a3, a9, 0
        memw
        l32i     a3, a2, 0
.Lsec_93_167:
        or       a3, a3, a8
        memw
        s32i     a3, a2, 0
.Lsec_93_170:
        memw
        retw.n
        .size   hal_mac_tsf_reset, .-hal_mac_tsf_reset
.Lsec_93_175:

        .section .text.hal_mac_set_csi,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_94_0, g_wifi_menuconfig
        .literal .Lsec_94_4, 0x3ff7309c
        .literal .Lsec_94_8, 0xff7fffff
.Lsec_94_c:
        .global hal_mac_set_csi
        .type   hal_mac_set_csi, @function
hal_mac_set_csi:
        entry    a1, 32
        extui    a10, a2, 0, 8
        l32r     a2, .Lsec_94_0
        l32i.n   a8, a2, 24
        movi.n   a2, -1
        beqz.n   a8, .Lsec_94_50
        l32r     a8, .Lsec_94_4
        beqz.n   a10, .Lsec_94_3c
        movi.n   a9, 1
        slli     a9, a9, 23
        memw
        l32i.n   a2, a8, 0
        or       a2, a2, a9
        memw
        s32i.n   a2, a8, 0
        movi.n   a2, 0
        memw
        j        .Lsec_94_50
        .byte    0x00
        .byte    0x00
.Lsec_94_3c:
        memw
        l32i.n   a9, a8, 0
        l32r     a2, .Lsec_94_8
        and      a9, a9, a2
        or       a2, a10, a10
        memw
        s32i     a9, a8, 0
.Lsec_94_50:
        memw
        retw.n
        .size   hal_mac_set_csi, .-hal_mac_set_csi
.Lsec_94_55:

        .section .wifislprxiram.16,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_95_0, 0x3ff73cb8
.Lsec_95_4:
        .global hal_mac_init
        .type   hal_mac_init, @function
hal_mac_init:
        entry    a1, 32
        l32r     a10, .Lsec_95_0
        movi.n   a9, -3
        slli     a9, a9, 11
        movi.n   a2, 0
        memw
        l32i.n   a8, a10, 0
        and      a8, a8, a9
        memw
        s32i.n   a8, a10, 0
        memw
        retw.n
        .size   hal_mac_init, .-hal_mac_init
.Lsec_95_23:

        .section .wifi0iram.17,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_96_0, 0x3ff73cb8
        .literal .Lsec_96_4, 0x000017ff
        .literal .Lsec_96_8, .Lsec_103_0
        .literal .Lsec_96_c, 0x3ff73c58
        .literal .Lsec_96_10, 0xfffcffff
        .literal .Lsec_96_14, .Lsec_102_0
        .literal .Lsec_96_18, .Lsec_101_0
        .literal .Lsec_96_1c, ets_delay_us
        .literal .Lsec_96_20, wifi_log
        .literal .Lsec_96_24, wifi_log
        .literal .Lsec_96_28, wifi_log
        .literal .Lsec_96_2c, ets_delay_us
.Lsec_96_30:
        .global hal_mac_deinit
        .type   hal_mac_deinit, @function
hal_mac_deinit:
        entry    a1, 32
        l32r     a3, .Lsec_96_0
        movi.n   a10, 32
        memw
        l32i.n   a2, a3, 0
        l32r     a4, .Lsec_96_4
        or       a2, a2, a4
        movi.n   a4, 0
        memw
        s32i     a2, a3, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_96_1c
        callx8   a8
.Lsec_96_54:
        movi.n   a5, 1
        slli     a5, a5, 13
        memw
        l32i.n   a2, a3, 0
        and      a2, a2, a5
        beqz     a2, .Lsec_96_ec
        movi     a2, 0x271
        slli     a2, a2, 5
        bne      a4, a2, .Lsec_96_de
        movi.n   a12, 1
        l32r     a13, .Lsec_96_8
        mov.n    a11, a12
        movi.n   a10, 5
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_96_20
        callx8   a8
        movi.n   a5, 1
        l32r     a2, .Lsec_96_c
        slli     a5, a5, 17
        mov.n    a14, a2
        movi.n   a12, 1
        movi.n   a10, 6
        memw
        l32i.n   a8, a2, 0
        l32r     a4, .Lsec_96_10
        and      a8, a8, a4
        or       a8, a8, a5
        movi.n   a5, 1
        memw
        s32i.n   a8, a2, 0
        slli     a5, a5, 12
        mov.n    a11, a5
        memw
        l32i.n   a15, a2, 0
        l32r     a13, .Lsec_96_14
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_96_24
        callx8   a8
        mov.n    a14, a2
        movi.n   a12, 1
        memw
        l32i.n   a8, a2, 0
        mov.n    a11, a5
        and      a4, a8, a4
        movi.n   a10, 6
        memw
        s32i.n   a4, a2, 0
        movi.n   a4, 0
        memw
        l32i     a15, a2, 0
        l32r     a13, .Lsec_96_18
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_96_28
        callx8   a8
.Lsec_96_de:
        movi.n   a10, 2
        addi.n   a4, a4, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_96_2c
        callx8   a8
        j        .Lsec_96_54
        .byte    0x00
.Lsec_96_ec:
        retw.n
        .size   hal_mac_deinit, .-hal_mac_deinit
.Lsec_96_ee:

        .section .text.hal_mac_rx_enable,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_97_0, 0x3ff73084
.Lsec_97_4:
        .global hal_mac_rx_enable
        .type   hal_mac_rx_enable, @function
hal_mac_rx_enable:
        entry    a1, 32
        l32r     a10, .Lsec_97_0
        movi.n   a9, -1
        slli     a9, a9, 31
        memw
        l32i.n   a8, a10, 0
        or       a8, a8, a9
        memw
        s32i.n   a8, a10, 0
        memw
        retw.n
        .size   hal_mac_rx_enable, .-hal_mac_rx_enable
.Lsec_97_21:

        .section .text.hal_mac_rx_disable,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_98_0, 0x3ff73084
        .literal .Lsec_98_4, 0x7fffffff
.Lsec_98_8:
        .global hal_mac_rx_disable
        .type   hal_mac_rx_disable, @function
hal_mac_rx_disable:
        entry    a1, 32
        l32r     a9, .Lsec_98_0
        l32r     a10, .Lsec_98_4
        memw
        l32i.n   a8, a9, 0
        and      a8, a8, a10
        memw
        s32i.n   a8, a9, 0
        memw
        retw.n
        .size   hal_mac_rx_disable, .-hal_mac_rx_disable
.Lsec_98_23:

        .section .text.hal_rx_disable_bssid_check,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_99_0, 0x0ffdcc36
.Lsec_99_4:
        .global hal_rx_disable_bssid_check
        .type   hal_rx_disable_bssid_check, @function
hal_rx_disable_bssid_check:
        entry    a1, 32
        l32r     a8, .Lsec_99_0
        extui    a2, a2, 0, 8
        add.n    a2, a2, a8
        slli     a2, a2, 2
        movi.n   a9, -3
        memw
        l32i.n   a8, a2, 0
        and      a8, a8, a9
        memw
        s32i.n   a8, a2, 0
        memw
        retw.n
        .size   hal_rx_disable_bssid_check, .-hal_rx_disable_bssid_check
.Lsec_99_26:

        .section .text.hal_rx_enable_bssid_check,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_100_0, 0x0ffdcc36
.Lsec_100_4:
        .global hal_rx_enable_bssid_check
        .type   hal_rx_enable_bssid_check, @function
hal_rx_enable_bssid_check:
        entry    a1, 32
        l32r     a8, .Lsec_100_0
        extui    a2, a2, 0, 8
        add.n    a2, a2, a8
        slli     a2, a2, 2
        movi.n   a9, 2
        memw
        l32i.n   a8, a2, 0
        or       a8, a8, a9
        memw
        s32i.n   a8, a2, 0
        memw
        retw.n
        .size   hal_rx_enable_bssid_check, .-hal_rx_enable_bssid_check
.Lsec_100_26:

        .section .rodata_wlog_error.20,"a",@progbits
        .align  1
.Lsec_101_0:
        .byte   0x46, 0x6f, 0x72, 0x63, 0x65, 0x20, 0x54, 0x58, 0x52, 0x41, 0x4d, 0x45, 0x20, 0x41, 0x43, 0x4b
        .byte   0x28, 0x30, 0x78, 0x25, 0x30, 0x38, 0x78, 0x29, 0x3d, 0x30, 0x78, 0x25, 0x30, 0x38, 0x78, 0x00

        .section .rodata_wlog_error.19,"a",@progbits
        .align  1
.Lsec_102_0:
        .byte   0x46, 0x6f, 0x72, 0x63, 0x65, 0x20, 0x54, 0x58, 0x52, 0x41, 0x4d, 0x45, 0x20, 0x41, 0x43, 0x4b
        .byte   0x28, 0x30, 0x78, 0x25, 0x30, 0x38, 0x78, 0x29, 0x3d, 0x30, 0x78, 0x25, 0x30, 0x38, 0x78, 0x00

        .section .rodata_wlog_error.18,"a",@progbits
        .align  1
.Lsec_103_0:
        .byte   0x6d, 0x61, 0x63, 0x20, 0x64, 0x65, 0x69, 0x6e, 0x69, 0x74, 0x20, 0x66, 0x61, 0x69, 0x6c, 0x2c
        .byte   0x20, 0x74, 0x78, 0x69, 0x6e, 0x67, 0x20, 0x63, 0x61, 0x6e, 0x27, 0x74, 0x20, 0x73, 0x74, 0x6f
        .byte   0x70, 0x0a, 0x00

        .section .bss.g_mac_deinit_txing,"aw",@nobits
        .align  1
.Lsec_104_0:
        .global g_mac_deinit_txing
        .type   g_mac_deinit_txing, @object
        .size   g_mac_deinit_txing, 1
g_mac_deinit_txing:
        .skip   1

        .section .bss.g_mac_deinit_rxing,"aw",@nobits
        .align  1
.Lsec_105_0:
        .global g_mac_deinit_rxing
        .type   g_mac_deinit_rxing, @object
        .size   g_mac_deinit_rxing, 1
g_mac_deinit_rxing:
        .skip   1

        .section .bss.g_mac_deinit_count,"aw",@nobits
        .align  4
.Lsec_106_0:
        .global g_mac_deinit_count
        .type   g_mac_deinit_count, @object
        .size   g_mac_deinit_count, 4
g_mac_deinit_count:
        .skip   4
