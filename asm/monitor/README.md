# asm/monitor: a monitor for the machine that uCForth runs on

A monitor looks into the machine from the inside, as DEBUG does on DOS
or the monitor in the ROM of an old computer: the registers, the code
in the memory, calls of the BIOS or of a routine. Here the monitor is a
set of Forth words. uCForth itself runs it, on the machine that you
want to look at. So you can call the BIOS of a PC with registers that
you set, and see the registers and flags that it gives back:

    ok> $12 INT-CALL  .REGS
    AX=027F BX=0000 CX=0000 DX=0000 SI=0000 DI=0000 BP=0000 DS=1000 ES=1000 FL=0202
    CS=0000 IP=0000 SS=0000 SP=0000
    NV UP EI PL NZ NA PO NC
    ok> $F000 $FFF0 1 DISASML 2DROP
    f000:fff0  ea 5b e0 00 f0          ljmp 0xf000, 0xe05b

(`INT 12h` gives the memory below 640 KB in KB: $027F is 639, in QEMU;
its BIOS keeps 1 KB for itself. At `F000:FFF0` the PC starts after a
reset. The output is from a PC without DOS in QEMU. CS IP SS SP are
those of code that waits for a step: section "Breakpoints and single
steps on a PC".)

On a board with an ESP32 or an ESP32-S3 the monitor calls the
functions of the ROM with the registers a2 to a7, and shows the
registers of the last error of the CPU:

    board ok> $10000000 @
      error: invalid memory address: $10000000 at pc $40080382
    board ok> LAST-FAULT .REGS
    PC=40080382 PS=00060030 SAR=00000018 EXCCAUSE=1C EXCVADDR=10000000
    A0=00000040 A1=3FFFFFC0 A2=10000000 A3=00010A74 A4=00000001 A5=FFFFFFFF
    ...
    INTLEVEL=0 EXCM=1 UM=1 RING=0 OWB=0 CALLINC=2 WOE=1  cause: LoadProhibited
    board ok> GET-REG PC 1 DISASM DROP
    40080382  22 22 00                l32i a2, a2, 0

So far the monitor works on a PC, with DOS (`targets/dos`) and without
an operating system (`targets/pc-bios`), on the two ESP32 boards
(`targets/esp32`, `targets/esp32s3`), and on Windows (`targets/win32`). The disassemblers for the x86,
the Xtensa and AArch64 (the 64-bit ARM) run on every system with cells
of the right size, also on the Mac, to read code from a file. The
section "Plans" lists the next machines.

## Files

| File | Package | Contents |
| --- | --- | --- |
| `monitor.fs` | MONITOR | the common part: the register image (`.REGS`, `SET-REG`, `GET-REG`), `DISASM`, the vectors for the part of each CPU and machine |
| `disx86.fs` | DISX86 | the disassembler for the x86, from the 8086 to the x86-64: 16-bit code (real mode), 32-bit and 64-bit code |
| `monpc.fs` | MON-PC | a PC in real mode: its registers, `INT-CALL`, `CALL-AT`, breakpoints and single steps, memory with segment and offset (`@L` ... `DUMPL`), `DISASML`; CODE words (`lib/pc/asm.fs`) |
| `disxt.fs` | DISXT | the disassembler for the Xtensa: the ESP32 (LX6) and the ESP32-S3 (LX7); needs cells of 32 bits |
| `monesp.fs` | MON-ESP32 | a board with an ESP32 or an ESP32-S3: its registers, `CALL-AT`, `LAST-FAULT`, breakpoints and single steps, the special registers (`SR@`, `SR!`), memory in cells (`CELL-C@`, `DUMPC`); CODE words (`lib/esp32/asm.fs`) |
| `disa64.fs` | DISA64 | the disassembler for AArch64 (the 64-bit ARM): ARMv8.0 to ARMv8.7 for whole numbers and the system; needs cells of 32 bits |
| `monwin.fs` | MON-WIN32 | Windows (32 bits): the registers of the 386, `CALL-AT` with them, memory that runs (`EXEC-ALLOT`, `PLACE-CODE`), memory at any address (`X-C@`, `DUMPX`) |
| `test/disx86.fs` | | 1233 instructions of 16-bit code with their text from llvm-objdump; runs on every system (`tools/test.fs`) |
| `test/disx32.fs` | | 1338 instructions of 32-bit code, the same way; runs on every system with cells of 32 bits |
| `test/disx64.fs` | | 1684 instructions of 64-bit code, the same way; runs on every system with cells of 32 bits |
| `test/disa64.fs` | | 2773 instructions of AArch64 with their text from llvm-objdump; runs on every system with cells of 32 bits |
| `test/monpc.fs` | | 31 tests on DOS and on a PC |
| `test/disxt.fs` | | 1372 instructions with their text from GNU objdump; runs on every system with cells of 32 bits (`tools/test.fs`) |
| `test/monesp.fs` | | 31 tests on the boards |
| `test/monwin.fs` | | 12 tests on Windows (or in Wine) |

## Load

On DOS, each file with its packages before it:

    ok> ALSO FILES  INCLUDE lib/text.fs  INCLUDE asm/8086.fs  INCLUDE lib/pc/asm.fs
    ok> INCLUDE asm/monitor/monitor.fs  INCLUDE asm/monitor/disx86.fs
    ok> INCLUDE asm/monitor/monpc.fs

On a PC without DOS, the host sends the files with `ucf term`
(`targets/pc-bios/README.md`, "ucf term"):

    ok> #include lib/text.fs
    ok> #include asm/8086.fs
    ok> #include lib/pc/asm.fs
    ok> #include asm/monitor/monitor.fs
    ok> #include asm/monitor/disx86.fs
    ok> #include asm/monitor/monpc.fs

After the load, MON-PC, MONITOR, ASM and I8086 are in the search order.
`ONLY FORTH ALSO MONITOR ALSO MON-PC` gives the words of the monitor
alone.

On a board with an ESP32 or an ESP32-S3, in `ucf term`, best right
after a start (the search order has room for 16 vocabularies; the
files take up to 15 of them while they load):

    board ok> #require FAIL lib/text.fs
    board ok> #require ADD, asm/xtensa.fs
    board ok> #include lib/esp32/asm.fs
    board ok> #include asm/monitor/monitor.fs
    board ok> #include asm/monitor/disxt.fs
    board ok> #include asm/monitor/monesp.fs

Then the search order has MON-ESP32 DISXT MONITOR ASM XTENSA TEXT. For
the examples below: `ONLY FORTH ALSO ROM ALSO CHIP ALSO XTENSA ALSO ASM
ALSO MONITOR ALSO MON-ESP32` (ROM for `>CHIP`, CHIP for the addresses
of the ROM, XTENSA for the names of the special registers).

On Windows, after the package WINDOWS and the assembler X86:

    ok> ALSO FILES  INCLUDE lib/text.fs  INCLUDE asm/x86.fs  INCLUDE lib/windows/windows.fs
    ok> INCLUDE asm/monitor/monitor.fs  INCLUDE asm/monitor/disx86.fs
    ok> INCLUDE asm/monitor/monwin.fs

Then the search order has MON-WIN32 DISX86 MONITOR WINDOWS X86 TEXT.

On another host only MONITOR and the disassemblers (DISX86, DISXT,
DISA64) load (MON-PC needs a PC, MON-ESP32 a board, MON-WIN32 Windows). The section "The disassembler on a host" shows how
to read code from a file.

## The words

| Word | Package | Effect |
| --- | --- | --- |
| `.REGS ( -- )` | MONITOR | the registers of the image, then the flags |
| `x SET-REG name ( -- )` | MONITOR | the register name of the image gets x: `$0E41 SET-REG AX` |
| `GET-REG name ( -- x )` | MONITOR | the register name of the image; upper and lower case are the same |
| `a n DISASM ( -- a' )` | MONITOR | n instructions from a; a' is the next one, so `16 DISASM` goes on |
| `xt DIS-FETCH ( -- )` | MONITOR | DISASM reads each byte of the code with xt ( a -- c ); `' C@ DIS-FETCH`: the memory of the VM, as after the load |
| `n INT-CALL ( -- )` | MON-PC | `INT n` with the registers of the image; afterwards the image holds the registers and the flags that it left |
| `seg off CALL-AT ( -- )` | MON-PC | a far call of the code at seg:off with the registers of the image; the code ends with `RETF`; afterwards as `INT-CALL` |
| `seg off @L ( -- x )`, `x seg off !L ( -- )` | MON-PC | a word in the memory of the PC |
| `seg off C@L ( -- c )`, `c seg off C!L ( -- )` | MON-PC | a byte |
| `seg off u DUMPL ( -- )` | MON-PC | u bytes from seg:off, as `DUMP` shows them |
| `seg off n DISASML ( -- seg off' )` | MON-PC | n instructions from seg:off |
| `VM-SEG ( -- seg )` | MON-PC | the segment of the VM: VM address a is VM-SEG:a |
| `seg off DEBUG-AT ( -- )` | MON-PC | the code at seg:off waits for `STEP` or `GO`, with the registers of the image and a stack of its own; it ends with `RETF` |
| `a DEBUG-AT ( -- )` | MON-ESP32 | the function at a waits for `STEP` or `GO`: a call as `CALL-AT` makes it, with a stack of its own |
| `STEP ( -- )` | MON-PC, MON-ESP32 | the code runs one instruction; then the image holds its registers, the address of the next instruction (CS:IP, PC) |
| `GO ( -- )` | MON-PC, MON-ESP32 | the code runs up to a breakpoint (its address in the image), a `BREAK` instruction (Xtensa) or to its end |
| `seg off BREAK-AT ( -- )` | MON-PC | a breakpoint at seg:off, at most 8; `GO` puts `INT 3` there while the code runs |
| `a BREAK-AT ( -- )` | MON-ESP32 | a breakpoint at a, at most 2 (the registers IBREAKA0 and IBREAKA1): also in the ROM |
| `CLEAR-BREAKS ( -- )`, `.BREAKS ( -- )` | MON-PC, MON-ESP32 | no breakpoints; show them |
| `STOPPED ( -- n )` | MON-PC, MON-ESP32 | why the code stopped: 1 after a step, 3 at a breakpoint (or `BREAK`), 0 at its end |
| `X86-16 ( -- )` | DISX86 | `DISASM` takes 16-bit code: the 8086, a PC in real mode (so after the load) |
| `X86-32 ( -- )` | DISX86 | `DISASM` takes 32-bit code: the 386 in protected mode, Windows, Linux i386 |
| `X86-64 ( -- )` | DISX86 | `DISASM` takes 64-bit code (x86-64); needs cells of 32 bits |
| `A64 ( -- )` | DISA64 | `DISASM` takes AArch64 code (so after the load) and shows each instruction as a number of 32 bits |
| `XT-ESP32 ( -- )` | DISXT | `DISASM` takes code of the ESP32 (so after the load) |
| `XT-ESP32S3 ( -- )` | DISXT | `DISASM` takes code of the ESP32-S3 (MON-ESP32 sets it on this board) |
| `a CALL-AT ( -- )` | MON-ESP32 | a call of the function at a, as the ROM and C code expect it (`CALLX8`): a2 to a7 of the image are its arguments; afterwards the image has a2 to a7 as the function left them (the result in a2), SAR, and CYCLES |
| `LAST-FAULT ( -- )` | MON-ESP32 | the image gets the registers of the last exception of the CPU |
| `n SR@ ( -- x )`, `x n SR! ( -- )` | MON-ESP32 | the special register n: `CCOUNT SR@` (the names are in XTENSA) |
| `a u DUMPC ( -- )` | MON-ESP32 | u bytes from a, as `DUMP` shows them, read in cells |
| `a CELL-C@ ( -- c )` | MON-ESP32 | a byte, read from its cell; `DISASM` reads code with it (so after the load) |
| `x1 .. xn n a CALL-AT ( -- )` | MON-WIN32 | a call of the code at the address a (for Windows) with the registers of the image and n arguments on the stack, as `SYSCALL` takes them; afterwards the image holds the registers and the flags that the code left |
| `u EXEC-ALLOT ( -- xa )` | MON-WIN32 | u bytes of memory that Windows lets run, read and write; xa is an address for Windows |
| `PLACE-CODE ( -- )` | MON-WIN32 | the code that the assembler X86 made since `ORG` goes to its place (the address of `ORG`, from `EXEC-ALLOT`) |
| `a X-C@ ( -- c )` | MON-WIN32 | a byte at a VM address or at an address for Windows; `DISASM` reads code with it (so after the load) |
| `a u DUMPX ( -- )` | MON-WIN32 | u bytes from a, as `DUMP` shows them, at a VM address or an address for Windows |
| `/LINE ( -- )` | MONITOR | `.REGS` starts a new line before the next register (for the part of a machine) |

The names `@L !L C@L C!L` are those of F-PC.

## Registers and calls on a PC

The image holds AX BX CX DX SI DI BP DS ES and the flags FL. A call
loads the registers from the image, and afterwards the image holds the
registers that the call left. DS and ES hold `VM-SEG` at the start: so
an address in BX, DX, SI or DI is a VM address, and the BIOS can read
and write a buffer of Forth. FL is only a result; a call starts with
the flags of uCForth.

`.REGS` shows the flags in a second line, as DEBUG does:

| Flag | Set | Clear |
| --- | --- | --- |
| overflow | `OV` | `NV` |
| direction | `DN` | `UP` |
| interrupt | `EI` | `DI` |
| sign | `NG` | `PL` |
| zero | `ZR` | `NZ` |
| auxiliary carry | `AC` | `NA` |
| parity | `PE` | `PO` |
| carry | `CY` | `NC` |

Examples:

    \ a character on the screen: INT 10h, AH = 0Eh, AL = the character
    ok> $0E41 SET-REG AX  0 SET-REG BX  $10 INT-CALL

    \ the ticks of the clock since midnight: INT 1Ah, AH = 0; CX:DX
    ok> 0 SET-REG AX  $1A INT-CALL  GET-REG CX U.  GET-REG DX U.
    9 52874

    \ a sector of the floppy disk into a buffer of Forth: INT 13h,
    \ AH = 02h, AL = 1 sector, CX = cylinder 0 and sector 1, DX = head 0
    \ and drive 0, ES:BX = the buffer; the carry tells an error
    ok> CREATE SECTOR 512 ALLOT
    ok> $0201 SET-REG AX  1 SET-REG CX  0 SET-REG DX  SECTOR SET-REG BX
    ok> $13 INT-CALL  GET-REG FL 1 AND .  SECTOR 32 DUMP
    0
    8F1A: EB 3C 90 55 43 46 4F 52 54 48 20 00 02 01 91 00 .<.UCFORTH .....
    8F2A: 02 E0 00 40 0B F0 09 00 12 00 02 00 00 00 00 00 ...@............

    \ a routine of your own: MOV AX,BX  STC  RETF
    ok> CREATE R1  $8B C, $C3 C, $F9 C, $CB C,
    ok> $5678 SET-REG BX  VM-SEG R1 CALL-AT  .REGS
    AX=5678 BX=5678 CX=0000 DX=0000 SI=0000 DI=0000 BP=0000 DS=1000 ES=1000 FL=0203
    CS=0000 IP=0000 SS=0000 SP=0000
    NV UP EI PL NZ NA PO CY

A call runs on a stack of its own (512 bytes), not on the data stack of
the VM. It keeps IP, RP and the data stack of the VM, and afterwards
DS and ES hold `VM-SEG` again. What else the code changes stays
changed: the memory, the vectors of the interrupts, the ports. A wrong
call can stop the PC; Ctrl-C comes only after the call.

## Breakpoints and single steps on a PC

As `T` and `G` of DEBUG: `DEBUG-AT` prepares code, which then waits;
`STEP` lets it run one instruction (with the trap flag of the CPU), `GO`
up to a breakpoint or to its end. After each stop the image holds its
registers, also CS:IP of the next instruction and SS:SP of its stack.
The code starts with the registers of the image and the flags $0202
(interrupts on), on a stack of its own (512 bytes); it ends with
`RETF`. A breakpoint is the instruction `INT 3` (the byte $CC): `GO`
puts it at each address of `BREAK-AT`, and takes it away again when the
code stops. The routine counts AX up three times:

    \ MOV CX,3  L: INC AX  LOOP L  RETF
    ok> CREATE R2  $B9 C, $03 C, $00 C,  $40 C,  $E2 C, $FD C,  $CB C,
    ok> 0 SET-REG AX  VM-SEG R2 DEBUG-AT  STEP  .REGS
    AX=0000 BX=5678 CX=0003 DX=0000 SI=0000 DI=0000 BP=0000 DS=1000 ES=1000 FL=0202
    CS=1000 IP=B2F1 SS=1000 SP=AA4C
    NV UP EI PL NZ NA PO NC
    ok> GET-REG CS GET-REG IP 1 DISASML 2DROP
    1000:b2f1  40                      inc ax
    ok> VM-SEG R2 3 + BREAK-AT  GO  STOPPED .  GET-REG AX .  GET-REG CX .
    3 1 2
    ok> GO GET-REG AX .  GO STOPPED .  GET-REG AX .
    2 0 3

The first `GO` starts at the breakpoint: it runs the instruction there,
then the loop up to the breakpoint again. The last `GO` ends with the
`RETF` (`STOPPED` 0); afterwards no code waits, and `STEP` and `GO` give
an error until the next `DEBUG-AT`.

While the code runs, the vectors 1 (single step) and 3 (`INT 3`) point
to the monitor; afterwards they are as before. A step over `INT n`
runs the whole interrupt and the instruction after it (the CPU clears
the trap flag in an interrupt). Code that never stops cannot be
stopped (as with `CALL-AT`).

## Registers and calls on an ESP32 board

The image holds PC PS SAR EXCCAUSE EXCVADDR, a0 to a15, LBEG LEND
LCOUNT WINDOWBASE WINDOWSTART DEBUGCAUSE and CYCLES. `CALL-AT` uses and
changes a2 to a7, SAR, CYCLES and the PC (the address that it calls);
`LAST-FAULT` sets all of them but CYCLES. After `LAST-FAULT`, `.REGS`
shows the cause of the exception in the line of the flags (the names of
the Xtensa manual, table 4-64; 64 is a debug exception of the HAL).

The functions of the ROM and of C code use the register window of the
Xtensa: the function sees its arguments in a2 to a7 and gives its result
in a2. `CALL-AT` calls it with `CALLX8`, on the stack of the ROM
functions, as `SYSCALL` does (`targets/esp32s3/hal.fs`). A function of
your own starts with `ENTRY,` and ends with `RETW,` (the assembler
XTENSA); as a CODE word, `>CODE` gives the address of its code:

    \ the start of the image in the flash: esp_rom_spiflash_read
    \ ( faddr a u -- r ) of the ROM; r is 0 if all went well
    board ok> CREATE BUF 16 ALLOT
    board ok> $1000 SET-REG A2  BUF >CHIP SET-REG A3  16 SET-REG A4
    board ok> 'FLASH-READ CALL-AT  GET-REG A2 .  GET-REG CYCLES .  BUF 16 DUMP
    0 2395
    00021E7C: E9 03 02 20 94 11 08 40 EE 00 00 00 00 00 00 00 ... ...@........

    \ a function of your own
    board ok> CODE ADD3   A1 32 ENTRY,  A2 A2 A3 ADD,  A2 A2 A4 ADD,  RETW,  END-CODE
    board ok> 1 SET-REG A2  2 SET-REG A3  3 SET-REG A4  ' ADD3 >CODE CALL-AT  GET-REG A2 .
    6
    board ok> ' ADD3 >CODE 4 DISASM DROP
    40082a54  36 41 00                entry a1, 32
    40082a57  30 22 80                add a2, a2, a3
    40082a5a  40 22 80                add a2, a2, a4
    40082a5d  90 00 00                retw

    \ the special registers: the cycle counter, PS, the number of the core
    board ok> CCOUNT SR@ U.  CCOUNT SR@ U.  HEX PS SR@ U.  PRID SR@ U.  DECIMAL
    2932067820 2932110075 40020 CDCD

(The output is from the ESP32 of the board WROOM-32; `$1000` is the
image in its flash, on the ESP32-S3 it is 0.) A call keeps a0 to a7 of
its caller and the stacks of the VM. An exception in the function ends
as in `SYSCALL`: `THROW`, and `LAST-FAULT` has its registers. `SR!`
writes any special register: a wrong value (PS, WINDOWBASE, VECBASE)
can stop the board.

The CPU of the ESP32 reads the RAM of its code (SRAM0) and its ROM only
in cells: a single byte gives an exception. So `DISASM` reads code with
`CELL-C@`, and `DUMPC` shows such memory as `DUMP` does:

    board ok> $40000000 32 DUMPC
    40000000: 00 C5 49 10 D5 49 20 E5 49 30 F5 49 00 34 00 00 ..I..I .I0.I.4..
    40000010: 28 41 38 51 48 61 12 D1 01 00 D1 13 00 48 03 F0 (A8QHa.......H..

The HAL writes the registers of each exception of the CPU to
`FAULT-REGS` of the package INTR (`lib/esp32/intr.fs`), before it goes
on with `THROW`.

## Breakpoints and single steps on an ESP32 board

As on a PC: `DEBUG-AT` prepares a call of a function (as `CALL-AT`,
with a2 to a7 of the image), which then waits; `STEP` lets it run one
instruction, `GO` up to a breakpoint, a `BREAK` instruction in its code,
or its end. After each stop the image holds the registers of the
function: its register window a0 to a15, PC, PS, SAR, the loop
registers, WINDOWBASE, WINDOWSTART and DEBUGCAUSE (1: a step, 2: a
breakpoint, 8: `BREAK`, 16: `BREAK.N`). `SET-REG` changes them for the next `STEP` or
`GO`. At its end, a2 to a7 hold what the function left there (the
result in a2). The function runs on a stack of its own (4 KB):

    board ok> CODE INC3 ( -- )   A1 32 ENTRY,  A2 A2 1 ADDI,  A2 A2 1 ADDI,  A2 A2 1 ADDI,  RETW,  END-CODE
    board ok> 5 SET-REG A2  ' INC3 >CODE DEBUG-AT  STEP  .REGS
    PC=4008313F PS=00060020 SAR=00000000 EXCCAUSE=40 EXCVADDR=00000000
    A0=80082DA8 A1=3FFD3104 A2=00000005 A3=00000000 A4=00000000 A5=00000000
    ...
    DEBUGCAUSE=01 CYCLES=00000000
    INTLEVEL=0 EXCM=0 UM=1 RING=0 OWB=0 CALLINC=2 WOE=1
    board ok> GET-REG PC 1 DISASM DROP
    4008313f  22 c2 01                addi a2, a2, 1
    board ok> ' INC3 >CODE 9 + BREAK-AT  GO  STOPPED .  GET-REG A2 .
    3 7
    board ok> GO  STOPPED .  GET-REG A2 .
    0 8

(The first `STEP` runs `ENTRY`: then a2 is the argument 5. The
breakpoint is at the third `ADDI`. On the ESP32, in `ucf term`, with the
search order of "Registers and calls on an ESP32 board".)

A breakpoint uses the registers IBREAKA0 and IBREAKA1 of the CPU: so it
works in the ROM and in the code of the kernel, but there are only two.
A step uses ICOUNT; it counts only the instructions of the function,
not those of the handlers of interrupts. `BREAK` (`1 15 BREAK,` in a
CODE word) stops the function too; `GO` and `STEP` go on after it.

How it works: while the function waits, the monitor keeps its whole
register file (64 registers, WINDOWBASE, WINDOWSTART) and its special
registers. `STEP` and `GO` call `SYSCALL` with a function of the
monitor: it keeps the register file of the VM in the same way, takes
back that of the function, and goes on in it with `RFI 6`. A debug
exception (ICOUNT, IBREAK, `BREAK`) goes to the monitor (`DEBUG-HOOK` of
INTR, which the HAL reads), which changes the register files back. So
Forth runs between the steps, and the function waits as it was. An
exception of the CPU in the function ends it as in `CALL-AT` (-9).

## Registers and calls on Windows

uCForth for Windows calls the functions of Windows with `SYSCALL`
(`targets/win32/README.md`, "Functions of Windows"). MON-WIN32 calls
code at any address the same way, but with the registers of the image:
EAX EBX ECX EDX ESI EDI EBP; afterwards the image holds the registers
that the code left, and its flags in EFL. ESP and the segments stay as
Windows has them. The arguments go onto the stack as for `SYSCALL`; the
code ends with `RET` (the convention of C) or `RET n` (stdcall, as the
functions of Windows). `lstrlenA` counts the characters of a text with
a byte 0 at its end:

    ok> : LSTRLEN ( -- a )   S" KERNEL32.dll" S" lstrlenA" FIND-FUNCTION ;
    ok> CREATE HELLO 8 ALLOT  S" uCForth" HELLO SWAP MOVE  0 HELLO 7 + C!
    ok> HELLO >CHIP 1 LSTRLEN CALL-AT  GET-REG EAX .
    7

The memory of the VM does not run: Windows lets it read and write only.
For code of your own, `EXEC-ALLOT` gets memory that runs, the assembler
X86 makes the code (`ORG` with that address), and `PLACE-CODE` puts it
there. This code adds the register EBX and its argument:

    ok> 64 EXEC-ALLOT CONSTANT MY-CODE
    ok> MY-CODE ORG  EAX EBX MOV,  EAX 4 [ESP] ADD,  RET,  PLACE-CODE
    ok> 3 SET-REG EBX  7 1 MY-CODE CALL-AT  .REGS
    EAX=0000000A EBX=00000003 ECX=0031FEE8 EDX=0031FF94 ESI=00000000 EDI=00000000 EBP=00000000 EFL=00000206
    NV UP EI PL NZ NA PE NC
    ok> MY-CODE 3 DISASM DROP
    003c0000  89 d8                   mov eax, ebx
    003c0002  03 44 24 04             add eax, dword ptr [esp + 0x4]
    003c0006  c3                      ret

(ECX and EDX are those that `lstrlenA` left before. In Wine; on Windows
the address of `MY-CODE` differs.)

An address of Windows is outside the memory of the VM: `@` cannot read
there. `X-C@` and `DUMPX` read any address (with `RtlMoveMemory`), and
`DISASM` reads with `X-C@`. An exception in a call (the code reads at a
wrong address) gives the error -9, as in `SYSCALL`; the program goes on.

## The disassembler for the x86

DISX86 knows the instructions of the 8086, and those that the 80186 to
the x86-64 added for whole numbers and for the system (`PUSHA`,
`ENTER`, `MOVZX`, `BT`, `SETcc`, `LGDT`, `MOV CR0`, `SYSCALL`,
`CMPXCHG16B`, `RDTSCP` ...). It reads the three kinds of code of the
CPU: 16-bit code (`X86-16`, as after the load: the 8086, a PC in real
mode), 32-bit code (`X86-32`) and 64-bit code (`X86-64`). The prefixes
66h and 67h give the other size of operands and of addresses: in 16-bit
code 32 bits, also with a SIB byte, as a PC runs them in real mode (the
BIOS of a modern PC uses them there). In 64-bit code the prefix REX
gives 64-bit operands and the registers r8 to r15. The coprocessor (D8h
to DFh) shows as `esc` with its number of 6 bits and the operand, as
the manual of the 8086 writes it. A byte that starts no instruction
shows as `db`, also one of MMX, SSE and AVX.

A line has the address, up to 8 bytes of the instruction, and its text:

    1000:a928  8b c3                   mov ax, bx
    f000:e05b  2e 66 83 3e c8 61 00    cmp dword ptr cs:[0x61c8], 0x0
    f000:e06a  66 bc 00 70 00 00       mov esp, 0x7000

In 64-bit code an address relative to RIP shows after `#`:

    00402182  4c 8d 3d 77 de 00 00    lea r15, [rip + 0xde77] # 0x410000

The text is the form of Intel, as clang and llvm-objdump write it: lower
case, numbers as `0x` and hex digits, the size of memory as `byte ptr`,
`word ptr`, `dword ptr` or `qword ptr`, the destination first, a jump
with its target. It differs from llvm-objdump in these points:

| This disassembler | llvm-objdump | Why |
| --- | --- | --- |
| `mov al, 0xff` | `mov al, -0x1` | a number has no sign in the size of its field; only a byte that the CPU extends with its sign (83h, 6Ah, 6Bh) and a displacement have one |
| `mov ax, word ptr [0xffff]` | `[-0x1]` | an address is a number without sign |
| `shl al, 1` | `shl al` | the 1 is part of the instruction (D0h, D1h) |
| `cbw`, `cwd` | `cwde`, `cdq` | with 16-bit operands the CPU does CBW and CWD |
| `pusha`, `popa`, `pushad`, `popad` | `pushaw`, `popaw`, `pushal`, `popal` | the names of the manual |
| `xchg al, ah` | `xchg ah, al` | the field r/m first, as for memory |
| `ljmp 0xf000, 0x12345678` | `ljmp -0x1000, ...` | a segment has no sign |
| `lcall [si]` | `call [si]` | FF /3 and FF /5 are far, also with the prefix 66h |
| `jmp 0x10` | `jmp 0x10010` | with 16-bit operands the target is modulo 64 KB, as in the CPU |
| `lock xchg ...` | two lines | LOCK is a prefix of the instruction |
| `[eax]`, `[0x1000]` | `[eax + eiz]`, `[8*eiz + 0x1000]` | a SIB byte without index has none |
| `add al, al` | `addr16 add al, al` | a prefix 66h or 67h shows in the operands, or not at all |
| `rep`, `repne` | `xrelease`, `xacquire` | F3h and F2h before LOCK are the same prefixes |
| `wbinvd` | (no instruction) | 66h 0Fh 09h: the CPU ignores the prefix; so for some others |
| `db 0x0f` | `movaps ...`, `vaddps ...` | MMX, SSE and AVX are not in DISX86 |
| `mov al, byte ptr [0x...]`, `jecxz` | `addr32 movabs`, `addr32 jrcxz` | 67h with A0h to A3h and E3h in 64-bit code |
| `int1` | (no text) | F1h |

## The disassembler for the Xtensa

DISXT knows the instructions of the ESP32 (Xtensa LX6) as the GNU tools
of Espressif know them: the core, the code density option (the
instructions of 16 bits), the register window, loops, booleans, MAC16,
multiply and divide, floating point, exceptions and interrupts, debug,
the special registers, and the instructions that Espressif added
(`f64...`, `..._expstate`). The ESP32-S3 (LX7) has the same ones without
MAC16; its own instructions (PIE, `ee...`) show as bytes there. The
table of the instructions is in the source as in the manual: the bits
from 23 down to 0, fixed bits as 0 and 1, fields as letters, then the
operands:

    OP:   addi              iiii iiii 1100 ssss tttt 0010   at,as,s8

A line has the address, the bytes and the text, as GNU objdump writes
it: lower case, `a0` to `a15`, a number from -255 to 255 in decimal,
others as `0x` and hex digits, a special register in the name, a jump
with its target:

    40062ed8  36 41 00                entry a1, 32
    40062edb  51 ac fc                l32r a5, 0x4006218c
    40062ee1  c0 20 00                memw

It differs from GNU objdump in these points:

| This disassembler | GNU objdump | Why |
| --- | --- | --- |
| `j 0x40080010` | `j 40080010 <name>` | a target is a number as all others; there are no names |
| `.byte 0x10, 0x0, 0x0` | a line `.byte 0x10` for each byte, or `lsi f1, a0, 0` | an encoding without an instruction takes the length that op0 gives: 3 bytes for op0 0 to 7, 2 for 8 to 13 (on the ESP32 1 byte for 14 and 15, on the ESP32-S3 4 bytes). GNU objdump takes many such encodings for `lsi` |
| `.byte` | `ret`, `ret.n`, `rfdd` with s not 0; `any4 b0, b5:b6:b7:b8` | the manual wants these fields 0, and bs of ANY4 and ALL4 a multiple of 4 (of ANY8 and ALL8 of 8) |
| `rsr a2, 99` | `lsi ...` | a special register without a name: its number after the other operands |

EXTUI with shift + bits > 32 shows as GNU objdump shows it (the manual
calls it undefined; GNU as does not take it).

## The disassembler for AArch64

DISA64 knows the instructions of AArch64 (the instruction set A64 of
the 64-bit ARM) for whole numbers and for the system, from ARMv8.0 to
ARMv8.7: arithmetic and logic, bit fields, moves, jumps, loads and
stores (also those of the registers of floating point and SIMD),
exclusive and atomic access (LSE), pointer authentication (PAC), memory
tags (MTE), barriers, hints, the system registers and the operations of
SYS (`dc`, `ic`, `at`, `tlbi`). Of the later extensions it knows
`bc.cond`, the transactional memory (`tstart` ...), `smstart` and
`smstop`, `msr ALLINT` and the names of hints. The table of the
instructions is in the source as in the ARM manual: the bits from 31
down to 0, fixed bits as 0 and 1, fields as letters, then the operands.
An alias (the "preferred disassembly" of the manual) comes before its
instruction, with its condition:

    WHEN: MOVZ?
    OP: mov      s101 0010 1hhi iiii iiii iiii iiid dddd   Rd,#movz
    OP: movz     s101 0010 1hhi iiii iiii iiii iiid dddd   Rd,#i16.hw

A line has the address, the instruction as a number of 32 bits (as the
manual and llvm-objdump show it) and its text, as llvm-objdump writes
it: lower case; `x0` to `x30`, `w0` to `w30`, `sp`, `xzr`; a number as
`#` and hex digits, the shifts and the fields of bit fields in decimal;
a system register in upper case; a jump with its target:

    00401384  d280001c  mov x28, #0x0
    00401388  f2a0083c  movk x28, #0x41, lsl #16
    00401394  8b09039a  add x26, x28, x9

It differs from llvm-objdump in these points:

| This disassembler | llvm-objdump | Why |
| --- | --- | --- |
| `.inst 0x1e602820` | `fadd d0, d1, d0` | floating point and SIMD (but their loads and stores), SVE and SME are not in DISA64 |
| `.inst 0x9ac26020` | `smax x0, x1, x2` | the extensions after ARMv8.7 (MOPS, CSSC, RCPC3, THE, D128, LSE128, GCS, LSUI, CMPBR ...) are not in DISA64; some show as `msr`, `mrs` or `sys` with numbers |
| `.inst 0x13fcdaa8` | `<unknown>` | an encoding without an instruction: its number, as GNU as and llvm-mc take it |
| `mrs x0, S3_0_C1_C2_0` | `mrs x0, ZCR_EL1` | a system register without a name in the table (it has those of the base, of debug, the performance monitors, timers, the GIC, PAC and MTE): its numbers |
| `mov x0, #0x10000` | `mov x0, #0x10000 // =65536` | no comment |
| `bl 0x401384` | `bl 0x401384 <name>` | a target is a number as all others; there are no names |

## The disassembler on a host

On a host, DISASM reads the memory of the VM. For code in a file, read
the file into a buffer, and give DISASM a word that reads the buffer
at the addresses of the code. The kernel of DOS starts at $100:

    ok> ALSO FILES  INCLUDE asm/monitor/monitor.fs  INCLUDE asm/monitor/disx86.fs
    ok> CREATE BUF 4096 ALLOT
    ok> S" build/kernel.com" R/O OPEN-FILE THROW  BUF 4096 ROT READ-FILE THROW DROP
    ok> : BUF-C@ ( a -- c )   $100 - BUF + C@ ;  ' BUF-C@ DIS-FETCH
    ok> $100 4 DISASM DROP
    0100  e9 1a 17                jmp 0x181d
    0103  24 02                   and al, 0x2
    0105  38 02                   cmp byte ptr [bp + si], al
    0107  47                      inc di

(The bytes after the jump are the table of the primitives, not code:
DISASM shows them as instructions all the same.)

The same for the code of the ESP32 in `build/esp32.bin`: the code
starts after the 24 bytes of the header and the 8 bytes of the first
segment, at `$4008_0000` (`targets/esp32/README.md`, "The image format
of the ROM bootloader"); its table of primitives comes first, the
kernel then. With 32-bit cells:

    ok> ALSO FILES  INCLUDE asm/monitor/monitor.fs  INCLUDE asm/monitor/disxt.fs
    ok> CREATE BUF 8192 ALLOT
    ok> S" build/esp32.bin" R/O OPEN-FILE THROW  BUF 8192 ROT READ-FILE THROW DROP
    ok> : BUF-C@ ( a -- c )   $40080000 - 32 + BUF + C@ ;  ' BUF-C@ DIS-FETCH
    ok> $40080234 3 DISASM DROP
    40080234  56 a3 00                bnez a3, 0x40080242
    40080237  40 52 e2                remu a5, a2, a4
    4008023a  40 22 c2                quou a2, a2, a4

(These are the first instructions of the kernel, the division `UDIV` of
`kernels/xtensa.fs`, after the table and the constants.)

The same for 64-bit code: uCForth for Linux on the x86-64 in
`build/ucf-linux-x64`. Linux puts the start of the file at `$400000`
(`targets/linux-x64/README.md`, "Memory"); the code starts at `$402182`
(the entry of the ELF header):

    ok> ALSO FILES  INCLUDE asm/monitor/monitor.fs  INCLUDE asm/monitor/disx86.fs
    ok> CREATE BUF 12288 ALLOT
    ok> S" build/ucf-linux-x64" R/O OPEN-FILE THROW  BUF 12288 ROT READ-FILE THROW DROP
    ok> : BUF-C@ ( a -- c )   $400000 - BUF + C@ ;  ' BUF-C@ DIS-FETCH
    ok> X86-64  $402182 3 DISASM DROP
    00402182  4c 8d 3d 77 de 00 00    lea r15, [rip + 0xde77] # 0x410000
    00402189  4c 8d 35 20 df ff ff    lea r14, [rip - 0x20e0] # 0x4000b0
    00402190  49 89 a7 18 00 04 00    mov qword ptr [r15 + 0x40018], rsp

(R15 gets the address of the memory of the VM, R14 that of the table of
the primitives: `kernels/x64.fs`.)

And for AArch64: uCForth for Linux on arm64 in `build/ucf-linux-a64`
(`targets/linux-a64/README.md`, "Memory"); its code starts at
`$401384`:

    ok> ALSO FILES  INCLUDE asm/monitor/monitor.fs  INCLUDE asm/monitor/disa64.fs
    ok> CREATE BUF 8192 ALLOT
    ok> S" build/ucf-linux-a64" R/O OPEN-FILE THROW  BUF 8192 ROT READ-FILE THROW DROP
    ok> : BUF-C@ ( a -- c )   $400000 - BUF + C@ ;  ' BUF-C@ DIS-FETCH
    ok> $401384 5 DISASM DROP
    00401384  d280001c  mov x28, #0x0
    00401388  f2a0083c  movk x28, #0x41, lsl #16
    0040138c  d2800009  mov x9, #0x0
    00401390  f2a00089  movk x9, #0x4, lsl #16
    00401394  8b09039a  add x26, x28, x9

(X28 gets the address of the memory of the VM, X26 that of a scratch
area of the HAL: `kernels/aarch64.fs`.)

## CODE words on a PC

The words of MON-PC are CODE words: machine code in the dictionary,
written with the assembler I8086 (`asm/8086.fs`, `asm/README.md`).
`lib/pc/asm.fs` (package ASM) makes them on DOS and on a PC without DOS:

    CODE 3+ ( n -- n+3 )   AX POP,  AX 3 ADDI,  AX PUSH,  NEXT,  END-CODE

While the code runs, CS is the segment of the VM; `NEXT,` goes back to
the kernel. The rules are in `lib/pc/asm.fs`.

On the ESP32 boards, `lib/esp32/asm.fs` (package ASM) makes CODE words
with the assembler XTENSA: the ESP32-S3 runs the code in the dictionary,
the ESP32 in SRAM0, the RAM of its instruction bus
(`lib/esp32/esp32/asm.fs`). MON-ESP32 has four of them: the function
that `CALL-AT` calls with `SYSCALL`, two tables for `SR@` and `SR!`, and
`SR@` and `SR!` themselves. MON-ESP32 keeps its data (the image of the
registers, the registers of `DEBUG-AT` and its stack, the breakpoints)
and MONITOR its text of an instruction in the RAM data (`BUFFER:`):
after `FREEZE` the dictionary of the ESP32 is read only
(`targets/esp32/README.md`, "Memory").

## Test

On each system, with the other tests (`tools/test.fs`), or alone:

    build/ucf-c host/files.fs forth/test/tester.fs asm/monitor/test/disx86.fs
    build/ucf-c -32 host/files.fs forth/test/tester.fs asm/monitor/test/disx86.fs

Expected: `disx86: 1233 tests, 0 errors`. Each line of the test is an
instruction: its address, its bytes, and the text of llvm-objdump 21
(Apple LLVM, the triple `i386-pc-none-code16`), with the changes of the
table in "The disassembler for the x86". The bytes are those of the
tests of the assembler I8086, single cases, and each opcode with several
bytes ModR/M and with the prefixes.

    build/ucf-c -32 host/files.fs forth/test/tester.fs asm/monitor/test/disx32.fs
    build/ucf-c -32 host/files.fs forth/test/tester.fs asm/monitor/test/disx64.fs
    build/ucf-c -32 host/files.fs forth/test/tester.fs asm/monitor/test/disa64.fs

Expected: `disx32: 1338 tests, 0 errors` and `disx64: 1684 tests, 0
errors`. The texts come from llvm-objdump 21 with the triples
`i386-pc-none-elf` and `x86_64-pc-none-elf`; the head of each file lists
all rules for the changes. Once, 70 820 instructions of 32-bit code and
133 465 of 64-bit code went through DISX86 so, without an open
difference: each opcode of one byte and after 0Fh with 29 kinds of
ModR/M, SIB and displacement, without a prefix, with the prefixes and
with REX, each byte ModR/M and each SIB with some opcodes. The tests
keep a choice: each opcode with each of its names, prefixes, bytes that
are no instruction.

Expected for AArch64: `disa64: 2773 tests, 0 errors`. The texts come
from llvm-objdump 21 (the triple `aarch64-none-elf`, with all
extensions), with the changes of the table in "The disassembler for
AArch64". Once, 2 151 621 instructions went through DISA64 so, without
an open difference: each line of its table with up to 300 values of its
fields and with each of its fixed bits changed, all combinations of the
fields of bit fields, logical immediates, shifts, extensions and loads,
all numbers of the system registers and of SYS, 1.4 million other
numbers, and the code of uCForth for Linux (arm64) and the Raspberry
Pi. The test keeps a choice: each line of the table, each hint, the
system registers and operations, and each kind of instruction that
DISA64 does not know.

    build/ucf-c -32 host/files.fs forth/test/tester.fs asm/monitor/test/disxt.fs

Expected: `disxt: 1372 tests, 0 errors`. The texts come from GNU objdump
2.35.1 for the ESP32 (esp-2021r2), with the changes of the table in
"The disassembler for the Xtensa"; each text was checked with GNU as
(the same bytes at the same address). Once, all encodings of op0 0 and
4 and of 16 bits and samples of the others (2 314 376 encodings) went
through DISXT so, without an error; the test keeps a choice: each
instruction with several values, each special register, each kind of
encoding without an instruction, and the ESP32-S3.

On a PC in QEMU the test of MON-PC runs with all other tests of the PC
(`targets/pc-bios/README.md`, "Test"): `monpc: 31 tests, 0 errors`. On
DOS (FreeDOS in QEMU, `targets/dos/README.md`, "Test", step 8), the
same. They call INT 12h and compare it with the data area of the BIOS,
call code of their own with registers and flags, also with another DS,
read and write memory with segment and offset, disassemble the reset
vector of the BIOS, step through code, stop it at breakpoints (also in
a loop and at its end), and check that the bytes of the code and the
vectors 1 and 3 are as before.

On a board with an ESP32 or an ESP32-S3, in `ucf term`, after the
tester and the files of "Load":

    board ok> #include forth/test/tester.fs
    ...
    board ok> #include asm/monitor/test/monesp.fs

Expected: the image, then `monesp: 31 tests, 0 errors`. They call a
function of their own and one of the ROM, read the registers of two
exceptions (in the ROM and in the VM), disassemble the instruction of
the second one and the ROM, read and write special registers, step
through a function of their own and of the ROM, stop at a breakpoint
and at a `BREAK` instruction, change a register while the function
waits, and check that afterwards the stacks of the VM are as before and
no debug register is on. Load the files of "Load" right after a start:
the search order has room for 16 vocabularies.

On Windows, after the tester and the files of "Load":

    ok> ALSO FILES  INCLUDE forth/test/tester.fs
    ...
    ok> INCLUDE asm/monitor/test/monwin.fs

Expected: the image, three lines of `DISASM` and one of `DUMPX`, then
`monwin: 12 tests, 0 errors`. They call code of their own (with
registers, arguments and the flags) and `lstrlenA`, an exception in a
call, and read memory of Windows and of the VM. On the Mac it runs in
Wine (`targets/win32/README.md`, "Test"), in the folder `ucf`:

    printf '%s\n' 'ALSO FILES  INCLUDE forth/test/tester.fs' \
      'ALSO FILES  INCLUDE lib/text.fs  INCLUDE asm/x86.fs  INCLUDE lib/windows/windows.fs' \
      'INCLUDE asm/monitor/monitor.fs  INCLUDE asm/monitor/disx86.fs  INCLUDE asm/monitor/monwin.fs' \
      'INCLUDE asm/monitor/test/monwin.fs' 'BYE' > build/monwin.txt
    docker run --rm --platform linux/386 -v "$PWD":/u -w /u ucf-wine386 \
      sh -c 'wine build/ucf-win32.exe < build/monwin.txt'

## Memory

A KB here is 1024 bytes. With 16-bit cells, measured on a PC (QEMU):

| Packages | Dictionary | RAM data |
| --- | --- | --- |
| TEXT, I8086, ASM (CODE words) | 7.9 KB | 0.3 KB |
| MONITOR, DISX86 | 17.0 KB | 0.1 KB |
| MON-PC | 4.5 KB (two stacks of 512 bytes for calls and for `DEBUG-AT`) | 22 bytes |
| free afterwards | 19.3 KB of 58 KB | 3.0 KB |

The tables of DISX86 take 5.5 KB of it: 1 KB for the opcodes of one
byte, 1 KB for those after 0Fh, 0.8 KB for the groups, 1.9 KB for the
names, 0.8 KB for the names of 0Fh 01h with a register (`vmcall`,
`swapgs` ...), 0.1 KB for the opcodes that 64-bit code has otherwise.
With 32-bit cells, MONITOR and DISX86 take 25.9 KB.

On Windows (32-bit cells), measured in Wine:

| Packages | Dictionary |
| --- | --- |
| TEXT, X86 | 14.5 KB |
| WINDOWS | 0.9 KB |
| MONITOR, DISX86 | 26.0 KB |
| MON-WIN32 | 33.2 KB: 32 KB of it are the memory of the assembler X86 for the code (`TARGET`), which your code uses too |
| free afterwards | 96.6 KB of 171 KB |

With 32-bit cells, measured on the ESP32 (board WROOM-32):

| Packages | Dictionary | SRAM0 (code) |
| --- | --- | --- |
| TEXT | 0.8 KB | |
| XTENSA, ASM (CODE words) | 14.3 KB | 20 bytes |
| MONITOR, DISXT | 20.5 KB (MONITOR 2.9 KB, DISXT 17.6 KB) | |
| MON-ESP32 | 10.3 KB: 4 KB of it the stack for `DEBUG-AT`, 0.6 KB the register files | 4.4 KB |
| free afterwards | 112.2 KB of 189 KB (measured with the old map of 192 KB: 48.2 KB of 125 KB) | 115 KB |

DISXT has 302 instructions in its table (8 KB) and the names of 67
special registers. On the ESP32-S3 the code of the CODE words is in the
dictionary: there XTENSA and ASM take 14.0 KB, MON-ESP32 14.6 KB with
its code, the others as above; of the 154 KB that are free after the
start, 104 KB stay free.

AArch64, measured with `build/ucf-macos-a64`: MONITOR and DISA64 take
47.3 KB (DISA64 44.3 KB), of the 171 KB that are free after the start.
The table of DISA64 has 482 lines (11.3 KB), the names of 469 system
registers take 7.0 KB, those of 236 operations of SYS 3.8 KB.

## Plans

| Step | Machine | What comes |
| --- | --- | --- |
| M2 (done) | ESP32 and ESP32-S3 (Xtensa) | DISXT, the disassembler for the Xtensa; MON-ESP32: calls of functions in the ROM with the registers of the image (the windowed calls of the Xtensa), the registers of the last error of the CPU, the special registers |
| M3 (done) | the x86 in 32-bit mode (Windows, Linux i386) and in 64-bit mode (Linux, macOS on Intel, UEFI) | DISX86 for 32 and 64 bits; calls, where the system lets a program call any address: on Windows (MON-WIN32) |
| M3 (done) | AArch64 (the Raspberry Pi without an operating system, Linux, macOS on Apple silicon) | DISA64, the disassembler for the 64-bit ARM |
| M4 (done) | the PC and DOS, the ESP32 boards | breakpoints and single steps: on the x86 `INT 3` and the trap flag, on the Xtensa IBREAK, `BREAK` and ICOUNT (tested on the PC, DOS and both ESP32 boards) |
| later | the 65xx (6502 and its kin), with the target C64 | a disassembler and the calls of the ROM (KERNAL) |

On a machine with an operating system a program cannot read every
address (on 64 bits an address does not fit in a cell of 32 bits), and
the memory of the VM does not run. Windows lets a program call any
address (`SYSCALL`) and gives it memory that runs: MON-WIN32. On Linux,
macOS, UEFI and the Raspberry Pi the kernel has no word that calls code
at an address: there the monitor disassembles, for now.
