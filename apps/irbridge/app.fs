\ app.fs - the IR bridge on the ESP32 WROOM: IR frames over MQTT
\
\ The bridge connects IR devices (an air conditioner) to MQTT. The station
\ joins the saved WLAN and keeps it (WLAN-BACK), a temporary open network
\ sets up the board (SETUP-AP), our TCP/IP gives sockets over both (NET),
\ IR frames go out and come in while the console and the network work
\ (IR-QUEUE), and the bridge (IRBRIDGE, bridge.fs) reports the frames
\ that it hears to a broker and sends those that it gets (MQTT, JSON):
\ as a table of durations (IR-SYMBOLS), in the pulse distance code
\ (IRCODE) or as the state of the air conditioner (AC-BASE, RTY3); the
\ texts of the frames are in BRIDGE-FRAMES (frames.fs). The broker stays
\ in the flash (BRIDGE-SETTINGS, settings.fs), and BRIDGE-BOOT (boot.fs)
\ starts the bridge without a terminal, with its web page (BRIDGE-WEB,
\ web.fs, over HTTP); the button BOOT of the board (BOARD) starts the
\ setup of the WLAN and the broker over a setup network (BRIDGE-SETUP,
\ setup.fs).
\ Wiring, words and troubleshooting: apps/irbridge/README.md.
\
\ Start on the Mac, in the top folder, with the base image in the flash
\ (targets/esp32/README.md: ucf flash, then one #reset):
\   build/ucf term /dev/cu.usbserial-110 apps/irbridge/app.fs
\ The first run takes about 3 minutes. The RAM data of the base are too
\ small for all buffers, so the loader freezes the first part (the radio
\ and WLAN) into the flash and gives the RAM behind it to the next buffers
\ (XDATA-FREEZE), then loads the rest and freezes again (?FREEZE). After a
\ reset the board has all of it, with the radio and IR off. A second run
\ loads nothing and writes nothing. After a start the search order has only
\ FORTH: ALSO WLAN-BACK ALSO MQTT ALSO BRIDGE-SETTINGS ALSO IRBRIDGE. The
\ PRIVATE words of the packages keep no headers in the image (TEMP-HEADS
\ ON!, lib/package.fs): the dictionary has room for more; their names are
\ gone on the board.

TEMP-HEADS ON!
#require RADIO-LINK ../../lib/esp32/esp32/wlstack.fs
ONLY FORTH ALSO BOOTIMG  XDATA-FREEZE
#require NET-TIMER ../../lib/esp32/tcpip.fs
ONLY FORTH
#require WLAN-BACK ../../lib/esp32/esp32/wlback.fs
ONLY FORTH
#require DHCP-SERVER ../../lib/net/dhcps.fs
ONLY FORTH
#require SETUP-AP ../../lib/esp32/esp32/wlap.fs
ONLY FORTH
#require RMT ../../lib/esp32/rmt.fs
ONLY FORTH
#require IR ../../lib/esp32/ir.fs
ONLY FORTH
#require IR-CAPTURE ../../lib/esp32/esp32/ircap.fs
ONLY FORTH
#require IR-QUEUE ../../lib/esp32/esp32/irqueue.fs
ONLY FORTH
#require MQTT ../../lib/net/mqtt.fs
ONLY FORTH
#require JSON ../../lib/json.fs
ONLY FORTH
#require AC-BASE ../../lib/irremote/acbase.fs
ONLY FORTH
#require IRCODE ../../lib/irremote/ircode.fs
ONLY FORTH
#require RTY3 ../../lib/irremote/rty3.fs
ONLY FORTH
#require IR-SYMBOLS ../../lib/irremote/irsym.fs
ONLY FORTH
#require BRIDGE-FRAMES frames.fs
ONLY FORTH
#require IRBRIDGE bridge.fs
ONLY FORTH
#require BRIDGE-SETTINGS settings.fs
ONLY FORTH
#require HTTP ../../lib/net/http.fs
ONLY FORTH
#require BRIDGE-SETUP setup.fs
ONLY FORTH
#require BRIDGE-WEB web.fs
ONLY FORTH
#require BOARD ../../lib/esp32/board.fs
ONLY FORTH
#require BRIDGE-BOOT boot.fs
TEMP-HEADS OFF!                   \ later loads keep their headers
ONLY FORTH ALSO BOOTIMG  ?FREEZE
ONLY FORTH DEFINITIONS  ALSO IRBRIDGE  ALSO BRIDGE-SETTINGS  ALSO BRIDGE-WEB  ALSO BRIDGE-SETUP  ALSO MQTT  ALSO IR-QUEUE  ALSO SETUP-AP  ALSO WLAN-BACK
.( irbridge: ready; WBACK-START joins the saved WLAN, then BRIDGE-LOAD and 4 14 BRIDGE-START) CR
