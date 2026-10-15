# tools: applications on the host

A tool is an application: it loads the packages it needs, and they do not
become part of the base vocabulary. Start a tool in the folder `ucf`,
after the package FILES.

| File | Contents |
| --- | --- |
| `test.fs` | all tests; the exit status is 0 if all pass |
| `ucf.fs` | the tool `ucf`: `info`, `flash`, `reset`, `term`, `run` for an ESP32-S3 or an ESP32; a program for the Mac, Windows or Linux (section "ucf.fs") |
| `genesis.fs` | uCForth builds its reference images itself and compares them with `seed/`; the exit status is 0 if they are the same |
| `pick.fs` | package PICK: copies from a quarry of code of C (`blob/esp32`) only the functions and the data that an application needs; a pick list says which (section "pick.fs") |
| `shadows.fs` | at the end of each package that loads after it: the words of the package that a package of its PACKAGE line hides (section "shadows.fs") |

    LC_ALL=C awk -f bootstrap/awk/ucf.awk -- host/files.fs tools/test.fs
    LC_ALL=C awk -f bootstrap/awk/ucf.awk -- host/files.fs tools/genesis.fs

With `-32`, both tools work with 32-bit cells. The bash bootstrap VM runs
`test.fs` too (3 minutes), but it is too slow for `genesis.fs`. The C
bootstrap VM (`build/ucf-c host/files.fs tools/genesis.fs`) runs
`genesis.fs` in 1.5 s.

## ucf.fs

The tool `ucf` connects the computer to a board with uCForth. A
bootstrap VM builds it with the packages it needs (FILES, TEXT, SERIAL,
SLIP, MD5, ESPROM, TERM, ESPIMG), as a program for the Mac:

    LC_ALL=C awk -f bootstrap/awk/ucf.awk -- -32 host/files.fs tools/ucf.fs targets/macos-a64/build.fs </dev/null
    chmod +x build/ucf

or for Windows, `build/ucf.exe` (`targets/win32/README.md`; the port
is then `COM3` or similar):

    LC_ALL=C awk -f bootstrap/awk/ucf.awk -- -32 host/files.fs tools/ucf.fs targets/win32/build.fs </dev/null

or for Linux, `build/ucf-a64` (64-bit ARM, `targets/linux-a64/README.md`),
`build/ucf-x64` (a PC with 64 bits, `targets/linux-x64/README.md`) or
`build/ucf-i386` (a PC, `targets/linux-i386/README.md`); the port is
then `/dev/ttyACM0` or `/dev/ttyUSB0`:

    LC_ALL=C awk -f bootstrap/awk/ucf.awk -- -32 host/files.fs tools/ucf.fs targets/linux-a64/build.fs </dev/null
    LC_ALL=C awk -f bootstrap/awk/ucf.awk -- -32 host/files.fs tools/ucf.fs targets/linux-x64/build.fs </dev/null
    LC_ALL=C awk -f bootstrap/awk/ucf.awk -- -32 host/files.fs tools/ucf.fs targets/linux-i386/build.fs </dev/null

or for a Mac with Intel, `build/ucf-intel` (`targets/macos-x64/README.md`;
copy it as `ucf` on that Mac):

    LC_ALL=C awk -f bootstrap/awk/ucf.awk -- -32 host/files.fs tools/ucf.fs targets/macos-x64/build.fs </dev/null

The first argument is the command:

    $ build/ucf help
    usage: ucf info PORT
           ucf flash PORT FILE ADDRESS    (ADDRESS: 0xFF0000, '$FF0000' or 16711680)
           ucf reset PORT
           ucf term PORT [FILE]           (FILE: #include it first)
           ucf run PORT KERNEL [IMAGE]    (into the RAM, then term)
           ucf                            (the prompt of the host Forth)
      error: unknown command: help

`info`, `flash` and `run` talk to the ROM bootloader of the ESP32-S3 or
of the ESP32 (package ESPROM; it finds the chip itself, and `run`
refuses an image for the other chip); `term` is the console to uCForth
on the board (package TERM, host/README.md). With FILE, `term` first
sends the file to the board: so an application starts in one line
(apps/README.md). The exit status is 0, or 1 after an error. How to put
uCForth on a board: `targets/esp32s3/README.md`,
`targets/esp32/README.md` and `doc/start.md`.

### Which computer, which board

The tool runs on a target for a computer that has files, serial lines
and the arguments of the command line (HAL 89 to 97, doc/vm.md,
section 8):

| Computer | The tool | Status |
| --- | --- | --- |
| macOS on Apple silicon | `build/ucf` (`targets/macos-a64`) | tested with the ESP32-S3 and the ESP32 |
| Windows | `build/ucf.exe` (`targets/win32`) | tested in Wine with pseudo terminals; not yet with a real port |
| Linux on 64-bit ARM (a Raspberry Pi) | `build/ucf-a64` (`targets/linux-a64`) | tested in Docker with pseudo terminals; not yet with a real port |
| Linux on a PC | `build/ucf-i386` (`targets/linux-i386`) | tested in Docker with pseudo terminals; not yet with a real port |
| Linux on a PC with 64 bits | `build/ucf-x64` (`targets/linux-x64`) | tested in Docker with pseudo terminals; not yet with a real port |
| macOS on Intel | `build/ucf-intel` (`targets/macos-x64`) | tested with Rosetta on Apple silicon with the ESP32-S3 (`info`, `reset`, `term`); not yet on a Mac with Intel |
| a bootstrap VM | no: it has no serial lines | |

The commands and the boards:

| Command | Board | Why |
| --- | --- | --- |
| `term` | each board with uCForth on a serial line with 115200 baud | the console of uCForth is text, with the prompt `ok>`; package TERM |
| `info`, `flash`, `reset`, `run` | the ESP32-S3 and the ESP32 | they talk to the ROM bootloader of the chip (package ESPROM, its commands and the image format of `lib/esp32/`); `reset` and the start of the bootloader use the lines DTR and RTS, as the boards with these chips have them |

**Another board,** for example the Raspberry Pi 4: its program goes
onto the SD card (`targets/rpi4/README.md`), not over the serial line.
Its console is the UART on GPIO 14 and 15, with a USB serial adapter:
`ucf term PORT` is the console then (not yet tested with the Pi; the
README of the target shows `screen`). A board that gets its program
over a serial line, from a bootloader of its own, needs a package for
that protocol (as ESPROM is for the ESP32 chips), and a command in
`ucf.fs` that uses it.

**A PC without an operating system** (`targets/pc-bios`) is such a
board too: its program goes onto a disk (doc/disks.md), and COM1 with a
null modem cable is the line for `ucf term PORT` (tested in QEMU).

## genesis.fs

The tool runs a second uCForth, the target, in the package VM
(`lib/vm.fs`). It does in Forth what a bootstrap VM does in its own
language (doc/genesis.md): Genesis compiles `forth/core.fs`, then `QUIT`
of the target compiles `forth/base.fs`. It writes both images as Intel
HEX and compares the text with `seed/`, line by line. Expected:

      check core16: the same as seed/core16.hex
      check base16: the same as seed/base16.hex

So the circle is closed: uCForth, built by a bootstrap VM, builds the
same images again, without the bootstrap VM's Genesis. The target has
the cell size of the host: a 16-bit host builds the 16-bit images.

Time for one cell size: about 1 minute with mawk, 5 minutes with the awk
of macOS (Apple M4). The target VM runs in Forth on the bootstrap VM, so
each step of the target takes about 40 steps of the host.

## pick.fs

The quarry `blob/esp32` has three libraries of C of Espressif as
packages of `CODE` words: one package file for each object, one `CODE`
word for each function, 1 114 functions in all (its README). An
application needs only a few of them. The tool PICK reads package
files of the quarry, follows the references from the functions that
the application calls, and copies only the functions and the data that
they need.

A pick list says what to do. It is a Forth file: it loads
`tools/pick.fs` and then uses its words. The pick list of the radio of
the ESP32 is `lib/esp32/esp32/phypick.fs` (shortened):

    S" tools/pick.fs" INCLUDED
    PICK-FROM blob/esp32/libphy/phy_chip_v7.fs       chip    lib/esp32/esp32/phychip.fs
    PICK-FROM blob/esp32/librtc/rtc_analog.fs        rtc     lib/esp32/esp32/phyrtc.fs
    PICK-OUTSIDE ets_delay_us memcpy phy_get_romfuncs
    PICK-ENTRY register_chipv7_phy phy_wakeup_init phy_close_rf
    PICK-DATA lib/esp32/esp32/phydata.fs
    PICK-FOR ESP32-PHY lib/esp32/esp32/phypick.fs
    PICK-LICENSE Copyright Espressif Systems (Shanghai) Co., Ltd., Apache License 2.0
    PICK-SHA256 blob/esp32/libphy/libphy.a c2521071695734afa8f6f1ca3d3b604a1d08dd63fe52060449a11cfe8bac5740
    PICK-RUN
    PICK-WRITE

| Word | Effect |
| --- | --- |
| `PICK-FROM file tag out` | read this package file; the functions that PICK takes from it go to the file out. The tag has at most 7 characters (below) |
| `PICK-DATA out` | the data of all package files go to the file out |
| `PICK-OUTSIDE names` | the names outside the package files that the code may use: functions of the ROM, functions of your own (the rest of the line) |
| `PICK-ENTRY names` | the functions that the application calls (the rest of the line) |
| `PICK-FOR name file` | the copies are for the package name, and file is the pick list; both go into the heads |
| `PICK-LICENSE text` | a line of the license for the heads (the rest of the line) |
| `PICK-SHA256 file hex` | the SHA-256 that this archive must have |
| `PICK-RUN` | read the package files, check them, find the hull, and show it |
| `PICK-WRITE` | after `PICK-RUN`: check the archives, then write the copies |
| `PICK-LIST` | after `PICK-RUN`: the names in the hull |

Run the pick list in the folder `ucf`, with 32-bit cells:

    build/ucf-c -32 host/files.fs lib/esp32/esp32/phypick.fs </dev/null

The output, shortened: first what PICK read, then the hull, the names
outside, and the copies:

    package                 funcs global   data   code    RAM   dict
    PHY-PHY-CHIP-V7           153    149     50  34712    572   1762
    ...
    all                       304    298    102  70512    684   2573

    the hull of 8 entries:
    package                 funcs   data   code    RAM   dict  lines
    PHY-PHY-CHIP-V7            74     34  21072    497   1707   2937
    ...
    all                       171     76  47169    597   2446   6858
    names outside, with the number of literals:
      ets_delay_us (36)
    ...
    the copies:
      lib/esp32/esp32/phychip.fs: 2893 lines
    ...
    written in 2168ms; 6 names renamed

What PICK does:

1. **Read.** PICK reads each package file line by line, in the format
   of the quarry (`blob/esp32/README.md`, section 2). An object is a
   function (`CODE` ... `END-CODE`), data in the RAM data region
   (`n DATA: name` with its lines `INIT`), or data in the dictionary
   (`CREATE name` with its lines). PICK counts the functions and the
   data objects and compares them with the head of the package file. A
   line that it does not know stops it, with the file and the line.
2. **The hull.** An object needs each object that it names: the name
   after `SYMBOL`, `SYMBOL+` and `FUNC`, the table of jumps before
   `L!+`, and for data the name after `SYMBOL!`. PICK looks for a name
   as CLINK does: in the package of the object, then in the PUBLIC
   names of all package files. The hull is the entries, the objects
   that they need, the objects that these need, and so on. A name
   outside the package files must be on a line `PICK-OUTSIDE`, else
   PICK stops.
3. **Write.** `PICK-WRITE` first makes the SHA-256 of each archive
   (`lib/sha256.fs`). If one is not that of its `PICK-SHA256`, PICK
   stops and writes nothing. Then it writes each line of each object
   of the hull as it is, in the order of its package file: functions to
   the file of their `PICK-FROM`, data to the file of `PICK-DATA`. The
   frames of the packages stay out (`PACKAGE`, `PUBLIC`, `PRIVATE`,
   `EXPORTS`, `LINK-SYMBOLS`, `END-PACKAGE`): the file that loads the
   copies gives the frame (`lib/esp32/esp32/phy.fs`). When two objects
   of the hull have the same name (upper and lower case count as the
   same, as in a search of Forth), each gets the tag of its
   `PICK-FROM`, everywhere: `tag:name`. Each copy starts with a head:
   the package files, the SHA-256 of their archives, the lines of
   `PICK-LICENSE`, the new names. At the end PICK checks that it wrote
   each line of the hull once.

The same pick list with the same quarry gives the same copies, byte
for byte. The C bootstrap VM (3.7 s), the JavaScript VM (28 s) and the
Python VM (17 minutes) write the same copies. The awk and bash VMs read
files only as text and lose bytes of the archives: so `PICK-WRITE`
stops at the SHA-256, before it writes.

`lib/esp32/README.md` ("The radio of the ESP32: ESP32-PHY") shows how
the radio uses the copies, and how to pick again. A pick list for
another application works in the same way: its entries, the names
outside that it gives itself, and a file that loads the copies with
CLINK, as `phy.fs` does.

## shadows.fs

Inside a package, the packages of its PACKAGE line come first in the
search order (`lib/README.md`, "Packages"). A word of the package with
the name of a word of such a package is found by the later words of
the package only as that other word, without a message. `shadows.fs`
puts a check before `END-PACKAGE`: at the end of each package that
loads after it, one line for each such name: of the `PRIVATE`, the
`PROTECTED` and the `PUBLIC` words. It changes nothing else;
`.SHADOWS` gives the number of the lines. Load it first, then the
files, for example all tests:

    $ printf 'ALSO FILES INCLUDE tools/shadows.fs\nALSO FILES INCLUDE tools/test.fs\n' | build/ucf-c -32 host/files.fs
    ...
      LINK-NET: (TCP-CONNECT) is also in TCP
    ...
      RADIO-LINK: LINK-MAC is also in AP-LINK
    ...
    all: 12186 tests, 0 errors

These two show on purpose: LINK-NET wraps the words of UDP and TCP in
words with the same name (`: (TCP-CONNECT) ['] (TCP-CONNECT) LOCKED ;`),
and RADIO-LINK hands its words on to the LINK of the same name; no
later word of the two packages needs its own word under that name.
With 16-bit cells there is no line. The test: `tools/test/shadows.fs`.

