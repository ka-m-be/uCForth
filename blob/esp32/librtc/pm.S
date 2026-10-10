.type   tsf_delay_us, @object
        .comm   tsf_delay_us, 4, 4
        .type   hardware_reject, @object
        .comm   hardware_reject, 1, 1
        .type   sleep_mode_flag, @object
        .comm   sleep_mode_flag, 4, 4

        .section .text.pm_wakeup_opt,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_19_0, 0x3ff48038
        .literal .Lsec_19_4, 0xffc007ff
        .literal .Lsec_19_8, 0x3ff48064
        .literal .Lsec_19_c, esp_dport_access_reg_read
.Lsec_19_10:
        .global pm_wakeup_opt
        .type   pm_wakeup_opt, @function
pm_wakeup_opt:
        entry    a1, 32
        l32r     a4, .Lsec_19_0
        extui    a2, a2, 0, 11
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_19_c
        callx8   a8
        slli     a2, a2, 11
        l32r     a8, .Lsec_19_4
        and      a10, a10, a8
        or       a10, a2, a10
        l32r     a2, .Lsec_19_8
        memw
        s32i.n   a10, a4, 0
        memw
        s32i.n   a3, a2, 0
        retw.n
        .size   pm_wakeup_opt, .-pm_wakeup_opt
.Lsec_19_3c:

        .section .text.get_chip_version,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_20_0, chip_version
.Lsec_20_4:
        .global get_chip_version
        .type   get_chip_version, @function
get_chip_version:
        entry    a1, 32
        l32r     a8, .Lsec_20_0
        l32i.n   a2, a8, 0
        retw.n
        .size   get_chip_version, .-get_chip_version
.Lsec_20_e:

        .section .text.pm_set_sleep_cycles,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_21_0, periodic_cal_sat
        .literal .Lsec_21_4, 0x00001388
        .literal .Lsec_21_8, rtc_time_get
        .literal .Lsec_21_c, rtc_sleep_set_wakeup_time
.Lsec_21_10:
        .global pm_set_sleep_cycles
        .type   pm_set_sleep_cycles, @function
pm_set_sleep_cycles:
        entry    a1, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_time_get
        l32r     a8, .Lsec_21_8
        callx8   a8
        add.n    a10, a2, a10
        movi.n   a8, 1
        bltu     a10, a2, .Lsec_21_22
        movi.n   a8, 0
.Lsec_21_22:
        add.n    a11, a8, a11
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_sleep_set_wakeup_time
        l32r     a8, .Lsec_21_c
        callx8   a8
        l32r     a9, .Lsec_21_4
        movi.n   a8, 1
        bltu     a9, a2, .Lsec_21_34
        movi.n   a8, 0
.Lsec_21_34:
        l32r     a2, .Lsec_21_0
        s8i      a8, a2, 0
        memw
        retw.n
        .size   pm_set_sleep_cycles, .-pm_set_sleep_cycles
.Lsec_21_3f:

        .section .text.pm_rtc_clock_cali,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_22_0, rtc_slowck_cali
.Lsec_22_4:
        .global pm_rtc_clock_cali
        .type   pm_rtc_clock_cali, @function
pm_rtc_clock_cali:
        entry    a1, 32
        movi     a11, 128
        movi.n   a10, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_slowck_cali
        l32r     a8, .Lsec_22_0
        callx8   a8
        beqz.n   a10, .Lsec_22_34
        l32i.n   a8, a2, 4
        beqz.n   a8, .Lsec_22_32
        sub      a9, a8, a10
        bgeu     a8, a10, .Lsec_22_21
        sub      a9, a10, a8
.Lsec_22_21:
        movi     a11, 0x1ff
        bltu     a11, a9, .Lsec_22_34
        addx2    a8, a8, a8
        addx4    a10, a10, a10
        add.n    a10, a8, a10
        srli     a10, a10, 3
.Lsec_22_32:
        s32i.n   a10, a2, 4
.Lsec_22_34:
        retw.n
        .size   pm_rtc_clock_cali, .-pm_rtc_clock_cali
.Lsec_22_36:

        .section .text.pm_prepare_to_sleep,"ax",@progbits
        .align  4
.Lsec_23_0:
        .global pm_prepare_to_sleep
        .type   pm_prepare_to_sleep, @function
pm_prepare_to_sleep:
        entry    a1, 32
        retw.n
        .size   pm_prepare_to_sleep, .-pm_prepare_to_sleep
.Lsec_23_5:

        .section .text.pm_sdio_nidle,"ax",@progbits
        .align  4
.Lsec_24_0:
        .global pm_sdio_nidle
        .type   pm_sdio_nidle, @function
pm_sdio_nidle:
        entry    a1, 32
        movi.n   a2, 0
        retw.n
        .size   pm_sdio_nidle, .-pm_sdio_nidle
.Lsec_24_7:

        .section .text.pm_goto_sleep,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_25_0, software_slp_reject
        .literal .Lsec_25_4, SDIO_slp_reject
        .literal .Lsec_25_8, 0x3ff48048
        .literal .Lsec_25_c, 0x3ff48018
        .literal .Lsec_25_10, 0x80000000
        .literal .Lsec_25_14, 0x3ff48040
        .literal .Lsec_25_18, esp_dport_access_reg_read
        .literal .Lsec_25_1c, esp_dport_access_reg_read
        .literal .Lsec_25_20, esp_dport_access_reg_read
.Lsec_25_24:
        .global pm_goto_sleep
        .type   pm_goto_sleep, @function
pm_goto_sleep:
        entry    a1, 32
        l32r     a3, .Lsec_25_0
        movi.n   a2, 0
        l32r     a4, .Lsec_25_8
        mov.n    a10, a4
        s8i      a2, a3, 0
        memw
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a3, .Lsec_25_4
        s8i      a2, a3, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_25_18
        callx8   a8
        movi.n   a2, 3
        or       a2, a10, a2
        l32r     a3, .Lsec_25_c
        or       a10, a3, a3
        memw
        s32i.n   a2, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_25_1c
        callx8   a8
        l32r     a2, .Lsec_25_10
        or       a10, a10, a2
        l32r     a2, .Lsec_25_14
        memw
        s32i.n   a10, a3, 0
.Lsec_25_72:
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_25_20
        callx8   a8
        extui    a10, a10, 0, 2
        beqz     a10, .Lsec_25_72
        movi.n   a2, 0
        retw.n
        .size   pm_goto_sleep, .-pm_goto_sleep
.Lsec_25_84:

        .section .text.pm_sleep_set_mac,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_26_0, 0x60033cb8
        .literal .Lsec_26_4, 0x00008000
        .literal .Lsec_26_8, 0x00006000
        .literal .Lsec_26_c, 0x05fe0000
        .literal .Lsec_26_10, esp_dport_access_reg_read
        .literal .Lsec_26_14, ets_delay_us
        .literal .Lsec_26_18, esp_dport_access_reg_read
        .literal .Lsec_26_1c, esp_dport_access_reg_read
        .literal .Lsec_26_20, ets_delay_us
        .literal .Lsec_26_24, esp_dport_access_reg_read
.Lsec_26_28:
        .global pm_sleep_set_mac
        .type   pm_sleep_set_mac, @function
pm_sleep_set_mac:
        entry    a1, 32
        l32r     a2, .Lsec_26_0
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_26_10
        callx8   a8
        l32r     a3, .Lsec_26_4
        or       a10, a10, a3
        memw
        s32i.n   a10, a2, 0
        nop.n
        nop.n
        nop.n
        nop.n
        l32r     a3, .Lsec_26_8
        j        .Lsec_26_59
        .byte    0x00
        .byte    0x00
.Lsec_26_51:
        movi.n   a10, 2
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_26_14
        callx8   a8
.Lsec_26_59:
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_26_18
        callx8   a8
        bany     a10, a3, .Lsec_26_51
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_26_1c
        callx8   a8
        l32r     a8, .Lsec_26_c
        or       a10, a10, a8
        memw
        s32i.n   a10, a2, 0
        j        .Lsec_26_82
.Lsec_26_7a:
        movi.n   a10, 2
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_26_20
        callx8   a8
.Lsec_26_82:
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_26_24
        callx8   a8
        bany     a10, a3, .Lsec_26_7a
        retw.n
        .size   pm_sleep_set_mac, .-pm_sleep_set_mac
.Lsec_26_8f:

        .section .text.pm_mac_deinit,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_27_0, pm_sleep_set_mac
.Lsec_27_4:
        .global pm_mac_deinit
        .type   pm_mac_deinit, @function
pm_mac_deinit:
        entry    a1, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_sleep_set_mac
        l32r     a8, .Lsec_27_0
        callx8   a8
        retw.n
        .size   pm_mac_deinit, .-pm_mac_deinit
.Lsec_27_f:

        .section .text.pm_set_wakeup_mac,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_28_0, 0x60033cb8
        .literal .Lsec_28_4, 0xfa017fff
        .literal .Lsec_28_8, esp_dport_access_reg_read
.Lsec_28_c:
        .global pm_set_wakeup_mac
        .type   pm_set_wakeup_mac, @function
pm_set_wakeup_mac:
        entry    a1, 32
        l32r     a2, .Lsec_28_0
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_28_8
        callx8   a8
        l32r     a8, .Lsec_28_4
        and      a10, a10, a8
        memw
        s32i.n   a10, a2, 0
        retw.n
        .size   pm_set_wakeup_mac, .-pm_set_wakeup_mac
.Lsec_28_27:

        .section .text.pm_check_mac_idle,"ax",@progbits
        .align  4
.Lsec_29_0:
        .global pm_check_mac_idle
        .type   pm_check_mac_idle, @function
pm_check_mac_idle:
        entry    a1, 32
        movi.n   a2, 0
        retw.n
        .size   pm_check_mac_idle, .-pm_check_mac_idle
.Lsec_29_7:

        .section .text.pm_set_sleep_btco,"ax",@progbits
        .align  4
.Lsec_30_0:
        .global pm_set_sleep_btco
        .type   pm_set_sleep_btco, @function
pm_set_sleep_btco:
        entry    a1, 32
        retw.n
        .size   pm_set_sleep_btco, .-pm_set_sleep_btco
.Lsec_30_5:

        .section .text.pm_set_wakeup_btco,"ax",@progbits
        .align  4
.Lsec_31_0:
        .global pm_set_wakeup_btco
        .type   pm_set_wakeup_btco, @function
pm_set_wakeup_btco:
        entry    a1, 32
        retw.n
        .size   pm_set_wakeup_btco, .-pm_set_wakeup_btco
.Lsec_31_5:

        .section .text.pm_unmask_bt,"ax",@progbits
        .align  4
.Lsec_32_0:
        .global pm_unmask_bt
        .type   pm_unmask_bt, @function
pm_unmask_bt:
        entry    a1, 32
        retw.n
        .size   pm_unmask_bt, .-pm_unmask_bt
.Lsec_32_5:

        .section .text.pm_mac_init,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_33_0, 0x60033cb8
        .literal .Lsec_33_4, 0xfa017fff
        .literal .Lsec_33_8, esp_dport_access_reg_read
.Lsec_33_c:
        .global pm_mac_init
        .type   pm_mac_init, @function
pm_mac_init:
        entry    a1, 32
        l32r     a2, .Lsec_33_0
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_33_8
        callx8   a8
        l32r     a8, .Lsec_33_4
        and      a10, a10, a8
        memw
        s32i.n   a10, a2, 0
        retw.n
        .size   pm_mac_init, .-pm_mac_init
.Lsec_33_27:

        .section .rodata.str1.4,"aMS",@progbits,1
        .align  4
.Lsec_34_0:
        .ascii  "rom_ram_pd_en:%d"
        .ascii  "\012lslp_mem_force_"
        .ascii  "pu:%d\012slowmem_fo"
        .ascii  "rce_pu:%d\012fastme"
        .ascii  "m_force_pu:%d\012\000"

        .section .text.pm_set_sleep_mode_full,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_35_0, 0x3ff48088
        .literal .Lsec_35_4, 0x10000000
        .literal .Lsec_35_8, 0x3ff48084
        .literal .Lsec_35_c, 0x00020000
        .literal .Lsec_35_10, 0x3ff48078
        .literal .Lsec_35_14, 0xfcffffff
        .literal .Lsec_35_18, 0x3ff48000
        .literal .Lsec_35_1c, 0xfffeffff
        .literal .Lsec_35_20, 0x3ff480b4
        .literal .Lsec_35_24, 0x91a2b3c5
        .literal .Lsec_35_28, 0x3ff48080
        .literal .Lsec_35_2c, 0x00002000
        .literal .Lsec_35_30, .Lsec_34_0
        .literal .Lsec_35_34, 0x0000c350
        .literal .Lsec_35_38, phy_close_rf
        .literal .Lsec_35_3c, pm_sleep_set_mac
        .literal .Lsec_35_40, esp_dport_access_reg_read
        .literal .Lsec_35_44, ets_delay_us
        .literal .Lsec_35_48, esp_dport_access_reg_read
        .literal .Lsec_35_4c, esp_dport_access_reg_read
        .literal .Lsec_35_50, rtc_slp_prep
        .literal .Lsec_35_54, esp_dport_access_reg_read
        .literal .Lsec_35_58, esp_dport_access_reg_read
        .literal .Lsec_35_5c, uart_div_modify
        .literal .Lsec_35_60, esp_dport_access_reg_read
        .literal .Lsec_35_64, esp_dport_access_reg_read
        .literal .Lsec_35_68, esp_dport_access_reg_read
        .literal .Lsec_35_6c, esp_dport_access_reg_read
        .literal .Lsec_35_70, rtc_printf
        .literal .Lsec_35_74, ets_delay_us
        .literal .Lsec_35_78, esp_dport_access_reg_read
        .literal .Lsec_35_7c, esp_dport_access_reg_read
        .literal .Lsec_35_80, rtc_slp_prep
        .literal .Lsec_35_84, esp_dport_access_reg_read
        .literal .Lsec_35_88, uart_div_modify
        .literal .Lsec_35_8c, esp_dport_access_reg_read
        .literal .Lsec_35_90, esp_dport_access_reg_read
        .literal .Lsec_35_94, esp_dport_access_reg_read
        .literal .Lsec_35_98, esp_dport_access_reg_read
        .literal .Lsec_35_9c, esp_dport_access_reg_read
        .literal .Lsec_35_a0, rtc_printf
        .literal .Lsec_35_a4, ets_delay_us
        .literal .Lsec_35_a8, esp_dport_access_reg_read
        .literal .Lsec_35_ac, rtc_slp_prep
        .literal .Lsec_35_b0, rtc_slp_prep
.Lsec_35_b4:
        .global pm_set_sleep_mode_full
        .type   pm_set_sleep_mode_full, @function
pm_set_sleep_mode_full:
        entry    a1, 80
        .reloc  ., R_XTENSA_ASM_EXPAND, phy_close_rf
        l32r     a8, .Lsec_35_38
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, pm_sleep_set_mac
        l32r     a8, .Lsec_35_3c
        callx8   a8
        bnei     a2, 64, .Lsec_35_c9
        j        .Lsec_35_338
.Lsec_35_c9:
        movi.n   a8, 64
        bltu     a8, a2, .Lsec_35_f0
        bnei     a2, 8, .Lsec_35_d4
        j        .Lsec_35_37c
.Lsec_35_d4:
        movi.n   a8, 8
        bltu     a8, a2, .Lsec_35_e4
        bnei     a2, 4, .Lsec_35_df
        j        .Lsec_35_34e
.Lsec_35_df:
        j        .Lsec_35_3ba
        .byte    0x00
        .byte    0x00
.Lsec_35_e4:
        bnei     a2, 16, .Lsec_35_ea
        j        .Lsec_35_390
.Lsec_35_ea:
        beqi     a2, 32, .Lsec_35_152
        j        .Lsec_35_3ba
.Lsec_35_f0:
        bnei     a2, 0x100, .Lsec_35_f6
        j        .Lsec_35_234
.Lsec_35_f6:
        movi     a8, 0x100
        bltu     a8, a2, .Lsec_35_104
        beqi     a2, 128, .Lsec_35_119
        j        .Lsec_35_3ba
        .byte    0x00
        .byte    0x00
.Lsec_35_104:
        movi     a8, 0x200
        bne      a2, a8, .Lsec_35_10d
        j        .Lsec_35_220
.Lsec_35_10d:
        movi     a3, 0x400
        bne      a2, a3, .Lsec_35_116
        j        .Lsec_35_2fc
.Lsec_35_116:
        j        .Lsec_35_3ba
.Lsec_35_119:
        l32r     a3, .Lsec_35_0
        or       a10, a3, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_35_40
        callx8   a8
        l32r     a2, .Lsec_35_4
        or       a2, a10, a2
        movi.n   a10, 5
        memw
        s32i     a2, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_35_44
        callx8   a8
        l32r     a3, .Lsec_35_8
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_35_48
        callx8   a8
        l32r     a2, .Lsec_35_c
        or       a10, a10, a2
        memw
        s32i.n   a10, a3, 0
        j        .Lsec_35_3ba
.Lsec_35_152:
        l32r     a3, .Lsec_35_10
        movi.n   a2, 0
        or       a10, a3, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_35_4c
        callx8   a8
        movi.n   a12, 1
        l32r     a9, .Lsec_35_14
        movi.n   a8, 4
        and      a10, a10, a9
        mov.n    a15, a2
        mov.n    a14, a2
        memw
        s32i.n   a10, a3, 0
        mov.n    a13, a2
        mov.n    a11, a12
        mov.n    a10, a2
        s32i.n   a8, a1, 36
        s32i.n   a8, a1, 32
        s32i.n   a8, a1, 28
        s32i.n   a8, a1, 24
        s32i.n   a2, a1, 20
        s32i.n   a2, a1, 16
        s32i.n   a2, a1, 12
        s32i.n   a2, a1, 8
        s32i     a2, a1, 4
        s32i     a2, a1, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_slp_prep
        l32r     a8, .Lsec_35_50
        callx8   a8
        l32r     a3, .Lsec_35_18
        or       a10, a3, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_35_54
        callx8   a8
        l32r     a8, .Lsec_35_1c
        and      a8, a10, a8
        l32r     a10, .Lsec_35_20
        memw
        s32i.n   a8, a3, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_35_58
        callx8   a8
        slli     a11, a10, 16
        mov.n    a10, a2
        l32r     a2, .Lsec_35_24
        muluh    a11, a11, a2
        extui    a11, a11, 16, 16
        .reloc  ., R_XTENSA_ASM_EXPAND, uart_div_modify
        l32r     a8, .Lsec_35_5c
        callx8   a8
        l32r     a2, .Lsec_35_8
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_35_60
        callx8   a8
        or       a3, a10, a10
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_35_64
        callx8   a8
        l32r     a2, .Lsec_35_28
        mov.n    a4, a10
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_35_68
        callx8   a8
        mov.n    a5, a10
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_35_6c
        callx8   a8
        l32r     a14, .Lsec_35_2c
        extui    a13, a5, 16, 1
        and      a14, a10, a14
        srli     a14, a14, 13
        extui    a12, a4, 4, 1
        l32r     a10, .Lsec_35_30
        extui    a11, a3, 24, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_printf
        l32r     a8, .Lsec_35_70
        callx8   a8
        l32r     a10, .Lsec_35_34
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_35_74
        callx8   a8
        j        .Lsec_35_3ba
.Lsec_35_220:
        l32r     a3, .Lsec_35_10
        movi.n   a2, 0
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_35_78
        callx8   a8
        movi.n   a8, 4
        j        .Lsec_35_30b
        .byte    0x00
        .byte    0x00
.Lsec_35_234:
        l32r     a3, .Lsec_35_10
        movi.n   a2, 0
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_35_7c
        callx8   a8
        movi.n   a8, 7
        l32r     a9, .Lsec_35_14
        movi.n   a12, 1
        and      a10, a10, a9
        mov.n    a14, a2
        mov.n    a13, a2
        memw
        s32i.n   a10, a3, 0
        mov.n    a15, a2
        mov.n    a10, a2
        mov.n    a11, a12
        s32i.n   a8, a1, 36
        s32i.n   a8, a1, 32
        s32i.n   a8, a1, 28
        s32i.n   a8, a1, 24
        s32i.n   a2, a1, 20
        s32i.n   a2, a1, 16
        s32i.n   a2, a1, 12
        s32i.n   a2, a1, 8
.Lsec_35_26a:
        s32i.n   a2, a1, 4
        s32i.n   a2, a1, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_slp_prep
        l32r     a8, .Lsec_35_80
        callx8   a8
        l32r     a10, .Lsec_35_20
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_35_84
        callx8   a8
        slli     a11, a10, 16
        mov.n    a10, a2
        l32r     a2, .Lsec_35_24
        muluh    a11, a11, a2
        extui    a11, a11, 16, 16
        .reloc  ., R_XTENSA_ASM_EXPAND, uart_div_modify
        l32r     a8, .Lsec_35_88
        callx8   a8
        l32r     a4, .Lsec_35_18
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_35_8c
        callx8   a8
        l32r     a2, .Lsec_35_1c
        and      a2, a10, a2
        l32r     a3, .Lsec_35_8
        mov.n    a10, a3
        memw
        s32i.n   a2, a4, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_35_90
        callx8   a8
        mov.n    a4, a10
        or       a10, a3, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_35_94
        callx8   a8
        l32r     a2, .Lsec_35_28
        mov.n    a3, a10
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_35_98
        callx8   a8
        mov.n    a5, a10
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_35_9c
        callx8   a8
        l32r     a14, .Lsec_35_2c
        extui    a13, a5, 16, 1
        and      a14, a10, a14
        srli     a14, a14, 13
        extui    a12, a3, 4, 1
        l32r     a10, .Lsec_35_30
        extui    a11, a4, 24, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_printf
        l32r     a8, .Lsec_35_a0
        callx8   a8
        l32r     a10, .Lsec_35_34
        .reloc  ., R_XTENSA_ASM_EXPAND, ets_delay_us
        l32r     a8, .Lsec_35_a4
        callx8   a8
        j        .Lsec_35_3ba
.Lsec_35_2fc:
        l32r     a3, .Lsec_35_10
        movi.n   a2, 0
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, esp_dport_access_reg_read
        l32r     a8, .Lsec_35_a8
        callx8   a8
        movi.n   a8, 7
.Lsec_35_30b:
        l32r     a9, .Lsec_35_14
        mov.n    a14, a2
        and      a10, a10, a9
        mov.n    a13, a2
        mov.n    a12, a2
        memw
        s32i.n   a10, a3, 0
        movi.n   a3, 1
        mov.n    a11, a2
        mov.n    a10, a2
        mov.n    a15, a2
        s32i.n   a8, a1, 36
        s32i.n   a8, a1, 32
        s32i.n   a8, a1, 28
        s32i.n   a8, a1, 24
        s32i.n   a2, a1, 20
        s32i.n   a2, a1, 16
        s32i.n   a2, a1, 12
        s32i.n   a3, a1, 8
        j        .Lsec_35_26a
        .byte    0x00
.Lsec_35_338:
        movi.n   a2, 0
        movi.n   a12, 1
        s32i.n   a2, a1, 36
        mov.n    a15, a2
        mov.n    a14, a2
        mov.n    a13, a2
        mov.n    a11, a12
        s32i.n   a2, a1, 32
        j        .Lsec_35_3a0
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_35_34e:
        movi.n   a8, 0
        s32i.n   a2, a1, 24
        movi.n   a2, 1
        mov.n    a15, a8
        mov.n    a14, a8
        mov.n    a13, a8
        s32i.n   a8, a1, 36
        mov.n    a12, a8
        mov.n    a11, a8
        mov.n    a10, a8
        s32i.n   a8, a1, 32
        s32i.n   a8, a1, 28
        s32i.n   a8, a1, 20
        s32i.n   a8, a1, 16
        s32i.n   a8, a1, 12
        s32i.n   a8, a1, 8
        s32i.n   a2, a1, 4
        s32i.n   a8, a1, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_slp_prep
        l32r     a8, .Lsec_35_ac
        callx8   a8
        j        .Lsec_35_3ba
        .byte    0x00
.Lsec_35_37c:
        movi.n   a2, 0
        s32i.n   a2, a1, 36
        mov.n    a15, a2
        mov.n    a14, a2
        mov.n    a13, a2
        mov.n    a12, a2
        s32i.n   a2, a1, 32
        mov.n    a11, a2
        j        .Lsec_35_3a0
        .byte    0x00
.Lsec_35_390:
        movi.n   a2, 0
        s32i.n   a2, a1, 36
        mov.n    a15, a2
        mov.n    a14, a2
        mov.n    a13, a2
        mov.n    a12, a2
        s32i.n   a2, a1, 32
        movi.n   a11, 1
.Lsec_35_3a0:
        movi.n   a3, 4
        mov.n    a10, a2
        s32i.n   a2, a1, 28
        s32i.n   a3, a1, 24
        s32i.n   a2, a1, 20
        s32i.n   a2, a1, 16
        s32i.n   a2, a1, 12
        s32i.n   a2, a1, 8
        s32i.n   a2, a1, 4
        s32i.n   a2, a1, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, rtc_slp_prep
        l32r     a8, .Lsec_35_b0
        callx8   a8
.Lsec_35_3ba:
        movi.n   a2, 0
        retw.n
        .size   pm_set_sleep_mode_full, .-pm_set_sleep_mode_full
.Lsec_35_3be:

        .section .bss.wakeup_tsfearly_fix,"aw",@nobits
        .align  4
.Lsec_36_0:
        .global wakeup_tsfearly_fix
        .type   wakeup_tsfearly_fix, @object
        .size   wakeup_tsfearly_fix, 4
wakeup_tsfearly_fix:
        .skip   4

        .section .data.chip_version,"aw",@progbits
        .align  4
.Lsec_37_0:
        .global chip_version
        .type   chip_version, @object
        .size   chip_version, 4
chip_version:
        .byte   0xff, 0xff, 0xff, 0xff

        .section .bss.periodic_cal_sat,"aw",@nobits
        .align  1
.Lsec_38_0:
        .global periodic_cal_sat
        .type   periodic_cal_sat, @object
        .size   periodic_cal_sat, 1
periodic_cal_sat:
        .skip   1

        .section .bss.software_slp_reject,"aw",@nobits
        .align  1
.Lsec_39_0:
        .global software_slp_reject
        .type   software_slp_reject, @object
        .size   software_slp_reject, 1
software_slp_reject:
        .skip   1

        .section .bss.SDIO_slp_reject,"aw",@nobits
        .align  1
.Lsec_40_0:
        .global SDIO_slp_reject
        .type   SDIO_slp_reject, @object
        .size   SDIO_slp_reject, 1
SDIO_slp_reject:
        .skip   1

        .section .bss.pptask_busy,"aw",@nobits
        .align  1
.Lsec_41_0:
        .global pptask_busy
        .type   pptask_busy, @object
        .size   pptask_busy, 1
pptask_busy:
        .skip   1
