.section .iram1.2,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_8_0, 0x3ff730a4
.Lsec_8_4:
        .global hal_mac_rx_get_end_state
        .type   hal_mac_rx_get_end_state, @function
hal_mac_rx_get_end_state:
        entry    a1, 32
        l32r     a2, .Lsec_8_0
        memw
        l32i.n   a2, a2, 0
        extui    a2, a2, 0, 8
        retw.n
        .size   hal_mac_rx_get_end_state, .-hal_mac_rx_get_end_state
.Lsec_8_14:

        .section .wifislprxiram.3,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_9_0, 0x3ff73090
.Lsec_9_4:
        .global hal_mac_rx_get_last_dscr
        .type   hal_mac_rx_get_last_dscr, @function
hal_mac_rx_get_last_dscr:
        entry    a1, 32
        l32r     a2, .Lsec_9_0
        memw
        l32i.n   a2, a2, 0
        retw.n
        .size   hal_mac_rx_get_last_dscr, .-hal_mac_rx_get_last_dscr
.Lsec_9_11:

        .section .wifislprxiram.4,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_10_0, 0x3ff73084
.Lsec_10_4:
        .global hal_mac_rx_is_dscr_reload
        .type   hal_mac_rx_is_dscr_reload, @function
hal_mac_rx_is_dscr_reload:
        entry    a1, 32
        l32r     a2, .Lsec_10_0
        memw
        l32i.n   a2, a2, 0
        extui    a2, a2, 0, 1
        retw.n
        .size   hal_mac_rx_is_dscr_reload, .-hal_mac_rx_is_dscr_reload
.Lsec_10_14:

        .section .wifislprxiram.5,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_11_0, 0x3ff73084
.Lsec_11_4:
        .global hal_mac_rx_set_dscr_reload
        .type   hal_mac_rx_set_dscr_reload, @function
hal_mac_rx_set_dscr_reload:
        entry    a1, 32
        l32r     a9, .Lsec_11_0
        movi.n   a10, 1
        memw
        l32i.n   a8, a9, 0
        or       a8, a8, a10
        memw
        s32i.n   a8, a9, 0
        memw
        retw.n
        .size   hal_mac_rx_set_dscr_reload, .-hal_mac_rx_set_dscr_reload
.Lsec_11_1e:

        .section .iram1.6,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_12_0, 0x3ff73d84
        .literal .Lsec_12_4, 0x3ff73d88
        .literal .Lsec_12_8, 0x3ff730a4
        .literal .Lsec_12_c, 0x3ff732f8
        .literal .Lsec_12_10, 0x3ff732f4
        .literal .Lsec_12_14, 0x3ff732f0
.Lsec_12_18:
        .global hal_mac_rx_get_end_info
        .type   hal_mac_rx_get_end_info, @function
hal_mac_rx_get_end_info:
        entry    a1, 32
        l32r     a8, .Lsec_12_0
        memw
        l32i.n   a8, a8, 0
        s32i.n   a8, a2, 0
        l32r     a8, .Lsec_12_4
        memw
        l32i.n   a8, a8, 0
        s32i.n   a8, a3, 0
        l32r     a8, .Lsec_12_8
        memw
        l32i.n   a8, a8, 0
        extui    a8, a8, 0, 8
        s32i.n   a8, a4, 0
        l32r     a8, .Lsec_12_c
        memw
        l32i.n   a8, a8, 0
        s32i.n   a8, a5, 0
        l32r     a8, .Lsec_12_10
        memw
        l32i.n   a8, a8, 0
        s32i.n   a8, a6, 0
        l32r     a8, .Lsec_12_14
        memw
        l32i.n   a8, a8, 0
        s32i.n   a8, a7, 0
        memw
        retw.n
        .size   hal_mac_rx_get_end_info, .-hal_mac_rx_get_end_info
.Lsec_12_5f:
