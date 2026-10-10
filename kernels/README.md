# kernels: virtual machines in machine code

A kernel is the virtual machine (doc/vm.md) as machine code for one
processor: NEXT and the primitives. uCForth builds it with an assembler
from `asm/`. No C compiler is used. The platform adds its HAL
(`targets/NAME/hal.fs`).

| File | Contents |
| --- | --- |
| `aarch64.fs` | 64-bit ARM, 32-bit cells: primitives 0-28 and 32-65; used by `targets/linux-a64/`, `targets/macos-a64/` and `targets/rpi4/`. The table of the primitives holds offsets from its start, so the code runs at any address. |
| `8086.fs` | Intel 8086 in real mode, 16-bit cells: primitives 0-28 and 32-66 (66: `CODE` words, in the segment of the VM); the VM memory is one segment of 64 KB, the table of the primitives is in the code segment; SP is the data stack; used by `targets/dos/` and `targets/pc-bios/` (a PC without an operating system). The build needs a host with 16-bit cells |
| `xtensa.fs` | Xtensa LX7 (ESP32-S3) and LX6 (ESP32), 32-bit cells: primitives 0-28 and 32-66 (66: `CODE` words; the HAL of the ESP32 has its own 66, which takes the address of the code from the cell after the code field); the VM memory at a base address, registers reach the chip; the registers of the VM and NEXT are in `asm/xtensa.fs`; the build gives the base address (`RAM-ADDR`) and the distance of the code to its address on the data bus (`>DRAM`: 0 on the ESP32, which reads its code in SRAM0 in cells); on the ESP32 the frozen part of the dictionary can lie in the flash (`FLASH-DICT?` of `asm/xtensa.fs`: NEXT, the jumps and the memory words take the end of this part from slot 120 of the table of the primitives; a store into it gives -9); used by `targets/esp32s3/` and `targets/esp32/` |
| `x86.fs` | Intel 386 and later in 32-bit mode, 32-bit cells: primitives 0-28 and 32-65; the program lies at a fixed address, so each access to the VM memory is one instruction (`[reg+MEM]`); a break switches the table with one cell (BREAK-OFF); used by `targets/win32/` and `targets/linux-i386/` |
| `x64.fs` | x86-64 in 64-bit mode, 32-bit cells: primitives 0-28 and 32-65; the VM in the registers that the calls of the system keep (RBX RBP R12-R15); R15 holds VM address 0, so each access is one instruction (`[r15+reg]`); the code reaches its own cells relative to itself (`[RIP]`) and the table holds offsets, so it runs at any address; a break as in `x86.fs` (BREAK-OFF); used by `targets/linux-x64/`, `targets/macos-x64/` and `targets/uefi-x64/` |

## Memory

Each kernel runs the memory map of doc/vm.md, section 3: 64 KB with
16-bit cells, 256 KB with 32-bit cells (the ESP32: in two parts of its
RAM, and the frozen part of the dictionary in its flash). The dictionary, the RAM data,
the stacks and the input line are there; doc/vm.md, section 3, says how
large each part is and what happens when it is full. The kernel finds
VM address 0 at a base address that the target sets. The README of
each target (section "Memory") gives its addresses and how much is free
after the start.

| Kernel | VM address 0 | An access to a cell | An address outside the 64 or 256 KB | The code with the HAL |
| --- | --- | --- | --- | --- |
| `8086.fs` | offset 0 of a segment of its own (DS and ES); the code and its buffers are in another segment (DOS: the one before it; the PC: `$0800`, the VM at `$1000`) | the VM address is the offset | there is none: the offsets of a segment go from 0 to $FFFF | 6 KB (`targets/dos`), 3.3 KB (`targets/pc-bios`) |
| `aarch64.fs` | the register X28 (MEM): next to the code (Linux, macOS), or 0 (the Pi) | MEM + the VM address, in one instruction | MEM + the address, without a check: Linux and macOS end the program where there is no memory; the Pi reaches the chip (-9 for an address that is not aligned) | 7 to 9 KB (`targets/linux-a64`, `macos-a64`, `rpi4`) |
| `xtensa.fs` | the register A10 (MEM), in the RAM of the chip; on the ESP32 VM addresses 0 to `$FFFF` lie in another part of the RAM, at MEM + DATA-OFF, and the frozen part of the dictionary in the flash | MEM + the VM address (the ESP32: below `$10000` + DATA-OFF too; from `S0` up to the end of the frozen part `$3F40_0000` + the VM address, through the cache) | the address of the chip, as it is: the registers of the chip, the PSRAM | 8 KB (`targets/esp32s3`), 10 KB (`targets/esp32`) |
| `x86.fs` | MEM, a constant of the target (Windows: `$10010000`, Linux: `$410000`) | the instruction holds MEM: `[reg+MEM]` | MEM + the address: -9 where there is no memory | 9 KB (`targets/win32`, `linux-i386`) |
| `x64.fs` | the register R15 (MEM): next to the code (Linux: `$410000`; macOS and UEFI: where the system loads the program) | MEM + the VM address, in one instruction | MEM + the address: -9 where there is no memory (Linux, macOS); with UEFI the memory of the PC, without a check, as on a board | 8.4 to 8.8 KB (`targets/linux-x64`, `macos-x64`, `uefi-x64`) |

The stack of the processor: the 8086 uses SP as the data stack of the
VM; the other kernels do not need it, only the calls of the system do
(Linux, macOS, Windows: the system gives it; the ESP32 chips: a stack
of its own for the functions of the ROM). On a PC without DOS, the calls
of the BIOS run on a stack of their own too (`targets/pc-bios`). UEFI
gives its stack to the program; each call of the firmware aligns it to
16 bytes (`targets/uefi-x64`).
