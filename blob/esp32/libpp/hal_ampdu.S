.section .text.hal_agreement_add_rx_ba,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_9_0, .Lsec_17_0
        .literal .Lsec_9_4, 0x3ff73258
        .literal .Lsec_9_8, 0x3ff73254
        .literal .Lsec_9_c, 0x3ff7325c
        .literal .Lsec_9_10, 0x3ff73250
        .literal .Lsec_9_14, 0x7fffffff
        .literal .Lsec_9_18, 0x01ffb993
        .literal .Lsec_9_1c, 0x3ff73264
        .literal .Lsec_9_20, 0xc0000005
        .literal .Lsec_9_24, wifi_log
.Lsec_9_28:
        .global hal_agreement_add_rx_ba
        .type   hal_agreement_add_rx_ba, @function
hal_agreement_add_rx_ba:
        entry    a1, 80
        extui    a5, a5, 0, 8
        extui    a6, a6, 0, 16
        extui    a7, a7, 0, 8
        s32i.n   a7, a1, 32
        extui    a2, a2, 0, 8
        extui    a3, a3, 0, 8
        movi.n   a11, 1
        mov.n    a15, a3
        s32i.n   a6, a1, 28
        mov.n    a14, a2
        movi.n   a10, 5
        movi.n   a12, 3
        slli     a11, a11, 13
        s32i.n   a5, a1, 24
        slli     a3, a3, 16
        slli     a7, a7, 13
        extui    a6, a6, 0, 12
        l8ui     a8, a4, 5
        s32i.n   a8, a1, 20
        l8ui     a8, a4, 4
        s32i.n   a8, a1, 16
        l8ui     a8, a4, 3
        s32i.n   a8, a1, 12
        l8ui     a8, a4, 2
        s32i.n   a8, a1, 8
        l8ui     a8, a4, 1
        s32i.n   a8, a1, 4
        l8ui     a8, a4, 0
        l32r     a13, .Lsec_9_0
        s32i.n   a8, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_9_24
        callx8   a8
        l8ui     a9, a4, 1
        slli     a9, a9, 8
        l8ui     a8, a4, 0
        or       a9, a9, a8
        l8ui     a8, a4, 2
        slli     a8, a8, 16
        or       a8, a8, a9
        l8ui     a9, a4, 3
        slli     a9, a9, 24
        or       a9, a9, a8
        slli     a8, a2, 5
        l32r     a10, .Lsec_9_4
        sub      a10, a10, a8
        memw
        s32i.n   a9, a10, 0
        movi.n   a9, 3
        slli     a9, a9, 16
        and      a3, a3, a9
        l8ui     a9, a4, 4
        l8ui     a4, a4, 5
        slli     a4, a4, 8
        or       a4, a4, a9
        or       a3, a3, a4
        l32r     a4, .Lsec_9_8
        sub      a4, a4, a8
        memw
        s32i.n   a3, a4, 0
        movi     a3, 127
        slli     a3, a3, 13
        and      a7, a7, a3
        or       a7, a7, a6
        l32r     a3, .Lsec_9_c
        sub      a3, a3, a8
        memw
        s32i.n   a7, a3, 0
        l32r     a3, .Lsec_9_10
        sub      a3, a3, a8
        memw
        l32i.n   a4, a3, 0
        l32r     a6, .Lsec_9_14
        and      a4, a4, a6
        movi.n   a6, 0
        memw
        s32i.n   a4, a3, 0
        l32r     a4, .Lsec_9_18
        sub      a4, a4, a2
        slli     a4, a4, 5
        l32r     a2, .Lsec_9_1c
        sub      a2, a2, a8
        memw
        s32i.n   a6, a4, 0
        slli     a4, a5, 12
        extui    a4, a4, 0, 16
        memw
        s32i.n   a6, a2, 0
        l32r     a2, .Lsec_9_20
        or       a4, a4, a2
        memw
        s32i.n   a4, a3, 0
        memw
        retw.n
        .size   hal_agreement_add_rx_ba, .-hal_agreement_add_rx_ba
.Lsec_9_12c:

        .section .text.hal_agreement_clr_rx_ba,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_10_0, .Lsec_16_0
        .literal .Lsec_10_4, 0x3ff73250
        .literal .Lsec_10_8, 0x3ff7325c
        .literal .Lsec_10_c, 0x01ffb993
        .literal .Lsec_10_10, 0x3ff73264
        .literal .Lsec_10_14, 0xc0000005
        .literal .Lsec_10_18, wifi_log
.Lsec_10_1c:
        .global hal_agreement_clr_rx_ba
        .type   hal_agreement_clr_rx_ba, @function
hal_agreement_clr_rx_ba:
        entry    a1, 48
        extui    a2, a2, 0, 8
        extui    a3, a3, 0, 8
        extui    a4, a4, 0, 16
        extui    a5, a5, 0, 8
        l32r     a13, .Lsec_10_0
        movi.n   a11, 1
        mov.n    a15, a3
        mov.n    a14, a2
        slli     a11, a11, 13
        s32i.n   a5, a1, 4
        movi.n   a10, 5
        movi     a12, 3
        slli     a5, a5, 13
        slli     a3, a3, 12
        s32i     a4, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_10_18
        callx8   a8
        movi     a11, 127
        slli     a11, a11, 13
        extui    a4, a4, 0, 12
        l32r     a8, .Lsec_10_4
        and      a5, a5, a11
        slli     a10, a2, 5
        or       a5, a5, a4
        movi.n   a9, 0
        l32r     a4, .Lsec_10_8
        sub      a8, a8, a10
        sub      a4, a4, a10
        extui    a3, a3, 0, 16
        memw
        s32i.n   a9, a8, 0
        memw
        s32i.n   a5, a4, 0
        l32r     a5, .Lsec_10_c
        sub      a5, a5, a2
        slli     a5, a5, 5
        l32r     a2, .Lsec_10_10
        sub      a2, a2, a10
        memw
        s32i.n   a9, a5, 0
        memw
        s32i.n   a9, a2, 0
        l32r     a2, .Lsec_10_14
        or       a3, a3, a2
        memw
        s32i.n   a3, a8, 0
        memw
        retw.n
        .size   hal_agreement_clr_rx_ba, .-hal_agreement_clr_rx_ba
.Lsec_10_a6:

        .section .text.hal_agreement_del_rx_ba,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_11_0, 0x3ff73250
        .literal .Lsec_11_4, .Lsec_15_0
        .literal .Lsec_11_8, 0x7fffffff
        .literal .Lsec_11_c, 0x01ffb993
        .literal .Lsec_11_10, 0x3ff73264
        .literal .Lsec_11_14, wifi_log
.Lsec_11_18:
        .global hal_agreement_del_rx_ba
        .type   hal_agreement_del_rx_ba, @function
hal_agreement_del_rx_ba:
        entry    a1, 32
        extui    a4, a2, 0, 8
        l32r     a3, .Lsec_11_0
        slli     a2, a4, 5
        sub      a3, a3, a2
        movi.n   a11, 1
        mov.n    a14, a4
        memw
        l32i.n   a15, a3, 0
        movi.n   a12, 3
        extui    a15, a15, 12, 4
        slli     a11, a11, 13
        movi     a10, 5
        l32r     a13, .Lsec_11_4
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_11_14
        callx8   a8
        memw
        l32i.n   a8, a3, 0
        l32r     a9, .Lsec_11_8
        and      a8, a8, a9
        l32r     a14, .Lsec_11_c
        sub      a14, a14, a4
        slli     a14, a14, 5
        l32r     a9, .Lsec_11_10
        sub      a2, a9, a2
        movi.n   a9, -1
        slli     a9, a9, 31
        memw
        s32i.n   a8, a3, 0
        movi.n   a8, 0
        memw
        s32i.n   a8, a14, 0
        memw
        s32i.n   a8, a2, 0
        memw
        l32i.n   a2, a3, 0
        or       a2, a2, a9
        memw
        s32i.n   a2, a3, 0
        memw
        s32i.n   a8, a3, 0
        memw
        retw.n
        .size   hal_agreement_del_rx_ba, .-hal_agreement_del_rx_ba
.Lsec_11_8b:

        .section .text.hal_ba_session_store,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_12_0, 0x3ff73250
.Lsec_12_4:
        .global hal_ba_session_store
        .type   hal_ba_session_store, @function
hal_ba_session_store:
        entry    a1, 32
        l32r     a8, .Lsec_12_0
        movi.n   a9, 0
        movi.n   a10, 8
        loop     a10, .Lsec_12_3d
        memw
        l32i.n   a11, a8, 0
        bgez     a11, .Lsec_12_38
        add.n    a12, a2, a9
        memw
        l32i.n   a11, a8, 12
        extui    a11, a11, 20, 12
        s32i.n   a11, a12, 0
        add.n    a11, a3, a9
        memw
        l32i.n   a12, a8, 24
        s32i.n   a12, a11, 0
        add.n    a11, a4, a9
        memw
        l32i.n   a12, a8, 28
        s32i     a12, a11, 0
.Lsec_12_38:
        addi     a8, a8, -32
        addi.n   a9, a9, 4
.Lsec_12_3d:
        memw
        retw.n
        .size   hal_ba_session_store, .-hal_ba_session_store
.Lsec_12_42:

        .section .text.hal_ba_session_restore,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_13_0, 0x3ff73250
        .literal .Lsec_13_4, 0x7fffffff
.Lsec_13_8:
        .global hal_ba_session_restore
        .type   hal_ba_session_restore, @function
hal_ba_session_restore:
        entry    a1, 32
        l32r     a8, .Lsec_13_0
        movi.n   a9, 0
        movi.n   a11, 8
        l32r     a13, .Lsec_13_4
        loop     a11, .Lsec_13_72
        memw
        l32i.n   a10, a8, 0
        bgez     a10, .Lsec_13_6d
        movi.n   a14, -1
        slli     a14, a14, 12
        memw
        l32i.n   a10, a8, 0
        and      a10, a10, a13
        memw
        s32i.n   a10, a8, 0
        add.n    a10, a2, a9
        memw
        l32i.n   a12, a8, 12
        and      a12, a12, a14
        l32i.n   a10, a10, 0
        extui    a10, a10, 0, 12
        or       a10, a10, a12
        movi.n   a12, -1
        memw
        s32i.n   a10, a8, 12
        add.n    a10, a3, a9
        slli     a12, a12, 31
        l32i.n   a10, a10, 0
        memw
        s32i.n   a10, a8, 16
        add.n    a10, a4, a9
        l32i.n   a10, a10, 0
        memw
        s32i.n   a10, a8, 20
        memw
        l32i.n   a10, a8, 0
        or       a10, a10, a12
        memw
        s32i.n   a10, a8, 0
.Lsec_13_6d:
        addi     a8, a8, -32
        addi.n   a9, a9, 4
.Lsec_13_72:
        memw
        retw.n
        .size   hal_ba_session_restore, .-hal_ba_session_restore
.Lsec_13_77:

        .section .text.hal_ba_session_restore_by_hw_index,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_14_0, 0x3ff73250
        .literal .Lsec_14_4, 0x7fffffff
        .literal .Lsec_14_8, 0x01ffb993
        .literal .Lsec_14_c, 0x3ff73264
        .literal .Lsec_14_10, 0x3ff7325c
.Lsec_14_14:
        .global hal_ba_session_restore_by_hw_index
        .type   hal_ba_session_restore_by_hw_index, @function
hal_ba_session_restore_by_hw_index:
        entry    a1, 32
        extui    a11, a2, 0, 8
        l32r     a8, .Lsec_14_0
        slli     a10, a11, 5
        sub      a8, a8, a10
        mov.n    a2, a11
        memw
        l32i.n   a9, a8, 0
        bgez     a9, .Lsec_14_95
        addx4    a4, a11, a4
        addx4    a5, a11, a5
        memw
        l32i.n   a9, a8, 0
        addx4    a2, a11, a3
        l32r     a12, .Lsec_14_4
        and      a9, a9, a12
        memw
        s32i.n   a9, a8, 0
        l32r     a9, .Lsec_14_8
        sub      a9, a9, a11
        slli     a9, a9, 5
        movi.n   a11, -1
        l32i.n   a4, a4, 0
        slli     a11, a11, 12
        memw
        s32i.n   a4, a9, 0
        l32r     a9, .Lsec_14_c
        sub      a9, a9, a10
        l32i.n   a4, a5, 0
        memw
        s32i.n   a4, a9, 0
        l32r     a9, .Lsec_14_10
        sub      a9, a9, a10
        memw
        l32i.n   a10, a9, 0
        and      a10, a10, a11
        l32i.n   a2, a2, 0
        extui    a2, a2, 0, 12
        or       a2, a2, a10
        memw
        s32i.n   a2, a9, 0
        movi.n   a9, -1
        slli     a9, a9, 31
        memw
        l32i.n   a2, a8, 0
        or       a2, a2, a9
        memw
        s32i.n   a2, a8, 0
.Lsec_14_95:
        memw
        retw.n
        .size   hal_ba_session_restore_by_hw_index, .-hal_ba_session_restore_by_hw_index
.Lsec_14_9a:

        .section .rodata_wlog_info.4,"a",@progbits
        .align  1
.Lsec_15_0:
        .byte   0x3c, 0x62, 0x61, 0x2d, 0x64, 0x65, 0x6c, 0x3e, 0x69, 0x64, 0x78, 0x3a, 0x25, 0x64, 0x2c, 0x20
        .byte   0x74, 0x69, 0x64, 0x3a, 0x25, 0x64, 0x00

        .section .rodata_wlog_info.3,"a",@progbits
        .align  1
.Lsec_16_0:
        .byte   0x3c, 0x62, 0x61, 0x2d, 0x63, 0x6c, 0x72, 0x3e, 0x69, 0x64, 0x78, 0x3a, 0x25, 0x64, 0x2c, 0x20
        .byte   0x74, 0x69, 0x64, 0x3a, 0x25, 0x64, 0x2c, 0x20, 0x73, 0x73, 0x6e, 0x3a, 0x25, 0x64, 0x2c, 0x20
        .byte   0x77, 0x69, 0x6e, 0x53, 0x69, 0x7a, 0x65, 0x3a, 0x25, 0x64, 0x00

        .section .rodata_wlog_info.2,"a",@progbits
        .align  1
.Lsec_17_0:
        .byte   0x3c, 0x62, 0x61, 0x2d, 0x61, 0x64, 0x64, 0x3e, 0x69, 0x64, 0x78, 0x3a, 0x25, 0x64, 0x20, 0x28
        .byte   0x69, 0x66, 0x78, 0x3a, 0x25, 0x64, 0x2c, 0x20, 0x25, 0x30, 0x32, 0x78, 0x3a, 0x25, 0x30, 0x32
        .byte   0x78, 0x3a, 0x25, 0x30, 0x32, 0x78, 0x3a, 0x25, 0x30, 0x32, 0x78, 0x3a, 0x25, 0x30, 0x32, 0x78
        .byte   0x3a, 0x25, 0x30, 0x32, 0x78, 0x29, 0x2c, 0x20, 0x74, 0x69, 0x64, 0x3a, 0x25, 0x64, 0x2c, 0x20
        .byte   0x73, 0x73, 0x6e, 0x3a, 0x25, 0x64, 0x2c, 0x20, 0x77, 0x69, 0x6e, 0x53, 0x69, 0x7a, 0x65, 0x3a
        .byte   0x25, 0x64, 0x00
