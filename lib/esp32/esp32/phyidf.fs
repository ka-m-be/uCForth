\ phyidf.fs - the 12 functions outside the code of C of ESP32-PHY
\
\ The functions in phychip.fs, phyana.fs, phycal.fs and phyrtc.fs call
\ 12 functions that are not in these files (their heads;
\ archive/plan/002_Plan_Fassung73.md, section 50.5): 5 that ESP-IDF has, and 7 in the ROM.
\
\ The 5 of ESP-IDF are here as CODE words of C: ENTRY, at the start and
\ RETW.N, at the end, the windowed calls of the Xtensa. They are written
\ for uCForth and do what the reference does for one core without an
\ operating system (esp-phy 0.2.0, common_adapter.rs):
\   esp_dport_access_reg_read ( reg -- x )   read a register of the chip
\   phy_enter_critical ( -- ps )   interrupts off up to level 3; the old PS
\   phy_exit_critical ( ps -- )    PS as before
\   rtc_get_xtal ( -- 40 )         the crystal of the board has 40 MHz
\   phy_printf ( fmt ... -- 0 )    a text of the code of C: we show none
\ Then fpu_on ( -- ): the floating point coprocessor on (CPENABLE 1).
\ The code of C calculates with it; PHY-INIT calls fpu_on first.
\
\ The 7 functions in the ROM get their addresses with EXTERN. The
\ addresses are from the linker scripts esp32.rom*.ld of ESP-IDF, as
\ blob/esp32/romsyms.fs has them; the ROM of the chip does not change.
\
\ The file loads in the package PHY-C, with XTENSA, ASM and CLINK
\ (lib/esp32/esp32/phy.fs).

CODE esp_dport_access_reg_read   \ ( reg -- x )
  A1 32 ENTRY,  MEMW,  A2 A2 0 L32I.N,  RETW.N,
END-CODE
CODE phy_enter_critical   \ ( -- ps )
  A1 32 ENTRY,  A2 3 RSIL,  RETW.N,
END-CODE
CODE phy_exit_critical   \ ( ps -- )
  A1 32 ENTRY,  A2 PS WSR,  RSYNC,  RETW.N,
END-CODE
CODE rtc_get_xtal   \ ( -- 40 )
  A1 32 ENTRY,  A2 40 MOVI.N,  RETW.N,
END-CODE
CODE phy_printf   \ ( fmt ... -- 0 )
  A1 32 ENTRY,  A2 0 MOVI.N,  RETW.N,
END-CODE
CODE fpu_on   \ ( -- )
  A1 32 ENTRY,  A2 1 MOVI.N,  A2 CPENABLE WSR,  RSYNC,  RETW.N,
END-CODE

$40008534 EXTERN ets_delay_us    \ esp32.rom.ld
$40004100 EXTERN phy_get_romfuncs   \ esp32.rom.ld
$4000C2C8 EXTERN memcpy          \ esp32.rom.newlib-funcs.ld
$40002954 EXTERN __divdf3        \ esp32.rom.libgcc.ld
$4000234C EXTERN __divsf3        \ esp32.rom.libgcc.ld of the Arduino core 1.0.4
$4000C938 EXTERN __floatunsidf   \ esp32.rom.libgcc.ld
$40002B90 EXTERN __truncdfsf2    \ esp32.rom.libgcc.ld
