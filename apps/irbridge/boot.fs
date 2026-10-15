\ boot.fs - package BRIDGE-BOOT: the IR bridge starts by itself, without a terminal
\
\ After BRIDGE-BOOT-ON and FREEZE the board starts the bridge after each
\ reset and power on (TURNKEY, forth/base.fs), as a product: it joins the
\ saved WLAN (WLAN-BACK), takes the saved broker (BRIDGE-LOAD) and, when
\ the station is ready (its MAC is the ID of the bridge), starts the bridge
\ with the LEDs at GPIO 4 and the receiver at GPIO 14. The console stays
\ open ('LOOP is QUIT), as with WLAN-BOOT-ON. Ctrl-C or Esc in the first
\ second gives only the console (BOOT, forth/base.fs).
\
\ In each case BRIDGE-AUTO starts the web page (BRIDGE-WEB-START) and
\ watches the button BOOT (GPIO 0, BUTTON of the board): held down for
\ 3 s, it starts the setup (BRIDGE-SETUP-START, setup.fs). No network
\ opens by itself. Without a profile the board waits for the button;
\ without a broker the bridge runs without MQTT, and its web page sets the
\ broker. The LED of the board (GPIO 2) shows the state:
\   off           the WLAN and the broker are connected
\   short blips   one of them is not: no profile, the network or the
\                 broker is away, the setup network is open
\   slow blinks   the setup derives the key and joins (500 ms on, 500 ms
\                 off), up to the restart
\ When WLAN-BACK ends by itself (WBACK-GAVE-UP?), the chip starts again.
\
\   BRIDGE-BOOT-ON ( -- )    the start of the bridge after each reset, from the next FREEZE on
\   BRIDGE-BOOT-OFF ( -- )   no start any more, from the next FREEZE on
\   BRIDGE-AUTO ( -- )       the start itself, as 'SETUP runs it
\
\ FREEZE only with the radio off and no key in the RAM: right after a reset,
\ with Ctrl-C in the first second (apps/irbridge/README.md, "Start").
\ Load it after WLSAVE, WLAN-BACK, BRIDGE-SETTINGS, IRBRIDGE, BRIDGE-WEB,
\ BRIDGE-SETUP, MQTT and BOARD (apps/irbridge/app.fs does it). Test on the
\ host: apps/irbridge/test/boot.fs.

PACKAGE BRIDGE-BOOT  ALSO WLSAVE  ALSO WLAN-BACK  ALSO BRIDGE-SETTINGS  ALSO IRBRIDGE  ALSO BRIDGE-WEB
ALSO BRIDGE-SETUP  ALSO MQTT  ALSO GPIO  ALSO BOARD  ALSO IDLE   \ lib/package.fs

4 CONSTANT LEDS                     \ GPIO 4: IRout of the transceiver
14 CONSTANT RECEIVER                \ GPIO 14: IRin
3000 CONSTANT HOLD-MS               \ the button so long down: the setup
50 CONSTANT BLIP-MS                 \ not connected: the LED so long on in each second
500 CONSTANT SLOW-MS                \ the setup derives the key: so long on, so long off
VARIABLE WATCHING                   \ BRIDGE-AUTO came, the setup not yet
VARIABLE UP-AT                      \ MILLIS: the button was up then
VARIABLE STARTING                   \ the bridge starts when the station is ready

: LED-LEVEL ( ms -- f )   \ the LED at the ms of a second: off, short blips or slow blinks (see above)
  BRIDGE-SETUP-PHASE 2 < IF
    WBACK-READY? MQTT-READY? AND IF  DROP FALSE  ELSE  BLIP-MS <  THEN
  ELSE  SLOW-MS <  THEN ;
: SHOW ( -- )   MILLIS 0 1000 UM/MOD DROP  LED-LEVEL  LED PIN! ;
: START-BRIDGE ( -- )   LEDS RECEIVER BRIDGE-START  ." bridge: started" CR ;
: WATCH ( -- )   \ in IDLE: the LED; the bridge when the station is ready; WLAN-BACK; the button
  SHOW
  STARTING @ IF  WBACK-READY? IF
    STARTING OFF!  ['] START-BRIDGE CATCH ?DUP IF  ." bridge: no start, code " . CR  THEN   \ the button stays
  THEN  THEN
  WBACK-GAVE-UP? IF  ." bridge: WLAN-BACK ended by itself: a restart" CR  BRIDGE-RESTART  THEN
  WATCHING @ 0= IF EXIT THEN
  BUTTON PIN@ IF  MILLIS UP-AT !  EXIT  THEN
  MILLIS UP-AT @ - HOLD-MS < IF EXIT THEN
  WATCHING OFF!  STARTING OFF!  ['] BRIDGE-SETUP-START CATCH IF  BRIDGE-RESTART  THEN ;   \ if it cannot: a restart

PUBLIC
: BRIDGE-AUTO ( -- )
  LED-OFF  BUTTON INPUT  BUTTON PULLUP  MILLIS UP-AT !  WATCHING ON!  ['] WATCH IDLE+  BRIDGE-WEB-START
  BRIDGE-LOAD 0= IF  ." bridge: no broker; the web page sets it" CR  THEN
  WLAN-SAVED? 0= IF  ." bridge: no WLAN profile; BOOT 3 s gives the setup" CR  EXIT  THEN
  WBACK-START  STARTING ON! ;
: BRIDGE-BOOT-ON ( -- )   ['] BRIDGE-AUTO 'SETUP !  ['] QUIT 'LOOP !  TURNKEY ;
: BRIDGE-BOOT-OFF ( -- )   AUTORUN OFF!  ['] NOOP 'SETUP !  ['] NOOP 'LOOP ! ;

END-PACKAGE
