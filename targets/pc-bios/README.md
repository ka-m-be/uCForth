# pc-bios: uCForth for a PC with a BIOS, without an operating system

The result is `build/ucf-pc.img`: a floppy disk of 1.44 MB as a file. A
PC starts uCForth from it, without DOS or another system: from a floppy
disk, a USB stick or a hard disk. It runs on each PC from the IBM PC
(8088) on with 128 KB of RAM, and on each newer PC with a BIOS. A newer
PC with UEFI starts it when its "CSM" or "legacy boot" is on.

The kernel is the one of DOS (`kernels/8086.fs`, 16-bit cells); the HAL
calls the BIOS instead of DOS. The PC is then a board: the keyboard and
the screen are its console, and COM1 too, for the tool `ucf term`.

## Build

In the folder `ucf`, with a bootstrap VM with 16-bit cells (the
default):

    LC_ALL=C awk -f bootstrap/awk/ucf.awk -- lib/package.fs lib/pc/bios.fs targets/freeze.fs host/files.fs targets/pc-bios/build.fs </dev/null

(10 s; the C bootstrap VM `build/ucf-c` takes less than 1 s.) The
output ends with:

      saved: ucf.hex
      code: 3370 bytes, image: 10306 bytes
      written: build/ucf-pc.img (2880 sectors, 1440 KB)

1. `lib/package.fs` and `lib/pc/bios.fs`: the package BIOS (SYSCALL,
   the ports) goes into the image (section "The HAL of the PC"). Put
   your application files after them.
2. `targets/freeze.fs`: the running system is the image, without the
   package FILES (the PC has no files).
3. `build.fs` makes the boot sector (`bootsect.fs`) and the code with
   the assembler `asm/8086.fs`: the kernel `kernels/8086.fs` and
   `hal.fs` (the start, the console, the time, the break, `FREEZE`).
   Then it writes the disk:

| Sectors | Contents |
| --- | --- |
| 0 | the boot sector: it loads the code and the memory of the VM |
| 1-16 | the code (8 KB) |
| 17-144 | the memory of the VM (64 KB): the image at its addresses; `FREEZE` writes here |
| 145-162 | two FATs of an empty FAT12 file system |
| 163-2879 | its root folder (14 sectors) and 1.3 MB of free space |

So the disk is also a normal, empty floppy disk with the name UCFORTH:
uCForth is in its reserved sectors. DOS, Windows, macOS and Linux can
read it and put files on it; this does not change uCForth.

Each bootstrap VM builds the same file, byte for byte. DOS builds it
too, with the bootstrap VM for DOS and `SEED.HEX` in the folder of
uCForth, as in bootstrap/dos/README.md:

    C:\UCF>UCF -image SEED.HEX FORTH\BASE.FS LIB\PACKAGE.FS LIB\PC\BIOS.FS TARGETS\FREEZE.FS HOST\FILES.FS TARGETS\PC-BIOS\BUILD.FS

The file is then `BUILD\UCF-PC.IMG`. (A command line of DOS has at
most 127 characters: with the seed in another folder, the line is too
long.)

## Run in QEMU

QEMU emulates a PC with a BIOS (SeaBIOS). On a Mac with QEMU (for
example from MacPorts or Homebrew), in the folder `ucf`:

    qemu-system-i386 -drive file=build/ucf-pc.img,format=raw,if=floppy

A window shows the screen of the PC with the prompt `ok>`, and the
keyboard types there. In place of `if=floppy`, `if=ide` makes the file
a hard disk. As a USB stick:

    qemu-system-i386 -device usb-ehci,id=ehci -drive if=none,id=stick,format=raw,file=build/ucf-pc.img \
      -device usb-storage,bus=ehci.0,drive=stick

`BYE` starts the PC again; with `-no-reboot`, QEMU ends then. QEMU
writes to the file: `FREEZE` stays in it.

COM1 with the tool `ucf` (section "ucf term: the PC as a board"): with
`-display none -serial pty`, QEMU tells the name of a pseudo terminal
(for example `char device redirected to /dev/ttys003`), and
`build/ucf term /dev/ttys003` is the console then.

## Put it on a disk

doc/disks.md tells how the file goes onto a USB stick, a floppy disk or
a hard disk, from macOS or Linux. Then start the PC from this
disk: the boot menu of the BIOS (a key at the start: F12, F11, F8 or
Esc, as the screen says), or the order of the disks in the setup of the
BIOS. A PC with UEFI needs "CSM" or "legacy boot" in its setup, and
"secure boot" off.

## The console

The console has two sides at once:

- the keyboard and the screen;
- the serial line COM1: 115200 baud, 8 data bits, no parity, 1 stop bit.

Each output goes to both. What you type at the keyboard shows on both
(`ACCEPT` echoes it). What comes over COM1 shows on the screen, but does
not go back over COM1: the terminal shows what it sends, as for a board
(`ucf term` does so; with a terminal such as `screen` you do not see
what you type).

    ok> 3 4 + . 7
    ok> : X BEGIN AGAIN ;
    ok> X
      error: user interrupt
    ok>

- **Ctrl-C** (at the keyboard, or the byte 3 over COM1) and
  **Ctrl-Break** stop the word that runs, also a loop without output and
  a wait (`MS`). While a line is typed, Ctrl-C removes it and gives the
  prompt again.
- Backspace removes the last character. Keys without a character (F1,
  the arrows) do nothing.
- `BYE` starts the PC again.

## ucf term: the PC as a board

COM1 of the PC goes to a USB serial adapter at the host with a null modem
cable (RXD and TXD crossed). Then the tool `ucf` is the console, and
`#include` sends files to the PC, line by line after each prompt
(tools/README.md).

This example uses a MacOS host:

    $ build/ucf term /dev/cu.usbserial-1420
    term on /dev/cu.usbserial-1420: #help shows the commands
    3 4 + .
    7
    board ok> #include examples/hello.fs
    Hello from uCForth
    board ok> HELLO
    Hello from uCForth
    board ok> #bye
    term ends: back on the host

(The PC showed its first prompt before `term` started.) `#include
FILE` stops at an error and names the file and the line. Ctrl-C in
`ucf term` goes to the PC as the byte 3. Under Linux the tool is
`build/ucf-i386` or `build/ucf-a64`, under Windows `build/ucf.exe`; the
PC is the same.

## FREEZE on the PC

`FREEZE` writes the running system to the disk of the start: the 64 KB
of the VM go to the sectors 17 to 144. At the next start, the PC has the
new words:

    ok> : HELLO ." hello from the disk" CR ;
    ok> FREEZE
    ok> BYE
    ...
    ok> HELLO
    hello from the disk

A write-protected disk gives the error -37 ("file I/O error"), and the
disk stays as it was. To get the first image back, write the file to
the disk again.

## An application

An application sets the vectors `'SETUP` and `'LOOP`, then `TURNKEY`
(targets/linux-a64/README.md, section "An application"). Put the file
before `targets/freeze.fs`:

    LC_ALL=C awk -f bootstrap/awk/ucf.awk -- lib/package.fs lib/pc/bios.fs examples/tick.fs targets/freeze.fs host/files.fs targets/pc-bios/build.fs </dev/null

The PC then starts `RUN` after 1 s:

    autostart in 1 s (Ctrl-C: prompt)
    start
    tick 1
    tick 2

In this second, ESC or Ctrl-C gives the prompt instead of `RUN`. Later,
Ctrl-C stops `RUN` with `error: user interrupt`.

## The HAL of the PC: SYSCALL and ports

The package BIOS (`lib/pc/bios.fs`, in the image) gives the primitives
of this target their names, as the package DOS does on DOS:

| No. | Word | Stack | Effect |
| --- | --- | --- | --- |
| 100 | `SYSCALL` | `( x1 ... xn n id -- x )` | x1 to xn go to AX BX CX DX SI DI (n at most 6), then `INT id`; x is AX after it |
| | `REGS` | `( -- a )` | the registers after `SYSCALL` at the VM address $FFF0: AX BX CX DX SI DI, the flags (bit 0: the carry), ES |
| | `BIOS` | `( x1 ... xn n id -- ax f )` | `INT id`; f true if the carry is set (an error) |
| 110 | `PC@` | `( port -- c )` | a byte from a port: `IN AL,DX` |
| 111 | `PC!` | `( c port -- )` | a byte to a port: `OUT DX,AL` |
| 112 | `PW@` | `( port -- x )` | a word from a port: `IN AX,DX` |
| 113 | `PW!` | `( x port -- )` | a word to a port |

DS and ES are the segment of the VM during the call, so an address in a
register is a VM address. The call runs on a stack of its own, not on
the data stack of the VM. Examples:

    ok> ALSO BIOS
    ok> 0 $12 BIOS DROP .
    639

The memory below 640 KB, in KB (INT 12h). The speaker of the PC over
the ports: the timer, channel 2, makes the tone; port 61h switches it
on and off:

    ok> : TONE ( hz -- )   >R  $34DE $12 R> UM/MOD NIP  $B6 $43 PC!  DUP $42 PC!  8 RSHIFT $42 PC!  $61 PC@ 3 OR $61 PC! ;
    ok> : QUIET ( -- )   $61 PC@ $FC AND $61 PC! ;
    ok> 440 TONE  500 MS  QUIET

`$34DE $12` is 1193182 as two cells: the clock of the timer in Hz. The
clock of the PC (INT 1Ah, function 02h: the hour in CH, the minute in
CL, as BCD):

    ok> : .BCD ( x -- )   DUP 4 RSHIFT 48 + EMIT  15 AND 48 + EMIT ;
    ok> : .TIME ( -- )   $0200 1 $1A BIOS 2DROP  REGS 4 + @  DUP 8 RSHIFT .BCD  ':' EMIT  255 AND .BCD ;
    ok> .TIME
    22:43

The drivers of the PC belong in Forth over these words (doc/vm.md,
section 8).

## How it works

1. The BIOS loads the first sector of the disk to 0000:7C00 and runs
   it. It is the boot sector of `bootsect.fs`, and at the same time the
   boot sector of the FAT12 file system (its BPB describes the disk).
2. The boot sector loads the code to 0800:0000 and the memory of the VM
   to 1000:0000, then jumps to the code. A floppy disk it reads by
   cylinder, head and sector (CHS), with the form from the BPB. A hard
   disk or a USB stick it reads by the number of the sector (LBA), when
   the BIOS has the extensions of INT 13h (since about 1998); else by
   CHS, with the form that the BIOS tells.
3. START (`hal.fs`) sets the vectors of three interrupts (the timer,
   COM1, Ctrl-Break), sets up COM1 and the timer, then runs `BOOT` of
   the image.

The registers of the VM are those of DOS (`kernels/8086.fs`): SI is IP,
BX is W, SP the data stack, BP the return stack; DS, ES and SS are the
segment of the VM, so a VM address is the address for the processor.

- **The time:** the timer of the PC (channel 0) interrupts 1000 times a
  second; `MILLIS` counts these interrupts. Each 55 ms the interrupt
  goes on to the handler of the BIOS, as before: its clock and the motor
  of the floppy disk drive work as usual. `MS` waits with `HLT` (the
  processor sleeps until the next interrupt).
- **COM1:** the interrupt of the UART (IRQ 4) puts each byte that comes
  into a buffer of 256 bytes; the byte 3 is the break.
- **The break:** the interrupt of COM1, the interrupt of the timer (it
  looks for Ctrl-C in the buffer of the keyboard) and INT 1Bh
  (Ctrl-Break) only switch the table of the primitives: each primitive
  is BREAK now. So the next primitive runs BREAK, where the VM is
  complete (as on DOS). BREAK writes a new line if the line is not
  empty.
- **Calls of the BIOS** (the screen INT 10h, the disk INT 13h, the
  keyboard INT 16h, `SYSCALL`) run on a stack of their own, from
  0000:7C00 down: the data stack of the VM has only 512 bytes, and a
  BIOS can need more.
- **FREEZE** writes the 64 KB of the VM to the disk of the start, in the
  same way as the boot sector reads (function 43h, else 03h).

## Test

The tests go over COM1, as for the Raspberry Pi: uCForth on the host
sends the test files with the package SEND (`host/send.fs`), each line
after the prompt. The serial line of QEMU is the pair of FIFOs
`build/ser.in` and `build/ser.out`. On a Mac with QEMU, in the folder
`ucf`, after the build of `build/ucf-macos-a64`:

    mkfifo build/ser.in build/ser.out
    qemu-system-i386 -drive file=build/ucf-pc.img,format=raw,if=floppy -display none -no-reboot -serial pipe:build/ser &
    build/ucf-macos-a64 < targets/pc-bios/test.fs > build/test.log; echo "status $?"
    tail -c 80 build/test.log; kill %1; rm build/ser.in build/ser.out

In Docker, with the image `ucf-qemu` (doc/qemu.md) and
`build/ucf-linux-a64`:

    docker run --rm -v "$PWD":/u -w /u ucf-qemu bash -c '
      mkfifo build/ser.in build/ser.out
      qemu-system-i386 -drive file=build/ucf-pc.img,format=raw,if=floppy -display none -no-reboot -serial pipe:build/ser &
      ./build/ucf-linux-a64 < targets/pc-bios/test.fs > build/test.log; echo "status $?"
      tail -c 80 build/test.log; kill $!; rm build/ser.in build/ser.out'

Expected, after about 20 s: `status 0` and `all: 1110 tests, 0 errors`
(t0 183, hayes 872, the package BIOS 10: `lib/pc/test/bios.fs`; then
TEXT, the assembler I8086 and `CODE` words 14: `lib/pc/test/asm.fs`;
then the monitor 31: `asm/monitor/test/monpc.fs`).

**Tested (2026):** in QEMU 8.0 (SeaBIOS 1.16, on the Mac) and QEMU 10.0
(in Docker): all tests; the start from a floppy disk, an IDE hard disk,
a USB stick (EHCI) and on an ISA PC with a 486 (`-M isapc -cpu 486`),
each with `FREEZE` and a new start; a write-protected disk (-37); the
keyboard (keys from the monitor of QEMU): the echo, Backspace, Ctrl-C,
Ctrl-Break, also in `MS`; COM1: Ctrl-C (the byte 3), `ucf term` with
`#include`; a PC without COM1; an application with `TURNKEY`; the disk
as a floppy disk with FAT12 (macOS mounts it, `fsck_msdos` finds no
error, a file on it stays after `FREEZE`); `dd` to a disk of 8 MB with
the commands of doc/disks.md, then the start from it as a USB stick.
The six bootstrap VMs and DOS (FreeDOS 1.3 in QEMU) build the same
file. Not yet tested: a real PC.

## Test on a real PC

The test needs a PC with a BIOS, or a newer PC with UEFI and "CSM" (or
"legacy boot"), and a USB stick or a floppy disk. uCForth writes only
to this disk; the other disks of the PC stay as they are.

1. Write `build/ucf-pc.img` to the stick or the floppy disk
   (doc/disks.md). This erases the stick.
2. Start the PC from it (section "Put it on a disk"). Expected: `ok>`
   on the screen.
3. A sum, then Ctrl-C in a loop without end:

       ok> 3 4 + . 7
       ok> : X BEGIN AGAIN ;  X

   Press Ctrl-C. Expected: `error: user interrupt`, then `ok>`.
4. The BIOS: the memory of the PC below 640 KB, in KB (often 639 or
   640):

       ok> ALSO BIOS  0 $12 BIOS DROP .

5. `FREEZE` and a new start (section "FREEZE on the PC"): after the
   new start, the new word is there.
6. With a null modem cable at COM1 (section "ucf term: the PC as a
   board"): `ucf term PORT`, then `3 4 + .` and `#include
   examples/hello.fs`.

Please report the PC (maker and model), BIOS or UEFI with CSM, stick or
floppy disk, and the result of each step. What a finding can mean:

| Finding | Possible cause |
| --- | --- |
| The PC does not start from the stick | the BIOS starts a stick only in one mode ("USB-FDD" or "USB-HDD"): choose the other one in the setup; a PC with UEFI needs CSM and Secure Boot off |
| `uCForth: disk error` | the boot sector could not read uCForth from the disk; after a key, the BIOS looks for a disk to start again |
| The prompt, but no key works | the keyboard is at USB and the BIOS has no "USB legacy support": switch it on in the setup |
| Ctrl-C does not stop `X` | the timer of the PC does not come (please report) |
| `FREEZE` gives the error -37 | the disk is write-protected |

## Memory

The processor runs in real mode: an address is a segment and an offset,
the physical address is segment * 16 + offset, at most 1 MB.

| Physical address | Size | Contents |
| --- | --- | --- |
| `$00000` | 1 KB | the vectors of the interrupts |
| `$00400` | 256 bytes | the data of the BIOS |
| `$00500`-`$07BFF` | 30 KB | the stack of the boot sector, then of the calls of the BIOS (from `$7C00` down); at `$600` the variables of the boot sector |
| `$07C00` | 512 bytes | the boot sector |
| `$08000` | 8 KB | the code (segment `$0800`): the jump to START, the table of the primitives, the kernel, the HAL (3.3 KB of the 8 KB) |
| `$0A000` | 512 bytes | a copy of the table (for the break), the buffer of COM1 |
| `$10000` | 64 KB | the memory of the VM (segment `$1000`): doc/vm.md, section 3; at `$FFF0` the registers after `SYSCALL` |
| `$20000`-`$9FFFF` | up to 512 KB | free: uCForth does not use it |
| `$A0000`-`$FFFFF` | 384 KB | the memory of the screen, the ROMs of the BIOS |

So the PC needs 128 KB of RAM. After the start, with the image of the
section "Build" (base and the package BIOS):

| Area | Size | Free |
| --- | --- | --- |
| dictionary: words, `ALLOT`, `,` | 58 KB | 49 KB |
| RAM data: `VARIABLE`, `VALUE`, `BUFFER:`, vocabularies | 3.5 KB | 3.4 KB |
| data stack, return stack | 256 cells each | |
| the input line | 256 characters | |

`UNUSED U.` shows the free bytes of the dictionary,
`ALSO INTERNALS  DATA-END VP @ - U.` those of the RAM data. As on DOS, a
large buffer goes into the dictionary (`CREATE BUF 4000 ALLOT`), and
`ALLOT` takes at most 32767 bytes at a time (targets/dos/README.md,
section "Memory"). doc/vm.md, section 3, has the other limits of the VM.

## Limits

- No files: source text comes over COM1 (`ucf term`, `#include`) or from
  the keyboard. The FAT12 file system of the disk is for later.
- The memory above `$20000` is free, but no word of uCForth reaches it
  yet.
- `ERASE-IMAGE` does nothing: `FACTORY` gives the prompt of the image
  of the last `FREEZE`.
- The disk has the form of a floppy disk of 1.44 MB. A PC with only an
  older drive (360 KB, 1.2 MB) needs another form; not yet.
- An interrupt of the hardware writes its bytes below the top of the
  data stack, as on DOS: with more than about 200 cells on the data
  stack, it could change the return stack.
- `MS` and `US` wait at least as long as they should, in steps of 1 ms.
- A PC with UEFI and without CSM does not start it.
- Not yet tested on a real PC.
