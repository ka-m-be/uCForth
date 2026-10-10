# esp32s3: uCForth for the ESP32-S3

The result is `build/esp32s3.bin`: uCForth for an ESP32-S3 board (tested
board: diymore ESP32-S3 N16R8, DevKitC-1 compatible), in the format of
the ROM bootloader of the chip. The tool `ucf` (`tools/ucf.fs`) runs it
in the RAM, writes it to the flash, and is the console with `#include`.

**Status:** tested on the board (diymore ESP32-S3 N16R8): `ucf info`,
`run` and `flash`; t0 and hayes over `#include`: 1055 tests, 0 errors;
the board starts uCForth from the flash, with the CPU at 240 MHz and
the 8 MB PSRAM on; `FREEZE` on the board writes the running system to
the flash; Ctrl-C stops the word that runs, an error of the CPU gives
`THROW`, and Forth words run as handlers of interrupts, with the
network and IR in the background (section "Two levels"); `CODE` words
with the assembler on the board, also as handlers of interrupts; GPIO
with interrupts of the pins and the GPIO matrix; words that run every
n ms (`EVERY`, `AFTER`); the RMT sends and receives pulses (IR remote
controls, the RGB LED).

## The board

Tested board: the board "ESP32-S3 N16R8" of diymore, compatible with
the DevKitC-1 of Espressif. Another board with an ESP32-S3 and a USB
serial bridge at UART0 works in the same way; this table shows what to
look at on it.

| Part | On this board | What it means for uCForth |
| --- | --- | --- |
| Chip | ESP32-S3R8 (QFN56), revision v0.2, with 8 MB PSRAM in the package (octal) | the PSRAM is at `$3C00_0000` (section "The PSRAM"); GPIO 33 to 37 belong to it |
| Flash | 16 MB, in the module | the image at `$0`, the RAM data of the PSRAM from `FREEZE` at `$8_0000`; GPIO 26 to 32 belong to the flash |
| Socket "USB to serial port" | a bridge CH343P to UART0 (GPIO 43 and 44) | the console of uCForth; on a Mac `/dev/cu.usbmodem...` (the name has the serial number of the bridge); DTR and RTS reach EN and GPIO 0, so `ucf` restarts the board and starts the download mode itself |
| Socket "ESP32-S3 Direct Type-C USB & OTG" | the USB of the chip (USB-Serial/JTAG) at GPIO 19 (D-) and 20 (D+); the picture of the pins from the seller says 20 and 21 | not used by uCForth yet (section "Limits"); with the factory firmware it was `/dev/cu.usbmodem1234561` |
| Buttons | RST (EN) and BOOT (GPIO 0) | RST restarts the chip; BOOT held down during RST starts the download mode of the ROM; `BUTTON` in the package of the board |
| LED | an RGB LED WS2812 at GPIO 48 (the bridge "RGB" on the board is closed) | the package WS2812; `LED`, `LED-ON`, `LED-OFF` in the package of the board |
| Strapping pins | GPIO 0, 3, 45 and 46: the chip reads their levels at a reset | nothing may pull them to another level during a reset |

So these pins are not free for an application: 0 (BOOT), 19 and 20
(USB), 26 to 37 (flash and PSRAM), 43 and 44 (the console), 48 (the
LED). The IR transceiver of `apps/aircon` uses GPIO 4 and 14.

The package of the board is DIYMORE-N16R8
(`lib/esp32/esp32s3/diymore/n16r8.fs`): `LED`, `BUTTON`, `LED-ON`,
`LED-OFF`; after it is loaded, `ALSO BOARD` gives them
(lib/esp32/README.md).

## Build

In the folder `ucf`, with a bootstrap VM with 32-bit cells:

    LC_ALL=C awk -f bootstrap/awk/ucf.awk -- -32 lib/package.fs lib/idle.fs lib/esp32/rom.fs lib/esp32/esp32s3/chip.fs \
       lib/esp32/intr.fs lib/esp32/gpio.fs lib/esp32/timers.fs lib/esp32/flash.fs lib/esp32/esp32s3/clock.fs \
       lib/esp32/esp32s3/psram.fs lib/esp32/esp32s3/bootimg.fs targets/esp32s3/boot.fs targets/freeze.fs host/files.fs \
       targets/esp32s3/build.fs </dev/null

The output ends with:

      saved: ucf.hex
      code: 8106 bytes at $40378000, start at $4037917C
      image: $100-$00001D10 and $00010A00-$00019948, at $3FC90000 + VM address
      start word: ON-BOOT
      FREEZE on the board: yes
      interrupts: yes
      Ctrl-C waits for NO-BREAK: yes
      written: build/esp32s3.bin (60176 bytes)

1. `lib/package.fs` comes first: the packages for the board
   (`PACKAGE` ... `END-PACKAGE`) need it on the board too. Then
   `lib/idle.fs`, the package IDLE: the chain of words that run while
   the console waits, and `NO-BREAK`, for which Ctrl-C waits (section
   "Two levels").
2. The packages for the board in `lib/esp32/` (its README): ROM (the
   calls of the ROM), ESP32S3 (the layer of the chip: its addresses and
   numbers, `lib/esp32/esp32s3/chip.fs`), INTR (interrupts, errors of
   the CPU), GPIO (the base of the drivers), TIMERS (`EVERY`, `AFTER`),
   FLASH, and from `lib/esp32/esp32s3/` CLOCK, PSRAM, BOOTIMG (`FREEZE`
   on the board), in this order. Then
   `targets/esp32s3/boot.fs`: the image starts with `ON-BOOT`, which
   sets the CPU to 240 MHz, switches the PSRAM on, puts the RAM data of
   the last `FREEZE` back into it, then runs `BOOT` (sections "FREEZE
   on the board", "The clock", "The PSRAM" and "Interrupts and errors
   of the CPU"). Without these eleven files the image is 25 KB smaller,
   the CPU stays at 40 MHz without PSRAM, `FREEZE` on the board gives
   the error -21, there are no handlers of interrupts, and an error of
   the CPU gives -9 without its address; without `lib/idle.fs`
   Ctrl-C does not wait for a word of the network.
3. `targets/freeze.fs`: the running system is the image, without the
   package FILES (the board has no files). Put your application files
   before it.
4. `build.fs` makes the code with the assembler `asm/xtensa.fs`: the
   kernel `kernels/xtensa.fs` and `hal.fs` (start, UART0 with its
   interrupt, time, restart, the calls of the ROM, the vectors for
   exceptions and interrupts), and writes `build/esp32s3.bin` (section
   "The image format of the ROM bootloader").

The tool `ucf` for the Mac (see `tools/ucf.fs`):

    LC_ALL=C awk -f bootstrap/awk/ucf.awk -- -32 host/files.fs tools/ucf.fs targets/macos-a64/build.fs </dev/null
    chmod +x build/ucf

## Run

Connect the socket "USB to serial port" of the board (UART0 over a USB
bridge; on a Mac `/dev/cu.usbserial-...` or `/dev/cu.usbmodem...`).

In the RAM (the flash stays as it is; a reset starts the old program).
`ucf run` also sets up the flash through the ROM (`SPI_ATTACH`, as
esptool does), so the flash words and `FREEZE` work as after a start
from the flash:

    build/ucf run /dev/cu.usbmodem5C4C0505931 build/esp32s3.bin

In the flash, at address 0 (instead of the bootloader of the factory
firmware), then a restart:

    build/ucf flash /dev/cu.usbmodem5C4C0505931 build/esp32s3.bin 0
    build/ucf reset /dev/cu.usbmodem5C4C0505931
    build/ucf term /dev/cu.usbmodem5C4C0505931

The download mode of the ROM stays: it is in the mask ROM of the chip.
`ucf` enters it over DTR and RTS; else hold BOOT, press RESET, release
BOOT. So another program can always go to the flash again.

## Develop on the board

`ucf term` is the console (package TERM, host/README.md). It sends a
typed line when the return key comes, and shows what the board sends.
The prompt of the board shows as `board ok>` (`board c>` in compile
state); `ok>` alone is the prompt of the host. Its commands:

| Command | Effect |
| --- | --- |
| `#include FILE` | send the file, a line after each prompt; stop at an error and name the file and the line |
| `#require WORD FILE` | the same, if the board does not know WORD |
| `#when WORD FILE` | the same, if the board knows WORD: `lib/esp32/rmt.fs` takes the file of the chip with it (`#when ESP32S3 esp32s3/rmt.fs`) |
| `#reset` | restart the board over RTS; a line that still runs (`FREEZE`) ends first, and the next line waits for the prompt after the start (each at most 10 s) |
| `#help` | the commands |
| `#bye` | end `ucf term` |
| Ctrl-C | stop the word that runs on the board (and a file that loads) |

A file name after `#include` in a file is relative to that file. On
the board: `SEE`, `.S`, `DUMP`, `WORDS`, `ORDER`, `VOCS`, `MARKER`, and
the messages of `THROW`.

**The search order.** After each start, the search order has only FORTH
(`ONLY FORTH`), as on each target. The packages of the image are on the
board, but not in the search order: `ALSO GPIO` gives the words for
the pins, `ALSO TIMERS` `EVERY` and `AFTER`, `ALSO ESP32S3` the
addresses and numbers of the chip, and so on. `VOCS` shows all
vocabularies, the newest first:

    board ok> ORDER
    FORTH   current: FORTH
    board ok> VOCS
    BOOTIMG.I BOOTIMG PSRAM.I PSRAM CLOCK.I CLOCK FLASH.I FLASH TIMERS.I
    TIMERS GPIO.I GPIO INTR.I INTR ESP32S3.I ESP32S3 ROM.I ROM IDLE.I IDLE
    PACKAGES.I INTERNALS FORTH

Example: the RGB LED of the board blinks (`examples/blink.fs`):

    board ok> #include examples/blink.fs
    board ok> BLINK

The return key stops it (`term` sends keys only as a line).
`examples/blink.fs` loads the package of the board with `#require
BOARD lib/esp32/board.fs`: here `lib/esp32/esp32s3/diymore/n16r8.fs`,
and it loads `lib/esp32/rmt.fs` and `lib/esp32/ws2812.fs`.

## FREEZE on the board

`FREEZE` writes the running system to the flash: the kernel, the RAM
data and the dictionary, as a new image at flash address 0. After a
restart, the board starts this image. So the words that you add on the
board stay:

    board ok> : HELLO ." hello from the flash" CR ;
    board ok> FREEZE
    board ok> #reset
    ESP-ROM:esp32s3-20210327
    ...
    entry 0x403790a4
    board ok> HELLO
    hello from the flash

After the restart, the search order has only FORTH again, also when
`FREEZE` saved another one. `FREEZE` takes about 0.6 s. It uses the
functions of the ROM for the flash and for the SHA-256 digest (packages
ROM, FLASH and BOOTIMG, `lib/esp32/`). The RAM data in the PSRAM go to
the flash too (section "The PSRAM"). `ucf flash PORT build/esp32s3.bin
0` writes the image from the Mac again.

## The clock

The ROM starts the CPU at 40 MHz from the crystal. `ON-BOOT`
(`targets/esp32s3/boot.fs`) switches it to 240 MHz with the package
CLOCK (`lib/esp32/esp32s3/clock.fs`; `ALSO CLOCK` gives its words):

| Word | Effect |
| --- | --- |
| `n MHZ` | the CPU clock: 40 (crystal), 80, 160 or 240 (PLL) |
| `CPU-MHZ ( -- n )` | the CPU clock now |

UART0 and the SYSTIMER run from the crystal: the console and `MS` do
not change. The SPI flash stays at 40 MHz. The APB clock (for timers,
SPI, PWM) is 80 MHz with the PLL, else the CPU clock. Measured on the
board:

| Clock | 1 000 000 times `DO LOOP` | `25 FIB` (recursive) |
| --- | --- | --- |
| 40 MHz | 525 ms | 864 ms |
| 80 MHz | 263 ms | |
| 160 MHz | 131 ms | 216 ms |
| 240 MHz | 88 ms | 144 ms |

## The PSRAM

The chip has 8 MB PSRAM in its package. `ON-BOOT` switches it on with
the package PSRAM (`lib/esp32/esp32s3/psram.fs`). Then the PSRAM is at the chip
addresses `$3C00_0000` to `$3C7F_FFFF`, and `@ ! C@ C! MOVE` and the
other memory words reach it:

    board ok> ALSO PSRAM  PSRAM-ADDR . PSRAM-SIZE .
    1006632960 8388608
    board ok> $12345678 PSRAM-ADDR !  PSRAM-ADDR @ HEX . DECIMAL
    12345678

| Word | Effect |
| --- | --- |
| `PSRAM-ON` | set up the PSRAM; `ON-BOOT` does it |
| `PSRAM-ADDR ( -- a )` | its first address, `$3C00_0000` |
| `PSRAM-SIZE ( -- u )` | its size in bytes; 0 without PSRAM |
| `.PSRAM` | the mode registers of the PSRAM chip |
| `size ways line DCACHE-MODE` | another mode of the data cache (see below) |

**The RAM data are in the PSRAM.** `PSRAM-ON` gives the RAM data an
extension (doc/vm.md, section 3): from then on `VARIABLE`, `BUFFER:`,
`VALUE`, `DEFER` and new vocabularies get their memory in the PSRAM.
The dictionary stays inside the chip, and so do the data of the image
from the start. A buffer that must be inside the chip (for DMA, or
for the words of FLASH) comes from `SRAM-BUFFER:` (package GPIO):

    board ok> VARIABLE COUNTER  7 COUNTER !  : BUMP  1 COUNTER +! ;  BUMP BUMP
    board ok> COUNTER HEX . DECIMAL
    3C000000
    board ok> ALSO GPIO  64 SRAM-BUFFER: SB  SB HEX . DECIMAL
    1B80

`FREEZE` saves the RAM data in the PSRAM to the flash at `$8_0000`
(with a SHA-256 digest), and `ON-BOOT` loads them back after a restart:

    board ok> FREEZE
    board ok> #reset
    ...
    board ok> COUNTER @ .  BUMP COUNTER @ .
    9 10

`MARKER` frees the RAM data in the PSRAM as in the RAM data region.

**Speed.** The PSRAM runs at 40 MHz. The data cache has 64 KB with
lines of 32 bytes (`PSRAM-ON` sets it; the ROM leaves 32 KB with lines
of 16 bytes). Measured with the CPU at 240 MHz:

| Test | Time |
| --- | --- |
| 1 000 000 times `@` of one variable: internal RAM, PSRAM | 287 ms, 288 ms |
| one read that is not in the cache | 0.76 us |
| write 8 MB, a cell at a time | 1.0 s |
| read 8 MB and compare, a cell at a time | 1.8 s |
| `MOVE` 64 KB: internal to internal, internal to PSRAM, PSRAM to internal | 2 ms, 5 ms, 5 ms |

The modes of the data cache, measured (ms):

| Cache | read 1 MB | read 4 MB, every 64th byte | 100 000 reads at random places | write 1 MB | 48 KB 20 times |
| --- | --- | --- | --- | --- | --- |
| 32 KB, lines of 16 bytes | 136 | 64 | 147 | 130 | 59 |
| 32 KB, lines of 32 bytes | 118 | 63 | 157 | 116 | 41 |
| **64 KB, lines of 32 bytes** | 117 | 64 | 156 | 116 | 24 |
| 64 KB, lines of 64 bytes | 108 | 87 | 177 | 109 | - |

## Interrupts and errors of the CPU

**Errors of the CPU.** An address without memory, an unknown
instruction, or a division by 0 in a `CODE` word is an exception of the
CPU. The board does not stop: the kernel gives `THROW` (package INTR:
-9 with the address and the PC, or -10), and the prompt comes back. This
works also when the error comes in a function of the ROM (`SYSCALL`):

    board ok> $10000000 @
      error: invalid memory address: $10000000 at pc $40378382
    board ok>

Before the `THROW`, the HAL writes all registers of the exception to
`FAULT-REGS` of the package INTR (`lib/esp32/intr.fs`): the PC, PS,
SAR, the cause, EXCVADDR, a0 to a15 and the registers of loops and of
the register window. The monitor shows them (`LAST-FAULT .REGS`,
`asm/monitor/README.md`). A debug exception (a breakpoint, a step, a
`BREAK` instruction) goes to the monitor instead while it debugs code
(`DEBUG-HOOK` of INTR, the cell after `FAULT-REGS`).

**Ctrl-C** in `ucf term` stops the word that runs on the board:

    board ok> : X  BEGIN AGAIN ;  X
    ^C
      error: user interrupt
    board ok>

The interrupt of UART0 takes each byte that comes; the byte 3 is the
break (doc/vm.md, section 9). `MS` stops after the current
millisecond. A function of the ROM ends first: `FLASH-ERASE` stops
after the current block of 64 KB. The handlers of interrupts go on:
the timers, the receiver of IR, the network. Ctrl-C does not stop a
word in its middle when the middle matters:

| Ctrl-C | Effect |
| --- | --- |
| once | it waits until the Forth handler that runs ends, and while the variable `NO-BREAK` of the package IDLE is not 0 (a word of the network, a word of the chain of IDLE); then it stops the word that runs. Mostly this takes less than 1 ms |
| twice within 1 s | the same, and it switches the handlers off at once (as `-INT`): "handlers off". `+INT` switches them on again |
| while another one waits | it does not wait: it switches the handlers off and stops the word at once, also a handler or a word with `NO-BREAK` that hangs. Then the network may be in the middle of a segment: restart the board |

    board ok> X
    ^C
      error: user interrupt
    board ok> ^C
      handlers off
    board ok> +INT

**Forth words as handlers of interrupts.** The chip has 99 sources of
interrupts (Technical Reference Manual, table 9.3-1), for example GPIO
16, UART0 27, timer group 0 50 to 52, SYSTIMER 57 to 59, FROM_CPU 79 to
82 (a register of the CPU sets them: good for tests without wires). The
package INTR (`lib/esp32/intr.fs`):

| Word | Effect |
| --- | --- |
| `xt i INT!` | the Forth word xt ( -- ) is the handler of source i (0 to 98) |
| `i INT@ ( -- xt )` | the handler of source i, or 0 |
| `i INT-ON`, `i INT-OFF` | source i goes to the handlers, or not |
| `+INT`, `-INT` | all handlers on, off |

A handler runs at the next NEXT, between two primitives of the word
that runs, on its stacks. It must leave the stacks as they were, and
it must clear the interrupt in the registers of its source, else it
runs again at once. It runs in the middle of another word, so it must
not do all that Forth can do: section "Words that run later" has the
rules; they hold for each Forth handler. Example with
FROM_CPU 0 (source 79, register `SYSTEM_CPU_INTR_FROM_CPU_0` at
`$600C_0030`):

    board ok> ALSO INTR  VARIABLE HITS
    board ok> : H  0 $600C0030 !  1 HITS +! ;
    board ok> ' H 79 INT!  79 INT-ON  +INT
    board ok> 1 $600C0030 !
    board ok> HITS @ .
    1

An error in a handler (`THROW`, `ABORT"`, an error of the CPU) stops
only this handler: its source goes off, the other handlers and the
word that ran go on. Its message shows when the console waits for a
key (in the chain of the package IDLE), and it does not start with
`error:`, so a file that `ucf term` loads goes on:

    board ok> : H  0 $600C0030 !  1 0 / DROP ;
    board ok> ' H 79 INT!  79 INT-ON  +INT  1 $600C0030 !
    board ok>
      H stopped: division by zero

`79 INT-ON` switches the source on again. Only -28 (Ctrl-C at once)
goes on in the word that ran.

Measured, with the CPU at 240 MHz: a handler costs 6 to 8 us (the
break, the search for the source, `CATCH`, a handler with three
accesses to registers). A timer interrupt 10 000 times per second makes
a loop 7 % slower, 50 000 times per second 56 %. At about 150 000 times
per second the handlers take all the time; Ctrl-C then switches them
off. A handler comes 9 us after its event, also while `MS` waits: `MS`
and `US` wait until a time of the SYSTIMER and go on after the
handler, so they last as long as asked.

**`CODE` words as handlers** run at once, in the interrupt (level 2),
for up to 3 sources: see the next section.

| Level | CPU interrupt | For |
| --- | --- | --- |
| 1 | 1 | the Forth handlers, at the next NEXT |
| 2 | 19, 20, 21 | a `CODE` word each, at once |
| 3 | 23 | UART0: the console and Ctrl-C |

A handler of level 1 to 3 runs to its end before another interrupt of
these levels comes; then the highest level that waits comes first. So
Ctrl-C comes through also when a handler of level 2 does not clear its
source and comes again and again.

## GPIO

The package GPIO (`lib/esp32/gpio.fs`, in the image) has the words for
the pins; `ALSO GPIO` gives them. The same words work on the ESP32; the
addresses come from the layer of the chip (package ESP32S3). Pin
numbers are GPIO numbers, 0 to 48.

| Word | Effect |
| --- | --- |
| `pin OUTPUT`, `pin INPUT` | the pin drives, or only reads (no pull) |
| `pin PULLUP`, `pin PULLDOWN` | an input with a pull resistor in the chip |
| `pin OPEN-DRAIN` | an output: `LOW` pulls to 0 V, `HIGH` lets go |
| `pin HIGH`, `pin LOW`, `pin TOGGLE`, `f pin PIN!` | the level of an output |
| `pin PIN@ ( -- f )` | the level at the pin, also of an output |
| `n pin DRIVE` | the strength of the output, 0 to 3 (about 5, 10, 20, 40 mA); `OUTPUT` gives 2 |
| `signal pin SIGNAL>PIN` | the output signal of a peripheral drives the pin (GPIO matrix; the numbers: TRM, table 6.11-1) |
| `pin signal PIN>SIGNAL` | the pin goes to the input signal of a peripheral |
| `xt pin mode PIN-INT!` | xt ( -- ) runs when mode happens at the pin: `RISING FALLING ANY-EDGE LOW-LEVEL HIGH-LEVEL`; mode 0: not any more |

The word of `PIN-INT!` is a Forth handler (package INTR, source 16):
`+INT` switches it on. At a level (`LOW-LEVEL`, `HIGH-LEVEL`) it must
end the level, else it runs again at once. The button BOOT of the board
is GPIO 0; it pulls the pin to 0 V (a press can give more than one
edge):

    board ok> ALSO GPIO  ALSO INTR
    board ok> : PRESSED ( -- )  ." BOOT" CR ;
    board ok> ' PRESSED 0 FALLING PIN-INT!  +INT

A press now shows `BOOT`. The package WS2812 drives the LED through the
GPIO matrix with `SIGNAL>PIN`. `lib/esp32/esp32s3/test/gpio.fs` tests all
without wires: GPIO 2 as an output, its level back into the chip, into
the pulse counter (`PIN>SIGNAL`), and the RMT of WS2812 into GPIO 2.

## Words that run later

The package TIMERS (`lib/esp32/timers.fs`, in the image; `ALSO TIMERS`
gives its words):

| Word | Effect |
| --- | --- |
| `xt ms EVERY` | xt ( -- ) runs every ms milliseconds; `0 EVERY`: not any more |
| `xt ms AFTER` | xt runs once, after ms milliseconds |
| `.TIMERS` | the words that wait, their period, and when they come |

    board ok> ALSO TIMERS  VARIABLE COUNTS  : COUNT-IT ( -- )  1 COUNTS +! ;
    board ok> ' COUNT-IT 100 EVERY  1000 MS  COUNTS @ .
    10
    board ok> .TIMERS
    COUNT-IT every 100 ms, in 79 ms
    board ok> ' COUNT-IT 0 EVERY

The words run as Forth handlers (timer 0 of timer group 1, source 53,
each millisecond while a word waits). `EVERY` and `AFTER` switch the
handlers on (`+INT`); Ctrl-C twice switches them off, and the timers
with them, until `+INT`. At most 8 words wait at the same time. After a
restart no word waits: a program sets its timers again (`'SETUP`).

**What a word of a timer must not do.** The word runs between two
primitives of another word, on its stacks, and the other Forth
handlers wait until it ends. So these rules hold for it, and for each
Forth handler (`INT!`, `PIN-INT!`, `IR-LISTEN`):

| It must not | Because |
| --- | --- |
| wait (`MS`, `US`, `KEY`, a loop on a pin), or run long | the other word and all other handlers wait meanwhile; a timer that comes meanwhile runs once, later. Measured: next to a word with `50 MS` every 100 ms, a word every 1 ms ran 558 times in 1 s instead of 1000 |
| show numbers or text (`.`, `U.`, `<# #>`, `TYPE`, `EMIT`) | the output comes into the output of the other word, and `.` uses the area at `PAD`. Measured: with a timer word that formats a number each ms, 10 of 10 000 numbers of the other word came out wrong. A word of the chain of IDLE shows the result (section "Two levels"); only a test that waits at the prompt may show (as `LISTEN` of `apps/aircon`) |
| change the dictionary (`:`, `CREATE`, `VARIABLE`, `ALLOT`, `,`) | the other word may be in the middle of a definition |
| leave `BASE`, `'EMIT`, `STATE` or the search order changed | the other word uses them |
| use a word with its own variables that the other word may run at the same time (`IR-SEND`, `RMT-SEND`) | the variables change under the other word |
| change data of the other word in more than one step (two cells, a buffer and its count) | the other word can see half of the change: it puts `-INT` ... `+INT` around its own access. One primitive (`!`, `+!`, `MOVE`) is never cut |

It may: compute, read and write its own variables, buffers and the
registers of the chip, `CATCH` its own errors, and set a flag; a word
of the chain of IDLE then shows or sends the result. A timer every 1 ms
costs about 33 us each time, 3 % of the time; a word of 0.74 ms every
1 ms leaves the program a quarter of the time.

An error in the word (`THROW`, `ABORT"`, an error of the CPU) stops only
this word, as `0 EVERY`; the other timers and the word that ran go on.
Its message comes when the console waits:

      M-TICK stopped: division by zero

## Two levels: the network and IR in the background

An application such as the IR bridge does several things at the same
time: the network (TCP, MQTT, a web page), IR in and out, and the
console. The board runs them on two levels, without a multitasker:

| Level | What runs there | When | What it may do |
| --- | --- | --- | --- |
| 1: Forth handlers | our TCP/IP (`NET-START`: a step every 1 ms), the receiver of IR (`IR-LISTEN`), timers of the application | between two primitives of any word, also while the console computes or loads a file | only what the rules in "Words that run later" allow: short, no output |
| 2: the chain of IDLE | the logic of the application, MQTT, a web page, IR out | while the console waits for a key: at the prompt and between the lines of a file | all that a word at the console may do, but not wait long (as `loop()` of Arduino) |

The package IDLE (`lib/idle.fs`, in the image; `ALSO IDLE`):

| Word | Effect |
| --- | --- |
| `xt IDLE+` | xt ( -- ) runs from now on while the console waits (8 words at most) |
| `xt IDLE-` | not any more |
| `IDLE-RUN` | each word of the chain once (`KEY` does it; a program with a loop of its own calls it) |
| `.IDLE` | the words of the chain |
| `-BREAK`, `+BREAK` | Ctrl-C waits between them; they count |

    board ok> ALSO IDLE  VARIABLE N  : COUNT-N ( -- )  1 N +! ;
    board ok> ' COUNT-N IDLE+
    board ok> N @ .
    62368
    board ok> ' COUNT-N IDLE-

(A word of the chain ran about 60 000 times in the second at the
prompt.) A word of the chain runs with `CATCH`; `BASE` and `'EMIT` are
the same after it. An error takes it out of the chain, with the message
"COUNT-N stopped: ...". Ctrl-C waits until the word ends: it never
stops MQTT in the middle of a packet. While the console computes, level
2 rests, but nothing is lost: the bytes wait in the buffers of TCP (a
full buffer stops the other side, through the window of TCP), and what
a handler of level 1 took (a frame of IR) waits in its buffer.

Our TCP/IP in the background (lib/net/README.md, "Our TCP/IP"): load
it, then `#include lib/esp32/nettimer.fs` and `ALSO NET-TIMER
NET-START`. Each word of NETIF holds a lock while it runs: a step that
comes meanwhile runs at its end, and Ctrl-C waits too. The words of
NET wait as before (`NET-POLL`), so the same program runs on the Mac.

The test `lib/esp32/esp32s3/test/h1.fs` (section "Test") runs both
levels for about a minute, with Ctrl-C, a long computation and a timer
word with an error at the console: frames of the air conditioner out of
GPIO 2 and back in, a connection over 127.0.0.1 with an echo. Measured:
101 of 101 frames back, each time as sent; 863 552 bytes back, none
wrong, the connection open; a timer word every 10 ms ran 6 468 times,
also while the console computed for 10 s.

## CODE words: the assembler on the board

The assembler `asm/xtensa.fs` also runs on the board. The package ASM
(`lib/esp32/asm.fs`) puts its machine code into the dictionary, where
the CPU runs it. Load them in `ucf term` (14 KB of dictionary, 3 s):

    board ok> #require FAIL lib/text.fs
    board ok> #require ADD, asm/xtensa.fs
    board ok> #include lib/esp32/asm.fs

`CODE name` ... `END-CODE` makes a word of machine code. Between them:
the instructions (the operands first, then the instruction with a
comma, as in `asm/xtensa.fs`), the registers of the VM `MEM TBL IP W
SP RP`, the macros `NEXT, PUSH, POP, TOS@, TOS!, >CHIP,`, `CONST,` for
a constant of 32 bits, and the labels `L1` to `L9`. A `CODE` word may
use `A2` to `A9`; it ends with `NEXT,`:

    board ok> CODE 3+ ( n -- n+3 )  A2 TOS@,  A2 A2 3 ADDI,  A2 TOS!,  NEXT,  END-CODE
    board ok> 4 3+ .
    7

A `CODE` word as the handler of an interrupt (`INT!`) runs at once, in
the interrupt: it may use `A0`, `A2` to `A9` and `SAR`, and it ends
with `RET,`. It must clear the interrupt of its source, and it must
not use the PSRAM (a word of FLASH may run): its data belong inside the
chip (`SRAM-BUFFER:`). Example: timer 0 of timer group 0 (source 50)
toggles GPIO 2 and counts, 10 000 times per second
(`lib/esp32/esp32s3/test/asm.fs`). The words come from the packages
GPIO (`SRAM-BUFFER:`), INTR (`INT!`) and ROM (`>CHIP`, the chip address
of a VM address):

    ALSO GPIO  ALSO INTR  ALSO ROM
    4 SRAM-BUFFER: TICKS
    $6001F000 CONSTANT TG0
    CODE TICK-ASM ( -- )   \ clear the interrupt, the next alarm, toggle GPIO 2, count
      A2 TG0 $7C + CONST,  A3 1 MOVI.N,  MEMW,  A3 A2 0 S32I,
      A2 TG0 CONST,  A3 A2 0 L32I,  A4 $400 MOVI,  A3 A3 A4 OR,  MEMW,  A3 A2 0 S32I,
      A2 $60004004 CONST,  A3 A2 0 L32I,  A4 4 MOVI.N,  A3 A3 A4 XOR,  MEMW,  A3 A2 0 S32I,
      A2 TICKS >CHIP CONST,  A3 A2 0 L32I,  A3 A3 1 ADDI.N,  A3 A2 0 S32I,
      RET,
    END-CODE
    ' TICK-ASM 50 INT!  50 INT-ON  +INT

Measured, with the CPU at 240 MHz: a `CODE` handler costs about 0.5 us
(with the saving of the registers). 100 000 interrupts per second make
a loop 4.5 % slower, 200 000 per second 11 %. A `CODE` handler can ask
for a Forth handler: it sets FROM_CPU 1 (source 80), and the Forth
handler of source 80 runs at the next NEXT.

`CODE` words stay in the dictionary: `FREEZE` saves them, as all
words. After an error in the assembler, the search order still has
XTENSA on top: `PREVIOUS`. `' name >CODE` gives the address where the
CPU runs the code of a `CODE` word (on the instruction bus), for
example for `SYSCALL`: the monitor calls a function of its own so
(`asm/monitor/monesp.fs`). The ESP32 has the same words; its code lies
in SRAM0 (`targets/esp32/README.md`, "CODE words").

## The RMT: pulses for IR and LEDs

The remote control unit (RMT) of the chip sends and receives pulses:
a level and a time, again and again. It has 8 channels: 0 to 3 send,
4 to 7 receive. Three packages use it; they are not in the image:

| Package | File | For |
| --- | --- | --- |
| RMT | `lib/esp32/rmt.fs` | each channel: its tick (`RMT-DIV`), its RAM (`RMT-BLOCKS`), its pin (`RMT-OUT`, `RMT-IN`, `RMT-IN-INV`), a carrier for sending (`RMT-CARRIER`), a demodulator, a filter and the end of a frame for receiving (`RMT-DEMOD`, `RMT-FILTER`, `RMT-END`); `RMT-SEND`, `RMT-RX-ON`, `RMT-RECEIVED` |
| IR | `lib/esp32/ir.fs` | frames of IR remote controls as times in us: `IR-PINS`, `IR-SEND`, `IR-FRAME?`, `IR-LISTEN`; channel 1 sends with 38 kHz, channel 4 receives (4 blocks: 384 times) |
| WS2812 | `lib/esp32/ws2812.fs` | the RGB LED, channel 0 |

    board ok> #require RMT lib/esp32/rmt.fs
    board ok> #include lib/esp32/ir.fs
    board ok> ALSO IR  4 14 IR-PINS

The header of each file has its words and an example. The application
`apps/aircon/` controls an air conditioner with them (apps/README.md).

Measured on the board (`lib/esp32/esp32s3/test/rmt.fs`):

- A time comes back as it went out, to 1 us (ticks of 1 us, through
  GPIO 2).
- The carrier of `1 38000 33 RMT-CARRIER` is 347 x 25 ns high and 705
  x 25 ns low: 38.0 kHz with 33 %. The register takes the ticks
  themselves (the manual says "+1"; the measurement says no).
- With `RMT-DEMOD`, a channel gets the envelope back: a mark of 560 us
  comes as 580 us, the space after it 20 us shorter.
- `RMT-SEND` sends 150 pulse words through a channel with one block
  (48): it fills the RAM again while the channel sends.
- The RAM of the RMT takes only accesses of 32 bits: `DUMP` shows 0
  there, `@` the pulse words.
- The end of a frame (`RMT-END`) must be longer than the longest space
  inside a frame: else the frame ends there.
- A frame longer than the RAM of the receiving channel: the chip writes
  the RAM full and ends the frame without an error. `RMT-RECEIVED` sees
  it from the last word, which has no end mark, and gives -1. A frame
  that fills the RAM exactly (192 words with 4 blocks) gives 192.

## An application in the flash

Load the application on the board, then `FREEZE`. With `TURNKEY` the
board starts it 1 s after the restart (ESC or Ctrl-C in this second
gives the prompt). `examples/tick.fs` has `TURNKEY` at its end:

    board ok> #include examples/tick.fs
    board ok> FREEZE
    board ok> #reset
    ...
    autostart in 1 s (Ctrl-C: prompt)
    start
    tick 1
    tick 2

Or build it on the Mac: put the application files before
`targets/freeze.fs`:

    LC_ALL=C awk -f bootstrap/awk/ucf.awk -- -32 lib/package.fs lib/esp32/rom.fs lib/esp32/esp32s3/chip.fs lib/esp32/intr.fs \
       lib/esp32/gpio.fs lib/esp32/timers.fs lib/esp32/flash.fs lib/esp32/esp32s3/clock.fs lib/esp32/esp32s3/psram.fs \
       lib/esp32/esp32s3/bootimg.fs targets/esp32s3/boot.fs examples/tick.fs targets/freeze.fs host/files.fs \
       targets/esp32s3/build.fs </dev/null

## Test

On the board, in `ucf term`:

    board ok> #include forth/test/tester.fs
    board ok> #include forth/test/t0.fs
    board ok> #include forth/test/hayes.fs
    board ok> #include forth/test/all.fs
    board ok> #include lib/esp32/esp32s3/test/intr.fs
    board ok> #include lib/esp32/esp32s3/test/gpio.fs
    board ok> #include lib/esp32/test/timers.fs
    board ok> #include lib/esp32/esp32s3/test/rmt.fs
    board ok> #include lib/esp32/esp32s3/test/ir.fs
    board ok> #require FAIL lib/text.fs
    board ok> #require ADD, asm/xtensa.fs
    board ok> #include lib/esp32/asm.fs
    board ok> #include lib/esp32/esp32s3/test/asm.fs
    board ok> #include lib/net/test/board.fs

Expected: `all: 1055 tests, 0 errors` (8 s at 240 MHz), then `intr: 18
tests, 0 errors` (the package INTR: errors of the CPU, handlers, an
error in a handler, a timer while `MS` waits), `gpio: 20 tests, 0
errors`, `timers: 7 tests, 0 errors`, `rmt: 31 tests, 0 errors` (the
RMT and the WS2812 through GPIO 2), `ir: 11 tests, 0 errors` (frames
of the air conditioner out and back in the chip) and `asm: 16 tests, 0
errors` (`CODE` words, `CODE` handlers up to 100 000 per second); 27 s
together. Two handlers stop with an error on purpose, and their
messages show between the results ("H-ERR stopped: error -77",
"BAD-ONE stopped: error -77"). The last line loads our TCP/IP over
NO-LINK and runs the tests of the network over the loop, with the
steps of the network in a timer (lib/net/README.md, "Test"): `board:
85 tests, 0 errors` after about 11 s (8 s of them for the loading over
`ucf term`). The monitor has its own test (`asm/monitor/README.md`,
"Test"): `monesp: 31 tests, 0 errors`.

The test of the two levels (section "Two levels"), after a restart
(about 80 s with the waits below; the loading takes 12 s):

    board ok> #include lib/esp32/esp32s3/test/h1.fs
    board ok> H1-START                 then wait 20 s at the prompt
    board ok> 10000 H1-BUSY            the console computes 10 s
    board ok> 60000 H1-BUSY            Ctrl-C after a few seconds
    board ok>                          Ctrl-C a few times at the prompt
    board ok> H1-BAD                   its message comes, the rest goes on
    board ok> .H1                      then wait 20 s
    board ok> H1-END

Expected at the end: `h1: 7 tests, 0 errors`, with the counters of
`.H1`: IR frames out and back (none wrong, none lost), the bytes of TCP
out and back (none wrong), and the runs of the timer word.

In the emulator of the archive (a Python program, only a test tool). It
has no ROM, so it needs the image without the packages that call the
ROM:

    LC_ALL=C awk -f bootstrap/awk/ucf.awk -- -32 lib/package.fs targets/freeze.fs host/files.fs targets/esp32s3/build.fs </dev/null
    cat forth/test/tester.fs forth/test/t0.fs forth/test/hayes.fs forth/test/all.fs |
      python3 ../archive/m0-m3/tests/xtensa_emu.py build/esp32s3.bin | tail -2

Expected, after about 40 s: `all: 1055 tests, 0 errors`. The emulator
has no interrupts: `?RX` takes the bytes from UART0 itself.

In QEMU, the emulator of Espressif with the real ROM of the chip
(doc/qemu.md, section "The ESP32 in QEMU"): the image as it is, from a
flash file of 16 MB. There, `ucf term` runs all of t0, hayes and
`all.fs` over a pseudo terminal: `all: 1055 tests, 0 errors` in 10 s;
`FREEZE` writes the flash file, and the next start has the new words.
QEMU has no GPIO and no RMT, so the tests of `lib/esp32/esp32s3/test/`
do not run there; the CPU stays at 80 MHz.

## Memory

| Address | Contents |
| --- | --- |
| `$4037_8000` | the code (IRAM; on the data bus `$3FC8_8000`): the table of the primitives, constants, kernel, HAL, the break table, the variables of the HAL with the buffer of UART0, then at the next 1 KB the vectors (`VECBASE`) and the handlers of exceptions and interrupts |
| `$4038_0000` | VM address 0 on the instruction bus: the machine code of `CODE` words runs here (the data bus address + `$6F_0000`) |
| `$3FC8_FFC0` | the top of the stack for the functions of the ROM (`SYSCALL`); it grows down, the code must end below `$3FC8_C000` |
| `$3FC9_0000` | VM address 0: the memory of the virtual machine, 256 KB up to `$3FCC_FFFF` (doc/vm.md, section 3): 63 KB RAM data, 189 KB dictionary |
| `$3FCD_0000` | the data of the ROM bootloader: not used |
| `$3C00_0000` | the PSRAM, 8 MB, through the data cache: the RAM data after `PSRAM-ON` (section "The PSRAM") |
| `$6000_0000` | the registers of the chip: `@` and `!` reach them |

In the flash: the image at `$0`, the RAM data of the PSRAM from the
last `FREEZE` at `$8_0000`.

After the start, with the image of the section "Build" (base and the
eleven files for the board):

| Area | Size | Free |
| --- | --- | --- |
| dictionary: words, `ALLOT`, `,`; inside the chip | 189 KB | 154 KB |
| RAM data: `VARIABLE`, `VALUE`, `BUFFER:`, vocabularies; in the PSRAM after `PSRAM-ON` | 8 MB | 8 MB |
| RAM data inside the chip: the image, `SRAM-BUFFER:` | 63 KB | 57 KB |
| data stack, return stack | 256 cells each | |
| the input line | 256 characters | |

`UNUSED U.` shows the free bytes of the dictionary,
`ALSO INTERNALS  DATA-END VP @ - U.` those of the RAM data inside the chip,
`XDATA-END @ XVP @ - U.` those in the PSRAM. A full dictionary or RAM
data gives the error -8. doc/vm.md, section 3, has the other limits of
the VM. The flash (16 MB) holds the image and the RAM data of `FREEZE`;
the package FLASH reaches the rest of it.

The kernel keeps IP, W, SP and RP as chip addresses; the cells in memory
hold VM addresses (0 to `$3FFFF`), or chip addresses of the PSRAM. `@ ! C@ C!` and the other memory
words take an address below `$40000` as a VM address (`VM-BASE` +
address) and every other address as it is: so a driver in Forth writes
the registers of the chip.

## The image format of the ROM bootloader

All numbers with the lowest byte first.

| Offset | Bytes | Contents |
| --- | --- | --- |
| 0 | 1 | `$E9` |
| 1 | 1 | the number of segments: here 4 (the code, the RAM data, the dictionary, the code again at its address on the data bus) |
| 2 | 1 | SPI mode of the flash: 2 = DIO |
| 3 | 1 | bits 7-4: flash size (4 = 16 MB); bits 3-0: SPI clock (`$F` = 80 MHz) |
| 4 | 4 | the start address |
| 8 | 1 | the pin for write protect, `$EE` = none |
| 12 | 2 | the chip: 9 = ESP32-S3 |
| 17 | 2 | the highest revision of the chip: `$FFFF` = all |
| 23 | 1 | 1: a SHA-256 digest follows at the end; here 1 |
| 24 | | the segments: address (4 bytes), length (4 bytes, a multiple of 4), the bytes |
| end | | zeros until the next byte ends a block of 16, then the checksum: XOR of all bytes of the segments, from `$EF`; then the SHA-256 digest of all bytes before it (32 bytes) |

## Limits

- `FREEZE` erases the old image first. If the power fails before it
  ends, the board does not start: write an image with `ucf flash`.
  `ERASE-IMAGE` does nothing on the board.
- The flash and the PSRAM share the memory SPI: the words of FLASH take
  buffers only inside the chip (`SRAM-BUFFER:`), and nothing may use
  the PSRAM while they run. The Forth handlers of interrupts wait until
  the function of the ROM has ended.
- The function of the ROM that writes the flash takes at most 255 pages
  of 256 bytes (64 KB) in one call; with more it never ends and writes
  over the whole flash. `FLASH-WRITE` gives it 32 KB at a time; a call
  of it with `SYSCALL` must do the same.
- Ctrl-C stops a function of the ROM only after its end.
- All Forth handlers share CPU interrupt 1; `CODE` handlers are for 3
  sources at most (CPU interrupts 19, 20, 21). Levels 4 and 5 are not
  used.
- The console is UART0; the USB socket of the chip (USB-Serial/JTAG)
  comes later.
