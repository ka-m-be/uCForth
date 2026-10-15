# esp32: uCForth for the ESP32

The result is `build/esp32.bin`: uCForth for a board with the classic
ESP32 chip (tested board: ESP-WROOM-32 N4 of diymore),
in the format of the ROM bootloader of the chip. The tool `ucf`
(`tools/ucf.fs`) runs it in the RAM, writes it to the flash, and is the
console with `#include`. The ESP32-S3 has its own target
(`targets/esp32s3`); `lib/esp32/README.md` compares the two chips
("Which board").

**Status:** tested on the board (diymore ESP-WROOM-32 N4):
`ucf info`, `run` and `flash`; t0 and hayes over `#include`: 1058 tests,
0 errors; the board starts uCForth from the flash, with the CPU at 240
MHz; `FREEZE` on the board writes the running system to the flash, and
the frozen part of the dictionary stays there: the kernel reads it
through the cache, and its RAM is free for RAM data (section "Memory");
Ctrl-C stops the word that runs, an error of the CPU gives `THROW`, and
Forth words run as handlers of interrupts, with the network in the
background (two levels, as on the ESP32-S3); GPIO with interrupts of the
pins and the GPIO matrix; words that run every n ms (`EVERY`, `AFTER`);
the RMT sends and receives pulses (IR remote controls); `CODE` words
with the assembler, their code in SRAM0 (section "CODE words"); the
calibration of the radio with code of C of Espressif, and the reception
of beacons: a list of the Wi-Fi networks around (section "The radio");
the monitor (`asm/monitor/README.md`). Also in the QEMU of
Espressif (section "Test").

The words are the same as on the ESP32-S3: the packages in `lib/esp32/`
take the addresses of the chip from its layer (package ESP32,
`lib/esp32/esp32/chip.fs`). So this README shows what is different;
`targets/esp32s3/README.md` has the words in more detail.

## The board

Tested board: the board ESP-WROOM-32 N4 of diymore, with the module
ESP32-WROOM-32 N4, a USB serial bridge CH340C, the buttons EN and BOOT
and an LED at GPIO 2. Its package is DIYMORE-WROOM-N4
(`lib/esp32/esp32/diymore/wroom-n4.fs`; `ALSO BOARD` after it is
loaded, lib/esp32/README.md). Many boards with this module ("ESP32
DevKit") are made in this way; this table shows what to look at on
another one.

| Part | On this board | What it means for uCForth |
| --- | --- | --- |
| Module | ESP32-WROOM-32 N4: the chip ESP32-D0WD-V3 (revision v3.1, two cores, a crystal of 40 MHz) and 4 MB flash; no PSRAM | uCForth runs on the first core; the image is at `$1000` in the flash; GPIO 6 to 11 belong to the flash |
| USB socket | a bridge CH340C to UART0 (GPIO 1 and 3) | the console of uCForth; on a Mac `/dev/cu.usbserial-...` (the number is the USB port of the Mac: here `110`); DTR and RTS reach EN and GPIO 0, so `ucf` restarts the board and starts the download mode itself |
| Buttons | EN and BOOT (GPIO 0) | EN restarts the chip; BOOT held down during EN starts the download mode of the ROM; `BUTTON` in the package of the board |
| LED | an LED at GPIO 2 | `LED`, `LED-ON`, `LED-OFF` in the package of the board |
| Strapping pins | GPIO 0, 2, 5, 12 and 15: the chip reads their levels at a reset | GPIO 0 low: the download mode (GPIO 2 must be low then too). GPIO 12 high: the flash gets 1.8 V instead of 3.3 V, and the board does not start. GPIO 15 low: the ROM sends no lines at the start. Nothing may pull them to another level during a reset |
| Inputs only | GPIO 34 to 39 | no output, no pull resistors (`PULLUP` gives an error) |

So these pins are not free for an application: 0 (BOOT), 1 and 3 (the
console), 2 (the LED), 6 to 11 (the flash), and 12 only with care. The
IR transceiver of `apps/aircon` uses GPIO 4 and 14, as on the ESP32-S3.
The tests use GPIO 25 (section "Test"): nothing may be connected there
while they run.

## Build

In the folder `ucf`, with a bootstrap VM with 32-bit cells:

    LC_ALL=C awk -f bootstrap/awk/ucf.awk -- -32 lib/package.fs lib/idle.fs lib/esp32/rom.fs lib/esp32/esp32/chip.fs \
       lib/esp32/intr.fs lib/esp32/gpio.fs lib/esp32/timers.fs lib/esp32/flash.fs lib/esp32/esp32/clock.fs \
       lib/esp32/esp32/bootimg.fs targets/esp32/boot.fs targets/freeze.fs host/files.fs targets/esp32/build.fs </dev/null

The output ends with:

      saved: ucf.hex
      code: 10166 bytes at $40080000, start at $400818D0
      image: $100-$00000B38 at $3FFE8100, $00010A00-$000197B0 at $3FFB0A00
      start word: ON-BOOT
      dictionary: up to $0003FFF0 (DICT-END)
      FREEZE on the board: yes
      interrupts: yes
      Ctrl-C waits for NO-BREAK: yes
      written: build/esp32.bin (49152 bytes)

1. `lib/package.fs` comes first: the packages for the board
   (`PACKAGE` ... `END-PACKAGE`) need it on the board too. Then
   `lib/idle.fs`, the package IDLE (`targets/esp32s3/README.md`, "Two
   levels").
2. The packages for the board in `lib/esp32/` (its README): ROM (the
   calls of the ROM), ESP32 (the layer of the chip: its addresses and
   numbers, `lib/esp32/esp32/chip.fs`), INTR (interrupts, errors of the
   CPU), GPIO (the base of the drivers), TIMERS (`EVERY`, `AFTER`),
   FLASH, and from `lib/esp32/esp32/` CLOCK and BOOTIMG (`FREEZE` on the
   board), in this order. Then `targets/esp32/boot.fs`: the image starts
   with `ON-BOOT`, which sets the CPU to 240 MHz, then runs `BOOT`.
   Without these ten files the image is 16 KB smaller, the CPU stays at
   40 MHz, `FREEZE` on the board gives the error -21, there are no
   handlers of interrupts, and an error of the CPU gives -9 without its
   address.
3. `targets/freeze.fs`: the running system is the image, without the
   package FILES (the board has no files). Put your application files
   before it.
4. `build.fs` makes the code with the assembler `asm/xtensa.fs`: the
   kernel `kernels/xtensa.fs` (the same as for the ESP32-S3) and
   `hal.fs` (start, UART0 with its interrupt, time, restart, the calls
   of the ROM, the vectors for exceptions and interrupts). It writes
   `build/esp32.bin` (section "The image format of the ROM
   bootloader"), with the RAM data and the dictionary at their two
   places (section "Memory"), and in it the end of the code
   (`CODE-START` of the package BOOTIMG, section "CODE words"). All of
   its dictionary is in the RAM; the first `FREEZE` on the board puts
   the frozen part into the flash (section "FREEZE on the board").

The tool `ucf` for the Mac (see `tools/ucf.fs`):

    LC_ALL=C awk -f bootstrap/awk/ucf.awk -- -32 host/files.fs tools/ucf.fs targets/macos-a64/build.fs </dev/null
    chmod +x build/ucf

## Run

Connect the USB socket of the board. `ls /dev/cu.*` shows its port:
here `/dev/cu.usbserial-110`. The ROM bootloader of the chip answers;
`ucf` starts it over the lines DTR and RTS:

    $ build/ucf info /dev/cu.usbserial-110
    connecting .. ok
    chip        ESP32-D0WD-V3, revision v3.1
    features    Wi-Fi, Bluetooth, two cores
    in package  none
    link        UART
    mac         ...
    crystal     40 MHz

In the RAM (the flash stays as it is; a reset starts the old program).
`ucf run` also sets up the flash through the ROM, so the flash words
and `FREEZE` work as after a start from the flash:

    $ build/ucf run /dev/cu.usbserial-110 build/esp32.bin
    connecting .. ok
    loading build/esp32.bin: 10168 bytes at $40080000 2560 bytes at $3FFE8100 34400 bytes at $3FFB0A00
    started at $400818D0
    term on /dev/cu.usbserial-110: #help shows the commands
    board ok>

In the flash, at address `$1000` (the ROM of the ESP32 reads the image
there), then a restart. This replaces the bootloader of the factory
firmware; the rest of that firmware stays in the flash, but it does not
start any more:

    $ build/ucf flash /dev/cu.usbserial-110 build/esp32.bin 0x1000
    connecting .. ok
    erasing 46 KB at $1000 ...
    writing .. ok
    checking MD5 ... ok
    46 KB in 5.1 s
    The board waits in the bootloader; reset starts it.
    $ build/ucf reset /dev/cu.usbserial-110
    $ build/ucf term /dev/cu.usbserial-110

The download mode of the ROM stays: it is in the mask ROM of the chip.
`ucf` enters it over DTR and RTS; else hold BOOT, press EN, release
BOOT. So another program can always go to the flash again.

## Develop on the board

`ucf term` is the console, with the same commands as for the ESP32-S3
(`#include`, `#require`, `#when`, `#reset`, Ctrl-C:
`targets/esp32s3/README.md`, "Develop on the board"; host/README.md).
After each start, the search order has only FORTH. The packages of the
image are on the board; `ALSO GPIO` gives the words for the pins, `ALSO
ESP32` the addresses and numbers of the chip, and so on:

    board ok> ORDER
    FORTH   current: FORTH
    board ok> VOCS
    BOOTIMG.I BOOTIMG CLOCK.I CLOCK FLASH.I FLASH TIMERS.I TIMERS GPIO.I
    GPIO INTR.I INTR ESP32.I ESP32 ROM.I ROM PACKAGES.I INTERNALS FORTH

The LED of the board goes on and off (`PIN@` reads the level at the
pin, also of an output):

    board ok> ALSO GPIO  2 OUTPUT  2 HIGH  2 PIN@ .
    -1
    board ok> 2 LOW

The same with the package of the board; `examples/blink.fs` loads it
with `#require BOARD lib/esp32/board.fs` (here
`lib/esp32/esp32/diymore/wroom-n4.fs`), and `BLINK` lets the LED blink
until the return key:

    board ok> #include examples/blink.fs
    board ok> ORDER
    DIYMORE-WROOM-N4 FORTH   current: FORTH
    board ok> LED-ON  LED-OFF
    board ok> BLINK

A file for the chip of the board: `#when ESP32 FILE` sends FILE only to
a board with the package ESP32 (host/README.md). So
`lib/esp32/rmt.fs` loads the package RMT of this chip.

## FREEZE on the board

`FREEZE` writes the running system to the flash, in two steps
(package BOOTIMG, `lib/esp32/esp32/bootimg.fs`):

1. The dictionary up to `HERE` goes to the flash, at `$6_0000` + VM
   address, and stays there: below `FLASH-END` the kernel reads the
   words from the flash (section "Memory"). The first `FREEZE` of an
   image without a frozen part (after `ucf flash` or `ucf run`) erases
   the flash of the dictionary first (192 KB); each later `FREEZE` only
   appends the new words.
2. The boot image at flash address `$1000`: the code of the kernel and
   of the `CODE` words (section "CODE words"), the RAM data, and with
   `XDATA-ON` the RAM data behind the frozen part. The dictionary is no
   longer in it.

After a restart, the board starts this image:

    board ok> : HELLO ." hello from the flash" CR ;
    board ok> FREEZE
    board ok> #reset
    ets Jul 29 2019 12:21:46

    rst:0x1 (POWERON_RESET),boot:0x13 (SPI_FAST_FLASH_BOOT)
    ...
    load:0x40080000,len:10168
    load:0x3ffe8100,len:2560
    entry 0x400818d0
    board ok> HELLO
    hello from the flash

After the restart, the search order has only FORTH again. The system
runs on after `FREEZE`, without a restart. A store into the frozen part
gives the error -9. Measured on the WROOM at 240 MHz: the first `FREEZE`
of the image of the section "Build" 0.3 to 0.5 s, a `FREEZE` that
appends a few words 0.1 s; with the IR bridge (`apps/irbridge/app.fs`:
163 KB of dictionary, 57 KB of code, 89 KB of RAM data) 0.9 s. The
first `FREEZE` of an image of `build.fs` also erases the 192 KB of the
dictionary in the flash first. It uses
the functions of the ROM for the flash (packages ROM, FLASH and BOOTIMG;
`FLASH-SYSCALL` switches the cache off meanwhile). The image of
`FREEZE` has no SHA-256 digest: the ROM of the ESP32 does not check it.
`ucf flash PORT build/esp32.bin 0x1000` writes the image from the Mac
again; its dictionary is all in the RAM again.

`XDATA-ON` gives the RAM behind the frozen part to the RAM data
(section "Memory"); `FREEZE` saves the setting:

    board ok> ALSO BOOTIMG  FREEZE  XDATA-ON  FREEZE

## The clock

The ROM starts the CPU at 40 MHz from the crystal. `ON-BOOT`
(`targets/esp32/boot.fs`) switches it to 240 MHz with the package
CLOCK (`lib/esp32/esp32/clock.fs`; `ALSO CLOCK` gives its words):

| Word | Effect |
| --- | --- |
| `n MHZ` | the CPU clock: 40 (crystal), 80, 160 or 240 (PLL) |
| `CPU-MHZ ( -- n )` | the CPU clock now |

**Unlike on the ESP32-S3, UART0, the timer groups and the RMT count the
APB clock:** 40 MHz with the crystal, 80 MHz with the PLL. `MHZ` gives
UART0 and the timers new dividers, so the console and `MS` do not
change: `5000 MS` lasts 5.00 s at each clock (measured with the clock
of the Mac). The RMT takes the clock at the time when a channel is set
up (`IR-PINS`, `RMT-DIV`): change the CPU clock before that, not after.
The SPI flash runs at the APB clock / 2. For 80 and 160 MHz the PLL
runs at 320 MHz, for 240 MHz at 480 MHz; each change goes through the
crystal, as in ESP-IDF. Measured on the board:

| Clock | 1 000 000 times `DO LOOP` | `25 FIB` (recursive) |
| --- | --- | --- |
| 40 MHz | 850 ms | 1490 ms |
| 80 MHz | 425 ms | 745 ms |
| 160 MHz | 212 ms | 373 ms |
| 240 MHz | 142 ms | 248 ms |

## Interrupts and errors of the CPU

As on the ESP32-S3 (`targets/esp32s3/README.md`, "Interrupts and
errors of the CPU"): an error of the CPU gives `THROW`, Ctrl-C stops
the word that runs (also `MS`; it waits for the end of a handler and of
a word of the network; twice within 1 s also the handlers), and Forth
words are the handlers of interrupts (package INTR: `INT!`, `INT@`,
`INT-ON`, `INT-OFF`, `+INT`, `-INT`); an error in a handler stops only
this handler:

    board ok> $10000000 @
      error: invalid memory address: $10000000 at pc $40080382
    board ok>

The ESP32 has 69 sources of interrupts (Technical Reference Manual of
the ESP32, version 4.1, table 7), for example timer group 0 14 to
17, timer group 1 18 to 21, GPIO 22, FROM_CPU 24 to 27 (a register of
the CPU sets them: good for tests without wires), UART0 34, the RMT 47.
Example with FROM_CPU 0 (source 24, register
`DPORT_CPU_INTR_FROM_CPU_0` at `$3FF0_00DC`):

    board ok> ALSO INTR  VARIABLE HITS
    board ok> : H  0 $3FF000DC !  1 HITS +! ;
    board ok> ' H 24 INT!  24 INT-ON  +INT
    board ok> 1 $3FF000DC !
    board ok> HITS @ .
    1

| Level | CPU interrupt | For |
| --- | --- | --- |
| 1 | 1 | the Forth handlers, at the next NEXT |
| 2 | 19, 20, 21 | a `CODE` word each, at once (`INT!` with a `CODE` word, section "CODE words") |
| 3 | 23 | UART0: the console and Ctrl-C |

## GPIO

The package GPIO (`lib/esp32/gpio.fs`, in the image) has the same words
as on the ESP32-S3 (`targets/esp32s3/README.md`, "GPIO"): `OUTPUT`,
`INPUT`, `PULLUP`, `PULLDOWN`, `OPEN-DRAIN`, `HIGH`, `LOW`, `TOGGLE`,
`PIN!`, `PIN@`, `DRIVE`, `SIGNAL>PIN`, `PIN>SIGNAL`, `PIN-INT!`. The
differences come from the layer of the chip:

- Pin numbers are GPIO numbers, 0 to 39, without 20, 24 and 28 to 31.
  34 to 39 are inputs only, without pull resistors.
- The IO MUX has its registers not in the order of the pins; `>MUX`
  (package ESP32) takes them from a table.
- The pins with an RTC function (0, 2, 4, 12 to 15, 25 to 27, 32, 33)
  have their pull resistors in the RTC IO; `PULLUP` and `PULLDOWN` find
  them there (`>PULL`).
- The signals of the GPIO matrix have other numbers (Technical
  Reference Manual of the ESP32, version 4.1, table 17; the pins: tables
  18 and 19). The interrupt of the pins is source 22.

The button BOOT is GPIO 0; it pulls the pin to 0 V:

    board ok> ALSO GPIO  ALSO INTR
    board ok> : PRESSED ( -- )  ." BOOT" CR ;
    board ok> ' PRESSED 0 FALLING PIN-INT!  +INT

## Words that run later

The package TIMERS (`lib/esp32/timers.fs`, in the image): `EVERY`,
`AFTER`, `.TIMERS`, as on the ESP32-S3 (`targets/esp32s3/README.md`,
"Words that run later"; there also: what a word of a timer, and each
Forth handler, must not do). Here they run on timer 0 of timer group 1
(source 18). The two levels of the ESP32-S3 (section "Two levels"
there: the network in a timer, the chain of IDLE at the console) work
here too; the ESP32 has no PSRAM, so the buffers are smaller.

## CODE words

`lib/esp32/asm.fs` (package ASM, `lib/esp32/esp32/asm.fs`) makes `CODE`
words with the assembler XTENSA, with the words of the ESP32-S3
(`targets/esp32s3/README.md`, "CODE words: the assembler on the board"):

    board ok> #require FAIL lib/text.fs
    board ok> #require ADD, asm/xtensa.fs
    board ok> #include lib/esp32/asm.fs
    board ok> CODE 3+ ( n -- n+3 )   A2 TOS@,  A2 A2 3 ADDI,  A2 TOS!,  NEXT,  END-CODE
    board ok> 4 3+ .
    7

The CPU of the ESP32 runs no code in the RAM for data, where the
dictionary is. So the assembler writes the code into a buffer of 2 KB
(at most 2 KB for one word), and `END-CODE` copies it into SRAM0, the
RAM of the instruction bus, a cell at a time (the CPU writes SRAM0 only
in cells). The code of the `CODE` words follows the code of the kernel:
from `CODE-START` up to `CODE-HERE` (package BOOTIMG; 120 KB from
`$4008_1FAC` up to `$400A_0000`). The cell after the code field of a `CODE` word holds the
address of its code (`>CODE`); the HAL has its own primitive 66 for it
(`targets/esp32/hal.fs`, "CODE words"). `FREEZE` saves this code with
the kernel; `MARKER` gives the dictionary back, not SRAM0: the code of
the words that it forgets stays there until the next start.

A `CODE` word that ends with `RET,` can be the handler of an interrupt,
at once, on level 2 (`INT!` of the package INTR, as on the ESP32-S3):
`lib/esp32/esp32/test/asm.fs` counts 100 000 alarms of a timer per
second with it.

## The radio

The radio of the ESP32 must be calibrated before it sends or receives.
The package ESP32-PHY (`lib/esp32/esp32/phy.fs`) does this with code of
C of Espressif, in `CODE` words. Load it right after a reset; this
takes 66 s:

    board ok> #include lib/esp32/esp32/phy.fs
    board ok> PHY-INIT .
    1
    board ok> 6 PHY-CHANNEL!

`PHY-INIT` calibrates in 60 ms. It gives 1 the first time (there were
no valid data of a calibration), then 0. `FREEZE` keeps ESP32-PHY;
after each start, `ALSO ESP32-PHY  PHY-INIT` calibrates again. The
calibration does not change the clock: `3000 MS` and `3000000 US` last
as long after it as before it (within 2 ms, measured with the clock of
the Mac).

The package ESP32-RADIO (`lib/esp32/esp32/radio.fs`) receives frames
with it, and WIFI (`lib/net/wifi.fs`) lists the networks around. Load
them after `phy.fs` (1 s):

    board ok> #include lib/net/bytes.fs
    board ok> #include lib/esp32/esp32/macinit.fs
    board ok> #include lib/esp32/esp32/radio.fs
    board ok> #include lib/net/wifi.fs
    board ok> ALSO RADIO  ALSO WIFI
    board ok> RADIO-START  WIFI-SCAN .
    5
    board ok> .WIFI

`WIFI-SCAN` listens 120 ms on each of the channels 1 to 13 (1.6 s).
`.WIFI` shows each network: its channel, its level in dBm, its name.
Load `lib/net/bytes.fs` and `lib/esp32/esp32/macinit.fs` before Radio.
`RADIO-SEND` can start one slot and wait for a completion (100 ms
at most). Broadcast is tested on the board (six frames in 602 ms,
60 timer ticks); ACK waiting also on the WROOM, with an own MAC set
by `RADIO-MAC!` after the start. Do not call it from a Forth handler. The reception keeps its
existing behavior (`lib/esp32/README.md`, "Transmission (N3c-2)").
`lib/esp32/README.md` ("The radio of the
ESP32: ESP32-PHY" and "The radio of the ESP32: ESP32-RADIO") has the
words, the rules, the memory, the tests, and where the code comes
from; `lib/net/README.md` ("The contract WIFI") has the words of
WIFI.

The initial ACK timeout `$5300` led to controlled tests. The receive
list had eight full descriptors; returning the beacon buffers permits
positive ACK, negative ACK and restoration. Radio never silently drops
application data to make space: callers keep servicing `RADIO-RECEIVE`.
The Forth MAC start agrees with the binary oracle on 69 registers and
passes RX 25/0 and ACK 4/0. A reset invalidates WIFI list indices.

**The WLAN:** the WROOM joins a WPA2 network as a station and keeps the
connection (WLAN-BACK), or opens a temporary network for one device
(SETUP-AP); our TCP/IP gives sockets over both. `apps/irbridge/app.fs`
loads all of it onto the image of the section "Build" and freezes it;
after a start the radio is off. The words: `lib/esp32/README.md`,
"The WLAN of the WROOM".

## The RMT: pulses for IR

The package RMT of the ESP32 (`lib/esp32/esp32/rmt.fs`) has the words
of the package RMT of the ESP32-S3, so IR, WS2812 and `apps/aircon`
work with it unchanged. `#require RMT lib/esp32/rmt.fs` loads it
(`#when`). The differences:

| | ESP32 | ESP32-S3 |
| --- | --- | --- |
| channels | 8; each one sends or receives | 0 to 3 send, 4 to 7 receive |
| RAM | 8 blocks of 64 pulse words | 8 blocks of 48 |
| clock (`RMT-HZ`) | the APB clock: 80 MHz with the PLL | 40 MHz |
| demodulator | none: `RMT-DEMOD` takes only 0 | yes |
| RX wrap | not supported | supported by hardware; I1 backend not implemented here |
| interrupt source (`RMT-SOURCE`) | 47 | 40 |

The receiver of the ESP32 keeps its state until `RMT-END` ticks pass
without a change of the level: `RMT-RX-ON` in the middle of a frame
gets the rest of that frame. A channel that receives and is not asked
(`RMT-RECEIVED`) gets an error with the next pulses after the end of a
frame; `RMT-RX-ON` ends it. The IR transceiver of Adafruit (product
5990) demodulates itself, so the package IR needs no demodulator
(`lib/esp32/ir.fs`).

For the IR bridge, IR-CAPTURE and IR-QUEUE take the IR frames by a
`CODE` handler and send in the background, with a check of the echo
(`apps/irbridge/app.fs` loads them; `lib/esp32/README.md`, "IR for the
bridge on the WROOM"). A frame has at most 256 pulse words and 400 times:
the RMT of the ESP32 has no RX wrap.

## An application in the flash

As on the ESP32-S3: load the application on the board, then `FREEZE`;
with `TURNKEY` the board starts it 1 s after the restart. Or build it
on the Mac: put the application files before `targets/freeze.fs`:

    LC_ALL=C awk -f bootstrap/awk/ucf.awk -- -32 lib/package.fs lib/idle.fs lib/esp32/rom.fs lib/esp32/esp32/chip.fs \
       lib/esp32/intr.fs lib/esp32/gpio.fs lib/esp32/timers.fs lib/esp32/flash.fs lib/esp32/esp32/clock.fs \
       lib/esp32/esp32/bootimg.fs targets/esp32/boot.fs examples/tick.fs targets/freeze.fs host/files.fs \
       targets/esp32/build.fs </dev/null

`apps/aircon` runs on this board with the same file `app.fs` as on the
ESP32-S3 (apps/aircon/README.md).

## Test

On the board, in `ucf term`:

    board ok> #include forth/test/tester.fs
    board ok> #include forth/test/t0.fs
    board ok> #include forth/test/hayes.fs
    board ok> #include forth/test/all.fs
    board ok> #include lib/esp32/esp32/test/intr.fs
    board ok> #include lib/esp32/esp32/test/gpio.fs
    board ok> #include lib/esp32/test/timers.fs
    board ok> #include lib/esp32/esp32/test/rmt.fs
    board ok> #include lib/esp32/esp32/test/ir.fs
    board ok> #include lib/net/test/board.fs
    board ok> #require FAIL lib/text.fs
    board ok> #require ADD, asm/xtensa.fs
    board ok> #include lib/esp32/asm.fs
    board ok> #include lib/esp32/esp32/test/asm.fs

Expected: `all: 1058 tests, 0 errors` (13 s at 240 MHz), then `intr: 18
tests, 0 errors` (errors of the CPU, handlers, an error in a handler, a
timer while `MS` waits), `gpio: 21 tests, 0 errors`, `timers: 7 tests, 0
errors`, `rmt:
25 tests, 0 errors` and `ir: 11 tests, 0 errors` (frames of the air
conditioner out and back in the chip); 27 s together. The tests need no
wires: GPIO 25 is an output, and the GPIO matrix gives its level back
to the chip. Two handlers stop with an error on purpose; their messages
show between the results. The last line loads our TCP/IP over NO-LINK
and runs the tests of the network with the steps in a timer
(`targets/esp32s3/README.md`, "Two levels"): `board: 128 tests, 0
errors` after about 18 s, most of them for the loading over `ucf term`
(measured with `MILLIS`). The last one tests the
`CODE` words: `asm: 19 tests, 0 errors` (also as the handlers of a
timer, 100 000 times a second; GPIO 25 toggles). The monitor has its
own test (`asm/monitor/README.md`, "Test").

The same tests give the same results when the image of the section
"Build" ran `FREEZE` first and started again: then the kernel reads
the base from the flash (section "Memory").

The test of ESP32-PHY (section "The radio") starts with a reset, on
the image of the section "Build": `phy.fs` loads its own tools.

    board ok> #reset
    board ok> #include lib/esp32/esp32/phy.fs
    board ok> #include forth/test/tester.fs
    board ok> #include lib/esp32/esp32/test/phy.fs

Expected: `phy: 13 tests, 0 errors` after 1.1 s (the load before it
takes 66 s): the calibration, the clocks (`US` and `MS` agree), the
channels 1 to 13, `PHY-TRACK`, off and on. The test of ESP32-RADIO
follows on the same board, after `#include lib/net/bytes.fs` and
`#include lib/esp32/esp32/macinit.fs`, then `radio.fs`:

    board ok> #include lib/esp32/esp32/test/radio.fs

Expected: `radio: 25 tests, 0 errors` after 4.5 s: on and off, the
channels, the list of the reception and its handler, the beacons of the
channels 1, 6 and 11 with the filter of a scan, the clocks, a timer
every 10 ms while the beacons come. It needs an access point near the
board, on one of these channels.

**In QEMU,** the emulator of Espressif with the real ROM of the chip
(doc/qemu.md, section "The ESP32 in QEMU"): the image as it is, at
`$1000` in a flash file of 4 MB:

    dd if=/dev/zero bs=1048576 count=4 | LC_ALL=C tr '\0' '\377' > build/flash-32.bin
    dd if=build/esp32.bin of=build/flash-32.bin bs=4096 seek=1 conv=notrunc
    docker run --rm -it -v "$PWD":/u ucf-qemu-esp qemu-system-xtensa -nographic -machine esp32 \
      -drive file=/u/build/flash-32.bin,if=mtd,format=raw

There, `ucf term` runs t0, hayes and `all.fs` over a pseudo terminal:
`all: 1058 tests, 0 errors` in 3 s (no serial line slows it down);
`FREEZE` writes the flash file, and the next start has the new words
and reads the frozen part through the cache, as on the board. (QEMU
writes its flash file a little later than the function of the ROM
returns: so BOOTIMG reads the flash once more before it checks the new
bytes through the cache.)
QEMU has no GPIO and no RMT, and the Forth handlers of interrupts do
not run there (doc/qemu.md): so the tests of `lib/esp32/esp32/test/`
and of TIMERS do not run in QEMU.

## The primitives of the HAL

The kernel `kernels/xtensa.fs` has the primitives of doc/vm.md, section
7 (0 to 28 and 32 to 65). The HAL `hal.fs` adds those that need the
chip, with these numbers. doc/vm.md, sections 7 and 8, has the rules
for the numbers; "Header" names the file that makes the word in the
dictionary.

| No. | Name | Stack | Effect | Header |
| --- | --- | --- | --- | --- |
| 29 | `?RX` | `( -- c true \| false )` | a byte from the console: the interrupt of UART0 puts each byte into the buffer RX (256 bytes); Ctrl-C (byte 3) stops the word that runs instead | Genesis (INTERNALS) |
| 30 | `TX!` | `( c -- )` | a byte to UART0; it waits while the FIFO is full | Genesis (INTERNALS) |
| 31 | `!IO` | `( -- )` | nothing: the ROM has set up UART0, START adds its interrupt | Genesis (INTERNALS) |
| 66 | `DOCODE` | a code field | a `CODE` word: its machine code is in SRAM0, at the address in the cell after the code field (section "CODE words"); the 66 of the kernel would run code in the dictionary | the package ASM |
| 80 | `MILLIS` | `( -- u )` | ms since the start: timer 0 of group 0 (1 MHz) / 1000 | Genesis |
| 81 | `MS` | `( u -- )` | wait u ms; Forth handlers and Ctrl-C come meanwhile | Genesis |
| 82 | `US` | `( u -- )` | wait u microseconds, the same | Genesis |
| 83 | `BIS!` | `( mask a -- )` | set the bits of mask in the cell at a | Genesis |
| 84 | `BIC!` | `( mask a -- )` | clear the bits of mask in the cell at a | Genesis |
| 85 | `SAVE-IMAGE` | `( -- ior )` | for `FREEZE`: runs `(SAVE-IMAGE)` of BOOTIMG; -21 without it | Genesis |
| 86 | `ERASE-IMAGE` | `( -- )` | does nothing | Genesis |
| 87 | `REBOOT` | `( f -- )` | empty stacks, then `BOOT` (f true) or `QUIT` | Genesis |
| 88 | `BYE` | `( -- )` | a restart of the chip (`RTC_CNTL_OPTIONS0`: SW_SYS_RST) | Genesis |
| 100 | `SYSCALL` | `( x1 .. xn n a -- x )` | the function of the ROM (or of C) at a, with n arguments (n at most 13), as the windowed call CALLX8; x is its result | ROM (`lib/esp32/rom.fs`) |
| 101 | `>CHIP` | `( a -- a' )` | the chip address of a VM address (section "Memory"); any other address stays | ROM |
| 102 | `+INT` | `( -- )` | the Forth handlers of interrupts on; -21 without the package INTR | INTR (`lib/esp32/intr.fs`) |
| 103 | `-INT` | `( -- )` | the Forth handlers off | INTR |
| 104 | `CODE-INT!` | `( xt n -- )` | the `CODE` word xt handles CPU interrupt n (19 to 21); xt 0: none | INTR (internal) |
| 105 | `FLASH-SYSCALL` | `( x1 .. xn n a -- x )` | as `SYSCALL`, for the functions of the ROM that read, write or erase the flash: the cache of the flash is off and the interrupts of level 1 and 2 wait meanwhile; then the cache is flushed and on again | ESP32 (`lib/esp32/esp32/chip.fs`) |
| 126 | - | `( lo hi -- )` | no name: the code field in the cell V-WAIT of the HAL; `MS` and `US` wait on with it after a break | none |
| 127 | `HALT` | `( -- )` | the code field in the cells of HALT (VM `$3FFF0`, doc/vm.md section 3): the end of a Forth handler, IP and W come back from the return stack | none |

Slots 120 to 122 of the table hold no primitive, but the end of the
frozen part, `$3F40_0000` and `S0` (section "Memory"). Each other
number without code runs UNKNOWN: error -21. The ESP32-S3 has the same
primitives, except 105: there `FLASH-SYSCALL` is a second name of 100.

**How a primitive comes into the VM.** In two halves:

1. The code: `n CODE name ... NEXT,` in `hal.fs`, with the assembler
   XTENSA. At the end of the build, `FILL-TABLE` puts the address of
   each code into the table of the primitives at its number. So a new
   primitive needs a new build (`build.fs`) and a new start of the
   board (`ucf flash`, or `ucf run`).
2. The name: a word whose code field holds the number, in a package
   that is part of the image. For example `lib/esp32/esp32/chip.fs`:

       PARSE-NAME FLASH-SYSCALL HEADER  105 ,  REVEAL

   The dictionary holds only the number, no code. So the word runs on
   each kernel that has the primitive, and gives -21 on one without it
   (for example the bootstrap VM that builds the image: it only
   compiles these words). The first line of `ucf.hex` names in `prims`
   the primitives of the VM that built the image (0-65, 80-88), not
   those of the board from 100 on.

Later, on the board, without a new build: a `CODE` word (section "CODE
words"). Its code field is 66, its machine code goes to SRAM0, and
`FREEZE` keeps it. It has no number of its own: the threads call it
by its xt, as any word.

## Memory

The whole memory of the board, measured on the WROOM. "Without Wi-Fi":
the image of the section "Build" after the start, and after its first
`FREEZE`. "With the IR bridge": all of `apps/irbridge/app.fs` (the radio,
WPA2, our TCP/IP, DNS, the setup network, IR-QUEUE) frozen in the
flash with `XDATA-ON`, after a start (the radio off; the radio on adds
the stack of the ROM in use). The sizes are in bytes; 1 KB is 1 024
bytes.

**In the chip:**

| Address | Size | Contents | Without Wi-Fi | With the IR bridge |
| --- | --- | --- | --- | --- |
| `$4000_0000`-`$4005_FFFF` | 384 KB | ROM 0: the code of the ROM (the bootloader, the functions for the flash, the cache, the UART, `ets_...`) | `SYSCALL` calls functions there | also the code of C of ESP32-PHY (7 functions) |
| `$3FF9_0000`-`$3FF9_FFFF` | 64 KB | ROM 1: the data of the ROM | read only | read only |
| `$4007_0000`-`$4007_7FFF` | 32 KB | SRAM0, POOL0: the cache of the flash for the CPU | in use (START switches it on) | in use |
| `$4007_8000`-`$4007_FFFF` | 32 KB | SRAM0, POOL1: the cache of the second core | free: the second core does not run (as RAM of the instruction bus: cells only) | free |
| `$4008_0000`-`$4008_27B7` | 10 168 | SRAM0: the code of the kernel and of the HAL: the table of the primitives, constants, the break table, at the next 1 KB the vectors | in use | in use |
| `$4008_27B8`-`$4009_FFFF` | 120 904 | SRAM0: the code of the `CODE` words (`CODE-START` up to `CODE-HERE`) | free | 47 948 in use (ESP32-PHY 47 484, IR-CAPTURE), 72 956 free |
| `$3FFA_E000`-`$3FFA_E6DF` | 1 760 | SRAM2: the data of the ROM | the ROM | the ROM |
| `$3FFA_E6E0`-`$3FFA_FFFF` | 6 432 | SRAM2 | free (not used) | free |
| `$3FFB_0000`-`$3FFB_09FF` | 2 560 | SRAM2, VM `$10000`-`$109FF`: TIB, PAD, the return stack, the data stack | in use | in use |
| `$3FFB_0A00`-`$3FFD_FFEF` | 194 032 | SRAM2, VM `S0` (`$10A00`) up to `$3FFEF`: the dictionary (the 8 cells of the guard first) | 36 272 in use, 157 760 free; after `FREEZE` the frozen part is in the flash, and 36 240 of its RAM are free for `XDATA-ON` | 190 064 in use, all of it frozen in the flash; 3 968 free; 190 032 of its RAM are RAM data (`XDATA-ON`): 58 516 in use, 131 516 free |
| `$3FFD_FFF0`-`$3FFD_FFFF` | 16 | SRAM2, VM `$3FFF0`: the two cells of HALT, 8 bytes free | in use | in use |
| `$3FFE_0000`-`$3FFE_043F` | 1 088 | SRAM1: the data of the ROM | the ROM | the ROM |
| `$3FFE_0440`-`$3FFE_3F1F` | 15 072 | SRAM1: the stack of the ROM during the start, free afterwards | free | `$3FFE_0500`-`$3FFE_3F1F`: the packet RAM of the LINKs (`RADIO-POOL`, 14 880 bytes): WIFI-LINK uses 11 392 of it, AP-LINK 8 168; only one runs |
| `$3FFE_3F20`-`$3FFE_434F` | 1 072 | SRAM1: the data of the ROM | the ROM | the ROM |
| `$3FFE_4350`-`$3FFE_43FF` | 176 | SRAM1 | free | free |
| `$3FFE_4400`-`$3FFE_7CAB` | 14 508 | SRAM1: ESP32-RADIO (`lib/esp32/esp32/radio.fs`): the reception (8 descriptors, 8 buffers of 1 600 bytes, up to `$3FFE_765F`), then a descriptor and a buffer for sending | free | in use |
| `$3FFE_7CAC`-`$3FFE_7FFF` | 852 | SRAM1 | free | free |
| `$3FFE_8000`-`$3FFE_80FF` | 256 | SRAM1, VM `$0`-`$FF` | not used by the VM | not used |
| `$3FFE_8100`-`$3FFE_83FF` | 768 | SRAM1, VM `$100`-`$3FF`: SYS, the system variables | in use | in use |
| `$3FFE_8400`-`$3FFF_7FFF` | 64 512 | SRAM1, VM `$400`-`$FFFF`: DATA, the RAM data (`VP`) | 1 848 in use, 62 664 free | 23 240 in use, 41 272 free |
| `$3FFF_8000`-`$3FFF_816B` | 364 | SRAM1: the variables of the HAL, with the buffer of UART0 | in use | in use |
| `$3FFF_816C`-`$3FFF_81FF` | 148 | SRAM1 | free | free |
| `$3FFF_8200`-`$3FFF_85FF` | 1 024 | SRAM1: the copies of the table of the primitives and of the break table: TBL points to them (START makes them; the CPU reads SRAM0 on the data bus 8 cycles slower) | in use | in use |
| `$3FFF_8600`-`$3FFF_FFBF` | 31 168 | SRAM1: the stack of the functions of the ROM (`SYSCALL`); it grows down from `$3FFF_FFC0` | at most 176 in use (`FREEZE`) | at most 1 200 in use (the calibration of the radio, a scan) |
| `$3FFF_FFC0`-`$3FFF_FFFF` | 64 | SRAM1: the first frame of this stack (START) | in use | in use |
| `$3FF8_0000`-`$3FF8_1FFF` | 8 KB | RTC FAST memory | not used | not used |
| `$5000_0000`-`$5000_1FFF` | 8 KB | RTC SLOW memory | not used | not used |
| `$3FF0_0000`-`$3FF7_FFFF` | 512 KB | the registers of the chip: `@` and `!` reach them | | |
| `$3F40_0000`-`$3F7F_FFFF` | 4 MB | the flash through the cache (DROM, read only): START maps `$3F41_0000`-`$3F43_FFFF` (VM address a at `$3F40_0000` + a) onto the flash `$7_0000`-`$9_FFFF`; the kernel reads the frozen part there, from `S0` up to `FLASH-END` | the frozen part after `FREEZE` | the frozen part after `FREEZE` |
| `$3F80_0000`-`$3FBF_FFFF` | 4 MB | external RAM (PSRAM) | none on the WROOM-32 | none |
| `$400C_2000`-`$40BF_FFFF` | | code in the flash through the cache (IROM) | not used | not used |

The SRAM of the chip is 520 KB: SRAM0 192 KB, SRAM2 200 KB, SRAM1
128 KB. Free without Wi-Fi: 157 760 bytes of dictionary and 62 664
bytes of RAM data for Forth, 120 904 bytes for the code of `CODE`
words. Besides, uCForth leaves about 99 KB unused without Wi-Fi and
about 73 KB with the IR bridge: POOL1 (32 KB), most of the stack of the ROM
(about 30 KB), the gaps in SRAM1 and SRAM2, and without Wi-Fi the
places of ESP32-RADIO and WIFI-LINK.

**In the flash** (4 MB):

| Flash address | Size | Contents | Without Wi-Fi | With the IR bridge |
| --- | --- | --- | --- | --- |
| `$0_0000`-`$0_0FFF` | 4 KB | not used: the ROM reads the image at `$1000` | | |
| `$0_1000`-`$6_FFFF` | 444 KB | the boot image (section "The image format of the ROM bootloader"): the code of the kernel and of the `CODE` words, the RAM data, the dictionary after `FLASH-END` (0 to 3 bytes), with `XDATA-ON` the RAM data behind the frozen part. `FREEZE` checks that it ends before `$7_0000` | the image of `build.fs`: 49 152 (all of its dictionary in it); after `FREEZE` 12 784 (code 10 168, RAM data 2 560) | after `FREEZE` 140 640 and the headers: code 58 116, RAM data 24 008, the RAM data behind the frozen part 58 516 |
| `$7_0000`-`$9_FFFF` | 192 KB | the frozen part of the dictionary: VM address a at `$6_0000` + a (`DICT-FLASH` of BOOTIMG and `build.fs`), from `$7_0A00` | 36 272 after `FREEZE` | 190 064 after `FREEZE` |
| `$A_0000`-`$3F_BFFF` | 3 440 KB | free for the package FLASH. It can still hold old bytes, for example of the factory firmware: erase before a write | free | free |
| `$3F_C000`-`$3F_DFFF` | 8 KB | the broker of the IR bridge (BRIDGE-SETTINGS, apps/irbridge/settings.fs), in two sectors | free | in use when saved |
| `$3F_E000`-`$3F_FFFF` | 8 KB | the profile of the network (WLSAVE), in the last two sectors | | in use when saved |

**The memory of the VM lies in three places.** The ESP32 has no free
block of 256 KB in its RAM: the ROM keeps its data at `$3FFE_0000`. So
the kernel puts the standard map of doc/vm.md (section 3) into two
parts of the RAM: VM addresses 0 to `$FFFF` (SYS and the RAM data) at
`$3FFE_8000` + address, in SRAM1; VM addresses `$10000` to `$3FFFF`
(TIB, PAD, the stacks and the dictionary) at `$3FFA_0000` + address,
in SRAM2 from `$3FFB_0000` on. The map itself is the standard, with
`DICT-END` at `$3FFF0`: the image of the bootstrap VM runs as it is.
`build.fs` has the two bases (`DATA-ADDR`, `RAM-ADDR`) and gives their
difference to the assembler (`DATA-OFF` of `asm/xtensa.fs`): the
memory words (`>CHIP,`) and the kernel use it. IP and W never point
into SRAM1.

The third place is the flash. After the first `FREEZE` on the board,
the frozen part of the dictionary, from `S0` up to `FLASH-END`, lies
there (section "FREEZE on the board"), and the MMU of the cache shows it
to the CPU at `$3F40_0000` + VM address (START sets up SPI0, the MMU
and the cache). Slot 120 of the table of the primitives holds
`FLASH-END`, slot 121 `$3F40_0000`, slot 122 `S0` (`FLASH-DICT?` of
`asm/xtensa.fs`). NEXT, `EXIT`, the jumps and the loops give each xt and
each target its chip address: in the flash below `FLASH-END`, else in
the RAM; `DOCOL` takes it back to a VM address by bit 23 of the chip
address. The memory words read from `S0` up to `FLASH-END` in the
flash; a store there (`!`, `C!`, `+!`, `MOVE`, `FILL`, `BIS!`, `BIC!`)
gives the error -9: the frozen part is read only, as doc/vm.md (section
13.4) allows. The image of `build.fs` has `FLASH-END` = `S0`: all of its
dictionary is in the RAM until the first `FREEZE`.

The RAM below `FLASH-END` is free then. `XDATA-ON` (package BOOTIMG)
gives it to the RAM data: `VARIABLE`, `VALUE`, `BUFFER:` and `DEFER`
take their memory there, from `S0` + 32 up to `FLASH-END` (`XVP` and
`XDATA-END` of `core.fs`), and no longer at `VP`; `FREEZE` saves them
with the boot image, and the end moves with `FLASH-END`. A `CODE` word
or code of C that keeps the chip address of a table in the dictionary
must take a table in the RAM data instead: after `FREEZE` the table has
another chip address (ESP32-PHY moves its tables of C there,
`lib/esp32/esp32/phy.fs`).

After the start, with the image of the section "Build":

| Area | Size | Free |
| --- | --- | --- |
| dictionary: words, `ALLOT`, `,` | 189.5 KB | 154 KB (157 764 bytes) |
| RAM data: `VARIABLE`, `VALUE`, `BUFFER:`, vocabularies | 63 KB | 61 KB (62 680 bytes) |
| the RAM below `FLASH-END` (`XDATA-ON`) | after `FREEZE` | 35 KB after the first `FREEZE` of this image (36 236 bytes); with the IR bridge 181 KB (185 164 bytes) |
| the code of the `CODE` words: SRAM0 from `CODE-START` (`$4008_27B8`) | 118 KB | 118 KB |
| data stack, return stack | 256 cells each | |
| the input line | 256 characters | |

`UNUSED U.` shows the free bytes of the dictionary,
`ALSO INTERNALS  DATA-END VP @ - U.` those of the RAM data. A full
dictionary or RAM data gives the error -8. doc/vm.md, section 3, has
the other limits of the VM.

Measured on the WROOM: PHY keeps 7 432 bytes of dictionary and 5 172
bytes of RAM data; BYTES 160 and 24; ESP32-MAC 2 668 and 24; RADIO
8 724 and 112; WIFI 2 340 and 1 616. After these loads, the measurement
session had 74 920 dictionary bytes and 58 240 RAM-data bytes free
(including the small measurement words), with the old map of 192 KB;
the standard map gives the dictionary 65 536 bytes more. The PHY code selection is
cleaned: 47 484 SRAM0 code bytes, 2 192 fewer than before. No libpp MAC
functions remain selected. The production start uses Forth; the old
comparison setup remains in git commit `9b67376`.

RX keeps 12 896 bytes of SRAM1; the TX descriptor and buffer reserve
1 612 more, up to `$3FFE_7CAB` (the table above). Servicing the receive
list remains necessary even when waiting for a TX ACK.

With the IR bridge (`apps/irbridge/app.fs`, 15.10.) 3 968 bytes of
dictionary and 41 272 bytes of RAM data are free, and 131 516 of the
190 032 bytes of RAM below `FLASH-END`. The loader keeps no headers for the `PRIVATE`
words (`TEMP-HEADS ON!`, lib/package.fs): the frozen part is about
17 KB smaller, and so is the RAM behind it.

The kernel keeps IP, W, SP and RP as chip addresses; the cells in
memory hold VM addresses. `@ ! C@ C!` and the other memory words take
an address below `$40000` as a VM address (below `$10000`: `$3FFE_8000`
+ address; from `S0` up to `FLASH-END`: `$3F40_0000` + address; else
`$3FFA_0000` + address) and every other address as it is: so a driver
in Forth writes the registers of the chip. `>CHIP` gives the chip
address of a VM address.

**Speed.** Measured on the WROOM at 240 MHz, in ms for 1 000 000 runs
(FIND-NAME: 10 000 runs of a name that is not there; AES: 100 blocks
of AES128), each word in the RAM, then frozen in the flash. The
memory map of the image before (the dictionary always in the RAM) is
for comparison:

| | before | the RAM | the flash |
| --- | --- | --- | --- |
| `DO LOOP` | 141 | 150 | 150 |
| `X @ DROP` (a variable) | 492 | 467 | 467 |
| `HERE C@ DROP` | 858 | 850 | 850 |
| a call of an empty colon word | 359 | 370 | 370 |
| `TT @ DROP` (a table made with `CREATE`) | 496 | 505 | 521 |
| FIND-NAME | 176 | 220 | 348 to 466 |
| AES128, 100 blocks | 97 | 90 | 91 |

The cache holds the words that run: a loop in the flash is as fast as
in the RAM. A search through the names (FIND-NAME) reads many headers
and runs about two times slower in the flash; when files load over
`ucf term`, the serial line is slower still (`hayes.fs` loads in 8.4 s
either way). The start up to the first line: 354 ms with the image of
`build.fs`, 326 ms after its first `FREEZE` (300 ms of it are the wait
at the start); with N3f 437 ms before, 384 ms now: the ROM no longer
loads the dictionary (136 KB).

## The image format of the ROM bootloader

The format of the ESP32-S3 (`targets/esp32s3/README.md`), with these
values. All numbers with the lowest byte first.

| Offset | Bytes | Contents |
| --- | --- | --- |
| 0 | 1 | `$E9` |
| 1 | 1 | the number of segments: here 3 (the code, the RAM data, the dictionary) |
| 2 | 1 | SPI mode of the flash: 2 = DIO |
| 3 | 1 | bits 7-4: flash size (2 = 4 MB); bits 3-0: SPI clock (0 = 40 MHz) |
| 4 | 4 | the start address |
| 8 | 1 | the pin for write protect, `$EE` = none |
| 12 | 2 | the chip: 0 = ESP32 |
| 17 | 2 | the highest revision of the chip: `$FFFF` = all |
| 23 | 1 | 1: a SHA-256 digest follows at the end (`build.fs`); 0: none (`FREEZE`). The ROM of the ESP32 does not check it |
| 24 | | the segments: address (4 bytes), length (4 bytes, a multiple of 4), the bytes |
| end | | zeros until the next byte ends a block of 16, then the checksum: XOR of all bytes of the segments, from `$EF`; then, with 1 in byte 23, the SHA-256 digest of all bytes before it (32 bytes) |

The ROM checks the checksum. The image goes to the flash at `$1000`.
`ucf run` refuses an image for the ESP32-S3 (chip 9) on this board, and
the other way round; `ucf flash` writes each file as it is.

## Limits

- The memory of the VM lies in three places (section "Memory"). `MOVE`
  and `FILL` take the chip address of their first byte: an area that
  runs over VM address `$10000` (from the RAM data into TIB), or over
  `S0` or `FLASH-END`, does not go on in the next place. The standard
  map has no such area: `BUFFER:` stays in the RAM data.
- Below `FLASH-END` the dictionary is read only: a store gives -9. Data
  that change belong in `VARIABLE`, `VALUE` or `BUFFER:` (doc/vm.md,
  section 10). A `CODE` word or code of C must not keep the chip
  address of a table of the dictionary over `FREEZE` (section
  "Memory").
- A `CODE` word has at most 2 KB of code. `MARKER` does not give the
  SRAM0 of the `CODE` words back (section "CODE words").
- No PSRAM: the module WROOM-32 has none. (A module ESP32-WROVER has
  PSRAM; uCForth does not use it yet.)
- `FREEZE` writes the frozen part first (only behind the old one), then
  the boot image: if the power fails while it writes the boot image,
  the board does not start: write an image with `ucf flash`. The first
  `FREEZE` of an image without a frozen part erases the 192 KB of the
  dictionary in the flash; if the boot image in the flash had a frozen
  part there, `FREEZE` erases that boot image first.
- During a write to the flash (`FREEZE`, the package FLASH, `WLAN-SAVE`)
  the cache is off and the Forth handlers wait (`FLASH-SYSCALL`):
  frames of the radio can get lost then.
- The function of the ROM that writes the flash takes at most 255 pages
  of 256 bytes (64 KB) in one call; with more it does not stop at the
  end of the data and writes over the rest of the flash. `FLASH-WRITE`
  gives it 32 KB at a time; a call of it with `SYSCALL` must do the same.
- Only the first core runs. Bluetooth is not used. Of Wi-Fi, only the
  calibration of the radio and the reception of beacons run (section
  "The radio"); the chip does not send frames yet.
- All Forth handlers share CPU interrupt 1. Ctrl-C stops a function of
  the ROM only after its end.
- The RMT has no demodulator, and its clock follows the CPU clock
  (section "The RMT").
