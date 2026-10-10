\ bt_bb.fs - package RTC-BT-BB: bt_bb.o of librtc.a (ESP32)
\
\ Made from bt_bb.S by s2fs.py, not by hand. Each function of C is a
\ CODE word: its bytes are those of the compiler, at the same offsets of
\ its section (L gives the address of an offset); its literals are at the
\ start of the word, after a jump. The data come first, then the
\ functions, each after those that it calls. The global symbols are
\ PUBLIC, the static ones PRIVATE. lib/esp32/clink.fs explains the words.
\
\ 15 functions (15 global), 2 data objects.
\ A function runs as C code with the windowed calls of the Xtensa: call
\ it with CALL-C ( x1 .. xn n xt -- x ), never by its name alone.
\
\ Names of other packages: a literal waits until its package comes
\ (LINK-SYMBOLS); a name outside the libraries until EXTERN gives it.
\ Uses:
\   PHY-PHY-CHIP-V7  g_phyFuns
\   PHY-PHY-CHIP-V7-ANA  phy_dis_hw_set_freq phy_en_hw_set_freq
\                        phy_force_wifi_chan phy_get_fetx_delay
\                        phy_unforce_wifi_chan
\ Outside the libraries (../README.md):
\   esp_dport_access_reg_read ets_delay_us rtc_printf
\
\ Needs XTENSA (asm/xtensa.fs), ASM (lib/esp32/asm.fs) and CLINK
\ (lib/esp32/clink.fs) of ucf; the ESP32.

PACKAGE RTC-BT-BB  ALSO ASM  ALSO CLINK   \ lib/package.fs
EXPORTS   \ the PUBLIC words are symbols for the other packages

\ ==== Data =============================================================

PRIVATE
CREATE rodata.str1.4   \ .rodata.str1.4, 64 bytes
  ," 13:39:26"  0 C,  0 C,  0 C,  0 C,
  ," Mar 22 2024"  0 C,
  ," 6759f983"  0 C,  0 C,  0 C,  0 C,
  ," librtc version: %s, %s, %s"  10 C,  0 C,
PUBLIC
4 DATA: force_wifi_mode_on   \ .bss.force_wifi_mode_on

\ ==== Code =============================================================

CODE BT_tx_8m_enable   \ .text.BT_tx_8m_enable $28
  $0 10 LITERALS
    $3FF51000 T,  $3FF51014 T,  $18021 T,  $3FF51044 T,  $3FF51040 T,
    $F81FFFFF T,  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read
$28 L:
  A1 32 ENTRY,  A5 $0 L L32R,  A3 -61 MOVI,  A10 A5 MOV.N,  A8 $18 L L32R,
  A8 CALLX8,
  A3 A10 A3 AND,  MEMW,  A3 A5 0 S32I.N,  NOP.N,  NOP.N,  NOP.N,  NOP.N,
  A4 $C L L32R,  A10 A4 MOV.N,  A3 $4 L L32R,  A5 $8 L L32R,  MEMW,  A5 A3 0 S32I,
  A8 $1C L L32R,  A8 CALLX8,
  A8 -256 MOVI,  A10 A10 A8 AND,  A8 160 MOVI,  A3 $10 L L32R,  A8 A10 A8 OR,
  A10 A3 MOV.N,  MEMW,  A8 A4 0 S32I.N,  A8 $20 L L32R,  A8 CALLX8,
  A8 A2 0 6 EXTUI,  A8 A8 21 SLLI,  A2 1 MOVI.N,  A9 $14 L L32R,  A10 A10 A9 AND,
  A8 A8 A10 OR,  A10 A3 A3 OR,  MEMW,  A8 A3 0 S32I,  A8 $24 L L32R,  A8 CALLX8,
  A10 A10 A2 OR,  MEMW,  A10 A3 0 S32I.N,  RETW.N,
$A6 L:  END-CODE

CODE BT_tx_if_init   \ .text.BT_tx_if_init $28
  $0 10 LITERALS
    $3FF5103C T,  $801FFFFF T,  $FFE007FF T,  $80000000 T,
    SYMBOL esp_dport_access_reg_read  SYMBOL phy_get_fetx_delay
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
$28 L:
  A1 32 ENTRY,  A3 $0 L L32R,  A2 A2 0 10 EXTUI,  A10 A3 MOV.N,  A8 $10 L L32R,
  A8 CALLX8,
  A2 A2 21 SLLI,  A8 $4 L L32R,  A4 A4 0 10 EXTUI,  A10 A10 A8 AND,  A10 A2 A10 OR,
  A4 A4 1 SLLI,  MEMW,  A10 A3 0 S32I.N,  A8 $14 L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  A10 A3 A3 OR,  A8 $18 L L32R,  A8 CALLX8,
  A8 $3E8 MOVI,  A9 $8 L L32R,  A8 A8 A2 SUB,  A8 A8 0 10 EXTUI,  A10 A10 A9 AND,
  A8 A8 11 SLLI,  A8 A8 A10 OR,  MEMW,  A8 A3 0 S32I,  A10 A3 A3 OR,  A8 $1C L L32R,
  A8 CALLX8,
  A8 -2047 MOVI,  A10 A10 A8 AND,  A10 A4 A10 OR,  MEMW,  A10 A3 0 S32I,
  A10 A3 A3 OR,  A8 $20 L L32R,  A8 CALLX8,
  A2 $C L L32R,  A10 A10 A2 OR,  MEMW,  A10 A3 0 S32I.N,  A5 $BC L BEQZ.N,
  A10 A3 MOV.N,  A8 $24 L L32R,  A8 CALLX8,
  A2 1 MOVI.N,  A10 A10 A2 OR,  MEMW,  A10 A3 0 S32I.N,
$BC L:
  RETW.N,
$BE L:  END-CODE

CODE BT_init_rx_filters   \ .text.BT_init_rx_filters $110
  $0 68 LITERALS
    $3FF51028 T,  $3FF5105C T,  $FC1FFFFF T,  $3400000 T,  $FFFF07FF T,  $9800 T,
    $3FF51058 T,  $FFFF03FF T,  $5000 T,  $3FF51020 T,  $3FF5C07C T,  $FF00FFFF T,
    $A10000 T,  $3FF5C090 T,  $C1FFFFFF T,  $32000000 T,  $FE0FFFFF T,  $700000 T,
    $3FF5C080 T,  $FFFE00FF T,  $1D400 T,  $3FF5C0D0 T,  $FF81FFFF T,  $180000 T,
    $3FF5104C T,  $FFFF83FF T,  $7800 T,  $F0FFFFFF T,  $3000000 T,  $CFFFFFFF T,
    $1FFFFFFF T,  $60000000 T,  $3FF5C084 T,  $FFE07FFF T,  $188000 T,
    $FFFF81FF T,  $1E00 T,  $E7FFFFFF T,  $F83FFFFF T,  $800000 T,  $3FF5106C T,
    $FFF0FFFF T,  $60000 T,  $60032124 T,  $3FF51084 T,  $FB0000 T,
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
$110 L:
  A1 32 ENTRY,  A3 $4 L L32R,  A5 23 MOVI.N,  A10 A3 MOV.N,  A2 A2 0 16 EXTUI,
  A2 A2 NEG,  A4 $0 L L32R,  A2 A2 0 6 EXTUI,  MEMW,  A5 A4 0 S32I.N,
  A8 $B8 L L32R,  A8 CALLX8,
  A4 $8 L L32R,  A5 A10 A4 AND,  A10 A3 A3 OR,  A4 $C L L32R,  A4 A5 A4 OR,  MEMW,
  A4 A3 0 S32I,  A8 $BC L L32R,  A8 CALLX8,
  A5 $10 L L32R,  A10 A10 A5 AND,  A4 $14 L L32R,  A5 A10 A4 OR,  A4 $18 L L32R,
  A10 A4 MOV.N,  MEMW,  A5 A3 0 S32I.N,  A8 $C0 L L32R,  A8 CALLX8,
  A3 $1C L L32R,  A10 A10 A3 AND,  A3 $20 L L32R,  A3 A10 A3 OR,  A5 $24 L L32R,
  A10 A5 MOV.N,  MEMW,  A3 A4 0 S32I.N,  A8 $C4 L L32R,  A8 CALLX8,
  A3 -64 MOVI,  A10 A10 A3 AND,  A2 A2 A10 OR,  A4 $28 L L32R,  A10 A4 MOV.N,  MEMW,
  A2 A5 0 S32I.N,  A8 $C8 L L32R,  A8 CALLX8,
  A5 $2C L L32R,  A10 A10 A5 AND,  A2 $30 L L32R,  A3 A10 A2 OR,  A2 $34 L L32R,
  A10 A2 MOV.N,  MEMW,  A3 A4 0 S32I.N,  A8 $CC L L32R,  A8 CALLX8,
  A3 $38 L L32R,  A6 A10 A3 AND,  A10 A2 A2 OR,  A3 $3C L L32R,  A3 A6 A3 OR,  MEMW,
  A3 A2 0 S32I,  A8 $D0 L L32R,  A8 CALLX8,
  A6 $40 L L32R,  A10 A10 A6 AND,  A3 $44 L L32R,  A6 A10 A3 OR,  A3 $48 L L32R,
  A10 A3 MOV.N,  MEMW,  A6 A2 0 S32I.N,  A8 $D4 L L32R,  A8 CALLX8,
  A2 $4C L L32R,  A10 A10 A2 AND,  A2 $50 L L32R,  A2 A10 A2 OR,  A6 $54 L L32R,
  A10 A6 MOV.N,  MEMW,  A2 A3 0 S32I.N,  A8 $D8 L L32R,  A8 CALLX8,
  A2 -512 MOVI,  A10 A10 A2 AND,  A2 $1D4 MOVI,  A10 A10 A2 OR,  MEMW,
  A10 A6 0 S32I.N,  A10 A3 MOV.N,  A8 $DC L L32R,  A8 CALLX8,
  A2 $58 L L32R,  A10 A10 A2 AND,  A2 $5C L L32R,  A2 A10 A2 OR,  A6 $60 L L32R,
  A10 A6 MOV.N,  MEMW,  A2 A3 0 S32I.N,  A8 $E0 L L32R,  A8 CALLX8,
  A2 $64 L L32R,  A8 A10 A2 AND,  A10 A4 A4 OR,  A2 $68 L L32R,  A2 A8 A2 OR,  MEMW,
  A2 A6 0 S32I,  A8 $E4 L L32R,  A8 CALLX8,
  A2 $6C L L32R,  A8 A10 A2 AND,  A10 A3 MOV.N,  A2 $70 L L32R,  A2 A8 A2 OR,  MEMW,
  A2 A4 0 S32I.N,  A8 $E8 L L32R,  A8 CALLX8,
  A2 -63 MOVI,  A10 A10 A2 AND,  A2 8 MOVI.N,  A10 A10 A2 OR,  MEMW,
  A10 A3 0 S32I.N,  A10 A4 MOV.N,  A8 $EC L L32R,  A8 CALLX8,
  A2 $74 L L32R,  A2 A10 A2 AND,  A10 A3 MOV.N,  MEMW,  A2 A4 0 S32I.N,
  A8 $F0 L L32R,  A8 CALLX8,
  A4 $78 L L32R,  A10 A10 A4 AND,  A2 $7C L L32R,  A4 A10 A2 OR,  A2 $80 L L32R,
  A10 A2 MOV.N,  MEMW,  A4 A3 0 S32I.N,  A8 $F4 L L32R,  A8 CALLX8,
  A3 $84 L L32R,  A4 A10 A3 AND,  A10 A2 MOV.N,  A3 $88 L L32R,  A3 A4 A3 OR,  MEMW,
  A3 A2 0 S32I.N,  A8 $F8 L L32R,  A8 CALLX8,
  A3 $8C L L32R,  A4 A10 A3 AND,  A10 A2 A2 OR,  A3 $90 L L32R,  A3 A4 A3 OR,  MEMW,
  A3 A2 0 S32I,  A8 $FC L L32R,  A8 CALLX8,
  A3 $94 L L32R,  A3 A10 A3 AND,  A10 A2 MOV.N,  MEMW,  A3 A2 0 S32I,
  A8 $100 L L32R,  A8 CALLX8,
  A4 $98 L L32R,  A10 A10 A4 AND,  A3 $9C L L32R,  A4 A10 A3 OR,  A3 $A0 L L32R,
  A10 A3 MOV.N,  MEMW,  A4 A2 0 S32I.N,  A8 $104 L L32R,  A8 CALLX8,
  A2 $A4 L L32R,  A10 A10 A2 AND,  A2 $A8 L L32R,  A2 A10 A2 OR,  A4 $AC L L32R,
  A10 A4 MOV.N,  MEMW,  A2 A3 0 S32I.N,  A8 $108 L L32R,  A8 CALLX8,
  A2 $200 MOVI,  A2 A10 A2 OR,  A3 $B0 L L32R,  A10 A3 A3 OR,  MEMW,  A2 A4 0 S32I,
  A8 $10C L L32R,  A8 CALLX8,
  A10 A10 A5 AND,  A5 $B4 L L32R,  A10 A10 A5 OR,  MEMW,  A10 A3 0 S32I.N,  RETW.N,
$364 L:  END-CODE

CODE bt_dgmixer_fstep_250k   \ .text.bt_dgmixer_fstep_250k $10
  $0 4 LITERALS
    $3FF51040 T,  $FFE00001 T,  $199A T,  SYMBOL esp_dport_access_reg_read
$10 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A10 A2 MOV.N,  A8 $C L L32R,  A8 CALLX8,
  A8 $4 L L32R,  A10 A10 A8 AND,  A8 $8 L L32R,  A10 A10 A8 OR,  MEMW,
  A10 A2 0 S32I.N,  RETW.N,
$31 L:  END-CODE

CODE bt_rfoffset_en   \ .text.bt_rfoffset_en $18
  $0 6 LITERALS
    $3FF51020 T,  $3FF51040 T,  $EFFFFFFF T,  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
$18 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A10 A2 MOV.N,  A8 $C L L32R,  A8 CALLX8,
  A8 $100 MOVI,  A3 $4 L L32R,  A8 A10 A8 OR,  A10 A3 MOV.N,  MEMW,  A8 A2 0 S32I,
  A8 $10 L L32R,  A8 CALLX8,
  A8 $8 L L32R,  A8 A10 A8 AND,  A10 A2 MOV.N,  MEMW,  A8 A3 0 S32I.N,
  A8 $14 L L32R,  A8 CALLX8,
  A8 128 MOVI,  A10 A10 A8 OR,  MEMW,  A10 A2 0 S32I.N,  RETW.N,
$5D L:  END-CODE

CODE bt_bb_init_cmplx   \ .text.bt_bb_init_cmplx $7C
  $0 31 LITERALS
    $3FF51040 T,  $FFE00001 T,  $199A T,  $60032130 T,  $FFFF00FF T,  $8800 T,
    $600320F4 T,  $FFFFC0FF T,  $A00 T,  $600320F0 T,  $FE00FFFF T,  $DC0000 T,
    $600320DC T,  $FFFF7FFF T,  $6000604C T,  $C800 T,  SYMBOL phy_dis_hw_set_freq
    SYMBOL ets_delay_us  SYMBOL BT_tx_8m_enable  SYMBOL BT_tx_if_init
    SYMBOL BT_init_rx_filters  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL phy_en_hw_set_freq
$7C L:
  A1 32 ENTRY,  A8 $40 L L32R,  A8 CALLX8,
  A10 2 MOVI.N,  A8 $44 L L32R,  A8 CALLX8,
  A10 0 MOVI.N,  A8 $48 L L32R,  A8 CALLX8,
  A13 1 MOVI.N,  A12 200 MOVI,  A11 $3E8 MOVI,  A10 $122 MOVI,  A8 $4C L L32R,
  A8 CALLX8,
  A10 56 MOVI.N,  A8 $50 L L32R,  A8 CALLX8,
  A4 $0 L L32R,  A10 A4 MOV.N,  A8 $54 L L32R,  A8 CALLX8,
  A2 $4 L L32R,  A10 A10 A2 AND,  A2 $8 L L32R,  A2 A10 A2 OR,  A3 $C L L32R,
  A10 A3 MOV.N,  MEMW,  A2 A4 0 S32I.N,  A8 $58 L L32R,  A8 CALLX8,
  A2 -256 MOVI,  A10 A10 A2 AND,  A2 230 MOVI,  A10 A10 A2 OR,  MEMW,
  A10 A3 0 S32I,  A10 A3 A3 OR,  A8 $5C L L32R,  A8 CALLX8,
  A2 $10 L L32R,  A10 A10 A2 AND,  A4 $14 L L32R,  A5 A10 A4 OR,  A4 $18 L L32R,
  A10 A4 MOV.N,  MEMW,  A5 A3 0 S32I,  A8 $60 L L32R,  A8 CALLX8,
  A3 $1C L L32R,  A5 A10 A3 AND,  A10 A4 MOV.N,  A3 $20 L L32R,  A3 A5 A3 OR,
  A5 63 MOVI.N,  MEMW,  A3 A4 0 S32I,  A8 $64 L L32R,  A8 CALLX8,
  A5 A10 A5 OR,  A3 $24 L L32R,  A10 A3 MOV.N,  MEMW,  A5 A4 0 S32I.N,
  A8 $68 L L32R,  A8 CALLX8,
  A4 $28 L L32R,  A5 A10 A4 AND,  A10 A3 MOV.N,  A4 $2C L L32R,  A4 A5 A4 OR,  MEMW,
  A4 A3 0 S32I.N,  A8 $6C L L32R,  A8 CALLX8,
  A4 -512 MOVI,  A10 A10 A4 AND,  A4 220 MOVI,  A5 $30 L L32R,  A4 A10 A4 OR,
  A10 A5 MOV.N,  MEMW,  A4 A3 0 S32I.N,  A8 $70 L L32R,  A8 CALLX8,
  A3 $34 L L32R,  A3 A10 A3 AND,  A4 $38 L L32R,  A10 A4 MOV.N,  MEMW,
  A3 A5 0 S32I,  A8 $74 L L32R,  A8 CALLX8,
  A10 A10 A2 AND,  A2 $3C L L32R,  A10 A10 A2 OR,  MEMW,  A10 A4 0 S32I.N,
  A8 $78 L L32R,  A8 CALLX8,
  RETW.N,
$19E L:  END-CODE

CODE bt_bb_init_cmplx_reg   \ .text.bt_bb_init_cmplx_reg $1C
  $0 7 LITERALS
    $3FF51040 T,  $FFE00001 T,  $199A T,  SYMBOL BT_tx_8m_enable
    SYMBOL BT_tx_if_init  SYMBOL BT_init_rx_filters
    SYMBOL esp_dport_access_reg_read
$1C L:
  A1 32 ENTRY,  A10 0 MOVI,  A8 $C L L32R,  A8 CALLX8,
  A13 1 MOVI.N,  A12 200 MOVI,  A11 $3E8 MOVI,  A10 $122 MOVI,  A8 $10 L L32R,
  A8 CALLX8,
  A10 56 MOVI.N,  A8 $14 L L32R,  A8 CALLX8,
  A2 $0 L L32R,  A10 A2 MOV.N,  A8 $18 L L32R,  A8 CALLX8,
  A8 $4 L L32R,  A10 A10 A8 AND,  A8 $8 L L32R,  A10 A10 A8 OR,  MEMW,
  A10 A2 0 S32I.N,  RETW.N,
$5F L:  END-CODE

CODE rw_coex_on   \ .text.rw_coex_on $38
  $0 14 LITERALS
    $600310D0 T,  $60031300 T,  $60033D38 T,  $10000 T,  $60033D30 T,
    $F0FFFFFF T,  $4000000 T,  $FF0FFFFF T,  $400000 T,
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read
$38 L:
  A1 32 ENTRY,  A3 $0 L L32R,  A2 1 MOVI.N,  A10 A3 A3 OR,  A8 $24 L L32R,
  A8 CALLX8,
  A8 A10 A2 OR,  A4 $4 L L32R,  A10 A4 MOV.N,  MEMW,  A8 A3 0 S32I,  A8 $28 L L32R,
  A8 CALLX8,
  A2 A10 A2 OR,  A3 $8 L L32R,  A10 A3 MOV.N,  MEMW,  A2 A4 0 S32I.N,
  A8 $2C L L32R,  A8 CALLX8,
  A10 A10 0 16 EXTUI,  A2 $C L L32R,  A8 A10 A2 OR,  A2 $10 L L32R,  A10 A2 MOV.N,
  MEMW,  A8 A3 0 S32I.N,  A8 $30 L L32R,  A8 CALLX8,
  A8 $14 L L32R,  A9 A10 A8 AND,  A10 A2 A2 OR,  A8 $18 L L32R,  A8 A9 A8 OR,  MEMW,
  A8 A2 0 S32I,  A8 $34 L L32R,  A8 CALLX8,
  A8 $1C L L32R,  A10 A10 A8 AND,  A8 $20 L L32R,  A10 A10 A8 OR,  MEMW,
  A10 A2 0 S32I.N,  RETW.N,
$B7 L:  END-CODE

CODE force_bt_mode   \ .text.force_bt_mode $5C
  $0 23 LITERALS
    $600310D0 T,  $60031300 T,  $3FF5C080 T,  $60033D30 T,  $F0FFFFFF T,
    $1000000 T,  $FF0FFFFF T,  $100000 T,  $60033D38 T,  $FFFF0000 T,
    $60033D40 T,  $600041C4 T,  $3FF5D040 T,  $7FFFFFFF T,
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read
$5C L:
  A1 32 ENTRY,  A4 $0 L L32R,  A2 -2 MOVI.N,  A10 A4 A4 OR,  A8 $38 L L32R,
  A8 CALLX8,
  A8 A10 A2 AND,  A3 $4 L L32R,  A10 A3 MOV.N,  MEMW,  A8 A4 0 S32I,  A8 $3C L L32R,
  A8 CALLX8,
  A2 A10 A2 AND,  A4 $8 L L32R,  A10 A4 MOV.N,  MEMW,  A2 A3 0 S32I,  A8 $40 L L32R,
  A8 CALLX8,
  A3 192 MOVI,  A3 A10 A3 OR,  A2 $C L L32R,  A10 A2 MOV.N,  MEMW,  A3 A4 0 S32I,
  A8 $44 L L32R,  A8 CALLX8,
  A3 $10 L L32R,  A4 A10 A3 AND,  A10 A2 MOV.N,  A3 $14 L L32R,  A3 A4 A3 OR,  MEMW,
  A3 A2 0 S32I.N,  A8 $48 L L32R,  A8 CALLX8,
  A4 $18 L L32R,  A10 A10 A4 AND,  A3 $1C L L32R,  A4 A10 A3 OR,  A3 $20 L L32R,
  A10 A3 MOV.N,  MEMW,  A4 A2 0 S32I.N,  A8 $4C L L32R,  A8 CALLX8,
  A2 $24 L L32R,  A10 A10 A2 AND,  A2 10 MOVI.N,  A2 A10 A2 OR,  A4 $28 L L32R,
  A10 A4 MOV.N,  MEMW,  A2 A3 0 S32I,  A8 $50 L L32R,  A8 CALLX8,
  A2 -57 MOVI,  A10 A10 A2 AND,  A2 16 MOVI.N,  A3 $2C L L32R,  A2 A10 A2 OR,
  A10 A3 MOV.N,  MEMW,  A2 A4 0 S32I.N,  A8 $54 L L32R,  A8 CALLX8,
  A2 64 MOVI.N,  A2 A10 A2 OR,  A4 $30 L L32R,  A10 A4 MOV.N,  MEMW,
  A2 A3 0 S32I.N,  A8 $58 L L32R,  A8 CALLX8,
  A2 $34 L L32R,  A10 A10 A2 AND,  MEMW,  A10 A4 0 S32I.N,  RETW.N,
$13A L:  END-CODE

CODE force_wifi_mode   \ .text.force_wifi_mode $3C
  $0 15 LITERALS
    SYMBOL force_wifi_mode_on  $3FF51098 T,  $600310D0 T,  $60031300 T,
    $3FF5C080 T,  $600041C4 T,  $3FF5D040 T,  $80000000 T,
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL phy_force_wifi_chan
    SYMBOL esp_dport_access_reg_read
$3C L:
  A1 32 ENTRY,  A3 $0 L L32R,  A4 A3 0 L32I.N,  A4 $E4 L BNEZ,
  A2 $60 L BEQZ,
  A2 -2 MOVI.N,  A4 $4 L L32R,  A10 A4 A4 OR,  A8 $20 L L32R,  A8 CALLX8,
  A10 A10 A2 AND,  MEMW,  A10 A4 0 S32I.N,
$60 L:
  A2 -241 MOVI,  NOP.N,  NOP.N,  NOP.N,  A5 $8 L L32R,  A10 A5 A5 OR,
  A8 $24 L L32R,  A8 CALLX8,
  A8 A10 A2 AND,  A4 $C L L32R,  A10 A4 MOV.N,  MEMW,  A8 A5 0 S32I,  A8 $28 L L32R,
  A8 CALLX8,
  A2 A10 A2 AND,  A5 $10 L L32R,  A10 A5 MOV.N,  MEMW,  A2 A4 0 S32I.N,
  A8 $2C L L32R,  A8 CALLX8,
  A2 -193 MOVI,  A10 A10 A2 AND,  A2 128 MOVI,  A4 $14 L L32R,  A2 A10 A2 OR,
  A10 A4 MOV.N,  MEMW,  A2 A5 0 S32I.N,  A8 $30 L L32R,  A8 CALLX8,
  A2 -65 MOVI,  A10 A10 A2 AND,  MEMW,  A10 A4 0 S32I.N,  A8 $34 L L32R,  A8 CALLX8,
  A4 $18 L L32R,  A10 A4 A4 OR,  A8 $38 L L32R,  A8 CALLX8,
  A2 $1C L L32R,  A10 A10 A2 OR,  A2 1 MOVI.N,  MEMW,  A10 A4 0 S32I.N,
  A2 A3 0 S32I.N,
$E4 L:
  RETW.N,
$E6 L:  END-CODE

CODE unforce_wifi_mode   \ .text.unforce_wifi_mode $38
  $0 14 LITERALS
    SYMBOL force_wifi_mode_on  $3FF5110C T,  $3FF5C080 T,  $600041C4 T,
    $3FF5D040 T,  $7FFFFFFF T,  $3FF51098 T,  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL phy_unforce_wifi_chan
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
$38 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A8 A2 0 L32I,  A8 1 $C0 L BNEI,
  A3 $4 L L32R,  A10 A3 A3 OR,  A8 $1C L L32R,  A8 CALLX8,
  A10 30 $5F L BBCI,
  A10 A3 A3 OR,  A8 $20 L L32R,  A8 CALLX8,
  A10 20 $C0 L BBSI,
$5F L:
  A8 $24 L L32R,  A8 CALLX8,
  A4 $8 L L32R,  A10 A4 MOV.N,  A8 $28 L L32R,  A8 CALLX8,
  A8 -193 MOVI,  A3 $C L L32R,  A8 A10 A8 AND,  A10 A3 MOV.N,  MEMW,
  A8 A4 0 S32I.N,  A8 $2C L L32R,  A8 CALLX8,
  A8 64 MOVI.N,  A8 A10 A8 OR,  A4 $10 L L32R,  A10 A4 A4 OR,  MEMW,
  A8 A3 0 S32I.N,  A8 $30 L L32R,  A8 CALLX8,
  A8 $14 L L32R,  A8 A10 A8 AND,  A3 $18 L L32R,  A10 A3 MOV.N,  MEMW,
  A8 A4 0 S32I.N,  A8 $34 L L32R,  A8 CALLX8,
  A8 1 MOVI.N,  A10 A10 A8 OR,  MEMW,  A10 A3 0 S32I.N,  A3 0 MOVI.N,
  A3 A2 0 S32I.N,
$C0 L:
  RETW.N,
$C2 L:  END-CODE

CODE coex_bt_high_prio   \ .text.coex_bt_high_prio $70
  $0 28 LITERALS
    $3FF5C080 T,  $600310D0 T,  $60031300 T,  $60033D30 T,  $F0FFFFFF T,
    $1000000 T,  $FF0FFFFF T,  $400000 T,  $60033D38 T,  $C800000 T,  $FFFF0000 T,
    $60033D40 T,  $600041C4 T,  $3FF5D040 T,  $7FFFFFFF T,
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read
$70 L:
  A1 32 ENTRY,  A3 $0 L L32R,  A2 -193 MOVI,  A10 A3 MOV.N,  A8 $3C L L32R,
  A8 CALLX8,
  A2 A10 A2 AND,  A5 $4 L L32R,  A10 A5 MOV.N,  MEMW,  A2 A3 0 S32I,  A8 $40 L L32R,
  A8 CALLX8,
  A2 -241 MOVI,  A3 A10 A2 AND,  A4 $8 L L32R,  A10 A4 A4 OR,  MEMW,  A3 A5 0 S32I,
  A8 $44 L L32R,  A8 CALLX8,
  A3 A10 A2 AND,  A2 $C L L32R,  A10 A2 A2 OR,  MEMW,  A3 A4 0 S32I,  A8 $48 L L32R,
  A8 CALLX8,
  A3 1 MOVI.N,  A6 A10 A3 OR,  A10 A2 MOV.N,  MEMW,  A6 A2 0 S32I.N,  A8 $4C L L32R,
  A8 CALLX8,
  A6 $10 L L32R,  A8 A10 A6 AND,  A10 A2 MOV.N,  A6 $14 L L32R,  A6 A8 A6 OR,  MEMW,
  A6 A2 0 S32I.N,  A8 $50 L L32R,  A8 CALLX8,
  A8 $18 L L32R,  A10 A10 A8 AND,  A6 $1C L L32R,  A8 A10 A6 OR,  A6 $20 L L32R,
  A10 A6 MOV.N,  MEMW,  A8 A2 0 S32I.N,  A8 $54 L L32R,  A8 CALLX8,
  A8 A10 0 16 EXTUI,  A10 A6 MOV.N,  A2 $24 L L32R,  A2 A8 A2 OR,  MEMW,
  A2 A6 0 S32I,  A8 $58 L L32R,  A8 CALLX8,
  A2 10 MOVI.N,  A8 $28 L L32R,  A10 A10 A8 AND,  A8 A10 A2 OR,  A2 $2C L L32R,
  A10 A2 A2 OR,  MEMW,  A8 A6 0 S32I,  A8 $5C L L32R,  A8 CALLX8,
  A8 -57 MOVI,  A10 A10 A8 AND,  A8 16 MOVI.N,  A6 $30 L L32R,  A8 A10 A8 OR,
  A10 A6 A6 OR,  MEMW,  A8 A2 0 S32I,  A8 $60 L L32R,  A8 CALLX8,
  A2 64 MOVI.N,  A2 A10 A2 OR,  A10 A5 MOV.N,  MEMW,  A2 A6 0 S32I,  A8 $64 L L32R,
  A8 CALLX8,
  A2 A10 A3 OR,  A10 A4 MOV.N,  MEMW,  A2 A5 0 S32I.N,  A8 $68 L L32R,  A8 CALLX8,
  A3 A10 A3 OR,  A5 $34 L L32R,  A10 A5 MOV.N,  MEMW,  A3 A4 0 S32I.N,
  A8 $6C L L32R,  A8 CALLX8,
  A2 $38 L L32R,  A10 A10 A2 AND,  MEMW,  A10 A5 0 S32I.N,  RETW.N,
$19D L:  END-CODE

CODE bt_cmplx_hq_re   \ .text.bt_cmplx_hq_re $4
  $0 1 LITERALS
    SYMBOL g_phyFuns
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A13 6 MOVI.N,  A14 0 MOVI.N,  A12 A13 MOV.N,
  A11 1 MOVI.N,  A8 A8 0 L32I.N,  A10 103 MOVI,  A8 A8 164 L32I,  A8 CALLX8,
  A2 A10 MOV.N,  RETW.N,
$21 L:  END-CODE

CODE bt_cmplx_lq_re   \ .text.bt_cmplx_lq_re $4
  $0 1 LITERALS
    SYMBOL g_phyFuns
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A14 0 MOVI.N,  A13 6 MOVI.N,  A12 5 MOVI.N,
  A11 1 MOVI.N,  A8 A8 0 L32I.N,  A10 103 MOVI,  A8 A8 164 L32I,  A8 CALLX8,
  A2 A10 MOV.N,  RETW.N,
$21 L:  END-CODE

CODE bt_bb_version   \ .text.bt_bb_version $14
  $0 5 LITERALS
    SYMBOL rodata.str1.4  $C SYMBOL+ rodata.str1.4  $18 SYMBOL+ rodata.str1.4
    $24 SYMBOL+ rodata.str1.4  SYMBOL rtc_printf
$14 L:
  A1 32 ENTRY,  A13 $0 L L32R,  A12 $4 L L32R,  A11 $8 L L32R,  A10 $C L L32R,
  A8 $10 L L32R,  A8 CALLX8,
  RETW.N,
$2B L:  END-CODE

LINK-SYMBOLS   \ the cells of other packages that wait for these names
END-PACKAGE  PREVIOUS   \ not in the search order: ALSO name gives its words (README.md)
