.section .text.hal_enable_sta_tsf,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_8_0, 0x3ff75020
.Lsec_8_4:
        .global hal_enable_sta_tsf
        .type   hal_enable_sta_tsf, @function
hal_enable_sta_tsf:
        entry    a1, 32
        l32r     a10, .Lsec_8_0
        movi.n   a9, -13
        slli     a9, a9, 27
        memw
        l32i.n   a8, a10, 0
        or       a8, a8, a9
        memw
        s32i.n   a8, a10, 0
        memw
        retw.n
        .size   hal_enable_sta_tsf, .-hal_enable_sta_tsf
.Lsec_8_21:

        .section .text.hal_disable_sta_tsf,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_9_0, 0x3ff75020
        .literal .Lsec_9_4, 0x67ffffff
.Lsec_9_8:
        .global hal_disable_sta_tsf
        .type   hal_disable_sta_tsf, @function
hal_disable_sta_tsf:
        entry    a1, 32
        l32r     a9, .Lsec_9_0
        l32r     a10, .Lsec_9_4
        memw
        l32i.n   a8, a9, 0
        and      a8, a8, a10
        memw
        s32i.n   a8, a9, 0
        memw
        retw.n
        .size   hal_disable_sta_tsf, .-hal_disable_sta_tsf
.Lsec_9_23:

        .section .text.hal_enable_nan_tsf,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_10_0, 0x3ff75098
.Lsec_10_4:
        .global hal_enable_nan_tsf
        .type   hal_enable_nan_tsf, @function
hal_enable_nan_tsf:
        entry    a1, 32
        l32r     a10, .Lsec_10_0
        movi.n   a9, -13
        slli     a9, a9, 27
        memw
        l32i.n   a8, a10, 0
        or       a8, a8, a9
        memw
        s32i.n   a8, a10, 0
        memw
        retw.n
        .size   hal_enable_nan_tsf, .-hal_enable_nan_tsf
.Lsec_10_21:

        .section .text.hal_disable_nan_tsf,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_11_0, 0x3ff75098
        .literal .Lsec_11_4, 0x67ffffff
.Lsec_11_8:
        .global hal_disable_nan_tsf
        .type   hal_disable_nan_tsf, @function
hal_disable_nan_tsf:
        entry    a1, 32
        l32r     a9, .Lsec_11_0
        l32r     a10, .Lsec_11_4
        memw
        l32i.n   a8, a9, 0
        and      a8, a8, a10
        memw
        s32i.n   a8, a9, 0
        memw
        retw.n
        .size   hal_disable_nan_tsf, .-hal_disable_nan_tsf
.Lsec_11_23:

        .section .text.hal_disable_softap_tsf,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_12_0, 0x3ff7505c
        .literal .Lsec_12_4, 0x3dffffff
.Lsec_12_8:
        .global hal_disable_softap_tsf
        .type   hal_disable_softap_tsf, @function
hal_disable_softap_tsf:
        entry    a1, 32
        l32r     a9, .Lsec_12_0
        l32r     a10, .Lsec_12_4
        memw
        l32i.n   a8, a9, 0
        and      a8, a8, a10
        memw
        s32i.n   a8, a9, 0
        memw
        retw.n
        .size   hal_disable_softap_tsf, .-hal_disable_softap_tsf
.Lsec_12_23:

        .section .bss.s_tbttstart,"aw",@nobits
        .align  8
.Lsec_13_0:
        .global s_tbttstart
        .type   s_tbttstart, @object
        .size   s_tbttstart, 8
s_tbttstart:
        .skip   8
