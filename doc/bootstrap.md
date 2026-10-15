# From nothing to uCForth on a new computer

This manual shows the way to bring uCForth to a computer that has no
uCForth yet, and from there to a new target. The way has four steps:

| Step | Question | Result | Folder |
| --- | --- | --- | --- |
| 1 The platform | What does the computer have? | a language that runs there | - |
| 2 A bootstrap VM | Can uCForth run in that language? | the prompt `ok>`, files, all tests | `bootstrap/` |
| 3 An image | Can uCForth save itself? | a file with the running system | `seed/`, `ucf.hex` |
| 4 A target image | Can it run without the bootstrap VM? | a program file or a boot image | `targets/` |

Steps 1 and 2 give you uCForth on the computer. Step 3 keeps what you
build. Step 4 makes a program in machine code for the computer itself,
or for another one, such as a board.

`doc/start.md` shows the way for a computer that is already done
(Linux, macOS). This manual is for a new one.

## 1 The platform

Look for a language that runs on the computer, without anything else:

| The computer has | Then | Example |
| --- | --- | --- |
| a shell or awk (each Unix, also BusyBox) | a bootstrap VM in that language | `bootstrap/bash/`, `bootstrap/awk/` |
| a C compiler | a bootstrap VM in C, the fastest | `bootstrap/c/` |
| Python | a bootstrap VM in Python | `bootstrap/python/` |
| a web browser | a bootstrap VM in JavaScript, in a web page | `bootstrap/js/` |
| Windows | a bootstrap VM in JScript, for the Windows Script Host of each Windows | `bootstrap/windows/` |
| only a way to put in bytes (`DEBUG` in MS-DOS, a monitor) | a small bootstrap VM without Genesis: `DEBUG` makes it from a text file | `bootstrap/dos/`; planned: C64 |

A bootstrap VM **with Genesis** reads `forth/core.fs` as text: it needs
a language with strings and files. A bootstrap VM **without Genesis**
is smaller: it loads the seed, an image that another bootstrap VM made
(`seed/core16.hex`), and then compiles `forth/base.fs` itself
(`bootstrap/README.md`).

## 2 A bootstrap VM

Read `doc/vm.md` (the contract) and `doc/genesis.md` (how Genesis
compiles `core.fs`). Then take `bootstrap/c/ucf.c` as the pattern: its
parts come in the order below, and its README explains each one. Write
the parts in this order, and test each one as soon as it is there:

1. **Memory.** An array of bytes, 64 KB (16-bit cells) or 256 KB (32
   bit). A cell is 2 or 4 bytes, the low byte first. Two functions read
   and write a cell (`get`, `put`). Keep each value in the range of a
   cell after each operation.
2. **The memory map** (`doc/vm.md`, section 3): the addresses of the
   system variables, TIB, the stacks, the dictionary, the HALT cells.
3. **NEXT and the 32 mandatory primitives** (`doc/vm.md`, sections 5
   and 7). Then, for speed, the optional primitives 32 to 65. Each one
   must do exactly what its Forth version in `core.fs` or `base.fs` does.
4. **The HAL** (section 8): the clock, `BYE`, `SAVE-IMAGE`; the files
   (89 to 93, 98, 99) for a host.
5. **The terminal** (section 9): `?RX` gives the lines of the files,
   then of the keyboard; a new line only when `QUIT` waits in `REFILL`.
   `TX!` collects an output line: so the prompts stay hidden while a
   file loads, and a line with `error:` stops the loading.
6. **Genesis** (`doc/genesis.md`): the headers of the primitives, the
   constants, the loop over the lines of `core.fs`, `?:`, the numbers,
   `CATCH`, and at the end the removal of the host primitives.
7. **Images** (`doc/vm.md`, section 10): write and read Intel HEX; the
   options `-dump-core`, `-dump`, `-image` and `-check`.

**The tests,** in this order; each one needs the one before:

| Test | Command (C bootstrap VM as the example) | Expected |
| --- | --- | --- |
| the images | `build/ucf-c -check` and `-32 -check` | four times "the same" |
| all words | `build/ucf-c host/files.fs tools/test.fs </dev/null` (and `-32`) | `all: 2882 tests, 0 errors` (`12210` with `-32`) |
| an image again | `FREEZE`, then `-image ucf.hex` | the new words are there |
| the images of the others | `-image` with an image of another bootstrap VM | it starts |
| the fixed point | `host/files.fs tools/genesis.fs </dev/null` | twice "the same" |
| the targets | build a target (`targets/*/README.md`) and compare it with the file from another bootstrap VM (`cmp`) | the same, byte for byte |

**What helped with five bootstrap VMs:**

- `-check` first. If a byte differs, the message names the first
  address; the dump of the other bootstrap VM (`-pure -dump-core`)
  shows which header or word it is.
- Compare the output of `tools/test.fs` with the output of another
  bootstrap VM, line for line (`diff`). Only the first line (the
  banner) may differ.
- The traps of a language are in the README of each bootstrap VM: awk
  has no bit operations and counts characters, not bytes (`LC_ALL=C`);
  bash is slow; C89 has no type with 64 bits for sure (`UM*`, `UM/MOD`
  in halves); Python calls are slow; the bit operations of JavaScript
  take 32 bits with a sign, and a web page must not wait (the VM runs
  in slices); JScript on Windows is ECMAScript 3 of 1999, without bytes
  and slow with global variables.
- Write the README while you write the program: a reader with the
  knowledge of a 1980s handbook must understand each part.

## 3 An image

An image is the running system in a text file: a first line with the
cell size and the start words, then Intel HEX records (`doc/vm.md`,
section 10). There are three kinds:

| Image | Made by | Starts with |
| --- | --- | --- |
| the seed (`seed/core16.hex`) | `-pure -dump-core`: Genesis is complete, `core.fs` only | `QUIT` |
| the base image (`seed/base16.hex`) | `-pure -dump`: `base.fs` is complete | `BOOT` |
| your system (`ucf.hex`) | `FREEZE` at the prompt, with your words and packages | `BOOT` |

Each bootstrap VM starts each image with `-image`, whatever bootstrap VM
made it. A bootstrap VM without Genesis starts from the seed.

## 4 A target image

A target image runs without a bootstrap VM: the virtual machine is
machine code, and the image is in the same file. `targets/README.md`
has the steps; in short:

1. **An assembler** in Forth for the processor (`asm/`), with tests.
2. **A kernel**: the virtual machine in machine code, written with the
   assembler (`kernels/`): NEXT, the primitives.
3. **The HAL of the target** (`targets/NAME/hal.fs`): the start, the
   console, the clock; as small as possible. Drivers are Forth.
4. **The build** (`targets/NAME/build.fs`): `FREEZE` the running
   system as the image, assemble the code, write the file in the format
   of the platform (ELF, Mach-O, a boot image for a chip).
5. **The tests** on the target: `forth/test/t0.fs` and `hayes.fs`, over
   the console or a serial line.

A bootstrap VM on any host builds each target; the C bootstrap VM does
it in less than half a second, awk in a minute. The file is the same
from each bootstrap VM (tested with awk, C, Python and JavaScript). So a
computer with only a web browser can build the image for a board.

| Target | Processor | File format | README |
| --- | --- | --- | --- |
| Linux | aarch64 | ELF program | `targets/linux-a64/README.md` |
| Linux | x86, 32 bit (also on x86-64) | ELF program | `targets/linux-i386/README.md` |
| Linux | x86-64 | ELF program | `targets/linux-x64/README.md` |
| macOS | aarch64 (Apple silicon) | Mach-O program with a code signature | `targets/macos-a64/README.md` |
| macOS | x86-64 (Intel) | Mach-O program with a code signature | `targets/macos-x64/README.md` |
| Raspberry Pi 4 | aarch64, without an operating system | `kernel8.img` | `targets/rpi4/README.md` |
| ESP32-S3 | Xtensa LX7 | image of the ROM bootloader | `targets/esp32s3/README.md` |
| ESP32 | Xtensa LX6 | image of the ROM bootloader, at `$1000` in the flash | `targets/esp32/README.md` |
| MS-DOS | 8086 (real mode) | `.COM` program; the kernel alone also as a script for DEBUG | `targets/dos/README.md` |
| a PC with a BIOS, without an operating system | 8086 (real mode) | a disk: a boot sector, the code, the memory of the VM; also a floppy disk with FAT12 | `targets/pc-bios/README.md` |
| Windows | x86, 32 bit | PE32 program, also as hex text for `certutil` | `targets/win32/README.md` |
| a PC with UEFI, without an operating system | x86-64 | PE32+ program (an EFI application) for a USB stick with FAT32 | `targets/uefi-x64/README.md` |
| a web browser | JavaScript (the JavaScript bootstrap VM) | one HTML file with the VM and the image | `targets/browser/README.md` |
