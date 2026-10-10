\ hal_mac.fs - package PP-HAL-MAC: hal_mac.o of libpp.a (ESP32)
\
\ Made from hal_mac.S by s2fs.py, not by hand. Each function of C is a
\ CODE word: its bytes are those of the compiler, at the same offsets of
\ its section (L gives the address of an offset); its literals are at the
\ start of the word, after a jump. The data come first, then the
\ functions, each after those that it calls. The global symbols are
\ PUBLIC, the static ones PRIVATE. lib/esp32/clink.fs explains the words.
\
\ 49 functions (49 global), 7 data objects.
\ A function runs as C code with the windowed calls of the Xtensa: call
\ it with CALL-C ( x1 .. xn n xt -- x ), never by its name alone.
\
\ Names of other packages: a literal waits until its package comes
\ (LINK-SYMBOLS); a name outside the libraries until EXTERN gives it.
\ Uses:
\   PHY-PHY-CHIP-V7  bb_wdt_int_enable phy_disable_low_rate
\                    phy_enable_low_rate
\   PP-HAL-CRYPTO  hal_crypto_init
\   PP-HAL-MAC-TX  hal_attenna_init hal_mac_rate_autoack_init
\                  mac_tx_set_htsig mac_tx_set_plcp1 mac_tx_set_plcp2
\   PP-HAL-TSF  hal_disable_sta_tsf
\   PP-WDEV  wDevCtrl wDev_reset_bcnSendTick
\ Outside the libraries (../README.md):
\   ets_delay_us g_mesh_init_ps_type g_mesh_is_root g_mesh_is_started
\   g_osi_funcs_p g_wifi_mac_time_delta g_wifi_menuconfig memcpy wifi_log
\ C names with more than 31 characters (Forth takes 31): the first 26,
\ ~ and 4 hex digits of their CRC-32 (../README.md):
\   rodata.hal_mac_rx_set_poli~3A6E = rodata.hal_mac_rx_set_policy.str1.1
\
\ Needs XTENSA (asm/xtensa.fs), ASM (lib/esp32/asm.fs) and CLINK
\ (lib/esp32/clink.fs) of ucf; the ESP32.

PACKAGE PP-HAL-MAC  ALSO ASM  ALSO CLINK   \ lib/package.fs
EXPORTS   \ the PUBLIC words are symbols for the other packages

\ ==== Data =============================================================

PRIVATE
CREATE rodata.hal_mac_rx_set_poli~3A6E   \ .rodata.hal_mac_rx_set_policy.str1.1, 7 bytes
  255 C,  255 C,  255 C,  255 C,  255 C,  255 C,  0 C,
CREATE rodata_wlog_error.20   \ .rodata_wlog_error.20, 32 bytes
  ," Force TXRAME ACK(0x%08x)=0x%08x"  0 C,
CREATE rodata_wlog_error.19   \ .rodata_wlog_error.19, 32 bytes
  ," Force TXRAME ACK(0x%08x)=0x%08x"  0 C,
CREATE rodata_wlog_error.18   \ .rodata_wlog_error.18, 35 bytes
  ," mac deinit fail, txing can't stop"  10 C,  0 C,
PUBLIC
1 DATA: g_mac_deinit_txing   \ .bss.g_mac_deinit_txing
1 DATA: g_mac_deinit_rxing   \ .bss.g_mac_deinit_rxing
4 DATA: g_mac_deinit_count   \ .bss.g_mac_deinit_count

\ ==== Code =============================================================

CODE mac_tx_set_plcp0   \ .text.mac_tx_set_plcp0 $14
  $0 5 LITERALS
    $FFFFF T,  $2FFFFF T,  $F8FFFFFF T,  $FFFFFF T,  $7FEE7A4 T,
$14 L:
  A1 32 ENTRY,  A10 A2 0 L32I.N,  A9 $0 L L32R,  A8 A10 4 L32I.N,  A8 A8 A9 AND,
  A9 1 MOVI.N,  A9 A9 21 SLLI,  A11 A10 44 L32I.N,  A8 A8 A9 OR,  A10 A2 20 L16SI,
  A9 $4 L L32R,  A8 A8 A9 AND,  A9 A11 0 L32I.N,  A10 1 $58 L BGEI,
  A10 192 MOVI,  A10 A9 A10 AND,  A10 128 $58 L BEQI,
  A10 A11 12 L8UI,  A11 15 MOVI.N,  A10 A10 -16 ADDI,  A10 A10 0 8 EXTUI,
  A11 A10 $58 L BGEU,
  A10 1 MOVI.N,  A10 A10 22 SLLI,  A8 A8 A10 OR,
$58 L:
  A10 $402 MOVI,  A9 A10 $93 L BANY,
  A10 9 MOVI.N,  A10 A10 19 SLLI,  A12 1 MOVI.N,  A10 A9 A10 AND,  A12 A12 22 SLLI,
  A11 1 MOVI.N,  A10 A12 $93 L BEQ,
  A10 $8 L L32R,  A10 A8 A10 AND,  A9 20 $84 L BBCI,
  A8 3 MOVI.N,  A8 A8 24 SLLI,  A8 A10 A8 OR,  $93 L J,
$84 L:
  A9 19 $8D L BBCI,
  A11 A11 25 SLLI,  $90 L J,
$8D L:
  A11 A11 24 SLLI,
$90 L:
  A8 A10 A11 OR,
$93 L:
  A10 A9 9 1 EXTUI,  A9 A9 8 1 EXTUI,  A10 A10 4 SLLI,  A9 A9 3 SLLI,  A9 A10 A9 OR,
  A11 -25 MOVI.N,  A10 A8 24 8 EXTUI,  A10 A10 A11 AND,  A9 A9 A10 OR,
  A10 $C L L32R,  A9 A9 24 SLLI,  A8 A8 A10 AND,  A8 A8 A9 OR,  A9 A2 4 L8UI,
  A2 $10 L L32R,  A2 A2 A9 SUB,  A2 A2 3 SLLI,  MEMW,  A8 A2 0 S32I.N,  A2 0 MOVI.N,
  MEMW,  RETW.N,
$D1 L:  END-CODE

CODE mac_tx_set_duration   \ .text.mac_tx_set_duration $4
  $0 1 LITERALS
    $3FF74268 T,
$4 L:
  A1 32 ENTRY,  A9 A2 0 L32I.N,  A8 A9 4 L32I.N,  A9 A9 44 L32I.N,  A9 A9 0 L32I.N,
  A8 A8 4 L32I,  A9 18 $18 L BBCI,
  A8 A8 4 ADDI,
$18 L:
  A10 A8 2 L16UI,  A10 A10 16 SLLI,  A9 A8 2 L16UI,  A9 A9 A10 OR,  A8 A2 4 L8UI,
  A2 A8 4 SLLI,  A8 A8 A2 SUB,  A2 $0 L L32R,  A8 A8 A2 ADDX4,  A2 0 MOVI.N,  MEMW,
  A9 A8 0 S32I.N,  MEMW,  RETW.N,
$3F L:  END-CODE

CODE hal_mac_tx_set_ppdu   \ .text.hal_mac_tx_set_ppdu $14
  $0 5 LITERALS
    SYMBOL mac_tx_set_plcp0  SYMBOL mac_tx_set_plcp1  SYMBOL mac_tx_set_plcp2
    SYMBOL mac_tx_set_duration  SYMBOL mac_tx_set_htsig
$14 L:
  A1 32 ENTRY,  A10 A2 A2 OR,  A8 $0 L L32R,  A8 CALLX8,
  A10 A2 MOV.N,  A8 $4 L L32R,  A8 CALLX8,
  A10 A2 MOV.N,  A8 $8 L L32R,  A8 CALLX8,
  A10 A2 MOV.N,  A8 $C L L32R,  A8 CALLX8,
  A11 A3 MOV.N,  A10 A2 MOV.N,  A8 $10 L L32R,  A8 CALLX8,
  A2 0 MOVI.N,  RETW.N,
$46 L:  END-CODE

CODE hal_mac_tx_config_timeout   \ .wifi0iram.2 $4
  $0 1 LITERALS
    $3FF73D1C T,
$4 L:
  A1 32 ENTRY,  A8 A2 4 L8UI,  A10 -1 MOVI.N,  A8 A8 3 SLLI,  A10 A10 12 SLLI,
  A3 A3 0 12 EXTUI,  A9 $0 L L32R,  A2 0 MOVI.N,  A9 A9 A8 SUB,  MEMW,
  A8 A9 0 L32I.N,  A8 A8 A10 AND,  A8 A8 A3 OR,  MEMW,  A8 A9 0 S32I.N,  MEMW,
  RETW.N,
$32 L:  END-CODE

CODE hal_mac_tx_config_edca   \ .wifi0iram.3 $C
  $0 3 LITERALS
    $3FF73D1C T,  $F0FFFFFF T,  $FFC00FFF T,
$C L:
  A1 32 ENTRY,  A8 A2 4 L8UI,  A11 15 MOVI.N,  A8 A8 3 SLLI,  A11 A11 24 SLLI,
  A9 $0 L L32R,  A9 A9 A8 SUB,  A8 A2 5 L8UI,  A8 A8 24 SLLI,  A8 A8 A11 AND,  MEMW,
  A10 A9 0 L32I.N,  A11 $4 L L32R,  A10 A10 A11 AND,  A8 A8 A10 OR,  A11 $3FF MOVI,
  MEMW,  A8 A9 0 S32I.N,  A11 A11 12 SLLI,  A8 A2 6 L16UI,  A8 A8 12 SLLI,
  A8 A8 A11 AND,  MEMW,  A10 A9 0 L32I.N,  A2 $8 L L32R,  A10 A10 A2 AND,
  A8 A8 A10 OR,  A2 0 MOVI.N,  MEMW,  A8 A9 0 S32I.N,  MEMW,  RETW.N,
$65 L:  END-CODE

CODE hal_mac_txq_enable   \ .wifi0iram.4 $4
  $0 1 LITERALS
    $7FEE7A4 T,
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A2 0 8 EXTUI,  A8 A8 A2 SUB,  A8 A8 3 SLLI,
  A10 -1 MOVI.N,  MEMW,  A9 A8 0 L32I.N,  A10 A10 30 SLLI,  A9 A9 A10 OR,  MEMW,
  A9 A8 0 S32I.N,  MEMW,  RETW.N,
$2A L:  END-CODE

CODE hal_mac_txq_disable   \ .wifiextrairam.5 $8
  $0 2 LITERALS
    $7FEE7A4 T,  $3FFFFFFF T,
$8 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A2 0 8 EXTUI,  A8 A8 A2 SUB,  A8 A8 3 SLLI,  MEMW,
  A9 A8 0 L32I.N,  A10 $4 L L32R,  A9 A9 A10 AND,  MEMW,  A9 A8 0 S32I.N,  MEMW,
  RETW.N,
$2C L:  END-CODE

CODE hal_mac_tx_get_blockack   \ .wifi0iram.6 $C
  $0 3 LITERALS
    $3FF74284 T,  $3FF74278 T,  $3FF74274 T,
$C L:
  A1 32 ENTRY,  A2 A2 0 8 EXTUI,  A8 A2 4 SLLI,  A2 A2 A8 SUB,  A8 $0 L L32R,
  A8 A2 A8 ADDX4,  MEMW,  A9 A8 0 L32I.N,  A9 A9 12 4 EXTUI,  A9 A3 0 S8I,  MEMW,
  MEMW,  A8 A8 0 L32I.N,  A8 A8 0 12 EXTUI,  A8 A3 2 S16I,  MEMW,  A8 $4 L L32R,
  A8 A2 A8 ADDX4,  MEMW,  A8 A8 0 L32I.N,  A8 A3 4 S32I.N,  A8 $8 L L32R,
  A2 A2 A8 ADDX4,  MEMW,  A2 A2 0 L32I.N,  A2 A3 8 S32I.N,  A2 0 MOVI.N,  MEMW,
  RETW.N,
$5B L:  END-CODE

CODE hal_mac_set_txq_invalid   \ .text.hal_mac_set_txq_invalid $8
  $0 2 LITERALS
    $7FEE7A4 T,  $BFFFFFFF T,
$8 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A2 0 8 EXTUI,  A8 A8 A2 SUB,  A8 A8 3 SLLI,  MEMW,
  A9 A8 0 L32I.N,  A10 $4 L L32R,  A9 A9 A10 AND,  MEMW,  A9 A8 0 S32I.N,  MEMW,
  RETW.N,
$2C L:  END-CODE

CODE hal_mac_is_txq_valid   \ .text.hal_mac_is_txq_valid $4
  $0 1 LITERALS
    $7FEE7A4 T,
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A2 0 8 EXTUI,  A8 A8 A2 SUB,  A8 A8 3 SLLI,  MEMW,
  A2 A8 0 L32I.N,  A2 A2 30 1 EXTUI,  RETW.N,
$1D L:  END-CODE

CODE hal_mac_is_txq_enabled   \ .text.hal_mac_is_txq_enabled $4
  $0 1 LITERALS
    $7FEE7A4 T,
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A2 0 8 EXTUI,  A8 A8 A2 SUB,  A8 A8 3 SLLI,  MEMW,
  A2 A8 0 L32I.N,  A2 A2 31 1 EXTUI,  RETW.N,
$1D L:  END-CODE

CODE hal_random   \ .wifi0iram.7 $4
  $0 1 LITERALS
    $3FF75144 T,
$4 L:
  A1 32 ENTRY,  A2 $0 L L32R,  MEMW,  A2 A2 0 L32I.N,  RETW.N,
$11 L:  END-CODE

CODE hal_now   \ .text.hal_now $18
  $0 6 LITERALS
    SYMBOL g_mesh_is_started  SYMBOL g_mesh_is_root  SYMBOL g_mesh_init_ps_type
    SYMBOL g_osi_funcs_p  $3FF73C00 T,  SYMBOL g_wifi_mac_time_delta
$18 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A8 A8 0 L8UI,  A8 $48 L BEQZ.N,
  A8 $4 L L32R,  MEMW,  A8 A8 0 L8UI,  A8 A8 0 8 EXTUI,  A8 $48 L BNEZ.N,
  A8 $8 L L32R,  A8 A8 0 L32I.N,  A8 $48 L BEQZ.N,
  A8 $C L L32R,  A8 A8 0 L32I,  A10 A8 $110 L32I,  A10 CALLX8,
  $57 L J,
  1 ZEROS,
$48 L:
  A8 $10 L L32R,  MEMW,  A10 A8 0 L32I.N,  A8 $14 L L32R,  A8 A8 0 L32I.N,
  A10 A10 A8 ADD.N,
$57 L:
  A2 A10 MOV.N,  RETW.N,
$5B L:  END-CODE

CODE hal_mac_tx_set_cca   \ .text.hal_mac_tx_set_cca $8
  $0 2 LITERALS
    $3FF73C58 T,  $3FFFFFFF T,
$8 L:
  A1 32 ENTRY,  A9 $0 L L32R,  A2 A2 30 SLLI,  MEMW,  A8 A9 0 L32I.N,
  A10 $4 L L32R,  A8 A8 A10 AND,  A8 A8 A2 OR,  A2 0 MOVI.N,  MEMW,  A8 A9 0 S32I.N,
  MEMW,  RETW.N,
$2B L:  END-CODE

CODE hal_mac_disable_low_rate   \ .text.hal_mac_disable_low_rate $20
  $0 8 LITERALS
    $3FF7341C T,  $90B0B0B T,  $3FF73418 T,  $9090909 T,  $6001C860 T,
    $FFFFF7FF T,  $6001C87C T,  SYMBOL phy_disable_low_rate
$20 L:
  A1 32 ENTRY,  A8 $1C L L32R,  A8 CALLX8,
  A8 $0 L L32R,  A10 -1025 MOVI,  A9 $4 L L32R,  MEMW,  A9 A8 0 S32I.N,
  A8 $8 L L32R,  A9 $C L L32R,  MEMW,  A9 A8 0 S32I.N,  A8 $10 L L32R,  MEMW,
  A9 A8 0 L32I.N,  A9 A9 A10 AND,  MEMW,  A9 A8 0 S32I.N,  MEMW,  A9 A8 0 L32I.N,
  A10 $14 L L32R,  A9 A9 A10 AND,  MEMW,  A9 A8 0 S32I.N,  A9 $18 L L32R,  MEMW,
  A8 A9 0 L32I.N,  A8 A8 A10 AND,  MEMW,  A8 A9 0 S32I.N,  MEMW,  RETW.N,
$77 L:  END-CODE

CODE hal_mac_enable_low_rate   \ .text.hal_mac_enable_low_rate $18
  $0 6 LITERALS
    $3FF7341C T,  $B0B0B0B T,  $3FF73418 T,  $6001C860 T,  $6001C87C T,
    SYMBOL phy_enable_low_rate
$18 L:
  A1 32 ENTRY,  A8 $14 L L32R,  A8 CALLX8,
  A8 $4 L L32R,  A10 $400 MOVI,  A9 $0 L L32R,  MEMW,  A8 A9 0 S32I.N,
  A9 $8 L L32R,  MEMW,  A8 A9 0 S32I.N,  A9 $C L L32R,  MEMW,  A8 A9 0 L32I.N,
  A8 A8 A10 OR,  MEMW,  A8 A9 0 S32I.N,  A8 1 MOVI.N,  A8 A8 11 SLLI,  MEMW,
  A10 A9 0 L32I.N,  A10 A10 A8 OR,  MEMW,  A10 A9 0 S32I.N,  A10 $10 L L32R,  MEMW,
  A9 A10 0 L32I.N,  A8 A9 A8 OR,  MEMW,  A8 A10 0 S32I.N,  MEMW,  RETW.N,
$6E L:  END-CODE

CODE hal_mac_is_low_rate_enabled   \ .wifi0iram.8 $8
  $0 2 LITERALS
    $6001C860 T,  $6001C87C T,
$8 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A8 3 MOVI.N,  A8 A8 10 SLLI,  MEMW,  A9 A2 0 L32I.N,
  A2 0 MOVI.N,  A9 A9 A8 AND,  A9 A8 $2C L BNE,
  A2 $4 L L32R,  MEMW,  A2 A2 0 L32I,  A2 A2 11 1 EXTUI,
$2C L:
  RETW.N,
$2E L:  END-CODE

CODE mac_rxbuf_init   \ .text.mac_rxbuf_init $28
  $0 10 LITERALS
    $3FF7311C T,  $3FFAE000 T,  $3FF73124 T,  $3FF73118 T,  $400A0000 T,
    $3FF73120 T,  $3FF73080 T,  SYMBOL wDevCtrl  $3FF73088 T,  $DFFFFFFF T,
$28 L:
  A1 32 ENTRY,  A8 $4 L L32R,  A10 -256 MOVI,  A9 $0 L L32R,  MEMW,  A8 A9 0 S32I.N,
  A9 $8 L L32R,  MEMW,  A8 A9 0 S32I.N,  A8 $10 L L32R,  A9 $C L L32R,  MEMW,
  A8 A9 0 S32I.N,  A9 $14 L L32R,  MEMW,  A8 A9 0 S32I.N,  A8 $18 L L32R,  MEMW,
  A9 A8 0 L32I.N,  A9 A9 A10 AND,  MEMW,  A9 A8 0 S32I.N,  A9 $1C L L32R,
  A10 A9 0 L32I.N,  A9 $20 L L32R,  MEMW,  A10 A9 0 S32I.N,  MEMW,  A9 A8 0 L32I.N,
  A10 $24 L L32R,  A9 A9 A10 AND,  MEMW,  A9 A8 0 S32I.N,  MEMW,  RETW.N,
$86 L:  END-CODE

CODE hal_disable_mac   \ .text.hal_disable_mac $10
  $0 4 LITERALS
    $3FF73C04 T,  $3FF73C40 T,  $3FF73C44 T,  $3FF73C30 T,
$10 L:
  A1 32 ENTRY,  A9 $0 L L32R,  A2 240 MOVI,  MEMW,  A8 A9 0 L32I.N,  A8 A8 A2 OR,
  MEMW,  A8 A9 0 S32I.N,  A8 0 MOVI.N,  A9 $4 L L32R,  MEMW,  A2 A9 0 L32I.N,  MEMW,
  A8 A9 0 S32I.N,  A9 $8 L L32R,  MEMW,  A8 A9 0 S32I.N,  A9 $C L L32R,  MEMW,
  A8 A9 0 S32I.N,  MEMW,  RETW.N,
$4A L:  END-CODE

CODE hal_enable_mac   \ .text.hal_enable_mac $C
  $0 3 LITERALS
    $3FF73C04 T,  $3FF73C40 T,  $3FF73088 T,
$C L:
  A1 32 ENTRY,  A9 $0 L L32R,  A10 -241 MOVI,  MEMW,  A8 A9 0 L32I.N,
  A8 A8 A10 AND,  MEMW,  A8 A9 0 S32I.N,  A8 $4 L L32R,  MEMW,  A2 A8 0 S32I.N,
  A8 $8 L L32R,  MEMW,  A3 A8 0 S32I.N,  MEMW,  RETW.N,
$37 L:  END-CODE

CODE hal_mac_rx_read_rxdscrlast   \ .wifislpiram.9 $4
  $0 1 LITERALS
    $3FF73090 T,
$4 L:
  A1 32 ENTRY,  A2 $0 L L32R,  MEMW,  A2 A2 0 L32I.N,  RETW.N,
$11 L:  END-CODE

CODE hal_mac_rx_read_rxdscrnext   \ .wifislprxiram.10 $4
  $0 1 LITERALS
    $3FF7308C T,
$4 L:
  A1 32 ENTRY,  A2 $0 L L32R,  MEMW,  A2 A2 0 L32I.N,  RETW.N,
$11 L:  END-CODE

CODE hal_mac_rx_set_base   \ .wifislprxiram.11 $4
  $0 1 LITERALS
    $3FF73088 T,
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  MEMW,  A2 A8 0 S32I.N,  MEMW,  RETW.N,
$14 L:  END-CODE

CODE mac_txrx_init   \ .text.mac_txrx_init $68
  $0 26 LITERALS
    $3FF730D8 T,  $3FF73C6C T,  $80802000 T,  $3FF73C74 T,  $3FF730F8 T,
    $3FF730FC T,  $3FF73100 T,  $3FF73104 T,  $3FF7310C T,  $3FF73114 T,
    $F00FFFFF T,  $3FF73C78 T,  $3FF73D78 T,  $3FF73C54 T,  $FFFF00FF T,
    $3FF73C1C T,  $3FF73C20 T,  $3FF73C24 T,  $3FF73CAC T,  $3FF73C5C T,
    $7FFF0000 T,  $3FF73C88 T,  $F0FFFFFF T,  $3FF73288 T,  $3FF73084 T,
    $7FFFFFFF T,
$68 L:
  A1 32 ENTRY,  A8 $4 L L32R,  A11 64 MOVI.N,  A12 -33 MOVI,  MEMW,  A9 A8 0 L32I.N,
  A10 $8 L L32R,  A9 A9 A10 OR,  A10 $100 MOVI,  MEMW,  A9 A8 0 S32I.N,  MEMW,
  A9 A8 0 L32I.N,  A9 A9 A10 OR,  MEMW,  A9 A8 0 S32I.N,  A9 4 MOVI,  A8 $0 L L32R,
  A9 $B8 L LOOP,
  MEMW,  A10 A8 0 L32I.N,  A10 A10 A11 OR,  MEMW,  A10 A8 0 S32I.N,  MEMW,
  A10 A8 0 L32I.N,  A10 A10 A12 AND,  MEMW,  A10 A8 0 S32I.N,  A8 A8 4 ADDI.N,
$B8 L:
  A10 8 MOVI.N,  A13 -241 MOVI,  A9 $C L L32R,  A14 240 MOVI,  A15 1 MOVI.N,
  A15 A15 30 SLLI,  MEMW,  A8 A9 0 L32I.N,  A8 A8 A10 OR,  MEMW,  A8 A9 0 S32I.N,
  A9 $10 L L32R,  MEMW,  A8 A9 0 L32I.N,  A8 A8 0 16 EXTUI,  MEMW,  A8 A9 0 S32I.N,
  A8 $14 L L32R,  MEMW,  A10 A8 0 L32I.N,  A10 A10 0 16 EXTUI,  A12 $18 L L32R,
  MEMW,  A10 A8 0 S32I.N,  MEMW,  A10 A12 0 L32I.N,  A10 A10 0 16 EXTUI,  MEMW,
  A10 A12 0 S32I.N,  A12 $1C L L32R,  MEMW,  A10 A12 0 L32I.N,  A10 A10 0 16 EXTUI,
  MEMW,  A10 A12 0 S32I.N,  A10 1 MOVI.N,  A10 A10 24 SLLI,  MEMW,  A12 A9 0 L32I.N,
  A12 A12 A10 OR,  MEMW,  A12 A9 0 S32I.N,  MEMW,  A12 A8 0 L32I.N,  A10 A12 A10 OR,
  MEMW,  A10 A8 0 S32I.N,  A10 1 MOVI.N,  A10 A10 26 SLLI,  MEMW,  A12 A9 0 L32I.N,
  A12 A12 A10 OR,  MEMW,  A12 A9 0 S32I.N,  MEMW,  A9 A8 0 L32I.N,  A9 A9 A10 OR,
  MEMW,  A9 A8 0 S32I.N,  A9 $20 L L32R,  MEMW,  A8 A9 0 L32I.N,  A8 A8 A13 AND,
  A10 $24 L L32R,  MEMW,  A8 A9 0 S32I.N,  A9 -1 MOVI.N,  A9 A9 31 SLLI,  MEMW,
  A8 A10 0 L32I.N,  A8 A8 A9 OR,  MEMW,  A8 A10 0 S32I.N,  MEMW,  A8 A10 0 L32I.N,
  A12 $28 L L32R,  A8 A8 A12 AND,  A12 27 MOVI.N,  A12 A12 20 SLLI,  A8 A8 A12 OR,
  MEMW,  A8 A10 0 S32I.N,  A12 3 MOVI.N,  A10 $2C L L32R,  MEMW,  A8 A10 0 L32I.N,
  A8 A8 A12 OR,  A12 -256 MOVI,  MEMW,  A8 A10 0 S32I.N,  A10 $30 L L32R,  MEMW,
  A8 A10 0 L32I.N,  A8 A8 A13 AND,  MEMW,  A8 A10 0 S32I.N,  A10 $34 L L32R,  MEMW,
  A8 A10 0 L32I.N,  A8 A8 A12 AND,  A12 4 MOVI.N,  A8 A8 A12 OR,  MEMW,
  A8 A10 0 S32I.N,  MEMW,  A8 A10 0 L32I.N,  A12 $38 L L32R,  A8 A8 A12 AND,
  A12 $400 MOVI,  A8 A8 A12 OR,  MEMW,  A8 A10 0 S32I.N,  A10 -1 MOVI.N,
  A10 A10 12 SLLI,  A8 $3C L L32R,  MEMW,  A12 A8 0 L32I.N,  A12 A12 A10 AND,
  A12 A12 A14 OR,  MEMW,  A12 A8 0 S32I.N,  MEMW,  A12 A8 0 L32I.N,  A12 A12 A9 OR,
  MEMW,  A12 A8 0 S32I.N,  MEMW,  A12 A8 0 L32I.N,  A12 A12 A15 OR,  MEMW,
  A12 A8 0 S32I.N,  A12 $40 L L32R,  MEMW,  A8 A12 0 L32I.N,  A8 A8 A10 AND,
  A8 A8 A14 OR,  MEMW,  A8 A12 0 S32I.N,  A12 $44 L L32R,  MEMW,  A8 A12 0 L32I.N,
  A8 A8 A10 AND,  A8 A8 A14 OR,  A10 $48 L L32R,  MEMW,  A8 A12 0 S32I.N,  MEMW,
  A8 A10 0 L32I.N,  A8 A8 A13 AND,  A8 A8 A11 OR,  MEMW,  A8 A10 0 S32I.N,
  A8 $4C L L32R,  A11 $50 L L32R,  MEMW,  A10 A8 0 L32I.N,  A10 A10 A11 OR,  MEMW,
  A10 A8 0 S32I.N,  MEMW,  A10 A8 0 L32I.N,  A9 A10 A9 OR,  MEMW,  A9 A8 0 S32I.N,
  A9 $54 L L32R,  A10 $58 L L32R,  MEMW,  A8 A9 0 L32I.N,  A8 A8 A10 AND,
  A10 2 MOVI.N,  MEMW,  A8 A9 0 S32I.N,  A9 $5C L L32R,  MEMW,  A8 A9 0 L32I.N,
  A8 A8 A10 OR,  MEMW,  A8 A9 0 S32I.N,  A9 $60 L L32R,  A10 $64 L L32R,  MEMW,
  A8 A9 0 L32I.N,  A8 A8 A10 AND,  MEMW,  A8 A9 0 S32I.N,  MEMW,  RETW.N,
$2A8 L:  END-CODE

CODE hal_mac_set_rxq_policy   \ .text.hal_mac_set_rxq_policy $4
  $0 1 LITERALS
    $FFDCC36 T,
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A2 0 8 EXTUI,  A2 A2 A8 ADD.N,  A2 A2 2 SLLI,
  A3 A3 0 8 EXTUI,  MEMW,  A8 A2 0 L32I.N,  A3 $24 L BEQZ.N,
  A9 2 MOVI.N,  A8 A8 A9 OR,  $29 L J,
$24 L:
  A9 -3 MOVI.N,  A8 A8 A9 AND,
$29 L:
  MEMW,  A8 A2 0 S32I.N,  MEMW,  RETW.N,
$33 L:  END-CODE

CODE mac_last_rxbuf_init   \ .text.mac_last_rxbuf_init $18
  $0 6 LITERALS
    $3FF7314C T,  $3000 T,  $3FF73158 T,  $3FF73164 T,  $3FF73148 T,  $3FF7309C T,
$18 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A10 1 MOVI.N,  A10 A10 11 SLLI,  A9 $4 L L32R,  MEMW,
  A9 A8 0 S32I.N,  A9 0 MOVI.N,  A8 $8 L L32R,  MEMW,  A9 A8 0 S32I.N,
  A9 12 MOVI.N,  A8 $C L L32R,  MEMW,  A9 A8 0 S32I.N,  A8 $10 L L32R,  MEMW,
  A9 A8 0 L32I.N,  A9 A9 A10 OR,  A10 $100 MOVI,  MEMW,  A9 A8 0 S32I.N,  MEMW,
  A9 A8 0 L32I.N,  A9 A9 A10 OR,  A10 $14 L L32R,  MEMW,  A9 A8 0 S32I.N,
  A9 1 MOVI.N,  A9 A9 27 SLLI,  MEMW,  A8 A10 0 L32I.N,  A8 A8 A9 OR,  MEMW,
  A8 A10 0 S32I.N,  MEMW,  RETW.N,
$79 L:  END-CODE

CODE mac_last_rxbuf_deinit   \ .text.mac_last_rxbuf_deinit $1C
  $0 7 LITERALS
    $3FF7314C T,  $3FF73158 T,  $3FF73164 T,  $3FF73148 T,  $FFFFF7FF T,
    $3FF7309C T,  $F7FFFFFF T,
$1C L:
  A1 32 ENTRY,  A9 $0 L L32R,  A8 0 MOVI.N,  MEMW,  A8 A9 0 S32I.N,  A9 $4 L L32R,
  MEMW,  A8 A9 0 S32I.N,  A9 $8 L L32R,  MEMW,  A8 A9 0 S32I.N,  A8 $C L L32R,
  A10 $10 L L32R,  MEMW,  A9 A8 0 L32I.N,  A9 A9 A10 AND,  A10 -257 MOVI,  MEMW,
  A9 A8 0 S32I.N,  MEMW,  A9 A8 0 L32I.N,  A9 A9 A10 AND,  MEMW,  A9 A8 0 S32I.N,
  A9 $14 L L32R,  A10 $18 L L32R,  MEMW,  A8 A9 0 L32I.N,  A8 A8 A10 AND,  MEMW,
  A8 A9 0 S32I.N,  MEMW,  RETW.N,
$74 L:  END-CODE

CODE mac_rxbuf_disable   \ .text.mac_rxbuf_disable $0
  $0 ORIGIN
  A1 32 ENTRY,  RETW.N,
$5 L:  END-CODE

CODE hal_deinit   \ .text.hal_deinit $14
  $0 5 LITERALS
    $3FF73288 T,  $3FF73C40 T,  $FE17C61F T,  $3FF73C4C T,  $3FF73D24 T,
$14 L:
  A1 32 ENTRY,  A9 $0 L L32R,  A10 1 MOVI.N,  MEMW,  A8 A9 0 L32I.N,  A8 A8 A10 OR,
  MEMW,  A8 A9 0 S32I.N,  A8 $4 L L32R,  A10 $8 L L32R,  MEMW,  A9 A8 0 L32I.N,
  A9 A9 A10 AND,  A10 2 MOVI.N,  MEMW,  A9 A8 0 S32I.N,  A9 0 MOVI.N,  MEMW,
  A9 A8 0 S32I.N,  A9 -1 MOVI.N,  A8 $C L L32R,  MEMW,  A9 A8 0 S32I.N,
  A8 $10 L L32R,  MEMW,  A9 A8 0 L32I.N,  A9 A9 A10 OR,  MEMW,  A9 A8 0 S32I,
$60 L:
  MEMW,  A9 A8 0 L32I.N,  A9 0 $60 L BBCI,
  MEMW,  RETW.N,
$6D L:  END-CODE

CODE hal_mac_interrupt_get_event   \ .iram1.12 $4
  $0 1 LITERALS
    $3FF73C48 T,
$4 L:
  A1 32 ENTRY,  A2 $0 L L32R,  MEMW,  A2 A2 0 L32I.N,  RETW.N,
$11 L:  END-CODE

CODE hal_mac_interrupt_clr_event   \ .iram1.13 $4
  $0 1 LITERALS
    $3FF73C4C T,
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  MEMW,  A2 A8 0 S32I.N,  MEMW,  RETW.N,
$14 L:  END-CODE

CODE hal_mac_interrupt_clr_watchdog   \ .iram1.14 $4
  $0 1 LITERALS
    $3FF73C4C T,
$4 L:
  A1 32 ENTRY,  A10 $0 L L32R,  A9 1 MOVI.N,  A9 A9 11 SLLI,  MEMW,
  A8 A10 0 L32I.N,  A8 A8 A9 OR,  MEMW,  A8 A10 0 S32I.N,  MEMW,  RETW.N,
$21 L:  END-CODE

CODE hal_mac_set_addr   \ .text.hal_mac_set_addr $8
  $0 2 LITERALS
    $7FEE608 T,  $FFFF T,
$8 L:
  A1 32 ENTRY,  A8 A3 1 L8UI,  A2 A2 0 8 EXTUI,  A8 A8 8 SLLI,  A9 A3 0 L8UI,
  A8 A8 A9 OR,  A9 A3 2 L8UI,  A9 A9 16 SLLI,  A9 A9 A8 OR,  A8 A3 3 L8UI,
  A8 A8 24 SLLI,  A8 A8 A9 OR,  A9 $0 L L32R,  A2 A2 A9 ADD.N,  A2 A2 3 SLLI,  MEMW,
  A8 A2 0 S32I.N,  A8 A3 4 L8UI,  A3 A3 5 L8UI,  A3 A3 8 SLLI,  A3 A3 A8 OR,
  A8 1 MOVI.N,  MEMW,  A3 A2 4 S32I.N,  A3 -1 MOVI.N,  A8 A8 16 SLLI,  MEMW,
  A3 A2 32 S32I.N,  A3 $4 L L32R,  MEMW,  A3 A2 36 S32I.N,  MEMW,  A3 A2 36 L32I.N,
  A3 A3 A8 OR,  MEMW,  A3 A2 36 S32I.N,  MEMW,  RETW.N,
$70 L:  END-CODE

CODE hal_mac_set_bssid   \ .text.hal_mac_set_bssid $18
  $0 6 LITERALS
    $3FF73024 T,  $FFFEFFFF T,  $3FF73000 T,  $3FF73004 T,  $3FF73020 T,  $FFFF T,
$18 L:
  A1 32 ENTRY,  A9 A3 1 L8UI,  A2 A2 0 8 EXTUI,  A9 A9 8 SLLI,  A8 A3 0 L8UI,
  A9 A9 A8 OR,  A8 A3 2 L8UI,  A8 A8 16 SLLI,  A8 A8 A9 OR,  A9 A3 3 L8UI,
  A9 A9 24 SLLI,  A9 A9 A8 OR,  A8 $0 L L32R,  A8 A2 A8 ADDX8,  MEMW,
  A10 A8 0 L32I.N,  A11 $4 L L32R,  A10 A10 A11 AND,  A11 -1 MOVI.N,
  A11 A11 16 SLLI,  MEMW,  A10 A8 0 S32I.N,  A10 $8 L L32R,  A10 A2 A10 ADDX8,
  MEMW,  A9 A10 0 S32I.N,  A10 $C L L32R,  A10 A2 A10 ADDX8,  MEMW,
  A9 A10 0 L32I.N,  A9 A9 A11 AND,  A11 A3 4 L8UI,  A3 A3 5 L8UI,  A3 A3 8 SLLI,
  A3 A3 A11 OR,  A9 A9 A3 OR,  A3 $10 L L32R,  A2 A2 A3 ADDX8,  A3 -1 MOVI.N,  MEMW,
  A9 A10 0 S32I.N,  MEMW,  A3 A2 0 S32I.N,  A3 1 MOVI.N,  A3 A3 16 SLLI,
  A2 $14 L L32R,  MEMW,  A2 A8 0 S32I.N,  MEMW,  A2 A8 0 L32I.N,  A2 A2 A3 OR,
  MEMW,  A2 A8 0 S32I.N,  MEMW,  RETW.N,
$B0 L:  END-CODE

CODE hal_mac_rx_set_policy   \ .text.hal_mac_rx_set_policy $1C
  $0 7 LITERALS
    $FFDCC36 T,  $3FF73024 T,  $FFFEFFFF T,
    SYMBOL rodata.hal_mac_rx_set_poli~3A6E  $3FF73064 T,  SYMBOL memcpy
    SYMBOL hal_mac_set_bssid
$1C L:
  A1 48 ENTRY,  A2 A2 0 8 EXTUI,  A2 3 $28 L BLTUI,
  $DA L J,
$28 L:
  A8 $0 L L32R,  A8 A2 A8 ADD.N,  A8 A8 2 SLLI,  A3 2 $41 L BGEUI,
  MEMW,  A3 A8 0 L32I.N,  A9 -273 MOVI,  A3 A3 A9 AND,  $4C L J,
$41 L:
  MEMW,  A3 A8 0 L32I.N,  A9 $110 MOVI,  A3 A3 A9 OR,
$4C L:
  MEMW,  A3 A8 0 S32I.N,  A3 A2 3 SLLI,  A4 $71 L BNEZ.N,
  A2 $4 L L32R,  A2 A3 A2 ADD.N,  MEMW,  A4 A2 0 L32I.N,  A8 $8 L L32R,
  A4 A4 A8 AND,  MEMW,  A4 A2 0 S32I.N,  MEMW,  $A8 L J,
$71 L:
  A4 1 $91 L BNEI,
  A2 $4 L L32R,  A4 A4 16 SLLI,  A2 A3 A2 ADD.N,  MEMW,  A8 A2 0 L32I.N,
  A8 A8 A4 OR,  MEMW,  A8 A2 0 S32I.N,  MEMW,  $A8 L J,
  2 ZEROS,
$91 L:
  A11 $C L L32R,  A12 6 MOVI.N,  A10 A1 MOV.N,  A8 $14 L L32R,  A8 CALLX8,
  A11 A1 MOV.N,  A10 A2 MOV.N,  A8 $18 L L32R,  A8 CALLX8,
$A8 L:
  A2 $10 L L32R,  A8 A3 A2 ADD.N,  MEMW,  A2 A8 0 L32I.N,  A5 $C5 L BNEZ.N,
  A3 $8 L L32R,  A2 A2 A3 AND,  MEMW,  A2 A8 0 S32I.N,  MEMW,  $DC L J,
$C5 L:
  A3 1 MOVI.N,  A3 A3 16 SLLI,  A2 A2 A3 OR,  MEMW,  A2 A8 0 S32I.N,  A5 0 MOVI.N,
  MEMW,  $DC L J,
$DA L:
  A5 -1 MOVI.N,
$DC L:
  A2 A5 MOV.N,  RETW.N,
$E0 L:  END-CODE

CODE hal_init   \ .text.hal_init $40
  $0 16 LITERALS
    $3FF73D24 T,  $3FF73C40 T,  $3FF73C4C T,  $3FF730D8 T,  $FFFFF6FF T,
    $1E839E0 T,  $3FF7309C T,  SYMBOL mac_txrx_init  SYMBOL hal_mac_rx_set_policy
    SYMBOL mac_rxbuf_init  SYMBOL mac_last_rxbuf_init
    SYMBOL hal_mac_rate_autoack_init  SYMBOL hal_mac_disable_low_rate
    SYMBOL hal_crypto_init  SYMBOL hal_attenna_init  SYMBOL bb_wdt_int_enable
$40 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A8 2 MOVI.N,  MEMW,  A3 A2 0 L32I.N,  A3 A3 A8 OR,
  MEMW,  A3 A2 0 S32I.N,
$55 L:
  MEMW,  A3 A2 0 L32I.N,  A3 0 $55 L BBCI,
  A2 0 MOVI.N,  A8 -1 MOVI.N,  A3 $4 L L32R,  MEMW,  A2 A3 0 S32I.N,  A3 $8 L L32R,
  MEMW,  A8 A3 0 S32I.N,  MEMW,  A8 $1C L L32R,  A8 CALLX8,
  A3 A2 MOV.N,
$7C L:
  A10 5 MOVI.N,  A13 A3 MOV.N,  A8 $C L L32R,  A12 A3 MOV.N,  A8 A2 A8 ADDX4,
  A11 A3 MOV.N,  MEMW,  A9 A8 0 L32I.N,  A9 A9 A10 OR,  MEMW,  A9 A8 0 S32I.N,
  MEMW,  A9 A8 0 L32I.N,  A10 $10 L L32R,  A9 A9 A10 AND,  A10 A2 MOV.N,
  A2 A2 1 ADDI.N,  MEMW,  A9 A8 0 S32I.N,  MEMW,  A8 $20 L L32R,  A8 CALLX8,
  A2 4 $7C L BNEI,
  A8 $24 L L32R,  A8 CALLX8,
  A8 $28 L L32R,  A8 CALLX8,
  A8 $2C L L32R,  A8 CALLX8,
  A8 $30 L L32R,  A8 CALLX8,
  A8 $34 L L32R,  A8 CALLX8,
  A8 $38 L L32R,  A8 CALLX8,
  A2 $4 L L32R,  A10 1 MOVI.N,  A3 $14 L L32R,  MEMW,  A3 A2 0 S32I.N,  MEMW,
  A8 $3C L L32R,  A8 CALLX8,
  A8 -256 MOVI,  A3 $18 L L32R,  MEMW,  A2 A3 0 L32I.N,  A2 A2 A8 AND,  A8 2 MOVI.N,
  A2 A2 A8 OR,  MEMW,  A2 A3 0 S32I.N,  MEMW,  RETW.N,
$10E L:  END-CODE

CODE hal_mac_clr_bssid   \ .text.hal_mac_clr_bssid $8
  $0 2 LITERALS
    $3FF73024 T,  $FFFEFFFF T,
$8 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A2 0 8 EXTUI,  A2 A2 A8 ADDX8,  MEMW,
  A8 A2 0 L32I.N,  A9 $4 L L32R,  A8 A8 A9 AND,  MEMW,  A8 A2 0 S32I.N,  MEMW,
  RETW.N,
$29 L:  END-CODE

CODE hal_mac_tsf_get_time   \ .text.hal_mac_tsf_get_time $10
  $0 4 LITERALS
    $3FF75010 T,  $3FF75094 T,  $3FF75090 T,  $3FF75054 T,
$10 L:
  A1 32 ENTRY,  A3 A2 MOV.N,  A2 1 $4C L BEQI,
  A2 2 $4C L BEQI,
  A2 $24 L BEQZ.N,
  A2 0 MOVI.N,  A3 A2 MOV.N,  $7D L J,
$24 L:
  A8 $0 L L32R,  A2 2 MOVI.N,  A10 -3 MOVI.N,  MEMW,  A9 A8 0 L32I.N,  A9 A9 A2 OR,
  A2 $C L L32R,  MEMW,  A9 A8 0 S32I.N,  MEMW,  A2 A2 0 L32I.N,  MEMW,
  A9 A8 0 L32I.N,  MEMW,  $75 L J,
  1 ZEROS,
$4C L:
  A8 $0 L L32R,  A2 4 MOVI.N,  A10 -5 MOVI.N,  MEMW,  A3 A8 0 L32I.N,  A3 A3 A2 OR,
  A2 $4 L L32R,  MEMW,  A3 A8 0 S32I.N,  MEMW,  A3 A2 0 L32I.N,  A2 $8 L L32R,
  MEMW,  A2 A2 0 L32I.N,  MEMW,  A9 A8 0 L32I.N,
$75 L:
  A9 A9 A10 AND,  MEMW,  A9 A8 0 S32I.N,
$7D L:
  MEMW,  RETW.N,
$82 L:  END-CODE

CODE wDev_Mesh_Disable_Tsf   \ .text.wDev_Mesh_Disable_Tsf $4
  $0 1 LITERALS
    SYMBOL hal_disable_sta_tsf
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$F L:  END-CODE

CODE hal_get_tsf_time   \ .iram1.15 $14
  $0 5 LITERALS
    $3FF75010 T,  $3FF75018 T,  $3FF75014 T,  $3FF75058 T,  $3FF75054 T,
$14 L:
  A1 32 ENTRY,  A8 $0 L L32R,  MEMW,  A3 A8 0 L32I.N,  A2 $48 L BNEZ.N,
  A2 1 MOVI.N,  A3 A3 A2 OR,  A10 -2 MOVI.N,  A2 $4 L L32R,  MEMW,  A3 A8 0 S32I.N,
  MEMW,  A3 A2 0 L32I.N,  A2 $8 L L32R,  MEMW,  A2 A2 0 L32I.N,  MEMW,
  A9 A8 0 L32I.N,  MEMW,  $69 L J,
$48 L:
  A2 2 MOVI.N,  A3 A3 A2 OR,  A2 $C L L32R,  A10 -3 MOVI.N,  MEMW,  A3 A8 0 S32I.N,
  MEMW,  A3 A2 0 L32I.N,  A2 $10 L L32R,  MEMW,  A2 A2 0 L32I.N,  MEMW,
  A9 A8 0 L32I.N,
$69 L:
  A9 A9 A10 AND,  MEMW,  A9 A8 0 S32I.N,  MEMW,  RETW.N,
$76 L:  END-CODE

CODE hal_mac_tsf_set_time   \ .text.hal_mac_tsf_set_time $14
  $0 5 LITERALS
    $3FF7509C T,  $3FF750A0 T,  $3FF75010 T,  $3FF75060 T,  $3FF75064 T,
$14 L:
  A1 32 ENTRY,  A2 1 $49 L BEQI,
  A2 2 $6C L BNEI,
  A2 $0 L L32R,  MEMW,  A4 A2 0 S32I.N,  A2 $4 L L32R,  A4 $8 L L32R,  MEMW,
  A5 A2 0 S32I.N,  A2 $100 MOVI,  MEMW,  A8 A4 0 L32I.N,  A8 A8 A2 OR,  MEMW,
  A8 A4 0 S32I.N,  MEMW,  $6C L J,
  3 ZEROS,
$49 L:
  A2 $C L L32R,  MEMW,  A4 A2 0 S32I.N,  A2 $10 L L32R,  A9 $8 L L32R,  MEMW,
  A5 A2 0 S32I.N,  A2 128 MOVI,  MEMW,  A8 A9 0 L32I.N,  A8 A8 A2 OR,  MEMW,
  A8 A9 0 S32I.N,
$6C L:
  MEMW,  RETW.N,
$71 L:  END-CODE

CODE hal_mac_tsf_reset   \ .text.hal_mac_tsf_reset $34
  $0 13 LITERALS
    $3FF75098 T,  $7FFFFFFF T,  $3FF7509C T,  $3FF750A0 T,  $3FF75010 T,
    $3FF7505C T,  $3DFFFFFF T,  $3FF75060 T,  $3FF75064 T,  $3FF75020 T,
    $3FF75024 T,  $3FF75028 T,  SYMBOL wDev_reset_bcnSendTick
$34 L:
  A1 32 ENTRY,  A2 1 $3D L BNEI,
  $E8 L J,
$3D L:
  A2 2 $46 L BEQI,
  A2 $170 L BNEZ,
  $90 L J,
$46 L:
  A2 $0 L L32R,  A9 $100 MOVI,  MEMW,  A3 A2 0 L32I.N,  A8 $4 L L32R,  A3 A3 A8 AND,
  A8 $8 L L32R,  MEMW,  A3 A2 0 S32I.N,  A3 0 MOVI.N,  MEMW,  A3 A8 0 S32I.N,
  A8 $C L L32R,  MEMW,  A3 A8 0 S32I.N,  A8 $10 L L32R,  MEMW,  A3 A8 0 L32I.N,
  A3 A3 A9 OR,  MEMW,  A3 A8 0 S32I.N,  A8 -1 MOVI.N,  A8 A8 31 SLLI,  MEMW,
  A3 A2 0 L32I.N,  MEMW,  $167 L J,
  2 ZEROS,
$90 L:
  A3 $14 L L32R,  A9 $18 L L32R,  MEMW,  A8 A3 0 L32I.N,  A8 A8 A9 AND,  MEMW,
  A8 A3 0 S32I.N,  MEMW,  A8 $30 L L32R,  A8 CALLX8,
  A9 128 MOVI,  A8 $1C L L32R,  MEMW,  A2 A8 0 S32I.N,  A8 $20 L L32R,  MEMW,
  A2 A8 0 S32I.N,  A8 $10 L L32R,  MEMW,  A2 A8 0 L32I.N,  A2 A2 A9 OR,  MEMW,
  A2 A8 0 S32I.N,  A8 -31 MOVI.N,  A8 A8 25 SLLI,  MEMW,  A2 A3 0 L32I.N,
  A2 A2 A8 OR,  MEMW,  A2 A3 0 S32I.N,  MEMW,  $170 L J,
  1 ZEROS,
$E8 L:
  A2 $24 L L32R,  A10 0 MOVI.N,  A8 64 MOVI.N,  MEMW,  A3 A2 0 L32I.N,
  A11 $4 L L32R,  A3 A3 A11 AND,  MEMW,  A3 A2 0 S32I.N,  A3 $28 L L32R,  MEMW,
  A10 A3 0 S32I.N,  A3 $2C L L32R,  A9 $10 L L32R,  MEMW,  A10 A3 0 S32I.N,  MEMW,
  A3 A9 0 L32I.N,  A3 A3 A8 OR,  A8 -1 MOVI.N,  A8 A8 31 SLLI,  MEMW,
  A3 A9 0 S32I.N,  MEMW,  A3 A2 0 L32I.N,  A3 A3 A8 OR,  MEMW,  A3 A2 0 S32I.N,
  A2 $14 L L32R,  MEMW,  A3 A2 0 L32I.N,  A3 A3 A11 AND,  MEMW,  A3 A2 0 S32I.N,
  A3 $1C L L32R,  MEMW,  A10 A3 0 S32I.N,  A3 $20 L L32R,  MEMW,  A10 A3 0 S32I.N,
  A10 128 MOVI,  MEMW,  A3 A9 0 L32I.N,  A3 A3 A10 OR,  MEMW,  A3 A9 0 S32I.N,
  MEMW,  A3 A2 0 L32I,
$167 L:
  A3 A3 A8 OR,  MEMW,  A3 A2 0 S32I,
$170 L:
  MEMW,  RETW.N,
$175 L:  END-CODE

CODE hal_mac_set_csi   \ .text.hal_mac_set_csi $C
  $0 3 LITERALS
    SYMBOL g_wifi_menuconfig  $3FF7309C T,  $FF7FFFFF T,
$C L:
  A1 32 ENTRY,  A10 A2 0 8 EXTUI,  A2 $0 L L32R,  A8 A2 24 L32I.N,  A2 -1 MOVI.N,
  A8 $50 L BEQZ.N,
  A8 $4 L L32R,  A10 $3C L BEQZ.N,
  A9 1 MOVI.N,  A9 A9 23 SLLI,  MEMW,  A2 A8 0 L32I.N,  A2 A2 A9 OR,  MEMW,
  A2 A8 0 S32I.N,  A2 0 MOVI.N,  MEMW,  $50 L J,
  2 ZEROS,
$3C L:
  MEMW,  A9 A8 0 L32I.N,  A2 $8 L L32R,  A9 A9 A2 AND,  A2 A10 A10 OR,  MEMW,
  A9 A8 0 S32I,
$50 L:
  MEMW,  RETW.N,
$55 L:  END-CODE

CODE hal_mac_init   \ .wifislprxiram.16 $4
  $0 1 LITERALS
    $3FF73CB8 T,
$4 L:
  A1 32 ENTRY,  A10 $0 L L32R,  A9 -3 MOVI.N,  A9 A9 11 SLLI,  A2 0 MOVI.N,  MEMW,
  A8 A10 0 L32I.N,  A8 A8 A9 AND,  MEMW,  A8 A10 0 S32I.N,  MEMW,  RETW.N,
$23 L:  END-CODE

CODE hal_mac_deinit   \ .wifi0iram.17 $30
  $0 12 LITERALS
    $3FF73CB8 T,  $17FF T,  SYMBOL rodata_wlog_error.18  $3FF73C58 T,
    $FFFCFFFF T,  SYMBOL rodata_wlog_error.19  SYMBOL rodata_wlog_error.20
    SYMBOL ets_delay_us  SYMBOL wifi_log  SYMBOL wifi_log  SYMBOL wifi_log
    SYMBOL ets_delay_us
$30 L:
  A1 32 ENTRY,  A3 $0 L L32R,  A10 32 MOVI.N,  MEMW,  A2 A3 0 L32I.N,  A4 $4 L L32R,
  A2 A2 A4 OR,  A4 0 MOVI.N,  MEMW,  A2 A3 0 S32I,  MEMW,  A8 $1C L L32R,
  A8 CALLX8,
$54 L:
  A5 1 MOVI.N,  A5 A5 13 SLLI,  MEMW,  A2 A3 0 L32I.N,  A2 A2 A5 AND,
  A2 $EC L BEQZ,
  A2 $271 MOVI,  A2 A2 5 SLLI,  A4 A2 $DE L BNE,
  A12 1 MOVI.N,  A13 $8 L L32R,  A11 A12 MOV.N,  A10 5 MOVI.N,  A8 $20 L L32R,
  A8 CALLX8,
  A5 1 MOVI.N,  A2 $C L L32R,  A5 A5 17 SLLI,  A14 A2 MOV.N,  A12 1 MOVI.N,
  A10 6 MOVI.N,  MEMW,  A8 A2 0 L32I.N,  A4 $10 L L32R,  A8 A8 A4 AND,  A8 A8 A5 OR,
  A5 1 MOVI.N,  MEMW,  A8 A2 0 S32I.N,  A5 A5 12 SLLI,  A11 A5 MOV.N,  MEMW,
  A15 A2 0 L32I.N,  A13 $14 L L32R,  MEMW,  A8 $24 L L32R,  A8 CALLX8,
  A14 A2 MOV.N,  A12 1 MOVI.N,  MEMW,  A8 A2 0 L32I.N,  A11 A5 MOV.N,  A4 A8 A4 AND,
  A10 6 MOVI.N,  MEMW,  A4 A2 0 S32I.N,  A4 0 MOVI.N,  MEMW,  A15 A2 0 L32I,
  A13 $18 L L32R,  MEMW,  A8 $28 L L32R,  A8 CALLX8,
$DE L:
  A10 2 MOVI.N,  A4 A4 1 ADDI.N,  A8 $2C L L32R,  A8 CALLX8,
  $54 L J,
  1 ZEROS,
$EC L:
  RETW.N,
$EE L:  END-CODE

CODE hal_mac_rx_enable   \ .text.hal_mac_rx_enable $4
  $0 1 LITERALS
    $3FF73084 T,
$4 L:
  A1 32 ENTRY,  A10 $0 L L32R,  A9 -1 MOVI.N,  A9 A9 31 SLLI,  MEMW,
  A8 A10 0 L32I.N,  A8 A8 A9 OR,  MEMW,  A8 A10 0 S32I.N,  MEMW,  RETW.N,
$21 L:  END-CODE

CODE hal_mac_rx_disable   \ .text.hal_mac_rx_disable $8
  $0 2 LITERALS
    $3FF73084 T,  $7FFFFFFF T,
$8 L:
  A1 32 ENTRY,  A9 $0 L L32R,  A10 $4 L L32R,  MEMW,  A8 A9 0 L32I.N,
  A8 A8 A10 AND,  MEMW,  A8 A9 0 S32I.N,  MEMW,  RETW.N,
$23 L:  END-CODE

CODE hal_rx_disable_bssid_check   \ .text.hal_rx_disable_bssid_check $4
  $0 1 LITERALS
    $FFDCC36 T,
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A2 0 8 EXTUI,  A2 A2 A8 ADD.N,  A2 A2 2 SLLI,
  A9 -3 MOVI.N,  MEMW,  A8 A2 0 L32I.N,  A8 A8 A9 AND,  MEMW,  A8 A2 0 S32I.N,
  MEMW,  RETW.N,
$26 L:  END-CODE

CODE hal_rx_enable_bssid_check   \ .text.hal_rx_enable_bssid_check $4
  $0 1 LITERALS
    $FFDCC36 T,
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A2 0 8 EXTUI,  A2 A2 A8 ADD.N,  A2 A2 2 SLLI,
  A9 2 MOVI.N,  MEMW,  A8 A2 0 L32I.N,  A8 A8 A9 OR,  MEMW,  A8 A2 0 S32I.N,  MEMW,
  RETW.N,
$26 L:  END-CODE

LINK-SYMBOLS   \ the cells of other packages that wait for these names
END-PACKAGE  PREVIOUS   \ not in the search order: ALSO name gives its words (README.md)
