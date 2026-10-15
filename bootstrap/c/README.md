# The C bootstrap VM

`ucf.c` runs uCForth on any computer with a C compiler: Linux, macOS,
BSD, Windows, and older systems. It is ANSI C (C89) and uses only the
standard library; four small functions for the clock and the keyboard
exist in two forms, for POSIX and for Windows.

The program is the virtual machine of `doc/vm.md`, with the Genesis of
`doc/genesis.md`. It is the reference among the bootstrap VMs: its parts
come in the order of the contract, each with a comment, and this file
explains them one by one. The other bootstrap VMs (awk, bash, Python,
JavaScript) have the same parts with the same names. The C bootstrap VM is also the fastest: it
builds uCForth in 5 ms and each target in less than half a second.

## Build

In the folder `ucf`:

    cc -O2 -o build/ucf-c bootstrap/c/ucf.c

Every C compiler works: gcc, clang and tcc are tested, on Windows
MinGW (see Tests). With all checks of the compiler on, it gives no
warning:

    cc -std=c89 -pedantic -Wall -Wextra -Wshadow -O2 -o build/ucf-c bootstrap/c/ucf.c

On Windows with MinGW: `gcc -O2 -o ucf-c.exe bootstrap/c/ucf.c`. The
compiler of Visual Studio should work too (`cl /O2 bootstrap\c\ucf.c`),
but it is not tested yet.

## Start

    $ build/ucf-c
    uCForth - C bootstrap VM - 16 bit - 165 words in FORTH - built in 4 ms
    ok> 3 4 + .
    7
    ok> BYE

The bootstrap VM compiles `forth/core.fs` (Genesis) and `forth/base.fs`
(the Forth interpreter `QUIT`), then the prompt `ok>` comes. Options and
source files come after the name of the program; the files load before
the keyboard:

    build/ucf-c -32 forth/test/tester.fs forth/test/t0.fs

| Option | Effect |
| --- | --- |
| `-32` | 32-bit cells (default: 16 bit) |
| `-pure` | only the 32 mandatory primitives and the HAL; the words with `?:` stay Forth words |
| `-core FILE`, `-base FILE` | other files instead of `forth/core.fs` and `forth/base.fs` |
| `-image FILE` | start from an image (written by `FREEZE` or `-dump`), without Genesis |
| `-dump-core FILE` | write the image after `core.fs`: the seed |
| `-dump FILE` | write the image after `base.fs` |
| `-check` | build with `-pure` and compare the images with `seed/` (see Tests) |

The program finds `forth/` and `seed/` in the current folder or two
folders above it. From another folder, give `-core` and `-base`.

Files and `INCLUDE` come with the package FILES:

    $ build/ucf-c host/files.fs
    uCForth - C bootstrap VM - 16 bit - 165 words in FORTH - built in 4 ms
    ok> INCLUDE examples/hello.fs
    Hello from uCForth

`FREEZE` saves the image to `ucf.hex` (or to the file in `$UCF_IMAGE`),
and `-image` starts it again. The images are the same as the images of
the other bootstrap VMs, so each of them starts the images of the
others:

    $ build/ucf-c
    uCForth - C bootstrap VM - 16 bit - 165 words in FORTH - built in 4 ms
    ok> : HELLO  ." Hello" CR ;
    ok> FREEZE
      saved: ucf.hex
    ok> BYE
    $ build/ucf-c -image ucf.hex
    uCForth - C bootstrap VM - 16 bit - 164 words in FORTH - image ucf.hex
    ok> HELLO
    Hello

Ctrl-C stops the word that runs; at the prompt it gives a new prompt:

    ok> : X  BEGIN AGAIN ;  X
    ^C
      error: user interrupt
    ok>

## How it works

The parts of `ucf.c`, in their order:

| Part | Contents |
| --- | --- |
| The system | the clock, a wait, the test for a keyboard, Ctrl-C: for POSIX and for Windows |
| Options | the command line |
| Memory map | the memory of the VM, its areas, the system variables |
| Primitives | the table of the primitives: number, name, vocabulary, flags |
| Helpers | cells in memory, the stacks, the output line, messages |
| Search in the dictionary | the names in the wordlists and in the search order |
| The virtual machine | NEXT and the primitives |
| Terminal | the input lines for `?RX`, the output for `TX!` |
| Files | the HAL words for files |
| Genesis | the outer interpreter that compiles `core.fs` |
| Images | Intel HEX: write, read, compare |
| The HAL | time, `SAVE-IMAGE`, `BYE`, the files, the host primitives of Genesis |
| Start | `main`: Genesis or an image, then `QUIT` |

### Memory

The memory of the VM is an array of bytes, `mem`: 64 KB with 16-bit
cells, 256 KB with 32-bit cells. `calloc` gets it at the start, with
each byte 0. A VM address is an index into this array. `get` and `put` read and write a cell byte by byte, the low byte
first:

    x = mem[a] | mem[a + 1] << 8 | mem[a + 2] << 16 | mem[a + 3] << 24

So the memory has the order of `doc/vm.md` (little endian), whatever
the order of the computer is: the program runs the same on a computer
with the high byte first (big endian, such as the IBM mainframe s390x).

Each access takes the address modulo the size of the memory (`AMASK`).
So a wrong address in a program never reaches other memory; it wraps
around in the memory of the VM. The VM does not check the stacks, as a
kernel on a board does not. A small underflow of the data stack lands
in the 8 free cells at the start of the dictionary (`GUARD`;
`doc/vm.md`, section 3).

### Cells and numbers

A cell is an `unsigned long`. C guarantees at least 32 bits for it; on
most computers of today it has 64 bits. After each operation, the
program keeps only the bits of a cell: `& MASK` (`MASK` is `$FFFF` or
`$FFFFFFFF`). Unsigned numbers in C wrap around, so `+`, `-` and
`NEGATE` need no more than this mask.

Some primitives need a trick:

- **Signed comparison** (`<`): the program flips the top bit of both
  numbers (`^ SIGN`), then it compares them unsigned. So -1 (`$FFFF`)
  becomes `$7FFF` and 0 becomes `$8000`: the order is right.
- **`2/`** keeps the top bit: `x >> 1 | (x & SIGN)`.
- **`UM*`** with 32-bit cells gives a product with 64 bits. C89 has no
  type with 64 bits for sure. So `um_star` splits each cell into two
  halves of 16 bits and adds the four part products, as on paper.
- **`UM/MOD`** divides a number of two cells by one cell. `um_slash_mod`
  does it bit by bit, as a division on paper in base 2: shift the
  remainder left, take the next bit of the dividend, subtract the
  divisor if it fits. After 16 or 32 steps the quotient is complete.
- **`(+LOOP)`** ends the loop when the index crosses the border between
  limit-1 and limit. With x = index - limit before the step and y after
  it, this is the case when x and y have different signs, and x and the
  step too: `(x ^ y) & (x ^ n) & SIGN`.

### The virtual machine

The function `vm` is NEXT and a `switch` over the primitive number:

    w = get(ip); ip += C;          the xt of the next word
    op = get(w);                   its code field
    switch (op) { case 0: ...      DOCOL, DOVAR, ... one case each

A primitive with more code (`UM*`, `UM/MOD`, `MOVE`, `FIND-NAME`, the
console, the HAL and the files) is a function. The loop ends at
primitive 127, HALT: then the VM gives control back to C.

To run one word, `run_xt` puts its xt and the address of the HALT cells
into two free cells at the top of the memory, sets IP there, and runs
`vm`. Genesis runs each word of `core.fs` so.

A primitive that finds an error (for example a division by 0) pushes
the error code, puts the xt of `THROW` into two other free cells at the
top of the memory and continues there, as a kernel does (`throw_`).

### Search in the dictionary

`FIND-NAME` (primitive 58) goes through the search order and each
wordlist, and compares the length and then the characters of each name
(`wlfind`, `gfind`). Without `CASE-SENSE`, the letters a to z match A to
Z. Genesis uses the same functions. `sysxt` finds a word of the image by
its exact name, for example `QUIT`, `THROW` and `REFILL`.

### The terminal

`?RX` gives the characters of one line, then LF. The next line comes:

1. from the files: `base.fs`, then the files of the command line;
2. then from the keyboard, or from a pipe.

The bootstrap VM reads a new line only when `QUIT` waits in `REFILL`
(the return stack holds a return address into `REFILL`). If another word
asks for a key while files load, `?RX` answers "no key"
(`doc/vm.md`, section 9).

`TX!` collects the characters of an output line and shows the line at
LF. So the bootstrap VM can hide the prompts of `QUIT` while a file
loads. A line with `error:` stops the loading, and the bootstrap VM shows
the file and the line number. At the keyboard, the terminal shows the
typed line; from a pipe, the bootstrap VM shows it after the prompt.

### Ctrl-C

Ctrl-C makes the signal SIGINT. Its handler only sets the variable
`brk`. NEXT looks at `brk` before each word, so the break comes between
two primitives, where the VM is complete (`doc/vm.md`, section 9). Then
`do_break` writes a new line and sets `OUT` to 0; with a `CATCH` it runs
`-28 THROW` ("user interrupt"), else it empties the stacks and runs
`QUIT`. `MS` waits in steps of 100 ms, so Ctrl-C also ends a long wait.

At the prompt, Ctrl-C ends the read at the keyboard: the program
writes a new line and shows the prompt again. On POSIX this needs the
handler without `SA_RESTART` (`sigaction`); else the read would go on.

### Genesis

Genesis compiles `forth/core.fs`, as `doc/genesis.md` tells:

| Function | Task |
| --- | --- |
| `genesis` | the start dictionary, the loop over the lines, the end |
| `gline` | one line: copy it into TIB, take the words, find, execute, compile |
| `gnumber` | convert a word into a number |
| `gheader` | the host primitive `HEADER` |
| `gexec` | execute one word, through `CATCH` when `core.fs` has it |

### Files

`readlines` cuts a file into lines at each LF and takes away a CR at
the end of a line. The text after the last LF is one more line, also
when it is empty (`doc/genesis.md`, section 2).

The HAL words for files (`doc/vm.md`, section 8) use the numbers 10 to
29 as fid. `OPEN-FILE` reads the whole file into memory (`slurp`);
`READ-LINE` and `READ-FILE` take the bytes from there. `CREATE-FILE`
removes an old file first and makes a new one: macOS keeps the code
signature of a program for each file, and a program that is only
shorter would not start. `WRITE-FILE` writes into the file at once.

### Images

An image is a text file with Intel HEX records (`doc/vm.md`, section
10). `write_image` writes it, `hexread` reads it again: into the memory
(`-image`) or to compare it with the memory (`-check`). The images of
this program are the same as the files in `seed/`, also as text.

### The system

Four functions depend on the operating system; they come first in the
file:

| Function | POSIX | Windows |
| --- | --- | --- |
| `now_ms`: the clock in milliseconds | `gettimeofday` | `GetTickCount` |
| `wait_us`: wait | `nanosleep` (Ctrl-C ends it) | `Sleep` |
| `is_tty`: the input is a keyboard | `isatty` | `_isatty` |
| `catch_int`: the handler of Ctrl-C | `sigaction` without `SA_RESTART` | `signal` |

On POSIX the program defines `_POSIX_C_SOURCE` before the first
`#include`; so a strict C89 compiler knows these functions too.

## Tests

In the folder `ucf`:

    build/ucf-c -check
    build/ucf-c -32 -check

Genesis compiles `core.fs` with only the mandatory primitives (`-pure`),
and the program compares its memory with `seed/core16.hex`; then `QUIT`
compiles `base.fs`, and the program compares with `seed/base16.hex`.
Expected, and the exit status 0:

      check core16: the same as seed/core16.hex
      check base16: the same as seed/base16.hex

Then all tests of the words, with 16 and 32 bit (`-32`):

    build/ucf-c host/files.fs tools/test.fs </dev/null

Expected, and the exit status 0:

    t0: 183 tests, 0 errors
    hayes: 875 tests, 0 errors
    files: 26 tests, 0 errors
    ihex: 11 tests, 0 errors
    vm: 9 tests, 0 errors
    fbcon: 14 tests, 0 errors
    text: 10 tests, 0 errors
    package: 8 tests, 0 errors
    slip: 6 tests, 0 errors
    all: 2882 tests, 0 errors

Between these lines, some tests show messages such as `redefined HD-A`
or `cannot open host/test/none.tmp`: the tests cause them on purpose.
The output is the same as the output of the awk bootstrap VM, line for
line. With `-32`: `all: 12186 tests, 0 errors`.

The fixed point (`tools/README.md`): uCForth builds its reference
images again, in 1.5 s:

    build/ucf-c host/files.fs tools/genesis.fs </dev/null

Each target that the C bootstrap VM builds (`targets/*/README.md`, with
`build/ucf-c -32` instead of `LC_ALL=C awk -f bootstrap/awk/ucf.awk --
-32`) is the same file, byte for byte, as from the awk bootstrap VM:
`ucf-macos-a64`, `ucf`, `ucf-linux-a64`, `kernel8.img`, `esp32s3.bin`,
`esp32.bin`.

Tested (2026), each with `-check` (16 and 32 bit) and all tests:

| Compiler | System | Note |
| --- | --- | --- |
| Apple clang 21 | macOS 26, Apple M4 (arm64) | also Ctrl-C at a terminal |
| gcc 12, tcc 0.9.27 | Debian 12, arm64 (Docker) | |
| gcc 12 | Debian 12, i686 and armv7 (Docker) | 32-bit computers: `unsigned long` has 32 bits there |
| gcc 14 | Alpine 3.22, s390x (Docker) | IBM Z: the high byte first (big endian) |
| MinGW (gcc 12) | Windows, in Wine (Docker, amd64) | also a target: `ucf-linux-a64` the same as from macOS |

The systems in Docker other than arm64 run in an emulator (doc/docker.md).

## Speed

Measured on an Apple M4 (2026), with 32-bit cells where it matters:

| Task | C | awk (macOS) | bash 3.2 |
| --- | --- | --- | --- |
| compile `core.fs` and `base.fs` | 5 ms | 1 s | 16 to 20 s |
| `-check` | 0.04 s | 10 s | 2.5 min |
| all tests, 16 bit | 0.07 s | 13 s | 8.5 min |
| `tools/genesis.fs`, 16 bit | 1.5 s | 6 min | - |
| build `ucf-macos-a64` | 0.3 s | 1 min | - |
| steps of the VM per second | 300 million | 1 million | 40 000 to 85 000 |

A loop of 10 million `DO LOOP` steps takes 35 ms.

## Limits

- `?RX` gets whole lines: the terminal sends a line with the return key.
  A word that waits for single keys gets them only after the return key.
- No checks of the stacks; a wrong address wraps around in the memory
  of the VM.
- `REBOOT` starts `BOOT` or `QUIT` again, but does not load the image
  again. `ERASE-IMAGE` does nothing.
- `OPEN-FILE` with `W/O` or `R/W` writes at the end of the file.
- `TX!` does not send the byte 0.
- No serial lines (94-96) and no `ARG` (97) yet.
