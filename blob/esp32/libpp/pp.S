.section .text.pp_delete_task_manually,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_95_0, .Lsec_254_0
        .literal .Lsec_95_4, g_osi_funcs_p
        .literal .Lsec_95_8, s_wifi_queue
.Lsec_95_c:
        entry    a1, 32
        l32r     a2, .Lsec_95_0
        l32i.n   a10, a2, 0
        beqz.n   a10, .Lsec_95_25
        l32r     a8, .Lsec_95_4
        l32i.n   a8, a8, 0
        l32i     a8, a8, 152
        callx8   a8
        movi.n   a8, 0
        s32i.n   a8, a2, 0
.Lsec_95_25:
        l32r     a2, .Lsec_95_8
        l32i.n   a10, a2, 0
        beqz.n   a10, .Lsec_95_3e
        l32r     a8, .Lsec_95_4
        l32i.n   a8, a8, 0
        l32i     a8, a8, 0x180
        memw
        callx8   a8
        movi.n   a8, 0
        s32i.n   a8, a2, 0
.Lsec_95_3e:
        movi.n   a2, 0
        memw
        retw.n

        .section .wifislpiram.5,"ax",@progbits
        .align  4
.Lsec_96_0:
        .global pp_hdrsize
        .type   pp_hdrsize, @function
pp_hdrsize:
        entry    a1, 32
        l8ui     a9, a2, 1
        mov.n    a10, a2
        extui    a9, a9, 0, 2
        addi     a9, a9, -3
        movi.n   a8, 24
        movi.n   a2, 30
        movnez   a2, a8, a9
        l8ui     a8, a10, 0
        movi     a9, 140
        and      a8, a8, a9
        movi     a9, 136
        bne      a8, a9, .Lsec_96_26
        addi.n   a2, a2, 2
.Lsec_96_26:
        retw.n
        .size   pp_hdrsize, .-pp_hdrsize
.Lsec_96_28:

        .section .text.getle32,"ax",@progbits
        .align  4
.Lsec_97_0:
        .global getle32
        .type   getle32, @function
getle32:
        entry    a1, 32
        l8ui     a9, a2, 1
        slli     a9, a9, 8
        l8ui     a8, a2, 0
        or       a9, a9, a8
        l8ui     a8, a2, 2
        slli     a8, a8, 16
        or       a8, a8, a9
        l8ui     a2, a2, 3
        slli     a2, a2, 24
        or       a2, a2, a8
        retw.n
        .size   getle32, .-getle32
.Lsec_97_23:

        .section .text.putle32,"ax",@progbits
        .align  4
.Lsec_98_0:
        .global putle32
        .type   putle32, @function
putle32:
        entry    a1, 32
        srli     a8, a3, 8
        s8i      a3, a2, 0
        s8i      a8, a2, 1
        extui    a8, a3, 16, 16
        extui    a3, a3, 24, 8
        s8i      a8, a2, 2
        s8i      a3, a2, 3
        memw
        memw
        retw.n
        .size   putle32, .-putle32
.Lsec_98_20:

        .section .text.rotl,"ax",@progbits
        .align  4
.Lsec_99_0:
        .global rotl
        .type   rotl, @function
rotl:
        entry    a1, 32
        ssl      a3
        src      a2, a2, a2
        retw.n
        .size   rotl, .-rotl
.Lsec_99_b:

        .section .text.rotr,"ax",@progbits
        .align  4
.Lsec_100_0:
        .global rotr
        .type   rotr, @function
rotr:
        entry    a1, 32
        ssr      a3
        src      a2, a2, a2
        retw.n
        .size   rotr, .-rotr
.Lsec_100_b:

        .section .text.xswap,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_101_0, 0xff00ff00
        .literal .Lsec_101_4, 0x00ff00ff
.Lsec_101_8:
        .global xswap
        .type   xswap, @function
xswap:
        entry    a1, 32
        l32r     a9, .Lsec_101_0
        slli     a8, a2, 8
        and      a8, a8, a9
        srli     a2, a2, 8
        l32r     a9, .Lsec_101_4
        and      a2, a2, a9
        or       a2, a8, a2
        retw.n
        .size   xswap, .-xswap
.Lsec_101_22:

        .section .text.pp_set_cut_rx,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_102_0, pp_need_cut_rx
.Lsec_102_4:
        .global pp_set_cut_rx
        .type   pp_set_cut_rx, @function
pp_set_cut_rx:
        entry    a1, 32
        l32r     a8, .Lsec_102_0
        s8i      a2, a8, 0
        memw
        memw
        retw.n
        .size   pp_set_cut_rx, .-pp_set_cut_rx
.Lsec_102_15:

        .section .text.pp_set_cut_evt,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_103_0, pp_allow_cut_sevt
.Lsec_103_4:
        .global pp_set_cut_evt
        .type   pp_set_cut_evt, @function
pp_set_cut_evt:
        entry    a1, 32
        l32r     a8, .Lsec_103_0
        s8i      a2, a8, 0
        memw
        memw
        retw.n
        .size   pp_set_cut_evt, .-pp_set_cut_evt
.Lsec_103_15:

        .section .text.pp_can_cut_sevt,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_104_0, pp_allow_cut_sevt
.Lsec_104_4:
        .global pp_can_cut_sevt
        .type   pp_can_cut_sevt, @function
pp_can_cut_sevt:
        entry    a1, 32
        l32r     a8, .Lsec_104_0
        l8ui     a2, a8, 0
        retw.n
        .size   pp_can_cut_sevt, .-pp_can_cut_sevt
.Lsec_104_f:

        .section .text.pp_can_cut_evt,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_105_0, pp_need_cut_rx
        .literal .Lsec_105_4, pp_allow_cut_sevt
.Lsec_105_8:
        .global pp_can_cut_evt
        .type   pp_can_cut_evt, @function
pp_can_cut_evt:
        entry    a1, 32
        l32r     a2, .Lsec_105_0
        l8ui     a8, a2, 0
        movi.n   a2, 0
        beq      a8, a2, .Lsec_105_1c
        l32r     a2, .Lsec_105_4
        l8ui     a2, a2, 0
.Lsec_105_1c:
        extui    a2, a2, 0, 1
        retw.n
        .size   pp_can_cut_evt, .-pp_can_cut_evt
.Lsec_105_21:

        .section .text.RxNodeNum,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_106_0, wDevCtrl
.Lsec_106_4:
        .global RxNodeNum
        .type   RxNodeNum, @function
RxNodeNum:
        entry    a1, 32
        l32r     a2, .Lsec_106_0
        l32i.n   a8, a2, 0
        movi.n   a2, 0
        mov.n    a9, a2
        j        .Lsec_106_26
        .byte    0x00
        .byte    0x00
.Lsec_106_15:
        memw
        l32i.n   a10, a8, 0
        addi.n   a2, a2, 1
        extui    a2, a2, 0, 8
        bbci     a10, 30, .Lsec_106_24
        mov.n    a2, a9
.Lsec_106_24:
        l32i.n   a8, a8, 8
.Lsec_106_26:
        bnez     a8, .Lsec_106_15
        retw.n
        .size   RxNodeNum, .-RxNodeNum
.Lsec_106_2b:

        .section .text.TxNodeNum,"ax",@progbits
        .align  4
.Lsec_107_0:
        .global TxNodeNum
        .type   TxNodeNum, @function
TxNodeNum:
        entry    a1, 32
        movi.n   a2, 0
        retw.n
        .size   TxNodeNum, .-TxNodeNum
.Lsec_107_7:

        .section .text.SigSpaceMalloc,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_108_0, pTxRx
        .literal .Lsec_108_4, CurFreeSigIdx
        .literal .Lsec_108_8, SigInMacISR
.Lsec_108_c:
        .global SigSpaceMalloc
        .type   SigSpaceMalloc, @function
SigSpaceMalloc:
        entry    a1, 32
        l32r     a2, .Lsec_108_0
        movi.n   a10, 50
        l32i.n   a2, a2, 0
        l32i     a8, a2, 0x3c0
        addi.n   a9, a8, 1
        s32i     a9, a2, 0x3c0
        bgeu     a10, a9, .Lsec_108_2c
        s32i     a8, a2, 0x3c0
        movi.n   a2, 0
        memw
        j        .Lsec_108_4d
.Lsec_108_2c:
        l32r     a9, .Lsec_108_4
        movi.n   a10, 49
        l16ui    a8, a9, 0
        l32r     a2, .Lsec_108_8
        addx8    a2, a8, a2
        addi.n   a8, a8, 1
        extui    a8, a8, 0, 16
        bltu     a10, a8, .Lsec_108_45
        j        .Lsec_108_47
.Lsec_108_45:
        movi.n   a8, 0
.Lsec_108_47:
        s16i     a8, a9, 0
        memw
.Lsec_108_4d:
        memw
        retw.n
        .size   SigSpaceMalloc, .-SigSpaceMalloc
.Lsec_108_52:

        .section .text.SigSpaceFree,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_109_0, pTxRx
.Lsec_109_4:
        .global SigSpaceFree
        .type   SigSpaceFree, @function
SigSpaceFree:
        entry    a1, 32
        l32r     a8, .Lsec_109_0
        l32i.n   a9, a8, 0
        l32i     a8, a9, 0x3c0
        addi.n   a8, a8, -1
        s32i     a8, a9, 0x3c0
        memw
        retw.n
        .size   SigSpaceFree, .-SigSpaceFree
.Lsec_109_19:

        .section .text.DefFreqCalTimerCB,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_110_0, CanDoFreqCal
.Lsec_110_4:
        .global DefFreqCalTimerCB
        .type   DefFreqCalTimerCB, @function
DefFreqCalTimerCB:
        entry    a1, 32
        l32r     a8, .Lsec_110_0
        movi.n   a9, 1
        s8i      a9, a8, 0
        memw
        memw
        retw.n
        .size   DefFreqCalTimerCB, .-DefFreqCalTimerCB
.Lsec_110_17:

        .section .wifirxiram.6,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_111_0, pTxRx
        .literal .Lsec_111_4, g_osi_funcs_p
.Lsec_111_8:
        .global ppProcessRxPktHdr
        .type   ppProcessRxPktHdr, @function
ppProcessRxPktHdr:
        entry    a1, 32
        l32r     a3, .Lsec_111_0
        l32i.n   a3, a3, 0
        l32i     a3, a3, 0x3bc
        beqz.n   a3, .Lsec_111_1d
        l32i.n   a11, a2, 0
        l32i     a10, a2, 4
        callx8   a3
.Lsec_111_1d:
        l32r     a3, .Lsec_111_4
        l32i.n   a8, a3, 0
        l32i.n   a10, a2, 4
        l32i     a8, a8, 176
        callx8   a8
        mov.n    a10, a2
        l32i.n   a3, a3, 0
        l32i     a3, a3, 176
        callx8   a3
        retw.n
        .size   ppProcessRxPktHdr, .-ppProcessRxPktHdr
.Lsec_111_36:

        .section .text.pp_register_net80211_tx_cb,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_112_0, .Lsec_257_0
.Lsec_112_4:
        .global pp_register_net80211_tx_cb
        .type   pp_register_net80211_tx_cb, @function
pp_register_net80211_tx_cb:
        entry    a1, 32
        l32r     a8, .Lsec_112_0
        s32i.n   a2, a8, 0
        movi.n   a2, 1
        memw
        retw.n
        .size   pp_register_net80211_tx_cb, .-pp_register_net80211_tx_cb
.Lsec_112_13:

        .section .text.pp_register_config_cb,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_113_0, .Lsec_255_0
.Lsec_113_4:
        .global pp_register_config_cb
        .type   pp_register_config_cb, @function
pp_register_config_cb:
        entry    a1, 32
        l32r     a8, .Lsec_113_0
        s32i.n   a2, a8, 0
        movi.n   a2, 1
        memw
        retw.n
        .size   pp_register_config_cb, .-pp_register_config_cb
.Lsec_113_13:

        .section .text.pp_register_timer_cb,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_114_0, .Lsec_256_0
.Lsec_114_4:
        .global pp_register_timer_cb
        .type   pp_register_timer_cb, @function
pp_register_timer_cb:
        entry    a1, 32
        l32r     a8, .Lsec_114_0
        s32i.n   a2, a8, 0
        movi.n   a2, 1
        memw
        retw.n
        .size   pp_register_timer_cb, .-pp_register_timer_cb
.Lsec_114_13:

        .section .text.pp_register_michael_mic_failure_cb,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_115_0, .Lsec_251_0
.Lsec_115_4:
        .global pp_register_michael_mic_failure_cb
        .type   pp_register_michael_mic_failure_cb, @function
pp_register_michael_mic_failure_cb:
        entry    a1, 32
        l32r     a8, .Lsec_115_0
        s32i.n   a2, a8, 0
        movi.n   a2, 0
        memw
        retw.n
        .size   pp_register_michael_mic_failure_cb, .-pp_register_michael_mic_failure_cb
.Lsec_115_13:

        .section .wifi0iram.7,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_116_0, .Lsec_257_0
        .literal .Lsec_116_4, config_is_cache_tx_buf_enabled
.Lsec_116_8:
        .global pp_process_hmac_waiting_txq
        .type   pp_process_hmac_waiting_txq, @function
pp_process_hmac_waiting_txq:
        entry    a1, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, config_is_cache_tx_buf_enabled
        l32r     a8, .Lsec_116_4
        callx8   a8
        beqz.n   a10, .Lsec_116_1c
        l32r     a8, .Lsec_116_0
        l32i     a10, a8, 0
        callx8   a10
.Lsec_116_1c:
        retw.n
        .size   pp_process_hmac_waiting_txq, .-pp_process_hmac_waiting_txq
.Lsec_116_1e:

        .section .rodata.pp_default_event_handler.str1.1,"aMS",@progbits,1
        .align  1
.Lsec_117_0:
        .ascii  "pp.c\000"

        .section .text.pp_default_event_handler,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_118_0, .Lsec_117_0
        .literal .Lsec_118_4, .Lsec_234_0
        .literal .Lsec_118_8, wifi_log
.Lsec_118_c:
        .global pp_default_event_handler
        .type   pp_default_event_handler, @function
pp_default_event_handler:
        entry    a1, 32
        movi.n   a12, 1
        l32r     a14, .Lsec_118_0
        movi     a15, 0x178
        slli     a11, a12, 11
        movi.n   a10, 6
        l32r     a13, .Lsec_118_4
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_118_8
        callx8   a8
.Lsec_118_25:
        j        .Lsec_118_25
        .size   pp_default_event_handler, .-pp_default_event_handler
.Lsec_118_28:

        .section .wifi0iram.11,"ax",@progbits
        .align  4
.Lsec_119_0:
        .global ppTxProtoProc
        .type   ppTxProtoProc, @function
ppTxProtoProc:
        entry    a1, 32
        l32i.n   a9, a2, 44
        l32i.n   a2, a2, 4
        l32i.n   a8, a9, 0
        l32i     a13, a2, 4
        mov.n    a12, a13
        bbci     a8, 18, .Lsec_119_14
        addi     a12, a13, 4
.Lsec_119_14:
        l8ui     a2, a12, 4
        bbci     a2, 0, .Lsec_119_21
        movi.n   a2, 2
        or       a8, a8, a2
        s32i.n   a8, a9, 0
.Lsec_119_21:
        movi.n   a11, 12
        l8ui     a10, a12, 0
        and      a11, a10, a11
        l32i.n   a8, a9, 0
        bnei     a11, 8, .Lsec_119_48
        movi     a2, 112
        and      a10, a10, a2
        beqi     a10, 64, .Lsec_119_40
        or       a8, a8, a11
        memw
        j        .Lsec_119_6f
.Lsec_119_40:
        movi.n   a2, -9
        and      a8, a8, a2
        j        .Lsec_119_6f
.Lsec_119_48:
        bnez.n   a11, .Lsec_119_71
        movi.n   a2, -16
        and      a10, a10, a2
        movi.n   a2, 80
        bne      a10, a2, .Lsec_119_61
        movi.n   a10, 1
        slli     a10, a10, 27
        bbci     a8, 1, .Lsec_119_6c
        j        .Lsec_119_71
        .byte    0x00
        .byte    0x00
.Lsec_119_61:
        bnei     a10, 64, .Lsec_119_71
        bbsi     a8, 1, .Lsec_119_71
        movi.n   a10, 1
        slli     a10, a10, 11
.Lsec_119_6c:
        or       a8, a8, a10
.Lsec_119_6f:
        s32i.n   a8, a9, 0
.Lsec_119_71:
        l32i.n   a2, a9, 0
        bbci     a2, 18, .Lsec_119_82
        l16ui    a8, a12, 22
        srli     a8, a8, 4
        s8i      a8, a13, 3
        memw
.Lsec_119_82:
        memw
        retw.n
        .size   ppTxProtoProc, .-ppTxProtoProc
.Lsec_119_87:

        .section .text.ppAddTimCount,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_120_0, pp_hdrsize
.Lsec_120_4:
        .global ppAddTimCount
        .type   ppAddTimCount, @function
ppAddTimCount:
        entry    a1, 32
        l16ui    a4, a2, 12
        bltui    a4, 3, .Lsec_120_10
        j        .Lsec_120_18a
.Lsec_120_10:
        l32i.n   a8, a2, 44
        l32i.n   a3, a2, 4
        l32i.n   a8, a8, 0
        l32i.n   a5, a3, 4
        bbci     a8, 18, .Lsec_120_1d
        addi.n   a5, a5, 4
.Lsec_120_1d:
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_hdrsize
        l32r     a8, .Lsec_120_0
        callx8   a8
        l16ui    a9, a2, 22
        addi     a9, a9, -4
        sub      a9, a9, a10
        extui    a9, a9, 0, 16
        addi     a9, a9, 24
        l32i.n   a8, a2, 16
        add.n    a9, a5, a9
        bltu     a9, a8, .Lsec_120_48
        memw
        l32i.n   a10, a3, 0
        extui    a10, a10, 0, 12
        add.n    a10, a8, a10
        bltu     a9, a10, .Lsec_120_4e
.Lsec_120_48:
        bnei     a4, 1, .Lsec_120_4e
        j        .Lsec_120_18a
.Lsec_120_4e:
        addi     a10, a5, 36
        movi.n   a13, 1
        movi.n   a12, 0
        j        .Lsec_120_ca
.Lsec_120_58:
        bnei     a4, 2, .Lsec_120_91
        beqz.n   a13, .Lsec_120_91
        bltu     a10, a8, .Lsec_120_6d
        memw
        l32i.n   a11, a3, 0
        extui    a11, a11, 0, 12
        add.n    a11, a8, a11
        bltu     a10, a11, .Lsec_120_91
.Lsec_120_6d:
        memw
        l32i.n   a11, a3, 0
        extui    a11, a11, 0, 12
        add.n    a11, a8, a11
        sub      a10, a10, a11
        memw
        l32i.n   a11, a3, 0
        extui    a11, a11, 0, 12
        add.n    a11, a8, a11
        sub      a9, a9, a11
        l32i.n   a5, a2, 8
        l32i.n   a13, a5, 4
        add.n    a10, a13, a10
        add.n    a9, a13, a9
        mov.n    a13, a12
.Lsec_120_91:
        l8ui     a5, a10, 0
        mov.n    a14, a12
        addi     a5, a5, -5
        moveqz   a14, a10, a5
        beqz.n   a13, .Lsec_120_b0
        addi.n   a5, a10, 1
        bltu     a5, a8, .Lsec_120_b6
        memw
        l32i.n   a11, a3, 0
        extui    a11, a11, 0, 12
        add.n    a11, a8, a11
        bgeu     a5, a11, .Lsec_120_b6
.Lsec_120_b0:
        l8ui     a11, a10, 1
        j        .Lsec_120_c3
.Lsec_120_b6:
        l32i.n   a5, a2, 8
        l32i.n   a5, a5, 4
        memw
        l8ui     a11, a5, 0
        extui    a11, a11, 0, 8
.Lsec_120_c3:
        bnez     a14, .Lsec_120_170
        addi.n   a11, a11, 2
        add.n    a10, a10, a11
.Lsec_120_ca:
        sub      a5, a9, a10
        bgei     a5, 2, .Lsec_120_58
        j        .Lsec_120_18a
.Lsec_120_d3:
        bnei     a4, 2, .Lsec_120_144
        bltu     a10, a8, .Lsec_120_e9
        memw
        l32i.n   a4, a3, 0
        extui    a4, a4, 0, 12
        add.n    a4, a8, a4
        bgeu     a10, a4, .Lsec_120_e9
        j        .Lsec_120_184
.Lsec_120_e9:
        memw
        l32i.n   a4, a3, 0
        extui    a4, a4, 0, 12
        add.n    a4, a8, a4
        sub      a4, a10, a4
        l32i.n   a5, a2, 8
        l32i.n   a10, a5, 4
        add.n    a10, a10, a4
        j        .Lsec_120_184
.Lsec_120_ff:
        memw
        l32i.n   a4, a3, 0
        extui    a4, a4, 0, 12
        add.n    a4, a8, a4
        bltu     a11, a4, .Lsec_120_17c
.Lsec_120_10c:
        memw
        l32i.n   a4, a3, 0
        extui    a4, a4, 0, 12
        add.n    a4, a8, a4
        sub      a4, a11, a4
        l32i.n   a5, a2, 8
        l32i.n   a11, a5, 4
        add.n    a11, a11, a4
        j        .Lsec_120_17c
        .byte    0x00
        .byte    0x00
.Lsec_120_124:
        memw
        l32i.n   a4, a3, 0
        extui    a4, a4, 0, 12
        add.n    a4, a8, a4
        bltu     a9, a4, .Lsec_120_144
.Lsec_120_131:
        memw
        l32i.n   a3, a3, 0
        extui    a3, a3, 0, 12
        add.n    a3, a8, a3
        sub      a3, a9, a3
        l32i.n   a2, a2, 8
        l32i.n   a9, a2, 4
        add.n    a9, a9, a3
.Lsec_120_144:
        l8ui     a8, a10, 0
        beqz.n   a8, .Lsec_120_14c
        j        .Lsec_120_14f
.Lsec_120_14c:
        l8ui     a8, a11, 0
.Lsec_120_14f:
        addi.n   a8, a8, -1
        extui    a8, a8, 0, 8
        s8i      a8, a10, 0
        memw
        bnez.n   a8, .Lsec_120_18a
        movi.n   a3, 1
        l8ui     a2, a9, 0
        or       a2, a2, a3
        s8i      a2, a9, 0
        memw
        memw
        j        .Lsec_120_18a
.Lsec_120_170:
        addi.n   a10, a14, 2
        addi.n   a11, a14, 3
        addi.n   a9, a14, 4
        bnez     a13, .Lsec_120_d3
        j        .Lsec_120_144
.Lsec_120_17c:
        bltu     a9, a8, .Lsec_120_131
        j        .Lsec_120_124
        .byte    0x00
        .byte    0x00
.Lsec_120_184:
        bltu     a11, a8, .Lsec_120_10c
        j        .Lsec_120_ff
.Lsec_120_18a:
        retw.n
        .size   ppAddTimCount, .-ppAddTimCount
.Lsec_120_18c:

        .section .text.ppRegisterPromisRxCallback,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_121_0, pTxRx
.Lsec_121_4:
        .global ppRegisterPromisRxCallback
        .type   ppRegisterPromisRxCallback, @function
ppRegisterPromisRxCallback:
        entry    a1, 32
        l32r     a8, .Lsec_121_0
        l32i.n   a8, a8, 0
        s32i     a2, a8, 0x3bc
        movi.n   a2, 0
        memw
        retw.n
        .size   ppRegisterPromisRxCallback, .-ppRegisterPromisRxCallback
.Lsec_121_16:

        .section .text.ppRegisterTxDoneUserActionCallback,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_122_0, .Lsec_258_0
.Lsec_122_4:
        .global ppRegisterTxDoneUserActionCallback
        .type   ppRegisterTxDoneUserActionCallback, @function
ppRegisterTxDoneUserActionCallback:
        entry    a1, 32
        l32r     a8, .Lsec_122_0
        s32i.n   a2, a8, 0
        movi.n   a2, 0
        memw
        retw.n
        .size   ppRegisterTxDoneUserActionCallback, .-ppRegisterTxDoneUserActionCallback
.Lsec_122_13:

        .section .text.ppRegisterRxCallback,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_123_0, pTxRx
.Lsec_123_4:
        .global ppRegisterRxCallback
        .type   ppRegisterRxCallback, @function
ppRegisterRxCallback:
        entry    a1, 32
        extui    a3, a3, 0, 8
        movi.n   a8, 1
        bgeui    a3, 3, .Lsec_123_1c
        l32r     a8, .Lsec_123_0
        l32i.n   a8, a8, 0
        addx4    a3, a3, a8
        movi.n   a8, 0
        s32i     a2, a3, 0x3b0
.Lsec_123_1c:
        mov.n    a2, a8
        memw
        retw.n
        .size   ppRegisterRxCallback, .-ppRegisterRxCallback
.Lsec_123_23:

        .section .text.ppRegisterTxCallback,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_124_0, pTxRx
.Lsec_124_4:
        .global ppRegisterTxCallback
        .type   ppRegisterTxCallback, @function
ppRegisterTxCallback:
        entry    a1, 32
        l32r     a8, .Lsec_124_0
        movi.n   a11, 1
        extui    a12, a3, 0, 8
        ssl      a3
        sll      a3, a11
        extui    a4, a4, 0, 8
        l32i.n   a8, a8, 0
        mov.n    a9, a11
        l32i     a10, a8, 0x35c
        bany     a10, a3, .Lsec_124_4c
        l32i     a13, a8, 0x3c8
        and      a9, a3, a13
        bnez.n   a9, .Lsec_124_4a
        beqz.n   a4, .Lsec_124_38
        or       a3, a3, a13
        s32i     a3, a8, 0x3c8
        memw
        j        .Lsec_124_3e
.Lsec_124_38:
        or       a10, a10, a3
        s32i     a10, a8, 0x35c
.Lsec_124_3e:
        addx4    a12, a12, a8
        s32i     a2, a12, 0x364
        memw
        j        .Lsec_124_4c
.Lsec_124_4a:
        mov.n    a9, a11
.Lsec_124_4c:
        mov.n    a2, a9
        retw.n
        .size   ppRegisterTxCallback, .-ppRegisterTxCallback
.Lsec_124_50:

        .section .text.ppUnregisterTxCallback,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_125_0, pTxRx
.Lsec_125_4:
        .global ppUnregisterTxCallback
        .type   ppUnregisterTxCallback, @function
ppUnregisterTxCallback:
        entry    a1, 32
        l32r     a8, .Lsec_125_0
        extui    a9, a2, 0, 8
        l32i.n   a10, a8, 0
        movi.n   a8, 1
        ssl      a2
        sll      a13, a8
        mov.n    a2, a8
        l32i     a11, a10, 0x35c
        l32i     a12, a10, 0x3c8
        or       a14, a11, a12
        bnone    a14, a13, .Lsec_125_44
        movi.n   a8, -1
        xor      a2, a8, a13
        and      a11, a11, a2
        and      a2, a2, a12
        s32i     a2, a10, 0x3c8
        movi     a2, 216
        add.n    a9, a9, a2
        addx4    a9, a9, a10
        movi     a2, 0
        s32i     a11, a10, 0x35c
        s32i     a2, a9, 4
.Lsec_125_44:
        memw
        retw.n
        .size   ppUnregisterTxCallback, .-ppUnregisterTxCallback
.Lsec_125_49:

        .section .text.pp_register_tx_cb,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_126_0, ppRegisterTxCallback
.Lsec_126_4:
        .global pp_register_tx_cb
        .type   pp_register_tx_cb, @function
pp_register_tx_cb:
        entry    a1, 32
        extui    a12, a4, 0, 8
        extui    a11, a3, 0, 8
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, ppRegisterTxCallback
        l32r     a8, .Lsec_126_0
        callx8   a8
        mov.n    a2, a10
        retw.n
        .size   pp_register_tx_cb, .-pp_register_tx_cb
.Lsec_126_19:

        .section .text.pp_unregister_tx_cb,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_127_0, ppUnregisterTxCallback
.Lsec_127_4:
        .global pp_unregister_tx_cb
        .type   pp_unregister_tx_cb, @function
pp_unregister_tx_cb:
        entry    a1, 32
        extui    a10, a2, 0, 8
        .reloc  ., R_XTENSA_ASM_EXPAND, ppUnregisterTxCallback
        l32r     a8, .Lsec_127_0
        callx8   a8
        mov.n    a2, a10
        retw.n
        .size   pp_unregister_tx_cb, .-pp_unregister_tx_cb
.Lsec_127_14:

        .section .wifiextrairam.16,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_128_0, pTxRx
        .literal .Lsec_128_4, .Lsec_117_0
        .literal .Lsec_128_8, .Lsec_229_0
        .literal .Lsec_128_c, wifi_log
        .literal .Lsec_128_10, wDev_AppendRxBlocks
        .literal .Lsec_128_14, esf_buf_recycle
.Lsec_128_18:
        .global ppRecycleRxPkt
        .type   ppRecycleRxPkt, @function
ppRecycleRxPkt:
        entry    a1, 32
        l32i.n   a8, a2, 4
        l32i.n   a9, a2, 16
        s32i.n   a9, a8, 4
        l8ui     a10, a2, 26
        bnei     a10, 6, .Lsec_128_61
        l32r     a11, .Lsec_128_0
        l32i.n   a11, a11, 0
        memw
        l32i.n   a9, a8, 0
        extui    a9, a9, 0, 12
        l32i     a11, a11, 0x3c4
        beq      a9, a11, .Lsec_128_54
        movi.n   a12, 1
        movi     a15, 0x451
        l32r     a14, .Lsec_128_4
        slli     a11, a12, 11
        l32r     a13, .Lsec_128_8
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_128_c
        callx8   a8
.Lsec_128_51:
        j        .Lsec_128_51
.Lsec_128_54:
        l16ui    a12, a2, 12
        mov.n    a10, a8
        l32i.n   a11, a2, 8
        .reloc  ., R_XTENSA_ASM_EXPAND, wDev_AppendRxBlocks
        l32r     a8, .Lsec_128_10
        callx8   a8
.Lsec_128_61:
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esf_buf_recycle
        l32r     a8, .Lsec_128_14
        callx8   a8
        retw.n
        .size   ppRecycleRxPkt, .-ppRecycleRxPkt
.Lsec_128_6b:

        .section .text.ppRecycleTxBuf,"ax",@progbits
        .align  4
.Lsec_129_0:
        .global ppRecycleTxBuf
        .type   ppRecycleTxBuf, @function
ppRecycleTxBuf:
        entry    a1, 32
        movi.n   a2, 1
        retw.n
        .size   ppRecycleTxBuf, .-ppRecycleTxBuf
.Lsec_129_7:

        .section .text.ppCheckTxQIdle,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_130_0, lmacIsIdle
.Lsec_130_4:
        .global ppCheckTxQIdle
        .type   ppCheckTxQIdle, @function
ppCheckTxQIdle:
        entry    a1, 32
        movi.n   a3, 0
        mov.n    a2, a3
        movi.n   a4, 1
.Lsec_130_d:
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacIsIdle
        l32r     a8, .Lsec_130_0
        callx8   a8
        addi.n   a3, a3, 1
        moveqz   a2, a4, a10
        bnei     a3, 5, .Lsec_130_d
        retw.n
        .size   ppCheckTxQIdle, .-ppCheckTxQIdle
.Lsec_130_1f:

        .section .text.ppCheckIsConnTraffic,"ax",@progbits
        .align  4
.Lsec_131_0:
        .global ppCheckIsConnTraffic
        .type   ppCheckIsConnTraffic, @function
ppCheckIsConnTraffic:
        entry    a1, 32
        l32i.n   a10, a2, 44
        movi.n   a8, 0
        l32i.n   a9, a10, 16
        extui    a9, a9, 18, 2
        bne      a9, a8, .Lsec_131_4a
        movi.n   a8, 1
        l32i.n   a10, a10, 0
        slli     a8, a8, 21
        and      a9, a10, a8
        movi.n   a8, 1
        bnez.n   a9, .Lsec_131_4a
        l32i.n   a11, a2, 36
        mov.n    a8, a9
        beqz.n   a11, .Lsec_131_4a
        l8ui     a9, a11, 11
        bbsi     a9, 7, .Lsec_131_4a
        movi.n   a8, 1
        bbsi     a10, 13, .Lsec_131_4a
        l32i.n   a2, a2, 4
        l32i     a8, a2, 4
        bbci     a10, 18, .Lsec_131_38
        addi.n   a8, a8, 4
.Lsec_131_38:
        movi.n   a2, 12
        l8ui     a9, a8, 0
        movi.n   a8, 1
        and      a9, a9, a2
        movi.n   a2, 0
        movnez   a8, a2, a9
        extui    a8, a8, 0, 8
.Lsec_131_4a:
        mov.n    a2, a8
        retw.n
        .size   ppCheckIsConnTraffic, .-ppCheckIsConnTraffic
.Lsec_131_4e:

        .section .text.ppCheckTxConnTrafficIdle,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_132_0, pTxRx
        .literal .Lsec_132_4, lmacGetTxFrame
        .literal .Lsec_132_8, ppCheckIsConnTraffic
        .literal .Lsec_132_c, ppCheckIsConnTraffic
        .literal .Lsec_132_10, ppCheckIsConnTraffic
        .literal .Lsec_132_14, ppCheckIsConnTraffic
        .literal .Lsec_132_18, ppCheckIsConnTraffic
.Lsec_132_1c:
        .global ppCheckTxConnTrafficIdle
        .type   ppCheckTxConnTrafficIdle, @function
ppCheckTxConnTrafficIdle:
        entry    a1, 32
        movi.n   a2, 0
.Lsec_132_21:
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacGetTxFrame
        l32r     a8, .Lsec_132_4
        callx8   a8
        beqz.n   a10, .Lsec_132_34
        .reloc  ., R_XTENSA_ASM_EXPAND, ppCheckIsConnTraffic
        l32r     a8, .Lsec_132_8
        callx8   a8
        bnez     a10, .Lsec_132_e4
.Lsec_132_34:
        addi.n   a2, a2, 1
        bnei     a2, 5, .Lsec_132_21
        l32r     a2, .Lsec_132_0
        movi.n   a4, 1
        movi.n   a5, -1
        l32i.n   a3, a2, 0
        l32i.n   a2, a3, 0
        j        .Lsec_132_98
        .byte    0x00
.Lsec_132_48:
        neg      a8, a2
        and      a8, a8, a2
        nsau     a8, a8
        neg      a8, a8
        addi     a6, a8, 31
        addx2    a8, a6, a6
        slli     a8, a8, 4
        add.n    a8, a3, a8
        l32i.n   a7, a8, 32
        j        .Lsec_132_71
.Lsec_132_64:
        mov.n    a10, a7
        .reloc  ., R_XTENSA_ASM_EXPAND, ppCheckIsConnTraffic
        l32r     a8, .Lsec_132_c
        callx8   a8
        bnez     a10, .Lsec_132_e4
        l32i.n   a7, a7, 40
.Lsec_132_71:
        bnez     a7, .Lsec_132_64
        addi.n   a9, a6, 1
        addx2    a9, a9, a9
        slli     a9, a9, 4
        add.n    a9, a3, a9
        l32i.n   a10, a9, 4
        beqz     a10, .Lsec_132_8c
        .reloc  ., R_XTENSA_ASM_EXPAND, ppCheckIsConnTraffic
        l32r     a8, .Lsec_132_10
        callx8   a8
        bnez     a10, .Lsec_132_e4
.Lsec_132_8c:
        ssl      a6
        sll      a8, a4
        xor      a8, a5, a8
        and      a2, a2, a8
.Lsec_132_98:
        bnez     a2, .Lsec_132_48
        movi     a4, 0x334
        movi     a6, 0x34c
        add.n    a4, a3, a4
        add.n    a6, a3, a6
.Lsec_132_a5:
        l32i.n   a5, a4, 0
        j        .Lsec_132_b6
.Lsec_132_aa:
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, ppCheckIsConnTraffic
        l32r     a8, .Lsec_132_14
        callx8   a8
        bnez.n   a10, .Lsec_132_e4
        l32i.n   a5, a5, 40
.Lsec_132_b6:
        bnez     a5, .Lsec_132_aa
        addi.n   a4, a4, 8
        bne      a6, a4, .Lsec_132_a5
        movi     a5, 224
        addi     a4, a3, 32
        add.n    a3, a3, a5
.Lsec_132_c6:
        l32i.n   a5, a4, 0
        j        .Lsec_132_d8
        .byte    0x00
.Lsec_132_cc:
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, ppCheckIsConnTraffic
        l32r     a8, .Lsec_132_18
        callx8   a8
        bnez.n   a10, .Lsec_132_e4
        l32i.n   a5, a5, 40
.Lsec_132_d8:
        bnez     a5, .Lsec_132_cc
        addi     a4, a4, 48
        bne      a4, a3, .Lsec_132_c6
        j        .Lsec_132_e6
.Lsec_132_e4:
        movi.n   a2, 1
.Lsec_132_e6:
        retw.n
        .size   ppCheckTxConnTrafficIdle, .-ppCheckTxConnTrafficIdle
.Lsec_132_e8:

        .section .wifiextrairam.18,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_133_0, g_osi_funcs_p
.Lsec_133_4:
        .global pp_coex_tx_release
        .type   pp_coex_tx_release, @function
pp_coex_tx_release:
        entry    a1, 32
        l32i.n   a8, a2, 4
        movi.n   a10, 12
        l32i.n   a8, a8, 4
        l8ui     a9, a8, 0
        movi.n   a8, -16
        and      a10, a9, a10
        and      a8, a9, a8
        bnez.n   a10, .Lsec_133_44
        bnei     a8, 128, .Lsec_133_2a
        l32r     a8, .Lsec_133_0
        movi.n   a10, 4
        l32i.n   a8, a8, 0
        l32i     a8, a8, 0x1a0
        j        .Lsec_133_5a
.Lsec_133_2a:
        bnez.n   a10, .Lsec_133_44
        addi     a8, a8, 96
        movi     a9, 208
        bany     a8, a9, .Lsec_133_5d
        l32r     a8, .Lsec_133_0
        movi.n   a10, 7
        l32i.n   a8, a8, 0
        l32i     a8, a8, 0x1a0
        j        .Lsec_133_5a
        .byte    0x00
        .byte    0x00
.Lsec_133_44:
        bnei     a10, 8, .Lsec_133_5d
        movi     a8, 112
        and      a9, a9, a8
        bnei     a9, 64, .Lsec_133_5d
        l32r     a8, .Lsec_133_0
        movi.n   a10, 9
        l32i.n   a8, a8, 0
        l32i     a8, a8, 0x1a0
.Lsec_133_5a:
        callx8   a8
.Lsec_133_5d:
        retw.n
        .size   pp_coex_tx_release, .-pp_coex_tx_release
.Lsec_133_5f:

        .section .wifi0iram.19,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_134_0, pTxRx
.Lsec_134_4:
        .global ppProcTxCallback
        .type   ppProcTxCallback, @function
ppProcTxCallback:
        entry    a1, 32
        l32i.n   a4, a2, 44
        extui    a3, a3, 0, 8
        l32i.n   a4, a4, 20
        j        .Lsec_134_50
.Lsec_134_11:
        neg      a8, a4
        and      a8, a8, a4
        l32r     a5, .Lsec_134_0
        nsau     a8, a8
        neg      a8, a8
        addi     a8, a8, 31
        l32i.n   a9, a5, 0
        movi.n   a5, 1
        ssl      a8
        sll      a5, a5
        beqz.n   a3, .Lsec_134_35
        l32i     a10, a9, 0x3c8
        j        .Lsec_134_38
.Lsec_134_35:
        l32i     a10, a9, 0x35c
.Lsec_134_38:
        bnone    a5, a10, .Lsec_134_48
        addx4    a8, a8, a9
        l32i     a8, a8, 0x364
        beqz.n   a8, .Lsec_134_48
        mov.n    a10, a2
        callx8   a8
.Lsec_134_48:
        movi.n   a8, -1
        xor      a5, a8, a5
        and      a4, a4, a5
.Lsec_134_50:
        bnez     a4, .Lsec_134_11
        retw.n
        .size   ppProcTxCallback, .-ppProcTxCallback
.Lsec_134_55:

        .section .text.ppSetInterface,"ax",@progbits
        .align  4
.Lsec_135_0:
        .global ppSetInterface
        .type   ppSetInterface, @function
ppSetInterface:
        entry    a1, 32
        retw.n
        .size   ppSetInterface, .-ppSetInterface
.Lsec_135_5:

        .section .wifi0iram.22,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_136_0, g_osi_funcs_p
        .literal .Lsec_136_4, pm_coex_reconnect_policy
        .literal .Lsec_136_8, chm_get_current_band
        .literal .Lsec_136_c, pm_coex_reconnect_policy
.Lsec_136_10:
        .global pp_coex_tx_request
        .type   pp_coex_tx_request, @function
pp_coex_tx_request:
        entry    a1, 48
        l32i.n   a3, a2, 4
        movi.n   a4, 0
        movi.n   a8, -16
        l32i.n   a3, a3, 4
        s32i.n   a4, a1, 0
        l8ui     a9, a3, 0
        movi.n   a3, 12
        and      a3, a9, a3
        and      a8, a9, a8
        bne      a3, a4, .Lsec_136_79
        bnei     a8, 128, .Lsec_136_4e
        l32r     a2, .Lsec_136_0
        mov.n    a11, a1
        movi.n   a10, 4
        l32i.n   a3, a2, 0
        l32i     a3, a3, 0x1a8
        memw
        callx8   a3
        mov.n    a11, a4
        movi.n   a10, 4
        l32i.n   a2, a2, 0
        l32i.n   a12, a1, 0
        l32i     a2, a2, 0x19c
        j        .Lsec_136_11f
.Lsec_136_4e:
        bnez.n   a3, .Lsec_136_79
        addi     a10, a8, 96
        movi     a4, 208
        bany     a10, a4, .Lsec_136_a4
        l32r     a2, .Lsec_136_0
        mov.n    a11, a1
        movi.n   a10, 7
        l32i.n   a4, a2, 0
        l32i     a4, a4, 0x1a8
        callx8   a4
        mov.n    a11, a3
        movi.n   a10, 7
        l32i.n   a2, a2, 0
        l32i.n   a12, a1, 0
        l32i     a2, a2, 0x19c
        j        .Lsec_136_11f
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_136_79:
        bnei     a3, 8, .Lsec_136_da
        movi     a10, 112
        and      a10, a9, a10
        bnei     a10, 64, .Lsec_136_da
        l32r     a2, .Lsec_136_0
        mov.n    a11, a1
        movi.n   a10, 9
        l32i.n   a3, a2, 0
        l32i     a3, a3, 0x1a8
        callx8   a3
        movi.n   a11, 0
        movi.n   a10, 9
        l32i.n   a2, a2, 0
        l32i.n   a12, a1, 0
        l32i     a2, a2, 0x19c
        j        .Lsec_136_11f
        .byte    0x00
        .byte    0x00
.Lsec_136_a4:
        bnei     a8, 64, .Lsec_136_da
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_coex_reconnect_policy
        l32r     a8, .Lsec_136_4
        callx8   a8
        beqz.n   a10, .Lsec_136_cc
.Lsec_136_af:
        l32r     a2, .Lsec_136_0
        mov.n    a11, a1
        movi.n   a10, 39
        l32i.n   a3, a2, 0
        l32i     a3, a3, 0x1a8
        callx8   a3
        movi.n   a11, 0
        movi.n   a10, 39
        l32i.n   a2, a2, 0
        l32i.n   a12, a1, 0
        l32i     a2, a2, 0x19c
        j        .Lsec_136_11f
.Lsec_136_cc:
        .reloc  ., R_XTENSA_ASM_EXPAND, chm_get_current_band
        l32r     a8, .Lsec_136_8
        callx8   a8
        bnei     a10, 2, .Lsec_136_122
        j        .Lsec_136_af
        .byte    0x00
        .byte    0x00
.Lsec_136_da:
        l32i.n   a2, a2, 44
        l32i.n   a2, a2, 0
        bbsi     a2, 2, .Lsec_136_fc
        bnez.n   a3, .Lsec_136_122
        movi     a2, -48
        and      a9, a9, a2
        movi.n   a10, 1
        mov.n    a2, a3
        moveqz   a2, a10, a9
        bnez.n   a2, .Lsec_136_fc
        movi     a2, -176
        add.n    a8, a8, a2
        movnez   a10, a3, a8
        beqz.n   a10, .Lsec_136_122
.Lsec_136_fc:
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_coex_reconnect_policy
        l32r     a8, .Lsec_136_c
        callx8   a8
        beqz.n   a10, .Lsec_136_122
        l32r     a3, .Lsec_136_0
        mov.n    a11, a1
        movi     a10, 40
        l32i.n   a2, a3, 0
        l32i     a2, a2, 0x1a8
        callx8   a2
        movi.n   a11, 0
        movi.n   a10, 40
        l32i.n   a2, a3, 0
        l32i.n   a12, a1, 0
        l32i     a2, a2, 0x19c
.Lsec_136_11f:
        callx8   a2
.Lsec_136_122:
        retw.n
        .size   pp_coex_tx_request, .-pp_coex_tx_request
.Lsec_136_124:

        .section .text.esp_wifi_enable_easy_fragment,"ax",@progbits
        .align  4
.Lsec_137_0:
        .global esp_wifi_enable_easy_fragment
        .type   esp_wifi_enable_easy_fragment, @function
esp_wifi_enable_easy_fragment:
        entry    a1, 32
        retw.n
        .size   esp_wifi_enable_easy_fragment, .-esp_wifi_enable_easy_fragment
.Lsec_137_5:

        .section .wifislprxiram.24,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_138_0, pm_on_data_rx
        .literal .Lsec_138_4, pp_hdrsize
        .literal .Lsec_138_8, pm_sleep_for
        .literal .Lsec_138_c, pm_set_beacon_duration
        .literal .Lsec_138_10, pm_on_beacon_rx
        .literal .Lsec_138_14, rc_get_trc
        .literal .Lsec_138_18, rcUpdateRxDone
.Lsec_138_1c:
        .global ppRxProtoProc
        .type   ppRxProtoProc, @function
ppRxProtoProc:
        entry    a1, 48
        l32i.n   a4, a2, 4
        movi.n   a6, -16
        movi.n   a9, 12
        l32i.n   a5, a4, 4
        l8ui     a4, a5, 0
        and      a8, a4, a6
        and      a9, a4, a9
        movi.n   a4, 1
        l32i.n   a7, a3, 0
        slli     a4, a4, 29
        and      a6, a7, a4
        bbci     a7, 28, .Lsec_138_48
        movi.n   a4, 3
        movi.n   a7, 0
        moveqz   a4, a7, a6
        j        .Lsec_138_59
        .byte    0x00
.Lsec_138_48:
        movi.n   a4, 1
        bnez.n   a6, .Lsec_138_59
        slli     a4, a4, 30
        and      a7, a7, a4
        movi.n   a6, 2
        movi.n   a4, 3
        movnez   a4, a6, a7
.Lsec_138_59:
        bnei     a9, 8, .Lsec_138_6f
        l8ui     a13, a5, 1
        addi.n   a12, a5, 4
        mov.n    a11, a4
        addi     a10, a5, 10
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_on_data_rx
        l32r     a8, .Lsec_138_0
        callx8   a8
        j        .Lsec_138_fc
.Lsec_138_6f:
        bnez     a9, .Lsec_138_fc
        beqi     a8, 128, .Lsec_138_78
        j        .Lsec_138_fc
.Lsec_138_78:
        l32i.n   a6, a2, 44
        movi.n   a7, 16
        l32i.n   a6, a6, 0
        and      a6, a6, a7
        bnez     a6, .Lsec_138_118
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_hdrsize
        l32r     a8, .Lsec_138_4
        callx8   a8
        l32i.n   a8, a3, 24
        extui    a7, a10, 0, 16
        extui    a8, a8, 0, 12
        addi.n   a7, a7, 3
        bge      a7, a8, .Lsec_138_a2
        addi     a8, a8, -4
        sub      a8, a8, a10
        extui    a6, a8, 0, 16
.Lsec_138_a2:
        addi.n   a10, a5, 10
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_sleep_for
        l32r     a8, .Lsec_138_8
        callx8   a8
        beqz     a10, .Lsec_138_fc
        l16ui    a7, a2, 12
        bnei     a7, 1, .Lsec_138_fc
        movi.n   a9, 31
        slli     a9, a9, 8
        l32i.n   a8, a3, 0
        addi     a11, a5, 24
        and      a8, a8, a9
        addmi    a8, a8, 0xfffff500
        movi.n   a9, 6
        l32i.n   a10, a3, 24
        moveqz   a7, a9, a8
        extui    a10, a10, 12, 12
        slli     a10, a10, 3
        quou     a10, a10, a7
        s32i.n   a11, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_set_beacon_duration
        l32r     a8, .Lsec_138_c
        callx8   a8
        movi.n   a13, 0
        l32i.n   a8, a3, 24
        extui    a8, a8, 0, 12
        slli     a8, a8, 3
        quou     a8, a8, a7
        l32i.n   a9, a2, 44
        l32i.n   a11, a1, 0
        add.n    a12, a11, a6
        l32i.n   a10, a9, 4
        sub      a10, a10, a8
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_on_beacon_rx
        l32r     a8, .Lsec_138_10
        callx8   a8
.Lsec_138_fc:
        beqi     a4, 3, .Lsec_138_118
        addi.n   a11, a5, 10
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, rc_get_trc
        l32r     a8, .Lsec_138_14
        callx8   a8
        s32i.n   a10, a2, 36
        beqz.n   a10, .Lsec_138_118
        mov.n    a11, a3
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, rcUpdateRxDone
        l32r     a8, .Lsec_138_18
        callx8   a8
.Lsec_138_118:
        movi.n   a2, 0
        retw.n
        .size   ppRxProtoProc, .-ppRxProtoProc
.Lsec_138_11c:

        .section .text.pp_gettid,"ax",@progbits
        .align  4
.Lsec_139_0:
        .global pp_gettid
        .type   pp_gettid, @function
pp_gettid:
        entry    a1, 32
        l8ui     a9, a2, 0
        movi     a8, 140
        and      a9, a9, a8
        movi     a10, 136
        movi.n   a8, 16
        bne      a9, a10, .Lsec_139_2a
        l8ui     a8, a2, 1
        extui    a8, a8, 0, 2
        bnei     a8, 3, .Lsec_139_24
        l8ui     a8, a2, 30
        j        .Lsec_139_27
        .byte    0x00
.Lsec_139_24:
        l8ui     a8, a2, 24
.Lsec_139_27:
        extui    a8, a8, 0, 4
.Lsec_139_2a:
        mov.n    a2, a8
        retw.n
        .size   pp_gettid, .-pp_gettid
.Lsec_139_2e:

        .section .text.ppAssembleMicHdr,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_140_0, memcpy
        .literal .Lsec_140_4, memcpy
        .literal .Lsec_140_8, memcpy
        .literal .Lsec_140_c, memcpy
        .literal .Lsec_140_10, pp_gettid
.Lsec_140_14:
        .global ppAssembleMicHdr
        .type   ppAssembleMicHdr, @function
ppAssembleMicHdr:
        entry    a1, 32
        l8ui     a8, a2, 1
        addi.n   a4, a3, 6
        extui    a8, a8, 0, 2
        beqi     a8, 2, .Lsec_140_40
        beqi     a8, 3, .Lsec_140_54
        addi.n   a5, a2, 10
        movi.n   a12, 6
        addi.n   a11, a2, 4
        bnei     a8, 1, .Lsec_140_31
        addi     a11, a2, 16
.Lsec_140_31:
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_140_0
        callx8   a8
        movi.n   a12, 6
        mov.n    a11, a5
        j        .Lsec_140_66
.Lsec_140_40:
        movi.n   a12, 6
        addi.n   a11, a2, 4
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_140_4
        callx8   a8
        movi.n   a12, 6
        addi     a11, a2, 16
        j        .Lsec_140_66
.Lsec_140_54:
        movi.n   a12, 6
        addi     a11, a2, 16
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_140_8
        callx8   a8
        movi.n   a12, 6
        addi     a11, a2, 24
.Lsec_140_66:
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_140_c
        callx8   a8
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_gettid
        l32r     a8, .Lsec_140_10
        callx8   a8
        bnei     a10, 16, .Lsec_140_7c
        movi     a10, 0
.Lsec_140_7c:
        movi.n   a2, 0
        s8i      a10, a3, 12
        s8i      a2, a3, 13
        s8i      a2, a3, 14
        s8i      a2, a3, 15
        memw
        memw
        retw.n
        .size   ppAssembleMicHdr, .-ppAssembleMicHdr
.Lsec_140_92:

        .section .text.ppCalTkipMic,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_141_0, s_fragment
        .literal .Lsec_141_4, roundup2
        .literal .Lsec_141_8, ppAssembleMicHdr
        .literal .Lsec_141_c, memcpy
        .literal .Lsec_141_10, getle32
        .literal .Lsec_141_14, getle32
        .literal .Lsec_141_18, getle32
        .literal .Lsec_141_1c, xswap
        .literal .Lsec_141_20, putle32
        .literal .Lsec_141_24, putle32
.Lsec_141_28:
        .global ppCalTkipMic
        .type   ppCalTkipMic, @function
ppCalTkipMic:
        entry    a1, 48
        mov.n    a12, a3
        l32r     a3, .Lsec_141_0
        extui    a14, a4, 0, 16
        mov.n    a7, a1
        movi.n   a11, 4
        l16ui    a8, a3, 4
        sub      a4, a8, a14
        addi.n   a10, a4, 13
        addi.n   a3, a4, 8
        s32i.n   a14, a7, 8
        s32i.n   a12, a7, 4
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, roundup2
        l32r     a8, .Lsec_141_4
        callx8   a8
        addi.n   a9, a10, 15
        srli     a9, a9, 4
        slli     a9, a9, 4
        sub      a9, a1, a9
        movsp    a1, a9
        l32i.n   a12, a7, 4
        sub      a13, a10, a3
        addi.n   a13, a13, -1
        mov.n    a11, a1
        mov.n    a6, a10
        l32i.n   a9, a12, 4
        add.n    a3, a1, a3
        l32i.n   a9, a9, 4
        mov.n    a10, a9
        s32i.n   a13, a7, 12
        s32i     a9, a7, 4
        s32i     a1, a7, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, ppAssembleMicHdr
        l32r     a8, .Lsec_141_8
        callx8   a8
        addi     a12, a4, -8
        addi     a10, a1, 16
        l32i.n   a9, a7, 4
        l32i.n   a14, a7, 8
        add.n    a11, a9, a14
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_141_c
        callx8   a8
        movi.n   a9, 90
        s8i      a9, a3, 0
        memw
        movi.n   a3, 0
        mov.n    a10, a3
        l32i.n   a13, a7, 12
        memw
        j        .Lsec_141_b7
.Lsec_141_a6:
        l32i.n   a8, a7, 0
        add.n    a9, a8, a3
        add.n    a9, a9, a4
        addi.n   a3, a3, 1
        s8i      a10, a9, 9
        memw
        extui    a3, a3, 0, 8
.Lsec_141_b7:
        bltu     a3, a13, .Lsec_141_a6
        addi     a10, a2, 16
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, getle32
        l32r     a8, .Lsec_141_10
        callx8   a8
        mov.n    a3, a10
        addi     a10, a2, 20
        .reloc  ., R_XTENSA_ASM_EXPAND, getle32
        l32r     a8, .Lsec_141_14
        callx8   a8
        mov.n    a2, a10
        movi.n   a8, 0
        j        .Lsec_141_11f
        .byte    0x00
.Lsec_141_d9:
        l32i.n   a4, a7, 0
        add.n    a10, a4, a8
        s32i.n   a8, a7, 4
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, getle32
        l32r     a8, .Lsec_141_18
        callx8   a8
        xor      a3, a3, a10
        ssai     15
        src      a4, a3, a3
        xor      a2, a4, a2
        add      a3, a3, a2
        or       a10, a3, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, xswap
        l32r     a8, .Lsec_141_1c
        callx8   a8
        xor      a10, a2, a10
        add.n    a3, a3, a10
        ssai     29
        src      a2, a3, a3
        xor      a2, a2, a10
        add.n    a3, a3, a2
        ssai     2
        src      a9, a3, a3
        l32i.n   a8, a7, 4
        xor      a2, a9, a2
        add.n    a3, a3, a2
        addi.n   a8, a8, 4
.Lsec_141_11f:
        bltu     a8, a6, .Lsec_141_d9
        mov.n    a11, a3
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, putle32
        l32r     a8, .Lsec_141_20
        callx8   a8
        mov.n    a11, a2
        addi.n   a10, a5, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, putle32
        l32r     a8, .Lsec_141_24
        callx8   a8
        retw.n
        .size   ppCalTkipMic, .-ppCalTkipMic
.Lsec_141_138:

        .section .text.ppClearRxFragment,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_142_0, s_fragment
        .literal .Lsec_142_4, .Lsec_226_0
        .literal .Lsec_142_8, wifi_log
        .literal .Lsec_142_c, ppRecycleRxPkt
.Lsec_142_10:
        .global ppClearRxFragment
        .type   ppClearRxFragment, @function
ppClearRxFragment:
        entry    a1, 32
        l32r     a2, .Lsec_142_0
        l32i.n   a8, a2, 0
        beqz.n   a8, .Lsec_142_35
        movi.n   a12, 3
        movi.n   a11, 32
        l32r     a13, .Lsec_142_4
        movi.n   a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_142_8
        callx8   a8
        l32i.n   a10, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, ppRecycleRxPkt
        l32r     a8, .Lsec_142_c
        callx8   a8
        movi.n   a8, 0
        s32i.n   a8, a2, 0
.Lsec_142_35:
        memw
        retw.n
        .size   ppClearRxFragment, .-ppClearRxFragment
.Lsec_142_3a:

        .section .wifirxiram.26,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_143_0, s_fragment
        .literal .Lsec_143_4, g_mesh_is_started
        .literal .Lsec_143_8, g_mesh_is_root
        .literal .Lsec_143_c, g_mesh_init_ps_type
        .literal .Lsec_143_10, g_osi_funcs_p
        .literal .Lsec_143_14, 0x3ff73c00
        .literal .Lsec_143_18, g_wifi_mac_time_delta
        .literal .Lsec_143_1c, lmacConfMib
        .literal .Lsec_143_20, g_lmac_cnt
        .literal .Lsec_143_24, .Lsec_225_0
        .literal .Lsec_143_28, pTxRx
        .literal .Lsec_143_2c, 0xff0000ff
        .literal .Lsec_143_30, 0xf000ffff
        .literal .Lsec_143_34, .Lsec_251_0
        .literal .Lsec_143_38, ppRecycleRxPkt
        .literal .Lsec_143_3c, ic_get_ptk_alg
        .literal .Lsec_143_40, pp_hdrsize
        .literal .Lsec_143_44, wifi_log
        .literal .Lsec_143_48, ppRecycleRxPkt
        .literal .Lsec_143_4c, pp_gettid
        .literal .Lsec_143_50, pp_gettid
        .literal .Lsec_143_54, memcmp
        .literal .Lsec_143_58, memcpy
        .literal .Lsec_143_5c, ppRecycleRxPkt
        .literal .Lsec_143_60, memcpy
        .literal .Lsec_143_64, ic_obtain_key
        .literal .Lsec_143_68, ppCalTkipMic
        .literal .Lsec_143_6c, memcmp
        .literal .Lsec_143_70, ppRecycleRxPkt
        .literal .Lsec_143_74, ppRecycleRxPkt
        .literal .Lsec_143_78, ppRecycleRxPkt
.Lsec_143_7c:
        .global ppRxFragmentProc
        .type   ppRxFragmentProc, @function
ppRxFragmentProc:
        entry    a1, 128
        l32r     a4, .Lsec_143_0
        l32i.n   a3, a4, 0
        beqz     a3, .Lsec_143_ec
        l32r     a3, .Lsec_143_4
        l8ui     a3, a3, 0
        beqz.n   a3, .Lsec_143_b3
        l32r     a3, .Lsec_143_8
        memw
        l8ui     a3, a3, 0
        extui    a3, a3, 0, 8
        bnez.n   a3, .Lsec_143_b3
        l32r     a3, .Lsec_143_c
        l32i.n   a3, a3, 0
        beqz.n   a3, .Lsec_143_b3
        l32r     a3, .Lsec_143_10
        l32i     a3, a3, 0
        l32i     a10, a3, 0x110
        callx8   a10
        j        .Lsec_143_c2
.Lsec_143_b3:
        l32r     a3, .Lsec_143_14
        memw
        l32i.n   a10, a3, 0
        l32r     a3, .Lsec_143_18
        l32i.n   a3, a3, 0
        add.n    a10, a10, a3
.Lsec_143_c2:
        l32i.n   a6, a4, 0
        l32r     a5, .Lsec_143_1c
        l32i.n   a3, a6, 44
        l32i.n   a5, a5, 16
        slli     a5, a5, 10
        l32i.n   a3, a3, 4
        sub      a3, a10, a3
        bgeu     a5, a3, .Lsec_143_ec
        movi.n   a3, 0
        mov.n    a10, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, ppRecycleRxPkt
        l32r     a8, .Lsec_143_38
        callx8   a8
        s32i.n   a3, a4, 0
        s16i     a3, a4, 4
        memw
        s32i.n   a3, a4, 8
        s32i.n   a3, a4, 12
.Lsec_143_ec:
        bnez.n   a2, .Lsec_143_f4
        memw
        j        .Lsec_143_4ba
.Lsec_143_f4:
        l32i.n   a3, a2, 4
        movi.n   a4, 0
        l32i.n   a9, a3, 4
        l32i.n   a3, a2, 36
        s32i.n   a4, a1, 56
        beq      a3, a4, .Lsec_143_106
        l8ui     a3, a3, 129
        s32i.n   a3, a1, 56
.Lsec_143_106:
        movi.n   a4, 4
        l32i.n   a10, a1, 56
        s32i     a9, a1, 72
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, ic_get_ptk_alg
        l32r     a8, .Lsec_143_3c
        callx8   a8
        mov.n    a6, a10
        l32i     a9, a1, 72
        l8ui     a3, a9, 0
        and      a3, a3, a4
        bnez     a3, .Lsec_143_4bc
        mov.n    a13, a3
        l8ui     a7, a9, 1
        and      a4, a7, a4
        l16ui    a5, a9, 22
        extui    a8, a5, 0, 4
        s32i.n   a4, a1, 48
        movi.n   a4, 1
        movnez   a13, a4, a8
        s32i.n   a5, a1, 52
        l32i.n   a5, a1, 48
        moveqz   a4, a3, a5
        or       a13, a13, a4
        s32i.n   a8, a1, 60
        beqz     a13, .Lsec_143_4bc
        mov.n    a10, a9
        l32r     a4, .Lsec_143_20
        l16ui    a3, a4, 176
        addi.n   a3, a3, 1
        s16i     a3, a4, 176
        memw
        l32i.n   a5, a2, 16
        s32i     a9, a1, 72
        s32i     a13, a1, 76
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_hdrsize
        l32r     a8, .Lsec_143_40
        callx8   a8
        extui    a4, a10, 0, 16
        l32i.n   a3, a2, 4
        l32i.n   a8, a3, 4
        movi.n   a3, 64
        and      a3, a7, a3
        l32i     a9, a1, 72
        l32i     a13, a1, 76
        beqz     a3, .Lsec_143_239
        extui    a7, a10, 0, 16
        movi.n   a10, 32
        l32i.n   a3, a5, 24
        add.n    a5, a9, a7
        extui    a3, a3, 0, 12
        l8ui     a5, a5, 3
        and      a5, a5, a10
        beqz     a5, .Lsec_143_224
        add.n    a10, a8, a7
        l8ui     a8, a10, 7
        slli     a8, a8, 8
        l8ui     a5, a10, 4
        slli     a5, a5, 16
        l8ui     a7, a10, 5
        slli     a7, a7, 24
        l8ui     a11, a10, 6
        or       a8, a8, a11
        bnei     a6, 2, .Lsec_143_1d0
        addi.n   a4, a4, 8
        or       a7, a5, a7
        l8ui     a11, a10, 2
        extui    a4, a4, 0, 16
        addi     a3, a3, -8
        sub      a3, a3, a4
        extui    a3, a3, 0, 16
        l8ui     a5, a10, 0
        or       a7, a11, a7
        slli     a5, a5, 8
        j        .Lsec_143_21d
.Lsec_143_1d0:
        bnei     a6, 4, .Lsec_143_200
        l8ui     a5, a10, 3
        addi     a4, a4, 18
        slli     a5, a5, 8
        extui    a4, a4, 0, 16
        addi     a3, a3, -20
        l8ui     a7, a10, 2
        sub      a3, a3, a4
        or       a5, a5, a7
        extui    a3, a3, 0, 16
        l8ui     a7, a10, 4
        slli     a7, a7, 16
        or       a7, a7, a5
        l8ui     a5, a10, 5
        slli     a5, a5, 24
        j        .Lsec_143_21d
.Lsec_143_200:
        addi.n   a4, a4, 8
        or       a5, a5, a7
        l8ui     a11, a10, 0
        extui    a4, a4, 0, 16
        addi     a3, a3, -12
        sub      a3, a3, a4
        extui    a3, a3, 0, 16
        l8ui     a7, a10, 1
        or       a5, a11, a5
        slli     a7, a7, 8
.Lsec_143_21d:
        or       a5, a5, a7
        j        .Lsec_143_268
        .byte    0x00
.Lsec_143_224:
        addi.n   a4, a4, 4
        extui    a4, a4, 0, 16
        addi     a3, a3, -8
        sub      a3, a3, a4
        extui    a3, a3, 0, 16
        mov.n    a13, a5
        mov.n    a8, a5
        j        .Lsec_143_268
.Lsec_143_239:
        bnez.n   a6, .Lsec_143_255
        l32i.n   a3, a5, 24
        mov.n    a13, a6
        extui    a3, a3, 0, 12
        addi     a3, a3, -4
        sub      a3, a3, a4
        extui    a3, a3, 0, 16
        mov.n    a5, a6
        mov.n    a8, a6
        j        .Lsec_143_268
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_143_255:
        l32r     a13, .Lsec_143_24
        movi.n   a12, 2
        movi.n   a11, 32
        movi.n   a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_143_44
        callx8   a8
        j        .Lsec_143_274
        .byte    0x00
.Lsec_143_268:
        l32r     a10, .Lsec_143_0
        l32i.n   a14, a10, 0
        bnez.n   a14, .Lsec_143_281
        l32i.n   a6, a1, 60
        beqz     a6, .Lsec_143_4a7
.Lsec_143_274:
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, ppRecycleRxPkt
        l32r     a8, .Lsec_143_48
        callx8   a8
        j        .Lsec_143_4ba
        .byte    0x00
        .byte    0x00
.Lsec_143_281:
        l32i.n   a7, a14, 4
        mov.n    a10, a9
        l32i.n   a7, a7, 4
        s32i     a9, a1, 72
        s32i     a8, a1, 80
        s32i     a13, a1, 76
        s32i     a14, a1, 68
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_gettid
        l32r     a8, .Lsec_143_4c
        callx8   a8
        mov.n    a11, a10
        mov.n    a10, a7
        s32i     a11, a1, 64
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_gettid
        l32r     a8, .Lsec_143_50
        callx8   a8
        l32i     a11, a1, 64
        l32i     a9, a1, 72
        bne      a11, a10, .Lsec_143_274
        addi.n   a10, a9, 4
        movi.n   a12, 12
        addi.n   a11, a7, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, memcmp
        l32r     a8, .Lsec_143_54
        callx8   a8
        l32i     a8, a1, 80
        l32i     a9, a1, 72
        l32i     a13, a1, 76
        l32i     a14, a1, 68
        bnez     a10, .Lsec_143_274
        l32i.n   a12, a1, 52
        l16ui    a10, a7, 22
        xor      a11, a10, a12
        movi.n   a12, -16
        and      a11, a11, a12
        bnez     a11, .Lsec_143_464
        l32i.n   a12, a1, 52
        addi.n   a10, a10, 1
        beq      a12, a10, .Lsec_143_2ea
        j        .Lsec_143_45c
.Lsec_143_2ea:
        beqz.n   a13, .Lsec_143_30c
        l32r     a10, .Lsec_143_0
        l32i.n   a13, a10, 8
        addi.n   a12, a13, 1
        l32i.n   a15, a10, 12
        movi     a10, 1
        bltu     a12, a13, .Lsec_143_2fe
        or       a10, a11, a11
.Lsec_143_2fe:
        add.n    a10, a10, a15
        beq      a12, a5, .Lsec_143_306
        j        .Lsec_143_468
.Lsec_143_306:
        beq      a10, a8, .Lsec_143_30c
        j        .Lsec_143_468
.Lsec_143_30c:
        l32r     a11, .Lsec_143_28
        l32r     a13, .Lsec_143_0
        l32i.n   a11, a11, 0
        l16ui    a10, a13, 4
        add.n    a12, a10, a3
        l32i     a11, a11, 0x3c4
        addi     a11, a11, -28
        bgeu     a12, a11, .Lsec_143_3a2
        add.n    a11, a9, a4
        l32i.n   a14, a14, 16
        or       a12, a3, a3
        add      a10, a7, a10
        s32i     a8, a1, 80
        s32i     a13, a1, 76
        s32i     a14, a1, 68
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_143_58
        callx8   a8
        movi.n   a11, -1
        slli     a11, a11, 12
        l32i     a13, a1, 76
        l16ui    a9, a13, 4
        add.n    a9, a3, a9
        l32i     a14, a1, 68
        s16i     a9, a13, 4
        memw
        l32i.n   a9, a14, 24
        add.n    a10, a9, a3
        extui    a10, a10, 0, 12
        and      a9, a9, a11
        or       a9, a9, a10
        l32i.n   a10, a14, 4
        s32i.n   a9, a14, 24
        extui    a9, a10, 8, 16
        add.n    a9, a3, a9
        extui    a11, a9, 0, 16
        slli     a9, a11, 8
        l32r     a11, .Lsec_143_2c
        and      a10, a10, a11
        or       a9, a10, a9
        s32i.n   a9, a14, 4
        l32i.n   a9, a14, 0
        extui    a10, a9, 16, 12
        add.n    a3, a3, a10
        extui    a10, a3, 0, 12
        slli     a3, a10, 16
        l32r     a10, .Lsec_143_30
        and      a9, a9, a10
        or       a3, a9, a3
        s32i.n   a3, a14, 0
        l32i.n   a3, a1, 52
        l32i     a8, a1, 80
        s16i     a3, a7, 22
        memw
        s32i.n   a5, a13, 8
        s32i.n   a8, a13, 12
.Lsec_143_3a2:
        mov.n    a10, a2
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, ppRecycleRxPkt
        l32r     a8, .Lsec_143_5c
        callx8   a8
        l32i.n   a5, a1, 48
        bnez     a5, .Lsec_143_4ba
        movi.n   a5, -16
        l32i.n   a8, a1, 52
        and      a5, a8, a5
        movi.n   a8, -5
        s16i     a5, a7, 22
        memw
        l8ui     a5, a7, 1
        and      a5, a5, a8
        l32r     a3, .Lsec_143_0
        l32i.n   a2, a3, 0
        s8i      a5, a7, 1
        memw
        bnei     a6, 2, .Lsec_143_44c
        movi.n   a12, 8
        addi     a10, a1, 40
        l32i.n   a5, a1, 48
        l16ui    a11, a3, 4
        addi     a11, a11, -8
        extui    a11, a11, 0, 16
        s32i.n   a5, a1, 32
        s32i.n   a5, a1, 36
        l32i.n   a5, a2, 4
        l32i     a5, a5, 4
        add      a11, a5, a11
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_143_60
        callx8   a8
        movi.n   a15, 32
        l32i.n   a12, a1, 48
        mov.n    a14, a1
        addi.n   a13, a5, 10
        mov.n    a11, a6
        l32i.n   a10, a1, 56
        .reloc  ., R_XTENSA_ASM_EXPAND, ic_obtain_key
        l32r     a8, .Lsec_143_64
        callx8   a8
        beqz     a10, .Lsec_143_470
        mov.n    a12, a4
        mov.n    a11, a2
        mov.n    a10, a1
        addi     a13, a1, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, ppCalTkipMic
        l32r     a8, .Lsec_143_68
        callx8   a8
        movi.n   a12, 8
        addi     a11, a1, 40
        addi     a10, a1, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, memcmp
        l32r     a8, .Lsec_143_6c
        callx8   a8
        beqz.n   a10, .Lsec_143_44c
        l32r     a2, .Lsec_143_34
        movi.n   a10, 1
        l32i.n   a3, a2, 0
        l32i.n   a6, a1, 56
        l32i.n   a2, a1, 48
        moveqz   a2, a10, a6
        beqz.n   a2, .Lsec_143_470
        l32i.n   a2, a1, 48
        movnez   a2, a10, a3
        beqz.n   a2, .Lsec_143_470
        callx8   a3
        j        .Lsec_143_470
        .byte    0x00
.Lsec_143_44c:
        movi.n   a4, 0
        s32i.n   a4, a3, 0
        s16i     a4, a3, 4
        memw
        memw
        j        .Lsec_143_489
.Lsec_143_45c:
        l32i.n   a8, a1, 52
        blt      a10, a8, .Lsec_143_468
        j        .Lsec_143_274
.Lsec_143_464:
        l32i.n   a6, a1, 60
        beqz.n   a6, .Lsec_143_496
.Lsec_143_468:
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, ppRecycleRxPkt
        l32r     a8, .Lsec_143_70
        callx8   a8
.Lsec_143_470:
        l32r     a3, .Lsec_143_0
        l32i.n   a10, a3, 0
        beqz.n   a10, .Lsec_143_47d
        .reloc  ., R_XTENSA_ASM_EXPAND, ppRecycleRxPkt
        l32r     a8, .Lsec_143_74
        callx8   a8
.Lsec_143_47d:
        movi.n   a2, 0
        s32i.n   a2, a3, 0
        mov.n    a4, a2
        s16i     a2, a3, 4
        memw
.Lsec_143_489:
        mov.n    a5, a4
        s32i.n   a4, a3, 8
        s32i.n   a4, a3, 12
        memw
        j        .Lsec_143_4bc
        .byte    0x00
.Lsec_143_496:
        mov.n    a10, a14
        s32i     a8, a1, 80
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, ppRecycleRxPkt
        l32r     a8, .Lsec_143_78
        callx8   a8
        l32i     a8, a1, 80
.Lsec_143_4a7:
        add.n    a4, a4, a3
        l32r     a6, .Lsec_143_0
        s32i.n   a2, a6, 0
        s16i     a4, a6, 4
        memw
        s32i     a5, a6, 8
        s32i     a8, a6, 12
.Lsec_143_4ba:
        movi.n   a2, 0
.Lsec_143_4bc:
        memw
        retw.n
        .size   ppRxFragmentProc, .-ppRxFragmentProc
.Lsec_143_4c1:

        .section .wifirxiram.28,"ax",@progbits
        .align  4
.Lsec_144_0:
        .global is_fragmented_pkt
        .type   is_fragmented_pkt, @function
is_fragmented_pkt:
        entry    a1, 32
        l32i.n   a2, a2, 4
        movi.n   a10, 4
        l32i.n   a9, a2, 4
        movi.n   a2, 0
        l8ui     a8, a9, 0
        bany     a8, a10, .Lsec_144_24
        movi.n   a2, 1
        l8ui     a8, a9, 1
        and      a8, a8, a10
        bnez.n   a8, .Lsec_144_24
        l16ui    a9, a9, 22
        extui    a9, a9, 0, 4
        moveqz   a2, a8, a9
.Lsec_144_24:
        retw.n
        .size   is_fragmented_pkt, .-is_fragmented_pkt
.Lsec_144_26:

        .section .text.ppGetTxQFirstAvail_Locked,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_145_0, pTxRx
.Lsec_145_4:
        .global ppGetTxQFirstAvail_Locked
        .type   ppGetTxQFirstAvail_Locked, @function
ppGetTxQFirstAvail_Locked:
        entry    a1, 32
        extui    a8, a2, 0, 8
        movi.n   a9, 15
        movi.n   a2, 0
        bltu     a9, a8, .Lsec_145_3c
        l32r     a9, .Lsec_145_0
        l32i.n   a10, a9, 0
        addi.n   a9, a8, 1
        addx2    a9, a9, a9
        slli     a9, a9, 4
        add.n    a9, a10, a9
        l8ui     a9, a9, 0
        bne      a9, a2, .Lsec_145_3c
        addx2    a8, a8, a8
        slli     a8, a8, 4
        add.n    a10, a10, a8
        l32i.n   a2, a10, 32
        beqz.n   a2, .Lsec_145_3c
        l32i.n   a8, a2, 44
        l32i.n   a8, a8, 0
        extui    a8, a8, 18, 1
        movnez   a2, a9, a8
.Lsec_145_3c:
        retw.n
        .size   ppGetTxQFirstAvail_Locked, .-ppGetTxQFirstAvail_Locked
.Lsec_145_3e:

        .section .text.ppFetchTxQFirstAvail,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_146_0, pTxRx
.Lsec_146_4:
        .global ppFetchTxQFirstAvail
        .type   ppFetchTxQFirstAvail, @function
ppFetchTxQFirstAvail:
        entry    a1, 32
        extui    a8, a2, 0, 8
        movi.n   a9, 15
        movi.n   a2, 0
        bltu     a9, a8, .Lsec_146_58
        l32r     a9, .Lsec_146_0
        l32i.n   a10, a9, 0
        addi.n   a9, a8, 1
        addx2    a9, a9, a9
        slli     a9, a9, 4
        add.n    a9, a10, a9
        l8ui     a11, a9, 0
        bne      a11, a2, .Lsec_146_58
        addx2    a9, a8, a8
        slli     a9, a9, 4
        add.n    a9, a10, a9
        l32i.n   a2, a9, 32
        beqz.n   a2, .Lsec_146_58
        l32i.n   a12, a2, 44
        l32i.n   a12, a12, 0
        bbsi     a12, 18, .Lsec_146_56
        l32i.n   a11, a2, 40
        s32i.n   a11, a9, 32
        bnez.n   a11, .Lsec_146_4c
        addx2    a8, a8, a8
        slli     a8, a8, 4
        addi     a8, a8, 32
        add.n    a10, a10, a8
        s32i.n   a10, a9, 36
.Lsec_146_4c:
        movi.n   a8, 0
        s32i.n   a8, a2, 40
        memw
        j        .Lsec_146_58
.Lsec_146_56:
        mov.n    a2, a11
.Lsec_146_58:
        retw.n
        .size   ppFetchTxQFirstAvail, .-ppFetchTxQFirstAvail
.Lsec_146_5a:

        .section .text.ppDequeueTxQ,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_147_0, pTxRx
.Lsec_147_4:
        .global ppDequeueTxQ
        .type   ppDequeueTxQ, @function
ppDequeueTxQ:
        entry    a1, 32
        extui    a8, a2, 0, 8
        movi.n   a9, 15
        movi.n   a2, 0
        bltu     a9, a8, .Lsec_147_39
        l32r     a2, .Lsec_147_0
        addx2    a9, a8, a8
        slli     a9, a9, 4
        l32i.n   a10, a2, 0
        add.n    a9, a10, a9
        l32i.n   a2, a9, 32
        beqz.n   a2, .Lsec_147_39
        l32i.n   a11, a2, 40
        s32i.n   a11, a9, 32
        bnez.n   a11, .Lsec_147_35
        addx2    a8, a8, a8
        slli     a8, a8, 4
        addi     a8, a8, 32
        add.n    a10, a10, a8
        s32i.n   a10, a9, 36
.Lsec_147_35:
        movi.n   a8, 0
        s32i.n   a8, a2, 40
.Lsec_147_39:
        memw
        retw.n
        .size   ppDequeueTxQ, .-ppDequeueTxQ
.Lsec_147_3e:

        .section .text.ppRollBackTxQ,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_148_0, pTxRx
.Lsec_148_4:
        .global ppRollBackTxQ
        .type   ppRollBackTxQ, @function
ppRollBackTxQ:
        entry    a1, 32
        l32r     a8, .Lsec_148_0
        l32i.n   a11, a8, 0
        l32i.n   a8, a2, 44
        l32i.n   a8, a8, 16
        extui    a8, a8, 20, 4
        addx2    a9, a8, a8
        slli     a9, a9, 4
        add.n    a9, a11, a9
        l32i.n   a12, a9, 32
        add.n    a10, a8, a8
        s32i.n   a12, a2, 40
        bnez.n   a12, .Lsec_148_28
        addi     a12, a2, 40
        s32i.n   a12, a9, 36
.Lsec_148_28:
        add.n    a8, a10, a8
        slli     a8, a8, 4
        add.n    a11, a11, a8
        s32i.n   a2, a11, 32
        memw
        retw.n
        .size   ppRollBackTxQ, .-ppRollBackTxQ
.Lsec_148_36:

        .section .text.ppSetBarRate,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_149_0, pTxRx
        .literal .Lsec_149_4, rcSetBarRate
.Lsec_149_8:
        .global ppSetBarRate
        .type   ppSetBarRate, @function
ppSetBarRate:
        entry    a1, 32
        l32r     a4, .Lsec_149_0
        extui    a2, a2, 0, 8
        movi     a3, 0
.Lsec_149_14:
        mov.n    a11, a2
        l32i.n   a8, a4, 0
        addx4    a8, a3, a8
        addi.n   a3, a3, 1
        l32i     a8, a8, 0x320
        l32i.n   a10, a8, 44
        .reloc  ., R_XTENSA_ASM_EXPAND, rcSetBarRate
        l32r     a8, .Lsec_149_4
        callx8   a8
        bnei     a3, 4, .Lsec_149_14
        retw.n
        .size   ppSetBarRate, .-ppSetBarRate
.Lsec_149_2d:

        .section .text.ppFillAMPDUBar,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_150_0, pTxRx
        .literal .Lsec_150_4, bars
        .literal .Lsec_150_8, 0xdfffffff
        .literal .Lsec_150_c, 0xff000fff
        .literal .Lsec_150_10, memcpy
        .literal .Lsec_150_14, memcpy
.Lsec_150_18:
        .global ppFillAMPDUBar
        .type   ppFillAMPDUBar, @function
ppFillAMPDUBar:
        entry    a1, 48
        extui    a7, a2, 0, 8
        mov.n    a11, a3
        movi.n   a8, 4
        extui    a3, a5, 0, 16
        bgeui    a7, 8, .Lsec_150_44
        beqz.n   a7, .Lsec_150_42
        beqi     a7, 3, .Lsec_150_42
        movi.n   a8, 3
        bltu     a7, a8, .Lsec_150_44
        movi.n   a8, 1
        bltui    a7, 6, .Lsec_150_39
        movi.n   a8, 0
.Lsec_150_39:
        extui    a8, a8, 0, 8
        j        .Lsec_150_44
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_150_42:
        movi.n   a8, 2
.Lsec_150_44:
        l32r     a2, .Lsec_150_0
        movi.n   a10, -16
        extui    a9, a7, 0, 4
        movi.n   a13, -1
        slli     a13, a13, 31
        l32i.n   a2, a2, 0
        slli     a3, a3, 4
        addx4    a2, a8, a2
        l32i     a2, a2, 0x320
        l32i.n   a6, a2, 44
        l32i.n   a5, a6, 4
        and      a5, a5, a10
        or       a5, a5, a9
        movi.n   a9, 1
        s32i.n   a5, a6, 4
        slli     a9, a9, 19
        l32i.n   a5, a6, 0
        or       a5, a5, a9
        s32i.n   a5, a6, 0
        addx8    a5, a8, a8
        slli     a5, a5, 2
        addi     a10, a5, 16
        l32r     a9, .Lsec_150_4
        add.n    a10, a10, a9
        l32i.n   a6, a2, 4
        s32i.n   a10, a6, 4
        memw
        l32i.n   a10, a6, 0
        or       a10, a10, a13
        movi.n   a13, 1
        slli     a13, a13, 30
        l16ui    a12, a2, 22
        extui    a12, a12, 0, 12
        slli     a12, a12, 12
        memw
        s32i.n   a10, a6, 0
        memw
        l32i.n   a10, a6, 0
        or       a10, a10, a13
        memw
        s32i.n   a10, a6, 0
        memw
        l32i.n   a10, a6, 0
        l32r     a13, .Lsec_150_8
        and      a10, a10, a13
        memw
        s32i.n   a10, a6, 0
        memw
        l32i.n   a10, a6, 0
        l32r     a13, .Lsec_150_c
        and      a10, a10, a13
        or       a10, a10, a12
        movi.n   a12, 6
        memw
        s32i.n   a10, a6, 0
        addx8    a6, a8, a8
        addx4    a6, a6, a9
        movi.n   a10, -9
        l8ui     a8, a6, 17
        and      a8, a8, a10
        addi     a10, a5, 20
        add.n    a10, a9, a10
        s8i      a8, a6, 17
        memw
        s32i     a9, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_150_10
        callx8   a8
        addi     a10, a5, 26
        mov.n    a11, a4
        movi.n   a12, 6
        l32i.n   a9, a1, 0
        movi.n   a4, 4
        add      a10, a9, a10
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_150_14
        callx8   a8
        slli     a8, a7, 12
        or       a8, a8, a4
        s16i     a8, a6, 32
        s16i     a3, a6, 34
        memw
        memw
        retw.n
        .size   ppFillAMPDUBar, .-ppFillAMPDUBar
.Lsec_150_120:

        .section .wifiextrairam.32,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_151_0, pTxRx
.Lsec_151_4:
        .global ppPauseTxAMPDU
        .type   ppPauseTxAMPDU, @function
ppPauseTxAMPDU:
        entry    a1, 32
        l32r     a8, .Lsec_151_0
        extui    a2, a2, 0, 8
        addx2    a2, a2, a2
        slli     a2, a2, 4
        l32i.n   a8, a8, 0
        add.n    a8, a8, a2
        movi.n   a2, 1
        s8i      a2, a8, 41
        memw
        memw
        retw.n
        .size   ppPauseTxAMPDU, .-ppPauseTxAMPDU
.Lsec_151_24:

        .section .wifi0iram.33,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_152_0, pTxRx
.Lsec_152_4:
        .global ppResumeTxAMPDU
        .type   ppResumeTxAMPDU, @function
ppResumeTxAMPDU:
        entry    a1, 32
        l32r     a8, .Lsec_152_0
        extui    a2, a2, 0, 8
        addx2    a2, a2, a2
        slli     a2, a2, 4
        l32i.n   a8, a8, 0
        add.n    a8, a8, a2
        movi.n   a2, 0
        s16i     a2, a8, 40
        memw
        memw
        retw.n
        .size   ppResumeTxAMPDU, .-ppResumeTxAMPDU
.Lsec_152_24:

        .section .wifi0iram.34,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_153_0, roundup2
.Lsec_153_4:
        .global ppCalSubFrameLength
        .type   ppCalSubFrameLength, @function
ppCalSubFrameLength:
        entry    a1, 32
        l32i.n   a2, a2, 4
        movi.n   a11, 4
        l32i.n   a2, a2, 4
        l32i.n   a10, a2, 0
        extui    a10, a10, 0, 12
        .reloc  ., R_XTENSA_ASM_EXPAND, roundup2
        l32r     a8, .Lsec_153_0
        callx8   a8
        l32i.n   a2, a2, 0
        extui    a2, a2, 12, 7
        addi.n   a2, a2, 1
        addx4    a2, a2, a10
        extui    a2, a2, 0, 16
        retw.n
        .size   ppCalSubFrameLength, .-ppCalSubFrameLength
.Lsec_153_27:

        .section .wifi0iram.35,"ax",@progbits
        .align  4
.Lsec_154_0:
        .global ppEmptyDelimiterLength
        .type   ppEmptyDelimiterLength, @function
ppEmptyDelimiterLength:
        entry    a1, 32
        l32i.n   a2, a2, 4
        l32i.n   a2, a2, 4
        l32i.n   a2, a2, 0
        extui    a8, a2, 0, 2
        beqz     a8, .Lsec_154_18
        movi     a9, 4
        sub      a8, a9, a8
        extui    a8, a8, 0, 8
.Lsec_154_18:
        extui    a2, a2, 12, 7
        addx4    a2, a2, a8
        retw.n
        .size   ppEmptyDelimiterLength, .-ppEmptyDelimiterLength
.Lsec_154_20:

        .section .text.ppCheckTxAMPDUlength,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_155_0, pTxRx
        .literal .Lsec_155_4, roundup2
.Lsec_155_8:
        .global ppCheckTxAMPDUlength
        .type   ppCheckTxAMPDUlength, @function
ppCheckTxAMPDUlength:
        entry    a1, 32
        l32i.n   a7, a5, 4
        movi     a11, 4
        extui    a4, a4, 0, 8
        l32i     a7, a7, 4
        l32i     a10, a7, 0
        extui    a10, a10, 0, 12
        .reloc  ., R_XTENSA_ASM_EXPAND, roundup2
        l32r     a8, .Lsec_155_4
        callx8   a8
        l32i.n   a5, a5, 44
        l32i.n   a8, a5, 16
        extui    a5, a10, 0, 16
        add.n    a3, a5, a3
        extui    a8, a8, 20, 4
        l16ui    a5, a2, 122
        bltu     a5, a3, .Lsec_155_3d
.Lsec_155_34:
        l16ui    a2, a2, 124
        bgeu     a2, a3, .Lsec_155_5d
        j        .Lsec_155_55
.Lsec_155_3d:
        l32r     a5, .Lsec_155_0
        addx2    a11, a8, a8
        slli     a11, a11, 4
        l32i.n   a9, a5, 0
        movi.n   a5, 1
        add.n    a9, a9, a11
        s8i      a5, a9, 76
        memw
        bnez     a6, .Lsec_155_34
.Lsec_155_55:
        movi.n   a2, 0
        memw
        j        .Lsec_155_86
.Lsec_155_5d:
        l32r     a2, .Lsec_155_0
        addx2    a8, a8, a8
        slli     a8, a8, 4
        l32i.n   a3, a2, 0
        add.n    a3, a3, a8
        l8ui     a2, a7, 3
        sub      a2, a2, a4
        extui    a2, a2, 0, 8
        l8ui     a3, a3, 47
        bgeu     a2, a3, .Lsec_155_55
        l32i.n   a2, a7, 0
        extui    a2, a2, 12, 7
        addi.n   a2, a2, 1
        addx4    a2, a2, a10
        extui    a2, a2, 0, 16
.Lsec_155_86:
        retw.n
        .size   ppCheckTxAMPDUlength, .-ppCheckTxAMPDUlength
.Lsec_155_88:

        .section .wifiextrairam.36,"ax",@progbits
        .align  4
.Lsec_156_0:
        .global ppAdd2AMPDUTail
        .type   ppAdd2AMPDUTail, @function
ppAdd2AMPDUTail:
        entry    a1, 32
        s32i.n   a3, a2, 40
        l32i.n   a8, a3, 4
        l32i.n   a2, a2, 8
        s32i.n   a8, a2, 8
        movi.n   a2, 0
        s32i.n   a2, a3, 40
        memw
        retw.n
        .size   ppAdd2AMPDUTail, .-ppAdd2AMPDUTail
.Lsec_156_14:

        .section .wifi0iram.37,"ax",@progbits
        .align  4
.Lsec_157_0:
        .global ppSubFromAMPDU
        .type   ppSubFromAMPDU, @function
ppSubFromAMPDU:
        entry    a1, 32
        l32i.n   a9, a3, 8
        l32i.n   a8, a2, 8
        l32i.n   a10, a9, 8
        s32i.n   a10, a8, 8
        movi.n   a8, 0
        s32i.n   a8, a9, 8
        l32i.n   a9, a3, 40
        s32i.n   a9, a2, 40
        s32i.n   a8, a3, 40
        memw
        retw.n
        .size   ppSubFromAMPDU, .-ppSubFromAMPDU
.Lsec_157_1a:

        .section .text.ppReSendBar,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_158_0, pTxRx
        .literal .Lsec_158_4, 0xff0fffff
        .literal .Lsec_158_8, g_mesh_is_started
        .literal .Lsec_158_c, g_mesh_is_root
        .literal .Lsec_158_10, g_mesh_init_ps_type
        .literal .Lsec_158_14, g_osi_funcs_p
        .literal .Lsec_158_18, 0x3ff73c00
        .literal .Lsec_158_1c, g_wifi_mac_time_delta
        .literal .Lsec_158_20, bars
        .literal .Lsec_158_24, 0xdfffffff
        .literal .Lsec_158_28, 0xff000fff
        .literal .Lsec_158_2c, rcGetRate
        .literal .Lsec_158_30, lmacTxFrame
.Lsec_158_34:
        .global ppReSendBar
        .type   ppReSendBar, @function
ppReSendBar:
        entry    a1, 32
        extui    a2, a2, 0, 8
        extui    a3, a3, 0, 8
        movi.n   a4, 4
        bgeui    a2, 8, .Lsec_158_5c
        beqz.n   a2, .Lsec_158_5a
        beqi     a2, 3, .Lsec_158_5a
        movi.n   a4, 3
        bltu     a2, a4, .Lsec_158_5c
        movi.n   a4, 1
        bltui    a2, 6, .Lsec_158_54
        movi     a4, 0
.Lsec_158_54:
        extui    a4, a4, 0, 8
        j        .Lsec_158_5c
.Lsec_158_5a:
        movi.n   a4, 2
.Lsec_158_5c:
        l32r     a2, .Lsec_158_0
        extui    a3, a3, 0, 4
        slli     a3, a3, 20
        l32i.n   a2, a2, 0
        addx4    a2, a4, a2
        l32i     a2, a2, 0x320
        l32i.n   a9, a2, 44
        l8ui     a8, a9, 4
        l32r     a10, .Lsec_158_4
        s32i.n   a8, a9, 4
        l32i.n   a8, a9, 16
        and      a8, a8, a10
        or       a8, a8, a3
        l32r     a3, .Lsec_158_8
        s32i.n   a8, a9, 16
        l8ui     a3, a3, 0
        beqz.n   a3, .Lsec_158_af
        l32r     a3, .Lsec_158_c
        memw
        l8ui     a3, a3, 0
        extui    a3, a3, 0, 8
        bnez.n   a3, .Lsec_158_af
        l32r     a3, .Lsec_158_10
        l32i.n   a3, a3, 0
        beqz.n   a3, .Lsec_158_af
        l32r     a3, .Lsec_158_14
        l32i.n   a3, a3, 0
        l32i     a10, a3, 0x110
        memw
        callx8   a10
        j        .Lsec_158_be
.Lsec_158_af:
        l32r     a3, .Lsec_158_18
        memw
        l32i.n   a10, a3, 0
        l32r     a3, .Lsec_158_1c
        l32i.n   a3, a3, 0
        add.n    a10, a10, a3
.Lsec_158_be:
        addx8    a4, a4, a4
        slli     a4, a4, 2
        l32i.n   a11, a2, 44
        addi     a4, a4, 16
        s32i.n   a10, a11, 24
        l32i.n   a10, a2, 36
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, rcGetRate
        l32r     a8, .Lsec_158_2c
        callx8   a8
        mov.n    a10, a2
        l32r     a3, .Lsec_158_20
        add.n    a4, a4, a3
        l32i.n   a8, a2, 4
        memw
        l32i.n   a3, a8, 0
        s32i.n   a4, a8, 4
        movi.n   a4, -1
        slli     a4, a4, 31
        or       a3, a3, a4
        movi.n   a4, 1
        l16ui    a9, a2, 22
        slli     a4, a4, 30
        extui    a9, a9, 0, 12
        slli     a9, a9, 12
        memw
        s32i.n   a3, a8, 0
        memw
        l32i.n   a3, a8, 0
        or       a3, a3, a4
        memw
        s32i.n   a3, a8, 0
        memw
        l32i.n   a3, a8, 0
        l32r     a4, .Lsec_158_24
        and      a3, a3, a4
        memw
        s32i.n   a3, a8, 0
        memw
        l32i.n   a4, a8, 0
        l32r     a3, .Lsec_158_28
        and      a4, a4, a3
        or       a4, a4, a9
        memw
        s32i.n   a4, a8, 0
        l32i.n   a3, a2, 44
        l32i.n   a11, a3, 4
        extui    a11, a11, 4, 4
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacTxFrame
        l32r     a8, .Lsec_158_30
        callx8   a8
        retw.n
        .size   ppReSendBar, .-ppReSendBar
.Lsec_158_143:

        .section .text.ppRecordBarRRC,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_159_0, pTxRx
.Lsec_159_4:
        .global ppRecordBarRRC
        .type   ppRecordBarRRC, @function
ppRecordBarRRC:
        entry    a1, 32
        l32r     a8, .Lsec_159_0
        extui    a2, a2, 0, 8
        addx2    a2, a2, a2
        slli     a2, a2, 4
        l32i.n   a8, a8, 0
        add.n    a8, a8, a2
        s8i      a3, a8, 40
        memw
        memw
        retw.n
        .size   ppRecordBarRRC, .-ppRecordBarRRC
.Lsec_159_22:

        .section .wifiextrairam.43,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_160_0, pTxRx
.Lsec_160_4:
        .global ppTxqUpdateBitmap
        .type   ppTxqUpdateBitmap, @function
ppTxqUpdateBitmap:
        entry    a1, 32
        l32r     a8, .Lsec_160_0
        extui    a2, a2, 0, 8
        l32i.n   a9, a8, 0
        addx2    a8, a2, a2
        slli     a8, a8, 4
        add.n    a8, a9, a8
        l8ui     a2, a8, 40
        add.n    a6, a6, a2
        s16i     a3, a8, 42
        s32i.n   a5, a8, 56
        s32i.n   a4, a8, 60
        s8i      a6, a8, 40
        memw
        memw
        retw.n
        .size   ppTxqUpdateBitmap, .-ppTxqUpdateBitmap
.Lsec_160_2e:

        .section .wifiextrairam.47,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_161_0, .Lsec_117_0
        .literal .Lsec_161_4, .Lsec_219_0
        .literal .Lsec_161_8, pTxRx
        .literal .Lsec_161_c, wifi_log
.Lsec_161_10:
        .global ppEnqueueTxDone
        .type   ppEnqueueTxDone, @function
ppEnqueueTxDone:
        entry    a1, 32
        bnez.n   a2, .Lsec_161_34
        movi     a15, 111
        movi     a12, 1
        l32r     a14, .Lsec_161_0
        slli     a15, a15, 5
        slli     a11, a12, 11
        movi     a10, 6
        l32r     a13, .Lsec_161_4
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_161_c
        callx8   a8
.Lsec_161_30:
        j        .Lsec_161_30
        .byte    0x00
.Lsec_161_34:
        l32i.n   a9, a2, 44
        movi.n   a10, 1
        slli     a10, a10, 26
        l32i.n   a9, a9, 0
        and      a9, a9, a10
        l32r     a8, .Lsec_161_8
        l32i.n   a8, a8, 0
        beqz.n   a9, .Lsec_161_58
        l8ui     a2, a8, 31
        addi.n   a2, a2, -1
        s8i      a2, a8, 31
        memw
        memw
        j        .Lsec_161_65
.Lsec_161_58:
        s32i.n   a9, a2, 40
        l32i     a9, a8, 0x350
        s32i.n   a2, a9, 0
        addi     a2, a2, 40
        s32i     a2, a8, 0x350
.Lsec_161_65:
        memw
        retw.n
        .size   ppEnqueueTxDone, .-ppEnqueueTxDone
.Lsec_161_6a:

        .section .wifiextrairam.49,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_162_0, pTxRx
.Lsec_162_4:
        .global ppDequeueTxDone_Locked
        .type   ppDequeueTxDone_Locked, @function
ppDequeueTxDone_Locked:
        entry    a1, 32
        l32r     a2, .Lsec_162_0
        l32i.n   a8, a2, 0
        l32i     a2, a8, 0x34c
        beqz.n   a2, .Lsec_162_24
        l32i.n   a9, a2, 40
        s32i     a9, a8, 0x34c
        bnez.n   a9, .Lsec_162_20
        movi     a9, 0x34c
        add.n    a9, a8, a9
        s32i     a9, a8, 0x350
.Lsec_162_20:
        movi.n   a8, 0
        s32i.n   a8, a2, 40
.Lsec_162_24:
        memw
        retw.n
        .size   ppDequeueTxDone_Locked, .-ppDequeueTxDone_Locked
.Lsec_162_29:

        .section .wifiextrairam.20,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_163_0, g_mesh_is_started
        .literal .Lsec_163_4, 0x00080008
        .literal .Lsec_163_8, .Lsec_258_0
        .literal .Lsec_163_c, 0xff000fff
        .literal .Lsec_163_10, 0xff7fffff
        .literal .Lsec_163_14, .Lsec_228_0
        .literal .Lsec_163_18, ppProcTxCallback
        .literal .Lsec_163_1c, pp_coex_tx_release
        .literal .Lsec_163_20, trc_onPPTxDone
        .literal .Lsec_163_24, wifi_log
        .literal .Lsec_163_28, esf_buf_recycle
        .literal .Lsec_163_2c, ppDequeueTxDone_Locked
        .literal .Lsec_163_30, pp_process_hmac_waiting_txq
        .literal .Lsec_163_34, pm_on_data_tx_done
.Lsec_163_38:
        .global ppProcTxDone
        .type   ppProcTxDone, @function
ppProcTxDone:
        entry    a1, 48
        movi.n   a2, 0
        mov.n    a4, a2
        s32i.n   a2, a1, 0
        mov.n    a3, a2
        movi     a6, 255
        movi.n   a5, 1
        memw
        j        .Lsec_163_148
.Lsec_163_4e:
        movi.n   a11, 0
        mov.n    a10, a7
        .reloc  ., R_XTENSA_ASM_EXPAND, ppProcTxCallback
        l32r     a8, .Lsec_163_18
        callx8   a8
        mov.n    a10, a7
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_coex_tx_release
        l32r     a8, .Lsec_163_1c
        callx8   a8
        l32i.n   a11, a7, 44
        l32i.n   a9, a11, 0
        bbci     a9, 13, .Lsec_163_db
        l32i.n   a10, a7, 36
        beqz     a10, .Lsec_163_b0
        l8ui     a12, a10, 11
        bbsi     a12, 7, .Lsec_163_b0
        l32i.n   a12, a11, 16
        extui    a6, a12, 18, 2
        beqz     a6, .Lsec_163_16c
        j        .Lsec_163_9e
.Lsec_163_7d:
        l32r     a3, .Lsec_163_0
        l8ui     a3, a3, 0
        beqz.n   a3, .Lsec_163_9a
        movi.n   a4, 15
        slli     a4, a4, 8
        and      a12, a12, a4
        moveqz   a11, a6, a12
        s32i.n   a11, a1, 0
        mov.n    a4, a3
        memw
        j        .Lsec_163_9e
.Lsec_163_9a:
        mov.n    a4, a5
        mov.n    a3, a11
.Lsec_163_9e:
        l32r     a11, .Lsec_163_4
        and      a9, a9, a11
        bnei     a9, 8, .Lsec_163_b0
        or       a11, a7, a7
        .reloc  ., R_XTENSA_ASM_EXPAND, trc_onPPTxDone
        l32r     a8, .Lsec_163_20
        callx8   a8
.Lsec_163_b0:
        l32r     a9, .Lsec_163_8
        l32i.n   a9, a9, 0
        beqz     a9, .Lsec_163_138
        l32i.n   a10, a7, 44
        l32i.n   a11, a7, 4
        l32i.n   a12, a10, 0
        l32i.n   a11, a11, 4
        bbci     a12, 18, .Lsec_163_c6
        addi     a11, a11, 4
.Lsec_163_c6:
        movi.n   a13, 0
        addi     a12, a7, 22
        l8ui     a10, a10, 19
        addi.n   a10, a10, -1
        moveqz   a13, a5, a10
        mov.n    a10, a6
        callx8   a9
        j        .Lsec_163_138
.Lsec_163_db:
        bbci     a9, 23, .Lsec_163_120
        l16ui    a10, a7, 22
        addi     a10, a10, -4
        s16i     a10, a7, 22
        memw
        l32i.n   a10, a7, 8
        memw
        l32i.n   a8, a10, 0
        extui    a8, a8, 12, 12
        addi     a8, a8, -4
        extui    a8, a8, 0, 12
        memw
        l32i.n   a12, a10, 0
        slli     a8, a8, 12
        l32r     a13, .Lsec_163_c
        and      a12, a12, a13
        or       a8, a12, a8
        memw
        s32i.n   a8, a10, 0
        l32r     a8, .Lsec_163_10
        and      a8, a9, a8
        s32i.n   a8, a11, 0
        memw
        j        .Lsec_163_148
        .byte    0x00
        .byte    0x00
.Lsec_163_120:
        l8ui     a9, a7, 26
        bnei     a9, 5, .Lsec_163_138
        movi.n   a12, 3
        movi.n   a11, 64
        l32r     a13, .Lsec_163_14
        movi.n   a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_163_24
        callx8   a8
        j        .Lsec_163_148
.Lsec_163_138:
        l8ui     a9, a7, 26
        mov.n    a10, a7
        addi.n   a9, a9, -1
        moveqz   a2, a5, a9
        .reloc  ., R_XTENSA_ASM_EXPAND, esf_buf_recycle
        l32r     a8, .Lsec_163_28
        callx8   a8
.Lsec_163_148:
        .reloc  ., R_XTENSA_ASM_EXPAND, ppDequeueTxDone_Locked
        l32r     a8, .Lsec_163_2c
        callx8   a8
        mov.n    a7, a10
        bnez     a10, .Lsec_163_4e
        beqz     a2, .Lsec_163_15c
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_process_hmac_waiting_txq
        l32r     a8, .Lsec_163_30
        callx8   a8
.Lsec_163_15c:
        l32i.n   a11, a1, 0
        mov.n    a12, a4
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_on_data_tx_done
        l32r     a8, .Lsec_163_34
        callx8   a8
        retw.n
        .byte    0x00
        .byte    0x00
.Lsec_163_16c:
        l8ui     a11, a11, 19
        mov.n    a4, a5
        beqi     a11, 1, .Lsec_163_177
        j        .Lsec_163_9e
.Lsec_163_177:
        j        .Lsec_163_7d
        .size   ppProcTxDone, .-ppProcTxDone
.Lsec_163_17a:

        .section .wifislprxiram.50,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_164_0, pTxRx
.Lsec_164_4:
        .global ppEnqueueRxq
        .type   ppEnqueueRxq, @function
ppEnqueueRxq:
        entry    a1, 32
        movi.n   a8, 0
        s32i.n   a8, a2, 40
        l32r     a8, .Lsec_164_0
        l32i.n   a8, a8, 0
        l32i     a9, a8, 0x358
        s32i.n   a2, a9, 0
        addi     a2, a2, 40
        s32i     a2, a8, 0x358
        memw
        retw.n
        .size   ppEnqueueRxq, .-ppEnqueueRxq
.Lsec_164_20:

        .section .wifislprxiram.51,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_165_0, pTxRx
.Lsec_165_4:
        .global ppDequeueRxq_Locked
        .type   ppDequeueRxq_Locked, @function
ppDequeueRxq_Locked:
        entry    a1, 32
        l32r     a2, .Lsec_165_0
        l32i.n   a8, a2, 0
        l32i     a2, a8, 0x354
        beqz.n   a2, .Lsec_165_24
        l32i.n   a9, a2, 40
        s32i     a9, a8, 0x354
        bnez.n   a9, .Lsec_165_20
        movi     a9, 0x354
        add.n    a9, a8, a9
        s32i     a9, a8, 0x358
.Lsec_165_20:
        movi.n   a8, 0
        s32i.n   a8, a2, 40
.Lsec_165_24:
        memw
        retw.n
        .size   ppDequeueRxq_Locked, .-ppDequeueRxq_Locked
.Lsec_165_29:

        .section .wifislprxiram.29,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_166_0, pTxRx
        .literal .Lsec_166_4, .Lsec_251_0
        .literal .Lsec_166_8, .Lsec_224_0
        .literal .Lsec_166_c, wDevCheckBlockError
        .literal .Lsec_166_10, ppRxProtoProc
        .literal .Lsec_166_14, ppRxFragmentProc
        .literal .Lsec_166_18, ppRxProtoProc
        .literal .Lsec_166_1c, is_fragmented_pkt
        .literal .Lsec_166_20, wifi_log
        .literal .Lsec_166_24, ppRecycleRxPkt
        .literal .Lsec_166_28, ppDequeueRxq_Locked
.Lsec_166_2c:
        .global ppRxPkt
        .type   ppRxPkt, @function
ppRxPkt:
        entry    a1, 32
        movi.n   a3, 1
        l32r     a6, .Lsec_166_0
        movi.n   a4, 16
        slli     a5, a3, 29
        j        .Lsec_166_13c
.Lsec_166_3c:
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, wDevCheckBlockError
        l32r     a8, .Lsec_166_c
        callx8   a8
        l32i.n   a7, a2, 16
        mov.n    a10, a2
        addi     a9, a7, 28
        mov.n    a11, a7
        l32i.n   a8, a2, 4
        s32i.n   a9, a8, 4
        l32i.n   a8, a2, 44
        l32i.n   a8, a8, 0
        and      a8, a8, a4
        bbsi     a8, 4, .Lsec_166_78
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, ppRxProtoProc
        l32r     a8, .Lsec_166_10
        callx8   a8
        beqz.n   a10, .Lsec_166_69
        j        .Lsec_166_134
.Lsec_166_69:
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, ppRxFragmentProc
        l32r     a8, .Lsec_166_14
        callx8   a8
        mov.n    a2, a10
        bnez.n   a10, .Lsec_166_91
        j        .Lsec_166_13c
.Lsec_166_78:
        .reloc  ., R_XTENSA_ASM_EXPAND, ppRxProtoProc
        l32r     a8, .Lsec_166_18
        callx8   a8
        beqz.n   a10, .Lsec_166_84
        j        .Lsec_166_134
        .byte    0x00
.Lsec_166_84:
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, is_fragmented_pkt
        l32r     a8, .Lsec_166_1c
        callx8   a8
        beqz.n   a10, .Lsec_166_91
        j        .Lsec_166_134
.Lsec_166_91:
        l32i.n   a8, a7, 0
        movi.n   a12, 1
        slli     a12, a12, 28
        and      a12, a8, a12
        beqz     a12, .Lsec_166_100
        l8ui     a8, a7, 27
        beqz.n   a8, .Lsec_166_ad
        movi     a9, -245
        add.n    a9, a8, a9
        beqz.n   a9, .Lsec_166_ad
        j        .Lsec_166_134
.Lsec_166_ad:
        l32r     a9, .Lsec_166_0
        l32i.n   a9, a9, 0
        l32i     a9, a9, 0x3b0
        beqz     a9, .Lsec_166_134
        movi     a10, 245
        bne      a8, a10, .Lsec_166_ed
        l32r     a8, .Lsec_166_4
        l32i.n   a8, a8, 0
        beqz.n   a8, .Lsec_166_d9
        movi     a10, -1
        l32i     a7, a7, 8
        xor      a10, a10, a7
        extui    a10, a10, 31, 1
        callx8   a8
        j        .Lsec_166_134
        .byte    0x00
        .byte    0x00
.Lsec_166_d9:
        l32r     a13, .Lsec_166_8
        mov.n    a12, a3
        movi.n   a11, 32
        movi.n   a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_166_20
        callx8   a8
        j        .Lsec_166_134
        .byte    0x00
        .byte    0x00
.Lsec_166_ed:
        l32i.n   a7, a2, 16
        movi.n   a12, 0
        mov.n    a10, a2
        l8ui     a11, a7, 0
        sext     a11, a11, 7
        callx8   a9
        j        .Lsec_166_13c
        .byte    0x00
.Lsec_166_100:
        and      a9, a8, a5
        beqz.n   a9, .Lsec_166_118
        l32r     a7, .Lsec_166_0
        l32i.n   a7, a7, 0
        l32i     a7, a7, 0x3b4
        beqz.n   a7, .Lsec_166_134
        l32i.n   a8, a2, 16
        l8ui     a11, a8, 0
        j        .Lsec_166_129
        .byte    0x00
.Lsec_166_118:
        bbci     a8, 30, .Lsec_166_134
        l32i.n   a7, a6, 0
        l32i     a7, a7, 0x3b8
        beqz.n   a7, .Lsec_166_134
        mov.n    a12, a9
        l32i.n   a8, a2, 16
        l8ui     a11, a8, 0
.Lsec_166_129:
        sext     a11, a11, 7
        mov.n    a10, a2
        callx8   a7
        j        .Lsec_166_13c
.Lsec_166_134:
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, ppRecycleRxPkt
        l32r     a8, .Lsec_166_24
        callx8   a8
.Lsec_166_13c:
        .reloc  ., R_XTENSA_ASM_EXPAND, ppDequeueRxq_Locked
        l32r     a8, .Lsec_166_28
        callx8   a8
        mov.n    a2, a10
        bnez     a10, .Lsec_166_3c
        retw.n
        .size   ppRxPkt, .-ppRxPkt
.Lsec_166_149:

        .section .text.emul_timeout,"ax",@progbits
        .align  4
.Lsec_167_0:
        .global emul_timeout
        .type   emul_timeout, @function
emul_timeout:
        entry    a1, 32
        retw.n
        .size   emul_timeout, .-emul_timeout
.Lsec_167_5:

        .section .rodata.pp_create_task.str1.1,"aMS",@progbits,1
        .align  1
.Lsec_168_0:
        .ascii  "wifi\000"

        .section .text.pp_create_task,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_169_0, g_wifi_menuconfig
        .literal .Lsec_169_4, g_osi_funcs_p
        .literal .Lsec_169_8, .Lsec_254_0
        .literal .Lsec_169_c, .Lsec_218_0
        .literal .Lsec_169_10, s_wifi_queue
        .literal .Lsec_169_14, .Lsec_217_0
        .literal .Lsec_169_18, .Lsec_216_0
        .literal .Lsec_169_1c, xphyQueue
        .literal .Lsec_169_20, .Lsec_215_0
        .literal .Lsec_169_24, .Lsec_252_0
        .literal .Lsec_169_28, .Lsec_214_0
        .literal .Lsec_169_2c, .Lsec_168_0
        .literal .Lsec_169_30, ppTask
        .literal .Lsec_169_34, .Lsec_213_0
        .literal .Lsec_169_38, .Lsec_212_0
        .literal .Lsec_169_3c, wifi_log
        .literal .Lsec_169_40, wifi_log
        .literal .Lsec_169_44, wifi_log
        .literal .Lsec_169_48, config_get_wifi_task_stack_size
        .literal .Lsec_169_4c, wifi_log
        .literal .Lsec_169_50, config_get_wifi_task_stack_size
        .literal .Lsec_169_54, wifi_log
.Lsec_169_58:
        .global pp_create_task
        .type   pp_create_task, @function
pp_create_task:
        entry    a1, 64
        l32r     a2, .Lsec_169_0
        l32i.n   a6, a2, 52
        l32r     a2, .Lsec_169_4
        l32i     a3, a2, 0
        l32i     a10, a3, 168
        callx8   a10
        mov.n    a5, a10
        l32r     a3, .Lsec_169_8
        l32i.n   a4, a3, 0
        beqz.n   a4, .Lsec_169_90
        movi.n   a10, 6
        movi     a12, 2
        l32r     a13, .Lsec_169_c
        movi     a11, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_169_3c
        callx8   a8
        l32i.n   a4, a2, 0
        l32i.n   a10, a3, 0
        l32i     a4, a4, 152
        callx8   a4
.Lsec_169_90:
        l32r     a4, .Lsec_169_10
        l32i.n   a7, a4, 0
        beqz.n   a7, .Lsec_169_b1
        movi.n   a10, 6
        movi.n   a12, 2
        l32r     a13, .Lsec_169_14
        movi.n   a11, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_169_40
        callx8   a8
        l32i.n   a7, a2, 0
        l32i     a10, a4, 0
        l32i     a7, a7, 0x180
        callx8   a7
.Lsec_169_b1:
        movi.n   a11, 8
        movi     a10, 200
        l32i.n   a7, a2, 0
        l32i     a7, a7, 0x17c
        callx8   a7
        s32i.n   a10, a4, 0
        bnez.n   a10, .Lsec_169_d9
        movi.n   a12, 1
        l32r     a13, .Lsec_169_18
        mov.n    a11, a12
        movi.n   a10, 6
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_169_44
        callx8   a8
        j        .Lsec_169_1ca
        .byte    0x00
        .byte    0x00
.Lsec_169_d9:
        l32i.n   a4, a10, 0
        l32r     a7, .Lsec_169_1c
        s32i.n   a4, a7, 0
        l32r     a13, .Lsec_169_20
        beqz     a4, .Lsec_169_13c
        movi.n   a11, 0
        movi.n   a10, 1
        l32i.n   a4, a2, 0
        l32i     a4, a4, 52
        memw
        callx8   a4
        l32r     a4, .Lsec_169_24
        s32i.n   a10, a4, 0
        l32r     a13, .Lsec_169_28
        beqz.n   a10, .Lsec_169_13c
        l32i     a7, a2, 0
        l32i     a7, a7, 144
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, config_get_wifi_task_stack_size
        l32r     a8, .Lsec_169_48
        callx8   a8
        mov.n    a12, a10
        l32i.n   a8, a2, 0
        l32i     a10, a8, 168
        s32i.n   a12, a1, 16
        memw
        callx8   a10
        addi     a14, a10, -2
        mov.n    a15, a3
        movi.n   a13, 0
        l32i.n   a12, a1, 16
        l32r     a11, .Lsec_169_2c
        l32r     a10, .Lsec_169_30
        s32i.n   a6, a1, 0
        memw
        callx8   a7
        mov.n    a7, a10
        beqi     a10, 1, .Lsec_169_14d
        l32r     a13, .Lsec_169_34
.Lsec_169_13c:
        movi.n   a12, 1
        slli     a11, a12, 11
        movi.n   a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_169_4c
        callx8   a8
        j        .Lsec_169_19f
        .byte    0x00
.Lsec_169_14d:
        l32i.n   a8, a2, 0
        movi.n   a11, -1
        l32i.n   a8, a8, 60
        l32i.n   a10, a4, 0
        callx8   a8
        mov.n    a10, a7
        l32i.n   a8, a2, 0
        l32i     a8, a8, 156
        callx8   a8
        l32i.n   a2, a2, 0
        l32i     a10, a4, 0
        l32i     a2, a2, 56
        callx8   a2
        movi.n   a2, 0
        l32i.n   a3, a3, 0
        s32i.n   a2, a4, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, config_get_wifi_task_stack_size
        l32r     a8, .Lsec_169_50
        callx8   a8
        addi     a15, a5, -2
        extui    a15, a15, 0, 8
        mov.n    a14, a3
        l32r     a13, .Lsec_169_38
        movi.n   a12, 3
        mov.n    a11, a7
        s32i.n   a10, a1, 0
        movi     a10, 6
        s32i     a6, a1, 4
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_169_54
        callx8   a8
        j        .Lsec_169_1cd
.Lsec_169_19f:
        l32r     a4, .Lsec_169_10
        l32i     a10, a4, 0
        beqz     a10, .Lsec_169_1b5
        l32i     a5, a2, 0
        l32i     a5, a5, 0x180
        callx8   a5
        movi.n   a5, 0
        s32i.n   a5, a4, 0
.Lsec_169_1b5:
        l32r     a4, .Lsec_169_24
        l32i.n   a10, a4, 0
        beqz.n   a10, .Lsec_169_1c6
        l32i.n   a2, a2, 0
        l32i.n   a2, a2, 56
        memw
        callx8   a2
.Lsec_169_1c6:
        movi.n   a2, 0
        s32i.n   a2, a3, 0
.Lsec_169_1ca:
        movi     a2, 0x101
.Lsec_169_1cd:
        memw
        retw.n
        .size   pp_create_task, .-pp_create_task
.Lsec_169_1d2:

        .section .text.pp_deattach,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_170_0, pTxRx
        .literal .Lsec_170_4, esf_buf_recycle
        .literal .Lsec_170_8, esf_buf_setdown
.Lsec_170_c:
        .global pp_deattach
        .type   pp_deattach, @function
pp_deattach:
        entry    a1, 32
        l32r     a3, .Lsec_170_0
        movi     a2, 0
.Lsec_170_15:
        movi     a8, 200
        add.n    a8, a2, a8
        slli     a4, a8, 2
        l32i.n   a9, a3, 0
        addx4    a8, a8, a9
        l32i.n   a10, a8, 0
        beqz.n   a10, .Lsec_170_34
        .reloc  ., R_XTENSA_ASM_EXPAND, esf_buf_recycle
        l32r     a8, .Lsec_170_4
        callx8   a8
        movi.n   a9, 0
        l32i.n   a8, a3, 0
        add.n    a8, a8, a4
        s32i.n   a9, a8, 0
.Lsec_170_34:
        addi.n   a2, a2, 1
        bnei     a2, 4, .Lsec_170_15
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, esf_buf_setdown
        l32r     a8, .Lsec_170_8
        callx8   a8
        retw.n
        .size   pp_deattach, .-pp_deattach
.Lsec_170_44:

        .section .wifiextrairam.59,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_171_0, pTxRx
        .literal .Lsec_171_4, GetAccess
        .literal .Lsec_171_8, lmacAdjustTimestamp
.Lsec_171_c:
        .global ppGetTxframe
        .type   ppGetTxframe, @function
ppGetTxframe:
        entry    a1, 32
        l32r     a4, .Lsec_171_0
        extui    a3, a2, 0, 8
        addx2    a9, a3, a3
        slli     a9, a9, 4
        add.n    a2, a3, a3
        l32i.n   a8, a4, 0
        movi.n   a10, 4
        add.n    a8, a8, a9
        l8ui     a8, a8, 44
        bgeui    a8, 8, .Lsec_171_40
        beqz.n   a8, .Lsec_171_3e
        beqi     a8, 3, .Lsec_171_3e
        movi.n   a10, 3
        bltu     a8, a10, .Lsec_171_40
        movi.n   a10, 1
        bltui    a8, 6, .Lsec_171_40
        movi.n   a10, 0
        j        .Lsec_171_40
        .byte    0x00
.Lsec_171_3e:
        movi.n   a10, 2
.Lsec_171_40:
        .reloc  ., R_XTENSA_ASM_EXPAND, GetAccess
        l32r     a8, .Lsec_171_4
        callx8   a8
        l32i.n   a9, a4, 0
        add.n    a8, a2, a3
        slli     a8, a8, 4
        add.n    a8, a9, a8
        movi.n   a2, 0
        l8ui     a4, a8, 41
        bne      a4, a2, .Lsec_171_94
        addi.n   a2, a3, 1
        addx2    a2, a2, a2
        slli     a2, a2, 4
        add.n    a2, a9, a2
        l32i.n   a2, a2, 4
        bnez.n   a2, .Lsec_171_94
        j        .Lsec_171_8f
        .byte    0x00
.Lsec_171_69:
        l32i.n   a10, a2, 40
        s32i.n   a10, a8, 32
        bnez.n   a10, .Lsec_171_7d
        addx2    a3, a3, a3
        slli     a3, a3, 4
        addi     a3, a3, 32
        add.n    a3, a9, a3
        s32i     a3, a8, 36
.Lsec_171_7d:
        movi.n   a3, 0
        mov.n    a10, a2
        s32i.n   a3, a2, 40
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacAdjustTimestamp
        l32r     a8, .Lsec_171_8
        callx8   a8
        j        .Lsec_171_94
.Lsec_171_8f:
        l32i.n   a2, a8, 32
        bnez     a2, .Lsec_171_69
.Lsec_171_94:
        retw.n
        .size   ppGetTxframe, .-ppGetTxframe
.Lsec_171_96:

        .section .text.ppInitTxq,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_172_0, pTxRx
        .literal .Lsec_172_4, 0x0d0a0704
.Lsec_172_8:
        .global ppInitTxq
        .type   ppInitTxq, @function
ppInitTxq:
        entry    a1, 32
        l32r     a8, .Lsec_172_0
        movi.n   a9, 0
        movi.n   a11, 16
        l32i.n   a8, a8, 0
        addi     a10, a8, 32
        loop     a11, .Lsec_172_27
        s32i.n   a9, a10, 0
        s32i.n   a10, a10, 4
        s8i      a9, a10, 16
        memw
        addi     a10, a10, 48
.Lsec_172_27:
        movi.n   a10, 1
        s32i.n   a9, a8, 0
        movi.n   a11, 7
        s32i.n   a9, a8, 4
        s32i.n   a9, a8, 8
        s32i.n   a9, a8, 12
        s32i.n   a9, a8, 16
        s32i.n   a9, a8, 20
        l32r     a9, .Lsec_172_4
        s8i      a10, a8, 48
        s8i      a10, a8, 96
        s8i      a10, a8, 144
        s8i      a10, a8, 192
        movi.n   a10, 8
        s8i      a11, a8, 44
        s8i      a11, a8, 92
        s8i      a11, a8, 140
        s8i      a10, a8, 188
        memw
        s32i.n   a9, a8, 24
        memw
        retw.n
        .size   ppInitTxq, .-ppInitTxq
.Lsec_172_5e:

        .section .text.ppEnableQueue,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_173_0, pTxRx
.Lsec_173_4:
        .global ppEnableQueue
        .type   ppEnableQueue, @function
ppEnableQueue:
        entry    a1, 32
        l32r     a8, .Lsec_173_0
        extui    a2, a2, 0, 8
        movi.n   a10, 0
        extui    a6, a6, 0, 8
        l32i.n   a9, a8, 0
        addx2    a8, a2, a2
        slli     a8, a8, 4
        add.n    a8, a9, a8
        addx4    a6, a6, a9
        s8i      a5, a8, 44
        s8i      a4, a8, 45
        s8i      a3, a8, 46
        addi.n   a8, a2, 1
        addx2    a8, a8, a8
        slli     a8, a8, 4
        add.n    a8, a9, a8
        s8i      a10, a8, 1
        memw
        movi.n   a8, 1
        ssl      a2
        sll      a8, a8
        l32i.n   a2, a6, 4
        or       a2, a2, a8
        s32i.n   a2, a6, 4
        l32i.n   a2, a9, 0
        or       a2, a2, a8
        s32i.n   a2, a9, 0
        memw
        retw.n
        .size   ppEnableQueue, .-ppEnableQueue
.Lsec_173_53:

        .section .wifi0iram.62,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_174_0, pTxRx
        .literal .Lsec_174_4, 0x0000ffff
        .literal .Lsec_174_8, ppEnableQueue
.Lsec_174_c:
        .global ppSearchTxQueue
        .type   ppSearchTxQueue, @function
ppSearchTxQueue:
        entry    a1, 32
        extui    a11, a2, 0, 8
        l32r     a2, .Lsec_174_0
        extui    a14, a5, 0, 8
        extui    a13, a4, 0, 8
        extui    a12, a3, 0, 8
        movi.n   a15, 1
        l32i.n   a8, a2, 0
        movi.n   a10, -1
        addx4    a2, a14, a8
        l32i.n   a4, a2, 4
        l32i.n   a9, a8, 0
        bnez.n   a4, .Lsec_174_38
        add.n    a2, a8, a14
        l8ui     a2, a2, 24
        bbs      a9, a2, .Lsec_174_78
        j        .Lsec_174_f8
.Lsec_174_38:
        neg      a2, a4
        and      a2, a2, a4
        nsau     a2, a2
        neg      a2, a2
        addi     a2, a2, 31
        addx2    a3, a2, a2
        slli     a3, a3, 4
        add.n    a3, a8, a3
        l8ui     a5, a3, 44
        bne      a5, a13, .Lsec_174_69
        l8ui     a5, a3, 45
        bne      a5, a12, .Lsec_174_69
        l8ui     a3, a3, 46
        bne      a3, a11, .Lsec_174_69
        extui    a2, a2, 0, 8
        j        .Lsec_174_100
        .byte    0x00
        .byte    0x00
.Lsec_174_69:
        ssl      a2
        sll      a2, a15
        xor      a2, a10, a2
        and      a4, a4, a2
        bnez     a4, .Lsec_174_38
.Lsec_174_78:
        add.n    a8, a8, a14
        l8ui     a15, a8, 24
        movi.n   a10, 15
        movi.n   a8, -1
        or       a10, a9, a10
        ssl      a15
        sll      a15, a8
        l32r     a4, .Lsec_174_4
        xor      a8, a8, a15
        xor      a3, a10, a4
        and      a8, a8, a3
        movi.n   a2, 16
        beq      a10, a4, .Lsec_174_100
        movi.n   a10, 31
        beqz.n   a8, .Lsec_174_a8
        nsau     a8, a8
        sub      a10, a10, a8
        j        .Lsec_174_b1
.Lsec_174_a8:
        and      a15, a15, a3
        nsau     a15, a15
        sub      a10, a10, a15
.Lsec_174_b1:
        movi.n   a2, -1
        ssl      a10
        sll      a4, a2
        xor      a2, a2, a4
        and      a2, a2, a9
        movi.n   a3, 31
        beqz.n   a2, .Lsec_174_d8
        nsau     a2, a2
        sub      a3, a3, a2
        addi.n   a10, a10, 1
        add.n    a10, a10, a3
        extui    a2, a10, 1, 8
        srai     a10, a10, 1
        j        .Lsec_174_fa
        .byte    0x00
        .byte    0x00
.Lsec_174_d8:
        and      a4, a4, a9
        nsau     a4, a4
        addi.n   a10, a10, 12
        sub      a3, a3, a4
        add      a8, a10, a3
        addi     a10, a8, 1
        movi     a2, 30
        srai     a10, a10, 1
        bge      a2, a8, .Lsec_174_f5
        addi     a10, a10, -12
.Lsec_174_f5:
        extui    a2, a10, 0, 8
.Lsec_174_f8:
        mov.n    a10, a2
.Lsec_174_fa:
        .reloc  ., R_XTENSA_ASM_EXPAND, ppEnableQueue
        l32r     a8, .Lsec_174_8
        callx8   a8
.Lsec_174_100:
        retw.n
        .size   ppSearchTxQueue, .-ppSearchTxQueue
.Lsec_174_102:

        .section .text.ppTxqEmpty,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_175_0, pTxRx
        .literal .Lsec_175_4, is_lmac_idle
.Lsec_175_8:
        .global ppTxqEmpty
        .type   ppTxqEmpty, @function
ppTxqEmpty:
        entry    a1, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, is_lmac_idle
        l32r     a8, .Lsec_175_4
        callx8   a8
        beqz.n   a10, .Lsec_175_50
        l32r     a8, .Lsec_175_0
        l32i.n   a9, a8, 0
        l32i     a11, a9, 0x334
        bnez.n   a11, .Lsec_175_48
        l32i     a8, a9, 0x33c
        bnez.n   a8, .Lsec_175_48
        l32i     a11, a9, 0x344
        bnez.n   a11, .Lsec_175_48
        addi     a8, a9, 32
        movi     a11, 16
        loop     a11, .Lsec_175_45
        l8ui     a12, a8, 9
        bnez.n   a12, .Lsec_175_39
        l32i.n   a13, a8, 20
        bnez.n   a13, .Lsec_175_4e
.Lsec_175_39:
        l32i.n   a12, a8, 0
        bnez.n   a12, .Lsec_175_48
        l32i     a13, a9, 0x34c
        bnez.n   a13, .Lsec_175_4e
        addi     a8, a8, 48
.Lsec_175_45:
        j        .Lsec_175_50
.Lsec_175_48:
        movi.n   a10, 0
        j        .Lsec_175_50
        .byte    0x00
.Lsec_175_4e:
        mov.n    a10, a12
.Lsec_175_50:
        mov.n    a2, a10
        retw.n
        .size   ppTxqEmpty, .-ppTxqEmpty
.Lsec_175_54:

        .section .iram1.66,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_176_0, g_osi_funcs_p
        .literal .Lsec_176_4, g_intr_lock_mux
        .literal .Lsec_176_8, pp_sig_cnt
        .literal .Lsec_176_c, xphyQueue
        .literal .Lsec_176_10, pTxRx
        .literal .Lsec_176_14, .Lsec_211_0
        .literal .Lsec_176_18, wifi_log
.Lsec_176_1c:
        .global pp_post
        .type   pp_post, @function
pp_post:
        entry    a1, 48
        l32r     a5, .Lsec_176_0
        movi.n   a9, 17
        l32i.n   a4, a5, 0
        l32r     a6, .Lsec_176_4
        s32i.n   a2, a1, 0
        s32i.n   a3, a1, 4
        l32i.n   a8, a4, 40
        l32i.n   a10, a6, 0
        l32r     a4, .Lsec_176_8
        bgeu     a9, a2, .Lsec_176_9d
        add      a4, a4, a2
        memw
        callx8   a8
        mov.n    a11, a10
        memw
        l8ui     a2, a4, 0
        extui    a2, a2, 0, 8
        l32i.n   a3, a5, 0
        beqz.n   a2, .Lsec_176_55
        l32i.n   a2, a3, 44
        j        .Lsec_176_ba
        .byte    0x00
.Lsec_176_55:
        memw
        l8ui     a2, a4, 0
        addi.n   a2, a2, 1
        extui    a2, a2, 0, 8
        l32i.n   a10, a6, 0
        memw
        s8i      a2, a4, 0
        l32i.n   a2, a3, 44
        memw
        callx8   a2
        addi.n   a12, a1, 8
        mov.n    a11, a1
        l32r     a3, .Lsec_176_c
        l32i.n   a2, a5, 0
        l32i.n   a10, a3, 0
        l32i     a2, a2, 104
        callx8   a2
        mov.n    a2, a10
        l32i.n   a3, a1, 8
        beqz.n   a3, .Lsec_176_8e
        l32i.n   a3, a5, 0
        l32i.n   a3, a3, 48
        callx8   a3
.Lsec_176_8e:
        addi.n   a10, a2, -1
        movi.n   a8, 1
        movi.n   a2, 0
        moveqz   a8, a2, a10
        j        .Lsec_176_150
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_176_9d:
        callx8   a8
        add.n    a8, a4, a2
        memw
        l8ui     a8, a8, 0
        mov.n    a11, a10
        extui    a8, a8, 0, 8
        beqz.n   a8, .Lsec_176_c6
        addi     a8, a2, -6
        bltui    a8, 3, .Lsec_176_c6
        l32i.n   a2, a5, 0
        l32i     a2, a2, 44
.Lsec_176_ba:
        l32i     a10, a6, 0
        callx8   a2
        j        .Lsec_176_10f
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_176_c6:
        movi.n   a8, 13
        beq      a2, a8, .Lsec_176_de
        add.n    a4, a4, a2
        memw
        l8ui     a8, a4, 0
        addi.n   a8, a8, 1
        extui    a8, a8, 0, 8
        memw
        s8i      a8, a4, 0
.Lsec_176_de:
        l32i.n   a4, a5, 0
        l32i.n   a10, a6, 0
        movi.n   a6, 13
        l32i.n   a4, a4, 44
        memw
        callx8   a4
        l32r     a4, .Lsec_176_c
        beq      a2, a6, .Lsec_176_116
.Lsec_176_f2:
        movi.n   a10, 10
        l32i.n   a5, a5, 0
        l32i     a6, a5, 100
        l32i     a5, a5, 160
        l32i.n   a4, a4, 0
        callx8   a5
        mov.n    a12, a10
        or       a11, a1, a1
        or       a10, a4, a4
        callx8   a6
        bnei     a10, 1, .Lsec_176_138
.Lsec_176_10f:
        movi.n   a8, 0
        j        .Lsec_176_150
        .byte    0x00
        .byte    0x00
.Lsec_176_116:
        l32i.n   a6, a5, 0
        l32i.n   a10, a4, 0
        l32i     a6, a6, 120
        callx8   a6
        movi     a6, 149
        movi.n   a8, 1
        bltu     a6, a10, .Lsec_176_150
        l32r     a6, .Lsec_176_10
        movi.n   a9, 48
        l32i.n   a6, a6, 0
        l32i     a6, a6, 0x3c0
        bgeu     a9, a6, .Lsec_176_f2
        j        .Lsec_176_150
.Lsec_176_138:
        l32r     a13, .Lsec_176_14
        movi.n   a11, 1
        mov.n    a15, a3
        mov.n    a14, a2
        movi.n   a12, 2
        slli     a11, a11, 11
        movi.n   a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_176_18
        callx8   a8
        movi.n   a8, 1
.Lsec_176_150:
        mov.n    a2, a8
        retw.n
        .size   pp_post, .-pp_post
.Lsec_176_154:

        .section .text.pp_delete_task,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_177_0, .Lsec_254_0
        .literal .Lsec_177_4, s_wifi_queue
        .literal .Lsec_177_8, g_osi_funcs_p
        .literal .Lsec_177_c, .Lsec_253_0
        .literal .Lsec_177_10, pp_post
        .literal .Lsec_177_14, .Lsec_95_c
        .literal .Lsec_177_18, .Lsec_95_c
.Lsec_177_1c:
        .global pp_delete_task
        .type   pp_delete_task, @function
pp_delete_task:
        entry    a1, 32
        l32r     a2, .Lsec_177_0
        l32i.n   a2, a2, 0
        beqz     a2, .Lsec_177_8c
        l32r     a2, .Lsec_177_4
        l32i.n   a2, a2, 0
        beqz     a2, .Lsec_177_8c
        movi.n   a11, 0
        movi.n   a10, 1
        l32r     a3, .Lsec_177_8
        l32i.n   a2, a3, 0
        l32i.n   a2, a2, 52
        callx8   a2
        l32r     a2, .Lsec_177_c
        s32i.n   a10, a2, 0
        bnez.n   a10, .Lsec_177_4a
        memw
        j        .Lsec_177_8c
.Lsec_177_4a:
        movi.n   a11, 0
        movi.n   a10, 15
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_post
        l32r     a8, .Lsec_177_10
        callx8   a8
        beqz.n   a10, .Lsec_177_61
        .reloc  ., R_XTENSA_ASM_EXPAND, .Lsec_95_c
        l32r     a8, .Lsec_177_14
        callx8   a8
        j        .Lsec_177_6c
        .byte    0x00
        .byte    0x00
.Lsec_177_61:
        l32i.n   a8, a3, 0
        movi.n   a11, -1
        l32i.n   a8, a8, 60
        l32i.n   a10, a2, 0
        callx8   a8
.Lsec_177_6c:
        movi.n   a10, 1
        l32i.n   a8, a3, 0
        l32i     a8, a8, 156
        callx8   a8
        l32i.n   a3, a3, 0
        l32i.n   a10, a2, 0
        l32i     a3, a3, 56
        callx8   a3
        movi.n   a10, 0
        s32i.n   a10, a2, 0
        memw
        j        .Lsec_177_92
        .byte    0x00
        .byte    0x00
.Lsec_177_8c:
        .reloc  ., R_XTENSA_ASM_EXPAND, .Lsec_95_c
        l32r     a8, .Lsec_177_18
        callx8   a8
.Lsec_177_92:
        mov.n    a2, a10
        retw.n
        .size   pp_delete_task, .-pp_delete_task
.Lsec_177_96:

        .section .wifiextrairam.63,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_178_0, pTxRx
        .literal .Lsec_178_4, 0x0000ffff
        .literal .Lsec_178_8, 0xff0fffff
        .literal .Lsec_178_c, ppSearchTxQueue
        .literal .Lsec_178_10, pp_post
.Lsec_178_14:
        .global ppMapWaitTxq
        .type   ppMapWaitTxq, @function
ppMapWaitTxq:
        entry    a1, 48
        extui    a5, a2, 0, 8
        l32r     a2, .Lsec_178_0
        movi.n   a4, 15
        l32i.n   a7, a2, 0
        l32i.n   a2, a7, 0
        or       a4, a4, a2
        movi.n   a2, 0
        l32r     a6, .Lsec_178_4
        bne      a4, a6, .Lsec_178_31
        j        .Lsec_178_f5
.Lsec_178_31:
        addx8    a6, a5, a7
        slli     a4, a5, 3
        l32i     a3, a6, 0x334
        bne      a3, a2, .Lsec_178_40
        j        .Lsec_178_f5
.Lsec_178_40:
        addi     a5, a5, 102
        addx8    a5, a5, a7
        addi.n   a5, a5, 4
        s32i     a5, a6, 0x338
        movi     a5, 12
        s32i     a2, a6, 0x334
.Lsec_178_51:
        mov.n    a6, a3
        l32i.n   a2, a6, 4
        l32i.n   a7, a6, 44
        l32i.n   a10, a2, 4
        l32i.n   a2, a7, 0
        l32i.n   a3, a3, 40
        l32i.n   a11, a6, 36
        bbci     a2, 18, .Lsec_178_64
        addi.n   a10, a10, 4
.Lsec_178_64:
        addi     a9, a6, 40
        l32r     a8, .Lsec_178_0
        l8ui     a12, a10, 0
        l32i.n   a2, a8, 0
        l32i.n   a10, a7, 16
        bany     a12, a5, .Lsec_178_7d
        extui    a10, a10, 20, 4
        memw
        j        .Lsec_178_b0
.Lsec_178_7d:
        l32i.n   a12, a7, 4
        extui    a10, a10, 18, 2
        extui    a13, a12, 4, 4
        extui    a12, a12, 0, 4
        l8ui     a11, a11, 128
        s32i.n   a9, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, ppSearchTxQueue
        l32r     a8, .Lsec_178_c
        callx8   a8
        l32i.n   a9, a1, 0
        bnei     a10, 16, .Lsec_178_b0
        movi.n   a7, 0
        add.n    a2, a2, a4
        s32i.n   a7, a6, 40
        l32i     a7, a2, 0x338
        s32i.n   a6, a7, 0
        s32i     a9, a2, 0x338
        memw
        j        .Lsec_178_f0
        .byte    0x00
.Lsec_178_b0:
        l32i.n   a11, a7, 16
        extui    a12, a10, 0, 4
        slli     a10, a12, 20
        addx2    a12, a12, a12
        slli     a12, a12, 4
        l32r     a13, .Lsec_178_8
        add.n    a12, a2, a12
        and      a11, a11, a13
        or       a10, a11, a10
        movi.n   a11, 0
        s32i.n   a10, a7, 16
        l32i.n   a10, a12, 36
        s32i.n   a11, a6, 40
        s32i.n   a6, a10, 0
        l32i.n   a6, a7, 16
        extui    a6, a6, 20, 4
        addx2    a6, a6, a6
        slli     a6, a6, 4
        l32i.n   a10, a7, 4
        add.n    a2, a2, a6
        extui    a10, a10, 4, 4
        s32i.n   a9, a2, 36
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_post
        l32r     a8, .Lsec_178_10
        callx8   a8
.Lsec_178_f0:
        bnez     a3, .Lsec_178_51
        movi.n   a2, 1
.Lsec_178_f5:
        retw.n
        .size   ppMapWaitTxq, .-ppMapWaitTxq
.Lsec_178_f7:

        .section .wifiextrairam.64,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_179_0, .Lsec_208_0
        .literal .Lsec_179_4, pm_allow_tx
        .literal .Lsec_179_8, ppMapWaitTxq
.Lsec_179_c:
        .global ppProcessWaitingQueue
        .type   ppProcessWaitingQueue, @function
ppProcessWaitingQueue:
        entry    a1, 32
        l32r     a4, .Lsec_179_0
        movi.n   a3, -1
        movi     a5, 0
        l8ui     a2, a4, 0
.Lsec_179_1a:
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_allow_tx
        l32r     a8, .Lsec_179_4
        callx8   a8
        beqz.n   a10, .Lsec_179_30
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, ppMapWaitTxq
        l32r     a8, .Lsec_179_8
        callx8   a8
        movnez   a3, a2, a10
.Lsec_179_30:
        addi.n   a2, a2, 1
        extui    a2, a2, 0, 8
        bnei     a2, 3, .Lsec_179_3a
        mov.n    a2, a5
.Lsec_179_3a:
        l8ui     a8, a4, 0
        bne      a8, a2, .Lsec_179_1a
        beqi     a3, -1, .Lsec_179_60
        addi.n   a3, a3, 1
        extui    a3, a3, 0, 8
        beqi     a3, 3, .Lsec_179_58
        s8i      a3, a4, 0
        memw
        memw
        j        .Lsec_179_60
        .byte    0x00
.Lsec_179_58:
        movi.n   a2, 0
        s8i      a2, a4, 0
        memw
.Lsec_179_60:
        memw
        retw.n
        .size   ppProcessWaitingQueue, .-ppProcessWaitingQueue
.Lsec_179_65:

        .section .text.ppProcessWaitQ,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_180_0, ppProcessWaitingQueue
.Lsec_180_4:
        .global ppProcessWaitQ
        .type   ppProcessWaitQ, @function
ppProcessWaitQ:
        entry    a1, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, ppProcessWaitingQueue
        l32r     a8, .Lsec_180_0
        callx8   a8
        retw.n
        .size   ppProcessWaitQ, .-ppProcessWaitQ
.Lsec_180_f:

        .section .text.ppDisableQueue,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_181_0, pTxRx
        .literal .Lsec_181_4, ppProcessWaitingQueue
.Lsec_181_8:
        .global ppDisableQueue
        .type   ppDisableQueue, @function
ppDisableQueue:
        entry    a1, 32
        l32r     a8, .Lsec_181_0
        extui    a3, a3, 0, 8
        l32i.n   a9, a8, 0
        movi.n   a8, -2
        addx4    a3, a3, a9
        ssl      a2
        src      a2, a8, a8
        l32i.n   a8, a3, 4
        and      a8, a8, a2
        s32i.n   a8, a3, 4
        l32i.n   a3, a9, 0
        and      a3, a3, a2
        s32i.n   a3, a9, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, ppProcessWaitingQueue
        l32r     a8, .Lsec_181_4
        callx8   a8
        retw.n
        .size   ppDisableQueue, .-ppDisableQueue
.Lsec_181_37:

        .section .text.ppCheckTxIdle,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_182_0, pTxRx
        .literal .Lsec_182_4, lmacIsIdle
        .literal .Lsec_182_8, ppDisableQueue
.Lsec_182_c:
        .global ppCheckTxIdle
        .type   ppCheckTxIdle, @function
ppCheckTxIdle:
        entry    a1, 32
        movi     a2, 0
.Lsec_182_12:
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacIsIdle
        l32r     a8, .Lsec_182_4
        callx8   a8
        bnez.n   a10, .Lsec_182_21
.Lsec_182_1c:
        movi.n   a2, 1
        j        .Lsec_182_bc
.Lsec_182_21:
        addi.n   a2, a2, 1
        bnei     a2, 5, .Lsec_182_12
        j        .Lsec_182_8e
.Lsec_182_29:
        neg      a8, a2
        and      a8, a8, a2
        nsau     a8, a8
        neg      a8, a8
        addi     a10, a8, 31
        addx2    a11, a10, a10
        slli     a11, a11, 4
        add.n    a11, a9, a11
        l32i.n   a2, a11, 32
        bnez     a2, .Lsec_182_1c
        addi     a8, a8, 32
        addx2    a8, a8, a8
        slli     a8, a8, 4
        add.n    a8, a9, a8
        l32i.n   a2, a8, 4
        bnez     a2, .Lsec_182_1c
        l8ui     a8, a11, 44
        movi.n   a11, 4
        bgeui    a8, 8, .Lsec_182_88
        movi.n   a9, 1
        mov.n    a11, a2
        movnez   a11, a9, a8
        extui    a11, a11, 0, 8
        beqz.n   a11, .Lsec_182_86
        addi     a12, a8, -3
        mov.n    a11, a2
        movnez   a11, a9, a12
        extui    a11, a11, 0, 8
        beqz.n   a11, .Lsec_182_86
        movi.n   a11, 3
        bltu     a8, a11, .Lsec_182_88
        mov.n    a11, a9
        bltui    a8, 6, .Lsec_182_88
        mov.n    a11, a2
        j        .Lsec_182_88
        .byte    0x00
.Lsec_182_86:
        movi.n   a11, 2
.Lsec_182_88:
        .reloc  ., R_XTENSA_ASM_EXPAND, ppDisableQueue
        l32r     a8, .Lsec_182_8
        callx8   a8
.Lsec_182_8e:
        l32r     a2, .Lsec_182_0
        l32i.n   a9, a2, 0
        l32i.n   a2, a9, 0
        bnez     a2, .Lsec_182_29
        addi     a8, a9, 32
        movi.n   a2, 4
        loop     a2, .Lsec_182_a8
        l32i.n   a10, a8, 0
        bnez     a10, .Lsec_182_1c
        addi     a8, a8, 48
.Lsec_182_a8:
        l32i     a2, a9, 0x334
        bnez     a2, .Lsec_182_1c
        l32i     a8, a9, 0x33c
        bnez     a8, .Lsec_182_1c
        movi.n   a2, 1
        l32i     a9, a9, 0x344
        moveqz   a2, a8, a9
.Lsec_182_bc:
        retw.n
        .size   ppCheckTxIdle, .-ppCheckTxIdle
.Lsec_182_be:

        .section .wifiextrairam.61,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_183_0, pTxRx
        .literal .Lsec_183_4, ppDisableQueue
.Lsec_183_8:
        .global ppSelectNextQueue
        .type   ppSelectNextQueue, @function
ppSelectNextQueue:
        entry    a1, 32
        l32r     a4, .Lsec_183_0
        extui    a2, a2, 0, 8
        slli     a5, a2, 2
        l32i.n   a9, a4, 0
        add.n    a3, a9, a2
        addx4    a8, a2, a9
        l8ui     a3, a3, 24
        l32i.n   a8, a8, 4
        bbc      a8, a3, .Lsec_183_9c
        addx2    a8, a3, a3
        slli     a8, a8, 4
        add.n    a8, a9, a8
        l8ui     a10, a8, 49
        beqz     a10, .Lsec_183_9c
        movi.n   a10, 0
        l32i.n   a9, a8, 32
        s8i      a10, a8, 49
        memw
        bnez.n   a9, .Lsec_183_4e
        l32i.n   a8, a8, 52
        bnez.n   a8, .Lsec_183_4e
        mov.n    a11, a2
        mov.n    a10, a3
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, ppDisableQueue
        l32r     a8, .Lsec_183_4
        callx8   a8
.Lsec_183_4e:
        l32i.n   a11, a4, 0
        add.n    a5, a11, a5
        l32i.n   a4, a5, 4
        beqz     a4, .Lsec_183_9c
        movi.n   a9, -1
        movi.n   a10, 1
        ssl      a3
        sll      a5, a9
        ssl      a3
        sll      a10, a10
        and      a8, a4, a5
        xor      a10, a9, a10
        and      a10, a10, a8
        beqz.n   a10, .Lsec_183_7d
        neg      a8, a10
        and      a8, a8, a10
        j        .Lsec_183_8b
        .byte    0x00
        .byte    0x00
.Lsec_183_7d:
        xor      a9, a9, a5
        and      a9, a9, a4
        beqz.n   a9, .Lsec_183_9c
        neg      a8, a9
        and      a8, a8, a9
.Lsec_183_8b:
        nsau     a8, a8
        neg      a8, a8
        addi     a8, a8, 31
        add.n    a11, a11, a2
        s8i      a8, a11, 24
        memw
.Lsec_183_9c:
        memw
        retw.n
        .size   ppSelectNextQueue, .-ppSelectNextQueue
.Lsec_183_a1:

        .section .wifi0iram.60,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_184_0, pTxRx
        .literal .Lsec_184_4, ppGetTxframe
        .literal .Lsec_184_8, ppSelectNextQueue
        .literal .Lsec_184_c, ppGetTxframe
        .literal .Lsec_184_10, ppGetTxframe
        .literal .Lsec_184_14, ppGetTxframe
.Lsec_184_18:
        .global ppSearchTxframe
        .type   ppSearchTxframe, @function
ppSearchTxframe:
        entry    a1, 48
        extui    a4, a2, 0, 8
        bnei     a4, 4, .Lsec_184_2c
        movi.n   a10, 2
        .reloc  ., R_XTENSA_ASM_EXPAND, ppGetTxframe
        l32r     a8, .Lsec_184_4
        callx8   a8
        j        .Lsec_184_131
.Lsec_184_2c:
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, ppSelectNextQueue
        l32r     a8, .Lsec_184_8
        callx8   a8
        l32r     a6, .Lsec_184_0
        l32i.n   a2, a6, 0
        add.n    a3, a2, a4
        l8ui     a5, a3, 24
        bnez.n   a4, .Lsec_184_66
        l32i.n   a3, a2, 4
        bbc      a3, a5, .Lsec_184_54
        addi.n   a3, a5, 1
        addx2    a3, a3, a3
        slli     a3, a3, 4
        add.n    a2, a2, a3
        l8ui     a2, a2, 1
        bnez.n   a2, .Lsec_184_66
.Lsec_184_54:
        mov.n    a2, a4
.Lsec_184_56:
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, ppGetTxframe
        l32r     a8, .Lsec_184_c
        callx8   a8
        bnez     a10, .Lsec_184_131
        addi.n   a2, a2, 1
        bnei     a2, 3, .Lsec_184_56
.Lsec_184_66:
        l32i.n   a2, a6, 0
        movi.n   a7, -1
        addx4    a2, a4, a2
        ssl      a5
        sll      a7, a7
        movi.n   a9, 1
        l32i.n   a8, a2, 4
        and      a2, a8, a7
        j        .Lsec_184_d2
.Lsec_184_7d:
        neg      a3, a2
        and      a3, a3, a2
        nsau     a3, a3
        neg      a3, a3
        addi     a5, a3, 31
        mov.n    a10, a5
        s32i.n   a8, a1, 0
        s32i     a9, a1, 4
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, ppGetTxframe
        l32r     a8, .Lsec_184_10
        callx8   a8
        l32i.n   a8, a1, 0
        l32i.n   a9, a1, 4
        beqz.n   a10, .Lsec_184_c4
        l32i.n   a2, a6, 0
        addi     a3, a3, 32
        addx2    a3, a3, a3
        add.n    a4, a2, a4
        slli     a3, a3, 4
        add.n    a2, a2, a3
        s8i      a5, a4, 24
        movi.n   a3, 1
        s8i      a3, a2, 1
        memw
        memw
        j        .Lsec_184_131
        .byte    0x00
        .byte    0x00
.Lsec_184_c4:
        ssl      a5
        sll      a5, a9
        movi.n   a3, -1
        xor      a3, a3, a5
        and      a2, a2, a3
.Lsec_184_d2:
        bnez     a2, .Lsec_184_7d
        movi.n   a5, -1
        xor      a5, a5, a7
        and      a5, a5, a8
        movi.n   a7, 1
        j        .Lsec_184_12c
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_184_e5:
        neg      a3, a5
        and      a3, a3, a5
        nsau     a3, a3
        neg      a3, a3
        addi     a2, a3, 31
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, ppGetTxframe
        l32r     a8, .Lsec_184_14
        callx8   a8
        beqz.n   a10, .Lsec_184_11e
        l32i.n   a5, a6, 0
        addi     a3, a3, 32
        addx2    a3, a3, a3
        add.n    a4, a5, a4
        slli     a3, a3, 4
        s8i      a2, a4, 24
        add.n    a5, a5, a3
        movi.n   a2, 1
        s8i      a2, a5, 1
        memw
        memw
        j        .Lsec_184_131
.Lsec_184_11e:
        ssl      a2
        sll      a2, a7
        movi.n   a3, -1
        xor      a2, a3, a2
        and      a5, a5, a2
.Lsec_184_12c:
        bnez     a5, .Lsec_184_e5
        mov.n    a10, a5
.Lsec_184_131:
        mov.n    a2, a10
        retw.n
        .size   ppSearchTxframe, .-ppSearchTxframe
.Lsec_184_135:

        .section .wifi0iram.65,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_185_0, 0xff0fffff
        .literal .Lsec_185_4, g_mesh_is_started
        .literal .Lsec_185_8, ppProcessWaitingQueue
        .literal .Lsec_185_c, pm_on_data_tx
        .literal .Lsec_185_10, pm_on_data_tx
        .literal .Lsec_185_14, ppSearchTxQueue
        .literal .Lsec_185_18, pm_on_data_tx
        .literal .Lsec_185_1c, ppSearchTxQueue
.Lsec_185_20:
        .global ppMapTxQueue
        .type   ppMapTxQueue, @function
ppMapTxQueue:
        entry    a1, 48
        mov.n    a4, a2
        l32i.n   a7, a2, 36
        l32i.n   a2, a2, 4
        l32i.n   a9, a2, 4
        l32i.n   a2, a4, 44
        l32i.n   a2, a2, 0
        bbci     a2, 18, .Lsec_185_34
        addi.n   a9, a9, 4
.Lsec_185_34:
        movi     a2, 240
        s32i.n   a9, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, ppProcessWaitingQueue
        l32r     a8, .Lsec_185_8
        callx8   a8
        l32i.n   a3, a4, 44
        l32i.n   a12, a3, 4
        and      a2, a2, a12
        l32i.n   a9, a1, 0
        bnei     a2, 64, .Lsec_185_66
        l32i.n   a2, a3, 16
        l32r     a4, .Lsec_185_0
        and      a2, a2, a4
        movi.n   a4, 3
        slli     a4, a4, 20
        or       a2, a2, a4
        s32i.n   a2, a3, 16
        memw
        j        .Lsec_185_a0
.Lsec_185_66:
        l32i.n   a10, a3, 16
        extui    a8, a10, 18, 2
        mov.n    a5, a8
        l8ui     a6, a9, 0
        beqz.n   a7, .Lsec_185_78
        l8ui     a2, a7, 11
        bbci     a2, 7, .Lsec_185_a6
.Lsec_185_78:
        movi.n   a2, 7
        extui    a8, a8, 0, 2
        l32r     a4, .Lsec_185_0
        slli     a8, a8, 20
        movi.n   a11, 0
        mov.n    a10, a5
        s8i      a2, a3, 4
        memw
        l32i.n   a2, a3, 16
        and      a2, a2, a4
        or       a2, a2, a8
        s32i.n   a2, a3, 16
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_on_data_tx
        l32r     a8, .Lsec_185_c
        callx8   a8
.Lsec_185_a0:
        movi.n   a2, 0
        j        .Lsec_185_1b1
        .byte    0x00
.Lsec_185_a6:
        movi.n   a7, 12
        and      a7, a6, a7
        bnez     a7, .Lsec_185_112
        movi.n   a2, 7
        s8i      a2, a3, 4
        memw
        extui    a8, a8, 0, 2
        slli     a8, a8, 20
        movi.n   a11, 1
        l32r     a2, .Lsec_185_0
        and      a2, a10, a2
        or       a2, a2, a8
        mov.n    a10, a5
        s32i.n   a2, a3, 16
        mov.n    a2, a7
        s32i.n   a9, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_on_data_tx
        l32r     a8, .Lsec_185_10
        callx8   a8
        l32i.n   a9, a1, 0
        beqz     a10, .Lsec_185_1b1
        l32r     a3, .Lsec_185_4
        l8ui     a3, a3, 0
        bnez.n   a3, .Lsec_185_10c
        movi.n   a3, -16
        and      a3, a6, a3
        bnei     a3, 64, .Lsec_185_101
        l8ui     a3, a9, 1
        movi.n   a4, 16
        or       a3, a3, a4
        s8i      a3, a9, 1
        memw
        memw
        j        .Lsec_185_1b1
.Lsec_185_101:
        l32i.n   a3, a4, 44
        l16ui    a3, a3, 2
        bbci     a3, 0, .Lsec_185_10c
        j        .Lsec_185_1b1
.Lsec_185_10c:
        movi.n   a2, 1
        j        .Lsec_185_1b1
        .byte    0x00
.Lsec_185_112:
        l32i.n   a2, a3, 0
        movi.n   a9, 1
        slli     a9, a9, 13
        and      a2, a2, a9
        beqz.n   a2, .Lsec_185_159
        l32i.n   a2, a4, 36
        extui    a13, a12, 4, 4
        mov.n    a10, a8
        extui    a12, a12, 0, 4
        l8ui     a11, a2, 128
        .reloc  ., R_XTENSA_ASM_EXPAND, ppSearchTxQueue
        l32r     a8, .Lsec_185_14
        callx8   a8
        beqi     a10, 16, .Lsec_185_10c
        extui    a10, a10, 0, 4
        l32i.n   a2, a3, 16
        slli     a10, a10, 20
        movi.n   a11, 1
        l32r     a4, .Lsec_185_0
        and      a2, a2, a4
        or       a2, a2, a10
        mov.n    a10, a5
        s32i.n   a2, a3, 16
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_on_data_tx
        l32r     a8, .Lsec_185_18
        callx8   a8
        mov.n    a2, a10
        j        .Lsec_185_1b1
.Lsec_185_159:
        bnei     a7, 8, .Lsec_185_185
        movi     a7, 112
        and      a6, a6, a7
        bnei     a6, 64, .Lsec_185_185
        movi.n   a4, 7
        s8i      a4, a3, 4
        memw
        extui    a9, a8, 0, 2
        slli     a9, a9, 20
        l32r     a4, .Lsec_185_0
        and      a8, a10, a4
        or       a8, a8, a9
        s32i.n   a8, a3, 16
        memw
        j        .Lsec_185_1b1
        .byte    0x00
.Lsec_185_185:
        l32i.n   a4, a4, 36
        extui    a13, a12, 4, 4
        mov.n    a10, a5
        extui    a12, a12, 0, 4
        l8ui     a11, a4, 128
        .reloc  ., R_XTENSA_ASM_EXPAND, ppSearchTxQueue
        l32r     a8, .Lsec_185_1c
        callx8   a8
        bnei     a10, 16, .Lsec_185_19e
        j        .Lsec_185_10c
.Lsec_185_19e:
        extui    a10, a10, 0, 4
        l32i.n   a4, a3, 16
        slli     a10, a10, 20
        l32r     a5, .Lsec_185_0
        and      a4, a4, a5
        or       a4, a4, a10
        s32i.n   a4, a3, 16
.Lsec_185_1b1:
        memw
        retw.n
        .size   ppMapTxQueue, .-ppMapTxQueue
.Lsec_185_1b6:

        .section .text.ppDiscardMPDU,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_186_0, ppEnqueueTxDone
        .literal .Lsec_186_4, pp_post
.Lsec_186_8:
        .global ppDiscardMPDU
        .type   ppDiscardMPDU, @function
ppDiscardMPDU:
        entry    a1, 32
        l32i.n   a8, a2, 44
        movi.n   a3, 0
        movi.n   a9, 4
        mov.n    a10, a2
        s32i.n   a3, a8, 20
        s8i      a9, a8, 19
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, ppEnqueueTxDone
        l32r     a8, .Lsec_186_0
        callx8   a8
        mov.n    a11, a3
        movi.n   a10, 16
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_post
        l32r     a8, .Lsec_186_4
        callx8   a8
        retw.n
        .size   ppDiscardMPDU, .-ppDiscardMPDU
.Lsec_186_30:

        .section .wifi0iram.68,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_187_0, .Lsec_207_0
        .literal .Lsec_187_4, 0xff000fff
        .literal .Lsec_187_8, 0x00001166
        .literal .Lsec_187_c, .Lsec_117_0
        .literal .Lsec_187_10, .Lsec_210_0
        .literal .Lsec_187_14, 0xffc80fff
        .literal .Lsec_187_18, wifi_log
.Lsec_187_1c:
        .global ppProcTxSecFrame
        .type   ppProcTxSecFrame, @function
ppProcTxSecFrame:
        entry    a1, 32
        l32i.n   a12, a2, 44
        movi.n   a11, 4
        l32i.n   a8, a12, 16
        extui    a8, a8, 8, 4
        addi.n   a8, a8, -1
        extui    a8, a8, 0, 8
        l32i     a10, a2, 36
        bgeu     a8, a11, .Lsec_187_3c
        l32r     a9, .Lsec_187_0
        add      a8, a9, a8
        l8ui     a11, a8, 0
.Lsec_187_3c:
        l16ui    a8, a2, 22
        add.n    a8, a11, a8
        extui    a8, a8, 0, 16
        l32i.n   a13, a2, 8
        memw
        l32i.n   a9, a13, 0
        extui    a9, a9, 12, 12
        add.n    a9, a9, a11
        extui    a9, a9, 0, 12
        s16i     a8, a2, 22
        memw
        slli     a9, a9, 12
        memw
        l32i.n   a11, a13, 0
        l32r     a14, .Lsec_187_4
        and      a11, a11, a14
        or       a9, a11, a9
        movi.n   a11, 1
        memw
        s32i.n   a9, a13, 0
        movi.n   a9, 17
        slli     a9, a9, 14
        l32i.n   a14, a12, 0
        and      a14, a14, a9
        movi.n   a9, 1
        slli     a9, a9, 18
        beq      a14, a9, .Lsec_187_86
        j        .Lsec_187_122
.Lsec_187_86:
        beqz     a10, .Lsec_187_13a
        l32i.n   a14, a2, 4
        slli     a13, a11, 29
        memw
        l32i.n   a9, a14, 0
        or       a9, a9, a13
        movi.n   a13, 11
        slli     a13, a13, 8
        memw
        s32i.n   a9, a14, 0
        movi.n   a9, 15
        slli     a9, a9, 8
        l32i.n   a12, a12, 16
        and      a13, a12, a13
        and      a9, a12, a9
        movi.n   a12, 0
        mov.n    a15, a12
        movnez   a15, a11, a13
        addmi    a13, a9, 0xfffffd00
        movnez   a12, a11, a13
        bnone    a15, a12, .Lsec_187_e0
        addmi    a9, a9, 0xfffff700
        beqz.n   a9, .Lsec_187_e0
        l32r     a15, .Lsec_187_8
        mov.n    a12, a11
        movi     a10, 6
        slli     a11, a11, 11
        l32r     a14, .Lsec_187_c
        l32r     a13, .Lsec_187_10
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_187_18
        callx8   a8
.Lsec_187_dc:
        j        .Lsec_187_dc
        .byte    0x00
.Lsec_187_e0:
        l16ui    a2, a2, 20
        movi.n   a11, -1
        addi     a2, a2, -4
        add.n    a8, a8, a2
        slli     a11, a11, 12
        l32i.n   a9, a14, 4
        extui    a12, a8, 0, 12
        movi.n   a8, 0
        l32i.n   a2, a9, 0
        and      a2, a2, a11
        or       a2, a2, a12
        s32i.n   a2, a9, 0
        l16ui    a2, a10, 126
        addi.n   a10, a12, 3
        bge      a10, a2, .Lsec_187_10c
        sub      a2, a2, a12
        extui    a8, a2, 2, 7
.Lsec_187_10c:
        l32i.n   a2, a9, 0
        slli     a8, a8, 12
        l32r     a10, .Lsec_187_14
        and      a2, a2, a10
        or       a2, a2, a8
        s32i.n   a2, a9, 0
        memw
        j        .Lsec_187_132
.Lsec_187_122:
        memw
        l32i.n   a8, a13, 0
        slli     a11, a11, 30
        or       a8, a8, a11
        memw
        s32i.n   a8, a13, 0
.Lsec_187_132:
        movi.n   a2, 0
        memw
        j        .Lsec_187_13c
.Lsec_187_13a:
        mov.n    a2, a11
.Lsec_187_13c:
        retw.n
        .size   ppProcTxSecFrame, .-ppProcTxSecFrame
.Lsec_187_13e:

        .section .wifi0iram.70,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_188_0, RC_GetBlockAckTime
        .literal .Lsec_188_4, RC_GetAckTime
        .literal .Lsec_188_8, RC_GetCtsTime
.Lsec_188_c:
        .global ppCalFrameTimes
        .type   ppCalFrameTimes, @function
ppCalFrameTimes:
        entry    a1, 32
        l32i.n   a8, a2, 44
        movi.n   a12, 1
        slli     a12, a12, 19
        l32i     a3, a8, 0
        and      a12, a3, a12
        l8ui     a10, a8, 12
        beqz     a12, .Lsec_188_2c
        .reloc  ., R_XTENSA_ASM_EXPAND, RC_GetBlockAckTime
        l32r     a8, .Lsec_188_0
        callx8   a8
        j        .Lsec_188_38
        .byte    0x00
.Lsec_188_2c:
        movi     a8, 0x402
        bany     a3, a8, .Lsec_188_3a
        .reloc  ., R_XTENSA_ASM_EXPAND, RC_GetAckTime
        l32r     a8, .Lsec_188_4
        callx8   a8
.Lsec_188_38:
        mov.n    a12, a10
.Lsec_188_3a:
        l32i.n   a3, a2, 44
        l16ui    a11, a2, 22
        l16ui    a2, a2, 20
        add.n    a11, a11, a2
        extui    a11, a11, 0, 16
        l8ui     a10, a3, 12
        s16i     a12, a3, 10
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, RC_GetCtsTime
        l32r     a8, .Lsec_188_8
        callx8   a8
        s16i     a10, a3, 14
        memw
        memw
        retw.n
        .size   ppCalFrameTimes, .-ppCalFrameTimes
.Lsec_188_64:

        .section .wifi0iram.12,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_189_0, .Lsec_232_0
        .literal .Lsec_189_4, .Lsec_117_0
        .literal .Lsec_189_8, .Lsec_231_0
        .literal .Lsec_189_c, g_lmac_cnt
        .literal .Lsec_189_10, g_mesh_is_started
        .literal .Lsec_189_14, g_mesh_is_root
        .literal .Lsec_189_18, g_mesh_init_ps_type
        .literal .Lsec_189_1c, g_osi_funcs_p
        .literal .Lsec_189_20, 0x3ff73c00
        .literal .Lsec_189_24, g_wifi_mac_time_delta
        .literal .Lsec_189_28, pTxRx
        .literal .Lsec_189_2c, .Lsec_230_0
        .literal .Lsec_189_30, ic_interface_enabled
        .literal .Lsec_189_34, wifi_log
        .literal .Lsec_189_38, wifi_log
        .literal .Lsec_189_3c, ppTxProtoProc
        .literal .Lsec_189_40, ppProcTxSecFrame
        .literal .Lsec_189_44, rcGetSched
        .literal .Lsec_189_48, ppCalFrameTimes
        .literal .Lsec_189_4c, ppMapTxQueue
        .literal .Lsec_189_50, lmacIsIdle
        .literal .Lsec_189_54, pp_post
        .literal .Lsec_189_58, wifi_log
        .literal .Lsec_189_5c, esf_buf_recycle
        .literal .Lsec_189_60, pp_process_hmac_waiting_txq
.Lsec_189_64:
        .global ppTxPkt
        .type   ppTxPkt, @function
ppTxPkt:
        entry    a1, 32
        l32i.n   a4, a2, 44
        extui    a3, a3, 0, 8
        l32i     a10, a4, 16
        extui    a10, a10, 18, 2
        .reloc  ., R_XTENSA_ASM_EXPAND, ic_interface_enabled
        l32r     a8, .Lsec_189_30
        callx8   a8
        l32i.n   a4, a2, 44
        bnez.n   a10, .Lsec_189_94
        movi.n   a12, 2
        movi.n   a11, 64
        movi.n   a10, 6
        l32i.n   a14, a4, 16
        extui    a14, a14, 18, 2
        l32r     a13, .Lsec_189_0
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_189_34
        callx8   a8
        j        .Lsec_189_238
        .byte    0x00
.Lsec_189_94:
        l32i.n   a10, a4, 4
        movi.n   a8, 4
        extui    a4, a10, 0, 4
        bgeui    a4, 8, .Lsec_189_b9
        movi.n   a9, 9
        ssr      a4
        srl      a9, a9
        extui    a9, a9, 0, 1
        movi.n   a8, 2
        bnez.n   a9, .Lsec_189_b9
        movi.n   a8, 3
        bltu     a4, a8, .Lsec_189_b9
        movi.n   a8, 1
        bltui    a4, 6, .Lsec_189_b9
        mov.n    a8, a9
.Lsec_189_b9:
        extui    a10, a10, 4, 4
        beq      a10, a8, .Lsec_189_d8
        movi.n   a12, 1
        l32r     a14, .Lsec_189_4
        movi     a15, 0x38f
        slli     a11, a12, 11
        movi.n   a10, 6
        l32r     a13, .Lsec_189_8
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_189_38
        callx8   a8
.Lsec_189_d5:
        j        .Lsec_189_d5
.Lsec_189_d8:
        l32r     a4, .Lsec_189_c
        addi.n   a8, a8, 14
        addx8    a8, a8, a4
        mov.n    a10, a2
        l32i.n   a4, a8, 8
        addi.n   a4, a4, 1
        s32i.n   a4, a8, 8
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, ppTxProtoProc
        l32r     a8, .Lsec_189_3c
        callx8   a8
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, ppProcTxSecFrame
        l32r     a8, .Lsec_189_40
        callx8   a8
        bnei     a10, 1, .Lsec_189_ff
        j        .Lsec_189_238
.Lsec_189_ff:
        l32i.n   a11, a2, 44
        l32i.n   a10, a2, 36
        .reloc  ., R_XTENSA_ASM_EXPAND, rcGetSched
        l32r     a8, .Lsec_189_44
        callx8   a8
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, ppCalFrameTimes
        l32r     a8, .Lsec_189_48
        callx8   a8
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, ppMapTxQueue
        l32r     a8, .Lsec_189_4c
        callx8   a8
        bnez     a10, .Lsec_189_1b5
        l32r     a4, .Lsec_189_10
        l8ui     a4, a4, 0
        beqz.n   a4, .Lsec_189_148
        l32r     a4, .Lsec_189_14
        memw
        l8ui     a4, a4, 0
        extui    a4, a4, 0, 8
        bnez.n   a4, .Lsec_189_148
        l32r     a4, .Lsec_189_18
        l32i.n   a4, a4, 0
        beqz.n   a4, .Lsec_189_148
        l32r     a4, .Lsec_189_1c
        l32i.n   a4, a4, 0
        l32i     a10, a4, 0x110
        callx8   a10
        j        .Lsec_189_157
        .byte    0x00
.Lsec_189_148:
        l32r     a4, .Lsec_189_20
        memw
        l32i.n   a10, a4, 0
        l32r     a4, .Lsec_189_24
        l32i.n   a4, a4, 0
        add.n    a10, a10, a4
.Lsec_189_157:
        movi.n   a4, 0
        l32i.n   a9, a2, 44
        l32r     a8, .Lsec_189_28
        s32i.n   a10, a9, 24
        l32i.n   a10, a8, 0
        l32i.n   a8, a9, 16
        extui    a8, a8, 20, 4
        addx2    a8, a8, a8
        slli     a8, a8, 4
        add.n    a8, a10, a8
        l32i.n   a8, a8, 36
        s32i.n   a4, a2, 40
        s32i.n   a2, a8, 0
        l32i.n   a8, a9, 16
        extui    a8, a8, 20, 4
        addx2    a8, a8, a8
        slli     a8, a8, 4
        add.n    a10, a10, a8
        addi     a8, a2, 40
        s32i.n   a8, a10, 36
        bne      a3, a4, .Lsec_189_192
.Lsec_189_18a:
        movi.n   a2, 0
        memw
        j        .Lsec_189_24e
.Lsec_189_192:
        l32i.n   a10, a9, 4
        extui    a10, a10, 4, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacIsIdle
        l32r     a8, .Lsec_189_50
        callx8   a8
        beqz     a10, .Lsec_189_18a
        l32i.n   a2, a2, 44
        mov.n    a11, a4
        l32i     a10, a2, 4
        extui    a10, a10, 4, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_post
        l32r     a8, .Lsec_189_54
        callx8   a8
        mov.n    a2, a10
        j        .Lsec_189_24e
.Lsec_189_1b5:
        bnei     a10, 1, .Lsec_189_221
        l32r     a3, .Lsec_189_10
        l8ui     a3, a3, 0
        beqz.n   a3, .Lsec_189_1e4
        l32r     a3, .Lsec_189_14
        memw
        l8ui     a3, a3, 0
        extui    a3, a3, 0, 8
        bnez.n   a3, .Lsec_189_1e4
        l32r     a3, .Lsec_189_18
        l32i.n   a3, a3, 0
        beqz.n   a3, .Lsec_189_1e4
        l32r     a3, .Lsec_189_1c
        l32i.n   a3, a3, 0
        l32i     a10, a3, 0x110
        callx8   a10
        j        .Lsec_189_1f3
        .byte    0x00
.Lsec_189_1e4:
        l32r     a3, .Lsec_189_20
        memw
        l32i.n   a10, a3, 0
        l32r     a3, .Lsec_189_24
        l32i.n   a3, a3, 0
        add.n    a10, a10, a3
.Lsec_189_1f3:
        l32i.n   a4, a2, 44
        l32r     a3, .Lsec_189_28
        l32i.n   a8, a3, 0
        l32i.n   a3, a4, 16
        extui    a3, a3, 18, 2
        addx8    a3, a3, a8
        s32i.n   a10, a4, 24
        movi.n   a10, 0
        l32i     a3, a3, 0x338
        s32i.n   a10, a2, 40
        s32i.n   a2, a3, 0
        addi     a2, a2, 40
        l32i.n   a3, a4, 16
        extui    a3, a3, 18, 2
        addx8    a3, a3, a8
        s32i     a2, a3, 0x338
        memw
        j        .Lsec_189_18a
.Lsec_189_221:
        beqi     a10, 2, .Lsec_189_227
        j        .Lsec_189_18a
.Lsec_189_227:
        l32r     a13, .Lsec_189_2c
        or       a12, a10, a10
        movi     a11, 64
        movi.n   a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_189_58
        callx8   a8
.Lsec_189_238:
        l8ui     a3, a2, 26
        mov.n    a10, a2
        movi.n   a2, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, esf_buf_recycle
        l32r     a8, .Lsec_189_5c
        callx8   a8
        bne      a3, a2, .Lsec_189_24e
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_process_hmac_waiting_txq
        l32r     a8, .Lsec_189_60
        callx8   a8
.Lsec_189_24e:
        retw.n
        .size   ppTxPkt, .-ppTxPkt
.Lsec_189_250:

        .section .text.ppPrepareBarFrame,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_190_0, .Lsec_223_0
        .literal .Lsec_190_4, bars
        .literal .Lsec_190_8, esf_buf_alloc
        .literal .Lsec_190_c, wifi_log
        .literal .Lsec_190_10, rc_get_trc_by_index
        .literal .Lsec_190_14, rcGetSched
        .literal .Lsec_190_18, ppCalFrameTimes
.Lsec_190_1c:
        .global ppPrepareBarFrame
        .type   ppPrepareBarFrame, @function
ppPrepareBarFrame:
        entry    a1, 32
        movi.n   a12, 0
        movi     a11, 5
        mov.n    a10, a12
        .reloc  ., R_XTENSA_ASM_EXPAND, esf_buf_alloc
        l32r     a8, .Lsec_190_8
        callx8   a8
        extui    a3, a2, 0, 8
        mov.n    a2, a10
        bnez.n   a10, .Lsec_190_48
        l32r     a13, .Lsec_190_0
        movi.n   a12, 2
        movi     a11, 64
        movi     a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_190_c
        callx8   a8
        j        .Lsec_190_d3
        .byte    0x00
.Lsec_190_48:
        l32r     a8, .Lsec_190_4
        addx8    a9, a3, a3
        addx4    a9, a9, a8
        movi.n   a4, 24
        extui    a3, a3, 0, 4
        s16i     a4, a10, 22
        memw
        mov.n    a4, a9
        slli     a3, a3, 4
        movi.n   a11, 0
        s32i.n   a9, a10, 4
        movi.n   a10, -1
        slli     a10, a10, 12
        memw
        l32i.n   a8, a9, 0
        and      a8, a8, a10
        movi.n   a10, 24
        or       a8, a8, a10
        memw
        s32i.n   a8, a9, 0
        movi.n   a8, 1
        movi     a10, -241
        s32i.n   a9, a2, 8
        l32i.n   a9, a2, 44
        s16i     a8, a2, 12
        memw
        l32i.n   a8, a9, 4
        and      a8, a8, a10
        or       a3, a8, a3
        mov.n    a10, a11
        s32i.n   a3, a9, 4
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, rc_get_trc_by_index
        l32r     a8, .Lsec_190_10
        callx8   a8
        l32i.n   a11, a2, 44
        s32i.n   a10, a2, 36
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, rcGetSched
        l32r     a8, .Lsec_190_14
        callx8   a8
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, ppCalFrameTimes
        l32r     a8, .Lsec_190_18
        callx8   a8
        movi.n   a9, -7
        l16ui    a3, a2, 22
        slli     a9, a9, 19
        extui    a3, a3, 0, 12
        l32i.n   a8, a4, 12
        and      a8, a8, a9
        or       a3, a3, a8
        s32i.n   a3, a4, 12
        movi     a3, 132
        s16i     a3, a4, 16
        memw
.Lsec_190_d3:
        memw
        retw.n
        .size   ppPrepareBarFrame, .-ppPrepareBarFrame
.Lsec_190_d8:

        .section .text.pp_attach,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_191_0, pp_sig_cnt
        .literal .Lsec_191_4, bars
        .literal .Lsec_191_8, TxRxCxt
        .literal .Lsec_191_c, pTxRx
        .literal .Lsec_191_10, memset
        .literal .Lsec_191_14, memset
        .literal .Lsec_191_18, memset
        .literal .Lsec_191_1c, dbg_lmac_init
        .literal .Lsec_191_20, ppInitTxq
        .literal .Lsec_191_24, esf_buf_setup
        .literal .Lsec_191_28, pp_deattach
        .literal .Lsec_191_2c, ppPrepareBarFrame
.Lsec_191_30:
        .global pp_attach
        .type   pp_attach, @function
pp_attach:
        entry    a1, 32
        l32r     a10, .Lsec_191_0
        movi.n   a12, 30
        movi     a11, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, memset
        l32r     a8, .Lsec_191_10
        callx8   a8
        movi     a12, 144
        l32r     a10, .Lsec_191_4
        movi     a11, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, memset
        l32r     a8, .Lsec_191_14
        callx8   a8
        movi     a12, 0x3cc
        movi.n   a11, 0
        l32r     a10, .Lsec_191_8
        movi     a5, 0x640
        .reloc  ., R_XTENSA_ASM_EXPAND, memset
        l32r     a8, .Lsec_191_18
        callx8   a8
        movi     a6, 200
        l32r     a4, .Lsec_191_c
        l32i.n   a3, a4, 0
        s32i     a5, a3, 0x3c4
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, dbg_lmac_init
        l32r     a8, .Lsec_191_1c
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, ppInitTxq
        l32r     a8, .Lsec_191_20
        callx8   a8
        movi     a9, 0x334
        movi.n   a3, 0
        l32i.n   a8, a4, 0
        mov.n    a10, a2
        add.n    a9, a8, a9
        s32i     a9, a8, 0x338
        movi     a9, 0x33c
        add.n    a9, a8, a9
        s32i     a9, a8, 0x340
        movi     a9, 0x344
        add.n    a9, a8, a9
        s32i     a9, a8, 0x348
        movi     a9, 0x34c
        add.n    a9, a8, a9
        s32i     a9, a8, 0x350
        movi     a9, 0x354
        add.n    a9, a8, a9
        s32i     a3, a8, 0x334
        s32i     a3, a8, 0x33c
        s32i     a3, a8, 0x344
        s32i     a3, a8, 0x34c
        s32i     a3, a8, 0x354
        s32i     a9, a8, 0x358
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, esf_buf_setup
        l32r     a8, .Lsec_191_24
        callx8   a8
        mov.n    a2, a10
        beq      a10, a3, .Lsec_191_d0
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_deattach
        l32r     a8, .Lsec_191_28
        callx8   a8
        j        .Lsec_191_101
        .byte    0x00
.Lsec_191_d0:
        mov.n    a10, a3
        l32i.n   a5, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, ppPrepareBarFrame
        l32r     a8, .Lsec_191_2c
        callx8   a8
        add.n    a8, a3, a6
        addx4    a8, a8, a5
        addi.n   a3, a3, 1
        s32i.n   a10, a8, 0
        bnei     a3, 4, .Lsec_191_d0
        movi     a9, 0x100
        l32i.n   a3, a4, 0
        movi.n   a4, 1
        addmi    a8, a3, 0x300
        s8i      a4, a3, 29
        movi.n   a4, 0
        s32i     a4, a3, 0x330
        s16i     a9, a8, 96
        s8i      a4, a3, 30
        memw
.Lsec_191_101:
        memw
        retw.n
        .size   pp_attach, .-pp_attach
.Lsec_191_106:

        .section .wifiextrairam.38,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_192_0, 0x00000b47
        .literal .Lsec_192_4, .Lsec_117_0
        .literal .Lsec_192_8, .Lsec_222_0
        .literal .Lsec_192_c, wifi_log
        .literal .Lsec_192_10, ppCalFrameTimes
.Lsec_192_14:
        .global ppAssembleAMPDU
        .type   ppAssembleAMPDU, @function
ppAssembleAMPDU:
        entry    a1, 32
        l32i.n   a8, a2, 44
        movi.n   a10, 9
        slli     a10, a10, 19
        l32i.n   a9, a8, 0
        or       a9, a9, a10
        s32i.n   a9, a8, 0
        l16ui    a9, a2, 20
        sub      a4, a4, a9
        s16i     a4, a2, 22
        memw
        l8ui     a4, a8, 4
        s32i.n   a4, a8, 4
        movi.n   a4, 15
        l8ui     a8, a8, 12
        addi     a8, a8, -16
        extui    a8, a8, 0, 8
        bgeu     a4, a8, .Lsec_192_60
        movi.n   a12, 1
        l32r     a15, .Lsec_192_0
        slli     a11, a12, 11
        movi.n   a10, 6
        l32r     a14, .Lsec_192_4
        l32r     a13, .Lsec_192_8
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_192_c
        callx8   a8
.Lsec_192_5d:
        j        .Lsec_192_5d
.Lsec_192_60:
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, ppCalFrameTimes
        l32r     a8, .Lsec_192_10
        callx8   a8
        l32i.n   a4, a3, 8
        movi.n   a9, 1
        slli     a9, a9, 30
        memw
        l32i.n   a8, a4, 0
        or       a8, a8, a9
        memw
        s32i.n   a8, a4, 0
        l32i.n   a3, a3, 44
        l32i.n   a2, a2, 44
        l32i.n   a3, a3, 24
        s32i.n   a3, a2, 24
        memw
        retw.n
        .size   ppAssembleAMPDU, .-ppAssembleAMPDU
.Lsec_192_89:

        .section .wifi0iram.41,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_193_0, pTxRx
        .literal .Lsec_193_4, 0xbfffffff
        .literal .Lsec_193_8, 0x00000c03
        .literal .Lsec_193_c, .Lsec_117_0
        .literal .Lsec_193_10, .Lsec_221_0
        .literal .Lsec_193_14, rcGetAmpduSched
        .literal .Lsec_193_18, ppCheckTxAMPDUlength
        .literal .Lsec_193_1c, ppEmptyDelimiterLength
        .literal .Lsec_193_20, ppCalSubFrameLength
        .literal .Lsec_193_24, ppCalSubFrameLength
        .literal .Lsec_193_28, wifi_log
        .literal .Lsec_193_2c, lmacAdjustTimestamp
        .literal .Lsec_193_30, ppCheckTxAMPDUlength
        .literal .Lsec_193_34, ppEmptyDelimiterLength
        .literal .Lsec_193_38, ppEmptyDelimiterLength
        .literal .Lsec_193_3c, ppAssembleAMPDU
        .literal .Lsec_193_40, ppPauseTxAMPDU
.Lsec_193_44:
        .global ppCalTxAMPDULength
        .type   ppCalTxAMPDULength, @function
ppCalTxAMPDULength:
        entry    a1, 112
        l32i.n   a11, a2, 44
        mov.n    a7, a2
        l32i.n   a2, a11, 0
        bbsi     a2, 14, .Lsec_193_58
        l32i.n   a10, a7, 36
        .reloc  ., R_XTENSA_ASM_EXPAND, rcGetAmpduSched
        l32r     a8, .Lsec_193_14
        callx8   a8
.Lsec_193_58:
        movi.n   a3, 0
        l32i.n   a2, a7, 44
        l32i.n   a4, a2, 16
        extui    a4, a4, 20, 4
        l32i.n   a2, a7, 36
        s32i.n   a3, a1, 48
        s32i.n   a2, a1, 52
        beq      a2, a3, .Lsec_193_7c
        movi.n   a3, 1
        l8ui     a2, a2, 130
        addi     a2, a2, -4
        extui    a2, a2, 0, 8
        bltui    a2, 2, .Lsec_193_7a
        l32i.n   a3, a1, 48
.Lsec_193_7a:
        s32i.n   a3, a1, 48
.Lsec_193_7c:
        movi.n   a3, 0
        mov.n    a6, a1
        l32r     a2, .Lsec_193_0
        l32i.n   a5, a2, 0
        addx2    a2, a4, a4
        slli     a2, a2, 4
        add.n    a5, a5, a2
        s8i      a3, a5, 76
        movi     a3, 0x101
        s16i     a3, a5, 64
        memw
        movi.n   a3, 0
        s32i     a3, a5, 68
        l8ui     a8, a7, 27
        s8i      a8, a5, 47
        memw
        l32i.n   a5, a7, 4
        l32i.n   a5, a5, 4
        l8ui     a5, a5, 3
        s32i.n   a7, a1, 40
        s32i.n   a5, a1, 56
        movi.n   a5, 1
        memw
        j        .Lsec_193_18d
.Lsec_193_ba:
        l32i.n   a11, a15, 8
        mov.n    a13, a15
        memw
        l32i.n   a10, a11, 0
        l32r     a8, .Lsec_193_4
        and      a10, a10, a8
        memw
        s32i.n   a10, a11, 0
        mov.n    a11, a3
        l32i.n   a12, a1, 56
        l32i.n   a14, a1, 48
        l32i.n   a10, a1, 52
        s32i     a15, a1, 76
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, ppCheckTxAMPDUlength
        l32r     a8, .Lsec_193_18
        callx8   a8
        mov.n    a12, a10
        l32r     a10, .Lsec_193_0
        l32i.n   a11, a10, 0
        add.n    a13, a11, a2
        l8ui     a10, a13, 76
        l32i     a15, a1, 76
        beqz     a10, .Lsec_193_14a
        l32i.n   a8, a1, 48
        and      a10, a5, a8
        beqz     a10, .Lsec_193_14a
        mov.n    a5, a10
        beq      a1, a6, .Lsec_193_14a
        l32i.n   a14, a6, 8
        movi.n   a10, 1
        slli     a10, a10, 30
        memw
        l32i.n   a5, a14, 0
        or       a5, a5, a10
        mov.n    a10, a6
        memw
        s32i.n   a5, a14, 0
        movi     a5, 2
        s8i      a5, a13, 65
        memw
        movi     a5, 0
        s32i     a11, a1, 64
        s32i     a12, a1, 68
        s32i     a13, a1, 72
        s32i     a15, a1, 76
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, ppEmptyDelimiterLength
        l32r     a8, .Lsec_193_1c
        callx8   a8
        sub      a10, a3, a10
        l32i     a13, a1, 72
        l32i     a15, a1, 76
        l32i     a12, a1, 68
        l32i     a11, a1, 64
        s32i     a10, a13, 68
.Lsec_193_14a:
        bnez.n   a12, .Lsec_193_189
        l32i.n   a2, a6, 8
        mov.n    a10, a6
        s32i.n   a12, a2, 8
        memw
        j        .Lsec_193_15e
.Lsec_193_158:
        l32i.n   a10, a2, 8
        s32i.n   a12, a10, 8
        mov.n    a10, a2
.Lsec_193_15e:
        l32i.n   a2, a10, 40
        bnez     a2, .Lsec_193_158
        add.n    a12, a4, a4
        s32i.n   a2, a6, 40
        addx2    a2, a4, a4
        slli     a2, a2, 4
        add.n    a2, a11, a2
        l32i.n   a13, a2, 32
        s32i.n   a13, a10, 40
        bnez.n   a13, .Lsec_193_17a
        addi     a10, a10, 40
        s32i.n   a10, a2, 36
.Lsec_193_17a:
        add.n    a2, a12, a4
        slli     a2, a2, 4
        add.n    a11, a11, a2
        s32i.n   a15, a11, 32
        memw
        j        .Lsec_193_192
.Lsec_193_189:
        add.n    a3, a3, a12
        mov.n    a6, a15
.Lsec_193_18d:
        l32i.n   a15, a6, 40
        bnez     a15, .Lsec_193_ba
.Lsec_193_192:
        l32i.n   a2, a7, 44
        l32i.n   a2, a2, 0
        bbci     a2, 22, .Lsec_193_1aa
.Lsec_193_199:
        addx2    a15, a4, a4
        slli     a15, a15, 4
        addi     a2, a15, 32
        s32i.n   a2, a1, 60
        memw
        j        .Lsec_193_295
.Lsec_193_1aa:
        mov.n    a10, a7
        .reloc  ., R_XTENSA_ASM_EXPAND, ppCalSubFrameLength
        l32r     a8, .Lsec_193_20
        callx8   a8
        mov.n    a10, a7
        .reloc  ., R_XTENSA_ASM_EXPAND, ppCalSubFrameLength
        l32r     a8, .Lsec_193_24
        callx8   a8
        beq      a10, a3, .Lsec_193_199
        movi.n   a12, 1
        l32r     a15, .Lsec_193_8
        slli     a11, a12, 11
        movi     a10, 6
        l32r     a14, .Lsec_193_c
        l32r     a13, .Lsec_193_10
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_193_28
        callx8   a8
.Lsec_193_1d4:
        j        .Lsec_193_1d4
        .byte    0x00
.Lsec_193_1d8:
        l32i.n   a10, a2, 44
        l32i.n   a10, a10, 0
        bbsi     a10, 18, .Lsec_193_1e2
        j        .Lsec_193_2a2
.Lsec_193_1e2:
        mov.n    a10, a2
        s32i     a15, a1, 76
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacAdjustTimestamp
        l32r     a8, .Lsec_193_2c
        callx8   a8
        mov.n    a13, a2
        l32i.n   a11, a2, 8
        l32r     a12, .Lsec_193_4
        memw
        l32i.n   a10, a11, 0
        and      a10, a10, a12
        memw
        s32i.n   a10, a11, 0
        mov.n    a11, a3
        l32i.n   a12, a1, 56
        l32i.n   a14, a1, 48
        l32i.n   a10, a1, 52
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, ppCheckTxAMPDUlength
        l32r     a8, .Lsec_193_30
        callx8   a8
        mov.n    a13, a10
        l32r     a8, .Lsec_193_0
        l32i     a15, a1, 76
        l32i.n   a11, a8, 0
        add.n    a12, a11, a15
        l8ui     a10, a12, 76
        beqz     a10, .Lsec_193_273
        l32i.n   a8, a1, 48
        bnone    a5, a8, .Lsec_193_273
        movi.n   a10, 1
        slli     a10, a10, 30
        l32i.n   a14, a6, 8
        memw
        l32i.n   a5, a14, 0
        or       a5, a5, a10
        mov.n    a10, a6
        memw
        s32i.n   a5, a14, 0
        movi.n   a5, 2
        s8i      a5, a12, 65
        memw
        movi.n   a5, 0
        s32i     a11, a1, 64
        s32i     a12, a1, 68
        s32i     a13, a1, 72
        s32i     a15, a1, 76
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, ppEmptyDelimiterLength
        l32r     a8, .Lsec_193_34
        callx8   a8
        sub      a10, a3, a10
        l32i     a12, a1, 68
        l32i     a15, a1, 76
        l32i     a13, a1, 72
        l32i     a11, a1, 64
        s32i     a10, a12, 68
.Lsec_193_273:
        beqz.n   a13, .Lsec_193_2a2
        add.n    a10, a11, a15
        l32i.n   a12, a10, 32
        l32i.n   a12, a12, 40
        s32i.n   a12, a10, 32
        bnez.n   a12, .Lsec_193_285
        l32i.n   a8, a1, 60
        add.n    a11, a11, a8
        s32i.n   a11, a10, 36
.Lsec_193_285:
        add.n    a3, a3, a13
        l32i.n   a8, a6, 8
        l32i.n   a10, a2, 4
        s32i.n   a2, a6, 40
        mov.n    a6, a2
        s32i.n   a10, a8, 8
        movi.n   a8, 0
        s32i.n   a8, a2, 40
.Lsec_193_295:
        l32r     a8, .Lsec_193_0
        l32i.n   a2, a8, 0
        add.n    a2, a2, a15
        l32i     a2, a2, 32
        bnez     a2, .Lsec_193_1d8
.Lsec_193_2a2:
        mov.n    a10, a6
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, ppEmptyDelimiterLength
        l32r     a8, .Lsec_193_38
        callx8   a8
        l32r     a8, .Lsec_193_0
        sub      a10, a3, a10
        bnei     a5, 1, .Lsec_193_2c4
        addx2    a3, a4, a4
        slli     a3, a3, 4
        l32i.n   a2, a8, 0
        add      a2, a2, a3
        s32i     a10, a2, 68
.Lsec_193_2c4:
        extui    a12, a10, 0, 16
        mov.n    a11, a6
        mov.n    a10, a7
        addi.n   a2, a4, 1
        s32i     a8, a1, 64
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, ppAssembleAMPDU
        l32r     a8, .Lsec_193_3c
        callx8   a8
        addx2    a2, a2, a2
        slli     a2, a2, 4
        mov.n    a10, a4
        l32i     a8, a1, 64
        l32i.n   a3, a8, 0
        add.n    a2, a3, a2
        s32i     a7, a2, 4
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, ppPauseTxAMPDU
        l32r     a8, .Lsec_193_40
        callx8   a8
        mov.n    a2, a7
        retw.n
        .size   ppCalTxAMPDULength, .-ppCalTxAMPDULength
.Lsec_193_2f8:

        .section .wifiextrairam.40,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_194_0, 0xdfffffff
        .literal .Lsec_194_4, 0xff000fff
        .literal .Lsec_194_8, 0xffb3feff
        .literal .Lsec_194_c, rcGetSched
        .literal .Lsec_194_10, ppCalFrameTimes
.Lsec_194_14:
        .global ppAMPDU2Normal
        .type   ppAMPDU2Normal, @function
ppAMPDU2Normal:
        entry    a1, 32
        l32i.n   a9, a2, 4
        l32i.n   a11, a9, 4
        l16ui    a8, a2, 20
        addi     a8, a8, -4
        extui    a8, a8, 0, 16
        l32i.n   a10, a11, 0
        addi.n   a11, a11, 4
        extui    a10, a10, 0, 12
        s16i     a8, a2, 20
        sub      a8, a10, a8
        s16i     a8, a2, 22
        memw
        memw
        l32i.n   a8, a9, 0
        l32r     a10, .Lsec_194_0
        and      a8, a8, a10
        memw
        s32i.n   a8, a9, 0
        memw
        l32i.n   a8, a9, 0
        extui    a8, a8, 12, 12
        addi     a8, a8, -4
        extui    a8, a8, 0, 12
        memw
        l32i.n   a10, a9, 0
        slli     a8, a8, 12
        l32r     a12, .Lsec_194_4
        and      a10, a10, a12
        or       a8, a10, a8
        memw
        s32i.n   a8, a9, 0
        s32i.n   a11, a9, 4
        l32i.n   a11, a2, 44
        l8ui     a8, a11, 4
        l32r     a9, .Lsec_194_8
        s32i.n   a8, a11, 4
        l32i.n   a8, a11, 0
        and      a8, a8, a9
        l32i.n   a10, a2, 36
        s32i.n   a8, a11, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, rcGetSched
        l32r     a8, .Lsec_194_c
        callx8   a8
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, ppCalFrameTimes
        l32r     a8, .Lsec_194_10
        callx8   a8
        movi.n   a8, 1
        l32i.n   a9, a2, 8
        slli     a8, a8, 30
        memw
        l32i.n   a2, a9, 0
        or       a2, a2, a8
        memw
        s32i.n   a2, a9, 0
        memw
        retw.n
        .size   ppAMPDU2Normal, .-ppAMPDU2Normal
.Lsec_194_aa:

        .section .wifi0iram.23,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_195_0, g_mesh_is_started
        .literal .Lsec_195_4, g_mesh_is_root
        .literal .Lsec_195_8, pTxRx
        .literal .Lsec_195_c, lmacIsIdle
        .literal .Lsec_195_10, ppSearchTxframe
        .literal .Lsec_195_14, pm_is_waked
        .literal .Lsec_195_18, ppAMPDU2Normal
        .literal .Lsec_195_1c, trc_isTxAmpduOperational
        .literal .Lsec_195_20, trc_tid_isTxAmpduOperational
        .literal .Lsec_195_24, ppCalTxAMPDULength
        .literal .Lsec_195_28, pp_coex_tx_request
        .literal .Lsec_195_2c, lmacTxFrame
.Lsec_195_30:
        .global ppProcessTxQ
        .type   ppProcessTxQ, @function
ppProcessTxQ:
        entry    a1, 32
        extui    a4, a2, 0, 8
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacIsIdle
        l32r     a8, .Lsec_195_c
        callx8   a8
        bnez.n   a10, .Lsec_195_45
.Lsec_195_40:
        movi.n   a2, -1
        j        .Lsec_195_144
.Lsec_195_45:
        l32r     a2, .Lsec_195_0
        l8ui     a2, a2, 0
        bnez.n   a2, .Lsec_195_60
.Lsec_195_4d:
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, ppSearchTxframe
        l32r     a8, .Lsec_195_10
        callx8   a8
        mov.n    a3, a10
        movi.n   a2, -2
        bnez.n   a10, .Lsec_195_7c
        j        .Lsec_195_144
        .byte    0x00
        .byte    0x00
.Lsec_195_60:
        l32r     a2, .Lsec_195_4
        memw
        l8ui     a2, a2, 0
        extui    a2, a2, 0, 8
        bnez     a2, .Lsec_195_4d
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_is_waked
        l32r     a8, .Lsec_195_14
        callx8   a8
        bnez     a10, .Lsec_195_4d
        j        .Lsec_195_40
        .byte    0x00
.Lsec_195_7c:
        l32i.n   a2, a10, 44
        l32i.n   a2, a2, 0
        bbsi     a2, 18, .Lsec_195_86
        j        .Lsec_195_12d
.Lsec_195_86:
        l32i.n   a10, a10, 36
        bnez.n   a10, .Lsec_195_99
.Lsec_195_8a:
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, ppAMPDU2Normal
        l32r     a8, .Lsec_195_18
        callx8   a8
        l32i.n   a11, a3, 40
        bnez.n   a11, .Lsec_195_c2
        j        .Lsec_195_115
.Lsec_195_99:
        .reloc  ., R_XTENSA_ASM_EXPAND, trc_isTxAmpduOperational
        l32r     a8, .Lsec_195_1c
        callx8   a8
        beqz     a10, .Lsec_195_8a
        l32i.n   a2, a3, 44
        l32i.n   a11, a2, 4
        extui    a11, a11, 0, 4
        l32i.n   a10, a3, 36
        .reloc  ., R_XTENSA_ASM_EXPAND, trc_tid_isTxAmpduOperational
        l32r     a8, .Lsec_195_20
        callx8   a8
        beqz     a10, .Lsec_195_8a
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, ppCalTxAMPDULength
        l32r     a8, .Lsec_195_24
        callx8   a8
        beqz     a10, .Lsec_195_8a
        j        .Lsec_195_12d
.Lsec_195_c2:
        l32i.n   a8, a3, 8
        movi.n   a2, 0
        s32i.n   a2, a3, 40
        s32i.n   a2, a8, 8
        mov.n    a8, a11
.Lsec_195_cc:
        mov.n    a13, a8
        l32i.n   a9, a8, 8
        l32i.n   a8, a8, 40
        s32i.n   a2, a9, 8
        bnez     a8, .Lsec_195_cc
        l32r     a2, .Lsec_195_8
        l32i.n   a12, a2, 0
        l32i.n   a2, a3, 44
        l32i.n   a8, a2, 16
        extui    a8, a8, 20, 4
        addx2    a10, a8, a8
        slli     a10, a10, 4
        add.n    a10, a12, a10
        l32i.n   a2, a10, 32
        add.n    a9, a8, a8
        s32i.n   a2, a13, 40
        bnez.n   a2, .Lsec_195_f8
        addi     a13, a13, 40
        s32i.n   a13, a10, 36
.Lsec_195_f8:
        add.n    a9, a9, a8
        addi.n   a8, a8, 1
        slli     a9, a9, 4
        addx2    a8, a8, a8
        add.n    a9, a12, a9
        slli     a8, a8, 4
        s32i.n   a11, a9, 32
        add.n    a12, a12, a8
        movi.n   a2, 0
        s32i.n   a2, a12, 4
        memw
        j        .Lsec_195_12d
.Lsec_195_115:
        l32i.n   a8, a3, 44
        l32i.n   a8, a8, 16
        extui    a8, a8, 20, 4
        addi.n   a8, a8, 1
        addx2    a8, a8, a8
        l32r     a2, .Lsec_195_8
        slli     a8, a8, 4
        l32i.n   a2, a2, 0
        add.n    a2, a2, a8
        s32i.n   a11, a2, 4
.Lsec_195_12d:
        mov.n    a10, a3
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_coex_tx_request
        l32r     a8, .Lsec_195_28
        callx8   a8
        mov.n    a11, a4
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacTxFrame
        l32r     a8, .Lsec_195_2c
        callx8   a8
        movi.n   a2, 0
.Lsec_195_144:
        retw.n
        .size   ppProcessTxQ, .-ppProcessTxQ
.Lsec_195_146:

        .section .wifi0iram.44,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_196_0, pTxRx
        .literal .Lsec_196_4, .Lsec_117_0
        .literal .Lsec_196_8, .Lsec_220_0
        .literal .Lsec_196_c, GetAccess
        .literal .Lsec_196_10, wifi_log
        .literal .Lsec_196_14, memcpy
        .literal .Lsec_196_18, lmacMSDUAged
        .literal .Lsec_196_1c, ppCalSubFrameLength
        .literal .Lsec_196_20, ppSubFromAMPDU
        .literal .Lsec_196_24, lmacRecycleMPDU
        .literal .Lsec_196_28, lmacDiscardAgedMSDU
        .literal .Lsec_196_2c, rcUpdateTxDoneAmpdu2
        .literal .Lsec_196_30, trc_isTxAmpduOperational
        .literal .Lsec_196_34, trc_tid_isTxAmpduOperational
        .literal .Lsec_196_38, ppAMPDU2Normal
        .literal .Lsec_196_3c, ppFillAMPDUBar
        .literal .Lsec_196_40, ppReSendBar
        .literal .Lsec_196_44, ppResumeTxAMPDU
        .literal .Lsec_196_48, ppProcessTxQ
.Lsec_196_4c:
        .global ppResortTxAMPDU
        .type   ppResortTxAMPDU, @function
ppResortTxAMPDU:
        entry    a1, 192
        l32r     a6, .Lsec_196_0
        extui    a2, a2, 0, 8
        addx2    a5, a2, a2
        slli     a5, a5, 4
        add.n    a4, a2, a2
        l32i.n   a3, a6, 0
        movi.n   a10, 4
        add.n    a3, a3, a5
        l8ui     a3, a3, 44
        bgeui    a3, 8, .Lsec_196_7f
        beqz.n   a3, .Lsec_196_7d
        beqi     a3, 3, .Lsec_196_7d
        movi.n   a10, 3
        bltu     a3, a10, .Lsec_196_7f
        movi.n   a10, 1
        bltui    a3, 6, .Lsec_196_7f
        movi.n   a10, 0
        j        .Lsec_196_7f
.Lsec_196_7d:
        movi.n   a10, 2
.Lsec_196_7f:
        .reloc  ., R_XTENSA_ASM_EXPAND, GetAccess
        l32r     a8, .Lsec_196_c
        callx8   a8
        l32i.n   a9, a6, 0
        addx2    a8, a2, a2
        slli     a8, a8, 4
        add.n    a8, a9, a8
        l32i.n   a7, a8, 52
        s32i     a10, a1, 116
        beqz     a7, .Lsec_196_415
        add.n    a5, a4, a2
        slli     a5, a5, 4
        add.n    a5, a9, a5
        l8ui     a6, a5, 41
        bnez.n   a6, .Lsec_196_c4
        movi     a15, 0x669
        movi.n   a12, 1
        l32r     a14, .Lsec_196_4
        add      a15, a15, a15
        slli     a11, a12, 11
        movi     a10, 6
        l32r     a13, .Lsec_196_8
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_196_10
        callx8   a8
.Lsec_196_c0:
        j        .Lsec_196_c0
        .byte    0x00
.Lsec_196_c4:
        l32i.n   a6, a7, 44
        movi.n   a12, 32
        mov.n    a11, a6
        addi     a10, a1, 48
        s32i     a8, a1, 148
        s32i     a9, a1, 152
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_196_14
        callx8   a8
        movi.n   a10, 0
        s16i     a10, a1, 54
        movi.n   a10, 5
        s8i      a10, a1, 67
        memw
        addi     a10, a1, 80
        s32i.n   a10, a1, 8
        l32i.n   a10, a7, 4
        s32i     a10, a1, 88
        l8ui     a10, a5, 77
        s32i.n   a7, a1, 40
        l32i     a8, a1, 148
        l32i     a9, a1, 152
        bnez.n   a10, .Lsec_196_110
        l8ui     a10, a5, 65
        bnei     a10, 2, .Lsec_196_110
        l32i     a5, a5, 68
        memw
        j        .Lsec_196_118
        .byte    0x00
.Lsec_196_110:
        l16ui    a10, a7, 22
        l16ui    a5, a7, 20
        add.n    a5, a10, a5
.Lsec_196_118:
        add.n    a4, a4, a2
        slli     a4, a4, 4
        add.n    a9, a9, a4
        l16ui    a4, a9, 42
        s32i     a4, a1, 124
        movi.n   a4, 1
        ssl      a3
        sll      a4, a4
        s32i     a5, a1, 112
        l32i.n   a5, a8, 60
        l32i.n   a8, a8, 56
        s32i     a4, a1, 100
        movi.n   a4, -1
        s32i     a8, a1, 132
        l32i     a8, a1, 100
        xor      a4, a4, a8
        addx2    a8, a2, a2
        extui    a4, a4, 0, 8
        l8ui     a9, a9, 40
        slli     a8, a8, 4
        l8ui     a6, a6, 5
        s32i     a9, a1, 136
        movi.n   a9, 0
        s32i     a6, a1, 104
        s32i     a5, a1, 128
        s32i     a4, a1, 120
        mov.n    a4, a9
        l32i.n   a6, a7, 36
        l32r     a5, .Lsec_196_0
        s32i     a9, a1, 108
        s32i     a1, a1, 96
        s32i     a8, a1, 140
        memw
        j        .Lsec_196_2e8
.Lsec_196_175:
        l32i.n   a10, a5, 0
        l32i     a13, a1, 140
        add.n    a10, a10, a13
        l8ui     a11, a10, 77
        bnez.n   a11, .Lsec_196_1a9
        l8ui     a10, a10, 65
        bnei     a10, 2, .Lsec_196_1a9
        bnez.n   a4, .Lsec_196_194
        l8ui     a10, a1, 55
        addi.n   a10, a10, 1
        s8i      a10, a1, 55
        memw
.Lsec_196_194:
        l32i.n   a10, a8, 8
        memw
        l32i.n   a10, a10, 0
        bbci     a10, 30, .Lsec_196_1b4
        addi.n   a4, a4, 1
        extui    a4, a4, 0, 8
        memw
        j        .Lsec_196_1b4
.Lsec_196_1a9:
        l8ui     a10, a1, 55
        addi.n   a10, a10, 1
        s8i      a10, a1, 55
        memw
.Lsec_196_1b4:
        movi     a11, 192
        l32i     a10, a8, 4
        l32i     a10, a10, 4
        l32i     a13, a1, 124
        l8ui     a10, a10, 3
        sub      a10, a10, a13
        bany     a10, a11, .Lsec_196_1ee
        movi     a12, 224
        l32i     a13, a1, 128
        extui    a11, a10, 0, 8
        bnone    a10, a12, .Lsec_196_1d8
        l32i     a13, a1, 132
.Lsec_196_1d8:
        bbc      a13, a11, .Lsec_196_1ee
        movi.n   a12, 1
        l8ui     a10, a1, 54
        addi.n   a10, a10, 1
        s8i      a10, a1, 54
        memw
        memw
        j        .Lsec_196_249
.Lsec_196_1ee:
        mov.n    a10, a8
        s32i     a8, a1, 148
        s32i     a9, a1, 152
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacMSDUAged
        l32r     a8, .Lsec_196_18
        callx8   a8
        movi.n   a12, 0
        l32i     a8, a1, 148
        l32i     a9, a1, 152
        bne      a10, a12, .Lsec_196_249
        beq      a4, a12, .Lsec_196_21a
        bnei     a4, 1, .Lsec_196_238
        l32i.n   a10, a8, 8
        memw
        l32i.n   a10, a10, 0
        bbci     a10, 30, .Lsec_196_238
.Lsec_196_21a:
        l32i.n   a10, a8, 4
        l32i.n   a11, a10, 4
        l32i     a10, a8, 44
        l32i     a10, a10, 0
        bbci     a10, 18, .Lsec_196_22a
        addi     a11, a11, 4
.Lsec_196_22a:
        movi.n   a12, 8
        l8ui     a10, a11, 1
        or       a10, a10, a12
        s8i      a10, a11, 1
        memw
.Lsec_196_238:
        l32i     a13, a1, 96
        l32i.n   a13, a13, 40
        s32i     a8, a1, 108
        s32i     a13, a1, 96
        memw
        j        .Lsec_196_2e8
.Lsec_196_249:
        mov.n    a10, a8
        s32i     a9, a1, 152
        s32i     a12, a1, 144
        s32i     a8, a1, 148
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, ppCalSubFrameLength
        l32r     a8, .Lsec_196_1c
        callx8   a8
        l32i     a8, a1, 148
        or       a11, a8, a8
        l32i     a10, a1, 96
        .reloc  ., R_XTENSA_ASM_EXPAND, ppSubFromAMPDU
        l32r     a8, .Lsec_196_20
        callx8   a8
        movi.n   a10, 1
        slli     a10, a10, 22
        l32i     a8, a1, 148
        l32i.n   a14, a8, 44
        l32i.n   a11, a14, 0
        or       a10, a11, a10
        l32i     a12, a1, 144
        s32i.n   a10, a14, 0
        l32i     a9, a1, 152
        beqz.n   a12, .Lsec_196_2c4
        l8ui     a10, a6, 38
        l32i     a13, a1, 100
        and      a12, a10, a13
        bne      a13, a12, .Lsec_196_2ad
        movi.n   a9, 0
        bbci     a11, 19, .Lsec_196_2a1
        l32i.n   a9, a8, 4
        l32i     a9, a9, 4
        bbci     a11, 18, .Lsec_196_2a1
        addi     a9, a9, 4
.Lsec_196_2a1:
        l32i     a13, a1, 120
        and      a10, a10, a13
        s8i      a10, a6, 38
        memw
.Lsec_196_2ad:
        movi.n   a12, 0
        mov.n    a11, a8
        l32i     a10, a1, 116
        s32i     a9, a1, 152
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacRecycleMPDU
        l32r     a8, .Lsec_196_24
        callx8   a8
        j        .Lsec_196_2e5
        .byte    0x00
.Lsec_196_2c4:
        mov.n    a9, a12
        bbci     a11, 19, .Lsec_196_2d2
        l32i.n   a9, a8, 4
        l32i.n   a9, a9, 4
        bbci     a11, 18, .Lsec_196_2d2
        addi.n   a9, a9, 4
.Lsec_196_2d2:
        movi.n   a12, 0
        mov.n    a11, a8
        l32i     a10, a1, 116
        s32i     a9, a1, 152
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacDiscardAgedMSDU
        l32r     a8, .Lsec_196_28
        callx8   a8
.Lsec_196_2e5:
        l32i     a9, a1, 152
.Lsec_196_2e8:
        l32i     a13, a1, 96
        l32i.n   a8, a13, 40
        bnez     a8, .Lsec_196_175
        l32i     a4, a1, 136
        bnez.n   a4, .Lsec_196_311
        movi.n   a4, 1
        s8i      a4, a1, 67
        memw
        l32i.n   a4, a7, 44
        l8ui     a4, a4, 13
        s8i      a4, a1, 61
        memw
        memw
        j        .Lsec_196_31c
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_196_311:
        l32i     a8, a1, 104
        addi.n   a4, a8, 1
        extui    a4, a4, 0, 8
        s32i     a4, a1, 104
.Lsec_196_31c:
        addi     a11, a1, 48
        mov.n    a10, a6
        l32i     a4, a1, 112
        extui    a12, a4, 0, 16
        addi.n   a4, a2, 1
        l32i     a13, a1, 104
        s32i     a9, a1, 152
        s8i      a13, a1, 53
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, rcUpdateTxDoneAmpdu2
        l32r     a8, .Lsec_196_2c
        callx8   a8
        mov.n    a10, a6
        l32i     a7, a1, 40
        .reloc  ., R_XTENSA_ASM_EXPAND, trc_isTxAmpduOperational
        l32r     a8, .Lsec_196_30
        callx8   a8
        l32i     a9, a1, 152
        beqz.n   a10, .Lsec_196_380
        mov.n    a11, a3
        mov.n    a10, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, trc_tid_isTxAmpduOperational
        l32r     a8, .Lsec_196_34
        callx8   a8
        l32i     a9, a1, 152
        beqz.n   a10, .Lsec_196_380
        addx2    a4, a4, a4
        slli     a4, a4, 4
        l32i.n   a5, a5, 0
        add.n    a4, a5, a4
        s32i.n   a7, a4, 4
        beqz     a7, .Lsec_196_3ea
        movi.n   a5, 9
        slli     a5, a5, 19
        l32i.n   a6, a7, 44
        l32i.n   a4, a6, 0
        or       a4, a4, a5
        s32i.n   a4, a6, 0
        memw
        j        .Lsec_196_3ea
.Lsec_196_380:
        beqz     a7, .Lsec_196_3dc
        l32i.n   a6, a5, 0
        addx2    a8, a4, a4
        slli     a8, a8, 4
        add.n    a6, a6, a8
        l32i.n   a6, a6, 4
        bne      a6, a7, .Lsec_196_3a4
        or       a10, a7, a7
        s32i     a9, a1, 152
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, ppAMPDU2Normal
        l32r     a8, .Lsec_196_38
        callx8   a8
        l32i     a9, a1, 152
.Lsec_196_3a4:
        mov.n    a6, a7
        movi.n   a8, 0
        j        .Lsec_196_3b4
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_196_3ae:
        l32i.n   a10, a6, 8
        l32i.n   a6, a6, 40
        s32i.n   a8, a10, 8
.Lsec_196_3b4:
        l32i     a13, a1, 108
        bne      a6, a13, .Lsec_196_3ae
        addx2    a8, a2, a2
        slli     a8, a8, 4
        l32i.n   a10, a5, 0
        add.n    a11, a2, a2
        add.n    a8, a10, a8
        l32i.n   a12, a8, 32
        s32i.n   a12, a6, 40
        bnez.n   a12, .Lsec_196_3d2
        addi     a6, a6, 40
        s32i     a6, a8, 36
.Lsec_196_3d2:
        add.n    a6, a11, a2
        slli     a6, a6, 4
        add      a10, a10, a6
        s32i.n   a7, a10, 32
.Lsec_196_3dc:
        addx2    a4, a4, a4
        slli     a4, a4, 4
        l32i.n   a5, a5, 0
        add.n    a4, a5, a4
        movi.n   a5, 0
        s32i.n   a5, a4, 4
.Lsec_196_3ea:
        beqz.n   a9, .Lsec_196_415
        addi.n   a11, a9, 4
        addi.n   a12, a9, 10
        mov.n    a10, a3
        l16ui    a13, a9, 22
        srli     a13, a13, 4
        addi.n   a13, a13, 1
        extui    a13, a13, 0, 12
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, ppFillAMPDUBar
        l32r     a8, .Lsec_196_3c
        callx8   a8
        mov.n    a11, a2
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, ppReSendBar
        l32r     a8, .Lsec_196_40
        callx8   a8
        movi.n   a2, 0
        j        .Lsec_196_440
.Lsec_196_415:
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, ppResumeTxAMPDU
        l32r     a8, .Lsec_196_44
        callx8   a8
        movi.n   a10, 4
        bgeui    a3, 8, .Lsec_196_438
        beqz.n   a3, .Lsec_196_436
        beqi     a3, 3, .Lsec_196_436
        movi.n   a10, 3
        bltu     a3, a10, .Lsec_196_438
        movi.n   a10, 1
        bltui    a3, 6, .Lsec_196_438
        movi.n   a10, 0
        j        .Lsec_196_438
.Lsec_196_436:
        movi.n   a10, 2
.Lsec_196_438:
        .reloc  ., R_XTENSA_ASM_EXPAND, ppProcessTxQ
        l32r     a8, .Lsec_196_48
        callx8   a8
        mov.n    a2, a10
.Lsec_196_440:
        retw.n
        .size   ppResortTxAMPDU, .-ppResortTxAMPDU
.Lsec_196_442:

        .section .wifi0iram.9,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_197_0, .Lsec_252_0
        .literal .Lsec_197_4, g_osi_funcs_p
        .literal .Lsec_197_8, xphyQueue
        .literal .Lsec_197_c, g_intr_lock_mux
        .literal .Lsec_197_10, pp_sig_cnt
        .literal .Lsec_197_14, .Lsec_198_0
        .literal .Lsec_197_18, .Lsec_256_0
        .literal .Lsec_197_1c, .Lsec_255_0
        .literal .Lsec_197_20, .Lsec_257_0
        .literal .Lsec_197_24, .Lsec_233_0
        .literal .Lsec_197_28, .Lsec_253_0
        .literal .Lsec_197_2c, s_wifi_queue
        .literal .Lsec_197_30, .Lsec_254_0
        .literal .Lsec_197_34, pp_timer_do_process
        .literal .Lsec_197_38, ppProcessTxQ
        .literal .Lsec_197_3c, ppProcTxDone
        .literal .Lsec_197_40, ppRxPkt
        .literal .Lsec_197_44, ppResortTxAMPDU
        .literal .Lsec_197_48, ppProcessRxPktHdr
        .literal .Lsec_197_4c, lmacProcessTxTimeout
        .literal .Lsec_197_50, lmacProcessTxComplete
        .literal .Lsec_197_54, lmacProcessCollisions_task
        .literal .Lsec_197_58, wifi_gpio_debug
        .literal .Lsec_197_5c, wdevProcessRxSucDataAll
        .literal .Lsec_197_60, wdev_process_misc_pkt
        .literal .Lsec_197_64, wifi_log
        .literal .Lsec_197_68, pp_default_event_handler
.Lsec_197_6c:
        .global ppTask
        .type   ppTask, @function
ppTask:
        entry    a1, 48
        l32r     a2, .Lsec_197_4
        l32r     a4, .Lsec_197_0
        l32i.n   a3, a2, 0
        l32i.n   a10, a4, 0
        movi.n   a4, 0
        l32i     a3, a3, 64
        callx8   a3
        l32r     a3, .Lsec_197_8
        j        .Lsec_197_8a
        .byte    0x00
.Lsec_197_88:
        movi.n   a4, 1
.Lsec_197_8a:
        l32i.n   a5, a2, 0
        movi.n   a12, -1
        mov.n    a11, a1
        l32i     a5, a5, 116
        l32i     a10, a3, 0
        callx8   a5
        beqi     a10, 1, .Lsec_197_9f
        j        .Lsec_197_224
.Lsec_197_9f:
        movi.n   a8, 29
        l32i.n   a5, a1, 0
        bltu     a8, a5, .Lsec_197_e6
        addi     a5, a5, -13
        movi.n   a8, 0
        moveqz   a10, a8, a5
        extui    a10, a10, 0, 8
        beq      a10, a8, .Lsec_197_e6
        l32r     a5, .Lsec_197_c
        l32i.n   a8, a2, 0
        l32i.n   a10, a5, 0
        l32i.n   a8, a8, 40
        callx8   a8
        mov.n    a11, a10
        l32i.n   a8, a1, 0
        l32r     a9, .Lsec_197_10
        add.n    a9, a9, a8
        memw
        l8ui     a8, a9, 0
        addi.n   a8, a8, -1
        extui    a8, a8, 0, 8
        memw
        s8i      a8, a9, 0
        l32i.n   a8, a2, 0
        l32i.n   a10, a5, 0
        l32i.n   a8, a8, 44
        memw
        callx8   a8
.Lsec_197_e6:
        movi.n   a5, 26
        l32i.n   a10, a1, 0
        bgeu     a5, a10, .Lsec_197_f0
        j        .Lsec_197_21c
.Lsec_197_f0:
        l32r     a5, .Lsec_197_14
        addx4    a5, a10, a5
        l32i.n   a5, a5, 0
        jx       a5
        .byte    0x00
.Lsec_197_fc:
        l32i.n   a5, a2, 0
        l32i     a5, a5, 216
        callx8   a5
        l32i.n   a10, a1, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_timer_do_process
        l32r     a8, .Lsec_197_34
        callx8   a8
        j        .Lsec_197_128
        .byte    0x00
        .byte    0x00
.Lsec_197_111:
        l32i.n   a8, a2, 0
        l32r     a5, .Lsec_197_18
.Lsec_197_116:
        l32i.n   a9, a5, 0
        beqz     a9, .Lsec_197_224
        l32i     a8, a8, 216
        callx8   a8
        l32i.n   a10, a1, 4
        l32i.n   a5, a5, 0
        callx8   a5
.Lsec_197_128:
        l32i.n   a5, a2, 0
        l32i     a5, a5, 220
        callx8   a5
        j        .Lsec_197_224
        .byte    0x00
.Lsec_197_134:
        l32i.n   a8, a2, 0
        l32r     a5, .Lsec_197_1c
        j        .Lsec_197_116
.Lsec_197_13c:
        movi.n   a10, 4
.Lsec_197_13e:
        .reloc  ., R_XTENSA_ASM_EXPAND, ppProcessTxQ
        l32r     a8, .Lsec_197_38
        callx8   a8
        j        .Lsec_197_224
        .byte    0x00
.Lsec_197_148:
        l32r     a5, .Lsec_197_20
        l32i.n   a10, a5, 0
        callx8   a10
        j        .Lsec_197_224
        .byte    0x00
.Lsec_197_154:
        movi.n   a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, ppProcTxDone
        l32r     a8, .Lsec_197_3c
        callx8   a8
        j        .Lsec_197_224
        .byte    0x00
.Lsec_197_160:
        l32i.n   a5, a2, 0
        l32i     a5, a5, 216
        callx8   a5
        .reloc  ., R_XTENSA_ASM_EXPAND, ppRxPkt
        l32r     a8, .Lsec_197_40
        callx8   a8
        j        .Lsec_197_128
.Lsec_197_171:
        l32i.n   a5, a2, 0
        l32i     a5, a5, 216
        callx8   a5
        l8ui     a10, a1, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, ppResortTxAMPDU
        l32r     a8, .Lsec_197_44
        callx8   a8
        j        .Lsec_197_128
.Lsec_197_185:
        l32i.n   a5, a2, 0
        l32i     a5, a5, 216
        callx8   a5
        l32i.n   a10, a1, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, ppProcessRxPktHdr
        l32r     a8, .Lsec_197_48
        callx8   a8
        j        .Lsec_197_128
.Lsec_197_198:
        l32i.n   a5, a2, 0
        l32i     a5, a5, 216
        callx8   a5
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacProcessTxTimeout
        l32r     a8, .Lsec_197_4c
        callx8   a8
        j        .Lsec_197_1ca
.Lsec_197_1a9:
        l32i.n   a5, a2, 0
        l32i     a5, a5, 216
        callx8   a5
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacProcessTxComplete
        l32r     a8, .Lsec_197_50
        callx8   a8
        j        .Lsec_197_128
        .byte    0x00
        .byte    0x00
.Lsec_197_1bc:
        l32i.n   a5, a2, 0
        l32i     a5, a5, 216
        callx8   a5
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacProcessCollisions_task
        l32r     a8, .Lsec_197_54
        callx8   a8
.Lsec_197_1ca:
        l32i.n   a5, a2, 0
        l32i     a5, a5, 220
        callx8   a5
        movi.n   a11, 4
        movi.n   a10, 10
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_gpio_debug
        l32r     a8, .Lsec_197_58
        callx8   a8
        j        .Lsec_197_224
        .byte    0x00
.Lsec_197_1e0:
        l32i.n   a5, a2, 0
        l32i     a5, a5, 216
        callx8   a5
        l32i.n   a10, a1, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, wdevProcessRxSucDataAll
        l32r     a8, .Lsec_197_5c
        callx8   a8
        j        .Lsec_197_128
        .byte    0x00
.Lsec_197_1f4:
        l32i.n   a5, a2, 0
        l32i     a5, a5, 216
        callx8   a5
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_process_misc_pkt
        l32r     a8, .Lsec_197_60
        callx8   a8
        j        .Lsec_197_128
        .byte    0x00
.Lsec_197_206:
        movi.n   a12, 1
        l32i.n   a14, a1, 4
        slli     a11, a12, 11
        movi.n   a10, 6
        l32r     a13, .Lsec_197_24
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_197_64
        callx8   a8
.Lsec_197_218:
        j        .Lsec_197_218
        .byte    0x00
.Lsec_197_21c:
        l32i.n   a11, a1, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_default_event_handler
        l32r     a8, .Lsec_197_68
        callx8   a8
.Lsec_197_224:
        beqz     a4, .Lsec_197_8a
.Lsec_197_227:
        l32i.n   a4, a2, 0
        l32i.n   a10, a3, 0
        l32i     a4, a4, 120
        callx8   a4
        bnez     a10, .Lsec_197_88
        l32r     a3, .Lsec_197_28
        l32i.n   a10, a3, 0
        beqz.n   a10, .Lsec_197_244
        l32i     a3, a2, 0
        l32i     a3, a3, 64
        callx8   a3
.Lsec_197_244:
        l32r     a3, .Lsec_197_2c
        l32i.n   a4, a2, 0
        l32i.n   a10, a3, 0
        l32i     a4, a4, 0x180
        callx8   a4
        movi.n   a10, 0
        l32i.n   a2, a2, 0
        s32i.n   a10, a3, 0
        l32r     a3, .Lsec_197_30
        l32i     a2, a2, 152
        s32i.n   a10, a3, 0
        memw
        callx8   a2
        retw.n
        .size   ppTask, .-ppTask
.Lsec_197_267:

        .section .rodata,"a",@progbits
        .align  4
.Lsec_198_0:
        .word   .Lsec_197_13e
        .word   .Lsec_197_13e
        .word   .Lsec_197_13e
        .word   .Lsec_197_13e
        .word   .Lsec_197_13c
        .word   .Lsec_197_148
        .word   .Lsec_197_134
        .word   .Lsec_197_111
        .word   .Lsec_197_fc
        .word   .Lsec_197_21c
        .word   .Lsec_197_21c
        .word   .Lsec_197_21c
        .word   .Lsec_197_21c
        .word   .Lsec_197_185
        .word   .Lsec_197_206
        .word   .Lsec_197_227
        .word   .Lsec_197_154
        .word   .Lsec_197_160
        .word   .Lsec_197_171
        .word   .Lsec_197_224
        .word   .Lsec_197_224
        .word   .Lsec_197_224
        .word   .Lsec_197_198
        .word   .Lsec_197_1a9
        .word   .Lsec_197_1bc
        .word   .Lsec_197_1e0
        .word   .Lsec_197_1f4

        .section .wifiextrairam.71,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_199_0, pTxRx
        .literal .Lsec_199_4, ppResumeTxAMPDU
        .literal .Lsec_199_8, esf_buf_recycle
.Lsec_199_c:
        .global ppDirectRecycleAmpdu
        .type   ppDirectRecycleAmpdu, @function
ppDirectRecycleAmpdu:
        entry    a1, 32
        l32i.n   a3, a2, 44
        l32i.n   a10, a3, 16
        extui    a10, a10, 20, 4
        addi.n   a8, a10, 1
        addx2    a8, a8, a8
        l32r     a3, .Lsec_199_0
        slli     a8, a8, 4
        l32i.n   a3, a3, 0
        add.n    a8, a3, a8
        movi.n   a3, 0
        s32i.n   a3, a8, 4
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, ppResumeTxAMPDU
        l32r     a8, .Lsec_199_4
        callx8   a8
.Lsec_199_32:
        mov.n    a10, a2
        movi.n   a9, 1
        slli     a9, a9, 22
        l32i.n   a8, a10, 8
        l32i.n   a11, a10, 44
        l32i.n   a2, a2, 40
        s32i.n   a3, a10, 40
        s32i.n   a3, a8, 8
        l32i.n   a8, a11, 0
        or       a8, a8, a9
        s32i     a8, a11, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, esf_buf_recycle
        l32r     a8, .Lsec_199_8
        callx8   a8
        bnez     a2, .Lsec_199_32
        retw.n
        .size   ppDirectRecycleAmpdu, .-ppDirectRecycleAmpdu
.Lsec_199_59:

        .section .text.ppClearTxq,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_200_0, pTxRx
        .literal .Lsec_200_4, ppDirectRecycleAmpdu
        .literal .Lsec_200_8, esf_buf_recycle
        .literal .Lsec_200_c, ppDequeueTxQ
.Lsec_200_10:
        .global ppClearTxq
        .type   ppClearTxq, @function
ppClearTxq:
        entry    a1, 32
        l32r     a8, .Lsec_200_0
        extui    a2, a2, 0, 8
        l32i.n   a9, a8, 0
        addx2    a8, a2, a2
        slli     a8, a8, 4
        add.n    a8, a9, a8
        l8ui     a8, a8, 41
        bnez.n   a8, .Lsec_200_45
        addi.n   a8, a2, 1
        addx2    a8, a8, a8
        slli     a8, a8, 4
        add.n    a9, a9, a8
        l32i.n   a10, a9, 4
        beqz.n   a10, .Lsec_200_45
        .reloc  ., R_XTENSA_ASM_EXPAND, ppDirectRecycleAmpdu
        l32r     a8, .Lsec_200_4
        callx8   a8
        j        .Lsec_200_45
.Lsec_200_3f:
        .reloc  ., R_XTENSA_ASM_EXPAND, esf_buf_recycle
        l32r     a8, .Lsec_200_8
        callx8   a8
.Lsec_200_45:
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, ppDequeueTxQ
        l32r     a8, .Lsec_200_c
        callx8   a8
        bnez     a10, .Lsec_200_3f
        retw.n
        .size   ppClearTxq, .-ppClearTxq
.Lsec_200_52:

        .section .text.pp_stop_sw_txq,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_201_0, pTxRx
        .literal .Lsec_201_4, esf_buf_recycle
        .literal .Lsec_201_8, ppClearTxq
        .literal .Lsec_201_c, esf_buf_recycle
        .literal .Lsec_201_10, ppDequeueTxDone_Locked
        .literal .Lsec_201_14, ppClearRxFragment
.Lsec_201_18:
        .global pp_stop_sw_txq
        .type   pp_stop_sw_txq, @function
pp_stop_sw_txq:
        entry    a1, 32
        movi.n   a2, 0
        movi     a4, 0x334
        j        .Lsec_201_4f
        .byte    0x00
.Lsec_201_24:
        l32i.n   a11, a10, 40
        s32i     a11, a9, 0x334
        bnez.n   a11, .Lsec_201_30
        add.n    a8, a8, a3
        s32i     a8, a9, 0x338
.Lsec_201_30:
        movi.n   a8, 0
        s32i.n   a8, a10, 40
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, esf_buf_recycle
        l32r     a8, .Lsec_201_4
        callx8   a8
.Lsec_201_3d:
        l32r     a6, .Lsec_201_0
        l32i.n   a8, a6, 0
        add.n    a9, a8, a5
        l32i     a10, a9, 0x334
        bnez     a10, .Lsec_201_24
        addi.n   a2, a2, 1
        beqi     a2, 3, .Lsec_201_58
.Lsec_201_4f:
        slli     a5, a2, 3
        addx8    a3, a2, a4
        j        .Lsec_201_3d
.Lsec_201_58:
        mov.n    a2, a10
.Lsec_201_5a:
        mov.n    a10, a2
        addi.n   a2, a2, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, ppClearTxq
        l32r     a8, .Lsec_201_8
        callx8   a8
        bnei     a2, 16, .Lsec_201_5a
        j        .Lsec_201_72
        .byte    0x00
        .byte    0x00
.Lsec_201_6c:
        .reloc  ., R_XTENSA_ASM_EXPAND, esf_buf_recycle
        l32r     a8, .Lsec_201_c
        callx8   a8
.Lsec_201_72:
        .reloc  ., R_XTENSA_ASM_EXPAND, ppDequeueTxDone_Locked
        l32r     a8, .Lsec_201_10
        callx8   a8
        bnez     a10, .Lsec_201_6c
        .reloc  ., R_XTENSA_ASM_EXPAND, ppClearRxFragment
        l32r     a8, .Lsec_201_14
        callx8   a8
        retw.n
        .size   pp_stop_sw_txq, .-pp_stop_sw_txq
.Lsec_201_83:

        .section .wifiextrairam.72,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_202_0, pTxRx
        .literal .Lsec_202_4, GetAccess
        .literal .Lsec_202_8, ppResumeTxAMPDU
        .literal .Lsec_202_c, lmacDiscardAgedMSDU
        .literal .Lsec_202_10, lmacDiscardAgedMSDU
.Lsec_202_14:
        .global ppRecycleAmpdu
        .type   ppRecycleAmpdu, @function
ppRecycleAmpdu:
        entry    a1, 32
        l32i.n   a4, a2, 44
        extui    a3, a3, 0, 8
        l32i.n   a10, a4, 4
        extui    a10, a10, 4, 4
        l32i.n   a5, a4, 16
        .reloc  ., R_XTENSA_ASM_EXPAND, GetAccess
        l32r     a8, .Lsec_202_4
        callx8   a8
        extui    a5, a5, 20, 4
        addi.n   a8, a5, 1
        addx2    a8, a8, a8
        l32r     a4, .Lsec_202_0
        slli     a8, a8, 4
        mov.n    a6, a10
        mov.n    a10, a5
        l32i.n   a4, a4, 0
        add.n    a8, a4, a8
        movi.n   a4, 0
        s32i     a4, a8, 4
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, ppResumeTxAMPDU
        l32r     a8, .Lsec_202_8
        callx8   a8
.Lsec_202_4d:
        movi.n   a8, 1
        slli     a8, a8, 22
        l32i.n   a5, a2, 8
        l32i.n   a9, a2, 44
        l32i.n   a7, a2, 40
        s32i.n   a4, a2, 40
        s32i.n   a4, a5, 8
        l32i.n   a5, a9, 0
        or       a5, a5, a8
        s32i.n   a5, a9, 0
        beqz.n   a7, .Lsec_202_79
        mov.n    a11, a2
        mov.n    a12, a4
        mov.n    a10, a6
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacDiscardAgedMSDU
        l32r     a8, .Lsec_202_c
        callx8   a8
        mov.n    a2, a7
        j        .Lsec_202_4d
.Lsec_202_79:
        mov.n    a12, a3
        mov.n    a11, a2
        mov.n    a10, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacDiscardAgedMSDU
        l32r     a8, .Lsec_202_10
        callx8   a8
        retw.n
        .size   ppRecycleAmpdu, .-ppRecycleAmpdu
.Lsec_202_87:

        .section .wifiextrairam.73,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_203_0, pTxRx
        .literal .Lsec_203_4, ppAMPDU2Normal
        .literal .Lsec_203_8, ppResumeTxAMPDU
        .literal .Lsec_203_c, pp_post
.Lsec_203_10:
        .global ppRegressAmpdu
        .type   ppRegressAmpdu, @function
ppRegressAmpdu:
        entry    a1, 32
        l32i.n   a4, a2, 44
        extui    a3, a3, 0, 8
        mov.n    a5, a2
        movi.n   a9, 0
        l32i.n   a4, a4, 16
        extui    a4, a4, 20, 4
        j        .Lsec_203_2c
        .byte    0x00
        .byte    0x00
.Lsec_203_26:
        l32i.n   a5, a5, 8
        s32i.n   a9, a5, 8
        mov.n    a5, a8
.Lsec_203_2c:
        l32i.n   a8, a5, 40
        bnez     a8, .Lsec_203_26
        mov.n    a10, a2
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, ppAMPDU2Normal
        l32r     a8, .Lsec_203_4
        callx8   a8
        l32r     a8, .Lsec_203_0
        addx2    a9, a4, a4
        slli     a9, a9, 4
        l32i.n   a10, a8, 0
        add.n    a8, a4, a4
        add.n    a9, a10, a9
        l32i.n   a11, a9, 32
        s32i.n   a11, a5, 40
        bnez.n   a11, .Lsec_203_56
        addi     a5, a5, 40
        s32i.n   a5, a9, 36
.Lsec_203_56:
        add.n    a8, a8, a4
        addi.n   a5, a4, 1
        slli     a8, a8, 4
        addx2    a5, a5, a5
        add.n    a8, a10, a8
        slli     a5, a5, 4
        add.n    a10, a10, a5
        s32i.n   a2, a8, 32
        movi.n   a5, 0
        s32i.n   a5, a10, 4
        mov.n    a10, a4
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, ppResumeTxAMPDU
        l32r     a8, .Lsec_203_8
        callx8   a8
        beq      a3, a5, .Lsec_203_8a
        mov.n    a11, a5
        l32i.n   a2, a2, 44
        l32i.n   a10, a2, 4
        extui    a10, a10, 4, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_post
        l32r     a8, .Lsec_203_c
        callx8   a8
.Lsec_203_8a:
        retw.n
        .size   ppRegressAmpdu, .-ppRegressAmpdu
.Lsec_203_8c:

        .section .text.ppGetTaskHdl,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_204_0, .Lsec_254_0
.Lsec_204_4:
        .global ppGetTaskHdl
        .type   ppGetTaskHdl, @function
ppGetTaskHdl:
        entry    a1, 32
        l32r     a8, .Lsec_204_0
        l32i.n   a2, a8, 0
        retw.n
        .size   ppGetTaskHdl, .-ppGetTaskHdl
.Lsec_204_e:

        .section .text.ppMessageInQ,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_205_0, xphyQueue
        .literal .Lsec_205_4, g_osi_funcs_p
.Lsec_205_8:
        .global ppMessageInQ
        .type   ppMessageInQ, @function
ppMessageInQ:
        entry    a1, 32
        l32r     a8, .Lsec_205_4
        l32r     a9, .Lsec_205_0
        l32i.n   a8, a8, 0
        l32i     a10, a9, 0
        l32i     a8, a8, 120
        callx8   a8
        mov.n    a2, a10
        retw.n
        .size   ppMessageInQ, .-ppMessageInQ
.Lsec_205_20:

        .section .wifi0iram.74,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_206_0, lmacConfMib
        .literal .Lsec_206_4, 0x00010020
        .literal .Lsec_206_8, g_offchan_packet_lifetime
        .literal .Lsec_206_c, .Lsec_117_0
        .literal .Lsec_206_10, .Lsec_209_0
        .literal .Lsec_206_14, pm_get_null_max_tx_time
        .literal .Lsec_206_18, wifi_log
.Lsec_206_1c:
        .global ppProcessLifeTime
        .type   ppProcessLifeTime, @function
ppProcessLifeTime:
        entry    a1, 32
        l32i.n   a13, a2, 44
        l32i.n   a8, a13, 0
        l32r     a9, .Lsec_206_0
        bbci     a8, 22, .Lsec_206_2e
        l32i.n   a10, a9, 0
        j        .Lsec_206_30
.Lsec_206_2e:
        l32i.n   a10, a9, 8
.Lsec_206_30:
        slli     a10, a10, 10
        l32r     a9, .Lsec_206_4
        bnone    a8, a9, .Lsec_206_ad
        l32i.n   a2, a2, 4
        l32i.n   a11, a2, 4
        bbci     a8, 18, .Lsec_206_42
        addi.n   a11, a11, 4
.Lsec_206_42:
        movi.n   a2, -16
        movi.n   a12, 12
        l8ui     a8, a11, 0
        movi.n   a9, 1
        and      a12, a8, a12
        and      a8, a8, a2
        movi     a2, 0
        movnez   a9, a2, a12
        extui    a9, a9, 0, 8
        bnei     a8, 64, .Lsec_206_60
        bne      a9, a2, .Lsec_206_a8
.Lsec_206_60:
        bnei     a12, 8, .Lsec_206_74
        l8ui     a10, a11, 1
        extui    a10, a10, 4, 1
        l32i.n   a11, a13, 24
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_get_null_max_tx_time
        l32r     a8, .Lsec_206_14
        callx8   a8
        j        .Lsec_206_ad
.Lsec_206_74:
        movi     a2, -208
        add.n    a8, a8, a2
        bnez.n   a8, .Lsec_206_8c
        beqz.n   a9, .Lsec_206_8c
        l32r     a2, .Lsec_206_8
        l32i.n   a8, a2, 0
        beqz.n   a8, .Lsec_206_ad
        slli     a10, a8, 10
        j        .Lsec_206_ad
        .byte    0x00
        .byte    0x00
.Lsec_206_8c:
        movi     a15, 0x491
        movi.n   a12, 1
        l32r     a14, .Lsec_206_c
        slli     a15, a15, 2
        slli     a11, a12, 11
        movi.n   a10, 6
        l32r     a13, .Lsec_206_10
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_206_18
        callx8   a8
.Lsec_206_a5:
        j        .Lsec_206_a5
.Lsec_206_a8:
        movi.n   a10, 15
        slli     a10, a10, 13
.Lsec_206_ad:
        s32i.n   a10, a4, 0
        memw
        retw.n
        .size   ppProcessLifeTime, .-ppProcessLifeTime
.Lsec_206_b4:

        .section .rodata.CSWTCH$606,"a",@progbits
        .align  1
.Lsec_207_0:
        .byte   0x08, 0x10, 0x0c, 0x14

        .section .bss.current_ifidx$0,"aw",@nobits
        .align  1
.Lsec_208_0:
        .skip   1

        .section .rodata_wlog_error.75,"a",@progbits
        .align  1
.Lsec_209_0:
        .byte   0x25, 0x73, 0x20, 0x25, 0x75, 0x0a, 0x00

        .section .rodata_wlog_error.69,"a",@progbits
        .align  1
.Lsec_210_0:
        .byte   0x25, 0x73, 0x20, 0x25, 0x75, 0x0a, 0x00

        .section .rodata_wlog_warning.67,"a",@progbits
        .align  1
.Lsec_211_0:
        .byte   0x70, 0x70, 0x20, 0x74, 0x61, 0x73, 0x6b, 0x20, 0x71, 0x20, 0x66, 0x75, 0x6c, 0x6c, 0x3a, 0x20
        .byte   0x73, 0x3d, 0x25, 0x64, 0x20, 0x70, 0x3d, 0x25, 0x78, 0x0a, 0x00

        .section .rodata_wlog_info.58,"a",@progbits
        .align  1
.Lsec_212_0:
        .byte   0x77, 0x69, 0x66, 0x69, 0x20, 0x64, 0x72, 0x69, 0x76, 0x65, 0x72, 0x20, 0x74, 0x61, 0x73, 0x6b
        .byte   0x3a, 0x20, 0x25, 0x78, 0x2c, 0x20, 0x70, 0x72, 0x69, 0x6f, 0x3a, 0x25, 0x64, 0x2c, 0x20, 0x73
        .byte   0x74, 0x61, 0x63, 0x6b, 0x3a, 0x25, 0x64, 0x2c, 0x20, 0x63, 0x6f, 0x72, 0x65, 0x3d, 0x25, 0x64
        .byte   0x00

        .section .rodata_wlog_error.57,"a",@progbits
        .align  1
.Lsec_213_0:
        .byte   0x63, 0x72, 0x65, 0x61, 0x74, 0x65, 0x20, 0x77, 0x69, 0x66, 0x69, 0x20, 0x74, 0x61, 0x73, 0x6b
        .byte   0x3a, 0x20, 0x66, 0x61, 0x69, 0x6c, 0x65, 0x64, 0x20, 0x74, 0x6f, 0x20, 0x63, 0x72, 0x65, 0x61
        .byte   0x74, 0x65, 0x20, 0x74, 0x61, 0x73, 0x6b, 0x00

        .section .rodata_wlog_error.56,"a",@progbits
        .align  1
.Lsec_214_0:
        .byte   0x63, 0x72, 0x65, 0x61, 0x74, 0x65, 0x20, 0x77, 0x69, 0x66, 0x69, 0x20, 0x74, 0x61, 0x73, 0x6b
        .byte   0x3a, 0x20, 0x66, 0x61, 0x69, 0x6c, 0x65, 0x64, 0x20, 0x74, 0x6f, 0x20, 0x63, 0x72, 0x65, 0x61
        .byte   0x74, 0x65, 0x20, 0x73, 0x65, 0x6d, 0x00

        .section .rodata_wlog_error.55,"a",@progbits
        .align  1
.Lsec_215_0:
        .byte   0x63, 0x72, 0x65, 0x61, 0x74, 0x65, 0x20, 0x77, 0x69, 0x66, 0x69, 0x20, 0x74, 0x61, 0x73, 0x6b
        .byte   0x3a, 0x20, 0x66, 0x61, 0x69, 0x6c, 0x65, 0x64, 0x20, 0x74, 0x6f, 0x20, 0x63, 0x72, 0x65, 0x61
        .byte   0x74, 0x65, 0x20, 0x71, 0x75, 0x65, 0x75, 0x65, 0x00

        .section .rodata_wlog_error.54,"a",@progbits
        .align  1
.Lsec_216_0:
        .byte   0x63, 0x72, 0x65, 0x61, 0x74, 0x65, 0x20, 0x77, 0x69, 0x66, 0x69, 0x20, 0x74, 0x61, 0x73, 0x6b
        .byte   0x3a, 0x20, 0x66, 0x61, 0x69, 0x6c, 0x65, 0x64, 0x20, 0x74, 0x6f, 0x20, 0x63, 0x72, 0x65, 0x61
        .byte   0x74, 0x65, 0x20, 0x71, 0x75, 0x65, 0x75, 0x65, 0x00

        .section .rodata_wlog_warning.53,"a",@progbits
        .align  1
.Lsec_217_0:
        .byte   0x63, 0x72, 0x65, 0x61, 0x74, 0x65, 0x20, 0x77, 0x69, 0x66, 0x69, 0x20, 0x74, 0x61, 0x73, 0x6b
        .byte   0x3a, 0x20, 0x71, 0x75, 0x65, 0x75, 0x65, 0x20, 0x6e, 0x6f, 0x74, 0x20, 0x6e, 0x75, 0x6c, 0x6c
        .byte   0x00

        .section .rodata_wlog_warning.52,"a",@progbits
        .align  1
.Lsec_218_0:
        .byte   0x63, 0x72, 0x65, 0x61, 0x74, 0x65, 0x20, 0x77, 0x69, 0x66, 0x69, 0x20, 0x74, 0x61, 0x73, 0x6b
        .byte   0x3a, 0x20, 0x74, 0x61, 0x73, 0x6b, 0x20, 0x6e, 0x6f, 0x74, 0x20, 0x6e, 0x75, 0x6c, 0x6c, 0x00

        .section .rodata_wlog_error.48,"a",@progbits
        .align  1
.Lsec_219_0:
        .byte   0x25, 0x73, 0x20, 0x25, 0x75, 0x0a, 0x00

        .section .rodata_wlog_error.45,"a",@progbits
        .align  1
.Lsec_220_0:
        .byte   0x25, 0x73, 0x20, 0x25, 0x75, 0x0a, 0x00

        .section .rodata_wlog_error.42,"a",@progbits
        .align  1
.Lsec_221_0:
        .byte   0x25, 0x73, 0x20, 0x25, 0x75, 0x0a, 0x00

        .section .rodata_wlog_error.39,"a",@progbits
        .align  1
.Lsec_222_0:
        .byte   0x25, 0x73, 0x20, 0x25, 0x75, 0x0a, 0x00

        .section .rodata_wlog_warning.31,"a",@progbits
        .align  1
.Lsec_223_0:
        .byte   0x62, 0x61, 0x72, 0x20, 0x65, 0x62, 0x20, 0x66, 0x61, 0x69, 0x6c, 0x0a, 0x00

        .section .rodata_wlog_error.30,"a",@progbits
        .align  1
.Lsec_224_0:
        .byte   0x69, 0x6e, 0x76, 0x61, 0x6c, 0x69, 0x64, 0x20, 0x6d, 0x69, 0x63, 0x61, 0x68, 0x65, 0x6c, 0x20
        .byte   0x6d, 0x69, 0x63, 0x20, 0x66, 0x61, 0x69, 0x6c, 0x75, 0x72, 0x65, 0x20, 0x63, 0x62, 0x00

        .section .rodata_wlog_warning.27,"a",@progbits
        .align  1
.Lsec_225_0:
        .byte   0x72, 0x65, 0x6a, 0x65, 0x63, 0x74, 0x20, 0x70, 0x61, 0x69, 0x6e, 0x74, 0x65, 0x78, 0x74, 0x20
        .byte   0x66, 0x72, 0x61, 0x67, 0x6d, 0x65, 0x6e, 0x74, 0x0a, 0x00

        .section .rodata_wlog_info.25,"a",@progbits
        .align  1
.Lsec_226_0:
        .byte   0x63, 0x6c, 0x65, 0x61, 0x72, 0x20, 0x72, 0x78, 0x20, 0x66, 0x72, 0x61, 0x67, 0x6d, 0x65, 0x6e
        .byte   0x74, 0x00

        .section .bss.s_fragment,"aw",@nobits
        .align  8
.Lsec_227_0:
        .global s_fragment
        .type   s_fragment, @object
        .size   s_fragment, 16
s_fragment:
        .skip   16

        .section .rodata_wlog_info.21,"a",@progbits
        .align  1
.Lsec_228_0:
        .byte   0x49, 0x67, 0x6e, 0x6f, 0x72, 0x65, 0x20, 0x42, 0x41, 0x52, 0x20, 0x66, 0x72, 0x65, 0x65, 0x21
        .byte   0x00

        .section .rodata_wlog_error.17,"a",@progbits
        .align  1
.Lsec_229_0:
        .byte   0x25, 0x73, 0x20, 0x25, 0x75, 0x0a, 0x00

        .section .rodata_wlog_warning.15,"a",@progbits
        .align  1
.Lsec_230_0:
        .byte   0x6d, 0x61, 0x70, 0x20, 0x66, 0x61, 0x69, 0x6c, 0x0a, 0x00

        .section .rodata_wlog_error.14,"a",@progbits
        .align  1
.Lsec_231_0:
        .byte   0x25, 0x73, 0x20, 0x25, 0x75, 0x0a, 0x00

        .section .rodata_wlog_warning.13,"a",@progbits
        .align  1
.Lsec_232_0:
        .byte   0x6c, 0x6d, 0x61, 0x63, 0x20, 0x69, 0x66, 0x25, 0x64, 0x20, 0x73, 0x74, 0x6f, 0x70, 0x2c, 0x20
        .byte   0x64, 0x69, 0x73, 0x63, 0x61, 0x72, 0x64, 0x20, 0x70, 0x61, 0x63, 0x6b, 0x65, 0x74, 0x00

        .section .rodata_wlog_error.10,"a",@progbits
        .align  1
.Lsec_233_0:
        .byte   0x77, 0x69, 0x66, 0x69, 0x20, 0x61, 0x73, 0x73, 0x65, 0x72, 0x74, 0x3a, 0x20, 0x25, 0x64, 0x00

        .section .rodata_wlog_error.8,"a",@progbits
        .align  1
.Lsec_234_0:
        .byte   0x25, 0x73, 0x20, 0x25, 0x75, 0x0a, 0x00

        .section .data.CanDoFreqCal,"aw",@progbits
        .align  1
.Lsec_235_0:
        .global CanDoFreqCal
        .type   CanDoFreqCal, @object
        .size   CanDoFreqCal, 1
CanDoFreqCal:
        .byte   0x01

        .section .bss.DefFreqCalTimer,"aw",@nobits
        .align  4
.Lsec_236_0:
        .global DefFreqCalTimer
        .type   DefFreqCalTimer, @object
        .size   DefFreqCalTimer, 20
DefFreqCalTimer:
        .skip   20

        .section .bss.LowestFreqOffsetInOneChk,"aw",@nobits
        .align  2
.Lsec_237_0:
        .global LowestFreqOffsetInOneChk
        .type   LowestFreqOffsetInOneChk, @object
        .size   LowestFreqOffsetInOneChk, 2
LowestFreqOffsetInOneChk:
        .skip   2

        .section .bss.HighestFreqOffsetInOneChk,"aw",@nobits
        .align  2
.Lsec_238_0:
        .global HighestFreqOffsetInOneChk
        .type   HighestFreqOffsetInOneChk, @object
        .size   HighestFreqOffsetInOneChk, 2
HighestFreqOffsetInOneChk:
        .skip   2

        .section .data.NoiseTimerInterval,"aw",@progbits
        .align  2
.Lsec_239_0:
        .global NoiseTimerInterval
        .type   NoiseTimerInterval, @object
        .size   NoiseTimerInterval, 2
NoiseTimerInterval:
        .byte   0x64, 0x00

        .section .bss.pend_flag_periodic_cal,"aw",@nobits
        .align  1
.Lsec_240_0:
        .global pend_flag_periodic_cal
        .type   pend_flag_periodic_cal, @object
        .size   pend_flag_periodic_cal, 1
pend_flag_periodic_cal:
        .skip   1

        .section .bss.CurSigIdxToBeUse,"aw",@nobits
        .align  2
.Lsec_241_0:
        .global CurSigIdxToBeUse
        .type   CurSigIdxToBeUse, @object
        .size   CurSigIdxToBeUse, 2
CurSigIdxToBeUse:
        .skip   2

        .section .bss.CurFreeSigIdx,"aw",@nobits
        .align  2
.Lsec_242_0:
        .global CurFreeSigIdx
        .type   CurFreeSigIdx, @object
        .size   CurFreeSigIdx, 2
CurFreeSigIdx:
        .skip   2

        .section .bss.SigInMacISR,"aw",@nobits
        .align  4
.Lsec_243_0:
        .global SigInMacISR
        .type   SigInMacISR, @object
        .size   SigInMacISR, 400
SigInMacISR:
        .skip   400

        .section .dram1.4,"aw",@progbits
        .align  4
.Lsec_244_0:
        .global xphyQueue
        .type   xphyQueue, @object
        .size   xphyQueue, 4
xphyQueue:
        .byte   0x00, 0x00, 0x00, 0x00

        .section .bss.pp_allow_cut_sevt,"aw",@nobits
        .align  1
.Lsec_245_0:
        .global pp_allow_cut_sevt
        .type   pp_allow_cut_sevt, @object
        .size   pp_allow_cut_sevt, 1
pp_allow_cut_sevt:
        .skip   1

        .section .bss.pp_need_cut_rx,"aw",@nobits
        .align  1
.Lsec_246_0:
        .global pp_need_cut_rx
        .type   pp_need_cut_rx, @object
        .size   pp_need_cut_rx, 1
pp_need_cut_rx:
        .skip   1

        .section .dram1.3,"aw",@progbits
        .align  4
.Lsec_247_0:
        .global bars
        .type   bars, @object
        .size   bars, 144
bars:
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00

        .section .data.pTxRx,"aw",@progbits
        .align  4
.Lsec_248_0:
        .global pTxRx
        .type   pTxRx, @object
        .size   pTxRx, 4
pTxRx:
        .word   TxRxCxt

        .section .data.TxRxCxt,"aw",@progbits
        .align  4
.Lsec_249_0:
        .global TxRxCxt
        .type   TxRxCxt, @object
        .size   TxRxCxt, 972
TxRxCxt:
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x40, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00

        .section .bss.s_wifi_queue,"aw",@nobits
        .align  4
.Lsec_250_0:
        .global s_wifi_queue
        .type   s_wifi_queue, @object
        .size   s_wifi_queue, 4
s_wifi_queue:
        .skip   4

        .section .bss.s_michael_mic_failure_cb,"aw",@nobits
        .align  4
.Lsec_251_0:
        .skip   4

        .section .bss.s_pp_task_create_sem,"aw",@nobits
        .align  4
.Lsec_252_0:
        .skip   4

        .section .bss.s_pp_task_del_sem,"aw",@nobits
        .align  4
.Lsec_253_0:
        .skip   4

        .section .bss.pp_task_hdl,"aw",@nobits
        .align  4
.Lsec_254_0:
        .skip   4

        .section .bss.g_config_func,"aw",@nobits
        .align  4
.Lsec_255_0:
        .skip   4

        .section .bss.g_timer_func,"aw",@nobits
        .align  4
.Lsec_256_0:
        .skip   4

        .section .bss.g_net80211_tx_func,"aw",@nobits
        .align  4
.Lsec_257_0:
        .skip   4

        .section .bss.g_tx_done_cb_func,"aw",@nobits
        .align  4
.Lsec_258_0:
        .skip   4

        .section .dram1.2,"aw",@progbits
        .align  1
.Lsec_259_0:
        .global pp_sig_cnt
        .type   pp_sig_cnt, @object
        .size   pp_sig_cnt, 30
pp_sig_cnt:
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00

        .section .rodata.str1.1,"aMS",@progbits,1
        .align  1
.Lsec_260_0:
        .ascii  "4df78f2\000"

        .section .data.libpp_reversion_git,"aw",@progbits
        .align  4
.Lsec_261_0:
        .global libpp_reversion_git
        .type   libpp_reversion_git, @object
        .size   libpp_reversion_git, 4
libpp_reversion_git:
        .word   .Lsec_260_0
