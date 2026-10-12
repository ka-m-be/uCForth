# First steps

This manual shows a path through uCForth: from the first prompt on your
computer to uCForth on a board. Each step gives the commands and what
comes out. The README of each folder tells more.

For the basics of Forth, read `forth.md` first.

What you need:

- For steps 1 to 3: a computer with awk or bash or Javascript or...:
  Linux, MacOS, BSD, or a small system with BusyBox.
- For step 4: MacOS, Linux or Windows.
- For steps 5 and 6: MacOS (Windows and Linux have the tool `ucf` too), and
  an ESP32-S3 board or an ESP32 board with a USB cable.

Give all commands in the top folder. The times are from a Mac with
Apple M4.

## 1 The first prompt

A *bootstrap VM* runs uCForth on a computer that has no uCForth yet
(bootstrap/README.md). The awk bootstrap VM runs on each system with
awk:

    $ LC_ALL=C awk -f bootstrap/awk/ucf.awk
    uCForth - awk bootstrap VM - 16 bit - 163 words in FORTH - built in 1 s
    ok> 3 4 + .
    7
    ok> : SQUARE ( n -- n*n )  DUP * ;
    ok> 12 SQUARE .
    144
    ok> BYE

The bootstrap VM compiles `forth/core.fs` and `forth/base.fs`, then the
prompt `ok>` comes. `BYE` ends uCForth. `LC_ALL=C` tells awk that one
character is one byte. The bash bootstrap VM does the same
(`bash bootstrap/bash/ucf.sh`), but its start takes 20 to 30 s.

With a C compiler, the C bootstrap VM is the fastest: its prompt comes
at once, and it builds each program of step 4 to 6 in less than half a
second (bootstrap/c/README.md). Build it once, then use `build/ucf-c`
instead of `LC_ALL=C awk -f bootstrap/awk/ucf.awk --` in each command
of this manual:

    $ cc -O2 -o build/ucf-c bootstrap/c/ucf.c
    $ build/ucf-c
    uCForth - C bootstrap VM - 16 bit - 163 words in FORTH - built in 4 ms
    ok>

There are bootstrap VMs for Python and for JavaScript too; the
JavaScript one also runs in a web browser (bootstrap/README.md). On
Windows, one in JScript needs nothing but Windows
(bootstrap/windows/README.md).

## 2 Files and the search order

Files on the command line load before the keyboard. They come after
`--`, which stops the options of awk. The package FILES
(`host/files.fs`) gives uCForth files and `INCLUDE`:

    $ LC_ALL=C awk -f bootstrap/awk/ucf.awk -- host/files.fs
    uCForth - awk bootstrap VM - 16 bit - 163 words in FORTH - built in 1 s
    ok> INCLUDE examples/hello.fs
    Hello from uCForth

The words of uCForth are in vocabularies. The search order is the list
of the vocabularies in which the interpreter looks for a word. `ORDER`
shows it, the first vocabulary on the left; after `current:` comes the
vocabulary for new words. `VOCS` shows all vocabularies, `WORDS` the
words of the first vocabulary in the search order:

    ok> ORDER
    FILES FORTH   current: FORTH
    ok> VOCS
    PACKAGES.I FILES.I FILES INTERNALS FORTH
    ok> ALSO FILES.I  ORDER
    FILES.I FILES FORTH   current: FORTH
    ok> WORDS

    (INCLUDE-FILE) .WHERE (INCLUDE) FID THIS FILE-NAMES LINE-NOS FIDS
    SAVED-TIBS NEST MAX-NEST PRIM
    ok> ONLY FORTH  ORDER
    FORTH   current: FORTH

`ALSO name` puts a vocabulary first, `PREVIOUS` takes the first one
away, `ONLY FORTH` leaves only FORTH. A package such as FILES has two
vocabularies: `FILES` for its words, `FILES.I` for its helpers (I:
internals; lib/README.md). After a package loads, it is in the search
order. Each program starts with only FORTH; `ALSO name` then gives the
words of a package.

## 3 The tests

    $ LC_ALL=C awk -f bootstrap/awk/ucf.awk -- host/files.fs tools/test.fs </dev/null
    ...
    all: 2742 tests, 0 errors

This takes about 20 s. `</dev/null` gives the bootstrap VM an empty
keyboard: without it, the bootstrap VM waits for keys after the last
file. With `-32` (32-bit cells, before `host/files.fs`), more packages
run their tests: `all: 10979 tests, 0 errors`.

## 4 A program for your computer

The bootstrap VM also builds uCForth as a program: the virtual machine
in machine code, and the image of the running system, in one file. The
program needs no other file. On macOS on Apple silicon (about 1
minute):

    $ LC_ALL=C awk -f bootstrap/awk/ucf.awk -- -32 host/files.fs targets/macos-a64/build.fs </dev/null
    ...
      code: 7900 bytes, image: 86824 bytes
      written: build/ucf-macos-a64 (115696 bytes)
    $ chmod +x build/ucf-macos-a64
    $ build/ucf-macos-a64
    ok> 3 4 * .
    12
    ok> BYE

The parts of the command line:

- `-32`: the program has 32-bit cells.
- `host/files.fs`: the package FILES goes into the program.
- `targets/macos-a64/build.fs`: the tool that builds the program. It
  freezes the running system as the image (`FREEZE`), makes the machine
  code with the assembler `asm/aarch64.fs`, and writes the file.
- Files between `host/files.fs` and `build.fs` go into the program too.
  So an application becomes a program (targets/macos-a64/README.md).

On Linux on 64-bit ARM (also Raspberry Pi OS with 64 bits), the same
with `targets/linux-a64/build.fs` gives `build/ucf-linux-a64`
(targets/linux-a64/README.md); on Linux on a PC, with
`targets/linux-x64/build.fs`, `build/ucf-linux-x64` (64 bits,
targets/linux-x64/README.md) or with `targets/linux-i386/build.fs`,
`build/ucf-linux-i386` (32 bits, targets/linux-i386/README.md). On a Mac
with Intel, `targets/macos-x64/build.fs` gives `build/ucf-macos-x64`
(targets/macos-x64/README.md). On Windows, the same with
`targets/win32/build.fs` gives `build/ucf-win32.exe`
(targets/win32/README.md). On other computers the bootstrap VM stays
the host for now.

The program runs all tests in about 0.4 s:

    $ echo "ALSO FILES  INCLUDE tools/test.fs" | build/ucf-macos-a64
    ...
    all: 10979 tests, 0 errors

On macOS and Linux, the program also has the network: TCP, UDP and
names over DNS (lib/net/README.md). Our own TCP/IP in Forth, for a
board, runs there too, over the loop or over a TAP device of Linux. A
page from a web server:

    ok> ALSO FILES  INCLUDE examples/webget.fs
    ok> S" example.org" WEBGET
    HTTP/1.1 200 OK
    ...

For each computer with a web browser, `targets/browser/build.fs` gives
`build/ucf-web.html`: one file with the JavaScript bootstrap VM and the
image. A double click opens it, and the prompt comes at once
(targets/browser/README.md):

    $ LC_ALL=C awk -f bootstrap/awk/ucf.awk -- -32 host/files.fs targets/browser/build.fs </dev/null
    ...
      written: build/ucf-web.html (122514 bytes, 32-bit cells)

## 5 The tool ucf (macOS)

The tool `ucf` connects the Mac to a board: it writes the flash,
restarts the board, and is the console. It is an application from
packages (`tools/ucf.fs`). The bootstrap VM builds it as in step 4
(about 80 s):

    $ LC_ALL=C awk -f bootstrap/awk/ucf.awk -- -32 host/files.fs tools/ucf.fs targets/macos-a64/build.fs </dev/null
    ...
      written: build/ucf (132208 bytes)
    $ chmod +x build/ucf

Its commands are in tools/README.md. With `targets/win32/build.fs`
instead, the same command line gives `build/ucf.exe`, the tool for
Windows; with `targets/linux-a64/build.fs`, `targets/linux-x64/build.fs`
or `targets/linux-i386/build.fs`, `build/ucf-a64`, `build/ucf-x64` or
`build/ucf-i386` for Linux; with `targets/macos-x64/build.fs`,
`build/ucf-intel` for a Mac with Intel. `info`, `flash`, `reset` and
`run` are for the ESP32-S3 and the ESP32: tools/README.md, section
"Which computer, which board".

## 6 uCForth on an ESP32-S3 board

**Build the image** `build/esp32s3.bin` (about 75 s):

    LC_ALL=C awk -f bootstrap/awk/ucf.awk -- -32 lib/package.fs lib/idle.fs lib/esp32/rom.fs lib/esp32/esp32s3/chip.fs \
       lib/esp32/intr.fs lib/esp32/gpio.fs lib/esp32/timers.fs lib/esp32/flash.fs lib/esp32/esp32s3/clock.fs \
       lib/esp32/esp32s3/psram.fs lib/esp32/esp32s3/bootimg.fs targets/esp32s3/boot.fs targets/freeze.fs host/files.fs \
       targets/esp32s3/build.fs </dev/null

The command line has the package IDLE (`lib/idle.fs`: the words that
run while the console waits), the packages for the board
(`lib/esp32/`, and the ones that only the ESP32-S3 has in
`lib/esp32/esp32s3/`), the start of the board (`boot.fs`), `FREEZE`
(`targets/freeze.fs`), and the tool that writes the file (`build.fs`).
Each part is in targets/esp32s3/README.md, section "Build".

**Connect the board.** A USB cable goes from the Mac to the socket "USB
to serial port" of the board. `ls /dev/cu.*` shows its port:
`/dev/cu.usbmodem...` or `/dev/cu.usbserial-...`. Here it is
`/dev/cu.usbmodem5C4C0505931`.

**Ask the chip.** The ROM bootloader of the chip answers; `ucf` starts
it over the lines DTR and RTS:

    $ build/ucf info /dev/cu.usbmodem5C4C0505931
    connecting .. ok
    chip        ESP32-S3 (QFN56), revision v0.2
    features    Wi-Fi, BLE
    in package  PSRAM 8 MB
    link        UART
    mac         ...
    crystal     40 MHz

**Write the image** to the flash at address 0, restart the board, and
start the console:

    $ build/ucf flash /dev/cu.usbmodem5C4C0505931 build/esp32s3.bin 0
    connecting .. ok
    erasing 55 KB at $0 ...
    writing ... ok
    checking MD5 ... ok
    55 KB in 5.9 s
    The board waits in the bootloader; reset starts it.
    $ build/ucf reset /dev/cu.usbmodem5C4C0505931
    restarted over RTS
    $ build/ucf term /dev/cu.usbmodem5C4C0505931
    term on /dev/cu.usbmodem5C4C0505931: #help shows the commands
    ESP-ROM:esp32s3-20210327
    ...
    entry 0x403790a4
    board ok> 3 4 + .
    7
    board ok> ORDER
    FORTH   current: FORTH
    board ok> #bye
    term ends: back on the host

The ROM of the chip sends the first lines, then uCForth starts and
sends its prompt. `board ok>` is the prompt of the board: each line
goes to the board, except the commands of term. They start with `#`
(host/README.md). When the board runs already, no prompt comes at
first: press the return key, and the board answers with its prompt.

**Load a file** with `#include`. The LED of the board blinks (on this
board the RGB LED):

    board ok> #include examples/blink.fs
    board ok> BLINK

The return key stops it. The file loads the package of the board
first: its LED and its button BOOT (lib/esp32/README.md, "The
layout"). The words for the board are in packages:
`ALSO GPIO` gives the words for the pins, `ALSO TIMERS` the words
that run later, and so on; `VOCS` shows them all
(targets/esp32s3/README.md).

**Start an application** with its file after the port: `term` sends
the file first, then you use its words (apps/README.md):

    $ build/ucf term /dev/cu.usbmodem5C4C0505931 apps/aircon/app.fs

**An ESP32 board** (for example with the module ESP32-WROOM-32) goes
the same way, with its own image. The ROM of the ESP32 reads the image
at `$1000` in the flash (targets/esp32/README.md). Build it (about 75
s), then ask the chip, write the image, restart, and start the console;
here the port is `/dev/cu.usbserial-110`:

    LC_ALL=C awk -f bootstrap/awk/ucf.awk -- -32 lib/package.fs lib/idle.fs lib/esp32/rom.fs lib/esp32/esp32/chip.fs \
       lib/esp32/intr.fs lib/esp32/gpio.fs lib/esp32/timers.fs lib/esp32/flash.fs lib/esp32/esp32/clock.fs \
       lib/esp32/esp32/bootimg.fs targets/esp32/boot.fs targets/freeze.fs host/files.fs targets/esp32/build.fs </dev/null

    $ build/ucf info /dev/cu.usbserial-110
    connecting .. ok
    chip        ESP32-D0WD-V3, revision v3.1
    ...
    $ build/ucf flash /dev/cu.usbserial-110 build/esp32.bin 0x1000
    ...
    47 KB in 5.2 s
    The board waits in the bootloader; reset starts it.
    $ build/ucf reset /dev/cu.usbserial-110
    $ build/ucf term /dev/cu.usbserial-110

The words are the same as on the ESP32-S3, and so is the line for an
application: `apps/aircon/app.fs` runs on both boards.

**The IR bridge on the WROOM** (in work: the WLAN with a saved
network, a setup network, sockets, IR in the background): after the
steps above, one line loads it and freezes it into the flash (about 2
minutes):

    $ build/ucf term /dev/cu.usbserial-110 apps/irbridge/app.fs
    ...
    irbridge: ready; WBACK-START joins the saved WLAN, 4 14 IR-Q-START starts IR
    board ok> WBACK-START
    WLAN back: link and DHCP ready

Then the words of NET work as on a host. `WBACK-START` needs the
profile of the network in the flash, and the passphrase is typed only
at the board: apps/irbridge/README.md has these steps, the setup
network, IR, and what to do when it does not work.

## 7 Where next

| For | Read |
| --- | --- |
| Forth itself: the stack, the words, how to read source | doc/forth.md |
| all words of FORTH and INTERNALS in one list | doc/words.md |
| the virtual machine and its rules | doc/vm.md |
| how a bootstrap VM works | bootstrap/c/README.md (the reference), bootstrap/awk/README.md, bootstrap/bash/README.md, doc/genesis.md |
| packages | lib/README.md |
| the network: TCP, UDP, DNS on macOS and Linux; our own TCP/IP in Forth for a board; the layers | lib/net/README.md |
| the host: files, serial lines, term | host/README.md |
| the ESP32-S3: `FREEZE`, clock, PSRAM, interrupts, GPIO, timers, the RMT, `CODE` words | targets/esp32s3/README.md |
| the ESP32: the board, its memory, `CODE` words in SRAM0, what is different; which of the two chips | targets/esp32/README.md, lib/esp32/README.md ("Which board") |
| the monitor: registers, calls of the BIOS or the ROM, disassemblers for the x86, the Xtensa and AArch64 | asm/monitor/README.md |
| applications for a board, for example an air conditioner over infrared | apps/README.md |
| the other targets: Linux, Windows, a Mac with Intel, the Raspberry Pi 4, MS-DOS, a PC without an operating system (BIOS or UEFI), a web browser | targets/README.md, targets/linux-x64/README.md, targets/linux-i386/README.md, targets/macos-x64/README.md, targets/win32/README.md, targets/uefi-x64/README.md, targets/dos/README.md, targets/pc-bios/README.md, targets/browser/README.md |
| uCForth on a USB stick, a floppy disk or an SD card | doc/disks.md |
| uCForth on a PC with only DOS: a script for DEBUG | bootstrap/dos/README.md |
| tests on other processors and computers | doc/docker.md, doc/qemu.md |
| uCForth on a new computer: platform, bootstrap VM, image, target | doc/bootstrap.md |
| where the ideas of uCForth come from | doc/origins.md |

## All builds and checks

In the folder `ucf`. A bootstrap VM builds all programs; the tests and
checks run on the bootstrap VM or on a program.

| What | Command | Time |
| --- | --- | --- |
| the reference images | `LC_ALL=C awk -f bootstrap/awk/ucf.awk -- -check` (and with `-32`) | 10 s |
| all tests | `LC_ALL=C awk -f bootstrap/awk/ucf.awk -- host/files.fs tools/test.fs </dev/null` (and with `-32`) | 13 s, 21 s |
| the fixed point (tools/README.md) | `LC_ALL=C awk -f bootstrap/awk/ucf.awk -- host/files.fs tools/genesis.fs </dev/null` | 6 min |
| `build/ucf-macos-a64` | `LC_ALL=C awk -f bootstrap/awk/ucf.awk -- -32 host/files.fs targets/macos-a64/build.fs </dev/null` | 1 min |
| `build/ucf-linux-a64` | `LC_ALL=C awk -f bootstrap/awk/ucf.awk -- -32 host/files.fs targets/linux-a64/build.fs </dev/null` | 10 s |
| `build/ucf-linux-i386` (Linux on a PC) | `LC_ALL=C awk -f bootstrap/awk/ucf.awk -- -32 host/files.fs targets/linux-i386/build.fs </dev/null` | 10 s |
| `build/ucf-linux-x64` (Linux on a PC with 64 bits) | `LC_ALL=C awk -f bootstrap/awk/ucf.awk -- -32 host/files.fs targets/linux-x64/build.fs </dev/null` | 12 s |
| `build/ucf-macos-x64` (a Mac with Intel) | `LC_ALL=C awk -f bootstrap/awk/ucf.awk -- -32 host/files.fs targets/macos-x64/build.fs </dev/null` | 1 min |
| `build/ucf-x64.efi` (a PC with UEFI: a USB stick) | `LC_ALL=C awk -f bootstrap/awk/ucf.awk -- -32 host/files.fs targets/uefi-x64/build.fs </dev/null` | 14 s |
| `build/ucf` | `LC_ALL=C awk -f bootstrap/awk/ucf.awk -- -32 host/files.fs tools/ucf.fs targets/macos-a64/build.fs </dev/null` | 80 s |
| `build/esp32s3.bin` | the command in step 6 | 75 s |
| `build/esp32.bin` (an ESP32 board) | the command in step 6, "An ESP32 board" | 75 s |
| `build/kernel8.img` (Raspberry Pi 4) | `LC_ALL=C awk -f bootstrap/awk/ucf.awk -- -32 targets/freeze.fs host/files.fs targets/rpi4/build.fs </dev/null` | 7 s |
| `build/ucf.com` (MS-DOS), `kernel.com`, `ucf.scr`, `ucf.lst` | `LC_ALL=C awk -f bootstrap/awk/ucf.awk -- host/files.fs targets/dos/build.fs </dev/null` (16-bit cells) | 12 s |
| `build/ucf-pc.img` (a PC with a BIOS: a floppy disk, a USB stick) | `LC_ALL=C awk -f bootstrap/awk/ucf.awk -- lib/package.fs lib/pc/bios.fs targets/freeze.fs host/files.fs targets/pc-bios/build.fs </dev/null` (16-bit cells) | 10 s |
| `build/ucf-win32.exe` (Windows), `ucf-win32.exe.hex` | `LC_ALL=C awk -f bootstrap/awk/ucf.awk -- -32 host/files.fs targets/win32/build.fs </dev/null` | 20 s |
| `build/ucf.exe` (the tool for Windows) | `LC_ALL=C awk -f bootstrap/awk/ucf.awk -- -32 host/files.fs tools/ucf.fs targets/win32/build.fs </dev/null` | 35 s |
| `build/ucf-a64`, `build/ucf-x64`, `build/ucf-i386` (the tool for Linux) | `LC_ALL=C awk -f bootstrap/awk/ucf.awk -- -32 host/files.fs tools/ucf.fs targets/linux-a64/build.fs </dev/null`, the same with `targets/linux-x64/build.fs` or `targets/linux-i386/build.fs` | 20 s each |
| `build/ucf-intel` (the tool for a Mac with Intel) | `LC_ALL=C awk -f bootstrap/awk/ucf.awk -- -32 host/files.fs tools/ucf.fs targets/macos-x64/build.fs </dev/null` | 80 s |
| `build/ucf-web.html` (a web browser) | `LC_ALL=C awk -f bootstrap/awk/ucf.awk -- -32 host/files.fs targets/browser/build.fs </dev/null` | 2 s |
| all tests on the Mac | `echo "ALSO FILES  INCLUDE tools/test.fs" \| build/ucf-macos-a64` | 0.1 s |
| the tests of the network (macOS, Linux) | `echo "ALSO FILES  INCLUDE lib/net/test/net.fs" \| build/ucf-macos-a64` | 1.3 s |
| the same tests over our TCP/IP, without a network | `echo "ALSO FILES  INCLUDE lib/net/test/loop.fs" \| build/ucf-macos-a64` | 1.1 s |

With the C bootstrap VM (`build/ucf-c` instead of `LC_ALL=C awk -f
bootstrap/awk/ucf.awk --`), each command takes less than half a second,
the fixed point 1.5 s; the results are the same, byte for byte. After a
build, `chmod +x` makes a program for macOS or Linux executable. The tests on the board, on Linux and on the Raspberry Pi
are in the README of each target.
