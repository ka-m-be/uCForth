\ intr.fs - tests of the package INTR on the ESP32, on the board (lib/esp32/intr.fs)
\
\ On the board, in ucf term, after the tester:
\   #include forth/test/tester.fs
\   #include lib/esp32/esp32/test/intr.fs
\ The image must have INTR (targets/esp32/README.md, "Build"). The
\ sources need no wires: FROM_CPU 0 (source 24) asks while a register of
\ the CPU is 1; timer 1 of timer group 0 (source 15) gives an alarm
\ every n us (timer 0 is the clock of the HAL).

ONLY FORTH DEFINITIONS  ALSO INTERNALS  ALSO ROM  ALSO CHIP  ALSO INTR  ALSO INTR.I  DECIMAL

\ ---- exceptions of the CPU: THROW, and the VM goes on
T{ $10000000 ' @ CATCH NIP -> -9 }T               \ LoadProhibited
T{ 0 $10000000 ' ! CATCH NIP NIP -> -9 }T          \ StoreProhibited
T{ $50000000 ' EXECUTE CATCH NIP -> -9 }T          \ an xt without memory
: IN-ROM ( -- )   0 $10000000 4  3 $40062ED8 SYSCALL DROP ;   \ esp_rom_spiflash_read to it
T{ ' IN-ROM CATCH -> -9 }T                         \ in a function of the ROM
T{ 0 $4000855C SYSCALL 0 > -> TRUE }T              \ the ROM again: ets_get_cpu_frequency
T{ 1 2 + -> 3 }T

\ ---- a handler for FROM_CPU 0
$3FF000DC CONSTANT FROM-CPU0        \ DPORT_CPU_INTR_FROM_CPU_0_REG: 1, and source 24 asks
VARIABLE HITS
: H0 ( -- )   0 FROM-CPU0 !  1 HITS +! ;
: ASK ( -- )   1 FROM-CPU0 !  10 US ;
' H0 24 INT!  24 INT-ON  +INT
T{ 24 INT@ -> ' H0 }T
T{ 0 HITS !  ASK  HITS @ -> 1 }T
T{ 0 HITS !  ASK ASK ASK  HITS @ -> 3 }T
T{ 0 HITS !  1 FROM-CPU0 ! 2 3 4  10 US  HITS @ -> 2 3 4 1 }T   \ the stacks stay

\ ---- off: a source, all handlers
T{ 24 INT-OFF  0 HITS !  ASK  0 FROM-CPU0 !  HITS @ -> 0 }T
T{ 24 INT-ON  0 HITS !  ASK  HITS @ -> 1 }T
T{ -INT  0 HITS !  ASK  HITS @ -> 0 }T
T{ +INT  10 US  HITS @ -> 1 }T                      \ the source asked all the time

\ ---- an error in a handler: only its source goes off; the word that ran goes on
: H-ERR ( -- )   0 FROM-CPU0 !  1 HITS +!  -77 THROW ;
' H-ERR 24 INT!
T{ STOPS-IN @  0 HITS !  ' ASK CATCH  HITS @  ROT STOPS-IN @ SWAP - -> 0 1 1 }T   \ no THROW; the message waits
T{ ASK  0 FROM-CPU0 !  HITS @  24 ON?  24 INT@ -> 1 <FALSE> ' H-ERR }T     \ source 24 off, its handler stays
' H0 24 INT!  24 INT-ON
T{ 0 HITS !  ASK  HITS @ -> 1 }T                    \ on again

\ ---- a timer: 1000 alarms per second, while MS waits
$3FF5F024 CONSTANT T1               \ timer 1 of timer group 0
$3FF5F000 CONSTANT TG0              \ its interrupts: ENA at $98, CLR at $A4, bit 1
: TIMER ( us -- )   \ an alarm every us microseconds, at 1 MHz (TG-1MHZ of the layer of the chip)
  0 T1 !  T1 $10 + !  0 T1 $14 + !              \ the alarm value
  0 T1 $18 + !  0 T1 $1C + !  1 T1 $20 + !      \ the counter from 0
  2 TG0 $98 + BIS!                              \ its interrupt
  TG-1MHZ T1 ! ;                                \ on, up, reload, alarm, by level
: TIMER-OFF ( -- )   0 T1 !  2 TG0 $98 + BIC!  2 TG0 $A4 + ! ;
: TICK ( -- )   2 TG0 $A4 + !  $400 T1 BIS!  1 HITS +! ;       \ clear, the next alarm
' TICK 15 INT!  15 INT-ON
: 100MS ( -- ms )   0 HITS !  1000 TIMER  MILLIS 100 MS MILLIS SWAP -  TIMER-OFF ;
T{ 100MS 100 106 WITHIN  HITS @ 95 106 WITHIN -> TRUE TRUE }T
15 INT-OFF  24 INT-OFF  -INT

S" intr" .RESULTS
