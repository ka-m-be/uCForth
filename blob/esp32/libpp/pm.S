.section .text.hal_pm_unblock_txq,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_147_0, 0x3ff73cb8
        .literal .Lsec_147_4, 0xfff1ffff
.Lsec_147_8:
        entry    a1, 32
        l32r     a9, .Lsec_147_0
        l32r     a10, .Lsec_147_4
        memw
        l32i.n   a8, a9, 0
        and      a8, a8, a10
        memw
        s32i.n   a8, a9, 0
        memw
        retw.n

        .section .text.pm_disable_max_idle_timer,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_148_0, g_pm+0x1ec
        .literal .Lsec_148_4, g_osi_funcs_p
.Lsec_148_8:
        entry    a1, 32
        l32r     a8, .Lsec_148_4
        l32i.n   a8, a8, 0
        l32r     a10, .Lsec_148_0
        l32i     a8, a8, 236
        callx8   a8
        retw.n

        .section .text.pm_incr_coex_active_cnt,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_149_0, g_pm
.Lsec_149_4:
        entry    a1, 32
        l32r     a9, .Lsec_149_0
        movi.n   a10, 1
        l32i     a8, a9, 64
        addi.n   a8, a8, 1
        maxu     a8, a8, a10
        s32i     a8, a9, 64
        memw
        retw.n

        .section .text.pm_disable_active_timer,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_150_0, g_pm+0xcc
        .literal .Lsec_150_4, g_osi_funcs_p
.Lsec_150_8:
        entry    a1, 32
        l32r     a8, .Lsec_150_4
        l32i.n   a8, a8, 0
        l32r     a10, .Lsec_150_0
        l32i     a8, a8, 236
        callx8   a8
        retw.n

        .section .text.pm_set_state,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_151_0, g_pm
        .literal .Lsec_151_4, wifi_gpio_debug
        .literal .Lsec_151_8, wifi_gpio_debug
.Lsec_151_c:
        entry    a1, 32
        l32r     a3, .Lsec_151_0
        l8ui     a10, a3, 1
        beq      a10, a2, .Lsec_151_30
        movi.n   a4, 5
        movi.n   a11, 1
        sub      a10, a4, a10
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_gpio_debug
        l32r     a8, .Lsec_151_4
        callx8   a8
        movi.n   a11, 0
        sub      a10, a4, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_gpio_debug
        l32r     a8, .Lsec_151_8
        callx8   a8
.Lsec_151_30:
        s8i      a2, a3, 1
        memw
        memw
        retw.n

        .section .text.pm_beacon_monitor_tbtt_timeout,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_152_0, pp_timer_process
.Lsec_152_4:
        entry    a1, 32
        mov.n    a11, a2
        movi.n   a10, 10
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_timer_process
        l32r     a8, .Lsec_152_0
        callx8   a8
        retw.n

        .section .text.pm_coex_restart_timeout,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_153_0, pp_timer_process
.Lsec_153_4:
        entry    a1, 32
        movi.n   a11, 0
        movi.n   a10, 13
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_timer_process
        l32r     a8, .Lsec_153_0
        callx8   a8
        retw.n

        .section .text.pm_max_idle_timeout,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_154_0, pp_timer_process
.Lsec_154_4:
        entry    a1, 32
        movi.n   a11, 0
        movi.n   a10, 11
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_timer_process
        l32r     a8, .Lsec_154_0
        callx8   a8
        retw.n

        .section .text.pm_beacon_monitor_timeout,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_155_0, pp_timer_process
.Lsec_155_4:
        entry    a1, 32
        mov.n    a11, a2
        movi.n   a10, 9
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_timer_process
        l32r     a8, .Lsec_155_0
        callx8   a8
        retw.n

        .section .text.pm_connectionless_wake_window_timeout,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_156_0, pp_timer_process
.Lsec_156_4:
        entry    a1, 32
        movi.n   a11, 0
        movi.n   a10, 8
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_timer_process
        l32r     a8, .Lsec_156_0
        callx8   a8
        retw.n

        .section .text.pm_connectionless_wake_interval_timeout,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_157_0, pp_timer_process
.Lsec_157_4:
        entry    a1, 32
        movi.n   a11, 0
        movi.n   a10, 7
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_timer_process
        l32r     a8, .Lsec_157_0
        callx8   a8
        retw.n

        .section .text.pm_disconnected_sleep_delay_timeout,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_158_0, pp_timer_process
.Lsec_158_4:
        entry    a1, 32
        movi.n   a11, 0
        movi.n   a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_timer_process
        l32r     a8, .Lsec_158_0
        callx8   a8
        retw.n

        .section .text.pm_coex_slice_wifi_timeout,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_159_0, pp_timer_process
.Lsec_159_4:
        entry    a1, 32
        movi.n   a11, 0
        movi.n   a10, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_timer_process
        l32r     a8, .Lsec_159_0
        callx8   a8
        retw.n

        .section .text.pm_sleep_delay_timeout,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_160_0, pp_timer_process
.Lsec_160_4:
        entry    a1, 32
        movi.n   a11, 0
        movi.n   a10, 5
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_timer_process
        l32r     a8, .Lsec_160_0
        callx8   a8
        retw.n

        .section .text.pm_active_timeout,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_161_0, pp_timer_process
.Lsec_161_4:
        entry    a1, 32
        movi.n   a11, 0
        movi.n   a10, 3
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_timer_process
        l32r     a8, .Lsec_161_0
        callx8   a8
        retw.n

        .section .text.pm_dream_timeout,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_162_0, pp_timer_process
.Lsec_162_4:
        entry    a1, 32
        movi.n   a11, 0
        movi.n   a10, 2
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_timer_process
        l32r     a8, .Lsec_162_0
        callx8   a8
        retw.n

        .section .text.pm_tbtt_timeout,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_163_0, pp_timer_process
.Lsec_163_4:
        entry    a1, 32
        movi.n   a11, 0
        movi.n   a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_timer_process
        l32r     a8, .Lsec_163_0
        callx8   a8
        retw.n

        .section .text.wifi_apb80m_request,"ax",@progbits
        .align  4
.Lsec_164_0:
        .weak   wifi_apb80m_request
        .type   wifi_apb80m_request, @function
wifi_apb80m_request:
        entry    a1, 32
        retw.n
        .size   wifi_apb80m_request, .-wifi_apb80m_request
.Lsec_164_5:

        .section .text.wifi_apb80m_release,"ax",@progbits
        .align  4
.Lsec_165_0:
        .weak   wifi_apb80m_release
        .type   wifi_apb80m_release, @function
wifi_apb80m_release:
        entry    a1, 32
        retw.n
        .size   wifi_apb80m_release, .-wifi_apb80m_release
.Lsec_165_5:

        .section .wifislprxiram.2,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_166_0, g_pm
.Lsec_166_4:
        .global pm_is_waked
        .type   pm_is_waked, @function
pm_is_waked:
        entry    a1, 32
        l32r     a2, .Lsec_166_0
        movi.n   a8, 0
        l8ui     a9, a2, 1
        movi.n   a2, 1
        movnez   a2, a8, a9
        retw.n
        .size   pm_is_waked, .-pm_is_waked
.Lsec_166_16:

        .section .text.pm_is_dream,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_167_0, g_pm
.Lsec_167_4:
        .global pm_is_dream
        .type   pm_is_dream, @function
pm_is_dream:
        entry    a1, 32
        l32r     a2, .Lsec_167_0
        movi.n   a9, 0
        l8ui     a8, a2, 1
        movi.n   a2, 1
        addi.n   a8, a8, -1
        movnez   a2, a9, a8
        retw.n
        .size   pm_is_dream, .-pm_is_dream
.Lsec_167_18:

        .section .text.pm_is_sleeping,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_168_0, g_pm
.Lsec_168_4:
        .global pm_is_sleeping
        .type   pm_is_sleeping, @function
pm_is_sleeping:
        entry    a1, 32
        l32r     a8, .Lsec_168_0
        movi.n   a2, 1
        l8ui     a9, a8, 1
        beqi     a9, 2, .Lsec_168_20
        addmi    a8, a8, 0x100
        movi.n   a9, 0
        l8ui     a8, a8, 29
        addi     a8, a8, -3
        movnez   a2, a9, a8
.Lsec_168_20:
        retw.n
        .size   pm_is_sleeping, .-pm_is_sleeping
.Lsec_168_22:

        .section .text.pm_is_open,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_169_0, g_pm
.Lsec_169_4:
        .global pm_is_open
        .type   pm_is_open, @function
pm_is_open:
        entry    a1, 32
        l32r     a8, .Lsec_169_0
        l8ui     a2, a8, 13
        retw.n
        .size   pm_is_open, .-pm_is_open
.Lsec_169_f:

        .section .wifislprxiram.3,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_170_0, g_mesh_is_started
        .literal .Lsec_170_4, g_mesh_is_root
        .literal .Lsec_170_8, g_mesh_init_ps_type
        .literal .Lsec_170_c, g_pm+0x3
        .literal .Lsec_170_10, g_pm
        .literal .Lsec_170_14, memcmp
        .literal .Lsec_170_18, memcmp
.Lsec_170_1c:
        .global pm_sleep_for
        .type   pm_sleep_for, @function
pm_sleep_for:
        entry    a1, 32
        mov.n    a10, a2
        l32r     a2, .Lsec_170_0
        l8ui     a2, a2, 0
        beqz.n   a2, .Lsec_170_55
        l32r     a2, .Lsec_170_4
        memw
        l8ui     a3, a2, 0
        extui    a3, a3, 0, 8
        bnez.n   a3, .Lsec_170_55
        l32r     a2, .Lsec_170_8
        l32i.n   a2, a2, 0
        beqz.n   a2, .Lsec_170_55
        movi.n   a12, 6
        movi     a2, 1
        l32r     a11, .Lsec_170_c
        .reloc  ., R_XTENSA_ASM_EXPAND, memcmp
        l32r     a8, .Lsec_170_14
        callx8   a8
        movnez   a2, a3, a10
        j        .Lsec_170_72
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_170_55:
        l32r     a2, .Lsec_170_10
        movi.n   a3, 0
        l8ui     a2, a2, 13
        beq      a2, a3, .Lsec_170_70
        movi.n   a12, 6
        movi.n   a2, 1
        l32r     a11, .Lsec_170_c
        .reloc  ., R_XTENSA_ASM_EXPAND, memcmp
        l32r     a8, .Lsec_170_18
        callx8   a8
        moveqz   a3, a2, a10
.Lsec_170_70:
        mov.n    a2, a3
.Lsec_170_72:
        retw.n
        .size   pm_sleep_for, .-pm_sleep_for
.Lsec_170_74:

        .section .text.pm_allow_tx,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_171_0, g_pm
        .literal .Lsec_171_4, g_mesh_is_started
        .literal .Lsec_171_8, g_mesh_is_root
        .literal .Lsec_171_c, g_mesh_init_ps_type
.Lsec_171_10:
        .global pm_allow_tx
        .type   pm_allow_tx, @function
pm_allow_tx:
        entry    a1, 32
        extui    a11, a2, 0, 8
        l32r     a2, .Lsec_171_4
        l32r     a9, .Lsec_171_0
        l8ui     a2, a2, 0
        l8ui     a10, a9, 1
        beqz.n   a2, .Lsec_171_59
        l32r     a12, .Lsec_171_8
        memw
        l8ui     a8, a12, 0
        extui    a8, a8, 0, 8
        bnez.n   a8, .Lsec_171_59
        l32r     a2, .Lsec_171_c
        l32i.n   a2, a2, 0
        beqz.n   a2, .Lsec_171_59
        movi.n   a2, 1
        beqz.n   a10, .Lsec_171_68
        memw
        l8ui     a10, a12, 0
        mov.n    a2, a8
        extui    a10, a10, 0, 8
        beqz.n   a10, .Lsec_171_68
        l8ui     a8, a9, 2
        movi.n   a9, 1
        sub      a8, a8, a11
        movnez   a2, a9, a8
        j        .Lsec_171_68
        .byte    0x00
.Lsec_171_59:
        movi.n   a2, 1
        beqz.n   a10, .Lsec_171_68
        l8ui     a8, a9, 2
        movi.n   a9, 0
        sub      a8, a8, a11
        moveqz   a2, a9, a8
.Lsec_171_68:
        retw.n
        .size   pm_allow_tx, .-pm_allow_tx
.Lsec_171_6a:

        .section .text.pm_allow_to_enable_bcn_filter,"ax",@progbits
        .align  4
.Lsec_172_0:
        .global pm_allow_to_enable_bcn_filter
        .type   pm_allow_to_enable_bcn_filter, @function
pm_allow_to_enable_bcn_filter:
        entry    a1, 32
        movi.n   a2, 0
        retw.n
        .size   pm_allow_to_enable_bcn_filter, .-pm_allow_to_enable_bcn_filter
.Lsec_172_7:

        .section .wifislpiram.4,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_173_0, g_pm
.Lsec_173_4:
        .global pm_get_connectionless_status
        .type   pm_get_connectionless_status, @function
pm_get_connectionless_status:
        entry    a1, 32
        l32r     a10, .Lsec_173_0
        l32i     a8, a10, 0x160
        mov.n    a2, a8
        beqz.n   a8, .Lsec_173_29
        l32i.n   a9, a8, 0
        mov.n    a2, a9
        beqz.n   a9, .Lsec_173_29
        movi.n   a2, -2
        l16ui    a8, a10, 0x18a
        beqz.n   a8, .Lsec_173_29
        l16ui    a2, a9, 0
        sub      a8, a8, a2
        bgei     a8, 6, .Lsec_173_29
        movi.n   a2, -1
.Lsec_173_29:
        retw.n
        .size   pm_get_connectionless_status, .-pm_get_connectionless_status
.Lsec_173_2b:

        .section .iram1.7,"ax",@progbits
        .align  4
.Lsec_174_0:
        .weak   pm_parse_beacon_trace
        .type   pm_parse_beacon_trace, @function
pm_parse_beacon_trace:
        entry    a1, 32
        retw.n
        .size   pm_parse_beacon_trace, .-pm_parse_beacon_trace
.Lsec_174_5:

        .section .text.pm_mac_sleep_init,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_175_0, wdev_mac_sleep_init
.Lsec_175_4:
        .global pm_mac_sleep_init
        .type   pm_mac_sleep_init, @function
pm_mac_sleep_init:
        entry    a1, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_mac_sleep_init
        l32r     a8, .Lsec_175_0
        callx8   a8
        retw.n
        .size   pm_mac_sleep_init, .-pm_mac_sleep_init
.Lsec_175_f:

        .section .text.pm_mac_sleep_deinit,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_176_0, wdev_mac_sleep_deinit
.Lsec_176_4:
        .global pm_mac_sleep_deinit
        .type   pm_mac_sleep_deinit, @function
pm_mac_sleep_deinit:
        entry    a1, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_mac_sleep_deinit
        l32r     a8, .Lsec_176_0
        callx8   a8
        retw.n
        .size   pm_mac_sleep_deinit, .-pm_mac_sleep_deinit
.Lsec_176_f:

        .section .text.pm_enable_max_idle_timer,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_177_0, g_pm_cfg
        .literal .Lsec_177_4, g_pm
        .literal .Lsec_177_8, g_pm+0x1ec
        .literal .Lsec_177_c, g_osi_funcs_p
.Lsec_177_10:
        .global pm_enable_max_idle_timer
        .type   pm_enable_max_idle_timer, @function
pm_enable_max_idle_timer:
        entry    a1, 32
        l32r     a2, .Lsec_177_0
        l8ui     a3, a2, 29
        beqz.n   a3, .Lsec_177_44
        l32r     a3, .Lsec_177_4
        l32i.n   a3, a3, 36
        bnez.n   a3, .Lsec_177_44
        l32r     a4, .Lsec_177_c
        l32i.n   a8, a4, 0
        l16ui    a5, a2, 48
        l32r     a2, .Lsec_177_8
        mov.n    a10, a2
        l32i     a8, a8, 236
        callx8   a8
        mov.n    a12, a3
        slli     a11, a5, 9
        l32i.n   a4, a4, 0
        mov.n    a10, a2
        l32i     a4, a4, 232
        callx8   a4
.Lsec_177_44:
        retw.n
        .size   pm_enable_max_idle_timer, .-pm_enable_max_idle_timer
.Lsec_177_46:

        .section .text.pm_is_sending_keep_alive,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_178_0, g_pm
.Lsec_178_4:
        .global pm_is_sending_keep_alive
        .type   pm_is_sending_keep_alive, @function
pm_is_sending_keep_alive:
        entry    a1, 32
        l32r     a8, .Lsec_178_0
        addmi    a8, a8, 0x100
        l8ui     a2, a8, 232
        retw.n
        .size   pm_is_sending_keep_alive, .-pm_is_sending_keep_alive
.Lsec_178_12:

        .section .text.pm_set_bss_max_idle,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_179_0, g_pm_cfg
        .literal .Lsec_179_4, pm_enable_max_idle_timer
        .literal .Lsec_179_8, .Lsec_148_8
.Lsec_179_c:
        .global pm_set_bss_max_idle
        .type   pm_set_bss_max_idle, @function
pm_set_bss_max_idle:
        entry    a1, 32
        extui    a2, a2, 0, 16
        l32r     a8, .Lsec_179_0
        beqz.n   a2, .Lsec_179_41
        movi.n   a9, 1
        s8i      a9, a8, 29
        memw
        bltui    a2, 5, .Lsec_179_2d
        addi     a9, a2, -4
        add.n    a9, a9, a9
        max      a2, a9, a2
        extui    a2, a2, 0, 16
.Lsec_179_2d:
        s16i     a2, a8, 48
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_enable_max_idle_timer
        l32r     a8, .Lsec_179_4
        callx8   a8
        j        .Lsec_179_50
        .byte    0x00
        .byte    0x00
.Lsec_179_41:
        s8i      a2, a8, 29
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, .Lsec_148_8
        l32r     a8, .Lsec_179_8
        callx8   a8
.Lsec_179_50:
        retw.n
        .size   pm_set_bss_max_idle, .-pm_set_bss_max_idle
.Lsec_179_52:

        .section .text.pm_noise_check_disable,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_180_0, g_pm
.Lsec_180_4:
        .global pm_noise_check_disable
        .type   pm_noise_check_disable, @function
pm_noise_check_disable:
        entry    a1, 32
        l32r     a8, .Lsec_180_0
        movi.n   a9, 0
        s8i      a9, a8, 21
        memw
        memw
        retw.n
        .size   pm_noise_check_disable, .-pm_noise_check_disable
.Lsec_180_17:

        .section .text.pm_noise_check_enable,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_181_0, g_pm
.Lsec_181_4:
        .global pm_noise_check_enable
        .type   pm_noise_check_enable, @function
pm_noise_check_enable:
        entry    a1, 32
        l32r     a8, .Lsec_181_0
        movi.n   a9, 1
        s8i      a9, a8, 21
        memw
        memw
        retw.n
        .size   pm_noise_check_enable, .-pm_noise_check_enable
.Lsec_181_17:

        .section .wifirxiram.19,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_182_0, 0x002dc6c0
        .literal .Lsec_182_4, g_pm_cfg
        .literal .Lsec_182_8, g_pm
        .literal .Lsec_182_c, g_pm+0xcc
        .literal .Lsec_182_10, g_osi_funcs_p
        .literal .Lsec_182_14, g_mesh_is_started
        .literal .Lsec_182_18, g_mesh_is_root
        .literal .Lsec_182_1c, g_mesh_init_ps_type
.Lsec_182_20:
        .global pm_enable_active_timer
        .type   pm_enable_active_timer, @function
pm_enable_active_timer:
        entry    a1, 32
        l32r     a5, .Lsec_182_4
        l8ui     a2, a5, 29
        l32r     a3, .Lsec_182_8
        beqz.n   a2, .Lsec_182_34
        l32i     a2, a3, 36
        beqz     a2, .Lsec_182_9c
.Lsec_182_34:
        l32r     a4, .Lsec_182_10
        l32i.n   a8, a4, 0
        l32r     a2, .Lsec_182_c
        or       a10, a2, a2
        l32i     a8, a8, 236
        callx8   a8
        l32i.n   a9, a3, 36
        l32r     a8, .Lsec_182_14
        bnez.n   a9, .Lsec_182_74
        l8ui     a3, a8, 0
        l32i.n   a11, a5, 16
        beqz.n   a3, .Lsec_182_64
        l32r     a3, .Lsec_182_18
        memw
        l8ui     a3, a3, 0
        extui    a3, a3, 0, 8
        bnez.n   a3, .Lsec_182_64
        l32r     a11, .Lsec_182_0
.Lsec_182_64:
        movi.n   a12, 0
        mov.n    a10, a2
        l32i.n   a3, a4, 0
        l32i     a3, a3, 248
        callx8   a3
        j        .Lsec_182_9c
        .byte    0x00
.Lsec_182_74:
        l8ui     a5, a8, 0
        beqz.n   a5, .Lsec_182_8e
        l32r     a5, .Lsec_182_18
        memw
        l8ui     a8, a5, 0
        extui    a8, a8, 0, 8
        bnez.n   a8, .Lsec_182_8e
        l32r     a5, .Lsec_182_1c
        l32i.n   a5, a5, 0
        bnez.n   a5, .Lsec_182_9c
.Lsec_182_8e:
        movi.n   a12, 0
        mov.n    a10, a2
        l32i.n   a4, a4, 0
        l32i.n   a11, a3, 48
        l32i     a4, a4, 248
        callx8   a4
.Lsec_182_9c:
        retw.n
        .size   pm_enable_active_timer, .-pm_enable_active_timer
.Lsec_182_9e:

        .section .text.pm_reset_keep_alive,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_183_0, g_pm
        .literal .Lsec_183_4, g_osi_funcs_p
        .literal .Lsec_183_8, pm_enable_active_timer
.Lsec_183_c:
        entry    a1, 32
        l32r     a2, .Lsec_183_0
        l8ui     a3, a2, 13
        beqz.n   a3, .Lsec_183_40
        l32r     a3, .Lsec_183_4
        l32i.n   a8, a3, 0
        l32i     a10, a8, 0x110
        callx8   a10
        s32i     a10, a2, 72
        s32i     a11, a2, 76
        l32i.n   a2, a2, 36
        bnez.n   a2, .Lsec_183_40
        l32i     a2, a3, 0
        l32i     a10, a2, 0x194
        memw
        callx8   a10
        bnez.n   a10, .Lsec_183_40
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_enable_active_timer
        l32r     a8, .Lsec_183_8
        callx8   a8
.Lsec_183_40:
        retw.n

        .section .wifislpiram.20,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_184_0, g_pm+0xb8
        .literal .Lsec_184_4, g_osi_funcs_p
.Lsec_184_8:
        .global pm_disable_dream_timer
        .type   pm_disable_dream_timer, @function
pm_disable_dream_timer:
        entry    a1, 32
        l32r     a8, .Lsec_184_4
        l32i.n   a8, a8, 0
        l32r     a10, .Lsec_184_0
        l32i     a8, a8, 236
        callx8   a8
        retw.n
        .size   pm_disable_dream_timer, .-pm_disable_dream_timer
.Lsec_184_1b:

        .section .wifislpiram.21,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_185_0, g_pm+0xb8
        .literal .Lsec_185_4, g_osi_funcs_p
        .literal .Lsec_185_8, g_pm
.Lsec_185_c:
        .global pm_enable_dream_timer
        .type   pm_enable_dream_timer, @function
pm_enable_dream_timer:
        entry    a1, 32
        l32r     a3, .Lsec_185_4
        l32i     a8, a3, 0
        l32r     a2, .Lsec_185_0
        mov.n    a10, a2
        l32i     a8, a8, 236
        callx8   a8
        movi.n   a12, 0
        mov.n    a10, a2
        l32r     a8, .Lsec_185_8
        l32i.n   a3, a3, 0
        l32i     a11, a8, 0x1c4
        l32i     a3, a3, 248
        callx8   a3
        retw.n
        .size   pm_enable_dream_timer, .-pm_enable_dream_timer
.Lsec_185_34:

        .section .wifislprxiram.22,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_186_0, g_pm
        .literal .Lsec_186_4, g_pm+0x108
        .literal .Lsec_186_8, g_osi_funcs_p
.Lsec_186_c:
        .global pm_disable_sleep_delay_timer
        .type   pm_disable_sleep_delay_timer, @function
pm_disable_sleep_delay_timer:
        entry    a1, 32
        l32r     a8, .Lsec_186_0
        movi.n   a9, 0
        addmi    a8, a8, 0x100
        s8i      a9, a8, 184
        memw
        l32r     a8, .Lsec_186_8
        l32i.n   a8, a8, 0
        l32r     a10, .Lsec_186_4
        l32i     a8, a8, 236
        memw
        callx8   a8
        retw.n
        .size   pm_disable_sleep_delay_timer, .-pm_disable_sleep_delay_timer
.Lsec_186_30:

        .section .text.pm_enable_sleep_delay_timer,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_187_0, g_pm+0x108
        .literal .Lsec_187_4, g_osi_funcs_p
        .literal .Lsec_187_8, g_pm
.Lsec_187_c:
        .global pm_enable_sleep_delay_timer
        .type   pm_enable_sleep_delay_timer, @function
pm_enable_sleep_delay_timer:
        entry    a1, 32
        l32r     a4, .Lsec_187_4
        l32i     a2, a4, 0
        l32r     a3, .Lsec_187_0
        mov.n    a10, a3
        l32i     a2, a2, 236
        callx8   a2
        mov.n    a10, a3
        movi.n   a12, 0
        l32r     a2, .Lsec_187_8
        movi.n   a3, 1
        l32i     a4, a4, 0
        l32i     a11, a2, 56
        addmi    a2, a2, 0x100
        l32i     a4, a4, 248
        callx8   a4
        s8i      a3, a2, 184
        memw
        memw
        retw.n
        .size   pm_enable_sleep_delay_timer, .-pm_enable_sleep_delay_timer
.Lsec_187_43:

        .section .text.pm_disable_disconnected_sleep_delay_timer,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_188_0, g_pm
        .literal .Lsec_188_4, g_pm+0x124
        .literal .Lsec_188_8, g_osi_funcs_p
.Lsec_188_c:
        .global pm_disable_disconnected_sleep_delay_timer
        .type   pm_disable_disconnected_sleep_delay_timer, @function
pm_disable_disconnected_sleep_delay_timer:
        entry    a1, 32
        l32r     a8, .Lsec_188_0
        movi.n   a9, 0
        addmi    a8, a8, 0x100
        s8i      a9, a8, 184
        memw
        l32r     a8, .Lsec_188_8
        l32i.n   a8, a8, 0
        l32r     a10, .Lsec_188_4
        l32i     a8, a8, 236
        memw
        callx8   a8
        retw.n
        .size   pm_disable_disconnected_sleep_delay_timer, .-pm_disable_disconnected_sleep_delay_timer
.Lsec_188_30:

        .section .text.pm_enable_disconnected_sleep_delay_timer,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_189_0, g_pm
        .literal .Lsec_189_4, g_mesh_is_started
        .literal .Lsec_189_8, g_pm+0x124
        .literal .Lsec_189_c, g_osi_funcs_p
.Lsec_189_10:
        .global pm_enable_disconnected_sleep_delay_timer
        .type   pm_enable_disconnected_sleep_delay_timer, @function
pm_enable_disconnected_sleep_delay_timer:
        entry    a1, 32
        l32r     a2, .Lsec_189_0
        addmi    a2, a2, 0x100
        l8ui     a3, a2, 29
        bnei     a3, 2, .Lsec_189_52
        l32r     a3, .Lsec_189_4
        l8ui     a4, a3, 0
        bnez     a4, .Lsec_189_52
        l32r     a5, .Lsec_189_c
        l32i     a8, a5, 0
        l32r     a3, .Lsec_189_8
        mov.n    a10, a3
        l32i     a8, a8, 236
        callx8   a8
        mov.n    a10, a3
        mov.n    a12, a4
        l32i     a5, a5, 0
        movi     a11, 0x3e8
        movi     a3, 1
        l32i     a5, a5, 248
        callx8   a5
        s8i      a3, a2, 184
        memw
.Lsec_189_52:
        memw
        retw.n
        .size   pm_enable_disconnected_sleep_delay_timer, .-pm_enable_disconnected_sleep_delay_timer
.Lsec_189_57:

        .section .text.pm_noise_check,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_190_0, g_pm
        .literal .Lsec_190_4, wDevCtrl
        .literal .Lsec_190_8, noise_check_loop
        .literal .Lsec_190_c, read_hw_noisefloor
.Lsec_190_10:
        .global pm_noise_check
        .type   pm_noise_check, @function
pm_noise_check:
        entry    a1, 32
        l32r     a9, .Lsec_190_0
        movi.n   a2, -2
        l8ui     a8, a9, 21
        beqz     a8, .Lsec_190_70
        l32i.n   a2, a9, 36
        l32i.n   a8, a9, 44
        beqz.n   a2, .Lsec_190_36
        bnei     a2, 1, .Lsec_190_2d
        l8ui     a2, a9, 11
        j        .Lsec_190_33
.Lsec_190_2d:
        bnei     a2, 2, .Lsec_190_36
        l16ui    a2, a9, 32
.Lsec_190_33:
        mull     a8, a2, a8
.Lsec_190_36:
        l16ui    a2, a9, 28
        movi.n   a9, 1
        slli     a9, a9, 18
        add.n    a9, a8, a9
        quos     a9, a9, a8
        rems     a2, a2, a9
        bnez.n   a2, .Lsec_190_6e
        movi     a11, 1
        or       a10, a11, a11
        .reloc  ., R_XTENSA_ASM_EXPAND, noise_check_loop
        l32r     a8, .Lsec_190_8
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, read_hw_noisefloor
        l32r     a8, .Lsec_190_c
        callx8   a8
        l32r     a8, .Lsec_190_4
        addi.n   a10, a10, 2
        srai     a10, a10, 2
        s8i      a10, a8, 47
        memw
        memw
        j        .Lsec_190_70
.Lsec_190_6e:
        movi.n   a2, 0
.Lsec_190_70:
        retw.n
        .size   pm_noise_check, .-pm_noise_check
.Lsec_190_72:

        .section .text.pm_pend_noise_check,"ax",@progbits
        .align  4
.Lsec_191_0:
        .global pm_pend_noise_check
        .type   pm_pend_noise_check, @function
pm_pend_noise_check:
        entry    a1, 32
        retw.n
        .size   pm_pend_noise_check, .-pm_pend_noise_check
.Lsec_191_5:

        .section .text.pm_noise_check_process,"ax",@progbits
        .align  4
.Lsec_192_0:
        .global pm_noise_check_process
        .type   pm_noise_check_process, @function
pm_noise_check_process:
        entry    a1, 32
        retw.n
        .size   pm_noise_check_process, .-pm_noise_check_process
.Lsec_192_5:

        .section .text.pm_register_pm_tx_null_cb,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_193_0, .Lsec_330_0
.Lsec_193_4:
        .global pm_register_pm_tx_null_cb
        .type   pm_register_pm_tx_null_cb, @function
pm_register_pm_tx_null_cb:
        entry    a1, 32
        l32r     a8, .Lsec_193_0
        s32i.n   a2, a8, 0
        movi.n   a2, 1
        memw
        retw.n
        .size   pm_register_pm_tx_null_cb, .-pm_register_pm_tx_null_cb
.Lsec_193_13:

        .section .text.pm_send_nullfunc,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_194_0, g_pm
        .literal .Lsec_194_4, .Lsec_330_0
        .literal .Lsec_194_8, wifi_gpio_debug
.Lsec_194_c:
        .global pm_send_nullfunc
        .type   pm_send_nullfunc, @function
pm_send_nullfunc:
        entry    a1, 32
        l32r     a3, .Lsec_194_0
        extui    a2, a2, 0, 8
        movi.n   a8, 1
        l8ui     a5, a3, 20
        l8ui     a4, a3, 19
        beqz.n   a2, .Lsec_194_30
        s8i      a8, a3, 20
        movi.n   a8, 0
        s8i      a8, a3, 19
        memw
        memw
        j        .Lsec_194_39
.Lsec_194_30:
        s8i      a8, a3, 19
        s8i      a2, a3, 20
        memw
.Lsec_194_39:
        addi.n   a11, a2, 2
        movi.n   a10, 6
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_gpio_debug
        l32r     a8, .Lsec_194_8
        callx8   a8
        movi.n   a12, 0
        l32r     a8, .Lsec_194_4
        mov.n    a11, a12
        mov.n    a10, a2
        l32i     a8, a8, 0
        callx8   a8
        bnez.n   a10, .Lsec_194_60
        s8i      a5, a3, 20
        s8i      a4, a3, 19
        memw
.Lsec_194_60:
        memw
        retw.n
        .size   pm_send_nullfunc, .-pm_send_nullfunc
.Lsec_194_65:

        .section .text.pm_mac_wakeup,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_195_0, wdev_mac_wakeup
.Lsec_195_4:
        .global pm_mac_wakeup
        .type   pm_mac_wakeup, @function
pm_mac_wakeup:
        entry    a1, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_mac_wakeup
        l32r     a8, .Lsec_195_0
        callx8   a8
        retw.n
        .size   pm_mac_wakeup, .-pm_mac_wakeup
.Lsec_195_f:

        .section .text.pm_mac_sleep,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_196_0, wdev_mac_sleep
.Lsec_196_4:
        .global pm_mac_sleep
        .type   pm_mac_sleep, @function
pm_mac_sleep:
        entry    a1, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_mac_sleep
        l32r     a8, .Lsec_196_0
        callx8   a8
        retw.n
        .size   pm_mac_sleep, .-pm_mac_sleep
.Lsec_196_f:

        .section .wifislprxiram.24,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_197_0, g_osi_funcs_p
        .literal .Lsec_197_4, g_pm
        .literal .Lsec_197_8, __udivdi3
        .literal .Lsec_197_c, pm_coex_schm_overall_period_get
.Lsec_197_10:
        .global pm_get_tbtt_count
        .type   pm_get_tbtt_count, @function
pm_get_tbtt_count:
        entry    a1, 32
        l32r     a5, .Lsec_197_0
        l32i.n   a4, a5, 0
        l32i     a10, a4, 0x110
        callx8   a10
        movi.n   a8, 1
        l32r     a4, .Lsec_197_4
        l32i.n   a12, a4, 44
        l32i     a9, a4, 88
        add.n    a9, a12, a9
        l32i     a6, a4, 92
        bltu     a9, a12, .Lsec_197_32
        movi.n   a8, 0
.Lsec_197_32:
        add.n    a13, a8, a6
        movi.n   a8, 1
        l32i.n   a6, a4, 40
        add.n    a10, a6, a10
        bltu     a10, a6, .Lsec_197_40
        movi     a8, 0
.Lsec_197_40:
        sub      a10, a9, a10
        add.n    a8, a8, a11
        movi.n   a11, 1
        bltu     a9, a10, .Lsec_197_4c
        movi.n   a11, 0
.Lsec_197_4c:
        sub      a8, a13, a8
        sub      a11, a8, a11
        movi.n   a13, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, __udivdi3
        l32r     a8, .Lsec_197_8
        callx8   a8
        l32i.n   a5, a5, 0
        or       a6, a10, a10
        l32i     a10, a5, 0x194
        callx8   a10
        beqz.n   a10, .Lsec_197_78
        movi.n   a10, 1
        l8ui     a4, a4, 14
        beqz.n   a4, .Lsec_197_8c
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_coex_schm_overall_period_get
        l32r     a8, .Lsec_197_c
        callx8   a8
        j        .Lsec_197_8c
        .byte    0x00
.Lsec_197_78:
        l32i.n   a5, a4, 36
        bnei     a5, 1, .Lsec_197_84
        l8ui     a10, a4, 11
        j        .Lsec_197_8c
        .byte    0x00
.Lsec_197_84:
        movi.n   a10, 1
        bnei     a5, 2, .Lsec_197_8c
        l16ui    a10, a4, 32
.Lsec_197_8c:
        s32i.n   a10, a3, 0
        movi.n   a3, 0
        bltu     a10, a6, .Lsec_197_99
        sub      a3, a10, a6
        remu     a3, a3, a10
.Lsec_197_99:
        s32i.n   a3, a2, 0
        memw
        retw.n
        .size   pm_get_tbtt_count, .-pm_get_tbtt_count
.Lsec_197_a0:

        .section .wifislprxiram.25,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_198_0, pm_get_tbtt_count
.Lsec_198_4:
        .global pm_save_tbtt_info
        .type   pm_save_tbtt_info, @function
pm_save_tbtt_info:
        entry    a1, 48
        mov.n    a11, a1
        addi.n   a10, a1, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_get_tbtt_count
        l32r     a8, .Lsec_198_0
        callx8   a8
        l32i.n   a8, a1, 4
        addmi    a2, a2, 0x100
        s8i      a8, a2, 214
        memw
        l32i.n   a8, a1, 0
        s8i      a8, a2, 215
        memw
        memw
        retw.n
        .size   pm_save_tbtt_info, .-pm_save_tbtt_info
.Lsec_198_29:

        .section .wifislprxiram.26,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_199_0, g_pm_cfg
        .literal .Lsec_199_4, g_pm
.Lsec_199_8:
        .global pm_beacon_miss_exceeded_wakeup_disabled
        .type   pm_beacon_miss_exceeded_wakeup_disabled, @function
pm_beacon_miss_exceeded_wakeup_disabled:
        entry    a1, 32
        l32r     a2, .Lsec_199_0
        l8ui     a8, a2, 51
        movi.n   a2, 1
        beqi     a8, 2, .Lsec_199_24
        movi.n   a2, 0
        bnei     a8, 1, .Lsec_199_24
        l32r     a2, .Lsec_199_4
        addmi    a2, a2, 0x200
        l8ui     a2, a2, 28
.Lsec_199_24:
        extui    a2, a2, 0, 1
        retw.n
        .size   pm_beacon_miss_exceeded_wakeup_disabled, .-pm_beacon_miss_exceeded_wakeup_disabled
.Lsec_199_29:

        .section .wifislprxiram.27,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_200_0, ptr_beacon_offset_funcs
        .literal .Lsec_200_4, pm_beacon_miss_exceeded_wakeup_disabled
.Lsec_200_8:
        .global pm_beacon_monitor_tbtt_allowed
        .type   pm_beacon_monitor_tbtt_allowed, @function
pm_beacon_monitor_tbtt_allowed:
        entry    a1, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_beacon_miss_exceeded_wakeup_disabled
        l32r     a8, .Lsec_200_4
        callx8   a8
        addmi    a8, a2, 0x100
        movi.n   a3, 1
        beqz.n   a10, .Lsec_200_25
        l16si    a9, a2, 0x182
        addi.n   a9, a9, -1
        l8ui     a10, a8, 214
        blt      a10, a9, .Lsec_200_25
        movi.n   a3, 0
.Lsec_200_25:
        l8ui     a9, a8, 100
        bnez.n   a9, .Lsec_200_31
.Lsec_200_2a:
        movi.n   a3, 0
        j        .Lsec_200_58
        .byte    0x00
        .byte    0x00
.Lsec_200_31:
        l8ui     a9, a8, 212
        bnez     a9, .Lsec_200_2a
        l8ui     a2, a2, 13
        beqz     a2, .Lsec_200_2a
        l8ui     a2, a8, 214
        l8ui     a8, a8, 215
        addi.n   a8, a8, -1
        beq      a2, a8, .Lsec_200_2a
        l32r     a2, .Lsec_200_0
        l32i.n   a2, a2, 0
        l32i.n   a10, a2, 12
        beqz     a10, .Lsec_200_58
        callx8   a10
        bnez     a10, .Lsec_200_2a
.Lsec_200_58:
        extui    a2, a3, 0, 1
        retw.n
        .size   pm_beacon_monitor_tbtt_allowed, .-pm_beacon_monitor_tbtt_allowed
.Lsec_200_5d:

        .section .wifislprxiram.28,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_201_0, ptr_beacon_offset_funcs
        .literal .Lsec_201_4, g_osi_funcs_p
        .literal .Lsec_201_8, 0x00001387
        .literal .Lsec_201_c, g_pm
        .literal .Lsec_201_10, __udivdi3
.Lsec_201_14:
        .global pm_beacon_monitor_tbtt_start
        .type   pm_beacon_monitor_tbtt_start, @function
pm_beacon_monitor_tbtt_start:
        entry    a1, 48
        l32r     a3, .Lsec_201_0
        l32i.n   a3, a3, 0
        l32i.n   a10, a3, 16
        bnez.n   a10, .Lsec_201_35
.Lsec_201_20:
        addmi    a3, a2, 0x100
        l8ui     a5, a3, 126
        slli     a5, a5, 5
        l16si    a3, a2, 0x180
        mull     a5, a5, a3
        j        .Lsec_201_3d
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_201_35:
        callx8   a10
        beqz     a10, .Lsec_201_20
        movi.n   a5, 0
.Lsec_201_3d:
        l32r     a6, .Lsec_201_4
        l32i     a3, a6, 0
        l32i     a10, a3, 0x110
        callx8   a10
        mov.n    a8, a10
        or       a7, a11, a11
        l32i     a4, a2, 92
        l32i     a3, a2, 88
        bltu     a11, a4, .Lsec_201_5d
        bne      a4, a11, .Lsec_201_a4
        bgeu     a10, a3, .Lsec_201_a4
.Lsec_201_5d:
        sub      a10, a3, a10
        l32i.n   a9, a2, 44
        movi.n   a14, 1
        bltu     a3, a10, .Lsec_201_69
        movi.n   a14, 0
.Lsec_201_69:
        sub      a11, a4, a11
        mov.n    a12, a9
        movi.n   a13, 0
        sub      a11, a11, a14
        s32i     a8, a1, 4
        s32i.n   a9, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, __udivdi3
        l32r     a8, .Lsec_201_10
        callx8   a8
        l32i.n   a9, a1, 0
        mull     a12, a10, a9
        mull     a11, a11, a9
        muluh    a10, a10, a9
        sub      a9, a3, a12
        add.n    a10, a11, a10
        l32i.n   a8, a1, 4
        movi     a11, 1
        bltu     a3, a9, .Lsec_201_9c
        movi     a11, 0
.Lsec_201_9c:
        sub      a10, a4, a10
        mov.n    a3, a9
        sub      a4, a10, a11
.Lsec_201_a4:
        l32r     a13, .Lsec_201_8
        movi.n   a10, 1
        movi.n   a11, 0
        j        .Lsec_201_bd
.Lsec_201_ae:
        l32i.n   a9, a2, 44
        mov.n    a12, a10
        add.n    a9, a3, a9
        bltu     a9, a3, .Lsec_201_b9
        mov.n    a12, a11
.Lsec_201_b9:
        mov.n    a3, a9
        add.n    a4, a12, a4
.Lsec_201_bd:
        bltu     a7, a4, .Lsec_201_c6
        bne      a4, a7, .Lsec_201_ae
        bgeu     a8, a3, .Lsec_201_ae
.Lsec_201_c6:
        sub      a9, a3, a8
        mov.n    a12, a10
        bltu     a3, a9, .Lsec_201_d0
        mov.n    a12, a11
.Lsec_201_d0:
        sub      a14, a4, a7
        bne      a14, a12, .Lsec_201_d9
        bgeu     a13, a9, .Lsec_201_ae
.Lsec_201_d9:
        l16si    a4, a2, 0x182
        sub      a3, a3, a5
        addi.n   a4, a4, -1
        l16si    a5, a2, 0x180
        bne      a5, a4, .Lsec_201_f0
        l32r     a4, .Lsec_201_c
        l32i     a4, a4, 0x1cc
        sub      a3, a3, a4
.Lsec_201_f0:
        l32i.n   a4, a6, 0
        l32i     a10, a4, 0x110
        l32i     a2, a2, 0x184
        l32i     a5, a4, 248
        callx8   a10
        sub      a11, a3, a10
        movi.n   a12, 0
        mov.n    a10, a2
        callx8   a5
        retw.n
        .size   pm_beacon_monitor_tbtt_start, .-pm_beacon_monitor_tbtt_start
.Lsec_201_10a:

        .section .wifislprxiram.29,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_202_0, g_osi_funcs_p
.Lsec_202_4:
        .global pm_beacon_monitor_tbtt_stop
        .type   pm_beacon_monitor_tbtt_stop, @function
pm_beacon_monitor_tbtt_stop:
        entry    a1, 32
        l32r     a8, .Lsec_202_0
        l32i.n   a8, a8, 0
        l32i     a10, a2, 0x184
        l32i     a8, a8, 236
        callx8   a8
        retw.n
        .size   pm_beacon_monitor_tbtt_stop, .-pm_beacon_monitor_tbtt_stop
.Lsec_202_17:

        .section .wifislprxiram.31,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_203_0, g_osi_funcs_p
        .literal .Lsec_203_4, pm_beacon_monitor_tbtt_stop
.Lsec_203_8:
        .global pm_disable_beacon_monitor_timer
        .type   pm_disable_beacon_monitor_timer, @function
pm_disable_beacon_monitor_timer:
        entry    a1, 32
        addmi    a3, a2, 0x100
        l8ui     a4, a3, 100
        beqz.n   a4, .Lsec_203_3e
        l8ui     a4, a3, 212
        bnez.n   a4, .Lsec_203_3e
        l8ui     a8, a2, 13
        beqz.n   a8, .Lsec_203_3e
        movi     a10, 0x168
        add.n    a10, a2, a10
        l32r     a8, .Lsec_203_0
        l32i.n   a8, a8, 0
        l32i     a8, a8, 236
        callx8   a8
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_beacon_monitor_tbtt_stop
        l32r     a8, .Lsec_203_4
        callx8   a8
        s16i     a4, a2, 0x180
        s8i      a4, a3, 125
        memw
.Lsec_203_3e:
        memw
        retw.n
        .size   pm_disable_beacon_monitor_timer, .-pm_disable_beacon_monitor_timer
.Lsec_203_43:

        .section .text.pm_beacon_monitor_init,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_204_0, g_pm_cfg
        .literal .Lsec_204_4, g_pm
        .literal .Lsec_204_8, .Lsec_305_0
        .literal .Lsec_204_c, .Lsec_152_4
        .literal .Lsec_204_10, g_osi_funcs_p
.Lsec_204_14:
        .global pm_beacon_monitor_init
        .type   pm_beacon_monitor_init, @function
pm_beacon_monitor_init:
        entry    a1, 32
        l32r     a9, .Lsec_204_0
        l32r     a12, .Lsec_204_4
        addmi    a8, a12, 0x100
        l8ui     a10, a9, 36
        s8i      a10, a8, 100
        memw
        beqz.n   a10, .Lsec_204_51
        l8ui     a10, a9, 37
        s8i      a10, a8, 124
        movi.n   a10, 0
        s8i      a10, a8, 125
        memw
        l8ui     a10, a9, 38
        s16i     a10, a12, 0x182
        memw
        l8ui     a10, a9, 39
        l8ui     a9, a9, 40
        s8i      a10, a8, 126
        s8i      a9, a8, 127
        memw
.Lsec_204_51:
        l32r     a8, .Lsec_204_10
        l32i.n   a8, a8, 0
        l32r     a10, .Lsec_204_8
        l32i     a8, a8, 244
        l32r     a11, .Lsec_204_c
        s32i     a10, a12, 0x184
        memw
        callx8   a8
        retw.n
        .size   pm_beacon_monitor_init, .-pm_beacon_monitor_init
.Lsec_204_6a:

        .section .wifislprxiram.32,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_205_0, g_pm
        .literal .Lsec_205_4, g_mesh_is_started
        .literal .Lsec_205_8, g_mesh_is_root
        .literal .Lsec_205_c, g_mesh_init_ps_type
        .literal .Lsec_205_10, g_osi_funcs_p
        .literal .Lsec_205_14, g_pm_cnt
        .literal .Lsec_205_18, g_pm+0xf4
        .literal .Lsec_205_1c, wifi_rf_phy_enable
        .literal .Lsec_205_20, esp_wifi_get_mode
        .literal .Lsec_205_24, wifi_rf_phy_enable
        .literal .Lsec_205_28, .Lsec_151_c
.Lsec_205_2c:
        .global pm_dream
        .type   pm_dream, @function
pm_dream:
        entry    a1, 48
        l32r     a2, .Lsec_205_0
        l8ui     a3, a2, 1
        beqi     a3, 2, .Lsec_205_3b
        j        .Lsec_205_f9
.Lsec_205_3b:
        movi     a10, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_rf_phy_enable
        l32r     a8, .Lsec_205_1c
        callx8   a8
        l32r     a3, .Lsec_205_4
        l8ui     a3, a3, 0
        beqz.n   a3, .Lsec_205_7d
        l32r     a3, .Lsec_205_8
        memw
        l8ui     a3, a3, 0
        extui    a3, a3, 0, 8
        bnez.n   a3, .Lsec_205_7d
        l32r     a4, .Lsec_205_c
        l32i.n   a4, a4, 0
        beqz.n   a4, .Lsec_205_7d
        mov.n    a10, a1
        s32i.n   a3, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_wifi_get_mode
        l32r     a8, .Lsec_205_20
        callx8   a8
        bnez.n   a10, .Lsec_205_7d
        l32i.n   a3, a1, 0
        bnei     a3, 3, .Lsec_205_7d
        movi.n   a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_rf_phy_enable
        l32r     a8, .Lsec_205_24
        callx8   a8
.Lsec_205_7d:
        l32r     a3, .Lsec_205_10
        l32i.n   a3, a3, 0
        l32i     a10, a3, 0x110
        callx8   a10
        mov.n    a5, a10
        l32r     a8, .Lsec_205_14
        l32i.n   a3, a8, 44
        addi.n   a3, a3, 1
        s32i.n   a3, a8, 44
        l32i     a3, a2, 112
        s32i     a10, a2, 120
        sub      a10, a10, a3
        movi.n   a3, 1
        s32i     a11, a2, 124
        l32i     a4, a2, 116
        bltu     a5, a10, .Lsec_205_a9
        movi.n   a3, 0
.Lsec_205_a9:
        sub      a9, a11, a4
        sub      a9, a9, a3
        l32i.n   a5, a8, 48
        movi.n   a3, 1
        add.n    a4, a5, a10
        l32i.n   a11, a8, 52
        bltu     a4, a5, .Lsec_205_bd
        movi     a3, 0
.Lsec_205_bd:
        add.n    a11, a11, a9
        add.n    a3, a3, a11
        l32i     a5, a8, 64
        s32i.n   a4, a8, 48
        add.n    a4, a5, a10
        s32i.n   a3, a8, 52
        movi.n   a3, 1
        l32i     a11, a8, 68
        bltu     a4, a5, .Lsec_205_d4
        movi.n   a3, 0
.Lsec_205_d4:
        add.n    a11, a11, a9
        add.n    a3, a3, a11
        s32i     a4, a8, 64
        l32i     a4, a2, 136
        add.n    a10, a4, a10
        s32i     a3, a8, 68
        movi.n   a3, 1
        l32i     a8, a2, 140
        bltu     a10, a4, .Lsec_205_ee
        movi     a3, 0
.Lsec_205_ee:
        add.n    a8, a8, a9
        add      a3, a3, a8
        s32i     a10, a2, 136
        s32i     a3, a2, 140
.Lsec_205_f9:
        addmi    a3, a2, 0x100
        l8ui     a3, a3, 213
        beqz     a3, .Lsec_205_159
        l32r     a3, .Lsec_205_10
        l32i     a4, a3, 0
        l32i     a10, a4, 0x194
        memw
        callx8   a10
        beqz     a10, .Lsec_205_159
        l32i.n   a4, a3, 0
        l32i     a10, a4, 0x110
        callx8   a10
        mov.n    a5, a10
        l32i     a4, a2, 108
        bltu     a11, a4, .Lsec_205_12d
        bne      a4, a11, .Lsec_205_151
        l32i     a4, a2, 104
        bgeu     a10, a4, .Lsec_205_151
.Lsec_205_12d:
        l32i.n   a8, a3, 0
        l32r     a4, .Lsec_205_18
        mov.n    a10, a4
        l32i     a8, a8, 236
        callx8   a8
        movi.n   a12, 0
        mov.n    a10, a4
        l32i     a11, a2, 104
        sub      a11, a11, a5
        l32i.n   a2, a3, 0
        l32i     a2, a2, 248
        callx8   a2
        j        .Lsec_205_159
        .byte    0x00
        .byte    0x00
.Lsec_205_151:
        movi.n   a3, 0
        s8i      a3, a2, 24
        memw
.Lsec_205_159:
        movi.n   a10, 1
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, .Lsec_151_c
        l32r     a8, .Lsec_205_28
        callx8   a8
        retw.n
        .size   pm_dream, .-pm_dream
.Lsec_205_166:

        .section .text.pm_disconnected_wake,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_206_0, g_pm
        .literal .Lsec_206_4, g_mesh_is_started
        .literal .Lsec_206_8, wifi_rf_phy_enable
        .literal .Lsec_206_c, .Lsec_151_c
        .literal .Lsec_206_10, wifi_gpio_debug
        .literal .Lsec_206_14, wifi_gpio_debug
        .literal .Lsec_206_18, wifi_gpio_debug
.Lsec_206_1c:
        .global pm_disconnected_wake
        .type   pm_disconnected_wake, @function
pm_disconnected_wake:
        entry    a1, 32
        l32r     a3, .Lsec_206_0
        addmi    a3, a3, 0x100
        l8ui     a4, a3, 29
        bnei     a4, 3, .Lsec_206_6e
        l32r     a2, .Lsec_206_4
        l8ui     a2, a2, 0
        bnez.n   a2, .Lsec_206_6e
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_rf_phy_enable
        l32r     a8, .Lsec_206_8
        callx8   a8
        movi     a8, 2
        mov.n    a10, a2
        s8i      a8, a3, 29
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, .Lsec_151_c
        l32r     a8, .Lsec_206_c
        callx8   a8
        movi.n   a11, 1
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_gpio_debug
        l32r     a8, .Lsec_206_10
        callx8   a8
        movi.n   a11, 1
        movi.n   a10, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_gpio_debug
        l32r     a8, .Lsec_206_14
        callx8   a8
        mov.n    a11, a2
        movi.n   a10, 5
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_gpio_debug
        l32r     a8, .Lsec_206_18
        callx8   a8
.Lsec_206_6e:
        retw.n
        .size   pm_disconnected_wake, .-pm_disconnected_wake
.Lsec_206_70:

        .section .wifislprxiram.33,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_207_0, g_pm
        .literal .Lsec_207_4, g_mesh_is_started
        .literal .Lsec_207_8, g_mesh_is_root
        .literal .Lsec_207_c, g_mesh_init_ps_type
        .literal .Lsec_207_10, g_osi_funcs_p
        .literal .Lsec_207_14, g_pm+0xf4
        .literal .Lsec_207_18, g_pm_cnt
        .literal .Lsec_207_1c, ppCheckTxQIdle
        .literal .Lsec_207_20, ppCheckTxIdle
        .literal .Lsec_207_24, wifi_rf_phy_disable
        .literal .Lsec_207_28, esp_wifi_get_mode
        .literal .Lsec_207_2c, wifi_rf_phy_disable
        .literal .Lsec_207_30, .Lsec_151_c
.Lsec_207_34:
        .global pm_sleep
        .type   pm_sleep, @function
pm_sleep:
        entry    a1, 48
        l32r     a3, .Lsec_207_4
        l32r     a2, .Lsec_207_0
        l8ui     a4, a3, 0
        l8ui     a8, a2, 16
        beqz.n   a4, .Lsec_207_84
        l32r     a4, .Lsec_207_8
        memw
        l8ui     a4, a4, 0
        extui    a4, a4, 0, 8
        bnez.n   a4, .Lsec_207_84
        l32r     a4, .Lsec_207_c
        l32i.n   a4, a4, 0
        beqz.n   a4, .Lsec_207_84
        bnez.n   a8, .Lsec_207_60
        j        .Lsec_207_86
        .byte    0x00
.Lsec_207_60:
        l8ui     a4, a2, 18
        bnez.n   a4, .Lsec_207_86
        l8ui     a4, a2, 19
        bnez.n   a4, .Lsec_207_86
        l8ui     a4, a2, 20
        bnez.n   a4, .Lsec_207_86
        l8ui     a4, a2, 14
        beqz.n   a4, .Lsec_207_86
        l32i     a4, a2, 0x120
        bnez.n   a4, .Lsec_207_86
        movi.n   a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, ppCheckTxQIdle
        l32r     a8, .Lsec_207_1c
        callx8   a8
        j        .Lsec_207_be
.Lsec_207_84:
        bnez.n   a8, .Lsec_207_8c
.Lsec_207_86:
        movi.n   a8, 0
        j        .Lsec_207_dc
        .byte    0x00
.Lsec_207_8c:
        l8ui     a4, a2, 17
        bnez     a4, .Lsec_207_86
        l8ui     a4, a2, 18
        bnez     a4, .Lsec_207_86
        l8ui     a4, a2, 19
        bnez     a4, .Lsec_207_86
        l8ui     a4, a2, 20
        bnez     a4, .Lsec_207_86
        l8ui     a4, a2, 14
        beqz     a4, .Lsec_207_86
        l8ui     a4, a2, 15
        bnez     a4, .Lsec_207_86
        l32i     a4, a2, 0x120
        bnez     a4, .Lsec_207_86
        movi.n   a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, ppCheckTxIdle
        l32r     a8, .Lsec_207_20
        callx8   a8
.Lsec_207_be:
        movi.n   a8, 1
        beqz.n   a10, .Lsec_207_dc
        l32r     a4, .Lsec_207_10
        l32i.n   a4, a4, 0
        l32i     a10, a4, 0x194
        callx8   a10
        beqz     a10, .Lsec_207_86
        movi     a4, 1
        l8ui     a8, a2, 24
        xor      a8, a8, a4
        extui    a8, a8, 0, 8
.Lsec_207_dc:
        extui    a8, a8, 0, 1
        beqz     a8, .Lsec_207_1dd
        l32r     a4, .Lsec_207_10
        l32i.n   a8, a4, 0
        l32i     a10, a8, 0x110
        callx8   a10
        l32i     a8, a4, 0
        l32i     a8, a8, 0x1a0
        s32i     a11, a2, 116
        s32i     a10, a2, 112
        movi     a10, 1
        memw
        callx8   a8
        movi.n   a10, 2
        l32i.n   a8, a4, 0
        l32i     a8, a8, 0x1a0
        callx8   a8
        movi.n   a10, 38
        l32i.n   a8, a4, 0
        l32i     a8, a8, 0x1a0
        callx8   a8
        movi.n   a10, 0
        l32i.n   a8, a4, 0
        l32i     a8, a8, 0x1a0
        callx8   a8
        addmi    a8, a2, 0x100
        l8ui     a8, a8, 213
        beqz.n   a8, .Lsec_207_13d
        l32i.n   a8, a4, 0
        l32i     a10, a8, 0x194
        callx8   a10
        beqz.n   a10, .Lsec_207_13d
        l32i.n   a4, a4, 0
        l32r     a10, .Lsec_207_14
        l32i     a4, a4, 236
        callx8   a4
.Lsec_207_13d:
        movi.n   a10, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_rf_phy_disable
        l32r     a8, .Lsec_207_24
        callx8   a8
        l8ui     a3, a3, 0
        beqz.n   a3, .Lsec_207_17d
        l32r     a3, .Lsec_207_8
        memw
        l8ui     a3, a3, 0
        extui    a3, a3, 0, 8
        bnez.n   a3, .Lsec_207_17d
        l32r     a4, .Lsec_207_c
        l32i.n   a4, a4, 0
        beqz.n   a4, .Lsec_207_17d
        mov.n    a10, a1
        s32i     a3, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_wifi_get_mode
        l32r     a8, .Lsec_207_28
        callx8   a8
        bnez.n   a10, .Lsec_207_17d
        l32i.n   a3, a1, 0
        bnei     a3, 3, .Lsec_207_17d
        movi     a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_rf_phy_disable
        l32r     a8, .Lsec_207_2c
        callx8   a8
.Lsec_207_17d:
        l32r     a3, .Lsec_207_18
        l32i.n   a4, a3, 40
        addi.n   a4, a4, 1
        l32i     a8, a2, 120
        l32i     a9, a2, 112
        l32i     a10, a2, 124
        s32i.n   a4, a3, 40
        l32i     a4, a2, 116
        sub      a2, a9, a8
        movi.n   a8, 1
        bltu     a9, a2, .Lsec_207_19d
        movi     a8, 0
.Lsec_207_19d:
        l32i.n   a11, a3, 56
        sub      a4, a4, a10
        add.n    a10, a11, a2
        sub      a4, a4, a8
        movi.n   a8, 1
        l32i.n   a9, a3, 60
        bltu     a10, a11, .Lsec_207_1b1
        movi     a8, 0
.Lsec_207_1b1:
        add.n    a9, a9, a4
        add.n    a8, a8, a9
        s32i.n   a10, a3, 56
        l32i     a10, a3, 64
        add.n    a2, a10, a2
        s32i.n   a8, a3, 60
        movi.n   a8, 1
        l32i     a9, a3, 68
        bltu     a2, a10, .Lsec_207_1c8
        movi.n   a8, 0
.Lsec_207_1c8:
        add.n    a4, a9, a4
        add.n    a8, a8, a4
        movi.n   a10, 2
        s32i     a2, a3, 64
        s32i     a8, a3, 68
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, .Lsec_151_c
        l32r     a8, .Lsec_207_30
        callx8   a8
.Lsec_207_1dd:
        retw.n
        .size   pm_sleep, .-pm_sleep
.Lsec_207_1df:

        .section .text.pm_disconnected_sleep,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_208_0, g_pm
        .literal .Lsec_208_4, g_osi_funcs_p
        .literal .Lsec_208_8, g_mesh_is_started
        .literal .Lsec_208_c, ppCheckTxIdle
        .literal .Lsec_208_10, wifi_rf_phy_disable
        .literal .Lsec_208_14, wifi_gpio_debug
        .literal .Lsec_208_18, wifi_gpio_debug
        .literal .Lsec_208_1c, wifi_gpio_debug
.Lsec_208_20:
        .global pm_disconnected_sleep
        .type   pm_disconnected_sleep, @function
pm_disconnected_sleep:
        entry    a1, 32
        movi     a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, ppCheckTxIdle
        l32r     a8, .Lsec_208_c
        callx8   a8
        bnez.n   a10, .Lsec_208_39
.Lsec_208_2e:
        l32r     a2, .Lsec_208_0
        l32i     a3, a2, 0x120
        beqz.n   a3, .Lsec_208_54
        j        .Lsec_208_aa
.Lsec_208_39:
        l32r     a2, .Lsec_208_4
        l32i.n   a2, a2, 0
        l32i     a10, a2, 0x194
        callx8   a10
        beqz     a10, .Lsec_208_aa
        l32r     a2, .Lsec_208_0
        l8ui     a2, a2, 24
        bnez     a2, .Lsec_208_aa
        j        .Lsec_208_2e
        .byte    0x00
.Lsec_208_54:
        l32r     a3, .Lsec_208_8
        l8ui     a3, a3, 0
        bnez     a3, .Lsec_208_aa
        movi.n   a10, 1
        addmi    a2, a2, 0x100
        l32r     a4, .Lsec_208_4
        l32i     a8, a4, 0
        l32i     a8, a8, 0x1a0
        callx8   a8
        movi.n   a10, 38
        l32i     a4, a4, 0
        l32i     a4, a4, 0x1a0
        callx8   a4
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_rf_phy_disable
        l32r     a8, .Lsec_208_10
        callx8   a8
        mov.n    a11, a3
        movi.n   a10, 3
        movi.n   a4, 3
        s8i      a4, a2, 29
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_gpio_debug
        l32r     a8, .Lsec_208_14
        callx8   a8
        movi.n   a11, 1
        movi.n   a10, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_gpio_debug
        l32r     a8, .Lsec_208_18
        callx8   a8
        movi.n   a11, 1
        movi.n   a10, 5
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_gpio_debug
        l32r     a8, .Lsec_208_1c
        callx8   a8
.Lsec_208_aa:
        retw.n
        .size   pm_disconnected_sleep, .-pm_disconnected_sleep
.Lsec_208_ac:

        .section .text.is_off_channel,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_209_0, g_pm
.Lsec_209_4:
        .global is_off_channel
        .type   is_off_channel, @function
is_off_channel:
        entry    a1, 32
        l32r     a2, .Lsec_209_0
        movi.n   a8, 1
        l8ui     a2, a2, 14
        xor      a2, a2, a8
        extui    a2, a2, 0, 8
        retw.n
        .size   is_off_channel, .-is_off_channel
.Lsec_209_17:

        .section .text.pm_enable_send_wake_null_on_channel,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_210_0, g_pm
.Lsec_210_4:
        .global pm_enable_send_wake_null_on_channel
        .type   pm_enable_send_wake_null_on_channel, @function
pm_enable_send_wake_null_on_channel:
        entry    a1, 32
        l32r     a8, .Lsec_210_0
        movi.n   a9, 1
        addmi    a8, a8, 0x100
        s8i      a9, a8, 28
        memw
        memw
        retw.n
        .size   pm_enable_send_wake_null_on_channel, .-pm_enable_send_wake_null_on_channel
.Lsec_210_1a:

        .section .text.pm_offchan_is_waiting,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_211_0, .Lsec_329_0
.Lsec_211_4:
        .global pm_offchan_is_waiting
        .type   pm_offchan_is_waiting, @function
pm_offchan_is_waiting:
        entry    a1, 32
        l32r     a2, .Lsec_211_0
        movi.n   a8, 0
        l32i.n   a9, a2, 0
        movi.n   a2, 1
        moveqz   a2, a8, a9
        retw.n
        .size   pm_offchan_is_waiting, .-pm_offchan_is_waiting
.Lsec_211_15:

        .section .text.pm_offchan_execute_ready_cb,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_212_0, .Lsec_329_0
        .literal .Lsec_212_4, .Lsec_316_0
        .literal .Lsec_212_8, .Lsec_328_0
        .literal .Lsec_212_c, wifi_log
.Lsec_212_10:
        .global pm_offchan_execute_ready_cb
        .type   pm_offchan_execute_ready_cb, @function
pm_offchan_execute_ready_cb:
        entry    a1, 32
        l32r     a3, .Lsec_212_0
        l32i.n   a14, a3, 0
        beqz.n   a14, .Lsec_212_42
        movi.n   a11, 1
        movi.n   a12, 4
        l32r     a13, .Lsec_212_4
        slli     a11, a11, 12
        movi.n   a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_212_c
        callx8   a8
        movi.n   a9, 0
        l32r     a8, .Lsec_212_8
        extui    a11, a2, 0, 8
        l32i.n   a12, a3, 0
        l32i.n   a10, a8, 0
        s32i.n   a9, a3, 0
        s32i.n   a9, a8, 0
        memw
        callx8   a12
.Lsec_212_42:
        retw.n
        .size   pm_offchan_execute_ready_cb, .-pm_offchan_execute_ready_cb
.Lsec_212_44:

        .section .text.pm_wake_up,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_213_0, g_pm
        .literal .Lsec_213_4, pm_disable_sleep_delay_timer
        .literal .Lsec_213_8, pm_dream
        .literal .Lsec_213_c, pm_disable_disconnected_sleep_delay_timer
        .literal .Lsec_213_10, pm_disconnected_wake
.Lsec_213_14:
        .global pm_wake_up
        .type   pm_wake_up, @function
pm_wake_up:
        entry    a1, 32
        l32r     a2, .Lsec_213_0
        l8ui     a8, a2, 13
        beqz.n   a8, .Lsec_213_34
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_disable_sleep_delay_timer
        l32r     a8, .Lsec_213_4
        callx8   a8
        l8ui     a8, a2, 1
        bnei     a8, 2, .Lsec_213_40
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_dream
        l32r     a8, .Lsec_213_8
        callx8   a8
        j        .Lsec_213_40
.Lsec_213_34:
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_disable_disconnected_sleep_delay_timer
        l32r     a8, .Lsec_213_c
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_disconnected_wake
        l32r     a8, .Lsec_213_10
        callx8   a8
.Lsec_213_40:
        l32i     a8, a2, 0x120
        addi.n   a8, a8, 1
        s32i     a8, a2, 0x120
        memw
        retw.n
        .size   pm_wake_up, .-pm_wake_up
.Lsec_213_4d:

        .section .text.pm_send_probe_start,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_214_0, g_pm
        .literal .Lsec_214_4, .Lsec_150_8
        .literal .Lsec_214_8, pm_wake_up
.Lsec_214_c:
        .global pm_send_probe_start
        .type   pm_send_probe_start, @function
pm_send_probe_start:
        entry    a1, 32
        l32r     a2, .Lsec_214_0
        addmi    a8, a2, 0x100
        l8ui     a8, a8, 29
        bgeui    a8, 2, .Lsec_214_21
        l8ui     a8, a2, 13
        beqz     a8, .Lsec_214_3a
.Lsec_214_21:
        l8ui     a8, a2, 15
        bnez.n   a8, .Lsec_214_3a
        .reloc  ., R_XTENSA_ASM_EXPAND, .Lsec_150_8
        l32r     a8, .Lsec_214_4
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_wake_up
        l32r     a8, .Lsec_214_8
        callx8   a8
        movi.n   a8, 1
        s8i      a8, a2, 15
        memw
.Lsec_214_3a:
        memw
        retw.n
        .size   pm_send_probe_start, .-pm_send_probe_start
.Lsec_214_3f:

        .section .text.pm_off_channel,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_215_0, g_pm
        .literal .Lsec_215_4, g_osi_funcs_p
        .literal .Lsec_215_8, 0x3ff73cb8
        .literal .Lsec_215_c, .Lsec_150_8
        .literal .Lsec_215_10, pm_wake_up
        .literal .Lsec_215_14, pm_dream
        .literal .Lsec_215_18, pm_send_nullfunc
        .literal .Lsec_215_1c, pm_dream
        .literal .Lsec_215_20, pm_wake_up
.Lsec_215_24:
        .global pm_off_channel
        .type   pm_off_channel, @function
pm_off_channel:
        entry    a1, 32
        l32r     a2, .Lsec_215_0
        addmi    a3, a2, 0x100
        l8ui     a4, a3, 29
        bgeui    a4, 2, .Lsec_215_38
        l8ui     a4, a2, 13
        beqz.n   a4, .Lsec_215_3d
.Lsec_215_38:
        l8ui     a4, a2, 14
        bnez.n   a4, .Lsec_215_4b
.Lsec_215_3d:
        movi.n   a3, 0
        s8i      a3, a2, 14
        memw
        memw
        j        .Lsec_215_107
.Lsec_215_4b:
        .reloc  ., R_XTENSA_ASM_EXPAND, .Lsec_150_8
        l32r     a8, .Lsec_215_c
        callx8   a8
        l8ui     a4, a2, 13
        bnez.n   a4, .Lsec_215_69
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_wake_up
        l32r     a8, .Lsec_215_10
        callx8   a8
        s8i      a4, a2, 14
        memw
        memw
        j        .Lsec_215_107
        .byte    0x00
.Lsec_215_69:
        addmi    a5, a2, 0x200
        movi.n   a6, 1
        l8ui     a4, a2, 1
        s8i      a6, a5, 30
        memw
        bnez     a4, .Lsec_215_c9
        l32r     a4, .Lsec_215_4
        l32i.n   a4, a4, 0
        l32i     a10, a4, 0x194
        memw
        callx8   a10
        beqz.n   a10, .Lsec_215_8d
        s8i      a6, a2, 25
.Lsec_215_8d:
        movi.n   a4, 1
        s8i      a4, a3, 28
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_dream
        l32r     a8, .Lsec_215_14
        callx8   a8
        movi.n   a4, 7
        slli     a4, a4, 17
        l32r     a8, .Lsec_215_8
        memw
        l32i.n   a3, a8, 0
        or       a3, a3, a4
        memw
        s32i.n   a3, a8, 0
.Lsec_215_b3:
        l8ui     a3, a2, 20
        bnez.n   a3, .Lsec_215_f6
        movi     a10, 1
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_send_nullfunc
        l32r     a8, .Lsec_215_18
        callx8   a8
        j        .Lsec_215_f6
        .byte    0x00
        .byte    0x00
.Lsec_215_c9:
        bnei     a4, 1, .Lsec_215_f0
        l8ui     a6, a2, 19
        beqz.n   a6, .Lsec_215_f6
        l32r     a6, .Lsec_215_4
        l32i.n   a6, a6, 0
        l32i     a10, a6, 0x194
        callx8   a10
        beqz.n   a10, .Lsec_215_e1
        s8i      a4, a2, 25
.Lsec_215_e1:
        movi.n   a4, 1
        s8i      a4, a3, 28
        memw
        memw
        j        .Lsec_215_b3
        .byte    0x00
.Lsec_215_f0:
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_dream
        l32r     a8, .Lsec_215_1c
        callx8   a8
.Lsec_215_f6:
        movi.n   a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_wake_up
        l32r     a8, .Lsec_215_20
        callx8   a8
        s8i      a3, a2, 14
        s8i      a3, a5, 30
        memw
.Lsec_215_107:
        memw
        retw.n
        .size   pm_off_channel, .-pm_off_channel
.Lsec_215_10c:

        .section .text.pm_offchan_ready_or_defer,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_216_0, g_pm
        .literal .Lsec_216_4, .Lsec_329_0
        .literal .Lsec_216_8, .Lsec_328_0
        .literal .Lsec_216_c, .Lsec_317_0
        .literal .Lsec_216_10, pm_off_channel
        .literal .Lsec_216_14, wifi_log
.Lsec_216_18:
        .global pm_offchan_ready_or_defer
        .type   pm_offchan_ready_or_defer, @function
pm_offchan_ready_or_defer:
        entry    a1, 32
        l32r     a4, .Lsec_216_0
        movi     a8, 2
        l8ui     a9, a4, 14
        beqz.n   a9, .Lsec_216_64
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_off_channel
        l32r     a8, .Lsec_216_10
        callx8   a8
        movi.n   a8, 0
        l8ui     a9, a4, 13
        beq      a9, a8, .Lsec_216_64
        l8ui     a9, a4, 20
        beq      a9, a8, .Lsec_216_4d
        movi.n   a8, 1
        l32r     a4, .Lsec_216_4
        s32i.n   a2, a4, 0
        l32r     a4, .Lsec_216_8
        s32i.n   a3, a4, 0
        memw
        j        .Lsec_216_64
        .byte    0x00
.Lsec_216_4d:
        l8ui     a4, a4, 1
        bnez.n   a4, .Lsec_216_64
        movi.n   a12, 1
        slli     a11, a12, 12
        l32r     a13, .Lsec_216_c
        movi.n   a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_216_14
        callx8   a8
        movi.n   a8, 2
.Lsec_216_64:
        mov.n    a2, a8
        retw.n
        .size   pm_offchan_ready_or_defer, .-pm_offchan_ready_or_defer
.Lsec_216_68:

        .section .text.pm_wake_done,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_217_0, g_pm
        .literal .Lsec_217_4, pm_sleep
        .literal .Lsec_217_8, pm_disconnected_sleep
.Lsec_217_c:
        .global pm_wake_done
        .type   pm_wake_done, @function
pm_wake_done:
        entry    a1, 32
        l32r     a8, .Lsec_217_0
        l32i     a9, a8, 0x120
        beqz.n   a9, .Lsec_217_1c
        addi.n   a9, a9, -1
        s32i     a9, a8, 0x120
.Lsec_217_1c:
        l8ui     a10, a8, 13
        l32i     a9, a8, 0x120
        beqz.n   a10, .Lsec_217_40
        bnez.n   a9, .Lsec_217_56
        l8ui     a9, a8, 1
        bnei     a9, 1, .Lsec_217_56
        addmi    a8, a8, 0x100
        l8ui     a8, a8, 184
        bnez.n   a8, .Lsec_217_56
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_sleep
        l32r     a8, .Lsec_217_4
        callx8   a8
        j        .Lsec_217_56
.Lsec_217_40:
        bnez.n   a9, .Lsec_217_56
        addmi    a8, a8, 0x100
        l8ui     a9, a8, 29
        bnei     a9, 2, .Lsec_217_56
        l8ui     a8, a8, 184
        bnez.n   a8, .Lsec_217_56
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_disconnected_sleep
        l32r     a8, .Lsec_217_8
        callx8   a8
.Lsec_217_56:
        retw.n
        .size   pm_wake_done, .-pm_wake_done
.Lsec_217_58:

        .section .text.pm_coex_separate_connectionless_window,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_218_0, g_osi_funcs_p
        .literal .Lsec_218_4, g_pm
        .literal .Lsec_218_8, g_pm+0x14c
        .literal .Lsec_218_c, g_pm+0x138
        .literal .Lsec_218_10, pm_coex_schm_overall_period_get
        .literal .Lsec_218_14, pm_wake_up
        .literal .Lsec_218_18, wifi_gpio_debug
        .literal .Lsec_218_1c, wifi_gpio_debug
        .literal .Lsec_218_20, pm_wake_done
.Lsec_218_24:
        .global pm_coex_separate_connectionless_window
        .type   pm_coex_separate_connectionless_window, @function
pm_coex_separate_connectionless_window:
        entry    a1, 64
        l32r     a3, .Lsec_218_0
        l32i.n   a2, a3, 0
        l32i     a10, a2, 0x194
        callx8   a10
        beqz     a10, .Lsec_218_2da
        l32r     a6, .Lsec_218_4
        addmi    a4, a6, 0x100
        l8ui     a2, a4, 29
        bgeui    a2, 2, .Lsec_218_47
        l8ui     a2, a6, 13
        beqz     a2, .Lsec_218_2da
.Lsec_218_47:
        l32i     a2, a3, 0
        l32i     a10, a2, 0x110
        callx8   a10
        mov.n    a5, a10
        mov.n    a13, a11
        l8ui     a2, a4, 136
        beqz     a2, .Lsec_218_ab
        l16ui    a2, a6, 0x18a
        beqz     a2, .Lsec_218_ab
        movi     a7, 1
        l32i     a2, a3, 0
        l32r     a10, .Lsec_218_8
        l32i     a2, a2, 236
        s32i     a11, a1, 16
        memw
        callx8   a2
        l32i     a9, a6, 0x1a8
        l32i     a8, a6, 0x1a0
        add.n    a8, a9, a8
        l32i     a2, a6, 0x1ac
        l32i     a10, a6, 0x1a4
        l32i.n   a13, a1, 16
        bltu     a8, a9, .Lsec_218_8a
        movi.n   a7, 0
.Lsec_218_8a:
        add.n    a2, a2, a10
        add.n    a2, a7, a2
        sub      a7, a8, a5
        movi.n   a9, 1
        bltu     a8, a7, .Lsec_218_98
        movi.n   a9, 0
.Lsec_218_98:
        sub      a2, a2, a13
        sub      a2, a2, a9
        bgez     a2, .Lsec_218_a5
        movi.n   a7, 0
        mov.n    a2, a7
.Lsec_218_a5:
        s32i     a7, a6, 0x1a8
        s32i     a2, a6, 0x1ac
.Lsec_218_ab:
        l32i     a7, a6, 0x1a8
        l32i     a8, a6, 0x1ac
        or       a9, a7, a8
        l32i     a14, a6, 96
        l32i     a2, a6, 100
        beqz     a9, .Lsec_218_2b9
        l16ui    a12, a6, 0x18a
        bnez     a12, .Lsec_218_14c
        l32i.n   a8, a3, 0
        l32r     a7, .Lsec_218_c
        mov.n    a10, a7
        l32i     a8, a8, 236
        s32i.n   a13, a1, 16
        s32i.n   a14, a1, 8
        s32i     a12, a1, 4
        memw
        callx8   a8
        l32i.n   a8, a3, 0
        l32i     a8, a8, 248
        s32i.n   a8, a1, 12
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_coex_schm_overall_period_get
        l32r     a8, .Lsec_218_10
        callx8   a8
        mov.n    a11, a10
        l32i.n   a9, a3, 0
        l32i     a10, a9, 0x1bc
        s32i.n   a11, a1, 0
        memw
        callx8   a10
        l32i.n   a11, a1, 0
        mull     a10, a11, a10
        addx4    a10, a10, a10
        addx4    a10, a10, a10
        l32i.n   a12, a1, 4
        slli     a11, a10, 2
        mov.n    a10, a7
        l32i     a8, a1, 12
        callx8   a8
        l32i.n   a13, a1, 16
        l32i.n   a14, a1, 8
        bltu     a13, a2, .Lsec_218_124
        beq      a2, a13, .Lsec_218_11e
        j        .Lsec_218_2b9
.Lsec_218_11e:
        bltu     a5, a14, .Lsec_218_124
        j        .Lsec_218_2b9
.Lsec_218_124:
        movi.n   a2, 1
        l32i     a7, a6, 96
        sub      a5, a7, a5
        l32i     a8, a6, 100
        bltu     a7, a5, .Lsec_218_134
        movi.n   a2, 0
.Lsec_218_134:
        sub      a8, a8, a13
        sub      a8, a8, a2
        l32i.n   a2, a3, 0
        mov.n    a7, a5
        mov.n    a12, a5
        movi.n   a11, 0
        movi.n   a10, 1
        l32i     a2, a2, 0x19c
        j        .Lsec_218_260
        .byte    0x00
        .byte    0x00
.Lsec_218_14c:
        bltu     a13, a2, .Lsec_218_155
        bne      a2, a13, .Lsec_218_181
        bgeu     a5, a14, .Lsec_218_181
.Lsec_218_155:
        sub      a12, a14, a5
        movi     a5, 1
        bltu     a14, a12, .Lsec_218_161
        movi     a5, 0
.Lsec_218_161:
        sub      a2, a2, a13
        sub      a2, a2, a5
        bltu     a2, a8, .Lsec_218_170
        bne      a8, a2, .Lsec_218_172
        bgeu     a12, a7, .Lsec_218_172
.Lsec_218_170:
        mov.n    a7, a12
.Lsec_218_172:
        l32i.n   a2, a3, 0
        movi.n   a11, 0
        movi.n   a10, 1
        l32i     a2, a2, 0x19c
        j        .Lsec_218_201
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_218_181:
        slli     a9, a12, 5
        l32i     a10, a6, 0x198
        sub      a9, a9, a12
        addx4    a12, a9, a12
        slli     a15, a12, 3
        srai     a9, a12, 31
        addx8    a12, a12, a10
        l32i     a11, a6, 0x19c
        movi     a10, 1
        bltu     a12, a15, .Lsec_218_1a2
        movi     a10, 0
.Lsec_218_1a2:
        add.n    a9, a9, a11
        add.n    a9, a10, a9
        sub      a10, a12, a5
        movi.n   a11, 1
        bltu     a12, a10, .Lsec_218_1b0
        movi.n   a11, 0
.Lsec_218_1b0:
        sub      a9, a9, a13
        sub      a9, a9, a11
        bgez     a9, .Lsec_218_1bd
        movi.n   a10, 0
        mov.n    a9, a10
.Lsec_218_1bd:
        l32i     a11, a6, 0x1b0
        movi.n   a12, 1
        add      a11, a14, a11
        l32i     a15, a6, 0x1b4
        bltu     a11, a14, .Lsec_218_1ce
        movi     a12, 0
.Lsec_218_1ce:
        add.n    a2, a2, a15
        sub      a5, a11, a5
        add.n    a2, a12, a2
        movi.n   a12, 1
        bltu     a11, a5, .Lsec_218_1dc
        movi.n   a12, 0
.Lsec_218_1dc:
        sub      a2, a2, a13
        sub      a2, a2, a12
        bgez     a2, .Lsec_218_1ec
        movi.n   a5, 0
        mov.n    a2, a5
        j        .Lsec_218_1ef
.Lsec_218_1ec:
        bltu     a9, a2, .Lsec_218_1f5
.Lsec_218_1ef:
        bne      a2, a9, .Lsec_218_209
        bgeu     a10, a5, .Lsec_218_209
.Lsec_218_1f5:
        l32i.n   a2, a3, 0
        mov.n    a12, a7
        movi.n   a11, 0
        movi     a10, 38
        l32i     a2, a2, 0x19c
.Lsec_218_201:
        callx8   a2
        j        .Lsec_218_271
        .byte    0x00
        .byte    0x00
.Lsec_218_209:
        sub      a12, a10, a5
        movi.n   a13, 1
        bltu     a10, a12, .Lsec_218_214
        movi     a13, 0
.Lsec_218_214:
        sub      a11, a9, a2
        sub      a11, a11, a13
        add.n    a11, a11, a11
        extui    a13, a12, 31, 1
        or       a11, a13, a11
        add      a12, a12, a12
        bltu     a11, a8, .Lsec_218_234
        beq      a8, a11, .Lsec_218_22e
        j        .Lsec_218_2b9
.Lsec_218_22e:
        bltu     a12, a7, .Lsec_218_234
        j        .Lsec_218_2b9
.Lsec_218_234:
        add.n    a5, a7, a5
        movi.n   a11, 1
        bltu     a5, a7, .Lsec_218_23e
        movi     a11, 0
.Lsec_218_23e:
        add.n    a2, a8, a2
        sub      a10, a5, a10
        add.n    a2, a11, a2
        movi.n   a8, 1
        bltu     a5, a10, .Lsec_218_24d
        movi     a8, 0
.Lsec_218_24d:
        sub      a2, a2, a9
        sub      a8, a2, a8
        l32i.n   a2, a3, 0
        mov.n    a7, a10
        mov.n    a12, a10
        movi.n   a11, 0
        movi.n   a10, 38
        l32i     a2, a2, 0x19c
.Lsec_218_260:
        s32i     a8, a1, 12
        memw
        callx8   a2
        l32i.n   a8, a1, 12
        or       a8, a7, a8
        beqz     a8, .Lsec_218_2b9
.Lsec_218_271:
        l8ui     a2, a4, 136
        bnez.n   a2, .Lsec_218_291
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_wake_up
        l32r     a8, .Lsec_218_14
        callx8   a8
        movi.n   a5, 1
        mov.n    a11, a2
        movi.n   a10, 17
        s8i      a5, a4, 136
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_gpio_debug
        l32r     a8, .Lsec_218_18
        callx8   a8
.Lsec_218_291:
        l16ui    a2, a6, 0x18a
        beqz.n   a2, .Lsec_218_2a5
        movi.n   a12, 0
        mov.n    a11, a7
        l32i.n   a2, a3, 0
        l32r     a10, .Lsec_218_8
        l32i     a2, a2, 248
        callx8   a2
.Lsec_218_2a5:
        l32i.n   a2, a3, 0
        l32i     a10, a2, 0x110
        callx8   a10
        s32i     a10, a6, 0x1a0
        s32i     a11, a6, 0x1a4
        memw
        j        .Lsec_218_2da
.Lsec_218_2b9:
        l8ui     a2, a4, 136
        beqz.n   a2, .Lsec_218_2da
        movi.n   a2, 0
        movi     a11, 1
        movi.n   a10, 17
        s8i      a2, a4, 136
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_gpio_debug
        l32r     a8, .Lsec_218_1c
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_wake_done
        l32r     a8, .Lsec_218_20
        callx8   a8
.Lsec_218_2da:
        retw.n
        .size   pm_coex_separate_connectionless_window, .-pm_coex_separate_connectionless_window
.Lsec_218_2dc:

        .section .wifislpiram.23,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_219_0, 0x0000ffff
        .literal .Lsec_219_4, g_pm+0x138
        .literal .Lsec_219_8, g_osi_funcs_p
        .literal .Lsec_219_c, g_pm+0x14c
        .literal .Lsec_219_10, g_pm
        .literal .Lsec_219_14, wifi_gpio_debug
        .literal .Lsec_219_18, pm_wake_done
        .literal .Lsec_219_1c, pm_get_connectionless_status
        .literal .Lsec_219_20, wifi_event_post
        .literal .Lsec_219_24, wifi_gpio_debug
        .literal .Lsec_219_28, pm_coex_separate_connectionless_window
        .literal .Lsec_219_2c, pm_wake_up
        .literal .Lsec_219_30, wifi_gpio_debug
.Lsec_219_34:
        .global pm_update_by_connectionless_status
        .type   pm_update_by_connectionless_status, @function
pm_update_by_connectionless_status:
        entry    a1, 32
        l32r     a3, .Lsec_219_8
        movi.n   a6, 0
        l32i.n   a2, a3, 0
        l32r     a7, .Lsec_219_4
        mov.n    a10, a7
        l32i     a2, a2, 236
        callx8   a2
        l32i     a2, a3, 0
        l32r     a5, .Lsec_219_c
        or       a10, a5, a5
        l32i     a2, a2, 236
        callx8   a2
        l32r     a2, .Lsec_219_10
        addmi    a4, a2, 0x100
        l8ui     a8, a4, 136
        s8i      a6, a4, 140
        memw
        beqz.n   a8, .Lsec_219_82
        movi.n   a11, 1
        movi.n   a10, 17
        s8i      a6, a4, 136
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_gpio_debug
        l32r     a8, .Lsec_219_14
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_wake_done
        l32r     a8, .Lsec_219_18
        callx8   a8
.Lsec_219_82:
        l8ui     a6, a4, 29
        bgeui    a6, 2, .Lsec_219_8e
        l8ui     a6, a2, 13
        beqz     a6, .Lsec_219_170
.Lsec_219_8e:
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_get_connectionless_status
        l32r     a8, .Lsec_219_1c
        callx8   a8
        mov.n    a6, a10
        l16ui    a8, a2, 0x18a
        beqz     a8, .Lsec_219_ea
        movi.n   a12, 0
        mov.n    a11, a12
        movi     a10, 22
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_event_post
        l32r     a8, .Lsec_219_20
        callx8   a8
        l16ui    a8, a2, 0x18a
        movi.n   a12, 0
        slli     a11, a8, 5
        sub      a11, a11, a8
        addx4    a11, a11, a8
        l32i     a8, a3, 0
        or       a10, a7, a7
        slli     a11, a11, 3
        l32i     a8, a8, 248
        callx8   a8
        l32i.n   a7, a3, 0
        l32i     a10, a7, 0x110
        movi     a7, 1
        callx8   a10
        s32i     a10, a2, 0x198
        movi.n   a10, 16
        s32i     a11, a2, 0x19c
        movi.n   a11, 2
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_gpio_debug
        l32r     a8, .Lsec_219_24
        callx8   a8
        s8i      a7, a4, 140
        memw
.Lsec_219_ea:
        movi.n   a9, -2
        movi.n   a7, 0
        l32r     a8, .Lsec_219_0
        beq      a6, a9, .Lsec_219_114
        extui    a7, a6, 0, 16
        bnei     a6, -1, .Lsec_219_105
        l16ui    a7, a2, 0x18a
        memw
        j        .Lsec_219_105
        .byte    0x00
        .byte    0x00
.Lsec_219_105:
        slli     a8, a7, 5
        sub      a8, a8, a7
        addx4    a8, a8, a7
        slli     a8, a8, 3
        srai     a7, a8, 31
.Lsec_219_114:
        s32i     a8, a2, 0x1a8
        s32i     a7, a2, 0x1ac
        l32i     a2, a3, 0
        l32i     a10, a2, 0x194
        memw
        callx8   a10
        mov.n    a2, a10
        beqz.n   a10, .Lsec_219_134
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_coex_separate_connectionless_window
        l32r     a8, .Lsec_219_28
        callx8   a8
        j        .Lsec_219_170
        .byte    0x00
.Lsec_219_134:
        beqz.n   a6, .Lsec_219_170
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_wake_up
        l32r     a8, .Lsec_219_2c
        callx8   a8
        movi.n   a7, 1
        mov.n    a11, a2
        movi.n   a10, 17
        s8i      a7, a4, 136
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_gpio_debug
        l32r     a8, .Lsec_219_30
        callx8   a8
        blti     a6, 1, .Lsec_219_170
        extui    a6, a6, 0, 16
        slli     a11, a6, 5
        l32i.n   a3, a3, 0
        sub      a11, a11, a6
        addx4    a11, a11, a6
        mov.n    a12, a2
        slli     a11, a11, 3
        l32i     a3, a3, 248
        or       a10, a5, a5
        callx8   a3
.Lsec_219_170:
        retw.n
        .size   pm_update_by_connectionless_status, .-pm_update_by_connectionless_status
.Lsec_219_172:

        .section .wifislprxiram.36,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_220_0, g_pm
        .literal .Lsec_220_4, g_osi_funcs_p
        .literal .Lsec_220_8, pm_dream
        .literal .Lsec_220_c, .Lsec_151_c
.Lsec_220_10:
        .global pm_check_state
        .type   pm_check_state, @function
pm_check_state:
        entry    a1, 32
        l32r     a3, .Lsec_220_0
        l32i     a2, a3, 36
        beqz     a2, .Lsec_220_25
.Lsec_220_1c:
        l8ui     a2, a3, 13
        bnez.n   a2, .Lsec_220_50
        j        .Lsec_220_3a
        .byte    0x00
.Lsec_220_25:
        l32r     a2, .Lsec_220_4
        l32i     a2, a2, 0
        l32i     a10, a2, 0x194
        callx8   a10
        bnez     a10, .Lsec_220_1c
        l8ui     a2, a3, 14
        beqz     a2, .Lsec_220_1c
.Lsec_220_3a:
        movi.n   a2, 0
        l8ui     a3, a3, 1
        beq      a3, a2, .Lsec_220_50
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_dream
        l32r     a8, .Lsec_220_8
        callx8   a8
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, .Lsec_151_c
        l32r     a8, .Lsec_220_c
        callx8   a8
.Lsec_220_50:
        retw.n
        .size   pm_check_state, .-pm_check_state
.Lsec_220_52:

        .section .text.pm_go_to_sleep,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_221_0, g_pm
        .literal .Lsec_221_4, g_mesh_is_started
        .literal .Lsec_221_8, g_mesh_is_root
        .literal .Lsec_221_c, g_mesh_init_ps_type
        .literal .Lsec_221_10, g_pm+0x13
        .literal .Lsec_221_14, g_pm+0x14
        .literal .Lsec_221_18, esp_mesh_quick_funcs
        .literal .Lsec_221_1c, pm_check_state
        .literal .Lsec_221_20, .Lsec_151_c
        .literal .Lsec_221_24, pm_send_nullfunc
.Lsec_221_28:
        .global pm_go_to_sleep
        .type   pm_go_to_sleep, @function
pm_go_to_sleep:
        entry    a1, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_check_state
        l32r     a8, .Lsec_221_1c
        callx8   a8
        beqz     a10, .Lsec_221_85
        l32r     a2, .Lsec_221_0
        l8ui     a8, a2, 1
        beqi     a8, 2, .Lsec_221_85
        movi.n   a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, .Lsec_151_c
        l32r     a8, .Lsec_221_20
        callx8   a8
        l32r     a8, .Lsec_221_4
        l8ui     a8, a8, 0
        beqz.n   a8, .Lsec_221_78
        l32r     a8, .Lsec_221_8
        memw
        l8ui     a8, a8, 0
        extui    a8, a8, 0, 8
        bnez.n   a8, .Lsec_221_78
        l32r     a8, .Lsec_221_c
        l32i.n   a8, a8, 0
        beqz.n   a8, .Lsec_221_78
        movi     a10, 1
        l32r     a2, .Lsec_221_18
        l32r     a12, .Lsec_221_10
        l32i     a2, a2, 136
        l32r     a11, .Lsec_221_14
        callx8   a2
        j        .Lsec_221_85
        .byte    0x00
.Lsec_221_78:
        l8ui     a2, a2, 20
        bnez.n   a2, .Lsec_221_85
        movi.n   a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_send_nullfunc
        l32r     a8, .Lsec_221_24
        callx8   a8
.Lsec_221_85:
        retw.n
        .size   pm_go_to_sleep, .-pm_go_to_sleep
.Lsec_221_87:

        .section .text.pm_set_next_tbtt,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_222_0, g_osi_funcs_p
        .literal .Lsec_222_4, g_pm
        .literal .Lsec_222_8, g_intr_lock_mux
        .literal .Lsec_222_c, 0x00001387
        .literal .Lsec_222_10, g_pm+0xa4
        .literal .Lsec_222_14, .Lsec_315_0
        .literal .Lsec_222_18, pm_coex_schm_overall_period_get
        .literal .Lsec_222_1c, __udivdi3
        .literal .Lsec_222_20, wifi_log
.Lsec_222_24:
        .global pm_set_next_tbtt
        .type   pm_set_next_tbtt, @function
pm_set_next_tbtt:
        entry    a1, 96
        l32r     a4, .Lsec_222_0
        l32i.n   a2, a4, 0
        l32i     a10, a2, 0x110
        movi     a2, 1
        callx8   a10
        l32r     a6, .Lsec_222_4
        l32i.n   a3, a6, 40
        add.n    a10, a3, a10
        bltu     a10, a3, .Lsec_222_41
        movi.n   a2, 0
.Lsec_222_41:
        l32i     a3, a6, 92
        add      a2, a2, a11
        bgeu     a2, a3, .Lsec_222_4d
        j        .Lsec_222_250
.Lsec_222_4d:
        bne      a3, a2, .Lsec_222_59
        l32i     a2, a6, 88
        bgeu     a10, a2, .Lsec_222_59
        j        .Lsec_222_250
.Lsec_222_59:
        l32i.n   a2, a4, 0
        l32i     a10, a2, 0x110
        callx8   a10
        l32i.n   a2, a4, 0
        s32i.n   a10, a1, 32
        l32i     a10, a2, 0x194
        s32i.n   a11, a1, 36
        memw
        callx8   a10
        beqz.n   a10, .Lsec_222_aa
        l8ui     a5, a6, 14
        beqz.n   a5, .Lsec_222_94
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_coex_schm_overall_period_get
        l32r     a8, .Lsec_222_18
        callx8   a8
        movi.n   a3, 1
        l32i.n   a2, a6, 44
        mull     a10, a10, a2
        l32i     a2, a6, 88
        add.n    a2, a10, a2
        l32i     a5, a6, 92
        bltu     a2, a10, .Lsec_222_f6
        movi.n   a3, 0
        j        .Lsec_222_f6
.Lsec_222_94:
        l32i.n   a7, a6, 44
        movi.n   a3, 1
        l32i     a2, a6, 88
        add.n    a2, a7, a2
        l32i     a8, a6, 92
        bltu     a2, a7, .Lsec_222_a5
        mov.n    a3, a5
.Lsec_222_a5:
        add.n    a3, a3, a8
        j        .Lsec_222_f8
.Lsec_222_aa:
        l32i.n   a5, a6, 36
        l32i     a2, a6, 88
        l32i     a3, a6, 92
        l32i.n   a8, a6, 44
        bnez.n   a5, .Lsec_222_c4
        add.n    a2, a8, a2
        movi.n   a7, 1
        bltu     a2, a8, .Lsec_222_e8
        mov.n    a7, a5
        j        .Lsec_222_e8
        .byte    0x00
        .byte    0x00
.Lsec_222_c4:
        bnei     a5, 1, .Lsec_222_d5
        l8ui     a7, a6, 11
        mull     a7, a7, a8
        add.n    a2, a7, a2
        bltu     a2, a7, .Lsec_222_f6
        j        .Lsec_222_f4
.Lsec_222_d5:
        bnei     a5, 2, .Lsec_222_ed
        l16ui    a5, a6, 32
        movi.n   a7, 1
        mull     a5, a5, a8
        add.n    a2, a5, a2
        bltu     a2, a5, .Lsec_222_e8
        or       a7, a10, a10
.Lsec_222_e8:
        add.n    a3, a7, a3
        j        .Lsec_222_f8
.Lsec_222_ed:
        add.n    a2, a8, a2
        movi.n   a5, 1
        bltu     a2, a8, .Lsec_222_f6
.Lsec_222_f4:
        mov.n    a5, a10
.Lsec_222_f6:
        add.n    a3, a5, a3
.Lsec_222_f8:
        s32i     a2, a6, 88
        s32i     a3, a6, 92
        l32i.n   a2, a4, 0
        l32r     a3, .Lsec_222_8
        l32i.n   a2, a2, 40
        l32i.n   a10, a3, 0
        memw
        callx8   a2
        l32i.n   a2, a4, 0
        s32i.n   a10, a1, 40
        l32i     a10, a2, 0x110
        memw
        callx8   a10
        mov.n    a9, a10
        mov.n    a7, a11
        l32i     a2, a6, 88
        movi     a11, 1
        sub      a10, a2, a10
        l32i     a5, a6, 92
        bltu     a2, a10, .Lsec_222_130
        movi     a11, 0
.Lsec_222_130:
        sub      a12, a5, a7
        sub      a3, a12, a11
        bne      a12, a11, .Lsec_222_162
        l32r     a11, .Lsec_222_c
        bltu     a11, a10, .Lsec_222_15a
        movi.n   a10, 1
        l32i.n   a11, a6, 44
        add.n    a2, a11, a2
        bltu     a2, a11, .Lsec_222_14a
        mov.n    a10, a3
.Lsec_222_14a:
        add.n    a10, a10, a5
        s32i     a2, a6, 88
        movi.n   a3, 0
        s32i     a10, a6, 92
        memw
        j        .Lsec_222_1d2
.Lsec_222_15a:
        movi.n   a11, -1
        slli     a11, a11, 31
        bgeu     a11, a10, .Lsec_222_1d2
.Lsec_222_162:
        movi     a3, 0x271
        slli     a3, a3, 3
        sub      a12, a3, a2
        l32i.n   a6, a6, 44
        movi.n   a10, 1
        bltu     a3, a12, .Lsec_222_175
        movi     a10, 0
.Lsec_222_175:
        neg      a3, a5
        sub      a3, a3, a10
        add.n    a10, a12, a9
        movi     a11, 1
        bltu     a10, a12, .Lsec_222_186
        movi     a11, 0
.Lsec_222_186:
        add.n    a3, a3, a7
        mov.n    a12, a6
        movi.n   a13, 0
        add.n    a11, a11, a3
        s32i     a9, a1, 52
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, __udivdi3
        l32r     a8, .Lsec_222_1c
        callx8   a8
        mov.n    a3, a10
        addi.n   a10, a10, 1
        mov.n    a13, a11
        l32r     a14, .Lsec_222_4
        movi.n   a12, 1
        l32i.n   a9, a1, 52
        bltu     a10, a3, .Lsec_222_1ac
        movi.n   a12, 0
.Lsec_222_1ac:
        add.n    a12, a12, a11
        mull     a11, a10, a6
        mull     a12, a12, a6
        muluh    a10, a10, a6
        add.n    a2, a11, a2
        add.n    a10, a12, a10
        movi.n   a6, 1
        bltu     a2, a11, .Lsec_222_1c2
        movi.n   a6, 0
.Lsec_222_1c2:
        add.n    a10, a10, a5
        add.n    a6, a6, a10
        s32i     a2, a14, 88
        s32i     a6, a14, 92
        memw
        j        .Lsec_222_1d4
.Lsec_222_1d2:
        mov.n    a13, a3
.Lsec_222_1d4:
        l32i.n   a2, a4, 0
        l32r     a5, .Lsec_222_10
        mov.n    a10, a5
        l32i     a2, a2, 236
        s32i.n   a9, a1, 52
        s32i.n   a13, a1, 48
        memw
        callx8   a2
        l32r     a2, .Lsec_222_4
        l32i.n   a10, a4, 0
        l32i     a14, a2, 88
        l32i     a6, a10, 248
        l32i     a10, a10, 0x110
        s32i.n   a14, a1, 44
        memw
        callx8   a10
        movi.n   a12, 0
        l32i.n   a14, a1, 44
        sub      a11, a14, a10
        mov.n    a10, a5
        callx8   a6
        l32i.n   a4, a4, 0
        l32r     a5, .Lsec_222_8
        l32i.n   a4, a4, 44
        l32i.n   a11, a1, 40
        l32i     a10, a5, 0
        callx8   a4
        l32i.n   a13, a1, 48
        l32i.n   a9, a1, 52
        bnez.n   a13, .Lsec_222_225
        movi     a4, 100
        bgeu     a4, a3, .Lsec_222_250
.Lsec_222_225:
        movi.n   a11, 1
        l32i.n   a3, a2, 44
        movi.n   a12, 2
        slli     a11, a11, 12
        movi.n   a10, 6
        s32i.n   a3, a1, 16
        l32i     a3, a2, 92
        l32i     a2, a2, 88
        l32i.n   a14, a1, 32
        l32i.n   a15, a1, 36
        l32r     a13, .Lsec_222_14
        s32i.n   a2, a1, 8
        s32i.n   a3, a1, 12
        s32i.n   a9, a1, 0
        s32i.n   a7, a1, 4
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_222_20
        callx8   a8
.Lsec_222_250:
        retw.n
        .size   pm_set_next_tbtt, .-pm_set_next_tbtt
.Lsec_222_252:

        .section .text.pm_mesh_set_next_tbtt,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_223_0, g_mesh_is_started
        .literal .Lsec_223_4, g_mesh_is_root
        .literal .Lsec_223_8, g_mesh_init_ps_type
        .literal .Lsec_223_c, g_pm
        .literal .Lsec_223_10, esp_mesh_quick_funcs
        .literal .Lsec_223_14, g_pm+0xa4
        .literal .Lsec_223_18, g_osi_funcs_p
.Lsec_223_1c:
        .global pm_mesh_set_next_tbtt
        .type   pm_mesh_set_next_tbtt, @function
pm_mesh_set_next_tbtt:
        entry    a1, 48
        l32r     a2, .Lsec_223_0
        l8ui     a2, a2, 0
        beqz     a2, .Lsec_223_84
        l32r     a3, .Lsec_223_4
        memw
        l8ui     a3, a3, 0
        extui    a3, a3, 0, 8
        bnez     a3, .Lsec_223_7c
        l32r     a4, .Lsec_223_8
        l32i.n   a8, a4, 0
        beqz     a8, .Lsec_223_82
        mov.n    a12, a1
        l32r     a4, .Lsec_223_c
        l32r     a5, .Lsec_223_10
        l8ui     a11, a4, 11
        l32i     a5, a5, 120
        l32i.n   a10, a4, 44
        s32i     a3, a1, 0
        memw
        callx8   a5
        l32r     a5, .Lsec_223_18
        l32i     a8, a5, 0
        l32r     a4, .Lsec_223_14
        or       a10, a4, a4
        l32i     a8, a8, 236
        callx8   a8
        mov.n    a12, a3
        mov.n    a10, a4
        l32i.n   a5, a5, 0
        l32i.n   a11, a1, 0
        l32i     a5, a5, 248
        callx8   a5
        j        .Lsec_223_84
        .byte    0x00
.Lsec_223_7c:
        movi.n   a2, 0
        j        .Lsec_223_84
        .byte    0x00
.Lsec_223_82:
        mov.n    a2, a8
.Lsec_223_84:
        retw.n
        .size   pm_mesh_set_next_tbtt, .-pm_mesh_set_next_tbtt
.Lsec_223_86:

        .section .text.pm_update_at_next_beacon,"ax",@progbits
        .align  4
.Lsec_224_0:
        .global pm_update_at_next_beacon
        .type   pm_update_at_next_beacon, @function
pm_update_at_next_beacon:
        entry    a1, 32
        retw.n
        .size   pm_update_at_next_beacon, .-pm_update_at_next_beacon
.Lsec_224_5:

        .section .rodata.str1.1,"aMS",@progbits,1
        .align  1
.Lsec_225_0:
        .ascii  "pm.c\000"

        .section .wifislpiram.38,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_226_0, g_pm
        .literal .Lsec_226_4, 0x00000817
        .literal .Lsec_226_8, .Lsec_225_0
        .literal .Lsec_226_c, .Lsec_314_0
        .literal .Lsec_226_10, wifi_log
.Lsec_226_14:
        .global pm_handle_tbtt_interval
        .type   pm_handle_tbtt_interval, @function
pm_handle_tbtt_interval:
        entry    a1, 32
        l32r     a9, .Lsec_226_0
        beqz.n   a3, .Lsec_226_5c
        l8ui     a10, a9, 11
        l8ui     a8, a3, 3
        beq      a8, a10, .Lsec_226_40
        movi.n   a12, 1
        slli     a11, a12, 12
        l32r     a15, .Lsec_226_4
        movi     a10, 6
        l32r     a14, .Lsec_226_8
        l32r     a13, .Lsec_226_c
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_226_10
        callx8   a8
.Lsec_226_3c:
        j        .Lsec_226_3c
        .byte    0x00
.Lsec_226_40:
        remu     a10, a2, a8
        beqz     a10, .Lsec_226_4c
        remu     a8, a8, a2
        bnez     a8, .Lsec_226_61
.Lsec_226_4c:
        l8ui     a8, a3, 2
        remu     a8, a8, a2
        beqz.n   a8, .Lsec_226_61
        l32i.n   a2, a9, 44
        mull     a2, a8, a2
        j        .Lsec_226_66
.Lsec_226_5c:
        l32i.n   a2, a9, 44
        j        .Lsec_226_66
.Lsec_226_61:
        l32i.n   a3, a9, 44
        mull     a2, a2, a3
.Lsec_226_66:
        retw.n
        .size   pm_handle_tbtt_interval, .-pm_handle_tbtt_interval
.Lsec_226_68:

        .section .wifislpiram.40,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_227_0, g_osi_funcs_p
        .literal .Lsec_227_4, g_pm_cfg
        .literal .Lsec_227_8, g_pm
        .literal .Lsec_227_c, ptr_beacon_offset_funcs
        .literal .Lsec_227_10, g_intr_lock_mux
        .literal .Lsec_227_14, g_mesh_is_started
        .literal .Lsec_227_18, g_mesh_is_root
        .literal .Lsec_227_1c, g_mesh_init_ps_type
        .literal .Lsec_227_20, 0x3ff73c00
        .literal .Lsec_227_24, g_wifi_mac_time_delta
        .literal .Lsec_227_28, 0x00001387
        .literal .Lsec_227_2c, .Lsec_313_0
        .literal .Lsec_227_30, g_pm+0xa4
        .literal .Lsec_227_34, esp_mesh_quick_funcs
        .literal .Lsec_227_38, __umoddi3
        .literal .Lsec_227_3c, pm_coex_schm_overall_period_get
        .literal .Lsec_227_40, pm_handle_tbtt_interval
        .literal .Lsec_227_44, wifi_log
.Lsec_227_48:
        .global pm_update_next_tbtt
        .type   pm_update_next_tbtt, @function
pm_update_next_tbtt:
        entry    a1, 96
        l32r     a5, .Lsec_227_0
        l32i.n   a6, a5, 0
        l32i     a10, a6, 0x194
        s32i     a3, a1, 52
        memw
        callx8   a10
        movi.n   a13, 0
        l32r     a7, .Lsec_227_4
        l32r     a3, .Lsec_227_8
        l32i.n   a8, a1, 52
        l32i.n   a6, a7, 4
        l32i.n   a7, a7, 8
        movnez   a6, a7, a10
        l32i.n   a12, a3, 44
        l32i.n   a10, a8, 0
        l32i.n   a11, a8, 4
        s32i.n   a6, a3, 40
        addmi    a6, a3, 0x100
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, __umoddi3
        l32r     a8, .Lsec_227_38
        callx8   a8
        l8ui     a7, a6, 100
        s32i.n   a10, a1, 48
        beqz.n   a7, .Lsec_227_a2
        l8ui     a6, a6, 212
        bnez.n   a6, .Lsec_227_a2
        l8ui     a6, a3, 13
        beqz.n   a6, .Lsec_227_a2
        l32r     a6, .Lsec_227_c
        l32i.n   a6, a6, 0
        l32i.n   a6, a6, 4
        beqz     a6, .Lsec_227_a2
        memw
        callx8   a6
.Lsec_227_a2:
        l32i.n   a6, a1, 48
        sub      a2, a2, a6
        l32i     a6, a5, 0
        l32i     a10, a6, 0x194
        callx8   a10
        beqz.n   a10, .Lsec_227_c2
        l8ui     a6, a3, 14
        beqz.n   a6, .Lsec_227_e9
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_coex_schm_overall_period_get
        l32r     a8, .Lsec_227_3c
        callx8   a8
        mov.n    a11, a4
        j        .Lsec_227_d2
.Lsec_227_c2:
        l32i.n   a6, a3, 36
        bnez.n   a6, .Lsec_227_c9
        j        .Lsec_227_e9
.Lsec_227_c9:
        bnei     a6, 1, .Lsec_227_dd
        l8ui     a10, a3, 11
        or       a11, a4, a4
.Lsec_227_d2:
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_handle_tbtt_interval
        l32r     a8, .Lsec_227_40
        callx8   a8
        add.n    a9, a10, a2
        j        .Lsec_227_ed
.Lsec_227_dd:
        bnei     a6, 2, .Lsec_227_e9
        l16ui    a10, a3, 32
        mov.n    a11, a4
        j        .Lsec_227_d2
        .byte    0x00
.Lsec_227_e9:
        l32i.n   a9, a3, 44
        add.n    a9, a2, a9
.Lsec_227_ed:
        l32i.n   a6, a3, 40
        l32i     a7, a3, 0x1cc
        sub      a7, a7, a6
        add.n    a7, a7, a9
        l32i.n   a6, a5, 0
        l32r     a10, .Lsec_227_10
        l32i.n   a6, a6, 40
        l32i.n   a10, a10, 0
        s32i.n   a9, a1, 56
        memw
        callx8   a6
        mov.n    a6, a10
        l32r     a10, .Lsec_227_14
        l8ui     a10, a10, 0
        l32i.n   a9, a1, 56
        beqz.n   a10, .Lsec_227_138
        l32r     a10, .Lsec_227_18
        memw
        l8ui     a10, a10, 0
        extui    a10, a10, 0, 8
        bnez.n   a10, .Lsec_227_138
        l32r     a10, .Lsec_227_1c
        l32i.n   a10, a10, 0
        beqz.n   a10, .Lsec_227_138
        l32i.n   a10, a5, 0
        l32i     a10, a10, 0x110
        callx8   a10
        l32i.n   a9, a1, 56
        j        .Lsec_227_147
        .byte    0x00
        .byte    0x00
.Lsec_227_138:
        l32r     a10, .Lsec_227_20
        l32r     a11, .Lsec_227_24
        memw
        l32i.n   a10, a10, 0
        l32i.n   a11, a11, 0
        add.n    a10, a10, a11
.Lsec_227_147:
        sub      a13, a7, a10
        l32r     a11, .Lsec_227_28
        l32i.n   a12, a5, 0
        bltu     a11, a13, .Lsec_227_15a
        l32i.n   a2, a3, 44
        add.n    a9, a9, a2
        j        .Lsec_227_215
        .byte    0x00
.Lsec_227_15a:
        movi.n   a11, -1
        slli     a11, a11, 31
        bltu     a11, a13, .Lsec_227_165
        j        .Lsec_227_215
.Lsec_227_165:
        movi     a11, 0x271
        l32i.n   a13, a3, 44
        subx8    a7, a11, a7
        add.n    a10, a7, a10
        quou     a10, a10, a13
        addi.n   a7, a10, 1
        mull     a7, a7, a13
        add.n    a9, a9, a7
        movi     a7, 100
        bltu     a7, a10, .Lsec_227_182
        j        .Lsec_227_215
.Lsec_227_182:
        mov.n    a11, a6
        l32r     a6, .Lsec_227_10
        l32i.n   a7, a12, 44
        l32i.n   a10, a6, 0
        mov.n    a6, a4
        s32i     a9, a1, 56
        memw
        callx8   a7
        mov.n    a7, a4
        l32i.n   a12, a3, 44
        l32i.n   a9, a1, 56
        beqz     a4, .Lsec_227_1a5
        l8ui     a6, a4, 3
        l8ui     a7, a4, 2
.Lsec_227_1a5:
        l32r     a4, .Lsec_227_14
        l8ui     a4, a4, 0
        l16ui    a3, a3, 32
        beqz.n   a4, .Lsec_227_1db
        l32r     a4, .Lsec_227_18
        memw
        l8ui     a4, a4, 0
        extui    a4, a4, 0, 8
        bnez.n   a4, .Lsec_227_1db
        l32r     a4, .Lsec_227_1c
        l32i.n   a4, a4, 0
        beqz.n   a4, .Lsec_227_1db
        l32i.n   a4, a5, 0
        l32i     a10, a4, 0x110
        s32i.n   a9, a1, 56
        s32i.n   a12, a1, 52
        memw
        callx8   a10
        l32i.n   a9, a1, 56
        l32i.n   a12, a1, 52
        j        .Lsec_227_1ea
.Lsec_227_1db:
        l32r     a4, .Lsec_227_20
        memw
        l32i.n   a10, a4, 0
        l32r     a4, .Lsec_227_24
        l32i.n   a4, a4, 0
        add.n    a10, a10, a4
.Lsec_227_1ea:
        movi.n   a11, 1
        mov.n    a14, a2
        l32i.n   a15, a1, 48
        slli     a11, a11, 12
        l32r     a13, .Lsec_227_2c
        s32i.n   a10, a1, 20
        movi.n   a10, 6
        s32i.n   a12, a1, 4
        movi.n   a12, 2
        s32i.n   a3, a1, 16
        s32i.n   a7, a1, 12
        s32i.n   a6, a1, 8
        s32i     a9, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_227_44
        callx8   a8
        j        .Lsec_227_324
        .byte    0x00
        .byte    0x00
.Lsec_227_215:
        l32i.n   a4, a3, 40
        l32i     a2, a3, 0x1cc
        sub      a2, a2, a4
        add.n    a2, a2, a9
        l32r     a4, .Lsec_227_30
        mov.n    a10, a4
        l32i     a7, a12, 236
        callx8   a7
        l32i.n   a7, a5, 0
        l32i     a10, a7, 0x110
        callx8   a10
        mov.n    a9, a10
        mov.n    a7, a11
        l32r     a8, .Lsec_227_14
        l8ui     a10, a8, 0
        beqz.n   a10, .Lsec_227_265
        l32r     a10, .Lsec_227_18
        memw
        l8ui     a10, a10, 0
        extui    a10, a10, 0, 8
        bnez.n   a10, .Lsec_227_265
        l32r     a10, .Lsec_227_1c
        l32i.n   a10, a10, 0
        beqz.n   a10, .Lsec_227_265
        l32i.n   a10, a5, 0
        l32i     a10, a10, 0x110
        s32i.n   a9, a1, 56
        memw
        callx8   a10
        l32i.n   a9, a1, 56
        j        .Lsec_227_274
.Lsec_227_265:
        l32r     a10, .Lsec_227_20
        l32r     a11, .Lsec_227_24
        memw
        l32i.n   a10, a10, 0
        l32i.n   a11, a11, 0
        add.n    a10, a10, a11
.Lsec_227_274:
        sub      a10, a2, a10
        add.n    a11, a10, a9
        movi.n   a9, 1
        bltu     a11, a10, .Lsec_227_281
        movi     a9, 0
.Lsec_227_281:
        l32r     a8, .Lsec_227_14
        add.n    a9, a9, a7
        s32i     a9, a3, 92
        l8ui     a9, a8, 0
        s32i     a11, a3, 88
        beqz.n   a9, .Lsec_227_2d1
        l32r     a7, .Lsec_227_18
        memw
        l8ui     a7, a7, 0
        extui    a7, a7, 0, 8
        bnez.n   a7, .Lsec_227_2d1
        l32r     a10, .Lsec_227_1c
        l32i.n   a10, a10, 0
        beqz.n   a10, .Lsec_227_2d1
        addi     a13, a1, 32
        l32r     a2, .Lsec_227_34
        l32i.n   a12, a1, 48
        l8ui     a11, a3, 11
        l32i     a2, a2, 124
        l32i.n   a10, a3, 44
        s32i.n   a7, a1, 32
        memw
        callx8   a2
        mov.n    a12, a7
        mov.n    a10, a4
        l32i.n   a2, a5, 0
        l32i.n   a11, a1, 32
        l32i     a2, a2, 248
        callx8   a2
        j        .Lsec_227_316
        .byte    0x00
        .byte    0x00
.Lsec_227_2d1:
        l32i.n   a7, a5, 0
        l32i     a8, a7, 248
        beqz.n   a9, .Lsec_227_2fd
        l32r     a3, .Lsec_227_18
        memw
        l8ui     a3, a3, 0
        extui    a3, a3, 0, 8
        bnez.n   a3, .Lsec_227_2fd
        l32r     a3, .Lsec_227_1c
        l32i.n   a3, a3, 0
        beqz.n   a3, .Lsec_227_2fd
        l32i     a10, a7, 0x110
        s32i.n   a8, a1, 52
        memw
        callx8   a10
        l32i.n   a8, a1, 52
        j        .Lsec_227_30c
.Lsec_227_2fd:
        l32r     a3, .Lsec_227_20
        memw
        l32i.n   a10, a3, 0
        l32r     a3, .Lsec_227_24
        l32i.n   a3, a3, 0
        add.n    a10, a10, a3
.Lsec_227_30c:
        sub      a11, a2, a10
        movi.n   a12, 0
        mov.n    a10, a4
        callx8   a8
.Lsec_227_316:
        l32i.n   a2, a5, 0
        mov.n    a11, a6
        l32r     a3, .Lsec_227_10
        l32i.n   a2, a2, 44
        l32i.n   a10, a3, 0
        callx8   a2
.Lsec_227_324:
        retw.n
        .size   pm_update_next_tbtt, .-pm_update_next_tbtt
.Lsec_227_326:

        .section .text.pm_beacon_timestamp_statistic,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_228_0, g_pm_cnt
        .literal .Lsec_228_4, g_pm
        .literal .Lsec_228_8, __umoddi3
.Lsec_228_c:
        .global pm_beacon_timestamp_statistic
        .type   pm_beacon_timestamp_statistic, @function
pm_beacon_timestamp_statistic:
        entry    a1, 32
        beqz.n   a2, .Lsec_228_34
        l32r     a8, .Lsec_228_4
        movi.n   a13, 0
        l32r     a3, .Lsec_228_0
        l32i.n   a10, a2, 0
        l32i.n   a11, a2, 4
        l32i     a12, a8, 44
        .reloc  ., R_XTENSA_ASM_EXPAND, __umoddi3
        l32r     a8, .Lsec_228_8
        callx8   a8
        l32i.n   a4, a3, 36
        sub      a10, a10, a4
        l32i.n   a2, a3, 4
        quou     a10, a10, a2
        add.n    a10, a10, a4
        s32i.n   a10, a3, 36
.Lsec_228_34:
        memw
        retw.n
        .size   pm_beacon_timestamp_statistic, .-pm_beacon_timestamp_statistic
.Lsec_228_39:

        .section .text.pm_beacon_misc_process,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_229_0, pm_beacon_timestamp_statistic
.Lsec_229_4:
        .global pm_beacon_misc_process
        .type   pm_beacon_misc_process, @function
pm_beacon_misc_process:
        entry    a1, 32
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_beacon_timestamp_statistic
        l32r     a8, .Lsec_229_0
        callx8   a8
        retw.n
        .size   pm_beacon_misc_process, .-pm_beacon_misc_process
.Lsec_229_12:

        .section .text.pm_is_in_wifi_slice_threshold,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_230_0, g_osi_funcs_p
        .literal .Lsec_230_4, g_pm
.Lsec_230_8:
        .global pm_is_in_wifi_slice_threshold
        .type   pm_is_in_wifi_slice_threshold, @function
pm_is_in_wifi_slice_threshold:
        entry    a1, 32
        l32r     a8, .Lsec_230_0
        l32i.n   a8, a8, 0
        l32i     a10, a8, 0x194
        callx8   a10
        movi.n   a8, 1
        beqz.n   a10, .Lsec_230_50
        l32r     a8, .Lsec_230_4
        l32i     a10, a8, 104
        l32i     a8, a8, 108
        bltu     a3, a8, .Lsec_230_2c
        bne      a8, a3, .Lsec_230_4e
        bgeu     a2, a10, .Lsec_230_4e
.Lsec_230_2c:
        sub      a9, a10, a2
        movi.n   a11, 0
        movi.n   a2, 1
        bltu     a10, a9, .Lsec_230_38
        mov.n    a2, a11
.Lsec_230_38:
        sub      a8, a8, a3
        sub      a8, a8, a2
        movi.n   a2, 1
        bne      a11, a8, .Lsec_230_48
        bgeu     a9, a4, .Lsec_230_48
        movi.n   a2, 0
.Lsec_230_48:
        mov.n    a8, a2
        j        .Lsec_230_50
        .byte    0x00
.Lsec_230_4e:
        movi.n   a8, 0
.Lsec_230_50:
        extui    a2, a8, 0, 1
        retw.n
        .size   pm_is_in_wifi_slice_threshold, .-pm_is_in_wifi_slice_threshold
.Lsec_230_55:

        .section .wifislprxiram.42,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_231_0, 0x002dc6c0
        .literal .Lsec_231_4, g_pm
        .literal .Lsec_231_8, g_osi_funcs_p
        .literal .Lsec_231_c, g_pm_cfg
        .literal .Lsec_231_10, g_mesh_is_started
        .literal .Lsec_231_14, g_mesh_is_root
.Lsec_231_18:
        .global pm_keep_alive
        .type   pm_keep_alive, @function
pm_keep_alive:
        entry    a1, 32
        l32r     a3, .Lsec_231_4
        movi     a2, 0
        l32i     a8, a3, 36
        bne      a8, a2, .Lsec_231_2a
        j        .Lsec_231_c0
.Lsec_231_2a:
        l32r     a8, .Lsec_231_8
        l32i     a8, a8, 0
        l32i     a10, a8, 0x110
        callx8   a10
        mov.n    a13, a10
        l32r     a9, .Lsec_231_10
        l32r     a8, .Lsec_231_c
        l8ui     a9, a9, 0
        l32i.n   a12, a8, 16
        beq      a9, a2, .Lsec_231_58
        l32r     a9, .Lsec_231_14
        memw
        l8ui     a9, a9, 0
        extui    a9, a9, 0, 8
        bne      a9, a2, .Lsec_231_58
        l32r     a12, .Lsec_231_0
.Lsec_231_58:
        l8ui     a9, a8, 29
        beqz.n   a9, .Lsec_231_9d
        l32i     a2, a3, 0x1e0
        sub      a10, a10, a2
        movi.n   a2, 1
        l32i     a9, a3, 0x1e4
        bltu     a13, a10, .Lsec_231_6d
        movi.n   a2, 0
.Lsec_231_6d:
        sub      a9, a11, a9
        sub      a9, a9, a2
        l16ui    a8, a8, 48
        slli     a2, a8, 9
        slli     a8, a8, 14
        sub      a8, a8, a2
        addx4    a8, a8, a2
        slli     a8, a8, 3
        srai     a3, a8, 31
        movi     a2, 1
        bltu     a9, a3, .Lsec_231_94
        bne      a3, a9, .Lsec_231_96
        bgeu     a10, a8, .Lsec_231_96
.Lsec_231_94:
        movi.n   a2, 0
.Lsec_231_96:
        extui    a2, a2, 0, 8
        j        .Lsec_231_c0
        .byte    0x00
.Lsec_231_9d:
        l32i     a14, a3, 72
        sub      a10, a10, a14
        l32i     a8, a3, 76
        movi.n   a3, 1
        bltu     a13, a10, .Lsec_231_ad
        mov.n    a3, a9
.Lsec_231_ad:
        sub      a11, a11, a8
        sub      a11, a11, a3
        movi.n   a3, 1
        bne      a2, a11, .Lsec_231_bd
        bgeu     a10, a12, .Lsec_231_bd
        movi.n   a3, 0
.Lsec_231_bd:
        extui    a2, a3, 0, 8
.Lsec_231_c0:
        retw.n
        .size   pm_keep_alive, .-pm_keep_alive
.Lsec_231_c2:

        .section .text.pm_coex_schm_process,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_232_0, g_osi_funcs_p
        .literal .Lsec_232_4, g_pm
        .literal .Lsec_232_8, g_pm+0xf4
        .literal .Lsec_232_c, 0x3ff73cb8
        .literal .Lsec_232_10, ppCheckTxIdle
        .literal .Lsec_232_14, pm_disconnected_wake
        .literal .Lsec_232_18, pm_coex_schm_overall_period_get
        .literal .Lsec_232_1c, pm_coex_separate_connectionless_window
        .literal .Lsec_232_20, pm_is_sleeping
        .literal .Lsec_232_24, .Lsec_147_8
        .literal .Lsec_232_28, wifi_gpio_debug
        .literal .Lsec_232_2c, pm_is_sleeping
.Lsec_232_30:
        .global pm_coex_schm_process
        .type   pm_coex_schm_process, @function
pm_coex_schm_process:
        entry    a1, 48
        l32r     a3, .Lsec_232_0
        l32i.n   a2, a3, 0
        l32i     a10, a2, 0x1c4
        callx8   a10
        mov.n    a6, a10
        beqz     a10, .Lsec_232_201
        l32i.n   a2, a3, 0
        movi.n   a5, 1
        l32i     a10, a2, 0x110
        callx8   a10
        mov.n    a7, a10
        l32i     a2, a3, 0
        l32i     a10, a2, 0x1c0
        s32i     a11, a1, 4
        memw
        callx8   a10
        mov.n    a4, a10
        l32i.n   a2, a3, 0
        l32i     a10, a2, 0x1bc
        callx8   a10
        l8ui     a2, a6, 0
        mull     a10, a2, a10
        mull     a10, a10, a4
        l32i.n   a11, a1, 4
        add.n    a4, a7, a10
        s32i     a10, a1, 0
        bltu     a4, a7, .Lsec_232_7e
        movi     a5, 0
.Lsec_232_7e:
        add.n    a5, a5, a11
        l8ui     a2, a6, 1
        bbsi     a2, 1, .Lsec_232_89
        j        .Lsec_232_152
.Lsec_232_89:
        movi     a7, -65
        l32r     a2, .Lsec_232_4
        l32i     a8, a2, 0x190
        and      a7, a7, a8
        bnez.n   a7, .Lsec_232_d6
        movi.n   a10, 1
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, ppCheckTxIdle
        l32r     a8, .Lsec_232_10
        callx8   a8
        beqz.n   a10, .Lsec_232_c6
        l32i.n   a8, a3, 0
        l32i     a9, a8, 0x19c
        l32i     a10, a8, 0x110
        s32i.n   a9, a1, 4
        memw
        callx8   a10
        sub      a12, a4, a10
        mov.n    a11, a7
        movi.n   a10, 1
        l32i.n   a9, a1, 4
        callx8   a9
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_disconnected_wake
        l32r     a8, .Lsec_232_14
        callx8   a8
.Lsec_232_c6:
        movi.n   a7, 1
        s8i      a7, a2, 24
        memw
        memw
        j        .Lsec_232_ed
        .byte    0x00
        .byte    0x00
.Lsec_232_d6:
        l32i.n   a8, a3, 0
        l32i     a10, a8, 0x110
        l32i     a7, a8, 0x19c
        callx8   a10
        sub      a12, a4, a10
        movi     a11, 0
        movi     a10, 1
        callx8   a7
.Lsec_232_ed:
        s32i     a4, a2, 96
        s32i     a5, a2, 100
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_coex_schm_overall_period_get
        l32r     a8, .Lsec_232_18
        callx8   a8
        or       a7, a10, a10
        l32i     a8, a3, 0
        l32i     a10, a8, 0x1bc
        callx8   a10
        mull     a10, a7, a10
        movi.n   a7, 1
        l32i.n   a9, a1, 0
        addx4    a10, a10, a10
        addx4    a10, a10, a10
        slli     a8, a10, 2
        subx4    a10, a10, a9
        bltu     a8, a10, .Lsec_232_120
        movi.n   a7, 0
.Lsec_232_120:
        neg      a7, a7
        s32i     a10, a2, 0x1b0
        s32i     a7, a2, 0x1b4
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_coex_separate_connectionless_window
        l32r     a8, .Lsec_232_1c
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_is_sleeping
        l32r     a8, .Lsec_232_20
        callx8   a8
        bnez.n   a10, .Lsec_232_152
        l8ui     a7, a2, 13
        beqz.n   a7, .Lsec_232_14c
        l8ui     a7, a2, 14
        beqz.n   a7, .Lsec_232_152
        addmi    a2, a2, 0x200
        l8ui     a2, a2, 30
        bnez.n   a2, .Lsec_232_152
.Lsec_232_14c:
        .reloc  ., R_XTENSA_ASM_EXPAND, .Lsec_147_8
        l32r     a8, .Lsec_232_24
        callx8   a8
.Lsec_232_152:
        l8ui     a2, a6, 1
        bbci     a2, 0, .Lsec_232_1c4
        movi     a11, 0
        movi     a10, 7
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_gpio_debug
        l32r     a8, .Lsec_232_28
        callx8   a8
        l32r     a8, .Lsec_232_4
        l8ui     a9, a8, 13
        beqz.n   a9, .Lsec_232_181
        movi.n   a7, 1
        l32i.n   a2, a8, 56
        sub      a2, a4, a2
        bltu     a4, a2, .Lsec_232_178
        movi.n   a7, 0
.Lsec_232_178:
        sub      a5, a5, a7
        j        .Lsec_232_191
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_232_181:
        movi     a2, 0xfffffc18
        add.n    a2, a4, a2
        movi.n   a7, 1
        bltu     a2, a4, .Lsec_232_18d
        mov.n    a7, a9
.Lsec_232_18d:
        addi.n   a5, a5, -1
        add.n    a5, a7, a5
.Lsec_232_191:
        s32i     a5, a8, 108
        l32i.n   a5, a3, 0
        s32i     a2, a8, 104
        l32r     a2, .Lsec_232_8
        mov.n    a10, a2
        l32i     a5, a5, 236
        s32i.n   a8, a1, 4
        memw
        callx8   a5
        l32i.n   a9, a3, 0
        l32i.n   a8, a1, 4
        l32i     a10, a9, 0x110
        l32i     a5, a8, 104
        l32i     a7, a9, 248
        callx8   a10
        sub      a11, a5, a10
        movi.n   a12, 0
        or       a10, a2, a2
        callx8   a7
.Lsec_232_1c4:
        l8ui     a2, a6, 1
        bbci     a2, 2, .Lsec_232_201
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_is_sleeping
        l32r     a8, .Lsec_232_2c
        callx8   a8
        mov.n    a2, a10
        bnez     a10, .Lsec_232_1ec
        l32i     a3, a3, 0
        l32i     a10, a3, 0x110
        l32i     a5, a3, 0x19c
        callx8   a10
        sub      a12, a4, a10
        mov.n    a11, a2
        movi     a10, 1
        callx8   a5
.Lsec_232_1ec:
        l32r     a4, .Lsec_232_c
        movi.n   a3, 7
        slli     a3, a3, 17
        memw
        l32i.n   a2, a4, 0
        or       a2, a2, a3
        memw
        s32i.n   a2, a4, 0
.Lsec_232_201:
        memw
        retw.n
        .size   pm_coex_schm_process, .-pm_coex_schm_process
.Lsec_232_206:

        .section .text.pm_coex_schm_process_restart,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_233_0, g_osi_funcs_p
.Lsec_233_4:
        .global pm_coex_schm_process_restart
        .type   pm_coex_schm_process_restart, @function
pm_coex_schm_process_restart:
        entry    a1, 32
        l32r     a8, .Lsec_233_0
        l32i.n   a8, a8, 0
        l32i     a10, a8, 0x1c8
        callx8   a10
        retw.n
        .size   pm_coex_schm_process_restart, .-pm_coex_schm_process_restart
.Lsec_233_14:

        .section .text.pm_coex_slice_timeout_process,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_234_0, g_osi_funcs_p
        .literal .Lsec_234_4, g_pm
        .literal .Lsec_234_8, g_pm+0x200
        .literal .Lsec_234_c, 0x3ff73cb8
        .literal .Lsec_234_10, g_pm_cfg
        .literal .Lsec_234_14, wifi_gpio_debug
        .literal .Lsec_234_18, ppCheckTxIdle
        .literal .Lsec_234_1c, pm_enable_disconnected_sleep_delay_timer
        .literal .Lsec_234_20, ppCheckTxConnTrafficIdle
        .literal .Lsec_234_24, pm_go_to_sleep
        .literal .Lsec_234_28, pm_enable_sleep_delay_timer
.Lsec_234_2c:
        .global pm_coex_slice_timeout_process
        .type   pm_coex_slice_timeout_process, @function
pm_coex_slice_timeout_process:
        entry    a1, 32
        l32r     a5, .Lsec_234_0
        l32i.n   a2, a5, 0
        l32i     a10, a2, 0x110
        callx8   a10
        l32r     a2, .Lsec_234_4
        l8ui     a3, a2, 14
        beqz     a3, .Lsec_234_131
        l8ui     a3, a2, 15
        bnez     a3, .Lsec_234_131
        movi.n   a10, 7
        movi     a11, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_gpio_debug
        l32r     a8, .Lsec_234_14
        callx8   a8
        l32i.n   a4, a5, 0
        l32i     a10, a4, 0x194
        callx8   a10
        beqz     a10, .Lsec_234_131
        movi     a8, -65
        l32i     a4, a2, 0x190
        and      a8, a4, a8
        bnez     a8, .Lsec_234_b8
        s8i      a8, a2, 24
        memw
        bnez.n   a4, .Lsec_234_a1
        movi.n   a10, 1
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, ppCheckTxIdle
        l32r     a8, .Lsec_234_18
        callx8   a8
        beqz.n   a10, .Lsec_234_a1
        l32i.n   a8, a5, 0
        l32r     a3, .Lsec_234_8
        mov.n    a10, a3
        l32i     a8, a8, 236
        callx8   a8
        movi     a11, 0x3e8
        mov.n    a12, a4
        l32i.n   a5, a5, 0
        mov.n    a10, a3
        l32i     a8, a2, 0x1b0
        add.n    a11, a11, a8
        l32i     a5, a5, 248
        callx8   a5
.Lsec_234_a1:
        addmi    a2, a2, 0x100
        l8ui     a2, a2, 29
        beqi     a2, 2, .Lsec_234_ad
        j        .Lsec_234_131
.Lsec_234_ad:
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_enable_disconnected_sleep_delay_timer
        l32r     a8, .Lsec_234_1c
        callx8   a8
        j        .Lsec_234_131
        .byte    0x00
        .byte    0x00
.Lsec_234_b8:
        bnei     a4, 4, .Lsec_234_131
        l8ui     a5, a2, 17
        addmi    a4, a2, 0x200
        s8i      a5, a4, 29
        memw
        movi.n   a4, 7
        slli     a4, a4, 17
        l32r     a5, .Lsec_234_c
        s8i      a3, a2, 24
        s8i      a3, a2, 17
        s8i      a3, a2, 18
        memw
        memw
        l32i.n   a3, a5, 0
        or       a3, a3, a4
        memw
        s32i     a3, a5, 0
        l8ui     a3, a2, 1
        beqz     a3, .Lsec_234_f4
        l8ui     a4, a2, 19
        beqz.n   a4, .Lsec_234_128
.Lsec_234_f4:
        l32r     a3, .Lsec_234_10
        l8ui     a3, a3, 0
        beqz.n   a3, .Lsec_234_10d
.Lsec_234_fc:
        movi.n   a3, 1
        s8i      a3, a2, 25
        memw
        memw
        j        .Lsec_234_11f
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_234_10d:
        l32i     a3, a2, 64
        bnez     a3, .Lsec_234_fc
        movi     a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, ppCheckTxConnTrafficIdle
        l32r     a8, .Lsec_234_20
        callx8   a8
        bnez     a10, .Lsec_234_fc
.Lsec_234_11f:
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_go_to_sleep
        l32r     a8, .Lsec_234_24
        callx8   a8
        j        .Lsec_234_131
.Lsec_234_128:
        bnei     a3, 1, .Lsec_234_131
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_enable_sleep_delay_timer
        l32r     a8, .Lsec_234_28
        callx8   a8
.Lsec_234_131:
        retw.n
        .size   pm_coex_slice_timeout_process, .-pm_coex_slice_timeout_process
.Lsec_234_133:

        .section .text.pm_dream_timeout_process,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_235_0, g_pm
        .literal .Lsec_235_4, pm_enable_sleep_delay_timer
.Lsec_235_8:
        .global pm_dream_timeout_process
        .type   pm_dream_timeout_process, @function
pm_dream_timeout_process:
        entry    a1, 32
        l32r     a8, .Lsec_235_0
        movi     a9, 0
        s8i      a9, a8, 18
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_enable_sleep_delay_timer
        l32r     a8, .Lsec_235_4
        callx8   a8
        retw.n
        .size   pm_dream_timeout_process, .-pm_dream_timeout_process
.Lsec_235_22:

        .section .text.pm_sleep_delay_timeout_process,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_236_0, g_pm
        .literal .Lsec_236_4, pm_sleep
.Lsec_236_8:
        .global pm_sleep_delay_timeout_process
        .type   pm_sleep_delay_timeout_process, @function
pm_sleep_delay_timeout_process:
        entry    a1, 32
        l32r     a8, .Lsec_236_0
        movi.n   a10, 0
        addmi    a9, a8, 0x100
        l8ui     a8, a8, 1
        s8i      a10, a9, 184
        memw
        bnei     a8, 1, .Lsec_236_28
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_sleep
        l32r     a8, .Lsec_236_4
        callx8   a8
.Lsec_236_28:
        retw.n
        .size   pm_sleep_delay_timeout_process, .-pm_sleep_delay_timeout_process
.Lsec_236_2a:

        .section .text.pm_disconnected_sleep_delay_timeout_process,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_237_0, g_pm
        .literal .Lsec_237_4, pm_disconnected_sleep
.Lsec_237_8:
        .global pm_disconnected_sleep_delay_timeout_process
        .type   pm_disconnected_sleep_delay_timeout_process, @function
pm_disconnected_sleep_delay_timeout_process:
        entry    a1, 32
        l32r     a8, .Lsec_237_0
        movi.n   a9, 0
        addmi    a8, a8, 0x100
        s8i      a9, a8, 184
        memw
        l8ui     a8, a8, 29
        bnei     a8, 2, .Lsec_237_28
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_disconnected_sleep
        l32r     a8, .Lsec_237_4
        callx8   a8
.Lsec_237_28:
        retw.n
        .size   pm_disconnected_sleep_delay_timeout_process, .-pm_disconnected_sleep_delay_timeout_process
.Lsec_237_2a:

        .section .wifislpiram.46,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_238_0, g_pm
        .literal .Lsec_238_4, g_osi_funcs_p
        .literal .Lsec_238_8, pm_update_by_connectionless_status
.Lsec_238_c:
        .global pm_connectionless_wake_interval_timeout_process
        .type   pm_connectionless_wake_interval_timeout_process, @function
pm_connectionless_wake_interval_timeout_process:
        entry    a1, 32
        l32r     a3, .Lsec_238_0
        movi     a4, 0
        addmi    a2, a3, 0x100
        s8i      a4, a2, 140
        memw
        l8ui     a2, a3, 13
        bnez     a2, .Lsec_238_30
.Lsec_238_24:
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_update_by_connectionless_status
        l32r     a8, .Lsec_238_8
        callx8   a8
        j        .Lsec_238_68
.Lsec_238_30:
        l32r     a8, .Lsec_238_4
        l32i.n   a8, a8, 0
        l32i     a10, a8, 0x110
        l32i     a4, a3, 88
        l32i     a2, a3, 92
        callx8   a10
        sub      a10, a4, a10
        l16ui    a8, a3, 0x18a
        movi     a3, 0x3e8
        mull     a9, a8, a3
        muluh    a8, a8, a3
        movi     a3, 1
        bltu     a4, a10, .Lsec_238_59
        movi     a3, 0
.Lsec_238_59:
        sub      a11, a2, a11
        sub      a11, a11, a3
        bltu     a11, a8, .Lsec_238_68
        bne      a8, a11, .Lsec_238_24
        bgeu     a10, a9, .Lsec_238_24
.Lsec_238_68:
        retw.n
        .size   pm_connectionless_wake_interval_timeout_process, .-pm_connectionless_wake_interval_timeout_process
.Lsec_238_6a:

        .section .wifislpiram.47,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_239_0, g_pm
        .literal .Lsec_239_4, g_osi_funcs_p
        .literal .Lsec_239_8, pm_coex_separate_connectionless_window
        .literal .Lsec_239_c, pm_get_connectionless_status
        .literal .Lsec_239_10, wifi_gpio_debug
        .literal .Lsec_239_14, pm_wake_done
.Lsec_239_18:
        .global pm_connectionless_wake_window_timeout_process
        .type   pm_connectionless_wake_window_timeout_process, @function
pm_connectionless_wake_window_timeout_process:
        entry    a1, 32
        l32r     a3, .Lsec_239_0
        addmi    a2, a3, 0x100
        l8ui     a8, a2, 29
        bgeui    a8, 2, .Lsec_239_2d
        l8ui     a3, a3, 13
        beqz     a3, .Lsec_239_72
.Lsec_239_2d:
        l32r     a3, .Lsec_239_4
        l32i.n   a3, a3, 0
        l32i     a10, a3, 0x194
        callx8   a10
        or       a3, a10, a10
        beqz     a10, .Lsec_239_49
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_coex_separate_connectionless_window
        l32r     a8, .Lsec_239_8
        callx8   a8
        j        .Lsec_239_72
        .byte    0x00
        .byte    0x00
.Lsec_239_49:
        l8ui     a8, a2, 136
        beqz.n   a8, .Lsec_239_72
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_get_connectionless_status
        l32r     a8, .Lsec_239_c
        callx8   a8
        blti     a10, 1, .Lsec_239_72
        movi     a11, 1
        movi     a10, 17
        s8i      a3, a2, 136
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_gpio_debug
        l32r     a8, .Lsec_239_10
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_wake_done
        l32r     a8, .Lsec_239_14
        callx8   a8
.Lsec_239_72:
        retw.n
        .size   pm_connectionless_wake_window_timeout_process, .-pm_connectionless_wake_window_timeout_process
.Lsec_239_74:

        .section .text.pm_coex_restart_timeout_process,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_240_0, g_osi_funcs_p
        .literal .Lsec_240_4, g_pm
        .literal .Lsec_240_8, pm_coex_schm_process_restart
.Lsec_240_c:
        .global pm_coex_restart_timeout_process
        .type   pm_coex_restart_timeout_process, @function
pm_coex_restart_timeout_process:
        entry    a1, 32
        l32r     a8, .Lsec_240_0
        l32i.n   a8, a8, 0
        l32i     a10, a8, 0x194
        callx8   a10
        beqz.n   a10, .Lsec_240_2a
        l32r     a8, .Lsec_240_4
        l32i     a8, a8, 0x190
        bnez.n   a8, .Lsec_240_2a
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_coex_schm_process_restart
        l32r     a8, .Lsec_240_8
        callx8   a8
.Lsec_240_2a:
        retw.n
        .size   pm_coex_restart_timeout_process, .-pm_coex_restart_timeout_process
.Lsec_240_2c:

        .section .wifirxiram.51,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_241_0, g_pm
        .literal .Lsec_241_4, pm_check_state
        .literal .Lsec_241_8, .Lsec_149_4
        .literal .Lsec_241_c, pm_enable_dream_timer
        .literal .Lsec_241_10, pm_disable_dream_timer
        .literal .Lsec_241_14, pm_enable_sleep_delay_timer
        .literal .Lsec_241_18, pm_enable_active_timer
.Lsec_241_1c:
        .global pm_rx_data_process
        .type   pm_rx_data_process, @function
pm_rx_data_process:
        entry    a1, 32
        extui    a2, a2, 0, 8
        extui    a3, a3, 0, 8
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_check_state
        l32r     a8, .Lsec_241_4
        callx8   a8
        beqz     a10, .Lsec_241_94
        .reloc  ., R_XTENSA_ASM_EXPAND, .Lsec_149_4
        l32r     a8, .Lsec_241_8
        callx8   a8
        l32r     a8, .Lsec_241_0
        l8ui     a9, a8, 1
        bnei     a9, 1, .Lsec_241_8c
        l8ui     a10, a8, 24
        beqz     a10, .Lsec_241_94
        beqz.n   a2, .Lsec_241_71
        beqz.n   a3, .Lsec_241_59
        s8i      a9, a8, 18
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_enable_dream_timer
        l32r     a8, .Lsec_241_c
        callx8   a8
        j        .Lsec_241_94
.Lsec_241_59:
        s8i      a3, a8, 18
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_disable_dream_timer
        l32r     a8, .Lsec_241_10
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_enable_sleep_delay_timer
        l32r     a8, .Lsec_241_14
        callx8   a8
        j        .Lsec_241_94
.Lsec_241_71:
        beqz.n   a3, .Lsec_241_80
        s8i      a9, a8, 18
        memw
        memw
        j        .Lsec_241_94
        .byte    0x00
.Lsec_241_80:
        s8i      a3, a8, 18
        memw
        memw
        j        .Lsec_241_94
.Lsec_241_8c:
        bnez.n   a9, .Lsec_241_94
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_enable_active_timer
        l32r     a8, .Lsec_241_18
        callx8   a8
.Lsec_241_94:
        retw.n
        .size   pm_rx_data_process, .-pm_rx_data_process
.Lsec_241_96:

        .section .wifirxiram.52,"ax",@progbits
        .align  4
.Lsec_242_0:
        .global pm_hmac_rx_data_process
        .type   pm_hmac_rx_data_process, @function
pm_hmac_rx_data_process:
        entry    a1, 32
        retw.n
        .size   pm_hmac_rx_data_process, .-pm_hmac_rx_data_process
.Lsec_242_5:

        .section .text.pm_tx_data_done_process,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_243_0, g_mesh_is_started
        .literal .Lsec_243_4, g_pm_cfg
        .literal .Lsec_243_8, g_pm
        .literal .Lsec_243_c, g_osi_funcs_p
        .literal .Lsec_243_10, .Lsec_183_c
        .literal .Lsec_243_14, sta_reset_beacon_timeout
        .literal .Lsec_243_18, pm_enable_max_idle_timer
        .literal .Lsec_243_1c, ppCheckTxIdle
        .literal .Lsec_243_20, pm_enable_sleep_delay_timer
        .literal .Lsec_243_24, pm_enable_disconnected_sleep_delay_timer
        .literal .Lsec_243_28, pm_check_state
        .literal .Lsec_243_2c, .Lsec_149_4
        .literal .Lsec_243_30, pm_enable_active_timer
.Lsec_243_34:
        .global pm_tx_data_done_process
        .type   pm_tx_data_done_process, @function
pm_tx_data_done_process:
        entry    a1, 32
        extui    a2, a2, 0, 8
        extui    a3, a3, 0, 8
        extui    a4, a4, 0, 8
        beqz.n   a2, .Lsec_243_82
        l32r     a2, .Lsec_243_0
        l8ui     a2, a2, 0
        beqz.n   a2, .Lsec_243_4d
        bnez     a3, .Lsec_243_59
.Lsec_243_4d:
        .reloc  ., R_XTENSA_ASM_EXPAND, .Lsec_183_c
        l32r     a8, .Lsec_243_10
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, sta_reset_beacon_timeout
        l32r     a8, .Lsec_243_14
        callx8   a8
.Lsec_243_59:
        l32r     a2, .Lsec_243_4
        l8ui     a2, a2, 29
        beqz.n   a2, .Lsec_243_82
        beqz.n   a4, .Lsec_243_82
        l32r     a2, .Lsec_243_8
        l32i.n   a8, a2, 36
        bnez     a8, .Lsec_243_71
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_enable_max_idle_timer
        l32r     a8, .Lsec_243_18
        callx8   a8
.Lsec_243_71:
        l32r     a8, .Lsec_243_c
        l32i.n   a8, a8, 0
        l32i     a10, a8, 0x110
        callx8   a10
        s32i     a10, a2, 0x1e0
        s32i     a11, a2, 0x1e4
.Lsec_243_82:
        movi.n   a10, 1
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, ppCheckTxIdle
        l32r     a8, .Lsec_243_1c
        callx8   a8
        bnez.n   a10, .Lsec_243_ae
        l32r     a8, .Lsec_243_8
        l8ui     a2, a8, 13
        beqz     a2, .Lsec_243_a8
        l8ui     a2, a8, 1
        bnei     a2, 1, .Lsec_243_ae
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_enable_sleep_delay_timer
        l32r     a8, .Lsec_243_20
        callx8   a8
        j        .Lsec_243_ae
        .byte    0x00
.Lsec_243_a8:
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_enable_disconnected_sleep_delay_timer
        l32r     a8, .Lsec_243_24
        callx8   a8
.Lsec_243_ae:
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_check_state
        l32r     a8, .Lsec_243_28
        callx8   a8
        beqz.n   a10, .Lsec_243_de
        beqz.n   a4, .Lsec_243_de
        .reloc  ., R_XTENSA_ASM_EXPAND, .Lsec_149_4
        l32r     a8, .Lsec_243_2c
        callx8   a8
        l32r     a8, .Lsec_243_8
        l8ui     a2, a8, 24
        bnez     a2, .Lsec_243_d0
        movi     a2, 1
        s8i      a2, a8, 25
        memw
.Lsec_243_d0:
        l8ui     a2, a8, 1
        bnez.n   a2, .Lsec_243_de
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_enable_active_timer
        l32r     a8, .Lsec_243_30
        callx8   a8
.Lsec_243_de:
        retw.n
        .size   pm_tx_data_done_process, .-pm_tx_data_done_process
.Lsec_243_e0:

        .section .text.pm_tx_null_data_done_quick_wake_process,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_244_0, g_pm
        .literal .Lsec_244_4, g_osi_funcs_p
        .literal .Lsec_244_8, sta_reset_beacon_timeout
        .literal .Lsec_244_c, ppCheckTxIdle
        .literal .Lsec_244_10, pm_send_nullfunc
.Lsec_244_14:
        .global pm_tx_null_data_done_quick_wake_process
        .type   pm_tx_null_data_done_quick_wake_process, @function
pm_tx_null_data_done_quick_wake_process:
        entry    a1, 32
        l32r     a3, .Lsec_244_0
        extui    a2, a2, 0, 8
        addmi    a8, a3, 0x100
        l8ui     a8, a8, 212
        bnez.n   a8, .Lsec_244_2a
.Lsec_244_25:
        movi.n   a2, 1
        j        .Lsec_244_72
.Lsec_244_2a:
        beqz.n   a2, .Lsec_244_32
        .reloc  ., R_XTENSA_ASM_EXPAND, sta_reset_beacon_timeout
        l32r     a8, .Lsec_244_8
        callx8   a8
.Lsec_244_32:
        l8ui     a2, a3, 17
        beqz.n   a2, .Lsec_244_54
        movi.n   a8, 0
        movi.n   a10, 0
        s8i      a8, a3, 17
        memw
        l32r     a3, .Lsec_244_4
        l32i     a3, a3, 0
        l32i     a3, a3, 0x1a0
        memw
        callx8   a3
        j        .Lsec_244_72
        .byte    0x00
.Lsec_244_54:
        l8ui     a2, a3, 18
        bnez     a2, .Lsec_244_25
        movi.n   a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, ppCheckTxIdle
        l32r     a8, .Lsec_244_c
        callx8   a8
        bnez     a10, .Lsec_244_25
        l8ui     a3, a3, 20
        bnez.n   a3, .Lsec_244_72
        movi.n   a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_send_nullfunc
        l32r     a8, .Lsec_244_10
        callx8   a8
.Lsec_244_72:
        retw.n
        .size   pm_tx_null_data_done_quick_wake_process, .-pm_tx_null_data_done_quick_wake_process
.Lsec_244_74:

        .section .text.pm_tx_null_data_done_process,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_245_0, g_mesh_is_started
        .literal .Lsec_245_4, g_mesh_is_root
        .literal .Lsec_245_8, g_mesh_init_ps_type
        .literal .Lsec_245_c, g_pm
        .literal .Lsec_245_10, esp_mesh_quick_funcs
        .literal .Lsec_245_14, g_pm_cfg
        .literal .Lsec_245_18, g_osi_funcs_p
        .literal .Lsec_245_1c, g_pm_cnt
        .literal .Lsec_245_20, .Lsec_183_c
        .literal .Lsec_245_24, pm_check_state
        .literal .Lsec_245_28, pm_dream
        .literal .Lsec_245_2c, pm_enable_max_idle_timer
        .literal .Lsec_245_30, pm_tx_null_data_done_quick_wake_process
        .literal .Lsec_245_34, .Lsec_151_c
        .literal .Lsec_245_38, pm_enable_active_timer
        .literal .Lsec_245_3c, ppProcessWaitingQueue
        .literal .Lsec_245_40, pm_sleep
        .literal .Lsec_245_44, pm_enable_sleep_delay_timer
        .literal .Lsec_245_48, pm_is_in_wifi_slice_threshold
        .literal .Lsec_245_4c, pm_send_nullfunc
        .literal .Lsec_245_50, pm_enable_sleep_delay_timer
        .literal .Lsec_245_54, pm_offchan_execute_ready_cb
.Lsec_245_58:
        .global pm_tx_null_data_done_process
        .type   pm_tx_null_data_done_process, @function
pm_tx_null_data_done_process:
        entry    a1, 32
        l32r     a6, .Lsec_245_0
        l8ui     a5, a6, 0
        l32r     a4, .Lsec_245_c
        beqz.n   a5, .Lsec_245_9c
        l32r     a5, .Lsec_245_4
        memw
        l8ui     a5, a5, 0
        extui    a5, a5, 0, 8
        bnez.n   a5, .Lsec_245_9c
        l32r     a5, .Lsec_245_8
        l32i.n   a5, a5, 0
        beqz.n   a5, .Lsec_245_9c
        bnez.n   a3, .Lsec_245_8c
.Lsec_245_7d:
        movi.n   a3, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, .Lsec_183_c
        l32r     a8, .Lsec_245_20
        callx8   a8
        mov.n    a5, a3
        j        .Lsec_245_a1
        .byte    0x00
        .byte    0x00
.Lsec_245_8c:
        l32r     a5, .Lsec_245_10
        mov.n    a11, a3
        mov.n    a10, a2
        l32i     a5, a5, 132
        l8ui     a12, a4, 9
        callx8   a5
.Lsec_245_9c:
        beqi     a3, 1, .Lsec_245_7d
        movi.n   a5, 0
.Lsec_245_a1:
        l8ui     a8, a4, 14
        beqz.n   a8, .Lsec_245_b8
        l8ui     a9, a4, 15
        bnez.n   a9, .Lsec_245_bd
        bnez.n   a2, .Lsec_245_c4
        movi.n   a10, 15
        l8ui     a9, a4, 9
        bltu     a10, a9, .Lsec_245_c2
        j        .Lsec_245_c4
.Lsec_245_b8:
        movi.n   a5, 1
        j        .Lsec_245_c4
.Lsec_245_bd:
        mov.n    a5, a9
        j        .Lsec_245_c4
.Lsec_245_c2:
        mov.n    a5, a8
.Lsec_245_c4:
        l8ui     a8, a6, 0
        movi.n   a6, 1
        xor      a8, a8, a6
        extui    a8, a8, 0, 8
        bnez     a8, .Lsec_245_d4
        beqz.n   a5, .Lsec_245_e9
.Lsec_245_d4:
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_check_state
        l32r     a8, .Lsec_245_24
        callx8   a8
        bnez.n   a10, .Lsec_245_e9
        l8ui     a6, a4, 13
        bnez.n   a6, .Lsec_245_e9
.Lsec_245_e1:
        movi.n   a10, 0
        j        .Lsec_245_28c
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_245_e9:
        l8ui     a6, a4, 19
        bnez     a6, .Lsec_245_f5
        l8ui     a8, a4, 20
        beqz     a8, .Lsec_245_292
.Lsec_245_f5:
        beqz     a5, .Lsec_245_1fd
        l8ui     a5, a4, 1
        bnei     a5, 2, .Lsec_245_104
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_dream
        l32r     a8, .Lsec_245_28
        callx8   a8
.Lsec_245_104:
        l32r     a5, .Lsec_245_14
        l8ui     a5, a5, 29
        beqz.n   a5, .Lsec_245_13c
        addmi    a5, a4, 0x100
        l8ui     a6, a5, 232
        beqz.n   a6, .Lsec_245_13c
        l32i.n   a6, a4, 36
        bnez.n   a6, .Lsec_245_121
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_enable_max_idle_timer
        l32r     a8, .Lsec_245_2c
        callx8   a8
        j        .Lsec_245_133
.Lsec_245_121:
        l32r     a6, .Lsec_245_18
        l32i     a6, a6, 0
        l32i     a10, a6, 0x110
        callx8   a10
        s32i     a10, a4, 0x1e0
        s32i     a11, a4, 0x1e4
.Lsec_245_133:
        movi     a6, 0
        s8i      a6, a5, 232
        memw
.Lsec_245_13c:
        bnez     a2, .Lsec_245_1ac
        l8ui     a5, a4, 19
        s8i      a2, a4, 9
        memw
        beqz     a5, .Lsec_245_292
        addmi    a5, a4, 0x100
        l8ui     a6, a5, 200
        beqz.n   a6, .Lsec_245_156
        s8i      a2, a5, 200
.Lsec_245_156:
        movi.n   a2, 0
        s8i      a2, a4, 19
        memw
        l8ui     a2, a4, 24
        beqz     a2, .Lsec_245_23c
        movi     a2, 0
        addi     a3, a3, -1
        movi     a10, 1
        movnez   a10, a2, a3
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_tx_null_data_done_quick_wake_process
        l32r     a8, .Lsec_245_30
        callx8   a8
        bne      a10, a2, .Lsec_245_17f
        j        .Lsec_245_292
.Lsec_245_17f:
        mov.n    a10, a2
        s8i      a2, a4, 18
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, .Lsec_151_c
        l32r     a8, .Lsec_245_34
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_enable_active_timer
        l32r     a8, .Lsec_245_38
        callx8   a8
        l32r     a3, .Lsec_245_1c
        l32i.n   a2, a3, 32
        addi.n   a2, a2, 1
        s32i.n   a2, a3, 32
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, ppProcessWaitingQueue
        l32r     a8, .Lsec_245_3c
        callx8   a8
        j        .Lsec_245_292
        .byte    0x00
.Lsec_245_1ac:
        movi.n   a2, 0
        l8ui     a3, a4, 20
        s8i      a2, a4, 10
        memw
        beqz     a3, .Lsec_245_e1
        s8i      a2, a4, 20
        memw
        addmi    a2, a4, 0x100
        l8ui     a2, a2, 212
        beqz.n   a2, .Lsec_245_1e8
        l32r     a2, .Lsec_245_18
        l32i.n   a2, a2, 0
        l32i     a10, a2, 0x194
        memw
        callx8   a10
        beqz.n   a10, .Lsec_245_1dd
        l8ui     a2, a4, 25
        bnez.n   a2, .Lsec_245_1e8
.Lsec_245_1dd:
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_sleep
        l32r     a8, .Lsec_245_40
        callx8   a8
        j        .Lsec_245_1ee
        .byte    0x00
        .byte    0x00
.Lsec_245_1e8:
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_enable_sleep_delay_timer
        l32r     a8, .Lsec_245_44
        callx8   a8
.Lsec_245_1ee:
        l32r     a3, .Lsec_245_1c
        l32i.n   a2, a3, 28
        addi.n   a2, a2, 1
        s32i.n   a2, a3, 28
        memw
        j        .Lsec_245_e1
.Lsec_245_1fd:
        bnez     a2, .Lsec_245_24c
        beqz     a6, .Lsec_245_292
        l32r     a3, .Lsec_245_18
        l32i     a3, a3, 0
        l32i     a10, a3, 0x110
        s8i      a2, a4, 19
        memw
        memw
        callx8   a10
        movi     a12, 0x271
        slli     a12, a12, 3
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_is_in_wifi_slice_threshold
        l32r     a8, .Lsec_245_48
        callx8   a8
        beqz.n   a10, .Lsec_245_239
        l8ui     a3, a4, 9
        addi.n   a3, a3, 1
        s8i      a3, a4, 9
        memw
        memw
        j        .Lsec_245_26a
        .byte    0x00
        .byte    0x00
.Lsec_245_239:
        s8i      a2, a4, 9
.Lsec_245_23c:
        movi.n   a2, 1
        s8i      a2, a4, 25
        memw
        memw
        j        .Lsec_245_292
        .byte    0x00
        .byte    0x00
.Lsec_245_24c:
        bnei     a2, 1, .Lsec_245_292
        l8ui     a3, a4, 20
        beqz.n   a3, .Lsec_245_292
        l8ui     a3, a4, 24
        s8i      a5, a4, 20
        memw
        beqz.n   a3, .Lsec_245_278
        l8ui     a3, a4, 10
        addi.n   a3, a3, 1
        s8i      a3, a4, 10
        memw
.Lsec_245_26a:
        mov.n    a10, a2
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_send_nullfunc
        l32r     a8, .Lsec_245_4c
        callx8   a8
        j        .Lsec_245_292
.Lsec_245_278:
        s8i      a5, a4, 10
        s8i      a2, a4, 25
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_enable_sleep_delay_timer
        l32r     a8, .Lsec_245_50
        callx8   a8
        mov.n    a10, a2
.Lsec_245_28c:
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_offchan_execute_ready_cb
        l32r     a8, .Lsec_245_54
        callx8   a8
.Lsec_245_292:
        retw.n
        .size   pm_tx_null_data_done_process, .-pm_tx_null_data_done_process
.Lsec_245_294:

        .section .text.pm_go_to_wake,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_246_0, g_pm
        .literal .Lsec_246_4, g_mesh_is_started
        .literal .Lsec_246_8, g_mesh_is_root
        .literal .Lsec_246_c, g_mesh_init_ps_type
        .literal .Lsec_246_10, g_pm+0x13
        .literal .Lsec_246_14, g_pm+0x14
        .literal .Lsec_246_18, esp_mesh_quick_funcs
        .literal .Lsec_246_1c, g_osi_funcs_p
        .literal .Lsec_246_20, pm_dream
        .literal .Lsec_246_24, ppCheckTxConnTrafficIdle
        .literal .Lsec_246_28, pm_tx_null_data_done_process
        .literal .Lsec_246_2c, pm_is_in_wifi_slice_threshold
        .literal .Lsec_246_30, pm_send_nullfunc
        .literal .Lsec_246_34, pm_tx_null_data_done_process
.Lsec_246_38:
        .global pm_go_to_wake
        .type   pm_go_to_wake, @function
pm_go_to_wake:
        entry    a1, 32
        l32r     a4, .Lsec_246_0
        extui    a5, a2, 0, 8
        movi.n   a2, 0
        extui    a3, a3, 0, 8
        l8ui     a8, a4, 13
        bne      a8, a2, .Lsec_246_4f
        j        .Lsec_246_12a
.Lsec_246_4f:
        l8ui     a8, a4, 14
        bne      a8, a2, .Lsec_246_58
        j        .Lsec_246_12a
.Lsec_246_58:
        l8ui     a8, a4, 15
        beq      a8, a2, .Lsec_246_61
        j        .Lsec_246_12a
.Lsec_246_61:
        l8ui     a2, a4, 24
        bnez.n   a2, .Lsec_246_78
.Lsec_246_66:
        movi.n   a2, 1
        s8i      a2, a4, 25
        memw
        movi.n   a2, 1
        memw
        j        .Lsec_246_12a
        .byte    0x00
        .byte    0x00
.Lsec_246_78:
        l8ui     a2, a4, 1
        bnei     a2, 2, .Lsec_246_84
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_dream
        l32r     a8, .Lsec_246_20
        callx8   a8
.Lsec_246_84:
        beqz.n   a5, .Lsec_246_b8
        l32r     a2, .Lsec_246_4
        l8ui     a2, a2, 0
        beqz.n   a2, .Lsec_246_b8
        l32r     a2, .Lsec_246_8
        memw
        l8ui     a10, a2, 0
        extui    a10, a10, 0, 8
        bnez.n   a10, .Lsec_246_b8
        l32r     a2, .Lsec_246_c
        l32i     a2, a2, 0
        beqz     a2, .Lsec_246_b8
        l32r     a2, .Lsec_246_18
        l32r     a12, .Lsec_246_10
        l32i     a2, a2, 140
        l32r     a11, .Lsec_246_14
        callx8   a2
        j        .Lsec_246_128
        .byte    0x00
.Lsec_246_b8:
        l8ui     a2, a4, 19
        bnez     a2, .Lsec_246_112
        bnez     a3, .Lsec_246_112
        movi.n   a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, ppCheckTxConnTrafficIdle
        l32r     a8, .Lsec_246_24
        callx8   a8
        mov.n    a2, a10
        beqz.n   a10, .Lsec_246_e5
        movi     a8, 1
        movi     a11, 1
        or       a10, a3, a3
        s8i      a8, a4, 19
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_tx_null_data_done_process
        l32r     a8, .Lsec_246_28
        callx8   a8
.Lsec_246_e5:
        l32r     a3, .Lsec_246_1c
        l32i.n   a3, a3, 0
        l32i     a10, a3, 0x110
        callx8   a10
        movi     a12, 0x271
        slli     a12, a12, 3
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_is_in_wifi_slice_threshold
        l32r     a8, .Lsec_246_2c
        callx8   a8
        beqz.n   a10, .Lsec_246_10c
        movi.n   a10, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_send_nullfunc
        l32r     a8, .Lsec_246_30
        callx8   a8
        movi.n   a2, 3
        j        .Lsec_246_12a
        .byte    0x00
.Lsec_246_10c:
        beqz     a2, .Lsec_246_66
        j        .Lsec_246_128
.Lsec_246_112:
        movi.n   a2, 1
        movi     a11, 1
        movi.n   a10, 0
        s8i      a2, a4, 19
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_tx_null_data_done_process
        l32r     a8, .Lsec_246_34
        callx8   a8
.Lsec_246_128:
        movi.n   a2, 2
.Lsec_246_12a:
        retw.n
        .size   pm_go_to_wake, .-pm_go_to_wake
.Lsec_246_12c:

        .section .text.pm_send_probe_stop,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_247_0, g_pm
        .literal .Lsec_247_4, pm_go_to_wake
        .literal .Lsec_247_8, ppCheckTxConnTrafficIdle
        .literal .Lsec_247_c, pm_enable_active_timer
        .literal .Lsec_247_10, pm_wake_done
.Lsec_247_14:
        .global pm_send_probe_stop
        .type   pm_send_probe_stop, @function
pm_send_probe_stop:
        entry    a1, 32
        l32r     a8, .Lsec_247_0
        addmi    a9, a8, 0x100
        l8ui     a10, a9, 29
        bgeui    a10, 2, .Lsec_247_29
        l8ui     a10, a8, 13
        beqz     a10, .Lsec_247_6b
.Lsec_247_29:
        l8ui     a10, a8, 15
        beqz.n   a10, .Lsec_247_6b
        movi.n   a10, 0
        s8i      a10, a8, 15
        memw
        l8ui     a8, a9, 28
        beqz     a8, .Lsec_247_4e
.Lsec_247_3c:
        movi.n   a11, 0
        mov.n    a10, a11
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_go_to_wake
        l32r     a8, .Lsec_247_4
        callx8   a8
        j        .Lsec_247_5f
        .byte    0x00
        .byte    0x00
.Lsec_247_4e:
        l8ui     a8, a9, 200
        bnez     a8, .Lsec_247_3c
        movi.n   a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, ppCheckTxConnTrafficIdle
        l32r     a8, .Lsec_247_8
        callx8   a8
        bnez     a10, .Lsec_247_3c
.Lsec_247_5f:
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_enable_active_timer
        l32r     a8, .Lsec_247_c
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_wake_done
        l32r     a8, .Lsec_247_10
        callx8   a8
.Lsec_247_6b:
        retw.n
        .size   pm_send_probe_stop, .-pm_send_probe_stop
.Lsec_247_6d:

        .section .text.pm_on_channel,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_248_0, g_pm
        .literal .Lsec_248_4, pm_wake_done
        .literal .Lsec_248_8, .Lsec_147_8
        .literal .Lsec_248_c, pm_go_to_wake
        .literal .Lsec_248_10, ppCheckTxConnTrafficIdle
        .literal .Lsec_248_14, pm_wake_done
        .literal .Lsec_248_18, pm_enable_active_timer
.Lsec_248_1c:
        .global pm_on_channel
        .type   pm_on_channel, @function
pm_on_channel:
        entry    a1, 32
        l32r     a8, .Lsec_248_0
        addmi    a2, a8, 0x100
        l8ui     a9, a2, 29
        bgeui    a9, 2, .Lsec_248_30
        l8ui     a9, a8, 13
        beqz.n   a9, .Lsec_248_35
.Lsec_248_30:
        l8ui     a9, a8, 14
        beqz.n   a9, .Lsec_248_46
.Lsec_248_35:
        movi.n   a2, 1
        s8i      a2, a8, 14
        memw
        memw
        j        .Lsec_248_aa
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_248_46:
        movi.n   a9, 1
        s8i      a9, a8, 14
        memw
        l8ui     a9, a8, 13
        bnez.n   a9, .Lsec_248_61
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_wake_done
        l32r     a8, .Lsec_248_4
        callx8   a8
        j        .Lsec_248_aa
        .byte    0x00
        .byte    0x00
.Lsec_248_61:
        l8ui     a8, a8, 24
        beqz     a8, .Lsec_248_6d
        .reloc  ., R_XTENSA_ASM_EXPAND, .Lsec_147_8
        l32r     a8, .Lsec_248_8
        callx8   a8
.Lsec_248_6d:
        l8ui     a8, a2, 28
        beqz.n   a8, .Lsec_248_81
.Lsec_248_72:
        movi.n   a11, 0
        mov.n    a10, a11
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_go_to_wake
        l32r     a8, .Lsec_248_c
        callx8   a8
        bnez.n   a10, .Lsec_248_93
        j        .Lsec_248_9b
.Lsec_248_81:
        l8ui     a8, a2, 200
        bnez     a8, .Lsec_248_72
        movi     a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, ppCheckTxConnTrafficIdle
        l32r     a8, .Lsec_248_10
        callx8   a8
        bnez     a10, .Lsec_248_72
.Lsec_248_93:
        movi.n   a8, 0
        s8i      a8, a2, 28
        memw
.Lsec_248_9b:
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_wake_done
        l32r     a8, .Lsec_248_14
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_enable_active_timer
        l32r     a8, .Lsec_248_18
        callx8   a8
.Lsec_248_aa:
        retw.n
        .size   pm_on_channel, .-pm_on_channel
.Lsec_248_ac:

        .section .text.pm_update_params,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_249_0, g_osi_funcs_p
        .literal .Lsec_249_4, g_pm
        .literal .Lsec_249_8, g_pm_cfg
        .literal .Lsec_249_c, pm_coex_schm_overall_period_get
        .literal .Lsec_249_10, pm_go_to_wake
.Lsec_249_14:
        .global pm_update_params
        .type   pm_update_params, @function
pm_update_params:
        entry    a1, 32
        l32r     a4, .Lsec_249_0
        l32i.n   a2, a4, 0
        l32i     a10, a2, 0x194
        callx8   a10
        l32r     a2, .Lsec_249_4
        l32r     a3, .Lsec_249_8
        bnez.n   a10, .Lsec_249_38
        l32i.n   a8, a3, 20
        l32i.n   a3, a3, 4
        s32i.n   a8, a2, 48
        memw
        j        .Lsec_249_54
        .byte    0x00
        .byte    0x00
.Lsec_249_38:
        addmi    a8, a2, 0x100
        l8ui     a8, a8, 213
        beqz.n   a8, .Lsec_249_45
        l32i.n   a10, a3, 20
        j        .Lsec_249_50
.Lsec_249_45:
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_coex_schm_overall_period_get
        l32r     a8, .Lsec_249_c
        callx8   a8
        l32i.n   a8, a2, 44
        mull     a10, a10, a8
.Lsec_249_50:
        l32i.n   a3, a3, 8
        s32i.n   a10, a2, 48
.Lsec_249_54:
        l32i     a8, a2, 40
        beq      a8, a3, .Lsec_249_5d
        s32i     a3, a2, 40
.Lsec_249_5d:
        l32i.n   a3, a4, 0
        l32i     a10, a3, 0x194
        memw
        callx8   a10
        l8ui     a3, a2, 12
        beq      a10, a3, .Lsec_249_97
        bnei     a3, 1, .Lsec_249_89
        l32i.n   a10, a2, 36
        bnez.n   a10, .Lsec_249_89
        addmi    a8, a2, 0x100
        mov.n    a11, a10
        s8i      a3, a8, 200
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_go_to_wake
        l32r     a8, .Lsec_249_10
        callx8   a8
.Lsec_249_89:
        l32i.n   a3, a4, 0
        l32i     a10, a3, 0x194
        callx8   a10
        s8i      a10, a2, 12
        memw
.Lsec_249_97:
        memw
        retw.n
        .size   pm_update_params, .-pm_update_params
.Lsec_249_9c:

        .section .text.pm_coex_tbtt_process,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_250_0, g_pm+0xf4
        .literal .Lsec_250_4, g_osi_funcs_p
        .literal .Lsec_250_8, g_pm
        .literal .Lsec_250_c, 0x51eb851f
        .literal .Lsec_250_10, pm_coex_schm_overall_period_get
        .literal .Lsec_250_14, pm_coex_schm_process_restart
        .literal .Lsec_250_18, pm_is_sleeping
        .literal .Lsec_250_1c, .Lsec_147_8
        .literal .Lsec_250_20, ppCheckTxConnTrafficIdle
        .literal .Lsec_250_24, pm_is_sleeping
        .literal .Lsec_250_28, .Lsec_147_8
        .literal .Lsec_250_2c, pm_go_to_wake
.Lsec_250_30:
        .global pm_coex_tbtt_process
        .type   pm_coex_tbtt_process, @function
pm_coex_tbtt_process:
        entry    a1, 32
        l32r     a5, .Lsec_250_4
        l32i     a4, a5, 0
        l32r     a10, .Lsec_250_0
        l32i     a4, a4, 236
        callx8   a4
        movi.n   a8, 1
        l32r     a4, .Lsec_250_8
        s8i      a8, a4, 24
        memw
        l32i.n   a8, a5, 0
        l32i     a10, a8, 0x194
        memw
        callx8   a10
        beqz     a10, .Lsec_250_c8
        l8ui     a8, a4, 14
        beqz.n   a8, .Lsec_250_87
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_coex_schm_overall_period_get
        l32r     a8, .Lsec_250_10
        callx8   a8
        movi.n   a9, 1
        l32i.n   a8, a4, 44
        mull     a10, a10, a8
        add.n    a2, a10, a2
        bltu     a2, a10, .Lsec_250_74
        movi.n   a9, 0
.Lsec_250_74:
        add.n    a9, a9, a3
        s32i     a2, a4, 104
        l8ui     a2, a4, 14
        s32i     a9, a4, 108
        bnez.n   a2, .Lsec_250_a0
        memw
        j        .Lsec_250_106
.Lsec_250_87:
        l32i.n   a9, a4, 44
        movi.n   a5, 1
        add.n    a2, a9, a2
        bltu     a2, a9, .Lsec_250_92
        mov.n    a5, a8
.Lsec_250_92:
        add.n    a5, a5, a3
        s32i     a2, a4, 104
        s32i     a5, a4, 108
        memw
        j        .Lsec_250_106
.Lsec_250_a0:
        l32r     a2, .Lsec_250_c
        muluh    a10, a8, a2
        srli     a10, a10, 5
        l32i.n   a2, a5, 0
        l32i     a2, a2, 0x1b8
        callx8   a2
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_coex_schm_process_restart
        l32r     a8, .Lsec_250_14
        callx8   a8
        movi.n   a11, 0
        l32i.n   a2, a5, 0
        mov.n    a10, a11
        l32i     a2, a2, 0x19c
        l32i.n   a12, a4, 52
        callx8   a2
        j        .Lsec_250_106
.Lsec_250_c8:
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_is_sleeping
        l32r     a8, .Lsec_250_18
        callx8   a8
        bnez     a10, .Lsec_250_14d
        l8ui     a2, a4, 13
        beqz.n   a2, .Lsec_250_e5
        l8ui     a2, a4, 14
        beqz     a2, .Lsec_250_14d
        addmi    a2, a4, 0x200
        l8ui     a2, a2, 30
        bnez     a2, .Lsec_250_14d
.Lsec_250_e5:
        .reloc  ., R_XTENSA_ASM_EXPAND, .Lsec_147_8
        l32r     a8, .Lsec_250_1c
        callx8   a8
        movi     a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, ppCheckTxConnTrafficIdle
        l32r     a8, .Lsec_250_20
        callx8   a8
        beqz     a10, .Lsec_250_14d
        l8ui     a2, a4, 14
        movi.n   a11, 1
        movi.n   a10, 0
        bnez     a2, .Lsec_250_144
        j        .Lsec_250_14d
        .byte    0x00
        .byte    0x00
.Lsec_250_106:
        movi.n   a2, 0
        s32i     a2, a4, 64
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_is_sleeping
        l32r     a8, .Lsec_250_24
        callx8   a8
        bne      a10, a2, .Lsec_250_12f
        l8ui     a2, a4, 13
        beqz.n   a2, .Lsec_250_129
        l8ui     a2, a4, 14
        beqz.n   a2, .Lsec_250_12f
        addmi    a2, a4, 0x200
        l8ui     a2, a2, 30
        bnez.n   a2, .Lsec_250_12f
.Lsec_250_129:
        .reloc  ., R_XTENSA_ASM_EXPAND, .Lsec_147_8
        l32r     a8, .Lsec_250_28
        callx8   a8
.Lsec_250_12f:
        l32i.n   a2, a4, 36
        beqz.n   a2, .Lsec_250_138
        l8ui     a2, a4, 25
        beqz.n   a2, .Lsec_250_14d
.Lsec_250_138:
        movi.n   a2, 0
        movi.n   a11, 0
        s8i      a2, a4, 25
        memw
        movi.n   a10, 1
.Lsec_250_144:
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_go_to_wake
        l32r     a8, .Lsec_250_2c
        callx8   a8
.Lsec_250_14d:
        retw.n
        .size   pm_coex_tbtt_process, .-pm_coex_tbtt_process
.Lsec_250_14f:

        .section .text.pm_active_timeout_process,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_251_0, g_pm_cnt
        .literal .Lsec_251_4, g_pm
        .literal .Lsec_251_8, g_pm_cfg
        .literal .Lsec_251_c, pm_go_to_wake
        .literal .Lsec_251_10, pm_enable_active_timer
        .literal .Lsec_251_14, ppCheckTxIdle
        .literal .Lsec_251_18, pm_go_to_sleep
.Lsec_251_1c:
        .global pm_active_timeout_process
        .type   pm_active_timeout_process, @function
pm_active_timeout_process:
        entry    a1, 32
        l32r     a9, .Lsec_251_0
        l32i.n   a8, a9, 20
        addi.n   a8, a8, 1
        s32i.n   a8, a9, 20
        l32r     a8, .Lsec_251_4
        l8ui     a9, a8, 13
        beqz     a9, .Lsec_251_85
        l8ui     a9, a8, 14
        beqz     a9, .Lsec_251_85
        l8ui     a9, a8, 15
        bnez     a9, .Lsec_251_85
        l32r     a9, .Lsec_251_8
        l8ui     a9, a9, 29
        l32i.n   a10, a8, 36
        beqz.n   a9, .Lsec_251_52
        bnez.n   a10, .Lsec_251_60
        memw
        j        .Lsec_251_85
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_251_52:
        bnez.n   a10, .Lsec_251_60
        mov.n    a11, a10
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_go_to_wake
        l32r     a8, .Lsec_251_c
        callx8   a8
        j        .Lsec_251_6a
        .byte    0x00
.Lsec_251_60:
        l8ui     a9, a8, 1
        bnez.n   a9, .Lsec_251_85
        l8ui     a8, a8, 18
        beqz.n   a8, .Lsec_251_74
.Lsec_251_6a:
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_enable_active_timer
        l32r     a8, .Lsec_251_10
        callx8   a8
        j        .Lsec_251_85
        .byte    0x00
.Lsec_251_74:
        movi.n   a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, ppCheckTxIdle
        l32r     a8, .Lsec_251_14
        callx8   a8
        bnez     a10, .Lsec_251_6a
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_go_to_sleep
        l32r     a8, .Lsec_251_18
        callx8   a8
.Lsec_251_85:
        retw.n
        .size   pm_active_timeout_process, .-pm_active_timeout_process
.Lsec_251_87:

        .section .wifislprxiram.43,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_252_0, g_pm
        .literal .Lsec_252_4, g_osi_funcs_p
        .literal .Lsec_252_8, g_pm_cnt
        .literal .Lsec_252_c, g_pm_cfg
        .literal .Lsec_252_10, pm_keep_alive
        .literal .Lsec_252_14, pm_is_in_wifi_slice_threshold
        .literal .Lsec_252_18, pm_go_to_wake
        .literal .Lsec_252_1c, pm_is_in_wifi_slice_threshold
        .literal .Lsec_252_20, pm_enable_dream_timer
        .literal .Lsec_252_24, pm_send_nullfunc
        .literal .Lsec_252_28, pm_disable_sleep_delay_timer
        .literal .Lsec_252_2c, pm_sleep
        .literal .Lsec_252_30, .Lsec_150_8
        .literal .Lsec_252_34, pm_active_timeout_process
        .literal .Lsec_252_38, pm_send_nullfunc
.Lsec_252_3c:
        .global pm_process_tim
        .type   pm_process_tim, @function
pm_process_tim:
        entry    a1, 48
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_keep_alive
        l32r     a8, .Lsec_252_10
        callx8   a8
        l32r     a4, .Lsec_252_0
        extui    a2, a2, 0, 8
        extui    a3, a3, 0, 8
        or       a5, a10, a10
        l8ui     a7, a4, 1
        beqi     a7, 1, .Lsec_252_5a
        j        .Lsec_252_1ba
.Lsec_252_5a:
        beqz     a2, .Lsec_252_c9
        l32r     a5, .Lsec_252_4
        l32i     a2, a5, 0
        l32i     a10, a2, 0x110
        callx8   a10
        movi     a12, 0x271
        slli     a12, a12, 3
        mov.n    a2, a10
        mov.n    a3, a11
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_is_in_wifi_slice_threshold
        l32r     a8, .Lsec_252_14
        callx8   a8
        beqz.n   a10, .Lsec_252_bc
        l32i     a12, a4, 96
        s8i      a7, a4, 18
        memw
        l32i     a4, a4, 100
        bltu     a3, a4, .Lsec_252_90
        bne      a4, a3, .Lsec_252_a2
        bgeu     a2, a12, .Lsec_252_a2
.Lsec_252_90:
        l32i.n   a3, a5, 0
        sub      a12, a12, a2
        movi.n   a11, 0
        movi.n   a10, 1
        l32i     a3, a3, 0x19c
        memw
        callx8   a3
.Lsec_252_a2:
        movi.n   a11, 0
        movi.n   a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_go_to_wake
        l32r     a8, .Lsec_252_18
        callx8   a8
        l32r     a3, .Lsec_252_8
        l32i.n   a2, a3, 8
        addi.n   a2, a2, 1
        s32i.n   a2, a3, 8
        memw
        j        .Lsec_252_208
        .byte    0x00
.Lsec_252_bc:
        s8i      a7, a4, 25
        memw
        memw
        j        .Lsec_252_208
        .byte    0x00
.Lsec_252_c9:
        beqz     a3, .Lsec_252_17b
        l32r     a2, .Lsec_252_4
        l32i     a3, a2, 0
        l32i     a10, a3, 0x110
        callx8   a10
        movi     a12, 0x3e8
        mov.n    a6, a10
        mov.n    a2, a11
        s32i.n   a10, a1, 0
        or       a3, a11, a11
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_is_in_wifi_slice_threshold
        l32r     a8, .Lsec_252_1c
        callx8   a8
        beqz     a10, .Lsec_252_208
        s8i      a7, a4, 18
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_enable_dream_timer
        l32r     a8, .Lsec_252_20
        callx8   a8
        l32i     a8, a4, 100
        l32i     a7, a4, 96
        bltu     a2, a8, .Lsec_252_110
        bne      a8, a2, .Lsec_252_13c
        l32i.n   a3, a1, 0
        bgeu     a3, a7, .Lsec_252_13c
.Lsec_252_110:
        sub      a9, a7, a6
        movi.n   a3, 0
        l32i     a12, a4, 52
        movi     a6, 1
        bltu     a7, a9, .Lsec_252_121
        or       a6, a3, a3
.Lsec_252_121:
        sub      a8, a8, a2
        sub      a8, a8, a6
        bne      a3, a8, .Lsec_252_12d
        minu     a12, a12, a9
.Lsec_252_12d:
        l32r     a3, .Lsec_252_4
        movi.n   a11, 0
        movi.n   a10, 2
        l32i.n   a2, a3, 0
        l32i     a2, a2, 0x19c
        callx8   a2
.Lsec_252_13c:
        l8ui     a2, a4, 19
        bnez.n   a2, .Lsec_252_16c
        movi.n   a3, 1
        l8ui     a2, a4, 20
        xor      a2, a2, a3
        extui    a2, a2, 0, 8
        beqz.n   a2, .Lsec_252_16c
        beqz.n   a5, .Lsec_252_16c
        l32r     a2, .Lsec_252_c
        l8ui     a2, a2, 29
        beqz.n   a2, .Lsec_252_161
        addmi    a4, a4, 0x100
        s8i      a3, a4, 232
        memw
.Lsec_252_161:
        movi.n   a10, 1
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_send_nullfunc
        l32r     a8, .Lsec_252_24
        callx8   a8
.Lsec_252_16c:
        l32r     a3, .Lsec_252_8
        l32i.n   a2, a3, 12
        addi.n   a2, a2, 1
        s32i.n   a2, a3, 12
        memw
        j        .Lsec_252_208
.Lsec_252_17b:
        l8ui     a2, a4, 19
        s8i      a3, a4, 18
        memw
        bnez     a2, .Lsec_252_208
        l8ui     a2, a4, 20
        bnez     a2, .Lsec_252_208
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_disable_sleep_delay_timer
        l32r     a8, .Lsec_252_28
        callx8   a8
        beqz.n   a5, .Lsec_252_1b1
        l32r     a2, .Lsec_252_c
        l8ui     a2, a2, 29
        beqz.n   a2, .Lsec_252_1a9
        addmi    a4, a4, 0x100
        s8i      a7, a4, 232
        memw
.Lsec_252_1a9:
        movi.n   a10, 1
        memw
        j        .Lsec_252_1ff
.Lsec_252_1b1:
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_sleep
        l32r     a8, .Lsec_252_2c
        callx8   a8
        j        .Lsec_252_208
.Lsec_252_1ba:
        movi.n   a6, 1
        movi.n   a8, 0
        moveqz   a6, a8, a7
        or       a3, a3, a6
        bne      a3, a8, .Lsec_252_208
        bne      a2, a8, .Lsec_252_208
        s8i      a2, a4, 18
        memw
        addmi    a4, a4, 0x100
        l8ui     a3, a4, 212
        beq      a3, a8, .Lsec_252_1ea
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, .Lsec_150_8
        l32r     a8, .Lsec_252_30
        callx8   a8
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_active_timeout_process
        l32r     a8, .Lsec_252_34
        callx8   a8
.Lsec_252_1ea:
        beqz.n   a5, .Lsec_252_208
        l32r     a2, .Lsec_252_c
        l8ui     a2, a2, 29
        beqz.n   a2, .Lsec_252_1fd
        movi     a2, 1
        s8i      a2, a4, 232
        memw
.Lsec_252_1fd:
        movi.n   a10, 0
.Lsec_252_1ff:
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_send_nullfunc
        l32r     a8, .Lsec_252_38
        callx8   a8
.Lsec_252_208:
        retw.n
        .size   pm_process_tim, .-pm_process_tim
.Lsec_252_20a:

        .section .wifislprxiram.49,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_253_0, g_pm
        .literal .Lsec_253_4, g_osi_funcs_p
        .literal .Lsec_253_8, g_pm_cfg
        .literal .Lsec_253_c, wifi_gpio_debug
        .literal .Lsec_253_10, sta_reset_beacon_timeout
        .literal .Lsec_253_14, pm_process_tim
.Lsec_253_18:
        .global pm_rx_beacon_process
        .type   pm_rx_beacon_process, @function
pm_rx_beacon_process:
        entry    a1, 32
        l32r     a6, .Lsec_253_0
        movi.n   a4, 0
        addmi    a5, a6, 0x200
        extui    a3, a3, 0, 8
        s8i      a4, a5, 29
        s8i      a4, a6, 17
        memw
        l8ui     a4, a6, 14
        l32r     a5, .Lsec_253_4
        bnez.n   a4, .Lsec_253_3e
        memw
        j        .Lsec_253_ae
        .byte    0x00
.Lsec_253_3e:
        movi.n   a11, 2
        movi.n   a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_gpio_debug
        l32r     a8, .Lsec_253_c
        callx8   a8
        beqz.n   a3, .Lsec_253_59
        movi.n   a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, sta_reset_beacon_timeout
        l32r     a8, .Lsec_253_10
        callx8   a8
        mov.n    a3, a4
        j        .Lsec_253_a4
        .byte    0x00
        .byte    0x00
.Lsec_253_59:
        beqz     a2, .Lsec_253_a4
        l16ui    a11, a6, 26
        movi.n   a4, 1
        extui    a8, a11, 0, 14
        l8ui     a10, a2, 4
        beqz.n   a8, .Lsec_253_99
        movi.n   a4, -2
        and      a6, a10, a4
        srli     a8, a8, 3
        mov.n    a4, a3
        bltu     a8, a6, .Lsec_253_99
        l8ui     a9, a2, 1
        addi     a9, a9, -4
        add.n    a9, a6, a9
        extui    a9, a9, 0, 8
        bltu     a9, a8, .Lsec_253_99
        sub      a8, a8, a6
        add      a2, a2, a8
        l8ui     a4, a2, 5
        extui    a11, a11, 0, 3
        ssr      a11
        sra      a4, a4
        extui    a4, a4, 0, 1
.Lsec_253_99:
        l32r     a2, .Lsec_253_8
        l8ui     a2, a2, 28
        bnez.n   a2, .Lsec_253_a4
        extui    a3, a10, 0, 1
.Lsec_253_a4:
        mov.n    a11, a3
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_process_tim
        l32r     a8, .Lsec_253_14
        callx8   a8
.Lsec_253_ae:
        l32i.n   a2, a5, 0
        movi.n   a10, 0
        l32i     a2, a2, 0x1a0
        callx8   a2
        retw.n
        .size   pm_rx_beacon_process, .-pm_rx_beacon_process
.Lsec_253_ba:

        .section .text.pm_max_idle_timeout_process,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_254_0, g_pm
        .literal .Lsec_254_4, pm_enable_max_idle_timer
        .literal .Lsec_254_8, pm_go_to_wake
        .literal .Lsec_254_c, pm_dream
        .literal .Lsec_254_10, pm_send_nullfunc
.Lsec_254_14:
        .global pm_max_idle_timeout_process
        .type   pm_max_idle_timeout_process, @function
pm_max_idle_timeout_process:
        entry    a1, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_enable_max_idle_timer
        l32r     a8, .Lsec_254_4
        callx8   a8
        l32r     a8, .Lsec_254_0
        l8ui     a9, a8, 13
        beqz.n   a9, .Lsec_254_62
        l8ui     a9, a8, 14
        beqz.n   a9, .Lsec_254_62
        l8ui     a9, a8, 15
        bnez.n   a9, .Lsec_254_62
        movi.n   a10, 1
        addmi    a9, a8, 0x100
        s8i      a10, a9, 232
        memw
        l32i.n   a10, a8, 36
        bnez.n   a10, .Lsec_254_4c
        mov.n    a11, a10
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_go_to_wake
        l32r     a8, .Lsec_254_8
        callx8   a8
        j        .Lsec_254_62
.Lsec_254_4c:
        l8ui     a10, a8, 1
        bnez.n   a10, .Lsec_254_54
        j        .Lsec_254_5c
.Lsec_254_54:
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_dream
        l32r     a8, .Lsec_254_c
        callx8   a8
        movi.n   a10, 1
.Lsec_254_5c:
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_send_nullfunc
        l32r     a8, .Lsec_254_10
        callx8   a8
.Lsec_254_62:
        retw.n
        .size   pm_max_idle_timeout_process, .-pm_max_idle_timeout_process
.Lsec_254_64:

        .section .wifiextrairam.50,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_255_0, g_mesh_is_started
        .literal .Lsec_255_4, g_mesh_is_root
        .literal .Lsec_255_8, g_mesh_init_ps_type
        .literal .Lsec_255_c, esp_mesh_quick_funcs
        .literal .Lsec_255_10, g_pm
        .literal .Lsec_255_14, g_osi_funcs_p
        .literal .Lsec_255_18, g_pm+0x200
        .literal .Lsec_255_1c, g_pm_cnt
        .literal .Lsec_255_20, pm_check_state
        .literal .Lsec_255_24, pm_check_state
        .literal .Lsec_255_28, pm_is_in_wifi_slice_threshold
        .literal .Lsec_255_2c, pm_disconnected_wake
        .literal .Lsec_255_30, pm_coex_schm_process_restart
        .literal .Lsec_255_34, pm_is_in_wifi_slice_threshold
        .literal .Lsec_255_38, pm_go_to_wake
        .literal .Lsec_255_3c, pm_is_in_wifi_slice_threshold
        .literal .Lsec_255_40, pm_dream
        .literal .Lsec_255_44, pm_go_to_wake
        .literal .Lsec_255_48, pm_enable_active_timer
        .literal .Lsec_255_4c, .Lsec_149_4
.Lsec_255_50:
        .global pm_tx_data_process
        .type   pm_tx_data_process, @function
pm_tx_data_process:
        entry    a1, 48
        extui    a6, a2, 0, 8
        movi.n   a2, 0
        s8i      a2, a1, 0
        memw
        extui    a3, a3, 0, 8
        l32r     a2, .Lsec_255_0
        l8ui     a5, a2, 0
        beqz.n   a5, .Lsec_255_91
        l32r     a2, .Lsec_255_4
        memw
        l8ui     a4, a2, 0
        extui    a4, a4, 0, 8
        bnez.n   a4, .Lsec_255_91
        l32r     a2, .Lsec_255_8
        l32i.n   a2, a2, 0
        beqz.n   a2, .Lsec_255_91
        mov.n    a10, a1
        l32r     a2, .Lsec_255_c
        l32i     a2, a2, 128
        memw
        callx8   a2
        j        .Lsec_255_24a
        .byte    0x00
        .byte    0x00
.Lsec_255_91:
        l32r     a4, .Lsec_255_10
        movi.n   a2, 0
        l8ui     a7, a4, 2
        beq      a7, a6, .Lsec_255_9f
        j        .Lsec_255_24d
.Lsec_255_9f:
        beq      a5, a2, .Lsec_255_ae
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_check_state
        l32r     a8, .Lsec_255_20
        callx8   a8
        bne      a10, a2, .Lsec_255_b4
        j        .Lsec_255_24a
.Lsec_255_ae:
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_check_state
        l32r     a8, .Lsec_255_24
        callx8   a8
.Lsec_255_b4:
        l8ui     a2, a4, 13
        bnez     a2, .Lsec_255_145
        l32r     a6, .Lsec_255_14
        l32i.n   a2, a6, 0
        l32i     a10, a2, 0x110
        callx8   a10
        movi     a12, 0x271
        slli     a12, a12, 3
        mov.n    a2, a10
        mov.n    a5, a11
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_is_in_wifi_slice_threshold
        l32r     a8, .Lsec_255_28
        callx8   a8
        l32i     a12, a4, 96
        l32i     a8, a4, 100
        beqz.n   a10, .Lsec_255_101
        bltu     a5, a8, .Lsec_255_e6
        bne      a8, a5, .Lsec_255_f5
        bgeu     a2, a12, .Lsec_255_f5
.Lsec_255_e6:
        l32i.n   a3, a6, 0
        sub      a12, a12, a2
        movi.n   a11, 0
        movi.n   a10, 1
        l32i     a3, a3, 0x19c
        callx8   a3
.Lsec_255_f5:
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_disconnected_wake
        l32r     a8, .Lsec_255_2c
        callx8   a8
        j        .Lsec_255_24a
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_255_101:
        l32i     a3, a4, 0x1b0
        add.n    a3, a12, a3
        l32i     a7, a4, 0x1b4
        movi.n   a4, 1
        bltu     a3, a12, .Lsec_255_110
        mov.n    a4, a10
.Lsec_255_110:
        add.n    a8, a8, a7
        add.n    a4, a4, a8
        bltu     a5, a4, .Lsec_255_11d
        bne      a4, a5, .Lsec_255_13c
        bgeu     a2, a3, .Lsec_255_13c
.Lsec_255_11d:
        l32i.n   a5, a6, 0
        l32r     a4, .Lsec_255_18
        mov.n    a10, a4
        l32i     a5, a5, 236
        callx8   a5
        movi.n   a12, 0
        sub      a11, a3, a2
        l32i.n   a5, a6, 0
        mov.n    a10, a4
        l32i     a5, a5, 248
        callx8   a5
        j        .Lsec_255_24a
.Lsec_255_13c:
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_coex_schm_process_restart
        l32r     a8, .Lsec_255_30
        callx8   a8
        j        .Lsec_255_24a
.Lsec_255_145:
        l8ui     a2, a4, 1
        bnei     a2, 1, .Lsec_255_1b4
        l32r     a7, .Lsec_255_14
        l32i.n   a5, a7, 0
        l32i     a10, a5, 0x110
        callx8   a10
        movi     a12, 0x271
        slli     a12, a12, 3
        mov.n    a5, a10
        mov.n    a6, a11
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_is_in_wifi_slice_threshold
        l32r     a8, .Lsec_255_34
        callx8   a8
        beqz.n   a10, .Lsec_255_1a1
        l32i     a2, a4, 100
        l32i     a12, a4, 96
        bltu     a6, a2, .Lsec_255_177
        bne      a2, a6, .Lsec_255_18c
        bgeu     a5, a12, .Lsec_255_18c
.Lsec_255_177:
        l8ui     a2, a4, 14
        beqz.n   a2, .Lsec_255_18c
        sub      a12, a12, a5
        movi.n   a11, 0
        l32i.n   a2, a7, 0
        movi     a10, 1
        l32i     a2, a2, 0x19c
        callx8   a2
.Lsec_255_18c:
        beqz     a3, .Lsec_255_24a
        movi.n   a11, 1
        movi.n   a10, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_go_to_wake
        l32r     a8, .Lsec_255_38
        callx8   a8
        movi.n   a2, 1
        bltui    a10, 2, .Lsec_255_1a7
        j        .Lsec_255_244
.Lsec_255_1a1:
        beqz     a3, .Lsec_255_24a
        s8i      a2, a4, 25
.Lsec_255_1a7:
        s8i      a2, a1, 0
        memw
        memw
        j        .Lsec_255_244
        .byte    0x00
.Lsec_255_1b4:
        bnei     a2, 2, .Lsec_255_22e
        l32r     a6, .Lsec_255_14
        l32i.n   a2, a6, 0
        l32i     a10, a2, 0x110
        callx8   a10
        movi     a12, 0x271
        slli     a12, a12, 3
        mov.n    a2, a10
        mov.n    a5, a11
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_is_in_wifi_slice_threshold
        l32r     a8, .Lsec_255_3c
        callx8   a8
        beqz.n   a10, .Lsec_255_212
        l32i     a8, a4, 100
        l32i     a12, a4, 96
        bltu     a5, a8, .Lsec_255_1e3
        bne      a8, a5, .Lsec_255_1f8
        bgeu     a2, a12, .Lsec_255_1f8
.Lsec_255_1e3:
        l8ui     a4, a4, 14
        beqz.n   a4, .Lsec_255_1f8
        sub      a12, a12, a2
        movi.n   a11, 0
        l32i.n   a4, a6, 0
        movi     a10, 1
        l32i     a4, a4, 0x19c
        callx8   a4
.Lsec_255_1f8:
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_dream
        l32r     a8, .Lsec_255_40
        callx8   a8
        beqz.n   a3, .Lsec_255_21f
        movi.n   a11, 1
        movi.n   a10, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_go_to_wake
        l32r     a8, .Lsec_255_44
        callx8   a8
        movi.n   a2, 1
        bltui    a10, 2, .Lsec_255_219
        j        .Lsec_255_21f
.Lsec_255_212:
        beqz.n   a3, .Lsec_255_21f
        movi.n   a2, 1
        s8i      a2, a4, 25
.Lsec_255_219:
        s8i      a2, a1, 0
        memw
.Lsec_255_21f:
        l32r     a4, .Lsec_255_1c
        l32i.n   a2, a4, 24
        addi.n   a2, a2, 1
        s32i.n   a2, a4, 24
        memw
        j        .Lsec_255_242
.Lsec_255_22e:
        bnez.n   a2, .Lsec_255_242
        beqz.n   a3, .Lsec_255_24a
        l32i.n   a2, a4, 36
        beqz.n   a2, .Lsec_255_244
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_enable_active_timer
        l32r     a8, .Lsec_255_48
        callx8   a8
        j        .Lsec_255_244
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_255_242:
        beqz.n   a3, .Lsec_255_24a
.Lsec_255_244:
        .reloc  ., R_XTENSA_ASM_EXPAND, .Lsec_149_4
        l32r     a8, .Lsec_255_4c
        callx8   a8
.Lsec_255_24a:
        l8ui     a2, a1, 0
.Lsec_255_24d:
        retw.n
        .size   pm_tx_data_process, .-pm_tx_data_process
.Lsec_255_24f:

        .section .text.pm_send_sleep_null_cb,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_256_0, pm_tx_null_data_done_process
.Lsec_256_4:
        .global pm_send_sleep_null_cb
        .type   pm_send_sleep_null_cb, @function
pm_send_sleep_null_cb:
        entry    a1, 32
        l32i.n   a8, a2, 44
        movi.n   a10, 1
        l8ui     a11, a8, 19
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_tx_null_data_done_process
        l32r     a8, .Lsec_256_0
        callx8   a8
        retw.n
        .size   pm_send_sleep_null_cb, .-pm_send_sleep_null_cb
.Lsec_256_16:

        .section .text.pm_send_wake_null_cb,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_257_0, pm_tx_null_data_done_process
.Lsec_257_4:
        .global pm_send_wake_null_cb
        .type   pm_send_wake_null_cb, @function
pm_send_wake_null_cb:
        entry    a1, 32
        l32i.n   a8, a2, 44
        movi.n   a10, 0
        l8ui     a11, a8, 19
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_tx_null_data_done_process
        l32r     a8, .Lsec_257_0
        callx8   a8
        retw.n
        .size   pm_send_wake_null_cb, .-pm_send_wake_null_cb
.Lsec_257_16:

        .section .text.pm_on_coex_schm_process_restart,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_258_0, pm_coex_schm_process_restart
.Lsec_258_4:
        .global pm_on_coex_schm_process_restart
        .type   pm_on_coex_schm_process_restart, @function
pm_on_coex_schm_process_restart:
        entry    a1, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_coex_schm_process_restart
        l32r     a8, .Lsec_258_0
        callx8   a8
        retw.n
        .size   pm_on_coex_schm_process_restart, .-pm_on_coex_schm_process_restart
.Lsec_258_f:

        .section .text.pm_on_coex_schm_status_config,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_259_0, 0x0000ffff
        .literal .Lsec_259_4, g_osi_funcs_p
        .literal .Lsec_259_8, g_pm
        .literal .Lsec_259_c, 0x51eb851f
        .literal .Lsec_259_10, pm_get_connectionless_status
.Lsec_259_14:
        .global pm_on_coex_schm_status_config
        .type   pm_on_coex_schm_status_config, @function
pm_on_coex_schm_status_config:
        entry    a1, 32
        bnez     a2, .Lsec_259_28
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_get_connectionless_status
        l32r     a8, .Lsec_259_10
        callx8   a8
        movi.n   a3, 1
        movnez   a2, a3, a10
        slli     a2, a2, 6
.Lsec_259_28:
        l32r     a3, .Lsec_259_4
        movi.n   a10, 0
        l32i.n   a8, a3, 0
        l32r     a11, .Lsec_259_0
        l32i     a8, a8, 0x1b0
        callx8   a8
        mov.n    a11, a2
        movi.n   a10, 0
        l32i.n   a8, a3, 0
        l32i     a8, a8, 0x1b4
        callx8   a8
        movi     a9, -65
        l32r     a8, .Lsec_259_8
        s32i     a2, a8, 0x190
        bany     a2, a9, .Lsec_259_60
        movi     a10, 0x3e8
        l32i.n   a2, a3, 0
        l32i     a2, a2, 0x1b8
        memw
        j        .Lsec_259_9f
        .byte    0x00
        .byte    0x00
.Lsec_259_60:
        bnei     a2, 1, .Lsec_259_70
        l32i.n   a2, a3, 0
        movi     a10, 100
        l32i     a2, a2, 0x1b8
        j        .Lsec_259_9f
        .byte    0x00
        .byte    0x00
.Lsec_259_70:
        bnei     a2, 2, .Lsec_259_8c
        addmi    a8, a8, 0x100
        l8ui     a10, a8, 201
        movi     a8, 100
        l32i.n   a2, a3, 0
        movi     a3, 240
        movnez   a3, a8, a10
        mov.n    a10, a3
        l32i     a2, a2, 0x1b8
        j        .Lsec_259_9f
.Lsec_259_8c:
        bnei     a2, 4, .Lsec_259_a2
        l32r     a2, .Lsec_259_c
        l32i.n   a10, a8, 44
        muluh    a10, a10, a2
        srli     a10, a10, 5
        l32i.n   a2, a3, 0
        l32i     a2, a2, 0x1b8
.Lsec_259_9f:
        callx8   a2
.Lsec_259_a2:
        retw.n
        .size   pm_on_coex_schm_status_config, .-pm_on_coex_schm_status_config
.Lsec_259_a4:

        .section .text.pm_on_coex_start,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_260_0, g_pm
        .literal .Lsec_260_4, .Lsec_312_0
        .literal .Lsec_260_8, pm_on_coex_schm_status_config
        .literal .Lsec_260_c, pm_coex_schm_process_restart
        .literal .Lsec_260_10, pm_update_by_connectionless_status
        .literal .Lsec_260_14, wifi_log
.Lsec_260_18:
        .global pm_on_coex_start
        .type   pm_on_coex_start, @function
pm_on_coex_start:
        entry    a1, 32
        l32r     a2, .Lsec_260_0
        l32i     a10, a2, 0x190
        bnez.n   a10, .Lsec_260_29
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_on_coex_schm_status_config
        l32r     a8, .Lsec_260_8
        callx8   a8
.Lsec_260_29:
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_coex_schm_process_restart
        l32r     a8, .Lsec_260_c
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_update_by_connectionless_status
        l32r     a8, .Lsec_260_10
        callx8   a8
        l32i.n   a3, a2, 36
        bnez.n   a3, .Lsec_260_56
        movi.n   a11, 1
        movi     a12, 3
        l32r     a13, .Lsec_260_4
        slli     a11, a11, 12
        movi     a10, 6
        addmi    a2, a2, 0x100
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_260_14
        callx8   a8
        s8i      a3, a2, 200
        memw
.Lsec_260_56:
        memw
        retw.n
        .size   pm_on_coex_start, .-pm_on_coex_start
.Lsec_260_5b:

        .section .wifirxiram.55,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_261_0, pm_sleep_for
        .literal .Lsec_261_4, pm_rx_data_process
.Lsec_261_8:
        .global pm_on_data_rx
        .type   pm_on_data_rx, @function
pm_on_data_rx:
        entry    a1, 32
        mov.n    a10, a2
        extui    a5, a5, 0, 8
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_sleep_for
        l32r     a8, .Lsec_261_0
        callx8   a8
        beqz     a10, .Lsec_261_28
        l8ui     a10, a4, 0
        extui    a11, a5, 5, 1
        extui    a10, a10, 0, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_rx_data_process
        l32r     a8, .Lsec_261_4
        callx8   a8
.Lsec_261_28:
        retw.n
        .size   pm_on_data_rx, .-pm_on_data_rx
.Lsec_261_2a:

        .section .wifiextrairam.56,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_262_0, pm_tx_data_process
.Lsec_262_4:
        .global pm_on_data_tx
        .type   pm_on_data_tx, @function
pm_on_data_tx:
        entry    a1, 32
        extui    a11, a3, 0, 8
        extui    a10, a2, 0, 8
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_tx_data_process
        l32r     a8, .Lsec_262_0
        callx8   a8
        mov.n    a2, a10
        retw.n
        .size   pm_on_data_tx, .-pm_on_data_tx
.Lsec_262_17:

        .section .text.pm_on_data_tx_done,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_263_0, pm_tx_data_done_process
.Lsec_263_4:
        .global pm_on_data_tx_done
        .type   pm_on_data_tx_done, @function
pm_on_data_tx_done:
        entry    a1, 32
        extui    a12, a4, 0, 8
        extui    a11, a3, 0, 8
        extui    a10, a2, 0, 8
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_tx_data_done_process
        l32r     a8, .Lsec_263_0
        callx8   a8
        retw.n
        .size   pm_on_data_tx_done, .-pm_on_data_tx_done
.Lsec_263_18:

        .section .text.pm_on_sample_beacon,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_264_0, g_pm
        .literal .Lsec_264_4, ptr_beacon_offset_funcs
.Lsec_264_8:
        .global pm_on_sample_beacon
        .type   pm_on_sample_beacon, @function
pm_on_sample_beacon:
        entry    a1, 32
        l32r     a8, .Lsec_264_0
        l8ui     a2, a8, 13
        beqz.n   a2, .Lsec_264_35
        addmi    a8, a8, 0x100
        l8ui     a2, a8, 100
        beqz.n   a2, .Lsec_264_35
        l8ui     a2, a8, 212
        bnez.n   a2, .Lsec_264_35
        l32r     a2, .Lsec_264_4
        l32i.n   a8, a2, 0
        l32i.n   a8, a8, 0
        beqz.n   a8, .Lsec_264_2c
        callx8   a8
.Lsec_264_2c:
        l32i.n   a2, a2, 0
        l32i.n   a10, a2, 12
        beqz.n   a10, .Lsec_264_35
        callx8   a10
.Lsec_264_35:
        retw.n
        .size   pm_on_sample_beacon, .-pm_on_sample_beacon
.Lsec_264_37:

        .section .wifislprxiram.5,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_265_0, g_pm
        .literal .Lsec_265_4, .Lsec_327_0
        .literal .Lsec_265_8, wifi_nvs_get_sta_listen_interval
        .literal .Lsec_265_c, wifi_nvs_get_sta_listen_interval
        .literal .Lsec_265_10, wifi_log
        .literal .Lsec_265_14, pm_coex_pwr_update
        .literal .Lsec_265_18, pm_on_sample_beacon
.Lsec_265_1c:
        .global pm_scale_listen_interval
        .type   pm_scale_listen_interval, @function
pm_scale_listen_interval:
        entry    a1, 48
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_nvs_get_sta_listen_interval
        l32r     a8, .Lsec_265_8
        callx8   a8
        l32r     a2, .Lsec_265_0
        addx4    a10, a10, a10
        addx4    a8, a10, a10
        slli     a8, a8, 12
        movi.n   a4, 1
        l32i.n   a3, a2, 44
        quou     a8, a8, a3
        extui    a8, a8, 0, 16
        maxu     a8, a8, a4
        l8ui     a5, a2, 11
        bltu     a8, a5, .Lsec_265_50
        j        .Lsec_265_6b
.Lsec_265_47:
        addi.n   a9, a9, 1
        extui    a9, a9, 0, 16
        j        .Lsec_265_52
        .byte    0x00
.Lsec_265_50:
        mov.n    a9, a8
.Lsec_265_52:
        rems     a4, a5, a9
        bnez     a4, .Lsec_265_47
        mov.n    a4, a8
        j        .Lsec_265_62
.Lsec_265_5d:
        addi.n   a4, a4, -1
        extui    a4, a4, 0, 16
.Lsec_265_62:
        rems     a10, a5, a4
        bnez     a10, .Lsec_265_5d
        j        .Lsec_265_79
.Lsec_265_6b:
        rems     a4, a8, a5
        sub      a4, a8, a4
        extui    a4, a4, 0, 16
        add.n    a9, a5, a4
        extui    a9, a9, 0, 16
.Lsec_265_79:
        sub      a10, a9, a8
        sub      a8, a8, a4
        blt      a8, a10, .Lsec_265_84
        mov.n    a4, a9
.Lsec_265_84:
        l16ui    a8, a2, 32
        beq      a8, a4, .Lsec_265_c3
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_nvs_get_sta_listen_interval
        l32r     a8, .Lsec_265_c
        callx8   a8
        addx4    a10, a10, a10
        l32i.n   a8, a2, 44
        addx4    a10, a10, a10
        mull     a8, a4, a8
        slli     a10, a10, 12
        movi.n   a11, 1
        l32r     a13, .Lsec_265_4
        mov.n    a15, a3
        mov.n    a14, a5
        movi.n   a12, 3
        slli     a11, a11, 12
        s32i.n   a10, a1, 4
        movi.n   a10, 6
        s32i.n   a8, a1, 8
        s32i.n   a4, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_265_10
        callx8   a8
        s16i     a4, a2, 32
        memw
.Lsec_265_c3:
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_coex_pwr_update
        l32r     a8, .Lsec_265_14
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_on_sample_beacon
        l32r     a8, .Lsec_265_18
        callx8   a8
        retw.n
        .size   pm_scale_listen_interval, .-pm_scale_listen_interval
.Lsec_265_d4:

        .section .wifislprxiram.9,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_266_0, g_pm
        .literal .Lsec_266_4, .Lsec_326_0
        .literal .Lsec_266_8, .Lsec_325_0
        .literal .Lsec_266_c, .Lsec_324_0
        .literal .Lsec_266_10, .Lsec_323_0
        .literal .Lsec_266_14, .Lsec_322_0
        .literal .Lsec_266_18, .Lsec_321_0
        .literal .Lsec_266_1c, .Lsec_320_0
        .literal .Lsec_266_20, .Lsec_319_0
        .literal .Lsec_266_24, g_osi_funcs_p
        .literal .Lsec_266_28, 0x51eb851f
        .literal .Lsec_266_2c, .Lsec_318_0
        .literal .Lsec_266_30, wifi_log
        .literal .Lsec_266_34, wifi_log
        .literal .Lsec_266_38, wifi_log
        .literal .Lsec_266_3c, wifi_log
        .literal .Lsec_266_40, wifi_log
        .literal .Lsec_266_44, wifi_log
        .literal .Lsec_266_48, wifi_log
        .literal .Lsec_266_4c, wifi_log
        .literal .Lsec_266_50, pm_scale_listen_interval
        .literal .Lsec_266_54, pm_scale_listen_interval
        .literal .Lsec_266_58, pm_scale_listen_interval
        .literal .Lsec_266_5c, wifi_log
.Lsec_266_60:
        .global pm_parse_beacon
        .type   pm_parse_beacon, @function
pm_parse_beacon:
        entry    a1, 32
        sub      a9, a3, a2
        movi.n   a10, 11
        movi.n   a8, 0
        blt      a10, a9, .Lsec_266_70
        j        .Lsec_266_238
.Lsec_266_70:
        l16ui    a8, a2, 8
        s32i.n   a2, a5, 0
        slli     a5, a8, 10
        bnez     a8, .Lsec_266_d4
        l32r     a5, .Lsec_266_0
        l8ui     a8, a5, 16
        movi.n   a5, 25
        slli     a5, a5, 12
        bnez     a8, .Lsec_266_d4
        movi.n   a5, 1
        slli     a5, a5, 12
        l32r     a13, .Lsec_266_4
        mov.n    a11, a5
        movi.n   a12, 3
        movi     a10, 6
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_266_30
        callx8   a8
        l32r     a13, .Lsec_266_8
        mov.n    a11, a5
        movi.n   a12, 3
        movi     a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_266_34
        callx8   a8
        l32r     a13, .Lsec_266_c
        mov.n    a11, a5
        movi.n   a12, 3
        movi.n   a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_266_38
        callx8   a8
        l32r     a13, .Lsec_266_10
        mov.n    a11, a5
        movi.n   a12, 3
        movi.n   a10, 6
        movi.n   a5, 25
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_266_3c
        callx8   a8
        slli     a5, a5, 12
.Lsec_266_d4:
        addi.n   a2, a2, 12
        j        .Lsec_266_f6
.Lsec_266_d9:
        l8ui     a8, a2, 1
        addi.n   a10, a8, 1
        bge      a10, a9, .Lsec_266_fc
        l8ui     a9, a2, 0
        bnei     a9, 5, .Lsec_266_f2
        s32i.n   a2, a4, 0
        memw
        j        .Lsec_266_15c
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_266_f2:
        addi.n   a8, a8, 2
        add.n    a2, a2, a8
.Lsec_266_f6:
        sub      a9, a3, a2
        bgei     a9, 2, .Lsec_266_d9
.Lsec_266_fc:
        l32r     a2, .Lsec_266_0
        l32i.n   a3, a4, 0
        l8ui     a2, a2, 16
        bnez     a3, .Lsec_266_15c
        beqz.n   a2, .Lsec_266_118
.Lsec_266_109:
        l32r     a2, .Lsec_266_0
        l32i.n   a3, a2, 44
        bne      a3, a5, .Lsec_266_114
        j        .Lsec_266_198
.Lsec_266_114:
        j        .Lsec_266_16a
        .byte    0x00
.Lsec_266_118:
        movi.n   a2, 1
        slli     a2, a2, 12
        l32r     a13, .Lsec_266_14
        movi.n   a12, 3
        mov.n    a11, a2
        movi     a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_266_40
        callx8   a8
        l32r     a13, .Lsec_266_18
        movi.n   a12, 3
        mov.n    a11, a2
        movi     a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_266_44
        callx8   a8
        l32r     a13, .Lsec_266_1c
        movi.n   a12, 3
        mov.n    a11, a2
        movi     a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_266_48
        callx8   a8
        l32r     a13, .Lsec_266_20
        movi.n   a12, 3
        mov.n    a11, a2
        movi.n   a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_266_4c
        callx8   a8
.Lsec_266_15c:
        l32r     a2, .Lsec_266_0
        l8ui     a3, a2, 16
        beqz     a3, .Lsec_266_1ca
        j        .Lsec_266_109
        .byte    0x00
        .byte    0x00
.Lsec_266_16a:
        s32i.n   a5, a2, 44
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_scale_listen_interval
        l32r     a8, .Lsec_266_50
        callx8   a8
        l32r     a3, .Lsec_266_24
        l32i.n   a8, a3, 0
        l32i     a10, a8, 0x194
        callx8   a10
        beqz.n   a10, .Lsec_266_198
        l8ui     a8, a2, 14
        beqz.n   a8, .Lsec_266_198
        l32r     a10, .Lsec_266_28
        muluh    a10, a5, a10
        srli     a10, a10, 5
        l32i.n   a3, a3, 0
        l32i     a3, a3, 0x1b8
        callx8   a3
.Lsec_266_198:
        l32i.n   a4, a4, 0
        bnez.n   a4, .Lsec_266_1a0
        j        .Lsec_266_236
        .byte    0x00
.Lsec_266_1a0:
        l8ui     a3, a4, 3
        l8ui     a5, a2, 11
        bne      a3, a5, .Lsec_266_1b5
        l8ui     a8, a2, 17
        beqz     a8, .Lsec_266_236
        l8ui     a4, a4, 2
        beqz     a4, .Lsec_266_236
.Lsec_266_1b5:
        s8i      a3, a2, 11
        memw
        beq      a3, a5, .Lsec_266_236
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_scale_listen_interval
        l32r     a8, .Lsec_266_54
        callx8   a8
        j        .Lsec_266_236
.Lsec_266_1ca:
        l32i.n   a3, a4, 0
        s32i.n   a5, a2, 44
        beqz.n   a3, .Lsec_266_1d9
        l8ui     a3, a3, 3
        s8i      a3, a2, 11
        memw
.Lsec_266_1d9:
        l8ui     a3, a2, 11
        bnez     a3, .Lsec_266_1e8
        movi     a3, 1
        s8i      a3, a2, 11
        memw
.Lsec_266_1e8:
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_scale_listen_interval
        l32r     a8, .Lsec_266_58
        callx8   a8
        l32r     a3, .Lsec_266_24
        l32i.n   a4, a3, 0
        l32i     a10, a4, 0x194
        callx8   a10
        beqz.n   a10, .Lsec_266_214
        l8ui     a4, a2, 14
        beqz.n   a4, .Lsec_266_214
        l32r     a4, .Lsec_266_28
        muluh    a10, a5, a4
        srli     a10, a10, 5
        l32i.n   a3, a3, 0
        l32i     a3, a3, 0x1b8
        callx8   a3
.Lsec_266_214:
        movi.n   a11, 1
        movi.n   a3, 1
        l8ui     a15, a2, 11
        movi.n   a12, 3
        slli     a11, a11, 12
        movi.n   a10, 6
        l32i.n   a14, a2, 44
        l32r     a13, .Lsec_266_2c
        s8i      a3, a2, 16
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_266_5c
        callx8   a8
.Lsec_266_236:
        movi.n   a8, 1
.Lsec_266_238:
        mov.n    a2, a8
        retw.n
        .size   pm_parse_beacon, .-pm_parse_beacon
.Lsec_266_23c:

        .section .wifislprxiram.54,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_267_0, g_pm
        .literal .Lsec_267_4, g_osi_funcs_p
        .literal .Lsec_267_8, g_mesh_is_root
        .literal .Lsec_267_c, g_mesh_init_ps_type
        .literal .Lsec_267_10, esp_mesh_quick_funcs
        .literal .Lsec_267_14, g_pm_cnt
        .literal .Lsec_267_18, ptr_beacon_offset_funcs
        .literal .Lsec_267_1c, pm_disable_beacon_monitor_timer
        .literal .Lsec_267_20, pm_coex_schm_overall_period_get
        .literal .Lsec_267_24, pm_beacon_monitor_tbtt_stop
        .literal .Lsec_267_28, pm_parse_beacon
        .literal .Lsec_267_2c, pm_update_next_tbtt
        .literal .Lsec_267_30, pm_rx_beacon_process
        .literal .Lsec_267_34, pm_beacon_timestamp_statistic
.Lsec_267_38:
        .global pm_on_beacon_rx
        .type   pm_on_beacon_rx, @function
pm_on_beacon_rx:
        entry    a1, 48
        movi.n   a6, 0
        s32i     a6, a1, 4
        extui    a5, a5, 0, 8
        s32i     a6, a1, 0
        l32r     a6, .Lsec_267_0
        or       a10, a6, a6
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_disable_beacon_monitor_timer
        l32r     a8, .Lsec_267_1c
        callx8   a8
        l32r     a8, .Lsec_267_4
        l32i     a8, a8, 0
        l32i     a10, a8, 0x194
        callx8   a10
        beqz.n   a10, .Lsec_267_74
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_coex_schm_overall_period_get
        l32r     a8, .Lsec_267_20
        callx8   a8
        beqi     a10, 1, .Lsec_267_74
        mov.n    a10, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_beacon_monitor_tbtt_stop
        l32r     a8, .Lsec_267_24
        callx8   a8
.Lsec_267_74:
        l32r     a8, .Lsec_267_8
        memw
        l8ui     a8, a8, 0
        extui    a8, a8, 0, 8
        beqz.n   a8, .Lsec_267_94
        l32r     a8, .Lsec_267_c
        l32i.n   a8, a8, 0
        beqz.n   a8, .Lsec_267_94
        l32r     a8, .Lsec_267_10
        l32i     a10, a8, 148
        beqz.n   a10, .Lsec_267_94
        callx8   a10
.Lsec_267_94:
        l8ui     a8, a6, 13
        beqz     a8, .Lsec_267_f4
        bnez.n   a5, .Lsec_267_b8
        addi.n   a13, a1, 4
        mov.n    a12, a1
        mov.n    a11, a4
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_parse_beacon
        l32r     a8, .Lsec_267_28
        callx8   a8
        beqz.n   a10, .Lsec_267_b8
        l32i.n   a12, a1, 0
        mov.n    a10, a2
        l32i.n   a11, a1, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_update_next_tbtt
        l32r     a8, .Lsec_267_2c
        callx8   a8
.Lsec_267_b8:
        mov.n    a11, a5
        l16ui    a8, a6, 30
        addi.n   a8, a8, 1
        l32r     a3, .Lsec_267_14
        s16i     a8, a6, 30
        memw
        l32i.n   a6, a3, 4
        addi.n   a6, a6, 1
        l32i.n   a10, a1, 0
        s32i     a6, a3, 4
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_rx_beacon_process
        l32r     a8, .Lsec_267_30
        callx8   a8
        l32i.n   a4, a1, 4
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_beacon_timestamp_statistic
        l32r     a8, .Lsec_267_34
        callx8   a8
        l32r     a3, .Lsec_267_18
        l32i.n   a3, a3, 0
        l32i.n   a3, a3, 44
        beqz.n   a3, .Lsec_267_f4
        mov.n    a11, a2
        mov.n    a10, a4
        callx8   a3
.Lsec_267_f4:
        retw.n
        .size   pm_on_beacon_rx, .-pm_on_beacon_rx
.Lsec_267_f6:

        .section .text.pm_on_probe_resp_rx,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_268_0, g_pm
        .literal .Lsec_268_4, g_osi_funcs_p
        .literal .Lsec_268_8, 0x51eb851f
        .literal .Lsec_268_c, pm_scale_listen_interval
        .literal .Lsec_268_10, pm_update_next_tbtt
.Lsec_268_14:
        .global pm_on_probe_resp_rx
        .type   pm_on_probe_resp_rx, @function
pm_on_probe_resp_rx:
        entry    a1, 48
        l32r     a6, .Lsec_268_0
        extui    a2, a2, 0, 16
        l8ui     a8, a6, 13
        beqz     a8, .Lsec_268_70
        slli     a8, a2, 10
        bnez.n   a2, .Lsec_268_2d
        movi.n   a8, 25
        slli     a8, a8, 12
.Lsec_268_2d:
        l32i.n   a2, a6, 44
        beq      a2, a8, .Lsec_268_3d
        s32i.n   a8, a6, 44
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_scale_listen_interval
        l32r     a8, .Lsec_268_c
        callx8   a8
.Lsec_268_3d:
        l32r     a2, .Lsec_268_4
        l32i.n   a8, a2, 0
        l32i     a10, a8, 0x194
        callx8   a10
        beqz.n   a10, .Lsec_268_5d
        l32i.n   a10, a6, 44
        l32r     a6, .Lsec_268_8
        muluh    a10, a10, a6
        srli     a10, a10, 5
        l32i.n   a2, a2, 0
        l32i     a2, a2, 0x1b8
        callx8   a2
.Lsec_268_5d:
        movi.n   a12, 0
        mov.n    a11, a1
        mov.n    a10, a3
        s32i.n   a4, a1, 0
        s32i.n   a5, a1, 4
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_update_next_tbtt
        l32r     a8, .Lsec_268_10
        callx8   a8
.Lsec_268_70:
        retw.n
        .size   pm_on_probe_resp_rx, .-pm_on_probe_resp_rx
.Lsec_268_72:

        .section .text.pm_disconnected_start,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_269_0, g_pm
        .literal .Lsec_269_4, g_osi_funcs_p
        .literal .Lsec_269_8, pm_on_coex_schm_status_config
        .literal .Lsec_269_c, pm_coex_schm_process_restart
        .literal .Lsec_269_10, pm_disable_disconnected_sleep_delay_timer
        .literal .Lsec_269_14, wifi_gpio_debug
        .literal .Lsec_269_18, wifi_gpio_debug
        .literal .Lsec_269_1c, wifi_gpio_debug
        .literal .Lsec_269_20, pm_update_by_connectionless_status
.Lsec_269_24:
        .global pm_disconnected_start
        .type   pm_disconnected_start, @function
pm_disconnected_start:
        entry    a1, 32
        l32r     a2, .Lsec_269_0
        movi     a10, 0
        s32i     a10, a2, 0x120
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_on_coex_schm_status_config
        l32r     a8, .Lsec_269_8
        callx8   a8
        l32r     a3, .Lsec_269_4
        l32i.n   a3, a3, 0
        l32i     a10, a3, 0x194
        callx8   a10
        beqz.n   a10, .Lsec_269_4c
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_coex_schm_process_restart
        l32r     a8, .Lsec_269_c
        callx8   a8
.Lsec_269_4c:
        l32i     a3, a2, 0x120
        addi.n   a3, a3, 1
        s32i     a3, a2, 0x120
        addmi    a3, a2, 0x100
        l8ui     a8, a3, 29
        beqz     a8, .Lsec_269_a2
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_disable_disconnected_sleep_delay_timer
        l32r     a8, .Lsec_269_10
        callx8   a8
        movi.n   a8, 1
        s16i     a8, a2, 14
        movi.n   a2, 0
        movi.n   a11, 1
        movi.n   a10, 3
        s8i      a2, a3, 136
        movi     a2, 2
        s8i      a2, a3, 29
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_gpio_debug
        l32r     a8, .Lsec_269_14
        callx8   a8
        movi     a11, 1
        movi     a10, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_gpio_debug
        l32r     a8, .Lsec_269_18
        callx8   a8
        movi.n   a11, 0
        movi.n   a10, 5
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_gpio_debug
        l32r     a8, .Lsec_269_1c
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_update_by_connectionless_status
        l32r     a8, .Lsec_269_20
        callx8   a8
.Lsec_269_a2:
        retw.n
        .size   pm_disconnected_start, .-pm_disconnected_start
.Lsec_269_a4:

        .section .text.pm_disconnected_stop,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_270_0, g_pm
        .literal .Lsec_270_4, g_pm+0xf4
        .literal .Lsec_270_8, g_osi_funcs_p
        .literal .Lsec_270_c, pm_disable_disconnected_sleep_delay_timer
        .literal .Lsec_270_10, wifi_gpio_debug
        .literal .Lsec_270_14, wifi_gpio_debug
        .literal .Lsec_270_18, wifi_gpio_debug
        .literal .Lsec_270_1c, pm_update_by_connectionless_status
.Lsec_270_20:
        .global pm_disconnected_stop
        .type   pm_disconnected_stop, @function
pm_disconnected_stop:
        entry    a1, 32
        l32r     a8, .Lsec_270_8
        movi     a3, 0
        l32i     a8, a8, 0
        l32r     a2, .Lsec_270_0
        addmi    a2, a2, 0x100
        l32i     a8, a8, 236
        l32r     a10, .Lsec_270_4
        s32i     a3, a2, 32
        memw
        callx8   a8
        l8ui     a8, a2, 29
        beq      a8, a3, .Lsec_270_7e
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_disable_disconnected_sleep_delay_timer
        l32r     a8, .Lsec_270_c
        callx8   a8
        movi.n   a8, 1
        or       a11, a3, a3
        movi     a10, 3
        s8i      a8, a2, 29
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_gpio_debug
        l32r     a8, .Lsec_270_10
        callx8   a8
        movi.n   a11, 1
        movi.n   a10, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_gpio_debug
        l32r     a8, .Lsec_270_14
        callx8   a8
        movi.n   a11, 1
        movi.n   a10, 5
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_gpio_debug
        l32r     a8, .Lsec_270_18
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_update_by_connectionless_status
        l32r     a8, .Lsec_270_1c
        callx8   a8
.Lsec_270_7e:
        retw.n
        .size   pm_disconnected_stop, .-pm_disconnected_stop
.Lsec_270_80:

        .section .text.pm_tbtt_quick_wake_configure,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_271_0, g_pm
        .literal .Lsec_271_4, ptr_beacon_offset_funcs
.Lsec_271_8:
        .global pm_tbtt_quick_wake_configure
        .type   pm_tbtt_quick_wake_configure, @function
pm_tbtt_quick_wake_configure:
        entry    a1, 32
        l32r     a8, .Lsec_271_0
        extui    a2, a2, 0, 8
        addmi    a8, a8, 0x100
        s8i      a2, a8, 212
        memw
        beqz.n   a2, .Lsec_271_2c
        l32r     a2, .Lsec_271_4
        l32i.n   a2, a2, 0
        l32i.n   a8, a2, 8
        beqz     a8, .Lsec_271_2c
        memw
        callx8   a8
.Lsec_271_2c:
        retw.n
        .size   pm_tbtt_quick_wake_configure, .-pm_tbtt_quick_wake_configure
.Lsec_271_2e:

        .section .text.pm_start,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_272_0, g_pm
        .literal .Lsec_272_4, ptr_beacon_offset_funcs
        .literal .Lsec_272_8, .Lsec_311_0
        .literal .Lsec_272_c, g_osi_funcs_p
        .literal .Lsec_272_10, 0x51eb851f
        .literal .Lsec_272_14, .Lsec_329_0
        .literal .Lsec_272_18, .Lsec_328_0
        .literal .Lsec_272_1c, wifi_log
        .literal .Lsec_272_20, memcpy
        .literal .Lsec_272_24, pm_scale_listen_interval
        .literal .Lsec_272_28, dbg_lmac_ps_statis_reset
        .literal .Lsec_272_2c, pm_update_next_tbtt
        .literal .Lsec_272_30, pm_beacon_monitor_init
        .literal .Lsec_272_34, pm_enable_active_timer
        .literal .Lsec_272_38, pm_on_coex_schm_status_config
        .literal .Lsec_272_3c, wifi_gpio_debug
        .literal .Lsec_272_40, wifi_gpio_debug
        .literal .Lsec_272_44, wifi_gpio_debug
        .literal .Lsec_272_48, pm_update_by_connectionless_status
.Lsec_272_4c:
        .global pm_start
        .type   pm_start, @function
pm_start:
        entry    a1, 64
        l32r     a7, .Lsec_272_0
        extui    a2, a2, 0, 8
        extui    a3, a3, 0, 16
        extui    a5, a5, 0, 16
        l8ui     a8, a7, 13
        bnez     a8, .Lsec_272_219
        l32r     a8, .Lsec_272_4
        l32i.n   a8, a8, 0
        l32i.n   a8, a8, 48
        beqz.n   a8, .Lsec_272_6d
        callx8   a8
.Lsec_272_6d:
        movi.n   a11, 1
        l32i.n   a14, a7, 36
        movi.n   a12, 3
        slli     a11, a11, 12
        movi.n   a10, 6
        l32r     a13, .Lsec_272_8
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_272_1c
        callx8   a8
        movi.n   a8, 1
        movi.n   a12, 6
        s16i     a8, a7, 14
        movi.n   a8, 0
        mov.n    a11, a4
        addi.n   a10, a7, 3
        s16i     a3, a7, 26
        addmi    a3, a7, 0x100
        s8i      a8, a3, 192
        s8i      a2, a7, 2
        memw
        movi.n   a2, 0
        s32i.n   a8, a1, 16
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_272_20
        callx8   a8
        l32i.n   a8, a1, 16
        s32i.n   a2, a7, 60
        s32i     a2, a7, 64
        s32i     a2, a7, 136
        s32i     a2, a7, 140
        l32r     a2, .Lsec_272_c
        l32i     a4, a2, 0
        l32i     a10, a4, 0x110
        s8i      a8, a7, 19
        s8i      a8, a7, 20
        s8i      a8, a3, 232
        s8i      a8, a7, 9
        s8i      a8, a7, 10
        memw
        memw
        callx8   a10
        l32i.n   a4, a2, 0
        s32i     a10, a7, 128
        l32i     a10, a4, 0x110
        s32i     a11, a7, 132
        memw
        callx8   a10
        l32i.n   a4, a2, 0
        s32i     a10, a7, 112
        l32i     a10, a4, 0x110
        s32i     a11, a7, 116
        memw
        callx8   a10
        l32i.n   a4, a2, 0
        s32i     a10, a7, 120
        l32i     a10, a4, 0x110
        slli     a4, a5, 10
        s32i     a11, a7, 124
        memw
        callx8   a10
        s32i     a10, a7, 72
        s32i     a11, a7, 76
        bnez.n   a5, .Lsec_272_11c
        movi     a4, 25
        slli     a4, a4, 12
.Lsec_272_11c:
        s32i.n   a4, a7, 44
        l32i.n   a4, a2, 0
        l32i     a10, a4, 0x194
        memw
        callx8   a10
        beqz.n   a10, .Lsec_272_13e
        l32r     a4, .Lsec_272_10
        l32i.n   a10, a7, 44
        muluh    a10, a10, a4
        srli     a10, a10, 5
        l32i.n   a4, a2, 0
        l32i     a4, a4, 0x1b8
        callx8   a4
.Lsec_272_13e:
        movi.n   a5, 1
        movi     a4, 0
        s8i      a5, a7, 11
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_scale_listen_interval
        l32r     a8, .Lsec_272_24
        callx8   a8
        movi.n   a13, 1
        l32i.n   a2, a2, 0
        s16i     a13, a7, 24
        memw
        l32i     a10, a2, 0x194
        s8i      a4, a3, 28
        memw
        s32i.n   a13, a1, 16
        memw
        callx8   a10
        movi.n   a8, 0
        mov.n    a2, a8
        s8i      a4, a3, 200
        addmi    a4, a7, 0x200
        s8i      a10, a7, 12
        s32i     a8, a7, 80
        s32i     a8, a7, 84
        s32i     a8, a7, 0x1d0
        s8i      a8, a4, 28
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, dbg_lmac_ps_statis_reset
        l32r     a8, .Lsec_272_28
        callx8   a8
        mov.n    a12, a2
        mov.n    a11, a1
        mov.n    a10, a6
        l32i     a8, a1, 64
        l32i     a9, a1, 68
        s32i     a8, a1, 0
        s32i     a9, a1, 4
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_update_next_tbtt
        l32r     a8, .Lsec_272_2c
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_beacon_monitor_init
        l32r     a8, .Lsec_272_30
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_enable_active_timer
        l32r     a8, .Lsec_272_34
        callx8   a8
        movi     a10, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_on_coex_schm_status_config
        l32r     a8, .Lsec_272_38
        callx8   a8
        l32i.n   a13, a1, 16
        l8ui     a2, a3, 29
        s8i      a13, a3, 201
        memw
        beqz.n   a2, .Lsec_272_1d5
        s8i      a5, a3, 29
        memw
.Lsec_272_1d5:
        movi.n   a11, 1
        movi.n   a10, 3
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_gpio_debug
        l32r     a8, .Lsec_272_3c
        callx8   a8
        movi.n   a11, 1
        movi.n   a10, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_gpio_debug
        l32r     a8, .Lsec_272_40
        callx8   a8
        movi.n   a11, 0
        movi.n   a10, 5
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_gpio_debug
        l32r     a8, .Lsec_272_44
        callx8   a8
        movi.n   a2, 1
        l32r     a3, .Lsec_272_14
        s8i      a2, a7, 13
        memw
        movi.n   a2, 0
        s32i.n   a2, a3, 0
        l32r     a3, .Lsec_272_18
        s8i      a2, a4, 30
        memw
        s32i.n   a2, a3, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_update_by_connectionless_status
        l32r     a8, .Lsec_272_48
        callx8   a8
.Lsec_272_219:
        retw.n
        .size   pm_start, .-pm_start
.Lsec_272_21b:

        .section .text.pm_stop,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_273_0, g_pm
        .literal .Lsec_273_4, g_pm+0xa4
        .literal .Lsec_273_8, g_osi_funcs_p
        .literal .Lsec_273_c, g_pm+0xb8
        .literal .Lsec_273_10, .Lsec_310_0
        .literal .Lsec_273_14, 0x00030000
        .literal .Lsec_273_18, ptr_beacon_offset_funcs
        .literal .Lsec_273_1c, g_mesh_is_started
        .literal .Lsec_273_20, g_mesh_is_root
        .literal .Lsec_273_24, g_mesh_init_ps_type
        .literal .Lsec_273_28, .Lsec_309_0
        .literal .Lsec_273_2c, .Lsec_147_8
        .literal .Lsec_273_30, pm_disable_sleep_delay_timer
        .literal .Lsec_273_34, .Lsec_150_8
        .literal .Lsec_273_38, pm_dream
        .literal .Lsec_273_3c, .Lsec_151_c
        .literal .Lsec_273_40, ppProcessWaitingQueue
        .literal .Lsec_273_44, wifi_log
        .literal .Lsec_273_48, pm_wake_done
        .literal .Lsec_273_4c, pm_coex_pwr_update
        .literal .Lsec_273_50, pm_on_coex_schm_status_config
        .literal .Lsec_273_54, pm_coex_schm_process_restart
        .literal .Lsec_273_58, pm_enable_disconnected_sleep_delay_timer
        .literal .Lsec_273_5c, pm_update_by_connectionless_status
        .literal .Lsec_273_60, esp_mesh_get_running_active_duty_cycle
        .literal .Lsec_273_64, __floatundisf
        .literal .Lsec_273_68, __floatundisf
        .literal .Lsec_273_6c, __divsf3
        .literal .Lsec_273_70, __extendsfdf2
        .literal .Lsec_273_74, wifi_log
        .literal .Lsec_273_78, ieee80211_hostapd_beacon_txcb
        .literal .Lsec_273_7c, pm_set_bss_max_idle
.Lsec_273_80:
        .global pm_stop
        .type   pm_stop, @function
pm_stop:
        entry    a1, 80
        l32r     a2, .Lsec_273_0
        l8ui     a3, a2, 13
        beqz     a3, .Lsec_273_292
        .reloc  ., R_XTENSA_ASM_EXPAND, .Lsec_147_8
        l32r     a8, .Lsec_273_2c
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_disable_sleep_delay_timer
        l32r     a8, .Lsec_273_30
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, .Lsec_150_8
        l32r     a8, .Lsec_273_34
        callx8   a8
        l32r     a5, .Lsec_273_8
        l32i     a3, a5, 0
        l32r     a10, .Lsec_273_4
        l32i     a3, a3, 236
        callx8   a3
        l32i.n   a3, a5, 0
        l32r     a10, .Lsec_273_c
        l32i     a3, a3, 236
        callx8   a3
        movi.n   a3, 0
        s16i     a3, a2, 26
        memw
        l8ui     a3, a2, 1
        beqz.n   a3, .Lsec_273_dc
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_dream
        l32r     a8, .Lsec_273_38
        callx8   a8
        movi.n   a10, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, .Lsec_151_c
        l32r     a8, .Lsec_273_3c
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, ppProcessWaitingQueue
        l32r     a8, .Lsec_273_40
        callx8   a8
.Lsec_273_dc:
        l32i.n   a3, a5, 0
        movi     a6, 1
        l32i     a14, a2, 136
        l32i     a15, a2, 140
        l32i     a10, a3, 0x110
        s32i     a14, a1, 32
        s32i     a15, a1, 36
        memw
        callx8   a10
        mov.n    a3, a11
        movi.n   a11, 1
        slli     a11, a11, 12
        l32i     a4, a2, 128
        sub      a4, a10, a4
        l32i     a7, a2, 132
        l32i.n   a14, a1, 32
        l32i     a15, a1, 36
        bltu     a10, a4, .Lsec_273_111
        movi     a6, 0
.Lsec_273_111:
        sub      a3, a3, a7
        sub      a3, a3, a6
        l32r     a13, .Lsec_273_10
        movi.n   a12, 3
        movi.n   a10, 6
        s32i     a3, a1, 4
        s32i     a4, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_273_44
        callx8   a8
        l8ui     a3, a2, 15
        beqz.n   a3, .Lsec_273_138
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_wake_done
        l32r     a8, .Lsec_273_48
        callx8   a8
.Lsec_273_138:
        movi.n   a4, 1
        l32r     a6, .Lsec_273_14
        movi.n   a3, 0
        s8i      a4, a2, 11
        movi.n   a4, 1
        s16i     a4, a2, 24
        addmi    a4, a2, 0x100
        s32i     a6, a2, 0x180
        s8i      a3, a2, 15
        s8i      a3, a2, 16
        s8i      a3, a2, 13
        s8i      a3, a4, 100
        s8i      a3, a4, 125
        s8i      a3, a4, 193
        s8i      a3, a4, 200
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_coex_pwr_update
        l32r     a8, .Lsec_273_4c
        callx8   a8
        movi.n   a10, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_on_coex_schm_status_config
        l32r     a8, .Lsec_273_50
        callx8   a8
        l32i.n   a3, a5, 0
        l32i     a10, a3, 0x194
        callx8   a10
        beqz.n   a10, .Lsec_273_186
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_coex_schm_process_restart
        l32r     a8, .Lsec_273_54
        callx8   a8
.Lsec_273_186:
        l32r     a3, .Lsec_273_18
        l32i.n   a3, a3, 0
        l32i     a3, a3, 52
        beqz     a3, .Lsec_273_194
        callx8   a3
.Lsec_273_194:
        l8ui     a3, a4, 29
        beqz.n   a3, .Lsec_273_1b1
        movi     a3, 2
        s8i      a3, a4, 29
        memw
        l32i     a3, a2, 0x120
        bnez     a3, .Lsec_273_1b1
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_enable_disconnected_sleep_delay_timer
        l32r     a8, .Lsec_273_58
        callx8   a8
.Lsec_273_1b1:
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_update_by_connectionless_status
        l32r     a8, .Lsec_273_5c
        callx8   a8
        l32r     a3, .Lsec_273_1c
        l8ui     a3, a3, 0
        beqz     a3, .Lsec_273_280
        l32r     a3, .Lsec_273_20
        memw
        l8ui     a6, a3, 0
        extui    a6, a6, 0, 8
        bnez     a6, .Lsec_273_280
        l32r     a3, .Lsec_273_24
        l32i.n   a3, a3, 0
        beqz     a3, .Lsec_273_280
        movi.n   a7, 1
        l32i.n   a3, a5, 0
        l32i     a10, a3, 0x110
        callx8   a10
        l32i     a5, a2, 128
        sub      a5, a10, a5
        l32i     a3, a2, 132
        bltu     a10, a5, .Lsec_273_1f0
        or       a7, a6, a6
.Lsec_273_1f0:
        sub      a3, a11, a3
        sub      a3, a3, a7
        l32i     a15, a2, 140
        slli     a7, a15, 12
        extui    a15, a15, 20, 12
        l32i     a6, a2, 136
        extui    a6, a6, 20, 12
        or       a6, a7, a6
        movi.n   a7, 1
        s32i.n   a15, a1, 36
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_mesh_get_running_active_duty_cycle
        l32r     a8, .Lsec_273_60
        callx8   a8
        or       a8, a10, a10
        slli     a7, a7, 12
        l32i     a11, a2, 140
        l32i     a10, a2, 136
        s32i.n   a8, a1, 32
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, __floatundisf
        l32r     a8, .Lsec_273_64
        callx8   a8
        mov.n    a2, a10
        or       a11, a3, a3
        or       a10, a5, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, __floatundisf
        l32r     a8, .Lsec_273_68
        callx8   a8
        mov.n    a11, a10
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, __divsf3
        l32r     a8, .Lsec_273_6c
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, __extendsfdf2
        l32r     a8, .Lsec_273_70
        callx8   a8
        l32i.n   a8, a1, 32
        slli     a2, a3, 12
        extui    a3, a3, 20, 12
        mov.n    a14, a6
        movi.n   a12, 3
        s32i.n   a10, a1, 16
        extui    a10, a5, 20, 12
        or       a10, a2, a10
        l32i.n   a15, a1, 36
        l32r     a13, .Lsec_273_28
        s32i.n   a11, a1, 20
        mov.n    a11, a7
        s32i.n   a10, a1, 0
        movi.n   a10, 6
        s32i.n   a8, a1, 8
        s32i.n   a3, a1, 4
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_273_74
        callx8   a8
        movi.n   a10, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, ieee80211_hostapd_beacon_txcb
        l32r     a8, .Lsec_273_78
        callx8   a8
.Lsec_273_280:
        movi     a10, 0
        movi     a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_set_bss_max_idle
        l32r     a8, .Lsec_273_7c
        callx8   a8
        s8i      a2, a4, 193
        memw
.Lsec_273_292:
        memw
        retw.n
        .size   pm_stop, .-pm_stop
.Lsec_273_297:

        .section .text.pm_attach,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_274_0, g_pm
        .literal .Lsec_274_4, g_pm_cfg
        .literal .Lsec_274_8, 0x00019000
        .literal .Lsec_274_c, 0x00030000
        .literal .Lsec_274_10, pm_send_sleep_null_cb
        .literal .Lsec_274_14, pm_send_wake_null_cb
        .literal .Lsec_274_18, .Lsec_163_4
        .literal .Lsec_274_1c, g_pm+0xa4
        .literal .Lsec_274_20, g_osi_funcs_p
        .literal .Lsec_274_24, .Lsec_162_4
        .literal .Lsec_274_28, g_pm+0xb8
        .literal .Lsec_274_2c, .Lsec_161_4
        .literal .Lsec_274_30, g_pm+0xcc
        .literal .Lsec_274_34, .Lsec_160_4
        .literal .Lsec_274_38, g_pm+0x108
        .literal .Lsec_274_3c, .Lsec_159_4
        .literal .Lsec_274_40, g_pm+0xf4
        .literal .Lsec_274_44, .Lsec_158_4
        .literal .Lsec_274_48, g_pm+0x124
        .literal .Lsec_274_4c, .Lsec_157_4
        .literal .Lsec_274_50, g_pm+0x138
        .literal .Lsec_274_54, .Lsec_156_4
        .literal .Lsec_274_58, g_pm+0x14c
        .literal .Lsec_274_5c, .Lsec_155_4
        .literal .Lsec_274_60, g_pm+0x168
        .literal .Lsec_274_64, .Lsec_154_4
        .literal .Lsec_274_68, g_pm+0x1ec
        .literal .Lsec_274_6c, .Lsec_153_4
        .literal .Lsec_274_70, g_pm+0x200
        .literal .Lsec_274_74, memset
        .literal .Lsec_274_78, .Lsec_151_c
        .literal .Lsec_274_7c, ppRegisterTxCallback
        .literal .Lsec_274_80, ppRegisterTxCallback
.Lsec_274_84:
        .global pm_attach
        .type   pm_attach, @function
pm_attach:
        entry    a1, 32
        l32r     a2, .Lsec_274_0
        movi     a12, 0x220
        movi     a11, 0
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, memset
        l32r     a8, .Lsec_274_74
        callx8   a8
        movi.n   a5, 1
        movi.n   a10, 0
        s32i.n   a5, a2, 36
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, .Lsec_151_c
        l32r     a8, .Lsec_274_78
        callx8   a8
        addmi    a6, a2, 0x100
        movi.n   a11, 5
        l32r     a3, .Lsec_274_4
        l32i.n   a4, a3, 4
        s32i.n   a4, a2, 40
        l32i.n   a4, a3, 12
        s32i.n   a4, a2, 52
        l32r     a4, .Lsec_274_8
        s32i.n   a4, a2, 44
        l32i.n   a4, a3, 20
        s32i.n   a4, a2, 48
        l32i.n   a4, a3, 44
        l32i.n   a3, a3, 24
        s32i     a4, a2, 0x1c4
        l32r     a4, .Lsec_274_c
        s32i.n   a3, a2, 56
        movi.n   a3, 0
        mov.n    a12, a3
        l32r     a10, .Lsec_274_10
        s32i     a4, a2, 0x180
        s16i     a5, a2, 24
        s16i     a3, a2, 0x17e
        s8i      a5, a2, 11
        s16i     a5, a2, 32
        s32i.n   a3, a2, 60
        s8i      a3, a2, 21
        s8i      a3, a6, 125
        s8i      a5, a2, 14
        s8i      a3, a6, 212
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, ppRegisterTxCallback
        l32r     a8, .Lsec_274_7c
        callx8   a8
        mov.n    a12, a3
        movi.n   a11, 6
        l32r     a10, .Lsec_274_14
        .reloc  ., R_XTENSA_ASM_EXPAND, ppRegisterTxCallback
        l32r     a8, .Lsec_274_80
        callx8   a8
        mov.n    a12, a3
        l32r     a4, .Lsec_274_20
        l32i.n   a8, a4, 0
        l32r     a11, .Lsec_274_18
        l32i     a8, a8, 244
        l32r     a10, .Lsec_274_1c
        callx8   a8
        mov.n    a12, a3
        l32i.n   a8, a4, 0
        l32r     a11, .Lsec_274_24
        l32i     a8, a8, 244
        l32r     a10, .Lsec_274_28
        callx8   a8
        mov.n    a12, a3
        l32i.n   a8, a4, 0
        l32r     a11, .Lsec_274_2c
        l32i     a8, a8, 244
        l32r     a10, .Lsec_274_30
        callx8   a8
        mov.n    a12, a3
        l32i.n   a8, a4, 0
        l32r     a11, .Lsec_274_34
        l32i     a8, a8, 244
        l32r     a10, .Lsec_274_38
        callx8   a8
        mov.n    a12, a3
        l32i.n   a8, a4, 0
        l32r     a11, .Lsec_274_3c
        l32i     a8, a8, 244
        l32r     a10, .Lsec_274_40
        callx8   a8
        mov.n    a12, a3
        l32i.n   a8, a4, 0
        l32r     a11, .Lsec_274_44
        l32i     a8, a8, 244
        l32r     a10, .Lsec_274_48
        callx8   a8
        mov.n    a12, a3
        l32i.n   a8, a4, 0
        l32r     a11, .Lsec_274_4c
        l32i     a8, a8, 244
        l32r     a10, .Lsec_274_50
        callx8   a8
        mov.n    a12, a3
        l32i.n   a8, a4, 0
        l32r     a11, .Lsec_274_54
        l32i     a8, a8, 244
        l32r     a10, .Lsec_274_58
        callx8   a8
        mov.n    a12, a2
        l32i.n   a8, a4, 0
        l32r     a11, .Lsec_274_5c
        l32i     a8, a8, 244
        l32r     a10, .Lsec_274_60
        callx8   a8
        mov.n    a12, a3
        l32i.n   a8, a4, 0
        l32r     a11, .Lsec_274_64
        l32i     a8, a8, 244
        l32r     a10, .Lsec_274_68
        callx8   a8
        mov.n    a12, a3
        l32i.n   a4, a4, 0
        l32r     a11, .Lsec_274_6c
        l32i     a4, a4, 244
        l32r     a10, .Lsec_274_70
        callx8   a4
        s16i     a3, a2, 0x18a
        s32i     a3, a2, 0x1a8
        s32i     a3, a2, 0x1ac
        s8i      a3, a6, 29
        s8i      a5, a6, 201
        s8i      a5, a2, 0
        memw
        memw
        retw.n
        .size   pm_attach, .-pm_attach
.Lsec_274_1d7:

        .section .text.pm_deattach,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_275_0, g_pm
        .literal .Lsec_275_4, g_pm+0xa4
        .literal .Lsec_275_8, g_osi_funcs_p
        .literal .Lsec_275_c, g_pm+0xb8
        .literal .Lsec_275_10, g_pm+0xcc
        .literal .Lsec_275_14, g_pm+0x108
        .literal .Lsec_275_18, g_pm+0xf4
        .literal .Lsec_275_1c, g_pm+0x124
        .literal .Lsec_275_20, g_pm+0x138
        .literal .Lsec_275_24, g_pm+0x14c
        .literal .Lsec_275_28, g_pm+0x168
        .literal .Lsec_275_2c, g_pm+0x1ec
        .literal .Lsec_275_30, g_pm+0x200
.Lsec_275_34:
        .global pm_deattach
        .type   pm_deattach, @function
pm_deattach:
        entry    a1, 32
        l32r     a3, .Lsec_275_0
        l8ui     a2, a3, 0
        beqi     a2, 1, .Lsec_275_43
        j        .Lsec_275_d0
.Lsec_275_43:
        l32r     a2, .Lsec_275_8
        l32i     a8, a2, 0
        l32r     a10, .Lsec_275_4
        l32i     a8, a8, 240
        callx8   a8
        l32i     a8, a2, 0
        l32r     a10, .Lsec_275_c
        l32i     a8, a8, 240
        callx8   a8
        l32i     a8, a2, 0
        l32r     a10, .Lsec_275_10
        l32i     a8, a8, 240
        callx8   a8
        l32i     a8, a2, 0
        l32r     a10, .Lsec_275_14
        l32i     a8, a8, 240
        callx8   a8
        l32i     a8, a2, 0
        l32r     a10, .Lsec_275_18
        l32i     a8, a8, 240
        callx8   a8
        l32i     a8, a2, 0
        l32r     a10, .Lsec_275_1c
        l32i     a8, a8, 240
        callx8   a8
        l32i     a8, a2, 0
        l32r     a10, .Lsec_275_20
        l32i     a8, a8, 240
        callx8   a8
        l32i     a8, a2, 0
        l32r     a10, .Lsec_275_24
        l32i     a8, a8, 240
        callx8   a8
        l32i     a8, a2, 0
        l32r     a10, .Lsec_275_28
        l32i     a8, a8, 240
        callx8   a8
        l32i.n   a8, a2, 0
        l32r     a10, .Lsec_275_2c
        l32i     a8, a8, 240
        callx8   a8
        l32i.n   a2, a2, 0
        l32r     a10, .Lsec_275_30
        l32i     a2, a2, 240
        callx8   a2
        movi.n   a2, 0
        s8i      a2, a3, 0
        memw
.Lsec_275_d0:
        memw
        retw.n
        .size   pm_deattach, .-pm_deattach
.Lsec_275_d5:

        .section .text.pm_set_sleep_type,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_276_0, g_osi_funcs_p
        .literal .Lsec_276_4, .Lsec_308_0
        .literal .Lsec_276_8, .Lsec_307_0
        .literal .Lsec_276_c, g_pm
        .literal .Lsec_276_10, wifi_log
        .literal .Lsec_276_14, wifi_log
        .literal .Lsec_276_18, pm_enable_active_timer
        .literal .Lsec_276_1c, .Lsec_148_8
        .literal .Lsec_276_20, pm_dream
        .literal .Lsec_276_24, .Lsec_151_c
        .literal .Lsec_276_28, pm_go_to_wake
        .literal .Lsec_276_2c, pm_enable_active_timer
        .literal .Lsec_276_30, pm_enable_max_idle_timer
        .literal .Lsec_276_34, pm_coex_pwr_update
.Lsec_276_38:
        .global pm_set_sleep_type
        .type   pm_set_sleep_type, @function
pm_set_sleep_type:
        entry    a1, 32
        l32r     a5, .Lsec_276_0
        l32i.n   a3, a5, 0
        l32i     a10, a3, 0x194
        movi.n   a3, 1
        callx8   a10
        slli     a3, a3, 12
        mov.n    a15, a10
        l32r     a13, .Lsec_276_4
        mov.n    a14, a2
        movi.n   a12, 3
        mov.n    a11, a3
        movi.n   a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_276_10
        callx8   a8
        bnez.n   a2, .Lsec_276_79
        l32i.n   a4, a5, 0
        l32i     a10, a4, 0x194
        callx8   a10
        beqz.n   a10, .Lsec_276_79
        movi.n   a12, 3
        l32r     a13, .Lsec_276_8
        mov.n    a11, a3
        movi.n   a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_276_14
        callx8   a8
.Lsec_276_79:
        l32r     a3, .Lsec_276_c
        l8ui     a8, a3, 13
        l32i.n   a4, a3, 36
        s32i.n   a2, a3, 36
        beqz     a8, .Lsec_276_dc
        l32i.n   a5, a5, 0
        l32i     a10, a5, 0x194
        memw
        callx8   a10
        beqi     a10, 1, .Lsec_276_dc
        bnez.n   a4, .Lsec_276_aa
        addi.n   a2, a2, -1
        bgeui    a2, 2, .Lsec_276_dc
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_enable_active_timer
        l32r     a8, .Lsec_276_18
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, .Lsec_148_8
        l32r     a8, .Lsec_276_1c
        callx8   a8
        j        .Lsec_276_dc
.Lsec_276_aa:
        bnez.n   a2, .Lsec_276_dc
        addi     a4, a4, -1
        bgeui    a4, 2, .Lsec_276_dc
        l8ui     a3, a3, 1
        beqz     a3, .Lsec_276_d0
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_dream
        l32r     a8, .Lsec_276_20
        callx8   a8
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, .Lsec_151_c
        l32r     a8, .Lsec_276_24
        callx8   a8
        mov.n    a11, a2
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_go_to_wake
        l32r     a8, .Lsec_276_28
        callx8   a8
.Lsec_276_d0:
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_enable_active_timer
        l32r     a8, .Lsec_276_2c
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_enable_max_idle_timer
        l32r     a8, .Lsec_276_30
        callx8   a8
.Lsec_276_dc:
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_coex_pwr_update
        l32r     a8, .Lsec_276_34
        callx8   a8
        retw.n
        .size   pm_set_sleep_type, .-pm_set_sleep_type
.Lsec_276_e4:

        .section .text.pm_get_sleep_type,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_277_0, g_pm
.Lsec_277_4:
        .global pm_get_sleep_type
        .type   pm_get_sleep_type, @function
pm_get_sleep_type:
        entry    a1, 32
        l32r     a8, .Lsec_277_0
        l32i.n   a2, a8, 36
        retw.n
        .size   pm_get_sleep_type, .-pm_get_sleep_type
.Lsec_277_e:

        .section .text.pm_beacon_monitor_configure,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_278_0, g_pm_cfg
.Lsec_278_4:
        .global pm_beacon_monitor_configure
        .type   pm_beacon_monitor_configure, @function
pm_beacon_monitor_configure:
        entry    a1, 32
        l8ui     a9, a2, 0
        l32r     a8, .Lsec_278_0
        s8i      a9, a8, 36
        memw
        beqz.n   a9, .Lsec_278_41
        l8ui     a9, a2, 1
        s8i      a9, a8, 37
        memw
        l8ui     a9, a2, 2
        s8i      a9, a8, 38
        memw
        l8ui     a9, a2, 3
        s8i      a9, a8, 39
        memw
        l8ui     a9, a2, 4
        s8i      a9, a8, 40
        memw
        l32i.n   a2, a2, 8
        s8i      a2, a8, 51
        memw
.Lsec_278_41:
        memw
        retw.n
        .size   pm_beacon_monitor_configure, .-pm_beacon_monitor_configure
.Lsec_278_46:

        .section .text.pm_update_modem_sleep_default_params,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_279_0, g_pm_cfg
.Lsec_279_4:
        .global pm_update_modem_sleep_default_params
        .type   pm_update_modem_sleep_default_params, @function
pm_update_modem_sleep_default_params:
        entry    a1, 32
        l32r     a8, .Lsec_279_0
        movi     a9, 0x1f4
        s32i.n   a9, a8, 4
        memw
        retw.n
        .size   pm_update_modem_sleep_default_params, .-pm_update_modem_sleep_default_params
.Lsec_279_14:

        .section .text.pm_update_light_sleep_default_params,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_280_0, g_pm_cfg
.Lsec_280_4:
        .global pm_update_light_sleep_default_params
        .type   pm_update_light_sleep_default_params, @function
pm_update_light_sleep_default_params:
        entry    a1, 32
        l32r     a8, .Lsec_280_0
        movi     a9, 0x1f4
        s32i.n   a9, a8, 4
        memw
        retw.n
        .size   pm_update_light_sleep_default_params, .-pm_update_light_sleep_default_params
.Lsec_280_14:

        .section .text.pm_set_sleep_min_active_time,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_281_0, g_pm_cfg
.Lsec_281_4:
        .global pm_set_sleep_min_active_time
        .type   pm_set_sleep_min_active_time, @function
pm_set_sleep_min_active_time:
        entry    a1, 32
        l32r     a8, .Lsec_281_0
        s32i.n   a2, a8, 20
        memw
        retw.n
        .size   pm_set_sleep_min_active_time, .-pm_set_sleep_min_active_time
.Lsec_281_11:

        .section .text.pm_set_keep_alive_time,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_282_0, g_pm_cfg
.Lsec_282_4:
        .global pm_set_keep_alive_time
        .type   pm_set_keep_alive_time, @function
pm_set_keep_alive_time:
        entry    a1, 32
        l32r     a8, .Lsec_282_0
        s32i.n   a2, a8, 16
        memw
        retw.n
        .size   pm_set_keep_alive_time, .-pm_set_keep_alive_time
.Lsec_282_11:

        .section .text.pm_get_keep_alive_time,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_283_0, g_pm_cfg
.Lsec_283_4:
        .global pm_get_keep_alive_time
        .type   pm_get_keep_alive_time, @function
pm_get_keep_alive_time:
        entry    a1, 32
        l32r     a8, .Lsec_283_0
        l32i.n   a2, a8, 16
        retw.n
        .size   pm_get_keep_alive_time, .-pm_get_keep_alive_time
.Lsec_283_e:

        .section .text.pm_set_sleep_wait_broadcast_data_time,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_284_0, g_pm_cfg
.Lsec_284_4:
        .global pm_set_sleep_wait_broadcast_data_time
        .type   pm_set_sleep_wait_broadcast_data_time, @function
pm_set_sleep_wait_broadcast_data_time:
        entry    a1, 32
        l32r     a8, .Lsec_284_0
        s32i.n   a2, a8, 44
        memw
        retw.n
        .size   pm_set_sleep_wait_broadcast_data_time, .-pm_set_sleep_wait_broadcast_data_time
.Lsec_284_11:

        .section .text.pm_scan_lock,"ax",@progbits
        .align  4
.Lsec_285_0:
        .global pm_scan_lock
        .type   pm_scan_lock, @function
pm_scan_lock:
        entry    a1, 32
        retw.n
        .size   pm_scan_lock, .-pm_scan_lock
.Lsec_285_5:

        .section .text.pm_scan_unlock,"ax",@progbits
        .align  4
.Lsec_286_0:
        .global pm_scan_unlock
        .type   pm_scan_unlock, @function
pm_scan_unlock:
        entry    a1, 32
        retw.n
        .size   pm_scan_unlock, .-pm_scan_unlock
.Lsec_286_5:

        .section .text.pm_try_scan_unlock,"ax",@progbits
        .align  4
.Lsec_287_0:
        .global pm_try_scan_unlock
        .type   pm_try_scan_unlock, @function
pm_try_scan_unlock:
        entry    a1, 32
        retw.n
        .size   pm_try_scan_unlock, .-pm_try_scan_unlock
.Lsec_287_5:

        .section .text.pm_force_scan_unlock,"ax",@progbits
        .align  4
.Lsec_288_0:
        .global pm_force_scan_unlock
        .type   pm_force_scan_unlock, @function
pm_force_scan_unlock:
        entry    a1, 32
        retw.n
        .size   pm_force_scan_unlock, .-pm_force_scan_unlock
.Lsec_288_5:

        .section .text.pm_get_idle_wait_time,"ax",@progbits
        .align  4
.Lsec_289_0:
        .global pm_get_idle_wait_time
        .type   pm_get_idle_wait_time, @function
pm_get_idle_wait_time:
        entry    a1, 32
        movi.n   a2, 0
        retw.n
        .size   pm_get_idle_wait_time, .-pm_get_idle_wait_time
.Lsec_289_7:

        .section .text.pm_enable_sta_disconnected_power_management,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_290_0, g_pm
.Lsec_290_4:
        .global pm_enable_sta_disconnected_power_management
        .type   pm_enable_sta_disconnected_power_management, @function
pm_enable_sta_disconnected_power_management:
        entry    a1, 32
        l32r     a9, .Lsec_290_0
        extui    a2, a2, 0, 8
        addmi    a8, a9, 0x100
        l8ui     a10, a8, 29
        bgeui    a10, 2, .Lsec_290_3a
        l8ui     a9, a9, 13
        bnez.n   a9, .Lsec_290_3a
        beqz.n   a2, .Lsec_290_2c
        movi.n   a9, 1
        s8i      a9, a8, 29
        memw
        memw
        j        .Lsec_290_3c
        .byte    0x00
.Lsec_290_2c:
        s8i      a2, a8, 29
        memw
        movi.n   a2, 1
        memw
        j        .Lsec_290_3c
.Lsec_290_3a:
        movi.n   a2, 0
.Lsec_290_3c:
        retw.n
        .size   pm_enable_sta_disconnected_power_management, .-pm_enable_sta_disconnected_power_management
.Lsec_290_3e:

        .section .text.pm_register_connectionless_wake_window,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_291_0, g_osi_funcs_p
        .literal .Lsec_291_4, g_pm
        .literal .Lsec_291_8, pm_update_by_connectionless_status
        .literal .Lsec_291_c, pm_on_coex_schm_status_config
        .literal .Lsec_291_10, pm_coex_schm_process_restart
.Lsec_291_14:
        .global pm_register_connectionless_wake_window
        .type   pm_register_connectionless_wake_window, @function
pm_register_connectionless_wake_window:
        entry    a1, 48
        beqz     a2, .Lsec_291_8c
        l16ui    a3, a2, 0
        beqz     a3, .Lsec_291_8c
        movi.n   a10, 8
        l32r     a4, .Lsec_291_0
        l32i.n   a3, a4, 0
        l32i     a3, a3, 0x16c
        callx8   a3
        beqz     a10, .Lsec_291_8c
        movi.n   a3, 0
        s32i.n   a3, a10, 4
        l32r     a3, .Lsec_291_4
        l32i     a8, a3, 0x160
        s32i.n   a2, a10, 0
        s32i.n   a8, a1, 4
        mov.n    a8, a1
        memw
        j        .Lsec_291_5a
.Lsec_291_46:
        l32i.n   a11, a8, 0
        l16ui    a12, a11, 0
        l16ui    a11, a2, 0
        bgeu     a12, a11, .Lsec_291_5a
        s32i.n   a8, a10, 4
        memw
        j        .Lsec_291_61
        .byte    0x00
.Lsec_291_5a:
        mov.n    a9, a8
        l32i.n   a8, a8, 4
        bnez     a8, .Lsec_291_46
.Lsec_291_61:
        s32i.n   a10, a9, 4
        l32i.n   a2, a1, 4
        s32i     a2, a3, 0x160
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_update_by_connectionless_status
        l32r     a8, .Lsec_291_8
        callx8   a8
        l8ui     a10, a3, 13
        bnez.n   a10, .Lsec_291_8c
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_on_coex_schm_status_config
        l32r     a8, .Lsec_291_c
        callx8   a8
        l32i.n   a2, a4, 0
        l32i     a10, a2, 0x194
        callx8   a10
        beqz.n   a10, .Lsec_291_8c
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_coex_schm_process_restart
        l32r     a8, .Lsec_291_10
        callx8   a8
.Lsec_291_8c:
        retw.n
        .size   pm_register_connectionless_wake_window, .-pm_register_connectionless_wake_window
.Lsec_291_8e:

        .section .text.pm_unregister_connectionless_wake_window,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_292_0, g_pm
        .literal .Lsec_292_4, g_osi_funcs_p
        .literal .Lsec_292_8, pm_update_by_connectionless_status
        .literal .Lsec_292_c, pm_on_coex_schm_status_config
        .literal .Lsec_292_10, pm_coex_schm_process_restart
.Lsec_292_14:
        .global pm_unregister_connectionless_wake_window
        .type   pm_unregister_connectionless_wake_window, @function
pm_unregister_connectionless_wake_window:
        entry    a1, 48
        beqz     a2, .Lsec_292_78
        l32r     a3, .Lsec_292_0
        mov.n    a10, a1
        l32i     a8, a3, 0x160
        s32i.n   a8, a1, 4
        memw
        j        .Lsec_292_44
.Lsec_292_2a:
        l32i.n   a9, a10, 0
        bne      a9, a2, .Lsec_292_44
        l32i.n   a2, a10, 4
        s32i.n   a2, a8, 4
        l32r     a2, .Lsec_292_4
        l32i.n   a2, a2, 0
        l32i     a2, a2, 176
        memw
        callx8   a2
        j        .Lsec_292_4b
.Lsec_292_44:
        mov.n    a8, a10
        l32i.n   a10, a10, 4
        bnez     a10, .Lsec_292_2a
.Lsec_292_4b:
        l32i.n   a2, a1, 4
        s32i     a2, a3, 0x160
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_update_by_connectionless_status
        l32r     a8, .Lsec_292_8
        callx8   a8
        l8ui     a10, a3, 13
        bnez     a10, .Lsec_292_78
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_on_coex_schm_status_config
        l32r     a8, .Lsec_292_c
        callx8   a8
        l32r     a2, .Lsec_292_4
        l32i.n   a2, a2, 0
        l32i     a10, a2, 0x194
        callx8   a10
        beqz.n   a10, .Lsec_292_78
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_coex_schm_process_restart
        l32r     a8, .Lsec_292_10
        callx8   a8
.Lsec_292_78:
        retw.n
        .size   pm_unregister_connectionless_wake_window, .-pm_unregister_connectionless_wake_window
.Lsec_292_7a:

        .section .text.pm_connectionless_module_set_wake_interval,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_293_0, g_pm
        .literal .Lsec_293_4, pm_update_by_connectionless_status
.Lsec_293_8:
        .global pm_connectionless_module_set_wake_interval
        .type   pm_connectionless_module_set_wake_interval, @function
pm_connectionless_module_set_wake_interval:
        entry    a1, 32
        l32r     a8, .Lsec_293_0
        s16i     a2, a8, 0x18a
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_update_by_connectionless_status
        l32r     a8, .Lsec_293_4
        callx8   a8
        retw.n
        .size   pm_connectionless_module_set_wake_interval, .-pm_connectionless_module_set_wake_interval
.Lsec_293_1f:

        .section .wifislprxiram.62,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_294_0, g_pm
        .literal .Lsec_294_4, .Lsec_306_0
        .literal .Lsec_294_8, pm_on_sample_beacon
        .literal .Lsec_294_c, wifi_log
.Lsec_294_10:
        .global pm_set_beacon_duration
        .type   pm_set_beacon_duration, @function
pm_set_beacon_duration:
        entry    a1, 32
        l32r     a3, .Lsec_294_0
        l32i     a8, a3, 0x1d0
        beq      a8, a2, .Lsec_294_40
        bnez     a8, .Lsec_294_25
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_on_sample_beacon
        l32r     a8, .Lsec_294_8
        callx8   a8
.Lsec_294_25:
        l32i     a14, a3, 0x1d0
        movi.n   a11, 1
        mov.n    a15, a2
        movi.n   a12, 4
        slli     a11, a11, 12
        l32r     a13, .Lsec_294_4
        movi     a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_294_c
        callx8   a8
        s32i     a2, a3, 0x1d0
.Lsec_294_40:
        memw
        retw.n
        .size   pm_set_beacon_duration, .-pm_set_beacon_duration
.Lsec_294_45:

        .section .text.pm_get_beacon_duration,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_295_0, g_pm
.Lsec_295_4:
        .global pm_get_beacon_duration
        .type   pm_get_beacon_duration, @function
pm_get_beacon_duration:
        entry    a1, 32
        l32r     a8, .Lsec_295_0
        l32i     a2, a8, 0x1d0
        retw.n
        .size   pm_get_beacon_duration, .-pm_get_beacon_duration
.Lsec_295_f:

        .section .text.pm_beacon_add_total_counter,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_296_0, g_pm
        .literal .Lsec_296_4, ptr_beacon_offset_funcs
.Lsec_296_8:
        .global pm_beacon_add_total_counter
        .type   pm_beacon_add_total_counter, @function
pm_beacon_add_total_counter:
        entry    a1, 32
        l32r     a9, .Lsec_296_0
        l32i     a8, a9, 0x214
        addi.n   a8, a8, 1
        s32i     a8, a9, 0x214
        l32r     a8, .Lsec_296_4
        l32i.n   a8, a8, 0
        l32i.n   a8, a8, 36
        beqz.n   a8, .Lsec_296_25
        memw
        callx8   a8
.Lsec_296_25:
        retw.n
        .size   pm_beacon_add_total_counter, .-pm_beacon_add_total_counter
.Lsec_296_27:

        .section .wifislprxiram.30,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_297_0, ptr_beacon_offset_funcs
        .literal .Lsec_297_4, g_osi_funcs_p
        .literal .Lsec_297_8, __udivdi3
        .literal .Lsec_297_c, pm_beacon_miss_exceeded_wakeup_disabled
        .literal .Lsec_297_10, pm_beacon_add_total_counter
        .literal .Lsec_297_14, pm_beacon_monitor_tbtt_allowed
        .literal .Lsec_297_18, pm_beacon_monitor_tbtt_start
.Lsec_297_1c:
        .global pm_enable_beacon_monitor_timer
        .type   pm_enable_beacon_monitor_timer, @function
pm_enable_beacon_monitor_timer:
        entry    a1, 48
        l32r     a8, .Lsec_297_0
        addmi    a6, a2, 0x100
        l32i.n   a4, a8, 0
        l8ui     a3, a6, 124
        slli     a3, a3, 10
        l32i.n   a10, a4, 16
        l8ui     a7, a6, 214
        l8ui     a5, a6, 215
        bnez     a10, .Lsec_297_45
.Lsec_297_38:
        l8ui     a4, a6, 125
        slli     a4, a4, 10
        add.n    a3, a4, a3
        j        .Lsec_297_ab
        .byte    0x00
        .byte    0x00
.Lsec_297_45:
        callx8   a10
        beqz     a10, .Lsec_297_38
        l32r     a4, .Lsec_297_4
        l32i.n   a4, a4, 0
        l32i     a10, a4, 0x110
        callx8   a10
        or       a14, a10, a10
        l32i     a12, a2, 92
        l32i     a4, a2, 88
        bltu     a11, a12, .Lsec_297_68
        bne      a12, a11, .Lsec_297_99
        bgeu     a10, a4, .Lsec_297_99
.Lsec_297_68:
        sub      a10, a4, a14
        l32i.n   a9, a2, 44
        movi.n   a15, 1
        bltu     a4, a10, .Lsec_297_75
        movi     a15, 0
.Lsec_297_75:
        sub      a11, a12, a11
        movi.n   a13, 0
        mov.n    a12, a9
        sub      a11, a11, a15
        s32i.n   a9, a1, 4
        s32i     a14, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, __udivdi3
        l32r     a8, .Lsec_297_8
        callx8   a8
        addi.n   a10, a10, 1
        l32i.n   a9, a1, 4
        mull     a10, a10, a9
        sub      a4, a4, a10
        l32i.n   a14, a1, 0
.Lsec_297_99:
        sub      a10, a14, a4
        movi.n   a11, 0
        l32i.n   a9, a2, 40
        bgeu     a10, a9, .Lsec_297_a9
        sub      a9, a9, a14
        add      a11, a9, a4
.Lsec_297_a9:
        add.n    a3, a3, a11
.Lsec_297_ab:
        l8ui     a4, a6, 100
        bnez.n   a4, .Lsec_297_b5
.Lsec_297_b0:
        movi.n   a3, 0
        j        .Lsec_297_132
.Lsec_297_b5:
        l8ui     a8, a6, 212
        bnez     a8, .Lsec_297_b0
        l8ui     a4, a2, 13
        beqz     a4, .Lsec_297_b0
        s32i.n   a8, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_beacon_miss_exceeded_wakeup_disabled
        l32r     a8, .Lsec_297_c
        callx8   a8
        l16si    a9, a2, 0x180
        l16si    a4, a2, 0x182
        l32i.n   a8, a1, 0
        beqz.n   a10, .Lsec_297_eb
        bge      a9, a4, .Lsec_297_e5
.Lsec_297_d9:
        l32r     a6, .Lsec_297_0
        l32i.n   a4, a6, 0
        l32i.n   a10, a4, 12
        bnez.n   a10, .Lsec_297_121
        j        .Lsec_297_f7
.Lsec_297_e5:
        s16i     a8, a2, 0x180
        memw
.Lsec_297_eb:
        l16si    a8, a2, 0x180
        bge      a8, a4, .Lsec_297_b0
        memw
        j        .Lsec_297_d9
.Lsec_297_f7:
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_beacon_add_total_counter
        l32r     a8, .Lsec_297_10
        callx8   a8
        l32r     a4, .Lsec_297_4
        movi     a10, 0x168
        mov.n    a11, a3
        movi.n   a12, 0
        add.n    a10, a2, a10
        l32i.n   a4, a4, 0
        l32i     a4, a4, 248
        callx8   a4
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_beacon_monitor_tbtt_allowed
        l32r     a8, .Lsec_297_14
        callx8   a8
        mov.n    a3, a10
        beqz     a10, .Lsec_297_b0
        j        .Lsec_297_12a
.Lsec_297_121:
        callx8   a10
        bnez     a10, .Lsec_297_b0
        j        .Lsec_297_f7
.Lsec_297_12a:
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_beacon_monitor_tbtt_start
        l32r     a8, .Lsec_297_18
        callx8   a8
.Lsec_297_132:
        addi.n   a5, a5, -1
        sub      a5, a5, a7
        movi.n   a2, 1
        movnez   a2, a3, a5
        retw.n
        .size   pm_enable_beacon_monitor_timer, .-pm_enable_beacon_monitor_timer
.Lsec_297_13e:

        .section .wifislprxiram.48,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_298_0, g_osi_funcs_p
        .literal .Lsec_298_4, g_pm
        .literal .Lsec_298_8, g_pm+0xa4
        .literal .Lsec_298_c, g_pm+0xf4
        .literal .Lsec_298_10, g_pm_cnt
        .literal .Lsec_298_14, g_mesh_is_started
        .literal .Lsec_298_18, g_mesh_is_root
        .literal .Lsec_298_1c, g_mesh_init_ps_type
        .literal .Lsec_298_20, 0x3ff73c00
        .literal .Lsec_298_24, g_wifi_mac_time_delta
        .literal .Lsec_298_28, pm_check_state
        .literal .Lsec_298_2c, .Lsec_147_8
        .literal .Lsec_298_30, pm_save_tbtt_info
        .literal .Lsec_298_34, pm_enable_beacon_monitor_timer
        .literal .Lsec_298_38, pm_coex_schm_overall_period_get
        .literal .Lsec_298_3c, pm_beacon_monitor_tbtt_start
        .literal .Lsec_298_40, wifi_gpio_debug
        .literal .Lsec_298_44, pm_dream
        .literal .Lsec_298_48, pm_coex_tbtt_process
        .literal .Lsec_298_4c, pm_noise_check
        .literal .Lsec_298_50, pm_mesh_set_next_tbtt
        .literal .Lsec_298_54, pm_set_next_tbtt
        .literal .Lsec_298_58, pm_go_to_wake
        .literal .Lsec_298_5c, pm_update_params
        .literal .Lsec_298_60, pm_update_by_connectionless_status
.Lsec_298_64:
        .global pm_tbtt_process
        .type   pm_tbtt_process, @function
pm_tbtt_process:
        entry    a1, 32
        l32r     a7, .Lsec_298_0
        l32i.n   a2, a7, 0
        l32i     a10, a2, 0x110
        callx8   a10
        mov.n    a4, a10
        or       a5, a11, a11
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_check_state
        l32r     a8, .Lsec_298_28
        callx8   a8
        l32r     a2, .Lsec_298_4
        l8ui     a3, a2, 13
        bnez.n   a3, .Lsec_298_b0
        l32i     a3, a7, 0
        l32r     a10, .Lsec_298_8
        l32i     a3, a3, 236
        callx8   a3
        l32i.n   a3, a7, 0
        l32r     a10, .Lsec_298_c
        l32i     a3, a3, 236
        callx8   a3
        movi.n   a3, 1
        s8i      a3, a2, 24
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, .Lsec_147_8
        l32r     a8, .Lsec_298_2c
        callx8   a8
        j        .Lsec_298_1e1
.Lsec_298_b0:
        mov.n    a10, a2
        movi.n   a6, 1
        l8ui     a3, a2, 17
        s8i      a6, a2, 17
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_save_tbtt_info
        l32r     a8, .Lsec_298_30
        callx8   a8
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_enable_beacon_monitor_timer
        l32r     a8, .Lsec_298_34
        callx8   a8
        bnez.n   a10, .Lsec_298_ec
        l32i     a6, a7, 0
        l32i     a10, a6, 0x194
        callx8   a10
        beqz.n   a10, .Lsec_298_ec
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_coex_schm_overall_period_get
        l32r     a8, .Lsec_298_38
        callx8   a8
        beqi     a10, 1, .Lsec_298_ec
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_beacon_monitor_tbtt_start
        l32r     a8, .Lsec_298_3c
        callx8   a8
.Lsec_298_ec:
        movi.n   a11, 2
        movi.n   a10, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_gpio_debug
        l32r     a8, .Lsec_298_40
        callx8   a8
        l8ui     a8, a2, 1
        l32r     a6, .Lsec_298_10
        bnei     a8, 2, .Lsec_298_116
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_dream
        l32r     a8, .Lsec_298_44
        callx8   a8
        movi.n   a8, 0
        s8i      a8, a2, 18
        memw
        l32i     a8, a6, 16
        addi     a8, a8, 1
        s32i     a8, a6, 16
.Lsec_298_116:
        beqz.n   a3, .Lsec_298_124
.Lsec_298_118:
        l8ui     a3, a2, 14
        bnez     a3, .Lsec_298_182
        memw
        j        .Lsec_298_12e
.Lsec_298_124:
        l32i.n   a3, a7, 0
        l32i     a10, a3, 0x194
        callx8   a10
        bnez.n   a10, .Lsec_298_139
.Lsec_298_12e:
        l32r     a3, .Lsec_298_14
        l8ui     a3, a3, 0
        bnez.n   a3, .Lsec_298_145
        j        .Lsec_298_169
.Lsec_298_139:
        addmi    a3, a2, 0x200
        l8ui     a3, a3, 29
        bnez     a3, .Lsec_298_118
        j        .Lsec_298_12e
.Lsec_298_145:
        l32r     a3, .Lsec_298_18
        memw
        l8ui     a3, a3, 0
        extui    a3, a3, 0, 8
        bnez.n   a3, .Lsec_298_169
        l32r     a8, .Lsec_298_1c
        l32i.n   a8, a8, 0
        beqz     a8, .Lsec_298_169
        l32i     a7, a7, 0
        l32i     a10, a7, 0x110
        callx8   a10
        j        .Lsec_298_17c
        .byte    0x00
        .byte    0x00
.Lsec_298_169:
        l32r     a3, .Lsec_298_20
        memw
        l32i.n   a10, a3, 0
        l32r     a3, .Lsec_298_24
        l32i.n   a3, a3, 0
        add      a10, a10, a3
        movi     a3, 0
.Lsec_298_17c:
        s32i     a10, a2, 80
        s32i     a3, a2, 84
.Lsec_298_182:
        mov.n    a11, a5
        mov.n    a10, a4
        l16ui    a3, a2, 28
        addi.n   a3, a3, 1
        s16i     a3, a2, 28
        memw
        l32i.n   a3, a6, 0
        addi.n   a3, a3, 1
        s32i     a3, a6, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_coex_tbtt_process
        l32r     a8, .Lsec_298_48
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_noise_check
        l32r     a8, .Lsec_298_4c
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_mesh_set_next_tbtt
        l32r     a8, .Lsec_298_50
        callx8   a8
        bnez.n   a10, .Lsec_298_1e1
        addmi    a3, a2, 0x100
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_set_next_tbtt
        l32r     a8, .Lsec_298_54
        callx8   a8
        l8ui     a4, a3, 212
        beqz.n   a4, .Lsec_298_1d0
        l8ui     a4, a2, 14
        beqz.n   a4, .Lsec_298_1d0
        l8ui     a10, a2, 15
        bnez     a10, .Lsec_298_1d0
        mov.n    a11, a10
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_go_to_wake
        l32r     a8, .Lsec_298_58
        callx8   a8
.Lsec_298_1d0:
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_update_params
        l32r     a8, .Lsec_298_5c
        callx8   a8
        l8ui     a2, a3, 140
        bnez.n   a2, .Lsec_298_1e1
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_update_by_connectionless_status
        l32r     a8, .Lsec_298_60
        callx8   a8
.Lsec_298_1e1:
        retw.n
        .size   pm_tbtt_process, .-pm_tbtt_process
.Lsec_298_1e3:

        .section .wifislprxiram.45,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_299_0, g_osi_funcs_p
        .literal .Lsec_299_4, pm_tbtt_process
.Lsec_299_8:
        .global pm_beacon_monitor_tbtt_timeout_process
        .type   pm_beacon_monitor_tbtt_timeout_process, @function
pm_beacon_monitor_tbtt_timeout_process:
        entry    a1, 32
        l32r     a8, .Lsec_299_0
        l32i.n   a8, a8, 0
        l32i     a10, a2, 0x184
        l32i     a8, a8, 236
        callx8   a8
        movi.n   a10, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_tbtt_process
        l32r     a8, .Lsec_299_4
        callx8   a8
        retw.n
        .size   pm_beacon_monitor_tbtt_timeout_process, .-pm_beacon_monitor_tbtt_timeout_process
.Lsec_299_23:

        .section .text.pm_beacon_add_loss_counter,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_300_0, g_pm
        .literal .Lsec_300_4, ptr_beacon_offset_funcs
.Lsec_300_8:
        .global pm_beacon_add_loss_counter
        .type   pm_beacon_add_loss_counter, @function
pm_beacon_add_loss_counter:
        entry    a1, 32
        l32r     a9, .Lsec_300_0
        l32i     a8, a9, 0x218
        addi.n   a8, a8, 1
        s32i     a8, a9, 0x218
        l32r     a8, .Lsec_300_4
        l32i.n   a8, a8, 0
        l32i.n   a8, a8, 40
        beqz.n   a8, .Lsec_300_25
        memw
        callx8   a8
.Lsec_300_25:
        retw.n
        .size   pm_beacon_add_loss_counter, .-pm_beacon_add_loss_counter
.Lsec_300_27:

        .section .wifislprxiram.44,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_301_0, g_osi_funcs_p
        .literal .Lsec_301_4, pm_beacon_add_loss_counter
        .literal .Lsec_301_8, pm_sleep
        .literal .Lsec_301_c, wifi_gpio_debug
.Lsec_301_10:
        .global pm_beacon_monitor_timeout_process
        .type   pm_beacon_monitor_timeout_process, @function
pm_beacon_monitor_timeout_process:
        entry    a1, 32
        addmi    a3, a2, 0x100
        l8ui     a4, a3, 100
        beqz     a4, .Lsec_301_7c
        l8ui     a4, a3, 212
        bnez     a4, .Lsec_301_7c
        l8ui     a8, a2, 13
        beqz     a8, .Lsec_301_7c
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_beacon_add_loss_counter
        l32r     a8, .Lsec_301_4
        callx8   a8
        l8ui     a8, a2, 1
        bnei     a8, 1, .Lsec_301_72
        l16ui    a8, a2, 0x180
        sext     a10, a8, 15
        addi.n   a8, a8, 1
        extui    a8, a8, 0, 16
        l16si    a9, a2, 0x182
        s16i     a8, a2, 0x180
        memw
        bge      a10, a9, .Lsec_301_72
        or       a10, a4, a4
        l8ui     a9, a3, 127
        mul16u   a8, a9, a8
        s8i      a8, a3, 125
        s8i      a4, a2, 17
        memw
        l32r     a2, .Lsec_301_0
        l32i     a2, a2, 0
        l32i     a2, a2, 0x1a0
        memw
        callx8   a2
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_sleep
        l32r     a8, .Lsec_301_8
        callx8   a8
.Lsec_301_72:
        movi.n   a11, 2
        mov.n    a10, a11
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_gpio_debug
        l32r     a8, .Lsec_301_c
        callx8   a8
.Lsec_301_7c:
        retw.n
        .size   pm_beacon_monitor_timeout_process, .-pm_beacon_monitor_timeout_process
.Lsec_301_7e:

        .section .text.pm_beacon_loss_info_get,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_302_0, g_pm
.Lsec_302_4:
        .global pm_beacon_loss_info_get
        .type   pm_beacon_loss_info_get, @function
pm_beacon_loss_info_get:
        entry    a1, 32
        l32r     a8, .Lsec_302_0
        l32i     a9, a8, 0x214
        s32i.n   a9, a2, 0
        l32i     a8, a8, 0x218
        s32i.n   a8, a3, 0
        memw
        retw.n
        .size   pm_beacon_loss_info_get, .-pm_beacon_loss_info_get
.Lsec_302_19:

        .section .text.pm_beacon_loss_info_clean,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_303_0, g_pm
.Lsec_303_4:
        .global pm_beacon_loss_info_clean
        .type   pm_beacon_loss_info_clean, @function
pm_beacon_loss_info_clean:
        entry    a1, 32
        l32r     a8, .Lsec_303_0
        movi.n   a9, 0
        s32i     a9, a8, 0x218
        s32i     a9, a8, 0x214
        memw
        retw.n
        .size   pm_beacon_loss_info_clean, .-pm_beacon_loss_info_clean
.Lsec_303_17:

        .section .text.pm_get_tx_blocks_retention_mask,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_304_0, 0xfff1ffff
        .literal .Lsec_304_4, g_pm
.Lsec_304_8:
        .global pm_get_tx_blocks_retention_mask
        .type   pm_get_tx_blocks_retention_mask, @function
pm_get_tx_blocks_retention_mask:
        entry    a1, 32
        l32r     a8, .Lsec_304_4
        movi.n   a2, -1
        l8ui     a9, a8, 13
        beqz.n   a9, .Lsec_304_2a
        l8ui     a9, a8, 14
        l32r     a2, .Lsec_304_0
        beqz.n   a9, .Lsec_304_2a
        addmi    a8, a8, 0x200
        movi.n   a2, -1
        l8ui     a8, a8, 30
        beqz.n   a8, .Lsec_304_2a
        l32r     a2, .Lsec_304_0
.Lsec_304_2a:
        retw.n
        .size   pm_get_tx_blocks_retention_mask, .-pm_get_tx_blocks_retention_mask
.Lsec_304_2c:

        .section .bss.beacon_monitor_data$0,"aw",@nobits
        .align  4
.Lsec_305_0:
        .skip   20

        .section .rodata_wlog_debug.63,"a",@progbits
        .align  1
.Lsec_306_0:
        .byte   0x62, 0x63, 0x6e, 0x64, 0x3a, 0x20, 0x25, 0x6c, 0x75, 0x20, 0x2d, 0x3e, 0x20, 0x25, 0x6c, 0x75
        .byte   0x00

        .section .rodata_wlog_info.61,"a",@progbits
        .align  1
.Lsec_307_0:
        .byte   0x43, 0x6f, 0x65, 0x78, 0x69, 0x73, 0x74, 0x21, 0x21, 0x21, 0x20, 0x57, 0x69, 0x2d, 0x46, 0x69
        .byte   0x20, 0x73, 0x74, 0x61, 0x74, 0x69, 0x6f, 0x6e, 0x20, 0x77, 0x6f, 0x75, 0x6c, 0x64, 0x20, 0x6f
        .byte   0x6e, 0x6c, 0x79, 0x20, 0x6b, 0x65, 0x65, 0x70, 0x20, 0x77, 0x61, 0x6b, 0x65, 0x64, 0x20, 0x77
        .byte   0x68, 0x65, 0x6e, 0x20, 0x61, 0x76, 0x61, 0x69, 0x6c, 0x61, 0x62, 0x6c, 0x65, 0x0a, 0x00

        .section .rodata_wlog_info.60,"a",@progbits
        .align  1
.Lsec_308_0:
        .byte   0x53, 0x65, 0x74, 0x20, 0x70, 0x73, 0x20, 0x74, 0x79, 0x70, 0x65, 0x3a, 0x20, 0x25, 0x64, 0x2c
        .byte   0x20, 0x63, 0x6f, 0x65, 0x78, 0x69, 0x73, 0x74, 0x3a, 0x20, 0x25, 0x64, 0x0a, 0x00

        .section .rodata_wlog_info.59,"a",@progbits
        .align  1
.Lsec_309_0:
        .byte   0x28, 0x6d, 0x65, 0x73, 0x68, 0x29, 0x70, 0x6d, 0x20, 0x73, 0x74, 0x6f, 0x70, 0x2c, 0x20, 0x73
        .byte   0x6c, 0x65, 0x65, 0x70, 0x3a, 0x25, 0x6c, 0x6c, 0x64, 0x20, 0x73, 0x65, 0x63, 0x73, 0x2c, 0x20
        .byte   0x74, 0x6f, 0x74, 0x61, 0x6c, 0x3a, 0x25, 0x6c, 0x6c, 0x64, 0x20, 0x73, 0x65, 0x63, 0x73, 0x2c
        .byte   0x20, 0x63, 0x75, 0x72, 0x72, 0x65, 0x6e, 0x74, 0x20, 0x61, 0x63, 0x74, 0x69, 0x76, 0x65, 0x20
        .byte   0x73, 0x65, 0x74, 0x3a, 0x25, 0x64, 0x2c, 0x20, 0x72, 0x75, 0x6e, 0x6e, 0x69, 0x6e, 0x67, 0x3a
        .byte   0x25, 0x2e, 0x30, 0x32, 0x66, 0x00

        .section .rodata_wlog_info.58,"a",@progbits
        .align  1
.Lsec_310_0:
        .byte   0x70, 0x6d, 0x20, 0x73, 0x74, 0x6f, 0x70, 0x2c, 0x20, 0x74, 0x6f, 0x74, 0x61, 0x6c, 0x20, 0x73
        .byte   0x6c, 0x65, 0x65, 0x70, 0x20, 0x74, 0x69, 0x6d, 0x65, 0x3a, 0x20, 0x25, 0x6c, 0x6c, 0x75, 0x20
        .byte   0x75, 0x73, 0x20, 0x2f, 0x20, 0x25, 0x6c, 0x6c, 0x75, 0x20, 0x75, 0x73, 0x0a, 0x00

        .section .rodata_wlog_info.57,"a",@progbits
        .align  1
.Lsec_311_0:
        .byte   0x70, 0x6d, 0x20, 0x73, 0x74, 0x61, 0x72, 0x74, 0x2c, 0x20, 0x74, 0x79, 0x70, 0x65, 0x3a, 0x20
        .byte   0x25, 0x64, 0x0a, 0x00

        .section .rodata_wlog_info.53,"a",@progbits
        .align  1
.Lsec_312_0:
        .byte   0x43, 0x6f, 0x65, 0x78, 0x69, 0x73, 0x74, 0x21, 0x21, 0x21, 0x20, 0x57, 0x69, 0x2d, 0x46, 0x69
        .byte   0x20, 0x73, 0x74, 0x61, 0x74, 0x69, 0x6f, 0x6e, 0x20, 0x77, 0x6f, 0x75, 0x6c, 0x64, 0x20, 0x6f
        .byte   0x6e, 0x6c, 0x79, 0x20, 0x6b, 0x65, 0x65, 0x70, 0x20, 0x77, 0x61, 0x6b, 0x65, 0x64, 0x20, 0x77
        .byte   0x68, 0x65, 0x6e, 0x20, 0x61, 0x76, 0x61, 0x69, 0x6c, 0x61, 0x62, 0x6c, 0x65, 0x0a, 0x00

        .section .rodata_wlog_warning.41,"a",@progbits
        .align  1
.Lsec_313_0:
        .byte   0x4e, 0x65, 0x78, 0x74, 0x20, 0x54, 0x42, 0x54, 0x54, 0x20, 0x69, 0x6e, 0x63, 0x6f, 0x72, 0x72
        .byte   0x65, 0x63, 0x74, 0x21, 0x20, 0x6c, 0x61, 0x73, 0x74, 0x20, 0x62, 0x65, 0x61, 0x63, 0x6f, 0x6e
        .byte   0x3a, 0x25, 0x75, 0x2c, 0x20, 0x6f, 0x66, 0x66, 0x73, 0x65, 0x74, 0x3a, 0x25, 0x75, 0x2c, 0x20
        .byte   0x6e, 0x65, 0x78, 0x74, 0x20, 0x62, 0x65, 0x61, 0x63, 0x6f, 0x6e, 0x3a, 0x25, 0x75, 0x2c, 0x20
        .byte   0x62, 0x65, 0x61, 0x63, 0x6f, 0x6e, 0x20, 0x69, 0x6e, 0x74, 0x65, 0x72, 0x76, 0x61, 0x6c, 0x3a
        .byte   0x25, 0x75, 0x2c, 0x20, 0x64, 0x74, 0x69, 0x6d, 0x20, 0x70, 0x65, 0x72, 0x69, 0x6f, 0x64, 0x3a
        .byte   0x25, 0x75, 0x2c, 0x20, 0x64, 0x74, 0x69, 0x6d, 0x20, 0x63, 0x6f, 0x75, 0x6e, 0x74, 0x3a, 0x25
        .byte   0x75, 0x2c, 0x20, 0x6c, 0x69, 0x73, 0x74, 0x65, 0x6e, 0x20, 0x69, 0x6e, 0x74, 0x65, 0x72, 0x76
        .byte   0x61, 0x6c, 0x3a, 0x25, 0x75, 0x2c, 0x20, 0x6e, 0x6f, 0x77, 0x3a, 0x25, 0x75, 0x00

        .section .rodata_wlog_error.39,"a",@progbits
        .align  1
.Lsec_314_0:
        .byte   0x25, 0x73, 0x20, 0x25, 0x64, 0x00

        .section .rodata_wlog_warning.37,"a",@progbits
        .align  1
.Lsec_315_0:
        .byte   0x54, 0x42, 0x54, 0x54, 0x20, 0x64, 0x65, 0x6c, 0x61, 0x79, 0x20, 0x74, 0x6f, 0x6f, 0x20, 0x6d
        .byte   0x75, 0x63, 0x68, 0x21, 0x20, 0x65, 0x6e, 0x74, 0x72, 0x79, 0x3a, 0x25, 0x6c, 0x6c, 0x75, 0x2c
        .byte   0x20, 0x6e, 0x6f, 0x77, 0x3a, 0x25, 0x6c, 0x6c, 0x75, 0x2c, 0x20, 0x6e, 0x65, 0x78, 0x74, 0x20
        .byte   0x62, 0x65, 0x61, 0x63, 0x6f, 0x6e, 0x3a, 0x25, 0x6c, 0x6c, 0x75, 0x2c, 0x20, 0x62, 0x65, 0x61
        .byte   0x63, 0x6f, 0x6e, 0x20, 0x69, 0x6e, 0x74, 0x65, 0x72, 0x76, 0x61, 0x6c, 0x3a, 0x25, 0x75, 0x00

        .section .rodata_wlog_debug.35,"a",@progbits
        .align  1
.Lsec_316_0:
        .byte   0x45, 0x78, 0x65, 0x63, 0x75, 0x74, 0x69, 0x6e, 0x67, 0x20, 0x6f, 0x66, 0x66, 0x63, 0x68, 0x61
        .byte   0x6e, 0x20, 0x6f, 0x70, 0x65, 0x72, 0x61, 0x74, 0x69, 0x6f, 0x6e, 0x20, 0x25, 0x70, 0x00

        .section .rodata_wlog_error.34,"a",@progbits
        .align  1
.Lsec_317_0:
        .byte   0x66, 0x61, 0x69, 0x6c, 0x65, 0x64, 0x20, 0x74, 0x6f, 0x20, 0x73, 0x65, 0x6e, 0x64, 0x20, 0x4e
        .byte   0x55, 0x4c, 0x4c, 0x20, 0x66, 0x72, 0x61, 0x6d, 0x65, 0x00

        .section .rodata_wlog_info.18,"a",@progbits
        .align  1
.Lsec_318_0:
        .byte   0x41, 0x50, 0x27, 0x73, 0x20, 0x62, 0x65, 0x61, 0x63, 0x6f, 0x6e, 0x20, 0x69, 0x6e, 0x74, 0x65
        .byte   0x72, 0x76, 0x61, 0x6c, 0x20, 0x3d, 0x20, 0x25, 0x75, 0x20, 0x75, 0x73, 0x2c, 0x20, 0x44, 0x54
        .byte   0x49, 0x4d, 0x20, 0x70, 0x65, 0x72, 0x69, 0x6f, 0x64, 0x20, 0x3d, 0x20, 0x25, 0x75, 0x00

        .section .rodata_wlog_info.17,"a",@progbits
        .align  1
.Lsec_319_0:
        .byte   0x41, 0x6e, 0x64, 0x20, 0x75, 0x73, 0x65, 0x20, 0x61, 0x6e, 0x6f, 0x74, 0x68, 0x65, 0x72, 0x20
        .byte   0x6f, 0x75, 0x74, 0x65, 0x72, 0x2f, 0x6d, 0x6f, 0x62, 0x69, 0x6c, 0x65, 0x2d, 0x68, 0x6f, 0x74
        .byte   0x73, 0x70, 0x6f, 0x74, 0x20, 0x69, 0x6e, 0x73, 0x74, 0x65, 0x61, 0x64, 0x2e, 0x00

        .section .rodata_wlog_info.16,"a",@progbits
        .align  1
.Lsec_320_0:
        .byte   0x50, 0x6c, 0x65, 0x61, 0x73, 0x65, 0x20, 0x72, 0x65, 0x70, 0x6f, 0x72, 0x74, 0x20, 0x74, 0x68
        .byte   0x69, 0x73, 0x20, 0x62, 0x75, 0x67, 0x20, 0x74, 0x6f, 0x20, 0x74, 0x68, 0x65, 0x20, 0x6d, 0x61
        .byte   0x6e, 0x75, 0x66, 0x61, 0x74, 0x75, 0x72, 0x65, 0x72, 0x20, 0x6f, 0x66, 0x20, 0x74, 0x68, 0x65
        .byte   0x20, 0x72, 0x6f, 0x75, 0x74, 0x65, 0x72, 0x2f, 0x6d, 0x6f, 0x62, 0x69, 0x6c, 0x65, 0x2d, 0x68
        .byte   0x6f, 0x74, 0x73, 0x70, 0x6f, 0x74, 0x2e, 0x00

        .section .rodata_wlog_info.15,"a",@progbits
        .align  1
.Lsec_321_0:
        .byte   0x54, 0x68, 0x65, 0x20, 0x72, 0x6f, 0x75, 0x74, 0x65, 0x72, 0x2f, 0x6d, 0x6f, 0x62, 0x69, 0x6c
        .byte   0x65, 0x2d, 0x68, 0x6f, 0x74, 0x73, 0x70, 0x6f, 0x74, 0x20, 0x64, 0x6f, 0x65, 0x73, 0x20, 0x6e
        .byte   0x6f, 0x74, 0x20, 0x66, 0x6f, 0x6c, 0x6c, 0x6f, 0x77, 0x20, 0x57, 0x69, 0x2d, 0x46, 0x69, 0x20
        .byte   0x70, 0x72, 0x6f, 0x74, 0x6f, 0x63, 0x6f, 0x6c, 0x2e, 0x00

        .section .rodata_wlog_info.14,"a",@progbits
        .align  1
.Lsec_322_0:
        .byte   0x54, 0x68, 0x65, 0x72, 0x65, 0x20, 0x69, 0x73, 0x20, 0x6e, 0x6f, 0x20, 0x54, 0x49, 0x4d, 0x20
        .byte   0x49, 0x45, 0x20, 0x69, 0x6e, 0x20, 0x42, 0x65, 0x61, 0x63, 0x6f, 0x6e, 0x20, 0x66, 0x72, 0x61
        .byte   0x6d, 0x65, 0x2e, 0x00

        .section .rodata_wlog_info.13,"a",@progbits
        .align  1
.Lsec_323_0:
        .byte   0x41, 0x6e, 0x64, 0x20, 0x75, 0x73, 0x65, 0x20, 0x61, 0x6e, 0x6f, 0x74, 0x68, 0x65, 0x72, 0x20
        .byte   0x6f, 0x75, 0x74, 0x65, 0x72, 0x2f, 0x6d, 0x6f, 0x62, 0x69, 0x6c, 0x65, 0x2d, 0x68, 0x6f, 0x74
        .byte   0x73, 0x70, 0x6f, 0x74, 0x20, 0x69, 0x6e, 0x73, 0x74, 0x65, 0x61, 0x64, 0x2e, 0x00

        .section .rodata_wlog_info.12,"a",@progbits
        .align  1
.Lsec_324_0:
        .byte   0x50, 0x6c, 0x65, 0x61, 0x73, 0x65, 0x20, 0x72, 0x65, 0x70, 0x6f, 0x72, 0x74, 0x20, 0x74, 0x68
        .byte   0x69, 0x73, 0x20, 0x62, 0x75, 0x67, 0x20, 0x74, 0x6f, 0x20, 0x74, 0x68, 0x65, 0x20, 0x6d, 0x61
        .byte   0x6e, 0x75, 0x66, 0x61, 0x74, 0x75, 0x72, 0x65, 0x72, 0x20, 0x6f, 0x66, 0x20, 0x74, 0x68, 0x65
        .byte   0x20, 0x72, 0x6f, 0x75, 0x74, 0x65, 0x72, 0x2f, 0x6d, 0x6f, 0x62, 0x69, 0x6c, 0x65, 0x2d, 0x68
        .byte   0x6f, 0x74, 0x73, 0x70, 0x6f, 0x74, 0x2e, 0x00

        .section .rodata_wlog_info.11,"a",@progbits
        .align  1
.Lsec_325_0:
        .byte   0x54, 0x68, 0x65, 0x20, 0x72, 0x6f, 0x75, 0x74, 0x65, 0x72, 0x2f, 0x6d, 0x6f, 0x62, 0x69, 0x6c
        .byte   0x65, 0x2d, 0x68, 0x6f, 0x74, 0x73, 0x70, 0x6f, 0x74, 0x20, 0x64, 0x6f, 0x65, 0x73, 0x20, 0x6e
        .byte   0x6f, 0x74, 0x20, 0x66, 0x6f, 0x6c, 0x6c, 0x6f, 0x77, 0x20, 0x57, 0x69, 0x2d, 0x46, 0x69, 0x20
        .byte   0x70, 0x72, 0x6f, 0x74, 0x6f, 0x63, 0x6f, 0x6c, 0x2e, 0x00

        .section .rodata_wlog_info.10,"a",@progbits
        .align  1
.Lsec_326_0:
        .byte   0x42, 0x65, 0x61, 0x63, 0x6f, 0x6e, 0x20, 0x69, 0x6e, 0x74, 0x65, 0x72, 0x76, 0x61, 0x6c, 0x20
        .byte   0x49, 0x45, 0x20, 0x6f, 0x66, 0x20, 0x42, 0x65, 0x61, 0x63, 0x6f, 0x6e, 0x20, 0x66, 0x72, 0x61
        .byte   0x6d, 0x65, 0x20, 0x69, 0x73, 0x20, 0x30, 0x2e, 0x00

        .section .rodata_wlog_info.6,"a",@progbits
        .align  1
.Lsec_327_0:
        .byte   0x64, 0x70, 0x3a, 0x20, 0x25, 0x75, 0x2c, 0x20, 0x62, 0x69, 0x3a, 0x20, 0x25, 0x75, 0x2c, 0x20
        .byte   0x6c, 0x69, 0x3a, 0x20, 0x25, 0x75, 0x2c, 0x20, 0x73, 0x63, 0x61, 0x6c, 0x65, 0x20, 0x6c, 0x69
        .byte   0x73, 0x74, 0x65, 0x6e, 0x20, 0x69, 0x6e, 0x74, 0x65, 0x72, 0x76, 0x61, 0x6c, 0x20, 0x66, 0x72
        .byte   0x6f, 0x6d, 0x20, 0x25, 0x75, 0x20, 0x75, 0x73, 0x20, 0x74, 0x6f, 0x20, 0x25, 0x75, 0x20, 0x75
        .byte   0x73, 0x00

        .section .bss.g_offchan_ready_ctx,"aw",@nobits
        .align  4
.Lsec_328_0:
        .skip   4

        .section .bss.g_offchan_ready_cb,"aw",@nobits
        .align  4
.Lsec_329_0:
        .skip   4

        .section .bss.g_pm_tx_null_func,"aw",@nobits
        .align  4
.Lsec_330_0:
        .skip   4

        .section .bss.g_pm,"aw",@nobits
        .align  8
.Lsec_331_0:
        .global g_pm
        .type   g_pm, @object
        .size   g_pm, 544
g_pm:
        .skip   544

        .section .data.g_pm_cfg,"aw",@progbits
        .align  4
.Lsec_332_0:
        .global g_pm_cfg
        .type   g_pm_cfg, @object
        .size   g_pm_cfg, 52
g_pm_cfg:
        .byte   0x00, 0x00, 0x00, 0x00, 0xb8, 0x0b, 0x00, 0x00, 0xac, 0x0d, 0x00, 0x00, 0xa8, 0x61, 0x00, 0x00
        .byte   0x80, 0x96, 0x98, 0x00, 0x50, 0xc3, 0x00, 0x00, 0x88, 0x13, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x19, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x98, 0x3a, 0x00, 0x00
        .byte   0x3c, 0x00, 0x00, 0x01
