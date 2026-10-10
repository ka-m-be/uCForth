.section .text.esf_buf_alloc_dynamic_default_handler,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_15_0, .Lsec_42_0
        .literal .Lsec_15_4, wifi_log
.Lsec_15_8:
        .global esf_buf_alloc_dynamic_default_handler
        .type   esf_buf_alloc_dynamic_default_handler, @function
esf_buf_alloc_dynamic_default_handler:
        entry    a1, 32
        l32r     a13, .Lsec_15_0
        mov.n    a14, a2
        movi.n   a12, 1
        movi     a11, 0x400
        movi.n   a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_15_4
        callx8   a8
        movi.n   a2, 0
        retw.n
        .size   esf_buf_alloc_dynamic_default_handler, .-esf_buf_alloc_dynamic_default_handler
.Lsec_15_21:

        .section .wifiextrairam.4,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_16_0, g_wifi_global_lock
        .literal .Lsec_16_4, g_osi_funcs_p
        .literal .Lsec_16_8, g_lmac_cnt
        .literal .Lsec_16_c, 0x00000ffb
        .literal .Lsec_16_10, 0xdeadbeef
        .literal .Lsec_16_14, esf_buf_alloc_dynamic_default_handler
        .literal .Lsec_16_18, memset
        .literal .Lsec_16_1c, memset
        .literal .Lsec_16_20, memcpy
.Lsec_16_24:
        .global esf_buf_alloc_dynamic
        .type   esf_buf_alloc_dynamic, @function
esf_buf_alloc_dynamic:
        entry    a1, 48
        l16ui    a5, a2, 16
        l8ui     a6, a2, 8
        l8ui     a7, a2, 12
        beqz     a5, .Lsec_16_94
        l32r     a5, .Lsec_16_4
        l32r     a9, .Lsec_16_0
        l32i.n   a8, a5, 0
        l32i.n   a10, a9, 0
        l32i     a8, a8, 84
        s32i.n   a9, a1, 4
        memw
        callx8   a8
        l16ui    a8, a2, 14
        l16ui    a12, a2, 16
        l32i.n   a10, a5, 0
        add.n    a5, a6, a6
        l32r     a11, .Lsec_16_8
        l32i.n   a9, a1, 4
        bltu     a8, a12, .Lsec_16_76
        add.n    a5, a5, a6
        addx4    a5, a5, a11
        l16ui    a2, a5, 10
        addi.n   a2, a2, 1
        s16i     a2, a5, 10
        memw
        l32i     a2, a10, 88
        l32i.n   a10, a9, 0
        memw
        j        .Lsec_16_1dc
        .byte    0x00
.Lsec_16_76:
        add.n    a5, a5, a6
        addx4    a5, a5, a11
        addi.n   a8, a8, 1
        s16i     a8, a2, 14
        memw
        l32i.n   a8, a5, 0
        addi.n   a8, a8, 1
        s32i.n   a8, a5, 0
        l32i     a5, a10, 88
        l32i.n   a10, a9, 0
        memw
        callx8   a5
.Lsec_16_94:
        bgeui    a6, 5, .Lsec_16_9c
        beqz.n   a6, .Lsec_16_cd
        j        .Lsec_16_aa
.Lsec_16_9c:
        addi     a5, a6, -7
        extui    a5, a5, 0, 8
        bltui    a5, 3, .Lsec_16_b8
        j        .Lsec_16_cd
        .byte    0x00
        .byte    0x00
.Lsec_16_aa:
        l32r     a5, .Lsec_16_4
        addi     a10, a3, 96
        l32i.n   a5, a5, 0
        l32i     a5, a5, 0x15c
        j        .Lsec_16_c3
.Lsec_16_b8:
        l32r     a5, .Lsec_16_4
        addi     a10, a3, 96
        l32i.n   a5, a5, 0
        l32i     a5, a5, 0x16c
.Lsec_16_c3:
        add.n    a10, a10, a7
        callx8   a5
        j        .Lsec_16_d5
        .byte    0x00
        .byte    0x00
.Lsec_16_cd:
        mov.n    a10, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, esf_buf_alloc_dynamic_default_handler
        l32r     a8, .Lsec_16_14
        callx8   a8
.Lsec_16_d5:
        mov.n    a5, a10
        beqz     a10, .Lsec_16_190
        movi.n   a12, 92
        movi.n   a11, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, memset
        l32r     a8, .Lsec_16_18
        callx8   a8
        addi     a8, a5, 60
        addi     a13, a5, 48
        s32i.n   a8, a5, 44
        addi     a8, a5, 92
        mov.n    a12, a7
        movi.n   a11, 0
        mov.n    a10, a8
        s32i.n   a13, a5, 4
        s32i.n   a8, a5, 16
        s32i.n   a13, a1, 0
        s32i.n   a5, a1, 4
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, memset
        l32r     a8, .Lsec_16_1c
        callx8   a8
        mov.n    a8, a10
        movi.n   a10, 1
        extui    a11, a3, 0, 16
        l32i.n   a2, a2, 4
        l32i.n   a13, a1, 0
        s32i.n   a2, a5, 60
        l32i     a2, a5, 64
        s16i     a10, a5, 12
        memw
        movi.n   a10, 15
        or       a2, a2, a10
        add.n    a10, a8, a7
        l32r     a12, .Lsec_16_c
        s32i     a2, a5, 64
        add.n    a2, a7, a3
        s32i.n   a13, a5, 8
        s32i.n   a10, a5, 52
        l32i.n   a9, a1, 4
        bgeu     a12, a2, .Lsec_16_14d
        movi.n   a7, -1
        slli     a7, a7, 12
        memw
        l32i.n   a2, a5, 48
        and      a2, a2, a7
        movi     a7, 0x3ff
        slli     a7, a7, 2
        memw
        j        .Lsec_16_166
.Lsec_16_14d:
        add.n    a8, a8, a2
        l32r     a2, .Lsec_16_10
        add.n    a7, a11, a7
        extui    a7, a7, 0, 12
        s32i.n   a2, a8, 0
        movi.n   a8, -1
        slli     a8, a8, 12
        memw
        l32i.n   a2, a5, 48
        and      a2, a2, a8
.Lsec_16_166:
        or       a2, a2, a7
        memw
        s32i.n   a2, a5, 48
        s16i     a11, a5, 22
        s8i      a6, a5, 26
        memw
        beqz     a4, .Lsec_16_1e4
        mov.n    a12, a3
        mov.n    a11, a4
        s32i.n   a9, a1, 4
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_16_20
        callx8   a8
        l32i.n   a9, a1, 4
        j        .Lsec_16_1e4
        .byte    0x00
        .byte    0x00
.Lsec_16_190:
        l16ui    a3, a2, 16
        movi.n   a9, 0
        beq      a3, a9, .Lsec_16_1e4
        l32r     a7, .Lsec_16_4
        l32r     a4, .Lsec_16_0
        l32i.n   a3, a7, 0
        l32i.n   a10, a4, 0
        l32i     a3, a3, 84
        callx8   a3
        add.n    a3, a6, a6
        l16ui    a5, a2, 14
        l32r     a8, .Lsec_16_8
        beqz.n   a5, .Lsec_16_1c5
        addi.n   a5, a5, -1
        s16i     a5, a2, 14
        memw
        add.n    a2, a3, a6
        addx4    a2, a2, a8
        l32i.n   a5, a2, 0
        addi.n   a5, a5, 1
        s32i.n   a5, a2, 0
.Lsec_16_1c5:
        add.n    a3, a3, a6
        addx4    a3, a3, a8
        l16ui    a2, a3, 8
        addi.n   a2, a2, 1
        s16i     a2, a3, 8
        memw
        l32i.n   a2, a7, 0
        l32i.n   a10, a4, 0
        l32i     a2, a2, 88
.Lsec_16_1dc:
        memw
        callx8   a2
        movi.n   a9, 0
.Lsec_16_1e4:
        mov.n    a2, a9
        retw.n
        .size   esf_buf_alloc_dynamic, .-esf_buf_alloc_dynamic
.Lsec_16_1e8:

        .section .text.esf_buf_alloc_default_handler,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_17_0, .Lsec_41_0
        .literal .Lsec_17_4, wifi_log
.Lsec_17_8:
        .global esf_buf_alloc_default_handler
        .type   esf_buf_alloc_default_handler, @function
esf_buf_alloc_default_handler:
        entry    a1, 32
        l32r     a13, .Lsec_17_0
        mov.n    a14, a2
        movi.n   a12, 1
        movi     a11, 0x400
        movi.n   a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_17_4
        callx8   a8
        movi.n   a2, 0
        retw.n
        .size   esf_buf_alloc_default_handler, .-esf_buf_alloc_default_handler
.Lsec_17_21:

        .section .wifislprxiram.6,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_18_0, .Lsec_19_0
        .literal .Lsec_18_4, g_wifi_menuconfig
        .literal .Lsec_18_8, g_wifi_global_lock
        .literal .Lsec_18_c, g_osi_funcs_p
        .literal .Lsec_18_10, g_eb_list_desc
        .literal .Lsec_18_14, g_lmac_cnt
        .literal .Lsec_18_18, memcpy
        .literal .Lsec_18_1c, esf_buf_alloc_dynamic
        .literal .Lsec_18_20, esf_buf_alloc_default_handler
.Lsec_18_24:
        .global esf_buf_alloc
        .type   esf_buf_alloc, @function
esf_buf_alloc:
        entry    a1, 48
        mov.n    a6, a2
        movi.n   a5, 8
        addi.n   a2, a3, -1
        bgeu     a5, a2, .Lsec_18_33
        j        .Lsec_18_149
.Lsec_18_33:
        l32r     a5, .Lsec_18_0
        addx4    a2, a2, a5
        l32i.n   a2, a2, 0
        jx       a2
.Lsec_18_3e:
        l32r     a2, .Lsec_18_4
        l32i.n   a2, a2, 8
        j        .Lsec_18_4d
        .byte    0x00
.Lsec_18_47:
        l32r     a2, .Lsec_18_4
        l32i     a2, a2, 84
.Lsec_18_4d:
        bnez     a2, .Lsec_18_131
.Lsec_18_50:
        slli     a5, a3, 2
        l32r     a8, .Lsec_18_c
        l32r     a11, .Lsec_18_8
        l32i.n   a2, a8, 0
        l32i     a10, a11, 0
        l32i     a2, a2, 84
        s32i.n   a8, a1, 4
        s32i     a11, a1, 0
        memw
        callx8   a2
        addx4    a9, a3, a3
        l32r     a2, .Lsec_18_10
        addx4    a7, a9, a2
        l32i.n   a8, a1, 4
        l32i.n   a2, a7, 0
        l32i.n   a10, a8, 0
        l32r     a12, .Lsec_18_14
        l32i.n   a11, a1, 0
        beqz.n   a2, .Lsec_18_b5
        l32i.n   a8, a2, 40
        s32i.n   a8, a7, 0
        movi.n   a8, 0
        s32i.n   a8, a2, 40
        addx2    a8, a3, a3
        addx4    a8, a8, a12
        l32i.n   a12, a8, 0
        addi.n   a12, a12, 1
        s32i.n   a12, a8, 0
        l32i     a8, a10, 88
        l32i.n   a10, a11, 0
        memw
        callx8   a8
        extui    a8, a4, 0, 16
        l32i.n   a10, a2, 4
        s16i     a8, a2, 22
        memw
        bnez.n   a10, .Lsec_18_d8
        memw
        j        .Lsec_18_fb
        .byte    0x00
.Lsec_18_b5:
        l8ui     a3, a7, 8
        addx2    a3, a3, a3
        addx4    a3, a3, a12
        l16ui    a4, a3, 10
        addi.n   a4, a4, 1
        s16i     a4, a3, 10
        memw
        l32i     a3, a10, 88
        l32i.n   a10, a11, 0
        memw
        callx8   a3
        j        .Lsec_18_15c
        .byte    0x00
.Lsec_18_d8:
        l8ui     a11, a7, 12
        add.n    a8, a8, a11
        extui    a8, a8, 0, 12
        l32i.n   a9, a2, 16
        add.n    a9, a9, a11
        movi.n   a11, -1
        slli     a11, a11, 12
        s32i.n   a9, a10, 4
        memw
        l32i.n   a9, a10, 0
        and      a9, a9, a11
        or       a8, a9, a8
        memw
        s32i.n   a8, a10, 0
.Lsec_18_fb:
        beqz.n   a6, .Lsec_18_10c
        mov.n    a12, a4
        mov.n    a11, a6
        l32i.n   a10, a10, 4
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_18_18
        callx8   a8
.Lsec_18_10c:
        add.n    a5, a5, a3
        l32r     a3, .Lsec_18_10
        addx4    a5, a5, a3
        l32i.n   a4, a2, 44
        l32i.n   a5, a5, 4
        l32i.n   a3, a4, 0
        or       a3, a3, a5
        movi.n   a5, 15
        s32i.n   a3, a4, 0
        l32i.n   a3, a4, 4
        or       a3, a3, a5
        s32i.n   a3, a4, 4
        memw
        j        .Lsec_18_15c
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_18_131:
        l32r     a10, .Lsec_18_10
        addx4    a3, a3, a3
        mov.n    a12, a6
        mov.n    a11, a4
        addx4    a10, a3, a10
        .reloc  ., R_XTENSA_ASM_EXPAND, esf_buf_alloc_dynamic
        l32r     a8, .Lsec_18_1c
        callx8   a8
        mov.n    a2, a10
        j        .Lsec_18_15c
.Lsec_18_149:
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esf_buf_alloc_default_handler
        l32r     a8, .Lsec_18_20
        callx8   a8
        beqi     a10, 1, .Lsec_18_131
        bnei     a10, 2, .Lsec_18_15a
        j        .Lsec_18_50
.Lsec_18_15a:
        movi.n   a2, 0
.Lsec_18_15c:
        retw.n
        .size   esf_buf_alloc, .-esf_buf_alloc
.Lsec_18_15e:

        .section .rodata,"a",@progbits
        .align  4
.Lsec_19_0:
        .word   .Lsec_18_3e
        .word   .Lsec_18_131
        .word   .Lsec_18_131
        .word   .Lsec_18_131
        .word   .Lsec_18_50
        .word   .Lsec_18_50
        .word   .Lsec_18_131
        .word   .Lsec_18_47
        .word   .Lsec_18_50
.Lsec_19_24:
        .word   .Lsec_21_41
        .word   .Lsec_21_a4
        .word   .Lsec_21_a4
        .word   .Lsec_21_a4
        .word   .Lsec_21_52
        .word   .Lsec_21_52
        .word   .Lsec_21_a4
        .word   .Lsec_21_49
        .word   .Lsec_21_52

        .section .text.esf_buf_recycle_default_handler,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_20_0, .Lsec_40_0
        .literal .Lsec_20_4, wifi_log
.Lsec_20_8:
        .global esf_buf_recycle_default_handler
        .type   esf_buf_recycle_default_handler, @function
esf_buf_recycle_default_handler:
        entry    a1, 32
        l32r     a13, .Lsec_20_0
        extui    a14, a2, 0, 8
        movi.n   a12, 1
        movi     a11, 0x400
        movi.n   a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_20_4
        callx8   a8
        movi.n   a2, 0
        retw.n
        .size   esf_buf_recycle_default_handler, .-esf_buf_recycle_default_handler
.Lsec_20_22:

        .section .wifirxiram.8,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_21_0, .Lsec_19_24
        .literal .Lsec_21_4, g_wifi_menuconfig
        .literal .Lsec_21_8, g_wifi_global_lock
        .literal .Lsec_21_c, g_osi_funcs_p
        .literal .Lsec_21_10, g_eb_list_desc
        .literal .Lsec_21_14, g_lmac_cnt
        .literal .Lsec_21_18, memset
        .literal .Lsec_21_1c, esf_buf_recycle_default_handler
.Lsec_21_20:
        .global esf_buf_recycle
        .type   esf_buf_recycle, @function
esf_buf_recycle:
        entry    a1, 32
        beqz     a2, .Lsec_21_10c
        l8ui     a3, a2, 26
        movi.n   a4, 8
        addi.n   a8, a3, -1
        extui    a8, a8, 0, 8
        bgeu     a4, a8, .Lsec_21_36
        j        .Lsec_21_fb
.Lsec_21_36:
        l32r     a4, .Lsec_21_0
        addx4    a8, a8, a4
        l32i.n   a4, a8, 0
        jx       a4
.Lsec_21_41:
        l32r     a4, .Lsec_21_4
        l32i.n   a4, a4, 8
        j        .Lsec_21_4f
.Lsec_21_49:
        l32r     a4, .Lsec_21_4
        l32i     a4, a4, 84
.Lsec_21_4f:
        bnez     a4, .Lsec_21_a4
.Lsec_21_52:
        movi.n   a12, 32
        movi.n   a11, 0
        movi.n   a4, 0
        l32i.n   a10, a2, 44
        .reloc  ., R_XTENSA_ASM_EXPAND, memset
        l32r     a8, .Lsec_21_18
        callx8   a8
        l32r     a6, .Lsec_21_c
        l32r     a5, .Lsec_21_8
        s16i     a4, a2, 20
        memw
        l32i.n   a4, a6, 0
        l32i.n   a10, a5, 0
        l32i     a4, a4, 84
        memw
        callx8   a4
        addx4    a4, a3, a3
        addx2    a3, a3, a3
        l32r     a8, .Lsec_21_10
        addx4    a4, a4, a8
        l32i.n   a8, a4, 0
        s32i.n   a8, a2, 40
        s32i.n   a2, a4, 0
        l32r     a2, .Lsec_21_14
        addx4    a3, a3, a2
        l32i.n   a2, a3, 4
        addi.n   a2, a2, 1
        s32i.n   a2, a3, 4
        l32i.n   a2, a6, 0
        l32i.n   a10, a5, 0
        l32i     a2, a2, 88
        memw
        j        .Lsec_21_f2
.Lsec_21_a4:
        l32r     a5, .Lsec_21_c
        mov.n    a10, a2
        addx4    a2, a3, a3
        l32i.n   a4, a5, 0
        l32i     a4, a4, 176
        callx8   a4
        l32r     a4, .Lsec_21_10
        addx4    a2, a2, a4
        l16ui    a4, a2, 16
        beqz     a4, .Lsec_21_10c
        l32r     a4, .Lsec_21_8
        l32i.n   a6, a5, 0
        l32i.n   a10, a4, 0
        l32i     a6, a6, 84
        callx8   a6
        l16ui    a8, a2, 14
        beqz     a8, .Lsec_21_dc
        addi     a8, a8, -1
        s16i     a8, a2, 14
        memw
.Lsec_21_dc:
        addx2    a3, a3, a3
        l32r     a2, .Lsec_21_14
        addx4    a3, a3, a2
        l32i.n   a2, a3, 4
        addi.n   a2, a2, 1
        s32i.n   a2, a3, 4
        l32i.n   a2, a5, 0
        l32i.n   a10, a4, 0
        l32i     a2, a2, 88
.Lsec_21_f2:
        memw
        callx8   a2
        j        .Lsec_21_10c
.Lsec_21_fb:
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esf_buf_recycle_default_handler
        l32r     a8, .Lsec_21_1c
        callx8   a8
        beqi     a10, 1, .Lsec_21_a4
        bnei     a10, 2, .Lsec_21_10c
        j        .Lsec_21_52
.Lsec_21_10c:
        retw.n
        .size   esf_buf_recycle, .-esf_buf_recycle
.Lsec_21_10e:

        .section .text.esf_buf_setup_for_mesh,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_22_0, g_eb_list_desc
.Lsec_22_4:
        .global esf_buf_setup_for_mesh
        .type   esf_buf_setup_for_mesh, @function
esf_buf_setup_for_mesh:
        entry    a1, 32
        l32r     a8, .Lsec_22_0
        movi.n   a9, 64
        movi     a10, 128
        movi.n   a2, 0
        s16i     a9, a8, 96
        s16i     a10, a8, 76
        s16i     a9, a8, 56
        memw
        memw
        retw.n
        .size   esf_buf_setup_for_mesh, .-esf_buf_setup_for_mesh
.Lsec_22_22:

        .section .text.esf_buf_setup_static,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_23_0, g_eb_list_desc
        .literal .Lsec_23_4, .Lsec_39_0
        .literal .Lsec_23_8, esf_buf_alloc_dynamic
        .literal .Lsec_23_c, wifi_log
        .literal .Lsec_23_10, esf_buf_recycle
.Lsec_23_14:
        .global esf_buf_setup_static
        .type   esf_buf_setup_static, @function
esf_buf_setup_static:
        entry    a1, 32
        l32r     a6, .Lsec_23_0
        addx4    a5, a2, a2
        addx4    a5, a5, a6
        movi.n   a6, 0
        s16i     a3, a5, 10
        memw
        memw
        j        .Lsec_23_5e
.Lsec_23_2e:
        mov.n    a12, a6
        mov.n    a11, a4
        mov.n    a10, a5
        addi.n   a3, a3, -1
        .reloc  ., R_XTENSA_ASM_EXPAND, esf_buf_alloc_dynamic
        l32r     a8, .Lsec_23_8
        callx8   a8
        bnez.n   a10, .Lsec_23_58
        l32r     a13, .Lsec_23_4
        mov.n    a14, a2
        movi.n   a12, 2
        movi     a11, 0x400
        movi.n   a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_23_c
        callx8   a8
        movi     a3, 0x101
        j        .Lsec_23_61
        .byte    0x00
        .byte    0x00
.Lsec_23_58:
        .reloc  ., R_XTENSA_ASM_EXPAND, esf_buf_recycle
        l32r     a8, .Lsec_23_10
        callx8   a8
.Lsec_23_5e:
        bnez     a3, .Lsec_23_2e
.Lsec_23_61:
        mov.n    a2, a3
        retw.n
        .size   esf_buf_setup_static, .-esf_buf_setup_static
.Lsec_23_65:

        .section .text.esf_buf_free_static,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_24_0, g_eb_list_desc
        .literal .Lsec_24_4, g_wifi_global_lock
        .literal .Lsec_24_8, g_osi_funcs_p
        .literal .Lsec_24_c, memset
        .literal .Lsec_24_10, memset
.Lsec_24_14:
        .global esf_buf_free_static
        .type   esf_buf_free_static, @function
esf_buf_free_static:
        entry    a1, 32
        movi.n   a3, 9
        bge      a3, a2, .Lsec_24_1f
        j        .Lsec_24_a4
.Lsec_24_1f:
        l32r     a4, .Lsec_24_0
        addx4    a3, a2, a2
        slli     a5, a3, 2
        movi.n   a6, 0
        addx4    a3, a3, a4
        s16i     a6, a3, 14
        memw
.Lsec_24_33:
        l32r     a6, .Lsec_24_8
        l32r     a4, .Lsec_24_4
        l32i     a3, a6, 0
        l32i     a10, a4, 0
        l32i     a3, a3, 84
        memw
        callx8   a3
        l32r     a3, .Lsec_24_0
        add.n    a7, a3, a5
        l32i.n   a3, a7, 0
        l32i.n   a9, a6, 0
        bnez.n   a3, .Lsec_24_61
        l32i     a2, a9, 88
        l32i     a10, a4, 0
        callx8   a2
        j        .Lsec_24_a4
        .byte    0x00
        .byte    0x00
.Lsec_24_61:
        l32i.n   a10, a3, 40
        l32i     a9, a9, 88
        s32i.n   a10, a7, 0
        movi.n   a7, 0
        l32i.n   a10, a4, 0
        s32i     a7, a3, 40
        memw
        callx8   a9
        bnei     a2, 5, .Lsec_24_96
        mov.n    a11, a7
        movi.n   a12, 32
        l32i.n   a10, a3, 44
        .reloc  ., R_XTENSA_ASM_EXPAND, memset
        l32r     a8, .Lsec_24_c
        callx8   a8
        movi.n   a12, 48
        mov.n    a11, a7
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, memset
        l32r     a8, .Lsec_24_10
        callx8   a8
        j        .Lsec_24_33
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_24_96:
        l32i.n   a6, a6, 0
        mov.n    a10, a3
        l32i     a6, a6, 176
        callx8   a6
        j        .Lsec_24_33
        .byte    0x00
.Lsec_24_a4:
        retw.n
        .size   esf_buf_free_static, .-esf_buf_free_static
.Lsec_24_a6:

        .section .text.esf_buf_setdown,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_25_0, esf_buf_free_static
.Lsec_25_4:
        .global esf_buf_setdown
        .type   esf_buf_setdown, @function
esf_buf_setdown:
        entry    a1, 32
        movi     a2, 1
.Lsec_25_a:
        mov.n    a10, a2
        addi.n   a2, a2, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, esf_buf_free_static
        l32r     a8, .Lsec_25_0
        callx8   a8
        bnei     a2, 10, .Lsec_25_a
        retw.n
        .size   esf_buf_setdown, .-esf_buf_setdown
.Lsec_25_19:

        .section .text.esf_buf_setup,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_26_0, eb_space
        .literal .Lsec_26_4, eb_txdesc_space
        .literal .Lsec_26_8, g_wifi_menuconfig
        .literal .Lsec_26_c, g_eb_list_desc
        .literal .Lsec_26_10, .Lsec_38_0
        .literal .Lsec_26_14, .Lsec_37_0
        .literal .Lsec_26_18, .Lsec_36_0
        .literal .Lsec_26_1c, .Lsec_35_0
        .literal .Lsec_26_20, .Lsec_34_0
        .literal .Lsec_26_24, .Lsec_33_0
        .literal .Lsec_26_28, .Lsec_32_0
        .literal .Lsec_26_2c, memset
        .literal .Lsec_26_30, memset
        .literal .Lsec_26_34, wifi_log
        .literal .Lsec_26_38, esf_buf_setup_static
        .literal .Lsec_26_3c, wifi_log
        .literal .Lsec_26_40, wifi_log
        .literal .Lsec_26_44, esf_buf_setup_static
        .literal .Lsec_26_48, wifi_log
        .literal .Lsec_26_4c, config_is_cache_tx_buf_enabled
        .literal .Lsec_26_50, esf_buf_setup_static
        .literal .Lsec_26_54, wifi_log
        .literal .Lsec_26_58, esf_buf_recycle
        .literal .Lsec_26_5c, esf_buf_setdown
.Lsec_26_60:
        .global esf_buf_setup
        .type   esf_buf_setup, @function
esf_buf_setup:
        entry    a1, 48
        l32r     a6, .Lsec_26_4
        movi     a12, 128
        movi.n   a11, 0
        or       a10, a6, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, memset
        l32r     a8, .Lsec_26_2c
        callx8   a8
        l32r     a5, .Lsec_26_0
        movi     a12, 192
        movi.n   a11, 0
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, memset
        l32r     a8, .Lsec_26_30
        callx8   a8
        l32r     a3, .Lsec_26_8
        l16ui    a14, a3, 4
        bnez.n   a14, .Lsec_26_8e
        movi.n   a14, 64
.Lsec_26_8e:
        movi.n   a11, 1
        l32r     a4, .Lsec_26_c
        movi.n   a12, 3
        movi.n   a10, 6
        l32r     a13, .Lsec_26_10
        s16i     a14, a4, 156
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_26_34
        callx8   a8
        l32i     a8, a3, 84
        l16ui    a11, a3, 88
        bnez.n   a8, .Lsec_26_d5
        bnez.n   a11, .Lsec_26_b5
        movi.n   a11, 10
.Lsec_26_b5:
        movi     a12, 0x1f4
        s16i     a11, a4, 176
        memw
        movi.n   a10, 8
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, esf_buf_setup_static
        l32r     a8, .Lsec_26_38
        callx8   a8
        bnez     a10, .Lsec_26_1c1
        l16ui    a14, a4, 176
        l32r     a13, .Lsec_26_14
        j        .Lsec_26_e6
.Lsec_26_d5:
        mov.n    a14, a11
        bnez     a11, .Lsec_26_dd
        movi     a14, 10
.Lsec_26_dd:
        l32r     a13, .Lsec_26_18
        s16i     a14, a4, 176
        memw
.Lsec_26_e6:
        movi.n   a12, 3
        movi.n   a11, 1
        movi.n   a10, 6
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_26_3c
        callx8   a8
        l16ui    a14, a3, 60
        bnez.n   a14, .Lsec_26_fd
        movi     a14, 32
.Lsec_26_fd:
        movi.n   a12, 3
        l32r     a13, .Lsec_26_1c
        movi.n   a11, 1
        movi.n   a10, 6
        s16i     a14, a4, 76
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_26_40
        callx8   a8
        l32i.n   a8, a3, 8
        bnez.n   a8, .Lsec_26_149
        l16ui    a8, a3, 12
        bnez.n   a8, .Lsec_26_120
        movi.n   a8, 32
.Lsec_26_120:
        movi     a12, 0x640
        s16i     a8, a4, 36
        memw
        mov.n    a11, a2
        movi.n   a10, 1
        l8ui     a8, a4, 32
        sub      a12, a12, a8
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, esf_buf_setup_static
        l32r     a8, .Lsec_26_44
        callx8   a8
        mov.n    a14, a2
        l32r     a13, .Lsec_26_20
        beqz.n   a10, .Lsec_26_159
        j        .Lsec_26_1c1
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_26_149:
        l16ui    a14, a3, 16
        bnez.n   a14, .Lsec_26_150
        movi.n   a14, 32
.Lsec_26_150:
        l32r     a13, .Lsec_26_24
        s16i     a14, a4, 36
        memw
.Lsec_26_159:
        movi.n   a12, 3
        movi.n   a11, 1
        movi.n   a10, 6
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_26_48
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, config_is_cache_tx_buf_enabled
        l32r     a8, .Lsec_26_4c
        callx8   a8
        beqz.n   a10, .Lsec_26_198
        l8ui     a2, a4, 192
        movi     a12, 0x640
        sub      a12, a12, a2
        movi.n   a10, 9
        l32i     a11, a3, 20
        .reloc  ., R_XTENSA_ASM_EXPAND, esf_buf_setup_static
        l32r     a8, .Lsec_26_50
        callx8   a8
        bnez.n   a10, .Lsec_26_1c1
        movi.n   a12, 3
        l32i.n   a14, a3, 20
        movi.n   a11, 1
        movi     a10, 6
        l32r     a13, .Lsec_26_28
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_26_54
        callx8   a8
.Lsec_26_198:
        movi.n   a2, 0
        movi.n   a3, 5
.Lsec_26_19c:
        add.n    a4, a6, a2
        s32i.n   a4, a5, 44
        mov.n    a10, a5
        addi     a2, a2, 32
        s8i      a3, a5, 26
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, esf_buf_recycle
        l32r     a8, .Lsec_26_58
        callx8   a8
        addi     a5, a5, 48
        bnei     a2, 128, .Lsec_26_19c
        movi.n   a10, 0
        j        .Lsec_26_1ce
        .byte    0x00
        .byte    0x00
.Lsec_26_1c1:
        s32i.n   a10, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, esf_buf_setdown
        l32r     a8, .Lsec_26_5c
        callx8   a8
        l32i.n   a10, a1, 0
.Lsec_26_1ce:
        mov.n    a2, a10
        retw.n
        .size   esf_buf_setup, .-esf_buf_setup
.Lsec_26_1d2:

        .section .rodata.esf_buf_statis_dump.str1.1,"aMS",@progbits,1
        .align  1
.Lsec_27_0:
        .ascii  "max\000"
.Lsec_27_4:
        .ascii  "t\000"
.Lsec_27_6:
        .ascii  "fail_oom\000"
.Lsec_27_f:
        .ascii  "fail_fc\000"
.Lsec_27_17:
        .ascii  "total_free\000"
.Lsec_27_22:
        .ascii  "total_alloc\000"
.Lsec_27_2e:
        .ascii  "alloc\000"

        .section .text.esf_buf_statis_dump,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_28_0, .Lsec_31_0
        .literal .Lsec_28_4, .Lsec_27_0
        .literal .Lsec_28_8, .Lsec_27_4
        .literal .Lsec_28_c, .Lsec_30_0
        .literal .Lsec_28_10, .Lsec_27_6
        .literal .Lsec_28_14, .Lsec_27_f
        .literal .Lsec_28_18, .Lsec_27_17
        .literal .Lsec_28_1c, .Lsec_27_22
        .literal .Lsec_28_20, .Lsec_27_2e
        .literal .Lsec_28_24, g_eb_list_desc
        .literal .Lsec_28_28, g_lmac_cnt
        .literal .Lsec_28_2c, .Lsec_29_0
        .literal .Lsec_28_30, wifi_log
        .literal .Lsec_28_34, wifi_log
        .literal .Lsec_28_38, wifi_log
.Lsec_28_3c:
        .global esf_buf_statis_dump
        .type   esf_buf_statis_dump, @function
esf_buf_statis_dump:
        entry    a1, 64
        l32r     a13, .Lsec_28_0
        movi     a12, 3
        movi     a11, 0x400
        movi.n   a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_28_30
        callx8   a8
        l32r     a2, .Lsec_28_10
        movi.n   a12, 3
        movi     a11, 0x400
        movi.n   a10, 1
        s32i.n   a2, a1, 16
        l32r     a2, .Lsec_28_14
        s32i.n   a2, a1, 12
        l32r     a2, .Lsec_28_18
        s32i     a2, a1, 8
        l32r     a2, .Lsec_28_1c
        s32i.n   a2, a1, 4
        l32r     a2, .Lsec_28_20
        l32r     a15, .Lsec_28_4
        l32r     a14, .Lsec_28_8
        l32r     a13, .Lsec_28_c
        s32i     a2, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_28_34
        callx8   a8
        l32r     a2, .Lsec_28_28
        addi     a4, a2, 108
        l32r     a3, .Lsec_28_24
.Lsec_28_8d:
        movi.n   a12, 3
        movi     a11, 0x400
        movi.n   a10, 1
        addi.n   a2, a2, 12
        l16ui    a8, a2, 10
        addi     a3, a3, 20
        l16ui    a15, a3, 16
        l8ui     a14, a3, 8
        s32i.n   a8, a1, 16
        l16ui    a8, a2, 8
        s32i.n   a8, a1, 12
        l32i.n   a8, a2, 4
        s32i.n   a8, a1, 8
        l32i.n   a8, a2, 0
        s32i     a8, a1, 4
        l16ui    a8, a3, 14
        l32r     a13, .Lsec_28_2c
        s32i     a8, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_28_38
        callx8   a8
        bne      a2, a4, .Lsec_28_8d
        retw.n
        .size   esf_buf_statis_dump, .-esf_buf_statis_dump
.Lsec_28_c9:

        .section .rodata_wlog_info.19,"a",@progbits
        .align  1
.Lsec_29_0:
        .byte   0x20, 0x25, 0x2d, 0x32, 0x64, 0x20, 0x25, 0x2d, 0x33, 0x75, 0x20, 0x25, 0x2d, 0x35, 0x75, 0x20
        .byte   0x25, 0x2d, 0x31, 0x31, 0x75, 0x20, 0x25, 0x2d, 0x31, 0x30, 0x75, 0x20, 0x25, 0x2d, 0x38, 0x75
        .byte   0x20, 0x25, 0x2d, 0x38, 0x75, 0x00

        .section .rodata_wlog_info.18,"a",@progbits
        .align  1
.Lsec_30_0:
        .byte   0x20, 0x25, 0x2d, 0x32, 0x73, 0x20, 0x25, 0x2d, 0x33, 0x73, 0x20, 0x25, 0x2d, 0x35, 0x73, 0x20
        .byte   0x25, 0x2d, 0x31, 0x31, 0x73, 0x20, 0x25, 0x2d, 0x31, 0x30, 0x73, 0x20, 0x25, 0x2d, 0x38, 0x73
        .byte   0x20, 0x25, 0x2d, 0x38, 0x73, 0x00

        .section .rodata_wlog_info.17,"a",@progbits
        .align  1
.Lsec_31_0:
        .byte   0x20, 0x42, 0x55, 0x46, 0x46, 0x45, 0x52, 0x3a, 0x00

        .section .rodata_wlog_info.16,"a",@progbits
        .align  1
.Lsec_32_0:
        .byte   0x49, 0x6e, 0x69, 0x74, 0x20, 0x74, 0x78, 0x20, 0x63, 0x61, 0x63, 0x68, 0x65, 0x20, 0x62, 0x75
        .byte   0x66, 0x66, 0x65, 0x72, 0x20, 0x6e, 0x75, 0x6d, 0x3a, 0x20, 0x25, 0x64, 0x00

        .section .rodata_wlog_info.15,"a",@progbits
        .align  1
.Lsec_33_0:
        .byte   0x49, 0x6e, 0x69, 0x74, 0x20, 0x64, 0x79, 0x6e, 0x61, 0x6d, 0x69, 0x63, 0x20, 0x74, 0x78, 0x20
        .byte   0x62, 0x75, 0x66, 0x66, 0x65, 0x72, 0x20, 0x6e, 0x75, 0x6d, 0x3a, 0x20, 0x25, 0x64, 0x00

        .section .rodata_wlog_info.14,"a",@progbits
        .align  1
.Lsec_34_0:
        .byte   0x49, 0x6e, 0x69, 0x74, 0x20, 0x73, 0x74, 0x61, 0x74, 0x69, 0x63, 0x20, 0x74, 0x78, 0x20, 0x62
        .byte   0x75, 0x66, 0x66, 0x65, 0x72, 0x20, 0x6e, 0x75, 0x6d, 0x3a, 0x20, 0x25, 0x64, 0x00

        .section .rodata_wlog_info.13,"a",@progbits
        .align  1
.Lsec_35_0:
        .byte   0x49, 0x6e, 0x69, 0x74, 0x20, 0x6d, 0x61, 0x6e, 0x61, 0x67, 0x65, 0x6d, 0x65, 0x6e, 0x74, 0x20
        .byte   0x73, 0x68, 0x6f, 0x72, 0x74, 0x20, 0x62, 0x75, 0x66, 0x66, 0x65, 0x72, 0x20, 0x6e, 0x75, 0x6d
        .byte   0x3a, 0x20, 0x25, 0x64, 0x00

        .section .rodata_wlog_info.12,"a",@progbits
        .align  1
.Lsec_36_0:
        .byte   0x49, 0x6e, 0x69, 0x74, 0x20, 0x64, 0x79, 0x6e, 0x61, 0x6d, 0x69, 0x63, 0x20, 0x72, 0x78, 0x20
        .byte   0x6d, 0x67, 0x6d, 0x74, 0x20, 0x62, 0x75, 0x66, 0x66, 0x65, 0x72, 0x20, 0x6e, 0x75, 0x6d, 0x3a
        .byte   0x20, 0x25, 0x64, 0x00

        .section .rodata_wlog_info.11,"a",@progbits
        .align  1
.Lsec_37_0:
        .byte   0x49, 0x6e, 0x69, 0x74, 0x20, 0x73, 0x74, 0x61, 0x74, 0x69, 0x63, 0x20, 0x72, 0x78, 0x20, 0x6d
        .byte   0x67, 0x6d, 0x74, 0x20, 0x62, 0x75, 0x66, 0x66, 0x65, 0x72, 0x20, 0x6e, 0x75, 0x6d, 0x3a, 0x20
        .byte   0x25, 0x64, 0x00

        .section .rodata_wlog_info.10,"a",@progbits
        .align  1
.Lsec_38_0:
        .byte   0x49, 0x6e, 0x69, 0x74, 0x20, 0x64, 0x61, 0x74, 0x61, 0x20, 0x66, 0x72, 0x61, 0x6d, 0x65, 0x20
        .byte   0x64, 0x79, 0x6e, 0x61, 0x6d, 0x69, 0x63, 0x20, 0x72, 0x78, 0x20, 0x62, 0x75, 0x66, 0x66, 0x65
        .byte   0x72, 0x20, 0x6e, 0x75, 0x6d, 0x3a, 0x20, 0x25, 0x64, 0x00

        .section .rodata_wlog_warning.9,"a",@progbits
        .align  1
.Lsec_39_0:
        .byte   0x65, 0x73, 0x66, 0x5f, 0x62, 0x75, 0x66, 0x5f, 0x73, 0x65, 0x74, 0x75, 0x70, 0x5f, 0x73, 0x74
        .byte   0x61, 0x74, 0x69, 0x63, 0x3a, 0x20, 0x61, 0x6c, 0x6c, 0x6f, 0x63, 0x20, 0x65, 0x62, 0x20, 0x66
        .byte   0x61, 0x69, 0x6c, 0x28, 0x25, 0x64, 0x29, 0x00

        .section .rodata_wlog_error.7,"a",@progbits
        .align  1
.Lsec_40_0:
        .byte   0x65, 0x62, 0x75, 0x66, 0x5f, 0x66, 0x72, 0x65, 0x65, 0x3a, 0x20, 0x69, 0x6e, 0x76, 0x61, 0x6c
        .byte   0x69, 0x64, 0x20, 0x74, 0x79, 0x70, 0x65, 0x3d, 0x25, 0x64, 0x00

        .section .rodata_wlog_error.5,"a",@progbits
        .align  1
.Lsec_41_0:
        .byte   0x65, 0x73, 0x66, 0x5f, 0x62, 0x75, 0x66, 0x5f, 0x61, 0x6c, 0x6c, 0x6f, 0x63, 0x3a, 0x20, 0x69
        .byte   0x6e, 0x76, 0x61, 0x6c, 0x69, 0x64, 0x20, 0x74, 0x79, 0x70, 0x65, 0x3d, 0x25, 0x64, 0x00

        .section .rodata_wlog_error.3,"a",@progbits
        .align  1
.Lsec_42_0:
        .byte   0x65, 0x73, 0x66, 0x5f, 0x62, 0x75, 0x66, 0x5f, 0x61, 0x6c, 0x6c, 0x6f, 0x63, 0x5f, 0x64, 0x79
        .byte   0x6e, 0x61, 0x6d, 0x69, 0x63, 0x3a, 0x20, 0x69, 0x6e, 0x76, 0x61, 0x6c, 0x69, 0x64, 0x20, 0x74
        .byte   0x79, 0x70, 0x65, 0x3d, 0x25, 0x64, 0x00

        .section .data.g_eb_list_desc,"aw",@progbits
        .align  4
.Lsec_43_0:
        .global g_eb_list_desc
        .type   g_eb_list_desc, @object
        .size   g_eb_list_desc, 200
g_eb_list_desc:
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x20, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00
        .byte   0x24, 0x00, 0x00, 0x00, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x20, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x20, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x20, 0x00, 0x05, 0x00, 0x04, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x0a, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x20, 0x00, 0x00, 0x09, 0x00, 0x00, 0x00
        .byte   0x24, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00

        .section .bss.eb_space,"aw",@nobits
        .align  4
.Lsec_44_0:
        .global eb_space
        .type   eb_space, @object
        .size   eb_space, 192
eb_space:
        .skip   192

        .section .dram1.2,"aw",@progbits
        .align  4
.Lsec_45_0:
        .global eb_txdesc_space
        .type   eb_txdesc_space, @object
        .size   eb_txdesc_space, 128
eb_txdesc_space:
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
