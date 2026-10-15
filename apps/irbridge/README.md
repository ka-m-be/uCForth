# irbridge: the IR bridge

The bridge lets MQTT control IR devices, such as an air conditioner:
it reports the IR frames that it hears to a broker, and it sends the
frames that it gets from there. It runs on an ESP32 board with the
module ESP32-WROOM-32:

- the WLAN: the board joins a saved WPA2 network and keeps the
  connection (package WLAN-BACK); for the setup it opens a temporary
  network of its own for one device (SETUP-AP);
- our TCP/IP: the sockets of NET over both (`lib/net/README.md`);
- IR in the background: frames come in and go out while the console
  and the network work, with a check of the own echo (IR-QUEUE);
- the bridge (IRBRIDGE, `bridge.fs`): the frames as messages of MQTT,
  in JSON (section "The topics"); the texts of the frames
  (BRIDGE-FRAMES, `frames.fs`): a heard frame as a short table, a frame
  to send in one of three forms (section "The frames in JSON").

The bridge keeps its broker in the flash (BRIDGE-SETTINGS,
`settings.fs`), it has a web page (BRIDGE-WEB, `web.fs`), and it can
start without a terminal (BRIDGE-BOOT, `boot.fs`). The button BOOT
opens a setup network: there a phone gives the bridge its WLAN and its
broker (BRIDGE-SETUP, `setup.fs`).

## Hardware

- An ESP32 board with the module ESP32-WROOM-32 and 4 MB flash; tested:
  ESP-WROOM-32 N4 of diymore.
- The IR transceiver of Adafruit (product 5990): 3V3, GND, IRout (the
  LED) at GPIO 4, IRin (the receiver) at GPIO 14. The same wires as for
  `apps/aircon`.
- A WPA2 network (WPA2-Personal, CCMP) on 2.4 GHz.

## Start

In the top folder, on the Mac (the port of the board here is
`/dev/cu.usbserial-110`; `lsof` on it first shows that no other program
holds it):

1. The base image into the flash, once (targets/esp32/README.md,
   "Build" and "Run"): build `build/esp32.bin`, then
   `build/ucf flash /dev/cu.usbserial-110 build/esp32.bin 0x1000` and
   `build/ucf reset /dev/cu.usbserial-110`.
2. One line loads the bridge and freezes it into the flash (about
   3 minutes):

       $ build/ucf term /dev/cu.usbserial-110 apps/irbridge/app.fs
       ...
       irbridge: ready; WBACK-START joins the saved WLAN, then BRIDGE-LOAD and 4 14 BRIDGE-START
       board ok>

   The board keeps all of it after a restart, with the radio and IR
   off. A second run loads nothing again.
3. Once: the profile of the network. Type the passphrase only at the
   board, never into a file, a script or a log:

       board ok> #require WLAN-SETUP lib/net/wlsetup.fs
       board ok> ALSO RADIO ALSO WIFI ALSO WLAN ALSO WLAN-SETUP ALSO WLSAVE
       board ok> RADIO-START RADIO-FACTORY WIFI-SCAN .  .WIFI

   `.WIFI` lists the networks, the strongest first (row 0). Then
   `i WLAN-PASSWORD` with the row i of the network: the board asks for
   the passphrase without an echo. `WIFI-WAIT .` gives 0 when the
   board has joined (about 50 s: the key derivation). `WLAN-SAVE`
   puts the name and the key (not the passphrase) into the flash.
   Then `#reset`: the board starts from its image again, without
   WLAN-SETUP and without keys in the RAM. Without a terminal: the
   setup network (section "The setup without a terminal") does steps 3
   and 4.
4. Once: the address of the broker in your network (Mosquitto, for
   example; port 1883, no password) into the flash. A start gives the
   board only FORTH in its search order: the first line names the
   packages.

       board ok> ALSO WLAN-BACK  ALSO MQTT  ALSO BRIDGE-SETTINGS  ALSO IRBRIDGE
       board ok> S" 192.168.1.20" 1883 MQTT-BROKER  BRIDGE-SAVE
       bridge: broker saved

5. After each start:

       board ok> ALSO WLAN-BACK  ALSO MQTT  ALSO BRIDGE-SETTINGS  ALSO IRBRIDGE
       board ok> WBACK-START
       WLAN back: link and DHCP ready
       board ok> BRIDGE-LOAD .
       -1
       board ok> 4 14 BRIDGE-START  BRIDGE-WEB-START
       board ok> .BRIDGE
       bridge A0B1C2D3E4F5 heard/sent/failed/lost/code: 0 0 0 0 0
       broker 192.168.1.20:1883

   `BRIDGE-LOAD` gives the broker from the flash to `MQTT-BROKER`
   (false: none saved). `4 14`: the LEDs at GPIO 4, the receiver at
   GPIO 14. `BRIDGE-WEB-START`: the web page (section "The web page").
   `.BRIDGE` shows the ID (the 12 hex digits of the MAC) and the broker.
   `BRIDGE-STOP` and `BRIDGE-WEB-STOP` end them.
6. The start without a terminal, once, right after a reset (`#reset`:
   then the radio is off and no key is in the RAM):

       board ok> ALSO BRIDGE-BOOT  BRIDGE-BOOT-ON  FREEZE

   From now on the board starts the bridge after each reset and power
   on, as a product: the saved WLAN, the saved broker, and
   `4 14 BRIDGE-START` as soon as the station is ready (its MAC gives
   the ID), and `BRIDGE-WEB-START`; the console stays open. In each case
   it starts the web page and watches the button BOOT (section "The
   setup without a terminal"). No network opens by itself. Without a
   profile the board waits for the button; without a broker the bridge
   runs without MQTT, and its web page sets the broker (section "The web
   page"). The LED of the board shows the state (section "The LED").

       autostart in 1 s (Ctrl-C: prompt)
       bridge: started

   Ctrl-C (once) in this second gives only the console, also right
   after `#reset` in `ucf term`; then `ALSO BRIDGE-BOOT BRIDGE-BOOT-OFF
   FREEZE` ends the start without a terminal. Ctrl-C twice switches the
   handlers off ("handlers off"): `+INT` before `FREEZE`. (`WLAN-BOOT-ON`
   of WLAN-BACK starts only the WLAN.)

## The setup without a terminal

After the start without a terminal (step 6) the board watches its
button BOOT. Hold it down for 3 s: the bridge and the station stop, the
board looks for the networks around (2 s), and it opens the setup
network `IRBRIDGE-` and the last 6 hex digits of its ID (as in
`.BRIDGE`), open, without a password. Only the button opens it.

1. Join the setup network with a phone or a computer. The first device
   is the only one; it gets the address 192.168.77.2.
2. Open `http://192.168.77.1/`: the page of the setup. It lists the
   networks with WPA2, the strongest first, and shows the broker saved
   before.
3. Choose the network, type the passphrase (8 to 63 characters) and
   the address and port of the broker, then "Save". The page says what
   comes now.
4. The setup network closes. The bridge looks for the networks again,
   takes the chosen one by its name, derives the key (about 50 s) and
   joins the network. When that works, it saves the profile (the
   name and the key, not the passphrase) and the broker into the flash,
   and the chip starts again: then the bridge runs in the network (with
   `BRIDGE-BOOT-ON`; without it the board waits at the console). When it
   does not work, the setup network comes back after about a minute:
   join it again; the page shows the code (section "When it does not
   work").

After 5 minutes without "Save" the chip starts again with the settings
it had. The setup network is open: during the setup the passphrase goes
over the air, readable for each device near the board. The board keeps
it in the RAM only up to the start of the derivation; the restart at
the end gives the RAM its frozen state again.

At the console `ALSO BRIDGE-SETUP BRIDGE-SETUP-START` does the same as
the button. `.BRIDGE-SETUP` shows the phase (0 none, 1 the setup
network is open, 2 the answer goes out, 3 the try), the networks of the
scan and the code of the last try:

    board ok> ALSO BRIDGE-SETUP  BRIDGE-SETUP-START  .BRIDGE-SETUP
    bridge setup phase/networks/code: 1 3 0

## The LED

The LED of the board (GPIO 2) shows the state of the bridge:

| LED | State |
| --- | --- |
| off | the WLAN and the broker are connected |
| short blips (50 ms each second) | one of them is not: no profile, the network or the broker is away, or the setup network is open |
| slow blinks (500 ms on, 500 ms off) | the setup derives the key and joins, after "Save", up to the restart |

## The topics

Under `IRBRIDGE/<ID>/`, with the ID of `.BRIDGE`; the client
identifier is `IRBRIDGE<ID>`. A program finds all bridges with
`IRBRIDGE/+/online`.

| Topic | Text | Retained |
| --- | --- | --- |
| `online` | `true` while connected; `false` as the will and after `BRIDGE-STOP` | yes |
| `status` | `{"version":"0.2","ip":"192.168.1.30","uptime":3600,"ir":{"heard":12,"sent":3,"failed":0},"ac":{"power":true,"mode":"heat","temp":23,"fan":0,"flap":0,"clock":"14:05"},"broker":"192.168.1.20:1883","mqtt":true}`, after each connection and every 60 s; `uptime` in seconds; `ac`: the state for the schema `rty3`, `clock` only when it is set | yes |
| `events` | each frame heard: `{"t":[8991,4507,528,576,1726,816],"s":"0123232323..."}`; with more than 16 durations `{"times":[9000,4500,560,...]}` | no |
| `send` | a frame to send, in one of the forms of the next section: `{"id":7,"schema":"rty3","power":true}` | no |
| `result` | `{"id":7,"code":0}` for each request: 0 sent (its echo came), else the code; without `id` when the request had none | no |

The times are in us, the first one is light, at most 400. A frame
heard while MQTT is not connected is lost (`.BRIDGE` counts it).

| Code | Meaning |
| --- | --- |
| -450 ... -454 | IR-QUEUE: a time out of range, a send waits already, no quiet, no echo, IR off (`lib/esp32/README.md`) |
| -471, -472, -473 | the request is no JSON, a field has another type, or a limit is passed (`lib/README.md`, "JSON") |
| -480 | a field of the frame is missing: `times`, or `t` and `s`; for `pd` `shape` or `hex` |
| -481 | no such schema |
| -482 | a field does not fit: a value out of range (`rty3`), a digit outside the table (`raw`), a shape without 6 or 7 times, a block of 0 bytes, blocks with fewer bytes than `hex` |

Try it from a computer with the clients of Mosquitto (the ID of
`.BRIDGE`; this frame means nothing to a device):

    $ mosquitto_sub -h 192.168.1.20 -v -t 'IRBRIDGE/#' &
    $ mosquitto_pub -h 192.168.1.20 -t IRBRIDGE/A0B1C2D3E4F5/send -m '{"id":1,"times":[3000,3000,600,600,600]}'
    IRBRIDGE/A0B1C2D3E4F5/send {"id":1,"times":[3000,3000,600,600,600]}
    IRBRIDGE/A0B1C2D3E4F5/result {"id":1,"code":0}

## The frames in JSON

**A frame heard** (`events`) comes as a table of at most 16 durations
in us (`t`) and one hex digit for each time (`s`): its place in the
table. Marks and spaces have durations of their own; a time belongs to
a duration that is at most 20 % (at least 80 us) away from its first
time, and each duration is the mean of its times (package IR-SYMBOLS,
`lib/irremote/irsym.fs`). A frame of the remote control RCH-RTY3-0, 371
times:

    {"t":[8991,4507,528,576,1726,816],"s":"0123232323232424232323232424242323..."}

That is 412 bytes, the times themselves would take 1 549. A frame with
more than 16 durations comes as `{"times":[...]}`. The bridge does not
decode a frame: that is the work of the program that reads `events`.

**A frame to send** (`send`) has one of three forms, by its field
`schema`:

| Schema | Fields | Example |
| --- | --- | --- |
| `raw`, or no `schema` | `t` and `s` as in `events`, or `times` | `{"id":1,"t":[3000,600],"s":"00111"}` |
| `pd` | the pulse distance code (package IRCODE): `shape` the header mark, the header space, the mark of a bit, the space of a 1, the space of a 0, the gap between blocks (a header mark 0: no header; a gap 0: no gap), and as a 7th the last light (else that of a bit); `blocks` the bytes of each block, at most 4 (without it one block); `hex` the bytes, at most 24, the lowest bit of each goes first | `{"id":2,"schema":"pd","shape":[9000,4500,560,1690,560,0],"hex":"04FB08F7"}` (NEC) |
| `rty3` | the air conditioner (package RTY3): `power` (true or false), `mode` (`heat`, `auto`, `cool`, `dry`, `fan`), `temp` (16 to 30), `fan` (0 to 7), `flap` (0 to 7), `clock` (`"14:05"`); each field is free | `{"id":3,"schema":"rty3","power":true,"mode":"heat","temp":23}` |

For `rty3` the bridge keeps the state of the air conditioner, as the
remote control RCH-RTY3-0 does (after the load: off, cool, 24 C, fan 0,
flap 0, no clock). The fields of a request change it, then the whole
frame goes out; if one field does not fit, nothing changes. The frame
names the key of the last field in the order mode, temp, fan, flap,
power, as the remote control names the key that was pressed. `status`
shows the state as `ac`. A frame of the real remote control does not
change it: the bridge does not decode.

## The web page

`http://` and the address of the board (`ip` in `status`) shows the
page: the state (as `status`, with the broker and `mqtt`), a form for
the broker, and the last messages heard and sent (`events`, `send`,
`result`, a line each). A few lines of JavaScript ask for the state and
the messages every 2 s. There is no password: the form changes only the
broker. It saves it (`BRIDGE-SAVE`) and connects to it at once.

| Path | Answer |
| --- | --- |
| `GET /` | the page (986 bytes of HTML) |
| `GET /status` | the state as JSON, as `status` |
| `GET /events` | the last messages as text, at most 4 KB |
| `POST /settings` | the form: `ip` (1 to 64 letters, digits, `.` and `-`) and `port` (1 to 65535); then back to the page; a wrong field: 400, nothing changes |
| `GET /` in the setup | the page of the setup (section "The setup without a terminal") |
| `GET /nets` | only in the setup: `{"nets":{"0":"name",...},"ip":"192.168.1.20","port":1883,"code":0}`, the networks with WPA2 by their number in the scan, the broker, the code of the last try; else 400 |
| `POST /setup` | only in the setup: `net` (the number), `pass` (8 to 63 characters from space to `~`), `ip`, `port`; then a page that says what comes; a wrong field: 400, nothing changes. Its body and the bytes the connection received are 0 after it |
| another | 404 |

From the Mac (the address of the board):

    $ curl http://192.168.1.30/status
    {"version":"0.2","ip":"192.168.1.30","uptime":21,...,"broker":"192.168.1.20:1883","mqtt":true}
    $ curl -d 'ip=192.168.1.20&port=1883' http://192.168.1.30/settings

## The words

| Word | Effect |
| --- | --- |
| `WBACK-START ( -- )`, `WBACK-STOP ( -- )` | the station with the saved profile; it joins again by itself (WLAN-BACK) |
| `WBACK-READY? ( -- f )`, `.WBACK ( -- )` | joined, with an address; the state, the attempts, the joins, the code of the last failure |
| `a u port MQTT-BROKER ( -- )`, `.MQTT ( -- )` | the broker: its address as text and its port; the state and the counters of MQTT |
| `out in BRIDGE-START ( -- )`, `BRIDGE-STOP ( -- )` | the bridge: IR at the pins out and in, MQTT, its step in the chain of IDLE |
| `.BRIDGE ( -- )` | the ID; the frames heard, the requests sent and failed, the frames lost, the code of the last error; the broker |
| `BRIDGE-WEB-START ( -- )`, `BRIDGE-WEB-STOP ( -- )` | the web page at port 80 (package BRIDGE-WEB, `web.fs`; HTTP, `lib/net/http.fs`) |
| `BRIDGE-RECONNECT ( -- )`, `a u BRIDGE-STATUS ( -- ta tu ior )`, `BRIDGE-LOG ( -- a u )` | a new connection to the broker of `MQTT-BROKER`; the state as JSON in the buffer a u; the last messages, a line each |
| `BRIDGE-BOOT-ON ( -- )`, `BRIDGE-BOOT-OFF ( -- )`, `BRIDGE-AUTO ( -- )` | the start without a terminal after each reset, from the next `FREEZE` on; none any more; the start itself: the LED, the button, the web page, the saved WLAN, the saved broker, `4 14 BRIDGE-START` when the station is ready, a restart when WLAN-BACK ends by itself (package BRIDGE-BOOT, `boot.fs`) |
| `BRIDGE-SETUP-START ( -- )`, `.BRIDGE-SETUP ( -- )` | the setup now, as the button does it (package BRIDGE-SETUP, `setup.fs`); the phase, the networks of the scan, the code of the last try |
| `i a n BRIDGE-SETUP-JOIN ( -- )`, `BRIDGE-SETUP? ( -- f )`, `BRIDGE-SETUP-NETS ( -- u )`, `BRIDGE-SETUP-CODE ( -- ior )`, `BRIDGE-SETUP-PHASE ( -- n )` | for the page of the setup: the try with network i of the scan and the passphrase a n, 2 s later; the setup network is open; the networks of the scan; the code of the last try; the phase (0 none, 1 open, 2 the answer, 3 the try) |
| `BRIDGE-RESTART ( -- )` | the end of the setup: `BYE`, the chip starts again |
| `mac BRIDGE-ID ( -- a u )` | the 12 hex digits of the 6 bytes at mac: the ID in the topics |
| `BRIDGE-SAVE ( -- )`, `BRIDGE-LOAD ( -- f )`, `BRIDGE-FORGET ( -- )` | the broker of `MQTT-BROKER` into the flash; from the flash to `MQTT-BROKER` (false: none saved); none in the flash. In two sectors, `$3F_C000` and `$3F_D000`: a power loss while it writes keeps the broker of before (FLASH-PAIR, `lib/esp32/flpair.fs`) |
| `a n ch SETUP-START ( -- )`, `SETUP-STOP ( -- )`, `.SETUP ( -- )` | the setup network (SETUP-AP); the station must be off |
| `a u out in IR-Q-START ( -- )`, `IR-Q-STOP ( -- )` | IR in the background (IR-QUEUE) without the bridge; a u: a ring of the program (`lib/esp32/README.md`) |
| `a n IR-Q-SEND ( -- ior )`, `IR-Q-RESULT ( -- ior \| 1 )` | send a frame of n times in us, then its result |
| `IR-Q-FRAME? ( -- a n end-us true \| false )`, `.IR-Q ( -- )` | the next received frame; the counters |

The words of NET (`TCP-OPEN`, `TCP-LISTEN`, `SOCK-READ`, `UDP-SEND`
...): `lib/net/README.md`, "The words of an application". All of the
packages: `lib/esp32/README.md`, "The WLAN of the WROOM" and "IR for the
bridge on the WROOM".

## Memory

Measured on the WROOM with all of the bridge in the flash (15.10.):
free are 3 164 bytes of dictionary, 41 244 bytes of RAM data, 132 320
bytes of RAM data behind the frozen part (XDATA), and 72 892 bytes for
`CODE` words (targets/esp32/README.md, "Memory"). The bridge runs on
core 0; it does not load MULTICORE (the second core, lib/esp32/README.md),
which the bridge does not need. The XDATA reaches up
to the end of the frozen part: a bigger dictionary gives it more. The
loader keeps no headers for the `PRIVATE` words (`TEMP-HEADS ON!`,
lib/package.fs): that gives the dictionary about 17 KB, and the XDATA
has as much less. So `SEE`, `.IDLE` and error messages show `?` and
the address for such a word.

The frames take 8 180 bytes of the dictionary: AC-BASE 1 524, IRCODE
984, RTY3 2 124, IR-SYMBOLS 1 304, `frames.fs` 2 244; the bridge itself
2 320 (measured on the host with `TEMP-HEADS ON!`, as the loader does
it). The broker in the flash takes 2 532 bytes (FLASH-PAIR 2 108, of
which WLSAVE took a part before, `settings.fs` 424), the start without a
terminal 376 (`boot.fs`), the web page 5 860 (HTTP 3 252, `web.fs` and
the log in the bridge), the setup without a terminal 3 960 (`setup.fs`
1 428, its page and routes in `web.fs` 1 732, the button in `boot.fs`
208, `HTTP-DECODE` 388, BOARD 168). The buffers of the bridge (33 KB:
the ring of 4 frames, the room of MQTT, the texts, the times of a
request, the log of 4 KB, the connections of the page 4 KB, a page of
the setup 2 KB) are in the XDATA.

The chain of IDLE has room for 12 words (lib/idle.fs). While the bridge
runs, 8 places are taken: the start (the LED and the button), HTTP,
WLAN-BACK, the messages of the handlers (`.STOPS`), IR-QUEUE, the
bridge, MQTT and the network; in the setup 6.

## Files

| File | Contents |
| --- | --- |
| `app.fs` | the loader: all packages of the bridge, frozen into the flash |
| `bridge.fs` | package IRBRIDGE: the logic of the bridge: the topics, MQTT, IR-QUEUE |
| `frames.fs` | package BRIDGE-FRAMES: the frames in JSON: `events`, the schemas of `send`, `ac` |
| `settings.fs` | package BRIDGE-SETTINGS: the broker in the flash |
| `setup.fs` | package BRIDGE-SETUP: the setup without a terminal: the scan, the setup network, the try, the profile and the broker into the flash |
| `web.fs` | package BRIDGE-WEB: the web pages: that of the bridge, that of the setup |
| `boot.fs` | package BRIDGE-BOOT: the start without a terminal, the button |
| `test/bridge.fs`, `test/settings.fs`, `test/web.fs`, `test/setup.fs`, `test/boot.fs` | the tests on the host, with mocks (`test/mocks.fs`, `test/helpers.fs`, `lib/esp32/test/flmock.fs`) and the real packages of `lib/irremote/`; in `tools/test.fs` |

## Tests on the board

On the WROOM, against the Mosquitto on the Mac (14.10. and 15.10.):

- **9 hours**, the bridge over GPIO 25 (a loop inside the chip, no
  light): a request every 10 s, 3 237 in all (2 589 `raw`, 324 `pd`, 324
  `rty3`, frames without a meaning): 3 234 results, each with code 0. A
  `status` came every 60 s, and the free memory stayed the same.
- **A WLAN outage**: the board moved its radio to another channel, so
  the beacons of the access point stayed away. 12 s later the bridge was
  in the network again and connected to the broker. One request that
  came meanwhile got no result.
- **A power loss** (a reset over RTS): the board started, took the
  broker from the flash and joined. 12 s later the bridge answered
  again; two requests that came meanwhile got no result.
- A frame from outside (sent past IR-QUEUE) came as `events`.
- **The setup** at the console (`BRIDGE-SETUP-START`): the setup network
  sends its beacons; a try that fails gives its code and the setup
  network again; after 5 min the chip starts again. With Kai and a
  phone (15.10.): the button, the page, the passphrase, the profile
  saved, the bridge online with it after about a minute.
- **The start as a product** (15.10.): with profile and broker the LED
  stays off (0 of 100 samples); without a broker the bridge runs, the
  LED blips (5 of 100), the web page sets the broker, MQTT connects, the
  LED goes off.

A request that comes while the bridge is not connected gets no result:
the program that sends it waits for `result` and sends it again.

The loop for such a test, in step 5 instead of `4 14 BRIDGE-START`
(`IR-Q-IDLE-HIGH OFF!`: the loop is low while it is quiet, a receiver
high):

    board ok> ALSO IR-QUEUE  ALSO IR-QUEUE.I  ALSO IR  ALSO IR.I  ALSO RMT  ALSO IR-CAPTURE
    board ok> IR-Q-IDLE-HIGH OFF!  25 25 BRIDGE-START
    board ok> TX 25 RMT-OUT  25 RX RMT-IN  0 0 IR-CARRIER  20 MS
    board ok> IR-CAP-OFF  0 WRITE-N !  0 READ-N !  HELD OFF!  DECODING OFF!  IR-CAP-ON

## When it does not work

| What you see | What it means |
| --- | --- |
| `WBACK-START` throws -431 | no profile in the flash: step 3 |
| `WLAN retry code: -2`, again and again | the saved network is not in the scan: its name, 2.4 GHz, the distance |
| `WLAN retry code: -428` once | the access point sent a deauthentication, for example because it still knew the board from before a power loss; the next attempt works |
| `WLAN retry code: -302` | no address from DHCP within 15 s |
| `WLAN retry code: -433` | the radio got no end of a send: the next attempt starts it again |
| each `IR-Q-SEND` ends with -452, and `.IR-Q` shows many short frames | the input pin floats: check the wire IRin to GPIO 14 and the supply of the receiver |
| each `IR-Q-SEND` ends with -453 | the receiver does not hear the own frame (the echo): check the wire IRout to GPIO 4 and the supply of the LEDs (`lib/esp32/README.md`, "The echo") |
| `.MQTT` shows state 1 and code -301 | no broker at the address and port of `MQTT-BROKER`; MQTT tries again after 1, 2, 4 ... 32 s |
| each `result` has code -453 | as for `IR-Q-SEND`: no echo |
| `.BRIDGE` counts frames lost | they came while MQTT was not connected |
| the page of the setup: "The last try failed: code -422" or "-428" | the access point did not finish the key exchange, or it sent a deauthentication: most often a wrong passphrase |
| the page of the setup: code -2 | a check in the try failed, for example the chosen network was not in the new scan; the console shows the code and its text (`bridge setup: the try failed, code -2 ...`) |
| the setup network does not come | the button was not down for 3 s, or `BRIDGE-AUTO` did not run (no start without a terminal: step 6) |
| the LED blips, and the network `IRBRIDGE-` is there | the button BOOT was down for 3 s: the setup network is open; without "Save" the board starts again after 5 min |
| the LED blips, and there is no network `IRBRIDGE-` | the WLAN or the broker is away; `.WBACK` and `.MQTT` at the console show which; without a broker its web page sets it |
| the page of the bridge answers slowly while a browser has it open | the server takes one connection at a time, and a browser keeps empty connections open, each up to 5 s |

Before `FREEZE`: `BRIDGE-STOP`, `WBACK-STOP` (or `SETUP-STOP`) and
`IR-Q-STOP`. Safer still: a reset, and Ctrl-C in the first second; then
the radio is off and no key is in the RAM.
