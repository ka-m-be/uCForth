.type   tx_link, @object
        .comm   tx_link, 12, 4

        .section .text.rtc_mac_tx_init,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_5_0, 0x0000b000
        .literal .Lsec_5_4, 0x60033d24
        .literal .Lsec_5_8, 0x60033c6c
        .literal .Lsec_5_c, 0x60033c78
        .literal .Lsec_5_10, 0x600330d8
        .literal .Lsec_5_14, 0x600330dc
        .literal .Lsec_5_18, 0x600330e0
        .literal .Lsec_5_1c, 0x600330e4
        .literal .Lsec_5_20, 0x10000000
        .literal .Lsec_5_24, 0x60033040
        .literal .Lsec_5_28, 0x60033044
        .literal .Lsec_5_2c, 0x60033c40
        .literal .Lsec_5_30, 0x60033c4c
        .literal .Lsec_5_34, tx_link
        .literal .Lsec_5_38, 0xc0000000
        .literal .Lsec_5_3c, 0x3ff5c860
        .literal .Lsec_5_40, 0x00000c00
        .literal .Lsec_5_44, 0x3ff5c87c
        .literal .Lsec_5_48, 0x00000800
        .literal .Lsec_5_4c, 0xfffff3ff
        .literal .Lsec_5_50, 0xfffff7ff
        .literal .Lsec_5_54, 0x600340f0
        .literal .Lsec_5_58, 0xf9ffffff
        .literal .Lsec_5_5c, 0x02000000
        .literal .Lsec_5_60, 0x80000000
        .literal .Lsec_5_64, 0xfffe0fff
        .literal .Lsec_5_68, 0xfffff000
        .literal .Lsec_5_6c, 0x600340f8
        .literal .Lsec_5_70, 0x600340fc
        .literal .Lsec_5_74, 0xffff0000
        .literal .Lsec_5_78, 0x60033cec
        .literal .Lsec_5_7c, 0xf0ffffff
        .literal .Lsec_5_80, 0xffc00fff
        .literal .Lsec_5_84, 0x60033cf0
        .literal .Lsec_5_88, 0x08000000
        .literal .Lsec_5_8c, 0xf8ffffff
        .literal .Lsec_5_90, 0xfff00000
        .literal .Lsec_5_94, 0x000fffff
        .literal .Lsec_5_98, esp_dport_access_reg_read
        .literal .Lsec_5_9c, esp_dport_access_reg_read
        .literal .Lsec_5_a0, esp_dport_access_reg_read
        .literal .Lsec_5_a4, esp_dport_access_reg_read
        .literal .Lsec_5_a8, esp_dport_access_reg_read
        .literal .Lsec_5_ac, esp_dport_access_reg_read
        .literal .Lsec_5_b0, esp_dport_access_reg_read
        .literal .Lsec_5_b4, esp_dport_access_reg_read
        .literal .Lsec_5_b8, esp_dport_access_reg_read
        .literal .Lsec_5_bc, esp_dport_access_reg_read
        .literal .Lsec_5_c0, esp_dport_access_reg_read
        .literal .Lsec_5_c4, esp_dport_access_reg_read
        .literal .Lsec_5_c8, esp_dport_access_reg_read
        .literal .Lsec_5_cc, esp_dport_access_reg_read
        .literal .Lsec_5_d0, esp_dport_access_reg_read
        .literal .Lsec_5_d4, esp_dport_access_reg_read
        .literal .Lsec_5_d8, esp_dport_access_reg_read
        .literal .Lsec_5_dc, esp_dport_access_reg_read
        .literal .Lsec_5_e0, esp_dport_access_reg_read
        .literal .Lsec_5_e4, esp_dport_access_reg_read
        .literal .Lsec_5_e8, esp_dport_access_reg_read
        .literal .Lsec_5_ec, esp_dport_access_reg_read
        .literal .Lsec_5_f0, esp_dport_access_reg_read
        .literal .Lsec_5_f4, esp_dport_access_reg_read
        .literal .Lsec_5_f8, esp_dport_access_reg_read
        .literal .Lsec_5_fc, esp_dport_access_reg_read
        .literal .Lsec_5_100, esp_dport_access_reg_read
        .literal .Lsec_5_104, esp_dport_access_reg_read
        .literal .Lsec_5_108, esp_dport_access_reg_read
        .literal .Lsec_5_10c, esp_dport_access_reg_read
.Lsec_5_110:
        .global rtc_mac_tx_init
        .type   rtc_mac_tx_init, @function
rtc_mac_tx_init:
        entry    a1, 32
        l32r     a3, .Lsec_5_4
        movi.n   a4, 2
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_5_98
        callx8   a8
        or       a10, a10, a4
        memw
        s32i.n   a10, a3, 0
.Lsec_5_128:
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_5_9c
        callx8   a8
        bbci     a10, 0, .Lsec_5_128
        l32r     a3, .Lsec_5_8
        movi.n   a5, -3
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_5_a0
        callx8   a8
        and      a10, a10, a5
        memw
        s32i.n   a10, a3, 0
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_5_a4
        callx8   a8
        movi.n   a6, -17
        and      a6, a10, a6
        l32r     a5, .Lsec_5_c
        movi.n   a4, 7
        mov.n    a10, a5
        movi     a7, 128
        memw
        s32i.n   a6, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_5_a8
        callx8   a8
        movi.n   a8, 2
        or       a8, a10, a8
        l32r     a6, .Lsec_5_10
        or       a10, a6, a6
        memw
        s32i     a8, a5, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_5_ac
        callx8   a8
        or       a8, a10, a4
        l32r     a5, .Lsec_5_14
        mov.n    a10, a5
        memw
        s32i     a8, a6, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_5_b0
        callx8   a8
        or       a8, a10, a4
        l32r     a6, .Lsec_5_18
        mov.n    a10, a6
        memw
        s32i     a8, a5, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_5_b4
        callx8   a8
        or       a8, a10, a4
        l32r     a5, .Lsec_5_1c
        mov.n    a10, a5
        memw
        s32i     a8, a6, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_5_b8
        callx8   a8
        or       a4, a10, a4
        mov.n    a10, a3
        memw
        s32i.n   a4, a5, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_5_bc
        callx8   a8
        l32r     a5, .Lsec_5_20
        or       a4, a10, a5
        l32i.n   a10, a2, 8
        addi.n   a10, a10, 8
        memw
        s32i.n   a4, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_5_c0
        callx8   a8
        mov.n    a3, a10
        extui    a3, a3, 16, 16
        l32i.n   a10, a2, 8
        addi.n   a10, a10, 12
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_5_c4
        callx8   a8
        slli     a10, a10, 16
        or       a3, a3, a10
        l32r     a4, .Lsec_5_24
        l32i.n   a10, a2, 8
        addi.n   a10, a10, 12
        memw
        s32i.n   a3, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_5_c8
        callx8   a8
        extui    a6, a10, 16, 16
        l32r     a3, .Lsec_5_2c
        mov.n    a10, a3
        l32r     a4, .Lsec_5_28
        memw
        s32i     a6, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_5_cc
        callx8   a8
        or       a4, a10, a7
        l32r     a6, .Lsec_5_30
        mov.n    a10, a6
        memw
        s32i     a4, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_5_d0
        callx8   a8
        or       a10, a10, a7
        l32i.n   a3, a2, 0
        extui    a3, a3, 8, 12
        slli     a7, a3, 12
        l32r     a4, .Lsec_5_38
        add.n    a4, a7, a4
        add.n    a3, a4, a3
        l32i.n   a7, a2, 8
        l32r     a4, .Lsec_5_34
        memw
        s32i.n   a10, a6, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a6, a2, 0
        s32i.n   a3, a4, 0
        movi.n   a3, 0
        s32i.n   a7, a4, 4
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a7, .Lsec_5_3c
        mov.n    a10, a7
        s32i.n   a3, a4, 8
        bbci     a6, 21, .Lsec_5_2a0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_5_d4
        callx8   a8
        l32r     a3, .Lsec_5_40
        or       a3, a10, a3
        l32r     a6, .Lsec_5_44
        or       a10, a6, a6
        memw
        s32i     a3, a7, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_5_d8
        callx8   a8
        l32r     a3, .Lsec_5_48
        or       a10, a10, a3
        memw
        s32i.n   a10, a6, 0
        j        .Lsec_5_2c7
        .byte    0x00
        .byte    0x00
.Lsec_5_2a0:
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_5_dc
        callx8   a8
        l32r     a3, .Lsec_5_4c
        and      a3, a10, a3
        l32r     a6, .Lsec_5_44
        mov.n    a10, a6
        memw
        s32i.n   a3, a7, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_5_e0
        callx8   a8
        l32r     a3, .Lsec_5_50
        and      a10, a10, a3
        memw
        s32i.n   a10, a6, 0
.Lsec_5_2c7:
        movi.n   a6, 31
        nop.n
        nop.n
        nop.n
        l32i.n   a3, a2, 0
        extui    a3, a3, 0, 6
        bge      a6, a3, .Lsec_5_2e8
        addi     a3, a3, -16
        and      a3, a3, a6
        slli     a3, a3, 12
        movi.n   a6, 0
        l32r     a7, .Lsec_5_54
        j        .Lsec_5_3a0
.Lsec_5_2e8:
        movi.n   a6, 23
        bge      a6, a3, .Lsec_5_340
        l32r     a7, .Lsec_5_54
        mov.n    a10, a7
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_5_e4
        callx8   a8
        l32i.n   a3, a2, 0
        extui    a8, a3, 22, 1
        extui    a6, a3, 8, 12
        slli     a6, a6, 8
        l32r     a9, .Lsec_5_58
        slli     a8, a8, 30
        and      a10, a10, a9
        or       a8, a8, a6
        extui    a6, a3, 20, 1
        l32r     a9, .Lsec_5_60
        slli     a6, a6, 7
        or       a8, a8, a9
        or       a6, a8, a6
        extui    a3, a3, 0, 6
        l32r     a8, .Lsec_5_5c
        addi     a3, a3, -24
        or       a8, a10, a8
        or       a6, a6, a3
        memw
        s32i.n   a8, a7, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a3, .Lsec_5_0
        j        .Lsec_5_3a0
.Lsec_5_340:
        movi.n   a6, 15
        bge      a6, a3, .Lsec_5_395
        l32r     a7, .Lsec_5_54
        mov.n    a10, a7
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_5_e8
        callx8   a8
        l32i.n   a3, a2, 0
        extui    a11, a3, 22, 1
        extui    a8, a3, 8, 12
        extui    a6, a3, 20, 1
        l32r     a9, .Lsec_5_58
        slli     a11, a11, 30
        and      a10, a10, a9
        slli     a8, a8, 8
        or       a8, a11, a8
        l32r     a9, .Lsec_5_5c
        slli     a6, a6, 7
        extui    a3, a3, 0, 6
        addi     a3, a3, -16
        or       a9, a10, a9
        or       a6, a8, a6
        or       a6, a6, a3
        memw
        s32i.n   a9, a7, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a3, .Lsec_5_0
        j        .Lsec_5_3a0
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_5_395:
        extui    a3, a3, 0, 5
        l32r     a7, .Lsec_5_54
        slli     a3, a3, 12
        movi.n   a6, 0
.Lsec_5_3a0:
        or       a10, a7, a7
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_5_ec
        callx8   a8
        l32r     a8, .Lsec_5_64
        and      a10, a10, a8
        or       a3, a10, a3
        or       a10, a7, a7
        memw
        s32i     a3, a7, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_5_f0
        callx8   a8
        l32r     a8, .Lsec_5_68
        and      a10, a10, a8
        l32i.n   a3, a2, 0
        extui    a3, a3, 8, 12
        or       a3, a10, a3
        memw
        s32i.n   a3, a7, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a7, .Lsec_5_6c
        l32i.n   a3, a2, 0
        memw
        s32i.n   a6, a7, 0
        bbci     a3, 20, .Lsec_5_412
        nop.n
        nop.n
        nop.n
        nop
        l32r     a6, .Lsec_5_70
        or       a10, a6, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_5_f4
        callx8   a8
        l32i.n   a3, a2, 0
        extui    a3, a3, 8, 12
        l32r     a7, .Lsec_5_74
        and      a10, a10, a7
        or       a3, a10, a3
        memw
        s32i     a3, a6, 0
.Lsec_5_412:
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a6, .Lsec_5_78
        or       a10, a6, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_5_f8
        callx8   a8
        l32i.n   a3, a2, 4
        extui    a3, a3, 12, 4
        slli     a3, a3, 24
        l32r     a7, .Lsec_5_7c
        and      a10, a10, a7
        or       a3, a3, a10
        or       a10, a6, a6
        memw
        s32i     a3, a6, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_5_fc
        callx8   a8
        l32i.n   a7, a2, 4
        extui    a7, a7, 0, 10
        slli     a7, a7, 12
        l32r     a3, .Lsec_5_80
        and      a10, a10, a3
        or       a10, a7, a10
        l32i.n   a7, a2, 0
        memw
        s32i.n   a10, a6, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a3, .Lsec_5_84
        bbci     a7, 23, .Lsec_5_481
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_5_100
        callx8   a8
        l32r     a6, .Lsec_5_88
        or       a10, a10, a6
        l32i.n   a7, a2, 0
        memw
        s32i.n   a10, a3, 0
.Lsec_5_481:
        bbci     a7, 24, .Lsec_5_494
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_5_104
        callx8   a8
        or       a10, a10, a5
        memw
        s32i.n   a10, a3, 0
.Lsec_5_494:
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_5_108
        callx8   a8
        l32i.n   a2, a2, 0
        extui    a2, a2, 25, 1
        slli     a2, a2, 24
        l32r     a5, .Lsec_5_8c
        and      a10, a10, a5
        or       a2, a2, a10
        or       a10, a3, a3
        memw
        s32i     a2, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_5_10c
        callx8   a8
        l32r     a2, .Lsec_5_90
        and      a10, a10, a2
        l32r     a2, .Lsec_5_94
        and      a4, a4, a2
        or       a4, a10, a4
        memw
        s32i.n   a4, a3, 0
        retw.n
        .size   rtc_mac_tx_init, .-rtc_mac_tx_init
.Lsec_5_4d2:

        .section .text.rtc_mac_tx,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_6_0, 0x60033c4c
        .literal .Lsec_6_4, 0x60033cf0
        .literal .Lsec_6_8, 0xc0000000
        .literal .Lsec_6_c, 0x60033c48
        .literal .Lsec_6_10, 0x60033cc8
        .literal .Lsec_6_14, 0x60033cc4
        .literal .Lsec_6_18, 0x60034108
        .literal .Lsec_6_1c, esp_dport_access_reg_read
        .literal .Lsec_6_20, esp_dport_access_reg_read
        .literal .Lsec_6_24, esp_dport_access_reg_read
        .literal .Lsec_6_28, esp_dport_access_reg_read
        .literal .Lsec_6_2c, esp_dport_access_reg_read
        .literal .Lsec_6_30, esp_dport_access_reg_read
.Lsec_6_34:
        .global rtc_mac_tx
        .type   rtc_mac_tx, @function
rtc_mac_tx:
        entry    a1, 32
        l32r     a3, .Lsec_6_0
        movi     a2, 128
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_6_1c
        callx8   a8
        or       a8, a10, a2
        l32r     a4, .Lsec_6_4
        mov.n    a10, a4
        memw
        s32i.n   a8, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_6_20
        callx8   a8
        l32r     a3, .Lsec_6_8
        or       a10, a10, a3
        l32r     a3, .Lsec_6_c
        memw
        s32i.n   a10, a4, 0
.Lsec_6_66:
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_6_24
        callx8   a8
        bnone    a10, a2, .Lsec_6_66
        l32r     a10, .Lsec_6_10
        movi     a3, 64
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_6_28
        callx8   a8
        movi.n   a2, -1
        bnone    a10, a3, .Lsec_6_a0
        l32r     a2, .Lsec_6_14
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_6_2c
        callx8   a8
        or       a3, a10, a3
        l32r     a10, .Lsec_6_18
        memw
        s32i.n   a3, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_6_30
        callx8   a8
        mov.n    a2, a10
.Lsec_6_a0:
        retw.n
        .size   rtc_mac_tx, .-rtc_mac_tx
.Lsec_6_a2:
