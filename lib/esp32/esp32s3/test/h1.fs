\ h1.fs - the test of the two levels on the board: IR and the network in the background
\
\ Level 1, Forth handlers: our TCP/IP over NO-LINK, a step every 1 ms
\ (NET-START), the receiver of IR (IR-LISTEN), a timer word of the
\ application. Level 2, the chain of IDLE while the console waits: a
\ connection over 127.0.0.1 that sends bytes to an echo server and checks
\ each byte that comes back, and frames of the air conditioner (KELON168,
\ 343 times) out of GPIO 2 and back in, inside the chip (as
\ lib/esp32/esp32s3/test/ir.fs), each time compared with the time that
\ went out. No wires, no transceiver.
\
\   H1-START ( -- )      load, set up, start both levels
\   ms H1-BUSY ( -- )    the console computes for ms (level 2 rests)
\   H1-BAD ( -- )        a timer word of the application with an error
\   .H1 ( -- )           the counters
\   H1-END ( -- )        stop all; the tests
\
\ In ucf term (about 80 s; Ctrl-C from the terminal):
\   board ok> #include lib/esp32/esp32s3/test/h1.fs
\   board ok> H1-START                 wait 20 s at the prompt
\   board ok> 10000 H1-BUSY            the console computes 10 s
\   board ok> 60000 H1-BUSY            Ctrl-C after a few seconds: only this word stops
\   board ok>                          Ctrl-C a few times at the prompt (the chain runs)
\   board ok> H1-BAD                   its message comes; the rest goes on
\   board ok> .H1                      wait 20 s more
\   board ok> H1-END
\ Expected: "h1: 7 tests, 0 errors": no IR frame lost or wrong, no byte
\ lost or wrong, the connection stands, the timer word ran while the
\ console computed, and only the timer word with the error stopped.

ONLY FORTH
#require T{ ../../../../forth/test/tester.fs
ONLY FORTH
#require TEXT ../../../text.fs
ONLY FORTH
#require INET ../../../net/inet.fs
ONLY FORTH
#require NO-LINK ../../../net/nolink.fs
ONLY FORTH
#require ARP ../../../net/arp.fs
ONLY FORTH
#require IPV4 ../../../net/ipv4.fs
ONLY FORTH
#require UDP ../../../net/udp.fs
ONLY FORTH
#require TCP ../../../net/tcp.fs
ONLY FORTH
#require DHCP ../../../net/dhcp.fs
ONLY FORTH
#require LINK-NET ../../../net/linknet.fs
ONLY FORTH
#require NET ../../../net/net.fs
ONLY FORTH
#require NET-START ../../nettimer.fs
ONLY FORTH
#require RMT ../../rmt.fs
ONLY FORTH
#require IR ../../ir.fs
ONLY FORTH
#require IRCODE ../../../irremote/ircode.fs
ONLY FORTH
#require KELON168 ../../../irremote/kelon168.fs
ONLY FORTH DEFINITIONS  ALSO INTERNALS  ALSO INET  ALSO NET  ALSO NET-TIMER  ALSO TIMERS  ALSO TIMERS.I
ALSO IDLE  ALSO GPIO  ALSO RMT  ALSO IR  ALSO IR.I  ALSO KELON168  DECIMAL

\ ---- level 1: a timer word of the application
VARIABLE H-TICKS
: H1-TICK ( -- )   1 H-TICKS +! ;

\ ---- level 2: an echo server and a client over 127.0.0.1, port 7007
7007 CONSTANT H-PORT
VARIABLE H-SERVER   VARIABLE H-PEER   VARIABLE H-CLIENT
VARIABLE H-SENT   VARIABLE H-BACK   VARIABLE H-WRONG   VARIABLE H-STOP
256 BUFFER: H-SBUF   64 BUFFER: H-OUT   256 BUFFER: H-IN
: H1-SERVE ( -- )   \ take the connection; send back what came
  H-PEER @ 0= IF  H-SERVER @ TCP-ACCEPT IF H-PEER ! THEN  EXIT  THEN
  H-PEER @ SOCK-AVAILABLE 0= IF EXIT THEN
  H-SBUF 256 H-PEER @ SOCK-READ THROW  H-SBUF SWAP H-PEER @ SOCK-WRITE THROW ;
: PATTERN ( u -- c )   251 MOD ;
: H1-CLIENT ( -- )   \ 64 bytes out while less than 512 are on the way; check what comes back
  H-STOP @ 0=  H-SENT @ H-BACK @ - 512 <  AND IF
    64 0 DO  H-SENT @ I + PATTERN  H-OUT I + C!  LOOP
    H-OUT 64 H-CLIENT @ SOCK-WRITE THROW  64 H-SENT +!
  THEN
  H-CLIENT @ SOCK-AVAILABLE 0= IF EXIT THEN
  H-IN 256 H-CLIENT @ SOCK-READ THROW                    ( u )
  0 ?DO  H-IN I + C@  H-BACK @ PATTERN <> IF 1 H-WRONG +! THEN  1 H-BACK +!  LOOP ;

\ ---- IR: out and in at GPIO 2; level 2 sends, level 1 receives
VARIABLE H-FRAME   VARIABLE H-N                          \ the times of the frame
VARIABLE IR-OUT   VARIABLE IR-GOOD   VARIABLE IR-BAD   VARIABLE IR-LOST
VARIABLE IR-WAITING   VARIABLE IR-AT                     \ a frame is on its way; MILLIS when it went
: LOOPBACK ( -- )   \ as lib/esp32/esp32s3/test/ir.fs
  2 2 IR-PINS  TX 2 RMT-OUT  2 RX RMT-IN  RX 38000 RMT-DEMOD  TRUE IR-ECHO !  RX RMT-RX-ON ;
: SAME-TIMES? ( a n -- f )   \ n times as sent, each within 40 us
  DUP H-N @ <> IF 2DROP FALSE EXIT THEN
  TRUE SWAP 0 ?DO  OVER I CELLS + @  H-FRAME @ I CELLS + @  - ABS 40 < AND  LOOP  NIP ;
: H1-HEARD ( a n -- )   \ the handler of IR-LISTEN: level 1
  SAME-TIMES? IF 1 IR-GOOD +! ELSE 1 IR-BAD +! THEN  FALSE IR-WAITING ! ;
: H1-IR ( -- )   \ a frame every 500 ms; one that did not come back in 500 ms is lost
  IR-WAITING @ IF
    MILLIS IR-AT @ - 500 < IF EXIT THEN
    1 IR-LOST +!  FALSE IR-WAITING !
  THEN
  MILLIS IR-AT @ - 500 < IF EXIT THEN
  MILLIS IR-AT !  TRUE IR-WAITING !  1 IR-OUT +!
  H-FRAME @ H-N @ IR-SEND ;

\ ---- the console
VARIABLE BUSY-TICKS
: H1-BUSY ( ms -- )   \ compute: no KEY, so level 2 rests; the timer word goes on
  H-TICKS @ >R  MILLIS +  BEGIN DUP MILLIS - 0< UNTIL  DROP  H-TICKS @ R> - BUSY-TICKS ! ;
: H1-ERR ( -- )   1 0 / DROP ;
: H1-BAD ( -- )   ['] H1-ERR 100 EVERY ;

: .H1 ( -- )
  ." IR: " IR-OUT @ . ." out, " IR-GOOD @ . ." back, " IR-BAD @ . ." wrong, " IR-LOST @ . ." lost" CR
  ." TCP: " H-SENT @ . ." bytes out, " H-BACK @ . ." back, " H-WRONG @ . ." wrong" CR
  ." timer word: " H-TICKS @ . ." runs" CR ;
: H1-START ( -- )
  0 H-TICKS !  0 H-SENT !  0 H-BACK !  0 H-WRONG !  0 H-PEER !  0 BUSY-TICKS !  FALSE H-STOP !
  0 IR-OUT !  0 IR-GOOD !  0 IR-BAD !  0 IR-LOST !  FALSE IR-WAITING !  MILLIS 1000 - IR-AT !
  NET-START  ['] H1-TICK 10 EVERY
  H-PORT TCP-LISTEN THROW H-SERVER !
  S" 127.0.0.1" H-PORT TCP-OPEN THROW H-CLIENT !
  LOOPBACK  AC-RESET  TRUE AC-POWER!  HEAT AC-MODE!  23 AC-TEMP!  AC-TIMINGS H-N ! H-FRAME !
  ['] H1-HEARD IR-LISTEN
  ['] H1-SERVE IDLE+  ['] H1-CLIENT IDLE+  ['] H1-IR IDLE+ ;
VARIABLE OPEN-AT-END   VARIABLE ERR-GONE   VARIABLE TICK-STAYS
: H1-END ( -- )
  ['] H1-IR IDLE-  TRUE H-STOP !
  MILLIS 1000 +  BEGIN IDLE-RUN  DUP MILLIS - 0< UNTIL  DROP   \ the rest comes back
  ['] H1-CLIENT IDLE-  ['] H1-SERVE IDLE-
  IR-WAITING @ IF 1 IR-LOST +! THEN
  H-CLIENT @ SOCK-OPEN? OPEN-AT-END !
  ['] H1-ERR SLOT-OF 0= ERR-GONE !  ['] H1-TICK SLOT-OF 0<> TICK-STAYS !
  0 IR-LISTEN  ['] H1-TICK 0 EVERY  ['] H1-ERR 0 EVERY
  H-CLIENT @ SOCK-CLOSE DROP  H-PEER @ SOCK-CLOSE DROP  H-SERVER @ SOCK-CLOSE DROP
  NET-STOP  2 INPUT  .H1
  T{ IR-OUT @ 20 > -> <TRUE> }T                       \ frames went out
  T{ IR-GOOD @ IR-OUT @ = -> <TRUE> }T                \ each came back
  T{ IR-BAD @ IR-LOST @ -> 0 0 }T                     \ none wrong, none lost
  T{ H-BACK @ 10000 > H-BACK @ H-SENT @ = -> <TRUE> <TRUE> }T   \ bytes went through, all came back
  T{ H-WRONG @ OPEN-AT-END @ -> 0 <TRUE> }T           \ none wrong; the connection stands
  T{ BUSY-TICKS @ 900 > -> <TRUE> }T                  \ level 1 ran while the console computed
  T{ ERR-GONE @ TICK-STAYS @ -> <TRUE> <TRUE> }T      \ only the word with the error stopped
  S" h1" .RESULTS ;
