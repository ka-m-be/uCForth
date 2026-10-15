\ settings.fs - the test of BRIDGE-SETTINGS on the host: the broker in the flash (apps/irbridge/)
\
\ The flash and MQTT are mocks here (lib/esp32/test/flmock.fs, test/mocks.fs).
\ 32-bit cells (SHA1); in the folder ucf:
\   build/ucf-c -32 host/files.fs apps/irbridge/test/settings.fs </dev/null
\ tools/test.fs runs it too. At the end: "settings: n tests, 0 errors".

ONLY FORTH DEFINITIONS  ALSO FILES
[DEFINED] T{ S" forth/test/tester.fs" ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] TEXT   S" lib/text.fs"       ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] SHA1   S" lib/sha1.fs"       ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] BYTES  S" lib/net/bytes.fs"  ?INCLUDED
ONLY FORTH DEFINITIONS  ALSO TEXT  ALSO FILES
INCLUDE apps/irbridge/test/mocknet.fs
INCLUDE apps/irbridge/test/mocks.fs
ONLY FORTH DEFINITIONS ALSO FILES  INCLUDE lib/esp32/test/flmock.fs
ONLY FORTH DEFINITIONS ALSO FILES  INCLUDE lib/esp32/flpair.fs
ONLY FORTH DEFINITIONS ALSO FILES  INCLUDE apps/irbridge/settings.fs
ONLY FORTH DEFINITIONS  ALSO TEXT  ALSO BYTES  ALSO MQTT  ALSO FLASH  ALSO FLASH.I  ALSO BRIDGE-SETTINGS
DECIMAL
: BROKER? ( a u port -- f )   \ MQTT has this broker
  MQTT-BROKER@ >R ROT R> =  >R STR= R> AND ;

T{ BRIDGE-LOAD -> FALSE }T                                   \ nothing saved
T{ ' BRIDGE-SAVE CATCH -> -2 }T                              \ no broker yet
T{ S" 192.168.1.20" 1883 MQTT-BROKER  BRIDGE-SAVE  $3FC000 AT @ -> $31425249 }T
T{ $3FC008 AT LE16@  $3FC00A AT C@  $3FC00B AT 12 S" 192.168.1.20" STR= -> 1883 12 TRUE }T
T{ S" 10.0.0.1" 8883 MQTT-BROKER  BRIDGE-LOAD  S" 192.168.1.20" 1883 BROKER? -> TRUE TRUE }T
T{ S" 10.0.0.1" 8883 MQTT-BROKER  BRIDGE-SAVE  S" x" 1 MQTT-BROKER  BRIDGE-LOAD
   S" 10.0.0.1" 8883 BROKER?  $3FD000 AT @ -> TRUE TRUE $31425249 }T   \ the second sector
T{ $3FE000 AT @  $3FF000 AT @ -> -1 -1 }T                    \ the sectors of the WLAN stay
T{ BRIDGE-FORGET  BRIDGE-LOAD -> FALSE }T

S" settings" .RESULTS
