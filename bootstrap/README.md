# bootstrap: the bootstrap VMs

A *bootstrap VM* brings uCForth to a computer that has no uCForth yet.
It is the virtual machine of `doc/vm.md` (the primitives and a small HAL),
in a form that the computer can already run. There are two kinds:

- **With Genesis**, for computers with a script language or a
  compiler: bash, awk, C, Python, JavaScript, and JScript on Windows;
  later sh. Genesis is a small interpreter that compiles `forth/core.fs`
  once. From then on, uCForth compiles itself (`doc/genesis.md`).
- **Without Genesis**, for computers with only a way to put in machine
  code, such as DEBUG in MS-DOS or a monitor on a C64. This kind is
  smaller: primitives, a loader for images and the terminal. It starts
  from the seed (`seed/core16.hex`, or a seed with more primitives), an
  image that a bootstrap VM with Genesis wrote. The first file of the
  command line is then `forth/base.fs`; when it is complete, the VM
  completes the base as Genesis does (doc/genesis.md, section 6).

| Folder | Runs on | Status |
| --- | --- | --- |
| `bash/` | Linux, macOS, BSD with bash 3.2 or newer | ready |
| `awk/` | every POSIX system: the awk of macOS and BSD, mawk, gawk, BusyBox | ready |
| `c/` | every computer with a C compiler (ANSI C, C89); the reference, and the fastest | ready |
| `python/` | every computer with Python 3.6 or newer | ready |
| `js/` | a web browser (`ucf.html`), or Node.js | ready |
| `windows/` | Windows 7 to 11, with nothing to install: JScript in the Windows Script Host (`cscript`) | ready; tested on Windows 7 |
| `sh/` | dash, BusyBox ash | planned, optional |
| `dos/` | MS-DOS 2.0 or newer, FreeDOS: without Genesis; DEBUG makes `UCF.COM` from the script `ucf.scr` | ready |
| `c64/` | C64 | later |

## Start

In the folder `ucf`, the awk bootstrap VM or the bash bootstrap VM:

    LC_ALL=C awk -f bootstrap/awk/ucf.awk
    bash bootstrap/bash/ucf.sh

The prompt `ok>` comes after about 1 s (awk) or 20 to 30 s (bash). With
a C compiler, build the C bootstrap VM first; its prompt comes at once:

    cc -O2 -o build/ucf-c bootstrap/c/ucf.c
    build/ucf-c

With Python: `python3 bootstrap/python/ucf.py` (about as fast as awk).
With Node.js: `node bootstrap/js/ucf.js`. In a web browser: open
`bootstrap/js/ucf.html` and choose the folder `ucf` (`js/README.md`).
On Windows, in a command window: `cscript //nologo
bootstrap\windows\ucf-wsh.js` (`windows/README.md`).
All bootstrap VMs have the same options. The README of each one shows
how files load; `c/README.md` explains the parts of a bootstrap VM at
its program. doc/start.md shows the first steps, up to a program and a
board.

## The rule for a bootstrap VM

Inside, a bootstrap VM is free: the form of the memory, the Genesis loop
in its own language, its own tokenizer, tables or functions for the
primitives, search aids that Forth does not see.

Outside, it keeps the standard (doc/vm.md):

- cells of 16 or 32 bits, byte addresses, little endian, the memory map;
- the primitives: their numbers and what they do;
- word headers, wordlists and system variables in the memory of the VM;
- `forth/core.fs` and `forth/base.fs` without changes;
- faster words only through `?:`, as primitives with fixed numbers.

## The test for a bootstrap VM

1. Its images are the same as the reference images in `seed/`, byte for
   byte (option `-check`; without Genesis: `-dump` after the seed
   `seed/core16.hex` and `forth/base.fs` gives `seed/base16.hex`).
2. `forth/test/t0.fs` and `forth/test/hayes.fs` run with 0 errors, with 16
   and 32 bit cells.
3. `FREEZE` writes an image, and the bootstrap VM starts again from it.

## How to add a bootstrap VM

1. Read doc/bootstrap.md (the way, step by step), doc/vm.md and
   doc/genesis.md. The C bootstrap VM is the example: its README
   explains each part.
2. Make a folder with the name of the language, with the program and a
   README.md that explains each part to a reader with 1980s knowledge.
3. Pass the three tests above.
