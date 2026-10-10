\ pm.fs - package RTC-PM: pm.o of librtc.a (ESP32)
\
\ Made from pm.S by s2fs.py, not by hand. Each function of C is a
\ CODE word: its bytes are those of the compiler, at the same offsets of
\ its section (L gives the address of an offset); its literals are at the
\ start of the word, after a jump. The data come first, then the
\ functions, each after those that it calls. The global symbols are
\ PUBLIC, the static ones PRIVATE. lib/esp32/clink.fs explains the words.
\
\ 16 functions (16 global), 10 data objects.
\ A function runs as C code with the windowed calls of the Xtensa: call
\ it with CALL-C ( x1 .. xn n xt -- x ), never by its name alone.
\
\ Names of other packages: a literal waits until its package comes
\ (LINK-SYMBOLS); a name outside the libraries until EXTERN gives it.
\ Uses:
\   PHY-PHY-CHIP-V7  phy_close_rf
\ Outside the libraries (../README.md):
\   esp_dport_access_reg_read ets_delay_us rtc_printf
\   rtc_sleep_set_wakeup_time rtc_slowck_cali rtc_slp_prep rtc_time_get
\   uart_div_modify
\
\ Needs XTENSA (asm/xtensa.fs), ASM (lib/esp32/asm.fs) and CLINK
\ (lib/esp32/clink.fs) of ucf; the ESP32.

PACKAGE RTC-PM  ALSO ASM  ALSO CLINK   \ lib/package.fs
EXPORTS   \ the PUBLIC words are symbols for the other packages

\ ==== Data =============================================================

\ ---- COMMON (.comm): data of C without a start value
4 DATA: tsf_delay_us
1 DATA: hardware_reject
4 DATA: sleep_mode_flag

PRIVATE
CREATE rodata.str1.4   \ .rodata.str1.4, 79 bytes
  ," rom_ram_pd_en:%d"  10 C,  ," lslp_mem_force_pu:%d"  10 C,
  ," slowmem_force_pu:%d"  10 C,  ," fastmem_force_pu:%d"  10 C,  0 C,
PUBLIC
4 DATA: wakeup_tsfearly_fix   \ .bss.wakeup_tsfearly_fix
4 DATA: chip_version   \ .data.chip_version
  $FF  $FF  $FF  $FF  4 chip_version INIT
1 DATA: periodic_cal_sat   \ .bss.periodic_cal_sat
1 DATA: software_slp_reject   \ .bss.software_slp_reject
1 DATA: SDIO_slp_reject   \ .bss.SDIO_slp_reject
1 DATA: pptask_busy   \ .bss.pptask_busy

\ ==== Code =============================================================

CODE pm_wakeup_opt   \ .text.pm_wakeup_opt $10
  $0 4 LITERALS
    $3FF48038 T,  $FFC007FF T,  $3FF48064 T,  SYMBOL esp_dport_access_reg_read
$10 L:
  A1 32 ENTRY,  A4 $0 L L32R,  A2 A2 0 11 EXTUI,  A10 A4 MOV.N,  A8 $C L L32R,
  A8 CALLX8,
  A2 A2 11 SLLI,  A8 $4 L L32R,  A10 A10 A8 AND,  A10 A2 A10 OR,  A2 $8 L L32R,
  MEMW,  A10 A4 0 S32I.N,  MEMW,  A3 A2 0 S32I.N,  RETW.N,
$3C L:  END-CODE

CODE get_chip_version   \ .text.get_chip_version $4
  $0 1 LITERALS
    SYMBOL chip_version
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A2 A8 0 L32I.N,  RETW.N,
$E L:  END-CODE

CODE pm_set_sleep_cycles   \ .text.pm_set_sleep_cycles $10
  $0 4 LITERALS
    SYMBOL periodic_cal_sat  $1388 T,  SYMBOL rtc_time_get
    SYMBOL rtc_sleep_set_wakeup_time
$10 L:
  A1 32 ENTRY,  A8 $8 L L32R,  A8 CALLX8,
  A10 A2 A10 ADD.N,  A8 1 MOVI.N,  A10 A2 $22 L BLTU,
  A8 0 MOVI.N,
$22 L:
  A11 A8 A11 ADD.N,  A8 $C L L32R,  A8 CALLX8,
  A9 $4 L L32R,  A8 1 MOVI.N,  A9 A2 $34 L BLTU,
  A8 0 MOVI.N,
$34 L:
  A2 $0 L L32R,  A8 A2 0 S8I,  MEMW,  RETW.N,
$3F L:  END-CODE

CODE pm_rtc_clock_cali   \ .text.pm_rtc_clock_cali $4
  $0 1 LITERALS
    SYMBOL rtc_slowck_cali
$4 L:
  A1 32 ENTRY,  A11 128 MOVI,  A10 0 MOVI.N,  A8 $0 L L32R,  A8 CALLX8,
  A10 $34 L BEQZ.N,
  A8 A2 4 L32I.N,  A8 $32 L BEQZ.N,
  A9 A8 A10 SUB,  A8 A10 $21 L BGEU,
  A9 A10 A8 SUB,
$21 L:
  A11 $1FF MOVI,  A11 A9 $34 L BLTU,
  A8 A8 A8 ADDX2,  A10 A10 A10 ADDX4,  A10 A8 A10 ADD.N,  A10 A10 3 SRLI,
$32 L:
  A10 A2 4 S32I.N,
$34 L:
  RETW.N,
$36 L:  END-CODE

CODE pm_prepare_to_sleep   \ .text.pm_prepare_to_sleep $0
  $0 ORIGIN
  A1 32 ENTRY,  RETW.N,
$5 L:  END-CODE

CODE pm_sdio_nidle   \ .text.pm_sdio_nidle $0
  $0 ORIGIN
  A1 32 ENTRY,  A2 0 MOVI.N,  RETW.N,
$7 L:  END-CODE

CODE pm_goto_sleep   \ .text.pm_goto_sleep $24
  $0 9 LITERALS
    SYMBOL software_slp_reject  SYMBOL SDIO_slp_reject  $3FF48048 T,  $3FF48018 T,
    $80000000 T,  $3FF48040 T,  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
$24 L:
  A1 32 ENTRY,  A3 $0 L L32R,  A2 0 MOVI.N,  A4 $8 L L32R,  A10 A4 MOV.N,
  A2 A3 0 S8I,  MEMW,  NOP.N,  NOP.N,  NOP.N,  NOP.N,  A3 $4 L L32R,  A2 A3 0 S8I,
  MEMW,  A8 $18 L L32R,  A8 CALLX8,
  A2 3 MOVI.N,  A2 A10 A2 OR,  A3 $C L L32R,  A10 A3 A3 OR,  MEMW,  A2 A4 0 S32I.N,
  A8 $1C L L32R,  A8 CALLX8,
  A2 $10 L L32R,  A10 A10 A2 OR,  A2 $14 L L32R,  MEMW,  A10 A3 0 S32I.N,
$72 L:
  A10 A2 MOV.N,  A8 $20 L L32R,  A8 CALLX8,
  A10 A10 0 2 EXTUI,  A10 $72 L BEQZ,
  A2 0 MOVI.N,  RETW.N,
$84 L:  END-CODE

CODE pm_sleep_set_mac   \ .text.pm_sleep_set_mac $28
  $0 10 LITERALS
    $60033CB8 T,  $8000 T,  $6000 T,  $5FE0000 T,
    SYMBOL esp_dport_access_reg_read  SYMBOL ets_delay_us
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL ets_delay_us  SYMBOL esp_dport_access_reg_read
$28 L:
  A1 32 ENTRY,  A2 $0 L L32R,  A10 A2 MOV.N,  A8 $10 L L32R,  A8 CALLX8,
  A3 $4 L L32R,  A10 A10 A3 OR,  MEMW,  A10 A2 0 S32I.N,  NOP.N,  NOP.N,  NOP.N,
  NOP.N,  A3 $8 L L32R,  $59 L J,
  2 ZEROS,
$51 L:
  A10 2 MOVI.N,  A8 $14 L L32R,  A8 CALLX8,
$59 L:
  A10 A2 MOV.N,  A8 $18 L L32R,  A8 CALLX8,
  A10 A3 $51 L BANY,
  A10 A2 MOV.N,  A8 $1C L L32R,  A8 CALLX8,
  A8 $C L L32R,  A10 A10 A8 OR,  MEMW,  A10 A2 0 S32I.N,  $82 L J,
$7A L:
  A10 2 MOVI.N,  A8 $20 L L32R,  A8 CALLX8,
$82 L:
  A10 A2 MOV.N,  A8 $24 L L32R,  A8 CALLX8,
  A10 A3 $7A L BANY,
  RETW.N,
$8F L:  END-CODE

CODE pm_mac_deinit   \ .text.pm_mac_deinit $4
  $0 1 LITERALS
    SYMBOL pm_sleep_set_mac
$4 L:
  A1 32 ENTRY,  A8 $0 L L32R,  A8 CALLX8,
  RETW.N,
$F L:  END-CODE

CODE pm_set_wakeup_mac   \ .text.pm_set_wakeup_mac $C
  $0 3 LITERALS
    $60033CB8 T,  $FA017FFF T,  SYMBOL esp_dport_access_reg_read
$C L:
  A1 32 ENTRY,  A2 $0 L L32R,  A10 A2 MOV.N,  A8 $8 L L32R,  A8 CALLX8,
  A8 $4 L L32R,  A10 A10 A8 AND,  MEMW,  A10 A2 0 S32I.N,  RETW.N,
$27 L:  END-CODE

CODE pm_check_mac_idle   \ .text.pm_check_mac_idle $0
  $0 ORIGIN
  A1 32 ENTRY,  A2 0 MOVI.N,  RETW.N,
$7 L:  END-CODE

CODE pm_set_sleep_btco   \ .text.pm_set_sleep_btco $0
  $0 ORIGIN
  A1 32 ENTRY,  RETW.N,
$5 L:  END-CODE

CODE pm_set_wakeup_btco   \ .text.pm_set_wakeup_btco $0
  $0 ORIGIN
  A1 32 ENTRY,  RETW.N,
$5 L:  END-CODE

CODE pm_unmask_bt   \ .text.pm_unmask_bt $0
  $0 ORIGIN
  A1 32 ENTRY,  RETW.N,
$5 L:  END-CODE

CODE pm_mac_init   \ .text.pm_mac_init $C
  $0 3 LITERALS
    $60033CB8 T,  $FA017FFF T,  SYMBOL esp_dport_access_reg_read
$C L:
  A1 32 ENTRY,  A2 $0 L L32R,  A10 A2 MOV.N,  A8 $8 L L32R,  A8 CALLX8,
  A8 $4 L L32R,  A10 A10 A8 AND,  MEMW,  A10 A2 0 S32I.N,  RETW.N,
$27 L:  END-CODE

CODE pm_set_sleep_mode_full   \ .text.pm_set_sleep_mode_full $B4
  $0 45 LITERALS
    $3FF48088 T,  $10000000 T,  $3FF48084 T,  $20000 T,  $3FF48078 T,
    $FCFFFFFF T,  $3FF48000 T,  $FFFEFFFF T,  $3FF480B4 T,  $91A2B3C5 T,
    $3FF48080 T,  $2000 T,  SYMBOL rodata.str1.4  $C350 T,  SYMBOL phy_close_rf
    SYMBOL pm_sleep_set_mac  SYMBOL esp_dport_access_reg_read  SYMBOL ets_delay_us
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL rtc_slp_prep  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL uart_div_modify
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL rtc_printf  SYMBOL ets_delay_us  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL rtc_slp_prep
    SYMBOL esp_dport_access_reg_read  SYMBOL uart_div_modify
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read  SYMBOL rtc_printf  SYMBOL ets_delay_us
    SYMBOL esp_dport_access_reg_read  SYMBOL rtc_slp_prep  SYMBOL rtc_slp_prep
$B4 L:
  A1 80 ENTRY,  A8 $38 L L32R,  A8 CALLX8,
  A8 $3C L L32R,  A8 CALLX8,
  A2 64 $C9 L BNEI,
  $338 L J,
$C9 L:
  A8 64 MOVI.N,  A8 A2 $F0 L BLTU,
  A2 8 $D4 L BNEI,
  $37C L J,
$D4 L:
  A8 8 MOVI.N,  A8 A2 $E4 L BLTU,
  A2 4 $DF L BNEI,
  $34E L J,
$DF L:
  $3BA L J,
  2 ZEROS,
$E4 L:
  A2 16 $EA L BNEI,
  $390 L J,
$EA L:
  A2 32 $152 L BEQI,
  $3BA L J,
$F0 L:
  A2 $100 $F6 L BNEI,
  $234 L J,
$F6 L:
  A8 $100 MOVI,  A8 A2 $104 L BLTU,
  A2 128 $119 L BEQI,
  $3BA L J,
  2 ZEROS,
$104 L:
  A8 $200 MOVI,  A2 A8 $10D L BNE,
  $220 L J,
$10D L:
  A3 $400 MOVI,  A2 A3 $116 L BNE,
  $2FC L J,
$116 L:
  $3BA L J,
$119 L:
  A3 $0 L L32R,  A10 A3 A3 OR,  A8 $40 L L32R,  A8 CALLX8,
  A2 $4 L L32R,  A2 A10 A2 OR,  A10 5 MOVI.N,  MEMW,  A2 A3 0 S32I,  A8 $44 L L32R,
  A8 CALLX8,
  A3 $8 L L32R,  A10 A3 MOV.N,  A8 $48 L L32R,  A8 CALLX8,
  A2 $C L L32R,  A10 A10 A2 OR,  MEMW,  A10 A3 0 S32I.N,  $3BA L J,
$152 L:
  A3 $10 L L32R,  A2 0 MOVI.N,  A10 A3 A3 OR,  A8 $4C L L32R,  A8 CALLX8,
  A12 1 MOVI.N,  A9 $14 L L32R,  A8 4 MOVI.N,  A10 A10 A9 AND,  A15 A2 MOV.N,
  A14 A2 MOV.N,  MEMW,  A10 A3 0 S32I.N,  A13 A2 MOV.N,  A11 A12 MOV.N,
  A10 A2 MOV.N,  A8 A1 36 S32I.N,  A8 A1 32 S32I.N,  A8 A1 28 S32I.N,
  A8 A1 24 S32I.N,  A2 A1 20 S32I.N,  A2 A1 16 S32I.N,  A2 A1 12 S32I.N,
  A2 A1 8 S32I.N,  A2 A1 4 S32I,  A2 A1 0 S32I,  A8 $50 L L32R,  A8 CALLX8,
  A3 $18 L L32R,  A10 A3 A3 OR,  A8 $54 L L32R,  A8 CALLX8,
  A8 $1C L L32R,  A8 A10 A8 AND,  A10 $20 L L32R,  MEMW,  A8 A3 0 S32I.N,
  A8 $58 L L32R,  A8 CALLX8,
  A11 A10 16 SLLI,  A10 A2 MOV.N,  A2 $24 L L32R,  A11 A11 A2 MULUH,
  A11 A11 16 16 EXTUI,  A8 $5C L L32R,  A8 CALLX8,
  A2 $8 L L32R,  A10 A2 MOV.N,  A8 $60 L L32R,  A8 CALLX8,
  A3 A10 A10 OR,  A10 A2 A2 OR,  A8 $64 L L32R,  A8 CALLX8,
  A2 $28 L L32R,  A4 A10 MOV.N,  A10 A2 A2 OR,  A8 $68 L L32R,  A8 CALLX8,
  A5 A10 MOV.N,  A10 A2 A2 OR,  A8 $6C L L32R,  A8 CALLX8,
  A14 $2C L L32R,  A13 A5 16 1 EXTUI,  A14 A10 A14 AND,  A14 A14 13 SRLI,
  A12 A4 4 1 EXTUI,  A10 $30 L L32R,  A11 A3 24 6 EXTUI,  A8 $70 L L32R,  A8 CALLX8,
  A10 $34 L L32R,  A8 $74 L L32R,  A8 CALLX8,
  $3BA L J,
$220 L:
  A3 $10 L L32R,  A2 0 MOVI.N,  A10 A3 MOV.N,  A8 $78 L L32R,  A8 CALLX8,
  A8 4 MOVI.N,  $30B L J,
  2 ZEROS,
$234 L:
  A3 $10 L L32R,  A2 0 MOVI.N,  A10 A3 MOV.N,  A8 $7C L L32R,  A8 CALLX8,
  A8 7 MOVI.N,  A9 $14 L L32R,  A12 1 MOVI.N,  A10 A10 A9 AND,  A14 A2 MOV.N,
  A13 A2 MOV.N,  MEMW,  A10 A3 0 S32I.N,  A15 A2 MOV.N,  A10 A2 MOV.N,
  A11 A12 MOV.N,  A8 A1 36 S32I.N,  A8 A1 32 S32I.N,  A8 A1 28 S32I.N,
  A8 A1 24 S32I.N,  A2 A1 20 S32I.N,  A2 A1 16 S32I.N,  A2 A1 12 S32I.N,
  A2 A1 8 S32I.N,
$26A L:
  A2 A1 4 S32I.N,  A2 A1 0 S32I.N,  A8 $80 L L32R,  A8 CALLX8,
  A10 $20 L L32R,  A8 $84 L L32R,  A8 CALLX8,
  A11 A10 16 SLLI,  A10 A2 MOV.N,  A2 $24 L L32R,  A11 A11 A2 MULUH,
  A11 A11 16 16 EXTUI,  A8 $88 L L32R,  A8 CALLX8,
  A4 $18 L L32R,  A10 A4 MOV.N,  A8 $8C L L32R,  A8 CALLX8,
  A2 $1C L L32R,  A2 A10 A2 AND,  A3 $8 L L32R,  A10 A3 MOV.N,  MEMW,
  A2 A4 0 S32I.N,  A8 $90 L L32R,  A8 CALLX8,
  A4 A10 MOV.N,  A10 A3 A3 OR,  A8 $94 L L32R,  A8 CALLX8,
  A2 $28 L L32R,  A3 A10 MOV.N,  A10 A2 MOV.N,  A8 $98 L L32R,  A8 CALLX8,
  A5 A10 MOV.N,  A10 A2 A2 OR,  A8 $9C L L32R,  A8 CALLX8,
  A14 $2C L L32R,  A13 A5 16 1 EXTUI,  A14 A10 A14 AND,  A14 A14 13 SRLI,
  A12 A3 4 1 EXTUI,  A10 $30 L L32R,  A11 A4 24 6 EXTUI,  A8 $A0 L L32R,  A8 CALLX8,
  A10 $34 L L32R,  A8 $A4 L L32R,  A8 CALLX8,
  $3BA L J,
$2FC L:
  A3 $10 L L32R,  A2 0 MOVI.N,  A10 A3 MOV.N,  A8 $A8 L L32R,  A8 CALLX8,
  A8 7 MOVI.N,
$30B L:
  A9 $14 L L32R,  A14 A2 MOV.N,  A10 A10 A9 AND,  A13 A2 MOV.N,  A12 A2 MOV.N,
  MEMW,  A10 A3 0 S32I.N,  A3 1 MOVI.N,  A11 A2 MOV.N,  A10 A2 MOV.N,  A15 A2 MOV.N,
  A8 A1 36 S32I.N,  A8 A1 32 S32I.N,  A8 A1 28 S32I.N,  A8 A1 24 S32I.N,
  A2 A1 20 S32I.N,  A2 A1 16 S32I.N,  A2 A1 12 S32I.N,  A3 A1 8 S32I.N,  $26A L J,
  1 ZEROS,
$338 L:
  A2 0 MOVI.N,  A12 1 MOVI.N,  A2 A1 36 S32I.N,  A15 A2 MOV.N,  A14 A2 MOV.N,
  A13 A2 MOV.N,  A11 A12 MOV.N,  A2 A1 32 S32I.N,  $3A0 L J,
  3 ZEROS,
$34E L:
  A8 0 MOVI.N,  A2 A1 24 S32I.N,  A2 1 MOVI.N,  A15 A8 MOV.N,  A14 A8 MOV.N,
  A13 A8 MOV.N,  A8 A1 36 S32I.N,  A12 A8 MOV.N,  A11 A8 MOV.N,  A10 A8 MOV.N,
  A8 A1 32 S32I.N,  A8 A1 28 S32I.N,  A8 A1 20 S32I.N,  A8 A1 16 S32I.N,
  A8 A1 12 S32I.N,  A8 A1 8 S32I.N,  A2 A1 4 S32I.N,  A8 A1 0 S32I.N,
  A8 $AC L L32R,  A8 CALLX8,
  $3BA L J,
  1 ZEROS,
$37C L:
  A2 0 MOVI.N,  A2 A1 36 S32I.N,  A15 A2 MOV.N,  A14 A2 MOV.N,  A13 A2 MOV.N,
  A12 A2 MOV.N,  A2 A1 32 S32I.N,  A11 A2 MOV.N,  $3A0 L J,
  1 ZEROS,
$390 L:
  A2 0 MOVI.N,  A2 A1 36 S32I.N,  A15 A2 MOV.N,  A14 A2 MOV.N,  A13 A2 MOV.N,
  A12 A2 MOV.N,  A2 A1 32 S32I.N,  A11 1 MOVI.N,
$3A0 L:
  A3 4 MOVI.N,  A10 A2 MOV.N,  A2 A1 28 S32I.N,  A3 A1 24 S32I.N,  A2 A1 20 S32I.N,
  A2 A1 16 S32I.N,  A2 A1 12 S32I.N,  A2 A1 8 S32I.N,  A2 A1 4 S32I.N,
  A2 A1 0 S32I.N,  A8 $B0 L L32R,  A8 CALLX8,
$3BA L:
  A2 0 MOVI.N,  RETW.N,
$3BE L:  END-CODE

LINK-SYMBOLS   \ the cells of other packages that wait for these names
END-PACKAGE  PREVIOUS   \ not in the search order: ALSO name gives its words (README.md)
