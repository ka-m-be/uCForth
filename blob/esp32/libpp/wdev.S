.section .text.wDev_SetCurChannel,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_61_0, wDevCtrl
.Lsec_61_4:
        .global wDev_SetCurChannel
        .type   wDev_SetCurChannel, @function
wDev_SetCurChannel:
        entry    a1, 32
        l8ui     a9, a2, 0
        l32r     a8, .Lsec_61_0
        s8i      a9, a8, 44
        memw
        l8ui     a2, a2, 1
        s8i      a2, a8, 45
        memw
        memw
        retw.n
        .size   wDev_SetCurChannel, .-wDev_SetCurChannel
.Lsec_61_21:

        .section .text.wDev_SetAuthed,"ax",@progbits
        .align  4
.Lsec_62_0:
        .global wDev_SetAuthed
        .type   wDev_SetAuthed, @function
wDev_SetAuthed:
        entry    a1, 32
        retw.n
        .size   wDev_SetAuthed, .-wDev_SetAuthed
.Lsec_62_5:

        .section .text.wDev_Set_Beacon_Int,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_63_0, BcnInterval
.Lsec_63_4:
        .global wDev_Set_Beacon_Int
        .type   wDev_Set_Beacon_Int, @function
wDev_Set_Beacon_Int:
        entry    a1, 32
        l32r     a8, .Lsec_63_0
        s32i.n   a2, a8, 0
        memw
        retw.n
        .size   wDev_Set_Beacon_Int, .-wDev_Set_Beacon_Int
.Lsec_63_11:

        .section .text.wDev_Reset_TBTT,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_64_0, hal_mac_tsf_reset
.Lsec_64_4:
        .global wDev_Reset_TBTT
        .type   wDev_Reset_TBTT, @function
wDev_Reset_TBTT:
        entry    a1, 32
        movi     a10, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_tsf_reset
        l32r     a8, .Lsec_64_0
        callx8   a8
        retw.n
        .size   wDev_Reset_TBTT, .-wDev_Reset_TBTT
.Lsec_64_12:

        .section .text.wDev_reset_bcnSendTick,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_65_0, BcnSendTick
.Lsec_65_4:
        .global wDev_reset_bcnSendTick
        .type   wDev_reset_bcnSendTick, @function
wDev_reset_bcnSendTick:
        entry    a1, 32
        l32r     a8, .Lsec_65_0
        movi.n   a9, 0
        s32i.n   a9, a8, 0
        memw
        retw.n
        .size   wDev_reset_bcnSendTick, .-wDev_reset_bcnSendTick
.Lsec_65_13:

        .section .text.wDev_Mesh_Enable_Tsf,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_66_0, hal_mac_tsf_reset
.Lsec_66_4:
        .global wDev_Mesh_Enable_Tsf
        .type   wDev_Mesh_Enable_Tsf, @function
wDev_Mesh_Enable_Tsf:
        entry    a1, 32
        movi     a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_tsf_reset
        l32r     a8, .Lsec_66_0
        callx8   a8
        retw.n
        .size   wDev_Mesh_Enable_Tsf, .-wDev_Mesh_Enable_Tsf
.Lsec_66_12:

        .section .text.wDev_Mesh_Set_TBTT,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_67_0, hal_mac_tsf_set_time
.Lsec_67_4:
        .global wDev_Mesh_Set_TBTT
        .type   wDev_Mesh_Set_TBTT, @function
wDev_Mesh_Set_TBTT:
        entry    a1, 32
        mov.n    a12, a2
        mov.n    a13, a3
        movi     a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_tsf_set_time
        l32r     a8, .Lsec_67_0
        callx8   a8
        retw.n
        .size   wDev_Mesh_Set_TBTT, .-wDev_Mesh_Set_TBTT
.Lsec_67_16:

        .section .text.wDev_Get_Next_TBTT,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_68_0, BcnSendTick
        .literal .Lsec_68_4, BcnInterval
        .literal .Lsec_68_8, 0x10624dd3
        .literal .Lsec_68_c, hal_get_tsf_time
.Lsec_68_10:
        .global wDev_Get_Next_TBTT
        .type   wDev_Get_Next_TBTT, @function
wDev_Get_Next_TBTT:
        entry    a1, 32
        l32r     a4, .Lsec_68_0
.Lsec_68_16:
        movi.n   a10, 1
        l32r     a3, .Lsec_68_4
        l32i.n   a2, a4, 0
        l32i.n   a8, a3, 0
        add.n    a2, a2, a8
        s32i.n   a2, a4, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_get_tsf_time
        l32r     a8, .Lsec_68_c
        callx8   a8
        sub      a10, a2, a10
        l32i.n   a2, a3, 0
        bltu     a2, a10, .Lsec_68_16
        l32r     a2, .Lsec_68_8
        muluh    a2, a10, a2
        srli     a2, a2, 6
        addi.n   a2, a2, 1
        retw.n
        .size   wDev_Get_Next_TBTT, .-wDev_Get_Next_TBTT
.Lsec_68_41:

        .section .wifi0iram.3,"ax",@progbits
        .align  4
.Lsec_69_0:
        .global wdev_bank_store
        .type   wdev_bank_store, @function
wdev_bank_store:
        entry    a1, 32
        mov.n    a8, a3
        j        .Lsec_69_16
.Lsec_69_8:
        memw
        l32i.n   a10, a8, 0
        sub      a9, a8, a3
        add.n    a9, a2, a9
        addi.n   a8, a8, 4
        s32i.n   a10, a9, 0
.Lsec_69_16:
        addi.n   a4, a4, -1
        bnei     a4, -1, .Lsec_69_8
        memw
        retw.n
        .size   wdev_bank_store, .-wdev_bank_store
.Lsec_69_20:

        .section .wifi0iram.4,"ax",@progbits
        .align  4
.Lsec_70_0:
        .global wdev_bank_load
        .type   wdev_bank_load, @function
wdev_bank_load:
        entry    a1, 32
        sub      a2, a2, a3
        j        .Lsec_70_12
.Lsec_70_9:
        l32i.n   a9, a3, 0
        addi.n   a3, a3, 4
        memw
        s32i.n   a9, a8, 0
.Lsec_70_12:
        addi.n   a4, a4, -1
        add.n    a8, a3, a2
        bnei     a4, -1, .Lsec_70_9
        memw
        retw.n
        .size   wdev_bank_load, .-wdev_bank_load
.Lsec_70_1e:

        .section .wifi0iram.5,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_71_0, 0x3ff73000
        .literal .Lsec_71_4, 0x3ff73400
        .literal .Lsec_71_8, 0x3ff73800
        .literal .Lsec_71_c, 0x3ff73c00
        .literal .Lsec_71_10, 0x3ff74000
        .literal .Lsec_71_14, 0x3ff74400
        .literal .Lsec_71_18, wdev_bank_load
        .literal .Lsec_71_1c, wdev_bank_load
        .literal .Lsec_71_20, wdev_bank_load
        .literal .Lsec_71_24, wdev_bank_load
        .literal .Lsec_71_28, wdev_bank_load
        .literal .Lsec_71_2c, wdev_bank_load
.Lsec_71_30:
        .global wdev_mac_reg_load
        .type   wdev_mac_reg_load, @function
wdev_mac_reg_load:
        entry    a1, 32
        l32r     a10, .Lsec_71_0
        mov.n    a11, a2
        movi     a12, 188
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_bank_load
        l32r     a8, .Lsec_71_18
        callx8   a8
        movi     a11, 0x2f0
        l32r     a10, .Lsec_71_4
        add.n    a11, a2, a11
        movi.n   a12, 16
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_bank_load
        l32r     a8, .Lsec_71_1c
        callx8   a8
        movi     a11, 0x330
        l32r     a10, .Lsec_71_8
        add.n    a11, a2, a11
        movi.n   a12, 17
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_bank_load
        l32r     a8, .Lsec_71_20
        callx8   a8
        movi     a11, 0x374
        l32r     a10, .Lsec_71_c
        add.n    a11, a2, a11
        movi     a12, 120
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_bank_load
        l32r     a8, .Lsec_71_24
        callx8   a8
        movi     a11, 0x554
        l32r     a10, .Lsec_71_10
        add      a11, a2, a11
        movi     a12, 210
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_bank_load
        l32r     a8, .Lsec_71_28
        callx8   a8
        movi     a11, 0x227
        l32r     a10, .Lsec_71_14
        movi     a12, 250
        addx4    a11, a11, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_bank_load
        l32r     a8, .Lsec_71_2c
        callx8   a8
        retw.n
        .size   wdev_mac_reg_load, .-wdev_mac_reg_load
.Lsec_71_98:

        .section .wifi0iram.6,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_72_0, 0x3ff73000
        .literal .Lsec_72_4, 0x3ff73400
        .literal .Lsec_72_8, 0x3ff73800
        .literal .Lsec_72_c, 0x3ff73c00
        .literal .Lsec_72_10, 0x3ff74000
        .literal .Lsec_72_14, 0x3ff74400
        .literal .Lsec_72_18, wdev_bank_store
        .literal .Lsec_72_1c, wdev_bank_store
        .literal .Lsec_72_20, wdev_bank_store
        .literal .Lsec_72_24, wdev_bank_store
        .literal .Lsec_72_28, wdev_bank_store
        .literal .Lsec_72_2c, wdev_bank_store
.Lsec_72_30:
        .global wdev_mac_reg_store
        .type   wdev_mac_reg_store, @function
wdev_mac_reg_store:
        entry    a1, 32
        l32r     a11, .Lsec_72_0
        mov.n    a10, a2
        movi     a12, 188
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_bank_store
        l32r     a8, .Lsec_72_18
        callx8   a8
        movi     a10, 0x2f0
        l32r     a11, .Lsec_72_4
        add.n    a10, a2, a10
        movi.n   a12, 16
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_bank_store
        l32r     a8, .Lsec_72_1c
        callx8   a8
        movi     a10, 0x330
        l32r     a11, .Lsec_72_8
        add.n    a10, a2, a10
        movi.n   a12, 17
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_bank_store
        l32r     a8, .Lsec_72_20
        callx8   a8
        movi     a10, 0x374
        l32r     a11, .Lsec_72_c
        add.n    a10, a2, a10
        movi     a12, 120
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_bank_store
        l32r     a8, .Lsec_72_24
        callx8   a8
        movi     a10, 0x554
        l32r     a11, .Lsec_72_10
        add      a10, a2, a10
        movi     a12, 210
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_bank_store
        l32r     a8, .Lsec_72_28
        callx8   a8
        movi     a10, 0x227
        l32r     a11, .Lsec_72_14
        movi     a12, 250
        addx4    a10, a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_bank_store
        l32r     a8, .Lsec_72_2c
        callx8   a8
        retw.n
        .size   wdev_mac_reg_store, .-wdev_mac_reg_store
.Lsec_72_98:

        .section .wifi0iram.7,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_73_0, wDevMacSleep+0x50
        .literal .Lsec_73_4, wDevMacSleep+0x30
        .literal .Lsec_73_8, wDevMacSleep+0x10
        .literal .Lsec_73_c, hal_ba_session_restore
.Lsec_73_10:
        .global wdev_mac_special_reg_load
        .type   wdev_mac_special_reg_load, @function
wdev_mac_special_reg_load:
        entry    a1, 32
        l32r     a12, .Lsec_73_0
        l32r     a11, .Lsec_73_4
        l32r     a10, .Lsec_73_8
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_ba_session_restore
        l32r     a8, .Lsec_73_c
        callx8   a8
        retw.n
        .size   wdev_mac_special_reg_load, .-wdev_mac_special_reg_load
.Lsec_73_24:

        .section .wifi0iram.8,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_74_0, wDevMacSleep+0x50
        .literal .Lsec_74_4, wDevMacSleep+0x30
        .literal .Lsec_74_8, wDevMacSleep+0x10
        .literal .Lsec_74_c, hal_ba_session_store
.Lsec_74_10:
        .global wdev_mac_special_reg_store
        .type   wdev_mac_special_reg_store, @function
wdev_mac_special_reg_store:
        entry    a1, 32
        l32r     a12, .Lsec_74_0
        l32r     a11, .Lsec_74_4
        l32r     a10, .Lsec_74_8
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_ba_session_store
        l32r     a8, .Lsec_74_c
        callx8   a8
        retw.n
        .size   wdev_mac_special_reg_store, .-wdev_mac_special_reg_store
.Lsec_74_24:

        .section .text.wdev_mac_sleep_init,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_75_0, wDevMacSleep
        .literal .Lsec_75_4, g_osi_funcs_p
        .literal .Lsec_75_8, .Lsec_159_0
        .literal .Lsec_75_c, .Lsec_158_0
        .literal .Lsec_75_10, wifi_log
        .literal .Lsec_75_14, wifi_log
.Lsec_75_18:
        .global wdev_mac_sleep_init
        .type   wdev_mac_sleep_init, @function
wdev_mac_sleep_init:
        entry    a1, 32
        l32r     a2, .Lsec_75_0
        l32i.n   a8, a2, 4
        bnez.n   a8, .Lsec_75_60
        movi     a10, 0x321
        slli     a10, a10, 2
        l32r     a8, .Lsec_75_4
        l32i     a8, a8, 0
        l32i     a8, a8, 0x15c
        callx8   a8
        movi.n   a11, 1
        slli     a11, a11, 12
        s32i.n   a10, a2, 4
        bnez.n   a10, .Lsec_75_51
        movi.n   a12, 2
        movi.n   a10, 6
        l32r     a13, .Lsec_75_8
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_75_10
        callx8   a8
        j        .Lsec_75_60
        .byte    0x00
.Lsec_75_51:
        l32r     a13, .Lsec_75_c
        mov.n    a14, a10
        movi.n   a12, 3
        movi.n   a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_75_14
        callx8   a8
.Lsec_75_60:
        retw.n
        .size   wdev_mac_sleep_init, .-wdev_mac_sleep_init
.Lsec_75_62:

        .section .text.wdev_mac_sleep_deinit,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_76_0, wDevMacSleep
        .literal .Lsec_76_4, g_osi_funcs_p
.Lsec_76_8:
        .global wdev_mac_sleep_deinit
        .type   wdev_mac_sleep_deinit, @function
wdev_mac_sleep_deinit:
        entry    a1, 32
        l32r     a2, .Lsec_76_0
        l32i.n   a10, a2, 4
        beqz.n   a10, .Lsec_76_21
        l32r     a8, .Lsec_76_4
        l32i.n   a8, a8, 0
        l32i     a8, a8, 176
        callx8   a8
        movi.n   a8, 0
        s32i.n   a8, a2, 4
.Lsec_76_21:
        memw
        retw.n
        .size   wdev_mac_sleep_deinit, .-wdev_mac_sleep_deinit
.Lsec_76_26:

        .section .wifi0iram.11,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_77_0, wDevMacSleep
        .literal .Lsec_77_4, g_osi_funcs_p
        .literal .Lsec_77_8, 0x3ff73c04
        .literal .Lsec_77_c, 0x3ff73084
        .literal .Lsec_77_10, 0x7fffffff
        .literal .Lsec_77_14, wdev_mac_reg_load
        .literal .Lsec_77_18, hal_enable_mac
        .literal .Lsec_77_1c, wdev_mac_special_reg_load
.Lsec_77_20:
        .global wdev_mac_wakeup
        .type   wdev_mac_wakeup, @function
wdev_mac_wakeup:
        entry    a1, 32
        l32r     a2, .Lsec_77_0
        l8ui     a3, a2, 0
        beqz.n   a3, .Lsec_77_39
        movi.n   a3, 0
        s8i      a3, a2, 0
        memw
        memw
        j        .Lsec_77_ac
.Lsec_77_39:
        l8ui     a8, a2, 1
        beqz     a8, .Lsec_77_ac
        s8i      a3, a2, 1
        memw
        l32i.n   a3, a2, 4
        beqz     a3, .Lsec_77_ac
        l32r     a3, .Lsec_77_4
        l32i.n   a3, a3, 0
        l32i     a3, a3, 0x10c
        memw
        callx8   a3
        movi     a9, 240
        l32r     a8, .Lsec_77_8
        memw
        l32i.n   a3, a8, 0
        or       a3, a3, a9
        memw
        s32i.n   a3, a8, 0
        l32r     a3, .Lsec_77_c
        l32r     a9, .Lsec_77_10
        memw
        l32i.n   a8, a3, 0
        and      a8, a8, a9
        l32i.n   a10, a2, 4
        memw
        s32i     a8, a3, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_mac_reg_load
        l32r     a8, .Lsec_77_14
        callx8   a8
        l32i.n   a11, a2, 12
        l32i.n   a10, a2, 8
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_enable_mac
        l32r     a8, .Lsec_77_18
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_mac_special_reg_load
        l32r     a8, .Lsec_77_1c
        callx8   a8
        movi.n   a8, -1
        memw
        l32i.n   a2, a3, 0
        slli     a8, a8, 31
        or       a2, a2, a8
        memw
        s32i.n   a2, a3, 0
.Lsec_77_ac:
        memw
        retw.n
        .size   wdev_mac_wakeup, .-wdev_mac_wakeup
.Lsec_77_b1:

        .section .wifi0iram.12,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_78_0, wDevCtrl
        .literal .Lsec_78_4, hal_mac_rx_get_last_dscr
.Lsec_78_8:
        .global wdev_is_data_in_rxlist
        .type   wdev_is_data_in_rxlist, @function
wdev_is_data_in_rxlist:
        entry    a1, 32
        l32r     a2, .Lsec_78_0
        l32i.n   a3, a2, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_rx_get_last_dscr
        l32r     a8, .Lsec_78_4
        callx8   a8
        j        .Lsec_78_2b
.Lsec_78_19:
        memw
        l32i.n   a2, a3, 0
        extui    a2, a2, 30, 1
        l32i.n   a8, a3, 8
        beqi     a2, 1, .Lsec_78_30
        beq      a3, a10, .Lsec_78_30
        mov.n    a3, a8
.Lsec_78_2b:
        bnez     a3, .Lsec_78_19
        mov.n    a2, a3
.Lsec_78_30:
        retw.n
        .size   wdev_is_data_in_rxlist, .-wdev_is_data_in_rxlist
.Lsec_78_32:

        .section .wifi0iram.13,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_79_0, wDevMacSleep
        .literal .Lsec_79_4, 0x3ff73084
        .literal .Lsec_79_8, 0x7fffffff
        .literal .Lsec_79_c, wDevCtrl
        .literal .Lsec_79_10, g_osi_funcs_p
        .literal .Lsec_79_14, wdev_is_data_in_rxlist
        .literal .Lsec_79_18, wdev_mac_special_reg_store
        .literal .Lsec_79_1c, hal_disable_mac
        .literal .Lsec_79_20, wdev_mac_reg_store
.Lsec_79_24:
        .global wdev_mac_sleep
        .type   wdev_mac_sleep, @function
wdev_mac_sleep:
        entry    a1, 32
        l32r     a2, .Lsec_79_0
        l32i.n   a3, a2, 4
        beqz     a3, .Lsec_79_a5
        l32r     a3, .Lsec_79_4
        l32r     a9, .Lsec_79_8
        memw
        l32i.n   a8, a3, 0
        and      a8, a8, a9
        memw
        s32i     a8, a3, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_is_data_in_rxlist
        l32r     a8, .Lsec_79_14
        callx8   a8
        s8i      a10, a2, 0
        memw
        beqz.n   a10, .Lsec_79_6c
        movi.n   a8, -1
        slli     a8, a8, 31
        memw
        l32i.n   a2, a3, 0
        or       a2, a2, a8
        memw
        s32i.n   a2, a3, 0
        memw
        j        .Lsec_79_a5
.Lsec_79_6c:
        movi.n   a3, 1
        s8i      a3, a2, 1
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_mac_special_reg_store
        l32r     a8, .Lsec_79_18
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_disable_mac
        l32r     a8, .Lsec_79_1c
        callx8   a8
        s32i.n   a10, a2, 8
        l32i.n   a10, a2, 4
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_mac_reg_store
        l32r     a8, .Lsec_79_20
        callx8   a8
        l32r     a3, .Lsec_79_c
        l32i.n   a3, a3, 0
        s32i.n   a3, a2, 12
        l32r     a2, .Lsec_79_10
        l32i.n   a2, a2, 0
        l32i     a2, a2, 0x108
        memw
        callx8   a2
.Lsec_79_a5:
        retw.n
        .size   wdev_mac_sleep, .-wdev_mac_sleep
.Lsec_79_a7:

        .section .wifislpiram.14,"ax",@progbits
        .align  4
.Lsec_80_0:
        .global wdev_csi_len_align
        .type   wdev_csi_len_align, @function
wdev_csi_len_align:
        entry    a1, 32
        l32i.n   a8, a2, 20
        movi     a2, 0x3ff
        slli     a2, a2, 8
        and      a2, a8, a2
        beqz.n   a2, .Lsec_80_1c
        extui    a2, a8, 8, 10
        addi     a2, a2, 3
        srai     a2, a2, 2
        slli     a2, a2, 2
.Lsec_80_1c:
        retw.n
        .size   wdev_csi_len_align, .-wdev_csi_len_align
.Lsec_80_1e:

        .section .text.wdev_csi_hw_bug_check,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_81_0, .Lsec_157_0
        .literal .Lsec_81_4, wDevCtrl
        .literal .Lsec_81_8, wifi_log
.Lsec_81_c:
        .global wdev_csi_hw_bug_check
        .type   wdev_csi_hw_bug_check, @function
wdev_csi_hw_bug_check:
        entry    a1, 32
        l32i.n   a9, a2, 0
        extui    a8, a9, 0, 12
        extui    a9, a9, 12, 12
        l32i.n   a2, a3, 0
        extui    a10, a2, 0, 12
        bne      a8, a9, .Lsec_81_21
        bnez.n   a8, .Lsec_81_29
.Lsec_81_21:
        addi.n   a9, a9, 8
        bne      a9, a8, .Lsec_81_38
        j        .Lsec_81_6b
.Lsec_81_29:
        sub      a8, a8, a10
        movi.n   a9, 1
        movi.n   a2, 2
        moveqz   a2, a9, a8
        j        .Lsec_81_75
        .byte    0x00
        .byte    0x00
.Lsec_81_38:
        extui    a8, a2, 12, 12
        movi.n   a2, 0
        beq      a10, a8, .Lsec_81_75
        addi.n   a8, a8, 8
        beq      a8, a10, .Lsec_81_75
        l32r     a13, .Lsec_81_0
        movi.n   a12, 2
        movi.n   a11, 32
        movi.n   a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_81_8
        callx8   a8
        l32r     a2, .Lsec_81_4
        l32i.n   a2, a2, 0
        j        .Lsec_81_5e
.Lsec_81_5c:
        l32i.n   a2, a2, 8
.Lsec_81_5e:
        memw
        l32i.n   a8, a2, 0
        bbci     a8, 30, .Lsec_81_5c
        movi.n   a2, 3
        j        .Lsec_81_75
.Lsec_81_6b:
        sub      a8, a8, a10
        movi.n   a2, 1
        movi.n   a9, 2
        movnez   a2, a9, a8
.Lsec_81_75:
        retw.n
        .size   wdev_csi_hw_bug_check, .-wdev_csi_hw_bug_check
.Lsec_81_77:

        .section .text.wdev_set_promis_misc_pkt,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_82_0, wDevCtrl
        .literal .Lsec_82_4, 0x3ff730f4
        .literal .Lsec_82_8, 0xffff00ff
        .literal .Lsec_82_c, hal_sniffer_rx_set_promis
        .literal .Lsec_82_10, hal_sniffer_rx_set_promis
.Lsec_82_14:
        .global wdev_set_promis_misc_pkt
        .type   wdev_set_promis_misc_pkt, @function
wdev_set_promis_misc_pkt:
        entry    a1, 32
        extui    a2, a2, 0, 8
        l32r     a3, .Lsec_82_0
        beqz.n   a2, .Lsec_82_5c
        l8ui     a8, a3, 48
        beqz.n   a8, .Lsec_82_5c
        l32i.n   a8, a3, 52
        bbci     a8, 3, .Lsec_82_5c
        movi.n   a12, 0
        movi.n   a11, 1
        mov.n    a10, a12
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_sniffer_rx_set_promis
        l32r     a8, .Lsec_82_c
        callx8   a8
        l32i.n   a3, a3, 52
        bbci     a3, 6, .Lsec_82_80
        l32r     a9, .Lsec_82_4
        l32r     a3, .Lsec_82_8
        memw
        l32i.n   a8, a9, 0
        and      a8, a8, a3
        movi.n   a3, 65
        slli     a3, a3, 8
        or       a8, a8, a3
        memw
        s32i.n   a8, a9, 0
        memw
        j        .Lsec_82_80
        .byte    0x00
.Lsec_82_5c:
        movi.n   a12, 0
        mov.n    a11, a12
        mov.n    a10, a12
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_sniffer_rx_set_promis
        l32r     a8, .Lsec_82_10
        callx8   a8
        l32i.n   a3, a3, 52
        bbci     a3, 6, .Lsec_82_80
        l32r     a8, .Lsec_82_4
        l32r     a9, .Lsec_82_8
        memw
        l32i.n   a3, a8, 0
        and      a3, a3, a9
        memw
        s32i.n   a3, a8, 0
.Lsec_82_80:
        movi.n   a2, 0
        memw
        retw.n
        .size   wdev_set_promis_misc_pkt, .-wdev_set_promis_misc_pkt
.Lsec_82_87:

        .section .iram1.17,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_83_0, .Lsec_156_0
        .literal .Lsec_83_4, memcpy
.Lsec_83_8:
        .global wdev_push_promis_misc_buf
        .type   wdev_push_promis_misc_buf, @function
wdev_push_promis_misc_buf:
        entry    a1, 32
        l32r     a3, .Lsec_83_0
        mov.n    a11, a2
        movi.n   a2, -1
        l32i.n   a10, a3, 0
        beqz     a10, .Lsec_83_64
        movi.n   a13, 9
        l8ui     a8, a10, 0
        addx8    a3, a8, a8
        subx8    a3, a3, a8
        slli     a9, a3, 2
        addx4    a3, a3, a10
        l8ui     a12, a3, 6
        bltu     a13, a12, .Lsec_83_64
        addi.n   a9, a9, 8
        movi.n   a12, 28
        l8ui     a8, a3, 4
        subx8    a8, a8, a8
        addx4    a8, a8, a9
        add      a10, a10, a8
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_83_4
        callx8   a8
        l8ui     a8, a3, 4
        addi.n   a8, a8, 1
        extui    a8, a8, 0, 8
        bnei     a8, 10, .Lsec_83_51
        movi.n   a8, 0
.Lsec_83_51:
        s8i      a8, a3, 4
        memw
        movi.n   a2, 0
        l8ui     a8, a3, 6
        addi.n   a8, a8, 1
        s8i      a8, a3, 6
        memw
.Lsec_83_64:
        memw
        retw.n
        .size   wdev_push_promis_misc_buf, .-wdev_push_promis_misc_buf
.Lsec_83_69:

        .section .text.wdev_pop_promis_misc_buf,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_84_0, .Lsec_156_0
        .literal .Lsec_84_4, memcpy
.Lsec_84_8:
        .global wdev_pop_promis_misc_buf
        .type   wdev_pop_promis_misc_buf, @function
wdev_pop_promis_misc_buf:
        entry    a1, 32
        l32r     a3, .Lsec_84_0
        mov.n    a10, a2
        movi.n   a2, -1
        l32i.n   a11, a3, 0
        beqz     a11, .Lsec_84_68
        movi.n   a8, 1
        l8ui     a3, a11, 0
        sub      a8, a8, a3
        addx8    a3, a8, a8
        subx8    a3, a3, a8
        slli     a9, a3, 2
        addx4    a3, a3, a11
        l8ui     a12, a3, 6
        beqz.n   a12, .Lsec_84_68
        addi.n   a9, a9, 8
        movi.n   a12, 28
        l8ui     a8, a3, 5
        subx8    a8, a8, a8
        addx4    a8, a8, a9
        add.n    a11, a11, a8
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_84_4
        callx8   a8
        l8ui     a8, a3, 5
        addi     a8, a8, 1
        extui    a8, a8, 0, 8
        bnei     a8, 10, .Lsec_84_54
        movi     a8, 0
.Lsec_84_54:
        s8i      a8, a3, 5
        memw
        movi.n   a2, 0
        l8ui     a8, a3, 6
        addi     a8, a8, -1
        s8i      a8, a3, 6
        memw
.Lsec_84_68:
        memw
        retw.n
        .size   wdev_pop_promis_misc_buf, .-wdev_pop_promis_misc_buf
.Lsec_84_6d:

        .section .text.wdev_process_misc_pkt,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_85_0, .Lsec_156_0
        .literal .Lsec_85_4, g_osi_funcs_p
        .literal .Lsec_85_8, memcpy
        .literal .Lsec_85_c, pp_post
        .literal .Lsec_85_10, wdev_pop_promis_misc_buf
.Lsec_85_14:
        .global wdev_process_misc_pkt
        .type   wdev_process_misc_pkt, @function
wdev_process_misc_pkt:
        entry    a1, 64
        l32r     a2, .Lsec_85_0
        l32i.n   a2, a2, 0
        beqz     a2, .Lsec_85_98
        movi.n   a3, 1
        l8ui     a4, a2, 0
        sub      a3, a3, a4
        s8i      a3, a2, 0
        memw
        l32r     a3, .Lsec_85_4
        memw
        j        .Lsec_85_8d
.Lsec_85_36:
        l32i.n   a2, a3, 0
        movi     a10, 8
        l32i     a2, a2, 0x16c
        callx8   a2
        mov.n    a2, a10
        beqz     a10, .Lsec_85_8d
        l32i.n   a4, a3, 0
        movi.n   a10, 28
        l32i     a4, a4, 0x16c
        callx8   a4
        mov.n    a4, a10
        bnez.n   a10, .Lsec_85_58
        j        .Lsec_85_82
        .byte    0x00
.Lsec_85_58:
        mov.n    a11, a1
        movi.n   a12, 28
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_85_8
        callx8   a8
        movi.n   a8, 3
        s32i.n   a8, a2, 0
        mov.n    a11, a2
        movi.n   a10, 13
        s32i.n   a4, a2, 4
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_post
        l32r     a8, .Lsec_85_c
        callx8   a8
        bnei     a10, 1, .Lsec_85_8d
        mov.n    a10, a4
        l32i.n   a8, a3, 0
        l32i     a8, a8, 176
        callx8   a8
.Lsec_85_82:
        mov.n    a10, a2
        l32i     a4, a3, 0
        l32i     a4, a4, 176
        callx8   a4
.Lsec_85_8d:
        mov.n    a10, a1
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_pop_promis_misc_buf
        l32r     a8, .Lsec_85_10
        callx8   a8
        bnei     a10, -1, .Lsec_85_36
.Lsec_85_98:
        movi.n   a2, 0
        retw.n
        .size   wdev_process_misc_pkt, .-wdev_process_misc_pkt
.Lsec_85_9c:

        .section .text.wdev_set_promis_misc_buf,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_86_0, .Lsec_156_0
        .literal .Lsec_86_4, g_osi_funcs_p
        .literal .Lsec_86_8, .Lsec_155_0
        .literal .Lsec_86_c, wifi_log
        .literal .Lsec_86_10, memset
.Lsec_86_14:
        .global wdev_set_promis_misc_buf
        .type   wdev_set_promis_misc_buf, @function
wdev_set_promis_misc_buf:
        entry    a1, 32
        l32r     a3, .Lsec_86_0
        extui    a2, a2, 0, 8
        l32i     a10, a3, 0
        beqz     a2, .Lsec_86_6d
        l32r     a2, .Lsec_86_4
        beqz.n   a10, .Lsec_86_31
        l32i     a8, a2, 0
        l32i     a8, a8, 176
        callx8   a8
.Lsec_86_31:
        movi     a10, 0x23c
        l32i.n   a2, a2, 0
        l32i     a2, a2, 0x15c
        callx8   a2
        s32i.n   a10, a3, 0
        bnez.n   a10, .Lsec_86_5d
        movi     a12, 1
        slli     a11, a12, 11
        movi     a10, 6
        l32r     a13, .Lsec_86_8
        movi     a2, 0x101
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_86_c
        callx8   a8
        j        .Lsec_86_7c
        .byte    0x00
        .byte    0x00
.Lsec_86_5d:
        movi     a12, 0x23c
        movi.n   a11, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, memset
        l32r     a8, .Lsec_86_10
        callx8   a8
        movi.n   a2, 0
        j        .Lsec_86_7c
.Lsec_86_6d:
        beqz.n   a10, .Lsec_86_7c
        l32r     a8, .Lsec_86_4
        l32i.n   a8, a8, 0
        l32i     a8, a8, 176
        callx8   a8
        s32i.n   a2, a3, 0
.Lsec_86_7c:
        memw
        retw.n
        .size   wdev_set_promis_misc_buf, .-wdev_set_promis_misc_buf
.Lsec_86_81:

        .section .text.wdev_set_promis_ctrl_pkt,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_87_0, wDevCtrl
        .literal .Lsec_87_4, .Lsec_162_0
        .literal .Lsec_87_8, hal_sniffer_rx_set_promis
.Lsec_87_c:
        .global wdev_set_promis_ctrl_pkt
        .type   wdev_set_promis_ctrl_pkt, @function
wdev_set_promis_ctrl_pkt:
        entry    a1, 32
        extui    a2, a2, 0, 8
        beqz.n   a2, .Lsec_87_2d
        l32r     a8, .Lsec_87_0
        l8ui     a9, a8, 48
        beqz.n   a9, .Lsec_87_2d
        l32i.n   a8, a8, 52
        bbci     a8, 1, .Lsec_87_2d
        movi.n   a11, 1
        mov.n    a10, a11
        l32r     a8, .Lsec_87_4
        l32i.n   a12, a8, 0
        j        .Lsec_87_33
.Lsec_87_2d:
        movi.n   a12, 0
        mov.n    a11, a12
        movi.n   a10, 1
.Lsec_87_33:
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_sniffer_rx_set_promis
        l32r     a8, .Lsec_87_8
        callx8   a8
        movi.n   a2, 0
        retw.n
        .size   wdev_set_promis_ctrl_pkt, .-wdev_set_promis_ctrl_pkt
.Lsec_87_3d:

        .section .text.wdev_set_promis,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_88_0, .Lsec_154_0
        .literal .Lsec_88_4, wDevCtrl
        .literal .Lsec_88_8, .Lsec_153_0
        .literal .Lsec_88_c, wifi_log
        .literal .Lsec_88_10, hal_sniffer_enable
        .literal .Lsec_88_14, wdev_set_promis_misc_buf
        .literal .Lsec_88_18, wdev_set_promis_misc_pkt
        .literal .Lsec_88_1c, wifi_log
        .literal .Lsec_88_20, hal_sniffer_disable
        .literal .Lsec_88_24, wdev_set_promis_misc_buf
        .literal .Lsec_88_28, wdev_set_promis_misc_pkt
        .literal .Lsec_88_2c, wdev_set_promis_ctrl_pkt
.Lsec_88_30:
        .global wdev_set_promis
        .type   wdev_set_promis, @function
wdev_set_promis:
        entry    a1, 32
        movi.n   a11, 1
        extui    a2, a2, 0, 8
        l32r     a3, .Lsec_88_4
        slli     a11, a11, 11
        beqz.n   a2, .Lsec_88_7a
        movi.n   a12, 3
        movi     a10, 6
        l32r     a13, .Lsec_88_0
        movi     a2, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_88_c
        callx8   a8
        s8i      a2, a3, 48
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_sniffer_enable
        l32r     a8, .Lsec_88_10
        callx8   a8
        movi.n   a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_set_promis_misc_buf
        l32r     a8, .Lsec_88_14
        callx8   a8
        mov.n    a2, a10
        bnez     a10, .Lsec_88_b0
        movi.n   a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_set_promis_misc_pkt
        l32r     a8, .Lsec_88_18
        callx8   a8
        movi.n   a10, 1
        j        .Lsec_88_aa
.Lsec_88_7a:
        l32r     a13, .Lsec_88_8
        movi.n   a12, 3
        movi     a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_88_1c
        callx8   a8
        s8i      a2, a3, 48
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_sniffer_disable
        l32r     a8, .Lsec_88_20
        callx8   a8
        or       a10, a2, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_set_promis_misc_buf
        l32r     a8, .Lsec_88_24
        callx8   a8
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_set_promis_misc_pkt
        l32r     a8, .Lsec_88_28
        callx8   a8
        mov.n    a10, a2
.Lsec_88_aa:
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_set_promis_ctrl_pkt
        l32r     a8, .Lsec_88_2c
        callx8   a8
.Lsec_88_b0:
        retw.n
        .size   wdev_set_promis, .-wdev_set_promis
.Lsec_88_b2:

        .section .text.wdev_set_promis_filter,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_89_0, wDevCtrl+0x34
        .literal .Lsec_89_4, wdev_set_promis_misc_pkt
.Lsec_89_8:
        .global wdev_set_promis_filter
        .type   wdev_set_promis_filter, @function
wdev_set_promis_filter:
        entry    a1, 32
        l8ui     a9, a2, 0
        l32r     a8, .Lsec_89_0
        l8ui     a10, a2, 1
        s8i      a9, a8, 0
        memw
        l8ui     a9, a2, 2
        s8i      a10, a8, 1
        memw
        movi.n   a10, 1
        l8ui     a2, a2, 3
        s8i      a9, a8, 2
        s8i      a2, a8, 3
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_set_promis_misc_pkt
        l32r     a8, .Lsec_89_4
        callx8   a8
        movi.n   a2, 0
        retw.n
        .size   wdev_set_promis_filter, .-wdev_set_promis_filter
.Lsec_89_3e:

        .section .text.wdev_get_promis_filter,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_90_0, wDevCtrl+0x34
.Lsec_90_4:
        .global wdev_get_promis_filter
        .type   wdev_get_promis_filter, @function
wdev_get_promis_filter:
        entry    a1, 32
        l32r     a8, .Lsec_90_0
        l8ui     a9, a8, 0
        l8ui     a10, a8, 1
        s8i      a9, a2, 0
        memw
        l8ui     a9, a8, 2
        s8i      a10, a2, 1
        memw
        l8ui     a8, a8, 3
        s8i      a9, a2, 2
        s8i      a8, a2, 3
        memw
        movi.n   a2, 0
        memw
        retw.n
        .size   wdev_get_promis_filter, .-wdev_get_promis_filter
.Lsec_90_32:

        .section .text.wdev_set_promis_ctrl_filter,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_91_0, .Lsec_162_0
        .literal .Lsec_91_4, wdev_set_promis_ctrl_pkt
.Lsec_91_8:
        .global wdev_set_promis_ctrl_filter
        .type   wdev_set_promis_ctrl_filter, @function
wdev_set_promis_ctrl_filter:
        entry    a1, 32
        l8ui     a9, a2, 0
        l32r     a8, .Lsec_91_0
        l8ui     a10, a2, 1
        s8i      a9, a8, 0
        memw
        l8ui     a9, a2, 2
        s8i      a10, a8, 1
        memw
        movi.n   a10, 1
        l8ui     a2, a2, 3
        s8i      a9, a8, 2
        s8i      a2, a8, 3
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_set_promis_ctrl_pkt
        l32r     a8, .Lsec_91_4
        callx8   a8
        movi.n   a2, 0
        retw.n
        .size   wdev_set_promis_ctrl_filter, .-wdev_set_promis_ctrl_filter
.Lsec_91_3e:

        .section .text.wdev_get_promis_ctrl_filter,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_92_0, .Lsec_162_0
.Lsec_92_4:
        .global wdev_get_promis_ctrl_filter
        .type   wdev_get_promis_ctrl_filter, @function
wdev_get_promis_ctrl_filter:
        entry    a1, 32
        l32r     a8, .Lsec_92_0
        l8ui     a9, a8, 0
        l8ui     a10, a8, 1
        s8i      a9, a2, 0
        memw
        l8ui     a9, a8, 2
        s8i      a10, a2, 1
        memw
        l8ui     a8, a8, 3
        s8i      a9, a2, 2
        s8i      a8, a2, 3
        memw
        movi.n   a2, 0
        memw
        retw.n
        .size   wdev_get_promis_ctrl_filter, .-wdev_get_promis_ctrl_filter
.Lsec_92_32:

        .section .text.wDev_SnifferRxAmpdu,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_93_0, wDevCtrl
        .literal .Lsec_93_4, g_osi_funcs_p
        .literal .Lsec_93_8, memcpy
        .literal .Lsec_93_c, pp_post
.Lsec_93_10:
        .global wDev_SnifferRxAmpdu
        .type   wDev_SnifferRxAmpdu, @function
wDev_SnifferRxAmpdu:
        entry    a1, 32
        l32r     a3, .Lsec_93_0
        movi.n   a4, 36
        l32i.n   a3, a3, 52
        bnone    a4, a3, .Lsec_93_84
        movi.n   a10, 8
        l32i.n   a2, a2, 4
        l32r     a4, .Lsec_93_4
        l32i.n   a6, a2, 4
        l32i.n   a2, a4, 0
        l32i     a2, a2, 0x16c
        callx8   a2
        mov.n    a3, a10
        l32i.n   a2, a6, 24
        extui    a2, a2, 0, 12
        addi     a2, a2, 28
        extui    a2, a2, 0, 16
        beqz     a10, .Lsec_93_84
        l32i.n   a5, a4, 0
        mov.n    a10, a2
        l32i     a5, a5, 0x16c
        callx8   a5
        mov.n    a5, a10
        bnez.n   a10, .Lsec_93_50
        j        .Lsec_93_7a
        .byte    0x00
.Lsec_93_50:
        mov.n    a12, a2
        mov.n    a11, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_93_8
        callx8   a8
        movi.n   a2, 2
        s32i.n   a2, a3, 0
        mov.n    a11, a3
        movi.n   a10, 13
        s32i.n   a5, a3, 4
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_post
        l32r     a8, .Lsec_93_c
        callx8   a8
        bnei     a10, 1, .Lsec_93_84
        mov.n    a10, a5
        l32i.n   a2, a4, 0
        l32i     a2, a2, 176
        callx8   a2
.Lsec_93_7a:
        mov.n    a10, a3
        l32i.n   a2, a4, 0
        l32i     a2, a2, 176
        callx8   a2
.Lsec_93_84:
        retw.n
        .size   wDev_SnifferRxAmpdu, .-wDev_SnifferRxAmpdu
.Lsec_93_86:

        .section .text.wDev_RxSecError,"ax",@progbits
        .align  4
.Lsec_94_0:
        .global wDev_RxSecError
        .type   wDev_RxSecError, @function
wDev_RxSecError:
        entry    a1, 32
        extui    a2, a2, 0, 8
        addi     a9, a2, 31
        extui    a9, a9, 0, 8
        movi     a8, 1
        bltui    a9, 6, .Lsec_94_15
        movi     a8, 0
.Lsec_94_15:
        addi     a2, a2, 16
        extui    a2, a2, 0, 8
        movi.n   a10, 12
        movi.n   a9, 1
        bgeu     a10, a2, .Lsec_94_24
        movi.n   a9, 0
.Lsec_94_24:
        or       a2, a8, a9
        extui    a2, a2, 0, 1
        retw.n
        .size   wDev_RxSecError, .-wDev_RxSecError
.Lsec_94_2c:

        .section .wifiorslpiram.25,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_95_0, wDevCtrl
        .literal .Lsec_95_4, read_hw_noisefloor
.Lsec_95_8:
        .global wDev_GetNoiseFloor
        .type   wDev_GetNoiseFloor, @function
wDev_GetNoiseFloor:
        entry    a1, 32
        l32r     a2, .Lsec_95_0
        l8ui     a8, a2, 47
        sext     a8, a8, 7
        blti     a8, 1, .Lsec_95_28
        .reloc  ., R_XTENSA_ASM_EXPAND, read_hw_noisefloor
        l32r     a8, .Lsec_95_4
        callx8   a8
        addi.n   a10, a10, 2
        srai     a10, a10, 2
        s8i      a10, a2, 47
        memw
.Lsec_95_28:
        l8ui     a2, a2, 47
        sext     a2, a2, 7
        memw
        retw.n
        .size   wDev_GetNoiseFloor, .-wDev_GetNoiseFloor
.Lsec_95_33:

        .section .text.wdev_csi_rx_process,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_96_0, g_wdev_csi_rx
        .literal .Lsec_96_4, g_osi_funcs_p
        .literal .Lsec_96_8, wDevCtrl
        .literal .Lsec_96_c, g_wdev_csi_rx_ctx
        .literal .Lsec_96_10, memcpy
        .literal .Lsec_96_14, memset
        .literal .Lsec_96_18, memcpy
        .literal .Lsec_96_1c, memcpy
        .literal .Lsec_96_20, wDev_GetNoiseFloor
.Lsec_96_24:
        .global wdev_csi_rx_process
        .type   wdev_csi_rx_process, @function
wdev_csi_rx_process:
        entry    a1, 48
        extui    a3, a3, 0, 16
        s32i.n   a3, a1, 0
        extui    a3, a6, 0, 16
        bnei     a7, 3, .Lsec_96_35
        j        .Lsec_96_134
.Lsec_96_35:
        l32r     a6, .Lsec_96_0
        l32i.n   a8, a6, 0
        beqz     a8, .Lsec_96_134
        movi.n   a10, 64
        l32r     a6, .Lsec_96_4
        l32i.n   a8, a6, 0
        l32i     a8, a8, 0x16c
        memw
        callx8   a8
        mov.n    a6, a10
        beqz     a10, .Lsec_96_134
        movi.n   a9, 15
        addi     a10, a10, 28
        movi.n   a12, 6
        bgeu     a9, a3, .Lsec_96_68
        addi.n   a11, a5, 10
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_96_10
        callx8   a8
        j        .Lsec_96_70
        .byte    0x00
.Lsec_96_68:
        movi.n   a11, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, memset
        l32r     a8, .Lsec_96_14
        callx8   a8
.Lsec_96_70:
        movi.n   a12, 6
        addi.n   a11, a5, 4
        addi     a10, a6, 34
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_96_18
        callx8   a8
        l32r     a10, .Lsec_96_8
        mov.n    a11, a4
        movi.n   a12, 28
        addi     a7, a7, -1
        l8ui     a9, a10, 45
        slli     a9, a9, 4
        l8ui     a10, a10, 44
        extui    a10, a10, 0, 4
        or       a9, a9, a10
        or       a10, a6, a6
        s8i      a9, a4, 10
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_96_1c
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, wDev_GetNoiseFloor
        l32r     a8, .Lsec_96_20
        callx8   a8
        movi.n   a9, 1
        s8i      a10, a6, 8
        memw
        movi.n   a10, 0
        mov.n    a11, a10
        moveqz   a11, a9, a7
        s32i.n   a2, a6, 44
        l32i.n   a2, a1, 0
        s16i     a2, a6, 48
        addi     a2, a5, 24
        s16i     a3, a6, 60
        movi.n   a3, 12
        s8i      a11, a6, 40
        memw
        s32i.n   a5, a6, 52
        s32i.n   a2, a6, 56
        l8ui     a2, a5, 0
        and      a2, a2, a3
        bnei     a2, 4, .Lsec_96_e7
        movi.n   a2, -1
        memw
        j        .Lsec_96_10e
.Lsec_96_e7:
        l8ui     a2, a4, 27
        movi     a4, -198
        mov.n    a3, a10
        add.n    a4, a2, a4
        moveqz   a3, a9, a2
        moveqz   a10, a9, a4
        or       a3, a3, a10
        bnez.n   a3, .Lsec_96_108
        movi     a4, -245
        add      a2, a2, a4
        movnez   a9, a3, a2
        beqz     a9, .Lsec_96_114
.Lsec_96_108:
        l16ui    a2, a5, 22
        srli     a2, a2, 4
.Lsec_96_10e:
        s16i     a2, a6, 62
        memw
.Lsec_96_114:
        mov.n    a11, a6
        l32r     a3, .Lsec_96_0
        l32i.n   a2, a3, 0
        l32r     a3, .Lsec_96_c
        l32i.n   a10, a3, 0
        memw
        callx8   a2
        mov.n    a10, a6
        l32r     a3, .Lsec_96_4
        l32i     a2, a3, 0
        l32i     a2, a2, 176
        callx8   a2
.Lsec_96_134:
        retw.n
        .size   wdev_csi_rx_process, .-wdev_csi_rx_process
.Lsec_96_136:

        .section .text.wDev_SnifferRxData,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_97_0, wDevCtrl
        .literal .Lsec_97_4, g_osi_funcs_p
        .literal .Lsec_97_8, wdev_csi_len_align
        .literal .Lsec_97_c, wdev_csi_hw_bug_check
        .literal .Lsec_97_10, memcpy
        .literal .Lsec_97_14, memcpy
        .literal .Lsec_97_18, memcpy
        .literal .Lsec_97_1c, memcpy
        .literal .Lsec_97_20, memcpy
        .literal .Lsec_97_24, memcpy
        .literal .Lsec_97_28, memcpy
        .literal .Lsec_97_2c, wdev_csi_rx_process
        .literal .Lsec_97_30, pp_post
.Lsec_97_34:
        .global wDev_SnifferRxData
        .type   wDev_SnifferRxData, @function
wDev_SnifferRxData:
        entry    a1, 48
        l32r     a6, .Lsec_97_0
        extui    a3, a3, 0, 8
        s32i.n   a2, a1, 8
        l32i.n   a2, a6, 0
        s32i.n   a4, a1, 12
        l32i.n   a4, a2, 4
        l32i.n   a2, a1, 12
        bnei     a2, 64, .Lsec_97_4d
        j        .Lsec_97_28e
.Lsec_97_4d:
        movi     a2, 98
        l32i     a5, a1, 12
        bne      a5, a2, .Lsec_97_59
        j        .Lsec_97_10a
.Lsec_97_59:
        mov.n    a10, a4
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_csi_len_align
        l32r     a8, .Lsec_97_8
        callx8   a8
        movi.n   a8, 0
        extui    a2, a10, 0, 16
        s32i.n   a8, a1, 0
        beq      a2, a8, .Lsec_97_bc
        extui    a5, a10, 0, 16
        addi     a5, a5, 24
        add.n    a5, a4, a5
        addi     a7, a4, 24
        mov.n    a11, a5
        mov.n    a10, a7
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_csi_hw_bug_check
        l32r     a8, .Lsec_97_c
        callx8   a8
        s32i.n   a10, a1, 0
        bnei     a10, 1, .Lsec_97_b2
        l8ui     a10, a4, 24
        l8ui     a11, a4, 25
        s8i      a10, a5, 0
        memw
        l8ui     a10, a4, 26
        s8i      a11, a5, 1
        memw
        l8ui     a8, a4, 27
        s8i      a10, a5, 2
        s8i      a8, a5, 3
        memw
        memw
        j        .Lsec_97_bc
.Lsec_97_b2:
        l32i.n   a9, a1, 0
        movi.n   a8, 0
        addi     a5, a9, -2
        moveqz   a2, a8, a5
.Lsec_97_bc:
        add.n    a5, a4, a2
        l16ui    a10, a5, 28
        movi.n   a5, 0
        l32i.n   a8, a6, 0
        j        .Lsec_97_d7
.Lsec_97_c8:
        memw
        l32i.n   a11, a8, 0
        extui    a11, a11, 12, 12
        add.n    a5, a11, a5
        extui    a5, a5, 0, 16
        l32i.n   a8, a8, 8
.Lsec_97_d7:
        l32i.n   a9, a1, 8
        bne      a8, a9, .Lsec_97_c8
        sub      a8, a5, a2
        extui    a5, a10, 0, 4
        memw
        l32i.n   a11, a9, 0
        movi.n   a10, 1
        extui    a11, a11, 12, 12
        add.n    a8, a8, a11
        extui    a7, a8, 0, 16
        s32i.n   a5, a1, 4
        beqz.n   a5, .Lsec_97_116
        movi.n   a11, 2
        addi     a5, a5, -4
        movnez   a10, a11, a5
        s32i.n   a10, a1, 4
        movi.n   a10, 20
        moveqz   a10, a11, a5
        memw
        j        .Lsec_97_116
.Lsec_97_10a:
        movi.n   a2, 0
        movi.n   a5, 3
        s32i.n   a2, a1, 0
        movi.n   a7, 28
        movi.n   a10, 8
        s32i.n   a5, a1, 4
.Lsec_97_116:
        l32i.n   a5, a6, 52
        bany     a10, a5, .Lsec_97_11e
        j        .Lsec_97_28e
.Lsec_97_11e:
        movi.n   a10, 8
        l32r     a8, .Lsec_97_4
        l32i.n   a5, a8, 0
        l32i     a5, a5, 0x16c
        memw
        callx8   a5
        mov.n    a5, a10
        beqz     a10, .Lsec_97_28e
        l32r     a9, .Lsec_97_4
        l32i.n   a10, a9, 0
        l32i     a11, a10, 0x16c
        mov.n    a10, a7
        callx8   a11
        mov.n    a7, a10
        bnez.n   a10, .Lsec_97_14a
        l32r     a3, .Lsec_97_4
        j        .Lsec_97_284
.Lsec_97_14a:
        l32i.n   a8, a1, 12
        movi     a10, 98
        bne      a8, a10, .Lsec_97_191
        addi     a2, a2, 24
        add.n    a2, a4, a2
        mov.n    a11, a4
        movi.n   a12, 24
        or       a10, a7, a7
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_97_10
        callx8   a8
        l8ui     a4, a2, 0
        addi     a3, a7, 24
        l8ui     a6, a2, 1
        s8i      a4, a7, 24
        memw
        l8ui     a4, a2, 2
        s8i      a6, a7, 25
        memw
        l8ui     a2, a2, 3
        s8i      a4, a7, 26
        s8i      a2, a7, 27
        memw
        memw
        j        .Lsec_97_260
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_97_191:
        bgeui    a3, 2, .Lsec_97_1c4
        mov.n    a11, a4
        movi     a12, 24
        or       a10, a7, a7
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_97_14
        callx8   a8
        l32i.n   a3, a6, 0
        addi     a11, a2, 24
        add.n    a11, a4, a11
        addi     a10, a7, 24
        memw
        l32i.n   a12, a3, 0
        extui    a12, a12, 12, 12
        addi     a12, a12, -24
        sub      a12, a12, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_97_18
        callx8   a8
        j        .Lsec_97_242
        .byte    0x00
.Lsec_97_1c4:
        l32i.n   a4, a6, 0
        mov.n    a3, a7
        movi.n   a10, 1
        j        .Lsec_97_226
.Lsec_97_1cd:
        beqz.n   a10, .Lsec_97_208
        movi.n   a12, 24
        mov.n    a10, a3
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_97_1c
        callx8   a8
        memw
        l32i.n   a10, a4, 0
        movi.n   a9, -24
        extui    a10, a10, 12, 12
        sub      a12, a9, a2
        addi     a11, a2, 24
        l32i.n   a13, a4, 4
        add.n    a12, a12, a10
        add.n    a11, a13, a11
        addi     a10, a3, 24
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_97_20
        callx8   a8
        memw
        l32i.n   a10, a4, 0
        extui    a10, a10, 12, 12
        sub      a10, a10, a2
        j        .Lsec_97_220
        .byte    0x00
        .byte    0x00
.Lsec_97_208:
        memw
        l32i.n   a12, a4, 0
        mov.n    a10, a3
        extui    a12, a12, 12, 12
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_97_24
        callx8   a8
        memw
        l32i.n   a10, a4, 0
        extui    a10, a10, 12, 12
.Lsec_97_220:
        add.n    a3, a3, a10
        movi.n   a10, 0
        l32i.n   a4, a4, 8
.Lsec_97_226:
        l32i.n   a8, a1, 8
        l32i.n   a11, a4, 4
        bne      a4, a8, .Lsec_97_1cd
        mov.n    a10, a3
        memw
        l32i.n   a12, a4, 0
        extui    a12, a12, 12, 12
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_97_28
        callx8   a8
        memw
        l32i.n   a3, a4, 0
.Lsec_97_242:
        beqz.n   a2, .Lsec_97_260
        addi     a13, a7, 28
        mov.n    a12, a7
        mov.n    a11, a2
        l32i.n   a3, a6, 0
        l32i.n   a14, a7, 24
        extui    a14, a14, 0, 12
        l32i     a10, a3, 4
        addi     a10, a10, 24
        l32i.n   a15, a1, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_csi_rx_process
        l32r     a8, .Lsec_97_2c
        callx8   a8
.Lsec_97_260:
        mov.n    a11, a5
        movi.n   a10, 13
        l32i.n   a9, a1, 4
        s32i     a7, a5, 4
        s32i.n   a9, a5, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_post
        l32r     a8, .Lsec_97_30
        callx8   a8
        bnei     a10, 1, .Lsec_97_28e
        mov.n    a10, a7
        l32r     a3, .Lsec_97_4
        l32i.n   a2, a3, 0
        l32i     a2, a2, 176
        callx8   a2
.Lsec_97_284:
        mov.n    a10, a5
        l32i.n   a2, a3, 0
        l32i     a2, a2, 176
        callx8   a2
.Lsec_97_28e:
        retw.n
        .size   wDev_SnifferRxData, .-wDev_SnifferRxData
.Lsec_97_290:

        .section .text.wDev_IndicateCtrlFrame,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_98_0, .Lsec_152_0
        .literal .Lsec_98_4, wDevCtrl
        .literal .Lsec_98_8, g_osi_funcs_p
        .literal .Lsec_98_c, wifi_log
        .literal .Lsec_98_10, wdev_csi_len_align
        .literal .Lsec_98_14, wdev_csi_hw_bug_check
        .literal .Lsec_98_18, memcpy
        .literal .Lsec_98_1c, memcpy
        .literal .Lsec_98_20, wdev_csi_rx_process
.Lsec_98_24:
        .global wDev_IndicateCtrlFrame
        .type   wDev_IndicateCtrlFrame, @function
wDev_IndicateCtrlFrame:
        entry    a1, 32
        extui    a14, a4, 0, 16
        beqi     a14, 1, .Lsec_98_3f
        l32r     a13, .Lsec_98_0
        movi.n   a12, 2
        movi.n   a11, 32
        movi.n   a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_98_c
        callx8   a8
        j        .Lsec_98_114
.Lsec_98_3f:
        l32r     a2, .Lsec_98_4
        l32i     a3, a2, 0
        l32i     a5, a3, 4
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_csi_len_align
        l32r     a8, .Lsec_98_10
        callx8   a8
        extui    a4, a10, 0, 16
        beqz     a4, .Lsec_98_114
        l8ui     a2, a2, 48
        bnei     a2, 1, .Lsec_98_61
        l32i.n   a2, a5, 0
        bltz     a2, .Lsec_98_114
.Lsec_98_61:
        extui    a2, a10, 0, 16
        addi     a2, a2, 24
        addi     a6, a5, 24
        add.n    a2, a5, a2
        mov.n    a11, a2
        mov.n    a10, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_csi_hw_bug_check
        l32r     a8, .Lsec_98_14
        callx8   a8
        mov.n    a7, a10
        bnei     a10, 1, .Lsec_98_a4
        l8ui     a8, a5, 24
        l8ui     a5, a5, 25
        s8i      a8, a2, 0
        memw
        l8ui     a8, a6, 2
        s8i      a5, a2, 1
        memw
        l8ui     a5, a6, 3
        s8i      a8, a2, 2
        s8i      a5, a2, 3
        memw
        memw
        j        .Lsec_98_ac
        .byte    0x00
        .byte    0x00
.Lsec_98_a4:
        addi     a2, a10, -2
        movi.n   a5, 0
        moveqz   a4, a5, a2
.Lsec_98_ac:
        l32r     a5, .Lsec_98_8
        memw
        l32i.n   a10, a3, 0
        extui    a10, a10, 12, 12
        sub      a10, a10, a4
        l32i.n   a2, a5, 0
        l32i     a2, a2, 0x16c
        callx8   a2
        mov.n    a2, a10
        beqz     a10, .Lsec_98_114
        l32i.n   a11, a3, 4
        movi.n   a12, 24
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_98_18
        callx8   a8
        addi     a6, a4, 24
        addi     a10, a2, 24
        memw
        l32i.n   a12, a3, 0
        extui    a12, a12, 12, 12
        sub      a12, a12, a4
        addi     a12, a12, -24
        l32i.n   a11, a3, 4
        add      a11, a11, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_98_1c
        callx8   a8
        mov.n    a15, a7
        l32i.n   a10, a3, 4
        addi     a13, a2, 28
        addi     a10, a10, 24
        mov.n    a12, a2
        mov.n    a11, a4
        l32i     a14, a2, 24
        extui    a14, a14, 0, 12
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_csi_rx_process
        l32r     a8, .Lsec_98_20
        callx8   a8
        mov.n    a10, a2
        l32i.n   a3, a5, 0
        l32i     a3, a3, 176
        callx8   a3
.Lsec_98_114:
        movi.n   a2, 1
        retw.n
        .size   wDev_IndicateCtrlFrame, .-wDev_IndicateCtrlFrame
.Lsec_98_118:

        .section .wifirxiram.26,"ax",@progbits
        .align  4
.Lsec_99_0:
        .global wDev_record_ftm_data
        .type   wDev_record_ftm_data, @function
wDev_record_ftm_data:
        entry    a1, 32
        retw.n
        .size   wDev_record_ftm_data, .-wDev_record_ftm_data
.Lsec_99_5:

        .section .text.wdev_nan_set_callbacks,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_100_0, g_wdev_is_nan_pkt_in_valid_slot_cb
.Lsec_100_4:
        .global wdev_nan_set_callbacks
        .type   wdev_nan_set_callbacks, @function
wdev_nan_set_callbacks:
        entry    a1, 32
        l32r     a8, .Lsec_100_0
        s32i.n   a2, a8, 0
        movi.n   a2, 0
        memw
        retw.n
        .size   wdev_nan_set_callbacks, .-wdev_nan_set_callbacks
.Lsec_100_13:

        .section .text.wDev_isNANPktInValidSlot,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_101_0, g_wdev_is_nan_pkt_in_valid_slot_cb
.Lsec_101_4:
        .global wDev_isNANPktInValidSlot
        .type   wDev_isNANPktInValidSlot, @function
wDev_isNANPktInValidSlot:
        entry    a1, 32
        l32i.n   a8, a2, 44
        movi.n   a9, 1
        slli     a9, a9, 19
        mov.n    a10, a2
        movi.n   a2, 1
        l32i.n   a11, a8, 16
        movi.n   a8, 3
        slli     a8, a8, 18
        and      a11, a11, a8
        bne      a11, a9, .Lsec_101_2e
        l32r     a8, .Lsec_101_0
        movi.n   a2, 0
        l32i.n   a9, a8, 0
        beq      a9, a2, .Lsec_101_2e
        callx8   a9
        mov.n    a2, a10
.Lsec_101_2e:
        retw.n
        .size   wDev_isNANPktInValidSlot, .-wDev_isNANPktInValidSlot
.Lsec_101_30:

        .section .text.wDev_ProcessRxData_NAN_Interface_Hook,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_102_0, NAN_STANDARD_BSSID
        .literal .Lsec_102_4, 0xbfffffff
        .literal .Lsec_102_8, NAN_NETWORK_ID
        .literal .Lsec_102_c, memcmp
        .literal .Lsec_102_10, memcmp
.Lsec_102_14:
        .global wDev_ProcessRxData_NAN_Interface_Hook
        .type   wDev_ProcessRxData_NAN_Interface_Hook, @function
wDev_ProcessRxData_NAN_Interface_Hook:
        entry    a1, 32
        l16ui    a8, a3, 0
        movi     a4, 240
        and      a8, a8, a4
        bnei     a8, 128, .Lsec_102_64
        movi     a12, 4
        l32r     a11, .Lsec_102_0
        addi     a10, a3, 16
        .reloc  ., R_XTENSA_ASM_EXPAND, memcmp
        l32r     a8, .Lsec_102_c
        callx8   a8
        bnez.n   a10, .Lsec_102_50
.Lsec_102_34:
        movi     a3, -113
        l8ui     a8, a2, 3
        movi.n   a4, 1
        and      a8, a8, a3
        movi.n   a3, 64
        or       a8, a8, a3
        s8i      a8, a2, 3
        memw
        memw
        j        .Lsec_102_7c
.Lsec_102_50:
        l32i.n   a3, a2, 0
        l32r     a4, .Lsec_102_4
        and      a3, a3, a4
        movi.n   a4, 0
        s32i.n   a3, a2, 0
        memw
        j        .Lsec_102_7c
        .byte    0x00
        .byte    0x00
.Lsec_102_64:
        movi     a9, 208
        movi.n   a4, 1
        bne      a8, a9, .Lsec_102_7c
        l32r     a11, .Lsec_102_8
        movi.n   a12, 6
        addi.n   a10, a3, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, memcmp
        l32r     a8, .Lsec_102_10
        callx8   a8
        beqz     a10, .Lsec_102_34
.Lsec_102_7c:
        mov.n    a2, a4
        retw.n
        .size   wDev_ProcessRxData_NAN_Interface_Hook, .-wDev_ProcessRxData_NAN_Interface_Hook
.Lsec_102_80:

        .section .text.wDev_FetchFirstDesc,"ax",@progbits
        .align  4
.Lsec_103_0:
        .global wDev_FetchFirstDesc
        .type   wDev_FetchFirstDesc, @function
wDev_FetchFirstDesc:
        entry    a1, 32
        movi.n   a2, 0
        retw.n
        .size   wDev_FetchFirstDesc, .-wDev_FetchFirstDesc
.Lsec_103_7:

        .section .text.wDev_Rxbuf_Deinit,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_104_0, wDevCtrl
        .literal .Lsec_104_4, g_osi_funcs_p
        .literal .Lsec_104_8, .Lsec_142_0
        .literal .Lsec_104_c, wifi_log
.Lsec_104_10:
        .global wDev_Rxbuf_Deinit
        .type   wDev_Rxbuf_Deinit, @function
wDev_Rxbuf_Deinit:
        entry    a1, 32
        l32r     a3, .Lsec_104_0
        movi.n   a4, 0
        l32i.n   a5, a3, 0
        mov.n    a2, a5
        l32r     a7, .Lsec_104_4
        j        .Lsec_104_3d
.Lsec_104_22:
        l32i.n   a10, a2, 4
        l32i.n   a6, a2, 8
        beqz     a10, .Lsec_104_36
        l32i     a8, a7, 0
        l32i     a8, a8, 176
        callx8   a8
        movi.n   a8, 0
        s32i.n   a8, a2, 4
.Lsec_104_36:
        addi.n   a4, a4, 1
        beq      a5, a6, .Lsec_104_40
        mov.n    a2, a6
.Lsec_104_3d:
        bnez     a2, .Lsec_104_22
.Lsec_104_40:
        l32r     a13, .Lsec_104_8
        movi.n   a10, 6
        movi.n   a2, 0
        mov.n    a14, a4
        movi.n   a12, 3
        movi     a11, 1
        s32i     a2, a3, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_104_c
        callx8   a8
        l32i.n   a10, a3, 8
        beq      a10, a2, .Lsec_104_6c
        l32r     a4, .Lsec_104_4
        l32i.n   a4, a4, 0
        l32i     a4, a4, 176
        callx8   a4
        s32i.n   a2, a3, 8
.Lsec_104_6c:
        memw
        retw.n
        .size   wDev_Rxbuf_Deinit, .-wDev_Rxbuf_Deinit
.Lsec_104_71:

        .section .text.wDev_Rxbuf_Init,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_105_0, g_osi_funcs_p
        .literal .Lsec_105_4, wDevCtrl
        .literal .Lsec_105_8, .Lsec_147_0
        .literal .Lsec_105_c, pTxRx
        .literal .Lsec_105_10, g_wifi_menuconfig
        .literal .Lsec_105_14, 0x000008a4
        .literal .Lsec_105_18, .Lsec_146_0
        .literal .Lsec_105_1c, .Lsec_148_0
        .literal .Lsec_105_20, 0xff000fff
        .literal .Lsec_105_24, 0xbfffffff
        .literal .Lsec_105_28, 0xdfffffff
        .literal .Lsec_105_2c, .Lsec_145_0
        .literal .Lsec_105_30, .Lsec_144_0
        .literal .Lsec_105_34, .Lsec_143_0
        .literal .Lsec_105_38, 0xdeadbeef
        .literal .Lsec_105_3c, wifi_log
        .literal .Lsec_105_40, wifi_log
        .literal .Lsec_105_44, wifi_log
        .literal .Lsec_105_48, memset
        .literal .Lsec_105_4c, wifi_log
        .literal .Lsec_105_50, wifi_log
        .literal .Lsec_105_54, wifi_log
        .literal .Lsec_105_58, wDev_Rxbuf_Deinit
.Lsec_105_5c:
        .global wDev_Rxbuf_Init
        .type   wDev_Rxbuf_Init, @function
wDev_Rxbuf_Init:
        entry    a1, 64
        l32r     a4, .Lsec_105_0
        addx2    a10, a2, a2
        slli     a10, a10, 2
        extui    a10, a10, 0, 16
        l32i     a3, a4, 0
        l32i     a3, a3, 0x168
        callx8   a3
        l32r     a3, .Lsec_105_4
        s32i.n   a10, a3, 8
        bnez.n   a10, .Lsec_105_94
        movi.n   a12, 1
        l32r     a13, .Lsec_105_8
        mov.n    a14, a2
        mov.n    a11, a12
        movi     a10, 6
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_105_3c
        callx8   a8
        j        .Lsec_105_1de
        .byte    0x00
.Lsec_105_94:
        l32r     a5, .Lsec_105_10
        l32r     a4, .Lsec_105_c
        l32i.n   a8, a5, 24
        l32i.n   a7, a4, 0
        beqz.n   a8, .Lsec_105_a6
        l32r     a8, .Lsec_105_14
        s32i     a8, a7, 0x3c4
.Lsec_105_a6:
        movi.n   a12, 3
        movi.n   a11, 1
        movi.n   a10, 6
        movi.n   a5, 0
        l32i     a14, a7, 0x3c4
        or       a7, a1, a1
        l32r     a13, .Lsec_105_18
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_105_40
        callx8   a8
        extui    a9, a2, 0, 16
        l32i.n   a4, a4, 0
        l32i     a4, a4, 0x3c4
        addi.n   a4, a4, 4
        extui    a6, a4, 0, 16
        extui    a4, a4, 0, 12
        l32i.n   a8, a3, 8
        s32i.n   a9, a1, 16
        memw
        j        .Lsec_105_186
        .byte    0x00
        .byte    0x00
.Lsec_105_dc:
        l32r     a10, .Lsec_105_0
        l32i.n   a9, a10, 0
        mov.n    a10, a6
        l32i     a9, a9, 0x15c
        s32i.n   a8, a1, 20
        memw
        callx8   a9
        l32i.n   a8, a1, 20
        bnez.n   a10, .Lsec_105_106
        l32r     a13, .Lsec_105_1c
        movi.n   a12, 2
        movi.n   a11, 1
        movi.n   a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_105_44
        callx8   a8
        j        .Lsec_105_198
        .byte    0x00
        .byte    0x00
.Lsec_105_106:
        movi.n   a11, 0
        mov.n    a12, a6
        s32i.n   a8, a1, 20
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, memset
        l32r     a8, .Lsec_105_48
        callx8   a8
        addi.n   a5, a5, 1
        l32i.n   a8, a1, 20
        memw
        l32i.n   a9, a8, 0
        s32i.n   a10, a8, 4
        movi.n   a10, -1
        slli     a10, a10, 12
        and      a9, a9, a10
        or       a9, a9, a4
        memw
        s32i.n   a9, a8, 0
        movi.n   a10, -1
        slli     a10, a10, 31
        memw
        l32i.n   a9, a8, 0
        or       a9, a9, a10
        memw
        s32i.n   a9, a8, 0
        memw
        l32i.n   a10, a8, 0
        extui    a10, a10, 0, 12
        slli     a10, a10, 12
        memw
        l32i.n   a9, a8, 0
        l32r     a11, .Lsec_105_20
        and      a9, a9, a11
        or       a9, a9, a10
        memw
        s32i.n   a9, a8, 0
        memw
        l32i.n   a9, a8, 0
        l32r     a10, .Lsec_105_24
        and      a9, a9, a10
        memw
        s32i.n   a9, a8, 0
        memw
        l32i.n   a9, a8, 0
        l32r     a10, .Lsec_105_28
        and      a9, a9, a10
        memw
        s32i.n   a9, a8, 0
        s32i.n   a8, a7, 8
        mov.n    a7, a8
        addi.n   a8, a8, 12
.Lsec_105_186:
        l32i.n   a9, a1, 16
        bge      a5, a9, .Lsec_105_18e
        j        .Lsec_105_dc
.Lsec_105_18e:
        movi.n   a4, 0
        s32i.n   a4, a7, 8
        l32i.n   a4, a1, 8
        s32i.n   a7, a3, 4
        s32i.n   a4, a3, 0
.Lsec_105_198:
        mov.n    a14, a5
        movi.n   a12, 3
        movi.n   a11, 1
        movi.n   a10, 6
        l32r     a13, .Lsec_105_2c
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_105_4c
        callx8   a8
        movi.n   a12, 3
        movi.n   a11, 1
        l32r     a10, .Lsec_105_10
        l32r     a13, .Lsec_105_30
        l32i.n   a14, a10, 4
        movi.n   a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_105_50
        callx8   a8
        bgeu     a5, a2, .Lsec_105_1e4
        movi.n   a12, 1
        l32r     a13, .Lsec_105_34
        mov.n    a15, a5
        mov.n    a14, a2
        or       a11, a12, a12
        movi     a10, 6
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_105_54
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, wDev_Rxbuf_Deinit
        l32r     a8, .Lsec_105_58
        callx8   a8
.Lsec_105_1de:
        movi     a2, 0x101
        j        .Lsec_105_249
.Lsec_105_1e4:
        l32i.n   a2, a3, 0
        l32r     a7, .Lsec_105_20
        l32r     a6, .Lsec_105_38
        j        .Lsec_105_240
        .byte    0x00
.Lsec_105_1f0:
        memw
        l32i.n   a4, a2, 0
        movi.n   a8, -1
        extui    a4, a4, 0, 12
        addi     a4, a4, -4
        slli     a8, a8, 12
        memw
        l32i.n   a5, a2, 0
        extui    a4, a4, 0, 12
        and      a5, a5, a8
        or       a4, a5, a4
        memw
        s32i.n   a4, a2, 0
        memw
        l32i.n   a5, a2, 0
        extui    a5, a5, 0, 12
        slli     a5, a5, 12
        memw
        l32i.n   a4, a2, 0
        and      a4, a4, a7
        or       a4, a4, a5
        memw
        s32i.n   a4, a2, 0
        l32i.n   a4, a2, 4
        s32i.n   a6, a4, 0
        memw
        l32i.n   a5, a2, 0
        extui    a5, a5, 0, 12
        add.n    a4, a4, a5
        s32i.n   a6, a4, 0
        l32i.n   a2, a2, 8
.Lsec_105_240:
        bnez     a2, .Lsec_105_1f0
        s8i      a2, a3, 48
        memw
.Lsec_105_249:
        memw
        retw.n
        .size   wDev_Rxbuf_Init, .-wDev_Rxbuf_Init
.Lsec_105_24e:

        .section .text.wDev_disable_low_rate,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_106_0, hal_mac_disable_low_rate
.Lsec_106_4:
        .global wDev_disable_low_rate
        .type   wDev_disable_low_rate, @function
wDev_disable_low_rate:
        entry    a1, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_disable_low_rate
        l32r     a8, .Lsec_106_0
        callx8   a8
        retw.n
        .size   wDev_disable_low_rate, .-wDev_disable_low_rate
.Lsec_106_f:

        .section .text.wDev_enable_low_rate,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_107_0, hal_mac_enable_low_rate
.Lsec_107_4:
        .global wDev_enable_low_rate
        .type   wDev_enable_low_rate, @function
wDev_enable_low_rate:
        entry    a1, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_enable_low_rate
        l32r     a8, .Lsec_107_0
        callx8   a8
        retw.n
        .size   wDev_enable_low_rate, .-wDev_enable_low_rate
.Lsec_107_f:

        .section .text.wDev_is_low_rate_enable,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_108_0, hal_mac_is_low_rate_enabled
.Lsec_108_4:
        .global wDev_is_low_rate_enable
        .type   wDev_is_low_rate_enable, @function
wDev_is_low_rate_enable:
        entry    a1, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_is_low_rate_enabled
        l32r     a8, .Lsec_108_0
        callx8   a8
        mov.n    a2, a10
        retw.n
        .size   wDev_is_low_rate_enable, .-wDev_is_low_rate_enable
.Lsec_108_11:

        .section .iram1.44,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_109_0, 0x3ff00024
        .literal .Lsec_109_4, hal_mac_rx_get_end_state
        .literal .Lsec_109_8, bb_wdt_get_status
        .literal .Lsec_109_c, 0x3ff40078
        .literal .Lsec_109_10, 0x3ff40078
        .literal .Lsec_109_14, bb_wdt_timeout_clear
        .literal .Lsec_109_18, hal_mac_interrupt_clr_watchdog
.Lsec_109_1c:
        .global wdev_process_panic_watchdog
        .type   wdev_process_panic_watchdog, @function
wdev_process_panic_watchdog:
        entry    a1, 32
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_rx_get_end_state
        l32r     a8, .Lsec_109_4
        callx8   a8
        mov.n    a2, a10
        .reloc  ., R_XTENSA_ASM_EXPAND, bb_wdt_get_status
        l32r     a8, .Lsec_109_8
        callx8   a8
        movi.n   a8, 1
        slli     a8, a8, 29
        and      a8, a10, a8
        bnez     a8, .Lsec_109_81
        movi     a8, 240
        and      a10, a10, a8
        bnei     a10, 32, .Lsec_109_81
        addi     a2, a2, -100
        bnez.n   a2, .Lsec_109_81
        l32r     a9, .Lsec_109_0
        mov.n    a8, a9
        l32r     a2, .Lsec_109_c
        rsil     a10, 3
        l32i.n   a2, a2, 0
        l32i.n   a8, a8, 0
        wsr.ps   a10
        rsync
        movi.n   a2, 1
        or       a8, a8, a2
        memw
        s32i.n   a8, a9, 0
        mov.n    a8, a9
        l32r     a2, .Lsec_109_10
        rsil     a10, 3
        l32i.n   a2, a2, 0
        l32i.n   a8, a8, 0
        wsr.ps   a10
        rsync
        movi.n   a2, -2
        and      a8, a8, a2
        memw
        s32i.n   a8, a9, 0
.Lsec_109_81:
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, bb_wdt_timeout_clear
        l32r     a8, .Lsec_109_14
        callx8   a8
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_interrupt_clr_watchdog
        l32r     a8, .Lsec_109_18
        callx8   a8
        retw.n
        .size   wdev_process_panic_watchdog, .-wdev_process_panic_watchdog
.Lsec_109_92:

        .section .iram1.45,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_110_0, g_osi_funcs_p
        .literal .Lsec_110_4, 0x01000024
        .literal .Lsec_110_8, wDevCtrl
        .literal .Lsec_110_c, .Lsec_141_4
        .literal .Lsec_110_10, .Lsec_141_8
        .literal .Lsec_110_14, .Lsec_141_c
        .literal .Lsec_110_18, .Lsec_141_0
        .literal .Lsec_110_1c, .Lsec_140_4
        .literal .Lsec_110_20, .Lsec_140_0
        .literal .Lsec_110_24, 0x6001c06c
        .literal .Lsec_110_28, .Lsec_138_0
        .literal .Lsec_110_2c, .Lsec_139_0
        .literal .Lsec_110_30, 0xff0000ff
        .literal .Lsec_110_34, 0x00000fff
        .literal .Lsec_110_38, 0xffff3fff
        .literal .Lsec_110_3c, 0xf7ffff00
        .literal .Lsec_110_40, 0x00fff000
        .literal .Lsec_110_44, hal_mac_interrupt_get_event
        .literal .Lsec_110_48, hal_mac_interrupt_clr_event
        .literal .Lsec_110_4c, wdev_process_panic_watchdog
        .literal .Lsec_110_50, pp_post
        .literal .Lsec_110_54, hal_mac_rx_get_end_info
        .literal .Lsec_110_58, hal_sniffer_rx_clr_statistics
        .literal .Lsec_110_5c, wdev_push_promis_misc_buf
        .literal .Lsec_110_60, pp_post
        .literal .Lsec_110_64, lmacProcessRxSucData
        .literal .Lsec_110_68, lmacPostTxComplete
        .literal .Lsec_110_6c, lmacProcessAllTxTimeout
        .literal .Lsec_110_70, lmacProcessCollisions
.Lsec_110_74:
        .global wDev_ProcessFiq
        .type   wDev_ProcessFiq, @function
wDev_ProcessFiq:
        entry    a1, 32
        l32r     a3, .Lsec_110_0
        l32i.n   a2, a3, 0
        l32i     a2, a2, 216
        callx8   a2
.Lsec_110_82:
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_interrupt_get_event
        l32r     a8, .Lsec_110_44
        callx8   a8
        or       a2, a10, a10
        beqz     a10, .Lsec_110_208
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_interrupt_clr_event
        l32r     a8, .Lsec_110_48
        callx8   a8
        bbci     a2, 11, .Lsec_110_9d
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_process_panic_watchdog
        l32r     a8, .Lsec_110_4c
        callx8   a8
.Lsec_110_9d:
        movi.n   a4, 3
        slli     a4, a4, 21
        bnone    a2, a4, .Lsec_110_b0
        movi     a11, 1
        movi.n   a10, 14
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_post
        l32r     a8, .Lsec_110_50
        callx8   a8
.Lsec_110_b0:
        l32r     a4, .Lsec_110_4
        bany     a2, a4, .Lsec_110_b9
        j        .Lsec_110_1e7
.Lsec_110_b9:
        l32r     a4, .Lsec_110_8
        l8ui     a5, a4, 48
        beqz     a5, .Lsec_110_1e1
        l32i.n   a4, a4, 52
        bbsi     a4, 3, .Lsec_110_ca
        j        .Lsec_110_1e1
.Lsec_110_ca:
        l32r     a7, .Lsec_110_18
        mov.n    a12, a7
        l32r     a5, .Lsec_110_14
        mov.n    a13, a5
        l32r     a4, .Lsec_110_1c
        or       a11, a4, a4
        l32r     a6, .Lsec_110_20
        or       a10, a6, a6
        l32r     a15, .Lsec_110_c
        l32r     a14, .Lsec_110_10
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_rx_get_end_info
        l32r     a8, .Lsec_110_54
        callx8   a8
        movi     a10, 98
        l32r     a8, .Lsec_110_24
        l32i.n   a7, a7, 0
        memw
        l32i.n   a8, a8, 0
        beq      a7, a10, .Lsec_110_ff
        j        .Lsec_110_1e1
.Lsec_110_ff:
        l32r     a7, .Lsec_110_28
        l8ui     a10, a7, 0
        l32i.n   a6, a6, 0
        bnez.n   a10, .Lsec_110_120
        l32i.n   a4, a4, 0
        beq      a4, a6, .Lsec_110_111
        j        .Lsec_110_132
.Lsec_110_111:
        movi.n   a4, 1
        s8i      a4, a7, 0
        memw
        memw
        j        .Lsec_110_1e1
        .byte    0x00
.Lsec_110_120:
        movi.n   a10, -17
        bltu     a10, a6, .Lsec_110_12a
        l32i.n   a9, a4, 0
        bgeu     a9, a6, .Lsec_110_141
.Lsec_110_12a:
        movi.n   a4, 0
        s8i      a4, a7, 0
        memw
.Lsec_110_132:
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_sniffer_rx_clr_statistics
        l32r     a8, .Lsec_110_58
        callx8   a8
        j        .Lsec_110_1e1
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_110_141:
        beq      a9, a6, .Lsec_110_147
        j        .Lsec_110_1e1
.Lsec_110_147:
        l32r     a10, .Lsec_110_2c
        l32i.n   a7, a5, 0
        extui    a5, a7, 16, 16
        slli     a9, a5, 8
        l32i.n   a6, a10, 4
        l32r     a11, .Lsec_110_30
        and      a6, a6, a11
        or       a6, a6, a9
        l32r     a9, .Lsec_110_34
        s32i.n   a6, a10, 4
        bltu     a9, a5, .Lsec_110_1e1
        srli     a8, a8, 8
        extui    a5, a5, 0, 12
        l32r     a4, .Lsec_110_c
        s8i      a8, a10, 0
        memw
        l32i.n   a8, a4, 0
        l32r     a4, .Lsec_110_10
        l32i.n   a9, a4, 0
        extui    a9, a9, 30, 2
        slli     a9, a9, 14
        l32i.n   a4, a10, 0
        l32r     a11, .Lsec_110_38
        and      a4, a4, a11
        or       a4, a4, a9
        s32i.n   a4, a10, 0
        srli     a4, a7, 15
        slli     a4, a4, 7
        extui    a7, a7, 8, 7
        extui    a4, a4, 0, 8
        or       a4, a4, a7
        slli     a7, a8, 26
        movi.n   a8, 1
        slli     a8, a8, 27
        and      a7, a7, a8
        or       a4, a4, a7
        l32r     a7, .Lsec_110_3c
        and      a6, a6, a7
        or       a4, a4, a6
        s32i.n   a4, a10, 4
        movi.n   a4, 49
        slli     a4, a4, 25
        or       a5, a5, a4
        l32r     a6, .Lsec_110_40
        l32i     a4, a10, 24
        and      a4, a4, a6
        or       a5, a5, a4
        s32i     a5, a10, 24
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_push_promis_misc_buf
        l32r     a8, .Lsec_110_5c
        callx8   a8
        movi.n   a11, 0
        movi     a10, 26
        .reloc  ., R_XTENSA_ASM_EXPAND, pp_post
        l32r     a8, .Lsec_110_60
        callx8   a8
.Lsec_110_1e1:
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacProcessRxSucData
        l32r     a8, .Lsec_110_64
        callx8   a8
.Lsec_110_1e7:
        bbci     a2, 7, .Lsec_110_1f0
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacPostTxComplete
        l32r     a8, .Lsec_110_68
        callx8   a8
.Lsec_110_1f0:
        bbci     a2, 19, .Lsec_110_1f9
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacProcessAllTxTimeout
        l32r     a8, .Lsec_110_6c
        callx8   a8
.Lsec_110_1f9:
        bbsi     a2, 8, .Lsec_110_1ff
        j        .Lsec_110_82
.Lsec_110_1ff:
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacProcessCollisions
        l32r     a8, .Lsec_110_70
        callx8   a8
        j        .Lsec_110_82
.Lsec_110_208:
        l32i.n   a2, a3, 0
        l32i     a2, a2, 220
        callx8   a2
        retw.n
        .size   wDev_ProcessFiq, .-wDev_ProcessFiq
.Lsec_110_212:

        .section .iram1.47,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_111_0, g_osi_funcs_p
        .literal .Lsec_111_4, s_record_list
        .literal .Lsec_111_8, 0x3ff73088
        .literal .Lsec_111_c, 0x3ff73090
        .literal .Lsec_111_10, 0x3ff7308c
        .literal .Lsec_111_14, 0x3ff73098
        .literal .Lsec_111_18, wDevCtrl
        .literal .Lsec_111_1c, 0x3ff732f4
        .literal .Lsec_111_20, 0x3ff732fc
.Lsec_111_24:
        .global wdev_record_rx_linked_list
        .type   wdev_record_rx_linked_list, @function
wdev_record_rx_linked_list:
        entry    a1, 32
        bgeui    a2, 3, .Lsec_111_90
        l32r     a8, .Lsec_111_0
        l32i.n   a8, a8, 0
        l32i     a10, a8, 0x110
        callx8   a10
        addx4    a8, a2, a2
        addx2    a8, a8, a2
        l32r     a2, .Lsec_111_4
        addx4    a8, a8, a2
        l32r     a2, .Lsec_111_8
        s32i.n   a10, a8, 0
        memw
        l32i.n   a2, a2, 0
        s32i.n   a2, a8, 4
        l32r     a2, .Lsec_111_c
        memw
        l32i.n   a2, a2, 0
        s32i.n   a2, a8, 8
        l32r     a2, .Lsec_111_10
        memw
        l32i.n   a2, a2, 0
        s32i.n   a2, a8, 12
        l32r     a2, .Lsec_111_14
        memw
        l32i.n   a2, a2, 0
        s32i.n   a2, a8, 20
        l32r     a2, .Lsec_111_18
        l32i.n   a9, a2, 0
        l32i.n   a2, a2, 4
        s32i.n   a2, a8, 28
        l32i.n   a2, a2, 8
        s32i.n   a2, a8, 32
        l32r     a2, .Lsec_111_1c
        s32i.n   a9, a8, 24
        memw
        l32i.n   a2, a2, 0
        s32i.n   a2, a8, 36
        l32r     a2, .Lsec_111_20
        memw
        l32i     a2, a2, 0
        s32i     a2, a8, 40
.Lsec_111_90:
        memw
        retw.n
        .size   wdev_record_rx_linked_list, .-wdev_record_rx_linked_list
.Lsec_111_95:

        .section .iram1.48,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_112_0, .Lsec_136_0
        .literal .Lsec_112_4, s_record_list
        .literal .Lsec_112_8, .Lsec_135_0
        .literal .Lsec_112_c, .Lsec_134_0
        .literal .Lsec_112_10, wdev_record_rx_linked_list
        .literal .Lsec_112_14, wifi_log
        .literal .Lsec_112_18, wifi_log
        .literal .Lsec_112_1c, wifi_log
.Lsec_112_20:
        .global wdev_dump_rx_linked_list
        .type   wdev_dump_rx_linked_list, @function
wdev_dump_rx_linked_list:
        entry    a1, 48
        movi     a10, 2
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_record_rx_linked_list
        l32r     a8, .Lsec_112_10
        callx8   a8
        l32r     a13, .Lsec_112_0
        mov.n    a15, a3
        mov.n    a14, a2
        movi.n   a12, 3
        movi     a11, 32
        movi.n   a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_112_14
        callx8   a8
        l32r     a2, .Lsec_112_4
        movi     a3, 132
        add.n    a3, a2, a3
.Lsec_112_48:
        movi.n   a12, 3
        movi.n   a11, 32
        l32i.n   a8, a2, 20
        movi.n   a10, 1
        s32i.n   a8, a1, 12
        l32i.n   a8, a2, 16
        s32i.n   a8, a1, 8
        l32i.n   a8, a2, 12
        s32i.n   a8, a1, 4
        l32i.n   a8, a2, 8
        s32i.n   a8, a1, 0
        l32i.n   a15, a2, 4
        l32i.n   a14, a2, 0
        l32r     a13, .Lsec_112_8
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_112_18
        callx8   a8
        movi.n   a12, 3
        movi.n   a11, 32
        l32i.n   a8, a2, 40
        movi.n   a10, 1
        s32i.n   a8, a1, 8
        l32i.n   a8, a2, 36
        s32i.n   a8, a1, 4
        l32i.n   a8, a2, 32
        s32i.n   a8, a1, 0
        l32i.n   a15, a2, 28
        l32i     a14, a2, 24
        addi     a2, a2, 44
        l32r     a13, .Lsec_112_c
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_112_1c
        callx8   a8
        bne      a2, a3, .Lsec_112_48
        retw.n
        .size   wdev_dump_rx_linked_list, .-wdev_dump_rx_linked_list
.Lsec_112_99:

        .section .rodata.str1.1,"aMS",@progbits,1
        .align  1
.Lsec_113_0:
        .ascii  "wdev.c\000"

        .section .wifislprxiram.52,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_114_0, 0x000186a1
        .literal .Lsec_114_4, .Lsec_128_0
        .literal .Lsec_114_8, .Lsec_113_0
        .literal .Lsec_114_c, .Lsec_133_0
        .literal .Lsec_114_10, 0x00000ce5
        .literal .Lsec_114_14, 0xbfffffff
        .literal .Lsec_114_18, 0xdfffffff
        .literal .Lsec_114_1c, 0xff000fff
        .literal .Lsec_114_20, 0xdeadbeef
        .literal .Lsec_114_24, wDevCtrl
        .literal .Lsec_114_28, g_intr_lock_mux
        .literal .Lsec_114_2c, g_osi_funcs_p
        .literal .Lsec_114_30, wifi_log
        .literal .Lsec_114_34, wdev_record_rx_linked_list
        .literal .Lsec_114_38, hal_mac_rx_set_dscr_reload
        .literal .Lsec_114_3c, wdev_record_rx_linked_list
        .literal .Lsec_114_40, wdev_dump_rx_linked_list
        .literal .Lsec_114_44, hal_mac_rx_is_dscr_reload
        .literal .Lsec_114_48, hal_mac_rx_read_rxdscrnext
        .literal .Lsec_114_4c, hal_mac_rx_get_last_dscr
        .literal .Lsec_114_50, hal_mac_rx_set_base
.Lsec_114_54:
        .global wDev_AppendRxBlocks
        .type   wDev_AppendRxBlocks, @function
wDev_AppendRxBlocks:
        entry    a1, 64
        mov.n    a4, a2
        beqz.n   a2, .Lsec_114_6c
        l32r     a13, .Lsec_114_14
        l32r     a12, .Lsec_114_18
        l32r     a11, .Lsec_114_1c
        l32r     a10, .Lsec_114_20
        j        .Lsec_114_90
        .byte    0x00
        .byte    0x00
.Lsec_114_6c:
        l32r     a2, .Lsec_114_10
        movi.n   a12, 1
        slli     a11, a12, 11
        movi     a10, 6
        l32r     a15, .Lsec_114_4
        l32r     a14, .Lsec_114_8
        l32r     a13, .Lsec_114_c
        s32i     a2, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_114_30
        callx8   a8
.Lsec_114_8c:
        j        .Lsec_114_8c
        .byte    0x00
.Lsec_114_90:
        memw
        l32i.n   a5, a4, 0
        movi.n   a8, -1
        slli     a8, a8, 31
        or       a5, a5, a8
        memw
        s32i.n   a5, a4, 0
        memw
        l32i.n   a5, a4, 0
        and      a5, a5, a13
        memw
        s32i.n   a5, a4, 0
        memw
        l32i.n   a5, a4, 0
        and      a5, a5, a12
        memw
        s32i.n   a5, a4, 0
        memw
        l32i.n   a9, a4, 0
        extui    a9, a9, 0, 12
        slli     a9, a9, 12
        memw
        l32i.n   a8, a4, 0
        and      a8, a8, a11
        or       a8, a8, a9
        l32i.n   a5, a4, 4
        memw
        s32i.n   a8, a4, 0
        s32i.n   a10, a5, 0
        memw
        l32i.n   a8, a4, 0
        extui    a8, a8, 0, 12
        add.n    a5, a5, a8
        s32i.n   a10, a5, 0
        l32i.n   a4, a4, 8
        bnez     a4, .Lsec_114_90
        l32r     a5, .Lsec_114_24
        l32i.n   a6, a5, 0
        beqz     a6, .Lsec_114_178
        l32r     a7, .Lsec_114_2c
        l32r     a6, .Lsec_114_28
        l32i.n   a8, a7, 0
        l32i.n   a10, a6, 0
        l32i.n   a8, a8, 40
        memw
        callx8   a8
        mov.n    a11, a10
        mov.n    a10, a4
        s32i.n   a11, a1, 16
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_record_rx_linked_list
        l32r     a8, .Lsec_114_34
        callx8   a8
        l32i.n   a4, a5, 4
        s32i.n   a2, a4, 8
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_rx_set_dscr_reload
        l32r     a8, .Lsec_114_38
        callx8   a8
        movi.n   a10, 1
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_record_rx_linked_list
        l32r     a8, .Lsec_114_3c
        callx8   a8
        l32i.n   a4, a7, 0
        l32i.n   a11, a1, 16
        l32i.n   a4, a4, 44
        l32i.n   a10, a6, 0
        callx8   a4
        l32r     a4, .Lsec_114_0
        j        .Lsec_114_150
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_114_13e:
        addi.n   a4, a4, -1
        bnez.n   a4, .Lsec_114_150
        mov.n    a11, a3
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_dump_rx_linked_list
        l32r     a8, .Lsec_114_40
        callx8   a8
        j        .Lsec_114_159
        .byte    0x00
.Lsec_114_150:
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_rx_is_dscr_reload
        l32r     a8, .Lsec_114_44
        callx8   a8
        bnez     a10, .Lsec_114_13e
.Lsec_114_159:
        movi     a2, 0x3ff
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_rx_read_rxdscrnext
        l32r     a8, .Lsec_114_48
        callx8   a8
        slli     a2, a2, 20
        bne      a10, a2, .Lsec_114_185
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_rx_get_last_dscr
        l32r     a8, .Lsec_114_4c
        callx8   a8
        beq      a3, a10, .Lsec_114_185
        l32i.n   a10, a10, 8
        beqz.n   a10, .Lsec_114_185
        j        .Lsec_114_17c
.Lsec_114_178:
        s32i.n   a2, a5, 0
        mov.n    a10, a2
.Lsec_114_17c:
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_rx_set_base
        l32r     a8, .Lsec_114_50
        callx8   a8
.Lsec_114_185:
        s32i.n   a3, a5, 4
        memw
        retw.n
        .size   wDev_AppendRxBlocks, .-wDev_AppendRxBlocks
.Lsec_114_18c:

        .section .wifislpiram.24,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_115_0, wDevCtrl
        .literal .Lsec_115_4, wDev_AppendRxBlocks
.Lsec_115_8:
        .global wDev_DiscardFrame
        .type   wDev_DiscardFrame, @function
wDev_DiscardFrame:
        entry    a1, 32
        l32r     a8, .Lsec_115_0
        mov.n    a11, a2
        extui    a12, a3, 0, 16
        l32i.n   a9, a2, 8
        l32i.n   a10, a8, 0
        s32i.n   a9, a8, 0
        movi.n   a8, 0
        s32i.n   a8, a2, 8
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wDev_AppendRxBlocks
        l32r     a8, .Lsec_115_4
        callx8   a8
        retw.n
        .size   wDev_DiscardFrame, .-wDev_DiscardFrame
.Lsec_115_28:

        .section .wifislprxiram.21,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_116_0, wDevCtrl
        .literal .Lsec_116_4, pTxRx
        .literal .Lsec_116_8, 0xff000fff
        .literal .Lsec_116_c, esf_buf_alloc
        .literal .Lsec_116_10, esf_buf_alloc
        .literal .Lsec_116_14, wifi_gpio_debug
        .literal .Lsec_116_18, wifi_gpio_debug
        .literal .Lsec_116_1c, wDev_DiscardFrame
        .literal .Lsec_116_20, wdev_csi_len_align
        .literal .Lsec_116_24, wdev_csi_hw_bug_check
        .literal .Lsec_116_28, memcpy
        .literal .Lsec_116_2c, memcpy
        .literal .Lsec_116_30, memcpy
        .literal .Lsec_116_34, memcpy
        .literal .Lsec_116_38, memcpy
        .literal .Lsec_116_3c, memcpy
        .literal .Lsec_116_40, wdev_csi_rx_process
        .literal .Lsec_116_44, wDev_DiscardFrame
        .literal .Lsec_116_48, lmacRxDone
.Lsec_116_4c:
        .global wDev_IndicateFrame
        .type   wDev_IndicateFrame, @function
wDev_IndicateFrame:
        entry    a1, 64
        extui    a3, a3, 0, 8
        s32i.n   a6, a1, 12
        extui    a2, a2, 0, 8
        extui    a5, a5, 0, 16
        s32i.n   a3, a1, 8
        beqz     a2, .Lsec_116_a5
        beqi     a5, 1, .Lsec_116_68
        memw
        j        .Lsec_116_d6
.Lsec_116_68:
        l32r     a3, .Lsec_116_0
        movi     a7, 0x1f4
        l32i.n   a6, a3, 0
        memw
        l32i.n   a3, a6, 0
        extui    a3, a3, 12, 12
        bgeu     a7, a3, .Lsec_116_8c
.Lsec_116_7b:
        movi.n   a11, 7
        l32r     a3, .Lsec_116_4
        movi.n   a10, 0
        l32i.n   a3, a3, 0
        l32i     a12, a3, 0x3c4
        j        .Lsec_116_b4
        .byte    0x00
        .byte    0x00
.Lsec_116_8c:
        memw
        l32i.n   a12, a6, 0
        movi.n   a11, 8
        extui    a12, a12, 12, 12
        movi.n   a10, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, esf_buf_alloc
        l32r     a8, .Lsec_116_c
        callx8   a8
        mov.n    a7, a10
        bnez.n   a10, .Lsec_116_bc
        j        .Lsec_116_7b
.Lsec_116_a5:
        l32r     a3, .Lsec_116_4
        movi.n   a11, 7
        mov.n    a10, a2
        l32i.n   a3, a3, 0
        l32i     a12, a3, 0x3c4
        mull     a12, a5, a12
.Lsec_116_b4:
        .reloc  ., R_XTENSA_ASM_EXPAND, esf_buf_alloc
        l32r     a8, .Lsec_116_10
        callx8   a8
        mov.n    a7, a10
.Lsec_116_bc:
        addi.n   a2, a2, 2
        or       a11, a2, a2
        movi     a10, 11
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_gpio_debug
        l32r     a8, .Lsec_116_14
        callx8   a8
        bnez.n   a7, .Lsec_116_e5
        mov.n    a11, a2
        movi.n   a10, 12
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_gpio_debug
        l32r     a8, .Lsec_116_18
        callx8   a8
.Lsec_116_d6:
        mov.n    a11, a5
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, wDev_DiscardFrame
        l32r     a8, .Lsec_116_1c
        callx8   a8
        j        .Lsec_116_2e4
        .byte    0x00
        .byte    0x00
.Lsec_116_e5:
        l32r     a2, .Lsec_116_0
        l32i.n   a3, a2, 0
        l32i.n   a6, a3, 4
        mov.n    a10, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_csi_len_align
        l32r     a8, .Lsec_116_20
        callx8   a8
        movi.n   a8, 0
        extui    a2, a10, 0, 16
        s32i.n   a8, a1, 0
        beq      a2, a8, .Lsec_116_154
        extui    a9, a10, 0, 16
        addi     a9, a9, 24
        addi     a12, a6, 24
        add.n    a9, a6, a9
        mov.n    a11, a9
        mov.n    a10, a12
        s32i.n   a9, a1, 20
        s32i.n   a12, a1, 16
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_csi_hw_bug_check
        l32r     a8, .Lsec_116_24
        callx8   a8
        s32i.n   a10, a1, 0
        l32i.n   a9, a1, 20
        l32i.n   a12, a1, 16
        bnei     a10, 1, .Lsec_116_14a
        l8ui     a11, a6, 24
        l8ui     a10, a12, 1
        s8i      a11, a9, 0
        memw
        l8ui     a11, a12, 2
        s8i      a10, a9, 1
        memw
        l8ui     a10, a12, 3
        s8i      a11, a9, 2
        s8i      a10, a9, 3
        memw
        memw
        j        .Lsec_116_154
.Lsec_116_14a:
        l32i.n   a8, a1, 0
        movi.n   a10, 0
        addi     a9, a8, -2
        moveqz   a2, a10, a9
.Lsec_116_154:
        movi.n   a10, 1
        l32i.n   a9, a7, 4
        l32i.n   a11, a3, 4
        l32i.n   a9, a9, 4
        beq      a4, a3, .Lsec_116_16e
        addi     a11, a2, 24
        extui    a11, a11, 0, 16
        s32i.n   a11, a1, 4
        memw
        j        .Lsec_116_1ae
        .byte    0x00
.Lsec_116_16e:
        movi.n   a12, 24
        mov.n    a10, a9
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_116_28
        callx8   a8
        memw
        l32i.n   a12, a4, 0
        addi     a11, a2, 24
        extui    a12, a12, 12, 12
        addi     a12, a12, -24
        sub      a12, a12, a2
        l32i     a3, a4, 4
        addi     a10, a10, 24
        add      a11, a3, a11
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_116_2c
        callx8   a8
        memw
        l32i.n   a3, a4, 0
        memw
        l32i.n   a3, a4, 0
        extui    a3, a3, 12, 12
        sub      a3, a3, a2
        l32i.n   a10, a7, 4
        j        .Lsec_116_23d
        .byte    0x00
.Lsec_116_1ae:
        l32i.n   a11, a3, 4
        beqz.n   a10, .Lsec_116_1ec
        movi.n   a12, 24
        mov.n    a10, a9
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_116_30
        callx8   a8
        l32r     a8, .Lsec_116_4
        addi     a9, a10, 24
        mov.n    a10, a9
        l32i.n   a13, a8, 0
        l32i.n   a6, a3, 4
        l32i     a12, a13, 0x3c4
        l32i.n   a8, a1, 4
        sub      a12, a12, a8
        add.n    a11, a6, a8
        s32i.n   a13, a1, 16
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_116_34
        callx8   a8
        mov.n    a9, a10
        l32i.n   a13, a1, 16
        l32i.n   a8, a1, 4
        l32i     a10, a13, 0x3c4
        sub      a10, a10, a8
        j        .Lsec_116_206
.Lsec_116_1ec:
        l32r     a8, .Lsec_116_4
        l32i.n   a10, a8, 0
        l32i     a12, a10, 0x3c4
        mov.n    a10, a9
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_116_38
        callx8   a8
        mov.n    a9, a10
        l32r     a8, .Lsec_116_4
        l32i.n   a10, a8, 0
        l32i     a10, a10, 0x3c4
.Lsec_116_206:
        add.n    a9, a9, a10
        movi.n   a10, 0
        l32i.n   a3, a3, 8
        bne      a4, a3, .Lsec_116_1ae
        mov.n    a10, a9
        memw
        l32i.n   a12, a4, 0
        extui    a12, a12, 12, 12
        l32i.n   a11, a4, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_116_3c
        callx8   a8
        addi.n   a11, a5, -1
        l32r     a8, .Lsec_116_4
        l32i.n   a9, a8, 0
        l32i     a9, a9, 0x3c4
        mul16u   a9, a9, a11
        sub      a9, a9, a2
        memw
        l32i.n   a3, a4, 0
        extui    a3, a3, 12, 12
        add.n    a3, a3, a9
        l32i.n   a10, a7, 4
.Lsec_116_23d:
        extui    a3, a3, 0, 12
        slli     a3, a3, 12
        memw
        l32i.n   a9, a10, 0
        l32r     a11, .Lsec_116_8
        and      a9, a9, a11
        or       a3, a9, a3
        memw
        s32i.n   a3, a10, 0
        l32r     a3, .Lsec_116_0
        l8ui     a9, a3, 48
        bnei     a9, 1, .Lsec_116_264
        l32i.n   a6, a6, 0
        bltz     a6, .Lsec_116_284
.Lsec_116_264:
        beqz.n   a2, .Lsec_116_284
        mov.n    a11, a2
        l32i.n   a12, a7, 16
        addi     a13, a12, 28
        l32i.n   a6, a3, 0
        l32i.n   a14, a12, 24
        extui    a14, a14, 0, 12
        l32i.n   a10, a6, 4
        addi     a10, a10, 24
        l32i.n   a15, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_csi_rx_process
        l32r     a8, .Lsec_116_40
        callx8   a8
.Lsec_116_284:
        mov.n    a10, a4
        mov.n    a11, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, wDev_DiscardFrame
        l32r     a8, .Lsec_116_44
        callx8   a8
        l32i.n   a8, a1, 12
        l32i.n   a4, a7, 44
        s32i.n   a8, a4, 4
        l8ui     a2, a3, 44
        s8i      a2, a4, 8
        memw
        l8ui     a2, a3, 45
        movi     a3, 0x100
        s8i      a2, a4, 9
        memw
        l32i.n   a2, a4, 0
        extui    a2, a2, 0, 12
        beqi     a5, 1, .Lsec_116_2b4
        movi     a3, 128
.Lsec_116_2b4:
        l32i.n   a8, a1, 8
        or       a2, a2, a3
        extui    a2, a2, 0, 16
        l32i.n   a3, a4, 0
        bnez.n   a8, .Lsec_116_2c6
        memw
        j        .Lsec_116_2cb
.Lsec_116_2c6:
        movi.n   a5, 16
        or       a2, a2, a5
.Lsec_116_2cb:
        movi.n   a5, -1
        slli     a5, a5, 12
        and      a3, a3, a5
        or       a3, a3, a2
        s32i.n   a3, a4, 0
        or       a10, a7, a7
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacRxDone
        l32r     a8, .Lsec_116_48
        callx8   a8
.Lsec_116_2e4:
        retw.n
        .size   wDev_IndicateFrame, .-wDev_IndicateFrame
.Lsec_116_2e6:

        .section .wifirxiram.23,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_117_0, wDevCtrl
        .literal .Lsec_117_4, wdev_csi_len_align
        .literal .Lsec_117_8, wdev_csi_hw_bug_check
        .literal .Lsec_117_c, wDev_RxSecError
        .literal .Lsec_117_10, roundup2
        .literal .Lsec_117_14, esf_buf_alloc
        .literal .Lsec_117_18, memcpy
        .literal .Lsec_117_1c, memcpy
        .literal .Lsec_117_20, esf_buf_recycle
        .literal .Lsec_117_24, memcpy
        .literal .Lsec_117_28, roundup2
        .literal .Lsec_117_2c, memcpy
        .literal .Lsec_117_30, roundup2
        .literal .Lsec_117_34, wdev_csi_rx_process
        .literal .Lsec_117_38, wDev_SnifferRxAmpdu
        .literal .Lsec_117_3c, lmacRxDone
        .literal .Lsec_117_40, esf_buf_recycle
        .literal .Lsec_117_44, wDev_DiscardFrame
.Lsec_117_48:
        .global wDev_IndicateAmpdu
        .type   wDev_IndicateAmpdu, @function
wDev_IndicateAmpdu:
        entry    a1, 64
        s32i.n   a2, a1, 12
        extui    a3, a3, 0, 16
        movi.n   a5, 0
        l32r     a2, .Lsec_117_0
        l32i.n   a2, a2, 0
        s32i.n   a3, a1, 16
        l32i.n   a3, a2, 4
        mov.n    a10, a3
        s32i.n   a4, a1, 20
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_csi_len_align
        l32r     a8, .Lsec_117_4
        callx8   a8
        extui    a4, a10, 0, 16
        extui    a8, a10, 0, 16
        addi     a7, a8, 24
        s32i.n   a4, a1, 0
        s32i.n   a5, a1, 4
        bne      a4, a5, .Lsec_117_7b
        j        .Lsec_117_2d2
.Lsec_117_7b:
        addi     a6, a3, 24
        add.n    a4, a3, a7
        mov.n    a11, a4
        mov.n    a10, a6
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_csi_hw_bug_check
        l32r     a8, .Lsec_117_8
        callx8   a8
        s32i.n   a10, a1, 4
        bnei     a10, 1, .Lsec_117_b9
        l8ui     a9, a3, 24
        l8ui     a10, a3, 25
        s8i      a9, a4, 0
        memw
        l8ui     a9, a3, 26
        s8i      a10, a4, 1
        memw
        l8ui     a6, a3, 27
        s8i      a9, a4, 2
        s8i      a6, a4, 3
        memw
        memw
        j        .Lsec_117_2d2
.Lsec_117_b9:
        l32i.n   a6, a1, 4
        addi     a4, a6, -2
        movi.n   a6, 0
        l32i.n   a8, a1, 0
        moveqz   a8, a6, a4
        movi.n   a6, 24
        moveqz   a7, a6, a4
        s32i.n   a8, a1, 0
        memw
        j        .Lsec_117_2d2
.Lsec_117_d2:
        l32i.n   a13, a2, 4
        movi     a9, 0x627
        add.n    a13, a13, a7
        l32i.n   a12, a13, 0
        extui    a5, a12, 0, 12
        addi     a6, a5, -24
        mov.n    a4, a5
        bgeu     a9, a6, .Lsec_117_e9
        j        .Lsec_117_30d
.Lsec_117_e9:
        memw
        l32i.n   a9, a2, 0
        addi.n   a6, a7, 4
        extui    a9, a9, 12, 12
        bgeu     a6, a9, .Lsec_117_fd
        l16ui    a9, a13, 4
        j        .Lsec_117_104
        .byte    0x00
.Lsec_117_fd:
        l32i.n   a9, a2, 8
        l32i.n   a9, a9, 4
        l16ui    a9, a9, 0
.Lsec_117_104:
        bbci     a9, 14, .Lsec_117_114
        movi.n   a9, 7
        slli     a9, a9, 28
        l32i.n   a10, a3, 0
        bnone    a10, a9, .Lsec_117_114
        addi     a4, a5, -8
.Lsec_117_114:
        l8ui     a10, a13, 3
        beqz     a10, .Lsec_117_15c
        s32i.n   a13, a1, 28
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wDev_RxSecError
        l32r     a8, .Lsec_117_c
        callx8   a8
        l32i.n   a13, a1, 28
        bnez.n   a10, .Lsec_117_15c
        add.n    a10, a4, a7
        memw
        l32i.n   a4, a2, 0
        extui    a4, a4, 12, 12
        bltu     a10, a4, .Lsec_117_14e
        memw
        l32i.n   a4, a2, 0
        bbci     a4, 30, .Lsec_117_141
        j        .Lsec_117_308
.Lsec_117_141:
        memw
        l32i.n   a4, a2, 0
        extui    a4, a4, 12, 12
        sub      a10, a10, a4
        l32i.n   a2, a2, 8
.Lsec_117_14e:
        movi.n   a11, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, roundup2
        l32r     a8, .Lsec_117_10
        callx8   a8
        mov.n    a7, a10
        j        .Lsec_117_2e4
        .byte    0x00
.Lsec_117_15c:
        addi     a12, a5, 28
        extui    a12, a12, 0, 12
        movi.n   a11, 7
        movi.n   a10, 0
        s32i.n   a13, a1, 28
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, esf_buf_alloc
        l32r     a8, .Lsec_117_14
        callx8   a8
        mov.n    a5, a10
        beqz     a10, .Lsec_117_30d
        movi.n   a12, 24
        l32i.n   a10, a10, 4
        mov.n    a11, a3
        l32i.n   a10, a10, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_117_18
        callx8   a8
        l32i.n   a13, a1, 28
        l32i.n   a10, a5, 4
        l8ui     a12, a13, 0
        l32i.n   a11, a10, 4
        addi     a10, a11, 24
        l8ui     a14, a13, 1
        s8i      a12, a11, 24
        memw
        l8ui     a12, a13, 2
        s8i      a14, a11, 25
        memw
        l8ui     a11, a13, 3
        s8i      a12, a10, 2
        s8i      a11, a10, 3
        memw
        add.n    a11, a4, a7
        memw
        l32i.n   a10, a2, 0
        extui    a10, a10, 12, 12
        bltu     a11, a10, .Lsec_117_228
        memw
        l32i.n   a11, a2, 0
        extui    a11, a11, 12, 12
        sub      a13, a11, a7
        sub      a11, a7, a11
        l32i.n   a10, a5, 4
        add.n    a7, a11, a4
        addi     a12, a13, -4
        l32i.n   a10, a10, 4
        addi     a10, a10, 28
        l32i.n   a11, a2, 4
        add.n    a11, a11, a6
        s32i.n   a13, a1, 28
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_117_1c
        callx8   a8
        memw
        l32i.n   a6, a2, 0
        extui    a6, a6, 30, 1
        l32i.n   a13, a1, 28
        bnei     a6, 1, .Lsec_117_200
        beqz     a7, .Lsec_117_24a
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esf_buf_recycle
        l32r     a8, .Lsec_117_20
        callx8   a8
        j        .Lsec_117_308
        .byte    0x00
.Lsec_117_200:
        l32i.n   a2, a2, 8
        beqz     a7, .Lsec_117_24a
        addi     a13, a13, 24
        mov.n    a12, a7
        l32i.n   a10, a5, 4
        l32i.n   a10, a10, 4
        add.n    a10, a10, a13
        l32i.n   a11, a2, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_117_24
        callx8   a8
        mov.n    a10, a7
        movi.n   a11, 4
        .reloc  ., R_XTENSA_ASM_EXPAND, roundup2
        l32r     a8, .Lsec_117_28
        callx8   a8
        mov.n    a7, a10
        j        .Lsec_117_24a
        .byte    0x00
.Lsec_117_228:
        l32i.n   a10, a5, 4
        addi     a12, a4, -4
        l32i.n   a10, a10, 4
        addi     a10, a10, 28
        l32i.n   a11, a2, 4
        add.n    a11, a11, a6
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_117_2c
        callx8   a8
        movi.n   a11, 4
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, roundup2
        l32r     a8, .Lsec_117_30
        callx8   a8
        add.n    a7, a7, a10
        movi.n   a6, 0
.Lsec_117_24a:
        l32i.n   a8, a1, 24
        beqz.n   a8, .Lsec_117_272
        l32i.n   a8, a1, 8
        beqz.n   a8, .Lsec_117_272
        mov.n    a14, a4
        movi.n   a4, 0
        l32r     a10, .Lsec_117_0
        l32i.n   a10, a10, 0
        l32i.n   a12, a5, 16
        addi     a13, a12, 28
        l32i.n   a10, a10, 4
        addi     a10, a10, 24
        l32i     a15, a1, 4
        l32i.n   a11, a1, 0
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_csi_rx_process
        l32r     a8, .Lsec_117_34
        callx8   a8
        s32i.n   a4, a1, 8
.Lsec_117_272:
        movi.n   a12, 16
        l32i.n   a4, a5, 44
        l32i.n   a8, a1, 20
        l32r     a10, .Lsec_117_0
        s32i.n   a8, a4, 4
        l8ui     a11, a10, 44
        s8i      a11, a4, 8
        memw
        l8ui     a11, a10, 45
        s8i      a11, a4, 9
        memw
        l32i.n   a11, a4, 0
        or       a11, a11, a12
        s32i.n   a11, a4, 0
        l8ui     a4, a10, 48
        bnei     a4, 1, .Lsec_117_2ae
        l32i     a4, a3, 0
        bgez     a4, .Lsec_117_2ae
        or       a10, a5, a5
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wDev_SnifferRxAmpdu
        l32r     a8, .Lsec_117_38
        callx8   a8
.Lsec_117_2ae:
        movi.n   a4, 7
        l32i.n   a10, a3, 0
        slli     a4, a4, 28
        bnone    a10, a4, .Lsec_117_2c4
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, lmacRxDone
        l32r     a8, .Lsec_117_3c
        callx8   a8
        j        .Lsec_117_2cc
        .byte    0x00
.Lsec_117_2c4:
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, esf_buf_recycle
        l32r     a8, .Lsec_117_40
        callx8   a8
.Lsec_117_2cc:
        beqz.n   a6, .Lsec_117_2e4
        j        .Lsec_117_308
        .byte    0x00
.Lsec_117_2d2:
        movi.n   a4, 1
        s32i.n   a4, a1, 8
        movi.n   a4, 0
        l32i.n   a5, a1, 0
        l32i.n   a6, a1, 8
        movnez   a4, a6, a5
        extui    a4, a4, 0, 8
        s32i.n   a4, a1, 24
.Lsec_117_2e4:
        memw
        l32i.n   a4, a2, 0
        bbsi     a4, 30, .Lsec_117_2ef
        j        .Lsec_117_d2
.Lsec_117_2ef:
        memw
        l32i.n   a4, a2, 0
        bbci     a4, 30, .Lsec_117_308
        addi     a6, a7, 24
        memw
        l32i.n   a4, a2, 0
        extui    a4, a4, 12, 12
        bgeu     a6, a4, .Lsec_117_308
        j        .Lsec_117_d2
.Lsec_117_308:
        memw
        l32i.n   a2, a2, 0
.Lsec_117_30d:
        l32i.n   a11, a1, 16
        l32i.n   a10, a1, 12
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wDev_DiscardFrame
        l32r     a8, .Lsec_117_44
        callx8   a8
        retw.n
        .size   wDev_IndicateAmpdu, .-wDev_IndicateAmpdu
.Lsec_117_31c:

        .section .wifislprxiram.27,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_118_0, wDevCtrl
        .literal .Lsec_118_4, g_lmac_cnt
        .literal .Lsec_118_8, wifi_sta_rx_probe_req
        .literal .Lsec_118_c, g_osi_funcs_p
        .literal .Lsec_118_10, .Lsec_130_0
        .literal .Lsec_118_14, .Lsec_113_0
        .literal .Lsec_118_18, .Lsec_151_0
        .literal .Lsec_118_1c, 0x00000904
        .literal .Lsec_118_20, wDev_GetNoiseFloor
        .literal .Lsec_118_24, wdev_csi_len_align
        .literal .Lsec_118_28, wdev_csi_hw_bug_check
        .literal .Lsec_118_2c, wDev_SnifferRxData
        .literal .Lsec_118_30, wDev_SnifferRxData
        .literal .Lsec_118_34, ic_interface_enabled
        .literal .Lsec_118_38, ic_interface_enabled
        .literal .Lsec_118_3c, wDev_ProcessRxData_NAN_Interface_Hook
        .literal .Lsec_118_40, ic_interface_enabled
        .literal .Lsec_118_44, wDev_ProcessRxData_NAN_Interface_Hook
        .literal .Lsec_118_48, wDev_IndicateCtrlFrame
        .literal .Lsec_118_4c, ic_interface_enabled
        .literal .Lsec_118_50, ic_interface_enabled
        .literal .Lsec_118_54, ic_interface_enabled
        .literal .Lsec_118_58, wDev_DiscardFrame
        .literal .Lsec_118_5c, wifi_log
        .literal .Lsec_118_60, wDev_IndicateAmpdu
        .literal .Lsec_118_64, wDev_IndicateFrame
.Lsec_118_68:
        .global wDev_ProcessRxSucData
        .type   wDev_ProcessRxSucData, @function
wDev_ProcessRxSucData:
        entry    a1, 64
        l32r     a9, .Lsec_118_0
        extui    a3, a3, 0, 16
        s32i.n   a4, a1, 20
        l32i     a4, a9, 0
        l32i.n   a5, a4, 4
        l8ui     a6, a9, 46
        l8ui     a4, a5, 0
        sub      a4, a4, a6
        s8i      a4, a5, 0
        memw
        l8ui     a4, a9, 45
        slli     a4, a4, 4
        l8ui     a6, a9, 44
        extui    a6, a6, 0, 4
        or       a4, a4, a6
        s8i      a4, a5, 10
        memw
        s32i     a9, a1, 28
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wDev_GetNoiseFloor
        l32r     a8, .Lsec_118_20
        callx8   a8
        s8i      a10, a5, 8
        memw
        mov.n    a10, a5
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_csi_len_align
        l32r     a8, .Lsec_118_24
        callx8   a8
        extui    a6, a10, 0, 16
        l32i.n   a9, a1, 28
        beqz.n   a6, .Lsec_118_ef
        l32i.n   a4, a9, 0
        l32i.n   a7, a4, 4
        extui    a4, a10, 0, 16
        addi     a4, a4, 24
        add.n    a4, a7, a4
        mov.n    a11, a4
        l8ui     a12, a7, 27
        addi     a10, a7, 24
        s32i     a12, a1, 24
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wdev_csi_hw_bug_check
        l32r     a8, .Lsec_118_28
        callx8   a8
        l32i.n   a9, a1, 28
        l32i.n   a12, a1, 24
        beqi     a10, 2, .Lsec_118_f6
        bnez.n   a10, .Lsec_118_f8
        l8ui     a12, a4, 3
        j        .Lsec_118_f8
.Lsec_118_ef:
        l8ui     a12, a5, 27
        j        .Lsec_118_f8
        .byte    0x00
.Lsec_118_f6:
        movi.n   a6, 0
.Lsec_118_f8:
        movi.n   a7, 0
        movi     a10, -245
        movi.n   a8, 1
        mov.n    a4, a7
        add.n    a10, a12, a10
        movnez   a4, a8, a12
        movnez   a7, a8, a10
        and      a4, a4, a7
        beqz.n   a4, .Lsec_118_138
        addi     a4, a12, -65
        beqz.n   a4, .Lsec_118_119
        addi     a4, a12, -98
        bnez     a4, .Lsec_118_35e
.Lsec_118_119:
        l8ui     a4, a9, 48
        beqz     a4, .Lsec_118_35e
        l32i.n   a4, a9, 52
        bbsi     a4, 3, .Lsec_118_129
        j        .Lsec_118_35e
        .byte    0x00
        .byte    0x00
.Lsec_118_129:
        extui    a11, a3, 0, 8
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, wDev_SnifferRxData
        l32r     a8, .Lsec_118_2c
        callx8   a8
        j        .Lsec_118_35e
        .byte    0x00
.Lsec_118_138:
        l32i.n   a7, a5, 4
        slli     a10, a8, 27
        and      a7, a7, a10
        mov.n    a10, a4
        addi     a6, a6, 28
        l8ui     a11, a9, 48
        add.n    a6, a5, a6
        addi.n   a11, a11, -1
        moveqz   a10, a8, a11
        extui    a10, a10, 0, 8
        l16ui    a13, a6, 0
        beqz.n   a10, .Lsec_118_17a
        movnez   a8, a4, a7
        extui    a8, a8, 0, 8
        beqz.n   a8, .Lsec_118_17a
        l32i.n   a8, a5, 0
        bgez     a8, .Lsec_118_17a
        extui    a11, a3, 0, 8
        mov.n    a10, a2
        s32i.n   a9, a1, 28
        s32i.n   a13, a1, 24
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wDev_SnifferRxData
        l32r     a8, .Lsec_118_30
        callx8   a8
        l32i.n   a13, a1, 24
        l32i.n   a9, a1, 28
.Lsec_118_17a:
        movi.n   a8, 4
        l8ui     a10, a6, 1
        and      a8, a10, a8
        s32i.n   a8, a1, 16
        l32i.n   a10, a5, 0
        beqz.n   a7, .Lsec_118_1c2
        movi.n   a6, 3
        slli     a6, a6, 28
        and      a6, a10, a6
        movi.n   a8, 1
        movi.n   a11, 0
        l8ui     a9, a9, 48
        movnez   a8, a11, a6
        extui    a8, a8, 0, 8
        or       a6, a4, a4
        bnei     a9, 1, .Lsec_118_1ac
        or       a6, a9, a9
        movltz   a8, a4, a10
        movgez   a6, a4, a10
.Lsec_118_1ac:
        l32r     a10, .Lsec_118_4
        l32i     a9, a10, 160
        addi.n   a9, a9, 1
        s32i     a9, a10, 160
        bnez     a6, .Lsec_118_371
        memw
        j        .Lsec_118_31d
        .byte    0x00
        .byte    0x00
.Lsec_118_1c2:
        movi.n   a8, 7
        slli     a8, a8, 28
        bany     a10, a8, .Lsec_118_1cd
        j        .Lsec_118_3d4
.Lsec_118_1cd:
        extui    a9, a13, 0, 4
        bnei     a9, 4, .Lsec_118_1d6
        j        .Lsec_118_28d
.Lsec_118_1d6:
        bnei     a9, 8, .Lsec_118_1dc
        j        .Lsec_118_2ed
.Lsec_118_1dc:
        movi.n   a8, 1
        bnez     a9, .Lsec_118_31f
        movi     a8, 240
        and      a8, a13, a8
        bnei     a8, 64, .Lsec_118_228
        l8ui     a8, a5, 3
        movi     a9, -49
        and      a8, a8, a9
        movi.n   a9, 32
        or       a8, a8, a9
        s8i      a8, a5, 3
        memw
        mov.n    a10, a7
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, ic_interface_enabled
        l32r     a8, .Lsec_118_34
        callx8   a8
        beqz     a10, .Lsec_118_278
        l32r     a8, .Lsec_118_8
        l32i.n   a8, a8, 0
        beqz     a8, .Lsec_118_278
        mov.n    a10, a6
        l8ui     a12, a5, 0
        sext     a12, a12, 7
        l32i.n   a11, a5, 24
        extui    a11, a11, 0, 12
        callx8   a8
        j        .Lsec_118_278
        .byte    0x00
.Lsec_118_228:
        beqi     a8, 128, .Lsec_118_234
        movi     a9, 208
        bne      a8, a9, .Lsec_118_278
        j        .Lsec_118_264
.Lsec_118_234:
        movi.n   a10, 2
        .reloc  ., R_XTENSA_ASM_EXPAND, ic_interface_enabled
        l32r     a8, .Lsec_118_38
        callx8   a8
        beqz.n   a10, .Lsec_118_278
        mov.n    a11, a6
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, wDev_ProcessRxData_NAN_Interface_Hook
        l32r     a8, .Lsec_118_3c
        callx8   a8
        bnez.n   a10, .Lsec_118_278
        l8ui     a6, a5, 3
        movi     a8, -49
        and      a6, a6, a8
        movi.n   a8, 16
        or       a6, a6, a8
        s8i      a6, a5, 3
        memw
        memw
        j        .Lsec_118_278
.Lsec_118_264:
        movi.n   a10, 2
        .reloc  ., R_XTENSA_ASM_EXPAND, ic_interface_enabled
        l32r     a8, .Lsec_118_40
        callx8   a8
        beqz.n   a10, .Lsec_118_278
        mov.n    a11, a6
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, wDev_ProcessRxData_NAN_Interface_Hook
        l32r     a8, .Lsec_118_44
        callx8   a8
.Lsec_118_278:
        l32r     a8, .Lsec_118_4
        l32i     a6, a8, 168
        addi.n   a6, a6, 1
        s32i     a6, a8, 168
        mov.n    a8, a4
        movi.n   a6, 1
        memw
        j        .Lsec_118_3d8
.Lsec_118_28d:
        l32r     a8, .Lsec_118_4
        l32i     a6, a8, 172
        addi.n   a6, a6, 1
        s32i     a6, a8, 172
        movi     a6, 240
        and      a6, a13, a6
        movi     a8, 176
        bne      a6, a8, .Lsec_118_2a7
        j        .Lsec_118_3c8
.Lsec_118_2a7:
        bltu     a8, a6, .Lsec_118_2c5
        movi     a8, 160
        bne      a6, a8, .Lsec_118_2b3
        j        .Lsec_118_3e4
.Lsec_118_2b3:
        movi     a6, 224
        and      a6, a13, a6
        bnei     a6, 128, .Lsec_118_2bf
        j        .Lsec_118_3e4
.Lsec_118_2bf:
        memw
        j        .Lsec_118_2d8
.Lsec_118_2c5:
        movi     a6, 224
        and      a6, a13, a6
        movi     a10, 192
        mov.n    a9, a4
        mov.n    a8, a4
        bne      a6, a10, .Lsec_118_2d8
        j        .Lsec_118_3cc
.Lsec_118_2d8:
        mov.n    a12, a3
        mov.n    a11, a2
        mov.n    a10, a13
        .reloc  ., R_XTENSA_ASM_EXPAND, wDev_IndicateCtrlFrame
        l32r     a8, .Lsec_118_48
        callx8   a8
        mov.n    a8, a10
        mov.n    a9, a4
        j        .Lsec_118_3cc
        .byte    0x00
        .byte    0x00
.Lsec_118_2ed:
        movi     a8, 112
        and      a13, a13, a8
        movi.n   a9, 1
        beqi     a13, 64, .Lsec_118_30c
        l32r     a8, .Lsec_118_c
        l32i.n   a8, a8, 0
        l32i.n   a10, a8, 4
        callx8   a10
        mov.n    a9, a4
        bnez.n   a10, .Lsec_118_30c
        l8ui     a9, a6, 4
        extui    a9, a9, 0, 1
.Lsec_118_30c:
        l32r     a8, .Lsec_118_4
        l32i     a6, a8, 164
        addi.n   a6, a6, 1
        s32i     a6, a8, 164
        memw
        j        .Lsec_118_3ca
.Lsec_118_31d:
        mov.n    a4, a6
.Lsec_118_31f:
        movi.n   a10, 1
        s32i.n   a8, a1, 24
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, ic_interface_enabled
        l32r     a8, .Lsec_118_4c
        callx8   a8
        l32i.n   a8, a1, 24
        beqz.n   a10, .Lsec_118_335
        l32i.n   a6, a5, 0
        bbsi     a6, 29, .Lsec_118_371
.Lsec_118_335:
        movi.n   a10, 0
        s32i.n   a8, a1, 24
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, ic_interface_enabled
        l32r     a8, .Lsec_118_50
        callx8   a8
        l32i.n   a8, a1, 24
        beqz     a10, .Lsec_118_34d
        l32i     a6, a5, 0
        bbsi     a6, 28, .Lsec_118_371
.Lsec_118_34d:
        movi.n   a10, 2
        s32i.n   a8, a1, 24
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, ic_interface_enabled
        l32r     a8, .Lsec_118_54
        callx8   a8
        l32i.n   a8, a1, 24
        bnez.n   a10, .Lsec_118_36c
.Lsec_118_35e:
        mov.n    a11, a3
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, wDev_DiscardFrame
        l32r     a8, .Lsec_118_58
        callx8   a8
        j        .Lsec_118_3f2
        .byte    0x00
.Lsec_118_36c:
        l32i.n   a5, a5, 0
        bbci     a5, 30, .Lsec_118_35e
.Lsec_118_371:
        bnez     a8, .Lsec_118_35e
        memw
        l32i.n   a5, a2, 0
        extui    a5, a5, 12, 12
        bnez.n   a5, .Lsec_118_3a2
        movi.n   a12, 1
        l32r     a2, .Lsec_118_1c
        slli     a11, a12, 11
        movi.n   a10, 6
        l32r     a15, .Lsec_118_10
        l32r     a14, .Lsec_118_14
        l32r     a13, .Lsec_118_18
        s32i.n   a2, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_118_5c
        callx8   a8
.Lsec_118_39c:
        j        .Lsec_118_39c
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_118_3a2:
        beqz.n   a7, .Lsec_118_3b4
        l32i.n   a12, a1, 20
        mov.n    a11, a3
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, wDev_IndicateAmpdu
        l32r     a8, .Lsec_118_60
        callx8   a8
        j        .Lsec_118_3f2
        .byte    0x00
.Lsec_118_3b4:
        l32i.n   a14, a1, 20
        mov.n    a13, a3
        mov.n    a12, a2
        mov.n    a11, a7
        mov.n    a10, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, wDev_IndicateFrame
        l32r     a8, .Lsec_118_64
        callx8   a8
        j        .Lsec_118_3f2
        .byte    0x00
.Lsec_118_3c8:
        mov.n    a9, a4
.Lsec_118_3ca:
        mov.n    a8, a4
.Lsec_118_3cc:
        l32i.n   a6, a1, 16
        bnez     a6, .Lsec_118_31f
        j        .Lsec_118_3df
.Lsec_118_3d4:
        mov.n    a6, a4
        movi.n   a8, 1
.Lsec_118_3d8:
        l32i.n   a9, a1, 16
        beqz     a9, .Lsec_118_31d
        mov.n    a9, a4
.Lsec_118_3df:
        mov.n    a4, a9
        j        .Lsec_118_31f
.Lsec_118_3e4:
        l32i.n   a9, a1, 16
        movi.n   a8, 0
        movi.n   a6, 1
        movnez   a6, a8, a9
        mov.n    a8, a4
        j        .Lsec_118_31d
.Lsec_118_3f2:
        retw.n
        .size   wDev_ProcessRxSucData, .-wDev_ProcessRxSucData
.Lsec_118_3f4:

        .section .wifislprxiram.29,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_119_0, wDevCtrl
        .literal .Lsec_119_4, g_wdev_last_desc_reset
        .literal .Lsec_119_8, .Lsec_129_0
        .literal .Lsec_119_c, .Lsec_113_0
        .literal .Lsec_119_10, .Lsec_150_0
        .literal .Lsec_119_14, 0x00000937
        .literal .Lsec_119_18, g_intr_lock_mux
        .literal .Lsec_119_1c, g_osi_funcs_p
        .literal .Lsec_119_20, .Lsec_149_0
        .literal .Lsec_119_24, 0x0000095a
        .literal .Lsec_119_28, hal_mac_rx_get_last_dscr
        .literal .Lsec_119_2c, wifi_log
        .literal .Lsec_119_30, wDev_ProcessRxSucData
        .literal .Lsec_119_34, hal_mac_rx_get_last_dscr
        .literal .Lsec_119_38, wifi_log
.Lsec_119_3c:
        .global wdevProcessRxSucDataAll
        .type   wdevProcessRxSucDataAll, @function
wdevProcessRxSucDataAll:
        entry    a1, 64
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_rx_get_last_dscr
        l32r     a8, .Lsec_119_28
        callx8   a8
        l32r     a3, .Lsec_119_0
        mov.n    a7, a10
        l32r     a4, .Lsec_119_4
        l32i.n   a5, a3, 0
        l8ui     a3, a4, 0
        beqz.n   a3, .Lsec_119_64
        beqz.n   a10, .Lsec_119_89
        movi.n   a3, 0
        s8i      a3, a4, 0
        memw
        memw
        j        .Lsec_119_89
.Lsec_119_64:
        bnez.n   a10, .Lsec_119_89
        l32r     a2, .Lsec_119_14
        movi.n   a12, 1
        slli     a11, a12, 11
        movi.n   a10, 6
        l32r     a15, .Lsec_119_8
        l32r     a14, .Lsec_119_c
        l32r     a13, .Lsec_119_10
        s32i.n   a2, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_119_2c
        callx8   a8
.Lsec_119_84:
        j        .Lsec_119_84
        .byte    0x00
        .byte    0x00
.Lsec_119_89:
        l32r     a4, .Lsec_119_1c
        l32r     a3, .Lsec_119_18
        l32i.n   a6, a4, 0
        l32i.n   a10, a3, 0
        l32i.n   a6, a6, 40
        callx8   a6
        mov.n    a11, a10
        movi.n   a6, 0
        j        .Lsec_119_122
.Lsec_119_9f:
        l32i.n   a8, a5, 8
        memw
        l32i.n   a10, a5, 0
        s32i.n   a8, a1, 16
        addi.n   a8, a6, 1
        extui    a6, a8, 0, 16
        bbci     a10, 30, .Lsec_119_d8
        l32i.n   a10, a4, 0
        l32i.n   a12, a10, 44
        l32i     a10, a3, 0
        memw
        callx8   a12
        mov.n    a11, a6
        mov.n    a12, a2
        mov.n    a10, a5
        .reloc  ., R_XTENSA_ASM_EXPAND, wDev_ProcessRxSucData
        l32r     a8, .Lsec_119_30
        callx8   a8
        l32i.n   a8, a4, 0
        movi.n   a6, 0
        l32i     a8, a8, 40
        l32i     a10, a3, 0
        callx8   a8
        mov.n    a11, a10
.Lsec_119_d8:
        beq      a5, a7, .Lsec_119_125
        s32i.n   a11, a1, 20
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_mac_rx_get_last_dscr
        l32r     a8, .Lsec_119_34
        callx8   a8
        mov.n    a7, a10
        movi.n   a10, 1
        l32r     a5, .Lsec_119_4
        l8ui     a9, a5, 0
        xor      a9, a9, a10
        extui    a9, a9, 0, 8
        l32i.n   a11, a1, 20
        beqz.n   a9, .Lsec_119_120
        movi.n   a9, 1
        bnez.n   a7, .Lsec_119_120
        l32r     a2, .Lsec_119_24
        mov.n    a12, a9
        slli     a11, a9, 11
        movi.n   a10, 6
        l32r     a15, .Lsec_119_8
        l32r     a14, .Lsec_119_c
        l32r     a13, .Lsec_119_20
        s32i.n   a2, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_119_38
        callx8   a8
.Lsec_119_11c:
        j        .Lsec_119_11c
        .byte    0x00
.Lsec_119_120:
        l32i.n   a5, a1, 16
.Lsec_119_122:
        bnez     a5, .Lsec_119_9f
.Lsec_119_125:
        l32i.n   a2, a4, 0
        l32i.n   a10, a3, 0
        l32i.n   a2, a2, 44
        callx8   a2
        retw.n
        .size   wdevProcessRxSucDataAll, .-wdevProcessRxSucDataAll
.Lsec_119_130:

        .section .text.wDev_Insert_KeyEntry,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_120_0, wDevCtrl
        .literal .Lsec_120_4, memcpy
        .literal .Lsec_120_8, hal_crypto_set_key_entry
        .literal .Lsec_120_c, hal_crypto_enable
.Lsec_120_10:
        .global wDev_Insert_KeyEntry
        .type   wDev_Insert_KeyEntry, @function
wDev_Insert_KeyEntry:
        entry    a1, 64
        l8ui     a8, a1, 72
        mov.n    a9, a7
        movi.n   a12, 6
        mov.n    a11, a5
        extui    a3, a3, 0, 8
        addi.n   a10, a1, 3
        s32i.n   a8, a1, 20
        s32i.n   a9, a1, 16
        l8ui     a7, a1, 68
        s8i      a2, a1, 1
        s8i      a3, a1, 0
        s8i      a4, a1, 2
        memw
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, memcpy
        l32r     a8, .Lsec_120_4
        callx8   a8
        mov.n    a13, a1
        mov.n    a10, a6
        l32i.n   a9, a1, 16
        mov.n    a11, a9
        l32i     a12, a1, 64
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_crypto_set_key_entry
        l32r     a8, .Lsec_120_8
        callx8   a8
        l32i.n   a8, a1, 20
        bgeui    a3, 3, .Lsec_120_6d
        blti     a6, 3, .Lsec_120_6d
        addi.n   a3, a3, 4
        l32r     a9, .Lsec_120_0
        addx4    a3, a3, a9
        movi.n   a9, 1
        ssl      a6
        sll      a9, a9
        l32i.n   a10, a3, 12
        or       a10, a10, a9
        s32i.n   a10, a3, 12
.Lsec_120_6d:
        mov.n    a13, a8
        mov.n    a12, a7
        l8ui     a11, a1, 1
        l8ui     a10, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_crypto_enable
        l32r     a8, .Lsec_120_c
        callx8   a8
        retw.n
        .size   wDev_Insert_KeyEntry, .-wDev_Insert_KeyEntry
.Lsec_120_82:

        .section .text.wDev_remove_KeyEntry,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_121_0, wDevCtrl
        .literal .Lsec_121_4, hal_crypto_clr_key_entry
.Lsec_121_8:
        .global wDev_remove_KeyEntry
        .type   wDev_remove_KeyEntry, @function
wDev_remove_KeyEntry:
        entry    a1, 32
        l32r     a8, .Lsec_121_0
        movi.n   a9, -2
        ssl      a2
        src      a9, a9, a9
        mov.n    a10, a2
        l32i.n   a11, a8, 28
        and      a11, a11, a9
        s32i.n   a11, a8, 28
        l32i.n   a11, a8, 32
        and      a11, a11, a9
        s32i.n   a11, a8, 32
        l32i.n   a11, a8, 36
        and      a11, a11, a9
        s32i.n   a11, a8, 36
        l32i.n   a11, a8, 40
        and      a9, a11, a9
        s32i.n   a9, a8, 40
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_crypto_clr_key_entry
        l32r     a8, .Lsec_121_4
        callx8   a8
        retw.n
        .size   wDev_remove_KeyEntry, .-wDev_remove_KeyEntry
.Lsec_121_3f:

        .section .text.wDev_remove_KeyEntry_all_cnx,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_122_0, wDevCtrl
        .literal .Lsec_122_4, wDev_remove_KeyEntry
.Lsec_122_8:
        .global wDev_remove_KeyEntry_all_cnx
        .type   wDev_remove_KeyEntry_all_cnx, @function
wDev_remove_KeyEntry_all_cnx:
        entry    a1, 32
        l32r     a8, .Lsec_122_0
        extui    a2, a2, 0, 8
        addx4    a2, a2, a8
        movi.n   a8, -1
        xor      a3, a8, a3
        l32i.n   a2, a2, 28
        and      a3, a3, a2
        movi     a2, 0
.Lsec_122_21:
        bbc      a3, a2, .Lsec_122_2c
        mov.n    a10, a2
        .reloc  ., R_XTENSA_ASM_EXPAND, wDev_remove_KeyEntry
        l32r     a8, .Lsec_122_4
        callx8   a8
.Lsec_122_2c:
        addi.n   a2, a2, 1
        movi.n   a8, 25
        bne      a2, a8, .Lsec_122_21
        retw.n
        .size   wDev_remove_KeyEntry_all_cnx, .-wDev_remove_KeyEntry_all_cnx
.Lsec_122_35:

        .section .text.wDev_Crypto_Disable,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_123_0, wDevCtrl
        .literal .Lsec_123_4, hal_crypto_disable
.Lsec_123_8:
        .global wDev_Crypto_Disable
        .type   wDev_Crypto_Disable, @function
wDev_Crypto_Disable:
        entry    a1, 32
        l32r     a9, .Lsec_123_0
        extui    a10, a2, 0, 8
        addi.n   a8, a10, 4
        addx4    a8, a8, a9
        movi.n   a9, 0
        l32i.n   a11, a8, 12
        s32i.n   a9, a8, 12
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_crypto_disable
        l32r     a8, .Lsec_123_4
        callx8   a8
        retw.n
        .size   wDev_Crypto_Disable, .-wDev_Crypto_Disable
.Lsec_123_27:

        .section .wifislprxiram.56,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_124_0, 0xdeadbeef
        .literal .Lsec_124_4, 0x00000ffc
        .literal .Lsec_124_8, .Lsec_132_0
        .literal .Lsec_124_c, .Lsec_127_0
        .literal .Lsec_124_10, .Lsec_113_0
        .literal .Lsec_124_14, .Lsec_131_0
        .literal .Lsec_124_18, 0x00000d78
        .literal .Lsec_124_1c, wifi_log
        .literal .Lsec_124_20, wifi_log
.Lsec_124_24:
        .global wDevCheckBlockError
        .type   wDevCheckBlockError, @function
wDevCheckBlockError:
        entry    a1, 48
        l32i.n   a8, a2, 44
        l32i.n   a8, a8, 0
        bbci     a8, 4, .Lsec_124_9e
        l32i.n   a8, a2, 4
        l32r     a12, .Lsec_124_0
        l32r     a13, .Lsec_124_4
        j        .Lsec_124_9b
.Lsec_124_39:
        memw
        l32i.n   a9, a8, 0
        extui    a9, a9, 0, 12
        l32i.n   a14, a8, 4
        add.n    a9, a14, a9
        l32i.n   a11, a9, 0
        beq      a11, a12, .Lsec_124_94
        extui    a15, a13, 0, 16
        memw
        l32i.n   a10, a8, 0
        extui    a10, a10, 0, 12
        beq      a10, a15, .Lsec_124_94
        movi.n   a12, 1
        s32i.n   a11, a1, 4
        movi.n   a10, 6
        movi.n   a11, 32
        s32i     a9, a1, 0
        l32i     a15, a14, 0
        l32r     a13, .Lsec_124_8
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_124_1c
        callx8   a8
        movi.n   a12, 1
        slli     a11, a12, 11
        l32r     a2, .Lsec_124_18
        movi.n   a10, 6
        l32r     a15, .Lsec_124_c
        l32r     a14, .Lsec_124_10
        l32r     a13, .Lsec_124_14
        s32i.n   a2, a1, 0
        memw
        .reloc  ., R_XTENSA_ASM_EXPAND, wifi_log
        l32r     a8, .Lsec_124_20
        callx8   a8
.Lsec_124_90:
        j        .Lsec_124_90
        .byte    0x00
.Lsec_124_94:
        l32i.n   a9, a2, 8
        beq      a9, a8, .Lsec_124_9e
        l32i.n   a8, a8, 8
.Lsec_124_9b:
        bnez     a8, .Lsec_124_39
.Lsec_124_9e:
        retw.n
        .size   wDevCheckBlockError, .-wDevCheckBlockError
.Lsec_124_a0:

        .section .text.wdev_set_csi_rx_cb,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_125_0, g_wdev_csi_rx
        .literal .Lsec_125_4, g_wdev_csi_rx_ctx
.Lsec_125_8:
        .global wdev_set_csi_rx_cb
        .type   wdev_set_csi_rx_cb, @function
wdev_set_csi_rx_cb:
        entry    a1, 32
        l32r     a8, .Lsec_125_0
        s32i.n   a2, a8, 0
        movi.n   a2, 0
        l32r     a8, .Lsec_125_4
        s32i.n   a3, a8, 0
        memw
        retw.n
        .size   wdev_set_csi_rx_cb, .-wdev_set_csi_rx_cb
.Lsec_125_1c:

        .section .text.wdev_is_mgmt_hwdecr_enabled,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_126_0, hal_crypto_mgmt_rx_enabled
.Lsec_126_4:
        .global wdev_is_mgmt_hwdecr_enabled
        .type   wdev_is_mgmt_hwdecr_enabled, @function
wdev_is_mgmt_hwdecr_enabled:
        entry    a1, 32
        extui    a10, a2, 0, 8
        .reloc  ., R_XTENSA_ASM_EXPAND, hal_crypto_mgmt_rx_enabled
        l32r     a8, .Lsec_126_0
        callx8   a8
        mov.n    a2, a10
        retw.n
        .size   wdev_is_mgmt_hwdecr_enabled, .-wdev_is_mgmt_hwdecr_enabled
.Lsec_126_14:

        .section .rodata.__FUNCTION__$0,"a",@progbits
        .align  1
.Lsec_127_0:
        .byte   0x77, 0x44, 0x65, 0x76, 0x50, 0x72, 0x6f, 0x63, 0x65, 0x73, 0x73, 0x44, 0x69, 0x72, 0x74, 0x79
        .byte   0x42, 0x75, 0x66, 0x00

        .section .rodata.__FUNCTION__$1,"a",@progbits
        .align  1
.Lsec_128_0:
        .byte   0x77, 0x44, 0x65, 0x76, 0x5f, 0x41, 0x70, 0x70, 0x65, 0x6e, 0x64, 0x52, 0x78, 0x42, 0x6c, 0x6f
        .byte   0x63, 0x6b, 0x73, 0x00

        .section .rodata.__FUNCTION__$2,"a",@progbits
        .align  1
.Lsec_129_0:
        .byte   0x77, 0x64, 0x65, 0x76, 0x50, 0x72, 0x6f, 0x63, 0x65, 0x73, 0x73, 0x52, 0x78, 0x53, 0x75, 0x63
        .byte   0x44, 0x61, 0x74, 0x61, 0x41, 0x6c, 0x6c, 0x00

        .section .rodata.__FUNCTION__$3,"a",@progbits
        .align  1
.Lsec_130_0:
        .byte   0x77, 0x44, 0x65, 0x76, 0x5f, 0x50, 0x72, 0x6f, 0x63, 0x65, 0x73, 0x73, 0x52, 0x78, 0x53, 0x75
        .byte   0x63, 0x44, 0x61, 0x74, 0x61, 0x00

        .section .rodata_wlog_error.55,"a",@progbits
        .align  1
.Lsec_131_0:
        .byte   0x25, 0x73, 0x20, 0x25, 0x73, 0x20, 0x25, 0x75, 0x0a, 0x00

        .section .rodata_wlog_error.54,"a",@progbits
        .align  1
.Lsec_132_0:
        .byte   0x64, 0x69, 0x72, 0x74, 0x79, 0x20, 0x62, 0x75, 0x66, 0x20, 0x25, 0x30, 0x38, 0x78, 0x3a, 0x20
        .byte   0x25, 0x30, 0x38, 0x78, 0x2c, 0x20, 0x25, 0x30, 0x38, 0x78, 0x3a, 0x20, 0x25, 0x30, 0x38, 0x78
        .byte   0x0a, 0x00

        .section .rodata_wlog_error.53,"a",@progbits
        .align  1
.Lsec_133_0:
        .byte   0x25, 0x73, 0x20, 0x25, 0x73, 0x20, 0x25, 0x75, 0x0a, 0x00

        .section .rodata_wlog_info.51,"a",@progbits
        .align  1
.Lsec_134_0:
        .byte   0x28, 0x25, 0x78, 0x2c, 0x20, 0x25, 0x78, 0x2c, 0x20, 0x25, 0x78, 0x2c, 0x20, 0x25, 0x78, 0x2c
        .byte   0x20, 0x25, 0x78, 0x29, 0x00

        .section .rodata_wlog_info.50,"a",@progbits
        .align  1
.Lsec_135_0:
        .byte   0x25, 0x75, 0x20, 0x28, 0x25, 0x78, 0x2c, 0x20, 0x25, 0x78, 0x2c, 0x20, 0x25, 0x78, 0x2c, 0x20
        .byte   0x25, 0x78, 0x2c, 0x20, 0x25, 0x78, 0x29, 0x00

        .section .rodata_wlog_info.49,"a",@progbits
        .align  1
.Lsec_136_0:
        .byte   0x72, 0x65, 0x6c, 0x6f, 0x61, 0x64, 0x28, 0x25, 0x78, 0x2c, 0x20, 0x25, 0x78, 0x29, 0x00

        .section .dram1.46,"aw",@progbits
        .align  4
.Lsec_137_0:
        .global s_record_list
        .type   s_record_list, @object
        .size   s_record_list, 132
s_record_list:
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00

        .section .dram1.43,"aw",@progbits
        .align  1
.Lsec_138_0:
        .byte   0x01

        .section .dram1.42,"aw",@progbits
        .align  4
.Lsec_139_0:
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00

        .section .dram1.41,"aw",@progbits
        .align  4
.Lsec_140_0:
        .byte   0x00, 0x00, 0x00, 0x00
.Lsec_140_4:
        .byte   0x00, 0x00, 0x00, 0x00

        .section .dram1.40,"aw",@progbits
        .align  4
.Lsec_141_0:
        .byte   0x00, 0x00, 0x00, 0x00
.Lsec_141_4:
        .byte   0x00, 0x00, 0x00, 0x00
.Lsec_141_8:
        .byte   0x00, 0x00, 0x00, 0x00
.Lsec_141_c:
        .byte   0x00, 0x00, 0x00, 0x00

        .section .rodata_wlog_info.39,"a",@progbits
        .align  1
.Lsec_142_0:
        .byte   0x44, 0x65, 0x69, 0x6e, 0x69, 0x74, 0x20, 0x6c, 0x6c, 0x64, 0x65, 0x73, 0x63, 0x20, 0x72, 0x78
        .byte   0x20, 0x6d, 0x62, 0x6c, 0x6f, 0x63, 0x6b, 0x3a, 0x25, 0x64, 0x00

        .section .rodata_wlog_error.38,"a",@progbits
        .align  1
.Lsec_143_0:
        .byte   0x45, 0x78, 0x70, 0x65, 0x63, 0x74, 0x65, 0x64, 0x20, 0x74, 0x6f, 0x20, 0x69, 0x6e, 0x69, 0x74
        .byte   0x20, 0x25, 0x64, 0x20, 0x72, 0x78, 0x20, 0x62, 0x75, 0x66, 0x66, 0x65, 0x72, 0x2c, 0x20, 0x61
        .byte   0x63, 0x74, 0x75, 0x61, 0x6c, 0x20, 0x69, 0x73, 0x20, 0x25, 0x64, 0x00

        .section .rodata_wlog_info.37,"a",@progbits
        .align  1
.Lsec_144_0:
        .byte   0x49, 0x6e, 0x69, 0x74, 0x20, 0x64, 0x79, 0x6e, 0x61, 0x6d, 0x69, 0x63, 0x20, 0x72, 0x78, 0x20
        .byte   0x62, 0x75, 0x66, 0x66, 0x65, 0x72, 0x20, 0x6e, 0x75, 0x6d, 0x3a, 0x20, 0x25, 0x64, 0x00

        .section .rodata_wlog_info.36,"a",@progbits
        .align  1
.Lsec_145_0:
        .byte   0x49, 0x6e, 0x69, 0x74, 0x20, 0x73, 0x74, 0x61, 0x74, 0x69, 0x63, 0x20, 0x72, 0x78, 0x20, 0x62
        .byte   0x75, 0x66, 0x66, 0x65, 0x72, 0x20, 0x6e, 0x75, 0x6d, 0x3a, 0x20, 0x25, 0x64, 0x00

        .section .rodata_wlog_info.35,"a",@progbits
        .align  1
.Lsec_146_0:
        .byte   0x49, 0x6e, 0x69, 0x74, 0x20, 0x73, 0x74, 0x61, 0x74, 0x69, 0x63, 0x20, 0x72, 0x78, 0x20, 0x62
        .byte   0x75, 0x66, 0x66, 0x65, 0x72, 0x20, 0x73, 0x69, 0x7a, 0x65, 0x3a, 0x20, 0x25, 0x64, 0x00

        .section .rodata_wlog_error.34,"a",@progbits
        .align  1
.Lsec_147_0:
        .byte   0x46, 0x61, 0x69, 0x6c, 0x65, 0x64, 0x20, 0x74, 0x6f, 0x20, 0x69, 0x6e, 0x69, 0x74, 0x20, 0x72
        .byte   0x78, 0x20, 0x64, 0x65, 0x73, 0x63, 0x20, 0x73, 0x70, 0x61, 0x63, 0x65, 0x2c, 0x20, 0x62, 0x75
        .byte   0x66, 0x66, 0x65, 0x72, 0x20, 0x6e, 0x75, 0x6d, 0x62, 0x65, 0x72, 0x3d, 0x25, 0x64, 0x00

        .section .rodata_wlog_warning.33,"a",@progbits
        .align  1
.Lsec_148_0:
        .byte   0x6d, 0x61, 0x6c, 0x6c, 0x6f, 0x63, 0x20, 0x62, 0x75, 0x66, 0x66, 0x65, 0x72, 0x20, 0x66, 0x61
        .byte   0x69, 0x6c, 0x00

        .section .rodata_wlog_error.31,"a",@progbits
        .align  1
.Lsec_149_0:
        .byte   0x25, 0x73, 0x20, 0x25, 0x73, 0x20, 0x25, 0x75, 0x0a, 0x00

        .section .rodata_wlog_error.30,"a",@progbits
        .align  1
.Lsec_150_0:
        .byte   0x25, 0x73, 0x20, 0x25, 0x73, 0x20, 0x25, 0x75, 0x0a, 0x00

        .section .rodata_wlog_error.28,"a",@progbits
        .align  1
.Lsec_151_0:
        .byte   0x25, 0x73, 0x20, 0x25, 0x73, 0x20, 0x25, 0x75, 0x0a, 0x00

        .section .rodata_wlog_warning.22,"a",@progbits
        .align  1
.Lsec_152_0:
        .byte   0x72, 0x78, 0x20, 0x63, 0x6f, 0x6e, 0x74, 0x72, 0x6f, 0x6c, 0x20, 0x62, 0x6c, 0x6f, 0x63, 0x6b
        .byte   0x73, 0x3d, 0x25, 0x64, 0x00

        .section .rodata_wlog_info.20,"a",@progbits
        .align  1
.Lsec_153_0:
        .byte   0x69, 0x63, 0x5f, 0x64, 0x69, 0x73, 0x61, 0x62, 0x6c, 0x65, 0x5f, 0x73, 0x6e, 0x69, 0x66, 0x66
        .byte   0x65, 0x72, 0x00

        .section .rodata_wlog_info.19,"a",@progbits
        .align  1
.Lsec_154_0:
        .byte   0x69, 0x63, 0x5f, 0x65, 0x6e, 0x61, 0x62, 0x6c, 0x65, 0x5f, 0x73, 0x6e, 0x69, 0x66, 0x66, 0x65
        .byte   0x72, 0x00

        .section .rodata_wlog_error.18,"a",@progbits
        .align  1
.Lsec_155_0:
        .byte   0x70, 0x72, 0x6f, 0x6d, 0x69, 0x73, 0x20, 0x62, 0x75, 0x66, 0x3a, 0x20, 0x6f, 0x75, 0x74, 0x20
        .byte   0x6f, 0x66, 0x20, 0x6d, 0x65, 0x6d, 0x6f, 0x72, 0x79, 0x00

        .section .dram1.16,"aw",@progbits
        .align  4
.Lsec_156_0:
        .byte   0x00, 0x00, 0x00, 0x00

        .section .rodata_wlog_warning.15,"a",@progbits
        .align  1
.Lsec_157_0:
        .byte   0x75, 0x6e, 0x6b, 0x6e, 0x6f, 0x77, 0x6e, 0x20, 0x63, 0x73, 0x69, 0x20, 0x62, 0x75, 0x67, 0x21
        .byte   0x00

        .section .rodata_wlog_info.10,"a",@progbits
        .align  1
.Lsec_158_0:
        .byte   0x41, 0x6c, 0x6c, 0x6f, 0x63, 0x61, 0x74, 0x65, 0x20, 0x4d, 0x41, 0x43, 0x20, 0x72, 0x65, 0x67
        .byte   0x69, 0x73, 0x74, 0x65, 0x72, 0x20, 0x6d, 0x65, 0x6d, 0x6f, 0x72, 0x79, 0x20, 0x62, 0x61, 0x73
        .byte   0x65, 0x20, 0x61, 0x64, 0x64, 0x72, 0x28, 0x30, 0x78, 0x25, 0x30, 0x38, 0x78, 0x29, 0x2e, 0x00

        .section .rodata_wlog_warning.9,"a",@progbits
        .align  1
.Lsec_159_0:
        .byte   0x41, 0x6c, 0x6c, 0x6f, 0x63, 0x61, 0x74, 0x65, 0x20, 0x4d, 0x41, 0x43, 0x20, 0x72, 0x65, 0x67
        .byte   0x69, 0x73, 0x74, 0x65, 0x72, 0x20, 0x6d, 0x65, 0x6d, 0x6f, 0x72, 0x79, 0x20, 0x66, 0x61, 0x69
        .byte   0x6c, 0x00

        .section .data.BcnInterval,"aw",@progbits
        .align  4
.Lsec_160_0:
        .global BcnInterval
        .type   BcnInterval, @object
        .size   BcnInterval, 4
BcnInterval:
        .byte   0x00, 0x90, 0x01, 0x00

        .section .bss.BcnSendTick,"aw",@nobits
        .align  4
.Lsec_161_0:
        .global BcnSendTick
        .type   BcnSendTick, @object
        .size   BcnSendTick, 4
BcnSendTick:
        .skip   4

        .section .dram1.2,"aw",@progbits
        .align  4
.Lsec_162_0:
        .byte   0x00, 0x00, 0x00, 0x00

        .section .bss.wDevMacSleep,"aw",@nobits
        .align  4
.Lsec_163_0:
        .global wDevMacSleep
        .type   wDevMacSleep, @object
        .size   wDevMacSleep, 112
wDevMacSleep:
        .skip   112

        .section .data.wDevCtrl,"aw",@progbits
        .align  4
.Lsec_164_0:
        .global wDevCtrl
        .type   wDevCtrl, @object
        .size   wDevCtrl, 56
wDevCtrl:
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x60, 0x17
        .byte   0x00, 0x00, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00

        .section .bss.g_wdev_csi_rx_ctx,"aw",@nobits
        .align  4
.Lsec_165_0:
        .global g_wdev_csi_rx_ctx
        .type   g_wdev_csi_rx_ctx, @object
        .size   g_wdev_csi_rx_ctx, 4
g_wdev_csi_rx_ctx:
        .skip   4

        .section .bss.g_wdev_csi_rx,"aw",@nobits
        .align  4
.Lsec_166_0:
        .global g_wdev_csi_rx
        .type   g_wdev_csi_rx, @object
        .size   g_wdev_csi_rx, 4
g_wdev_csi_rx:
        .skip   4

        .section .bss.g_wdev_is_nan_pkt_in_valid_slot_cb,"aw",@nobits
        .align  4
.Lsec_167_0:
        .global g_wdev_is_nan_pkt_in_valid_slot_cb
        .type   g_wdev_is_nan_pkt_in_valid_slot_cb, @object
        .size   g_wdev_is_nan_pkt_in_valid_slot_cb, 4
g_wdev_is_nan_pkt_in_valid_slot_cb:
        .skip   4

        .section .bss.g_wdev_last_desc_reset,"aw",@nobits
        .align  1
.Lsec_168_0:
        .global g_wdev_last_desc_reset
        .type   g_wdev_last_desc_reset, @object
        .size   g_wdev_last_desc_reset, 1
g_wdev_last_desc_reset:
        .skip   1
