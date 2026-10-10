\ monesp.fs - tests of the package MON-ESP32 (asm/monitor/monesp.fs)
\
\ Runs on the ESP32 and on the ESP32-S3, in ucf term, after the tester,
\ TEXT, XTENSA, ASM, MONITOR, DISXT and MON-ESP32 (asm/monitor/README.md,
\ "Test").

ONLY FORTH DEFINITIONS  ALSO INTERNALS  ALSO TEXT  ALSO ROM  ALSO CHIP  ALSO INTR
ALSO BOOTIMG.I  ALSO XTENSA  ALSO ASM  ALSO MONITOR  ALSO MON-ESP32  DECIMAL

\ Memory in cells
VARIABLE M-CELL
T{ $11223344 M-CELL !  M-CELL CELL-C@  M-CELL 3 + CELL-C@ -> $44 $11 }T

\ A function of our own, as C code has them (ENTRY, RETW): a2 = a2 + a3
\ + a4, a5 = -a5; the other registers stay
CODE M-ADD3 ( -- )   \ not a word for the VM: a function for CALL-AT
  A1 32 ENTRY,  A2 A2 A3 ADD,  A2 A2 A4 ADD,  A5 A5 NEG,  RETW,
END-CODE
T{ 1 SET-REG A2  2 SET-REG A3  3 SET-REG A4  7 SET-REG A5  9 SET-REG A6
   ' M-ADD3 >CODE CALL-AT  GET-REG A2  GET-REG A5  GET-REG A6 -> 6 -7 9 }T
T{ GET-REG PC -> ' M-ADD3 >CODE }T
T{ GET-REG CYCLES 1 200 WITHIN -> TRUE }T
T{ 1 2 3  ' M-ADD3 >CODE CALL-AT -> 1 2 3 }T          \ the stacks of the VM stay

\ A function of the ROM: esp_rom_spiflash_read ( faddr a u -- r ) reads
\ the start of the image from the flash (at $1000 on the ESP32, at 0 on
\ the ESP32-S3)
CHIP-NAME S" ESP32" S= $1000 AND CONSTANT M-IMAGE
CREATE M-BUF 16 ALLOT
T{ M-IMAGE SET-REG A2  M-BUF >CHIP SET-REG A3  16 SET-REG A4
   'FLASH-READ CALL-AT  GET-REG A2  M-BUF C@ -> 0 $E9 }T

\ An exception in a call: THROW, and LAST-FAULT has its registers
T{ 0 SET-REG A2  $10000000 SET-REG A3  4 SET-REG A4
   'FLASH-READ ' CALL-AT CATCH NIP -> -9 }T
T{ LAST-FAULT  GET-REG EXCVADDR  GET-REG PC $40000000 $40070000 WITHIN -> $10000000 TRUE }T

\ An exception in the VM: @ of an address without memory, in the primitive
\ @ (l32i a2, a2, 0)
T{ $10000000 ' @ CATCH NIP -> -9 }T
T{ LAST-FAULT  GET-REG EXCCAUSE  GET-REG EXCVADDR  GET-REG A2 -> 28 $10000000 $10000000 }T
T{ GET-REG PC DECODE DROP  TXT COUNT S" l32i a2, a2, 0" S= -> TRUE }T

\ The special registers
T{ PS SR@ $40000 AND -> $40000 }T                       \ WOE: the register window is on
T{ CCOUNT SR@  CCOUNT SR@  U< -> TRUE }T
T{ $12345678 MISC0 SR!  MISC0 SR@ -> $12345678 }T
T{ 0 MISC0 SR!  MISC0 SR@ -> 0 }T

\ The disassembler reads the ROM: an instruction there (on the ESP32 the
\ ENTRY of the function, on the ESP32-S3 the L32R of a jump to it)
T{ 'FLASH-READ DECODE 'FLASH-READ -  TXT COUNT DROP 6 S" .byte " S= -> 3 FALSE }T

\ Single steps: a function of our own, a2 = a2 + 3 (ENTRY at 0, ADDI at
\ 3, 6 and 9, RETW at 12)
CODE M-INC3 ( -- )   \ not a word for the VM: a function
  A1 32 ENTRY,  A2 A2 1 ADDI,  A2 A2 1 ADDI,  A2 A2 1 ADDI,  RETW,
END-CODE
T{ 5 SET-REG A2  ' M-INC3 >CODE DEBUG-AT  STEP  GET-REG PC ' M-INC3 >CODE -  STOPPED  GET-REG A2 -> 3 1 5 }T
T{ STEP  GET-REG PC ' M-INC3 >CODE -  GET-REG A2 -> 6 6 }T
T{ STEP STEP  GET-REG PC ' M-INC3 >CODE -  GET-REG A2 -> 12 8 }T
T{ STEP  STOPPED  GET-REG A2 -> 0 8 }T                \ RETW: the end; a2 as the function left it
T{ ' STEP CATCH -> -2 }T                              \ no function waits
\ SET-REG while it waits; Forth runs between the steps
T{ 5 SET-REG A2  ' M-INC3 >CODE DEBUG-AT  STEP  100 SET-REG A2  3 4 +  STEP  GET-REG A2 -> 7 101 }T
T{ GO  STOPPED  GET-REG A2 -> 0 103 }T
\ A breakpoint at the third ADDI; GO from there goes on to the end
T{ 5 SET-REG A2  ' M-INC3 >CODE DEBUG-AT  ' M-INC3 >CODE 9 + BREAK-AT
   GO  STOPPED  GET-REG PC ' M-INC3 >CODE -  GET-REG A2 -> 3 9 7 }T
T{ GO  STOPPED  GET-REG A2 -> 0 8 }T
CLEAR-BREAKS
\ A BREAK instruction in the code: GO stops at it, and goes on after it
CODE M-BRK ( -- )   \ not a word for the VM: a function
  A1 32 ENTRY,  A2 A2 1 ADDI,  1 15 BREAK,  A2 A2 1 ADDI,  RETW,
END-CODE
T{ 0 SET-REG A2  ' M-BRK >CODE DEBUG-AT  GO  STOPPED  GET-REG PC ' M-BRK >CODE -  GET-REG A2 -> 3 6 1 }T
T{ GO  STOPPED  GET-REG A2 -> 0 2 }T
\ The stacks of the VM stay; afterwards no debug register is on
T{ 1 2 3  ' M-INC3 >CODE DEBUG-AT  STEP STEP GO -> 1 2 3 }T
T{ ICOUNTLEVEL SR@  IBREAKENABLE SR@  DEBUG-HOOK @ -> 0 0 0 }T
\ A function of the ROM, a step, then to its end: it reads the flash as before
T{ M-IMAGE SET-REG A2  M-BUF >CHIP SET-REG A3  16 SET-REG A4  0 M-BUF C!
   'FLASH-READ DEBUG-AT  STEP  STOPPED  GET-REG PC 'FLASH-READ U> -> 1 TRUE }T
T{ GO  STOPPED  GET-REG A2  M-BUF C@ -> 0 0 $E9 }T

CR .REGS
S" monesp" .RESULTS
