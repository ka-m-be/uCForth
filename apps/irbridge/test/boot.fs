\ boot.fs - the test of BRIDGE-BOOT on the host: the start without a terminal (apps/irbridge/)
\
\ WLSAVE, WLAN-BACK, BRIDGE-SETTINGS, IRBRIDGE, BRIDGE-WEB, BRIDGE-SETUP,
\ MQTT, GPIO and BOARD are mocks here: they count what the start asks of
\ them; LEVEL is the level of the button (TRUE: up), LIGHT the level of the
\ LED. The step of the start runs with IDLE-RUN, as KEY runs it on the
\ board. Each system with files; in the folder ucf:
\   build/ucf-c host/files.fs apps/irbridge/test/boot.fs </dev/null
\ tools/test.fs runs it too. At the end: "boot: n tests, 0 errors".

ONLY FORTH DEFINITIONS  ALSO FILES
[DEFINED] T{ S" forth/test/tester.fs" ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] IDLE  S" lib/idle.fs"  ?INCLUDED
ONLY FORTH DEFINITIONS
VARIABLE PROFILE?   VARIABLE BROKER?   VARIABLE #JOINS   VARIABLE PIN-OUT   VARIABLE PIN-IN   VARIABLE #WEB
VARIABLE LEVEL   LEVEL ON!   VARIABLE PULLED   -1 PULLED !   VARIABLE LIGHT   VARIABLE LIGHT-PIN
VARIABLE #SETUPS   VARIABLE #RESTARTS   VARIABLE SETUP-FAIL   VARIABLE PHASE-NOW
VARIABLE READY?   READY? ON!   VARIABLE MQTT?   MQTT? ON!   VARIABLE GAVE-UP?   VARIABLE START-FAIL
PACKAGE WLSAVE  PUBLIC  : WLAN-SAVED? ( -- f )   PROFILE? @ ;  END-PACKAGE
PACKAGE WLAN-BACK  PUBLIC
: WBACK-START ( -- )   1 #JOINS +! ;   : WBACK-READY? ( -- f )   READY? @ ;   : WBACK-GAVE-UP? ( -- f )   GAVE-UP? @ ;
END-PACKAGE
PACKAGE BRIDGE-SETTINGS  PUBLIC  : BRIDGE-LOAD ( -- f )   BROKER? @ ;  END-PACKAGE
PACKAGE IRBRIDGE  PUBLIC  : BRIDGE-START ( out in -- )   START-FAIL @ ABORT" mock: no start"  PIN-IN !  PIN-OUT ! ;  END-PACKAGE
PACKAGE BRIDGE-WEB  PUBLIC  : BRIDGE-WEB-START ( -- )   1 #WEB +! ;  END-PACKAGE
PACKAGE BRIDGE-SETUP  PUBLIC
: BRIDGE-SETUP-START ( -- )   SETUP-FAIL @ ABORT" mock: no setup"  1 #SETUPS +! ;
: BRIDGE-RESTART ( -- )   1 #RESTARTS +! ;
: BRIDGE-SETUP-PHASE ( -- n )   PHASE-NOW @ ;
END-PACKAGE
PACKAGE MQTT  PUBLIC  : MQTT-READY? ( -- f )   MQTT? @ ;  END-PACKAGE
PACKAGE GPIO  PUBLIC
: INPUT ( pin -- )   DROP ;   : PULLUP ( pin -- )   PULLED ! ;   : PIN@ ( pin -- f )   DROP LEVEL @ ;
: PIN! ( f pin -- )   LIGHT-PIN !  LIGHT ! ;
END-PACKAGE
PACKAGE BOARD  PUBLIC  0 CONSTANT BUTTON   2 CONSTANT LED   : LED-OFF ( -- )   FALSE LED PIN! ;  END-PACKAGE
ONLY FORTH DEFINITIONS  ALSO FILES  INCLUDE apps/irbridge/boot.fs
ONLY FORTH DEFINITIONS  ALSO INTERNALS  ALSO IDLE  ALSO BRIDGE-BOOT  ALSO BRIDGE-BOOT.I
DECIMAL
: STARTS ( -- joins out in web )   \ BRIDGE-AUTO, then a step of IDLE
  0 #JOINS !  0 PIN-OUT !  0 PIN-IN !  0 #WEB !  BRIDGE-AUTO  IDLE-RUN  #JOINS @ PIN-OUT @ PIN-IN @ #WEB @ ;

\ ---- the start: the page and the button in each case, the bridge with a profile, also without a broker
T{ PROFILE? OFF!  BROKER? ON!  STARTS -> 0 0 0 1 }T          \ no profile: the page and the button only
T{ PROFILE? ON!  BROKER? OFF!  STARTS -> 1 4 14 1 }T         \ no broker: the bridge without MQTT, the page sets it
T{ BROKER? ON!  STARTS -> 1 4 14 1 }T                        \ both
T{ READY? OFF!  STARTS  IDLE-RUN  PIN-OUT @ -> 1 0 0 1 0 }T  \ the bridge waits for the station: its MAC is the ID
T{ READY? ON!  IDLE-RUN  PIN-OUT @ PIN-IN @  IDLE-RUN  STARTING @ -> 4 14 FALSE }T   \ then once
T{ START-FAIL ON!  STARTS  START-FAIL OFF!  WATCHING @ -> 1 0 0 1 TRUE }T   \ no start: the button stays
T{ BRIDGE-BOOT-ON  'SETUP @  'LOOP @  AUTORUN @ -> ' BRIDGE-AUTO ' QUIT TRUE }T
T{ BRIDGE-BOOT-OFF  'SETUP @  'LOOP @  AUTORUN @ -> ' NOOP ' NOOP FALSE }T

\ ---- the LED: off when WLAN and broker are connected, else short blips; slow blinks from "Save" on
T{ 10 LED-LEVEL  900 LED-LEVEL -> FALSE FALSE }T                                  \ connected
T{ MQTT? OFF!  10 LED-LEVEL  49 LED-LEVEL  50 LED-LEVEL  900 LED-LEVEL -> TRUE TRUE FALSE FALSE }T
T{ MQTT? ON!  READY? OFF!  10 LED-LEVEL  READY? ON! -> TRUE }T                    \ no WLAN
T{ 1 PHASE-NOW !  MQTT? OFF!  10 LED-LEVEL  900 LED-LEVEL -> TRUE FALSE }T        \ the setup network is open
T{ 3 PHASE-NOW !  10 LED-LEVEL  499 LED-LEVEL  500 LED-LEVEL  900 LED-LEVEL -> TRUE TRUE FALSE FALSE }T
T{ 0 PHASE-NOW !  MQTT? ON!  -1 LIGHT !  0 LIGHT-PIN !  IDLE-RUN  LIGHT @  LIGHT-PIN @ -> FALSE 2 }T   \ the step sets it

\ ---- WLAN-BACK ended by itself: a restart
T{ 0 #RESTARTS !  GAVE-UP? ON!  IDLE-RUN  GAVE-UP? OFF!  #RESTARTS @ -> 1 }T

\ ---- the button BOOT: 3 s down starts the setup, once
T{ STARTS 2DROP 2DROP  0 #SETUPS !  PULLED @  WATCHING @  IDLE-RUN  #SETUPS @ -> 0 TRUE 0 }T   \ up
T{ LEVEL OFF!  IDLE-RUN  #SETUPS @ -> 0 }T                    \ down, not yet 3 s
T{ MILLIS 3001 - UP-AT !  IDLE-RUN  #SETUPS @  WATCHING @ -> 1 FALSE }T
T{ MILLIS 9000 - UP-AT !  IDLE-RUN  #SETUPS @  #RESTARTS @ -> 1 1 }T   \ held on: not again
T{ LEVEL ON!  STARTS 2DROP 2DROP  LEVEL OFF!  MILLIS 2000 - UP-AT !  IDLE-RUN  LEVEL ON!  IDLE-RUN
   LEVEL OFF!  IDLE-RUN  #SETUPS @  MILLIS UP-AT @ - 1000 < -> 1 TRUE }T     \ up between: the time again

\ ---- the setup cannot start: the chip starts again
T{ SETUP-FAIL ON!  MILLIS 3001 - UP-AT !  IDLE-RUN  #RESTARTS @  #SETUPS @  WATCHING @ -> 2 1 FALSE }T

' WATCH IDLE-
S" boot" .RESULTS
