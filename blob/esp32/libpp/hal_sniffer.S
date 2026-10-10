.section .text.hal_sniffer_enable,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_7_0, 0x3ff730e4
.Lsec_7_4:
        .global hal_sniffer_enable
        .type   hal_sniffer_enable, @function
hal_sniffer_enable:
        entry    a1, 32
        l32r     a8, .Lsec_7_0
        movi.n   a10, 1
        slli     a10, a10, 15
        memw
        l32i.n   a9, a8, 0
        or       a9, a9, a10
        movi.n   a10, -2
        memw
        s32i.n   a9, a8, 0
        memw
        l32i.n   a9, a8, 0
        and      a9, a9, a10
        movi.n   a10, -3
        memw
        s32i.n   a9, a8, 0
        memw
        l32i.n   a9, a8, 0
        and      a9, a9, a10
        movi.n   a10, -5
        memw
        s32i.n   a9, a8, 0
        memw
        l32i.n   a9, a8, 0
        and      a9, a9, a10
        movi.n   a10, -9
        memw
        s32i.n   a9, a8, 0
        memw
        l32i.n   a9, a8, 0
        and      a9, a9, a10
        movi.n   a10, 32
        memw
        s32i.n   a9, a8, 0
        memw
        l32i.n   a9, a8, 0
        or       a9, a9, a10
        movi.n   a10, 64
        memw
        s32i.n   a9, a8, 0
        memw
        l32i.n   a9, a8, 0
        or       a9, a9, a10
        movi     a10, 128
        memw
        s32i.n   a9, a8, 0
        memw
        l32i.n   a9, a8, 0
        or       a9, a9, a10
        memw
        s32i.n   a9, a8, 0
        memw
        retw.n
        .size   hal_sniffer_enable, .-hal_sniffer_enable
.Lsec_7_8b:

        .section .text.hal_sniffer_disable,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_8_0, 0x3ff730e4
        .literal .Lsec_8_4, 0xffff7fff
.Lsec_8_8:
        .global hal_sniffer_disable
        .type   hal_sniffer_disable, @function
hal_sniffer_disable:
        entry    a1, 32
        l32r     a8, .Lsec_8_0
        l32r     a10, .Lsec_8_4
        memw
        l32i.n   a9, a8, 0
        and      a9, a9, a10
        movi.n   a10, 1
        memw
        s32i.n   a9, a8, 0
        memw
        l32i.n   a9, a8, 0
        or       a9, a9, a10
        movi.n   a10, 2
        memw
        s32i.n   a9, a8, 0
        memw
        l32i.n   a9, a8, 0
        or       a9, a9, a10
        movi.n   a10, 4
        memw
        s32i.n   a9, a8, 0
        memw
        l32i.n   a9, a8, 0
        or       a9, a9, a10
        movi.n   a10, 8
        memw
        s32i.n   a9, a8, 0
        memw
        l32i.n   a9, a8, 0
        or       a9, a9, a10
        movi     a10, -33
        memw
        s32i.n   a9, a8, 0
        memw
        l32i.n   a9, a8, 0
        and      a9, a9, a10
        movi     a10, -65
        memw
        s32i.n   a9, a8, 0
        memw
        l32i.n   a9, a8, 0
        and      a9, a9, a10
        movi     a10, -129
        memw
        s32i.n   a9, a8, 0
        memw
        l32i.n   a9, a8, 0
        and      a9, a9, a10
        memw
        s32i.n   a9, a8, 0
        memw
        retw.n
        .size   hal_sniffer_disable, .-hal_sniffer_disable
.Lsec_8_8f:

        .section .text.hal_sniffer_rx_set_promis,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_9_0, 0x3ff73c40
        .literal .Lsec_9_4, 0x3ff73104
        .literal .Lsec_9_8, 0x3ff730f8
        .literal .Lsec_9_c, 0x3ff730fc
.Lsec_9_10:
        .global hal_sniffer_rx_set_promis
        .type   hal_sniffer_rx_set_promis, @function
hal_sniffer_rx_set_promis:
        entry    a1, 32
        extui    a3, a3, 0, 8
        beqz.n   a2, .Lsec_9_20
        beqi     a2, 1, .Lsec_9_35
        j        .Lsec_9_c1
        .byte    0x00
        .byte    0x00
.Lsec_9_20:
        l32r     a8, .Lsec_9_0
        movi.n   a4, -5
        memw
        l32i.n   a9, a8, 0
        and      a4, a9, a4
        beqz     a3, .Lsec_9_bc
        movi.n   a4, 4
        j        .Lsec_9_b9
.Lsec_9_35:
        l32r     a8, .Lsec_9_4
        memw
        l32i.n   a9, a8, 0
        extui    a9, a9, 0, 16
        memw
        s32i.n   a9, a8, 0
        beqz.n   a3, .Lsec_9_7d
        memw
        l32i.n   a9, a8, 0
        or       a9, a9, a4
        memw
        s32i.n   a9, a8, 0
        l32r     a9, .Lsec_9_8
        memw
        l32i.n   a8, a9, 0
        or       a8, a8, a4
        memw
        s32i.n   a8, a9, 0
        l32r     a9, .Lsec_9_c
        memw
        l32i.n   a8, a9, 0
        or       a8, a8, a4
        memw
        s32i.n   a8, a9, 0
        memw
        j        .Lsec_9_c1
        .byte    0x00
        .byte    0x00
        .byte    0x00
.Lsec_9_7d:
        l32r     a9, .Lsec_9_8
        movi.n   a8, -1
        xor      a4, a8, a4
        memw
        l32i.n   a10, a9, 0
        and      a10, a10, a4
        l32r     a8, .Lsec_9_c
        memw
        s32i.n   a10, a9, 0
        memw
        l32i.n   a2, a8, 0
        and      a4, a4, a2
        memw
        s32i.n   a4, a8, 0
        movi.n   a4, 5
        slli     a4, a4, 24
        memw
        l32i.n   a10, a9, 0
        or       a10, a10, a4
        memw
        s32i.n   a10, a9, 0
        memw
        l32i.n   a9, a8, 0
.Lsec_9_b9:
        or       a4, a9, a4
.Lsec_9_bc:
        memw
        s32i.n   a4, a8, 0
.Lsec_9_c1:
        memw
        retw.n
        .size   hal_sniffer_rx_set_promis, .-hal_sniffer_rx_set_promis
.Lsec_9_c6:

        .section .text.hal_sniffer_rx_clr_statistics,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_10_0, 0x3ff73d7c
        .literal .Lsec_10_4, 0x7fffffff
.Lsec_10_8:
        .global hal_sniffer_rx_clr_statistics
        .type   hal_sniffer_rx_clr_statistics, @function
hal_sniffer_rx_clr_statistics:
        entry    a1, 32
        l32r     a8, .Lsec_10_0
        movi.n   a10, -1
        slli     a10, a10, 31
        memw
        l32i.n   a9, a8, 0
        or       a9, a9, a10
        memw
        s32i.n   a9, a8, 0
        memw
        l32i.n   a9, a8, 0
        l32r     a10, .Lsec_10_4
        and      a9, a9, a10
        memw
        s32i.n   a9, a8, 0
        memw
        retw.n
        .size   hal_sniffer_rx_clr_statistics, .-hal_sniffer_rx_clr_statistics
.Lsec_10_35:
