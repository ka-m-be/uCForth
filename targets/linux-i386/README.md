# linux-i386: uCForth for Linux on a PC with 32 bits

The result is `build/ucf-linux-i386`: one program file (ELF, the 32-bit
form) with the virtual machine in x86 machine code and the image. It
needs no other file and no C library. It runs on each Linux for a PC:
a Linux with 32 bits (the program needs a 386 or later), and most
Linux systems with 64 bits on an x86-64 PC (their kernel runs programs
with 32 bits too; tested: Alpine Linux).

The kernel is the kernel of Windows (`kernels/x86.fs`); only the HAL
differs: calls of the Linux kernel instead of `KERNEL32.dll`.

## Build

In the folder `ucf`, with a bootstrap VM with 32-bit cells:

    build/ucf-c -32 host/files.fs targets/linux-i386/build.fs </dev/null
    chmod +x build/ucf-linux-i386

The output ends with:

      saved: ucf.hex
      code: 8816 bytes, image: 86824 bytes
      written: build/ucf-linux-i386 (152360 bytes)

Each bootstrap VM builds the same file, byte for byte. `build.fs` does
three things:

1. `FREEZE`: the running system is the image: `base.fs`, the package
   FILES and each file before `build.fs` on the command line.
2. The assembler `asm/x86.fs` makes the code: the kernel
   `kernels/x86.fs` and `hal.fs` (the start, the console, time, files,
   serial lines, the arguments, Ctrl-C, faults).
3. It writes the ELF file (package ELF, `lib/linux/elf.fs`): the headers
   and the code, then the memory of the virtual machine with the image.

`chmod +x` is necessary once: a bootstrap VM cannot mark a file as a
program.

## Run

    $ build/ucf-linux-i386
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
package LINUX-I386 (`lib/linux/i386.fs`) gives it its header, and the calls and
numbers for the network. The packages in `lib/net/` use it: TCP, UDP
and DNS, with the same words as on each other system.

    ok> ALSO FILES  INCLUDE lib/net/host.fs
    ok> S" example.org" RESOLVE . .IP
    0 172.66.157.237

The test of the network: `echo "ALSO FILES  INCLUDE lib/net/test/net.fs"
| build/ucf-linux-i386` gives `net: 89 tests, 0 errors` (in Docker: the command in section "Test" with this line). lib/net/README.md
explains the words, `SYSCALL` with VM addresses, and the limits.

## An application, and the tool ucf

Files between `host/files.fs` and `build.fs` go into the program. With
`TURNKEY` it starts by itself; an application may define `ON-BOOT` (the
program starts with it instead of `BOOT`) and `PROGRAM ( -- a u )`, the
name of the file: `build.fs` adds `-i386` to it. The tool `ucf`
(`tools/ucf.fs`) does so:

    build/ucf-c -32 host/files.fs tools/ucf.fs targets/linux-i386/build.fs </dev/null
    chmod +x build/ucf-i386

gives `build/ucf-i386`, the tool for Linux on a PC (copy it as `ucf`):

    $ build/ucf-i386 term /dev/ttyACM0
    $ build/ucf-i386 flash /dev/ttyACM0 build/esp32s3.bin 0

`tools/README.md` and `host/README.md` explain the commands.

## Test

In Docker on a Mac, in the folder `ucf` (the platform `linux/386` runs
in QEMU):

    docker run --rm --platform linux/386 -v "$PWD":/u -w /u debian:bookworm-slim \
      sh -c 'echo "ALSO FILES  INCLUDE tools/test.fs" | ./build/ucf-linux-i386'

The output ends with `all: 10979 tests, 0 errors`, after about 2 s.

**Tested (2026):** in Docker (Debian 12, i386, QEMU on the Mac): all
tests; `ARG`; the exit status; the end of the input; `MS`; Ctrl-C at a
pseudo terminal (a word in a loop, and `MS`); -9 for a wrong address,
also more than once; the serial line with `socat` (send, receive, the
speeds, a speed that Linux does not know, a port that is not there, a
file that is not a serial line); the tool `ucf-i386` with `term` over a
pseudo terminal to a second uCForth. On Alpine Linux 3.22 (x86-64, a
real Linux kernel in QEMU, `doc/qemu.md`): all tests, -9, `ARG`, `MS`,
the exit status, Ctrl-C (the signal SIGINT). Not yet tested: a PC with a
Linux of 32 bits, a real serial port.

## Memory

Linux loads the program at a fixed address. Like each program, it has
virtual addresses of its own: Linux maps their pages to the physical
memory when the program uses them.

| Address | Size | Contents |
| --- | --- | --- |
| `$400000` | 64 KB | ELF headers (116 bytes), then the code: the table of the primitives, the kernel, the HAL (9 KB of the 64 KB); read and execute |
| `$410000` | 256 KB | the memory of the virtual machine: VM address 0, the image (doc/vm.md, section 3); read and write |
| `$450000` | 16 KB | the scratch area of the HAL: the console, the time, a file name, a serial line, the arguments for `ARG`, the registers for `SYSCALL` |
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

More: a file name and an argument have at most 255 bytes; `ARG` gives
the arguments 0 to 31.

## How it works

The registers of the virtual machine are ESI (IP), EBX (W), EDI (the
data stack) and EBP (the return stack), as on Windows
(`kernels/x86.fs`). A call of Linux puts its number into EAX and its
arguments into EBX, ECX and EDX, then `int 0x80`; Linux keeps all other
registers. W in EBX is free inside a primitive: NEXT loads it again.

- **The console:** `?RX` asks with `poll` whether a byte is there and
  waits at most 1 ms. So `KEY?` never stops the program.
- **Ctrl-C:** the handler of the signal SIGINT sets one cell
  (`BREAK-OFF` in the kernel): from then on the dispatch of the next
  primitive runs BREAK. A wait (`poll`, `nanosleep`) ends at once.
- **Faults:** the handler of the signal SIGSEGV looks at the address of
  the instruction. In the code of the program, it lets the program go
  on at FAULT, which runs `-9 THROW`. Else the program ends, as without
  the handler.
- **Serial lines:** `ioctl` with `TCGETS` and `TCSETS` sets the line raw
  (8 data bits, no parity, no flow control, a read does not wait);
  `TIOCMBIS` and `TIOCMBIC` set DTR and RTS.

## Limits

- `SAVE-IMAGE` (and so `FREEZE`) gives the error -21: `build.fs` on a
  bootstrap VM makes the programs.
- A serial line takes only the speeds of the table in `hal.fs`.
- DTR and RTS: a pseudo terminal has none (`LINES?` false); a USB
  serial line has them.
