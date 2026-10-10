.section .text.RFChannelSel,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_6_0, chip_v7_set_chan
.Lsec_6_4:
        .global RFChannelSel
        .type   RFChannelSel, @function
RFChannelSel:
        entry    a1, 32
        sext     a11, a3, 7
        sext     a10, a2, 7
        .reloc  ., R_XTENSA_ASM_EXPAND, chip_v7_set_chan
        l32r     a8, .Lsec_6_0
        callx8   a8
        retw.n
        .size   RFChannelSel, .-RFChannelSel
.Lsec_6_15:

        .section .text.phy_change_channel,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_7_0, set_chanfreq
.Lsec_7_4:
        .global phy_change_channel
        .type   phy_change_channel, @function
phy_change_channel:
        entry    a1, 32
        extui    a10, a2, 0, 16
        sext     a11, a5, 7
        .reloc  ., R_XTENSA_ASM_EXPAND, set_chanfreq
        l32r     a8, .Lsec_7_0
        callx8   a8
        movi.n   a2, 0
        retw.n
        .size   phy_change_channel, .-phy_change_channel
.Lsec_7_17:

        .section .text.phy_change_channel_nomac,"ax",@progbits
        .align  4
        .literal_position
        .literal .Lsec_8_0, set_chanfreq_nomac
.Lsec_8_4:
        .global phy_change_channel_nomac
        .type   phy_change_channel_nomac, @function
phy_change_channel_nomac:
        entry    a1, 32
        extui    a10, a2, 0, 16
        sext     a11, a5, 7
        .reloc  ., R_XTENSA_ASM_EXPAND, set_chanfreq_nomac
        l32r     a8, .Lsec_8_0
        callx8   a8
        movi.n   a2, 0
        retw.n
        .size   phy_change_channel_nomac, .-phy_change_channel_nomac
.Lsec_8_17:
