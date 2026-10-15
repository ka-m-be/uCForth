\ cores.fs - tests of the package MULTICORE on the ESP32, on the board (lib/esp32/multicore.fs)
\
\ On the board, in ucf term, after the tester and the package:
\   #include forth/test/tester.fs
\   #require MULTICORE lib/esp32/cores.fs
\   #include lib/esp32/esp32/test/cores.fs
\ With the base image (MULTICORE.I: TEMP-HEADS off). The words of core 1
\ write only variables; core 0 checks them. No wires.

ONLY FORTH DEFINITIONS  ALSO INTERNALS  ALSO FLASH  ALSO IDLE  ALSO INTR  ALSO TIMERS  ALSO MULTICORE  ALSO MULTICORE.I  DECIMAL
: CODE@ ( i -- n )   CELLS CODES + @ ;
: STOPPED ( i -- )   \ until the word of core i ends, at most 8 s
  MILLIS 8000 +  BEGIN  OVER CORE? 0=  OVER MILLIS - 0<  OR  UNTIL  2DROP ;

\ ---- the numbers; a word that ends; a counter, its stop
VARIABLE WHO   VARIABLE N
: WHO-AM-I ( -- )   CORE# WHO ! ;
: COUNTER ( -- )   BEGIN  1 N +!  AGAIN ;
T{ CORES CORE# -> 2 0 }T
T{ -1 WHO !  ' WHO-AM-I 1 CORE-RUN  1 STOPPED  WHO @  1 CODE@ -> 1 0 }T
T{ 0 N !  ' COUNTER 1 CORE-RUN  10 MS  N @ 0 >  1 CORE? -> TRUE TRUE }T
T{ N @  10 MS  N @ < -> TRUE }T
T{ 1 CORE-STOP  N @  10 MS  N @ =  1 CORE?  1 CODE@ -> TRUE FALSE -28 }T

\ ---- errors on core 1: THROW, an exception of the CPU
: THROWS ( -- )   -5 THROW ;
: FAULTS ( -- )   0 $10000000 ! ;
T{ ' THROWS 1 CORE-RUN  1 STOPPED  1 CODE@ -> -5 }T
T{ ' FAULTS 1 CORE-RUN  1 STOPPED  1 CODE@ -> -9 }T

\ ---- its clock: MILLIS as on core 0, MS waits as long
VARIABLE T-START   VARIABLE T-END
: WAITER ( -- )   MILLIS T-START !  100 MS  MILLIS T-END ! ;
T{ MILLIS  ' WAITER 1 CORE-RUN  1 STOPPED  T-START @ SWAP -  0 10 WITHIN  T-END @ T-START @ -  100 102 WITHIN -> TRUE TRUE }T

\ ---- both cores at once: the number output and CATCH with THROW (each has its own HLD, PAD, HANDLER)
VARIABLE ERRS0   VARIABLE ERRS1
: ROUND-TRIP? ( n -- f )   DUP 0 <# #S #>  NUMBER? DROP = ;   \ (SAME? is a word of INTERNALS)
: NUMBERS ( a -- )   0 OVER !  100000 0 DO  I ROUND-TRIP? 0= IF 1 OVER +! THEN  LOOP  DROP ;
: NUMBERS1 ( -- )   ERRS1 NUMBERS ;
T{ -1 ERRS1 !  ' NUMBERS1 1 CORE-RUN  ERRS0 NUMBERS  1 STOPPED  ERRS0 @ ERRS1 @ -> 0 0 }T
: THROWER ( n -- )   THROW ;
: CATCHES ( a -- )   0 OVER !  100000 0 DO  I 1+ ['] THROWER CATCH NIP  I 1+ <> IF 1 OVER +! THEN  LOOP  DROP ;
: CATCHES1 ( -- )   ERRS1 CATCHES ;
T{ -1 ERRS1 !  ' CATCHES1 1 CORE-RUN  ERRS0 CATCHES  1 STOPPED  ERRS0 @ ERRS1 @ -> 0 0 }T

\ ---- the flash while core 1 runs: it waits meanwhile; FREEZE refuses
CREATE FBUF 16 ALLOT
T{ 0 N !  ' COUNTER 1 CORE-RUN  10 MS  $1000 FBUF 16 FLASH-READ  FBUF C@  N @  10 MS  N @ < -> 0 $E9 TRUE }T
T{ ' FREEZE CATCH  'FLASH-HOLD @ 0<> -> -2 TRUE }T
T{ 1 CORE-STOP  'FLASH-HOLD @  'FLASH-FREE @ -> 0 0 }T

\ ---- the output of core 1: in its channel (core 0 writes it while it waits for a key)
CREATE HEARD-BUF 300 ALLOT          \ not PAD: S" at the prompt uses it
: HEARD ( -- a u )   \ what core 1 wrote, from its channel
  HEARD-BUF 0  BEGIN  1 OUT-OF CH-TAKE  WHILE  >R 2DUP + R> SWAP C!  1+  REPEAT ;
: TEXT= ( a u a2 u2 -- f )   \ the same bytes
  ROT OVER <> IF  2DROP DROP FALSE EXIT  THEN          ( a a2 u )
  0 ?DO  OVER I + C@  OVER I + C@ <> IF  2DROP FALSE UNLOOP EXIT  THEN  LOOP  2DROP TRUE ;
: SAYS ( -- )   ." core " CORE# . ;
T{ ' SAYS 1 CORE-RUN  1 STOPPED  HEARD S" core 1 " TEXT= -> TRUE }T

\ ---- a channel under load: 100 000 cells from core 1 to core 0, in order
64 CHANNEL PIPE   VARIABLE GOT   VARIABLE MISS
: PRODUCER ( -- )   100000 0 DO  BEGIN  I PIPE CH-PUT  UNTIL  LOOP ;
: CONSUMER ( -- )   \ at most 8 s
  0 GOT !  0 MISS !  MILLIS 8000 +
  BEGIN  PIPE CH-TAKE IF  GOT @ <> IF 1 MISS +! THEN  1 GOT +!  THEN
         GOT @ 100000 =  OVER MILLIS - 0<  OR  UNTIL  DROP ;
T{ ' PRODUCER 1 CORE-RUN  CONSUMER  1 STOPPED  GOT @ MISS @  1 CODE@ -> 100000 0 0 }T

\ ---- ON-CORE: words for core 1 in its chain of IDLE (CORE-SERVE); its chain, a timer, a handler
VARIABLE X   VARIABLE T1   VARIABLE B1   VARIABLE B0   VARIABLE H1
: MARK-IT ( -- )   7 X ! ;
: SAYS-LATER ( -- )   20 MS SAYS ;  \ after ON-CORE: it writes into the channel of core 1 while it waits
: TICK1 ( -- )   1 T1 +! ;
: ADD-TICK ( -- )   ['] TICK1 IDLE+ ;
: BEAT1 ( -- )   1 B1 +! ;
: BEAT0 ( -- )   1 B0 +! ;
: BEATS ( -- )   ['] BEAT1 10 EVERY ;
$3FF000E4 CONSTANT FROM-CPU2        \ DPORT_CPU_INTR_FROM_CPU_2_REG: source 26
: H26 ( -- )   0 FROM-CPU2 !  1 H1 +! ;
: HANDLES ( -- )   ['] H26 26 INT!  26 INT-ON  +INT ;
T{ ' CORE-SERVE 1 CORE-RUN  0 X !  ' MARK-IT 1 ON-CORE  10 MS  X @ -> 7 }T
T{ HEARD NIP  ' SAYS-LATER 1 ON-CORE  50 MS  HEARD S" core 1 " TEXT= -> 0 TRUE }T
T{ 0 T1 !  ' ADD-TICK 1 ON-CORE  100 MS  T1 @ 50 > -> TRUE }T
T{ 0 B1 !  0 B0 !  ' BEATS 1 ON-CORE  ' BEAT0 10 EVERY  205 MS  ' BEAT0 0 EVERY  B1 @ 18 23 WITHIN  B0 @ 18 23 WITHIN -> TRUE TRUE }T
T{ 0 H1 !  ' HANDLES 1 ON-CORE  1 FROM-CPU2 !  10 MS  1 FROM-CPU2 !  10 MS  H1 @ -> 2 }T
T{ 1 CORE-STOP  1 CODE@  HEARD NIP -> -28 0 }T

S" cores" .RESULTS

\ ---- measurements, no tests: the speed, and reads of the APB while core 0 reads DPORT
: SPIN ( -- ms )   MILLIS  1000000 0 DO LOOP  MILLIS SWAP - ;
.( 1000000 x DO LOOP on core 0, alone and with the counter on core 1: ) SPIN .
0 N !  ' COUNTER 1 CORE-RUN  SPIN .  .( ms; core 1 counted ) N @ .  1 CORE-STOP CR
VARIABLE BAD   VARIABLE READS
$3FF40014 CONSTANT UART-DIV         \ UART0_CLKDIV on the APB: the same value all the time
: APB-READS ( -- )   UART-DIV @  BEGIN  UART-DIV @ OVER <> IF 1 BAD +! THEN  1 READS +!  AGAIN ;
: DPORT-READS ( n -- )   0 ?DO  $3FF0002C @ DROP  LOOP ;   \ APPCPU_CTRL_A
0 BAD !  0 READS !  ' APB-READS 1 CORE-RUN  3000000 DPORT-READS  1 CORE-STOP
.( reads of the APB on core 1, wrong ones: ) READS @ .  BAD @ . CR
' CORE-SERVE 1 CORE-RUN  .( DO LOOP on core 0 while core 1 sleeps in CORE-SERVE: ) SPIN .  1 CORE-STOP CR
