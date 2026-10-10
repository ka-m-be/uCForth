# The awk bootstrap VM

`ucf.awk` runs uCForth on any computer with a POSIX awk: Linux, macOS,
BSD, and small Linux systems with BusyBox (Alpine, routers, rescue
systems). Each POSIX system has awk. The script uses no extensions of
awk, also no bit operations. It reads the Forth source files as text, so
no binary file is necessary.

The script is the virtual machine of `doc/vm.md`, with the Genesis of
`doc/genesis.md`. It has the same parts as the bash bootstrap VM
(`bootstrap/bash/`), with the same names. This file explains how it
works, part by part, in the order of the script.

## Start

    cd ucf
    LC_ALL=C awk -f bootstrap/awk/ucf.awk

The bootstrap VM compiles `forth/core.fs` (Genesis) and `forth/base.fs`
(the Forth interpreter `QUIT`), then the prompt `ok>` comes, after about
1 second. `LC_ALL=C` tells awk that one character is one byte (see
Traps). Options and source files come after `--`:

    LC_ALL=C awk -f bootstrap/awk/ucf.awk -- -32 forth/test/tester.fs forth/test/t0.fs

The `--` stops the options of awk itself. Without it, some versions of
awk take `-32` or `-check` as their own options.

| Option | Effect |
| --- | --- |
| `-32` | 32-bit cells (default: 16 bit) |
| `-pure` | only the 32 mandatory primitives and the HAL; the words with `?:` stay Forth words |
| `-core FILE`, `-base FILE` | other files instead of `forth/core.fs` and `forth/base.fs` |
| `-image FILE` | start from an image (written by `FREEZE` or `-dump`), without Genesis |
| `-dump-core FILE` | write the image after `core.fs`: the seed |
| `-dump FILE` | write the image after `base.fs` |
| `-check` | build with `-pure` and compare the images with `seed/` (see Tests) |

The script finds `forth/` and `seed/` in the current folder or two
folders above it. From another folder, give `-core` and `-base`.

Files and `INCLUDE` come with the package FILES:

    $ LC_ALL=C awk -f bootstrap/awk/ucf.awk -- host/files.fs
    uCForth - awk bootstrap VM - 16 bit - 163 words in FORTH - built in 1 s
    ok> INCLUDE examples/hello.fs
    Hello from uCForth

`FREEZE` saves the image to `ucf.hex` (or to the file in `$UCF_IMAGE`):

    $ LC_ALL=C awk -f bootstrap/awk/ucf.awk --
    uCForth - awk bootstrap VM - 16 bit - 163 words in FORTH - built in 1 s
    ok> : HELLO  ." Hello" CR ;
    ok> FREEZE
      saved: ucf.hex
    ok> BYE
    $ LC_ALL=C awk -f bootstrap/awk/ucf.awk -- -image ucf.hex
    uCForth - awk bootstrap VM - 16 bit - 164 words in FORTH - image ucf.hex
    ok> HELLO
    Hello

The images are the same as the images of the other bootstrap VMs (bash,
C, Python, JavaScript), and each of them starts the images of the
others.

## How it works

### Memory

The memory of the VM is the awk array `M`. The index is the byte address
of a cell, and the element holds the cell: `M[a]` is the cell at the
address a (a is a multiple of 2, or of 4 with 32-bit cells). At the
start, each cell gets the number 0. So each element is a number, and a
comparison such as `M[sp] == 0` compares numbers, not texts.

A byte is a part of its cell. `C@` divides the cell by 256, 65536 or
16777216 and keeps the remainder of a division by 256. `C!` adds the
difference between the new and the old byte, multiplied with the same
power of 256. The low byte has the lowest address (little endian).

An awk array is a hash table: an access takes the same time, whatever
the size of the memory is. This is why awk is much faster than bash
here.

### Numbers without bit operations

awk has only one type of number: floating point with 53 bits. So
integers up to 2^53 are exact. A cell is always a number from 0 to
2^16 - 1 (or 2^32 - 1). Each primitive keeps this range: `+` subtracts
2^16 when the sum is too big, `-` adds 2^16 when the difference is
negative.

POSIX awk has no `and()`, `or()` or `xor()`. The script has a table
`AND8` with x AND y for all pairs of bytes (65536 elements). `AND` of two
cells takes the bytes one by one from the table. `OR` and `XOR` come
from `AND`:

    x OR y  = x + y - (x AND y)
    x XOR y = x + y - 2 * (x AND y)

Example: 12 OR 10. 12 AND 10 is 8, so 12 OR 10 = 12 + 10 - 8 = 14.

`UM*` with 32-bit cells gives a product with 64 bits, too much for
53 bits. The function `mul` splits each cell into two halves of 16 bits
and adds the part products. `UM/MOD` with 32-bit cells divides in two
steps of 16 bits. No value becomes bigger than 2^48.

### The virtual machine

The function `vm` is NEXT and the choice of the primitive:

    w = M[ip]; ip += C; op = M[w]

awk has no `switch`. So the tests of `op` form a tree: first a test for
groups of 16 primitives, then for groups of 4, then single numbers. Each
primitive needs at most 12 tests, not 60. The primitives with more code
(`UM/MOD`, `MOVE`, `FIND-NAME`, the console, the HAL and the files) are
functions. The loop ends at primitive 127, HALT: then the VM gives
control back to the script.

A primitive that finds an error (for example a division by 0) pushes the
error code, puts the xt of `THROW` into two free cells at the top of the
memory and continues there, as a kernel does.

### Search in the dictionary

`FIND-NAME` (primitive 58) is awk code: it goes through the search order
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
the file and the line number. Before it waits for the keyboard, it sends
all output (`fflush`).

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

A normal `getline` does not tell if the last line of a file has an LF.
But Genesis, `READ-FILE` and the tests must know it. So the function
`slurp` reads a file in one piece: it sets the record separator `RS` to
the byte 1, which a text file does not hold. Then `split` cuts the text
at each LF. The text after the last LF is one more part, also when it is
empty (`doc/genesis.md`, section 2).

The HAL words for files (`doc/vm.md`, section 8) use the numbers 10 to
29 as fid. `OPEN-FILE` reads the whole file into the parts
`FP[fid, 1 ...]`. `READ-LINE` gives one part, `READ-FILE` gives the bytes
of the parts with an LF between them. `CREATE-FILE` makes an empty file,
and `WRITE-FILE` appends to it with `printf ... >>`.

awk stops with an error when it cannot open a file for output. So
`CREATE-FILE` and `SAVE-IMAGE` first let the shell make the file:
`system("2>/dev/null : > 'name'")`. `:` is a command of the shell itself,
so no other program is necessary. The status tells if it worked.

`(BYE)` ends awk with an exit status.

### Images

An image is a text file with Intel HEX records (`doc/vm.md`, section 10).
`write_image` writes it, `hexread` reads it again: into the memory
(`-image`) or to compare it with the memory (`-check`).

## Tests

    LC_ALL=C awk -f bootstrap/awk/ucf.awk -- -check
    LC_ALL=C awk -f bootstrap/awk/ucf.awk -- -32 -check

Each command builds with `-pure` and compares the seed and the base image
with `seed/`. Expected:

      check core16: the same as seed/core16.hex
      check base16: the same as seed/base16.hex

Then all tests of the words, with 16 and 32 bit (`-32`):

    LC_ALL=C awk -f bootstrap/awk/ucf.awk -- host/files.fs tools/test.fs < /dev/null

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

Tested (2026) with these versions of awk:

| awk | System | `-check` | all tests |
| --- | --- | --- | --- |
| awk 20200816 (the "one true awk") | macOS 26, Apple M4 | 8 s | 5 s |
| mawk 1.3.4 | Debian 12, arm64 (Docker) | 2 s | 1 s |
| mawk 1.3.4 of 2025 (its clock: see "Traps") | Debian 13, arm64 (Docker) | 2 s | 3 s |
| gawk 5.2.1, also with `--posix` | Debian 12, arm64 (Docker) | 3 s | 3 s |
| BusyBox awk 1.37 | Alpine Linux, arm64 (Docker) | 23 s | 13 s |

Debian and Raspberry Pi OS have mawk as `awk`.

## Speed

Measured on an Apple M4 (2026), with the awk of macOS:

| Task | Time |
| --- | --- |
| compile `core.fs` and `base.fs` | 1 s |
| the same with `-pure` | 8 s |
| all tests | 5 s |
| all tests with `-pure` | 2 min |

The VM does about 2 million steps per second with the awk of macOS, and
more with mawk. The bash bootstrap VM does 40 000 to 85 000.

## Traps in awk

For the next bootstrap VM, and for changes to this one:

- **Locale:** An awk that knows UTF-8 (gawk, newer versions of the one
  true awk) counts characters, not bytes, if the locale is UTF-8. Then
  `length`, `substr` and `printf "%c"` are wrong for bytes from 128 on.
  `LC_ALL=C` makes one character one byte. The script shows a warning
  if the locale is not right.
- A statement outside of `BEGIN` or a function is a *pattern* for the
  input lines. The script reads no input lines in this way, so such a
  statement never runs. All variables start in `BEGIN` or with 0 and "".
- `next` is a statement of awk; it cannot be the name of a variable.
- A name is an array or a number, not both, also as a local variable.
  The awk of macOS and BusyBox do not see the fault; mawk and gawk stop.
- An element that awk does not know yet is empty. If it is copied, the
  copy can be the text "", and `"" == 0` is false. So all cells get the
  number 0 at the start.
- `getline` does not tell if the last line of a file had an LF. The
  script reads a file in one piece (see Files).
- A redirection that fails (`printf ... > "file"`) stops awk with an
  error. The script tests the file first with the shell.
- In `: > file 2>/dev/null`, the shell opens `file` before it redirects
  the errors. So the error message comes out. `2>/dev/null` goes first.
- awk has no function for the time. `srand()` returns the seed before
  it, and without an argument it takes the time as the new seed:
  `srand(); t = srand()` gives the time. Most awks give it in seconds.
  mawk 1.3.4 of 2025 (Debian 13) gives it in nanoseconds, and on a
  system without that clock in microseconds. So the script divides the
  number by 1000 until it is a time in seconds (less than 10^11).
- `-32` and `-check` after `-f file` can be options for awk: mawk stops
  with "not an option: -32". `--` stops the options of awk.

## Limits

- `?RX` waits for a complete line: awk reads lines, not single keys.
- The bootstrap VM does not check the stacks and the addresses, as a
  kernel on a board does not.
- Most awks have only a clock with seconds. `MS` does not wait, and
  `MILLIS` counts the whole seconds plus the times of `MS`. With a mawk
  that gives nanoseconds, `MILLIS` counts real milliseconds.
- `REBOOT` starts `BOOT` or `QUIT` again, but does not load the image
  again. `ERASE-IMAGE` does nothing.
- Text files only. `OPEN-FILE` with `W/O` or `R/W` writes at the end of
  the file, not at its start. Two fids for the same file name share one
  output stream of awk.
