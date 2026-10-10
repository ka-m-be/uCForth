\ app.fs - the application aircon: a Hisense air conditioner over infrared
\
\ The board speaks as the remote control RCH-RTY3-0 of a Hisense air
\ conditioner (package RTY3), through the IR transceiver of Adafruit
\ (product 5990, package IR), and it listens to the remote control.
\ Wiring, start and all words: apps/aircon/README.md.
\
\ Start on the Mac, in the folder ucf (the board runs uCForth):
\   build/ucf term /dev/cu.usbmodem5C4C0505931 apps/aircon/app.fs
\ Then at the prompt, for example:
\   AIRCON-ON  HEAT MODE  23 TEMP  .AC  AIRCON-HELP
\ This script loads only what the board does not have yet: it works
\ again after #bye, and also with the application in the flash (FREEZE).

#require RMT ../../lib/esp32/rmt.fs
#require IR ../../lib/esp32/ir.fs
#require IRCODE ../../lib/irremote/ircode.fs
#require RTY3 ../../lib/irremote/rty3.fs
#require AIRCON aircon.fs
ONLY FORTH DEFINITIONS  ALSO AIRCON  AIRCON-START
.( aircon: ready; AIRCON-HELP shows the commands) CR
