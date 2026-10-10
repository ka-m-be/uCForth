.section .text.hal_crypto_clr_key_entry,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_12_0, 0x3ff73814
        .literal .Lsec_12_4, 0x3ff74400
        .literal .Lsec_12_8, 0x3ff74404
        .literal .Lsec_12_c, 0x3ff74408
        .literal .Lsec_12_10, 0x3ff7440c
        .literal .Lsec_12_14, 0x3ff74410
        .literal .Lsec_12_18, 0x3ff74414
        .literal .Lsec_12_1c, 0x3ff74418
        .literal .Lsec_12_20, 0x3ff7441c
        .literal .Lsec_12_24, 0x3ff74420
        .literal .Lsec_12_28, 0x3ff74424
.Lsec_12_2c:
        .global hal_crypto_clr_key_entry
        .type   hal_crypto_clr_key_entry, @function
hal_crypto_clr_key_entry:
        entry    a1, 32
        l32r     a9, .Lsec_12_0
        movi.n   a8, -2
        ssl      a2
        src      a8, a8, a8
        addx4    a2, a2, a2
        memw
        l32i.n   a10, a9, 0
        and      a8, a8, a10
        memw
        s32i.n   a8, a9, 0
        movi.n   a8, 0
        l32r     a9, .Lsec_12_4
        addx8    a9, a2, a9
        memw
        s32i.n   a8, a9, 0
        l32r     a9, .Lsec_12_8
        addx8    a9, a2, a9
        memw
        s32i.n   a8, a9, 0
        l32r     a9, .Lsec_12_c
        addx8    a9, a2, a9
        memw
        s32i.n   a8, a9, 0
        l32r     a9, .Lsec_12_10
        addx8    a9, a2, a9
        memw
        s32i.n   a8, a9, 0
        l32r     a9, .Lsec_12_14
        addx8    a9, a2, a9
        memw
        s32i.n   a8, a9, 0
        l32r     a9, .Lsec_12_18
        addx8    a9, a2, a9
        memw
        s32i.n   a8, a9, 0
        l32r     a9, .Lsec_12_1c
        addx8    a9, a2, a9
        memw
        s32i.n   a8, a9, 0
        l32r     a9, .Lsec_12_20
        addx8    a9, a2, a9
        memw
        s32i.n   a8, a9, 0
        l32r     a9, .Lsec_12_24
        addx8    a9, a2, a9
        memw
        s32i.n   a8, a9, 0
        l32r     a9, .Lsec_12_28
        addx8    a2, a2, a9
        memw
        s32i.n   a8, a2, 0
        memw
        retw.n
        .size   hal_crypto_clr_key_entry, .-hal_crypto_clr_key_entry
.Lsec_12_bf:

        .section .text.hal_crypto_get_using_key_idx,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_13_0, 0x3ff7442c
.Lsec_13_4:
        .global hal_crypto_get_using_key_idx
        .type   hal_crypto_get_using_key_idx, @function
hal_crypto_get_using_key_idx:
        entry    a1, 32
        l32r     a2, .Lsec_13_0
        memw
        l32i.n   a2, a2, 0
        extui    a2, a2, 30, 2
        retw.n
        .size   hal_crypto_get_using_key_idx, .-hal_crypto_get_using_key_idx
.Lsec_13_14:

        .section .text.hal_crypto_set_key_entry,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_14_0, g_osi_funcs_p
        .literal .Lsec_14_4, .Lsec_23_0
        .literal .Lsec_14_8, .Lsec_22_0
        .literal .Lsec_14_c, 0x00003c1f
        .literal .Lsec_14_10, 0x3ff74400
        .literal .Lsec_14_14, 0x3ff74404
        .literal .Lsec_14_18, 0x3ff73814
        .literal .Lsec_14_1c, 0x3ff74408
        .literal .Lsec_14_20, memset
        .literal .Lsec_14_24, memcpy
        .literal .Lsec_14_28, wifi_log
        .literal .Lsec_14_2c, memcpy
        .literal .Lsec_14_30, free
.Lsec_14_34:
        .global hal_crypto_set_key_entry
        .type   hal_crypto_set_key_entry, @function
hal_crypto_set_key_entry:
        entry    a1, 48
        movi.n   a7, -4
        addi.n   a6, a4, 3
        and      a6, a6, a7
        extui    a7, a3, 0, 2
        beqz.n   a7, .Lsec_14_76
        l32r     a7, .Lsec_14_0
        mov.n    a10, a6
        l32i.n   a7, a7, 0
        l32i     a7, a7, 0x16c
        callx8   a7
        mov.n    a7, a10
        beqz.n   a10, .Lsec_14_70
        mov.n    a12, a6
        movi.n   a11, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, memset
        l32r     a8, .Lsec_14_20
        callx8   a8
        mov.n    a12, a4
        or       a11, a3, a3
        or       a10, a7, a7
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_14_24
        callx8   a8
        j        .Lsec_14_78
        .byte    0x00
.Lsec_14_70:
        l32r     a13, .Lsec_14_4
        j        .Lsec_14_80
.Lsec_14_76:
        mov.n    a7, a3
.Lsec_14_78:
        extui    a4, a7, 0, 2
        beqz.n   a4, .Lsec_14_90
        l32r     a13, .Lsec_14_8
.Lsec_14_80:
        movi.n   a11, 1
        movi.n   a12, 2
        slli     a11, a11, 13
        movi     a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_14_28
        callx8   a8
.Lsec_14_90:
        movi.n   a4, 0
        s16i     a4, a1, 0
        memw
        l8ui     a11, a5, 6
        slli     a11, a11, 24
        l8ui     a8, a5, 5
        slli     a8, a8, 16
        add.n    a11, a11, a8
        l8ui     a8, a5, 4
        slli     a8, a8, 8
        add.n    a11, a11, a8
        l8ui     a9, a5, 8
        slli     a9, a9, 8
        l8ui     a8, a5, 7
        add.n    a8, a8, a9
        extui    a8, a8, 0, 16
        l8ui     a4, a5, 3
        add.n    a11, a11, a4
        l8ui     a9, a5, 1
        bnei     a9, 5, .Lsec_14_d0
        slli     a9, a9, 16
        memw
        j        .Lsec_14_d6
.Lsec_14_d0:
        extui    a9, a9, 0, 3
        slli     a9, a9, 18
.Lsec_14_d6:
        or       a8, a8, a9
        movi.n   a9, 7
        slli     a9, a9, 18
        movi.n   a12, 1
        and      a9, a8, a9
        slli     a12, a12, 18
        movi     a10, 7
        beq      a9, a12, .Lsec_14_f4
        movi.n   a10, 3
        bgei     a2, 4, .Lsec_14_f4
        movi     a10, 6
.Lsec_14_f4:
        l8ui     a9, a5, 0
        slli     a10, a10, 5
        extui    a9, a9, 0, 2
        slli     a9, a9, 8
        or       a9, a9, a10
        l8ui     a5, a5, 2
        mov.n    a12, a6
        slli     a5, a5, 14
        or       a9, a9, a5
        extui    a5, a8, 16, 16
        l32r     a4, .Lsec_14_c
        extui    a8, a8, 0, 16
        and      a5, a5, a4
        or       a9, a9, a5
        slli     a9, a9, 16
        l32r     a5, .Lsec_14_10
        or       a8, a8, a9
        addx4    a9, a2, a2
        addx8    a5, a9, a5
        memw
        s32i.n   a11, a5, 0
        mov.n    a11, a7
        l32r     a5, .Lsec_14_14
        addx8    a5, a9, a5
        memw
        s32i.n   a8, a5, 0
        movi.n   a5, 1
        ssl      a2
        sll      a5, a5
        l32r     a8, .Lsec_14_18
        memw
        l32i.n   a4, a8, 0
        or       a5, a5, a4
        l32r     a10, .Lsec_14_1c
        addx8    a10, a9, a10
        memw
        s32i.n   a5, a8, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_14_2c
        callx8   a8
        beq      a7, a3, .Lsec_14_170
        mov.n    a10, a7
        .reloc  ., R_XTENSA_ASM_EXPAND, free
        l32r     a8, .Lsec_14_30
        callx8   a8
.Lsec_14_170:
        retw.n
        .size   hal_crypto_set_key_entry, .-hal_crypto_set_key_entry
.Lsec_14_172:

        .section .text.hal_crypto_is_key_valid,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_15_0, 0x3ff73814
.Lsec_15_4:
        .global hal_crypto_is_key_valid
        .type   hal_crypto_is_key_valid, @function
hal_crypto_is_key_valid:
        entry    a1, 32
        l32r     a8, .Lsec_15_0
        memw
        l32i.n   a9, a8, 0
        movi.n   a8, 1
        ssl      a2
        sll      a2, a8
        and      a9, a2, a9
        movi.n   a2, 0
        movnez   a2, a8, a9
        retw.n
        .size   hal_crypto_is_key_valid, .-hal_crypto_is_key_valid
.Lsec_15_21:

        .section .text.hal_crypto_get_key_entry,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_16_0, .Lsec_21_0
        .literal .Lsec_16_4, 0x3ff74400
        .literal .Lsec_16_8, 0x3ff74404
        .literal .Lsec_16_c, 0x3ff74408
        .literal .Lsec_16_10, wifi_log
        .literal .Lsec_16_14, memcpy
.Lsec_16_18:
        .global hal_crypto_get_key_entry
        .type   hal_crypto_get_key_entry, @function
hal_crypto_get_key_entry:
        entry    a1, 48
        extui    a8, a3, 0, 2
        beqz.n   a8, .Lsec_16_34
        movi.n   a11, 1
        l32r     a13, .Lsec_16_0
        movi     a12, 2
        slli     a11, a11, 13
        movi     a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_16_10
        callx8   a8
.Lsec_16_34:
        l32r     a8, .Lsec_16_4
        addx4    a2, a2, a2
        addx8    a8, a2, a8
        slli     a10, a2, 3
        l32i.n   a9, a8, 0
        extui    a14, a9, 8, 8
        extui    a13, a9, 16, 8
        extui    a12, a9, 24, 8
        l32r     a8, .Lsec_16_8
        addx8    a2, a2, a8
        l32i.n   a2, a2, 0
        extui    a8, a2, 24, 2
        extui    a11, a2, 8, 8
        s8i      a8, a5, 0
        extui    a8, a2, 30, 2
        s8i      a8, a5, 2
        movi.n   a8, 31
        slli     a8, a8, 16
        and      a8, a2, a8
        s32i.n   a9, a1, 4
        s8i      a9, a5, 3
        movi.n   a9, 5
        slli     a9, a9, 16
        s8i      a11, a5, 8
        movi.n   a11, 5
        s32i.n   a2, a1, 0
        s8i      a14, a5, 4
        s8i      a13, a5, 5
        s8i      a12, a5, 6
        s8i      a2, a5, 7
        memw
        beq      a8, a9, .Lsec_16_90
        extui    a11, a2, 18, 3
.Lsec_16_90:
        addi.n   a4, a4, 3
        movi.n   a12, -4
        s8i      a11, a5, 1
        memw
        and      a12, a4, a12
        l32r     a11, .Lsec_16_c
        add.n    a11, a10, a11
        mov.n    a10, a3
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_16_14
        callx8   a8
        retw.n
        .size   hal_crypto_get_key_entry, .-hal_crypto_get_key_entry
.Lsec_16_af:

        .section .text.hal_crypto_init,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_17_0, 0x3ff73800
        .literal .Lsec_17_4, 0x00030000
        .literal .Lsec_17_8, 0x3ff73804
        .literal .Lsec_17_c, 0x3ff73808
        .literal .Lsec_17_10, 0x3ff7380c
        .literal .Lsec_17_14, 0x3ff73810
.Lsec_17_18:
        .global hal_crypto_init
        .type   hal_crypto_init, @function
hal_crypto_init:
        entry    a1, 32
        l32r     a8, .Lsec_17_4
        l32r     a9, .Lsec_17_0
        memw
        s32i.n   a8, a9, 0
        l32r     a9, .Lsec_17_8
        memw
        s32i.n   a8, a9, 0
        movi.n   a8, 0
        l32r     a9, .Lsec_17_c
        memw
        s32i.n   a8, a9, 0
        l32r     a9, .Lsec_17_10
        memw
        s32i.n   a8, a9, 0
        l32r     a9, .Lsec_17_14
        memw
        s32i.n   a8, a9, 0
        memw
        retw.n
        .size   hal_crypto_init, .-hal_crypto_init
.Lsec_17_4d:

        .section .text.hal_crypto_mgmt_rx_enabled,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_18_0, 0x3ff73804
        .literal .Lsec_18_4, 0x3ff73800
.Lsec_18_8:
        .global hal_crypto_mgmt_rx_enabled
        .type   hal_crypto_mgmt_rx_enabled, @function
hal_crypto_mgmt_rx_enabled:
        entry    a1, 32
        extui    a2, a2, 0, 8
        l32r     a8, .Lsec_18_4
        bnei     a2, 1, .Lsec_18_17
        l32r     a8, .Lsec_18_0
.Lsec_18_17:
        memw
        l32i.n   a2, a8, 0
        extui    a2, a2, 17, 1
        retw.n
        .size   hal_crypto_mgmt_rx_enabled, .-hal_crypto_mgmt_rx_enabled
.Lsec_18_21:

        .section .text.hal_crypto_enable,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_19_0, 0x3ff73804
        .literal .Lsec_19_4, 0x3ff73800
        .literal .Lsec_19_8, 0x00010103
        .literal .Lsec_19_c, 0x10010103
        .literal .Lsec_19_10, 0x3ff73810
        .literal .Lsec_19_14, 0x00ffff00
        .literal .Lsec_19_18, 0x3fffffff
        .literal .Lsec_19_1c, 0xff0000ff
.Lsec_19_20:
        .global hal_crypto_enable
        .type   hal_crypto_enable, @function
hal_crypto_enable:
        entry    a1, 32
        extui    a2, a2, 0, 8
        l32r     a8, .Lsec_19_4
        extui    a3, a3, 0, 8
        extui    a4, a4, 0, 8
        extui    a5, a5, 0, 8
        bnei     a2, 1, .Lsec_19_38
        l32r     a8, .Lsec_19_0
.Lsec_19_38:
        movi.n   a10, -5
        and      a10, a3, a10
        l32r     a9, .Lsec_19_c
        beqi     a10, 1, .Lsec_19_46
        l32r     a9, .Lsec_19_8
.Lsec_19_46:
        beqz.n   a4, .Lsec_19_50
        movi.n   a10, 1
        slli     a10, a10, 17
        or       a9, a9, a10
.Lsec_19_50:
        beqz.n   a5, .Lsec_19_58
        movi     a10, 0x200
        or       a9, a9, a10
.Lsec_19_58:
        memw
        s32i.n   a9, a8, 0
        l32r     a9, .Lsec_19_10
        memw
        l32i.n   a10, a9, 0
        bnei     a3, 4, .Lsec_19_8c
        l32r     a3, .Lsec_19_14
        or       a10, a10, a3
        memw
        s32i.n   a10, a9, 0
        movi.n   a10, -1
        slli     a10, a10, 31
        memw
        l32i.n   a9, a8, 0
        l32r     a3, .Lsec_19_18
        and      a9, a9, a3
        or       a9, a9, a10
        memw
        j        .Lsec_19_a2
.Lsec_19_8c:
        l32r     a3, .Lsec_19_1c
        and      a10, a10, a3
        memw
        s32i.n   a10, a9, 0
        memw
        l32i.n   a9, a8, 0
        l32r     a3, .Lsec_19_18
        and      a9, a9, a3
.Lsec_19_a2:
        memw
        s32i.n   a9, a8, 0
        memw
        retw.n
        .size   hal_crypto_enable, .-hal_crypto_enable
.Lsec_19_ac:

        .section .text.hal_crypto_disable,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_20_0, 0x3ff73800
        .literal .Lsec_20_4, 0x00030000
        .literal .Lsec_20_8, 0x3ff73804
        .literal .Lsec_20_c, 0x3ff73814
.Lsec_20_10:
        .global hal_crypto_disable
        .type   hal_crypto_disable, @function
hal_crypto_disable:
        entry    a1, 32
        extui    a2, a2, 0, 8
        bnez.n   a2, .Lsec_20_20
        l32r     a2, .Lsec_20_0
        j        .Lsec_20_26
        .byte    0x00
        .byte    0x00
.Lsec_20_20:
        bnei     a2, 1, .Lsec_20_2e
        l32r     a2, .Lsec_20_8
.Lsec_20_26:
        l32r     a8, .Lsec_20_4
        memw
        s32i.n   a8, a2, 0
.Lsec_20_2e:
        movi.n   a8, -1
        xor      a8, a8, a3
        l32r     a9, .Lsec_20_c
        memw
        l32i.n   a2, a9, 0
        and      a8, a8, a2
        memw
        s32i.n   a8, a9, 0
        memw
        retw.n
        .size   hal_crypto_disable, .-hal_crypto_disable
.Lsec_20_48:

        .section .rodata_wlog_warning.4,"a",@progbits
        .align  1
.Lsec_21_0:
        .byte   0x69, 0x70, 0x65, 0x6e, 0x63, 0x69, 0x6c, 0x0a, 0x00

        .section .rodata_wlog_warning.3,"a",@progbits
        .align  1
.Lsec_22_0:
        .byte   0x69, 0x70, 0x65, 0x6e, 0x63, 0x69, 0x6c, 0x0a, 0x00

        .section .rodata_wlog_warning.2,"a",@progbits
        .align  1
.Lsec_23_0:
        .byte   0x69, 0x70, 0x65, 0x6e, 0x63, 0x69, 0x6c, 0x0a, 0x00
