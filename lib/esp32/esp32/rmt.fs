\ rmt.fs - package RMT: the remote control unit of the ESP32, channel by channel
\
\ The RMT sends and receives pulses (Technical Reference Manual of the
\ ESP32, chapter 15). The words are those of the package RMT of the
\ ESP32-S3 (lib/esp32/esp32s3/rmt.fs), so the packages IR and WS2812 and
\ an application use both in the same way. The differences: the ESP32 has
\ 8 channels, and each one sends or receives; a block of RAM has 64 pulse
\ words; the clock of the channels is the APB clock (RMT-HZ: 80 MHz with
\ the PLL, 40 MHz without); there is no demodulator.
\
\ A pulse word has 32 bits: a time in bits 0-14 and its level in bit 15,
\ then a second time in bits 16-30 and its level in bit 31. A time of 0
\ ends the pulses. Times count in ticks of the channel: n periods of
\ RMT-HZ (RMT-DIV); with n = RMT-HZ / 1000000 a tick is 1 us.
\
\ The RAM of the RMT holds 512 pulse words in 8 blocks of 64; channel ch
\ has block ch. A channel can take more blocks (RMT-BLOCKS): n blocks
\ from its own on. The channels whose blocks it takes cannot work then.
\ The RAM takes only accesses of 32 bits (@ and !).
\
\   RMT-ON                  the unit on; a channel that runs goes on
\   RMT-HZ ( -- hz )        the clock of the channels: the APB clock
\   RMT-SOURCE ( -- i )     the interrupt source of the RMT, 47 (package INTR)
\   ch n RMT-DIV            a tick of the channel is n periods of RMT-HZ (1 to 256)
\   ch n RMT-BLOCKS         the channel has n blocks of RAM (1 to 8-ch)
\   ch pin RMT-OUT          a sending channel drives the pin
\   pin ch RMT-IN           the level of the pin goes to a receiving channel
\   pin ch RMT-IN-INV       the same, inverted: 1 in the RAM while the pin is 0
\   ch level RMT-IDLE       sending: the level between the pulses
\   ch hz % RMT-CARRIER     sending: a carrier of hz with % high in each
\                           period, on level 1 only; hz 0: no carrier
\   ch hz RMT-DEMOD         receiving: 0 only (the ESP32 has no demodulator)
\   ch n RMT-FILTER         receiving: pulses shorter than n periods of
\                           RMT-HZ do not count (1 to 255); 0: all count
\   ch n RMT-END            receiving: the pulses end after n ticks without
\                           a change of the level (1 to 65535)
\   a n ch RMT-SEND         send n pulse words from a; it adds the 0 at the
\                           end and returns when all is out. More words than
\                           the RAM of the channel holds: it fills the RAM
\                           again, half by half, while the channel sends.
\                           After 5 s without progress, or Ctrl-C: the
\                           channel stops
\   ch RMT-RX-ON            receiving: wait for the next pulses. A frame
\                           that has not ended goes on: the receiver of the
\                           ESP32 keeps its state until RMT-END ticks pass
\                           without a change. It also ends an error of the
\                           channel: a full RAM, or pulses after an end
\                           that RMT-RECEIVED did not take
\   ch RMT-RX-OFF           receiving: stop
\   ch RMT-RECEIVED ( -- n )   0: nothing yet; n: pulses came and ended, n
\                           pulse words in the RAM of the channel (RMT-RAM);
\                           -1: more than the RAM holds. After n or -1 the
\                           channel waits no more: RMT-RX-ON again
\   ch f RMT-RX-INT         receiving: the end of the pulses (and a full RAM)
\                           asks for an interrupt, source 47 (package INTR)
\   ch RMT-RAM ( -- a )     the RAM of a channel
\   t0 l0 t1 l1 PULSE ( -- x )   a pulse word from two times and levels
\   x PULSE> ( -- t0 l0 t1 l1 )
\
\ On the board, in ucf term:  #include lib/esp32/rmt.fs  (it chooses this file; needs GPIO)

PACKAGE RMT  ALSO CHIP  ALSO GPIO   \ lib/package.fs

$3FF56000 CONSTANT RMT-BASE
$3FF000C0 CONSTANT PERIP-CLK-EN     \ DPORT_PERIP_CLK_EN: bit 9, the clock of the RMT
$3FF000C4 CONSTANT PERIP-RST-EN     \ DPORT_PERIP_RST_EN: bit 9, the RMT in reset
RMT-BASE $A0 + CONSTANT INT-RAW     \ bits: 3 ch the end of sending, 3 ch + 1 the end
RMT-BASE $A8 + CONSTANT INT-ENA     \ of receiving, 3 ch + 2 an error, 24 + ch a part sent
RMT-BASE $AC + CONSTANT INT-CLR
RMT-BASE $F0 + CONSTANT APB-CONF    \ bit 0: the CPU reaches the RAM; bit 1: sending wraps
87 CONSTANT OUT0                    \ GPIO matrix: out of channel 0
83 CONSTANT IN0                     \ into channel 0

: ?CH ( ch -- ch )   DUP 0 8 WITHIN 0= ABORT" RMT: the channels are 0 to 7" ;
: CONF0 ( ch -- a )   8 * RMT-BASE $20 + + ;
: CONF1 ( ch -- a )   8 * RMT-BASE $24 + + ;
: DUTY ( ch -- a )    CELLS RMT-BASE $B0 + + ;        \ sending: the carrier
: TX-LIM ( ch -- a )  CELLS RMT-BASE $D0 + + ;
: BIT ( n -- mask )   1 SWAP LSHIFT ;
: BLOCKS@ ( ch -- n )   CONF0 @ 24 RSHIFT $F AND ;

PUBLIC
: RMT-HZ ( -- hz )   APB-MHZ 1000000 * ;
47 CONSTANT RMT-SOURCE
: RMT-RAM ( ch -- a )   ?CH 256 * RMT-BASE $800 + + ;
PRIVATE
: STOP-TX ( ch -- )   \ as ESP-IDF: the first word 0, TX_START off, the read pointer back
  0 OVER RMT-RAM !  1 OVER CONF1 BIC!  8 SWAP CONF1 BIS! ;
: STOP-RX ( ch -- )   \ receiving off; the RAM belongs to the CPU
  $22 SWAP CONF1 BIC! ;

\ ---- RMT-SEND: the pulse words, then a 0, into the RAM of the channel
VARIABLE SRC                        \ the next pulse word to go
VARIABLE LEFT                       \ the words still to go, with the 0 at the end
VARIABLE AT                         \ the next place in the RAM
VARIABLE RAM-START   VARIABLE RAM-END   VARIABLE HALF-N
: NEXT-WORD ( -- x )   LEFT @ 1 > IF  SRC @ @  CELL SRC +!  ELSE 0 THEN  -1 LEFT +! ;
: FILL-RAM ( k -- )   \ k words to the RAM from AT on; after its end AT starts again
  0 ?DO
    LEFT @ 0= IF LEAVE THEN
    NEXT-WORD AT @ !  CELL AT +!  AT @ RAM-END @ = IF RAM-START @ AT ! THEN
  LOOP ;
: WAIT-FOR ( mask -- )   \ until the bit comes in INT-RAW; then clear it
  MILLIS 5000 + SWAP
  BEGIN DUP INT-RAW @ AND 0= WHILE
    OVER MILLIS - 0< ABORT" RMT: the channel does not send"
  REPEAT  INT-CLR !  DROP ;
: (SEND) ( ch -- )
  >R  R@ RMT-RAM DUP RAM-START ! AT !
  R@ BLOCKS@ 64 * DUP CELLS RAM-START @ + RAM-END !      ( size )
  $20 R@ CONF1 BIC!                                     \ the RAM belongs to the sender
  8 R@ CONF1 BIS!  8 R@ CONF1 BIC!                       \ read the RAM from its start
  DUP FILL-RAM
  LEFT @ IF                                             \ more than the RAM holds: wrap
    2/ DUP HALF-N !  $1FF R@ TX-LIM BITS!               \ an event after each half
    2 APB-CONF BIS!
  ELSE  DROP  2 APB-CONF BIC!  THEN
  R@ 3 * BIT INT-CLR !  R@ 24 + BIT INT-CLR !          \ old events go
  1 R@ CONF1 BIS!                                       \ start
  BEGIN LEFT @ WHILE  R@ 24 + BIT WAIT-FOR  HALF-N @ FILL-RAM  REPEAT
  R> 3 * BIT WAIT-FOR ;

PUBLIC
: RMT-ON ( -- )
  $200 PERIP-CLK-EN BIS!  $200 PERIP-RST-EN BIC!
  $80000000 0 CONF0 BIS!  $40000000 0 CONF0 BIC!        \ channel 0, for all: CLK_EN on, MEM_PD off
  1 APB-CONF BIS! ;                                     \ the CPU reaches the RAM directly
: RMT-DIV ( ch n -- )   $FF AND  $FF ROT ?CH CONF0 BITS! ;
: RMT-BLOCKS ( ch n -- )
  2DUP SWAP ?CH 9 SWAP -  1 SWAP WITHIN 0= ABORT" RMT: 1 to 8-ch blocks"
  24 LSHIFT  $F000000  ROT CONF0 BITS! ;
: RMT-OUT ( ch pin -- )   \ the clock of the channel: the APB clock (REF_ALWAYS_ON)
  OVER ?CH  $20000 SWAP CONF1 BIS!
  SWAP OUT0 + SWAP SIGNAL>PIN ;
: RMT-IN ( pin ch -- )
  ?CH  $20000 OVER CONF1 BIS!
  IN0 + PIN>SIGNAL ;
: RMT-IN-INV ( pin ch -- )   \ GPIO_FUNCn_IN_SEL_CFG, bit 6: inverted
  DUP >R RMT-IN  $40  R> IN0 + CELLS GPIO-BASE IN-SEL + + BIS! ;
: RMT-IDLE ( ch level -- )   0<> $40000 AND $80000 OR  $C0000  ROT ?CH CONF1 BITS! ;
: RMT-CARRIER ( ch hz % -- )
  ROT ?CH >R
  OVER 0= IF  2DROP  $10000000 R> CONF0 BIC!  EXIT  THEN
  SWAP RMT-HZ SWAP /                                    ( % period )
  TUCK * 100 /  TUCK -                                  ( high low )
  SWAP 16 LSHIFT OR  R@ DUTY !
  $30000000 R> CONF0 BIS! ;   \ carrier on, on level 1
: RMT-DEMOD ( ch hz -- )
  NIP ABORT" RMT: the ESP32 has no demodulator (hz must be 0)" ;
: RMT-FILTER ( ch n -- )
  SWAP ?CH >R  $FF AND  ?DUP IF 8 LSHIFT $80 OR ELSE 0 THEN
  $FF80 R> CONF1 BITS! ;
: RMT-END ( ch n -- )   $FFFF AND 8 LSHIFT  $FFFF00  ROT ?CH CONF0 BITS! ;

: RMT-SEND ( a n ch -- )
  ?CH >R  1+ LEFT !  SRC !
  R@ ['] (SEND) CATCH ?DUP IF  NIP  R> STOP-TX  THROW  THEN  R> DROP ;

: RMT-RX-ON ( ch -- )   \ as ESP-IDF after an error: both pointers back; the events go last
  ?CH >R  R@ STOP-RX                                     \ else a frame that runs ends at the start
  $C R@ CONF1 BIS!  $C R@ CONF1 BIC!                     \ write (and read) the RAM from its start
  $22 R@ CONF1 BIS!                                      \ the receiver owns the RAM; on
  R@ 3 * 1+ BIT INT-CLR !  R> 3 * 2 + BIT INT-CLR ! ;    \ old ends and errors go: after an error the
                                                         \ start gives the error again
: RMT-RX-OFF ( ch -- )   ?CH STOP-RX ;
PRIVATE
: COUNTED ( ch -- n )   \ the pulse words up to the end mark (a time 0), as ESP-IDF counts them
  DUP RMT-RAM  SWAP BLOCKS@ 64 *  DUP >R  0 DO                ( a )  ( R: size )
    DUP I CELLS + @  DUP $7FFF AND 0=  SWAP 16 RSHIFT $7FFF AND 0= OR
    IF  DROP I 1+  R> DROP UNLOOP EXIT  THEN
  LOOP  DROP R> ;
PUBLIC
: RMT-RECEIVED ( ch -- n )
  ?CH >R
  INT-RAW @ R@ 3 * 2 + BIT AND IF  R@ 3 * 2 + BIT INT-CLR !  R> STOP-RX  -1 EXIT  THEN
  INT-RAW @ R@ 3 * 1+ BIT AND 0= IF  R> DROP  0 EXIT  THEN
  R@ 3 * 1+ BIT INT-CLR !  R@ STOP-RX
  R> COUNTED ;
: RMT-RX-INT ( ch f -- )
  SWAP ?CH  3 * 1+ BIT  DUP 2* OR  SWAP IF INT-ENA BIS! ELSE INT-ENA BIC! THEN ;

: PULSE ( t0 l0 t1 l1 -- x )
  0<> $80000000 AND  SWAP $7FFF AND 16 LSHIFT OR
  SWAP 0<> $8000 AND OR  SWAP $7FFF AND OR ;
: PULSE> ( x -- t0 l0 t1 l1 )
  DUP $7FFF AND  OVER 15 RSHIFT 1 AND  ROT DUP 16 RSHIFT $7FFF AND  SWAP 31 RSHIFT ;

END-PACKAGE
