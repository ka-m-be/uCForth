\ ir.fs - tests of IR with the frames of KELON168, on the board (lib/esp32/ir.fs)
\
\ On the board, in ucf term, after the tester:
\   #include forth/test/tester.fs
\   #include lib/esp32/esp32s3/test/ir.fs
\ No wires, no transceiver: the sending channel drives GPIO 2, and GPIO 2
\ goes back into the receiving channel, not inverted, through the
\ demodulator of the RMT (a transceiver has its own). So a frame of the
\ air conditioner goes out and comes back inside the chip.

#require RMT ../../rmt.fs
#require IR ../../ir.fs
#require IRCODE ../../../irremote/ircode.fs
#require KELON168 ../../../irremote/kelon168.fs
ONLY FORTH DEFINITIONS  ALSO INTERNALS  ALSO GPIO  ALSO RMT  ALSO IR  ALSO IR.I  ALSO KELON168  DECIMAL

: LOOPBACK ( -- )   \ out and in at GPIO 2
  2 2 IR-PINS  TX 2 RMT-OUT  2 RX RMT-IN  RX 38000 RMT-DEMOD  TRUE IR-ECHO !  RX RMT-RX-ON ;
: SAME-BYTES? ( a1 a2 u -- f )
  0 ?DO  OVER I + C@  OVER I + C@ <> IF 2DROP UNLOOP FALSE EXIT THEN  LOOP  2DROP TRUE ;
: WAIT-FRAME ( -- a n true | false )   \ a frame, or false after 500 ms
  MILLIS 500 +  BEGIN  IR-FRAME? IF  ROT DROP TRUE EXIT  THEN  DUP MILLIS - 0< UNTIL  DROP FALSE ;
: PULSE@ ( i -- x )   CELLS PULSES + @ ;
CREATE T3  100 , 200 , 300 ,
CREATE T2  40000 , 100 ,
CREATE T0  100 , 0 , 200 ,

\ ---- times into pulse words
T{ T3 3 >PULSES  N-PULSES @  0 PULSE@  1 PULSE@ -> 2  100 1 200 0 PULSE  300 1 0 0 PULSE }T
T{ T2 2 >PULSES  N-PULSES @  0 PULSE@  1 PULSE@ -> 2  32767 1 7233 1 PULSE  100 0 0 0 PULSE }T
T{ T0 3 >PULSES  N-PULSES @  0 PULSE@ -> 1  100 1 200 1 PULSE }T   \ a time 0 goes away

\ ---- pulse words into times, also after an old end at the start
LOOPBACK  RX RMT-RX-OFF
0  RX RMT-RAM !  100 1 200 0 PULSE  RX RMT-RAM CELL+ !  300 1 0 0 PULSE  RX RMT-RAM 2 CELLS + !
T{ 3 >TIMES  #FRAME @  TIMES @  TIMES CELL+ @  TIMES 2 CELLS + @ -> 3 100 200 300 }T

\ ---- a frame of the air conditioner: out, back in, the same bytes
LOOPBACK  AC-RESET  TRUE AC-POWER!  HEAT AC-MODE!  23 AC-TEMP!
T{ AC-TIMINGS IR-SEND  WAIT-FRAME  ROT DROP -> 343 TRUE }T
T{ TIMES 343 AC-DECODE  SWAP AC-FRAME 21 SAME-BYTES? -> 168 TRUE }T
T{ TIMES @ 9000 - ABS 40 <  TIMES CELL+ @ 4600 - ABS 40 < -> TRUE TRUE }T   \ the header
VARIABLE SENT
: CLOSE? ( -- f )   \ each time that came back is near (40 us) to the time that went out
  AC-TIMINGS DROP SENT !
  TRUE  343 0 DO  I CELLS TIMES + @  I CELLS SENT @ + @  - ABS 40 < AND  LOOP ;
T{ CLOSE? -> TRUE }T

\ ---- IR-LISTEN: a Forth handler takes the frames
VARIABLE HEARD-N
: HEARD-IT ( a n -- )   HEARD-N !  DROP ;
T{ 0 HEARD-N !  ' HEARD-IT IR-LISTEN  AC-TIMINGS IR-SEND  50 MS  HEARD-N @ -> 343 }T
T{ 0 IR-LISTEN  0 HEARD-N !  AC-TIMINGS IR-SEND  50 MS  HEARD-N @  WAIT-FRAME NIP NIP -> 0 TRUE }T

\ ---- IR-ECHO false: the receiver does not listen while the board sends
T{ FALSE IR-ECHO !  AC-TIMINGS IR-SEND  WAIT-FRAME -> FALSE }T
2 INPUT

S" ir" .RESULTS
