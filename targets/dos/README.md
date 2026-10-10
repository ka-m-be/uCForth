# dos: uCForth for MS-DOS

The result is `build/ucf.com`: one program for DOS with the virtual
machine in 8086 machine code and the image. It runs on each PC with
MS-DOS or PC-DOS 2.0 or newer, FreeDOS or DR-DOS, from the IBM PC (8088)
on, and in the DOS box of 32-bit Windows (XP, 7). It needs 128 KB of
free memory: 64 KB for the program and 64 KB for the memory of the VM.

The same kernel without an image is the bootstrap VM for DOS: a PC that
has only DOS makes it from a text file with DEBUG
(`bootstrap/dos/README.md`).

## Build

In the folder `ucf`, with a bootstrap VM with 16-bit cells (the
default; the build makes a 16-bit image):

    LC_ALL=C awk -f bootstrap/awk/ucf.awk -- host/files.fs targets/dos/build.fs </dev/null

(12 s; the C bootstrap VM `build/ucf-c` takes less than 1 s, Node.js
1 s, Python 10 s.) The output ends with:

      saved: ucf.hex
      code: 6194 bytes at $100, image: 12310 bytes
      written: build/ucf.com (18504 bytes), build/kernel.com (6194 bytes),
      build/ucf.scr (402 lines), build/ucf.lst (1033 lines of source)

`build.fs` does three things:

1. `FREEZE`: the running system is the image: `base.fs`, the package
   FILES and each file before `build.fs` on the command line. It goes to
   `ucf.hex`.
2. The assembler `asm/8086.fs` makes the code at address $100 (where DOS
   loads a `.COM` program): the kernel `kernels/8086.fs` (NEXT and the
   primitives) and `hal.fs` (the start, the console, files and images
   through DOS). `build.fs` loads the two files line by line and notes
   the address of each line, for the listing.
3. It writes four files:

| File | Contents |
| --- | --- |
| `build/ucf.com` | the kernel, then the image: the program for DOS |
| `build/kernel.com` | the kernel alone: the bootstrap VM without Genesis (it needs `-image FILE`) |
| `build/ucf.scr` | the kernel as a script for DEBUG: `DEBUG < UCF.SCR` makes the same file |
| `build/ucf.lst` | the listing: for each source line the address and the bytes |

The kernel, the script and the listing do not depend on the image, so
`bootstrap/dos/` keeps a copy of `ucf.scr` and `ucf.lst`. After a
change of the kernel or of `hal.fs`, copy them again:

    cp build/ucf.scr build/ucf.lst bootstrap/dos/

DOS can build the same files itself: a DOS computer with the files of
uCForth runs the same command with the bootstrap VM for DOS (section
"Test", step 5). The files are the same, byte for byte.

## Run

Copy `build/ucf.com` to the DOS computer (a disk, a USB stick, the
serial line) and start it:

    C:\UCF>UCF
    ok> 3 4 + .
    7
    ok> BYE
    C:\UCF>

DOS reads the line, with its keys for editing: Backspace, F3 (the last
line again), Esc (the line goes away). `BYE` ends the program with the
exit status 0 (`ERRORLEVEL`), `(BYE)` with another one.

The package FILES is in the image; names of files take `/` or `\`, and
each part of a name has at most 8 characters and 3 after the point:

    ok> ALSO FILES  INCLUDE examples/hello.fs
    Hello from uCForth

The input can come from a file, and the output can go to a file or to
the serial line, with the redirection of DOS:

    C:\UCF>UCF < CMDS.TXT > COM1

At the end of such a file, UCF ends when `QUIT` waits for the next line.

**Ctrl-Break** stops the word that runs, also a loop without output:

    ok> : X BEGIN AGAIN ;  X
      error: user interrupt
    ok>

**Ctrl-C** does the same when DOS sees it: at the keyboard, and while
the program writes or reads. In the input line, Ctrl-C (and
Ctrl-Break) removes the line and gives the prompt again.

## An application

An application sets the vectors `'SETUP` and `'LOOP`, then `TURNKEY`
(targets/linux-a64/README.md, section "An application"). Put the file
before `build.fs` on the command line:

    LC_ALL=C awk -f bootstrap/awk/ucf.awk -- host/files.fs examples/tick.fs targets/dos/build.fs </dev/null

`UCF` then starts `RUN` after 1 s:

    autostart in 1 s (Ctrl-C: prompt)
    start
    tick 1
    tick 2

In this second, Ctrl-Break gives the prompt instead of `RUN`. Later,
Ctrl-Break stops `RUN` (also in the middle of `1000 MS`) and gives the
prompt with `error: user interrupt`.

## FREEZE on DOS

`FREEZE` writes the running system to `UCF.HEX` in the current folder.
The kernel starts it again:

    C:\UCF>KERNEL -image UCF.HEX

The bootstrap VM for DOS (`bootstrap/dos/`) is this kernel. With the
files of uCForth on the DOS computer, it also builds a new `UCF.COM`
with the image (section "Test", step 5).

## The HAL of DOS: SYSCALL and ports

The package DOS (`lib/dos/dos.fs`) gives the primitives of this target
their names:

| No. | Word | Stack | Effect |
| --- | --- | --- | --- |
| 100 | `SYSCALL` | `( x1 ... xn n id -- x )` | x1 to xn go to AX BX CX DX SI DI (n at most 6), then `INT id`; x is AX after it |
| | `REGS` | `( -- a )` | the registers after `SYSCALL` at the VM address $FFF0: AX BX CX DX SI DI, the flags (bit 0: the carry), ES |
| | `DOS` | `( x1 ... xn n -- ax f )` | `INT 21h`; f true if the carry is set (an error of DOS) |
| 110 | `PC@` | `( port -- c )` | a byte from a port: `IN AL,DX` |
| 111 | `PC!` | `( c port -- )` | a byte to a port: `OUT DX,AL` |
| 112 | `PW@` | `( port -- x )` | a word from a port: `IN AX,DX` |
| 113 | `PW!` | `( x port -- )` | a word to a port |

DS and ES are the segment of the VM during the call, so an address in
DX, SI or DI is a VM address. Examples:

    ok> ALSO FILES  S" lib/dos/dos.fs" INCLUDED
    ok> $3000 1 DOS DROP  DUP 255 AND . 8 RSHIFT .
    7 10
    ok> $0200 0 0 'A' 4 DOS 2DROP CR
    A
    ok> $21 PC@ .
    184

The first line is the version of DOS (FreeDOS says 7.10), the second
writes the character A (DOS function 02h, the character in DL), the
third reads the mask of the interrupt controller. The drivers of the
PC (the serial ports, the timer, the speaker) belong in Forth over
these words (doc/vm.md, section 8).

## The kernel

`build/ucf.lst` shows each line of `kernels/8086.fs` and `hal.fs` with
its address and bytes, for example NEXT at the end of the primitive `+`:

    03E8  58 5A 01 D0 50 AD   32 CODE +         AX POP,  DX POP,  AX DX ADD,  AX PUSH,  NEXT,
    03EE  93 8B 3F D1 E7 2E
    03F4  FF A5 03 01

DEBUG shows the same code: `DEBUG KERNEL.COM`, then `U 3E8 3F7`:

    27F0:03E8 58                POP     AX
    27F0:03E9 5A                POP     DX
    27F0:03EA 01D0              ADD     AX,DX
    27F0:03EC 50                PUSH    AX
    27F0:03ED AD                LODSW
    27F0:03EE 93                XCHG    AX,BX
    27F0:03EF 8B3F              MOV     DI,[BX]
    27F0:03F1 D1E7              SHL     DI,1
    27F0:03F3 2EFFA50301        JMP     WORD PTR CS:[DI+0103]

The segment (27F0 here) depends on the memory that DOS gives.

asm/README.md explains how to read the source. The registers of the
VM: SI is IP, BX is W, SP the data stack (the stack of the CPU), BP the
return stack; DS, ES and SS are the segment of the VM, so a VM address
is the address for the processor.

| Part | Where | Contents |
| --- | --- | --- |
| the jump to START | `kernels/8086.fs`, $100 | DOS starts a `.COM` program at its first byte |
| OPTAB | $103 | the address of the code of each primitive, 128 words |
| NEXT, the primitives 0-28 and 32-66 | `kernels/8086.fs` | the virtual machine; 66: `CODE` words (`lib/pc/asm.fs`) |
| the console 29-31 | `hal.fs` | the lines of the files of the command line, then the keyboard |
| files 89-93, 98, 99 | `hal.fs` | through the handles of DOS; a fid is 10 to 29 as in the C bootstrap VM |
| images | `hal.fs` | Intel HEX: read at the start (`-image`), written by `SAVE-IMAGE` and `-dump` |
| START | `hal.fs` | the memory, the command line, Ctrl-Break, the image, then `BOOT` |

## Test

The tests run in FreeDOS in QEMU (doc/qemu.md, section "MS-DOS: FreeDOS
in QEMU"): C: is `build/dos`, D: the folder `ucf`. Build first, then:

    cp build/ucf.com build/kernel.com build/dos/
    cp bootstrap/dos/ucf.scr build/dos/UCF.SCR
    printf 'ALSO FILES INCLUDE tools/test.fs\r\n' > build/dos/TEST.TXT
    printf 'BYE\r\n' > build/dos/BYE.TXT

Write the commands of a step to `build/dos/GO.BAT` (with CR LF), then
start QEMU with the command of doc/qemu.md. The output comes on the
terminal of the Mac.

1. All tests with `UCF.COM`:

       D:
       C:\UCF.COM < C:\TEST.TXT > COM1
       C:\DEBUG < C:\EXIT.SCR

   The output ends with `all: 2742 tests, 0 errors`, 5 s with the start
   of FreeDOS.

2. The bootstrap VM from the reference seed: its base image must be the
   reference, byte for byte:

       D:
       C:\KERNEL.COM -image seed\core16.hex forth\base.fs -dump D:\B16.HEX < C:\BYE.TXT > COM1
       FC /B D:\B16.HEX D:\SEED\BASE16.HEX > COM1
       C:\DEBUG < C:\EXIT.SCR

   `FC` says `No differences`.

3. All tests with the bootstrap VM, from the seed with the primitives
   0-65:

       D:
       C:\KERNEL.COM -image BOOTST~1\DOS\SEED.HEX forth\base.fs host\files.fs tools\test.fs > COM1

   On D: the folder `bootstrap` has the short name `BOOTST~1`
   (doc/qemu.md). Expected: as in step 1.

4. The fixed point: `ALSO FILES INCLUDE tools/genesis.fs` as input for
   `UCF.COM` gives twice "the same".

5. DOS builds UCF.COM itself, the same as the Mac:

       D:
       C:\KERNEL.COM -image BOOTST~1\DOS\SEED.HEX forth\base.fs host\files.fs targets\dos\build.fs < C:\BYE.TXT > COM1
       FC /B D:\BUILD\UCF.COM C:\UCF.COM > COM1

   `FC` says `No differences`, also for `KERNEL.COM`, `UCF.SCR` and
   `UCF.LST`.

6. The script for DEBUG: bootstrap/dos/README.md, section "Test".

7. The package DOS: `ALSO FILES S" forth/test/tester.fs" INCLUDED`, then
   `ALSO FILES S" lib/dos/test/dos.fs" INCLUDED` gives `dos: 7 tests, 0
   errors`.

8. `CODE` words and the monitor (`lib/pc/asm.fs`,
   `asm/monitor/README.md`): a file `C:\MON.TXT` with one line for each
   file, `ONLY FORTH ALSO FILES S" name" INCLUDED`, for
   `forth/test/tester.fs`, `lib/text.fs`, `asm/8086.fs`,
   `lib/pc/asm.fs`, `lib/pc/test/asm.fs`, `asm/monitor/monitor.fs`,
   `asm/monitor/disx86.fs`, `asm/monitor/monpc.fs` and
   `asm/monitor/test/monpc.fs`, then `BYE`:

       D:
       C:\UCF.COM < C:\MON.TXT > COM1
       C:\DEBUG < C:\EXIT.SCR

   Expected: `asm: 14 tests, 0 errors` and `monpc: 31 tests, 0 errors`;
   the segment of the VM in the line of `.REGS` (`DS=` and `ES=`) is
   where DOS put it, for example 3246.

Write the files of a test to D: (the folder `ucf`), not to C:. QEMU
writes a file of more than a few KB wrong back into `build/dos`
(doc/qemu.md); on D: DOS sees its files correctly until QEMU ends.

## Memory

| Segment and address | Contents |
| --- | --- |
| code, $0000-$00FF | the PSP of DOS; the command line at $80 |
| code, $0100 | the kernel: 6 KB |
| code, after the kernel | the image in `UCF.COM` (build.fs) |
| code, $F000-$F5FF | buffers: the input line, the output line, a file name, an image line, the files of the command line, the fids, a copy of OPTAB for Ctrl-Break |
| code, $FE00-$FFFF | the stack of DOS at the start |
| VM (code + $1000), $0000-$FFFF | the memory of the virtual machine (doc/vm.md, section 3); $FFF0-$FFFF: the registers of `SYSCALL` |

So `UCF.COM` needs two segments, 128 KB of the 640 KB of DOS; with
less, it says "not enough memory: UCF needs 128 KB". After the start,
with the image of `build.fs` (base and FILES):

| Area | Size | Free |
| --- | --- | --- |
| dictionary: words, `ALLOT`, `,` | 58 KB | 48 KB |
| RAM data: `VARIABLE`, `VALUE`, `BUFFER:`, vocabularies | 3.5 KB | 2.1 KB |
| data stack, return stack | 256 cells each | |
| the input line | 250 characters from the keyboard, 256 from a file | |

The RAM data are small with 16-bit cells: a large buffer goes into the
dictionary (`CREATE BUF 4000 ALLOT`), not into the RAM data
(`4000 BUFFER: BUF` gives the error -8). `ALLOT` takes a number with a
sign: with 16-bit cells at most 32767 bytes at a time (a larger number
is negative, and `ALLOT` does not check it); a larger buffer takes two
`ALLOT`s.
`UNUSED U.` shows the free bytes of the dictionary,
`ALSO INTERNALS  DATA-END VP @ - U.` those of the RAM data. doc/vm.md,
section 3, has the other limits of the VM.

## Limits

- `MILLIS` counts in steps of 55 ms (the clock of the BIOS); `MS` and
  `US` wait at least as long as they should, in steps of 55 ms.
- `ARG` (97) and the serial HAL (94-96) are not there. The serial ports
  work through `PC@` and `PC!` (a driver in Forth).
- An input line has at most 250 characters from the keyboard and 256
  from a file.
- An interrupt of the PC writes its bytes below the top of the data
  stack: with more than about 230 cells on the data stack, it could
  change the return stack.
- `OPEN-FILE` with R/W reads and writes at the same place in the file
  (DOS has one file pointer); with W/O it writes at the end.
- In FreeDOS 1.3 in QEMU, input over the serial line does not come in:
  `CTTY COM1` and `COPY COM1 FILE` wait, although the BIOS (INT 14h)
  gets the bytes. Output to COM1 works. On MS-DOS this is not tested.
