\ rmt.fs - package RMT: the remote control unit of the ESP32-S3, channel by channel
\
\ The RMT sends and receives pulses (Technical Reference Manual, chapter
\ 37). It is made for IR remote controls (package IR), and it also
\ drives LEDs like the WS2812 (package WS2812). It has 8 channels: 0 to 3
\ send, 4 to 7 receive. Each word takes the number of a channel, ch.
\
\ A pulse word has 32 bits: a time in bits 0-14 and its level in bit 15,
\ then a second time in bits 16-30 and its level in bit 31. A time of 0
\ ends the pulses. Times count in ticks of the channel: n x 25 ns
\ (RMT-DIV); with n = 40 a tick is 1 us, and a time is 32767 us at most.
\
\ The RAM of the RMT holds 384 pulse words in 8 blocks of 48; channel ch
\ has block ch. A channel can take more blocks (RMT-BLOCKS): n blocks
\ from its own on. The channels whose blocks it takes cannot work then.
\ The RAM takes only accesses of 32 bits (@ and !): DUMP shows 0 there.
\
\   RMT-ON                  the unit on; the clock of all channels is the
\                           crystal (40 MHz); a channel that runs goes on
\   RMT-HZ ( -- hz )        this clock, 40 000 000: a common driver takes
\                           its dividers from it (packages IR, WS2812)
\   RMT-SOURCE ( -- i )     the interrupt source of the RMT (package INTR)
\   ch n RMT-DIV            a tick of the channel is n x 25 ns (1 to 256)
\   ch n RMT-BLOCKS         the channel has n blocks of RAM (1 to 8-ch)
\   ch pin RMT-OUT          a sending channel drives the pin
\   pin ch RMT-IN           the level of the pin goes to a receiving channel
\   pin ch RMT-IN-INV       the same, inverted: 1 in the RAM while the pin is 0
\   ch level RMT-IDLE       sending: the level between the pulses
\   ch hz % RMT-CARRIER     sending: a carrier of hz with % high in each
\                           period, on level 1 only; hz 0: no carrier
\   ch hz RMT-DEMOD         receiving: take away a carrier of about hz (for
\                           a sensor without a demodulator); 0: no. After RMT-DIV
\   ch n RMT-FILTER         receiving: pulses shorter than n x 25 ns do not
\                           count (1 to 255); 0: all count
\   ch n RMT-END            receiving: the pulses end after n ticks without
\                           a change of the level (1 to 32767)
\   a n ch RMT-SEND         send n pulse words from a; it adds the 0 at the
\                           end and returns when all is out. More words than
\                           the RAM of the channel holds: it fills the RAM
\                           again, half by half, while the channel sends (so
\                           a half must last 1 ms or more). After 5 s
\                           without progress, or Ctrl-C: the channel stops
\   ch RMT-RX-ON            receiving: wait for the next pulses
\   ch RMT-RX-OFF           receiving: stop
\   ch RMT-RECEIVED ( -- n )   0: nothing yet; n: pulses came and ended, n
\                           pulse words in the RAM of the channel (RMT-RAM);
\                           -1: more than the RAM holds (the chip writes the
\                           RAM full, without an end mark at its end). After
\                           n or -1 the channel waits no more: RMT-RX-ON again
\   ch f RMT-RX-INT         receiving: the end of the pulses (and a full RAM)
\                           asks for an interrupt, source 40 (package INTR)
\   ch RMT-RAM ( -- a )     the RAM of a channel
\   t0 l0 t1 l1 PULSE ( -- x )   a pulse word from two times and levels
\   x PULSE> ( -- t0 l0 t1 l1 )
\
\ On the board, in ucf term:  #include lib/esp32/rmt.fs  (it chooses this file; needs GPIO)
\ Example: GPIO 2 is 1 for 100 us, then 0 for 200 us:
\
\   ALSO RMT
\   RMT-ON  1 40 RMT-DIV  1 0 RMT-IDLE  1 2 RMT-OUT
\   CREATE P  100 1 200 0 PULSE ,
\   P 1 1 RMT-SEND

PACKAGE RMT  ALSO CHIP  ALSO GPIO   \ lib/package.fs

$60016000 CONSTANT RMT-BASE
$600C0018 CONSTANT PERIP-CLK-EN0    \ bit 9: the clock of the RMT
$600C0020 CONSTANT PERIP-RST-EN0    \ bit 9: the RMT in reset
RMT-BASE $70 + CONSTANT INT-RAW     \ bits: 0-3 end of sending, 8-11 half sent,
RMT-BASE $78 + CONSTANT INT-ENA     \ 16-19 end of receiving, 20-23 errors of
RMT-BASE $7C + CONSTANT INT-CLR     \ receiving (channels 4-7)
RMT-BASE $C0 + CONSTANT SYS-CONF
81 CONSTANT SIGNAL0                 \ GPIO matrix: out of channel 0, into channel 4

: TX? ( ch -- f )   4 < ;
: ?CH ( ch -- ch )   DUP 0 8 WITHIN 0= ABORT" RMT: the channels are 0 to 7" ;
: ?TX-CH ( ch -- ch )   ?CH DUP TX? 0= ABORT" RMT: a sending channel is 0 to 3" ;
: ?RX-CH ( ch -- ch )   ?CH DUP TX? ABORT" RMT: a receiving channel is 4 to 7" ;
: CONF0 ( ch -- a )   DUP TX? IF CELLS RMT-BASE $20 + + ELSE 4 - 8 * RMT-BASE $30 + + THEN ;
: CONF1 ( ch -- a )   4 - 8 * RMT-BASE $34 + + ;      \ receiving channels
: STATUS ( ch -- a )  CELLS RMT-BASE $50 + + ;
: DUTY ( ch -- a )    CELLS RMT-BASE $80 + + ;        \ sending: the carrier
: THRESHOLDS ( ch -- a )  4 - CELLS RMT-BASE $90 + + ;   \ receiving: the demodulator
: TX-LIM ( ch -- a )  CELLS RMT-BASE $A0 + + ;
: BIT ( n -- mask )   1 SWAP LSHIFT ;
: UPDATE ( ch -- )   \ the channel takes its new settings
  DUP TX? IF  $1000000 SWAP CONF0 BIS!  ELSE  $8000 SWAP CONF1 BIS!  THEN ;
: FIELD! ( x mask ch -- )   DUP >R CONF0 BITS!  R> UPDATE ;
: BLOCKS@ ( ch -- n )   DUP CONF0 @  SWAP TX? IF 16 ELSE 24 THEN RSHIFT  $F AND ;
: STOP-TX ( ch -- )   $80 OVER CONF0 BIS!  UPDATE ;
: STOP-RX ( ch -- )   \ receiving off; the RAM belongs to the CPU
  $9 OVER CONF1 BIC!  UPDATE ;

PUBLIC
40000000 CONSTANT RMT-HZ            \ the clock of the RMT: the crystal, 40 MHz
40 CONSTANT RMT-SOURCE              \ the interrupt source of the RMT (package INTR)
: RMT-RAM ( ch -- a )   ?CH 192 * RMT-BASE $800 + + ;
PRIVATE

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
  R@ BLOCKS@ 48 * DUP CELLS RAM-START @ + RAM-END !      ( size )
  $6 R@ CONF0 BIS!                                      \ read the RAM from its start
  DUP FILL-RAM
  LEFT @ IF                                             \ more than the RAM holds: wrap mode
    2/ DUP HALF-N !  $1FF R@ TX-LIM BITS!               \ an event after each half
    $10 R@ CONF0 BIS!
  ELSE  DROP  $10 R@ CONF0 BIC!  THEN
  R@ BIT INT-CLR !  R@ 8 + BIT INT-CLR !                \ old events go
  R@ UPDATE  1 R@ CONF0 BIS!                            \ start
  BEGIN LEFT @ WHILE  R@ 8 + BIT WAIT-FOR  HALF-N @ FILL-RAM  REPEAT
  R> BIT WAIT-FOR ;

PUBLIC
: RMT-ON ( -- )
  $200 PERIP-CLK-EN0 BIS!  $200 PERIP-RST-EN0 BIC!
  $87000003 SYS-CONF ! ;   \ registers on; the crystal, divider 1; RAM clock on, RAM direct
: RMT-DIV ( ch n -- )
  $FF AND  OVER ?CH TX? IF 8 LSHIFT $FF00 ELSE $FF THEN  ROT FIELD! ;
: RMT-BLOCKS ( ch n -- )
  2DUP SWAP ?CH 9 SWAP -  1 SWAP WITHIN 0= ABORT" RMT: 1 to 8-ch blocks"
  OVER TX? IF 16 LSHIFT $F0000 ELSE 24 LSHIFT $F000000 THEN  ROT FIELD! ;
: RMT-OUT ( ch pin -- )   SWAP ?TX-CH SIGNAL0 + SWAP SIGNAL>PIN ;
: RMT-IN ( pin ch -- )   ?RX-CH 4 - SIGNAL0 + PIN>SIGNAL ;
: RMT-IN-INV ( pin ch -- )   \ GPIO_FUNCn_IN_SEL_CFG, bit 6: inverted
  DUP >R RMT-IN  $40  R> 4 - SIGNAL0 + CELLS GPIO-BASE IN-SEL + + BIS! ;
: RMT-IDLE ( ch level -- )   0<> $20 AND $40 OR  $60  ROT ?TX-CH FIELD! ;
: RMT-CARRIER ( ch hz % -- )
  ROT ?TX-CH >R
  OVER 0= IF  2DROP  0 $200000 R> FIELD!  EXIT  THEN
  SWAP RMT-HZ SWAP /                                     ( % period )
  TUCK * 100 /  TUCK -                                  ( high low )
  SWAP 16 LSHIFT OR  R@ DUTY !
  $700000 DUP R> FIELD! ;   \ carrier on, on level 1, only while sending
: RMT-DEMOD ( ch hz -- )
  SWAP ?RX-CH >R
  ?DUP 0= IF  0 $10000000 R> FIELD!  EXIT  THEN
  RMT-HZ SWAP /  R@ CONF0 @ $FF AND ?DUP 0= IF 256 THEN /     \ a period in ticks
  DUP 16 LSHIFT OR  R@ THRESHOLDS !
  $30000000 DUP R> FIELD! ;   \ demodulator on, the carrier is on level 1
: RMT-FILTER ( ch n -- )
  SWAP ?RX-CH >R  $FF AND  ?DUP IF 5 LSHIFT $10 OR ELSE 0 THEN
  $1FF0 R@ CONF1 BITS!  R> UPDATE ;
: RMT-END ( ch n -- )   $7FFF AND 8 LSHIFT  $7FFF00  ROT ?RX-CH FIELD! ;

: RMT-SEND ( a n ch -- )
  ?TX-CH >R  1+ LEFT !  SRC !
  R@ ['] (SEND) CATCH ?DUP IF  NIP  R> STOP-TX  THROW  THEN  R> DROP ;

: RMT-RX-ON ( ch -- )
  ?RX-CH >R  R@ STOP-RX                                  \ else a frame that runs ends at the start
  R@ 12 + BIT INT-CLR !  R@ 16 + BIT INT-CLR !           \ old ends and errors go
  $6 R@ CONF1 BIS!  $9 R@ CONF1 BIS!  R> UPDATE ;        \ from the start; the receiver owns the RAM; on
: RMT-RX-OFF ( ch -- )   ?RX-CH STOP-RX ;
PRIVATE
: ENDLESS? ( ch n -- f )   \ word n-1 has no end mark (a time 0): the frame went on
  1- CELLS SWAP RMT-RAM + @  DUP $7FFF AND 0<>  SWAP $7FFF0000 AND 0<> AND ;
PUBLIC
: RMT-RECEIVED ( ch -- n )
  ?RX-CH >R
  INT-RAW @ R@ 16 + BIT AND IF  R@ 16 + BIT INT-CLR !  R> STOP-RX  -1 EXIT  THEN
  INT-RAW @ R@ 12 + BIT AND 0= IF  R> DROP  0 EXIT  THEN
  R@ 12 + BIT INT-CLR !  R@ STOP-RX
  R@ STATUS @ $3FF AND  R@ 48 * -                        \ the place where the receiver writes next
  DUP R@ BLOCKS@ 48 * =  IF                              \ a full RAM: without an end mark at its end,
    R@ OVER ENDLESS? IF DROP -1 THEN                     \ the frame was longer (the S3 gives no error)
  THEN  R> DROP ;
: RMT-RX-INT ( ch f -- )
  SWAP ?RX-CH  DUP 12 + BIT SWAP 16 + BIT OR  SWAP IF INT-ENA BIS! ELSE INT-ENA BIC! THEN ;

: PULSE ( t0 l0 t1 l1 -- x )
  0<> $80000000 AND  SWAP $7FFF AND 16 LSHIFT OR
  SWAP 0<> $8000 AND OR  SWAP $7FFF AND OR ;
: PULSE> ( x -- t0 l0 t1 l1 )
  DUP $7FFF AND  OVER 15 RSHIFT 1 AND  ROT DUP 16 RSHIFT $7FFF AND  SWAP 31 RSHIFT ;

END-PACKAGE
