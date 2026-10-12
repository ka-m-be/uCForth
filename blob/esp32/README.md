# blob/esp32: the libraries of the ESP32 as uCForth packages

This folder (the quarry) has three precompiled libraries of Espressif
for the ESP32 (Xtensa LX6): `libphy` (the radio: calibration, channels,
power), `libpp` (the lower MAC layer of Wi-Fi) and `librtc` (clocks,
sleep, ADC and sensors). Each object of an archive is a uCForth package
(`.fs`), and each function of C is a `CODE` word for the assembler
XTENSA (`asm/xtensa.fs`). Only the global symbols (the names that a
linker sees) are PUBLIC; all static ones are PRIVATE.

The folder has two uses:

- It is an example: how uCForth uses a binary library of another
  system.
- It is the source of the radio of the ESP32. `tools/pick.fs` copies
  from here only the functions that the package ESP32-PHY needs
  (section 7). For the radio, use ESP32-PHY (`lib/esp32/README.md`,
  "The radio of the ESP32: ESP32-PHY"). Do not load the full libraries.

This README is about all three libraries. The README in `libphy/`,
`libpp/` and `librtc/` is about one library.

## Status

- 43 packages, one for each object. 14 of them are empty, because their
  objects are empty.
- On the host, the code (169 715 bytes) and the data (15 594 bytes) of
  all packages are the same as in the objects, byte for byte
  (section 6).
- On the board, only the functions in ESP32-PHY ran: 171 functions of
  libphy and librtc, on the WROOM. No other function ran.
- The three libraries have 175 KB of code. SRAM0 has 118 KB free for
  `CODE` words. Thus all three do not fit at the same time (section 4).
- 40 names are outside the three libraries and outside the ROM. Most of
  them are the interface of ESP-IDF to its operating system, for libpp
  (section 5).

**Origin and license:** The archives are those of the crate
`esp-wifi-sys-esp32` 0.2.0 (checked with SHA-256). Copyright Espressif
Systems (Shanghai) Co., Ltd., Apache License 2.0. The text of the
license is in `LICENSE` in this folder; the file `LICENSE` in the top folder
names the files that it covers.

## 1 The files

| File | Content | In git |
| --- | --- | --- |
| `libphy/`, `libpp/`, `librtc/` | for each library: the archive `.a`, the assembler sources `.S`, the packages `.fs`, a README | yes |
| `romsyms.fs` | the addresses of the 21 functions of the ROM that the libraries call (`EXTERN`) | yes |
| `idf.fs` | package IDF: an **untested** proposal for 6 short functions of ESP-IDF. The 5 that the radio needs are in `lib/esp32/esp32/phyidf.fs`, tested on the board | yes |
| `LICENSE` | the Apache License 2.0 of the three libraries | yes |
| `tools/`, `test/`, `s2fs.py`, the scripts in `libpp/` and `librtc/`, `libphy/rewrite_rules.md`; in each library folder the objects `.o`, the listings `.dis` and `.info` | the tools that made and tested the packages, and their intermediate files (section 8) | no |

A tool made the `.fs` files. Do not change them by hand: change the
tool and make them again (section 8).

## 2 A package

`libXY/NAME.S` becomes `libXY/NAME.fs` with the package `XY-NAME`. The
name is in upper case, and `_` and `.` become `-`: `phy_chip_v7.S` gives
`PHY-PHY-CHIP-V7`, `eagle.app.S` gives `RTC-EAGLE-APP`. The prefix is
necessary, because `pm.o` is in libpp and in librtc.

A package has this structure:

```forth
\ head: origin, number of functions and data, the names of other
\ packages and from outside, the long names of C
PACKAGE PP-LMAC  ALSO ASM  ALSO CLINK   \ lib/package.fs
EXPORTS          \ the PUBLIC words are symbols for the other packages
\ ==== Data      COMMON, .bss, .data (DATA:, INIT), .rodata (CREATE C, ,")
\ ==== Code      a CODE word for each function, after the functions that it calls with call8
\ ==== Addresses in the data     pointers in the data (SYMBOL!)
LINK-SYMBOLS     \ the literals of other packages that wait for names of this package
END-PACKAGE  PREVIOUS
```

A function (from `libphy/phy_chip_v7.fs`):

```forth
CODE phy_wifi_enable_set   \ .iram1 $860
  $854 3 LITERALS
    $3FF5C080 T,  SYMBOL esp_dport_access_reg_read
    SYMBOL esp_dport_access_reg_read
$860 L:
  A1 32 ENTRY,  A2 A2 0 8 EXTUI,  A2 $885 L BEQZ,
  A3 $854 L L32R,  A2 -193 MOVI,  A10 A3 A3 OR,  A8 $858 L L32R,  A8 CALLX8,
  A10 A10 A2 AND,  MEMW,  A10 A3 0 S32I.N,  RETW.N,
  3 ZEROS,
$885 L:
  A2 $854 L L32R,  A10 A2 MOV.N,  A8 $85C L L32R,  A8 CALLX8,
  A8 192 MOVI,  A10 A10 A8 OR,  MEMW,  A10 A2 0 S32I.N,  RETW.N,
$89D L:  END-CODE
```

The rules of the conversion:

| In the `.S` | In the package |
| --- | --- |
| a function with a literal pool | `CODE f`; in the next line `$off n LITERALS` (a `j` over the literals), the literals, `$start L:` |
| a function without literals | `CODE f`; in the next line `$start ORIGIN` |
| the end of the function (`.size`) | in the last line `$end L:  END-CODE` (it checks the length) |
| an instruction `op a, b, c` | `A B C OP,` (the operands as in GNU as, then the instruction with a comma) |
| a jump target `.Lsec_S_O` | `$O L` (the address of offset O in the original section); `$O L:` checks the position when the package loads |
| `call8 f` | `FUNC f CALL8,` |
| `wsr.ps a10` | `A10 PS WSR,` |
| `movi a8, 0xfffffeff` | `A8 -257 MOVI,` |
| `.byte 0x00` (filler after a jump) | `n ZEROS,` |
| `.literal .Lx, 0x3ff5c030` / `sym+0x81` | `$3FF5C030 T,` / `$81 SYMBOL+ sym` |
| `.comm x, n, a` / `.bss` | `n DATA: x` (RAM data, filled with 0; before it `8 DATA-ALIGN` for an alignment of 8) |
| `.data` | `n DATA: x` and `bytes n x INIT` (in parts of 16 bytes) |
| `.rodata` | `CREATE x` with `C,` and `," text"` (in the dictionary; read only after `FREEZE`) |
| `.word` in a jump table | `0 ,`; at the end of the function `table  $211 L!+ ... DROP` |
| `.word sym` (a pointer) | `0 ,` or 0 in `INIT`; at the end of the package `0 x SYMBOL! sym` |
| `.global`, `.weak`, `.comm` | PUBLIC; all others PRIVATE |
| a static function without a symbol | PRIVATE, with the name of its section (`.text.foo` gives `foo`) |
| a section without a symbol | PRIVATE, with the name of the section (`.rodata.str1.4` gives `rodata.str1.4`) |
| a name of C with more than 31 characters | the first 26 characters, `~` and 4 hex digits of its CRC-32: `pm_connectionless_wake_window_timeout_process` gives `pm_connectionless_wake_win~1498`. The same everywhere, also for `EXTERN`. 67 names of libpp have this form; the head of each package names its long names. |
| `.reloc …, R_XTENSA_ASM_EXPAND` | not used (only a hint for the linker) |

After it loads, a package is **not** in the search order (`END-PACKAGE
PREVIOUS`). This is different from ucf, where a package stays in the
search order: with 43 packages, the 16 places would be full. `ALSO
PP-LMAC` gives the words of a package.

A function of C is not a primitive of the VM. **Do not execute it by
its name:** the VM jumps into `entry`, the register window rotates,
and the chip crashes. Call it only with `CALL-C ( x1 .. xn n xt -- x )`,
through `SYSCALL`, with at most 13 arguments:

```forth
ALSO CLINK  ALSO PHY-PHY  FPU-ON
6 0 2 ' RFChannelSel CALL-C DROP   \ only the notation: without initialization this does nothing useful
```

How this looks on the board with initialization: ESP32-PHY in ucf
(`lib/esp32/README.md`, "The radio of the ESP32: ESP32-PHY"). It calls
its functions through `SYSCALL`, without their names.

### What `tools/pick.fs` reads

PICK reads the packages of this folder, not the `.S`. Thus the packages
must keep this form:

- Line 1 names the object: `… - package PHY-PHY-CHIP-V7-ANA:
  phy_chip_v7_ana.o of libphy.a (ESP32)`.
- The head has the numbers: `\ 76 functions (74 global), 26 data
  objects.` (also on two lines).
- `CODE name`; in the next line `$off n LITERALS` or `$off ORIGIN`; in
  the last line `$end L:  END-CODE`.
- `n DATA: name`, if necessary with `8 DATA-ALIGN` before it, then its
  lines with `INIT`.
- `CREATE name`, then its lines, each with a space at the start.
- `0 x SYMBOL! name` (also `0 x $off + SYMBOL! name`) for pointers in
  the data; `PUBLIC`, `PRIVATE` and `EXPORTS` for the visibility.

Each other line stops PICK, with the file and the line.

## 3 The linker: CLINK

The objects refer to each other, also in a circle (section 4), and to
names outside. Thus a literal does not hold a fixed value, but `SYMBOL
name`. CLINK (`lib/esp32/clink.fs`) looks for a name first in the
package itself (PUBLIC and PRIVATE), then in the PUBLIC words of all
packages with `EXPORTS` and in the names of `EXTERN`. It never looks in
the rest of the search order.

- **The name is known:** the literal gets the address at once: `>CODE`
  for a function, the chip address (`>CHIP`) for data.
- **The name is not known yet:** the literal waits. An entry in the RAM
  data remembers the place (12 bytes for each literal, and an entry for
  each name and package; they do not become free).
- **Resolve:** `LINK-SYMBOLS` at the end of each package and `EXTERN`
  fill the places that wait.
- **Check:** `.UNRESOLVED` shows the names that literals still wait
  for.

`FUNC name` (for `call8`) must know the function already. The
generator (`s2fs.py`) orders the functions of a package so that this
is always true; no object has a circle over `call8`.

## 4 Dependencies, load order, memory

Between the libraries:

```text
libpp  ──▶ libphy ◀──▶ librtc
```

libpp needs 10 names of libphy, but nothing of librtc. libphy and
librtc need each other: libphy needs `temprature_sens_read` of librtc,
librtc needs 7 names of libphy.
In the libraries there are circles too:

- libphy: `PHY-PHY-CHIP-V7`, `-ANA` and `-CAL`, and `RTC-RTC-ANALOG`;
- libpp: `PP-ESF-BUF` with `PP-PP-DEBUG`, and a large core with
  `PP-HAL-MAC`, `PP-IF-HWCTRL`, `PP-LMAC`, `PP-PM`, `PP-PM-BEACON-OFFSET`,
  `PP-PM-COEX`, `PP-PP`, `PP-PP-TIMER`, `PP-TRC`, `PP-WDEV`.

The packages load correctly in **each** order: the linker fills the
literals that wait. The order changes only the memory for them: with
the recommended order, the RAM data of all three libraries are 12.7 KB;
with the worst order, 36.1 KB. The recommended order:

1. `romsyms.fs`, `idf.fs` (or your own versions), `EXTERN` for the other names;
2. libphy: `phy_chip_v7_ana phy_chip_v7_cal phy_chip_v7 phy phy_fpga_v7_cal phy_fpga_v7_init`;
3. librtc: `bt_bb pm rtc rtc_analog rtc_cntl rtc_mac`, then the empty `eagle.app`;
4. libpp: `esf_buf pp_debug hal_ampdu hal_crypto hal_mac_tx hal_tsf hal_mac_rx hal_sniffer rate_control hal_mac pm_coex trc pm_beacon_offset wdev pm pp_timer pp lmac if_hwctrl`, then the empty ones.

The head of each package names the packages and names that it needs.

Memory, measured on the host, each library alone, in the worst order:

| | Code (SRAM0) | Dictionary | RAM data | of it, data of C |
| --- | ---: | ---: | ---: | ---: |
| libphy | 65.1 KB | 14.9 KB | 5.3 KB | 0.7 KB |
| libpp | 87.0 KB | 47.7 KB | 29.4 KB | 4.7 KB |
| librtc | 23.6 KB | 3.8 KB | 1.4 KB | 0.04 KB |
| all three | 175.5 KB | 65.8 KB | 12.7 to 36.1 KB | 5.5 KB |

The code is the problem: SRAM0 has 118 KB free for `CODE` words
(`targets/esp32/README.md`, "Memory"). libpp with libphy (152 KB) does
not fit. Thus an application with Wi-Fi needs a subset (section 7).
Each `CODE` word is smaller than the buffer of 2 KB that `CODE` has on
the ESP32 (the largest has 1 588 bytes).

## 5 Names outside the libraries

The libraries call 67 names that none of them defines:

- **21 in the ROM:** `romsyms.fs` gives their addresses. They come from
  the linker scripts `esp32.rom*.ld` of PlatformIO
  (`framework-arduinoespressif32` 3.20017), `__divsf3` only from the
  older script of the Arduino core 1.0.4. The ROM does not change.
  - `sprintf` needs the data of newlib in the ROM. It is not tested
    whether it runs without the preparation of ESP-IDF.
  - `free` is in the ROM too (`$4000BEB8`), but it works on the heap of
    the ROM, and without ESP-IDF nobody sets up this heap. Thus it is a
    comment in `romsyms.fs`, and it needs a replacement.
- **6 of ESP-IDF, with a proposal:** `idf.fs` has
  `esp_dport_access_reg_read` (1 119 literals: libphy 585, librtc 534),
  `phy_enter_critical`, `phy_exit_critical`, `rtc_get_xtal`,
  `phy_printf` and `rtc_printf` (= `ets_printf` in the ROM). Not tested.
- **40 are missing:** they are not in the project and not in the ROM.
  `.UNRESOLVED` shows them after loading; `EXTERN` must give them:

| Group | Names (literals) | Library |
| --- | --- | --- |
| the interface of esp_wifi to the operating system | `g_osi_funcs_p` (130): a table of functions for tasks, queues, semaphores, timers, memory | libpp |
| the Wi-Fi driver, the configuration | `wifi_log` (197), `g_wifi_mac_time_delta` (19), `g_wifi_menuconfig` (8), `g_intr_lock_mux` (6), `g_wifi_global_lock` (4), `wifi_rf_phy_enable` `wifi_rf_phy_disable` (3 each), `wifi_event_post` `wifi_set_rx_policy` `esp_wifi_get_mode` `config_get_wifi_task_stack_size` `config_is_cache_tx_buf_enabled` `wifi_nvs_get_sta_listen_interval` (2 each), `wifi_nvs_set_sta_listen_interval` `g_wifi_nvs` `wifi_sta_rx_probe_req` `g_offchan_packet_lifetime` `offchan_tx_progress_in` `esp_wifi_internal_on_coex_start` `esp_wifi_internal_on_coex_schm_phase` `wl_is_ap_no_lr` (1 each) | libpp |
| libnet80211 | `sta_reset_beacon_timeout` (3), `g_ic` `ieee80211_hostapd_beacon_txcb` `chm_get_current_band` (1 each) | libpp |
| Mesh | `g_mesh_is_started` (42), `g_mesh_is_root` (33), `g_mesh_init_ps_type` (30), `esp_mesh_quick_funcs` (7), `esp_mesh_get_running_active_duty_cycle` (1) | libpp |
| NAN | `NAN_NETWORK_ID` `NAN_STANDARD_BSSID` (1 each) | libpp |
| the heap | `free` (1) | libpp |
| the RTC of ESP-IDF | `rtc_slp_prep` (5), `rtc_init_clk` `rtc_slowck_cali` `rtc_time_get` `rtc_sleep_set_wakeup_time` `rtc_dbias_cfg` (1 each) | librtc |

Also missing is what ESP-IDF does around the libraries: the init data
and the calibration of the PHY (for the radio, ESP32-PHY does this
now), the Wi-Fi driver with its tasks, the interrupts of the MAC. The
README of each library tells the details.

## 6 Loading, and the test on the host

**For the radio**, ESP32-PHY is sufficient. It loads only the functions
that are necessary, links them, and then forgets the tools and the
names of the code (`lib/esp32/README.md`, "The radio of the ESP32:
ESP32-PHY"). In `ucf term`, started in the folder `ucf`, immediately
after a reset (66 s):

```text
board ok> #include lib/esp32/esp32/phy.fs
board ok> PHY-INIT .
1
```

**Full libraries** load as follows; paths after `#include` are relative
to `ucf`. This is not tested on the board. The same sequence is tested
on the host (with `mock.fs` instead of ASM):

```text
board ok> #require FAIL lib/text.fs
board ok> #require ADD, asm/xtensa.fs
board ok> #include lib/esp32/asm.fs
board ok> #include lib/esp32/clink.fs
board ok> #include blob/esp32/romsyms.fs
board ok> #include blob/esp32/idf.fs
          ... EXTERN for the other names ...
board ok> #include blob/esp32/libphy/phy_chip_v7_ana.fs
          ... the packages in the order of section 4 ...
board ok> ALSO CLINK  .UNRESOLVED
```

Load time: `ucf term` sends one line for each prompt. ESP32-PHY, with
7 400 lines of code and data and the tools, needs 66 s; libpp alone has
17 800 lines.

`FREEZE` only before a function of C ran: `FREEZE` saves the data of C
with their current values, but C expects the start values after each
start. Also, on the ESP32 the first `FREEZE` moves the dictionary into
the flash, but C keeps the chip addresses of its `.rodata` (jump
tables, texts). Thus tables that C reads must be in the RAM data
(`lib/esp32/clink.fs` tells the rule). ESP32-PHY does both: it keeps a
copy of the start values and puts them back before each calibration,
and it copies its tables into the RAM data after loading.

**The test on the host** compares all packages with the objects. It
needs the tools of section 8 (they are not in the repository),
`build/ucf-c` of ucf and `python3`. In this folder, with the absolute
path of `ucf` as the argument:

```sh
test/run.sh "$PWD/../.."            # all three libraries
test/run.sh "$PWD/../.." libpp      # one library; the names of the others get test addresses
```

`run.sh` loads TEXT, XTENSA, `mock.fs` and CLINK of ucf, then
`romsyms.fs` and `idf.fs`, then all packages in the **worst** order,
and only then the other names with test addresses.
`lib/esp32/test/mock.fs` replaces the ROM and ASM of the board: `CODE`
assembles into a buffer of 2 KB, as on the ESP32, and gives the code.
Then `verify.py` compares with the objects (`*.info`):

- each function from its literals to its end, with the relocations at
  the addresses that Forth gave: literals get the address of the symbol
  plus the offset, `call8` gets the distance to the `CODE` word of the
  target;
- the jump over the literals at the start of each `CODE` word;
- each data object (jump tables and pointers with the addresses of
  Forth), `.bss` and COMMON with 0, the alignment and the kind (RAM
  data or dictionary);
- PUBLIC is the same as the set of the global symbols of each object.

The result (2026-10-04, 1.5 s):

```text
verify: 43 packages, 40 names with test addresses
waiting after EXTERN: <none>
175500 bytes of code, 65756 of dictionary, 36080 of RAM data
verify: 169715 bytes of code, 15594 bytes of data, 0 errors
```

Not tested is the behavior on the chip: the addresses of the ROM, the
proposals in `idf.fs`, all of section 5.

## 7 In ucf: the radio and `tools/pick.fs`

An application does not need all 1 114 functions, and the three
libraries do not fit into SRAM0. `tools/pick.fs` finds the hull: what a
list of entries calls, what that calls, and so on (it looks for names
as CLINK does). Then it copies each line of these functions and data
without a change, without the frames of the packages, with a head about
origin and license. Its use is in `tools/README.md`, "pick.fs".

The pick list of the radio is `lib/esp32/esp32/phypick.fs`: 8 entries in
4 package files, 171 functions and 76 data objects; from outside only
12 names (7 in the ROM, 5 of ESP-IDF). On the WROOM, ESP32-PHY has
47 484 bytes of code in SRAM0. The same list with the same packages
gives the same copies, byte for byte. If an archive changes, its
SHA-256 changes: then PICK writes nothing until the pick list names the
new SHA-256, and the copies need a new test.

What of this folder is in ucf:

| Here | In ucf |
| --- | --- |
| 20 instructions for the code of C (LOOP, MUL16U, MOVSP, floating point, Boolean, BF/BT) | `asm/xtensa.fs`, with `LOOPNEZ,` and `LOOPGTZ,`; tests in `asm/test/xtensa.fs`; the disassembler DISXT knows them |
| the linker | `lib/esp32/clink.fs` (package CLINK) |
| ROM and ASM for tests on the host | `lib/esp32/test/mock.fs` |
| 171 functions and 76 data objects of `phy_chip_v7`, `phy_chip_v7_ana`, `phy_chip_v7_cal` (libphy) and `rtc_analog` (librtc) | `lib/esp32/esp32/phychip.fs`, `phyana.fs`, `phycal.fs`, `phyrtc.fs`, `phydata.fs`, made by `tools/pick.fs` |
| 7 addresses of `romsyms.fs`, 5 functions of `idf.fs`, `FPU-ON` | `lib/esp32/esp32/phyidf.fs`: `EXTERN`, the 5 functions as `CODE` words (written as in the reference, tested on the board), `fpu_on` |
| | the package ESP32-PHY (`lib/esp32/esp32/phy.fs`): it links the copies and calls them; tested on the WROOM |

Nothing of libpp is in ucf: the MAC of the ESP32 starts in Forth
(`lib/esp32/esp32/macinit.fs`, package ESP32-MAC).

## 8 How the packages were made

The tools are not in the repository; the repository has their results
(`.S`, `.fs`). Each tool stops with a message instead of writing a
wrong result. On 2026-10-04, each step made the existing files again,
byte for byte: all 43 objects, the listings, the `.S` and the `.fs`.

```text
libXY.a ──ar_roundtrip.sh──▶ *.o ──objdump_listings.sh──▶ *.dis, *.info
*.o (or *.dis with *.info) ──rewrite_objdump.py──▶ *.S ──s2fs.py──▶ *.fs
*.fs ──test/run.sh, verify.py──▶ 0 errors
*.fs ──tools/pick.fs of ucf──▶ lib/esp32/esp32/phy*.fs (section 7)
```

- `tools/ar_roundtrip.sh` takes an archive apart into its objects,
  packs them again and compares. The result for all three archives: the
  same bytes.
- `tools/objdump_listings.sh` writes two listings for each object:
  `.dis` (`objdump -d -r -z`: the code with its relocations) and `.info`
  (`objdump -x -s`: sections, symbols, relocations, the contents).
- `rewrite_objdump.py` makes an assembler source `.S` from an object
  (objdump does not give a listing that assembles again). The rules are
  in `libphy/rewrite_rules.md`. The version in `libpp/` is the newest.
- `s2fs.py` makes the packages `.fs` from the `.S` (section 2).
  `s2fs.py --report` shows the packages, their dependencies, the load
  order and the long names.
- `test/run.sh` and `test/verify.py` are the test of section 6.

They need:

- the binutils for the ESP32: `xtensa-esp32-elf-ar`, `-objdump` and
  `-as`, version **2.46** (crosstool-NG esp-16.1.0). With 2.35.1 (the
  version of PlatformIO), the listings have other spaces, and
  `rewrite_objdump.py` stops. `ar` works with both versions;
- Python 3.10 or newer, without other packages;
- `build/ucf-c` of ucf for the test.

## 9 Open points

These points are open. They become important when an application needs
more than ESP32-PHY.

- **Room for code:** SRAM0 has 118 KB free; the three libraries need
  175 KB (section 4). Only a subset fits.
- **Functions of C as interrupt handlers:** `wDev_ProcessFiq` (the MAC)
  and `bb_intr_handl` (the baseband), both in libpp, are handlers in C.
  They need a call with a register window from the interrupt. ucf has
  Forth handlers (level 1) and `CODE` handlers with `RET,` (level 2).
- **The exception Alloca (cause 5):** `MOVSP` in `ppCalTkipMic` (libpp)
  causes it when the windows of the caller are still in use. ESP-IDF
  then saves the windows; the HAL of ucf makes a `THROW`.
- **The floating point coprocessor:** the HAL does not switch it on.
  Before a function with floating point, `FPU-ON` (CLINK) or `fpu_on`
  (`phyidf.fs`); otherwise the exception "Coprocessor 0 disabled". The
  HAL does not save the registers `f0`–`f15` at an interrupt.
- **A replacement for ESP-IDF** (`idf.fs`) and the 40 missing names
  (section 5): only when an application needs them. The interface to
  the operating system (`g_osi_funcs_p`) is the largest part.
- **`CODE!` on the ESP32-S3:** there, the code is in the dictionary and
  needs the address on the data bus (`>IRAM` of
  `lib/esp32/esp32s3/asm.fs`).
- **Packages that do not stay in the search order:** `END-PACKAGE
  PREVIOUS` is a makeshift. A word for libraries with many packages
  would be clearer.
- **The search in a list of wordlists:** CLINK changes the search order
  for a short time. `SEARCH-WORDLIST` (Forth-2012) or a primitive would
  be cleaner and faster.
- **Names with more than 31 characters:** a header has 5 bits for the
  length; thus the names are shorter here (section 2).
- **Long file names on the host:** `S"` at the input line has 80
  characters; long paths work only through links (as in `test/run.sh`).
- **File names 8.3:** the `.fs` here have the names of the `.S`
  (`phy_chip_v7_ana.fs`); ucf wants 8.3 names. The copies in ucf have
  them.
