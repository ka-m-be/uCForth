\ rmt.fs - tests of the RMT, on the board (lib/esp32/esp32s3/rmt.fs, lib/esp32/ws2812.fs)
\
\ On the board, in ucf term, after the tester:
\   #include forth/test/tester.fs
\   #include lib/esp32/esp32s3/test/rmt.fs
\ No wires: channel 1 sends to GPIO 2, and the level of GPIO 2 goes back
\ into the chip, to channel 4 (the GPIO matrix). The RAM of the RMT takes
\ only 32-bit access (@ and !).

#require RMT ../../rmt.fs
#require WS2812 ../../ws2812.fs
ONLY FORTH DEFINITIONS  ALSO INTERNALS  ALSO GPIO  ALSO RMT  ALSO RMT.I  DECIMAL

: SETUP ( div -- )   \ channel 1 sends to GPIO 2 (ticks of 1 us); channel 4 hears GPIO 2 (ticks of div x 25 ns)
  RMT-ON  1 40 RMT-DIV  1 1 RMT-BLOCKS  1 0 RMT-IDLE  1 0 0 RMT-CARRIER  1 2 RMT-OUT
  2 4 RMT-IN  4 SWAP RMT-DIV  4 4 RMT-BLOCKS  4 0 RMT-FILTER  4 0 RMT-DEMOD  4 12000 RMT-END ;
: GOT ( i -- t0 l0 t1 l1 )   CELLS 4 RMT-RAM + @ PULSE> ;
: RECEIVE ( -- n )   \ the number of pulse words that came, 0 after 500 ms
  MILLIS 500 +  BEGIN  4 RMT-RECEIVED ?DUP IF NIP EXIT THEN  DUP MILLIS - 0< UNTIL  DROP 0 ;
: HALVES ( i -- t0 t1 )   GOT DROP NIP ;
: LEVELS ( i -- l0 l1 )   GOT NIP ROT DROP ;
: NEAR ( t t' -- f )   - ABS 3 < ;
: ABOUT ( t t' -- f )   - ABS 40 < ;
200 CELLS BUFFER: P                                 \ the pulse words to send
: P! ( t0 l0 t1 l1 i -- )   >R PULSE R> CELLS P + ! ;

\ ---- pulse words
T{ 100 1 200 0 PULSE -> $00C88064 }T
T{ $00C88064 PULSE> -> 100 1 200 0 }T
T{ 32767 0 5 TRUE PULSE PULSE> -> 32767 0 5 1 }T

\ ---- the settings of a channel
40 SETUP
T{ 1 CONF0 @ 8 RSHIFT $FF AND  1 BLOCKS@  4 CONF0 @ $FF AND  4 BLOCKS@ -> 40 1 40 4 }T
T{ 4 CONF0 @ 8 RSHIFT $7FFF AND  4 RMT-RAM -> 12000 $60016B00 }T
T{ 8 40 ' RMT-DIV CATCH NIP NIP -> -2 }T            \ no channel 8
T{ 1 8 ' RMT-BLOCKS CATCH NIP NIP -> -2 }T          \ channel 1 has 7 blocks at most
T{ 4 2 ' RMT-OUT CATCH NIP NIP -> -2 }T             \ channel 4 does not send
T{ 2 1 ' RMT-IN CATCH NIP NIP -> -2 }T              \ channel 1 does not receive

\ ---- send and receive
T{ 4 RMT-RX-ON  4 RMT-RECEIVED -> 0 }T
T{ 100 1 200 0  0 P!  300 1 400 0  1 P!  P 2 1 RMT-SEND  RECEIVE -> 2 }T
T{ 0 GOT  1 GOT -> 100 1 200 0  300 1 0 0 }T         \ the last dark time ends in the end
T{ 4 RMT-RECEIVED -> 0 }T                           \ until RMT-RX-ON
T{ 4 RMT-RX-ON  2 4 RMT-IN-INV  2 4 RMT-IN  4 RMT-RX-ON  P 2 1 RMT-SEND  RECEIVE  0 GOT -> 2 100 1 200 0 }T   \ nothing old
\ more than the RAM holds gives -1; RMT-RX-ON starts clean after it, and after pulses
\ at the end of a frame that nobody took with RMT-RECEIVED (Befund 15 of the ESP32)
: CPU-PULSES ( n -- )   \ n pulses at GPIO 2 from the CPU, 20 ms of rest; then GPIO 2 belongs to channel 1 again
  2 OUTPUT  0 ?DO  2 HIGH 2 LOW  LOOP  20 MS  1 2 RMT-OUT ;
T{ 4 RMT-RX-ON  1000 CPU-PULSES  4 RMT-RECEIVED  4 RMT-RX-ON  4 RMT-RECEIVED -> -1 0 }T
T{ 4 RMT-RX-ON  10 CPU-PULSES  10 CPU-PULSES  4 RMT-RX-ON  4 RMT-RECEIVED -> 0 }T

\ ---- more pulse words than the RAM of channel 1 holds (48): RMT-SEND fills it again
: MANY ( n -- )   0 ?DO  I 500 + 1 600 0  I P!  LOOP ;   \ 500 + i us light, 600 us dark
: ALL-NEAR? ( n -- f )   TRUE SWAP 0 ?DO  I GOT 2DROP DROP  I 500 + NEAR AND  LOOP ;
T{ 150 MANY  4 RMT-RX-ON  P 150 1 RMT-SEND  RECEIVE -> 150 }T
T{ 149 ALL-NEAR?  149 GOT -> TRUE 649 1 0 0 }T
T{ 0 HALVES NIP 600 NEAR  60 HALVES NIP 600 NEAR  120 HALVES NIP 600 NEAR -> TRUE TRUE TRUE }T
T{ 192 MANY  4 RMT-RX-ON  P 192 1 RMT-SEND  RECEIVE -> 192 }T   \ the RAM of channel 4 full: the end mark in its last half
T{ 193 MANY  4 RMT-RX-ON  P 193 1 RMT-SEND  RECEIVE -> -1 }T    \ longer than the RAM

\ ---- the carrier: 38 kHz, 33 % light; channel 4 measures it in ticks of 25 ns
1 SETUP  4 4000 RMT-END  1 38000 33 RMT-CARRIER
T{ 1 DUTY @ -> 347 16 LSHIFT 705 OR }T              \ 1052 x 25 ns: 38.0 kHz
T{ 4 RMT-RX-ON  200 1 100 0  0 P!  P 1 1 RMT-SEND  RECEIVE 7 10 WITHIN -> TRUE }T   \ 200 us: 7.6 periods
T{ 1 GOT  3 GOT -> 347 1 705 0  347 1 705 0 }T

\ ---- the demodulator takes the carrier away again (ticks of 1 us)
40 SETUP  1 38000 33 RMT-CARRIER  4 38000 RMT-DEMOD
T{ 4 THRESHOLDS @ -> 26 16 LSHIFT 26 OR }T          \ one period: 26 us
560 1 600 0  0 P!  560 1 1680 0  1 P!  560 1 0 0  2 P!
T{ 4 RMT-RX-ON  P 3 1 RMT-SEND  RECEIVE -> 3 }T
T{ 0 HALVES 600 ABOUT  SWAP 560 ABOUT  0 LEVELS -> TRUE TRUE 1 0 }T
T{ 1 HALVES 1680 ABOUT  SWAP 560 ABOUT  2 HALVES NIP -> TRUE TRUE 0 }T
T{ 1 0 0 RMT-CARRIER  4 0 RMT-DEMOD  1 CONF0 @ $200000 AND  4 CONF0 @ $10000000 AND -> 0 0 }T

\ ---- WS2812 on channel 0: 24 bits, then 50 us dark, into channel 4 (ticks of 0.1 us)
4 SETUP  ALSO WS2812  2 RGB-PIN
T{ 4 RMT-RX-ON  0 0 0 RGB!  RECEIVE -> 24 }T        \ 0 is 0.4 us light, 0.8 us dark
T{ 0 HALVES 8 NEAR  SWAP 4 NEAR  0 LEVELS -> TRUE TRUE 1 0 }T
PREVIOUS  2 INPUT

S" rmt" .RESULTS
