# Origins - Where the concepts of uCForth come from

uCForth has few ideas of its own. It takes ideas from more than fifty
years of Forth and puts them together for one aim: a small Forth that a
person can build from nothing, understand as a whole, and bring to a
new computer. This file names the sources. For each part it tells what
uCForth took, and what it changed.

## Contents

1. Forth itself
2. Why Forth, and why from nothing: Collapse OS
3. The primitives: eForth
4. The virtual machine and the xt
5. The seed and Intel HEX
6. The tests
7. Genesis
8. The assemblers
9. Freeze
10. More loans
11. Sources

## 1 Forth itself

Charles H. Moore made Forth around 1970, to control a radio telescope
with a small computer. His design is still the whole of Forth: a stack
for the data, a dictionary of short words, and an interpreter and a
compiler that are words themselves. The programmer tests each word at
the keyboard, on the machine that the program is for (`doc/forth.md`).

Three later things are important here:

- **fig-Forth** (Forth Interest Group, from 1978): one Forth model as a
  listing for the microprocessors of its time. A person with an
  assembler could bring Forth to a computer. eForth and uCForth follow
  this aim.
- **The standards**: Forth-79, Forth-83, ANS Forth (1994) and
  Forth-2012. uCForth takes its names and their effects from
  Forth-2012, so a reader of a Forth book knows the words. It is not a
  standard system: the base has only the 165 words that a small
  controller needs.
- **The books of Leo Brodie**, *Starting Forth* (1981) and *Thinking
  Forth* (1984), teach Forth and its style of small words with clear
  names. Collapse OS names *Starting Forth* as its textbook.

## 2 Why Forth, and why from nothing: Collapse OS

Collapse OS (Virgil Dupras, since 2019) is a Forth operating system
with one purpose: "preserve the ability to program microcontrollers
through civilizational collapse". It must run on machines from old
parts, and a person must be able to build it "without external
resources (i.e. internet)". Its order of aims is simplicity, then
compactness, then speed, and the whole must stay "understandable in its
whole by a single person".

uCForth asks the same question without the collapse: what is the least
that you need to get a programming system for small computers, if you
have only a minimal computer and no tools? The aim is to learn how
computers work from the first byte on, as people did in the first years
of the personal computer.

Forth fits this task well:

- The interpreter and the compiler have a few kilobytes.
- Only a small part depends on the machine (section 3).
- The compiler is made of Forth words, so Forth compiles itself
  (`doc/forth.md`, section 8).
- You test hardware at the prompt, one register at a time.
- One person can read all of it.

| From Collapse OS | In uCForth |
| --- | --- |
| no tool from outside: the system and text files are all | no compiler, no make, no download; each tool is a Forth application |
| 34 native words, all other words in Forth | 32 primitives (section 3) |
| `?:` defines a word only if no native word has the name | `?:` and the optional primitives 32 to 65 |
| `STACK?` checks the stack after each word of a line | `?STACK` in `INTERPRET` |
| a seed and a small "bootstrap VM" to start without a running Collapse OS | the terms *seed* and *bootstrap VM* (section 5) |
| `$` for hex numbers, `'A'` for a character | the same (they are in Forth-2012 too) |
| manuals as plain text, for a reader without the internet | manuals for a reader with a handbook of the 1980s |

What is different: Collapse OS builds a new system with a cross
compiler that runs in Collapse OS ("To build Collapse OS, you need
Collapse OS"); its seed is a binary image for a 6502, and an emulator
in C runs it. uCForth needs no running uCForth and no C compiler: a
bootstrap VM is a script in a language that the computer has already,
and it compiles the Forth source itself (section 7). uCForth has no
blocks and no editor; it has vocabularies and files on the host.

## 3 The primitives: eForth

eForth 1.0 (Bill Muench and C. H. Ting, 1990) is a model of a Forth
that is easy to port. Only 31 words are machine code; about 190 words
are Forth. Ting gives the rule for the 31: "The selection of words in
this kernel is based on the criteria that they are very difficult if
not impossible to synthesize from other primitive words." So the only
arithmetic is `UM+`, an addition that gives the carry, and the only
comparison is `0<`. Ting expected a port to a new processor in four
weeks; others did it in one.

The 32 mandatory primitives of uCForth are 29 of these 31, and three
more kinds of code field:

| eForth 1.0 | uCForth |
| --- | --- |
| `doLIST`, `doLIT`, `EXIT`, `EXECUTE`, `branch`, `?branch` | `DOCOL`, `LIT`, `EXIT`, `EXECUTE`, `BRANCH`, `?BRANCH` |
| `@ ! C@ C!`, `DUP DROP SWAP OVER`, `>R R> R@`, `SP@ SP! RP@ RP!`, `AND OR XOR 0<`, `UM+` | the same |
| `?RX TX! !IO`: the console in three words (`?RX` is an idea of Bill Muench) | the same |
| `BYE` | a word of the HAL |
| `next`, for the loop `FOR` ... `NEXT` | not there; `DO` ... `LOOP` is Forth |
| a variable is a colon word; there is no `CONSTANT` and no `DOES>` | `DOVAR`, `DOCON`, `DODOES` are primitives, as in each later eForth in C |

eForth gave more than the list:

- `UM*` and `UM/MOD` in Forth, with `UM+` and one loop step for each
  bit (`forth/core.fs`, section 5).
- `CATCH` and `THROW` as the one way for errors.
- A new word is visible only at its `;`: there is no bit that hides it
  (`OVERT` in eForth, `REVEAL` here).
- The header: a link, a count byte with the bits for IMMEDIATE ($80)
  and COMPILE-ONLY ($40), the name, filled up to the next cell; in the
  later form of eForth, where the code follows the name.
- The console through vectors (`'EMIT`, `'KEY?`).
- The advice to write the slow words again in machine code after a
  port. uCForth has a fixed way for this: the optional primitives with `?:`.

uCForth did not take `FOR` ... `NEXT`, the vector `'EVAL` in place of
`STATE`, the user area, and the two dictionaries for names and code
(Ting put them together again in 1999).

## 4 The virtual machine and the xt

eForth describes a "virtual Forth machine" with the registers IP, SP,
RP and WP. uCForth has the same four (`doc/vm.md`, section 5). The name
*execution token* (xt) is from ANS Forth: a number that stands for a
word, and the standard does not tell what it is. In uCForth it is the
address of the code field.

eForth 1.0 has machine code in each code field. A program in C cannot
put machine code into a dictionary. So in the eForth of Ting in C
(Arduino eForth 2.0, 2011) the code field holds byte codes, and a table
of C functions runs them; he called it a "FORTH Virtual Machine".
uEforth (Brad Nelson, since 2021) does the same with a number and a
`switch`, where the C compiler cannot jump to a label. The C eForth for
the Arduino also maps one row of addresses to the RAM and the flash of
the chip.

uCForth takes this form for another reason: an image without machine
code is the same for each processor, and a script in bash or awk can
run it. What uCForth added is the contract (`doc/vm.md`): fixed numbers
for the primitives on all machines, optional primitives, one memory
map, one image format, and the rule that each VM builds the same image,
byte for byte. `doc/vm.md`, section 13, compares this with other Forth
systems.

## 5 The seed and Intel HEX

The words *seed* and *bootstrap VM* are from Collapse OS. There the
seed is needed always as a binary file. In uCForth a computer with a script
language needs no seed: Genesis compiles `core.fs`. The seed is for the
smallest computers only, where a monitor program such as DEBUG puts the VM
into the memory byte by byte (`seed/README.md`).

All files of uCForth are text, also the seed and each image. There are
two reasons. A shell, awk, JScript or BASIC read lines of text well and
bytes badly. And text goes through each way that an old computer has: a
diskette, a serial line, the keyboard, paper.

Intel HEX is the text format for the contents of a memory that Intel
made in the 1970s for its development systems. Each line has a count,
an address, the bytes and a check sum. It fits here:

- A loader has a few lines of code, and it checks each line.
- EPROM programmers and the monitor programs of old computers read it.
- It was in the eForth line already: the eForth of C. H. Ting for the
  8051 sent its system as Intel HEX.

uCForth added one comment line at the start, with the cell size, the
start words and the primitives that the image needs (`doc/vm.md`,
section 10). An image with 16-bit cells has only data records and the
end record, so its loader stays small.

## 6 The tests

John Hayes (Johns Hopkins University, Applied Physics Laboratory)
wrote a small tester and a set of tests for the core words of ANS Forth
in 1995. A test is one line: code, an arrow, the expected stack.

    T{ 1 2 3 SWAP -> 1 3 2 }T

Later authors changed his `{` and `}` to `T{` and `}T`; this form is in
the test suite for Forth-2012. uCForth has a new tester with this
syntax (`forth/test/tester.fs`) and the tests of Hayes, adapted to the
words of uCForth, with many more for its own words
(`forth/test/hayes.fs`).

uCForth added three kinds of tests:

- **`t0.fs`**: each primitive with its border cases. Each VM must pass
  it, with the same output.
- **The reference images**: each bootstrap VM must build the images of
  `seed/`, byte for byte. This one test finds almost each fault of a
  new VM. At the end uCForth builds the same images itself
  (`tools/genesis.fs`): the fixed point.
- **A foreign tool as the oracle**: the bytes of each assembler were
  compared once with a foreign assembler, and the results are data in
  the test. A foreign tool is never a part of the build.

A test belongs to its package: it is in the folder `test/` next to the
file.

## 7 Genesis

The compiler of Forth is written in Forth. So who compiles the first
line? Other systems give these answers:

| Answer | Example | Price |
| --- | --- | --- |
| a metacompiler: a running Forth compiles the new one | F83, Collapse OS | you need a running Forth; two dictionaries at once |
| the whole Forth as a listing for an assembler | fig-Forth, eForth 1.0 | you need an assembler; each colon word is a list of data lines |
| the first part of the compiler in the language of the kernel | eForth in C, uEforth | this part stays in the other language |

Ting chose the listing on purpose: "it takes a whole book to explain
how to build your 8051 Forth using metacompilation technique".

Genesis is the answer of uCForth. It is a loop of 200 to 300 lines in
the language of the bootstrap VM: take a word, find it, execute or
compile it, else read a number. Nine *host primitives* help it
(`HEADER`, `,` and others). Genesis compiles only `forth/core.fs`. This
file defines a Forth version of each host primitive, and at its end the
interpreter `QUIT`. Then Genesis takes its host primitives out of the
dictionary and stops. The image holds only Forth from then on, and
`QUIT` compiles all other source (`doc/genesis.md`).

Genesis comes from the first uCForth, a simulator in Python (2026). Its
interpreter loop read all source at first. When the interpreter became
Forth, the loop stayed for the first file.

The results: there is no metacompiler, and one source for all machines.
A bootstrap VM in a new language needs only this loop. And a computer
that is too small for the loop starts from the seed.

## 8 The assemblers

An assembler in Forth is an old Forth idea: each instruction is a word
that puts its bytes into memory. The operands come first, as always in
Forth, and the word ends with a comma. Forth systems have had such
assemblers since the 1970s; Collapse OS and uEforth have them too. The
Xtensa assembler of uEforth showed the form and the size for the first
assembler here.

The assemblers of uCForth (`asm/`) are new. Their encodings are
from the manuals of the makers: the Xtensa Instruction Set Architecture
Reference Manual, the Arm Architecture Reference Manual, and the Intel
iAPX 86/88 User's Manual. uCForth added these rules:

- The operands have the order of the manual of the processor. So a
  line compares with the listing of another assembler, or of DEBUG.
- A number as operand has an instruction word of its own with an I
  (`MOVI,` `ADDI,`). The sign `#` of older Forth assemblers would hide
  the `#` of the number output.
- `IF,` `THEN,` `BEGIN,` `UNTIL,` make short jumps without labels.
- The tests hold the bytes of a foreign assembler (section 6).

The kernels are written with these assemblers (`kernels/`). So uCForth
builds its own machine code, and no C compiler is needed, as in
Collapse OS.

## 9 Freeze

The task: an application that is complete starts by itself when the
power comes. The parts and their sources:

| Part | From |
| --- | --- |
| two words for an application, one for the start and one that runs again and again (`'SETUP`, `'LOOP`, `RUN`) | `setup()` and `loop()` of the Arduino |
| `TURNKEY`: the system starts the application | the turnkey systems of classic Forth |
| one second at the start in which a key gives the prompt | the safe boot of some eForth ports (a pin), the start window of uEforth |
| save the running system as the new start | `SAVESYSTEM` and other words of eForth ports; `save` of uEforth |
| data that change are not in the dictionary (`VP`) | eForth ports for ROM systems: H8/532, 8098, 8051 |
| start values of the variables come from the saved system | the table of the user variables in the ROM of eForth |

`FREEZE` puts these together: one word saves the running system, and
the system that starts next is exactly the one that you tested. The
word is the same on each machine: a bootstrap VM writes a file, a board
writes its flash. A build for a target is a `FREEZE` too
(`targets/README.md`). Two rules of uCForth stand behind it: the
dictionary below `FROZEN-END` is read-only, and the data that change
are outside of it. So the frozen part can stay where it lies, also in a
ROM or a flash (`doc/vm.md`, section 13.4).

## 10 More loans

| Part of uCForth | From |
| --- | --- |
| `ONLY`, `ALSO` and the search order | the proposal of William Ragsdale in Forth-83; eForth has the mechanism |
| `PACKAGE` ... `PUBLIC` ... `PRIVATE` ... `END-PACKAGE` | SwiftForth |
| `HIDE` | SwiftForth; pForth has `PRIVATIZE` |
| `MARKER` in place of `FORGET` | Forth-2012 advises it |
| `VALUE`, `TO`, `BUFFER:`, `[DEFINED]`, `PARSE-NAME`, the signs `$ # %` | Forth-2012 |
| a vocabulary for the internal words | uEforth has `internals` |
| the search without upper and lower case | later eForth ports, uEforth |
| the prompt as the sign "send the next line" | eForth sends a character after each line for this |
| division that rounds toward zero | C |

## 11 Sources

| Source | What |
| --- | --- |
| Bill Muench, C. H. Ting: eForth 1.0 (1990), with `EFORTH.ASM` and the text "eForth Overview" | the model, the 31 primitives |
| C. H. Ting: *eForth and Zen* (1993) | the history and the aims of eForth |
| C. H. Ting: *eForth as Arduino Sketch* (Arduino eForth 2.0, 2011) | the virtual machine in C |
| Brad Nelson: uEforth and ESP32forth 7 | a Forth that starts from a small kernel in C |
| Virgil Dupras: Collapse OS, the files in `doc/` and `vm/` | the aim, `?:`, the seed |
| John Hayes: `tester.fr` and `core.fr` (1995) | the tester and the core tests |
| Forth 200x committee: Forth-2012 | the names and the effects of the words |
| Leo Brodie: *Starting Forth* (1981), *Thinking Forth* (1984) | how to write Forth |
| the manuals of the makers of the processors | the encodings in `asm/` |
