# libpp: the lower MAC layer of Wi-Fi as uCForth packages

`libpp.a` is the lower MAC layer of Wi-Fi in the ESP32 ("packet
processing"): the transmit and receive paths of the MAC hardware
(`hal_*`, `wdev`), the control of the MAC (`lmac`), power save (`pm*`),
the selection of the rate (`trc`, `rate_control`), buffers (`esf_buf`),
timers. This folder has its 30 objects as uCForth packages, one for
each `.S`. The README of the folder above tells what all three
libraries have in common: a package, the linker, loading, the test, the
tools.

## Status

- **Packages:** a `.fs` for each of the 30 objects; 11 objects are
  empty, their packages too.
- **Tested on the host:** the code (83 175 bytes) and the data (12 157
  bytes) are those of the objects, byte for byte.
- **Not tested on the board.** libpp runs only with what ESP-IDF gives
  around it (section 3): above all the interface to the operating
  system `g_osi_funcs_p`, libnet80211 and the interrupts of the MAC.
- **Nothing of libpp is in ucf.** The MAC of the ESP32 starts in Forth
  (`lib/esp32/esp32/macinit.fs`, package ESP32-MAC).
- **In git:** `libpp.a`, the `.S`, the `.fs` and this README. The
  objects, the listings and the newest version of `rewrite_objdump.py`
  are not in git (`../README.md`, section 8).

## 1 The packages

| File | Package | Functions (global, without symbol) | Data | Code |
| --- | --- | --- | ---: | ---: |
| `esf_buf.fs` | PP-ESF-BUF | 12 (12, 0) | 19 | 2.2 KB |
| `hal_ampdu.fs` | PP-HAL-AMPDU | 6 (6, 0) | 3 | 1.0 KB |
| `hal_crypto.fs` | PP-HAL-CRYPTO | 9 (9, 0) | 3 | 1.2 KB |
| `hal_mac.fs` | PP-HAL-MAC | 49 (49, 0) | 7 | 4.9 KB |
| `hal_mac_rx.fs` | PP-HAL-MAC-RX | 5 (5, 0) | 0 | 0.2 KB |
| `hal_mac_tx.fs` | PP-HAL-MAC-TX | 9 (9, 0) | 2 | 1.2 KB |
| `hal_sniffer.fs` | PP-HAL-SNIFFER | 4 (4, 0) | 0 | 0.5 KB |
| `hal_tsf.fs` | PP-HAL-TSF | 5 (5, 0) | 1 | 0.2 KB |
| `if_hwctrl.fs` | PP-IF-HWCTRL | 103 (103, 0) | 11 | 4.1 KB |
| `lmac.fs` | PP-LMAC | 57 (56, 1) | 58 | 8.1 KB |
| `pm.fs` | PP-PM | 157 (139, 18) | 29 | 19.2 KB |
| `pm_beacon_offset.fs` | PP-PM-BEACON-OFFSET | 32 (31, 1) | 16 | 3.8 KB |
| `pm_coex.fs` | PP-PM-COEX | 8 (8, 0) | 3 | 1.2 KB |
| `pp.fs` | PP-PP | 109 (108, 1) | 58 | 15.8 KB |
| `pp_debug.fs` | PP-PP-DEBUG | 19 (19, 0) | 40 | 2.5 KB |
| `pp_timer.fs` | PP-PP-TIMER | 17 (3, 14) | 4 | 0.6 KB |
| `rate_control.fs` | PP-RATE-CONTROL | 7 (6, 1) | 1 | 0.4 KB |
| `trc.fs` | PP-TRC | 73 (69, 4) | 44 | 8.9 KB |
| `wdev.fs` | PP-WDEV | 65 (65, 0) | 43 | 9.8 KB |
| `hal_coex.fs` `hal_he_common.fs` `hal_pwr.fs` `if_hecfg.fs` `pm_extend.fs` `pm_mbssid.fs` `pm_twt.fs` `pp_he.fs` `pp_he_ctrl.fs` `pp_test.fs` `pp_vht.fs` | PP-HAL-COEX … PP-PP-VHT | empty | | |

Together 746 functions. "Without symbol" counts the static functions
whose symbol the object does not have (section 2). Measured on the
host: 87.0 KB of code (SRAM0), 47.7 KB of dictionary, up to 29.4 KB of
RAM data in the worst order, of it 4.7 KB for the data of C. The
largest `CODE` word has 1 217 bytes (`ppRxFragmentProc`).

The order with the fewest literals that wait (two circles: PP-ESF-BUF
with PP-PP-DEBUG, and a core of PP-HAL-MAC, PP-IF-HWCTRL, PP-LMAC,
PP-PM, PP-PM-BEACON-OFFSET, PP-PM-COEX, PP-PP, PP-PP-TIMER, PP-TRC and
PP-WDEV):

```text
esf_buf pp_debug hal_ampdu hal_crypto hal_mac_tx hal_tsf hal_mac_rx hal_sniffer
rate_control hal_mac pm_coex trc pm_beacon_offset wdev pm pp_timer pp lmac if_hwctrl
```

Then the 11 empty ones in any order. Before them: `../romsyms.fs`,
`../idf.fs`, the missing names (section 3) and libphy. libpp needs 10
names of libphy and nothing of librtc; no other library needs libpp.
The head of each package names the packages and names that it needs.

## 2 What is special about the objects

libpp is built with GCC 11, libphy and librtc with GCC 5. This shows as
follows:

- **Static functions without a symbol:** the local symbols are not
  there. 40 static functions are known only by their section and get
  its name: `.text.offchan_in_progress` gives `offchan_in_progress`,
  `.wifislpiram.2` gives `wifislpiram.2`. They are PRIVATE, and the head
  of their `CODE` word says "static, without symbol". The same is true
  for static data, for example 23 objects in `.bss.*`.
- **Texts for each function:** `.rodata.<function>.str1.1`, and the
  texts of `wifi_log` in `.rodata_wlog_error.N` (`…_info`, `…_warning`,
  `…_debug`). They are in the dictionary as `," …"`.
- **Alignment 8:** `g_pm`, `g_pm_cnt`, `s_fragment`, `s_tbttstart`
  (`8 DATA-ALIGN`).
- **Pointers in the data:** `pTxRx` points to `TxRxCxt`,
  `libpp_reversion_git` to a text (`SYMBOL!` at the end of the package).
- **Long names:** 67 names have more than the 31 characters of a name
  in Forth. In Forth, a long name is its first 26 characters, `~` and 4
  hex digits of its CRC-32 (`../README.md`, section 2). The head of each
  package names its long names; `EXTERN` and `'` need these names.
- **Floating point:** 7 functions of `pm_beacon_offset` and
  `dbg_lmac_ps_statis_dump` (`pp_debug`) use the floating point
  coprocessor and the jumps `bf` and `bt`.

## 3 What is missing

- **17 functions of the ROM:** `ets_delay_us` `memcpy` `memset` `memcmp`
  `roundup2` `free` and the floating point helpers of libgcc
  (`__adddf3` `__subdf3` `__muldf3` `__divsf3` `__udivdi3` `__umoddi3`
  `__floatsidf` `__floatunsidf` `__floatundisf` `__extendsfdf2`
  `__truncdfsf2`). Their addresses are in `../romsyms.fs`, from the
  linker scripts of PlatformIO, but not `free` (section 4).
- **The interface to the operating system** `g_osi_funcs_p` (130
  literals): a pointer to the table `wifi_osi_funcs_t` of esp_wifi:
  tasks, queues, semaphores, mutexes, timers, memory, interrupts, time.
  libpp calls nearly everything through it. ucf has no RTOS; a layer in
  Forth or C would have to make this table.
- **32 other names** of ESP-IDF and other libraries: `wifi_log`, Mesh,
  the Wi-Fi driver and its configuration, libnet80211, NAN. They are not
  in the project and not in the ROM; the list is in `../README.md`,
  section 5.
- **The prototypes of C:** the number and the types of the arguments
  are not known; `CALL-C` needs the number from the caller.
- **The driver around libpp:** the task of Wi-Fi (`ppTask`) runs under
  FreeRTOS. The sequence of start, connection and power save is in
  esp_wifi, libnet80211 and libcore, not in libpp.
- **The version:** libpp is built with GCC 11.2 (esp-2022r1), libphy
  and librtc with GCC 5.2. The archives are those of the reference
  (`esp-wifi-sys-esp32` 0.2.0); the reference runs with exactly these
  three archives.

## 4 Problems

| Problem | Result, what to do |
| --- | --- |
| `movsp` in `ppCalTkipMic` | can cause the exception Alloca (cause 5) when the windows of the caller are still in use. ESP-IDF then saves the windows; the HAL of ucf makes a `THROW` (`../README.md`, section 9). |
| `break 1, 15` in `lmacProcessCtsTimeout`, `lmacProcessTxError`, `lmacProcessTxRtsError`, `lmacProcessTxopSuccess`, `lmacSetTxFrame` | breakpoints on error paths (like `assert`). They give a debug exception; under ucf, a `THROW`. |
| floating point in `pm_beacon_offset` and `pp_debug` | `FPU-ON` (CLINK) before the call; otherwise the exception "Coprocessor 0 disabled". |
| `free` | The ROM has `free` (`$4000BEB8`), but it works on the heap of the ROM, and without ESP-IDF nobody sets up this heap. Thus it is a comment in `../romsyms.fs`. It needs a replacement that matches the `malloc` of the interface to the operating system. |
| interrupts | `wDev_ProcessFiq` (`wdev`) is the handler of the interrupt of the MAC, `bb_intr_handl` (`if_hwctrl`) that of the baseband; both are functions of C. ucf calls Forth handlers (level 1) or `CODE` handlers with `RET,` (level 2) from an interrupt, no functions with a register window (`../README.md`, section 9). |
| room | 87 KB of code fit into SRAM0 alone, but not with libphy (65 KB). An application needs a subset (`../README.md`, section 7). |
| `FREEZE` | saves the data of C with their current values; thus only before a function ran. |
