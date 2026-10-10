\ esf_buf.fs - package PP-ESF-BUF: esf_buf.o of libpp.a (ESP32)
\
\ Made from esf_buf.S by s2fs.py, not by hand. Each function of C is a
\ CODE word: its bytes are those of the compiler, at the same offsets of
\ its section (L gives the address of an offset); its literals are at the
\ start of the word, after a jump. The data come first, then the
\ functions, each after those that it calls. The global symbols are
\ PUBLIC, the static ones PRIVATE. lib/esp32/clink.fs explains the words.
\
\ 12 functions (12 global), 19 data objects.
\ A function runs as C code with the windowed calls of the Xtensa: call
\ it with CALL-C ( x1 .. xn n xt -- x ), never by its name alone.
\
\ Names of other packages: a literal waits until its package comes
\ (LINK-SYMBOLS); a name outside the libraries until EXTERN gives it.
\ Uses:
\   PP-PP-DEBUG  g_lmac_cnt
\ Outside the libraries (../README.md):
\   config_is_cache_tx_buf_enabled g_osi_funcs_p g_wifi_global_lock
\   g_wifi_menuconfig memcpy memset wifi_log
\ C names with more than 31 characters (Forth takes 31): the first 26,
\ ~ and 4 hex digits of their CRC-32 (../README.md):
\   esf_buf_alloc_dynamic_defa~A225 = esf_buf_alloc_dynamic_default_handler
\   rodata.esf_buf_statis_dump~CF0A = rodata.esf_buf_statis_dump.str1.1
\
\ Needs XTENSA (asm/xtensa.fs), ASM (lib/esp32/asm.fs) and CLINK
\ (lib/esp32/clink.fs) of ucf; the ESP32.

PACKAGE PP-ESF-BUF  ALSO ASM  ALSO CLINK   \ lib/package.fs
EXPORTS   \ the PUBLIC words are symbols for the other packages

\ ==== Data =============================================================

PRIVATE
CREATE rodata   \ .rodata, 72 bytes
  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,   \ 9 jumps into esf_buf_alloc
  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,   \ 9 jumps into esf_buf_recycle
CREATE rodata.esf_buf_statis_dump~CF0A   \ .rodata.esf_buf_statis_dump.str1.1, 52 bytes
  ," max"  0 C,
  ," t"  0 C,
  ," fail_oom"  0 C,
  ," fail_fc"  0 C,
  ," total_free"  0 C,
  ," total_alloc"  0 C,
  ," alloc"  0 C,
CREATE rodata_wlog_info.19   \ .rodata_wlog_info.19, 38 bytes
  ,"  %-2d %-3u %-5u %-11u %-10u %-8u %-8u"  0 C,
CREATE rodata_wlog_info.18   \ .rodata_wlog_info.18, 38 bytes
  ,"  %-2s %-3s %-5s %-11s %-10s %-8s %-8s"  0 C,
CREATE rodata_wlog_info.17   \ .rodata_wlog_info.17, 9 bytes
  ,"  BUFFER:"  0 C,
CREATE rodata_wlog_info.16   \ .rodata_wlog_info.16, 29 bytes
  ," Init tx cache buffer num: %d"  0 C,
CREATE rodata_wlog_info.15   \ .rodata_wlog_info.15, 31 bytes
  ," Init dynamic tx buffer num: %d"  0 C,
CREATE rodata_wlog_info.14   \ .rodata_wlog_info.14, 30 bytes
  ," Init static tx buffer num: %d"  0 C,
CREATE rodata_wlog_info.13   \ .rodata_wlog_info.13, 37 bytes
  ," Init management short buffer num: %d"  0 C,
CREATE rodata_wlog_info.12   \ .rodata_wlog_info.12, 36 bytes
  ," Init dynamic rx mgmt buffer num: %d"  0 C,
CREATE rodata_wlog_info.11   \ .rodata_wlog_info.11, 35 bytes
  ," Init static rx mgmt buffer num: %d"  0 C,
CREATE rodata_wlog_info.10   \ .rodata_wlog_info.10, 42 bytes
  ," Init data frame dynamic rx buffer num: %d"  0 C,
CREATE rodata_wlog_warning.9   \ .rodata_wlog_warning.9, 40 bytes
  ," esf_buf_setup_static: alloc eb fail(%d)"  0 C,
CREATE rodata_wlog_error.7   \ .rodata_wlog_error.7, 27 bytes
  ," ebuf_free: invalid type=%d"  0 C,
CREATE rodata_wlog_error.5   \ .rodata_wlog_error.5, 31 bytes
  ," esf_buf_alloc: invalid type=%d"  0 C,
CREATE rodata_wlog_error.3   \ .rodata_wlog_error.3, 39 bytes
  ," esf_buf_alloc_dynamic: invalid type=%d"  0 C,
PUBLIC
200 DATA: g_eb_list_desc   \ .data.g_eb_list_desc
  $00  $00  $00  $00  $00  $00  $00  $00  $00  $20  $00  $00  $01  $00  $00  $00  16 g_eb_list_desc $10 + INIT
  $24  $00  $00  $00  $20  $00  $00  $00  $00  $00  $00  $00  $00  $00  $00  $00  16 g_eb_list_desc $20 + INIT
  $02  $00  $00  $00  $00  $00  $00  $00  $20  $00  $00  $00  $00  $00  $00  $00  16 g_eb_list_desc $30 + INIT
  $00  $00  $00  $00  $03  $00  $00  $00  $00  $00  $00  $00  $20  $00  $00  $00  16 g_eb_list_desc $40 + INIT
  $00  $00  $00  $00  $00  $00  $00  $00  $04  $00  $00  $00  $00  $00  $00  $00  16 g_eb_list_desc $50 + INIT
  $20  $00  $00  $00  $00  $00  $00  $00  $00  $00  $20  $00  $05  $00  $04  $00  16 g_eb_list_desc $60 + INIT
  $06  $00  $00  $00  $00  $00  $00  $00  $00  $00  $00  $00  $00  $00  $00  $00  16 g_eb_list_desc $80 + INIT
  $00  $00  $00  $00  $07  $00  $00  $00  $00  $00  $00  $00  $40  $00  $00  $00  16 g_eb_list_desc $90 + INIT
  $00  $00  $00  $00  $00  $00  $00  $00  $08  $00  $00  $00  $00  $00  $00  $00  16 g_eb_list_desc $A0 + INIT
  $0A  $00  $00  $00  $00  $00  $00  $00  $00  $20  $00  $00  $09  $00  $00  $00  16 g_eb_list_desc $B0 + INIT
  $24  $00  $00  $00  $00  $00  $00  $00  8 g_eb_list_desc $C0 + INIT
192 DATA: eb_space   \ .bss.eb_space
128 DATA: eb_txdesc_space   \ .dram1.2

\ ==== Code =============================================================

CODE esf_buf_alloc_dynamic_defa~A225   \ esf_buf_alloc_dynamic_default_handler: .text.esf_buf_alloc_dynamic_default_handler $8
  $0 2 LITERALS
    SYMBOL rodata_wlog_error.3  SYMBOL wifi_log
$8 L:
  A1 32 ENTRY,  A13 $0 L L32R,  A14 A2 MOV.N,  A12 1 MOVI.N,  A11 $400 MOVI,
  A10 6 MOVI.N,  A8 $4 L L32R,  A8 CALLX8,
  A2 0 MOVI.N,  RETW.N,
$21 L:  END-CODE

CODE esf_buf_alloc_dynamic   \ .wifiextrairam.4 $24
  $0 9 LITERALS
    SYMBOL g_wifi_global_lock  SYMBOL g_osi_funcs_p  SYMBOL g_lmac_cnt  $FFB T,
    $DEADBEEF T,  SYMBOL esf_buf_alloc_dynamic_defa~A225  SYMBOL memset
    SYMBOL memset  SYMBOL memcpy
$24 L:
  A1 48 ENTRY,  A5 A2 16 L16UI,  A6 A2 8 L8UI,  A7 A2 12 L8UI,  A5 $94 L BEQZ,
  A5 $4 L L32R,  A9 $0 L L32R,  A8 A5 0 L32I.N,  A10 A9 0 L32I.N,  A8 A8 84 L32I,
  A9 A1 4 S32I.N,  MEMW,  A8 CALLX8,
  A8 A2 14 L16UI,  A12 A2 16 L16UI,  A10 A5 0 L32I.N,  A5 A6 A6 ADD.N,
  A11 $8 L L32R,  A9 A1 4 L32I.N,  A8 A12 $76 L BLTU,
  A5 A5 A6 ADD.N,  A5 A5 A11 ADDX4,  A2 A5 10 L16UI,  A2 A2 1 ADDI.N,
  A2 A5 10 S16I,  MEMW,  A2 A10 88 L32I,  A10 A9 0 L32I.N,  MEMW,  $1DC L J,
  1 ZEROS,
$76 L:
  A5 A5 A6 ADD.N,  A5 A5 A11 ADDX4,  A8 A8 1 ADDI.N,  A8 A2 14 S16I,  MEMW,
  A8 A5 0 L32I.N,  A8 A8 1 ADDI.N,  A8 A5 0 S32I.N,  A5 A10 88 L32I,
  A10 A9 0 L32I.N,  MEMW,  A5 CALLX8,
$94 L:
  A6 5 $9C L BGEUI,
  A6 $CD L BEQZ.N,
  $AA L J,
$9C L:
  A5 A6 -7 ADDI,  A5 A5 0 8 EXTUI,  A5 3 $B8 L BLTUI,
  $CD L J,
  2 ZEROS,
$AA L:
  A5 $4 L L32R,  A10 A3 96 ADDI,  A5 A5 0 L32I.N,  A5 A5 $15C L32I,  $C3 L J,
$B8 L:
  A5 $4 L L32R,  A10 A3 96 ADDI,  A5 A5 0 L32I.N,  A5 A5 $16C L32I,
$C3 L:
  A10 A10 A7 ADD.N,  A5 CALLX8,
  $D5 L J,
  2 ZEROS,
$CD L:
  A10 A6 MOV.N,  A8 $14 L L32R,  A8 CALLX8,
$D5 L:
  A5 A10 MOV.N,  A10 $190 L BEQZ,
  A12 92 MOVI.N,  A11 0 MOVI.N,  A8 $18 L L32R,  A8 CALLX8,
  A8 A5 60 ADDI,  A13 A5 48 ADDI,  A8 A5 44 S32I.N,  A8 A5 92 ADDI,  A12 A7 MOV.N,
  A11 0 MOVI.N,  A10 A8 MOV.N,  A13 A5 4 S32I.N,  A8 A5 16 S32I.N,  A13 A1 0 S32I.N,
  A5 A1 4 S32I.N,  MEMW,  A8 $1C L L32R,  A8 CALLX8,
  A8 A10 MOV.N,  A10 1 MOVI.N,  A11 A3 0 16 EXTUI,  A2 A2 4 L32I.N,
  A13 A1 0 L32I.N,  A2 A5 60 S32I.N,  A2 A5 64 L32I,  A10 A5 12 S16I,  MEMW,
  A10 15 MOVI.N,  A2 A2 A10 OR,  A10 A8 A7 ADD.N,  A12 $C L L32R,  A2 A5 64 S32I,
  A2 A7 A3 ADD.N,  A13 A5 8 S32I.N,  A10 A5 52 S32I.N,  A9 A1 4 L32I.N,
  A12 A2 $14D L BGEU,
  A7 -1 MOVI.N,  A7 A7 12 SLLI,  MEMW,  A2 A5 48 L32I.N,  A2 A2 A7 AND,
  A7 $3FF MOVI,  A7 A7 2 SLLI,  MEMW,  $166 L J,
$14D L:
  A8 A8 A2 ADD.N,  A2 $10 L L32R,  A7 A11 A7 ADD.N,  A7 A7 0 12 EXTUI,
  A2 A8 0 S32I.N,  A8 -1 MOVI.N,  A8 A8 12 SLLI,  MEMW,  A2 A5 48 L32I.N,
  A2 A2 A8 AND,
$166 L:
  A2 A2 A7 OR,  MEMW,  A2 A5 48 S32I.N,  A11 A5 22 S16I,  A6 A5 26 S8I,  MEMW,
  A4 $1E4 L BEQZ,
  A12 A3 MOV.N,  A11 A4 MOV.N,  A9 A1 4 S32I.N,  MEMW,  A8 $20 L L32R,  A8 CALLX8,
  A9 A1 4 L32I.N,  $1E4 L J,
  2 ZEROS,
$190 L:
  A3 A2 16 L16UI,  A9 0 MOVI.N,  A3 A9 $1E4 L BEQ,
  A7 $4 L L32R,  A4 $0 L L32R,  A3 A7 0 L32I.N,  A10 A4 0 L32I.N,  A3 A3 84 L32I,
  A3 CALLX8,
  A3 A6 A6 ADD.N,  A5 A2 14 L16UI,  A8 $8 L L32R,  A5 $1C5 L BEQZ.N,
  A5 A5 -1 ADDI.N,  A5 A2 14 S16I,  MEMW,  A2 A3 A6 ADD.N,  A2 A2 A8 ADDX4,
  A5 A2 0 L32I.N,  A5 A5 1 ADDI.N,  A5 A2 0 S32I.N,
$1C5 L:
  A3 A3 A6 ADD.N,  A3 A3 A8 ADDX4,  A2 A3 8 L16UI,  A2 A2 1 ADDI.N,  A2 A3 8 S16I,
  MEMW,  A2 A7 0 L32I.N,  A10 A4 0 L32I.N,  A2 A2 88 L32I,
$1DC L:
  MEMW,  A2 CALLX8,
  A9 0 MOVI.N,
$1E4 L:
  A2 A9 MOV.N,  RETW.N,
$1E8 L:  END-CODE

CODE esf_buf_alloc_default_handler   \ .text.esf_buf_alloc_default_handler $8
  $0 2 LITERALS
    SYMBOL rodata_wlog_error.5  SYMBOL wifi_log
$8 L:
  A1 32 ENTRY,  A13 $0 L L32R,  A14 A2 MOV.N,  A12 1 MOVI.N,  A11 $400 MOVI,
  A10 6 MOVI.N,  A8 $4 L L32R,  A8 CALLX8,
  A2 0 MOVI.N,  RETW.N,
$21 L:  END-CODE

CODE esf_buf_alloc   \ .wifislprxiram.6 $24
  $0 9 LITERALS
    SYMBOL rodata  SYMBOL g_wifi_menuconfig  SYMBOL g_wifi_global_lock
    SYMBOL g_osi_funcs_p  SYMBOL g_eb_list_desc  SYMBOL g_lmac_cnt  SYMBOL memcpy
    SYMBOL esf_buf_alloc_dynamic  SYMBOL esf_buf_alloc_default_handler
$24 L:
  A1 48 ENTRY,  A6 A2 MOV.N,  A5 8 MOVI.N,  A2 A3 -1 ADDI.N,  A5 A2 $33 L BGEU,
  $149 L J,
$33 L:
  A5 $0 L L32R,  A2 A2 A5 ADDX4,  A2 A2 0 L32I.N,  A2 JX,
$3E L:
  A2 $4 L L32R,  A2 A2 8 L32I.N,  $4D L J,
  1 ZEROS,
$47 L:
  A2 $4 L L32R,  A2 A2 84 L32I,
$4D L:
  A2 $131 L BNEZ,
$50 L:
  A5 A3 2 SLLI,  A8 $C L L32R,  A11 $8 L L32R,  A2 A8 0 L32I.N,  A10 A11 0 L32I,
  A2 A2 84 L32I,  A8 A1 4 S32I.N,  A11 A1 0 S32I,  MEMW,  A2 CALLX8,
  A9 A3 A3 ADDX4,  A2 $10 L L32R,  A7 A9 A2 ADDX4,  A8 A1 4 L32I.N,  A2 A7 0 L32I.N,
  A10 A8 0 L32I.N,  A12 $14 L L32R,  A11 A1 0 L32I.N,  A2 $B5 L BEQZ.N,
  A8 A2 40 L32I.N,  A8 A7 0 S32I.N,  A8 0 MOVI.N,  A8 A2 40 S32I.N,  A8 A3 A3 ADDX2,
  A8 A8 A12 ADDX4,  A12 A8 0 L32I.N,  A12 A12 1 ADDI.N,  A12 A8 0 S32I.N,
  A8 A10 88 L32I,  A10 A11 0 L32I.N,  MEMW,  A8 CALLX8,
  A8 A4 0 16 EXTUI,  A10 A2 4 L32I.N,  A8 A2 22 S16I,  MEMW,  A10 $D8 L BNEZ.N,
  MEMW,  $FB L J,
  1 ZEROS,
$B5 L:
  A3 A7 8 L8UI,  A3 A3 A3 ADDX2,  A3 A3 A12 ADDX4,  A4 A3 10 L16UI,  A4 A4 1 ADDI.N,
  A4 A3 10 S16I,  MEMW,  A3 A10 88 L32I,  A10 A11 0 L32I.N,  MEMW,  A3 CALLX8,
  $15C L J,
  1 ZEROS,
$D8 L:
  A11 A7 12 L8UI,  A8 A8 A11 ADD.N,  A8 A8 0 12 EXTUI,  A9 A2 16 L32I.N,
  A9 A9 A11 ADD.N,  A11 -1 MOVI.N,  A11 A11 12 SLLI,  A9 A10 4 S32I.N,  MEMW,
  A9 A10 0 L32I.N,  A9 A9 A11 AND,  A8 A9 A8 OR,  MEMW,  A8 A10 0 S32I.N,
$FB L:
  A6 $10C L BEQZ.N,
  A12 A4 MOV.N,  A11 A6 MOV.N,  A10 A10 4 L32I.N,  MEMW,  A8 $18 L L32R,  A8 CALLX8,
$10C L:
  A5 A5 A3 ADD.N,  A3 $10 L L32R,  A5 A5 A3 ADDX4,  A4 A2 44 L32I.N,
  A5 A5 4 L32I.N,  A3 A4 0 L32I.N,  A3 A3 A5 OR,  A5 15 MOVI.N,  A3 A4 0 S32I.N,
  A3 A4 4 L32I.N,  A3 A3 A5 OR,  A3 A4 4 S32I.N,  MEMW,  $15C L J,
  3 ZEROS,
$131 L:
  A10 $10 L L32R,  A3 A3 A3 ADDX4,  A12 A6 MOV.N,  A11 A4 MOV.N,  A10 A3 A10 ADDX4,
  A8 $1C L L32R,  A8 CALLX8,
  A2 A10 MOV.N,  $15C L J,
$149 L:
  A10 A3 MOV.N,  A8 $20 L L32R,  A8 CALLX8,
  A10 1 $131 L BEQI,
  A10 2 $15A L BNEI,
  $50 L J,
$15A L:
  A2 0 MOVI.N,
$15C L:
  RETW.N,
  rodata  $3E L!+  $131 L!+  $131 L!+  $131 L!+  $50 L!+  $50 L!+  $131 L!+  $47 L!+
  $50 L!+  DROP   \ the table of jumps
$15E L:  END-CODE

CODE esf_buf_recycle_default_handler   \ .text.esf_buf_recycle_default_handler $8
  $0 2 LITERALS
    SYMBOL rodata_wlog_error.7  SYMBOL wifi_log
$8 L:
  A1 32 ENTRY,  A13 $0 L L32R,  A14 A2 0 8 EXTUI,  A12 1 MOVI.N,  A11 $400 MOVI,
  A10 6 MOVI.N,  A8 $4 L L32R,  A8 CALLX8,
  A2 0 MOVI.N,  RETW.N,
$22 L:  END-CODE

CODE esf_buf_recycle   \ .wifirxiram.8 $20
  $0 8 LITERALS
    $24 SYMBOL+ rodata  SYMBOL g_wifi_menuconfig  SYMBOL g_wifi_global_lock
    SYMBOL g_osi_funcs_p  SYMBOL g_eb_list_desc  SYMBOL g_lmac_cnt  SYMBOL memset
    SYMBOL esf_buf_recycle_default_handler
$20 L:
  A1 32 ENTRY,  A2 $10C L BEQZ,
  A3 A2 26 L8UI,  A4 8 MOVI.N,  A8 A3 -1 ADDI.N,  A8 A8 0 8 EXTUI,
  A4 A8 $36 L BGEU,
  $FB L J,
$36 L:
  A4 $0 L L32R,  A8 A8 A4 ADDX4,  A4 A8 0 L32I.N,  A4 JX,
$41 L:
  A4 $4 L L32R,  A4 A4 8 L32I.N,  $4F L J,
$49 L:
  A4 $4 L L32R,  A4 A4 84 L32I,
$4F L:
  A4 $A4 L BNEZ,
$52 L:
  A12 32 MOVI.N,  A11 0 MOVI.N,  A4 0 MOVI.N,  A10 A2 44 L32I.N,  A8 $18 L L32R,
  A8 CALLX8,
  A6 $C L L32R,  A5 $8 L L32R,  A4 A2 20 S16I,  MEMW,  A4 A6 0 L32I.N,
  A10 A5 0 L32I.N,  A4 A4 84 L32I,  MEMW,  A4 CALLX8,
  A4 A3 A3 ADDX4,  A3 A3 A3 ADDX2,  A8 $10 L L32R,  A4 A4 A8 ADDX4,  A8 A4 0 L32I.N,
  A8 A2 40 S32I.N,  A2 A4 0 S32I.N,  A2 $14 L L32R,  A3 A3 A2 ADDX4,
  A2 A3 4 L32I.N,  A2 A2 1 ADDI.N,  A2 A3 4 S32I.N,  A2 A6 0 L32I.N,
  A10 A5 0 L32I.N,  A2 A2 88 L32I,  MEMW,  $F2 L J,
$A4 L:
  A5 $C L L32R,  A10 A2 MOV.N,  A2 A3 A3 ADDX4,  A4 A5 0 L32I.N,  A4 A4 176 L32I,
  A4 CALLX8,
  A4 $10 L L32R,  A2 A2 A4 ADDX4,  A4 A2 16 L16UI,  A4 $10C L BEQZ,
  A4 $8 L L32R,  A6 A5 0 L32I.N,  A10 A4 0 L32I.N,  A6 A6 84 L32I,  A6 CALLX8,
  A8 A2 14 L16UI,  A8 $DC L BEQZ,
  A8 A8 -1 ADDI,  A8 A2 14 S16I,  MEMW,
$DC L:
  A3 A3 A3 ADDX2,  A2 $14 L L32R,  A3 A3 A2 ADDX4,  A2 A3 4 L32I.N,  A2 A2 1 ADDI.N,
  A2 A3 4 S32I.N,  A2 A5 0 L32I.N,  A10 A4 0 L32I.N,  A2 A2 88 L32I,
$F2 L:
  MEMW,  A2 CALLX8,
  $10C L J,
$FB L:
  A10 A3 MOV.N,  A8 $1C L L32R,  A8 CALLX8,
  A10 1 $A4 L BEQI,
  A10 2 $10C L BNEI,
  $52 L J,
$10C L:
  RETW.N,
  rodata $24 +  $41 L!+  $A4 L!+  $A4 L!+  $A4 L!+  $52 L!+  $52 L!+  $A4 L!+  $49 L!+
  $52 L!+  DROP   \ the table of jumps
$10E L:  END-CODE

CODE esf_buf_setup_for_mesh   \ .text.esf_buf_setup_for_mesh $4
  $0 1 LITERALS
    SYMBOL g_eb_list_desc
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A9 64 MOVI.N,  A10 128 MOVI,  A2 0 MOVI.N,
  A9 A8 96 S16I,  A10 A8 76 S16I,  A9 A8 56 S16I,  MEMW,  MEMW,  RETW.N,
$22 L:  END-CODE

CODE esf_buf_setup_static   \ .text.esf_buf_setup_static $14
  $0 5 LITERALS
    SYMBOL g_eb_list_desc  SYMBOL rodata_wlog_warning.9
    SYMBOL esf_buf_alloc_dynamic  SYMBOL wifi_log  SYMBOL esf_buf_recycle
$14 L:
  A1 32 ENTRY,  A6 $0 L L32R,  A5 A2 A2 ADDX4,  A5 A5 A6 ADDX4,  A6 0 MOVI.N,
  A3 A5 10 S16I,  MEMW,  MEMW,  $5E L J,
$2E L:
  A12 A6 MOV.N,  A11 A4 MOV.N,  A10 A5 MOV.N,  A3 A3 -1 ADDI.N,  A8 $8 L L32R,
  A8 CALLX8,
  A10 $58 L BNEZ.N,
  A13 $4 L L32R,  A14 A2 MOV.N,  A12 2 MOVI.N,  A11 $400 MOVI,  A10 1 MOVI.N,
  A8 $C L L32R,  A8 CALLX8,
  A3 $101 MOVI,  $61 L J,
  2 ZEROS,
$58 L:
  A8 $10 L L32R,  A8 CALLX8,
$5E L:
  A3 $2E L BNEZ,
$61 L:
  A2 A3 MOV.N,  RETW.N,
$65 L:  END-CODE

CODE esf_buf_free_static   \ .text.esf_buf_free_static $14
  $0 5 LITERALS
    SYMBOL g_eb_list_desc  SYMBOL g_wifi_global_lock  SYMBOL g_osi_funcs_p
    SYMBOL memset  SYMBOL memset
$14 L:
  A1 32 ENTRY,  A3 9 MOVI.N,  A3 A2 $1F L BGE,
  $A4 L J,
$1F L:
  A4 $0 L L32R,  A3 A2 A2 ADDX4,  A5 A3 2 SLLI,  A6 0 MOVI.N,  A3 A3 A4 ADDX4,
  A6 A3 14 S16I,  MEMW,
$33 L:
  A6 $8 L L32R,  A4 $4 L L32R,  A3 A6 0 L32I,  A10 A4 0 L32I,  A3 A3 84 L32I,  MEMW,
  A3 CALLX8,
  A3 $0 L L32R,  A7 A3 A5 ADD.N,  A3 A7 0 L32I.N,  A9 A6 0 L32I.N,  A3 $61 L BNEZ.N,
  A2 A9 88 L32I,  A10 A4 0 L32I,  A2 CALLX8,
  $A4 L J,
  2 ZEROS,
$61 L:
  A10 A3 40 L32I.N,  A9 A9 88 L32I,  A10 A7 0 S32I.N,  A7 0 MOVI.N,
  A10 A4 0 L32I.N,  A7 A3 40 S32I,  MEMW,  A9 CALLX8,
  A2 5 $96 L BNEI,
  A11 A7 MOV.N,  A12 32 MOVI.N,  A10 A3 44 L32I.N,  A8 $C L L32R,  A8 CALLX8,
  A12 48 MOVI.N,  A11 A7 MOV.N,  A10 A3 MOV.N,  A8 $10 L L32R,  A8 CALLX8,
  $33 L J,
  3 ZEROS,
$96 L:
  A6 A6 0 L32I.N,  A10 A3 MOV.N,  A6 A6 176 L32I,  A6 CALLX8,
  $33 L J,
  1 ZEROS,
$A4 L:
  RETW.N,
$A6 L:  END-CODE

CODE esf_buf_setdown   \ .text.esf_buf_setdown $4
  $0 1 LITERALS
    SYMBOL esf_buf_free_static
$4 L:
  A1 32 ENTRY,  A2 1 MOVI,
$A L:
  A10 A2 MOV.N,  A2 A2 1 ADDI.N,  A8 $0 L L32R,  A8 CALLX8,
  A2 10 $A L BNEI,
  RETW.N,
$19 L:  END-CODE

CODE esf_buf_setup   \ .text.esf_buf_setup $60
  $0 24 LITERALS
    SYMBOL eb_space  SYMBOL eb_txdesc_space  SYMBOL g_wifi_menuconfig
    SYMBOL g_eb_list_desc  SYMBOL rodata_wlog_info.10  SYMBOL rodata_wlog_info.11
    SYMBOL rodata_wlog_info.12  SYMBOL rodata_wlog_info.13
    SYMBOL rodata_wlog_info.14  SYMBOL rodata_wlog_info.15
    SYMBOL rodata_wlog_info.16  SYMBOL memset  SYMBOL memset  SYMBOL wifi_log
    SYMBOL esf_buf_setup_static  SYMBOL wifi_log  SYMBOL wifi_log
    SYMBOL esf_buf_setup_static  SYMBOL wifi_log
    SYMBOL config_is_cache_tx_buf_enabled  SYMBOL esf_buf_setup_static
    SYMBOL wifi_log  SYMBOL esf_buf_recycle  SYMBOL esf_buf_setdown
$60 L:
  A1 48 ENTRY,  A6 $4 L L32R,  A12 128 MOVI,  A11 0 MOVI.N,  A10 A6 A6 OR,
  A8 $2C L L32R,  A8 CALLX8,
  A5 $0 L L32R,  A12 192 MOVI,  A11 0 MOVI.N,  A10 A5 MOV.N,  A8 $30 L L32R,
  A8 CALLX8,
  A3 $8 L L32R,  A14 A3 4 L16UI,  A14 $8E L BNEZ.N,
  A14 64 MOVI.N,
$8E L:
  A11 1 MOVI.N,  A4 $C L L32R,  A12 3 MOVI.N,  A10 6 MOVI.N,  A13 $10 L L32R,
  A14 A4 156 S16I,  MEMW,  MEMW,  A8 $34 L L32R,  A8 CALLX8,
  A8 A3 84 L32I,  A11 A3 88 L16UI,  A8 $D5 L BNEZ.N,
  A11 $B5 L BNEZ.N,
  A11 10 MOVI.N,
$B5 L:
  A12 $1F4 MOVI,  A11 A4 176 S16I,  MEMW,  A10 8 MOVI.N,  MEMW,  A8 $38 L L32R,
  A8 CALLX8,
  A10 $1C1 L BNEZ,
  A14 A4 176 L16UI,  A13 $14 L L32R,  $E6 L J,
$D5 L:
  A14 A11 MOV.N,  A11 $DD L BNEZ,
  A14 10 MOVI,
$DD L:
  A13 $18 L L32R,  A14 A4 176 S16I,  MEMW,
$E6 L:
  A12 3 MOVI.N,  A11 1 MOVI.N,  A10 6 MOVI.N,  MEMW,  A8 $3C L L32R,  A8 CALLX8,
  A14 A3 60 L16UI,  A14 $FD L BNEZ.N,
  A14 32 MOVI,
$FD L:
  A12 3 MOVI.N,  A13 $1C L L32R,  A11 1 MOVI.N,  A10 6 MOVI.N,  A14 A4 76 S16I,
  MEMW,  MEMW,  A8 $40 L L32R,  A8 CALLX8,
  A8 A3 8 L32I.N,  A8 $149 L BNEZ.N,
  A8 A3 12 L16UI,  A8 $120 L BNEZ.N,
  A8 32 MOVI.N,
$120 L:
  A12 $640 MOVI,  A8 A4 36 S16I,  MEMW,  A11 A2 MOV.N,  A10 1 MOVI.N,
  A8 A4 32 L8UI,  A12 A12 A8 SUB,  MEMW,  A8 $44 L L32R,  A8 CALLX8,
  A14 A2 MOV.N,  A13 $20 L L32R,  A10 $159 L BEQZ.N,
  $1C1 L J,
  3 ZEROS,
$149 L:
  A14 A3 16 L16UI,  A14 $150 L BNEZ.N,
  A14 32 MOVI.N,
$150 L:
  A13 $24 L L32R,  A14 A4 36 S16I,  MEMW,
$159 L:
  A12 3 MOVI.N,  A11 1 MOVI.N,  A10 6 MOVI.N,  MEMW,  A8 $48 L L32R,  A8 CALLX8,
  A8 $4C L L32R,  A8 CALLX8,
  A10 $198 L BEQZ.N,
  A2 A4 192 L8UI,  A12 $640 MOVI,  A12 A12 A2 SUB,  A10 9 MOVI.N,  A11 A3 20 L32I,
  A8 $50 L L32R,  A8 CALLX8,
  A10 $1C1 L BNEZ.N,
  A12 3 MOVI.N,  A14 A3 20 L32I.N,  A11 1 MOVI.N,  A10 6 MOVI,  A13 $28 L L32R,
  A8 $54 L L32R,  A8 CALLX8,
$198 L:
  A2 0 MOVI.N,  A3 5 MOVI.N,
$19C L:
  A4 A6 A2 ADD.N,  A4 A5 44 S32I.N,  A10 A5 MOV.N,  A2 A2 32 ADDI,  A3 A5 26 S8I,
  MEMW,  MEMW,  A8 $58 L L32R,  A8 CALLX8,
  A5 A5 48 ADDI,  A2 128 $19C L BNEI,
  A10 0 MOVI.N,  $1CE L J,
  2 ZEROS,
$1C1 L:
  A10 A1 0 S32I.N,  MEMW,  A8 $5C L L32R,  A8 CALLX8,
  A10 A1 0 L32I.N,
$1CE L:
  A2 A10 MOV.N,  RETW.N,
$1D2 L:  END-CODE

CODE esf_buf_statis_dump   \ .text.esf_buf_statis_dump $3C
  $0 15 LITERALS
    SYMBOL rodata_wlog_info.17  SYMBOL rodata.esf_buf_statis_dump~CF0A
    $4 SYMBOL+ rodata.esf_buf_statis_dump~CF0A  SYMBOL rodata_wlog_info.18
    $6 SYMBOL+ rodata.esf_buf_statis_dump~CF0A
    $F SYMBOL+ rodata.esf_buf_statis_dump~CF0A
    $17 SYMBOL+ rodata.esf_buf_statis_dump~CF0A
    $22 SYMBOL+ rodata.esf_buf_statis_dump~CF0A
    $2E SYMBOL+ rodata.esf_buf_statis_dump~CF0A  SYMBOL g_eb_list_desc
    SYMBOL g_lmac_cnt  SYMBOL rodata_wlog_info.19  SYMBOL wifi_log
    SYMBOL wifi_log  SYMBOL wifi_log
$3C L:
  A1 64 ENTRY,  A13 $0 L L32R,  A12 3 MOVI,  A11 $400 MOVI,  A10 1 MOVI.N,
  A8 $30 L L32R,  A8 CALLX8,
  A2 $10 L L32R,  A12 3 MOVI.N,  A11 $400 MOVI,  A10 1 MOVI.N,  A2 A1 16 S32I.N,
  A2 $14 L L32R,  A2 A1 12 S32I.N,  A2 $18 L L32R,  A2 A1 8 S32I,  A2 $1C L L32R,
  A2 A1 4 S32I.N,  A2 $20 L L32R,  A15 $4 L L32R,  A14 $8 L L32R,  A13 $C L L32R,
  A2 A1 0 S32I,  MEMW,  A8 $34 L L32R,  A8 CALLX8,
  A2 $28 L L32R,  A4 A2 108 ADDI,  A3 $24 L L32R,
$8D L:
  A12 3 MOVI.N,  A11 $400 MOVI,  A10 1 MOVI.N,  A2 A2 12 ADDI.N,  A8 A2 10 L16UI,
  A3 A3 20 ADDI,  A15 A3 16 L16UI,  A14 A3 8 L8UI,  A8 A1 16 S32I.N,  A8 A2 8 L16UI,
  A8 A1 12 S32I.N,  A8 A2 4 L32I.N,  A8 A1 8 S32I.N,  A8 A2 0 L32I.N,  A8 A1 4 S32I,
  A8 A3 14 L16UI,  A13 $2C L L32R,  A8 A1 0 S32I,  MEMW,  A8 $38 L L32R,  A8 CALLX8,
  A2 A4 $8D L BNE,
  RETW.N,
$C9 L:  END-CODE

LINK-SYMBOLS   \ the cells of other packages that wait for these names
END-PACKAGE  PREVIOUS   \ not in the search order: ALSO name gives its words (README.md)
