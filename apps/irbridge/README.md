# irbridge: the IR bridge (in work)

The bridge shall let MQTT and a web page control IR devices, such as an
air conditioner, and report the IR frames that it hears. So far it has
its base, on an ESP32 board with the module ESP32-WROOM-32:

- the WLAN: the board joins a saved WPA2 network and keeps the
  connection (package WLAN-BACK); for the setup it opens a temporary
  network of its own for one device (SETUP-AP);
- our TCP/IP: the sockets of NET over both (`lib/net/README.md`);
- IR in the background: frames come in and go out while the console
  and the network work, with a check of the own echo (IR-QUEUE).

MQTT, the web page and the settings in the flash come next.

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
   2 minutes):

       $ build/ucf term /dev/cu.usbserial-110 apps/irbridge/app.fs
       ...
       irbridge: ready; WBACK-START joins the saved WLAN, 4 14 IR-Q-START starts IR
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
   WLAN-SETUP and without keys in the RAM.
4. After each start:

       board ok> ALSO WLAN-BACK  WBACK-START
       WLAN back: link and DHCP ready
       board ok> ALSO IR-QUEUE  4 14 IR-Q-START

   `WLAN-BOOT-ON FREEZE` (package WLAN-BACK) makes the board join by
   itself after each start; `WLAN-BOOT-OFF FREEZE` ends that.

## The words

| Word | Effect |
| --- | --- |
| `WBACK-START ( -- )`, `WBACK-STOP ( -- )` | the station with the saved profile; it joins again by itself (WLAN-BACK) |
| `WBACK-READY? ( -- f )`, `.WBACK ( -- )` | joined, with an address; the state, the attempts, the joins, the code of the last failure |
| `a n ch SETUP-START ( -- )`, `SETUP-STOP ( -- )`, `.SETUP ( -- )` | the setup network (SETUP-AP); the station must be off |
| `out in IR-Q-START ( -- )`, `IR-Q-STOP ( -- )` | IR in the background (IR-QUEUE) |
| `a n IR-Q-SEND ( -- ior )`, `IR-Q-RESULT ( -- ior \| 1 )` | send a frame of n times in us, then its result |
| `IR-Q-FRAME? ( -- a n end-us true \| false )`, `.IR-Q ( -- )` | the next received frame; the counters |

The words of NET (`TCP-OPEN`, `TCP-LISTEN`, `SOCK-READ`, `UDP-SEND`
...): `lib/net/README.md`, "The words of an application". All of the
packages: `lib/esp32/README.md`, "The WLAN of the WROOM" and "IR for the
bridge on the WROOM".

## Memory

Measured on the WROOM with all of the bridge in the flash: free are
27 432 bytes of dictionary, 41 248 bytes of RAM data, 99 020 bytes of
RAM data behind the frozen part (XDATA), and 72 956 bytes for `CODE`
words (targets/esp32/README.md, "Memory"). New buffers of the bridge
go into the XDATA.

## When it does not work

| What you see | What it means |
| --- | --- |
| `WBACK-START` throws -431 | no profile in the flash: step 3 |
| `WLAN retry code: -2`, again and again | the saved network is not in the scan: its name, 2.4 GHz, the distance |
| `WLAN retry code: -428` once | the access point sent a deauthentication, for example because it still knew the board from before a power loss; the next attempt works |
| `WLAN retry code: -302` | no address from DHCP within 15 s |
| `WLAN retry code: -433` | the radio got no end of a send: the next attempt starts it again |
| each `IR-Q-SEND` ends with -452, and `.IR-Q` shows many short frames | the input pin floats: check the wire IRin to GPIO 14 and the supply of the receiver |
| long frames end with -453 | the receiver shortens each time of light on long frames; see `lib/esp32/README.md`, "The echo of long frames" |

Before `FREEZE`: `WBACK-STOP` (or `SETUP-STOP`) and `IR-Q-STOP`.
