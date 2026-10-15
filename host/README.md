# host: packages that need an operating system

Files, serial lines, images, and the terminal to a board. They run on a
host (a bootstrap VM or a native program), not on a board.

| File | Contents |
| --- | --- |
| `files.fs` | package FILES: the HAL words for files, `INCLUDE`, `INCLUDED`, `INCLUDE-FILE`, `(BYE)` |
| `image.fs` | package IMAGE: read the image `ucf.hex`, write a binary file (for `targets/*/build.fs`); the vector `ON-PUT` sees each byte (for a signature) |
| `send.fs` | package SEND: send source text to a target over a serial line, line by line after each prompt |
| `serial.fs` | package SERIAL: the serial port of the host (`OPEN-PORT`, `SEND`, `RECV`, `DTR!`, `RTS!`); needs the HAL primitives 94-96 (`targets/macos-a64`, `targets/macos-x64`, `targets/win32`, `targets/linux-a64`, `targets/linux-x64`, `targets/linux-i386`) and the package TEXT |
| `term.fs` | package TERM: the console to uCForth on a board (`TERMINAL`; `TERMINAL-INCLUDE` sends a file first), with `#include`, `#require`, `#when`, `#reset`; needs FILES, TEXT, SERIAL |
| `test/files.fs` | its tests |

The packages for the ROM bootloader of the ESP32 (ESPROM, ESPIMG) run
on the host too; they are with the other ESP32 packages in `lib/esp32/`.

## Load a package

A program starts with only FORTH in the search order. The programs
`build/ucf-macos-a64` and `build/ucf-linux-a64` have the package FILES
in them; a bootstrap VM loads it from the command line
(`host/files.fs`). `ALSO FILES` puts FILES into the search order, then
`INCLUDE` loads a file. After the load, the new package is in the
search order too:

    $ build/ucf-macos-a64
    ok> ORDER
    FORTH   current: FORTH
    ok> ALSO FILES  INCLUDE lib/text.fs
    ok> ORDER
    TEXT FILES FORTH   current: FORTH

The words for the search order:

| Word | Effect |
| --- | --- |
| `ORDER` | the search order, the first vocabulary on the left; after `current:` the vocabulary for new words |
| `VOCS` | all vocabularies, the newest first |
| `WORDS` | the words of the first vocabulary in the search order |
| `ALSO name` | put the vocabulary name first into the search order |
| `PREVIOUS` | take the first vocabulary out of the search order |
| `ONLY FORTH` | only FORTH, as after the start |
| `DEFINITIONS` | new words go into the first vocabulary |

A package has two vocabularies: `name` for its words, and `name.I`
for its helpers, `PROTECTED` and `PRIVATE` (lib/README.md; with
`TEMP-HEADS ON!` the `PRIVATE` words keep no names):

    ok> VOCS
    TEXT.I TEXT PACKAGES.I FILES.I FILES INTERNALS FORTH
    ok> WORDS

    FAIL N>S +PLACE PLACE SPLIT -TRAILING -LEADING S= /STRING 2SWAP
    ok> ONLY FORTH  ORDER
    FORTH   current: FORTH

## The console to a board: term

The package TERM connects the host to uCForth on a board over a serial
line. On a Mac, a board with USB is `/dev/cu.usbmodem...` or
`/dev/cu.usbserial-...`; `ls /dev/cu.*` shows the ports. On Linux it is
`/dev/ttyACM0` or `/dev/ttyUSB0`, on Windows `COM3` or similar. The
programs for macOS, Windows and Linux have serial lines (HAL 94-96);
the bootstrap VMs do not.

The tool `ucf` (tools/README.md) has TERM in it. It starts the console
with one command:

    $ build/ucf term /dev/cu.usbmodem5C4C0505931
    term on /dev/cu.usbmodem5C4C0505931: #help shows the commands

    board ok> 3 4 + .
    7
    board ok> #bye
    term ends: back on the host

The board waits at its prompt already, so no prompt comes at first:
press the return key, and the board answers with its prompt. After a
restart, the board sends its start messages and the prompt itself.

With a file after the port, `term` first sends the file (as `#include`
does), then it is the console. So an application starts in one line
(apps/README.md):

    $ build/ucf term /dev/cu.usbmodem5C4C0505931 apps/aircon/app.fs
    term on /dev/cu.usbmodem5C4C0505931: #help shows the commands
    aircon: ready; AIRCON-HELP shows the commands
    board ok>

From the prompt of the host, two lines do the same: the first loads
TERM and the packages that it needs (TEXT, SERIAL), the second opens
the port and starts the console:

    $ build/ucf-macos-a64
    ok> ALSO FILES  INCLUDE lib/text.fs  INCLUDE host/serial.fs  INCLUDE host/term.fs
    ok> S" /dev/cu.usbmodem5C4C0505931" OPEN-PORT  TERMINAL
    term on /dev/cu.usbmodem5C4C0505931: #help shows the commands

    board ok> 3 4 + .
    7
    board ok> #bye
    term ends: back on the host
    ok> ORDER
    TERM SERIAL TEXT FILES FORTH   current: FORTH

In `build/ucf` without a command, the packages are there already:

    $ build/ucf
    ucf: the host Forth; BYE ends it
    ok> ALSO SERIAL  ALSO TERM  S" /dev/cu.usbmodem5C4C0505931" OPEN-PORT  TERMINAL
    term on /dev/cu.usbmodem5C4C0505931: #help shows the commands

**The prompts.** `board ok>` is the prompt of the board (`board c>`
in compile state): each line that you type goes to the board. `ok>`
without `board` is the prompt of the host. The board itself sends
`ok>`; `term` puts `board` in front of it.

**The commands of term** start with `#`. They stay on the host:

| Command | Effect |
| --- | --- |
| `#include FILE` | send the file, a line after each prompt; stop at an error and name the file and the line |
| `#require WORD FILE` | the same, if the board does not know WORD |
| `#when WORD FILE` | the same, if the board knows WORD |
| `#reset` | restart the board over RTS; a line that still runs (`FREEZE`) ends first, and the next line waits for the prompt after the start (each at most 10 s) |
| `#help` | the commands |
| `#bye` | end term |
| Ctrl-C | stop the word that runs on the board (and a file that loads); also while `#reset` waits for the prompt, so it reaches the second of `autostart in 1 s` |

A file name after `#include` in a file is relative to that file. Files
load files up to 8 deep.
`#require` and `#when` ask the board with `[DEFINED]`: the word must be
in the search order of the board. The board starts with only FORTH in
its search order, as the host does (`targets/esp32s3/README.md`); the
names of the packages are in FORTH.

`#when` lets one file choose the file for the chip of the board. Each
ESP32 board has the package of its chip in its image (`ESP32S3` or
`ESP32`), so `lib/esp32/rmt.fs` holds only these two lines:

    #when ESP32S3 esp32s3/rmt.fs
    #when ESP32 esp32/rmt.fs

An application loads `lib/esp32/rmt.fs` and gets the package RMT of its
chip (`apps/aircon/app.fs`).
