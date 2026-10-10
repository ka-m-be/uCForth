# The uCForth virtual machine

This is the contract for each program that runs uCForth: each bootstrap
VM in `bootstrap/` and each kernel in `kernels/`. If a program keeps this
contract, `forth/core.fs` and `forth/base.fs` run on it without changes,
and its images are the same as the images of all other programs.

The C bootstrap VM (`bootstrap/c/ucf.c`) is the reference: its README
explains each part at the program. The other bootstrap VMs (bash, awk,
Python, JavaScript) have the same parts.

## Contents

1. Overview
2. Cells and addresses
3. The memory map
4. System variables
5. Registers, stacks and threaded code
6. Word headers and wordlists
7. Primitives
8. HAL
9. The console
10. Start, application, images
11. Errors
12. Tests
13. uCForth and other ways to think Forth

## 1 Overview

The virtual machine (VM) is a small computer that exists only as a
program. It has four registers, a memory of bytes and a set of
*primitives*: small operations such as `DUP` or `+`, each with a fixed
number. A Forth word is a list of other words. The VM runs a word, and
each word in the list, until it comes to a primitive.

Only the primitives depend on the real computer. There are 32 mandatory
primitives (numbers 0 to 31), 34 optional primitives (32 to 65) and a
small HAL (hardware abstraction layer, 80 to 99; from 100 on, the words of
one platform). Everything else is Forth
source: the interpreter, the compiler and the drivers.

## 2 Cells and addresses

- Forth Memory is always organized in 8 bit bytes.
- A *cell* is the working width of a VM. It has 16 or 32 bits.
  I.e. it uses 2 or 4 bytes. 16 bits is the reference.
  The cell size is the same for the whole image.
- Numbers in cells are unsigned, modulo 2^16 or 2^32. The same bits are a
  signed number in two's complement when a word says so (`<`, `.`).
- A flag is a cell with all bits 1 (true, -1) or all bits 0 (false).
- Addresses are byte addresses. A cell lies at an address that is a
  multiple of the cell size (2 or 4).
- The low byte of a cell comes first in memory (little endian).

Example with 16-bit cells: the cell at address `$1600` holds `$1234`. Then
the byte at `$1600` is `$34`, and the byte at `$1601` is `$12`.

## 3 The memory map

All addresses are VM addresses. A specific VM maps them to its own memory
as it likes: a bash array, a block of RAM, a segment. Only a kernel for
one fixed target may use the real addresses of the chip.

The following memory areas must exist. The standard map puts them at
these VM addresses. An image holds the addresses (section 10). So VMs
with the same map and the same cell size run the images of each other.

| Area | 16 bit | 32 bit | Contents |
| --- | --- | --- | --- |
| free | `$0000`–`$00FF` | `$0000`–`$00FF` | not used by the VM |
| SYS | `$0100`–`$01FF` | `$0100`–`$03FF` | system variables (section 4), two HALT cells at the end |
| DATA | `$0200`–`$0FFF` | `$0400`–`$FFFF` | RAM data: wordlist records, data of `VARIABLE`, `VALUE`, `BUFFER:`; the pointer is `VP` |
| TIB | `$1000`–`$10FF` | `$10000`–`$100FF` | the input line, 256 bytes |
| PAD | `$1100`–`$11FF` | `$10100`–`$101FF` | scratch area; number output |
| return stack | `$1200`–`$13FF` | `$10200`–`$105FF` | 256 cells; it grows down from `R0` |
| data stack | `$1400`–`$15FF` | `$10600`–`$109FF` | 256 cells; it grows down from `S0` |
| dictionary | `$1600`–`$FFEF` | `$10A00`–`$3FFEF` | words; the pointer is `DP` (`HERE`). The first 8 cells stay free (the guard, below) |
| free | `$FFF0`–`$FFFF` | `$3FFF0`–`$3FFFF` | 16 bytes for the program that runs the VM (DOS and the PC: the registers after `SYSCALL`; Windows: the addresses of two functions for `SYSCALL`) |

With 16 bits the memory is 64 KB; with 32 bits it is 256 KB (63 KB of
RAM data, 189 KB of dictionary): the internal RAM of the ESP32-S3 holds
it, and the ESP32 holds it in two parts of its RAM (below, "Another
map").

**What a program can use.** The map fixes the sizes, the same on each
VM and each target. The image takes a part; the README of each target
tells how much for its image. With base and the package FILES:

| Area | 16 bit | 32 bit | When it is full |
| --- | --- | --- | --- |
| dictionary: words, `ALLOT`, `,` | 58 KB, of it 48 KB free | 189 KB, of it 171 KB free | error −8 (dictionary overflow) |
| RAM data: `VARIABLE`, `VALUE`, `BUFFER:`, `DEFER`, vocabularies | 3.5 KB, of it 2.1 KB free | 63 KB, of it 61 KB free | error −8 |
| data stack | 256 cells | 256 cells | no check: more cells overwrite the return stack; too few give −4 after the word |
| return stack | 256 cells | 256 cells | no check: deeper calls overwrite PAD and the input line |
| the input line (TIB) | 256 characters | 256 characters | the rest of a longer line is lost |
| `S"` at the prompt (in PAD) | 80 characters | 80 characters | the rest is lost |
| a name | 31 characters | 31 characters | error −19 (name too long) |
| the search order | 16 vocabularies | 16 vocabularies | error −49 |

At the prompt, `UNUSED U.` shows the free bytes of the dictionary, and
`ALSO INTERNALS  DATA-END VP @ - U.` those of the RAM data (`U.`: with
16 bits, `.` shows a number above 32767 as negative). A
target can have more RAM data outside the map (below, "An extension of
the RAM data").

| Constant | 16 bit | 32 bit |
| --- | --- | --- |
| `R0`: start of the return stack | `$1400` | `$10600` |
| `S0`: start of the data stack, and of the dictionary | `$1600` | `$10A00` |
| the first word of the dictionary, above the guard | `$1610` | `$10A20` |
| `DICT-END`: end of the dictionary | `$FFF0` | `$3FFF0` |
| `HALT`: a code field with the number 127 | `$01F0` | `$03F0` |
| a thread cell that holds the HALT address | `$01F2` | `$03F4` |
| `WL-FORTH`: the wordlist record of `FORTH` | `$0200` | `$0400` |
| `WL-INTERNALS`: the wordlist record of `INTERNALS` | `$0206` | `$040C` |
| first free byte for `VP` | `$020C` | `$0418` |

The HALT cells let a bootstrap VM run one word and then get control
back: it puts the xt of the word and the HALT address into two cells of
the free area at the top, points IP at them, and runs until primitive
127 comes. A kernel can use them in the same way (section 9,
interrupts).

**The guard.** A VM does not check the stacks (section 11). A word
that takes more items than the data stack holds reads and writes the
cells above `S0`, and `INTERPRET` reports the underflow only after the
word (error -4). So the first 8 cells of the dictionary are free:
Genesis puts the first word above them (`doc/genesis.md`, section 1).
An underflow of up to 8 cells changes only these free cells.

**Another map.** The map above is the standard. It could be another
one: `core.fs` and `base.fs` hold no address of the map, they take each
one from a constant that Genesis makes (`S0`, `TIB`, `DICT-END` ...).
So a VM can have another map, for example for a chip that has no block
of 256 KB. Such a VM must document its map, and it must look closely at
what follows from it. If it moves an area, its images are not the
images of `seed/`: no VM with the standard map can run them, and test 3
of section 12 needs reference images of its own.

Each bootstrap VM and each kernel has the standard map. Where the map
lies in the memory of the computer is a matter of the VM, not of the
map: the ESP32 (`targets/esp32`) has no free block of 256 KB, so its
kernel puts VM addresses 0 to `$FFFF` (SYS and the RAM data) into one
part of its RAM, and VM addresses `$10000` to `$3FFFF` (TIB, PAD, the
stacks and the dictionary) into another part; after `FREEZE` the frozen
part of the dictionary lies in its flash (`targets/esp32/README.md`,
"Memory", and section 13.4). The image and each address in it stay as
they are.

**An extension of the RAM data.** A target can give the RAM data more
memory outside the map, for example the PSRAM of a board
(`targets/esp32s3`). Two variables of `core.fs` (in `INTERNALS`)
describe it: `XDATA-END` is its end (0: no extension) and `XVP` its
next free byte; the addresses are addresses of the target. With an
extension, `VALLOT` (and so `VARIABLE`, `VALUE`, `BUFFER:`, `DEFER` and
new vocabularies) takes memory there; `DATA-ALLOT` always takes memory
in DATA. `MARKER` saves and restores `XVP` too. A kernel then must read
a wordlist record through the same rule as `@` (`FIND-NAME`).

## 4 System variables

The system variables lie at `$0100`, one cell each, in this order:

| Cell | Name | Contents |
| --- | --- | --- |
| 0 | `DP` | the next free byte of the dictionary (`HERE`) |
| 1 | `VP` | the next free byte of the RAM data |
| 2 | `LATEST` | the newest header (nt), also before it is visible |
| 3 | `STATE` | 0: interpret; true: compile |
| 4 | `BASE` | the number base, 10 at the start |
| 5 | `>IN` | the parse position in the input line |
| 6 | `#TIB` | the length of the input line |
| 7 | `CURRENT` | the wordlist for new words |
| 8 | `#ORDER` | the number of wordlists in the search order (1 to 16) |
| 9–24 | `CONTEXT` | the search order: 16 wordlist addresses |
| 25 | `VOC-LINK` | the newest wordlist record; the records form a chain |
| 26 | `CASE-SENSE` | 0: a search ignores upper and lower case |
| 27 | `BASE-END` | the end of the base vocabulary |
| 28 | `FROZEN-END` | the end of the frozen part; below it, the dictionary is read-only |
| 29 | `AUTORUN` | true: `BOOT` starts the application (`TURNKEY`) |
| 30–31 | `ERRMSG` | address and length of a text for an error message |
| 32 | `HANDLER` | the return stack pointer of the newest `CATCH` |

Forth sees each variable as a constant with its address. `BASE` is in the
wordlist `FORTH`, all others are in `INTERNALS`. There are also the
constants `TIB PAD S0 R0 DATA-END WL-FORTH WL-INTERNALS CELL DICT-END`.

## 5 Registers, stacks and threaded code

| Register | Contents |
| --- | --- |
| IP | the address of the next cell of the running word |
| W | the xt of the word that runs now |
| SP | the address of the top cell of the data stack |
| RP | the address of the top cell of the return stack |

Both stacks grow down. `SP` = `S0` means: the data stack is empty. To
push x: SP = SP − CELL, then store x at SP.

**NEXT** is the inner interpreter. It runs after each primitive:

```text
W  = the cell at IP        the xt of the next word
IP = IP + CELL
op = the cell at W         the code field: a primitive number
run primitive op
```

A word with code field 0 (`DOCOL`) is a colon word. Its body is a list of
xts. `DOCOL` saves IP on the return stack and sets IP to the body; `EXIT`
takes IP back. Some primitives use the next cell of the body:

| In the body | Meaning |
| --- | --- |
| `LIT` x | push x |
| `BRANCH` a | continue at the address a |
| `?BRANCH` a | take a flag; if it is 0, continue at a |
| `(DO)` a, `(?DO)` a | a is the address after the loop |
| `(LOOP)` a, `(+LOOP)` a | a is the address of the loop body |
| `(S")` c text | a counted string; the next xt follows at the next cell boundary |

## 6 Word headers and wordlists

A header starts at a cell boundary. Its address is the *name token* (nt):

```text
nt:  link          the nt of the older word in the same wordlist, 0 at the end
     count byte    bits 0-4: length of the name (1 to 31)
                   bit 5 ($20): UNCHECKED: the word changes the return stack on
                                purpose (for a VM that checks it at EXIT)
                   bit 6 ($40): COMPILE-ONLY: only inside a definition; at
                                the prompt the word is error -14
                   bit 7 ($80): IMMEDIATE: the compiler executes the word
                                and does not compile it
     name          the characters, then bytes 0 up to the next cell boundary
xt:  code field    a primitive number
     parameter field
```

The xt is the address of the code field: `xt = nt + CELL + 1 + length`,
rounded up to the next cell boundary.

| Code field | Name | The word … |
| --- | --- | --- |
| 0 | `DOCOL` | is a colon word; the body follows |
| 1 | `DOVAR` | is made by `CREATE`: the cell after the code field is for `DOES>`, the data start at xt + 2 cells |
| 2 | `DOCON` | is a constant: the value follows. A `VARIABLE` is a constant with the address of its cell in DATA. |
| 3 | `DODOES` | is made by `CREATE … DOES>`: like `DOVAR`, then the code at the address in the cell after the code field runs |
| 4 and more | | is a primitive with this number; it has no parameter field |

A **wordlist record** has three cells in DATA: the nt of the newest word,
the address of the next record (chain from `VOC-LINK`), and the nt of the
vocabulary name (0 before `VOCABULARY` names it).

A search goes through the search order from `CONTEXT` cell #ORDER−1 down
to cell 0. In each wordlist it starts at the newest word. A name matches
if the length is the same and all characters are the same; if
`CASE-SENSE` is 0, the letters a–z match A–Z.

## 7 Primitives

Stack effects: `( before -- after )`. `R:` is the return stack. n is a
signed number, u an unsigned number, a an address, c a character, f a
flag, x any cell. "Push" and "pop" mean the data stack.

**Mandatory primitives**, numbers 0 to 31. Each VM must supply them.

| No. | Name | Stack | Effect |
| --- | --- | --- | --- |
| 0 | `DOCOL` | R: `( -- a )` | push IP on the return stack; IP = W + CELL |
| 1 | `DOVAR` | `( -- a )` | push W + 2·CELL |
| 2 | `DOCON` | `( -- x )` | push the cell at W + CELL |
| 3 | `DODOES` | `( -- a )` | push W + 2·CELL; push IP on the return stack; IP = the cell at W + CELL |
| 4 | `EXIT` | R: `( a -- )` | IP = the top of the return stack |
| 5 | `LIT` | `( -- x )` | push the cell at IP; IP = IP + CELL |
| 6 | `BRANCH` | `( -- )` | IP = the cell at IP |
| 7 | `?BRANCH` | `( f -- )` | if f = 0: IP = the cell at IP; else IP = IP + CELL |
| 8 | `EXECUTE` | `( xt -- )` | run the word xt (W = xt) |
| 9 | `@` | `( a -- x )` | the cell at a |
| 10 | `!` | `( x a -- )` | store x at a |
| 11 | `C@` | `( a -- c )` | the byte at a |
| 12 | `C!` | `( c a -- )` | store the low 8 bits of c at a |
| 13 | `DUP` | `( x -- x x )` | |
| 14 | `DROP` | `( x -- )` | |
| 15 | `SWAP` | `( x1 x2 -- x2 x1 )` | |
| 16 | `OVER` | `( x1 x2 -- x1 x2 x1 )` | |
| 17 | `>R` | `( x -- )` R: `( -- x )` | |
| 18 | `R>` | `( -- x )` R: `( x -- )` | |
| 19 | `R@` | `( -- x )` R: `( x -- x )` | |
| 20 | `SP@` | `( -- a )` | push the value that SP had before; with an empty stack this is `S0` |
| 21 | `SP!` | `( a -- )` | SP = a |
| 22 | `RP@` | `( -- a )` | push RP |
| 23 | `RP!` | `( a -- )` | RP = a |
| 24 | `AND` | `( x1 x2 -- x3 )` | bitwise |
| 25 | `OR` | `( x1 x2 -- x3 )` | bitwise |
| 26 | `XOR` | `( x1 x2 -- x3 )` | bitwise |
| 27 | `0<` | `( n -- f )` | true if the top bit of n is 1 |
| 28 | `UM+` | `( u1 u2 -- u3 c )` | u3 = (u1 + u2) modulo 2^bits; c = 1 if there was a carry, else 0 |
| 29 | `?RX` | `( -- c true \| false )` | a character from the console, if there is one (section 9) |
| 30 | `TX!` | `( c -- )` | send the low 8 bits of c to the console |
| 31 | `!IO` | `( -- )` | make the console ready |

**Optional primitives**, numbers 32 to 65. `core.fs` and `base.fs` define
each of them in Forth with `?:`. If a VM supplies the primitive, `?:`
skips the Forth version. The Forth version is the reference for the
effect. Exceptions are in the last column.

| No. | Name | No. | Name | No. | Name |
| --- | --- | --- | --- | --- | --- |
| 32 | `+` | 44 | `<` (signed) | 56 | `FILL` |
| 33 | `-` | 45 | `U<` | 57 | `COUNT` |
| 34 | `1+` | 46 | `*` | 58 | `FIND-NAME` (section 6) |
| 35 | `1-` | 47 | `UM*` | 59 | `(DO)` |
| 36 | `NEGATE` | 48 | `UM/MOD` (see below) | 60 | `(?DO)` |
| 37 | `INVERT` | 49 | `ROT` | 61 | `(LOOP)` |
| 38 | `2*` | 50 | `NIP` | 62 | `(+LOOP)` |
| 39 | `2/` (the sign stays) | 51 | `?DUP` | 63 | `I` |
| 40 | `LSHIFT` (u ≥ bits: 0) | 52 | `2DUP` | 64 | `J` |
| 41 | `RSHIFT` (u ≥ bits: 0) | 53 | `2DROP` | 65 | `UNLOOP` |
| 42 | `=` | 54 | `+!` | | |
| 43 | `0=` | 55 | `MOVE` (the areas may overlap) | | |

`UM/MOD ( ud u -- urem uquot )`: if u is 0, the primitive does `THROW`
−10; if the quotient does not fit in a cell, `THROW` −11. The Forth
version gives −1 −1 in both cases. A program must not depend on either.

**Machine code** (optional): 66 `DOCODE` is a code field. The machine
code of the processor follows it; the kernel jumps there, and the code
ends with NEXT as a primitive does. A kernel with an assembler on the
target has it (`targets/esp32s3` and `targets/esp32`, and `targets/dos`
and `targets/pc-bios` with the kernel of the 8086: `CODE` words);
others give −21. On the 8086 the code is in the segment of the VM, not
in the segment of the kernel: DOCODE puts the far address of NEXT on
the return stack, and NEXT of the code (`lib/pc/asm.fs`) jumps there.
On the ESP32 the CPU runs no code in the RAM of the VM: the cell after
the code field holds the address of the code in the RAM of the
instruction bus (`lib/esp32/esp32/asm.fs`), and the HAL has its own
primitive 66 for it.

The loop primitives keep three cells on the return stack: the address
after the loop (deepest), the limit, the index (top). `I` is the top of
the return stack, `J` the fourth cell. `(+LOOP)` ends the loop when the
index crosses the border between limit−1 and limit.

## 8 HAL

| No. | Name | Stack | Effect |
| --- | --- | --- | --- |
| 80 | `MILLIS` | `( -- u )` | milliseconds since the start, modulo 2^bits |
| 81 | `MS` | `( u -- )` | wait u milliseconds |
| 82 | `US` | `( u -- )` | wait u microseconds |
| 83 | `BIS!` | `( mask a -- )` | set the bits of mask in the byte at a (on 32-bit chips: in the cell at a) |
| 84 | `BIC!` | `( mask a -- )` | clear the bits of mask in the byte at a (on 32-bit chips: in the cell) |
| 85 | `SAVE-IMAGE` | `( -- ior )` | save the image (section 10); ior 0 or an error code |
| 86 | `ERASE-IMAGE` | `( -- )` | delete the saved image |
| 87 | `REBOOT` | `( f -- )` | start again: stacks empty, then `BOOT` (f true) or `QUIT` (f false) |
| 88 | `BYE` | `( -- )` | end the program |

Each image has headers for 80 to 88. A VM that cannot do one of them may
do nothing, or `THROW` −21.

A host (a bootstrap VM, or a kernel for an operating system) also has
these HAL words. The package `host/files.fs` gives them their headers and
adds `INCLUDE`. The bootstrap VMs have 89 to 93, 98 and 99.

| No. | Name | Stack | Effect |
| --- | --- | --- | --- |
| 89 | `OPEN-FILE` | `( a u fam -- fid ior )` | open the file with the name a u; fam 0 read, 1 write, 2 both |
| 90 | `CREATE-FILE` | `( a u fam -- fid ior )` | make a new, empty file; an old file with the name is removed first (macOS keeps the signature of a program for each file) |
| 91 | `CLOSE-FILE` | `( fid -- ior )` | |
| 92 | `READ-FILE` | `( a u fid -- u2 ior )` | read up to u bytes; u2 = 0 at the end of the file |
| 93 | `WRITE-FILE` | `( a u fid -- ior )` | write u bytes |
| 94 | `SERIAL-OPEN` | `( a u baud -- fid ior )` | open the serial line a u: raw bytes, 8 data bits, no parity, no flow control; a read gives the bytes that are there, or none |
| 95 | `SERIAL-LINES` | `( dtr rts fid -- ior )` | set the control lines DTR and RTS (true: active); a line without them (a pseudo terminal) gives −37 |
| 96 | `SERIAL-BAUD` | `( baud fid -- ior )` | a new speed for the open line |
| 97 | `ARG` | `( n -- a u )` | the argument n of the command line; u = 0 if there is none |
| 98 | `READ-LINE` | `( a u fid -- u2 f ior )` | read one line without its end; f = false at the end of the file |
| 99 | `(BYE)` | `( n -- )` | end the program with the exit status n |

The error codes (ior) are 0 (no error), −37 (file I/O error) and −38 (the
file does not exist).

From 100 on, the HAL words of one platform. Their stack effect is the
same everywhere; what the platform does with them is in the README of
the target.

| No. | Name | Stack | Effect | Targets |
| --- | --- | --- | --- | --- |
| 100 | `SYSCALL` | `( x1 … xn n id -- x )` | call the system with n arguments; x is the result. id: the address of a function in the ROM (the ESP32 chips), or of a function of a DLL, x1 the first argument (Windows); the number of an interrupt, x1 … xn in AX BX CX DX SI DI, x = AX (DOS, the PC); the number of a call of the kernel, with bit 16 + k set when argument k + 1 is a VM address, x = the result or minus the error number (macOS, Linux; lib/net/README.md) | `esp32s3`, `esp32` (package ROM), `win32` (package WINDOWS), `dos` (package DOS), `pc-bios` (package BIOS), `macos-a64`, `macos-x64` (package MACOS), `linux-a64`, `linux-x64`, `linux-i386` (packages LINUX-A64, LINUX-X64, LINUX-I386) |
| 101 | `>CHIP` | `( a -- a' )` | the address of the VM address a for the hardware and the system; any other address stays | `esp32s3`, `esp32` (package ROM), `win32` (package WINDOWS) |
| 102 | `+INT` | `( -- )` | the handlers of interrupts on (section 9); −21 if the image has none | `esp32s3`, `esp32` (package INTR) |
| 103 | `-INT` | `( -- )` | the handlers of interrupts off | `esp32s3`, `esp32` (package INTR) |
| 104 | `CODE-INT!` | `( xt n -- )` | the `CODE` word xt is the handler of CPU interrupt n; xt 0: none | `esp32s3`, `esp32` (package INTR, internal) |
| 105 | `FLASH-SYSCALL` | `( x1 … xn n id -- x )` | as `SYSCALL`, for the functions of the ROM that read, write or erase the flash: the cache of the flash is off during the call (the frozen part of the dictionary lies in the flash) | `esp32` (package ESP32); on the `esp32s3` the same name for 100 |
| 110 | `PC@` | `( port -- c )` | a byte from an I/O port | `dos` (package DOS), `pc-bios` (package BIOS) |
| 111 | `PC!` | `( c port -- )` | a byte to an I/O port | `dos` (package DOS), `pc-bios` (package BIOS) |
| 112 | `PW@` | `( port -- x )` | 16 bits from an I/O port | `dos` (package DOS), `pc-bios` (package BIOS) |
| 113 | `PW!` | `( x port -- )` | 16 bits to an I/O port | `dos` (package DOS), `pc-bios` (package BIOS) |

The kernels of the ESP32 chips also use 126 and 127 without a name, as
code fields inside the HAL: 126 lets `MS` and `US` wait on after a
break, 127 is HALT (section 3). A word of a target from 100 on gets its
name in a package of the image (`PARSE-NAME SYSCALL HEADER 100 ,
REVEAL`); the README of the target lists them (`targets/esp32/README.md`,
"The primitives of the HAL").

## 9 The console

The console is a stream of bytes: `?RX` reads, `TX!` writes. `EMIT` and
`KEY` in `core.fs` use them through the vectors `'EMIT` and `'KEY?`.

- A line ends with CR, LF or CR LF. `ACCEPT` takes each of them.
- `QUIT` shows the prompt `ok> ` (in compile state `c> `) at the start of
  a line, then reads a line. A terminal sends the next line only after
  the prompt. So no input buffer overflows, also on a slow target.
- A bootstrap VM reads lines from files and from the keyboard. It gives
  the characters of the next line to `?RX` only when `QUIT` waits in
  `REFILL` (a return address in `REFILL` is on the return stack). If a
  word reads keys while files load, or while the input comes from a pipe
  and not from a keyboard, `?RX` answers "no key". At the keyboard,
  such a word waits for a line; in a web page (`bootstrap/js/ucf.html`,
  `targets/browser`), `?RX` answers "no key" until a line comes.
- `QUIT` always reads from the console. A word in a file that calls
  `QUIT` ends the loading of that file.
- While a file loads, the bootstrap VM hides the prompts. An output line
  that contains `error:` stops the loading; the bootstrap VM names the
  file and the line.
- Break (optional): a kernel can stop the running word when the user
  types Ctrl-C (Linux: the signal SIGINT; a board: the byte 3 in the
  interrupt of the UART). It does this only between two primitives,
  where the VM is complete. With a `CATCH` (`HANDLER` not 0) it runs
  `-28 THROW` ("user interrupt"). Without one (`QUIT` waits in
  `REFILL`, or `BOOT` waits for a key) it empties the stacks, sets
  `STATE` to 0 and runs `QUIT`. Before this, it writes a new line and
  sets `OUT` to 0. A kernel may let the break wait: on the ESP32 boards
  it waits until a Forth handler of an interrupt ends, and while the
  variable `NO-BREAK` of the package IDLE (`lib/idle.fs`) is not 0;
  Ctrl-C twice within 1 s also switches the handlers off, and Ctrl-C
  while another one waits does not wait (`targets/esp32s3/README.md`,
  "Interrupts and errors of the CPU").
- Interrupts (optional): the same break can run a Forth word as the
  handler of an interrupt, between two primitives, on the stacks of
  the word that runs. The kernel puts IP and the next xt on the return
  stack and runs the handler as if its word came first; the handler
  returns to the thread cell of HALT (section 3), and primitive 127
  takes IP and the xt back and runs the xt. `targets/esp32s3` does
  this (its README: "Interrupts and errors of the CPU"). `targets/esp32`
  does the same with HALT cells of its own in the 16 free bytes at
  `$3FFF0`, beside the dictionary (its README, "Memory").

## 10 Start, application, images

**Start.** SP = `S0`, RP = `R0`, `STATE` = 0, `HANDLER` = 0. Then the start
word runs: `BOOT` from `base.fs`, or `QUIT` in a seed. `BOOT` first sets
the search order to FORTH only, with new words in FORTH (`ONLY FORTH
DEFINITIONS`), also when `FREEZE` saved another search order. Without
`TURNKEY`, `BOOT` then runs `QUIT`. With `TURNKEY`, `BOOT` waits 1 s
for ESC or Ctrl-C (then `QUIT`), else it runs `RUN`.

**Application.** `RUN` executes the vector `'SETUP` once, then `'LOOP`
again and again. `TURNKEY` sets `AUTORUN`. `FREEZE` sets `FROZEN-END` to
`HERE` and saves the image (`SAVE-IMAGE`). Below `FROZEN-END` the
dictionary is read-only: a kernel may refuse writes there. Tables made
with `CREATE … ,` are then read-only; data that change belong in
`VARIABLE`, `VALUE` or `BUFFER:`.

**Images.** An image is a text file. The first line names the cell size
and the start words; then come Intel HEX records.

```text
; ucf image bits=16 boot=12352 quit=12352 throw=7996 prims=0-31,80-88
:100100007C302002383000000A00000000000002AD
…
:00000001FF
```

- The first line starts with `; ucf image`. The numbers are decimal:
  `bits` is the cell size; `boot`, `quit` and `throw` are the xts of the
  start word, of `QUIT` and of `THROW`; `prims` lists the primitives that
  the image needs, as ranges.
- A data record is `:` count address `00` data checksum, in hex digits.
  The checksum makes the sum of all bytes of the record 0 (modulo 256).
- A record with type `04` gives the upper 16 bits of the following
  addresses. It comes only for addresses above `$FFFF`.
- The last record is `:00000001FF`.
- The records hold three areas: SYS (`$0100` up to DATA), DATA up to
  `VP`, and the dictionary from `S0` up to `DP`. The stacks, TIB and PAD
  are not in the image.

A VM loads the records into its memory, checks that it supplies all
primitives in `prims`, and starts with `boot`.

**A bootstrap VM without Genesis** (`bootstrap/dos/`) starts from a
seed: its `boot` is its `quit`. The first file of the command line is
then `forth/base.fs`. When `QUIT` wants the next line after this file,
the VM completes the base as Genesis does (doc/genesis.md, section 6),
and `-dump FILE` writes the base image. From `seed/core16.hex` this
image is the same as `seed/base16.hex`. An image that it writes has the
`prims` of the image it loaded.

## 11 Errors

A primitive that finds an error pushes an error code and runs the xt of
`THROW` from the image. `CATCH` in `QUIT` then shows the message.

| Code | Meaning | Who |
| --- | --- | --- |
| −9 | invalid memory address | a kernel after an exception of the CPU (optional) |
| −10 | division by zero | `UM/MOD` |
| −11 | result out of range | `UM/MOD` |
| −21 | unsupported operation | an unknown primitive number |

A VM does not have to check the stacks and the addresses. `INTERPRET`
checks the data stack after each word (−4). A kernel may catch the
exceptions of the CPU (an address without memory) and run −9 `THROW`,
as `targets/rpi4` and `targets/esp32s3` do. The text in `ERRMSG` (set
by `NAME-THROW`) then tells more: `.ERROR` shows it after the message
of −9, −13, −14 and −24, for example `invalid memory address:
$10000000 at pc $4037837E`.

## 12 Tests

A VM keeps this contract if:

1. `forth/test/t0.fs` runs with 0 errors: it tests each primitive and
   each code field type;
2. `forth/test/hayes.fs` runs with 0 errors: the core words;
3. its images are the same as the reference images in `seed/`, byte for
   byte (a bootstrap VM has the option `-check`).

## 13 uCForth and other ways to think Forth

Other Forth systems do the same tasks in other ways. This section puts
them next to the virtual machine of this contract. It is for readers
who know another Forth, and for the writer of a new kernel.
`doc/origins.md` tells where the parts of uCForth come from.

### 13.1 Threaded code

The body of a colon word is a list of the words that it calls. Forth
systems differ in what this list holds, and in what NEXT does with it:

| Model | The body holds | The code field holds | NEXT | Examples |
| --- | --- | --- | --- | --- |
| indirect threaded | the address of a code field, for each word | the address of machine code | W = the cell at IP; jump to the address in the cell at W | fig-Forth, F83, uEforth with GCC |
| direct threaded | the address of machine code, for each word | machine code: a call of the routine for colon words | jump to the address in the cell at IP | eForth 1.0 for the 8086 |
| subroutine threaded | machine code: a call instruction for each word | nothing; there is no code field | the call and the return of the processor | eForth 2 for the 8051, SwiftForth |
| token threaded | a number (a token) for each word | nothing | a table or a `switch` gives the code for the number | jeForth (eForth in Java) |
| **uCForth** | the address of a code field (the xt), for each word | **the number of a primitive** | W = the cell at IP; op = the cell at W; run primitive op | the eForth of C. H. Ting in C, uEforth without GCC |

So uCForth is indirect threaded, with one change: the code field holds
a number, not an address of machine code. This has three effects:

- An image holds no address of the real computer. The same image runs
  on each processor, and in each language, also in bash or awk, which
  cannot jump to an address.
- NEXT has one step more: from the number to the code (a table, or a
  `switch`). A VM in machine code is faster with the other models.
- A kernel is free in its registers and in its table (section 5), but
  not in the form of the image: a colon word stays a list of xts. For
  speed a kernel supplies optional primitives (section 7). Only a
  target with an assembler can have words in machine code (`DOCODE`,
  section 7); an image with such words runs on this target only.

### 13.2 Native code

Many Forth systems of today compile machine code: a call instruction
for each word, short words in line, and for `IF` a jump instruction of
the processor. uCForth compiles cells only: the xt of each word, and
for `IF` the xt of `?BRANCH` with the VM address of its target in the
next cell (section 5). The address is absolute, not a distance.

The reasons:

- The compiler in `core.fs` is the same for each processor. It has no
  word that knows a machine instruction.
- A bootstrap VM in a script language cannot run machine code.
- A list of addresses can lie in ROM or flash, and it can go to another
  computer in an image.
- `SEE` shows each word again as a list of names.

Where speed counts, there are three ways, and none changes the source
of a program: an optional primitive replaces the Forth version of a
word (`?:`); a HAL primitive does what needs an exact time (`US`); a
`CODE` word holds machine code for one target.

### 13.3 Target compilers and metacompilers

Many Forth systems come to a new computer through a *metacompiler*: a
Forth that compiles a new Forth for another machine. It has two
dictionaries at the same time, one for the host and one for the target,
and words such as `:` and `IF` exist twice, with different effects.
This is hard to read and hard to explain. eForth 1.0 avoided it: its
source is a listing for an assembler.

uCForth has no metacompiler, and no target compiler:

1. There is one source, `core.fs` and `base.fs`. The normal compiler
   compiles it into the memory of the VM in which it runs: first
   Genesis (`doc/genesis.md`), then `QUIT` of the image itself.
2. The image of the host is the image of the target. It holds no
   machine code, so it needs no change for another processor. A build
   is: `FREEZE` the running system, assemble a kernel for the target,
   write both into one file (`targets/NAME/build.fs`).
3. Only the kernel is for another machine. An assembler in Forth
   (`asm/`) writes it as data into a memory of the host. This is a
   plain tool with one dictionary.

The condition for step 2: host and target have the same cell size and
the same memory map (section 3). So a host with 32-bit cells builds the
32-bit targets, and a host with 16-bit cells the targets `dos` and
`pc-bios`.

The package VM (`lib/vm.fs`) is the way for the other cases. It is the
virtual machine of this contract in Forth: a second uCForth, the
target, runs inside the host and compiles its own source with its own
compiler. The host only supplies the primitives. `tools/genesis.fs`
builds the reference images in this way. So far the target of the
package has the cell size and the map of its host.

### 13.4 Forth in ROM: the data are not in the dictionary

In a classic Forth, `VARIABLE X` puts the cell of X into the body of X,
in the dictionary. Then the dictionary must be RAM. A system for a ROM
needs one more trick: it copies the ROM into RAM at the start, or it
keeps its variables in a table that the start copies (the user area of
eForth), or it has a second pointer for RAM.

In uCForth the second pointer is the rule:

| | Dictionary (`DP`, `HERE`) | RAM data (`VP`) |
| --- | --- | --- |
| Contents | headers, code fields, bodies; tables and texts made with `CREATE`, `,` and `S"` | the cells of `VARIABLE`, `VALUE`, `BUFFER:` and `DEFER`; the wordlist records; the system variables |
| After `FREEZE` | read-only below `FROZEN-END` | can change; the image holds the start values |

- A `VARIABLE` is a constant with the address of its cell in DATA
  (section 6).
- No word of `core.fs` and `base.fs` needs to write below
  `FROZEN-END`. A new word goes to the end of the dictionary, and the
  newest word of each wordlist is in its record in DATA. `MARKER` and
  `HIDE` stop with an error before they write into the frozen part.
- An image holds SYS and DATA too (section 10). So after a start each
  variable has the value that it had at `FREEZE`.

So a kernel can leave the frozen part in ROM or flash, and keep only
DATA, the stacks, TIB, PAD and the new words in RAM. The kernel of the
ESP32 does this: after `FREEZE` it reads the frozen part from the flash,
through the cache, and a store there gives -9; the RAM of the frozen
part can then hold RAM data (`targets/esp32/README.md`, "Memory"). The
other kernels have the whole image in RAM.

### 13.5 I/O that is not in the memory

A driver in uCForth is Forth: it reads and writes the registers of the
hardware with `@`, `!`, `C@`, `C!`, `BIS!` and `BIC!`. This needs the
registers at addresses that `@` reaches. The cases:

| Case | Way | Example |
| --- | --- | --- |
| registers in the memory; VM addresses are chip addresses | `@` and `!` | `targets/rpi4` |
| registers in the memory; the memory of the VM starts at a base address | the kernel takes an address outside the map as a chip address; `>CHIP` (101) gives the chip address of a VM address | `targets/esp32s3` |
| the processor has I/O ports of their own (8080, Z80, 8086) | the HAL words `PC@ PC! PW@ PW!` (110 to 113) | `targets/dos`, `targets/pc-bios` |
| an operating system owns the hardware | the HAL words for files and serial lines (89 to 99); also `SYSCALL` (100) for each call of the system (Linux, macOS, DOS, Windows) | `targets/linux-a64`, `targets/macos-a64`, `targets/dos` |
| code and data are in different memories (AVR, 8051) | the kernel puts both into the VM addresses | not built yet |
| a step needs an exact time | a HAL primitive (`US`), or a `CODE` word | `targets/esp32s3` |

A bootstrap VM has no registers at all: its memory is an array. The
rule for each case is the same: a HAL word exists only for what Forth
cannot do itself. Each driver above it is Forth source.
