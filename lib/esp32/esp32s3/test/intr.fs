\ intr.fs - tests of the package INTR, on the board (lib/esp32/intr.fs)
\
\ On the board, in ucf term, after the tester:
\   #include forth/test/tester.fs
\   #include lib/esp32/esp32s3/test/intr.fs
\ The image must have INTR (targets/esp32s3/README.md, "Build"). The
\ sources need no wires: FROM_CPU 0 (source 79) asks while a register of
\ the CPU is 1; timer 0 of timer group 0 (source 50) gives an alarm
\ every n us.

ONLY FORTH DEFINITIONS  ALSO INTERNALS  ALSO ROM  ALSO INTR  ALSO INTR.I  DECIMAL

\ ---- exceptions of the CPU: THROW, and the VM goes on
T{ $10000000 ' @ CATCH NIP -> -9 }T               \ LoadProhibited
T{ 0 $10000000 ' ! CATCH NIP NIP -> -9 }T          \ StoreProhibited
T{ $50000000 ' EXECUTE CATCH NIP -> -9 }T          \ an xt without memory
: IN-ROM ( -- )   0 $10000000 4  3 $40000A20 SYSCALL DROP ;   \ esp_rom_spiflash_read to it
T{ ' IN-ROM CATCH -> -9 }T                         \ in a function of the ROM
T{ 0 $40001A40 SYSCALL 0 > -> TRUE }T              \ the ROM again: ets_get_cpu_frequency
T{ 1 2 + -> 3 }T

\ ---- a handler for FROM_CPU 0
$600C0030 CONSTANT FROM-CPU0        \ SYSTEM_CPU_INTR_FROM_CPU_0_REG: 1, and source 79 asks
VARIABLE HITS
: H0 ( -- )   0 FROM-CPU0 !  1 HITS +! ;
: ASK ( -- )   1 FROM-CPU0 !  10 US ;
' H0 79 INT!  79 INT-ON  +INT
T{ 79 INT@ -> ' H0 }T
T{ 0 HITS !  ASK  HITS @ -> 1 }T
T{ 0 HITS !  ASK ASK ASK  HITS @ -> 3 }T
T{ 0 HITS !  1 FROM-CPU0 ! 2 3 4  10 US  HITS @ -> 2 3 4 1 }T   \ the stacks stay

\ ---- off: a source, all handlers
T{ 79 INT-OFF  0 HITS !  ASK  0 FROM-CPU0 !  HITS @ -> 0 }T
T{ 79 INT-ON  0 HITS !  ASK  HITS @ -> 1 }T
T{ -INT  0 HITS !  ASK  HITS @ -> 0 }T
T{ +INT  10 US  HITS @ -> 1 }T                      \ the source asked all the time

\ ---- an error in a handler: only its source goes off; the word that ran goes on
: H-ERR ( -- )   0 FROM-CPU0 !  1 HITS +!  -77 THROW ;
' H-ERR 79 INT!
T{ STOPS-IN @  0 HITS !  ' ASK CATCH  HITS @  ROT STOPS-IN @ SWAP - -> 0 1 1 }T   \ no THROW; the message waits
T{ ASK  0 FROM-CPU0 !  HITS @  79 ON?  79 INT@ -> 1 <FALSE> ' H-ERR }T     \ source 79 off, its handler stays
' H0 79 INT!  79 INT-ON
T{ 0 HITS !  ASK  HITS @ -> 1 }T                    \ on again

\ ---- a timer: 1000 alarms per second, while MS waits
$6001F000 CONSTANT TG0              \ timer group 0: timer 0 at 0, the interrupts at $70
: TIMER ( us -- )   \ an alarm every us microseconds; the clock: the crystal / 40 = 1 MHz
  0 TG0 !  TG0 $10 + !  0 TG0 $14 + !           \ the alarm value
  0 TG0 $18 + !  0 TG0 $1C + !  1 TG0 $20 + !   \ the counter from 0
  1 TG0 $70 + BIS!                              \ its interrupt
  $E0000000 40 13 LSHIFT OR $600 OR  TG0 ! ;    \ on, up, reload; divider 40; alarm; crystal
: TIMER-OFF ( -- )   0 TG0 !  1 TG0 $70 + BIC!  1 TG0 $7C + ! ;
: TICK ( -- )   1 TG0 $7C + !  $400 TG0 BIS!  1 HITS +! ;      \ clear, the next alarm
' TICK 50 INT!  50 INT-ON
: 100MS ( -- ms )   0 HITS !  1000 TIMER  MILLIS 100 MS MILLIS SWAP -  TIMER-OFF ;
T{ 100MS 100 106 WITHIN  HITS @ 95 106 WITHIN -> TRUE TRUE }T
50 INT-OFF  79 INT-OFF  -INT

S" intr" .RESULTS
