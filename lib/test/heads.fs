\ heads.fs - the packages that load on the host, once with TEMP-HEADS on (lib/package.fs)
\
\ With TEMP-HEADS on the PRIVATE words of a package keep no headers. A
\ package that takes words of another package with ALSO name.I then
\ finds only its PROTECTED words: a missing one stops the load (-13). The
\ white-box tests run with TEMP-HEADS off and cannot show this. This
\ file loads with TEMP-HEADS on what loads on the host: the packages
\ without hardware, IR and IR-QUEUE over their mock, WLAN and WIFI-LINK
\ over the mocks of the station. Then a few public words run. The board
\ loads the rest (apps/irbridge/app.fs). It needs 32-bit cells; in the
\ folder ucf:
\   build/ucf-c -32 host/files.fs lib/test/heads.fs </dev/null
\ tools/test.fs runs it too. At the end: "heads: n tests, 0 errors". The
\ lines "redefined IDLE" (the mock of IR) and "redefined LINK" (WIFI-LINK
\ after NO-LINK) are right.

ONLY FORTH DEFINITIONS  ALSO FILES
[DEFINED] T{ S" forth/test/tester.fs" ?INCLUDED
TEMP-HEADS ON!
\ The packages without hardware
ONLY FORTH ALSO FILES  [DEFINED] TEXT      S" lib/text.fs"               ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] BYTES     S" lib/net/bytes.fs"          ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] JSON      S" lib/json.fs"               ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] AC-BASE   S" lib/irremote/acbase.fs"    ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] IRCODE    S" lib/irremote/ircode.fs"    ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] IRCODE-READ  S" lib/irremote/irread.fs"  ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] RTY3      S" lib/irremote/rty3.fs"      ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] RTY3-READ  S" lib/irremote/rty3read.fs"  ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] KELON168  S" lib/irremote/kelon168.fs"  ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] KELON168-READ  S" lib/irremote/kelonrd.fs"  ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] IR-SYMBOLS  S" lib/irremote/irsym.fs"  ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] BRIDGE-FRAMES  S" apps/irbridge/frames.fs"  ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] NET       S" lib/net/nolink.fs"         ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] NET       S" lib/net/tcpip.fs"          ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] MQTT      S" lib/net/mqtt.fs"           ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] HTTP      S" lib/net/http.fs"           ?INCLUDED
\ IR and IR-QUEUE over the mock (as lib/esp32/test/irqueue.fs). It comes after the
\ others: its names RMT GPIO INTR IR-CAPTURE TIMERS IDLE CHIP give IR-MOCK.
ONLY FORTH DEFINITIONS ALSO FILES  INCLUDE lib/esp32/test/irmock.fs
ONLY FORTH DEFINITIONS ALSO FILES  INCLUDE lib/esp32/ir.fs
ONLY FORTH DEFINITIONS ALSO FILES  INCLUDE lib/esp32/esp32/irqueue.fs
\ WLAN and WIFI-LINK over the mocks of the station (as lib/net/test/wpasta.fs and wlink.fs)
ONLY FORTH ALSO FILES  [DEFINED] SHA1      S" lib/sha1.fs"               ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] KDF1      S" lib/kdf1.fs"               ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] AES128    S" lib/aes128.fs"             ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] WPAKEY    S" lib/net/wpakey.fs"         ?INCLUDED
ONLY FORTH ALSO FILES  S" lib/net/test/stamock.fs"  INCLUDED
ONLY FORTH ALSO FILES  S" lib/net/wifi.fs"          INCLUDED
ONLY FORTH ALSO FILES  S" lib/net/wpasta.fs"        INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] CCM       S" lib/ccm.fs"                ?INCLUDED
ONLY FORTH ALSO FILES  S" lib/net/ccmp.fs"          INCLUDED
ONLY FORTH ALSO FILES  S" lib/net/test/txmock.fs"   INCLUDED
ONLY FORTH ALSO FILES  S" lib/net/wlink.fs"         INCLUDED
TEMP-HEADS OFF!

ONLY FORTH DEFINITIONS  ALSO INTERNALS
T{ TEMP-START @ DICT-END = -> TRUE }T        \ each package gave its temp store back
\ name.I only where a package has PROTECTED words; their headers stay, the PRIVATE ones go
T{ S" IR.I" FIND-NAME 0<>  S" IR-QUEUE.I" FIND-NAME 0<>  S" MQTT.I" FIND-NAME  S" JSON.I" FIND-NAME -> TRUE TRUE 0 0 }T
T{ S" WLAN.I" FIND-NAME 0<>  S" WIFI.I" FIND-NAME 0<>  S" WIFI-LINK.I" FIND-NAME -> TRUE TRUE 0 }T
T{ ALSO WLAN.I  S" STA-PMK" FIND-NAME 0<>  S" STA-PTK" FIND-NAME  PREVIOUS -> TRUE 0 }T
ONLY FORTH DEFINITIONS  ALSO RTY3  ALSO RTY3-READ  ALSO TEXT
CREATE B19  $60 C, $78 C, $09 C, $03 C, $7F C, $00 C, $00 C, $00 C, $00 C, $00 C, $00 C, $0F C, $73 C,
T{ B19 13 AC>TIMES AC-DECODE  SWAP 13 B19 13 STR= -> 104 TRUE }T   \ the code of RTY3 runs without its headers
ONLY FORTH DEFINITIONS  ALSO AC-BASE  ALSO KELON168  ALSO KELON168-READ
T{ AC-RESET  TRUE AC-POWER!  AC-TIMINGS AC-DECODE  NIP  AC-FRAME AC-OK? -> 168 TRUE }T   \ and so of KELON168
ONLY FORTH DEFINITIONS  ALSO BRIDGE-FRAMES
: DQ ( a u -- a u )   \ each ' of the text becomes a ": S" cannot hold a "
  2DUP OVER + SWAP ?DO  I C@ 39 = IF  34 I C!  THEN  LOOP ;
: PD-A1 ( -- a u )   S" {'schema':'pd','shape':[9000,4500,560,1690,540,0,800],'hex':'60381305FF005020DC0015030000000003030006000046'}" DQ ;
: RTY3-22 ( -- a u )   S" {'schema':'rty3','temp':22}" DQ ;
T{ PD-A1 REQUEST-FRAME NIP NIP  RTY3-22 REQUEST-FRAME NIP NIP -> 0 0 }T   \ BRIDGE-FRAMES too
T{ PD-A1 REQUEST-FRAME DROP NIP  S" {}" REQUEST-FRAME NIP NIP -> 371 -480 }T

S" heads" .RESULTS
