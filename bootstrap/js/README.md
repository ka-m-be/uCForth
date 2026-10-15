# The JavaScript bootstrap VM

`ucf.js` runs uCForth in a web browser and in Node.js. In a browser,
uCForth needs nothing but the folder `ucf`: no installation, no
compiler, no server.

The program has the same parts, with the same names, as the C bootstrap
VM (`bootstrap/c/ucf.c`), the reference: `bootstrap/c/README.md`
explains each part. This file tells how to start it, and what is
special in JavaScript.

## In a web browser

1. Open `bootstrap/js/ucf.html` in the browser (a double click on the
   file).
2. At "1. The folder ucf", choose the folder `ucf`. The page reads all
   its files into memory; they stay on your computer.
3. At "2. Options and files", give the options and files as on a
   command line, for example `-32 host/files.fs`. Empty: uCForth with
   16-bit cells.
4. Click "Start". The prompt `ok>` comes in the terminal.

A line for uCForth goes into the field below the terminal; the return
key sends it. The button "Ctrl-C" (or Ctrl-C in the empty field) stops
the word that runs.

The files that uCForth writes show below the terminal as links: a click
saves them. A build gives its file, for example `build/esp32s3.bin`
from the command line of `targets/esp32s3/README.md` ("Build"). So a
browser alone builds uCForth for each target.

`FREEZE` gives two links: `ucf.hex`, the image, and `ucf-web.html`, a
page with the image in it. That page starts uCForth at once, without
the folder `ucf`: the target browser (`targets/browser/README.md`).
There, `targets/browser/build.fs` makes the same page from a command
line.

**From a web server.** When the page comes over http, it takes the files
from the server, and step 2 is not necessary; the field "1." says so.
In the folder `ucf`, for example with Python:

    python3 -m http.server 8000

then open `http://localhost:8000/bootstrap/js/ucf.html`. With
`?args=...` in the address, the page starts at once with these options
and files: `ucf.html?args=-check` or `ucf.html?args=host/files.fs%20tools/test.fs`
(`%20` is a space).

## In Node.js

    $ node bootstrap/js/ucf.js
    uCForth - JavaScript bootstrap VM - 16 bit - 165 words in FORTH - built in 80 ms
    ok> 3 4 + .
    7
    ok> BYE

The options are the same as for the other bootstrap VMs (`-32`, `-pure`,
`-core`, `-base`, `-image`, `-dump-core`, `-dump`, `-check`; see
`bootstrap/c/README.md`). Files load before the keyboard, and Ctrl-C
stops the word that runs.

## What is special in JavaScript

**One file, two hosts.** `makeVM(host)` makes a VM. The host is the
system around it: at the end of `ucf.js` the host for Node.js (files,
console, Ctrl-C), in `ucf.html` the host for the page (the files in
memory, the terminal, the links). The VM itself does not know where it
runs.

**The VM runs in slices.** JavaScript has one thread, and a web page
freezes while a script runs. So `run` executes the VM for at most a
million steps, then gives control back and continues a moment later.
A primitive that must wait ends the slice: `?RX` without a line sets
`pendingRx` (the VM does the rest of `?RX` when the line comes), and
`MS` sets the time `waitUntil`. The registers and the stacks keep their
values; the VM goes on where it stopped.

**Keys in a web page.** In a web page, a word that asks for a key does
not wait for a line (`host.poll`): `?RX` answers "no key", and the VM
pauses for 1 ms, so that the page can take a typed line. So `KEY?`
gives false, and an application with `TURNKEY` starts after 1 s as on a
board. While a word waits (`MS`, `KEY?`), its output so far shows. In
Node.js, as in the other bootstrap VMs, such a word waits for a line.

**Numbers.** A JavaScript number is a floating point number with 53
bits for integers. The bit operations take 32 bits with a sign. So
`cell` keeps the bits of a cell (`x & 0xFFFF`, or `x >>> 0` for 32
bits), `sgn` makes a signed number of it, and `*` uses `Math.imul`.
`UM*` and `UM/MOD` need 64 bits: they take `BigInt`.

**Files.** In a browser, `readFile` takes a file from the chosen folder
or from the web server. It must give the bytes at once, because
`OPEN-FILE` is a primitive: for the server, the page asks with a
synchronous `XMLHttpRequest`. A written file stays in memory too, so
uCForth can read it again (a build reads its `ucf.hex`).

## Tests

With Node.js, as for the other bootstrap VMs:

    node bootstrap/js/ucf.js -check
    node bootstrap/js/ucf.js -32 -check
    node bootstrap/js/ucf.js host/files.fs tools/test.fs </dev/null

The expected output is in `bootstrap/c/README.md`, section "Tests"; the
output of this program is the same, line for line. `-dump-core` and
`-dump` write the same files as `seed/`, the fixed point
(`tools/genesis.fs`) gives the same images, and each target that this
program builds is the same file, byte for byte, as from the C and the awk
bootstrap VM.

In a browser, over a web server (see above): `ucf.html?args=-check`,
`ucf.html?args=-32%20-check` and `ucf.html?args=host/files.fs%20tools/test.fs`
end with "the same" and `all: 2882 tests, 0 errors`, then
`[the end, exit status 0]`.

Tested (2026): Node.js 20 on macOS, also Ctrl-C at a terminal; Google
Chrome (headless) over a web server, also the build of
`build/esp32s3.bin` (the same size as from Node.js), and `FREEZE` with
the page that it makes; Firefox on macOS by hand: the choice of the
folder, `-32`, Ctrl-C. The page with an image (`targets/browser/README.md`,
section "Test") also in Firefox and Safari on macOS by hand, with the
choice of the folder; Genesis in the page not yet in Safari.

## Speed

Measured on an Apple M4 (2026), Node.js 20:

| Task | JavaScript | C | Python |
| --- | --- | --- | --- |
| compile `core.fs` and `base.fs` | 80 ms | 5 ms | 0.8 s |
| `-check`, 16 bit | 0.5 s | 0.04 s | 8.5 s |
| all tests, 16 bit | 0.4 s | 0.04 s | 6 s |
| `tools/genesis.fs`, 16 bit | 13 s | 1.5 s | 6 min |
| build `ucf-macos-a64` | 2.3 s | 0.3 s | 77 s |

## Limits

The same as for the C bootstrap VM (`bootstrap/c/README.md`, section
"Limits"). In a browser: no environment variables (`FREEZE` writes
`ucf.hex`), and `BYE` ends the VM, not the page.
