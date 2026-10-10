.section .text.offchan_in_progress,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_56_0, offchan_tx_progress_in
.Lsec_56_4:
        entry    a1, 32
        l32r     a8, .Lsec_56_0
        l8ui     a2, a8, 0
        retw.n

        .section .text.lmacIsIdle,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_57_0, .Lsec_170_0
.Lsec_57_4:
        .global lmacIsIdle
        .type   lmacIsIdle, @function
lmacIsIdle:
        entry    a1, 32
        l32r     a8, .Lsec_57_0
        extui    a2, a2, 0, 8
        addx8    a2, a2, a2
        addx4    a2, a2, a8
        movi.n   a8, 0
        l8ui     a9, a2, 18
        movi.n   a2, 1
        movnez   a2, a8, a9
        retw.n
        .size   lmacIsIdle, .-lmacIsIdle
.Lsec_57_1f:

        .section .text.lmacGetTxFrame,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_58_0, .Lsec_170_0
.Lsec_58_4:
        .global lmacGetTxFrame
        .type   lmacGetTxFrame, @function
lmacGetTxFrame:
        entry    a1, 32
        l32r     a8, .Lsec_58_0
        extui    a2, a2, 0, 8
        addx8    a2, a2, a2
        addx4    a8, a2, a8
        movi.n   a2, 0
        l8ui     a9, a8, 18
        beq      a9, a2, .Lsec_58_1d
        l32i.n   a2, a8, 0
.Lsec_58_1d:
        retw.n
        .size   lmacGetTxFrame, .-lmacGetTxFrame
.Lsec_58_1f:

        .section .text.lmacReachShortLimit,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_59_0, lmacConfMib
.Lsec_59_4:
        .global lmacReachShortLimit
        .type   lmacReachShortLimit, @function
lmacReachShortLimit:
        entry    a1, 32
        l32r     a9, .Lsec_59_0
        extui    a2, a2, 0, 8
        movi.n   a8, 1
        l8ui     a9, a9, 21
        bgeu     a2, a9, .Lsec_59_18
        movi     a8, 0
.Lsec_59_18:
        extui    a2, a8, 0, 1
        retw.n
        .size   lmacReachShortLimit, .-lmacReachShortLimit
.Lsec_59_1d:

        .section .text.lmacReachLongLimit,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_60_0, lmacConfMib
.Lsec_60_4:
        .global lmacReachLongLimit
        .type   lmacReachLongLimit, @function
lmacReachLongLimit:
        entry    a1, 32
        l32r     a9, .Lsec_60_0
        extui    a2, a2, 0, 8
        movi.n   a8, 1
        l8ui     a9, a9, 20
        bgeu     a2, a9, .Lsec_60_18
        movi     a8, 0
.Lsec_60_18:
        extui    a2, a8, 0, 1
        retw.n
        .size   lmacReachLongLimit, .-lmacReachLongLimit
.Lsec_60_1d:

        .section .text.lmacIsLongFrame,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_61_0, lmacConfMib
.Lsec_61_4:
        .global lmacIsLongFrame
        .type   lmacIsLongFrame, @function
lmacIsLongFrame:
        entry    a1, 32
        l16ui    a8, a2, 22
        l16ui    a2, a2, 20
        add.n    a8, a8, a2
        l32r     a2, .Lsec_61_0
        l16ui    a9, a2, 22
        movi.n   a2, 1
        blt      a9, a8, .Lsec_61_1c
        movi.n   a2, 0
.Lsec_61_1c:
        extui    a2, a2, 0, 1
        retw.n
        .size   lmacIsLongFrame, .-lmacIsLongFrame
.Lsec_61_21:

        .section .wifi0iram.2,"ax",@progbits
        .align  4
.Lsec_62_0:
        .global lmacProcessLongFrameSuccess
        .type   lmacProcessLongFrameSuccess, @function
lmacProcessLongFrameSuccess:
        entry    a1, 32
        l8ui     a8, a2, 9
        s8i      a8, a2, 8
        movi.n   a8, 0
        s8i      a8, a2, 12
        memw
        memw
        retw.n
        .size   lmacProcessLongFrameSuccess, .-lmacProcessLongFrameSuccess
.Lsec_62_16:

        .section .wifi0iram.3,"ax",@progbits
        .align  4
.Lsec_63_0:
        .global lmacProcessShortFrameSuccess
        .type   lmacProcessShortFrameSuccess, @function
lmacProcessShortFrameSuccess:
        entry    a1, 32
        l8ui     a8, a2, 9
        s8i      a8, a2, 8
        movi.n   a8, 0
        s8i      a8, a2, 11
        memw
        memw
        retw.n
        .size   lmacProcessShortFrameSuccess, .-lmacProcessShortFrameSuccess
.Lsec_63_16:

        .section .wifiextrairam.4,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_64_0, 0xff00ff00
        .literal .Lsec_64_4, lmacConfMib
        .literal .Lsec_64_8, g_mesh_is_started
        .literal .Lsec_64_c, g_mesh_is_root
        .literal .Lsec_64_10, g_mesh_init_ps_type
        .literal .Lsec_64_14, g_osi_funcs_p
        .literal .Lsec_64_18, 0x3ff73c00
        .literal .Lsec_64_1c, g_wifi_mac_time_delta
        .literal .Lsec_64_20, pTxRx
        .literal .Lsec_64_24, wDev_is_low_rate_enable
        .literal .Lsec_64_28, ppProcessLifeTime
        .literal .Lsec_64_2c, hal_mac_tx_config_timeout
        .literal .Lsec_64_30, hal_mac_tx_set_ppdu
.Lsec_64_34:
        .global lmacSetTxFrame
        .type   lmacSetTxFrame, @function
lmacSetTxFrame:
        entry    a1, 48
        extui    a3, a3, 0, 8
        bnez     a3, .Lsec_64_180
        l32i.n   a4, a2, 0
        l32i.n   a3, a4, 44
        l32i.n   a5, a4, 4
        l32i.n   a6, a3, 0
        l32i.n   a5, a5, 4
        bbci     a6, 18, .Lsec_64_4c
        addi.n   a5, a5, 4
.Lsec_64_4c:
        l16ui    a6, a3, 10
        .reloc  ., R_XTENSA_ASM_EXPAND, wDev_is_low_rate_enable
        l32r     a8, .Lsec_64_24
        callx8   a8
        l32i.n   a9, a4, 44
        beqz.n   a10, .Lsec_64_74
        movi.n   a8, 9
        l8ui     a3, a9, 12
        bne      a3, a8, .Lsec_64_69
        movi.n   a3, 14
        j        .Lsec_64_6e
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_64_69:
        bnei     a3, 10, .Lsec_64_74
        movi.n   a3, 15
.Lsec_64_6e:
        s8i      a3, a9, 12
        memw
.Lsec_64_74:
        l32i.n   a3, a9, 0
        bbci     a3, 22, .Lsec_64_80
        memw
        j        .Lsec_64_96
        .byte    0x00
.Lsec_64_80:
        l32i.n   a8, a2, 12
        l32r     a3, .Lsec_64_0
        bany     a8, a3, .Lsec_64_9c
        movi.n   a8, 15
        l8ui     a3, a9, 12
        addi     a3, a3, -16
        extui    a3, a3, 0, 8
        bltu     a8, a3, .Lsec_64_9c
.Lsec_64_96:
        s16i     a6, a5, 2
        memw
.Lsec_64_9c:
        l32r     a5, .Lsec_64_4
        l8ui     a3, a5, 46
        beqz.n   a3, .Lsec_64_cc
        l8ui     a3, a5, 45
        l8ui     a8, a9, 12
        beqz.n   a3, .Lsec_64_bb
        addi     a8, a8, -4
        extui    a8, a8, 0, 8
        bgeui    a8, 4, .Lsec_64_cc
        memw
        j        .Lsec_64_c6
.Lsec_64_bb:
        addi.n   a3, a8, -1
        extui    a3, a3, 0, 8
        bgeui    a3, 3, .Lsec_64_cc
        addi     a8, a8, 4
.Lsec_64_c6:
        s8i      a8, a9, 12
        memw
.Lsec_64_cc:
        l32r     a6, .Lsec_64_8
        l8ui     a3, a6, 0
        beqz.n   a3, .Lsec_64_fd
        l32r     a3, .Lsec_64_c
        memw
        l8ui     a3, a3, 0
        extui    a3, a3, 0, 8
        bnez.n   a3, .Lsec_64_fd
        l32r     a3, .Lsec_64_10
        l32i.n   a3, a3, 0
        beqz.n   a3, .Lsec_64_fd
        l32r     a3, .Lsec_64_14
        l32i     a3, a3, 0
        l32i     a10, a3, 0x110
        memw
        callx8   a10
        mov.n    a3, a10
        j        .Lsec_64_10c
.Lsec_64_fd:
        l32r     a3, .Lsec_64_18
        l32r     a8, .Lsec_64_1c
        memw
        l32i.n   a3, a3, 0
        l32i.n   a8, a8, 0
        add.n    a3, a3, a8
.Lsec_64_10c:
        l8ui     a6, a6, 0
        l32i.n   a8, a4, 44
        beqz.n   a6, .Lsec_64_12c
        l32i.n   a10, a8, 24
        sub      a10, a3, a10
        srli     a10, a10, 10
        l32i.n   a11, a5, 8
        sub      a10, a11, a10
        bltu     a11, a10, .Lsec_64_168
        movi.n   a11, 1
        maxu     a11, a10, a11
        j        .Lsec_64_168
        .byte    0x00
.Lsec_64_12c:
        l32i.n   a6, a8, 0
        bbci     a6, 22, .Lsec_64_136
        l32i.n   a8, a5, 0
        j        .Lsec_64_138
.Lsec_64_136:
        l32i.n   a8, a5, 8
.Lsec_64_138:
        mov.n    a10, a4
        slli     a8, a8, 10
        mov.n    a11, a3
        mov.n    a12, a1
        s32i     a8, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, ppProcessLifeTime
        l32r     a8, .Lsec_64_28
        callx8   a8
        movi.n   a11, 10
        l32i.n   a4, a4, 44
        l32i.n   a5, a1, 0
        l32i.n   a10, a4, 24
        add.n    a10, a5, a10
        sub      a10, a10, a3
        bgeu     a10, a5, .Lsec_64_168
        ssr      a11
        srl      a11, a10
        movi.n   a3, 1
        maxu     a11, a11, a3
.Lsec_64_168:
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_tx_config_timeout
        l32r     a8, .Lsec_64_2c
        callx8   a8
        l32r     a3, .Lsec_64_20
        mov.n    a10, a2
        l32i.n   a11, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_tx_set_ppdu
        l32r     a8, .Lsec_64_30
        callx8   a8
        retw.n
        .byte    0x00
.Lsec_64_180:
        movi.n   a2, 0
        memw
        l32i.n   a2, a2, 44
        break    1, 15
        .size   lmacSetTxFrame, .-lmacSetTxFrame
.Lsec_64_18a:

        .section .text.lmacSetAcParam,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_65_0, .Lsec_170_0
.Lsec_65_4:
        .global lmacSetAcParam
        .type   lmacSetAcParam, @function
lmacSetAcParam:
        entry    a1, 32
        extui    a2, a2, 0, 8
        l32r     a10, .Lsec_65_0
        addx8    a8, a2, a2
        addx4    a8, a8, a10
        extui    a4, a4, 0, 8
        extui    a5, a5, 0, 8
        l8ui     a11, a8, 8
        slli     a9, a2, 3
        s8i      a3, a8, 5
        s8i      a4, a8, 9
        s8i      a5, a8, 10
        memw
        bgeu     a5, a11, .Lsec_65_3c
        s8i      a5, a8, 8
        memw
        memw
        j        .Lsec_65_42
        .byte    0x00
        .byte    0x00
.Lsec_65_3c:
        bgeu     a11, a4, .Lsec_65_42
        s8i      a4, a8, 8
.Lsec_65_42:
        add.n    a9, a9, a2
        addx4    a9, a9, a10
        movi.n   a2, 0
        s16i     a2, a9, 24
        memw
        memw
        retw.n
        .size   lmacSetAcParam, .-lmacSetAcParam
.Lsec_65_54:

        .section .text.lmacInitAc,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_66_0, .Lsec_170_0
.Lsec_66_4:
        .global lmacInitAc
        .type   lmacInitAc, @function
lmacInitAc:
        entry    a1, 32
        extui    a2, a2, 0, 8
        l32r     a9, .Lsec_66_0
        addx8    a8, a2, a2
        addx4    a8, a8, a9
        extui    a4, a4, 0, 8
        movi.n   a9, 0
        s8i      a2, a8, 4
        movi.n   a2, 0
        s8i      a3, a8, 5
        s8i      a4, a8, 9
        s8i      a5, a8, 10
        s8i      a4, a8, 8
        s16i     a9, a8, 24
        s8i      a9, a8, 11
        s8i      a9, a8, 12
        s8i      a9, a8, 18
        memw
        s32i.n   a2, a8, 28
        s32i.n   a2, a8, 32
        memw
        retw.n
        .size   lmacInitAc, .-lmacInitAc
.Lsec_66_41:

        .section .text.lmacInit,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_67_0, lmacConfMib
        .literal .Lsec_67_4, 0x092a2020
        .literal .Lsec_67_8, 0x0fff015f
        .literal .Lsec_67_c, lmacInitAc
        .literal .Lsec_67_10, lmacInitAc
        .literal .Lsec_67_14, lmacInitAc
        .literal .Lsec_67_18, lmacInitAc
        .literal .Lsec_67_1c, lmacInitAc
        .literal .Lsec_67_20, RC_SetBasicRate
        .literal .Lsec_67_24, rcAttach
.Lsec_67_28:
        .global lmacInit
        .type   lmacInit, @function
lmacInit:
        entry    a1, 32
        l32r     a8, .Lsec_67_0
        movi.n   a9, 9
        movi.n   a14, 0
        movi.n   a13, 10
        movi.n   a12, 4
        s16i     a9, a8, 28
        memw
        movi     a9, 0x600
        movi.n   a11, 3
        movi.n   a10, 2
        s32i.n   a9, a8, 0
        movi     a9, 0x400
        s32i.n   a9, a8, 4
        s32i.n   a9, a8, 8
        movi     a9, 0x200
        s32i.n   a9, a8, 12
        s32i.n   a9, a8, 16
        l32r     a9, .Lsec_67_4
        s32i.n   a9, a8, 20
        movi     a9, 0x100
        s16i     a9, a8, 24
        memw
        l32r     a9, .Lsec_67_8
        s32i     a9, a8, 36
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacInitAc
        l32r     a8, .Lsec_67_c
        callx8   a8
        movi.n   a14, 0
        movi.n   a13, 10
        movi.n   a12, 4
        movi.n   a11, 7
        movi     a10, 3
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacInitAc
        l32r     a8, .Lsec_67_10
        callx8   a8
        movi.n   a14, 47
        slli     a14, a14, 6
        movi.n   a13, 4
        movi.n   a12, 3
        movi.n   a11, 2
        movi.n   a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacInitAc
        l32r     a8, .Lsec_67_14
        callx8   a8
        movi.n   a12, 2
        mov.n    a11, a12
        movi     a14, 0x5e0
        movi.n   a13, 3
        movi.n   a10, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacInitAc
        l32r     a8, .Lsec_67_18
        callx8   a8
        movi.n   a14, 0
        mov.n    a13, a14
        mov.n    a12, a14
        movi.n   a11, 1
        movi.n   a10, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacInitAc
        l32r     a8, .Lsec_67_1c
        callx8   a8
        movi.n   a11, 1
        movi     a10, 0x15f
        .reloc  ., R_XTENSA_ASM_EXPAND, RC_SetBasicRate
        l32r     a8, .Lsec_67_20
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, rcAttach
        l32r     a8, .Lsec_67_24
        callx8   a8
        retw.n
        .size   lmacInit, .-lmacInit
.Lsec_67_c8:

        .section .rodata.lmacProcessTxopSuccess.str1.1,"aMS",@progbits,1
        .align  1
.Lsec_68_0:
        .ascii  "lmac.c\000"

        .section .text.lmacProcessTxopSuccess,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_69_0, .Lsec_129_0
        .literal .Lsec_69_4, .Lsec_68_0
        .literal .Lsec_69_8, .Lsec_167_0
        .literal .Lsec_69_c, .Lsec_170_0
        .literal .Lsec_69_10, .Lsec_166_0
        .literal .Lsec_69_14, .Lsec_165_0
        .literal .Lsec_69_18, wifi_log
        .literal .Lsec_69_1c, wifi_log
        .literal .Lsec_69_20, wifi_log
.Lsec_69_24:
        .global lmacProcessTxopSuccess
        .type   lmacProcessTxopSuccess, @function
lmacProcessTxopSuccess:
        entry    a1, 48
        extui    a2, a2, 0, 8
        bnei     a2, 10, .Lsec_69_38
        movi.n   a2, 0
        memw
        l32i     a2, a2, 44
        break    1, 15
.Lsec_69_38:
        l32r     a15, .Lsec_69_0
        l32r     a14, .Lsec_69_4
        bltui    a2, 5, .Lsec_69_5c
        movi     a2, 0x27b
        movi.n   a12, 1
        slli     a11, a12, 11
        l32r     a13, .Lsec_69_8
        movi.n   a10, 6
        s32i.n   a2, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_69_18
        callx8   a8
.Lsec_69_59:
        j        .Lsec_69_59
.Lsec_69_5c:
        l32r     a8, .Lsec_69_c
        addx8    a2, a2, a2
        addx4    a2, a2, a8
        movi.n   a11, 1
        slli     a11, a11, 11
        l8ui     a2, a2, 18
        beqi     a2, 2, .Lsec_69_88
        movi     a2, 0x27e
        movi.n   a12, 1
        l32r     a13, .Lsec_69_10
        movi.n   a10, 6
        s32i.n   a2, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_69_1c
        callx8   a8
.Lsec_69_85:
        j        .Lsec_69_85
.Lsec_69_88:
        movi     a2, 0x280
        l32r     a13, .Lsec_69_14
        movi.n   a12, 1
        movi.n   a10, 6
        s32i.n   a2, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_69_20
        callx8   a8
.Lsec_69_9d:
        j        .Lsec_69_9d
        .size   lmacProcessTxopSuccess, .-lmacProcessTxopSuccess
.Lsec_69_a0:

        .section .wifi0iram.16,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_70_0, .Lsec_170_0
.Lsec_70_4:
        .global GetAccess
        .type   GetAccess, @function
GetAccess:
        entry    a1, 32
        l32r     a8, .Lsec_70_0
        extui    a2, a2, 0, 8
        addx8    a2, a2, a2
        addx4    a2, a2, a8
        retw.n
        .size   GetAccess, .-GetAccess
.Lsec_70_15:

        .section .text.lmacEndRetryAMPDUFail,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_71_0, trc_isTxAmpduOperational
        .literal .Lsec_71_4, ppRecordBarRRC
        .literal .Lsec_71_8, ppFillAMPDUBar
        .literal .Lsec_71_c, ppReSendBar
        .literal .Lsec_71_10, ppRegressAmpdu
.Lsec_71_14:
        .global lmacEndRetryAMPDUFail
        .type   lmacEndRetryAMPDUFail, @function
lmacEndRetryAMPDUFail:
        entry    a1, 32
        movi     a4, 0
        s8i      a4, a2, 18
        memw
        l32i     a10, a3, 36
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, trc_isTxAmpduOperational
        l32r     a8, .Lsec_71_0
        callx8   a8
        beqz     a10, .Lsec_71_78
        l32i.n   a8, a3, 44
        l32i.n   a4, a3, 4
        l32i.n   a9, a8, 0
        l32i.n   a4, a4, 4
        bbci     a9, 18, .Lsec_71_3c
        addi.n   a4, a4, 4
.Lsec_71_3c:
        movi.n   a11, 1
        l32i.n   a10, a8, 16
        extui    a10, a10, 20, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, ppRecordBarRRC
        l32r     a8, .Lsec_71_4
        callx8   a8
        addi.n   a11, a4, 4
        l32i.n   a8, a3, 44
        addi     a12, a4, 10
        l16ui    a13, a4, 22
        srli     a13, a13, 4
        l32i     a10, a8, 4
        extui    a10, a10, 0, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, ppFillAMPDUBar
        l32r     a8, .Lsec_71_8
        callx8   a8
        l32i.n   a3, a3, 44
        l32i.n   a11, a3, 16
        extui    a11, a11, 20, 4
        l32i.n   a10, a3, 4
        extui    a10, a10, 0, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, ppReSendBar
        l32r     a8, .Lsec_71_c
        callx8   a8
        j        .Lsec_71_82
        .byte    0x00
.Lsec_71_78:
        movi.n   a11, 1
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, ppRegressAmpdu
        l32r     a8, .Lsec_71_10
        callx8   a8
.Lsec_71_82:
        movi.n   a3, 8
        s8i      a3, a2, 19
        memw
        memw
        retw.n
        .size   lmacEndRetryAMPDUFail, .-lmacEndRetryAMPDUFail
.Lsec_71_8f:

        .section .iram1.27,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_72_0, pp_post
.Lsec_72_4:
        .global lmacProcessCollisions
        .type   lmacProcessCollisions, @function
lmacProcessCollisions:
        entry    a1, 32
        movi.n   a11, 0
        movi.n   a10, 24
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_post
        l32r     a8, .Lsec_72_0
        callx8   a8
        retw.n
        .size   lmacProcessCollisions, .-lmacProcessCollisions
.Lsec_72_13:

        .section .iram1.31,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_73_0, pp_post
.Lsec_73_4:
        .global lmacProcessAllTxTimeout
        .type   lmacProcessAllTxTimeout, @function
lmacProcessAllTxTimeout:
        entry    a1, 32
        movi.n   a11, 0
        movi.n   a10, 22
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_post
        l32r     a8, .Lsec_73_0
        callx8   a8
        retw.n
        .size   lmacProcessAllTxTimeout, .-lmacProcessAllTxTimeout
.Lsec_73_13:

        .section .iram1.32,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_74_0, pp_post
.Lsec_74_4:
        .global lmacPostTxComplete
        .type   lmacPostTxComplete, @function
lmacPostTxComplete:
        entry    a1, 32
        movi.n   a11, 0
        movi.n   a10, 23
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_post
        l32r     a8, .Lsec_74_0
        callx8   a8
        retw.n
        .size   lmacPostTxComplete, .-lmacPostTxComplete
.Lsec_74_13:

        .section .iram1.33,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_75_0, g_mesh_is_started
        .literal .Lsec_75_4, g_mesh_is_root
        .literal .Lsec_75_8, g_mesh_init_ps_type
        .literal .Lsec_75_c, g_osi_funcs_p
        .literal .Lsec_75_10, 0x3ff73c00
        .literal .Lsec_75_14, g_wifi_mac_time_delta
        .literal .Lsec_75_18, pp_post
.Lsec_75_1c:
        .global lmacProcessRxSucData
        .type   lmacProcessRxSucData, @function
lmacProcessRxSucData:
        entry    a1, 32
        l32r     a8, .Lsec_75_0
        l8ui     a8, a8, 0
        beqz.n   a8, .Lsec_75_4c
        l32r     a8, .Lsec_75_4
        memw
        l8ui     a8, a8, 0
        extui    a8, a8, 0, 8
        bnez.n   a8, .Lsec_75_4c
        l32r     a8, .Lsec_75_8
        l32i.n   a8, a8, 0
        beqz.n   a8, .Lsec_75_4c
        l32r     a8, .Lsec_75_c
        l32i     a8, a8, 0
        l32i     a10, a8, 0x110
        callx8   a10
        j        .Lsec_75_5b
        .byte    0x00
.Lsec_75_4c:
        l32r     a8, .Lsec_75_10
        memw
        l32i.n   a10, a8, 0
        l32r     a8, .Lsec_75_14
        l32i.n   a8, a8, 0
        add.n    a10, a10, a8
.Lsec_75_5b:
        mov.n    a11, a10
        movi.n   a10, 25
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_post
        l32r     a8, .Lsec_75_18
        callx8   a8
        retw.n
        .size   lmacProcessRxSucData, .-lmacProcessRxSucData
.Lsec_75_67:

        .section .wifi0iram.37,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_76_0, g_mesh_is_started
        .literal .Lsec_76_4, g_mesh_is_root
        .literal .Lsec_76_8, g_mesh_init_ps_type
        .literal .Lsec_76_c, g_osi_funcs_p
        .literal .Lsec_76_10, 0x3ff73c00
        .literal .Lsec_76_14, g_wifi_mac_time_delta
        .literal .Lsec_76_18, lmacConfMib
        .literal .Lsec_76_1c, ppProcessLifeTime
.Lsec_76_20:
        .global lmacMSDUAged
        .type   lmacMSDUAged, @function
lmacMSDUAged:
        entry    a1, 48
        l32r     a3, .Lsec_76_0
        l8ui     a3, a3, 0
        beqz.n   a3, .Lsec_76_51
        l32r     a3, .Lsec_76_4
        memw
        l8ui     a3, a3, 0
        extui    a3, a3, 0, 8
        bnez.n   a3, .Lsec_76_51
        l32r     a3, .Lsec_76_8
        l32i.n   a3, a3, 0
        beqz.n   a3, .Lsec_76_51
        l32r     a3, .Lsec_76_c
        l32i     a3, a3, 0
        l32i     a10, a3, 0x110
        callx8   a10
        mov.n    a3, a10
        j        .Lsec_76_60
.Lsec_76_51:
        l32r     a3, .Lsec_76_10
        l32r     a8, .Lsec_76_14
        memw
        l32i.n   a3, a3, 0
        l32i.n   a8, a8, 0
        add.n    a3, a3, a8
.Lsec_76_60:
        l32i.n   a8, a2, 44
        l32i.n   a9, a8, 0
        l32r     a8, .Lsec_76_18
        bbci     a9, 22, .Lsec_76_70
        l32i.n   a8, a8, 0
        j        .Lsec_76_72
        .byte    0x00
.Lsec_76_70:
        l32i.n   a8, a8, 8
.Lsec_76_72:
        slli     a8, a8, 10
        mov.n    a11, a3
        mov.n    a10, a2
        mov.n    a12, a1
        s32i.n   a8, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, ppProcessLifeTime
        l32r     a8, .Lsec_76_1c
        callx8   a8
        l32i.n   a2, a2, 44
        l32i.n   a2, a2, 24
        sub      a3, a3, a2
        movi.n   a2, 1
        l32i.n   a8, a1, 0
        bltu     a8, a3, .Lsec_76_a0
        addmi    a8, a8, 0xffffec00
        bltu     a8, a3, .Lsec_76_9d
        movi     a2, 0
.Lsec_76_9d:
        extui    a2, a2, 0, 8
.Lsec_76_a0:
        retw.n
        .size   lmacMSDUAged, .-lmacMSDUAged
.Lsec_76_a2:

        .section .text.lmacDebugTxDrop,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_77_0, lmacConfMib
        .literal .Lsec_77_4, g_lmac_cnt
        .literal .Lsec_77_8, lmacMSDUAged
.Lsec_77_c:
        .global dbg_cnt_lmac_drop
        .type   dbg_cnt_lmac_drop, @function
dbg_cnt_lmac_drop:
        entry    a1, 32
        mov.n    a10, a2
        beqz     a2, .Lsec_77_88
        l32i.n   a8, a2, 44
        l32i.n   a3, a8, 4
        extui    a3, a3, 4, 4
        bgei     a3, 5, .Lsec_77_88
        addi.n   a3, a3, 14
        l32r     a9, .Lsec_77_0
        l8ui     a12, a8, 6
        l8ui     a11, a9, 21
        l32r     a4, .Lsec_77_4
        bltu     a12, a11, .Lsec_77_44
        addx8    a3, a3, a4
        l8ui     a4, a3, 13
        addi.n   a4, a4, 1
        s8i      a4, a3, 13
        memw
        memw
        j        .Lsec_77_88
        .byte    0x00
.Lsec_77_44:
        l8ui     a11, a8, 7
        l8ui     a8, a9, 20
        bltu     a11, a8, .Lsec_77_61
        addx8    a3, a3, a4
        l8ui     a4, a3, 12
        addi.n   a4, a4, 1
        s8i      a4, a3, 12
        memw
        memw
        j        .Lsec_77_88
.Lsec_77_61:
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacMSDUAged
        l32r     a8, .Lsec_77_8
        callx8   a8
        addx8    a3, a3, a4
        beqz.n   a10, .Lsec_77_7d
        l8ui     a4, a3, 14
        addi.n   a4, a4, 1
        s8i      a4, a3, 14
        memw
        memw
        j        .Lsec_77_88
.Lsec_77_7d:
        l8ui     a4, a3, 15
        addi.n   a4, a4, 1
        s8i      a4, a3, 15
        memw
.Lsec_77_88:
        memw
        retw.n
        .size   dbg_cnt_lmac_drop, .-dbg_cnt_lmac_drop
.Lsec_77_8d:

        .section .text.esp_wifi_internal_set_retry_counter,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_78_0, lmacConfMib
.Lsec_78_4:
        .global esp_wifi_internal_set_retry_counter
        .type   esp_wifi_internal_set_retry_counter, @function
esp_wifi_internal_set_retry_counter:
        entry    a1, 32
        l32r     a8, .Lsec_78_0
        s8i      a2, a8, 21
        movi.n   a2, 0
        s8i      a3, a8, 20
        memw
        memw
        retw.n
        .size   esp_wifi_internal_set_retry_counter, .-esp_wifi_internal_set_retry_counter
.Lsec_78_1a:

        .section .text.esp_wifi_internal_set_msdu_lifetime,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_79_0, lmacConfMib
.Lsec_79_4:
        .global esp_wifi_internal_set_msdu_lifetime
        .type   esp_wifi_internal_set_msdu_lifetime, @function
esp_wifi_internal_set_msdu_lifetime:
        entry    a1, 32
        l32r     a8, .Lsec_79_0
        s32i.n   a2, a8, 16
        movi.n   a2, 0
        s32i.n   a3, a8, 8
        memw
        retw.n
        .size   esp_wifi_internal_set_msdu_lifetime, .-esp_wifi_internal_set_msdu_lifetime
.Lsec_79_15:

        .section .text.esp_wifi_internal_get_mib,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_80_0, lmacConfMib
        .literal .Lsec_80_4, memcpy
.Lsec_80_8:
        .global esp_wifi_internal_get_mib
        .type   esp_wifi_internal_get_mib, @function
esp_wifi_internal_get_mib:
        entry    a1, 32
        mov.n    a10, a2
        movi     a2, 0x102
        beqz.n   a10, .Lsec_80_20
        l32r     a11, .Lsec_80_0
        movi     a12, 48
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_80_4
        callx8   a8
        movi.n   a2, 0
.Lsec_80_20:
        retw.n
        .size   esp_wifi_internal_get_mib, .-esp_wifi_internal_get_mib
.Lsec_80_22:

        .section .text.esp_wifi_internal_set_rts,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_81_0, lmacConfMib
        .literal .Lsec_81_4, .Lsec_136_0
        .literal .Lsec_81_8, wifi_log
.Lsec_81_c:
        .global esp_wifi_internal_set_rts
        .type   esp_wifi_internal_set_rts, @function
esp_wifi_internal_set_rts:
        entry    a1, 48
        mov.n    a8, a2
        movi     a2, 0x102
        beqz.n   a8, .Lsec_81_55
        l16ui    a14, a8, 0
        movi.n   a12, 3
        movi.n   a11, 1
        movi.n   a10, 6
        l32r     a9, .Lsec_81_0
        s16i     a14, a9, 22
        memw
        l8ui     a15, a8, 2
        s8i      a15, a9, 42
        memw
        l8ui     a2, a8, 3
        s8i      a2, a9, 43
        memw
        l8ui     a8, a8, 4
        l32r     a13, .Lsec_81_4
        s8i      a8, a9, 44
        memw
        s32i.n   a2, a1, 0
        movi.n   a2, 0
        s32i.n   a8, a1, 4
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_81_8
        callx8   a8
.Lsec_81_55:
        retw.n
        .size   esp_wifi_internal_set_rts, .-esp_wifi_internal_set_rts
.Lsec_81_57:

        .section .text.esp_wifi_internal_get_rts,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_82_0, lmacConfMib
.Lsec_82_4:
        .global esp_wifi_internal_get_rts
        .type   esp_wifi_internal_get_rts, @function
esp_wifi_internal_get_rts:
        entry    a1, 32
        mov.n    a8, a2
        movi     a2, 0x102
        beqz.n   a8, .Lsec_82_38
        l32r     a9, .Lsec_82_0
        l16ui    a2, a9, 22
        s16i     a2, a8, 0
        memw
        l8ui     a2, a9, 42
        s8i      a2, a8, 2
        memw
        l8ui     a2, a9, 43
        s8i      a2, a8, 3
        memw
        l8ui     a2, a9, 44
        s8i      a2, a8, 4
        memw
        movi     a2, 0
.Lsec_82_38:
        memw
        retw.n
        .size   esp_wifi_internal_get_rts, .-esp_wifi_internal_get_rts
.Lsec_82_3d:

        .section .wifi0iram.53,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_83_0, g_mesh_is_started
        .literal .Lsec_83_4, 0x3ff73c00
        .literal .Lsec_83_8, g_wifi_mac_time_delta
        .literal .Lsec_83_c, lmacConfMib
.Lsec_83_10:
        .global lmacAdjustTimestamp
        .type   lmacAdjustTimestamp, @function
lmacAdjustTimestamp:
        entry    a1, 32
        l32r     a8, .Lsec_83_0
        l8ui     a8, a8, 0
        bnez.n   a8, .Lsec_83_55
        l32i.n   a11, a2, 44
        l32i.n   a8, a11, 0
        bbsi     a8, 5, .Lsec_83_55
        l32r     a9, .Lsec_83_4
        l32r     a10, .Lsec_83_8
        memw
        l32i.n   a9, a9, 0
        l32i.n   a10, a10, 0
        add.n    a9, a9, a10
        l32r     a10, .Lsec_83_c
        bbci     a8, 22, .Lsec_83_3e
        l32i.n   a8, a10, 0
        l32i.n   a10, a10, 4
        j        .Lsec_83_42
.Lsec_83_3e:
        l32i.n   a8, a10, 8
        l32i.n   a10, a10, 12
.Lsec_83_42:
        sub      a8, a8, a10
        slli     a8, a8, 10
        l32i.n   a10, a11, 24
        sub      a10, a9, a10
        bgeu     a8, a10, .Lsec_83_55
        sub      a9, a9, a8
        s32i.n   a9, a11, 24
.Lsec_83_55:
        memw
        retw.n
        .size   lmacAdjustTimestamp, .-lmacAdjustTimestamp
.Lsec_83_5a:

        .section .text.lmac_record_txtime,"ax",@progbits
        .align  4
.Lsec_84_0:
        .global lmac_record_txtime
        .type   lmac_record_txtime, @function
lmac_record_txtime:
        entry    a1, 32
        retw.n
        .size   lmac_record_txtime, .-lmac_record_txtime
.Lsec_84_5:

        .section .text.lmac_update_tx_statistic,"ax",@progbits
        .align  4
.Lsec_85_0:
        .global lmac_update_tx_statistic
        .type   lmac_update_tx_statistic, @function
lmac_update_tx_statistic:
        entry    a1, 32
        retw.n
        .size   lmac_update_tx_statistic, .-lmac_update_tx_statistic
.Lsec_85_5:

        .section .wifi0iram.59,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_86_0, 0x00400008
        .literal .Lsec_86_4, .Lsec_170_0
        .literal .Lsec_86_8, .Lsec_115_0
        .literal .Lsec_86_c, .Lsec_68_0
        .literal .Lsec_86_10, .Lsec_132_0
        .literal .Lsec_86_14, 0x0000098a
        .literal .Lsec_86_18, ppProcTxCallback
        .literal .Lsec_86_1c, ppEnqueueTxDone
        .literal .Lsec_86_20, rcUpdateTxDone
        .literal .Lsec_86_24, pp_post
        .literal .Lsec_86_28, wifi_log
        .literal .Lsec_86_2c, ppProcessTxQ
.Lsec_86_30:
        .global lmacTxDone
        .type   lmacTxDone, @function
lmacTxDone:
        entry    a1, 48
        l32i.n   a4, a2, 44
        movi.n   a11, 1
        mov.n    a10, a2
        extui    a3, a3, 0, 8
        l32i.n   a4, a4, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, ppProcTxCallback
        l32r     a8, .Lsec_86_18
        callx8   a8
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, ppEnqueueTxDone
        l32r     a8, .Lsec_86_1c
        callx8   a8
        extui    a4, a4, 4, 4
        l32i.n   a11, a2, 44
        l32r     a9, .Lsec_86_0
        l32i.n   a8, a11, 0
        and      a8, a8, a9
        bnei     a8, 8, .Lsec_86_64
        l32i.n   a10, a2, 36
        .reloc  ., R_XTENSA_ASM_EXPAND, rcUpdateTxDone
        l32r     a8, .Lsec_86_20
        callx8   a8
.Lsec_86_64:
        movi.n   a11, 0
        movi.n   a10, 16
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_post
        l32r     a8, .Lsec_86_24
        callx8   a8
        beqz.n   a3, .Lsec_86_a8
        l32r     a2, .Lsec_86_4
        addx8    a8, a4, a4
        addx4    a8, a8, a2
        l8ui     a2, a8, 18
        beqz.n   a2, .Lsec_86_a0
        movi.n   a12, 1
        slli     a11, a12, 11
        l32r     a2, .Lsec_86_14
        movi.n   a10, 6
        l32r     a15, .Lsec_86_8
        l32r     a14, .Lsec_86_c
        l32r     a13, .Lsec_86_10
        s32i.n   a2, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_86_28
        callx8   a8
.Lsec_86_9c:
        j        .Lsec_86_9c
        .byte    0x00
.Lsec_86_a0:
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, ppProcessTxQ
        l32r     a8, .Lsec_86_2c
        callx8   a8
.Lsec_86_a8:
        retw.n
        .size   lmacTxDone, .-lmacTxDone
.Lsec_86_aa:

        .section .text.lmacDiscardMSDU,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_87_0, .Lsec_77_c
        .literal .Lsec_87_4, lmacTxDone
        .literal .Lsec_87_8, ppDequeueTxQ
        .literal .Lsec_87_c, lmacTxDone
.Lsec_87_10:
        .global lmacDiscardMSDU
        .type   lmacDiscardMSDU, @function
lmacDiscardMSDU:
        entry    a1, 32
        extui    a5, a5, 0, 8
        movi     a7, 192
.Lsec_87_19:
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, .Lsec_77_c
        l32r     a8, .Lsec_87_0
        callx8   a8
        l32i.n   a6, a2, 32
        addi.n   a6, a6, 1
        l32i.n   a8, a3, 44
        s32i.n   a6, a2, 32
        s8i      a4, a8, 19
        memw
        l32i.n   a6, a8, 16
        extui    a6, a6, 20, 4
        l32i.n   a8, a8, 0
        and      a8, a7, a8
        bnei     a8, 128, .Lsec_87_5a
        mov.n    a10, a3
        movi.n   a11, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacTxDone
        l32r     a8, .Lsec_87_4
        callx8   a8
        mov.n    a10, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, ppDequeueTxQ
        l32r     a8, .Lsec_87_8
        callx8   a8
        mov.n    a3, a10
        bnez     a10, .Lsec_87_19
        j        .Lsec_87_64
        .byte    0x00
.Lsec_87_5a:
        mov.n    a11, a5
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacTxDone
        l32r     a8, .Lsec_87_c
        callx8   a8
.Lsec_87_64:
        retw.n
        .size   lmacDiscardMSDU, .-lmacDiscardMSDU
.Lsec_87_66:

        .section .wifi0iram.17,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_88_0, lmacDiscardMSDU
.Lsec_88_4:
        .global lmacDiscardAgedMSDU
        .type   lmacDiscardAgedMSDU, @function
lmacDiscardAgedMSDU:
        entry    a1, 32
        extui    a13, a4, 0, 8
        movi.n   a12, 4
        mov.n    a11, a3
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacDiscardMSDU
        l32r     a8, .Lsec_88_0
        callx8   a8
        retw.n
        .size   lmacDiscardAgedMSDU, .-lmacDiscardAgedMSDU
.Lsec_88_18:

        .section .wifi0iram.54,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_89_0, .Lsec_170_0
        .literal .Lsec_89_4, .Lsec_135_0
        .literal .Lsec_89_8, .Lsec_116_0
        .literal .Lsec_89_c, .Lsec_68_0
        .literal .Lsec_89_10, .Lsec_134_0
        .literal .Lsec_89_14, 0x000008b6
        .literal .Lsec_89_18, g_osi_funcs_p
        .literal .Lsec_89_1c, .Lsec_133_0
        .literal .Lsec_89_20, 0x000008be
        .literal .Lsec_89_24, 0x00002102
        .literal .Lsec_89_28, 0xffffefff
        .literal .Lsec_89_2c, lmacConfMib
        .literal .Lsec_89_30, .Lsec_56_4
        .literal .Lsec_89_34, wifi_log
        .literal .Lsec_89_38, wDev_isNANPktInValidSlot
        .literal .Lsec_89_3c, lmacDiscardAgedMSDU
        .literal .Lsec_89_40, ppRecycleAmpdu
        .literal .Lsec_89_44, pp_post
        .literal .Lsec_89_48, wifi_log
        .literal .Lsec_89_4c, wifi_log
        .literal .Lsec_89_50, lmacIsLongFrame
        .literal .Lsec_89_54, lmacSetTxFrame
        .literal .Lsec_89_58, lmacIsLongFrame
        .literal .Lsec_89_5c, hal_random
        .literal .Lsec_89_60, hal_random
        .literal .Lsec_89_64, hal_mac_tx_config_edca
        .literal .Lsec_89_68, wifi_gpio_debug
        .literal .Lsec_89_6c, hal_mac_txq_enable
.Lsec_89_70:
        .global lmacTxFrame
        .type   lmacTxFrame, @function
lmacTxFrame:
        entry    a1, 64
        l32i     a4, a2, 44
        extui    a5, a3, 0, 8
        addx8    a3, a5, a5
        l32r     a6, .Lsec_89_0
        addx4    a3, a3, a6
        l16ui    a7, a4, 2
        slli     a4, a5, 3
        bbci     a7, 0, .Lsec_89_c6
        .reloc  ., R_XTENSA_ASM_EXPAND, .Lsec_56_4
        l32r     a8, .Lsec_89_30
        callx8   a8
        bnez.n   a10, .Lsec_89_c6
.Lsec_89_93:
        add.n    a4, a4, a5
        addx4    a4, a4, a6
        l32r     a13, .Lsec_89_4
        movi.n   a6, 0
        movi.n   a12, 4
        movi.n   a11, 64
        movi.n   a10, 6
        s32i.n   a6, a4, 0
        s8i      a6, a4, 18
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_89_34
        callx8   a8
        l32i.n   a4, a2, 44
        l32i.n   a12, a4, 0
        movi.n   a4, 1
        slli     a4, a4, 22
        and      a12, a12, a4
        beq      a12, a6, .Lsec_89_d6
        j        .Lsec_89_e4
.Lsec_89_c6:
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, wDev_isNANPktInValidSlot
        l32r     a8, .Lsec_89_38
        callx8   a8
        bnez.n   a10, .Lsec_89_fb
        j        .Lsec_89_93
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_89_d6:
        mov.n    a11, a2
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacDiscardAgedMSDU
        l32r     a8, .Lsec_89_3c
        callx8   a8
        j        .Lsec_89_ee
        .byte    0x00
.Lsec_89_e4:
        mov.n    a11, a6
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, ppRecycleAmpdu
        l32r     a8, .Lsec_89_40
        callx8   a8
.Lsec_89_ee:
        movi.n   a11, 0
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_post
        l32r     a8, .Lsec_89_44
        callx8   a8
        j        .Lsec_89_27c
.Lsec_89_fb:
        add.n    a7, a4, a5
        addx4    a7, a7, a6
        l8ui     a8, a7, 18
        addi     a7, a8, -3
        extui    a7, a7, 0, 8
        bltui    a7, 2, .Lsec_89_135
        movi.n   a11, 1
        beqz.n   a8, .Lsec_89_135
        l32r     a2, .Lsec_89_14
        mov.n    a12, a11
        movi     a10, 6
        slli     a11, a11, 11
        l32r     a15, .Lsec_89_8
        l32r     a14, .Lsec_89_c
        l32r     a13, .Lsec_89_10
        s32i     a2, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_89_48
        callx8   a8
.Lsec_89_130:
        j        .Lsec_89_130
        .byte    0x00
        .byte    0x00
.Lsec_89_135:
        l32r     a9, .Lsec_89_18
        l32i.n   a8, a9, 0
        l32i     a8, a8, 216
        callx8   a8
        add.n    a8, a4, a5
        addx4    a8, a8, a6
        l8ui     a11, a8, 18
        bnei     a11, 4, .Lsec_89_174
        l32i.n   a8, a8, 0
        bne      a8, a2, .Lsec_89_153
        j        .Lsec_89_1f5
.Lsec_89_153:
        movi.n   a12, 1
        slli     a11, a12, 11
        l32r     a2, .Lsec_89_20
        movi.n   a10, 6
        l32r     a15, .Lsec_89_8
        l32r     a14, .Lsec_89_c
        l32r     a13, .Lsec_89_1c
        s32i.n   a2, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_89_4c
        callx8   a8
.Lsec_89_171:
        j        .Lsec_89_171
.Lsec_89_174:
        l32i.n   a7, a2, 44
        movi.n   a10, 1
        slli     a10, a10, 13
        l32i.n   a9, a7, 0
        l32r     a12, .Lsec_89_24
        and      a12, a9, a12
        s32i.n   a2, a8, 0
        bne      a12, a10, .Lsec_89_192
        movi.n   a10, 1
        slli     a10, a10, 12
        or       a9, a9, a10
        s32i.n   a9, a7, 0
.Lsec_89_192:
        mov.n    a10, a2
        s32i     a11, a1, 16
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacIsLongFrame
        l32r     a8, .Lsec_89_50
        callx8   a8
        l32i.n   a11, a1, 16
        beqz.n   a10, .Lsec_89_1b8
        l32i.n   a9, a7, 0
        bbsi     a9, 1, .Lsec_89_1b8
        l32r     a10, .Lsec_89_28
        and      a9, a9, a10
        movi     a10, 0x100
        or       a9, a9, a10
        s32i     a9, a7, 0
.Lsec_89_1b8:
        l32i.n   a9, a7, 0
        bbci     a9, 12, .Lsec_89_1e8
        bnei     a11, 3, .Lsec_89_1e8
        l32r     a10, .Lsec_89_2c
        l8ui     a11, a7, 5
        l8ui     a10, a10, 42
        bltu     a11, a10, .Lsec_89_1e8
        l32r     a10, .Lsec_89_28
        and      a9, a9, a10
        movi     a10, 0x100
        or       a9, a9, a10
        s32i.n   a9, a7, 0
        l8ui     a9, a7, 6
        s8i      a9, a7, 7
        movi.n   a9, 0
        s8i      a9, a7, 6
        memw
.Lsec_89_1e8:
        movi.n   a11, 0
        mov.n    a10, a3
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacSetTxFrame
        l32r     a8, .Lsec_89_54
        callx8   a8
.Lsec_89_1f5:
        add.n    a8, a4, a5
        addx4    a7, a8, a6
        l8ui     a9, a7, 9
        add.n    a9, a9, a9
        l8ui     a10, a7, 8
        bge      a9, a10, .Lsec_89_21d
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacIsLongFrame
        l32r     a8, .Lsec_89_58
        callx8   a8
        bnez.n   a10, .Lsec_89_21d
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_random
        l32r     a8, .Lsec_89_5c
        callx8   a8
        l8ui     a2, a7, 9
        add.n    a2, a2, a2
        j        .Lsec_89_22b
.Lsec_89_21d:
        add.n    a2, a4, a5
        addx4    a2, a2, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_random
        l32r     a8, .Lsec_89_60
        callx8   a8
        l8ui     a2, a2, 8
.Lsec_89_22b:
        movi.n   a8, -1
        ssl      a2
        sll      a2, a8
        xor      a8, a8, a2
        and      a10, a8, a10
        add.n    a4, a4, a5
        addx4    a4, a4, a6
        extui    a10, a10, 0, 16
        s16i     a10, a4, 6
        memw
        mov.n    a10, a3
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_tx_config_edca
        l32r     a8, .Lsec_89_64
        callx8   a8
        movi.n   a2, 1
        movi.n   a11, 2
        s8i      a2, a4, 18
        memw
        movi     a10, 9
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_gpio_debug
        l32r     a8, .Lsec_89_68
        callx8   a8
        l8ui     a10, a4, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_txq_enable
        l32r     a8, .Lsec_89_6c
        callx8   a8
        l32r     a3, .Lsec_89_18
        l32i.n   a2, a3, 0
        l32i     a2, a2, 220
        callx8   a2
.Lsec_89_27c:
        retw.n
        .size   lmacTxFrame, .-lmacTxFrame
.Lsec_89_27e:

        .section .text.lmacRetryTxFrame,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_90_0, .Lsec_147_0
        .literal .Lsec_90_4, rcGetRate
        .literal .Lsec_90_8, ppCalFrameTimes
        .literal .Lsec_90_c, lmacTxFrame
        .literal .Lsec_90_10, wifi_log
        .literal .Lsec_90_14, lmacDiscardAgedMSDU
        .literal .Lsec_90_18, ppRecycleAmpdu
.Lsec_90_1c:
        .global lmacRetryTxFrame
        .type   lmacRetryTxFrame, @function
lmacRetryTxFrame:
        entry    a1, 32
        l32i.n   a4, a2, 0
        extui    a3, a3, 0, 8
        l32i.n   a11, a4, 44
        l32i.n   a5, a4, 4
        l32i     a9, a11, 0
        l32i     a5, a5, 4
        bbsi     a9, 17, .Lsec_90_89
        bnez     a3, .Lsec_90_c6
        l8ui     a8, a2, 13
        bnez     a8, .Lsec_90_c6
        l8ui     a10, a2, 15
        beqz     a10, .Lsec_90_49
        s8i      a8, a2, 15
        s8i      a8, a2, 13
        memw
.Lsec_90_49:
        bbci     a9, 18, .Lsec_90_4e
        addi.n   a5, a5, 4
.Lsec_90_4e:
        l32i.n   a10, a4, 36
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, rcGetRate
        l32r     a8, .Lsec_90_4
        callx8   a8
        l32i.n   a10, a2, 0
        l32i.n   a4, a10, 44
        l32i.n   a4, a4, 0
        bbci     a4, 6, .Lsec_90_75
        .reloc  ., R_XTENSA_ASM_EXPAND, ppCalFrameTimes
        l32r     a8, .Lsec_90_8
        callx8   a8
        l32i.n   a4, a2, 0
        l32i.n   a4, a4, 44
        l16ui    a4, a4, 10
        s16i     a4, a5, 2
        memw
.Lsec_90_75:
        l8ui     a11, a2, 4
        l32i     a10, a2, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacTxFrame
        l32r     a8, .Lsec_90_c
        callx8   a8
        j        .Lsec_90_c6
        .byte    0x00
        .byte    0x00
.Lsec_90_89:
        movi.n   a5, 0
        l32r     a13, .Lsec_90_0
        movi.n   a12, 3
        movi.n   a11, 64
        movi.n   a10, 6
        s32i.n   a5, a2, 0
        s8i      a5, a2, 18
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_90_10
        callx8   a8
        l32i.n   a5, a4, 44
        l32i.n   a5, a5, 0
        bbsi     a5, 22, .Lsec_90_bc
        movi.n   a12, 1
        mov.n    a11, a4
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacDiscardAgedMSDU
        l32r     a8, .Lsec_90_14
        callx8   a8
        j        .Lsec_90_c6
        .byte    0x00
.Lsec_90_bc:
        movi.n   a11, 1
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, ppRecycleAmpdu
        l32r     a8, .Lsec_90_18
        callx8   a8
.Lsec_90_c6:
        retw.n
        .size   lmacRetryTxFrame, .-lmacRetryTxFrame
.Lsec_90_c8:

        .section .wifiextrairam.19,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_91_0, .Lsec_126_0
        .literal .Lsec_91_4, .Lsec_68_0
        .literal .Lsec_91_8, .Lsec_162_0
        .literal .Lsec_91_c, lmacConfMib
        .literal .Lsec_91_10, wifi_log
        .literal .Lsec_91_14, ppRecordBarRRC
        .literal .Lsec_91_18, ppResortTxAMPDU
        .literal .Lsec_91_1c, ppRecycleAmpdu
        .literal .Lsec_91_20, lmacDiscardMSDU
.Lsec_91_24:
        .global lmacDiscardFrameExchangeSequence
        .type   lmacDiscardFrameExchangeSequence, @function
lmacDiscardFrameExchangeSequence:
        entry    a1, 48
        mov.n    a10, a2
        l8ui     a8, a10, 18
        l32i.n   a2, a2, 0
        beqi     a8, 6, .Lsec_91_55
        movi     a2, 0x32f
        movi.n   a12, 1
        slli     a11, a12, 11
        l32r     a15, .Lsec_91_0
        movi.n   a10, 6
        l32r     a14, .Lsec_91_4
        l32r     a13, .Lsec_91_8
        s32i     a2, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_91_10
        callx8   a8
.Lsec_91_50:
        j        .Lsec_91_50
        .byte    0x00
        .byte    0x00
.Lsec_91_55:
        movi.n   a8, 0
        s8i      a8, a10, 18
        memw
        movi.n   a8, 0
        movi.n   a13, 1
        s32i.n   a8, a10, 0
        l32i.n   a8, a2, 44
        l32i.n   a9, a8, 0
        bbci     a9, 21, .Lsec_91_94
        l8ui     a11, a8, 5
        addi     a11, a11, 2
        extui    a11, a11, 0, 8
        l32i.n   a10, a8, 16
        extui    a10, a10, 20, 4
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, ppRecordBarRRC
        l32r     a8, .Lsec_91_14
        callx8   a8
        l32i     a2, a2, 44
        l32i     a10, a2, 16
        extui    a10, a10, 20, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, ppResortTxAMPDU
        l32r     a8, .Lsec_91_18
        callx8   a8
        j        .Lsec_91_ca
        .byte    0x00
.Lsec_91_94:
        bbci     a9, 22, .Lsec_91_a4
        mov.n    a11, a13
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, ppRecycleAmpdu
        l32r     a8, .Lsec_91_1c
        callx8   a8
        j        .Lsec_91_ca
.Lsec_91_a4:
        l32r     a9, .Lsec_91_c
        l8ui     a12, a8, 6
        l8ui     a11, a9, 21
        bltu     a12, a11, .Lsec_91_b5
        movi.n   a12, 2
        j        .Lsec_91_c2
.Lsec_91_b5:
        l8ui     a11, a8, 7
        movi.n   a12, 3
        l8ui     a8, a9, 20
        bgeu     a11, a8, .Lsec_91_c2
        movi.n   a12, 4
.Lsec_91_c2:
        mov.n    a11, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacDiscardMSDU
        l32r     a8, .Lsec_91_20
        callx8   a8
.Lsec_91_ca:
        retw.n
        .size   lmacDiscardFrameExchangeSequence, .-lmacDiscardFrameExchangeSequence
.Lsec_91_cc:

        .section .wifi0iram.18,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_92_0, lmacTxDone
.Lsec_92_4:
        .global lmacRecycleMPDU
        .type   lmacRecycleMPDU, @function
lmacRecycleMPDU:
        entry    a1, 32
        l32i.n   a8, a2, 28
        mov.n    a10, a3
        addi.n   a8, a8, 1
        extui    a11, a4, 0, 8
        s32i.n   a8, a2, 28
        movi.n   a8, 1
        l32i.n   a2, a3, 44
        s8i      a8, a2, 19
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacTxDone
        l32r     a8, .Lsec_92_0
        callx8   a8
        retw.n
        .size   lmacRecycleMPDU, .-lmacRecycleMPDU
.Lsec_92_27:

        .section .wifi0iram.21,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_93_0, .Lsec_125_0
        .literal .Lsec_93_4, .Lsec_68_0
        .literal .Lsec_93_8, .Lsec_161_0
        .literal .Lsec_93_c, lmacConfMib
        .literal .Lsec_93_10, .Lsec_160_0
        .literal .Lsec_93_14, pTxRx
        .literal .Lsec_93_18, .Lsec_159_0
        .literal .Lsec_93_1c, .Lsec_158_0
        .literal .Lsec_93_20, .Lsec_157_0
        .literal .Lsec_93_24, wifi_log
        .literal .Lsec_93_28, ppRecordBarRRC
        .literal .Lsec_93_2c, ppResortTxAMPDU
        .literal .Lsec_93_30, ppRecycleAmpdu
        .literal .Lsec_93_34, lmacDiscardMSDU
        .literal .Lsec_93_38, wifi_log
        .literal .Lsec_93_3c, memset
        .literal .Lsec_93_40, hal_mac_tx_get_blockack
        .literal .Lsec_93_44, hal_mac_tx_is_cbw40
        .literal .Lsec_93_48, ppTxqUpdateBitmap
        .literal .Lsec_93_4c, ppRecycleAmpdu
        .literal .Lsec_93_50, ppResortTxAMPDU
        .literal .Lsec_93_54, wifi_log
        .literal .Lsec_93_58, lmacRecycleMPDU
        .literal .Lsec_93_5c, wifi_log
        .literal .Lsec_93_60, wifi_log
        .literal .Lsec_93_64, lmacRetryTxFrame
        .literal .Lsec_93_68, lmacIsLongFrame
        .literal .Lsec_93_6c, lmacRetryTxFrame
        .literal .Lsec_93_70, lmacEndRetryAMPDUFail
.Lsec_93_74:
        .global lmacEndFrameExchangeSequence
        .type   lmacEndFrameExchangeSequence, @function
lmacEndFrameExchangeSequence:
        entry    a1, 64
        extui    a4, a4, 0, 8
        l8ui     a10, a2, 18
        extui    a11, a3, 0, 8
        bnez     a4, .Lsec_93_27e
        beqi     a10, 6, .Lsec_93_89
        j        .Lsec_93_130
.Lsec_93_89:
        s8i      a4, a2, 18
        memw
        l32i.n   a5, a2, 0
        bnez.n   a11, .Lsec_93_ba
        movi.n   a3, 9
        bnez.n   a5, .Lsec_93_bc
        movi     a2, 0x48a
        movi.n   a12, 1
        l32r     a15, .Lsec_93_0
        slli     a11, a12, 11
        l32r     a14, .Lsec_93_4
        l32r     a13, .Lsec_93_8
        s32i     a2, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_93_24
        callx8   a8
.Lsec_93_b4:
        j        .Lsec_93_b4
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_93_ba:
        movi.n   a3, 10
.Lsec_93_bc:
        s8i      a3, a2, 19
        memw
        movi.n   a13, 1
        l32i.n   a3, a5, 44
        l32i.n   a4, a3, 0
        bbci     a4, 21, .Lsec_93_f4
        l8ui     a11, a3, 5
        addi.n   a11, a11, 2
        extui    a11, a11, 0, 8
        l32i.n   a10, a3, 16
        extui    a10, a10, 20, 4
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, ppRecordBarRRC
        l32r     a8, .Lsec_93_28
        callx8   a8
        l32i     a2, a5, 44
        l32i     a10, a2, 16
        extui    a10, a10, 20, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, ppResortTxAMPDU
        l32r     a8, .Lsec_93_2c
        callx8   a8
        j        .Lsec_93_316
        .byte    0x00
.Lsec_93_f4:
        bbci     a4, 22, .Lsec_93_105
        mov.n    a11, a13
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, ppRecycleAmpdu
        l32r     a8, .Lsec_93_30
        callx8   a8
        j        .Lsec_93_316
        .byte    0x00
.Lsec_93_105:
        l32r     a4, .Lsec_93_c
        movi.n   a12, 2
        l8ui     a8, a3, 6
        l8ui     a6, a4, 21
        bgeu     a8, a6, .Lsec_93_122
        movi     a12, 4
        l8ui     a6, a3, 7
        l8ui     a3, a4, 20
        bltu     a6, a3, .Lsec_93_122
        movi     a12, 3
.Lsec_93_122:
        mov.n    a11, a5
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacDiscardMSDU
        l32r     a8, .Lsec_93_34
        callx8   a8
        j        .Lsec_93_316
        .byte    0x00
.Lsec_93_130:
        beqi     a10, 5, .Lsec_93_136
        j        .Lsec_93_258
.Lsec_93_136:
        l32i.n   a11, a2, 0
        movi.n   a3, 1
        s8i      a4, a2, 18
        memw
        bnez.n   a11, .Lsec_93_166
        movi     a2, 0x4aa
        mov.n    a12, a3
        slli     a11, a3, 11
        l32r     a15, .Lsec_93_0
        movi.n   a10, 6
        l32r     a14, .Lsec_93_4
        l32r     a13, .Lsec_93_10
        s32i.n   a2, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_93_38
        callx8   a8
.Lsec_93_160:
        j        .Lsec_93_160
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_93_166:
        l32i.n   a6, a11, 44
        l32i.n   a5, a6, 0
        bbsi     a5, 19, .Lsec_93_170
        j        .Lsec_93_219
.Lsec_93_170:
        mov.n    a11, a4
        movi.n   a12, 12
        addi     a10, a1, 16
        .reloc  ., R_XTENSA_ASM_EXPAND, memset
        l32r     a8, .Lsec_93_3c
        callx8   a8
        l32i.n   a4, a6, 4
        addi     a11, a1, 16
        extui    a4, a4, 0, 4
        slli     a3, a3, 21
        l8ui     a10, a2, 4
        s8i      a4, a1, 16
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_tx_get_blockack
        l32r     a8, .Lsec_93_40
        callx8   a8
        l32i.n   a4, a2, 0
        l32i.n   a4, a4, 44
        l32i.n   a5, a4, 0
        and      a5, a5, a3
        l32i.n   a10, a4, 16
        bnez.n   a5, .Lsec_93_1d5
        l32r     a3, .Lsec_93_14
        l32i.n   a4, a3, 0
        extui    a3, a10, 20, 4
        addx2    a3, a3, a3
        slli     a3, a3, 4
        add.n    a4, a4, a3
        l8ui     a10, a2, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_tx_is_cbw40
        l32r     a8, .Lsec_93_44
        callx8   a8
        mov.n    a14, a5
        l32i.n   a3, a2, 0
        l32i.n   a3, a3, 44
        s8i      a10, a4, 77
        memw
        l32i.n   a10, a3, 16
        memw
        j        .Lsec_93_1dd
        .byte    0x00
.Lsec_93_1d5:
        l8ui     a14, a4, 5
        addi.n   a14, a14, 1
        extui    a14, a14, 0, 8
.Lsec_93_1dd:
        extui    a10, a10, 20, 4
        l32i.n   a13, a1, 24
        l32i.n   a12, a1, 20
        l16ui    a11, a1, 18
        .reloc  ., R_XTENSA_ASM_EXPAND, ppTxqUpdateBitmap
        l32r     a8, .Lsec_93_48
        callx8   a8
        l32i.n   a10, a2, 0
        l32i.n   a3, a10, 44
        l32i.n   a4, a3, 0
        bbci     a4, 17, .Lsec_93_209
        l8ui     a4, a10, 26
        beqi     a4, 5, .Lsec_93_209
        movi.n   a11, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, ppRecycleAmpdu
        l32r     a8, .Lsec_93_4c
        callx8   a8
        j        .Lsec_93_214
        .byte    0x00
        .byte    0x00
.Lsec_93_209:
        l32i.n   a10, a3, 16
        extui    a10, a10, 20, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, ppResortTxAMPDU
        l32r     a8, .Lsec_93_50
        callx8   a8
.Lsec_93_214:
        movi.n   a3, 1
        j        .Lsec_93_2ff
.Lsec_93_219:
        bbci     a5, 22, .Lsec_93_23f
        movi     a2, 0x4c9
        l32r     a15, .Lsec_93_0
        mov.n    a12, a3
        slli     a11, a3, 11
        movi     a10, 6
        l32r     a14, .Lsec_93_4
        l32r     a13, .Lsec_93_18
        s32i     a2, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_93_54
        callx8   a8
.Lsec_93_23c:
        j        .Lsec_93_23c
.Lsec_93_23f:
        movi.n   a4, 3
        s8i      a4, a2, 19
        memw
        mov.n    a12, a3
        mov.n    a10, a2
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacRecycleMPDU
        l32r     a8, .Lsec_93_58
        callx8   a8
        j        .Lsec_93_316
        .byte    0x00
.Lsec_93_258:
        movi     a2, 0x4d8
        movi.n   a12, 1
        l32r     a15, .Lsec_93_0
        slli     a11, a12, 11
        movi     a10, 6
        l32r     a14, .Lsec_93_4
        l32r     a13, .Lsec_93_1c
        s32i     a2, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_93_5c
        callx8   a8
.Lsec_93_278:
        j        .Lsec_93_278
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_93_27e:
        addi.n   a10, a10, -1
        extui    a10, a10, 0, 8
        bltui    a10, 2, .Lsec_93_2a8
        movi     a2, 0x4db
        movi.n   a12, 1
        l32r     a15, .Lsec_93_0
        slli     a11, a12, 11
        movi.n   a10, 6
        l32r     a14, .Lsec_93_4
        l32r     a13, .Lsec_93_20
        s32i.n   a2, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_93_60
        callx8   a8
.Lsec_93_2a4:
        j        .Lsec_93_2a4
        .byte    0x00
.Lsec_93_2a8:
        bnez.n   a11, .Lsec_93_2c2
        movi.n   a3, 3
        s8i      a3, a2, 18
        memw
        mov.n    a10, a2
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacRetryTxFrame
        l32r     a8, .Lsec_93_64
        callx8   a8
        movi.n   a3, 5
        j        .Lsec_93_2ff
.Lsec_93_2c2:
        l32i.n   a3, a2, 0
        l32i.n   a4, a3, 44
        l32i.n   a5, a4, 0
        bbci     a5, 22, .Lsec_93_2e8
        l32r     a5, .Lsec_93_c
        l8ui     a4, a4, 5
        l8ui     a6, a5, 43
        bltu     a4, a6, .Lsec_93_2e8
        or       a10, a3, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacIsLongFrame
        l32r     a8, .Lsec_93_68
        callx8   a8
        bnez.n   a10, .Lsec_93_30c
        l8ui     a5, a5, 44
        bgeu     a4, a5, .Lsec_93_30c
.Lsec_93_2e8:
        movi.n   a3, 3
        movi.n   a11, 0
        s8i      a3, a2, 18
        memw
        mov.n    a10, a2
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacRetryTxFrame
        l32r     a8, .Lsec_93_6c
        callx8   a8
        movi.n   a3, 7
.Lsec_93_2ff:
        s8i      a3, a2, 19
        memw
        memw
        j        .Lsec_93_316
        .byte    0x00
.Lsec_93_30c:
        mov.n    a11, a3
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacEndRetryAMPDUFail
        l32r     a8, .Lsec_93_70
        callx8   a8
.Lsec_93_316:
        retw.n
        .size   lmacEndFrameExchangeSequence, .-lmacEndFrameExchangeSequence
.Lsec_93_318:

        .section .wifi0iram.12,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_94_0, .Lsec_128_0
        .literal .Lsec_94_4, .Lsec_68_0
        .literal .Lsec_94_8, .Lsec_164_0
        .literal .Lsec_94_c, .Lsec_170_0
        .literal .Lsec_94_10, wifi_log
        .literal .Lsec_94_14, lmacEndFrameExchangeSequence
.Lsec_94_18:
        .global lmacProcessTBSuccess
        .type   lmacProcessTBSuccess, @function
lmacProcessTBSuccess:
        entry    a1, 48
        extui    a2, a2, 0, 8
        bltui    a2, 4, .Lsec_94_45
        movi     a2, 0x29d
        movi.n   a12, 1
        l32r     a15, .Lsec_94_0
        slli     a11, a12, 11
        movi.n   a10, 6
        l32r     a14, .Lsec_94_4
        l32r     a13, .Lsec_94_8
        s32i     a2, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_94_10
        callx8   a8
.Lsec_94_40:
        j        .Lsec_94_40
        .byte    0x00
        .byte    0x00
.Lsec_94_45:
        l32r     a10, .Lsec_94_c
        addx8    a8, a2, a2
        addx4    a8, a8, a10
        movi.n   a9, 5
        movi.n   a12, 0
        s8i      a9, a8, 18
        memw
        movi.n   a11, 1
        mov.n    a10, a8
        addx8    a2, a2, a2
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacEndFrameExchangeSequence
        l32r     a8, .Lsec_94_14
        callx8   a8
        retw.n
        .size   lmacProcessTBSuccess, .-lmacProcessTBSuccess
.Lsec_94_6a:

        .section .wifiextrairam.14,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_95_0, .Lsec_127_0
        .literal .Lsec_95_4, .Lsec_68_0
        .literal .Lsec_95_8, .Lsec_163_0
        .literal .Lsec_95_c, .Lsec_170_0
        .literal .Lsec_95_10, wifi_log
        .literal .Lsec_95_14, lmacIsLongFrame
        .literal .Lsec_95_18, lmacEndFrameExchangeSequence
.Lsec_95_1c:
        .global lmacProcessTxSuccess
        .type   lmacProcessTxSuccess, @function
lmacProcessTxSuccess:
        entry    a1, 48
        extui    a2, a2, 0, 8
        extui    a14, a3, 0, 8
        bltui    a2, 5, .Lsec_95_4c
        movi     a2, 0x2ac
        movi.n   a12, 1
        l32r     a15, .Lsec_95_0
        slli     a11, a12, 11
        movi     a10, 6
        l32r     a14, .Lsec_95_4
        l32r     a13, .Lsec_95_8
        s32i     a2, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_95_10
        callx8   a8
.Lsec_95_48:
        j        .Lsec_95_48
        .byte    0x00
.Lsec_95_4c:
        l32r     a6, .Lsec_95_c
        addx8    a5, a2, a2
        addx4    a5, a5, a6
        movi     a11, 0x402
        slli     a4, a2, 3
        l32i.n   a10, a5, 0
        l32i.n   a7, a10, 44
        l32i.n   a9, a7, 0
        and      a11, a9, a11
        l8ui     a3, a5, 9
        bnez.n   a11, .Lsec_95_79
        movi.n   a12, 9
        slli     a12, a12, 19
        movi.n   a13, 1
        and      a12, a9, a12
        slli     a13, a13, 22
        bne      a12, a13, .Lsec_95_ab
.Lsec_95_79:
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacIsLongFrame
        l32r     a8, .Lsec_95_14
        callx8   a8
        add.n    a8, a4, a2
        addx4    a8, a8, a6
        s8i      a3, a8, 8
        memw
        beqz.n   a10, .Lsec_95_9c
        movi.n   a3, 0
        s8i      a3, a8, 12
        memw
        memw
        j        .Lsec_95_a2
        .byte    0x00
        .byte    0x00
.Lsec_95_9c:
        s8i      a10, a8, 11
        memw
.Lsec_95_a2:
        movi     a14, 127
        memw
        j        .Lsec_95_cb
.Lsec_95_ab:
        movi     a10, 0x100
        and      a9, a9, a10
        s8i      a3, a5, 8
        memw
        bnez.n   a9, .Lsec_95_c5
        s8i      a9, a5, 11
        memw
        memw
        j        .Lsec_95_cb
.Lsec_95_c5:
        s8i      a11, a5, 11
        s8i      a11, a5, 12
.Lsec_95_cb:
        add.n    a4, a4, a2
        addx4    a4, a4, a6
        movi.n   a2, 5
        movi.n   a12, 0
        s8i      a14, a7, 13
        movi.n   a11, 1
        mov.n    a10, a5
        s8i      a2, a4, 18
        movi     a2, 1
        s8i      a2, a7, 19
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacEndFrameExchangeSequence
        l32r     a8, .Lsec_95_18
        callx8   a8
        retw.n
        .size   lmacProcessTxSuccess, .-lmacProcessTxSuccess
.Lsec_95_f2:

        .section .text.lmacProcessShortRetryFail,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_96_0, .Lsec_122_0
        .literal .Lsec_96_4, .Lsec_68_0
        .literal .Lsec_96_8, .Lsec_150_0
        .literal .Lsec_96_c, lmacConfMib
        .literal .Lsec_96_10, .Lsec_149_0
        .literal .Lsec_96_14, 0x00400100
        .literal .Lsec_96_18, 0x00600002
        .literal .Lsec_96_1c, .Lsec_148_0
        .literal .Lsec_96_20, wifi_log
        .literal .Lsec_96_24, wifi_log
        .literal .Lsec_96_28, rcReachRetryLimit
        .literal .Lsec_96_2c, lmacMSDUAged
        .literal .Lsec_96_30, lmacIsLongFrame
        .literal .Lsec_96_34, lmacEndRetryAMPDUFail
        .literal .Lsec_96_38, lmacDiscardFrameExchangeSequence
        .literal .Lsec_96_3c, lmacEndFrameExchangeSequence
        .literal .Lsec_96_40, wifi_log
        .literal .Lsec_96_44, lmacRetryTxFrame
.Lsec_96_48:
        .global lmacProcessShortRetryFail
        .type   lmacProcessShortRetryFail, @function
lmacProcessShortRetryFail:
        entry    a1, 48
        extui    a3, a3, 0, 8
        extui    a4, a4, 0, 8
        extui    a5, a5, 0, 8
        movi.n   a6, 1
        or       a7, a4, a3
        bne      a5, a6, .Lsec_96_5e
        beqz.n   a7, .Lsec_96_62
.Lsec_96_5e:
        l32i.n   a5, a2, 0
        bnez.n   a5, .Lsec_96_84
.Lsec_96_62:
        movi     a2, 0x64d
        movi.n   a12, 1
        l32r     a15, .Lsec_96_0
        slli     a11, a12, 11
        movi.n   a10, 6
        l32r     a14, .Lsec_96_4
        l32r     a13, .Lsec_96_8
        s32i.n   a2, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_96_20
        callx8   a8
.Lsec_96_80:
        j        .Lsec_96_80
        .byte    0x00
.Lsec_96_84:
        l32r     a6, .Lsec_96_c
        l8ui     a8, a2, 11
        l8ui     a9, a6, 21
        bgeu     a8, a9, .Lsec_96_b1
        addi.n   a8, a8, 1
        extui    a8, a8, 0, 8
        s8i      a8, a2, 11
        memw
        bgeu     a8, a9, .Lsec_96_b1
        l8ui     a8, a2, 8
        l8ui     a9, a2, 10
        bgeu     a8, a9, .Lsec_96_ba
        addi.n   a8, a8, 1
        memw
        j        .Lsec_96_b4
        .byte    0x00
        .byte    0x00
.Lsec_96_b1:
        l8ui     a8, a2, 9
.Lsec_96_b4:
        s8i      a8, a2, 8
        memw
.Lsec_96_ba:
        l32i.n   a10, a5, 44
        l8ui     a8, a10, 6
        addi.n   a8, a8, 1
        s8i      a8, a10, 6
        memw
        bnez.n   a7, .Lsec_96_d4
        l8ui     a8, a10, 5
        addi.n   a8, a8, 1
        s8i      a8, a10, 5
        memw
.Lsec_96_d4:
        l8ui     a8, a2, 18
        addi.n   a8, a8, -1
        extui    a8, a8, 0, 8
        bltui    a8, 2, .Lsec_96_100
        movi     a2, 0x672
        movi.n   a12, 1
        l32r     a15, .Lsec_96_0
        slli     a11, a12, 11
        movi.n   a10, 6
        l32r     a14, .Lsec_96_4
        l32r     a13, .Lsec_96_10
        s32i.n   a2, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_96_24
        callx8   a8
.Lsec_96_fd:
        j        .Lsec_96_fd
.Lsec_96_100:
        .reloc  ., R_XTENSA_ASM_EXPAND, rcReachRetryLimit
        l32r     a8, .Lsec_96_28
        callx8   a8
        beqz.n   a10, .Lsec_96_119
.Lsec_96_108:
        l32i.n   a7, a5, 44
        l32r     a8, .Lsec_96_14
        l32i.n   a4, a7, 0
        and      a4, a4, a8
        bne      a4, a8, .Lsec_96_172
        j        .Lsec_96_141
        .byte    0x00
.Lsec_96_119:
        l32i.n   a7, a5, 44
        l8ui     a8, a7, 6
        l8ui     a7, a6, 21
        bgeu     a8, a7, .Lsec_96_108
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacMSDUAged
        l32r     a8, .Lsec_96_2c
        callx8   a8
        bnez     a10, .Lsec_96_108
        l32i.n   a7, a5, 44
        movi.n   a11, 1
        slli     a11, a11, 28
        l32i.n   a8, a7, 0
        and      a11, a8, a11
        bnez     a11, .Lsec_96_108
        j        .Lsec_96_18f
.Lsec_96_141:
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacIsLongFrame
        l32r     a8, .Lsec_96_30
        callx8   a8
        beqz.n   a10, .Lsec_96_151
        l8ui     a3, a6, 43
        j        .Lsec_96_154
.Lsec_96_151:
        l8ui     a3, a6, 44
.Lsec_96_154:
        mov.n    a11, a5
        mov.n    a10, a2
        s8i      a3, a7, 5
        memw
        movi.n   a3, 0
        s32i     a3, a2, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacEndRetryAMPDUFail
        l32r     a8, .Lsec_96_34
        callx8   a8
        j        .Lsec_96_22c
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_96_172:
        movi.n   a4, 6
        s8i      a4, a2, 18
        memw
        mov.n    a12, a3
        movi.n   a11, 1
        beqz     a3, .Lsec_96_1e1
        mov.n    a10, a2
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacDiscardFrameExchangeSequence
        l32r     a8, .Lsec_96_38
        callx8   a8
        j        .Lsec_96_22c
.Lsec_96_18f:
        bnez     a3, .Lsec_96_1f0
        bnez     a4, .Lsec_96_1dd
        l32r     a3, .Lsec_96_18
        bany     a8, a3, .Lsec_96_1b8
        l32i.n   a3, a5, 4
        l32i.n   a3, a3, 4
        bbci     a8, 18, .Lsec_96_1a4
        addi.n   a3, a3, 4
.Lsec_96_1a4:
        movi.n   a5, 8
        l8ui     a4, a3, 1
        or       a4, a4, a5
        s8i      a4, a3, 1
        memw
        memw
        j        .Lsec_96_1dd
.Lsec_96_1b8:
        bbci     a8, 22, .Lsec_96_1dd
        movi.n   a6, 8
.Lsec_96_1bd:
        l32i.n   a4, a5, 44
        l32i.n   a3, a5, 4
        l32i     a4, a4, 0
        l32i.n   a3, a3, 4
        bbci     a4, 18, .Lsec_96_1cc
        addi     a3, a3, 4
.Lsec_96_1cc:
        l8ui     a4, a3, 1
        or       a4, a4, a6
        s8i      a4, a3, 1
        memw
        l32i.n   a5, a5, 40
        bnez     a5, .Lsec_96_1bd
.Lsec_96_1dd:
        movi.n   a12, 1
        mov.n    a11, a12
.Lsec_96_1e1:
        mov.n    a10, a2
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacEndFrameExchangeSequence
        l32r     a8, .Lsec_96_3c
        callx8   a8
        j        .Lsec_96_22c
        .byte    0x00
.Lsec_96_1f0:
        l8ui     a3, a2, 18
        beqi     a3, 1, .Lsec_96_219
        movi     a2, 0x6b9
        movi.n   a12, 1
        l32r     a15, .Lsec_96_0
        slli     a11, a12, 11
        movi.n   a10, 6
        l32r     a14, .Lsec_96_4
        l32r     a13, .Lsec_96_1c
        s32i.n   a2, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_96_40
        callx8   a8
.Lsec_96_214:
        j        .Lsec_96_214
        .byte    0x00
        .byte    0x00
.Lsec_96_219:
        movi.n   a3, 4
        s8i      a3, a2, 18
        memw
        mov.n    a10, a2
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacRetryTxFrame
        l32r     a8, .Lsec_96_44
        callx8   a8
.Lsec_96_22c:
        retw.n
        .size   lmacProcessShortRetryFail, .-lmacProcessShortRetryFail
.Lsec_96_22e:

        .section .text.lmacProcessCtsTimeout,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_97_0, .Lsec_170_0
        .literal .Lsec_97_4, .Lsec_120_0
        .literal .Lsec_97_8, .Lsec_68_0
        .literal .Lsec_97_c, .Lsec_144_0
        .literal .Lsec_97_10, wifi_log
        .literal .Lsec_97_14, lmacProcessShortRetryFail
.Lsec_97_18:
        .global lmacProcessCtsTimeout
        .type   lmacProcessCtsTimeout, @function
lmacProcessCtsTimeout:
        entry    a1, 48
        extui    a2, a2, 0, 8
        extui    a13, a3, 0, 8
        bnei     a2, 10, .Lsec_97_30
        movi     a2, 0
        memw
        l32i     a2, a2, 44
        break    1, 15
.Lsec_97_30:
        l32r     a10, .Lsec_97_0
        addx8    a8, a2, a2
        addx4    a8, a8, a10
        l8ui     a8, a8, 18
        addi.n   a8, a8, -1
        extui    a8, a8, 0, 8
        bltui    a8, 2, .Lsec_97_68
        movi     a2, 0x789
        movi.n   a12, 1
        l32r     a15, .Lsec_97_4
        slli     a11, a12, 11
        movi     a10, 6
        l32r     a14, .Lsec_97_8
        l32r     a13, .Lsec_97_c
        s32i     a2, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_97_10
        callx8   a8
.Lsec_97_64:
        j        .Lsec_97_64
        .byte    0x00
.Lsec_97_68:
        addx8    a2, a2, a2
        movi.n   a12, 1
        movi.n   a11, 0
        addx4    a10, a2, a10
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacProcessShortRetryFail
        l32r     a8, .Lsec_97_14
        callx8   a8
        retw.n
        .size   lmacProcessCtsTimeout, .-lmacProcessCtsTimeout
.Lsec_97_7a:

        .section .text.lmacProcessLongRetryFail,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_98_0, lmacConfMib
        .literal .Lsec_98_4, .Lsec_121_0
        .literal .Lsec_98_8, .Lsec_68_0
        .literal .Lsec_98_c, .Lsec_146_0
        .literal .Lsec_98_10, 0x00400002
        .literal .Lsec_98_14, .Lsec_145_0
        .literal .Lsec_98_18, wifi_log
        .literal .Lsec_98_1c, rcReachRetryLimit
        .literal .Lsec_98_20, lmacMSDUAged
        .literal .Lsec_98_24, lmacDiscardFrameExchangeSequence
        .literal .Lsec_98_28, lmacEndFrameExchangeSequence
        .literal .Lsec_98_2c, wifi_log
        .literal .Lsec_98_30, lmacRetryTxFrame
.Lsec_98_34:
        .global lmacProcessLongRetryFail
        .type   lmacProcessLongRetryFail, @function
lmacProcessLongRetryFail:
        entry    a1, 48
        extui    a4, a4, 0, 8
        movi.n   a5, 0
        extui    a3, a3, 0, 8
        bne      a4, a5, .Lsec_98_44
        l32i.n   a5, a2, 0
.Lsec_98_44:
        l32r     a6, .Lsec_98_0
        l8ui     a8, a2, 12
        l8ui     a9, a6, 20
        bgeu     a8, a9, .Lsec_98_71
        addi.n   a8, a8, 1
        extui    a8, a8, 0, 8
        s8i      a8, a2, 12
        memw
        bgeu     a8, a9, .Lsec_98_71
        l8ui     a8, a2, 8
        l8ui     a9, a2, 10
        bgeu     a8, a9, .Lsec_98_7a
        addi.n   a8, a8, 1
        memw
        j        .Lsec_98_74
        .byte    0x00
        .byte    0x00
.Lsec_98_71:
        l8ui     a8, a2, 9
.Lsec_98_74:
        s8i      a8, a2, 8
        memw
.Lsec_98_7a:
        l32i.n   a10, a5, 44
        l8ui     a8, a10, 7
        addi.n   a8, a8, 1
        s8i      a8, a10, 7
        memw
        bnez.n   a3, .Lsec_98_94
        l8ui     a8, a10, 5
        addi.n   a8, a8, 1
        s8i      a8, a10, 5
        memw
.Lsec_98_94:
        l8ui     a8, a2, 18
        addi.n   a8, a8, -1
        extui    a8, a8, 0, 8
        bltui    a8, 2, .Lsec_98_c0
        movi     a2, 0x747
        movi.n   a12, 1
        l32r     a15, .Lsec_98_4
        slli     a11, a12, 11
        movi.n   a10, 6
        l32r     a14, .Lsec_98_8
        l32r     a13, .Lsec_98_c
        s32i.n   a2, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_98_18
        callx8   a8
.Lsec_98_bd:
        j        .Lsec_98_bd
.Lsec_98_c0:
        .reloc  ., R_XTENSA_ASM_EXPAND, rcReachRetryLimit
        l32r     a8, .Lsec_98_1c
        callx8   a8
        beqz.n   a10, .Lsec_98_d9
.Lsec_98_c8:
        movi.n   a5, 6
        s8i      a5, a2, 18
        memw
        beqz.n   a3, .Lsec_98_f2
        memw
        j        .Lsec_98_fc
        .byte    0x00
.Lsec_98_d9:
        l32i.n   a8, a5, 44
        l8ui     a6, a6, 20
        l8ui     a8, a8, 7
        bgeu     a8, a6, .Lsec_98_c8
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacMSDUAged
        l32r     a8, .Lsec_98_20
        callx8   a8
        bnez     a10, .Lsec_98_c8
        j        .Lsec_98_107
.Lsec_98_f2:
        mov.n    a12, a4
        movi.n   a11, 1
        beqz     a4, .Lsec_98_15d
        j        .Lsec_98_1a9
.Lsec_98_fc:
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacDiscardFrameExchangeSequence
        l32r     a8, .Lsec_98_24
        callx8   a8
        j        .Lsec_98_1a9
.Lsec_98_107:
        bnez     a3, .Lsec_98_16c
        l32i.n   a3, a5, 44
        l32r     a6, .Lsec_98_10
        l32i.n   a3, a3, 0
        bany     a3, a6, .Lsec_98_131
        l32i.n   a5, a5, 4
        l32i.n   a5, a5, 4
        bbci     a3, 18, .Lsec_98_11d
        addi.n   a5, a5, 4
.Lsec_98_11d:
        movi.n   a6, 8
        l8ui     a3, a5, 1
        or       a3, a3, a6
        s8i      a3, a5, 1
        memw
        memw
        j        .Lsec_98_155
.Lsec_98_131:
        bbci     a3, 22, .Lsec_98_155
        movi.n   a8, 8
.Lsec_98_136:
        l32i.n   a3, a5, 4
        l32i.n   a6, a3, 4
        l32i.n   a3, a5, 44
        l32i.n   a3, a3, 0
        bbci     a3, 18, .Lsec_98_144
        addi     a6, a6, 4
.Lsec_98_144:
        l8ui     a3, a6, 1
        or       a3, a3, a8
        s8i      a3, a6, 1
        memw
        l32i.n   a5, a5, 40
        bnez     a5, .Lsec_98_136
.Lsec_98_155:
        bnez     a4, .Lsec_98_1a9
        movi.n   a12, 1
        or       a11, a12, a12
.Lsec_98_15d:
        mov.n    a10, a2
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacEndFrameExchangeSequence
        l32r     a8, .Lsec_98_28
        callx8   a8
        j        .Lsec_98_1a9
        .byte    0x00
.Lsec_98_16c:
        l8ui     a3, a2, 18
        beqi     a3, 1, .Lsec_98_194
        movi     a2, 0x776
        movi.n   a12, 1
        l32r     a15, .Lsec_98_4
        slli     a11, a12, 11
        movi.n   a10, 6
        l32r     a14, .Lsec_98_8
        l32r     a13, .Lsec_98_14
        s32i.n   a2, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_98_2c
        callx8   a8
.Lsec_98_190:
        j        .Lsec_98_190
        .byte    0x00
.Lsec_98_194:
        movi.n   a3, 4
        mov.n    a11, a10
        s8i      a3, a2, 18
        memw
        mov.n    a10, a2
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacRetryTxFrame
        l32r     a8, .Lsec_98_30
        callx8   a8
.Lsec_98_1a9:
        retw.n
        .size   lmacProcessLongRetryFail, .-lmacProcessLongRetryFail
.Lsec_98_1ab:

        .section .text.lmacProcessCollision,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_99_0, .Lsec_124_0
        .literal .Lsec_99_4, .Lsec_68_0
        .literal .Lsec_99_8, .Lsec_156_0
        .literal .Lsec_99_c, .Lsec_170_0
        .literal .Lsec_99_10, .Lsec_155_0
        .literal .Lsec_99_14, .Lsec_154_0
        .literal .Lsec_99_18, wifi_log
        .literal .Lsec_99_1c, wifi_log
        .literal .Lsec_99_20, wifi_log
        .literal .Lsec_99_24, lmacIsLongFrame
        .literal .Lsec_99_28, lmacProcessShortRetryFail
        .literal .Lsec_99_2c, lmacProcessLongRetryFail
.Lsec_99_30:
        .global lmacProcessCollision
        .type   lmacProcessCollision, @function
lmacProcessCollision:
        entry    a1, 48
        extui    a2, a2, 0, 8
        bltui    a2, 5, .Lsec_99_5d
        movi     a2, 0x52f
        movi.n   a12, 1
        l32r     a15, .Lsec_99_0
        slli     a11, a12, 11
        movi.n   a10, 6
        l32r     a14, .Lsec_99_4
        l32r     a13, .Lsec_99_8
        s32i     a2, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_99_18
        callx8   a8
.Lsec_99_58:
        j        .Lsec_99_58
        .byte    0x00
        .byte    0x00
.Lsec_99_5d:
        l32r     a8, .Lsec_99_c
        addx8    a3, a2, a2
        addx4    a3, a3, a8
        l32i.n   a10, a3, 0
        bnez.n   a10, .Lsec_99_8c
        movi     a2, 0x531
        movi.n   a12, 1
        l32r     a15, .Lsec_99_0
        slli     a11, a12, 11
        movi.n   a10, 6
        l32r     a14, .Lsec_99_4
        l32r     a13, .Lsec_99_10
        s32i.n   a2, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_99_1c
        callx8   a8
.Lsec_99_88:
        j        .Lsec_99_88
        .byte    0x00
.Lsec_99_8c:
        l8ui     a4, a3, 18
        beqi     a4, 1, .Lsec_99_b4
        movi     a2, 0x532
        movi.n   a12, 1
        l32r     a15, .Lsec_99_0
        slli     a11, a12, 11
        movi.n   a10, 6
        l32r     a14, .Lsec_99_4
        l32r     a13, .Lsec_99_14
        s32i.n   a2, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_99_20
        callx8   a8
.Lsec_99_b0:
        j        .Lsec_99_b0
        .byte    0x00
.Lsec_99_b4:
        l32i.n   a3, a10, 44
        addx8    a2, a2, a2
        addx4    a2, a2, a8
        l32i.n   a8, a3, 0
        movi     a3, 0x300
        and      a3, a3, a8
        bnez.n   a3, .Lsec_99_ce
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacIsLongFrame
        l32r     a8, .Lsec_99_24
        callx8   a8
        bnez.n   a10, .Lsec_99_e0
.Lsec_99_ce:
        movi.n   a12, 1
        movi.n   a13, 0
        mov.n    a11, a12
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacProcessShortRetryFail
        l32r     a8, .Lsec_99_28
        callx8   a8
        j        .Lsec_99_ec
        .byte    0x00
.Lsec_99_e0:
        mov.n    a12, a3
        mov.n    a11, a4
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacProcessLongRetryFail
        l32r     a8, .Lsec_99_2c
        callx8   a8
.Lsec_99_ec:
        retw.n
        .size   lmacProcessCollision, .-lmacProcessCollision
.Lsec_99_ee:

        .section .text.lmacProcessCollisions_task,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_100_0, hal_mac_get_txq_state
        .literal .Lsec_100_4, hal_mac_txq_disable
        .literal .Lsec_100_8, hal_mac_clr_txq_state
        .literal .Lsec_100_c, lmacProcessCollision
.Lsec_100_10:
        .global lmacProcessCollisions_task
        .type   lmacProcessCollisions_task, @function
lmacProcessCollisions_task:
        entry    a1, 32
        movi     a10, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_get_txq_state
        l32r     a8, .Lsec_100_0
        callx8   a8
        or       a3, a10, a10
        beqz.n   a10, .Lsec_100_60
        movi     a4, 1
.Lsec_100_24:
        neg      a10, a3
        and      a10, a10, a3
        nsau     a10, a10
        neg      a10, a10
        addi     a2, a10, 31
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_txq_disable
        l32r     a8, .Lsec_100_4
        callx8   a8
        ssl      a2
        sll      a5, a4
        mov.n    a11, a5
        movi     a10, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_clr_txq_state
        l32r     a8, .Lsec_100_8
        callx8   a8
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacProcessCollision
        l32r     a8, .Lsec_100_c
        callx8   a8
        movi.n   a11, -1
        xor      a11, a11, a5
        and      a3, a3, a11
        bnez     a3, .Lsec_100_24
.Lsec_100_60:
        retw.n
        .size   lmacProcessCollisions_task, .-lmacProcessCollisions_task
.Lsec_100_62:

        .section .text.lmacProcessAckTimeout,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_101_0, .Lsec_119_0
        .literal .Lsec_101_4, .Lsec_68_0
        .literal .Lsec_101_8, .Lsec_143_0
        .literal .Lsec_101_c, .Lsec_142_0
        .literal .Lsec_101_10, .Lsec_170_0
        .literal .Lsec_101_14, .Lsec_141_0
        .literal .Lsec_101_18, wifi_log
        .literal .Lsec_101_1c, wifi_log
        .literal .Lsec_101_20, wifi_log
        .literal .Lsec_101_24, lmacProcessLongRetryFail
        .literal .Lsec_101_28, lmacProcessShortRetryFail
.Lsec_101_2c:
        .global lmacProcessAckTimeout
        .type   lmacProcessAckTimeout, @function
lmacProcessAckTimeout:
        entry    a1, 48
        extui    a2, a2, 0, 8
        extui    a3, a3, 0, 8
        bnei     a2, 10, .Lsec_101_5b
        movi     a2, 0x799
        movi.n   a12, 1
        l32r     a15, .Lsec_101_0
        slli     a11, a12, 11
        movi     a10, 6
        l32r     a14, .Lsec_101_4
        l32r     a13, .Lsec_101_8
        s32i     a2, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_101_18
        callx8   a8
.Lsec_101_58:
        j        .Lsec_101_58
.Lsec_101_5b:
        bltui    a2, 5, .Lsec_101_80
        movi     a2, 0x79c
        movi.n   a12, 1
        l32r     a15, .Lsec_101_0
        slli     a11, a12, 11
        movi.n   a10, 6
        l32r     a14, .Lsec_101_4
        l32r     a13, .Lsec_101_c
        s32i.n   a2, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_101_1c
        callx8   a8
.Lsec_101_7c:
        j        .Lsec_101_7c
        .byte    0x00
.Lsec_101_80:
        l32r     a8, .Lsec_101_10
        addx8    a10, a2, a2
        addx4    a10, a10, a8
        bnez.n   a3, .Lsec_101_91
        mov.n    a2, a10
        l32i.n   a8, a10, 0
        bnez.n   a8, .Lsec_101_b4
.Lsec_101_91:
        movi     a2, 0x7a7
        movi.n   a12, 1
        l32r     a15, .Lsec_101_0
        slli     a11, a12, 11
        movi.n   a10, 6
        l32r     a14, .Lsec_101_4
        l32r     a13, .Lsec_101_14
        s32i     a2, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_101_20
        callx8   a8
.Lsec_101_b0:
        j        .Lsec_101_b0
        .byte    0x00
.Lsec_101_b4:
        l32i.n   a8, a8, 44
        l32i.n   a9, a8, 0
        movi     a8, 0x100
        and      a11, a8, a9
        beqz.n   a11, .Lsec_101_dc
        mov.n    a12, a3
        l8ui     a8, a10, 9
        mov.n    a11, a3
        s8i      a8, a10, 8
        s8i      a3, a10, 11
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacProcessLongRetryFail
        l32r     a8, .Lsec_101_24
        callx8   a8
        j        .Lsec_101_e6
.Lsec_101_dc:
        mov.n    a13, a11
        mov.n    a12, a11
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacProcessShortRetryFail
        l32r     a8, .Lsec_101_28
        callx8   a8
.Lsec_101_e6:
        retw.n
        .size   lmacProcessAckTimeout, .-lmacProcessAckTimeout
.Lsec_101_e8:

        .section .text.lmacProcessTxRtsError,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_102_0, .Lsec_118_0
        .literal .Lsec_102_4, .Lsec_68_0
        .literal .Lsec_102_8, .Lsec_140_0
        .literal .Lsec_102_c, .Lsec_170_0
        .literal .Lsec_102_10, .Lsec_139_0
        .literal .Lsec_102_14, .Lsec_138_0
        .literal .Lsec_102_18, wifi_log
        .literal .Lsec_102_1c, lmacProcessCollision
        .literal .Lsec_102_20, lmacEndFrameExchangeSequence
        .literal .Lsec_102_24, wifi_log
        .literal .Lsec_102_28, wifi_log
.Lsec_102_2c:
        .global lmacProcessTxRtsError
        .type   lmacProcessTxRtsError, @function
lmacProcessTxRtsError:
        entry    a1, 48
        extui    a10, a3, 0, 8
        extui    a14, a2, 0, 8
        extui    a12, a4, 0, 8
        bnei     a10, 10, .Lsec_102_45
        movi.n   a8, 0
        memw
        l32i.n   a8, a8, 44
        break    1, 15
.Lsec_102_45:
        bltui    a10, 5, .Lsec_102_6c
        movi     a8, 0x7cb
        movi.n   a12, 1
        l32r     a15, .Lsec_102_0
        slli     a11, a12, 11
        movi     a10, 6
        l32r     a14, .Lsec_102_4
        l32r     a13, .Lsec_102_8
        s32i     a8, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_102_18
        callx8   a8
.Lsec_102_68:
        j        .Lsec_102_68
        .byte    0x00
.Lsec_102_6c:
        bgeui    a14, 6, .Lsec_102_78
        bgeui    a14, 3, .Lsec_102_81
        beqi     a14, 1, .Lsec_102_81
        j        .Lsec_102_b1
.Lsec_102_78:
        movi     a8, 192
        beq      a14, a8, .Lsec_102_8c
        j        .Lsec_102_b1
.Lsec_102_81:
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacProcessCollision
        l32r     a8, .Lsec_102_1c
        callx8   a8
        j        .Lsec_102_e4
        .byte    0x00
        .byte    0x00
.Lsec_102_8c:
        l32r     a9, .Lsec_102_c
        addx8    a8, a10, a10
        addx4    a8, a8, a9
        movi.n   a11, 6
        s8i      a11, a8, 18
        memw
        bnez     a12, .Lsec_102_e4
        movi.n   a11, 1
        mov.n    a10, a8
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacEndFrameExchangeSequence
        l32r     a8, .Lsec_102_20
        callx8   a8
        j        .Lsec_102_e4
        .byte    0x00
.Lsec_102_b1:
        l32r     a13, .Lsec_102_10
        movi.n   a12, 2
        movi.n   a11, 64
        movi.n   a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_102_24
        callx8   a8
        movi     a8, 0x7f0
        movi.n   a12, 1
        l32r     a15, .Lsec_102_0
        slli     a11, a12, 11
        movi     a10, 6
        l32r     a14, .Lsec_102_4
        l32r     a13, .Lsec_102_14
        s32i     a8, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_102_28
        callx8   a8
.Lsec_102_e0:
        j        .Lsec_102_e0
        .byte    0x00
.Lsec_102_e4:
        retw.n
        .size   lmacProcessTxRtsError, .-lmacProcessTxRtsError
.Lsec_102_e6:

        .section .text.lmacProcessTxseckiderr,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_103_0, lmacEndFrameExchangeSequence
.Lsec_103_4:
        .global lmacProcessTxseckiderr
        .type   lmacProcessTxseckiderr, @function
lmacProcessTxseckiderr:
        entry    a1, 32
        movi.n   a12, 0
        movi.n   a11, 1
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacEndFrameExchangeSequence
        l32r     a8, .Lsec_103_0
        callx8   a8
        retw.n
        .size   lmacProcessTxseckiderr, .-lmacProcessTxseckiderr
.Lsec_103_16:

        .section .text.lmacProcessTxError,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_104_0, .Lsec_117_0
        .literal .Lsec_104_4, .Lsec_68_0
        .literal .Lsec_104_8, .Lsec_137_0
        .literal .Lsec_104_c, 0x00000809
        .literal .Lsec_104_10, .Lsec_170_0
        .literal .Lsec_104_14, wifi_log
        .literal .Lsec_104_18, lmacProcessCollision
        .literal .Lsec_104_1c, lmacProcessTxseckiderr
        .literal .Lsec_104_20, lmacProcessCtsTimeout
        .literal .Lsec_104_24, lmacProcessAckTimeout
.Lsec_104_28:
        .global lmacProcessTxError
        .type   lmacProcessTxError, @function
lmacProcessTxError:
        entry    a1, 48
        extui    a10, a2, 0, 8
        extui    a3, a3, 0, 8
        extui    a11, a4, 0, 8
        bnei     a10, 10, .Lsec_104_41
        movi.n   a3, 0
        memw
        l32i.n   a3, a3, 44
        break    1, 15
.Lsec_104_41:
        bltui    a10, 5, .Lsec_104_68
        l32r     a3, .Lsec_104_c
        movi.n   a12, 1
        slli     a11, a12, 11
        movi     a10, 6
        l32r     a15, .Lsec_104_0
        l32r     a14, .Lsec_104_4
        l32r     a13, .Lsec_104_8
        s32i     a3, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_104_14
        callx8   a8
.Lsec_104_64:
        j        .Lsec_104_64
        .byte    0x00
.Lsec_104_68:
        bgeui    a3, 6, .Lsec_104_78
        bgeui    a3, 3, .Lsec_104_81
        beqz     a3, .Lsec_104_b8
        beqi     a3, 1, .Lsec_104_81
        j        .Lsec_104_c2
        .byte    0x00
.Lsec_104_78:
        movi     a8, 192
        beq      a3, a8, .Lsec_104_91
        j        .Lsec_104_c2
.Lsec_104_81:
        bnez.n   a11, .Lsec_104_8c
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacProcessCollision
        l32r     a8, .Lsec_104_18
        callx8   a8
        j        .Lsec_104_c8
.Lsec_104_8c:
        movi.n   a11, 1
        j        .Lsec_104_c2
.Lsec_104_91:
        l32r     a8, .Lsec_104_10
        addx8    a3, a10, a10
        addx4    a3, a3, a8
        movi.n   a9, 6
        s8i      a9, a3, 18
        memw
        bnez.n   a11, .Lsec_104_c8
        addx8    a3, a10, a10
        mov.n    a11, a10
        addx4    a10, a3, a8
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacProcessTxseckiderr
        l32r     a8, .Lsec_104_1c
        callx8   a8
        j        .Lsec_104_c8
.Lsec_104_b8:
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacProcessCtsTimeout
        l32r     a8, .Lsec_104_20
        callx8   a8
        j        .Lsec_104_c8
        .byte    0x00
.Lsec_104_c2:
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacProcessAckTimeout
        l32r     a8, .Lsec_104_24
        callx8   a8
.Lsec_104_c8:
        retw.n
        .size   lmacProcessTxError, .-lmacProcessTxError
.Lsec_104_ca:

        .section .wifiextrairam.34,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_105_0, g_osi_funcs_p
        .literal .Lsec_105_4, g_pp_tx_pkt_num
        .literal .Lsec_105_8, .Lsec_170_0
        .literal .Lsec_105_c, .Lsec_152_0
        .literal .Lsec_105_10, .Lsec_106_0
        .literal .Lsec_105_14, .Lsec_123_0
        .literal .Lsec_105_18, .Lsec_68_0
        .literal .Lsec_105_1c, .Lsec_151_0
        .literal .Lsec_105_20, hal_mac_get_txq_state
        .literal .Lsec_105_24, pm_is_sleeping
        .literal .Lsec_105_28, tx_pwctrl_background
        .literal .Lsec_105_2c, hal_mac_clr_txq_state
        .literal .Lsec_105_30, wifi_log
        .literal .Lsec_105_34, hal_mac_get_txq_pmd
        .literal .Lsec_105_38, hal_mac_clr_txq_state
        .literal .Lsec_105_3c, wifi_gpio_debug
        .literal .Lsec_105_40, lmacProcessTxSuccess
        .literal .Lsec_105_44, lmacProcessTxRtsError
        .literal .Lsec_105_48, lmacProcessCtsTimeout
        .literal .Lsec_105_4c, lmacProcessAckTimeout
        .literal .Lsec_105_50, lmacProcessTxError
        .literal .Lsec_105_54, wifi_log
.Lsec_105_58:
        .global lmacProcessTxComplete
        .type   lmacProcessTxComplete, @function
lmacProcessTxComplete:
        entry    a1, 64
        movi     a10, 2
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_get_txq_state
        l32r     a8, .Lsec_105_20
        callx8   a8
        l32r     a2, .Lsec_105_0
        mov.n    a3, a10
        l32i.n   a2, a2, 0
        l32i.n   a10, a2, 4
        callx8   a10
        beqz.n   a10, .Lsec_105_a8
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_is_sleeping
        l32r     a8, .Lsec_105_24
        callx8   a8
        bnez.n   a10, .Lsec_105_a8
        l32r     a4, .Lsec_105_4
        l8ui     a2, a4, 0
        addi.n   a2, a2, 1
        extui    a2, a2, 0, 8
        bgeui    a2, 5, .Lsec_105_95
        s8i      a2, a4, 0
        memw
        memw
        j        .Lsec_105_a8
        .byte    0x00
.Lsec_105_95:
        s8i      a10, a4, 0
        memw
        mov.n    a11, a10
        movi.n   a10, 1
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, tx_pwctrl_background
        l32r     a8, .Lsec_105_28
        callx8   a8
.Lsec_105_a8:
        movi.n   a2, 0
        s32i.n   a2, a1, 16
        movi.n   a4, 1
        memw
        j        .Lsec_105_1bf
.Lsec_105_b4:
        neg      a2, a3
        and      a2, a2, a3
        nsau     a2, a2
        neg      a2, a2
        addi     a2, a2, 31
        ssl      a2
        sll      a7, a4
        movi.n   a5, -1
        xor      a5, a5, a7
        and      a3, a3, a5
        l32r     a6, .Lsec_105_8
        slli     a5, a2, 3
        bgei     a2, 5, .Lsec_105_e5
        add.n    a8, a5, a2
        addx4    a8, a8, a6
        l8ui     a8, a8, 18
        beqi     a8, 1, .Lsec_105_10d
.Lsec_105_e5:
        mov.n    a11, a7
        add.n    a5, a5, a2
        movi.n   a10, 2
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_clr_txq_state
        l32r     a8, .Lsec_105_2c
        callx8   a8
        addx4    a5, a5, a6
        l8ui     a14, a5, 18
        mov.n    a15, a2
        movi.n   a12, 3
        movi.n   a11, 64
        movi.n   a10, 6
        l32r     a13, .Lsec_105_c
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_105_30
        callx8   a8
        j        .Lsec_105_1bf
        .byte    0x00
        .byte    0x00
.Lsec_105_10d:
        addi     a11, a1, 16
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_get_txq_pmd
        l32r     a8, .Lsec_105_34
        callx8   a8
        mov.n    a11, a7
        movi.n   a10, 2
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_clr_txq_state
        l32r     a8, .Lsec_105_38
        callx8   a8
        movi.n   a8, 15
        l32i.n   a5, a1, 16
        slli     a8, a8, 12
        and      a5, a5, a8
        movi.n   a11, 3
        movi.n   a6, 2
        moveqz   a11, a6, a5
        movi     a10, 10
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_gpio_debug
        l32r     a8, .Lsec_105_3c
        callx8   a8
        l32i.n   a8, a1, 16
        extui    a8, a8, 12, 4
        bgeui    a8, 6, .Lsec_105_19c
        l32r     a5, .Lsec_105_10
        addx4    a8, a8, a5
        l32i.n   a5, a8, 0
        jx       a5
        .byte    0x00
        .byte    0x00
.Lsec_105_151:
        l8ui     a11, a1, 18
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacProcessTxSuccess
        l32r     a8, .Lsec_105_40
        callx8   a8
        j        .Lsec_105_1bf
.Lsec_105_15f:
        l8ui     a10, a1, 16
        movi.n   a12, 0
        mov.n    a11, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacProcessTxRtsError
        l32r     a8, .Lsec_105_44
        callx8   a8
        j        .Lsec_105_1bf
.Lsec_105_16f:
        movi.n   a11, 0
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacProcessCtsTimeout
        l32r     a8, .Lsec_105_48
        callx8   a8
        j        .Lsec_105_1bf
        .byte    0x00
        .byte    0x00
.Lsec_105_17e:
        movi.n   a11, 0
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacProcessAckTimeout
        l32r     a8, .Lsec_105_4c
        callx8   a8
        j        .Lsec_105_1bf
.Lsec_105_18b:
        l8ui     a11, a1, 16
        movi.n   a12, 0
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacProcessTxError
        l32r     a8, .Lsec_105_50
        callx8   a8
        j        .Lsec_105_1bf
        .byte    0x00
.Lsec_105_19c:
        movi     a2, 0x627
        movi.n   a12, 1
        l32r     a15, .Lsec_105_14
        slli     a11, a12, 11
        movi     a10, 6
        l32r     a14, .Lsec_105_18
        l32r     a13, .Lsec_105_1c
        s32i     a2, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_105_54
        callx8   a8
.Lsec_105_1bc:
        j        .Lsec_105_1bc
.Lsec_105_1bf:
        bnez     a3, .Lsec_105_b4
        retw.n
        .size   lmacProcessTxComplete, .-lmacProcessTxComplete
.Lsec_105_1c4:

        .section .rodata,"a",@progbits
        .align  4
.Lsec_106_0:
        .word   .Lsec_105_151
        .word   .Lsec_105_15f
        .word   .Lsec_105_16f
        .word   .Lsec_105_19c
        .word   .Lsec_105_18b
        .word   .Lsec_105_17e

        .section .text.lmacRxDone,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_107_0, ppEnqueueRxq
        .literal .Lsec_107_4, pp_post
.Lsec_107_8:
        .global lmacRxDone
        .type   lmacRxDone, @function
lmacRxDone:
        entry    a1, 32
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, ppEnqueueRxq
        l32r     a8, .Lsec_107_0
        callx8   a8
        movi.n   a11, 0
        movi.n   a10, 17
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_post
        l32r     a8, .Lsec_107_4
        callx8   a8
        retw.n
        .size   lmacRxDone, .-lmacRxDone
.Lsec_107_20:

        .section .text.lmacDisableTransmit,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_108_0, .Lsec_170_0
        .literal .Lsec_108_4, hal_mac_tx_set_cca
        .literal .Lsec_108_8, ets_delay_us
        .literal .Lsec_108_c, hal_mac_is_txq_valid
        .literal .Lsec_108_10, hal_mac_set_txq_invalid
        .literal .Lsec_108_14, hal_mac_tx_set_cca
        .literal .Lsec_108_18, hal_mac_txq_disable
        .literal .Lsec_108_1c, lmacDiscardFrameExchangeSequence
.Lsec_108_20:
        .global lmacDisableTransmit
        .type   lmacDisableTransmit, @function
lmacDisableTransmit:
        entry    a1, 32
        movi     a10, 3
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_tx_set_cca
        l32r     a8, .Lsec_108_4
        callx8   a8
        movi     a10, 16
        extui    a2, a2, 0, 8
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_108_8
        callx8   a8
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_is_txq_valid
        l32r     a8, .Lsec_108_c
        callx8   a8
        mov.n    a5, a10
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_set_txq_invalid
        l32r     a8, .Lsec_108_10
        callx8   a8
        movi.n   a10, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_tx_set_cca
        l32r     a8, .Lsec_108_14
        callx8   a8
        slli     a3, a2, 3
        l32r     a4, .Lsec_108_0
        beqz.n   a5, .Lsec_108_87
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_txq_disable
        l32r     a8, .Lsec_108_18
        callx8   a8
        add.n    a3, a3, a2
        addx4    a3, a3, a4
        movi     a8, 6
        s8i      a8, a3, 18
        memw
        l32i     a3, a3, 0
        beqz.n   a3, .Lsec_108_a0
        addx8    a2, a2, a2
        addx4    a10, a2, a4
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacDiscardFrameExchangeSequence
        l32r     a8, .Lsec_108_1c
        callx8   a8
        j        .Lsec_108_a0
.Lsec_108_87:
        add.n    a3, a3, a2
        addx4    a3, a3, a4
        l32i.n   a2, a3, 0
        beqz.n   a2, .Lsec_108_a0
        movi.n   a3, 1
        slli     a3, a3, 17
        l32i.n   a4, a2, 44
        l32i     a2, a4, 0
        or       a2, a2, a3
        s32i     a2, a4, 0
.Lsec_108_a0:
        memw
        retw.n
        .size   lmacDisableTransmit, .-lmacDisableTransmit
.Lsec_108_a5:

        .section .text.lmacProcessTxTimeout,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_109_0, hal_mac_get_txq_state
        .literal .Lsec_109_4, lmacDisableTransmit
        .literal .Lsec_109_8, hal_mac_clr_txq_state
.Lsec_109_c:
        .global lmacProcessTxTimeout
        .type   lmacProcessTxTimeout, @function
lmacProcessTxTimeout:
        entry    a1, 32
        movi     a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_get_txq_state
        l32r     a8, .Lsec_109_0
        callx8   a8
        or       a3, a10, a10
        beqz.n   a10, .Lsec_109_54
        movi     a4, 1
.Lsec_109_20:
        neg      a10, a3
        and      a10, a10, a3
        nsau     a10, a10
        neg      a10, a10
        addi     a2, a10, 31
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacDisableTransmit
        l32r     a8, .Lsec_109_4
        callx8   a8
        ssl      a2
        sll      a2, a4
        mov.n    a11, a2
        movi     a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_clr_txq_state
        l32r     a8, .Lsec_109_8
        callx8   a8
        movi.n   a8, -1
        xor      a2, a8, a2
        and      a3, a3, a2
        bnez     a3, .Lsec_109_20
.Lsec_109_54:
        retw.n
        .size   lmacProcessTxTimeout, .-lmacProcessTxTimeout
.Lsec_109_56:

        .section .text.lmacStopTransmit,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_110_0, .Lsec_170_0
        .literal .Lsec_110_4, pTxRx
        .literal .Lsec_110_8, hal_mac_tx_set_cca
        .literal .Lsec_110_c, ets_delay_us
        .literal .Lsec_110_10, hal_mac_is_txq_valid
        .literal .Lsec_110_14, hal_mac_set_txq_invalid
        .literal .Lsec_110_18, hal_mac_tx_set_cca
        .literal .Lsec_110_1c, hal_mac_txq_disable
        .literal .Lsec_110_20, ppDirectRecycleAmpdu
        .literal .Lsec_110_24, esf_buf_recycle
        .literal .Lsec_110_28, ppDirectRecycleAmpdu
.Lsec_110_2c:
        .global lmacStopTransmit
        .type   lmacStopTransmit, @function
lmacStopTransmit:
        entry    a1, 32
        movi     a10, 3
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_tx_set_cca
        l32r     a8, .Lsec_110_8
        callx8   a8
        movi     a10, 16
        extui    a2, a2, 0, 8
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_110_c
        callx8   a8
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_is_txq_valid
        l32r     a8, .Lsec_110_10
        callx8   a8
        mov.n    a6, a10
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_set_txq_invalid
        l32r     a8, .Lsec_110_14
        callx8   a8
        movi.n   a10, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_tx_set_cca
        l32r     a8, .Lsec_110_18
        callx8   a8
        l32r     a4, .Lsec_110_0
        addx8    a5, a2, a2
        addx4    a5, a5, a4
        slli     a3, a2, 3
        l32i     a5, a5, 0
        beqz     a6, .Lsec_110_c9
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_txq_disable
        l32r     a8, .Lsec_110_1c
        callx8   a8
        beqz     a5, .Lsec_110_100
        l32i.n   a8, a5, 44
        mov.n    a10, a5
        l32i.n   a9, a8, 0
        bbsi     a9, 22, .Lsec_110_a4
        l8ui     a9, a5, 26
        bnei     a9, 5, .Lsec_110_ad
        l32i.n   a8, a8, 16
        extui    a8, a8, 20, 4
        addi.n   a8, a8, 1
        addx2    a8, a8, a8
        l32r     a5, .Lsec_110_4
        slli     a8, a8, 4
        l32i.n   a5, a5, 0
        add.n    a5, a5, a8
        l32i     a10, a5, 4
        beqz     a10, .Lsec_110_b3
.Lsec_110_a4:
        .reloc  ., R_XTENSA_ASM_EXPAND, ppDirectRecycleAmpdu
        l32r     a8, .Lsec_110_20
        callx8   a8
        j        .Lsec_110_b3
.Lsec_110_ad:
        .reloc  ., R_XTENSA_ASM_EXPAND, esf_buf_recycle
        l32r     a8, .Lsec_110_24
        callx8   a8
.Lsec_110_b3:
        add.n    a3, a3, a2
        addx4    a3, a3, a4
        movi.n   a2, 0
        s32i.n   a2, a3, 0
        s8i      a2, a3, 18
        memw
        memw
        j        .Lsec_110_100
        .byte    0x00
.Lsec_110_c9:
        beqz.n   a5, .Lsec_110_100
        l32i.n   a3, a5, 44
        movi.n   a4, 1
        slli     a4, a4, 17
        l32i.n   a2, a3, 0
        or       a2, a2, a4
        s32i.n   a2, a3, 0
        l8ui     a2, a5, 26
        bnei     a2, 5, .Lsec_110_100
        l32i.n   a8, a3, 16
        extui    a8, a8, 20, 4
        addi.n   a8, a8, 1
        addx2    a8, a8, a8
        l32r     a2, .Lsec_110_4
        slli     a8, a8, 4
        l32i.n   a2, a2, 0
        add.n    a2, a2, a8
        l32i.n   a10, a2, 4
        beqz.n   a10, .Lsec_110_100
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, ppDirectRecycleAmpdu
        l32r     a8, .Lsec_110_28
        callx8   a8
.Lsec_110_100:
        retw.n
        .size   lmacStopTransmit, .-lmacStopTransmit
.Lsec_110_102:

        .section .text.dbg_lmac_get_acs,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_111_0, .Lsec_170_0
.Lsec_111_4:
        .global dbg_lmac_get_acs
        .type   dbg_lmac_get_acs, @function
dbg_lmac_get_acs:
        entry    a1, 32
        l32r     a2, .Lsec_111_0
        retw.n
        .size   dbg_lmac_get_acs, .-dbg_lmac_get_acs
.Lsec_111_c:

        .section .text.lmac_stop_hw_txq,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_112_0, .Lsec_131_0
        .literal .Lsec_112_4, .Lsec_170_0
        .literal .Lsec_112_8, .Lsec_130_0
        .literal .Lsec_112_c, wifi_log
        .literal .Lsec_112_10, wifi_log
        .literal .Lsec_112_14, lmacStopTransmit
.Lsec_112_18:
        .global lmac_stop_hw_txq
        .type   lmac_stop_hw_txq, @function
lmac_stop_hw_txq:
        entry    a1, 32
        l32r     a13, .Lsec_112_0
        movi     a12, 3
        movi.n   a11, 1
        movi     a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_112_c
        callx8   a8
        l32r     a3, .Lsec_112_4
        movi.n   a2, 0
.Lsec_112_31:
        l8ui     a15, a3, 18
        beqz.n   a15, .Lsec_112_50
        movi.n   a10, 6
        mov.n    a14, a2
        l32r     a13, .Lsec_112_8
        movi.n   a12, 2
        movi     a11, 64
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_112_10
        callx8   a8
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacStopTransmit
        l32r     a8, .Lsec_112_14
        callx8   a8
.Lsec_112_50:
        addi.n   a2, a2, 1
        addi     a3, a3, 36
        bnei     a2, 5, .Lsec_112_31
        movi.n   a2, 0
        retw.n
        .size   lmac_stop_hw_txq, .-lmac_stop_hw_txq
.Lsec_112_5c:

        .section .wifi0iram.63,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_113_0, .Lsec_170_0
.Lsec_113_4:
        .global is_lmac_idle
        .type   is_lmac_idle, @function
is_lmac_idle:
        entry    a1, 32
        l32r     a9, .Lsec_113_0
        movi     a8, 5
        loop     a8, .Lsec_113_18
        l8ui     a2, a9, 18
        bnez.n   a2, .Lsec_113_1d
        addi     a9, a9, 36
.Lsec_113_18:
        movi.n   a2, 1
        j        .Lsec_113_1f
.Lsec_113_1d:
        movi.n   a2, 0
.Lsec_113_1f:
        retw.n
        .size   is_lmac_idle, .-is_lmac_idle
.Lsec_113_21:

        .section .text.esp_wifi_set_11b_tx_plcp,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_114_0, lmacConfMib
.Lsec_114_4:
        .global esp_wifi_set_11b_tx_plcp
        .type   esp_wifi_set_11b_tx_plcp, @function
esp_wifi_set_11b_tx_plcp:
        entry    a1, 32
        l32r     a8, .Lsec_114_0
        s8i      a3, a8, 45
        s8i      a2, a8, 46
        memw
        memw
        retw.n
        .size   esp_wifi_set_11b_tx_plcp, .-esp_wifi_set_11b_tx_plcp
.Lsec_114_18:

        .section .rodata.__FUNCTION__$0,"a",@progbits
        .align  1
.Lsec_115_0:
        .byte   0x6c, 0x6d, 0x61, 0x63, 0x54, 0x78, 0x44, 0x6f, 0x6e, 0x65, 0x00

        .section .rodata.__FUNCTION__$1,"a",@progbits
        .align  1
.Lsec_116_0:
        .byte   0x6c, 0x6d, 0x61, 0x63, 0x54, 0x78, 0x46, 0x72, 0x61, 0x6d, 0x65, 0x00

        .section .rodata.__FUNCTION__$2,"a",@progbits
        .align  1
.Lsec_117_0:
        .byte   0x6c, 0x6d, 0x61, 0x63, 0x50, 0x72, 0x6f, 0x63, 0x65, 0x73, 0x73, 0x54, 0x78, 0x45, 0x72, 0x72
        .byte   0x6f, 0x72, 0x00

        .section .rodata.__FUNCTION__$3,"a",@progbits
        .align  1
.Lsec_118_0:
        .byte   0x6c, 0x6d, 0x61, 0x63, 0x50, 0x72, 0x6f, 0x63, 0x65, 0x73, 0x73, 0x54, 0x78, 0x52, 0x74, 0x73
        .byte   0x45, 0x72, 0x72, 0x6f, 0x72, 0x00

        .section .rodata.__FUNCTION__$4,"a",@progbits
        .align  1
.Lsec_119_0:
        .byte   0x6c, 0x6d, 0x61, 0x63, 0x50, 0x72, 0x6f, 0x63, 0x65, 0x73, 0x73, 0x41, 0x63, 0x6b, 0x54, 0x69
        .byte   0x6d, 0x65, 0x6f, 0x75, 0x74, 0x00

        .section .rodata.__FUNCTION__$5,"a",@progbits
        .align  1
.Lsec_120_0:
        .byte   0x6c, 0x6d, 0x61, 0x63, 0x50, 0x72, 0x6f, 0x63, 0x65, 0x73, 0x73, 0x43, 0x74, 0x73, 0x54, 0x69
        .byte   0x6d, 0x65, 0x6f, 0x75, 0x74, 0x00

        .section .rodata.__FUNCTION__$6,"a",@progbits
        .align  1
.Lsec_121_0:
        .byte   0x6c, 0x6d, 0x61, 0x63, 0x50, 0x72, 0x6f, 0x63, 0x65, 0x73, 0x73, 0x4c, 0x6f, 0x6e, 0x67, 0x52
        .byte   0x65, 0x74, 0x72, 0x79, 0x46, 0x61, 0x69, 0x6c, 0x00

        .section .rodata.__FUNCTION__$7,"a",@progbits
        .align  1
.Lsec_122_0:
        .byte   0x6c, 0x6d, 0x61, 0x63, 0x50, 0x72, 0x6f, 0x63, 0x65, 0x73, 0x73, 0x53, 0x68, 0x6f, 0x72, 0x74
        .byte   0x52, 0x65, 0x74, 0x72, 0x79, 0x46, 0x61, 0x69, 0x6c, 0x00

        .section .rodata.__FUNCTION__$8,"a",@progbits
        .align  1
.Lsec_123_0:
        .byte   0x6c, 0x6d, 0x61, 0x63, 0x50, 0x72, 0x6f, 0x63, 0x65, 0x73, 0x73, 0x54, 0x78, 0x43, 0x6f, 0x6d
        .byte   0x70, 0x6c, 0x65, 0x74, 0x65, 0x00

        .section .rodata.__FUNCTION__$9,"a",@progbits
        .align  1
.Lsec_124_0:
        .byte   0x6c, 0x6d, 0x61, 0x63, 0x50, 0x72, 0x6f, 0x63, 0x65, 0x73, 0x73, 0x43, 0x6f, 0x6c, 0x6c, 0x69
        .byte   0x73, 0x69, 0x6f, 0x6e, 0x00

        .section .rodata.__FUNCTION__$10,"a",@progbits
        .align  1
.Lsec_125_0:
        .byte   0x6c, 0x6d, 0x61, 0x63, 0x45, 0x6e, 0x64, 0x46, 0x72, 0x61, 0x6d, 0x65, 0x45, 0x78, 0x63, 0x68
        .byte   0x61, 0x6e, 0x67, 0x65, 0x53, 0x65, 0x71, 0x75, 0x65, 0x6e, 0x63, 0x65, 0x00

        .section .rodata.__FUNCTION__$11,"a",@progbits
        .align  1
.Lsec_126_0:
        .byte   0x6c, 0x6d, 0x61, 0x63, 0x44, 0x69, 0x73, 0x63, 0x61, 0x72, 0x64, 0x46, 0x72, 0x61, 0x6d, 0x65
        .byte   0x45, 0x78, 0x63, 0x68, 0x61, 0x6e, 0x67, 0x65, 0x53, 0x65, 0x71, 0x75, 0x65, 0x6e, 0x63, 0x65
        .byte   0x00

        .section .rodata.__FUNCTION__$12,"a",@progbits
        .align  1
.Lsec_127_0:
        .byte   0x6c, 0x6d, 0x61, 0x63, 0x50, 0x72, 0x6f, 0x63, 0x65, 0x73, 0x73, 0x54, 0x78, 0x53, 0x75, 0x63
        .byte   0x63, 0x65, 0x73, 0x73, 0x00

        .section .rodata.__FUNCTION__$13,"a",@progbits
        .align  1
.Lsec_128_0:
        .byte   0x6c, 0x6d, 0x61, 0x63, 0x50, 0x72, 0x6f, 0x63, 0x65, 0x73, 0x73, 0x54, 0x42, 0x53, 0x75, 0x63
        .byte   0x63, 0x65, 0x73, 0x73, 0x00

        .section .rodata.__FUNCTION__$14,"a",@progbits
        .align  1
.Lsec_129_0:
        .byte   0x6c, 0x6d, 0x61, 0x63, 0x50, 0x72, 0x6f, 0x63, 0x65, 0x73, 0x73, 0x54, 0x78, 0x6f, 0x70, 0x53
        .byte   0x75, 0x63, 0x63, 0x65, 0x73, 0x73, 0x00

        .section .rodata_wlog_warning.62,"a",@progbits
        .align  1
.Lsec_130_0:
        .byte   0x73, 0x77, 0x20, 0x74, 0x78, 0x71, 0x5b, 0x25, 0x64, 0x5d, 0x20, 0x73, 0x74, 0x61, 0x74, 0x65
        .byte   0x28, 0x25, 0x64, 0x29, 0x20, 0x69, 0x73, 0x20, 0x6e, 0x6f, 0x74, 0x20, 0x69, 0x64, 0x6c, 0x65
        .byte   0x00

        .section .rodata_wlog_info.61,"a",@progbits
        .align  1
.Lsec_131_0:
        .byte   0x6c, 0x6d, 0x61, 0x63, 0x20, 0x73, 0x74, 0x6f, 0x70, 0x20, 0x68, 0x77, 0x20, 0x74, 0x78, 0x71
        .byte   0x00

        .section .rodata_wlog_error.60,"a",@progbits
        .align  1
.Lsec_132_0:
        .byte   0x25, 0x73, 0x20, 0x25, 0x73, 0x20, 0x25, 0x75, 0x0a, 0x00

        .section .rodata_wlog_error.57,"a",@progbits
        .align  1
.Lsec_133_0:
        .byte   0x25, 0x73, 0x20, 0x25, 0x73, 0x20, 0x25, 0x75, 0x0a, 0x00

        .section .rodata_wlog_error.56,"a",@progbits
        .align  1
.Lsec_134_0:
        .byte   0x25, 0x73, 0x20, 0x25, 0x73, 0x20, 0x25, 0x75, 0x0a, 0x00

        .section .rodata_wlog_debug.55,"a",@progbits
        .align  1
.Lsec_135_0:
        .byte   0x64, 0x69, 0x73, 0x63, 0x61, 0x72, 0x64, 0x20, 0x66, 0x6c, 0x61, 0x67, 0x00

        .section .rodata_wlog_info.52,"a",@progbits
        .align  1
.Lsec_136_0:
        .byte   0x74, 0x68, 0x72, 0x65, 0x73, 0x68, 0x6f, 0x6c, 0x64, 0x3a, 0x20, 0x25, 0x64, 0x20, 0x6d, 0x70
        .byte   0x64, 0x75, 0x5f, 0x72, 0x65, 0x74, 0x72, 0x79, 0x3a, 0x20, 0x25, 0x64, 0x20, 0x6c, 0x6f, 0x6e
        .byte   0x67, 0x5f, 0x61, 0x6d, 0x70, 0x64, 0x75, 0x3a, 0x25, 0x64, 0x20, 0x73, 0x68, 0x6f, 0x72, 0x74
        .byte   0x5f, 0x61, 0x6d, 0x70, 0x64, 0x75, 0x3a, 0x25, 0x64, 0x00

        .section .rodata_wlog_error.51,"a",@progbits
        .align  1
.Lsec_137_0:
        .byte   0x25, 0x73, 0x20, 0x25, 0x73, 0x20, 0x25, 0x75, 0x0a, 0x00

        .section .rodata_wlog_error.50,"a",@progbits
        .align  1
.Lsec_138_0:
        .byte   0x25, 0x73, 0x20, 0x25, 0x73, 0x20, 0x25, 0x75, 0x0a, 0x00

        .section .rodata_wlog_warning.49,"a",@progbits
        .align  1
.Lsec_139_0:
        .byte   0x74, 0x78, 0x20, 0x72, 0x74, 0x73, 0x20, 0x65, 0x72, 0x72, 0x6f, 0x72, 0x20, 0x30, 0x78, 0x25
        .byte   0x78, 0x0a, 0x00

        .section .rodata_wlog_error.48,"a",@progbits
        .align  1
.Lsec_140_0:
        .byte   0x25, 0x73, 0x20, 0x25, 0x73, 0x20, 0x25, 0x75, 0x0a, 0x00

        .section .rodata_wlog_error.47,"a",@progbits
        .align  1
.Lsec_141_0:
        .byte   0x25, 0x73, 0x20, 0x25, 0x73, 0x20, 0x25, 0x75, 0x0a, 0x00

        .section .rodata_wlog_error.46,"a",@progbits
        .align  1
.Lsec_142_0:
        .byte   0x25, 0x73, 0x20, 0x25, 0x73, 0x20, 0x25, 0x75, 0x0a, 0x00

        .section .rodata_wlog_error.45,"a",@progbits
        .align  1
.Lsec_143_0:
        .byte   0x25, 0x73, 0x20, 0x25, 0x73, 0x20, 0x25, 0x75, 0x0a, 0x00

        .section .rodata_wlog_error.44,"a",@progbits
        .align  1
.Lsec_144_0:
        .byte   0x25, 0x73, 0x20, 0x25, 0x73, 0x20, 0x25, 0x75, 0x0a, 0x00

        .section .rodata_wlog_error.43,"a",@progbits
        .align  1
.Lsec_145_0:
        .byte   0x25, 0x73, 0x20, 0x25, 0x73, 0x20, 0x25, 0x75, 0x0a, 0x00

        .section .rodata_wlog_error.42,"a",@progbits
        .align  1
.Lsec_146_0:
        .byte   0x25, 0x73, 0x20, 0x25, 0x73, 0x20, 0x25, 0x75, 0x0a, 0x00

        .section .rodata_wlog_info.41,"a",@progbits
        .align  1
.Lsec_147_0:
        .byte   0x64, 0x69, 0x73, 0x63, 0x61, 0x72, 0x64, 0x20, 0x66, 0x6c, 0x61, 0x67, 0x00

        .section .rodata_wlog_error.40,"a",@progbits
        .align  1
.Lsec_148_0:
        .byte   0x25, 0x73, 0x20, 0x25, 0x73, 0x20, 0x25, 0x75, 0x0a, 0x00

        .section .rodata_wlog_error.39,"a",@progbits
        .align  1
.Lsec_149_0:
        .byte   0x25, 0x73, 0x20, 0x25, 0x73, 0x20, 0x25, 0x75, 0x0a, 0x00

        .section .rodata_wlog_error.38,"a",@progbits
        .align  1
.Lsec_150_0:
        .byte   0x25, 0x73, 0x20, 0x25, 0x73, 0x20, 0x25, 0x75, 0x0a, 0x00

        .section .rodata_wlog_error.36,"a",@progbits
        .align  1
.Lsec_151_0:
        .byte   0x25, 0x73, 0x20, 0x25, 0x73, 0x20, 0x25, 0x75, 0x0a, 0x00

        .section .rodata_wlog_info.35,"a",@progbits
        .align  1
.Lsec_152_0:
        .byte   0x69, 0x6e, 0x74, 0x3a, 0x20, 0x73, 0x74, 0x61, 0x74, 0x65, 0x3d, 0x25, 0x64, 0x20, 0x69, 0x3d
        .byte   0x25, 0x64, 0x00

        .section .bss.g_pp_tx_pkt_num,"aw",@nobits
        .align  1
.Lsec_153_0:
        .global g_pp_tx_pkt_num
        .type   g_pp_tx_pkt_num, @object
        .size   g_pp_tx_pkt_num, 1
g_pp_tx_pkt_num:
        .skip   1

        .section .rodata_wlog_error.30,"a",@progbits
        .align  1
.Lsec_154_0:
        .byte   0x25, 0x73, 0x20, 0x25, 0x73, 0x20, 0x25, 0x75, 0x0a, 0x00

        .section .rodata_wlog_error.29,"a",@progbits
        .align  1
.Lsec_155_0:
        .byte   0x25, 0x73, 0x20, 0x25, 0x73, 0x20, 0x25, 0x75, 0x0a, 0x00

        .section .rodata_wlog_error.28,"a",@progbits
        .align  1
.Lsec_156_0:
        .byte   0x25, 0x73, 0x20, 0x25, 0x73, 0x20, 0x25, 0x75, 0x0a, 0x00

        .section .rodata_wlog_error.26,"a",@progbits
        .align  1
.Lsec_157_0:
        .byte   0x25, 0x73, 0x20, 0x25, 0x73, 0x20, 0x25, 0x75, 0x0a, 0x00

        .section .rodata_wlog_error.25,"a",@progbits
        .align  1
.Lsec_158_0:
        .byte   0x25, 0x73, 0x20, 0x25, 0x73, 0x20, 0x25, 0x75, 0x0a, 0x00

        .section .rodata_wlog_error.24,"a",@progbits
        .align  1
.Lsec_159_0:
        .byte   0x25, 0x73, 0x20, 0x25, 0x73, 0x20, 0x25, 0x75, 0x0a, 0x00

        .section .rodata_wlog_error.23,"a",@progbits
        .align  1
.Lsec_160_0:
        .byte   0x25, 0x73, 0x20, 0x25, 0x73, 0x20, 0x25, 0x75, 0x0a, 0x00

        .section .rodata_wlog_error.22,"a",@progbits
        .align  1
.Lsec_161_0:
        .byte   0x25, 0x73, 0x20, 0x25, 0x73, 0x20, 0x25, 0x75, 0x0a, 0x00

        .section .rodata_wlog_error.20,"a",@progbits
        .align  1
.Lsec_162_0:
        .byte   0x25, 0x73, 0x20, 0x25, 0x73, 0x20, 0x25, 0x75, 0x0a, 0x00

        .section .rodata_wlog_error.15,"a",@progbits
        .align  1
.Lsec_163_0:
        .byte   0x25, 0x73, 0x20, 0x25, 0x73, 0x20, 0x25, 0x75, 0x0a, 0x00

        .section .rodata_wlog_error.13,"a",@progbits
        .align  1
.Lsec_164_0:
        .byte   0x25, 0x73, 0x20, 0x25, 0x73, 0x20, 0x25, 0x75, 0x0a, 0x00

        .section .rodata_wlog_error.11,"a",@progbits
        .align  1
.Lsec_165_0:
        .byte   0x25, 0x73, 0x20, 0x25, 0x73, 0x20, 0x25, 0x75, 0x0a, 0x00

        .section .rodata_wlog_error.9,"a",@progbits
        .align  1
.Lsec_166_0:
        .byte   0x25, 0x73, 0x20, 0x25, 0x73, 0x20, 0x25, 0x75, 0x0a, 0x00

        .section .rodata_wlog_error.8,"a",@progbits
        .align  1
.Lsec_167_0:
        .byte   0x25, 0x73, 0x20, 0x25, 0x73, 0x20, 0x25, 0x75, 0x0a, 0x00

        .section .data.txopstart_index,"aw",@progbits
        .align  1
.Lsec_168_0:
        .global txopstart_index
        .type   txopstart_index, @object
        .size   txopstart_index, 1
txopstart_index:
        .byte   0x05

        .section .data.lmacConfMib,"aw",@progbits
        .align  4
.Lsec_169_0:
        .global lmacConfMib
        .type   lmacConfMib, @object
        .size   lmacConfMib, 48
lmacConfMib:
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x01, 0x00, 0x00, 0x00

        .section .bss.our_instances,"aw",@nobits
        .align  4
.Lsec_170_0:
        .skip   180
