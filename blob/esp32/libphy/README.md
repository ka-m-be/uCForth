# libphy: the PHY of the ESP32 as uCForth packages

`libphy.a` is the physical layer (PHY) of the ESP32: calibration,
channel, transmit power, gain. This folder has its six objects as
uCForth packages, one for each `.S`. The README of the folder above
tells what all three libraries have in common: a package, the linker,
loading, the test, the tools.

## Status

- **Packages:** a `.fs` for each of the six `.S`. The two FPGA objects
  are empty; their packages too.
- **Tested on the host:** the code (63 457 bytes) and the data (3 185
  bytes) are those of the objects, byte for byte.
- **In ucf and on the board:** 170 of the 285 functions of
  `phy_chip_v7`, `phy_chip_v7_ana` and `phy_chip_v7_cal` are copied to
  ucf and linked in the package ESP32-PHY (section 5). Its entries run on
  the WROOM: calibration, channels, power, off and on. The other
  functions never ran.
- **In git:** `libphy.a`, the `.S`, the `.fs` and this README. The
  objects, the listings and `rewrite_rules.md` are not in git
  (`../README.md`, section 8).

## 1 The packages

| File | Package | Functions (global) | Data | Code |
| --- | --- | ---: | ---: | ---: |
| `phy.fs` | PHY-PHY | 3 (3) | 0 | 79 B |
| `phy_chip_v7.fs` | PHY-PHY-CHIP-V7 | 153 (149) | 50 | 34.7 KB |
| `phy_chip_v7_ana.fs` | PHY-PHY-CHIP-V7-ANA | 76 (74) | 26 | 14.5 KB |
| `phy_chip_v7_cal.fs` | PHY-PHY-CHIP-V7-CAL | 56 (56) | 24 | 15.2 KB |
| `phy_fpga_v7_cal.fs` | PHY-PHY-FPGA-V7-CAL | empty | | |
| `phy_fpga_v7_init.fs` | PHY-PHY-FPGA-V7-INIT | empty | | |

Global means PUBLIC: a symbol for the other packages (`.global`,
`.weak`, `.comm`); all others are PRIVATE. "Data" counts the data
objects and the sections without a symbol (texts, jump tables).
`set_xpd_sar` is weak in the object; in Forth it is a normal word.

Measured on the host: 65.1 KB of code (SRAM0), 14.9 KB of dictionary,
5.3 KB of RAM data in the worst order, of it 0.7 KB for the data of C.
The largest `CODE` word has 1 588 bytes (`bt_i2c_write_set`, with 47
jump targets).

## 2 Dependencies and load order

```text
PHY-PHY ──▶ PHY-PHY-CHIP-V7 ◀──▶ PHY-PHY-CHIP-V7-ANA
                   ▲                   ▲
                   ▼                   │
            PHY-PHY-CHIP-V7-CAL ───────┘
                   │
                   ▼
            RTC-RTC-ANALOG (librtc)
```

PHY-PHY-CHIP-V7, -ANA and -CAL need each other, functions and data (for
example `chip7_sleep_params+0x81`). Thus no order is without literals
that wait; `LINK-SYMBOLS` at the end of each package fills them. The
order with the fewest names that wait:

```text
phy_chip_v7_ana  phy_chip_v7_cal  phy_chip_v7  phy  phy_fpga_v7_cal  phy_fpga_v7_init
```

Before it: `../romsyms.fs` and `../idf.fs` (the names from outside).
After libphy: librtc. Until then, only `temprature_sens_read` waits.
librtc needs 7 names of libphy, libpp needs 10; the head of each
package names them.

## 3 What is missing

- **9 functions of the ROM** (`ets_delay_us`, `memcpy`, `__divdi3`,
  `sprintf`, `__divdf3`, `__divsf3`, `__floatunsidf`, `__truncdfsf2`,
  `phy_get_romfuncs`): their addresses are in `../romsyms.fs`, from the
  linker scripts of PlatformIO. The 7 that ESP32-PHY needs are in
  `lib/esp32/esp32/phyidf.fs` of ucf; 6 of them are also checked in the
  linker scripts of ESP-IDF. They run on the board.
- **`temprature_sens_read`** is in librtc (RTC-RTC-ANALOG). For
  ESP32-PHY it is in `lib/esp32/esp32/phyrtc.fs`.
- **5 functions of ESP-IDF** (`esp_dport_access_reg_read`,
  `phy_printf`, `phy_enter_critical`, `phy_exit_critical`,
  `rtc_get_xtal`): not in the project and not in the ROM. An untested
  proposal is in `../idf.fs`. For ESP32-PHY they are written as in the
  reference and tested on the board: `lib/esp32/esp32/phyidf.fs`.
- **The prototypes of C:** the number and the types of the arguments
  are not known (there are no headers of ESP-IDF in the project).
  `CALL-C` needs the number from the caller.
- **The init data and the sequence:** `register_chipv7_phy` expects
  `esp_phy_init_data_t` (128 bytes, `phy_init_data.h`) and calibration
  data, and ESP-IDF has the sequence around it (clocks and the reset of
  Wi-Fi and Bluetooth through DPORT). Without this, libphy does not
  start the radio. ESP32-PHY does it: `PHY-INIT` with the init data of
  `esp-phy` 0.2.0 and the sequence of the reference (FPU, clocks through
  DPORT, RTC_CNTL, `register_chipv7_phy`).
- **The version:** the archive is that of the reference
  (`esp-wifi-sys-esp32` 0.2.0, with the date "Oct 28 2025"); the
  reference runs with exactly these three archives.

## 4 Problems

| Problem | Result, what to do |
| --- | --- |
| `ram_rfpll_set_freq` needs the floating point coprocessor. | The HAL of ucf does not switch it on. Without `FPU-ON` (CLINK) there is the exception "Coprocessor 0 disabled", thus `THROW`. In ESP32-PHY, `PHY-INIT` calls `fpu_on` first. |
| `sprintf` of the ROM | uses the data of newlib in the ROM; it is not tested whether it runs without the preparation of ESP-IDF. Only `get_phy_version_str` uses it. |
| interrupts | During `SYSCALL`, the Forth handlers wait; `CODE` handlers and the console continue. Critical sections of the calibration need a `phy_enter_critical` that blocks up to level 3 (`../idf.fs`); for this time, Ctrl-C waits too. |
| the stack | `SYSCALL` uses the stack of the functions of the ROM, down from `$3FFF_FFC0`. A frame has up to 416 bytes (`entry a1, 0x1a0`). On the WROOM, the calibration, the start of the radio and a scan use at most 1 200 bytes of it (`targets/esp32/README.md`, "Memory"). |
| `FREEZE` | saves the data of C with their current values. Thus `FREEZE` only before a function of libphy ran. ESP32-PHY keeps a copy of the start values and puts it back before each `PHY-INIT`; there, `FREEZE` also works after a calibration. |
| only the ESP32 | libphy v7 is for the ESP32 (registers `$3FF4xxxx`, `$3FF5xxxx`). `CODE!` writes into SRAM0 with `!`. |

How to call a function: `../README.md`, section 2.

## 5 In ucf

`tools/pick.fs` took from three objects what the 8 entries of ESP32-PHY
need (the pick list `lib/esp32/esp32/phypick.fs`: `register_chipv7_phy`,
`phy_wakeup_init`, `phy_close_rf`, `chip_v7_set_chan_nomac`,
`disable_wifi_agc`, `enable_wifi_agc`, `tx_pwctrl_background`,
`phy_disable_low_rate`):

| Object | In ucf | Functions | Data objects |
| --- | --- | ---: | ---: |
| `phy_chip_v7.o` | `lib/esp32/esp32/phychip.fs` | 74 of 153 | 34 of 50 |
| `phy_chip_v7_ana.o` | `lib/esp32/esp32/phyana.fs` | 48 of 76 | 18 of 26 |
| `phy_chip_v7_cal.o` | `lib/esp32/esp32/phycal.fs` | 48 of 56 | 24 of 24 |
| `phy.o`, the two FPGA objects | nothing | | |

The data of all copies are in `lib/esp32/esp32/phydata.fs`. Each line is
that of the package here. Only `rodata` and `rodata.str1.4` are in all
three objects; in ucf they have a tag: `chip:rodata`, `ana:rodata`,
`cal:rodata` and so on. The head of each copy names the archive with
its SHA-256, and the license.
