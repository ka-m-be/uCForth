\ snd.fs - six synthetic broadcast probes at most; no ACK or association
\
\ Load only after permission for channel 6, phy.fs, radio.fs, tester.fs
\ and lib/esp32/test/probe.fs. Use only on the owner's ESP32 board.
\ The source MAC is read from the factory eFuse, never from a received
\ frame. The test prints only results and counters; no frame contents.

ONLY FORTH  ALSO TEST-PROBE  ALSO ESP32-RADIO  ALSO ESP32-RADIO.I  ALSO TIMERS  DECIMAL
6 BUFFER: OWN-MAC
VARIABLE TX-TICKS
VARIABLE TX-ELAPSED
: TX-TICK ( -- )   1 TX-TICKS +! ;
: READ-MAC ( -- )
  $3FF5A004 @ $3FF5A008 @ OWN-MAC MAC-DECODE
  OWN-MAC C@ 1 AND ABORT" snd: factory MAC is a group address"
  0 6 0 DO  OWN-MAC I + C@ OR  LOOP  0= ABORT" snd: factory MAC is zero" ;
: SEND-PROBE ( -- )   OWN-MAC PROBE-BUILD FALSE RADIO-SEND ;
: SIX ( -- )
  6 0 DO  SEND-PROBE  100 MS  LOOP ;
: SIX-TIMED ( -- )
  0 TX-TICKS !  MILLIS >R  ['] TX-TICK 10 EVERY
  ['] SIX CATCH  ['] TX-TICK 0 EVERY  R> MILLIS SWAP - TX-ELAPSED !  THROW ;

READ-MAC
RADIO-START  6 RADIO-CHANNEL!  RADIO-BEACONS
T{ RADIO-CHANNEL@ -> 6 }T
T{ ' SIX-TIMED CATCH -> 0 }T
T{ #TX @ TX-RESULT @ TX-LAST-PMD @ -> 6 TX-COMPLETE 0 }T
T{ TX-CTRL @ TX-ACTIVE? TX-BLOCKED? -> 0 FALSE FALSE }T
T{ TX-ELAPSED @ 10 / TX-TICKS @ - ABS 3 < -> TRUE }T
T{ RX-CTRL @ RX-ON AND -> RX-ON }T
." snd: completed " #TX @ . ." frames; elapsed " TX-ELAPSED @ .
." ms; timer " TX-TICKS @ . ." ticks; interrupts " #INTS @ . CR
RADIO-STOP
S" snd" .RESULTS
