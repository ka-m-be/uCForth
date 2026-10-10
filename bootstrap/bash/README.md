# The bash bootstrap VM

`ucf.sh` runs uCForth on any computer with bash 3.2 or newer: Linux,
macOS, BSD. It needs no other program, only the builtin commands of bash.
It reads the Forth source files as text, so no binary file is necessary.

The script is the virtual machine of `doc/vm.md`, with the Genesis of
`doc/genesis.md`. This file explains how it works, part by part, in the
order of the script.

## Start

    cd ucf
    bash bootstrap/bash/ucf.sh

The bootstrap VM compiles `forth/core.fs` (Genesis) and `forth/base.fs`
(the Forth interpreter `QUIT`), then the prompt `ok>` comes. Source files
on the command line load before the keyboard:

    bash bootstrap/bash/ucf.sh forth/test/tester.fs forth/test/t0.fs

| Option | Effect |
| --- | --- |
| `-32` | 32-bit cells (default: 16 bit) |
| `-pure` | only the 32 mandatory primitives and the HAL; the words with `?:` stay Forth words |
| `-core FILE`, `-base FILE` | other files instead of `forth/core.fs` and `forth/base.fs` |
| `-image FILE` | start from an image (written by `FREEZE` or `-dump`), without Genesis |
| `-dump-core FILE` | write the image after `core.fs`: the seed |
| `-dump FILE` | write the image after `base.fs` |
| `-check` | build with `-pure` and compare the images with `seed/` (see Tests) |

Files and `INCLUDE` come with the package FILES:

    $ bash bootstrap/bash/ucf.sh host/files.fs
    uCForth - bash bootstrap VM - 16 bit - 163 words in FORTH - built in 20 s
    ok> INCLUDE examples/hello.fs
    Hello from uCForth

`FREEZE` saves the image to `ucf.hex` (or to the file in `$UCF_IMAGE`):

    $ bash bootstrap/bash/ucf.sh
    uCForth - bash bootstrap VM - 16 bit - 163 words in FORTH - built in 20 s
    ok> : HELLO  ." Hello" CR ;
    ok> FREEZE
      saved: ucf.hex
    ok> BYE
    $ bash bootstrap/bash/ucf.sh -image ucf.hex
    uCForth - bash bootstrap VM - 16 bit - 164 words in FORTH - image ucf.hex
    ok> HELLO
    Hello

On macOS, `/bin/bash` is bash 3.2. A bash from another source (for
example MacPorts or Homebrew) can be first in the path; both work.

## How it works

### Memory

The memory of the VM is the bash array `M`. One element holds one cell:
`M[a >> S]` is the cell at the byte address a (S is 1 for 16-bit cells, 2
for 32 bit). A byte is a part of its cell: `C@` shifts the cell to the
right and keeps 8 bits, `C!` changes only these 8 bits. The low byte has
the lowest address (little endian). An element that bash does not know
yet counts as 0.

The addresses are the addresses of `doc/vm.md`, section 3. The data stack
and the return stack are in this memory, so `SP@` and `RP@` give real
addresses.

### Primitives

Most primitives are one arithmetic expression each, in the table `P`.
Bash computes an expression with `(( ))`. Example, `DUP`:

    P[13]='sp -= C, M[sp>>S] = M[(sp+C)>>S]'

`sp` is the register SP, `C` the cell size in bytes. The expression makes
room on the stack and copies the old top cell into it. The names of the
temporary values start with `p` (`pt`, `pa` …), so they do not disturb
other parts of the script.

The expressions have no branches: no `&&`, `||` or `?:`. A condition
gives 0 or 1, and the result multiplies with it. `=` is:

    P[42]='pt = M[sp>>S], sp += C, M[sp>>S] = (M[sp>>S] == pt) * MASK'

The reason is a fault in bash 3.2 (see Traps).

About 20 primitives need more than one expression: `EXECUTE`, the
console (`?RX`, `TX!`), `UM/MOD`, `MOVE`, `FILL`, `FIND-NAME`, the HAL
and the host primitives of Genesis. The array `SPEC` marks them, and the
virtual machine runs bash code for them.

### The virtual machine

The function `vm` is NEXT and the choice of the primitive:

    if (( w = M[ip>>S], ip += C, op = M[w>>S], op > 127 || SPEC[op] )); then
      ... bash code for EXECUTE, ?RX, TX!, UM/MOD, HALT ...
    else
      (( ${P[op]} ))
    fi

The first line fetches the next xt into W, moves IP, and fetches the
code field. Then bash computes the expression of the primitive. The loop
ends at primitive 127, HALT: then the VM gives control back to bash.

A primitive that finds an error (for example a division by 0) pushes the
error code and runs the Forth word `THROW`, as a kernel does.

### Search in the dictionary

`FIND-NAME` (primitive 58) is bash code: it goes through the search order
and each wordlist, and compares the length and then the characters of
each name. Genesis uses the same functions to find words in `core.fs`.

### The terminal

`?RX` gives the characters of one line, then LF. Where the next line
comes from:

1. from the files: `base.fs`, then the files of the command line;
2. then from the keyboard, or from a pipe.

The bootstrap VM reads a new line only when `QUIT` waits in `REFILL`
(the return stack holds a return address into `REFILL`). If another word
asks for a key while files load, `?RX` answers "no key".

`TX!` collects the characters of an output line and shows the line at
LF. So the bootstrap VM can hide the prompts of `QUIT` while a file
loads. A line with `error:` stops the loading, and the bootstrap VM shows
the file and the line number.

### Genesis

Genesis compiles `forth/core.fs`, as `doc/genesis.md` tells. In the
script:

| Function | Task |
| --- | --- |
| `genesis` | the start dictionary, the loop over the lines, the end |
| `gline` | one line: copy it into TIB, take the words, find, execute, compile |
| `gnumber` | convert a word into a number |
| `gheader` | the host primitive `HEADER` |
| `gexec` | execute one word, through `CATCH` when `core.fs` has it |

### Files

The HAL words for files (`doc/vm.md`, section 8) use the file
descriptors 10 to 29 of bash; the fid is the number of the descriptor.
`READ-LINE` and `READ-FILE` read a file line by line with `read` and keep
the rest of the current line. So the bootstrap VM reads only text files;
bash cannot hold the byte 0 in a string. `WRITE-FILE` writes each byte
as an octal escape with `printf`. `(BYE)` ends bash with an exit status.

### Images

An image is a text file with Intel HEX records (`doc/vm.md`, section 10).
`write_image` writes it, `hexread` reads it again: into the memory
(`-image`) or to compare it with the memory (`-check`).

## Tests

    bash bootstrap/bash/ucf.sh -check
    bash bootstrap/bash/ucf.sh -32 -check

Each command builds with `-pure` and compares the seed and the base image
with `seed/`. Expected:

      check core16: the same as seed/core16.hex
      check base16: the same as seed/base16.hex

Then all tests of the words, with 16 and 32 bit (`-32`):

    bash bootstrap/bash/ucf.sh host/files.fs tools/test.fs < /dev/null

Expected, and the exit status 0:

    t0: 183 tests, 0 errors
    hayes: 872 tests, 0 errors
    files: 26 tests, 0 errors
    ihex: 11 tests, 0 errors
    vm: 9 tests, 0 errors
    fbcon: 14 tests, 0 errors
    text: 10 tests, 0 errors
    package: 8 tests, 0 errors
    slip: 6 tests, 0 errors
    all: 2742 tests, 0 errors

Between these lines, some tests show messages such as `redefined HD-A`
or `cannot open host/test/none.tmp`: the tests cause them on purpose.

## Speed

Measured on an Apple M4 (2026):

| Task | bash 3.2 | bash 5.2 (x86, Rosetta) |
| --- | --- | --- |
| compile `core.fs` and `base.fs` | 16 s | 25 s |
| the same with `-pure` | 2.5 min | 3.5 min |
| compile, then `t0.fs` and `hayes.fs` | 3 min | 4 min |

The VM does 40 000 to 85 000 steps per second. Bash arrays are linked
lists, so an access to a far element takes longer when the memory has
many cells. The awk bootstrap VM is 15 to 30 times faster, the C
bootstrap VM several thousand times (bootstrap/c/README.md).

## Traps in bash

For the next bootstrap VM, and for changes to this one:

- **bash 3.2** does an assignment to an array element also in the branch
  of `&&`, `||` or `?:` that it must skip, when that branch reads the
  array. So the primitives have no branches.
- **bash 3.2** gives negative codes for bytes from 128 on
  (`printf '%d' "'…"`). The script masks them with 255.
- **bash 3.2** has no clock with milliseconds (`EPOCHREALTIME` comes with
  bash 5). There `MS` does not wait, and `MILLIS` counts the whole
  seconds plus the times of `MS`.
- **bash 3.2** ends `$( case … ) … )` too early; use a function.
- `${…}` inside `(( ))` is replaced before the computation starts. It
  sees the old value of a variable that the same expression changes.
- In an associative array, bash takes the index inside `(( ))` as text.
  So the memory is a normal array.
- Bash keeps its own copies of file descriptors from 10 on while a
  redirection runs. An `exec 10<file` inside `{ …; } 2>/dev/null` is then
  lost (bash 5.2) or becomes a copy of the input (bash 3.2). The script
  tests the file first and runs `exec` without another redirection.
- `echo` in some shells (dash) changes `\` in the data. The script uses
  only `printf`.
- A file can end without LF. `read` then returns false, but the text is
  there.

## Limits

- `?RX` waits for a complete line: bash reads lines, not single keys.
- The bootstrap VM does not check the stacks and the addresses, as a
  kernel on a board does not.
- `REBOOT` starts `BOOT` or `QUIT` again, but does not load the image
  again. `ERASE-IMAGE` does nothing.
