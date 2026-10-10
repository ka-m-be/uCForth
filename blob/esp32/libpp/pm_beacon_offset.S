.section .wifislpiram.2,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_26_0, 0x00000000
        .literal .Lsec_26_4, g_pm
        .literal .Lsec_26_8, 0x447a0000
        .literal .Lsec_26_c, 0xd2f1a9fc
        .literal .Lsec_26_10, 0x3ff0624d
        .literal .Lsec_26_14, s_pm_beacon_offset
        .literal .Lsec_26_18, 0x00000000
        .literal .Lsec_26_1c, 0x3fe00000
        .literal .Lsec_26_20, __divsf3
        .literal .Lsec_26_24, __extendsfdf2
        .literal .Lsec_26_28, __muldf3
        .literal .Lsec_26_2c, __truncdfsf2
        .literal .Lsec_26_30, __floatsidf
        .literal .Lsec_26_34, __adddf3
        .literal .Lsec_26_38, __muldf3
        .literal .Lsec_26_3c, __extendsfdf2
        .literal .Lsec_26_40, __adddf3
        .literal .Lsec_26_44, __truncdfsf2
        .literal .Lsec_26_48, __divsf3
        .literal .Lsec_26_4c, __extendsfdf2
        .literal .Lsec_26_50, __floatsidf
        .literal .Lsec_26_54, __muldf3
        .literal .Lsec_26_58, __subdf3
        .literal .Lsec_26_5c, __truncdfsf2
.Lsec_26_60:
        .global pm_beacon_offset_get_average
        .type   pm_beacon_offset_get_average, @function
pm_beacon_offset_get_average:
        entry    a1, 64
        l32r     a7, .Lsec_26_4
        extui    a6, a3, 0, 8
        extui    a2, a2, 0, 8
        l32i     a3, a7, 0x1d0
        ufloat.s f0, a3, 0
        rfr      a10, f0
        l32r     a11, .Lsec_26_8
        .reloc  ., R_XTENSA_ASM_EXPAND, __divsf3
        l32r     a8, .Lsec_26_20
        callx8   a8
        mov.n    a3, a10
        l32i.n   a7, a7, 44
        srli     a7, a7, 10
        float.s  f1, a7, 0
        rfr      a10, f1
        .reloc  ., R_XTENSA_ASM_EXPAND, __extendsfdf2
        l32r     a8, .Lsec_26_24
        callx8   a8
        l32r     a12, .Lsec_26_c
        l32r     a13, .Lsec_26_10
        .reloc  ., R_XTENSA_ASM_EXPAND, __muldf3
        l32r     a8, .Lsec_26_28
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, __truncdfsf2
        l32r     a8, .Lsec_26_2c
        callx8   a8
        l32r     a8, .Lsec_26_0
        wfr      f0, a8
        l32r     a7, .Lsec_26_14
        s32i.n   a10, a1, 0
        l32i.n   a15, a7, 4
        mov.n    a7, a2
        memw
        j        .Lsec_26_13c
        .byte    0x00
.Lsec_26_b9:
        addx2    a9, a7, a15
        l16ui    a14, a9, 0
        beqz     a14, .Lsec_26_13a
        add.n    a8, a14, a8
        mov.n    a10, a7
        s16i     a8, a4, 0
        memw
        s32i.n   a14, a1, 20
        s32i     a15, a1, 12
        ssi      f0, a1, 16
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, __floatsidf
        l32r     a8, .Lsec_26_30
        callx8   a8
        l32r     a12, .Lsec_26_18
        l32r     a13, .Lsec_26_1c
        .reloc  ., R_XTENSA_ASM_EXPAND, __adddf3
        l32r     a8, .Lsec_26_34
        callx8   a8
        l32r     a12, .Lsec_26_c
        l32r     a13, .Lsec_26_10
        .reloc  ., R_XTENSA_ASM_EXPAND, __muldf3
        l32r     a8, .Lsec_26_38
        callx8   a8
        mov.n    a8, a10
        mov.n    a10, a3
        s32i.n   a8, a1, 4
        s32i.n   a11, a1, 8
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, __extendsfdf2
        l32r     a8, .Lsec_26_3c
        callx8   a8
        mov.n    a12, a10
        mov.n    a13, a11
        l32i.n   a8, a1, 4
        mov.n    a10, a8
        l32i.n   a9, a1, 8
        mov.n    a11, a9
        .reloc  ., R_XTENSA_ASM_EXPAND, __adddf3
        l32r     a8, .Lsec_26_40
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, __truncdfsf2
        l32r     a8, .Lsec_26_44
        callx8   a8
        lsi      f2, a1, 0
        wfr      f1, a10
        olt.s    b0, f2, f1
        l32i.n   a14, a1, 20
        l32i.n   a15, a1, 12
        lsi      f0, a1, 16
        bf       b0, .Lsec_26_134
        mov.s    f1, f2
.Lsec_26_134:
        float.s  f2, a14, 0
        madd.s   f0, f2, f1
.Lsec_26_13a:
        addi.n   a7, a7, 1
.Lsec_26_13c:
        extui    a9, a7, 0, 8
        l16ui    a8, a4, 0
        bgeu     a9, a6, .Lsec_26_148
        j        .Lsec_26_b9
.Lsec_26_148:
        beqz     a8, .Lsec_26_198
        float.s  f1, a8, 0
        rfr      a10, f0
        rfr      a11, f1
        .reloc  ., R_XTENSA_ASM_EXPAND, __divsf3
        l32r     a8, .Lsec_26_48
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, __extendsfdf2
        l32r     a8, .Lsec_26_4c
        callx8   a8
        mov.n    a8, a10
        mov.n    a10, a2
        s32i.n   a8, a1, 4
        s32i.n   a11, a1, 8
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, __floatsidf
        l32r     a8, .Lsec_26_50
        callx8   a8
        l32r     a12, .Lsec_26_c
        l32r     a13, .Lsec_26_10
        .reloc  ., R_XTENSA_ASM_EXPAND, __muldf3
        l32r     a8, .Lsec_26_54
        callx8   a8
        mov.n    a12, a10
        mov.n    a13, a11
        l32i.n   a8, a1, 4
        mov.n    a10, a8
        l32i     a9, a1, 8
        mov.n    a11, a9
        .reloc  ., R_XTENSA_ASM_EXPAND, __subdf3
        l32r     a8, .Lsec_26_58
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, __truncdfsf2
        l32r     a8, .Lsec_26_5c
        callx8   a8
        s32i.n   a10, a5, 0
.Lsec_26_198:
        memw
        retw.n
        .size   pm_beacon_offset_get_average, .-pm_beacon_offset_get_average
.Lsec_26_19d:

        .section .wifislpiram.3,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_27_0, 0x3f800000
        .literal .Lsec_27_4, 0x00000000
        .literal .Lsec_27_8, s_pm_beacon_offset_config
        .literal .Lsec_27_c, g_pm
        .literal .Lsec_27_10, 0x447a0000
        .literal .Lsec_27_14, 0xd2f1a9fc
        .literal .Lsec_27_18, 0x3ff0624d
        .literal .Lsec_27_1c, pm_beacon_offset_get_average
        .literal .Lsec_27_20, __divsf3
        .literal .Lsec_27_24, __divsf3
        .literal .Lsec_27_28, __extendsfdf2
        .literal .Lsec_27_2c, __extendsfdf2
        .literal .Lsec_27_30, __muldf3
        .literal .Lsec_27_34, __adddf3
        .literal .Lsec_27_38, __truncdfsf2
        .literal .Lsec_27_3c, __extendsfdf2
        .literal .Lsec_27_40, __muldf3
        .literal .Lsec_27_44, __truncdfsf2
        .literal .Lsec_27_48, __divsf3
        .literal .Lsec_27_4c, __divsf3
.Lsec_27_50:
        .global pm_beacon_offset_get_expect
        .type   pm_beacon_offset_get_expect, @function
pm_beacon_offset_get_expect:
        entry    a1, 80
        movi.n   a4, 0
        lsi      f0, a6, 20
        extui    a2, a2, 0, 8
        extui    a3, a3, 0, 8
        mov.n    a10, a2
        mov.n    a13, a1
        s16i     a4, a1, 4
        memw
        addi.n   a12, a1, 4
        or       a11, a3, a3
        l32r     a4, .Lsec_27_4
        s32i     a5, a1, 16
        s32i     a4, a1, 0
        ssi      f0, a1, 20
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_beacon_offset_get_average
        l32r     a8, .Lsec_27_1c
        callx8   a8
        mov.n    a10, a4
        l16ui    a7, a1, 4
        beqz     a7, .Lsec_27_1e2
        ufloat.s f4, a7, 0
        sub      a3, a3, a2
        l32r     a4, .Lsec_27_8
        rfr      a10, f4
        extui    a3, a3, 0, 8
        l16ui    a4, a4, 0
        ufloat.s f1, a4, 0
        rfr      a11, f1
        .reloc  ., R_XTENSA_ASM_EXPAND, __divsf3
        l32r     a8, .Lsec_27_20
        callx8   a8
        lsi      f0, a1, 16
        wfr      f5, a10
        mul.s    f5, f5, f0
        l32r     a4, .Lsec_27_0
        wfr      f1, a4
        olt.s    b0, f5, f1
        ssi      f5, a1, 16
        lsi      f2, a1, 0
        bt       b0, .Lsec_27_c6
        j        .Lsec_27_1b8
.Lsec_27_c6:
        l32r     a8, .Lsec_27_c
        l32i     a4, a8, 0x1d0
        ufloat.s f0, a4, 0
        rfr      a10, f0
        l32r     a11, .Lsec_27_10
        s32i.n   a8, a1, 32
        ssi      f1, a1, 28
        ssi      f2, a1, 36
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, __divsf3
        l32r     a8, .Lsec_27_24
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, __extendsfdf2
        l32r     a8, .Lsec_27_28
        callx8   a8
        ufloat.s f4, a3, 0
        mov.n    a4, a10
        rfr      a10, f4
        mov.n    a5, a11
        .reloc  ., R_XTENSA_ASM_EXPAND, __extendsfdf2
        l32r     a8, .Lsec_27_2c
        callx8   a8
        l32r     a12, .Lsec_27_14
        l32r     a13, .Lsec_27_18
        .reloc  ., R_XTENSA_ASM_EXPAND, __muldf3
        l32r     a8, .Lsec_27_30
        callx8   a8
        mov.n    a12, a10
        or       a13, a11, a11
        or       a10, a4, a4
        mov.n    a11, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, __adddf3
        l32r     a8, .Lsec_27_34
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, __truncdfsf2
        l32r     a8, .Lsec_27_38
        callx8   a8
        float.s  f0, a2, 0
        lsi      f5, a1, 20
        wfr      f3, a10
        sub.s    f5, f5, f0
        ssi      f3, a1, 24
        rfr      a10, f5
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, __extendsfdf2
        l32r     a8, .Lsec_27_3c
        callx8   a8
        l32r     a12, .Lsec_27_14
        l32r     a13, .Lsec_27_18
        .reloc  ., R_XTENSA_ASM_EXPAND, __muldf3
        l32r     a8, .Lsec_27_40
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, __truncdfsf2
        l32r     a8, .Lsec_27_44
        callx8   a8
        wfr      f0, a10
        lsi      f3, a1, 24
        olt.s    b0, f0, f3
        l32i.n   a8, a1, 32
        lsi      f1, a1, 28
        lsi      f2, a1, 36
        bf       b0, .Lsec_27_162
        wfr      f3, a10
.Lsec_27_162:
        movi.n   a4, 0
        mov.s    f4, f1
        lsi      f5, a1, 16
        sub.s    f0, f1, f5
        l16si    a5, a8, 0x182
        j        .Lsec_27_17c
        .byte    0x00
.Lsec_27_174:
        addi.n   a4, a4, 1
        sext     a4, a4, 15
        mov.s    f4, f6
.Lsec_27_17c:
        mul.s    f6, f0, f4
        blt      a4, a5, .Lsec_27_174
        lsi      f5, a6, 24
        sub.s    f1, f1, f6
        sub.s    f5, f5, f2
        sub.s    f0, f2, f3
        mul.s    f1, f1, f3
        l32i.n   a11, a1, 16
        madd.s   f0, f5, f4
        rfr      a10, f1
        ssi      f0, a1, 24
        ssi      f2, a1, 36
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, __divsf3
        l32r     a8, .Lsec_27_48
        callx8   a8
        wfr      f1, a10
        lsi      f0, a1, 24
        add.s    f0, f1, f0
        lsi      f2, a1, 36
        j        .Lsec_27_1bb
        .byte    0x00
.Lsec_27_1b8:
        mov.s    f0, f2
.Lsec_27_1bb:
        lsi      f4, a1, 16
        rfr      a10, f0
        rfr      a11, f4
        ssi      f0, a6, 0
        ssi      f4, a6, 4
        ssi      f2, a6, 8
        s8i      a2, a6, 16
        s8i      a3, a6, 17
        s16i     a7, a6, 18
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, __divsf3
        l32r     a8, .Lsec_27_4c
        callx8   a8
.Lsec_27_1e2:
        s32i.n   a10, a6, 28
        memw
        retw.n
        .size   pm_beacon_offset_get_expect, .-pm_beacon_offset_get_expect
.Lsec_27_1e9:

        .section .wifislpiram.11,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_28_0, s_pm_beacon_offset
.Lsec_28_4:
        .global pm_beacon_offset_is_sampling
        .type   pm_beacon_offset_is_sampling, @function
pm_beacon_offset_is_sampling:
        entry    a1, 32
        l32r     a2, .Lsec_28_0
        movi.n   a8, 0
        l16ui    a9, a2, 0
        movi.n   a2, 1
        moveqz   a2, a8, a9
        retw.n
        .size   pm_beacon_offset_is_sampling, .-pm_beacon_offset_is_sampling
.Lsec_28_16:

        .section .wifislpiram.12,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_29_0, s_pm_beacon_offset_config
.Lsec_29_4:
        .global pm_beacon_offset_is_enabled
        .type   pm_beacon_offset_is_enabled, @function
pm_beacon_offset_is_enabled:
        entry    a1, 32
        l32r     a2, .Lsec_29_0
        movi.n   a8, 0
        l16ui    a9, a2, 0
        movi.n   a2, 1
        moveqz   a2, a8, a9
        retw.n
        .size   pm_beacon_offset_is_enabled, .-pm_beacon_offset_is_enabled
.Lsec_29_16:

        .section .text.pm_beacon_offset_get_rx_beacon_success_rate,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_30_0, s_pm_beacon_offset
        .literal .Lsec_30_4, __divsf3
.Lsec_30_8:
        .global pm_beacon_offset_get_rx_beacon_success_rate
        .type   pm_beacon_offset_get_rx_beacon_success_rate, @function
pm_beacon_offset_get_rx_beacon_success_rate:
        entry    a1, 32
        l32r     a8, .Lsec_30_0
        l32i.n   a9, a8, 8
        ufloat.s f0, a9, 0
        rfr      a11, f0
        l32i     a8, a8, 12
        sub      a8, a9, a8
        ufloat.s f0, a8, 0
        rfr      a10, f0
        .reloc  ., R_XTENSA_ASM_EXPAND, __divsf3
        l32r     a8, .Lsec_30_4
        callx8   a8
        mov.n    a2, a10
        retw.n
        .size   pm_beacon_offset_get_rx_beacon_success_rate, .-pm_beacon_offset_get_rx_beacon_success_rate
.Lsec_30_2c:

        .section .text.pm_beacon_offset_add_total_counter,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_31_0, s_pm_beacon_offset
.Lsec_31_4:
        .global pm_beacon_offset_add_total_counter
        .type   pm_beacon_offset_add_total_counter, @function
pm_beacon_offset_add_total_counter:
        entry    a1, 32
        l32r     a9, .Lsec_31_0
        l32i.n   a8, a9, 8
        addi.n   a8, a8, 1
        s32i.n   a8, a9, 8
        memw
        retw.n
        .size   pm_beacon_offset_add_total_counter, .-pm_beacon_offset_add_total_counter
.Lsec_31_15:

        .section .text.pm_beacon_offset_add_loss_counter,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_32_0, s_pm_beacon_offset
.Lsec_32_4:
        .global pm_beacon_offset_add_loss_counter
        .type   pm_beacon_offset_add_loss_counter, @function
pm_beacon_offset_add_loss_counter:
        entry    a1, 32
        l32r     a9, .Lsec_32_0
        l32i.n   a8, a9, 12
        addi.n   a8, a8, 1
        s32i.n   a8, a9, 12
        memw
        retw.n
        .size   pm_beacon_offset_add_loss_counter, .-pm_beacon_offset_add_loss_counter
.Lsec_32_15:

        .section .text.empty_func_p_void_r_void,"ax",@progbits
        .align  4
.Lsec_33_0:
        .global empty_func_p_void_r_void
        .type   empty_func_p_void_r_void, @function
empty_func_p_void_r_void:
        entry    a1, 32
        retw.n
        .size   empty_func_p_void_r_void, .-empty_func_p_void_r_void
.Lsec_33_5:

        .section .text.empty_func_p_u32_r_void,"ax",@progbits
        .align  4
.Lsec_34_0:
        .global empty_func_p_u32_r_void
        .type   empty_func_p_u32_r_void, @function
empty_func_p_u32_r_void:
        entry    a1, 32
        retw.n
        .size   empty_func_p_u32_r_void, .-empty_func_p_u32_r_void
.Lsec_34_5:

        .section .text.empty_func_p_void_r_bool,"ax",@progbits
        .align  4
.Lsec_35_0:
        .global empty_func_p_void_r_bool
        .type   empty_func_p_void_r_bool, @function
empty_func_p_void_r_bool:
        entry    a1, 32
        movi.n   a2, 0
        retw.n
        .size   empty_func_p_void_r_bool, .-empty_func_p_void_r_bool
.Lsec_35_7:

        .section .text.empty_func_p_pconfg_r_void,"ax",@progbits
        .align  4
.Lsec_36_0:
        .global empty_func_p_pconfg_r_void
        .type   empty_func_p_pconfg_r_void, @function
empty_func_p_pconfg_r_void:
        entry    a1, 32
        retw.n
        .size   empty_func_p_pconfg_r_void, .-empty_func_p_pconfg_r_void
.Lsec_36_5:

        .section .text.empty_func_p_u8_r_void,"ax",@progbits
        .align  4
.Lsec_37_0:
        .global empty_func_p_u8_r_void
        .type   empty_func_p_u8_r_void, @function
empty_func_p_u8_r_void:
        entry    a1, 32
        retw.n
        .size   empty_func_p_u8_r_void, .-empty_func_p_u8_r_void
.Lsec_37_5:

        .section .text.empty_func_p_void_r_float,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_38_0, 0x00000000
.Lsec_38_4:
        .global empty_func_p_void_r_float
        .type   empty_func_p_void_r_float, @function
empty_func_p_void_r_float:
        entry    a1, 32
        l32r     a2, .Lsec_38_0
        retw.n
        .size   empty_func_p_void_r_float, .-empty_func_p_void_r_float
.Lsec_38_c:

        .section .text.empty_func_p_ptsf_u32_r_void,"ax",@progbits
        .align  4
.Lsec_39_0:
        .global empty_func_p_ptsf_u32_r_void
        .type   empty_func_p_ptsf_u32_r_void, @function
empty_func_p_ptsf_u32_r_void:
        entry    a1, 32
        retw.n
        .size   empty_func_p_ptsf_u32_r_void, .-empty_func_p_ptsf_u32_r_void
.Lsec_39_5:

        .section .text.empty_func_p_u8_u8_pu16_pfloat_r_void,"ax",@progbits
        .align  4
.Lsec_40_0:
        .global empty_func_p_u8_u8_pu16_pfloat_r_void
        .type   empty_func_p_u8_u8_pu16_pfloat_r_void, @function
empty_func_p_u8_u8_pu16_pfloat_r_void:
        entry    a1, 32
        retw.n
        .size   empty_func_p_u8_u8_pu16_pfloat_r_void, .-empty_func_p_u8_u8_pu16_pfloat_r_void
.Lsec_40_5:

        .section .text.empty_func_p_u8_u8_float_float_pparams_r_void,"ax",@progbits
        .align  4
.Lsec_41_0:
        .global empty_func_p_u8_u8_float_float_pparams_r_void
        .type   empty_func_p_u8_u8_float_float_pparams_r_void, @function
empty_func_p_u8_u8_float_float_pparams_r_void:
        entry    a1, 32
        retw.n
        .size   empty_func_p_u8_u8_float_float_pparams_r_void, .-empty_func_p_u8_u8_float_float_pparams_r_void
.Lsec_41_5:

        .section .wifislpiram.4,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_42_0, 0x00000000
        .literal .Lsec_42_4, g_pm
        .literal .Lsec_42_8, s_pm_beacon_offset_config
        .literal .Lsec_42_c, .Lsec_71_0
        .literal .Lsec_42_10, 0xd2f1a9fc
        .literal .Lsec_42_14, 0x3ff0624d
        .literal .Lsec_42_18, s_pm_beacon_offset
        .literal .Lsec_42_1c, 0x42c80000
        .literal .Lsec_42_20, g_pm_cfg
        .literal .Lsec_42_24, 0x0036ee80
        .literal .Lsec_42_28, s_pm_beacon_offset+0x10
        .literal .Lsec_42_2c, g_osi_funcs_p
        .literal .Lsec_42_30, pm_beacon_offset_get_average
        .literal .Lsec_42_34, wifi_log
        .literal .Lsec_42_38, memset
        .literal .Lsec_42_3c, __divsf3
        .literal .Lsec_42_40, __floatunsidf
        .literal .Lsec_42_44, __muldf3
        .literal .Lsec_42_48, __truncdfsf2
        .literal .Lsec_42_4c, __divsf3
        .literal .Lsec_42_50, memcpy
        .literal .Lsec_42_54, pm_beacon_offset_get_expect
        .literal .Lsec_42_58, memcpy
        .literal .Lsec_42_5c, memcpy
.Lsec_42_60:
        .global pm_beacon_offset_get_params
        .type   pm_beacon_offset_get_params, @function
pm_beacon_offset_get_params:
        entry    a1, 160
        l32r     a3, .Lsec_42_4
        mov.n    a6, a2
        movi.n   a5, 0
        addi     a13, a1, 80
        addi     a12, a1, 84
        l32r     a2, .Lsec_42_0
        movi.n   a10, 0
        l32i.n   a4, a3, 44
        s32i     a2, a1, 80
        srli     a2, a4, 10
        extui    a4, a4, 10, 8
        mov.n    a11, a4
        s16i     a5, a1, 84
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_beacon_offset_get_average
        l32r     a8, .Lsec_42_30
        callx8   a8
        l32r     a5, .Lsec_42_8
        l16ui    a15, a1, 84
        l16ui    a5, a5, 0
        bne      a5, a15, .Lsec_42_a0
        bgeu     a6, a5, .Lsec_42_bc
.Lsec_42_a0:
        movi.n   a12, 1
        l32r     a13, .Lsec_42_c
        mov.n    a14, a5
        slli     a11, a12, 12
        movi.n   a10, 6
        s32i     a6, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_42_34
        callx8   a8
        j        .Lsec_42_2a6
        .byte    0x00
.Lsec_42_bc:
        extui    a8, a2, 0, 8
        movi     a12, 32
        movi     a11, 0
        addi     a10, a1, 48
        s32i     a8, a1, 100
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, memset
        l32r     a8, .Lsec_42_38
        callx8   a8
        ufloat.s f1, a5, 0
        ufloat.s f0, a6, 0
        rfr      a10, f1
        rfr      a11, f0
        .reloc  ., R_XTENSA_ASM_EXPAND, __divsf3
        l32r     a8, .Lsec_42_3c
        callx8   a8
        mov.n    a6, a10
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, __floatunsidf
        l32r     a8, .Lsec_42_40
        callx8   a8
        l32r     a12, .Lsec_42_10
        l32r     a13, .Lsec_42_14
        .reloc  ., R_XTENSA_ASM_EXPAND, __muldf3
        l32r     a8, .Lsec_42_44
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, __truncdfsf2
        l32r     a8, .Lsec_42_48
        callx8   a8
        wfr      f3, a10
        lsi      f2, a1, 80
        mov.n    a11, a6
        sub.s    f0, f2, f3
        ssi      f2, a1, 96
        ssi      f0, a1, 120
        s32i     a10, a1, 68
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, __divsf3
        l32r     a8, .Lsec_42_4c
        callx8   a8
        wfr      f1, a10
        movi.n   a12, 32
        addi     a11, a1, 48
        lsi      f0, a1, 120
        addi     a10, a1, 16
        add.s    f0, f0, f1
        ssi      f0, a1, 72
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_42_50
        callx8   a8
        l32i     a5, a1, 100
        addi.n   a2, a5, -1
        extui    a2, a2, 0, 8
        movi.n   a5, 0
        l32r     a8, .Lsec_42_0
        sub      a2, a4, a2
        wfr      f0, a8
        mov.n    a9, a5
        wfr      f1, a8
        mov.n    a15, a5
.Lsec_42_157:
        sub      a8, a4, a2
        movi.n   a7, 0
        s32i     a8, a1, 104
        memw
        j        .Lsec_42_1df
.Lsec_42_165:
        l32i     a8, a1, 100
        addi     a14, a1, 48
        sub      a11, a8, a7
        mov.n    a13, a6
        extui    a11, a11, 0, 8
        l32i     a12, a1, 96
        l32i     a10, a1, 104
        s32i     a9, a1, 116
        s32i     a15, a1, 108
        ssi      f0, a1, 120
        ssi      f1, a1, 112
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_beacon_offset_get_expect
        l32r     a8, .Lsec_42_54
        callx8   a8
        l32r     a8, .Lsec_42_0
        wfr      f3, a8
        lsi      f2, a1, 76
        oeq.s    b0, f2, f3
        l32i     a9, a1, 116
        l32i     a15, a1, 108
        lsi      f0, a1, 120
        lsi      f1, a1, 112
        bt       b0, .Lsec_42_1da
        ole.s    b0, f2, f0
        bt       b0, .Lsec_42_1b5
        oeq.s    b0, f0, f3
        bf       b0, .Lsec_42_1da
.Lsec_42_1b5:
        movi.n   a12, 32
        addi     a11, a1, 48
        addi     a10, a1, 16
        ssi      f2, a1, 108
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_42_58
        callx8   a8
        movi.n   a15, 1
        lsi      f2, a1, 108
        mov.s    f0, f2
        lsi      f1, a1, 52
        l8ui     a9, a1, 64
        l8ui     a5, a1, 65
.Lsec_42_1da:
        addi.n   a8, a7, 1
        extui    a7, a8, 0, 8
.Lsec_42_1df:
        blt      a7, a2, .Lsec_42_165
        beq      a4, a2, .Lsec_42_1ea
        addi.n   a2, a2, 1
        j        .Lsec_42_157
.Lsec_42_1ea:
        l32r     a4, .Lsec_42_18
        beqz     a15, .Lsec_42_274
        slli     a2, a9, 10
        movi     a8, 255
        addmi    a7, a3, 0x100
        s32i     a2, a3, 0x1cc
        l32i     a2, a3, 0x1d0
        srli     a2, a2, 10
        addi.n   a2, a2, 1
        add.n    a2, a2, a5
        minu     a2, a2, a8
        s8i      a2, a7, 124
        memw
        movi.n   a8, 1
        l32r     a2, .Lsec_42_1c
        wfr      f3, a2
        mul.s    f2, f1, f3
        l32r     a7, .Lsec_42_8
        utrunc.s a2, f2, 0
        extui    a2, a2, 0, 8
        l8ui     a10, a7, 3
        s8i      a8, a4, 73
        memw
        bgeu     a2, a10, .Lsec_42_27a
        addmi    a3, a3, 0x200
        l8ui     a2, a7, 2
        s8i      a8, a3, 28
        memw
        beqz.n   a2, .Lsec_42_27a
        l32r     a3, .Lsec_42_20
        l8ui     a3, a3, 51
        bnei     a3, 1, .Lsec_42_27a
        movi.n   a12, 0
        l32r     a3, .Lsec_42_2c
        l32r     a11, .Lsec_42_24
        mull     a11, a2, a11
        l32i.n   a3, a3, 0
        l32r     a10, .Lsec_42_28
        l32i     a3, a3, 232
        s32i     a9, a1, 116
        ssi      f0, a1, 120
        ssi      f1, a1, 112
        memw
        callx8   a3
        l32i     a9, a1, 116
        lsi      f0, a1, 120
        lsi      f1, a1, 112
        j        .Lsec_42_27a
.Lsec_42_274:
        s8i      a15, a4, 73
        memw
.Lsec_42_27a:
        movi.n   a3, 2
        movi.n   a12, 32
        addi     a11, a1, 16
        addi     a10, a4, 40
        l32i.n   a2, a4, 36
        or       a2, a2, a3
        s32i     a2, a4, 36
        ssi      f1, a1, 20
        s8i      a9, a1, 32
        s8i      a5, a1, 33
        memw
        ssi      f0, a1, 44
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_42_5c
        callx8   a8
        s32i.n   a6, a4, 52
.Lsec_42_2a6:
        memw
        retw.n
        .size   pm_beacon_offset_get_params, .-pm_beacon_offset_get_params
.Lsec_42_2ab:

        .section .wifislpiram.9,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_43_0, s_pm_beacon_offset
        .literal .Lsec_43_4, g_pm
        .literal .Lsec_43_8, .Lsec_60_0
        .literal .Lsec_43_c, g_mesh_is_started
        .literal .Lsec_43_10, g_mesh_is_root
        .literal .Lsec_43_14, g_mesh_init_ps_type
        .literal .Lsec_43_18, g_osi_funcs_p
        .literal .Lsec_43_1c, 0x3ff73c00
        .literal .Lsec_43_20, g_wifi_mac_time_delta
        .literal .Lsec_43_24, .Lsec_59_0
        .literal .Lsec_43_28, pm_beacon_offset_get_params
.Lsec_43_2c:
        .global pm_beacon_offset_sample
        .type   pm_beacon_offset_sample, @function
pm_beacon_offset_sample:
        entry    a1, 32
        l32r     a3, .Lsec_43_0
        l16ui    a4, a3, 0
        beqz     a4, .Lsec_43_f0
        l32r     a4, .Lsec_43_4
        l32i     a8, a4, 80
        l32i     a9, a4, 84
        or       a5, a8, a9
        beqz     a5, .Lsec_43_f0
        l32r     a5, .Lsec_43_8
        l32i.n   a10, a5, 0
        bne      a8, a10, .Lsec_43_52
        beqz     a9, .Lsec_43_f0
.Lsec_43_52:
        movi.n   a9, 1
        s32i.n   a8, a5, 0
        l32i.n   a8, a3, 36
        or       a8, a8, a9
        s32i.n   a8, a3, 36
        l32r     a8, .Lsec_43_c
        l8ui     a8, a8, 0
        beqz.n   a8, .Lsec_43_8c
        l32r     a8, .Lsec_43_10
        memw
        l8ui     a8, a8, 0
        extui    a8, a8, 0, 8
        bnez.n   a8, .Lsec_43_8c
        l32r     a8, .Lsec_43_14
        l32i.n   a8, a8, 0
        beqz.n   a8, .Lsec_43_8c
        l32r     a8, .Lsec_43_18
        l32i.n   a8, a8, 0
        l32i     a10, a8, 0x110
        memw
        callx8   a10
        j        .Lsec_43_9b
        .byte    0x00
.Lsec_43_8c:
        l32r     a8, .Lsec_43_1c
        memw
        l32i.n   a10, a8, 0
        l32r     a8, .Lsec_43_20
        l32i.n   a8, a8, 0
        add.n    a10, a10, a8
.Lsec_43_9b:
        srli     a2, a2, 10
        l32i.n   a5, a5, 0
        sub      a10, a10, a5
        l32i.n   a4, a4, 44
        quou     a10, a10, a4
        l32r     a4, .Lsec_43_24
        l32i.n   a5, a3, 4
        addx2    a2, a2, a5
        l32i.n   a8, a4, 0
        addi.n   a8, a8, 1
        add.n    a10, a10, a8
        l16ui    a8, a2, 0
        addi.n   a8, a8, 1
        s16i     a8, a2, 0
        memw
        l16ui    a2, a3, 0
        addi.n   a2, a2, -1
        extui    a2, a2, 0, 16
        s32i.n   a10, a4, 0
        s16i     a2, a3, 0
        memw
        bnez.n   a2, .Lsec_43_f0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_beacon_offset_get_params
        l32r     a8, .Lsec_43_28
        callx8   a8
        s32i.n   a2, a4, 0
        l32r     a4, .Lsec_43_18
        l32i.n   a4, a4, 0
        l32i.n   a10, a3, 4
        l32i     a4, a4, 176
        memw
        callx8   a4
        s32i.n   a2, a3, 4
.Lsec_43_f0:
        memw
        retw.n
        .size   pm_beacon_offset_sample, .-pm_beacon_offset_sample
.Lsec_43_f5:

        .section .text.pm_beacon_offset_configure,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_44_0, s_pm_beacon_offset_config
        .literal .Lsec_44_4, .Lsec_68_0
        .literal .Lsec_44_8, 0x42c80000
        .literal .Lsec_44_c, __divsf3
        .literal .Lsec_44_10, __extendsfdf2
        .literal .Lsec_44_14, __divsf3
        .literal .Lsec_44_18, __extendsfdf2
        .literal .Lsec_44_1c, wifi_log
.Lsec_44_20:
        .global pm_beacon_offset_configure
        .type   pm_beacon_offset_configure, @function
pm_beacon_offset_configure:
        entry    a1, 48
        l16ui    a3, a2, 0
        l32r     a8, .Lsec_44_0
        s16i     a3, a8, 0
        memw
        beqz     a3, .Lsec_44_9c
        l8ui     a9, a2, 4
        ufloat.s f0, a9, 0
        rfr      a10, f0
        l8ui     a4, a2, 2
        l8ui     a5, a2, 3
        movi.n   a2, 1
        slli     a2, a2, 12
        l32r     a11, .Lsec_44_8
        s8i      a4, a8, 2
        s8i      a5, a8, 3
        s8i      a9, a8, 4
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, __divsf3
        l32r     a8, .Lsec_44_c
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, __extendsfdf2
        l32r     a8, .Lsec_44_10
        callx8   a8
        ufloat.s f0, a5, 0
        s32i     a11, a1, 12
        l32r     a11, .Lsec_44_8
        s32i     a10, a1, 8
        rfr      a10, f0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, __divsf3
        l32r     a8, .Lsec_44_14
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, __extendsfdf2
        l32r     a8, .Lsec_44_18
        callx8   a8
        mov.n    a15, a4
        l32r     a13, .Lsec_44_4
        mov.n    a14, a3
        movi.n   a12, 3
        s32i.n   a10, a1, 0
        movi.n   a10, 6
        s32i.n   a11, a1, 4
        mov.n    a11, a2
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_44_1c
        callx8   a8
.Lsec_44_9c:
        retw.n
        .size   pm_beacon_offset_configure, .-pm_beacon_offset_configure
.Lsec_44_9e:

        .section .text.pm_beacon_offset_set_rx_beacon_standard,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_45_0, s_pm_beacon_offset_config
        .literal .Lsec_45_4, 0x42c80000
        .literal .Lsec_45_8, .Lsec_67_0
        .literal .Lsec_45_c, __divsf3
        .literal .Lsec_45_10, __extendsfdf2
        .literal .Lsec_45_14, __divsf3
        .literal .Lsec_45_18, __extendsfdf2
        .literal .Lsec_45_1c, wifi_log
.Lsec_45_20:
        .global pm_beacon_offset_set_rx_beacon_standard
        .type   pm_beacon_offset_set_rx_beacon_standard, @function
pm_beacon_offset_set_rx_beacon_standard:
        entry    a1, 64
        l32r     a4, .Lsec_45_0
        extui    a2, a2, 0, 8
        l8ui     a3, a4, 3
        ufloat.s f0, a3, 0
        movi.n   a3, 1
        rfr      a10, f0
        l32r     a11, .Lsec_45_4
        slli     a3, a3, 12
        .reloc  ., R_XTENSA_ASM_EXPAND, __divsf3
        l32r     a8, .Lsec_45_c
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, __extendsfdf2
        l32r     a8, .Lsec_45_10
        callx8   a8
        ufloat.s f0, a2, 0
        mov.n    a15, a11
        l32r     a11, .Lsec_45_4
        mov.n    a14, a10
        rfr      a10, f0
        s32i.n   a14, a1, 16
        s32i.n   a15, a1, 20
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, __divsf3
        l32r     a8, .Lsec_45_14
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, __extendsfdf2
        l32r     a8, .Lsec_45_18
        callx8   a8
        movi.n   a12, 3
        l32i.n   a14, a1, 16
        l32i.n   a15, a1, 20
        l32r     a13, .Lsec_45_8
        s32i.n   a10, a1, 0
        movi.n   a10, 6
        s32i.n   a11, a1, 4
        mov.n    a11, a3
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_45_1c
        callx8   a8
        s8i      a2, a4, 3
        memw
        memw
        retw.n
        .size   pm_beacon_offset_set_rx_beacon_standard, .-pm_beacon_offset_set_rx_beacon_standard
.Lsec_45_8b:

        .section .wifislprxiram.10,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_46_0, s_pm_beacon_offset
        .literal .Lsec_46_4, g_pm
        .literal .Lsec_46_8, g_pm_cfg
        .literal .Lsec_46_c, g_osi_funcs_p
        .literal .Lsec_46_10, s_pm_beacon_offset+0x28
        .literal .Lsec_46_14, memset
.Lsec_46_18:
        .global pm_beacon_offset_reset
        .type   pm_beacon_offset_reset, @function
pm_beacon_offset_reset:
        entry    a1, 32
        l32r     a2, .Lsec_46_0
        movi.n   a3, 0
        movi.n   a4, 0
        l32r     a9, .Lsec_46_8
        s16i     a3, a2, 0
        memw
        l32r     a3, .Lsec_46_4
        addmi    a8, a3, 0x100
        l8ui     a9, a9, 37
        l32i.n   a10, a2, 4
        s32i     a4, a3, 0x1cc
        s8i      a9, a8, 124
        memw
        beq      a10, a4, .Lsec_46_52
        l32r     a8, .Lsec_46_c
        l32i.n   a8, a8, 0
        l32i     a8, a8, 176
        memw
        callx8   a8
        s32i.n   a4, a2, 4
.Lsec_46_52:
        movi.n   a4, 0
        movi.n   a12, 32
        mov.n    a11, a4
        addmi    a3, a3, 0x200
        l32r     a10, .Lsec_46_10
        s32i.n   a4, a2, 8
        s32i     a4, a2, 12
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, memset
        l32r     a8, .Lsec_46_14
        callx8   a8
        s16i     a4, a2, 72
        s8i      a4, a3, 28
        memw
        memw
        retw.n
        .size   pm_beacon_offset_reset, .-pm_beacon_offset_reset
.Lsec_46_7a:

        .section .text.pm_beacon_offset_init,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_47_0, .Lsec_53_4
        .literal .Lsec_47_4, s_pm_beacon_offset+0x10
        .literal .Lsec_47_8, g_osi_funcs_p
        .literal .Lsec_47_c, pm_beacon_offset_reset
.Lsec_47_10:
        .global pm_beacon_offset_init
        .type   pm_beacon_offset_init, @function
pm_beacon_offset_init:
        entry    a1, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_beacon_offset_reset
        l32r     a8, .Lsec_47_c
        callx8   a8
        l32r     a8, .Lsec_47_8
        movi.n   a12, 0
        l32i.n   a8, a8, 0
        l32r     a11, .Lsec_47_0
        l32i     a8, a8, 244
        l32r     a10, .Lsec_47_4
        callx8   a8
        retw.n
        .size   pm_beacon_offset_init, .-pm_beacon_offset_init
.Lsec_47_2e:

        .section .text.pm_beacon_offset_deinit,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_48_0, s_pm_beacon_offset+0x10
        .literal .Lsec_48_4, g_osi_funcs_p
        .literal .Lsec_48_8, pm_beacon_offset_reset
.Lsec_48_c:
        .global pm_beacon_offset_deinit
        .type   pm_beacon_offset_deinit, @function
pm_beacon_offset_deinit:
        entry    a1, 32
        l32r     a3, .Lsec_48_4
        l32i     a8, a3, 0
        l32r     a2, .Lsec_48_0
        mov.n    a10, a2
        l32i     a8, a8, 236
        callx8   a8
        or       a10, a2, a2
        l32i     a3, a3, 0
        l32i     a3, a3, 240
        callx8   a3
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_beacon_offset_reset
        l32r     a8, .Lsec_48_8
        callx8   a8
        retw.n
        .size   pm_beacon_offset_deinit, .-pm_beacon_offset_deinit
.Lsec_48_34:

        .section .wifislprxiram.6,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_49_0, s_pm_beacon_offset
        .literal .Lsec_49_4, s_pm_beacon_offset_config
        .literal .Lsec_49_8, g_pm
        .literal .Lsec_49_c, .Lsec_70_0
        .literal .Lsec_49_10, g_osi_funcs_p
        .literal .Lsec_49_14, .Lsec_69_0
        .literal .Lsec_49_18, s_pm_beacon_offset+0x10
        .literal .Lsec_49_1c, pm_beacon_offset_reset
        .literal .Lsec_49_20, wifi_log
        .literal .Lsec_49_24, memset
.Lsec_49_28:
        .global pm_beacon_offset_sample_start
        .type   pm_beacon_offset_sample_start, @function
pm_beacon_offset_sample_start:
        entry    a1, 32
        l32r     a3, .Lsec_49_0
        l16ui    a5, a3, 0
        bnez     a5, .Lsec_49_a8
        l32r     a4, .Lsec_49_4
        l16ui    a2, a4, 0
        beqz     a2, .Lsec_49_a8
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_beacon_offset_reset
        l32r     a8, .Lsec_49_1c
        callx8   a8
        movi     a6, 255
        l32r     a2, .Lsec_49_8
        l32i.n   a2, a2, 44
        srli     a2, a2, 10
        bgeu     a6, a2, .Lsec_49_69
        movi.n   a12, 3
        l32r     a13, .Lsec_49_c
.Lsec_49_56:
        movi.n   a11, 1
        slli     a11, a11, 12
        movi     a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_49_20
        callx8   a8
        j        .Lsec_49_a8
        .byte    0x00
        .byte    0x00
.Lsec_49_69:
        l32r     a6, .Lsec_49_10
        add.n    a2, a2, a2
        mov.n    a10, a2
        l32i     a8, a6, 0
        l32i     a8, a8, 172
        callx8   a8
        movi.n   a12, 1
        s32i.n   a10, a3, 4
        l32r     a13, .Lsec_49_14
        beqz     a10, .Lsec_49_56
        mov.n    a12, a2
        or       a11, a5, a5
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, memset
        l32r     a8, .Lsec_49_24
        callx8   a8
        l16ui    a2, a4, 0
        s16i     a2, a3, 0
        memw
        l32i.n   a2, a6, 0
        l32r     a10, .Lsec_49_18
        l32i     a2, a2, 236
        memw
        callx8   a2
.Lsec_49_a8:
        retw.n
        .size   pm_beacon_offset_sample_start, .-pm_beacon_offset_sample_start
.Lsec_49_aa:

        .section .text.pm_beacon_offset_resample_timeout_process,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_50_0, pm_on_sample_beacon
.Lsec_50_4:
        .global pm_beacon_offset_resample_timeout_process
        .type   pm_beacon_offset_resample_timeout_process, @function
pm_beacon_offset_resample_timeout_process:
        entry    a1, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_on_sample_beacon
        l32r     a8, .Lsec_50_0
        callx8   a8
        retw.n
        .size   pm_beacon_offset_resample_timeout_process, .-pm_beacon_offset_resample_timeout_process
.Lsec_50_f:

        .section .rodata.pm_beacon_offset_check.str1.1,"aMS",@progbits,1
        .align  1
.Lsec_51_0:
        .ascii  "finished\000"
.Lsec_51_9:
        .ascii  "failed\000"

        .section .text.pm_beacon_offset_check,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_52_0, .Lsec_51_0
        .literal .Lsec_52_4, .Lsec_51_9
        .literal .Lsec_52_8, s_pm_beacon_offset_config
        .literal .Lsec_52_c, s_pm_beacon_offset
        .literal .Lsec_52_10, g_pm
        .literal .Lsec_52_14, 0x3f000000
        .literal .Lsec_52_18, 0x42c80000
        .literal .Lsec_52_1c, .Lsec_66_0
        .literal .Lsec_52_20, .Lsec_65_0
        .literal .Lsec_52_24, .Lsec_64_0
        .literal .Lsec_52_28, .Lsec_63_0
        .literal .Lsec_52_2c, pm_beacon_offset_get_rx_beacon_success_rate
        .literal .Lsec_52_30, wifi_event_post
        .literal .Lsec_52_34, __divsf3
        .literal .Lsec_52_38, __extendsfdf2
        .literal .Lsec_52_3c, wifi_log
        .literal .Lsec_52_40, wifi_log
        .literal .Lsec_52_44, __extendsfdf2
        .literal .Lsec_52_48, __extendsfdf2
        .literal .Lsec_52_4c, wifi_log
        .literal .Lsec_52_50, __extendsfdf2
        .literal .Lsec_52_54, __extendsfdf2
        .literal .Lsec_52_58, __extendsfdf2
        .literal .Lsec_52_5c, wifi_log
.Lsec_52_60:
        .global pm_beacon_offset_check
        .type   pm_beacon_offset_check, @function
pm_beacon_offset_check:
        entry    a1, 96
        l32r     a5, .Lsec_52_8
        l16ui    a9, a5, 0
        beqz     a9, .Lsec_52_220
        l32r     a4, .Lsec_52_c
        l8ui     a8, a4, 73
        beqz     a8, .Lsec_52_129
        l32r     a8, .Lsec_52_10
        l16si    a8, a8, 0x182
        mull     a8, a8, a9
        l32i.n   a9, a4, 8
        bgeu     a9, a8, .Lsec_52_86
        j        .Lsec_52_129
.Lsec_52_86:
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_beacon_offset_get_rx_beacon_success_rate
        l32r     a8, .Lsec_52_2c
        callx8   a8
        wfr      f0, a10
        l32r     a6, .Lsec_52_14
        wfr      f3, a6
        lsi      f1, a4, 44
        mul.s    f2, f1, f3
        olt.s    b0, f2, f0
        bt       b0, .Lsec_52_c1
.Lsec_52_a1:
        movi.n   a5, 0
        movi.n   a12, 4
        addi     a11, a1, 32
        movi.n   a10, 46
        s8i      a5, a4, 72
        memw
        ssi      f0, a1, 32
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_event_post
        l32r     a8, .Lsec_52_30
        callx8   a8
        j        .Lsec_52_122
        .byte    0x00
        .byte    0x00
.Lsec_52_c1:
        l8ui     a5, a5, 4
        ufloat.s f2, a5, 0
        rfr      a10, f2
        l32r     a11, .Lsec_52_18
        ssi      f0, a1, 52
        ssi      f1, a1, 48
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, __divsf3
        l32r     a8, .Lsec_52_34
        callx8   a8
        wfr      f3, a10
        lsi      f1, a1, 48
        sub.s    f1, f1, f3
        lsi      f0, a1, 52
        olt.s    b0, f1, f0
        bt       b0, .Lsec_52_11a
        l8ui     a5, a4, 72
        bnez     a5, .Lsec_52_a1
        rfr      a10, f0
        .reloc  ., R_XTENSA_ASM_EXPAND, __extendsfdf2
        l32r     a8, .Lsec_52_38
        callx8   a8
        mov.n    a15, a11
        l32r     a13, .Lsec_52_1c
        movi.n   a11, 1
        mov.n    a14, a10
        movi.n   a12, 4
        slli     a11, a11, 12
        movi     a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_52_3c
        callx8   a8
        movi.n   a5, 1
        j        .Lsec_52_11c
        .byte    0x00
.Lsec_52_11a:
        movi.n   a5, 0
.Lsec_52_11c:
        s8i      a5, a4, 72
        memw
.Lsec_52_122:
        movi.n   a5, 0
        s32i.n   a5, a4, 12
        s32i     a5, a4, 8
.Lsec_52_129:
        l32i.n   a5, a4, 36
        bbci     a5, 0, .Lsec_52_16a
        mov.n    a14, a2
        mov.n    a15, a2
        beqz.n   a2, .Lsec_52_138
        l32i.n   a14, a2, 0
        l32i.n   a15, a2, 4
.Lsec_52_138:
        movi.n   a11, 1
        slli     a11, a11, 12
        l32r     a5, .Lsec_52_10
        l32i     a2, a5, 80
        sub      a2, a3, a2
        l32i     a8, a5, 84
        movi.n   a5, 1
        bltu     a3, a2, .Lsec_52_150
        movi.n   a5, 0
.Lsec_52_150:
        neg      a8, a8
        sub      a8, a8, a5
        l32r     a13, .Lsec_52_20
        movi.n   a12, 4
        movi.n   a10, 6
        s32i.n   a2, a1, 0
        s32i.n   a8, a1, 4
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_52_40
        callx8   a8
.Lsec_52_16a:
        l32i.n   a2, a4, 36
        bbsi     a2, 1, .Lsec_52_172
        j        .Lsec_52_21c
.Lsec_52_172:
        l8ui     a2, a4, 73
        l32r     a5, .Lsec_52_0
        bnez.n   a2, .Lsec_52_17d
        l32r     a5, .Lsec_52_4
.Lsec_52_17d:
        movi.n   a2, 1
        slli     a2, a2, 12
        l32r     a3, .Lsec_52_10
        l32i     a8, a3, 0x1d0
        s32i.n   a8, a1, 24
        l32i.n   a10, a4, 44
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, __extendsfdf2
        l32r     a8, .Lsec_52_44
        callx8   a8
        s32i.n   a10, a1, 16
        s32i.n   a11, a1, 20
        l32i.n   a10, a4, 52
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, __extendsfdf2
        l32r     a8, .Lsec_52_48
        callx8   a8
        addmi    a8, a3, 0x100
        movi.n   a12, 3
        s32i.n   a10, a1, 8
        mov.n    a14, a5
        movi.n   a10, 6
        s32i.n   a11, a1, 12
        mov.n    a11, a2
        l8ui     a8, a8, 124
        slli     a8, a8, 10
        s32i.n   a8, a1, 0
        l32i     a15, a3, 0x1cc
        l32r     a13, .Lsec_52_24
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_52_4c
        callx8   a8
        l32i.n   a10, a4, 40
        .reloc  ., R_XTENSA_ASM_EXPAND, __extendsfdf2
        l32r     a8, .Lsec_52_50
        callx8   a8
        mov.n    a14, a10
        l16ui    a3, a4, 58
        s32i.n   a3, a1, 24
        l8ui     a3, a4, 57
        s32i.n   a3, a1, 20
        l8ui     a3, a4, 56
        s32i.n   a3, a1, 16
        l32i.n   a10, a4, 48
        s32i.n   a14, a1, 48
        s32i.n   a11, a1, 52
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, __extendsfdf2
        l32r     a8, .Lsec_52_54
        callx8   a8
        s32i.n   a10, a1, 8
        s32i.n   a11, a1, 12
        l32i     a10, a4, 68
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, __extendsfdf2
        l32r     a8, .Lsec_52_58
        callx8   a8
        movi.n   a12, 4
        l32i.n   a14, a1, 48
        l32i.n   a15, a1, 52
        l32r     a13, .Lsec_52_28
        s32i.n   a10, a1, 0
        movi.n   a10, 6
        s32i.n   a11, a1, 4
        mov.n    a11, a2
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_52_5c
        callx8   a8
.Lsec_52_21c:
        movi.n   a2, 0
        s32i.n   a2, a4, 36
.Lsec_52_220:
        memw
        retw.n
        .size   pm_beacon_offset_check, .-pm_beacon_offset_check
.Lsec_52_225:

        .section .text.pm_beacon_offset_resample_timeout,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_53_0, pp_timer_process
.Lsec_53_4:
        entry    a1, 32
        movi.n   a11, 0
        movi.n   a10, 12
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_timer_process
        l32r     a8, .Lsec_53_0
        callx8   a8
        retw.n

        .section .text.empty_func_p_pvoid_r_void,"ax",@progbits
        .align  4
.Lsec_54_0:
        .global empty_func_p_pvoid_r_void
        .type   empty_func_p_pvoid_r_void, @function
empty_func_p_pvoid_r_void:
        entry    a1, 32
        retw.n
        .size   empty_func_p_pvoid_r_void, .-empty_func_p_pvoid_r_void
.Lsec_54_5:

        .section .text.pm_beacon_offset_funcs_init,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_55_0, ptr_beacon_offset_funcs
        .literal .Lsec_55_4, pm_beacon_offset_sample_start
        .literal .Lsec_55_8, pm_beacon_offset_sample
        .literal .Lsec_55_c, pm_beacon_offset_reset
        .literal .Lsec_55_10, pm_beacon_offset_is_sampling
        .literal .Lsec_55_14, pm_beacon_offset_is_enabled
        .literal .Lsec_55_18, pm_beacon_offset_configure
        .literal .Lsec_55_1c, pm_beacon_offset_resample_timeout_process
        .literal .Lsec_55_20, pm_beacon_offset_set_rx_beacon_standard
        .literal .Lsec_55_24, pm_beacon_offset_get_rx_beacon_success_rate
        .literal .Lsec_55_28, pm_beacon_offset_add_total_counter
        .literal .Lsec_55_2c, pm_beacon_offset_add_loss_counter
        .literal .Lsec_55_30, pm_beacon_offset_check
        .literal .Lsec_55_34, pm_beacon_offset_init
        .literal .Lsec_55_38, pm_beacon_offset_deinit
        .literal .Lsec_55_3c, pm_beacon_offset_get_average
        .literal .Lsec_55_40, pm_beacon_offset_get_expect
        .literal .Lsec_55_44, pm_beacon_offset_get_params
.Lsec_55_48:
        .weak   pm_beacon_offset_funcs_init
        .type   pm_beacon_offset_funcs_init, @function
pm_beacon_offset_funcs_init:
        entry    a1, 32
        l32r     a8, .Lsec_55_0
        l32r     a9, .Lsec_55_4
        l32i.n   a8, a8, 0
        s32i.n   a9, a8, 0
        l32r     a9, .Lsec_55_8
        s32i.n   a9, a8, 4
        l32r     a9, .Lsec_55_c
        s32i.n   a9, a8, 8
        l32r     a9, .Lsec_55_10
        s32i.n   a9, a8, 12
        l32r     a9, .Lsec_55_14
        s32i.n   a9, a8, 16
        l32r     a9, .Lsec_55_18
        s32i.n   a9, a8, 20
        l32r     a9, .Lsec_55_1c
        s32i.n   a9, a8, 24
        l32r     a9, .Lsec_55_20
        s32i.n   a9, a8, 28
        l32r     a9, .Lsec_55_24
        s32i.n   a9, a8, 32
        l32r     a9, .Lsec_55_28
        s32i.n   a9, a8, 36
        l32r     a9, .Lsec_55_2c
        s32i.n   a9, a8, 40
        l32r     a9, .Lsec_55_30
        s32i.n   a9, a8, 44
        l32r     a9, .Lsec_55_34
        s32i.n   a9, a8, 48
        l32r     a9, .Lsec_55_38
        s32i.n   a9, a8, 52
        l32r     a9, .Lsec_55_3c
        s32i.n   a9, a8, 56
        l32r     a9, .Lsec_55_40
        s32i.n   a9, a8, 60
        l32r     a9, .Lsec_55_44
        s32i     a9, a8, 64
        memw
        retw.n
        .size   pm_beacon_offset_funcs_init, .-pm_beacon_offset_funcs_init
.Lsec_55_ab:

        .section .text.pm_beacon_offset_funcs_empty_init,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_56_0, ptr_beacon_offset_funcs
        .literal .Lsec_56_4, empty_func_p_void_r_void
        .literal .Lsec_56_8, empty_func_p_u32_r_void
        .literal .Lsec_56_c, empty_func_p_void_r_bool
        .literal .Lsec_56_10, empty_func_p_pconfg_r_void
        .literal .Lsec_56_14, empty_func_p_pvoid_r_void
        .literal .Lsec_56_18, empty_func_p_u8_r_void
        .literal .Lsec_56_1c, empty_func_p_void_r_float
        .literal .Lsec_56_20, empty_func_p_ptsf_u32_r_void
        .literal .Lsec_56_24, empty_func_p_u8_u8_pu16_pfloat_r_void
        .literal .Lsec_56_28, empty_func_p_u8_u8_float_float_pparams_r_void
.Lsec_56_2c:
        .global pm_beacon_offset_funcs_empty_init
        .type   pm_beacon_offset_funcs_empty_init, @function
pm_beacon_offset_funcs_empty_init:
        entry    a1, 32
        l32r     a8, .Lsec_56_0
        l32r     a11, .Lsec_56_c
        l32i.n   a8, a8, 0
        s32i.n   a11, a8, 12
        s32i.n   a11, a8, 16
        l32r     a11, .Lsec_56_10
        s32i.n   a11, a8, 20
        l32r     a11, .Lsec_56_14
        l32r     a9, .Lsec_56_4
        s32i.n   a11, a8, 24
        l32r     a11, .Lsec_56_18
        s32i.n   a9, a8, 0
        s32i.n   a9, a8, 8
        s32i.n   a11, a8, 28
        s32i.n   a9, a8, 36
        l32r     a11, .Lsec_56_1c
        s32i.n   a9, a8, 40
        s32i.n   a9, a8, 48
        s32i.n   a9, a8, 52
        l32r     a9, .Lsec_56_24
        l32r     a10, .Lsec_56_8
        s32i.n   a11, a8, 32
        s32i.n   a9, a8, 56
        l32r     a11, .Lsec_56_20
        l32r     a9, .Lsec_56_28
        s32i.n   a10, a8, 4
        s32i.n   a11, a8, 44
        s32i.n   a9, a8, 60
        s32i     a10, a8, 64
        memw
        retw.n
        .size   pm_beacon_offset_funcs_empty_init, .-pm_beacon_offset_funcs_empty_init
.Lsec_56_7a:

        .section .text.pm_funcs_init,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_57_0, g_osi_funcs_p
        .literal .Lsec_57_4, ptr_beacon_offset_funcs
        .literal .Lsec_57_8, .Lsec_61_0
        .literal .Lsec_57_c, wifi_log
        .literal .Lsec_57_10, pm_beacon_offset_funcs_init
.Lsec_57_14:
        .global pm_funcs_init
        .type   pm_funcs_init, @function
pm_funcs_init:
        entry    a1, 32
        l32r     a2, .Lsec_57_0
        movi.n   a11, 68
        movi     a10, 1
        l32i     a2, a2, 0
        l32i     a2, a2, 0x164
        callx8   a2
        l32r     a2, .Lsec_57_4
        s32i.n   a10, a2, 0
        bnez.n   a10, .Lsec_57_48
        movi.n   a12, 1
        mov.n    a11, a12
        movi.n   a10, 6
        l32r     a13, .Lsec_57_8
        movi     a2, 0x101
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_57_c
        callx8   a8
        j        .Lsec_57_50
        .byte    0x00
.Lsec_57_48:
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_beacon_offset_funcs_init
        l32r     a8, .Lsec_57_10
        callx8   a8
        movi.n   a2, 0
.Lsec_57_50:
        retw.n
        .size   pm_funcs_init, .-pm_funcs_init
.Lsec_57_52:

        .section .text.pm_funcs_deinit,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_58_0, ptr_beacon_offset_funcs
        .literal .Lsec_58_4, g_osi_funcs_p
.Lsec_58_8:
        .global pm_funcs_deinit
        .type   pm_funcs_deinit, @function
pm_funcs_deinit:
        entry    a1, 32
        l32r     a2, .Lsec_58_0
        l32i.n   a10, a2, 0
        beqz.n   a10, .Lsec_58_21
        l32r     a8, .Lsec_58_4
        l32i.n   a8, a8, 0
        l32i     a8, a8, 176
        callx8   a8
        movi.n   a8, 0
        s32i.n   a8, a2, 0
.Lsec_58_21:
        movi.n   a2, 0
        memw
        retw.n
        .size   pm_funcs_deinit, .-pm_funcs_deinit
.Lsec_58_28:

        .section .bss.total_tbtt_count$0,"aw",@nobits
        .align  4
.Lsec_59_0:
        .skip   4

        .section .bss.prev_last_tbtt_time$1,"aw",@nobits
        .align  4
.Lsec_60_0:
        .skip   4

        .section .rodata_wlog_error.20,"a",@progbits
        .align  1
.Lsec_61_0:
        .byte   0x61, 0x6c, 0x6c, 0x6f, 0x63, 0x20, 0x70, 0x6d, 0x5f, 0x62, 0x65, 0x61, 0x63, 0x6f, 0x6e, 0x5f
        .byte   0x6f, 0x66, 0x66, 0x73, 0x65, 0x74, 0x20, 0x66, 0x61, 0x69, 0x6c, 0x0a, 0x00

        .section .dram1.19,"aw",@progbits
        .align  4
.Lsec_62_0:
        .global ptr_beacon_offset_funcs
        .type   ptr_beacon_offset_funcs, @object
        .size   ptr_beacon_offset_funcs, 4
ptr_beacon_offset_funcs:
        .byte   0x00, 0x00, 0x00, 0x00

        .section .rodata_wlog_debug.18,"a",@progbits
        .align  1
.Lsec_63_0:
        .byte   0x65, 0x3a, 0x20, 0x25, 0x2e, 0x34, 0x66, 0x2c, 0x20, 0x65, 0x64, 0x70, 0x3a, 0x20, 0x25, 0x2e
        .byte   0x34, 0x66, 0x2c, 0x20, 0x61, 0x3a, 0x20, 0x25, 0x2e, 0x34, 0x66, 0x2c, 0x20, 0x73, 0x3a, 0x20
        .byte   0x25, 0x75, 0x2c, 0x20, 0x64, 0x3a, 0x20, 0x25, 0x75, 0x2c, 0x20, 0x63, 0x3a, 0x20, 0x25, 0x75
        .byte   0x0a, 0x00

        .section .rodata_wlog_info.17,"a",@progbits
        .align  1
.Lsec_64_0:
        .byte   0x5b, 0x63, 0x61, 0x6c, 0x63, 0x75, 0x6c, 0x61, 0x74, 0x69, 0x6f, 0x6e, 0x20, 0x25, 0x73, 0x5d
        .byte   0x20, 0x6f, 0x66, 0x66, 0x73, 0x65, 0x74, 0x3a, 0x20, 0x25, 0x6c, 0x75, 0x2c, 0x20, 0x74, 0x69
        .byte   0x6d, 0x65, 0x6f, 0x75, 0x74, 0x3a, 0x20, 0x25, 0x6c, 0x75, 0x2c, 0x20, 0x78, 0x3a, 0x20, 0x25
        .byte   0x2e, 0x34, 0x66, 0x2c, 0x20, 0x70, 0x3a, 0x20, 0x25, 0x2e, 0x34, 0x66, 0x2c, 0x20, 0x62, 0x6c
        .byte   0x3a, 0x20, 0x25, 0x6c, 0x75, 0x00

        .section .rodata_wlog_debug.16,"a",@progbits
        .align  1
.Lsec_65_0:
        .byte   0x25, 0x6c, 0x6c, 0x75, 0x3a, 0x25, 0x6c, 0x75, 0x00

        .section .rodata_wlog_debug.15,"a",@progbits
        .align  1
.Lsec_66_0:
        .byte   0x66, 0x69, 0x72, 0x73, 0x74, 0x20, 0x62, 0x65, 0x61, 0x63, 0x6f, 0x6e, 0x20, 0x75, 0x6e, 0x73
        .byte   0x74, 0x61, 0x62, 0x6c, 0x65, 0x20, 0x70, 0x65, 0x72, 0x69, 0x6f, 0x64, 0x3a, 0x20, 0x25, 0x2e
        .byte   0x34, 0x66, 0x00

        .section .rodata_wlog_info.14,"a",@progbits
        .align  1
.Lsec_67_0:
        .byte   0x73, 0x74, 0x64, 0x20, 0x63, 0x68, 0x61, 0x6e, 0x67, 0x65, 0x3a, 0x20, 0x25, 0x2e, 0x32, 0x66
        .byte   0x20, 0x2d, 0x3e, 0x20, 0x25, 0x2e, 0x32, 0x66, 0x00

        .section .rodata_wlog_info.13,"a",@progbits
        .align  1
.Lsec_68_0:
        .byte   0x62, 0x65, 0x61, 0x63, 0x6f, 0x6e, 0x20, 0x6f, 0x66, 0x66, 0x73, 0x65, 0x74, 0x20, 0x70, 0x65
        .byte   0x72, 0x69, 0x6f, 0x64, 0x3a, 0x20, 0x25, 0x6c, 0x75, 0x2c, 0x20, 0x72, 0x65, 0x73, 0x61, 0x6d
        .byte   0x70, 0x6c, 0x65, 0x20, 0x70, 0x65, 0x72, 0x69, 0x6f, 0x64, 0x3a, 0x20, 0x25, 0x75, 0x2c, 0x20
        .byte   0x73, 0x74, 0x61, 0x6e, 0x64, 0x61, 0x72, 0x64, 0x3a, 0x20, 0x25, 0x2e, 0x32, 0x66, 0x2c, 0x20
        .byte   0x64, 0x69, 0x66, 0x66, 0x65, 0x72, 0x65, 0x6e, 0x63, 0x65, 0x3a, 0x20, 0x25, 0x2e, 0x32, 0x66
        .byte   0x00

        .section .rodata_wlog_error.8,"a",@progbits
        .align  1
.Lsec_69_0:
        .byte   0x6d, 0x20, 0x66, 0x20, 0x62, 0x63, 0x6e, 0x20, 0x73, 0x61, 0x6d, 0x70, 0x6c, 0x65, 0x00

        .section .rodata_wlog_info.7,"a",@progbits
        .align  1
.Lsec_70_0:
        .byte   0x62, 0x63, 0x6e, 0x20, 0x73, 0x61, 0x6d, 0x70, 0x6c, 0x65, 0x20, 0x74, 0x65, 0x72, 0x6d, 0x69
        .byte   0x6e, 0x61, 0x74, 0x65, 0x64, 0x20, 0x64, 0x75, 0x65, 0x20, 0x74, 0x6f, 0x20, 0x74, 0x6f, 0x6f
        .byte   0x20, 0x6c, 0x61, 0x72, 0x67, 0x65, 0x20, 0x62, 0x65, 0x61, 0x63, 0x6f, 0x6e, 0x20, 0x69, 0x6e
        .byte   0x74, 0x65, 0x72, 0x76, 0x61, 0x6c, 0x00

        .section .rodata_wlog_error.5,"a",@progbits
        .align  1
.Lsec_71_0:
        .byte   0x63, 0x61, 0x6c, 0x63, 0x75, 0x6c, 0x61, 0x74, 0x65, 0x20, 0x63, 0x6f, 0x75, 0x6e, 0x74, 0x20
        .byte   0x65, 0x72, 0x72, 0x6f, 0x72, 0x2c, 0x20, 0x25, 0x75, 0x2c, 0x20, 0x25, 0x75, 0x2c, 0x20, 0x25
        .byte   0x75, 0x00

        .section .bss.s_pm_beacon_offset_config,"aw",@nobits
        .align  2
.Lsec_72_0:
        .global s_pm_beacon_offset_config
        .type   s_pm_beacon_offset_config, @object
        .size   s_pm_beacon_offset_config, 6
s_pm_beacon_offset_config:
        .skip   6

        .section .bss.s_pm_beacon_offset,"aw",@nobits
        .align  4
.Lsec_73_0:
        .global s_pm_beacon_offset
        .type   s_pm_beacon_offset, @object
        .size   s_pm_beacon_offset, 76
s_pm_beacon_offset:
        .skip   76
