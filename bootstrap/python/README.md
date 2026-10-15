# The Python bootstrap VM

`ucf.py` runs uCForth on any computer with Python 3.6 or newer: Linux,
macOS, BSD, and Windows (not tested yet). It is one file and uses only
the standard library of Python.

The program has the same parts, with the same names, as the C bootstrap
VM (`bootstrap/c/ucf.c`), the reference: `bootstrap/c/README.md`
explains each part. This file tells only what is special in Python.

## Start

In the folder `ucf`:

    $ python3 bootstrap/python/ucf.py
    uCForth - Python bootstrap VM - 16 bit - 165 words in FORTH - built in 0.8 s
    ok> 3 4 + .
    7
    ok> BYE

The options are the same as for the other bootstrap VMs (`-32`, `-pure`,
`-core`, `-base`, `-image`, `-dump-core`, `-dump`, `-check`); see
`bootstrap/c/README.md`. Files load before the keyboard:

    $ python3 bootstrap/python/ucf.py host/files.fs
    uCForth - Python bootstrap VM - 16 bit - 165 words in FORTH - built in 0.8 s
    ok> INCLUDE examples/hello.fs
    Hello from uCForth

## What is special in Python

**Memory.** The memory of the VM is a `bytearray` of 64 KB or 256 KB.
`get` and `put` read and write a cell byte by byte, the low byte first.
There are two forms of each, for 2 and for 4 bytes (`get2`, `get4`);
`memory_map` chooses one at the start, so a cell access needs no test of
the cell size.

**Numbers.** A Python integer has no limit. So each primitive keeps only
the bits of a cell with `& MASK`, as in C. `UM*` and `UM/MOD` are simple
here: Python multiplies and divides numbers with 64 bits directly.

**Primitives.** Each primitive is a function; the list `P` holds the
function of each primitive number, and `vm` calls `P[op]()`. The
primitives with two operands come from a small helper: `binary(lambda
x, y: x & y)` makes `AND`. The registers `ip`, `w`, `sp` and `rp` are
global variables of the module.

**Output.** `TX!` collects bytes, not characters; the program writes
them with `sys.stdout.buffer`. So each byte of a Forth string comes out
as it is.

**Ctrl-C.** Python calls the handler `on_int` between two of its own
steps. The handler only sets `brk`; the VM stops the word at the next
NEXT, as in C. But Python repeats a read at the keyboard that a signal
interrupts (PEP 475). So while the program waits for a line, the
handler raises `KeyboardInterrupt`; `read_line` catches it and shows the
prompt again.

**Files.** A file name from the image is a string of bytes; `open`
takes it as it is.

## Tests

    python3 bootstrap/python/ucf.py -check
    python3 bootstrap/python/ucf.py -32 -check
    python3 bootstrap/python/ucf.py host/files.fs tools/test.fs </dev/null

The expected output is in `bootstrap/c/README.md`, section "Tests"; the
output of this program is the same, line for line. `-dump-core` and
`-dump` write the same files as `seed/`. The fixed point
(`tools/genesis.fs`) gives the same images, and each target that this
program builds is the same file, byte for byte, as from the C and the awk
bootstrap VM.

Tested (2026), each with `-check` and all tests: Python 3.9 and 3.14
on macOS; in Docker 3.6, 3.11 (Debian 12) and 3.12 (Alpine 3.22). Also
Ctrl-C at a terminal (macOS).

## Speed

Measured on an Apple M4 (2026) with Python 3.14:

| Task | Python | C | awk (macOS) |
| --- | --- | --- | --- |
| compile `core.fs` and `base.fs` | 0.8 s | 5 ms | 1 s |
| `-check`, 16 bit | 8.5 s | 0.04 s | 10 s |
| all tests, 16 bit | 6 s | 0.04 s | 7 s |
| `tools/genesis.fs`, 16 bit | 6 min | 1.5 s | 6 min |
| build `esp32s3.bin` | 51 s | 0.2 s | 42 s |
| build `ucf-macos-a64` | 77 s | 0.3 s | 60 s |

Python is about as fast as awk here: each step of the VM calls some
small functions, and a call in Python costs about 0.1 us. For much work
the C bootstrap VM is the better tool; the Python bootstrap VM is for a
computer that has Python, but no C compiler.

## Limits

The same as for the C bootstrap VM (`bootstrap/c/README.md`, section
"Limits").
