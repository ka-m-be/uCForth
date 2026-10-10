\ idf.fs - package IDF: a proposal for some functions of ESP-IDF that the libraries call
\
\ libphy, libpp and librtc call 46 functions and data of ESP-IDF that are
\ neither in this project nor in the ROM (README.md of this folder,
\ section 6). This package has 6 of them: those that are short, for one
\ core without an RTOS, as ESP-IDF has them (from memory of ESP-IDF; the
\ project has no source for them). NOT TESTED on a board. The bytes equal
\ those of the GNU assembler.
\
\   esp_dport_access_reg_read ( reg -- x )   read a register of DPORT, with
\                                the workaround of ESP-IDF for the DPORT error
\   phy_enter_critical ( -- ps )  interrupts off up to level 3; the old PS
\   phy_exit_critical ( ps -- )   PS as before
\   rtc_get_xtal ( -- 40 )        the crystal: 40 MHz (boards with WROOM-32)
\   phy_printf rtc_printf         = ets_printf of the ROM (EXTERN)
\
\ All others are missing: above all the table of the functions of the
\ operating system (g_osi_funcs_p) for libpp, and the functions of
\ libnet80211, of the Wi-Fi driver and of the mesh (README.md).
\
\ Needs CLINK (clink.fs) and ASM (lib/esp32/asm.fs of ucf).

PACKAGE IDF  ALSO ASM  ALSO CLINK   \ lib/package.fs
EXPORTS   \ the PUBLIC words are symbols for the packages of the libraries

CODE esp_dport_access_reg_read   \ ( reg -- x )
  $0 1 LITERALS  $3FF40078 T,
$4 L:
  A1 32 ENTRY,  A4 5 RSIL,  A3 $0 L L32R,  A3 A3 0 L32I,  A2 A2 0 L32I,
  A4 PS WSR,  RSYNC,  RETW.N,
END-CODE
CODE phy_enter_critical   \ ( -- ps )
  $0 ORIGIN  A1 32 ENTRY,  A2 3 RSIL,  RETW.N,
END-CODE
CODE phy_exit_critical   \ ( ps -- )
  $0 ORIGIN  A1 32 ENTRY,  A2 PS WSR,  RSYNC,  RETW.N,
END-CODE
CODE rtc_get_xtal   \ ( -- 40 )
  $0 ORIGIN  A1 32 ENTRY,  A2 40 MOVI.N,  RETW.N,
END-CODE

$40007D54 EXTERN phy_printf   \ ets_printf of the ROM (esp32.rom.ld)
$40007D54 EXTERN rtc_printf

END-PACKAGE
