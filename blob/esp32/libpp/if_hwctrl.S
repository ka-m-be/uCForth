.section .text.bb_intr_handl,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_105_0, 0x3ff5d04c
.Lsec_105_4:
        .global bb_intr_handl
        .type   bb_intr_handl, @function
bb_intr_handl:
        entry    a1, 32
        l32r     a8, .Lsec_105_0
        memw
        l32i.n   a8, a8, 0
        retw.n
        .size   bb_intr_handl, .-bb_intr_handl
.Lsec_105_11:

        .section .text.ic_get_addr,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_106_0, if_ctrl
.Lsec_106_4:
        .global ic_get_addr
        .type   ic_get_addr, @function
ic_get_addr:
        entry    a1, 32
        l32r     a8, .Lsec_106_0
        extui    a2, a2, 0, 8
        addx4    a2, a2, a2
        addx2    a2, a2, a8
        retw.n
        .size   ic_get_addr, .-ic_get_addr
.Lsec_106_15:

        .section .wifislprxiram.2,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_107_0, wDevCtrl
.Lsec_107_4:
        .global ic_interface_enabled
        .type   ic_interface_enabled, @function
ic_interface_enabled:
        entry    a1, 32
        l32r     a8, .Lsec_107_0
        l8ui     a8, a8, 49
        ssr      a2
        srl      a2, a8
        extui    a2, a2, 0, 1
        retw.n
        .size   ic_interface_enabled, .-ic_interface_enabled
.Lsec_107_18:

        .section .text.ic_is_pure_sta,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_108_0, if_ctrl
.Lsec_108_4:
        .global ic_is_pure_sta
        .type   ic_is_pure_sta, @function
ic_is_pure_sta:
        entry    a1, 32
        l32r     a8, .Lsec_108_0
        extui    a2, a2, 0, 8
        addx4    a2, a2, a2
        addx2    a8, a2, a8
        movi     a2, 0
        l8ui     a9, a8, 6
        beqi     a9, 1, .Lsec_108_28
        l8ui     a2, a8, 7
        movi     a8, 1
        xor      a2, a2, a8
        extui    a2, a2, 0, 8
.Lsec_108_28:
        extui    a2, a2, 0, 1
        retw.n
        .size   ic_is_pure_sta, .-ic_is_pure_sta
.Lsec_108_2d:

        .section .text.ic_get_ptk_alg,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_109_0, if_ctrl
.Lsec_109_4:
        .global ic_get_ptk_alg
        .type   ic_get_ptk_alg, @function
ic_get_ptk_alg:
        entry    a1, 32
        l32r     a8, .Lsec_109_0
        extui    a2, a2, 0, 8
        addx4    a2, a2, a2
        addx2    a2, a2, a8
        l8ui     a2, a2, 8
        retw.n
        .size   ic_get_ptk_alg, .-ic_get_ptk_alg
.Lsec_109_18:

        .section .text.ic_disable_crypto,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_110_0, if_ctrl
        .literal .Lsec_110_4, wDev_Crypto_Disable
.Lsec_110_8:
        .global ic_disable_crypto
        .type   ic_disable_crypto, @function
ic_disable_crypto:
        entry    a1, 32
        extui    a3, a3, 0, 8
        extui    a10, a2, 0, 8
        bnez.n   a3, .Lsec_110_2e
        l32r     a9, .Lsec_110_0
        addx4    a8, a10, a10
        addx2    a8, a8, a9
        s8i      a3, a8, 8
        s8i      a3, a8, 9
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wDev_Crypto_Disable
        l32r     a8, .Lsec_110_4
        callx8   a8
.Lsec_110_2e:
        retw.n
        .size   ic_disable_crypto, .-ic_disable_crypto
.Lsec_110_30:

        .section .text.ic_del_ptk_and_gtk_alg,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_111_0, if_ctrl
.Lsec_111_4:
        .global ic_del_ptk_and_gtk_alg
        .type   ic_del_ptk_and_gtk_alg, @function
ic_del_ptk_and_gtk_alg:
        entry    a1, 32
        l32r     a8, .Lsec_111_0
        extui    a2, a2, 0, 8
        addx4    a2, a2, a2
        addx2    a2, a2, a8
        movi.n   a8, 0
        s8i      a8, a2, 8
        s8i      a8, a2, 9
        memw
        memw
        retw.n
        .size   ic_del_ptk_and_gtk_alg, .-ic_del_ptk_and_gtk_alg
.Lsec_111_23:

        .section .text.ic_set_key,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_112_0, if_ctrl
        .literal .Lsec_112_4, g_osi_funcs_p
        .literal .Lsec_112_8, wDev_Insert_KeyEntry
.Lsec_112_c:
        .global ic_set_key
        .type   ic_set_key, @function
ic_set_key:
        entry    a1, 64
        extui    a2, a2, 0, 8
        extui    a12, a4, 0, 8
        slli     a8, a2, 2
        l32r     a4, .Lsec_112_0
        add.n    a8, a8, a2
        extui    a6, a6, 0, 8
        extui    a3, a3, 0, 8
        l8ui     a9, a1, 64
        addx2    a8, a8, a4
        l8ui     a10, a1, 68
        l8ui     a11, a1, 72
        bltui    a6, 4, .Lsec_112_40
        s8i      a3, a8, 8
        memw
        memw
        j        .Lsec_112_4b
        .byte    0x00
        .byte    0x00
.Lsec_112_40:
        bnez.n   a3, .Lsec_112_45
        s8i      a3, a8, 8
.Lsec_112_45:
        s8i      a3, a8, 9
        memw
.Lsec_112_4b:
        l32r     a4, .Lsec_112_4
        l32i.n   a8, a4, 0
        l32i     a8, a8, 216
        s32i.n   a9, a1, 20
        s32i.n   a10, a1, 24
        s32i.n   a11, a1, 28
        s32i.n   a12, a1, 16
        memw
        callx8   a8
        mov.n    a15, a7
        mov.n    a14, a6
        mov.n    a13, a5
        l32i.n   a11, a1, 28
        l32i.n   a10, a1, 24
        l32i.n   a9, a1, 20
        l32i.n   a12, a1, 16
        s32i.n   a11, a1, 8
        mov.n    a11, a2
        s32i.n   a10, a1, 4
        mov.n    a10, a3
        s32i.n   a9, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wDev_Insert_KeyEntry
        l32r     a8, .Lsec_112_8
        callx8   a8
        l32i.n   a2, a4, 0
        l32i     a2, a2, 220
        callx8   a2
        retw.n
        .size   ic_set_key, .-ic_set_key
.Lsec_112_8c:

        .section .text.ic_get_key,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_113_0, memset
        .literal .Lsec_113_4, hal_crypto_get_key_entry
        .literal .Lsec_113_8, memcpy
.Lsec_113_c:
        .global ic_get_key
        .type   ic_get_key, @function
ic_get_key:
        entry    a1, 64
        l8ui     a8, a1, 64
        movi.n   a12, 9
        movi.n   a11, 0
        mov.n    a10, a1
        extui    a6, a6, 0, 8
        s32i.n   a8, a1, 16
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, memset
        l32r     a8, .Lsec_113_0
        callx8   a8
        mov.n    a13, a1
        mov.n    a11, a7
        mov.n    a10, a6
        l32i.n   a8, a1, 16
        mov.n    a12, a8
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_crypto_get_key_entry
        l32r     a8, .Lsec_113_4
        callx8   a8
        movi.n   a12, 6
        l8ui     a8, a1, 0
        addi.n   a11, a1, 3
        mov.n    a10, a5
        s8i      a8, a2, 0
        memw
        movi.n   a2, 0
        l8ui     a8, a1, 1
        s32i.n   a8, a3, 0
        l8ui     a8, a1, 2
        s32i.n   a8, a4, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_113_8
        callx8   a8
        retw.n
        .size   ic_get_key, .-ic_get_key
.Lsec_113_5c:

        .section .text.ic_obtain_key,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_114_0, hal_crypto_is_key_valid
        .literal .Lsec_114_4, ic_get_key
        .literal .Lsec_114_8, memcmp
.Lsec_114_c:
        .global ic_obtain_key
        .type   ic_obtain_key, @function
ic_obtain_key:
        entry    a1, 96
        movi.n   a8, 3
        extui    a3, a3, 0, 8
        extui    a2, a2, 0, 8
        s8i      a8, a1, 32
        movi.n   a9, 3
        movi.n   a8, 0
        extui    a4, a4, 0, 8
        s32i.n   a3, a1, 52
        movi.n   a3, 4
        s32i.n   a2, a1, 48
        s32i.n   a8, a1, 28
        s32i     a9, a1, 24
        s32i     a8, a1, 16
        s16i     a8, a1, 20
        memw
.Lsec_114_35:
        mov.n    a10, a3
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_crypto_is_key_valid
        l32r     a8, .Lsec_114_0
        callx8   a8
        mov.n    a2, a10
        bnez.n   a10, .Lsec_114_50
.Lsec_114_44:
        addi.n   a3, a3, 1
        movi.n   a9, 25
        bne      a3, a9, .Lsec_114_35
        movi.n   a2, 0
        j        .Lsec_114_94
.Lsec_114_50:
        extui    a10, a7, 0, 8
        s32i     a10, a1, 0
        mov.n    a15, a6
        or       a14, a3, a3
        addi     a13, a1, 16
        addi     a12, a1, 24
        addi     a11, a1, 28
        addi     a10, a1, 32
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, ic_get_key
        l32r     a8, .Lsec_114_4
        callx8   a8
        l8ui     a10, a1, 32
        l32i.n   a8, a1, 48
        bne      a10, a8, .Lsec_114_44
        l32i.n   a10, a1, 28
        l32i.n   a8, a1, 52
        bne      a8, a10, .Lsec_114_44
        l32i.n   a10, a1, 24
        bne      a4, a10, .Lsec_114_44
        movi.n   a12, 6
        mov.n    a11, a5
        addi     a10, a1, 16
        .reloc  ., R_XTENSA_ASM_EXPAND, memcmp
        l32r     a8, .Lsec_114_8
        callx8   a8
        bnez     a10, .Lsec_114_44
.Lsec_114_94:
        retw.n
        .size   ic_obtain_key, .-ic_obtain_key
.Lsec_114_96:

        .section .text.ic_get_rssi,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_115_0, wDevCtrl
        .literal .Lsec_115_4, rc_get_trc
.Lsec_115_8:
        .global ic_get_rssi
        .type   ic_get_rssi, @function
ic_get_rssi:
        entry    a1, 32
        extui    a10, a2, 0, 8
        mov.n    a11, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, rc_get_trc
        l32r     a8, .Lsec_115_4
        callx8   a8
        movi.n   a2, 31
        beqz.n   a10, .Lsec_115_29
        l32r     a8, .Lsec_115_0
        l8ui     a2, a10, 3
        l8ui     a8, a8, 46
        sub      a2, a2, a8
        extui    a2, a2, 0, 8
.Lsec_115_29:
        retw.n
        .size   ic_get_rssi, .-ic_get_rssi
.Lsec_115_2b:

        .section .text.ic_tx_pkt,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_116_0, ppTxPkt
.Lsec_116_4:
        .global ic_tx_pkt
        .type   ic_tx_pkt, @function
ic_tx_pkt:
        entry    a1, 32
        movi.n   a11, 1
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, ppTxPkt
        l32r     a8, .Lsec_116_0
        callx8   a8
        mov.n    a2, a10
        retw.n
        .size   ic_tx_pkt, .-ic_tx_pkt
.Lsec_116_15:

        .section .text.ic_ebuf_alloc,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_117_0, esf_buf_alloc
.Lsec_117_4:
        .global ic_ebuf_alloc
        .type   ic_ebuf_alloc, @function
ic_ebuf_alloc:
        entry    a1, 32
        mov.n    a12, a4
        mov.n    a11, a3
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esf_buf_alloc
        l32r     a8, .Lsec_117_0
        callx8   a8
        mov.n    a2, a10
        retw.n
        .size   ic_ebuf_alloc, .-ic_ebuf_alloc
.Lsec_117_18:

        .section .text.ic_ebuf_recycle_tx,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_118_0, esf_buf_recycle
.Lsec_118_4:
        .global ic_ebuf_recycle_tx
        .type   ic_ebuf_recycle_tx, @function
ic_ebuf_recycle_tx:
        entry    a1, 32
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esf_buf_recycle
        l32r     a8, .Lsec_118_0
        callx8   a8
        retw.n
        .size   ic_ebuf_recycle_tx, .-ic_ebuf_recycle_tx
.Lsec_118_12:

        .section .text.ic_ebuf_recycle_rx,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_119_0, ppRecycleRxPkt
.Lsec_119_4:
        .global ic_ebuf_recycle_rx
        .type   ic_ebuf_recycle_rx, @function
ic_ebuf_recycle_rx:
        entry    a1, 32
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, ppRecycleRxPkt
        l32r     a8, .Lsec_119_0
        callx8   a8
        retw.n
        .size   ic_ebuf_recycle_rx, .-ic_ebuf_recycle_rx
.Lsec_119_12:

        .section .text.esp_wifi_internal_free_rx_buffer,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_120_0, ppRecycleRxPkt
.Lsec_120_4:
        .global esp_wifi_internal_free_rx_buffer
        .type   esp_wifi_internal_free_rx_buffer, @function
esp_wifi_internal_free_rx_buffer:
        entry    a1, 32
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, ppRecycleRxPkt
        l32r     a8, .Lsec_120_0
        callx8   a8
        retw.n
        .size   esp_wifi_internal_free_rx_buffer, .-esp_wifi_internal_free_rx_buffer
.Lsec_120_12:

        .section .text.ic_register_tx_cb,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_121_0, ppRegisterTxCallback
.Lsec_121_4:
        .global ic_register_tx_cb
        .type   ic_register_tx_cb, @function
ic_register_tx_cb:
        entry    a1, 32
        extui    a12, a4, 0, 8
        extui    a11, a3, 0, 8
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, ppRegisterTxCallback
        l32r     a8, .Lsec_121_0
        callx8   a8
        mov.n    a2, a10
        retw.n
        .size   ic_register_tx_cb, .-ic_register_tx_cb
.Lsec_121_19:

        .section .text.ic_register_rx_cb,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_122_0, ppRegisterRxCallback
.Lsec_122_4:
        .global ic_register_rx_cb
        .type   ic_register_rx_cb, @function
ic_register_rx_cb:
        entry    a1, 32
        extui    a11, a3, 0, 8
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, ppRegisterRxCallback
        l32r     a8, .Lsec_122_0
        callx8   a8
        mov.n    a2, a10
        retw.n
        .size   ic_register_rx_cb, .-ic_register_rx_cb
.Lsec_122_16:

        .section .text.ic_register_timer_post_cb,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_123_0, pp_timer_register_post_cb
.Lsec_123_4:
        .global ic_register_timer_post_cb
        .type   ic_register_timer_post_cb, @function
ic_register_timer_post_cb:
        entry    a1, 32
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_timer_register_post_cb
        l32r     a8, .Lsec_123_0
        callx8   a8
        mov.n    a2, a10
        retw.n
        .size   ic_register_timer_post_cb, .-ic_register_timer_post_cb
.Lsec_123_14:

        .section .text.ic_register_michael_mic_failure_cb,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_124_0, pp_register_michael_mic_failure_cb
.Lsec_124_4:
        .global ic_register_michael_mic_failure_cb
        .type   ic_register_michael_mic_failure_cb, @function
ic_register_michael_mic_failure_cb:
        entry    a1, 32
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_register_michael_mic_failure_cb
        l32r     a8, .Lsec_124_0
        callx8   a8
        mov.n    a2, a10
        retw.n
        .size   ic_register_michael_mic_failure_cb, .-ic_register_michael_mic_failure_cb
.Lsec_124_14:

        .section .text.ic_register_pp_tx_done_cb,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_125_0, ppRegisterTxDoneUserActionCallback
.Lsec_125_4:
        .global ic_register_pp_tx_done_cb
        .type   ic_register_pp_tx_done_cb, @function
ic_register_pp_tx_done_cb:
        entry    a1, 32
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, ppRegisterTxDoneUserActionCallback
        l32r     a8, .Lsec_125_0
        callx8   a8
        mov.n    a2, a10
        retw.n
        .size   ic_register_pp_tx_done_cb, .-ic_register_pp_tx_done_cb
.Lsec_125_14:

        .section .text.ic_set_promis_filter,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_126_0, wdev_set_promis_filter
.Lsec_126_4:
        .global ic_set_promis_filter
        .type   ic_set_promis_filter, @function
ic_set_promis_filter:
        entry    a1, 32
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_set_promis_filter
        l32r     a8, .Lsec_126_0
        callx8   a8
        mov.n    a2, a10
        retw.n
        .size   ic_set_promis_filter, .-ic_set_promis_filter
.Lsec_126_14:

        .section .text.ic_get_promis_filter,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_127_0, wdev_get_promis_filter
.Lsec_127_4:
        .global ic_get_promis_filter
        .type   ic_get_promis_filter, @function
ic_get_promis_filter:
        entry    a1, 32
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_get_promis_filter
        l32r     a8, .Lsec_127_0
        callx8   a8
        mov.n    a2, a10
        retw.n
        .size   ic_get_promis_filter, .-ic_get_promis_filter
.Lsec_127_14:

        .section .text.ic_set_promis_ctrl_filter,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_128_0, wdev_set_promis_ctrl_filter
.Lsec_128_4:
        .global ic_set_promis_ctrl_filter
        .type   ic_set_promis_ctrl_filter, @function
ic_set_promis_ctrl_filter:
        entry    a1, 32
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_set_promis_ctrl_filter
        l32r     a8, .Lsec_128_0
        callx8   a8
        mov.n    a2, a10
        retw.n
        .size   ic_set_promis_ctrl_filter, .-ic_set_promis_ctrl_filter
.Lsec_128_14:

        .section .text.ic_get_promis_ctrl_filter,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_129_0, wdev_get_promis_ctrl_filter
.Lsec_129_4:
        .global ic_get_promis_ctrl_filter
        .type   ic_get_promis_ctrl_filter, @function
ic_get_promis_ctrl_filter:
        entry    a1, 32
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_get_promis_ctrl_filter
        l32r     a8, .Lsec_129_0
        callx8   a8
        mov.n    a2, a10
        retw.n
        .size   ic_get_promis_ctrl_filter, .-ic_get_promis_ctrl_filter
.Lsec_129_14:

        .section .text.ic_register_promis_rx_cb,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_130_0, ppRegisterPromisRxCallback
.Lsec_130_4:
        .global ic_register_promis_rx_cb
        .type   ic_register_promis_rx_cb, @function
ic_register_promis_rx_cb:
        entry    a1, 32
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, ppRegisterPromisRxCallback
        l32r     a8, .Lsec_130_0
        callx8   a8
        movi.n   a8, 0
        movi.n   a2, 1
        moveqz   a2, a8, a10
        retw.n
        .size   ic_register_promis_rx_cb, .-ic_register_promis_rx_cb
.Lsec_130_19:

        .section .text.ic_register_csi_rx_cb,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_131_0, wdev_set_csi_rx_cb
.Lsec_131_4:
        .global ic_register_csi_rx_cb
        .type   ic_register_csi_rx_cb, @function
ic_register_csi_rx_cb:
        entry    a1, 32
        mov.n    a11, a3
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_set_csi_rx_cb
        l32r     a8, .Lsec_131_0
        callx8   a8
        mov.n    a2, a10
        retw.n
        .size   ic_register_csi_rx_cb, .-ic_register_csi_rx_cb
.Lsec_131_15:

        .section .text.ic_register_config_cb,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_132_0, pp_register_config_cb
.Lsec_132_4:
        .global ic_register_config_cb
        .type   ic_register_config_cb, @function
ic_register_config_cb:
        entry    a1, 32
        mov.n    a10, a2
        movi.n   a2, 0
        beq      a10, a2, .Lsec_132_16
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_register_config_cb
        l32r     a8, .Lsec_132_0
        callx8   a8
        movi.n   a2, 1
.Lsec_132_16:
        retw.n
        .size   ic_register_config_cb, .-ic_register_config_cb
.Lsec_132_18:

        .section .text.ic_is_mgmt_hwdecr_enabled,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_133_0, wdev_is_mgmt_hwdecr_enabled
.Lsec_133_4:
        .global ic_is_mgmt_hwdecr_enabled
        .type   ic_is_mgmt_hwdecr_enabled, @function
ic_is_mgmt_hwdecr_enabled:
        entry    a1, 32
        extui    a10, a2, 0, 8
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_is_mgmt_hwdecr_enabled
        l32r     a8, .Lsec_133_0
        callx8   a8
        mov.n    a2, a10
        retw.n
        .size   ic_is_mgmt_hwdecr_enabled, .-ic_is_mgmt_hwdecr_enabled
.Lsec_133_14:

        .section .text.ic_register_pm_tx_null_cb,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_134_0, pm_register_pm_tx_null_cb
.Lsec_134_4:
        .global ic_register_pm_tx_null_cb
        .type   ic_register_pm_tx_null_cb, @function
ic_register_pm_tx_null_cb:
        entry    a1, 32
        mov.n    a10, a2
        movi.n   a2, 0
        beq      a10, a2, .Lsec_134_16
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_register_pm_tx_null_cb
        l32r     a8, .Lsec_134_0
        callx8   a8
        movi.n   a2, 1
.Lsec_134_16:
        retw.n
        .size   ic_register_pm_tx_null_cb, .-ic_register_pm_tx_null_cb
.Lsec_134_18:

        .section .text.ic_register_net80211_tx_cb,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_135_0, pp_register_net80211_tx_cb
.Lsec_135_4:
        .global ic_register_net80211_tx_cb
        .type   ic_register_net80211_tx_cb, @function
ic_register_net80211_tx_cb:
        entry    a1, 32
        mov.n    a10, a2
        movi.n   a2, 0
        beq      a10, a2, .Lsec_135_16
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_register_net80211_tx_cb
        l32r     a8, .Lsec_135_0
        callx8   a8
        mov.n    a2, a10
.Lsec_135_16:
        retw.n
        .size   ic_register_net80211_tx_cb, .-ic_register_net80211_tx_cb
.Lsec_135_18:

        .section .text.ic_register_timer_cb,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_136_0, pp_register_timer_cb
.Lsec_136_4:
        .global ic_register_timer_cb
        .type   ic_register_timer_cb, @function
ic_register_timer_cb:
        entry    a1, 32
        mov.n    a10, a2
        movi.n   a2, 0
        beq      a10, a2, .Lsec_136_16
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_register_timer_cb
        l32r     a8, .Lsec_136_0
        callx8   a8
        mov.n    a2, a10
.Lsec_136_16:
        retw.n
        .size   ic_register_timer_cb, .-ic_register_timer_cb
.Lsec_136_18:

        .section .text.ic_register_ftm_callbacks,"ax",@progbits
        .align  4
.Lsec_137_0:
        .global ic_register_ftm_callbacks
        .type   ic_register_ftm_callbacks, @function
ic_register_ftm_callbacks:
        entry    a1, 32
        movi.n   a2, 0
        retw.n
        .size   ic_register_ftm_callbacks, .-ic_register_ftm_callbacks
.Lsec_137_7:

        .section .text.ic_register_nan_callbacks,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_138_0, wdev_nan_set_callbacks
.Lsec_138_4:
        .global ic_register_nan_callbacks
        .type   ic_register_nan_callbacks, @function
ic_register_nan_callbacks:
        entry    a1, 32
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_nan_set_callbacks
        l32r     a8, .Lsec_138_0
        callx8   a8
        mov.n    a2, a10
        retw.n
        .size   ic_register_nan_callbacks, .-ic_register_nan_callbacks
.Lsec_138_14:

        .section .text.ic_pp_post,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_139_0, pp_post
.Lsec_139_4:
        .global ic_pp_post
        .type   ic_pp_post, @function
ic_pp_post:
        entry    a1, 32
        mov.n    a11, a3
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_post
        l32r     a8, .Lsec_139_0
        callx8   a8
        mov.n    a2, a10
        retw.n
        .size   ic_pp_post, .-ic_pp_post
.Lsec_139_15:

        .section .text.ic_enable_sniffer,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_140_0, wdev_set_promis
.Lsec_140_4:
        .global ic_enable_sniffer
        .type   ic_enable_sniffer, @function
ic_enable_sniffer:
        entry    a1, 32
        movi     a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_set_promis
        l32r     a8, .Lsec_140_0
        callx8   a8
        retw.n
        .size   ic_enable_sniffer, .-ic_enable_sniffer
.Lsec_140_12:

        .section .text.ic_disable_sniffer,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_141_0, wdev_set_promis
.Lsec_141_4:
        .global ic_disable_sniffer
        .type   ic_disable_sniffer, @function
ic_disable_sniffer:
        entry    a1, 32
        movi     a10, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_set_promis
        l32r     a8, .Lsec_141_0
        callx8   a8
        retw.n
        .size   ic_disable_sniffer, .-ic_disable_sniffer
.Lsec_141_12:

        .section .text.ic_set_vif,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_142_0, if_ctrl
        .literal .Lsec_142_4, .Lsec_217_0
        .literal .Lsec_142_8, .Lsec_216_0
        .literal .Lsec_142_c, .Lsec_215_0
        .literal .Lsec_142_10, wDevCtrl
        .literal .Lsec_142_14, 0x3ff73084
        .literal .Lsec_142_18, 0x7fffffff
        .literal .Lsec_142_1c, .Lsec_214_0
        .literal .Lsec_142_20, .Lsec_213_0
        .literal .Lsec_142_24, .Lsec_212_0
        .literal .Lsec_142_28, .Lsec_211_0
        .literal .Lsec_142_2c, wifi_log
        .literal .Lsec_142_30, wifi_log
        .literal .Lsec_142_34, wifi_set_rx_policy
        .literal .Lsec_142_38, wifi_log
        .literal .Lsec_142_3c, ic_enable_sniffer
        .literal .Lsec_142_40, memcpy
        .literal .Lsec_142_44, ic_disable_crypto
        .literal .Lsec_142_48, rc_disable_trc_by_interface
        .literal .Lsec_142_4c, pm_force_scan_unlock
        .literal .Lsec_142_50, wifi_log
        .literal .Lsec_142_54, wifi_log
        .literal .Lsec_142_58, wifi_log
        .literal .Lsec_142_5c, wifi_set_rx_policy
        .literal .Lsec_142_60, ic_disable_sniffer
        .literal .Lsec_142_64, wifi_log
.Lsec_142_68:
        .global ic_set_vif
        .type   ic_set_vif, @function
ic_set_vif:
        entry    a1, 32
        extui    a2, a2, 0, 8
        extui    a6, a6, 0, 8
        extui    a7, a7, 0, 8
        bnei     a3, 2, .Lsec_142_90
        l32r     a3, .Lsec_142_0
        addx4    a2, a2, a2
        addx2    a2, a2, a3
        s8i      a5, a2, 6
        s8i      a6, a2, 7
        memw
        memw
        j        .Lsec_142_211
        .byte    0x00
.Lsec_142_90:
        bnez     a3, .Lsec_142_153
        bnez.n   a2, .Lsec_142_ac
        movi.n   a14, 1
        l32r     a13, .Lsec_142_4
        mov.n    a10, a14
        movi.n   a12, 4
        movi.n   a11, 12
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_142_2c
        callx8   a8
        movi.n   a10, 1
        j        .Lsec_142_c2
        .byte    0x00
.Lsec_142_ac:
        bnei     a2, 2, .Lsec_142_cc
        l32r     a13, .Lsec_142_8
        movi.n   a10, 1
        movi.n   a14, 11
        movi.n   a12, 4
        movi.n   a11, 12
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_142_30
        callx8   a8
        movi.n   a10, 11
.Lsec_142_c2:
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_set_rx_policy
        l32r     a8, .Lsec_142_34
        callx8   a8
        j        .Lsec_142_ee
        .byte    0x00
.Lsec_142_cc:
        bnei     a2, 1, .Lsec_142_e5
        l32r     a13, .Lsec_142_c
        mov.n    a10, a2
        movi.n   a14, 9
        movi.n   a12, 4
        movi.n   a11, 12
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_142_38
        callx8   a8
        movi.n   a10, 9
        j        .Lsec_142_c2
.Lsec_142_e5:
        bnei     a2, 3, .Lsec_142_ee
        .reloc  ., R_XTENSA_ASM_EXPAND, ic_enable_sniffer
        l32r     a8, .Lsec_142_3c
        callx8   a8
.Lsec_142_ee:
        l32r     a3, .Lsec_142_0
        or       a11, a4, a4
        addx4    a10, a2, a2
        addx4    a4, a2, a2
        addx2    a10, a10, a3
        movi.n   a12, 6
        addx2    a3, a4, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_142_40
        callx8   a8
        s8i      a5, a3, 6
        movi.n   a8, 1
        ssl      a2
        sll      a8, a8
        s8i      a6, a3, 7
        memw
        l32r     a3, .Lsec_142_10
        l8ui     a4, a3, 49
        or       a8, a8, a4
        extui    a8, a8, 0, 8
        s8i      a8, a3, 49
        memw
        beqz.n   a8, .Lsec_142_142
        movi.n   a4, -1
        slli     a4, a4, 31
        l32r     a5, .Lsec_142_14
        memw
        l32i.n   a3, a5, 0
        or       a3, a3, a4
        memw
        s32i.n   a3, a5, 0
.Lsec_142_142:
        mov.n    a11, a7
        or       a10, a2, a2
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, ic_disable_crypto
        l32r     a8, .Lsec_142_44
        callx8   a8
        j        .Lsec_142_211
.Lsec_142_153:
        beqi     a3, 1, .Lsec_142_159
        j        .Lsec_142_200
.Lsec_142_159:
        l32r     a5, .Lsec_142_10
        l8ui     a4, a5, 49
        bbc      a4, a2, .Lsec_142_181
        ssl      a2
        sll      a3, a3
        movi.n   a6, -1
        xor      a3, a6, a3
        and      a4, a4, a3
        mov.n    a10, a2
        s8i      a4, a5, 49
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, rc_disable_trc_by_interface
        l32r     a8, .Lsec_142_48
        callx8   a8
.Lsec_142_181:
        l8ui     a3, a5, 49
        bnez.n   a3, .Lsec_142_1a2
        l32r     a4, .Lsec_142_14
        l32r     a5, .Lsec_142_18
        memw
        l32i.n   a3, a4, 0
        and      a3, a3, a5
        memw
        s32i.n   a3, a4, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_force_scan_unlock
        l32r     a8, .Lsec_142_4c
        callx8   a8
.Lsec_142_1a2:
        bnez.n   a2, .Lsec_142_1bc
        movi.n   a10, 1
        movi.n   a14, 2
        l32r     a13, .Lsec_142_1c
        movi.n   a12, 4
        movi.n   a11, 12
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_142_50
        callx8   a8
        movi.n   a10, 2
        j        .Lsec_142_1eb
        .byte    0x00
        .byte    0x00
.Lsec_142_1bc:
        bnei     a2, 2, .Lsec_142_1d5
        l32r     a13, .Lsec_142_20
        movi.n   a10, 1
        movi.n   a14, 13
        movi.n   a12, 4
        movi.n   a11, 12
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_142_54
        callx8   a8
        movi.n   a10, 13
        j        .Lsec_142_1eb
.Lsec_142_1d5:
        bnei     a2, 1, .Lsec_142_1f4
        l32r     a13, .Lsec_142_24
        mov.n    a10, a2
        movi.n   a14, 10
        movi.n   a12, 4
        movi.n   a11, 12
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_142_58
        callx8   a8
        movi.n   a10, 10
.Lsec_142_1eb:
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_set_rx_policy
        l32r     a8, .Lsec_142_5c
        callx8   a8
        j        .Lsec_142_211
.Lsec_142_1f4:
        bnei     a2, 3, .Lsec_142_211
        .reloc  ., R_XTENSA_ASM_EXPAND, ic_disable_sniffer
        l32r     a8, .Lsec_142_60
        callx8   a8
        j        .Lsec_142_211
.Lsec_142_200:
        movi.n   a12, 1
        l32r     a13, .Lsec_142_28
        mov.n    a14, a2
        mov.n    a11, a12
        movi.n   a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_142_64
        callx8   a8
.Lsec_142_211:
        retw.n
        .size   ic_set_vif, .-ic_set_vif
.Lsec_142_213:

        .section .text.ic_get_next_tbtt,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_143_0, wDev_Get_Next_TBTT
.Lsec_143_4:
        .global ic_get_next_tbtt
        .type   ic_get_next_tbtt, @function
ic_get_next_tbtt:
        entry    a1, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, wDev_Get_Next_TBTT
        l32r     a8, .Lsec_143_0
        callx8   a8
        mov.n    a2, a10
        retw.n
        .size   ic_get_next_tbtt, .-ic_get_next_tbtt
.Lsec_143_11:

        .section .text.ic_del_rx_ba,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_144_0, hal_agreement_del_rx_ba
.Lsec_144_4:
        .global ic_del_rx_ba
        .type   ic_del_rx_ba, @function
ic_del_rx_ba:
        entry    a1, 32
        extui    a10, a2, 0, 8
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_agreement_del_rx_ba
        l32r     a8, .Lsec_144_0
        callx8   a8
        retw.n
        .size   ic_del_rx_ba, .-ic_del_rx_ba
.Lsec_144_12:

        .section .text.ic_reset_rx_ba,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_145_0, hal_agreement_clr_rx_ba
.Lsec_145_4:
        .global ic_reset_rx_ba
        .type   ic_reset_rx_ba, @function
ic_reset_rx_ba:
        entry    a1, 32
        extui    a13, a5, 0, 8
        extui    a12, a4, 0, 16
        extui    a11, a3, 0, 8
        extui    a10, a2, 0, 8
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_agreement_clr_rx_ba
        l32r     a8, .Lsec_145_0
        callx8   a8
        retw.n
        .size   ic_reset_rx_ba, .-ic_reset_rx_ba
.Lsec_145_1b:

        .section .text.ic_add_rx_ba,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_146_0, hal_agreement_add_rx_ba
.Lsec_146_4:
        .global ic_add_rx_ba
        .type   ic_add_rx_ba, @function
ic_add_rx_ba:
        entry    a1, 32
        extui    a15, a7, 0, 8
        extui    a14, a6, 0, 16
        extui    a13, a5, 0, 8
        mov.n    a12, a4
        extui    a11, a2, 0, 8
        extui    a10, a3, 0, 8
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_agreement_add_rx_ba
        l32r     a8, .Lsec_146_0
        callx8   a8
        retw.n
        .size   ic_add_rx_ba, .-ic_add_rx_ba
.Lsec_146_20:

        .section .text.ic_reset_tbtt,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_147_0, wDev_Reset_TBTT
.Lsec_147_4:
        .global ic_reset_tbtt
        .type   ic_reset_tbtt, @function
ic_reset_tbtt:
        entry    a1, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, wDev_Reset_TBTT
        l32r     a8, .Lsec_147_0
        callx8   a8
        retw.n
        .size   ic_reset_tbtt, .-ic_reset_tbtt
.Lsec_147_f:

        .section .text.ic_del_key_all,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_148_0, wDev_remove_KeyEntry_all_cnx
.Lsec_148_4:
        .global ic_del_key_all
        .type   ic_del_key_all, @function
ic_del_key_all:
        entry    a1, 32
        mov.n    a11, a3
        extui    a10, a2, 0, 8
        .reloc  ., R_XTENSA_ASM_EXPAND, wDev_remove_KeyEntry_all_cnx
        l32r     a8, .Lsec_148_0
        callx8   a8
        retw.n
        .size   ic_del_key_all, .-ic_del_key_all
.Lsec_148_14:

        .section .text.ic_del_key,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_149_0, g_osi_funcs_p
        .literal .Lsec_149_4, wDev_remove_KeyEntry
.Lsec_149_8:
        .global ic_del_key
        .type   ic_del_key, @function
ic_del_key:
        entry    a1, 32
        l32r     a3, .Lsec_149_0
        l32i.n   a8, a3, 0
        l32i     a8, a8, 216
        callx8   a8
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, wDev_remove_KeyEntry
        l32r     a8, .Lsec_149_4
        callx8   a8
        l32i.n   a3, a3, 0
        l32i     a3, a3, 220
        callx8   a3
        retw.n
        .size   ic_del_key, .-ic_del_key
.Lsec_149_28:

        .section .text.ic_set_ac_param,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_150_0, lmacSetAcParam
.Lsec_150_4:
        .global ic_set_ac_param
        .type   ic_set_ac_param, @function
ic_set_ac_param:
        entry    a1, 32
        extui    a14, a6, 0, 16
        extui    a13, a5, 0, 8
        extui    a12, a4, 0, 8
        extui    a11, a3, 0, 8
        extui    a10, a2, 0, 8
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacSetAcParam
        l32r     a8, .Lsec_150_0
        callx8   a8
        retw.n
        .size   ic_set_ac_param, .-ic_set_ac_param
.Lsec_150_1e:

        .section .text.ic_ampdu_op,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_151_0, rc_get_trc_by_index
        .literal .Lsec_151_4, trc_onAmpduOp
.Lsec_151_8:
        .global ic_ampdu_op
        .type   ic_ampdu_op, @function
ic_ampdu_op:
        entry    a1, 32
        extui    a11, a3, 0, 8
        extui    a10, a2, 0, 8
        .reloc  ., R_XTENSA_ASM_EXPAND, rc_get_trc_by_index
        l32r     a8, .Lsec_151_0
        callx8   a8
        extui    a4, a4, 0, 8
        mov.n    a12, a4
        movi     a11, 2
        beqi     a5, 1, .Lsec_151_24
        movi.n   a11, 1
.Lsec_151_24:
        .reloc  ., R_XTENSA_ASM_EXPAND, trc_onAmpduOp
        l32r     a8, .Lsec_151_4
        callx8   a8
        retw.n
        .size   ic_ampdu_op, .-ic_ampdu_op
.Lsec_151_2c:

        .section .text.esp_mesh_set_6m_rate,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_152_0, s_is_6m
.Lsec_152_4:
        .global esp_mesh_set_6m_rate
        .type   esp_mesh_set_6m_rate, @function
esp_mesh_set_6m_rate:
        entry    a1, 32
        l32r     a8, .Lsec_152_0
        s8i      a2, a8, 0
        memw
        movi.n   a2, 0
        memw
        retw.n
        .size   esp_mesh_set_6m_rate, .-esp_mesh_set_6m_rate
.Lsec_152_17:

        .section .text.ic_set_trc,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_153_0, if_ctrl
        .literal .Lsec_153_4, g_mesh_is_started
        .literal .Lsec_153_8, s_is_6m
        .literal .Lsec_153_c, g_mesh_is_root
        .literal .Lsec_153_10, rc_enable_trc
        .literal .Lsec_153_14, rcUpdateAMPDUParam
        .literal .Lsec_153_18, rcUpdatePhyMode
        .literal .Lsec_153_1c, rc_disable_trc
.Lsec_153_20:
        .global ic_set_trc
        .type   ic_set_trc, @function
ic_set_trc:
        entry    a1, 32
        l8ui     a3, a2, 0
        l8ui     a10, a2, 1
        l32i     a11, a2, 4
        l8ui     a12, a2, 8
        beqz     a3, .Lsec_153_c8
        l8ui     a13, a2, 10
        l32i     a4, a2, 20
        .reloc  ., R_XTENSA_ASM_EXPAND, rc_enable_trc
        l32r     a8, .Lsec_153_10
        callx8   a8
        mov.n    a3, a10
        beqz     a10, .Lsec_153_ce
        l8ui     a8, a2, 1
        addx4    a9, a8, a8
        l32r     a5, .Lsec_153_0
        addx2    a9, a9, a5
        l8ui     a5, a9, 7
        l32r     a9, .Lsec_153_4
        l8ui     a9, a9, 0
        beqz.n   a9, .Lsec_153_74
        beqz.n   a8, .Lsec_153_65
.Lsec_153_5c:
        l32r     a5, .Lsec_153_8
        l8ui     a5, a5, 0
        j        .Lsec_153_74
.Lsec_153_65:
        l32r     a8, .Lsec_153_c
        memw
        l8ui     a8, a8, 0
        extui    a8, a8, 0, 8
        beqz     a8, .Lsec_153_5c
.Lsec_153_74:
        mov.n    a10, a3
        l8ui     a12, a2, 26
        extui    a13, a12, 2, 3
        extui    a12, a12, 0, 2
        l8ui     a11, a2, 12
        .reloc  ., R_XTENSA_ASM_EXPAND, rcUpdateAMPDUParam
        l32r     a8, .Lsec_153_14
        callx8   a8
        l8ui     a8, a2, 12
        s8i      a8, a3, 130
        memw
        beqz.n   a4, .Lsec_153_a9
        l8ui     a8, a4, 1
        s8i      a8, a3, 135
        memw
        l8ui     a4, a4, 0
        beqz.n   a4, .Lsec_153_a9
        movi.n   a4, 6
        s8i      a4, a3, 130
        memw
.Lsec_153_a9:
        mov.n    a12, a5
        mov.n    a10, a3
        l32i.n   a11, a2, 16
        addi     a11, a11, 96
        sext     a11, a11, 7
        l8ui     a14, a2, 25
        l8ui     a13, a2, 24
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, rcUpdatePhyMode
        l32r     a8, .Lsec_153_18
        callx8   a8
        j        .Lsec_153_ce
        .byte    0x00
.Lsec_153_c8:
        .reloc  ., R_XTENSA_ASM_EXPAND, rc_disable_trc
        l32r     a8, .Lsec_153_1c
        callx8   a8
.Lsec_153_ce:
        retw.n
        .size   ic_set_trc, .-ic_set_trc
.Lsec_153_d0:

        .section .text.ic_bb_check_noise_init,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_154_0, .Lsec_209_0
        .literal .Lsec_154_4, bb_intr_handl
        .literal .Lsec_154_8, g_osi_funcs_p
        .literal .Lsec_154_c, wifi_log
.Lsec_154_10:
        .global ic_bb_check_noise_init
        .type   ic_bb_check_noise_init, @function
ic_bb_check_noise_init:
        entry    a1, 32
        l32r     a13, .Lsec_154_0
        movi.n   a11, 1
        movi.n   a12, 3
        slli     a11, a11, 11
        movi.n   a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_154_c
        callx8   a8
        l32r     a2, .Lsec_154_8
        movi.n   a12, 0
        movi.n   a10, 9
        l32i     a8, a2, 0
        l32r     a11, .Lsec_154_4
        l32i     a8, a8, 16
        callx8   a8
        movi     a10, 0x200
        l32i.n   a2, a2, 0
        l32i.n   a2, a2, 20
        callx8   a2
        retw.n
        .size   ic_bb_check_noise_init, .-ic_bb_check_noise_init
.Lsec_154_44:

        .section .text.ic_deinit,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_155_0, pm_deattach
        .literal .Lsec_155_4, wDev_Rxbuf_Deinit
        .literal .Lsec_155_8, pp_deattach
        .literal .Lsec_155_c, trc_deinit
.Lsec_155_10:
        .global ic_deinit
        .type   ic_deinit, @function
ic_deinit:
        entry    a1, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_deattach
        l32r     a8, .Lsec_155_0
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, wDev_Rxbuf_Deinit
        l32r     a8, .Lsec_155_4
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_deattach
        l32r     a8, .Lsec_155_8
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, trc_deinit
        l32r     a8, .Lsec_155_c
        callx8   a8
        movi.n   a2, 0
        retw.n
        .size   ic_deinit, .-ic_deinit
.Lsec_155_2f:

        .section .text.ic_init,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_156_0, esp_wifi_internal_on_coex_start
        .literal .Lsec_156_4, g_osi_funcs_p
        .literal .Lsec_156_8, esp_wifi_internal_on_coex_schm_phase
        .literal .Lsec_156_c, g_wifi_menuconfig
        .literal .Lsec_156_10, trc_init
        .literal .Lsec_156_14, lmacInit
        .literal .Lsec_156_18, pp_attach
        .literal .Lsec_156_1c, wDev_Rxbuf_Init
        .literal .Lsec_156_20, pm_attach
        .literal .Lsec_156_24, pm_enable_sta_disconnected_power_management
        .literal .Lsec_156_28, ic_deinit
.Lsec_156_2c:
        .global ic_init
        .type   ic_init, @function
ic_init:
        entry    a1, 32
        extui    a10, a4, 0, 8
        .reloc  ., R_XTENSA_ASM_EXPAND, trc_init
        l32r     a8, .Lsec_156_10
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacInit
        l32r     a8, .Lsec_156_14
        callx8   a8
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_attach
        l32r     a8, .Lsec_156_18
        callx8   a8
        mov.n    a5, a2
        or       a2, a10, a10
        bnez     a10, .Lsec_156_90
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, wDev_Rxbuf_Init
        l32r     a8, .Lsec_156_1c
        callx8   a8
        mov.n    a2, a10
        bnez.n   a10, .Lsec_156_90
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_attach
        l32r     a8, .Lsec_156_20
        callx8   a8
        l32r     a3, .Lsec_156_4
        l32i.n   a8, a3, 0
        l32r     a10, .Lsec_156_0
        l32i     a8, a8, 0x1d0
        callx8   a8
        mov.n    a10, a2
        l32i     a3, a3, 0
        l32r     a11, .Lsec_156_8
        l32i     a3, a3, 0x1cc
        callx8   a3
        l32r     a3, .Lsec_156_c
        l8ui     a3, a3, 72
        beqz.n   a3, .Lsec_156_96
        movi.n   a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_enable_sta_disconnected_power_management
        l32r     a8, .Lsec_156_24
        callx8   a8
        j        .Lsec_156_96
        .byte    0x00
.Lsec_156_90:
        .reloc  ., R_XTENSA_ASM_EXPAND, ic_deinit
        l32r     a8, .Lsec_156_28
        callx8   a8
.Lsec_156_96:
        retw.n
        .size   ic_init, .-ic_init
.Lsec_156_98:

        .section .text.ic_set_interrupt_handler,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_157_0, g_wifi_menuconfig
        .literal .Lsec_157_4, g_osi_funcs_p
        .literal .Lsec_157_8, wDev_ProcessFiq
        .literal .Lsec_157_c, hal_init
.Lsec_157_10:
        .global ic_set_interrupt_handler
        .type   ic_set_interrupt_handler, @function
ic_set_interrupt_handler:
        entry    a1, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_init
        l32r     a8, .Lsec_157_c
        callx8   a8
        l32r     a2, .Lsec_157_4
        movi.n   a12, 0
        movi.n   a13, 1
        mov.n    a11, a12
        l32r     a9, .Lsec_157_0
        l32i.n   a8, a2, 0
        l32i.n   a10, a9, 52
        l32i.n   a8, a8, 8
        callx8   a8
        movi.n   a12, 0
        mov.n    a10, a12
        l32i.n   a8, a2, 0
        l32r     a11, .Lsec_157_8
        l32i.n   a8, a8, 16
        callx8   a8
        movi.n   a10, 1
        l32i.n   a2, a2, 0
        l32i.n   a2, a2, 20
        callx8   a2
        retw.n
        .size   ic_set_interrupt_handler, .-ic_set_interrupt_handler
.Lsec_157_47:

        .section .text.ic_clear_interrupt_handler,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_158_0, g_osi_funcs_p
        .literal .Lsec_158_4, hal_deinit
.Lsec_158_8:
        .global ic_clear_interrupt_handler
        .type   ic_clear_interrupt_handler, @function
ic_clear_interrupt_handler:
        entry    a1, 32
        l32r     a2, .Lsec_158_0
        movi.n   a10, 1
        l32i.n   a8, a2, 0
        l32i     a8, a8, 24
        callx8   a8
        movi.n   a12, 0
        mov.n    a11, a12
        l32i.n   a8, a2, 0
        mov.n    a10, a12
        l32i.n   a8, a8, 16
        callx8   a8
        movi.n   a11, 0
        mov.n    a10, a11
        l32i.n   a2, a2, 0
        l32i.n   a2, a2, 12
        callx8   a2
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_deinit
        l32r     a8, .Lsec_158_4
        callx8   a8
        retw.n
        .size   ic_clear_interrupt_handler, .-ic_clear_interrupt_handler
.Lsec_158_38:

        .section .text.ic_enable_rx,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_159_0, 0x3ff73084
.Lsec_159_4:
        .global ic_enable_rx
        .type   ic_enable_rx, @function
ic_enable_rx:
        entry    a1, 32
        l32r     a10, .Lsec_159_0
        movi.n   a9, -1
        slli     a9, a9, 31
        memw
        l32i.n   a8, a10, 0
        or       a8, a8, a9
        memw
        s32i.n   a8, a10, 0
        memw
        retw.n
        .size   ic_enable_rx, .-ic_enable_rx
.Lsec_159_21:

        .section .text.ic_disable_rx,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_160_0, 0x3ff73084
        .literal .Lsec_160_4, 0x7fffffff
.Lsec_160_8:
        .global ic_disable_rx
        .type   ic_disable_rx, @function
ic_disable_rx:
        entry    a1, 32
        l32r     a9, .Lsec_160_0
        l32r     a10, .Lsec_160_4
        memw
        l32i.n   a8, a9, 0
        and      a8, a8, a10
        memw
        s32i.n   a8, a9, 0
        memw
        retw.n
        .size   ic_disable_rx, .-ic_disable_rx
.Lsec_160_23:

        .section .text.ic_set_beacon_int,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_161_0, wDev_Set_Beacon_Int
.Lsec_161_4:
        .global ic_set_beacon_int
        .type   ic_set_beacon_int, @function
ic_set_beacon_int:
        entry    a1, 32
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, wDev_Set_Beacon_Int
        l32r     a8, .Lsec_161_0
        callx8   a8
        retw.n
        .size   ic_set_beacon_int, .-ic_set_beacon_int
.Lsec_161_12:

        .section .text.ic_set_mac,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_162_0, hal_mac_set_addr
.Lsec_162_4:
        .global ic_set_mac
        .type   ic_set_mac, @function
ic_set_mac:
        entry    a1, 32
        mov.n    a11, a3
        extui    a10, a2, 0, 8
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_set_addr
        l32r     a8, .Lsec_162_0
        callx8   a8
        retw.n
        .size   ic_set_mac, .-ic_set_mac
.Lsec_162_14:

        .section .text.ic_set_bssid,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_163_0, hal_mac_set_bssid
.Lsec_163_4:
        .global ic_set_bssid
        .type   ic_set_bssid, @function
ic_set_bssid:
        entry    a1, 32
        mov.n    a11, a3
        extui    a10, a2, 0, 8
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_set_bssid
        l32r     a8, .Lsec_163_0
        callx8   a8
        retw.n
        .size   ic_set_bssid, .-ic_set_bssid
.Lsec_163_14:

        .section .text.ic_set_current_channel,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_164_0, wDev_SetCurChannel
.Lsec_164_4:
        .global ic_set_current_channel
        .type   ic_set_current_channel, @function
ic_set_current_channel:
        entry    a1, 32
        mov.n    a10, a2
        beqz.n   a2, .Lsec_164_11
        .reloc  ., R_XTENSA_ASM_EXPAND, wDev_SetCurChannel
        l32r     a8, .Lsec_164_0
        callx8   a8
.Lsec_164_11:
        retw.n
        .size   ic_set_current_channel, .-ic_set_current_channel
.Lsec_164_13:

        .section .text.ic_get_random,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_165_0, hal_random
.Lsec_165_4:
        .global ic_get_random
        .type   ic_get_random, @function
ic_get_random:
        entry    a1, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_random
        l32r     a8, .Lsec_165_0
        callx8   a8
        mov.n    a2, a10
        retw.n
        .size   ic_get_random, .-ic_get_random
.Lsec_165_11:

        .section .text.ic_get_trc,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_166_0, rc_get_trc_by_index
.Lsec_166_4:
        .global ic_get_trc
        .type   ic_get_trc, @function
ic_get_trc:
        entry    a1, 32
        extui    a11, a3, 0, 8
        extui    a10, a2, 0, 8
        .reloc  ., R_XTENSA_ASM_EXPAND, rc_get_trc_by_index
        l32r     a8, .Lsec_166_0
        callx8   a8
        mov.n    a2, a10
        retw.n
        .size   ic_get_trc, .-ic_get_trc
.Lsec_166_17:

        .section .text.ic_tx_is_idle,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_167_0, lmacIsIdle
.Lsec_167_4:
        .global ic_tx_is_idle
        .type   ic_tx_is_idle, @function
ic_tx_is_idle:
        entry    a1, 32
        extui    a10, a2, 0, 8
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacIsIdle
        l32r     a8, .Lsec_167_0
        callx8   a8
        mov.n    a2, a10
        retw.n
        .size   ic_tx_is_idle, .-ic_tx_is_idle
.Lsec_167_14:

        .section .text.ic_get_pp_hdl,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_168_0, ppGetTaskHdl
.Lsec_168_4:
        .global ic_get_pp_hdl
        .type   ic_get_pp_hdl, @function
ic_get_pp_hdl:
        entry    a1, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, ppGetTaskHdl
        l32r     a8, .Lsec_168_0
        callx8   a8
        mov.n    a2, a10
        retw.n
        .size   ic_get_pp_hdl, .-ic_get_pp_hdl
.Lsec_168_11:

        .section .text.ic_set_rx_policy_ubssid_check,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_169_0, hal_mac_set_rxq_policy
.Lsec_169_4:
        .global ic_set_rx_policy_ubssid_check
        .type   ic_set_rx_policy_ubssid_check, @function
ic_set_rx_policy_ubssid_check:
        entry    a1, 32
        extui    a10, a2, 0, 8
        extui    a11, a3, 0, 8
        movi.n   a2, 0
        bgeui    a10, 4, .Lsec_169_1a
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_set_rxq_policy
        l32r     a8, .Lsec_169_0
        callx8   a8
        movi.n   a2, 1
.Lsec_169_1a:
        retw.n
        .size   ic_set_rx_policy_ubssid_check, .-ic_set_rx_policy_ubssid_check
.Lsec_169_1c:

        .section .text.ic_set_rx_policy,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_170_0, hal_mac_rx_set_policy
.Lsec_170_4:
        .global ic_set_rx_policy
        .type   ic_set_rx_policy, @function
ic_set_rx_policy:
        entry    a1, 32
        extui    a13, a5, 0, 8
        extui    a12, a4, 0, 8
        extui    a11, a3, 0, 8
        extui    a10, a2, 0, 8
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_rx_set_policy
        l32r     a8, .Lsec_170_0
        callx8   a8
        movi.n   a2, 1
        retw.n
        .size   ic_set_rx_policy, .-ic_set_rx_policy
.Lsec_170_1d:

        .section .text.ic_set_sta_auth_flag,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_171_0, wDev_SetAuthed
.Lsec_171_4:
        .global ic_set_sta_auth_flag
        .type   ic_set_sta_auth_flag, @function
ic_set_sta_auth_flag:
        entry    a1, 32
        extui    a10, a2, 0, 8
        .reloc  ., R_XTENSA_ASM_EXPAND, wDev_SetAuthed
        l32r     a8, .Lsec_171_0
        callx8   a8
        retw.n
        .size   ic_set_sta_auth_flag, .-ic_set_sta_auth_flag
.Lsec_171_12:

        .section .text.ic_set_interface,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_172_0, ppSetInterface
.Lsec_172_4:
        .global ic_set_interface
        .type   ic_set_interface, @function
ic_set_interface:
        entry    a1, 32
        mov.n    a11, a3
        extui    a10, a2, 0, 8
        .reloc  ., R_XTENSA_ASM_EXPAND, ppSetInterface
        l32r     a8, .Lsec_172_0
        callx8   a8
        retw.n
        .size   ic_set_interface, .-ic_set_interface
.Lsec_172_14:

        .section .text.ic_trc_set_per_pkt_rate,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_173_0, trc_set_per_pkt_rate
.Lsec_173_4:
        .global ic_trc_set_per_pkt_rate
        .type   ic_trc_set_per_pkt_rate, @function
ic_trc_set_per_pkt_rate:
        entry    a1, 32
        mov.n    a11, a3
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, trc_set_per_pkt_rate
        l32r     a8, .Lsec_173_0
        callx8   a8
        retw.n
        .size   ic_trc_set_per_pkt_rate, .-ic_trc_set_per_pkt_rate
.Lsec_173_13:

        .section .text.ic_trc_update_ifx_phy_mode,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_174_0, trc_update_ifx_phy_mode
.Lsec_174_4:
        .global ic_trc_update_ifx_phy_mode
        .type   ic_trc_update_ifx_phy_mode, @function
ic_trc_update_ifx_phy_mode:
        entry    a1, 32
        mov.n    a11, a3
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, trc_update_ifx_phy_mode
        l32r     a8, .Lsec_174_0
        callx8   a8
        retw.n
        .size   ic_trc_update_ifx_phy_mode, .-ic_trc_update_ifx_phy_mode
.Lsec_174_13:

        .section .text.ic_stop_hw_txq,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_175_0, lmac_stop_hw_txq
.Lsec_175_4:
        .global ic_stop_hw_txq
        .type   ic_stop_hw_txq, @function
ic_stop_hw_txq:
        entry    a1, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, lmac_stop_hw_txq
        l32r     a8, .Lsec_175_0
        callx8   a8
        mov.n    a2, a10
        retw.n
        .size   ic_stop_hw_txq, .-ic_stop_hw_txq
.Lsec_175_11:

        .section .text.ic_stop_sw_txq,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_176_0, .Lsec_208_0
        .literal .Lsec_176_4, wifi_log
        .literal .Lsec_176_8, pp_stop_sw_txq
.Lsec_176_c:
        .global ic_stop_sw_txq
        .type   ic_stop_sw_txq, @function
ic_stop_sw_txq:
        entry    a1, 32
        l32r     a13, .Lsec_176_0
        movi.n   a12, 3
        movi     a11, 1
        movi     a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_176_4
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_stop_sw_txq
        l32r     a8, .Lsec_176_8
        callx8   a8
        movi.n   a2, 0
        retw.n
        .size   ic_stop_sw_txq, .-ic_stop_sw_txq
.Lsec_176_2a:

        .section .text.ic_txq_empty,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_177_0, ppTxqEmpty
.Lsec_177_4:
        .global ic_txq_empty
        .type   ic_txq_empty, @function
ic_txq_empty:
        entry    a1, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, ppTxqEmpty
        l32r     a8, .Lsec_177_0
        callx8   a8
        mov.n    a2, a10
        retw.n
        .size   ic_txq_empty, .-ic_txq_empty
.Lsec_177_11:

        .section .text.ic_set_fix_rate,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_178_0, rc_set_fix_rate
.Lsec_178_4:
        .global ic_set_fix_rate
        .type   ic_set_fix_rate, @function
ic_set_fix_rate:
        entry    a1, 32
        mov.n    a12, a4
        extui    a11, a3, 0, 8
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, rc_set_fix_rate
        l32r     a8, .Lsec_178_0
        callx8   a8
        mov.n    a2, a10
        retw.n
        .size   ic_set_fix_rate, .-ic_set_fix_rate
.Lsec_178_18:

        .section .text.ic_get_fix_rate,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_179_0, rc_get_fix_rate
.Lsec_179_4:
        .global ic_get_fix_rate
        .type   ic_get_fix_rate, @function
ic_get_fix_rate:
        entry    a1, 32
        mov.n    a12, a4
        mov.n    a11, a3
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, rc_get_fix_rate
        l32r     a8, .Lsec_179_0
        callx8   a8
        mov.n    a2, a10
        retw.n
        .size   ic_get_fix_rate, .-ic_get_fix_rate
.Lsec_179_18:

        .section .text.ic_create_wifi_task,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_180_0, pp_create_task
.Lsec_180_4:
        .global ic_create_wifi_task
        .type   ic_create_wifi_task, @function
ic_create_wifi_task:
        entry    a1, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_create_task
        l32r     a8, .Lsec_180_0
        callx8   a8
        mov.n    a2, a10
        retw.n
        .size   ic_create_wifi_task, .-ic_create_wifi_task
.Lsec_180_11:

        .section .text.ic_delete_wifi_task,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_181_0, pp_delete_task
.Lsec_181_4:
        .global ic_delete_wifi_task
        .type   ic_delete_wifi_task, @function
ic_delete_wifi_task:
        entry    a1, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_delete_task
        l32r     a8, .Lsec_181_0
        callx8   a8
        mov.n    a2, a10
        retw.n
        .size   ic_delete_wifi_task, .-ic_delete_wifi_task
.Lsec_181_11:

        .section .text.ic_set_csi,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_182_0, hal_mac_set_csi
.Lsec_182_4:
        .global ic_set_csi
        .type   ic_set_csi, @function
ic_set_csi:
        entry    a1, 32
        extui    a10, a2, 0, 8
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_set_csi
        l32r     a8, .Lsec_182_0
        callx8   a8
        mov.n    a2, a10
        retw.n
        .size   ic_set_csi, .-ic_set_csi
.Lsec_182_14:

        .section .wifislprxiram.12,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_183_0, hal_mac_init
.Lsec_183_4:
        .global ic_mac_init
        .type   ic_mac_init, @function
ic_mac_init:
        entry    a1, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_init
        l32r     a8, .Lsec_183_0
        callx8   a8
        movi.n   a2, 0
        retw.n
        .size   ic_mac_init, .-ic_mac_init
.Lsec_183_11:

        .section .wifislprxiram.13,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_184_0, hal_mac_deinit
.Lsec_184_4:
        .global ic_mac_deinit
        .type   ic_mac_deinit, @function
ic_mac_deinit:
        entry    a1, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_deinit
        l32r     a8, .Lsec_184_0
        callx8   a8
        movi.n   a2, 0
        retw.n
        .size   ic_mac_deinit, .-ic_mac_deinit
.Lsec_184_11:

        .section .text.ic_csi_set_config,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_185_0, 0x3ff730f8
        .literal .Lsec_185_4, 0x3ff730fc
        .literal .Lsec_185_8, 0xdfffffff
        .literal .Lsec_185_c, phy_chan_dump_cfg
        .literal .Lsec_185_10, phy_chan_filt_set
.Lsec_185_14:
        .global ic_csi_set_config
        .type   ic_csi_set_config, @function
ic_csi_set_config:
        entry    a1, 32
        l8ui     a14, a2, 2
        l8ui     a13, a2, 1
        l8ui     a12, a2, 0
        l8ui     a11, a2, 6
        l8ui     a10, a2, 5
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_chan_dump_cfg
        l32r     a8, .Lsec_185_c
        callx8   a8
        l8ui     a11, a2, 3
        l8ui     a10, a2, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_chan_filt_set
        l32r     a8, .Lsec_185_10
        callx8   a8
        l8ui     a8, a2, 7
        l32r     a2, .Lsec_185_0
        beqz.n   a8, .Lsec_185_68
        movi.n   a8, 1
        slli     a8, a8, 29
        memw
        l32i.n   a9, a2, 0
        or       a9, a9, a8
        memw
        s32i.n   a9, a2, 0
        l32r     a9, .Lsec_185_4
        memw
        l32i.n   a2, a9, 0
        or       a2, a2, a8
        memw
        s32i.n   a2, a9, 0
        memw
        j        .Lsec_185_88
.Lsec_185_68:
        memw
        l32i.n   a8, a2, 0
        l32r     a9, .Lsec_185_8
        and      a8, a8, a9
        memw
        s32i.n   a8, a2, 0
        l32r     a8, .Lsec_185_4
        memw
        l32i.n   a2, a8, 0
        and      a2, a2, a9
        memw
        s32i.n   a2, a8, 0
.Lsec_185_88:
        movi.n   a2, 0
        memw
        retw.n
        .size   ic_csi_set_config, .-ic_csi_set_config
.Lsec_185_8f:

        .section .text.ic_update_modem_sleep_default_params,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_186_0, pm_update_modem_sleep_default_params
.Lsec_186_4:
        .global ic_update_modem_sleep_default_params
        .type   ic_update_modem_sleep_default_params, @function
ic_update_modem_sleep_default_params:
        entry    a1, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_update_modem_sleep_default_params
        l32r     a8, .Lsec_186_0
        callx8   a8
        retw.n
        .size   ic_update_modem_sleep_default_params, .-ic_update_modem_sleep_default_params
.Lsec_186_f:

        .section .text.ic_update_light_sleep_default_params,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_187_0, pm_update_light_sleep_default_params
.Lsec_187_4:
        .global ic_update_light_sleep_default_params
        .type   ic_update_light_sleep_default_params, @function
ic_update_light_sleep_default_params:
        entry    a1, 32
        mov.n    a11, a3
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_update_light_sleep_default_params
        l32r     a8, .Lsec_187_0
        callx8   a8
        retw.n
        .size   ic_update_light_sleep_default_params, .-ic_update_light_sleep_default_params
.Lsec_187_13:

        .section .text.ic_set_sleep_min_active_time,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_188_0, pm_set_sleep_min_active_time
.Lsec_188_4:
        .global ic_set_sleep_min_active_time
        .type   ic_set_sleep_min_active_time, @function
ic_set_sleep_min_active_time:
        entry    a1, 32
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_set_sleep_min_active_time
        l32r     a8, .Lsec_188_0
        callx8   a8
        retw.n
        .size   ic_set_sleep_min_active_time, .-ic_set_sleep_min_active_time
.Lsec_188_12:

        .section .text.ic_set_keep_alive_time,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_189_0, pm_set_keep_alive_time
.Lsec_189_4:
        .global ic_set_keep_alive_time
        .type   ic_set_keep_alive_time, @function
ic_set_keep_alive_time:
        entry    a1, 32
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_set_keep_alive_time
        l32r     a8, .Lsec_189_0
        callx8   a8
        retw.n
        .size   ic_set_keep_alive_time, .-ic_set_keep_alive_time
.Lsec_189_12:

        .section .text.ic_get_keep_alive_time,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_190_0, pm_get_keep_alive_time
.Lsec_190_4:
        .global ic_get_keep_alive_time
        .type   ic_get_keep_alive_time, @function
ic_get_keep_alive_time:
        entry    a1, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_get_keep_alive_time
        l32r     a8, .Lsec_190_0
        callx8   a8
        mov.n    a2, a10
        retw.n
        .size   ic_get_keep_alive_time, .-ic_get_keep_alive_time
.Lsec_190_11:

        .section .text.ic_set_sleep_wait_broadcast_data_time,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_191_0, pm_set_sleep_wait_broadcast_data_time
.Lsec_191_4:
        .global ic_set_sleep_wait_broadcast_data_time
        .type   ic_set_sleep_wait_broadcast_data_time, @function
ic_set_sleep_wait_broadcast_data_time:
        entry    a1, 32
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_set_sleep_wait_broadcast_data_time
        l32r     a8, .Lsec_191_0
        callx8   a8
        retw.n
        .size   ic_set_sleep_wait_broadcast_data_time, .-ic_set_sleep_wait_broadcast_data_time
.Lsec_191_12:

        .section .text.ic_beacon_monitor_configure,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_192_0, pm_beacon_monitor_configure
.Lsec_192_4:
        .global ic_beacon_monitor_configure
        .type   ic_beacon_monitor_configure, @function
ic_beacon_monitor_configure:
        entry    a1, 32
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_beacon_monitor_configure
        l32r     a8, .Lsec_192_0
        callx8   a8
        retw.n
        .size   ic_beacon_monitor_configure, .-ic_beacon_monitor_configure
.Lsec_192_12:

        .section .text.ic_beacon_offset_configure,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_193_0, ptr_beacon_offset_funcs
.Lsec_193_4:
        .global ic_beacon_offset_configure
        .type   ic_beacon_offset_configure, @function
ic_beacon_offset_configure:
        entry    a1, 32
        l32r     a8, .Lsec_193_0
        mov.n    a10, a2
        l32i.n   a8, a8, 0
        l32i.n   a8, a8, 20
        beqz.n   a8, .Lsec_193_15
        callx8   a8
.Lsec_193_15:
        retw.n
        .size   ic_beacon_offset_configure, .-ic_beacon_offset_configure
.Lsec_193_17:

        .section .text.ic_beacon_offset_set_rx_beacon_standard,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_194_0, ptr_beacon_offset_funcs
.Lsec_194_4:
        .global ic_beacon_offset_set_rx_beacon_standard
        .type   ic_beacon_offset_set_rx_beacon_standard, @function
ic_beacon_offset_set_rx_beacon_standard:
        entry    a1, 32
        l32r     a8, .Lsec_194_0
        extui    a10, a2, 0, 8
        l32i.n   a8, a8, 0
        l32i.n   a8, a8, 28
        beqz.n   a8, .Lsec_194_16
        callx8   a8
.Lsec_194_16:
        retw.n
        .size   ic_beacon_offset_set_rx_beacon_standard, .-ic_beacon_offset_set_rx_beacon_standard
.Lsec_194_18:

        .section .text.ic_update_listen_interval,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_195_0, wifi_nvs_set_sta_listen_interval
        .literal .Lsec_195_4, pm_is_open
        .literal .Lsec_195_8, pm_scale_listen_interval
.Lsec_195_c:
        .global ic_update_listen_interval
        .type   ic_update_listen_interval, @function
ic_update_listen_interval:
        entry    a1, 32
        extui    a10, a2, 0, 16
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_nvs_set_sta_listen_interval
        l32r     a8, .Lsec_195_0
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_is_open
        l32r     a8, .Lsec_195_4
        callx8   a8
        beqz.n   a10, .Lsec_195_26
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_scale_listen_interval
        l32r     a8, .Lsec_195_8
        callx8   a8
.Lsec_195_26:
        retw.n
        .size   ic_update_listen_interval, .-ic_update_listen_interval
.Lsec_195_28:

        .section .text.ic_rx_disable_bssid_check,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_196_0, hal_rx_disable_bssid_check
.Lsec_196_4:
        .global ic_rx_disable_bssid_check
        .type   ic_rx_disable_bssid_check, @function
ic_rx_disable_bssid_check:
        entry    a1, 32
        extui    a10, a2, 0, 8
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_rx_disable_bssid_check
        l32r     a8, .Lsec_196_0
        callx8   a8
        retw.n
        .size   ic_rx_disable_bssid_check, .-ic_rx_disable_bssid_check
.Lsec_196_12:

        .section .text.ic_rx_enable_bssid_check,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_197_0, hal_rx_enable_bssid_check
.Lsec_197_4:
        .global ic_rx_enable_bssid_check
        .type   ic_rx_enable_bssid_check, @function
ic_rx_enable_bssid_check:
        entry    a1, 32
        extui    a10, a2, 0, 8
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_rx_enable_bssid_check
        l32r     a8, .Lsec_197_0
        callx8   a8
        retw.n
        .size   ic_rx_enable_bssid_check, .-ic_rx_enable_bssid_check
.Lsec_197_12:

        .section .text.ic_set_bar_rate,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_198_0, ppSetBarRate
.Lsec_198_4:
        .global ic_set_bar_rate
        .type   ic_set_bar_rate, @function
ic_set_bar_rate:
        entry    a1, 32
        extui    a10, a2, 0, 8
        .reloc  ., R_XTENSA_ASM_EXPAND, ppSetBarRate
        l32r     a8, .Lsec_198_0
        callx8   a8
        retw.n
        .size   ic_set_bar_rate, .-ic_set_bar_rate
.Lsec_198_12:

        .section .text.ic_trc_update_def_rate,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_199_0, trc_update_def_rate
.Lsec_199_4:
        .global ic_trc_update_def_rate
        .type   ic_trc_update_def_rate, @function
ic_trc_update_def_rate:
        entry    a1, 32
        extui    a12, a4, 0, 8
        mov.n    a11, a3
        extui    a10, a2, 0, 8
        .reloc  ., R_XTENSA_ASM_EXPAND, trc_update_def_rate
        l32r     a8, .Lsec_199_0
        callx8   a8
        retw.n
        .size   ic_trc_update_def_rate, .-ic_trc_update_def_rate
.Lsec_199_17:

        .section .text.ic_get_espnow_rate,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_200_0, trc_get_espnow_rate
.Lsec_200_4:
        .global ic_get_espnow_rate
        .type   ic_get_espnow_rate, @function
ic_get_espnow_rate:
        entry    a1, 32
        extui    a10, a2, 0, 8
        .reloc  ., R_XTENSA_ASM_EXPAND, trc_get_espnow_rate
        l32r     a8, .Lsec_200_0
        callx8   a8
        mov.n    a2, a10
        retw.n
        .size   ic_get_espnow_rate, .-ic_get_espnow_rate
.Lsec_200_14:

        .section .text.ic_get_80211_tx_rate,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_201_0, trc_get_80211_tx_rate
.Lsec_201_4:
        .global ic_get_80211_tx_rate
        .type   ic_get_80211_tx_rate, @function
ic_get_80211_tx_rate:
        entry    a1, 32
        extui    a10, a2, 0, 8
        .reloc  ., R_XTENSA_ASM_EXPAND, trc_get_80211_tx_rate
        l32r     a8, .Lsec_201_0
        callx8   a8
        mov.n    a2, a10
        retw.n
        .size   ic_get_80211_tx_rate, .-ic_get_80211_tx_rate
.Lsec_201_14:

        .section .text.ic_get_80211_tx_rate_config,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_202_0, trc_get_80211_tx_rate_config
.Lsec_202_4:
        .global ic_get_80211_tx_rate_config
        .type   ic_get_80211_tx_rate_config, @function
ic_get_80211_tx_rate_config:
        entry    a1, 32
        mov.n    a11, a3
        extui    a10, a2, 0, 8
        .reloc  ., R_XTENSA_ASM_EXPAND, trc_get_80211_tx_rate_config
        l32r     a8, .Lsec_202_0
        callx8   a8
        retw.n
        .size   ic_get_80211_tx_rate_config, .-ic_get_80211_tx_rate_config
.Lsec_202_14:

        .section .text.ic_set_espnow_rate,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_203_0, trc_set_espnow_rate
.Lsec_203_4:
        .global ic_set_espnow_rate
        .type   ic_set_espnow_rate, @function
ic_set_espnow_rate:
        entry    a1, 32
        mov.n    a11, a3
        extui    a10, a2, 0, 8
        .reloc  ., R_XTENSA_ASM_EXPAND, trc_set_espnow_rate
        l32r     a8, .Lsec_203_0
        callx8   a8
        retw.n
        .size   ic_set_espnow_rate, .-ic_set_espnow_rate
.Lsec_203_14:

        .section .text.ic_set_80211_tx_rate,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_204_0, trc_set_80211_tx_rate
.Lsec_204_4:
        .global ic_set_80211_tx_rate
        .type   ic_set_80211_tx_rate, @function
ic_set_80211_tx_rate:
        entry    a1, 32
        mov.n    a11, a3
        extui    a10, a2, 0, 8
        .reloc  ., R_XTENSA_ASM_EXPAND, trc_set_80211_tx_rate
        l32r     a8, .Lsec_204_0
        callx8   a8
        retw.n
        .size   ic_set_80211_tx_rate, .-ic_set_80211_tx_rate
.Lsec_204_14:

        .section .text.ic_set_80211_tx_rate_config,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_205_0, trc_set_80211_tx_rate_config
.Lsec_205_4:
        .global ic_set_80211_tx_rate_config
        .type   ic_set_80211_tx_rate_config, @function
ic_set_80211_tx_rate_config:
        entry    a1, 32
        mov.n    a11, a3
        extui    a10, a2, 0, 8
        .reloc  ., R_XTENSA_ASM_EXPAND, trc_set_80211_tx_rate_config
        l32r     a8, .Lsec_205_0
        callx8   a8
        retw.n
        .size   ic_set_80211_tx_rate_config, .-ic_set_80211_tx_rate_config
.Lsec_205_14:

        .section .text.ic_get_default_sched,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_206_0, rc_get_default_sched
.Lsec_206_4:
        .global ic_get_default_sched
        .type   ic_get_default_sched, @function
ic_get_default_sched:
        entry    a1, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, rc_get_default_sched
        l32r     a8, .Lsec_206_0
        callx8   a8
        mov.n    a2, a10
        retw.n
        .size   ic_get_default_sched, .-ic_get_default_sched
.Lsec_206_11:

        .section .text.ic_get_G6M_sched,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_207_0, rc_get_G6M_sched
.Lsec_207_4:
        .global ic_get_G6M_sched
        .type   ic_get_G6M_sched, @function
ic_get_G6M_sched:
        entry    a1, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, rc_get_G6M_sched
        l32r     a8, .Lsec_207_0
        callx8   a8
        mov.n    a2, a10
        retw.n
        .size   ic_get_G6M_sched, .-ic_get_G6M_sched
.Lsec_207_11:

        .section .rodata_wlog_info.11,"a",@progbits
        .align  1
.Lsec_208_0:
        .byte   0x73, 0x74, 0x6f, 0x70, 0x20, 0x73, 0x77, 0x20, 0x74, 0x78, 0x71, 0x00

        .section .rodata_wlog_info.10,"a",@progbits
        .align  1
.Lsec_209_0:
        .byte   0x62, 0x62, 0x20, 0x6e, 0x6f, 0x69, 0x73, 0x65, 0x20, 0x63, 0x68, 0x65, 0x63, 0x6b, 0x20, 0x69
        .byte   0x6e, 0x69, 0x74, 0x00

        .section .bss.s_is_6m,"aw",@nobits
        .align  1
.Lsec_210_0:
        .global s_is_6m
        .type   s_is_6m, @object
        .size   s_is_6m, 1
s_is_6m:
        .skip   1

        .section .rodata_wlog_error.9,"a",@progbits
        .align  1
.Lsec_211_0:
        .byte   0x73, 0x65, 0x74, 0x20, 0x76, 0x69, 0x66, 0x3a, 0x20, 0x69, 0x6e, 0x76, 0x61, 0x6c, 0x69, 0x64
        .byte   0x20, 0x69, 0x6e, 0x64, 0x65, 0x78, 0x3d, 0x25, 0x64, 0x00

        .section .rodata_wlog_debug.8,"a",@progbits
        .align  1
.Lsec_212_0:
        .byte   0x66, 0x69, 0x6c, 0x74, 0x65, 0x72, 0x3a, 0x20, 0x73, 0x65, 0x74, 0x20, 0x72, 0x78, 0x20, 0x70
        .byte   0x6f, 0x6c, 0x69, 0x63, 0x79, 0x3d, 0x25, 0x64, 0x00

        .section .rodata_wlog_debug.7,"a",@progbits
        .align  1
.Lsec_213_0:
        .byte   0x66, 0x69, 0x6c, 0x74, 0x65, 0x72, 0x3a, 0x20, 0x73, 0x65, 0x74, 0x20, 0x72, 0x78, 0x20, 0x70
        .byte   0x6f, 0x6c, 0x69, 0x63, 0x79, 0x3d, 0x25, 0x64, 0x00

        .section .rodata_wlog_debug.6,"a",@progbits
        .align  1
.Lsec_214_0:
        .byte   0x66, 0x69, 0x6c, 0x74, 0x65, 0x72, 0x3a, 0x20, 0x73, 0x65, 0x74, 0x20, 0x72, 0x78, 0x20, 0x70
        .byte   0x6f, 0x6c, 0x69, 0x63, 0x79, 0x3d, 0x25, 0x64, 0x00

        .section .rodata_wlog_debug.5,"a",@progbits
        .align  1
.Lsec_215_0:
        .byte   0x66, 0x69, 0x6c, 0x74, 0x65, 0x72, 0x3a, 0x20, 0x73, 0x65, 0x74, 0x20, 0x72, 0x78, 0x20, 0x70
        .byte   0x6f, 0x6c, 0x69, 0x63, 0x79, 0x3d, 0x25, 0x64, 0x00

        .section .rodata_wlog_debug.4,"a",@progbits
        .align  1
.Lsec_216_0:
        .byte   0x66, 0x69, 0x6c, 0x74, 0x65, 0x72, 0x3a, 0x20, 0x73, 0x65, 0x74, 0x20, 0x72, 0x78, 0x20, 0x70
        .byte   0x6f, 0x6c, 0x69, 0x63, 0x79, 0x3d, 0x25, 0x64, 0x00

        .section .rodata_wlog_debug.3,"a",@progbits
        .align  1
.Lsec_217_0:
        .byte   0x66, 0x69, 0x6c, 0x74, 0x65, 0x72, 0x3a, 0x20, 0x73, 0x65, 0x74, 0x20, 0x72, 0x78, 0x20, 0x70
        .byte   0x6f, 0x6c, 0x69, 0x63, 0x79, 0x3d, 0x25, 0x64, 0x00

        .section .bss.if_ctrl,"aw",@nobits
        .align  1
.Lsec_218_0:
        .global if_ctrl
        .type   if_ctrl, @object
        .size   if_ctrl, 40
if_ctrl:
        .skip   40
