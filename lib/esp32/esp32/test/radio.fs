\ radio.fs - the tests of ESP32-RADIO on the board (ESP32)
\
\ ESP32-PHY and ESP32-RADIO must be on the board: phy.fs and radio.fs
\ (lib/esp32/esp32/) loaded after a reset. On the board, in ucf term:
\   #include forth/test/tester.fs
\   #include lib/esp32/esp32/test/radio.fs
\ Expected: "radio: 25 tests, 0 errors". The tests switch the radio on
\ and off and set the channels; they check that the words keep their
\ rules, that the reception and the handler of the MAC are on while the
\ radio is on, and that the clocks and the timers of the system stay as
\ they were. Without a filter no frame comes; with the filter of a scan
\ (RADIO-BEACONS) the beacons come. So the test needs an access point
\ near the board, on the channel 1, 6 or 11. The list of the reception
\ has its test on the host: lib/esp32/test/radiohost.fs.

ONLY FORTH  ALSO ESP32-RADIO  ALSO ESP32-RADIO.I  ALSO CLOCK  ALSO INTR  ALSO TIMERS  ALSO CHIP  DECIMAL

: TIMED ( xt -- ms )   MILLIS >R  EXECUTE  MILLIS R> - ;
: US-AS-MS ( -- ms )   MILLIS  200000 US  MILLIS SWAP - ;   \ 200 ms by the CPU, by MILLIS
: CHANNELS ( -- )   14 1 DO  I RADIO-CHANNEL!  LOOP ;
: CLOCKS-OK? ( -- f )   \ the CPU at 240 MHz; US and MILLIS agree
  CPU-MHZ 240 =  US-AS-MS 199 202 WITHIN  AND ;
VARIABLE TICKS
: TICK ( -- )   1 TICKS +! ;
: TICKS-OK? ( -- f )   \ a word of a timer, every 10 ms, runs 50 times in 500 ms
  0 TICKS !  ['] TICK 10 EVERY  500 MS  ['] TICK 0 EVERY  TICKS @ 49 52 WITHIN ;
: ROUTED? ( -- f )   INTR-MAP MAC-SOURCE CELLS + @  1 = ;   \ the MAC goes to CPU interrupt 1

\ While the radio is off: no channel, no reception; RADIO-STOP does nothing.
T{ RADIO-STOP  RADIO-CHANNEL@ -> 0 }T
T{ 6 ' RADIO-CHANNEL! CATCH NIP -> -2 }T
T{ ' RADIO-RECEIVE CATCH -> -2 }T

\ The start: about 60 ms, then channel 1. The reception is on: the DMA
\ begins with the first descriptor, and the handler of the MAC waits.
T{ ' RADIO-START TIMED 1000 < -> TRUE }T
T{ RADIO-CHANNEL@ ON? -> 1 TRUE }T
T{ RX-CTRL @ RX-ON AND  RX-BASE @  RX-NEXT @ -> RX-ON 0 DESC 0 DESC }T
T{ MAC-SOURCE INT@  ROUTED? -> ' MAC-INT TRUE }T
T{ CLOCKS-OK?  TICKS-OK? -> TRUE TRUE }T
T{ RADIO-RECEIVE  RX-NEXT @  BEACONS? -> 0 0 DESC FALSE }T   \ no filter: no frame

\ The filter of a scan: the beacons come. Each frame is a beacon (the
\ type $80, at least the 36 bytes before its elements), with a level.
\ The handler counts the interrupts; a timer keeps its time meanwhile.
VARIABLE #B   VARIABLE #BAD
: BEACON-OK? ( a n -- f )   36 < IF  DROP FALSE EXIT  THEN  C@ $80 = ;
: GATHER ( ms -- )   \ RADIO-RECEIVE for ms: #B counts the frames, #BAD the wrong ones
  MILLIS +  BEGIN
    RADIO-RECEIVE ?DUP IF  BEACON-OK?  RADIO-LEVEL -100 -9 WITHIN AND  0= IF 1 #BAD +! THEN  1 #B +!  THEN
  DUP MILLIS - 0 <  UNTIL DROP ;
: SCAN3 ( -- ms )   \ the channels 1, 6 and 11, 700 ms each; the time it took
  0 #B !  0 #BAD !  MILLIS
  1 RADIO-CHANNEL! 700 GATHER  6 RADIO-CHANNEL! 700 GATHER  11 RADIO-CHANNEL! 700 GATHER  MILLIS SWAP - ;
T{ RADIO-BEACONS  BEACONS?  FILTER @ FILTER-BITS AND  CTRL-FILTER @ CTRL-TYPES AND -> TRUE SCAN-FILTER 0 }T
T{ 0 #INTS !  0 TICKS !  ' TICK 10 EVERY  SCAN3  ' TICK 0 EVERY  10 /  TICKS @ - ABS 3 < -> TRUE }T
T{ #B @ 0 >  #BAD @  #INTS @ 0 > -> TRUE 0 TRUE }T

\ The channels 1 to 13; 0 and 14 are errors and change nothing.
T{ ' CHANNELS TIMED 1000 < -> TRUE }T
T{ RADIO-CHANNEL@ -> 13 }T
T{ 0 ' RADIO-CHANNEL! CATCH NIP -> -2 }T
T{ 14 ' RADIO-CHANNEL! CATCH NIP -> -2 }T
T{ RADIO-CHANNEL@ -> 13 }T

\ Off: no reception, no handler. On again: the second start calibrates
\ again and makes a new list.
T{ RADIO-STOP  RADIO-CHANNEL@ ON? BEACONS? -> 0 FALSE FALSE }T
T{ RX-CTRL @ RX-ON AND  ROUTED? -> 0 FALSE }T
T{ ' RADIO-RECEIVE CATCH -> -2 }T
T{ ' RADIO-START TIMED 1000 < -> TRUE }T
T{ 6 RADIO-CHANNEL!  RADIO-CHANNEL@  RX-NEXT @ -> 6 0 DESC }T
T{ CLOCKS-OK?  TICKS-OK? -> TRUE TRUE }T
T{ MILLIS 200 MS MILLIS SWAP - 200 203 WITHIN -> TRUE }T
T{ RADIO-STOP -> }T

S" radio" .RESULTS
