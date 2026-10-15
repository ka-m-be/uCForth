# lib/irremote: IR remote controls

The packages in this folder make the frames of IR remote controls and
read them back. They do not touch a chip, so they run on every system,
also on the host, where their tests run. The code that drives a chip
(the RMT, the pins, the interrupts) is in `lib/esp32/`: the code for
both chips in that folder, the code for the ESP32 only in
`lib/esp32/esp32/`.

Each package that reads frames back is in a file of its own. So a
program that only sends frames (the IR bridge) loads only the parts
that make them, and IR-SYMBOLS for the frames that it hears.

## The files

| File | Package | Contents | Needs |
| --- | --- | --- | --- |
| `acbase.fs` | AC-BASE | what the frames of air conditioners share: the modes (`HEAT AUTO COOL DRY FAN`), bit fields in a frame (`FIELDS-IN`, `FIELD`, `FIELD@`, `FIELD!`), the XOR of bytes, the clock of the frame (`SET-CLOCK`, `NOW`), the state as text (`MODE-NAME`, `HM>S`) and words that show it (`.ON`, `.MODE`, `.HM`, `.BYTES`) | – |
| `ircode.fs` | IRCODE | the pulse distance code: a shape (`SHAPE`, `SHAPE-NOW`), bytes to times in us (`>TIMINGS`) | – |
| `irread.fs` | IRCODE-READ | times back into bytes (`TIMINGS>`) | IRCODE |
| `rty3.fs` | RTY3 | the frame of the remote control RCH-RTY3-0 (23 bytes, measured): the state, the checksum, the times (`AC-RESET`, `AC-POWER!` ... `AC-CLOCK!`, `AC-POWER@` ... `AC-FLAP@`, `AC-FRAME`, `AC-TIMINGS`, `AC>TIMES`) | AC-BASE, IRCODE |
| `rty3read.fs` | RTY3-READ | frames of RTY3 back into bytes and their checks, a frame as the state, the state in words (`AC-DECODE`, `AC-CHECK?`, `AC-OK?`, `AC-TAKE`, `.AC`, `.FRAME`) | RTY3, IRCODE-READ |
| `kelon168.fs` | KELON168 | the frame of other Hisense remote controls (21 bytes; DG11R2-01, RCH-R0Y3): the words of RTY3 without `AC-FLAP!`, and swing, sleep, super, display, the timers, `FAN2-AT` | AC-BASE, IRCODE |
| `kelonrd.fs` | KELON168-READ | the words of RTY3-READ for KELON168, without `AC-CHECK?`, and `.UNKNOWN` | KELON168, IRCODE-READ |
| `irsym.fs` | IR-SYMBOLS | any frame as a table of at most 16 durations and a hex digit for each time, and back (`>SYMBOLS`, `DURATIONS`, `SYMBOLS>`): the short form of the bridge | – |

The words of RTY3 and KELON168 have the same names. So an application
changes only the package (`ALSO RTY3` or `ALSO KELON168`). Each file
has its words, an example and its load lines at its top.

## Load

The order is the order of the table. On the host, in the folder `ucf`:

    ALSO FILES
    S" lib/irremote/acbase.fs" INCLUDED
    S" lib/irremote/ircode.fs" INCLUDED
    S" lib/irremote/rty3.fs" INCLUDED
    ALSO AC-BASE  ALSO RTY3
    TRUE AC-POWER!  HEAT AC-MODE!  23 AC-TEMP!
    AC-FRAME 23 .BYTES

The last line shows the frame:

    60 38 13 01 FF 00 46 00 00 00 00 00 00 00 00 00 00 00 00 02 00 00 BA

On a board, `ucf term` loads with `#require` lines, as
`apps/aircon/app.fs` does:

    #require AC-BASE ../../lib/irremote/acbase.fs
    #require IRCODE ../../lib/irremote/ircode.fs
    #require IRCODE-READ ../../lib/irremote/irread.fs
    #require RTY3 ../../lib/irremote/rty3.fs
    #require RTY3-READ ../../lib/irremote/rty3read.fs

## Memory

Measured on the C VM with 32-bit cells (as on the ESP32), on
14.10.2026. "Heads on": `TEMP-HEADS ON!` (lib/package.fs), as the
loader of the bridge does it; the private words keep no headers.
"Heads off": the default, as `ucf term` loads.

| File | Dictionary, heads on | Dictionary, heads off | RAM data |
| --- | ---: | ---: | ---: |
| `acbase.fs` | 1 524 B | 1 624 B | 28 B (40 B) |
| `ircode.fs` | 984 B | 1 052 B | 36 B |
| `irread.fs` | 692 B | 808 B | 24 B (36 B) |
| `rty3.fs` | 2 124 B | 2 268 B | 1 572 B |
| `rty3read.fs` | 1 088 B | 1 140 B | 76 B (88 B) |
| `kelon168.fs` | 3 744 B | 4 000 B | 1 460 B |
| `kelonrd.fs` | 1 484 B | 1 592 B | 76 B (88 B) |
| `irsym.fs` | 1 304 B | 1 524 B | 300 B (312 B) |

The RAM data in brackets are those with heads off: there the first
private word makes the vocabulary `name.I` (12 B). Most of the RAM data
of RTY3 and KELON168 is the buffer for the times of a frame (380 and
350 cells); IR-SYMBOLS has its table and its sums.

To send frames of RTY3, a program needs AC-BASE, IRCODE and RTY3:
4 632 B with heads on; the bridge also loads IR-SYMBOLS (5 936 B). The
aircon application loads all parts of RTY3: 6 892 B with heads off.

## Where the IR code is

| Where | File | What |
| --- | --- | --- |
| this folder | see above | frames: make them, read them back |
| `lib/esp32/` | `ir.fs` | package IR: frames as times in us over the RMT, both chips (`IR-PINS`, `IR-SEND`, `IR-FRAME?`, `IR-LISTEN`) |
| | `rmt.fs` | chooses the package RMT of the chip (`#when`) |
| `lib/esp32/esp32/` | `rmt.fs` | package RMT of the ESP32 |
| | `ircap.fs` | package IR-CAPTURE: the receiver into 4 raw slots by a `CODE` handler; for IR-QUEUE |
| | `irqueue.fs` | package IR-QUEUE: IR in the background for the bridge, with the check of the echo (lib/esp32/README.md, "IR for the bridge on the WROOM") |
| `lib/esp32/esp32s3/` | `rmt.fs` | package RMT of the ESP32-S3 |
| `apps/aircon/` | `app.fs`, `aircon.fs` | the air conditioner at the console: RTY3 and RTY3-READ over IR |
| `apps/irbridge/` | `bridge.fs` | the IR bridge: frames over MQTT, over IR-QUEUE |
| | `frames.fs` | package BRIDGE-FRAMES: the frames in JSON; it uses AC-BASE, IRCODE, RTY3 and IR-SYMBOLS |

The tests:

| Where | File | What |
| --- | --- | --- |
| `lib/test/` | `ircode.fs`, `rty3.fs`, `kelon168.fs`, `irsym.fs` | the packages of this folder, each with its part that reads, on the host (`tools/test.fs`); `rty3raw.fs`: a real frame of RCH-RTY3-0 as times, for `rty3.fs` and `irsym.fs` |
| | `heads.fs` | all packages of the host once with heads on: the parts that read find their words |
| `lib/esp32/test/` | `irmock.fs`, `irqueue.fs` | IR and IR-QUEUE over mocks of the chip, on the host |
| `lib/esp32/esp32/test/` | `ir.fs`, `rmt.fs`, `irqueue.fs`, `irqbusy.fs` | IR, RMT and IR-QUEUE on the WROOM, over GPIO 25 (a loop inside the chip, no light) |
| `lib/esp32/esp32s3/test/` | `ir.fs`, `rmt.fs`, `h1.fs` | IR and RMT on the ESP32-S3; `h1.fs`: IR and the network in the background |
| `apps/irbridge/test/` | `bridge.fs` | the bridge with mocks, on the host |
