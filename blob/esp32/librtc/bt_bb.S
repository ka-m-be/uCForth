.section .text.BT_tx_8m_enable,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_18_0, 0x3ff51000
        .literal .Lsec_18_4, 0x3ff51014
        .literal .Lsec_18_8, 0x00018021
        .literal .Lsec_18_c, 0x3ff51044
        .literal .Lsec_18_10, 0x3ff51040
        .literal .Lsec_18_14, 0xf81fffff
        .literal .Lsec_18_18, esp_dport_access_reg_read
        .literal .Lsec_18_1c, esp_dport_access_reg_read
        .literal .Lsec_18_20, esp_dport_access_reg_read
        .literal .Lsec_18_24, esp_dport_access_reg_read
.Lsec_18_28:
        .global BT_tx_8m_enable
        .type   BT_tx_8m_enable, @function
BT_tx_8m_enable:
        entry    a1, 32
        l32r     a5, .Lsec_18_0
        movi     a3, -61
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_18_18
        callx8   a8
        and      a3, a10, a3
        memw
        s32i.n   a3, a5, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a4, .Lsec_18_c
        mov.n    a10, a4
        l32r     a3, .Lsec_18_4
        l32r     a5, .Lsec_18_8
        memw
        s32i     a5, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_18_1c
        callx8   a8
        movi     a8, 0xffffff00
        and      a10, a10, a8
        movi     a8, 160
        l32r     a3, .Lsec_18_10
        or       a8, a10, a8
        mov.n    a10, a3
        memw
        s32i.n   a8, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_18_20
        callx8   a8
        extui    a8, a2, 0, 6
        slli     a8, a8, 21
        movi.n   a2, 1
        l32r     a9, .Lsec_18_14
        and      a10, a10, a9
        or       a8, a8, a10
        or       a10, a3, a3
        memw
        s32i     a8, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_18_24
        callx8   a8
        or       a10, a10, a2
        memw
        s32i.n   a10, a3, 0
        retw.n
        .size   BT_tx_8m_enable, .-BT_tx_8m_enable
.Lsec_18_a6:

        .section .text.BT_tx_if_init,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_19_0, 0x3ff5103c
        .literal .Lsec_19_4, 0x801fffff
        .literal .Lsec_19_8, 0xffe007ff
        .literal .Lsec_19_c, 0x80000000
        .literal .Lsec_19_10, esp_dport_access_reg_read
        .literal .Lsec_19_14, phy_get_fetx_delay
        .literal .Lsec_19_18, esp_dport_access_reg_read
        .literal .Lsec_19_1c, esp_dport_access_reg_read
        .literal .Lsec_19_20, esp_dport_access_reg_read
        .literal .Lsec_19_24, esp_dport_access_reg_read
.Lsec_19_28:
        .global BT_tx_if_init
        .type   BT_tx_if_init, @function
BT_tx_if_init:
        entry    a1, 32
        l32r     a3, .Lsec_19_0
        extui    a2, a2, 0, 10
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_19_10
        callx8   a8
        slli     a2, a2, 21
        l32r     a8, .Lsec_19_4
        extui    a4, a4, 0, 10
        and      a10, a10, a8
        or       a10, a2, a10
        slli     a4, a4, 1
        memw
        s32i.n   a10, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_get_fetx_delay
        l32r     a8, .Lsec_19_14
        callx8   a8
        mov.n    a2, a10
        or       a10, a3, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_19_18
        callx8   a8
        movi     a8, 0x3e8
        l32r     a9, .Lsec_19_8
        sub      a8, a8, a2
        extui    a8, a8, 0, 10
        and      a10, a10, a9
        slli     a8, a8, 11
        or       a8, a8, a10
        memw
        s32i     a8, a3, 0
        or       a10, a3, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_19_1c
        callx8   a8
        movi     a8, 0xfffff801
        and      a10, a10, a8
        or       a10, a4, a10
        memw
        s32i     a10, a3, 0
        or       a10, a3, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_19_20
        callx8   a8
        l32r     a2, .Lsec_19_c
        or       a10, a10, a2
        memw
        s32i.n   a10, a3, 0
        beqz.n   a5, .Lsec_19_bc
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_19_24
        callx8   a8
        movi.n   a2, 1
        or       a10, a10, a2
        memw
        s32i.n   a10, a3, 0
.Lsec_19_bc:
        retw.n
        .size   BT_tx_if_init, .-BT_tx_if_init
.Lsec_19_be:

        .section .text.BT_init_rx_filters,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_20_0, 0x3ff51028
        .literal .Lsec_20_4, 0x3ff5105c
        .literal .Lsec_20_8, 0xfc1fffff
        .literal .Lsec_20_c, 0x03400000
        .literal .Lsec_20_10, 0xffff07ff
        .literal .Lsec_20_14, 0x00009800
        .literal .Lsec_20_18, 0x3ff51058
        .literal .Lsec_20_1c, 0xffff03ff
        .literal .Lsec_20_20, 0x00005000
        .literal .Lsec_20_24, 0x3ff51020
        .literal .Lsec_20_28, 0x3ff5c07c
        .literal .Lsec_20_2c, 0xff00ffff
        .literal .Lsec_20_30, 0x00a10000
        .literal .Lsec_20_34, 0x3ff5c090
        .literal .Lsec_20_38, 0xc1ffffff
        .literal .Lsec_20_3c, 0x32000000
        .literal .Lsec_20_40, 0xfe0fffff
        .literal .Lsec_20_44, 0x00700000
        .literal .Lsec_20_48, 0x3ff5c080
        .literal .Lsec_20_4c, 0xfffe00ff
        .literal .Lsec_20_50, 0x0001d400
        .literal .Lsec_20_54, 0x3ff5c0d0
        .literal .Lsec_20_58, 0xff81ffff
        .literal .Lsec_20_5c, 0x00180000
        .literal .Lsec_20_60, 0x3ff5104c
        .literal .Lsec_20_64, 0xffff83ff
        .literal .Lsec_20_68, 0x00007800
        .literal .Lsec_20_6c, 0xf0ffffff
        .literal .Lsec_20_70, 0x03000000
        .literal .Lsec_20_74, 0xcfffffff
        .literal .Lsec_20_78, 0x1fffffff
        .literal .Lsec_20_7c, 0x60000000
        .literal .Lsec_20_80, 0x3ff5c084
        .literal .Lsec_20_84, 0xffe07fff
        .literal .Lsec_20_88, 0x00188000
        .literal .Lsec_20_8c, 0xffff81ff
        .literal .Lsec_20_90, 0x00001e00
        .literal .Lsec_20_94, 0xe7ffffff
        .literal .Lsec_20_98, 0xf83fffff
        .literal .Lsec_20_9c, 0x00800000
        .literal .Lsec_20_a0, 0x3ff5106c
        .literal .Lsec_20_a4, 0xfff0ffff
        .literal .Lsec_20_a8, 0x00060000
        .literal .Lsec_20_ac, 0x60032124
        .literal .Lsec_20_b0, 0x3ff51084
        .literal .Lsec_20_b4, 0x00fb0000
        .literal .Lsec_20_b8, esp_dport_access_reg_read
        .literal .Lsec_20_bc, esp_dport_access_reg_read
        .literal .Lsec_20_c0, esp_dport_access_reg_read
        .literal .Lsec_20_c4, esp_dport_access_reg_read
        .literal .Lsec_20_c8, esp_dport_access_reg_read
        .literal .Lsec_20_cc, esp_dport_access_reg_read
        .literal .Lsec_20_d0, esp_dport_access_reg_read
        .literal .Lsec_20_d4, esp_dport_access_reg_read
        .literal .Lsec_20_d8, esp_dport_access_reg_read
        .literal .Lsec_20_dc, esp_dport_access_reg_read
        .literal .Lsec_20_e0, esp_dport_access_reg_read
        .literal .Lsec_20_e4, esp_dport_access_reg_read
        .literal .Lsec_20_e8, esp_dport_access_reg_read
        .literal .Lsec_20_ec, esp_dport_access_reg_read
        .literal .Lsec_20_f0, esp_dport_access_reg_read
        .literal .Lsec_20_f4, esp_dport_access_reg_read
        .literal .Lsec_20_f8, esp_dport_access_reg_read
        .literal .Lsec_20_fc, esp_dport_access_reg_read
        .literal .Lsec_20_100, esp_dport_access_reg_read
        .literal .Lsec_20_104, esp_dport_access_reg_read
        .literal .Lsec_20_108, esp_dport_access_reg_read
        .literal .Lsec_20_10c, esp_dport_access_reg_read
.Lsec_20_110:
        .global BT_init_rx_filters
        .type   BT_init_rx_filters, @function
BT_init_rx_filters:
        entry    a1, 32
        l32r     a3, .Lsec_20_4
        movi.n   a5, 23
        mov.n    a10, a3
        extui    a2, a2, 0, 16
        neg      a2, a2
        l32r     a4, .Lsec_20_0
        extui    a2, a2, 0, 6
        memw
        s32i.n   a5, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_20_b8
        callx8   a8
        l32r     a4, .Lsec_20_8
        and      a5, a10, a4
        or       a10, a3, a3
        l32r     a4, .Lsec_20_c
        or       a4, a5, a4
        memw
        s32i     a4, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_20_bc
        callx8   a8
        l32r     a5, .Lsec_20_10
        and      a10, a10, a5
        l32r     a4, .Lsec_20_14
        or       a5, a10, a4
        l32r     a4, .Lsec_20_18
        mov.n    a10, a4
        memw
        s32i.n   a5, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_20_c0
        callx8   a8
        l32r     a3, .Lsec_20_1c
        and      a10, a10, a3
        l32r     a3, .Lsec_20_20
        or       a3, a10, a3
        l32r     a5, .Lsec_20_24
        mov.n    a10, a5
        memw
        s32i.n   a3, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_20_c4
        callx8   a8
        movi     a3, -64
        and      a10, a10, a3
        or       a2, a2, a10
        l32r     a4, .Lsec_20_28
        mov.n    a10, a4
        memw
        s32i.n   a2, a5, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_20_c8
        callx8   a8
        l32r     a5, .Lsec_20_2c
        and      a10, a10, a5
        l32r     a2, .Lsec_20_30
        or       a3, a10, a2
        l32r     a2, .Lsec_20_34
        mov.n    a10, a2
        memw
        s32i.n   a3, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_20_cc
        callx8   a8
        l32r     a3, .Lsec_20_38
        and      a6, a10, a3
        or       a10, a2, a2
        l32r     a3, .Lsec_20_3c
        or       a3, a6, a3
        memw
        s32i     a3, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_20_d0
        callx8   a8
        l32r     a6, .Lsec_20_40
        and      a10, a10, a6
        l32r     a3, .Lsec_20_44
        or       a6, a10, a3
        l32r     a3, .Lsec_20_48
        mov.n    a10, a3
        memw
        s32i.n   a6, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_20_d4
        callx8   a8
        l32r     a2, .Lsec_20_4c
        and      a10, a10, a2
        l32r     a2, .Lsec_20_50
        or       a2, a10, a2
        l32r     a6, .Lsec_20_54
        mov.n    a10, a6
        memw
        s32i.n   a2, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_20_d8
        callx8   a8
        movi     a2, 0xfffffe00
        and      a10, a10, a2
        movi     a2, 0x1d4
        or       a10, a10, a2
        memw
        s32i.n   a10, a6, 0
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_20_dc
        callx8   a8
        l32r     a2, .Lsec_20_58
        and      a10, a10, a2
        l32r     a2, .Lsec_20_5c
        or       a2, a10, a2
        l32r     a6, .Lsec_20_60
        mov.n    a10, a6
        memw
        s32i.n   a2, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_20_e0
        callx8   a8
        l32r     a2, .Lsec_20_64
        and      a8, a10, a2
        or       a10, a4, a4
        l32r     a2, .Lsec_20_68
        or       a2, a8, a2
        memw
        s32i     a2, a6, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_20_e4
        callx8   a8
        l32r     a2, .Lsec_20_6c
        and      a8, a10, a2
        mov.n    a10, a3
        l32r     a2, .Lsec_20_70
        or       a2, a8, a2
        memw
        s32i.n   a2, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_20_e8
        callx8   a8
        movi     a2, -63
        and      a10, a10, a2
        movi.n   a2, 8
        or       a10, a10, a2
        memw
        s32i.n   a10, a3, 0
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_20_ec
        callx8   a8
        l32r     a2, .Lsec_20_74
        and      a2, a10, a2
        mov.n    a10, a3
        memw
        s32i.n   a2, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_20_f0
        callx8   a8
        l32r     a4, .Lsec_20_78
        and      a10, a10, a4
        l32r     a2, .Lsec_20_7c
        or       a4, a10, a2
        l32r     a2, .Lsec_20_80
        mov.n    a10, a2
        memw
        s32i.n   a4, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_20_f4
        callx8   a8
        l32r     a3, .Lsec_20_84
        and      a4, a10, a3
        mov.n    a10, a2
        l32r     a3, .Lsec_20_88
        or       a3, a4, a3
        memw
        s32i.n   a3, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_20_f8
        callx8   a8
        l32r     a3, .Lsec_20_8c
        and      a4, a10, a3
        or       a10, a2, a2
        l32r     a3, .Lsec_20_90
        or       a3, a4, a3
        memw
        s32i     a3, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_20_fc
        callx8   a8
        l32r     a3, .Lsec_20_94
        and      a3, a10, a3
        mov.n    a10, a2
        memw
        s32i     a3, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_20_100
        callx8   a8
        l32r     a4, .Lsec_20_98
        and      a10, a10, a4
        l32r     a3, .Lsec_20_9c
        or       a4, a10, a3
        l32r     a3, .Lsec_20_a0
        mov.n    a10, a3
        memw
        s32i.n   a4, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_20_104
        callx8   a8
        l32r     a2, .Lsec_20_a4
        and      a10, a10, a2
        l32r     a2, .Lsec_20_a8
        or       a2, a10, a2
        l32r     a4, .Lsec_20_ac
        mov.n    a10, a4
        memw
        s32i.n   a2, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_20_108
        callx8   a8
        movi     a2, 0x200
        or       a2, a10, a2
        l32r     a3, .Lsec_20_b0
        or       a10, a3, a3
        memw
        s32i     a2, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_20_10c
        callx8   a8
        and      a10, a10, a5
        l32r     a5, .Lsec_20_b4
        or       a10, a10, a5
        memw
        s32i.n   a10, a3, 0
        retw.n
        .size   BT_init_rx_filters, .-BT_init_rx_filters
.Lsec_20_364:

        .section .text.bt_dgmixer_fstep_250k,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_21_0, 0x3ff51040
        .literal .Lsec_21_4, 0xffe00001
        .literal .Lsec_21_8, 0x0000199a
        .literal .Lsec_21_c, esp_dport_access_reg_read
.Lsec_21_10:
        .global bt_dgmixer_fstep_250k
        .type   bt_dgmixer_fstep_250k, @function
bt_dgmixer_fstep_250k:
        entry    a1, 32
        l32r     a2, .Lsec_21_0
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_21_c
        callx8   a8
        l32r     a8, .Lsec_21_4
        and      a10, a10, a8
        l32r     a8, .Lsec_21_8
        or       a10, a10, a8
        memw
        s32i.n   a10, a2, 0
        retw.n
        .size   bt_dgmixer_fstep_250k, .-bt_dgmixer_fstep_250k
.Lsec_21_31:

        .section .text.bt_rfoffset_en,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_22_0, 0x3ff51020
        .literal .Lsec_22_4, 0x3ff51040
        .literal .Lsec_22_8, 0xefffffff
        .literal .Lsec_22_c, esp_dport_access_reg_read
        .literal .Lsec_22_10, esp_dport_access_reg_read
        .literal .Lsec_22_14, esp_dport_access_reg_read
.Lsec_22_18:
        .global bt_rfoffset_en
        .type   bt_rfoffset_en, @function
bt_rfoffset_en:
        entry    a1, 32
        l32r     a2, .Lsec_22_0
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_22_c
        callx8   a8
        movi     a8, 0x100
        l32r     a3, .Lsec_22_4
        or       a8, a10, a8
        mov.n    a10, a3
        memw
        s32i     a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_22_10
        callx8   a8
        l32r     a8, .Lsec_22_8
        and      a8, a10, a8
        mov.n    a10, a2
        memw
        s32i.n   a8, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_22_14
        callx8   a8
        movi     a8, 128
        or       a10, a10, a8
        memw
        s32i.n   a10, a2, 0
        retw.n
        .size   bt_rfoffset_en, .-bt_rfoffset_en
.Lsec_22_5d:

        .section .text.bt_bb_init_cmplx,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_23_0, 0x3ff51040
        .literal .Lsec_23_4, 0xffe00001
        .literal .Lsec_23_8, 0x0000199a
        .literal .Lsec_23_c, 0x60032130
        .literal .Lsec_23_10, 0xffff00ff
        .literal .Lsec_23_14, 0x00008800
        .literal .Lsec_23_18, 0x600320f4
        .literal .Lsec_23_1c, 0xffffc0ff
        .literal .Lsec_23_20, 0x00000a00
        .literal .Lsec_23_24, 0x600320f0
        .literal .Lsec_23_28, 0xfe00ffff
        .literal .Lsec_23_2c, 0x00dc0000
        .literal .Lsec_23_30, 0x600320dc
        .literal .Lsec_23_34, 0xffff7fff
        .literal .Lsec_23_38, 0x6000604c
        .literal .Lsec_23_3c, 0x0000c800
        .literal .Lsec_23_40, phy_dis_hw_set_freq
        .literal .Lsec_23_44, ets_delay_us
        .literal .Lsec_23_48, BT_tx_8m_enable
        .literal .Lsec_23_4c, BT_tx_if_init
        .literal .Lsec_23_50, BT_init_rx_filters
        .literal .Lsec_23_54, esp_dport_access_reg_read
        .literal .Lsec_23_58, esp_dport_access_reg_read
        .literal .Lsec_23_5c, esp_dport_access_reg_read
        .literal .Lsec_23_60, esp_dport_access_reg_read
        .literal .Lsec_23_64, esp_dport_access_reg_read
        .literal .Lsec_23_68, esp_dport_access_reg_read
        .literal .Lsec_23_6c, esp_dport_access_reg_read
        .literal .Lsec_23_70, esp_dport_access_reg_read
        .literal .Lsec_23_74, esp_dport_access_reg_read
        .literal .Lsec_23_78, phy_en_hw_set_freq
.Lsec_23_7c:
        .global bt_bb_init_cmplx
        .type   bt_bb_init_cmplx, @function
bt_bb_init_cmplx:
        entry    a1, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_dis_hw_set_freq
        l32r     a8, .Lsec_23_40
        callx8   a8
        movi.n   a10, 2
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_23_44
        callx8   a8
        movi.n   a10, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, BT_tx_8m_enable
        l32r     a8, .Lsec_23_48
        callx8   a8
        movi.n   a13, 1
        movi     a12, 200
        movi     a11, 0x3e8
        movi     a10, 0x122
        .reloc  ., R_XTENSA_ASM_EXPAND, BT_tx_if_init
        l32r     a8, .Lsec_23_4c
        callx8   a8
        movi.n   a10, 56
        .reloc  ., R_XTENSA_ASM_EXPAND, BT_init_rx_filters
        l32r     a8, .Lsec_23_50
        callx8   a8
        l32r     a4, .Lsec_23_0
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_23_54
        callx8   a8
        l32r     a2, .Lsec_23_4
        and      a10, a10, a2
        l32r     a2, .Lsec_23_8
        or       a2, a10, a2
        l32r     a3, .Lsec_23_c
        mov.n    a10, a3
        memw
        s32i.n   a2, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_23_58
        callx8   a8
        movi     a2, 0xffffff00
        and      a10, a10, a2
        movi     a2, 230
        or       a10, a10, a2
        memw
        s32i     a10, a3, 0
        or       a10, a3, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_23_5c
        callx8   a8
        l32r     a2, .Lsec_23_10
        and      a10, a10, a2
        l32r     a4, .Lsec_23_14
        or       a5, a10, a4
        l32r     a4, .Lsec_23_18
        mov.n    a10, a4
        memw
        s32i     a5, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_23_60
        callx8   a8
        l32r     a3, .Lsec_23_1c
        and      a5, a10, a3
        mov.n    a10, a4
        l32r     a3, .Lsec_23_20
        or       a3, a5, a3
        movi.n   a5, 63
        memw
        s32i     a3, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_23_64
        callx8   a8
        or       a5, a10, a5
        l32r     a3, .Lsec_23_24
        mov.n    a10, a3
        memw
        s32i.n   a5, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_23_68
        callx8   a8
        l32r     a4, .Lsec_23_28
        and      a5, a10, a4
        mov.n    a10, a3
        l32r     a4, .Lsec_23_2c
        or       a4, a5, a4
        memw
        s32i.n   a4, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_23_6c
        callx8   a8
        movi     a4, 0xfffffe00
        and      a10, a10, a4
        movi     a4, 220
        l32r     a5, .Lsec_23_30
        or       a4, a10, a4
        mov.n    a10, a5
        memw
        s32i.n   a4, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_23_70
        callx8   a8
        l32r     a3, .Lsec_23_34
        and      a3, a10, a3
        l32r     a4, .Lsec_23_38
        mov.n    a10, a4
        memw
        s32i     a3, a5, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_23_74
        callx8   a8
        and      a10, a10, a2
        l32r     a2, .Lsec_23_3c
        or       a10, a10, a2
        memw
        s32i.n   a10, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_en_hw_set_freq
        l32r     a8, .Lsec_23_78
        callx8   a8
        retw.n
        .size   bt_bb_init_cmplx, .-bt_bb_init_cmplx
.Lsec_23_19e:

        .section .text.bt_bb_init_cmplx_reg,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_24_0, 0x3ff51040
        .literal .Lsec_24_4, 0xffe00001
        .literal .Lsec_24_8, 0x0000199a
        .literal .Lsec_24_c, BT_tx_8m_enable
        .literal .Lsec_24_10, BT_tx_if_init
        .literal .Lsec_24_14, BT_init_rx_filters
        .literal .Lsec_24_18, esp_dport_access_reg_read
.Lsec_24_1c:
        .global bt_bb_init_cmplx_reg
        .type   bt_bb_init_cmplx_reg, @function
bt_bb_init_cmplx_reg:
        entry    a1, 32
        movi     a10, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, BT_tx_8m_enable
        l32r     a8, .Lsec_24_c
        callx8   a8
        movi.n   a13, 1
        movi     a12, 200
        movi     a11, 0x3e8
        movi     a10, 0x122
        .reloc  ., R_XTENSA_ASM_EXPAND, BT_tx_if_init
        l32r     a8, .Lsec_24_10
        callx8   a8
        movi.n   a10, 56
        .reloc  ., R_XTENSA_ASM_EXPAND, BT_init_rx_filters
        l32r     a8, .Lsec_24_14
        callx8   a8
        l32r     a2, .Lsec_24_0
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_24_18
        callx8   a8
        l32r     a8, .Lsec_24_4
        and      a10, a10, a8
        l32r     a8, .Lsec_24_8
        or       a10, a10, a8
        memw
        s32i.n   a10, a2, 0
        retw.n
        .size   bt_bb_init_cmplx_reg, .-bt_bb_init_cmplx_reg
.Lsec_24_5f:

        .section .text.rw_coex_on,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_25_0, 0x600310d0
        .literal .Lsec_25_4, 0x60031300
        .literal .Lsec_25_8, 0x60033d38
        .literal .Lsec_25_c, 0x00010000
        .literal .Lsec_25_10, 0x60033d30
        .literal .Lsec_25_14, 0xf0ffffff
        .literal .Lsec_25_18, 0x04000000
        .literal .Lsec_25_1c, 0xff0fffff
        .literal .Lsec_25_20, 0x00400000
        .literal .Lsec_25_24, esp_dport_access_reg_read
        .literal .Lsec_25_28, esp_dport_access_reg_read
        .literal .Lsec_25_2c, esp_dport_access_reg_read
        .literal .Lsec_25_30, esp_dport_access_reg_read
        .literal .Lsec_25_34, esp_dport_access_reg_read
.Lsec_25_38:
        .global rw_coex_on
        .type   rw_coex_on, @function
rw_coex_on:
        entry    a1, 32
        l32r     a3, .Lsec_25_0
        movi.n   a2, 1
        or       a10, a3, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_25_24
        callx8   a8
        or       a8, a10, a2
        l32r     a4, .Lsec_25_4
        mov.n    a10, a4
        memw
        s32i     a8, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_25_28
        callx8   a8
        or       a2, a10, a2
        l32r     a3, .Lsec_25_8
        mov.n    a10, a3
        memw
        s32i.n   a2, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_25_2c
        callx8   a8
        extui    a10, a10, 0, 16
        l32r     a2, .Lsec_25_c
        or       a8, a10, a2
        l32r     a2, .Lsec_25_10
        mov.n    a10, a2
        memw
        s32i.n   a8, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_25_30
        callx8   a8
        l32r     a8, .Lsec_25_14
        and      a9, a10, a8
        or       a10, a2, a2
        l32r     a8, .Lsec_25_18
        or       a8, a9, a8
        memw
        s32i     a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_25_34
        callx8   a8
        l32r     a8, .Lsec_25_1c
        and      a10, a10, a8
        l32r     a8, .Lsec_25_20
        or       a10, a10, a8
        memw
        s32i.n   a10, a2, 0
        retw.n
        .size   rw_coex_on, .-rw_coex_on
.Lsec_25_b7:

        .section .text.force_bt_mode,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_26_0, 0x600310d0
        .literal .Lsec_26_4, 0x60031300
        .literal .Lsec_26_8, 0x3ff5c080
        .literal .Lsec_26_c, 0x60033d30
        .literal .Lsec_26_10, 0xf0ffffff
        .literal .Lsec_26_14, 0x01000000
        .literal .Lsec_26_18, 0xff0fffff
        .literal .Lsec_26_1c, 0x00100000
        .literal .Lsec_26_20, 0x60033d38
        .literal .Lsec_26_24, 0xffff0000
        .literal .Lsec_26_28, 0x60033d40
        .literal .Lsec_26_2c, 0x600041c4
        .literal .Lsec_26_30, 0x3ff5d040
        .literal .Lsec_26_34, 0x7fffffff
        .literal .Lsec_26_38, esp_dport_access_reg_read
        .literal .Lsec_26_3c, esp_dport_access_reg_read
        .literal .Lsec_26_40, esp_dport_access_reg_read
        .literal .Lsec_26_44, esp_dport_access_reg_read
        .literal .Lsec_26_48, esp_dport_access_reg_read
        .literal .Lsec_26_4c, esp_dport_access_reg_read
        .literal .Lsec_26_50, esp_dport_access_reg_read
        .literal .Lsec_26_54, esp_dport_access_reg_read
        .literal .Lsec_26_58, esp_dport_access_reg_read
.Lsec_26_5c:
        .global force_bt_mode
        .type   force_bt_mode, @function
force_bt_mode:
        entry    a1, 32
        l32r     a4, .Lsec_26_0
        movi.n   a2, -2
        or       a10, a4, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_26_38
        callx8   a8
        and      a8, a10, a2
        l32r     a3, .Lsec_26_4
        mov.n    a10, a3
        memw
        s32i     a8, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_26_3c
        callx8   a8
        and      a2, a10, a2
        l32r     a4, .Lsec_26_8
        mov.n    a10, a4
        memw
        s32i     a2, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_26_40
        callx8   a8
        movi     a3, 192
        or       a3, a10, a3
        l32r     a2, .Lsec_26_c
        mov.n    a10, a2
        memw
        s32i     a3, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_26_44
        callx8   a8
        l32r     a3, .Lsec_26_10
        and      a4, a10, a3
        mov.n    a10, a2
        l32r     a3, .Lsec_26_14
        or       a3, a4, a3
        memw
        s32i.n   a3, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_26_48
        callx8   a8
        l32r     a4, .Lsec_26_18
        and      a10, a10, a4
        l32r     a3, .Lsec_26_1c
        or       a4, a10, a3
        l32r     a3, .Lsec_26_20
        mov.n    a10, a3
        memw
        s32i.n   a4, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_26_4c
        callx8   a8
        l32r     a2, .Lsec_26_24
        and      a10, a10, a2
        movi.n   a2, 10
        or       a2, a10, a2
        l32r     a4, .Lsec_26_28
        mov.n    a10, a4
        memw
        s32i     a2, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_26_50
        callx8   a8
        movi     a2, -57
        and      a10, a10, a2
        movi.n   a2, 16
        l32r     a3, .Lsec_26_2c
        or       a2, a10, a2
        mov.n    a10, a3
        memw
        s32i.n   a2, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_26_54
        callx8   a8
        movi.n   a2, 64
        or       a2, a10, a2
        l32r     a4, .Lsec_26_30
        mov.n    a10, a4
        memw
        s32i.n   a2, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_26_58
        callx8   a8
        l32r     a2, .Lsec_26_34
        and      a10, a10, a2
        memw
        s32i.n   a10, a4, 0
        retw.n
        .size   force_bt_mode, .-force_bt_mode
.Lsec_26_13a:

        .section .text.force_wifi_mode,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_27_0, force_wifi_mode_on
        .literal .Lsec_27_4, 0x3ff51098
        .literal .Lsec_27_8, 0x600310d0
        .literal .Lsec_27_c, 0x60031300
        .literal .Lsec_27_10, 0x3ff5c080
        .literal .Lsec_27_14, 0x600041c4
        .literal .Lsec_27_18, 0x3ff5d040
        .literal .Lsec_27_1c, 0x80000000
        .literal .Lsec_27_20, esp_dport_access_reg_read
        .literal .Lsec_27_24, esp_dport_access_reg_read
        .literal .Lsec_27_28, esp_dport_access_reg_read
        .literal .Lsec_27_2c, esp_dport_access_reg_read
        .literal .Lsec_27_30, esp_dport_access_reg_read
        .literal .Lsec_27_34, phy_force_wifi_chan
        .literal .Lsec_27_38, esp_dport_access_reg_read
.Lsec_27_3c:
        .global force_wifi_mode
        .type   force_wifi_mode, @function
force_wifi_mode:
        entry    a1, 32
        l32r     a3, .Lsec_27_0
        l32i.n   a4, a3, 0
        bnez     a4, .Lsec_27_e4
        beqz     a2, .Lsec_27_60
        movi.n   a2, -2
        l32r     a4, .Lsec_27_4
        or       a10, a4, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_27_20
        callx8   a8
        and      a10, a10, a2
        memw
        s32i.n   a10, a4, 0
.Lsec_27_60:
        movi     a2, -241
        nop.n
        nop.n
        nop.n
        l32r     a5, .Lsec_27_8
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_27_24
        callx8   a8
        and      a8, a10, a2
        l32r     a4, .Lsec_27_c
        mov.n    a10, a4
        memw
        s32i     a8, a5, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_27_28
        callx8   a8
        and      a2, a10, a2
        l32r     a5, .Lsec_27_10
        mov.n    a10, a5
        memw
        s32i.n   a2, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_27_2c
        callx8   a8
        movi     a2, -193
        and      a10, a10, a2
        movi     a2, 128
        l32r     a4, .Lsec_27_14
        or       a2, a10, a2
        mov.n    a10, a4
        memw
        s32i.n   a2, a5, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_27_30
        callx8   a8
        movi     a2, -65
        and      a10, a10, a2
        memw
        s32i.n   a10, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_force_wifi_chan
        l32r     a8, .Lsec_27_34
        callx8   a8
        l32r     a4, .Lsec_27_18
        or       a10, a4, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_27_38
        callx8   a8
        l32r     a2, .Lsec_27_1c
        or       a10, a10, a2
        movi.n   a2, 1
        memw
        s32i.n   a10, a4, 0
        s32i.n   a2, a3, 0
.Lsec_27_e4:
        retw.n
        .size   force_wifi_mode, .-force_wifi_mode
.Lsec_27_e6:

        .section .text.unforce_wifi_mode,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_28_0, force_wifi_mode_on
        .literal .Lsec_28_4, 0x3ff5110c
        .literal .Lsec_28_8, 0x3ff5c080
        .literal .Lsec_28_c, 0x600041c4
        .literal .Lsec_28_10, 0x3ff5d040
        .literal .Lsec_28_14, 0x7fffffff
        .literal .Lsec_28_18, 0x3ff51098
        .literal .Lsec_28_1c, esp_dport_access_reg_read
        .literal .Lsec_28_20, esp_dport_access_reg_read
        .literal .Lsec_28_24, phy_unforce_wifi_chan
        .literal .Lsec_28_28, esp_dport_access_reg_read
        .literal .Lsec_28_2c, esp_dport_access_reg_read
        .literal .Lsec_28_30, esp_dport_access_reg_read
        .literal .Lsec_28_34, esp_dport_access_reg_read
.Lsec_28_38:
        .global unforce_wifi_mode
        .type   unforce_wifi_mode, @function
unforce_wifi_mode:
        entry    a1, 32
        l32r     a2, .Lsec_28_0
        l32i     a8, a2, 0
        bnei     a8, 1, .Lsec_28_c0
        l32r     a3, .Lsec_28_4
        or       a10, a3, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_28_1c
        callx8   a8
        bbci     a10, 30, .Lsec_28_5f
        or       a10, a3, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_28_20
        callx8   a8
        bbsi     a10, 20, .Lsec_28_c0
.Lsec_28_5f:
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_unforce_wifi_chan
        l32r     a8, .Lsec_28_24
        callx8   a8
        l32r     a4, .Lsec_28_8
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_28_28
        callx8   a8
        movi     a8, -193
        l32r     a3, .Lsec_28_c
        and      a8, a10, a8
        mov.n    a10, a3
        memw
        s32i.n   a8, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_28_2c
        callx8   a8
        movi.n   a8, 64
        or       a8, a10, a8
        l32r     a4, .Lsec_28_10
        or       a10, a4, a4
        memw
        s32i.n   a8, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_28_30
        callx8   a8
        l32r     a8, .Lsec_28_14
        and      a8, a10, a8
        l32r     a3, .Lsec_28_18
        mov.n    a10, a3
        memw
        s32i.n   a8, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_28_34
        callx8   a8
        movi.n   a8, 1
        or       a10, a10, a8
        memw
        s32i.n   a10, a3, 0
        movi.n   a3, 0
        s32i.n   a3, a2, 0
.Lsec_28_c0:
        retw.n
        .size   unforce_wifi_mode, .-unforce_wifi_mode
.Lsec_28_c2:

        .section .text.coex_bt_high_prio,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_29_0, 0x3ff5c080
        .literal .Lsec_29_4, 0x600310d0
        .literal .Lsec_29_8, 0x60031300
        .literal .Lsec_29_c, 0x60033d30
        .literal .Lsec_29_10, 0xf0ffffff
        .literal .Lsec_29_14, 0x01000000
        .literal .Lsec_29_18, 0xff0fffff
        .literal .Lsec_29_1c, 0x00400000
        .literal .Lsec_29_20, 0x60033d38
        .literal .Lsec_29_24, 0x0c800000
        .literal .Lsec_29_28, 0xffff0000
        .literal .Lsec_29_2c, 0x60033d40
        .literal .Lsec_29_30, 0x600041c4
        .literal .Lsec_29_34, 0x3ff5d040
        .literal .Lsec_29_38, 0x7fffffff
        .literal .Lsec_29_3c, esp_dport_access_reg_read
        .literal .Lsec_29_40, esp_dport_access_reg_read
        .literal .Lsec_29_44, esp_dport_access_reg_read
        .literal .Lsec_29_48, esp_dport_access_reg_read
        .literal .Lsec_29_4c, esp_dport_access_reg_read
        .literal .Lsec_29_50, esp_dport_access_reg_read
        .literal .Lsec_29_54, esp_dport_access_reg_read
        .literal .Lsec_29_58, esp_dport_access_reg_read
        .literal .Lsec_29_5c, esp_dport_access_reg_read
        .literal .Lsec_29_60, esp_dport_access_reg_read
        .literal .Lsec_29_64, esp_dport_access_reg_read
        .literal .Lsec_29_68, esp_dport_access_reg_read
        .literal .Lsec_29_6c, esp_dport_access_reg_read
.Lsec_29_70:
        .global coex_bt_high_prio
        .type   coex_bt_high_prio, @function
coex_bt_high_prio:
        entry    a1, 32
        l32r     a3, .Lsec_29_0
        movi     a2, -193
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_29_3c
        callx8   a8
        and      a2, a10, a2
        l32r     a5, .Lsec_29_4
        mov.n    a10, a5
        memw
        s32i     a2, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_29_40
        callx8   a8
        movi     a2, -241
        and      a3, a10, a2
        l32r     a4, .Lsec_29_8
        or       a10, a4, a4
        memw
        s32i     a3, a5, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_29_44
        callx8   a8
        and      a3, a10, a2
        l32r     a2, .Lsec_29_c
        or       a10, a2, a2
        memw
        s32i     a3, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_29_48
        callx8   a8
        movi.n   a3, 1
        or       a6, a10, a3
        mov.n    a10, a2
        memw
        s32i.n   a6, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_29_4c
        callx8   a8
        l32r     a6, .Lsec_29_10
        and      a8, a10, a6
        mov.n    a10, a2
        l32r     a6, .Lsec_29_14
        or       a6, a8, a6
        memw
        s32i.n   a6, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_29_50
        callx8   a8
        l32r     a8, .Lsec_29_18
        and      a10, a10, a8
        l32r     a6, .Lsec_29_1c
        or       a8, a10, a6
        l32r     a6, .Lsec_29_20
        mov.n    a10, a6
        memw
        s32i.n   a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_29_54
        callx8   a8
        extui    a8, a10, 0, 16
        mov.n    a10, a6
        l32r     a2, .Lsec_29_24
        or       a2, a8, a2
        memw
        s32i     a2, a6, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_29_58
        callx8   a8
        movi.n   a2, 10
        l32r     a8, .Lsec_29_28
        and      a10, a10, a8
        or       a8, a10, a2
        l32r     a2, .Lsec_29_2c
        or       a10, a2, a2
        memw
        s32i     a8, a6, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_29_5c
        callx8   a8
        movi     a8, -57
        and      a10, a10, a8
        movi.n   a8, 16
        l32r     a6, .Lsec_29_30
        or       a8, a10, a8
        or       a10, a6, a6
        memw
        s32i     a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_29_60
        callx8   a8
        movi.n   a2, 64
        or       a2, a10, a2
        mov.n    a10, a5
        memw
        s32i     a2, a6, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_29_64
        callx8   a8
        or       a2, a10, a3
        mov.n    a10, a4
        memw
        s32i.n   a2, a5, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_29_68
        callx8   a8
        or       a3, a10, a3
        l32r     a5, .Lsec_29_34
        mov.n    a10, a5
        memw
        s32i.n   a3, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_29_6c
        callx8   a8
        l32r     a2, .Lsec_29_38
        and      a10, a10, a2
        memw
        s32i.n   a10, a5, 0
        retw.n
        .size   coex_bt_high_prio, .-coex_bt_high_prio
.Lsec_29_19d:

        .section .text.bt_cmplx_hq_re,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_30_0, g_phyFuns
.Lsec_30_4:
        .global bt_cmplx_hq_re
        .type   bt_cmplx_hq_re, @function
bt_cmplx_hq_re:
        entry    a1, 32
        l32r     a8, .Lsec_30_0
        movi.n   a13, 6
        movi.n   a14, 0
        mov.n    a12, a13
        movi.n   a11, 1
        l32i.n   a8, a8, 0
        movi     a10, 103
        l32i     a8, a8, 164
        callx8   a8
        mov.n    a2, a10
        retw.n
        .size   bt_cmplx_hq_re, .-bt_cmplx_hq_re
.Lsec_30_21:

        .section .text.bt_cmplx_lq_re,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_31_0, g_phyFuns
.Lsec_31_4:
        .global bt_cmplx_lq_re
        .type   bt_cmplx_lq_re, @function
bt_cmplx_lq_re:
        entry    a1, 32
        l32r     a8, .Lsec_31_0
        movi.n   a14, 0
        movi.n   a13, 6
        movi.n   a12, 5
        movi.n   a11, 1
        l32i.n   a8, a8, 0
        movi     a10, 103
        l32i     a8, a8, 164
        callx8   a8
        mov.n    a2, a10
        retw.n
        .size   bt_cmplx_lq_re, .-bt_cmplx_lq_re
.Lsec_31_21:

        .section .rodata.str1.4,"aMS",@progbits,1
        .align  4
.Lsec_32_0:
        .ascii  "13:39:26\000\000\000\000"
.Lsec_32_c:
        .ascii  "Mar 22 2024\000"
.Lsec_32_18:
        .ascii  "6759f983\000\000\000\000"
.Lsec_32_24:
        .ascii  "librtc version: "
        .ascii  "%s, %s, %s\012\000"

        .section .text.bt_bb_version,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_33_0, .Lsec_32_0
        .literal .Lsec_33_4, .Lsec_32_c
        .literal .Lsec_33_8, .Lsec_32_18
        .literal .Lsec_33_c, .Lsec_32_24
        .literal .Lsec_33_10, rtc_printf
.Lsec_33_14:
        .global bt_bb_version
        .type   bt_bb_version, @function
bt_bb_version:
        entry    a1, 32
        l32r     a13, .Lsec_33_0
        l32r     a12, .Lsec_33_4
        l32r     a11, .Lsec_33_8
        l32r     a10, .Lsec_33_c
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_printf
        l32r     a8, .Lsec_33_10
        callx8   a8
        retw.n
        .size   bt_bb_version, .-bt_bb_version
.Lsec_33_2b:

        .section .bss.force_wifi_mode_on,"aw",@nobits
        .align  4
.Lsec_34_0:
        .global force_wifi_mode_on
        .type   force_wifi_mode_on, @object
        .size   force_wifi_mode_on, 4
force_wifi_mode_on:
        .skip   4
