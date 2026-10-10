.section .text.rc11NRate2SchedIdx,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_62_0, .Lsec_63_0
.Lsec_62_4:
        entry    a1, 32
        extui    a9, a2, 0, 8
        bgeui    a9, 7, .Lsec_62_1d
        l32r     a2, .Lsec_62_0
        addx4    a2, a9, a2
        l32i.n   a2, a2, 0
        jx       a2
.Lsec_62_18:
        movi.n   a2, 9
        j        .Lsec_62_48
.Lsec_62_1d:
        movi.n   a8, 41
        movi.n   a2, 13
        beq      a9, a8, .Lsec_62_48
        movi.n   a8, 42
        movi.n   a2, 12
        beq      a9, a8, .Lsec_62_48
        movi.n   a8, 31
        movi.n   a2, 0
        beq      a9, a8, .Lsec_62_48
        j        .Lsec_62_3b
        .byte    0x00
.Lsec_62_36:
        movi.n   a2, 11
        j        .Lsec_62_48
.Lsec_62_3b:
        movi.n   a8, 24
        sub      a2, a8, a9
        extui    a2, a2, 0, 8
        j        .Lsec_62_48
.Lsec_62_46:
        movi.n   a2, 10
.Lsec_62_48:
        retw.n

        .section .rodata.rc11NRate2SchedIdx,"a",@progbits
        .align  4
.Lsec_63_0:
        .word   .Lsec_62_36
        .word   .Lsec_62_46
        .word   .Lsec_62_18
        .word   .Lsec_62_3b
        .word   .Lsec_62_3b
        .word   .Lsec_62_46
        .word   .Lsec_62_18

        .section .text.rc11GRate2SchedIdx,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_64_0, .Lsec_143_0
.Lsec_64_4:
        entry    a1, 32
        extui    a9, a2, 0, 8
        movi.n   a8, 42
        movi     a2, 255
        bltu     a8, a9, .Lsec_64_1a
        l32r     a8, .Lsec_64_0
        add.n    a8, a8, a9
        l8ui     a2, a8, 0
.Lsec_64_1a:
        retw.n

        .section .text.rc11BRate2SchedIdx,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_65_0, .Lsec_142_0
.Lsec_65_4:
        entry    a1, 32
        extui    a9, a2, 0, 8
        movi.n   a8, 42
        movi     a2, 255
        bltu     a8, a9, .Lsec_65_1a
        l32r     a8, .Lsec_65_0
        add.n    a8, a8, a9
        l8ui     a2, a8, 0
.Lsec_65_1a:
        retw.n

        .section .text.rcLoRaRate2SchedIdx,"ax",@progbits
        .align  4
.Lsec_66_0:
        .global rcLoRaRate2SchedIdx
        .type   rcLoRaRate2SchedIdx, @function
rcLoRaRate2SchedIdx:
        entry    a1, 32
        extui    a8, a2, 0, 8
        movi.n   a9, 41
        movi.n   a2, 1
        beq      a8, a9, .Lsec_66_1c
        addi     a8, a8, -42
        movi     a9, 0
        moveqz   a2, a9, a8
        neg      a2, a2
        extui    a2, a2, 0, 8
.Lsec_66_1c:
        retw.n
        .size   rcLoRaRate2SchedIdx, .-rcLoRaRate2SchedIdx
.Lsec_66_1e:

        .section .text.rcBuildIndex,"ax",@progbits
        .align  4
.Lsec_67_0:
        entry    a1, 32
        addi.n   a3, a3, -1
        extui    a3, a3, 0, 8
        movi.n   a8, 0
        addi.n   a3, a3, 1
.Lsec_67_c:
        s8i      a8, a2, 10
        memw
        addi.n   a8, a8, 1
        extui    a8, a8, 0, 8
        addi.n   a2, a2, 12
        addi.n   a3, a3, -1
        bnez     a3, .Lsec_67_c
        memw
        retw.n

        .section .text.trc_calc_duration,"ax",@progbits
        .align  4
.Lsec_68_0:
        .global trc_calc_duration
        .type   trc_calc_duration, @function
trc_calc_duration:
        entry    a1, 32
        sub      a8, a2, a3
        bgeu     a2, a3, .Lsec_68_c
        addi     a8, a8, -1
.Lsec_68_c:
        mov.n    a2, a8
        retw.n
        .size   trc_calc_duration, .-trc_calc_duration
.Lsec_68_10:

        .section .text.rx11NRate2AMPDULimit,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_69_0, .Lsec_141_0
.Lsec_69_4:
        .global rx11NRate2AMPDULimit
        .type   rx11NRate2AMPDULimit, @function
rx11NRate2AMPDULimit:
        entry    a1, 32
        addi     a2, a2, -16
        extui    a8, a2, 0, 8
        movi.n   a9, 15
        movi.n   a2, 0
        bltu     a9, a8, .Lsec_69_1d
        l32r     a2, .Lsec_69_0
        addx2    a8, a8, a2
        l16ui    a2, a8, 0
.Lsec_69_1d:
        retw.n
        .size   rx11NRate2AMPDULimit, .-rx11NRate2AMPDULimit
.Lsec_69_1f:

        .section .text.trcAmpduSetState,"ax",@progbits
        .align  4
.Lsec_70_0:
        .global trcAmpduSetState
        .type   trcAmpduSetState, @function
trcAmpduSetState:
        entry    a1, 32
        l8ui     a10, a2, 16
        extui    a3, a3, 0, 8
        movi.n   a8, 0
        movi     a9, 8
        loop     a9, .Lsec_70_1e
        bbc      a10, a8, .Lsec_70_1c
        add.n    a11, a2, a8
        s8i      a3, a11, 17
        memw
.Lsec_70_1c:
        addi.n   a8, a8, 1
.Lsec_70_1e:
        memw
        retw.n
        .size   trcAmpduSetState, .-trcAmpduSetState
.Lsec_70_23:

        .section .text.rcSetTxAmpduLimit,"ax",@progbits
        .align  4
.Lsec_71_0:
        .global rcSetTxAmpduLimit
        .type   rcSetTxAmpduLimit, @function
rcSetTxAmpduLimit:
        entry    a1, 32
        l16ui    a8, a2, 120
        extui    a3, a3, 0, 16
        minu     a9, a8, a3
        srli     a3, a3, 1
        minu     a3, a3, a8
        s16i     a9, a2, 124
        s16i     a3, a2, 122
        memw
        memw
        retw.n
        .size   rcSetTxAmpduLimit, .-rcSetTxAmpduLimit
.Lsec_71_20:

        .section .text.rcUpdateAMPDUParam,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_72_0, 0x00001fff
        .literal .Lsec_72_4, 0x00003fff
        .literal .Lsec_72_8, 0x00007fff
        .literal .Lsec_72_c, 0x0000ffff
.Lsec_72_10:
        .global rcUpdateAMPDUParam
        .type   rcUpdateAMPDUParam, @function
rcUpdateAMPDUParam:
        entry    a1, 32
        extui    a3, a3, 0, 8
        s8i      a3, a2, 130
        memw
        extui    a4, a4, 0, 8
        extui    a5, a5, 0, 8
        bgeui    a3, 4, .Lsec_72_30
        bltui    a3, 2, .Lsec_72_a8
        memw
        j        .Lsec_72_3c
        .byte    0x00
        .byte    0x00
.Lsec_72_30:
        addi     a3, a3, -4
        extui    a3, a3, 0, 8
        bltui    a3, 2, .Lsec_72_78
        j        .Lsec_72_a8
.Lsec_72_3c:
        l32r     a3, .Lsec_72_8
        beqi     a4, 2, .Lsec_72_51
        l32r     a3, .Lsec_72_c
        beqi     a4, 3, .Lsec_72_51
        l32r     a3, .Lsec_72_0
        bnei     a4, 1, .Lsec_72_51
        l32r     a3, .Lsec_72_4
.Lsec_72_51:
        s16i     a3, a2, 120
        memw
        movi.n   a3, 40
        beqi     a5, 5, .Lsec_72_a2
        bltui    a5, 6, .Lsec_72_68
        beqi     a5, 6, .Lsec_72_6d
        memw
        j        .Lsec_72_72
.Lsec_72_68:
        movi.n   a3, 20
        j        .Lsec_72_a2
.Lsec_72_6d:
        movi.n   a3, 76
        j        .Lsec_72_a2
.Lsec_72_72:
        movi     a3, 148
        j        .Lsec_72_a2
.Lsec_72_78:
        l32r     a3, .Lsec_72_8
        beqi     a4, 2, .Lsec_72_8d
        l32r     a3, .Lsec_72_c
        beqi     a4, 3, .Lsec_72_8d
        l32r     a3, .Lsec_72_0
        bnei     a4, 1, .Lsec_72_8d
        l32r     a3, .Lsec_72_4
.Lsec_72_8d:
        s16i     a3, a2, 120
        memw
        beqi     a5, 5, .Lsec_72_6d
        bltui    a5, 6, .Lsec_72_68
        movi     a3, 152
        beqi     a5, 6, .Lsec_72_a2
        movi     a3, 0x12c
.Lsec_72_a2:
        s16i     a3, a2, 126
        memw
.Lsec_72_a8:
        memw
        retw.n
        .size   rcUpdateAMPDUParam, .-rcUpdateAMPDUParam
.Lsec_72_ad:

        .section .text.rcGet11NHighestRateIdx,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_73_0, .Lsec_163_0
        .literal .Lsec_73_4, wifi_log
.Lsec_73_8:
        .global rcGet11NHighestRateIdx
        .type   rcGet11NHighestRateIdx, @function
rcGet11NHighestRateIdx:
        entry    a1, 32
        extui    a14, a2, 0, 8
        movi.n   a8, 78
        movi.n   a2, 4
        beq      a14, a8, .Lsec_73_6e
        bltu     a8, a14, .Lsec_73_38
        movi.n   a8, 39
        movi.n   a2, 6
        beq      a14, a8, .Lsec_73_6e
        bltu     a8, a14, .Lsec_73_30
        movi.n   a8, 13
        movi.n   a2, 8
        beq      a14, a8, .Lsec_73_6e
        movi.n   a8, 26
        movi.n   a2, 7
        j        .Lsec_73_59
.Lsec_73_30:
        movi.n   a8, 52
        movi.n   a2, 5
        j        .Lsec_73_59
        .byte    0x00
.Lsec_73_38:
        movi     a8, 130
        movi.n   a2, 1
        beq      a14, a8, .Lsec_73_6e
        bltu     a8, a14, .Lsec_73_54
        movi     a8, 104
        movi.n   a2, 3
        beq      a14, a8, .Lsec_73_6e
        movi     a8, 117
        movi.n   a2, 2
        j        .Lsec_73_59
        .byte    0x00
.Lsec_73_54:
        movi     a8, 144
        movi.n   a2, 0
.Lsec_73_59:
        beq      a14, a8, .Lsec_73_6e
        l32r     a13, .Lsec_73_0
        movi.n   a12, 1
        movi     a11, 0x200
        movi.n   a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_73_4
        callx8   a8
        movi.n   a2, 0
.Lsec_73_6e:
        retw.n
        .size   rcGet11NHighestRateIdx, .-rcGet11NHighestRateIdx
.Lsec_73_70:

        .section .text.rcGet11GHighestRateIdx,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_74_0, .Lsec_162_0
        .literal .Lsec_74_4, wifi_log
.Lsec_74_8:
        .global rcGet11GHighestRateIdx
        .type   rcGet11GHighestRateIdx, @function
rcGet11GHighestRateIdx:
        entry    a1, 32
        extui    a14, a2, 0, 8
        movi.n   a8, 48
        movi.n   a2, 3
        beq      a14, a8, .Lsec_74_5e
        bltu     a8, a14, .Lsec_74_35
        movi.n   a8, 24
        movi.n   a2, 5
        beq      a14, a8, .Lsec_74_5e
        bltu     a8, a14, .Lsec_74_2e
        movi.n   a2, 7
        beqi     a14, 12, .Lsec_74_5e
        movi.n   a8, 18
        movi.n   a2, 6
        j        .Lsec_74_49
.Lsec_74_2e:
        movi.n   a8, 36
        movi.n   a2, 4
        j        .Lsec_74_49
.Lsec_74_35:
        movi     a8, 96
        movi.n   a2, 1
        beq      a14, a8, .Lsec_74_5e
        movi     a8, 108
        movi.n   a2, 0
        beq      a14, a8, .Lsec_74_5e
        movi.n   a8, 72
        movi.n   a2, 2
.Lsec_74_49:
        beq      a14, a8, .Lsec_74_5e
        l32r     a13, .Lsec_74_0
        movi.n   a12, 1
        movi     a11, 0x200
        movi.n   a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_74_4
        callx8   a8
        movi.n   a2, 0
.Lsec_74_5e:
        retw.n
        .size   rcGet11GHighestRateIdx, .-rcGet11GHighestRateIdx
.Lsec_74_60:

        .section .text.rcGet11BHighestRateIdx,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_75_0, .Lsec_161_0
        .literal .Lsec_75_4, wifi_log
.Lsec_75_8:
        .global rcGet11BHighestRateIdx
        .type   rcGet11BHighestRateIdx, @function
rcGet11BHighestRateIdx:
        entry    a1, 32
        extui    a14, a2, 0, 8
        movi.n   a8, 11
        movi.n   a2, 1
        beq      a14, a8, .Lsec_75_3e
        bltu     a8, a14, .Lsec_75_25
        movi.n   a2, 3
        beqi     a14, 2, .Lsec_75_3e
        movi.n   a2, 2
        beqi     a14, 4, .Lsec_75_3e
        j        .Lsec_75_2c
.Lsec_75_25:
        movi.n   a8, 22
        movi.n   a2, 0
        beq      a14, a8, .Lsec_75_3e
.Lsec_75_2c:
        l32r     a13, .Lsec_75_0
        movi.n   a12, 1
        movi     a11, 0x200
        movi.n   a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_75_4
        callx8   a8
        movi.n   a2, 0
.Lsec_75_3e:
        retw.n
        .size   rcGet11BHighestRateIdx, .-rcGet11BHighestRateIdx
.Lsec_75_40:

        .section .text.rcGetDefaultHigestRateIdx,"ax",@progbits
        .align  4
.Lsec_76_0:
        .global rcGetDefaultHigestRateIdx
        .type   rcGetDefaultHigestRateIdx, @function
rcGetDefaultHigestRateIdx:
        entry    a1, 32
        l8ui     a8, a2, 130
        movi     a2, 253
        addi     a8, a8, -2
        and      a8, a8, a2
        movi.n   a9, 0
        movi.n   a2, 1
        movnez   a2, a9, a8
        retw.n
        .size   rcGetDefaultHigestRateIdx, .-rcGetDefaultHigestRateIdx
.Lsec_76_18:

        .section .text.rcGetHighestRateIdx,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_77_0, rcGet11NHighestRateIdx
        .literal .Lsec_77_4, rcGet11GHighestRateIdx
        .literal .Lsec_77_8, rcGet11BHighestRateIdx
        .literal .Lsec_77_c, rcGetDefaultHigestRateIdx
.Lsec_77_10:
        .global rcGetHighestRateIdx
        .type   rcGetHighestRateIdx, @function
rcGetHighestRateIdx:
        entry    a1, 32
        extui    a4, a4, 0, 8
        extui    a10, a3, 0, 8
        bnez.n   a4, .Lsec_77_20
        j        .Lsec_77_4d
        .byte    0x00
        .byte    0x00
.Lsec_77_20:
        l8ui     a8, a2, 130
        beqi     a8, 1, .Lsec_77_3b
        bltui    a8, 2, .Lsec_77_44
        addi     a8, a8, -2
        extui    a8, a8, 0, 8
        bgeui    a8, 4, .Lsec_77_4d
        .reloc  ., R_XTENSA_ASM_EXPAND, rcGet11NHighestRateIdx
        l32r     a8, .Lsec_77_0
        callx8   a8
        j        .Lsec_77_55
.Lsec_77_3b:
        .reloc  ., R_XTENSA_ASM_EXPAND, rcGet11GHighestRateIdx
        l32r     a8, .Lsec_77_4
        callx8   a8
        j        .Lsec_77_55
.Lsec_77_44:
        .reloc  ., R_XTENSA_ASM_EXPAND, rcGet11BHighestRateIdx
        l32r     a8, .Lsec_77_8
        callx8   a8
        j        .Lsec_77_55
.Lsec_77_4d:
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, rcGetDefaultHigestRateIdx
        l32r     a8, .Lsec_77_c
        callx8   a8
.Lsec_77_55:
        mov.n    a2, a10
        retw.n
        .size   rcGetHighestRateIdx, .-rcGetHighestRateIdx
.Lsec_77_59:

        .section .text.rcClearCurStat,"ax",@progbits
        .align  4
.Lsec_78_0:
        .global rcClearCurStat
        .type   rcClearCurStat, @function
rcClearCurStat:
        entry    a1, 32
        movi.n   a8, 0
        s32i.n   a8, a2, 44
        s32i.n   a8, a2, 40
        s32i.n   a8, a2, 60
        memw
        retw.n
        .size   rcClearCurStat, .-rcClearCurStat
.Lsec_78_10:

        .section .text.rcClearCurSched,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_79_0, 0x0007a120
.Lsec_79_4:
        .global rcClearCurSched
        .type   rcClearCurSched, @function
rcClearCurSched:
        entry    a1, 32
        l32i     a8, a2, 96
        movi.n   a9, 0
        s8i      a9, a8, 11
        memw
        l32r     a8, .Lsec_79_0
        s32i     a8, a2, 92
        movi.n   a8, 0
        s32i.n   a8, a2, 44
        s32i.n   a8, a2, 40
        s32i.n   a8, a2, 60
        s8i      a8, a2, 27
        s8i      a9, a2, 28
        s8i      a9, a2, 7
        memw
        memw
        retw.n
        .size   rcClearCurSched, .-rcClearCurSched
.Lsec_79_31:

        .section .text.rcClearCurAMPDUSched,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_80_0, 0x0007a120
.Lsec_80_4:
        .global rcClearCurAMPDUSched
        .type   rcClearCurAMPDUSched, @function
rcClearCurAMPDUSched:
        entry    a1, 32
        l32i     a8, a2, 96
        movi.n   a9, 0
        s8i      a9, a8, 11
        memw
        l32r     a8, .Lsec_80_0
        s32i     a8, a2, 92
        movi.n   a8, 0
        s32i     a8, a2, 68
        s32i     a8, a2, 64
        s32i     a8, a2, 72
        s8i      a8, a2, 29
        s8i      a9, a2, 30
        s8i      a9, a2, 15
        memw
        s32i     a8, a2, 88
        memw
        retw.n
        .size   rcClearCurAMPDUSched, .-rcClearCurAMPDUSched
.Lsec_80_37:

        .section .text.rcUpdatePhyMode,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_81_0, rcP2P11NSchedTbl
        .literal .Lsec_81_4, rc11NSchedTbl
        .literal .Lsec_81_8, rcP2P11GSchedTbl+0x54
        .literal .Lsec_81_c, BasicOFDMSched
        .literal .Lsec_81_10, rcP2P11GSchedTbl
        .literal .Lsec_81_14, rc11GSchedTbl
        .literal .Lsec_81_18, rc11BSchedTbl+0x24
        .literal .Lsec_81_1c, rcLoRaSchedTbl
        .literal .Lsec_81_20, rcLoRaSchedTbl+0xc
        .literal .Lsec_81_24, g_mesh_is_started
        .literal .Lsec_81_28, .Lsec_160_0
        .literal .Lsec_81_2c, trc_ctl
        .literal .Lsec_81_30, .Lsec_62_4
        .literal .Lsec_81_34, .Lsec_64_4
        .literal .Lsec_81_38, rc11BSchedTbl
        .literal .Lsec_81_3c, .Lsec_65_4
        .literal .Lsec_81_40, rcLoRaRate2SchedIdx
        .literal .Lsec_81_44, .Lsec_159_0
        .literal .Lsec_81_48, 0x7f7f7f7f
        .literal .Lsec_81_4c, wifi_log
        .literal .Lsec_81_50, rcGetHighestRateIdx
        .literal .Lsec_81_54, rx11NRate2AMPDULimit
        .literal .Lsec_81_58, rcSetTxAmpduLimit
        .literal .Lsec_81_5c, trcAmpduSetState
        .literal .Lsec_81_60, rcClearCurAMPDUSched
        .literal .Lsec_81_64, wifi_log
        .literal .Lsec_81_68, rcClearCurSched
.Lsec_81_6c:
        .global rcUpdatePhyMode
        .type   rcUpdatePhyMode, @function
rcUpdatePhyMode:
        entry    a1, 48
        l32r     a8, .Lsec_81_24
        extui    a3, a3, 0, 8
        extui    a4, a4, 0, 8
        extui    a5, a5, 0, 8
        extui    a6, a6, 0, 8
        l8ui     a9, a8, 0
        l8ui     a7, a2, 135
        beqz.n   a9, .Lsec_81_a5
        beqz.n   a4, .Lsec_81_a5
        sext     a9, a3, 7
        mov.n    a15, a7
        l8ui     a14, a2, 130
        movi.n   a12, 2
        movi     a11, 0x200
        movi.n   a10, 6
        l32r     a13, .Lsec_81_28
        s32i.n   a9, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_81_4c
        callx8   a8
.Lsec_81_a5:
        mov.n    a11, a5
        mov.n    a12, a6
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, rcGetHighestRateIdx
        l32r     a8, .Lsec_81_50
        callx8   a8
        l8ui     a5, a2, 130
        s8i      a3, a2, 26
        memw
        bgeui    a5, 6, .Lsec_81_c9
        bgeui    a5, 2, .Lsec_81_d4
        bnez     a5, .Lsec_81_1ac
        memw
        j        .Lsec_81_230
.Lsec_81_c9:
        bnei     a5, 6, .Lsec_81_cf
        j        .Lsec_81_270
.Lsec_81_cf:
        j        .Lsec_81_290
        .byte    0x00
        .byte    0x00
.Lsec_81_d4:
        l32r     a5, .Lsec_81_2c
        movi     a9, 0x505
        s16i     a9, a5, 24
        memw
        l32r     a9, .Lsec_81_0
        bnez.n   a4, .Lsec_81_e8
        l32r     a9, .Lsec_81_4
.Lsec_81_e8:
        movi.n   a5, 9
        s32i     a9, a2, 112
        bnez     a4, .Lsec_81_f8
        movi.n   a5, 13
        movi     a11, 11
        moveqz   a5, a11, a7
.Lsec_81_f8:
        s8i      a5, a2, 5
        memw
        movi.n   a11, 14
        sext     a3, a3, 7
        l32r     a5, .Lsec_81_30
        s32i     a5, a2, 116
        movi.n   a5, 10
        moveqz   a5, a11, a4
        s8i      a5, a2, 6
        movi.n   a5, 11
        s8i      a10, a2, 4
        memw
        bge      a5, a3, .Lsec_81_141
        movi.n   a11, 16
        movi.n   a5, 7
        bge      a11, a3, .Lsec_81_141
        movi.n   a11, 21
        movi.n   a5, 5
        bge      a11, a3, .Lsec_81_141
        movi.n   a11, 26
        movi.n   a5, 4
        bge      a11, a3, .Lsec_81_141
        movi.n   a11, 29
        movi.n   a5, 3
        bge      a11, a3, .Lsec_81_141
        movi.n   a11, 33
        mov.n    a5, a10
        blt      a11, a3, .Lsec_81_141
        movi.n   a5, 2
.Lsec_81_141:
        maxu     a5, a5, a10
        extui    a5, a5, 0, 8
        bltui    a5, 8, .Lsec_81_150
        movi     a3, 7
        movnez   a5, a3, a4
.Lsec_81_150:
        movnez   a5, a10, a6
        extui    a3, a5, 0, 16
        addx2    a3, a3, a3
        addx4    a3, a3, a9
        s32i     a3, a2, 96
        movi.n   a3, 5
        minu     a3, a5, a3
        extui    a3, a3, 0, 8
        addx2    a3, a3, a3
        addx4    a3, a3, a9
        l8ui     a10, a3, 0
        movi     a3, 4
        s8i      a10, a2, 13
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, rx11NRate2AMPDULimit
        l32r     a8, .Lsec_81_54
        callx8   a8
        or       a11, a10, a10
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, rcSetTxAmpduLimit
        l32r     a8, .Lsec_81_58
        callx8   a8
        mov.n    a10, a2
        movi.n   a11, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, trcAmpduSetState
        l32r     a8, .Lsec_81_5c
        callx8   a8
        mov.n    a10, a2
        s8i      a3, a2, 14
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, rcClearCurAMPDUSched
        l32r     a8, .Lsec_81_60
        callx8   a8
        j        .Lsec_81_221
.Lsec_81_1ac:
        l32r     a9, .Lsec_81_10
        bnez.n   a4, .Lsec_81_1b4
        l32r     a9, .Lsec_81_14
.Lsec_81_1b4:
        movi.n   a5, 7
        s32i     a9, a2, 112
        s8i      a10, a2, 4
        memw
        bnez.n   a4, .Lsec_81_1c8
        movi.n   a5, 12
        movi.n   a11, 10
        moveqz   a5, a11, a7
.Lsec_81_1c8:
        s8i      a5, a2, 5
        memw
        movi.n   a11, 13
        sext     a3, a3, 7
        l32r     a5, .Lsec_81_34
        s32i     a5, a2, 116
        movi.n   a5, 8
        moveqz   a5, a11, a4
        s8i      a5, a2, 6
        memw
        movi.n   a5, 11
        blt      a5, a3, .Lsec_81_1f6
        movi.n   a5, 7
        movi.n   a3, 10
        moveqz   a5, a3, a4
        memw
        j        .Lsec_81_212
.Lsec_81_1f6:
        movi.n   a11, 16
        movi.n   a5, 5
        bge      a11, a3, .Lsec_81_212
        movi.n   a11, 21
        movi.n   a5, 3
        bge      a11, a3, .Lsec_81_212
        movi.n   a11, 29
        movi.n   a5, 1
        bge      a11, a3, .Lsec_81_20d
        movi.n   a5, 0
.Lsec_81_20d:
        add.n    a5, a5, a5
        extui    a5, a5, 0, 8
.Lsec_81_212:
        movnez   a5, a10, a6
        extui    a3, a5, 0, 16
        addx2    a3, a3, a3
        addx4    a3, a3, a9
        s32i     a3, a2, 96
.Lsec_81_221:
        l32r     a3, .Lsec_81_8
        bnez     a4, .Lsec_81_2c0
        l32r     a3, .Lsec_81_c
        memw
        j        .Lsec_81_2c0
.Lsec_81_230:
        l32r     a11, .Lsec_81_38
        l32r     a3, .Lsec_81_18
        or       a9, a3, a3
        s32i     a11, a2, 112
        beqz     a6, .Lsec_81_248
        extui    a9, a10, 0, 16
        addx2    a9, a9, a9
        addx4    a9, a9, a11
.Lsec_81_248:
        s32i     a9, a2, 96
        movi.n   a8, 5
        movi.n   a9, 3
        moveqz   a8, a9, a7
        s8i      a8, a2, 5
        memw
        l32r     a8, .Lsec_81_3c
        s32i     a8, a2, 116
        movi.n   a8, 6
        s8i      a10, a2, 4
        s8i      a8, a2, 6
        memw
        memw
        j        .Lsec_81_2c0
        .byte    0x00
.Lsec_81_270:
        l32r     a5, .Lsec_81_40
        l32r     a3, .Lsec_81_1c
        s32i     a5, a2, 116
        movi     a5, 0x100
        s16i     a5, a2, 4
        memw
        movi.n   a5, 2
        s32i     a3, a2, 112
        s32i     a3, a2, 96
        memw
        j        .Lsec_81_2b7
.Lsec_81_290:
        movi.n   a5, 0
        l32r     a3, .Lsec_81_38
        movi.n   a8, 3
        s8i      a5, a2, 4
        movi     a5, 5
        moveqz   a5, a8, a7
        s32i     a3, a2, 112
        s8i      a5, a2, 5
        memw
        l32r     a3, .Lsec_81_18
        l32r     a5, .Lsec_81_3c
        s32i     a3, a2, 96
        s32i     a5, a2, 116
        movi.n   a5, 6
.Lsec_81_2b7:
        s8i      a5, a2, 6
        memw
        movi     a5, 0
.Lsec_81_2c0:
        l32r     a6, .Lsec_81_24
        s32i     a3, a2, 100
        l8ui     a3, a6, 0
        beqz.n   a3, .Lsec_81_2e5
        beqz.n   a4, .Lsec_81_2e5
        mov.n    a15, a5
        l8ui     a14, a2, 5
        movi.n   a12, 2
        movi     a11, 0x200
        movi.n   a10, 6
        l32r     a13, .Lsec_81_44
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_81_64
        callx8   a8
.Lsec_81_2e5:
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, rcClearCurSched
        l32r     a8, .Lsec_81_68
        callx8   a8
        l32r     a3, .Lsec_81_48
        s32i     a3, a2, 0
        l32r     a3, .Lsec_81_20
        bnez     a7, .Lsec_81_2fc
        l32i     a3, a2, 100
.Lsec_81_2fc:
        s32i     a3, a2, 104
        memw
        retw.n
        .size   rcUpdatePhyMode, .-rcUpdatePhyMode
.Lsec_81_304:

        .section .text.rcLowerSched,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_82_0, g_mesh_is_started
        .literal .Lsec_82_4, g_mesh_is_root
        .literal .Lsec_82_8, g_mesh_init_ps_type
        .literal .Lsec_82_c, g_osi_funcs_p
        .literal .Lsec_82_10, 0x3ff73c00
        .literal .Lsec_82_14, g_wifi_mac_time_delta
        .literal .Lsec_82_18, rcClearCurSched
.Lsec_82_1c:
        .global rcLowerSched
        .type   rcLowerSched, @function
rcLowerSched:
        entry    a1, 32
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, rcClearCurSched
        l32r     a8, .Lsec_82_18
        callx8   a8
        l32i     a10, a2, 96
        extui    a3, a3, 0, 8
        l8ui     a9, a10, 10
        add.n    a9, a9, a3
        l8ui     a8, a2, 5
        blt      a8, a9, .Lsec_82_45
        addx2    a3, a3, a3
        addx4    a8, a3, a10
        j        .Lsec_82_4e
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_82_45:
        l32i     a3, a2, 112
        addx2    a8, a8, a8
        addx4    a8, a8, a3
.Lsec_82_4e:
        l32r     a3, .Lsec_82_0
        l8ui     a3, a3, 0
        s32i     a8, a2, 96
        beqz.n   a3, .Lsec_82_80
        l32r     a3, .Lsec_82_4
        memw
        l8ui     a3, a3, 0
        extui    a3, a3, 0, 8
        bnez.n   a3, .Lsec_82_80
        l32r     a3, .Lsec_82_8
        l32i.n   a3, a3, 0
        beqz.n   a3, .Lsec_82_80
        l32r     a3, .Lsec_82_c
        l32i.n   a3, a3, 0
        l32i     a10, a3, 0x110
        memw
        callx8   a10
        j        .Lsec_82_8f
        .byte    0x00
.Lsec_82_80:
        l32r     a3, .Lsec_82_10
        memw
        l32i.n   a10, a3, 0
        l32r     a3, .Lsec_82_14
        l32i.n   a3, a3, 0
        add.n    a10, a10, a3
.Lsec_82_8f:
        s32i.n   a10, a2, 48
        memw
        retw.n
        .size   rcLowerSched, .-rcLowerSched
.Lsec_82_96:

        .section .text.rcUpSched,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_83_0, g_mesh_is_started
        .literal .Lsec_83_4, g_mesh_is_root
        .literal .Lsec_83_8, g_mesh_init_ps_type
        .literal .Lsec_83_c, g_osi_funcs_p
        .literal .Lsec_83_10, 0x3ff73c00
        .literal .Lsec_83_14, g_wifi_mac_time_delta
        .literal .Lsec_83_18, rcClearCurSched
.Lsec_83_1c:
        .global rcUpSched
        .type   rcUpSched, @function
rcUpSched:
        entry    a1, 32
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, rcClearCurSched
        l32r     a8, .Lsec_83_18
        callx8   a8
        l32i     a8, a2, 96
        l8ui     a10, a2, 4
        l8ui     a9, a8, 10
        bgeu     a10, a9, .Lsec_83_89
        addi     a8, a8, -12
        movi.n   a10, 1
        s32i     a8, a2, 96
        l8ui     a9, a8, 11
        or       a9, a9, a10
        s8i      a9, a8, 11
        memw
        l32r     a8, .Lsec_83_0
        l8ui     a8, a8, 0
        beqz.n   a8, .Lsec_83_78
        l32r     a8, .Lsec_83_4
        memw
        l8ui     a8, a8, 0
        extui    a8, a8, 0, 8
        bnez.n   a8, .Lsec_83_78
        l32r     a8, .Lsec_83_8
        l32i.n   a8, a8, 0
        beqz.n   a8, .Lsec_83_78
        l32r     a8, .Lsec_83_c
        l32i     a8, a8, 0
        l32i     a10, a8, 0x110
        memw
        callx8   a10
        j        .Lsec_83_87
        .byte    0x00
.Lsec_83_78:
        l32r     a8, .Lsec_83_10
        memw
        l32i.n   a10, a8, 0
        l32r     a8, .Lsec_83_14
        l32i.n   a8, a8, 0
        add.n    a10, a10, a8
.Lsec_83_87:
        s32i.n   a10, a2, 52
.Lsec_83_89:
        memw
        retw.n
        .size   rcUpSched, .-rcUpSched
.Lsec_83_8e:

        .section .text.rcUpdateAckSnr,"ax",@progbits
        .align  4
.Lsec_84_0:
        .global rcUpdateAckSnr
        .type   rcUpdateAckSnr, @function
rcUpdateAckSnr:
        entry    a1, 32
        extui    a11, a3, 0, 8
        movi     a10, 127
        sext     a3, a3, 7
        beq      a3, a10, .Lsec_84_56
        l8ui     a8, a2, 0
        movi.n   a9, 0
        sext     a8, a8, 7
        beq      a8, a10, .Lsec_84_20
        add      a8, a8, a3
        extui    a9, a8, 1, 8
.Lsec_84_20:
        l8ui     a8, a2, 1
        movi     a3, 127
        sext     a8, a8, 7
        s8i      a11, a2, 0
        memw
        beq      a8, a3, .Lsec_84_50
        sext     a9, a9, 7
        addx2    a8, a8, a8
        add.n    a8, a8, a9
        addi.n   a9, a8, 3
        movgez   a9, a8, a8
        srai     a8, a9, 2
        s8i      a8, a2, 1
        memw
        memw
        j        .Lsec_84_56
        .byte    0x00
        .byte    0x00
.Lsec_84_50:
        s8i      a9, a2, 1
        memw
.Lsec_84_56:
        memw
        retw.n
        .size   rcUpdateAckSnr, .-rcUpdateAckSnr
.Lsec_84_5b:

        .section .text.rcTxUpdatePer,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_85_0, 0x01ffffff
        .literal .Lsec_85_4, rcLowerSched
.Lsec_85_8:
        .global rcTxUpdatePer
        .type   rcTxUpdatePer, @function
rcTxUpdatePer:
        entry    a1, 32
        l32i     a8, a2, 96
        extui    a3, a3, 0, 8
        mov.n    a10, a2
        l8ui     a9, a8, 1
        addi     a8, a9, 2
        extui    a8, a8, 0, 8
        bltu     a9, a3, .Lsec_85_24
        addi.n   a8, a3, 1
        extui    a8, a8, 0, 8
.Lsec_85_24:
        l32i.n   a9, a10, 44
        addi.n   a11, a9, 1
        l32r     a12, .Lsec_85_0
        bgeu     a12, a11, .Lsec_85_3a
        srli     a9, a9, 1
        l32i.n   a11, a10, 40
        srli     a11, a11, 1
        s32i.n   a11, a10, 40
        s32i.n   a9, a10, 44
.Lsec_85_3a:
        l32i.n   a9, a10, 44
        addi.n   a9, a9, 1
        s32i.n   a9, a10, 44
        l32i.n   a9, a10, 40
        add.n    a8, a9, a8
        s32i.n   a8, a10, 40
        bltui    a3, 8, .Lsec_85_54
        l8ui     a3, a10, 7
        addi.n   a3, a3, 2
        memw
        j        .Lsec_85_5c
.Lsec_85_54:
        bltui    a3, 5, .Lsec_85_68
        l8ui     a3, a10, 7
        addi.n   a3, a3, 1
.Lsec_85_5c:
        s8i      a3, a10, 7
        memw
        memw
        j        .Lsec_85_79
.Lsec_85_68:
        bgeui    a3, 3, .Lsec_85_79
        movi.n   a3, 0
        s8i      a3, a10, 7
        memw
        memw
        j        .Lsec_85_88
.Lsec_85_79:
        l8ui     a3, a10, 7
        bltui    a3, 7, .Lsec_85_88
        movi     a11, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, rcLowerSched
        l32r     a8, .Lsec_85_4
        callx8   a8
.Lsec_85_88:
        retw.n
        .size   rcTxUpdatePer, .-rcTxUpdatePer
.Lsec_85_8a:

        .section .wifi0iram.7,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_86_0, g_mesh_is_started
        .literal .Lsec_86_4, rcUpdateAckSnr
        .literal .Lsec_86_8, rcTxUpdatePer
.Lsec_86_c:
        .global rcUpdateTxDone
        .type   rcUpdateTxDone, @function
rcUpdateTxDone:
        entry    a1, 32
        beqz     a2, .Lsec_86_75
        l8ui     a8, a2, 11
        sext     a9, a8, 7
        bltz     a9, .Lsec_86_75
        l32i     a10, a2, 96
        l32i.n   a9, a3, 28
        bne      a9, a10, .Lsec_86_75
        movi.n   a10, 4
        bany     a8, a10, .Lsec_86_75
        l32i.n   a8, a2, 60
        addi.n   a8, a8, 1
        s32i.n   a8, a2, 60
        l8ui     a8, a3, 19
        bnei     a8, 1, .Lsec_86_54
        l8ui     a8, a2, 25
        bany     a8, a10, .Lsec_86_4c
        or       a10, a2, a2
        l8ui     a11, a3, 13
        sext     a11, a11, 7
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, rcUpdateAckSnr
        l32r     a8, .Lsec_86_4
        callx8   a8
.Lsec_86_4c:
        l8ui     a11, a3, 5
        j        .Lsec_86_6d
        .byte    0x00
        .byte    0x00
.Lsec_86_54:
        addi     a8, a8, -2
        extui    a8, a8, 0, 8
        bgeui    a8, 2, .Lsec_86_75
        l32r     a3, .Lsec_86_0
        l8ui     a3, a3, 0
        l8ui     a11, a9, 8
        beqz.n   a3, .Lsec_86_6d
        movi.n   a3, 11
        minu     a11, a11, a3
.Lsec_86_6d:
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, rcTxUpdatePer
        l32r     a8, .Lsec_86_8
        callx8   a8
.Lsec_86_75:
        retw.n
        .size   rcUpdateTxDone, .-rcUpdateTxDone
.Lsec_86_77:

        .section .wifirxiram.8,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_87_0, wDevCtrl
.Lsec_87_4:
        .global rcUpdateRxDone
        .type   rcUpdateRxDone, @function
rcUpdateRxDone:
        entry    a1, 32
        l8ui     a8, a2, 11
        bbsi     a8, 7, .Lsec_87_72
        l8ui     a9, a2, 25
        extui    a9, a9, 0, 1
        bnez     a9, .Lsec_87_72
        movi     a12, 127
        l32r     a10, .Lsec_87_0
        l8ui     a8, a3, 0
        l8ui     a10, a10, 46
        add.n    a8, a8, a10
        extui    a8, a8, 0, 8
        mov.n    a11, a8
        l8ui     a10, a2, 2
        sext     a10, a10, 7
        beq      a10, a12, .Lsec_87_3c
        sext     a9, a8, 7
        add.n    a10, a10, a9
        extui    a9, a10, 1, 8
        mov.n    a11, a9
.Lsec_87_3c:
        s8i      a8, a2, 2
        memw
        movi     a10, 127
        l8ui     a8, a2, 3
        sext     a8, a8, 7
        beq      a8, a10, .Lsec_87_6c
        sext     a9, a9, 7
        addx2    a8, a8, a8
        add.n    a8, a8, a9
        addi.n   a9, a8, 3
        movgez   a9, a8, a8
        srai     a8, a9, 2
        s8i      a8, a2, 3
        memw
        memw
        j        .Lsec_87_72
        .byte    0x00
        .byte    0x00
.Lsec_87_6c:
        s8i      a11, a2, 3
        memw
.Lsec_87_72:
        memw
        retw.n
        .size   rcUpdateRxDone, .-rcUpdateRxDone
.Lsec_87_77:

        .section .text.trc_onAmpduOp,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_88_0, trcAmpduSetState
.Lsec_88_4:
        .global trc_onAmpduOp
        .type   trc_onAmpduOp, @function
trc_onAmpduOp:
        entry    a1, 32
        l8ui     a8, a2, 130
        extui    a4, a4, 0, 8
        bgeui    a8, 2, .Lsec_88_13
        j        .Lsec_88_e5
.Lsec_88_13:
        beqi     a3, 2, .Lsec_88_19
        j        .Lsec_88_aa
.Lsec_88_19:
        movi     a9, -128
        l8ui     a8, a2, 25
        movi     a10, 253
        or       a8, a8, a9
        s8i      a8, a2, 25
        memw
        movi     a8, 1
        ssl      a4
        sll      a8, a8
        l8ui     a9, a2, 16
        or       a9, a8, a9
        s8i      a9, a2, 16
        memw
        l8ui     a9, a2, 14
        addi     a9, a9, -2
        bany     a9, a10, .Lsec_88_60
        add.n    a9, a2, a4
        s8i      a3, a9, 17
        memw
        l8ui     a9, a2, 38
        or       a8, a8, a9
        s8i      a8, a2, 38
        s8i      a3, a2, 14
        memw
.Lsec_88_60:
        movi.n   a8, 16
        l8ui     a3, a2, 26
        bltu     a8, a3, .Lsec_88_9c
        movi     a8, 109
        l8ui     a3, a2, 1
        addi     a3, a3, -17
        extui    a3, a3, 0, 8
        bgeu     a8, a3, .Lsec_88_9c
        movi.n   a3, 3
        movi     a11, 3
        or       a10, a2, a2
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, trcAmpduSetState
        l32r     a8, .Lsec_88_0
        callx8   a8
        s8i      a3, a2, 14
        movi.n   a3, 33
        s8i      a3, a2, 26
        memw
        movi.n   a3, 3
        memw
        j        .Lsec_88_aa
        .byte    0x00
.Lsec_88_9c:
        movi.n   a3, 33
        s8i      a3, a2, 26
        memw
        memw
        j        .Lsec_88_e5
.Lsec_88_aa:
        movi.n   a8, -3
        and      a8, a3, a8
        bnei     a8, 1, .Lsec_88_e5
        bnei     a3, 1, .Lsec_88_c9
        movi.n   a8, -1
        ssl      a4
        sll      a3, a3
        xor      a3, a8, a3
        l8ui     a8, a2, 16
        and      a3, a3, a8
        s8i      a3, a2, 16
.Lsec_88_c9:
        add.n    a4, a2, a4
        movi.n   a8, 1
        s8i      a8, a4, 17
        memw
        l8ui     a3, a2, 14
        addi     a3, a3, -3
        extui    a3, a3, 0, 8
        bgeui    a3, 2, .Lsec_88_e5
        s8i      a8, a2, 14
        memw
.Lsec_88_e5:
        memw
        retw.n
        .size   trc_onAmpduOp, .-trc_onAmpduOp
.Lsec_88_ea:

        .section .text.trc_set_per_pkt_rate,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_89_0, rc11BSchedTbl+0x24
.Lsec_89_4:
        .global trc_set_per_pkt_rate
        .type   trc_set_per_pkt_rate, @function
trc_set_per_pkt_rate:
        entry    a1, 32
        l32i.n   a8, a2, 44
        movi.n   a10, 1
        slli     a10, a10, 25
        l32i.n   a9, a8, 0
        or       a9, a9, a10
        s32i.n   a9, a8, 0
        l32r     a9, .Lsec_89_0
        s32i.n   a9, a8, 28
        memw
        retw.n
        .size   trc_set_per_pkt_rate, .-trc_set_per_pkt_rate
.Lsec_89_1f:

        .section .text.rcSetBarRate,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_90_0, BAROFDMSched
        .literal .Lsec_90_4, rc11BSchedTbl+0x24
.Lsec_90_8:
        .global rcSetBarRate
        .type   rcSetBarRate, @function
rcSetBarRate:
        entry    a1, 32
        extui    a3, a3, 0, 8
        l32r     a8, .Lsec_90_0
        bnez.n   a3, .Lsec_90_16
        l32r     a8, .Lsec_90_4
.Lsec_90_16:
        s32i.n   a8, a2, 28
        l8ui     a8, a8, 0
        s8i      a8, a2, 12
        memw
        memw
        retw.n
        .size   rcSetBarRate, .-rcSetBarRate
.Lsec_90_26:

        .section .text.rc_get_default_sched,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_91_0, rc11BSchedTbl+0x24
.Lsec_91_4:
        .global rc_get_default_sched
        .type   rc_get_default_sched, @function
rc_get_default_sched:
        entry    a1, 32
        l32r     a2, .Lsec_91_0
        retw.n
        .size   rc_get_default_sched, .-rc_get_default_sched
.Lsec_91_c:

        .section .text.rc_get_G6M_sched,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_92_0, rc11GSchedTbl+0x54
.Lsec_92_4:
        .global rc_get_G6M_sched
        .type   rc_get_G6M_sched, @function
rc_get_G6M_sched:
        entry    a1, 32
        l32r     a2, .Lsec_92_0
        retw.n
        .size   rc_get_G6M_sched, .-rc_get_G6M_sched
.Lsec_92_c:

        .section .rodata.rcGetAmpduSched.str1.1,"aMS",@progbits,1
        .align  1
.Lsec_93_0:
        .ascii  "trc.c\000"

        .section .text.rcGetAmpduSched,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_94_0, .Lsec_146_0
        .literal .Lsec_94_4, .Lsec_93_0
        .literal .Lsec_94_8, .Lsec_158_0
        .literal .Lsec_94_c, 0x000008d2
        .literal .Lsec_94_10, wifi_log
.Lsec_94_14:
        .global rcGetAmpduSched
        .type   rcGetAmpduSched, @function
rcGetAmpduSched:
        entry    a1, 48
        bnez.n   a2, .Lsec_94_3c
        l32r     a2, .Lsec_94_c
        movi.n   a12, 1
        slli     a11, a12, 11
        movi.n   a10, 6
        l32r     a15, .Lsec_94_0
        l32r     a14, .Lsec_94_4
        l32r     a13, .Lsec_94_8
        s32i     a2, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_94_10
        callx8   a8
.Lsec_94_38:
        j        .Lsec_94_38
        .byte    0x00
.Lsec_94_3c:
        l32i     a4, a2, 96
        s32i.n   a4, a3, 28
        l8ui     a4, a2, 11
        bbci     a4, 2, .Lsec_94_51
        l8ui     a2, a2, 10
        memw
        j        .Lsec_94_8d
        .byte    0x00
.Lsec_94_51:
        l8ui     a4, a2, 25
        bbsi     a4, 3, .Lsec_94_93
        l32i     a4, a2, 116
        l8ui     a10, a2, 13
        callx8   a4
        l8ui     a4, a2, 12
        add.n    a4, a10, a4
        movi     a10, 16
        extui    a4, a4, 0, 8
        l32i     a8, a2, 116
        callx8   a8
        bgeu     a10, a4, .Lsec_94_7e
        movi.n   a10, 16
        l32i     a4, a2, 116
        callx8   a4
        mov.n    a4, a10
.Lsec_94_7e:
        extui    a4, a4, 0, 16
        l32i     a2, a2, 112
        addx2    a4, a4, a4
        addx4    a4, a4, a2
        l8ui     a2, a4, 0
.Lsec_94_8d:
        s8i      a2, a3, 12
        memw
.Lsec_94_93:
        memw
        retw.n
        .size   rcGetAmpduSched, .-rcGetAmpduSched
.Lsec_94_98:

        .section .wifiextrairam.12,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_95_0, .Lsec_145_0
        .literal .Lsec_95_4, .Lsec_93_0
        .literal .Lsec_95_8, .Lsec_157_0
        .literal .Lsec_95_c, 0x00000965
        .literal .Lsec_95_10, wifi_log
.Lsec_95_14:
        .global rcGetRate
        .type   rcGetRate, @function
rcGetRate:
        entry    a1, 48
        l32i.n   a10, a3, 0
        extui    a8, a10, 22, 1
        bnez     a8, .Lsec_95_9c
        movi.n   a11, 1
        moveqz   a8, a11, a2
        bnez     a8, .Lsec_95_9c
        l8ui     a8, a2, 11
        extui    a8, a8, 0, 2
        l8ui     a13, a3, 5
        l8ui     a9, a3, 6
        beqz.n   a8, .Lsec_95_46
        bbci     a10, 3, .Lsec_95_40
        l8ui     a2, a2, 8
        j        .Lsec_95_86
        .byte    0x00
        .byte    0x00
.Lsec_95_40:
        l8ui     a2, a2, 9
        j        .Lsec_95_86
.Lsec_95_46:
        l32i.n   a12, a3, 28
        bnez.n   a12, .Lsec_95_6c
        mov.n    a12, a11
        movi.n   a10, 6
        l32r     a2, .Lsec_95_c
        slli     a11, a11, 11
        l32r     a15, .Lsec_95_0
        l32r     a14, .Lsec_95_4
        l32r     a13, .Lsec_95_8
        s32i.n   a2, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_95_10
        callx8   a8
.Lsec_95_68:
        j        .Lsec_95_68
        .byte    0x00
.Lsec_95_6c:
        maxu     a11, a9, a13
        addi.n   a10, a12, 1
        mov.n    a9, a8
        movi.n   a2, 4
.Lsec_95_75:
        l8ui     a13, a10, 0
        add.n    a8, a8, a13
        extui    a8, a8, 0, 8
        bgeu     a11, a8, .Lsec_95_92
        addx2    a9, a9, a12
        l8ui     a2, a9, 0
.Lsec_95_86:
        s8i      a2, a3, 12
        memw
        memw
        j        .Lsec_95_9c
.Lsec_95_92:
        addi.n   a9, a9, 1
        addi.n   a10, a10, 2
        addi     a2, a2, -1
        bnez     a2, .Lsec_95_75
.Lsec_95_9c:
        retw.n
        .size   rcGetRate, .-rcGetRate
.Lsec_95_9e:

        .section .text.rssi_margin,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_96_0, .Lsec_97_0
.Lsec_96_4:
        .global rssi_margin
        .type   rssi_margin, @function
rssi_margin:
        entry    a1, 32
        extui    a3, a3, 0, 8
        extui    a8, a2, 0, 8
        bgeui    a3, 8, .Lsec_96_50
        j        .Lsec_96_2c
        .byte    0x00
.Lsec_96_14:
        addi     a3, a3, -9
        extui    a3, a3, 0, 8
        movi.n   a2, 22
        bltu     a2, a3, .Lsec_96_90
        l32r     a2, .Lsec_96_0
        addx4    a3, a3, a2
        l32i.n   a2, a3, 0
        jx       a2
        .byte    0x00
        .byte    0x00
.Lsec_96_2c:
        movi.n   a2, 1
        ssl      a3
        sll      a3, a2
        movi.n   a9, 35
        and      a9, a3, a9
        bnez     a9, .Lsec_96_124
        movi     a10, 136
        bnone    a3, a10, .Lsec_96_45
        j        .Lsec_96_110
.Lsec_96_45:
        movi.n   a2, 68
        and      a2, a3, a2
        bnez     a2, .Lsec_96_dd
        j        .Lsec_96_144
.Lsec_96_50:
        addi     a2, a3, -8
        extui    a2, a2, 0, 8
        movi.n   a9, 23
        bgeu     a9, a2, .Lsec_96_14
        j        .Lsec_96_142
        .byte    0x00
.Lsec_96_5f:
        movi.n   a2, 33
        bltu     a2, a8, .Lsec_96_67
        j        .Lsec_96_142
.Lsec_96_67:
        addi     a2, a8, -127
        beqz     a2, .Lsec_96_142
        addi     a2, a8, -33
        j        .Lsec_96_84
.Lsec_96_73:
        movi.n   a2, 29
        bltu     a2, a8, .Lsec_96_7b
        j        .Lsec_96_142
.Lsec_96_7b:
        addi     a2, a8, -127
        beqz     a2, .Lsec_96_142
        addi     a2, a8, -29
.Lsec_96_84:
        srai     a2, a2, 1
        addx2    a2, a2, a2
        extui    a2, a2, 0, 8
        j        .Lsec_96_13a
.Lsec_96_90:
        movi.n   a2, 26
        bltu     a2, a8, .Lsec_96_98
        j        .Lsec_96_142
.Lsec_96_98:
        addi     a2, a8, -127
        beqz     a2, .Lsec_96_142
        addi     a2, a8, -26
        j        .Lsec_96_84
.Lsec_96_a4:
        movi.n   a2, 21
        bltu     a2, a8, .Lsec_96_ac
        j        .Lsec_96_142
.Lsec_96_ac:
        addi     a2, a8, -127
        beqz     a2, .Lsec_96_142
        addi     a2, a8, -21
        j        .Lsec_96_84
.Lsec_96_b8:
        movi.n   a2, 16
        bltu     a2, a8, .Lsec_96_c0
        j        .Lsec_96_142
.Lsec_96_c0:
        addi     a2, a8, -127
        beqz     a2, .Lsec_96_142
        addi     a2, a8, -16
        j        .Lsec_96_84
.Lsec_96_cc:
        movi.n   a2, 13
        bgeu     a2, a8, .Lsec_96_142
        addi     a2, a8, -127
        beqz     a2, .Lsec_96_142
        addi     a2, a8, -13
        j        .Lsec_96_84
.Lsec_96_dd:
        movi.n   a2, 11
        bgeu     a2, a8, .Lsec_96_142
        addi     a2, a8, -127
        beqz     a2, .Lsec_96_142
        addi     a2, a8, -11
        j        .Lsec_96_84
.Lsec_96_ee:
        movi.n   a2, 8
        bgeu     a2, a8, .Lsec_96_142
        addi     a2, a8, -127
        beqz     a2, .Lsec_96_142
        addi     a2, a8, -8
        j        .Lsec_96_84
        .byte    0x00
.Lsec_96_100:
        movi.n   a2, 9
        bgeu     a2, a8, .Lsec_96_142
        addi     a2, a8, -127
        beqz.n   a2, .Lsec_96_142
        addi     a2, a8, -9
        j        .Lsec_96_84
.Lsec_96_110:
        movi.n   a3, 14
        bgeu     a3, a8, .Lsec_96_142
        addi     a3, a8, -127
        moveqz   a2, a9, a3
        beqz.n   a2, .Lsec_96_142
        addi     a2, a8, -14
        j        .Lsec_96_84
        .byte    0x00
.Lsec_96_124:
        movi.n   a2, 10
        bgeu     a2, a8, .Lsec_96_142
        addi     a2, a8, -127
        beqz.n   a2, .Lsec_96_142
        addi     a8, a8, -10
        srai     a8, a8, 1
        addx2    a8, a8, a8
        extui    a2, a8, 0, 8
.Lsec_96_13a:
        movi.n   a3, 32
        minu     a2, a2, a3
        j        .Lsec_96_144
.Lsec_96_142:
        movi.n   a2, 0
.Lsec_96_144:
        retw.n
        .size   rssi_margin, .-rssi_margin
.Lsec_96_146:

        .section .rodata.rssi_margin,"a",@progbits
        .align  4
.Lsec_97_0:
        .word   .Lsec_96_b8
        .word   .Lsec_96_dd
        .word   .Lsec_96_ee
        .word   .Lsec_96_73
        .word   .Lsec_96_a4
        .word   .Lsec_96_cc
        .word   .Lsec_96_100
        .word   .Lsec_96_ee
        .word   .Lsec_96_dd
        .word   .Lsec_96_cc
        .word   .Lsec_96_b8
        .word   .Lsec_96_a4
        .word   .Lsec_96_90
        .word   .Lsec_96_73
        .word   .Lsec_96_5f
        .word   .Lsec_96_ee
        .word   .Lsec_96_dd
        .word   .Lsec_96_cc
        .word   .Lsec_96_b8
        .word   .Lsec_96_a4
        .word   .Lsec_96_90
        .word   .Lsec_96_73
        .word   .Lsec_96_5f

        .section .text.TRC_PER_IS_GOOD,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_98_0, rssi_margin
.Lsec_98_4:
        .global TRC_PER_IS_GOOD
        .type   TRC_PER_IS_GOOD, @function
TRC_PER_IS_GOOD:
        entry    a1, 32
        l32i     a4, a2, 96
        l8ui     a10, a2, 1
        movi     a2, 90
        l8ui     a11, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, rssi_margin
        l32r     a8, .Lsec_98_0
        callx8   a8
        sub      a9, a2, a10
        l8ui     a8, a4, 9
        beqz.n   a8, .Lsec_98_2d
        movi     a9, 100
        movi     a2, 64
        sub      a9, a9, a10
        bltu     a2, a8, .Lsec_98_3a
.Lsec_98_2d:
        movi.n   a2, 1
        bltu     a9, a3, .Lsec_98_34
        movi.n   a2, 0
.Lsec_98_34:
        extui    a2, a2, 0, 8
        j        .Lsec_98_53
.Lsec_98_3a:
        movi.n   a2, 0
        bgeu     a9, a3, .Lsec_98_50
        addi     a8, a8, 2
        sub      a8, a8, a10
        movi     a9, 1
        bltu     a8, a3, .Lsec_98_4e
        or       a9, a2, a2
.Lsec_98_4e:
        mov.n    a2, a9
.Lsec_98_50:
        extui    a2, a2, 0, 1
.Lsec_98_53:
        retw.n
        .size   TRC_PER_IS_GOOD, .-TRC_PER_IS_GOOD
.Lsec_98_55:

        .section .text.TRC_AMPDU_PER_DOWN_THRESHOLD,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_99_0, .Lsec_100_0
        .literal .Lsec_99_4, rssi_margin
        .literal .Lsec_99_8, rssi_margin
        .literal .Lsec_99_c, rssi_margin
        .literal .Lsec_99_10, rssi_margin
.Lsec_99_14:
        .global TRC_AMPDU_PER_DOWN_THRESHOLD
        .type   TRC_AMPDU_PER_DOWN_THRESHOLD, @function
TRC_AMPDU_PER_DOWN_THRESHOLD:
        entry    a1, 32
        l8ui     a8, a2, 13
        movi.n   a9, 12
        addi     a8, a8, -19
        extui    a8, a8, 0, 8
        bltu     a9, a8, .Lsec_99_84
        l32r     a9, .Lsec_99_0
        addx4    a8, a8, a9
        l32i.n   a8, a8, 0
        jx       a8
.Lsec_99_30:
        movi.n   a11, 31
        j        .Lsec_99_3c
.Lsec_99_35:
        movi.n   a11, 23
        j        .Lsec_99_4d
.Lsec_99_3a:
        movi.n   a11, 22
.Lsec_99_3c:
        l8ui     a10, a2, 1
        movi     a2, 114
        .reloc  ., R_XTENSA_ASM_EXPAND, rssi_margin
        l32r     a8, .Lsec_99_4
        callx8   a8
        j        .Lsec_99_7c
.Lsec_99_4b:
        movi.n   a11, 21
.Lsec_99_4d:
        l8ui     a10, a2, 1
        movi     a2, 115
        .reloc  ., R_XTENSA_ASM_EXPAND, rssi_margin
        l32r     a8, .Lsec_99_8
        callx8   a8
        j        .Lsec_99_7c
        .byte    0x00
.Lsec_99_5d:
        l8ui     a10, a2, 1
        movi.n   a11, 20
        .reloc  ., R_XTENSA_ASM_EXPAND, rssi_margin
        l32r     a8, .Lsec_99_c
        callx8   a8
        movi     a2, 109
        j        .Lsec_99_7c
.Lsec_99_6e:
        l8ui     a10, a2, 1
        movi.n   a11, 19
        .reloc  ., R_XTENSA_ASM_EXPAND, rssi_margin
        l32r     a8, .Lsec_99_10
        callx8   a8
        movi     a2, 108
.Lsec_99_7c:
        sub      a2, a2, a10
        j        .Lsec_99_87
        .byte    0x00
        .byte    0x00
.Lsec_99_84:
        movi     a2, 114
.Lsec_99_87:
        retw.n
        .size   TRC_AMPDU_PER_DOWN_THRESHOLD, .-TRC_AMPDU_PER_DOWN_THRESHOLD
.Lsec_99_89:

        .section .rodata.TRC_AMPDU_PER_DOWN_THRESHOLD,"a",@progbits
        .align  4
.Lsec_100_0:
        .word   .Lsec_99_6e
        .word   .Lsec_99_5d
        .word   .Lsec_99_4b
        .word   .Lsec_99_3a
        .word   .Lsec_99_35
        .word   .Lsec_99_84
        .word   .Lsec_99_84
        .word   .Lsec_99_84
        .word   .Lsec_99_84
        .word   .Lsec_99_84
        .word   .Lsec_99_84
        .word   .Lsec_99_84
        .word   .Lsec_99_30

        .section .text.rcAmpduLowerRate,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_101_0, trc_ctl
        .literal .Lsec_101_4, g_mesh_is_started
        .literal .Lsec_101_8, g_mesh_is_root
        .literal .Lsec_101_c, g_mesh_init_ps_type
        .literal .Lsec_101_10, g_osi_funcs_p
        .literal .Lsec_101_14, 0x3ff73c00
        .literal .Lsec_101_18, g_wifi_mac_time_delta
        .literal .Lsec_101_1c, TRC_AMPDU_PER_DOWN_THRESHOLD
        .literal .Lsec_101_20, trcAmpduSetState
        .literal .Lsec_101_24, rcClearCurSched
        .literal .Lsec_101_28, trc_onAmpduOp
        .literal .Lsec_101_2c, rx11NRate2AMPDULimit
        .literal .Lsec_101_30, rcSetTxAmpduLimit
        .literal .Lsec_101_34, rcClearCurAMPDUSched
.Lsec_101_38:
        .global rcAmpduLowerRate
        .type   rcAmpduLowerRate, @function
rcAmpduLowerRate:
        entry    a1, 32
        l32r     a4, .Lsec_101_0
        extui    a3, a3, 0, 8
        l8ui     a4, a4, 24
        bne      a4, a3, .Lsec_101_aa
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, TRC_AMPDU_PER_DOWN_THRESHOLD
        l32r     a8, .Lsec_101_1c
        callx8   a8
        l8ui     a3, a2, 30
        addi     a3, a3, 20
        bltu     a3, a10, .Lsec_101_5c
        j        .Lsec_101_11c
.Lsec_101_5c:
        movi.n   a11, 3
        mov.n    a10, a2
        movi     a3, 3
        addi     a4, a4, -1
        .reloc  ., R_XTENSA_ASM_EXPAND, trcAmpduSetState
        l32r     a8, .Lsec_101_20
        callx8   a8
        extui    a4, a4, 0, 8
        s8i      a3, a2, 14
        memw
        addx2    a4, a4, a4
        mov.n    a10, a2
        l32i     a3, a2, 112
        addx4    a4, a4, a3
        movi.n   a3, 0
        s32i     a4, a2, 96
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, rcClearCurSched
        l32r     a8, .Lsec_101_24
        callx8   a8
        movi.n   a4, 3
.Lsec_101_90:
        l8ui     a8, a2, 16
        bbc      a8, a3, .Lsec_101_a2
        mov.n    a12, a3
        mov.n    a11, a4
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, trc_onAmpduOp
        l32r     a8, .Lsec_101_28
        callx8   a8
.Lsec_101_a2:
        addi.n   a3, a3, 1
        bnei     a3, 8, .Lsec_101_90
        j        .Lsec_101_124
.Lsec_101_aa:
        movi.n   a4, 0
        s8i      a4, a2, 12
        memw
        l32r     a4, .Lsec_101_4
        l8ui     a4, a4, 0
        beqz.n   a4, .Lsec_101_e0
        l32r     a4, .Lsec_101_8
        memw
        l8ui     a4, a4, 0
        extui    a4, a4, 0, 8
        bnez.n   a4, .Lsec_101_e0
        l32r     a4, .Lsec_101_c
        l32i.n   a4, a4, 0
        beqz.n   a4, .Lsec_101_e0
        l32r     a4, .Lsec_101_10
        l32i.n   a4, a4, 0
        l32i     a10, a4, 0x110
        memw
        callx8   a10
        j        .Lsec_101_ef
.Lsec_101_e0:
        l32r     a4, .Lsec_101_14
        memw
        l32i.n   a10, a4, 0
        l32r     a4, .Lsec_101_18
        l32i.n   a4, a4, 0
        add.n    a10, a10, a4
.Lsec_101_ef:
        addi.n   a3, a3, 1
        extui    a3, a3, 0, 8
        l32i     a4, a2, 112
        addx2    a3, a3, a3
        addx4    a3, a3, a4
        s32i     a10, a2, 80
        l8ui     a10, a3, 0
        s8i      a10, a2, 13
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, rx11NRate2AMPDULimit
        l32r     a8, .Lsec_101_2c
        callx8   a8
        mov.n    a11, a10
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, rcSetTxAmpduLimit
        l32r     a8, .Lsec_101_30
        callx8   a8
.Lsec_101_11c:
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, rcClearCurAMPDUSched
        l32r     a8, .Lsec_101_34
        callx8   a8
.Lsec_101_124:
        retw.n
        .size   rcAmpduLowerRate, .-rcAmpduLowerRate
.Lsec_101_126:

        .section .text.TRC_AMPDU_PER_UP_THRESHOLD,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_102_0, .Lsec_103_0
        .literal .Lsec_102_4, rssi_margin
        .literal .Lsec_102_8, rssi_margin
.Lsec_102_c:
        .global TRC_AMPDU_PER_UP_THRESHOLD
        .type   TRC_AMPDU_PER_UP_THRESHOLD, @function
TRC_AMPDU_PER_UP_THRESHOLD:
        entry    a1, 32
        l8ui     a8, a2, 13
        movi.n   a9, 12
        addi     a8, a8, -19
        extui    a8, a8, 0, 8
        bltu     a9, a8, .Lsec_102_65
        l32r     a9, .Lsec_102_0
        addx4    a8, a8, a9
        l32i.n   a8, a8, 0
        jx       a8
.Lsec_102_28:
        movi.n   a11, 31
        j        .Lsec_102_40
.Lsec_102_2d:
        movi.n   a11, 23
        j        .Lsec_102_40
.Lsec_102_32:
        movi.n   a11, 22
        j        .Lsec_102_40
.Lsec_102_37:
        movi.n   a11, 21
        j        .Lsec_102_40
        .byte    0x00
        .byte    0x00
.Lsec_102_3e:
        movi.n   a11, 20
.Lsec_102_40:
        l8ui     a10, a2, 1
        movi     a2, 121
        .reloc  ., R_XTENSA_ASM_EXPAND, rssi_margin
        l32r     a8, .Lsec_102_4
        callx8   a8
        j        .Lsec_102_5f
        .byte    0x00
        .byte    0x00
.Lsec_102_51:
        l8ui     a10, a2, 1
        movi.n   a11, 19
        .reloc  ., R_XTENSA_ASM_EXPAND, rssi_margin
        l32r     a8, .Lsec_102_8
        callx8   a8
        movi     a2, 116
.Lsec_102_5f:
        sub      a2, a2, a10
        j        .Lsec_102_68
.Lsec_102_65:
        movi     a2, 121
.Lsec_102_68:
        retw.n
        .size   TRC_AMPDU_PER_UP_THRESHOLD, .-TRC_AMPDU_PER_UP_THRESHOLD
.Lsec_102_6a:

        .section .rodata.TRC_AMPDU_PER_UP_THRESHOLD,"a",@progbits
        .align  4
.Lsec_103_0:
        .word   .Lsec_102_51
        .word   .Lsec_102_3e
        .word   .Lsec_102_37
        .word   .Lsec_102_32
        .word   .Lsec_102_2d
        .word   .Lsec_102_65
        .word   .Lsec_102_65
        .word   .Lsec_102_65
        .word   .Lsec_102_65
        .word   .Lsec_102_65
        .word   .Lsec_102_65
        .word   .Lsec_102_65
        .word   .Lsec_102_28

        .section .wifi0iram.9,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_104_0, 0x01ffffff
        .literal .Lsec_104_4, trc_ctl
        .literal .Lsec_104_8, g_mesh_is_started
        .literal .Lsec_104_c, g_mesh_is_root
        .literal .Lsec_104_10, g_mesh_init_ps_type
        .literal .Lsec_104_14, g_osi_funcs_p
        .literal .Lsec_104_18, 0x3ff73c00
        .literal .Lsec_104_1c, g_wifi_mac_time_delta
        .literal .Lsec_104_20, 0x0001869f
        .literal .Lsec_104_24, 0x000186a0
        .literal .Lsec_104_28, 0x003d08ff
        .literal .Lsec_104_2c, 0x0007a120
        .literal .Lsec_104_30, rcUpdateAckSnr
        .literal .Lsec_104_34, TRC_AMPDU_PER_DOWN_THRESHOLD
        .literal .Lsec_104_38, trc_calc_duration
        .literal .Lsec_104_3c, trc_calc_duration
        .literal .Lsec_104_40, rcAmpduLowerRate
        .literal .Lsec_104_44, TRC_AMPDU_PER_UP_THRESHOLD
        .literal .Lsec_104_48, trc_calc_duration
        .literal .Lsec_104_4c, rx11NRate2AMPDULimit
        .literal .Lsec_104_50, rcSetTxAmpduLimit
        .literal .Lsec_104_54, rcClearCurAMPDUSched
.Lsec_104_58:
        .global rcUpdateTxDoneAmpdu2
        .type   rcUpdateTxDoneAmpdu2, @function
rcUpdateTxDoneAmpdu2:
        entry    a1, 48
        l32i.n   a5, a3, 4
        extui    a7, a4, 0, 16
        extui    a5, a5, 0, 4
        l8ui     a9, a3, 7
        l8ui     a6, a3, 6
        beqz     a9, .Lsec_104_37a
        l32i     a4, a2, 96
        beqz     a4, .Lsec_104_37a
        l8ui     a4, a2, 14
        beqi     a4, 2, .Lsec_104_7b
        j        .Lsec_104_37a
.Lsec_104_7b:
        movi.n   a10, 4
        l8ui     a4, a2, 11
        bnone    a4, a10, .Lsec_104_86
        j        .Lsec_104_37a
.Lsec_104_86:
        movi     a4, 0x7ff
        add.n    a7, a7, a4
        l8ui     a11, a3, 19
        extui    a8, a6, 0, 16
        srli     a7, a7, 11
        l8ui     a4, a3, 5
        bnei     a11, 1, .Lsec_104_bd
        mul16u   a4, a4, a7
        add.n    a4, a9, a4
        l8ui     a7, a2, 25
        bany     a7, a10, .Lsec_104_d4
        mov.n    a10, a2
        l8ui     a11, a3, 13
        sext     a11, a11, 7
        s32i.n   a8, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, rcUpdateAckSnr
        l32r     a8, .Lsec_104_30
        callx8   a8
        l32i.n   a8, a1, 0
        j        .Lsec_104_d4
.Lsec_104_bd:
        beqz.n   a8, .Lsec_104_cc
        addi.n   a4, a4, -1
        mul16u   a4, a4, a7
        add.n    a9, a9, a4
        extui    a4, a9, 0, 16
        j        .Lsec_104_d4
.Lsec_104_cc:
        mull     a4, a4, a7
        j        .Lsec_104_da
        .byte    0x00
        .byte    0x00
.Lsec_104_d4:
        bgeu     a4, a8, .Lsec_104_da
        j        .Lsec_104_37a
.Lsec_104_da:
        l32i     a3, a2, 68
        add.n    a7, a3, a6
        l32r     a9, .Lsec_104_0
        bgeu     a9, a7, .Lsec_104_f4
        srli     a3, a3, 1
        l32i     a7, a2, 64
        s32i     a3, a2, 68
        srli     a3, a7, 1
        s32i     a3, a2, 64
.Lsec_104_f4:
        l32i     a3, a2, 68
        add.n    a3, a3, a6
        s32i     a3, a2, 68
        l32i     a3, a2, 64
        add.n    a3, a3, a4
        s32i     a3, a2, 64
        l8ui     a10, a2, 13
        l32i     a3, a2, 116
        s32i.n   a8, a1, 0
        memw
        callx8   a3
        mov.n    a3, a10
        l32i.n   a8, a1, 0
        l8ui     a7, a2, 12
        bnez     a8, .Lsec_104_168
        addi.n   a7, a7, 1
        extui    a7, a7, 0, 8
        l8ui     a8, a2, 15
        add.n    a8, a4, a8
        s8i      a8, a2, 15
        memw
        l32i     a8, a2, 116
        s8i      a7, a2, 12
        memw
        add      a7, a7, a10
        movi     a10, 16
        memw
        callx8   a8
        bge      a10, a7, .Lsec_104_150
        l32r     a7, .Lsec_104_4
        l8ui     a7, a7, 24
        bgeu     a3, a7, .Lsec_104_150
        j        .Lsec_104_28d
.Lsec_104_150:
        movi.n   a8, 8
        l8ui     a7, a2, 12
        bgeu     a8, a7, .Lsec_104_15c
        j        .Lsec_104_28d
        .byte    0x00
.Lsec_104_15c:
        l8ui     a7, a2, 15
        movi.n   a8, 64
        bgeu     a8, a7, .Lsec_104_17a
        j        .Lsec_104_28d
        .byte    0x00
.Lsec_104_168:
        beqz.n   a7, .Lsec_104_17a
        movi.n   a3, 0
        s8i      a3, a2, 12
        memw
        memw
        j        .Lsec_104_37a
        .byte    0x00
        .byte    0x00
.Lsec_104_17a:
        mov.n    a10, a2
        slli     a6, a6, 7
        .reloc  ., R_XTENSA_ASM_EXPAND, TRC_AMPDU_PER_DOWN_THRESHOLD
        l32r     a8, .Lsec_104_34
        callx8   a8
        quos     a6, a6, a4
        mov.n    a7, a10
        bgeu     a10, a6, .Lsec_104_195
        movi.n   a4, 0
        s8i      a4, a2, 15
        memw
.Lsec_104_195:
        l32r     a6, .Lsec_104_8
        l8ui     a4, a6, 0
        beqz.n   a4, .Lsec_104_1c5
        l32r     a4, .Lsec_104_c
        memw
        l8ui     a4, a4, 0
        extui    a4, a4, 0, 8
        bnez.n   a4, .Lsec_104_1c5
        l32r     a4, .Lsec_104_10
        l32i.n   a4, a4, 0
        beqz.n   a4, .Lsec_104_1c5
        l32r     a4, .Lsec_104_14
        l32i.n   a4, a4, 0
        l32i     a10, a4, 0x110
        memw
        callx8   a10
        mov.n    a6, a10
        j        .Lsec_104_1d4
.Lsec_104_1c5:
        l32r     a4, .Lsec_104_18
        memw
        l32i.n   a9, a4, 0
        l32r     a4, .Lsec_104_1c
        l32i.n   a4, a4, 0
        add.n    a6, a9, a4
.Lsec_104_1d4:
        l32i     a4, a2, 64
        l32i     a11, a2, 76
        beqz     a4, .Lsec_104_37a
        mov.n    a10, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, trc_calc_duration
        l32r     a8, .Lsec_104_38
        callx8   a8
        l32r     a8, .Lsec_104_20
        bltu     a8, a10, .Lsec_104_1f4
        movi     a8, 0x1f3
        bltu     a8, a4, .Lsec_104_1f4
        j        .Lsec_104_37a
.Lsec_104_1f4:
        l32i     a8, a2, 88
        addi.n   a8, a8, 1
        s32i     a8, a2, 88
        l32i     a8, a2, 68
        slli     a8, a8, 7
        quou     a8, a8, a4
        extui    a4, a8, 0, 8
        l8ui     a11, a2, 30
        s32i     a6, a2, 76
        s8i      a4, a2, 29
        memw
        beqz.n   a11, .Lsec_104_238
        extui    a8, a8, 0, 8
        addx2    a4, a11, a11
        add.n    a4, a4, a8
        srai     a4, a4, 2
        s8i      a4, a2, 30
        memw
        maxu     a10, a4, a11
        l8ui     a8, a2, 39
        bgeu     a10, a7, .Lsec_104_29c
        memw
        j        .Lsec_104_266
        .byte    0x00
        .byte    0x00
.Lsec_104_238:
        movi     a3, 128
        addx2    a10, a7, a7
        add.n    a10, a10, a3
        srli     a3, a10, 2
        extui    a10, a10, 2, 8
        bltu     a10, a4, .Lsec_104_24c
        j        .Lsec_104_369
.Lsec_104_24c:
        extui    a3, a3, 0, 8
        extui    a8, a8, 0, 8
        add.n    a3, a3, a8
        srai     a3, a3, 1
        s8i      a3, a2, 30
        memw
        memw
        j        .Lsec_104_36f
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_104_266:
        beqz.n   a8, .Lsec_104_276
        l32i     a4, a2, 92
        l32r     a7, .Lsec_104_28
        bltu     a7, a4, .Lsec_104_276
        add.n    a4, a4, a4
        s32i     a4, a2, 92
.Lsec_104_276:
        mov.n    a10, a6
        l32i     a11, a2, 80
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, trc_calc_duration
        l32r     a8, .Lsec_104_3c
        callx8   a8
        l32r     a4, .Lsec_104_24
        bltu     a4, a10, .Lsec_104_28d
        j        .Lsec_104_36f
.Lsec_104_28d:
        mov.n    a12, a5
        mov.n    a11, a3
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, rcAmpduLowerRate
        l32r     a8, .Lsec_104_40
        callx8   a8
        j        .Lsec_104_37a
.Lsec_104_29c:
        beqz.n   a8, .Lsec_104_2b4
        movi.n   a3, 0
        s8i      a3, a2, 39
        memw
        l32r     a3, .Lsec_104_2c
        s32i     a3, a2, 92
        memw
        j        .Lsec_104_36f
        .byte    0x00
        .byte    0x00
.Lsec_104_2b4:
        l8ui     a5, a2, 4
        bne      a5, a3, .Lsec_104_2bd
        j        .Lsec_104_36f
.Lsec_104_2bd:
        mov.n    a10, a2
        s32i.n   a11, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, TRC_AMPDU_PER_UP_THRESHOLD
        l32r     a8, .Lsec_104_44
        callx8   a8
        l32i.n   a11, a1, 0
        minu     a4, a4, a11
        bltu     a10, a4, .Lsec_104_2d5
        j        .Lsec_104_36f
.Lsec_104_2d5:
        mov.n    a10, a6
        l32i     a11, a2, 84
        .reloc  ., R_XTENSA_ASM_EXPAND, trc_calc_duration
        l32r     a8, .Lsec_104_48
        callx8   a8
        l32i     a4, a2, 92
        bltu     a4, a10, .Lsec_104_2e9
        j        .Lsec_104_36f
.Lsec_104_2e9:
        l32r     a5, .Lsec_104_8
        l8ui     a4, a5, 0
        beqz.n   a4, .Lsec_104_315
        l32r     a4, .Lsec_104_c
        memw
        l8ui     a4, a4, 0
        extui    a4, a4, 0, 8
        bnez.n   a4, .Lsec_104_315
        l32r     a4, .Lsec_104_10
        l32i.n   a4, a4, 0
        beqz.n   a4, .Lsec_104_315
        l32r     a4, .Lsec_104_14
        l32i.n   a4, a4, 0
        l32i     a10, a4, 0x110
        callx8   a10
        j        .Lsec_104_324
        .byte    0x00
.Lsec_104_315:
        l32r     a4, .Lsec_104_18
        memw
        l32i.n   a10, a4, 0
        l32r     a4, .Lsec_104_1c
        l32i.n   a4, a4, 0
        add.n    a10, a10, a4
.Lsec_104_324:
        movi.n   a4, 0
        addi.n   a3, a3, -1
        s8i      a4, a2, 12
        memw
        extui    a3, a3, 0, 8
        addx2    a3, a3, a3
        l32i     a4, a2, 112
        addx4    a3, a3, a4
        s32i     a10, a2, 84
        l8ui     a10, a3, 0
        movi.n   a3, 1
        s8i      a10, a2, 13
        s8i      a3, a2, 39
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, rx11NRate2AMPDULimit
        l32r     a8, .Lsec_104_4c
        callx8   a8
        mov.n    a11, a10
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, rcSetTxAmpduLimit
        l32r     a8, .Lsec_104_50
        callx8   a8
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, rcClearCurAMPDUSched
        l32r     a8, .Lsec_104_54
        callx8   a8
        j        .Lsec_104_37a
.Lsec_104_369:
        s8i      a10, a2, 30
        memw
.Lsec_104_36f:
        movi.n   a3, 0
        s32i     a3, a2, 68
        s32i     a3, a2, 64
        s32i     a3, a2, 72
.Lsec_104_37a:
        memw
        retw.n
        .size   rcUpdateTxDoneAmpdu2, .-rcUpdateTxDoneAmpdu2
.Lsec_104_37f:

        .section .text.rcUpdateRate,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_105_0, g_mesh_is_started
        .literal .Lsec_105_4, g_mesh_is_root
        .literal .Lsec_105_8, g_mesh_init_ps_type
        .literal .Lsec_105_c, g_osi_funcs_p
        .literal .Lsec_105_10, 0x3ff73c00
        .literal .Lsec_105_14, g_wifi_mac_time_delta
        .literal .Lsec_105_18, 0x0001e847
        .literal .Lsec_105_1c, 0x0001e848
        .literal .Lsec_105_20, 0x0007a120
        .literal .Lsec_105_24, trc_ctl
        .literal .Lsec_105_28, 0x003d08ff
        .literal .Lsec_105_2c, trc_calc_duration
        .literal .Lsec_105_30, trc_calc_duration
        .literal .Lsec_105_34, rcClearCurSched
        .literal .Lsec_105_38, trc_calc_duration
        .literal .Lsec_105_3c, TRC_PER_IS_GOOD
        .literal .Lsec_105_40, rcUpSched
        .literal .Lsec_105_44, rssi_margin
        .literal .Lsec_105_48, trc_calc_duration
        .literal .Lsec_105_4c, rcLowerSched
.Lsec_105_50:
        .global rcUpdateRate
        .type   rcUpdateRate, @function
rcUpdateRate:
        entry    a1, 48
        l32r     a3, .Lsec_105_0
        l8ui     a3, a3, 0
        beqz.n   a3, .Lsec_105_81
        l32r     a3, .Lsec_105_4
        memw
        l8ui     a3, a3, 0
        extui    a3, a3, 0, 8
        bnez.n   a3, .Lsec_105_81
        l32r     a3, .Lsec_105_8
        l32i.n   a3, a3, 0
        beqz.n   a3, .Lsec_105_81
        l32r     a3, .Lsec_105_c
        l32i     a3, a3, 0
        l32i     a10, a3, 0x110
        callx8   a10
        mov.n    a4, a10
        j        .Lsec_105_90
.Lsec_105_81:
        l32r     a3, .Lsec_105_10
        memw
        l32i.n   a4, a3, 0
        l32r     a3, .Lsec_105_14
        l32i.n   a3, a3, 0
        add.n    a4, a4, a3
.Lsec_105_90:
        l32i.n   a6, a2, 40
        l32i.n   a3, a2, 56
        beqz     a6, .Lsec_105_d8
        mov.n    a11, a3
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, trc_calc_duration
        l32r     a8, .Lsec_105_2c
        callx8   a8
        l32i.n   a5, a2, 60
        movi     a7, 99
        bgeu     a7, a5, .Lsec_105_d8
        l32r     a5, .Lsec_105_18
        bgeu     a5, a10, .Lsec_105_d8
        l32r     a3, .Lsec_105_24
        l32i.n   a5, a3, 0
        addi.n   a5, a5, 1
        s32i.n   a5, a3, 0
        l32i.n   a3, a2, 44
        slli     a3, a3, 7
        quou     a3, a3, a6
        extui    a6, a3, 0, 8
        l8ui     a7, a2, 28
        s32i.n   a4, a2, 56
        s8i      a6, a2, 27
        memw
        bnez     a7, .Lsec_105_14a
        memw
        j        .Lsec_105_176
        .byte    0x00
.Lsec_105_d8:
        l32i     a4, a2, 76
        mov.n    a11, a3
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, trc_calc_duration
        l32r     a8, .Lsec_105_30
        callx8   a8
        l8ui     a3, a2, 14
        beqi     a3, 2, .Lsec_105_ee
        j        .Lsec_105_248
.Lsec_105_ee:
        l32r     a3, .Lsec_105_18
        bltu     a3, a10, .Lsec_105_f7
        j        .Lsec_105_248
.Lsec_105_f7:
        mov.n    a10, a2
        l32i     a3, a2, 96
        l8ui     a3, a3, 10
        s32i.n   a4, a2, 56
        l32r     a4, .Lsec_105_20
        s32i     a4, a2, 92
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, rcClearCurSched
        l32r     a8, .Lsec_105_34
        callx8   a8
        l8ui     a10, a2, 13
        l32i     a5, a2, 116
        l32i     a4, a2, 112
        callx8   a5
        extui    a10, a10, 0, 16
        addx2    a10, a10, a10
        addx4    a10, a10, a4
        s32i     a10, a2, 96
        l8ui     a4, a10, 10
        bgeu     a3, a4, .Lsec_105_139
        l32i     a3, a2, 76
        s32i.n   a3, a2, 48
        memw
        j        .Lsec_105_240
.Lsec_105_139:
        bltu     a4, a3, .Lsec_105_13f
        j        .Lsec_105_240
.Lsec_105_13f:
        l32i     a3, a2, 76
        s32i.n   a3, a2, 52
        memw
        j        .Lsec_105_240
.Lsec_105_14a:
        extui    a3, a3, 0, 8
        add.n    a3, a3, a7
        l32i.n   a11, a2, 52
        srai     a3, a3, 1
        mov.n    a10, a4
        s8i      a3, a2, 28
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, trc_calc_duration
        l32r     a8, .Lsec_105_38
        callx8   a8
        mov.n    a7, a10
        mov.n    a11, a3
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, TRC_PER_IS_GOOD
        l32r     a8, .Lsec_105_3c
        callx8   a8
        bnez.n   a10, .Lsec_105_1ad
        j        .Lsec_105_1c0
.Lsec_105_176:
        l32i     a4, a2, 96
        l8ui     a8, a4, 9
        movi     a4, 128
        addx2    a8, a8, a8
        add.n    a8, a8, a4
        srai     a4, a8, 2
        extui    a8, a8, 2, 8
        bltu     a8, a6, .Lsec_105_199
        s8i      a8, a2, 28
        memw
        memw
        j        .Lsec_105_240
.Lsec_105_199:
        extui    a3, a3, 0, 8
        add.n    a3, a3, a4
        srai     a3, a3, 1
        s8i      a3, a2, 28
        memw
        memw
        j        .Lsec_105_240
.Lsec_105_1ad:
        l32i     a6, a2, 92
        bgeu     a6, a7, .Lsec_105_1c0
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, rcUpSched
        l32r     a8, .Lsec_105_40
        callx8   a8
        j        .Lsec_105_240
        .byte    0x00
.Lsec_105_1c0:
        l32i     a6, a2, 96
        l8ui     a8, a6, 11
        extui    a9, a8, 0, 1
        bnez.n   a9, .Lsec_105_1d1
        extui    a5, a5, 0, 2
        bnez     a5, .Lsec_105_240
.Lsec_105_1d1:
        l8ui     a11, a6, 0
        l8ui     a10, a2, 1
        s32i.n   a8, a1, 0
        s32i.n   a9, a1, 4
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, rssi_margin
        l32r     a8, .Lsec_105_44
        callx8   a8
        extui    a10, a10, 1, 8
        l8ui     a5, a6, 9
        sub      a5, a5, a10
        l32i.n   a8, a1, 0
        l32i.n   a9, a1, 4
        bge      a3, a5, .Lsec_105_21d
        beqz.n   a9, .Lsec_105_204
        l32i     a3, a2, 92
        l32r     a5, .Lsec_105_28
        bltu     a5, a3, .Lsec_105_204
        add.n    a3, a3, a3
        s32i     a3, a2, 92
.Lsec_105_204:
        mov.n    a10, a4
        l32i.n   a11, a2, 48
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, trc_calc_duration
        l32r     a8, .Lsec_105_48
        callx8   a8
        l32r     a3, .Lsec_105_1c
        bgeu     a3, a7, .Lsec_105_240
        bltu     a3, a10, .Lsec_105_236
        j        .Lsec_105_240
.Lsec_105_21d:
        beqz.n   a9, .Lsec_105_240
        movi.n   a3, -2
        and      a8, a8, a3
        l32r     a3, .Lsec_105_20
        s8i      a8, a6, 11
        memw
        s32i     a3, a2, 92
        memw
        j        .Lsec_105_240
.Lsec_105_236:
        movi.n   a11, 1
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, rcLowerSched
        l32r     a8, .Lsec_105_4c
        callx8   a8
.Lsec_105_240:
        movi.n   a3, 0
        s32i.n   a3, a2, 44
        s32i.n   a3, a2, 40
        s32i.n   a3, a2, 60
.Lsec_105_248:
        memw
        retw.n
        .size   rcUpdateRate, .-rcUpdateRate
.Lsec_105_24d:

        .section .wifi0iram.10,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_106_0, 0x02000008
        .literal .Lsec_106_4, rcLoRaSchedTbl+0xc
        .literal .Lsec_106_8, rc11BSchedTbl+0x24
        .literal .Lsec_106_c, g_wifi_nvs
        .literal .Lsec_106_10, BasicOFDMSched
        .literal .Lsec_106_14, rcLoRaSchedTbl
        .literal .Lsec_106_18, rcUpdateRate
        .literal .Lsec_106_1c, wl_is_ap_no_lr
.Lsec_106_20:
        .global rcGetSched
        .type   rcGetSched, @function
rcGetSched:
        entry    a1, 32
        mov.n    a10, a2
        beqz     a2, .Lsec_106_e9
        l32i.n   a9, a3, 0
        l32r     a11, .Lsec_106_0
        and      a11, a9, a11
        l8ui     a8, a2, 11
        bnei     a11, 8, .Lsec_106_60
        l32i     a11, a2, 96
        s32i.n   a11, a3, 28
        bbci     a8, 0, .Lsec_106_48
        l8ui     a8, a2, 8
        memw
        j        .Lsec_106_e3
        .byte    0x00
.Lsec_106_48:
        bbci     a9, 1, .Lsec_106_51
        l32i     a8, a2, 104
        j        .Lsec_106_dc
.Lsec_106_51:
        sext     a8, a8, 7
        bltz     a8, .Lsec_106_de
        .reloc  ., R_XTENSA_ASM_EXPAND, rcUpdateRate
        l32r     a8, .Lsec_106_18
        callx8   a8
        j        .Lsec_106_de
.Lsec_106_60:
        l32i     a11, a2, 100
        s32i.n   a11, a3, 28
        bbci     a8, 1, .Lsec_106_71
        l8ui     a8, a2, 9
        memw
        j        .Lsec_106_e3
.Lsec_106_71:
        sext     a8, a8, 7
        bgez     a8, .Lsec_106_c1
        movi.n   a9, 3
        l32i.n   a8, a3, 16
        slli     a9, a9, 18
        bany     a8, a9, .Lsec_106_b0
        bbci     a8, 16, .Lsec_106_8d
.Lsec_106_84:
        l32r     a8, .Lsec_106_4
        j        .Lsec_106_dc
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_106_8d:
        bbsi     a8, 17, .Lsec_106_98
.Lsec_106_90:
        l32r     a8, .Lsec_106_8
        j        .Lsec_106_dc
        .byte    0x00
        .byte    0x00
.Lsec_106_98:
        .reloc  ., R_XTENSA_ASM_EXPAND, wl_is_ap_no_lr
        l32r     a8, .Lsec_106_1c
        callx8   a8
        bnez     a10, .Lsec_106_90
        l32r     a8, .Lsec_106_c
        l32i.n   a8, a8, 0
        addmi    a8, a8, 0x400
        l8ui     a8, a8, 117
        j        .Lsec_106_bb
        .byte    0x00
.Lsec_106_b0:
        l32r     a8, .Lsec_106_c
        l32i.n   a8, a8, 0
        addmi    a8, a8, 0x500
        l8ui     a8, a8, 13
.Lsec_106_bb:
        bnez     a8, .Lsec_106_84
        j        .Lsec_106_de
.Lsec_106_c1:
        movi     a8, 0x401
        slli     a8, a8, 11
        bnone    a9, a8, .Lsec_106_de
        l8ui     a8, a2, 130
        beqz.n   a8, .Lsec_106_de
        l32r     a8, .Lsec_106_10
        s32i.n   a8, a3, 28
        l8ui     a8, a2, 135
        beqz.n   a8, .Lsec_106_de
        l32r     a8, .Lsec_106_14
.Lsec_106_dc:
        s32i.n   a8, a3, 28
.Lsec_106_de:
        l32i.n   a8, a3, 28
        l8ui     a8, a8, 0
.Lsec_106_e3:
        s8i      a8, a3, 12
        memw
.Lsec_106_e9:
        memw
        retw.n
        .size   rcGetSched, .-rcGetSched
.Lsec_106_ee:

        .section .text.rcReachRetryLimit,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_107_0, .Lsec_156_0
        .literal .Lsec_107_4, .Lsec_144_0
        .literal .Lsec_107_8, .Lsec_93_0
        .literal .Lsec_107_c, .Lsec_155_0
        .literal .Lsec_107_10, 0x00000ac7
        .literal .Lsec_107_14, .Lsec_154_0
        .literal .Lsec_107_18, .Lsec_153_0
        .literal .Lsec_107_1c, 0x00000ad2
        .literal .Lsec_107_20, g_mesh_is_started
        .literal .Lsec_107_24, wifi_log
        .literal .Lsec_107_28, wifi_log
        .literal .Lsec_107_2c, wifi_log
        .literal .Lsec_107_30, wifi_log
.Lsec_107_34:
        .global rcReachRetryLimit
        .type   rcReachRetryLimit, @function
rcReachRetryLimit:
        entry    a1, 48
        bnez.n   a2, .Lsec_107_6d
        l32r     a13, .Lsec_107_0
        movi.n   a12, 1
        movi     a11, 0x200
        movi.n   a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_107_24
        callx8   a8
        l32r     a2, .Lsec_107_10
        movi.n   a12, 1
        slli     a11, a12, 11
        movi.n   a10, 6
        l32r     a15, .Lsec_107_4
        l32r     a14, .Lsec_107_8
        l32r     a13, .Lsec_107_c
        s32i     a2, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_107_28
        callx8   a8
.Lsec_107_68:
        j        .Lsec_107_68
        .byte    0x00
        .byte    0x00
.Lsec_107_6d:
        l8ui     a9, a2, 5
        bltui    a9, 5, .Lsec_107_78
        l32i.n   a8, a2, 0
        bbsi     a8, 27, .Lsec_107_e8
.Lsec_107_78:
        l32i.n   a8, a2, 28
        bnez     a8, .Lsec_107_c0
        movi.n   a12, 1
        movi     a11, 0x200
        l32i.n   a15, a2, 4
        movi.n   a10, 6
        extui    a15, a15, 0, 4
        l32i.n   a14, a2, 16
        extui    a14, a14, 18, 2
        l32i.n   a2, a2, 0
        l32r     a13, .Lsec_107_14
        s32i.n   a2, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_107_2c
        callx8   a8
        movi.n   a12, 1
        slli     a11, a12, 11
        movi.n   a10, 6
        l32r     a2, .Lsec_107_1c
        l32r     a15, .Lsec_107_4
        l32r     a14, .Lsec_107_8
        l32r     a13, .Lsec_107_18
        s32i.n   a2, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_107_30
        callx8   a8
.Lsec_107_bc:
        j        .Lsec_107_bc
        .byte    0x00
.Lsec_107_c0:
        l32r     a2, .Lsec_107_20
        l8ui     a10, a2, 0
        movi.n   a2, 1
        l8ui     a8, a8, 8
        beqz.n   a10, .Lsec_107_dc
        movi.n   a10, 11
        minu     a8, a8, a10
        bgeu     a9, a8, .Lsec_107_e1
        movi.n   a2, 0
        j        .Lsec_107_e1
        .byte    0x00
        .byte    0x00
.Lsec_107_dc:
        bgeu     a9, a8, .Lsec_107_e1
        mov.n    a2, a10
.Lsec_107_e1:
        extui    a2, a2, 0, 8
        j        .Lsec_107_ea
        .byte    0x00
.Lsec_107_e8:
        movi.n   a2, 1
.Lsec_107_ea:
        retw.n
        .size   rcReachRetryLimit, .-rcReachRetryLimit
.Lsec_107_ec:

        .section .text.rcAttach,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_108_0, trc_ctl
        .literal .Lsec_108_4, rc11NSchedTbl
        .literal .Lsec_108_8, rcP2P11NSchedTbl
        .literal .Lsec_108_c, rc11GSchedTbl
        .literal .Lsec_108_10, rcP2P11GSchedTbl
        .literal .Lsec_108_14, rc11BSchedTbl
        .literal .Lsec_108_18, rcLoRaSchedTbl
        .literal .Lsec_108_1c, .Lsec_67_0
        .literal .Lsec_108_20, .Lsec_67_0
        .literal .Lsec_108_24, .Lsec_67_0
        .literal .Lsec_108_28, .Lsec_67_0
        .literal .Lsec_108_2c, .Lsec_67_0
        .literal .Lsec_108_30, .Lsec_67_0
.Lsec_108_34:
        .global rcAttach
        .type   rcAttach, @function
rcAttach:
        entry    a1, 32
        l32r     a8, .Lsec_108_0
        movi.n   a9, 0
        movi.n   a11, 14
        l32r     a10, .Lsec_108_4
        s32i.n   a9, a8, 4
        s32i     a9, a8, 8
        s32i     a9, a8, 12
        s32i     a9, a8, 20
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, .Lsec_67_0
        l32r     a8, .Lsec_108_1c
        callx8   a8
        movi     a11, 10
        l32r     a10, .Lsec_108_8
        .reloc  ., R_XTENSA_ASM_EXPAND, .Lsec_67_0
        l32r     a8, .Lsec_108_20
        callx8   a8
        movi     a11, 13
        l32r     a10, .Lsec_108_c
        .reloc  ., R_XTENSA_ASM_EXPAND, .Lsec_67_0
        l32r     a8, .Lsec_108_24
        callx8   a8
        movi     a11, 8
        l32r     a10, .Lsec_108_10
        .reloc  ., R_XTENSA_ASM_EXPAND, .Lsec_67_0
        l32r     a8, .Lsec_108_28
        callx8   a8
        movi     a11, 6
        l32r     a10, .Lsec_108_14
        .reloc  ., R_XTENSA_ASM_EXPAND, .Lsec_67_0
        l32r     a8, .Lsec_108_2c
        callx8   a8
        movi.n   a11, 2
        l32r     a10, .Lsec_108_18
        .reloc  ., R_XTENSA_ASM_EXPAND, .Lsec_67_0
        l32r     a8, .Lsec_108_30
        callx8   a8
        retw.n
        .size   rcAttach, .-rcAttach
.Lsec_108_92:

        .section .text.trc_NeedRTS,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_109_0, 0x02020480
        .literal .Lsec_109_4, 0x01010840
        .literal .Lsec_109_8, .Lsec_110_0
        .literal .Lsec_109_c, lmacConfMib
.Lsec_109_10:
        .global trc_NeedRTS
        .type   trc_NeedRTS, @function
trc_NeedRTS:
        entry    a1, 32
        l32i.n   a8, a2, 44
        l8ui     a10, a8, 12
        l16ui    a8, a2, 20
        l16ui    a2, a2, 22
        add.n    a8, a8, a2
        bgeui    a10, 6, .Lsec_109_50
        j        .Lsec_109_45
.Lsec_109_26:
        movi.n   a2, 1
        l32r     a11, .Lsec_109_0
        ssl      a10
        sll      a9, a2
        bany     a9, a11, .Lsec_109_80
        l32r     a11, .Lsec_109_4
        and      a9, a9, a11
        bnez.n   a9, .Lsec_109_6e
        movi.n   a11, 15
        bne      a10, a11, .Lsec_109_88
        j        .Lsec_109_77
        .byte    0x00
.Lsec_109_45:
        l32r     a2, .Lsec_109_8
        addx4    a2, a10, a2
        l32i.n   a2, a2, 0
        jx       a2
.Lsec_109_50:
        addi     a2, a10, -6
        extui    a2, a2, 0, 8
        movi.n   a9, 19
        bgeu     a9, a2, .Lsec_109_26
        j        .Lsec_109_88
.Lsec_109_5e:
        movi.n   a2, 1
        movi     a9, 0x271
        j        .Lsec_109_90
.Lsec_109_66:
        movi.n   a2, 1
        movi     a9, 0x3e8
        j        .Lsec_109_90
.Lsec_109_6e:
        movi.n   a2, 1
        movi     a9, 0x2bc
        j        .Lsec_109_90
        .byte    0x00
.Lsec_109_77:
        movi     a10, 0x41a
        blt      a10, a8, .Lsec_109_95
        j        .Lsec_109_93
.Lsec_109_80:
        movi.n   a2, 1
        movi     a9, 0x460
        j        .Lsec_109_90
.Lsec_109_88:
        l32r     a2, .Lsec_109_c
        l16ui    a9, a2, 22
        movi.n   a2, 1
.Lsec_109_90:
        blt      a9, a8, .Lsec_109_95
.Lsec_109_93:
        movi.n   a2, 0
.Lsec_109_95:
        extui    a2, a2, 0, 8
        retw.n
        .size   trc_NeedRTS, .-trc_NeedRTS
.Lsec_109_9a:

        .section .rodata.trc_NeedRTS,"a",@progbits
        .align  4
.Lsec_110_0:
        .word   .Lsec_109_5e
        .word   .Lsec_109_66
        .word   .Lsec_109_6e
        .word   .Lsec_109_80
        .word   .Lsec_109_88
        .word   .Lsec_109_66

        .section .text.trc_onDisconnect,"ax",@progbits
        .align  4
.Lsec_111_0:
        .global trc_onDisconnect
        .type   trc_onDisconnect, @function
trc_onDisconnect:
        entry    a1, 32
        retw.n
        .size   trc_onDisconnect, .-trc_onDisconnect
.Lsec_111_5:

        .section .text.trc_onScanStart,"ax",@progbits
        .align  4
.Lsec_112_0:
        .global trc_onScanStart
        .type   trc_onScanStart, @function
trc_onScanStart:
        entry    a1, 32
        l8ui     a8, a2, 25
        movi.n   a9, -6
        and      a8, a8, a9
        movi.n   a9, 5
        or       a8, a8, a9
        s8i      a8, a2, 25
        memw
        memw
        retw.n
        .size   trc_onScanStart, .-trc_onScanStart
.Lsec_112_1b:

        .section .text.trc_onScanDone,"ax",@progbits
        .align  4
.Lsec_113_0:
        .global trc_onScanDone
        .type   trc_onScanDone, @function
trc_onScanDone:
        entry    a1, 32
        l8ui     a8, a2, 25
        movi.n   a9, -6
        and      a8, a8, a9
        s8i      a8, a2, 25
        memw
        memw
        retw.n
        .size   trc_onScanDone, .-trc_onScanDone
.Lsec_113_16:

        .section .text.trc_isAmpduOn,"ax",@progbits
        .align  4
.Lsec_114_0:
        .global trc_isAmpduOn
        .type   trc_isAmpduOn, @function
trc_isAmpduOn:
        entry    a1, 32
        l8ui     a2, a2, 25
        srli     a2, a2, 7
        retw.n
        .size   trc_isAmpduOn, .-trc_isAmpduOn
.Lsec_114_b:

        .section .wifiextrairam.18,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_115_0, trc_ctl
        .literal .Lsec_115_4, trcAmpduSetState
        .literal .Lsec_115_8, rcClearCurAMPDUSched
        .literal .Lsec_115_c, trc_onAmpduOp
.Lsec_115_10:
        .global trc_onPPTxDone
        .type   trc_onPPTxDone, @function
trc_onPPTxDone:
        entry    a1, 32
        beqz     a2, .Lsec_115_9f
        l8ui     a4, a2, 11
        sext     a8, a4, 7
        bltz     a8, .Lsec_115_9f
        extui    a4, a4, 0, 1
        bnez     a4, .Lsec_115_9f
        l8ui     a8, a2, 25
        bbci     a8, 7, .Lsec_115_9f
        l32i.n   a3, a3, 44
        l32i.n   a3, a3, 4
        extui    a3, a3, 0, 4
        add.n    a3, a2, a3
        l8ui     a3, a3, 17
        bnei     a3, 1, .Lsec_115_9f
        l32r     a3, .Lsec_115_0
        l8ui     a8, a3, 25
        l32i     a3, a2, 96
        l8ui     a9, a3, 10
        addi.n   a3, a8, -1
        bge      a9, a3, .Lsec_115_9f
        addi     a8, a8, -2
        extui    a8, a8, 0, 8
        l32i     a3, a2, 112
        addx2    a8, a8, a8
        addx4    a8, a8, a3
        movi.n   a11, 4
        or       a10, a2, a2
        l8ui     a3, a8, 0
        s8i      a3, a2, 13
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, trcAmpduSetState
        l32r     a8, .Lsec_115_4
        callx8   a8
        movi.n   a3, 4
        mov.n    a10, a2
        s8i      a3, a2, 14
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, rcClearCurAMPDUSched
        l32r     a8, .Lsec_115_8
        callx8   a8
        mov.n    a3, a4
        movi.n   a4, 2
.Lsec_115_88:
        l8ui     a8, a2, 16
        bbc      a8, a3, .Lsec_115_9a
        mov.n    a12, a3
        mov.n    a11, a4
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, trc_onAmpduOp
        l32r     a8, .Lsec_115_c
        callx8   a8
.Lsec_115_9a:
        addi.n   a3, a3, 1
        bnei     a3, 8, .Lsec_115_88
.Lsec_115_9f:
        retw.n
        .size   trc_onPPTxDone, .-trc_onPPTxDone
.Lsec_115_a1:

        .section .text.trc_SetTxAmpduState,"ax",@progbits
        .align  4
.Lsec_116_0:
        .global trc_SetTxAmpduState
        .type   trc_SetTxAmpduState, @function
trc_SetTxAmpduState:
        entry    a1, 32
        s8i      a3, a2, 14
        memw
        memw
        retw.n
        .size   trc_SetTxAmpduState, .-trc_SetTxAmpduState
.Lsec_116_e:

        .section .wifi0iram.19,"ax",@progbits
        .align  4
.Lsec_117_0:
        .global trc_isTxAmpduOperational
        .type   trc_isTxAmpduOperational, @function
trc_isTxAmpduOperational:
        entry    a1, 32
        l8ui     a8, a2, 14
        movi.n   a9, 0
        addi     a8, a8, -2
        movi.n   a2, 1
        movnez   a2, a9, a8
        retw.n
        .size   trc_isTxAmpduOperational, .-trc_isTxAmpduOperational
.Lsec_117_12:

        .section .wifi0iram.20,"ax",@progbits
        .align  4
.Lsec_118_0:
        .global trc_tid_isTxAmpduOperational
        .type   trc_tid_isTxAmpduOperational, @function
trc_tid_isTxAmpduOperational:
        entry    a1, 32
        extui    a3, a3, 0, 8
        add.n    a2, a2, a3
        l8ui     a8, a2, 17
        movi.n   a3, 0
        addi     a8, a8, -2
        movi.n   a2, 1
        movnez   a2, a3, a8
        retw.n
        .size   trc_tid_isTxAmpduOperational, .-trc_tid_isTxAmpduOperational
.Lsec_118_17:

        .section .text.rc_get_mask,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_119_0, trc_ctl
.Lsec_119_4:
        .global rc_get_mask
        .type   rc_get_mask, @function
rc_get_mask:
        entry    a1, 32
        l32r     a8, .Lsec_119_0
        l32i.n   a2, a8, 20
        retw.n
        .size   rc_get_mask, .-rc_get_mask
.Lsec_119_e:

        .section .text.rc_disable_trc,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_120_0, trc_ctl
        .literal .Lsec_120_4, .Lsec_178_0
        .literal .Lsec_120_8, .Lsec_150_0
        .literal .Lsec_120_c, memcmp
        .literal .Lsec_120_10, wifi_log
.Lsec_120_14:
        .global rc_disable_trc
        .type   rc_disable_trc, @function
rc_disable_trc:
        entry    a1, 32
        l32r     a6, .Lsec_120_0
        extui    a2, a2, 0, 8
        addx4    a2, a2, a6
        mov.n    a11, a3
        extui    a7, a4, 0, 8
        l32i.n   a5, a2, 4
        bbc      a5, a4, .Lsec_120_5a
        movi.n   a12, 6
        l32r     a8, .Lsec_120_4
        addx4    a8, a7, a8
        l32i.n   a10, a8, 0
        addi     a10, a10, 31
        .reloc  ., R_XTENSA_ASM_EXPAND, memcmp
        l32r     a8, .Lsec_120_c
        callx8   a8
        bnez.n   a10, .Lsec_120_5a
        movi.n   a8, -2
        ssl      a7
        src      a8, a8, a8
        and      a5, a5, a8
        s32i.n   a5, a2, 4
        l32i.n   a2, a6, 20
        and      a2, a2, a8
        s32i.n   a2, a6, 20
        memw
        j        .Lsec_120_6c
        .byte    0x00
.Lsec_120_5a:
        movi.n   a11, 1
        l32r     a13, .Lsec_120_8
        movi.n   a12, 2
        slli     a11, a11, 11
        movi.n   a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_120_10
        callx8   a8
.Lsec_120_6c:
        retw.n
        .size   rc_disable_trc, .-rc_disable_trc
.Lsec_120_6e:

        .section .text.rc_disable_trc_by_interface,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_121_0, trc_ctl
.Lsec_121_4:
        .global rc_disable_trc_by_interface
        .type   rc_disable_trc_by_interface, @function
rc_disable_trc_by_interface:
        entry    a1, 32
        l32r     a8, .Lsec_121_0
        extui    a2, a2, 0, 8
        addx4    a2, a2, a8
        l32i.n   a9, a8, 20
        l32i.n   a10, a2, 4
        xor      a9, a9, a10
        s32i.n   a9, a8, 20
        movi.n   a8, 0
        s32i.n   a8, a2, 4
        memw
        retw.n
        .size   rc_disable_trc_by_interface, .-rc_disable_trc_by_interface
.Lsec_121_22:

        .section .text.rc_get_sta_trc,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_122_0, trc_ctl
        .literal .Lsec_122_4, .Lsec_178_0
.Lsec_122_8:
        .global rc_get_sta_trc
        .type   rc_get_sta_trc, @function
rc_get_sta_trc:
        entry    a1, 32
        l32r     a8, .Lsec_122_0
        extui    a2, a2, 0, 8
        addx4    a2, a2, a8
        l32i.n   a9, a2, 4
        mov.n    a2, a9
        beqz.n   a9, .Lsec_122_36
        addi.n   a8, a9, -1
        movi.n   a2, 0
        bany     a8, a9, .Lsec_122_36
        neg      a8, a9
        and      a8, a8, a9
        l32r     a2, .Lsec_122_4
        nsau     a8, a8
        neg      a8, a8
        addx4    a8, a8, a2
        l32i     a2, a8, 124
.Lsec_122_36:
        retw.n
        .size   rc_get_sta_trc, .-rc_get_sta_trc
.Lsec_122_38:

        .section .wifirxiram.24,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_123_0, trc_ctl
        .literal .Lsec_123_4, .Lsec_178_0
        .literal .Lsec_123_8, memcmp
.Lsec_123_c:
        .global rc_get_trc
        .type   rc_get_trc, @function
rc_get_trc:
        entry    a1, 48
        l32r     a4, .Lsec_123_0
        extui    a2, a2, 0, 8
        addx4    a2, a2, a4
        movi.n   a6, 6
        movi.n   a7, 1
        l32i.n   a5, a2, 4
        movi.n   a9, -1
        j        .Lsec_123_5d
        .byte    0x00
.Lsec_123_24:
        neg      a8, a5
        and      a8, a8, a5
        nsau     a8, a8
        neg      a8, a8
        addi     a4, a8, 31
        l32r     a8, .Lsec_123_4
        mov.n    a12, a6
        addx4    a2, a4, a8
        mov.n    a11, a3
        l32i.n   a2, a2, 0
        addi     a10, a2, 31
        s32i.n   a9, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, memcmp
        l32r     a8, .Lsec_123_8
        callx8   a8
        l32i.n   a9, a1, 0
        beqz.n   a10, .Lsec_123_62
        ssl      a4
        sll      a8, a7
        xor      a8, a9, a8
        and      a5, a5, a8
.Lsec_123_5d:
        bnez     a5, .Lsec_123_24
        mov.n    a2, a5
.Lsec_123_62:
        retw.n
        .size   rc_get_trc, .-rc_get_trc
.Lsec_123_64:

        .section .text.rc_get_trc_by_index,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_124_0, trc_ctl
        .literal .Lsec_124_4, .Lsec_178_0
.Lsec_124_8:
        .global rc_get_trc_by_index
        .type   rc_get_trc_by_index, @function
rc_get_trc_by_index:
        entry    a1, 32
        extui    a8, a2, 0, 8
        extui    a3, a3, 0, 8
        bnez.n   a8, .Lsec_124_28
        l32r     a2, .Lsec_124_0
        l32i.n   a8, a2, 20
        l32r     a2, .Lsec_124_4
        bbc      a8, a3, .Lsec_124_21
        j        .Lsec_124_54
.Lsec_124_21:
        l32i     a2, a2, 76
        j        .Lsec_124_60
        .byte    0x00
.Lsec_124_28:
        bnei     a8, 1, .Lsec_124_42
        l32r     a2, .Lsec_124_4
        beqz.n   a3, .Lsec_124_3c
        l32r     a8, .Lsec_124_0
        l32i.n   a8, a8, 20
        bbc      a8, a3, .Lsec_124_3c
        j        .Lsec_124_54
        .byte    0x00
.Lsec_124_3c:
        l32i     a2, a2, 80
        j        .Lsec_124_60
.Lsec_124_42:
        movi.n   a2, 0
        bnei     a8, 2, .Lsec_124_60
        l32r     a2, .Lsec_124_4
        beqz.n   a3, .Lsec_124_5d
        l32r     a8, .Lsec_124_0
        l32i.n   a8, a8, 20
        bbc      a8, a3, .Lsec_124_5d
.Lsec_124_54:
        addx4    a3, a3, a2
        l32i.n   a2, a3, 0
        j        .Lsec_124_60
        .byte    0x00
.Lsec_124_5d:
        l32i     a2, a2, 84
.Lsec_124_60:
        retw.n
        .size   rc_get_trc_by_index, .-rc_get_trc_by_index
.Lsec_124_62:

        .section .text.rc_get_trc_default,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_125_0, .Lsec_178_0
.Lsec_125_4:
        .global rc_get_trc_default
        .type   rc_get_trc_default, @function
rc_get_trc_default:
        entry    a1, 32
        extui    a8, a2, 0, 8
        bnez.n   a8, .Lsec_125_15
        l32r     a2, .Lsec_125_0
        l32i     a2, a2, 76
        j        .Lsec_125_2c
.Lsec_125_15:
        bnei     a8, 1, .Lsec_125_21
        l32r     a2, .Lsec_125_0
        l32i     a2, a2, 80
        j        .Lsec_125_2c
.Lsec_125_21:
        movi.n   a2, 0
        bnei     a8, 2, .Lsec_125_2c
        l32r     a2, .Lsec_125_0
        l32i     a2, a2, 84
.Lsec_125_2c:
        retw.n
        .size   rc_get_trc_default, .-rc_get_trc_default
.Lsec_125_2e:

        .section .text.rc_only_sta_trc,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_126_0, trc_ctl
.Lsec_126_4:
        .global rc_only_sta_trc
        .type   rc_only_sta_trc, @function
rc_only_sta_trc:
        entry    a1, 32
        l32r     a11, .Lsec_126_0
        movi.n   a2, 1
        movi.n   a9, 0
        l32i.n   a10, a11, 20
        addi.n   a8, a10, -1
        and      a8, a8, a10
        movnez   a9, a2, a8
        extui    a8, a9, 0, 8
        bnez.n   a9, .Lsec_126_2a
        moveqz   a8, a2, a10
        bnez.n   a8, .Lsec_126_2a
        l32i.n   a9, a11, 4
        movnez   a2, a8, a9
        j        .Lsec_126_2c
.Lsec_126_2a:
        movi.n   a2, -1
.Lsec_126_2c:
        retw.n
        .size   rc_only_sta_trc, .-rc_only_sta_trc
.Lsec_126_2e:

        .section .text.trc_init,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_127_0, g_osi_funcs_p
        .literal .Lsec_127_4, .Lsec_178_0
        .literal .Lsec_127_8, .Lsec_149_0
        .literal .Lsec_127_c, rc11BSchedTbl+0x24
        .literal .Lsec_127_10, rc11BSchedTbl
        .literal .Lsec_127_14, rcP2P11GSchedTbl+0x54
        .literal .Lsec_127_18, wifi_log
.Lsec_127_1c:
        .global trc_init
        .type   trc_init, @function
trc_init:
        entry    a1, 32
        l32r     a3, .Lsec_127_0
        movi     a10, 136
        l32i.n   a2, a3, 0
        l32i     a2, a2, 0x178
        callx8   a2
        l32i.n   a8, a3, 0
        l32r     a2, .Lsec_127_4
        l32i     a8, a8, 0x178
        s32i     a10, a2, 76
        movi     a10, 136
        memw
        callx8   a8
        l32i.n   a3, a3, 0
        l32i     a3, a3, 0x178
        s32i     a10, a2, 80
        movi     a10, 136
        memw
        callx8   a3
        l32i     a9, a2, 76
        s32i     a10, a2, 84
        beqz.n   a9, .Lsec_127_71
        movi.n   a15, 1
        movi     a11, 0
        moveqz   a11, a15, a10
        l32i     a8, a2, 80
        extui    a2, a11, 0, 8
        bnez     a11, .Lsec_127_71
        moveqz   a2, a15, a8
        beqz     a2, .Lsec_127_8c
.Lsec_127_71:
        l32r     a13, .Lsec_127_8
        movi.n   a12, 1
        movi     a11, 0x200
        movi.n   a10, 6
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_127_18
        callx8   a8
        movi     a2, 0x101
        j        .Lsec_127_ee
        .byte    0x00
        .byte    0x00
.Lsec_127_8c:
        l32r     a11, .Lsec_127_c
        movi     a12, -128
        movi.n   a3, 2
        l32r     a14, .Lsec_127_10
        l32r     a13, .Lsec_127_14
        s32i     a11, a9, 96
        s32i     a14, a9, 112
        s32i     a11, a9, 100
        s32i     a11, a9, 104
        s32i     a13, a9, 108
        s8i      a12, a9, 11
        s8i      a2, a9, 38
        s8i      a2, a9, 131
        s8i      a2, a9, 129
        s32i     a11, a8, 96
        s32i     a14, a8, 112
        s32i     a11, a8, 100
        s32i     a11, a8, 104
        s32i     a13, a8, 108
        s8i      a12, a8, 11
        s8i      a2, a8, 38
        s8i      a2, a8, 131
        s8i      a15, a8, 129
        s32i     a11, a10, 96
        s32i     a14, a10, 112
        s32i     a11, a10, 100
        s32i     a11, a10, 104
        s32i     a13, a10, 108
        s8i      a12, a10, 11
        s8i      a2, a10, 38
        s8i      a2, a10, 131
        s8i      a3, a10, 129
        memw
.Lsec_127_ee:
        memw
        retw.n
        .size   trc_init, .-trc_init
.Lsec_127_f3:

        .section .text.trc_update_def_rate,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_128_0, .Lsec_178_0
        .literal .Lsec_128_4, rcP2P11GSchedTbl+0x54
        .literal .Lsec_128_8, rcP2P11GSchedTbl
        .literal .Lsec_128_c, rc11BSchedTbl+0x24
        .literal .Lsec_128_10, rc11BSchedTbl
        .literal .Lsec_128_14, .Lsec_148_0
        .literal .Lsec_128_18, wifi_log
.Lsec_128_1c:
        .global trc_update_def_rate
        .type   trc_update_def_rate, @function
trc_update_def_rate:
        entry    a1, 32
        l32r     a10, .Lsec_128_0
        extui    a4, a4, 0, 8
        l32i     a9, a10, 76
        l32i     a8, a10, 80
        l32i     a10, a10, 84
        bnez     a3, .Lsec_128_7c
        beqz.n   a4, .Lsec_128_58
        l32r     a3, .Lsec_128_4
        l32r     a4, .Lsec_128_8
        s32i     a3, a9, 96
        s32i     a3, a9, 100
        s32i     a3, a9, 104
        movi.n   a3, 64
        s8i      a3, a9, 11
        movi.n   a3, 1
        s32i     a4, a9, 112
        s8i      a3, a9, 130
        memw
        memw
        j        .Lsec_128_119
.Lsec_128_58:
        l32r     a3, .Lsec_128_c
        l32r     a8, .Lsec_128_10
        s32i     a3, a9, 96
        s32i     a3, a9, 100
        s32i     a3, a9, 104
        movi     a3, -128
        s32i     a8, a9, 112
        s8i      a3, a9, 11
        s8i      a4, a9, 130
        memw
        memw
        j        .Lsec_128_119
.Lsec_128_7c:
        bnei     a3, 1, .Lsec_128_c8
        beqz.n   a4, .Lsec_128_a4
        l32r     a4, .Lsec_128_4
        l32r     a9, .Lsec_128_8
        s32i     a4, a8, 96
        s32i     a4, a8, 100
        s32i     a4, a8, 104
        movi.n   a4, 64
        s32i     a9, a8, 112
        s8i      a4, a8, 11
        s8i      a3, a8, 130
        memw
        memw
        j        .Lsec_128_119
.Lsec_128_a4:
        l32r     a3, .Lsec_128_c
        l32r     a9, .Lsec_128_10
        s32i     a3, a8, 96
        s32i     a3, a8, 100
        s32i     a3, a8, 104
        movi     a3, -128
        s32i     a9, a8, 112
        s8i      a3, a8, 11
        s8i      a4, a8, 130
        memw
        memw
        j        .Lsec_128_119
.Lsec_128_c8:
        bnei     a3, 2, .Lsec_128_109
        beqz.n   a4, .Lsec_128_e8
        l32r     a3, .Lsec_128_4
        l32r     a4, .Lsec_128_8
        s32i     a3, a10, 96
        s32i     a3, a10, 100
        s32i     a3, a10, 104
        movi.n   a3, 64
        s32i     a4, a10, 112
        memw
        j        .Lsec_128_fd
        .byte    0x00
.Lsec_128_e8:
        l32r     a3, .Lsec_128_c
        l32r     a4, .Lsec_128_10
        s32i     a3, a10, 96
        s32i     a3, a10, 100
        s32i     a3, a10, 104
        movi     a3, -128
        s32i     a4, a10, 112
.Lsec_128_fd:
        s8i      a3, a10, 11
        memw
        memw
        j        .Lsec_128_119
.Lsec_128_109:
        l32r     a13, .Lsec_128_14
        movi.n   a12, 2
        movi     a11, 0x200
        movi.n   a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_128_18
        callx8   a8
.Lsec_128_119:
        retw.n
        .size   trc_update_def_rate, .-trc_update_def_rate
.Lsec_128_11b:

        .section .text.trc_update_ifx_phy_mode,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_129_0, rcP2P11GSchedTbl+0x54
        .literal .Lsec_129_4, rc11BSchedTbl+0x24
        .literal .Lsec_129_8, .Lsec_178_0
        .literal .Lsec_129_c, g_ic
        .literal .Lsec_129_10, .Lsec_147_0
        .literal .Lsec_129_14, rcLoRaSchedTbl+0xc
        .literal .Lsec_129_18, rcLoRaSchedTbl
        .literal .Lsec_129_1c, wifi_log
.Lsec_129_20:
        .global trc_update_ifx_phy_mode
        .type   trc_update_ifx_phy_mode, @function
trc_update_ifx_phy_mode:
        entry    a1, 32
        bnez.n   a2, .Lsec_129_3c
        l32r     a2, .Lsec_129_8
        l32i     a8, a2, 76
        l32r     a2, .Lsec_129_c
        addmi    a2, a2, 0x100
        l8ui     a10, a2, 216
        extui    a10, a10, 4, 1
        j        .Lsec_129_7a
        .byte    0x00
        .byte    0x00
.Lsec_129_3c:
        bnei     a2, 1, .Lsec_129_54
        l32r     a2, .Lsec_129_8
        l32i     a8, a2, 80
        l32r     a2, .Lsec_129_c
        addmi    a2, a2, 0x100
        l8ui     a10, a2, 216
        extui    a10, a10, 5, 1
        j        .Lsec_129_7a
.Lsec_129_54:
        bnei     a2, 2, .Lsec_129_64
        l32r     a2, .Lsec_129_8
        l32i     a8, a2, 84
        beqi     a3, 6, .Lsec_129_7d
        j        .Lsec_129_9a
        .byte    0x00
.Lsec_129_64:
        l32r     a13, .Lsec_129_10
        movi.n   a12, 1
        movi     a11, 0x200
        movi.n   a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_129_1c
        callx8   a8
        movi     a2, 0x102
        j        .Lsec_129_ab
.Lsec_129_7a:
        bnei     a3, 6, .Lsec_129_95
.Lsec_129_7d:
        l32r     a2, .Lsec_129_14
        l32r     a9, .Lsec_129_18
        s32i     a2, a8, 96
        s32i     a9, a8, 112
        s32i     a9, a8, 100
        s32i     a2, a8, 104
        memw
        j        .Lsec_129_a9
.Lsec_129_95:
        l32r     a9, .Lsec_129_0
        bnez.n   a10, .Lsec_129_9d
.Lsec_129_9a:
        l32r     a9, .Lsec_129_4
.Lsec_129_9d:
        s32i     a9, a8, 96
        s32i     a9, a8, 112
        s32i     a9, a8, 100
        s32i     a9, a8, 104
.Lsec_129_a9:
        movi.n   a2, 0
.Lsec_129_ab:
        memw
        retw.n
        .size   trc_update_ifx_phy_mode, .-trc_update_ifx_phy_mode
.Lsec_129_b0:

        .section .text.trc_deinit,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_130_0, .Lsec_178_0
        .literal .Lsec_130_4, g_osi_funcs_p
.Lsec_130_8:
        .global trc_deinit
        .type   trc_deinit, @function
trc_deinit:
        entry    a1, 32
        l32r     a2, .Lsec_130_0
        movi     a3, 0
        l32r     a4, .Lsec_130_4
.Lsec_130_14:
        l32i.n   a10, a2, 0
        beqz.n   a10, .Lsec_130_24
        l32i.n   a8, a4, 0
        l32i     a8, a8, 176
        callx8   a8
        movi.n   a8, 0
        s32i.n   a8, a2, 0
.Lsec_130_24:
        addi.n   a3, a3, 1
        movi.n   a8, 22
        addi.n   a2, a2, 4
        bne      a3, a8, .Lsec_130_14
        movi.n   a2, 0
        memw
        retw.n
        .size   trc_deinit, .-trc_deinit
.Lsec_130_34:

        .section .text.rc_set_per_conn_fix_rate,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_131_0, .Lsec_178_0
        .literal .Lsec_131_4, s_fix_rate_mask
        .literal .Lsec_131_8, s_fix_rate
.Lsec_131_c:
        .global rc_set_per_conn_fix_rate
        .type   rc_set_per_conn_fix_rate, @function
rc_set_per_conn_fix_rate:
        entry    a1, 32
        extui    a10, a2, 0, 8
        movi.n   a8, 21
        movi     a2, 0x102
        bltu     a8, a10, .Lsec_131_7d
        l32r     a8, .Lsec_131_0
        addx4    a8, a10, a8
        l32i.n   a9, a8, 0
        beqz     a9, .Lsec_131_7d
        movi.n   a11, 0
        addi     a12, a10, -19
        movi.n   a8, 1
        mov.n    a2, a11
        moveqz   a2, a8, a12
        mov.n    a12, a11
        moveqz   a12, a8, a10
        or       a2, a2, a12
        xor      a8, a8, a2
        l32r     a2, .Lsec_131_4
        l32i.n   a2, a2, 0
        ssr      a8
        srl      a2, a2
        extui    a2, a2, 0, 1
        l8ui     a10, a9, 11
        beq      a2, a11, .Lsec_131_72
        l32r     a2, .Lsec_131_8
        addx4    a8, a8, a2
        l8ui     a2, a8, 0
        s8i      a2, a9, 8
        s8i      a2, a9, 9
        movi.n   a2, 3
        or       a10, a10, a2
        mov.n    a2, a11
        s8i      a10, a9, 11
        memw
        memw
        j        .Lsec_131_7d
.Lsec_131_72:
        movi.n   a8, -4
        and      a10, a10, a8
        s8i      a10, a9, 11
        memw
.Lsec_131_7d:
        memw
        retw.n
        .size   rc_set_per_conn_fix_rate, .-rc_set_per_conn_fix_rate
.Lsec_131_82:

        .section .text.rc_enable_trc,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_132_0, .Lsec_178_0
        .literal .Lsec_132_4, g_osi_funcs_p
        .literal .Lsec_132_8, .Lsec_152_0
        .literal .Lsec_132_c, trc_ctl
        .literal .Lsec_132_10, .Lsec_151_0
        .literal .Lsec_132_14, wifi_log
        .literal .Lsec_132_18, memcmp
        .literal .Lsec_132_1c, wifi_log
        .literal .Lsec_132_20, memset
        .literal .Lsec_132_24, memcpy
        .literal .Lsec_132_28, rc_set_per_conn_fix_rate
.Lsec_132_2c:
        .global rc_enable_trc
        .type   rc_enable_trc, @function
rc_enable_trc:
        entry    a1, 32
        l32r     a7, .Lsec_132_0
        extui    a4, a4, 0, 8
        addx4    a7, a4, a7
        extui    a6, a2, 0, 8
        extui    a5, a5, 0, 8
        l32i     a2, a7, 0
        bnez.n   a2, .Lsec_132_6f
        movi     a10, 136
        l32r     a2, .Lsec_132_4
        l32i     a2, a2, 0
        l32i     a2, a2, 0x178
        callx8   a2
        mov.n    a2, a10
        s32i.n   a10, a7, 0
        bnez     a10, .Lsec_132_b5
        l32r     a13, .Lsec_132_8
        movi.n   a12, 1
        movi     a11, 0x200
        movi.n   a10, 6
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_132_14
        callx8   a8
        j        .Lsec_132_fa
.Lsec_132_6f:
        l32r     a8, .Lsec_132_c
        l32i.n   a7, a8, 20
        bbc      a7, a4, .Lsec_132_a8
        addx4    a8, a6, a8
        l32i.n   a7, a8, 4
        bbc      a7, a4, .Lsec_132_90
        movi     a12, 6
        or       a11, a3, a3
        addi     a10, a2, 31
        .reloc  ., R_XTENSA_ASM_EXPAND, memcmp
        l32r     a8, .Lsec_132_18
        callx8   a8
        beqz.n   a10, .Lsec_132_a8
.Lsec_132_90:
        l32r     a13, .Lsec_132_10
        movi.n   a11, 1
        movi.n   a12, 2
        slli     a11, a11, 11
        movi.n   a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_132_1c
        callx8   a8
        movi.n   a2, 0
        j        .Lsec_132_fa
        .byte    0x00
.Lsec_132_a8:
        movi     a12, 136
        movi.n   a11, 0
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, memset
        l32r     a8, .Lsec_132_20
        callx8   a8
.Lsec_132_b5:
        l32r     a8, .Lsec_132_c
        movi.n   a9, 1
        ssl      a4
        sll      a9, a9
        movi.n   a12, 6
        mov.n    a11, a3
        l32i.n   a7, a8, 20
        addi     a10, a2, 31
        or       a7, a7, a9
        s32i.n   a7, a8, 20
        addx4    a8, a6, a8
        l32i.n   a7, a8, 4
        or       a7, a7, a9
        s8i      a5, a2, 37
        movi.n   a5, 0
        s32i.n   a7, a8, 4
        s8i      a4, a2, 128
        s8i      a6, a2, 129
        s8i      a5, a2, 38
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_132_24
        callx8   a8
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, rc_set_per_conn_fix_rate
        l32r     a8, .Lsec_132_28
        callx8   a8
.Lsec_132_fa:
        retw.n
        .size   rc_enable_trc, .-rc_enable_trc
.Lsec_132_fc:

        .section .text.rc_get_fix_rate,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_133_0, s_fix_rate
        .literal .Lsec_133_4, s_fix_rate_mask
.Lsec_133_8:
        .global rc_get_fix_rate
        .type   rc_get_fix_rate, @function
rc_get_fix_rate:
        entry    a1, 32
        l32r     a8, .Lsec_133_0
        addx4    a8, a2, a8
        l32i.n   a8, a8, 0
        s32i.n   a8, a4, 0
        l32r     a8, .Lsec_133_4
        l32i.n   a8, a8, 0
        ssr      a2
        srl      a8, a8
        extui    a8, a8, 0, 1
        movi.n   a2, 0
        s8i      a8, a3, 0
        memw
        memw
        retw.n
        .size   rc_get_fix_rate, .-rc_get_fix_rate
.Lsec_133_30:

        .section .text.rc_set_fix_rate,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_134_0, s_fix_rate_mask
        .literal .Lsec_134_4, s_fix_rate
        .literal .Lsec_134_8, rc_set_per_conn_fix_rate
        .literal .Lsec_134_c, rc_set_per_conn_fix_rate
        .literal .Lsec_134_10, rc_set_per_conn_fix_rate
        .literal .Lsec_134_14, rc_set_per_conn_fix_rate
        .literal .Lsec_134_18, rc_set_per_conn_fix_rate
.Lsec_134_1c:
        .global rc_set_fix_rate
        .type   rc_set_fix_rate, @function
rc_set_fix_rate:
        entry    a1, 32
        l32r     a11, .Lsec_134_0
        movi.n   a8, 1
        extui    a3, a3, 0, 8
        mov.n    a10, a2
        ssl      a2
        sll      a8, a8
        l32i.n   a2, a11, 0
        beqz.n   a3, .Lsec_134_39
        or       a2, a8, a2
        j        .Lsec_134_41
.Lsec_134_39:
        movi.n   a9, -1
        xor      a8, a9, a8
        and      a2, a8, a2
.Lsec_134_41:
        s32i.n   a2, a11, 0
        l32r     a2, .Lsec_134_4
        addx4    a2, a10, a2
        s32i.n   a4, a2, 0
        bnez.n   a10, .Lsec_134_5d
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, rc_set_per_conn_fix_rate
        l32r     a8, .Lsec_134_8
        callx8   a8
        movi.n   a10, 19
        j        .Lsec_134_8e
        .byte    0x00
        .byte    0x00
.Lsec_134_5d:
        bnei     a10, 1, .Lsec_134_79
        or       a2, a10, a10
        movi     a3, 17
.Lsec_134_66:
        mov.n    a10, a2
        addi.n   a2, a2, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, rc_set_per_conn_fix_rate
        l32r     a8, .Lsec_134_c
        callx8   a8
        bne      a2, a3, .Lsec_134_66
        movi.n   a10, 20
        j        .Lsec_134_8e
        .byte    0x00
.Lsec_134_79:
        bnei     a10, 2, .Lsec_134_94
        movi.n   a10, 17
        .reloc  ., R_XTENSA_ASM_EXPAND, rc_set_per_conn_fix_rate
        l32r     a8, .Lsec_134_10
        callx8   a8
        movi.n   a10, 18
        .reloc  ., R_XTENSA_ASM_EXPAND, rc_set_per_conn_fix_rate
        l32r     a8, .Lsec_134_14
        callx8   a8
        movi.n   a10, 21
.Lsec_134_8e:
        .reloc  ., R_XTENSA_ASM_EXPAND, rc_set_per_conn_fix_rate
        l32r     a8, .Lsec_134_18
        callx8   a8
.Lsec_134_94:
        movi.n   a2, 0
        retw.n
        .size   rc_set_fix_rate, .-rc_set_fix_rate
.Lsec_134_98:

        .section .text.trc_get_espnow_rate,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_135_0, .Lsec_177_0
.Lsec_135_4:
        .global trc_get_espnow_rate
        .type   trc_get_espnow_rate, @function
trc_get_espnow_rate:
        entry    a1, 32
        l32r     a8, .Lsec_135_0
        extui    a2, a2, 0, 8
        addx4    a2, a2, a8
        l32i.n   a2, a2, 0
        retw.n
        .size   trc_get_espnow_rate, .-trc_get_espnow_rate
.Lsec_135_14:

        .section .text.trc_set_espnow_rate,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_136_0, .Lsec_177_0
.Lsec_136_4:
        .global trc_set_espnow_rate
        .type   trc_set_espnow_rate, @function
trc_set_espnow_rate:
        entry    a1, 32
        l32r     a8, .Lsec_136_0
        extui    a2, a2, 0, 8
        addx4    a2, a2, a8
        s32i.n   a3, a2, 0
        memw
        retw.n
        .size   trc_set_espnow_rate, .-trc_set_espnow_rate
.Lsec_136_17:

        .section .text.trc_get_80211_tx_rate,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_137_0, .Lsec_176_0
.Lsec_137_4:
        .global trc_get_80211_tx_rate
        .type   trc_get_80211_tx_rate, @function
trc_get_80211_tx_rate:
        entry    a1, 32
        l32r     a8, .Lsec_137_0
        extui    a2, a2, 0, 8
        addx4    a2, a2, a8
        l32i.n   a2, a2, 0
        retw.n
        .size   trc_get_80211_tx_rate, .-trc_get_80211_tx_rate
.Lsec_137_14:

        .section .text.trc_get_80211_tx_rate_config,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_138_0, .Lsec_175_0
        .literal .Lsec_138_4, memcpy
.Lsec_138_8:
        .global trc_get_80211_tx_rate_config
        .type   trc_get_80211_tx_rate_config, @function
trc_get_80211_tx_rate_config:
        entry    a1, 32
        l32r     a11, .Lsec_138_0
        extui    a2, a2, 0, 8
        addx2    a2, a2, a2
        movi.n   a12, 12
        addx4    a11, a2, a11
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_138_4
        callx8   a8
        retw.n
        .size   trc_get_80211_tx_rate_config, .-trc_get_80211_tx_rate_config
.Lsec_138_23:

        .section .text.trc_set_80211_tx_rate,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_139_0, .Lsec_176_0
.Lsec_139_4:
        .global trc_set_80211_tx_rate
        .type   trc_set_80211_tx_rate, @function
trc_set_80211_tx_rate:
        entry    a1, 32
        l32r     a8, .Lsec_139_0
        extui    a2, a2, 0, 8
        addx4    a2, a2, a8
        s32i.n   a3, a2, 0
        memw
        retw.n
        .size   trc_set_80211_tx_rate, .-trc_set_80211_tx_rate
.Lsec_139_17:

        .section .text.trc_set_80211_tx_rate_config,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_140_0, .Lsec_175_0
        .literal .Lsec_140_4, memcpy
.Lsec_140_8:
        .global trc_set_80211_tx_rate_config
        .type   trc_set_80211_tx_rate_config, @function
trc_set_80211_tx_rate_config:
        entry    a1, 32
        l32r     a10, .Lsec_140_0
        extui    a2, a2, 0, 8
        addx2    a2, a2, a2
        movi.n   a12, 12
        mov.n    a11, a3
        addx4    a10, a2, a10
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_140_4
        callx8   a8
        retw.n
        .size   trc_set_80211_tx_rate_config, .-trc_set_80211_tx_rate_config
.Lsec_140_23:

        .section .rodata.CSWTCH$98,"a",@progbits
        .align  2
.Lsec_141_0:
        .byte   0x5a, 0x19, 0x80, 0x25, 0x00, 0x4b, 0x00, 0x64, 0xc0, 0xa8, 0x50, 0xc3, 0x00, 0xe1, 0xff, 0xff
        .byte   0x5a, 0x19, 0x80, 0x25, 0x00, 0x4b, 0x00, 0x64, 0xc0, 0xa8, 0x50, 0xc3, 0x00, 0xe1, 0xff, 0xff

        .section .rodata.CSWTCH$90,"a",@progbits
        .align  1
.Lsec_142_0:
        .byte   0x03, 0x02, 0x01, 0x00, 0xff, 0x02, 0x01, 0x00, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff
        .byte   0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff
        .byte   0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x05, 0x04

        .section .rodata.CSWTCH$88,"a",@progbits
        .align  1
.Lsec_143_0:
        .byte   0x0a, 0x09, 0x08, 0xff, 0xff, 0x09, 0x08, 0xff, 0x01, 0x03, 0x05, 0x07, 0x00, 0x02, 0x04, 0x06
        .byte   0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff
        .byte   0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x0c, 0x0b

        .section .rodata.__FUNCTION__$0,"a",@progbits
        .align  1
.Lsec_144_0:
        .byte   0x72, 0x63, 0x52, 0x65, 0x61, 0x63, 0x68, 0x52, 0x65, 0x74, 0x72, 0x79, 0x4c, 0x69, 0x6d, 0x69
        .byte   0x74, 0x00

        .section .rodata.__FUNCTION__$1,"a",@progbits
        .align  1
.Lsec_145_0:
        .byte   0x72, 0x63, 0x47, 0x65, 0x74, 0x52, 0x61, 0x74, 0x65, 0x00

        .section .rodata.__FUNCTION__$2,"a",@progbits
        .align  1
.Lsec_146_0:
        .byte   0x72, 0x63, 0x47, 0x65, 0x74, 0x41, 0x6d, 0x70, 0x64, 0x75, 0x53, 0x63, 0x68, 0x65, 0x64, 0x00

        .section .rodata_wlog_error.27,"a",@progbits
        .align  1
.Lsec_147_0:
        .byte   0x75, 0x70, 0x64, 0x61, 0x74, 0x65, 0x20, 0x64, 0x65, 0x66, 0x61, 0x75, 0x6c, 0x74, 0x20, 0x74
        .byte   0x72, 0x63, 0x3a, 0x20, 0x69, 0x6e, 0x76, 0x61, 0x6c, 0x69, 0x64, 0x20, 0x69, 0x66, 0x78, 0x00

        .section .rodata_wlog_warning.26,"a",@progbits
        .align  1
.Lsec_148_0:
        .byte   0x69, 0x6e, 0x76, 0x61, 0x6c, 0x69, 0x64, 0x20, 0x69, 0x6e, 0x74, 0x65, 0x72, 0x66, 0x61, 0x63
        .byte   0x65, 0x21, 0x00

        .section .rodata_wlog_error.25,"a",@progbits
        .align  1
.Lsec_149_0:
        .byte   0x74, 0x72, 0x63, 0x20, 0x69, 0x6e, 0x69, 0x74, 0x20, 0x66, 0x61, 0x69, 0x6c, 0x2c, 0x20, 0x6e
        .byte   0x6f, 0x20, 0x6d, 0x65, 0x6d, 0x00

        .section .rodata_wlog_warning.23,"a",@progbits
        .align  1
.Lsec_150_0:
        .byte   0x72, 0x6d, 0x20, 0x6d, 0x69, 0x73, 0x00

        .section .rodata_wlog_warning.22,"a",@progbits
        .align  1
.Lsec_151_0:
        .byte   0x61, 0x64, 0x64, 0x20, 0x6d, 0x69, 0x73, 0x6d, 0x61, 0x74, 0x63, 0x68, 0x20, 0x00

        .section .rodata_wlog_error.21,"a",@progbits
        .align  1
.Lsec_152_0:
        .byte   0x72, 0x63, 0x5f, 0x65, 0x6e, 0x61, 0x62, 0x6c, 0x65, 0x5f, 0x74, 0x72, 0x63, 0x20, 0x66, 0x61
        .byte   0x69, 0x6c, 0x2c, 0x20, 0x6e, 0x6f, 0x20, 0x6d, 0x65, 0x6d, 0x00

        .section .rodata_wlog_error.17,"a",@progbits
        .align  1
.Lsec_153_0:
        .byte   0x25, 0x73, 0x20, 0x25, 0x73, 0x20, 0x25, 0x75, 0x0a, 0x00

        .section .rodata_wlog_error.16,"a",@progbits
        .align  1
.Lsec_154_0:
        .byte   0x73, 0x63, 0x68, 0x65, 0x64, 0x20, 0x69, 0x73, 0x20, 0x6e, 0x75, 0x6c, 0x6c, 0x2c, 0x20, 0x69
        .byte   0x66, 0x3d, 0x25, 0x64, 0x20, 0x74, 0x69, 0x64, 0x3d, 0x25, 0x64, 0x20, 0x66, 0x6c, 0x61, 0x67
        .byte   0x73, 0x3d, 0x25, 0x78, 0x00

        .section .rodata_wlog_error.15,"a",@progbits
        .align  1
.Lsec_155_0:
        .byte   0x25, 0x73, 0x20, 0x25, 0x73, 0x20, 0x25, 0x75, 0x0a, 0x00

        .section .rodata_wlog_error.14,"a",@progbits
        .align  1
.Lsec_156_0:
        .byte   0x64, 0x65, 0x73, 0x63, 0x20, 0x69, 0x73, 0x20, 0x6e, 0x75, 0x6c, 0x6c, 0x00

        .section .rodata_wlog_error.13,"a",@progbits
        .align  1
.Lsec_157_0:
        .byte   0x25, 0x73, 0x20, 0x25, 0x73, 0x20, 0x25, 0x75, 0x0a, 0x00

        .section .rodata_wlog_error.11,"a",@progbits
        .align  1
.Lsec_158_0:
        .byte   0x25, 0x73, 0x20, 0x25, 0x73, 0x20, 0x25, 0x75, 0x0a, 0x00

        .section .rodata_wlog_warning.6,"a",@progbits
        .align  1
.Lsec_159_0:
        .byte   0x72, 0x63, 0x55, 0x70, 0x64, 0x61, 0x74, 0x65, 0x50, 0x68, 0x79, 0x4d, 0x6f, 0x64, 0x65, 0x3a
        .byte   0x20, 0x6c, 0x6f, 0x77, 0x65, 0x73, 0x74, 0x52, 0x61, 0x74, 0x65, 0x49, 0x64, 0x78, 0x3a, 0x25
        .byte   0x64, 0x2c, 0x20, 0x72, 0x61, 0x74, 0x65, 0x49, 0x64, 0x78, 0x3a, 0x25, 0x64, 0x00

        .section .rodata_wlog_warning.5,"a",@progbits
        .align  1
.Lsec_160_0:
        .byte   0x72, 0x63, 0x55, 0x70, 0x64, 0x61, 0x74, 0x65, 0x50, 0x68, 0x79, 0x4d, 0x6f, 0x64, 0x65, 0x3a
        .byte   0x20, 0x70, 0x68, 0x79, 0x74, 0x79, 0x70, 0x65, 0x3a, 0x25, 0x64, 0x2c, 0x20, 0x72, 0x73, 0x5f
        .byte   0x6e, 0x72, 0x61, 0x74, 0x65, 0x3a, 0x25, 0x64, 0x2c, 0x20, 0x72, 0x73, 0x73, 0x69, 0x3a, 0x25
        .byte   0x64, 0x00

        .section .rodata_wlog_error.4,"a",@progbits
        .align  1
.Lsec_161_0:
        .byte   0x72, 0x63, 0x47, 0x65, 0x74, 0x31, 0x31, 0x42, 0x48, 0x69, 0x67, 0x68, 0x65, 0x73, 0x74, 0x52
        .byte   0x61, 0x74, 0x65, 0x49, 0x64, 0x78, 0x3a, 0x20, 0x69, 0x6e, 0x76, 0x61, 0x6c, 0x69, 0x64, 0x20
        .byte   0x6d, 0x61, 0x78, 0x20, 0x72, 0x61, 0x74, 0x65, 0x3d, 0x25, 0x64, 0x00

        .section .rodata_wlog_error.3,"a",@progbits
        .align  1
.Lsec_162_0:
        .byte   0x72, 0x63, 0x47, 0x65, 0x74, 0x31, 0x31, 0x47, 0x48, 0x69, 0x67, 0x68, 0x65, 0x73, 0x74, 0x52
        .byte   0x61, 0x74, 0x65, 0x49, 0x64, 0x78, 0x3a, 0x20, 0x69, 0x6e, 0x76, 0x61, 0x6c, 0x69, 0x64, 0x20
        .byte   0x6d, 0x61, 0x78, 0x20, 0x72, 0x61, 0x74, 0x65, 0x3d, 0x25, 0x64, 0x00

        .section .rodata_wlog_error.2,"a",@progbits
        .align  1
.Lsec_163_0:
        .byte   0x72, 0x63, 0x47, 0x65, 0x74, 0x31, 0x31, 0x4e, 0x48, 0x69, 0x67, 0x68, 0x65, 0x73, 0x74, 0x52
        .byte   0x61, 0x74, 0x65, 0x49, 0x64, 0x78, 0x3a, 0x20, 0x69, 0x6e, 0x76, 0x61, 0x6c, 0x69, 0x64, 0x20
        .byte   0x6d, 0x61, 0x78, 0x20, 0x72, 0x61, 0x74, 0x65, 0x3d, 0x25, 0x64, 0x00

        .section .data.rc11BSchedTbl,"aw",@progbits
        .align  1
.Lsec_164_0:
        .global rc11BSchedTbl
        .type   rc11BSchedTbl, @object
        .size   rc11BSchedTbl, 72
rc11BSchedTbl:
        .byte   0x07, 0x02, 0x06, 0x02, 0x06, 0x03, 0x06, 0x19, 0x20, 0x40, 0x00, 0x00, 0x06, 0x02, 0x06, 0x02
        .byte   0x06, 0x03, 0x06, 0x19, 0x20, 0x39, 0x00, 0x00, 0x05, 0x02, 0x00, 0x02, 0x00, 0x03, 0x00, 0x19
        .byte   0x20, 0x39, 0x00, 0x00, 0x00, 0x02, 0x00, 0x02, 0x00, 0x03, 0x00, 0x19, 0x20, 0x1e, 0x00, 0x00
        .byte   0x2a, 0x08, 0x2a, 0x08, 0x2a, 0x02, 0x29, 0x0e, 0x20, 0x14, 0x00, 0x00, 0x29, 0x02, 0x29, 0x02
        .byte   0x29, 0x03, 0x29, 0x19, 0x20, 0x00, 0x00, 0x00

        .section .data.rcP2P11GSchedTbl,"aw",@progbits
        .align  1
.Lsec_165_0:
        .global rcP2P11GSchedTbl
        .type   rcP2P11GSchedTbl, @object
        .size   rcP2P11GSchedTbl, 96
rcP2P11GSchedTbl:
        .byte   0x0c, 0x02, 0x08, 0x02, 0x0d, 0x03, 0x0b, 0x19, 0x20, 0x72, 0x00, 0x00, 0x08, 0x02, 0x0d, 0x02
        .byte   0x09, 0x03, 0x0b, 0x19, 0x20, 0x65, 0x00, 0x00, 0x0d, 0x02, 0x09, 0x02, 0x0e, 0x03, 0x0b, 0x19
        .byte   0x20, 0x5c, 0x00, 0x00, 0x09, 0x02, 0x0e, 0x02, 0x0a, 0x03, 0x0b, 0x19, 0x20, 0x58, 0x00, 0x00
        .byte   0x0e, 0x02, 0x0a, 0x02, 0x0f, 0x03, 0x0b, 0x19, 0x20, 0x52, 0x00, 0x00, 0x0a, 0x02, 0x0f, 0x02
        .byte   0x0b, 0x03, 0x0b, 0x19, 0x20, 0x4b, 0x00, 0x00, 0x0f, 0x02, 0x0b, 0x02, 0x0b, 0x03, 0x0b, 0x19
        .byte   0x20, 0x38, 0x00, 0x00, 0x0b, 0x02, 0x0b, 0x02, 0x0b, 0x03, 0x0b, 0x19, 0x20, 0x40, 0x00, 0x00

        .section .data.rc11GSchedTbl,"aw",@progbits
        .align  1
.Lsec_166_0:
        .global rc11GSchedTbl
        .type   rc11GSchedTbl, @object
        .size   rc11GSchedTbl, 156
rc11GSchedTbl:
        .byte   0x0c, 0x02, 0x08, 0x02, 0x0b, 0x03, 0x06, 0x19, 0x20, 0x72, 0x00, 0x00, 0x08, 0x02, 0x0d, 0x02
        .byte   0x0b, 0x03, 0x06, 0x19, 0x20, 0x65, 0x00, 0x00, 0x0d, 0x02, 0x09, 0x02, 0x0b, 0x03, 0x06, 0x19
        .byte   0x20, 0x5c, 0x00, 0x00, 0x09, 0x02, 0x0e, 0x02, 0x0b, 0x03, 0x06, 0x19, 0x20, 0x58, 0x00, 0x00
        .byte   0x0e, 0x02, 0x0a, 0x02, 0x0b, 0x03, 0x06, 0x19, 0x20, 0x52, 0x00, 0x00, 0x0a, 0x02, 0x0b, 0x02
        .byte   0x06, 0x03, 0x06, 0x19, 0x20, 0x4b, 0x00, 0x00, 0x0f, 0x02, 0x0b, 0x02, 0x06, 0x03, 0x06, 0x19
        .byte   0x20, 0x38, 0x00, 0x00, 0x0b, 0x02, 0x06, 0x02, 0x06, 0x03, 0x06, 0x19, 0x20, 0x40, 0x00, 0x00
        .byte   0x06, 0x02, 0x06, 0x02, 0x06, 0x03, 0x06, 0x19, 0x20, 0x39, 0x00, 0x00, 0x05, 0x02, 0x00, 0x02
        .byte   0x00, 0x03, 0x00, 0x19, 0x20, 0x39, 0x00, 0x00, 0x00, 0x02, 0x00, 0x02, 0x00, 0x03, 0x00, 0x19
        .byte   0x20, 0x1e, 0x00, 0x00, 0x2a, 0x08, 0x2a, 0x08, 0x2a, 0x02, 0x29, 0x0e, 0x20, 0x14, 0x00, 0x00
        .byte   0x29, 0x02, 0x29, 0x02, 0x29, 0x03, 0x29, 0x19, 0x20, 0x00, 0x00, 0x00

        .section .data.rcP2P11NSchedTbl,"aw",@progbits
        .align  1
.Lsec_167_0:
        .global rcP2P11NSchedTbl
        .type   rcP2P11NSchedTbl, @object
        .size   rcP2P11NSchedTbl, 120
rcP2P11NSchedTbl:
        .byte   0x1f, 0x02, 0x16, 0x02, 0x15, 0x03, 0x0b, 0x19, 0x20, 0x72, 0x00, 0x00, 0x17, 0x02, 0x15, 0x02
        .byte   0x14, 0x03, 0x0b, 0x19, 0x20, 0x72, 0x00, 0x00, 0x16, 0x02, 0x14, 0x02, 0x13, 0x03, 0x0b, 0x19
        .byte   0x20, 0x72, 0x00, 0x00, 0x15, 0x02, 0x13, 0x02, 0x12, 0x03, 0x0b, 0x19, 0x20, 0x60, 0x00, 0x00
        .byte   0x14, 0x02, 0x12, 0x02, 0x11, 0x03, 0x0b, 0x19, 0x20, 0x55, 0x00, 0x00, 0x13, 0x02, 0x12, 0x02
        .byte   0x11, 0x03, 0x0b, 0x19, 0x20, 0x4b, 0x00, 0x00, 0x12, 0x02, 0x11, 0x02, 0x10, 0x03, 0x0b, 0x19
        .byte   0x20, 0x40, 0x00, 0x00, 0x11, 0x02, 0x10, 0x02, 0x0b, 0x03, 0x0b, 0x19, 0x20, 0x3b, 0x00, 0x00
        .byte   0x10, 0x02, 0x0b, 0x02, 0x0b, 0x03, 0x0b, 0x19, 0x20, 0x32, 0x00, 0x00, 0x0b, 0x02, 0x0b, 0x02
        .byte   0x0b, 0x03, 0x0b, 0x19, 0x20, 0x40, 0x00, 0x00

        .section .data.rc11NSchedTbl,"aw",@progbits
        .align  1
.Lsec_168_0:
        .global rc11NSchedTbl
        .type   rc11NSchedTbl, @object
        .size   rc11NSchedTbl, 168
rc11NSchedTbl:
        .byte   0x1f, 0x02, 0x16, 0x02, 0x0b, 0x03, 0x06, 0x19, 0x20, 0x72, 0x00, 0x00, 0x17, 0x02, 0x15, 0x02
        .byte   0x0b, 0x03, 0x06, 0x19, 0x20, 0x72, 0x00, 0x00, 0x16, 0x02, 0x14, 0x02, 0x0b, 0x03, 0x06, 0x19
        .byte   0x20, 0x72, 0x00, 0x00, 0x15, 0x02, 0x13, 0x02, 0x0b, 0x03, 0x06, 0x19, 0x20, 0x60, 0x00, 0x00
        .byte   0x14, 0x02, 0x12, 0x02, 0x0b, 0x03, 0x06, 0x19, 0x20, 0x55, 0x00, 0x00, 0x13, 0x02, 0x12, 0x02
        .byte   0x0b, 0x03, 0x06, 0x19, 0x20, 0x4b, 0x00, 0x00, 0x12, 0x02, 0x11, 0x02, 0x0b, 0x03, 0x06, 0x19
        .byte   0x20, 0x40, 0x00, 0x00, 0x11, 0x02, 0x0b, 0x02, 0x06, 0x03, 0x06, 0x19, 0x20, 0x3b, 0x00, 0x00
        .byte   0x10, 0x02, 0x0b, 0x02, 0x06, 0x03, 0x06, 0x19, 0x20, 0x32, 0x00, 0x00, 0x06, 0x02, 0x06, 0x02
        .byte   0x06, 0x03, 0x06, 0x19, 0x20, 0x39, 0x00, 0x00, 0x05, 0x02, 0x00, 0x02, 0x00, 0x03, 0x00, 0x19
        .byte   0x20, 0x39, 0x00, 0x00, 0x00, 0x02, 0x00, 0x02, 0x00, 0x03, 0x00, 0x19, 0x20, 0x1e, 0x00, 0x00
        .byte   0x2a, 0x08, 0x2a, 0x08, 0x2a, 0x02, 0x29, 0x0e, 0x20, 0x14, 0x00, 0x00, 0x29, 0x02, 0x29, 0x02
        .byte   0x29, 0x03, 0x29, 0x19, 0x20, 0x00, 0x00, 0x00

        .section .data.rcLoRaSchedTbl,"aw",@progbits
        .align  1
.Lsec_169_0:
        .global rcLoRaSchedTbl
        .type   rcLoRaSchedTbl, @object
        .size   rcLoRaSchedTbl, 24
rcLoRaSchedTbl:
        .byte   0x2a, 0x08, 0x2a, 0x08, 0x2a, 0x02, 0x29, 0x0e, 0x20, 0x14, 0x00, 0x00, 0x29, 0x02, 0x29, 0x02
        .byte   0x29, 0x03, 0x29, 0x19, 0x20, 0x00, 0x00, 0x00

        .section .data.BAROFDMSched,"aw",@progbits
        .align  1
.Lsec_170_0:
        .global BAROFDMSched
        .type   BAROFDMSched, @object
        .size   BAROFDMSched, 12
BAROFDMSched:
        .byte   0x0b, 0x02, 0x0b, 0x02, 0x0b, 0x03, 0x0b, 0x19, 0x20, 0x00, 0x00, 0x00

        .section .data.BasicOFDMSched,"aw",@progbits
        .align  1
.Lsec_171_0:
        .global BasicOFDMSched
        .type   BasicOFDMSched, @object
        .size   BasicOFDMSched, 12
BasicOFDMSched:
        .byte   0x0b, 0x02, 0x0b, 0x02, 0x0b, 0x03, 0x00, 0x19, 0x20, 0x00, 0x00, 0x00

        .section .data.trc_ctl,"aw",@progbits
        .align  4
.Lsec_172_0:
        .global trc_ctl
        .type   trc_ctl, @object
        .size   trc_ctl, 28
trc_ctl:
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05, 0x05, 0x00, 0x00

        .section .bss.s_fix_rate,"aw",@nobits
        .align  4
.Lsec_173_0:
        .global s_fix_rate
        .type   s_fix_rate, @object
        .size   s_fix_rate, 12
s_fix_rate:
        .skip   12

        .section .bss.s_fix_rate_mask,"aw",@nobits
        .align  4
.Lsec_174_0:
        .global s_fix_rate_mask
        .type   s_fix_rate_mask, @object
        .size   s_fix_rate_mask, 4
s_fix_rate_mask:
        .skip   4

        .section .bss.g_80211_tx_rate_config,"aw",@nobits
        .align  4
.Lsec_175_0:
        .skip   36

        .section .bss.g_80211_tx_rate,"aw",@nobits
        .align  4
.Lsec_176_0:
        .skip   12

        .section .bss.g_espnow_rate,"aw",@nobits
        .align  4
.Lsec_177_0:
        .skip   12

        .section .bss.g_per_conn_trc,"aw",@nobits
        .align  4
.Lsec_178_0:
        .skip   88
