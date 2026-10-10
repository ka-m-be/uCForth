\ asm.fs - tests of CODE words, on the board (lib/esp32/esp32s3/asm.fs)
\
\ On the board, in ucf term, after the tester and the assembler:
\   #include forth/test/tester.fs
\   #require FAIL lib/text.fs
\   #require ADD, asm/xtensa.fs
\   #include lib/esp32/asm.fs
\   #include lib/esp32/esp32s3/test/asm.fs
\ A CODE word as the handler of timer 0 of timer group 0 (source 50)
\ toggles GPIO 2 and counts; no wires.

ONLY FORTH DEFINITIONS  ALSO INTERNALS  ALSO ROM  ALSO GPIO  ALSO INTR  ALSO ASM  DECIMAL

\ ---- CODE words in the VM
CODE 3+ ( n -- n+3 )   A2 TOS@,  A2 A2 3 ADDI,  A2 TOS!,  NEXT,  END-CODE
T{ 4 3+ -> 7 }T
T{ 4 ' 3+ EXECUTE -> 7 }T
T{ ' 3+ @ -> 66 }T                                   \ the code field: DOCODE
CODE SUM3 ( a b c -- a+b+c )
  A2 POP,  A3 POP,  A2 A2 A3 ADD.N,  A3 TOS@,  A2 A2 A3 ADD.N,  A2 TOS!,  NEXT,
END-CODE
T{ 1 2 3 SUM3 -> 6 }T
CODE BIG ( -- x )   A2 $12345678 CONST,  A2 PUSH,  NEXT,  END-CODE
T{ BIG -> $12345678 }T
CODE DOWN ( n -- 0 )   \ a jump back
  A2 TOS@,
LABEL L1
  A2 A2 -1 ADDI.N,  A2 L1 BNEZ,  A2 TOS!,  NEXT,
END-CODE
T{ 1000 DOWN -> 0 }T
CODE ABS1 ( n -- u )   \ a jump forward
  A2 TOS@,  A2 L1 BGEZ,  A2 A2 NEG,
LABEL L1
  A2 TOS!,  NEXT,
END-CODE
T{ -5 ABS1  5 ABS1 -> 5 5 }T
VARIABLE V
CODE V@ ( -- x )   A2 V CONST,  A2 >CHIP,  A2 A2 0 L32I,  A2 PUSH,  NEXT,  END-CODE
T{ 42 V !  V@ -> 42 }T
: TWICE ( n -- n+6 )   3+ 3+ ;
T{ 1 TWICE -> 7 }T

\ ---- a CODE word as a handler: at once, at level 2, with CALLX0
\ TICKS is inside the chip (not in the PSRAM); TG0: timer group 0, timer
\ 0 at 0, its interrupts at $70; $60004004: GPIO_OUT_REG.
4 SRAM-BUFFER: TICKS
$6001F000 CONSTANT TG0
CODE TICK-ASM ( -- )   \ clear the interrupt, the next alarm, toggle GPIO 2, count
  A2 TG0 $7C + CONST,  A3 1 MOVI.N,  MEMW,  A3 A2 0 S32I,
  A2 TG0 CONST,  A3 A2 0 L32I,  A4 $400 MOVI,  A3 A3 A4 OR,  MEMW,  A3 A2 0 S32I,
  A2 $60004004 CONST,  A3 A2 0 L32I,  A4 4 MOVI.N,  A3 A3 A4 XOR,  MEMW,  A3 A2 0 S32I,
  A2 TICKS >CHIP CONST,  A3 A2 0 L32I,  A3 A3 1 ADDI.N,  A3 A2 0 S32I,
  RET,
END-CODE
: TIMER ( us -- )   \ an alarm every us microseconds; the clock: the crystal / 40 = 1 MHz
  0 TG0 !  TG0 $10 + !  0 TG0 $14 + !
  0 TG0 $18 + !  0 TG0 $1C + !  1 TG0 $20 + !
  1 TG0 $70 + BIS!
  $E0000000 40 13 LSHIFT OR $600 OR  TG0 ! ;
: TIMER-OFF ( -- )   0 TG0 !  1 TG0 $70 + BIC!  1 TG0 $7C + ! ;
2 OUTPUT  2 LOW
' TICK-ASM 50 INT!  50 INT-ON  +INT
T{ 50 INT@ -> ' TICK-ASM }T
: RATE ( us -- n )   0 TICKS !  TIMER  1000 MS  TIMER-OFF  TICKS @ ;
T{ 100 RATE 9990 10030 WITHIN -> TRUE }T            \ 10 000 per second (1000 MS: 1.001 s)
T{ TICKS @ 1 AND  2 PIN@ 1 AND = -> TRUE }T          \ GPIO 2 toggled each time
T{ 10 RATE 99900 100300 WITHIN -> TRUE }T           \ 100 000 per second
T{ -INT  0 TICKS !  1000 TIMER  100 MS  TIMER-OFF  TICKS @ -> 0 }T
+INT

\ ---- a CODE handler and a Forth handler: FROM_CPU 1 (source 80)
$600C0034 CONSTANT FROM-CPU1        \ SYSTEM_CPU_INTR_FROM_CPU_1_REG
VARIABLE LATER
: LATE ( -- )   0 FROM-CPU1 !  1 LATER +! ;
CODE TICK-ASK ( -- )   \ as TICK-ASM; then it asks for the Forth handler of source 80
  A2 TG0 $7C + CONST,  A3 1 MOVI.N,  MEMW,  A3 A2 0 S32I,
  A2 TG0 CONST,  A3 A2 0 L32I,  A4 $400 MOVI,  A3 A3 A4 OR,  MEMW,  A3 A2 0 S32I,
  A2 TICKS >CHIP CONST,  A3 A2 0 L32I,  A3 A3 1 ADDI.N,  A3 A2 0 S32I,
  A2 FROM-CPU1 CONST,  A3 1 MOVI.N,  MEMW,  A3 A2 0 S32I,
  RET,
END-CODE
' LATE 80 INT!  80 INT-ON
' TICK-ASK 50 INT!
T{ 0 LATER !  1000 RATE DROP  LATER @ 990 1010 WITHIN -> TRUE }T
T{ 0 50 INT!  50 INT@ -> 0 }T
50 INT-OFF  80 INT-OFF  -INT  2 INPUT

S" asm" .RESULTS
