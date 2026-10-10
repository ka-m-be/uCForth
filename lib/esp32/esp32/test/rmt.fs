\ rmt.fs - tests of the RMT of the ESP32, on the board (lib/esp32/esp32/rmt.fs, lib/esp32/ws2812.fs)
\
\ On the board, in ucf term, after the tester:
\   #include forth/test/tester.fs
\   #include lib/esp32/esp32/test/rmt.fs
\ No wires: channel 1 sends to GPIO 25, and the level of GPIO 25 goes
\ back into the chip, to channel 4 (the GPIO matrix). The RAM of the RMT
\ takes only 32-bit access (@ and !). The CPU runs at 240 MHz: RMT-HZ is
\ 80 MHz.

#require RMT ../../rmt.fs
#require WS2812 ../../ws2812.fs
ONLY FORTH DEFINITIONS  ALSO INTERNALS  ALSO GPIO  ALSO RMT  ALSO RMT.I  DECIMAL

25 CONSTANT P
: US-DIV ( -- n )   RMT-HZ 1000000 / ;
: SETUP ( div -- )   \ channel 1 sends to P (ticks of 1 us); channel 4 hears P (ticks of div periods of RMT-HZ)
  RMT-ON  1 US-DIV RMT-DIV  1 1 RMT-BLOCKS  1 0 RMT-IDLE  1 0 0 RMT-CARRIER  1 P RMT-OUT
  P 4 RMT-IN  4 SWAP RMT-DIV  4 4 RMT-BLOCKS  4 0 RMT-FILTER  4 0 RMT-DEMOD  4 12000 RMT-END ;
: GOT ( i -- t0 l0 t1 l1 )   CELLS 4 RMT-RAM + @ PULSE> ;
: RECEIVE ( -- n )   \ the number of pulse words that came, 0 after 500 ms
  MILLIS 500 +  BEGIN  4 RMT-RECEIVED ?DUP IF NIP EXIT THEN  DUP MILLIS - 0< UNTIL  DROP 0 ;
: HALVES ( i -- t0 t1 )   GOT DROP NIP ;
: LEVELS ( i -- l0 l1 )   GOT NIP ROT DROP ;
: NEAR ( t t' -- f )   - ABS 3 < ;
200 CELLS BUFFER: PW                                \ the pulse words to send
: P! ( t0 l0 t1 l1 i -- )   >R PULSE R> CELLS PW + ! ;

\ ---- pulse words
T{ 100 1 200 0 PULSE -> $00C88064 }T
T{ $00C88064 PULSE> -> 100 1 200 0 }T
T{ 32767 0 5 TRUE PULSE PULSE> -> 32767 0 5 1 }T

\ ---- the settings of a channel
T{ RMT-HZ -> 80000000 }T
US-DIV SETUP
T{ 1 CONF0 @ $FF AND  1 BLOCKS@  4 CONF0 @ $FF AND  4 BLOCKS@ -> 80 1 80 4 }T
T{ 4 CONF0 @ 8 RSHIFT $FFFF AND  4 RMT-RAM -> 12000 $3FF56C00 }T
T{ 8 40 ' RMT-DIV CATCH NIP NIP -> -2 }T            \ no channel 8
T{ 1 8 ' RMT-BLOCKS CATCH NIP NIP -> -2 }T          \ channel 1 has 7 blocks at most
T{ 4 38000 ' RMT-DEMOD CATCH NIP NIP -> -2 }T       \ the ESP32 has no demodulator

\ ---- send and receive
T{ 4 RMT-RX-ON  4 RMT-RECEIVED -> 0 }T
T{ 100 1 200 0  0 P!  300 1 400 0  1 P!  PW 2 1 RMT-SEND  RECEIVE -> 2 }T
T{ 0 GOT  1 GOT -> 100 1 200 0  300 1 0 0 }T         \ the last dark time ends in the end
T{ 4 RMT-RECEIVED -> 0 }T                           \ until RMT-RX-ON
\ nothing old: the ESP32 ends a frame only after RMT-END ticks without a change (12 ms)
T{ 4 RMT-RX-ON  P 4 RMT-IN-INV  P 4 RMT-IN  20 MS  4 RMT-RX-ON  PW 2 1 RMT-SEND  RECEIVE  0 GOT -> 2 100 1 200 0 }T
\ RMT-RX-ON starts clean after an error: a full RAM (-1), or pulses after the end of a
\ frame that nobody took with RMT-RECEIVED (the channel no longer owns its RAM then)
: CPU-PULSES ( n -- )   \ n pulses at P from the CPU, 20 ms of rest; then P belongs to channel 1 again
  P OUTPUT  0 ?DO  P HIGH P LOW  LOOP  20 MS  1 P RMT-OUT ;
T{ 4 RMT-RX-ON  1000 CPU-PULSES  4 RMT-RECEIVED  4 RMT-RX-ON  4 RMT-RECEIVED -> -1 0 }T
T{ 4 RMT-RX-ON  10 CPU-PULSES  10 CPU-PULSES  4 RMT-RX-ON  4 RMT-RECEIVED -> 0 }T

\ ---- more pulse words than the RAM of channel 1 holds (64): RMT-SEND fills it again
: MANY ( n -- )   0 ?DO  I 500 + 1 600 0  I P!  LOOP ;   \ 500 + i us light, 600 us dark
: ALL-NEAR? ( n -- f )   TRUE SWAP 0 ?DO  I GOT 2DROP DROP  I 500 + NEAR AND  LOOP ;
T{ 150 MANY  4 RMT-RX-ON  PW 150 1 RMT-SEND  RECEIVE -> 150 }T
T{ 149 ALL-NEAR?  149 GOT -> TRUE 649 1 0 0 }T
T{ 0 HALVES NIP 600 NEAR  60 HALVES NIP 600 NEAR  120 HALVES NIP 600 NEAR -> TRUE TRUE TRUE }T

\ ---- the carrier: 38 kHz, 33 % light; channel 4 measures it in periods of RMT-HZ (12.5 ns)
1 SETUP  4 4000 RMT-END  1 38000 33 RMT-CARRIER
T{ 1 DUTY @ -> 694 16 LSHIFT 1411 OR }T             \ 2105 x 12.5 ns: 38.0 kHz
T{ 4 RMT-RX-ON  200 1 100 0  0 P!  PW 1 1 RMT-SEND  RECEIVE 7 10 WITHIN -> TRUE }T   \ 200 us: 7.6 periods
T{ 1 GOT  3 GOT -> 694 1 1411 0  694 1 1411 0 }T
T{ 1 0 0 RMT-CARRIER  1 CONF0 @ $10000000 AND -> 0 }T

\ ---- WS2812 on channel 0: 24 bits, then 50 us dark, into channel 4 (ticks of 0.1 us)
RMT-HZ 10000000 / SETUP  ALSO WS2812  P RGB-PIN
T{ 4 RMT-RX-ON  0 0 0 RGB!  RECEIVE -> 24 }T        \ 0 is 0.4 us light, 0.8 us dark
T{ 0 HALVES 8 NEAR  SWAP 4 NEAR  0 LEVELS -> TRUE TRUE 1 0 }T
PREVIOUS  P INPUT

S" rmt" .RESULTS
