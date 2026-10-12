\ app.fs - the IR bridge on the ESP32 WROOM: its base so far (WLAN, setup network, IR in the background)
\
\ The bridge will connect IR devices (an air conditioner) to MQTT and a web
\ page. Its base is here: the station joins the saved WLAN and keeps it
\ (WLAN-BACK), a temporary open network sets up the board (SETUP-AP), our
\ TCP/IP gives sockets over both (NET), and IR frames go out and come in
\ while the console and the network work (IR-QUEUE). MQTT, the web page and
\ the settings come next. Wiring, words and troubleshooting:
\ apps/irbridge/README.md.
\
\ Start on the Mac, in the top folder, with the base image in the flash
\ (targets/esp32/README.md: ucf flash, then one #reset):
\   build/ucf term /dev/cu.usbserial-110 apps/irbridge/app.fs
\ The first run takes about 3 minutes. The RAM data of the base are too
\ small for all buffers, so the loader freezes the first part (the radio
\ and WLAN) into the flash and gives the RAM behind it to the next buffers
\ (XDATA-FREEZE), then loads the rest and freezes again (?FREEZE). After a
\ reset the board has all of it, with the radio and IR off. A second run
\ loads nothing and writes nothing.

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
#require IR-QUEUE ../../lib/esp32/irqueue.fs
ONLY FORTH ALSO BOOTIMG  ?FREEZE
ONLY FORTH DEFINITIONS  ALSO IR-QUEUE  ALSO SETUP-AP  ALSO WLAN-BACK
.( irbridge: ready; WBACK-START joins the saved WLAN, 4 14 IR-Q-START starts IR) CR
