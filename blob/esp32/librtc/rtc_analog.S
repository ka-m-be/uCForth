.section .text.temprature_sens_read,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_22_0, 0x3ff4884c
        .literal .Lsec_22_4, 0xff00ffff
        .literal .Lsec_22_8, 0x00020000
        .literal .Lsec_22_c, 0x02000000
        .literal .Lsec_22_10, 0xfeffffff
        .literal .Lsec_22_14, 0x01000000
        .literal .Lsec_22_18, 0x04000000
        .literal .Lsec_22_1c, 0x3ff48844
        .literal .Lsec_22_20, 0xfbffffff
        .literal .Lsec_22_24, 0xfdffffff
        .literal .Lsec_22_28, 0x3fc00000
        .literal .Lsec_22_2c, esp_dport_access_reg_read
        .literal .Lsec_22_30, esp_dport_access_reg_read
        .literal .Lsec_22_34, esp_dport_access_reg_read
        .literal .Lsec_22_38, esp_dport_access_reg_read
        .literal .Lsec_22_3c, ets_delay_us
        .literal .Lsec_22_40, esp_dport_access_reg_read
        .literal .Lsec_22_44, ets_delay_us
        .literal .Lsec_22_48, esp_dport_access_reg_read
        .literal .Lsec_22_4c, esp_dport_access_reg_read
        .literal .Lsec_22_50, esp_dport_access_reg_read
        .literal .Lsec_22_54, esp_dport_access_reg_read
.Lsec_22_58:
        .global temprature_sens_read
        .type   temprature_sens_read, @function
temprature_sens_read:
        entry    a1, 32
        l32r     a3, .Lsec_22_0
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_22_2c
        callx8   a8
        l32r     a2, .Lsec_22_4
        and      a4, a10, a2
        or       a10, a3, a3
        l32r     a2, .Lsec_22_8
        or       a2, a4, a2
        memw
        s32i     a2, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_22_30
        callx8   a8
        l32r     a2, .Lsec_22_c
        or       a2, a10, a2
        mov.n    a10, a3
        memw
        s32i     a2, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_22_34
        callx8   a8
        l32r     a4, .Lsec_22_10
        and      a2, a10, a4
        mov.n    a10, a3
        memw
        s32i     a2, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_22_38
        callx8   a8
        l32r     a2, .Lsec_22_14
        or       a2, a10, a2
        movi     a10, 100
        memw
        s32i.n   a2, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_22_3c
        callx8   a8
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_22_40
        callx8   a8
        l32r     a2, .Lsec_22_18
        or       a2, a10, a2
        movi.n   a10, 2
        memw
        s32i     a2, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_22_44
        callx8   a8
        l32r     a10, .Lsec_22_1c
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_22_48
        callx8   a8
        mov.n    a5, a10
        or       a10, a3, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_22_4c
        callx8   a8
        l32r     a8, .Lsec_22_20
        and      a8, a10, a8
        mov.n    a10, a3
        memw
        s32i.n   a8, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_22_50
        callx8   a8
        and      a4, a10, a4
        mov.n    a10, a3
        memw
        s32i.n   a4, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_22_54
        callx8   a8
        l32r     a4, .Lsec_22_24
        and      a10, a10, a4
        l32r     a2, .Lsec_22_28
        and      a2, a5, a2
        extui    a2, a2, 22, 8
        memw
        s32i.n   a10, a3, 0
        retw.n
        .size   temprature_sens_read, .-temprature_sens_read
.Lsec_22_126:

        .section .text.dac_out,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_23_0, 0x3ff48898
        .literal .Lsec_23_4, 0xffff0000
        .literal .Lsec_23_8, 0x00010000
        .literal .Lsec_23_c, 0xfffeffff
        .literal .Lsec_23_10, 0x3ff4889c
        .literal .Lsec_23_14, 0x01000000
        .literal .Lsec_23_18, 0xfffcffff
        .literal .Lsec_23_1c, 0xffcfffff
        .literal .Lsec_23_20, 0x00200000
        .literal .Lsec_23_24, 0xfeffffff
        .literal .Lsec_23_28, 0x3ff48484
        .literal .Lsec_23_2c, 0xf807ffff
        .literal .Lsec_23_30, 0x02000000
        .literal .Lsec_23_34, 0xfff3ffff
        .literal .Lsec_23_38, 0xffff00ff
        .literal .Lsec_23_3c, 0xff3fffff
        .literal .Lsec_23_40, 0x00800000
        .literal .Lsec_23_44, 0xfdffffff
        .literal .Lsec_23_48, 0x3ff48488
        .literal .Lsec_23_4c, 0x00020000
        .literal .Lsec_23_50, 0xe7ffffff
        .literal .Lsec_23_54, 0x00040400
        .literal .Lsec_23_58, 0xfffbfbff
        .literal .Lsec_23_5c, esp_dport_access_reg_read
        .literal .Lsec_23_60, esp_dport_access_reg_read
        .literal .Lsec_23_64, esp_dport_access_reg_read
        .literal .Lsec_23_68, esp_dport_access_reg_read
        .literal .Lsec_23_6c, esp_dport_access_reg_read
        .literal .Lsec_23_70, esp_dport_access_reg_read
        .literal .Lsec_23_74, esp_dport_access_reg_read
        .literal .Lsec_23_78, esp_dport_access_reg_read
        .literal .Lsec_23_7c, esp_dport_access_reg_read
        .literal .Lsec_23_80, esp_dport_access_reg_read
        .literal .Lsec_23_84, esp_dport_access_reg_read
        .literal .Lsec_23_88, esp_dport_access_reg_read
        .literal .Lsec_23_8c, esp_dport_access_reg_read
        .literal .Lsec_23_90, esp_dport_access_reg_read
        .literal .Lsec_23_94, esp_dport_access_reg_read
        .literal .Lsec_23_98, esp_dport_access_reg_read
        .literal .Lsec_23_9c, esp_dport_access_reg_read
        .literal .Lsec_23_a0, esp_dport_access_reg_read
        .literal .Lsec_23_a4, esp_dport_access_reg_read
        .literal .Lsec_23_a8, esp_dport_access_reg_read
        .literal .Lsec_23_ac, esp_dport_access_reg_read
        .literal .Lsec_23_b0, esp_dport_access_reg_read
        .literal .Lsec_23_b4, esp_dport_access_reg_read
.Lsec_23_b8:
        .global dac_out
        .type   dac_out, @function
dac_out:
        entry    a1, 48
        extui    a2, a2, 0, 8
        s32i     a2, a1, 0
        extui    a2, a6, 0, 16
        extui    a4, a4, 0, 16
        extui    a3, a3, 0, 8
        extui    a7, a4, 0, 8
        l32r     a6, .Lsec_23_0
        extui    a5, a5, 0, 16
        srli     a4, a4, 8
        mov.n    a10, a6
        beqz.n   a3, .Lsec_23_109
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_23_5c
        callx8   a8
        l32r     a9, .Lsec_23_4
        and      a10, a10, a9
        or       a8, a10, a2
        or       a10, a6, a6
        memw
        s32i     a8, a6, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_23_60
        callx8   a8
        l32r     a8, .Lsec_23_8
        or       a10, a10, a8
        memw
        s32i.n   a10, a6, 0
        bbsi     a3, 0, .Lsec_23_11d
        j        .Lsec_23_18f
.Lsec_23_109:
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_23_64
        callx8   a8
        l32r     a8, .Lsec_23_c
        and      a10, a10, a8
        memw
        s32i.n   a10, a6, 0
        j        .Lsec_23_18f
.Lsec_23_11d:
        l32r     a2, .Lsec_23_10
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_23_68
        callx8   a8
        l32r     a6, .Lsec_23_14
        or       a6, a10, a6
        mov.n    a10, a2
        memw
        s32i.n   a6, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_23_6c
        callx8   a8
        extui    a6, a5, 0, 2
        slli     a6, a6, 16
        l32r     a9, .Lsec_23_18
        and      a10, a10, a9
        or       a6, a6, a10
        mov.n    a10, a2
        memw
        s32i.n   a6, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_23_70
        callx8   a8
        movi     a6, 0xffffff00
        and      a10, a10, a6
        or       a7, a10, a7
        memw
        s32i     a7, a2, 0
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_23_74
        callx8   a8
        l32r     a6, .Lsec_23_1c
        and      a10, a10, a6
        l32r     a6, .Lsec_23_20
        or       a10, a10, a6
        memw
        s32i.n   a10, a2, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a6, .Lsec_23_28
        j        .Lsec_23_1c2
.Lsec_23_18f:
        l32r     a2, .Lsec_23_10
        slli     a7, a7, 19
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_23_78
        callx8   a8
        l32r     a9, .Lsec_23_24
        and      a9, a10, a9
        l32r     a6, .Lsec_23_28
        or       a10, a6, a6
        memw
        s32i.n   a9, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_23_7c
        callx8   a8
        l32r     a9, .Lsec_23_2c
        and      a10, a10, a9
        or       a7, a10, a7
        memw
        s32i.n   a7, a6, 0
.Lsec_23_1c2:
        mov.n    a10, a2
        bbci     a3, 1, .Lsec_23_239
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_23_80
        callx8   a8
        extui    a5, a5, 8, 2
        l32r     a3, .Lsec_23_30
        slli     a5, a5, 18
        or       a3, a10, a3
        mov.n    a10, a2
        slli     a4, a4, 8
        memw
        s32i.n   a3, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_23_84
        callx8   a8
        l32r     a3, .Lsec_23_34
        and      a10, a10, a3
        or       a5, a5, a10
        or       a10, a2, a2
        memw
        s32i.n   a5, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_23_88
        callx8   a8
        l32r     a3, .Lsec_23_38
        and      a10, a10, a3
        or       a4, a10, a4
        or       a10, a2, a2
        memw
        s32i     a4, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_23_8c
        callx8   a8
        l32r     a3, .Lsec_23_3c
        and      a10, a10, a3
        l32r     a3, .Lsec_23_40
        or       a10, a10, a3
        memw
        s32i.n   a10, a2, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a3, .Lsec_23_48
        j        .Lsec_23_266
        .byte    0x00
        .byte    0x00
.Lsec_23_239:
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_23_90
        callx8   a8
        l32r     a5, .Lsec_23_44
        slli     a4, a4, 19
        and      a5, a10, a5
        l32r     a3, .Lsec_23_48
        mov.n    a10, a3
        memw
        s32i.n   a5, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_23_94
        callx8   a8
        l32r     a5, .Lsec_23_2c
        and      a10, a10, a5
        or       a4, a10, a4
        memw
        s32i.n   a4, a3, 0
.Lsec_23_266:
        mov.n    a10, a6
        nop.n
        nop.n
        nop.n
        l32i.n   a2, a1, 0
        bbci     a2, 0, .Lsec_23_2b0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_23_98
        callx8   a8
        l32r     a4, .Lsec_23_4c
        or       a4, a10, a4
        mov.n    a10, a6
        memw
        s32i     a4, a6, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_23_9c
        callx8   a8
        l32r     a4, .Lsec_23_50
        and      a4, a10, a4
        mov.n    a10, a6
        memw
        s32i.n   a4, a6, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_23_a0
        callx8   a8
        l32r     a4, .Lsec_23_54
        or       a10, a10, a4
        memw
        s32i.n   a10, a6, 0
        j        .Lsec_23_2c1
        .byte    0x00
        .byte    0x00
.Lsec_23_2b0:
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_23_a4
        callx8   a8
        l32r     a4, .Lsec_23_58
        and      a10, a10, a4
        memw
        s32i.n   a10, a6, 0
.Lsec_23_2c1:
        mov.n    a10, a3
        nop.n
        nop.n
        nop.n
        l32i     a2, a1, 0
        bbci     a2, 1, .Lsec_23_309
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_23_a8
        callx8   a8
        l32r     a2, .Lsec_23_4c
        or       a2, a10, a2
        mov.n    a10, a3
        memw
        s32i     a2, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_23_ac
        callx8   a8
        l32r     a2, .Lsec_23_50
        and      a2, a10, a2
        mov.n    a10, a3
        memw
        s32i.n   a2, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_23_b0
        callx8   a8
        l32r     a2, .Lsec_23_54
        or       a10, a10, a2
        memw
        s32i.n   a10, a3, 0
        retw.n
.Lsec_23_309:
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_23_b4
        callx8   a8
        l32r     a2, .Lsec_23_58
        and      a10, a10, a2
        memw
        s32i.n   a10, a3, 0
        retw.n
        .size   dac_out, .-dac_out
.Lsec_23_31c:

        .section .text.vdd33_init,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_24_0, 0x3ff4882c
        .literal .Lsec_24_4, 0x3ff460c0
        .literal .Lsec_24_8, 0x3ff4e050
        .literal .Lsec_24_c, g_phyFuns
        .literal .Lsec_24_10, 0x3ff48890
        .literal .Lsec_24_14, 0x08000000
        .literal .Lsec_24_18, 0x3ff48030
        .literal .Lsec_24_1c, 0x3ff4880c
        .literal .Lsec_24_20, 0x000c0000
        .literal .Lsec_24_24, 0x3ff4e05c
        .literal .Lsec_24_28, 0x00800000
        .literal .Lsec_24_2c, 0xffdfffff
        .literal .Lsec_24_30, 0x3ff48894
        .literal .Lsec_24_34, 0x8007ffff
        .literal .Lsec_24_38, 0x3ff48838
        .literal .Lsec_24_3c, 0x3fffffff
        .literal .Lsec_24_40, 0x3ff66010
        .literal .Lsec_24_44, esp_dport_access_reg_read
        .literal .Lsec_24_48, esp_dport_access_reg_read
        .literal .Lsec_24_4c, esp_dport_access_reg_read
        .literal .Lsec_24_50, esp_dport_access_reg_read
        .literal .Lsec_24_54, esp_dport_access_reg_read
        .literal .Lsec_24_58, esp_dport_access_reg_read
        .literal .Lsec_24_5c, esp_dport_access_reg_read
        .literal .Lsec_24_60, esp_dport_access_reg_read
        .literal .Lsec_24_64, esp_dport_access_reg_read
        .literal .Lsec_24_68, esp_dport_access_reg_read
        .literal .Lsec_24_6c, esp_dport_access_reg_read
        .literal .Lsec_24_70, esp_dport_access_reg_read
.Lsec_24_74:
        .global vdd33_init
        .type   vdd33_init, @function
vdd33_init:
        entry    a1, 32
        l32r     a2, .Lsec_24_0
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_24_44
        callx8   a8
        movi     a8, -225
        l32r     a3, .Lsec_24_4
        and      a8, a10, a8
        mov.n    a10, a3
        memw
        s32i     a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_24_48
        callx8   a8
        movi     a8, 128
        or       a8, a10, a8
        mov.n    a10, a2
        memw
        s32i.n   a8, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_24_4c
        callx8   a8
        movi.n   a8, 12
        or       a8, a10, a8
        l32r     a3, .Lsec_24_8
        mov.n    a10, a3
        memw
        s32i.n   a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_24_50
        callx8   a8
        movi     a2, 192
        or       a10, a10, a2
        movi.n   a15, 0
        l32r     a2, .Lsec_24_c
        movi.n   a13, 2
        mov.n    a14, a15
        movi.n   a12, 9
        mov.n    a11, a13
        l32i.n   a2, a2, 0
        l32i     a2, a2, 168
        memw
        s32i     a10, a3, 0
        movi     a10, 107
        callx8   a2
        l32r     a3, .Lsec_24_10
        or       a10, a3, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_24_54
        callx8   a8
        l32r     a2, .Lsec_24_14
        or       a8, a10, a2
        l32r     a4, .Lsec_24_18
        or       a10, a4, a4
        memw
        s32i     a8, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_24_58
        callx8   a8
        or       a2, a10, a2
        l32r     a3, .Lsec_24_1c
        mov.n    a10, a3
        memw
        s32i     a2, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_24_5c
        callx8   a8
        l32r     a8, .Lsec_24_20
        or       a8, a10, a8
        l32r     a2, .Lsec_24_24
        or       a10, a2, a2
        memw
        s32i     a8, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_24_60
        callx8   a8
        l32r     a8, .Lsec_24_28
        or       a8, a10, a8
        mov.n    a10, a2
        memw
        s32i.n   a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_24_64
        callx8   a8
        l32r     a8, .Lsec_24_2c
        and      a8, a10, a8
        l32r     a3, .Lsec_24_30
        or       a10, a3, a3
        memw
        s32i     a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_24_68
        callx8   a8
        l32r     a2, .Lsec_24_34
        and      a2, a10, a2
        l32r     a4, .Lsec_24_38
        or       a10, a4, a4
        memw
        s32i     a2, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_24_6c
        callx8   a8
        l32r     a2, .Lsec_24_3c
        and      a2, a10, a2
        l32r     a3, .Lsec_24_40
        mov.n    a10, a3
        memw
        s32i.n   a2, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_24_70
        callx8   a8
        movi.n   a2, -5
        and      a10, a10, a2
        memw
        s32i.n   a10, a3, 0
        retw.n
        .size   vdd33_init, .-vdd33_init
.Lsec_24_19a:

        .section .text.get_vdd33,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_25_0, 0x3ff4e080
        .literal .Lsec_25_4, g_phyFuns
        .literal .Lsec_25_8, 0x3ff4882c
        .literal .Lsec_25_c, 0x3ff4e05c
        .literal .Lsec_25_10, 0x00800000
        .literal .Lsec_25_14, 0x3ff4e050
        .literal .Lsec_25_18, 0x3ff4e0a0
        .literal .Lsec_25_1c, 0xff7fffff
        .literal .Lsec_25_20, 0x00000fff
        .literal .Lsec_25_24, esp_dport_access_reg_read
        .literal .Lsec_25_28, esp_dport_access_reg_read
        .literal .Lsec_25_2c, esp_dport_access_reg_read
        .literal .Lsec_25_30, esp_dport_access_reg_read
        .literal .Lsec_25_34, esp_dport_access_reg_read
        .literal .Lsec_25_38, ets_delay_us
        .literal .Lsec_25_3c, esp_dport_access_reg_read
        .literal .Lsec_25_40, esp_dport_access_reg_read
        .literal .Lsec_25_44, esp_dport_access_reg_read
        .literal .Lsec_25_48, esp_dport_access_reg_read
.Lsec_25_4c:
        .global get_vdd33
        .type   get_vdd33, @function
get_vdd33:
        entry    a1, 32
        l32r     a5, .Lsec_25_4
        movi.n   a14, 7
        movi.n   a15, 1
        mov.n    a13, a14
        movi     a12, 9
        l32i     a2, a5, 0
        movi.n   a11, 2
        movi     a10, 107
        l32i     a2, a2, 168
        callx8   a2
        movi     a2, 16
        l32r     a4, .Lsec_25_8
        or       a10, a4, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_25_24
        callx8   a8
        or       a2, a10, a2
        l32r     a3, .Lsec_25_c
        or       a10, a3, a3
        memw
        s32i.n   a2, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_25_28
        callx8   a8
        l32r     a2, .Lsec_25_10
        or       a10, a10, a2
        l32r     a2, .Lsec_25_14
        memw
        s32i.n   a10, a3, 0
.Lsec_25_9a:
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_25_2c
        callx8   a8
        extui    a10, a10, 24, 3
        bnez     a10, .Lsec_25_9a
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_25_30
        callx8   a8
        movi.n   a6, -3
        and      a10, a10, a6
        memw
        s32i.n   a10, a2, 0
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_25_34
        callx8   a8
        movi.n   a6, 2
        or       a10, a10, a6
        memw
        s32i.n   a10, a2, 0
        movi.n   a10, 10
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_25_38
        callx8   a8
.Lsec_25_d4:
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_25_3c
        callx8   a8
        extui    a10, a10, 24, 3
        bnez     a10, .Lsec_25_d4
        l32r     a2, .Lsec_25_0
        mov.n    a6, a10
        l32r     a7, .Lsec_25_18
.Lsec_25_ea:
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_25_40
        callx8   a8
        extui    a10, a10, 0, 12
        addi.n   a2, a2, 4
        add.n    a6, a6, a10
        bne      a2, a7, .Lsec_25_ea
        l32i.n   a2, a5, 0
        movi.n   a14, 7
        movi.n   a15, 0
        mov.n    a13, a14
        movi.n   a12, 9
        l32i     a2, a2, 168
        movi     a11, 2
        movi     a10, 107
        callx8   a2
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_25_44
        callx8   a8
        movi.n   a2, -17
        and      a10, a10, a2
        memw
        s32i.n   a10, a4, 0
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_25_48
        callx8   a8
        addi.n   a6, a6, 4
        srli     a6, a6, 3
        l32r     a2, .Lsec_25_1c
        and      a10, a10, a2
        l32r     a2, .Lsec_25_20
        sub      a2, a2, a6
        extui    a2, a2, 0, 16
        memw
        s32i.n   a10, a3, 0
        retw.n
        .size   get_vdd33, .-get_vdd33
.Lsec_25_147:

        .section .text.adc1_read_test,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_26_0, 0x3ff4882c
        .literal .Lsec_26_4, 0x3ff48800
        .literal .Lsec_26_8, 0x00030000
        .literal .Lsec_26_c, 0x3ff48854
        .literal .Lsec_26_10, 0x00040000
        .literal .Lsec_26_14, 0x10000000
        .literal .Lsec_26_18, 0x3ff4880c
        .literal .Lsec_26_1c, 0xfffcffff
        .literal .Lsec_26_20, 0x00020000
        .literal .Lsec_26_24, 0x3ff48834
        .literal .Lsec_26_28, 0x8007ffff
        .literal .Lsec_26_2c, 0x3ff488a0
        .literal .Lsec_26_30, 0x3ff48810
        .literal .Lsec_26_34, 0xfffff0ff
        .literal .Lsec_26_38, 0xffff0fff
        .literal .Lsec_26_3c, 0x3ff48808
        .literal .Lsec_26_40, 0xffff0000
        .literal .Lsec_26_44, 0x00010000
        .literal .Lsec_26_48, 0x80000000
        .literal .Lsec_26_4c, 0x3ff4883c
        .literal .Lsec_26_50, 0x01c00000
        .literal .Lsec_26_54, 0xfffdffff
        .literal .Lsec_26_58, esp_dport_access_reg_read
        .literal .Lsec_26_5c, esp_dport_access_reg_read
        .literal .Lsec_26_60, esp_dport_access_reg_read
        .literal .Lsec_26_64, esp_dport_access_reg_read
        .literal .Lsec_26_68, esp_dport_access_reg_read
        .literal .Lsec_26_6c, esp_dport_access_reg_read
        .literal .Lsec_26_70, esp_dport_access_reg_read
        .literal .Lsec_26_74, esp_dport_access_reg_read
        .literal .Lsec_26_78, esp_dport_access_reg_read
        .literal .Lsec_26_7c, esp_dport_access_reg_read
        .literal .Lsec_26_80, esp_dport_access_reg_read
        .literal .Lsec_26_84, esp_dport_access_reg_read
        .literal .Lsec_26_88, esp_dport_access_reg_read
        .literal .Lsec_26_8c, esp_dport_access_reg_read
        .literal .Lsec_26_90, esp_dport_access_reg_read
        .literal .Lsec_26_94, esp_dport_access_reg_read
        .literal .Lsec_26_98, esp_dport_access_reg_read
        .literal .Lsec_26_9c, esp_dport_access_reg_read
        .literal .Lsec_26_a0, esp_dport_access_reg_read
        .literal .Lsec_26_a4, esp_dport_access_reg_read
        .literal .Lsec_26_a8, esp_dport_access_reg_read
        .literal .Lsec_26_ac, esp_dport_access_reg_read
.Lsec_26_b0:
        .global adc1_read_test
        .type   adc1_read_test, @function
adc1_read_test:
        entry    a1, 48
        l32r     a5, .Lsec_26_0
        movi.n   a7, 15
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_26_58
        callx8   a8
        or       a7, a10, a7
        l32r     a6, .Lsec_26_4
        extui    a8, a4, 0, 16
        mov.n    a10, a6
        extui    a2, a2, 0, 8
        extui    a3, a3, 0, 8
        memw
        s32i.n   a7, a5, 0
        extui    a3, a3, 0, 2
        s32i.n   a8, a1, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_26_5c
        callx8   a8
        l32r     a4, .Lsec_26_8
        or       a4, a10, a4
        l32r     a5, .Lsec_26_c
        or       a10, a5, a5
        memw
        s32i     a4, a6, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_26_60
        callx8   a8
        l32r     a4, .Lsec_26_10
        or       a4, a10, a4
        mov.n    a10, a6
        memw
        s32i.n   a4, a5, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_26_64
        callx8   a8
        l32r     a4, .Lsec_26_14
        or       a4, a10, a4
        l32r     a7, .Lsec_26_18
        mov.n    a10, a7
        memw
        s32i.n   a4, a6, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_26_68
        callx8   a8
        movi.n   a6, 1
        l32r     a9, .Lsec_26_1c
        and      a10, a10, a9
        l32r     a4, .Lsec_26_20
        or       a9, a10, a4
        l32r     a4, .Lsec_26_24
        mov.n    a10, a4
        memw
        s32i.n   a9, a7, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_26_6c
        callx8   a8
        slli     a11, a2, 1
        movi.n   a9, 3
        movi.n   a12, -1
        ssl      a11
        sll      a9, a9
        xor      a9, a12, a9
        and      a10, a9, a10
        ssl      a11
        sll      a3, a3
        or       a3, a10, a3
        memw
        s32i.n   a3, a4, 0
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_26_70
        callx8   a8
        ssl      a2
        sll      a2, a6
        extui    a2, a2, 0, 12
        l32r     a3, .Lsec_26_28
        slli     a2, a2, 19
        and      a10, a10, a3
        or       a2, a2, a10
        movi.n   a4, -16
        l32r     a3, .Lsec_26_2c
        mov.n    a10, a3
        memw
        s32i     a2, a5, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_26_74
        callx8   a8
        and      a11, a10, a4
        l32r     a2, .Lsec_26_30
        mov.n    a10, a2
        memw
        s32i     a11, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_26_78
        callx8   a8
        and      a4, a10, a4
        mov.n    a10, a2
        movi     a3, -241
        memw
        s32i     a4, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_26_7c
        callx8   a8
        and      a3, a10, a3
        mov.n    a10, a2
        memw
        s32i.n   a3, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_26_80
        callx8   a8
        l32r     a3, .Lsec_26_34
        and      a3, a10, a3
        mov.n    a10, a2
        memw
        s32i     a3, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_26_84
        callx8   a8
        l32r     a3, .Lsec_26_38
        and      a3, a10, a3
        l32r     a4, .Lsec_26_3c
        mov.n    a10, a4
        memw
        s32i     a3, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_26_88
        callx8   a8
        l32r     a3, .Lsec_26_40
        and      a10, a10, a3
        l32i.n   a8, a1, 0
        or       a8, a10, a8
        mov.n    a10, a4
        memw
        s32i     a8, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_26_8c
        callx8   a8
        extui    a8, a10, 0, 16
        mov.n    a10, a7
        l32r     a2, .Lsec_26_44
        or       a8, a8, a2
        memw
        s32i     a8, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_26_90
        callx8   a8
        and      a10, a10, a3
        or       a9, a10, a6
        mov.n    a10, a5
        memw
        s32i     a9, a7, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_26_94
        callx8   a8
        l32r     a3, .Lsec_26_48
        or       a10, a10, a3
        l32r     a4, .Lsec_26_4c
        l32r     a3, .Lsec_26_50
        memw
        s32i.n   a10, a5, 0
.Lsec_26_249:
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_26_98
        callx8   a8
        and      a10, a10, a3
        extui    a10, a10, 22, 10
        bnez     a10, .Lsec_26_249
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_26_9c
        callx8   a8
        l32r     a3, .Lsec_26_54
        and      a3, a10, a3
        mov.n    a10, a5
        memw
        s32i.n   a3, a5, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_26_a0
        callx8   a8
        l32r     a4, .Lsec_26_20
        or       a10, a10, a4
        memw
        s32i     a10, a5, 0
.Lsec_26_281:
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_26_a4
        callx8   a8
        bnone    a10, a2, .Lsec_26_281
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_26_a8
        callx8   a8
        or       a2, a10, a10
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_26_ac
        callx8   a8
        l32r     a3, .Lsec_26_28
        extui    a2, a2, 0, 16
        and      a10, a10, a3
        memw
        s32i.n   a10, a5, 0
        retw.n
        .size   adc1_read_test, .-adc1_read_test
.Lsec_26_2b0:

        .section .text.adc1_amp_read_full,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_27_0, 0x3ff4882c
        .literal .Lsec_27_4, 0x3ff48800
        .literal .Lsec_27_8, 0x00030000
        .literal .Lsec_27_c, 0x3ff48854
        .literal .Lsec_27_10, 0x00040000
        .literal .Lsec_27_14, 0x10000000
        .literal .Lsec_27_18, 0x3ff48834
        .literal .Lsec_27_1c, 0x8007ffff
        .literal .Lsec_27_20, 0x80000000
        .literal .Lsec_27_24, 0x3ff48810
        .literal .Lsec_27_28, 0xffff0fff
        .literal .Lsec_27_2c, 0xfffff0ff
        .literal .Lsec_27_30, 0xff0fffff
        .literal .Lsec_27_34, 0x00300000
        .literal .Lsec_27_38, 0x000f0000
        .literal .Lsec_27_3c, 0x3ff48808
        .literal .Lsec_27_40, 0xffff0000
        .literal .Lsec_27_44, 0x3ff4880c
        .literal .Lsec_27_48, 0xfffdffff
        .literal .Lsec_27_4c, 0x00020000
        .literal .Lsec_27_50, 0x00010000
        .literal .Lsec_27_54, 0xfffbffff
        .literal .Lsec_27_58, esp_dport_access_reg_read
        .literal .Lsec_27_5c, esp_dport_access_reg_read
        .literal .Lsec_27_60, esp_dport_access_reg_read
        .literal .Lsec_27_64, esp_dport_access_reg_read
        .literal .Lsec_27_68, esp_dport_access_reg_read
        .literal .Lsec_27_6c, esp_dport_access_reg_read
        .literal .Lsec_27_70, esp_dport_access_reg_read
        .literal .Lsec_27_74, esp_dport_access_reg_read
        .literal .Lsec_27_78, esp_dport_access_reg_read
        .literal .Lsec_27_7c, esp_dport_access_reg_read
        .literal .Lsec_27_80, esp_dport_access_reg_read
        .literal .Lsec_27_84, esp_dport_access_reg_read
        .literal .Lsec_27_88, esp_dport_access_reg_read
        .literal .Lsec_27_8c, esp_dport_access_reg_read
        .literal .Lsec_27_90, esp_dport_access_reg_read
        .literal .Lsec_27_94, esp_dport_access_reg_read
        .literal .Lsec_27_98, esp_dport_access_reg_read
        .literal .Lsec_27_9c, esp_dport_access_reg_read
        .literal .Lsec_27_a0, ets_delay_us
        .literal .Lsec_27_a4, esp_dport_access_reg_read
        .literal .Lsec_27_a8, esp_dport_access_reg_read
        .literal .Lsec_27_ac, esp_dport_access_reg_read
.Lsec_27_b0:
        .global adc1_amp_read_full
        .type   adc1_amp_read_full, @function
adc1_amp_read_full:
        entry    a1, 48
        l32r     a7, .Lsec_27_0
        extui    a2, a2, 0, 8
        or       a10, a7, a7
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_27_58
        callx8   a8
        movi.n   a9, 15
        or       a11, a10, a9
        l32r     a10, .Lsec_27_4
        extui    a14, a4, 0, 16
        extui    a3, a3, 0, 8
        extui    a5, a5, 0, 16
        slli     a5, a5, 16
        memw
        s32i.n   a11, a7, 0
        extui    a6, a6, 0, 16
        s32i.n   a14, a1, 4
        s32i.n   a9, a1, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_27_5c
        callx8   a8
        l32r     a4, .Lsec_27_8
        or       a4, a10, a4
        l32r     a7, .Lsec_27_c
        mov.n    a10, a7
        l32r     a8, .Lsec_27_4
        memw
        s32i.n   a4, a8, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_27_60
        callx8   a8
        l32r     a4, .Lsec_27_10
        or       a4, a10, a4
        l32r     a10, .Lsec_27_4
        memw
        s32i.n   a4, a7, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_27_64
        callx8   a8
        l32r     a11, .Lsec_27_14
        or       a11, a10, a11
        l32r     a4, .Lsec_27_18
        or       a10, a4, a4
        l32r     a8, .Lsec_27_4
        memw
        s32i     a11, a8, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_27_68
        callx8   a8
        slli     a12, a2, 1
        movi.n   a11, 3
        ssl      a12
        sll      a11, a11
        movi.n   a13, -1
        xor      a11, a13, a11
        extui    a8, a3, 0, 2
        and      a10, a11, a10
        ssl      a12
        sll      a8, a8
        or       a8, a10, a8
        memw
        s32i.n   a8, a4, 0
        or       a10, a7, a7
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_27_6c
        callx8   a8
        movi.n   a8, 1
        ssl      a2
        sll      a2, a8
        l32r     a4, .Lsec_27_1c
        extui    a2, a2, 0, 12
        and      a10, a10, a4
        slli     a2, a2, 19
        or       a2, a2, a10
        memw
        s32i.n   a2, a7, 0
        mov.n    a10, a7
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_27_70
        callx8   a8
        l32r     a4, .Lsec_27_20
        or       a4, a10, a4
        l32r     a2, .Lsec_27_24
        or       a10, a2, a2
        memw
        s32i     a4, a7, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_27_74
        callx8   a8
        l32r     a4, .Lsec_27_28
        and      a4, a10, a4
        mov.n    a10, a2
        memw
        s32i.n   a4, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_27_78
        callx8   a8
        l32r     a4, .Lsec_27_2c
        and      a10, a10, a4
        movi     a4, 0x300
        or       a4, a10, a4
        memw
        s32i.n   a4, a2, 0
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_27_7c
        callx8   a8
        l32r     a4, .Lsec_27_30
        and      a8, a10, a4
        or       a10, a2, a2
        l32r     a4, .Lsec_27_34
        or       a4, a8, a4
        memw
        s32i     a4, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_27_80
        callx8   a8
        movi     a4, -241
        and      a10, a10, a4
        movi     a4, 128
        or       a10, a10, a4
        memw
        s32i.n   a10, a2, 0
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_27_84
        callx8   a8
        l32i.n   a9, a1, 0
        or       a9, a10, a9
        mov.n    a10, a2
        memw
        s32i     a9, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_27_88
        callx8   a8
        l32r     a8, .Lsec_27_38
        or       a8, a10, a8
        l32r     a4, .Lsec_27_3c
        mov.n    a10, a4
        memw
        s32i     a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_27_8c
        callx8   a8
        l32r     a2, .Lsec_27_40
        and      a10, a10, a2
        l32i.n   a14, a1, 4
        or       a3, a10, a14
        mov.n    a10, a4
        memw
        s32i     a3, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_27_90
        callx8   a8
        extui    a10, a10, 0, 16
        or       a5, a10, a5
        l32r     a3, .Lsec_27_44
        mov.n    a10, a3
        memw
        s32i     a5, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_27_94
        callx8   a8
        and      a10, a10, a2
        or       a6, a10, a6
        or       a10, a7, a7
        memw
        s32i     a6, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_27_98
        callx8   a8
        l32r     a2, .Lsec_27_48
        and      a2, a10, a2
        mov.n    a10, a7
        memw
        s32i.n   a2, a7, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_27_9c
        callx8   a8
        l32r     a2, .Lsec_27_4c
        or       a10, a10, a2
        memw
        s32i.n   a10, a7, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a2, .Lsec_27_50
        j        .Lsec_27_29d
.Lsec_27_295:
        movi.n   a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_27_a0
        callx8   a8
.Lsec_27_29d:
        mov.n    a10, a7
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_27_a4
        callx8   a8
        bnone    a10, a2, .Lsec_27_295
        mov.n    a10, a7
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_27_a8
        callx8   a8
        or       a2, a10, a10
        or       a10, a7, a7
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_27_ac
        callx8   a8
        l32r     a3, .Lsec_27_54
        extui    a2, a2, 0, 16
        and      a10, a10, a3
        memw
        s32i.n   a10, a7, 0
        retw.n
        .size   adc1_amp_read_full, .-adc1_amp_read_full
.Lsec_27_2cc:

        .section .text.hall_sens_read_full,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_28_0, 0x3ff48858
        .literal .Lsec_28_4, 0x04000000
        .literal .Lsec_28_8, 0x3ff48478
        .literal .Lsec_28_c, 0x80000000
        .literal .Lsec_28_10, 0x08000000
        .literal .Lsec_28_14, 0xbfffffff
        .literal .Lsec_28_18, 0x40000000
        .literal .Lsec_28_1c, 0xfbffffff
        .literal .Lsec_28_20, 0xf7ffffff
        .literal .Lsec_28_24, esp_dport_access_reg_read
        .literal .Lsec_28_28, esp_dport_access_reg_read
        .literal .Lsec_28_2c, esp_dport_access_reg_read
        .literal .Lsec_28_30, esp_dport_access_reg_read
        .literal .Lsec_28_34, ets_delay_us
        .literal .Lsec_28_38, adc1_read_test
        .literal .Lsec_28_3c, adc1_read_test
        .literal .Lsec_28_40, esp_dport_access_reg_read
        .literal .Lsec_28_44, adc1_read_test
        .literal .Lsec_28_48, adc1_read_test
        .literal .Lsec_28_4c, esp_dport_access_reg_read
        .literal .Lsec_28_50, esp_dport_access_reg_read
.Lsec_28_54:
        .global hall_sens_read_full
        .type   hall_sens_read_full, @function
hall_sens_read_full:
        entry    a1, 32
        l32r     a4, .Lsec_28_0
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_28_24
        callx8   a8
        l32r     a2, .Lsec_28_4
        or       a2, a10, a2
        l32r     a5, .Lsec_28_8
        mov.n    a10, a5
        memw
        s32i     a2, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_28_28
        callx8   a8
        l32r     a2, .Lsec_28_c
        or       a2, a10, a2
        mov.n    a10, a4
        memw
        s32i     a2, a5, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_28_2c
        callx8   a8
        l32r     a2, .Lsec_28_10
        or       a2, a10, a2
        mov.n    a10, a5
        memw
        s32i.n   a2, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_28_30
        callx8   a8
        l32r     a2, .Lsec_28_14
        and      a2, a10, a2
        movi     a10, 100
        memw
        s32i.n   a2, a5, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_28_34
        callx8   a8
        movi.n   a11, 0
        movi.n   a12, 2
        mov.n    a10, a11
        .reloc  ., R_XTENSA_ASM_EXPAND, adc1_read_test
        l32r     a8, .Lsec_28_38
        callx8   a8
        mov.n    a3, a10
        movi.n   a12, 2
        movi.n   a11, 0
        movi.n   a10, 3
        .reloc  ., R_XTENSA_ASM_EXPAND, adc1_read_test
        l32r     a8, .Lsec_28_3c
        callx8   a8
        mov.n    a2, a10
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_28_40
        callx8   a8
        l32r     a8, .Lsec_28_18
        movi.n   a11, 0
        or       a8, a10, a8
        movi.n   a12, 2
        mov.n    a10, a11
        memw
        s32i.n   a8, a5, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, adc1_read_test
        l32r     a8, .Lsec_28_44
        callx8   a8
        movi.n   a12, 2
        movi.n   a11, 0
        or       a5, a10, a10
        movi     a10, 3
        .reloc  ., R_XTENSA_ASM_EXPAND, adc1_read_test
        l32r     a8, .Lsec_28_48
        callx8   a8
        add.n    a10, a3, a10
        sub      a10, a10, a5
        sub      a2, a10, a2
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_28_4c
        callx8   a8
        l32r     a3, .Lsec_28_1c
        extui    a2, a2, 0, 16
        and      a3, a10, a3
        or       a10, a4, a4
        memw
        s32i     a3, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_28_50
        callx8   a8
        l32r     a3, .Lsec_28_20
        and      a10, a10, a3
        memw
        s32i.n   a10, a4, 0
        retw.n
        .size   hall_sens_read_full, .-hall_sens_read_full
.Lsec_28_135:

        .section .text.hall_sens_amp_read_full,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_29_0, 0x3ff48858
        .literal .Lsec_29_4, 0x04000000
        .literal .Lsec_29_8, 0x3ff48478
        .literal .Lsec_29_c, 0x80000000
        .literal .Lsec_29_10, 0x08000000
        .literal .Lsec_29_14, 0xbfffffff
        .literal .Lsec_29_18, 0x40000000
        .literal .Lsec_29_1c, 0xfbffffff
        .literal .Lsec_29_20, 0xf7ffffff
        .literal .Lsec_29_24, esp_dport_access_reg_read
        .literal .Lsec_29_28, esp_dport_access_reg_read
        .literal .Lsec_29_2c, esp_dport_access_reg_read
        .literal .Lsec_29_30, esp_dport_access_reg_read
        .literal .Lsec_29_34, ets_delay_us
        .literal .Lsec_29_38, adc1_amp_read_full
        .literal .Lsec_29_3c, esp_dport_access_reg_read
        .literal .Lsec_29_40, adc1_amp_read_full
        .literal .Lsec_29_44, esp_dport_access_reg_read
        .literal .Lsec_29_48, esp_dport_access_reg_read
.Lsec_29_4c:
        .global hall_sens_amp_read_full
        .type   hall_sens_amp_read_full, @function
hall_sens_amp_read_full:
        entry    a1, 32
        l32r     a5, .Lsec_29_0
        extui    a2, a2, 0, 16
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_29_24
        callx8   a8
        extui    a3, a3, 0, 16
        l32r     a7, .Lsec_29_4
        extui    a4, a4, 0, 16
        or       a7, a10, a7
        l32r     a6, .Lsec_29_8
        mov.n    a10, a6
        memw
        s32i.n   a7, a5, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_29_28
        callx8   a8
        l32r     a7, .Lsec_29_c
        or       a7, a10, a7
        mov.n    a10, a5
        memw
        s32i     a7, a6, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_29_2c
        callx8   a8
        l32r     a7, .Lsec_29_10
        or       a7, a10, a7
        mov.n    a10, a6
        memw
        s32i     a7, a5, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_29_30
        callx8   a8
        l32r     a7, .Lsec_29_14
        and      a7, a10, a7
        movi     a10, 100
        memw
        s32i.n   a7, a6, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_29_34
        callx8   a8
        movi.n   a11, 0
        mov.n    a14, a4
        mov.n    a13, a3
        mov.n    a12, a2
        or       a10, a11, a11
        .reloc  ., R_XTENSA_ASM_EXPAND, adc1_amp_read_full
        l32r     a8, .Lsec_29_38
        callx8   a8
        mov.n    a7, a10
        or       a10, a6, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_29_3c
        callx8   a8
        l32r     a8, .Lsec_29_18
        movi.n   a11, 0
        or       a10, a10, a8
        mov.n    a12, a2
        mov.n    a14, a4
        mov.n    a13, a3
        memw
        s32i.n   a10, a6, 0
        or       a10, a11, a11
        .reloc  ., R_XTENSA_ASM_EXPAND, adc1_amp_read_full
        l32r     a8, .Lsec_29_40
        callx8   a8
        sub      a2, a7, a10
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_29_44
        callx8   a8
        l32r     a6, .Lsec_29_1c
        extui    a2, a2, 0, 16
        and      a6, a10, a6
        or       a10, a5, a5
        memw
        s32i     a6, a5, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_29_48
        callx8   a8
        l32r     a6, .Lsec_29_20
        and      a10, a10, a6
        memw
        s32i.n   a10, a5, 0
        retw.n
        .size   hall_sens_amp_read_full, .-hall_sens_amp_read_full
.Lsec_29_11d:

        .section .text.adc2_read_test,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_30_0, 0x3ff4882c
        .literal .Lsec_30_4, 0x3ff48890
        .literal .Lsec_30_8, 0x00030000
        .literal .Lsec_30_c, 0x20000000
        .literal .Lsec_30_10, 0x3ff48894
        .literal .Lsec_30_14, 0x00040000
        .literal .Lsec_30_18, 0x3ff48838
        .literal .Lsec_30_1c, 0x8007ffff
        .literal .Lsec_30_20, 0x3ff4e050
        .literal .Lsec_30_24, 0xf7ffffff
        .literal .Lsec_30_28, 0x80000000
        .literal .Lsec_30_2c, 0x3ff4883c
        .literal .Lsec_30_30, 0x0e000000
        .literal .Lsec_30_34, 0xfffdffff
        .literal .Lsec_30_38, 0x00020000
        .literal .Lsec_30_3c, 0x00010000
        .literal .Lsec_30_40, 0x08000000
        .literal .Lsec_30_44, esp_dport_access_reg_read
        .literal .Lsec_30_48, esp_dport_access_reg_read
        .literal .Lsec_30_4c, esp_dport_access_reg_read
        .literal .Lsec_30_50, esp_dport_access_reg_read
        .literal .Lsec_30_54, esp_dport_access_reg_read
        .literal .Lsec_30_58, esp_dport_access_reg_read
        .literal .Lsec_30_5c, esp_dport_access_reg_read
        .literal .Lsec_30_60, esp_dport_access_reg_read
        .literal .Lsec_30_64, esp_dport_access_reg_read
        .literal .Lsec_30_68, esp_dport_access_reg_read
        .literal .Lsec_30_6c, esp_dport_access_reg_read
        .literal .Lsec_30_70, esp_dport_access_reg_read
        .literal .Lsec_30_74, esp_dport_access_reg_read
        .literal .Lsec_30_78, esp_dport_access_reg_read
        .literal .Lsec_30_7c, esp_dport_access_reg_read
        .literal .Lsec_30_80, esp_dport_access_reg_read
        .literal .Lsec_30_84, esp_dport_access_reg_read
        .literal .Lsec_30_88, esp_dport_access_reg_read
        .literal .Lsec_30_8c, esp_dport_access_reg_read
        .literal .Lsec_30_90, esp_dport_access_reg_read
.Lsec_30_94:
        .global adc2_read_test
        .type   adc2_read_test, @function
adc2_read_test:
        entry    a1, 32
        l32r     a4, .Lsec_30_0
        movi     a5, -225
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_30_44
        callx8   a8
        and      a10, a10, a5
        memw
        s32i.n   a10, a4, 0
        or       a10, a4, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_30_48
        callx8   a8
        movi.n   a6, 15
        or       a6, a10, a6
        l32r     a5, .Lsec_30_4
        extui    a2, a2, 0, 8
        mov.n    a10, a5
        extui    a3, a3, 0, 8
        extui    a3, a3, 0, 2
        memw
        s32i     a6, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_30_4c
        callx8   a8
        l32r     a4, .Lsec_30_8
        or       a4, a10, a4
        mov.n    a10, a5
        memw
        s32i     a4, a5, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_30_50
        callx8   a8
        l32r     a6, .Lsec_30_c
        or       a6, a10, a6
        l32r     a4, .Lsec_30_10
        or       a10, a4, a4
        memw
        s32i     a6, a5, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_30_54
        callx8   a8
        l32r     a7, .Lsec_30_14
        or       a7, a10, a7
        l32r     a6, .Lsec_30_18
        mov.n    a10, a6
        memw
        s32i     a7, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_30_58
        callx8   a8
        slli     a8, a2, 1
        movi.n   a7, 3
        ssl      a8
        sll      a7, a7
        movi.n   a9, -1
        xor      a7, a9, a7
        and      a10, a7, a10
        ssl      a8
        sll      a3, a3
        or       a3, a10, a3
        memw
        s32i.n   a3, a6, 0
        mov.n    a10, a4
        movi     a3, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_30_5c
        callx8   a8
        ssl      a2
        sll      a2, a3
        l32r     a7, .Lsec_30_1c
        extui    a2, a2, 0, 12
        slli     a2, a2, 19
        and      a10, a10, a7
        or       a2, a2, a10
        l32r     a3, .Lsec_30_20
        memw
        s32i.n   a2, a4, 0
.Lsec_30_161:
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_30_60
        callx8   a8
        extui    a10, a10, 24, 3
        bnez     a10, .Lsec_30_161
        or       a10, a3, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_30_64
        callx8   a8
        movi     a2, -2
        and      a10, a10, a2
        memw
        s32i     a10, a3, 0
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_30_68
        callx8   a8
        l32r     a2, .Lsec_30_24
        and      a2, a10, a2
        mov.n    a10, a4
        memw
        s32i.n   a2, a5, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_30_6c
        callx8   a8
        l32r     a2, .Lsec_30_28
        or       a10, a10, a2
        l32r     a6, .Lsec_30_2c
        l32r     a2, .Lsec_30_30
        memw
        s32i.n   a10, a4, 0
.Lsec_30_1b1:
        mov.n    a10, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_30_70
        callx8   a8
        and      a10, a10, a2
        extui    a10, a10, 22, 10
        bnez     a10, .Lsec_30_1b1
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_30_74
        callx8   a8
        l32r     a6, .Lsec_30_34
        and      a2, a10, a6
        mov.n    a10, a4
        memw
        s32i.n   a2, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_30_78
        callx8   a8
        l32r     a2, .Lsec_30_38
        or       a10, a10, a2
        l32r     a2, .Lsec_30_3c
        memw
        s32i     a10, a4, 0
.Lsec_30_1ec:
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_30_7c
        callx8   a8
        bnone    a10, a2, .Lsec_30_1ec
        or       a10, a4, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_30_80
        callx8   a8
        or       a2, a10, a10
        or       a10, a4, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_30_84
        callx8   a8
        and      a10, a10, a6
        memw
        s32i.n   a10, a4, 0
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_30_88
        callx8   a8
        and      a7, a10, a7
        or       a10, a3, a3
        memw
        s32i     a7, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_30_8c
        callx8   a8
        movi.n   a4, 1
        or       a4, a10, a4
        mov.n    a10, a5
        memw
        s32i.n   a4, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_30_90
        callx8   a8
        extui    a2, a2, 0, 16
        l32r     a3, .Lsec_30_40
        or       a10, a10, a3
        memw
        s32i.n   a10, a5, 0
        retw.n
        .size   adc2_read_test, .-adc2_read_test
.Lsec_30_250:

        .section .text.adc1_pad_init,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_31_0, .Lsec_42_0
        .literal .Lsec_31_4, rtc_pads_muxsel
        .literal .Lsec_31_8, rtc_pads_pu
        .literal .Lsec_31_c, rtc_pads_pd
        .literal .Lsec_31_10, rtc_pads_funie
        .literal .Lsec_31_14, rtc_pads_slpie
        .literal .Lsec_31_18, rtc_pads_slpoe
.Lsec_31_1c:
        .global adc1_pad_init
        .type   adc1_pad_init, @function
adc1_pad_init:
        entry    a1, 32
        extui    a2, a2, 0, 8
        movi     a3, 0
        bgeui    a2, 8, .Lsec_31_30
        l32r     a3, .Lsec_31_0
        addx4    a2, a2, a3
        l32i.n   a3, a2, 0
.Lsec_31_30:
        mov.n    a10, a3
        movi.n   a11, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_pads_muxsel
        l32r     a8, .Lsec_31_4
        callx8   a8
        mov.n    a10, a3
        movi.n   a11, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_pads_pu
        l32r     a8, .Lsec_31_8
        callx8   a8
        mov.n    a10, a3
        movi.n   a11, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_pads_pd
        l32r     a8, .Lsec_31_c
        callx8   a8
        mov.n    a10, a3
        movi.n   a11, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_pads_funie
        l32r     a8, .Lsec_31_10
        callx8   a8
        mov.n    a10, a3
        movi.n   a11, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_pads_slpie
        l32r     a8, .Lsec_31_14
        callx8   a8
        movi.n   a11, 0
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_pads_slpoe
        l32r     a8, .Lsec_31_18
        callx8   a8
        retw.n
        .size   adc1_pad_init, .-adc1_pad_init
.Lsec_31_6e:

        .section .text.adc1_read,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_32_0, adc1_pad_init
        .literal .Lsec_32_4, adc1_read_test
.Lsec_32_8:
        .global adc1_read
        .type   adc1_read, @function
adc1_read:
        entry    a1, 32
        extui    a2, a2, 0, 8
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, adc1_pad_init
        l32r     a8, .Lsec_32_0
        callx8   a8
        extui    a11, a3, 0, 8
        movi.n   a12, 2
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, adc1_read_test
        l32r     a8, .Lsec_32_4
        callx8   a8
        mov.n    a2, a10
        retw.n
        .size   adc1_read, .-adc1_read
.Lsec_32_28:

        .section .text.adc1_amp_read,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_33_0, adc1_pad_init
        .literal .Lsec_33_4, adc1_amp_read_full
.Lsec_33_8:
        .global adc1_amp_read
        .type   adc1_amp_read, @function
adc1_amp_read:
        entry    a1, 32
        movi     a10, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, adc1_pad_init
        l32r     a8, .Lsec_33_0
        callx8   a8
        movi.n   a11, 0
        movi.n   a14, 4
        movi.n   a13, 1
        movi     a12, 0x400
        mov.n    a10, a11
        .reloc  ., R_XTENSA_ASM_EXPAND, adc1_amp_read_full
        l32r     a8, .Lsec_33_4
        callx8   a8
        mov.n    a2, a10
        retw.n
        .size   adc1_amp_read, .-adc1_amp_read
.Lsec_33_29:

        .section .text.hall_sens_read,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_34_0, adc1_pad_init
        .literal .Lsec_34_4, adc1_pad_init
        .literal .Lsec_34_8, hall_sens_read_full
.Lsec_34_c:
        .global hall_sens_read
        .type   hall_sens_read, @function
hall_sens_read:
        entry    a1, 32
        movi     a10, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, adc1_pad_init
        l32r     a8, .Lsec_34_0
        callx8   a8
        movi.n   a10, 3
        .reloc  ., R_XTENSA_ASM_EXPAND, adc1_pad_init
        l32r     a8, .Lsec_34_4
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, hall_sens_read_full
        l32r     a8, .Lsec_34_8
        callx8   a8
        mov.n    a2, a10
        retw.n
        .size   hall_sens_read, .-hall_sens_read
.Lsec_34_2a:

        .section .text.hall_sens_amp_read,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_35_0, adc1_pad_init
        .literal .Lsec_35_4, hall_sens_amp_read_full
.Lsec_35_8:
        .global hall_sens_amp_read
        .type   hall_sens_amp_read, @function
hall_sens_amp_read:
        entry    a1, 32
        movi     a10, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, adc1_pad_init
        l32r     a8, .Lsec_35_0
        callx8   a8
        movi     a12, 100
        movi.n   a11, 1
        movi     a10, 0x3e8
        .reloc  ., R_XTENSA_ASM_EXPAND, hall_sens_amp_read_full
        l32r     a8, .Lsec_35_4
        callx8   a8
        mov.n    a2, a10
        retw.n
        .size   hall_sens_amp_read, .-hall_sens_amp_read
.Lsec_35_26:

        .section .text.adc2_pad_init,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_36_0, .Lsec_41_0
        .literal .Lsec_36_4, rtc_pads_muxsel
        .literal .Lsec_36_8, rtc_pads_pu
        .literal .Lsec_36_c, rtc_pads_pd
        .literal .Lsec_36_10, rtc_pads_funie
        .literal .Lsec_36_14, rtc_pads_slpie
        .literal .Lsec_36_18, rtc_pads_slpoe
.Lsec_36_1c:
        .global adc2_pad_init
        .type   adc2_pad_init, @function
adc2_pad_init:
        entry    a1, 32
        extui    a2, a2, 0, 8
        movi.n   a8, 9
        movi.n   a3, 0
        bltu     a8, a2, .Lsec_36_32
        l32r     a3, .Lsec_36_0
        addx4    a2, a2, a3
        l32i     a3, a2, 0
.Lsec_36_32:
        mov.n    a10, a3
        movi.n   a11, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_pads_muxsel
        l32r     a8, .Lsec_36_4
        callx8   a8
        mov.n    a10, a3
        movi.n   a11, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_pads_pu
        l32r     a8, .Lsec_36_8
        callx8   a8
        mov.n    a10, a3
        movi.n   a11, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_pads_pd
        l32r     a8, .Lsec_36_c
        callx8   a8
        mov.n    a10, a3
        movi.n   a11, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_pads_funie
        l32r     a8, .Lsec_36_10
        callx8   a8
        mov.n    a10, a3
        movi.n   a11, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_pads_slpie
        l32r     a8, .Lsec_36_14
        callx8   a8
        movi.n   a11, 0
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_pads_slpoe
        l32r     a8, .Lsec_36_18
        callx8   a8
        retw.n
        .size   adc2_pad_init, .-adc2_pad_init
.Lsec_36_70:

        .section .text.adc2_read,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_37_0, adc2_pad_init
        .literal .Lsec_37_4, adc2_read_test
.Lsec_37_8:
        .global adc2_read
        .type   adc2_read, @function
adc2_read:
        entry    a1, 32
        extui    a2, a2, 0, 8
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, adc2_pad_init
        l32r     a8, .Lsec_37_0
        callx8   a8
        extui    a3, a3, 0, 8
        mov.n    a11, a3
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, adc2_read_test
        l32r     a8, .Lsec_37_4
        callx8   a8
        mov.n    a2, a10
        retw.n
        .size   adc2_read, .-adc2_read
.Lsec_37_28:

        .section .text.adc_pad_int,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_38_0, 0x00000800
        .literal .Lsec_38_4, 0x00001000
        .literal .Lsec_38_8, 0x00002000
        .literal .Lsec_38_c, 0x00004000
        .literal .Lsec_38_10, 0x00008000
        .literal .Lsec_38_14, 0x00010000
        .literal .Lsec_38_18, 0x00020000
        .literal .Lsec_38_1c, rtc_pads_pu
        .literal .Lsec_38_20, rtc_pads_pd
        .literal .Lsec_38_24, rtc_pads_funie
        .literal .Lsec_38_28, rtc_pads_slpie
        .literal .Lsec_38_2c, rtc_pads_slpoe
.Lsec_38_30:
        .global adc_pad_int
        .type   adc_pad_int, @function
adc_pad_int:
        entry    a1, 32
        extui    a2, a2, 0, 8
        movi.n   a5, 0
        bbc      a2, a5, .Lsec_38_78
        movi.n   a9, 2
        and      a11, a3, a9
        movi.n   a8, 4
        extui    a5, a3, 0, 1
        movi.n   a10, 8
        movnez   a5, a9, a11
        and      a11, a3, a8
        movnez   a5, a8, a11
        movi.n   a9, 16
        and      a11, a3, a10
        movnez   a5, a10, a11
        movi.n   a8, 32
        and      a11, a3, a9
        movi     a10, 0x200
        movnez   a5, a10, a11
        and      a11, a3, a8
        movi     a10, 0x100
        movnez   a5, a10, a11
        extui    a10, a3, 6, 1
        movnez   a5, a9, a10
        extui    a3, a3, 7, 1
        movnez   a5, a8, a3
.Lsec_38_78:
        bbci     a2, 1, .Lsec_38_d4
        extui    a3, a4, 0, 1
        movi     a2, 0x400
        movnez   a5, a2, a3
        l32r     a2, .Lsec_38_0
        extui    a3, a4, 1, 1
        movnez   a5, a2, a3
        extui    a3, a4, 2, 1
        extui    a8, a4, 3, 1
        l32r     a2, .Lsec_38_4
        extui    a9, a4, 4, 1
        movnez   a5, a2, a3
        extui    a12, a4, 5, 1
        movi.n   a3, 64
        l32r     a2, .Lsec_38_8
        and      a11, a4, a3
        movnez   a5, a2, a8
        movi     a2, 128
        and      a10, a4, a2
        l32r     a8, .Lsec_38_c
        movnez   a5, a8, a9
        extui    a8, a4, 8, 1
        extui    a4, a4, 9, 1
        l32r     a9, .Lsec_38_10
        movnez   a5, a9, a12
        l32r     a9, .Lsec_38_14
        movnez   a5, a9, a11
        l32r     a9, .Lsec_38_18
        movnez   a5, a9, a10
        movnez   a5, a3, a8
        movnez   a5, a2, a4
.Lsec_38_d4:
        mov.n    a10, a5
        movi.n   a11, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_pads_pu
        l32r     a8, .Lsec_38_1c
        callx8   a8
        mov.n    a10, a5
        movi.n   a11, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_pads_pd
        l32r     a8, .Lsec_38_20
        callx8   a8
        mov.n    a10, a5
        movi.n   a11, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_pads_funie
        l32r     a8, .Lsec_38_24
        callx8   a8
        mov.n    a10, a5
        movi.n   a11, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_pads_slpie
        l32r     a8, .Lsec_38_28
        callx8   a8
        movi.n   a11, 0
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_pads_slpoe
        l32r     a8, .Lsec_38_2c
        callx8   a8
        retw.n
        .size   adc_pad_int, .-adc_pad_int
.Lsec_38_108:

        .section .text.adc_pad_init,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_39_0, 0x3ff4847c
        .literal .Lsec_39_4, 0x08000000
        .literal .Lsec_39_8, 0xfff7ffff
        .literal .Lsec_39_c, 0x04000000
        .literal .Lsec_39_10, 0xffffbfff
        .literal .Lsec_39_14, 0x02000000
        .literal .Lsec_39_18, 0x01000000
        .literal .Lsec_39_1c, 0x3ff4848c
        .literal .Lsec_39_20, 0x00020000
        .literal .Lsec_39_24, 0xff3fffdf
        .literal .Lsec_39_28, 0x00040000
        .literal .Lsec_39_2c, 0xe7fff7ff
        .literal .Lsec_39_30, 0x3ff48480
        .literal .Lsec_39_34, 0x20000000
        .literal .Lsec_39_38, 0xff7fffff
        .literal .Lsec_39_3c, 0x10000000
        .literal .Lsec_39_40, 0xfffbffff
        .literal .Lsec_39_44, esp_dport_access_reg_read
        .literal .Lsec_39_48, esp_dport_access_reg_read
        .literal .Lsec_39_4c, esp_dport_access_reg_read
        .literal .Lsec_39_50, esp_dport_access_reg_read
        .literal .Lsec_39_54, esp_dport_access_reg_read
        .literal .Lsec_39_58, esp_dport_access_reg_read
        .literal .Lsec_39_5c, esp_dport_access_reg_read
        .literal .Lsec_39_60, esp_dport_access_reg_read
        .literal .Lsec_39_64, esp_dport_access_reg_read
        .literal .Lsec_39_68, esp_dport_access_reg_read
        .literal .Lsec_39_6c, esp_dport_access_reg_read
        .literal .Lsec_39_70, esp_dport_access_reg_read
        .literal .Lsec_39_74, esp_dport_access_reg_read
        .literal .Lsec_39_78, esp_dport_access_reg_read
        .literal .Lsec_39_7c, esp_dport_access_reg_read
        .literal .Lsec_39_80, esp_dport_access_reg_read
.Lsec_39_84:
        .global adc_pad_init
        .type   adc_pad_init, @function
adc_pad_init:
        entry    a1, 32
        bbci     a2, 0, .Lsec_39_b4
        l32r     a3, .Lsec_39_0
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_39_44
        callx8   a8
        l32r     a8, .Lsec_39_4
        or       a8, a10, a8
        mov.n    a10, a3
        memw
        s32i     a8, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_39_48
        callx8   a8
        l32r     a8, .Lsec_39_8
        and      a10, a10, a8
        memw
        s32i.n   a10, a3, 0
.Lsec_39_b4:
        bbci     a2, 1, .Lsec_39_ec
        nop.n
        nop.n
        nop
        nop
        l32r     a3, .Lsec_39_0
        or       a10, a3, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_39_4c
        callx8   a8
        l32r     a8, .Lsec_39_c
        or       a8, a10, a8
        mov.n    a10, a3
        memw
        s32i     a8, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_39_50
        callx8   a8
        l32r     a8, .Lsec_39_10
        and      a10, a10, a8
        memw
        s32i.n   a10, a3, 0
.Lsec_39_ec:
        bbci     a2, 2, .Lsec_39_124
        nop.n
        nop.n
        nop
        nop
        l32r     a3, .Lsec_39_0
        or       a10, a3, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_39_54
        callx8   a8
        l32r     a8, .Lsec_39_14
        or       a8, a10, a8
        mov.n    a10, a3
        memw
        s32i     a8, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_39_58
        callx8   a8
        movi     a8, 0xfffffdff
        and      a10, a10, a8
        memw
        s32i.n   a10, a3, 0
.Lsec_39_124:
        bbci     a2, 3, .Lsec_39_15c
        nop.n
        nop.n
        nop
        nop
        l32r     a3, .Lsec_39_0
        or       a10, a3, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_39_5c
        callx8   a8
        l32r     a8, .Lsec_39_18
        or       a8, a10, a8
        or       a10, a3, a3
        memw
        s32i     a8, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_39_60
        callx8   a8
        movi.n   a8, -17
        and      a10, a10, a8
        memw
        s32i.n   a10, a3, 0
.Lsec_39_15c:
        bbci     a2, 4, .Lsec_39_194
        nop.n
        nop.n
        nop
        nop
        l32r     a3, .Lsec_39_1c
        or       a10, a3, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_39_64
        callx8   a8
        l32r     a8, .Lsec_39_20
        or       a8, a10, a8
        mov.n    a10, a3
        memw
        s32i     a8, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_39_68
        callx8   a8
        l32r     a8, .Lsec_39_24
        and      a10, a10, a8
        memw
        s32i.n   a10, a3, 0
.Lsec_39_194:
        bbci     a2, 5, .Lsec_39_1cc
        nop.n
        nop.n
        nop
        nop
        l32r     a3, .Lsec_39_1c
        or       a10, a3, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_39_6c
        callx8   a8
        l32r     a8, .Lsec_39_28
        or       a8, a10, a8
        mov.n    a10, a3
        memw
        s32i     a8, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_39_70
        callx8   a8
        l32r     a8, .Lsec_39_2c
        and      a10, a10, a8
        memw
        s32i.n   a10, a3, 0
.Lsec_39_1cc:
        bbci     a2, 6, .Lsec_39_204
        nop.n
        nop.n
        nop
        nop
        l32r     a3, .Lsec_39_30
        or       a10, a3, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_39_74
        callx8   a8
        l32r     a8, .Lsec_39_34
        or       a8, a10, a8
        mov.n    a10, a3
        memw
        s32i     a8, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_39_78
        callx8   a8
        l32r     a8, .Lsec_39_38
        and      a10, a10, a8
        memw
        s32i.n   a10, a3, 0
.Lsec_39_204:
        bbci     a2, 7, .Lsec_39_23c
        nop.n
        nop.n
        nop
        nop
        l32r     a2, .Lsec_39_30
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_39_7c
        callx8   a8
        l32r     a8, .Lsec_39_3c
        or       a8, a10, a8
        mov.n    a10, a2
        memw
        s32i     a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_39_80
        callx8   a8
        l32r     a8, .Lsec_39_40
        and      a10, a10, a8
        memw
        s32i.n   a10, a2, 0
.Lsec_39_23c:
        retw.n
        .size   adc_pad_init, .-adc_pad_init
.Lsec_39_23e:

        .section .text.dac_pad_init,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_40_0, 0x3ff4889c
        .literal .Lsec_40_4, 0xfcffffff
        .literal .Lsec_40_8, 0x3ff48898
        .literal .Lsec_40_c, 0x00010000
        .literal .Lsec_40_10, 0x3ff48484
        .literal .Lsec_40_14, 0x00020000
        .literal .Lsec_40_18, 0xe7ffffff
        .literal .Lsec_40_1c, 0xf807ffff
        .literal .Lsec_40_20, 0x3ff48488
        .literal .Lsec_40_24, esp_dport_access_reg_read
        .literal .Lsec_40_28, esp_dport_access_reg_read
        .literal .Lsec_40_2c, esp_dport_access_reg_read
        .literal .Lsec_40_30, esp_dport_access_reg_read
        .literal .Lsec_40_34, esp_dport_access_reg_read
        .literal .Lsec_40_38, esp_dport_access_reg_read
        .literal .Lsec_40_3c, esp_dport_access_reg_read
        .literal .Lsec_40_40, esp_dport_access_reg_read
.Lsec_40_44:
        .global dac_pad_init
        .type   dac_pad_init, @function
dac_pad_init:
        entry    a1, 32
        l32r     a4, .Lsec_40_0
        extui    a2, a2, 0, 8
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_40_24
        callx8   a8
        extui    a3, a3, 0, 8
        l32r     a8, .Lsec_40_4
        and      a8, a10, a8
        l32r     a5, .Lsec_40_8
        or       a10, a5, a5
        memw
        s32i     a8, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_40_28
        callx8   a8
        l32r     a8, .Lsec_40_c
        or       a10, a10, a8
        memw
        s32i.n   a10, a5, 0
        bbci     a2, 0, .Lsec_40_c9
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a4, .Lsec_40_10
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_40_2c
        callx8   a8
        l32r     a5, .Lsec_40_14
        or       a5, a10, a5
        mov.n    a10, a4
        memw
        s32i     a5, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_40_30
        callx8   a8
        l32r     a5, .Lsec_40_18
        and      a5, a10, a5
        mov.n    a10, a4
        memw
        s32i.n   a5, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_40_34
        callx8   a8
        slli     a5, a3, 19
        l32r     a8, .Lsec_40_1c
        and      a10, a10, a8
        or       a5, a10, a5
        memw
        s32i.n   a5, a4, 0
.Lsec_40_c9:
        bbci     a2, 1, .Lsec_40_116
        slli     a3, a3, 19
        nop.n
        nop.n
        nop.n
        l32r     a2, .Lsec_40_20
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_40_38
        callx8   a8
        l32r     a4, .Lsec_40_14
        or       a4, a10, a4
        mov.n    a10, a2
        memw
        s32i     a4, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_40_3c
        callx8   a8
        l32r     a4, .Lsec_40_18
        and      a4, a10, a4
        mov.n    a10, a2
        memw
        s32i.n   a4, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_40_40
        callx8   a8
        l32r     a4, .Lsec_40_1c
        and      a10, a10, a4
        or       a3, a10, a3
        memw
        s32i.n   a3, a2, 0
.Lsec_40_116:
        retw.n
        .size   dac_pad_init, .-dac_pad_init
.Lsec_40_118:

        .section .rodata.CSWTCH$19,"a",@progbits
        .align  4
.Lsec_41_0:
        .type   CSWTCH$19, @object
        .size   CSWTCH$19, 40
CSWTCH$19:
        .byte   0x00, 0x04, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x20, 0x00, 0x00
        .byte   0x00, 0x40, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x02, 0x00
        .byte   0x40, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00

        .section .rodata.CSWTCH$13,"a",@progbits
        .align  4
.Lsec_42_0:
        .type   CSWTCH$13, @object
        .size   CSWTCH$13, 32
CSWTCH$13:
        .byte   0x01, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00
        .byte   0x00, 0x02, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x20, 0x00, 0x00, 0x00
