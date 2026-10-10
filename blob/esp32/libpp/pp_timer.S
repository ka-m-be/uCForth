.section .text.pp_timer_resample,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_20_0, ptr_beacon_offset_funcs
.Lsec_20_4:
        entry    a1, 32
        l32r     a8, .Lsec_20_0
        l32i.n   a8, a8, 0
        l32i.n   a8, a8, 24
        beqz.n   a8, .Lsec_20_15
        l32i.n   a10, a2, 4
        callx8   a8
.Lsec_20_15:
        movi.n   a2, 1
        retw.n

        .section .text.pp_timer_coex_restart,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_21_0, pm_coex_restart_timeout_process
.Lsec_21_4:
        entry    a1, 32
        l32i.n   a10, a2, 4
        movi.n   a2, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_coex_restart_timeout_process
        l32r     a8, .Lsec_21_0
        callx8   a8
        retw.n

        .section .text.pp_timer_max_idle,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_22_0, pm_max_idle_timeout_process
.Lsec_22_4:
        entry    a1, 32
        l32i.n   a10, a2, 4
        movi.n   a2, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_max_idle_timeout_process
        l32r     a8, .Lsec_22_0
        callx8   a8
        retw.n

        .section .text.pp_timer_beacon_monitor_tbtt,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_23_0, pm_beacon_monitor_tbtt_timeout_process
.Lsec_23_4:
        entry    a1, 32
        l32i.n   a10, a2, 4
        movi.n   a2, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_beacon_monitor_tbtt_timeout_process
        l32r     a8, .Lsec_23_0
        callx8   a8
        retw.n

        .section .text.pp_timer_beacon_monitor,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_24_0, pm_beacon_monitor_timeout_process
.Lsec_24_4:
        entry    a1, 32
        l32i.n   a10, a2, 4
        movi.n   a2, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_beacon_monitor_timeout_process
        l32r     a8, .Lsec_24_0
        callx8   a8
        retw.n

        .section .text.pp_timer_connectionless_wake_window,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_25_0, pm_connectionless_wake_window_timeout_process
.Lsec_25_4:
        entry    a1, 32
        l32i.n   a10, a2, 4
        movi.n   a2, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_connectionless_wake_window_timeout_process
        l32r     a8, .Lsec_25_0
        callx8   a8
        retw.n

        .section .text.pp_timer_connectionless_wake_interval,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_26_0, pm_connectionless_wake_interval_timeout_process
.Lsec_26_4:
        entry    a1, 32
        l32i.n   a10, a2, 4
        movi.n   a2, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_connectionless_wake_interval_timeout_process
        l32r     a8, .Lsec_26_0
        callx8   a8
        retw.n

        .section .text.pp_timer_disconnected_sleep_delay,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_27_0, pm_disconnected_sleep_delay_timeout_process
.Lsec_27_4:
        entry    a1, 32
        l32i.n   a10, a2, 4
        movi.n   a2, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_disconnected_sleep_delay_timeout_process
        l32r     a8, .Lsec_27_0
        callx8   a8
        retw.n

        .section .text.pp_timer_sleep_delay,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_28_0, pm_sleep_delay_timeout_process
.Lsec_28_4:
        entry    a1, 32
        l32i.n   a10, a2, 4
        movi.n   a2, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_sleep_delay_timeout_process
        l32r     a8, .Lsec_28_0
        callx8   a8
        retw.n

        .section .text.pp_timer_coex_slice,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_29_0, pm_coex_slice_timeout_process
.Lsec_29_4:
        entry    a1, 32
        l32i.n   a10, a2, 4
        movi.n   a2, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_coex_slice_timeout_process
        l32r     a8, .Lsec_29_0
        callx8   a8
        retw.n

        .section .text.pp_timer_active,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_30_0, pm_active_timeout_process
.Lsec_30_4:
        entry    a1, 32
        l32i.n   a10, a2, 4
        movi.n   a2, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_active_timeout_process
        l32r     a8, .Lsec_30_0
        callx8   a8
        retw.n

        .section .text.pp_timer_dream,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_31_0, pm_dream_timeout_process
.Lsec_31_4:
        entry    a1, 32
        l32i.n   a10, a2, 4
        movi.n   a2, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_dream_timeout_process
        l32r     a8, .Lsec_31_0
        callx8   a8
        retw.n

        .section .wifislpiram.2,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_32_0, pm_tbtt_process
.Lsec_32_4:
        entry    a1, 32
        l32i.n   a10, a2, 4
        movi.n   a2, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_tbtt_process
        l32r     a8, .Lsec_32_0
        callx8   a8
        retw.n

        .section .text.pp_timer_noise_check,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_33_0, pm_noise_check_process
.Lsec_33_4:
        entry    a1, 32
        l32i.n   a10, a2, 4
        movi.n   a2, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_noise_check_process
        l32r     a8, .Lsec_33_0
        callx8   a8
        retw.n

        .section .wifislpiram.3,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_34_0, .Lsec_38_0
        .literal .Lsec_34_4, g_pp_timer_info
        .literal .Lsec_34_8, .Lsec_37_0
        .literal .Lsec_34_c, g_osi_funcs_p
        .literal .Lsec_34_10, wifi_log
        .literal .Lsec_34_14, wifi_log
.Lsec_34_18:
        .global pp_timer_do_process
        .type   pp_timer_do_process, @function
pp_timer_do_process:
        entry    a1, 32
        bnez.n   a2, .Lsec_34_34
        l32r     a13, .Lsec_34_0
        movi.n   a11, 1
        movi.n   a12, 2
        slli     a11, a11, 11
        movi     a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_34_10
        callx8   a8
        j        .Lsec_34_a8
        .byte    0x00
.Lsec_34_34:
        l8ui     a12, a2, 0
        l32r     a9, .Lsec_34_4
        addx8    a8, a12, a9
        l8ui     a8, a8, 0
        beq      a8, a12, .Lsec_34_5c
        mov.n    a11, a9
        movi.n   a8, 0
        movi.n   a10, 14
        loop     a10, .Lsec_34_59
        l8ui     a13, a11, 0
        beq      a13, a12, .Lsec_34_61
        addi.n   a8, a8, 1
        extui    a8, a8, 0, 8
        addi.n   a11, a11, 8
.Lsec_34_59:
        j        .Lsec_34_70
.Lsec_34_5c:
        movi.n   a10, 13
        bltu     a10, a8, .Lsec_34_70
.Lsec_34_61:
        addx8    a8, a8, a9
        l32i.n   a8, a8, 4
        beqz.n   a8, .Lsec_34_99
        mov.n    a10, a2
        callx8   a8
        j        .Lsec_34_99
.Lsec_34_70:
        l32r     a13, .Lsec_34_8
        movi     a11, 1
        movi     a12, 2
        slli     a11, a11, 11
        movi     a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_34_14
        callx8   a8
        l32r     a8, .Lsec_34_c
        mov.n    a10, a2
        movi.n   a2, 0
        l32i.n   a8, a8, 0
        l32i     a8, a8, 176
        callx8   a8
        j        .Lsec_34_a8
        .byte    0x00
        .byte    0x00
.Lsec_34_99:
        l32r     a8, .Lsec_34_c
        mov.n    a10, a2
        movi.n   a2, 1
        l32i.n   a8, a8, 0
        l32i     a8, a8, 176
        callx8   a8
.Lsec_34_a8:
        retw.n
        .size   pp_timer_do_process, .-pp_timer_do_process
.Lsec_34_aa:

        .section .text.pp_timer_register_post_cb,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_35_0, .Lsec_40_0
.Lsec_35_4:
        .global pp_timer_register_post_cb
        .type   pp_timer_register_post_cb, @function
pp_timer_register_post_cb:
        entry    a1, 32
        l32r     a8, .Lsec_35_0
        s32i.n   a2, a8, 0
        movi.n   a2, 0
        memw
        retw.n
        .size   pp_timer_register_post_cb, .-pp_timer_register_post_cb
.Lsec_35_13:

        .section .text.pp_timer_process,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_36_0, .Lsec_40_0
.Lsec_36_4:
        .global pp_timer_process
        .type   pp_timer_process, @function
pp_timer_process:
        entry    a1, 32
        l32r     a8, .Lsec_36_0
        mov.n    a12, a3
        extui    a11, a2, 0, 8
        movi.n   a10, -1
        l32i.n   a8, a8, 0
        beqz.n   a8, .Lsec_36_1a
        movi.n   a10, 8
        callx8   a8
.Lsec_36_1a:
        mov.n    a2, a10
        retw.n
        .size   pp_timer_process, .-pp_timer_process
.Lsec_36_1e:

        .section .rodata_wlog_warning.5,"a",@progbits
        .align  1
.Lsec_37_0:
        .byte   0x6e, 0x6f, 0x20, 0x74, 0x69, 0x6d, 0x65, 0x72, 0x20, 0x68, 0x61, 0x6e, 0x64, 0x6c, 0x65, 0x0a
        .byte   0x00

        .section .rodata_wlog_warning.4,"a",@progbits
        .align  1
.Lsec_38_0:
        .byte   0x74, 0x69, 0x6d, 0x65, 0x72, 0x20, 0x65, 0x0a, 0x00

        .section .data.g_pp_timer_info,"aw",@progbits
        .align  4
.Lsec_39_0:
        .global g_pp_timer_info
        .type   g_pp_timer_info, @object
        .size   g_pp_timer_info, 120
g_pp_timer_info:
        .byte   0x00, 0x00, 0x00, 0x00
        .word   .Lsec_33_4
        .byte   0x01, 0x00, 0x00, 0x00
        .word   .Lsec_32_4
        .byte   0x02, 0x00, 0x00, 0x00
        .word   .Lsec_31_4
        .byte   0x03, 0x00, 0x00, 0x00
        .word   .Lsec_30_4
        .byte   0x04, 0x00, 0x00, 0x00
        .word   .Lsec_29_4
        .byte   0x05, 0x00, 0x00, 0x00
        .word   .Lsec_28_4
        .byte   0x06, 0x00, 0x00, 0x00
        .word   .Lsec_27_4
        .byte   0x07, 0x00, 0x00, 0x00
        .word   .Lsec_26_4
        .byte   0x08, 0x00, 0x00, 0x00
        .word   .Lsec_25_4
        .byte   0x09, 0x00, 0x00, 0x00
        .word   .Lsec_24_4
        .byte   0x0a, 0x00, 0x00, 0x00
        .word   .Lsec_23_4
        .byte   0x0b, 0x00, 0x00, 0x00
        .word   .Lsec_22_4
        .byte   0x0c, 0x00, 0x00, 0x00
        .word   .Lsec_20_4
        .byte   0x0d, 0x00, 0x00, 0x00
        .word   .Lsec_21_4
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00

        .section .bss.s_timer_post,"aw",@nobits
        .align  4
.Lsec_40_0:
        .skip   4
