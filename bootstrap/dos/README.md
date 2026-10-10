# dos: the bootstrap VM for DOS, made with DEBUG

This folder brings uCForth to a PC that has only DOS: no compiler, no
other program. DOS has DEBUG, a small monitor for machine code. DEBUG
reads the text file `UCF.SCR` and writes the program `UCF.COM`: the
virtual machine of uCForth in 8086 machine code. `UCF.COM` then loads
the seed, compiles `base.fs`, and uCForth runs.

| File | Contents |
| --- | --- |
| `ucf.scr` | the script for DEBUG: the bytes of `UCF.COM` as text, 16 on each line, and a check |
| `ucf.lst` | the listing: each line of the source with its address and bytes |
| `seed.hex` | the seed: the image after `forth/core.fs`, with the primitives 0 to 65 |

`UCF.COM` is a bootstrap VM without Genesis (bootstrap/README.md): it
does not read `core.fs`; it starts from the seed. It is the kernel of
the DOS target (targets/dos/README.md) without an image.

## What the PC needs

- DOS 2.0 or newer with DEBUG: MS-DOS or PC-DOS up to 6.22, FreeDOS
  (DEBUG is a package there), or the DOS box of 32-bit Windows (XP, 7).
  DR-DOS has SID instead of DEBUG; it is not tested.
- 128 KB of free memory (`MEM` or `CHKDSK` shows it).
- The files: `UCF.SCR` and `SEED.HEX` from here, and the folder `ucf`
  of uCForth with its subfolders (`FORTH`, `HOST`, `LIB`, `TOOLS`, `ASM`
  for the tests, `SEED`, `KERNELS` and `TARGETS` for the build). Each
  name has at most 8 characters, and 3 after the point.

## Bring the files to the PC

All files are text. Take the way that the PC has:

1. **A diskette or a USB stick** (the BIOS of newer PCs shows a stick
   as a drive): copy the files on another computer, then on the PC:

       C:\>MD UCF
       C:\>COPY A:\*.* C:\UCF

2. **The serial line**, with a null modem cable from another computer.
   On the PC, for each file:

       C:\UCF>MODE COM1:2400,N,8,1
       C:\UCF>COPY COM1 UCF.SCR

   DOS writes what comes until Ctrl-Z (the byte 26). The other computer
   sends the file, then this byte. 2400 baud is safe: DOS reads the
   line through the BIOS, without a buffer. In FreeDOS 1.3 in QEMU this
   way does not work (targets/dos/README.md, "Limits").

3. **Typing** `UCF.SCR` (402 lines of hex digits) is possible, but a
   version for paper is not there yet.

## Make UCF.COM

    C:\UCF>DEBUG < UCF.SCR

DEBUG reads the script and shows each line. The last lines:

    -G=1932
    UCF.COM: the check sum is right.
    Unexpected breakpoint interrupt
    AX=0924 BX=0000 CX=1832 DX=195D SP=FFFE BP=0000 SI=1932 DI=0000
    DS=27D8 ES=27D8 SS=27D8 CS=27D8 IP=195D NV UP EI PL ZR NA PE NC
    27D8:195D 55                PUSH    BP
    -W
    Writing 1832 bytes
    -Q

`UCF.COM` now has 6194 bytes ($1832). If a byte of the script is wrong
(a fault on the serial line, a typing error), the check says so, and
`UCF.COM` gets 0 bytes:

    UCF.COM: a byte is wrong; UCF.COM gets 0 bytes. Check the script.

## Start uCForth

The seed and `base.fs`, then the keyboard:

    C:\UCF>UCF -image SEED.HEX FORTH\BASE.FS
    uCForth - DOS bootstrap VM - 16 bit - 122 words in FORTH - image SEED.HEX
    uCForth - DOS bootstrap VM - 16 bit - 163 words in FORTH - built in 329 ms
    ok> 3 4 + .
    7

The time is from FreeDOS in QEMU on a Mac; an IBM PC with 4.77 MHz
needs some minutes (an estimate, not measured).

The first file after the seed must be `FORTH\BASE.FS`: when it is
complete, the kernel completes the base (doc/genesis.md, section 6).
More files load after it, then the keyboard:

    C:\UCF>UCF -image SEED.HEX FORTH\BASE.FS HOST\FILES.FS MYAPP.FS

Keep the result: `FREEZE` writes the system to `UCF.HEX`, and the next
start takes it at once:

    ok> FREEZE
      saved: UCF.HEX
    ok> BYE
    C:\UCF>UCF -image UCF.HEX

With the files of uCForth, the PC also builds `UCF.COM` with the image
inside, the program of targets/dos/README.md:

    C:\UCF>UCF -image SEED.HEX FORTH\BASE.FS HOST\FILES.FS TARGETS\DOS\BUILD.FS

It writes `BUILD\UCF.COM` (the folder `BUILD` must be there). This
file is the same, byte for byte, as the one the Mac builds.

The options of UCF:

| Option | Effect |
| --- | --- |
| `-image FILE` | load this image (Intel HEX, doc/vm.md, section 10) |
| `-dump FILE` | with a seed: write the base image to FILE when `base.fs` is complete |
| `FILE ...` | these files load line by line, then the keyboard |

## Test

1. All tests: `UCF -image SEED.HEX FORTH\BASE.FS HOST\FILES.FS
   TOOLS\TEST.FS`. The last line: `all: 2742 tests, 0 errors`.
2. The base image must be the reference, byte for byte: with the pure
   seed of the project (`SEED\CORE16.HEX`, only the 32 mandatory
   primitives):

       C:\UCF>UCF -image SEED\CORE16.HEX FORTH\BASE.FS -dump B16.HEX
       uCForth - DOS bootstrap VM - 16 bit - 119 words in FORTH - image SEED\CORE16.HEX
       uCForth - DOS bootstrap VM - 16 bit - 163 words in FORTH - built in 494 ms
       ok> BYE
       C:\UCF>FC /B B16.HEX SEED\BASE16.HEX
       Comparing B16.HEX and SEED\BASE16.HEX
       No differences

3. The script: `FC /B UCF.COM KERNEL.COM` with `build/kernel.com` from
   another computer gives `No differences`.

On the Mac these tests run in FreeDOS in QEMU (targets/dos/README.md,
section "Test"; doc/qemu.md).

## The script, line by line

    N UCF.COM
    E 0100 E9 1A 17 24 02 38 02 47 02 55 02 6D 02 7E 02 8B
    ...
    E 19C2 0A 24
    G=1932
    W
    Q

| Command | Effect |
| --- | --- |
| `N UCF.COM` | the name of the file for `W` |
| `E 0100 E9 1A 17 ...` | enter these bytes at the address 0100, and on |
| `G=1932` | run the program at 1932: the check after the kernel |
| `W` | write CX bytes from 0100 to the file |
| `Q` | end DEBUG |

The check (its source is at the end of targets/dos/build.fs) adds the
bytes of the kernel: sum = sum rotated left by 1, plus the byte. It
compares the sum with the sum on the build computer, shows its message
and puts the length of the kernel into CX, or 0. Then `INT 3` gives the
control back to DEBUG.

## How UCF.COM works

`ucf.lst` shows each part with its address; the source is
`kernels/8086.fs` (the virtual machine) and `targets/dos/hal.fs` (the
start, the console, files, images). asm/README.md explains how to read
the assembler. The parts in the order of the program:

| Address | Part | What it does |
| --- | --- | --- |
| 0100 | the jump to START | DOS starts a `.COM` program at 0100 |
| 0103 | OPTAB | the address of the code of each primitive, 128 words |
| 0224 ... | the primitives | DOCOL first; each one ends with NEXT: `LODSW`, `XCHG AX,BX`, `MOV DI,[BX]`, `SHL DI,1`, `JMP CS:[DI+0103]` |
| after them | the HAL | `?RX` and `TX!` (the console), the files, `SAVE-IMAGE`, `SYSCALL`, the ports |
| near the end | the loader | reads the Intel HEX records of the image, checks each sum |
| the end | START | 64 KB for the VM, the command line, Ctrl-Break, the image, then `BOOT` |

The registers of the VM: SI is IP (the next cell of the word that
runs), BX is W (the word that runs), SP is the data stack, BP the
return stack. DS, ES and SS point to the segment of the VM, so a VM
address is an address for the processor. The calls of DOS are
`INT 21h` with the function in AH, as a DOS handbook describes them.

## Traps

- **8.3 names:** a name of a file or folder has at most 8 characters,
  and 3 after the point. DOS cuts a longer name, and the file is not
  found. A folder `bootstrap` on a disk of another computer has a
  short name such as `BOOTST~1`; `DIR` shows it.
- **CR LF:** the script needs CR LF at the end of each line. The files
  of uCForth may have LF or CR LF: the kernel takes both.
- **`/` and `\`:** the files of uCForth use `/` in names; the kernel
  gives DOS `\`. On the command line of DOS, write `\`.
- **The DOS box of Windows:** only 32-bit Windows up to Windows 7 has
  it. A 64-bit Windows runs no DOS programs.
- **FreeDOS:** DEBUG is not on the start disk; it is the package
  `debug` of FreeDOS (doc/qemu.md).
