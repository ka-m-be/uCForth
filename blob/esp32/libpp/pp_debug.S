.section .iram1.3,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_12_0, .Lsec_70_0
.Lsec_12_4:
        .global wifi_gpio_debug
        .type   wifi_gpio_debug, @function
wifi_gpio_debug:
        entry    a1, 32
        l32r     a8, .Lsec_12_0
        mov.n    a10, a2
        mov.n    a11, a3
        l32i.n   a8, a8, 0
        beqz.n   a8, .Lsec_12_15
        callx8   a8
.Lsec_12_15:
        retw.n
        .size   wifi_gpio_debug, .-wifi_gpio_debug
.Lsec_12_17:

        .section .iram1.4,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_13_0, .Lsec_70_0
.Lsec_13_4:
        .global wifi_set_gpio_debug_cb
        .type   wifi_set_gpio_debug_cb, @function
wifi_set_gpio_debug_cb:
        entry    a1, 32
        beqz     a2, .Lsec_13_10
        l32r     a8, .Lsec_13_0
        s32i     a2, a8, 0
.Lsec_13_10:
        memw
        retw.n
        .size   wifi_set_gpio_debug_cb, .-wifi_set_gpio_debug_cb
.Lsec_13_15:

        .section .text.wifi_gpio_debug_max_event_get,"ax",@progbits
        .align  4
.Lsec_14_0:
        .global wifi_gpio_debug_max_event_get
        .type   wifi_gpio_debug_max_event_get, @function
wifi_gpio_debug_max_event_get:
        entry    a1, 32
        movi.n   a2, 18
        retw.n
        .size   wifi_gpio_debug_max_event_get, .-wifi_gpio_debug_max_event_get
.Lsec_14_7:

        .section .rodata.dbg_lmac_rxtx_statis_dump.str1.1,"aMS",@progbits,1
        .align  1
.Lsec_15_0:
        .ascii  "lrc\000"
.Lsec_15_4:
        .ascii  "tx_all\000"
.Lsec_15_b:
        .ascii  "to\000"
.Lsec_15_e:
        .ascii  "age\000"
.Lsec_15_12:
        .ascii  "src\000"
.Lsec_15_16:
        .ascii  "mdpu\000"
.Lsec_15_1b:
        .ascii  "ampdu\000"
.Lsec_15_21:
        .ascii  "frag\000"
.Lsec_15_26:
        .ascii  "ctrl\000"
.Lsec_15_2b:
        .ascii  "mgmt\000"
.Lsec_15_30:
        .ascii  "rx_end\000"
.Lsec_15_37:
        .ascii  "rx_suc\000"
.Lsec_15_3e:
        .ascii  "full#2\000"
.Lsec_15_45:
        .ascii  "full#1\000"

        .section .text.dbg_lmac_rxtx_statis_dump,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_16_0, .Lsec_67_0
        .literal .Lsec_16_4, .Lsec_66_0
        .literal .Lsec_16_8, .Lsec_15_0
        .literal .Lsec_16_c, .Lsec_15_4
        .literal .Lsec_16_10, .Lsec_65_0
        .literal .Lsec_16_14, .Lsec_15_b
        .literal .Lsec_16_18, .Lsec_15_e
        .literal .Lsec_16_1c, .Lsec_15_12
        .literal .Lsec_16_20, g_lmac_cnt
        .literal .Lsec_16_24, .Lsec_64_0
        .literal .Lsec_16_28, .Lsec_63_0
        .literal .Lsec_16_2c, .Lsec_62_0
        .literal .Lsec_16_30, .Lsec_15_16
        .literal .Lsec_16_34, .Lsec_15_1b
        .literal .Lsec_16_38, .Lsec_61_0
        .literal .Lsec_16_3c, .Lsec_15_21
        .literal .Lsec_16_40, .Lsec_15_26
        .literal .Lsec_16_44, .Lsec_15_2b
        .literal .Lsec_16_48, .Lsec_60_0
        .literal .Lsec_16_4c, .Lsec_59_0
        .literal .Lsec_16_50, .Lsec_58_0
        .literal .Lsec_16_54, .Lsec_15_30
        .literal .Lsec_16_58, .Lsec_15_37
        .literal .Lsec_16_5c, .Lsec_57_0
        .literal .Lsec_16_60, .Lsec_15_3e
        .literal .Lsec_16_64, .Lsec_15_45
        .literal .Lsec_16_68, 0x3ff732cc
        .literal .Lsec_16_6c, 0x3ff732d0
        .literal .Lsec_16_70, 0x3ff732ac
        .literal .Lsec_16_74, 0x3ff732dc
        .literal .Lsec_16_78, .Lsec_56_0
        .literal .Lsec_16_7c, wifi_log
        .literal .Lsec_16_80, wifi_log
        .literal .Lsec_16_84, wifi_log
        .literal .Lsec_16_88, wifi_log
        .literal .Lsec_16_8c, wifi_log
        .literal .Lsec_16_90, wifi_log
        .literal .Lsec_16_94, wifi_log
        .literal .Lsec_16_98, wifi_log
        .literal .Lsec_16_9c, wifi_log
        .literal .Lsec_16_a0, wifi_log
        .literal .Lsec_16_a4, wifi_log
        .literal .Lsec_16_a8, wifi_log
.Lsec_16_ac:
        .global dbg_lmac_rxtx_statis_dump
        .type   dbg_lmac_rxtx_statis_dump, @function
dbg_lmac_rxtx_statis_dump:
        entry    a1, 48
        movi.n   a2, 1
        slli     a2, a2, 19
        l32r     a13, .Lsec_16_0
        movi.n   a12, 3
        or       a11, a2, a2
        movi     a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_16_7c
        callx8   a8
        l32r     a13, .Lsec_16_4
        movi.n   a12, 3
        mov.n    a11, a2
        movi.n   a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_16_80
        callx8   a8
        l32r     a3, .Lsec_16_14
        movi.n   a12, 3
        mov.n    a11, a2
        movi.n   a10, 6
        movi.n   a5, 0
        s32i.n   a3, a1, 8
        l32r     a3, .Lsec_16_18
        s32i.n   a3, a1, 4
        l32r     a3, .Lsec_16_1c
        l32r     a15, .Lsec_16_8
        l32r     a14, .Lsec_16_c
        l32r     a13, .Lsec_16_10
        s32i.n   a3, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_16_84
        callx8   a8
        l32r     a3, .Lsec_16_20
        mov.n    a4, a3
.Lsec_16_102:
        movi.n   a2, 1
        slli     a2, a2, 19
        l8ui     a8, a3, 127
        movi.n   a12, 3
        mov.n    a11, a2
        movi.n   a10, 6
        addi.n   a5, a5, 1
        l8ui     a15, a3, 124
        addi.n   a3, a3, 8
        s32i     a8, a1, 8
        l8ui     a8, a3, 118
        s32i     a8, a1, 4
        l8ui     a8, a3, 117
        s32i.n   a8, a1, 0
        l32i     a14, a3, 112
        l32r     a13, .Lsec_16_24
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_16_88
        callx8   a8
        bnei     a5, 5, .Lsec_16_102
        mov.n    a11, a2
        l32r     a13, .Lsec_16_28
        movi.n   a12, 3
        movi.n   a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_16_8c
        callx8   a8
        mov.n    a11, a2
        l32r     a13, .Lsec_16_2c
        movi.n   a12, 3
        movi     a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_16_90
        callx8   a8
        mov.n    a11, a2
        l32r     a3, .Lsec_16_3c
        movi.n   a12, 3
        movi.n   a10, 6
        s32i.n   a3, a1, 8
        l32r     a3, .Lsec_16_40
        s32i.n   a3, a1, 4
        l32r     a3, .Lsec_16_44
        l32r     a15, .Lsec_16_30
        l32r     a14, .Lsec_16_34
        l32r     a13, .Lsec_16_38
        s32i.n   a3, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_16_94
        callx8   a8
        mov.n    a11, a2
        movi.n   a12, 3
        movi.n   a10, 6
        l16ui    a3, a4, 176
        s32i.n   a3, a1, 8
        l32i     a3, a4, 172
        s32i     a3, a1, 4
        l32i     a3, a4, 168
        s32i     a3, a1, 0
        l32i     a15, a4, 164
        l32i     a14, a4, 160
        l32r     a13, .Lsec_16_48
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_16_98
        callx8   a8
        mov.n    a11, a2
        movi.n   a12, 3
        l32r     a13, .Lsec_16_4c
        movi.n   a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_16_9c
        callx8   a8
        mov.n    a11, a2
        movi.n   a12, 3
        l32r     a13, .Lsec_16_50
        movi.n   a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_16_a0
        callx8   a8
        mov.n    a11, a2
        movi.n   a12, 3
        l32r     a3, .Lsec_16_60
        movi.n   a10, 6
        s32i.n   a3, a1, 4
        l32r     a3, .Lsec_16_64
        l32r     a15, .Lsec_16_54
        l32r     a14, .Lsec_16_58
        l32r     a13, .Lsec_16_5c
        s32i.n   a3, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_16_a4
        callx8   a8
        movi.n   a12, 3
        mov.n    a11, a2
        movi.n   a10, 6
        l32r     a3, .Lsec_16_68
        memw
        l32i.n   a14, a3, 0
        l32r     a3, .Lsec_16_6c
        l32r     a4, .Lsec_16_74
        memw
        l32i.n   a15, a3, 0
        l32r     a3, .Lsec_16_70
        memw
        l32i.n   a3, a3, 0
        memw
        l32i.n   a4, a4, 0
        l32r     a13, .Lsec_16_78
        s32i.n   a4, a1, 4
        s32i.n   a3, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_16_a8
        callx8   a8
        retw.n
        .size   dbg_lmac_rxtx_statis_dump, .-dbg_lmac_rxtx_statis_dump
.Lsec_16_21e:

        .section .rodata.dbg_lmac_hw_statis_dump.str1.1,"aMS",@progbits,1
        .align  1
.Lsec_17_0:
        .ascii  "rxcckerr\000"
.Lsec_17_9:
        .ascii  "rxbufbk\000"
.Lsec_17_11:
        .ascii  "fifofull\000"
.Lsec_17_1a:
        .ascii  "rx_fcs\000"
.Lsec_17_21:
        .ascii  "rx_abort\000"
.Lsec_17_2a:
        .ascii  "rxsf\000"
.Lsec_17_2f:
        .ascii  "rx_agc\000"
.Lsec_17_36:
        .ascii  "ofdmerr\000"
.Lsec_17_3e:
        .ascii  "rx_oth\000"
.Lsec_17_45:
        .ascii  "afull\000"
.Lsec_17_4b:
        .ascii  "rifs_int\000"
.Lsec_17_54:
        .ascii  "cts_int\000"
.Lsec_17_5c:
        .ascii  "rts_int\000"
.Lsec_17_64:
        .ascii  "ack_int\000"
.Lsec_17_6c:
        .ascii  "samebm\000"
.Lsec_17_73:
        .ascii  "tkiperr\000"
.Lsec_17_7b:
        .ascii  "hoperr\000"
.Lsec_17_82:
        .ascii  "blockerr\000"
.Lsec_17_8b:
        .ascii  "panic\000"
.Lsec_17_91:
        .ascii  "txhung\000"
.Lsec_17_98:
        .ascii  "trigger\000"
.Lsec_17_a0:
        .ascii  "trcts\000"
.Lsec_17_a6:
        .ascii  "track\000"
.Lsec_17_ac:
        .ascii  "txcts\000"
.Lsec_17_b2:
        .ascii  "txrts\000"

        .section .text.dbg_lmac_hw_statis_dump,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_18_0, .Lsec_55_0
        .literal .Lsec_18_4, .Lsec_54_0
        .literal .Lsec_18_8, .Lsec_17_0
        .literal .Lsec_18_c, .Lsec_17_9
        .literal .Lsec_18_10, .Lsec_53_0
        .literal .Lsec_18_14, .Lsec_17_11
        .literal .Lsec_18_18, .Lsec_17_1a
        .literal .Lsec_18_1c, .Lsec_17_21
        .literal .Lsec_18_20, .Lsec_17_2a
        .literal .Lsec_18_24, .Lsec_17_2f
        .literal .Lsec_18_28, .Lsec_17_36
        .literal .Lsec_18_2c, 0x3ff73094
        .literal .Lsec_18_30, 0x3ff7328c
        .literal .Lsec_18_34, 0x3ff73290
        .literal .Lsec_18_38, 0x3ff73294
        .literal .Lsec_18_3c, 0x3ff73298
        .literal .Lsec_18_40, 0x3ff7329c
        .literal .Lsec_18_44, 0x3ff732a0
        .literal .Lsec_18_48, 0x3ff732a4
        .literal .Lsec_18_4c, .Lsec_52_0
        .literal .Lsec_18_50, .Lsec_17_3e
        .literal .Lsec_18_54, .Lsec_17_45
        .literal .Lsec_18_58, .Lsec_51_0
        .literal .Lsec_18_5c, .Lsec_17_4b
        .literal .Lsec_18_60, .Lsec_17_54
        .literal .Lsec_18_64, .Lsec_17_5c
        .literal .Lsec_18_68, .Lsec_17_64
        .literal .Lsec_18_6c, .Lsec_17_6c
        .literal .Lsec_18_70, .Lsec_17_73
        .literal .Lsec_18_74, 0x3ff732a8
        .literal .Lsec_18_78, 0x3ff732b0
        .literal .Lsec_18_7c, 0x3ff732b4
        .literal .Lsec_18_80, 0x3ff732b8
        .literal .Lsec_18_84, 0x3ff732bc
        .literal .Lsec_18_88, 0x3ff732c0
        .literal .Lsec_18_8c, 0x3ff732c4
        .literal .Lsec_18_90, 0x3ff732c8
        .literal .Lsec_18_94, .Lsec_50_0
        .literal .Lsec_18_98, .Lsec_17_7b
        .literal .Lsec_18_9c, .Lsec_17_82
        .literal .Lsec_18_a0, .Lsec_49_0
        .literal .Lsec_18_a4, .Lsec_17_8b
        .literal .Lsec_18_a8, .Lsec_17_91
        .literal .Lsec_18_ac, .Lsec_17_98
        .literal .Lsec_18_b0, .Lsec_17_a0
        .literal .Lsec_18_b4, .Lsec_17_a6
        .literal .Lsec_18_b8, .Lsec_17_ac
        .literal .Lsec_18_bc, .Lsec_17_b2
        .literal .Lsec_18_c0, 0x3ff732e0
        .literal .Lsec_18_c4, 0x3ff732d8
        .literal .Lsec_18_c8, 0x3ff73d58
        .literal .Lsec_18_cc, 0x3ff73d5c
        .literal .Lsec_18_d0, 0x3ff73d60
        .literal .Lsec_18_d4, 0x3ff73d64
        .literal .Lsec_18_d8, 0x3ff73d68
        .literal .Lsec_18_dc, 0x3ff73d6c
        .literal .Lsec_18_e0, 0x3ff73d70
        .literal .Lsec_18_e4, .Lsec_48_0
        .literal .Lsec_18_e8, wifi_log
        .literal .Lsec_18_ec, wifi_log
        .literal .Lsec_18_f0, wifi_log
        .literal .Lsec_18_f4, wifi_log
        .literal .Lsec_18_f8, wifi_log
        .literal .Lsec_18_fc, wifi_log
        .literal .Lsec_18_100, wifi_log
        .literal .Lsec_18_104, wifi_log
.Lsec_18_108:
        .global dbg_lmac_hw_statis_dump
        .type   dbg_lmac_hw_statis_dump, @function
dbg_lmac_hw_statis_dump:
        entry    a1, 64
        movi.n   a2, 1
        slli     a2, a2, 19
        l32r     a13, .Lsec_18_0
        mov.n    a11, a2
        movi     a12, 3
        movi     a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_18_e8
        callx8   a8
        l32r     a13, .Lsec_18_4
        mov.n    a11, a2
        movi.n   a12, 3
        movi.n   a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_18_ec
        callx8   a8
        l32r     a3, .Lsec_18_14
        mov.n    a11, a2
        movi.n   a12, 3
        movi.n   a10, 6
        s32i.n   a3, a1, 20
        l32r     a3, .Lsec_18_18
        s32i.n   a3, a1, 16
        l32r     a3, .Lsec_18_1c
        s32i.n   a3, a1, 12
        l32r     a3, .Lsec_18_20
        s32i     a3, a1, 8
        l32r     a3, .Lsec_18_24
        s32i     a3, a1, 4
        l32r     a3, .Lsec_18_28
        l32r     a15, .Lsec_18_8
        l32r     a14, .Lsec_18_c
        l32r     a13, .Lsec_18_10
        s32i.n   a3, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_18_f0
        callx8   a8
        l32r     a3, .Lsec_18_2c
        memw
        l32i.n   a14, a3, 0
        l32r     a3, .Lsec_18_30
        l32r     a8, .Lsec_18_38
        memw
        l32i.n   a15, a3, 0
        l32r     a9, .Lsec_18_3c
        l32r     a3, .Lsec_18_34
        l32r     a10, .Lsec_18_40
        l32r     a11, .Lsec_18_44
        l32r     a12, .Lsec_18_48
        memw
        l32i.n   a3, a3, 0
        memw
        l32i.n   a8, a8, 0
        memw
        l32i.n   a9, a9, 0
        memw
        l32i.n   a10, a10, 0
        memw
        l32i.n   a11, a11, 0
        memw
        l32i.n   a12, a12, 0
        l32r     a13, .Lsec_18_4c
        s32i.n   a9, a1, 8
        s32i.n   a8, a1, 4
        s32i.n   a12, a1, 20
        movi.n   a12, 3
        s32i.n   a11, a1, 16
        mov.n    a11, a2
        s32i.n   a10, a1, 12
        movi.n   a10, 6
        s32i.n   a3, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_18_f4
        callx8   a8
        mov.n    a11, a2
        movi.n   a12, 3
        movi.n   a10, 6
        l32r     a3, .Lsec_18_5c
        s32i.n   a3, a1, 20
        l32r     a3, .Lsec_18_60
        s32i.n   a3, a1, 16
        l32r     a3, .Lsec_18_64
        s32i.n   a3, a1, 12
        l32r     a3, .Lsec_18_68
        s32i.n   a3, a1, 8
        l32r     a3, .Lsec_18_6c
        s32i.n   a3, a1, 4
        l32r     a3, .Lsec_18_70
        l32r     a15, .Lsec_18_50
        l32r     a14, .Lsec_18_54
        l32r     a13, .Lsec_18_58
        s32i.n   a3, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_18_f8
        callx8   a8
        l32r     a3, .Lsec_18_74
        memw
        l32i.n   a14, a3, 0
        l32r     a3, .Lsec_18_78
        l32r     a8, .Lsec_18_80
        memw
        l32i.n   a15, a3, 0
        l32r     a9, .Lsec_18_84
        l32r     a3, .Lsec_18_7c
        l32r     a10, .Lsec_18_88
        l32r     a11, .Lsec_18_8c
        l32r     a12, .Lsec_18_90
        memw
        l32i.n   a3, a3, 0
        memw
        l32i.n   a8, a8, 0
        memw
        l32i.n   a9, a9, 0
        memw
        l32i.n   a10, a10, 0
        memw
        l32i.n   a11, a11, 0
        memw
        l32i.n   a12, a12, 0
        l32r     a13, .Lsec_18_94
        s32i.n   a9, a1, 8
        s32i.n   a8, a1, 4
        s32i.n   a12, a1, 20
        movi.n   a12, 3
        s32i.n   a11, a1, 16
        mov.n    a11, a2
        s32i.n   a10, a1, 12
        movi.n   a10, 6
        s32i.n   a3, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_18_fc
        callx8   a8
        mov.n    a11, a2
        movi.n   a12, 3
        movi.n   a10, 6
        l32r     a3, .Lsec_18_a4
        s32i.n   a3, a1, 24
        l32r     a3, .Lsec_18_a8
        s32i.n   a3, a1, 20
        l32r     a3, .Lsec_18_ac
        s32i.n   a3, a1, 16
        l32r     a3, .Lsec_18_b0
        s32i.n   a3, a1, 12
        l32r     a3, .Lsec_18_b4
        s32i.n   a3, a1, 8
        l32r     a3, .Lsec_18_b8
        s32i.n   a3, a1, 4
        l32r     a3, .Lsec_18_bc
        l32r     a15, .Lsec_18_98
        l32r     a14, .Lsec_18_9c
        l32r     a13, .Lsec_18_a0
        s32i.n   a3, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_18_100
        callx8   a8
        l32r     a3, .Lsec_18_c0
        memw
        l32i.n   a14, a3, 0
        l32r     a3, .Lsec_18_c4
        memw
        l32i.n   a15, a3, 0
        l32r     a3, .Lsec_18_c8
        memw
        l32i.n   a8, a3, 0
        l32r     a3, .Lsec_18_cc
        memw
        l32i.n   a9, a3, 0
        l32r     a3, .Lsec_18_d0
        memw
        l32i.n   a10, a3, 0
        l32r     a3, .Lsec_18_d4
        memw
        l32i.n   a11, a3, 0
        l32r     a3, .Lsec_18_d8
        memw
        l32i.n   a12, a3, 0
        l32r     a3, .Lsec_18_dc
        memw
        l32i.n   a13, a3, 0
        l32r     a3, .Lsec_18_e0
        memw
        l32i.n   a3, a3, 0
        s32i.n   a13, a1, 20
        l32r     a13, .Lsec_18_e4
        s32i.n   a12, a1, 16
        movi.n   a12, 3
        s32i.n   a11, a1, 12
        mov.n    a11, a2
        s32i.n   a10, a1, 8
        movi.n   a10, 6
        s32i.n   a9, a1, 4
        s32i.n   a8, a1, 0
        s32i.n   a3, a1, 24
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_18_104
        callx8   a8
        retw.n
        .size   dbg_lmac_hw_statis_dump, .-dbg_lmac_hw_statis_dump
.Lsec_18_2ff:

        .section .rodata.dbg_lmac_diag_statis_dump.str1.1,"aMS",@progbits,1
        .align  1
.Lsec_19_0:
        .ascii  "diag1\000"
.Lsec_19_6:
        .ascii  "diag0\000"
.Lsec_19_c:
        .ascii  "diag6\000"
.Lsec_19_12:
        .ascii  "diag5\000"
.Lsec_19_18:
        .ascii  "diag4\000"
.Lsec_19_1e:
        .ascii  "diag3\000"
.Lsec_19_24:
        .ascii  "diag2\000"
.Lsec_19_2a:
        .ascii  "diag8\000"
.Lsec_19_30:
        .ascii  "diag7\000"
.Lsec_19_36:
        .ascii  "diagsel\000"
.Lsec_19_3e:
        .ascii  "diag12\000"
.Lsec_19_45:
        .ascii  "diag10\000"
.Lsec_19_4c:
        .ascii  "diag9\000"

        .section .text.dbg_lmac_diag_statis_dump,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_20_0, .Lsec_47_0
        .literal .Lsec_20_4, .Lsec_46_0
        .literal .Lsec_20_8, .Lsec_19_0
        .literal .Lsec_20_c, .Lsec_19_6
        .literal .Lsec_20_10, .Lsec_45_0
        .literal .Lsec_20_14, .Lsec_19_c
        .literal .Lsec_20_18, .Lsec_19_12
        .literal .Lsec_20_1c, .Lsec_19_18
        .literal .Lsec_20_20, .Lsec_19_1e
        .literal .Lsec_20_24, .Lsec_19_24
        .literal .Lsec_20_28, 0x3ff73dac
        .literal .Lsec_20_2c, 0x3ff73db0
        .literal .Lsec_20_30, 0x3ff73db4
        .literal .Lsec_20_34, 0x3ff73db8
        .literal .Lsec_20_38, 0x3ff732f0
        .literal .Lsec_20_3c, 0x3ff732f4
        .literal .Lsec_20_40, 0x3ff732f8
        .literal .Lsec_20_44, .Lsec_44_0
        .literal .Lsec_20_48, .Lsec_19_2a
        .literal .Lsec_20_4c, .Lsec_19_30
        .literal .Lsec_20_50, .Lsec_43_0
        .literal .Lsec_20_54, .Lsec_19_36
        .literal .Lsec_20_58, .Lsec_19_3e
        .literal .Lsec_20_5c, .Lsec_19_45
        .literal .Lsec_20_60, .Lsec_19_4c
        .literal .Lsec_20_64, 0x3ff732fc
        .literal .Lsec_20_68, 0x3ff73424
        .literal .Lsec_20_6c, 0x3ff73428
        .literal .Lsec_20_70, 0x3ff7342c
        .literal .Lsec_20_74, 0x3ff73430
        .literal .Lsec_20_78, 0x3ff73dbc
        .literal .Lsec_20_7c, .Lsec_42_0
        .literal .Lsec_20_80, wifi_log
        .literal .Lsec_20_84, wifi_log
        .literal .Lsec_20_88, wifi_log
        .literal .Lsec_20_8c, wifi_log
        .literal .Lsec_20_90, wifi_log
        .literal .Lsec_20_94, wifi_log
.Lsec_20_98:
        .global dbg_lmac_diag_statis_dump
        .type   dbg_lmac_diag_statis_dump, @function
dbg_lmac_diag_statis_dump:
        entry    a1, 64
        movi.n   a2, 1
        slli     a2, a2, 19
        l32r     a13, .Lsec_20_0
        mov.n    a11, a2
        movi     a12, 3
        movi     a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_20_80
        callx8   a8
        l32r     a13, .Lsec_20_4
        mov.n    a11, a2
        movi.n   a12, 3
        movi.n   a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_20_84
        callx8   a8
        l32r     a8, .Lsec_20_14
        mov.n    a11, a2
        movi.n   a12, 3
        movi.n   a10, 6
        s32i.n   a8, a1, 16
        l32r     a8, .Lsec_20_18
        s32i.n   a8, a1, 12
        l32r     a8, .Lsec_20_1c
        s32i.n   a8, a1, 8
        l32r     a8, .Lsec_20_20
        s32i.n   a8, a1, 4
        l32r     a8, .Lsec_20_24
        l32r     a15, .Lsec_20_8
        l32r     a14, .Lsec_20_c
        l32r     a13, .Lsec_20_10
        s32i.n   a8, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_20_88
        callx8   a8
        l32r     a8, .Lsec_20_28
        memw
        l32i.n   a14, a8, 0
        l32r     a8, .Lsec_20_2c
        l32r     a9, .Lsec_20_34
        memw
        l32i.n   a15, a8, 0
        l32r     a10, .Lsec_20_38
        l32r     a8, .Lsec_20_30
        l32r     a11, .Lsec_20_3c
        l32r     a12, .Lsec_20_40
        memw
        l32i.n   a8, a8, 0
        memw
        l32i.n   a9, a9, 0
        memw
        l32i.n   a10, a10, 0
        memw
        l32i.n   a11, a11, 0
        memw
        l32i.n   a12, a12, 0
        l32r     a13, .Lsec_20_44
        s32i.n   a9, a1, 4
        s32i.n   a12, a1, 16
        movi.n   a12, 3
        s32i.n   a11, a1, 12
        mov.n    a11, a2
        s32i.n   a10, a1, 8
        movi.n   a10, 6
        s32i     a8, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_20_8c
        callx8   a8
        mov.n    a11, a2
        movi.n   a12, 3
        movi.n   a10, 6
        l32r     a8, .Lsec_20_54
        s32i.n   a8, a1, 12
        l32r     a8, .Lsec_20_58
        s32i.n   a8, a1, 8
        l32r     a8, .Lsec_20_5c
        s32i.n   a8, a1, 4
        l32r     a8, .Lsec_20_60
        l32r     a15, .Lsec_20_48
        l32r     a14, .Lsec_20_4c
        l32r     a13, .Lsec_20_50
        s32i.n   a8, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_20_90
        callx8   a8
        movi.n   a12, 3
        l32r     a8, .Lsec_20_64
        memw
        l32i.n   a14, a8, 0
        l32r     a8, .Lsec_20_68
        l32r     a9, .Lsec_20_70
        memw
        l32i.n   a15, a8, 0
        l32r     a10, .Lsec_20_74
        l32r     a8, .Lsec_20_6c
        l32r     a11, .Lsec_20_78
        memw
        l32i.n   a8, a8, 0
        memw
        l32i.n   a9, a9, 0
        memw
        l32i.n   a10, a10, 0
        memw
        l32i.n   a11, a11, 0
        l32r     a13, .Lsec_20_7c
        s32i.n   a11, a1, 12
        mov.n    a11, a2
        s32i.n   a10, a1, 8
        movi.n   a10, 6
        s32i.n   a9, a1, 4
        s32i.n   a8, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_20_94
        callx8   a8
        retw.n
        .size   dbg_lmac_diag_statis_dump, .-dbg_lmac_diag_statis_dump
.Lsec_20_1be:

        .section .rodata.dbg_lmac_ps_statis_dump.str1.1,"aMS",@progbits,1
        .align  1
.Lsec_21_0:
        .ascii  "beacon\000"
.Lsec_21_7:
        .ascii  "tbtt\000"
.Lsec_21_c:
        .ascii  "percent\000"
.Lsec_21_14:
        .ascii  "totalt\000"
.Lsec_21_1b:
        .ascii  "waket\000"
.Lsec_21_21:
        .ascii  "sleept\000"
.Lsec_21_28:
        .ascii  "wake\000"
.Lsec_21_2d:
        .ascii  "sleep\000"
.Lsec_21_33:
        .ascii  "bcndly\000"
.Lsec_21_3a:
        .ascii  "null0\000"
.Lsec_21_40:
        .ascii  "null1\000"
.Lsec_21_46:
        .ascii  "txwake\000"
.Lsec_21_4d:
        .ascii  "actto\000"
.Lsec_21_53:
        .ascii  "tbttdrm\000"
.Lsec_21_5b:
        .ascii  "dtimset\000"
.Lsec_21_63:
        .ascii  "timset\000"

        .section .text.dbg_lmac_ps_statis_dump,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_22_0, .Lsec_41_0
        .literal .Lsec_22_4, .Lsec_40_0
        .literal .Lsec_22_8, .Lsec_21_0
        .literal .Lsec_22_c, .Lsec_21_7
        .literal .Lsec_22_10, .Lsec_39_0
        .literal .Lsec_22_14, .Lsec_21_c
        .literal .Lsec_22_18, .Lsec_21_14
        .literal .Lsec_22_1c, .Lsec_21_1b
        .literal .Lsec_22_20, .Lsec_21_21
        .literal .Lsec_22_24, .Lsec_21_28
        .literal .Lsec_22_28, .Lsec_21_2d
        .literal .Lsec_22_2c, .Lsec_21_33
        .literal .Lsec_22_30, .Lsec_21_3a
        .literal .Lsec_22_34, .Lsec_21_40
        .literal .Lsec_22_38, .Lsec_21_46
        .literal .Lsec_22_3c, .Lsec_21_4d
        .literal .Lsec_22_40, .Lsec_21_53
        .literal .Lsec_22_44, .Lsec_21_5b
        .literal .Lsec_22_48, .Lsec_21_63
        .literal .Lsec_22_4c, g_pm_cnt
        .literal .Lsec_22_50, .Lsec_38_0
        .literal .Lsec_22_54, 0x42c80000
        .literal .Lsec_22_58, wifi_log
        .literal .Lsec_22_5c, wifi_log
        .literal .Lsec_22_60, wifi_log
        .literal .Lsec_22_64, __floatundisf
        .literal .Lsec_22_68, __floatundisf
        .literal .Lsec_22_6c, __divsf3
        .literal .Lsec_22_70, __extendsfdf2
        .literal .Lsec_22_74, wifi_log
.Lsec_22_78:
        .global dbg_lmac_ps_statis_dump
        .type   dbg_lmac_ps_statis_dump, @function
dbg_lmac_ps_statis_dump:
        entry    a1, 112
        movi.n   a3, 1
        slli     a3, a3, 19
        l32r     a13, .Lsec_22_0
        mov.n    a11, a3
        movi     a12, 3
        movi     a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_22_58
        callx8   a8
        l32r     a13, .Lsec_22_4
        mov.n    a11, a3
        movi.n   a12, 3
        movi.n   a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_22_5c
        callx8   a8
        l32r     a2, .Lsec_22_14
        movi.n   a12, 3
        mov.n    a11, a3
        movi.n   a10, 6
        s32i.n   a2, a1, 52
        l32r     a2, .Lsec_22_18
        s32i.n   a2, a1, 48
        l32r     a2, .Lsec_22_1c
        s32i.n   a2, a1, 44
        l32r     a2, .Lsec_22_20
        s32i.n   a2, a1, 40
        l32r     a2, .Lsec_22_24
        s32i.n   a2, a1, 36
        l32r     a2, .Lsec_22_28
        s32i.n   a2, a1, 32
        l32r     a2, .Lsec_22_2c
        s32i.n   a2, a1, 28
        l32r     a2, .Lsec_22_30
        s32i.n   a2, a1, 24
        l32r     a2, .Lsec_22_34
        s32i.n   a2, a1, 20
        l32r     a2, .Lsec_22_38
        s32i.n   a2, a1, 16
        l32r     a2, .Lsec_22_3c
        s32i.n   a2, a1, 12
        l32r     a2, .Lsec_22_40
        s32i     a2, a1, 8
        l32r     a2, .Lsec_22_44
        s32i     a2, a1, 4
        l32r     a2, .Lsec_22_48
        l32r     a15, .Lsec_22_8
        l32r     a14, .Lsec_22_c
        l32r     a13, .Lsec_22_10
        s32i     a2, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_22_60
        callx8   a8
        l32r     a2, .Lsec_22_4c
        l32i.n   a4, a2, 48
        mov.n    a10, a4
        l32i.n   a11, a2, 52
        .reloc  ., R_XTENSA_ASM_EXPAND, __floatundisf
        l32r     a8, .Lsec_22_64
        callx8   a8
        wfr      f0, a10
        l32r     a8, .Lsec_22_54
        wfr      f1, a8
        mul.s    f0, f0, f1
        l32i     a5, a2, 64
        mov.n    a10, a5
        l32i     a6, a2, 68
        mov.n    a11, a6
        ssi      f0, a1, 64
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, __floatundisf
        l32r     a8, .Lsec_22_68
        callx8   a8
        mov.n    a11, a10
        l32i     a10, a1, 64
        .reloc  ., R_XTENSA_ASM_EXPAND, __divsf3
        l32r     a8, .Lsec_22_6c
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, __extendsfdf2
        l32r     a8, .Lsec_22_70
        callx8   a8
        movi.n   a12, 3
        s32i.n   a10, a1, 56
        movi.n   a10, 6
        s32i.n   a11, a1, 60
        mov.n    a11, a3
        s32i.n   a5, a1, 48
        l32i.n   a5, a2, 56
        s32i.n   a4, a1, 40
        s32i.n   a5, a1, 44
        l32i.n   a4, a2, 44
        s32i.n   a4, a1, 36
        l32i.n   a4, a2, 40
        s32i.n   a4, a1, 32
        l32i.n   a4, a2, 36
        s32i.n   a4, a1, 28
        l32i.n   a4, a2, 32
        s32i.n   a4, a1, 24
        l32i.n   a4, a2, 28
        s32i.n   a4, a1, 20
        l32i.n   a4, a2, 24
        s32i.n   a4, a1, 16
        l32i.n   a4, a2, 20
        s32i.n   a4, a1, 12
        l32i.n   a4, a2, 16
        s32i.n   a4, a1, 8
        l32i.n   a4, a2, 12
        s32i.n   a4, a1, 4
        l32i.n   a4, a2, 8
        s32i.n   a4, a1, 0
        l32i.n   a15, a2, 4
        l32i.n   a14, a2, 0
        l32r     a13, .Lsec_22_50
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_22_74
        callx8   a8
        retw.n
        .size   dbg_lmac_ps_statis_dump, .-dbg_lmac_ps_statis_dump
.Lsec_22_18f:

        .section .text.dbg_lmac_ps_statis_reset,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_23_0, g_pm_cnt
        .literal .Lsec_23_4, memset
.Lsec_23_8:
        .global dbg_lmac_ps_statis_reset
        .type   dbg_lmac_ps_statis_reset, @function
dbg_lmac_ps_statis_reset:
        entry    a1, 32
        l32r     a10, .Lsec_23_0
        movi.n   a12, 72
        movi.n   a11, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, memset
        l32r     a8, .Lsec_23_4
        callx8   a8
        retw.n
        .size   dbg_lmac_ps_statis_reset, .-dbg_lmac_ps_statis_reset
.Lsec_23_1a:

        .section .text.dbg_lmac_statis_dump,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_24_0, esf_buf_statis_dump
        .literal .Lsec_24_4, dbg_lmac_rxtx_statis_dump
        .literal .Lsec_24_8, dbg_lmac_hw_statis_dump
        .literal .Lsec_24_c, dbg_lmac_diag_statis_dump
        .literal .Lsec_24_10, dbg_lmac_ps_statis_dump
.Lsec_24_14:
        .global dbg_lmac_statis_dump
        .type   dbg_lmac_statis_dump, @function
dbg_lmac_statis_dump:
        entry    a1, 32
        bbci     a2, 0, .Lsec_24_20
        .reloc  ., R_XTENSA_ASM_EXPAND, esf_buf_statis_dump
        l32r     a8, .Lsec_24_0
        callx8   a8
.Lsec_24_20:
        bbci     a2, 1, .Lsec_24_29
        .reloc  ., R_XTENSA_ASM_EXPAND, dbg_lmac_rxtx_statis_dump
        l32r     a8, .Lsec_24_4
        callx8   a8
.Lsec_24_29:
        bbci     a2, 2, .Lsec_24_32
        .reloc  ., R_XTENSA_ASM_EXPAND, dbg_lmac_hw_statis_dump
        l32r     a8, .Lsec_24_8
        callx8   a8
.Lsec_24_32:
        bbci     a2, 3, .Lsec_24_3b
        .reloc  ., R_XTENSA_ASM_EXPAND, dbg_lmac_diag_statis_dump
        l32r     a8, .Lsec_24_c
        callx8   a8
.Lsec_24_3b:
        bbci     a2, 4, .Lsec_24_44
        .reloc  ., R_XTENSA_ASM_EXPAND, dbg_lmac_ps_statis_dump
        l32r     a8, .Lsec_24_10
        callx8   a8
.Lsec_24_44:
        retw.n
        .size   dbg_lmac_statis_dump, .-dbg_lmac_statis_dump
.Lsec_24_46:

        .section .text.dbg_his_lmac_eb_show,"ax",@progbits
        .align  4
.Lsec_25_0:
        .global dbg_his_lmac_eb_show
        .type   dbg_his_lmac_eb_show, @function
dbg_his_lmac_eb_show:
        entry    a1, 32
        retw.n
        .size   dbg_his_lmac_eb_show, .-dbg_his_lmac_eb_show
.Lsec_25_5:

        .section .text.dbg_his_lmac_event_show,"ax",@progbits
        .align  4
.Lsec_26_0:
        .global dbg_his_lmac_event_show
        .type   dbg_his_lmac_event_show, @function
dbg_his_lmac_event_show:
        entry    a1, 32
        retw.n
        .size   dbg_his_lmac_event_show, .-dbg_his_lmac_event_show
.Lsec_26_5:

        .section .text.dbg_his_lmac_int_show,"ax",@progbits
        .align  4
.Lsec_27_0:
        .global dbg_his_lmac_int_show
        .type   dbg_his_lmac_int_show, @function
dbg_his_lmac_int_show:
        entry    a1, 32
        retw.n
        .size   dbg_his_lmac_int_show, .-dbg_his_lmac_int_show
.Lsec_27_5:

        .section .text.dbg_his_lmac_rx_show,"ax",@progbits
        .align  4
.Lsec_28_0:
        .global dbg_his_lmac_rx_show
        .type   dbg_his_lmac_rx_show, @function
dbg_his_lmac_rx_show:
        entry    a1, 32
        retw.n
        .size   dbg_his_lmac_rx_show, .-dbg_his_lmac_rx_show
.Lsec_28_5:

        .section .text.dbg_his_lmac_tx_show,"ax",@progbits
        .align  4
.Lsec_29_0:
        .global dbg_his_lmac_tx_show
        .type   dbg_his_lmac_tx_show, @function
dbg_his_lmac_tx_show:
        entry    a1, 32
        retw.n
        .size   dbg_his_lmac_tx_show, .-dbg_his_lmac_tx_show
.Lsec_29_5:

        .section .text.dbg_ebuf_loc_show,"ax",@progbits
        .align  4
.Lsec_30_0:
        .global dbg_ebuf_loc_show
        .type   dbg_ebuf_loc_show, @function
dbg_ebuf_loc_show:
        entry    a1, 32
        retw.n
        .size   dbg_ebuf_loc_show, .-dbg_ebuf_loc_show
.Lsec_30_5:

        .section .text.dbg_perf_path_show,"ax",@progbits
        .align  4
.Lsec_31_0:
        .global dbg_perf_path_show
        .type   dbg_perf_path_show, @function
dbg_perf_path_show:
        entry    a1, 32
        retw.n
        .size   dbg_perf_path_show, .-dbg_perf_path_show
.Lsec_31_5:

        .section .text.dbg_perf_path_set,"ax",@progbits
        .align  4
.Lsec_32_0:
        .global dbg_perf_path_set
        .type   dbg_perf_path_set, @function
dbg_perf_path_set:
        entry    a1, 32
        retw.n
        .size   dbg_perf_path_set, .-dbg_perf_path_set
.Lsec_32_5:

        .section .text.dbg_perf_throughput_cal,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_33_0, g_mesh_is_started
        .literal .Lsec_33_4, g_mesh_is_root
        .literal .Lsec_33_8, g_mesh_init_ps_type
        .literal .Lsec_33_c, g_osi_funcs_p
        .literal .Lsec_33_10, 0x3ff73c00
        .literal .Lsec_33_14, g_wifi_mac_time_delta
        .literal .Lsec_33_18, .Lsec_37_0
        .literal .Lsec_33_1c, .Lsec_36_0
        .literal .Lsec_33_20, 0x007a11ff
        .literal .Lsec_33_24, .Lsec_35_0
        .literal .Lsec_33_28, wifi_log
.Lsec_33_2c:
        .global dbg_perf_throughput_cal
        .type   dbg_perf_throughput_cal, @function
dbg_perf_throughput_cal:
        entry    a1, 32
        l32r     a3, .Lsec_33_0
        l8ui     a3, a3, 0
        beqz.n   a3, .Lsec_33_5c
        l32r     a3, .Lsec_33_4
        memw
        l8ui     a3, a3, 0
        extui    a3, a3, 0, 8
        bnez.n   a3, .Lsec_33_5c
        l32r     a3, .Lsec_33_8
        l32i.n   a3, a3, 0
        beqz.n   a3, .Lsec_33_5c
        l32r     a3, .Lsec_33_c
        l32i     a3, a3, 0
        l32i     a10, a3, 0x110
        callx8   a10
        j        .Lsec_33_6b
        .byte    0x00
.Lsec_33_5c:
        l32r     a3, .Lsec_33_10
        memw
        l32i.n   a10, a3, 0
        l32r     a3, .Lsec_33_14
        l32i.n   a3, a3, 0
        add.n    a10, a10, a3
.Lsec_33_6b:
        l32r     a3, .Lsec_33_18
        l32i.n   a8, a3, 0
        add.n    a14, a2, a8
        l32r     a8, .Lsec_33_1c
        l32i.n   a2, a8, 0
        sub      a2, a10, a2
        l32r     a9, .Lsec_33_20
        s32i.n   a14, a3, 0
        bgeu     a9, a2, .Lsec_33_9c
        movi.n   a12, 3
        movi.n   a11, 1
        movi.n   a2, 0
        l32r     a13, .Lsec_33_24
        s32i     a10, a8, 0
        movi     a10, 6
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_33_28
        callx8   a8
        s32i.n   a2, a3, 0
.Lsec_33_9c:
        memw
        retw.n
        .size   dbg_perf_throughput_cal, .-dbg_perf_throughput_cal
.Lsec_33_a1:

        .section .text.dbg_lmac_init,"ax",@progbits
        .align  4
.Lsec_34_0:
        .global dbg_lmac_init
        .type   dbg_lmac_init, @function
dbg_lmac_init:
        entry    a1, 32
        retw.n
        .size   dbg_lmac_init, .-dbg_lmac_init
.Lsec_34_5:

        .section .rodata_wlog_info.35,"a",@progbits
        .align  1
.Lsec_35_0:
        .byte   0x25, 0x64, 0x00

        .section .bss.s_t_old,"aw",@nobits
        .align  4
.Lsec_36_0:
        .skip   4

        .section .bss.s_total,"aw",@nobits
        .align  4
.Lsec_37_0:
        .skip   4

        .section .rodata_wlog_info.34,"a",@progbits
        .align  1
.Lsec_38_0:
        .byte   0x20, 0x25, 0x2d, 0x38, 0x75, 0x20, 0x25, 0x2d, 0x38, 0x75, 0x20, 0x25, 0x2d, 0x38, 0x75, 0x20
        .byte   0x25, 0x2d, 0x38, 0x75, 0x20, 0x25, 0x2d, 0x38, 0x75, 0x20, 0x25, 0x2d, 0x38, 0x75, 0x20, 0x25
        .byte   0x2d, 0x38, 0x75, 0x20, 0x25, 0x2d, 0x38, 0x75, 0x20, 0x25, 0x2d, 0x38, 0x75, 0x20, 0x25, 0x2d
        .byte   0x38, 0x75, 0x20, 0x25, 0x2d, 0x38, 0x75, 0x20, 0x25, 0x2d, 0x38, 0x75, 0x20, 0x25, 0x2d, 0x38
        .byte   0x75, 0x20, 0x25, 0x2d, 0x38, 0x75, 0x20, 0x25, 0x2d, 0x38, 0x75, 0x20, 0x25, 0x2d, 0x36, 0x2e
        .byte   0x32, 0x66, 0x25, 0x25, 0x00

        .section .rodata_wlog_info.33,"a",@progbits
        .align  1
.Lsec_39_0:
        .byte   0x20, 0x25, 0x2d, 0x38, 0x73, 0x20, 0x25, 0x2d, 0x38, 0x73, 0x20, 0x25, 0x2d, 0x38, 0x73, 0x20
        .byte   0x25, 0x2d, 0x38, 0x73, 0x20, 0x25, 0x2d, 0x38, 0x73, 0x20, 0x25, 0x2d, 0x38, 0x73, 0x20, 0x25
        .byte   0x2d, 0x38, 0x73, 0x20, 0x25, 0x2d, 0x38, 0x73, 0x20, 0x25, 0x2d, 0x38, 0x73, 0x20, 0x25, 0x2d
        .byte   0x38, 0x73, 0x20, 0x25, 0x2d, 0x38, 0x73, 0x20, 0x25, 0x2d, 0x38, 0x73, 0x20, 0x25, 0x2d, 0x38
        .byte   0x73, 0x20, 0x25, 0x2d, 0x38, 0x73, 0x20, 0x25, 0x2d, 0x38, 0x73, 0x20, 0x25, 0x2d, 0x38, 0x73
        .byte   0x00

        .section .rodata_wlog_info.32,"a",@progbits
        .align  1
.Lsec_40_0:
        .byte   0x20, 0x50, 0x6f, 0x77, 0x65, 0x72, 0x20, 0x73, 0x61, 0x76, 0x65, 0x3a, 0x00

        .section .rodata_wlog_info.31,"a",@progbits
        .align  1
.Lsec_41_0:
        .byte   0x00

        .section .rodata_wlog_info.30,"a",@progbits
        .align  1
.Lsec_42_0:
        .byte   0x20, 0x25, 0x2d, 0x38, 0x78, 0x20, 0x25, 0x2d, 0x38, 0x78, 0x20, 0x25, 0x2d, 0x38, 0x78, 0x20
        .byte   0x25, 0x2d, 0x38, 0x78, 0x20, 0x25, 0x2d, 0x38, 0x78, 0x20, 0x25, 0x2d, 0x38, 0x78, 0x00

        .section .rodata_wlog_info.29,"a",@progbits
        .align  1
.Lsec_43_0:
        .byte   0x20, 0x25, 0x2d, 0x38, 0x73, 0x20, 0x25, 0x2d, 0x38, 0x73, 0x20, 0x25, 0x2d, 0x38, 0x73, 0x20
        .byte   0x25, 0x2d, 0x38, 0x73, 0x20, 0x25, 0x2d, 0x38, 0x73, 0x20, 0x25, 0x2d, 0x38, 0x73, 0x00

        .section .rodata_wlog_info.28,"a",@progbits
        .align  1
.Lsec_44_0:
        .byte   0x20, 0x25, 0x2d, 0x38, 0x78, 0x20, 0x25, 0x2d, 0x38, 0x78, 0x20, 0x25, 0x2d, 0x38, 0x78, 0x20
        .byte   0x25, 0x2d, 0x38, 0x78, 0x20, 0x25, 0x2d, 0x38, 0x78, 0x20, 0x25, 0x2d, 0x38, 0x78, 0x20, 0x25
        .byte   0x2d, 0x38, 0x78, 0x00

        .section .rodata_wlog_info.27,"a",@progbits
        .align  1
.Lsec_45_0:
        .byte   0x20, 0x25, 0x2d, 0x38, 0x73, 0x20, 0x25, 0x2d, 0x38, 0x73, 0x20, 0x25, 0x2d, 0x38, 0x73, 0x20
        .byte   0x25, 0x2d, 0x38, 0x73, 0x20, 0x25, 0x2d, 0x38, 0x73, 0x20, 0x25, 0x2d, 0x38, 0x73, 0x20, 0x25
        .byte   0x2d, 0x38, 0x73, 0x00

        .section .rodata_wlog_info.26,"a",@progbits
        .align  1
.Lsec_46_0:
        .byte   0x20, 0x44, 0x49, 0x41, 0x47, 0x3a, 0x00

        .section .rodata_wlog_info.25,"a",@progbits
        .align  1
.Lsec_47_0:
        .byte   0x00

        .section .rodata_wlog_info.24,"a",@progbits
        .align  1
.Lsec_48_0:
        .byte   0x20, 0x25, 0x2d, 0x38, 0x78, 0x20, 0x25, 0x2d, 0x38, 0x78, 0x20, 0x25, 0x2d, 0x38, 0x78, 0x20
        .byte   0x25, 0x2d, 0x38, 0x78, 0x20, 0x25, 0x2d, 0x38, 0x78, 0x20, 0x25, 0x2d, 0x38, 0x78, 0x20, 0x25
        .byte   0x2d, 0x38, 0x78, 0x20, 0x25, 0x2d, 0x38, 0x78, 0x20, 0x25, 0x2d, 0x38, 0x78, 0x00

        .section .rodata_wlog_info.23,"a",@progbits
        .align  1
.Lsec_49_0:
        .byte   0x20, 0x25, 0x2d, 0x38, 0x73, 0x20, 0x25, 0x2d, 0x38, 0x73, 0x20, 0x25, 0x2d, 0x38, 0x73, 0x20
        .byte   0x25, 0x2d, 0x38, 0x73, 0x20, 0x25, 0x2d, 0x38, 0x73, 0x20, 0x25, 0x2d, 0x38, 0x73, 0x20, 0x25
        .byte   0x2d, 0x38, 0x73, 0x20, 0x25, 0x2d, 0x38, 0x73, 0x20, 0x25, 0x2d, 0x38, 0x73, 0x00

        .section .rodata_wlog_info.22,"a",@progbits
        .align  1
.Lsec_50_0:
        .byte   0x20, 0x25, 0x2d, 0x38, 0x78, 0x20, 0x25, 0x2d, 0x38, 0x78, 0x20, 0x25, 0x2d, 0x38, 0x78, 0x20
        .byte   0x25, 0x2d, 0x38, 0x78, 0x20, 0x25, 0x2d, 0x38, 0x78, 0x20, 0x25, 0x2d, 0x38, 0x78, 0x20, 0x25
        .byte   0x2d, 0x38, 0x78, 0x20, 0x25, 0x2d, 0x38, 0x78, 0x00

        .section .rodata_wlog_info.21,"a",@progbits
        .align  1
.Lsec_51_0:
        .byte   0x20, 0x25, 0x2d, 0x38, 0x73, 0x20, 0x25, 0x2d, 0x38, 0x73, 0x20, 0x25, 0x2d, 0x38, 0x73, 0x20
        .byte   0x25, 0x2d, 0x38, 0x73, 0x20, 0x25, 0x2d, 0x38, 0x73, 0x20, 0x25, 0x2d, 0x38, 0x73, 0x20, 0x25
        .byte   0x2d, 0x38, 0x73, 0x20, 0x25, 0x2d, 0x38, 0x73, 0x00

        .section .rodata_wlog_info.20,"a",@progbits
        .align  1
.Lsec_52_0:
        .byte   0x20, 0x25, 0x2d, 0x38, 0x78, 0x20, 0x25, 0x2d, 0x38, 0x78, 0x20, 0x25, 0x2d, 0x38, 0x78, 0x20
        .byte   0x25, 0x2d, 0x38, 0x78, 0x20, 0x25, 0x2d, 0x38, 0x78, 0x20, 0x25, 0x2d, 0x38, 0x78, 0x20, 0x25
        .byte   0x2d, 0x38, 0x78, 0x20, 0x25, 0x2d, 0x38, 0x78, 0x00

        .section .rodata_wlog_info.19,"a",@progbits
        .align  1
.Lsec_53_0:
        .byte   0x20, 0x25, 0x2d, 0x38, 0x73, 0x20, 0x25, 0x2d, 0x38, 0x73, 0x20, 0x25, 0x2d, 0x38, 0x73, 0x20
        .byte   0x25, 0x2d, 0x38, 0x73, 0x20, 0x25, 0x2d, 0x38, 0x73, 0x20, 0x25, 0x2d, 0x38, 0x73, 0x20, 0x25
        .byte   0x2d, 0x38, 0x73, 0x20, 0x25, 0x2d, 0x38, 0x73, 0x00

        .section .rodata_wlog_info.18,"a",@progbits
        .align  1
.Lsec_54_0:
        .byte   0x20, 0x48, 0x61, 0x72, 0x64, 0x77, 0x61, 0x72, 0x65, 0x3a, 0x00

        .section .rodata_wlog_info.17,"a",@progbits
        .align  1
.Lsec_55_0:
        .byte   0x00

        .section .rodata_wlog_info.16,"a",@progbits
        .align  1
.Lsec_56_0:
        .byte   0x20, 0x25, 0x2d, 0x38, 0x78, 0x20, 0x25, 0x2d, 0x38, 0x78, 0x20, 0x25, 0x2d, 0x38, 0x78, 0x20
        .byte   0x25, 0x2d, 0x38, 0x78, 0x00

        .section .rodata_wlog_info.15,"a",@progbits
        .align  1
.Lsec_57_0:
        .byte   0x20, 0x25, 0x2d, 0x38, 0x73, 0x20, 0x25, 0x2d, 0x38, 0x73, 0x20, 0x25, 0x2d, 0x38, 0x73, 0x20
        .byte   0x25, 0x2d, 0x38, 0x73, 0x00

        .section .rodata_wlog_info.14,"a",@progbits
        .align  1
.Lsec_58_0:
        .byte   0x20, 0x48, 0x57, 0x20, 0x52, 0x58, 0x3a, 0x00

        .section .rodata_wlog_info.13,"a",@progbits
        .align  1
.Lsec_59_0:
        .byte   0x00

        .section .rodata_wlog_info.12,"a",@progbits
        .align  1
.Lsec_60_0:
        .byte   0x20, 0x25, 0x2d, 0x38, 0x75, 0x20, 0x25, 0x2d, 0x38, 0x75, 0x20, 0x25, 0x2d, 0x38, 0x75, 0x20
        .byte   0x25, 0x2d, 0x38, 0x75, 0x20, 0x25, 0x2d, 0x34, 0x75, 0x00

        .section .rodata_wlog_info.11,"a",@progbits
        .align  1
.Lsec_61_0:
        .byte   0x20, 0x25, 0x2d, 0x38, 0x73, 0x20, 0x25, 0x2d, 0x38, 0x73, 0x20, 0x25, 0x2d, 0x38, 0x73, 0x20
        .byte   0x25, 0x2d, 0x38, 0x73, 0x20, 0x25, 0x2d, 0x34, 0x73, 0x00

        .section .rodata_wlog_info.10,"a",@progbits
        .align  1
.Lsec_62_0:
        .byte   0x20, 0x4c, 0x4d, 0x41, 0x43, 0x20, 0x52, 0x58, 0x3a, 0x00

        .section .rodata_wlog_info.9,"a",@progbits
        .align  1
.Lsec_63_0:
        .byte   0x00

        .section .rodata_wlog_info.8,"a",@progbits
        .align  1
.Lsec_64_0:
        .byte   0x20, 0x25, 0x2d, 0x38, 0x75, 0x20, 0x25, 0x2d, 0x33, 0x75, 0x20, 0x25, 0x2d, 0x33, 0x75, 0x20
        .byte   0x25, 0x2d, 0x33, 0x75, 0x20, 0x25, 0x2d, 0x33, 0x75, 0x00

        .section .rodata_wlog_info.7,"a",@progbits
        .align  1
.Lsec_65_0:
        .byte   0x20, 0x25, 0x2d, 0x38, 0x73, 0x20, 0x25, 0x2d, 0x33, 0x73, 0x20, 0x25, 0x2d, 0x33, 0x73, 0x20
        .byte   0x25, 0x2d, 0x33, 0x73, 0x20, 0x25, 0x2d, 0x33, 0x73, 0x00

        .section .rodata_wlog_info.6,"a",@progbits
        .align  1
.Lsec_66_0:
        .byte   0x20, 0x4c, 0x4d, 0x41, 0x43, 0x20, 0x54, 0x58, 0x3a, 0x00

        .section .rodata_wlog_info.5,"a",@progbits
        .align  1
.Lsec_67_0:
        .byte   0x00

        .section .bss.g_pm_cnt,"aw",@nobits
        .align  8
.Lsec_68_0:
        .global g_pm_cnt
        .type   g_pm_cnt, @object
        .size   g_pm_cnt, 72
g_pm_cnt:
        .skip   72

        .section .bss.g_lmac_cnt,"aw",@nobits
        .align  4
.Lsec_69_0:
        .global g_lmac_cnt
        .type   g_lmac_cnt, @object
        .size   g_lmac_cnt, 180
g_lmac_cnt:
        .skip   180

        .section .dram1.2,"aw",@progbits
        .align  4
.Lsec_70_0:
        .byte   0x00, 0x00, 0x00, 0x00
