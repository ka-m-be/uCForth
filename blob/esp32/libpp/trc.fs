\ trc.fs - package PP-TRC: trc.o of libpp.a (ESP32)
\
\ Made from trc.S by s2fs.py, not by hand. Each function of C is a
\ CODE word: its bytes are those of the compiler, at the same offsets of
\ its section (L gives the address of an offset); its literals are at the
\ start of the word, after a jump. The data come first, then the
\ functions, each after those that it calls. The global symbols are
\ PUBLIC, the static ones PRIVATE. lib/esp32/clink.fs explains the words.
\
\ 73 functions (69 global; 4 static without symbol, named after their
\ section), 44 data objects.
\ A function runs as C code with the windowed calls of the Xtensa: call
\ it with CALL-C ( x1 .. xn n xt -- x ), never by its name alone.
\
\ Names of other packages: a literal waits until its package comes
\ (LINK-SYMBOLS); a name outside the libraries until EXTERN gives it.
\ Uses:
\   PP-LMAC  lmacConfMib
\   PP-WDEV  wDevCtrl
\ Outside the libraries (../README.md):
\   g_ic g_mesh_init_ps_type g_mesh_is_root g_mesh_is_started g_osi_funcs_p
\   g_wifi_mac_time_delta g_wifi_nvs memcmp memcpy memset wifi_log
\   wl_is_ap_no_lr
\ C names with more than 31 characters (Forth takes 31): the first 26,
\ ~ and 4 hex digits of their CRC-32 (../README.md):
\   rodata.TRC_AMPDU_PER_DOWN_~934C = rodata.TRC_AMPDU_PER_DOWN_THRESHOLD
\   rodata.TRC_AMPDU_PER_UP_TH~9E84 = rodata.TRC_AMPDU_PER_UP_THRESHOLD
\
\ Needs XTENSA (asm/xtensa.fs), ASM (lib/esp32/asm.fs) and CLINK
\ (lib/esp32/clink.fs) of ucf; the ESP32.

PACKAGE PP-TRC  ALSO ASM  ALSO CLINK   \ lib/package.fs
EXPORTS   \ the PUBLIC words are symbols for the other packages

\ ==== Data =============================================================

PRIVATE
CREATE rodata.rc11NRate2SchedIdx   \ .rodata.rc11NRate2SchedIdx, 28 bytes
  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,   \ 7 jumps into rc11NRate2SchedIdx
CREATE rodata.rcGetAmpduSched.str1.1   \ .rodata.rcGetAmpduSched.str1.1, 6 bytes
  ," trc.c"  0 C,
CREATE rodata.rssi_margin   \ .rodata.rssi_margin, 92 bytes
  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,
  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,   \ 23 jumps into rssi_margin
CREATE rodata.TRC_AMPDU_PER_DOWN_~934C   \ .rodata.TRC_AMPDU_PER_DOWN_THRESHOLD, 52 bytes
  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,   \ 13 jumps into TRC_AMPDU_PER_DOWN_THRESHOLD
CREATE rodata.TRC_AMPDU_PER_UP_TH~9E84   \ .rodata.TRC_AMPDU_PER_UP_THRESHOLD, 52 bytes
  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,   \ 13 jumps into TRC_AMPDU_PER_UP_THRESHOLD
CREATE rodata.trc_NeedRTS   \ .rodata.trc_NeedRTS, 24 bytes
  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,   \ 6 jumps into trc_NeedRTS
CREATE rodata.CSWTCH$98   \ .rodata.CSWTCH$98, 32 bytes
  $5A C,  $19 C,  $80 C,  $25 C,  $00 C,  $4B C,  $00 C,  $64 C,  $C0 C,  $A8 C,
  $50 C,  $C3 C,  $00 C,  $E1 C,  $FF C,  $FF C,  $5A C,  $19 C,  $80 C,  $25 C,
  $00 C,  $4B C,  $00 C,  $64 C,  $C0 C,  $A8 C,  $50 C,  $C3 C,  $00 C,  $E1 C,
  $FF C,  $FF C,
CREATE rodata.CSWTCH$90   \ .rodata.CSWTCH$90, 43 bytes
  $03 C,  $02 C,  $01 C,  $00 C,  $FF C,  $02 C,  $01 C,  $00 C,  $FF C,  $FF C,
  $FF C,  $FF C,  $FF C,  $FF C,  $FF C,  $FF C,  $FF C,  $FF C,  $FF C,  $FF C,
  $FF C,  $FF C,  $FF C,  $FF C,  $FF C,  $FF C,  $FF C,  $FF C,  $FF C,  $FF C,
  $FF C,  $FF C,  $FF C,  $FF C,  $FF C,  $FF C,  $FF C,  $FF C,  $FF C,  $FF C,
  $FF C,  $05 C,  $04 C,
CREATE rodata.CSWTCH$88   \ .rodata.CSWTCH$88, 43 bytes
  $0A C,  $09 C,  $08 C,  $FF C,  $FF C,  $09 C,  $08 C,  $FF C,  $01 C,  $03 C,
  $05 C,  $07 C,  $00 C,  $02 C,  $04 C,  $06 C,  $FF C,  $FF C,  $FF C,  $FF C,
  $FF C,  $FF C,  $FF C,  $FF C,  $FF C,  $FF C,  $FF C,  $FF C,  $FF C,  $FF C,
  $FF C,  $FF C,  $FF C,  $FF C,  $FF C,  $FF C,  $FF C,  $FF C,  $FF C,  $FF C,
  $FF C,  $0C C,  $0B C,
CREATE rodata.__FUNCTION__$0   \ .rodata.__FUNCTION__$0, 18 bytes
  ," rcReachRetryLimit"  0 C,
CREATE rodata.__FUNCTION__$1   \ .rodata.__FUNCTION__$1, 10 bytes
  ," rcGetRate"  0 C,
CREATE rodata.__FUNCTION__$2   \ .rodata.__FUNCTION__$2, 16 bytes
  ," rcGetAmpduSched"  0 C,
CREATE rodata_wlog_error.27   \ .rodata_wlog_error.27, 32 bytes
  ," update default trc: invalid ifx"  0 C,
CREATE rodata_wlog_warning.26   \ .rodata_wlog_warning.26, 19 bytes
  ," invalid interface!"  0 C,
CREATE rodata_wlog_error.25   \ .rodata_wlog_error.25, 22 bytes
  ," trc init fail, no mem"  0 C,
CREATE rodata_wlog_warning.23   \ .rodata_wlog_warning.23, 7 bytes
  ," rm mis"  0 C,
CREATE rodata_wlog_warning.22   \ .rodata_wlog_warning.22, 14 bytes
  ," add mismatch "  0 C,
CREATE rodata_wlog_error.21   \ .rodata_wlog_error.21, 27 bytes
  ," rc_enable_trc fail, no mem"  0 C,
CREATE rodata_wlog_error.17   \ .rodata_wlog_error.17, 10 bytes
  ," %s %s %u"  10 C,  0 C,
CREATE rodata_wlog_error.16   \ .rodata_wlog_error.16, 37 bytes
  ," sched is null, if=%d tid=%d flags=%x"  0 C,
CREATE rodata_wlog_error.15   \ .rodata_wlog_error.15, 10 bytes
  ," %s %s %u"  10 C,  0 C,
CREATE rodata_wlog_error.14   \ .rodata_wlog_error.14, 13 bytes
  ," desc is null"  0 C,
CREATE rodata_wlog_error.13   \ .rodata_wlog_error.13, 10 bytes
  ," %s %s %u"  10 C,  0 C,
CREATE rodata_wlog_error.11   \ .rodata_wlog_error.11, 10 bytes
  ," %s %s %u"  10 C,  0 C,
CREATE rodata_wlog_warning.6   \ .rodata_wlog_warning.6, 46 bytes
  ," rcUpdatePhyMode: lowestRateIdx:%d, rateIdx:%d"  0 C,
CREATE rodata_wlog_warning.5   \ .rodata_wlog_warning.5, 50 bytes
  ," rcUpdatePhyMode: phytype:%d, rs_nrate:%d, rssi:%d"  0 C,
CREATE rodata_wlog_error.4   \ .rodata_wlog_error.4, 44 bytes
  ," rcGet11BHighestRateIdx: invalid max rate=%d"  0 C,
CREATE rodata_wlog_error.3   \ .rodata_wlog_error.3, 44 bytes
  ," rcGet11GHighestRateIdx: invalid max rate=%d"  0 C,
CREATE rodata_wlog_error.2   \ .rodata_wlog_error.2, 44 bytes
  ," rcGet11NHighestRateIdx: invalid max rate=%d"  0 C,
PUBLIC
72 DATA: rc11BSchedTbl   \ .data.rc11BSchedTbl
  $07  $02  $06  $02  $06  $03  $06  $19  $20  $40  $00  $00  $06  $02  $06  $02  16 rc11BSchedTbl INIT
  $06  $03  $06  $19  $20  $39  $00  $00  $05  $02  $00  $02  $00  $03  $00  $19  16 rc11BSchedTbl $10 + INIT
  $20  $39  $00  $00  $00  $02  $00  $02  $00  $03  $00  $19  $20  $1E  $00  $00  16 rc11BSchedTbl $20 + INIT
  $2A  $08  $2A  $08  $2A  $02  $29  $0E  $20  $14  $00  $00  $29  $02  $29  $02  16 rc11BSchedTbl $30 + INIT
  $29  $03  $29  $19  $20  $00  $00  $00  8 rc11BSchedTbl $40 + INIT
96 DATA: rcP2P11GSchedTbl   \ .data.rcP2P11GSchedTbl
  $0C  $02  $08  $02  $0D  $03  $0B  $19  $20  $72  $00  $00  $08  $02  $0D  $02  16 rcP2P11GSchedTbl INIT
  $09  $03  $0B  $19  $20  $65  $00  $00  $0D  $02  $09  $02  $0E  $03  $0B  $19  16 rcP2P11GSchedTbl $10 + INIT
  $20  $5C  $00  $00  $09  $02  $0E  $02  $0A  $03  $0B  $19  $20  $58  $00  $00  16 rcP2P11GSchedTbl $20 + INIT
  $0E  $02  $0A  $02  $0F  $03  $0B  $19  $20  $52  $00  $00  $0A  $02  $0F  $02  16 rcP2P11GSchedTbl $30 + INIT
  $0B  $03  $0B  $19  $20  $4B  $00  $00  $0F  $02  $0B  $02  $0B  $03  $0B  $19  16 rcP2P11GSchedTbl $40 + INIT
  $20  $38  $00  $00  $0B  $02  $0B  $02  $0B  $03  $0B  $19  $20  $40  $00  $00  16 rcP2P11GSchedTbl $50 + INIT
156 DATA: rc11GSchedTbl   \ .data.rc11GSchedTbl
  $0C  $02  $08  $02  $0B  $03  $06  $19  $20  $72  $00  $00  $08  $02  $0D  $02  16 rc11GSchedTbl INIT
  $0B  $03  $06  $19  $20  $65  $00  $00  $0D  $02  $09  $02  $0B  $03  $06  $19  16 rc11GSchedTbl $10 + INIT
  $20  $5C  $00  $00  $09  $02  $0E  $02  $0B  $03  $06  $19  $20  $58  $00  $00  16 rc11GSchedTbl $20 + INIT
  $0E  $02  $0A  $02  $0B  $03  $06  $19  $20  $52  $00  $00  $0A  $02  $0B  $02  16 rc11GSchedTbl $30 + INIT
  $06  $03  $06  $19  $20  $4B  $00  $00  $0F  $02  $0B  $02  $06  $03  $06  $19  16 rc11GSchedTbl $40 + INIT
  $20  $38  $00  $00  $0B  $02  $06  $02  $06  $03  $06  $19  $20  $40  $00  $00  16 rc11GSchedTbl $50 + INIT
  $06  $02  $06  $02  $06  $03  $06  $19  $20  $39  $00  $00  $05  $02  $00  $02  16 rc11GSchedTbl $60 + INIT
  $00  $03  $00  $19  $20  $39  $00  $00  $00  $02  $00  $02  $00  $03  $00  $19  16 rc11GSchedTbl $70 + INIT
  $20  $1E  $00  $00  $2A  $08  $2A  $08  $2A  $02  $29  $0E  $20  $14  $00  $00  16 rc11GSchedTbl $80 + INIT
  $29  $02  $29  $02  $29  $03  $29  $19  $20  $00  $00  $00  12 rc11GSchedTbl $90 + INIT
120 DATA: rcP2P11NSchedTbl   \ .data.rcP2P11NSchedTbl
  $1F  $02  $16  $02  $15  $03  $0B  $19  $20  $72  $00  $00  $17  $02  $15  $02  16 rcP2P11NSchedTbl INIT
  $14  $03  $0B  $19  $20  $72  $00  $00  $16  $02  $14  $02  $13  $03  $0B  $19  16 rcP2P11NSchedTbl $10 + INIT
  $20  $72  $00  $00  $15  $02  $13  $02  $12  $03  $0B  $19  $20  $60  $00  $00  16 rcP2P11NSchedTbl $20 + INIT
  $14  $02  $12  $02  $11  $03  $0B  $19  $20  $55  $00  $00  $13  $02  $12  $02  16 rcP2P11NSchedTbl $30 + INIT
  $11  $03  $0B  $19  $20  $4B  $00  $00  $12  $02  $11  $02  $10  $03  $0B  $19  16 rcP2P11NSchedTbl $40 + INIT
  $20  $40  $00  $00  $11  $02  $10  $02  $0B  $03  $0B  $19  $20  $3B  $00  $00  16 rcP2P11NSchedTbl $50 + INIT
  $10  $02  $0B  $02  $0B  $03  $0B  $19  $20  $32  $00  $00  $0B  $02  $0B  $02  16 rcP2P11NSchedTbl $60 + INIT
  $0B  $03  $0B  $19  $20  $40  $00  $00  8 rcP2P11NSchedTbl $70 + INIT
168 DATA: rc11NSchedTbl   \ .data.rc11NSchedTbl
  $1F  $02  $16  $02  $0B  $03  $06  $19  $20  $72  $00  $00  $17  $02  $15  $02  16 rc11NSchedTbl INIT
  $0B  $03  $06  $19  $20  $72  $00  $00  $16  $02  $14  $02  $0B  $03  $06  $19  16 rc11NSchedTbl $10 + INIT
  $20  $72  $00  $00  $15  $02  $13  $02  $0B  $03  $06  $19  $20  $60  $00  $00  16 rc11NSchedTbl $20 + INIT
  $14  $02  $12  $02  $0B  $03  $06  $19  $20  $55  $00  $00  $13  $02  $12  $02  16 rc11NSchedTbl $30 + INIT
  $0B  $03  $06  $19  $20  $4B  $00  $00  $12  $02  $11  $02  $0B  $03  $06  $19  16 rc11NSchedTbl $40 + INIT
  $20  $40  $00  $00  $11  $02  $0B  $02  $06  $03  $06  $19  $20  $3B  $00  $00  16 rc11NSchedTbl $50 + INIT
  $10  $02  $0B  $02  $06  $03  $06  $19  $20  $32  $00  $00  $06  $02  $06  $02  16 rc11NSchedTbl $60 + INIT
  $06  $03  $06  $19  $20  $39  $00  $00  $05  $02  $00  $02  $00  $03  $00  $19  16 rc11NSchedTbl $70 + INIT
  $20  $39  $00  $00  $00  $02  $00  $02  $00  $03  $00  $19  $20  $1E  $00  $00  16 rc11NSchedTbl $80 + INIT
  $2A  $08  $2A  $08  $2A  $02  $29  $0E  $20  $14  $00  $00  $29  $02  $29  $02  16 rc11NSchedTbl $90 + INIT
  $29  $03  $29  $19  $20  $00  $00  $00  8 rc11NSchedTbl $A0 + INIT
24 DATA: rcLoRaSchedTbl   \ .data.rcLoRaSchedTbl
  $2A  $08  $2A  $08  $2A  $02  $29  $0E  $20  $14  $00  $00  $29  $02  $29  $02  16 rcLoRaSchedTbl INIT
  $29  $03  $29  $19  $20  $00  $00  $00  8 rcLoRaSchedTbl $10 + INIT
12 DATA: BAROFDMSched   \ .data.BAROFDMSched
  $0B  $02  $0B  $02  $0B  $03  $0B  $19  $20  $00  $00  $00  12 BAROFDMSched INIT
12 DATA: BasicOFDMSched   \ .data.BasicOFDMSched
  $0B  $02  $0B  $02  $0B  $03  $00  $19  $20  $00  $00  $00  12 BasicOFDMSched INIT
28 DATA: trc_ctl   \ .data.trc_ctl
  $00  $00  $00  $00  $00  $00  $00  $00  $05  $05  $00  $00  12 trc_ctl $10 + INIT
12 DATA: s_fix_rate   \ .bss.s_fix_rate
4 DATA: s_fix_rate_mask   \ .bss.s_fix_rate_mask
PRIVATE
36 DATA: bss.g_80211_tx_rate_config   \ .bss.g_80211_tx_rate_config
12 DATA: bss.g_80211_tx_rate   \ .bss.g_80211_tx_rate
12 DATA: bss.g_espnow_rate   \ .bss.g_espnow_rate
88 DATA: bss.g_per_conn_trc   \ .bss.g_per_conn_trc

\ ==== Code =============================================================

CODE rc11NRate2SchedIdx   \ .text.rc11NRate2SchedIdx $4, static, without symbol
  $0 1 LITERALS
    SYMBOL rodata.rc11NRate2SchedIdx
$4 L:
  A1 32 ENTRY,  A9 A2 0 8 EXTUI,  A9 7 $1D L BGEUI,
  A2 $0 L L32R,  A2 A9 A2 ADDX4,  A2 A2 0 L32I.N,  A2 JX,
$18 L:
  A2 9 MOVI.N,  $48 L J,
$1D L:
  A8 41 MOVI.N,  A2 13 MOVI.N,  A9 A8 $48 L BEQ,
  A8 42 MOVI.N,  A2 12 MOVI.N,  A9 A8 $48 L BEQ,
  A8 31 MOVI.N,  A2 0 MOVI.N,  A9 A8 $48 L BEQ,
  $3B L J,
  1 ZEROS,
$36 L:
  A2 11 MOVI.N,  $48 L J,
$3B L:
  A8 24 MOVI.N,  A2 A8 A9 SUB,  A2 A2 0 8 EXTUI,  $48 L J,
$46 L:
  A2 10 MOVI.N,
$48 L:
  RETW.N,
  rodata.rc11NRate2SchedIdx  $36 L!+  $46 L!+  $18 L!+  $3B L!+  $3B L!+  $46 L!+
  $18 L!+  DROP   \ the table of jumps
$4A L:  END-CODE

CODE rc11GRate2SchedIdx   \ .text.rc11GRate2SchedIdx $4, static, without symbol
  $0 1 LITERALS
    SYMBOL rodata.CSWTCH$88
$4 L:
  A1 32 ENTRY,  A9 A2 0 8 EXTUI,  A8 42 MOVI.N,  A2 255 MOVI,  A8 A9 $1A L BLTU,
  A8 $0 L L32R,  A8 A8 A9 ADD.N,  A2 A8 0 L8UI,
$1A L:
  RETW.N,
$1C L:  END-CODE

CODE rc11BRate2SchedIdx   \ .text.rc11BRate2SchedIdx $4, static, without symbol
  $0 1 LITERALS
    SYMBOL rodata.CSWTCH$90
$4 L:
  A1 32 ENTRY,  A9 A2 0 8 EXTUI,  A8 42 MOVI.N,  A2 255 MOVI,  A8 A9 $1A L BLTU,
  A8 $0 L L32R,  A8 A8 A9 ADD.N,  A2 A8 0 L8UI,
$1A L:
  RETW.N,
$1C L:  END-CODE

PUBLIC
CODE rcLoRaRate2SchedIdx   \ .text.rcLoRaRate2SchedIdx $0
  $0 ORIGIN
  A1 32 ENTRY,  A8 A2 0 8 EXTUI,  A9 41 MOVI.N,  A2 1 MOVI.N,  A8 A9 $1C L BEQ,
  A8 A8 -42 ADDI,  A9 0 MOVI,  A2 A9 A8 MOVEQZ,  A2 A2 NEG,  A2 A2 0 8 EXTUI,
$1C L:
  RETW.N,
$1E L:  END-CODE

PRIVATE
CODE rcBuildIndex   \ .text.rcBuildIndex $0, static, without symbol
  $0 ORIGIN
  A1 32 ENTRY,  A3 A3 -1 ADDI.N,  A3 A3 0 8 EXTUI,  A8 0 MOVI.N,  A3 A3 1 ADDI.N,
$C L:
  A8 A2 10 S8I,  MEMW,  A8 A8 1 ADDI.N,  A8 A8 0 8 EXTUI,  A2 A2 12 ADDI.N,
  A3 A3 -1 ADDI.N,  A3 $C L BNEZ,
  MEMW,  RETW.N,
$23 L:  END-CODE

PUBLIC
CODE trc_calc_duration   \ .text.trc_calc_duration $0
  $0 ORIGIN
  A1 32 ENTRY,  A8 A2 A3 SUB,  A2 A3 $C L BGEU,
  A8 A8 -1 ADDI,
$C L:
  A2 A8 MOV.N,  RETW.N,
$10 L:  END-CODE

CODE rx11NRate2AMPDULimit   \ .text.rx11NRate2AMPDULimit $4
  $0 1 LITERALS
    SYMBOL rodata.CSWTCH$98
$4 L:
  A1 32 ENTRY,  A2 A2 -16 ADDI,  A8 A2 0 8 EXTUI,  A9 15 MOVI.N,  A2 0 MOVI.N,
  A9 A8 $1D L BLTU,
  A2 $0 L L32R,  A8 A8 A2 ADDX2,  A2 A8 0 L16UI,
$1D L:
  RETW.N,
$1F L:  END-CODE

CODE trcAmpduSetState   \ .text.trcAmpduSetState $0
  $0 ORIGIN
  A1 32 ENTRY,  A10 A2 16 L8UI,  A3 A3 0 8 EXTUI,  A8 0 MOVI.N,  A9 8 MOVI,
  A9 $1E L LOOP,
  A10 A8 $1C L BBC,
  A11 A2 A8 ADD.N,  A3 A11 17 S8I,  MEMW,
$1C L:
  A8 A8 1 ADDI.N,
$1E L:
  MEMW,  RETW.N,
$23 L:  END-CODE

CODE rcSetTxAmpduLimit   \ .text.rcSetTxAmpduLimit $0
  $0 ORIGIN
  A1 32 ENTRY,  A8 A2 120 L16UI,  A3 A3 0 16 EXTUI,  A9 A8 A3 MINU,  A3 A3 1 SRLI,
  A3 A3 A8 MINU,  A9 A2 124 S16I,  A3 A2 122 S16I,  MEMW,  MEMW,  RETW.N,
$20 L:  END-CODE

CODE rcUpdateAMPDUParam   \ .text.rcUpdateAMPDUParam $10
  $0 4 LITERALS
    $1FFF T,  $3FFF T,  $7FFF T,  $FFFF T,
$10 L:
  A1 32 ENTRY,  A3 A3 0 8 EXTUI,  A3 A2 130 S8I,  MEMW,  A4 A4 0 8 EXTUI,
  A5 A5 0 8 EXTUI,  A3 4 $30 L BGEUI,
  A3 2 $A8 L BLTUI,
  MEMW,  $3C L J,
  2 ZEROS,
$30 L:
  A3 A3 -4 ADDI,  A3 A3 0 8 EXTUI,  A3 2 $78 L BLTUI,
  $A8 L J,
$3C L:
  A3 $8 L L32R,  A4 2 $51 L BEQI,
  A3 $C L L32R,  A4 3 $51 L BEQI,
  A3 $0 L L32R,  A4 1 $51 L BNEI,
  A3 $4 L L32R,
$51 L:
  A3 A2 120 S16I,  MEMW,  A3 40 MOVI.N,  A5 5 $A2 L BEQI,
  A5 6 $68 L BLTUI,
  A5 6 $6D L BEQI,
  MEMW,  $72 L J,
$68 L:
  A3 20 MOVI.N,  $A2 L J,
$6D L:
  A3 76 MOVI.N,  $A2 L J,
$72 L:
  A3 148 MOVI,  $A2 L J,
$78 L:
  A3 $8 L L32R,  A4 2 $8D L BEQI,
  A3 $C L L32R,  A4 3 $8D L BEQI,
  A3 $0 L L32R,  A4 1 $8D L BNEI,
  A3 $4 L L32R,
$8D L:
  A3 A2 120 S16I,  MEMW,  A5 5 $6D L BEQI,
  A5 6 $68 L BLTUI,
  A3 152 MOVI,  A5 6 $A2 L BEQI,
  A3 $12C MOVI,
$A2 L:
  A3 A2 126 S16I,  MEMW,
$A8 L:
  MEMW,  RETW.N,
$AD L:  END-CODE

CODE rcGet11NHighestRateIdx   \ .text.rcGet11NHighestRateIdx $8
  $0 2 LITERALS
    SYMBOL rodata_wlog_error.2  SYMBOL wifi_log
$8 L:
  A1 32 ENTRY,  A14 A2 0 8 EXTUI,  A8 78 MOVI.N,  A2 4 MOVI.N,  A14 A8 $6E L BEQ,
  A8 A14 $38 L BLTU,
  A8 39 MOVI.N,  A2 6 MOVI.N,  A14 A8 $6E L BEQ,
  A8 A14 $30 L BLTU,
  A8 13 MOVI.N,  A2 8 MOVI.N,  A14 A8 $6E L BEQ,
  A8 26 MOVI.N,  A2 7 MOVI.N,  $59 L J,
$30 L:
  A8 52 MOVI.N,  A2 5 MOVI.N,  $59 L J,
  1 ZEROS,
$38 L:
  A8 130 MOVI,  A2 1 MOVI.N,  A14 A8 $6E L BEQ,
  A8 A14 $54 L BLTU,
  A8 104 MOVI,  A2 3 MOVI.N,  A14 A8 $6E L BEQ,
  A8 117 MOVI,  A2 2 MOVI.N,  $59 L J,
  1 ZEROS,
$54 L:
  A8 144 MOVI,  A2 0 MOVI.N,
$59 L:
  A14 A8 $6E L BEQ,
  A13 $0 L L32R,  A12 1 MOVI.N,  A11 $200 MOVI,  A10 6 MOVI.N,  A8 $4 L L32R,
  A8 CALLX8,
  A2 0 MOVI.N,
$6E L:
  RETW.N,
$70 L:  END-CODE

CODE rcGet11GHighestRateIdx   \ .text.rcGet11GHighestRateIdx $8
  $0 2 LITERALS
    SYMBOL rodata_wlog_error.3  SYMBOL wifi_log
$8 L:
  A1 32 ENTRY,  A14 A2 0 8 EXTUI,  A8 48 MOVI.N,  A2 3 MOVI.N,  A14 A8 $5E L BEQ,
  A8 A14 $35 L BLTU,
  A8 24 MOVI.N,  A2 5 MOVI.N,  A14 A8 $5E L BEQ,
  A8 A14 $2E L BLTU,
  A2 7 MOVI.N,  A14 12 $5E L BEQI,
  A8 18 MOVI.N,  A2 6 MOVI.N,  $49 L J,
$2E L:
  A8 36 MOVI.N,  A2 4 MOVI.N,  $49 L J,
$35 L:
  A8 96 MOVI,  A2 1 MOVI.N,  A14 A8 $5E L BEQ,
  A8 108 MOVI,  A2 0 MOVI.N,  A14 A8 $5E L BEQ,
  A8 72 MOVI.N,  A2 2 MOVI.N,
$49 L:
  A14 A8 $5E L BEQ,
  A13 $0 L L32R,  A12 1 MOVI.N,  A11 $200 MOVI,  A10 6 MOVI.N,  A8 $4 L L32R,
  A8 CALLX8,
  A2 0 MOVI.N,
$5E L:
  RETW.N,
$60 L:  END-CODE

CODE rcGet11BHighestRateIdx   \ .text.rcGet11BHighestRateIdx $8
  $0 2 LITERALS
    SYMBOL rodata_wlog_error.4  SYMBOL wifi_log
$8 L:
  A1 32 ENTRY,  A14 A2 0 8 EXTUI,  A8 11 MOVI.N,  A2 1 MOVI.N,  A14 A8 $3E L BEQ,
  A8 A14 $25 L BLTU,
  A2 3 MOVI.N,  A14 2 $3E L BEQI,
  A2 2 MOVI.N,  A14 4 $3E L BEQI,
  $2C L J,
$25 L:
  A8 22 MOVI.N,  A2 0 MOVI.N,  A14 A8 $3E L BEQ,
$2C L:
  A13 $0 L L32R,  A12 1 MOVI.N,  A11 $200 MOVI,  A10 6 MOVI.N,  A8 $4 L L32R,
  A8 CALLX8,
  A2 0 MOVI.N,
$3E L:
  RETW.N,
$40 L:  END-CODE

CODE rcGetDefaultHigestRateIdx   \ .text.rcGetDefaultHigestRateIdx $0
  $0 ORIGIN
  A1 32 ENTRY,  A8 A2 130 L8UI,  A2 253 MOVI,  A8 A8 -2 ADDI,  A8 A8 A2 AND,
  A9 0 MOVI.N,  A2 1 MOVI.N,  A2 A9 A8 MOVNEZ,  RETW.N,
$18 L:  END-CODE

CODE rcGetHighestRateIdx   \ .text.rcGetHighestRateIdx $10
  $0 4 LITERALS
    SYMBOL rcGet11NHighestRateIdx  SYMBOL rcGet11GHighestRateIdx
    SYMBOL rcGet11BHighestRateIdx  SYMBOL rcGetDefaultHigestRateIdx
$10 L:
  A1 32 ENTRY,  A4 A4 0 8 EXTUI,  A10 A3 0 8 EXTUI,  A4 $20 L BNEZ.N,
  $4D L J,
  2 ZEROS,
$20 L:
  A8 A2 130 L8UI,  A8 1 $3B L BEQI,
  A8 2 $44 L BLTUI,
  A8 A8 -2 ADDI,  A8 A8 0 8 EXTUI,  A8 4 $4D L BGEUI,
  A8 $0 L L32R,  A8 CALLX8,
  $55 L J,
$3B L:
  A8 $4 L L32R,  A8 CALLX8,
  $55 L J,
$44 L:
  A8 $8 L L32R,  A8 CALLX8,
  $55 L J,
$4D L:
  A10 A2 MOV.N,  A8 $C L L32R,  A8 CALLX8,
$55 L:
  A2 A10 MOV.N,  RETW.N,
$59 L:  END-CODE

CODE rcClearCurStat   \ .text.rcClearCurStat $0
  $0 ORIGIN
  A1 32 ENTRY,  A8 0 MOVI.N,  A8 A2 44 S32I.N,  A8 A2 40 S32I.N,  A8 A2 60 S32I.N,
  MEMW,  RETW.N,
$10 L:  END-CODE

CODE rcClearCurSched   \ .text.rcClearCurSched $4
  $0 1 LITERALS
    $7A120 T,
$4 L:
  A1 32 ENTRY,  A8 A2 96 L32I,  A9 0 MOVI.N,  A9 A8 11 S8I,  MEMW,  A8 $0 L L32R,
  A8 A2 92 S32I,  A8 0 MOVI.N,  A8 A2 44 S32I.N,  A8 A2 40 S32I.N,  A8 A2 60 S32I.N,
  A8 A2 27 S8I,  A9 A2 28 S8I,  A9 A2 7 S8I,  MEMW,  MEMW,  RETW.N,
$31 L:  END-CODE

CODE rcClearCurAMPDUSched   \ .text.rcClearCurAMPDUSched $4
  $0 1 LITERALS
    $7A120 T,
$4 L:
  A1 32 ENTRY,  A8 A2 96 L32I,  A9 0 MOVI.N,  A9 A8 11 S8I,  MEMW,  A8 $0 L L32R,
  A8 A2 92 S32I,  A8 0 MOVI.N,  A8 A2 68 S32I,  A8 A2 64 S32I,  A8 A2 72 S32I,
  A8 A2 29 S8I,  A9 A2 30 S8I,  A9 A2 15 S8I,  MEMW,  A8 A2 88 S32I,  MEMW,  RETW.N,
$37 L:  END-CODE

CODE rcUpdatePhyMode   \ .text.rcUpdatePhyMode $6C
  $0 27 LITERALS
    SYMBOL rcP2P11NSchedTbl  SYMBOL rc11NSchedTbl  $54 SYMBOL+ rcP2P11GSchedTbl
    SYMBOL BasicOFDMSched  SYMBOL rcP2P11GSchedTbl  SYMBOL rc11GSchedTbl
    $24 SYMBOL+ rc11BSchedTbl  SYMBOL rcLoRaSchedTbl  $C SYMBOL+ rcLoRaSchedTbl
    SYMBOL g_mesh_is_started  SYMBOL rodata_wlog_warning.5  SYMBOL trc_ctl
    SYMBOL rc11NRate2SchedIdx  SYMBOL rc11GRate2SchedIdx  SYMBOL rc11BSchedTbl
    SYMBOL rc11BRate2SchedIdx  SYMBOL rcLoRaRate2SchedIdx
    SYMBOL rodata_wlog_warning.6  $7F7F7F7F T,  SYMBOL wifi_log
    SYMBOL rcGetHighestRateIdx  SYMBOL rx11NRate2AMPDULimit
    SYMBOL rcSetTxAmpduLimit  SYMBOL trcAmpduSetState  SYMBOL rcClearCurAMPDUSched
    SYMBOL wifi_log  SYMBOL rcClearCurSched
$6C L:
  A1 48 ENTRY,  A8 $24 L L32R,  A3 A3 0 8 EXTUI,  A4 A4 0 8 EXTUI,  A5 A5 0 8 EXTUI,
  A6 A6 0 8 EXTUI,  A9 A8 0 L8UI,  A7 A2 135 L8UI,  A9 $A5 L BEQZ.N,
  A4 $A5 L BEQZ.N,
  A9 A3 7 SEXT,  A15 A7 MOV.N,  A14 A2 130 L8UI,  A12 2 MOVI.N,  A11 $200 MOVI,
  A10 6 MOVI.N,  A13 $28 L L32R,  A9 A1 0 S32I.N,  MEMW,  A8 $4C L L32R,  A8 CALLX8,
$A5 L:
  A11 A5 MOV.N,  A12 A6 MOV.N,  A10 A2 MOV.N,  A8 $50 L L32R,  A8 CALLX8,
  A5 A2 130 L8UI,  A3 A2 26 S8I,  MEMW,  A5 6 $C9 L BGEUI,
  A5 2 $D4 L BGEUI,
  A5 $1AC L BNEZ,
  MEMW,  $230 L J,
$C9 L:
  A5 6 $CF L BNEI,
  $270 L J,
$CF L:
  $290 L J,
  2 ZEROS,
$D4 L:
  A5 $2C L L32R,  A9 $505 MOVI,  A9 A5 24 S16I,  MEMW,  A9 $0 L L32R,
  A4 $E8 L BNEZ.N,
  A9 $4 L L32R,
$E8 L:
  A5 9 MOVI.N,  A9 A2 112 S32I,  A4 $F8 L BNEZ,
  A5 13 MOVI.N,  A11 11 MOVI,  A5 A11 A7 MOVEQZ,
$F8 L:
  A5 A2 5 S8I,  MEMW,  A11 14 MOVI.N,  A3 A3 7 SEXT,  A5 $30 L L32R,
  A5 A2 116 S32I,  A5 10 MOVI.N,  A5 A11 A4 MOVEQZ,  A5 A2 6 S8I,  A5 11 MOVI.N,
  A10 A2 4 S8I,  MEMW,  A5 A3 $141 L BGE,
  A11 16 MOVI.N,  A5 7 MOVI.N,  A11 A3 $141 L BGE,
  A11 21 MOVI.N,  A5 5 MOVI.N,  A11 A3 $141 L BGE,
  A11 26 MOVI.N,  A5 4 MOVI.N,  A11 A3 $141 L BGE,
  A11 29 MOVI.N,  A5 3 MOVI.N,  A11 A3 $141 L BGE,
  A11 33 MOVI.N,  A5 A10 MOV.N,  A11 A3 $141 L BLT,
  A5 2 MOVI.N,
$141 L:
  A5 A5 A10 MAXU,  A5 A5 0 8 EXTUI,  A5 8 $150 L BLTUI,
  A3 7 MOVI,  A5 A3 A4 MOVNEZ,
$150 L:
  A5 A10 A6 MOVNEZ,  A3 A5 0 16 EXTUI,  A3 A3 A3 ADDX2,  A3 A3 A9 ADDX4,
  A3 A2 96 S32I,  A3 5 MOVI.N,  A3 A5 A3 MINU,  A3 A3 0 8 EXTUI,  A3 A3 A3 ADDX2,
  A3 A3 A9 ADDX4,  A10 A3 0 L8UI,  A3 4 MOVI,  A10 A2 13 S8I,  MEMW,  MEMW,
  A8 $54 L L32R,  A8 CALLX8,
  A11 A10 A10 OR,  A10 A2 A2 OR,  A8 $58 L L32R,  A8 CALLX8,
  A10 A2 MOV.N,  A11 4 MOVI.N,  A8 $5C L L32R,  A8 CALLX8,
  A10 A2 MOV.N,  A3 A2 14 S8I,  MEMW,  MEMW,  A8 $60 L L32R,  A8 CALLX8,
  $221 L J,
$1AC L:
  A9 $10 L L32R,  A4 $1B4 L BNEZ.N,
  A9 $14 L L32R,
$1B4 L:
  A5 7 MOVI.N,  A9 A2 112 S32I,  A10 A2 4 S8I,  MEMW,  A4 $1C8 L BNEZ.N,
  A5 12 MOVI.N,  A11 10 MOVI.N,  A5 A11 A7 MOVEQZ,
$1C8 L:
  A5 A2 5 S8I,  MEMW,  A11 13 MOVI.N,  A3 A3 7 SEXT,  A5 $34 L L32R,
  A5 A2 116 S32I,  A5 8 MOVI.N,  A5 A11 A4 MOVEQZ,  A5 A2 6 S8I,  MEMW,
  A5 11 MOVI.N,  A5 A3 $1F6 L BLT,
  A5 7 MOVI.N,  A3 10 MOVI.N,  A5 A3 A4 MOVEQZ,  MEMW,  $212 L J,
$1F6 L:
  A11 16 MOVI.N,  A5 5 MOVI.N,  A11 A3 $212 L BGE,
  A11 21 MOVI.N,  A5 3 MOVI.N,  A11 A3 $212 L BGE,
  A11 29 MOVI.N,  A5 1 MOVI.N,  A11 A3 $20D L BGE,
  A5 0 MOVI.N,
$20D L:
  A5 A5 A5 ADD.N,  A5 A5 0 8 EXTUI,
$212 L:
  A5 A10 A6 MOVNEZ,  A3 A5 0 16 EXTUI,  A3 A3 A3 ADDX2,  A3 A3 A9 ADDX4,
  A3 A2 96 S32I,
$221 L:
  A3 $8 L L32R,  A4 $2C0 L BNEZ,
  A3 $C L L32R,  MEMW,  $2C0 L J,
$230 L:
  A11 $38 L L32R,  A3 $18 L L32R,  A9 A3 A3 OR,  A11 A2 112 S32I,  A6 $248 L BEQZ,
  A9 A10 0 16 EXTUI,  A9 A9 A9 ADDX2,  A9 A9 A11 ADDX4,
$248 L:
  A9 A2 96 S32I,  A8 5 MOVI.N,  A9 3 MOVI.N,  A8 A9 A7 MOVEQZ,  A8 A2 5 S8I,  MEMW,
  A8 $3C L L32R,  A8 A2 116 S32I,  A8 6 MOVI.N,  A10 A2 4 S8I,  A8 A2 6 S8I,  MEMW,
  MEMW,  $2C0 L J,
  1 ZEROS,
$270 L:
  A5 $40 L L32R,  A3 $1C L L32R,  A5 A2 116 S32I,  A5 $100 MOVI,  A5 A2 4 S16I,
  MEMW,  A5 2 MOVI.N,  A3 A2 112 S32I,  A3 A2 96 S32I,  MEMW,  $2B7 L J,
$290 L:
  A5 0 MOVI.N,  A3 $38 L L32R,  A8 3 MOVI.N,  A5 A2 4 S8I,  A5 5 MOVI,
  A5 A8 A7 MOVEQZ,  A3 A2 112 S32I,  A5 A2 5 S8I,  MEMW,  A3 $18 L L32R,
  A5 $3C L L32R,  A3 A2 96 S32I,  A5 A2 116 S32I,  A5 6 MOVI.N,
$2B7 L:
  A5 A2 6 S8I,  MEMW,  A5 0 MOVI,
$2C0 L:
  A6 $24 L L32R,  A3 A2 100 S32I,  A3 A6 0 L8UI,  A3 $2E5 L BEQZ.N,
  A4 $2E5 L BEQZ.N,
  A15 A5 MOV.N,  A14 A2 5 L8UI,  A12 2 MOVI.N,  A11 $200 MOVI,  A10 6 MOVI.N,
  A13 $44 L L32R,  MEMW,  A8 $64 L L32R,  A8 CALLX8,
$2E5 L:
  A10 A2 MOV.N,  A8 $68 L L32R,  A8 CALLX8,
  A3 $48 L L32R,  A3 A2 0 S32I,  A3 $20 L L32R,  A7 $2FC L BNEZ,
  A3 A2 100 L32I,
$2FC L:
  A3 A2 104 S32I,  MEMW,  RETW.N,
$304 L:  END-CODE

CODE rcLowerSched   \ .text.rcLowerSched $1C
  $0 7 LITERALS
    SYMBOL g_mesh_is_started  SYMBOL g_mesh_is_root  SYMBOL g_mesh_init_ps_type
    SYMBOL g_osi_funcs_p  $3FF73C00 T,  SYMBOL g_wifi_mac_time_delta
    SYMBOL rcClearCurSched
$1C L:
  A1 32 ENTRY,  A10 A2 A2 OR,  A8 $18 L L32R,  A8 CALLX8,
  A10 A2 96 L32I,  A3 A3 0 8 EXTUI,  A9 A10 10 L8UI,  A9 A9 A3 ADD.N,  A8 A2 5 L8UI,
  A8 A9 $45 L BLT,
  A3 A3 A3 ADDX2,  A8 A3 A10 ADDX4,  $4E L J,
  3 ZEROS,
$45 L:
  A3 A2 112 L32I,  A8 A8 A8 ADDX2,  A8 A8 A3 ADDX4,
$4E L:
  A3 $0 L L32R,  A3 A3 0 L8UI,  A8 A2 96 S32I,  A3 $80 L BEQZ.N,
  A3 $4 L L32R,  MEMW,  A3 A3 0 L8UI,  A3 A3 0 8 EXTUI,  A3 $80 L BNEZ.N,
  A3 $8 L L32R,  A3 A3 0 L32I.N,  A3 $80 L BEQZ.N,
  A3 $C L L32R,  A3 A3 0 L32I.N,  A10 A3 $110 L32I,  MEMW,  A10 CALLX8,
  $8F L J,
  1 ZEROS,
$80 L:
  A3 $10 L L32R,  MEMW,  A10 A3 0 L32I.N,  A3 $14 L L32R,  A3 A3 0 L32I.N,
  A10 A10 A3 ADD.N,
$8F L:
  A10 A2 48 S32I.N,  MEMW,  RETW.N,
$96 L:  END-CODE

CODE rcUpSched   \ .text.rcUpSched $1C
  $0 7 LITERALS
    SYMBOL g_mesh_is_started  SYMBOL g_mesh_is_root  SYMBOL g_mesh_init_ps_type
    SYMBOL g_osi_funcs_p  $3FF73C00 T,  SYMBOL g_wifi_mac_time_delta
    SYMBOL rcClearCurSched
$1C L:
  A1 32 ENTRY,  A10 A2 A2 OR,  A8 $18 L L32R,  A8 CALLX8,
  A8 A2 96 L32I,  A10 A2 4 L8UI,  A9 A8 10 L8UI,  A10 A9 $89 L BGEU,
  A8 A8 -12 ADDI,  A10 1 MOVI.N,  A8 A2 96 S32I,  A9 A8 11 L8UI,  A9 A9 A10 OR,
  A9 A8 11 S8I,  MEMW,  A8 $0 L L32R,  A8 A8 0 L8UI,  A8 $78 L BEQZ.N,
  A8 $4 L L32R,  MEMW,  A8 A8 0 L8UI,  A8 A8 0 8 EXTUI,  A8 $78 L BNEZ.N,
  A8 $8 L L32R,  A8 A8 0 L32I.N,  A8 $78 L BEQZ.N,
  A8 $C L L32R,  A8 A8 0 L32I,  A10 A8 $110 L32I,  MEMW,  A10 CALLX8,
  $87 L J,
  1 ZEROS,
$78 L:
  A8 $10 L L32R,  MEMW,  A10 A8 0 L32I.N,  A8 $14 L L32R,  A8 A8 0 L32I.N,
  A10 A10 A8 ADD.N,
$87 L:
  A10 A2 52 S32I.N,
$89 L:
  MEMW,  RETW.N,
$8E L:  END-CODE

CODE rcUpdateAckSnr   \ .text.rcUpdateAckSnr $0
  $0 ORIGIN
  A1 32 ENTRY,  A11 A3 0 8 EXTUI,  A10 127 MOVI,  A3 A3 7 SEXT,  A3 A10 $56 L BEQ,
  A8 A2 0 L8UI,  A9 0 MOVI.N,  A8 A8 7 SEXT,  A8 A10 $20 L BEQ,
  A8 A8 A3 ADD,  A9 A8 1 8 EXTUI,
$20 L:
  A8 A2 1 L8UI,  A3 127 MOVI,  A8 A8 7 SEXT,  A11 A2 0 S8I,  MEMW,  A8 A3 $50 L BEQ,
  A9 A9 7 SEXT,  A8 A8 A8 ADDX2,  A8 A8 A9 ADD.N,  A9 A8 3 ADDI.N,  A9 A8 A8 MOVGEZ,
  A8 A9 2 SRAI,  A8 A2 1 S8I,  MEMW,  MEMW,  $56 L J,
  2 ZEROS,
$50 L:
  A9 A2 1 S8I,  MEMW,
$56 L:
  MEMW,  RETW.N,
$5B L:  END-CODE

CODE rcTxUpdatePer   \ .text.rcTxUpdatePer $8
  $0 2 LITERALS
    $1FFFFFF T,  SYMBOL rcLowerSched
$8 L:
  A1 32 ENTRY,  A8 A2 96 L32I,  A3 A3 0 8 EXTUI,  A10 A2 MOV.N,  A9 A8 1 L8UI,
  A8 A9 2 ADDI,  A8 A8 0 8 EXTUI,  A9 A3 $24 L BLTU,
  A8 A3 1 ADDI.N,  A8 A8 0 8 EXTUI,
$24 L:
  A9 A10 44 L32I.N,  A11 A9 1 ADDI.N,  A12 $0 L L32R,  A12 A11 $3A L BGEU,
  A9 A9 1 SRLI,  A11 A10 40 L32I.N,  A11 A11 1 SRLI,  A11 A10 40 S32I.N,
  A9 A10 44 S32I.N,
$3A L:
  A9 A10 44 L32I.N,  A9 A9 1 ADDI.N,  A9 A10 44 S32I.N,  A9 A10 40 L32I.N,
  A8 A9 A8 ADD.N,  A8 A10 40 S32I.N,  A3 8 $54 L BLTUI,
  A3 A10 7 L8UI,  A3 A3 2 ADDI.N,  MEMW,  $5C L J,
$54 L:
  A3 5 $68 L BLTUI,
  A3 A10 7 L8UI,  A3 A3 1 ADDI.N,
$5C L:
  A3 A10 7 S8I,  MEMW,  MEMW,  $79 L J,
$68 L:
  A3 3 $79 L BGEUI,
  A3 0 MOVI.N,  A3 A10 7 S8I,  MEMW,  MEMW,  $88 L J,
$79 L:
  A3 A10 7 L8UI,  A3 7 $88 L BLTUI,
  A11 1 MOVI,  A8 $4 L L32R,  A8 CALLX8,
$88 L:
  RETW.N,
$8A L:  END-CODE

CODE rcUpdateTxDone   \ .wifi0iram.7 $C
  $0 3 LITERALS
    SYMBOL g_mesh_is_started  SYMBOL rcUpdateAckSnr  SYMBOL rcTxUpdatePer
$C L:
  A1 32 ENTRY,  A2 $75 L BEQZ,
  A8 A2 11 L8UI,  A9 A8 7 SEXT,  A9 $75 L BLTZ,
  A10 A2 96 L32I,  A9 A3 28 L32I.N,  A9 A10 $75 L BNE,
  A10 4 MOVI.N,  A8 A10 $75 L BANY,
  A8 A2 60 L32I.N,  A8 A8 1 ADDI.N,  A8 A2 60 S32I.N,  A8 A3 19 L8UI,
  A8 1 $54 L BNEI,
  A8 A2 25 L8UI,  A8 A10 $4C L BANY,
  A10 A2 A2 OR,  A11 A3 13 L8UI,  A11 A11 7 SEXT,  MEMW,  A8 $4 L L32R,  A8 CALLX8,
$4C L:
  A11 A3 5 L8UI,  $6D L J,
  2 ZEROS,
$54 L:
  A8 A8 -2 ADDI,  A8 A8 0 8 EXTUI,  A8 2 $75 L BGEUI,
  A3 $0 L L32R,  A3 A3 0 L8UI,  A11 A9 8 L8UI,  A3 $6D L BEQZ.N,
  A3 11 MOVI.N,  A11 A11 A3 MINU,
$6D L:
  A10 A2 MOV.N,  A8 $8 L L32R,  A8 CALLX8,
$75 L:
  RETW.N,
$77 L:  END-CODE

CODE rcUpdateRxDone   \ .wifirxiram.8 $4
  $0 1 LITERALS
    SYMBOL wDevCtrl
$4 L:
  A1 32 ENTRY,  A8 A2 11 L8UI,  A8 7 $72 L BBSI,
  A9 A2 25 L8UI,  A9 A9 0 1 EXTUI,  A9 $72 L BNEZ,
  A12 127 MOVI,  A10 $0 L L32R,  A8 A3 0 L8UI,  A10 A10 46 L8UI,  A8 A8 A10 ADD.N,
  A8 A8 0 8 EXTUI,  A11 A8 MOV.N,  A10 A2 2 L8UI,  A10 A10 7 SEXT,
  A10 A12 $3C L BEQ,
  A9 A8 7 SEXT,  A10 A10 A9 ADD.N,  A9 A10 1 8 EXTUI,  A11 A9 MOV.N,
$3C L:
  A8 A2 2 S8I,  MEMW,  A10 127 MOVI,  A8 A2 3 L8UI,  A8 A8 7 SEXT,
  A8 A10 $6C L BEQ,
  A9 A9 7 SEXT,  A8 A8 A8 ADDX2,  A8 A8 A9 ADD.N,  A9 A8 3 ADDI.N,  A9 A8 A8 MOVGEZ,
  A8 A9 2 SRAI,  A8 A2 3 S8I,  MEMW,  MEMW,  $72 L J,
  2 ZEROS,
$6C L:
  A11 A2 3 S8I,  MEMW,
$72 L:
  MEMW,  RETW.N,
$77 L:  END-CODE

CODE trc_onAmpduOp   \ .text.trc_onAmpduOp $4
  $0 1 LITERALS
    SYMBOL trcAmpduSetState
$4 L:
  A1 32 ENTRY,  A8 A2 130 L8UI,  A4 A4 0 8 EXTUI,  A8 2 $13 L BGEUI,
  $E5 L J,
$13 L:
  A3 2 $19 L BEQI,
  $AA L J,
$19 L:
  A9 -128 MOVI,  A8 A2 25 L8UI,  A10 253 MOVI,  A8 A8 A9 OR,  A8 A2 25 S8I,  MEMW,
  A8 1 MOVI,  A4 SSL,  A8 A8 SLL,  A9 A2 16 L8UI,  A9 A8 A9 OR,  A9 A2 16 S8I,
  MEMW,  A9 A2 14 L8UI,  A9 A9 -2 ADDI,  A9 A10 $60 L BANY,
  A9 A2 A4 ADD.N,  A3 A9 17 S8I,  MEMW,  A9 A2 38 L8UI,  A8 A8 A9 OR,  A8 A2 38 S8I,
  A3 A2 14 S8I,  MEMW,
$60 L:
  A8 16 MOVI.N,  A3 A2 26 L8UI,  A8 A3 $9C L BLTU,
  A8 109 MOVI,  A3 A2 1 L8UI,  A3 A3 -17 ADDI,  A3 A3 0 8 EXTUI,  A8 A3 $9C L BGEU,
  A3 3 MOVI.N,  A11 3 MOVI,  A10 A2 A2 OR,  MEMW,  A8 $0 L L32R,  A8 CALLX8,
  A3 A2 14 S8I,  A3 33 MOVI.N,  A3 A2 26 S8I,  MEMW,  A3 3 MOVI.N,  MEMW,  $AA L J,
  1 ZEROS,
$9C L:
  A3 33 MOVI.N,  A3 A2 26 S8I,  MEMW,  MEMW,  $E5 L J,
$AA L:
  A8 -3 MOVI.N,  A8 A3 A8 AND,  A8 1 $E5 L BNEI,
  A3 1 $C9 L BNEI,
  A8 -1 MOVI.N,  A4 SSL,  A3 A3 SLL,  A3 A8 A3 XOR,  A8 A2 16 L8UI,  A3 A3 A8 AND,
  A3 A2 16 S8I,
$C9 L:
  A4 A2 A4 ADD.N,  A8 1 MOVI.N,  A8 A4 17 S8I,  MEMW,  A3 A2 14 L8UI,
  A3 A3 -3 ADDI,  A3 A3 0 8 EXTUI,  A3 2 $E5 L BGEUI,
  A8 A2 14 S8I,  MEMW,
$E5 L:
  MEMW,  RETW.N,
$EA L:  END-CODE

CODE trc_set_per_pkt_rate   \ .text.trc_set_per_pkt_rate $4
  $0 1 LITERALS
    $24 SYMBOL+ rc11BSchedTbl
$4 L:
  A1 32 ENTRY,  A8 A2 44 L32I.N,  A10 1 MOVI.N,  A10 A10 25 SLLI,  A9 A8 0 L32I.N,
  A9 A9 A10 OR,  A9 A8 0 S32I.N,  A9 $0 L L32R,  A9 A8 28 S32I.N,  MEMW,  RETW.N,
$1F L:  END-CODE

CODE rcSetBarRate   \ .text.rcSetBarRate $8
  $0 2 LITERALS
    SYMBOL BAROFDMSched  $24 SYMBOL+ rc11BSchedTbl
$8 L:
  A1 32 ENTRY,  A3 A3 0 8 EXTUI,  A8 $0 L L32R,  A3 $16 L BNEZ.N,
  A8 $4 L L32R,
$16 L:
  A8 A2 28 S32I.N,  A8 A8 0 L8UI,  A8 A2 12 S8I,  MEMW,  MEMW,  RETW.N,
$26 L:  END-CODE

CODE rc_get_default_sched   \ .text.rc_get_default_sched $4
  $0 1 LITERALS
    $24 SYMBOL+ rc11BSchedTbl
$4 L:
  A1 32 ENTRY,  A2 $0 L L32R,  RETW.N,
$C L:  END-CODE

CODE rc_get_G6M_sched   \ .text.rc_get_G6M_sched $4
  $0 1 LITERALS
    $54 SYMBOL+ rc11GSchedTbl
$4 L:
  A1 32 ENTRY,  A2 $0 L L32R,  RETW.N,
$C L:  END-CODE

CODE rcGetAmpduSched   \ .text.rcGetAmpduSched $14
  $0 5 LITERALS
    SYMBOL rodata.__FUNCTION__$2  SYMBOL rodata.rcGetAmpduSched.str1.1
    SYMBOL rodata_wlog_error.11  $8D2 T,  SYMBOL wifi_log
$14 L:
  A1 48 ENTRY,  A2 $3C L BNEZ.N,
  A2 $C L L32R,  A12 1 MOVI.N,  A11 A12 11 SLLI,  A10 6 MOVI.N,  A15 $0 L L32R,
  A14 $4 L L32R,  A13 $8 L L32R,  A2 A1 0 S32I,  MEMW,  A8 $10 L L32R,  A8 CALLX8,
$38 L:
  $38 L J,
  1 ZEROS,
$3C L:
  A4 A2 96 L32I,  A4 A3 28 S32I.N,  A4 A2 11 L8UI,  A4 2 $51 L BBCI,
  A2 A2 10 L8UI,  MEMW,  $8D L J,
  1 ZEROS,
$51 L:
  A4 A2 25 L8UI,  A4 3 $93 L BBSI,
  A4 A2 116 L32I,  A10 A2 13 L8UI,  A4 CALLX8,
  A4 A2 12 L8UI,  A4 A10 A4 ADD.N,  A10 16 MOVI,  A4 A4 0 8 EXTUI,  A8 A2 116 L32I,
  A8 CALLX8,
  A10 A4 $7E L BGEU,
  A10 16 MOVI.N,  A4 A2 116 L32I,  A4 CALLX8,
  A4 A10 MOV.N,
$7E L:
  A4 A4 0 16 EXTUI,  A2 A2 112 L32I,  A4 A4 A4 ADDX2,  A4 A4 A2 ADDX4,
  A2 A4 0 L8UI,
$8D L:
  A2 A3 12 S8I,  MEMW,
$93 L:
  MEMW,  RETW.N,
$98 L:  END-CODE

CODE rcGetRate   \ .wifiextrairam.12 $14
  $0 5 LITERALS
    SYMBOL rodata.__FUNCTION__$1  SYMBOL rodata.rcGetAmpduSched.str1.1
    SYMBOL rodata_wlog_error.13  $965 T,  SYMBOL wifi_log
$14 L:
  A1 48 ENTRY,  A10 A3 0 L32I.N,  A8 A10 22 1 EXTUI,  A8 $9C L BNEZ,
  A11 1 MOVI.N,  A8 A11 A2 MOVEQZ,  A8 $9C L BNEZ,
  A8 A2 11 L8UI,  A8 A8 0 2 EXTUI,  A13 A3 5 L8UI,  A9 A3 6 L8UI,  A8 $46 L BEQZ.N,
  A10 3 $40 L BBCI,
  A2 A2 8 L8UI,  $86 L J,
  2 ZEROS,
$40 L:
  A2 A2 9 L8UI,  $86 L J,
$46 L:
  A12 A3 28 L32I.N,  A12 $6C L BNEZ.N,
  A12 A11 MOV.N,  A10 6 MOVI.N,  A2 $C L L32R,  A11 A11 11 SLLI,  A15 $0 L L32R,
  A14 $4 L L32R,  A13 $8 L L32R,  A2 A1 0 S32I.N,  MEMW,  A8 $10 L L32R,  A8 CALLX8,
$68 L:
  $68 L J,
  1 ZEROS,
$6C L:
  A11 A9 A13 MAXU,  A10 A12 1 ADDI.N,  A9 A8 MOV.N,  A2 4 MOVI.N,
$75 L:
  A13 A10 0 L8UI,  A8 A8 A13 ADD.N,  A8 A8 0 8 EXTUI,  A11 A8 $92 L BGEU,
  A9 A9 A12 ADDX2,  A2 A9 0 L8UI,
$86 L:
  A2 A3 12 S8I,  MEMW,  MEMW,  $9C L J,
$92 L:
  A9 A9 1 ADDI.N,  A10 A10 2 ADDI.N,  A2 A2 -1 ADDI,  A2 $75 L BNEZ,
$9C L:
  RETW.N,
$9E L:  END-CODE

CODE rssi_margin   \ .text.rssi_margin $4
  $0 1 LITERALS
    SYMBOL rodata.rssi_margin
$4 L:
  A1 32 ENTRY,  A3 A3 0 8 EXTUI,  A8 A2 0 8 EXTUI,  A3 8 $50 L BGEUI,
  $2C L J,
  1 ZEROS,
$14 L:
  A3 A3 -9 ADDI,  A3 A3 0 8 EXTUI,  A2 22 MOVI.N,  A2 A3 $90 L BLTU,
  A2 $0 L L32R,  A3 A3 A2 ADDX4,  A2 A3 0 L32I.N,  A2 JX,
  2 ZEROS,
$2C L:
  A2 1 MOVI.N,  A3 SSL,  A3 A2 SLL,  A9 35 MOVI.N,  A9 A3 A9 AND,  A9 $124 L BNEZ,
  A10 136 MOVI,  A3 A10 $45 L BNONE,
  $110 L J,
$45 L:
  A2 68 MOVI.N,  A2 A3 A2 AND,  A2 $DD L BNEZ,
  $144 L J,
$50 L:
  A2 A3 -8 ADDI,  A2 A2 0 8 EXTUI,  A9 23 MOVI.N,  A9 A2 $14 L BGEU,
  $142 L J,
  1 ZEROS,
$5F L:
  A2 33 MOVI.N,  A2 A8 $67 L BLTU,
  $142 L J,
$67 L:
  A2 A8 -127 ADDI,  A2 $142 L BEQZ,
  A2 A8 -33 ADDI,  $84 L J,
$73 L:
  A2 29 MOVI.N,  A2 A8 $7B L BLTU,
  $142 L J,
$7B L:
  A2 A8 -127 ADDI,  A2 $142 L BEQZ,
  A2 A8 -29 ADDI,
$84 L:
  A2 A2 1 SRAI,  A2 A2 A2 ADDX2,  A2 A2 0 8 EXTUI,  $13A L J,
$90 L:
  A2 26 MOVI.N,  A2 A8 $98 L BLTU,
  $142 L J,
$98 L:
  A2 A8 -127 ADDI,  A2 $142 L BEQZ,
  A2 A8 -26 ADDI,  $84 L J,
$A4 L:
  A2 21 MOVI.N,  A2 A8 $AC L BLTU,
  $142 L J,
$AC L:
  A2 A8 -127 ADDI,  A2 $142 L BEQZ,
  A2 A8 -21 ADDI,  $84 L J,
$B8 L:
  A2 16 MOVI.N,  A2 A8 $C0 L BLTU,
  $142 L J,
$C0 L:
  A2 A8 -127 ADDI,  A2 $142 L BEQZ,
  A2 A8 -16 ADDI,  $84 L J,
$CC L:
  A2 13 MOVI.N,  A2 A8 $142 L BGEU,
  A2 A8 -127 ADDI,  A2 $142 L BEQZ,
  A2 A8 -13 ADDI,  $84 L J,
$DD L:
  A2 11 MOVI.N,  A2 A8 $142 L BGEU,
  A2 A8 -127 ADDI,  A2 $142 L BEQZ,
  A2 A8 -11 ADDI,  $84 L J,
$EE L:
  A2 8 MOVI.N,  A2 A8 $142 L BGEU,
  A2 A8 -127 ADDI,  A2 $142 L BEQZ,
  A2 A8 -8 ADDI,  $84 L J,
  1 ZEROS,
$100 L:
  A2 9 MOVI.N,  A2 A8 $142 L BGEU,
  A2 A8 -127 ADDI,  A2 $142 L BEQZ.N,
  A2 A8 -9 ADDI,  $84 L J,
$110 L:
  A3 14 MOVI.N,  A3 A8 $142 L BGEU,
  A3 A8 -127 ADDI,  A2 A9 A3 MOVEQZ,  A2 $142 L BEQZ.N,
  A2 A8 -14 ADDI,  $84 L J,
  1 ZEROS,
$124 L:
  A2 10 MOVI.N,  A2 A8 $142 L BGEU,
  A2 A8 -127 ADDI,  A2 $142 L BEQZ.N,
  A8 A8 -10 ADDI,  A8 A8 1 SRAI,  A8 A8 A8 ADDX2,  A2 A8 0 8 EXTUI,
$13A L:
  A3 32 MOVI.N,  A2 A2 A3 MINU,  $144 L J,
$142 L:
  A2 0 MOVI.N,
$144 L:
  RETW.N,
  rodata.rssi_margin  $B8 L!+  $DD L!+  $EE L!+  $73 L!+  $A4 L!+  $CC L!+  $100 L!+
  $EE L!+  $DD L!+  $CC L!+  $B8 L!+  $A4 L!+  $90 L!+  $73 L!+  $5F L!+  $EE L!+
  $DD L!+  $CC L!+  $B8 L!+  $A4 L!+  $90 L!+  $73 L!+  $5F L!+  DROP   \ the table of jumps
$146 L:  END-CODE

CODE TRC_PER_IS_GOOD   \ .text.TRC_PER_IS_GOOD $4
  $0 1 LITERALS
    SYMBOL rssi_margin
$4 L:
  A1 32 ENTRY,  A4 A2 96 L32I,  A10 A2 1 L8UI,  A2 90 MOVI,  A11 A4 0 L8UI,
  A8 $0 L L32R,  A8 CALLX8,
  A9 A2 A10 SUB,  A8 A4 9 L8UI,  A8 $2D L BEQZ.N,
  A9 100 MOVI,  A2 64 MOVI,  A9 A9 A10 SUB,  A2 A8 $3A L BLTU,
$2D L:
  A2 1 MOVI.N,  A9 A3 $34 L BLTU,
  A2 0 MOVI.N,
$34 L:
  A2 A2 0 8 EXTUI,  $53 L J,
$3A L:
  A2 0 MOVI.N,  A9 A3 $50 L BGEU,
  A8 A8 2 ADDI,  A8 A8 A10 SUB,  A9 1 MOVI,  A8 A3 $4E L BLTU,
  A9 A2 A2 OR,
$4E L:
  A2 A9 MOV.N,
$50 L:
  A2 A2 0 1 EXTUI,
$53 L:
  RETW.N,
$55 L:  END-CODE

CODE TRC_AMPDU_PER_DOWN_THRESHOLD   \ .text.TRC_AMPDU_PER_DOWN_THRESHOLD $14
  $0 5 LITERALS
    SYMBOL rodata.TRC_AMPDU_PER_DOWN_~934C  SYMBOL rssi_margin  SYMBOL rssi_margin
    SYMBOL rssi_margin  SYMBOL rssi_margin
$14 L:
  A1 32 ENTRY,  A8 A2 13 L8UI,  A9 12 MOVI.N,  A8 A8 -19 ADDI,  A8 A8 0 8 EXTUI,
  A9 A8 $84 L BLTU,
  A9 $0 L L32R,  A8 A8 A9 ADDX4,  A8 A8 0 L32I.N,  A8 JX,
$30 L:
  A11 31 MOVI.N,  $3C L J,
$35 L:
  A11 23 MOVI.N,  $4D L J,
$3A L:
  A11 22 MOVI.N,
$3C L:
  A10 A2 1 L8UI,  A2 114 MOVI,  A8 $4 L L32R,  A8 CALLX8,
  $7C L J,
$4B L:
  A11 21 MOVI.N,
$4D L:
  A10 A2 1 L8UI,  A2 115 MOVI,  A8 $8 L L32R,  A8 CALLX8,
  $7C L J,
  1 ZEROS,
$5D L:
  A10 A2 1 L8UI,  A11 20 MOVI.N,  A8 $C L L32R,  A8 CALLX8,
  A2 109 MOVI,  $7C L J,
$6E L:
  A10 A2 1 L8UI,  A11 19 MOVI.N,  A8 $10 L L32R,  A8 CALLX8,
  A2 108 MOVI,
$7C L:
  A2 A2 A10 SUB,  $87 L J,
  2 ZEROS,
$84 L:
  A2 114 MOVI,
$87 L:
  RETW.N,
  rodata.TRC_AMPDU_PER_DOWN_~934C  $6E L!+  $5D L!+  $4B L!+  $3A L!+  $35 L!+
  $84 L!+  $84 L!+  $84 L!+  $84 L!+  $84 L!+  $84 L!+  $84 L!+  $30 L!+  DROP   \ the table of jumps
$89 L:  END-CODE

CODE rcAmpduLowerRate   \ .text.rcAmpduLowerRate $38
  $0 14 LITERALS
    SYMBOL trc_ctl  SYMBOL g_mesh_is_started  SYMBOL g_mesh_is_root
    SYMBOL g_mesh_init_ps_type  SYMBOL g_osi_funcs_p  $3FF73C00 T,
    SYMBOL g_wifi_mac_time_delta  SYMBOL TRC_AMPDU_PER_DOWN_THRESHOLD
    SYMBOL trcAmpduSetState  SYMBOL rcClearCurSched  SYMBOL trc_onAmpduOp
    SYMBOL rx11NRate2AMPDULimit  SYMBOL rcSetTxAmpduLimit
    SYMBOL rcClearCurAMPDUSched
$38 L:
  A1 32 ENTRY,  A4 $0 L L32R,  A3 A3 0 8 EXTUI,  A4 A4 24 L8UI,  A4 A3 $AA L BNE,
  A10 A2 A2 OR,  A8 $1C L L32R,  A8 CALLX8,
  A3 A2 30 L8UI,  A3 A3 20 ADDI,  A3 A10 $5C L BLTU,
  $11C L J,
$5C L:
  A11 3 MOVI.N,  A10 A2 MOV.N,  A3 3 MOVI,  A4 A4 -1 ADDI,  A8 $20 L L32R,
  A8 CALLX8,
  A4 A4 0 8 EXTUI,  A3 A2 14 S8I,  MEMW,  A4 A4 A4 ADDX2,  A10 A2 MOV.N,
  A3 A2 112 L32I,  A4 A4 A3 ADDX4,  A3 0 MOVI.N,  A4 A2 96 S32I,  MEMW,
  A8 $24 L L32R,  A8 CALLX8,
  A4 3 MOVI.N,
$90 L:
  A8 A2 16 L8UI,  A8 A3 $A2 L BBC,
  A12 A3 MOV.N,  A11 A4 MOV.N,  A10 A2 MOV.N,  A8 $28 L L32R,  A8 CALLX8,
$A2 L:
  A3 A3 1 ADDI.N,  A3 8 $90 L BNEI,
  $124 L J,
$AA L:
  A4 0 MOVI.N,  A4 A2 12 S8I,  MEMW,  A4 $4 L L32R,  A4 A4 0 L8UI,  A4 $E0 L BEQZ.N,
  A4 $8 L L32R,  MEMW,  A4 A4 0 L8UI,  A4 A4 0 8 EXTUI,  A4 $E0 L BNEZ.N,
  A4 $C L L32R,  A4 A4 0 L32I.N,  A4 $E0 L BEQZ.N,
  A4 $10 L L32R,  A4 A4 0 L32I.N,  A10 A4 $110 L32I,  MEMW,  A10 CALLX8,
  $EF L J,
$E0 L:
  A4 $14 L L32R,  MEMW,  A10 A4 0 L32I.N,  A4 $18 L L32R,  A4 A4 0 L32I.N,
  A10 A10 A4 ADD.N,
$EF L:
  A3 A3 1 ADDI.N,  A3 A3 0 8 EXTUI,  A4 A2 112 L32I,  A3 A3 A3 ADDX2,
  A3 A3 A4 ADDX4,  A10 A2 80 S32I,  A10 A3 0 L8UI,  A10 A2 13 S8I,  MEMW,  MEMW,
  A8 $2C L L32R,  A8 CALLX8,
  A11 A10 MOV.N,  A10 A2 MOV.N,  A8 $30 L L32R,  A8 CALLX8,
$11C L:
  A10 A2 MOV.N,  A8 $34 L L32R,  A8 CALLX8,
$124 L:
  RETW.N,
$126 L:  END-CODE

CODE TRC_AMPDU_PER_UP_THRESHOLD   \ .text.TRC_AMPDU_PER_UP_THRESHOLD $C
  $0 3 LITERALS
    SYMBOL rodata.TRC_AMPDU_PER_UP_TH~9E84  SYMBOL rssi_margin  SYMBOL rssi_margin
$C L:
  A1 32 ENTRY,  A8 A2 13 L8UI,  A9 12 MOVI.N,  A8 A8 -19 ADDI,  A8 A8 0 8 EXTUI,
  A9 A8 $65 L BLTU,
  A9 $0 L L32R,  A8 A8 A9 ADDX4,  A8 A8 0 L32I.N,  A8 JX,
$28 L:
  A11 31 MOVI.N,  $40 L J,
$2D L:
  A11 23 MOVI.N,  $40 L J,
$32 L:
  A11 22 MOVI.N,  $40 L J,
$37 L:
  A11 21 MOVI.N,  $40 L J,
  2 ZEROS,
$3E L:
  A11 20 MOVI.N,
$40 L:
  A10 A2 1 L8UI,  A2 121 MOVI,  A8 $4 L L32R,  A8 CALLX8,
  $5F L J,
  2 ZEROS,
$51 L:
  A10 A2 1 L8UI,  A11 19 MOVI.N,  A8 $8 L L32R,  A8 CALLX8,
  A2 116 MOVI,
$5F L:
  A2 A2 A10 SUB,  $68 L J,
$65 L:
  A2 121 MOVI,
$68 L:
  RETW.N,
  rodata.TRC_AMPDU_PER_UP_TH~9E84  $51 L!+  $3E L!+  $37 L!+  $32 L!+  $2D L!+
  $65 L!+  $65 L!+  $65 L!+  $65 L!+  $65 L!+  $65 L!+  $65 L!+  $28 L!+  DROP   \ the table of jumps
$6A L:  END-CODE

CODE rcUpdateTxDoneAmpdu2   \ .wifi0iram.9 $58
  $0 22 LITERALS
    $1FFFFFF T,  SYMBOL trc_ctl  SYMBOL g_mesh_is_started  SYMBOL g_mesh_is_root
    SYMBOL g_mesh_init_ps_type  SYMBOL g_osi_funcs_p  $3FF73C00 T,
    SYMBOL g_wifi_mac_time_delta  $1869F T,  $186A0 T,  $3D08FF T,  $7A120 T,
    SYMBOL rcUpdateAckSnr  SYMBOL TRC_AMPDU_PER_DOWN_THRESHOLD
    SYMBOL trc_calc_duration  SYMBOL trc_calc_duration  SYMBOL rcAmpduLowerRate
    SYMBOL TRC_AMPDU_PER_UP_THRESHOLD  SYMBOL trc_calc_duration
    SYMBOL rx11NRate2AMPDULimit  SYMBOL rcSetTxAmpduLimit
    SYMBOL rcClearCurAMPDUSched
$58 L:
  A1 48 ENTRY,  A5 A3 4 L32I.N,  A7 A4 0 16 EXTUI,  A5 A5 0 4 EXTUI,  A9 A3 7 L8UI,
  A6 A3 6 L8UI,  A9 $37A L BEQZ,
  A4 A2 96 L32I,  A4 $37A L BEQZ,
  A4 A2 14 L8UI,  A4 2 $7B L BEQI,
  $37A L J,
$7B L:
  A10 4 MOVI.N,  A4 A2 11 L8UI,  A4 A10 $86 L BNONE,
  $37A L J,
$86 L:
  A4 $7FF MOVI,  A7 A7 A4 ADD.N,  A11 A3 19 L8UI,  A8 A6 0 16 EXTUI,  A7 A7 11 SRLI,
  A4 A3 5 L8UI,  A11 1 $BD L BNEI,
  A4 A4 A7 MUL16U,  A4 A9 A4 ADD.N,  A7 A2 25 L8UI,  A7 A10 $D4 L BANY,
  A10 A2 MOV.N,  A11 A3 13 L8UI,  A11 A11 7 SEXT,  A8 A1 0 S32I.N,  MEMW,
  A8 $30 L L32R,  A8 CALLX8,
  A8 A1 0 L32I.N,  $D4 L J,
$BD L:
  A8 $CC L BEQZ.N,
  A4 A4 -1 ADDI.N,  A4 A4 A7 MUL16U,  A9 A9 A4 ADD.N,  A4 A9 0 16 EXTUI,  $D4 L J,
$CC L:
  A4 A4 A7 MULL,  $DA L J,
  2 ZEROS,
$D4 L:
  A4 A8 $DA L BGEU,
  $37A L J,
$DA L:
  A3 A2 68 L32I,  A7 A3 A6 ADD.N,  A9 $0 L L32R,  A9 A7 $F4 L BGEU,
  A3 A3 1 SRLI,  A7 A2 64 L32I,  A3 A2 68 S32I,  A3 A7 1 SRLI,  A3 A2 64 S32I,
$F4 L:
  A3 A2 68 L32I,  A3 A3 A6 ADD.N,  A3 A2 68 S32I,  A3 A2 64 L32I,  A3 A3 A4 ADD.N,
  A3 A2 64 S32I,  A10 A2 13 L8UI,  A3 A2 116 L32I,  A8 A1 0 S32I.N,  MEMW,
  A3 CALLX8,
  A3 A10 MOV.N,  A8 A1 0 L32I.N,  A7 A2 12 L8UI,  A8 $168 L BNEZ,
  A7 A7 1 ADDI.N,  A7 A7 0 8 EXTUI,  A8 A2 15 L8UI,  A8 A4 A8 ADD.N,  A8 A2 15 S8I,
  MEMW,  A8 A2 116 L32I,  A7 A2 12 S8I,  MEMW,  A7 A7 A10 ADD,  A10 16 MOVI,  MEMW,
  A8 CALLX8,
  A10 A7 $150 L BGE,
  A7 $4 L L32R,  A7 A7 24 L8UI,  A3 A7 $150 L BGEU,
  $28D L J,
$150 L:
  A8 8 MOVI.N,  A7 A2 12 L8UI,  A8 A7 $15C L BGEU,
  $28D L J,
  1 ZEROS,
$15C L:
  A7 A2 15 L8UI,  A8 64 MOVI.N,  A8 A7 $17A L BGEU,
  $28D L J,
  1 ZEROS,
$168 L:
  A7 $17A L BEQZ.N,
  A3 0 MOVI.N,  A3 A2 12 S8I,  MEMW,  MEMW,  $37A L J,
  2 ZEROS,
$17A L:
  A10 A2 MOV.N,  A6 A6 7 SLLI,  A8 $34 L L32R,  A8 CALLX8,
  A6 A6 A4 QUOS,  A7 A10 MOV.N,  A10 A6 $195 L BGEU,
  A4 0 MOVI.N,  A4 A2 15 S8I,  MEMW,
$195 L:
  A6 $8 L L32R,  A4 A6 0 L8UI,  A4 $1C5 L BEQZ.N,
  A4 $C L L32R,  MEMW,  A4 A4 0 L8UI,  A4 A4 0 8 EXTUI,  A4 $1C5 L BNEZ.N,
  A4 $10 L L32R,  A4 A4 0 L32I.N,  A4 $1C5 L BEQZ.N,
  A4 $14 L L32R,  A4 A4 0 L32I.N,  A10 A4 $110 L32I,  MEMW,  A10 CALLX8,
  A6 A10 MOV.N,  $1D4 L J,
$1C5 L:
  A4 $18 L L32R,  MEMW,  A9 A4 0 L32I.N,  A4 $1C L L32R,  A4 A4 0 L32I.N,
  A6 A9 A4 ADD.N,
$1D4 L:
  A4 A2 64 L32I,  A11 A2 76 L32I,  A4 $37A L BEQZ,
  A10 A6 MOV.N,  A8 $38 L L32R,  A8 CALLX8,
  A8 $20 L L32R,  A8 A10 $1F4 L BLTU,
  A8 $1F3 MOVI,  A8 A4 $1F4 L BLTU,
  $37A L J,
$1F4 L:
  A8 A2 88 L32I,  A8 A8 1 ADDI.N,  A8 A2 88 S32I,  A8 A2 68 L32I,  A8 A8 7 SLLI,
  A8 A8 A4 QUOU,  A4 A8 0 8 EXTUI,  A11 A2 30 L8UI,  A6 A2 76 S32I,  A4 A2 29 S8I,
  MEMW,  A11 $238 L BEQZ.N,
  A8 A8 0 8 EXTUI,  A4 A11 A11 ADDX2,  A4 A4 A8 ADD.N,  A4 A4 2 SRAI,  A4 A2 30 S8I,
  MEMW,  A10 A4 A11 MAXU,  A8 A2 39 L8UI,  A10 A7 $29C L BGEU,
  MEMW,  $266 L J,
  2 ZEROS,
$238 L:
  A3 128 MOVI,  A10 A7 A7 ADDX2,  A10 A10 A3 ADD.N,  A3 A10 2 SRLI,
  A10 A10 2 8 EXTUI,  A10 A4 $24C L BLTU,
  $369 L J,
$24C L:
  A3 A3 0 8 EXTUI,  A8 A8 0 8 EXTUI,  A3 A3 A8 ADD.N,  A3 A3 1 SRAI,  A3 A2 30 S8I,
  MEMW,  MEMW,  $36F L J,
  3 ZEROS,
$266 L:
  A8 $276 L BEQZ.N,
  A4 A2 92 L32I,  A7 $28 L L32R,  A7 A4 $276 L BLTU,
  A4 A4 A4 ADD.N,  A4 A2 92 S32I,
$276 L:
  A10 A6 MOV.N,  A11 A2 80 L32I,  MEMW,  A8 $3C L L32R,  A8 CALLX8,
  A4 $24 L L32R,  A4 A10 $28D L BLTU,
  $36F L J,
$28D L:
  A12 A5 MOV.N,  A11 A3 MOV.N,  A10 A2 MOV.N,  A8 $40 L L32R,  A8 CALLX8,
  $37A L J,
$29C L:
  A8 $2B4 L BEQZ.N,
  A3 0 MOVI.N,  A3 A2 39 S8I,  MEMW,  A3 $2C L L32R,  A3 A2 92 S32I,  MEMW,
  $36F L J,
  2 ZEROS,
$2B4 L:
  A5 A2 4 L8UI,  A5 A3 $2BD L BNE,
  $36F L J,
$2BD L:
  A10 A2 MOV.N,  A11 A1 0 S32I.N,  MEMW,  A8 $44 L L32R,  A8 CALLX8,
  A11 A1 0 L32I.N,  A4 A4 A11 MINU,  A10 A4 $2D5 L BLTU,
  $36F L J,
$2D5 L:
  A10 A6 MOV.N,  A11 A2 84 L32I,  A8 $48 L L32R,  A8 CALLX8,
  A4 A2 92 L32I,  A4 A10 $2E9 L BLTU,
  $36F L J,
$2E9 L:
  A5 $8 L L32R,  A4 A5 0 L8UI,  A4 $315 L BEQZ.N,
  A4 $C L L32R,  MEMW,  A4 A4 0 L8UI,  A4 A4 0 8 EXTUI,  A4 $315 L BNEZ.N,
  A4 $10 L L32R,  A4 A4 0 L32I.N,  A4 $315 L BEQZ.N,
  A4 $14 L L32R,  A4 A4 0 L32I.N,  A10 A4 $110 L32I,  A10 CALLX8,
  $324 L J,
  1 ZEROS,
$315 L:
  A4 $18 L L32R,  MEMW,  A10 A4 0 L32I.N,  A4 $1C L L32R,  A4 A4 0 L32I.N,
  A10 A10 A4 ADD.N,
$324 L:
  A4 0 MOVI.N,  A3 A3 -1 ADDI.N,  A4 A2 12 S8I,  MEMW,  A3 A3 0 8 EXTUI,
  A3 A3 A3 ADDX2,  A4 A2 112 L32I,  A3 A3 A4 ADDX4,  A10 A2 84 S32I,  A10 A3 0 L8UI,
  A3 1 MOVI.N,  A10 A2 13 S8I,  A3 A2 39 S8I,  MEMW,  MEMW,  A8 $4C L L32R,
  A8 CALLX8,
  A11 A10 MOV.N,  A10 A2 MOV.N,  A8 $50 L L32R,  A8 CALLX8,
  A10 A2 MOV.N,  A8 $54 L L32R,  A8 CALLX8,
  $37A L J,
$369 L:
  A10 A2 30 S8I,  MEMW,
$36F L:
  A3 0 MOVI.N,  A3 A2 68 S32I,  A3 A2 64 S32I,  A3 A2 72 S32I,
$37A L:
  MEMW,  RETW.N,
$37F L:  END-CODE

CODE rcUpdateRate   \ .text.rcUpdateRate $50
  $0 20 LITERALS
    SYMBOL g_mesh_is_started  SYMBOL g_mesh_is_root  SYMBOL g_mesh_init_ps_type
    SYMBOL g_osi_funcs_p  $3FF73C00 T,  SYMBOL g_wifi_mac_time_delta  $1E847 T,
    $1E848 T,  $7A120 T,  SYMBOL trc_ctl  $3D08FF T,  SYMBOL trc_calc_duration
    SYMBOL trc_calc_duration  SYMBOL rcClearCurSched  SYMBOL trc_calc_duration
    SYMBOL TRC_PER_IS_GOOD  SYMBOL rcUpSched  SYMBOL rssi_margin
    SYMBOL trc_calc_duration  SYMBOL rcLowerSched
$50 L:
  A1 48 ENTRY,  A3 $0 L L32R,  A3 A3 0 L8UI,  A3 $81 L BEQZ.N,
  A3 $4 L L32R,  MEMW,  A3 A3 0 L8UI,  A3 A3 0 8 EXTUI,  A3 $81 L BNEZ.N,
  A3 $8 L L32R,  A3 A3 0 L32I.N,  A3 $81 L BEQZ.N,
  A3 $C L L32R,  A3 A3 0 L32I,  A10 A3 $110 L32I,  A10 CALLX8,
  A4 A10 MOV.N,  $90 L J,
$81 L:
  A3 $10 L L32R,  MEMW,  A4 A3 0 L32I.N,  A3 $14 L L32R,  A3 A3 0 L32I.N,
  A4 A4 A3 ADD.N,
$90 L:
  A6 A2 40 L32I.N,  A3 A2 56 L32I.N,  A6 $D8 L BEQZ,
  A11 A3 MOV.N,  A10 A4 MOV.N,  A8 $2C L L32R,  A8 CALLX8,
  A5 A2 60 L32I.N,  A7 99 MOVI,  A7 A5 $D8 L BGEU,
  A5 $18 L L32R,  A5 A10 $D8 L BGEU,
  A3 $24 L L32R,  A5 A3 0 L32I.N,  A5 A5 1 ADDI.N,  A5 A3 0 S32I.N,
  A3 A2 44 L32I.N,  A3 A3 7 SLLI,  A3 A3 A6 QUOU,  A6 A3 0 8 EXTUI,  A7 A2 28 L8UI,
  A4 A2 56 S32I.N,  A6 A2 27 S8I,  MEMW,  A7 $14A L BNEZ,
  MEMW,  $176 L J,
  1 ZEROS,
$D8 L:
  A4 A2 76 L32I,  A11 A3 MOV.N,  A10 A4 MOV.N,  A8 $30 L L32R,  A8 CALLX8,
  A3 A2 14 L8UI,  A3 2 $EE L BEQI,
  $248 L J,
$EE L:
  A3 $18 L L32R,  A3 A10 $F7 L BLTU,
  $248 L J,
$F7 L:
  A10 A2 MOV.N,  A3 A2 96 L32I,  A3 A3 10 L8UI,  A4 A2 56 S32I.N,  A4 $20 L L32R,
  A4 A2 92 S32I,  MEMW,  A8 $34 L L32R,  A8 CALLX8,
  A10 A2 13 L8UI,  A5 A2 116 L32I,  A4 A2 112 L32I,  A5 CALLX8,
  A10 A10 0 16 EXTUI,  A10 A10 A10 ADDX2,  A10 A10 A4 ADDX4,  A10 A2 96 S32I,
  A4 A10 10 L8UI,  A3 A4 $139 L BGEU,
  A3 A2 76 L32I,  A3 A2 48 S32I.N,  MEMW,  $240 L J,
$139 L:
  A4 A3 $13F L BLTU,
  $240 L J,
$13F L:
  A3 A2 76 L32I,  A3 A2 52 S32I.N,  MEMW,  $240 L J,
$14A L:
  A3 A3 0 8 EXTUI,  A3 A3 A7 ADD.N,  A11 A2 52 L32I.N,  A3 A3 1 SRAI,  A10 A4 MOV.N,
  A3 A2 28 S8I,  MEMW,  MEMW,  A8 $38 L L32R,  A8 CALLX8,
  A7 A10 MOV.N,  A11 A3 MOV.N,  A10 A2 MOV.N,  A8 $3C L L32R,  A8 CALLX8,
  A10 $1AD L BNEZ.N,
  $1C0 L J,
$176 L:
  A4 A2 96 L32I,  A8 A4 9 L8UI,  A4 128 MOVI,  A8 A8 A8 ADDX2,  A8 A8 A4 ADD.N,
  A4 A8 2 SRAI,  A8 A8 2 8 EXTUI,  A8 A6 $199 L BLTU,
  A8 A2 28 S8I,  MEMW,  MEMW,  $240 L J,
$199 L:
  A3 A3 0 8 EXTUI,  A3 A3 A4 ADD.N,  A3 A3 1 SRAI,  A3 A2 28 S8I,  MEMW,  MEMW,
  $240 L J,
$1AD L:
  A6 A2 92 L32I,  A6 A7 $1C0 L BGEU,
  A10 A2 A2 OR,  A8 $40 L L32R,  A8 CALLX8,
  $240 L J,
  1 ZEROS,
$1C0 L:
  A6 A2 96 L32I,  A8 A6 11 L8UI,  A9 A8 0 1 EXTUI,  A9 $1D1 L BNEZ.N,
  A5 A5 0 2 EXTUI,  A5 $240 L BNEZ,
$1D1 L:
  A11 A6 0 L8UI,  A10 A2 1 L8UI,  A8 A1 0 S32I.N,  A9 A1 4 S32I.N,  MEMW,
  A8 $44 L L32R,  A8 CALLX8,
  A10 A10 1 8 EXTUI,  A5 A6 9 L8UI,  A5 A5 A10 SUB,  A8 A1 0 L32I.N,
  A9 A1 4 L32I.N,  A3 A5 $21D L BGE,
  A9 $204 L BEQZ.N,
  A3 A2 92 L32I,  A5 $28 L L32R,  A5 A3 $204 L BLTU,
  A3 A3 A3 ADD.N,  A3 A2 92 S32I,
$204 L:
  A10 A4 MOV.N,  A11 A2 48 L32I.N,  MEMW,  A8 $48 L L32R,  A8 CALLX8,
  A3 $1C L L32R,  A3 A7 $240 L BGEU,
  A3 A10 $236 L BLTU,
  $240 L J,
$21D L:
  A9 $240 L BEQZ.N,
  A3 -2 MOVI.N,  A8 A8 A3 AND,  A3 $20 L L32R,  A8 A6 11 S8I,  MEMW,  A3 A2 92 S32I,
  MEMW,  $240 L J,
$236 L:
  A11 1 MOVI.N,  A10 A2 MOV.N,  A8 $4C L L32R,  A8 CALLX8,
$240 L:
  A3 0 MOVI.N,  A3 A2 44 S32I.N,  A3 A2 40 S32I.N,  A3 A2 60 S32I.N,
$248 L:
  MEMW,  RETW.N,
$24D L:  END-CODE

CODE rcGetSched   \ .wifi0iram.10 $20
  $0 8 LITERALS
    $2000008 T,  $C SYMBOL+ rcLoRaSchedTbl  $24 SYMBOL+ rc11BSchedTbl
    SYMBOL g_wifi_nvs  SYMBOL BasicOFDMSched  SYMBOL rcLoRaSchedTbl
    SYMBOL rcUpdateRate  SYMBOL wl_is_ap_no_lr
$20 L:
  A1 32 ENTRY,  A10 A2 MOV.N,  A2 $E9 L BEQZ,
  A9 A3 0 L32I.N,  A11 $0 L L32R,  A11 A9 A11 AND,  A8 A2 11 L8UI,
  A11 8 $60 L BNEI,
  A11 A2 96 L32I,  A11 A3 28 S32I.N,  A8 0 $48 L BBCI,
  A8 A2 8 L8UI,  MEMW,  $E3 L J,
  1 ZEROS,
$48 L:
  A9 1 $51 L BBCI,
  A8 A2 104 L32I,  $DC L J,
$51 L:
  A8 A8 7 SEXT,  A8 $DE L BLTZ,
  A8 $18 L L32R,  A8 CALLX8,
  $DE L J,
$60 L:
  A11 A2 100 L32I,  A11 A3 28 S32I.N,  A8 1 $71 L BBCI,
  A8 A2 9 L8UI,  MEMW,  $E3 L J,
$71 L:
  A8 A8 7 SEXT,  A8 $C1 L BGEZ,
  A9 3 MOVI.N,  A8 A3 16 L32I.N,  A9 A9 18 SLLI,  A8 A9 $B0 L BANY,
  A8 16 $8D L BBCI,
$84 L:
  A8 $4 L L32R,  $DC L J,
  3 ZEROS,
$8D L:
  A8 17 $98 L BBSI,
$90 L:
  A8 $8 L L32R,  $DC L J,
  2 ZEROS,
$98 L:
  A8 $1C L L32R,  A8 CALLX8,
  A10 $90 L BNEZ,
  A8 $C L L32R,  A8 A8 0 L32I.N,  A8 A8 $400 ADDMI,  A8 A8 117 L8UI,  $BB L J,
  1 ZEROS,
$B0 L:
  A8 $C L L32R,  A8 A8 0 L32I.N,  A8 A8 $500 ADDMI,  A8 A8 13 L8UI,
$BB L:
  A8 $84 L BNEZ,
  $DE L J,
$C1 L:
  A8 $401 MOVI,  A8 A8 11 SLLI,  A9 A8 $DE L BNONE,
  A8 A2 130 L8UI,  A8 $DE L BEQZ.N,
  A8 $10 L L32R,  A8 A3 28 S32I.N,  A8 A2 135 L8UI,  A8 $DE L BEQZ.N,
  A8 $14 L L32R,
$DC L:
  A8 A3 28 S32I.N,
$DE L:
  A8 A3 28 L32I.N,  A8 A8 0 L8UI,
$E3 L:
  A8 A3 12 S8I,  MEMW,
$E9 L:
  MEMW,  RETW.N,
$EE L:  END-CODE

CODE rcReachRetryLimit   \ .text.rcReachRetryLimit $34
  $0 13 LITERALS
    SYMBOL rodata_wlog_error.14  SYMBOL rodata.__FUNCTION__$0
    SYMBOL rodata.rcGetAmpduSched.str1.1  SYMBOL rodata_wlog_error.15  $AC7 T,
    SYMBOL rodata_wlog_error.16  SYMBOL rodata_wlog_error.17  $AD2 T,
    SYMBOL g_mesh_is_started  SYMBOL wifi_log  SYMBOL wifi_log  SYMBOL wifi_log
    SYMBOL wifi_log
$34 L:
  A1 48 ENTRY,  A2 $6D L BNEZ.N,
  A13 $0 L L32R,  A12 1 MOVI.N,  A11 $200 MOVI,  A10 6 MOVI.N,  A8 $24 L L32R,
  A8 CALLX8,
  A2 $10 L L32R,  A12 1 MOVI.N,  A11 A12 11 SLLI,  A10 6 MOVI.N,  A15 $4 L L32R,
  A14 $8 L L32R,  A13 $C L L32R,  A2 A1 0 S32I,  MEMW,  A8 $28 L L32R,  A8 CALLX8,
$68 L:
  $68 L J,
  2 ZEROS,
$6D L:
  A9 A2 5 L8UI,  A9 5 $78 L BLTUI,
  A8 A2 0 L32I.N,  A8 27 $E8 L BBSI,
$78 L:
  A8 A2 28 L32I.N,  A8 $C0 L BNEZ,
  A12 1 MOVI.N,  A11 $200 MOVI,  A15 A2 4 L32I.N,  A10 6 MOVI.N,  A15 A15 0 4 EXTUI,
  A14 A2 16 L32I.N,  A14 A14 18 2 EXTUI,  A2 A2 0 L32I.N,  A13 $14 L L32R,
  A2 A1 0 S32I.N,  MEMW,  A8 $2C L L32R,  A8 CALLX8,
  A12 1 MOVI.N,  A11 A12 11 SLLI,  A10 6 MOVI.N,  A2 $1C L L32R,  A15 $4 L L32R,
  A14 $8 L L32R,  A13 $18 L L32R,  A2 A1 0 S32I.N,  MEMW,  A8 $30 L L32R,
  A8 CALLX8,
$BC L:
  $BC L J,
  1 ZEROS,
$C0 L:
  A2 $20 L L32R,  A10 A2 0 L8UI,  A2 1 MOVI.N,  A8 A8 8 L8UI,  A10 $DC L BEQZ.N,
  A10 11 MOVI.N,  A8 A8 A10 MINU,  A9 A8 $E1 L BGEU,
  A2 0 MOVI.N,  $E1 L J,
  2 ZEROS,
$DC L:
  A9 A8 $E1 L BGEU,
  A2 A10 MOV.N,
$E1 L:
  A2 A2 0 8 EXTUI,  $EA L J,
  1 ZEROS,
$E8 L:
  A2 1 MOVI.N,
$EA L:
  RETW.N,
$EC L:  END-CODE

CODE rcAttach   \ .text.rcAttach $34
  $0 13 LITERALS
    SYMBOL trc_ctl  SYMBOL rc11NSchedTbl  SYMBOL rcP2P11NSchedTbl
    SYMBOL rc11GSchedTbl  SYMBOL rcP2P11GSchedTbl  SYMBOL rc11BSchedTbl
    SYMBOL rcLoRaSchedTbl  SYMBOL rcBuildIndex  SYMBOL rcBuildIndex
    SYMBOL rcBuildIndex  SYMBOL rcBuildIndex  SYMBOL rcBuildIndex
    SYMBOL rcBuildIndex
$34 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A9 0 MOVI.N,  A11 14 MOVI.N,  A10 $4 L L32R,
  A9 A8 4 S32I.N,  A9 A8 8 S32I,  A9 A8 12 S32I,  A9 A8 20 S32I,  MEMW,
  A8 $1C L L32R,  A8 CALLX8,
  A11 10 MOVI,  A10 $8 L L32R,  A8 $20 L L32R,  A8 CALLX8,
  A11 13 MOVI,  A10 $C L L32R,  A8 $24 L L32R,  A8 CALLX8,
  A11 8 MOVI,  A10 $10 L L32R,  A8 $28 L L32R,  A8 CALLX8,
  A11 6 MOVI,  A10 $14 L L32R,  A8 $2C L L32R,  A8 CALLX8,
  A11 2 MOVI.N,  A10 $18 L L32R,  A8 $30 L L32R,  A8 CALLX8,
  RETW.N,
$92 L:  END-CODE

CODE trc_NeedRTS   \ .text.trc_NeedRTS $10
  $0 4 LITERALS
    $2020480 T,  $1010840 T,  SYMBOL rodata.trc_NeedRTS  SYMBOL lmacConfMib
$10 L:
  A1 32 ENTRY,  A8 A2 44 L32I.N,  A10 A8 12 L8UI,  A8 A2 20 L16UI,  A2 A2 22 L16UI,
  A8 A8 A2 ADD.N,  A10 6 $50 L BGEUI,
  $45 L J,
$26 L:
  A2 1 MOVI.N,  A11 $0 L L32R,  A10 SSL,  A9 A2 SLL,  A9 A11 $80 L BANY,
  A11 $4 L L32R,  A9 A9 A11 AND,  A9 $6E L BNEZ.N,
  A11 15 MOVI.N,  A10 A11 $88 L BNE,
  $77 L J,
  1 ZEROS,
$45 L:
  A2 $8 L L32R,  A2 A10 A2 ADDX4,  A2 A2 0 L32I.N,  A2 JX,
$50 L:
  A2 A10 -6 ADDI,  A2 A2 0 8 EXTUI,  A9 19 MOVI.N,  A9 A2 $26 L BGEU,
  $88 L J,
$5E L:
  A2 1 MOVI.N,  A9 $271 MOVI,  $90 L J,
$66 L:
  A2 1 MOVI.N,  A9 $3E8 MOVI,  $90 L J,
$6E L:
  A2 1 MOVI.N,  A9 $2BC MOVI,  $90 L J,
  1 ZEROS,
$77 L:
  A10 $41A MOVI,  A10 A8 $95 L BLT,
  $93 L J,
$80 L:
  A2 1 MOVI.N,  A9 $460 MOVI,  $90 L J,
$88 L:
  A2 $C L L32R,  A9 A2 22 L16UI,  A2 1 MOVI.N,
$90 L:
  A9 A8 $95 L BLT,
$93 L:
  A2 0 MOVI.N,
$95 L:
  A2 A2 0 8 EXTUI,  RETW.N,
  rodata.trc_NeedRTS  $5E L!+  $66 L!+  $6E L!+  $80 L!+  $88 L!+  $66 L!+  DROP   \ the table of jumps
$9A L:  END-CODE

CODE trc_onDisconnect   \ .text.trc_onDisconnect $0
  $0 ORIGIN
  A1 32 ENTRY,  RETW.N,
$5 L:  END-CODE

CODE trc_onScanStart   \ .text.trc_onScanStart $0
  $0 ORIGIN
  A1 32 ENTRY,  A8 A2 25 L8UI,  A9 -6 MOVI.N,  A8 A8 A9 AND,  A9 5 MOVI.N,
  A8 A8 A9 OR,  A8 A2 25 S8I,  MEMW,  MEMW,  RETW.N,
$1B L:  END-CODE

CODE trc_onScanDone   \ .text.trc_onScanDone $0
  $0 ORIGIN
  A1 32 ENTRY,  A8 A2 25 L8UI,  A9 -6 MOVI.N,  A8 A8 A9 AND,  A8 A2 25 S8I,  MEMW,
  MEMW,  RETW.N,
$16 L:  END-CODE

CODE trc_isAmpduOn   \ .text.trc_isAmpduOn $0
  $0 ORIGIN
  A1 32 ENTRY,  A2 A2 25 L8UI,  A2 A2 7 SRLI,  RETW.N,
$B L:  END-CODE

CODE trc_onPPTxDone   \ .wifiextrairam.18 $10
  $0 4 LITERALS
    SYMBOL trc_ctl  SYMBOL trcAmpduSetState  SYMBOL rcClearCurAMPDUSched
    SYMBOL trc_onAmpduOp
$10 L:
  A1 32 ENTRY,  A2 $9F L BEQZ,
  A4 A2 11 L8UI,  A8 A4 7 SEXT,  A8 $9F L BLTZ,
  A4 A4 0 1 EXTUI,  A4 $9F L BNEZ,
  A8 A2 25 L8UI,  A8 7 $9F L BBCI,
  A3 A3 44 L32I.N,  A3 A3 4 L32I.N,  A3 A3 0 4 EXTUI,  A3 A2 A3 ADD.N,
  A3 A3 17 L8UI,  A3 1 $9F L BNEI,
  A3 $0 L L32R,  A8 A3 25 L8UI,  A3 A2 96 L32I,  A9 A3 10 L8UI,  A3 A8 -1 ADDI.N,
  A9 A3 $9F L BGE,
  A8 A8 -2 ADDI,  A8 A8 0 8 EXTUI,  A3 A2 112 L32I,  A8 A8 A8 ADDX2,
  A8 A8 A3 ADDX4,  A11 4 MOVI.N,  A10 A2 A2 OR,  A3 A8 0 L8UI,  A3 A2 13 S8I,  MEMW,
  MEMW,  A8 $4 L L32R,  A8 CALLX8,
  A3 4 MOVI.N,  A10 A2 MOV.N,  A3 A2 14 S8I,  MEMW,  MEMW,  A8 $8 L L32R,
  A8 CALLX8,
  A3 A4 MOV.N,  A4 2 MOVI.N,
$88 L:
  A8 A2 16 L8UI,  A8 A3 $9A L BBC,
  A12 A3 MOV.N,  A11 A4 MOV.N,  A10 A2 MOV.N,  A8 $C L L32R,  A8 CALLX8,
$9A L:
  A3 A3 1 ADDI.N,  A3 8 $88 L BNEI,
$9F L:
  RETW.N,
$A1 L:  END-CODE

CODE trc_SetTxAmpduState   \ .text.trc_SetTxAmpduState $0
  $0 ORIGIN
  A1 32 ENTRY,  A3 A2 14 S8I,  MEMW,  MEMW,  RETW.N,
$E L:  END-CODE

CODE trc_isTxAmpduOperational   \ .wifi0iram.19 $0
  $0 ORIGIN
  A1 32 ENTRY,  A8 A2 14 L8UI,  A9 0 MOVI.N,  A8 A8 -2 ADDI,  A2 1 MOVI.N,
  A2 A9 A8 MOVNEZ,  RETW.N,
$12 L:  END-CODE

CODE trc_tid_isTxAmpduOperational   \ .wifi0iram.20 $0
  $0 ORIGIN
  A1 32 ENTRY,  A3 A3 0 8 EXTUI,  A2 A2 A3 ADD.N,  A8 A2 17 L8UI,  A3 0 MOVI.N,
  A8 A8 -2 ADDI,  A2 1 MOVI.N,  A2 A3 A8 MOVNEZ,  RETW.N,
$17 L:  END-CODE

CODE rc_get_mask   \ .text.rc_get_mask $4
  $0 1 LITERALS
    SYMBOL trc_ctl
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A8 20 L32I.N,  RETW.N,
$E L:  END-CODE

CODE rc_disable_trc   \ .text.rc_disable_trc $14
  $0 5 LITERALS
    SYMBOL trc_ctl  SYMBOL bss.g_per_conn_trc  SYMBOL rodata_wlog_warning.23
    SYMBOL memcmp  SYMBOL wifi_log
$14 L:
  A1 32 ENTRY,  A6 $0 L L32R,  A2 A2 0 8 EXTUI,  A2 A2 A6 ADDX4,  A11 A3 MOV.N,
  A7 A4 0 8 EXTUI,  A5 A2 4 L32I.N,  A5 A4 $5A L BBC,
  A12 6 MOVI.N,  A8 $4 L L32R,  A8 A7 A8 ADDX4,  A10 A8 0 L32I.N,  A10 A10 31 ADDI,
  A8 $C L L32R,  A8 CALLX8,
  A10 $5A L BNEZ.N,
  A8 -2 MOVI.N,  A7 SSL,  A8 A8 A8 SRC,  A5 A5 A8 AND,  A5 A2 4 S32I.N,
  A2 A6 20 L32I.N,  A2 A2 A8 AND,  A2 A6 20 S32I.N,  MEMW,  $6C L J,
  1 ZEROS,
$5A L:
  A11 1 MOVI.N,  A13 $8 L L32R,  A12 2 MOVI.N,  A11 A11 11 SLLI,  A10 6 MOVI.N,
  A8 $10 L L32R,  A8 CALLX8,
$6C L:
  RETW.N,
$6E L:  END-CODE

CODE rc_disable_trc_by_interface   \ .text.rc_disable_trc_by_interface $4
  $0 1 LITERALS
    SYMBOL trc_ctl
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A2 0 8 EXTUI,  A2 A2 A8 ADDX4,  A9 A8 20 L32I.N,
  A10 A2 4 L32I.N,  A9 A9 A10 XOR,  A9 A8 20 S32I.N,  A8 0 MOVI.N,  A8 A2 4 S32I.N,
  MEMW,  RETW.N,
$22 L:  END-CODE

CODE rc_get_sta_trc   \ .text.rc_get_sta_trc $8
  $0 2 LITERALS
    SYMBOL trc_ctl  SYMBOL bss.g_per_conn_trc
$8 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A2 0 8 EXTUI,  A2 A2 A8 ADDX4,  A9 A2 4 L32I.N,
  A2 A9 MOV.N,  A9 $36 L BEQZ.N,
  A8 A9 -1 ADDI.N,  A2 0 MOVI.N,  A8 A9 $36 L BANY,
  A8 A9 NEG,  A8 A8 A9 AND,  A2 $4 L L32R,  A8 A8 NSAU,  A8 A8 NEG,  A8 A8 A2 ADDX4,
  A2 A8 124 L32I,
$36 L:
  RETW.N,
$38 L:  END-CODE

CODE rc_get_trc   \ .wifirxiram.24 $C
  $0 3 LITERALS
    SYMBOL trc_ctl  SYMBOL bss.g_per_conn_trc  SYMBOL memcmp
$C L:
  A1 48 ENTRY,  A4 $0 L L32R,  A2 A2 0 8 EXTUI,  A2 A2 A4 ADDX4,  A6 6 MOVI.N,
  A7 1 MOVI.N,  A5 A2 4 L32I.N,  A9 -1 MOVI.N,  $5D L J,
  1 ZEROS,
$24 L:
  A8 A5 NEG,  A8 A8 A5 AND,  A8 A8 NSAU,  A8 A8 NEG,  A4 A8 31 ADDI,  A8 $4 L L32R,
  A12 A6 MOV.N,  A2 A4 A8 ADDX4,  A11 A3 MOV.N,  A2 A2 0 L32I.N,  A10 A2 31 ADDI,
  A9 A1 0 S32I.N,  MEMW,  A8 $8 L L32R,  A8 CALLX8,
  A9 A1 0 L32I.N,  A10 $62 L BEQZ.N,
  A4 SSL,  A8 A7 SLL,  A8 A9 A8 XOR,  A5 A5 A8 AND,
$5D L:
  A5 $24 L BNEZ,
  A2 A5 MOV.N,
$62 L:
  RETW.N,
$64 L:  END-CODE

CODE rc_get_trc_by_index   \ .text.rc_get_trc_by_index $8
  $0 2 LITERALS
    SYMBOL trc_ctl  SYMBOL bss.g_per_conn_trc
$8 L:
  A1 32 ENTRY,  A8 A2 0 8 EXTUI,  A3 A3 0 8 EXTUI,  A8 $28 L BNEZ.N,
  A2 $0 L L32R,  A8 A2 20 L32I.N,  A2 $4 L L32R,  A8 A3 $21 L BBC,
  $54 L J,
$21 L:
  A2 A2 76 L32I,  $60 L J,
  1 ZEROS,
$28 L:
  A8 1 $42 L BNEI,
  A2 $4 L L32R,  A3 $3C L BEQZ.N,
  A8 $0 L L32R,  A8 A8 20 L32I.N,  A8 A3 $3C L BBC,
  $54 L J,
  1 ZEROS,
$3C L:
  A2 A2 80 L32I,  $60 L J,
$42 L:
  A2 0 MOVI.N,  A8 2 $60 L BNEI,
  A2 $4 L L32R,  A3 $5D L BEQZ.N,
  A8 $0 L L32R,  A8 A8 20 L32I.N,  A8 A3 $5D L BBC,
$54 L:
  A3 A3 A2 ADDX4,  A2 A3 0 L32I.N,  $60 L J,
  1 ZEROS,
$5D L:
  A2 A2 84 L32I,
$60 L:
  RETW.N,
$62 L:  END-CODE

CODE rc_get_trc_default   \ .text.rc_get_trc_default $4
  $0 1 LITERALS
    SYMBOL bss.g_per_conn_trc
$4 L:
  A1 32 ENTRY,  A8 A2 0 8 EXTUI,  A8 $15 L BNEZ.N,
  A2 $0 L L32R,  A2 A2 76 L32I,  $2C L J,
$15 L:
  A8 1 $21 L BNEI,
  A2 $0 L L32R,  A2 A2 80 L32I,  $2C L J,
$21 L:
  A2 0 MOVI.N,  A8 2 $2C L BNEI,
  A2 $0 L L32R,  A2 A2 84 L32I,
$2C L:
  RETW.N,
$2E L:  END-CODE

CODE rc_only_sta_trc   \ .text.rc_only_sta_trc $4
  $0 1 LITERALS
    SYMBOL trc_ctl
$4 L:
  A1 32 ENTRY,  A11 $0 L L32R,  A2 1 MOVI.N,  A9 0 MOVI.N,  A10 A11 20 L32I.N,
  A8 A10 -1 ADDI.N,  A8 A8 A10 AND,  A9 A2 A8 MOVNEZ,  A8 A9 0 8 EXTUI,
  A9 $2A L BNEZ.N,
  A8 A2 A10 MOVEQZ,  A8 $2A L BNEZ.N,
  A9 A11 4 L32I.N,  A2 A8 A9 MOVNEZ,  $2C L J,
$2A L:
  A2 -1 MOVI.N,
$2C L:
  RETW.N,
$2E L:  END-CODE

CODE trc_init   \ .text.trc_init $1C
  $0 7 LITERALS
    SYMBOL g_osi_funcs_p  SYMBOL bss.g_per_conn_trc  SYMBOL rodata_wlog_error.25
    $24 SYMBOL+ rc11BSchedTbl  SYMBOL rc11BSchedTbl  $54 SYMBOL+ rcP2P11GSchedTbl
    SYMBOL wifi_log
$1C L:
  A1 32 ENTRY,  A3 $0 L L32R,  A10 136 MOVI,  A2 A3 0 L32I.N,  A2 A2 $178 L32I,
  A2 CALLX8,
  A8 A3 0 L32I.N,  A2 $4 L L32R,  A8 A8 $178 L32I,  A10 A2 76 S32I,  A10 136 MOVI,
  MEMW,  A8 CALLX8,
  A3 A3 0 L32I.N,  A3 A3 $178 L32I,  A10 A2 80 S32I,  A10 136 MOVI,  MEMW,
  A3 CALLX8,
  A9 A2 76 L32I,  A10 A2 84 S32I,  A9 $71 L BEQZ.N,
  A15 1 MOVI.N,  A11 0 MOVI,  A11 A15 A10 MOVEQZ,  A8 A2 80 L32I,  A2 A11 0 8 EXTUI,
  A11 $71 L BNEZ,
  A2 A15 A8 MOVEQZ,  A2 $8C L BEQZ,
$71 L:
  A13 $8 L L32R,  A12 1 MOVI.N,  A11 $200 MOVI,  A10 6 MOVI.N,  MEMW,
  A8 $18 L L32R,  A8 CALLX8,
  A2 $101 MOVI,  $EE L J,
  2 ZEROS,
$8C L:
  A11 $C L L32R,  A12 -128 MOVI,  A3 2 MOVI.N,  A14 $10 L L32R,  A13 $14 L L32R,
  A11 A9 96 S32I,  A14 A9 112 S32I,  A11 A9 100 S32I,  A11 A9 104 S32I,
  A13 A9 108 S32I,  A12 A9 11 S8I,  A2 A9 38 S8I,  A2 A9 131 S8I,  A2 A9 129 S8I,
  A11 A8 96 S32I,  A14 A8 112 S32I,  A11 A8 100 S32I,  A11 A8 104 S32I,
  A13 A8 108 S32I,  A12 A8 11 S8I,  A2 A8 38 S8I,  A2 A8 131 S8I,  A15 A8 129 S8I,
  A11 A10 96 S32I,  A14 A10 112 S32I,  A11 A10 100 S32I,  A11 A10 104 S32I,
  A13 A10 108 S32I,  A12 A10 11 S8I,  A2 A10 38 S8I,  A2 A10 131 S8I,
  A3 A10 129 S8I,  MEMW,
$EE L:
  MEMW,  RETW.N,
$F3 L:  END-CODE

CODE trc_update_def_rate   \ .text.trc_update_def_rate $1C
  $0 7 LITERALS
    SYMBOL bss.g_per_conn_trc  $54 SYMBOL+ rcP2P11GSchedTbl
    SYMBOL rcP2P11GSchedTbl  $24 SYMBOL+ rc11BSchedTbl  SYMBOL rc11BSchedTbl
    SYMBOL rodata_wlog_warning.26  SYMBOL wifi_log
$1C L:
  A1 32 ENTRY,  A10 $0 L L32R,  A4 A4 0 8 EXTUI,  A9 A10 76 L32I,  A8 A10 80 L32I,
  A10 A10 84 L32I,  A3 $7C L BNEZ,
  A4 $58 L BEQZ.N,
  A3 $4 L L32R,  A4 $8 L L32R,  A3 A9 96 S32I,  A3 A9 100 S32I,  A3 A9 104 S32I,
  A3 64 MOVI.N,  A3 A9 11 S8I,  A3 1 MOVI.N,  A4 A9 112 S32I,  A3 A9 130 S8I,  MEMW,
  MEMW,  $119 L J,
$58 L:
  A3 $C L L32R,  A8 $10 L L32R,  A3 A9 96 S32I,  A3 A9 100 S32I,  A3 A9 104 S32I,
  A3 -128 MOVI,  A8 A9 112 S32I,  A3 A9 11 S8I,  A4 A9 130 S8I,  MEMW,  MEMW,
  $119 L J,
$7C L:
  A3 1 $C8 L BNEI,
  A4 $A4 L BEQZ.N,
  A4 $4 L L32R,  A9 $8 L L32R,  A4 A8 96 S32I,  A4 A8 100 S32I,  A4 A8 104 S32I,
  A4 64 MOVI.N,  A9 A8 112 S32I,  A4 A8 11 S8I,  A3 A8 130 S8I,  MEMW,  MEMW,
  $119 L J,
$A4 L:
  A3 $C L L32R,  A9 $10 L L32R,  A3 A8 96 S32I,  A3 A8 100 S32I,  A3 A8 104 S32I,
  A3 -128 MOVI,  A9 A8 112 S32I,  A3 A8 11 S8I,  A4 A8 130 S8I,  MEMW,  MEMW,
  $119 L J,
$C8 L:
  A3 2 $109 L BNEI,
  A4 $E8 L BEQZ.N,
  A3 $4 L L32R,  A4 $8 L L32R,  A3 A10 96 S32I,  A3 A10 100 S32I,  A3 A10 104 S32I,
  A3 64 MOVI.N,  A4 A10 112 S32I,  MEMW,  $FD L J,
  1 ZEROS,
$E8 L:
  A3 $C L L32R,  A4 $10 L L32R,  A3 A10 96 S32I,  A3 A10 100 S32I,  A3 A10 104 S32I,
  A3 -128 MOVI,  A4 A10 112 S32I,
$FD L:
  A3 A10 11 S8I,  MEMW,  MEMW,  $119 L J,
$109 L:
  A13 $14 L L32R,  A12 2 MOVI.N,  A11 $200 MOVI,  A10 6 MOVI.N,  A8 $18 L L32R,
  A8 CALLX8,
$119 L:
  RETW.N,
$11B L:  END-CODE

CODE trc_update_ifx_phy_mode   \ .text.trc_update_ifx_phy_mode $20
  $0 8 LITERALS
    $54 SYMBOL+ rcP2P11GSchedTbl  $24 SYMBOL+ rc11BSchedTbl
    SYMBOL bss.g_per_conn_trc  SYMBOL g_ic  SYMBOL rodata_wlog_error.27
    $C SYMBOL+ rcLoRaSchedTbl  SYMBOL rcLoRaSchedTbl  SYMBOL wifi_log
$20 L:
  A1 32 ENTRY,  A2 $3C L BNEZ.N,
  A2 $8 L L32R,  A8 A2 76 L32I,  A2 $C L L32R,  A2 A2 $100 ADDMI,  A10 A2 216 L8UI,
  A10 A10 4 1 EXTUI,  $7A L J,
  2 ZEROS,
$3C L:
  A2 1 $54 L BNEI,
  A2 $8 L L32R,  A8 A2 80 L32I,  A2 $C L L32R,  A2 A2 $100 ADDMI,  A10 A2 216 L8UI,
  A10 A10 5 1 EXTUI,  $7A L J,
$54 L:
  A2 2 $64 L BNEI,
  A2 $8 L L32R,  A8 A2 84 L32I,  A3 6 $7D L BEQI,
  $9A L J,
  1 ZEROS,
$64 L:
  A13 $10 L L32R,  A12 1 MOVI.N,  A11 $200 MOVI,  A10 6 MOVI.N,  A8 $1C L L32R,
  A8 CALLX8,
  A2 $102 MOVI,  $AB L J,
$7A L:
  A3 6 $95 L BNEI,
$7D L:
  A2 $14 L L32R,  A9 $18 L L32R,  A2 A8 96 S32I,  A9 A8 112 S32I,  A9 A8 100 S32I,
  A2 A8 104 S32I,  MEMW,  $A9 L J,
$95 L:
  A9 $0 L L32R,  A10 $9D L BNEZ.N,
$9A L:
  A9 $4 L L32R,
$9D L:
  A9 A8 96 S32I,  A9 A8 112 S32I,  A9 A8 100 S32I,  A9 A8 104 S32I,
$A9 L:
  A2 0 MOVI.N,
$AB L:
  MEMW,  RETW.N,
$B0 L:  END-CODE

CODE trc_deinit   \ .text.trc_deinit $8
  $0 2 LITERALS
    SYMBOL bss.g_per_conn_trc  SYMBOL g_osi_funcs_p
$8 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A3 0 MOVI,  A4 $4 L L32R,
$14 L:
  A10 A2 0 L32I.N,  A10 $24 L BEQZ.N,
  A8 A4 0 L32I.N,  A8 A8 176 L32I,  A8 CALLX8,
  A8 0 MOVI.N,  A8 A2 0 S32I.N,
$24 L:
  A3 A3 1 ADDI.N,  A8 22 MOVI.N,  A2 A2 4 ADDI.N,  A3 A8 $14 L BNE,
  A2 0 MOVI.N,  MEMW,  RETW.N,
$34 L:  END-CODE

CODE rc_set_per_conn_fix_rate   \ .text.rc_set_per_conn_fix_rate $C
  $0 3 LITERALS
    SYMBOL bss.g_per_conn_trc  SYMBOL s_fix_rate_mask  SYMBOL s_fix_rate
$C L:
  A1 32 ENTRY,  A10 A2 0 8 EXTUI,  A8 21 MOVI.N,  A2 $102 MOVI,  A8 A10 $7D L BLTU,
  A8 $0 L L32R,  A8 A10 A8 ADDX4,  A9 A8 0 L32I.N,  A9 $7D L BEQZ,
  A11 0 MOVI.N,  A12 A10 -19 ADDI,  A8 1 MOVI.N,  A2 A11 MOV.N,  A2 A8 A12 MOVEQZ,
  A12 A11 MOV.N,  A12 A8 A10 MOVEQZ,  A2 A2 A12 OR,  A8 A8 A2 XOR,  A2 $4 L L32R,
  A2 A2 0 L32I.N,  A8 SSR,  A2 A2 SRL,  A2 A2 0 1 EXTUI,  A10 A9 11 L8UI,
  A2 A11 $72 L BEQ,
  A2 $8 L L32R,  A8 A8 A2 ADDX4,  A2 A8 0 L8UI,  A2 A9 8 S8I,  A2 A9 9 S8I,
  A2 3 MOVI.N,  A10 A10 A2 OR,  A2 A11 MOV.N,  A10 A9 11 S8I,  MEMW,  MEMW,
  $7D L J,
$72 L:
  A8 -4 MOVI.N,  A10 A10 A8 AND,  A10 A9 11 S8I,  MEMW,
$7D L:
  MEMW,  RETW.N,
$82 L:  END-CODE

CODE rc_enable_trc   \ .text.rc_enable_trc $2C
  $0 11 LITERALS
    SYMBOL bss.g_per_conn_trc  SYMBOL g_osi_funcs_p  SYMBOL rodata_wlog_error.21
    SYMBOL trc_ctl  SYMBOL rodata_wlog_warning.22  SYMBOL wifi_log  SYMBOL memcmp
    SYMBOL wifi_log  SYMBOL memset  SYMBOL memcpy  SYMBOL rc_set_per_conn_fix_rate
$2C L:
  A1 32 ENTRY,  A7 $0 L L32R,  A4 A4 0 8 EXTUI,  A7 A4 A7 ADDX4,  A6 A2 0 8 EXTUI,
  A5 A5 0 8 EXTUI,  A2 A7 0 L32I,  A2 $6F L BNEZ.N,
  A10 136 MOVI,  A2 $4 L L32R,  A2 A2 0 L32I,  A2 A2 $178 L32I,  A2 CALLX8,
  A2 A10 MOV.N,  A10 A7 0 S32I.N,  A10 $B5 L BNEZ,
  A13 $8 L L32R,  A12 1 MOVI.N,  A11 $200 MOVI,  A10 6 MOVI.N,  MEMW,
  A8 $14 L L32R,  A8 CALLX8,
  $FA L J,
$6F L:
  A8 $C L L32R,  A7 A8 20 L32I.N,  A7 A4 $A8 L BBC,
  A8 A6 A8 ADDX4,  A7 A8 4 L32I.N,  A7 A4 $90 L BBC,
  A12 6 MOVI,  A11 A3 A3 OR,  A10 A2 31 ADDI,  A8 $18 L L32R,  A8 CALLX8,
  A10 $A8 L BEQZ.N,
$90 L:
  A13 $10 L L32R,  A11 1 MOVI.N,  A12 2 MOVI.N,  A11 A11 11 SLLI,  A10 6 MOVI.N,
  A8 $1C L L32R,  A8 CALLX8,
  A2 0 MOVI.N,  $FA L J,
  1 ZEROS,
$A8 L:
  A12 136 MOVI,  A11 0 MOVI.N,  A10 A2 MOV.N,  A8 $20 L L32R,  A8 CALLX8,
$B5 L:
  A8 $C L L32R,  A9 1 MOVI.N,  A4 SSL,  A9 A9 SLL,  A12 6 MOVI.N,  A11 A3 MOV.N,
  A7 A8 20 L32I.N,  A10 A2 31 ADDI,  A7 A7 A9 OR,  A7 A8 20 S32I.N,  A8 A6 A8 ADDX4,
  A7 A8 4 L32I.N,  A7 A7 A9 OR,  A5 A2 37 S8I,  A5 0 MOVI.N,  A7 A8 4 S32I.N,
  A4 A2 128 S8I,  A6 A2 129 S8I,  A5 A2 38 S8I,  MEMW,  MEMW,  A8 $24 L L32R,
  A8 CALLX8,
  A10 A4 MOV.N,  A8 $28 L L32R,  A8 CALLX8,
$FA L:
  RETW.N,
$FC L:  END-CODE

CODE rc_get_fix_rate   \ .text.rc_get_fix_rate $8
  $0 2 LITERALS
    SYMBOL s_fix_rate  SYMBOL s_fix_rate_mask
$8 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A8 A2 A8 ADDX4,  A8 A8 0 L32I.N,  A8 A4 0 S32I.N,
  A8 $4 L L32R,  A8 A8 0 L32I.N,  A2 SSR,  A8 A8 SRL,  A8 A8 0 1 EXTUI,
  A2 0 MOVI.N,  A8 A3 0 S8I,  MEMW,  MEMW,  RETW.N,
$30 L:  END-CODE

CODE rc_set_fix_rate   \ .text.rc_set_fix_rate $1C
  $0 7 LITERALS
    SYMBOL s_fix_rate_mask  SYMBOL s_fix_rate  SYMBOL rc_set_per_conn_fix_rate
    SYMBOL rc_set_per_conn_fix_rate  SYMBOL rc_set_per_conn_fix_rate
    SYMBOL rc_set_per_conn_fix_rate  SYMBOL rc_set_per_conn_fix_rate
$1C L:
  A1 32 ENTRY,  A11 $0 L L32R,  A8 1 MOVI.N,  A3 A3 0 8 EXTUI,  A10 A2 MOV.N,
  A2 SSL,  A8 A8 SLL,  A2 A11 0 L32I.N,  A3 $39 L BEQZ.N,
  A2 A8 A2 OR,  $41 L J,
$39 L:
  A9 -1 MOVI.N,  A8 A9 A8 XOR,  A2 A8 A2 AND,
$41 L:
  A2 A11 0 S32I.N,  A2 $4 L L32R,  A2 A10 A2 ADDX4,  A4 A2 0 S32I.N,
  A10 $5D L BNEZ.N,
  MEMW,  A8 $8 L L32R,  A8 CALLX8,
  A10 19 MOVI.N,  $8E L J,
  2 ZEROS,
$5D L:
  A10 1 $79 L BNEI,
  A2 A10 A10 OR,  A3 17 MOVI,
$66 L:
  A10 A2 MOV.N,  A2 A2 1 ADDI.N,  A8 $C L L32R,  A8 CALLX8,
  A2 A3 $66 L BNE,
  A10 20 MOVI.N,  $8E L J,
  1 ZEROS,
$79 L:
  A10 2 $94 L BNEI,
  A10 17 MOVI.N,  A8 $10 L L32R,  A8 CALLX8,
  A10 18 MOVI.N,  A8 $14 L L32R,  A8 CALLX8,
  A10 21 MOVI.N,
$8E L:
  A8 $18 L L32R,  A8 CALLX8,
$94 L:
  A2 0 MOVI.N,  RETW.N,
$98 L:  END-CODE

CODE trc_get_espnow_rate   \ .text.trc_get_espnow_rate $4
  $0 1 LITERALS
    SYMBOL bss.g_espnow_rate
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A2 0 8 EXTUI,  A2 A2 A8 ADDX4,  A2 A2 0 L32I.N,
  RETW.N,
$14 L:  END-CODE

CODE trc_set_espnow_rate   \ .text.trc_set_espnow_rate $4
  $0 1 LITERALS
    SYMBOL bss.g_espnow_rate
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A2 0 8 EXTUI,  A2 A2 A8 ADDX4,  A3 A2 0 S32I.N,
  MEMW,  RETW.N,
$17 L:  END-CODE

CODE trc_get_80211_tx_rate   \ .text.trc_get_80211_tx_rate $4
  $0 1 LITERALS
    SYMBOL bss.g_80211_tx_rate
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A2 0 8 EXTUI,  A2 A2 A8 ADDX4,  A2 A2 0 L32I.N,
  RETW.N,
$14 L:  END-CODE

CODE trc_get_80211_tx_rate_config   \ .text.trc_get_80211_tx_rate_config $8
  $0 2 LITERALS
    SYMBOL bss.g_80211_tx_rate_config  SYMBOL memcpy
$8 L:
  A1 32 ENTRY,  A11 $0 L L32R,  A2 A2 0 8 EXTUI,  A2 A2 A2 ADDX2,  A12 12 MOVI.N,
  A11 A2 A11 ADDX4,  A10 A3 MOV.N,  A8 $4 L L32R,  A8 CALLX8,
  RETW.N,
$23 L:  END-CODE

CODE trc_set_80211_tx_rate   \ .text.trc_set_80211_tx_rate $4
  $0 1 LITERALS
    SYMBOL bss.g_80211_tx_rate
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A2 0 8 EXTUI,  A2 A2 A8 ADDX4,  A3 A2 0 S32I.N,
  MEMW,  RETW.N,
$17 L:  END-CODE

CODE trc_set_80211_tx_rate_config   \ .text.trc_set_80211_tx_rate_config $8
  $0 2 LITERALS
    SYMBOL bss.g_80211_tx_rate_config  SYMBOL memcpy
$8 L:
  A1 32 ENTRY,  A10 $0 L L32R,  A2 A2 0 8 EXTUI,  A2 A2 A2 ADDX2,  A12 12 MOVI.N,
  A11 A3 MOV.N,  A10 A2 A10 ADDX4,  A8 $4 L L32R,  A8 CALLX8,
  RETW.N,
$23 L:  END-CODE

LINK-SYMBOLS   \ the cells of other packages that wait for these names
END-PACKAGE  PREVIOUS   \ not in the search order: ALSO name gives its words (README.md)
