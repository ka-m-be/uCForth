.section .text.mac_tx_set_plcp1,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_12_0, 0x3ff74258
.Lsec_12_4:
        .global mac_tx_set_plcp1
        .type   mac_tx_set_plcp1, @function
mac_tx_set_plcp1:
        entry    a1, 32
        l32i.n   a12, a2, 0
        movi.n   a10, 15
        movi.n   a8, 1
        l32i.n   a9, a12, 44
        l8ui     a13, a9, 12
        addi     a11, a13, -16
        extui    a11, a11, 0, 8
        l32i.n   a14, a9, 0
        bgeu     a10, a11, .Lsec_12_20
        movi     a8, 0
.Lsec_12_20:
        slli     a8, a8, 9
        movi.n   a15, 31
        l8ui     a10, a9, 16
        add.n    a10, a10, a10
        or       a8, a8, a10
        slli     a8, a8, 16
        and      a10, a13, a15
        bgeu     a15, a13, .Lsec_12_39
        and      a10, a11, a15
.Lsec_12_39:
        slli     a10, a10, 12
        or       a8, a8, a10
        bbci     a14, 14, .Lsec_12_50
        l32i.n   a9, a9, 8
        bbci     a9, 15, .Lsec_12_50
        movi     a9, 1
        slli     a9, a9, 28
        or       a8, a8, a9
.Lsec_12_50:
        l16ui    a10, a12, 20
        l16ui    a9, a12, 22
        add.n    a9, a9, a10
        movi.n   a10, -1
        slli     a10, a10, 12
        extui    a9, a9, 0, 12
        and      a10, a8, a10
        or       a8, a10, a9
        l8ui     a9, a2, 4
        slli     a2, a9, 4
        sub      a9, a9, a2
        l32r     a2, .Lsec_12_0
        addx4    a9, a9, a2
        movi.n   a2, 0
        memw
        s32i.n   a8, a9, 0
        memw
        retw.n
        .size   mac_tx_set_plcp1, .-mac_tx_set_plcp1
.Lsec_12_81:

        .section .text.mac_tx_set_plcp2,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_13_0, .Lsec_22_0
        .literal .Lsec_13_4, .Lsec_21_0
        .literal .Lsec_13_8, 0x3ff7425c
        .literal .Lsec_13_c, wifi_log
        .literal .Lsec_13_10, wifi_log
.Lsec_13_14:
        .global mac_tx_set_plcp2
        .type   mac_tx_set_plcp2, @function
mac_tx_set_plcp2:
        entry    a1, 48
        l32i.n   a15, a2, 0
        movi.n   a9, 7
        movi.n   a8, 15
        slli     a9, a9, 13
        slli     a8, a8, 12
        l32i.n   a11, a15, 44
        l32i.n   a14, a11, 16
        and      a10, a14, a9
        and      a8, a14, a8
        beqz.n   a10, .Lsec_13_5c
        l32r     a13, .Lsec_13_0
        extui    a14, a14, 12, 4
        movi.n   a12, 2
        movi.n   a10, 6
        s32i.n   a11, a1, 0
        movi.n   a11, 64
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_13_c
        callx8   a8
        movi.n   a11, 1
        movi.n   a12, 2
        l32r     a13, .Lsec_13_4
        slli     a11, a11, 13
        movi.n   a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_13_10
        callx8   a8
        j        .Lsec_13_85
        .byte    0x00
.Lsec_13_5c:
        addmi    a9, a8, 0xfffff000
        movi.n   a8, 1
        movnez   a8, a10, a9
        slli     a8, a8, 4
        movi.n   a9, 32
        or       a8, a8, a9
        movi.n   a9, 48
        and      a8, a8, a9
        l8ui     a9, a2, 4
        slli     a2, a9, 4
        sub      a9, a9, a2
        l32r     a2, .Lsec_13_8
        addx4    a9, a9, a2
        memw
        s32i.n   a8, a9, 0
.Lsec_13_85:
        movi.n   a2, 0
        memw
        retw.n
        .size   mac_tx_set_plcp2, .-mac_tx_set_plcp2
.Lsec_13_8c:

        .section .text.mac_tx_set_htsig,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_14_0, 0xff0000ff
        .literal .Lsec_14_4, 0x00ffffff
        .literal .Lsec_14_8, 0x3ff74260
        .literal .Lsec_14_c, 0xfff3ffff
        .literal .Lsec_14_10, 0x3ff74264
.Lsec_14_14:
        .global mac_tx_set_htsig
        .type   mac_tx_set_htsig, @function
mac_tx_set_htsig:
        entry    a1, 48
        l32i.n   a8, a2, 0
        movi.n   a4, 15
        l32i.n   a11, a8, 44
        l8ui     a13, a11, 12
        addi     a9, a13, -16
        extui    a9, a9, 0, 8
        bgeu     a4, a9, .Lsec_14_2c
        j        .Lsec_14_132
.Lsec_14_2c:
        l32i.n   a9, a8, 36
        l32i     a10, a11, 16
        extui    a10, a10, 20, 4
        l32i     a4, a11, 0
        extui    a14, a4, 22, 1
        l16ui    a12, a8, 22
        l16ui    a15, a8, 20
        movi.n   a8, 0
        beq      a9, a8, .Lsec_14_54
        l8ui     a9, a9, 130
        addi     a9, a9, -4
        extui    a9, a9, 0, 8
        bgeui    a9, 2, .Lsec_14_54
        movi     a8, 128
.Lsec_14_54:
        bbci     a4, 14, .Lsec_14_62
        l32i.n   a4, a11, 8
        bbci     a4, 15, .Lsec_14_62
        movi     a4, 128
        or       a8, a8, a4
.Lsec_14_62:
        movi     a11, -128
        extui    a9, a13, 0, 3
        and      a11, a8, a11
        add.n    a12, a12, a15
        or       a8, a11, a9
        extui    a9, a12, 0, 16
        or       a11, a9, a9
        beqz.n   a14, .Lsec_14_84
        addx2    a11, a10, a10
        slli     a11, a11, 4
        add      a11, a3, a11
        l16ui    a11, a11, 68
.Lsec_14_84:
        slli     a11, a11, 8
        movi.n   a4, 7
        l32r     a15, .Lsec_14_0
        and      a8, a8, a15
        or       a8, a8, a11
        slli     a11, a14, 3
        or       a11, a11, a4
        movi.n   a4, 23
        movi.n   a15, 1
        bltu     a4, a13, .Lsec_14_a1
        movi.n   a15, 0
.Lsec_14_a1:
        extui    a13, a8, 24, 8
        slli     a15, a15, 7
        movi     a4, 112
        and      a13, a13, a4
        or       a11, a11, a15
        or       a11, a11, a13
        l32r     a13, .Lsec_14_4
        slli     a11, a11, 24
        and      a8, a8, a13
        or       a8, a8, a11
        l8ui     a13, a2, 4
        slli     a2, a13, 4
        sub      a13, a13, a2
        slli     a11, a13, 2
        l32r     a2, .Lsec_14_8
        addx4    a13, a13, a2
        movi.n   a2, 0
        memw
        s32i.n   a8, a13, 0
        s16i     a2, a1, 0
        memw
        beqz.n   a14, .Lsec_14_120
        addx2    a10, a10, a10
        slli     a10, a10, 4
        add.n    a3, a3, a10
        l8ui     a8, a3, 65
        movi.n   a2, 12
        slli     a8, a8, 2
        and      a8, a8, a2
        l8ui     a2, a3, 64
        extui    a2, a2, 0, 2
        or       a8, a8, a2
        extui    a8, a8, 0, 4
        l32i     a2, a3, 68
        slli     a8, a8, 16
        or       a8, a9, a8
        bne      a2, a12, .Lsec_14_128
        l32r     a2, .Lsec_14_c
        and      a8, a8, a2
        movi.n   a2, 1
        slli     a2, a2, 18
        or       a8, a8, a2
        memw
        j        .Lsec_14_128
        .byte    0x00
.Lsec_14_120:
        movi.n   a2, 5
        slli     a2, a2, 16
        or       a8, a9, a2
.Lsec_14_128:
        l32r     a2, .Lsec_14_10
        add.n    a11, a11, a2
        memw
        s32i.n   a8, a11, 0
.Lsec_14_132:
        movi.n   a2, 0
        memw
        retw.n
        .size   mac_tx_set_htsig, .-mac_tx_set_htsig
.Lsec_14_139:

        .section .wifi0iram.4,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_15_0, 0x3ff74270
.Lsec_15_4:
        .global hal_mac_tx_is_cbw40
        .type   hal_mac_tx_is_cbw40, @function
hal_mac_tx_is_cbw40:
        entry    a1, 32
        extui    a2, a2, 0, 8
        slli     a8, a2, 4
        sub      a2, a2, a8
        l32r     a8, .Lsec_15_0
        addx4    a2, a2, a8
        memw
        l32i.n   a2, a2, 0
        extui    a2, a2, 25, 1
        retw.n
        .size   hal_mac_tx_is_cbw40, .-hal_mac_tx_is_cbw40
.Lsec_15_20:

        .section .text.hal_mac_get_txq_state,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_16_0, 0x3ff73cc0
        .literal .Lsec_16_4, 0x3ff73cc8
.Lsec_16_8:
        .global hal_mac_get_txq_state
        .type   hal_mac_get_txq_state, @function
hal_mac_get_txq_state:
        entry    a1, 32
        beqi     a2, 1, .Lsec_16_24
        beqi     a2, 2, .Lsec_16_35
        movi.n   a8, 0
        bne      a2, a8, .Lsec_16_40
        l32r     a2, .Lsec_16_0
        memw
        l32i.n   a8, a2, 0
        extui    a8, a8, 0, 11
        j        .Lsec_16_40
.Lsec_16_24:
        l32r     a2, .Lsec_16_0
        memw
        l32i.n   a8, a2, 0
        extui    a8, a8, 16, 11
        j        .Lsec_16_40
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_16_35:
        l32r     a2, .Lsec_16_4
        memw
        l32i.n   a8, a2, 0
        extui    a8, a8, 0, 4
.Lsec_16_40:
        mov.n    a2, a8
        retw.n
        .size   hal_mac_get_txq_state, .-hal_mac_get_txq_state
.Lsec_16_44:

        .section .text.hal_mac_clr_txq_state,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_17_0, 0x3ff73cbc
        .literal .Lsec_17_4, 0xf800ffff
        .literal .Lsec_17_8, 0x3ff73cc4
.Lsec_17_c:
        .global hal_mac_clr_txq_state
        .type   hal_mac_clr_txq_state, @function
hal_mac_clr_txq_state:
        entry    a1, 32
        beqi     a2, 1, .Lsec_17_2c
        beqi     a2, 2, .Lsec_17_54
        bnez     a2, .Lsec_17_64
        l32r     a2, .Lsec_17_0
        movi     a9, 0xfffff800
        extui    a3, a3, 0, 11
        memw
        l32i.n   a8, a2, 0
        and      a8, a8, a9
        j        .Lsec_17_5c
.Lsec_17_2c:
        l32r     a2, .Lsec_17_0
        movi     a9, 0x7ff
        slli     a9, a9, 16
        slli     a3, a3, 16
        and      a3, a3, a9
        memw
        l32i.n   a8, a2, 0
        l32r     a9, .Lsec_17_4
        and      a8, a8, a9
        or       a3, a3, a8
        memw
        s32i.n   a3, a2, 0
        memw
        j        .Lsec_17_64
.Lsec_17_54:
        l32r     a2, .Lsec_17_8
        memw
        l32i.n   a8, a2, 0
.Lsec_17_5c:
        or       a8, a8, a3
        memw
        s32i.n   a8, a2, 0
.Lsec_17_64:
        movi.n   a2, 0
        memw
        retw.n
        .size   hal_mac_clr_txq_state, .-hal_mac_clr_txq_state
.Lsec_17_6b:

        .section .text.hal_mac_get_txq_pmd,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_18_0, 0x3ff74270
.Lsec_18_4:
        .global hal_mac_get_txq_pmd
        .type   hal_mac_get_txq_pmd, @function
hal_mac_get_txq_pmd:
        entry    a1, 48
        extui    a2, a2, 0, 8
        slli     a8, a2, 4
        sub      a2, a2, a8
        l32r     a8, .Lsec_18_0
        addx4    a2, a2, a8
        memw
        l32i.n   a8, a2, 0
        extui    a2, a8, 24, 8
        extui    a10, a8, 8, 8
        extui    a9, a8, 16, 8
        s8i      a2, a3, 3
        movi     a2, 127
        s8i      a8, a3, 0
        s8i      a10, a3, 1
        s8i      a9, a3, 2
        memw
        bbci     a8, 24, .Lsec_18_3c
        or       a2, a9, a9
.Lsec_18_3c:
        s8i      a2, a3, 2
        memw
        movi.n   a2, 0
        memw
        retw.n
        .size   hal_mac_get_txq_pmd, .-hal_mac_get_txq_pmd
.Lsec_18_49:

        .section .text.hal_attenna_init,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_19_0, 0x3ff7425c
        .literal .Lsec_19_4, 0x3ff73284
.Lsec_19_8:
        .global hal_attenna_init
        .type   hal_attenna_init, @function
hal_attenna_init:
        entry    a1, 32
        l32r     a8, .Lsec_19_0
        movi.n   a11, -8
        movi.n   a9, 8
        loop     a9, .Lsec_19_25
        addi     a8, a8, -60
        memw
        l32i.n   a10, a8, 60
        and      a10, a10, a11
        memw
        s32i.n   a10, a8, 60
.Lsec_19_25:
        movi.n   a13, -9
        movi.n   a11, 32
        movi.n   a12, -17
        l32r     a8, .Lsec_19_0
        movi     a9, 8
        loop     a9, .Lsec_19_5e
        addi     a8, a8, -60
        memw
        l32i.n   a10, a8, 60
        and      a10, a10, a13
        memw
        s32i.n   a10, a8, 60
        memw
        l32i.n   a10, a8, 60
        or       a10, a10, a11
        memw
        s32i.n   a10, a8, 60
        memw
        l32i.n   a10, a8, 60
        and      a10, a10, a12
        memw
        s32i.n   a10, a8, 60
.Lsec_19_5e:
        movi.n   a10, -8
        l32r     a8, .Lsec_19_4
        memw
        l32i.n   a9, a8, 0
        and      a9, a9, a10
        memw
        s32i.n   a9, a8, 0
        memw
        l32i.n   a9, a8, 0
        or       a9, a9, a11
        memw
        s32i.n   a9, a8, 0
        memw
        retw.n
        .size   hal_attenna_init, .-hal_attenna_init
.Lsec_19_82:

        .section .text.hal_mac_rate_autoack_init,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_20_0, 0x3ff73404
        .literal .Lsec_20_4, 0x3ff73400
        .literal .Lsec_20_8, 0x3ff7340c
        .literal .Lsec_20_c, 0x03020100
        .literal .Lsec_20_10, 0x3ff73408
        .literal .Lsec_20_14, 0x07060500
        .literal .Lsec_20_18, 0x3ff73414
        .literal .Lsec_20_1c, 0x0b0b0b0b
        .literal .Lsec_20_20, 0x3ff73410
        .literal .Lsec_20_24, 0x3ff7341c
        .literal .Lsec_20_28, 0x090b0b0b
        .literal .Lsec_20_2c, 0x3ff73418
        .literal .Lsec_20_30, 0x09090909
.Lsec_20_34:
        .global hal_mac_rate_autoack_init
        .type   hal_mac_rate_autoack_init, @function
hal_mac_rate_autoack_init:
        entry    a1, 32
        l32r     a9, .Lsec_20_0
        movi.n   a8, 0
        memw
        s32i.n   a8, a9, 0
        l32r     a9, .Lsec_20_4
        memw
        s32i.n   a8, a9, 0
        l32r     a8, .Lsec_20_8
        l32r     a9, .Lsec_20_c
        memw
        s32i.n   a9, a8, 0
        l32r     a8, .Lsec_20_10
        l32r     a9, .Lsec_20_14
        memw
        s32i.n   a9, a8, 0
        l32r     a8, .Lsec_20_1c
        l32r     a9, .Lsec_20_18
        memw
        s32i.n   a8, a9, 0
        l32r     a9, .Lsec_20_20
        memw
        s32i.n   a8, a9, 0
        l32r     a8, .Lsec_20_24
        l32r     a9, .Lsec_20_28
        memw
        s32i.n   a9, a8, 0
        l32r     a8, .Lsec_20_2c
        l32r     a9, .Lsec_20_30
        memw
        s32i.n   a9, a8, 0
        memw
        retw.n
        .size   hal_mac_rate_autoack_init, .-hal_mac_rate_autoack_init
.Lsec_20_8d:

        .section .rodata_wlog_warning.3,"a",@progbits
        .align  1
.Lsec_21_0:
        .byte   0x69, 0x70, 0x65, 0x6e, 0x63, 0x69, 0x6c, 0x0a, 0x00

        .section .rodata_wlog_warning.2,"a",@progbits
        .align  1
.Lsec_22_0:
        .byte   0x3c, 0x61, 0x6e, 0x74, 0x65, 0x6e, 0x6e, 0x61, 0x3e, 0x69, 0x6e, 0x76, 0x61, 0x6c, 0x69, 0x64
        .byte   0x20, 0x61, 0x6e, 0x74, 0x65, 0x6e, 0x6e, 0x61, 0x3a, 0x25, 0x64, 0x2c, 0x20, 0x65, 0x62, 0x3a
        .byte   0x25, 0x70, 0x2c, 0x20, 0x74, 0x78, 0x5f, 0x64, 0x65, 0x73, 0x63, 0x3a, 0x25, 0x70, 0x00
