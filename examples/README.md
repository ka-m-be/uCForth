# examples: small applications

| File | Contents |
| --- | --- |
| `hello.fs` | a first example: define a word, then use it |
| `blink.fs` | the LED of the board blinks: the RGB LED of the diymore ESP32-S3 N16R8, the LED at GPIO 2 of the diymore ESP-WROOM-32 N4 (`#include examples/blink.fs` in `ucf term`, then `BLINK`; the package of the board: `BOARD`, lib/esp32/README.md) |
| `tick.fs` | an application for `RUN` and `TURNKEY`: SETUP once, then a tick each second |
| `echo.fs` | a server for TCP on macOS or Linux: `7007 ECHO-SERVER`, then `nc 127.0.0.1 7007` in a second terminal; each line comes back (lib/net/README.md) |
| `webget.fs` | a page from a web server on macOS or Linux: `S" example.org" WEBGET` shows the answer; DNS, TCP and the output to a connection |

Coming from archive/m0-m3/examples: blink, button, analog and others.
File names have at most 8 characters and `.fs`.
