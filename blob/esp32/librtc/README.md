# librtc: the RTC part of the ESP32 as uCForth packages

`librtc.a` is the RTC part of the ESP32 from the time of ESP-IDF 3:
clocks and voltages at the start (`rtc_init_full`), the RTC pads, ADC,
DAC, the Hall sensor and the temperature sensor, sleep modes and wakeup
(`pm_*`), the baseband of Bluetooth (`bt_bb`). This folder has its
seven objects as uCForth packages, one for each `.S`. The README of the
folder above tells what all three libraries have in common: a package,
the linker, loading, the test, the tools.

## Status

- **Packages:** a `.fs` for each of the seven `.S`; `eagle.app` is
  empty, its package too.
- **Instructions:** `asm/xtensa.fs` has all 48 mnemonics of the 6 543
  instructions.
- **Tested on the host:** the code (23 083 bytes) and the data (252
  bytes) are those of the objects, byte for byte.
- **In ucf:** `temprature_sens_read` of RTC-RTC-ANALOG, the one function
  of librtc that libphy needs, is copied to ucf and linked in the package
  ESP32-PHY (section 4). No other function of librtc ran on the board.
- **In git:** `librtc.a`, the `.S`, the `.fs` and this README. The
  objects, the listings and an older version of the tools are not in
  git (`../README.md`, section 8).

## 1 The packages

| File | Package | Functions (global) | Data | Code | Content |
| --- | --- | ---: | ---: | ---: | --- |
| `bt_bb.fs` | RTC-BT-BB | 15 (15) | 2 | 3.4 KB | the baseband of Bluetooth, coexistence (`bt_bb_init_cmplx`, `force_bt_mode`, `rw_coex_on` …) |
| `pm.fs` | RTC-PM | 16 (16) | 10 | 1.6 KB | sleep and wakeup (`pm_goto_sleep`, `pm_set_sleep_mode_full`, `pm_rtc_clock_cali` …), `get_chip_version` |
| `rtc.fs` | RTC-RTC | 24 (24) | 0 | 10.4 KB | the start of the RTC (`rtc_init_full`), pads (`rtc_pads_*`), wakeup sources, RF on and off |
| `rtc_analog.fs` | RTC-RTC-ANALOG | 19 (19) | 2 | 6.0 KB | ADC (`adc1_read` …), DAC (`dac_out`), Hall sensor, `temprature_sens_read`, `get_vdd33` |
| `rtc_cntl.fs` | RTC-RTC-CNTL | 4 (4) | 0 | 0.5 KB | power save of the digital parts, the sleep timer of the SAR |
| `rtc_mac.fs` | RTC-RTC-MAC | 2 (2) | 1 | 1.4 KB | `rtc_mac_tx`, `rtc_mac_tx_init` |
| `eagle.app.fs` | RTC-EAGLE-APP | empty | | | |

All 80 functions are global, thus PUBLIC. Measured on the host: 23.6 KB
of code (SRAM0), 3.8 KB of dictionary, 1.4 KB of RAM data, of it 37
bytes for the data of C. The largest `CODE` word has 1 366 bytes
(`rtc_init_full`).

In librtc there is no circle: only RTC-RTC-ANALOG needs RTC-RTC. With
libphy, librtc is in a circle: librtc needs 7 names of libphy, and
libphy (`txpwr_offset` in PHY-PHY-CHIP-V7-CAL) needs
`temprature_sens_read` of RTC-RTC-ANALOG. The recommended order, after
libphy:

```text
bt_bb  pm  rtc  rtc_analog  rtc_cntl  rtc_mac  eagle.app
```

Before it: `../romsyms.fs` and `../idf.fs`. If librtc comes before
libphy, the literals for the 7 names wait until libphy is there; this
works too. The head of each package names the packages and names that
it needs.

## 2 What is missing

- **2 functions of the ROM:** `ets_delay_us`, `uart_div_modify`. Their
  addresses are in `../romsyms.fs`, from the linker scripts of
  PlatformIO.
- **`esp_dport_access_reg_read`** (534 literals) and `rtc_printf`: they
  come from ESP-IDF and are not in the project. An untested proposal is
  in `../idf.fs` (`rtc_printf` = `ets_printf` of the ROM).
- **6 functions of ESP-IDF for the RTC:** `rtc_slp_prep`,
  `rtc_init_clk`, `rtc_slowck_cali`, `rtc_time_get`,
  `rtc_sleep_set_wakeup_time`, `rtc_dbias_cfg`. They are not in the
  project and not in the ROM. They are part of the RTC of ESP-IDF
  (`rtc_clk.c`, `rtc_time.c`, `rtc_sleep.c` and older versions of them).
- **The prototypes of C:** the number and the types of the arguments
  are not known; `CALL-C` needs the number from the caller.
- **The version:** librtc is built with GCC 5.2, as libphy; libpp with
  GCC 11.2. The archives are those of the reference
  (`esp-wifi-sys-esp32` 0.2.0); the reference runs with exactly these
  three archives.

## 3 Problems

| Problem | Result |
| --- | --- |
| clocks and voltages | `rtc_init_full`, the functions `pm_*` and `rtc_cntl` change clocks, voltages and sleep modes. The HAL of ucf and the package CLOCK (`lib/esp32/esp32/clock.fs`) set the clock themselves; UART0 and the timers count the clock of the APB. A call can change the speed of the console and of `MS`. |
| sleep | `pm_goto_sleep` and related functions put the chip to sleep. It is not tested whether ucf continues after it (clocks, UART0, timers, interrupts). |
| ADC, DAC, pads | The functions write the registers of the RTC pads directly. Pins that ucf (GPIO) uses already change too. |
| `FREEZE` | saves the data of C with their current values; thus only before a function ran. |

How to call a function: `../README.md`, section 2. Whether a function
gives a useful value without the rest of the initialization is not
tested.

## 4 In ucf

`tools/pick.fs` took one function of librtc, because libphy calls it
(the pick list `lib/esp32/esp32/phypick.fs`):

| Object | In ucf | Functions | Data objects |
| --- | --- | ---: | ---: |
| `rtc_analog.o` | `lib/esp32/esp32/phyrtc.fs`: `temprature_sens_read` | 1 of 19 | 0 of 2 |
| the other six | nothing | | |

Its lines are those of the package here; the head of the copy names
the archive with its SHA-256, and the license.
