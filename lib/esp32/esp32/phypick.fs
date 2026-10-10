\ phypick.fs - the pick list of ESP32-PHY: which functions of blob/esp32
\
\ The radio of the ESP32 needs a few functions of the libraries of
\ Espressif (archive/plan/002_Plan_Fassung73.md, section 50). The quarry blob/esp32 has
\ them as packages of CODE words. tools/pick.fs reads the package files
\ that this list names. In the folder ucf:
\   build/ucf-c -32 host/files.fs lib/esp32/esp32/phypick.fs </dev/null

S" tools/pick.fs" INCLUDED

\         package file of the quarry              tag     file for its functions
PICK-FROM blob/esp32/libphy/phy_chip_v7.fs       chip    lib/esp32/esp32/phychip.fs
PICK-FROM blob/esp32/libphy/phy_chip_v7_ana.fs   ana     lib/esp32/esp32/phyana.fs
PICK-FROM blob/esp32/libphy/phy_chip_v7_cal.fs   cal     lib/esp32/esp32/phycal.fs
PICK-FROM blob/esp32/librtc/rtc_analog.fs        rtc     lib/esp32/esp32/phyrtc.fs

\ The names outside the package files: 7 functions of the ROM, and 5
\ that ESP-IDF has and we write ourselves (lib/esp32/esp32/phyidf.fs).
PICK-OUTSIDE ets_delay_us memcpy phy_get_romfuncs
PICK-OUTSIDE __divdf3 __divsf3 __floatunsidf __truncdfsf2
PICK-OUTSIDE esp_dport_access_reg_read phy_enter_critical phy_exit_critical
PICK-OUTSIDE rtc_get_xtal phy_printf

\ The functions that ESP32-PHY calls (archive/plan/002_Plan_Fassung73.md, section 50.5).
PICK-ENTRY register_chipv7_phy phy_wakeup_init phy_close_rf
PICK-ENTRY chip_v7_set_chan_nomac disable_wifi_agc enable_wifi_agc
PICK-ENTRY tx_pwctrl_background phy_disable_low_rate

\ The copies: the data go to phydata.fs; the heads name this list.
PICK-DATA lib/esp32/esp32/phydata.fs
PICK-FOR ESP32-PHY lib/esp32/esp32/phypick.fs
PICK-LICENSE Copyright Espressif Systems (Shanghai) Co., Ltd., Apache License 2.0
PICK-LICENSE (blob/esp32/LICENSE). blob/esp32/s2fs.py rewrote the code of the
PICK-LICENSE objects as CODE words for the assembler XTENSA (blob/esp32/README.md).

\ The archives of the reference (archive/plan/002_Plan_Fassung73.md, section 50.3).
PICK-SHA256 blob/esp32/libphy/libphy.a c2521071695734afa8f6f1ca3d3b604a1d08dd63fe52060449a11cfe8bac5740
PICK-SHA256 blob/esp32/librtc/librtc.a e3411aeeb48d1da8acd304808290196baa0d6a2397db1a0c7741abee85dade61

PICK-RUN
PICK-WRITE
