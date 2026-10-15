\ wlstack.fs - the WLAN of the WROOM for ucf term: radio, WPA2 station, setup AP, one LINK
\
\ One #require loads what the WLAN of the WROOM needs, in this order:
\ ESP32-PHY (the calibration; it links C code into SRAM0), BYTES, ESP32-MAC,
\ ESP32-RADIO, WIFI (the scan), SHA1, KDF1, AES128, WPAKEY, WLAN (the WPA2
\ station), FLASH-PAIR and WLSAVE (the profile in the flash), TEXT, INET, CCMP, WIFI-LINK
\ and AP-LINK (the two LINKs) and RADIO-LINK (it selects one of them). At the
\ end both LINKs get their packet RAM: RADIO-POOL LINK-POOL! The radio has
\ hardware CCMP, so the CCMP in software (lib/net/wlsoft.fs, package CCM) is
\ not loaded. #require loads only what the board does not have yet. Before
\ each one ONLY FORTH: each package adds its name to the search order, which
\ has 16 places.
\
\ ESP32-PHY loads only right after a reset. In ucf term:
\   #require RADIO-LINK lib/esp32/esp32/wlstack.fs
\ apps/irbridge/app.fs does it. The packages: lib/esp32/README.md, "WLAN".

ONLY FORTH
#require ESP32-PHY phy.fs
ONLY FORTH
#require BYTES ../../net/bytes.fs
ONLY FORTH
#require ESP32-MAC macinit.fs
ONLY FORTH
#require ESP32-RADIO radio.fs
ONLY FORTH
#require WIFI ../../net/wifi.fs
ONLY FORTH
#require SHA1 ../../sha1.fs
ONLY FORTH
#require KDF1 ../../kdf1.fs
ONLY FORTH
#require AES128 ../../aes128.fs
ONLY FORTH
#require WPAKEY ../../net/wpakey.fs
ONLY FORTH
#require WLAN ../../net/wpasta.fs
ONLY FORTH
#require FLASH-PAIR ../flpair.fs
ONLY FORTH
#require WLSAVE wlsave.fs
ONLY FORTH
#require TEXT ../../text.fs
ONLY FORTH
#require INET ../../net/inet.fs
ONLY FORTH
#require CCMP ../../net/ccmp.fs
ONLY FORTH
#require WIFI-LINK ../../net/wlink.fs
ONLY FORTH
#require AP-LINK ../../net/aplink.fs
ONLY FORTH
#require RADIO-LINK ../../net/linksel.fs
ONLY FORTH ALSO RADIO ALSO RADIO-LINK  RADIO-POOL LINK-POOL!
ONLY FORTH
