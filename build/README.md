# build: files that the tools make

The tools write their results here. Git keeps only this file. A
bootstrap VM cannot make a folder, so this folder exists from the start.

| File | Made by |
| --- | --- |
| `ucf-c` | the C compiler, from `bootstrap/c/ucf.c`: the C bootstrap VM |
| `ucf-macos-a64` | `targets/macos-a64/build.fs`: uCForth for macOS on Apple silicon |
| `ucf` | the same build with `tools/ucf.fs`: the tool for a board (macOS) |
| `ucf-linux-a64` | `targets/linux-a64/build.fs`: uCForth for Linux on 64-bit ARM |
| `ucf-a64` | the same build with `tools/ucf.fs`: the tool for a board (Linux on 64-bit ARM) |
| `ucf-linux-i386` | `targets/linux-i386/build.fs`: uCForth for Linux on a PC (32 bit) |
| `ucf-i386` | the same build with `tools/ucf.fs`: the tool for a board (Linux on a PC) |
| `ucf-linux-x64` | `targets/linux-x64/build.fs`: uCForth for Linux on a PC with 64 bits |
| `ucf-x64` | the same build with `tools/ucf.fs`: the tool for a board (Linux on a PC with 64 bits) |
| `ucf-macos-x64` | `targets/macos-x64/build.fs`: uCForth for macOS on a Mac with Intel |
| `ucf-intel` | the same build with `tools/ucf.fs`: the tool for a board (a Mac with Intel) |
| `ucf-x64.efi` | `targets/uefi-x64/build.fs`: uCForth for a PC with UEFI, without an operating system (`EFI/BOOT/BOOTX64.EFI` on a USB stick) |
| `esp32s3.bin` | `targets/esp32s3/build.fs`: the image for the flash of an ESP32-S3 board |
| `esp32.bin` | `targets/esp32/build.fs`: the image for the flash of an ESP32 board (at `$1000`) |
| `flash-s3.bin`, `flash-32.bin` | the flash of the ESP32-S3 and of the ESP32 for QEMU (doc/qemu.md, "The ESP32 in QEMU") |
| `qemu-esp/` | the files for the Docker image with the QEMU of Espressif (doc/qemu.md) |
| `kernel8.img` | `targets/rpi4/build.fs`: uCForth for the Raspberry Pi 4 and 400, bare metal |
| `ucf.com`, `kernel.com`, `ucf.scr`, `ucf.lst` | `targets/dos/build.fs`: uCForth for MS-DOS, its kernel alone, the script for DEBUG and its listing |
| `ucf-pc.img` | `targets/pc-bios/build.fs`: uCForth for a PC with a BIOS, without an operating system: a floppy disk of 1.44 MB, also for a USB stick or a hard disk |
| `ucf-web.html` | `targets/browser/build.fs`: uCForth for a web browser, in one HTML file |
| `ucf-win32.exe`, `ucf-win32.exe.hex` | `targets/win32/build.fs`: uCForth for Windows, and the same bytes as text for `certutil` |
| `ucf.exe`, `ucf.exe.hex` | the same build with `tools/ucf.fs`: the tool for a board (Windows) |
| `test.log` | the test of the Pi or of the PC in QEMU (`targets/rpi4/README.md`, `targets/pc-bios/README.md`, "Test") |
| `ser.in`, `ser.out` | FIFOs: the serial line of QEMU while a test runs |
| `screen.ppm` | a picture of the screen of QEMU (`screendump`) |
| `dos/`, `freedos/` | the test bench with FreeDOS in QEMU (`doc/qemu.md`) |
| `stick/` | the folder that QEMU shows as a USB stick with UEFI (`targets/uefi-x64/README.md`, "Test in QEMU") |

`doc/start.md`, section "All builds and checks", has the command for
each program. Each program holds its own image. So after a change to
`forth/core.fs`, `forth/base.fs`, a package, a kernel or a bootstrap
VM, build the programs again.

The image of the last `FREEZE` is `ucf.hex` in the folder `ucf`.
