# uefi-x64: uCForth for a PC with UEFI, without an operating system

The result is `build/ucf-x64.efi`: one program file (PE32+, an "EFI
application") with the virtual machine in x86-64 machine code and the
image. The firmware of the computer (UEFI) starts it from a USB stick,
without an operating system. It runs on each PC with UEFI and an x86-64
processor (since about 2010), and on a Mac with Intel.

The kernel is `kernels/x64.fs`, the same as for Linux on x86-64 and for
macOS on Intel; the HAL calls the firmware: its console (keyboard and
screen), its clock, the file system of the stick. A PC with a BIOS
(older ones, or a newer one with CSM) takes `targets/pc-bios` instead.

## Build

In the folder `ucf`, with a bootstrap VM with 32-bit cells:

    build/ucf-c -32 host/files.fs targets/uefi-x64/build.fs </dev/null

The output ends with:

      saved: ucf.hex
      code: 8817 bytes, image: 86824 bytes
      written: build/ucf-x64.efi (271872 bytes)

Each bootstrap VM builds the same file, byte for byte. `build.fs` does
three things:

1. `FREEZE`: the running system is the image: `base.fs`, the package
   FILES and each file before `build.fs` on the command line.
2. The assembler `asm/x64.fs` makes the code: the kernel
   `kernels/x64.fs` and `hal.fs` (the start, the console, time, files,
   Ctrl-C, `FREEZE`).
3. It writes the PE32+ file (package PE, `lib/windows/pe.fs`, with
   `EFI-START`): the headers, the code, then the memory of the virtual
   machine (256 KB) with the image.

## A USB stick

1. Format a USB stick with FAT32 (it erases the stick). On a Mac, with
   the name of the stick from `diskutil list` (doc/disks.md tells how
   to find the right one):

       diskutil eraseDisk FAT32 UCFORTH MBRFormat /dev/disk4

   On Windows: right click on the stick, "Format...", FAT32. On Linux:
   `mkfs.fat -F 32` on its partition.
2. Copy the program to the folder `EFI/BOOT` of the stick, with the name
   `BOOTX64.EFI`: the firmware starts this file from a removable disk.

       mkdir -p /Volumes/UCFORTH/EFI/BOOT
       cp build/ucf-x64.efi /Volumes/UCFORTH/EFI/BOOT/BOOTX64.EFI

3. For `INCLUDE` and the tests, copy the folders of `ucf` too: they go
   to the root of the stick.

       cp -R forth lib host tools asm examples /Volumes/UCFORTH/

4. Eject the stick (`diskutil eject /dev/disk4`).

## Start

Plug the stick into the PC and switch it on. Open the boot menu with
the key that the PC shows at the start (often F12, F11, F8 or Esc; on a
Mac with Intel: hold the Option key), and choose the stick (on some PCs
it is called "UEFI: ..."). The screen gets empty, then:

    ok>

**Secure Boot** must be off: the program has no signature from
Microsoft, and a PC with Secure Boot does not start it. Switch it off in
the setup of the firmware (often F2 or Del at the start). A Mac with
the T2 chip (2018 to 2020) needs, in the "Startup Security Utility" of
the recovery system, "No Security" and "Allow booting from external
media".

- The console is the keyboard and the screen of the firmware. If the
  firmware has a serial line as its console too (QEMU, some servers),
  uCForth works there as well.
- Ctrl-C stops the word that runs now and gives the prompt again; it
  also ends a wait (`MS`) at once.

      ok> : X BEGIN AGAIN ;  X
        error: user interrupt
      ok>

- Files: the root of the stick is the folder of the files. `/` and `\`
  both separate the folders:

      ok> ALSO FILES  INCLUDE examples/hello.fs

- `FREEZE` writes the memory of the virtual machine back into the
  program file on the stick: at the next start, the new words are
  there. The program as it was: copy `build/ucf-x64.efi` again.
- `BYE` (and `n (BYE)`) goes back to the firmware: it starts the next
  disk, or shows its menu.

## An application

Files between `host/files.fs` and `build.fs` go into the program. With
`TURNKEY` it starts by itself after 1 s (ESC or Ctrl-C in this second
gives the prompt); an application may define `ON-BOOT` (the program
starts with it instead of `BOOT`). The example `examples/tick.fs`
counts the seconds:

    build/ucf-c -32 host/files.fs examples/tick.fs targets/uefi-x64/build.fs </dev/null

The file is always `build/ucf-x64.efi`.

## Test on a real PC or Mac

This test needs a USB stick and a PC with UEFI and an x86-64
processor, or a Mac with Intel. uCForth writes only to the stick (step
6); the disks of the computer stay as they are.

1. Make the stick (section "A USB stick"), with the folders of `ucf`.
2. Switch Secure Boot off and start from the stick (section "Start").
   Expected: the screen gets empty, then `ok>`.
3. A sum, then Ctrl-C in a loop without end:

       ok> 3 4 + . 7
       ok> : X BEGIN AGAIN ;  X

   Press Ctrl-C. Expected: `error: user interrupt`, then `ok>`.
4. The clock: `5000 MS` waits 5 seconds (count along).
5. All tests, with the files of the stick:

       ok> ALSO FILES  INCLUDE tools/test.fs

   Expected, at the end: `all: 10979 tests, 0 errors`. Then the tests
   end uCForth with `BYE`, and the firmware comes back: start from the
   stick again.
6. `FREEZE` and a new start:

       ok> : HI ." hello from the stick" CR ;
       ok> FREEZE
       ok> BYE

   `BYE` goes back to the firmware. Start from the stick again: `HI`
   writes `hello from the stick`. Copy `build/ucf-x64.efi` to the stick
   again to get the first program back.

Please report the computer (maker and model; for a Mac, the model and
the year), the result of each step, and a photo of the screen when a
step does not work. What a finding can mean:

| Finding | Possible cause |
| --- | --- |
| The stick is not in the boot menu | the stick has no FAT32, or the file is not `EFI/BOOT/BOOTX64.EFI`; or the PC starts only in the mode for a BIOS (CSM): then use `targets/pc-bios` |
| A message about Secure Boot or a signature | Secure Boot is still on |
| An empty screen, no prompt | the program stopped before the prompt |
| The prompt, but the keys do nothing | the firmware gives no keys: try another USB port of the computer, not a hub |
| Ctrl-C does not stop `X` | the timer event of the firmware does not come |
| `5000 MS` takes much more or much less than 5 s | the clock of the processor (RDTSC) does not have a constant rate (processors before about 2008) |
| `INCLUDE` gives an error | the firmware gives no file system for the stick: the program runs, but without files |
| `FREEZE` gives an error | the stick is write-protected, or the firmware does not write the program file |

## Test in QEMU

QEMU emulates a PC, and OVMF is the UEFI of the project EDK2 for it. A
Docker image with both (`ucf-qemu` from `doc/qemu.md`, then OVMF):

    printf 'FROM ucf-qemu\nRUN apt-get update && apt-get install -y --no-install-recommends ovmf\n' |
      docker build -t ucf-qemu-efi -

A folder of the Mac is the stick (QEMU shows it as a disk with FAT):

    mkdir -p build/stick/EFI/BOOT
    cp build/ucf-x64.efi build/stick/EFI/BOOT/BOOTX64.EFI
    cp -R forth lib host tools asm examples build/stick/
    docker run --rm -it -v "$PWD":/u ucf-qemu-efi sh -c 'cp /usr/share/OVMF/OVMF_VARS_4M.fd /tmp/vars.fd &&
      qemu-system-x86_64 -machine q35 -m 512 \
        -drive if=pflash,format=raw,readonly=on,file=/usr/share/OVMF/OVMF_CODE_4M.fd \
        -drive if=pflash,format=raw,file=/tmp/vars.fd \
        -drive format=raw,file=fat:rw:/u/build/stick -display none -serial mon:stdio'

After a few seconds the prompt comes over the serial line, in the
terminal. Then:

    ok> ALSO FILES  INCLUDE tools/test.fs

The output ends with `all: 10979 tests, 0 errors`, after about 8 s (in
Docker on a Mac with M4; the start of the firmware takes 2 s more).
Ctrl-A X ends QEMU. For `FREEZE`, take a disk
image with FAT32 instead of the folder (`-drive format=raw,file=...`):
QEMU writes a large file into a folder only badly.

**Tested (2026):** in QEMU 10 with OVMF (EDK2 2025.02), Docker on the
Mac: all tests from the stick (a folder of the Mac, and a disk image
with FAT32); the console over the serial line, and with the keyboard
and the screen (keys and screen pictures over the monitor of QEMU);
Ctrl-C over both (a word in a loop, `MS`, with `CATCH`: -28); `MS`,
`US`, `MILLIS`; a file that is not there (-38), a wrong fid (-37),
`MOVE` outside the memory of the VM (-9); `FREEZE`, then a new start
with the new word, and `fsck.fat` finds no error on the disk image;
`BYE` back to the firmware; `examples/tick.fs` with `TURNKEY`. Not yet
tested: a real PC, a Mac with Intel.

## Memory

UEFI loads the program at an address A of its own choice; the code
finds everything relative to itself. The firmware maps the memory 1:1:
the addresses are the physical addresses.

| Address | Size | Contents |
| --- | --- | --- |
| A | 512 bytes | the headers of the file |
| A + `$1000` | 60 KB | the code (8.6 KB): the table of the primitives, the kernel, the HAL; the firmware may make it read only |
| A + `$10000` | 256 KB | the memory of the virtual machine: VM address 0, the image (doc/vm.md, section 3) |
| A + `$50000` | 16 KB | the scratch area of the HAL: the break cell, the keys (a ring buffer of 1 KB), the open files, the names of files |
| from UEFI | at least 128 KB | the stack of the processor (the firmware gives it) |

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
the other limits of the VM. At most 16 files are open at once; a file
name has at most 255 characters.

An address outside the memory of the virtual machine is not checked,
as on a board: `@` and `!` reach the memory of the PC (also the memory
of the firmware, and the registers of devices). Only `MOVE`, `FILL`,
`READ-FILE`, `WRITE-FILE` and `READ-LINE` check their area (-9).

## How it works

UEFI calls the program as a function, with the calling convention of
Microsoft: the arguments in RCX, RDX, R8 and R9, a fifth on the stack,
the stack aligned to 16 bytes. The firmware keeps RBX, RBP, RDI, RSI and
R12 to R15: so the virtual machine (R12 IP, RBX W, R13 the data stack,
RBP the return stack, R15 the memory, R14 the table) stays in its
registers. The firmware is a set of tables of functions: the HAL calls
`call [rax+n]` (`EFI,` in `hal.fs`).

- **The start:** no watchdog (the firmware would restart the PC after 5
  minutes), an empty screen, the clock, the timer for the keys, the file
  system of the disk that the program came from (`LoadedImage`,
  `SimpleFileSystem`), and the name of the program file.
- **The keys:** a timer event of the firmware calls ON-TIMER every
  10 ms. It reads each key that is there (`ReadKeyStroke`) into a ring
  buffer; `?RX` takes them from there. Ctrl-C sets the cell `BREAK-OFF`:
  the dispatch of the next primitive runs BREAK. So Ctrl-C also stops a
  word that reads no key.
- **The screen:** `TX!` gives each byte to `OutputString` (LF as CR LF).
- **The time:** `MILLIS`, `MS` and `US` count the clock of the processor
  (RDTSC). At the start, the function `Stall` of the firmware waits
  10 ms; so the HAL knows the ticks for each millisecond.
- **Files:** `Open`, `Read`, `Write`, `Close` of the file system;
  `CREATE-FILE` deletes an old file first. A fid is 10 + the place in a
  table of the HAL (the files of UEFI have addresses of 64 bits).
- **`FREEZE`:** `SAVE-IMAGE` opens the program file (its name comes
  from the device path of the program) and writes the 256 KB of the VM
  to the place where the file holds them.

## Limits

- `ARG` gives no argument (u = 0): a program from the boot menu has no
  command line.
- No serial lines (94-96), so no tool `ucf` here.
- Secure Boot must be off.
- The time needs a clock of the processor with a constant rate (each
  processor since about 2008).
- An address outside the memory of the VM is not checked (above).
- `READ-LINE` reads byte for byte from the firmware: `INCLUDE` of a large
  file is slower than on Linux.
