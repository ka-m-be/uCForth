# asm: assemblers in Forth

One file per processor. An assembler writes machine code into memory; a
kernel in `kernels/` uses it. Each assembler has tests in `asm/test/`:
known encodings, checked once against a foreign assembler and kept as data.

| File | Contents |
| --- | --- |
| `8086.fs` | package I8086: Intel 8086 (16-bit real mode, the PC and DOS), all instructions of the 8086 that a kernel needs, also far jumps and calls; labels L1 to L9, named labels, and IF, THEN, BEGIN, UNTIL ... for short jumps. Runs with 16-bit and 32-bit cells. It writes into a memory of the host (`ORG`), or on a PC into the dictionary, where the code runs (`IN-PLACE`; `CODE` words, `lib/pc/asm.fs`) |
| `test/8086.fs` | 266 tests, encodings from clang 21 (`.code16`) |
| `x86.fs` | package X86: Intel 386 and later in 32-bit mode (Windows, Linux i386), the instructions that a kernel needs; memory with an index register (`[EAX*4]`); labels and short structures as in I8086; short and near (32-bit) jumps |
| `test/x86.fs` | 252 tests, encodings from clang 21 |
| `x64.fs` | package X64: x86-64 in 64-bit mode (Linux, macOS on Intel, UEFI), the instructions that a kernel needs; the registers R8 to R15, memory relative to the instruction (`n [RIP]`) for code at any address; otherwise as X86 |
| `test/x64.fs` | 322 tests, encodings from clang 21 (`x86_64`) |
| `aarch64.fs` | package AARCH64: 64-bit ARM (A64), the instructions that the kernels need; labels, and IF, THEN, BEGIN, UNTIL for short jumps |
| `test/aarch64.fs` | 74 tests, encodings from the GNU assembler 2.40 (SXTW: clang 21) |
| `xtensa.fs` | package XTENSA: Xtensa LX7 (ESP32-S3) and LX6 (ESP32), the instructions of the kernel and of its vectors (exceptions, interrupts, the register window), `ENTRY,` and `RETW,` for a function as C code has it, `BREAK,` and `BREAK.N,`, the names of the special registers (also those for breakpoints: `IBREAKA0`, `ICOUNT` ...); for the code of C also the loops (`LOOP,` `LOOPNEZ,` `LOOPGTZ,`), `MUL16U,`, `MOVSP,` and a part of the floating point coprocessor (the registers `F0` to `F15`, the booleans `B0` to `B15`, `ADD.S,` `SUB.S,` `MUL.S,` `MADD.S,` `OEQ.S,` `OLT.S,` `OLE.S,` `FLOAT.S,` `UFLOAT.S,` `UTRUNC.S,` `MOV.S,` `RFR,` `WFR,` `LSI,` `SSI,` `BF,` `BT,`); local labels L1 to L9; the registers of the VM and its macros (`NEXT,` `PUSH,` `POP,` `>CHIP,` `>CHIP!,` ...), `CONST,`, and for the memory of the ESP32 `DATA-OFF` and `FLASH-DICT?` (`targets/esp32/README.md`, "Memory"). It writes into a memory of the host (`ORG`), or on the board into the memory where the code runs (`IN-PLACE`; `CODE` words, `lib/esp32/asm.fs`) |
| `test/xtensa.fs` | 387 tests, encodings from the GNU assembler 2.35.1 (ESP32) |

AARCH64, XTENSA, X86 and X64 need a host with 32-bit cells (`-32`). I8086
runs with 16-bit cells too: a DOS computer assembles its own kernel.
Planned: 6502.

The folder `monitor/` is the monitor: registers, calls of the BIOS or of
code at any address, disassemblers, as Forth words on the machine that
runs uCForth (`monitor/README.md`). So far for a PC with DOS or without
an operating system, and for the boards with an ESP32 or an ESP32-S3;
the disassemblers for the x86 (16, 32 and 64 bits), the Xtensa and
AArch64 run everywhere.

## How to read and write assembler in Forth

The example is the 8086, but the other assemblers work in the same way.
Assembler code looks hard. It is not: each line is a list of
instructions, and each instruction is the same as in the manual of the
processor, in a different order.

### The order: operands first

Forth reads a line from left to right and does each word at once. So
the operands come first, then the instruction word, which takes them
from the stack and writes the bytes. The instruction word ends with a
comma, as `,` and `C,` of Forth: it puts something into memory. The
order of the operands is the order of the Intel manual: the destination
first.

| Intel manual, DEBUG | I8086 | What it does |
| --- | --- | --- |
| `MOV AX,BX` | `AX BX MOV,` | AX gets the value of BX |
| `MOV AX,1234` | `AX $1234 MOVI,` | AX gets the number $1234 |
| `ADD AX,5` | `AX 5 ADDI,` | AX = AX + 5 |
| `MOV AX,[BX]` | `AX 0 [BX] MOV,` | AX gets the word at the address in BX |
| `MOV AX,[BP+4]` | `AX 4 [BP] MOV,` | ... at the address BP + 4 |
| `MOV [0106],AX` | `$0106 [] AX MOV,` | the word at address $0106 gets AX |
| `MOV BYTE PTR [DI],7` | `0 [DI] BYTE 7 MOVI,` | the byte at the address in DI gets 7 |
| `INC WORD PTR [BX]` | `0 [BX] INC,` | memory without `BYTE` is a word |
| `SHL AX,1` | `AX 1 SHL,` | shift left by 1 |
| `SHR DX,CL` | `DX CL SHR,` | shift right by the number in CL |
| `PUSH AX` | `AX PUSH,` | |
| `REP MOVSB` | `REP, MOVSB,` | a prefix is an instruction word of its own |
| `MOV AX,CS:[BX]` | `CS: AX 0 [BX] MOV,` | the segment prefix comes first |
| `INT 21` | `$21 INT,` | |
| `IN AL,DX` | `AL DX IN,` | |
| `OUT 43,AL` | `$43 AL OUT,` | the port first, as in the manual |
| `JMP [DI+0100]` | `$100 [DI] JMP-IND,` | jump to the address in memory |
| `JMP F000:E05B` | `$F000 $E05B JMP-FAR,` | a far jump: CS changes too; `CALL-FAR,` calls |
| `JMP FAR [BP-4]` | `-4 [BP] JMP-FAR-IND,` | jump to the far address in memory (offset, segment); `CALL-FAR-IND,` calls |
| `JNZ 0123` | `L1 JNZ,` | jump to a label (below) |

Three rules come from this table:

1. **A number as operand gets an instruction with I** at the end:
   `MOVI,` `ADDI,` `CMPI,` `TESTI,` ... So a number is never taken for
   a register by mistake.
2. **Memory is a displacement and a register form:** `n [BX]`,
   `n [SI]`, `n [DI]`, `n [BP]`, `n [BX+SI]`, `n [BX+DI]`, `n [BP+SI]`,
   `n [BP+DI]`, and `n []` for the address n alone. These are the
   eight forms of the 8086; the assembler takes the shortest encoding
   for n (0, 8 bits or 16 bits).
3. **The size comes from the register.** `AL 0 [BX] MOV,` moves a byte,
   `AX 0 [BX] MOV,` a word. Only when no register tells the size, memory
   is a word; `BYTE` after the memory operand makes it a byte.

The assembler stops with a message when something is wrong, for
example `AX BL MOV,` ("the operands have different sizes") or
`AX 2 SHL,` ("the 8086 shifts by 1 or by CL").

### Registers and conditions

| Words | Meaning |
| --- | --- |
| `AX CX DX BX SP BP SI DI` | the word registers (16 bits) |
| `AL CL DL BL AH CH DH BH` | the byte registers |
| `ES CS SS DS` | the segment registers; `ES:` `CS:` `SS:` `DS:` are the prefixes |
| `Z NZ C NC S NS O NO PE PO` | conditions from one flag: zero, carry, sign, overflow, parity |
| `E NE B AE BE A` | equal, and the comparison of numbers without sign: below, above |
| `L GE LE G` | the comparison of numbers with sign: less, greater |

The jumps have the Intel names: `JZ,` `JNZ,` `JC,` `JB,` `JAE,` `JL,`
`JG,` ... , `LOOP,` `JCXZ,`, `JMP,` and `CALL,`.

### Labels and jumps

A label is a name for an address in the code:

    LABEL START         \ START is the address of the next instruction
      CX 10 MOVI,
    LABEL L1
      AX INC,
      L1 LOOP,          \ back to L1, 10 times

`L1` to `L9` are local labels: each named label (`LABEL START`) starts a
new set of them. A jump may go to a label before its place: the
assembler fixes the jump when the label comes. `FORWARD name` makes a
named label that comes later. `CHECK-LABELS` at the end of the code
stops with an error if a label never came.

On the 8086 a conditional jump reaches only 127 bytes back or forward.
The assembler says "the jump is too far" when a label is further away.
`JMP,` takes the short form (2 bytes) when the label is near and known,
else the near form (3 bytes).

### Structures instead of labels

For short parts inside one routine, IF, THEN and their friends are
easier to read than labels. They make the same short jumps:

| I8086 | Intel with labels |
| --- | --- |
| `AX AX OR,  Z IF,  AX INC,  THEN,` | `OR AX,AX` / `JNZ x` / `INC AX` / `x:` |
| `NZ IF, ... ELSE, ... THEN,` | `JZ x` / ... / `JMP y` / `x:` ... / `y:` |
| `BEGIN, ... Z UNTIL,` | `x:` ... / `JNZ x`: again until the flag Z is set |
| `BEGIN, ... NZ WHILE, ... REPEAT,` | `x:` ... / `JZ y` / ... / `JMP x` / `y:` |
| `BEGIN, ... AGAIN,` | `x:` ... / `JMP x` |

`Z IF,` means: do the part when Z is set. The assembler writes the jump
with the other condition (`JNZ`) over the part.

### Where the code goes

`ORG ( ta -- )` starts new code in `TARGET`, a memory of 8 KB in the
host. `ta` is the address of the first byte in the target, for example
`$100` for a `.COM` program of DOS. `THERE` is the address of the next
byte, `TDP` holds the number of bytes. `TC,` `T,` and `TS,` put a byte, a
word or the bytes of a text into the code; `TC@` `T@` `TC!` `T!` read and
change it.

Before the first `ORG` there is no `TARGET`: the first instruction stops
with the error `no target memory: ORG first`, and nothing in the host
changes. In AARCH64, `START-CODE ( ta -- )` does the work of `ORG`;
without it the error is `no target address: START-CODE first`.

On the machine itself, `IN-PLACE ( ta a limit -- )` (I8086 and XTENSA)
puts the code into the memory of this system instead, where it runs:
the next byte goes to the address in the counter at a (the dictionary
pointer `DP`), below limit, and the CPU sees the byte at address x at
ta + x. The words CODE and END-CODE do this (`lib/pc/asm.fs` on a PC,
`lib/esp32/asm.fs` on the ESP32 boards; the ESP32 writes into a buffer
and copies the code to the RAM of its instruction bus). In place, only
the labels L1 to L9 exist: a named label would be a new word in the
middle of the code.

### A complete example: a DOS program

This file makes `HELLO.COM`, a program of 29 bytes for DOS. Load it in
the folder `ucf` with any bootstrap VM, for example
`build/ucf-c host/files.fs hello.fs`:

    ONLY FORTH DEFINITIONS  ALSO FILES
    S" lib/text.fs" INCLUDED  S" asm/8086.fs" INCLUDED
    ONLY FORTH DEFINITIONS  ALSO FILES  ALSO I8086

    $100 ORG                      \ DOS loads a .COM program at offset $100
      DX L1 MOVI,                 \ MOV DX,text     the address of the text
      AH 9 MOVI,                  \ MOV AH,09       DOS: write the text up to "$"
      $21 INT,                    \ INT 21
      AX $4C00 MOVI,              \ MOV AX,4C00     DOS: end the program, status 0
      $21 INT,                    \ INT 21
    LABEL L1
      S" Hello from DOS" TS,  13 TC, 10 TC,  '$' TC,
    CHECK-LABELS

    : SAVE-CODE ( a u -- )   \ write the code to the file a u
      W/O CREATE-FILE THROW  >R  TARGET TDP @ R@ WRITE-FILE THROW  R> CLOSE-FILE THROW ;
    S" HELLO.COM" SAVE-CODE

On the DOS computer (here FreeDOS in QEMU, `doc/qemu.md`), the program
runs, and DEBUG shows the same instructions with `U`:

    C:\>HELLO
    Hello from DOS
    C:\>DEBUG HELLO.COM
    -u 100 10b
    27F0:0100 BA0C01            MOV     DX,010C
    27F0:0103 B409              MOV     AH,09
    27F0:0105 CD21              INT     21
    27F0:0107 B8004C            MOV     AX,4C00
    27F0:010A CD21              INT     21

The kernel of the DOS target (`kernels/8086.fs`, `targets/dos/hal.fs`)
is written in the same way; `targets/dos/README.md` shows its listing,
the address and bytes of each line next to the source. The PC without
an operating system (`targets/pc-bios`) takes the same kernel; its boot
sector (`targets/pc-bios/bootsect.fs`) is a small example of a whole
sector in this assembler.

### The 386: X86

The package X86 (`x86.fs`) works like I8086, with the registers of 32
bits. The rules above stay: the operands first, the destination first,
`I` for a number, the size from the register, `BYTE` after memory.

| Intel manual | X86 | What it does |
| --- | --- | --- |
| `mov eax, ebx` | `EAX EBX MOV,` | EAX gets the value of EBX |
| `mov eax, 12345678h` | `EAX $12345678 MOVI,` | a number of 32 bits |
| `mov eax, [ebx+8]` | `EAX 8 [EBX] MOV,` | 32 bits at the address EBX + 8 |
| `mov [402000h], eax` | `$402000 [] EAX MOV,` | the cell at the address $402000 |
| `mov eax, [ecx+edx*4]` | `EAX 0 [ECX] [EDX*4] MOV,` | an index register, times 1, 2, 4 or 8 |
| `jmp [402000h+eax*4]` | `$402000 [] [EAX*4] JMP-IND,` | a jump through a table |
| `movzx eax, byte ptr [ebx]` | `EAX 0 [EBX] BYTE MOVZX,` | a byte, as a number of 32 bits |
| `shl eax, 3` | `EAX 3 SHL,` | the 386 shifts by any number, or by CL |
| `sete al` | `AL E SETCC,` | AL = 1 when the flag says "equal", else 0 |
| `push 0` | `0 PUSHI,` | a number onto the stack of the processor |
| `call [401004h]` | `$401004 [] CALL-IND,` | call the address in the cell (a function of Windows) |
| `ret 4` | `4 RETN,` | return, and drop 4 bytes of arguments |

What is different from I8086:

1. **The registers:** `EAX ECX EDX EBX ESP EBP ESI EDI` (32 bits) and
   `AL CL DL BL AH CH DH BH`. There are no segment registers and no
   16-bit registers: a program for Windows or Linux does not need them.
2. **Memory:** any register is a base: `n [EAX]` ... `n [EDI]`, and
   `n []` alone. After it, an index register may come: `[EAX*4]`,
   `[EBX*1]` ... (not ESP). The displacement n has 8 or 32 bits; the
   assembler takes the shorter one.
3. **Jumps:** a conditional jump has 8 or 32 bits. A jump to a label
   with its place takes the short form when it reaches; a jump to a
   label before its place takes 32 bits. `LOOP,` and `JECXZ,` are short
   only; IF, and its friends make short jumps, as on the 8086.
4. **Data:** `TC,` `TW,` `T,` put 8, 16 or 32 bits into the code; `T@`
   and `T!` read and change 32 bits; `4 TALIGN` goes to an address that
   is a multiple of 4.

A label before its place works for a jump, `CALL,`, `MOVI,` to a
register and `PUSHI,` (the address of a function for Windows: `ON-CTRL
PUSHI,`). `PUSHI,` of -1 takes 5 bytes, because a label before its
place gives -1.

The kernel `kernels/x86.fs` is an example of it all. Its inner
interpreter NEXT is six instructions:

    EBX MEM [ESI] MOV,  ESI 4 ADDI,             \ W = the cell at IP; IP = IP + 4
    EAX MEM [EBX] MOV,  EAX 127 CMPI,  UNKNOWN-OP JA,   \ the number of the primitive
    EAX BREAK-OFF [] ADD,  OPTAB [] [EAX*4] JMP-IND,    \ jump to its code

MEM is the address of VM address 0: each access to the memory of the
VM is one instruction.

### x86-64: X64

The package X64 (`x64.fs`) is X86 for the 64-bit mode of the same
processors: each PC and Mac with Intel or AMD since about 2005. The
rules stay: the operands first, the destination first, `I` for a
number, the size from the register, `BYTE` after memory, and now
`QWORD` for memory of 64 bits.

| Intel manual | X64 | What it does |
| --- | --- | --- |
| `mov rax, rbx` | `RAX RBX MOV,` | 64 bits |
| `mov eax, r9d` | `EAX R9D MOV,` | 32 bits; the upper half of RAX becomes 0 |
| `mov eax, [r15+r12]` | `EAX 0 [R15] [R12*1] MOV,` | base R15, index R12 |
| `mov eax, [r15+r13+4]` | `EAX 4 [R15] [R13*1] MOV,` | the same with 4 more |
| `mov ebx, [rip+...]` | `EBX XT-BOOT [RIP] MOV,` | the cell at the label XT-BOOT, relative to the next instruction |
| `lea r15, [rip+...]` | `R15 MEM-AT [RIP] LEA,` | the address of a place in the program |
| `mov rax, -1` | `RAX -1 MOVI,` | a number of 32 bits with its sign: all 64 bits are 1 |
| `mov eax, 0FFFFFFFFh` | `EAX -1 MOVI,` | the lower 32 bits are 1, the upper 0 |
| `mov qword ptr [rdi], 7` | `0 [RDI] QWORD 7 MOVI,` | 64 bits in memory |
| `movsxd rcx, dword ptr [r14+rax*4]` | `RCX 0 [R14] [RAX*4] MOVSXD,` | 32 bits with their sign to 64 |
| `jmp rax` | `RAX JMP-IND,` | jump to the address in RAX |
| `call qword ptr [rax+8]` | `8 [RAX] CALL-IND,` | call the function in a table (UEFI) |
| `syscall` | `SYSCALL,` | call the kernel of Linux or macOS |
| `rdtsc` | `RDTSC,` | the clock of the processor to EDX:EAX |

What is different from X86:

1. **The registers:** `RAX` to `RDI` and `R8` to `R15` (64 bits);
   `EAX` to `EDI` and `R8D` to `R15D` (their lower 32 bits); `AL` to
   `BL`, `SPL BPL SIL DIL` and `R8B` to `R15B` (their lower byte);
   `AH CH DH BH`. An instruction that writes a register of 32 bits sets
   the upper 32 bits to 0, so a VM address in `R12D` is also the right
   number in `R12`.
2. **Addresses have 64 bits:** the base and the index of memory are
   registers of 64 bits: `n [R15]`, `[R12*1]`, also for data of 32 bits.
3. **`n [RIP]`:** the processor adds the address of the next
   instruction. The assembler gives n minus that address, so the code
   can lie at any address: macOS and UEFI load a program at an address
   that they choose. The label must have its place before the
   instruction (no jump waits for it). `n []` is still the fixed address
   n.
4. **Numbers:** the host has cells of 32 bits, so a number in an
   instruction has 32 bits. With an operand of 64 bits the processor
   extends it with its sign: `RAX -1 MOVI,` sets all 64 bits, `RAX
   $7FFFFFFF ANDI,` keeps 31. For a number without sign use the
   register of 32 bits.
5. **The REX prefix:** an instruction with 64 bits or with R8 to R15
   gets a byte $40 to $4F before its opcode; the assembler adds it.
   `AH CH DH BH` cannot go with it: `AH R8B MOV,` is an error.
6. **Gone:** `INC,` and `DEC,` take two bytes (the short forms of the
   386 are the REX prefix now); `PUSH,` and `POP,` take 64 bits;
   `PUSHAD,` and `POPAD,` do not exist. `JECXZ,` is `JRCXZ,`.

The kernel `kernels/x64.fs` is an example of it all. R15 holds the
address of VM address 0, R14 the address of the table of the
primitives. Its NEXT is nine instructions:

    EBX 0 [R15] [R12*1] MOV,  R12D 4 ADDI,                \ W = the cell at IP; IP = IP + 4
    EAX 0 [R15] [RBX*1] MOV,  EAX 127 CMPI,  UNKNOWN-OP JA,   \ the number of the primitive
    EAX BREAK-OFF [R15] ADD,                               \ 128 more after Ctrl-C
    EAX 0 [R14] [RAX*4] MOV,  RAX R14 ADD,  RAX JMP-IND,   \ the table holds offsets from R14

The table holds offsets, not addresses: so it is right at each address
of the program, and nothing must change it at the start.

### How the tests are made

Each test assembles one instruction and compares the bytes with the
bytes of a foreign assembler, found once and kept as data. For the
8086, clang (Apple LLVM 21) assembled each line of Intel syntax:

    printf '.intel_syntax noprefix\n.code16\nmov ax, word ptr [bp+4]\n' > t.s
    clang -target i386-pc-none-elf -c t.s -o t.o
    objdump -d --triple=i386-pc-none-code16 t.o

The output line `0: 8b 46 04  mov ax, word ptr [bp + 0x4]` becomes the
test

    T{ AX 4 [BP] MOV, GOT -> $8B $46 $04 }T   \ mov ax, word ptr [bp+4]

Run the tests of the 8086 assembler alone:

    build/ucf-c host/files.fs forth/test/tester.fs asm/test/8086.fs

The last line is `8086: 249 tests, 0 errors`. For X86 the same, with
`clang -target i386-pc-none-elf` and `.intel_syntax noprefix` (without
`.code16`):

    build/ucf-c -32 host/files.fs forth/test/tester.fs asm/test/x86.fs

The last line is `x86: 252 tests, 0 errors`. For X64 the same with
`clang -target x86_64-pc-none-elf`:

    build/ucf-c -32 host/files.fs forth/test/tester.fs asm/test/x64.fs

The last line is `x64: 322 tests, 0 errors`. A test with `[RIP]` names
its target as `THERE` + n; for clang the displacement is n minus the
length of the instruction.
