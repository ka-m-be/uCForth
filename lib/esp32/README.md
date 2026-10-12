# lib/esp32: packages for the ESP32 chips

The packages for the ESP32 family: drivers that run on the board (Forth
over the registers of the chip), board files, and the packages that
load a board from the host. For the radio of the ESP32 there is also
code of C of Espressif, as `CODE` words (section "The radio of the
ESP32: ESP32-PHY"); the reception of frames is in Forth (section "The
radio of the ESP32: ESP32-RADIO"). So far there are two chips: the ESP32-S3
(`targets/esp32s3`) and the ESP32, for example in the module
ESP32-WROOM-32 (`targets/esp32`). The section "Which board" compares
them. File names and folder names have at most 8 characters.

## The layout: a layer for each chip, the drivers above it

A package has the same name and the same words on each chip. If only
an address, a number or a bit differs, its code is in this folder once,
and it takes these values from the layer of the chip. If the code
itself differs, each chip has its own file in its folder.

- **The layer of the chip** is a package with the name of the chip:
  ESP32S3 (`esp32s3/chip.fs`), ESP32 (`esp32/chip.fs`). It has the
  addresses of the registers, the numbers of the interrupt sources and
  of the signals, the addresses of the functions in the ROM, and
  `CHIP-NAME`. Each image has the layer of its chip.
- **`CHIP`** ( -- ) is the package of the chip of this board: a common
  package writes `ALSO CHIP` and gets the values of the chip it runs on.
- **`#when`** in `ucf term` chooses the file of the chip: `rmt.fs` and
  `asm.fs` in this folder hold lines such as `#when ESP32S3
  esp32s3/rmt.fs` (host/README.md). So an application (`apps/aircon`)
  loads `lib/esp32/rmt.fs` on each board and gets the package RMT of
  the chip. A build on the host names the file of the chip directly.
- **The board** has a package of its own, in a folder of its maker in
  the folder of its chip (all names 8.3): `esp32s3/diymore/n16r8.fs`
  (package DIYMORE-N16R8), `esp32/diymore/wroom-n4.fs` (package
  DIYMORE-WROOM-N4). It names the pins of the board and drives its LED
  (`LED`, `BUTTON`, `LED-ON`, `LED-OFF`). At its end, **`BOARD`** ( -- )
  gives the package, as `CHIP` does for the chip: `ALSO BOARD`.
  `board.fs` in this folder loads the file of the tested board of each
  chip (`#when`); a file that needs the board writes `#require BOARD
  ../lib/esp32/board.fs` (`examples/blink.fs`). For another board, load
  its file first: then `#require` does not load `board.fs`.

## The common packages (this folder)

| File | Runs on | Contents |
| --- | --- | --- |
| `rom.fs` | board | package ROM: the headers of the HAL words `SYSCALL` (a function of the ROM) and `>CHIP` (the chip address of a VM address) |
| `intr.fs` | board | package INTR: handlers of interrupts (`INT!`, `INT@`, `INT-ON`, `INT-OFF`, `+INT`, `-INT`): Forth words at the next NEXT, `CODE` words at once (level 2); an error stops only its handler, and its message comes when the console waits (`INT-STOPPED`); the message of an error of the CPU (-9 with the address and the PC); needs CHIP and IDLE; `FAULT-REGS`: the HAL writes all registers of the last error of the CPU there (the monitor shows them, `asm/monitor/README.md`); `DEBUG-HOOK`: the code of the monitor for a debug exception |
| `gpio.fs` | board | package GPIO: the base of the drivers: `BITS!` (bits in a register), `SRAM-ALLOT` and `SRAM-BUFFER:` (RAM inside the chip, also when the RAM data are in the PSRAM), GPIO (`OUTPUT INPUT PULLUP PULLDOWN OPEN-DRAIN HIGH LOW PIN@ PIN! TOGGLE DRIVE`), the GPIO matrix (`SIGNAL>PIN`, `PIN>SIGNAL`), interrupts of the pins (`PIN-INT!`); needs CHIP and INTR |
| `timers.fs` | board | package TIMERS: `EVERY`, `AFTER`, `.TIMERS`: Forth words that run later, or every n ms; an error stops only its word; needs CHIP and INTR |
| `nettimer.fs` | board | package NET-TIMER: our TCP/IP in the background, a step every 1 ms in a timer, and its foreground work in IDLE (`NET-START`, `NET-STOP`); needs TIMERS, IDLE and our TCP/IP (`lib/net/`) |
| `tcpip.fs` | `ucf term` | loads our TCP/IP and NET-TIMER with one `#require` (the board version of `lib/net/tcpip.fs`) |
| `flash.fs` | board | package FLASH: the SPI flash over the ROM (`FLASH-READ`, `FLASH-WRITE`, `FLASH-ERASE`), with `FLASH-SYSCALL` of the layer of the chip (on the ESP32 the cache is off meanwhile); buffers only inside the chip; needs CHIP and ROM |
| `ir.fs` | board | package IR: frames of IR remote controls as times in us over the RMT: `IR-PINS`, `IR-SEND`, `IR-FRAME?`, `IR-LISTEN`; made for the IR transceiver of Adafruit (product 5990); needs RMT and GPIO |
| `irqueue.fs` | WROOM, with IR-CAPTURE | package IR-QUEUE: IR in the background for the bridge: a ring of 16 received frames, sending after 15 ms without light with a check of the echo (`IR-Q-START`, `IR-Q-SEND`, `IR-Q-FRAME?`; section "IR for the bridge on the WROOM") |
| `ws2812.fs` | board | package WS2812: an RGB LED over channel 0 of the RMT (`RGB-PIN`, `RGB!`); needs RMT |
| `clink.fs` | board | package CLINK: links code of C that is written as `CODE` words (the quarry `blob/esp32`): cells that get the address of a name (`SYMBOL`, `SYMBOL!`), names outside (`EXTERN`), `LINK-SYMBOLS`, `.UNRESOLVED`, `UNRESOLVED?`, the data of C (`DATA:`, `INIT`), `CALL-C`, `FPU-ON`; needs TEXT, XTENSA, ROM and ASM; not in the image: ESP32-PHY loads it only for the link |
| `rmt.fs` | `ucf term` | chooses the package RMT of the chip (`#when`) |
| `asm.fs` | `ucf term` | chooses the package ASM of the chip (`#when`) |
| `board.fs` | `ucf term` | chooses the package of the tested board of the chip (`#when`): it defines `BOARD` |
| `espimg.fs` | host | package ESPIMG: the image format of the ROM bootloader, read and write; `IMAGE-CHIP` (0 the ESP32, 9 the ESP32-S3) |
| `esprom.fs` | host | package ESPROM: the ROM bootloader of both chips over a serial line: `START` (it finds the chip: `CHIP-ID`), `INFO`, `FLASH`, `HARD-RESET`, the RAM, `ATTACH-FLASH` |
| `test/timers.fs` | board | the test of TIMERS: the same on each chip |
| `test/mock.fs` | host | ROM and ASM on the host, for tests of `CODE` words of the ESP32: `CODE` assembles as on the chip and shows the code; `QUIET` hides it |
| `test/phyhost.fs` | host | the test of ESP32-PHY on the host (section "The radio of the ESP32: ESP32-PHY") |
| `test/radiohost.fs` | host | the test of ESP32-RADIO on the host (section "The radio of the ESP32: ESP32-RADIO"); the tests of WLAN-BACK, SETUP-AP and IR-QUEUE: section "The WLAN of the WROOM", "Tests" |

`lib/idle.fs` (package IDLE: the chain of words that run while the
console waits, and `NO-BREAK`, for which Ctrl-C waits) comes before
INTR in the image of both chips.

## The ESP32-S3 (`esp32s3/`)

| File | Contents |
| --- | --- |
| `chip.fs` | package ESP32S3: the layer of the chip (the addresses of GPIO, IO MUX, interrupt matrix, timer group 1, the functions of the ROM for the flash and `FLASH-SYSCALL`, here the same as `SYSCALL`, the PSRAM), `CHIP-NAME`; then `CHIP` |
| `clock.fs` | package CLOCK: the clock of the CPU, 40 to 240 MHz (`MHZ`, `CPU-MHZ`); needs ROM and GPIO |
| `psram.fs` | package PSRAM: the 8 MB octal PSRAM at `$3C00_0000`, the RAM data grow into it (`PSRAM-ON`, `PSRAM-ADDR`, `PSRAM-SIZE`, `.PSRAM`, `DCACHE-MODE`); needs ROM, CLOCK and GPIO |
| `bootimg.fs` | package BOOTIMG: `FREEZE` on the board writes the boot image and the RAM data of the PSRAM to the flash (`(SAVE-IMAGE)`); `XDATA-LOAD` loads these data back; needs ROM, FLASH and PSRAM |
| `rmt.fs` | package RMT: the remote control unit (RMT), channel by channel: ticks, RAM blocks, pins, carrier, demodulator, filter; send pulse words (also more than the RAM holds), receive them (`RMT-ON`, `RMT-HZ`, `RMT-SOURCE`, `RMT-DIV`, `RMT-CARRIER`, `RMT-SEND`, `RMT-RX-ON`, `RMT-RECEIVED` ...); needs CHIP and GPIO |
| `asm.fs` | package ASM: `CODE` ... `END-CODE`, machine code in the dictionary with the assembler `asm/xtensa.fs`; `>CODE` (the address of the code on the instruction bus); needs TEXT, XTENSA, ROM; not in the image (14 KB) |
| `diymore/n16r8.fs` | package DIYMORE-N16R8: the board ESP32-S3 N16R8 of diymore: `LED` (48, the RGB LED), `BUTTON` (0), `LED-ON`, `LED-OFF`; loads RMT and WS2812; then `BOARD` |
| `test/` | the tests of INTR, GPIO, RMT, IR and ASM on this chip (they use its registers and signals); `h1.fs`, the test of the two levels: IR and the network in the background, with Ctrl-C and errors at the console |

IDLE, ROM, ESP32S3, INTR, GPIO, TIMERS, FLASH, CLOCK, PSRAM and BOOTIMG are in
the image that `targets/esp32s3/build.fs` makes, in this order (its
README: "Build").

## The ESP32 (`esp32/`)

| File | Contents |
| --- | --- |
| `chip.fs` | package ESP32: the layer of the chip (the addresses of GPIO, the IO MUX with a table for its registers, the pull resistors in the RTC IO (`>PULL`), the interrupt matrix, timer group 0 with `MICROS` (the time in us), timer group 1, the functions of the ROM for the flash and `FLASH-SYSCALL`, HAL 105: `SYSCALL` with the cache of the flash off), `APB-MHZ`, `CHIP-NAME`; then `CHIP` |
| `clock.fs` | package CLOCK: the clock of the CPU, 40 to 240 MHz (`MHZ`, `CPU-MHZ`), with new dividers for UART0 and the timers (they count the APB clock); needs ROM, CHIP and GPIO |
| `bootimg.fs` | package BOOTIMG: `FREEZE` on the board appends the dictionary to its frozen part in the flash (at `$6_0000` + VM address; `FLASH-END`, `DICT-FLASH`), then writes the boot image at `$1000` (`(SAVE-IMAGE)`), with the code of the `CODE` words (`CODE-START`, `CODE-HERE`) and the RAM data; `XDATA-ON` gives the RAM behind the frozen part to the RAM data; `XDATA-FREEZE` and `?FREEZE` for the loader of an application; needs ROM and FLASH |
| `asm.fs` | package ASM: `CODE` ... `END-CODE` with the assembler `asm/xtensa.fs`, the words of the ESP32-S3; the code goes to SRAM0 after the code of the kernel, a cell at a time (at most 2 KB for one word); needs TEXT, XTENSA, ROM and BOOTIMG; not in the image |
| `rmt.fs` | package RMT: the words of the package RMT of the ESP32-S3 for the RMT of the ESP32 (8 channels, each sends or receives; 64 pulse words per block; the APB clock; no demodulator); needs CHIP and GPIO |
| `phy.fs` | package ESP32-PHY: calibration, channel, power and PHY rate tables, with selected code of C from libphy/librtc; no libpp MAC code |
| `macinit.fs` | package ESP32-MAC: `MAC-HAL-INIT`, the MAC register initialization in Forth; load after PHY and before RADIO |
| `phychip.fs`, `phyana.fs`, `phycal.fs`, `phyrtc.fs`, `phydata.fs` | the code and the data of C for ESP32-PHY: copies from the quarry `blob/esp32`, made by `tools/pick.fs`; Apache License 2.0 |
| `phypick.fs` | the pick list of ESP32-PHY: which functions `tools/pick.fs` copies |
| `phyidf.fs` | for ESP32-PHY: the 5 functions of ESP-IDF that the code of C calls (here as `CODE` words), `fpu_on`, and the addresses of the 7 functions of the ROM that it calls |
| `radio.fs` | package ESP32-RADIO: the radio of the ESP32 with the words of the contract RADIO: on and off, the channel, reception and sending, the filters of a scan, a station and an AP, the keys of the CCMP in the MAC, the packet RAM (`RADIO-POOL`); then `RADIO`; a file for `ucf term`, after `phy.fs`, not in the image (section "The radio of the ESP32: ESP32-RADIO") |
| `wlsave.fs` | package WLSAVE: the WLAN profile (name and key) in two sectors of the flash; `WLAN-SAVE`, `WLAN-SAVED?`, `WLAN-SAVED-NET`, `WLAN-JOIN`, `WLAN-FORGET` |
| `wlstack.fs` | `ucf term`: loads the WLAN of the WROOM with one `#require` (radio, WPA2 station, WLSAVE, both LINKs) |
| `wlback.fs` | package WLAN-BACK: the station joins its saved network and stays joined (`WBACK-START`, `WBACK-STOP`, `WLAN-BOOT-ON`) |
| `wlap.fs` | package SETUP-AP: a temporary open network for one device (`SETUP-START`, `SETUP-STOP`) |
| `ircap.fs` | package IR-CAPTURE: the IR receiver into 4 raw slots by a CODE handler; for IR-QUEUE |
| `diymore/wroom-n4.fs` | package DIYMORE-WROOM-N4: the board ESP-WROOM-32 N4 of diymore: `LED` (2), `BUTTON` (0), `LED-ON`, `LED-OFF`; then `BOARD` |
| `test/` | the tests of INTR, GPIO, RMT, IR and ASM on this chip (GPIO 25, without wires); `phy.fs`, the test of ESP32-PHY; `radio.fs`, the test of ESP32-RADIO (it needs an access point near the board); `irqueue.fs`, `irqbusy.fs`: IR-QUEUE; `airping.fs`, `airsock.fs`, `airtime.fs`, `aphttp.fs`: the WLAN (section "The WLAN of the WROOM") |

IDLE, ROM, ESP32, INTR, GPIO, TIMERS, FLASH, CLOCK and BOOTIMG are in the
image that `targets/esp32/build.fs` makes, in this order (its README:
"Build").

## Which board

The two chips run the same uCForth with the same words; an application
such as `apps/aircon` runs on both with the same files. They differ in
memory, speed and some units. Measured on the tested boards:

| | ESP32-S3 (`targets/esp32s3`) | ESP32 (`targets/esp32`) |
| --- | --- | --- |
| tested board | diymore ESP32-S3 N16R8 (`esp32s3/diymore/n16r8.fs`) | diymore ESP-WROOM-32 N4 (`esp32/diymore/wroom-n4.fs`) |
| CPU | Xtensa LX7, two cores, up to 240 MHz | Xtensa LX6, two cores, up to 240 MHz |
| 1 000 000 times `DO LOOP` at 240 MHz | 88 ms | 150 ms |
| the memory of the VM | 256 KB: the dictionary has 189 KB, 157 KB free | 256 KB in two parts of the RAM, after `FREEZE` the frozen part in the flash: the dictionary has 189 KB, 156 KB free |
| RAM data | 63 KB inside the chip, and 8 MB PSRAM | 63 KB, and after `FREEZE` the RAM of the frozen part (`XDATA-ON`; with N3f 134 KB) |
| flash | 16 MB; the image at `$0` | 4 MB; the boot image at `$1000`, the frozen part of the dictionary at `$7_0000` |
| console | UART0 over the bridge of the board (CH343P); the USB of the chip later | UART0 over the bridge of the board (CH340C) |
| `CODE` words | yes (package ASM), in the dictionary | yes (package ASM), in SRAM0 |
| the radio (Wi-Fi) | not yet | the calibration, the channel, the power (ESP32-PHY, with code of C of Espressif); the reception of beacons (ESP32-RADIO), the list of the networks (`lib/net/wifi.fs`); it does not send yet |
| GPIO | 0 to 48 | 0 to 39; 34 to 39 are inputs only |
| RMT | 4 channels send, 4 receive; 48 pulse words per block; 40 MHz; a demodulator | 8 channels, each sends or receives; 64 pulse words per block; the APB clock; no demodulator |
| UART0 and `MS` with a new CPU clock | they count the crystal: no change | they count the APB clock: CLOCK sets new dividers |
| in QEMU (doc/qemu.md) | the image runs, with PSRAM; no GPIO, no RMT, no Forth handlers | the image runs; no GPIO, no RMT, no Forth handlers |
| tests on the board | 1055; intr 17, gpio 20, timers 6, rmt 31, ir 11, asm 16 | 1055; intr 18, gpio 21, timers 7, rmt 25, ir 11, asm 19, phy 13, radio 25 |

For an application that needs much RAM data (PSRAM), take the
ESP32-S3. The section "The board" of each README lists the
pins that belong to the board.

## The radio of the ESP32: ESP32-PHY

The radio of the ESP32 must be calibrated before it can send or
receive. Espressif gives this calibration only as code of C, in its
library libphy. The package ESP32-PHY (`esp32/phy.fs`) has the
functions of C that the radio needs, as `CODE` words in SRAM0, and
calls them as the reference does (the Rust version of esp32-open-mac:
`esp-wifi-hal` 0.3.3 with `esp-phy` 0.2.0).

**Status:** tested on the board ESP-WROOM-32. The calibration takes
60 ms; the channels 1 to 13, the power, off and on work; the clock,
UART0, the timers and all tests stay as they are. The package
ESP32-RADIO (next section) receives frames with it; the radio does not
send yet. The MAC initialization now runs in Forth in `RADIO-START`.
The binary comparison version is preserved in git commit `9b67376`,
not loaded at runtime. The ESP32-S3 has another
radio and needs other libraries.

### Use

In `ucf term`, in the folder `ucf`. Load ESP32-PHY right after a reset
(the rules below). The load takes about 66 s and shows nothing; then
ESP32-PHY is in the search order:

    board ok> #reset
    ets Jul 29 2019 12:21:46
    ...
    board ok> #include lib/esp32/esp32/phy.fs
    board ok> PHY-INIT .
    1
    board ok> PHY-INIT .
    0
    board ok> 6 PHY-CHANNEL!

| Word | Effect | Time on the WROOM |
| --- | --- | --- |
| `PHY-INIT ( -- n )` | the radio on, with a full calibration; the result goes to `CAL-DATA` (1 904 bytes). n is 1 if `CAL-DATA` held no valid data of an earlier calibration (the first time after the load), else 0. Call it before the other words, and again after each start | 60 ms |
| `PHY-CHANNEL! ( ch -- )` | the channel, 1 to 13 | less than 1 ms |
| `PHY-TRACK ( -- )` | adjusts the power of the sender; ESP-IDF calls it after some frames | 0.02 ms |
| `PHY-LOW-OFF ( -- )` | updates the PHY rate and calibrated power tables as the reference does during MAC setup | |
| `PHY-OFF ( -- )` | the radio off | less than 1 ms |
| `PHY-ON ( -- )` | the radio on again, after `PHY-OFF` | less than 1 ms |

`FREEZE` keeps ESP32-PHY in the image, with `CAL-DATA`. After each
start, the search order has only FORTH, and the radio needs `PHY-INIT`
again. With the data of `CAL-DATA` from the flash, it gives 0:

    board ok> FREEZE
    board ok> #reset
    ...
    board ok> ALSO ESP32-PHY  PHY-INIT .
    0

Without `ALSO ESP32-PHY`, the board says `error: undefined word:
PHY-INIT`.

The rules:

- Load `phy.fs` right after a reset, without TEXT, XTENSA, ASM and
  CLINK. `phy.fs` loads these tools itself and forgets them at its end
  (a marker). A CLINK that was there before would stay, with pointers
  into RAM data that the marker gives back.
- While a function of C runs, the Forth handlers of interrupts wait, as
  during each `SYSCALL`. In its critical sections the console waits
  too. Ctrl-C stops `PHY-INIT` only after the call of C.
- `PHY-INIT` switches on the floating point coprocessor (the code of C
  needs it) and the clocks of the radio. It also gives the data of C
  their start values again, so `FREEZE` is safe after a calibration
  too.
- `MARKER` does not give SRAM0 back: the code of C stays there until
  the next start, also when a marker forgets ESP32-PHY.

### How it loads

`phy.fs` is a file for `ucf term`: it uses `#include` and `#require`.
It loads in four steps:

1. The package ESP32-PHY: its words, `INIT-DATA`, `CAL-DATA`, and the
   data of C (`phydata.fs`) with a copy of their start values. All this
   stays.
2. The marker `PHY-TOOLS`. Then the tools that are not there yet (TEXT,
   XTENSA, ASM, CLINK), and the package PHY-C: our functions
   (`phyidf.fs`) and the code of C (`phyana.fs`, `phycal.fs`,
   `phychip.fs`, `phyrtc.fs`). The code of each function
   goes to SRAM0, as the code of a `CODE` word. CLINK puts the
   addresses of the names into the literals; it finds the data in
   ESP32-PHY.
3. The check: if a literal still waits for a name, the load stops and
   shows the names; all names are still there then. Else the addresses
   of the 9 functions that ESP32-PHY calls go into its VALUEs, and the
   start values of the data of C are saved.
4. `PHY-TOOLS`: the tools, PHY-C and the names of the code go. The code
   stays in SRAM0.

### Memory

Measured on the WROOM:

| | Dictionary | RAM data | SRAM0 (code) |
| --- | --- | --- | --- |
| free after a start | 159 856 bytes | 62 720 bytes | 118 KB (120 904 bytes) |
| ESP32-PHY keeps | 7 432 bytes: start values, `INIT-DATA`, the tables of .rodata (also as copies in the RAM data), words | 5 172 bytes: data of C 732, `CAL-DATA` 1 904, the tables of .rodata 2 464 | 47 484 bytes; no unused binary MAC oracle |
| free after the load | 152 424 bytes | 57 548 bytes | 71.7 KB (73 420 bytes) |

The tables of .rodata are in the RAM data because the code of C keeps
their chip addresses: after `FREEZE` the dictionary lies in the flash,
at another chip address (`phy.fs`: `TABLES>RAM`). After `FREEZE` the
ROM loads 57 720 bytes of code at the start with ESP32-PHY, ESP32-MAC
and ESP32-RADIO, 10 168 bytes without them; the dictionary stays in the
flash.

### Where the code comes from, and its license

- **The code and the data of C** come from the libraries `libphy.a`
  and `librtc.a` of Espressif for the ESP32: Copyright Espressif
  Systems (Shanghai) Co., Ltd., Apache License 2.0
  (`blob/esp32/LICENSE`). They are the archives of the reference (the
  Rust crate `esp-wifi-sys-esp32` 0.2.0).
- **`blob/esp32`** has each object of these libraries as a
  package of `CODE` words, with the bytes of the compiler (its
  README). `tools/pick.fs` copied from it the 171 functions and 76 data
  objects that the 8 functions of ESP32-PHY need (the pick list
  `esp32/phypick.fs`).
- **The changes:** no frames of packages; 6 names that two objects have
  got the tag of their object (`chip:rodata`, `ana:rodata`,
  `cal:rodata`, and the same with `.str1.4`); a head in each copy with
  its package files, the SHA-256 of the archives, the license and the
  new names.
- **`INIT-DATA`** in `phy.fs`: the 128 bytes of parameters of ESP-IDF
  (`phy_init_data.c`, Apache License 2.0), as `esp-phy` 0.2.0 has them
  (MIT or Apache License 2.0); byte 44 is 78, as in ESP-IDF today.
- **Our own** (MIT License): `phy.fs` without `INIT-DATA`, and
  `phyidf.fs`: the 5 functions of ESP-IDF that the code of C calls,
  written as the reference has them for one core without an operating
  system (`esp_dport_access_reg_read`, `phy_enter_critical`,
  `phy_exit_critical`, `rtc_get_xtal`, and `phy_printf`, which shows
  nothing); `fpu_on`; the addresses of 7 functions of the ROM, from the
  linker scripts of ESP-IDF (`__divsf3` from the older script of the
  Arduino core 1.0.4).

The file `LICENSE` in the folder `ucf` names the files under the Apache
License 2.0.

### Pick again: `tools/pick.fs`

When the radio needs another function of C:

1. Put its name on a line `PICK-ENTRY` of `esp32/phypick.fs`. If it is
   in another package file of the quarry, add a line `PICK-FROM` too.
2. Pick, in the folder `ucf` (3.7 s):

       build/ucf-c -32 host/files.fs lib/esp32/esp32/phypick.fs </dev/null

   PICK shows the hull (the functions and data that the entries need)
   and writes the copies again. If the hull needs a name outside the
   package files that is not on a line `PICK-OUTSIDE`, PICK stops and
   shows it. Then that function needs its address in the ROM (`EXTERN`
   in `phyidf.fs`) or a `CODE` word in `phyidf.fs`, and a line
   `PICK-OUTSIDE`.
3. In `esp32/phy.fs`: a VALUE for the address of the function, a line
   `' name >CODE TO 'NAME` after `LINKED`, and a word that calls it
   with `C-CALL`.
4. Test on the host and on the board (below). The memory in the test
   on the host changes with the hull.

The same pick list with the same quarry gives the same copies, byte
for byte. `tools/README.md` ("pick.fs") explains the tool.

### Tests

On the host, with 32-bit cells: the link, the names after the marker,
the addresses in SRAM0, the memory, `INIT-DATA`, the start values.
`test/mock.fs` stands for ROM and ASM:

    build/ucf-c -32 host/files.fs lib/esp32/test/phyhost.fs </dev/null

Expected: `phyhost: 20 tests, 0 errors`. On the board, after the load
of `phy.fs`: the calibration, the clocks, the channels, `PHY-TRACK`,
off and on:

    board ok> #include forth/test/tester.fs
    board ok> #include lib/esp32/esp32/test/phy.fs
    phy: 13 tests, 0 errors

## The radio of the ESP32: ESP32-RADIO

The package ESP32-RADIO (`esp32/radio.fs`) is the radio of the ESP32
with the words of the contract RADIO (`lib/net/README.md`, "The
contract WIFI"): on and off, the channel, the reception of frames, the
filter of a scan. It is Forth over the registers of the MAC (the part
of the chip that sends and receives the frames), over ESP32-PHY. The
package WIFI (`lib/net/wifi.fs`) lists the networks around with it.

**Status:** tested on the board ESP-WROOM-32: it receives and sends, as a
station of a WPA2 network with the CCMP in the MAC, and as an open access
point for one device (section "The WLAN of the WROOM"). The addresses and
the bits of the MAC come from the reference (the register map of the crate esp32
0.40.2, the start in esp-wifi-hal 0.3.3) and from measurements on the
board.

### Use

In `ucf term`, in the folder `ucf`, right after a reset (the two loads
take about 67 s):

    board ok> #reset
    ets Jul 29 2019 12:21:46
    ...
    board ok> #include lib/esp32/esp32/phy.fs
    board ok> #include lib/net/bytes.fs
    board ok> #include lib/esp32/esp32/macinit.fs
    board ok> #include lib/esp32/esp32/radio.fs
    board ok> RADIO-START  6 RADIO-CHANNEL!  RADIO-BEACONS
    board ok> 200 MS  RADIO-RECEIVE . DROP  RADIO-LEVEL .
    410 -33

`RADIO-RECEIVE` gives the length and the address of a frame (here a
beacon of 410 bytes), or 0. `RADIO-LEVEL` gives the level of that frame.

| Word | Effect | Time on the WROOM |
| --- | --- | --- |
| `RADIO-START ( -- )` | `PHY-INIT`, reset and ready state of the MAC, Forth `MAC-HAL-INIT`, RX list and MAC handler; then channel 1 | about 60 ms |
| `ch RADIO-CHANNEL! ( -- )` | the channel, 1 to 13; another number is an error | about 1 ms |
| `RADIO-CHANNEL@ ( -- ch )` | the channel; 0 while the radio is off | |
| `RADIO-BEACONS ( -- )` | the filter of a scan: then `RADIO-RECEIVE` gives the beacons of the access points | |
| `RADIO-RECEIVE ( -- a n \| 0 )` | the next frame: n bytes at a, from its first byte, without the header of the MAC and without the check sum (FCS); they stay until the next `RADIO-RECEIVE`. 0: no frame | |
| `RADIO-LEVEL ( -- dbm )` | the level of that frame, in dBm (-90 is weak, -30 strong) | |
| `RADIO-MAC! ( a -- )` | own 6-byte unicast address for interface 0, after `RADIO-START`; copied, not borrowed | |
| `RADIO-MAC@ ( -- a )` | the configured address; do not change its bytes | |
| `RADIO-SEND ( a n ack? -- )` | send one frame without FCS; TRUE requests an ACK; then the header source must equal the configured own MAC | at most 100 ms for the completion; broadcast tested on the board, ACK on the host |
| `RADIO-POOL ( -- a n )` | the RAM of the chip for the packet buffers of the LINKs: SRAM1 from `$3FFE_0500`, 14 880 bytes | |
| `RADIO-FACTORY ( -- )`, `RADIO-RANDOM ( a n -- )` | the own address from the eFuses; random bytes of the hardware (section "The WPA2 station") | |
| `bssid RADIO-STATION ( -- )` | the filters of a station of the access point bssid | |
| `RADIO-AP ( -- )`, `peer RADIO-AP-PEER! ( -- )` | the filters of an AP: first only authentications and probes, then only the frames of the device peer | |
| `key peer id group? RADIO-CCMP-KEY! ( -- )`, `RADIO-KEYS-CLEAR ( -- )` | a key into the MAC (slot 0 the pair, 1 to 4 the group keys 0 to 3); all keys out | |
| `RADIO-CCMP? ( -- f )`, `RADIO-DECRYPTED? ( -- f )` | the MAC has keys; the MAC has decrypted the frame of the last `RADIO-RECEIVE` | |
| `RADIO-SOFT-CCMP ( -- )` | protected frames in software (a radio without keys in the MAC) | |
| `a n ack? RADIO-TX-START ( -- )`, `RADIO-TX-POLL ( -- n )` | send without waiting; the result later: 1 at work, 0 done, else the code | |
| `RADIO-TX-BUSY? RADIO-TX-BLOCKED? ( -- f )`, `RADIO-TX-CANCEL ( -- )`, `RADIO-TX-END ( -- )` | the state of a send; cancel it; let it end (at most 200 ms) | |
| `RADIO-TRACK ( -- )` | `PHY-TRACK` after 5 sends, while none runs (the foreground) | |
| `RADIO-STOP ( -- )` | the radio off: no reception, the handler off, `PHY-OFF` | |
| `RADIO ( -- )` | `ALSO RADIO` gives ESP32-RADIO, as `ALSO CHIP` gives the layer of the chip | |

While the radio is off, `RADIO-CHANNEL!`, `RADIO-BEACONS` and
`RADIO-RECEIVE`, the MAC words and `RADIO-SEND` stop with
`ESP32-RADIO: the radio is off`.

### How it receives

- The MAC writes each frame with DMA into a buffer in SRAM1, outside
  the memory of the VM: 8 buffers of 1 600 bytes. Each one has a
  descriptor of 3 cells: bits 0 to 11 the size of the buffer, 12 to 23
  the length of the data, bit 30 the end of a frame, bit 31 for the
  DMA; the address of the buffer; the next descriptor (0 at the end).
  The descriptors make a list, and the DMA fills one after the other.
- `RADIO-RECEIVE` first gives the descriptor of the call before back,
  at the end of the list. Then it takes the first descriptor if it
  holds the end of a frame. The DMA begins again with a descriptor that
  comes back only when it stands at the end of the list; then the
  given descriptor is its new start.
- Before each frame the MAC writes a header of 28 bytes: byte 0 the
  level (with its sign; minus 96 gives dBm), bytes 24 and 25 (bits 0
  to 11) the length of the frame with its check sum (4 bytes, not in
  the buffer). After the frame the buffer is filled up to 4 bytes.
- A frame longer than a buffer (more than 1 572 bytes) fills several
  descriptors; only the last one has the end of the frame and a
  length. `RADIO-RECEIVE` gives them all back and counts the frame in
  `#LONG`. Beacons are shorter.
- The filter. After `RADIO-START` the MAC lets no frame through.
  `RADIO-BEACONS` sets the filter of a scan of interface 0: in
  `$3FF7_30D8` the bits `$117` become `$17`, in `$3FF7_30F8` the bits
  23 to 31 go (`HAL-INIT` sets two of them: control frames). The MAC
  then lets the beacons and the probe responses of the access points
  through; the chip has no setting for the beacons alone. So
  `RADIO-RECEIVE` reads the first byte of each frame, its type: it
  gives a beacon (`$80`) and gives each other frame back unread
  (`#OTHER` counts them). The filters by address stay off.
- A Forth handler (package INTR, level 1; source 0 goes to CPU
  interrupt 1) counts calls of the MAC handler (`#INTS`, `INT-BITS`,
  RX bits `$0100_0020`) and clears them. This is not a frame counter:
  several frames can wait when it runs, and TX also uses it. The
  measured DMA frame counter is at MAC + `$2CC`. Call
  `RADIO-RECEIVE` from one place only: from the program, or from one
  handler.
- In station mode and AP mode, the software checks the addresses too
  (`WANTED?`): see sections "The WPA2 station" and "The WLAN of the
  WROOM". The host test is `test/starx.fs`.

### Transmission (N3c-2)

The private word `TX-PREPARE` checks a
frame and copies it to its own buffer in SRAM1, after the RX buffers.
It prepares one DMA descriptor and the registers of hardware slot 2
(best effort), but does not set the bits that start a transmission.
The radio must be on and the slot free. The length without FCS must
be 24 to 1 572 bytes; the buffer reserves four more bytes for the FCS
that the hardware adds. Protected frames need software or hardware
CCMP enabled; a group receiver with ACK requested is an error before
any write.

`RADIO-SEND` starts the slot after preparation. The MAC handler saves
the result and PMD (the MAC protocol status) before clearing the slot
status. A slot completion with a PMD error is an error, not success.
Channel access timeout and collision are also errors. The handler
does not wait or run PHY calibration; the foreground calls
`PHY-TRACK` after every five slot completions.

Use `RADIO-SEND` only in the foreground, not in a Forth handler. It
waits with the handlers on for at most 100 ms per attempt. A known
ACK timeout retries at most three times (four attempts total), with
the same sequence, packet number and prepared bytes, plus the Retry
bit. Other errors and unknown ends do not retry. `RADIO-TX-POLL`
performs the same policy without waiting. The caller's bytes are
unchanged. It does not enable
handlers that the caller switched off. If the end is unknown, or
Ctrl-C interrupts the wait, it disables the slot and locks the TX
buffer until `RADIO-STOP` and `RADIO-START` reset the MAC. It saves
the raw PMD also on an unknown end. A late interrupt does not change
this result or release the lock. Stop cancels a pending sender;
channel, scan filter and restart cannot change while it runs.

The backoff (0 to 15) comes from the ESP32 hardware RNG
(`$6003_5144`, ESP-IDF `WDEV_RND_REG`), while the radio is on. This
is for channel access only, not a cryptographic random-number API.
Broadcast completion is tested on the board: six frames in 602 ms,
60 timer ticks (10 ms each), six send tests and all 25 RX tests pass.
ACK waiting is now tested on the board too. With RX buffers returned,
the normal frame receives an ACK, a different own receive address
reports an ACK timeout, and restoration receives an ACK again. The
four-test suite passes also with Forth MAC initialization (`ack: 4
tests, 0 errors`). It requires
`RADIO-MAC!` after every start and an exact match
of the source bytes in the header. A group address, zero own address,
missing configuration or different source is an error before a write.
A slot completion alone never proves an ACK: PMD must have no error.
Unknown PMD errors remain errors, not a default success. A successful
raw PMD can have nonzero telemetry bits; only the error field is tested.

Keep calling `RADIO-RECEIVE` while the radio is on, also when sending.
The MAC handler counts RX but does not return its buffers. Eight
unconsumed beacons can exhaust the list and prevent ACK reception.
An ACK request with DMA at the end is rejected before TX preparation,
with a message to receive the queued frames. The sender never discards
application data. Beacon-only tests have bounded RX housekeeping;
that helper explicitly rejects normal application receive mode.

Own-MAC setup is not automatic, so the RX address banks stay off after
the start as before. `RADIO-MAC!` enables only the own RA bank. A MAC
reset invalidates the configuration. After an unknown TX end, all
configuration words are blocked; Stop switches off the PHY and resets
the MAC before another calibration.

### The MAC initialization in Forth

`macinit.fs` (ESP32-MAC) sets up the MAC registers and calls the PHY
only for its rate/power tables. It calls no binary MAC function.
The old and new start agree on 69 register values on the WROOM;
the new start passes all 25 receive tests and all four ACK tests.
`MAC-SETUP` is a private deferred word; the standard is
`MAC-HAL-INIT`. `MAC-SELECT` accepts another initializer only while
the radio is off. The binary oracle was removed from the PHY selection
after validation and remains reproducible from git commit `9b67376`.
`esp32/test/mac.fs` explicitly reports that reference requirement when
loaded with the normal, cleaned PHY. Generated CODE files were rebuilt
with PICK, not edited by hand.

### The rules

- Load `lib/net/bytes.fs` before `radio.fs` and `wifi.fs`. It provides
  exact packet-byte comparison, independent of `CASE-SENSE`.
- Load `macinit.fs` after PHY and before Radio. Radio needs ESP32-PHY,
  ESP32-MAC, INTR and BYTES.
- `FREEZE` only while the radio is off. After a start (also from an
  image) the radio is off: `RADIO-START` first.
- Each `RADIO-START` calibrates again and makes a new list; frames that
  waited are gone.
- Its test needs an access point near the board, on the channel 1, 6
  or 11.
- The send test needs permission for channel 6. It sends at most six
  synthetic broadcast probe requests, 100 ms apart, with the owner's
  factory MAC. It does not wait for an ACK or associate.

### Memory

These sizes were measured on the WROOM after the Forth MAC start and
the RX-credit guard. The TX descriptor and buffer reserve
1 612 more bytes of SRAM1: `$3FFE_7660` to `$3FFE_7CAB` (since 09.10.;
before, the reception started at `$3FFE_8000`).

| | Dictionary | RAM data | SRAM1 |
| --- | --- | --- | --- |
| BYTES | 160 bytes | 24 bytes | |
| ESP32-MAC | 2 668 bytes | 24 bytes | |
| ESP32-RADIO | 8 724 bytes | 112 bytes | RX 12 896 bytes, TX 1 612 bytes, from `$3FFE_4400` up to `$3FFE_7CAB` |
| WIFI | 2 340 bytes | 1 616 bytes | |
| free after PHY, BYTES, MAC, RADIO and WIFI | 74 920 bytes | 58 240 bytes | the free figures include the small measurement words; measured with the old map of 192 KB, the standard map has 65 536 bytes more dictionary |

### Tests

On the host, with 32-bit cells: the list of the reception with a model
of the MAC (`test/radiohost.fs`, package MOCK-MAC: the registers as
measured on the board). It checks the start, frames in order, more
frames than descriptors, a list that goes round many times, the length
and the level from the header, a frame over two descriptors, the filter
of a scan, the handler, off and on again:

    build/ucf-c -32 host/files.fs lib/esp32/test/radiohost.fs </dev/null

Expected: `radiohost: 34 tests, 0 errors`. On the board, after the
loads of `phy.fs` and `radio.fs`: on and off, the channels, the list
and the handler, the filter of a scan with the beacons of the channels
1, 6 and 11 (each frame a beacon with a level), the clocks, and a timer
every 10 ms that keeps its time while the beacons come:

    board ok> #include forth/test/tester.fs
    board ok> #include lib/esp32/esp32/test/radio.fs
    radio: 25 tests, 0 errors

The private TX preparation uses the same model, with 8 KB for the
MAC registers (TX also uses offsets above `$1000`). Its test first
runs all 34 RX tests, then checks the TX descriptor, register values,
copied bytes, FCS reserve, memory limits and invalid inputs. No TX
slot starts; invalid inputs write nothing:

    build/ucf-c -32 host/files.fs lib/esp32/test/txhost.fs </dev/null

Expected: `radiohost: 34 tests, 0 errors`, then
`txhost: 36 tests, 0 errors`. The test also checks the model's write
and start-bit counters themselves. This is not a board TX test.

For TX completion and cancellation, the same model adds a clock and
interrupt events. It checks stale and foreign slot status, PMD before
clearing, channel access and protocol errors, the exact 100-ms
deadline and clock wrap, re-entry, Stop, Ctrl-C, power tracking and
RX during TX:

    build/ucf-c -32 host/files.fs lib/esp32/test/txdone.fs </dev/null

Expected: `radiohost: 34 tests, 0 errors`, `txhost: 36 tests, 0 errors`,
then `txdone: 37 tests, 0 errors`. C and awk give these results.

The synthetic probe fixture and factory-MAC byte order have a separate
host test, with made-up addresses:

    build/ucf-c -32 host/files.fs lib/esp32/test/probhost.fs </dev/null

Expected: `probhost: 12 tests, 0 errors`, including the unicast
destination, BSSID, source and duration. Only after permission for the
board send test, load `lib/esp32/test/probe.fs` and
`lib/esp32/esp32/test/snd.fs`; it checks six completions and a timer
that keeps its time. Measured: `snd: 6 tests, 0 errors`, six frames in
602 ms, 60 timer ticks and 12 MAC interrupts, then Stop and reset.

The ACK and own-MAC test includes all previous host TX tests:

    build/ucf-c -32 host/files.fs lib/esp32/test/ackhost.fs </dev/null

Expected: `radiohost` 34/0, `txhost` 36/0, `txdone` 37/0,
`ackhost` 37/0. C and awk give these results. It checks the exact
address, masks and enable bit, a copied address, invalid configuration,
header-source mismatch, actual ACK-start bits, PMD ACK timeouts and
unknown errors, and configuration after an unknown DMA end. This is
also passed on the board: `ack: 4 tests, 0 errors` with the Forth
MAC start. `esp32/test/ack.fs` needs an
`AP-INDEX` from a current scan, chosen by the owner locally. Its four
tests need TEST-PROBE and TEST-ACKCFG loaded. The initial failure
PMD `$5300` is history, not a success-shaped fallback; RX-credit
housekeeping is now explicit. Do not reuse an index after a reset.

`test/machost.fs`: `machost` 23/0 after RX 34/0, C and awk.
`esp32/test/mac.fs`: 69 register comparisons, zero differences.
`test/diaghost.fs`: `diaghost` 16/0, including reclaiming eight full
RX descriptors without starting TX or draining application mode.
`test/probhost.fs`: 21/0, including named probes of length 0 to 32.

The test of WIFI is in `lib/net/test/wifi.fs` (`lib/net/README.md`,
"Test").

## Protocol arithmetic on the WROOM (N3d)

The platform-independent packages SHA1, KDF1, AES128, CCM and WPAKEY
are described in `lib/README.md` and `lib/net/README.md`. The board
driver `esp32/test/n3d.fs` uses the same vectors as the host loader
`lib/test/n3d.fs`; it needs no PHY or radio. Before every port access,
use `lsof /dev/cu.usbserial-110` as a hard guard: do not continue if
another process has the port.

In `ucf`, after that guard:

    build/ucf term /dev/cu.usbserial-110 lib/esp32/esp32/test/n3d.fs </dev/null

The driver resets, loads packages and tests into RAM, then advances
the four long public derivations in bounded groups. Each line takes
less than the 10-s term limit. Expected: **n3d total: 52 0**.
The extra test checks the maximum step below 100 ms and the 10-ms
timer count within 50 ms of elapsed time. Measured: **49 ms** maximum
step, **14884** timer calls in **148850 ms**. This proves timer service
during the arithmetic, not an IR/WLAN application test.

The driver resets again and ends term: clean RAM, port closed, no
flash write. It is a term command file, not a FILES INCLUDE file.
See `lib/README.md` for the measured dictionary/RAM cost.

## The WPA2 station on the WROOM (N3e)

RADIO-FACTORY configures the own eFuse MAC without printing it.
RADIO-STATION sets interface 0's BSSID bank at offsets 0/4, masks
0x20/0x24, and the known filter fields under mask 0x117 to **0x07**,
preserving unknown fields. The existing receiver-address bank is
required. No scan or general management/data bypass remains.
RADIO-RECEIVE adds a software check of receiver, transmitter and BSSID
before giving a frame to the station. This is not a stronger assertion
about every possible hardware filter hit: see the limitations in
`orga/N3b3_MAC_Filter.md`.

RADIO-RANDOM fills 0..1024 caller bytes, only while radio is on. Reads
of **0x60035144** are spaced by 1 ms (well above 16 APB clocks).
No timestamp or fixed nonce is a fallback. The RF source must remain
enabled. [Register](https://github.com/espressif/esp-idf/blob/v5.5.2/components/soc/esp32/include/soc/wdev_reg.h)
and [entropy/sampling requirement](https://github.com/espressif/esp-idf/blob/v5.5.2/components/esp_hw_support/hw_random.c).

After a hard lsof check, in `ucf`:

    build/ucf term /dev/cu.usbserial-110 lib/esp32/esp32/test/n3eload.fs </dev/null

This resets, loads with short search orders, checks changing random
samples and scans passively. It prints only counts, package sizes and
profile validity. It does not transmit or write flash; it closes term
with radio/list left in RAM for local owner selection. The owner uses
`.WIFI` privately, chooses the row from 0, then WLAN-PASSWORD.
See `lib/net/README.md` for the input, wait and save contract. Never
reuse an index after reset or a fresh scan. Do not enter a password if
the packages disappeared on opening the terminal.

`esp32/test/wpasta.fs` is the synthetic RAM-only board test: 24 station
vectors plus the live-timer test, **25/0**. It resets and closes term;
no PHY or flash access. `test/stahost.fs` checks 11 station-filter/RNG
cases after the 34 existing RX tests, C and awk.

Measured dictionary/RAM bytes at the real load:

| Package | Dictionary | RAM data |
| --- | ---: | ---: |
| PHY | 7020 | 2696 |
| BYTES | 288 | 24 |
| MAC | 2668 | 24 |
| RADIO | 10164 | 128 |
| WIFI | 4100 | 5852 |
| WLAN | 8896 | 2072 |
| WLAN-SETUP | 624 | 92 |
| WLSAVE | 2844 | 428 |

The N3d packages SHA1, KDF1, AES128 and WPAKEY are also loaded
(`lib/README.md`). Before the size-report helpers were added, the
complete real-load session had **48572/49640** dictionary/RAM bytes
free, including small RNG-test buffers. The original clean boot image
was not replaced. Only the owner's explicitly approved two profile
sectors changed. End of the real test: station left, radio off, reset,
RAM keys gone, serial port closed; both profile copies remain.

## The WLAN of the WROOM: station, setup network, sockets

The WROOM joins a WPA2 network as a station, keeps the connection, and
our TCP/IP gives sockets over it (`lib/net/README.md`, "The words of an
application"). For the setup, it can also open a temporary network of
its own for one device. Only one of the two runs at a time; the board
never routes between them.

### Load

`apps/irbridge/app.fs` loads all of it onto the base image and freezes
it (`apps/irbridge/README.md`). Its parts, for ucf term, right after a
reset:

| File | Loads |
| --- | --- |
| `esp32/wlstack.fs` | ESP32-PHY, BYTES, ESP32-MAC, ESP32-RADIO, WIFI, SHA1, KDF1, AES128, WPAKEY, WLAN, WLSAVE, TEXT, INET, CCMP, WIFI-LINK, AP-LINK, RADIO-LINK; then `RADIO-POOL LINK-POOL!` |
| `tcpip.fs` | our TCP/IP: ARP, IPV4, UDP, TCP, DHCP, LINK-NET, NET, DNS, NET-TIMER |
| `esp32/wlback.fs` | WLAN-BACK: the station with the saved profile |
| `../net/dhcps.fs`, `esp32/wlap.fs` | DHCP-SERVER and SETUP-AP: the setup network |

The radio does the CCMP of the data frames itself, so the CCMP in
software (`lib/net/wlsoft.fs`, package CCM) is not loaded. The base has
not enough RAM data for all buffers: after `wlstack.fs`,
`ALSO BOOTIMG XDATA-FREEZE` puts the first part into the flash and
gives the RAM behind it to the next buffers (package BOOTIMG).

### The station: WLAN-BACK

The profile (the name of the network and its key, never the password)
lies in the flash (package WLSAVE, section "The WPA2 station on the
WROOM"). WLAN-BACK uses it without a terminal. A step of it runs in
the chain of IDLE, while the console waits:

| Word | Effect |
| --- | --- |
| `WBACK-START ( -- )` | the radio on, a scan in steps over the channels 1 to 13, the strongest access point with the saved name, the join, WIFI-LINK, `NET-START`, DHCP |
| `WBACK-READY? ( -- f )` | joined, the link up, the address from DHCP |
| `.WBACK ( -- )` | the state (0 wait, 1 scan, 2 join, 3 DHCP, 4 ready), the attempts, the joins, the code of the last failure |
| `WBACK-STOP ( -- )` | the station tells the access point that it goes (a deauthentication), then the network and the radio go off; the keys in the RAM go, the profile stays |
| `WBACK-RUNNING? ( -- f )` | between `WBACK-START` and `WBACK-STOP` |
| `WLAN-BOOT-ON ( -- )` | after each start of the board: `WBACK-START`, then the console (TURNKEY); `FREEZE` keeps it |
| `WLAN-BOOT-OFF ( -- )` | the normal start again, the radio off; `FREEZE` keeps it |

When the beacons of the access point stay away for 5 s, or the link
fails, WLAN-BACK stops all and tries again after a pause: 1 s, then 2,
4, 8 ... at most 32 s, until `WBACK-STOP`. An error in one of its steps
does the same; only Ctrl-C goes through. It shows numbers only.

    board ok> ALSO WLAN-BACK  WBACK-START
    WLAN back: link and DHCP ready
    board ok> WBACK-READY? .  .WBACK
    -1 WLAN back state/attempts/joins/code: 4 1 1 0

Then the words of NET work as on a host. `FREEZE` only after
`WBACK-STOP`: no key may go into the flash.

**The CCMP in the radio.** The MAC of the ESP32 encrypts and decrypts
the data frames when it has the keys. WLAN gives them to it with
`RADIO-CCMP-KEY!` after the 4-way handshake and after a new group key:
slot 0 holds the key of the pair, the slots 1 to 4 the group keys 0 to
3. A slot is off while it changes, and a key goes in only when it is
new. `WIFI-LEAVE`, a failure and `RADIO-STOP` clear all slots. The MAC
writes the plain text of a received frame after its CCMP header and
leaves out MIC and FCS; `RADIO-RECEIVE` gives the shorter length, and
`RADIO-DECRYPTED?` says that this frame was decrypted. WIFI-LINK still
checks the addresses, the packet number (no frame twice) and the key
generation. The register facts come from esp-wifi-hal 0.3.3
(`src/ll.rs`) and the register map esp32 0.40.2 (`crypto_control`,
`crypto_key_slot`).

### The setup network: SETUP-AP

`SETUP-START` makes the board an open access point. The first device
that logs on is the only one until `SETUP-STOP`: the radio then gives
only its frames to the program (`RADIO-AP-PEER!`). It gets its address
by DHCP. Stop the station first:

    board ok> ALSO WLAN-BACK WBACK-STOP  ALSO SETUP-AP
    board ok> S" ucf-setup" 6 SETUP-START

| Word | Effect |
| --- | --- |
| `a n ch SETUP-START ( -- )` | the network with the name a n on channel ch, open; the board is 192.168.77.1/24 |
| `SETUP-STOP ( -- )` | a deauthentication to the device (a device in power save can miss it and sees the end some seconds later); AP, DHCP and the network go off |
| `.SETUP ( -- )` | the state of the device (0 none, 1 authenticated, 2 associated), frames in, out, dropped, the last code; the replies of DHCP |

The device gets 192.168.77.2, a lease of 600 s, no router and no DNS
server: it reaches only the board, and a phone may ask whether it
shall stay in a network without Internet. The words of NET work as with
the station. The network is open and its data readable, and another
device could log on first: use it only for a short time, and send no
secret over it. The AP knows the legacy OFDM rates (6 to 54 Mbit/s),
frames without fragments and without A-MSDU, and no power save of the
device: a frame to all goes to the device as a frame to it. After
`SETUP-STOP`, `WBACK-START` joins the saved network again.

A test reply for a browser: `#include lib/esp32/esp32/test/aphttp.fs`,
`AP-HTTP-START`; then `http://192.168.77.1:8080/` on the device gives
`ucf setup AP: ok`. `AP-HTTP-STOP` ends it.

### Memory and speed

Measured on the WROOM at 240 MHz with all of `apps/irbridge/app.fs`
in the flash:

| | |
| --- | --- |
| free: dictionary, RAM data, RAM behind the frozen part (XDATA) | 27 436, 41 248, 99 016 bytes |
| the packet RAM of the LINKs (`RADIO-POOL`) | 14 880 bytes in SRAM1 from `$3FFE_0500` (up to the data of the ROM); WIFI-LINK takes 11 392, AP-LINK 8 168 |
| `WBACK-START` to `WBACK-READY?`, the access point near | 2.1 s |
| ping to the router | 5 to 15 ms |
| TCP echo with a host, 10 times 1000 bytes each way | 183 to 396 ms |

### Tests

On the host, in `tools/test.fs` (each also alone, with
`build/ucf-c -32 host/files.fs FILE </dev/null`): `test/ackhost.fs`
(with `retry.fs`: the ACK and the repeats), `test/ccmpreg.fs` (the key
slots), `test/setup.fs` (with `aphost.fs`: the AP and SETUP-AP),
`test/wlback.fs`, and in `lib/net/test/`: `hwrekey.fs` (WIFI-LINK over
hardware and software CCMP, a new group key), `linksel.fs`,
`aphttp.fs` (with `dhcps.fs`), `netdown.fs`.

On the board, with the image of `apps/irbridge/app.fs` and
`WBACK-READY?` true: `#include lib/net/test/board.fs` (85 tests of NET
over the WLAN); `esp32/test/airping.fs` (`AIR-PING`, an echo of the router);
`esp32/test/airsock.fs` (UDP and TCP with a host that runs
`lib/net/test/echopeer.fs`); `esp32/test/airtime.fs` (the time of each
`NET-STEP`).

## IR for the bridge on the WROOM: IR-CAPTURE and IR-QUEUE

The bridge sends and receives IR frames while the console and the
network work, and loses no frame. The RMT of the ESP32 cannot take a
frame in parts (it has no RX wrap: ESP-IDF 5.5.2, `rmt_ll.h`), so a
frame has a fixed limit: channel 4 takes 4 blocks, 256 pulse words.

- IR-CAPTURE (`esp32/ircap.fs`): a CODE handler at interrupt level 2
  copies each frame into one of 4 raw slots in the internal RAM and
  starts the reception again, in about 21 us. It uses no Forth word, no
  flash and no ROM function.
- IR-QUEUE (`irqueue.fs`): a timer word decodes at most 16 pulse words
  per step and puts each frame into a ring of 16 frames of at most 400
  times. A word in IDLE sends: after at least 15 ms without light, with
  the receiver on; the timer compares the echo, at most 32 times per
  step.

A frame longer than 256 pulse words or 400 times is counted and
dropped, never cut; so is the rest of it up to its end. A frame that
comes while the queue waits for its echo, and is not the echo, goes
into the ring like any other.

| Word of IR-QUEUE | Effect |
| --- | --- |
| `out in IR-Q-START ( -- )` | the pins (38 kHz, 33 %), the capture, the timer word and the word in IDLE |
| `IR-Q-STOP ( -- )` | all off; the buffers are cleared |
| `IR-Q-FRAME? ( -- a n end-us true \| false )` | the next frame: n times at a; valid until the next call or `IR-Q-STOP` |
| `a n IR-Q-SEND ( -- ior )` | copy a frame of n times (n odd: it ends with light); 0: accepted |
| `IR-Q-RESULT ( -- ior \| 1 )` | 1 at work; 0 sent, the echo matched; else the code |
| `IR-Q-IDLE-HIGH ( -- a )` | TRUE: the receiver gives a high level without light |
| `.IR-Q ( -- )` | the counters, the longest step of the timer word, those of IR-CAPTURE |

| Code | Meaning |
| --- | --- |
| -450 | a time or a count out of range, or a frame longer than 400 times |
| -451 | a send waits already, or the ring was full |
| -452 | no 15 ms without light within 2 s |
| -453 | no matching echo after 3 attempts |
| -454 | the queue does not run |

While the queue runs, it alone uses the RMT channels 1 (TX) and 4 (RX):
do not use `IR-LISTEN`, `IR-FRAME?` or `IR-SEND` then. An error in its
timer word or its word in IDLE counts in `.IR-Q`; the service goes on.
Stop the queue before `FREEZE`.

With the transceiver of Adafruit (product 5990) at 3V3, GND, IRout
GPIO 4 and IRin GPIO 14:

    board ok> ALSO IR-QUEUE  4 14 IR-Q-START
    board ok> CREATE F 2000 , 1000 , 600 , 600 , 600 ,
    board ok> F 5 IR-Q-SEND .
    0
    board ok> IR-Q-RESULT .
    0

IR-Q-SEND only takes the frame; the word in IDLE sends it while the
console waits for the next line, so IR-Q-RESULT gives 1 until then.
**The echo of long frames.** The receiver gives each time of light
shorter and each time of darkness longer than sent, by the same amount;
on long frames the shift grows. Measured on the WROOM: frames of 51
times match, frames of 201 times differ by up to 45 us, frames of 399
times by up to 128 us; a pair of light and darkness differs by at most
27 us. With the rule "each time within 40 us", long frames end with
-453 although they went out well.

**Troubleshooting.** Each send ends with -452 and `.IR-Q` shows many
short frames: the input pin floats (50 Hz from the mains). Check the
wire IRin to GPIO 14 and the supply of the receiver.

Tests: on the host `test/irqueue.fs` (in `tools/test.fs`); on the board,
with the image of `apps/irbridge/app.fs`, `esp32/test/irqueue.fs` and then
`esp32/test/irqbusy.fs` (GPIO 25, a loop inside the chip, no light).
Measured: the CODE handler at most 5 038 CPU cycles (21 us), a step of
the timer word at most 326 us.

## A new driver

A new driver starts with `PACKAGE name ALSO CHIP ALSO GPIO` and takes
the values of the chip and the base from there. A driver has the same
names on each chip (`OUTPUT`, `HIGH`, `PIN@` ...). If its code differs
from chip to chip, it has a file in each folder of a chip and a front
file here with `#when` lines (as `rmt.fs`). A board file gives the
pins of one board their names (`LED`, `BUTTON`) and ends with `BOARD`.
A package for the board goes to the board with `ucf term`
(`targets/esp32s3/README.md`):

    board ok> #include lib/esp32/ws2812.fs

The host packages need FILES; ESPROM needs SERIAL too (the HAL
primitives 94-96). The tool `ucf` (`tools/ucf.fs`) loads them.

Tests: the board packages are tested on the board, in `ucf term`, after
`forth/test/tester.fs`: `test/timers.fs`, and the tests of the chip in
`esp32s3/test/` or `esp32/test/` (`targets/esp32s3/README.md` and
`targets/esp32/README.md`, "Test").
