\ pp_debug.fs - package PP-PP-DEBUG: pp_debug.o of libpp.a (ESP32)
\
\ Made from pp_debug.S by s2fs.py, not by hand. Each function of C is a
\ CODE word: its bytes are those of the compiler, at the same offsets of
\ its section (L gives the address of an offset); its literals are at the
\ start of the word, after a jump. The data come first, then the
\ functions, each after those that it calls. The global symbols are
\ PUBLIC, the static ones PRIVATE. lib/esp32/clink.fs explains the words.
\
\ 19 functions (19 global), 40 data objects.
\ A function runs as C code with the windowed calls of the Xtensa: call
\ it with CALL-C ( x1 .. xn n xt -- x ), never by its name alone.
\
\ Names of other packages: a literal waits until its package comes
\ (LINK-SYMBOLS); a name outside the libraries until EXTERN gives it.
\ Uses:
\   PP-ESF-BUF  esf_buf_statis_dump
\ Outside the libraries (../README.md):
\   __divsf3 __extendsfdf2 __floatundisf g_mesh_init_ps_type g_mesh_is_root
\   g_mesh_is_started g_osi_funcs_p g_wifi_mac_time_delta memset wifi_log
\ C names with more than 31 characters (Forth takes 31): the first 26,
\ ~ and 4 hex digits of their CRC-32 (../README.md):
\   rodata.dbg_lmac_diag_stati~D233 = rodata.dbg_lmac_diag_statis_dump.str1.1
\   rodata.dbg_lmac_hw_statis_~B823 = rodata.dbg_lmac_hw_statis_dump.str1.1
\   rodata.dbg_lmac_ps_statis_~48E0 = rodata.dbg_lmac_ps_statis_dump.str1.1
\   rodata.dbg_lmac_rxtx_stati~4649 = rodata.dbg_lmac_rxtx_statis_dump.str1.1
\
\ Needs XTENSA (asm/xtensa.fs), ASM (lib/esp32/asm.fs) and CLINK
\ (lib/esp32/clink.fs) of ucf; the ESP32.

PACKAGE PP-PP-DEBUG  ALSO ASM  ALSO CLINK   \ lib/package.fs
EXPORTS   \ the PUBLIC words are symbols for the other packages

\ ==== Data =============================================================

PRIVATE
CREATE rodata.dbg_lmac_rxtx_stati~4649   \ .rodata.dbg_lmac_rxtx_statis_dump.str1.1, 76 bytes
  ," lrc"  0 C,
  ," tx_all"  0 C,
  ," to"  0 C,
  ," age"  0 C,
  ," src"  0 C,
  ," mdpu"  0 C,
  ," ampdu"  0 C,
  ," frag"  0 C,
  ," ctrl"  0 C,
  ," mgmt"  0 C,
  ," rx_end"  0 C,
  ," rx_suc"  0 C,
  ," full#2"  0 C,
  ," full#1"  0 C,
CREATE rodata.dbg_lmac_hw_statis_~B823   \ .rodata.dbg_lmac_hw_statis_dump.str1.1, 184 bytes
  ," rxcckerr"  0 C,
  ," rxbufbk"  0 C,
  ," fifofull"  0 C,
  ," rx_fcs"  0 C,
  ," rx_abort"  0 C,
  ," rxsf"  0 C,
  ," rx_agc"  0 C,
  ," ofdmerr"  0 C,
  ," rx_oth"  0 C,
  ," afull"  0 C,
  ," rifs_int"  0 C,
  ," cts_int"  0 C,
  ," rts_int"  0 C,
  ," ack_int"  0 C,
  ," samebm"  0 C,
  ," tkiperr"  0 C,
  ," hoperr"  0 C,
  ," blockerr"  0 C,
  ," panic"  0 C,
  ," txhung"  0 C,
  ," trigger"  0 C,
  ," trcts"  0 C,
  ," track"  0 C,
  ," txcts"  0 C,
  ," txrts"  0 C,
CREATE rodata.dbg_lmac_diag_stati~D233   \ .rodata.dbg_lmac_diag_statis_dump.str1.1, 82 bytes
  ," diag1"  0 C,
  ," diag0"  0 C,
  ," diag6"  0 C,
  ," diag5"  0 C,
  ," diag4"  0 C,
  ," diag3"  0 C,
  ," diag2"  0 C,
  ," diag8"  0 C,
  ," diag7"  0 C,
  ," diagsel"  0 C,
  ," diag12"  0 C,
  ," diag10"  0 C,
  ," diag9"  0 C,
CREATE rodata.dbg_lmac_ps_statis_~48E0   \ .rodata.dbg_lmac_ps_statis_dump.str1.1, 106 bytes
  ," beacon"  0 C,
  ," tbtt"  0 C,
  ," percent"  0 C,
  ," totalt"  0 C,
  ," waket"  0 C,
  ," sleept"  0 C,
  ," wake"  0 C,
  ," sleep"  0 C,
  ," bcndly"  0 C,
  ," null0"  0 C,
  ," null1"  0 C,
  ," txwake"  0 C,
  ," actto"  0 C,
  ," tbttdrm"  0 C,
  ," dtimset"  0 C,
  ," timset"  0 C,
CREATE rodata_wlog_info.35   \ .rodata_wlog_info.35, 3 bytes
  $25 C,  $64 C,  $00 C,
4 DATA: bss.s_t_old   \ .bss.s_t_old
4 DATA: bss.s_total   \ .bss.s_total
CREATE rodata_wlog_info.34   \ .rodata_wlog_info.34, 85 bytes
  ,"  %-8u %-8u %-8u %-8u %-8u %-8u %-8u %-8u %-8u %-8u %-8u %-8u"
  ,"  %-8u %-8u %-8u %-6.2f%%"  0 C,
CREATE rodata_wlog_info.33   \ .rodata_wlog_info.33, 81 bytes
  ,"  %-8s %-8s %-8s %-8s %-8s %-8s %-8s %-8s %-8s %-8s %-8s %-8s"
  ,"  %-8s %-8s %-8s %-8s"  0 C,
CREATE rodata_wlog_info.32   \ .rodata_wlog_info.32, 13 bytes
  ,"  Power save:"  0 C,
CREATE rodata_wlog_info.31   \ .rodata_wlog_info.31, 1 bytes
  $00 C,
CREATE rodata_wlog_info.30   \ .rodata_wlog_info.30, 31 bytes
  ,"  %-8x %-8x %-8x %-8x %-8x %-8x"  0 C,
CREATE rodata_wlog_info.29   \ .rodata_wlog_info.29, 31 bytes
  ,"  %-8s %-8s %-8s %-8s %-8s %-8s"  0 C,
CREATE rodata_wlog_info.28   \ .rodata_wlog_info.28, 36 bytes
  ,"  %-8x %-8x %-8x %-8x %-8x %-8x %-8x"  0 C,
CREATE rodata_wlog_info.27   \ .rodata_wlog_info.27, 36 bytes
  ,"  %-8s %-8s %-8s %-8s %-8s %-8s %-8s"  0 C,
CREATE rodata_wlog_info.26   \ .rodata_wlog_info.26, 7 bytes
  ,"  DIAG:"  0 C,
CREATE rodata_wlog_info.25   \ .rodata_wlog_info.25, 1 bytes
  $00 C,
CREATE rodata_wlog_info.24   \ .rodata_wlog_info.24, 46 bytes
  ,"  %-8x %-8x %-8x %-8x %-8x %-8x %-8x %-8x %-8x"  0 C,
CREATE rodata_wlog_info.23   \ .rodata_wlog_info.23, 46 bytes
  ,"  %-8s %-8s %-8s %-8s %-8s %-8s %-8s %-8s %-8s"  0 C,
CREATE rodata_wlog_info.22   \ .rodata_wlog_info.22, 41 bytes
  ,"  %-8x %-8x %-8x %-8x %-8x %-8x %-8x %-8x"  0 C,
CREATE rodata_wlog_info.21   \ .rodata_wlog_info.21, 41 bytes
  ,"  %-8s %-8s %-8s %-8s %-8s %-8s %-8s %-8s"  0 C,
CREATE rodata_wlog_info.20   \ .rodata_wlog_info.20, 41 bytes
  ,"  %-8x %-8x %-8x %-8x %-8x %-8x %-8x %-8x"  0 C,
CREATE rodata_wlog_info.19   \ .rodata_wlog_info.19, 41 bytes
  ,"  %-8s %-8s %-8s %-8s %-8s %-8s %-8s %-8s"  0 C,
CREATE rodata_wlog_info.18   \ .rodata_wlog_info.18, 11 bytes
  ,"  Hardware:"  0 C,
CREATE rodata_wlog_info.17   \ .rodata_wlog_info.17, 1 bytes
  $00 C,
CREATE rodata_wlog_info.16   \ .rodata_wlog_info.16, 21 bytes
  ,"  %-8x %-8x %-8x %-8x"  0 C,
CREATE rodata_wlog_info.15   \ .rodata_wlog_info.15, 21 bytes
  ,"  %-8s %-8s %-8s %-8s"  0 C,
CREATE rodata_wlog_info.14   \ .rodata_wlog_info.14, 8 bytes
  ,"  HW RX:"  0 C,
CREATE rodata_wlog_info.13   \ .rodata_wlog_info.13, 1 bytes
  $00 C,
CREATE rodata_wlog_info.12   \ .rodata_wlog_info.12, 26 bytes
  ,"  %-8u %-8u %-8u %-8u %-4u"  0 C,
CREATE rodata_wlog_info.11   \ .rodata_wlog_info.11, 26 bytes
  ,"  %-8s %-8s %-8s %-8s %-4s"  0 C,
CREATE rodata_wlog_info.10   \ .rodata_wlog_info.10, 10 bytes
  ,"  LMAC RX:"  0 C,
CREATE rodata_wlog_info.9   \ .rodata_wlog_info.9, 1 bytes
  $00 C,
CREATE rodata_wlog_info.8   \ .rodata_wlog_info.8, 26 bytes
  ,"  %-8u %-3u %-3u %-3u %-3u"  0 C,
CREATE rodata_wlog_info.7   \ .rodata_wlog_info.7, 26 bytes
  ,"  %-8s %-3s %-3s %-3s %-3s"  0 C,
CREATE rodata_wlog_info.6   \ .rodata_wlog_info.6, 10 bytes
  ,"  LMAC TX:"  0 C,
CREATE rodata_wlog_info.5   \ .rodata_wlog_info.5, 1 bytes
  $00 C,
PUBLIC
8 DATA-ALIGN
72 DATA: g_pm_cnt   \ .bss.g_pm_cnt
180 DATA: g_lmac_cnt   \ .bss.g_lmac_cnt
PRIVATE
4 DATA: dram1.2   \ .dram1.2

\ ==== Code =============================================================

PUBLIC
CODE wifi_gpio_debug   \ .iram1.3 $4
  $0 1 LITERALS
    SYMBOL dram1.2
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A10 A2 MOV.N,  A11 A3 MOV.N,  A8 A8 0 L32I.N,
  A8 $15 L BEQZ.N,
  A8 CALLX8,
$15 L:
  RETW.N,
$17 L:  END-CODE

CODE wifi_set_gpio_debug_cb   \ .iram1.4 $4
  $0 1 LITERALS
    SYMBOL dram1.2
$4 L:
  A1 32 ENTRY,  A2 $10 L BEQZ,
  A8 $0 L L32R,  A2 A8 0 S32I,
$10 L:
  MEMW,  RETW.N,
$15 L:  END-CODE

CODE wifi_gpio_debug_max_event_get   \ .text.wifi_gpio_debug_max_event_get $0
  $0 ORIGIN
  A1 32 ENTRY,  A2 18 MOVI.N,  RETW.N,
$7 L:  END-CODE

CODE dbg_lmac_rxtx_statis_dump   \ .text.dbg_lmac_rxtx_statis_dump $AC
  $0 43 LITERALS
    SYMBOL rodata_wlog_info.5  SYMBOL rodata_wlog_info.6
    SYMBOL rodata.dbg_lmac_rxtx_stati~4649
    $4 SYMBOL+ rodata.dbg_lmac_rxtx_stati~4649  SYMBOL rodata_wlog_info.7
    $B SYMBOL+ rodata.dbg_lmac_rxtx_stati~4649
    $E SYMBOL+ rodata.dbg_lmac_rxtx_stati~4649
    $12 SYMBOL+ rodata.dbg_lmac_rxtx_stati~4649  SYMBOL g_lmac_cnt
    SYMBOL rodata_wlog_info.8  SYMBOL rodata_wlog_info.9
    SYMBOL rodata_wlog_info.10  $16 SYMBOL+ rodata.dbg_lmac_rxtx_stati~4649
    $1B SYMBOL+ rodata.dbg_lmac_rxtx_stati~4649  SYMBOL rodata_wlog_info.11
    $21 SYMBOL+ rodata.dbg_lmac_rxtx_stati~4649
    $26 SYMBOL+ rodata.dbg_lmac_rxtx_stati~4649
    $2B SYMBOL+ rodata.dbg_lmac_rxtx_stati~4649  SYMBOL rodata_wlog_info.12
    SYMBOL rodata_wlog_info.13  SYMBOL rodata_wlog_info.14
    $30 SYMBOL+ rodata.dbg_lmac_rxtx_stati~4649
    $37 SYMBOL+ rodata.dbg_lmac_rxtx_stati~4649  SYMBOL rodata_wlog_info.15
    $3E SYMBOL+ rodata.dbg_lmac_rxtx_stati~4649
    $45 SYMBOL+ rodata.dbg_lmac_rxtx_stati~4649  $3FF732CC T,  $3FF732D0 T,
    $3FF732AC T,  $3FF732DC T,  SYMBOL rodata_wlog_info.16  SYMBOL wifi_log
    SYMBOL wifi_log  SYMBOL wifi_log  SYMBOL wifi_log  SYMBOL wifi_log
    SYMBOL wifi_log  SYMBOL wifi_log  SYMBOL wifi_log  SYMBOL wifi_log
    SYMBOL wifi_log  SYMBOL wifi_log  SYMBOL wifi_log
$AC L:
  A1 48 ENTRY,  A2 1 MOVI.N,  A2 A2 19 SLLI,  A13 $0 L L32R,  A12 3 MOVI.N,
  A11 A2 A2 OR,  A10 6 MOVI,  A8 $7C L L32R,  A8 CALLX8,
  A13 $4 L L32R,  A12 3 MOVI.N,  A11 A2 MOV.N,  A10 6 MOVI.N,  A8 $80 L L32R,
  A8 CALLX8,
  A3 $14 L L32R,  A12 3 MOVI.N,  A11 A2 MOV.N,  A10 6 MOVI.N,  A5 0 MOVI.N,
  A3 A1 8 S32I.N,  A3 $18 L L32R,  A3 A1 4 S32I.N,  A3 $1C L L32R,  A15 $8 L L32R,
  A14 $C L L32R,  A13 $10 L L32R,  A3 A1 0 S32I.N,  MEMW,  A8 $84 L L32R,
  A8 CALLX8,
  A3 $20 L L32R,  A4 A3 MOV.N,
$102 L:
  A2 1 MOVI.N,  A2 A2 19 SLLI,  A8 A3 127 L8UI,  A12 3 MOVI.N,  A11 A2 MOV.N,
  A10 6 MOVI.N,  A5 A5 1 ADDI.N,  A15 A3 124 L8UI,  A3 A3 8 ADDI.N,  A8 A1 8 S32I,
  A8 A3 118 L8UI,  A8 A1 4 S32I,  A8 A3 117 L8UI,  A8 A1 0 S32I.N,  A14 A3 112 L32I,
  A13 $24 L L32R,  MEMW,  A8 $88 L L32R,  A8 CALLX8,
  A5 5 $102 L BNEI,
  A11 A2 MOV.N,  A13 $28 L L32R,  A12 3 MOVI.N,  A10 6 MOVI.N,  A8 $8C L L32R,
  A8 CALLX8,
  A11 A2 MOV.N,  A13 $2C L L32R,  A12 3 MOVI.N,  A10 6 MOVI,  A8 $90 L L32R,
  A8 CALLX8,
  A11 A2 MOV.N,  A3 $3C L L32R,  A12 3 MOVI.N,  A10 6 MOVI.N,  A3 A1 8 S32I.N,
  A3 $40 L L32R,  A3 A1 4 S32I.N,  A3 $44 L L32R,  A15 $30 L L32R,  A14 $34 L L32R,
  A13 $38 L L32R,  A3 A1 0 S32I.N,  MEMW,  A8 $94 L L32R,  A8 CALLX8,
  A11 A2 MOV.N,  A12 3 MOVI.N,  A10 6 MOVI.N,  A3 A4 176 L16UI,  A3 A1 8 S32I.N,
  A3 A4 172 L32I,  A3 A1 4 S32I,  A3 A4 168 L32I,  A3 A1 0 S32I,  A15 A4 164 L32I,
  A14 A4 160 L32I,  A13 $48 L L32R,  MEMW,  A8 $98 L L32R,  A8 CALLX8,
  A11 A2 MOV.N,  A12 3 MOVI.N,  A13 $4C L L32R,  A10 6 MOVI.N,  A8 $9C L L32R,
  A8 CALLX8,
  A11 A2 MOV.N,  A12 3 MOVI.N,  A13 $50 L L32R,  A10 6 MOVI.N,  A8 $A0 L L32R,
  A8 CALLX8,
  A11 A2 MOV.N,  A12 3 MOVI.N,  A3 $60 L L32R,  A10 6 MOVI.N,  A3 A1 4 S32I.N,
  A3 $64 L L32R,  A15 $54 L L32R,  A14 $58 L L32R,  A13 $5C L L32R,  A3 A1 0 S32I.N,
  MEMW,  A8 $A4 L L32R,  A8 CALLX8,
  A12 3 MOVI.N,  A11 A2 MOV.N,  A10 6 MOVI.N,  A3 $68 L L32R,  MEMW,
  A14 A3 0 L32I.N,  A3 $6C L L32R,  A4 $74 L L32R,  MEMW,  A15 A3 0 L32I.N,
  A3 $70 L L32R,  MEMW,  A3 A3 0 L32I.N,  MEMW,  A4 A4 0 L32I.N,  A13 $78 L L32R,
  A4 A1 4 S32I.N,  A3 A1 0 S32I.N,  MEMW,  A8 $A8 L L32R,  A8 CALLX8,
  RETW.N,
$21E L:  END-CODE

CODE dbg_lmac_hw_statis_dump   \ .text.dbg_lmac_hw_statis_dump $108
  $0 66 LITERALS
    SYMBOL rodata_wlog_info.17  SYMBOL rodata_wlog_info.18
    SYMBOL rodata.dbg_lmac_hw_statis_~B823
    $9 SYMBOL+ rodata.dbg_lmac_hw_statis_~B823  SYMBOL rodata_wlog_info.19
    $11 SYMBOL+ rodata.dbg_lmac_hw_statis_~B823
    $1A SYMBOL+ rodata.dbg_lmac_hw_statis_~B823
    $21 SYMBOL+ rodata.dbg_lmac_hw_statis_~B823
    $2A SYMBOL+ rodata.dbg_lmac_hw_statis_~B823
    $2F SYMBOL+ rodata.dbg_lmac_hw_statis_~B823
    $36 SYMBOL+ rodata.dbg_lmac_hw_statis_~B823  $3FF73094 T,  $3FF7328C T,
    $3FF73290 T,  $3FF73294 T,  $3FF73298 T,  $3FF7329C T,  $3FF732A0 T,
    $3FF732A4 T,  SYMBOL rodata_wlog_info.20
    $3E SYMBOL+ rodata.dbg_lmac_hw_statis_~B823
    $45 SYMBOL+ rodata.dbg_lmac_hw_statis_~B823  SYMBOL rodata_wlog_info.21
    $4B SYMBOL+ rodata.dbg_lmac_hw_statis_~B823
    $54 SYMBOL+ rodata.dbg_lmac_hw_statis_~B823
    $5C SYMBOL+ rodata.dbg_lmac_hw_statis_~B823
    $64 SYMBOL+ rodata.dbg_lmac_hw_statis_~B823
    $6C SYMBOL+ rodata.dbg_lmac_hw_statis_~B823
    $73 SYMBOL+ rodata.dbg_lmac_hw_statis_~B823  $3FF732A8 T,  $3FF732B0 T,
    $3FF732B4 T,  $3FF732B8 T,  $3FF732BC T,  $3FF732C0 T,  $3FF732C4 T,
    $3FF732C8 T,  SYMBOL rodata_wlog_info.22
    $7B SYMBOL+ rodata.dbg_lmac_hw_statis_~B823
    $82 SYMBOL+ rodata.dbg_lmac_hw_statis_~B823  SYMBOL rodata_wlog_info.23
    $8B SYMBOL+ rodata.dbg_lmac_hw_statis_~B823
    $91 SYMBOL+ rodata.dbg_lmac_hw_statis_~B823
    $98 SYMBOL+ rodata.dbg_lmac_hw_statis_~B823
    $A0 SYMBOL+ rodata.dbg_lmac_hw_statis_~B823
    $A6 SYMBOL+ rodata.dbg_lmac_hw_statis_~B823
    $AC SYMBOL+ rodata.dbg_lmac_hw_statis_~B823
    $B2 SYMBOL+ rodata.dbg_lmac_hw_statis_~B823  $3FF732E0 T,  $3FF732D8 T,
    $3FF73D58 T,  $3FF73D5C T,  $3FF73D60 T,  $3FF73D64 T,  $3FF73D68 T,
    $3FF73D6C T,  $3FF73D70 T,  SYMBOL rodata_wlog_info.24  SYMBOL wifi_log
    SYMBOL wifi_log  SYMBOL wifi_log  SYMBOL wifi_log  SYMBOL wifi_log
    SYMBOL wifi_log  SYMBOL wifi_log  SYMBOL wifi_log
$108 L:
  A1 64 ENTRY,  A2 1 MOVI.N,  A2 A2 19 SLLI,  A13 $0 L L32R,  A11 A2 MOV.N,
  A12 3 MOVI,  A10 6 MOVI,  A8 $E8 L L32R,  A8 CALLX8,
  A13 $4 L L32R,  A11 A2 MOV.N,  A12 3 MOVI.N,  A10 6 MOVI.N,  A8 $EC L L32R,
  A8 CALLX8,
  A3 $14 L L32R,  A11 A2 MOV.N,  A12 3 MOVI.N,  A10 6 MOVI.N,  A3 A1 20 S32I.N,
  A3 $18 L L32R,  A3 A1 16 S32I.N,  A3 $1C L L32R,  A3 A1 12 S32I.N,  A3 $20 L L32R,
  A3 A1 8 S32I,  A3 $24 L L32R,  A3 A1 4 S32I,  A3 $28 L L32R,  A15 $8 L L32R,
  A14 $C L L32R,  A13 $10 L L32R,  A3 A1 0 S32I.N,  MEMW,  A8 $F0 L L32R,
  A8 CALLX8,
  A3 $2C L L32R,  MEMW,  A14 A3 0 L32I.N,  A3 $30 L L32R,  A8 $38 L L32R,  MEMW,
  A15 A3 0 L32I.N,  A9 $3C L L32R,  A3 $34 L L32R,  A10 $40 L L32R,  A11 $44 L L32R,
  A12 $48 L L32R,  MEMW,  A3 A3 0 L32I.N,  MEMW,  A8 A8 0 L32I.N,  MEMW,
  A9 A9 0 L32I.N,  MEMW,  A10 A10 0 L32I.N,  MEMW,  A11 A11 0 L32I.N,  MEMW,
  A12 A12 0 L32I.N,  A13 $4C L L32R,  A9 A1 8 S32I.N,  A8 A1 4 S32I.N,
  A12 A1 20 S32I.N,  A12 3 MOVI.N,  A11 A1 16 S32I.N,  A11 A2 MOV.N,
  A10 A1 12 S32I.N,  A10 6 MOVI.N,  A3 A1 0 S32I.N,  MEMW,  A8 $F4 L L32R,
  A8 CALLX8,
  A11 A2 MOV.N,  A12 3 MOVI.N,  A10 6 MOVI.N,  A3 $5C L L32R,  A3 A1 20 S32I.N,
  A3 $60 L L32R,  A3 A1 16 S32I.N,  A3 $64 L L32R,  A3 A1 12 S32I.N,  A3 $68 L L32R,
  A3 A1 8 S32I.N,  A3 $6C L L32R,  A3 A1 4 S32I.N,  A3 $70 L L32R,  A15 $50 L L32R,
  A14 $54 L L32R,  A13 $58 L L32R,  A3 A1 0 S32I.N,  MEMW,  A8 $F8 L L32R,
  A8 CALLX8,
  A3 $74 L L32R,  MEMW,  A14 A3 0 L32I.N,  A3 $78 L L32R,  A8 $80 L L32R,  MEMW,
  A15 A3 0 L32I.N,  A9 $84 L L32R,  A3 $7C L L32R,  A10 $88 L L32R,  A11 $8C L L32R,
  A12 $90 L L32R,  MEMW,  A3 A3 0 L32I.N,  MEMW,  A8 A8 0 L32I.N,  MEMW,
  A9 A9 0 L32I.N,  MEMW,  A10 A10 0 L32I.N,  MEMW,  A11 A11 0 L32I.N,  MEMW,
  A12 A12 0 L32I.N,  A13 $94 L L32R,  A9 A1 8 S32I.N,  A8 A1 4 S32I.N,
  A12 A1 20 S32I.N,  A12 3 MOVI.N,  A11 A1 16 S32I.N,  A11 A2 MOV.N,
  A10 A1 12 S32I.N,  A10 6 MOVI.N,  A3 A1 0 S32I.N,  MEMW,  A8 $FC L L32R,
  A8 CALLX8,
  A11 A2 MOV.N,  A12 3 MOVI.N,  A10 6 MOVI.N,  A3 $A4 L L32R,  A3 A1 24 S32I.N,
  A3 $A8 L L32R,  A3 A1 20 S32I.N,  A3 $AC L L32R,  A3 A1 16 S32I.N,  A3 $B0 L L32R,
  A3 A1 12 S32I.N,  A3 $B4 L L32R,  A3 A1 8 S32I.N,  A3 $B8 L L32R,  A3 A1 4 S32I.N,
  A3 $BC L L32R,  A15 $98 L L32R,  A14 $9C L L32R,  A13 $A0 L L32R,  A3 A1 0 S32I.N,
  MEMW,  A8 $100 L L32R,  A8 CALLX8,
  A3 $C0 L L32R,  MEMW,  A14 A3 0 L32I.N,  A3 $C4 L L32R,  MEMW,  A15 A3 0 L32I.N,
  A3 $C8 L L32R,  MEMW,  A8 A3 0 L32I.N,  A3 $CC L L32R,  MEMW,  A9 A3 0 L32I.N,
  A3 $D0 L L32R,  MEMW,  A10 A3 0 L32I.N,  A3 $D4 L L32R,  MEMW,  A11 A3 0 L32I.N,
  A3 $D8 L L32R,  MEMW,  A12 A3 0 L32I.N,  A3 $DC L L32R,  MEMW,  A13 A3 0 L32I.N,
  A3 $E0 L L32R,  MEMW,  A3 A3 0 L32I.N,  A13 A1 20 S32I.N,  A13 $E4 L L32R,
  A12 A1 16 S32I.N,  A12 3 MOVI.N,  A11 A1 12 S32I.N,  A11 A2 MOV.N,
  A10 A1 8 S32I.N,  A10 6 MOVI.N,  A9 A1 4 S32I.N,  A8 A1 0 S32I.N,
  A3 A1 24 S32I.N,  MEMW,  A8 $104 L L32R,  A8 CALLX8,
  RETW.N,
$2FF L:  END-CODE

CODE dbg_lmac_diag_statis_dump   \ .text.dbg_lmac_diag_statis_dump $98
  $0 38 LITERALS
    SYMBOL rodata_wlog_info.25  SYMBOL rodata_wlog_info.26
    SYMBOL rodata.dbg_lmac_diag_stati~D233
    $6 SYMBOL+ rodata.dbg_lmac_diag_stati~D233  SYMBOL rodata_wlog_info.27
    $C SYMBOL+ rodata.dbg_lmac_diag_stati~D233
    $12 SYMBOL+ rodata.dbg_lmac_diag_stati~D233
    $18 SYMBOL+ rodata.dbg_lmac_diag_stati~D233
    $1E SYMBOL+ rodata.dbg_lmac_diag_stati~D233
    $24 SYMBOL+ rodata.dbg_lmac_diag_stati~D233  $3FF73DAC T,  $3FF73DB0 T,
    $3FF73DB4 T,  $3FF73DB8 T,  $3FF732F0 T,  $3FF732F4 T,  $3FF732F8 T,
    SYMBOL rodata_wlog_info.28  $2A SYMBOL+ rodata.dbg_lmac_diag_stati~D233
    $30 SYMBOL+ rodata.dbg_lmac_diag_stati~D233  SYMBOL rodata_wlog_info.29
    $36 SYMBOL+ rodata.dbg_lmac_diag_stati~D233
    $3E SYMBOL+ rodata.dbg_lmac_diag_stati~D233
    $45 SYMBOL+ rodata.dbg_lmac_diag_stati~D233
    $4C SYMBOL+ rodata.dbg_lmac_diag_stati~D233  $3FF732FC T,  $3FF73424 T,
    $3FF73428 T,  $3FF7342C T,  $3FF73430 T,  $3FF73DBC T,
    SYMBOL rodata_wlog_info.30  SYMBOL wifi_log  SYMBOL wifi_log  SYMBOL wifi_log
    SYMBOL wifi_log  SYMBOL wifi_log  SYMBOL wifi_log
$98 L:
  A1 64 ENTRY,  A2 1 MOVI.N,  A2 A2 19 SLLI,  A13 $0 L L32R,  A11 A2 MOV.N,
  A12 3 MOVI,  A10 6 MOVI,  A8 $80 L L32R,  A8 CALLX8,
  A13 $4 L L32R,  A11 A2 MOV.N,  A12 3 MOVI.N,  A10 6 MOVI.N,  A8 $84 L L32R,
  A8 CALLX8,
  A8 $14 L L32R,  A11 A2 MOV.N,  A12 3 MOVI.N,  A10 6 MOVI.N,  A8 A1 16 S32I.N,
  A8 $18 L L32R,  A8 A1 12 S32I.N,  A8 $1C L L32R,  A8 A1 8 S32I.N,  A8 $20 L L32R,
  A8 A1 4 S32I.N,  A8 $24 L L32R,  A15 $8 L L32R,  A14 $C L L32R,  A13 $10 L L32R,
  A8 A1 0 S32I.N,  MEMW,  A8 $88 L L32R,  A8 CALLX8,
  A8 $28 L L32R,  MEMW,  A14 A8 0 L32I.N,  A8 $2C L L32R,  A9 $34 L L32R,  MEMW,
  A15 A8 0 L32I.N,  A10 $38 L L32R,  A8 $30 L L32R,  A11 $3C L L32R,
  A12 $40 L L32R,  MEMW,  A8 A8 0 L32I.N,  MEMW,  A9 A9 0 L32I.N,  MEMW,
  A10 A10 0 L32I.N,  MEMW,  A11 A11 0 L32I.N,  MEMW,  A12 A12 0 L32I.N,
  A13 $44 L L32R,  A9 A1 4 S32I.N,  A12 A1 16 S32I.N,  A12 3 MOVI.N,
  A11 A1 12 S32I.N,  A11 A2 MOV.N,  A10 A1 8 S32I.N,  A10 6 MOVI.N,  A8 A1 0 S32I,
  MEMW,  A8 $8C L L32R,  A8 CALLX8,
  A11 A2 MOV.N,  A12 3 MOVI.N,  A10 6 MOVI.N,  A8 $54 L L32R,  A8 A1 12 S32I.N,
  A8 $58 L L32R,  A8 A1 8 S32I.N,  A8 $5C L L32R,  A8 A1 4 S32I.N,  A8 $60 L L32R,
  A15 $48 L L32R,  A14 $4C L L32R,  A13 $50 L L32R,  A8 A1 0 S32I.N,  MEMW,
  A8 $90 L L32R,  A8 CALLX8,
  A12 3 MOVI.N,  A8 $64 L L32R,  MEMW,  A14 A8 0 L32I.N,  A8 $68 L L32R,
  A9 $70 L L32R,  MEMW,  A15 A8 0 L32I.N,  A10 $74 L L32R,  A8 $6C L L32R,
  A11 $78 L L32R,  MEMW,  A8 A8 0 L32I.N,  MEMW,  A9 A9 0 L32I.N,  MEMW,
  A10 A10 0 L32I.N,  MEMW,  A11 A11 0 L32I.N,  A13 $7C L L32R,  A11 A1 12 S32I.N,
  A11 A2 MOV.N,  A10 A1 8 S32I.N,  A10 6 MOVI.N,  A9 A1 4 S32I.N,  A8 A1 0 S32I.N,
  MEMW,  A8 $94 L L32R,  A8 CALLX8,
  RETW.N,
$1BE L:  END-CODE

CODE dbg_lmac_ps_statis_dump   \ .text.dbg_lmac_ps_statis_dump $78
  $0 30 LITERALS
    SYMBOL rodata_wlog_info.31  SYMBOL rodata_wlog_info.32
    SYMBOL rodata.dbg_lmac_ps_statis_~48E0
    $7 SYMBOL+ rodata.dbg_lmac_ps_statis_~48E0  SYMBOL rodata_wlog_info.33
    $C SYMBOL+ rodata.dbg_lmac_ps_statis_~48E0
    $14 SYMBOL+ rodata.dbg_lmac_ps_statis_~48E0
    $1B SYMBOL+ rodata.dbg_lmac_ps_statis_~48E0
    $21 SYMBOL+ rodata.dbg_lmac_ps_statis_~48E0
    $28 SYMBOL+ rodata.dbg_lmac_ps_statis_~48E0
    $2D SYMBOL+ rodata.dbg_lmac_ps_statis_~48E0
    $33 SYMBOL+ rodata.dbg_lmac_ps_statis_~48E0
    $3A SYMBOL+ rodata.dbg_lmac_ps_statis_~48E0
    $40 SYMBOL+ rodata.dbg_lmac_ps_statis_~48E0
    $46 SYMBOL+ rodata.dbg_lmac_ps_statis_~48E0
    $4D SYMBOL+ rodata.dbg_lmac_ps_statis_~48E0
    $53 SYMBOL+ rodata.dbg_lmac_ps_statis_~48E0
    $5B SYMBOL+ rodata.dbg_lmac_ps_statis_~48E0
    $63 SYMBOL+ rodata.dbg_lmac_ps_statis_~48E0  SYMBOL g_pm_cnt
    SYMBOL rodata_wlog_info.34  $42C80000 T,  SYMBOL wifi_log  SYMBOL wifi_log
    SYMBOL wifi_log  SYMBOL __floatundisf  SYMBOL __floatundisf  SYMBOL __divsf3
    SYMBOL __extendsfdf2  SYMBOL wifi_log
$78 L:
  A1 112 ENTRY,  A3 1 MOVI.N,  A3 A3 19 SLLI,  A13 $0 L L32R,  A11 A3 MOV.N,
  A12 3 MOVI,  A10 6 MOVI,  A8 $58 L L32R,  A8 CALLX8,
  A13 $4 L L32R,  A11 A3 MOV.N,  A12 3 MOVI.N,  A10 6 MOVI.N,  A8 $5C L L32R,
  A8 CALLX8,
  A2 $14 L L32R,  A12 3 MOVI.N,  A11 A3 MOV.N,  A10 6 MOVI.N,  A2 A1 52 S32I.N,
  A2 $18 L L32R,  A2 A1 48 S32I.N,  A2 $1C L L32R,  A2 A1 44 S32I.N,  A2 $20 L L32R,
  A2 A1 40 S32I.N,  A2 $24 L L32R,  A2 A1 36 S32I.N,  A2 $28 L L32R,
  A2 A1 32 S32I.N,  A2 $2C L L32R,  A2 A1 28 S32I.N,  A2 $30 L L32R,
  A2 A1 24 S32I.N,  A2 $34 L L32R,  A2 A1 20 S32I.N,  A2 $38 L L32R,
  A2 A1 16 S32I.N,  A2 $3C L L32R,  A2 A1 12 S32I.N,  A2 $40 L L32R,  A2 A1 8 S32I,
  A2 $44 L L32R,  A2 A1 4 S32I,  A2 $48 L L32R,  A15 $8 L L32R,  A14 $C L L32R,
  A13 $10 L L32R,  A2 A1 0 S32I,  MEMW,  A8 $60 L L32R,  A8 CALLX8,
  A2 $4C L L32R,  A4 A2 48 L32I.N,  A10 A4 MOV.N,  A11 A2 52 L32I.N,  A8 $64 L L32R,
  A8 CALLX8,
  F0 A10 WFR,  A8 $54 L L32R,  F1 A8 WFR,  F0 F0 F1 MUL.S,  A5 A2 64 L32I,
  A10 A5 MOV.N,  A6 A2 68 L32I,  A11 A6 MOV.N,  F0 A1 64 SSI,  MEMW,  A8 $68 L L32R,
  A8 CALLX8,
  A11 A10 MOV.N,  A10 A1 64 L32I,  A8 $6C L L32R,  A8 CALLX8,
  A8 $70 L L32R,  A8 CALLX8,
  A12 3 MOVI.N,  A10 A1 56 S32I.N,  A10 6 MOVI.N,  A11 A1 60 S32I.N,  A11 A3 MOV.N,
  A5 A1 48 S32I.N,  A5 A2 56 L32I.N,  A4 A1 40 S32I.N,  A5 A1 44 S32I.N,
  A4 A2 44 L32I.N,  A4 A1 36 S32I.N,  A4 A2 40 L32I.N,  A4 A1 32 S32I.N,
  A4 A2 36 L32I.N,  A4 A1 28 S32I.N,  A4 A2 32 L32I.N,  A4 A1 24 S32I.N,
  A4 A2 28 L32I.N,  A4 A1 20 S32I.N,  A4 A2 24 L32I.N,  A4 A1 16 S32I.N,
  A4 A2 20 L32I.N,  A4 A1 12 S32I.N,  A4 A2 16 L32I.N,  A4 A1 8 S32I.N,
  A4 A2 12 L32I.N,  A4 A1 4 S32I.N,  A4 A2 8 L32I.N,  A4 A1 0 S32I.N,
  A15 A2 4 L32I.N,  A14 A2 0 L32I.N,  A13 $50 L L32R,  MEMW,  A8 $74 L L32R,
  A8 CALLX8,
  RETW.N,
$18F L:  END-CODE

CODE dbg_lmac_ps_statis_reset   \ .text.dbg_lmac_ps_statis_reset $8
  $0 2 LITERALS
    SYMBOL g_pm_cnt  SYMBOL memset
$8 L:
  A1 32 ENTRY,  A10 $0 L L32R,  A12 72 MOVI.N,  A11 0 MOVI.N,  A8 $4 L L32R,
  A8 CALLX8,
  RETW.N,
$1A L:  END-CODE

CODE dbg_lmac_statis_dump   \ .text.dbg_lmac_statis_dump $14
  $0 5 LITERALS
    SYMBOL esf_buf_statis_dump  SYMBOL dbg_lmac_rxtx_statis_dump
    SYMBOL dbg_lmac_hw_statis_dump  SYMBOL dbg_lmac_diag_statis_dump
    SYMBOL dbg_lmac_ps_statis_dump
$14 L:
  A1 32 ENTRY,  A2 0 $20 L BBCI,
  A8 $0 L L32R,  A8 CALLX8,
$20 L:
  A2 1 $29 L BBCI,
  A8 $4 L L32R,  A8 CALLX8,
$29 L:
  A2 2 $32 L BBCI,
  A8 $8 L L32R,  A8 CALLX8,
$32 L:
  A2 3 $3B L BBCI,
  A8 $C L L32R,  A8 CALLX8,
$3B L:
  A2 4 $44 L BBCI,
  A8 $10 L L32R,  A8 CALLX8,
$44 L:
  RETW.N,
$46 L:  END-CODE

CODE dbg_his_lmac_eb_show   \ .text.dbg_his_lmac_eb_show $0
  $0 ORIGIN
  A1 32 ENTRY,  RETW.N,
$5 L:  END-CODE

CODE dbg_his_lmac_event_show   \ .text.dbg_his_lmac_event_show $0
  $0 ORIGIN
  A1 32 ENTRY,  RETW.N,
$5 L:  END-CODE

CODE dbg_his_lmac_int_show   \ .text.dbg_his_lmac_int_show $0
  $0 ORIGIN
  A1 32 ENTRY,  RETW.N,
$5 L:  END-CODE

CODE dbg_his_lmac_rx_show   \ .text.dbg_his_lmac_rx_show $0
  $0 ORIGIN
  A1 32 ENTRY,  RETW.N,
$5 L:  END-CODE

CODE dbg_his_lmac_tx_show   \ .text.dbg_his_lmac_tx_show $0
  $0 ORIGIN
  A1 32 ENTRY,  RETW.N,
$5 L:  END-CODE

CODE dbg_ebuf_loc_show   \ .text.dbg_ebuf_loc_show $0
  $0 ORIGIN
  A1 32 ENTRY,  RETW.N,
$5 L:  END-CODE

CODE dbg_perf_path_show   \ .text.dbg_perf_path_show $0
  $0 ORIGIN
  A1 32 ENTRY,  RETW.N,
$5 L:  END-CODE

CODE dbg_perf_path_set   \ .text.dbg_perf_path_set $0
  $0 ORIGIN
  A1 32 ENTRY,  RETW.N,
$5 L:  END-CODE

CODE dbg_perf_throughput_cal   \ .text.dbg_perf_throughput_cal $2C
  $0 11 LITERALS
    SYMBOL g_mesh_is_started  SYMBOL g_mesh_is_root  SYMBOL g_mesh_init_ps_type
    SYMBOL g_osi_funcs_p  $3FF73C00 T,  SYMBOL g_wifi_mac_time_delta
    SYMBOL bss.s_total  SYMBOL bss.s_t_old  $7A11FF T,  SYMBOL rodata_wlog_info.35
    SYMBOL wifi_log
$2C L:
  A1 32 ENTRY,  A3 $0 L L32R,  A3 A3 0 L8UI,  A3 $5C L BEQZ.N,
  A3 $4 L L32R,  MEMW,  A3 A3 0 L8UI,  A3 A3 0 8 EXTUI,  A3 $5C L BNEZ.N,
  A3 $8 L L32R,  A3 A3 0 L32I.N,  A3 $5C L BEQZ.N,
  A3 $C L L32R,  A3 A3 0 L32I,  A10 A3 $110 L32I,  A10 CALLX8,
  $6B L J,
  1 ZEROS,
$5C L:
  A3 $10 L L32R,  MEMW,  A10 A3 0 L32I.N,  A3 $14 L L32R,  A3 A3 0 L32I.N,
  A10 A10 A3 ADD.N,
$6B L:
  A3 $18 L L32R,  A8 A3 0 L32I.N,  A14 A2 A8 ADD.N,  A8 $1C L L32R,  A2 A8 0 L32I.N,
  A2 A10 A2 SUB,  A9 $20 L L32R,  A14 A3 0 S32I.N,  A9 A2 $9C L BGEU,
  A12 3 MOVI.N,  A11 1 MOVI.N,  A2 0 MOVI.N,  A13 $24 L L32R,  A10 A8 0 S32I,
  A10 6 MOVI,  MEMW,  A8 $28 L L32R,  A8 CALLX8,
  A2 A3 0 S32I.N,
$9C L:
  MEMW,  RETW.N,
$A1 L:  END-CODE

CODE dbg_lmac_init   \ .text.dbg_lmac_init $0
  $0 ORIGIN
  A1 32 ENTRY,  RETW.N,
$5 L:  END-CODE

LINK-SYMBOLS   \ the cells of other packages that wait for these names
END-PACKAGE  PREVIOUS   \ not in the search order: ALSO name gives its words (README.md)
