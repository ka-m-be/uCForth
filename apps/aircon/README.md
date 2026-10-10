# aircon: a Hisense air conditioner over infrared

An ESP32-S3 board or an ESP32 board takes the place of the remote control
RCH-RTY3-0 of a Hisense air conditioner (Hisense Energy Pro X, indoor unit
QH35XV4AG). Its IR
LEDs send the frames of the remote control, and its IR receiver hears
the remote control. You work at the console on your host: `AIRCON-ON`,
`HEAT MODE`, `21 TEMP`.

**Status:** No library knows the code of this remote control; its frame
comes from measurements (package RTY3, `lib/irremote/rty3.fs`). On, off,
the mode, the temperature and the clock are known, and the air conditioner
follows the commands when the transceiver points at the receiver of the
indoor unit. At 3V3 its LEDs give little light, so the direction decides
(see "When it does not work"). The codes of the fan and the flap are known,
not yet their meaning; sleep, super, the display and the timers come after more
measurements with `LISTEN`.

The same files run on both boards (apps/README.md, "One application on
different chips"): tested at the air conditioner with the ESP32-S3 and
with the ESP32 (diymore ESP-WROOM-32 N4). A quick check of the
transceiver without the air conditioner: with `TRUE IR-ECHO !` (package
IR) the board reads its own frame back (371 times, the right
checksum).

## What you need

- An ESP32-S3 board or an ESP32 board with uCForth in its flash
  (targets/esp32s3/README.md or targets/esp32/README.md, "Run"; tested:
  diymore ESP32-S3 N16R8, diymore ESP-WROOM-32 N4).
- The IR transceiver of Adafruit, product 5990: two IR LEDs and a
  receiver for 38 kHz on one small board. A STEMMA cable (JST PH, 4
  wires) or 4 wires to its pins.
- If you have one: a capacitor of 100 to 470 uF.

## The wires

| Transceiver | Wire of the cable | Board |
| --- | --- | --- |
| VIN | red | 3V3 |
| GND | black | GND |
| IRout | green | GPIO 4 |
| IRin | white | GPIO 14 |

**VIN goes to 3V3, not to 5 V.** The receiver gives VIN - 0.4 V at
IRin, and a pin of the ESP32 chips takes 3.6 V at most. The two LEDs take
about 100 mA each while they send: put the capacitor between VIN and
GND, near the transceiver (the plus pin to VIN).

The pins are the same on both boards: GPIO 4 and 14 belong to no part
of the board (targets/esp32s3/README.md and targets/esp32/README.md,
"The board"). Other pins: change `IR-OUT` and `IR-IN` at the top of
`aircon.fs`.

## Start

In the folder `ucf` on the host. The cable goes to the socket "USB to
serial port" of the ESP32-S3 board, or to the USB socket of the ESP32
board; `ls /dev/cu.*` shows its port (on macOS):

    build/ucf term /dev/cu.usbmodem5C4C0505931 apps/aircon/app.fs

(the ESP32 board: `build/ucf term /dev/cu.usbserial-110
apps/aircon/app.fs`). `term` loads the packages to the board (4 s the
first time, 6 s on the ESP32; later it loads only what the board does
not have yet), then the prompt of the board comes:

    term on /dev/cu.usbmodem5C4C0505931: #help shows the commands
    aircon: ready; AIRCON-HELP shows the commands
    board ok>

Press a key of the remote control once, near the board: the board
takes its frame as the state (see "The state"). Then point the
transceiver to the air conditioner:

    board ok> AIRCON-ON
    board ok> HEAT MODE
    board ok> 21 TEMP
    board ok> .AC

`.AC` shows the state and the frame. Without a frame of the remote
control, the state starts as off, cool, 24 C:

    board ok> .AC
    off, cool, 24 C, fan 0, flap 0, clock not set
    60 38 13 04 FF 00 50 00 00 00 00 00 00 00 00 00 00 00 00 25 00 00 8E

`#bye` ends the console; the board keeps its words until a restart.

## The words

Each word that changes the state sends it at once: the whole state in
one frame, as the remote control does. So the air conditioner does what
the last frame says.

| Word | Effect |
| --- | --- |
| `AIRCON-ON`, `AIRCON-OFF` | on, off |
| `HEAT MODE`, `COOL MODE`, `DRY MODE`, `FAN MODE` | the mode: heat, cool, dry, only the fan (`AUTO MODE` is not measured) |
| `21 TEMP` | the temperature, 16 to 30 C; DRY and FAN have none (the frame carries 23 C, the board keeps your value for later) |
| `2 SPEED` | the fan: the code 0 to 7 as the remote control sends it (the meaning is measured next) |
| `3 FLAP` | the flap: the code 0 to 7 as the remote control sends it (each mode has its own) |
| `21 45 CLOCK!` | the clock in the frame is 21:45 now; it runs on in the board |
| `.AC` | the state, and the frame (23 bytes) |
| `SEND` | send the state again |
| `LISTEN` | show the frames of the remote control; the return key ends |
| `AIRCON-HELP` | these words |
| `AIRCON-START` | set up the pins again: after Ctrl-C twice (it switches the handlers off), or after a restart with the application in the flash |

## The state and the remote control

The board hears each frame of the remote control and takes it as its
state, with its clock. So `.AC` always shows the last frame, from the
board or from the remote control, and the bytes that the board does not
know yet stay as the remote control sent them.

## LISTEN: the frames of the remote control

`LISTEN` shows each frame of the remote control: its bits and bytes,
the check (`$60`, the length, the XOR), and the bytes that changed
since the frame before. So you find the byte of each key: press one
key after the other and note what the remote control shows.

The frame of the remote control (package RTY3, measured):

| Byte | Contents |
| --- | --- |
| 0, 1, 2 | `60`, the type (`38`: the state), the number of data bytes (19) |
| 3 | bit 0: on; bits 1-3: the mode (0 heat, 2 cool, 3 dry, 4 fan) |
| 6 | the temperature: (T - 16) x 10; dry and fan: 23 C |
| 7, 8 | dry: the grade; bit 5 of byte 7: iFeel on, byte 8: its temperature in 0.1 C |
| 10, 11 | the clock: hour, minute (byte 11 bits 0-5); byte 11 bit 6: super |
| 16 | the flap (key `07`) |
| 17 | the fan (key `08`) |
| 19 | the key of the frame |
| 22 | the XOR of the bytes 3 to 21 |

Not known yet: byte 4 (`FF` or `DA`), the bytes 13 to 15 (keys `2B`,
`2C`, `05`), bit 7 of byte 11 (key `24`), and a short frame of type
`78` (`60 78 09 ...`).

## Keep it in the board

    board ok> FREEZE

After a restart, the words are there, but the pins are not set up yet.
The same line as for the start does it; it loads nothing then:

    build/ucf term /dev/cu.usbmodem5C4C0505931 apps/aircon/app.fs

At the prompt of the board, `ALSO AIRCON  AIRCON-START` does the same.
`ucf flash PORT build/esp32s3.bin 0` (the ESP32: `ucf flash PORT
build/esp32.bin 0x1000`) puts the plain uCForth back into the flash.

## When it does not work

| What you see | What to do |
| --- | --- |
| `LISTEN` shows nothing | First the wires: at rest `ALSO GPIO  14 PIN@ .` gives -1 (the receiver holds IRin high). 0 means: the receiver has no power or no ground (in one test, GND and IRout had changed places). IRin and IRout changed places? IRin (white) goes to GPIO 14. Is VIN at 3V3? Point the remote control at the receiver (the black part in the middle), 5 to 50 cm. One of the two yellow LEDs of the transceiver blinks while it gets light. |
| The air conditioner does not react, or only sometimes | Look at the IR LEDs through the camera of a phone: they blink violet while the board sends. The other yellow LED of the transceiver blinks with them. Then the direction: the LEDs give little light at 3V3, and the worse the angle to the receiver of the indoor unit, the fewer commands arrive. Find the place of that receiver with the remote control: hold it where the transceiver is, in the same direction. If the unit follows the remote control from there, point the transceiver the same way. More light: `ALSO IR  38000 50 IR-CARRIER` (50 % instead of 33 % light in each period). After Ctrl-C twice: `AIRCON-START`. |
| It reacts only near | 3V3 gives less light than 5 V (the transceiver takes 5 V, but then IRin needs a voltage divider). |
| `error: RTY3: ...` | a value out of range: `AIRCON-HELP` |

## The packages

| Package | File | What it does |
| --- | --- | --- |
| RMT | `lib/esp32/rmt.fs` | the remote control unit of the chip, channel by channel; this file loads the one for the chip of the board (`esp32s3/rmt.fs` or `esp32/rmt.fs`) |
| IR | `lib/esp32/ir.fs` | IR frames as times in us: the carrier of 38 kHz, send, receive |
| IRCODE | `lib/irremote/ircode.fs` | the pulse distance code: bytes to times and back |
| RTY3 | `lib/irremote/rty3.fs` | the frame of the remote control RCH-RTY3-0: the state, the checksum, the times |
| AIRCON | `apps/aircon/aircon.fs` | the words above; `app.fs` loads all and starts them |

IRCODE and RTY3 run on each system; their tests run on the host (`tools/test.fs`),
with the frames and times that were measured from a real remote. The package KELON168
(`lib/irremote/kelon168.fs`) is the frame of other Hisense remote controls (DG11R2-01,
RCH-R0Y3).
