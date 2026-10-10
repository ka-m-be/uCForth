\ pm_beacon_offset.fs - package PP-PM-BEACON-OFFSET: pm_beacon_offset.o of libpp.a (ESP32)
\
\ Made from pm_beacon_offset.S by s2fs.py, not by hand. Each function of C is a
\ CODE word: its bytes are those of the compiler, at the same offsets of
\ its section (L gives the address of an offset); its literals are at the
\ start of the word, after a jump. The data come first, then the
\ functions, each after those that it calls. The global symbols are
\ PUBLIC, the static ones PRIVATE. lib/esp32/clink.fs explains the words.
\
\ 32 functions (31 global; 1 static without symbol, named after their
\ section), 16 data objects.
\ A function runs as C code with the windowed calls of the Xtensa: call
\ it with CALL-C ( x1 .. xn n xt -- x ), never by its name alone.
\
\ Names of other packages: a literal waits until its package comes
\ (LINK-SYMBOLS); a name outside the libraries until EXTERN gives it.
\ Uses:
\   PP-PM  g_pm g_pm_cfg pm_on_sample_beacon
\   PP-PP-TIMER  pp_timer_process
\ Outside the libraries (../README.md):
\   __adddf3 __divsf3 __extendsfdf2 __floatsidf __floatunsidf __muldf3
\   __subdf3 __truncdfsf2 g_mesh_init_ps_type g_mesh_is_root
\   g_mesh_is_started g_osi_funcs_p g_wifi_mac_time_delta memcpy memset
\   wifi_event_post wifi_log
\ C names with more than 31 characters (Forth takes 31): the first 26,
\ ~ and 4 hex digits of their CRC-32 (../README.md):
\   empty_func_p_u8_u8_float_f~0689 = empty_func_p_u8_u8_float_float_pparams_r_void
\   empty_func_p_u8_u8_pu16_pf~6606 = empty_func_p_u8_u8_pu16_pfloat_r_void
\   pm_beacon_offset_add_loss_~100A = pm_beacon_offset_add_loss_counter
\   pm_beacon_offset_add_total~1967 = pm_beacon_offset_add_total_counter
\   pm_beacon_offset_funcs_emp~83F5 = pm_beacon_offset_funcs_empty_init
\   pm_beacon_offset_get_rx_be~A9A9 = pm_beacon_offset_get_rx_beacon_success_rate
\   pm_beacon_offset_resample_~4AAB = pm_beacon_offset_resample_timeout
\   pm_beacon_offset_resample_~FE38 = pm_beacon_offset_resample_timeout_process
\   pm_beacon_offset_set_rx_be~AA14 = pm_beacon_offset_set_rx_beacon_standard
\   rodata.pm_beacon_offset_ch~6179 = rodata.pm_beacon_offset_check.str1.1
\
\ Needs XTENSA (asm/xtensa.fs), ASM (lib/esp32/asm.fs) and CLINK
\ (lib/esp32/clink.fs) of ucf; the ESP32.

PACKAGE PP-PM-BEACON-OFFSET  ALSO ASM  ALSO CLINK   \ lib/package.fs
EXPORTS   \ the PUBLIC words are symbols for the other packages

\ ==== Data =============================================================

PRIVATE
CREATE rodata.pm_beacon_offset_ch~6179   \ .rodata.pm_beacon_offset_check.str1.1, 16 bytes
  ," finished"  0 C,
  ," failed"  0 C,
4 DATA: bss.total_tbtt_count$0   \ .bss.total_tbtt_count$0
4 DATA: bss.prev_last_tbtt_time$1   \ .bss.prev_last_tbtt_time$1
CREATE rodata_wlog_error.20   \ .rodata_wlog_error.20, 29 bytes
  ," alloc pm_beacon_offset fail"  10 C,  0 C,
PUBLIC
4 DATA: ptr_beacon_offset_funcs   \ .dram1.19
PRIVATE
CREATE rodata_wlog_debug.18   \ .rodata_wlog_debug.18, 50 bytes
  ," e: %.4f, edp: %.4f, a: %.4f, s: %u, d: %u, c: %u"  10 C,  0 C,
CREATE rodata_wlog_info.17   \ .rodata_wlog_info.17, 70 bytes
  ," [calculation %s] offset: %lu, timeout: %lu, x: %.4f, p: %.4f"  ," , bl: %lu"
  0 C,
CREATE rodata_wlog_debug.16   \ .rodata_wlog_debug.16, 9 bytes
  ," %llu:%lu"  0 C,
CREATE rodata_wlog_debug.15   \ .rodata_wlog_debug.15, 35 bytes
  ," first beacon unstable period: %.4f"  0 C,
CREATE rodata_wlog_info.14   \ .rodata_wlog_info.14, 25 bytes
  ," std change: %.2f -> %.2f"  0 C,
CREATE rodata_wlog_info.13   \ .rodata_wlog_info.13, 81 bytes
  ," beacon offset period: %lu, resample period: %u, standard: %."
  ," 2f, difference: %.2f"  0 C,
CREATE rodata_wlog_error.8   \ .rodata_wlog_error.8, 15 bytes
  ," m f bcn sample"  0 C,
CREATE rodata_wlog_info.7   \ .rodata_wlog_info.7, 55 bytes
  ," bcn sample terminated due to too large beacon interval"  0 C,
CREATE rodata_wlog_error.5   \ .rodata_wlog_error.5, 34 bytes
  ," calculate count error, %u, %u, %u"  0 C,
PUBLIC
6 DATA: s_pm_beacon_offset_config   \ .bss.s_pm_beacon_offset_config
76 DATA: s_pm_beacon_offset   \ .bss.s_pm_beacon_offset

\ ==== Code =============================================================

CODE pm_beacon_offset_get_average   \ .wifislpiram.2 $60
  $0 24 LITERALS
    $0 T,  SYMBOL g_pm  $447A0000 T,  $D2F1A9FC T,  $3FF0624D T,
    SYMBOL s_pm_beacon_offset  $0 T,  $3FE00000 T,  SYMBOL __divsf3
    SYMBOL __extendsfdf2  SYMBOL __muldf3  SYMBOL __truncdfsf2  SYMBOL __floatsidf
    SYMBOL __adddf3  SYMBOL __muldf3  SYMBOL __extendsfdf2  SYMBOL __adddf3
    SYMBOL __truncdfsf2  SYMBOL __divsf3  SYMBOL __extendsfdf2  SYMBOL __floatsidf
    SYMBOL __muldf3  SYMBOL __subdf3  SYMBOL __truncdfsf2
$60 L:
  A1 64 ENTRY,  A7 $4 L L32R,  A6 A3 0 8 EXTUI,  A2 A2 0 8 EXTUI,  A3 A7 $1D0 L32I,
  F0 A3 0 UFLOAT.S,  A10 F0 RFR,  A11 $8 L L32R,  A8 $20 L L32R,  A8 CALLX8,
  A3 A10 MOV.N,  A7 A7 44 L32I.N,  A7 A7 10 SRLI,  F1 A7 0 FLOAT.S,  A10 F1 RFR,
  A8 $24 L L32R,  A8 CALLX8,
  A12 $C L L32R,  A13 $10 L L32R,  A8 $28 L L32R,  A8 CALLX8,
  A8 $2C L L32R,  A8 CALLX8,
  A8 $0 L L32R,  F0 A8 WFR,  A7 $14 L L32R,  A10 A1 0 S32I.N,  A15 A7 4 L32I.N,
  A7 A2 MOV.N,  MEMW,  $13C L J,
  1 ZEROS,
$B9 L:
  A9 A7 A15 ADDX2,  A14 A9 0 L16UI,  A14 $13A L BEQZ,
  A8 A14 A8 ADD.N,  A10 A7 MOV.N,  A8 A4 0 S16I,  MEMW,  A14 A1 20 S32I.N,
  A15 A1 12 S32I,  F0 A1 16 SSI,  MEMW,  A8 $30 L L32R,  A8 CALLX8,
  A12 $18 L L32R,  A13 $1C L L32R,  A8 $34 L L32R,  A8 CALLX8,
  A12 $C L L32R,  A13 $10 L L32R,  A8 $38 L L32R,  A8 CALLX8,
  A8 A10 MOV.N,  A10 A3 MOV.N,  A8 A1 4 S32I.N,  A11 A1 8 S32I.N,  MEMW,
  A8 $3C L L32R,  A8 CALLX8,
  A12 A10 MOV.N,  A13 A11 MOV.N,  A8 A1 4 L32I.N,  A10 A8 MOV.N,  A9 A1 8 L32I.N,
  A11 A9 MOV.N,  A8 $40 L L32R,  A8 CALLX8,
  A8 $44 L L32R,  A8 CALLX8,
  F2 A1 0 LSI,  F1 A10 WFR,  B0 F2 F1 OLT.S,  A14 A1 20 L32I.N,  A15 A1 12 L32I.N,
  F0 A1 16 LSI,  B0 $134 L BF,
  F1 F2 MOV.S,
$134 L:
  F2 A14 0 FLOAT.S,  F0 F2 F1 MADD.S,
$13A L:
  A7 A7 1 ADDI.N,
$13C L:
  A9 A7 0 8 EXTUI,  A8 A4 0 L16UI,  A9 A6 $148 L BGEU,
  $B9 L J,
$148 L:
  A8 $198 L BEQZ,
  F1 A8 0 FLOAT.S,  A10 F0 RFR,  A11 F1 RFR,  A8 $48 L L32R,  A8 CALLX8,
  A8 $4C L L32R,  A8 CALLX8,
  A8 A10 MOV.N,  A10 A2 MOV.N,  A8 A1 4 S32I.N,  A11 A1 8 S32I.N,  MEMW,
  A8 $50 L L32R,  A8 CALLX8,
  A12 $C L L32R,  A13 $10 L L32R,  A8 $54 L L32R,  A8 CALLX8,
  A12 A10 MOV.N,  A13 A11 MOV.N,  A8 A1 4 L32I.N,  A10 A8 MOV.N,  A9 A1 8 L32I,
  A11 A9 MOV.N,  A8 $58 L L32R,  A8 CALLX8,
  A8 $5C L L32R,  A8 CALLX8,
  A10 A5 0 S32I.N,
$198 L:
  MEMW,  RETW.N,
$19D L:  END-CODE

CODE pm_beacon_offset_get_expect   \ .wifislpiram.3 $50
  $0 20 LITERALS
    $3F800000 T,  $0 T,  SYMBOL s_pm_beacon_offset_config  SYMBOL g_pm
    $447A0000 T,  $D2F1A9FC T,  $3FF0624D T,  SYMBOL pm_beacon_offset_get_average
    SYMBOL __divsf3  SYMBOL __divsf3  SYMBOL __extendsfdf2  SYMBOL __extendsfdf2
    SYMBOL __muldf3  SYMBOL __adddf3  SYMBOL __truncdfsf2  SYMBOL __extendsfdf2
    SYMBOL __muldf3  SYMBOL __truncdfsf2  SYMBOL __divsf3  SYMBOL __divsf3
$50 L:
  A1 80 ENTRY,  A4 0 MOVI.N,  F0 A6 20 LSI,  A2 A2 0 8 EXTUI,  A3 A3 0 8 EXTUI,
  A10 A2 MOV.N,  A13 A1 MOV.N,  A4 A1 4 S16I,  MEMW,  A12 A1 4 ADDI.N,
  A11 A3 A3 OR,  A4 $4 L L32R,  A5 A1 16 S32I,  A4 A1 0 S32I,  F0 A1 20 SSI,  MEMW,
  A8 $1C L L32R,  A8 CALLX8,
  A10 A4 MOV.N,  A7 A1 4 L16UI,  A7 $1E2 L BEQZ,
  F4 A7 0 UFLOAT.S,  A3 A3 A2 SUB,  A4 $8 L L32R,  A10 F4 RFR,  A3 A3 0 8 EXTUI,
  A4 A4 0 L16UI,  F1 A4 0 UFLOAT.S,  A11 F1 RFR,  A8 $20 L L32R,  A8 CALLX8,
  F0 A1 16 LSI,  F5 A10 WFR,  F5 F5 F0 MUL.S,  A4 $0 L L32R,  F1 A4 WFR,
  B0 F5 F1 OLT.S,  F5 A1 16 SSI,  F2 A1 0 LSI,  B0 $C6 L BT,
  $1B8 L J,
$C6 L:
  A8 $C L L32R,  A4 A8 $1D0 L32I,  F0 A4 0 UFLOAT.S,  A10 F0 RFR,  A11 $10 L L32R,
  A8 A1 32 S32I.N,  F1 A1 28 SSI,  F2 A1 36 SSI,  MEMW,  A8 $24 L L32R,  A8 CALLX8,
  A8 $28 L L32R,  A8 CALLX8,
  F4 A3 0 UFLOAT.S,  A4 A10 MOV.N,  A10 F4 RFR,  A5 A11 MOV.N,  A8 $2C L L32R,
  A8 CALLX8,
  A12 $14 L L32R,  A13 $18 L L32R,  A8 $30 L L32R,  A8 CALLX8,
  A12 A10 MOV.N,  A13 A11 A11 OR,  A10 A4 A4 OR,  A11 A5 MOV.N,  A8 $34 L L32R,
  A8 CALLX8,
  A8 $38 L L32R,  A8 CALLX8,
  F0 A2 0 FLOAT.S,  F5 A1 20 LSI,  F3 A10 WFR,  F5 F5 F0 SUB.S,  F3 A1 24 SSI,
  A10 F5 RFR,  MEMW,  A8 $3C L L32R,  A8 CALLX8,
  A12 $14 L L32R,  A13 $18 L L32R,  A8 $40 L L32R,  A8 CALLX8,
  A8 $44 L L32R,  A8 CALLX8,
  F0 A10 WFR,  F3 A1 24 LSI,  B0 F0 F3 OLT.S,  A8 A1 32 L32I.N,  F1 A1 28 LSI,
  F2 A1 36 LSI,  B0 $162 L BF,
  F3 A10 WFR,
$162 L:
  A4 0 MOVI.N,  F4 F1 MOV.S,  F5 A1 16 LSI,  F0 F1 F5 SUB.S,  A5 A8 $182 L16SI,
  $17C L J,
  1 ZEROS,
$174 L:
  A4 A4 1 ADDI.N,  A4 A4 15 SEXT,  F4 F6 MOV.S,
$17C L:
  F6 F0 F4 MUL.S,  A4 A5 $174 L BLT,
  F5 A6 24 LSI,  F1 F1 F6 SUB.S,  F5 F5 F2 SUB.S,  F0 F2 F3 SUB.S,  F1 F1 F3 MUL.S,
  A11 A1 16 L32I.N,  F0 F5 F4 MADD.S,  A10 F1 RFR,  F0 A1 24 SSI,  F2 A1 36 SSI,
  MEMW,  A8 $48 L L32R,  A8 CALLX8,
  F1 A10 WFR,  F0 A1 24 LSI,  F0 F1 F0 ADD.S,  F2 A1 36 LSI,  $1BB L J,
  1 ZEROS,
$1B8 L:
  F0 F2 MOV.S,
$1BB L:
  F4 A1 16 LSI,  A10 F0 RFR,  A11 F4 RFR,  F0 A6 0 SSI,  F4 A6 4 SSI,  F2 A6 8 SSI,
  A2 A6 16 S8I,  A3 A6 17 S8I,  A7 A6 18 S16I,  MEMW,  MEMW,  A8 $4C L L32R,
  A8 CALLX8,
$1E2 L:
  A10 A6 28 S32I.N,  MEMW,  RETW.N,
$1E9 L:  END-CODE

CODE pm_beacon_offset_is_sampling   \ .wifislpiram.11 $4
  $0 1 LITERALS
    SYMBOL s_pm_beacon_offset
$4 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A8 0 MOVI.N,  A9 A2 0 L16UI,  A2 1 MOVI.N,
  A2 A8 A9 MOVEQZ,  RETW.N,
$16 L:  END-CODE

CODE pm_beacon_offset_is_enabled   \ .wifislpiram.12 $4
  $0 1 LITERALS
    SYMBOL s_pm_beacon_offset_config
$4 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A8 0 MOVI.N,  A9 A2 0 L16UI,  A2 1 MOVI.N,
  A2 A8 A9 MOVEQZ,  RETW.N,
$16 L:  END-CODE

CODE pm_beacon_offset_get_rx_be~A9A9   \ pm_beacon_offset_get_rx_beacon_success_rate: .text.pm_beacon_offset_get_rx_beacon_success_rate $8
  $0 2 LITERALS
    SYMBOL s_pm_beacon_offset  SYMBOL __divsf3
$8 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A9 A8 8 L32I.N,  F0 A9 0 UFLOAT.S,  A11 F0 RFR,
  A8 A8 12 L32I,  A8 A9 A8 SUB,  F0 A8 0 UFLOAT.S,  A10 F0 RFR,  A8 $4 L L32R,
  A8 CALLX8,
  A2 A10 MOV.N,  RETW.N,
$2C L:  END-CODE

CODE pm_beacon_offset_add_total~1967   \ pm_beacon_offset_add_total_counter: .text.pm_beacon_offset_add_total_counter $4
  $0 1 LITERALS
    SYMBOL s_pm_beacon_offset
$4 L:
  A1 32 ENTRY,  A9 $0 L L32R,  A8 A9 8 L32I.N,  A8 A8 1 ADDI.N,  A8 A9 8 S32I.N,
  MEMW,  RETW.N,
$15 L:  END-CODE

CODE pm_beacon_offset_add_loss_~100A   \ pm_beacon_offset_add_loss_counter: .text.pm_beacon_offset_add_loss_counter $4
  $0 1 LITERALS
    SYMBOL s_pm_beacon_offset
$4 L:
  A1 32 ENTRY,  A9 $0 L L32R,  A8 A9 12 L32I.N,  A8 A8 1 ADDI.N,  A8 A9 12 S32I.N,
  MEMW,  RETW.N,
$15 L:  END-CODE

CODE empty_func_p_void_r_void   \ .text.empty_func_p_void_r_void $0
  $0 ORIGIN
  A1 32 ENTRY,  RETW.N,
$5 L:  END-CODE

CODE empty_func_p_u32_r_void   \ .text.empty_func_p_u32_r_void $0
  $0 ORIGIN
  A1 32 ENTRY,  RETW.N,
$5 L:  END-CODE

CODE empty_func_p_void_r_bool   \ .text.empty_func_p_void_r_bool $0
  $0 ORIGIN
  A1 32 ENTRY,  A2 0 MOVI.N,  RETW.N,
$7 L:  END-CODE

CODE empty_func_p_pconfg_r_void   \ .text.empty_func_p_pconfg_r_void $0
  $0 ORIGIN
  A1 32 ENTRY,  RETW.N,
$5 L:  END-CODE

CODE empty_func_p_u8_r_void   \ .text.empty_func_p_u8_r_void $0
  $0 ORIGIN
  A1 32 ENTRY,  RETW.N,
$5 L:  END-CODE

CODE empty_func_p_void_r_float   \ .text.empty_func_p_void_r_float $4
  $0 1 LITERALS
    $0 T,
$4 L:
  A1 32 ENTRY,  A2 $0 L L32R,  RETW.N,
$C L:  END-CODE

CODE empty_func_p_ptsf_u32_r_void   \ .text.empty_func_p_ptsf_u32_r_void $0
  $0 ORIGIN
  A1 32 ENTRY,  RETW.N,
$5 L:  END-CODE

CODE empty_func_p_u8_u8_pu16_pf~6606   \ empty_func_p_u8_u8_pu16_pfloat_r_void: .text.empty_func_p_u8_u8_pu16_pfloat_r_void $0
  $0 ORIGIN
  A1 32 ENTRY,  RETW.N,
$5 L:  END-CODE

CODE empty_func_p_u8_u8_float_f~0689   \ empty_func_p_u8_u8_float_float_pparams_r_void: .text.empty_func_p_u8_u8_float_float_pparams_r_void $0
  $0 ORIGIN
  A1 32 ENTRY,  RETW.N,
$5 L:  END-CODE

CODE pm_beacon_offset_get_params   \ .wifislpiram.4 $60
  $0 24 LITERALS
    $0 T,  SYMBOL g_pm  SYMBOL s_pm_beacon_offset_config
    SYMBOL rodata_wlog_error.5  $D2F1A9FC T,  $3FF0624D T,
    SYMBOL s_pm_beacon_offset  $42C80000 T,  SYMBOL g_pm_cfg  $36EE80 T,
    $10 SYMBOL+ s_pm_beacon_offset  SYMBOL g_osi_funcs_p
    SYMBOL pm_beacon_offset_get_average  SYMBOL wifi_log  SYMBOL memset
    SYMBOL __divsf3  SYMBOL __floatunsidf  SYMBOL __muldf3  SYMBOL __truncdfsf2
    SYMBOL __divsf3  SYMBOL memcpy  SYMBOL pm_beacon_offset_get_expect
    SYMBOL memcpy  SYMBOL memcpy
$60 L:
  A1 160 ENTRY,  A3 $4 L L32R,  A6 A2 MOV.N,  A5 0 MOVI.N,  A13 A1 80 ADDI,
  A12 A1 84 ADDI,  A2 $0 L L32R,  A10 0 MOVI.N,  A4 A3 44 L32I.N,  A2 A1 80 S32I,
  A2 A4 10 SRLI,  A4 A4 10 8 EXTUI,  A11 A4 MOV.N,  A5 A1 84 S16I,  MEMW,  MEMW,
  A8 $30 L L32R,  A8 CALLX8,
  A5 $8 L L32R,  A15 A1 84 L16UI,  A5 A5 0 L16UI,  A5 A15 $A0 L BNE,
  A6 A5 $BC L BGEU,
$A0 L:
  A12 1 MOVI.N,  A13 $C L L32R,  A14 A5 MOV.N,  A11 A12 12 SLLI,  A10 6 MOVI.N,
  A6 A1 0 S32I,  MEMW,  A8 $34 L L32R,  A8 CALLX8,
  $2A6 L J,
  1 ZEROS,
$BC L:
  A8 A2 0 8 EXTUI,  A12 32 MOVI,  A11 0 MOVI,  A10 A1 48 ADDI,  A8 A1 100 S32I,
  MEMW,  A8 $38 L L32R,  A8 CALLX8,
  F1 A5 0 UFLOAT.S,  F0 A6 0 UFLOAT.S,  A10 F1 RFR,  A11 F0 RFR,  A8 $3C L L32R,
  A8 CALLX8,
  A6 A10 MOV.N,  A10 A2 A2 OR,  A8 $40 L L32R,  A8 CALLX8,
  A12 $10 L L32R,  A13 $14 L L32R,  A8 $44 L L32R,  A8 CALLX8,
  A8 $48 L L32R,  A8 CALLX8,
  F3 A10 WFR,  F2 A1 80 LSI,  A11 A6 MOV.N,  F0 F2 F3 SUB.S,  F2 A1 96 SSI,
  F0 A1 120 SSI,  A10 A1 68 S32I,  MEMW,  A8 $4C L L32R,  A8 CALLX8,
  F1 A10 WFR,  A12 32 MOVI.N,  A11 A1 48 ADDI,  F0 A1 120 LSI,  A10 A1 16 ADDI,
  F0 F0 F1 ADD.S,  F0 A1 72 SSI,  MEMW,  A8 $50 L L32R,  A8 CALLX8,
  A5 A1 100 L32I,  A2 A5 -1 ADDI.N,  A2 A2 0 8 EXTUI,  A5 0 MOVI.N,  A8 $0 L L32R,
  A2 A4 A2 SUB,  F0 A8 WFR,  A9 A5 MOV.N,  F1 A8 WFR,  A15 A5 MOV.N,
$157 L:
  A8 A4 A2 SUB,  A7 0 MOVI.N,  A8 A1 104 S32I,  MEMW,  $1DF L J,
$165 L:
  A8 A1 100 L32I,  A14 A1 48 ADDI,  A11 A8 A7 SUB,  A13 A6 MOV.N,
  A11 A11 0 8 EXTUI,  A12 A1 96 L32I,  A10 A1 104 L32I,  A9 A1 116 S32I,
  A15 A1 108 S32I,  F0 A1 120 SSI,  F1 A1 112 SSI,  MEMW,  A8 $54 L L32R,
  A8 CALLX8,
  A8 $0 L L32R,  F3 A8 WFR,  F2 A1 76 LSI,  B0 F2 F3 OEQ.S,  A9 A1 116 L32I,
  A15 A1 108 L32I,  F0 A1 120 LSI,  F1 A1 112 LSI,  B0 $1DA L BT,
  B0 F2 F0 OLE.S,  B0 $1B5 L BT,
  B0 F0 F3 OEQ.S,  B0 $1DA L BF,
$1B5 L:
  A12 32 MOVI.N,  A11 A1 48 ADDI,  A10 A1 16 ADDI,  F2 A1 108 SSI,  MEMW,
  A8 $58 L L32R,  A8 CALLX8,
  A15 1 MOVI.N,  F2 A1 108 LSI,  F0 F2 MOV.S,  F1 A1 52 LSI,  A9 A1 64 L8UI,
  A5 A1 65 L8UI,
$1DA L:
  A8 A7 1 ADDI.N,  A7 A8 0 8 EXTUI,
$1DF L:
  A7 A2 $165 L BLT,
  A4 A2 $1EA L BEQ,
  A2 A2 1 ADDI.N,  $157 L J,
$1EA L:
  A4 $18 L L32R,  A15 $274 L BEQZ,
  A2 A9 10 SLLI,  A8 255 MOVI,  A7 A3 $100 ADDMI,  A2 A3 $1CC S32I,
  A2 A3 $1D0 L32I,  A2 A2 10 SRLI,  A2 A2 1 ADDI.N,  A2 A2 A5 ADD.N,  A2 A2 A8 MINU,
  A2 A7 124 S8I,  MEMW,  A8 1 MOVI.N,  A2 $1C L L32R,  F3 A2 WFR,  F2 F1 F3 MUL.S,
  A7 $8 L L32R,  A2 F2 0 UTRUNC.S,  A2 A2 0 8 EXTUI,  A10 A7 3 L8UI,  A8 A4 73 S8I,
  MEMW,  A2 A10 $27A L BGEU,
  A3 A3 $200 ADDMI,  A2 A7 2 L8UI,  A8 A3 28 S8I,  MEMW,  A2 $27A L BEQZ.N,
  A3 $20 L L32R,  A3 A3 51 L8UI,  A3 1 $27A L BNEI,
  A12 0 MOVI.N,  A3 $2C L L32R,  A11 $24 L L32R,  A11 A2 A11 MULL,  A3 A3 0 L32I.N,
  A10 $28 L L32R,  A3 A3 232 L32I,  A9 A1 116 S32I,  F0 A1 120 SSI,  F1 A1 112 SSI,
  MEMW,  A3 CALLX8,
  A9 A1 116 L32I,  F0 A1 120 LSI,  F1 A1 112 LSI,  $27A L J,
$274 L:
  A15 A4 73 S8I,  MEMW,
$27A L:
  A3 2 MOVI.N,  A12 32 MOVI.N,  A11 A1 16 ADDI,  A10 A4 40 ADDI,  A2 A4 36 L32I.N,
  A2 A2 A3 OR,  A2 A4 36 S32I,  F1 A1 20 SSI,  A9 A1 32 S8I,  A5 A1 33 S8I,  MEMW,
  F0 A1 44 SSI,  MEMW,  A8 $5C L L32R,  A8 CALLX8,
  A6 A4 52 S32I.N,
$2A6 L:
  MEMW,  RETW.N,
$2AB L:  END-CODE

CODE pm_beacon_offset_sample   \ .wifislpiram.9 $2C
  $0 11 LITERALS
    SYMBOL s_pm_beacon_offset  SYMBOL g_pm  SYMBOL bss.prev_last_tbtt_time$1
    SYMBOL g_mesh_is_started  SYMBOL g_mesh_is_root  SYMBOL g_mesh_init_ps_type
    SYMBOL g_osi_funcs_p  $3FF73C00 T,  SYMBOL g_wifi_mac_time_delta
    SYMBOL bss.total_tbtt_count$0  SYMBOL pm_beacon_offset_get_params
$2C L:
  A1 32 ENTRY,  A3 $0 L L32R,  A4 A3 0 L16UI,  A4 $F0 L BEQZ,
  A4 $4 L L32R,  A8 A4 80 L32I,  A9 A4 84 L32I,  A5 A8 A9 OR,  A5 $F0 L BEQZ,
  A5 $8 L L32R,  A10 A5 0 L32I.N,  A8 A10 $52 L BNE,
  A9 $F0 L BEQZ,
$52 L:
  A9 1 MOVI.N,  A8 A5 0 S32I.N,  A8 A3 36 L32I.N,  A8 A8 A9 OR,  A8 A3 36 S32I.N,
  A8 $C L L32R,  A8 A8 0 L8UI,  A8 $8C L BEQZ.N,
  A8 $10 L L32R,  MEMW,  A8 A8 0 L8UI,  A8 A8 0 8 EXTUI,  A8 $8C L BNEZ.N,
  A8 $14 L L32R,  A8 A8 0 L32I.N,  A8 $8C L BEQZ.N,
  A8 $18 L L32R,  A8 A8 0 L32I.N,  A10 A8 $110 L32I,  MEMW,  A10 CALLX8,
  $9B L J,
  1 ZEROS,
$8C L:
  A8 $1C L L32R,  MEMW,  A10 A8 0 L32I.N,  A8 $20 L L32R,  A8 A8 0 L32I.N,
  A10 A10 A8 ADD.N,
$9B L:
  A2 A2 10 SRLI,  A5 A5 0 L32I.N,  A10 A10 A5 SUB,  A4 A4 44 L32I.N,
  A10 A10 A4 QUOU,  A4 $24 L L32R,  A5 A3 4 L32I.N,  A2 A2 A5 ADDX2,
  A8 A4 0 L32I.N,  A8 A8 1 ADDI.N,  A10 A10 A8 ADD.N,  A8 A2 0 L16UI,
  A8 A8 1 ADDI.N,  A8 A2 0 S16I,  MEMW,  A2 A3 0 L16UI,  A2 A2 -1 ADDI.N,
  A2 A2 0 16 EXTUI,  A10 A4 0 S32I.N,  A2 A3 0 S16I,  MEMW,  A2 $F0 L BNEZ.N,
  MEMW,  A8 $28 L L32R,  A8 CALLX8,
  A2 A4 0 S32I.N,  A4 $18 L L32R,  A4 A4 0 L32I.N,  A10 A3 4 L32I.N,
  A4 A4 176 L32I,  MEMW,  A4 CALLX8,
  A2 A3 4 S32I.N,
$F0 L:
  MEMW,  RETW.N,
$F5 L:  END-CODE

CODE pm_beacon_offset_configure   \ .text.pm_beacon_offset_configure $20
  $0 8 LITERALS
    SYMBOL s_pm_beacon_offset_config  SYMBOL rodata_wlog_info.13  $42C80000 T,
    SYMBOL __divsf3  SYMBOL __extendsfdf2  SYMBOL __divsf3  SYMBOL __extendsfdf2
    SYMBOL wifi_log
$20 L:
  A1 48 ENTRY,  A3 A2 0 L16UI,  A8 $0 L L32R,  A3 A8 0 S16I,  MEMW,  A3 $9C L BEQZ,
  A9 A2 4 L8UI,  F0 A9 0 UFLOAT.S,  A10 F0 RFR,  A4 A2 2 L8UI,  A5 A2 3 L8UI,
  A2 1 MOVI.N,  A2 A2 12 SLLI,  A11 $8 L L32R,  A4 A8 2 S8I,  A5 A8 3 S8I,
  A9 A8 4 S8I,  MEMW,  MEMW,  A8 $C L L32R,  A8 CALLX8,
  A8 $10 L L32R,  A8 CALLX8,
  F0 A5 0 UFLOAT.S,  A11 A1 12 S32I,  A11 $8 L L32R,  A10 A1 8 S32I,  A10 F0 RFR,
  MEMW,  A8 $14 L L32R,  A8 CALLX8,
  A8 $18 L L32R,  A8 CALLX8,
  A15 A4 MOV.N,  A13 $4 L L32R,  A14 A3 MOV.N,  A12 3 MOVI.N,  A10 A1 0 S32I.N,
  A10 6 MOVI.N,  A11 A1 4 S32I.N,  A11 A2 MOV.N,  MEMW,  A8 $1C L L32R,  A8 CALLX8,
$9C L:
  RETW.N,
$9E L:  END-CODE

CODE pm_beacon_offset_set_rx_be~AA14   \ pm_beacon_offset_set_rx_beacon_standard: .text.pm_beacon_offset_set_rx_beacon_standard $20
  $0 8 LITERALS
    SYMBOL s_pm_beacon_offset_config  $42C80000 T,  SYMBOL rodata_wlog_info.14
    SYMBOL __divsf3  SYMBOL __extendsfdf2  SYMBOL __divsf3  SYMBOL __extendsfdf2
    SYMBOL wifi_log
$20 L:
  A1 64 ENTRY,  A4 $0 L L32R,  A2 A2 0 8 EXTUI,  A3 A4 3 L8UI,  F0 A3 0 UFLOAT.S,
  A3 1 MOVI.N,  A10 F0 RFR,  A11 $4 L L32R,  A3 A3 12 SLLI,  A8 $C L L32R,
  A8 CALLX8,
  A8 $10 L L32R,  A8 CALLX8,
  F0 A2 0 UFLOAT.S,  A15 A11 MOV.N,  A11 $4 L L32R,  A14 A10 MOV.N,  A10 F0 RFR,
  A14 A1 16 S32I.N,  A15 A1 20 S32I.N,  MEMW,  A8 $14 L L32R,  A8 CALLX8,
  A8 $18 L L32R,  A8 CALLX8,
  A12 3 MOVI.N,  A14 A1 16 L32I.N,  A15 A1 20 L32I.N,  A13 $8 L L32R,
  A10 A1 0 S32I.N,  A10 6 MOVI.N,  A11 A1 4 S32I.N,  A11 A3 MOV.N,  MEMW,
  A8 $1C L L32R,  A8 CALLX8,
  A2 A4 3 S8I,  MEMW,  MEMW,  RETW.N,
$8B L:  END-CODE

CODE pm_beacon_offset_reset   \ .wifislprxiram.10 $18
  $0 6 LITERALS
    SYMBOL s_pm_beacon_offset  SYMBOL g_pm  SYMBOL g_pm_cfg  SYMBOL g_osi_funcs_p
    $28 SYMBOL+ s_pm_beacon_offset  SYMBOL memset
$18 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A3 0 MOVI.N,  A4 0 MOVI.N,  A9 $8 L L32R,
  A3 A2 0 S16I,  MEMW,  A3 $4 L L32R,  A8 A3 $100 ADDMI,  A9 A9 37 L8UI,
  A10 A2 4 L32I.N,  A4 A3 $1CC S32I,  A9 A8 124 S8I,  MEMW,  A10 A4 $52 L BEQ,
  A8 $C L L32R,  A8 A8 0 L32I.N,  A8 A8 176 L32I,  MEMW,  A8 CALLX8,
  A4 A2 4 S32I.N,
$52 L:
  A4 0 MOVI.N,  A12 32 MOVI.N,  A11 A4 MOV.N,  A3 A3 $200 ADDMI,  A10 $10 L L32R,
  A4 A2 8 S32I.N,  A4 A2 12 S32I,  MEMW,  A8 $14 L L32R,  A8 CALLX8,
  A4 A2 72 S16I,  A4 A3 28 S8I,  MEMW,  MEMW,  RETW.N,
$7A L:  END-CODE

PRIVATE
CODE pm_beacon_offset_resample_~4AAB   \ .text.pm_beacon_offset_resample_timeout $4, static, without symbol
  $0 1 LITERALS
    SYMBOL pp_timer_process
$4 L:
  A1 32 ENTRY,  A11 0 MOVI.N,  A10 12 MOVI.N,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$13 L:  END-CODE

PUBLIC
CODE pm_beacon_offset_init   \ .text.pm_beacon_offset_init $10
  $0 4 LITERALS
    SYMBOL pm_beacon_offset_resample_~4AAB  $10 SYMBOL+ s_pm_beacon_offset
    SYMBOL g_osi_funcs_p  SYMBOL pm_beacon_offset_reset
$10 L:
  A1 32 ENTRY,  A8 $C L L32R,  A8 CALLX8,
  A8 $8 L L32R,  A12 0 MOVI.N,  A8 A8 0 L32I.N,  A11 $0 L L32R,  A8 A8 244 L32I,
  A10 $4 L L32R,  A8 CALLX8,
  RETW.N,
$2E L:  END-CODE

CODE pm_beacon_offset_deinit   \ .text.pm_beacon_offset_deinit $C
  $0 3 LITERALS
    $10 SYMBOL+ s_pm_beacon_offset  SYMBOL g_osi_funcs_p
    SYMBOL pm_beacon_offset_reset
$C L:
  A1 32 ENTRY,  A3 $4 L L32R,  A8 A3 0 L32I,  A2 $0 L L32R,  A10 A2 MOV.N,
  A8 A8 236 L32I,  A8 CALLX8,
  A10 A2 A2 OR,  A3 A3 0 L32I,  A3 A3 240 L32I,  A3 CALLX8,
  A8 $8 L L32R,  A8 CALLX8,
  RETW.N,
$34 L:  END-CODE

CODE pm_beacon_offset_sample_start   \ .wifislprxiram.6 $28
  $0 10 LITERALS
    SYMBOL s_pm_beacon_offset  SYMBOL s_pm_beacon_offset_config  SYMBOL g_pm
    SYMBOL rodata_wlog_info.7  SYMBOL g_osi_funcs_p  SYMBOL rodata_wlog_error.8
    $10 SYMBOL+ s_pm_beacon_offset  SYMBOL pm_beacon_offset_reset  SYMBOL wifi_log
    SYMBOL memset
$28 L:
  A1 32 ENTRY,  A3 $0 L L32R,  A5 A3 0 L16UI,  A5 $A8 L BNEZ,
  A4 $4 L L32R,  A2 A4 0 L16UI,  A2 $A8 L BEQZ,
  A8 $1C L L32R,  A8 CALLX8,
  A6 255 MOVI,  A2 $8 L L32R,  A2 A2 44 L32I.N,  A2 A2 10 SRLI,  A6 A2 $69 L BGEU,
  A12 3 MOVI.N,  A13 $C L L32R,
$56 L:
  A11 1 MOVI.N,  A11 A11 12 SLLI,  A10 6 MOVI,  A8 $20 L L32R,  A8 CALLX8,
  $A8 L J,
  2 ZEROS,
$69 L:
  A6 $10 L L32R,  A2 A2 A2 ADD.N,  A10 A2 MOV.N,  A8 A6 0 L32I,  A8 A8 172 L32I,
  A8 CALLX8,
  A12 1 MOVI.N,  A10 A3 4 S32I.N,  A13 $14 L L32R,  A10 $56 L BEQZ,
  A12 A2 MOV.N,  A11 A5 A5 OR,  MEMW,  A8 $24 L L32R,  A8 CALLX8,
  A2 A4 0 L16UI,  A2 A3 0 S16I,  MEMW,  A2 A6 0 L32I.N,  A10 $18 L L32R,
  A2 A2 236 L32I,  MEMW,  A2 CALLX8,
$A8 L:
  RETW.N,
$AA L:  END-CODE

CODE pm_beacon_offset_resample_~FE38   \ pm_beacon_offset_resample_timeout_process: .text.pm_beacon_offset_resample_timeout_process $4
  $0 1 LITERALS
    SYMBOL pm_on_sample_beacon
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$F L:  END-CODE

CODE pm_beacon_offset_check   \ .text.pm_beacon_offset_check $60
  $0 24 LITERALS
    SYMBOL rodata.pm_beacon_offset_ch~6179
    $9 SYMBOL+ rodata.pm_beacon_offset_ch~6179  SYMBOL s_pm_beacon_offset_config
    SYMBOL s_pm_beacon_offset  SYMBOL g_pm  $3F000000 T,  $42C80000 T,
    SYMBOL rodata_wlog_debug.15  SYMBOL rodata_wlog_debug.16
    SYMBOL rodata_wlog_info.17  SYMBOL rodata_wlog_debug.18
    SYMBOL pm_beacon_offset_get_rx_be~A9A9  SYMBOL wifi_event_post
    SYMBOL __divsf3  SYMBOL __extendsfdf2  SYMBOL wifi_log  SYMBOL wifi_log
    SYMBOL __extendsfdf2  SYMBOL __extendsfdf2  SYMBOL wifi_log
    SYMBOL __extendsfdf2  SYMBOL __extendsfdf2  SYMBOL __extendsfdf2
    SYMBOL wifi_log
$60 L:
  A1 96 ENTRY,  A5 $8 L L32R,  A9 A5 0 L16UI,  A9 $220 L BEQZ,
  A4 $C L L32R,  A8 A4 73 L8UI,  A8 $129 L BEQZ,
  A8 $10 L L32R,  A8 A8 $182 L16SI,  A8 A8 A9 MULL,  A9 A4 8 L32I.N,
  A9 A8 $86 L BGEU,
  $129 L J,
$86 L:
  A8 $2C L L32R,  A8 CALLX8,
  F0 A10 WFR,  A6 $14 L L32R,  F3 A6 WFR,  F1 A4 44 LSI,  F2 F1 F3 MUL.S,
  B0 F2 F0 OLT.S,  B0 $C1 L BT,
$A1 L:
  A5 0 MOVI.N,  A12 4 MOVI.N,  A11 A1 32 ADDI,  A10 46 MOVI.N,  A5 A4 72 S8I,  MEMW,
  F0 A1 32 SSI,  MEMW,  A8 $30 L L32R,  A8 CALLX8,
  $122 L J,
  2 ZEROS,
$C1 L:
  A5 A5 4 L8UI,  F2 A5 0 UFLOAT.S,  A10 F2 RFR,  A11 $18 L L32R,  F0 A1 52 SSI,
  F1 A1 48 SSI,  MEMW,  A8 $34 L L32R,  A8 CALLX8,
  F3 A10 WFR,  F1 A1 48 LSI,  F1 F1 F3 SUB.S,  F0 A1 52 LSI,  B0 F1 F0 OLT.S,
  B0 $11A L BT,
  A5 A4 72 L8UI,  A5 $A1 L BNEZ,
  A10 F0 RFR,  A8 $38 L L32R,  A8 CALLX8,
  A15 A11 MOV.N,  A13 $1C L L32R,  A11 1 MOVI.N,  A14 A10 MOV.N,  A12 4 MOVI.N,
  A11 A11 12 SLLI,  A10 6 MOVI,  A8 $3C L L32R,  A8 CALLX8,
  A5 1 MOVI.N,  $11C L J,
  1 ZEROS,
$11A L:
  A5 0 MOVI.N,
$11C L:
  A5 A4 72 S8I,  MEMW,
$122 L:
  A5 0 MOVI.N,  A5 A4 12 S32I.N,  A5 A4 8 S32I,
$129 L:
  A5 A4 36 L32I.N,  A5 0 $16A L BBCI,
  A14 A2 MOV.N,  A15 A2 MOV.N,  A2 $138 L BEQZ.N,
  A14 A2 0 L32I.N,  A15 A2 4 L32I.N,
$138 L:
  A11 1 MOVI.N,  A11 A11 12 SLLI,  A5 $10 L L32R,  A2 A5 80 L32I,  A2 A3 A2 SUB,
  A8 A5 84 L32I,  A5 1 MOVI.N,  A3 A2 $150 L BLTU,
  A5 0 MOVI.N,
$150 L:
  A8 A8 NEG,  A8 A8 A5 SUB,  A13 $20 L L32R,  A12 4 MOVI.N,  A10 6 MOVI.N,
  A2 A1 0 S32I.N,  A8 A1 4 S32I.N,  MEMW,  A8 $40 L L32R,  A8 CALLX8,
$16A L:
  A2 A4 36 L32I.N,  A2 1 $172 L BBSI,
  $21C L J,
$172 L:
  A2 A4 73 L8UI,  A5 $0 L L32R,  A2 $17D L BNEZ.N,
  A5 $4 L L32R,
$17D L:
  A2 1 MOVI.N,  A2 A2 12 SLLI,  A3 $10 L L32R,  A8 A3 $1D0 L32I,  A8 A1 24 S32I.N,
  A10 A4 44 L32I.N,  MEMW,  A8 $44 L L32R,  A8 CALLX8,
  A10 A1 16 S32I.N,  A11 A1 20 S32I.N,  A10 A4 52 L32I.N,  MEMW,  A8 $48 L L32R,
  A8 CALLX8,
  A8 A3 $100 ADDMI,  A12 3 MOVI.N,  A10 A1 8 S32I.N,  A14 A5 MOV.N,  A10 6 MOVI.N,
  A11 A1 12 S32I.N,  A11 A2 MOV.N,  A8 A8 124 L8UI,  A8 A8 10 SLLI,  A8 A1 0 S32I.N,
  A15 A3 $1CC L32I,  A13 $24 L L32R,  MEMW,  A8 $4C L L32R,  A8 CALLX8,
  A10 A4 40 L32I.N,  A8 $50 L L32R,  A8 CALLX8,
  A14 A10 MOV.N,  A3 A4 58 L16UI,  A3 A1 24 S32I.N,  A3 A4 57 L8UI,
  A3 A1 20 S32I.N,  A3 A4 56 L8UI,  A3 A1 16 S32I.N,  A10 A4 48 L32I.N,
  A14 A1 48 S32I.N,  A11 A1 52 S32I.N,  MEMW,  A8 $54 L L32R,  A8 CALLX8,
  A10 A1 8 S32I.N,  A11 A1 12 S32I.N,  A10 A4 68 L32I,  MEMW,  A8 $58 L L32R,
  A8 CALLX8,
  A12 4 MOVI.N,  A14 A1 48 L32I.N,  A15 A1 52 L32I.N,  A13 $28 L L32R,
  A10 A1 0 S32I.N,  A10 6 MOVI.N,  A11 A1 4 S32I.N,  A11 A2 MOV.N,  MEMW,
  A8 $5C L L32R,  A8 CALLX8,
$21C L:
  A2 0 MOVI.N,  A2 A4 36 S32I.N,
$220 L:
  MEMW,  RETW.N,
$225 L:  END-CODE

CODE empty_func_p_pvoid_r_void   \ .text.empty_func_p_pvoid_r_void $0
  $0 ORIGIN
  A1 32 ENTRY,  RETW.N,
$5 L:  END-CODE

CODE pm_beacon_offset_funcs_init   \ .text.pm_beacon_offset_funcs_init $48
  $0 18 LITERALS
    SYMBOL ptr_beacon_offset_funcs  SYMBOL pm_beacon_offset_sample_start
    SYMBOL pm_beacon_offset_sample  SYMBOL pm_beacon_offset_reset
    SYMBOL pm_beacon_offset_is_sampling  SYMBOL pm_beacon_offset_is_enabled
    SYMBOL pm_beacon_offset_configure  SYMBOL pm_beacon_offset_resample_~FE38
    SYMBOL pm_beacon_offset_set_rx_be~AA14  SYMBOL pm_beacon_offset_get_rx_be~A9A9
    SYMBOL pm_beacon_offset_add_total~1967  SYMBOL pm_beacon_offset_add_loss_~100A
    SYMBOL pm_beacon_offset_check  SYMBOL pm_beacon_offset_init
    SYMBOL pm_beacon_offset_deinit  SYMBOL pm_beacon_offset_get_average
    SYMBOL pm_beacon_offset_get_expect  SYMBOL pm_beacon_offset_get_params
$48 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A9 $4 L L32R,  A8 A8 0 L32I.N,  A9 A8 0 S32I.N,
  A9 $8 L L32R,  A9 A8 4 S32I.N,  A9 $C L L32R,  A9 A8 8 S32I.N,  A9 $10 L L32R,
  A9 A8 12 S32I.N,  A9 $14 L L32R,  A9 A8 16 S32I.N,  A9 $18 L L32R,
  A9 A8 20 S32I.N,  A9 $1C L L32R,  A9 A8 24 S32I.N,  A9 $20 L L32R,
  A9 A8 28 S32I.N,  A9 $24 L L32R,  A9 A8 32 S32I.N,  A9 $28 L L32R,
  A9 A8 36 S32I.N,  A9 $2C L L32R,  A9 A8 40 S32I.N,  A9 $30 L L32R,
  A9 A8 44 S32I.N,  A9 $34 L L32R,  A9 A8 48 S32I.N,  A9 $38 L L32R,
  A9 A8 52 S32I.N,  A9 $3C L L32R,  A9 A8 56 S32I.N,  A9 $40 L L32R,
  A9 A8 60 S32I.N,  A9 $44 L L32R,  A9 A8 64 S32I,  MEMW,  RETW.N,
$AB L:  END-CODE

CODE pm_beacon_offset_funcs_emp~83F5   \ pm_beacon_offset_funcs_empty_init: .text.pm_beacon_offset_funcs_empty_init $2C
  $0 11 LITERALS
    SYMBOL ptr_beacon_offset_funcs  SYMBOL empty_func_p_void_r_void
    SYMBOL empty_func_p_u32_r_void  SYMBOL empty_func_p_void_r_bool
    SYMBOL empty_func_p_pconfg_r_void  SYMBOL empty_func_p_pvoid_r_void
    SYMBOL empty_func_p_u8_r_void  SYMBOL empty_func_p_void_r_float
    SYMBOL empty_func_p_ptsf_u32_r_void  SYMBOL empty_func_p_u8_u8_pu16_pf~6606
    SYMBOL empty_func_p_u8_u8_float_f~0689
$2C L:
  A1 32 ENTRY,  A8 $0 L L32R,  A11 $C L L32R,  A8 A8 0 L32I.N,  A11 A8 12 S32I.N,
  A11 A8 16 S32I.N,  A11 $10 L L32R,  A11 A8 20 S32I.N,  A11 $14 L L32R,
  A9 $4 L L32R,  A11 A8 24 S32I.N,  A11 $18 L L32R,  A9 A8 0 S32I.N,
  A9 A8 8 S32I.N,  A11 A8 28 S32I.N,  A9 A8 36 S32I.N,  A11 $1C L L32R,
  A9 A8 40 S32I.N,  A9 A8 48 S32I.N,  A9 A8 52 S32I.N,  A9 $24 L L32R,
  A10 $8 L L32R,  A11 A8 32 S32I.N,  A9 A8 56 S32I.N,  A11 $20 L L32R,
  A9 $28 L L32R,  A10 A8 4 S32I.N,  A11 A8 44 S32I.N,  A9 A8 60 S32I.N,
  A10 A8 64 S32I,  MEMW,  RETW.N,
$7A L:  END-CODE

CODE pm_funcs_init   \ .text.pm_funcs_init $14
  $0 5 LITERALS
    SYMBOL g_osi_funcs_p  SYMBOL ptr_beacon_offset_funcs
    SYMBOL rodata_wlog_error.20  SYMBOL wifi_log
    SYMBOL pm_beacon_offset_funcs_init
$14 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A11 68 MOVI.N,  A10 1 MOVI,  A2 A2 0 L32I,
  A2 A2 $164 L32I,  A2 CALLX8,
  A2 $4 L L32R,  A10 A2 0 S32I.N,  A10 $48 L BNEZ.N,
  A12 1 MOVI.N,  A11 A12 MOV.N,  A10 6 MOVI.N,  A13 $8 L L32R,  A2 $101 MOVI,  MEMW,
  A8 $C L L32R,  A8 CALLX8,
  $50 L J,
  1 ZEROS,
$48 L:
  A8 $10 L L32R,  A8 CALLX8,
  A2 0 MOVI.N,
$50 L:
  RETW.N,
$52 L:  END-CODE

CODE pm_funcs_deinit   \ .text.pm_funcs_deinit $8
  $0 2 LITERALS
    SYMBOL ptr_beacon_offset_funcs  SYMBOL g_osi_funcs_p
$8 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A10 A2 0 L32I.N,  A10 $21 L BEQZ.N,
  A8 $4 L L32R,  A8 A8 0 L32I.N,  A8 A8 176 L32I,  A8 CALLX8,
  A8 0 MOVI.N,  A8 A2 0 S32I.N,
$21 L:
  A2 0 MOVI.N,  MEMW,  RETW.N,
$28 L:  END-CODE

LINK-SYMBOLS   \ the cells of other packages that wait for these names
END-PACKAGE  PREVIOUS   \ not in the search order: ALSO name gives its words (README.md)
