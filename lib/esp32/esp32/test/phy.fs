\ phy.fs - the tests of ESP32-PHY on the board (ESP32)
\
\ ESP32-PHY (lib/esp32/esp32/phy.fs) must be on the board: loaded after
\ a reset, or in the image after FREEZE. On the board, in ucf term:
\   #include forth/test/tester.fs
\   #include lib/esp32/esp32/test/phy.fs
\ Expected: "phy: 13 tests, 0 errors". The tests calibrate the radio
\ (about 60 ms), set the channels 1 to 13, adjust the power and switch
\ the radio off and on. The radio sends and receives nothing yet; so the
\ tests check that the code of C runs and comes back, what it writes,
\ and that the clocks of the system stay as they were: US counts the
\ cycles of the CPU, MILLIS a timer at the APB clock. (Whether both
\ still count the real time, only a clock outside the board shows:
\ archive/plan/002_Plan_Fassung73.md, section 54.)

ONLY FORTH  ALSO ESP32-PHY  ALSO ESP32-PHY.I  ALSO CLOCK  DECIMAL

: NOT-ZERO ( a u -- n )   \ the bytes at a that are not 0
  0 SWAP 0 ?DO  OVER I + C@ 0<> -  LOOP  NIP ;
: SAME-BYTES? ( a1 a2 u -- f )   \ the u bytes at a1 and a2 are the same
  0 ?DO  OVER I + C@  OVER I + C@ <> IF  2DROP FALSE UNLOOP EXIT  THEN  LOOP  2DROP TRUE ;
: TIMED ( xt -- ms )   MILLIS >R  EXECUTE  MILLIS R> - ;
: US-AS-MS ( -- ms )   MILLIS  200000 US  MILLIS SWAP - ;   \ 200 ms by the CPU, by MILLIS
: CHANNELS ( -- )   14 1 DO  I PHY-CHANNEL!  LOOP ;
: TRACKS ( -- )   100 0 DO  PHY-TRACK  LOOP ;

\ The calibration: 1 if CAL-DATA held no valid data, else 0; it fills
\ CAL-DATA and changes the data of C. A second one finds valid data.
T{ PHY-INIT 2 U< -> TRUE }T
T{ CAL-DATA 1904 NOT-ZERO 100 > -> TRUE }T
T{ START-VALUES C-DATA #C-DATA SAME-BYTES? -> FALSE }T
T{ PHY-INIT -> 0 }T
T{ ' PHY-INIT TIMED NIP 1000 < -> TRUE }T

\ The clocks: the CPU at 240 MHz, US and MILLIS agree.
T{ CPU-MHZ -> 240 }T
T{ US-AS-MS 199 202 WITHIN -> TRUE }T
T{ MILLIS 200 MS MILLIS SWAP - 200 203 WITHIN -> TRUE }T

\ The channels 1 to 13, the power, off and on.
T{ ' CHANNELS TIMED 1000 < -> TRUE }T
T{ ' TRACKS TIMED 5000 < -> TRUE }T
T{ PHY-OFF  PHY-ON  6 PHY-CHANNEL! -> }T
T{ US-AS-MS 199 202 WITHIN -> TRUE }T
T{ CPU-MHZ -> 240 }T

S" phy" .RESULTS
