# apps: applications

An application is a file that loads its packages and adds the words
for one task. The applications here run on a board. The tool `ucf`
loads one in one line, then it is the console for its words (in the
folder `ucf`, on the Mac):

    build/ucf term PORT apps/aircon/app.fs

`ucf term` first sends the file to the board (as `#include` does:
host/README.md). Its `#require` lines load the packages that the board
does not have yet. Then the prompt of the board comes, and its words
are there.

| Folder | Application | Hardware |
| --- | --- | --- |
| `aircon/` | a Hisense air conditioner over infrared: on, off, mode, temperature, fan, timers; hears the remote control | an ESP32-S3 board or an ESP32 board, the IR transceiver of Adafruit (product 5990) |
| `irbridge/` | the IR bridge: IR frames over MQTT (heard ones as events, requests in three forms), a web page, the setup of the WLAN and the broker over a setup network (the button BOOT), the start without a terminal | an ESP32 board with the module ESP32-WROOM-32, the IR transceiver of Adafruit (product 5990) |

Each app should name the main file `app.fs`. The main file should:

- only `#require` the necessary packages
- leave the `ORDER` with only the app's words

Each application has a README: the hardware and its wires, the start,
the words, and what to do when it does not work.

An application stays in the board with `FREEZE`: load it, then
`FREEZE` (targets/esp32s3/README.md, "An application in the flash").
After a restart, the words are there again; the README of the
application says how it starts again.

## One application on different chips

`apps/aircon` runs on the ESP32-S3 and on the ESP32 with the same
files. The chips differ (other addresses, another RMT), but the
application does not see it:

- The application uses only the common words of the packages: `IR-PINS`,
  `IR-SEND`, `OUTPUT`, `EVERY` ... Each package has the same words on
  each chip.
- A package whose code is the same on each chip takes the addresses
  and numbers from the layer of the chip (`ALSO CHIP`): GPIO, INTR,
  TIMERS, FLASH, IR.
- A package whose code differs has a file for each chip, and a front
  file that chooses it on the board: `lib/esp32/rmt.fs` holds `#when
  ESP32S3 esp32s3/rmt.fs` and `#when ESP32 esp32/rmt.fs`. The
  application loads the front file.
- Packages that do not touch the chip (IRCODE, RTY3 and the others in
  `lib/irremote/`) run on each system, also on the host, where their
  tests run.

So a new chip needs its layer and its own files, not a new
application (lib/esp32/README.md, "The layout").
