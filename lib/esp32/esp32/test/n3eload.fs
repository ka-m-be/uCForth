\ n3eload.fs - real WROOM, passive scan only; leave RAM ready for local setup
\ Hard lsof guard before every port access. No flash write or credential input.
#reset
ONLY FORTH DEFINITIONS ALSO INTERNALS
VARIABLE SIZE-D VARIABLE SIZE-V
: SIZE-START ( -- ) HERE SIZE-D ! VP @ SIZE-V ! ;
: SIZE-END ( a n -- ) TYPE HERE SIZE-D @ - . VP @ SIZE-V @ - . CR ;
SIZE-START
#require ESP32-PHY ../phy.fs
S" PHY dictionary/data: " SIZE-END
ONLY FORTH
SIZE-START
#require BYTES ../../../net/bytes.fs
S" BYTES dictionary/data: " SIZE-END
ONLY FORTH
SIZE-START
#require ESP32-MAC ../macinit.fs
S" MAC dictionary/data: " SIZE-END
ONLY FORTH
SIZE-START
#require ESP32-RADIO ../radio.fs
S" RADIO dictionary/data: " SIZE-END
ONLY FORTH
SIZE-START
#require WIFI ../../../net/wifi.fs
S" WIFI dictionary/data: " SIZE-END
ONLY FORTH
#require SHA1 ../../../sha1.fs
ONLY FORTH
#require KDF1 ../../../kdf1.fs
ONLY FORTH
#require AES128 ../../../aes128.fs
ONLY FORTH
#require WPAKEY ../../../net/wpakey.fs
ONLY FORTH
SIZE-START
#require WLAN ../../../net/wpasta.fs
S" WLAN dictionary/data: " SIZE-END
ONLY FORTH
SIZE-START
#require WLAN-SETUP ../../../net/wlsetup.fs
S" WLAN-SETUP dictionary/data: " SIZE-END
ONLY FORTH
SIZE-START
#require WLSAVE ../wlsave.fs
S" WLSAVE dictionary/data: " SIZE-END
ONLY FORTH DEFINITIONS ALSO RADIO ALSO WIFI ALSO WLAN ALSO WLAN-SETUP ALSO WLSAVE ALSO BYTES
RADIO-START RADIO-FACTORY
32 BUFFER: RANDOM-A
32 BUFFER: RANDOM-B
: RANDOM-HEALTH ( -- f )
  RANDOM-A 32 RADIO-RANDOM RANDOM-B 32 RADIO-RANDOM
  RANDOM-A RANDOM-B 32 BYTES= 0=
  RANDOM-A 32 0 FILL RANDOM-B 32 0 FILL ;
: CHECK-RANDOM ( -- ) RANDOM-HEALTH 0= ABORT" WLAN: random source did not change" ;
CHECK-RANDOM
." passive scan count: " WIFI-SCAN . CR
." saved profile present: " WLAN-SAVED? . CR
." Local only: .WIFI; choose the row from 0; i WLAN-PASSWORD; WIFI-WAIT ." CR
." If state 6 and error 0: WLAN-SAVE. No credentials in chat or logs." CR
#bye
