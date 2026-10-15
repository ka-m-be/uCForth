# QEMU: tests on other computers and operating systems

QEMU emulates a whole computer: the processor, the memory, and devices
such as a serial line, a screen and disks. It starts a program as the
real computer does: a kernel without an operating system (bare metal),
or a complete operating system from a disk image.

Docker (`doc/docker.md`) runs only Linux programs. Take QEMU for:

- a kernel without an operating system: the Raspberry Pi 4
  (`targets/rpi4/`), a PC, the ESP32 chips (section "The ESP32 in
  QEMU");
- an operating system that is not Linux, or a Linux with its own
  kernel.

All examples here run in the folder `ucf`.

## QEMU in Docker

On the Mac, QEMU runs well in a Docker image. Make it once:

    printf 'FROM debian:trixie-slim\nRUN apt-get update && apt-get install -y --no-install-recommends qemu-system-arm qemu-system-x86 qemu-system-misc\n' |
      docker build -t ucf-qemu -

The image has 533 MB. A test:

    docker run --rm ucf-qemu qemu-system-aarch64 --version

The first line of the output:

    QEMU emulator version 10.0.13 (Debian 1:10.0.13+ds-0+deb13u1)

| Debian package | QEMU programs | Examples of machines |
| --- | --- | --- |
| `qemu-system-arm` | `qemu-system-aarch64`, `qemu-system-arm` | `raspi4b`, `raspi3b`, `virt` |
| `qemu-system-x86` | `qemu-system-x86_64`, `qemu-system-i386` | `pc`: a PC with BIOS |
| `qemu-system-misc` | `qemu-system-riscv64`, `-avr`, `-m68k`, `-s390x`, `-xtensa` and more | `virt` (RISC-V), `arduino-uno` (ATmega328P) |

For the Raspberry Pi alone, the package `qemu-system-arm` is enough.

Each QEMU program lists its machines with `-M help`:

    docker run --rm ucf-qemu qemu-system-aarch64 -M help | grep raspi

Output:

    raspi0               Raspberry Pi Zero (revision 1.2)
    raspi1ap             Raspberry Pi A+ (revision 1.1)
    raspi2b              Raspberry Pi 2B (revision 1.1)
    raspi3ap             Raspberry Pi 3A+ (revision 1.0)
    raspi3b              Raspberry Pi 3B (revision 1.2)
    raspi4b              Raspberry Pi 4B (revision 1.5)

QEMU can also run directly on the Mac (Homebrew, MacPorts). Look at the
version first: QEMU 8 does not have the machine `raspi4b`, QEMU 10 has
it.

## A kernel without an operating system: the Raspberry Pi 4

`build/kernel8.img` (`targets/rpi4/README.md`) in the emulated Pi 4:

    docker run --rm -it -v "$PWD":/u ucf-qemu qemu-system-aarch64 -M raspi4b -nographic -no-reboot \
      -device loader,file=/u/build/kernel8.img,addr=0,force-raw=on

| Option | Effect |
| --- | --- |
| `-M raspi4b` | the machine: a Raspberry Pi 4 B |
| `-nographic` | no window: the serial line (UART) of the Pi is the terminal |
| `-no-reboot` | a restart ends QEMU, so `BYE` ends it |
| `-device loader,file=...,addr=0,force-raw=on` | put the file into the memory at address 0, byte for byte; all four processor cores start there |

In the terminal:

    ok> 3 4 * . 12
    ok> BYE

Ctrl-A X ends QEMU at once. Ctrl-A C changes between the terminal and
the monitor of QEMU. In the monitor, `info registers` shows the
registers of each core, and `quit` ends QEMU:

    (qemu) info registers

    CPU#0
     PC=00000000000420d0 X00=000000000000001d X01=00000000000420c8
    ...

## The serial line as a pipe: tests without a person

With `-serial pipe:build/ser`, the serial line of the emulated computer
is a pair of FIFOs: QEMU reads `build/ser.in` and writes
`build/ser.out`. A second program writes the input and reads the
output. The test of the Pi uses this: uCForth for Linux sends the test
files, one line after each prompt (package SEND), in the same
container:

    docker run --rm -v "$PWD":/u -w /u ucf-qemu bash -c '
      mkfifo build/ser.in build/ser.out
      qemu-system-aarch64 -M raspi4b -display none -no-reboot \
        -device loader,file=build/kernel8.img,addr=0,force-raw=on -serial pipe:build/ser &
      ./build/ucf-linux-a64 < targets/rpi4/test.fs > build/test.log; echo "status $?"
      tail -c 80 build/test.log; kill $!; rm build/ser.in build/ser.out'

The output ends with:

    ok> ALL-RESULTS all: 1058 tests, 0 errors

`build/ucf-linux-a64` runs in the container without an emulator: the
container and the program are both for 64-bit ARM.

## Another operating system

Example: Alpine Linux for x86-64 (a PC), emulated on the Mac. The CD
image of the form "virt" of Alpine is small (68 MB), and its console is
the serial line. Load it once:

    curl -o build/alpine.iso https://dl-cdn.alpinelinux.org/alpine/v3.22/releases/x86_64/alpine-virt-3.22.6-x86_64.iso

Start it:

    docker run --rm -it -v "$PWD":/u ucf-qemu qemu-system-x86_64 -m 512 -nographic -boot d \
      -cdrom /u/build/alpine.iso -virtfs local,path=/u,mount_tag=ucf,security_model=none,readonly=on

| Option | Effect |
| --- | --- |
| `-m 512` | 512 MB of memory |
| `-boot d` | start from the CD |
| `-cdrom /u/build/alpine.iso` | the CD image |
| `-virtfs local,path=/u,mount_tag=ucf,...` | give the folder `/u` (the folder `ucf` of the Mac) to the guest system with the name `ucf`, read only (the protocol 9P) |

After less than a minute, the login comes. The user is `root`,
without a password. Then mount the folder and run a test:

    localhost login: root
    localhost:~# mount -t 9p -o trans=virtio ucf /mnt && cd /mnt && uname -m
    x86_64
    localhost:/mnt# time sh -c "LC_ALL=C awk -f bootstrap/awk/ucf.awk -- forth/test/tester.fs forth/test/t0.fs </dev/null | tail -2"
    t0: 183 tests, 0 errors
    ok> 
    real	2m 4.57s
    user	2m 3.30s
    sys	0m 0.18s
    localhost:/mnt# poweroff

`poweroff` stops the guest system, then QEMU ends. The same test in a
container for `linux/amd64` takes 12 s (`doc/docker.md`): QEMU emulates
the whole PC here, so a Linux program is faster in Docker.

The same guest system runs `build/ucf-linux-i386`, a program with 32
bits, on its kernel with 64 bits (start QEMU without `readonly=on`: the
tests write files):

    localhost:/mnt# echo "ALSO FILES  INCLUDE tools/test.fs" | ./build/ucf-linux-i386 | tail -1
    all: 12186 tests, 0 errors
    localhost:/mnt# echo "ALSO FILES  INCLUDE tools/test.fs" | ./build/ucf-linux-x64 | tail -1
    all: 12186 tests, 0 errors

So a real Linux kernel checks the signals and the calls of the system,
which QEMU in Docker only emulates.

For each other operating system the steps are the same: an image of
its disk or CD, a console (the serial line, or a window without
`-nographic`), and a way for the files: a shared folder (`-virtfs`, if
the guest system knows 9P), a disk image, or the network.

## MS-DOS: FreeDOS in QEMU

FreeDOS is a free DOS for the PC; its programs and calls are the same
as in MS-DOS. In QEMU it is the test bench for `targets/dos/` and for
`bootstrap/dos/`. The console of DOS goes to the serial line (the DOS
command `CTTY COM1`), so the output comes to the terminal of the Mac,
and a test runs without a person. QEMU runs directly on the Mac here
(MacPorts, QEMU 8.0); the Docker image above works in the same way,
with `/u/` in front of the paths.

Load the parts once, into `build/` (not in Git):

    mkdir -p build/freedos build/dos
    cd build/freedos
    curl -LO https://www.ibiblio.org/pub/micro/pc-stuff/freedos/files/distributions/1.3/official/FD13-FloppyEdition.zip
    curl -LO https://www.ibiblio.org/pub/micro/pc-stuff/freedos/files/repositories/1.3/base/debug.zip
    curl -LO https://www.ibiblio.org/pub/micro/pc-stuff/freedos/files/repositories/1.3/base/mode.zip
    unzip -j FD13-FloppyEdition.zip 144m/x86BOOT.img && mv x86BOOT.img boot.img
    perl -0777 -pi -e 's/MENUDEFAULT=1,10/MENUDEFAULT=1,0 /; s/if exist SETUP.BAT CALL SETUP.BAT BOOT/if exist C:\\GO.BAT CALL C:\\GO.BAT     /' boot.img
    cd ../dos
    unzip -j ../freedos/debug.zip BIN/DEBUG.COM
    unzip -j ../freedos/mode.zip BIN/MODE.COM
    printf 'o f4 0\r\nq\r\n' > EXIT.SCR
    cd ../..

`boot.img` is the start disk of FreeDOS 1.3 (1.44 MB). As it comes, it
waits 10 s for a language and then starts the installation. The line
with `perl` changes two lines on the disk, each with the same number of
bytes: no wait, and instead of the installation the batch file
`C:\GO.BAT`. The disk has the kernel, the shell `COMMAND.COM` and `FC`
(it compares files); `DEBUG` and `MODE` come from the packages of
FreeDOS. `EXIT.SCR` is a script for DEBUG: `O F4 0` writes 0 to the port
$F4, and QEMU ends (see `isa-debug-exit` below).

`build/dos` is the drive C: of the DOS computer: QEMU shows the folder of
the Mac as a disk with the file system FAT. What DOS writes there comes
into the folder. The folder `ucf` is drive D:, read only: DOS may write
there, but the changes go away when QEMU ends. A name longer than 8
characters (3 after the point) gets a short name: the folder
`bootstrap` is `BOOTST~1` on D:. The files that DOS loads have short
names (`doc/bootstrap.md`).

`build/dos/GO.BAT` holds the commands of the test, with CR LF at the end
of each line. An example:

    printf 'CTTY COM1\r\nVER\r\nD:\r\nDIR /B\r\nC:\\DEBUG < C:\\EXIT.SCR\r\n' > build/dos/GO.BAT

Then start QEMU:

    qemu-system-i386 -display none -no-reboot \
      -drive file=build/freedos/boot.img,format=raw,if=floppy,snapshot=on \
      -drive file=fat:rw:build/dos,format=raw,if=ide,index=0 \
      -drive file=fat:.,format=raw,if=ide,index=1,snapshot=on \
      -serial stdio -device isa-debug-exit,iobase=0xf4,iosize=0x04 -boot a

| Option | Effect |
| --- | --- |
| `-display none` | no window; the screen of the PC stays empty after `CTTY COM1` |
| `if=floppy,snapshot=on` | the start disk as drive A:; changes go away at the end |
| `fat:rw:build/dos` | the folder as a disk with FAT, drive C:; DOS writes into the folder |
| `fat:.` with `snapshot=on` | the folder `ucf` as drive D:, read only for the Mac |
| `-serial stdio` | the serial line COM1 is the terminal of the Mac |
| `-device isa-debug-exit,...` | a byte x to the port $F4 ends QEMU with the exit status 2x + 1 |
| `-boot a` | start from the floppy disk |

After 3 to 4 s the output ends with:

    FreeCom version 0.85a - WATCOMC - XMS_Swap [Jul 10 2021 19:28:06]
    ...
    -o f4 0

and QEMU ends with the exit status 1. Without `EXIT.SCR` at the end,
QEMU waits for the next command on COM1; Ctrl-A X ends it.

Two things do not work in this test bench (FreeDOS 1.3, QEMU 8.0):

- **Input over COM1.** `CTTY COM1` sends the output of DOS to the
  terminal, but DOS does not read the keys from there; `COPY COM1 FILE`
  waits too. The BIOS gets the bytes (INT 14h in DEBUG shows them), so
  the fault is in the serial driver of FreeDOS. A test gives its input
  in files: on the command line of the program, or with `<`.
- **Large files on C:.** QEMU writes a file of more than one cluster
  (about 16 KB) wrong back into `build/dos`: parts of it come twice.
  DOS sees the same wrong file. A test writes its files to D:; there
  they are right until QEMU ends. `FC /B` compares them in DOS, and
  `TYPE FILE > COM1` sends a text file to the Mac.

Keys come from the monitor of QEMU: start QEMU with `-monitor
unix:build/mon.sock,server,nowait`, then send for example `sendkey 3`,
`sendkey ret` or `sendkey ctrl-pause` (Ctrl-Break) with
`nc -U build/mon.sock`. So a test types at the keyboard of DOS while
the output comes on COM1 (`UCF > COM1`).

`asm/README.md` builds a small program `HELLO.COM` with the 8086
assembler and runs it here. `targets/dos/README.md` and
`bootstrap/dos/README.md` show the tests of uCForth for DOS.

## A PC without an operating system

`targets/pc-bios` makes `build/ucf-pc.img`, a disk that the BIOS starts.
QEMU takes it as a floppy disk, a hard disk or a USB stick:

| Disk | Options |
| --- | --- |
| floppy disk A: | `-drive file=build/ucf-pc.img,format=raw,if=floppy` |
| hard disk C: | `-drive file=build/ucf-pc.img,format=raw,if=ide` |
| USB stick | `-device usb-ehci,id=ehci -drive if=none,id=stick,format=raw,file=build/ucf-pc.img -device usb-storage,bus=ehci.0,drive=stick` |

More options:

| Option | Effect |
| --- | --- |
| (none) | a window with the screen of the PC; the keyboard types there |
| `-display none -serial mon:stdio` | no window: COM1 is the terminal; Ctrl-C goes to the PC, Ctrl-A X ends QEMU |
| `-display none -serial pty` | COM1 is a pseudo terminal for `ucf term` (QEMU says its name) |
| `-display none -serial pipe:build/ser` | COM1 is the pair of FIFOs `build/ser.in` and `build/ser.out`: the tests |
| `-no-reboot` | QEMU ends when the PC starts again (`BYE`) |
| `-M isapc -cpu 486` | an older PC: ISA instead of PCI, a 486 |

Not `-nographic`: with it, the BIOS of QEMU (SeaBIOS) also writes the
text of the screen to COM1, so each character comes twice.

Without a window, the monitor of QEMU types and reads the screen: start
QEMU with `-monitor tcp:127.0.0.1:4444,server,nowait`, then send lines
with `nc 127.0.0.1 4444`: `sendkey 3`, `sendkey ret`, `sendkey ctrl-c`,
`sendkey ctrl-pause` (Ctrl-Break) type; `xp /2000hx 0xb8000` shows the
text of the screen (80 x 25, each character with its color in a word);
`screendump FILE.ppm` makes a picture of it.

`targets/pc-bios/README.md` shows the tests (sections "Run in QEMU" and
"Test").

## A PC with UEFI

A newer PC has UEFI instead of a BIOS. For QEMU, OVMF is the UEFI of
the project EDK2; Debian has it in the package `ovmf`:

    printf 'FROM ucf-qemu\nRUN apt-get update && apt-get install -y --no-install-recommends ovmf\n' |
      docker build -t ucf-qemu-efi -

OVMF is two files of flash memory: its code (read only) and its
variables (a copy for each start). A folder of the Mac becomes a disk
with FAT (`fat:rw:FOLDER`); OVMF starts `EFI/BOOT/BOOTX64.EFI` from it:

| Option | Effect |
| --- | --- |
| `-machine q35 -m 512` | a PC with the chipset Q35 and 512 MB |
| `-drive if=pflash,format=raw,readonly=on,file=/usr/share/OVMF/OVMF_CODE_4M.fd` | the code of OVMF |
| `-drive if=pflash,format=raw,file=/tmp/vars.fd` | its variables: a copy of `/usr/share/OVMF/OVMF_VARS_4M.fd` |
| `-drive format=raw,file=fat:rw:/u/build/stick` | the folder `build/stick` as a disk |
| `-display none -serial mon:stdio` | no window: the serial line is the terminal (OVMF uses it as a console too) |

`targets/uefi-x64/README.md` (section "Test in QEMU") has the whole
command. A large file written into such a folder disk can come out
wrong (as with FreeDOS): for `FREEZE`, take a disk image with FAT32
(`-drive format=raw,file=stick.img`; `mkfs.fat -C` and `mcopy` of the
Debian packages `dosfstools` and `mtools` make one). The monitor
(`sendkey`, `screendump`) works as for the PC with a BIOS; the screen of
UEFI is graphics, so only `screendump` shows it.

QEMU 8 of MacPorts did not start its own file `edk2-x86_64-code.fd` on
the Mac (the processor stays at the reset); QEMU 10 in Docker does.

## Machines for later targets

| Processor | QEMU program | Machine | Note |
| --- | --- | --- | --- |
| 64-bit ARM | `qemu-system-aarch64` | `raspi4b`, `raspi3b`, `virt` | `virt`: a simple computer made for emulators |
| x86, 16 to 64 bits | `qemu-system-i386`, `qemu-system-x86_64` | `pc`, `isapc` | BIOS; start from a disk, CD or floppy image; FreeDOS for `targets/dos/`, `targets/pc-bios/` (above) |
| RISC-V | `qemu-system-riscv64` | `virt` | |
| AVR | `qemu-system-avr` | `arduino-uno` | the ATmega328P of the Arduino Uno |
| Xtensa | `qemu-system-xtensa` | `lx60`, `kc705`, `sim`, `virt` | development boards of Tensilica, not the ESP32; for the ESP32 see the next section |

## The ESP32 in QEMU

The QEMU of Debian has no ESP32. Espressif has a QEMU of its own (a
fork, on GitHub as `espressif/qemu`) with the machines `esp32` and
`esp32s3`, and with the real ROM of each chip. The ROM loads the image
from a flash file, as on the board. So a test without a board can check
the kernel, the HAL, the console and `FREEZE`.

| It has | It has not |
| --- | --- |
| the CPU; UART0 to 2 with interrupts; the timer groups with their watchdogs; the SYSTIMER (ESP32-S3); the interrupt matrix; the SPI flash (a write goes back into the file); SHA; eFuse; PSRAM (`-m`) | GPIO (it reads 0, a pin gives no interrupt); the RMT; the USB of the chip; the Forth handlers of interrupts do not run (below, "What it found"). On the ESP32-S3 the registers of the clock only store their values: the CPU stays at 80 MHz for `CPU-MHZ`, and the cycle counter at 40 MHz |

**A Docker image with it** (once, about 30 MB; in the folder `ucf`):

    mkdir -p build/qemu-esp && cd build/qemu-esp
    curl -LO https://github.com/espressif/qemu/releases/download/esp-develop-9.2.2-20260417/qemu-xtensa-softmmu-esp_develop_9.2.2_20260417-aarch64-linux-gnu.tar.xz
    curl -LO https://github.com/espressif/qemu/releases/download/esp-develop-9.2.2-20260417/qemu-esp_develop_9.2.2_20260417-checksum.sha256
    grep xtensa qemu-*-checksum.sha256 | grep aarch64-linux-gnu
    shasum -a 256 qemu-xtensa-*.tar.xz
    printf 'FROM debian:trixie-slim\nRUN apt-get update && apt-get install -y --no-install-recommends libglib2.0-0t64 libpixman-1-0 libgcrypt20 libslirp0 libsdl2-2.0-0 xz-utils\nCOPY qemu-xtensa-softmmu-esp_develop_9.2.2_20260417-aarch64-linux-gnu.tar.xz /tmp/q.tar.xz\nRUN tar -xJf /tmp/q.tar.xz -C /opt && rm /tmp/q.tar.xz\nENV PATH=/opt/qemu/bin:$PATH\n' > Dockerfile
    docker build --platform linux/arm64 -t ucf-qemu-esp .
    cd ../..

The two lines after `grep` and `shasum` must show the same number (the
SHA-256 digest of the file). This is the file for a Mac with Apple
silicon; on a PC with x86-64 take `x86_64-linux-gnu` in its name and
`--platform linux/amd64`. The archive has QEMU and the ROMs of the
chips (`/opt/qemu/share/qemu`).

**The flash file** has exactly 2, 4, 8 or 16 MB. For the ESP32-S3: 16
MB of `$FF`, the image at address 0:

    dd if=/dev/zero bs=1048576 count=16 | LC_ALL=C tr '\0' '\377' > build/flash-s3.bin
    dd if=build/esp32s3.bin of=build/flash-s3.bin conv=notrunc

**Start**, with the console in the terminal (Ctrl-A X ends QEMU):

    docker run --rm -it -v "$PWD":/u ucf-qemu-esp qemu-system-xtensa -nographic -machine esp32s3 \
      -m 8M -global driver=ssi_psram,property=is_octal,value=true \
      -drive file=/u/build/flash-s3.bin,if=mtd,format=raw

The ROM shows its lines (`ESP-ROM:esp32s3-20210327`, `load:0x40378000`
...), then `ok>` comes. `-m 8M` with the next option is the PSRAM of
the board (octal); without it the board has no PSRAM. Ctrl-A C switches
to the monitor of QEMU (`info registers`, `xp /4wx ADDRESS`), Ctrl-A C
again back.

**`ucf term` with QEMU.** With `-serial pty`, QEMU gives the console as
a pseudo terminal, and the tool for Linux (`build/ucf-a64`,
`targets/linux-a64/README.md`) in the same container is the console:

    docker run --rm -it -v "$PWD":/u -w /u ucf-qemu-esp sh -c '
      qemu-system-xtensa -machine esp32s3 -display none -monitor none -serial pty -m 8M \
        -global driver=ssi_psram,property=is_octal,value=true \
        -drive file=build/flash-s3.bin,if=mtd,format=raw > /tmp/q.log 2>&1 &
      sleep 2; ./build/ucf-a64 term $(grep -o "/dev/pts/[0-9]*" /tmp/q.log)'

Then the tests, as on the board: `#include forth/test/tester.fs`,
`#include forth/test/t0.fs`, `#include forth/test/hayes.fs`, `#include
forth/test/all.fs` give `all: 1058 tests, 0 errors` after about 10 s.
`FREEZE` writes into `build/flash-s3.bin`: the next start has the new
words. `#bye` ends `term`; QEMU ends with the container.

**What it found.** QEMU keeps the RAM of the ESP32-S3 for the
instruction bus (`$4037_8000`) and for the data bus (`$3FC8_8000`)
apart; in the chip they are the same RAM. The kernel reads its table
of primitives on the data bus. So `targets/esp32s3/build.fs` writes the
code twice, once for each bus (a fourth segment in the image; in the
chip the second copy writes the same bytes again), and `FREEZE` does the
same. Code that the chip runs from the data RAM (`CODE` words,
`targets/esp32s3/README.md`) does not run in QEMU.

The Forth handlers of interrupts (package INTR) do not run in QEMU: as
soon as a source asks, the board stops. So the tests of INTR and TIMERS
and `EVERY` hang there, on both machines. On the machine `esp32` the
registers that tell which sources ask (`$3FF0_00EC` ...) read 0, and so
does a FROM_CPU register after a write (`$3FF0_00DC`): the kernel finds
no source. The console (UART0 on a CPU interrupt of its own) and Ctrl-C
work.

**The ESP32** (`targets/esp32`) has its own machine: `-machine esp32`,
a flash file of 4 MB with the image at `$1000`:

    dd if=/dev/zero bs=1048576 count=4 | LC_ALL=C tr '\0' '\377' > build/flash-32.bin
    dd if=build/esp32.bin of=build/flash-32.bin bs=4096 seek=1 conv=notrunc
    docker run --rm -it -v "$PWD":/u ucf-qemu-esp qemu-system-xtensa -nographic -machine esp32 \
      -drive file=/u/build/flash-32.bin,if=mtd,format=raw

Its ROM runs in QEMU (`ets Jul 29 2019`), then `ok>` comes. There the
CPU clock follows the registers, as on the chip: `ALSO CLOCK  CPU-MHZ
.` gives 240. With `-serial pty` and `build/ucf-a64` as above (with
`-machine esp32` and this flash file), t0, hayes and `all.fs` give
`all: 1058 tests, 0 errors` in 3 s; `FREEZE` writes the flash file, and
the next start has the new words. No PSRAM (the module WROOM-32 has
none).

## Limits

- QEMU runs the instructions of another processor one after the other
  in software: 10 to 50 times slower than the real computer (the t0
  test above: 2.6 s on the Mac, 2 min in the emulated PC).
- A machine of QEMU is not the real computer: it has fewer devices, and
  some work in a different way (the firmware and the GPU of the Pi, the
  timing). A test in QEMU does not replace the test on the hardware.
