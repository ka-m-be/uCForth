\ bios.fs - tests of the package BIOS (lib/pc/bios.fs), only on the PC
\
\ The image for the PC has the package (targets/pc-bios/README.md); the
\ PC has no files. So a terminal sends this file after the tester, for
\ example targets/pc-bios/test.fs in QEMU.

ONLY FORTH DEFINITIONS  ALSO BIOS
DECIMAL

\ INT 12h: the memory below 640 KB, in KB
T{ 0 $12 BIOS DROP 128 641 WITHIN -> TRUE }T
T{ 0 $12 SYSCALL  REGS @ = -> TRUE }T

\ INT 1Ah: the clock of the BIOS (CX DX ticks since midnight, 18.2 a
\ second); it goes on while MS waits
: TICKS ( -- u )   0 1 $1A BIOS 2DROP  REGS 6 + @ ;
T{ TICKS 200 MS TICKS SWAP - 2 6 WITHIN -> TRUE }T

\ INT 10h, 0Fh: the mode of the screen (3: text, 80 columns; 7: mono)
T{ $0F00 1 $10 BIOS DROP 127 AND  DUP 3 = SWAP 7 = OR -> TRUE }T
\ INT 16h, 01h: no key waits (the flag Z, bit 6)
T{ $0100 1 $16 BIOS 2DROP  REGS 12 + @ 64 AND 0<> -> TRUE }T

\ SYSCALL takes at most 6 numbers
: SEVEN ( -- )   1 2 3 4 5 6 7  7 $12 SYSCALL ;
T{ ' SEVEN CATCH -> -24 }T

\ Ports: the mask of the interrupt controller (21h), back as it was; a
\ word from 20h has the byte of 21h in its upper half. IRQ 0 (the timer)
\ and IRQ 4 (COM1) are on.
T{ $21 PC@ DUP $21 PC!  $21 PC@ = -> TRUE }T
T{ $20 PW@ 8 RSHIFT  $21 PC@ = -> TRUE }T
T{ $21 PC@ $11 AND -> 0 }T
\ The timer, channel 0: it counts from 1193 down (1 ms); the command 0
\ at port 43h holds its count for two reads at port 40h
: COUNT0 ( -- u )   0 $43 PC!  $40 PC@  $40 PC@ 8 LSHIFT OR ;
T{ COUNT0 1194 U< -> TRUE }T

ONLY FORTH DEFINITIONS
S" bios" .RESULTS
