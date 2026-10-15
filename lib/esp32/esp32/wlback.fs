\ wlback.fs - package WLAN-BACK: the station joins its saved network and stays joined
\
\ The WROOM keeps one WLAN profile in its flash (package WLSAVE). WLAN-BACK
\ uses it without a terminal: a step of it runs in the chain of IDLE, while
\ the console waits. The step starts the radio, scans the channels one by
\ one, takes the strongest access point with the saved name, joins it with
\ the saved key, starts WIFI-LINK and the network (NET-START) and gets the
\ address from DHCP. Then it watches the link: when the beacons of the
\ access point stay away for 5 s, or the link fails, it stops all and tries
\ again after a pause: 1 s, then 2, 4, 8 ... at most 32 s, until WBACK-STOP.
\ An error in a step does not end the recovery: the step stops all and
\ tries again later, with the error as the code of the failure. Only
\ Ctrl-C goes through. The words show numbers only, no name and no key.
\
\   WBACK-START ( -- )      start the recovery; the station LINK is selected
\   WBACK-STOP ( -- )       stop the recovery, the network and the radio; the
\                           keys in the RAM go, the saved profile stays
\   WBACK-READY? ( -- f )   joined, the link is up, the address came from DHCP
\   WBACK-RUNNING? ( -- f ) between WBACK-START and WBACK-STOP
\   WBACK-GAVE-UP? ( -- f ) a step failed, and even its stop: the recovery
\                           ended by itself (an application may start the
\                           chip again); false again after WBACK-START
\   .WBACK ( -- )           the state, the attempts, the joins, the code of the
\                           last failure (0 after a join)
\   WLAN-BOOT-ON ( -- )     after each start of the board: WBACK-START, then
\                           the console; FREEZE keeps it (TURNKEY)
\   WLAN-BOOT-OFF ( -- )    the normal start again (the radio stays off); FREEZE
\
\ The states: 0 wait for the next attempt, 1 scan, 2 join, 3 DHCP, 4 ready.
\ The setup AP (package SETUP-AP) and WLAN-BACK never run at the same time.
\ FREEZE only after WBACK-STOP: then no key is in the RAM.
\
\ Load it in ucf term after the radio and our TCP/IP (apps/irbridge/app.fs
\ does it). Example, with a saved profile:
\   ALSO WLAN-BACK  WBACK-START
\   ( 2 s later )   WBACK-READY? .  .WBACK
\   -1 WLAN back state/attempts/joins/code: 4 1 1 0
\ The test on the host: lib/esp32/test/wlback.fs.

PACKAGE WLAN-BACK ALSO RADIO ALSO WIFI ALSO WLAN ALSO WLSAVE ALSO WIFI-LINK ALSO RADIO-LINK ALSO NETIF ALSO NET-TIMER ALSO IDLE

0 CONSTANT S-WAIT                \ the states of BACK-STATE
1 CONSTANT S-SCAN
2 CONSTANT S-JOIN
3 CONSTANT S-DHCP
4 CONSTANT S-READY
VARIABLE ENABLED                 \ between WBACK-START and WBACK-STOP
VARIABLE BACK-STATE
VARIABLE DUE                     \ MILLIS: the next attempt (S-WAIT), the end of DHCP (S-DHCP)
VARIABLE DELAY-MS                \ the pause before the next attempt
VARIABLE LAST-ERROR              \ the code of the last failure; 0 after a join
VARIABLE ATTEMPTS
VARIABLE JOINS
VARIABLE GAVE-UP                 \ a step failed, and even its stop

: OFFLINE ( -- )   \ all off: network, link, scan, station, radio; the addresses go
  NET-STOP WLINK-STOP WIFI-SCAN-CANCEL WIFI-LEAVE RADIO-STOP NET-DOWN ;
: FAILED ( ior -- )   \ all off; the next attempt after the pause, which then doubles
  DUP LAST-ERROR !  ." WLAN retry code: " . CR
  OFFLINE  S-WAIT BACK-STATE !  MILLIS DELAY-MS @ + DUE !
  DELAY-MS @ 2* 32000 MIN DELAY-MS ! ;
: DUE? ( -- f )   MILLIS DUE @ - 0< 0= ;
: STEP ( -- )   \ one step of the recovery
  BACK-STATE @ CASE
    S-WAIT OF
      DUE? IF  RADIO-START RADIO-FACTORY WIFI-SCAN-START  1 ATTEMPTS +!  S-SCAN BACK-STATE !  THEN
    ENDOF
    S-SCAN OF
      WIFI-SCAN-STEP IF  WLAN-SAVED-NET WLAN-JOIN  S-JOIN BACK-STATE !  THEN
    ENDOF
    S-JOIN OF
      WIFI-POLL
      WIFI-STATUS 7 = IF  WIFI-ERROR FAILED  ELSE
        WIFI-STATUS 6 = IF
          WLINK-START NET-DHCP NET-START  S-DHCP BACK-STATE !  MILLIS 15000 + DUE !
        THEN
      THEN
    ENDOF
    S-DHCP OF
      (NET-UP) DUP 1 = IF
        DROP  S-READY BACK-STATE !  1000 DELAY-MS !  0 LAST-ERROR !  1 JOINS +!
        ." WLAN back: link and DHCP ready" CR
      ELSE
        DUP 0< IF FAILED ELSE  DROP  DUE? IF -302 FAILED THEN  THEN
      THEN
    ENDOF
    S-READY OF
      LINK-UP? 0= IF  WIFI-ERROR ?DUP 0= IF -433 THEN  FAILED  ELSE
        WLINK-BEACON-AGE 5000 > IF -428 FAILED THEN
      THEN
    ENDOF
  ENDCASE ;
: WORK ( -- )   \ the word in IDLE: one step; an error stops all and tries again later
  ENABLED @ 0= IF EXIT THEN
  ['] STEP CATCH  ?DUP 0= IF EXIT THEN
  DUP -28 = IF THROW THEN                                 \ Ctrl-C as at the console
  ['] FAILED CATCH ?DUP IF  NIP  ENABLED OFF!  GAVE-UP ON!  THROW  THEN ;      \ not even the stop works

PUBLIC
: WBACK-STEP ( -- )   WORK ;
: WBACK-START ( -- )
  ENABLED @ ABORT" WLAN-BACK: already started"
  LINK-AP? ABORT" WLAN-BACK: stop the setup AP first"
  WLAN-SAVED? 0= IF -431 THROW THEN
  OFFLINE LINK-STATION  S-WAIT BACK-STATE !  0 LAST-ERROR ! 0 ATTEMPTS ! 0 JOINS !  GAVE-UP OFF!
  1000 DELAY-MS !  MILLIS DUE !  ENABLED ON!  ['] WBACK-STEP IDLE+ ;
: WBACK-STOP ( -- )
  LINK-AP? ABORT" WLAN-BACK: the setup AP runs; SETUP-STOP ends it"
  ENABLED OFF!  ['] WBACK-STEP IDLE-  OFFLINE  S-WAIT BACK-STATE ! ;
: WBACK-READY? ( -- f )   BACK-STATE @ S-READY = ;
: WBACK-RUNNING? ( -- f )   ENABLED @ ;
: WBACK-GAVE-UP? ( -- f )   GAVE-UP @ ;
: .WBACK ( -- )
  ." WLAN back state/attempts/joins/code: " BACK-STATE @ . ATTEMPTS @ . JOINS @ . LAST-ERROR @ . CR ;
: WLAN-BOOT-ON ( -- )   ['] WBACK-START 'SETUP !  ['] QUIT 'LOOP !  TURNKEY ;
: WLAN-BOOT-OFF ( -- )   AUTORUN OFF!  ['] NOOP 'SETUP !  ['] NOOP 'LOOP ! ;
END-PACKAGE
