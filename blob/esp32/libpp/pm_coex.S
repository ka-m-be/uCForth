.section .text.pm_coex_reconnect_policy,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_11_0, g_pm
.Lsec_11_4:
        .global pm_coex_reconnect_policy
        .type   pm_coex_reconnect_policy, @function
pm_coex_reconnect_policy:
        entry    a1, 32
        l32r     a2, .Lsec_11_0
        movi.n   a8, 1
        addmi    a2, a2, 0x100
        l8ui     a2, a2, 201
        xor      a2, a2, a8
        extui    a2, a2, 0, 8
        retw.n
        .size   pm_coex_reconnect_policy, .-pm_coex_reconnect_policy
.Lsec_11_1a:

        .section .text.pm_coex_set_reconnect_policy,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_12_0, g_osi_funcs_p
        .literal .Lsec_12_4, g_pm
        .literal .Lsec_12_8, .Lsec_21_0
        .literal .Lsec_12_c, wifi_log
.Lsec_12_10:
        .global pm_coex_set_reconnect_policy
        .type   pm_coex_set_reconnect_policy, @function
pm_coex_set_reconnect_policy:
        entry    a1, 32
        l32r     a8, .Lsec_12_0
        l32i.n   a8, a8, 0
        l32i     a10, a8, 0x194
        callx8   a10
        beqz.n   a10, .Lsec_12_44
        movi.n   a11, 1
        l32r     a8, .Lsec_12_4
        movi.n   a9, 0
        addmi    a8, a8, 0x100
        movi.n   a12, 3
        slli     a11, a11, 12
        l32r     a13, .Lsec_12_8
        movi     a10, 6
        s8i      a9, a8, 201
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_12_c
        callx8   a8
.Lsec_12_44:
        retw.n
        .size   pm_coex_set_reconnect_policy, .-pm_coex_set_reconnect_policy
.Lsec_12_46:

        .section .wifislprxiram.4,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_13_0, g_osi_funcs_p
.Lsec_13_4:
        .global pm_coex_schm_overall_period_get
        .type   pm_coex_schm_overall_period_get, @function
pm_coex_schm_overall_period_get:
        entry    a1, 32
        l32r     a3, .Lsec_13_0
        l32i.n   a2, a3, 0
        l32i     a10, a2, 0x1c0
        callx8   a10
        mov.n    a2, a10
        l32i.n   a3, a3, 0
        l32i     a10, a3, 0x1d8
        callx8   a10
        maxu     a2, a2, a10
        extui    a2, a2, 0, 8
        retw.n
        .size   pm_coex_schm_overall_period_get, .-pm_coex_schm_overall_period_get
.Lsec_13_24:

        .section .iram1.2,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_14_0, g_pm
        .literal .Lsec_14_4, g_osi_funcs_p
        .literal .Lsec_14_8, 0x00ffff00
        .literal .Lsec_14_c, g_mesh_is_started
        .literal .Lsec_14_10, g_mesh_is_root
        .literal .Lsec_14_14, g_mesh_init_ps_type
        .literal .Lsec_14_18, 0x3ff73c00
        .literal .Lsec_14_1c, g_wifi_mac_time_delta
        .literal .Lsec_14_20, pm_coex_schm_overall_period_get
.Lsec_14_24:
        .global pm_get_null_max_tx_time
        .type   pm_get_null_max_tx_time, @function
pm_get_null_max_tx_time:
        entry    a1, 32
        l32r     a4, .Lsec_14_4
        extui    a6, a2, 0, 8
        l32i     a8, a4, 0
        l32r     a5, .Lsec_14_0
        l32i     a10, a8, 0x194
        l32i     a2, a5, 44
        callx8   a10
        beqz     a10, .Lsec_14_ba
        l32r     a8, .Lsec_14_8
        l32i.n   a9, a5, 12
        and      a9, a9, a8
        movi     a8, 0x101
        slli     a8, a8, 8
        bne      a9, a8, .Lsec_14_ba
        beqz.n   a6, .Lsec_14_58
        l32i     a2, a5, 0x1b0
        j        .Lsec_14_ba
.Lsec_14_58:
        l32r     a2, .Lsec_14_c
        l8ui     a2, a2, 0
        beqz.n   a2, .Lsec_14_81
        l32r     a2, .Lsec_14_10
        memw
        l8ui     a2, a2, 0
        extui    a2, a2, 0, 8
        bnez.n   a2, .Lsec_14_81
        l32r     a2, .Lsec_14_14
        l32i.n   a2, a2, 0
        beqz.n   a2, .Lsec_14_81
        l32i.n   a2, a4, 0
        l32i     a10, a2, 0x110
        callx8   a10
        j        .Lsec_14_90
        .byte    0x00
.Lsec_14_81:
        l32r     a2, .Lsec_14_18
        l32r     a8, .Lsec_14_1c
        memw
        l32i.n   a2, a2, 0
        l32i.n   a8, a8, 0
        add.n    a10, a2, a8
.Lsec_14_90:
        sub      a2, a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_coex_schm_overall_period_get
        l32r     a8, .Lsec_14_20
        callx8   a8
        l32i.n   a3, a5, 44
        mull     a3, a10, a3
        l32i.n   a4, a4, 0
        l32i     a8, a5, 0x1b0
        sub      a3, a3, a8
        l32i     a10, a4, 0x110
        callx8   a10
        l32i     a8, a5, 104
        bgeu     a2, a3, .Lsec_14_ba
        bgeu     a10, a8, .Lsec_14_ba
        add.n    a8, a8, a2
        sub      a2, a8, a10
.Lsec_14_ba:
        retw.n
        .size   pm_get_null_max_tx_time, .-pm_get_null_max_tx_time
.Lsec_14_bc:

        .section .wifislprxiram.5,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_15_0, g_pm
        .literal .Lsec_15_4, g_pm_cfg
        .literal .Lsec_15_8, g_osi_funcs_p
        .literal .Lsec_15_c, g_pm+0x100
.Lsec_15_10:
        .global pm_coex_pwr_update
        .type   pm_coex_pwr_update, @function
pm_coex_pwr_update:
        entry    a1, 32
        l32r     a8, .Lsec_15_0
        l32i.n   a9, a8, 36
        l32r     a10, .Lsec_15_c
        bnez.n   a9, .Lsec_15_29
        s8i      a9, a10, 213
        memw
        memw
        j        .Lsec_15_74
.Lsec_15_29:
        l8ui     a11, a8, 13
        bnez.n   a11, .Lsec_15_3c
        s8i      a11, a10, 213
        memw
        memw
        j        .Lsec_15_74
        .byte    0x00
        .byte    0x00
.Lsec_15_3c:
        l32r     a11, .Lsec_15_4
        l8ui     a11, a11, 50
        s8i      a11, a10, 213
        memw
        beqz.n   a11, .Lsec_15_74
        bnei     a9, 2, .Lsec_15_60
        l32r     a9, .Lsec_15_8
        l32i.n   a9, a9, 0
        l8ui     a10, a8, 32
        l32i     a9, a9, 0x1d4
        memw
        j        .Lsec_15_6e
        .byte    0x00
        .byte    0x00
.Lsec_15_60:
        bnei     a9, 1, .Lsec_15_81
        l32r     a9, .Lsec_15_8
        l32i.n   a9, a9, 0
        l8ui     a10, a8, 11
        l32i     a9, a9, 0x1d4
.Lsec_15_6e:
        callx8   a9
        j        .Lsec_15_81
.Lsec_15_74:
        l32r     a8, .Lsec_15_8
        movi.n   a10, 1
        l32i.n   a8, a8, 0
        l32i     a8, a8, 0x1d4
        callx8   a8
.Lsec_15_81:
        retw.n
        .size   pm_coex_pwr_update, .-pm_coex_pwr_update
.Lsec_15_83:

        .section .text.pm_coex_pwr_configure,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_16_0, g_pm_cfg
        .literal .Lsec_16_4, pm_coex_pwr_update
.Lsec_16_8:
        .global pm_coex_pwr_configure
        .type   pm_coex_pwr_configure, @function
pm_coex_pwr_configure:
        entry    a1, 32
        l32r     a8, .Lsec_16_0
        s8i      a2, a8, 50
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_coex_pwr_update
        l32r     a8, .Lsec_16_4
        callx8   a8
        retw.n
        .size   pm_coex_pwr_configure, .-pm_coex_pwr_configure
.Lsec_16_1f:

        .section .rodata.pm_coex_recalculate_wifi_time_slice.str1.1,"aMS",@progbits,1
        .align  1
.Lsec_17_0:
        .ascii  "pm_coex.c\000"

        .section .text.pm_coex_recalculate_wifi_time_slice,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_18_0, g_osi_funcs_p
        .literal .Lsec_18_4, g_pm
        .literal .Lsec_18_8, g_mesh_is_started
        .literal .Lsec_18_c, g_mesh_is_root
        .literal .Lsec_18_10, g_mesh_init_ps_type
        .literal .Lsec_18_14, 0x3ff73c00
        .literal .Lsec_18_18, g_wifi_mac_time_delta
        .literal .Lsec_18_1c, .Lsec_17_0
        .literal .Lsec_18_20, .Lsec_20_0
        .literal .Lsec_18_24, g_pm+0xf4
        .literal .Lsec_18_28, pm_coex_schm_overall_period_get
        .literal .Lsec_18_2c, __umoddi3
        .literal .Lsec_18_30, wifi_log
        .literal .Lsec_18_34, pm_coex_schm_overall_period_get
.Lsec_18_38:
        .global pm_coex_recalculate_wifi_time_slice
        .type   pm_coex_recalculate_wifi_time_slice, @function
pm_coex_recalculate_wifi_time_slice:
        entry    a1, 48
        l32r     a6, .Lsec_18_0
        extui    a4, a4, 0, 8
        l32i.n   a5, a6, 0
        l32i     a10, a5, 0x194
        callx8   a10
        beqz     a10, .Lsec_18_208
        l32r     a5, .Lsec_18_4
        bnez.n   a4, .Lsec_18_79
        movi.n   a7, 1
        l32i     a10, a5, 96
        l32i     a8, a5, 0x1b0
        add.n    a8, a10, a8
        l32i     a9, a5, 100
        l32i     a11, a5, 0x1b4
        bltu     a8, a10, .Lsec_18_66
        mov.n    a7, a4
.Lsec_18_66:
        add.n    a9, a9, a11
        add.n    a7, a7, a9
        bgeu     a3, a7, .Lsec_18_70
        j        .Lsec_18_208
.Lsec_18_70:
        bne      a7, a3, .Lsec_18_79
        bgeu     a2, a8, .Lsec_18_79
        j        .Lsec_18_208
.Lsec_18_79:
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_coex_schm_overall_period_get
        l32r     a8, .Lsec_18_28
        callx8   a8
        l32i.n   a7, a6, 0
        mov.n    a4, a10
        l32i     a10, a7, 0x1bc
        callx8   a10
        mull     a10, a4, a10
        l32r     a7, .Lsec_18_8
        addx4    a10, a10, a10
        addx4    a10, a10, a10
        slli     a4, a10, 2
        l8ui     a7, a7, 0
        beqz.n   a7, .Lsec_18_c1
        l32r     a7, .Lsec_18_c
        memw
        l8ui     a7, a7, 0
        extui    a7, a7, 0, 8
        bnez.n   a7, .Lsec_18_c1
        l32r     a7, .Lsec_18_10
        l32i.n   a7, a7, 0
        beqz.n   a7, .Lsec_18_c1
        l32i.n   a7, a6, 0
        l32i     a10, a7, 0x110
        callx8   a10
        mov.n    a7, a10
        j        .Lsec_18_d0
        .byte    0x00
        .byte    0x00
.Lsec_18_c1:
        l32r     a7, .Lsec_18_14
        l32r     a8, .Lsec_18_18
        memw
        l32i.n   a7, a7, 0
        l32i.n   a8, a8, 0
        add.n    a7, a7, a8
.Lsec_18_d0:
        movi.n   a11, 1
        l32i     a10, a5, 80
        sub      a10, a7, a10
        l32i     a8, a5, 84
        bltu     a7, a10, .Lsec_18_e0
        movi.n   a11, 0
.Lsec_18_e0:
        neg      a8, a8
        mov.n    a12, a4
        movi.n   a13, 0
        sub      a11, a8, a11
        .reloc  ., R_XTENSA_ASM_EXPAND, __umoddi3
        l32r     a8, .Lsec_18_2c
        callx8   a8
        l32i.n   a8, a6, 0
        mov.n    a7, a10
        movi.n   a10, 0
        mov.n    a4, a11
        l32i     a8, a8, 0x1dc
        callx8   a8
        mov.n    a8, a10
        bnez.n   a10, .Lsec_18_11d
        movi.n   a12, 1
        l32r     a14, .Lsec_18_1c
        movi.n   a15, 88
        slli     a11, a12, 12
        movi     a10, 6
        l32r     a13, .Lsec_18_20
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_18_30
        callx8   a8
.Lsec_18_118:
        j        .Lsec_18_118
        .byte    0x00
        .byte    0x00
.Lsec_18_11d:
        l32i.n   a9, a6, 0
        sub      a7, a2, a7
        l32i     a10, a9, 0x1c0
        s32i.n   a8, a1, 4
        memw
        callx8   a10
        mov.n    a9, a10
        l32i.n   a10, a6, 0
        l32i     a10, a10, 0x1bc
        s32i.n   a9, a1, 0
        memw
        callx8   a10
        l32i.n   a8, a1, 4
        l8ui     a8, a8, 0
        mull     a8, a8, a10
        l32i.n   a9, a1, 0
        mull     a8, a8, a9
        movi.n   a9, 1
        bltu     a2, a7, .Lsec_18_151
        movi     a9, 0
.Lsec_18_151:
        sub      a4, a3, a4
        sub      a4, a4, a9
        add.n    a9, a7, a8
        movi     a11, 1
        bltu     a9, a7, .Lsec_18_162
        movi     a11, 0
.Lsec_18_162:
        add.n    a11, a11, a4
        l8ui     a7, a5, 13
        movi     a4, 0x3e8
        s32i     a9, a5, 96
        s32i     a11, a5, 100
        beqz.n   a7, .Lsec_18_174
        l32i.n   a4, a5, 56
.Lsec_18_174:
        sub      a4, a9, a4
        movi     a7, 1
        bltu     a9, a4, .Lsec_18_180
        movi     a7, 0
.Lsec_18_180:
        sub      a11, a11, a7
        s32i     a4, a5, 104
        s32i     a11, a5, 108
        s32i.n   a8, a1, 4
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_coex_schm_overall_period_get
        l32r     a8, .Lsec_18_34
        callx8   a8
        or       a4, a10, a10
        l32i     a7, a6, 0
        l32i     a10, a7, 0x1bc
        callx8   a10
        mull     a10, a4, a10
        movi.n   a4, 1
        l32i.n   a8, a1, 4
        addx4    a10, a10, a10
        addx4    a10, a10, a10
        slli     a7, a10, 2
        subx4    a10, a10, a8
        bltu     a7, a10, .Lsec_18_1b8
        movi.n   a4, 0
.Lsec_18_1b8:
        l32i.n   a7, a6, 0
        neg      a4, a4
        s32i     a4, a5, 0x1b4
        l32r     a4, .Lsec_18_24
        l32i     a7, a7, 236
        s32i     a10, a5, 0x1b0
        mov.n    a10, a4
        memw
        callx8   a7
        l32i     a7, a5, 108
        l32i     a11, a5, 104
        bltu     a3, a7, .Lsec_18_1e0
        bne      a7, a3, .Lsec_18_200
        bgeu     a2, a11, .Lsec_18_200
.Lsec_18_1e0:
        movi.n   a3, 1
        s8i      a3, a5, 24
        memw
        movi.n   a12, 0
        sub      a11, a11, a2
        or       a10, a4, a4
        l32i     a3, a6, 0
        l32i     a3, a3, 248
        memw
        callx8   a3
        j        .Lsec_18_208
        .byte    0x00
.Lsec_18_200:
        movi.n   a2, 0
        s8i      a2, a5, 24
        memw
.Lsec_18_208:
        memw
        retw.n
        .size   pm_coex_recalculate_wifi_time_slice, .-pm_coex_recalculate_wifi_time_slice
.Lsec_18_20d:

        .section .text.pm_coex_update_wifi_time_slice_state,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_19_0, g_osi_funcs_p
        .literal .Lsec_19_4, g_pm
        .literal .Lsec_19_8, 0x00001387
        .literal .Lsec_19_c, pm_coex_recalculate_wifi_time_slice
.Lsec_19_10:
        .global pm_coex_update_wifi_time_slice_state
        .type   pm_coex_update_wifi_time_slice_state, @function
pm_coex_update_wifi_time_slice_state:
        entry    a1, 32
        extui    a12, a4, 0, 8
        mov.n    a10, a2
        or       a11, a3, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_coex_recalculate_wifi_time_slice
        l32r     a8, .Lsec_19_c
        callx8   a8
        l32r     a4, .Lsec_19_0
        l32i.n   a8, a4, 0
        l32i     a10, a8, 0x194
        callx8   a10
        l32r     a9, .Lsec_19_4
        beqz.n   a10, .Lsec_19_58
        l32i     a8, a9, 108
        l32i     a10, a9, 104
        bltu     a3, a8, .Lsec_19_40
        bne      a8, a3, .Lsec_19_70
        bgeu     a2, a10, .Lsec_19_70
.Lsec_19_40:
        sub      a11, a10, a2
        movi     a12, 1
        bltu     a10, a11, .Lsec_19_4c
        movi     a12, 0
.Lsec_19_4c:
        sub      a8, a8, a3
        bne      a8, a12, .Lsec_19_58
        l32r     a3, .Lsec_19_8
        bgeu     a3, a11, .Lsec_19_70
.Lsec_19_58:
        movi.n   a11, 0
        movi     a10, 1
        l32i     a3, a4, 0
        l32i     a12, a9, 96
        sub      a12, a12, a2
        l32i     a3, a3, 0x19c
        callx8   a3
        j        .Lsec_19_7a
        .byte    0x00
.Lsec_19_70:
        l32i.n   a2, a4, 0
        movi.n   a10, 1
        l32i     a2, a2, 0x1a0
        callx8   a2
.Lsec_19_7a:
        retw.n
        .size   pm_coex_update_wifi_time_slice_state, .-pm_coex_update_wifi_time_slice_state
.Lsec_19_7c:

        .section .rodata_wlog_error.6,"a",@progbits
        .align  1
.Lsec_20_0:
        .byte   0x25, 0x73, 0x20, 0x25, 0x64, 0x00

        .section .rodata_wlog_info.3,"a",@progbits
        .align  1
.Lsec_21_0:
        .byte   0x43, 0x6f, 0x65, 0x78, 0x69, 0x73, 0x74, 0x3a, 0x20, 0x57, 0x69, 0x2d, 0x46, 0x69, 0x20, 0x63
        .byte   0x6f, 0x6e, 0x6e, 0x65, 0x63, 0x74, 0x20, 0x66, 0x61, 0x69, 0x6c, 0x2c, 0x20, 0x61, 0x70, 0x70
        .byte   0x6c, 0x79, 0x20, 0x72, 0x65, 0x63, 0x6f, 0x6e, 0x6e, 0x65, 0x63, 0x74, 0x20, 0x63, 0x6f, 0x65
        .byte   0x78, 0x20, 0x70, 0x6f, 0x6c, 0x69, 0x63, 0x79, 0x0a, 0x00
