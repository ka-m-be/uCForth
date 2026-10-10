# win32: uCForth for Windows (32 bit)

The result is `build/ucf-win32.exe`: one program for the command window
(the console) of Windows. It calls only functions that each Windows has
since Windows 95 and NT 4, and a Windows with 64 bits runs it too
(WoW64). The file holds the virtual machine in x86 machine code and the
image. It needs no other file and no DLL but `KERNEL32.dll`, a part of
each Windows.

The build also writes `build/ucf-win32.exe.hex`: the same bytes as text.
On a Windows that gets only text (a mail filter, a copy by hand), the
command `certutil` of Windows makes the program of it (see "The program
from text").

## Build

In the folder `ucf`, with a bootstrap VM with 32-bit cells:

    build/ucf-c -32 host/files.fs targets/win32/build.fs </dev/null

The output ends with:

      saved: ucf.hex
      code: 9368 bytes, image: 86824 bytes
      written: build/ucf-win32.exe (98304 bytes), build/ucf-win32.exe.hex (6144 lines)

Each bootstrap VM builds the same two files, byte for byte (tested:
bash, awk, C, Python, JavaScript, JScript). On Windows itself, the
JScript bootstrap VM builds them (`bootstrap/windows/README.md`):

    C:\ucf>cscript //nologo bootstrap\windows\ucf-wsh.js -32 host\files.fs targets\win32\build.fs < nul

`build.fs` does three things:

1. `FREEZE`: the running system is the image: `base.fs`, the package
   FILES and each file before `build.fs` on the command line.
2. The assembler `asm/x86.fs` makes the code: the kernel
   `kernels/x86.fs` and `hal.fs` (the start, the console, time, files,
   serial lines, Ctrl-C, the calls of Windows). The package PE
   (`lib/windows/pe.fs`) makes the headers of the file and the list of
   the functions of `KERNEL32.dll` that the code calls.
3. It writes the program, and the same bytes as text.

## Run

In the command window (start menu: `cmd`), in the folder `ucf`:

    C:\ucf>build\ucf-win32
    ok> 3 4 * .
    12
    ok> BYE

A double click on the file opens a console window with the prompt too.

- The console shows the line while you type; the return key sends it.
- Ctrl-C stops the word that runs now and gives the prompt again:

      ok> : X BEGIN AGAIN ;  X
        error: user interrupt
      ok>

  Ctrl-C also ends a wait (`MS`) at once.
- `BYE` ends the program; Ctrl-Z, then the return key, ends the input,
  and so the program at the prompt.
- Files: `ALSO FILES  INCLUDE examples/hello.fs`. A name can have `/`
  or `\`.
- The arguments of the command line: `n ARG ( -- a u )` (package FILES).
  Spaces part them; a text in double quotes is one argument. `0 ARG` is
  the program.
- The exit status: `n (BYE)`; in `cmd`, `echo %ERRORLEVEL%` shows it.
- An address outside the program (for example `$7FFFFFF0 @`) is the
  error -9 ("invalid memory address"), not the end of the program.

## Test

    C:\ucf>echo ALSO FILES  INCLUDE tools/test.fs | build\ucf-win32
    ...
    all: 10979 tests, 0 errors
    C:\ucf>echo %ERRORLEVEL%
    0

The package WINDOWS (section "Functions of Windows") has tests of its
own: they run only on Windows.

    C:\ucf>build\ucf-win32
    ok> ALSO FILES  S" forth/test/tester.fs" INCLUDED
    ok> ALSO FILES  S" lib/windows/test/windows.fs" INCLUDED
    windows: 14 tests, 0 errors

The monitor for Windows (MON-WIN32: registers, calls of code at any
address, memory that runs) has its tests too: `asm/monitor/README.md`,
section "Test".

On the Mac, the program runs in Wine in Docker. A 32-bit program needs
the platform `linux/386` (Docker on Apple silicon runs `linux/amd64`
with Rosetta, and Rosetta cannot run 32-bit x86 code; `linux/386` runs
in QEMU). The image, once (about 5 minutes):

    printf 'FROM debian:trixie-slim\nRUN apt-get update && apt-get install -y --no-install-recommends wine wine32 socat\n' \
      | docker build --platform linux/386 -t ucf-wine386 -

Then, in the folder `ucf`:

    docker run --rm --platform linux/386 -v "$PWD":/u -w /u ucf-wine386 \
      sh -c 'echo "ALSO FILES  INCLUDE tools/test.fs" | wine build/ucf-win32.exe'

The output ends with `all: 10979 tests, 0 errors`, after about 9 s:
first Wine makes its folder in the new container and writes some
messages of its own; the tests themselves take about 2 s. A serial line
in Wine: `socat` makes two connected pseudo terminals, and a link in
the folder `dosdevices` of Wine makes one of them `COM5`.

**Tested (2026):**

- on Windows 7 with 64 bits (in VirtualBox): the start, Ctrl-C, all
  tests, the exit status, `ARG` with quotes, `certutil` (`fc /b`: the
  same bytes as the program), the tests of WINDOWS and a message box
  (`MessageBoxA`).
- in Wine 10 (Debian 13, i386, in Docker): all tests and the tests of
  WINDOWS; `ARG` with quotes; the exit status; the end of the input (a
  pipe, `/dev/null`); `MS`; Ctrl-C at a pseudo terminal (a word in a
  loop, and `MS`); `-9` for a wrong address in `@`, `MOVE`, `FILL`;
  the serial line (send, receive, the speed; a port that is not there);
  the tool `ucf.exe`; an application with WINDOWS.
- Not yet tested: a real serial port.

## An application, and the tool ucf

Files between `host/files.fs` and `build.fs` go into the program. With
`TURNKEY` it starts by itself; an application may define `ON-BOOT` (the
program starts with it instead of `BOOT`) and `PROGRAM ( -- a u )`, the
name of the file without `.exe`. The tool `ucf` (`tools/ucf.fs`) does so:

    build/ucf-c -32 host/files.fs tools/ucf.fs targets/win32/build.fs </dev/null

gives `build/ucf.exe`. On Windows, the port of a board is `COM3` or
similar (the Device Manager shows it, under "Ports"):

    C:\ucf>build\ucf term COM3
    C:\ucf>build\ucf flash COM3 build\esp32s3.bin 0

`tools/README.md` and `host/README.md` explain the commands.

## Functions of Windows

Windows has thousands of functions in its DLLs: `KERNEL32.dll` (files,
time, processes), `USER32.dll` (windows, keyboard, messages),
`GDI32.dll` (drawing) and more. The program itself calls 26 functions
of `KERNEL32.dll`: the list is at the start of `hal.fs`. An application
calls each function of each DLL with the package WINDOWS
(`lib/windows/windows.fs`):

    ok> ALSO FILES  INCLUDE lib/windows/windows.fs
    ok> LIBRARY USER32.dll
    ok> 4 WINAPI: MessageBoxA
    ok> : HELLO  0 Z" Hello from uCForth" >CHIP Z" uCForth" >CHIP 0 MessageBoxA DROP ;
    ok> HELLO

Windows shows a small window with the text and the button OK; `HELLO`
ends when you click it. After `INCLUDE`, WINDOWS is in the search
order; in a program with the package in its image, `ALSO WINDOWS` gives
its words.

| Word | Stack | Meaning |
| --- | --- | --- |
| `LIBRARY name` | `( -- )` | the DLL for the next `WINAPI:` |
| `n WINAPI: name` | `( n -- )` | makes the word name `( x1 .. xn -- x )`: it calls the function name of the DLL with n arguments, x1 is the first, and gives its result x. The first call finds the function; a function that is not there gives the error -13 with its name |
| `Z" text"` | `( -- a )` | in a definition: the text with a byte 0 at the end, as Windows wants it |
| `>CHIP` | `( a -- a' )` | the address for Windows of the VM address a: a + `$10010000` (HAL 101); any other address stays |
| `FIND-FUNCTION` | `( a1 u1 a2 u2 -- a )` | the address of the function a2 u2 of the DLL a1 u1; 0: it is not there |
| `SYSCALL` | `( x1 .. xn n a -- x )` | call the function at the address a with n arguments (HAL 100) |

The rules:

- The name of the function as Microsoft writes it, with the same upper
  and lower case. A function with a text has two forms: `MessageBoxA`
  takes a byte for each character, `MessageBoxW` two bytes. Take the
  form with A.
- The arguments in the order of the documentation of Microsoft
  (learn.microsoft.com): the first argument lies deepest on the stack.
  A number, a handle, an address: each is one cell.
- Each address that goes to Windows needs `>CHIP`: a text, a buffer, a
  structure. A VM address means nothing to Windows.
- An address that Windows gives back points outside the VM: `@` cannot
  read there (-9). `RtlMoveMemory` of `KERNEL32.dll` (3 arguments: to,
  from, the number of bytes) copies the bytes into a buffer of the VM.
- An error inside the function (Windows reads at a wrong address) gives
  the error -9, as `@` does; the program goes on.
- Ctrl-C waits until the function returns.
- No callbacks: a function that calls a function of the program does
  not work, for example a window of its own (`CreateWindowExA` with a
  window procedure) or `EnumWindows`. A message box works.

How it works: `SYSCALL` pushes the n cells onto the stack of the
processor, the last one first, calls the address, and then sets the
stack pointer back. So a function with the convention of C, which
leaves its arguments on the stack (`wsprintfA`), works as well as the
usual convention of Windows (stdcall). At the start, the program puts
the addresses of `LoadLibraryA` and `GetProcAddress` into the last 8
bytes of the memory of the VM (doc/vm.md, section 3: free);
`FIND-FUNCTION` calls them.

An application with WINDOWS loads the package before its own files:

    build/ucf-c -32 host/files.fs lib/windows/windows.fs app.fs targets/win32/build.fs </dev/null

## The program from text

`build/ucf-win32.exe.hex` holds each byte of the program as two hex
digits, 16 bytes in a line. On the Windows computer (Windows 7 and
later have `certutil`):

    C:\ucf>certutil -decodehex ucf-win32.exe.hex ucf-win32.exe

So the program goes as text, the way `bootstrap/dos/ucf.scr` brings the
program for DOS as a script for DEBUG.

## Memory

Each program on Windows has addresses of its own: 4 GB for a program
with 32 bits, and the lower 2 GB of them are for the program. These are
virtual addresses. Windows maps each page of 4 KB that the program uses
to a page of the physical memory (or of the page file), when the program
first uses it. So `$10000000` is not a place in the RAM chips, and each
program has its own `$10000000`.

The file has no relocations. Relocations are a list of the places in the
code that hold an address: with it, Windows could load the program at
another address and correct these places. Without it, Windows loads the
program at the address that the file gives, `$10000000` (IMAGE-BASE in
`lib/windows/pe.fs`), or not at all. So the code uses fixed addresses,
and each access to the memory of the VM is one instruction.

| Address | Size | Contents |
| --- | --- | --- |
| `$10000000` | 4 KB | the headers of the file (PE32) |
| `$10001000` | 4 KB | the imports: the cells where Windows puts the addresses of the functions of `KERNEL32.dll` |
| `$10002000` | 56 KB | the code: the table of the primitives, the kernel, the HAL (9 KB of the 56 KB); read and execute |
| `$10010000` | 256 KB | the memory of the VM: VM address 0, the image; read and write |
| `$10050000` | 16 KB | the scratch area of the HAL: handles, the input line, the arguments for `ARG` |
| from Windows | 1 MB | the stack of the processor, for the calls of Windows |

The program takes 336 KB of addresses, and so a little more than 336 KB
of memory. The memory of the VM has the standard map (doc/vm.md, section
3). After the start, with the image of `build.fs` (base and FILES):

| Area | Size | Free |
| --- | --- | --- |
| dictionary: words, `ALLOT`, `,` | 189 KB | 171 KB |
| RAM data: `VARIABLE`, `VALUE`, `BUFFER:`, vocabularies | 63 KB | 61 KB |
| data stack, return stack | 256 cells each | |
| the input line | 256 characters | |

`UNUSED U.` shows the free bytes of the dictionary,
`ALSO INTERNALS  DATA-END VP @ - U.` those of the RAM data. A full
dictionary or RAM data gives the error -8. doc/vm.md, section 3, has
the other limits of the VM.

More: a file name has at most 255 characters; `ARG` gives the arguments
0 to 31, each with at most 255 characters; Windows reads at most 2048
bytes of the input at a time. Files: as many open at once as Windows
allows.

## How it works

The registers of the virtual machine are ESI (IP), EBX (W), EDI (the
data stack) and EBP (the return stack); ESP is the stack of the
processor for the calls of Windows (`kernels/x86.fs`). A call of Windows
keeps these registers, so the HAL needs no copies.

- **The console:** a second thread reads the standard input with
  `ReadFile`, a line at a time, and waits until `?RX` has taken it.
  `?RX` waits at most 1 ms for a line. So `KEY?` never stops the program,
  and the console of Windows shows and edits the line while you type. A
  pipe and a file go the same way.
- **Ctrl-C:** Windows calls the handler of the program in a thread of
  its own. The handler sets one cell (`BREAK-OFF` in the kernel): from
  then on the dispatch of the next primitive runs BREAK. So NEXT needs no
  test for the break.
- **Faults:** an exception of the thread of the VM goes to a filter
  (`SetUnhandledExceptionFilter`): in the code of the program (a wrong
  address), or in a function of Windows that `SYSCALL` called. The filter
  sets the program counter to FAULT, which runs `-9 THROW`.

## Limits

- `SAVE-IMAGE` (and so `FREEZE`) gives the error -21, as on macOS and
  Linux: `build.fs` on a bootstrap VM makes the programs.
- `MS` and `US` wait at least as long as they should; the timer of
  Windows often makes it up to 15.6 ms more. A wait of Windows alone can
  end up to one step of this timer too early: `MS` then waits again,
  until the clock of `MILLIS` (the performance counter) shows the time.
  `US` waits in whole milliseconds.
- The lines of the console end with CR LF. `ACCEPT` takes CR LF as one
  end; `KEY?` right after a typed line sees the LF.
- The program has no relocations (section "Memory"). A Windows with the
  option "Do not allow stripped images" of "Mandatory ASLR" (Exploit
  protection; off by default) refuses such a program.
- No callbacks from Windows into Forth (section "Functions of
  Windows").
