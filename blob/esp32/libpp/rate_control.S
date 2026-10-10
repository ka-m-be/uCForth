.section .text.RC_GetDuration,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_9_0, .Lsec_16_0
        .literal .Lsec_9_4, 0x00003fff
        .literal .Lsec_9_8, 0x0003ffff
.Lsec_9_c:
        entry    a1, 32
        slli     a3, a3, 3
        bltui    a2, 8, .Lsec_9_18
        addi     a3, a3, 22
.Lsec_9_18:
        l32r     a8, .Lsec_9_0
        addx8    a8, a2, a8
        l32r     a9, .Lsec_9_4
        l32i.n   a8, a8, 0
        bltu     a9, a3, .Lsec_9_35
        mull     a3, a3, a8
        l32r     a8, .Lsec_9_8
        add.n    a3, a3, a8
        extui    a3, a3, 18, 14
        j        .Lsec_9_5e
        .byte    0x00
.Lsec_9_35:
        extui    a10, a3, 0, 9
        extui    a9, a8, 0, 9
        srli     a3, a3, 9
        extui    a8, a8, 9, 16
        mull     a11, a3, a8
        mull     a3, a9, a3
        mull     a8, a10, a8
        mull     a9, a9, a10
        add.n    a3, a3, a8
        slli     a3, a3, 9
        l32r     a8, .Lsec_9_8
        add.n    a3, a3, a9
        add.n    a3, a3, a8
        srai     a3, a3, 18
        add.n    a3, a3, a11
.Lsec_9_5e:
        bltui    a2, 8, .Lsec_9_68
        addi.n   a3, a3, 3
        movi.n   a2, -4
        and      a3, a3, a2
.Lsec_9_68:
        extui    a2, a3, 0, 16
        retw.n

        .section .text.RC_SetBasicRate,"ax",@progbits
        .align  4
.Lsec_10_0:
        .global RC_SetBasicRate
        .type   RC_SetBasicRate, @function
RC_SetBasicRate:
        entry    a1, 32
        movi.n   a2, 0
        retw.n
        .size   RC_SetBasicRate, .-RC_SetBasicRate
.Lsec_10_7:

        .section .text.RC_GetAckRate,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_11_0, .Lsec_16_0
.Lsec_11_4:
        .global RC_GetAckRate
        .type   RC_GetAckRate, @function
RC_GetAckRate:
        entry    a1, 32
        l32r     a8, .Lsec_11_0
        extui    a2, a2, 0, 8
        addx8    a2, a2, a8
        l8ui     a2, a2, 6
        retw.n
        .size   RC_GetAckRate, .-RC_GetAckRate
.Lsec_11_15:

        .section .text.RC_GetRtsRate,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_12_0, .Lsec_16_0
.Lsec_12_4:
        .global RC_GetRtsRate
        .type   RC_GetRtsRate, @function
RC_GetRtsRate:
        entry    a1, 32
        l32r     a8, .Lsec_12_0
        extui    a2, a2, 0, 8
        addx8    a2, a2, a8
        l8ui     a2, a2, 6
        retw.n
        .size   RC_GetRtsRate, .-RC_GetRtsRate
.Lsec_12_15:

        .section .text.RC_GetAckTime,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_13_0, .Lsec_16_0
.Lsec_13_4:
        .global RC_GetAckTime
        .type   RC_GetAckTime, @function
RC_GetAckTime:
        entry    a1, 32
        l32r     a9, .Lsec_13_0
        extui    a2, a2, 0, 8
        addx8    a2, a2, a9
        l8ui     a8, a2, 6
        addx8    a8, a8, a9
        l16ui    a2, a8, 4
        retw.n
        .size   RC_GetAckTime, .-RC_GetAckTime
.Lsec_13_1b:

        .section .text.RC_GetCtsTime,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_14_0, .Lsec_9_c
.Lsec_14_4:
        .global RC_GetCtsTime
        .type   RC_GetCtsTime, @function
RC_GetCtsTime:
        entry    a1, 32
        extui    a10, a2, 0, 8
        extui    a11, a3, 0, 16
        extui    a4, a4, 0, 16
        movi.n   a8, 10
        bltui    a10, 8, .Lsec_14_18
        movi     a8, 16
.Lsec_14_18:
        addi     a9, a10, -41
        extui    a9, a9, 0, 8
        movi     a5, 0x120
        bltui    a9, 2, .Lsec_14_39
        movi     a5, 192
        bltui    a10, 5, .Lsec_14_39
        movi     a5, 96
        bltui    a10, 8, .Lsec_14_39
        movi.n   a9, 15
        movi.n   a5, 36
        bltu     a9, a10, .Lsec_14_39
        movi.n   a5, 20
.Lsec_14_39:
        add.n    a8, a8, a4
        extui    a4, a8, 0, 16
        add.n    a5, a5, a4
        .reloc  ., R_XTENSA_ASM_EXPAND, .Lsec_9_c
        l32r     a8, .Lsec_14_0
        callx8   a8
        add.n    a5, a10, a5
        extui    a2, a5, 0, 16
        retw.n
        .size   RC_GetCtsTime, .-RC_GetCtsTime
.Lsec_14_4d:

        .section .text.RC_GetBlockAckTime,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_15_0, .Lsec_16_0
        .literal .Lsec_15_4, .Lsec_9_c
.Lsec_15_8:
        .global RC_GetBlockAckTime
        .type   RC_GetBlockAckTime, @function
RC_GetBlockAckTime:
        entry    a1, 32
        l32r     a3, .Lsec_15_0
        extui    a2, a2, 0, 8
        addx8    a2, a2, a3
        l8ui     a10, a2, 6
        movi     a2, 10
        bltui    a10, 8, .Lsec_15_20
        movi     a2, 16
.Lsec_15_20:
        addi     a8, a10, -41
        extui    a8, a8, 0, 8
        movi     a3, 0x120
        bltui    a8, 2, .Lsec_15_41
        movi     a3, 192
        bltui    a10, 5, .Lsec_15_41
        movi     a3, 96
        bltui    a10, 8, .Lsec_15_41
        movi.n   a8, 15
        movi.n   a3, 36
        bltu     a8, a10, .Lsec_15_41
        movi.n   a3, 20
.Lsec_15_41:
        movi.n   a11, 30
        .reloc  ., R_XTENSA_ASM_EXPAND, .Lsec_9_c
        l32r     a8, .Lsec_15_4
        callx8   a8
        add.n    a10, a10, a3
        add.n    a2, a2, a10
        extui    a2, a2, 0, 16
        retw.n
        .size   RC_GetBlockAckTime, .-RC_GetBlockAckTime
.Lsec_15_52:

        .section .rodata.our_controls,"a",@progbits
        .align  4
.Lsec_16_0:
        .byte   0x00, 0x00, 0x04, 0x00, 0x3a, 0x01, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x02, 0x01, 0x05, 0x00
        .byte   0x2e, 0xba, 0x00, 0x00, 0xdf, 0x00, 0x06, 0x00, 0x17, 0x5d, 0x00, 0x00, 0xd5, 0x00, 0x07, 0x00
        .byte   0x00, 0x00, 0x04, 0x00, 0x3a, 0x01, 0x04, 0x00, 0x00, 0x00, 0x02, 0x00, 0xa2, 0x00, 0x05, 0x00
        .byte   0x2e, 0xba, 0x00, 0x00, 0x7f, 0x00, 0x06, 0x00, 0x17, 0x5d, 0x00, 0x00, 0x75, 0x00, 0x07, 0x00
        .byte   0x55, 0x15, 0x00, 0x00, 0x2c, 0x00, 0x09, 0x00, 0xaa, 0x2a, 0x00, 0x00, 0x2c, 0x00, 0x09, 0x00
        .byte   0x55, 0x55, 0x00, 0x00, 0x30, 0x00, 0x0a, 0x00, 0xaa, 0xaa, 0x00, 0x00, 0x3c, 0x00, 0x0b, 0x00
        .byte   0xf6, 0x12, 0x00, 0x00, 0x2c, 0x00, 0x09, 0x00, 0x71, 0x1c, 0x00, 0x00, 0x2c, 0x00, 0x09, 0x00
        .byte   0xe3, 0x38, 0x00, 0x00, 0x30, 0x00, 0x0a, 0x00, 0xc7, 0x71, 0x00, 0x00, 0x3c, 0x00, 0x0b, 0x00
        .byte   0x89, 0x9d, 0x00, 0x00, 0x3c, 0x00, 0x0b, 0x00, 0xc4, 0x4e, 0x00, 0x00, 0x30, 0x00, 0x0a, 0x00
        .byte   0x83, 0x34, 0x00, 0x00, 0x30, 0x00, 0x0a, 0x00, 0x62, 0x27, 0x00, 0x00, 0x2c, 0x00, 0x09, 0x00
        .byte   0x41, 0x1a, 0x00, 0x00, 0x2c, 0x00, 0x09, 0x00, 0xb1, 0x13, 0x00, 0x00, 0x2c, 0x00, 0x09, 0x00
        .byte   0x81, 0x11, 0x00, 0x00, 0x2c, 0x00, 0x09, 0x00, 0xc0, 0x0f, 0x00, 0x00, 0x2c, 0x00, 0x09, 0x00
        .byte   0xc8, 0x8d, 0x00, 0x00, 0x3c, 0x00, 0x0b, 0x00, 0xe4, 0x46, 0x00, 0x00, 0x30, 0x00, 0x0a, 0x00
        .byte   0x42, 0x2f, 0x00, 0x00, 0x30, 0x00, 0x0a, 0x00, 0x72, 0x23, 0x00, 0x00, 0x2c, 0x00, 0x09, 0x00
        .byte   0xa1, 0x17, 0x00, 0x00, 0x2c, 0x00, 0x09, 0x00, 0xb9, 0x11, 0x00, 0x00, 0x2c, 0x00, 0x09, 0x00
        .byte   0xc0, 0x0f, 0x00, 0x00, 0x2c, 0x00, 0x09, 0x00, 0x2d, 0x0e, 0x00, 0x00, 0x2c, 0x00, 0x09, 0x00
        .byte   0x00, 0x00, 0x08, 0x00, 0x0a, 0x02, 0x2a, 0x00, 0x00, 0x00, 0x10, 0x00, 0xea, 0x02, 0x29, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        .byte   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
