# browser: uCForth in one HTML file

The result is `build/ucf-web.html`: one file with the page of the
JavaScript bootstrap VM, the VM itself and an image. A double click
opens it in the web browser, and uCForth starts at once from its image:
no folder `ucf`, no server, no installation. The file also goes as the
attachment of a mail, with an application in it.

There is no machine code here: the virtual machine is the JavaScript
bootstrap VM (`bootstrap/js/ucf.js`).

## Build

In the folder `ucf`, with a bootstrap VM with 16-bit or 32-bit cells:

    build/ucf-c -32 host/files.fs targets/browser/build.fs </dev/null

The output ends with:

      saved: ucf.hex
      written: build/ucf-web.html (128140 bytes, 32-bit cells)

Without `-32`, the page has 16-bit cells (98537 bytes): 64 KB of memory
for uCForth instead of 256 KB. Each bootstrap VM builds the same file,
byte for byte (tested: bash, awk, C, Python, JavaScript, JScript); awk
takes 2 s, bash about a minute. In a web browser, the page of the
JavaScript bootstrap VM builds it too (`bootstrap/js/README.md`); the
file then shows as a link.

`build.fs` does two things:

1. `FREEZE`: the running system is the image: `base.fs`, the package
   FILES and each file before `build.fs` on the command line.
2. It writes the page: the lines of `bootstrap/js/ucf.html`, with the
   lines of `ucf.hex` in the element for the image, and with the lines
   of `bootstrap/js/ucf.js` in place of the line that loads it.

## Run

Open `build/ucf-web.html` in the web browser (a double click). The
terminal shows:

    uCForth - JavaScript bootstrap VM - 32 bit - 170 words in FORTH - image ucf-web.html
    ok>

A line for uCForth goes into the field below the terminal; the return
key sends it:

    ok> 3 4 + .
    7
    ok>

The button "Ctrl-C" (or Ctrl-C in the empty field) stops the word that
runs. `BYE` ends the virtual machine; "Start again" starts the image of
the page again.

**Files.** Without the folder `ucf`, uCForth has no files to read. When
you choose the folder at "The folder ucf", `INCLUDE` reads from it (the
files stay on your computer):

    ok> ALSO FILES  INCLUDE examples/hello.fs
    Hello from uCForth

When the page comes from a web server (for example from GitHub Pages),
`INCLUDE` reads the files from the server, in the folder above the
page. Then the field of the folder says so; a folder is not necessary.

The field "Files to load after the image" takes files as a command line
does; "Start again" loads them after the start.

**FREEZE** gives two links below the terminal: `ucf.hex`, the image, and
`ucf-web.html`, this page with the new image. Save the page: it starts
with your words.

    ok> : HI  ." Hello from my page" CR ;
    ok> FREEZE
      saved: ucf.hex

## An application in the page

Files between `host/files.fs` and `build.fs` go into the image. With
`TURNKEY` (`doc/vm.md`, section 10), the page starts the application
by itself, for example `examples/tick.fs`:

    build/ucf-c -32 host/files.fs examples/tick.fs targets/browser/build.fs </dev/null

The page then shows:

    uCForth - JavaScript bootstrap VM - 32 bit - 173 words in FORTH - image ucf-web.html
    autostart in 1 s (Ctrl-C: prompt)
    start
    tick 1
    tick 2

Ctrl-C in the first second gives the prompt instead. Later, it stops the
application:

      error: user interrupt
    ok>

In the page, `KEY?` does not wait for a line: without a typed line, it
gives false. The output of a word shows while the word waits (`MS`,
`KEY?`), not only at the end of the line.

## Test

Over a web server, as for the page of the bootstrap VM
(`bootstrap/js/README.md`). In the folder `ucf`:

    python3 -m http.server 8000

Then open `http://localhost:8000/build/ucf-web.html?args=tools/test.fs`.
The page takes the files from the server and loads `tools/test.fs`
after the image. The output ends with:

    all: 12210 tests, 0 errors

    [the end, exit status 0]

With 16-bit cells: `all: 2882 tests, 0 errors`. The lines are the same
as from the C bootstrap VM (`bootstrap/c/README.md`, section "Tests");
only the first line differs.

Google Chrome runs the test without a window (macOS):

    "/Applications/Google Chrome.app/Contents/MacOS/Google Chrome" --headless=new \
      --virtual-time-budget=60000 --dump-dom "http://localhost:8000/build/ucf-web.html?args=tools/test.fs"

The output is the page; the terminal is the element `<div id="term">`.

**Tested (2026):** Google Chrome on macOS, without a window. Over a web
server: all tests with 16 and 32 bits, `INCLUDE`. As a file, without
the folder: the prompt, typed lines, Ctrl-C, `KEY?`, `FREEZE` and the
page that it makes, `examples/tick.fs` with `TURNKEY`. The page of the
bootstrap VM: `FREEZE` and its page. Firefox and Safari on macOS, by
hand: the prompt, Ctrl-C, `INCLUDE` from the chosen folder, `FREEZE`
and the saved page, `examples/tick.fs` with `TURNKEY`.

## Speed

An empty loop shows the speed of the virtual machine: each pass is one
step, the primitive `(LOOP)`. In the page:

    ok> : L 0 DO LOOP ;
    ok> : T  MILLIS SWAP L  MILLIS SWAP - . ;
    ok> 10000000 T 10000000 T 10000000 T
    622 384 233

The numbers are milliseconds. The first runs are slower: the browser
makes the loop of the virtual machine faster while it runs (its JIT
compiler), and the page starts from its image, so nothing ran before.
Measure a long loop a few times and take the last value. `MILLIS` counts
whole milliseconds, so a loop of a few milliseconds says little: after
the start, 65 000 passes took 21, 19 and 6 ms in Chrome, later 1 to 4 ms.

Measured on an Apple M4 Pro (2026), 32-bit cells, the last of three
runs of `10000000 T`:

| Browser | 10 000 000 passes | Steps in one second |
| --- | --- | --- |
| Google Chrome | 233 ms | 43 million |
| Firefox | 362 ms | 28 million |
| Node.js 20 (`bootstrap/js/ucf.js`, for comparison) | 259 ms | 39 million |

Chrome without a window and with `--virtual-time-budget` (as in the
test above) stops the clock while the virtual machine runs: there,
`MILLIS` measures nothing.

## How it works

The page is the page of the JavaScript bootstrap VM (`bootstrap/js/`)
with two things more:

| Part of the page | Contents |
| --- | --- |
| the element `<script type="text/plain" id="image">` | the image: the lines of `ucf.hex`. The browser does not run it; the page reads it as text. In `bootstrap/js/ucf.html`, it is empty |
| the next `<script>` element | the virtual machine: the lines of `bootstrap/js/ucf.js` |
| the last `<script>` element | the page: the terminal, the files, the links; the same as in `bootstrap/js/ucf.html` |

When the element for the image is not empty, the page starts the
virtual machine with `-image` and the name of the page: the image comes
from the page, not from a file. The files of the field (or of `?args=`
in the address) load after it.

`FREEZE` in the page makes the new page from the elements of the page
as it came, with the new image. The browser writes the elements in its
own way: the new page works the same, but its bytes are not the same as
from `build.fs` (for example `disabled=""` instead of `disabled`). The
page of the bootstrap VM makes such a page too; it takes `ucf.js` from
the folder or from the web server.

## Memory

The memory of the virtual machine is an array of JavaScript in the
page: 256 KB with 32-bit cells, 64 KB with 16-bit cells (doc/vm.md,
section 3). After the start, with the image of `build.fs` (32 bits,
base and FILES):

| Area | Size | Free |
| --- | --- | --- |
| dictionary: words, `ALLOT`, `,` | 189 KB | 169 KB |
| RAM data: `VARIABLE`, `VALUE`, `BUFFER:`, vocabularies | 63 KB | 61 KB |
| data stack, return stack | 256 cells each | |
| the input line | 256 characters | |

`UNUSED U.` shows the free bytes of the dictionary,
`ALSO INTERNALS  DATA-END VP @ - U.` those of the RAM data. A full
dictionary or RAM data gives the error -8. doc/vm.md, section 3, has
the other limits of the VM. An address outside the memory of the VM
wraps around, as in the C bootstrap VM: with 32 bits, `$40004` is the
address `$4`.

## Limits

The same as for the JavaScript bootstrap VM in a browser
(`bootstrap/js/README.md`, section "Limits"), and:

- The page reads only the files of the folder that you choose, or of
  the web server. The files that uCForth writes are links below the
  terminal.
- On a web server, the folder `ucf` is the folder above the page, as
  for `build/ucf-web.html`.
- The bootstrap VM for DOS does not build the page: the names
  `bootstrap` and `.html` are too long for DOS.
