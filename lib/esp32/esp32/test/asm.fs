\ asm.fs - tests of CODE words on the ESP32, on the board (lib/esp32/esp32/asm.fs)
\
\ On the board, in ucf term, after the tester and the assembler:
\   #include forth/test/tester.fs
\   #require FAIL lib/text.fs
\   #require ADD, asm/xtensa.fs
\   #include lib/esp32/asm.fs
\   #include lib/esp32/esp32/test/asm.fs
\ A CODE word as the handler of timer 1 of timer group 0 (source 15)
\ toggles GPIO 25 and counts; no wires (GPIO 25 is free on the board;
\ timer 0 is the clock of the HAL).

ONLY FORTH DEFINITIONS  ALSO INTERNALS  ALSO ROM  ALSO CHIP  ALSO GPIO  ALSO INTR
ALSO BOOTIMG  ALSO ASM  DECIMAL

\ ---- CODE words in the VM; their code is in SRAM0
CODE 3+ ( n -- n+3 )   A2 TOS@,  A2 A2 3 ADDI,  A2 TOS!,  NEXT,  END-CODE
T{ 4 3+ -> 7 }T
T{ 4 ' 3+ EXECUTE -> 7 }T
T{ ' 3+ @ -> 66 }T                                   \ the code field: DOCODE
T{ ' 3+ >CODE  CODE-START $400A0000 WITHIN -> TRUE }T
T{ ' 3+ >CODE 4 +  CODE-HERE @ U< -> TRUE }T
CODE SUM3 ( a b c -- a+b+c )
  A2 POP,  A3 POP,  A2 A2 A3 ADD.N,  A3 TOS@,  A2 A2 A3 ADD.N,  A2 TOS!,  NEXT,
END-CODE
T{ 1 2 3 SUM3 -> 6 }T
T{ ' SUM3 >CODE  ' 3+ >CODE U> -> TRUE }T            \ after the code of 3+
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
\ TICKS is a cell inside the chip; T1: timer 1 of timer group 0, its
\ interrupts at $98 (enable) and $A4 (clear), bit 1; $3FF44004:
\ GPIO_OUT_REG.
4 SRAM-BUFFER: TICKS
$3FF5F024 CONSTANT T1
$3FF5F000 CONSTANT TG0
CODE TICK-ASM ( -- )   \ clear the interrupt, the next alarm, toggle GPIO 25, count
  A2 TG0 $A4 + CONST,  A3 2 MOVI.N,  MEMW,  A3 A2 0 S32I,
  A2 T1 CONST,  A3 A2 0 L32I,  A4 $400 MOVI,  A3 A3 A4 OR,  MEMW,  A3 A2 0 S32I,
  A2 $3FF44004 CONST,  A3 A2 0 L32I,  A4 $2000000 CONST,  A3 A3 A4 XOR,  MEMW,  A3 A2 0 S32I,
  A2 TICKS >CHIP CONST,  A3 A2 0 L32I,  A3 A3 1 ADDI.N,  A3 A2 0 S32I,
  RET,
END-CODE
: TIMER ( us -- )   \ an alarm every us microseconds, at 1 MHz (TG-1MHZ of the layer of the chip)
  0 T1 !  T1 $10 + !  0 T1 $14 + !
  0 T1 $18 + !  0 T1 $1C + !  1 T1 $20 + !
  2 TG0 $98 + BIS!
  TG-1MHZ T1 ! ;
: TIMER-OFF ( -- )   0 T1 !  2 TG0 $98 + BIC!  2 TG0 $A4 + ! ;
25 OUTPUT  25 LOW
' TICK-ASM 15 INT!  15 INT-ON  +INT
T{ 15 INT@ -> ' TICK-ASM }T
: RATE ( us -- n )   0 TICKS !  TIMER  1000 MS  TIMER-OFF  TICKS @ ;
T{ 100 RATE 9990 10030 WITHIN -> TRUE }T            \ 10 000 per second
T{ TICKS @ 1 AND  25 PIN@ 1 AND = -> TRUE }T         \ GPIO 25 toggled each time
T{ 10 RATE 99900 100300 WITHIN -> TRUE }T           \ 100 000 per second
T{ -INT  0 TICKS !  1000 TIMER  100 MS  TIMER-OFF  TICKS @ -> 0 }T
+INT

\ ---- a CODE handler and a Forth handler: FROM_CPU 1 (source 25)
$3FF000E0 CONSTANT FROM-CPU1        \ DPORT_CPU_INTR_FROM_CPU_1_REG
VARIABLE LATER
: LATE ( -- )   0 FROM-CPU1 !  1 LATER +! ;
CODE TICK-ASK ( -- )   \ as TICK-ASM; then it asks for the Forth handler of source 25
  A2 TG0 $A4 + CONST,  A3 2 MOVI.N,  MEMW,  A3 A2 0 S32I,
  A2 T1 CONST,  A3 A2 0 L32I,  A4 $400 MOVI,  A3 A3 A4 OR,  MEMW,  A3 A2 0 S32I,
  A2 TICKS >CHIP CONST,  A3 A2 0 L32I,  A3 A3 1 ADDI.N,  A3 A2 0 S32I,
  A2 FROM-CPU1 CONST,  A3 1 MOVI.N,  MEMW,  A3 A2 0 S32I,
  RET,
END-CODE
' LATE 25 INT!  25 INT-ON
' TICK-ASK 15 INT!
T{ 0 LATER !  1000 RATE DROP  LATER @ 990 1010 WITHIN -> TRUE }T
T{ 0 15 INT!  15 INT@ -> 0 }T
15 INT-OFF  25 INT-OFF  -INT  25 INPUT

S" asm" .RESULTS
