# rpi4: uCForth for the Raspberry Pi 4 and 400, bare metal

**Status: tested in QEMU only.** Nothing here ran on a real Pi yet: the
start at address 0 (`kernel_old=1`), the UART, the framebuffer of the
real firmware and the watchdog are not checked on the hardware. The
target is on hold until a USB serial adapter (3.3 V) is there.

The result is `build/kernel8.img`: uCForth without an operating system.
The firmware of the Pi loads it from the SD card (or a USB stick) and
starts it. The console is the serial line on the GPIO pins 14 and 15:
a USB serial adapter connects it to a terminal on the Mac.

The file has the memory from address 0: a jump to the start code, the
image at `$100`, the code (kernel and HAL) at `$40000`. The virtual
machine uses the addresses of the chip, so a program reaches the
registers of the peripherals with `@` and `!`.

## Build

In the folder `ucf`, with a bootstrap VM with 32-bit cells:

    LC_ALL=C awk -f bootstrap/awk/ucf.awk -- -32 targets/freeze.fs host/files.fs targets/rpi4/build.fs

The output ends with:

      saved: ucf.hex
      code: 9124 bytes, image: 84440 bytes
      written: build/kernel8.img (271268 bytes)

1. `targets/freeze.fs` runs `FREEZE`: the running system is the image,
   without the package FILES (the Pi has no files). Put your
   application files before it.
2. `build.fs` loads the package FILES and the tools, and makes the code:
   the kernel `kernels/aarch64.fs` and `hal.fs` (start, UART, time,
   restart).
3. It writes `kernel8.img`. The image starts with `BOOT`, or with
   `ON-BOOT` when the image has this word (next section).

## The screen (HDMI)

With five more files before `targets/freeze.fs`, the console is on the
screen too:

    LC_ALL=C awk -f bootstrap/awk/ucf.awk -- -32 lib/package.fs lib/font8.fs lib/fbcon.fs lib/rpi/bcm2711.fs targets/rpi4/screen.fs targets/freeze.fs host/files.fs targets/rpi4/build.fs

| File | Contents |
| --- | --- |
| `lib/package.fs` | the frame of a package (`PACKAGE` ... `END-PACKAGE`); here before FILES, so it comes first |
| `lib/font8.fs` | package FONT8: a font of 8 x 8 pixels (public domain) |
| `lib/fbcon.fs` | package FBCON: text on a framebuffer, `FB-EMIT` |
| `lib/rpi/bcm2711.fs` | package BCM2711: the mailbox to the GPU, `FB-ALLOC` |
| `targets/rpi4/screen.fs` | `ON-BOOT`: get a framebuffer, then each character goes to the UART and to the screen; then `BOOT` |

The framebuffer has 640 x 480 pixels: 80 characters in a line, 60
lines. The GPU scales it to the HDMI mode of the screen. At the bottom
the text goes on at the top; the new line and the one below it are
cleared first. The colors are in `FG` and `BG` (package FBCON).
Without a screen, only the UART works, as before.

In QEMU (next section), the monitor command `screendump build/screen.ppm`
saves the screen as a picture.

## Run on the Pi

**SD card.** Format it on the host as FAT32 (MS-DOS FAT). Copy these
files to it:

| File | From |
| --- | --- |
| `start4.elf`, `fixup4.dat` | the firmware of Raspberry Pi: github.com/raspberrypi/firmware, folder `boot` |
| `config.txt` | `targets/rpi4/config.txt` |
| `kernel8.img` | `build/kernel8.img` |

`config.txt` tells the firmware: 64 bit, load `kernel8.img` at address
0 (`kernel_old=1`), and put nothing else into the memory at `$100`
(`disable_commandline_tags=1`, no device tree).

**Serial line.** A USB serial adapter with 3.3 V. **The GPIO pins take only 3.3 V:** a
5-V TTL line at RXD needs a voltage divider (for example 2.2 kOhm and
3.3 kOhm); a true RS232 line (+-12 V, inverted) needs a level shifter
such as the MAX3232.

| Pi pin | Signal | Adapter |
| --- | --- | --- |
| 6 | GND | GND |
| 8 | GPIO 14, TXD | RXD |
| 10 | GPIO 15, RXD | TXD |

On the Pi 400 the GPIO pins are at the back. On the Mac:

    screen /dev/cu.usbserial-XXXX 115200

Then switch on the Pi. The terminal shows `ok> `. The terminal sends each
key at once, and uCForth shows it (`ECHO`). The return key sends CR,
backspace works.

`BYE` restarts the Pi (the watchdog).

## Run in QEMU

QEMU 10 emulates the Raspberry Pi 4 (`raspi4b`). In Docker, make an
image with QEMU once (`doc/qemu.md` has an image with QEMU for more
processors; it works here too):

    printf 'FROM debian:trixie-slim\nRUN apt-get update && apt-get install -y --no-install-recommends qemu-system-arm\n' |
      docker build -t ucf-qemu -

Then, in the folder `ucf`:

    docker run --rm -it -v "$PWD":/u ucf-qemu qemu-system-aarch64 -M raspi4b -nographic -no-reboot \
      -device loader,file=/u/build/kernel8.img,addr=0,force-raw=on

The loader puts the file at address 0; all four cores start there, as
on the Pi with `kernel_old=1`. The serial line is the terminal. `BYE`
ends QEMU (`-no-reboot`); Ctrl-A X ends it too.

## An application

An application sets the vectors `'SETUP` and `'LOOP`, then `TURNKEY`
(`examples/tick.fs`):

```forth
VARIABLE TICKS
: SETUP ( -- )   0 TICKS !  ." start" CR ;
: STEP  ( -- )   1 TICKS +!  ." tick " TICKS @ . CR  1000 MS ;
' SETUP 'SETUP !   ' STEP 'LOOP !   TURNKEY
```

Put the file before `targets/freeze.fs`:

    LC_ALL=C awk -f bootstrap/awk/ucf.awk -- -32 examples/tick.fs targets/freeze.fs host/files.fs targets/rpi4/build.fs

The Pi starts `RUN` after 1 second:

    autostart in 1 s (Ctrl-C: prompt)
    start
    tick 1
    tick 2

In this second, ESC or Ctrl-C gives the prompt instead of `RUN`.

## Test

The tests go over the serial line: uCForth for Linux
(`build/ucf-linux-a64`, see `targets/linux-a64/`) sends the test files
with the package SEND (`host/send.fs`), each line after the prompt. The
serial line of QEMU is the pair of FIFOs `build/ser.in` and
`build/ser.out`. In the folder `ucf`, after both builds:

    docker run --rm -v "$PWD":/u -w /u ucf-qemu bash -c '
      mkfifo build/ser.in build/ser.out
      qemu-system-aarch64 -M raspi4b -display none -no-reboot \
        -device loader,file=build/kernel8.img,addr=0,force-raw=on -serial pipe:build/ser &
      ./build/ucf-linux-a64 < targets/rpi4/test.fs > build/test.log; echo "status $?"
      tail -c 80 build/test.log; kill $!; rm build/ser.in build/ser.out'

Expected, after about 1 s: `all: 1055 tests, 0 errors` (t0 and hayes).

## Memory

The MMU is off: the addresses are the physical addresses of the chip,
and VM address a is the address a.

| Address | Size | Contents |
| --- | --- | --- |
| `$0` | 256 bytes | a jump to `START` (the free area of the memory map) |
| `$100`-`$3FFFF` | 256 KB | the memory of the virtual machine: the image, stacks, dictionary (doc/vm.md, section 3) |
| `$40000` | 9 KB | the code: the table of the primitives, the kernel, the HAL with the exception vectors (2 KB) |
| `$FE000000` | | the peripherals: `$FE003000` system timer, `$FE200000` GPIO, `$FE201000` UART0 |

After the start, with the image of the section "Build" (base, without
FILES):

| Area | Size | Free |
| --- | --- | --- |
| dictionary: words, `ALLOT`, `,` | 189 KB | 173 KB |
| RAM data: `VARIABLE`, `VALUE`, `BUFFER:`, vocabularies | 63 KB | 62 KB |
| data stack, return stack | 256 cells each | |
| the input line | 256 characters | |

`UNUSED U.` shows the free bytes of the dictionary,
`ALSO INTERNALS  DATA-END VP @ - U.` those of the RAM data; each file
before `targets/freeze.fs` takes its part (the five files for the screen:
4.5 KB). doc/vm.md, section 3, has the other limits of the VM.

The other RAM of the Pi is free: uCForth does not use it, but `@` and
`!` reach it below 4 GB (the addresses have 32 bits; an aligned
address), without a check. The GPU takes a part of the first GB for
itself and for the framebuffer.

## Limits

- Tested in QEMU. Not yet on a real Pi 4 or 400.
- No break: Ctrl-C stops a running word only when the word reads keys.
  (Later: the interrupt of the UART, doc/vm.md, section 9.)
- The MMU and the data cache are off: the Pi runs slower than it can.
  Each `@` and `!` needs an aligned address; else the error -9
  (invalid memory address) comes, as for an address where no memory is.
- `SAVE-IMAGE` gives -21; `FREEZE` on the Pi comes later.
- The input comes only from the UART: no USB keyboard, no serial line
  over USB yet (next: USB-C as a serial device to the Mac).
