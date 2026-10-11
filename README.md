# uCForth (ucf)

A small Forth for microcontrollers, and a way to build it from nothing.

The idea: How can you make a usable programming system for microcontrollers
or small computers, when you only have a minimal hardware and no real programming
tools? Say: A small laptop with DOS, Windows, Linux shell or a 1980s home
computer like C64. Plus wits, knowledge and (paper?) documentation to slowly build everything from scratch.

uCForth is an approach to this. The goal is to explore and understand the basics
of computing and hardware, like people in the early days of personal computing did.
Starting from a blinking cursor up to programming today's technology.

Task number one is to get yourself a basic programming environment for Forth.
Next, build a real executable on your programming machine. Finally use your
machine to program some bare metal microcontroller in uCForth. This might be a
modern 32bit SoC (system on chip, e.g. ESP32) or older 8 bit or 16 bit chips.

uCForth runs on a small virtual machine (or virtual CPU). Only 32 primitives and a small
HAL (hardware abstraction layer) depend on the computer. Everything else
is Forth: the interpreter, the compiler, the drivers and the tools.

To bring uCForth to a computer, you need a *bootstrap VM*: a program in
a language that the computer already has, for example bash, awk,
Python, or JavaScript in a web browser; with a C compiler, also C. On a
PC with only DOS, the program DEBUG makes it from a text file. The
bootstrap VM supplies the primitives and reads `forth/core.fs` once.
From then on, uCForth compiles itself. No compiler, no download, no
binary file: only text files.

## More information

Background about the design, Forth basics, the VM concept, etc. is given in `doc/`.
See the `README.md` there.

## Start, try out and using

All commands run in the top folder of this repository.

**On Linux or macOS with bash:**

    bash bootstrap/bash/ucf.sh

The bootstrap VM compiles `forth/core.fs` and `forth/base.fs` (about 20
seconds on a fast computer). Then the prompt `ok>` comes. Try:

    ok> 3 4 + .
    7
    ok> : SQUARE ( n -- n*n )  DUP * ;
    ok> 12 SQUARE .
    144
    ok> BYE

**With awk (every POSIX system, also BusyBox)**, the prompt comes after about
1 second:

    LC_ALL=C awk -f bootstrap/awk/ucf.awk

**With a C compiler**, the C bootstrap VM gives the prompt at once. It is
the fastest, and the reference for all bootstrap VMs:

    cc -O2 -o build/ucf-c bootstrap/c/ucf.c
    build/ucf-c

**With Python:** `python3 bootstrap/python/ucf.py`.

**With Node.js:** `node bootstrap/js/ucf.js`.

**In a web browser**, without any installation: open `bootstrap/js/ucf.html`,
choose the folder `ucf`, then "Start".

(There is a prebuilt GitHub page: https://ka-m-be.github.io/uCForth/build/ucf-web.html)

**On Windows** (7 to 11), without any installation, in a command
window in the folder `ucf`:

    cscript //nologo bootstrap\windows\ucf-wsh.js

**On a PC with MS-DOS** (or FreeDOS), DEBUG makes the program from the
script `bootstrap/dos/ucf.scr`; it starts from the seed:

    C:\UCF>DEBUG < UCF.SCR
    C:\UCF>UCF -image SEED.HEX FORTH\BASE.FS

`bootstrap/dos/README.md` shows the way, from the files to the tests.

`doc/start.md` shows the next steps: files, tests, a program for your
computer, and uCForth on a board. `doc/forth.md` is for readers who are
new to Forth. `doc/bootstrap.md` shows how to bring uCForth to a new
computer.

## The tree

| Folder | Contents |
| --- | --- |
| `doc/` | Manuals: first steps (`start.md`), Forth basics (`forth.md`), all words (`words.md`), a new computer (`bootstrap.md`), the virtual machine (`vm.md`), Genesis (`genesis.md`), the sources of the ideas (`origins.md`) |
| `forth/` | The portable system: `core.fs`, `base.fs` and their tests |
| `seed/` | Reference images: a bootstrap VM must build the same, byte for byte |
| `bootstrap/` | Bootstrap VMs: bash, awk, C, Python, JavaScript (also in a web browser), JScript for Windows, MS-DOS (a script for DEBUG); later sh, C64 |
| `lib/` | Packages for every system |
| `host/` | Packages that need an operating system: files, serial lines |
| `asm/` | Assemblers in Forth |
| `kernels/` | Virtual machines in machine code, built with `asm/` |
| `targets/` | uCForth as an executable binary or boot image for various platforms, or as one HTML file for a web browser |
| `tools/` | Applications on the host |
| `examples/` | Small applications |
| `apps/` | Applications for a board, loaded and started by `ucf term` in one line |

Each folder has a README.md: what is in it, and how to add something new.

## Status

The bootstrap VMs in bash, awk, C, Python, JavaScript and JScript
(Windows) are ready, and the one for MS-DOS (a script for DEBUG).
They build the same images as the reference, and all tests pass
(`forth/test/`). The other folders fill up
stage by stage.

## License

MIT, see LICENSE.
