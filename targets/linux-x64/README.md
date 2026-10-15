# linux-x64: uCForth for Linux on a PC with 64 bits

The result is `build/ucf-linux-x64`: one program file (ELF, the 64-bit
form) with the virtual machine in x86-64 machine code and the image. It
needs no other file and no C library. It runs on each Linux for a PC
with 64 bits (x86-64, also called amd64), also where the kernel has no
support for programs with 32 bits (`build/ucf-linux-i386` needs it).

The kernel is `kernels/x64.fs`, the same as for macOS on Intel and for
UEFI; only the HAL differs: calls of the Linux kernel.

## Build

In the folder `ucf`, with a bootstrap VM with 32-bit cells:

    build/ucf-c -32 host/files.fs targets/linux-x64/build.fs </dev/null
    chmod +x build/ucf-linux-x64

The output ends with:

      saved: ucf.hex
      code: 8586 bytes, image: 88700 bytes
      written: build/ucf-linux-x64 (154236 bytes)

Each bootstrap VM builds the same file, byte for byte. `build.fs` does
three things:

1. `FREEZE`: the running system is the image: `base.fs`, the package
   FILES and each file before `build.fs` on the command line.
2. The assembler `asm/x64.fs` makes the code: the kernel
   `kernels/x64.fs` and `hal.fs` (the start, the console, time, files,
   serial lines, the arguments, Ctrl-C, faults).
3. It writes the ELF file (package ELF, `lib/linux/elf.fs`): the headers
   and the code, then the memory of the virtual machine with the image.

`chmod +x` is necessary once: a bootstrap VM cannot mark a file as a
program.

## Run

    $ build/ucf-linux-x64
    ok> 3 4 * .
    12
    ok> BYE

The terminal works in its normal line mode: it shows what you type and
sends the line with the return key.

- Ctrl-C stops the word that runs now and gives the prompt again; it
  also ends a wait (`MS`) at once. Ctrl-\ ends the program at once.

      ok> : X BEGIN AGAIN ;  X
      ^C
        error: user interrupt
      ok>

- `BYE`, or Ctrl-D at the start of a line, ends the program. Input from
  a pipe works too: at its end, the program ends when `QUIT` waits for
  the next line.
- Files: `ALSO FILES  INCLUDE examples/hello.fs`.
- The arguments of the command line: `n ARG ( -- a u )` (package FILES);
  `0 ARG` is the program.
- The exit status: `n (BYE)`.
- An address outside the program (for example `$7FFFFFF0 @`) is the
  error -9 ("invalid memory address"), not the end of the program.

## Serial lines

The HAL has the primitives 94-96 (doc/vm.md, section 8), as
`targets/linux-i386`; the package SERIAL (`host/serial.fs`) uses them:

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
package LINUX-X64 (`lib/linux/x64.fs`) gives it its header, and the calls and
numbers for the network. The packages in `lib/net/` use it: TCP, UDP
and DNS, with the same words as on each other system.

    ok> ALSO FILES  INCLUDE lib/net/host.fs
    ok> S" example.org" RESOLVE . .IP
    0 172.66.157.237

The test of the network: `echo "ALSO FILES  INCLUDE lib/net/test/net.fs"
| build/ucf-linux-x64` gives `net: 93 tests, 0 errors` (in Docker: the command in section "Test" with this line). lib/net/README.md
explains the words, `SYSCALL` with VM addresses, and the limits.

## An application, and the tool ucf

Files between `host/files.fs` and `build.fs` go into the program. With
`TURNKEY` it starts by itself; an application may define `ON-BOOT` (the
program starts with it instead of `BOOT`) and `PROGRAM ( -- a u )`, the
name of the file: `build.fs` adds `-x64` to it. The tool `ucf`
(`tools/ucf.fs`) does so:

    build/ucf-c -32 host/files.fs tools/ucf.fs targets/linux-x64/build.fs </dev/null
    chmod +x build/ucf-x64

gives `build/ucf-x64`, the tool for Linux on a PC with 64 bits (copy it
as `ucf`):

    $ build/ucf-x64 term /dev/ttyACM0
    $ build/ucf-x64 flash /dev/ttyACM0 build/esp32s3.bin 0

`tools/README.md` and `host/README.md` explain the commands.

## Test

In Docker on a Mac, in the folder `ucf` (Docker Desktop runs the
platform `linux/amd64` with Rosetta):

    docker run --rm --platform linux/amd64 -v "$PWD":/u -w /u debian:trixie-slim \
      sh -c 'echo "ALSO FILES  INCLUDE tools/test.fs" | ./build/ucf-linux-x64'

The output ends with `all: 12210 tests, 0 errors`, after less than a
second.

**Tested (2026):** in Docker (Debian, amd64 with Rosetta on the Mac):
all tests; `ARG`; the exit status; `MS`; Ctrl-C at a pseudo terminal (a
word in a loop, `MS`, and with `CATCH`: -28); -9 for a wrong address,
also more than once and in `FILL`; the serial line with `socat` (send,
receive, the speeds, a speed that Linux does not know, a port that is
not there, a file that is not a serial line); the tool `ucf-x64` with
`info` and `term` over a pseudo terminal to a second uCForth. On Alpine
Linux 3.22 (x86-64, Linux 6.12, a real Linux kernel in QEMU,
`doc/qemu.md`): all tests, `ARG`, the exit status, `MS`, -9, Ctrl-C (in
a loop and in `MS`). Not yet tested: a PC, a real serial port.

## Test on a PC with Linux

The test needs a PC with Linux for 64 bits (`uname -m` shows `x86_64`)
and the folder `ucf` with `build/ucf-linux-x64` and `build/ucf-x64`.
After a copy, `chmod +x build/ucf-linux-x64 build/ucf-x64` may be
necessary.

1. Start the program in the folder `ucf`:

       $ build/ucf-linux-x64
       ok> 3 4 + .
       7

2. Ctrl-C, and an address that is not there:

       ok> : X BEGIN AGAIN ;  X
       ^C
         error: user interrupt
       ok> $7FFFFFF0 @
         error: invalid memory address
       ok> BYE

3. All tests:

       $ echo "ALSO FILES  INCLUDE tools/test.fs" | build/ucf-linux-x64
       ...
       all: 12210 tests, 0 errors

4. With an ESP32-S3 board at a USB port (section "Serial lines": the
   name of the port and the group `dialout`):

       $ build/ucf-x64 info /dev/ttyACM0
       $ build/ucf-x64 term /dev/ttyACM0

   `info` shows the chip, as in doc/start.md; `term` shows `board ok>`
   on a board with uCForth in its flash. `#bye` ends `term`.

Please report the distribution and its version
(`cat /etc/os-release`), the version of the kernel (`uname -r`), and
the result of each step with the messages.

## Memory

Linux loads the program at a fixed address. Like each program, it has
virtual addresses of its own: Linux maps their pages to the physical
memory when the program uses them.

| Address | Size | Contents |
| --- | --- | --- |
| `$400000` | 64 KB | ELF headers (176 bytes), then the code: the table of the primitives, the kernel, the HAL (8.4 KB of the 64 KB); read and execute |
| `$410000` | 256 KB | the memory of the virtual machine: VM address 0, the image (doc/vm.md, section 3); read and write |
| `$450000` | 16 KB | the scratch area of the HAL: the break cell, the console, the time, a file name, a serial line, the arguments for `ARG`, the registers for `SYSCALL` |
| from Linux | 8 MB | the stack of the processor (`ulimit -s`) |

After the start, with the image of `build.fs` (base and FILES):

| Area | Size | Free |
| --- | --- | --- |
| dictionary: words, `ALLOT`, `,` | 189 KB | 169 KB |
| RAM data: `VARIABLE`, `VALUE`, `BUFFER:`, vocabularies | 63 KB | 61 KB |
| data stack, return stack | 256 cells each | |
| the input line | 256 characters | |

`UNUSED U.` shows the free bytes of the dictionary,
`ALSO INTERNALS  DATA-END VP @ - U.` those of the RAM data. A full
dictionary or RAM data gives the error -8. doc/vm.md, section 3, has
the other limits of the VM.

More: a file name and an argument have at most 255 bytes; `ARG` gives
the arguments 0 to 31.

## How it works

The registers of the virtual machine are R12 (IP), RBX (W), R13 (the
data stack) and RBP (the return stack); R15 holds the address of VM
address 0 and R14 the address of the table of the primitives
(`kernels/x64.fs`). A call of Linux puts its number into EAX and its
arguments into RDI, RSI, RDX and R10, then `syscall`; Linux changes
only RAX, RCX and R11, so the VM stays in its registers.

- **The console:** `?RX` asks with `poll` whether a byte is there and
  waits at most 1 ms. So `KEY?` never stops the program.
- **Ctrl-C:** the handler of the signal SIGINT sets one cell
  (`BREAK-OFF`): from then on the dispatch of the next primitive runs
  BREAK. A wait (`poll`, `nanosleep`) ends at once.
- **Faults:** the handler of the signal SIGSEGV looks at the address of
  the instruction (RIP in the saved registers). In the code of the
  program, it lets the program go on at FAULT, which runs `-9 THROW`.
  Else the program ends, as without the handler.
- **Serial lines:** `ioctl` with `TCGETS` and `TCSETS` sets the line raw
  (8 data bits, no parity, no flow control, a read does not wait);
  `TIOCMBIS` and `TIOCMBIC` set DTR and RTS.

## Limits

- `SAVE-IMAGE` (and so `FREEZE`) gives the error -21: `build.fs` on a
  bootstrap VM makes the programs.
- A serial line takes only the speeds of the table in `hal.fs`.
- DTR and RTS: a pseudo terminal has none (`LINES?` false); a USB
  serial line has them.
