# macos-x64: uCForth for macOS on a Mac with Intel

The result is `build/ucf-macos-x64`: one program file (Mach-O for
x86-64) with the virtual machine in machine code, the image and a code
signature. It needs no other file and no C library. It is the host for
the boards on a Mac with an Intel processor: it has files and serial
lines. On a Mac with Apple silicon, Rosetta runs it too
(`build/ucf-macos-a64` is the native program there).

The kernel is `kernels/x64.fs`, the same as for Linux on x86-64 and for
UEFI; the HAL calls the kernel of macOS, as `targets/macos-a64` does.

## Build

In the folder `ucf`, with a bootstrap VM with 32-bit cells:

    build/ucf-c -32 host/files.fs targets/macos-x64/build.fs </dev/null
    chmod +x build/ucf-macos-x64

The output ends with:

      saved: ucf.hex
      code: 8576 bytes, image: 88700 bytes
      written: build/ucf-macos-x64 (115696 bytes)

Each bootstrap VM builds the same file, byte for byte. `build.fs` does
four things:

1. `FREEZE`: the running system is the image: `base.fs`, the package
   FILES and each file before `build.fs` on the command line.
2. The assembler `asm/x64.fs` makes the code: the kernel
   `kernels/x64.fs` and `hal.fs` (start, console, time, files, serial
   lines, the arguments, Ctrl-C, faults).
3. It writes the Mach-O file (package MACHO, `lib/macos/macho.fs`, with
   `MACHO-X86-64`): headers and code, the memory of the virtual machine
   with the image, then the code signature.
4. While it writes, it hashes each page of 4 KB (package SHA256).

Check the signature (optional):

    $ codesign -v -v build/ucf-macos-x64
    build/ucf-macos-x64: valid on disk
    build/ucf-macos-x64: satisfies its Designated Requirement

## Run

On a Mac with Intel:

    $ build/ucf-macos-x64
    ok> 3 4 * .
    12

On a Mac with Apple silicon, with Rosetta (macOS asks once to install
it): `arch -x86_64 build/ucf-macos-x64`.

Ctrl-C stops the word that runs now and gives the prompt again; it also
ends a wait (`MS`) at once:

    ok> : X BEGIN AGAIN ;  X
    ^C
      error: user interrupt
    ok>

`BYE`, or Ctrl-D at the start of a line, ends the program. Ctrl-\ ends
it at once. The arguments of the command line are for an application
(`n ARG`, package FILES); the exit status is `n (BYE)`. An address
outside the program (for example `$7FFFFFF0 @`) is the error -9
("invalid memory address"), not the end of the program.

## Test

    $ echo "ALSO FILES  INCLUDE tools/test.fs" | build/ucf-macos-x64
    ...
    all: 12186 tests, 0 errors

With Rosetta on a Mac with Apple silicon this takes 4.2 s
(`arch -x86_64` in front).

**Tested (2026):** on macOS 26 with Rosetta (a Mac with M4): all tests;
`ARG`; the exit status; `MS` and `US`; Ctrl-C at a pseudo terminal (a
word in a loop, `MS`, and with `CATCH`: -28); -9 for a wrong address
(SIGSEGV), also more than once and in `FILL`, and for a write into the
signature (SIGBUS); the tool `ucf-intel` with the ESP32-S3: `info`,
`reset`, `term` over the real USB port. Not yet tested: a Mac with
Intel.

## Test on a Mac with Intel

The test needs a Mac with an Intel processor (`uname -m` shows
`x86_64`) and the folder `ucf` with `build/ucf-macos-x64` and
`build/ucf-intel`: build them on another computer and copy the folder,
or build them on this Mac with a bootstrap VM (section "Build").

1. Start the program in the folder `ucf`:

       $ build/ucf-macos-x64
       ok> 3 4 + .
       7

   A file from the internet has the attribute "quarantine", and macOS
   asks first or does not start it; `xattr -d com.apple.quarantine
   build/ucf-macos-x64` removes the attribute.
2. Ctrl-C, and an address that is not there:

       ok> : X BEGIN AGAIN ;  X
       ^C
         error: user interrupt
       ok> $7FFFFFF0 @
         error: invalid memory address
       ok> BYE

3. All tests:

       $ echo "ALSO FILES  INCLUDE tools/test.fs" | build/ucf-macos-x64
       ...
       all: 12186 tests, 0 errors

4. With an ESP32-S3 board at a USB port (`ls /dev/cu.*` shows its name;
   `lsof` on it shows no other program):

       $ build/ucf-intel info /dev/cu.usbmodem5C4C0505931
       $ build/ucf-intel term /dev/cu.usbmodem5C4C0505931

   `info` shows the chip, as in doc/start.md; `term` shows `board ok>`
   on a board with uCForth in its flash. `#bye` ends `term`.

Please report the model of the Mac, the version of macOS (`sw_vers`),
and the result of each step with the messages. If macOS stops the
program at once ("killed"), the signature is the cause: please report
the output of `codesign -v -v build/ucf-macos-x64`.

## Serial lines

The HAL has the primitives 94-96 (doc/vm.md, section 8), as
`targets/macos-a64`; the package SERIAL (`host/serial.fs`) uses them:

    ok> ALSO FILES  S" lib/text.fs" INCLUDED  S" host/serial.fs" INCLUDED
    ok> : P  S" /dev/cu.usbserial-1110" OPEN-PORT ;  P
    ok> : HI  S" hello" SEND ;  HI

A USB serial adapter appears as `/dev/cu.usbserial-...`, a board with
USB (such as the ESP32-S3) as `/dev/cu.usbmodem...`.

## The network and SYSCALL

The HAL word 100, `SYSCALL`, makes each call of the macOS kernel. The
package MACOS (`lib/macos/system.fs`) gives it its header, and the calls and
numbers for the network. The packages in `lib/net/` use it: TCP, UDP
and DNS, with the same words as on each other system.

    ok> ALSO FILES  INCLUDE lib/net/host.fs
    ok> S" example.org" RESOLVE . .IP
    0 172.66.157.237

The test of the network: `echo "ALSO FILES  INCLUDE lib/net/test/net.fs"
| build/ucf-macos-x64` gives `net: 93 tests, 0 errors` (also with Rosetta on Apple silicon). lib/net/README.md
explains the words, `SYSCALL` with VM addresses, and the limits.

## An application, and the tool ucf

Files between `host/files.fs` and `build.fs` go into the program. With
`TURNKEY` it starts by itself; an application may define `ON-BOOT` (the
program starts with it instead of `BOOT`) and `PROGRAM ( -- a u )`, the
name of the file: `build.fs` adds `-intel` to it, so that it does not
replace the tool of a Mac with Apple silicon in the same folder. The
tool `ucf` (`tools/ucf.fs`) does so:

    build/ucf-c -32 host/files.fs tools/ucf.fs targets/macos-x64/build.fs </dev/null
    chmod +x build/ucf-intel

gives `build/ucf-intel`, the tool for a Mac with Intel (copy it as
`build/ucf` there):

    $ build/ucf-intel term /dev/cu.usbmodem5C4C0505931

`tools/README.md` and `host/README.md` explain the commands.

## How it works

The file has the same form as for Apple silicon (`targets/macos-a64`):
the load commands for `dyld`, the code, the memory of the virtual
machine, the code signature. Only the processor in the header differs.

| Part of the file | Contents                                                                                       |
| ---------------- | ---------------------------------------------------------------------------------------------- |
| offset 0         | Mach-O header and load commands (472 bytes)                                                    |
| `$200`           | the code: the table of the primitives, the kernel, the HAL                                     |
| `$4000`          | the memory of the virtual machine: VM address 0, the image; in memory 256 KB and 16 KB scratch |
| after the image  | the code signature: a SHA-256 digest of each page of 4 KB                                      |

macOS loads the program at an address that changes with each start.
The code finds the memory relative to itself (`n [RIP]`); the table of
the primitives holds offsets, not addresses. The registers of the
virtual machine are R12 (IP), RBX (W), R13 (the data stack) and RBP (the
return stack); R15 holds the address of VM address 0 (`kernels/x64.fs`).

- **Calls of macOS:** the number + `$2000000` in EAX, the arguments in
  RDI, RSI, RDX, R10 and R8, then `syscall`. On an error the carry flag
  is set; the HAL makes the error number negative, as on Linux.
- **Ctrl-C:** macOS calls a trampoline of the program for the signal
  SIGINT; it sets the cell `BREAK-OFF`, then `sigreturn` goes back.
  From then on the dispatch of the next primitive runs BREAK.
- **Faults:** the trampoline for SIGSEGV and SIGBUS looks at RIP in the
  saved registers. In the code of the program, the program goes on at
  FAULT (`-9 THROW`). Else the program ends, as without the trampoline.
- **The time:** `gettimeofday`. `MS` and `US` wait with `select`.

## Memory

macOS loads the program at another address at each start. Like each
program, it has virtual addresses of its own: macOS maps their pages to
the physical memory when the program uses them.

| Part | Size | Contents |
| --- | --- | --- |
| the code | 16 KB | the headers of the file, then the code (8.2 KB): the table of the primitives, the kernel, the HAL; read and execute |
| the memory of the VM | 256 KB | VM address 0, the image; read and write |
| the scratch area of the HAL | 16 KB | the break cell, the console, the time, a file name, the settings of a serial line, the arguments for `ARG` (0 to 31), the registers for `SYSCALL` |
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
the other limits of the VM. A file name and an argument (`ARG`) have at
most 255 bytes.

## Limits

- `SAVE-IMAGE` (and so `FREEZE`) gives the error -21: `build.fs` on a
  bootstrap VM makes the programs.
- The signature is ad hoc (no certificate). A file that comes from the
  internet has the attribute "quarantine"; then macOS asks first.
- Rosetta runs the program on Apple silicon; Apple has said that it
  will limit Rosetta in a later version of macOS.
