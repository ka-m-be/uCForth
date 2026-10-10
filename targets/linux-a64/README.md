# linux-a64: uCForth for Linux on 64-bit ARM

The result is `build/ucf-linux-a64`: one program file (ELF) with the
virtual machine in machine code and the image. It needs no other file
and no C library. It runs on each Linux for aarch64: Raspberry Pi OS
(64 bit) on a Raspberry Pi 3, 4, 400 or 5, Debian, Ubuntu, Alpine, and
in Docker on a Mac with Apple silicon.

## Build

In the folder `ucf`, with a bootstrap VM with 32-bit cells:

    LC_ALL=C awk -f bootstrap/awk/ucf.awk -- -32 host/files.fs targets/linux-a64/build.fs
    chmod +x build/ucf-linux-a64

The output ends with:

      saved: ucf.hex
      code: 7948 bytes, image: 86824 bytes
      written: build/ucf-linux-a64 (152360 bytes)

`build.fs` does three things:

1. `FREEZE`: the running system is the image: `base.fs`, the package
   FILES and each file before `build.fs` on the command line. It goes to
   `ucf.hex`.
2. The assembler `asm/aarch64.fs` makes the code: the kernel
   `kernels/aarch64.fs` (NEXT and the primitives) and `hal.fs` (the start,
   the console, time, files, serial lines and the arguments as calls of
   Linux).
3. It writes the ELF file: the headers and the code, then the memory of
   the virtual machine with the image.

`chmod +x` is necessary once: a bootstrap VM cannot mark a file as a
program.

## Run

    $ build/ucf-linux-a64
    ok> 3 4 * .
    12
    ok> BYE

The package FILES is in the image:

    ok> ALSO FILES  INCLUDE examples/hello.fs
    Hello from uCForth

Input from a pipe works too. At the end of the input, the program ends
when `QUIT` waits for the next line:

    $ echo "ALSO FILES  INCLUDE tools/test.fs" | build/ucf-linux-a64
    ...
    all: 10979 tests, 0 errors

The terminal works in its normal line mode: it shows what you type and
sends the line with the return key. Backspace and Ctrl-U correct the
line.

Ctrl-C stops the word that runs now and gives the prompt again:

    ok> : X BEGIN AGAIN ;  X
    ^C
      error: user interrupt
    ok>

In the input line, Ctrl-C removes the line and gives a new prompt.
`BYE`, or Ctrl-D at the start of a line, ends the program. Ctrl-\
ends it at once, also when the program does not answer.

The arguments of the command line: `n ARG ( -- a u )` (package FILES);
`0 ARG` is the program. The exit status: `n (BYE)`.

## Serial lines

The HAL has the primitives 94-96 (doc/vm.md, section 8); the package
SERIAL (`host/serial.fs`) uses them:

    ok> ALSO FILES  S" lib/text.fs" INCLUDED  S" host/serial.fs" INCLUDED
    ok> ALSO SERIAL  S" /dev/ttyACM0" OPEN-PORT  S" hello" SEND

A board with USB (such as the ESP32-S3) appears as `/dev/ttyACM0`, a
USB serial adapter as `/dev/ttyUSB0`. The user needs the right to use
the port: on most systems, membership in the group `dialout`
(`sudo usermod -aG dialout $USER`, then log in again). The speed is one
that Linux knows (50 to 4000000 baud: 9600, 115200, 921600 ...); another
speed gives an error.

## The network and SYSCALL

The HAL word 100, `SYSCALL`, makes each call of the Linux kernel. The
package LINUX-A64 (`lib/linux/a64.fs`) gives it its header, and the calls and
numbers for the network. The packages in `lib/net/` use it: TCP, UDP
and DNS, with the same words as on each other system.

    ok> ALSO FILES  INCLUDE lib/net/host.fs
    ok> S" example.org" RESOLVE . .IP
    0 172.66.157.237

The test of the network: `echo "ALSO FILES  INCLUDE lib/net/test/net.fs"
| build/ucf-linux-a64` gives `net: 89 tests, 0 errors` (in Docker: the command in section "Test" with this line). lib/net/README.md
explains the words, `SYSCALL` with VM addresses, and the limits.

## An application

An application sets the vectors `'SETUP` and `'LOOP`, then `TURNKEY`.
`RUN` runs `'SETUP` once, then `'LOOP` again and again
(`examples/tick.fs`):

```forth
VARIABLE TICKS
: SETUP ( -- )   0 TICKS !  ." start" CR ;
: STEP  ( -- )   1 TICKS +!  ." tick " TICKS @ . CR  1000 MS ;
' SETUP 'SETUP !   ' STEP 'LOOP !   TURNKEY
```

Put the file before `build.fs` on the command line:

    LC_ALL=C awk -f bootstrap/awk/ucf.awk -- -32 host/files.fs examples/tick.fs targets/linux-a64/build.fs

`TURNKEY` sets `AUTORUN` in the image. The program starts `RUN` after
1 second:

    $ build/ucf-linux-a64
    autostart in 1 s (Ctrl-C: prompt)
    start
    tick 1
    tick 2

In this second, Ctrl-C gives the prompt instead of `RUN`; ESC does
too, but only with the return key (the terminal sends the line only
then). Later, Ctrl-C stops `RUN` and gives the prompt.

An application may also define `ON-BOOT` (the program starts with it
instead of `BOOT`) and `PROGRAM ( -- a u )`, the name of the file:
`build.fs` adds `-a64` to it. The tool `ucf` (`tools/ucf.fs`) does so:

    LC_ALL=C awk -f bootstrap/awk/ucf.awk -- -32 host/files.fs tools/ucf.fs targets/linux-a64/build.fs </dev/null
    chmod +x build/ucf-a64

gives `build/ucf-a64`, the tool for Linux on 64-bit ARM, for example on
a Raspberry Pi (copy it as `ucf`):

    $ build/ucf-a64 term /dev/ttyACM0
    $ build/ucf-a64 flash /dev/ttyACM0 build/esp32s3.bin 0

`tools/README.md` and `host/README.md` explain the commands.

## Test

In Docker on a Mac (Apple silicon), in the folder `ucf`:

    docker run --rm --platform linux/arm64 -v "$PWD":/ucf:ro debian:bookworm-slim \
      sh -c 'cp -r /ucf /tmp/u && cd /tmp/u && chmod +x build/ucf-linux-a64 &&
             echo "ALSO FILES  INCLUDE tools/test.fs" | build/ucf-linux-a64'

Expected: `all: 10979 tests, 0 errors`; the tests take about 0.3 s. The tool
`tools/genesis.fs` (the reference images, 32 bit) takes 0.5 s.

**Tested (2026):** in Docker (Debian 12, arm64): all tests; `ARG`; the
exit status; the serial line with `socat` (send, receive, the speeds, a
speed that Linux does not know, a port that is not there, a file that
is not a serial line); the tool `ucf-a64` with `term` (also with a
file) over a pseudo terminal to a second uCForth. Not yet tested: a
real serial port, Raspberry Pi OS.

## Memory

Linux loads the program at a fixed address. Like each program, it has
virtual addresses of its own: Linux maps their pages to the physical
memory when the program uses them.

| Address | Size | Contents |
| --- | --- | --- |
| `$400000` | 64 KB | ELF headers (176 bytes), then the code: the table of the primitives, the kernel, the HAL (8 KB of the 64 KB) |
| `$410000` | 256 KB | the memory of the virtual machine: VM address 0 is here (doc/vm.md, section 3) |
| `$450000` | 16 KB | the scratch area of the HAL: a byte of the console, the time, a file name, a serial line, the arguments for `ARG`, the registers for `SYSCALL` |
| from Linux | 8 MB | the stack of the processor (`ulimit -s`) |

After the start, with the image of `build.fs` (base and FILES):

| Area | Size | Free |
| --- | --- | --- |
| dictionary: words, `ALLOT`, `,` | 189 KB | 171 KB |
| RAM data: `VARIABLE`, `VALUE`, `BUFFER:`, vocabularies | 63 KB | 61 KB |
| data stack, return stack | 256 cells each | |
| the input line | 256 characters | |

`UNUSED U.` shows the free bytes of the dictionary,
`ALSO INTERNALS  DATA-END VP @ - U.` those of the RAM data. A full
dictionary or RAM data gives the error -8. doc/vm.md, section 3, has
the other limits of the VM.

An address outside the memory of the VM is not checked: `@` there
reads what lies there in the memory of the program, or the program ends
(the signal SIGSEGV). A file name and an argument have at most 255
bytes; `ARG` gives the arguments 0 to 31.

## Limits

- `SAVE-IMAGE` (and so `FREEZE`) gives the error -21: `build.fs` makes
  the programs.
- A serial line takes only the speeds of the table in `hal.fs`; a
  pseudo terminal has no lines DTR and RTS (`LINES?` false).
- `?RX` waits at most 1 ms for a key, so `KEY?` in a loop does not stop
  the program.
