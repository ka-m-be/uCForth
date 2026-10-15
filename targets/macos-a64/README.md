# macos-a64: uCForth for macOS on Apple silicon

The result is `build/ucf-macos-a64`: one program file (Mach-O) with the
virtual machine in machine code, the image and a code signature. It
needs no other file and no C library. It is the host for the boards:
it has files and serial lines.

## Build

In the folder `ucf`, with a bootstrap VM with 32-bit cells:

    LC_ALL=C awk -f bootstrap/awk/ucf.awk -- -32 host/files.fs targets/macos-a64/build.fs </dev/null
    chmod +x build/ucf-macos-a64

The output ends with:

      saved: ucf.hex
      code: 7900 bytes, image: 88700 bytes
      written: build/ucf-macos-a64 (115696 bytes)

The build takes about 30 s: SHA-256 for the signature in Forth on the
awk VM.

`build.fs` does four things:

1. `FREEZE`: the running system is the image: `base.fs`, the package
   FILES and each file before `build.fs` on the command line.
2. The assembler `asm/aarch64.fs` makes the code: the kernel
   `kernels/aarch64.fs` and `hal.fs` (start, console, time, files,
   serial lines, Ctrl-C).
3. It writes the Mach-O file (package MACHO, `lib/macos/macho.fs`): headers
   and code, the memory of the virtual machine with the image, then the
   code signature.
4. While it writes, it hashes each page of 4 KB (package SHA256).

Check the signature (optional):

    $ codesign -v -v build/ucf-macos-a64
    build/ucf-macos-a64: valid on disk
    build/ucf-macos-a64: satisfies its Designated Requirement

## Run

    $ build/ucf-macos-a64
    ok> 3 4 * .
    12

Ctrl-C stops the word that runs now and gives the prompt again:

    ok> : X BEGIN AGAIN ;  X
    ^C
      error: user interrupt
    ok>

`BYE`, or Ctrl-D at the start of a line, ends the program. Ctrl-\ ends
it at once.

The program takes no commands: the arguments of the command line are
for an application (`n ARG`, package FILES), and uCForth itself does
not look at them. The tool `build/ucf` is such an application
(`build/ucf term PORT`, tools/README.md).

## Test

    $ echo "ALSO FILES  INCLUDE tools/test.fs" | build/ucf-macos-a64
    ...
    all: 12186 tests, 0 errors

This takes about 2.8 s. `tools/genesis.fs` (the reference images,
32 bit) takes 0.4 s.

## Serial lines

The HAL has the primitives 94-96 (doc/vm.md, section 8); the package
SERIAL (`host/serial.fs`) uses them:

    ok> ALSO FILES  S" lib/text.fs" INCLUDED  S" host/serial.fs" INCLUDED
    ok> : P  S" /dev/cu.usbserial-1110" OPEN-PORT ;  P
    ok> : HI  S" hello" SEND ;  HI

A USB serial adapter appears as `/dev/cu.usbserial-...`, a board with
USB (such as the ESP32-S3) as `/dev/cu.usbmodem...`. The console to
uCForth on a board (package TERM) is in host/README.md.

## The network and SYSCALL

The HAL word 100, `SYSCALL`, makes each call of the macOS kernel. The
package MACOS (`lib/macos/system.fs`) gives it its header, and the calls and
numbers for the network. The packages in `lib/net/` use it: TCP, UDP
and DNS, with the same words as on each other system.

    ok> ALSO FILES  INCLUDE lib/net/host.fs
    ok> S" example.org" RESOLVE . .IP
    0 172.66.157.237

The test of the network: `echo "ALSO FILES  INCLUDE lib/net/test/net.fs"
| build/ucf-macos-a64` gives `net: 93 tests, 0 errors`. lib/net/README.md
explains the words, `SYSCALL` with VM addresses, and the limits.

## How it works

macOS on Apple silicon starts only a program that its loader `dyld`
loads, at an address that changes with each start, and that has a code
signature. A program without `dyld` gets the signal SIGKILL at the
start. So the file has the load commands for `dyld`, but the program
calls the kernel directly ("svc #0x80"), without the C library.

| Part of the file | Contents                                                                                       |
| ---------------- | ---------------------------------------------------------------------------------------------- |
| offset 0         | Mach-O header and load commands (472 bytes)                                                    |
| `$200`           | the code: the table of the primitives, the kernel, the HAL                                     |
| `$4000`          | the memory of the virtual machine: VM address 0, the image; in memory 256 KB and 16 KB scratch |
| after the image  | the code signature: a SHA-256 digest of each page of 4 KB                                      |

The code finds the memory relative to itself (`ADR`); the table of the
primitives holds offsets, not addresses.

## Memory

macOS loads the program at another address at each start. Like each
program, it has virtual addresses of its own: macOS maps their pages to
the physical memory when the program uses them.

| Part | Size | Contents |
| --- | --- | --- |
| the code | 16 KB | the headers of the file, then the code (8 KB): the table of the primitives, the kernel, the HAL; read and execute |
| the memory of the VM | 256 KB | VM address 0, the image; read and write |
| the scratch area of the HAL | 16 KB | the console, the time, a file name, the settings of a serial line, the arguments for `ARG` (0 to 31), the registers for `SYSCALL` |
| from macOS | 8 MB | the stack of the processor |

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

An address outside the memory of the VM is not checked: `@` there
reads what lies there in the memory of the program, or the program ends
(the signal SIGSEGV or SIGBUS). A file name and an argument (`ARG`)
have at most 255 bytes.

## Limits

- `SAVE-IMAGE` (and so `FREEZE`) gives the error -21: `build.fs` on a
  bootstrap VM makes the programs.
- The signature is ad hoc (no certificate). A file that comes from the
  internet has the attribute "quarantine"; then macOS asks first.
