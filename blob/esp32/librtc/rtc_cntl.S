.section .text.rtc_wifi_force_pd_off,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_7_0, 0x3ff48084
        .literal .Lsec_7_4, 0xfffdffff
        .literal .Lsec_7_8, 0x3ff48088
        .literal .Lsec_7_c, 0xefffffff
        .literal .Lsec_7_10, 0x3ff000d0
        .literal .Lsec_7_14, 0x3ff000cc
        .literal .Lsec_7_18, esp_dport_access_reg_read
        .literal .Lsec_7_1c, esp_dport_access_reg_read
        .literal .Lsec_7_20, esp_dport_access_reg_read
        .literal .Lsec_7_24, esp_dport_access_reg_read
.Lsec_7_28:
        .global rtc_wifi_force_pd_off
        .type   rtc_wifi_force_pd_off, @function
rtc_wifi_force_pd_off:
        entry    a1, 32
        l32r     a3, .Lsec_7_0
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_7_18
        callx8   a8
        l32r     a8, .Lsec_7_4
        and      a8, a10, a8
        l32r     a2, .Lsec_7_8
        mov.n    a10, a2
        memw
        s32i.n   a8, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_7_1c
        callx8   a8
        l32r     a8, .Lsec_7_c
        and      a8, a10, a8
        l32r     a3, .Lsec_7_10
        mov.n    a10, a3
        memw
        s32i.n   a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_7_20
        callx8   a8
        movi.n   a8, -2
        and      a8, a10, a8
        l32r     a2, .Lsec_7_14
        mov.n    a10, a2
        memw
        s32i     a8, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_7_24
        callx8   a8
        movi     a8, 0x40f
        or       a10, a10, a8
        memw
        s32i.n   a10, a2, 0
        retw.n
        .size   rtc_wifi_force_pd_off, .-rtc_wifi_force_pd_off
.Lsec_7_85:

        .section .text.rtc_digital_lp_mode_off_stg1,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_8_0, 0x3ff48000
        .literal .Lsec_8_4, 0xffdfffff
        .literal .Lsec_8_8, 0xfffbffff
        .literal .Lsec_8_c, 0xffffefff
        .literal .Lsec_8_10, 0x3ff48084
        .literal .Lsec_8_14, 0xffff555f
        .literal .Lsec_8_18, 0x3ff48088
        .literal .Lsec_8_1c, 0xfaaaffff
        .literal .Lsec_8_20, 0x3ff000c0
        .literal .Lsec_8_24, 0x3ff000cc
        .literal .Lsec_8_28, esp_dport_access_reg_read
        .literal .Lsec_8_2c, esp_dport_access_reg_read
        .literal .Lsec_8_30, esp_dport_access_reg_read
        .literal .Lsec_8_34, esp_dport_access_reg_read
        .literal .Lsec_8_38, esp_dport_access_reg_read
        .literal .Lsec_8_3c, rtc_wifi_force_pd_off
        .literal .Lsec_8_40, rtc_dbias_cfg
.Lsec_8_44:
        .global rtc_digital_lp_mode_off_stg1
        .type   rtc_digital_lp_mode_off_stg1, @function
rtc_digital_lp_mode_off_stg1:
        entry    a1, 32
        l32r     a2, .Lsec_8_0
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_8_28
        callx8   a8
        l32r     a8, .Lsec_8_4
        and      a8, a10, a8
        mov.n    a10, a2
        memw
        s32i.n   a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_8_2c
        callx8   a8
        l32r     a8, .Lsec_8_8
        and      a8, a10, a8
        mov.n    a10, a2
        memw
        s32i.n   a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_8_30
        callx8   a8
        l32r     a8, .Lsec_8_c
        and      a8, a10, a8
        l32r     a4, .Lsec_8_10
        or       a10, a4, a4
        memw
        s32i     a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_8_34
        callx8   a8
        l32r     a2, .Lsec_8_14
        and      a2, a10, a2
        l32r     a3, .Lsec_8_18
        mov.n    a10, a3
        memw
        s32i.n   a2, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_8_38
        callx8   a8
        movi.n   a8, -1
        l32r     a2, .Lsec_8_1c
        and      a10, a10, a2
        l32r     a2, .Lsec_8_20
        memw
        s32i.n   a10, a3, 0
        memw
        s32i.n   a8, a2, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a2, .Lsec_8_24
        memw
        s32i     a8, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_wifi_force_pd_off
        l32r     a8, .Lsec_8_3c
        callx8   a8
        movi.n   a13, 0
        mov.n    a12, a13
        mov.n    a11, a13
        movi.n   a10, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_dbias_cfg
        l32r     a8, .Lsec_8_40
        callx8   a8
        retw.n
        .size   rtc_digital_lp_mode_off_stg1, .-rtc_digital_lp_mode_off_stg1
.Lsec_8_e2:

        .section .text.rtc_digital_lp_mode_off_stg2,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_9_0, 0x3ff48000
        .literal .Lsec_9_4, esp_dport_access_reg_read
.Lsec_9_8:
        .global rtc_digital_lp_mode_off_stg2
        .type   rtc_digital_lp_mode_off_stg2, @function
rtc_digital_lp_mode_off_stg2:
        entry    a1, 32
        l32r     a2, .Lsec_9_0
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_9_4
        callx8   a8
        movi     a8, 0xfffffaff
        and      a10, a10, a8
        memw
        s32i.n   a10, a2, 0
        retw.n
        .size   rtc_digital_lp_mode_off_stg2, .-rtc_digital_lp_mode_off_stg2
.Lsec_9_23:

        .section .text.rtc_sar_sleep_timer_start,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_10_0, 0x3ff4882c
        .literal .Lsec_10_4, 0x3ff48818
        .literal .Lsec_10_8, esp_dport_access_reg_read
        .literal .Lsec_10_c, esp_dport_access_reg_read
        .literal .Lsec_10_10, esp_dport_access_reg_read
        .literal .Lsec_10_14, esp_dport_access_reg_read
.Lsec_10_18:
        .global rtc_sar_sleep_timer_start
        .type   rtc_sar_sleep_timer_start, @function
rtc_sar_sleep_timer_start:
        entry    a1, 32
        l32r     a3, .Lsec_10_0
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_10_8
        callx8   a8
        movi     a8, 0x100
        or       a10, a10, a8
        memw
        s32i.n   a10, a3, 0
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_10_c
        callx8   a8
        movi     a8, 0xfffffdff
        and      a10, a10, a8
        memw
        s32i.n   a10, a3, 0
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_10_10
        callx8   a8
        movi     a8, 0x200
        or       a8, a10, a8
        memw
        s32i.n   a8, a3, 0
        mov.n    a10, a3
        nop.n
        nop.n
        nop.n
        l32r     a8, .Lsec_10_4
        memw
        s32i.n   a2, a8, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_10_14
        callx8   a8
        movi     a8, 0xfffffeff
        and      a10, a10, a8
        memw
        s32i.n   a10, a3, 0
        retw.n
        .size   rtc_sar_sleep_timer_start, .-rtc_sar_sleep_timer_start
.Lsec_10_7a:
