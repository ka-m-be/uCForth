# The JScript bootstrap VM for Windows

`ucf-wsh.js` runs uCForth on Windows with nothing but Windows itself.
The Windows Script Host and its language JScript are a part of each
Windows since Windows 98: no installation, no compiler, no rights of an
administrator.

The program has the same parts, with the same names, as the C bootstrap
VM (`bootstrap/c/ucf.c`), the reference: `bootstrap/c/README.md`
explains each part. This file tells how to start it, and what is
special in JScript.

**Status:** ready; tested on Windows 7 with 64 bits (see "Tests").

## What you need

- Windows 7, 8, 10 or 11, with 32 or 64 bits. The program is written
  for JScript 5.8, the version of Windows 7 to 11. It uses nothing that
  older versions do not have, but older Windows are not tested.
- The folder `ucf` on the computer, for example as `C:\ucf`: from a USB
  stick, a shared folder or a ZIP file.
- A command window: in the start menu, type `cmd`.

## Start

In the command window, in the folder `ucf`:

    C:\>cd \ucf
    C:\ucf>cscript //nologo bootstrap\windows\ucf-wsh.js
    uCForth - JScript bootstrap VM - 16 bit - 165 words in FORTH - built in 656 ms
    ok> 3 4 + .
    7
    ok> bye

The bootstrap VM compiles `forth/core.fs` (Genesis) and `forth/base.fs`
(the Forth interpreter `QUIT`), then the prompt `ok>` comes.

- `cscript` is the Windows Script Host for the command window.
  `//nologo` hides its heading of two lines.
- A double click on `ucf-wsh.js` starts `wscript`, the host for
  windows: it has no console. The program then shows how to start it.
- The console sends a line with the return key; uCForth gets whole
  lines. Ctrl-Z, then the return key, ends the input, and so the
  program. `< nul` gives the program an empty keyboard.
- Ctrl-C ends cscript and so uCForth: this bootstrap VM cannot stop only
  the word that runs.

The options are the same as for the other bootstrap VMs (`-32`, `-pure`,
`-core`, `-base`, `-image`, `-dump-core`, `-dump`, `-check`; see
`bootstrap/c/README.md`). Files come after the options and load before
the keyboard. A name can have `\` or `/`: Windows takes both. Inside
uCForth, the names are as in the sources, with `/`:

    C:\ucf>cscript //nologo bootstrap\windows\ucf-wsh.js host\files.fs
    uCForth - JScript bootstrap VM - 16 bit - 165 words in FORTH - built in 641 ms
    ok> INCLUDE examples/hello.fs
    Hello from uCForth

`FREEZE` saves the image to `ucf.hex` (or to the file in the
environment variable `UCF_IMAGE`: `set UCF_IMAGE=my.hex`), and `-image`
starts it again. The images are the same as the images of the other
bootstrap VMs, so each of them starts the images of the others:

    C:\ucf>cscript //nologo bootstrap\windows\ucf-wsh.js
    uCForth - JScript bootstrap VM - 16 bit - 165 words in FORTH - built in 641 ms
    ok> : HELLO  ." Hello" CR ;
    ok> FREEZE
      saved: ucf.hex
    ok> BYE
    C:\ucf>cscript //nologo bootstrap\windows\ucf-wsh.js -image ucf.hex
    uCForth - JScript bootstrap VM - 16 bit - 164 words in FORTH - image ucf.hex
    ok> HELLO
    Hello

A pipe works too (`type lines.txt | cscript //nologo ...`), but cscript
cannot tell a pipe from the keyboard: the lines of the pipe do not show
after the prompt. Files on the command line are the better way.

## Tests

In the folder `ucf`:

    C:\ucf>cscript //nologo bootstrap\windows\ucf-wsh.js -check
      check core16: the same as seed/core16.hex
      check base16: the same as seed/base16.hex
    C:\ucf>cscript //nologo bootstrap\windows\ucf-wsh.js -32 -check
      check core32: the same as seed/core32.hex
      check base32: the same as seed/base32.hex
    C:\ucf>cscript //nologo bootstrap\windows\ucf-wsh.js host\files.fs tools\test.fs
    uCForth - JScript bootstrap VM - 16 bit - 165 words in FORTH - built in 641 ms

    t0: 183 tests, 0 errors
      redefined HD-A
    hayes: 875 tests, 0 errors
    ...
    8086: 249 tests, 0 errors
    all: 2882 tests, 0 errors
    C:\ucf>echo %ERRORLEVEL%
    0

With `-32` before `host\files.fs`: `all: 12210 tests, 0 errors`. The
whole output is the same as for the C bootstrap VM, line for line
(`bootstrap/c/README.md`, section "Tests"); only the first line
differs.

The fixed point (`tools/README.md`) takes much longer than the tests:

    C:\ucf>cscript //nologo bootstrap\windows\ucf-wsh.js host\files.fs tools\genesis.fs < nul

Expected: twice "the same", as above.

**A target from Windows.** The bootstrap VM builds each target of
`targets/`, the same file, byte for byte, as the other bootstrap VMs.
For example uCForth for MS-DOS (`targets/dos/README.md`):

    C:\ucf>cscript //nologo bootstrap\windows\ucf-wsh.js host\files.fs targets\dos\build.fs < nul

writes `build\ucf.com`, `build\kernel.com`, `build\ucf.scr` and
`build\ucf.lst`. uCForth as a program for Windows itself, with 32-bit
cells (`targets/win32/README.md`):

    C:\ucf>cscript //nologo bootstrap\windows\ucf-wsh.js -32 host\files.fs targets\win32\build.fs < nul

writes `build\ucf-win32.exe`; then `build\ucf-win32` starts it.

**Tested (2026):**

- on Windows 7 with 64 bits (in VirtualBox): the start, `-32 -check`,
  all tests with 16 and 32 bits (1 472 and 1 875 tests, 0 errors; the
  251 tests of the assembler X86 came later), the exit status. The outputs in this file come from there, except
  `-check` with 16 bits, `INCLUDE` and `FREEZE`: these come from Wine
  (next point), with the same text.
- with the JScript of Wine 10 (Debian 13, in Docker), an engine of its
  own that follows the one of Windows: `-check`, all tests, the fixed
  point, with 16 and 32 bits. Wine has no `WScript.StdOut` and no
  `ADODB.Stream`: for this test, the functions of the part "The
  system" wrote with `WScript.Echo` and read text files with the
  `FileSystemObject`, and the "keyboard" came from a file. A typed line
  stands after its prompt, as the console shows it.
- with the engine of Node.js 20, with only the functions of JScript 5.8,
  and with stand-ins for the objects of Windows: all of the above, the
  options and messages (the same as from the C bootstrap VM), the
  keyboard from a pipe, and all targets, byte for byte.

## Speed

On Windows 7 with 64 bits (in VirtualBox), the prompt comes after
0.65 s, with 16 and with 32 bits.

## What is special in JScript

**Only parts of Windows.** The program needs cscript, JScript, and
three objects of Windows: `Scripting.FileSystemObject` (is the file
there?), `ADODB.Stream` (a file as bytes) and `Microsoft.XMLDOM` (bytes
as hex digits). Each Windows has them. The part "The system" of
`ucf-wsh.js` holds all that uses Windows.

**JScript 5.8 is ECMAScript 3, from 1999.** So the program has only
`var` (no `let`), no typed arrays, no `indexOf` for arrays, no `trim`,
no `JSON`. A character of a string comes with `charAt` and
`charCodeAt`: `s[i]` does not work. A list or object must not end with
a comma. The program keeps to all this, so it runs with the JScript of
each Windows.

**Numbers.** JScript has one kind of number: floating point, with 53
bits for integers. Its bit operations take 32 bits and give a number
with a sign; `>>> 0` makes a number without a sign of it. So
`(x & MASK) >>> 0` is x as a cell, for both cell sizes. The low cell of
a product (`*`) and `UM*` come from parts of 16 bits: a product of two
numbers with 32 bits has 64 bits, more than 53. `UM/MOD` divides one
bit at a time when the dividend has two cells.

**Parentheses.** The JScript of Wine reads `a >>> 0 < b >>> 0` in the
wrong way. The program writes `(a >>> 0) < (b >>> 0)`: with parentheses,
each engine gives the same result.

**The memory is an array of cells.** `M[a >> S]` is the cell at the
address a (`S` is 1 for 16-bit cells, 2 for 32-bit cells). A byte is a
part of its cell: `C@` shifts and masks it. So a cell must be at an
address that is a multiple of the cell size, as uCForth keeps it. The
awk and bash bootstrap VMs keep their memory the same way.

**Speed.** JScript finds a local variable much faster than a global
one. So the loop of the VM, `run`, gets the memory, the constants and
the registers as its arguments: inside, they have the names of the rest
of the program, but they are local. `run` executes NEXT and the
primitives 0 to 65 itself, except the long ones (`?RX`, `TX!`, `UM*`,
`UM/MOD`, `MOVE`, `FILL`, `FIND-NAME`): for these and for the HAL, it
gives the registers back to the program (`regs`), calls the function of
the primitive and ends; `vm` then calls `run` again. The primitives
stand in groups, because JScript tests the cases of a `switch` one
after the other.

**Files are bytes, JScript has strings.** The program keeps the bytes
of a file as a string with one character for each byte (0 to 255).
`ADODB.Stream` reads a file as bytes, but JScript cannot look into
them; an element of `Microsoft.XMLDOM` with the data type `bin.hex`
turns them into hex digits, and the program turns these into the
string. Writing goes the other way. `WRITE-FILE` collects the bytes;
the file is complete on the disk at `CLOSE-FILE`, or at the end of the
program.

**Text.** The lines on the console end with CR LF, as Windows writes
text. The files that uCForth writes keep their bytes: images end their
lines with LF, as on each system.

## Limits

The same as for the C bootstrap VM (`bootstrap/c/README.md`, section
"Limits"), and:

- Ctrl-C ends the program; it does not stop only the word that runs.
- cscript cannot tell a keyboard from a pipe: the lines of a pipe do
  not show after the prompt.
- A file that uCForth writes is complete on the disk only at
  `CLOSE-FILE`, or at the end of the program.
- A cell is at an address that is a multiple of the cell size.
