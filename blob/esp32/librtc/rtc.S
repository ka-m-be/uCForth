.section .text.rtc_cmd_wakeup_conf,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_27_0, 0x3ff48830
        .literal .Lsec_27_4, 0xffc007ff
        .literal .Lsec_27_8, 0x00008000
        .literal .Lsec_27_c, 0x3ff4882c
        .literal .Lsec_27_10, esp_dport_access_reg_read
        .literal .Lsec_27_14, esp_dport_access_reg_read
        .literal .Lsec_27_18, esp_dport_access_reg_read
        .literal .Lsec_27_1c, esp_dport_access_reg_read
.Lsec_27_20:
        .global rtc_cmd_wakeup_conf
        .type   rtc_cmd_wakeup_conf, @function
rtc_cmd_wakeup_conf:
        entry    a1, 32
        l32r     a3, .Lsec_27_0
        movi     a2, 0xfffff800
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_27_10
        callx8   a8
        and      a10, a10, a2
        movi     a2, 0x118
        or       a10, a10, a2
        memw
        s32i     a10, a3, 0
        or       a10, a3, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_27_14
        callx8   a8
        l32r     a8, .Lsec_27_4
        and      a10, a10, a8
        l32r     a2, .Lsec_27_8
        or       a8, a10, a2
        l32r     a2, .Lsec_27_c
        mov.n    a10, a2
        memw
        s32i.n   a8, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_27_18
        callx8   a8
        movi     a3, 0xfffffdff
        and      a3, a10, a3
        mov.n    a10, a2
        memw
        s32i.n   a3, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_27_1c
        callx8   a8
        movi     a3, 0x300
        or       a10, a10, a3
        memw
        s32i.n   a10, a2, 0
        retw.n
        .size   rtc_cmd_wakeup_conf, .-rtc_cmd_wakeup_conf
.Lsec_27_85:

        .section .text.rtc_pads_muxsel,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_28_0, 0x3ff4847c
        .literal .Lsec_28_4, 0xf7ffffff
        .literal .Lsec_28_8, 0xfbffffff
        .literal .Lsec_28_c, 0xfdffffff
        .literal .Lsec_28_10, 0xfeffffff
        .literal .Lsec_28_14, 0x3ff48480
        .literal .Lsec_28_18, 0xdfffffff
        .literal .Lsec_28_1c, 0xefffffff
        .literal .Lsec_28_20, 0x3ff48484
        .literal .Lsec_28_24, 0xfffdffff
        .literal .Lsec_28_28, 0x3ff48488
        .literal .Lsec_28_2c, 0x3ff4848c
        .literal .Lsec_28_30, 0xfffbffff
        .literal .Lsec_28_34, 0x3ff48494
        .literal .Lsec_28_38, 0xfff7ffff
        .literal .Lsec_28_3c, 0x3ff48498
        .literal .Lsec_28_40, 0x3ff4849c
        .literal .Lsec_28_44, 0x3ff484a0
        .literal .Lsec_28_48, 0x3ff484a4
        .literal .Lsec_28_4c, 0x3ff484a8
        .literal .Lsec_28_50, 0x3ff484ac
        .literal .Lsec_28_54, 0x3ff484b0
        .literal .Lsec_28_58, esp_dport_access_reg_read
        .literal .Lsec_28_5c, esp_dport_access_reg_read
        .literal .Lsec_28_60, esp_dport_access_reg_read
        .literal .Lsec_28_64, esp_dport_access_reg_read
        .literal .Lsec_28_68, esp_dport_access_reg_read
        .literal .Lsec_28_6c, esp_dport_access_reg_read
        .literal .Lsec_28_70, esp_dport_access_reg_read
        .literal .Lsec_28_74, esp_dport_access_reg_read
        .literal .Lsec_28_78, esp_dport_access_reg_read
        .literal .Lsec_28_7c, esp_dport_access_reg_read
        .literal .Lsec_28_80, esp_dport_access_reg_read
        .literal .Lsec_28_84, esp_dport_access_reg_read
        .literal .Lsec_28_88, esp_dport_access_reg_read
        .literal .Lsec_28_8c, esp_dport_access_reg_read
        .literal .Lsec_28_90, esp_dport_access_reg_read
        .literal .Lsec_28_94, esp_dport_access_reg_read
        .literal .Lsec_28_98, esp_dport_access_reg_read
        .literal .Lsec_28_9c, esp_dport_access_reg_read
.Lsec_28_a0:
        .global rtc_pads_muxsel
        .type   rtc_pads_muxsel, @function
rtc_pads_muxsel:
        entry    a1, 32
        bbci     a2, 0, .Lsec_28_c5
        l32r     a5, .Lsec_28_0
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_28_58
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_28_4
        slli     a8, a8, 27
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_28_c5:
        bbci     a2, 1, .Lsec_28_f0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a5, .Lsec_28_0
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_28_5c
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_28_8
        slli     a8, a8, 26
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_28_f0:
        bbci     a2, 2, .Lsec_28_11c
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_28_0
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_28_60
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_28_c
        slli     a8, a8, 25
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_28_11c:
        bbci     a2, 3, .Lsec_28_148
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_28_0
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_28_64
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_28_10
        slli     a8, a8, 24
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_28_148:
        bbci     a2, 4, .Lsec_28_174
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_28_14
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_28_68
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_28_18
        slli     a8, a8, 29
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_28_174:
        bbci     a2, 5, .Lsec_28_1a0
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_28_14
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_28_6c
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_28_1c
        slli     a8, a8, 28
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_28_1a0:
        bbci     a2, 6, .Lsec_28_1cc
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_28_20
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_28_70
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_28_24
        slli     a8, a8, 17
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_28_1cc:
        bbci     a2, 7, .Lsec_28_1f8
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_28_28
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_28_74
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_28_24
        slli     a8, a8, 17
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_28_1f8:
        bbci     a2, 8, .Lsec_28_224
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_28_2c
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_28_78
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_28_30
        slli     a8, a8, 18
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_28_224:
        bbci     a2, 9, .Lsec_28_250
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_28_2c
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_28_7c
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_28_24
        slli     a8, a8, 17
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_28_250:
        bbci     a2, 10, .Lsec_28_27c
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_28_34
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_28_80
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_28_38
        slli     a8, a8, 19
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_28_27c:
        bbci     a2, 11, .Lsec_28_2a8
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_28_3c
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_28_84
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_28_38
        slli     a8, a8, 19
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_28_2a8:
        bbci     a2, 12, .Lsec_28_2d4
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_28_40
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_28_88
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_28_38
        slli     a8, a8, 19
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_28_2d4:
        bbci     a2, 13, .Lsec_28_300
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_28_44
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_28_8c
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_28_38
        slli     a8, a8, 19
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_28_300:
        bbci     a2, 14, .Lsec_28_32c
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_28_48
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_28_90
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_28_38
        slli     a8, a8, 19
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_28_32c:
        bbci     a2, 15, .Lsec_28_358
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_28_4c
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_28_94
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_28_38
        slli     a8, a8, 19
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_28_358:
        bbci     a2, 16, .Lsec_28_384
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_28_50
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_28_98
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_28_38
        slli     a8, a8, 19
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_28_384:
        bbci     a2, 17, .Lsec_28_3aa
        extui    a3, a3, 0, 1
        slli     a3, a3, 19
        nop.n
        nop.n
        l32r     a4, .Lsec_28_54
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_28_9c
        callx8   a8
        l32r     a2, .Lsec_28_38
        and      a10, a10, a2
        or       a3, a3, a10
        memw
        s32i.n   a3, a4, 0
.Lsec_28_3aa:
        retw.n
        .size   rtc_pads_muxsel, .-rtc_pads_muxsel
.Lsec_28_3ac:

        .section .text.rtc_pads_funsel,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_29_0, 0x3ff4847c
        .literal .Lsec_29_4, 0xff3fffff
        .literal .Lsec_29_8, 0xfff9ffff
        .literal .Lsec_29_c, 0xffffcfff
        .literal .Lsec_29_10, 0x3ff48480
        .literal .Lsec_29_14, 0xf3ffffff
        .literal .Lsec_29_18, 0xff9fffff
        .literal .Lsec_29_1c, 0x3ff48484
        .literal .Lsec_29_20, 0xfffe7fff
        .literal .Lsec_29_24, 0x3ff48488
        .literal .Lsec_29_28, 0x3ff4848c
        .literal .Lsec_29_2c, 0x3ff48494
        .literal .Lsec_29_30, 0x3ff48498
        .literal .Lsec_29_34, 0x3ff4849c
        .literal .Lsec_29_38, 0x3ff484a0
        .literal .Lsec_29_3c, 0x3ff484a4
        .literal .Lsec_29_40, 0x3ff484a8
        .literal .Lsec_29_44, 0x3ff484ac
        .literal .Lsec_29_48, 0x3ff484b0
        .literal .Lsec_29_4c, esp_dport_access_reg_read
        .literal .Lsec_29_50, esp_dport_access_reg_read
        .literal .Lsec_29_54, esp_dport_access_reg_read
        .literal .Lsec_29_58, esp_dport_access_reg_read
        .literal .Lsec_29_5c, esp_dport_access_reg_read
        .literal .Lsec_29_60, esp_dport_access_reg_read
        .literal .Lsec_29_64, esp_dport_access_reg_read
        .literal .Lsec_29_68, esp_dport_access_reg_read
        .literal .Lsec_29_6c, esp_dport_access_reg_read
        .literal .Lsec_29_70, esp_dport_access_reg_read
        .literal .Lsec_29_74, esp_dport_access_reg_read
        .literal .Lsec_29_78, esp_dport_access_reg_read
        .literal .Lsec_29_7c, esp_dport_access_reg_read
        .literal .Lsec_29_80, esp_dport_access_reg_read
        .literal .Lsec_29_84, esp_dport_access_reg_read
        .literal .Lsec_29_88, esp_dport_access_reg_read
        .literal .Lsec_29_8c, esp_dport_access_reg_read
        .literal .Lsec_29_90, esp_dport_access_reg_read
.Lsec_29_94:
        .global rtc_pads_funsel
        .type   rtc_pads_funsel, @function
rtc_pads_funsel:
        entry    a1, 32
        bbci     a2, 0, .Lsec_29_b9
        l32r     a5, .Lsec_29_0
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_29_4c
        callx8   a8
        extui    a8, a3, 0, 2
        l32r     a4, .Lsec_29_4
        slli     a8, a8, 22
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_29_b9:
        bbci     a2, 1, .Lsec_29_e5
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a5, .Lsec_29_0
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_29_50
        callx8   a8
        extui    a8, a3, 0, 2
        l32r     a4, .Lsec_29_8
        slli     a8, a8, 17
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i     a10, a5, 0
.Lsec_29_e5:
        bbci     a2, 2, .Lsec_29_111
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a5, .Lsec_29_0
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_29_54
        callx8   a8
        extui    a8, a3, 0, 2
        l32r     a4, .Lsec_29_c
        slli     a8, a8, 12
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i     a10, a5, 0
.Lsec_29_111:
        bbci     a2, 3, .Lsec_29_139
        movi     a4, 0xfffffe7f
        nop.n
        nop.n
        nop.n
        l32r     a5, .Lsec_29_0
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_29_58
        callx8   a8
        extui    a8, a3, 0, 2
        slli     a8, a8, 7
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_29_139:
        bbci     a2, 4, .Lsec_29_165
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a5, .Lsec_29_10
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_29_5c
        callx8   a8
        extui    a8, a3, 0, 2
        l32r     a4, .Lsec_29_14
        slli     a8, a8, 26
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i     a10, a5, 0
.Lsec_29_165:
        bbci     a2, 5, .Lsec_29_191
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a5, .Lsec_29_10
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_29_60
        callx8   a8
        extui    a8, a3, 0, 2
        l32r     a4, .Lsec_29_18
        slli     a8, a8, 21
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i     a10, a5, 0
.Lsec_29_191:
        bbci     a2, 6, .Lsec_29_1bd
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a5, .Lsec_29_1c
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_29_64
        callx8   a8
        extui    a8, a3, 0, 2
        l32r     a4, .Lsec_29_20
        slli     a8, a8, 15
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i     a10, a5, 0
.Lsec_29_1bd:
        bbci     a2, 7, .Lsec_29_1e9
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a5, .Lsec_29_24
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_29_68
        callx8   a8
        extui    a8, a3, 0, 2
        l32r     a4, .Lsec_29_20
        slli     a8, a8, 15
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i     a10, a5, 0
.Lsec_29_1e9:
        bbci     a2, 8, .Lsec_29_215
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a5, .Lsec_29_28
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_29_6c
        callx8   a8
        extui    a8, a3, 0, 2
        l32r     a4, .Lsec_29_20
        slli     a8, a8, 15
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i     a10, a5, 0
.Lsec_29_215:
        bbci     a2, 9, .Lsec_29_23d
        movi     a4, 0xfffff9ff
        nop.n
        nop.n
        nop.n
        l32r     a5, .Lsec_29_28
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_29_70
        callx8   a8
        extui    a8, a3, 0, 2
        slli     a8, a8, 9
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_29_23d:
        bbci     a2, 10, .Lsec_29_268
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a5, .Lsec_29_2c
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_29_74
        callx8   a8
        extui    a8, a3, 0, 2
        l32r     a4, .Lsec_29_8
        slli     a8, a8, 17
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_29_268:
        bbci     a2, 11, .Lsec_29_294
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_29_30
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_29_78
        callx8   a8
        extui    a8, a3, 0, 2
        l32r     a4, .Lsec_29_8
        slli     a8, a8, 17
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_29_294:
        bbci     a2, 12, .Lsec_29_2c0
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_29_34
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_29_7c
        callx8   a8
        extui    a8, a3, 0, 2
        l32r     a4, .Lsec_29_8
        slli     a8, a8, 17
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_29_2c0:
        bbci     a2, 13, .Lsec_29_2ec
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_29_38
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_29_80
        callx8   a8
        extui    a8, a3, 0, 2
        l32r     a4, .Lsec_29_8
        slli     a8, a8, 17
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_29_2ec:
        bbci     a2, 14, .Lsec_29_318
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_29_3c
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_29_84
        callx8   a8
        extui    a8, a3, 0, 2
        l32r     a4, .Lsec_29_8
        slli     a8, a8, 17
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_29_318:
        bbci     a2, 15, .Lsec_29_344
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_29_40
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_29_88
        callx8   a8
        extui    a8, a3, 0, 2
        l32r     a4, .Lsec_29_8
        slli     a8, a8, 17
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_29_344:
        bbci     a2, 16, .Lsec_29_370
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_29_44
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_29_8c
        callx8   a8
        extui    a8, a3, 0, 2
        l32r     a4, .Lsec_29_8
        slli     a8, a8, 17
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_29_370:
        bbci     a2, 17, .Lsec_29_396
        extui    a3, a3, 0, 2
        slli     a3, a3, 17
        nop.n
        nop.n
        l32r     a4, .Lsec_29_48
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_29_90
        callx8   a8
        l32r     a2, .Lsec_29_8
        and      a10, a10, a2
        or       a3, a3, a10
        memw
        s32i.n   a3, a4, 0
.Lsec_29_396:
        retw.n
        .size   rtc_pads_funsel, .-rtc_pads_funsel
.Lsec_29_398:

        .section .text.rtc_pads_slpsel,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_30_0, 0x3ff4847c
        .literal .Lsec_30_4, 0xffdfffff
        .literal .Lsec_30_8, 0xfffeffff
        .literal .Lsec_30_c, 0xfffff7ff
        .literal .Lsec_30_10, 0x3ff48480
        .literal .Lsec_30_14, 0xfdffffff
        .literal .Lsec_30_18, 0xffefffff
        .literal .Lsec_30_1c, 0x3ff48484
        .literal .Lsec_30_20, 0xffffbfff
        .literal .Lsec_30_24, 0x3ff48488
        .literal .Lsec_30_28, 0x3ff4848c
        .literal .Lsec_30_2c, 0x3ff48494
        .literal .Lsec_30_30, 0x3ff48498
        .literal .Lsec_30_34, 0x3ff4849c
        .literal .Lsec_30_38, 0x3ff484a0
        .literal .Lsec_30_3c, 0x3ff484a4
        .literal .Lsec_30_40, 0x3ff484a8
        .literal .Lsec_30_44, 0x3ff484ac
        .literal .Lsec_30_48, 0x3ff484b0
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
        .global rtc_pads_slpsel
        .type   rtc_pads_slpsel, @function
rtc_pads_slpsel:
        entry    a1, 32
        bbci     a2, 0, .Lsec_30_b9
        l32r     a5, .Lsec_30_0
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_30_4c
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_30_4
        slli     a8, a8, 21
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_30_b9:
        bbci     a2, 1, .Lsec_30_e5
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a5, .Lsec_30_0
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_30_50
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_30_8
        slli     a8, a8, 16
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i     a10, a5, 0
.Lsec_30_e5:
        bbci     a2, 2, .Lsec_30_111
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a5, .Lsec_30_0
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_30_54
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_30_c
        slli     a8, a8, 11
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i     a10, a5, 0
.Lsec_30_111:
        bbci     a2, 3, .Lsec_30_139
        movi     a4, -65
        nop.n
        nop.n
        nop.n
        l32r     a5, .Lsec_30_0
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_30_58
        callx8   a8
        extui    a8, a3, 0, 1
        slli     a8, a8, 6
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_30_139:
        bbci     a2, 4, .Lsec_30_165
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a5, .Lsec_30_10
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_30_5c
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_30_14
        slli     a8, a8, 25
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i     a10, a5, 0
.Lsec_30_165:
        bbci     a2, 5, .Lsec_30_191
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a5, .Lsec_30_10
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_30_60
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_30_18
        slli     a8, a8, 20
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i     a10, a5, 0
.Lsec_30_191:
        bbci     a2, 6, .Lsec_30_1bd
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a5, .Lsec_30_1c
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_30_64
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_30_20
        slli     a8, a8, 14
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i     a10, a5, 0
.Lsec_30_1bd:
        bbci     a2, 7, .Lsec_30_1e9
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a5, .Lsec_30_24
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_30_68
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_30_20
        slli     a8, a8, 14
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i     a10, a5, 0
.Lsec_30_1e9:
        bbci     a2, 8, .Lsec_30_215
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a5, .Lsec_30_28
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_30_6c
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_30_20
        slli     a8, a8, 14
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i     a10, a5, 0
.Lsec_30_215:
        bbci     a2, 9, .Lsec_30_23d
        movi     a4, 0xfffffeff
        nop.n
        nop.n
        nop.n
        l32r     a5, .Lsec_30_28
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_30_70
        callx8   a8
        extui    a8, a3, 0, 1
        slli     a8, a8, 8
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_30_23d:
        bbci     a2, 10, .Lsec_30_268
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a5, .Lsec_30_2c
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_30_74
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_30_8
        slli     a8, a8, 16
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_30_268:
        bbci     a2, 11, .Lsec_30_294
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_30_30
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_30_78
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_30_8
        slli     a8, a8, 16
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_30_294:
        bbci     a2, 12, .Lsec_30_2c0
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_30_34
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_30_7c
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_30_8
        slli     a8, a8, 16
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_30_2c0:
        bbci     a2, 13, .Lsec_30_2ec
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_30_38
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_30_80
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_30_8
        slli     a8, a8, 16
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_30_2ec:
        bbci     a2, 14, .Lsec_30_318
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_30_3c
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_30_84
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_30_8
        slli     a8, a8, 16
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_30_318:
        bbci     a2, 15, .Lsec_30_344
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_30_40
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_30_88
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_30_8
        slli     a8, a8, 16
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_30_344:
        bbci     a2, 16, .Lsec_30_370
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_30_44
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_30_8c
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_30_8
        slli     a8, a8, 16
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_30_370:
        bbci     a2, 17, .Lsec_30_396
        extui    a3, a3, 0, 1
        slli     a3, a3, 16
        nop.n
        nop.n
        l32r     a4, .Lsec_30_48
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_30_90
        callx8   a8
        l32r     a2, .Lsec_30_8
        and      a10, a10, a2
        or       a3, a3, a10
        memw
        s32i.n   a3, a4, 0
.Lsec_30_396:
        retw.n
        .size   rtc_pads_slpsel, .-rtc_pads_slpsel
.Lsec_30_398:

        .section .text.rtc_pads_slpoe,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_31_0, 0x3ff48484
        .literal .Lsec_31_4, 0xffffefff
        .literal .Lsec_31_8, 0x3ff48488
        .literal .Lsec_31_c, 0x3ff4848c
        .literal .Lsec_31_10, 0x3ff48494
        .literal .Lsec_31_14, 0xffffbfff
        .literal .Lsec_31_18, 0x3ff48498
        .literal .Lsec_31_1c, 0x3ff4849c
        .literal .Lsec_31_20, 0x3ff484a0
        .literal .Lsec_31_24, 0x3ff484a4
        .literal .Lsec_31_28, 0x3ff484a8
        .literal .Lsec_31_2c, 0x3ff484ac
        .literal .Lsec_31_30, 0x3ff484b0
        .literal .Lsec_31_34, esp_dport_access_reg_read
        .literal .Lsec_31_38, esp_dport_access_reg_read
        .literal .Lsec_31_3c, esp_dport_access_reg_read
        .literal .Lsec_31_40, esp_dport_access_reg_read
        .literal .Lsec_31_44, esp_dport_access_reg_read
        .literal .Lsec_31_48, esp_dport_access_reg_read
        .literal .Lsec_31_4c, esp_dport_access_reg_read
        .literal .Lsec_31_50, esp_dport_access_reg_read
        .literal .Lsec_31_54, esp_dport_access_reg_read
        .literal .Lsec_31_58, esp_dport_access_reg_read
        .literal .Lsec_31_5c, esp_dport_access_reg_read
        .literal .Lsec_31_60, esp_dport_access_reg_read
.Lsec_31_64:
        .global rtc_pads_slpoe
        .type   rtc_pads_slpoe, @function
rtc_pads_slpoe:
        entry    a1, 32
        bbci     a2, 6, .Lsec_31_89
        l32r     a5, .Lsec_31_0
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_31_34
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_31_4
        slli     a8, a8, 12
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_31_89:
        bbci     a2, 7, .Lsec_31_b5
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a5, .Lsec_31_8
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_31_38
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_31_4
        slli     a8, a8, 12
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i     a10, a5, 0
.Lsec_31_b5:
        bbci     a2, 8, .Lsec_31_e1
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a5, .Lsec_31_c
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_31_3c
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_31_4
        slli     a8, a8, 12
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i     a10, a5, 0
.Lsec_31_e1:
        bbci     a2, 9, .Lsec_31_109
        movi     a4, -65
        nop.n
        nop.n
        nop.n
        l32r     a5, .Lsec_31_c
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_31_40
        callx8   a8
        extui    a8, a3, 0, 1
        slli     a8, a8, 6
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_31_109:
        bbci     a2, 10, .Lsec_31_134
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a5, .Lsec_31_10
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_31_44
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_31_14
        slli     a8, a8, 14
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_31_134:
        bbci     a2, 11, .Lsec_31_160
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_31_18
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_31_48
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_31_14
        slli     a8, a8, 14
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_31_160:
        bbci     a2, 12, .Lsec_31_18c
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_31_1c
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_31_4c
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_31_14
        slli     a8, a8, 14
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_31_18c:
        bbci     a2, 13, .Lsec_31_1b8
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_31_20
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_31_50
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_31_14
        slli     a8, a8, 14
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_31_1b8:
        bbci     a2, 14, .Lsec_31_1e4
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_31_24
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_31_54
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_31_14
        slli     a8, a8, 14
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_31_1e4:
        bbci     a2, 15, .Lsec_31_210
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_31_28
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_31_58
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_31_14
        slli     a8, a8, 14
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_31_210:
        bbci     a2, 16, .Lsec_31_23c
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_31_2c
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_31_5c
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_31_14
        slli     a8, a8, 14
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_31_23c:
        bbci     a2, 17, .Lsec_31_262
        extui    a3, a3, 0, 1
        slli     a3, a3, 14
        nop.n
        nop.n
        l32r     a4, .Lsec_31_30
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_31_60
        callx8   a8
        l32r     a2, .Lsec_31_14
        and      a10, a10, a2
        or       a3, a3, a10
        memw
        s32i.n   a3, a4, 0
.Lsec_31_262:
        retw.n
        .size   rtc_pads_slpoe, .-rtc_pads_slpoe
.Lsec_31_264:

        .section .text.rtc_pads_slpie,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_32_0, 0x3ff4847c
        .literal .Lsec_32_4, 0xffefffff
        .literal .Lsec_32_8, 0xffff7fff
        .literal .Lsec_32_c, 0x3ff48480
        .literal .Lsec_32_10, 0xfeffffff
        .literal .Lsec_32_14, 0xfff7ffff
        .literal .Lsec_32_18, 0x3ff48484
        .literal .Lsec_32_1c, 0xffffdfff
        .literal .Lsec_32_20, 0x3ff48488
        .literal .Lsec_32_24, 0x3ff4848c
        .literal .Lsec_32_28, 0x3ff48494
        .literal .Lsec_32_2c, 0x3ff48498
        .literal .Lsec_32_30, 0x3ff4849c
        .literal .Lsec_32_34, 0x3ff484a0
        .literal .Lsec_32_38, 0x3ff484a4
        .literal .Lsec_32_3c, 0x3ff484a8
        .literal .Lsec_32_40, 0x3ff484ac
        .literal .Lsec_32_44, 0x3ff484b0
        .literal .Lsec_32_48, esp_dport_access_reg_read
        .literal .Lsec_32_4c, esp_dport_access_reg_read
        .literal .Lsec_32_50, esp_dport_access_reg_read
        .literal .Lsec_32_54, esp_dport_access_reg_read
        .literal .Lsec_32_58, esp_dport_access_reg_read
        .literal .Lsec_32_5c, esp_dport_access_reg_read
        .literal .Lsec_32_60, esp_dport_access_reg_read
        .literal .Lsec_32_64, esp_dport_access_reg_read
        .literal .Lsec_32_68, esp_dport_access_reg_read
        .literal .Lsec_32_6c, esp_dport_access_reg_read
        .literal .Lsec_32_70, esp_dport_access_reg_read
        .literal .Lsec_32_74, esp_dport_access_reg_read
        .literal .Lsec_32_78, esp_dport_access_reg_read
        .literal .Lsec_32_7c, esp_dport_access_reg_read
        .literal .Lsec_32_80, esp_dport_access_reg_read
        .literal .Lsec_32_84, esp_dport_access_reg_read
        .literal .Lsec_32_88, esp_dport_access_reg_read
        .literal .Lsec_32_8c, esp_dport_access_reg_read
.Lsec_32_90:
        .global rtc_pads_slpie
        .type   rtc_pads_slpie, @function
rtc_pads_slpie:
        entry    a1, 32
        bbci     a2, 0, .Lsec_32_b5
        l32r     a5, .Lsec_32_0
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_32_48
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_32_4
        slli     a8, a8, 20
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_32_b5:
        bbci     a2, 1, .Lsec_32_e1
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a5, .Lsec_32_0
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_32_4c
        callx8   a8
        slli     a8, a3, 15
        l32r     a4, .Lsec_32_8
        extui    a8, a8, 0, 16
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i     a10, a5, 0
.Lsec_32_e1:
        bbci     a2, 2, .Lsec_32_109
        movi     a4, 0xfffffbff
        nop.n
        nop.n
        nop.n
        l32r     a5, .Lsec_32_0
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_32_50
        callx8   a8
        extui    a8, a3, 0, 1
        slli     a8, a8, 10
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_32_109:
        bbci     a2, 3, .Lsec_32_131
        movi     a4, -33
        nop.n
        nop.n
        nop.n
        l32r     a5, .Lsec_32_0
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_32_54
        callx8   a8
        extui    a8, a3, 0, 1
        slli     a8, a8, 5
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_32_131:
        bbci     a2, 4, .Lsec_32_15d
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a5, .Lsec_32_c
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_32_58
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_32_10
        slli     a8, a8, 24
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i     a10, a5, 0
.Lsec_32_15d:
        bbci     a2, 5, .Lsec_32_189
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a5, .Lsec_32_c
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_32_5c
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_32_14
        slli     a8, a8, 19
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i     a10, a5, 0
.Lsec_32_189:
        bbci     a2, 6, .Lsec_32_1b5
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a5, .Lsec_32_18
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_32_60
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_32_1c
        slli     a8, a8, 13
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i     a10, a5, 0
.Lsec_32_1b5:
        bbci     a2, 7, .Lsec_32_1e1
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a5, .Lsec_32_20
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_32_64
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_32_1c
        slli     a8, a8, 13
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i     a10, a5, 0
.Lsec_32_1e1:
        bbci     a2, 8, .Lsec_32_20d
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a5, .Lsec_32_24
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_32_68
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_32_1c
        slli     a8, a8, 13
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i     a10, a5, 0
.Lsec_32_20d:
        bbci     a2, 9, .Lsec_32_235
        movi     a4, -129
        nop.n
        nop.n
        nop.n
        l32r     a5, .Lsec_32_24
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_32_6c
        callx8   a8
        slli     a8, a3, 7
        extui    a8, a8, 0, 8
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_32_235:
        bbci     a2, 10, .Lsec_32_260
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a5, .Lsec_32_28
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_32_70
        callx8   a8
        slli     a8, a3, 15
        l32r     a4, .Lsec_32_8
        extui    a8, a8, 0, 16
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_32_260:
        bbci     a2, 11, .Lsec_32_28c
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_32_2c
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_32_74
        callx8   a8
        slli     a8, a3, 15
        l32r     a4, .Lsec_32_8
        extui    a8, a8, 0, 16
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_32_28c:
        bbci     a2, 12, .Lsec_32_2b8
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_32_30
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_32_78
        callx8   a8
        slli     a8, a3, 15
        l32r     a4, .Lsec_32_8
        extui    a8, a8, 0, 16
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_32_2b8:
        bbci     a2, 13, .Lsec_32_2e4
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_32_34
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_32_7c
        callx8   a8
        slli     a8, a3, 15
        l32r     a4, .Lsec_32_8
        extui    a8, a8, 0, 16
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_32_2e4:
        bbci     a2, 14, .Lsec_32_310
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_32_38
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_32_80
        callx8   a8
        slli     a8, a3, 15
        l32r     a4, .Lsec_32_8
        extui    a8, a8, 0, 16
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_32_310:
        bbci     a2, 15, .Lsec_32_33c
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_32_3c
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_32_84
        callx8   a8
        slli     a8, a3, 15
        l32r     a4, .Lsec_32_8
        extui    a8, a8, 0, 16
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_32_33c:
        bbci     a2, 16, .Lsec_32_368
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_32_40
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_32_88
        callx8   a8
        slli     a8, a3, 15
        l32r     a4, .Lsec_32_8
        extui    a8, a8, 0, 16
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_32_368:
        bbci     a2, 17, .Lsec_32_38e
        slli     a3, a3, 15
        extui    a3, a3, 0, 16
        nop.n
        nop.n
        l32r     a4, .Lsec_32_44
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_32_8c
        callx8   a8
        l32r     a2, .Lsec_32_8
        and      a10, a10, a2
        or       a3, a3, a10
        memw
        s32i.n   a3, a4, 0
.Lsec_32_38e:
        retw.n
        .size   rtc_pads_slpie, .-rtc_pads_slpie
.Lsec_32_390:

        .section .text.rtc_pads_funie,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_33_0, 0x3ff4847c
        .literal .Lsec_33_4, 0xfff7ffff
        .literal .Lsec_33_8, 0xffffbfff
        .literal .Lsec_33_c, 0x3ff48480
        .literal .Lsec_33_10, 0xff7fffff
        .literal .Lsec_33_14, 0xfffbffff
        .literal .Lsec_33_18, 0x3ff48484
        .literal .Lsec_33_1c, 0xfffff7ff
        .literal .Lsec_33_20, 0x3ff48488
        .literal .Lsec_33_24, 0x3ff4848c
        .literal .Lsec_33_28, 0x3ff48494
        .literal .Lsec_33_2c, 0xffffdfff
        .literal .Lsec_33_30, 0x3ff48498
        .literal .Lsec_33_34, 0x3ff4849c
        .literal .Lsec_33_38, 0x3ff484a0
        .literal .Lsec_33_3c, 0x3ff484a4
        .literal .Lsec_33_40, 0x3ff484a8
        .literal .Lsec_33_44, 0x3ff484ac
        .literal .Lsec_33_48, 0x3ff484b0
        .literal .Lsec_33_4c, esp_dport_access_reg_read
        .literal .Lsec_33_50, esp_dport_access_reg_read
        .literal .Lsec_33_54, esp_dport_access_reg_read
        .literal .Lsec_33_58, esp_dport_access_reg_read
        .literal .Lsec_33_5c, esp_dport_access_reg_read
        .literal .Lsec_33_60, esp_dport_access_reg_read
        .literal .Lsec_33_64, esp_dport_access_reg_read
        .literal .Lsec_33_68, esp_dport_access_reg_read
        .literal .Lsec_33_6c, esp_dport_access_reg_read
        .literal .Lsec_33_70, esp_dport_access_reg_read
        .literal .Lsec_33_74, esp_dport_access_reg_read
        .literal .Lsec_33_78, esp_dport_access_reg_read
        .literal .Lsec_33_7c, esp_dport_access_reg_read
        .literal .Lsec_33_80, esp_dport_access_reg_read
        .literal .Lsec_33_84, esp_dport_access_reg_read
        .literal .Lsec_33_88, esp_dport_access_reg_read
        .literal .Lsec_33_8c, esp_dport_access_reg_read
        .literal .Lsec_33_90, esp_dport_access_reg_read
.Lsec_33_94:
        .global rtc_pads_funie
        .type   rtc_pads_funie, @function
rtc_pads_funie:
        entry    a1, 32
        bbci     a2, 0, .Lsec_33_b9
        l32r     a5, .Lsec_33_0
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_33_4c
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_33_4
        slli     a8, a8, 19
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_33_b9:
        bbci     a2, 1, .Lsec_33_e5
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a5, .Lsec_33_0
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_33_50
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_33_8
        slli     a8, a8, 14
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i     a10, a5, 0
.Lsec_33_e5:
        bbci     a2, 2, .Lsec_33_10d
        movi     a4, 0xfffffdff
        nop.n
        nop.n
        nop.n
        l32r     a5, .Lsec_33_0
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_33_54
        callx8   a8
        extui    a8, a3, 0, 1
        slli     a8, a8, 9
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_33_10d:
        bbci     a2, 3, .Lsec_33_135
        movi.n   a4, -17
        nop.n
        nop.n
        nop.n
        l32r     a5, .Lsec_33_0
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_33_58
        callx8   a8
        extui    a8, a3, 0, 1
        slli     a8, a8, 4
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_33_135:
        bbci     a2, 4, .Lsec_33_161
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a5, .Lsec_33_c
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_33_5c
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_33_10
        slli     a8, a8, 23
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i     a10, a5, 0
.Lsec_33_161:
        bbci     a2, 5, .Lsec_33_18d
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a5, .Lsec_33_c
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_33_60
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_33_14
        slli     a8, a8, 18
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i     a10, a5, 0
.Lsec_33_18d:
        bbci     a2, 6, .Lsec_33_1b9
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a5, .Lsec_33_18
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_33_64
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_33_1c
        slli     a8, a8, 11
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i     a10, a5, 0
.Lsec_33_1b9:
        bbci     a2, 7, .Lsec_33_1e5
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a5, .Lsec_33_20
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_33_68
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_33_1c
        slli     a8, a8, 11
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i     a10, a5, 0
.Lsec_33_1e5:
        bbci     a2, 8, .Lsec_33_211
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a5, .Lsec_33_24
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_33_6c
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_33_1c
        slli     a8, a8, 11
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i     a10, a5, 0
.Lsec_33_211:
        bbci     a2, 9, .Lsec_33_239
        movi     a4, -33
        nop.n
        nop.n
        nop.n
        l32r     a5, .Lsec_33_24
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_33_70
        callx8   a8
        extui    a8, a3, 0, 1
        slli     a8, a8, 5
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_33_239:
        bbci     a2, 10, .Lsec_33_264
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a5, .Lsec_33_28
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_33_74
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_33_2c
        slli     a8, a8, 13
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_33_264:
        bbci     a2, 11, .Lsec_33_290
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_33_30
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_33_78
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_33_2c
        slli     a8, a8, 13
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_33_290:
        bbci     a2, 12, .Lsec_33_2bc
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_33_34
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_33_7c
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_33_2c
        slli     a8, a8, 13
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_33_2bc:
        bbci     a2, 13, .Lsec_33_2e8
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_33_38
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_33_80
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_33_2c
        slli     a8, a8, 13
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_33_2e8:
        bbci     a2, 14, .Lsec_33_314
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_33_3c
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_33_84
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_33_2c
        slli     a8, a8, 13
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_33_314:
        bbci     a2, 15, .Lsec_33_340
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_33_40
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_33_88
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_33_2c
        slli     a8, a8, 13
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_33_340:
        bbci     a2, 16, .Lsec_33_36c
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_33_44
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_33_8c
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_33_2c
        slli     a8, a8, 13
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_33_36c:
        bbci     a2, 17, .Lsec_33_392
        extui    a3, a3, 0, 1
        slli     a3, a3, 13
        nop.n
        nop.n
        l32r     a4, .Lsec_33_48
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_33_90
        callx8   a8
        l32r     a2, .Lsec_33_2c
        and      a10, a10, a2
        or       a3, a3, a10
        memw
        s32i.n   a3, a4, 0
.Lsec_33_392:
        retw.n
        .size   rtc_pads_funie, .-rtc_pads_funie
.Lsec_33_394:

        .section .text.rtc_pads_pu,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_34_0, 0x3ff48484
        .literal .Lsec_34_4, 0xf7ffffff
        .literal .Lsec_34_8, 0x3ff48488
        .literal .Lsec_34_c, 0x3ff4848c
        .literal .Lsec_34_10, 0xffbfffff
        .literal .Lsec_34_14, 0x3ff48494
        .literal .Lsec_34_18, 0x3ff48498
        .literal .Lsec_34_1c, 0x3ff4849c
        .literal .Lsec_34_20, 0x3ff484a0
        .literal .Lsec_34_24, 0x3ff484a4
        .literal .Lsec_34_28, 0x3ff484a8
        .literal .Lsec_34_2c, 0x3ff484ac
        .literal .Lsec_34_30, 0x3ff484b0
        .literal .Lsec_34_34, esp_dport_access_reg_read
        .literal .Lsec_34_38, esp_dport_access_reg_read
        .literal .Lsec_34_3c, esp_dport_access_reg_read
        .literal .Lsec_34_40, esp_dport_access_reg_read
        .literal .Lsec_34_44, esp_dport_access_reg_read
        .literal .Lsec_34_48, esp_dport_access_reg_read
        .literal .Lsec_34_4c, esp_dport_access_reg_read
        .literal .Lsec_34_50, esp_dport_access_reg_read
        .literal .Lsec_34_54, esp_dport_access_reg_read
        .literal .Lsec_34_58, esp_dport_access_reg_read
        .literal .Lsec_34_5c, esp_dport_access_reg_read
        .literal .Lsec_34_60, esp_dport_access_reg_read
.Lsec_34_64:
        .global rtc_pads_pu
        .type   rtc_pads_pu, @function
rtc_pads_pu:
        entry    a1, 32
        bbci     a2, 6, .Lsec_34_89
        l32r     a5, .Lsec_34_0
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_34_34
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_34_4
        slli     a8, a8, 27
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_34_89:
        bbci     a2, 7, .Lsec_34_b4
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a5, .Lsec_34_8
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_34_38
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_34_4
        slli     a8, a8, 27
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_34_b4:
        bbci     a2, 8, .Lsec_34_e0
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_34_c
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_34_3c
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_34_4
        slli     a8, a8, 27
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_34_e0:
        bbci     a2, 9, .Lsec_34_10c
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_34_c
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_34_40
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_34_10
        slli     a8, a8, 22
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_34_10c:
        bbci     a2, 10, .Lsec_34_138
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_34_14
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_34_44
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_34_4
        slli     a8, a8, 27
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_34_138:
        bbci     a2, 11, .Lsec_34_164
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_34_18
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_34_48
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_34_4
        slli     a8, a8, 27
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_34_164:
        bbci     a2, 12, .Lsec_34_190
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_34_1c
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_34_4c
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_34_4
        slli     a8, a8, 27
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_34_190:
        bbci     a2, 13, .Lsec_34_1bc
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_34_20
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_34_50
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_34_4
        slli     a8, a8, 27
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_34_1bc:
        bbci     a2, 14, .Lsec_34_1e8
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_34_24
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_34_54
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_34_4
        slli     a8, a8, 27
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_34_1e8:
        bbci     a2, 15, .Lsec_34_214
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_34_28
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_34_58
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_34_4
        slli     a8, a8, 27
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_34_214:
        bbci     a2, 16, .Lsec_34_240
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_34_2c
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_34_5c
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_34_4
        slli     a8, a8, 27
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_34_240:
        bbci     a2, 17, .Lsec_34_266
        extui    a3, a3, 0, 1
        slli     a3, a3, 27
        nop.n
        nop.n
        l32r     a4, .Lsec_34_30
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_34_60
        callx8   a8
        l32r     a2, .Lsec_34_4
        and      a10, a10, a2
        or       a3, a3, a10
        memw
        s32i.n   a3, a4, 0
.Lsec_34_266:
        retw.n
        .size   rtc_pads_pu, .-rtc_pads_pu
.Lsec_34_268:

        .section .text.rtc_pads_pd,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_35_0, 0x3ff48484
        .literal .Lsec_35_4, 0xefffffff
        .literal .Lsec_35_8, 0x3ff48488
        .literal .Lsec_35_c, 0x3ff4848c
        .literal .Lsec_35_10, 0xff7fffff
        .literal .Lsec_35_14, 0x3ff48494
        .literal .Lsec_35_18, 0x3ff48498
        .literal .Lsec_35_1c, 0x3ff4849c
        .literal .Lsec_35_20, 0x3ff484a0
        .literal .Lsec_35_24, 0x3ff484a4
        .literal .Lsec_35_28, 0x3ff484a8
        .literal .Lsec_35_2c, 0x3ff484ac
        .literal .Lsec_35_30, 0x3ff484b0
        .literal .Lsec_35_34, esp_dport_access_reg_read
        .literal .Lsec_35_38, esp_dport_access_reg_read
        .literal .Lsec_35_3c, esp_dport_access_reg_read
        .literal .Lsec_35_40, esp_dport_access_reg_read
        .literal .Lsec_35_44, esp_dport_access_reg_read
        .literal .Lsec_35_48, esp_dport_access_reg_read
        .literal .Lsec_35_4c, esp_dport_access_reg_read
        .literal .Lsec_35_50, esp_dport_access_reg_read
        .literal .Lsec_35_54, esp_dport_access_reg_read
        .literal .Lsec_35_58, esp_dport_access_reg_read
        .literal .Lsec_35_5c, esp_dport_access_reg_read
        .literal .Lsec_35_60, esp_dport_access_reg_read
.Lsec_35_64:
        .global rtc_pads_pd
        .type   rtc_pads_pd, @function
rtc_pads_pd:
        entry    a1, 32
        bbci     a2, 6, .Lsec_35_89
        l32r     a5, .Lsec_35_0
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_35_34
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_35_4
        slli     a8, a8, 28
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_35_89:
        bbci     a2, 7, .Lsec_35_b4
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a5, .Lsec_35_8
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_35_38
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_35_4
        slli     a8, a8, 28
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_35_b4:
        bbci     a2, 8, .Lsec_35_e0
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_35_c
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_35_3c
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_35_4
        slli     a8, a8, 28
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_35_e0:
        bbci     a2, 9, .Lsec_35_10c
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_35_c
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_35_40
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_35_10
        slli     a8, a8, 23
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_35_10c:
        bbci     a2, 10, .Lsec_35_138
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_35_14
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_35_44
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_35_4
        slli     a8, a8, 28
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_35_138:
        bbci     a2, 11, .Lsec_35_164
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_35_18
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_35_48
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_35_4
        slli     a8, a8, 28
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_35_164:
        bbci     a2, 12, .Lsec_35_190
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_35_1c
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_35_4c
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_35_4
        slli     a8, a8, 28
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_35_190:
        bbci     a2, 13, .Lsec_35_1bc
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_35_20
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_35_50
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_35_4
        slli     a8, a8, 28
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_35_1bc:
        bbci     a2, 14, .Lsec_35_1e8
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_35_24
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_35_54
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_35_4
        slli     a8, a8, 28
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_35_1e8:
        bbci     a2, 15, .Lsec_35_214
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_35_28
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_35_58
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_35_4
        slli     a8, a8, 28
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_35_214:
        bbci     a2, 16, .Lsec_35_240
        nop.n
        nop.n
        nop.n
        nop
        l32r     a5, .Lsec_35_2c
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_35_5c
        callx8   a8
        extui    a8, a3, 0, 1
        l32r     a4, .Lsec_35_4
        slli     a8, a8, 28
        and      a10, a10, a4
        or       a10, a8, a10
        memw
        s32i.n   a10, a5, 0
.Lsec_35_240:
        bbci     a2, 17, .Lsec_35_266
        extui    a3, a3, 0, 1
        slli     a3, a3, 28
        nop.n
        nop.n
        l32r     a4, .Lsec_35_30
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_35_60
        callx8   a8
        l32r     a2, .Lsec_35_4
        and      a10, a10, a2
        or       a3, a3, a10
        memw
        s32i.n   a3, a4, 0
.Lsec_35_266:
        retw.n
        .size   rtc_pads_pd, .-rtc_pads_pd
.Lsec_35_268:

        .section .text.rtc_pads_hold,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_36_0, 0x3ff4847c
        .literal .Lsec_36_4, 0x7fffffff
        .literal .Lsec_36_8, 0xbfffffff
        .literal .Lsec_36_c, 0xdfffffff
        .literal .Lsec_36_10, 0xefffffff
        .literal .Lsec_36_14, 0x3ff48480
        .literal .Lsec_36_18, 0x3ff48484
        .literal .Lsec_36_1c, 0x3ff48488
        .literal .Lsec_36_20, 0x3ff4848c
        .literal .Lsec_36_24, 0xfeffffff
        .literal .Lsec_36_28, 0x3ff48494
        .literal .Lsec_36_2c, 0x3ff48498
        .literal .Lsec_36_30, 0x3ff4849c
        .literal .Lsec_36_34, 0x3ff484a0
        .literal .Lsec_36_38, 0x3ff484a4
        .literal .Lsec_36_3c, 0x3ff484a8
        .literal .Lsec_36_40, 0x3ff484ac
        .literal .Lsec_36_44, 0x3ff484b0
        .literal .Lsec_36_48, esp_dport_access_reg_read
        .literal .Lsec_36_4c, esp_dport_access_reg_read
        .literal .Lsec_36_50, esp_dport_access_reg_read
        .literal .Lsec_36_54, esp_dport_access_reg_read
        .literal .Lsec_36_58, esp_dport_access_reg_read
        .literal .Lsec_36_5c, esp_dport_access_reg_read
        .literal .Lsec_36_60, esp_dport_access_reg_read
        .literal .Lsec_36_64, esp_dport_access_reg_read
        .literal .Lsec_36_68, esp_dport_access_reg_read
        .literal .Lsec_36_6c, esp_dport_access_reg_read
        .literal .Lsec_36_70, esp_dport_access_reg_read
        .literal .Lsec_36_74, esp_dport_access_reg_read
        .literal .Lsec_36_78, esp_dport_access_reg_read
        .literal .Lsec_36_7c, esp_dport_access_reg_read
        .literal .Lsec_36_80, esp_dport_access_reg_read
        .literal .Lsec_36_84, esp_dport_access_reg_read
        .literal .Lsec_36_88, esp_dport_access_reg_read
        .literal .Lsec_36_8c, esp_dport_access_reg_read
.Lsec_36_90:
        .global rtc_pads_hold
        .type   rtc_pads_hold, @function
rtc_pads_hold:
        entry    a1, 32
        bbci     a2, 0, .Lsec_36_b2
        l32r     a4, .Lsec_36_0
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_36_48
        callx8   a8
        slli     a8, a3, 31
        l32r     a9, .Lsec_36_4
        and      a10, a10, a9
        or       a8, a10, a8
        memw
        s32i.n   a8, a4, 0
.Lsec_36_b2:
        bbci     a2, 1, .Lsec_36_dc
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a4, .Lsec_36_0
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_36_4c
        callx8   a8
        extui    a9, a3, 0, 1
        l32r     a8, .Lsec_36_8
        slli     a9, a9, 30
        and      a10, a10, a8
        or       a10, a9, a10
        memw
        s32i.n   a10, a4, 0
.Lsec_36_dc:
        bbci     a2, 2, .Lsec_36_108
        nop.n
        nop.n
        nop
        nop.n
        l32r     a4, .Lsec_36_0
        or       a10, a4, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_36_50
        callx8   a8
        extui    a9, a3, 0, 1
        l32r     a8, .Lsec_36_c
        slli     a9, a9, 29
        and      a10, a10, a8
        or       a10, a9, a10
        memw
        s32i.n   a10, a4, 0
.Lsec_36_108:
        bbci     a2, 3, .Lsec_36_134
        nop.n
        nop.n
        nop
        nop.n
        l32r     a4, .Lsec_36_0
        or       a10, a4, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_36_54
        callx8   a8
        extui    a9, a3, 0, 1
        l32r     a8, .Lsec_36_10
        slli     a9, a9, 28
        and      a10, a10, a8
        or       a10, a9, a10
        memw
        s32i.n   a10, a4, 0
.Lsec_36_134:
        bbci     a2, 4, .Lsec_36_15d
        nop.n
        nop.n
        nop
        nop.n
        l32r     a4, .Lsec_36_14
        or       a10, a4, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_36_58
        callx8   a8
        slli     a8, a3, 31
        l32r     a9, .Lsec_36_4
        and      a10, a10, a9
        or       a8, a10, a8
        memw
        s32i.n   a8, a4, 0
.Lsec_36_15d:
        bbci     a2, 5, .Lsec_36_188
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a4, .Lsec_36_14
        or       a10, a4, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_36_5c
        callx8   a8
        extui    a9, a3, 0, 1
        l32r     a8, .Lsec_36_8
        slli     a9, a9, 30
        and      a10, a10, a8
        or       a10, a9, a10
        memw
        s32i.n   a10, a4, 0
.Lsec_36_188:
        bbci     a2, 6, .Lsec_36_1b4
        nop.n
        nop.n
        nop
        nop.n
        l32r     a4, .Lsec_36_18
        or       a10, a4, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_36_60
        callx8   a8
        extui    a9, a3, 0, 1
        l32r     a8, .Lsec_36_c
        slli     a9, a9, 29
        and      a10, a10, a8
        or       a10, a9, a10
        memw
        s32i.n   a10, a4, 0
.Lsec_36_1b4:
        bbci     a2, 7, .Lsec_36_1e0
        nop.n
        nop.n
        nop
        nop.n
        l32r     a4, .Lsec_36_1c
        or       a10, a4, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_36_64
        callx8   a8
        extui    a9, a3, 0, 1
        l32r     a8, .Lsec_36_c
        slli     a9, a9, 29
        and      a10, a10, a8
        or       a10, a9, a10
        memw
        s32i.n   a10, a4, 0
.Lsec_36_1e0:
        bbci     a2, 8, .Lsec_36_20c
        nop.n
        nop.n
        nop
        nop.n
        l32r     a4, .Lsec_36_20
        or       a10, a4, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_36_68
        callx8   a8
        extui    a9, a3, 0, 1
        l32r     a8, .Lsec_36_c
        slli     a9, a9, 29
        and      a10, a10, a8
        or       a10, a9, a10
        memw
        s32i.n   a10, a4, 0
.Lsec_36_20c:
        bbci     a2, 9, .Lsec_36_238
        nop.n
        nop.n
        nop
        nop.n
        l32r     a4, .Lsec_36_20
        or       a10, a4, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_36_6c
        callx8   a8
        extui    a9, a3, 0, 1
        l32r     a8, .Lsec_36_24
        slli     a9, a9, 24
        and      a10, a10, a8
        or       a10, a9, a10
        memw
        s32i.n   a10, a4, 0
.Lsec_36_238:
        bbci     a2, 10, .Lsec_36_261
        nop.n
        nop.n
        nop
        nop.n
        l32r     a4, .Lsec_36_28
        or       a10, a4, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_36_70
        callx8   a8
        slli     a8, a3, 31
        l32r     a9, .Lsec_36_4
        and      a10, a10, a9
        or       a8, a10, a8
        memw
        s32i.n   a8, a4, 0
.Lsec_36_261:
        bbci     a2, 11, .Lsec_36_289
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a4, .Lsec_36_2c
        or       a10, a4, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_36_74
        callx8   a8
        slli     a8, a3, 31
        l32r     a9, .Lsec_36_4
        and      a10, a10, a9
        or       a8, a10, a8
        memw
        s32i.n   a8, a4, 0
.Lsec_36_289:
        bbci     a2, 12, .Lsec_36_2b1
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a4, .Lsec_36_30
        or       a10, a4, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_36_78
        callx8   a8
        slli     a8, a3, 31
        l32r     a9, .Lsec_36_4
        and      a10, a10, a9
        or       a8, a10, a8
        memw
        s32i.n   a8, a4, 0
.Lsec_36_2b1:
        bbci     a2, 13, .Lsec_36_2d9
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a4, .Lsec_36_34
        or       a10, a4, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_36_7c
        callx8   a8
        slli     a8, a3, 31
        l32r     a9, .Lsec_36_4
        and      a10, a10, a9
        or       a8, a10, a8
        memw
        s32i.n   a8, a4, 0
.Lsec_36_2d9:
        bbci     a2, 14, .Lsec_36_301
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a4, .Lsec_36_38
        or       a10, a4, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_36_80
        callx8   a8
        slli     a8, a3, 31
        l32r     a9, .Lsec_36_4
        and      a10, a10, a9
        or       a8, a10, a8
        memw
        s32i.n   a8, a4, 0
.Lsec_36_301:
        bbci     a2, 15, .Lsec_36_329
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a4, .Lsec_36_3c
        or       a10, a4, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_36_84
        callx8   a8
        slli     a8, a3, 31
        l32r     a9, .Lsec_36_4
        and      a10, a10, a9
        or       a8, a10, a8
        memw
        s32i.n   a8, a4, 0
.Lsec_36_329:
        bbci     a2, 16, .Lsec_36_351
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a4, .Lsec_36_40
        or       a10, a4, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_36_88
        callx8   a8
        slli     a8, a3, 31
        l32r     a9, .Lsec_36_4
        and      a10, a10, a9
        or       a8, a10, a8
        memw
        s32i.n   a8, a4, 0
.Lsec_36_351:
        bbci     a2, 17, .Lsec_36_376
        slli     a3, a3, 31
        nop.n
        nop.n
        nop.n
        l32r     a4, .Lsec_36_44
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_36_8c
        callx8   a8
        l32r     a2, .Lsec_36_4
        and      a10, a10, a2
        or       a3, a10, a3
        memw
        s32i.n   a3, a4, 0
.Lsec_36_376:
        retw.n
        .size   rtc_pads_hold, .-rtc_pads_hold
.Lsec_36_378:

        .section .text.rtc_apbbridge_sel,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_37_0, 0x3ff48018
        .literal .Lsec_37_4, 0xffbfffff
        .literal .Lsec_37_8, esp_dport_access_reg_read
.Lsec_37_c:
        .global rtc_apbbridge_sel
        .type   rtc_apbbridge_sel, @function
rtc_apbbridge_sel:
        entry    a1, 32
        l32r     a3, .Lsec_37_0
        extui    a2, a2, 0, 1
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_37_8
        callx8   a8
        slli     a2, a2, 22
        l32r     a8, .Lsec_37_4
        and      a10, a10, a8
        or       a10, a2, a10
        memw
        s32i.n   a10, a3, 0
        retw.n
        .size   rtc_apbbridge_sel, .-rtc_apbbridge_sel
.Lsec_37_30:

        .section .text.rtc_powerup_rf,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_38_0, 0x3ff48030
        .literal .Lsec_38_4, 0xd8000000
        .literal .Lsec_38_8, esp_dport_access_reg_read
.Lsec_38_c:
        .global rtc_powerup_rf
        .type   rtc_powerup_rf, @function
rtc_powerup_rf:
        entry    a1, 32
        l32r     a2, .Lsec_38_0
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_38_8
        callx8   a8
        l32r     a8, .Lsec_38_4
        or       a10, a10, a8
        memw
        s32i.n   a10, a2, 0
        retw.n
        .size   rtc_powerup_rf, .-rtc_powerup_rf
.Lsec_38_27:

        .section .text.rtc_powerdown_rf,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_39_0, 0x3ff48030
        .literal .Lsec_39_4, 0x27ffffff
        .literal .Lsec_39_8, esp_dport_access_reg_read
.Lsec_39_c:
        .global rtc_powerdown_rf
        .type   rtc_powerdown_rf, @function
rtc_powerdown_rf:
        entry    a1, 32
        l32r     a2, .Lsec_39_0
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_39_8
        callx8   a8
        l32r     a8, .Lsec_39_4
        and      a10, a10, a8
        memw
        s32i.n   a10, a2, 0
        retw.n
        .size   rtc_powerdown_rf, .-rtc_powerdown_rf
.Lsec_39_27:

        .section .text.rtc_get_st,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_40_0, 0x3ff480c0
        .literal .Lsec_40_4, esp_dport_access_reg_read
.Lsec_40_8:
        .global rtc_get_st
        .type   rtc_get_st, @function
rtc_get_st:
        entry    a1, 32
        l32r     a10, .Lsec_40_0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_40_4
        callx8   a8
        extui    a2, a10, 28, 4
        retw.n
        .size   rtc_get_st, .-rtc_get_st
.Lsec_40_19:

        .section .text.rtc_is_st_idle,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_41_0, 0x3ff480c0
        .literal .Lsec_41_4, 0x08000000
        .literal .Lsec_41_8, esp_dport_access_reg_read
.Lsec_41_c:
        .global rtc_is_st_idle
        .type   rtc_is_st_idle, @function
rtc_is_st_idle:
        entry    a1, 32
        l32r     a10, .Lsec_41_0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_41_8
        callx8   a8
        l32r     a2, .Lsec_41_4
        and      a2, a10, a2
        extui    a2, a2, 27, 5
        retw.n
        .size   rtc_is_st_idle, .-rtc_is_st_idle
.Lsec_41_23:

        .section .text.rtc_soc_clk_ck12m,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_42_0, 0x3ff48070
        .literal .Lsec_42_4, 0xe7ffffff
        .literal .Lsec_42_8, 0x10000000
        .literal .Lsec_42_c, 0x3ff48000
        .literal .Lsec_42_10, 0xffffc0ff
        .literal .Lsec_42_14, 0x00001500
        .literal .Lsec_42_18, esp_dport_access_reg_read
        .literal .Lsec_42_1c, esp_dport_access_reg_read
.Lsec_42_20:
        .global rtc_soc_clk_ck12m
        .type   rtc_soc_clk_ck12m, @function
rtc_soc_clk_ck12m:
        entry    a1, 32
        l32r     a3, .Lsec_42_0
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_42_18
        callx8   a8
        l32r     a8, .Lsec_42_4
        and      a10, a10, a8
        l32r     a8, .Lsec_42_8
        or       a8, a10, a8
        l32r     a2, .Lsec_42_c
        or       a10, a2, a2
        memw
        s32i     a8, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_42_1c
        callx8   a8
        l32r     a8, .Lsec_42_10
        and      a10, a10, a8
        l32r     a8, .Lsec_42_14
        or       a10, a10, a8
        memw
        s32i.n   a10, a2, 0
        retw.n
        .size   rtc_soc_clk_ck12m, .-rtc_soc_clk_ck12m
.Lsec_42_5f:

        .section .text.rtc_init_full,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_43_0, 0x3ff4808c
        .literal .Lsec_43_4, 0x3ff4801c
        .literal .Lsec_43_8, 0x00ffffff
        .literal .Lsec_43_c, 0xff003fff
        .literal .Lsec_43_10, 0xffffc03f
        .literal .Lsec_43_14, 0x3ff48070
        .literal .Lsec_43_18, 0x04000000
        .literal .Lsec_43_1c, 0xfbffffff
        .literal .Lsec_43_20, 0x3ff48000
        .literal .Lsec_43_24, 0x00002000
        .literal .Lsec_43_28, 0xffffdfff
        .literal .Lsec_43_2c, 0x00400000
        .literal .Lsec_43_30, 0xffbfffff
        .literal .Lsec_43_34, 0x00080000
        .literal .Lsec_43_38, 0xfff7ffff
        .literal .Lsec_43_3c, 0x00010000
        .literal .Lsec_43_40, 0xfffeffff
        .literal .Lsec_43_44, 0xffefffff
        .literal .Lsec_43_48, 0xfffdffff
        .literal .Lsec_43_4c, 0xffffbfff
        .literal .Lsec_43_50, 0x00000800
        .literal .Lsec_43_54, 0xfffff7ff
        .literal .Lsec_43_58, 0x3ff48084
        .literal .Lsec_43_5c, 0xfffbffff
        .literal .Lsec_43_60, 0xfffeaabf
        .literal .Lsec_43_64, 0x3ff48080
        .literal .Lsec_43_68, 0xfffedfff
        .literal .Lsec_43_6c, 0x3ff48088
        .literal .Lsec_43_70, 0x7fffffff
        .literal .Lsec_43_74, 0xdfffffff
        .literal .Lsec_43_78, 0xf555ffff
        .literal .Lsec_43_7c, 0xffffefff
        .literal .Lsec_43_80, 0x3ff48078
        .literal .Lsec_43_84, 0x40000000
        .literal .Lsec_43_88, 0xbfffffff
        .literal .Lsec_43_8c, 0x20000000
        .literal .Lsec_43_90, 0x3ff4807c
        .literal .Lsec_43_94, 0xfe3fffff
        .literal .Lsec_43_98, 0xf1ffffff
        .literal .Lsec_43_9c, 0xffffc7ff
        .literal .Lsec_43_a0, esp_dport_access_reg_read
        .literal .Lsec_43_a4, esp_dport_access_reg_read
        .literal .Lsec_43_a8, esp_dport_access_reg_read
        .literal .Lsec_43_ac, esp_dport_access_reg_read
        .literal .Lsec_43_b0, esp_dport_access_reg_read
        .literal .Lsec_43_b4, esp_dport_access_reg_read
        .literal .Lsec_43_b8, esp_dport_access_reg_read
        .literal .Lsec_43_bc, esp_dport_access_reg_read
        .literal .Lsec_43_c0, esp_dport_access_reg_read
        .literal .Lsec_43_c4, esp_dport_access_reg_read
        .literal .Lsec_43_c8, esp_dport_access_reg_read
        .literal .Lsec_43_cc, esp_dport_access_reg_read
        .literal .Lsec_43_d0, esp_dport_access_reg_read
        .literal .Lsec_43_d4, esp_dport_access_reg_read
        .literal .Lsec_43_d8, esp_dport_access_reg_read
        .literal .Lsec_43_dc, esp_dport_access_reg_read
        .literal .Lsec_43_e0, esp_dport_access_reg_read
        .literal .Lsec_43_e4, esp_dport_access_reg_read
        .literal .Lsec_43_e8, esp_dport_access_reg_read
        .literal .Lsec_43_ec, esp_dport_access_reg_read
        .literal .Lsec_43_f0, esp_dport_access_reg_read
        .literal .Lsec_43_f4, esp_dport_access_reg_read
        .literal .Lsec_43_f8, esp_dport_access_reg_read
        .literal .Lsec_43_fc, esp_dport_access_reg_read
        .literal .Lsec_43_100, esp_dport_access_reg_read
        .literal .Lsec_43_104, esp_dport_access_reg_read
        .literal .Lsec_43_108, esp_dport_access_reg_read
        .literal .Lsec_43_10c, esp_dport_access_reg_read
        .literal .Lsec_43_110, esp_dport_access_reg_read
        .literal .Lsec_43_114, esp_dport_access_reg_read
        .literal .Lsec_43_118, esp_dport_access_reg_read
        .literal .Lsec_43_11c, esp_dport_access_reg_read
        .literal .Lsec_43_120, esp_dport_access_reg_read
        .literal .Lsec_43_124, esp_dport_access_reg_read
        .literal .Lsec_43_128, esp_dport_access_reg_read
        .literal .Lsec_43_12c, esp_dport_access_reg_read
        .literal .Lsec_43_130, esp_dport_access_reg_read
        .literal .Lsec_43_134, esp_dport_access_reg_read
        .literal .Lsec_43_138, esp_dport_access_reg_read
        .literal .Lsec_43_13c, esp_dport_access_reg_read
        .literal .Lsec_43_140, rtc_init_clk
.Lsec_43_144:
        .global rtc_init_full
        .type   rtc_init_full, @function
rtc_init_full:
        entry    a1, 48
        l32r     a10, .Lsec_43_0
        slli     a6, a6, 24
        extui    a5, a5, 0, 10
        slli     a5, a5, 14
        s32i     a4, a1, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_43_a0
        callx8   a8
        movi     a9, 0xfffffbff
        and      a9, a10, a9
        l32r     a4, .Lsec_43_4
        mov.n    a10, a4
        l32r     a11, .Lsec_43_0
        memw
        s32i.n   a9, a11, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_43_a4
        callx8   a8
        l32r     a9, .Lsec_43_8
        and      a10, a10, a9
        or       a6, a10, a6
        or       a10, a4, a4
        memw
        s32i.n   a6, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_43_a8
        callx8   a8
        l32r     a6, .Lsec_43_c
        and      a10, a10, a6
        or       a5, a5, a10
        mov.n    a10, a4
        memw
        s32i.n   a5, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_43_ac
        callx8   a8
        l32i.n   a8, a1, 0
        extui    a8, a8, 0, 8
        slli     a8, a8, 6
        l32r     a5, .Lsec_43_10
        and      a10, a10, a5
        or       a10, a8, a10
        memw
        s32i.n   a10, a4, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32i.n   a4, a1, 56
        beqz     a4, .Lsec_43_542
        l32r     a5, .Lsec_43_14
        mov.n    a10, a5
        l32i.n   a4, a1, 60
        beqz.n   a4, .Lsec_43_1e4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_43_b0
        callx8   a8
        l32r     a4, .Lsec_43_18
        or       a10, a10, a4
        memw
        s32i.n   a10, a5, 0
        j        .Lsec_43_1f5
        .byte    0x00
        .byte    0x00
.Lsec_43_1e4:
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_43_b4
        callx8   a8
        l32r     a4, .Lsec_43_1c
        and      a10, a10, a4
        memw
        s32i.n   a10, a5, 0
.Lsec_43_1f5:
        nop.n
        nop.n
        nop.n
        nop.n
        l32i     a4, a1, 64
        beqz.n   a4, .Lsec_43_21d
        l32r     a4, .Lsec_43_20
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_43_b8
        callx8   a8
        l32r     a5, .Lsec_43_24
        or       a10, a10, a5
        memw
        s32i.n   a10, a4, 0
        j        .Lsec_43_233
        .byte    0x00
        .byte    0x00
.Lsec_43_21d:
        l32r     a4, .Lsec_43_20
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_43_bc
        callx8   a8
        l32r     a5, .Lsec_43_28
        and      a10, a10, a5
        memw
        s32i.n   a10, a4, 0
.Lsec_43_233:
        mov.n    a10, a4
        nop.n
        nop.n
        nop
        l32i     a5, a1, 68
        beqz     a5, .Lsec_43_258
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_43_c0
        callx8   a8
        l32r     a5, .Lsec_43_2c
        or       a10, a10, a5
        memw
        s32i.n   a10, a4, 0
        j        .Lsec_43_269
        .byte    0x00
        .byte    0x00
.Lsec_43_258:
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_43_c4
        callx8   a8
        l32r     a5, .Lsec_43_30
        and      a10, a10, a5
        memw
        s32i.n   a10, a4, 0
.Lsec_43_269:
        mov.n    a10, a4
        nop.n
        nop.n
        nop.n
        l32i     a5, a1, 72
        beqz.n   a5, .Lsec_43_28c
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_43_c8
        callx8   a8
        l32r     a5, .Lsec_43_34
        or       a10, a10, a5
        memw
        s32i.n   a10, a4, 0
        j        .Lsec_43_29d
        .byte    0x00
        .byte    0x00
.Lsec_43_28c:
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_43_cc
        callx8   a8
        l32r     a5, .Lsec_43_38
        and      a10, a10, a5
        memw
        s32i.n   a10, a4, 0
.Lsec_43_29d:
        mov.n    a10, a4
        nop.n
        nop.n
        nop.n
        l32i     a5, a1, 76
        beqz.n   a5, .Lsec_43_2c0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_43_d0
        callx8   a8
        l32r     a5, .Lsec_43_3c
        or       a10, a10, a5
        memw
        s32i.n   a10, a4, 0
        j        .Lsec_43_2d1
        .byte    0x00
        .byte    0x00
.Lsec_43_2c0:
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_43_d4
        callx8   a8
        l32r     a5, .Lsec_43_40
        and      a10, a10, a5
        memw
        s32i.n   a10, a4, 0
.Lsec_43_2d1:
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_43_d8
        callx8   a8
        l32r     a6, .Lsec_43_44
        and      a5, a10, a6
        mov.n    a10, a4
        memw
        s32i     a5, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_43_dc
        callx8   a8
        l32r     a5, .Lsec_43_48
        and      a5, a10, a5
        mov.n    a10, a4
        memw
        s32i.n   a5, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_43_e0
        callx8   a8
        l32r     a5, .Lsec_43_4c
        and      a10, a10, a5
        l32i     a8, a1, 80
        memw
        s32i.n   a10, a4, 0
        mov.n    a10, a4
        beqz.n   a8, .Lsec_43_326
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_43_e4
        callx8   a8
        l32r     a8, .Lsec_43_50
        or       a10, a10, a8
        memw
        s32i.n   a10, a4, 0
        j        .Lsec_43_337
.Lsec_43_326:
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_43_e8
        callx8   a8
        l32r     a8, .Lsec_43_54
        and      a10, a10, a8
        memw
        s32i.n   a10, a4, 0
.Lsec_43_337:
        mov.n    a10, a4
        nop.n
        nop.n
        nop
        l32i     a8, a1, 84
        beqz     a8, .Lsec_43_35c
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_43_ec
        callx8   a8
        movi     a8, 0x200
        or       a10, a10, a8
        memw
        s32i.n   a10, a4, 0
        j        .Lsec_43_36d
        .byte    0x00
        .byte    0x00
.Lsec_43_35c:
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_43_f0
        callx8   a8
        movi     a8, 0xfffffdff
        and      a10, a10, a8
        memw
        s32i.n   a10, a4, 0
.Lsec_43_36d:
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a4, .Lsec_43_58
        or       a10, a4, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_43_f4
        callx8   a8
        and      a10, a10, a6
        memw
        s32i.n   a10, a4, 0
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_43_f8
        callx8   a8
        l32r     a6, .Lsec_43_5c
        and      a6, a10, a6
        mov.n    a10, a4
        memw
        s32i     a6, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_43_fc
        callx8   a8
        l32r     a6, .Lsec_43_60
        and      a6, a10, a6
        mov.n    a10, a4
        memw
        s32i.n   a6, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_43_100
        callx8   a8
        movi.n   a8, -17
        and      a8, a10, a8
        l32r     a6, .Lsec_43_64
        mov.n    a10, a6
        memw
        s32i.n   a8, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_43_104
        callx8   a8
        l32r     a8, .Lsec_43_68
        and      a8, a10, a8
        l32r     a4, .Lsec_43_6c
        or       a10, a4, a4
        memw
        s32i     a8, a6, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_43_108
        callx8   a8
        l32r     a8, .Lsec_43_70
        and      a8, a10, a8
        mov.n    a10, a4
        memw
        s32i     a8, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_43_10c
        callx8   a8
        l32r     a9, .Lsec_43_74
        and      a8, a10, a9
        mov.n    a10, a4
        memw
        s32i     a8, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_43_110
        callx8   a8
        l32r     a8, .Lsec_43_78
        and      a8, a10, a8
        or       a10, a6, a6
        memw
        s32i     a8, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_43_114
        callx8   a8
        movi.n   a8, -6
        and      a8, a10, a8
        mov.n    a10, a4
        memw
        s32i     a8, a6, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_43_118
        callx8   a8
        and      a5, a10, a5
        mov.n    a10, a4
        memw
        s32i.n   a5, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_43_11c
        callx8   a8
        l32r     a5, .Lsec_43_7c
        and      a10, a10, a5
        memw
        s32i.n   a10, a4, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32i     a4, a1, 96
        beqz.n   a4, .Lsec_43_479
        l32r     a4, .Lsec_43_80
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_43_120
        callx8   a8
        l32r     a5, .Lsec_43_84
        or       a10, a10, a5
        memw
        s32i.n   a10, a4, 0
        j        .Lsec_43_48f
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_43_479:
        l32r     a4, .Lsec_43_80
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_43_124
        callx8   a8
        l32r     a5, .Lsec_43_88
        and      a10, a10, a5
        memw
        s32i.n   a10, a4, 0
.Lsec_43_48f:
        mov.n    a10, a4
        nop.n
        nop.n
        nop
        l32i     a5, a1, 100
        beqz     a5, .Lsec_43_4b4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_43_128
        callx8   a8
        l32r     a5, .Lsec_43_8c
        or       a10, a10, a5
        memw
        s32i.n   a10, a4, 0
        j        .Lsec_43_4c5
        .byte    0x00
        .byte    0x00
.Lsec_43_4b4:
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_43_12c
        callx8   a8
        l32r     a11, .Lsec_43_74
        and      a10, a10, a11
        memw
        s32i.n   a10, a4, 0
.Lsec_43_4c5:
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a4, .Lsec_43_90
        mov.n    a10, a4
        l32i     a6, a1, 104
        extui    a6, a6, 0, 3
        l32i     a5, a1, 108
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_43_130
        callx8   a8
        slli     a9, a6, 22
        slli     a6, a6, 25
        extui    a5, a5, 0, 3
        l32r     a8, .Lsec_43_94
        and      a10, a10, a8
        or       a8, a10, a9
        mov.n    a10, a4
        memw
        s32i.n   a8, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_43_134
        callx8   a8
        l32r     a8, .Lsec_43_98
        and      a10, a10, a8
        or       a6, a10, a6
        or       a10, a4, a4
        memw
        s32i     a6, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_43_138
        callx8   a8
        slli     a8, a5, 11
        slli     a5, a5, 8
        l32r     a6, .Lsec_43_9c
        and      a10, a10, a6
        or       a6, a10, a8
        mov.n    a10, a4
        memw
        s32i.n   a6, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_43_13c
        callx8   a8
        movi     a6, 0xfffff8ff
        and      a10, a10, a6
        or       a5, a10, a5
        memw
        s32i.n   a5, a4, 0
.Lsec_43_542:
        mov.n    a13, a7
        mov.n    a12, a3
        mov.n    a11, a2
        movi.n   a10, 0
        l32i.n   a15, a1, 52
        l32i.n   a14, a1, 48
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_init_clk
        l32r     a8, .Lsec_43_140
        callx8   a8
        retw.n
        .size   rtc_init_full, .-rtc_init_full
.Lsec_43_556:

        .section .text.rtc_pad_gpio_wakeup,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_44_0, 0x3ff48428
        .literal .Lsec_44_4, 0x3ff4840c
        .literal .Lsec_44_8, 0x3ff48470
        .literal .Lsec_44_c, rtc_pads_muxsel
        .literal .Lsec_44_10, rtc_pads_funsel
        .literal .Lsec_44_14, esp_dport_access_reg_read
        .literal .Lsec_44_18, rtc_pads_slpsel
        .literal .Lsec_44_1c, rtc_pads_slpoe
        .literal .Lsec_44_20, rtc_pads_slpie
        .literal .Lsec_44_24, rtc_pads_funie
        .literal .Lsec_44_28, rtc_pads_pu
        .literal .Lsec_44_2c, rtc_pads_pd
        .literal .Lsec_44_30, esp_dport_access_reg_read
        .literal .Lsec_44_34, esp_dport_access_reg_read
.Lsec_44_38:
        .global rtc_pad_gpio_wakeup
        .type   rtc_pad_gpio_wakeup, @function
rtc_pad_gpio_wakeup:
        entry    a1, 48
        mov.n    a10, a2
        movi     a11, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_pads_muxsel
        l32r     a8, .Lsec_44_c
        callx8   a8
        movi.n   a11, 0
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_pads_funsel
        l32r     a8, .Lsec_44_10
        callx8   a8
        l32r     a5, .Lsec_44_4
        movi.n   a4, -1
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_44_14
        callx8   a8
        xor      a4, a4, a2
        and      a10, a4, a10
        memw
        s32i.n   a10, a5, 0
        movi     a11, 1
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_pads_slpsel
        l32r     a8, .Lsec_44_18
        callx8   a8
        mov.n    a10, a2
        movi.n   a11, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_pads_slpoe
        l32r     a8, .Lsec_44_1c
        callx8   a8
        mov.n    a10, a2
        movi.n   a11, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_pads_slpie
        l32r     a8, .Lsec_44_20
        callx8   a8
        mov.n    a10, a2
        movi.n   a11, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_pads_funie
        l32r     a8, .Lsec_44_24
        callx8   a8
        mov.n    a10, a2
        movi.n   a11, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_pads_pu
        l32r     a8, .Lsec_44_28
        callx8   a8
        movi.n   a11, 0
        mov.n    a10, a2
        extui    a3, a3, 0, 3
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_pads_pd
        l32r     a8, .Lsec_44_2c
        callx8   a8
        slli     a3, a3, 7
        l32r     a4, .Lsec_44_0
        movi     a7, 0x400
        movi     a6, 0xfffffc7f
        l32r     a5, .Lsec_44_8
        s32i.n   a3, a1, 0
        movi     a3, 1
.Lsec_44_bd:
        bnone    a2, a3, .Lsec_44_e5
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_44_30
        callx8   a8
        or       a10, a10, a7
        memw
        s32i.n   a10, a4, 0
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_44_34
        callx8   a8
        and      a10, a10, a6
        l32i.n   a8, a1, 0
        or       a10, a10, a8
        memw
        s32i.n   a10, a4, 0
.Lsec_44_e5:
        addi.n   a4, a4, 4
        slli     a3, a3, 1
        bne      a4, a5, .Lsec_44_bd
        retw.n
        .size   rtc_pad_gpio_wakeup, .-rtc_pad_gpio_wakeup
.Lsec_44_ef:

        .section .text.rtc_pad_ext_wakeup,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_45_0, 0x3ff4840c
        .literal .Lsec_45_4, 0x3ff484bc
        .literal .Lsec_45_8, 0x07ffffff
        .literal .Lsec_45_c, 0x3ff48060
        .literal .Lsec_45_10, 0xbfffffff
        .literal .Lsec_45_14, rtc_pads_muxsel
        .literal .Lsec_45_18, rtc_pads_funsel
        .literal .Lsec_45_1c, esp_dport_access_reg_read
        .literal .Lsec_45_20, rtc_pads_slpsel
        .literal .Lsec_45_24, rtc_pads_slpoe
        .literal .Lsec_45_28, rtc_pads_slpie
        .literal .Lsec_45_2c, rtc_pads_funie
        .literal .Lsec_45_30, rtc_pads_pu
        .literal .Lsec_45_34, rtc_pads_pd
        .literal .Lsec_45_38, esp_dport_access_reg_read
        .literal .Lsec_45_3c, esp_dport_access_reg_read
.Lsec_45_40:
        .global rtc_pad_ext_wakeup
        .type   rtc_pad_ext_wakeup, @function
rtc_pad_ext_wakeup:
        entry    a1, 32
        mov.n    a10, a2
        movi     a11, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_pads_muxsel
        l32r     a8, .Lsec_45_14
        callx8   a8
        mov.n    a10, a2
        movi.n   a11, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_pads_funsel
        l32r     a8, .Lsec_45_18
        callx8   a8
        l32r     a6, .Lsec_45_0
        slli     a5, a2, 14
        mov.n    a10, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_45_1c
        callx8   a8
        movi.n   a8, -1
        xor      a5, a8, a5
        and      a10, a5, a10
        memw
        s32i.n   a10, a6, 0
        movi.n   a11, 1
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_pads_slpsel
        l32r     a8, .Lsec_45_20
        callx8   a8
        mov.n    a10, a2
        movi.n   a11, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_pads_slpoe
        l32r     a8, .Lsec_45_24
        callx8   a8
        mov.n    a10, a2
        movi.n   a11, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_pads_slpie
        l32r     a8, .Lsec_45_28
        callx8   a8
        mov.n    a10, a2
        movi.n   a11, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_pads_funie
        l32r     a8, .Lsec_45_2c
        callx8   a8
        mov.n    a10, a2
        movi.n   a11, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_pads_pu
        l32r     a8, .Lsec_45_30
        callx8   a8
        mov.n    a10, a2
        movi.n   a11, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_pads_pd
        l32r     a8, .Lsec_45_34
        callx8   a8
        l32r     a2, .Lsec_45_4
        slli     a3, a3, 27
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_45_38
        callx8   a8
        extui    a4, a4, 0, 1
        l32r     a5, .Lsec_45_8
        slli     a4, a4, 30
        and      a10, a10, a5
        or       a3, a10, a3
        l32r     a5, .Lsec_45_c
        mov.n    a10, a5
        memw
        s32i.n   a3, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_45_3c
        callx8   a8
        l32r     a2, .Lsec_45_10
        and      a10, a10, a2
        or       a10, a4, a10
        memw
        s32i.n   a10, a5, 0
        retw.n
        .size   rtc_pad_ext_wakeup, .-rtc_pad_ext_wakeup
.Lsec_45_ed:

        .section .text.rtc_cmd_ext_wakeup,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_46_0, 0x3ff4840c
        .literal .Lsec_46_4, 0x3ff484bc
        .literal .Lsec_46_8, 0x07ffffff
        .literal .Lsec_46_c, 0x3ff48060
        .literal .Lsec_46_10, 0x40000000
        .literal .Lsec_46_14, rtc_pads_muxsel
        .literal .Lsec_46_18, rtc_pads_funsel
        .literal .Lsec_46_1c, esp_dport_access_reg_read
        .literal .Lsec_46_20, rtc_pads_slpsel
        .literal .Lsec_46_24, rtc_pads_slpoe
        .literal .Lsec_46_28, rtc_pads_slpie
        .literal .Lsec_46_2c, rtc_pads_funie
        .literal .Lsec_46_30, rtc_pads_pu
        .literal .Lsec_46_34, rtc_pads_pd
        .literal .Lsec_46_38, esp_dport_access_reg_read
        .literal .Lsec_46_3c, esp_dport_access_reg_read
.Lsec_46_40:
        .global rtc_cmd_ext_wakeup
        .type   rtc_cmd_ext_wakeup, @function
rtc_cmd_ext_wakeup:
        entry    a1, 32
        mov.n    a10, a2
        movi     a11, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_pads_muxsel
        l32r     a8, .Lsec_46_14
        callx8   a8
        mov.n    a10, a2
        movi.n   a11, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_pads_funsel
        l32r     a8, .Lsec_46_18
        callx8   a8
        l32r     a4, .Lsec_46_0
        movi.n   a5, -1
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_46_1c
        callx8   a8
        slli     a8, a2, 14
        xor      a8, a5, a8
        and      a10, a8, a10
        memw
        s32i.n   a10, a4, 0
        movi.n   a11, 1
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_pads_slpsel
        l32r     a8, .Lsec_46_20
        callx8   a8
        mov.n    a10, a2
        movi.n   a11, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_pads_slpoe
        l32r     a8, .Lsec_46_24
        callx8   a8
        mov.n    a10, a2
        movi.n   a11, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_pads_slpie
        l32r     a8, .Lsec_46_28
        callx8   a8
        mov.n    a10, a2
        movi.n   a11, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_pads_funie
        l32r     a8, .Lsec_46_2c
        callx8   a8
        mov.n    a10, a2
        movi.n   a11, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_pads_pu
        l32r     a8, .Lsec_46_30
        callx8   a8
        mov.n    a10, a2
        movi     a11, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_pads_pd
        l32r     a8, .Lsec_46_34
        callx8   a8
        l32r     a4, .Lsec_46_4
        slli     a3, a3, 27
        or       a10, a4, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_46_38
        callx8   a8
        l32r     a2, .Lsec_46_8
        and      a10, a10, a2
        or       a3, a10, a3
        l32r     a5, .Lsec_46_c
        mov.n    a10, a5
        memw
        s32i.n   a3, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_46_3c
        callx8   a8
        l32r     a2, .Lsec_46_10
        or       a10, a10, a2
        memw
        s32i.n   a10, a5, 0
        retw.n
        .size   rtc_cmd_ext_wakeup, .-rtc_cmd_ext_wakeup
.Lsec_46_e6:

        .section .text.rtc_wifi_force_pd,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_47_0, 0x3ff000cc
        .literal .Lsec_47_4, 0x3ff000d0
        .literal .Lsec_47_8, 0x3ff48088
        .literal .Lsec_47_c, 0x10000000
        .literal .Lsec_47_10, 0x3ff48084
        .literal .Lsec_47_14, 0x00020000
        .literal .Lsec_47_18, esp_dport_access_reg_read
        .literal .Lsec_47_1c, esp_dport_access_reg_read
        .literal .Lsec_47_20, esp_dport_access_reg_read
        .literal .Lsec_47_24, esp_dport_access_reg_read
.Lsec_47_28:
        .global rtc_wifi_force_pd
        .type   rtc_wifi_force_pd, @function
rtc_wifi_force_pd:
        entry    a1, 32
        l32r     a3, .Lsec_47_0
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_47_18
        callx8   a8
        movi     a8, -65
        l32r     a2, .Lsec_47_4
        and      a8, a10, a8
        mov.n    a10, a2
        memw
        s32i.n   a8, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_47_1c
        callx8   a8
        movi.n   a8, 4
        or       a8, a10, a8
        l32r     a3, .Lsec_47_8
        mov.n    a10, a3
        memw
        s32i.n   a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_47_20
        callx8   a8
        l32r     a8, .Lsec_47_c
        or       a8, a10, a8
        l32r     a2, .Lsec_47_10
        mov.n    a10, a2
        memw
        s32i     a8, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_47_24
        callx8   a8
        l32r     a8, .Lsec_47_14
        or       a10, a10, a8
        memw
        s32i.n   a10, a2, 0
        retw.n
        .size   rtc_wifi_force_pd, .-rtc_wifi_force_pd
.Lsec_47_85:

        .section .text.rtc_sdreg_off,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_48_0, 0x3ff48074
        .literal .Lsec_48_4, 0x00400000
        .literal .Lsec_48_8, 0x00800000
        .literal .Lsec_48_c, 0x7fffffff
        .literal .Lsec_48_10, esp_dport_access_reg_read
        .literal .Lsec_48_14, esp_dport_access_reg_read
        .literal .Lsec_48_18, esp_dport_access_reg_read
.Lsec_48_1c:
        .global rtc_sdreg_off
        .type   rtc_sdreg_off, @function
rtc_sdreg_off:
        entry    a1, 32
        l32r     a2, .Lsec_48_0
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_48_10
        callx8   a8
        l32r     a8, .Lsec_48_4
        or       a8, a10, a8
        mov.n    a10, a2
        memw
        s32i.n   a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_48_14
        callx8   a8
        l32r     a8, .Lsec_48_8
        or       a8, a10, a8
        mov.n    a10, a2
        memw
        s32i.n   a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_48_18
        callx8   a8
        l32r     a8, .Lsec_48_c
        and      a10, a10, a8
        memw
        s32i.n   a10, a2, 0
        retw.n
        .size   rtc_sdreg_off, .-rtc_sdreg_off
.Lsec_48_5d:

        .section .text.rtc_slp_prep_lite_12M,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_49_0, rtc_slp_prep
.Lsec_49_4:
        .global rtc_slp_prep_lite_12M
        .type   rtc_slp_prep_lite_12M, @function
rtc_slp_prep_lite_12M:
        entry    a1, 80
        movi.n   a8, 0
        movi.n   a9, 4
        s32i.n   a8, a1, 36
        mov.n    a15, a8
        mov.n    a14, a8
        mov.n    a13, a8
        mov.n    a12, a8
        s32i.n   a8, a1, 32
        mov.n    a11, a8
        movi.n   a10, 2
        s32i.n   a8, a1, 28
        s32i.n   a9, a1, 24
        s32i.n   a3, a1, 20
        s32i.n   a8, a1, 16
        s32i.n   a2, a1, 12
        s32i.n   a8, a1, 8
        s32i.n   a8, a1, 4
        s32i.n   a8, a1, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_slp_prep
        l32r     a8, .Lsec_49_0
        callx8   a8
        retw.n
        .size   rtc_slp_prep_lite_12M, .-rtc_slp_prep_lite_12M
.Lsec_49_33:

        .section .text.cfg_sdio_volt,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_50_0, 0x3ff48074
        .literal .Lsec_50_4, 0x80000000
        .literal .Lsec_50_8, 0x9fffffff
        .literal .Lsec_50_c, 0xe7ffffff
        .literal .Lsec_50_10, 0xf9ffffff
        .literal .Lsec_50_14, 0x01000000
        .literal .Lsec_50_18, 0xff7fffff
        .literal .Lsec_50_1c, 0x00400000
        .literal .Lsec_50_20, esp_dport_access_reg_read
        .literal .Lsec_50_24, esp_dport_access_reg_read
        .literal .Lsec_50_28, esp_dport_access_reg_read
        .literal .Lsec_50_2c, esp_dport_access_reg_read
        .literal .Lsec_50_30, esp_dport_access_reg_read
        .literal .Lsec_50_34, esp_dport_access_reg_read
        .literal .Lsec_50_38, esp_dport_access_reg_read
.Lsec_50_3c:
        .global cfg_sdio_volt
        .type   cfg_sdio_volt, @function
cfg_sdio_volt:
        entry    a1, 32
        l32r     a5, .Lsec_50_0
        extui    a2, a2, 0, 8
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_50_20
        callx8   a8
        extui    a3, a3, 0, 8
        l32r     a8, .Lsec_50_4
        extui    a4, a4, 0, 8
        or       a8, a10, a8
        or       a10, a5, a5
        memw
        s32i     a8, a5, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_50_24
        callx8   a8
        extui    a8, a2, 0, 2
        slli     a8, a8, 29
        l32r     a9, .Lsec_50_8
        and      a10, a10, a9
        or       a8, a8, a10
        mov.n    a10, a5
        memw
        s32i     a8, a5, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_50_28
        callx8   a8
        extui    a8, a3, 0, 2
        slli     a8, a8, 27
        l32r     a9, .Lsec_50_c
        and      a10, a10, a9
        or       a8, a8, a10
        mov.n    a10, a5
        memw
        s32i.n   a8, a5, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_50_2c
        callx8   a8
        extui    a8, a4, 0, 2
        slli     a8, a8, 25
        l32r     a9, .Lsec_50_10
        and      a10, a10, a9
        or       a8, a8, a10
        mov.n    a10, a5
        memw
        s32i.n   a8, a5, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_50_30
        callx8   a8
        l32r     a2, .Lsec_50_14
        or       a2, a10, a2
        mov.n    a10, a5
        memw
        s32i     a2, a5, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_50_34
        callx8   a8
        l32r     a2, .Lsec_50_18
        and      a2, a10, a2
        mov.n    a10, a5
        memw
        s32i.n   a2, a5, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_50_38
        callx8   a8
        l32r     a2, .Lsec_50_1c
        or       a10, a10, a2
        memw
        s32i.n   a10, a5, 0
        retw.n
        .size   cfg_sdio_volt, .-cfg_sdio_volt
.Lsec_50_f1:
