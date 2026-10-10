# targets: uCForth as a program for each platform

Each folder here makes one complete uCForth for one platform: a program
file or a boot image with the console. Only the parts for this one
platform are here; the parts that more targets use are in other folders:

| Folder | Contents |
| --- | --- |
| `asm/` | an assembler for each processor |
| `kernels/` | the virtual machine in machine code for each processor |
| `lib/` | packages such as `ihex.fs` (images); `lib/linux/elf.fs`, `lib/macos/macho.fs` and `lib/windows/pe.fs` (program files); `lib/esp32/` (the ESP32); `lib/pc/` (a PC without an operating system) |
| `host/` | `image.fs` (read `ucf.hex`, write the file), `send.fs` (tests over a serial line) |
| `targets/freeze.fs` | `FREEZE` for a target without files: before `host/files.fs` on the command line |
| `targets/NAME/` | `hal.fs` (the start, console, time, files of this platform), `build.fs` (the tool), `README.md` |

The process goes like this for most targets:

- make the HAL primitives (hardware dependent) for the target using the `asm`
- make the kernel with the `asm`
- get the *image* (the compiled dictionary of uCForth: `FREEZE`), 16 or 32 bit
- combine them all with the proper header or frame of an executable for the target
  e.g. `elf` for Linux, boot code for a board, etc.
- write it all into the final file in `build/`

The target `browser` has no machine code: its virtual machine is the
JavaScript bootstrap VM, in one HTML file with the image.

Each README shows how to build, run and test the target, and how an application
goes into it (`RUN`, `TURNKEY`). A bootstrap VM with 32-bit cells builds each target,
one with 16-bit cells the targets `dos` and `pc-bios`; the target `browser` takes both.
The targets for x86-64 (`linux-x64`, `macos-x64`, `uefi-x64`) share the
kernel `kernels/x64.fs`; their code does not depend on its address. The
targets `esp32s3` and `esp32` share the kernel `kernels/xtensa.fs` and
the packages of `lib/esp32/`.
The section "Memory" of each README gives the addresses and how much
memory is free for a program; doc/vm.md, section 3, the limits that
all targets share.

doc/start.md shows the first builds step by step (a program for the
computer, the tool `ucf`, the image of a board), and all build commands
in one table.

| Target | Result | Status |
| --- | --- | --- |
| `linux-a64/` | Linux on 64-bit ARM: an ELF program; the tool `ucf` as `ucf-a64`; the network (lib/net) | ready; tested in Docker (Debian 12 arm64), not yet on Raspberry Pi OS |
| `linux-i386/` | Linux on a PC (x86, 32-bit program; also on Linux with 64 bits): an ELF program with the kernel of `win32`; the tool `ucf` as `ucf-i386`; the network (lib/net) | ready; tested in Docker (Debian 12 i386 in QEMU) and on Alpine Linux (x86-64, in QEMU) |
| `linux-x64/` | Linux on a PC with 64 bits (x86-64): an ELF program with the kernel `x64.fs`; the tool `ucf` as `ucf-x64`; the network (lib/net) | ready; tested in Docker (Debian, `linux/amd64`) and on Alpine Linux (x86-64, in QEMU) |
| `macos-a64/` | macOS on Apple silicon: a Mach-O program with a code signature; the network (lib/net) | ready; tested on macOS 26 |
| `macos-x64/` | macOS on a Mac with Intel: a Mach-O program for x86-64 with a code signature; the tool `ucf` as `ucf-intel`; the network (lib/net) | ready; tested on macOS 26 with Rosetta (also the tool with the board), not yet on a Mac with Intel |
| `rpi4/` | Raspberry Pi 4 and 400 without an operating system: `kernel8.img` | tested in QEMU only; not tested on a real Pi (on hold until a USB serial adapter is there) |
| `esp32s3/` | ESP32-S3: an image for the RAM or the flash; the tool `ucf term` on the host is the console | ready; tested on the board (diymore N16R8), starts from the flash; also in the QEMU of Espressif (doc/qemu.md) |
| `esp32/` | ESP32 (for example in the module ESP32-WROOM-32): an image for the RAM or the flash (at `$1000`); the memory of the VM has the standard 256 KB, in two parts of the RAM, and after `FREEZE` the frozen part of the dictionary in the flash (`targets/esp32/README.md`, "Memory"); the tool `ucf term` on the host is the console | ready, without `CODE` words; tested on the board (diymore ESP-WROOM-32 N4), starts from the flash; also in the QEMU of Espressif (doc/qemu.md) |
| `dos/` | MS-DOS 2.0 or newer on a PC (8086 and later): `UCF.COM`; the kernel alone is the bootstrap VM for DOS (`bootstrap/dos/`). The build needs 16-bit cells | ready; tested in FreeDOS 1.3 in QEMU; DOS builds the same files itself |
| `pc-bios/` | a PC with a BIOS (8086 and later; a newer PC with CSM), without an operating system: `ucf-pc.img`, a floppy disk of 1.44 MB for a floppy disk, a USB stick or a hard disk; the kernel of `dos`, the HAL calls the BIOS; keyboard, screen and COM1; `FREEZE` writes to the disk. The build needs 16-bit cells | ready; tested in QEMU 8 and 10 (floppy disk, hard disk, USB stick, an ISA PC with a 486), not yet on a real PC |
| `uefi-x64/` | a PC or a Mac with UEFI (x86-64), without an operating system: `ucf-x64.efi`, the program for a USB stick with FAT32 (`EFI/BOOT/BOOTX64.EFI`); keyboard and screen of the firmware; the files of the stick; `FREEZE` writes into the program file | ready; tested in QEMU 10 with OVMF (UEFI of EDK2), not yet on a real PC |
| `win32/` | Windows on a PC (x86, 32-bit program; also on Windows with 64 bits): `ucf-win32.exe`, one program for the console; the same bytes as text for `certutil`; the tool `ucf` as `ucf.exe` | ready; tested on Windows 7 (64 bit) and in Wine (Debian 13, i386 in Docker) |
| `browser/` | a web browser: `ucf-web.html`, one file with the JavaScript bootstrap VM and the image; a double click starts uCForth, also with an application | ready; tested in Google Chrome, Firefox and Safari on macOS |
| `c64/` | C64: a module or program | planned |

## How to add a target

1. Is there an assembler and a kernel for the processor? Else write them
   first (`asm/`, `kernels/`), with tests.
2. Write `hal.fs`: the start code and the HAL of doc/vm.md, section 8,
   as small as possible. Drivers are Forth, not machine code.
3. Write `build.fs`: `FREEZE`, the code, the file format of the platform.
4. Run `tools/test.fs` on the target, and write the README.
